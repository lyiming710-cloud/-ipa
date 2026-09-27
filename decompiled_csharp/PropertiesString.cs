using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/Properties/Type/String/PropertiesString.cs")]
public class PropertiesString : PropertiesBase
{
	public delegate void ValueChangedEventHandler(string value);

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
		public static readonly StringName _lineEdit = "_lineEdit";

		public static readonly StringName value = "value";
	}

	public new class SignalName : PropertiesBase.SignalName
	{
	}

	private LineEdit _lineEdit;

	public string value = "";

	public event ValueChangedEventHandler OnValueChanged;

	public override void _Ready()
	{
		_lineEdit = GetNode<LineEdit>("%LineEdit");
		value = _lineEdit.Text;
		_lineEdit.TextChanged += ValueChange;
	}

	public void SetValue(string newValue)
	{
		if (IsNodeReady())
		{
			value = newValue;
			if (CanSetValue())
			{
				_lineEdit.TextChanged -= ValueChange;
				_lineEdit.Text = value;
				_lineEdit.TextChanged += ValueChange;
			}
		}
	}

	private void ValueChange(string newText)
	{
		value = _lineEdit.Text;
		OnValueChanged?.Invoke(value);
	}

	public override bool CanSetValue()
	{
		return !_lineEdit.HasFocus();
	}

	public override string _GetType()
	{
		return "String";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			SetValue(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ValueChange && args.Count == 1)
		{
			ValueChange(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (name == PropertyName._lineEdit)
		{
			_lineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName.value)
		{
			this.value = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._lineEdit)
		{
			value = VariantUtils.CreateFrom(in _lineEdit);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._lineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.value, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._lineEdit, Variant.From(in _lineEdit));
		info.AddProperty(PropertyName.value, Variant.From(in value));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._lineEdit, out var variant))
		{
			_lineEdit = variant.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName.value, out var variant2))
		{
			value = variant2.As<string>();
		}
	}
}
