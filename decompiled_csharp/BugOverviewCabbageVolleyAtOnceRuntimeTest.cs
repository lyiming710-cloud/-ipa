using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewCabbageVolleyAtOnceRuntimeTest.cs")]
public class BugOverviewCabbageVolleyAtOnceRuntimeTest : Node
{
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
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string CobPacketPath = "res://Asset/Anime/Character/Plant/Cover/CabbageCobX/Packet/PlantCabbageCobX.tres";

	private const string CobScenePath = "res://Asset/Anime/Character/Plant/Cover/CabbageCobX/Scene/TowerDefensePlantCabbageCobX.tscn";

	private const string InsidePacketPath = "res://Asset/Anime/Character/Plant/Other/CabbagepultInside/Packet/PlantCabbagepultInside.tres";

	private const string InsideScenePath = "res://Asset/Anime/Character/Plant/Other/CabbagepultInside/Scene/TowerDefensePlantCabbagepultInside.tscn";

	private const string ZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const int ExpectedVolleySize = 3;

	private const double ExpectedBaseArcHeight = 300.0;

	private const double ExpectedArcHeightStep = 120.0;

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		ProcessModeEnum previousProjectileProcessMode = ProjectileUpdateManager.Instance?.ProcessMode ?? ProcessModeEnum.Inherit;
		BugOverviewCabbageVolleyAtOnceControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ObjectManager.Instance), "ObjectManager autoload must be available for real Node projectiles.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance) || !GodotObject.IsInstanceValid(ObjectManager.Instance))
				{
					goto end_IL_00f2;
				}
				ProjectileUpdateManager.Instance.ProcessMode = ProcessModeEnum.Disabled;
				TowerDefenseProjectileRegistry.Init();
				RegisterRealFixtures();
				control = new BugOverviewCabbageVolleyAtOnceControlStub
				{
					Name = "CabbageVolleyAtOnceControl",
					isGameRunning = false,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig()
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = new Vector2(0f, 100f);
				manager.gridSize = new Vector2(100f, 76f);
				manager.gridNum = new Vector2I(9, 5);
				mapControl = new TowerDefenseMapControl
				{
					Name = "MapControl"
				};
				mapFeature = CreateMapFeature(mapControl, manager.gridNum);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				TowerDefensePlantCabbageCobX cob = LoadPacket("res://Asset/Anime/Character/Plant/Cover/CabbageCobX/Packet/PlantCabbageCobX.tres")?.Plant(new Vector2I(2, 2), playAudio: false, noLimit: true, default, skipPlacementCheck: true) as TowerDefensePlantCabbageCobX;
				TowerDefensePlantCabbagepultInside inside = LoadPacket("res://Asset/Anime/Character/Plant/Other/CabbagepultInside/Packet/PlantCabbagepultInside.tres")?.Plant(new Vector2I(2, 3), playAudio: false) as TowerDefensePlantCabbagepultInside;
				await WaitFrames(5);
				TowerDefensePlantCabbagepultInside insideStacker = LoadPacket("res://Asset/Anime/Character/Plant/Other/CabbagepultInside/Packet/PlantCabbagepultInside.tres")?.Plant(new Vector2I(3, 3), playAudio: false) as TowerDefensePlantCabbagepultInside;
				await WaitFrames(3);
				TowerDefenseCharacter cobTarget = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres")?.Plant(new Vector2I(7, 2), playAudio: false);
				TowerDefenseCharacter insideTarget = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres")?.Plant(new Vector2I(7, 3), playAudio: false);
				await WaitFrames(5);
				Check(GodotObject.IsInstanceValid(cob) && cob.config?.name == "PlantCabbageCobX", "The real Cabbage Cob X scene must instantiate.");
				Check(GodotObject.IsInstanceValid(inside) && inside.config?.name == "PlantCabbagepultInside", "The real Inside Cabbage-pult scene must instantiate.");
				Check(GodotObject.IsInstanceValid(insideStacker) && insideStacker.config?.name == "PlantCabbagepultInside", "A second real Inside Cabbage-pult must instantiate through normal packet placement.");
				Check(GodotObject.IsInstanceValid(cobTarget) && GodotObject.IsInstanceValid(insideTarget), "Two real normal-zombie targets must instantiate.");
				if (!GodotObject.IsInstanceValid(cob) || !GodotObject.IsInstanceValid(inside) || !GodotObject.IsInstanceValid(insideStacker) || !GodotObject.IsInstanceValid(cobTarget) || !GodotObject.IsInstanceValid(insideTarget))
				{
					goto end_IL_00f2;
				}
				cobTarget.instance.hitpoints = 1000000.0;
				cobTarget.instance.hitpointsBase = 1000000.0;
				insideTarget.instance.hitpoints = 1000000.0;
				insideTarget.instance.hitpointsBase = 1000000.0;
				cobTarget.ProcessMode = ProcessModeEnum.Disabled;
				insideTarget.ProcessMode = ProcessModeEnum.Disabled;
				FireComponent fireComponent = cob.componentManager?.GetRuntime<FireComponent>("character.fire");
				FireComponent fireComponent2 = inside.componentManager?.GetRuntime<FireComponent>("character.fire");
				FireComponent fireComponent3 = insideStacker.componentManager?.GetRuntime<FireComponent>("character.fire");
				Check(fireComponent != null && !fireComponent.IsReleased && fireComponent.fireNum == 3 && cob.fireNum == 3, "Cabbage Cob X must retain its authored three-projectile volley.");
				Check(fireComponent2 != null && !fireComponent2.IsReleased && fireComponent3 != null && !fireComponent3.IsReleased, "Both Inside Cabbage-pults must expose their real FireComponent runtimes.");
				Check(inside.fireNum == 2 && fireComponent2 != null && fireComponent2.fireNum == 2, "Planting a second Inside Cabbage-pult must increase the existing plant's volley to two shots.");
				Check(insideStacker.fireNum == 1 && fireComponent3 != null && fireComponent3.fireNum == 1, "The newly planted Inside Cabbage-pult must retain its authored one-shot starting volley.");
				if (fireComponent == null || fireComponent.IsReleased || fireComponent2 == null || fireComponent2.IsReleased || fireComponent3 == null || fireComponent3.IsReleased)
				{
					goto end_IL_00f2;
				}
				cob.ImportVariantSave(new Dictionary());
				Check(cob.fireNum == 3 && fireComponent.fireNum == 3, "A legacy Cabbage Cob X save without variant fields must retain the authored three-shot volley.");
				Check(cob.projectileName == "CabbageCobX", "A legacy Cabbage Cob X save without variant fields must retain its authored projectile.");
				cob.ImportVariantSave(new Dictionary
				{
					["fireNum"] = 1,
					["projectileName"] = "WinterMelon",
					["fireInterval"] = 3.0
				});
				Check(cob.fireNum == 3 && fireComponent.fireNum == 3 && cob.projectileName == "CabbageCobX", "A save containing the old one-shot WinterMelon fallback must migrate back to the authored volley.");
				cob.ImportVariantSave(new Dictionary
				{
					["fireNum"] = 1,
					["projectileName"] = "CabbageCobX",
					["fireInterval"] = 3.0
				});
				Check(cob.fireNum == 1 && fireComponent.fireNum == 1 && cob.projectileName == "CabbageCobX", "A legitimate custom one-shot Cabbage Cob X variant must not be mistaken for the legacy fallback.");
				cob.ImportVariantSave(new Dictionary());
				FireOneAuthoredEvent(cob, fireComponent, "Cabbage Cob X", 3, 300.0, 3, requireDistinctTrajectories: true);
				FireOneAuthoredEvent(inside, fireComponent2, "naturally stacked Inside Cabbage-pult", 2, 400.0, 1, requireDistinctTrajectories: true);
				inside.fireNum = 3;
				Check(fireComponent2.fireNum == 3, "Inside Cabbage-pult stacking must forward the dynamic volley size.");
				FireOneAuthoredEvent(inside, fireComponent2, "three-shot Inside Cabbage-pult", 3, 400.0, 1, requireDistinctTrajectories: true);
				goto end_IL_00db;
				end_IL_00f2:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[CabbageVolleyAtOnce] Unexpected exception: {value}");
				goto end_IL_00db;
			}
			return;
			end_IL_00db:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(ProjectileUpdateManager.Instance))
			{
				ProjectileUpdateManager.Instance.ProcessMode = previousProjectileProcessMode;
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
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
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		bool flag = _failures == 0;
		GD.Print($"CABBAGE_VOLLEY_AT_ONCE_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void FireOneAuthoredEvent(TowerDefensePlant plant, FireComponent fire, string label, int expectedVolleySize, double expectedBaseArcHeight, int expectedAuthoredProjectileCount = 1, bool requireDistinctTrajectories = false)
	{
		FireComponentCheckConfig fireComponentCheckConfig = ((fire.fireCheckList.Count > 0) ? fire.fireCheckList[0] : null);
		TowerDefenseProjectileCreateData instance = fireComponentCheckConfig?.projectile?.GetProjectile();
		Check(GodotObject.IsInstanceValid(fireComponentCheckConfig) && GodotObject.IsInstanceValid(instance), label + " must retain its authored projectile configuration.");
		if (!GodotObject.IsInstanceValid(fireComponentCheckConfig) || !GodotObject.IsInstanceValid(instance))
		{
			return;
		}
		if (requireDistinctTrajectories)
		{
			List<TowerDefenseProjectileCreateData> list = new List<TowerDefenseProjectileCreateData>();
			fireComponentCheckConfig.projectile.CollectProjectileData(list, new HashSet<FireComponentProjectileResource>());
			bool flag = list.Count == expectedAuthoredProjectileCount;
			foreach (TowerDefenseProjectileCreateData item in list)
			{
				flag &= item != null && Math.Abs(item.catapultHeight - expectedBaseArcHeight) < 0.001;
			}
			Check(flag, $"{label}'s authored projectiles must share the bounded {expectedBaseArcHeight}-pixel base arc; heights=[{string.Join(", ", list.ConvertAll((TowerDefenseProjectileCreateData data) => data?.catapultHeight ?? (0.0 / 0.0)))}].");
		}
		fire.alive = true;
		fire.timer = 0f;
		fire.checkInterval = 0;
		fire.runningCheck = fireComponentCheckConfig;
		fire.runningCheckId = 0;
		fire.currentFireNum = 0;
		int projectileCount = 0;
		int volleyCount = 0;
		List<int> fireIndices = new List<int>();
		List<int> spawnedIndices = new List<int>();
		List<double> launchSpeeds = new List<double>();
		List<double> preparedArcHeights = new List<double>();
		BulletField bulletField = BulletField.EnsureMountedOnCharacterNode();
		Check(GodotObject.IsInstanceValid(bulletField), label + " must have a mounted BulletField before firing.");
		if (!GodotObject.IsInstanceValid(bulletField))
		{
			return;
		}
		bulletField.OnBulletSpawned += OnBulletSpawned;
		fire.OnPrepareProjectileData += OnPrepareProjectileData;
		fire.OnFireVolley += OnVolley;
		fire.AnimeEvent(fire.fireEventName, default);
		fire.OnFireVolley -= OnVolley;
		fire.OnPrepareProjectileData -= OnPrepareProjectileData;
		bulletField.OnBulletSpawned -= OnBulletSpawned;
		Check(projectileCount == expectedVolleySize && volleyCount == expectedVolleySize, $"One authored {label} fire event must synchronously create the full {expectedVolleySize}-shot volley; projectiles={projectileCount}, volleys={volleyCount}.");
		bool flag2 = fireIndices.Count == expectedVolleySize;
		for (int num = 0; num < fireIndices.Count; num++)
		{
			flag2 &= fireIndices[num] == num;
		}
		Check(flag2, $"{label} must expose deterministic zero-based volley indices; indices=[{string.Join(", ", fireIndices)}].");
		Check(spawnedIndices.Count == expectedVolleySize && new HashSet<int>(spawnedIndices).Count == expectedVolleySize, $"One authored {label} event must create {expectedVolleySize} distinct active BulletField projectiles; spawned=[{string.Join(", ", spawnedIndices)}].");
		if (requireDistinctTrajectories)
		{
			bool flag3 = preparedArcHeights.Count == expectedVolleySize;
			for (int num2 = 0; num2 < preparedArcHeights.Count; num2++)
			{
				double num3 = (double)num2 - (double)(expectedVolleySize - 1) * 0.5;
				double num4 = Math.Max(100.0, expectedBaseArcHeight + num3 * 120.0);
				flag3 &= Math.Abs(preparedArcHeights[num2] - num4) < 0.001;
			}
			Check(flag3, $"{label} must prepare distinct but bounded catapult heights; heights=[{string.Join(", ", preparedArcHeights)}].");
			Check(launchSpeeds.Count == expectedVolleySize && new HashSet<double>(launchSpeeds).Count == expectedVolleySize, $"{label}'s simultaneous projectiles must use visibly distinct ballistic arcs; launchSpeeds=[{string.Join(", ", launchSpeeds)}].");
		}
		Check(fire.currentFireNum == 0, label + " must complete the volley in the same animation event without scheduling a repeat.");
		fire.alive = false;
		void OnBulletSpawned(int index)
		{
			ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(index);
			if (bulletDataRef.fireCharacter == plant)
			{
				spawnedIndices.Add(index);
				launchSpeeds.Add(bulletDataRef.ySpeed);
			}
		}
		void OnPrepareProjectileData(int _, TowerDefenseProjectileCreateData data)
		{
			if (data != null)
			{
				preparedArcHeights.Add(data.catapultHeight);
			}
		}
		void OnVolley(ulong _)
		{
			projectileCount++;
			volleyCount++;
			fireIndices.Add(fire.currentFireNum);
		}
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = new TowerDefenseMapConfig
			{
				gridNum = gridNum
			}
		});
		towerDefenseBattleFeatureMap.PlantGridInit();
		towerDefenseBattleFeatureMap.rect = TowerDefenseBattleFeatureMap.BuildProjectileBoundaryRect(towerDefenseBattleFeatureMap.config);
		for (int i = 1; i <= gridNum.X; i++)
		{
			for (int j = 1; j <= gridNum.Y; j++)
			{
				towerDefenseBattleFeatureMap.GetPlantGridCell(new Vector2I(i, j)).Init(new TowerDefenseCellConfig());
			}
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
		RegisterPacket("PlantCabbageCobX", "res://Asset/Anime/Character/Plant/Cover/CabbageCobX/Packet/PlantCabbageCobX.tres");
		RegisterPacket("PlantCabbagepultInside", "res://Asset/Anime/Character/Plant/Other/CabbagepultInside/Packet/PlantCabbagepultInside.tres");
		RegisterPacket("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres");
		RegisterCharacter("PlantCabbageCobX", "res://Asset/Anime/Character/Plant/Cover/CabbageCobX/Scene/TowerDefensePlantCabbageCobX.tscn");
		RegisterCharacter("PlantCabbagepultInside", "res://Asset/Anime/Character/Plant/Other/CabbagepultInside/Scene/TowerDefensePlantCabbagepultInside.tscn");
		RegisterCharacter("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
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
			GD.PushError("[CabbageVolleyAtOnce] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
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
