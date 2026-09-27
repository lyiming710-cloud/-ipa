using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugDepartmentPeaWellSpikePenetrationRuntimeTest.cs")]
public class BugDepartmentPeaWellSpikePenetrationRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName RegisterRealFixtures = "RegisterRealFixtures";

		public static readonly StringName RegisterPacket = "RegisterPacket";

		public static readonly StringName RegisterCharacter = "RegisterCharacter";

		public static readonly StringName RegisterProjectile = "RegisterProjectile";

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

	private const string PeaWellPacketPath = "res://Asset/Anime/Character/Plant/Chapter3/PeaWell/Packet/PlantPeaWell.tres";

	private const string PeaWellScenePath = "res://Asset/Anime/Character/Plant/Chapter3/PeaWell/Scene/TowerDefensePlantPeaWell.tscn";

	private const string ZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const string SpikeConfigPath = "res://Asset/Config/Projectile/Spike/SpikeDefault.tres";

	private const string IceSpikeConfigPath = "res://Asset/Config/Projectile/Spike/IceSpike.tres";

	private static readonly Vector2I SpikeWellGrid = new Vector2I(2, 2);

	private static readonly Vector2I SpikeNearGrid = new Vector2I(4, 2);

	private static readonly Vector2I SpikeFarGrid = new Vector2I(6, 2);

	private static readonly Vector2I IceWellGrid = new Vector2I(2, 4);

	private static readonly Vector2I IceNearGrid = new Vector2I(4, 4);

	private static readonly Vector2I IceFarGrid = new Vector2I(6, 4);

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousProjectiles = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private readonly HashSet<string> _missingProjectiles = new HashSet<string>();

	private readonly List<Resource> _registeredResources = new List<Resource>();

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		BugDepartmentPeaWellSpikePenetrationRuntimeControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		try
		{
			_ = 5;
			try
			{
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ResourceManager.Instance), "TowerDefenseManager and ResourceManager autoloads must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_00f2;
				}
				TowerDefenseProjectileRegistry.Init();
				RegisterRealFixtures();
				control = new BugDepartmentPeaWellSpikePenetrationRuntimeControlStub
				{
					Name = "PeaWellSpikePenetrationRuntimeControl",
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
				int shooterFlag = 1;
				int penetrateFlag = 4;
				TowerDefenseProjectileConfig projectileConfig = TowerDefenseManager.GetProjectileConfig("SpikeDefault");
				TowerDefenseProjectileConfig projectileConfig2 = TowerDefenseManager.GetProjectileConfig("IceSpike");
				Check(GodotObject.IsInstanceValid(projectileConfig) && (projectileConfig.fireMethodFlags & (shooterFlag | penetrateFlag)) == (shooterFlag | penetrateFlag) && projectileConfig.penetrateNum == 3, "The authored SpikeDefault config must retain shooter plus three-hit penetration.");
				Check(GodotObject.IsInstanceValid(projectileConfig2) && (projectileConfig2.fireMethodFlags & (shooterFlag | penetrateFlag)) == (shooterFlag | penetrateFlag) && projectileConfig2.penetrateNum == 3, "The authored IceSpike config must retain shooter plus three-hit penetration.");
				Check(GodotObject.IsInstanceValid(TowerDefenseProjectileRegistry.GetProjectile("Spike")) && GodotObject.IsInstanceValid(TowerDefenseProjectileRegistry.GetProjectile("IceSpike")), "The real Spike and IceSpike registry data must be available.");
				TowerDefensePlantPeaWell spikeWell = LoadPacket("res://Asset/Anime/Character/Plant/Chapter3/PeaWell/Packet/PlantPeaWell.tres")?.Plant(SpikeWellGrid, playAudio: false) as TowerDefensePlantPeaWell;
				await WaitFrames(4);
				TowerDefensePlantPeaWell iceWell = LoadPacket("res://Asset/Anime/Character/Plant/Chapter3/PeaWell/Packet/PlantPeaWell.tres")?.Plant(IceWellGrid, playAudio: false) as TowerDefensePlantPeaWell;
				await WaitFrames(4);
				TowerDefenseZombieNormal spikeNear = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres")?.Plant(SpikeNearGrid, playAudio: false) as TowerDefenseZombieNormal;
				TowerDefenseZombieNormal spikeFar = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres")?.Plant(SpikeFarGrid, playAudio: false) as TowerDefenseZombieNormal;
				TowerDefenseZombieNormal iceNear = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres")?.Plant(IceNearGrid, playAudio: false) as TowerDefenseZombieNormal;
				TowerDefenseZombieNormal iceFar = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres")?.Plant(IceFarGrid, playAudio: false) as TowerDefenseZombieNormal;
				await WaitFrames(7);
				Check(GodotObject.IsInstanceValid(spikeWell) && GodotObject.IsInstanceValid(iceWell) && GodotObject.IsInstanceValid(spikeNear) && GodotObject.IsInstanceValid(spikeFar) && GodotObject.IsInstanceValid(iceNear) && GodotObject.IsInstanceValid(iceFar), "Two real PeaWells and four real normal-zombie targets must spawn.");
				if (!GodotObject.IsInstanceValid(spikeWell) || !GodotObject.IsInstanceValid(iceWell) || !GodotObject.IsInstanceValid(spikeNear) || !GodotObject.IsInstanceValid(spikeFar) || !GodotObject.IsInstanceValid(iceNear) || !GodotObject.IsInstanceValid(iceFar))
				{
					goto end_IL_00f2;
				}
				spikeWell.ProcessMode = ProcessModeEnum.Disabled;
				iceWell.ProcessMode = ProcessModeEnum.Disabled;
				spikeNear.ProcessMode = ProcessModeEnum.Disabled;
				spikeFar.ProcessMode = ProcessModeEnum.Disabled;
				iceNear.ProcessMode = ProcessModeEnum.Disabled;
				iceFar.ProcessMode = ProcessModeEnum.Disabled;
				spikeWell.fireInterval = 1000.0;
				iceWell.fireInterval = 1000.0;
				control.isGameRunning = true;
				BulletField bulletField = BulletField.EnsureMountedOnCharacterNode();
				await WaitFrames(2);
				Check(GodotObject.IsInstanceValid(bulletField) && BulletField.Instance == bulletField, "The production BulletField must be mounted for PeaWell projectiles.");
				if (!GodotObject.IsInstanceValid(bulletField))
				{
					goto end_IL_00f2;
				}
				await VerifyProjectile(spikeWell, spikeNear, spikeFar, "Spike", bulletField, shooterFlag, penetrateFlag);
				await VerifyProjectile(iceWell, iceNear, iceFar, "IceSpike", bulletField, shooterFlag, penetrateFlag);
				goto end_IL_00cf;
				end_IL_00f2:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugDepartmentPeaWellSpikePenetrationRuntimeTest] Unexpected exception: {value}");
				goto end_IL_00cf;
			}
			return;
			end_IL_00cf:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(BulletField.Instance))
			{
				BulletField.Instance.ClearActiveBullets();
			}
			if (GodotObject.IsInstanceValid(control?.characterNode))
			{
				foreach (Node child in control.characterNode.GetChildren())
				{
					if (!child.IsQueuedForDeletion())
					{
						child.QueueFree();
					}
				}
			}
			await WaitFrames(5);
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
			if (GodotObject.IsInstanceValid(control) && !control.IsQueuedForDeletion())
			{
				control.QueueFree();
			}
			RestoreRealFixtures();
			await WaitFrames(6);
			if (GodotObject.IsInstanceValid(ObjectManager.Instance))
			{
				ObjectManager.Instance.Clear();
			}
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			foreach (Resource registeredResource in _registeredResources)
			{
				registeredResource?.Dispose();
			}
			_registeredResources.Clear();
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
		bool flag = _failures == 0 && _checks == 26;
		GD.Print($"PEA_WELL_SPIKE_PENETRATION_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private async Task VerifyProjectile(TowerDefensePlantPeaWell well, TowerDefenseZombieNormal nearTarget, TowerDefenseZombieNormal farTarget, string projectileName, BulletField bulletField, int shooterFlag, int penetrateFlag)
	{
		well.projectileName = projectileName;
		TowerDefenseProjectileCreateData towerDefenseProjectileCreateData = well.CreateProjectileData();
		TowerDefenseProjectileConfig towerDefenseProjectileConfig = towerDefenseProjectileCreateData.BuildConfig();
		Check(GodotObject.IsInstanceValid(towerDefenseProjectileConfig) && towerDefenseProjectileConfig.name == projectileName, "PeaWell must resolve the real " + projectileName + " projectile data.");
		if (GodotObject.IsInstanceValid(towerDefenseProjectileConfig))
		{
			Check((towerDefenseProjectileConfig.fireMethodFlags & (shooterFlag | penetrateFlag)) == (shooterFlag | penetrateFlag), $"PeaWell {projectileName} data must carry SHOOTER and PENETRATE; flags={towerDefenseProjectileConfig.fireMethodFlags}.");
			Check(towerDefenseProjectileCreateData.overridePenetrateNum && towerDefenseProjectileConfig.penetrateNum == 3, $"PeaWell {projectileName} data must carry the authored three-hit limit; override={towerDefenseProjectileCreateData.overridePenetrateNum}, num={towerDefenseProjectileConfig.penetrateNum}.");
			bulletField.ClearActiveBullets();
			int activeCount = bulletField.ActiveCount;
			well.Timeout("Fire");
			Check(bulletField.ActiveCount - activeCount == 36, $"The real PeaWell timeout must emit 36 {projectileName} bullets; active={bulletField.ActiveCount}.");
			int lastSpawnedIndex = bulletField.LastSpawnedIndex;
			Check(bulletField.IsBulletActive(lastSpawnedIndex), "The real PeaWell " + projectileName + " fan must expose its final struct bullet.");
			if (bulletField.IsBulletActive(lastSpawnedIndex))
			{
				ref BulletData bulletDataRef = ref bulletField.GetBulletDataRef(lastSpawnedIndex);
				Check(bulletDataRef.config?.name == projectileName && (bulletDataRef.fireMethodFlags & penetrateFlag) != 0 && bulletDataRef.penetrateNum == 3, $"The spawned PeaWell {projectileName} fan must retain live penetration state; flags={bulletDataRef.fireMethodFlags}, num={bulletDataRef.penetrateNum}.");
			}
			else
			{
				Check(condition: false, "The spawned PeaWell " + projectileName + " fan bullet was unexpectedly inactive.");
			}
			bulletField.ClearActiveBullets();
			double nearBefore = nearTarget.instance.hitpoints;
			double farBefore = farTarget.instance.hitpoints;
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				checkAllOverride = true,
				initialRotationOverride = 0f
			};
			FireComponent.CreateProjectilePositionByData(well, null, (float)(well.GetGroundHeight(well.GlobalPosition.Y) + 10.0), well.GlobalPosition, new Vector2(200f, 0f), towerDefenseProjectileCreateData, -1, well.camp, default, overrides);
			int lastSpawnedIndex2 = bulletField.LastSpawnedIndex;
			Check(bulletField.IsBulletActive(lastSpawnedIndex2), "The PeaWell " + projectileName + " production factory must spawn a straight struct bullet.");
			if (bulletField.IsBulletActive(lastSpawnedIndex2))
			{
				ref BulletData bulletDataRef2 = ref bulletField.GetBulletDataRef(lastSpawnedIndex2);
				Check((bulletDataRef2.fireMethodFlags & penetrateFlag) != 0 && bulletDataRef2.penetrateNum == 3, $"The live PeaWell {projectileName} bullet must enter BulletField's penetrating branch; flags={bulletDataRef2.fireMethodFlags}, num={bulletDataRef2.penetrateNum}.");
			}
			else
			{
				Check(condition: false, "The straight PeaWell " + projectileName + " bullet was unexpectedly inactive.");
			}
			await WaitSimulationFrames(180);
			Check(nearTarget.instance.hitpoints < nearBefore, $"The real PeaWell {projectileName} bullet must damage the near zombie; before={nearBefore}, after={nearTarget.instance.hitpoints}.");
			Check(farTarget.instance.hitpoints < farBefore, $"The same PeaWell {projectileName} bullet must penetrate and damage the far zombie; before={farBefore}, after={farTarget.instance.hitpoints}.");
		}
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			gridNum = gridNum,
			gridBeginPos = Vector2.Zero,
			gridSize = new Vector2(100f, 76f),
			plantOffset = 50.0
		};
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig
		});
		towerDefenseBattleFeatureMap.plantGrid.Resize(gridNum.X + 1);
		for (int i = 0; i <= gridNum.X; i++)
		{
			Godot.Collections.Array array = new Godot.Collections.Array();
			array.Resize(gridNum.Y + 1);
			for (int j = 1; j <= gridNum.Y; j++)
			{
				TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
				{
					gridPos = new Vector2I(i, j)
				};
				towerDefenseCellInstance.Init(new TowerDefenseCellConfig());
				array[j] = towerDefenseCellInstance;
			}
			towerDefenseBattleFeatureMap.plantGrid[i] = array;
		}
		towerDefenseBattleFeatureMap.iceCapList.Resize(gridNum.Y + 1);
		towerDefenseBattleFeatureMap.lineUse.Resize(gridNum.Y + 1);
		for (int k = 1; k <= gridNum.Y; k++)
		{
			towerDefenseBattleFeatureMap.lineUse[k] = true;
		}
		return towerDefenseBattleFeatureMap;
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private void RegisterRealFixtures()
	{
		RegisterPacket("PlantPeaWell", "res://Asset/Anime/Character/Plant/Chapter3/PeaWell/Packet/PlantPeaWell.tres");
		RegisterPacket("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres");
		RegisterCharacter("PlantPeaWell", "res://Asset/Anime/Character/Plant/Chapter3/PeaWell/Scene/TowerDefensePlantPeaWell.tscn");
		RegisterCharacter("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn");
		RegisterProjectile("SpikeDefault", "res://Asset/Config/Projectile/Spike/SpikeDefault.tres");
		RegisterProjectile("IceSpike", "res://Asset/Config/Projectile/Spike/IceSpike.tres");
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
		TowerDefensePacketConfig towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore);
		_registeredResources.Add(towerDefensePacketConfig);
		instance.TOWERDEFENSE_PACKETS[key] = towerDefensePacketConfig;
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
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Ignore);
		_registeredResources.Add(packedScene);
		instance.TOWERDEFENSE_CHARCATERS[key] = packedScene;
	}

	private void RegisterProjectile(string key, string path)
	{
		ResourceManager instance = ResourceManager.Instance;
		if (instance.PROJECTILE_CONFIG.TryGetValue(key, out var value))
		{
			_previousProjectiles[key] = value;
		}
		else
		{
			_missingProjectiles.Add(key);
		}
		TowerDefenseProjectileConfig towerDefenseProjectileConfig = ResourceLoader.Load<TowerDefenseProjectileConfig>(path, null, ResourceLoader.CacheMode.Ignore);
		_registeredResources.Add(towerDefenseProjectileConfig);
		instance.PROJECTILE_CONFIG[key] = towerDefenseProjectileConfig;
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
		foreach (string missingProjectile in _missingProjectiles)
		{
			instance.PROJECTILE_CONFIG.Remove(missingProjectile);
		}
		foreach (KeyValuePair<string, Resource> previousProjectile in _previousProjectiles)
		{
			instance.PROJECTILE_CONFIG[previousProjectile.Key] = previousProjectile.Value;
		}
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private async Task WaitSimulationFrames(int count)
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
			GD.PushError("[BugDepartmentPeaWellSpikePenetrationRuntimeTest] " + message);
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
			new MethodInfo(MethodName.RegisterProjectile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.RegisterProjectile && args.Count == 2)
		{
			RegisterProjectile(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.RegisterProjectile)
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
