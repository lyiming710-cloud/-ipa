using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/WaveInvisibleZombieSettlementRuntimeTest.cs")]
public class WaveInvisibleZombieSettlementRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName RunSettlementContract = "RunSettlementContract";

		public static readonly StringName ReleaseFixture = "ReleaseFixture";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _control = "_control";

		public static readonly StringName _zombie = "_zombie";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private int _checks;

	private int _failures;

	private WaveInvisibleZombieSettlementControlStub _control;

	private WaveInvisibleZombieSettlementProbeZombie _zombie;

	public override void _Ready()
	{
		bool flag = false;
		try
		{
			RunSettlementContract();
			flag = _failures == 0;
		}
		catch (Exception value)
		{
			_failures++;
			GD.PushError($"[WaveInvisibleZombieSettlement] 未预期异常：{value}");
		}
		finally
		{
			ReleaseFixture();
		}
		GD.Print($"WAVE_INVISIBLE_ZOMBIE_SETTLEMENT_RESULT passed={flag} checks={_checks} failures={_failures}");
		GetTree().Quit((!flag) ? 2 : 0);
	}

	private void RunSettlementContract()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance) || !GodotObject.IsInstanceValid(instance.characterRegistry))
		{
			throw new InvalidOperationException("塔防管理器或角色注册表不可用。");
		}
		_control = new WaveInvisibleZombieSettlementControlStub
		{
			Name = "WaveInvisibleZombieSettlementControl",
			isGameRunning = true,
			levelConfig = new TowerDefenseLevelConfig()
		};
		AddChild(_control, forceReadableName: false, InternalMode.Disabled);
		_control.characterNode = new Node2D
		{
			Name = "CharacterNode"
		};
		_control.AddChild(_control.characterNode, forceReadableName: false, InternalMode.Disabled);
		_control.levelControl = new WaveInvisibleZombieSettlementLevelControlStub
		{
			Name = "LevelControl"
		};
		_control.AddChild(_control.levelControl, forceReadableName: false, InternalMode.Disabled);
		instance.currentControl = _control;
		TowerDefenseBattleProcessWave towerDefenseBattleProcessWave = new TowerDefenseBattleProcessWave
		{
			control = _control,
			levelControl = _control.levelControl
		};
		_control.process = towerDefenseBattleProcessWave;
		_zombie = new WaveInvisibleZombieSettlementProbeZombie
		{
			Name = "UnregisteredInvisibleZombie",
			inGame = true,
			camp = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE,
			invisible = true,
			instance = new TowerDefenseCharacterInstance()
		};
		_control.characterNode.AddChild(_zombie, forceReadableName: false, InternalMode.Disabled);
		_zombie.AddToGroup("Zombie", persistent: true);
		instance.CharacterRegister(_zombie);
		bool condition = instance.characterRegistry.TryGetFinalWaveTarget(TowerDefenseEnum.CHARACTER_CAMP.PLANT, out var target, out var hasPendingDestroyZombie);
		Check(condition, "已注册的存活敌对隐身僵尸必须进入最终波目标集。");
		Check(!towerDefenseBattleProcessWave.CheckFinal(), "已注册的存活敌对隐身僵尸必须阻止 Wave 结算。");
		instance.CharacterUnregister(_zombie);
		condition = instance.characterRegistry.TryGetFinalWaveTarget(TowerDefenseEnum.CHARACTER_CAMP.PLANT, out target, out hasPendingDestroyZombie);
		Check(!condition, "测试前提要求注销后的隐身僵尸暂时不在角色注册表目标集中。");
		Check(!towerDefenseBattleProcessWave.CheckFinal(), "场景组内仍有存活敌对隐身僵尸时，Wave 不得判定游戏结束。");
	}

	private void ReleaseFixture()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(instance) && instance.currentControl == _control)
		{
			instance.currentControl = null;
		}
		if (GodotObject.IsInstanceValid(_zombie))
		{
			if (GodotObject.IsInstanceValid(instance))
			{
				instance.CharacterUnregister(_zombie);
			}
			_zombie.RemoveFromGroup("Zombie");
			_zombie.Free();
		}
		if (GodotObject.IsInstanceValid(_control))
		{
			_control.Free();
		}
		_zombie = null;
		_control = null;
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError("[WaveInvisibleZombieSettlement] " + message);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunSettlementContract, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseFixture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.RunSettlementContract && args.Count == 0)
		{
			RunSettlementContract();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseFixture && args.Count == 0)
		{
			ReleaseFixture();
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.RunSettlementContract)
		{
			return true;
		}
		if (method == MethodName.ReleaseFixture)
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
			_control = VariantUtils.ConvertTo<WaveInvisibleZombieSettlementControlStub>(in value);
			return true;
		}
		if (name == PropertyName._zombie)
		{
			_zombie = VariantUtils.ConvertTo<WaveInvisibleZombieSettlementProbeZombie>(in value);
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
		if (name == PropertyName._zombie)
		{
			value = VariantUtils.CreateFrom(in _zombie);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._control, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zombie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._control, Variant.From(in _control));
		info.AddProperty(PropertyName._zombie, Variant.From(in _zombie));
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
			_control = value3.As<WaveInvisibleZombieSettlementControlStub>();
		}
		if (info.TryGetProperty(PropertyName._zombie, out var value4))
		{
			_zombie = value4.As<WaveInvisibleZombieSettlementProbeZombie>();
		}
	}
}
