using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewShootingLevel11WildPeaMowerRuntimeTest.cs")]
public class BugOverviewShootingLevel11WildPeaMowerRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetupBattleFixture = "SetupBattleFixture";

		public static readonly StringName HasMowerContract = "HasMowerContract";

		public static readonly StringName FindWildPeaFireUpgrade = "FindWildPeaFireUpgrade";

		public static readonly StringName AllRealCellsExist = "AllRealCellsExist";

		public static readonly StringName CountLiveMowers = "CountLiveMowers";

		public static readonly StringName AllMowersMatchProductionContract = "AllMowersMatchProductionContract";

		public static readonly StringName RegisterRealFixtures = "RegisterRealFixtures";

		public static readonly StringName SnapshotPacketCache = "SnapshotPacketCache";

		public static readonly StringName RegisterPacket = "RegisterPacket";

		public static readonly StringName RegisterCharacter = "RegisterCharacter";

		public static readonly StringName RestoreRealFixtures = "RestoreRealFixtures";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _control = "_control";

		public static readonly StringName _mapFeature = "_mapFeature";

		public static readonly StringName _mowerFeature = "_mowerFeature";

		public static readonly StringName _mapControl = "_mapControl";

		public static readonly StringName _wildPea = "_wildPea";

		public static readonly StringName _previousMowerPacketCache = "_previousMowerPacketCache";

		public static readonly StringName _hadMowerPacketCache = "_hadMowerPacketCache";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string NormalLevelPath = "res://Asset/Config/Level/TowerDefense/Shooting/Shooting_Level1_11.tres";

	private const string DifficultLevelPath = "res://Asset/Config/Level/TowerDefense/Shooting/Shooting_Level1_11_D.tres";

	private const string MapPath = "res://Asset/Config/Map/Vampire/VampireMapVampire.tres";

	private const string WildPeaPacketPath = "res://Asset/Anime/Character/Plant/Gold/WildPea/Packet/PlantWildPea.tres";

	private const string WildPeaScenePath = "res://Asset/Anime/Character/Plant/Gold/WildPea/Scene/TowerDefensePlantWildPea.tscn";

	private const string MowerPacketPath = "res://Asset/Anime/Character/Mower/Default/Packet/MowerDefault.tres";

	private const string MowerScenePath = "res://Asset/Anime/Character/Mower/Default/Scene/TowerDefenseMowerDefault.tscn";

	private static readonly Vector2I WildPeaGrid = new Vector2I(5, 3);

	private int _checks;

	private int _failures;

	private ShootingLevel11WildPeaMowerControlStub _control;

	private TowerDefenseBattleFeatureMap _mapFeature;

	private TowerDefenseBattleFeatureMower _mowerFeature;

	private TowerDefenseMapControl _mapControl;

	private TowerDefensePlantWildPea _wildPea;

	private readonly Dictionary<string, Resource> _previousPackets = new Dictionary<string, Resource>();

	private readonly Dictionary<string, Resource> _previousCharacters = new Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private Dictionary<string, TowerDefensePacketConfig> _packetCache;

	private TowerDefensePacketConfig _previousMowerPacketCache;

	private bool _hadMowerPacketCache;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager) && GodotObject.IsInstanceValid(ResourceManager.Instance), "TowerDefenseManager and ResourceManager autoloads must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					throw new InvalidOperationException("Required autoloads are unavailable.");
				}
				TowerDefenseLevelConfig towerDefenseLevelConfig = Load<TowerDefenseLevelConfig>("res://Asset/Config/Level/TowerDefense/Shooting/Shooting_Level1_11.tres");
				TowerDefenseLevelConfig towerDefenseLevelConfig2 = Load<TowerDefenseLevelConfig>("res://Asset/Config/Level/TowerDefense/Shooting/Shooting_Level1_11_D.tres");
				Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig) && towerDefenseLevelConfig.ResourcePath == "res://Asset/Config/Level/TowerDefense/Shooting/Shooting_Level1_11.tres" && towerDefenseLevelConfig.levelName == "野火燎原" && GodotObject.IsInstanceValid(towerDefenseLevelConfig2) && towerDefenseLevelConfig2.ResourcePath == "res://Asset/Config/Level/TowerDefense/Shooting/Shooting_Level1_11_D.tres" && towerDefenseLevelConfig2.levelName == "野火燎原", "Both fixtures must be the production Shooting Level 1-11 resources.");
				Check(HasMowerContract(towerDefenseLevelConfig), "Normal Shooting Level 1-11 must enable MowerUse and retain the Mower feature.");
				Check(HasMowerContract(towerDefenseLevelConfig2), "Difficult Shooting Level 1-11 must enable MowerUse and retain the Mower feature.");
				TowerDefenseLevelPacketConfig normalUpgrade = FindWildPeaFireUpgrade(towerDefenseLevelConfig);
				TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = FindWildPeaFireUpgrade(towerDefenseLevelConfig2);
				Check(GodotObject.IsInstanceValid(normalUpgrade), "The normal level's recursive WildPea chain must resolve the FirePea upgrade.");
				Check(GodotObject.IsInstanceValid(towerDefenseLevelPacketConfig), "The difficult level's recursive WildPea chain must resolve the FirePea upgrade.");
				Check(HasPropertyValue(normalUpgrade?.@override?.characterOverride, "projectileName", (Variant variant) => variant.AsString() == "FirePea") && HasPropertyValue(towerDefenseLevelPacketConfig?.@override?.characterOverride, "projectileName", (Variant variant) => variant.AsString() == "FirePea"), "Both authored variants must upgrade WildPea to the same FirePea projectile.");
				TowerDefenseMapConfig towerDefenseMapConfig = Load<TowerDefenseMapConfig>("res://Asset/Config/Map/Vampire/VampireMapVampire.tres");
				Check(GodotObject.IsInstanceValid(towerDefenseMapConfig) && towerDefenseMapConfig.ResourcePath == "res://Asset/Config/Map/Vampire/VampireMapVampire.tres" && towerDefenseMapConfig.gridNum == new Vector2I(9, 5), "The fixture must use the real 9x5 VampireMapVampire resource.");
				Check(towerDefenseMapConfig != null && towerDefenseMapConfig.lineUse?.Count == 5 && towerDefenseMapConfig.lineUse.Contains(1) && towerDefenseMapConfig.lineUse.Contains(2) && towerDefenseMapConfig.lineUse.Contains(3) && towerDefenseMapConfig.lineUse.Contains(4) && towerDefenseMapConfig.lineUse.Contains(5), "The production Vampire map must expose all five playable lanes.");
				RegisterRealFixtures();
				SetupBattleFixture(manager, towerDefenseMapConfig);
				Check(AllRealCellsExist(), "PlantGridInit must create every production Vampire map cell.");
				TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantWildPea");
				Check(GodotObject.IsInstanceValid(packetConfig) && packetConfig.characterConfig?.name == "PlantWildPea", "The production registry must resolve the real PlantWildPea packet.");
				_wildPea = packetConfig?.Plant(WildPeaGrid, playAudio: false, noLimit: true, default, skipPlacementCheck: true) as TowerDefensePlantWildPea;
				await WaitFrames(5);
				Check(GodotObject.IsInstanceValid(_wildPea) && _wildPea.IsNodeReady() && _wildPea.config?.name == "PlantWildPea", "The real PlantWildPea scene must enter the live character tree and finish Ready.");
				Check(_wildPea?.packet?.saveKey == "PlantWildPea" && _wildPea.cell == TowerDefenseManager.GetMapCell(WildPeaGrid), "The live WildPea must retain its real packet and Vampire map cell ownership.");
				normalUpgrade?.@override?.characterOverride?.ExecuteCharacter(_wildPea);
				Check(_wildPea?.projectileName == "FirePea", "The real level upgrade must mutate the live WildPea facade to FirePea.");
				FireComponent fireComponent = _wildPea?.componentManager?.GetRuntime<FireComponent>("character.fire");
				Check(fireComponent != null && !fireComponent.IsReleased && fireComponent.Alive, "The upgraded WildPea must use its real resource-backed character.fire runtime.");
				Check(fireComponent != null && fireComponent.fireCheckList?.Count > 0 && fireComponent.fireCheckList[0].projectile?.GetProjectile()?.projectileName.ToString() == "FirePea", "The live FireComponent must build FirePea after the authored upgrade is applied.");
				TowerDefensePacketConfig packetConfig2 = TowerDefenseManager.GetPacketConfig("MowerDefault");
				PackedScene packedScene = Load<PackedScene>("res://Asset/Anime/Character/Mower/Default/Scene/TowerDefenseMowerDefault.tscn");
				Check(GodotObject.IsInstanceValid(packetConfig2) && packetConfig2.characterConfig?.name == "MowerDefault" && GodotObject.IsInstanceValid(packedScene) && packedScene.CanInstantiate(), "The registry must resolve the real MowerDefault packet and production scene.");
				Check(_control.GetFeature(new StringName("Map")) == _mapFeature && _control.GetFeature(new StringName("Mower")) == _mowerFeature, "The runtime fixture must expose the production Map/Mower feature dependency graph.");
				_mowerFeature.MowerInit();
				await WaitFrames(5);
				Check(CountLiveMowers() == 5, "Production MowerInit must create exactly one mower on each of five active lanes.");
				Check(AllMowersMatchProductionContract(), "Every generated mower must be a real MowerDefault bound to its exact lane, cell, packet, and authored spawn position.");
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewShootingLevel11WildPeaMowerRuntimeTest] Unexpected exception: {value}");
			}
		}
		finally
		{
			if (GodotObject.IsInstanceValid(_wildPea) && !_wildPea.IsQueuedForDeletion())
			{
				_wildPea.QueueFree();
			}
			if (_mowerFeature != null)
			{
				foreach (TowerDefenseMower item in _mowerFeature.mowerLine)
				{
					if (GodotObject.IsInstanceValid(item) && !item.IsQueuedForDeletion())
					{
						item.QueueFree();
					}
				}
			}
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
				manager.gridBeginPos = previousGridBegin;
				manager.gridSize = previousGridSize;
				manager.gridNum = previousGridNum;
			}
			if (GodotObject.IsInstanceValid(_control))
			{
				_control.QueueFree();
			}
			if (GodotObject.IsInstanceValid(_mapControl))
			{
				_mapControl.Free();
			}
			RestoreRealFixtures();
			await WaitFrames(8);
			TowerDefenseGroundItemBase.ClearStaticBattleReferences();
			ObjectManager.Instance?.Clear();
			ResourceManager.Instance?.ReleaseTransientResources();
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			AdobeAnimateDefinitionCache.Clear();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			await WaitFrames(8);
		}
		bool flag = _failures == 0 && _checks == 20;
		GD.Print($"SHOOTING_LEVEL11_WILDPEA_MOWER_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void SetupBattleFixture(TowerDefenseManager manager, TowerDefenseMapConfig map)
	{
		_control = new ShootingLevel11WildPeaMowerControlStub
		{
			Name = "ShootingLevel11WildPeaMowerControl",
			isGameRunning = false,
			isInit = true,
			levelConfig = new TowerDefenseLevelConfig()
		};
		AddChild(_control, forceReadableName: false, InternalMode.Disabled);
		_control.characterNode = new Node2D
		{
			Name = "CharacterNode"
		};
		_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
		manager.currentControl = _control;
		manager.gridBeginPos = map.gridBeginPos;
		manager.gridSize = map.gridSize;
		manager.gridNum = map.gridNum;
		_mapControl = new TowerDefenseMapControl
		{
			Name = "DetachedVampireMapControl"
		};
		_mapFeature = new TowerDefenseBattleFeatureMap
		{
			control = _control,
			mapControl = _mapControl,
			config = map,
			mapConfig = map
		};
		_mapControl.mapFeature = _mapFeature;
		_mapFeature.PlantGridInit();
		_mowerFeature = new TowerDefenseBattleFeatureMower
		{
			control = _control,
			config = new TowerDefenseBattleFeatureMowerConfig
			{
				mowerPacketName = "MowerDefault"
			}
		};
		_mowerFeature.mowerLine.Resize(51);
		_mowerFeature.targetZombieLine.Resize(51);
		_control.featureDictionary[new StringName("Map")] = _mapFeature;
		_control.featureDictionary[new StringName("Mower")] = _mowerFeature;
	}

	private static bool HasMowerContract(TowerDefenseLevelConfig level)
	{
		if (GodotObject.IsInstanceValid(level) && level.processData.GetValueOrDefault("MowerUse", false).AsBool())
		{
			return level.featureData.ContainsKey(new StringName("Mower"));
		}
		return false;
	}

	private static TowerDefenseLevelPacketConfig FindWildPeaFireUpgrade(TowerDefenseLevelConfig level)
	{
		if (!GodotObject.IsInstanceValid(level))
		{
			return null;
		}
		HashSet<ulong> visited = new HashSet<ulong>();
		foreach (Variant packetBank in level.packetBankList)
		{
			TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = FindWildPeaFireUpgrade(packetBank.AsGodotObject() as TowerDefenseLevelPacketConfig, visited);
			if (GodotObject.IsInstanceValid(towerDefenseLevelPacketConfig))
			{
				return towerDefenseLevelPacketConfig;
			}
		}
		return null;
	}

	private static TowerDefenseLevelPacketConfig FindWildPeaFireUpgrade(TowerDefenseLevelPacketConfig packet, HashSet<ulong> visited)
	{
		if (!GodotObject.IsInstanceValid(packet) || !visited.Add(packet.GetInstanceId()))
		{
			return null;
		}
		if (packet.packetName == "PlantWildPea" && HasPropertyValue(packet.@override?.characterOverride, "projectileName", (Variant value) => value.AsString() == "FirePea"))
		{
			return packet;
		}
		if (!GodotObject.IsInstanceValid(packet.@override))
		{
			return null;
		}
		foreach (CardActionBehaviorDefinition useSucceededAction in packet.@override.useSucceededActions)
		{
			if (useSucceededAction is CardActionBehaviorChangePacket cardActionBehaviorChangePacket)
			{
				TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = FindWildPeaFireUpgrade(cardActionBehaviorChangePacket.levelPacketConfig, visited);
				if (GodotObject.IsInstanceValid(towerDefenseLevelPacketConfig))
				{
					return towerDefenseLevelPacketConfig;
				}
			}
		}
		return null;
	}

	private static bool HasPropertyValue(TowerDefenseCharacterOverride characterOverride, string propertyName, Func<Variant, bool> predicate)
	{
		if (!GodotObject.IsInstanceValid(characterOverride))
		{
			return false;
		}
		foreach (TowerDefenseCharacterPropertyChangeConfig item in characterOverride.propertyChange)
		{
			if (GodotObject.IsInstanceValid(item) && item.propertyName == propertyName)
			{
				return predicate(item.value);
			}
		}
		return false;
	}

	private bool AllRealCellsExist()
	{
		for (int i = 1; i <= 9; i++)
		{
			for (int j = 1; j <= 5; j++)
			{
				if (!GodotObject.IsInstanceValid(TowerDefenseManager.GetMapCell(new Vector2I(i, j))))
				{
					return false;
				}
			}
		}
		return true;
	}

	private int CountLiveMowers()
	{
		int num = 0;
		for (int i = 1; i <= 5; i++)
		{
			if (GodotObject.IsInstanceValid(_mowerFeature.mowerLine[i]))
			{
				num++;
			}
		}
		return num;
	}

	private bool AllMowersMatchProductionContract()
	{
		for (int i = 1; i <= 5; i++)
		{
			TowerDefenseMower towerDefenseMower = _mowerFeature.mowerLine[i];
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(new Vector2I(1, i));
			Vector2 to = TowerDefenseManager.GetMapCellPlantPos(new Vector2I(0, i)) + new Vector2(10f, 0f);
			if (!(towerDefenseMower is TowerDefenseMowerDefault) || !GodotObject.IsInstanceValid(mapCell) || towerDefenseMower.gridPos != new Vector2I(0, i) || towerDefenseMower.packet?.saveKey != "MowerDefault" || towerDefenseMower.config?.name != "MowerDefault" || !towerDefenseMower.characterFilter || towerDefenseMower.GlobalPosition.DistanceTo(to) > 0.01f || Math.Abs(towerDefenseMower.groundHeight - mapCell.GetGroundHeight(0.0)) > 0.001)
			{
				return false;
			}
		}
		return true;
	}

	private void RegisterRealFixtures()
	{
		SnapshotPacketCache();
		RegisterPacket("PlantWildPea", "res://Asset/Anime/Character/Plant/Gold/WildPea/Packet/PlantWildPea.tres");
		RegisterCharacter("PlantWildPea", "res://Asset/Anime/Character/Plant/Gold/WildPea/Scene/TowerDefensePlantWildPea.tscn");
		RegisterPacket("MowerDefault", "res://Asset/Anime/Character/Mower/Default/Packet/MowerDefault.tres");
		RegisterCharacter("MowerDefault", "res://Asset/Anime/Character/Mower/Default/Scene/TowerDefenseMowerDefault.tscn");
	}

	private void SnapshotPacketCache()
	{
		_packetCache = typeof(TowerDefenseManager).GetField("_packetConfigRefCache", BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null) as Dictionary<string, TowerDefensePacketConfig>;
		if (_packetCache != null)
		{
			_hadMowerPacketCache = _packetCache.TryGetValue("MowerDefault", out _previousMowerPacketCache);
			_packetCache.Remove("MowerDefault");
		}
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
		instance.TOWERDEFENSE_PACKETS[key] = Load<TowerDefensePacketConfig>(path);
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
		instance.TOWERDEFENSE_CHARCATERS[key] = Load<PackedScene>(path);
	}

	private void RestoreRealFixtures()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
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
		if (_packetCache != null)
		{
			if (_hadMowerPacketCache)
			{
				_packetCache["MowerDefault"] = _previousMowerPacketCache;
			}
			else
			{
				_packetCache.Remove("MowerDefault");
			}
		}
	}

	private static T Load<T>(string path) where T : Resource
	{
		return ResourceLoader.Load<T>(path, null, ResourceLoader.CacheMode.Ignore);
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
			GD.PushError("[BugOverviewShootingLevel11WildPeaMowerRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(13)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.SetupBattleFixture, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "map", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.HasMowerContract, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "level", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.FindWildPeaFireUpgrade, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "level", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.AllRealCellsExist, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CountLiveMowers, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.AllMowersMatchProductionContract, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterRealFixtures, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.SnapshotPacketCache, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterPacket, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterCharacter, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RestoreRealFixtures, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SetupBattleFixture && args.Count == 2)
		{
			SetupBattleFixture(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasMowerContract && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasMowerContract(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FindWildPeaFireUpgrade && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelPacketConfig>(FindWildPeaFireUpgrade(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.AllRealCellsExist && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(AllRealCellsExist());
			return true;
		}
		if (method == MethodName.CountLiveMowers && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountLiveMowers());
			return true;
		}
		if (method == MethodName.AllMowersMatchProductionContract && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(AllMowersMatchProductionContract());
			return true;
		}
		if (method == MethodName.RegisterRealFixtures && args.Count == 0)
		{
			RegisterRealFixtures();
			ret = default;
			return true;
		}
		if (method == MethodName.SnapshotPacketCache && args.Count == 0)
		{
			SnapshotPacketCache();
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
		if (method == MethodName.HasMowerContract && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasMowerContract(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FindWildPeaFireUpgrade && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseLevelPacketConfig>(FindWildPeaFireUpgrade(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0])));
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
		if (method == MethodName.SetupBattleFixture)
		{
			return true;
		}
		if (method == MethodName.HasMowerContract)
		{
			return true;
		}
		if (method == MethodName.FindWildPeaFireUpgrade)
		{
			return true;
		}
		if (method == MethodName.AllRealCellsExist)
		{
			return true;
		}
		if (method == MethodName.CountLiveMowers)
		{
			return true;
		}
		if (method == MethodName.AllMowersMatchProductionContract)
		{
			return true;
		}
		if (method == MethodName.RegisterRealFixtures)
		{
			return true;
		}
		if (method == MethodName.SnapshotPacketCache)
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
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<ShootingLevel11WildPeaMowerControlStub>(in value);
			return true;
		}
		if (name == PropertyName._mapFeature)
		{
			_mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName._mowerFeature)
		{
			_mowerFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMower>(in value);
			return true;
		}
		if (name == PropertyName._mapControl)
		{
			_mapControl = VariantUtils.ConvertTo<TowerDefenseMapControl>(in value);
			return true;
		}
		if (name == PropertyName._wildPea)
		{
			_wildPea = VariantUtils.ConvertTo<TowerDefensePlantWildPea>(in value);
			return true;
		}
		if (name == PropertyName._previousMowerPacketCache)
		{
			_previousMowerPacketCache = VariantUtils.ConvertTo<TowerDefensePacketConfig>(in value);
			return true;
		}
		if (name == PropertyName._hadMowerPacketCache)
		{
			_hadMowerPacketCache = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._mapFeature)
		{
			value = VariantUtils.CreateFrom(in _mapFeature);
			return true;
		}
		if (name == PropertyName._mowerFeature)
		{
			value = VariantUtils.CreateFrom(in _mowerFeature);
			return true;
		}
		if (name == PropertyName._mapControl)
		{
			value = VariantUtils.CreateFrom(in _mapControl);
			return true;
		}
		if (name == PropertyName._wildPea)
		{
			value = VariantUtils.CreateFrom(in _wildPea);
			return true;
		}
		if (name == PropertyName._previousMowerPacketCache)
		{
			value = VariantUtils.CreateFrom(in _previousMowerPacketCache);
			return true;
		}
		if (name == PropertyName._hadMowerPacketCache)
		{
			value = VariantUtils.CreateFrom(in _hadMowerPacketCache);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._mowerFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._wildPea, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._previousMowerPacketCache, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._hadMowerPacketCache, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._mapFeature, Variant.From(in _mapFeature));
		info.AddProperty(PropertyName._mowerFeature, Variant.From(in _mowerFeature));
		info.AddProperty(PropertyName._mapControl, Variant.From(in _mapControl));
		info.AddProperty(PropertyName._wildPea, Variant.From(in _wildPea));
		info.AddProperty(PropertyName._previousMowerPacketCache, Variant.From(in _previousMowerPacketCache));
		info.AddProperty(PropertyName._hadMowerPacketCache, Variant.From(in _hadMowerPacketCache));
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
			_control = value3.As<ShootingLevel11WildPeaMowerControlStub>();
		}
		if (info.TryGetProperty(PropertyName._mapFeature, out var value4))
		{
			_mapFeature = value4.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName._mowerFeature, out var value5))
		{
			_mowerFeature = value5.As<TowerDefenseBattleFeatureMower>();
		}
		if (info.TryGetProperty(PropertyName._mapControl, out var value6))
		{
			_mapControl = value6.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName._wildPea, out var value7))
		{
			_wildPea = value7.As<TowerDefensePlantWildPea>();
		}
		if (info.TryGetProperty(PropertyName._previousMowerPacketCache, out var value8))
		{
			_previousMowerPacketCache = value8.As<TowerDefensePacketConfig>();
		}
		if (info.TryGetProperty(PropertyName._hadMowerPacketCache, out var value9))
		{
			_hadMowerPacketCache = value9.As<bool>();
		}
	}
}
