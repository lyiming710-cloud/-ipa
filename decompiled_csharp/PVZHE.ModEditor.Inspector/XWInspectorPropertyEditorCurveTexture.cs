using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/ResourceInspectorExtend/CurveTexture/XWInspectorPropertyEditorCurveTexture.cs")]
public class XWInspectorPropertyEditorCurveTexture : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName SetupOptions = "SetupOptions";

		public static readonly StringName SetupPicker = "SetupPicker";

		public static readonly StringName RefreshTexture = "RefreshTexture";

		public static readonly StringName ReadTexture = "ReadTexture";

		public static readonly StringName OnCurveChanged = "OnCurveChanged";

		public static readonly StringName OnWidthChanged = "OnWidthChanged";

		public static readonly StringName OnTextureModeSelected = "OnTextureModeSelected";

		public static readonly StringName CommitTextureChange = "CommitTextureChange";

		public static readonly StringName GetItemIndexById = "GetItemIndexById";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _curveTexturePreview = "_curveTexturePreview";

		public static readonly StringName _curvePicker = "_curvePicker";

		public static readonly StringName _curveXPicker = "_curveXPicker";

		public static readonly StringName _curveYPicker = "_curveYPicker";

		public static readonly StringName _curveZPicker = "_curveZPicker";

		public static readonly StringName _widthSpinBox = "_widthSpinBox";

		public static readonly StringName _textureModeOption = "_textureModeOption";

		public static readonly StringName _singleCurveControls = "_singleCurveControls";

		public static readonly StringName _xyzCurveControls = "_xyzCurveControls";

		public static readonly StringName _texture = "_texture";

		public static readonly StringName _updating = "_updating";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private TextureRect _curveTexturePreview;

	private XWResourcePicker _curvePicker;

	private XWResourcePicker _curveXPicker;

	private XWResourcePicker _curveYPicker;

	private XWResourcePicker _curveZPicker;

	private SpinBox _widthSpinBox;

	private OptionButton _textureModeOption;

	private Control _singleCurveControls;

	private Control _xyzCurveControls;

	private Texture2D _texture;

	private bool _updating;

	public override void _Ready()
	{
		base._Ready();
		_curveTexturePreview = GetNode<TextureRect>("%CurveTexturePreview");
		_curvePicker = GetNode<XWResourcePicker>("%CurvePicker");
		_curveXPicker = GetNode<XWResourcePicker>("%CurveXPicker");
		_curveYPicker = GetNode<XWResourcePicker>("%CurveYPicker");
		_curveZPicker = GetNode<XWResourcePicker>("%CurveZPicker");
		_widthSpinBox = GetNode<SpinBox>("%WidthSpinBox");
		_textureModeOption = GetNode<OptionButton>("%TextureModeOption");
		_singleCurveControls = GetNode<Control>("%SingleCurveControls");
		_xyzCurveControls = GetNode<Control>("%XyzCurveControls");
		SetupOptions();
		SetupPicker(_curvePicker);
		SetupPicker(_curveXPicker);
		SetupPicker(_curveYPicker);
		SetupPicker(_curveZPicker);
		_curvePicker.ResourceChanged += OnCurveChanged;
		_curveXPicker.ResourceChanged += (Resource resource) =>
		{
			OnCurveChanged("x", resource);
		};
		_curveYPicker.ResourceChanged += (Resource resource) =>
		{
			OnCurveChanged("y", resource);
		};
		_curveZPicker.ResourceChanged += (Resource resource) =>
		{
			OnCurveChanged("z", resource);
		};
		_widthSpinBox.ValueChanged += OnWidthChanged;
		_textureModeOption.ItemSelected += OnTextureModeSelected;
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		base.SetEditProperty(property, field);
		RefreshTexture();
	}

	public override void UpdateValue()
	{
		RefreshTexture();
	}

	public override Variant GetValue()
	{
		return _texture;
	}

	private void SetupOptions()
	{
		_textureModeOption.Clear();
		_textureModeOption.AddItem("RGB", 0);
		_textureModeOption.AddItem("Red", 1);
	}

	private static void SetupPicker(XWResourcePicker picker)
	{
		picker.Setup("Curve");
	}

	private void RefreshTexture()
	{
		_texture = ReadTexture();
		_updating = true;
		bool flag = _texture is CurveTexture;
		bool flag2 = _texture is CurveXyzTexture;
		_curveTexturePreview.Texture = (GodotObject.IsInstanceValid(_texture) ? _texture : null);
		_singleCurveControls.Visible = flag;
		_xyzCurveControls.Visible = flag2;
		_widthSpinBox.Editable = flag | flag2;
		_textureModeOption.Disabled = !flag;
		if (_texture is CurveTexture curveTexture)
		{
			_widthSpinBox.Value = curveTexture.Width;
			_textureModeOption.Select(GetItemIndexById(_textureModeOption, (int)curveTexture.TextureMode));
			_curvePicker.SetEditedResource(curveTexture.Curve);
		}
		else if (_texture is CurveXyzTexture curveXyzTexture)
		{
			_widthSpinBox.Value = curveXyzTexture.Width;
			_curveXPicker.SetEditedResource(curveXyzTexture.CurveX);
			_curveYPicker.SetEditedResource(curveXyzTexture.CurveY);
			_curveZPicker.SetEditedResource(curveXyzTexture.CurveZ);
		}
		_updating = false;
	}

	private Texture2D ReadTexture()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Object || !(propertyValue.As<GodotObject>() is Texture2D result))
		{
			return null;
		}
		return result;
	}

	private void OnCurveChanged(Resource resource)
	{
		if (!_updating && _texture is CurveTexture curveTexture)
		{
			curveTexture.Curve = resource as Curve;
			CommitTextureChange();
		}
	}

	private void OnCurveChanged(string channel, Resource resource)
	{
		if (!_updating && _texture is CurveXyzTexture curveXyzTexture)
		{
			Curve curve = resource as Curve;
			switch (channel)
			{
			case "x":
				curveXyzTexture.CurveX = curve;
				break;
			case "y":
				curveXyzTexture.CurveY = curve;
				break;
			case "z":
				curveXyzTexture.CurveZ = curve;
				break;
			}
			CommitTextureChange();
		}
	}

	private void OnWidthChanged(double value)
	{
		if (!_updating && GodotObject.IsInstanceValid(_texture))
		{
			int width = Mathf.Max(1, (int)value);
			if (_texture is CurveTexture curveTexture)
			{
				curveTexture.Width = width;
			}
			else if (_texture is CurveXyzTexture curveXyzTexture)
			{
				curveXyzTexture.Width = width;
			}
			CommitTextureChange();
		}
	}

	private void OnTextureModeSelected(long index)
	{
		if (!_updating && _texture is CurveTexture curveTexture)
		{
			curveTexture.TextureMode = (CurveTexture.TextureModeEnum)_textureModeOption.GetItemId((int)index);
			CommitTextureChange();
		}
	}

	private void CommitTextureChange()
	{
		if (GodotObject.IsInstanceValid(_texture))
		{
			_texture.EmitChanged();
			if (GodotObject.IsInstanceValid(Property) && Property.PropName != (StringName)"")
			{
				ValueChange(_texture);
			}
			else
			{
				Property?.SetCall();
			}
			_curveTexturePreview.Texture = _texture;
		}
	}

	private static int GetItemIndexById(OptionButton option, int id)
	{
		for (int i = 0; i < option.ItemCount; i++)
		{
			if (option.GetItemId(i) == id)
			{
				return i;
			}
		}
		return 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(14)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupOptions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "picker", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCurveChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnCurveChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "channel", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnWidthChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTextureModeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitTextureChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetItemIndexById, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SetEditProperty && args.Count == 2)
		{
			SetEditProperty(VariantUtils.ConvertTo<XWInspectorProperty>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
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
		if (method == MethodName.SetupOptions && args.Count == 0)
		{
			SetupOptions();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupPicker && args.Count == 1)
		{
			SetupPicker(VariantUtils.ConvertTo<XWResourcePicker>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshTexture && args.Count == 0)
		{
			RefreshTexture();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadTexture && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(ReadTexture());
			return true;
		}
		if (method == MethodName.OnCurveChanged && args.Count == 1)
		{
			OnCurveChanged(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCurveChanged && args.Count == 2)
		{
			OnCurveChanged(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnWidthChanged && args.Count == 1)
		{
			OnWidthChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTextureModeSelected && args.Count == 1)
		{
			OnTextureModeSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitTextureChange && args.Count == 0)
		{
			CommitTextureChange();
			ret = default;
			return true;
		}
		if (method == MethodName.GetItemIndexById && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetItemIndexById(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetupPicker && args.Count == 1)
		{
			SetupPicker(VariantUtils.ConvertTo<XWResourcePicker>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetItemIndexById && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetItemIndexById(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.SetEditProperty)
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
		if (method == MethodName.SetupOptions)
		{
			return true;
		}
		if (method == MethodName.SetupPicker)
		{
			return true;
		}
		if (method == MethodName.RefreshTexture)
		{
			return true;
		}
		if (method == MethodName.ReadTexture)
		{
			return true;
		}
		if (method == MethodName.OnCurveChanged)
		{
			return true;
		}
		if (method == MethodName.OnWidthChanged)
		{
			return true;
		}
		if (method == MethodName.OnTextureModeSelected)
		{
			return true;
		}
		if (method == MethodName.CommitTextureChange)
		{
			return true;
		}
		if (method == MethodName.GetItemIndexById)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._curveTexturePreview)
		{
			_curveTexturePreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._curvePicker)
		{
			_curvePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._curveXPicker)
		{
			_curveXPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._curveYPicker)
		{
			_curveYPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._curveZPicker)
		{
			_curveZPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._widthSpinBox)
		{
			_widthSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._textureModeOption)
		{
			_textureModeOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._singleCurveControls)
		{
			_singleCurveControls = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._xyzCurveControls)
		{
			_xyzCurveControls = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._texture)
		{
			_texture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._updating)
		{
			_updating = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._curveTexturePreview)
		{
			value = VariantUtils.CreateFrom(in _curveTexturePreview);
			return true;
		}
		if (name == PropertyName._curvePicker)
		{
			value = VariantUtils.CreateFrom(in _curvePicker);
			return true;
		}
		if (name == PropertyName._curveXPicker)
		{
			value = VariantUtils.CreateFrom(in _curveXPicker);
			return true;
		}
		if (name == PropertyName._curveYPicker)
		{
			value = VariantUtils.CreateFrom(in _curveYPicker);
			return true;
		}
		if (name == PropertyName._curveZPicker)
		{
			value = VariantUtils.CreateFrom(in _curveZPicker);
			return true;
		}
		if (name == PropertyName._widthSpinBox)
		{
			value = VariantUtils.CreateFrom(in _widthSpinBox);
			return true;
		}
		if (name == PropertyName._textureModeOption)
		{
			value = VariantUtils.CreateFrom(in _textureModeOption);
			return true;
		}
		if (name == PropertyName._singleCurveControls)
		{
			value = VariantUtils.CreateFrom(in _singleCurveControls);
			return true;
		}
		if (name == PropertyName._xyzCurveControls)
		{
			value = VariantUtils.CreateFrom(in _xyzCurveControls);
			return true;
		}
		if (name == PropertyName._texture)
		{
			value = VariantUtils.CreateFrom(in _texture);
			return true;
		}
		if (name == PropertyName._updating)
		{
			value = VariantUtils.CreateFrom(in _updating);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._curveTexturePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._curvePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._curveXPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._curveYPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._curveZPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._widthSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._textureModeOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._singleCurveControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._xyzCurveControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._curveTexturePreview, Variant.From(in _curveTexturePreview));
		info.AddProperty(PropertyName._curvePicker, Variant.From(in _curvePicker));
		info.AddProperty(PropertyName._curveXPicker, Variant.From(in _curveXPicker));
		info.AddProperty(PropertyName._curveYPicker, Variant.From(in _curveYPicker));
		info.AddProperty(PropertyName._curveZPicker, Variant.From(in _curveZPicker));
		info.AddProperty(PropertyName._widthSpinBox, Variant.From(in _widthSpinBox));
		info.AddProperty(PropertyName._textureModeOption, Variant.From(in _textureModeOption));
		info.AddProperty(PropertyName._singleCurveControls, Variant.From(in _singleCurveControls));
		info.AddProperty(PropertyName._xyzCurveControls, Variant.From(in _xyzCurveControls));
		info.AddProperty(PropertyName._texture, Variant.From(in _texture));
		info.AddProperty(PropertyName._updating, Variant.From(in _updating));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._curveTexturePreview, out var value))
		{
			_curveTexturePreview = value.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._curvePicker, out var value2))
		{
			_curvePicker = value2.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._curveXPicker, out var value3))
		{
			_curveXPicker = value3.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._curveYPicker, out var value4))
		{
			_curveYPicker = value4.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._curveZPicker, out var value5))
		{
			_curveZPicker = value5.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._widthSpinBox, out var value6))
		{
			_widthSpinBox = value6.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._textureModeOption, out var value7))
		{
			_textureModeOption = value7.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._singleCurveControls, out var value8))
		{
			_singleCurveControls = value8.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._xyzCurveControls, out var value9))
		{
			_xyzCurveControls = value9.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._texture, out var value10))
		{
			_texture = value10.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._updating, out var value11))
		{
			_updating = value11.As<bool>();
		}
	}
}
