using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewGhost2ImpStackedPlantTwitchRuntimeTest.cs")]
public class BugOverviewGhost2ImpStackedPlantTwitchRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName UsesBackyardNight = "UsesBackyardNight";

		public static readonly StringName HasPresetPackets = "HasPresetPackets";

		public static readonly StringName SpawnsZombie = "SpawnsZombie";

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

	private const string LevelPath = "res://Asset/Config/Level/TowerDefense/Challenge/Diamond/Challenge_Level_Diamond12_2.tres";

	private const string MapPath = "res://Asset/Config/Map/Backyard/Config/BackyardMapBackyardNight.tres";

	private const string SunPadPacketPath = "res://Asset/Anime/Character/Plant/Chapter3/SunPad/Packet/PlantSunPad.tres";

	private const string SunPadScenePath = "res://Asset/Anime/Character/Plant/Chapter3/SunPad/Scene/TowerDefensePlantSunPad.tscn";

	private const string SunFlowerZPacketPath = "res://Asset/Anime/Character/Plant/Chapter8/SunFlowerZ/Packet/PlantSunFlowerZ.tres";

	private const string SunFlowerZScenePath = "res://Asset/Anime/Character/Plant/Chapter8/SunFlowerZ/Scene/TowerDefensePlantSunFlowerZ.tscn";

	private const string ImpPacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/Base/ZombieImp.tres";

	private const string ImpScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Scene/Base/TowerDefenseZombieImp.tscn";

	private static readonly Vector2I ReportedCell = new Vector2I(1, 4);

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
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		BugOverviewGhost2ImpStackedPlantTwitchControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		try
		{
			_ = 2;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_00ce;
				}
				TowerDefenseLevelConfig towerDefenseLevelConfig = ResourceLoader.Load<TowerDefenseLevelConfig>("res://Asset/Config/Level/TowerDefense/Challenge/Diamond/Challenge_Level_Diamond12_2.tres", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(towerDefenseLevelConfig) && towerDefenseLevelConfig.name == "Challenge_Level_Diamond12_2", "The fixture must load the real Ghost Hypno-shroom Challenge 2 level.");
				Check(UsesBackyardNight(towerDefenseLevelConfig), "Ghost Hypno-shroom Challenge 2 must still use the BackyardNight map.");
				Check(HasPresetPackets(towerDefenseLevelConfig, "PlantSunPad", "PlantSunFlowerZ"), "The real challenge preset must contain Sun Pad and Zombie Sunflower.");
				Check(SpawnsZombie(towerDefenseLevelConfig, "ZombieImp"), "The real challenge waves must contain the reported Imp.");
				TowerDefenseMapConfig towerDefenseMapConfig = ResourceLoader.Load<TowerDefenseMapConfig>("res://Asset/Config/Map/Backyard/Config/BackyardMapBackyardNight.tres", null, ResourceLoader.CacheMode.Ignore);
				Check(GodotObject.IsInstanceValid(towerDefenseMapConfig) && towerDefenseMapConfig.gridNum == new Vector2I(9, 6) && towerDefenseMapConfig.gridBeginPos == new Vector2(260f, 60f) && towerDefenseMapConfig.gridSize == new Vector2(78f, 85f), "The fixture must use the real BackyardNight 9x6 grid geometry.");
				TowerDefenseCellConfig towerDefenseCellConfig = towerDefenseMapConfig?.GetEffectiveCellConfig(ReportedCell.X, ReportedCell.Y);
				Check(GodotObject.IsInstanceValid(towerDefenseCellConfig) && towerDefenseCellConfig.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.WATER), "Reported row 4 column 1 must be a real BackyardNight water cell.");
				if (!GodotObject.IsInstanceValid(towerDefenseLevelConfig) || !GodotObject.IsInstanceValid(towerDefenseMapConfig) || !GodotObject.IsInstanceValid(towerDefenseCellConfig))
				{
					goto end_IL_00ce;
				}
				RegisterRealFixtures();
				control = new BugOverviewGhost2ImpStackedPlantTwitchControlStub
				{
					Name = "Ghost2ImpStackedPlantTwitchControl",
					isGameRunning = false,
					isInit = true,
					levelConfig = towerDefenseLevelConfig
				};
				AddChild(control, forceReadableName: false, InternalMode.Disabled);
				control.characterNode = new Node2D
				{
					Name = "CharacterNode"
				};
				control.AddChild(control.characterNode, forceReadableName: false, InternalMode.Disabled);
				manager.currentControl = control;
				manager.gridBeginPos = towerDefenseMapConfig.gridBeginPos;
				manager.gridSize = towerDefenseMapConfig.gridSize;
				manager.gridNum = towerDefenseMapConfig.gridNum;
				mapControl = new TowerDefenseMapControl
				{
					Name = "BackyardNightMapControl"
				};
				mapFeature = CreateMapFeature(mapControl, towerDefenseMapConfig);
				mapFeature.control = control;
				control.featureDictionary[new StringName("Map")] = mapFeature;
				TowerDefensePacketConfig towerDefensePacketConfig = LoadPacket("res://Asset/Anime/Character/Plant/Chapter3/SunPad/Packet/PlantSunPad.tres");
				TowerDefensePacketConfig sunFlowerZPacket = LoadPacket("res://Asset/Anime/Character/Plant/Chapter8/SunFlowerZ/Packet/PlantSunFlowerZ.tres");
				TowerDefensePacketConfig impPacket = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/Base/ZombieImp.tres");
				TowerDefensePlantSunPad sunPad = towerDefensePacketConfig?.Plant(ReportedCell, playAudio: false) as TowerDefensePlantSunPad;
				await WaitFrames(4);
				TowerDefensePlantSunFlowerZ sunFlowerZ = sunFlowerZPacket?.Plant(ReportedCell, playAudio: false) as TowerDefensePlantSunFlowerZ;
				await WaitFrames(4);
				TowerDefenseZombieImp imp = impPacket?.Plant(ReportedCell, playAudio: false) as TowerDefenseZombieImp;
				await WaitFrames(5);
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(ReportedCell);
				Check(GodotObject.IsInstanceValid(sunPad) && GodotObject.IsInstanceValid(sunFlowerZ) && GodotObject.IsInstanceValid(imp), "The real Sun Pad, Zombie Sunflower and Imp scenes must instantiate.");
				Check(GodotObject.IsInstanceValid(mapCell) && sunPad.cell == mapCell && sunFlowerZ.cell == mapCell && mapCell.characterSlotDictionary.TryGetValue(sunPad, out var value) && value == sunFlowerZ, "Zombie Sunflower must occupy the real Sun Pad slot in the reported cell.");
				if (!GodotObject.IsInstanceValid(sunPad) || !GodotObject.IsInstanceValid(sunFlowerZ) || !GodotObject.IsInstanceValid(imp) || !GodotObject.IsInstanceValid(mapCell))
				{
					goto end_IL_00ce;
				}
				sunPad.ProcessMode = ProcessModeEnum.Disabled;
				sunFlowerZ.ProcessMode = ProcessModeEnum.Disabled;
				imp.ProcessMode = ProcessModeEnum.Disabled;
				control.isGameRunning = true;
				AttackComponent attackComponent = imp.componentManager?.GetRuntime<AttackComponent>("character.attack.0");
				Check(attackComponent != null && !attackComponent.IsReleased && imp.attackComponent == attackComponent, "The real Imp primary bite component must be active.");
				Check(imp.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE && sunPad.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT && imp.HasHitBox && sunPad.HasHitBox && sunFlowerZ.HasHitBox, "The real opposing camps and hit boxes must be registered.");
				if (attackComponent == null || attackComponent.IsReleased)
				{
					goto end_IL_00ce;
				}
				imp.groundRight = 2000.0;
				imp.useAttackDps = false;
				attackComponent.groundRight = 2000.0;
				attackComponent.alive = true;
				attackComponent.timer = 0.0;
				attackComponent.checkIntrevalNow = 0;
				attackComponent.target = sunPad;
				sunFlowerZ.nearDie = true;
				sunFlowerZ.die = true;
				Check(mapCell.characterList.Contains(sunFlowerZ) && GodotObject.IsInstanceValid(sunFlowerZ) && sunFlowerZ.IsInsideTree(), "The terminal upper plant must remain in the cell during its death animation.");
				TowerDefenseCharacter target = mapCell.GetTarget(imp.instance.collisionFlags, imp.camp);
				Check(target == sunFlowerZ, "The real cell-priority lookup must expose the terminal upper plant that triggered the old bug.");
				Check(attackComponent.CanAttack(), "Imp must keep attacking its already validated live Sun Pad target.");
				Check(attackComponent.target == sunPad, "Cell-priority resolution must not replace the cached Sun Pad with terminal Zombie Sunflower.");
				imp.Attack();
				Check(imp.CurrentStateHandle?.StableId == "zombie.attack", "Imp must enter attack before the continuity loop; current=" + imp.CurrentStateHandle?.StableId + ".");
				for (int i = 0; i < 12; i++)
				{
					imp.AttackProcessing(1.0 / 60.0);
					Check(attackComponent.target == sunPad, $"Bite target changed away from live Sun Pad at continuity step {i}.");
					Check(imp.CurrentStateHandle?.StableId == "zombie.attack", $"Imp twitched back to walking at continuity step {i}; current={imp.CurrentStateHandle?.StableId}.");
				}
				goto end_IL_00b7;
				end_IL_00ce:;
			}
			catch (Exception value2)
			{
				_failures++;
				GD.PushError($"[BugOverviewGhost2ImpStackedPlantTwitchRuntimeTest] Unexpected exception: {value2}");
				goto end_IL_00b7;
			}
			return;
			end_IL_00b7:;
		}
		finally
		{
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
			await WaitFrames(3);
		}
		bool flag = _failures == 0 && _checks == 41;
		GD.Print($"GHOST2_IMP_STACKED_PLANT_TWITCH_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private static bool UsesBackyardNight(TowerDefenseLevelConfig level)
	{
		if (GodotObject.IsInstanceValid(level) && level.featureData.TryGetValue(new StringName("Map"), out var value))
		{
			return value.GetValueOrDefault("MapName", "").AsString() == "BackyardNight";
		}
		return false;
	}

	private static bool HasPresetPackets(TowerDefenseLevelConfig level, params string[] names)
	{
		if (!GodotObject.IsInstanceValid(level))
		{
			return false;
		}
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		foreach (Variant packetBank in level.packetBankList)
		{
			if (packetBank.VariantType == Variant.Type.Object && packetBank.AsGodotObject() is TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig)
			{
				hashSet.Add(towerDefenseLevelPacketConfig.packetName);
			}
		}
		foreach (string item in names)
		{
			if (!hashSet.Contains(item))
			{
				return false;
			}
		}
		return true;
	}

	private static bool SpawnsZombie(TowerDefenseLevelConfig level, string name)
	{
		if (!GodotObject.IsInstanceValid(level))
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(level.waveManager))
		{
			foreach (TowerDefenseLevelWaveConfig item in level.waveManager.wave)
			{
				if (!GodotObject.IsInstanceValid(item))
				{
					continue;
				}
				foreach (TowerDefenseLevelSpawnConfig item2 in item.spawn)
				{
					if (GodotObject.IsInstanceValid(item2) && item2.zombie == name)
					{
						return true;
					}
				}
			}
		}
		if (!level.featureData.TryGetValue(new StringName("Wave"), out var value))
		{
			return false;
		}
		foreach (Variant item3 in value.GetValueOrDefault("Wave", new Godot.Collections.Array()).AsGodotArray())
		{
			if (item3.VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			foreach (Variant item4 in item3.AsGodotDictionary().GetValueOrDefault("Spawn", new Godot.Collections.Array()).AsGodotArray())
			{
				if (item4.VariantType == Variant.Type.Dictionary && item4.AsGodotDictionary().GetValueOrDefault("Zombie", "").AsString() == name)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, TowerDefenseMapConfig mapConfig)
	{
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = mapConfig
		});
		towerDefenseBattleFeatureMap.plantGrid.Resize(mapConfig.gridNum.X + 1);
		for (int i = 0; i <= mapConfig.gridNum.X; i++)
		{
			Godot.Collections.Array array = new Godot.Collections.Array();
			array.Resize(mapConfig.gridNum.Y + 1);
			for (int j = 1; j <= mapConfig.gridNum.Y; j++)
			{
				TowerDefenseCellConfig config = ((i == 0) ? new TowerDefenseCellConfig() : (mapConfig.GetEffectiveCellConfig(i, j) ?? new TowerDefenseCellConfig()));
				TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance
				{
					gridPos = new Vector2I(i, j)
				};
				towerDefenseCellInstance.Init(config);
				array[j] = towerDefenseCellInstance;
			}
			towerDefenseBattleFeatureMap.plantGrid[i] = array;
		}
		towerDefenseBattleFeatureMap.iceCapList.Resize(mapConfig.gridNum.Y + 1);
		return towerDefenseBattleFeatureMap;
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private void RegisterRealFixtures()
	{
		RegisterPacket("PlantSunPad", "res://Asset/Anime/Character/Plant/Chapter3/SunPad/Packet/PlantSunPad.tres");
		RegisterPacket("PlantSunFlowerZ", "res://Asset/Anime/Character/Plant/Chapter8/SunFlowerZ/Packet/PlantSunFlowerZ.tres");
		RegisterPacket("ZombieImp", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/Base/ZombieImp.tres");
		RegisterCharacter("PlantSunPad", "res://Asset/Anime/Character/Plant/Chapter3/SunPad/Scene/TowerDefensePlantSunPad.tscn");
		RegisterCharacter("PlantSunFlowerZ", "res://Asset/Anime/Character/Plant/Chapter8/SunFlowerZ/Scene/TowerDefensePlantSunFlowerZ.tscn");
		RegisterCharacter("ZombieImp", "res://Asset/Anime/Character/Zombie/Chapter1/Imp/Scene/Base/TowerDefenseZombieImp.tscn");
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
			GD.PushError("[BugOverviewGhost2ImpStackedPlantTwitchRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UsesBackyardNight, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "level", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasPresetPackets, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "level", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "names", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnsZombie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "level", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateMapFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "mapConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.UsesBackyardNight && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(UsesBackyardNight(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.HasPresetPackets && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPresetPackets(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.SpawnsZombie && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SpawnsZombie(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[1])));
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
		if (method == MethodName.UsesBackyardNight && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(UsesBackyardNight(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.HasPresetPackets && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPresetPackets(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.SpawnsZombie && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SpawnsZombie(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateMapFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeatureMap>(CreateMapFeature(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[1])));
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
		if (method == MethodName.UsesBackyardNight)
		{
			return true;
		}
		if (method == MethodName.HasPresetPackets)
		{
			return true;
		}
		if (method == MethodName.SpawnsZombie)
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
