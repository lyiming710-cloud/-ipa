using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Plane/XWInspectorPropertyEditorPlane.cs")]
public class XWInspectorPropertyEditorPlane : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _nxSpinBox = "_nxSpinBox";

		public static readonly StringName _nySpinBox = "_nySpinBox";

		public static readonly StringName _nzSpinBox = "_nzSpinBox";

		public static readonly StringName _dSpinBox = "_dSpinBox";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private SpinBox _nxSpinBox;

	private SpinBox _nySpinBox;

	private SpinBox _nzSpinBox;

	private SpinBox _dSpinBox;

	public override void _Ready()
	{
		base._Ready();
		VBoxContainer node = GetNode<VBoxContainer>("%EditorContainer");
		_nxSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "nx", XWInspectorPropertyEditorFactory.ColorX);
		_nySpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "ny", XWInspectorPropertyEditorFactory.ColorY);
		_nzSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "nz", XWInspectorPropertyEditorFactory.ColorZ);
		_dSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "d", XWInspectorPropertyEditorFactory.ColorW);
		_nxSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "normal:x");
		};
		_nySpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "normal:y");
		};
		_nzSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "normal:z");
		};
		_dSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "d");
		};
	}

	public override void UpdateValue()
	{
		Plane plane = GetPropertyValue().AsPlane();
		_nxSpinBox.Value = plane.Normal.X;
		_nySpinBox.Value = plane.Normal.Y;
		_nzSpinBox.Value = plane.Normal.Z;
		_dSpinBox.Value = plane.D;
	}

	public override Variant GetValue()
	{
		return new Plane((float)_nxSpinBox.Value, (float)_nySpinBox.Value, (float)_nzSpinBox.Value, (float)_dSpinBox.Value);
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
		if (name == PropertyName._nxSpinBox)
		{
			_nxSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._nySpinBox)
		{
			_nySpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._nzSpinBox)
		{
			_nzSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._dSpinBox)
		{
			_dSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._nxSpinBox)
		{
			value = VariantUtils.CreateFrom(in _nxSpinBox);
			return true;
		}
		if (name == PropertyName._nySpinBox)
		{
			value = VariantUtils.CreateFrom(in _nySpinBox);
			return true;
		}
		if (name == PropertyName._nzSpinBox)
		{
			value = VariantUtils.CreateFrom(in _nzSpinBox);
			return true;
		}
		if (name == PropertyName._dSpinBox)
		{
			value = VariantUtils.CreateFrom(in _dSpinBox);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._nxSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nySpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nzSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._dSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._nxSpinBox, Variant.From(in _nxSpinBox));
		info.AddProperty(PropertyName._nySpinBox, Variant.From(in _nySpinBox));
		info.AddProperty(PropertyName._nzSpinBox, Variant.From(in _nzSpinBox));
		info.AddProperty(PropertyName._dSpinBox, Variant.From(in _dSpinBox));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._nxSpinBox, out var value))
		{
			_nxSpinBox = value.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._nySpinBox, out var value2))
		{
			_nySpinBox = value2.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._nzSpinBox, out var value3))
		{
			_nzSpinBox = value3.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._dSpinBox, out var value4))
		{
			_dSpinBox = value4.As<SpinBox>();
		}
	}
}
