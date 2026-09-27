using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Prefab/GUI/DialogBox/DialogBoxGeneral/DialogBoxLevelEditorNewLevel/DialogBoxLevelEditorNewLevel.cs")]
public class DialogBoxLevelEditorNewLevel : DialogPopup
{
	public delegate void CreateLevelEventHandler(TowerDefenseLevelConfig level);

	public new class MethodName : DialogPopup.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName NormalButtonPressed = "NormalButtonPressed";

		public static readonly StringName ConveyorButtonPressed = "ConveyorButtonPressed";

		public static readonly StringName CancleButtonPressed = "CancleButtonPressed";
	}

	public new class PropertyName : DialogPopup.PropertyName
	{
	}

	public new class SignalName : DialogPopup.SignalName
	{
	}

	public event CreateLevelEventHandler OnCreateLevel;

	public override void _Ready()
	{
		base._Ready();
		GetNode<BaseButton>("%NormalButton").Pressed += NormalButtonPressed;
		GetNode<BaseButton>("%ConveyorButton").Pressed += ConveyorButtonPressed;
		GetNode<BaseButton>("%CancleButton").Pressed += CancleButtonPressed;
	}

	public void NormalButtonPressed()
	{
		TowerDefenseLevelConfig level = new TowerDefenseLevelConfig();
		OnCreateLevel?.Invoke(level);
		CloseDialog();
	}

	public void ConveyorButtonPressed()
	{
		TowerDefenseLevelConfig towerDefenseLevelConfig = new TowerDefenseLevelConfig();
		towerDefenseLevelConfig.ConveyorPreset();
		OnCreateLevel?.Invoke(towerDefenseLevelConfig);
		CloseDialog();
	}

	public void CancleButtonPressed()
	{
		CloseDialog();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConveyorButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancleButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.NormalButtonPressed && args.Count == 0)
		{
			NormalButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.ConveyorButtonPressed && args.Count == 0)
		{
			ConveyorButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.CancleButtonPressed && args.Count == 0)
		{
			CancleButtonPressed();
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
		if (method == MethodName.NormalButtonPressed)
		{
			return true;
		}
		if (method == MethodName.ConveyorButtonPressed)
		{
			return true;
		}
		if (method == MethodName.CancleButtonPressed)
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
