using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/CellAttackTargetRuntimeTest.cs")]
public class CellAttackTargetRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateCharacter = "CreateCharacter";

		public static readonly StringName VerifySelection = "VerifySelection";

		public static readonly StringName Benchmark = "Benchmark";

		public static readonly StringName Expect = "Expect";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _checks = "_checks";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<TowerDefenseCharacter> _characters = new List<TowerDefenseCharacter>();

	private int _checks;

	private const int GroundMask = 1;

	public override void _Ready()
	{
		try
		{
			VerifySelection();
			Benchmark(1);
			Benchmark(25);
			Benchmark(50);
			GD.Print($"CELL_ATTACK_TARGET_RESULT passed=True checks={_checks}");
			GetTree().Quit();
		}
		catch (Exception ex)
		{
			GD.PrintErr(ex);
			GetTree().Quit(1);
		}
		finally
		{
			foreach (TowerDefenseCharacter character in _characters)
			{
				if (GodotObject.IsInstanceValid(character))
				{
					character.Free();
				}
			}
		}
	}

	private TowerDefenseCharacter CreateCharacter()
	{
		TowerDefensePlant towerDefensePlant = new TowerDefensePlant
		{
			camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT,
			HitBoxDefinition = new CharacterHitBoxDefinition
			{
				Size = new Vector2(40f, 40f)
			}
		};
		towerDefensePlant.instance = new TowerDefenseCharacterInstance
		{
			character = towerDefensePlant,
			maskFlags = 1,
			collisionFlags = 1,
			canBeCollection = true
		};
		_characters.Add(towerDefensePlant);
		return towerDefensePlant;
	}

	private void VerifySelection()
	{
		using TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance();
		TowerDefenseCharacter towerDefenseCharacter = CreateCharacter();
		TowerDefenseCharacter towerDefenseCharacter2 = CreateCharacter();
		TowerDefenseCharacter towerDefenseCharacter3 = CreateCharacter();
		TowerDefenseCharacter towerDefenseCharacter4 = CreateCharacter();
		towerDefenseCellInstance.characterList.Add(towerDefenseCharacter);
		towerDefenseCellInstance.characterList.Add(towerDefenseCharacter2);
		Expect(towerDefenseCellInstance, towerDefenseCharacter, "普通植物保持加入顺序");
		towerDefenseCharacter.instance.invincible = true;
		Expect(towerDefenseCellInstance, towerDefenseCharacter2, "同帧无敌切换立即生效");
		Check(towerDefenseCellInstance.GetTarget(1, TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE, checkInvincible: false) == towerDefenseCharacter, "关闭无敌过滤保持原有语义");
		towerDefenseCharacter.instance.invincible = false;
		towerDefenseCharacter.instance.canBeCollection = false;
		Expect(towerDefenseCellInstance, towerDefenseCharacter2, "不可选取目标立即跳过");
		towerDefenseCharacter.instance.canBeCollection = true;
		towerDefenseCharacter.camp = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE;
		Expect(towerDefenseCellInstance, towerDefenseCharacter2, "同阵营目标不能被选中");
		towerDefenseCharacter.camp = TowerDefenseEnum.CHARACTER_CAMP.PLANT;
		towerDefenseCharacter.instance.maskFlags = 0;
		Expect(towerDefenseCellInstance, towerDefenseCharacter2, "碰撞掩码变化立即生效");
		towerDefenseCharacter.instance.maskFlags = 1;
		towerDefenseCellInstance.characterSurround = towerDefenseCharacter4;
		Expect(towerDefenseCellInstance, towerDefenseCharacter4, "保护层优先于普通植物");
		Check(towerDefenseCellInstance.GetTarget(1, TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE, checkInvincible: true, isCataplut: true) == towerDefenseCharacter, "投掷检测保持保护层后置");
		towerDefenseCellInstance.characterSurround = null;
		towerDefenseCellInstance.characterSlotDictionary[towerDefenseCharacter3] = towerDefenseCharacter;
		towerDefenseCellInstance.slot[TowerDefenseEnum.PLANTGRIDTYPE.GROUND] = towerDefenseCharacter;
		Expect(towerDefenseCellInstance, towerDefenseCharacter, "承载层保持优先级");
		Check(towerDefenseCellInstance.FindSlotParent(towerDefenseCharacter) == towerDefenseCharacter3, "承载关系可反查");
		towerDefenseCharacter3.instance.invincible = true;
		Expect(towerDefenseCellInstance, towerDefenseCharacter2, "承载父级无敌同时过滤槽位和普通列表");
		towerDefenseCharacter3.instance.invincible = false;
		towerDefenseCellInstance.characterSlotDictionary[towerDefenseCharacter3] = towerDefenseCharacter2;
		Check(towerDefenseCellInstance.FindSlotParent(towerDefenseCharacter) == null && towerDefenseCellInstance.FindSlotParent(towerDefenseCharacter2) == towerDefenseCharacter3, "同帧更换承载关系立即生效");
		Expect(towerDefenseCellInstance, towerDefenseCharacter2, "新承载目标立即获得优先级");
		towerDefenseCellInstance.slot[TowerDefenseEnum.PLANTGRIDTYPE.AIR] = towerDefenseCharacter4;
		Check(towerDefenseCellInstance.GetTarget(1, TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE, checkInvincible: true, isCataplut: true) == towerDefenseCharacter4, "投掷检测优先选空中槽位");
		towerDefenseCellInstance.characterSlotDictionary.Clear();
		towerDefenseCellInstance.slot.Clear();
		towerDefenseCellInstance.characterList.Remove(towerDefenseCharacter);
		Expect(towerDefenseCellInstance, towerDefenseCharacter2, "移除植物立即切换目标");
		towerDefenseCharacter2.Free();
		towerDefenseCellInstance.dirty = true;
		Expect(towerDefenseCellInstance, null, "已释放对象清理后返回空目标");
		Check(towerDefenseCellInstance.characterList.Count == 0, "脏格子仍执行原有清理");
	}

	private void Benchmark(int plantCount)
	{
		using TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance();
		towerDefenseCellInstance.slot[TowerDefenseEnum.PLANTGRIDTYPE.GROUND] = null;
		towerDefenseCellInstance.slot[TowerDefenseEnum.PLANTGRIDTYPE.AIR] = null;
		for (int i = 0; i < plantCount; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = CreateCharacter();
			towerDefenseCellInstance.characterList.Add(towerDefenseCharacter);
			towerDefenseCellInstance.characterSlotDictionary[towerDefenseCharacter] = null;
		}
		for (int j = 0; j < 4; j++)
		{
			long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
			long timestamp = Stopwatch.GetTimestamp();
			int num = 0;
			for (int k = 0; k < 200000; k++)
			{
				if (towerDefenseCellInstance.GetTarget(1, TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE) == towerDefenseCellInstance.characterList[0])
				{
					num++;
				}
			}
			GD.Print($"CELL_ATTACK_BENCH plants={plantCount} pass={j} calls=200000 matches={num} ms={Stopwatch.GetElapsedTime(timestamp).TotalMilliseconds:F3} allocated={GC.GetAllocatedBytesForCurrentThread() - allocatedBytesForCurrentThread}");
			Check(num == 200000, "压力测试始终选择正确目标");
		}
	}

	private void Expect(TowerDefenseCellInstance cell, TowerDefenseCharacter expected, string message)
	{
		Check(cell.GetTarget(1, TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE) == expected, message);
	}

	private void Check(bool condition, string message)
	{
		if (!condition)
		{
			throw new InvalidOperationException(message);
		}
		_checks++;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.VerifySelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Benchmark, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "plantCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Expect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "expected", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
		if (method == MethodName.CreateCharacter && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(CreateCharacter());
			return true;
		}
		if (method == MethodName.VerifySelection && args.Count == 0)
		{
			VerifySelection();
			ret = default;
			return true;
		}
		if (method == MethodName.Benchmark && args.Count == 1)
		{
			Benchmark(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Expect && args.Count == 3)
		{
			Expect(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
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
		if (method == MethodName.CreateCharacter)
		{
			return true;
		}
		if (method == MethodName.VerifySelection)
		{
			return true;
		}
		if (method == MethodName.Benchmark)
		{
			return true;
		}
		if (method == MethodName.Expect)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checks, out var value))
		{
			_checks = value.As<int>();
		}
	}
}
