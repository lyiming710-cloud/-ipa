using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewGarCobCannonOccupiedLandingRuntimeTest.cs")]
public class BugOverviewGarCobCannonOccupiedLandingRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindSpawnedVase = "FindSpawnedVase";

		public static readonly StringName HasGarCobAtGrid = "HasGarCobAtGrid";

		public static readonly StringName CreateGrid = "CreateGrid";

		public static readonly StringName RegisterFixtures = "RegisterFixtures";

		public static readonly StringName UnregisterFixtures = "UnregisterFixtures";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _control = "_control";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string MapControlScenePath = "res://Registry/Battle/Feature/Map/Control/TowerDefenseMapControl.tscn";

	private const string GarCobProjectilePath = "res://Registry/Projectile/Config/CannonCob/GarCobCannonCob.tres";

	private const string GarCobPacketPath = "res://Asset/Anime/Character/Plant/Gold/GarCobCannon/Packet/PlantGarCobCannon.tres";

	private const string GarCobScenePath = "res://Asset/Anime/Character/Plant/Gold/GarCobCannon/Scene/TowerDefensePlantGarCobCannon.tscn";

	private const string VasePacketPath = "res://Asset/Anime/Character/Vase/Zombie/Packet/VaseZombie.tres";

	private const string VaseScenePath = "res://Asset/Anime/Character/Vase/Zombie/Scene/TowerDefenseVaseZombie.tscn";

	private const string ContentPacketPath = "res://Asset/Anime/Character/Zombie/Boss/Boss/Packet/ZombieBoss.tres";

	private static readonly string[] GarCobContentKeys = new string[5] { "ZombieGargantuar", "ZombieGargantuarRedEyes", "ZombieFootballGargantuar", "ZombieFootballGargantuarBlack", "ZombieDiscoGargantuar" };

	private int _checks;

	private int _failures;

	private TowerDefenseControlNew _control;

	public override async void _Ready()
	{
		try
		{
			_ = 3;
			try
			{
				await SetupBattleFixture();
				await VerifyEmptyCellLandingStillCreatesVase();
				await VerifyOccupiedGarCobLandingCreatesVase();
				await VerifyExtendGridNoLimitBoundarySafety();
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[GarCobOccupiedLanding] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
			{
				TowerDefenseManager.Instance.currentControl = null;
			}
			if (GodotObject.IsInstanceValid(_control))
			{
				_control.QueueFree();
			}
			UnregisterFixtures();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0;
		GD.Print($"BUG_OVERVIEW_I106_GAR_COB_OCCUPIED_LANDING_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task SetupBattleFixture()
	{
		RegisterFixtures();
		_control = new BugOverviewI106GarCobControlStub
		{
			Name = "GarCobOccupiedLandingControl",
			isGameRunning = true,
			isInit = true,
			levelConfig = new TowerDefenseLevelConfig()
		};
		AddChild(_control, forceReadableName: false, InternalMode.Disabled);
		_control.characterNode = new Node2D
		{
			Name = "CharacterNode"
		};
		_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
		_control.levelControl = new BugOverviewI106GarCobLevelControlStub
		{
			Name = "LevelControl"
		};
		_control.AddChild(_control.levelControl, forceReadableName: false, InternalMode.Disabled);
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			throw new InvalidOperationException("TowerDefenseManager autoload is unavailable.");
		}
		instance.currentControl = _control;
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			gridNum = new Vector2I(9, 5),
			gridBeginPos = new Vector2(0f, 100f),
			gridSize = new Vector2(100f, 76f),
			plantOffset = 50.0
		};
		instance.gridNum = towerDefenseMapConfig.gridNum;
		instance.gridBeginPos = towerDefenseMapConfig.gridBeginPos;
		instance.gridSize = towerDefenseMapConfig.gridSize;
		TowerDefenseMapControl towerDefenseMapControl = ResourceLoader.Load<PackedScene>("res://Registry/Battle/Feature/Map/Control/TowerDefenseMapControl.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseMapControl>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(towerDefenseMapControl))
		{
			throw new InvalidOperationException("The real map control did not instantiate.");
		}
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (towerDefenseMapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			control = _control,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig,
			mapControl = towerDefenseMapControl
		});
		_control.featureDictionary["Map"] = towerDefenseBattleFeatureMap;
		CreateGrid(towerDefenseBattleFeatureMap, towerDefenseMapConfig.gridNum);
		_control.AddChild(towerDefenseMapControl, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}

	private async Task VerifyEmptyCellLandingStillCreatesVase()
	{
		Vector2I gridPos = new Vector2I(3, 3);
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
		Check(GodotObject.IsInstanceValid(mapCell), "The empty-cell scenario must use a real map cell.");
		Check((await SpawnLandingVase(gridPos)) is TowerDefenseVaseZombie, "A Gar-cob landing on an empty cell must still create a Zombie Vase.");
	}

	private async Task VerifyOccupiedGarCobLandingCreatesVase()
	{
		Vector2I gridPos = new Vector2I(5, 3);
		TowerDefenseCellInstance cell = TowerDefenseManager.GetMapCell(gridPos);
		Check(GodotObject.IsInstanceValid(cell), "The occupied-cell scenario must use a real map cell.");
		TowerDefensePacketConfig garCobPacket = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Plant/Gold/GarCobCannon/Packet/PlantGarCobCannon.tres", null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(garCobPacket), "The canonical GarCob packet must load.");
		Check(GodotObject.IsInstanceValid(cell) && cell.CanPacketPlant(garCobPacket), "The real map cell must accept the GarCob plant before occupation.");
		TowerDefenseCharacter garCob = garCobPacket?.Plant(gridPos, playAudio: false);
		Check(garCob is TowerDefensePlantGarCobCannon, "The occupied landing cell must contain a real GarCob plant.");
		for (int frame = 0; frame < 4; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		Check(GodotObject.IsInstanceValid(garCob) && GodotObject.IsInstanceValid(cell) && cell.GetCharacterList().Contains(garCob), "The GarCob plant must occupy the landing cell's ground slot.");
		TowerDefensePacketConfig towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Vase/Zombie/Packet/VaseZombie.tres", null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(towerDefensePacketConfig) && GodotObject.IsInstanceValid(cell) && !cell.CanPacketPlant(towerDefensePacketConfig), "Normal placement rules must reject the Zombie Vase on the occupied GarCob cell.");
		TowerDefenseCharacter instance = towerDefensePacketConfig?.Plant(gridPos, playAudio: false);
		Check(!GodotObject.IsInstanceValid(instance), "Plant with noLimit=false must preserve the occupied-cell rejection.");
		TowerDefenseCharacter instance2 = towerDefensePacketConfig?.Plant(gridPos, playAudio: false, noLimit: true);
		Check(!GodotObject.IsInstanceValid(instance2), "Plant with noLimit=true must still preserve terrain and occupied-cell validation unless placement checks are explicitly skipped.");
		Check((await SpawnLandingVase(gridPos)) is TowerDefenseVaseZombie, "A Gar-cob landing on its occupied origin cell must create a Zombie Vase.");
		Check(GodotObject.IsInstanceValid(garCob) && GodotObject.IsInstanceValid(cell) && cell.GetCharacterList().Contains(garCob), "Spawning the vase without placement limits must retain the original GarCob plant.");
		TowerDefenseCharacter forcedGarCob = garCobPacket.Plant(gridPos, playAudio: false, noLimit: true, default, skipPlacementCheck: true);
		Check(forcedGarCob is TowerDefensePlantGarCobCannon && forcedGarCob != garCob, "A valid occupied footprint must allow a real extend-grid GarCob to be force-spawned.");
		for (int frame = 0; frame < 4; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		Check(GodotObject.IsInstanceValid(forcedGarCob) && cell.GetCharacterList().Contains(forcedGarCob), "The valid no-limit GarCob must finish planting on the occupied cell.");
	}

	private async Task VerifyExtendGridNoLimitBoundarySafety()
	{
		Vector2I boundaryGridPos = new Vector2I(9, 4);
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(boundaryGridPos);
		Check(GodotObject.IsInstanceValid(mapCell), "The extend-grid boundary scenario must start from a real rightmost map cell.");
		TowerDefenseCharacter instance = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Plant/Gold/GarCobCannon/Packet/PlantGarCobCannon.tres", null, ResourceLoader.CacheMode.Ignore)?.Plant(boundaryGridPos, playAudio: false, noLimit: true, default, skipPlacementCheck: true);
		Check(!GodotObject.IsInstanceValid(instance), "A no-limit extend-grid plant whose footprint leaves the map must return null.");
		for (int frame = 0; frame < 4; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		Check(!HasGarCobAtGrid(boundaryGridPos), "A rejected right-boundary GarCob must not be added on a later deferred frame.");
	}

	private async Task<TowerDefenseVase> SpawnLandingVase(Vector2I gridPos)
	{
		TowerDefenseProjectileData towerDefenseProjectileData = ResourceLoader.Load<TowerDefenseProjectileData>("res://Registry/Projectile/Config/CannonCob/GarCobCannonCob.tres", null, ResourceLoader.CacheMode.Ignore);
		Check(towerDefenseProjectileData?.hitEffect != null, "The canonical GarCobCannonCob must retain its real landing effect.");
		TowerDefenseProjectileEffectBase towerDefenseProjectileEffectBase = towerDefenseProjectileData?.hitEffect?.Instantiate<TowerDefenseProjectileEffectBase>(PackedScene.GenEditState.Disabled);
		Check(towerDefenseProjectileEffectBase is TowerDefenseProjectileEffectGarCobCannonExplode, "GarCobCannonCob must instantiate the production vase-spawning effect.");
		if (!GodotObject.IsInstanceValid(towerDefenseProjectileEffectBase))
		{
			return null;
		}
		towerDefenseProjectileEffectBase.Init(gridPos, TowerDefenseEnum.CHARACTER_CAMP.PLANT, 0, null);
		towerDefenseProjectileEffectBase.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(gridPos);
		_control.characterNode.AddChild(towerDefenseProjectileEffectBase, forceReadableName: false, InternalMode.Disabled);
		TowerDefenseVase towerDefenseVase = null;
		for (int frame = 0; frame < 20; frame++)
		{
			if (GodotObject.IsInstanceValid(towerDefenseVase))
			{
				break;
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			towerDefenseVase = FindSpawnedVase(gridPos);
		}
		return towerDefenseVase;
	}

	private TowerDefenseVase FindSpawnedVase(Vector2I gridPos)
	{
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefenseVase towerDefenseVase && GodotObject.IsInstanceValid(towerDefenseVase) && towerDefenseVase.gridPos == gridPos)
			{
				return towerDefenseVase;
			}
		}
		return null;
	}

	private bool HasGarCobAtGrid(Vector2I gridPos)
	{
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefensePlantGarCobCannon towerDefensePlantGarCobCannon && GodotObject.IsInstanceValid(towerDefensePlantGarCobCannon) && towerDefensePlantGarCobCannon.gridPos == gridPos)
			{
				return true;
			}
		}
		return false;
	}

	private static void CreateGrid(TowerDefenseBattleFeatureMap mapFeature, Vector2I gridNum)
	{
		TowerDefenseCellConfig config = new TowerDefenseCellConfig();
		mapFeature.plantGrid.Clear();
		for (int i = 0; i <= gridNum.X; i++)
		{
			Godot.Collections.Array array = new Godot.Collections.Array();
			for (int j = 0; j <= gridNum.Y; j++)
			{
				if (i == 0 || j == 0)
				{
					array.Add(default);
					continue;
				}
				TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
				{
					gridPos = new Vector2I(i, j)
				};
				towerDefenseCellInstance.Init(config);
				array.Add(towerDefenseCellInstance);
			}
			mapFeature.plantGrid.Add(array);
		}
		mapFeature.iceCapList.Clear();
		mapFeature.lineUse.Clear();
		for (int k = 0; k <= gridNum.Y; k++)
		{
			mapFeature.iceCapList.Add(default);
			mapFeature.lineUse.Add(k > 0);
		}
	}

	private static void RegisterFixtures()
	{
		ResourceManager instance = ResourceManager.Instance;
		TowerDefensePacketConfig value = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Vase/Zombie/Packet/VaseZombie.tres", null, ResourceLoader.CacheMode.Ignore);
		PackedScene value2 = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Vase/Zombie/Scene/TowerDefenseVaseZombie.tscn", null, ResourceLoader.CacheMode.Ignore);
		TowerDefensePacketConfig value3 = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Plant/Gold/GarCobCannon/Packet/PlantGarCobCannon.tres", null, ResourceLoader.CacheMode.Ignore);
		PackedScene value4 = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Plant/Gold/GarCobCannon/Scene/TowerDefensePlantGarCobCannon.tscn", null, ResourceLoader.CacheMode.Ignore);
		TowerDefensePacketConfig value5 = ResourceLoader.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Zombie/Boss/Boss/Packet/ZombieBoss.tres", null, ResourceLoader.CacheMode.Ignore);
		instance.TOWERDEFENSE_PACKETS["VaseZombie"] = value;
		instance.TOWERDEFENSE_CHARCATERS["VaseZombie"] = value2;
		instance.TOWERDEFENSE_PACKETS["PlantGarCobCannon"] = value3;
		instance.TOWERDEFENSE_CHARCATERS["PlantGarCobCannon"] = value4;
		string[] garCobContentKeys = GarCobContentKeys;
		foreach (string key in garCobContentKeys)
		{
			instance.TOWERDEFENSE_PACKETS[key] = value5;
		}
	}

	private static void UnregisterFixtures()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			instance.TOWERDEFENSE_PACKETS.Remove("VaseZombie");
			instance.TOWERDEFENSE_CHARCATERS.Remove("VaseZombie");
			instance.TOWERDEFENSE_PACKETS.Remove("PlantGarCobCannon");
			instance.TOWERDEFENSE_CHARCATERS.Remove("PlantGarCobCannon");
			string[] garCobContentKeys = GarCobContentKeys;
			foreach (string key in garCobContentKeys)
			{
				instance.TOWERDEFENSE_PACKETS.Remove(key);
			}
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[GarCobOccupiedLanding] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindSpawnedVase, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasGarCobAtGrid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateGrid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapFeature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.UnregisterFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.FindSpawnedVase && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseVase>(FindSpawnedVase(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.HasGarCobAtGrid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasGarCobAtGrid(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateGrid && args.Count == 2)
		{
			CreateGrid(VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterFixtures && args.Count == 0)
		{
			RegisterFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.UnregisterFixtures && args.Count == 0)
		{
			UnregisterFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateGrid && args.Count == 2)
		{
			CreateGrid(VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterFixtures && args.Count == 0)
		{
			RegisterFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.UnregisterFixtures && args.Count == 0)
		{
			UnregisterFixtures();
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.FindSpawnedVase)
		{
			return true;
		}
		if (method == MethodName.HasGarCobAtGrid)
		{
			return true;
		}
		if (method == MethodName.CreateGrid)
		{
			return true;
		}
		if (method == MethodName.RegisterFixtures)
		{
			return true;
		}
		if (method == MethodName.UnregisterFixtures)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<TowerDefenseControlNew>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		if (name == PropertyName._control)
		{
			value = VariantUtils.CreateFrom(in _control);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value2))
		{
			_failures = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._control, out var value3))
		{
			_control = value3.As<TowerDefenseControlNew>();
		}
	}
}
