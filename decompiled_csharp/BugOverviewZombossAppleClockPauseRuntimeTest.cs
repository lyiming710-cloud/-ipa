using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/BugOverviewZombossAppleClockPauseRuntimeTest.cs")]
public class BugOverviewZombossAppleClockPauseRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetupBattleFixture = "SetupBattleFixture";

		public static readonly StringName VerifySchedulerFreeze = "VerifySchedulerFreeze";

		public static readonly StringName VerifyHeadAttackPauseFreeze = "VerifyHeadAttackPauseFreeze";

		public static readonly StringName ConfigureRestProbe = "ConfigureRestProbe";

		public static readonly StringName IsRestProbeFrozen = "IsRestProbeFrozen";

		public static readonly StringName ResetBossScheduler = "ResetBossScheduler";

		public static readonly StringName InvokeHeadAttackPauseStart = "InvokeHeadAttackPauseStart";

		public static readonly StringName AdvanceBossManually = "AdvanceBossManually";

		public static readonly StringName Check = "Check";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _control = "_control";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string BossScenePath = "res://Asset/Anime/Character/Zombie/Boss/Boss/Scene/TowerDefenseZombieBoss.tscn";

	private const string BossDaveScenePath = "res://Asset/Anime/Character/Zombie/Boss/BossDave/Scene/TowerDefenseZombieBossDave.tscn";

	private const int ExpectedChecks = 15;

	private int _checks;

	private readonly List<string> _failures = new List<string>();

	private ZombossAppleClockPauseRuntimeControlStub _control;

	public override async void _Ready()
	{
		TowerDefenseManager manager = TowerDefenseManager.Instance;
		TowerDefenseControlNew previousControl = manager?.currentControl;
		Vector2 previousGridBegin = manager?.gridBeginPos ?? Vector2.Zero;
		Vector2 previousGridSize = manager?.gridSize ?? Vector2.Zero;
		Vector2I previousGridNum = manager?.gridNum ?? Vector2I.Zero;
		bool previousPausePacket = manager?.pausePacket ?? false;
		bool previousPauseZombie = manager?.pauseZombie ?? false;
		bool previousUseBatch = TowerDefenseZombie.UseBatch;
		try
		{
			_ = 1;
			try
			{
				Check(GodotObject.IsInstanceValid(manager), "测试必须能够使用正式战斗管理器。");
				if (!GodotObject.IsInstanceValid(manager))
				{
					throw new InvalidOperationException("缺少测试所需的战斗管理器。");
				}
				TowerDefenseZombie.UseBatch = false;
				SetupBattleFixture(manager);
				await VerifyBossPause(manager, "res://Asset/Anime/Character/Zombie/Boss/Boss/Scene/TowerDefenseZombieBoss.tscn", isDave: false, "普通僵王");
				await VerifyBossPause(manager, "res://Asset/Anime/Character/Zombie/Boss/BossDave/Scene/TowerDefenseZombieBossDave.tscn", isDave: true, "戴夫博士");
			}
			catch (Exception value)
			{
				_failures.Add($"运行测试出现异常：{value}");
			}
		}
		finally
		{
			TowerDefenseZombie.UseBatch = previousUseBatch;
			await CleanupBattleFixture(manager, previousControl, previousGridBegin, previousGridSize, previousGridNum, previousPausePacket, previousPauseZombie);
		}
		Finish();
	}

	private void SetupBattleFixture(TowerDefenseManager manager)
	{
		_control = new ZombossAppleClockPauseRuntimeControlStub
		{
			Name = "ZombossAppleClockPauseRuntimeControl",
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
		manager.currentControl = _control;
		manager.gridBeginPos = Vector2.Zero;
		manager.gridSize = new Vector2(100f, 76f);
		manager.gridNum = new Vector2I(9, 5);
		manager.pausePacket = false;
		manager.pauseZombie = false;
	}

	private async Task VerifyBossPause(TowerDefenseManager manager, string scenePath, bool isDave, string label)
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(scenePath, null, ResourceLoader.CacheMode.Ignore);
		Check(GodotObject.IsInstanceValid(packedScene), label + "必须加载正式角色场景。");
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			return;
		}
		TowerDefenseZombie boss = (isDave ? ((TowerDefenseZombie)packedScene.Instantiate<TowerDefenseZombieBossDave>(PackedScene.GenEditState.Disabled)) : ((TowerDefenseZombie)packedScene.Instantiate<TowerDefenseZombieBoss>(PackedScene.GenEditState.Disabled)));
		Check(GodotObject.IsInstanceValid(boss), label + "必须从正式角色场景实例化。");
		if (GodotObject.IsInstanceValid(boss))
		{
			boss.Name = label + "AppleClockPauseRuntime";
			boss.inGame = true;
			boss.ProcessMode = ProcessModeEnum.Disabled;
			_control.characterNode.AddChild(boss, forceReadableName: false, InternalMode.Disabled);
			await WaitFrames(8);
			Check(GodotObject.IsInstanceValid(boss.sprite), label + "必须完成正式动画根节点初始化。");
			if (!GodotObject.IsInstanceValid(boss.sprite))
			{
				await CleanupCharacters(boss);
				return;
			}
			VerifySchedulerFreeze(manager, boss, isDave, label);
			VerifyHeadAttackPauseFreeze(manager, boss, isDave, label);
			await CleanupCharacters(boss);
		}
	}

	private void VerifySchedulerFreeze(TowerDefenseManager manager, TowerDefenseZombie boss, bool isDave, string label)
	{
		ConfigureRestProbe(boss, isDave);
		manager.pauseZombie = true;
		boss.BatchUpdate(0.5);
		Check(IsRestProbeFrozen(boss, isDave) && boss.sprite.pause, label + "在苹果闹钟暂停期间不能推进 Boss 调度队列。");
		manager.pauseZombie = false;
	}

	private void VerifyHeadAttackPauseFreeze(TowerDefenseManager manager, TowerDefenseZombie boss, bool isDave, string label)
	{
		ResetBossScheduler(boss, isDave);
		manager.pauseZombie = false;
		boss.spritePause = false;
		InvokeHeadAttackPauseStart(boss);
		Check(boss.spritePause, label + "吐球事件必须立刻持有动画暂停。");
		manager.pauseZombie = true;
		AdvanceBossManually(boss, 75, 1.0 / 60.0);
		Check(boss.spritePause && boss.sprite.pause, label + "苹果闹钟暂停超过一秒时，吐球等待不能提前释放。");
		manager.pauseZombie = false;
		AdvanceBossManually(boss, 75, 1.0 / 60.0);
		Check(!boss.spritePause, label + "苹果闹钟解除后，吐球等待必须按剩余时间释放。");
	}

	private static void ConfigureRestProbe(TowerDefenseZombie boss, bool isDave)
	{
		if (isDave && boss is TowerDefenseZombieBossDave towerDefenseZombieBossDave)
		{
			towerDefenseZombieBossDave.stateMethodList = new Godot.Collections.Array { "HeadIdle" };
			towerDefenseZombieBossDave.isRest = true;
			towerDefenseZombieBossDave.restTime = 0.1;
			towerDefenseZombieBossDave.restTimer = 0.2;
			towerDefenseZombieBossDave.stateNow = "";
			return;
		}
		if (boss is TowerDefenseZombieBoss towerDefenseZombieBoss)
		{
			towerDefenseZombieBoss.stateMethodList = new Godot.Collections.Array { "HeadIdle" };
			towerDefenseZombieBoss.isRest = true;
			towerDefenseZombieBoss.restTime = 0.1;
			towerDefenseZombieBoss.restTimer = 0.2;
			towerDefenseZombieBoss.stateNow = "";
			return;
		}
		throw new InvalidOperationException("测试收到未知僵王类型。");
	}

	private static bool IsRestProbeFrozen(TowerDefenseZombie boss, bool isDave)
	{
		if (isDave && boss is TowerDefenseZombieBossDave towerDefenseZombieBossDave)
		{
			if (towerDefenseZombieBossDave.isRest && towerDefenseZombieBossDave.stateNow == "" && towerDefenseZombieBossDave.stateMethodList.Count == 1)
			{
				return (string)towerDefenseZombieBossDave.stateMethodList[0] == "HeadIdle";
			}
			return false;
		}
		if (boss is TowerDefenseZombieBoss towerDefenseZombieBoss)
		{
			if (towerDefenseZombieBoss.isRest && towerDefenseZombieBoss.stateNow == "" && towerDefenseZombieBoss.stateMethodList.Count == 1)
			{
				return (string)towerDefenseZombieBoss.stateMethodList[0] == "HeadIdle";
			}
			return false;
		}
		return false;
	}

	private static void ResetBossScheduler(TowerDefenseZombie boss, bool isDave)
	{
		if (isDave && boss is TowerDefenseZombieBossDave towerDefenseZombieBossDave)
		{
			towerDefenseZombieBossDave.stateMethodList = new Godot.Collections.Array();
			towerDefenseZombieBossDave.isRest = false;
			towerDefenseZombieBossDave.restTimer = 0.0;
			towerDefenseZombieBossDave.stateNow = "";
			return;
		}
		if (boss is TowerDefenseZombieBoss towerDefenseZombieBoss)
		{
			towerDefenseZombieBoss.stateMethodList = new Godot.Collections.Array();
			towerDefenseZombieBoss.isRest = false;
			towerDefenseZombieBoss.restTimer = 0.0;
			towerDefenseZombieBoss.stateNow = "";
			return;
		}
		throw new InvalidOperationException("测试收到未知僵王类型。");
	}

	private static void InvokeHeadAttackPauseStart(TowerDefenseZombie boss)
	{
		System.Reflection.MethodInfo? method = boss.GetType().GetMethod("BeginHeadAttackPause", BindingFlags.Instance | BindingFlags.NonPublic);
		if (method == null)
		{
			throw new MissingMethodException(boss.GetType().FullName, "BeginHeadAttackPause");
		}
		method.Invoke(boss, null);
	}

	private static void AdvanceBossManually(TowerDefenseZombie boss, int frameCount, double delta)
	{
		for (int i = 0; i < frameCount; i++)
		{
			boss.BatchUpdate(delta);
		}
	}

	private async Task CleanupCharacters(params TowerDefenseCharacter[] characters)
	{
		foreach (TowerDefenseCharacter towerDefenseCharacter in characters)
		{
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && !towerDefenseCharacter.IsQueuedForDeletion())
			{
				towerDefenseCharacter.QueueFree();
			}
		}
		await WaitFrames(5);
	}

	private async Task CleanupBattleFixture(TowerDefenseManager manager, TowerDefenseControlNew previousControl, Vector2 previousGridBegin, Vector2 previousGridSize, Vector2I previousGridNum, bool previousPausePacket, bool previousPauseZombie)
	{
		if (GodotObject.IsInstanceValid(_control?.characterNode))
		{
			foreach (Node child in _control.characterNode.GetChildren())
			{
				if (GodotObject.IsInstanceValid(child) && !child.IsQueuedForDeletion())
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
			manager.pausePacket = previousPausePacket;
			manager.pauseZombie = previousPauseZombie;
		}
		if (GodotObject.IsInstanceValid(_control) && !_control.IsQueuedForDeletion())
		{
			_control.QueueFree();
		}
		await WaitFrames(3);
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
			_failures.Add(message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PushError("[BugOverviewZombossAppleClockPauseRuntimeTest] " + failure);
		}
		bool flag = _failures.Count == 0 && _checks == 15;
		GD.Print($"ZOMBOSS_APPLE_CLOCK_PAUSE_RESULT passed={flag} checks={_checks} failures={_failures.Count}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(11)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.SetupBattleFixture, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifySchedulerFreeze, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.VerifyHeadAttackPauseFreeze, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ConfigureRestProbe, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.IsRestProbeFrozen, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ResetBossScheduler, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "isDave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.InvokeHeadAttackPauseStart, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.AdvanceBossManually, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "boss", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "frameCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Finish, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetupBattleFixture && args.Count == 1)
		{
			SetupBattleFixture(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.VerifySchedulerFreeze && args.Count == 4)
		{
			VerifySchedulerFreeze(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<TowerDefenseZombie>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.VerifyHeadAttackPauseFreeze && args.Count == 4)
		{
			VerifyHeadAttackPauseFreeze(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<TowerDefenseZombie>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureRestProbe && args.Count == 2)
		{
			ConfigureRestProbe(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsRestProbeFrozen && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRestProbeFrozen(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ResetBossScheduler && args.Count == 2)
		{
			ResetBossScheduler(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvokeHeadAttackPauseStart && args.Count == 1)
		{
			InvokeHeadAttackPauseStart(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceBossManually && args.Count == 3)
		{
			AdvanceBossManually(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ConfigureRestProbe && args.Count == 2)
		{
			ConfigureRestProbe(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsRestProbeFrozen && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRestProbeFrozen(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ResetBossScheduler && args.Count == 2)
		{
			ResetBossScheduler(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvokeHeadAttackPauseStart && args.Count == 1)
		{
			InvokeHeadAttackPauseStart(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceBossManually && args.Count == 3)
		{
			AdvanceBossManually(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
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
		if (method == MethodName.SetupBattleFixture)
		{
			return true;
		}
		if (method == MethodName.VerifySchedulerFreeze)
		{
			return true;
		}
		if (method == MethodName.VerifyHeadAttackPauseFreeze)
		{
			return true;
		}
		if (method == MethodName.ConfigureRestProbe)
		{
			return true;
		}
		if (method == MethodName.IsRestProbeFrozen)
		{
			return true;
		}
		if (method == MethodName.ResetBossScheduler)
		{
			return true;
		}
		if (method == MethodName.InvokeHeadAttackPauseStart)
		{
			return true;
		}
		if (method == MethodName.AdvanceBossManually)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		if (method == MethodName.Finish)
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
		if (name == PropertyName._control)
		{
			_control = VariantUtils.ConvertTo<ZombossAppleClockPauseRuntimeControlStub>(in value);
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
		if (name == PropertyName._control)
		{
			value = VariantUtils.CreateFrom(in _control);
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
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
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
		if (info.TryGetProperty(PropertyName._control, out var value2))
		{
			_control = value2.As<ZombossAppleClockPauseRuntimeControlStub>();
		}
	}
}
