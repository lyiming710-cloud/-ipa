using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Tests/TowerDefenseCellClearEmptyRuntimeProbe.cs")]
public class TowerDefenseCellClearEmptyRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";
	}

	public new class PropertyName : Node.PropertyName
	{
	}

	public new class SignalName : Node.SignalName
	{
	}

	public override void _Ready()
	{
		TowerDefenseCellInstance towerDefenseCellInstance = new TowerDefenseCellInstance();
		TowerDefenseCharacter towerDefenseCharacter = new TowerDefenseCharacter();
		TowerDefenseCharacter towerDefenseCharacter2 = new TowerDefenseCharacter();
		TowerDefenseCharacter towerDefenseCharacter3 = new TowerDefenseCharacter();
		TowerDefenseCharacter towerDefenseCharacter4 = new TowerDefenseCharacter();
		towerDefenseCellInstance.characterSlotDictionary[towerDefenseCharacter] = towerDefenseCharacter2;
		towerDefenseCellInstance.characterSlotDictionary[towerDefenseCharacter3] = towerDefenseCharacter2;
		towerDefenseCellInstance.characterSlotDictionary[towerDefenseCharacter4] = towerDefenseCharacter2;
		towerDefenseCharacter3.Free();
		towerDefenseCharacter4.Free();
		towerDefenseCellInstance.dirty = true;
		bool flag = true;
		try
		{
			towerDefenseCellInstance.ClearEmpty();
		}
		catch (Exception value)
		{
			flag = false;
			GD.PushError($"TowerDefenseCellInstance.ClearEmpty threw: {value}");
		}
		bool flag2 = towerDefenseCellInstance.characterSlotDictionary.Count == 1;
		bool flag3 = towerDefenseCellInstance.characterSlotDictionary.TryGetValue(towerDefenseCharacter, out var value2) && value2 == towerDefenseCharacter2;
		bool flag4 = !towerDefenseCellInstance.dirty;
		bool flag5 = false;
		try
		{
			towerDefenseCellInstance.ClearEmpty();
			flag5 = towerDefenseCellInstance.characterSlotDictionary.Count == 1;
		}
		catch (Exception value3)
		{
			GD.PushError($"TowerDefenseCellInstance.ClearEmpty idempotence failed: {value3}");
		}
		bool flag6 = flag & flag2 & flag3 & flag4 & flag5;
		GD.Print($"[TOWER_DEFENSE_CELL_CLEAR_EMPTY_PROBE] noException={flag} removedInvalidKeys={flag2} preservedValidEntry={flag3} dirtyCleared={flag4} idempotent={flag5} passed={flag6}");
		towerDefenseCharacter.Free();
		towerDefenseCharacter2.Free();
		towerDefenseCellInstance.Dispose();
		GetTree().Quit((!flag6) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
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
