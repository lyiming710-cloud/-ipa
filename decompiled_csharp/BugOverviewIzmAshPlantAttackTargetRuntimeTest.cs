using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewIzmAshPlantAttackTargetRuntimeTest.cs")]
public class BugOverviewIzmAshPlantAttackTargetRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateMapFeature = "CreateMapFeature";

		public static readonly StringName LoadPacket = "LoadPacket";

		public static readonly StringName RefreshEncounterRegistration = "RefreshEncounterRegistration";

		public static readonly StringName IsZero = "IsZero";

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

	private const string JokerPacketPath = "res://Asset/Anime/Character/Plant/Other/JalaJoker/Packet/PlantJalaJoker.tres";

	private const string JokerScenePath = "res://Asset/Anime/Character/Plant/Other/JalaJoker/Scene/TowerDefensePlantJalaJoker.tscn";

	private const string DisguiserCherryPacketPath = "res://Asset/Anime/Character/Plant/Star/DisguiserCherry/Packet/PlantDisguiserCherry.tres";

	private const string DisguiserCherryScenePath = "res://Asset/Anime/Character/Plant/Star/DisguiserCherry/Scene/TowerDefensePlantDisguiserCherry.tscn";

	private const string ZombiePacketPath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres";

	private const string ZombieScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private static readonly Vector2I PlantGrid = new Vector2I(3, 2);

	private static readonly Vector2I ZombieGrid = new Vector2I(4, 2);

	private static readonly Vector2I DisguiserCherryGrid = new Vector2I(3, 5);

	private static readonly Vector2I DisguiserCherryZombieGrid = new Vector2I(4, 5);

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousPackets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _previousCharacters = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly HashSet<string> _missingPackets = new HashSet<string>();

	private readonly HashSet<string> _missingCharacters = new HashSet<string>();

	private int _checks;

	private int _failures;

	public override async void _Ready()
	{
		await ResourceManager.Instance.EnsureFullGameplayResourcesReadyAsync();
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		BugOverviewIzmAshPlantAttackTargetRuntimeControlStub control = null;
		TowerDefenseMapControl mapControl = null;
		TowerDefenseBattleFeatureMap mapFeature = null;
		TowerDefenseBattleProcessIZM izmProcess = null;
		try
		{
			int num;
			_ = num - 1;
			_ = 9;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "TowerDefenseManager autoload must be available.");
				Check(GodotObject.IsInstanceValid(ResourceManager.Instance), "ResourceManager autoload must be available.");
				if (!GodotObject.IsInstanceValid(manager) || !GodotObject.IsInstanceValid(ResourceManager.Instance))
				{
					goto end_IL_0145;
				}
				RegisterRealFixtures();
				control = new BugOverviewIzmAshPlantAttackTargetRuntimeControlStub
				{
					Name = "IzmAshPlantAttackTargetRuntimeControl",
					isGameRunning = true,
					isInit = true,
					levelConfig = new TowerDefenseLevelConfig
					{
						finishMethod = TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM,
						izmManager = new TowerDefenseLevelIZMManagerConfig()
					}
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
				izmProcess = new TowerDefenseBattleProcessIZM
				{
					control = control,
					mapFeature = mapFeature
				};
				izmProcess.Init(new Dictionary());
				control.process = izmProcess;
				Check(TowerDefenseManager.GetGameMethod() == TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM, "The fixture must use the production IZM finish method.");
				Check(control.process == izmProcess && izmProcess.GetType() == typeof(TowerDefenseBattleProcessIZM), "The fixture must own a real TowerDefenseBattleProcessIZM.");
				TowerDefensePlantJalaJoker joker = LoadPacket("res://Asset/Anime/Character/Plant/Other/JalaJoker/Packet/PlantJalaJoker.tres")?.Plant(PlantGrid, playAudio: false) as TowerDefensePlantJalaJoker;
				await WaitFrames(8);
				TowerDefenseZombieNormal zombie = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres")?.Plant(ZombieGrid, playAudio: false) as TowerDefenseZombieNormal;
				await WaitFrames(8);
				Check(GodotObject.IsInstanceValid(joker) && GodotObject.IsInstanceValid(zombie), "Real JalaJoker and ZombieNormal scenes must spawn on the IZM map.");
				if (!GodotObject.IsInstanceValid(joker) || !GodotObject.IsInstanceValid(zombie))
				{
					goto end_IL_0145;
				}
				joker.inGame = true;
				zombie.inGame = true;
				zombie.ProcessMode = ProcessModeEnum.Disabled;
				zombie.GlobalPosition = new Vector2(joker.GlobalPosition.X + 35f, joker.GlobalPosition.Y);
				zombie.gridPos = PlantGrid;
				RefreshEncounterRegistration(manager, joker, zombie);
				await WaitFrames(3);
				ExplodeComponent explode = joker.componentManager?.GetRuntime<ExplodeComponent>("character.explode");
				AttackComponent attack = zombie.componentManager?.GetRuntime<AttackComponent>("character.attack.0");
				Check(explode != null && !explode.IsReleased && attack != null && !attack.IsReleased, "The real characters must expose their authored Explode and Attack runtimes.");
				if (explode == null || explode.IsReleased || attack == null || attack.IsReleased)
				{
					goto end_IL_0145;
				}
				Check(await WaitUntil(() => explode.StateMachine?.CurrentStateHandle?.StableId == "explode.explode", 40) && explode.izmMode && !explode.isHurt, $"The pre-placed ash plant must enter the IZM waiting explosion state; state={explode.StateMachine?.CurrentStateHandle?.StableId}, izm={explode.izmMode}, hurt={explode.isHurt}.");
				Check(!joker.instance.invincible, "An unhurt IZM ash plant must remain targetable while its explosion animation is waiting.");
				Check(IsZero(explode.sprite.timeScale), $"The real JalaJoker explosion animation must wait for the first bite; speed={explode.sprite.timeScale}.");
				bool flag = attack.CanAttackOnce();
				Check(flag && attack.target == joker, "The real ZombieNormal AttackComponent must acquire the waiting JalaJoker.");
				double hitpointsBeforeBite = joker.instance.hitpoints;
				attack.AttackExecute(1.0);
				await WaitFrames(2);
				Check(joker.instance.hitpoints < hitpointsBeforeBite, "The first real zombie bite must damage the targetable JalaJoker.");
				Check(explode.isHurt, "The first real bite must release the IZM ash explosion wait gate.");
				Check(!joker.instance.invincible && explode.ProtectsFromBites, "首口啃咬后只启用灰烬的啃咬保护，爆炸动画期间仍允许子弹造成伤害。");
				Check(explode.sprite.timeScale > 0.0, $"After the first bite, the real explosion animation must resume; speed={explode.sprite.timeScale}.");
				TowerDefensePlantDisguiserCherry disguiserCherry = LoadPacket("res://Asset/Anime/Character/Plant/Star/DisguiserCherry/Packet/PlantDisguiserCherry.tres")?.Plant(DisguiserCherryGrid, playAudio: false) as TowerDefensePlantDisguiserCherry;
				await WaitFrames(8);
				TowerDefenseZombieNormal disguiserCherryZombie = LoadPacket("res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres")?.Plant(DisguiserCherryZombieGrid, playAudio: false) as TowerDefenseZombieNormal;
				await WaitFrames(8);
				Check(GodotObject.IsInstanceValid(disguiserCherry) && GodotObject.IsInstanceValid(disguiserCherryZombie), "Real DisguiserCherry and its ZombieNormal eater must spawn on the IZM map.");
				if (!GodotObject.IsInstanceValid(disguiserCherry) || !GodotObject.IsInstanceValid(disguiserCherryZombie))
				{
					goto end_IL_0145;
				}
				disguiserCherry.inGame = true;
				disguiserCherryZombie.inGame = true;
				disguiserCherryZombie.ProcessMode = ProcessModeEnum.Disabled;
				disguiserCherryZombie.GlobalPosition = new Vector2(disguiserCherry.GlobalPosition.X + 35f, disguiserCherry.GlobalPosition.Y);
				disguiserCherryZombie.gridPos = DisguiserCherryGrid;
				RefreshEncounterRegistration(manager, disguiserCherry, disguiserCherryZombie);
				await WaitFrames(3);
				ExplodeComponent passiveExplode = disguiserCherry.componentManager?.GetRuntime<ExplodeComponent>("character.explode");
				AttackComponent disguiserAttack = disguiserCherryZombie.componentManager?.GetRuntime<AttackComponent>("character.attack.0");
				Check(passiveExplode != null && !passiveExplode.IsReleased && disguiserAttack != null && !disguiserAttack.IsReleased, "DisguiserCherry and its eater must expose their authored component runtimes.");
				if (passiveExplode == null || passiveExplode.IsReleased || disguiserAttack == null || disguiserAttack.IsReleased)
				{
					goto end_IL_0145;
				}
				Check(passiveExplode.izmMode && !passiveExplode.reload && passiveExplode.StateMachine?.CurrentStateHandle?.StableId == "explode.idle", $"DisguiserCherry must use its explosion component passively in IZM; state={passiveExplode.StateMachine?.CurrentStateHandle?.StableId}, izm={passiveExplode.izmMode}, reload={passiveExplode.reload}.");
				bool flag2 = disguiserAttack.CanAttackOnce();
				Check(flag2 && disguiserAttack.target == disguiserCherry, "ZombieNormal must acquire DisguiserCherry before the first bite.");
				double disguiserHpBeforeFirstBite = disguiserCherry.instance.hitpoints;
				disguiserAttack.AttackExecute(1.0);
				await WaitFrames(2);
				Check(disguiserCherry.instance.hitpoints < disguiserHpBeforeFirstBite, "The first bite must damage DisguiserCherry.");
				Check(passiveExplode.isHurt && !disguiserCherry.instance.invincible, "A passive IZM explosion must record the hit without making DisguiserCherry invincible.");
				bool flag3 = disguiserAttack.TryRetargetImmediately();
				Check(flag3 && disguiserAttack.target == disguiserCherry, "ZombieNormal must reacquire DisguiserCherry after the first bite.");
				double disguiserHpBeforeSecondBite = disguiserCherry.instance.hitpoints;
				disguiserAttack.AttackExecute(1.0);
				await WaitFrames(2);
				Check(disguiserCherry.instance.hitpoints < disguiserHpBeforeSecondBite && !disguiserCherry.instance.invincible, "The second bite must keep damaging the still-edible DisguiserCherry.");
				goto end_IL_010d;
				end_IL_0145:;
			}
			catch (Exception value)
			{
				_failures++;
				GD.PushError($"[BugOverviewIzmAshPlantAttackTargetRuntimeTest] Unexpected exception: {value}");
				goto end_IL_010d;
			}
			return;
			end_IL_010d:;
		}
		finally
		{
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.currentControl = previousControl;
			}
			izmProcess?.Destroy();
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
		bool flag4 = _failures == 0 && _checks == 22;
		GD.Print($"BUG_OVERVIEW_IZM_ASH_PLANT_ATTACK_TARGET_RESULT passed={flag4} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag4) ? 2 : 0);
	}

	private static TowerDefenseBattleFeatureMap CreateMapFeature(TowerDefenseMapControl mapControl, Vector2I gridNum)
	{
		TowerDefenseMapConfig towerDefenseMapConfig = new TowerDefenseMapConfig
		{
			isNight = true,
			gridNum = gridNum,
			gridBeginPos = Vector2.Zero,
			gridSize = new Vector2(100f, 76f),
			plantOffset = 50.0
		};
		towerDefenseMapConfig.cellConfig.Add(new TowerDefenseCellConfig
		{
			pos = new Vector4I(1, 1, gridNum.X, gridNum.Y)
		});
		for (int i = 1; i <= gridNum.Y; i++)
		{
			towerDefenseMapConfig.lineUse.Add(i);
		}
		TowerDefenseBattleFeatureMap towerDefenseBattleFeatureMap = (mapControl.mapFeature = new TowerDefenseBattleFeatureMap
		{
			mapControl = mapControl,
			config = towerDefenseMapConfig,
			mapConfig = towerDefenseMapConfig
		});
		towerDefenseBattleFeatureMap.PlantGridInit();
		return towerDefenseBattleFeatureMap;
	}

	private static TowerDefensePacketConfig LoadPacket(string path)
	{
		return ResourceLoader.Load<TowerDefensePacketConfig>(path, null, ResourceLoader.CacheMode.Ignore)?.Duplicate(deep: true) as TowerDefensePacketConfig;
	}

	private static void RefreshEncounterRegistration(TowerDefenseManager manager, TowerDefenseCharacter plant, TowerDefenseCharacter zombie)
	{
		manager.CharacterUnregister(plant);
		manager.CharacterUnregister(zombie);
		manager.CharacterRegister(plant);
		manager.CharacterRegister(zombie);
	}

	private static bool IsZero(double value)
	{
		return Math.Abs(value) < 1E-06;
	}

	private async Task<bool> WaitUntil(Func<bool> predicate, int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (predicate())
			{
				return true;
			}
			await WaitFrames(1);
		}
		return predicate();
	}

	private async Task WaitFrames(int count)
	{
		for (int frame = 0; frame < count; frame++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	private void RegisterRealFixtures()
	{
		RegisterPacket("PlantJalaJoker", "res://Asset/Anime/Character/Plant/Other/JalaJoker/Packet/PlantJalaJoker.tres");
		RegisterPacket("PlantDisguiserCherry", "res://Asset/Anime/Character/Plant/Star/DisguiserCherry/Packet/PlantDisguiserCherry.tres");
		RegisterPacket("ZombieNormal", "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Packet/Normal/ZombieNormal.tres");
		RegisterCharacter("PlantJalaJoker", "res://Asset/Anime/Character/Plant/Other/JalaJoker/Scene/TowerDefensePlantJalaJoker.tscn");
		RegisterCharacter("PlantDisguiserCherry", "res://Asset/Anime/Character/Plant/Star/DisguiserCherry/Scene/TowerDefensePlantDisguiserCherry.tscn");
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

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[BugOverviewIzmAshPlantAttackTargetRuntimeTest] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
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
			new MethodInfo(MethodName.RefreshEncounterRegistration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsZero, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.RefreshEncounterRegistration && args.Count == 3)
		{
			RefreshEncounterRegistration(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsZero && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsZero(VariantUtils.ConvertTo<double>(in args[0])));
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
		if (method == MethodName.RefreshEncounterRegistration && args.Count == 3)
		{
			RefreshEncounterRegistration(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsZero && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsZero(VariantUtils.ConvertTo<double>(in args[0])));
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
		if (method == MethodName.RefreshEncounterRegistration)
		{
			return true;
		}
		if (method == MethodName.IsZero)
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
