using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/Properties/Type/Bool/PropertiesBool.cs")]
public class PropertiesBool : PropertiesBase
{
	public delegate void ValueChangedEventHandler(bool value);

	public new class MethodName : PropertiesBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetValue = "SetValue";

		public static readonly StringName ValueChange = "ValueChange";

		public new static readonly StringName CanSetValue = "CanSetValue";

		public new static readonly StringName _GetType = "_GetType";
	}

	public new class PropertyName : PropertiesBase.PropertyName
	{
		public static readonly StringName _checkbox = "_checkbox";

		public static readonly StringName value = "value";
	}

	public new class SignalName : PropertiesBase.SignalName
	{
	}

	private CheckBox _checkbox;

	public bool value;

	public event ValueChangedEventHandler OnValueChanged;

	public override void _Ready()
	{
		_checkbox = GetNode<CheckBox>("%Checkbox");
		value = _checkbox.ButtonPressed;
		_checkbox.Toggled += ValueChange;
	}

	public void SetValue(bool newValue)
	{
		if (IsNodeReady())
		{
			value = newValue;
			_checkbox.Toggled -= ValueChange;
			_checkbox.ButtonPressed = value;
			_checkbox.Toggled += ValueChange;
		}
	}

	private void ValueChange(bool toggledOn)
	{
		if (IsNodeReady())
		{
			value = _checkbox.ButtonPressed;
			OnValueChanged?.Invoke(value);
		}
	}

	public override bool CanSetValue()
	{
		return !_checkbox.HasFocus();
	}

	public override string _GetType()
	{
		return "Bool";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggledOn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanSetValue, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetValue && args.Count == 1)
		{
			SetValue(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ValueChange && args.Count == 1)
		{
			ValueChange(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanSetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSetValue());
			return true;
		}
		if (method == MethodName._GetType && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetType());
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
		if (method == MethodName.SetValue)
		{
			return true;
		}
		if (method == MethodName.ValueChange)
		{
			return true;
		}
		if (method == MethodName.CanSetValue)
		{
			return true;
		}
		if (method == MethodName._GetType)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._checkbox)
		{
			_checkbox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName.value)
		{
			this.value = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._checkbox)
		{
			value = VariantUtils.CreateFrom(in _checkbox);
			return true;
		}
		if (name == PropertyName.value)
		{
			value = VariantUtils.CreateFrom(in this.value);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._checkbox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.value, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._checkbox, Variant.From(in _checkbox));
		info.AddProperty(PropertyName.value, Variant.From(in value));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._checkbox, out var variant))
		{
			_checkbox = variant.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName.value, out var variant2))
		{
			value = variant2.As<bool>();
		}
	}
}
