using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/ResourceInspectorExtend/Gradient/XWInspectorPropertyEditorGradient.cs")]
public class XWInspectorPropertyEditorGradient : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName SetupOptions = "SetupOptions";

		public static readonly StringName RefreshGradient = "RefreshGradient";

		public static readonly StringName ReadGradient = "ReadGradient";

		public static readonly StringName UpdatePreviewTexture = "UpdatePreviewTexture";

		public static readonly StringName RebuildGradientStops = "RebuildGradientStops";

		public static readonly StringName ClearStopRows = "ClearStopRows";

		public static readonly StringName RefreshSelectedControls = "RefreshSelectedControls";

		public static readonly StringName SelectPoint = "SelectPoint";

		public static readonly StringName AddPoint = "AddPoint";

		public static readonly StringName RemovePoint = "RemovePoint";

		public static readonly StringName OnGradientPreviewGuiInput = "OnGradientPreviewGuiInput";

		public static readonly StringName OnInterpolationModeSelected = "OnInterpolationModeSelected";

		public static readonly StringName OnColorSpaceSelected = "OnColorSpaceSelected";

		public static readonly StringName OnOffsetChanged = "OnOffsetChanged";

		public static readonly StringName OnColorChanged = "OnColorChanged";

		public static readonly StringName FindClosestPoint = "FindClosestPoint";

		public static readonly StringName CommitGradientChange = "CommitGradientChange";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _gradientPreview = "_gradientPreview";

		public static readonly StringName _gradientStops = "_gradientStops";

		public static readonly StringName _interpolationModeOption = "_interpolationModeOption";

		public static readonly StringName _colorSpaceOption = "_colorSpaceOption";

		public static readonly StringName _offsetSpinBox = "_offsetSpinBox";

		public static readonly StringName _colorPickerButton = "_colorPickerButton";

		public static readonly StringName _addPointButton = "_addPointButton";

		public static readonly StringName _removePointButton = "_removePointButton";

		public static readonly StringName _gradient = "_gradient";

		public static readonly StringName _previewTexture = "_previewTexture";

		public static readonly StringName _selectedIndex = "_selectedIndex";

		public static readonly StringName _updating = "_updating";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private TextureRect _gradientPreview;

	private VBoxContainer _gradientStops;

	private OptionButton _interpolationModeOption;

	private OptionButton _colorSpaceOption;

	private SpinBox _offsetSpinBox;

	private ColorPickerButton _colorPickerButton;

	private Button _addPointButton;

	private Button _removePointButton;

	private Gradient _gradient;

	private GradientTexture1D _previewTexture;

	private int _selectedIndex = -1;

	private bool _updating;

	public override void _Ready()
	{
		base._Ready();
		_gradientPreview = GetNode<TextureRect>("%GradientPreview");
		_gradientStops = GetNode<VBoxContainer>("%GradientStops");
		_interpolationModeOption = GetNode<OptionButton>("%InterpolationModeOption");
		_colorSpaceOption = GetNode<OptionButton>("%ColorSpaceOption");
		_offsetSpinBox = GetNode<SpinBox>("%OffsetSpinBox");
		_colorPickerButton = GetNode<ColorPickerButton>("%ColorPickerButton");
		_addPointButton = GetNode<Button>("%AddPointButton");
		_removePointButton = GetNode<Button>("%RemovePointButton");
		SetupOptions();
		_gradientPreview.GuiInput += OnGradientPreviewGuiInput;
		_interpolationModeOption.ItemSelected += OnInterpolationModeSelected;
		_colorSpaceOption.ItemSelected += OnColorSpaceSelected;
		_offsetSpinBox.ValueChanged += OnOffsetChanged;
		_colorPickerButton.ColorChanged += OnColorChanged;
		_addPointButton.Pressed += AddPoint;
		_removePointButton.Pressed += RemovePoint;
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		base.SetEditProperty(property, field);
		RefreshGradient();
	}

	public override void UpdateValue()
	{
		RefreshGradient();
	}

	public override Variant GetValue()
	{
		return _gradient;
	}

	private void SetupOptions()
	{
		_interpolationModeOption.Clear();
		_interpolationModeOption.AddItem("线性", 0);
		_interpolationModeOption.AddItem("常量", 1);
		_interpolationModeOption.AddItem("三次", 2);
		_colorSpaceOption.Clear();
		_colorSpaceOption.AddItem("sRGB", 0);
		_colorSpaceOption.AddItem("线性 sRGB", 1);
		_colorSpaceOption.AddItem("Oklab", 2);
	}

	private void RefreshGradient()
	{
		_gradient = ReadGradient();
		if (!GodotObject.IsInstanceValid(_gradient))
		{
			ClearStopRows();
			_gradientPreview.Texture = null;
			_selectedIndex = -1;
			RefreshSelectedControls();
			return;
		}
		if (_selectedIndex < 0 || _selectedIndex >= _gradient.GetPointCount())
		{
			_selectedIndex = ((_gradient.GetPointCount() <= 0) ? (-1) : 0);
		}
		UpdatePreviewTexture();
		RebuildGradientStops();
		RefreshSelectedControls();
	}

	private Gradient ReadGradient()
	{
		if (GodotObject.IsInstanceValid(Property) && GodotObject.IsInstanceValid(Property.Object) && Property.PropName == (StringName)"" && Property.Object is Gradient result)
		{
			return result;
		}
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Object || !(propertyValue.As<GodotObject>() is Gradient result2))
		{
			return null;
		}
		return result2;
	}

	private void UpdatePreviewTexture()
	{
		if (GodotObject.IsInstanceValid(_gradient))
		{
			if (_previewTexture == null)
			{
				_previewTexture = new GradientTexture1D
				{
					Width = 256
				};
			}
			_previewTexture.Gradient = _gradient;
			_gradientPreview.Texture = _previewTexture;
		}
	}

	private void RebuildGradientStops()
	{
		ClearStopRows();
		if (!GodotObject.IsInstanceValid(_gradient))
		{
			return;
		}
		int pointCount = _gradient.GetPointCount();
		for (int i = 0; i < pointCount; i++)
		{
			int pointIndex = i;
			HBoxContainer hBoxContainer = new HBoxContainer
			{
				Name = $"GradientStop{pointIndex}",
				CustomMinimumSize = new Vector2(0f, 24f),
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};
			hBoxContainer.AddThemeConstantOverride("separation", 4);
			ColorRect node = new ColorRect
			{
				Color = _gradient.GetColor(pointIndex),
				CustomMinimumSize = new Vector2(28f, 20f),
				SizeFlagsVertical = SizeFlags.ShrinkCenter
			};
			hBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			Button button = new Button
			{
				Text = $"{pointIndex}: {_gradient.GetOffset(pointIndex):0.###}",
				ToggleMode = true,
				ButtonPressed = (pointIndex == _selectedIndex),
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				Alignment = HorizontalAlignment.Left,
				TooltipText = "选择这个颜色点"
			};
			button.Pressed += () =>
			{
				SelectPoint(pointIndex);
			};
			hBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
			_gradientStops.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void ClearStopRows()
	{
		if (!GodotObject.IsInstanceValid(_gradientStops))
		{
			return;
		}
		foreach (Node child in _gradientStops.GetChildren())
		{
			child.QueueFree();
		}
	}

	private void RefreshSelectedControls()
	{
		_updating = true;
		bool flag = GodotObject.IsInstanceValid(_gradient);
		bool flag2 = flag && _selectedIndex >= 0 && _selectedIndex < _gradient.GetPointCount();
		_interpolationModeOption.Disabled = !flag;
		_colorSpaceOption.Disabled = !flag;
		_offsetSpinBox.Editable = flag2;
		_colorPickerButton.Disabled = !flag2;
		_addPointButton.Disabled = !flag;
		_removePointButton.Disabled = !flag2 || _gradient.GetPointCount() <= 1;
		if (flag)
		{
			_interpolationModeOption.Select((int)_gradient.InterpolationMode);
			_colorSpaceOption.Select((int)_gradient.InterpolationColorSpace);
		}
		if (flag2)
		{
			_offsetSpinBox.Value = _gradient.GetOffset(_selectedIndex);
			_colorPickerButton.Color = _gradient.GetColor(_selectedIndex);
		}
		else
		{
			_offsetSpinBox.Value = 0.0;
			_colorPickerButton.Color = Colors.White;
		}
		_updating = false;
	}

	private void SelectPoint(int index)
	{
		if (GodotObject.IsInstanceValid(_gradient))
		{
			_selectedIndex = Mathf.Clamp(index, 0, Mathf.Max(_gradient.GetPointCount() - 1, 0));
			RebuildGradientStops();
			RefreshSelectedControls();
		}
	}

	private void AddPoint()
	{
		if (GodotObject.IsInstanceValid(_gradient))
		{
			float offset = 0.5f;
			if (_selectedIndex >= 0 && _selectedIndex < _gradient.GetPointCount())
			{
				offset = Mathf.Clamp(_gradient.GetOffset(_selectedIndex) + 0.05f, 0f, 1f);
			}
			Color color = _gradient.Sample(offset);
			_gradient.AddPoint(offset, color);
			_selectedIndex = FindClosestPoint(offset);
			CommitGradientChange();
		}
	}

	private void RemovePoint()
	{
		if (GodotObject.IsInstanceValid(_gradient) && _selectedIndex >= 0 && _selectedIndex < _gradient.GetPointCount() && _gradient.GetPointCount() > 1)
		{
			_gradient.RemovePoint(_selectedIndex);
			_selectedIndex = Mathf.Clamp(_selectedIndex, 0, Mathf.Max(_gradient.GetPointCount() - 1, 0));
			CommitGradientChange();
		}
	}

	private void OnGradientPreviewGuiInput(InputEvent inputEvent)
	{
		if (!GodotObject.IsInstanceValid(_gradient) || !(inputEvent is InputEventMouseButton { Pressed: not false } inputEventMouseButton))
		{
			return;
		}
		float offset = Mathf.Clamp(inputEventMouseButton.Position.X / Mathf.Max(_gradientPreview.Size.X, 1f), 0f, 1f);
		if (inputEventMouseButton.ButtonIndex == MouseButton.Left)
		{
			_gradient.AddPoint(offset, _gradient.Sample(offset));
			_selectedIndex = FindClosestPoint(offset);
			CommitGradientChange();
		}
		else if (inputEventMouseButton.ButtonIndex == MouseButton.Right)
		{
			int num = FindClosestPoint(offset);
			if (num >= 0)
			{
				SelectPoint(num);
			}
		}
	}

	private void OnInterpolationModeSelected(long index)
	{
		if (!_updating && GodotObject.IsInstanceValid(_gradient))
		{
			int itemId = _interpolationModeOption.GetItemId((int)index);
			_gradient.InterpolationMode = (Gradient.InterpolationModeEnum)itemId;
			CommitGradientChange();
		}
	}

	private void OnColorSpaceSelected(long index)
	{
		if (!_updating && GodotObject.IsInstanceValid(_gradient))
		{
			int itemId = _colorSpaceOption.GetItemId((int)index);
			_gradient.InterpolationColorSpace = (Gradient.ColorSpace)itemId;
			CommitGradientChange();
		}
	}

	private void OnOffsetChanged(double value)
	{
		if (!_updating && GodotObject.IsInstanceValid(_gradient) && _selectedIndex >= 0 && _selectedIndex < _gradient.GetPointCount())
		{
			_gradient.SetOffset(_selectedIndex, Mathf.Clamp((float)value, 0f, 1f));
			_selectedIndex = FindClosestPoint((float)value);
			CommitGradientChange();
		}
	}

	private void OnColorChanged(Color color)
	{
		if (!_updating && GodotObject.IsInstanceValid(_gradient) && _selectedIndex >= 0 && _selectedIndex < _gradient.GetPointCount())
		{
			_gradient.SetColor(_selectedIndex, color);
			CommitGradientChange();
		}
	}

	private int FindClosestPoint(float offset)
	{
		if (!GodotObject.IsInstanceValid(_gradient))
		{
			return -1;
		}
		int result = -1;
		float num = 3.4028235E+38f;
		int pointCount = _gradient.GetPointCount();
		for (int i = 0; i < pointCount; i++)
		{
			float num2 = Mathf.Abs(_gradient.GetOffset(i) - offset);
			if (num2 <= num)
			{
				num = num2;
				result = i;
			}
		}
		return result;
	}

	private void CommitGradientChange()
	{
		if (GodotObject.IsInstanceValid(_gradient))
		{
			_gradient.EmitChanged();
			if (GodotObject.IsInstanceValid(_previewTexture))
			{
				_previewTexture.Gradient = _gradient;
				_previewTexture.EmitChanged();
			}
			if (GodotObject.IsInstanceValid(Property) && Property.PropName != (StringName)"")
			{
				ValueChange(_gradient);
			}
			else
			{
				Property?.SetCall();
			}
			RebuildGradientStops();
			RefreshSelectedControls();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
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
			new MethodInfo(MethodName.RefreshGradient, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadGradient, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Gradient"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePreviewTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildGradientStops, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearStopRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshSelectedControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectPoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddPoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemovePoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGradientPreviewGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnInterpolationModeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnColorSpaceSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnOffsetChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnColorChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindClosestPoint, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitGradientChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.RefreshGradient && args.Count == 0)
		{
			RefreshGradient();
			ret = default;
			return true;
		}
		if (method == MethodName.ReadGradient && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Gradient>(ReadGradient());
			return true;
		}
		if (method == MethodName.UpdatePreviewTexture && args.Count == 0)
		{
			UpdatePreviewTexture();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildGradientStops && args.Count == 0)
		{
			RebuildGradientStops();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearStopRows && args.Count == 0)
		{
			ClearStopRows();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSelectedControls && args.Count == 0)
		{
			RefreshSelectedControls();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectPoint && args.Count == 1)
		{
			SelectPoint(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddPoint && args.Count == 0)
		{
			AddPoint();
			ret = default;
			return true;
		}
		if (method == MethodName.RemovePoint && args.Count == 0)
		{
			RemovePoint();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGradientPreviewGuiInput && args.Count == 1)
		{
			OnGradientPreviewGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnInterpolationModeSelected && args.Count == 1)
		{
			OnInterpolationModeSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnColorSpaceSelected && args.Count == 1)
		{
			OnColorSpaceSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnOffsetChanged && args.Count == 1)
		{
			OnOffsetChanged(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnColorChanged && args.Count == 1)
		{
			OnColorChanged(VariantUtils.ConvertTo<Color>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindClosestPoint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindClosestPoint(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.CommitGradientChange && args.Count == 0)
		{
			CommitGradientChange();
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
		if (method == MethodName.SetupOptions)
		{
			return true;
		}
		if (method == MethodName.RefreshGradient)
		{
			return true;
		}
		if (method == MethodName.ReadGradient)
		{
			return true;
		}
		if (method == MethodName.UpdatePreviewTexture)
		{
			return true;
		}
		if (method == MethodName.RebuildGradientStops)
		{
			return true;
		}
		if (method == MethodName.ClearStopRows)
		{
			return true;
		}
		if (method == MethodName.RefreshSelectedControls)
		{
			return true;
		}
		if (method == MethodName.SelectPoint)
		{
			return true;
		}
		if (method == MethodName.AddPoint)
		{
			return true;
		}
		if (method == MethodName.RemovePoint)
		{
			return true;
		}
		if (method == MethodName.OnGradientPreviewGuiInput)
		{
			return true;
		}
		if (method == MethodName.OnInterpolationModeSelected)
		{
			return true;
		}
		if (method == MethodName.OnColorSpaceSelected)
		{
			return true;
		}
		if (method == MethodName.OnOffsetChanged)
		{
			return true;
		}
		if (method == MethodName.OnColorChanged)
		{
			return true;
		}
		if (method == MethodName.FindClosestPoint)
		{
			return true;
		}
		if (method == MethodName.CommitGradientChange)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._gradientPreview)
		{
			_gradientPreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._gradientStops)
		{
			_gradientStops = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._interpolationModeOption)
		{
			_interpolationModeOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._colorSpaceOption)
		{
			_colorSpaceOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._offsetSpinBox)
		{
			_offsetSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._colorPickerButton)
		{
			_colorPickerButton = VariantUtils.ConvertTo<ColorPickerButton>(in value);
			return true;
		}
		if (name == PropertyName._addPointButton)
		{
			_addPointButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._removePointButton)
		{
			_removePointButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._gradient)
		{
			_gradient = VariantUtils.ConvertTo<Gradient>(in value);
			return true;
		}
		if (name == PropertyName._previewTexture)
		{
			_previewTexture = VariantUtils.ConvertTo<GradientTexture1D>(in value);
			return true;
		}
		if (name == PropertyName._selectedIndex)
		{
			_selectedIndex = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName._gradientPreview)
		{
			value = VariantUtils.CreateFrom(in _gradientPreview);
			return true;
		}
		if (name == PropertyName._gradientStops)
		{
			value = VariantUtils.CreateFrom(in _gradientStops);
			return true;
		}
		if (name == PropertyName._interpolationModeOption)
		{
			value = VariantUtils.CreateFrom(in _interpolationModeOption);
			return true;
		}
		if (name == PropertyName._colorSpaceOption)
		{
			value = VariantUtils.CreateFrom(in _colorSpaceOption);
			return true;
		}
		if (name == PropertyName._offsetSpinBox)
		{
			value = VariantUtils.CreateFrom(in _offsetSpinBox);
			return true;
		}
		if (name == PropertyName._colorPickerButton)
		{
			value = VariantUtils.CreateFrom(in _colorPickerButton);
			return true;
		}
		if (name == PropertyName._addPointButton)
		{
			value = VariantUtils.CreateFrom(in _addPointButton);
			return true;
		}
		if (name == PropertyName._removePointButton)
		{
			value = VariantUtils.CreateFrom(in _removePointButton);
			return true;
		}
		if (name == PropertyName._gradient)
		{
			value = VariantUtils.CreateFrom(in _gradient);
			return true;
		}
		if (name == PropertyName._previewTexture)
		{
			value = VariantUtils.CreateFrom(in _previewTexture);
			return true;
		}
		if (name == PropertyName._selectedIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedIndex);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._gradientPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gradientStops, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._interpolationModeOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._colorSpaceOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._offsetSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._colorPickerButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addPointButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._removePointButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gradient, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._gradientPreview, Variant.From(in _gradientPreview));
		info.AddProperty(PropertyName._gradientStops, Variant.From(in _gradientStops));
		info.AddProperty(PropertyName._interpolationModeOption, Variant.From(in _interpolationModeOption));
		info.AddProperty(PropertyName._colorSpaceOption, Variant.From(in _colorSpaceOption));
		info.AddProperty(PropertyName._offsetSpinBox, Variant.From(in _offsetSpinBox));
		info.AddProperty(PropertyName._colorPickerButton, Variant.From(in _colorPickerButton));
		info.AddProperty(PropertyName._addPointButton, Variant.From(in _addPointButton));
		info.AddProperty(PropertyName._removePointButton, Variant.From(in _removePointButton));
		info.AddProperty(PropertyName._gradient, Variant.From(in _gradient));
		info.AddProperty(PropertyName._previewTexture, Variant.From(in _previewTexture));
		info.AddProperty(PropertyName._selectedIndex, Variant.From(in _selectedIndex));
		info.AddProperty(PropertyName._updating, Variant.From(in _updating));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._gradientPreview, out var value))
		{
			_gradientPreview = value.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._gradientStops, out var value2))
		{
			_gradientStops = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._interpolationModeOption, out var value3))
		{
			_interpolationModeOption = value3.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._colorSpaceOption, out var value4))
		{
			_colorSpaceOption = value4.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._offsetSpinBox, out var value5))
		{
			_offsetSpinBox = value5.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._colorPickerButton, out var value6))
		{
			_colorPickerButton = value6.As<ColorPickerButton>();
		}
		if (info.TryGetProperty(PropertyName._addPointButton, out var value7))
		{
			_addPointButton = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._removePointButton, out var value8))
		{
			_removePointButton = value8.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._gradient, out var value9))
		{
			_gradient = value9.As<Gradient>();
		}
		if (info.TryGetProperty(PropertyName._previewTexture, out var value10))
		{
			_previewTexture = value10.As<GradientTexture1D>();
		}
		if (info.TryGetProperty(PropertyName._selectedIndex, out var value11))
		{
			_selectedIndex = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName._updating, out var value12))
		{
			_updating = value12.As<bool>();
		}
	}
}
