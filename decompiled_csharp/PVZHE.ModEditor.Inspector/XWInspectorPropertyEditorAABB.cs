using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/AABB/XWInspectorPropertyEditorAABB.cs")]
public class XWInspectorPropertyEditorAABB : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _pxSpinBox = "_pxSpinBox";

		public static readonly StringName _pySpinBox = "_pySpinBox";

		public static readonly StringName _pzSpinBox = "_pzSpinBox";

		public static readonly StringName _sxSpinBox = "_sxSpinBox";

		public static readonly StringName _sySpinBox = "_sySpinBox";

		public static readonly StringName _szSpinBox = "_szSpinBox";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private SpinBox _pxSpinBox;

	private SpinBox _pySpinBox;

	private SpinBox _pzSpinBox;

	private SpinBox _sxSpinBox;

	private SpinBox _sySpinBox;

	private SpinBox _szSpinBox;

	public override void _Ready()
	{
		base._Ready();
		VBoxContainer node = GetNode<VBoxContainer>("%EditorContainer");
		_pxSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "px", XWInspectorPropertyEditorFactory.ColorX);
		_pySpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "py", XWInspectorPropertyEditorFactory.ColorY);
		_pzSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "pz", XWInspectorPropertyEditorFactory.ColorZ);
		_sxSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "sx", XWInspectorPropertyEditorFactory.ColorX);
		_sySpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "sy", XWInspectorPropertyEditorFactory.ColorY);
		_szSpinBox = XWInspectorPropertyEditorFactory.CreateVectorRow(node, "sz", XWInspectorPropertyEditorFactory.ColorZ);
		_pxSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "position:x");
		};
		_pySpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "position:y");
		};
		_pzSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "position:z");
		};
		_sxSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "size:x");
		};
		_sySpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "size:y");
		};
		_szSpinBox.ValueChanged += (double v) =>
		{
			ValueChange(v, "size:z");
		};
	}

	public override void UpdateValue()
	{
		Aabb aabb = GetPropertyValue().AsAabb();
		_pxSpinBox.Value = aabb.Position.X;
		_pySpinBox.Value = aabb.Position.Y;
		_pzSpinBox.Value = aabb.Position.Z;
		_sxSpinBox.Value = aabb.Size.X;
		_sySpinBox.Value = aabb.Size.Y;
		_szSpinBox.Value = aabb.Size.Z;
	}

	public override Variant GetValue()
	{
		return new Aabb(new Vector3((float)_pxSpinBox.Value, (float)_pySpinBox.Value, (float)_pzSpinBox.Value), new Vector3((float)_sxSpinBox.Value, (float)_sySpinBox.Value, (float)_szSpinBox.Value));
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
		if (name == PropertyName._pxSpinBox)
		{
			_pxSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._pySpinBox)
		{
			_pySpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._pzSpinBox)
		{
			_pzSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._sxSpinBox)
		{
			_sxSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._sySpinBox)
		{
			_sySpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._szSpinBox)
		{
			_szSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._pxSpinBox)
		{
			value = VariantUtils.CreateFrom(in _pxSpinBox);
			return true;
		}
		if (name == PropertyName._pySpinBox)
		{
			value = VariantUtils.CreateFrom(in _pySpinBox);
			return true;
		}
		if (name == PropertyName._pzSpinBox)
		{
			value = VariantUtils.CreateFrom(in _pzSpinBox);
			return true;
		}
		if (name == PropertyName._sxSpinBox)
		{
			value = VariantUtils.CreateFrom(in _sxSpinBox);
			return true;
		}
		if (name == PropertyName._sySpinBox)
		{
			value = VariantUtils.CreateFrom(in _sySpinBox);
			return true;
		}
		if (name == PropertyName._szSpinBox)
		{
			value = VariantUtils.CreateFrom(in _szSpinBox);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._pxSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pySpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pzSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sxSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sySpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._szSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._pxSpinBox, Variant.From(in _pxSpinBox));
		info.AddProperty(PropertyName._pySpinBox, Variant.From(in _pySpinBox));
		info.AddProperty(PropertyName._pzSpinBox, Variant.From(in _pzSpinBox));
		info.AddProperty(PropertyName._sxSpinBox, Variant.From(in _sxSpinBox));
		info.AddProperty(PropertyName._sySpinBox, Variant.From(in _sySpinBox));
		info.AddProperty(PropertyName._szSpinBox, Variant.From(in _szSpinBox));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._pxSpinBox, out var value))
		{
			_pxSpinBox = value.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._pySpinBox, out var value2))
		{
			_pySpinBox = value2.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._pzSpinBox, out var value3))
		{
			_pzSpinBox = value3.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._sxSpinBox, out var value4))
		{
			_sxSpinBox = value4.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._sySpinBox, out var value5))
		{
			_sySpinBox = value5.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._szSpinBox, out var value6))
		{
			_szSpinBox = value6.As<SpinBox>();
		}
	}
}
