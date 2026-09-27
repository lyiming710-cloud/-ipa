using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentTanglekelpFamilyRuntimeTest.cs")]
public class BugDepartmentTanglekelpFamilyRuntimeTest : Node
{
	private readonly record struct DragScenario(string Key, string PacketPath, string ScenePath, Vector2I Grid, bool HypnotizesTarget);

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName RegisterRealFixtures = "RegisterRealFixtures";

		public static readonly StringName RegisterPacket = "RegisterPacket";

		public static readonly StringName RegisterCharacter = "RegisterCharacter";

		public static readonly StringName RestoreRealFixtures = "RestoreRealFixtures";

		public static readonly StringName Check = "Check";

		public static readonly StringName Require = "Require";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string ResultMarker = "BUG_DEPARTMENT_TANGLEKELP_FAMILY_RESULT";

	private const string ZombieKey = "ZombieNormal";

	private const string ZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const string PlanternKey = "PlantPlanternTanglekelp";

	private const string PlanternPacketPath = "res://Asset/Anime/Character/Plant/Chapter4/PlanternTanglekelp/Packet/PlantPlanternTanglekelp.tres";

	private const string PlanternScenePath = "res://Asset/Anime/Character/Plant/Chapter4/PlanternTanglekelp/Scene/TowerDefensePlantPlanternTanglekelp.tscn";

	private const string CraterDayGroundKey = "CraterDayGround";

	private const string CraterDayGroundPacketPath = "res://Asset/Anime/Character/Crater/CraterDayGround/Packet/CraterDayGround.tres";

	private const string CraterDayGroundScenePath = "res://Asset/Anime/Character/Crater/CraterDayGround/Scene/TowerDefenseCraterDayGround.tscn";

	private static readonly DragScenario[] DragScenarios = new DragScenario[4]
	{
		new DragScenario("PlantInsaniKelp", "res://Asset/Anime/Character/Plant/Star/InsaniKelp/Packet/PlantInsaniKelp.tres", "res://Asset/Anime/Character/Plant/Star/InsaniKelp/Scene/TowerDefensePlantInsaniKelp.tscn", new Vector2I(4, 1), HypnotizesTarget: false),
		new DragScenario("PlantTanglekelp", "res://Asset/Anime/Character/Plant/Chapter0/Tanglekelp/Packet/PlantTanglekelp.tres", "res://Asset/Anime/Character/Plant/Chapter0/Tanglekelp/Scene/TowerDefensePlantTanglekelp.tscn", new Vector2I(4, 2), HypnotizesTarget: false),
		new DragScenario("PlantTanglekelpH", "res://Asset/Anime/Character/Plant/Chapter3/TanglekelpH/Packet/PlantTanglekelpH.tres", "res://Asset/Anime/Character/Plant/Chapter3/TanglekelpH/Scene/TowerDefensePlantTanglekelpH.tscn", new Vector2I(4, 3), HypnotizesTarget: true),
		new DragScenario("PlantDoomTanglekelp", "res://Asset/Anime/Character/Plant/Chapter3/DoomTanglekelp/Packet/PlantDoomTanglekelp.tres", "res://Asset/Anime/Character/Plant/Chapter3/DoomTanglekelp/Scene/TowerDefensePlantDoomTanglekelp.tscn", new Vector2I(4, 4), HypnotizesTarget: false)
	};

	private int _checks;

