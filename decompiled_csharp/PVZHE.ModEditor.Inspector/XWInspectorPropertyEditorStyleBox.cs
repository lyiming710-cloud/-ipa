using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/ResourceInspectorExtend/StyleBox/XWInspectorPropertyEditorStyleBox.cs")]
public class XWInspectorPropertyEditorStyleBox : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName RefreshStyleBox = "RefreshStyleBox";

		public static readonly StringName ReadStyleBox = "ReadStyleBox";

		public static readonly StringName OnDrawStyleBoxPreview = "OnDrawStyleBoxPreview";

		public static readonly StringName OnBgColorChanged = "OnBgColorChanged";

		public static readonly StringName OnBorderColorChanged = "OnBorderColorChanged";

		public static readonly StringName OnLineColorChanged = "OnLineColorChanged";

		public static readonly StringName OnModulateColorChanged = "OnModulateColorChanged";

		public static readonly StringName OnTextureChanged = "OnTextureChanged";

		public static readonly StringName OnBorderWidthChanged = "OnBorderWidthChanged";

		public static readonly StringName OnCornerRadiusChanged = "OnCornerRadiusChanged";

		public static readonly StringName OnTextureMarginChanged = "OnTextureMarginChanged";

		public static readonly StringName OnThicknessChanged = "OnThicknessChanged";

		public static readonly StringName OnDrawCenterToggled = "OnDrawCenterToggled";

		public static readonly StringName OnVerticalToggled = "OnVerticalToggled";

		public static readonly StringName CommitStyleBoxChange = "CommitStyleBoxChange";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _styleBoxPreview = "_styleBoxPreview";

		public static readonly StringName _flatControls = "_flatControls";

		public static readonly StringName _textureControls = "_textureControls";

		public static readonly StringName _lineControls = "_lineControls";

		public static readonly StringName _bgColorPicker = "_bgColorPicker";

		public static readonly StringName _borderColorPicker = "_borderColorPicker";

		public static readonly StringName _lineColorPicker = "_lineColorPicker";

		public static readonly StringName _modulateColorPicker = "_modulateColorPicker";

		public static readonly StringName _texturePicker = "_texturePicker";

		public static readonly StringName _borderWidthSpinBox = "_borderWidthSpinBox";

		public static readonly StringName _cornerRadiusSpinBox = "_cornerRadiusSpinBox";

		public static readonly StringName _textureMarginSpinBox = "_textureMarginSpinBox";

		public static readonly StringName _thicknessSpinBox = "_thicknessSpinBox";

		public static readonly StringName _drawCenterCheckBox = "_drawCenterCheckBox";

		public static readonly StringName _verticalCheckBox = "_verticalCheckBox";

		public static readonly StringName _styleBox = "_styleBox";

		public static readonly StringName _updating = "_updating";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private Control _styleBoxPreview;

	private VBoxContainer _flatControls;

	private VBoxContainer _textureControls;

	private VBoxContainer _lineControls;

	private ColorPickerButton _bgColorPicker;

	private ColorPickerButton _borderColorPicker;

	private ColorPickerButton _lineColorPicker;

	private ColorPickerButton _modulateColorPicker;

	private XWResourcePicker _texturePicker;

	private SpinBox _borderWidthSpinBox;

	private SpinBox _cornerRadiusSpinBox;

	private SpinBox _textureMarginSpinBox;

	private SpinBox _thicknessSpinBox;

	private CheckBox _drawCenterCheckBox;

	private CheckBox _verticalCheckBox;

	private StyleBox _styleBox;

	private bool _updating;

	public override void _Ready()
	{
		base._Ready();
		_styleBoxPreview = GetNode<Control>("%StyleBoxPreview");
		_flatControls = GetNode<VBoxContainer>("%FlatControls");
		_textureControls = GetNode<VBoxContainer>("%TextureControls");
		_lineControls = GetNode<VBoxContainer>("%LineControls");
		_bgColorPicker = GetNode<ColorPickerButton>("%BgColorPicker");
		_borderColorPicker = GetNode<ColorPickerButton>("%BorderColorPicker");
		_lineColorPicker = GetNode<ColorPickerButton>("%LineColorPicker");
		_modulateColorPicker = GetNode<ColorPickerButton>("%ModulateColorPicker");
		_texturePicker = GetNode<XWResourcePicker>("%TexturePicker");
		_borderWidthSpinBox = GetNode<SpinBox>("%BorderWidthSpinBox");
		_cornerRadiusSpinBox = GetNode<SpinBox>("%CornerRadiusSpinBox");
		_textureMarginSpinBox = GetNode<SpinBox>("%TextureMarginSpinBox");
		_thicknessSpinBox = GetNode<SpinBox>("%ThicknessSpinBox");
		_drawCenterCheckBox = GetNode<CheckBox>("%DrawCenterCheckBox");
		_verticalCheckBox = GetNode<CheckBox>("%VerticalCheckBox");
		_texturePicker.Setup("Texture2D");
		_styleBoxPreview.Draw += OnDrawStyleBoxPreview;
		_bgColorPicker.ColorChanged += OnBgColorChanged;
		_borderColorPicker.ColorChanged += OnBorderColorChanged;
		_lineColorPicker.ColorChanged += OnLineColorChanged;
		_modulateColorPicker.ColorChanged += OnModulateColorChanged;
		_texturePicker.ResourceChanged += OnTextureChanged;
		_borderWidthSpinBox.ValueChanged += OnBorderWidthChanged;
		_cornerRadiusSpinBox.ValueChanged += OnCornerRadiusChanged;
		_textureMarginSpinBox.ValueChanged += OnTextureMarginChanged;
		_thicknessSpinBox.ValueChanged += OnThicknessChanged;
		_drawCenterCheckBox.Toggled += OnDrawCenterToggled;
		_verticalCheckBox.Toggled += OnVerticalToggled;
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		base.SetEditProperty(property, field);
		RefreshStyleBox();
	}

	public override void UpdateValue()
	{
		RefreshStyleBox();
	}

	public override Variant GetValue()
	{
		return _styleBox;
	}

	private void RefreshStyleBox()
	{
		_styleBox = ReadStyleBox();
		_updating = true;
		bool visible = _styleBox is StyleBoxFlat;
		bool visible2 = _styleBox is StyleBoxTexture;
		bool visible3 = _styleBox is StyleBoxLine;
		_flatControls.Visible = visible;
		_textureControls.Visible = visible2;
		_lineControls.Visible = visible3;
		if (_styleBox is StyleBoxFlat styleBoxFlat)
		{
			_bgColorPicker.Color = styleBoxFlat.BgColor;
			_borderColorPicker.Color = styleBoxFlat.BorderColor;
			_borderWidthSpinBox.Value = styleBoxFlat.BorderWidthLeft;
			_cornerRadiusSpinBox.Value = styleBoxFlat.CornerRadiusTopLeft;
			_drawCenterCheckBox.ButtonPressed = styleBoxFlat.DrawCenter;
		}
		else if (_styleBox is StyleBoxTexture styleBoxTexture)
		{
			_texturePicker.SetEditedResource(styleBoxTexture.Texture);
			_textureMarginSpinBox.Value = styleBoxTexture.TextureMarginLeft;
			_modulateColorPicker.Color = styleBoxTexture.ModulateColor;
			_drawCenterCheckBox.ButtonPressed = styleBoxTexture.DrawCenter;
		}
		else if (_styleBox is StyleBoxLine styleBoxLine)
		{
			_lineColorPicker.Color = styleBoxLine.Color;
			_thicknessSpinBox.Value = styleBoxLine.Thickness;
			_verticalCheckBox.ButtonPressed = styleBoxLine.Vertical;
		}
		_updating = false;
		_styleBoxPreview.QueueRedraw();
	}

	private StyleBox ReadStyleBox()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Object || !(propertyValue.As<GodotObject>() is StyleBox result))
		{
			return null;
		}
		return result;
	}

	private void OnDrawStyleBoxPreview()
	{
		Rect2 rect = new Rect2(Vector2.Zero, _styleBoxPreview.Size);
		StyleBoxFlat styleBoxFlat = new StyleBoxFlat
		{
			BgColor = new Color(0.08f, 0.08f, 0.08f)
		};
		styleBoxFlat.SetCornerRadiusAll(3);
		_styleBoxPreview.DrawStyleBox(styleBoxFlat, rect);
		if (GodotObject.IsInstanceValid(_styleBox))
		{
			_styleBoxPreview.DrawStyleBox(_styleBox, rect.Grow(-10f));
		}
	}

	private void OnBgColorChanged(Color color)
	{
		if (!_updating && _styleBox is StyleBoxFlat styleBoxFlat)
		{
			styleBoxFlat.BgColor = color;
			CommitStyleBoxChange();
		}
	}

	private void OnBorderColorChanged(Color color)
	{
		if (!_updating && _styleBox is StyleBoxFlat styleBoxFlat)
		{
			styleBoxFlat.BorderColor = color;
			CommitStyleBoxChange();
		}
	}

	private void OnLineColorChanged(Color color)
	{
		if (!_updating && _styleBox is StyleBoxLine styleBoxLine)
		{
			styleBoxLine.Color = color;
			CommitStyleBoxChange();
		}
	}

	private void OnModulateColorChanged(Color color)
	{
		if (!_updating && _styleBox is StyleBoxTexture styleBoxTexture)
		{
			styleBoxTexture.ModulateColor = color;
			CommitStyleBoxChange();
		}
	}

	private void OnTextureChanged(Resource resource)
	{
		if (!_updating && _styleBox is StyleBoxTexture styleBoxTexture)
		{
			styleBoxTexture.Texture = resource as Texture2D;
			CommitStyleBoxChange();
		}
	}

	private void OnBorderWidthChanged(double value)
	{
		if (!_updating && _styleBox is StyleBoxFlat styleBoxFlat)
		{
			styleBoxFlat.SetBorderWidthAll(Mathf.Max(0, (int)value));
			CommitStyleBoxChange();
		}
	}

	private void OnCornerRadiusChanged(double value)
	{
		if (!_updating && _styleBox is StyleBoxFlat styleBoxFlat)
		{
			styleBoxFlat.SetCornerRadiusAll(Mathf.Max(0, (int)value));
			CommitStyleBoxChange();
		}
	}

	private void OnTextureMarginChanged(double value)
	{
		if (!_updating && _styleBox is StyleBoxTexture styleBoxTexture)
		{
			float textureMarginBottom = (styleBoxTexture.TextureMarginRight = (styleBoxTexture.TextureMarginTop = (styleBoxTexture.TextureMarginLeft = Mathf.Max(0f, (float)value))));
			styleBoxTexture.TextureMarginBottom = textureMarginBottom;
			CommitStyleBoxChange();
		}
	}

	private void OnThicknessChanged(double value)
	{
		if (!_updating && _styleBox is StyleBoxLine styleBoxLine)
		{
			styleBoxLine.Thickness = Mathf.Max(0, (int)value);
			CommitStyleBoxChange();
		}
	}

	private void OnDrawCenterToggled(bool toggled)
	{
		if (_updating)
		{
			return;
		}
		if (_styleBox is StyleBoxFlat styleBoxFlat)
		{
			styleBoxFlat.DrawCenter = toggled;
		}
		else
		{
			if (!(_styleBox is StyleBoxTexture styleBoxTexture))
			{
				return;
			}
			styleBoxTexture.DrawCenter = toggled;
		}
		CommitStyleBoxChange();
	}

	private void OnVerticalToggled(bool toggled)
	{
		if (!_updating && _styleBox is StyleBoxLine styleBoxLine)
		{
			styleBoxLine.Vertical = toggled;
			CommitStyleBoxChange();
		}
	}

	private void CommitStyleBoxChange()
	{
		if (GodotObject.IsInstanceValid(_styleBox))
		{
			_styleBox.EmitChanged();
			if (GodotObject.IsInstanceValid(Property) && Property.PropName != (StringName)"")
			{
				ValueChange(_styleBox);
			}
			else
			{
				Property?.SetCall();
			}
			_styleBoxPreview.QueueRedraw();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(19)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshStyleBox, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadStyleBox, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBox"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDrawStyleBoxPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnBgColorChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnBorderColorChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnLineColorChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnModulateColorChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTextureChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnBorderWidthChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCornerRadiusChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTextureMarginChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnThicknessChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDrawCenterToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnVerticalToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitStyleBoxChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.RefreshStyleBox && args.Count == 0)
		{
			RefreshStyleBox();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadStyleBox && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StyleBox>(ReadStyleBox());
			return true;
		}
		if (method == MethodName.OnDrawStyleBoxPreview && args.Count == 0)
		{
			OnDrawStyleBoxPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.OnBgColorChanged && args.Count == 1)
		{
			OnBgColorChanged(VariantUtils.ConvertTo<Color>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnBorderColorChanged && args.Count == 1)
		{
			OnBorderColorChanged(VariantUtils.ConvertTo<Color>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnLineColorChanged && args.Count == 1)
		{
			OnLineColorChanged(VariantUtils.ConvertTo<Color>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnModulateColorChanged && args.Count == 1)
		{
			OnModulateColorChanged(VariantUtils.ConvertTo<Color>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTextureChanged && args.Count == 1)
		{
			OnTextureChanged(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnBorderWidthChanged && args.Count == 1)
		{
			OnBorderWidthChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCornerRadiusChanged && args.Count == 1)
		{
			OnCornerRadiusChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTextureMarginChanged && args.Count == 1)
		{
			OnTextureMarginChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnThicknessChanged && args.Count == 1)
		{
			OnThicknessChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDrawCenterToggled && args.Count == 1)
		{
			OnDrawCenterToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnVerticalToggled && args.Count == 1)
		{
			OnVerticalToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitStyleBoxChange && args.Count == 0)
		{
			CommitStyleBoxChange();
			ret = default;
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
		if (method == MethodName.RefreshStyleBox)
		{
			return true;
		}
		if (method == MethodName.ReadStyleBox)
		{
			return true;
		}
		if (method == MethodName.OnDrawStyleBoxPreview)
		{
			return true;
		}
		if (method == MethodName.OnBgColorChanged)
		{
			return true;
		}
		if (method == MethodName.OnBorderColorChanged)
		{
			return true;
		}
		if (method == MethodName.OnLineColorChanged)
		{
			return true;
		}
		if (method == MethodName.OnModulateColorChanged)
		{
			return true;
		}
		if (method == MethodName.OnTextureChanged)
		{
			return true;
		}
		if (method == MethodName.OnBorderWidthChanged)
		{
			return true;
		}
		if (method == MethodName.OnCornerRadiusChanged)
		{
			return true;
		}
		if (method == MethodName.OnTextureMarginChanged)
		{
			return true;
		}
		if (method == MethodName.OnThicknessChanged)
		{
			return true;
		}
		if (method == MethodName.OnDrawCenterToggled)
		{
			return true;
		}
		if (method == MethodName.OnVerticalToggled)
		{
			return true;
		}
		if (method == MethodName.CommitStyleBoxChange)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._styleBoxPreview)
		{
			_styleBoxPreview = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._flatControls)
		{
			_flatControls = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._textureControls)
		{
			_textureControls = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._lineControls)
		{
			_lineControls = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._bgColorPicker)
		{
			_bgColorPicker = VariantUtils.ConvertTo<ColorPickerButton>(in value);
			return true;
		}
		if (name == PropertyName._borderColorPicker)
		{
			_borderColorPicker = VariantUtils.ConvertTo<ColorPickerButton>(in value);
			return true;
		}
		if (name == PropertyName._lineColorPicker)
		{
			_lineColorPicker = VariantUtils.ConvertTo<ColorPickerButton>(in value);
			return true;
		}
		if (name == PropertyName._modulateColorPicker)
		{
			_modulateColorPicker = VariantUtils.ConvertTo<ColorPickerButton>(in value);
			return true;
		}
		if (name == PropertyName._texturePicker)
		{
			_texturePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._borderWidthSpinBox)
		{
			_borderWidthSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._cornerRadiusSpinBox)
		{
			_cornerRadiusSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._textureMarginSpinBox)
		{
			_textureMarginSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._thicknessSpinBox)
		{
			_thicknessSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._drawCenterCheckBox)
		{
			_drawCenterCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._verticalCheckBox)
		{
			_verticalCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._styleBox)
		{
			_styleBox = VariantUtils.ConvertTo<StyleBox>(in value);
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
		if (name == PropertyName._styleBoxPreview)
		{
			value = VariantUtils.CreateFrom(in _styleBoxPreview);
			return true;
		}
		if (name == PropertyName._flatControls)
		{
			value = VariantUtils.CreateFrom(in _flatControls);
			return true;
		}
		if (name == PropertyName._textureControls)
		{
			value = VariantUtils.CreateFrom(in _textureControls);
			return true;
		}
		if (name == PropertyName._lineControls)
		{
			value = VariantUtils.CreateFrom(in _lineControls);
			return true;
		}
		if (name == PropertyName._bgColorPicker)
		{
			value = VariantUtils.CreateFrom(in _bgColorPicker);
			return true;
		}
		if (name == PropertyName._borderColorPicker)
		{
			value = VariantUtils.CreateFrom(in _borderColorPicker);
			return true;
		}
		if (name == PropertyName._lineColorPicker)
		{
			value = VariantUtils.CreateFrom(in _lineColorPicker);
			return true;
		}
		if (name == PropertyName._modulateColorPicker)
		{
			value = VariantUtils.CreateFrom(in _modulateColorPicker);
			return true;
		}
		if (name == PropertyName._texturePicker)
		{
			value = VariantUtils.CreateFrom(in _texturePicker);
			return true;
		}
		if (name == PropertyName._borderWidthSpinBox)
		{
			value = VariantUtils.CreateFrom(in _borderWidthSpinBox);
			return true;
		}
		if (name == PropertyName._cornerRadiusSpinBox)
		{
			value = VariantUtils.CreateFrom(in _cornerRadiusSpinBox);
			return true;
		}
		if (name == PropertyName._textureMarginSpinBox)
		{
			value = VariantUtils.CreateFrom(in _textureMarginSpinBox);
			return true;
		}
		if (name == PropertyName._thicknessSpinBox)
		{
			value = VariantUtils.CreateFrom(in _thicknessSpinBox);
			return true;
		}
		if (name == PropertyName._drawCenterCheckBox)
		{
			value = VariantUtils.CreateFrom(in _drawCenterCheckBox);
			return true;
		}
		if (name == PropertyName._verticalCheckBox)
		{
			value = VariantUtils.CreateFrom(in _verticalCheckBox);
			return true;
		}
		if (name == PropertyName._styleBox)
		{
			value = VariantUtils.CreateFrom(in _styleBox);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._styleBoxPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._flatControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._textureControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lineControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bgColorPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._borderColorPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lineColorPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._modulateColorPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texturePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._borderWidthSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cornerRadiusSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._textureMarginSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._thicknessSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._drawCenterCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._verticalCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._styleBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._styleBoxPreview, Variant.From(in _styleBoxPreview));
		info.AddProperty(PropertyName._flatControls, Variant.From(in _flatControls));
		info.AddProperty(PropertyName._textureControls, Variant.From(in _textureControls));
		info.AddProperty(PropertyName._lineControls, Variant.From(in _lineControls));
		info.AddProperty(PropertyName._bgColorPicker, Variant.From(in _bgColorPicker));
		info.AddProperty(PropertyName._borderColorPicker, Variant.From(in _borderColorPicker));
		info.AddProperty(PropertyName._lineColorPicker, Variant.From(in _lineColorPicker));
		info.AddProperty(PropertyName._modulateColorPicker, Variant.From(in _modulateColorPicker));
		info.AddProperty(PropertyName._texturePicker, Variant.From(in _texturePicker));
		info.AddProperty(PropertyName._borderWidthSpinBox, Variant.From(in _borderWidthSpinBox));
		info.AddProperty(PropertyName._cornerRadiusSpinBox, Variant.From(in _cornerRadiusSpinBox));
		info.AddProperty(PropertyName._textureMarginSpinBox, Variant.From(in _textureMarginSpinBox));
		info.AddProperty(PropertyName._thicknessSpinBox, Variant.From(in _thicknessSpinBox));
		info.AddProperty(PropertyName._drawCenterCheckBox, Variant.From(in _drawCenterCheckBox));
		info.AddProperty(PropertyName._verticalCheckBox, Variant.From(in _verticalCheckBox));
		info.AddProperty(PropertyName._styleBox, Variant.From(in _styleBox));
		info.AddProperty(PropertyName._updating, Variant.From(in _updating));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._styleBoxPreview, out var value))
		{
			_styleBoxPreview = value.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._flatControls, out var value2))
		{
			_flatControls = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._textureControls, out var value3))
		{
			_textureControls = value3.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._lineControls, out var value4))
		{
			_lineControls = value4.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._bgColorPicker, out var value5))
		{
			_bgColorPicker = value5.As<ColorPickerButton>();
		}
		if (info.TryGetProperty(PropertyName._borderColorPicker, out var value6))
		{
			_borderColorPicker = value6.As<ColorPickerButton>();
		}
		if (info.TryGetProperty(PropertyName._lineColorPicker, out var value7))
		{
			_lineColorPicker = value7.As<ColorPickerButton>();
		}
		if (info.TryGetProperty(PropertyName._modulateColorPicker, out var value8))
		{
			_modulateColorPicker = value8.As<ColorPickerButton>();
		}
		if (info.TryGetProperty(PropertyName._texturePicker, out var value9))
		{
			_texturePicker = value9.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._borderWidthSpinBox, out var value10))
		{
			_borderWidthSpinBox = value10.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._cornerRadiusSpinBox, out var value11))
		{
			_cornerRadiusSpinBox = value11.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._textureMarginSpinBox, out var value12))
		{
			_textureMarginSpinBox = value12.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._thicknessSpinBox, out var value13))
		{
			_thicknessSpinBox = value13.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._drawCenterCheckBox, out var value14))
		{
			_drawCenterCheckBox = value14.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._verticalCheckBox, out var value15))
		{
			_verticalCheckBox = value15.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._styleBox, out var value16))
		{
			_styleBox = value16.As<StyleBox>();
		}
		if (info.TryGetProperty(PropertyName._updating, out var value17))
		{
			_updating = value17.As<bool>();
		}
	}
}
