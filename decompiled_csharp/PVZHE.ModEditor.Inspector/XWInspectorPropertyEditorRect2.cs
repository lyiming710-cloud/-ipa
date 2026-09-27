using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Rect2/XWInspectorPropertyEditorRect2.cs")]
public class XWInspectorPropertyEditorRect2 : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _xSpinBox = "_xSpinBox";

		public static readonly StringName _ySpinBox = "_ySpinBox";

		public static readonly StringName _wSpinBox = "_wSpinBox";

		public static readonly StringName _hSpinBox = "_hSpinBox";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private SpinBox _xSpinBox;

	private SpinBox _ySpinBox;

	private SpinBox _wSpinBox;

	private SpinBox _hSpinBox;

	public override void _Ready()
	{
		base._Ready();
		VBoxContainer node = GetNode<VBoxContainer>("%EditorContainer");
		_xSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "x", XWInspectorPropertyEditorFactory.ColorX);
		_ySpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "y", XWInspectorPropertyEditorFactory.ColorY);
		_wSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "w", XWInspectorPropertyEditorFactory.ColorW);
		_hSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "h", XWInspectorPropertyEditorFactory.ColorW);
		_xSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "position:x");
		};
		_ySpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "position:y");
		};
		_wSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "size:x");
		};
		_hSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "size:y");
		};
	}

	public override void UpdateValue()
	{
		Rect2 rect = GetPropertyValue().AsRect2();
		_xSpinBox.Value = rect.Position.X;
		_ySpinBox.Value = rect.Position.Y;
		_wSpinBox.Value = rect.Size.X;
		_hSpinBox.Value = rect.Size.Y;
	}

	public override Variant GetValue()
	{
		return new Rect2(new Vector2((float)_xSpinBox.Value, (float)_ySpinBox.Value), new Vector2((float)_wSpinBox.Value, (float)_hSpinBox.Value));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.UpdateValue && args.Count == 0)
		{
			UpdateValue();
			ret = default;
			return true;
		}
		if (method == MethodName.GetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetValue());
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
		if (method == MethodName.UpdateValue)
		{
			return true;
		}
		if (method == MethodName.GetValue)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._xSpinBox)
		{
			_xSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._ySpinBox)
		{
			_ySpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._wSpinBox)
		{
			_wSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._hSpinBox)
		{
			_hSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._xSpinBox)
		{
			value = VariantUtils.CreateFrom(in _xSpinBox);
			return true;
		}
		if (name == PropertyName._ySpinBox)
		{
			value = VariantUtils.CreateFrom(in _ySpinBox);
			return true;
		}
		if (name == PropertyName._wSpinBox)
		{
			value = VariantUtils.CreateFrom(in _wSpinBox);
			return true;
		}
		if (name == PropertyName._hSpinBox)
		{
			value = VariantUtils.CreateFrom(in _hSpinBox);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._xSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ySpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._wSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._xSpinBox, Variant.From(in _xSpinBox));
		info.AddProperty(PropertyName._ySpinBox, Variant.From(in _ySpinBox));
		info.AddProperty(PropertyName._wSpinBox, Variant.From(in _wSpinBox));
		info.AddProperty(PropertyName._hSpinBox, Variant.From(in _hSpinBox));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._xSpinBox, out var value))
		{
			_xSpinBox = value.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._ySpinBox, out var value2))
		{
			_ySpinBox = value2.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._wSpinBox, out var value3))
		{
			_wSpinBox = value3.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._hSpinBox, out var value4))
		{
			_hSpinBox = value4.As<SpinBox>();
		}
	}
}
