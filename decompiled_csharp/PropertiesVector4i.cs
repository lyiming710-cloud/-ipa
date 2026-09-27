using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/Properties/Type/Vector4i/PropertiesVector4i.cs")]
public class PropertiesVector4i : PropertiesBase
{
	public delegate void ValueChangedEventHandler(Vector4I value);

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
		public static readonly StringName _spinBoxX1 = "_spinBoxX1";

		public static readonly StringName _spinBoxY1 = "_spinBoxY1";

		public static readonly StringName _spinBoxX2 = "_spinBoxX2";

		public static readonly StringName _spinBoxY2 = "_spinBoxY2";

		public static readonly StringName value = "value";
	}

	public new class SignalName : PropertiesBase.SignalName
	{
	}

	private SpinBox _spinBoxX1;

	private SpinBox _spinBoxY1;

	private SpinBox _spinBoxX2;

	private SpinBox _spinBoxY2;

	public Vector4I value = Vector4I.Zero;

	public event ValueChangedEventHandler OnValueChanged;

	public override void _Ready()
	{
		_spinBoxX1 = GetNode<SpinBox>("%SpinBoxX1");
		_spinBoxY1 = GetNode<SpinBox>("%SpinBoxY1");
		_spinBoxX2 = GetNode<SpinBox>("%SpinBoxX2");
		_spinBoxY2 = GetNode<SpinBox>("%SpinBoxY2");
		value = new Vector4I((int)_spinBoxX1.Value, (int)_spinBoxY1.Value, (int)_spinBoxX2.Value, (int)_spinBoxY2.Value);
		_spinBoxX1.ValueChanged += ValueChange;
		_spinBoxY1.ValueChanged += ValueChange;
		_spinBoxX2.ValueChanged += ValueChange;
		_spinBoxY2.ValueChanged += ValueChange;
	}

	public void SetValue(Vector4I newValue)
	{
		if (IsNodeReady())
		{
			value = newValue;
			_spinBoxX1.ValueChanged -= ValueChange;
			_spinBoxY1.ValueChanged -= ValueChange;
			_spinBoxX2.ValueChanged -= ValueChange;
			_spinBoxY2.ValueChanged -= ValueChange;
			_spinBoxX1.Value = value.X;
			_spinBoxY1.Value = value.Y;
			_spinBoxX2.Value = value.Z;
			_spinBoxY2.Value = value.W;
			_spinBoxX1.ValueChanged += ValueChange;
			_spinBoxY1.ValueChanged += ValueChange;
			_spinBoxX2.ValueChanged += ValueChange;
			_spinBoxY2.ValueChanged += ValueChange;
		}
	}

	private void ValueChange(double v)
	{
		value = new Vector4I((int)_spinBoxX1.Value, (int)_spinBoxY1.Value, (int)_spinBoxX2.Value, (int)_spinBoxY2.Value);
		OnValueChanged?.Invoke(value);
	}

	public override bool CanSetValue()
	{
		if (!_spinBoxX1.GetLineEdit().HasFocus() && !_spinBoxY1.GetLineEdit().HasFocus() && !_spinBoxX2.GetLineEdit().HasFocus())
		{
			return !_spinBoxY2.GetLineEdit().HasFocus();
		}
		return false;
	}

	public override string _GetType()
	{
		return "Vector4i";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector4I, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "v", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			SetValue(VariantUtils.ConvertTo<Vector4I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ValueChange && args.Count == 1)
		{
			ValueChange(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (name == PropertyName._spinBoxX1)
		{
			_spinBoxX1 = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._spinBoxY1)
		{
			_spinBoxY1 = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._spinBoxX2)
		{
			_spinBoxX2 = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._spinBoxY2)
		{
			_spinBoxY2 = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName.value)
		{
			this.value = VariantUtils.ConvertTo<Vector4I>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._spinBoxX1)
		{
			value = VariantUtils.CreateFrom(in _spinBoxX1);
			return true;
		}
		if (name == PropertyName._spinBoxY1)
		{
			value = VariantUtils.CreateFrom(in _spinBoxY1);
			return true;
		}
		if (name == PropertyName._spinBoxX2)
		{
			value = VariantUtils.CreateFrom(in _spinBoxX2);
			return true;
		}
		if (name == PropertyName._spinBoxY2)
		{
			value = VariantUtils.CreateFrom(in _spinBoxY2);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._spinBoxX1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spinBoxY1, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spinBoxX2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spinBoxY2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector4I, PropertyName.value, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._spinBoxX1, Variant.From(in _spinBoxX1));
		info.AddProperty(PropertyName._spinBoxY1, Variant.From(in _spinBoxY1));
		info.AddProperty(PropertyName._spinBoxX2, Variant.From(in _spinBoxX2));
		info.AddProperty(PropertyName._spinBoxY2, Variant.From(in _spinBoxY2));
		info.AddProperty(PropertyName.value, Variant.From(in value));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._spinBoxX1, out var variant))
		{
			_spinBoxX1 = variant.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._spinBoxY1, out var variant2))
		{
			_spinBoxY1 = variant2.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._spinBoxX2, out var variant3))
		{
			_spinBoxX2 = variant3.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._spinBoxY2, out var variant4))
		{
			_spinBoxY2 = variant4.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName.value, out var variant5))
		{
			value = variant5.As<Vector4I>();
		}
	}
}
