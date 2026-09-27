using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Transform2D/XWInspectorPropertyEditorTransform2D.cs")]
public class XWInspectorPropertyEditorTransform2D : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _xxSpinBox = "_xxSpinBox";

		public static readonly StringName _yxSpinBox = "_yxSpinBox";

		public static readonly StringName _xySpinBox = "_xySpinBox";

		public static readonly StringName _yySpinBox = "_yySpinBox";

		public static readonly StringName _xoSpinBox = "_xoSpinBox";

		public static readonly StringName _yoSpinBox = "_yoSpinBox";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private SpinBox _xxSpinBox;

	private SpinBox _yxSpinBox;

	private SpinBox _xySpinBox;

	private SpinBox _yySpinBox;

	private SpinBox _xoSpinBox;

	private SpinBox _yoSpinBox;

	public override void _Ready()
	{
		base._Ready();
		VBoxContainer node = GetNode<VBoxContainer>("%EditorContainer");
		_xxSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "xx", XWInspectorPropertyEditorFactory.ColorX);
		_yxSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "yx", XWInspectorPropertyEditorFactory.ColorY);
		_xySpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "xy", XWInspectorPropertyEditorFactory.ColorX);
		_yySpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "yy", XWInspectorPropertyEditorFactory.ColorY);
		_xoSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "xo", XWInspectorPropertyEditorFactory.ColorX);
		_yoSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "yo", XWInspectorPropertyEditorFactory.ColorY);
		_xxSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "x:x");
		};
		_yxSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "x:y");
		};
		_xySpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "y:x");
		};
		_yySpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "y:y");
		};
		_xoSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "origin:x");
		};
		_yoSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "origin:y");
		};
	}

	public override void UpdateValue()
	{
		Transform2D transform2D = GetPropertyValue().AsTransform2D();
		_xxSpinBox.Value = transform2D.X.X;
		_yxSpinBox.Value = transform2D.X.Y;
		_xySpinBox.Value = transform2D.Y.X;
		_yySpinBox.Value = transform2D.Y.Y;
		_xoSpinBox.Value = transform2D.Origin.X;
		_yoSpinBox.Value = transform2D.Origin.Y;
	}

	public override Variant GetValue()
	{
		return new Transform2D(new Vector2((float)_xxSpinBox.Value, (float)_yxSpinBox.Value), new Vector2((float)_xySpinBox.Value, (float)_yySpinBox.Value), new Vector2((float)_xoSpinBox.Value, (float)_yoSpinBox.Value));
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
		if (name == PropertyName._xxSpinBox)
		{
			_xxSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._yxSpinBox)
		{
			_yxSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._xySpinBox)
		{
			_xySpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._yySpinBox)
		{
			_yySpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._xoSpinBox)
		{
			_xoSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._yoSpinBox)
		{
			_yoSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._xxSpinBox)
		{
			value = VariantUtils.CreateFrom(in _xxSpinBox);
			return true;
		}
		if (name == PropertyName._yxSpinBox)
		{
			value = VariantUtils.CreateFrom(in _yxSpinBox);
			return true;
		}
		if (name == PropertyName._xySpinBox)
		{
			value = VariantUtils.CreateFrom(in _xySpinBox);
			return true;
		}
		if (name == PropertyName._yySpinBox)
		{
			value = VariantUtils.CreateFrom(in _yySpinBox);
			return true;
		}
		if (name == PropertyName._xoSpinBox)
		{
			value = VariantUtils.CreateFrom(in _xoSpinBox);
			return true;
		}
		if (name == PropertyName._yoSpinBox)
		{
			value = VariantUtils.CreateFrom(in _yoSpinBox);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._xxSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._yxSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._xySpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._yySpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._xoSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._yoSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._xxSpinBox, Variant.From(in _xxSpinBox));
		info.AddProperty(PropertyName._yxSpinBox, Variant.From(in _yxSpinBox));
		info.AddProperty(PropertyName._xySpinBox, Variant.From(in _xySpinBox));
		info.AddProperty(PropertyName._yySpinBox, Variant.From(in _yySpinBox));
		info.AddProperty(PropertyName._xoSpinBox, Variant.From(in _xoSpinBox));
		info.AddProperty(PropertyName._yoSpinBox, Variant.From(in _yoSpinBox));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._xxSpinBox, out var value))
		{
			_xxSpinBox = value.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._yxSpinBox, out var value2))
		{
			_yxSpinBox = value2.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._xySpinBox, out var value3))
		{
			_xySpinBox = value3.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._yySpinBox, out var value4))
		{
			_yySpinBox = value4.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._xoSpinBox, out var value5))
		{
			_xoSpinBox = value5.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._yoSpinBox, out var value6))
		{
			_yoSpinBox = value6.As<SpinBox>();
		}
	}
}
