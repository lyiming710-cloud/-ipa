using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/ResourceInspectorExtend/GradientTexture/XWInspectorPropertyEditorGradientTexture.cs")]
public class XWInspectorPropertyEditorGradientTexture : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName SetupOptions = "SetupOptions";

		public static readonly StringName RefreshTexture = "RefreshTexture";

		public static readonly StringName ReadTexture = "ReadTexture";

		public static readonly StringName ReadGradient = "ReadGradient";

		public static readonly StringName OnGradientChanged = "OnGradientChanged";

		public static readonly StringName OnWidthChanged = "OnWidthChanged";

		public static readonly StringName OnHeightChanged = "OnHeightChanged";

		public static readonly StringName OnUseHdrToggled = "OnUseHdrToggled";

		public static readonly StringName OnFillSelected = "OnFillSelected";

		public static readonly StringName OnRepeatSelected = "OnRepeatSelected";

		public static readonly StringName OnFillVectorChanged = "OnFillVectorChanged";

		public static readonly StringName CommitTextureChange = "CommitTextureChange";

		public static readonly StringName GetItemIndexById = "GetItemIndexById";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _gradientTexturePreview = "_gradientTexturePreview";

		public static readonly StringName _gradientPicker = "_gradientPicker";

		public static readonly StringName _widthSpinBox = "_widthSpinBox";

		public static readonly StringName _heightSpinBox = "_heightSpinBox";

		public static readonly StringName _useHdrCheckBox = "_useHdrCheckBox";

		public static readonly StringName _fillOption = "_fillOption";

		public static readonly StringName _repeatOption = "_repeatOption";

		public static readonly StringName _fillFromXSpinBox = "_fillFromXSpinBox";

		public static readonly StringName _fillFromYSpinBox = "_fillFromYSpinBox";

		public static readonly StringName _fillToXSpinBox = "_fillToXSpinBox";

		public static readonly StringName _fillToYSpinBox = "_fillToYSpinBox";

		public static readonly StringName _texture2DControls = "_texture2DControls";

		public static readonly StringName _texture = "_texture";

		public static readonly StringName _updating = "_updating";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private TextureRect _gradientTexturePreview;

	private XWResourcePicker _gradientPicker;

	private SpinBox _widthSpinBox;

	private SpinBox _heightSpinBox;

	private CheckBox _useHdrCheckBox;

	private OptionButton _fillOption;

	private OptionButton _repeatOption;

	private SpinBox _fillFromXSpinBox;

	private SpinBox _fillFromYSpinBox;

	private SpinBox _fillToXSpinBox;

	private SpinBox _fillToYSpinBox;

	private Control _texture2DControls;

	private Texture2D _texture;

	private bool _updating;

	public override void _Ready()
	{
		base._Ready();
		_gradientTexturePreview = GetNode<TextureRect>("%GradientTexturePreview");
		_gradientPicker = GetNode<XWResourcePicker>("%GradientPicker");
		_widthSpinBox = GetNode<SpinBox>("%WidthSpinBox");
		_heightSpinBox = GetNode<SpinBox>("%HeightSpinBox");
		_useHdrCheckBox = GetNode<CheckBox>("%UseHdrCheckBox");
		_fillOption = GetNode<OptionButton>("%FillOption");
		_repeatOption = GetNode<OptionButton>("%RepeatOption");
		_fillFromXSpinBox = GetNode<SpinBox>("%FillFromXSpinBox");
		_fillFromYSpinBox = GetNode<SpinBox>("%FillFromYSpinBox");
		_fillToXSpinBox = GetNode<SpinBox>("%FillToXSpinBox");
		_fillToYSpinBox = GetNode<SpinBox>("%FillToYSpinBox");
		_texture2DControls = GetNode<Control>("%Texture2DControls");
		SetupOptions();
		_gradientPicker.Setup("Gradient");
		_gradientPicker.ResourceChanged += OnGradientChanged;
		_widthSpinBox.ValueChanged += OnWidthChanged;
		_heightSpinBox.ValueChanged += OnHeightChanged;
		_useHdrCheckBox.Toggled += OnUseHdrToggled;
		_fillOption.ItemSelected += OnFillSelected;
		_repeatOption.ItemSelected += OnRepeatSelected;
		_fillFromXSpinBox.ValueChanged += (double _) =>
		{
			OnFillVectorChanged();
		};
		_fillFromYSpinBox.ValueChanged += (double _) =>
		{
			OnFillVectorChanged();
		};
		_fillToXSpinBox.ValueChanged += (double _) =>
		{
			OnFillVectorChanged();
		};
		_fillToYSpinBox.ValueChanged += (double _) =>
		{
			OnFillVectorChanged();
		};
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
		_fillOption.Clear();
		_fillOption.AddItem("线性", 0);
		_fillOption.AddItem("径向", 1);
		_fillOption.AddItem("方形", 2);
		_fillOption.AddItem("锥形", 3);
		_repeatOption.Clear();
		_repeatOption.AddItem("不重复", 0);
		_repeatOption.AddItem("重复", 1);
		_repeatOption.AddItem("镜像", 2);
	}

	private void RefreshTexture()
	{
		_texture = ReadTexture();
		_updating = true;
		bool flag = GodotObject.IsInstanceValid(_texture);
		bool flag2 = _texture is GradientTexture1D;
		bool flag3 = _texture is GradientTexture2D;
		_gradientTexturePreview.Texture = (flag ? _texture : null);
		_gradientPicker.SetEditedResource(ReadGradient());
		_widthSpinBox.Editable = flag2 | flag3;
		_heightSpinBox.Editable = flag3;
		_useHdrCheckBox.Disabled = !(flag2 | flag3);
		_fillOption.Disabled = !flag3;
		_repeatOption.Disabled = !flag3;
		_fillFromXSpinBox.Editable = flag3;
		_fillFromYSpinBox.Editable = flag3;
		_fillToXSpinBox.Editable = flag3;
		_fillToYSpinBox.Editable = flag3;
		_texture2DControls.Visible = flag3;
		if (_texture is GradientTexture1D gradientTexture1D)
		{
			_widthSpinBox.Value = gradientTexture1D.Width;
			_heightSpinBox.Value = 1.0;
			_useHdrCheckBox.ButtonPressed = gradientTexture1D.UseHdr;
		}
		else if (_texture is GradientTexture2D gradientTexture2D)
		{
			_widthSpinBox.Value = gradientTexture2D.Width;
			_heightSpinBox.Value = gradientTexture2D.Height;
			_useHdrCheckBox.ButtonPressed = gradientTexture2D.UseHdr;
			_fillOption.Select(GetItemIndexById(_fillOption, (int)gradientTexture2D.Fill));
			_repeatOption.Select(GetItemIndexById(_repeatOption, (int)gradientTexture2D.Repeat));
			_fillFromXSpinBox.Value = gradientTexture2D.FillFrom.X;
			_fillFromYSpinBox.Value = gradientTexture2D.FillFrom.Y;
			_fillToXSpinBox.Value = gradientTexture2D.FillTo.X;
			_fillToYSpinBox.Value = gradientTexture2D.FillTo.Y;
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

	private Gradient ReadGradient()
	{
		Texture2D texture = _texture;
		if (!(texture is GradientTexture1D { Gradient: var gradient }))
		{
			if (!(texture is GradientTexture2D { Gradient: var gradient2 }))
			{
				return null;
			}
			return gradient2;
		}
		return gradient;
	}

	private void OnGradientChanged(Resource resource)
	{
		if (!_updating && GodotObject.IsInstanceValid(_texture))
		{
			if (_texture is GradientTexture1D gradientTexture1D)
			{
				gradientTexture1D.Gradient = resource as Gradient;
			}
			else if (_texture is GradientTexture2D gradientTexture2D)
			{
				gradientTexture2D.Gradient = resource as Gradient;
			}
			CommitTextureChange();
		}
	}

	private void OnWidthChanged(double value)
	{
		if (!_updating && GodotObject.IsInstanceValid(_texture))
		{
			int width = Mathf.Max(1, (int)value);
			if (_texture is GradientTexture1D gradientTexture1D)
			{
				gradientTexture1D.Width = width;
			}
			else if (_texture is GradientTexture2D gradientTexture2D)
			{
				gradientTexture2D.Width = width;
			}
			CommitTextureChange();
		}
	}

	private void OnHeightChanged(double value)
	{
		if (!_updating && _texture is GradientTexture2D gradientTexture2D)
		{
			gradientTexture2D.Height = Mathf.Max(1, (int)value);
			CommitTextureChange();
		}
	}

	private void OnUseHdrToggled(bool toggled)
	{
		if (!_updating && GodotObject.IsInstanceValid(_texture))
		{
			if (_texture is GradientTexture1D gradientTexture1D)
			{
				gradientTexture1D.UseHdr = toggled;
			}
			else if (_texture is GradientTexture2D gradientTexture2D)
			{
				gradientTexture2D.UseHdr = toggled;
			}
			CommitTextureChange();
		}
	}

	private void OnFillSelected(long index)
	{
		if (!_updating && _texture is GradientTexture2D gradientTexture2D)
		{
			gradientTexture2D.Fill = (GradientTexture2D.FillEnum)_fillOption.GetItemId((int)index);
			CommitTextureChange();
		}
	}

	private void OnRepeatSelected(long index)
	{
		if (!_updating && _texture is GradientTexture2D gradientTexture2D)
		{
			gradientTexture2D.Repeat = (GradientTexture2D.RepeatEnum)_repeatOption.GetItemId((int)index);
			CommitTextureChange();
		}
	}

	private void OnFillVectorChanged()
	{
		if (!_updating && _texture is GradientTexture2D gradientTexture2D)
		{
			gradientTexture2D.FillFrom = new Vector2((float)_fillFromXSpinBox.Value, (float)_fillFromYSpinBox.Value);
			gradientTexture2D.FillTo = new Vector2((float)_fillToXSpinBox.Value, (float)_fillToYSpinBox.Value);
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
			_gradientTexturePreview.Texture = _texture;
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
		return new List<MethodInfo>(17)
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
			new MethodInfo(MethodName.RefreshTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadGradient, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Gradient"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGradientChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnWidthChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnHeightChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnUseHdrToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnFillSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnRepeatSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnFillVectorChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.ReadGradient && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Gradient>(ReadGradient());
			return true;
		}
		if (method == MethodName.OnGradientChanged && args.Count == 1)
		{
			OnGradientChanged(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnWidthChanged && args.Count == 1)
		{
			OnWidthChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnHeightChanged && args.Count == 1)
		{
			OnHeightChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnUseHdrToggled && args.Count == 1)
		{
			OnUseHdrToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnFillSelected && args.Count == 1)
		{
			OnFillSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnRepeatSelected && args.Count == 1)
		{
			OnRepeatSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnFillVectorChanged && args.Count == 0)
		{
			OnFillVectorChanged();
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
		if (method == MethodName.RefreshTexture)
		{
			return true;
		}
		if (method == MethodName.ReadTexture)
		{
			return true;
		}
		if (method == MethodName.ReadGradient)
		{
			return true;
		}
		if (method == MethodName.OnGradientChanged)
		{
			return true;
		}
		if (method == MethodName.OnWidthChanged)
		{
			return true;
		}
		if (method == MethodName.OnHeightChanged)
		{
			return true;
		}
		if (method == MethodName.OnUseHdrToggled)
		{
			return true;
		}
		if (method == MethodName.OnFillSelected)
		{
			return true;
		}
		if (method == MethodName.OnRepeatSelected)
		{
			return true;
		}
		if (method == MethodName.OnFillVectorChanged)
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
		if (name == PropertyName._gradientTexturePreview)
		{
			_gradientTexturePreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._gradientPicker)
		{
			_gradientPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._widthSpinBox)
		{
			_widthSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._heightSpinBox)
		{
			_heightSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._useHdrCheckBox)
		{
			_useHdrCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._fillOption)
		{
			_fillOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._repeatOption)
		{
			_repeatOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._fillFromXSpinBox)
		{
			_fillFromXSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._fillFromYSpinBox)
		{
			_fillFromYSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._fillToXSpinBox)
		{
			_fillToXSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._fillToYSpinBox)
		{
			_fillToYSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._texture2DControls)
		{
			_texture2DControls = VariantUtils.ConvertTo<Control>(in value);
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
		if (name == PropertyName._gradientTexturePreview)
		{
			value = VariantUtils.CreateFrom(in _gradientTexturePreview);
			return true;
		}
		if (name == PropertyName._gradientPicker)
		{
			value = VariantUtils.CreateFrom(in _gradientPicker);
			return true;
		}
		if (name == PropertyName._widthSpinBox)
		{
			value = VariantUtils.CreateFrom(in _widthSpinBox);
			return true;
		}
		if (name == PropertyName._heightSpinBox)
		{
			value = VariantUtils.CreateFrom(in _heightSpinBox);
			return true;
		}
		if (name == PropertyName._useHdrCheckBox)
		{
			value = VariantUtils.CreateFrom(in _useHdrCheckBox);
			return true;
		}
		if (name == PropertyName._fillOption)
		{
			value = VariantUtils.CreateFrom(in _fillOption);
			return true;
		}
		if (name == PropertyName._repeatOption)
		{
			value = VariantUtils.CreateFrom(in _repeatOption);
			return true;
		}
		if (name == PropertyName._fillFromXSpinBox)
		{
			value = VariantUtils.CreateFrom(in _fillFromXSpinBox);
			return true;
		}
		if (name == PropertyName._fillFromYSpinBox)
		{
			value = VariantUtils.CreateFrom(in _fillFromYSpinBox);
			return true;
		}
		if (name == PropertyName._fillToXSpinBox)
		{
			value = VariantUtils.CreateFrom(in _fillToXSpinBox);
			return true;
		}
		if (name == PropertyName._fillToYSpinBox)
		{
			value = VariantUtils.CreateFrom(in _fillToYSpinBox);
			return true;
		}
		if (name == PropertyName._texture2DControls)
		{
			value = VariantUtils.CreateFrom(in _texture2DControls);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._gradientTexturePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gradientPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._widthSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._heightSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._useHdrCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fillOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._repeatOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fillFromXSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fillFromYSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fillToXSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fillToYSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texture2DControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._gradientTexturePreview, Variant.From(in _gradientTexturePreview));
		info.AddProperty(PropertyName._gradientPicker, Variant.From(in _gradientPicker));
		info.AddProperty(PropertyName._widthSpinBox, Variant.From(in _widthSpinBox));
		info.AddProperty(PropertyName._heightSpinBox, Variant.From(in _heightSpinBox));
		info.AddProperty(PropertyName._useHdrCheckBox, Variant.From(in _useHdrCheckBox));
		info.AddProperty(PropertyName._fillOption, Variant.From(in _fillOption));
		info.AddProperty(PropertyName._repeatOption, Variant.From(in _repeatOption));
		info.AddProperty(PropertyName._fillFromXSpinBox, Variant.From(in _fillFromXSpinBox));
		info.AddProperty(PropertyName._fillFromYSpinBox, Variant.From(in _fillFromYSpinBox));
		info.AddProperty(PropertyName._fillToXSpinBox, Variant.From(in _fillToXSpinBox));
		info.AddProperty(PropertyName._fillToYSpinBox, Variant.From(in _fillToYSpinBox));
		info.AddProperty(PropertyName._texture2DControls, Variant.From(in _texture2DControls));
		info.AddProperty(PropertyName._texture, Variant.From(in _texture));
		info.AddProperty(PropertyName._updating, Variant.From(in _updating));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._gradientTexturePreview, out var value))
		{
			_gradientTexturePreview = value.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._gradientPicker, out var value2))
		{
			_gradientPicker = value2.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._widthSpinBox, out var value3))
		{
			_widthSpinBox = value3.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._heightSpinBox, out var value4))
		{
			_heightSpinBox = value4.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._useHdrCheckBox, out var value5))
		{
			_useHdrCheckBox = value5.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._fillOption, out var value6))
		{
			_fillOption = value6.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._repeatOption, out var value7))
		{
			_repeatOption = value7.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._fillFromXSpinBox, out var value8))
		{
			_fillFromXSpinBox = value8.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._fillFromYSpinBox, out var value9))
		{
			_fillFromYSpinBox = value9.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._fillToXSpinBox, out var value10))
		{
			_fillToXSpinBox = value10.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._fillToYSpinBox, out var value11))
		{
			_fillToYSpinBox = value11.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._texture2DControls, out var value12))
		{
			_texture2DControls = value12.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._texture, out var value13))
		{
			_texture = value13.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._updating, out var value14))
		{
			_updating = value14.As<bool>();
		}
	}
}
