using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/Properties/Type/Vector2/PropertiesVector2.cs")]
public class PropertiesVector2 : PropertiesBase
{
	public delegate void ValueChangedEventHandler(Vector2 value);

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
		public static readonly StringName _spinBoxX = "_spinBoxX";

		public static readonly StringName _spinBoxY = "_spinBoxY";

		public static readonly StringName value = "value";
	}

	public new class SignalName : PropertiesBase.SignalName
	{
	}

	private SpinBox _spinBoxX;

	private SpinBox _spinBoxY;

	public Vector2 value = Vector2.Zero;

	public event ValueChangedEventHandler OnValueChanged;

	public override void _Ready()
	{
		_spinBoxX = GetNode<SpinBox>("%SpinBoxX");
		_spinBoxY = GetNode<SpinBox>("%SpinBoxY");
		value = new Vector2((float)_spinBoxX.Value, (float)_spinBoxY.Value);
		_spinBoxX.ValueChanged += ValueChange;
		_spinBoxY.ValueChanged += ValueChange;
	}

	public void SetValue(Vector2 newValue)
	{
		if (IsNodeReady())
		{
			value = newValue;
			_spinBoxX.ValueChanged -= ValueChange;
			_spinBoxY.ValueChanged -= ValueChange;
			_spinBoxX.Value = value.X;
			_spinBoxY.Value = value.Y;
			_spinBoxX.ValueChanged += ValueChange;
			_spinBoxY.ValueChanged += ValueChange;
		}
	}

	private void ValueChange(double v)
	{
		value = new Vector2((float)_spinBoxX.Value, (float)_spinBoxY.Value);
		OnValueChanged?.Invoke(value);
	}

	public override bool CanSetValue()
	{
		if (!_spinBoxX.GetLineEdit().HasFocus())
		{
			return !_spinBoxY.GetLineEdit().HasFocus();
		}
		return false;
	}

	public override string _GetType()
	{
		return "Vector2";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			SetValue(VariantUtils.ConvertTo<Vector2>(in args[0]));
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
		if (name == PropertyName._spinBoxX)
		{
			_spinBoxX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._spinBoxY)
		{
			_spinBoxY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName.value)
		{
			this.value = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._spinBoxX)
		{
			value = VariantUtils.CreateFrom(in _spinBoxX);
			return true;
		}
		if (name == PropertyName._spinBoxY)
		{
			value = VariantUtils.CreateFrom(in _spinBoxY);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._spinBoxX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spinBoxY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.value, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._spinBoxX, Variant.From(in _spinBoxX));
		info.AddProperty(PropertyName._spinBoxY, Variant.From(in _spinBoxY));
		info.AddProperty(PropertyName.value, Variant.From(in value));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._spinBoxX, out var variant))
		{
			_spinBoxX = variant.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._spinBoxY, out var variant2))
		{
			_spinBoxY = variant2.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName.value, out var variant3))
		{
			value = variant3.As<Vector2>();
		}
	}
}