	private int _failures;

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		BugDepartmentTanglekelpFamilyRuntimeControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				Require(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ResourceManager.Instance), "Required runtime autoloads are unavailable.");
				RegisterRealFixtures();
				control = new BugDepartmentTanglekelpFamilyRuntimeControlStub
				{
					Name = "BugDepartmentTanglekelpFamilyRuntimeControl",
					isGameRunning = false,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				Node2D node2D = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = node2D;
				manager.currentControl = control;
				manager.gridBeginPos = Vector2.Zero;
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapFeature = CreateMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				DragScenario[] dragScenarios = DragScenarios;
				foreach (DragScenario scenario in dragScenarios)
				{
					await VerifyActiveDragScenario(control, scenario);
				}
				await VerifyPlanternDeathDrag(control, new Vector2I(4, 5));
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[{"BugDepartmentTanglekelpFamilyRuntimeTest"}] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			mapFeature?.Destroy();
			if (GodotObject.IsInstanceValid(mapControl))
			{
				mapControl.Free();
			}
			if (GodotObject.IsInstanceValid(control))
			{
				control.QueueFree();
			}
			RestoreRealFixtures();
			await WaitFrames(4);
		}
		bool flag = _checks == 58 && _failures == 0;
		GD.Print($"{"BUG_DEPARTMENT_TANGLEKELP_FAMILY_RESULT"} passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyActiveDragScenario(BugDepartmentTanglekelpFamilyRuntimeControlStub control, DragScenario scenario)
	{
		control.isGameRunning = false;
		TowerDefenseCharacter plant = LoadPacket(scenario.PacketPath)?.Plant(scenario.Grid, playAudio: false);
		await WaitFrames(75);
		TowerDefenseZombieNormal zombie = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres")?.Plant(scenario.Grid, playAudio: false) as TowerDefenseZombieNormal;
		await WaitFrames(6);
		Check(GodotObject.IsInstanceValid(plant) && plant.config?.name == scenario.Key && plant.SceneFilePath == scenario.ScenePath, "The real " + scenario.Key + " role scene must spawn on water.");
		Check(GodotObject.IsInstanceValid(zombie) && zombie.config?.name == "ZombieNormal" && zombie.SceneFilePath == "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn", scenario.Key + " must face a real ZombieNormal target.");
		Require(GodotObject.IsInstanceValid(plant) && GodotObject.IsInstanceValid(zombie), "The real " + scenario.Key + " encounter did not spawn.");
		zombie.ProcessMode = ProcessModeEnum.Disabled;
		plant.inGame = true;
		zombie.inGame = true;
		zombie.gridPos = scenario.Grid;
		zombie.GlobalPosition = plant.GlobalPosition + new Vector2(28f, 0f);
		await WaitFrames(2);
		TanglekelpComponent runtime = plant.componentManager.GetRuntime<TanglekelpComponent>();
		AttackComponent attack = plant.componentManager.GetRuntime<AttackComponent>("character.attack.0");
		Check(runtime != null && !runtime.IsReleased && runtime.Alive, scenario.Key + " must expose an active real Tanglekelp runtime.");
		Check(attack != null && !attack.IsReleased, scenario.Key + " must expose its real Attack runtime.");
		Check(runtime?.attackComponent == attack, scenario.Key + " must rebind Tanglekelp to Attack after declaration-order activation.");
		Check(!plant.componentRunning, scenario.Key + " must finish its planting animation before acquiring a target.");
		Check(zombie.config?.canDragIntoWater ?? false, "The real ZombieNormal target for " + scenario.Key + " must allow a lethal water drag.");
		Require(runtime != null && !runtime.IsReleased && attack != null && !attack.IsReleased, scenario.Key + " component runtimes are unavailable.");
		bool dragBegan = false;
		bool dragCompleted = false;
		runtime.dragDelay = 0f;
		runtime.OnDragBegin += (TowerDefenseCharacter target) =>
		{
			if (target == zombie)
			{
				dragBegan = true;
			}
		};
		runtime.OnDrag += (TowerDefenseCharacter target, bool success) =>
		{
			if ((target == zombie) & success)
			{
				dragCompleted = true;
			}
		};
		control.isGameRunning = true;
		runtime.IdleProcessing(0.0);
		await WaitFrames(18);
		control.isGameRunning = false;
		Check((attack.target == zombie) | dragBegan, scenario.Key + " must acquire the overlapping real zombie through production collision.");
		Check(dragBegan, scenario.Key + " must begin its real water-grass drag.");
		Check(dragCompleted, scenario.Key + " must complete its real water-grass drag.");
		if (scenario.HypnotizesTarget)
		{
			Check(GodotObject.IsInstanceValid(zombie) && (zombie.instance?.hypnoses ?? false) && !zombie.die && !zombie.isDestroy, scenario.Key + " must convert and preserve the dragged zombie.");
		}
		else
		{
			Check(!GodotObject.IsInstanceValid(zombie) || zombie.die || zombie.isDestroy, scenario.Key + " must remove the lethally dragged zombie.");
		}
		Check(!GodotObject.IsInstanceValid(plant) || plant.isDestroy, "The one-use " + scenario.Key + " must consume itself after the completed drag.");
		if (GodotObject.IsInstanceValid(zombie))
		{
			zombie.QueueFree();
		}
		if (GodotObject.IsInstanceValid(plant))
		{
			plant.QueueFree();
		}
		await WaitFrames(5);
	}

	private async Task VerifyPlanternDeathDrag(BugDepartmentTanglekelpFamilyRuntimeControlStub control, Vector2I grid)
	{
		control.isGameRunning = false;
		TowerDefensePlantPlanternTanglekelp plant = LoadPacket("res://Asset/Anime/Character/Plant/Chapter4/PlanternTanglekelp/Packet/PlantPlanternTanglekelp.tres")?.Plant(grid, playAudio: false) as TowerDefensePlantPlanternTanglekelp;
		await WaitFrames(75);
		TowerDefenseZombieNormal zombie = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres")?.Plant(grid, playAudio: false) as TowerDefenseZombieNormal;
		await WaitFrames(6);
		Check(GodotObject.IsInstanceValid(plant) && plant.config?.name == "PlantPlanternTanglekelp" && plant.SceneFilePath == "res://Asset/Anime/Character/Plant/Chapter4/PlanternTanglekelp/Scene/TowerDefensePlantPlanternTanglekelp.tscn", "The real PlanternTanglekelp role scene must spawn on water.");
		Check(GodotObject.IsInstanceValid(zombie) && zombie.config?.name == "ZombieNormal", "PlanternTanglekelp must face a real ZombieNormal target.");
		Require(GodotObject.IsInstanceValid(plant) && GodotObject.IsInstanceValid(zombie), "The real PlanternTanglekelp encounter did not spawn.");
		zombie.ProcessMode = ProcessModeEnum.Disabled;
		plant.inGame = true;
		plant.inWater = true;
		zombie.inGame = true;
		zombie.gridPos = grid;
		zombie.GlobalPosition = plant.GlobalPosition + new Vector2(28f, 0f);
		await WaitFrames(3);
		TanglekelpComponent runtime = plant.componentManager.GetRuntime<TanglekelpComponent>();
		AttackComponent runtime2 = plant.componentManager.GetRuntime<AttackComponent>("character.attack.0");
		Check(runtime != null && !runtime.IsReleased && runtime.Alive && runtime.attackComponent == null && !runtime.destroyUse, "PlanternTanglekelp must retain its intentional death-only Tanglekelp definition.");
		Check(runtime2 != null && !runtime2.IsReleased && plant.attackComponent == runtime2, "PlanternTanglekelp must retain its production death-target Attack runtime.");
		Check((zombie.config?.canDragIntoWater ?? false) && plant.inWater, "The real Plantern encounter must use an eligible target in water.");
		Require(runtime != null && !runtime.IsReleased && runtime2 != null && !runtime2.IsReleased, "PlanternTanglekelp component runtimes are unavailable.");
		control.isGameRunning = true;
		bool flag = runtime2.CanAttackOnce();
		Check(flag && runtime2.target == zombie, "PlanternTanglekelp must acquire the overlapping real zombie before destruction.");
		plant.Destroy();
		await WaitFrames(55);
		control.isGameRunning = false;
		Check(!GodotObject.IsInstanceValid(zombie) || zombie.die || zombie.isDestroy, "A real PlanternTanglekelp destroyed in water must drag its acquired zombie under.");
		Check(!GodotObject.IsInstanceValid(plant) || plant.isDestroy, "The real PlanternTanglekelp must complete its own destruction lifecycle.");
		if (GodotObject.IsInstanceValid(zombie))
		{
			zombie.QueueFree();
		}
		if (GodotObject.IsInstanceValid(plant))
		{
			plant.QueueFree();
		}
		await WaitFrames(5);
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = new TowerDefenseMapConfig()
		});
		towerDefenseBattleFeatureMap.plantGrid.Resize(gridNum.X + 1);
		for (int i = 0; i <= gridNum.X; i++)
		{
			Godot.Collections.Array array = new Godot.Collections.Array();
			array.Resize(gridNum.Y + 1);
			for (int j = 1; j <= gridNum.Y; j++)
			{
				TowerDefenseCellConfig config = new TowerDefenseCellConfig
				{
					gridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>
					{
						TowerDefenseEnum.PLANTGRIDTYPE.WATER,
						TowerDefenseEnum.PLANTGRIDTYPE.AIR
					}
				};
				TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
				{
					gridPos = new Vector2I(i, j)
				};
				towerDefenseCellInstance.Init(config);
				array[j] = towerDefenseCellInstance;
			}
			towerDefenseBattleFeatureMap.plantGrid[i] = array;
		}
		towerDefenseBattleFeatureMap.iceCapList.Resize(gridNum.Y + 1);
		return towerDefenseBattleFeatureMap;
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private void RegisterRealFixtures()
	{
		RegisterPacket("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres");
		RegisterCharacter("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
		RegisterPacket("CraterDayGround", "res://Asset/Anime/Character/Crater/CraterDayGround/Packet/CraterDayGround.tres");
		RegisterCharacter("CraterDayGround", "res://Asset/Anime/Character/Crater/CraterDayGround/Scene/TowerDefenseCraterDayGround.tscn");
		DragScenario[] dragScenarios = DragScenarios;
		for (int i = 0; i < dragScenarios.Length; i++)
		{
			DragScenario dragScenario = dragScenarios[i];
			RegisterPacket(dragScenario.Key, dragScenario.PacketPath);
			RegisterCharacter(dragScenario.Key, dragScenario.ScenePath);
		}
		RegisterPacket("PlantPlanternTanglekelp", "res://Asset/Anime/Character/Plant/Chapter4/PlanternTanglekelp/Packet/PlantPlanternTanglekelp.tres");
		RegisterCharacter("PlantPlanternTanglekelp", "res://Asset/Anime/Character/Plant/Chapter4/PlanternTanglekelp/Scene/TowerDefensePlantPlanternTanglekelp.tscn");
	}

	private void RegisterPacket(string key, string path)
	{
		ResourceManager instance = ResourceManager.Instance;
		if (instance.TOWERDEFENSE_PACKETS.TryGetValue(key, out var value))
		{
			_previousPackets[key] = value;
		}
		else
		{
			_missingPackets.Add(key);
		}
		instance.TOWERDEFENSE_PACKETS[key] = ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore);
	}

	private void RegisterCharacter(string key, string path)
	{
		ResourceManager instance = ResourceManager.Instance;
		if (instance.TOWERDEFENSE_CHARCATERS.TryGetValue(key, out var value))
		{
			_previousCharacters[key] = value;
		}
		else
		{
			_missingCharacters.Add(key);
		}
		instance.TOWERDEFENSE_CHARCATERS[key] = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
	}

	private void RestoreRealFixtures()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return;
		}
		foreach (string missingPacket in _missingPackets)
		{
			instance.TOWERDEFENSE_PACKETS.Remove(missingPacket);
		}
		foreach (KeyValuePair<string, Resource> previousPacket in _previousPackets)
		{
			instance.TOWERDEFENSE_PACKETS[previousPacket.Key] = previousPacket.Value;
		}
		foreach (string missingCharacter in _missingCharacters)
		{
			instance.TOWERDEFENSE_CHARCATERS.Remove(missingCharacter);
		}
		foreach (KeyValuePair<string, Resource> previousCharacter in _previousCharacters)
		{
			instance.TOWERDEFENSE_CHARCATERS[previousCharacter.Key] = previousCharacter.Value;
		}
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugDepartmentTanglekelpFamilyRuntimeTest] " + message);
		}
	}

	private static void Require(bool condition, string message)
	{
		if (!condition)
		{
			throw new InvalidOperationException(message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadPacket, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterRealFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreRealFixtures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Check, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterRealFixtures && args.Count == 0)
		{
			RegisterRealFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterPacket && args.Count == 2)
		{
			RegisterPacket(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterCharacter && args.Count == 2)
		{
			RegisterCharacter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreRealFixtures && args.Count == 0)
		{
			RestoreRealFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.LoadPacket && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketConfig>(LoadPacket(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.CreateMapFeature)
		{
			return true;
		}
		if (method == MethodName.LoadPacket)
		{
			return true;
		}
		if (method == MethodName.RegisterRealFixtures)
		{
			return true;
		}
		if (method == MethodName.RegisterPacket)
		{
			return true;
		}
		if (method == MethodName.RegisterCharacter)
		{
			return true;
		}
		if (method == MethodName.RestoreRealFixtures)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Require)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
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
	}
}
