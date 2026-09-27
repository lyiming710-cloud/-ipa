using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/GUI/InGame/QuizManager/BetPanel/BetPanel.cs")]
public class BetPanel : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName Finish = "Finish";

		public static readonly StringName WinCheckBoxToggled = "WinCheckBoxToggled";

		public static readonly StringName FailCheckBoxToggled = "FailCheckBoxToggled";

		public static readonly StringName SkipCheckBoxToggled = "SkipCheckBoxToggled";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName betCoinSpinBox = "betCoinSpinBox";

		public static readonly StringName checkBoxNode = "checkBoxNode";

		public static readonly StringName winCheckBox = "winCheckBox";

		public static readonly StringName failCheckBox = "failCheckBox";

		public static readonly StringName skipCheckBox = "skipCheckBox";

		public static readonly StringName betCoinLabel = "betCoinLabel";

		public static readonly StringName lineLabel = "lineLabel";

		public static readonly StringName chooseLabel = "chooseLabel";

		public static readonly StringName line = "line";
	}

	public new class SignalName : Control.SignalName
	{
	}

	public SpinBox betCoinSpinBox;

	public Control checkBoxNode;

	public CheckBox winCheckBox;

	public CheckBox failCheckBox;

	public CheckBox skipCheckBox;

	public Label betCoinLabel;

	public Label lineLabel;

	public Label chooseLabel;

	public int line = 1;

	public override void _Ready()
	{
		betCoinSpinBox = GetNodeOrNull<SpinBox>("%BetCoinSpinBox");
		checkBoxNode = GetNodeOrNull<Control>("%CheckBoxNode");
		winCheckBox = GetNodeOrNull<CheckBox>("%WinCheckBox");
		failCheckBox = GetNodeOrNull<CheckBox>("%FailCheckBox");
		skipCheckBox = GetNodeOrNull<CheckBox>("%SkipCheckBox");
		betCoinLabel = GetNodeOrNull<Label>("%BetCoinLabel");
		lineLabel = GetNodeOrNull<Label>("%LineLabel");
		chooseLabel = GetNodeOrNull<Label>("%ChooseLabel");
		winCheckBox.Toggled += WinCheckBoxToggled;
		failCheckBox.Toggled += FailCheckBoxToggled;
		skipCheckBox.Toggled += SkipCheckBoxToggled;
	}

	public void Init(int _line = 1)
	{
		line = _line;
		lineLabel.Text = $"{line}行 ：";
	}

	public void Finish()
	{
		betCoinSpinBox.Visible = false;
		checkBoxNode.Visible = false;
		betCoinLabel.Visible = true;
		betCoinLabel.Text = $"{(int)betCoinSpinBox.Value} 金币";
		chooseLabel.Visible = true;
		if (winCheckBox.ButtonPressed)
		{
			chooseLabel.Text = "赢";
		}
		if (failCheckBox.ButtonPressed)
		{
			chooseLabel.Text = "输";
		}
		if (skipCheckBox.ButtonPressed)
		{
			chooseLabel.Text = "跳";
		}
	}

	public void WinCheckBoxToggled(bool toggledOn)
	{
		if (!winCheckBox.ButtonPressed)
		{
			winCheckBox.SetPressedNoSignal(pressed: true);
			toggledOn = true;
		}
		if (toggledOn)
		{
			failCheckBox.SetPressedNoSignal(pressed: false);
			skipCheckBox.SetPressedNoSignal(pressed: false);
		}
	}

	public void FailCheckBoxToggled(bool toggledOn)
	{
		if (!failCheckBox.ButtonPressed)
		{
			failCheckBox.SetPressedNoSignal(pressed: true);
			toggledOn = true;
		}
		if (toggledOn)
		{
			winCheckBox.SetPressedNoSignal(pressed: false);
			skipCheckBox.SetPressedNoSignal(pressed: false);
		}
	}

	public void SkipCheckBoxToggled(bool toggledOn)
	{
		if (!skipCheckBox.ButtonPressed)
		{
			skipCheckBox.SetPressedNoSignal(pressed: true);
			toggledOn = true;
		}
		if (toggledOn)
		{
			failCheckBox.SetPressedNoSignal(pressed: false);
			winCheckBox.SetPressedNoSignal(pressed: false);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WinCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FailCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SkipCheckBoxToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		if (method == MethodName.WinCheckBoxToggled && args.Count == 1)
		{
			WinCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FailCheckBoxToggled && args.Count == 1)
		{
			FailCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SkipCheckBoxToggled && args.Count == 1)
		{
			SkipCheckBoxToggled(VariantUtils.ConvertTo<bool>(in args[0]));
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		if (method == MethodName.WinCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.FailCheckBoxToggled)
		{
			return true;
		}
		if (method == MethodName.SkipCheckBoxToggled)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.betCoinSpinBox)
		{
			betCoinSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName.checkBoxNode)
		{
			checkBoxNode = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.winCheckBox)
		{
			winCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.failCheckBox)
		{
			failCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.skipCheckBox)
		{
			skipCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.betCoinLabel)
		{
			betCoinLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.lineLabel)
		{
			lineLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.chooseLabel)
		{
			chooseLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.line)
		{
			line = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.betCoinSpinBox)
		{
			value = VariantUtils.CreateFrom(in betCoinSpinBox);
			return true;
		}
		if (name == PropertyName.checkBoxNode)
		{
			value = VariantUtils.CreateFrom(in checkBoxNode);
			return true;
		}
		if (name == PropertyName.winCheckBox)
		{
			value = VariantUtils.CreateFrom(in winCheckBox);
			return true;
		}
		if (name == PropertyName.failCheckBox)
		{
			value = VariantUtils.CreateFrom(in failCheckBox);
			return true;
		}
		if (name == PropertyName.skipCheckBox)
		{
			value = VariantUtils.CreateFrom(in skipCheckBox);
			return true;
		}
		if (name == PropertyName.betCoinLabel)
		{
			value = VariantUtils.CreateFrom(in betCoinLabel);
			return true;
		}
		if (name == PropertyName.lineLabel)
		{
			value = VariantUtils.CreateFrom(in lineLabel);
			return true;
		}
		if (name == PropertyName.chooseLabel)
		{
			value = VariantUtils.CreateFrom(in chooseLabel);
			return true;
		}
		if (name == PropertyName.line)
		{
			value = VariantUtils.CreateFrom(in line);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.betCoinSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.checkBoxNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.winCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.failCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.skipCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.betCoinLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.lineLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.chooseLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.line, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.betCoinSpinBox, Variant.From(in betCoinSpinBox));
		info.AddProperty(PropertyName.checkBoxNode, Variant.From(in checkBoxNode));
		info.AddProperty(PropertyName.winCheckBox, Variant.From(in winCheckBox));
		info.AddProperty(PropertyName.failCheckBox, Variant.From(in failCheckBox));
		info.AddProperty(PropertyName.skipCheckBox, Variant.From(in skipCheckBox));
		info.AddProperty(PropertyName.betCoinLabel, Variant.From(in betCoinLabel));
		info.AddProperty(PropertyName.lineLabel, Variant.From(in lineLabel));
		info.AddProperty(PropertyName.chooseLabel, Variant.From(in chooseLabel));
		info.AddProperty(PropertyName.line, Variant.From(in line));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.betCoinSpinBox, out var value))
		{
			betCoinSpinBox = value.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName.checkBoxNode, out var value2))
		{
			checkBoxNode = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.winCheckBox, out var value3))
		{
			winCheckBox = value3.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.failCheckBox, out var value4))
		{
			failCheckBox = value4.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.skipCheckBox, out var value5))
		{
			skipCheckBox = value5.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.betCoinLabel, out var value6))
		{
			betCoinLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.lineLabel, out var value7))
		{
			lineLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.chooseLabel, out var value8))
		{
			chooseLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.line, out var value9))
		{
			line = value9.As<int>();
		}
	}
}
