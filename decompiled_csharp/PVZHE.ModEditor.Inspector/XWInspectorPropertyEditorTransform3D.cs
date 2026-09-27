using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Transform3D/XWInspectorPropertyEditorTransform3D.cs")]
public class XWInspectorPropertyEditorTransform3D : XWInspectorPropertyEditorBase
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

		public static readonly StringName _zxSpinBox = "_zxSpinBox";

		public static readonly StringName _xySpinBox = "_xySpinBox";

		public static readonly StringName _yySpinBox = "_yySpinBox";

		public static readonly StringName _zySpinBox = "_zySpinBox";

		public static readonly StringName _xzSpinBox = "_xzSpinBox";

		public static readonly StringName _yzSpinBox = "_yzSpinBox";

		public static readonly StringName _zzSpinBox = "_zzSpinBox";

		public static readonly StringName _oxSpinBox = "_oxSpinBox";

		public static readonly StringName _oySpinBox = "_oySpinBox";

		public static readonly StringName _ozSpinBox = "_ozSpinBox";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private SpinBox _xxSpinBox;

	private SpinBox _yxSpinBox;

	private SpinBox _zxSpinBox;

	private SpinBox _xySpinBox;

	private SpinBox _yySpinBox;

	private SpinBox _zySpinBox;

	private SpinBox _xzSpinBox;

	private SpinBox _yzSpinBox;

	private SpinBox _zzSpinBox;

	private SpinBox _oxSpinBox;

	private SpinBox _oySpinBox;

	private SpinBox _ozSpinBox;

	public override void _Ready()
	{
		base._Ready();
		VBoxContainer node = GetNode<VBoxContainer>("%EditorContainer");
		_xxSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "xx", XWInspectorPropertyEditorFactory.ColorX);
		_yxSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "yx", XWInspectorPropertyEditorFactory.ColorY);
		_zxSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "zx", XWInspectorPropertyEditorFactory.ColorZ);
		_xySpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "xy", XWInspectorPropertyEditorFactory.ColorX);
		_yySpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "yy", XWInspectorPropertyEditorFactory.ColorY);
		_zySpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "zy", XWInspectorPropertyEditorFactory.ColorZ);
		_xzSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "xz", XWInspectorPropertyEditorFactory.ColorX);
		_yzSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "yz", XWInspectorPropertyEditorFactory.ColorY);
		_zzSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "zz", XWInspectorPropertyEditorFactory.ColorZ);
		_oxSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "ox", XWInspectorPropertyEditorFactory.ColorX);
		_oySpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "oy", XWInspectorPropertyEditorFactory.ColorY);
		_ozSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "oz", XWInspectorPropertyEditorFactory.ColorZ);
		_xxSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "basis:x:x");
		};
		_yxSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "basis:x:y");
		};
		_zxSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "basis:x:z");
		};
		_xySpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "basis:y:x");
		};
		_yySpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "basis:y:y");
		};
		_zySpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "basis:y:z");
		};
		_xzSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "basis:z:x");
		};
		_yzSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "basis:z:y");
		};
		_zzSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "basis:z:z");
		};
		_oxSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "origin:x");
		};
		_oySpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "origin:y");
		};
		_ozSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "origin:z");
		};
	}

	public override void UpdateValue()
	{
		Transform3D transform3D = GetPropertyValue().AsTransform3D();
		_xxSpinBox.Value = transform3D.Basis.X.X;
		_yxSpinBox.Value = transform3D.Basis.X.Y;
		_zxSpinBox.Value = transform3D.Basis.X.Z;
		_xySpinBox.Value = transform3D.Basis.Y.X;
		_yySpinBox.Value = transform3D.Basis.Y.Y;
		_zySpinBox.Value = transform3D.Basis.Y.Z;
		_xzSpinBox.Value = transform3D.Basis.Z.X;
		_yzSpinBox.Value = transform3D.Basis.Z.Y;
		_zzSpinBox.Value = transform3D.Basis.Z.Z;
		_oxSpinBox.Value = transform3D.Origin.X;
		_oySpinBox.Value = transform3D.Origin.Y;
		_ozSpinBox.Value = transform3D.Origin.Z;
	}

	public override Variant GetValue()
	{
		Basis basis = new Basis(new Vector3((float)_xxSpinBox.Value, (float)_yxSpinBox.Value, (float)_zxSpinBox.Value), new Vector3((float)_xySpinBox.Value, (float)_yySpinBox.Value, (float)_zySpinBox.Value), new Vector3((float)_xzSpinBox.Value, (float)_yzSpinBox.Value, (float)_zzSpinBox.Value));
		Vector3 origin = new Vector3((float)_oxSpinBox.Value, (float)_oySpinBox.Value, (float)_ozSpinBox.Value);
		return new Transform3D(basis, origin);
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
		if (name == PropertyName._zxSpinBox)
		{
			_zxSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
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
		if (name == PropertyName._zySpinBox)
		{
			_zySpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._xzSpinBox)
		{
			_xzSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._yzSpinBox)
		{
			_yzSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._zzSpinBox)
		{
			_zzSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._oxSpinBox)
		{
			_oxSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._oySpinBox)
		{
			_oySpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._ozSpinBox)
		{
			_ozSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
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
		if (name == PropertyName._zxSpinBox)
		{
			value = VariantUtils.CreateFrom(in _zxSpinBox);
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
		if (name == PropertyName._zySpinBox)
		{
			value = VariantUtils.CreateFrom(in _zySpinBox);
			return true;
		}
		if (name == PropertyName._xzSpinBox)
		{
			value = VariantUtils.CreateFrom(in _xzSpinBox);
			return true;
		}
		if (name == PropertyName._yzSpinBox)
		{
			value = VariantUtils.CreateFrom(in _yzSpinBox);
			return true;
		}
		if (name == PropertyName._zzSpinBox)
		{
			value = VariantUtils.CreateFrom(in _zzSpinBox);
			return true;
		}
		if (name == PropertyName._oxSpinBox)
		{
			value = VariantUtils.CreateFrom(in _oxSpinBox);
			return true;
		}
		if (name == PropertyName._oySpinBox)
		{
			value = VariantUtils.CreateFrom(in _oySpinBox);
			return true;
		}
		if (name == PropertyName._ozSpinBox)
		{
			value = VariantUtils.CreateFrom(in _ozSpinBox);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._zxSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._xySpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._yySpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zySpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._xzSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._yzSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zzSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._oxSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._oySpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ozSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._xxSpinBox, Variant.From(in _xxSpinBox));
		info.AddProperty(PropertyName._yxSpinBox, Variant.From(in _yxSpinBox));
		info.AddProperty(PropertyName._zxSpinBox, Variant.From(in _zxSpinBox));
		info.AddProperty(PropertyName._xySpinBox, Variant.From(in _xySpinBox));
		info.AddProperty(PropertyName._yySpinBox, Variant.From(in _yySpinBox));
		info.AddProperty(PropertyName._zySpinBox, Variant.From(in _zySpinBox));
		info.AddProperty(PropertyName._xzSpinBox, Variant.From(in _xzSpinBox));
		info.AddProperty(PropertyName._yzSpinBox, Variant.From(in _yzSpinBox));
		info.AddProperty(PropertyName._zzSpinBox, Variant.From(in _zzSpinBox));
		info.AddProperty(PropertyName._oxSpinBox, Variant.From(in _oxSpinBox));
		info.AddProperty(PropertyName._oySpinBox, Variant.From(in _oySpinBox));
		info.AddProperty(PropertyName._ozSpinBox, Variant.From(in _ozSpinBox));
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
		if (info.TryGetProperty(PropertyName._zxSpinBox, out var value3))
		{
			_zxSpinBox = value3.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._xySpinBox, out var value4))
		{
			_xySpinBox = value4.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._yySpinBox, out var value5))
		{
			_yySpinBox = value5.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._zySpinBox, out var value6))
		{
			_zySpinBox = value6.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._xzSpinBox, out var value7))
		{
			_xzSpinBox = value7.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._yzSpinBox, out var value8))
		{
			_yzSpinBox = value8.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._zzSpinBox, out var value9))
		{
			_zzSpinBox = value9.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._oxSpinBox, out var value10))
		{
			_oxSpinBox = value10.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._oySpinBox, out var value11))
		{
			_oySpinBox = value11.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._ozSpinBox, out var value12))
		{
			_ozSpinBox = value12.As<SpinBox>();
		}
	}
}
