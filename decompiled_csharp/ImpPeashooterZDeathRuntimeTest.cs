using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/ImpPeashooterZDeathRuntimeTest.cs")]
public class ImpPeashooterZDeathRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CountSummons = "CountSummons";

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

	private int _checks;

	private int _failures;

	private ImpPeashooterZDeathControl _control;

	public override async void _Ready()
	{
		_ = 3;
		try
		{
			typeof(ResourceManager).GetField("_gameplayResourceLoadState", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(ResourceManager.Instance, GameplayResourceLoadState.Ready);
			ResourceManager.Instance.TOWERDEFENSE_CHARCATERS["ZombieImpPeashooterSingle"] = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Imp/Scene/PeashooterSingle/TowerDefenseZombieImpPeashooterSingle.tscn");
			ResourceManager.Instance.TOWERDEFENSE_PACKETS["ZombieImpPeashooterSingle"] = GD.Load<TowerDefensePacketConfig>("res://Asset/Anime/Character/Zombie/Chapter1/Imp/Packet/PeashooterSingle/ZombieImpPeashooterSingle.tres");
			_control = new ImpPeashooterZDeathControl
			{
				isInit = true,
				isGameRunning = true,
				levelConfig = new TowerDefenseLevelConfig()
			};
			AddChild(_control, forceReadableName: false, InternalMode.Disabled);
			_control.characterNode = new Node2D();
			_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
			TowerDefenseManager.Instance.currentControl = _control;
			await ProbeAlive();
			await Probe(lateLanding: false);
			await Probe(lateLanding: true);
			_control.isGameRunning = false;
			_control.QueueFree();
			await WaitFrames(5);
			GD.Print($"IMP_PEASHOOTER_DEATH_RESULT passed={_failures == 0 && _checks == 15} checks={_checks} failures={_failures}");
			GetTree().Quit((_failures != 0 || _checks != 15) ? 2 : 0);
		}
		catch (Exception ex)
		{
			GD.PushError(ex.ToString());
			GetTree().Quit(2);
		}
	}

	private async Task Probe(bool lateLanding)
	{
		TowerDefenseZombieImpPeashooterZ zombie = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Imp/Scene/PeashooterZ/TowerDefenseZombieImpPeashooterZ.tscn").Instantiate<TowerDefenseZombieImpPeashooterZ>(PackedScene.GenEditState.Disabled);
		zombie.Position = new Vector2(500f, 250f);
		zombie.gridPos = new Vector2I(3, 2);
		_control.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
		zombie.WalkReady();
		await WaitFrames(5);
		CharacterTimerComponent timer = zombie.componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
		timer.Run("Spawn", 0.0);
		timer.PhysicsProcess(0.0, Engine.GetPhysicsFrames());
		zombie.Hurt(10000.0);
		Check(zombie.die, $"landing={lateLanding}：致命伤害必须进入死亡状态。");
		if (lateLanding)
		{
			zombie.impFlightComponent.Land();
		}
		Check(zombie.sprite.clip == "Death", $"landing={lateLanding}：落地回调不能覆盖死亡动画。");
		zombie.SpawnZombie();
		await WaitFrames(5);
		Check(CountSummons() == 0, $"landing={lateLanding}：死亡前已到期的召唤回调不能创建小鬼。");
		Check(!timer.IsRunning("Spawn"), $"landing={lateLanding}：死亡后不能重启召唤计时器。");
		await WaitFrames(240);
		Check(!GodotObject.IsInstanceValid(zombie) || zombie.isDestroy, $"landing={lateLanding}：死亡动画结束后必须清理身体。");
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefenseCharacter towerDefenseCharacter)
			{
				towerDefenseCharacter.suppressDeathrattles = true;
				towerDefenseCharacter.ClearFromMap();
			}
		}
		await WaitFrames(5);
	}

	private async Task ProbeAlive()
	{
		TowerDefenseZombieImpPeashooterZ zombie = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter1/Imp/Scene/PeashooterZ/TowerDefenseZombieImpPeashooterZ.tscn").Instantiate<TowerDefenseZombieImpPeashooterZ>(PackedScene.GenEditState.Disabled);
		zombie.Position = new Vector2(500f, 250f);
		zombie.gridPos = new Vector2I(3, 2);
		_control.characterNode.AddChild(zombie, forceReadableName: false, InternalMode.Disabled);
		await WaitFrames(5);
		zombie.SpawnZombie();
		await WaitFrames(5);
		Check(CountSummons() == 1, "存活本体必须能够直接召唤一个豌豆小鬼。");
		CharacterTimerComponent timer = zombie.componentManager.GetRuntime<CharacterTimerComponent>("character.timer");
		timer.Run("Spawn", 0.0);
		timer.PhysicsProcess(0.0, Engine.GetPhysicsFrames());
		await WaitFrames(5);
		Check(CountSummons() == 2 && timer.IsRunning("Spawn"), "存活时计时器必须继续召唤并重启下一轮。");
		zombie.impFlightComponent.Fly();
		Check(zombie.sprite.clip == "Fly", "存活小鬼必须能够进入飞行动画。");
		zombie.impFlightComponent.Land();
		Check(zombie.sprite.clip == "Land", "存活小鬼必须能够进入落地动画。");
		await WaitFrames(90);
		Check(zombie.landOver && !zombie.die && zombie.CurrentStateHandle?.StableId == "zombie.walk", "正常落地完成后必须恢复行走。");
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefenseCharacter towerDefenseCharacter)
			{
				towerDefenseCharacter.suppressDeathrattles = true;
				towerDefenseCharacter.ClearFromMap();
			}
		}
		await WaitFrames(5);
	}

	private int CountSummons()
	{
		int num = 0;
		foreach (Node child in _control.characterNode.GetChildren())
		{
			if (child is TowerDefenseZombieImpPeashooterSingle)
			{
				num++;
			}
		}
		return num;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.Print("IMP_PEASHOOTER_DEATH_FAILURE " + message);
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(3)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CountSummons, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.CountSummons && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountSummons());
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.CountSummons)
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
			_control = VariantUtils.ConvertTo<ImpPeashooterZDeathControl>(in value);
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
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
			_control = value3.As<ImpPeashooterZDeathControl>();
		}
	}
}
