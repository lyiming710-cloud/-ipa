using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Color/XWInspectorPropertyEditorColor.cs")]
public class XWInspectorPropertyEditorColor : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName SetEditProperty = "SetEditProperty";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName ReadColor = "ReadColor";

		public static readonly StringName ApplyPaletteColor = "ApplyPaletteColor";

		public static readonly StringName ApplyColor = "ApplyColor";

		public static readonly StringName RefreshVisuals = "RefreshVisuals";

		public static readonly StringName IsPaletteSelected = "IsPaletteSelected";

		public static readonly StringName ApplyPaletteStyle = "ApplyPaletteStyle";

		public static readonly StringName CreatePaletteStyle = "CreatePaletteStyle";

		public static readonly StringName DrawColorPreview = "DrawColorPreview";

		public static readonly StringName DrawCheckerboard = "DrawCheckerboard";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _colorPickerButton = "_colorPickerButton";

		public static readonly StringName _colorPreview = "_colorPreview";

		public static readonly StringName _colorValueLabel = "_colorValueLabel";

		public static readonly StringName _paletteButtons = "_paletteButtons";

		public static readonly StringName _comparisonColor = "_comparisonColor";

		public static readonly StringName _currentColor = "_currentColor";

		public static readonly StringName _updating = "_updating";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private static readonly Color[] PaletteColors = new Color[7]
	{
		new Color("f6f1d5"),
		new Color("252934"),
		new Color("ffd34d"),
		new Color("65c466"),
		new Color("58b7e8"),
		new Color("ef5b5b"),
		new Color("a97be8")
	};

	private static readonly string[] PaletteNames = new string[7] { "月光白", "深夜黑", "阳光金", "植物绿", "寒冰蓝", "警告红", "能量紫" };

	private ColorPickerButton _colorPickerButton;

	private Control _colorPreview;

	private Label _colorValueLabel;

	private Button[] _paletteButtons;

	private Color _comparisonColor = Colors.Transparent;

	private Color _currentColor = Colors.Transparent;

	private bool _updating;

	public override void _Ready()
	{
		base._Ready();
		_colorPickerButton = GetNode<ColorPickerButton>("%ColorPickerButton");
		_colorPreview = GetNode<Control>("%ColorPreview");
		_colorValueLabel = GetNode<Label>("%ColorValueLabel");
		_paletteButtons = new Button[PaletteColors.Length];
		_colorPickerButton.ColorChanged += ApplyColor;
		_colorPreview.Draw += DrawColorPreview;
		for (int i = 0; i < PaletteColors.Length; i++)
		{
			int paletteIndex = i;
			Button node = GetNode<Button>($"%PaletteButton{i}");
			node.TooltipText = PaletteNames[i] + "  #" + PaletteColors[i].ToHtml(includeAlpha: false).ToUpperInvariant();
			node.Pressed += () =>
			{
				ApplyPaletteColor(paletteIndex);
			};
			_paletteButtons[i] = node;
		}
		RefreshVisuals();
	}

	public override void SetEditProperty(XWInspectorProperty property, StringName field = null)
	{
		base.SetEditProperty(property, field);
		_comparisonColor = ReadColor();
		_currentColor = _comparisonColor;
		RefreshVisuals();
	}

	public override void UpdateValue()
	{
		_currentColor = ReadColor();
		RefreshVisuals();
	}

	public override Variant GetValue()
	{
		return _currentColor;
	}

	private Color ReadColor()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Color)
		{
			return Colors.Transparent;
		}
		return propertyValue.AsColor();
	}

	private void ApplyPaletteColor(int index)
	{
		if (index >= 0 && index < PaletteColors.Length)
		{
			Color color = PaletteColors[index];
			color.A = _currentColor.A;
			ApplyColor(color);
		}
	}

	private void ApplyColor(Color color)
	{
		if (!_updating && !_currentColor.IsEqualApprox(color))
		{
			_currentColor = color;
			ValueChange(color);
			RefreshVisuals();
		}
	}

	private void RefreshVisuals()
	{
		if (IsNodeReady())
		{
			_updating = true;
			_colorPickerButton.Color = _currentColor;
			_colorValueLabel.Text = $"#{_currentColor.ToHtml().ToUpperInvariant()}  ·  A {Mathf.RoundToInt(_currentColor.A * 100f)}%";
			_colorValueLabel.TooltipText = $"R {Mathf.RoundToInt(_currentColor.R * 255f)}   G {Mathf.RoundToInt(_currentColor.G * 255f)}   B {Mathf.RoundToInt(_currentColor.B * 255f)}   A {Mathf.RoundToInt(_currentColor.A * 255f)}";
			for (int i = 0; i < _paletteButtons.Length; i++)
			{
				ApplyPaletteStyle(_paletteButtons[i], PaletteColors[i], IsPaletteSelected(i));
			}
			_updating = false;
			_colorPreview.QueueRedraw();
		}
	}

	private bool IsPaletteSelected(int index)
	{
		Color color = PaletteColors[index];
		if (Mathf.IsEqualApprox(_currentColor.R, color.R) && Mathf.IsEqualApprox(_currentColor.G, color.G))
		{
			return Mathf.IsEqualApprox(_currentColor.B, color.B);
		}
		return false;
	}

	private static void ApplyPaletteStyle(Button button, Color color, bool selected)
	{
		if (GodotObject.IsInstanceValid(button))
		{
			button.AddThemeStyleboxOverride("normal", CreatePaletteStyle(color, selected ? new Color("ffd45a") : new Color("38445a"), (!selected) ? 1 : 3));
			button.AddThemeStyleboxOverride("hover", CreatePaletteStyle(color.Lightened(0.08f), new Color("e7eefc"), 2));
			button.AddThemeStyleboxOverride("pressed", CreatePaletteStyle(color.Darkened(0.1f), new Color("ffd45a"), 3));
		}
	}

	private static StyleBoxFlat CreatePaletteStyle(Color background, Color border, int width)
	{
		StyleBoxFlat styleBoxFlat = new StyleBoxFlat();
		styleBoxFlat.BgColor = background;
		styleBoxFlat.BorderColor = border;
		styleBoxFlat.SetBorderWidthAll(width);
		styleBoxFlat.SetCornerRadiusAll(5);
		return styleBoxFlat;
	}

	private void DrawColorPreview()
	{
		if (GodotObject.IsInstanceValid(_colorPreview))
		{
			Rect2 rect = new Rect2(Vector2.Zero, _colorPreview.Size);
			DrawCheckerboard(rect);
			float num = Mathf.Floor(rect.Size.X * 0.34f);
			_colorPreview.DrawRect(new Rect2(0f, 0f, num, rect.Size.Y), _comparisonColor);
			_colorPreview.DrawRect(new Rect2(num, 0f, rect.Size.X - num, rect.Size.Y), _currentColor);
			_colorPreview.DrawLine(new Vector2(num, 0f), new Vector2(num, rect.Size.Y), new Color(1f, 1f, 1f, 0.34f), 1f);
			float width = Mathf.Max(0f, (rect.Size.X - 8f) * _currentColor.A);
			_colorPreview.DrawRect(new Rect2(4f, rect.Size.Y - 7f, rect.Size.X - 8f, 3f), new Color(0.04f, 0.05f, 0.08f, 0.82f));
			_colorPreview.DrawRect(new Rect2(4f, rect.Size.Y - 7f, width, 3f), new Color("ffd45a"));
			_colorPreview.DrawRect(rect.Grow(-0.5f), new Color("60708c"), filled: false, 1f);
		}
	}

	private void DrawCheckerboard(Rect2 rect)
	{
		Color color = new Color("252b38");
		Color color2 = new Color("465064");
		int num = Mathf.CeilToInt(rect.Size.X / 8f);
		int num2 = Mathf.CeilToInt(rect.Size.Y / 8f);
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				Color color3 = ((((j + i) & 1) == 0) ? color : color2);
				_colorPreview.DrawRect(new Rect2((float)j * 8f, (float)i * 8f, 8f, 8f), color3);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyPaletteColor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyColor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsPaletteSelected, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPaletteStyle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePaletteStyle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBoxFlat"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "background", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "border", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "width", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawColorPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawCheckerboard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ReadColor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Color>(ReadColor());
			return true;
		}
		if (method == MethodName.ApplyPaletteColor && args.Count == 1)
		{
			ApplyPaletteColor(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyColor && args.Count == 1)
		{
			ApplyColor(VariantUtils.ConvertTo<Color>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshVisuals && args.Count == 0)
		{
			RefreshVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.IsPaletteSelected && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPaletteSelected(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyPaletteStyle && args.Count == 3)
		{
			ApplyPaletteStyle(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePaletteStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreatePaletteStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.DrawColorPreview && args.Count == 0)
		{
			DrawColorPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawCheckerboard && args.Count == 1)
		{
			DrawCheckerboard(VariantUtils.ConvertTo<Rect2>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ApplyPaletteStyle && args.Count == 3)
		{
			ApplyPaletteStyle(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePaletteStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreatePaletteStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
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
		if (method == MethodName.ReadColor)
		{
			return true;
		}
		if (method == MethodName.ApplyPaletteColor)
		{
			return true;
		}
		if (method == MethodName.ApplyColor)
		{
			return true;
		}
		if (method == MethodName.RefreshVisuals)
		{
			return true;
		}
		if (method == MethodName.IsPaletteSelected)
		{
			return true;
		}
		if (method == MethodName.ApplyPaletteStyle)
		{
			return true;
		}
		if (method == MethodName.CreatePaletteStyle)
		{
			return true;
		}
		if (method == MethodName.DrawColorPreview)
		{
			return true;
		}
		if (method == MethodName.DrawCheckerboard)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._colorPickerButton)
		{
			_colorPickerButton = VariantUtils.ConvertTo<ColorPickerButton>(in value);
			return true;
		}
		if (name == PropertyName._colorPreview)
		{
			_colorPreview = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._colorValueLabel)
		{
			_colorValueLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._paletteButtons)
		{
			_paletteButtons = VariantUtils.ConvertToSystemArrayOfGodotObject<Button>(in value);
			return true;
		}
		if (name == PropertyName._comparisonColor)
		{
			_comparisonColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._currentColor)
		{
			_currentColor = VariantUtils.ConvertTo<Color>(in value);
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
		if (name == PropertyName._colorPickerButton)
		{
			value = VariantUtils.CreateFrom(in _colorPickerButton);
			return true;
		}
		if (name == PropertyName._colorPreview)
		{
			value = VariantUtils.CreateFrom(in _colorPreview);
			return true;
		}
		if (name == PropertyName._colorValueLabel)
		{
			value = VariantUtils.CreateFrom(in _colorValueLabel);
			return true;
		}
		if (name == PropertyName._paletteButtons)
		{
			GodotObject[] paletteButtons = _paletteButtons;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(paletteButtons);
			return true;
		}
		if (name == PropertyName._comparisonColor)
		{
			value = VariantUtils.CreateFrom(in _comparisonColor);
			return true;
		}
		if (name == PropertyName._currentColor)
		{
			value = VariantUtils.CreateFrom(in _currentColor);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._colorPickerButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._colorPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._colorValueLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._paletteButtons, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._comparisonColor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._currentColor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._colorPickerButton, Variant.From(in _colorPickerButton));
		info.AddProperty(PropertyName._colorPreview, Variant.From(in _colorPreview));
		info.AddProperty(PropertyName._colorValueLabel, Variant.From(in _colorValueLabel));
		StringName paletteButtons = PropertyName._paletteButtons;
		GodotObject[] paletteButtons2 = _paletteButtons;
		info.AddProperty(paletteButtons, Variant.CreateFrom(paletteButtons2));
		info.AddProperty(PropertyName._comparisonColor, Variant.From(in _comparisonColor));
		info.AddProperty(PropertyName._currentColor, Variant.From(in _currentColor));
		info.AddProperty(PropertyName._updating, Variant.From(in _updating));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._colorPickerButton, out var value))
		{
			_colorPickerButton = value.As<ColorPickerButton>();
		}
		if (info.TryGetProperty(PropertyName._colorPreview, out var value2))
		{
			_colorPreview = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._colorValueLabel, out var value3))
		{
			_colorValueLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._paletteButtons, out var value4))
		{
			_paletteButtons = value4.AsGodotObjectArray<Button>();
		}
		if (info.TryGetProperty(PropertyName._comparisonColor, out var value5))
		{
			_comparisonColor = value5.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._currentColor, out var value6))
		{
			_currentColor = value6.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._updating, out var value7))
		{
			_updating = value7.As<bool>();
		}
	}
}
