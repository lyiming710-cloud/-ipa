using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Command/DebugCommands.cs")]
public class DebugCommands : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Register = "Register";

		public static readonly StringName _GetTree = "_GetTree";

		public static readonly StringName _CmdKillAll = "_CmdKillAll";

		public static readonly StringName _CmdInstantWin = "_CmdInstantWin";

		public static readonly StringName _CmdSkipWaveWait = "_CmdSkipWaveWait";

		public static readonly StringName _CmdSkipToFinalWave = "_CmdSkipToFinalWave";

		public static readonly StringName _CmdSkipToWave = "_CmdSkipToWave";

		public static readonly StringName _CmdRestoreAllMowers = "_CmdRestoreAllMowers";

		public static readonly StringName _CmdRemoveAllMowers = "_CmdRemoveAllMowers";

		public static readonly StringName _CmdResetAllBrains = "_CmdResetAllBrains";

		public static readonly StringName _CmdDebug = "_CmdDebug";

		public static readonly StringName _CmdDebugList = "_CmdDebugList";

		public static readonly StringName _CmdPhonk = "_CmdPhonk";

		public static readonly StringName _CmdDismember = "_CmdDismember";

		public static readonly StringName _CmdShovel = "_CmdShovel";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	public static void Register()
	{
		CommandRegistry.RegisterCommand("sun", "设置阳光数量", "/sun <数量>", Callable.From((long value) =>
		{
			TowerDefenseManager.Instance.SetSun(value);
		}), new Array
		{
			new CommandArg("value", 2, _required: true, default, "阳光数量")
		});
		CommandRegistry.RegisterCommand("coin", "设置金币数量", "/coin <数量>", Callable.From((long value) =>
		{
			TowerDefenseManager.Instance.coinBank.num = value;
		}), new Array
		{
			new CommandArg("value", 2, _required: true, default, "金币数量")
		});
		CommandRegistry.RegisterCommand("crystal", "设置水晶数量", "/crystal <数量>", Callable.From((int value) =>
		{
			GameSaveManager.Instance.SetKeyValue("CrystalNum", value);
			GameSaveManager.Instance.Save();
		}), new Array
		{
			new CommandArg("value", 2, _required: true, default, "水晶数量")
		});
		CommandRegistry.RegisterCommand("killall", "秒杀所有僵尸", "/killall", Callable.From(() =>
		{
			_CmdKillAll();
		}));
		CommandRegistry.RegisterCommand("win", "直接胜利", "/win", Callable.From(() =>
		{
			_CmdInstantWin();
		}));
		CommandRegistry.RegisterCommand("skipwave", "跳过等待下一波", "/skipwave", Callable.From(() =>
		{
			_CmdSkipWaveWait();
		}));
		CommandRegistry.RegisterCommand("skipfinal", "跳到最终波", "/skipfinal", Callable.From(() =>
		{
			_CmdSkipToFinalWave();
		}));
		CommandRegistry.RegisterCommand("skipto", "跳到指定波次", "/skipto <波次>", Callable.From((int wave) =>
		{
			_CmdSkipToWave(wave);
		}), new Array
		{
			new CommandArg("wave", 2, _required: true, default, "目标波次")
		});
		CommandRegistry.RegisterCommand("restoremower", "恢复所有割草机", "/restoremower", Callable.From(() =>
		{
			_CmdRestoreAllMowers();
		}));
		CommandRegistry.RegisterCommand("removemower", "移除所有割草机", "/removemower", Callable.From(() =>
		{
			_CmdRemoveAllMowers();
		}));
		CommandRegistry.RegisterCommand("resetbrain", "重置所有脑子", "/resetbrain", Callable.From(() =>
		{
			_CmdResetAllBrains();
		}));
		CommandRegistry.RegisterCommand("debug", "切换调试选项", "/debug <选项名> [on/off]", Callable.From((string option, string value) =>
		{
			_CmdDebug(option, value);
		}), new Array
		{
			new CommandArg("option", 4, _required: true, default, "选项名"),
			new CommandArg("value", 4, _required: false, "toggle", "on/off")
		});
		CommandRegistry.RegisterCommand("debuglist", "列出所有调试选项", "/debuglist", Callable.From(() =>
		{
			_CmdDebugList();
		}));
		CommandRegistry.RegisterCommand("phonk", "切换Phonk果冻抖动效果", "/phonk <on/off> [intensity]", Callable.From((string state, double intensity) =>
		{
			_CmdPhonk(state, intensity);
		}), new Array
		{
			new CommandArg("state", 4, _required: true, default, "on/off"),
			new CommandArg("intensity", 3, _required: false, 1.0, "强度倍率")
		});
		CommandRegistry.RegisterCommand("dismember", "切换僵尸肢解效果", "/dismember <on/off>", Callable.From((string state) =>
		{
			_CmdDismember(state);
		}), new Array
		{
			new CommandArg("state", 4, _required: true, default, "on/off")
		});
		CommandRegistry.RegisterCommand("shovel", "更换铲子", "/shovel <铲子名称/list>", Callable.From((string name) =>
		{
			_CmdShovel(name);
		}), new Array
		{
			new CommandArg("name", 4, _required: true, default, "铲子名称/list", Callable.From(() => ShovelCommand.GetShovelNames()))
		});
	}

	private static SceneTree _GetTree()
	{
		return (SceneTree)Engine.GetMainLoop();
	}

	private static void _CmdKillAll()
	{
		foreach (Node item in _GetTree().GetNodesInGroup("Zombie"))
		{
			if (GodotObject.IsInstanceValid(item) && item is TowerDefenseZombie towerDefenseZombie && !towerDefenseZombie.instance.die)
			{
				towerDefenseZombie.instance.SkipInvincibleDealHurt(towerDefenseZombie.instance.hitpoints, playSplatAudio: false, default, createDamagePart: false);
			}
		}
		CommandConsole.Instance.PrintSuccess("已秒杀所有僵尸");
	}

	private static void _CmdInstantWin()
	{
		_CmdKillAll();
		TowerDefenseBattleFeatureWave instance = TowerDefenseBattleFeatureWave.Instance;
		if (instance == null)
		{
			CommandConsole.Instance.PrintError("当前不在战斗中");
			return;
		}
		instance.waveStart = true;
		instance.waveFinal = true;
		instance.EmitFinal();
		instance.awaitSpawn = false;
		CommandConsole.Instance.PrintSuccess("已直接胜利");
	}

	private static void _CmdSkipWaveWait()
	{
		TowerDefenseBattleFeatureWave instance = TowerDefenseBattleFeatureWave.Instance;
		if (instance == null)
		{
			CommandConsole.Instance.PrintError("当前不在战斗中");
			return;
		}
		instance.timer = instance.nextWaveTime;
		instance.awaitSpawn = false;
		CommandConsole.Instance.PrintSuccess("已跳过等待");
	}

	private static async void _CmdSkipToFinalWave()
	{
		TowerDefenseBattleFeatureWave wave = TowerDefenseBattleFeatureWave.Instance;
		if (wave == null)
		{
			CommandConsole.Instance.PrintError("当前不在战斗中");
			return;
		}
		if (!wave.waveStart)
		{
			wave.waveStart = true;
		}
		while (!wave.waveFinal)
		{
			wave.timer = wave.nextWaveTime;
			wave.awaitSpawn = false;
			SceneTreeTimer sceneTreeTimer = _GetTree().CreateTimer(0.2, processAlways: false);
			await sceneTreeTimer.ToSignal(sceneTreeTimer, SceneTreeTimer.SignalName.Timeout);
		}
		CommandConsole.Instance.PrintSuccess("已跳到最终波");
	}

	private static async void _CmdSkipToWave(int targetWave)
	{
		TowerDefenseBattleFeatureWave wave = TowerDefenseBattleFeatureWave.Instance;
		if (wave == null)
		{
			CommandConsole.Instance.PrintError("当前不在战斗中");
			return;
		}
		while (wave.currentWave < targetWave && !wave.waveFinal)
		{
			wave.timer = wave.nextWaveTime;
			wave.awaitSpawn = false;
			SceneTreeTimer sceneTreeTimer = _GetTree().CreateTimer(0.2, processAlways: false);
			await sceneTreeTimer.ToSignal(sceneTreeTimer, SceneTreeTimer.SignalName.Timeout);
		}
		CommandConsole.Instance.PrintSuccess($"已跳到第 {targetWave} 波");
	}

	private static void _CmdRestoreAllMowers()
	{
		if (!(TowerDefenseManager.CurrentControl.GetFeature("Mower") is TowerDefenseBattleFeatureMower towerDefenseBattleFeatureMower))
		{
			CommandConsole.Instance.PrintError("当前场景没有割草机");
			return;
		}
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (mapFeature == null)
		{
			return;
		}
		for (int i = 1; i <= mapFeature.config.gridNum.Y; i++)
		{
			if (mapFeature.lineUse[i] && !GodotObject.IsInstanceValid(towerDefenseBattleFeatureMower.mowerLine[i]))
			{
				towerDefenseBattleFeatureMower.CreateMower(i);
			}
		}
		CommandConsole.Instance.PrintSuccess("已恢复所有割草机");
	}

	private static void _CmdRemoveAllMowers()
	{
		if (!(TowerDefenseManager.CurrentControl.GetFeature("Mower") is TowerDefenseBattleFeatureMower towerDefenseBattleFeatureMower))
		{
			CommandConsole.Instance.PrintError("当前场景没有割草机");
			return;
		}
		for (int i = 0; i < towerDefenseBattleFeatureMower.mowerLine.Count; i++)
		{
			Node node = towerDefenseBattleFeatureMower.mowerLine[i];
			if (GodotObject.IsInstanceValid(node))
			{
				node.QueueFree();
				towerDefenseBattleFeatureMower.mowerLine[i] = null;
			}
		}
		CommandConsole.Instance.PrintSuccess("已移除所有割草机");
	}

	private static void _CmdResetAllBrains()
	{
		if (!(TowerDefenseManager.CurrentControl.GetFeature("Brain") is TowerDefenseBattleFeatureBrain towerDefenseBattleFeatureBrain))
		{
			CommandConsole.Instance.PrintError("当前场景没有脑子");
			return;
		}
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (mapFeature == null)
		{
			return;
		}
		for (int i = 0; i < towerDefenseBattleFeatureBrain.brainLine.Count; i++)
		{
			Node node = towerDefenseBattleFeatureBrain.brainLine[i];
			if (GodotObject.IsInstanceValid(node))
			{
				node.QueueFree();
				towerDefenseBattleFeatureBrain.brainLine[i] = null;
			}
		}
		for (int j = 1; j <= mapFeature.config.gridNum.Y; j++)
		{
			if (mapFeature.lineUse[j])
			{
				towerDefenseBattleFeatureBrain.CreateBrain(j);
			}
		}
		CommandConsole.Instance.PrintSuccess("已重置所有脑子");
	}

	private static void _CmdDebug(string option, string value = "toggle")
	{
		Dictionary dictionary = new Dictionary
		{
			["openalllevel"] = "debugOpenAllLevel",
			["coinmax"] = "debugCoinMax",
			["sunmax"] = "debugSunMax",
			["packetselect"] = "debugPacketSelect",
			["packetopenall"] = "debugPacketOpenAll",
			["packetcolddown"] = "debugPacketColdDown",
			["openallcustom"] = "debugOpenAllCustom",
			["openglove"] = "debugOpenGlove",
			["unlimitedfire"] = "debugUnlimitedFire",
			["plantinvincible"] = "debugPlantInvincible",
			["nolose"] = "debugNoLose",
			["wavepaused"] = "debugWavePaused",
			["nozombiespawn"] = "debugNoZombieSpawn",
			["braininvincible"] = "debugBrainInvincible"
		};
		string text = option.ToLower();
		if (!dictionary.ContainsKey(text))
		{
			CommandConsole.Instance.PrintError("未知调试选项: " + option + "  输入 /debuglist 查看所有选项");
			return;
		}
		string text2 = dictionary[text].AsString();
		switch (value.ToLower())
		{
		case "on":
		case "true":
		case "1":
			CommandManager.Instance.Set(text2, true);
			break;
		case "off":
		case "false":
		case "0":
			CommandManager.Instance.Set(text2, false);
			break;
		default:
		{
			bool flag = CommandManager.Instance.Get(text2).AsBool();
			CommandManager.Instance.Set(text2, !flag);
			break;
		}
		}
		bool flag2 = CommandManager.Instance.Get(text2).AsBool();
		CommandConsole.Instance.PrintSuccess(text + " -> " + (flag2 ? "开启" : "关闭"));
	}

	private static void _CmdDebugList()
	{
		Array array = new Array
		{
			new Array { "openalllevel", "开启所有关卡" },
			new Array { "coinmax", "满金币" },
			new Array { "sunmax", "满阳光" },
			new Array { "packetselect", "任何模式启用选卡" },
			new Array { "packetopenall", "解锁所有卡牌" },
			new Array { "packetcolddown", "卡牌无冷却" },
			new Array { "openallcustom", "开启所有装扮" },
			new Array { "openglove", "启用手套" },
			new Array { "unlimitedfire", "无限火力" },
			new Array { "plantinvincible", "植物无敌" },
			new Array { "nolose", "禁用失败" },
			new Array { "wavepaused", "暂停波次" },
			new Array { "nozombiespawn", "禁止僵尸生成" },
			new Array { "braininvincible", "脑子无敌" }
		};
		Dictionary dictionary = new Dictionary
		{
			["openalllevel"] = "debugOpenAllLevel",
			["coinmax"] = "debugCoinMax",
			["sunmax"] = "debugSunMax",
			["packetselect"] = "debugPacketSelect",
			["packetopenall"] = "debugPacketOpenAll",
			["packetcolddown"] = "debugPacketColdDown",
			["openallcustom"] = "debugOpenAllCustom",
			["openglove"] = "debugOpenGlove",
			["unlimitedfire"] = "debugUnlimitedFire",
			["plantinvincible"] = "debugPlantInvincible",
			["nolose"] = "debugNoLose",
			["wavepaused"] = "debugWavePaused",
			["nozombiespawn"] = "debugNoZombieSpawn",
			["braininvincible"] = "debugBrainInvincible"
		};
		CommandConsole.Instance.PrintLine("[color=cyan]═══════ 调试选项列表 ═══════[/color]");
		foreach (Variant item in array)
		{
			Array array2 = item.AsGodotArray();
			string text = array2[0].AsString();
			string text2 = dictionary[text].AsString();
			Variant variant = CommandManager.Instance.Get(text2);
			string value = ((variant.VariantType != Variant.Type.Nil && variant.AsBool()) ? "[color=green]ON[/color]" : "[color=red]OFF[/color]");
			CommandConsole.Instance.PrintLine($"[color=green]/debug {text}[/color] {value} - {array2[1].AsString()}");
		}
	}

	private static void _CmdPhonk(string state, double intensity = 1.0)
	{
		switch (state.ToLower())
		{
		case "on":
		case "true":
		case "1":
			PhonkComponent.phonkEnabled = true;
			PhonkComponent.InjectAll();
			break;
		case "off":
		case "false":
		case "0":
			PhonkComponent.phonkEnabled = false;
			PhonkComponent.RemoveAll();
			break;
		default:
			PhonkComponent.phonkEnabled = !PhonkComponent.phonkEnabled;
			if (PhonkComponent.phonkEnabled)
			{
				PhonkComponent.InjectAll();
			}
			else
			{
				PhonkComponent.RemoveAll();
			}
			break;
		}
		PhonkComponent.phonkIntensity = (float)intensity;
		string value = (PhonkComponent.phonkEnabled ? "[color=green]ON[/color]" : "[color=red]OFF[/color]");
		CommandConsole.Instance.PrintSuccess($"Phonk果冻抖动 {value}  强度: {PhonkComponent.phonkIntensity:F1}");
	}

	private static void _CmdDismember(string state)
	{
		switch (state.ToLower())
		{
		case "on":
		case "true":
		case "1":
			DismemberComponent.dismemberEnabled = true;
			DismemberComponent.InjectAll();
			break;
		case "off":
		case "false":
		case "0":
			DismemberComponent.dismemberEnabled = false;
			DismemberComponent.RemoveAll();
			break;
		default:
			DismemberComponent.dismemberEnabled = !DismemberComponent.dismemberEnabled;
			if (DismemberComponent.dismemberEnabled)
			{
				DismemberComponent.InjectAll();
			}
			else
			{
				DismemberComponent.RemoveAll();
			}
			break;
		}
		string text = (DismemberComponent.dismemberEnabled ? "[color=green]ON[/color]" : "[color=red]OFF[/color]");
		CommandConsole.Instance.PrintSuccess("僵尸肢解 " + text);
	}

	private static void _CmdShovel(string name)
	{
		if (name.ToLower() == "list")
		{
			ShovelCommand.ListShovels();
		}
		else
		{
			ShovelCommand.ChangeShovel(name);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(15)
		{
			new MethodInfo(MethodName.Register, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._GetTree, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SceneTree"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdKillAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdInstantWin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdSkipWaveWait, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdSkipToFinalWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdSkipToWave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "targetWave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._CmdRestoreAllMowers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdRemoveAllMowers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdResetAllBrains, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdDebug, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "option", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._CmdDebugList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._CmdPhonk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "intensity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._CmdDismember, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._CmdShovel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Register && args.Count == 0)
		{
			Register();
			ret = default;
			return true;
		}
		if (method == MethodName._GetTree && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<SceneTree>(_GetTree());
			return true;
		}
		if (method == MethodName._CmdKillAll && args.Count == 0)
		{
			_CmdKillAll();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdInstantWin && args.Count == 0)
		{
			_CmdInstantWin();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdSkipWaveWait && args.Count == 0)
		{
			_CmdSkipWaveWait();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdSkipToFinalWave && args.Count == 0)
		{
			_CmdSkipToFinalWave();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdSkipToWave && args.Count == 1)
		{
			_CmdSkipToWave(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdRestoreAllMowers && args.Count == 0)
		{
			_CmdRestoreAllMowers();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdRemoveAllMowers && args.Count == 0)
		{
			_CmdRemoveAllMowers();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdResetAllBrains && args.Count == 0)
		{
			_CmdResetAllBrains();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdDebug && args.Count == 2)
		{
			_CmdDebug(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdDebugList && args.Count == 0)
		{
			_CmdDebugList();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdPhonk && args.Count == 2)
		{
			_CmdPhonk(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdDismember && args.Count == 1)
		{
			_CmdDismember(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdShovel && args.Count == 1)
		{
			_CmdShovel(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Register && args.Count == 0)
		{
			Register();
			ret = default;
			return true;
		}
		if (method == MethodName._GetTree && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<SceneTree>(_GetTree());
			return true;
		}
		if (method == MethodName._CmdKillAll && args.Count == 0)
		{
			_CmdKillAll();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdInstantWin && args.Count == 0)
		{
			_CmdInstantWin();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdSkipWaveWait && args.Count == 0)
		{
			_CmdSkipWaveWait();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdSkipToFinalWave && args.Count == 0)
		{
			_CmdSkipToFinalWave();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdSkipToWave && args.Count == 1)
		{
			_CmdSkipToWave(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdRestoreAllMowers && args.Count == 0)
		{
			_CmdRestoreAllMowers();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdRemoveAllMowers && args.Count == 0)
		{
			_CmdRemoveAllMowers();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdResetAllBrains && args.Count == 0)
		{
			_CmdResetAllBrains();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdDebug && args.Count == 2)
		{
			_CmdDebug(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdDebugList && args.Count == 0)
		{
			_CmdDebugList();
			ret = default;
			return true;
		}
		if (method == MethodName._CmdPhonk && args.Count == 2)
		{
			_CmdPhonk(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdDismember && args.Count == 1)
		{
			_CmdDismember(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._CmdShovel && args.Count == 1)
		{
			_CmdShovel(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Register)
		{
			return true;
		}
		if (method == MethodName._GetTree)
		{
			return true;
		}
		if (method == MethodName._CmdKillAll)
		{
			return true;
		}
		if (method == MethodName._CmdInstantWin)
		{
			return true;
		}
		if (method == MethodName._CmdSkipWaveWait)
		{
			return true;
		}
		if (method == MethodName._CmdSkipToFinalWave)
		{
			return true;
		}
		if (method == MethodName._CmdSkipToWave)
		{
			return true;
		}
		if (method == MethodName._CmdRestoreAllMowers)
		{
			return true;
		}
		if (method == MethodName._CmdRemoveAllMowers)
		{
			return true;
		}
		if (method == MethodName._CmdResetAllBrains)
		{
			return true;
		}
		if (method == MethodName._CmdDebug)
		{
			return true;
		}
		if (method == MethodName._CmdDebugList)
		{
			return true;
		}
		if (method == MethodName._CmdPhonk)
		{
			return true;
		}
		if (method == MethodName._CmdDismember)
		{
			return true;
		}
		if (method == MethodName._CmdShovel)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
