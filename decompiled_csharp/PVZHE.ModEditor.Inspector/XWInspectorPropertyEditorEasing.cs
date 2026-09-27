using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/Editor/Easing/XWInspectorPropertyEditorEasing.cs")]
public class XWInspectorPropertyEditorEasing : XWInspectorPropertyEditorBase
{
	public new class MethodName : XWInspectorPropertyEditorBase.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitEasingPresets = "InitEasingPresets";

		public static readonly StringName BounceOut = "BounceOut";

		public new static readonly StringName UpdateValue = "UpdateValue";

		public new static readonly StringName GetValue = "GetValue";

		public static readonly StringName OnEasingSelected = "OnEasingSelected";

		public static readonly StringName OnPopupEasingSelected = "OnPopupEasingSelected";

		public static readonly StringName OnPreviewInput = "OnPreviewInput";

		public static readonly StringName OnDrawPreview = "OnDrawPreview";

		public static readonly StringName OnDrawPopupCurve = "OnDrawPopupCurve";
	}

	public new class PropertyName : XWInspectorPropertyEditorBase.PropertyName
	{
		public static readonly StringName _optionButton = "_optionButton";

		public static readonly StringName _preview = "_preview";

		public static readonly StringName _popup = "_popup";

		public static readonly StringName _popupCurve = "_popupCurve";

		public static readonly StringName _popupOption = "_popupOption";

		public static readonly StringName _closeButton = "_closeButton";

		public static readonly StringName _currentEasing = "_currentEasing";
	}

	public new class SignalName : XWInspectorPropertyEditorBase.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/Inspector/GUI/Editor/Easing/XWInspectorPropertyEditorEasing.tscn";

	private OptionButton _optionButton;

	private Control _preview;

	private PopupPanel _popup;

	private Control _popupCurve;

	private OptionButton _popupOption;

	private Button _closeButton;

	private readonly Dictionary<int, string> _easingNames = new Dictionary<int, string>();

	private readonly Dictionary<int, Func<double, double>> _easingFuncs = new Dictionary<int, Func<double, double>>();

	private int _currentEasing;

	public static XWInspectorPropertyEditorEasing Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/Editor/Easing/XWInspectorPropertyEditorEasing.tscn", null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspectorPropertyEditorEasing>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		base._Ready();
		_optionButton = GetNode<OptionButton>("%OptionButton");
		_preview = GetNode<Control>("%Preview");
		_popup = GetNode<PopupPanel>("%EasingPopup");
		_popupCurve = GetNode<Control>("%PopupCurve");
		_popupOption = GetNode<OptionButton>("%PopupOption");
		_closeButton = GetNode<Button>("%CloseButton");
		InitEasingPresets();
		_preview.Draw += OnDrawPreview;
		_preview.GuiInput += OnPreviewInput;
		_optionButton.ItemSelected += OnEasingSelected;
		_popupOption.ItemSelected += OnPopupEasingSelected;
		_closeButton.Pressed += () =>
		{
			_popup.Hide();
		};
		_popupCurve.Draw += OnDrawPopupCurve;
		foreach (KeyValuePair<int, string> easingName in _easingNames)
		{
			_optionButton.AddItem(easingName.Value, easingName.Key);
			_popupOption.AddItem(easingName.Value, easingName.Key);
		}
	}

	private void InitEasingPresets()
	{
		Add(0, "Linear", (double t) => t);
		Add(1, "Ease In", (double t) => t * t);
		Add(2, "Ease Out", (double t) => 1.0 - (1.0 - t) * (1.0 - t));
		Add(3, "Ease In-Out", (double t) => (!(t < 0.5)) ? (1.0 - Math.Pow(-2.0 * t + 2.0, 2.0) / 2.0) : (t * t * (3.0 - 2.0 * t)));
		Add(4, "Quad In", (double t) => t * t);
		Add(5, "Quad Out", (double t) => 1.0 - (1.0 - t) * (1.0 - t));
		Add(6, "Quad In-Out", (double t) => (!(t < 0.5)) ? (1.0 - Math.Pow(-2.0 * t + 2.0, 2.0) / 2.0) : (2.0 * t * t));
		Add(7, "Cubic In", (double t) => t * t * t);
		Add(8, "Cubic Out", (double t) => 1.0 - Math.Pow(1.0 - t, 3.0));
		Add(9, "Cubic In-Out", (double t) => (!(t < 0.5)) ? (1.0 - Math.Pow(-2.0 * t + 2.0, 3.0) / 2.0) : (4.0 * t * t * t));
		Add(10, "Quart In", (double t) => t * t * t * t);
		Add(11, "Quart Out", (double t) => 1.0 - Math.Pow(1.0 - t, 4.0));
		Add(12, "Quart In-Out", (double t) => (!(t < 0.5)) ? (1.0 - Math.Pow(-2.0 * t + 2.0, 4.0) / 2.0) : (8.0 * t * t * t * t));
		Add(13, "Quint In", (double t) => t * t * t * t * t);
		Add(14, "Quint Out", (double t) => 1.0 - Math.Pow(1.0 - t, 5.0));
		Add(15, "Quint In-Out", (double t) => (!(t < 0.5)) ? (1.0 - Math.Pow(-2.0 * t + 2.0, 5.0) / 2.0) : (16.0 * t * t * t * t * t));
		Add(16, "Expo In", (double t) => (t != 0.0) ? Math.Pow(2.0, 10.0 * t - 10.0) : 0.0);
		Add(17, "Expo Out", (double t) => (t != 1.0) ? (1.0 - Math.Pow(2.0, -10.0 * t)) : 1.0);
		Add(18, "Expo In-Out", (double t) =>
		{
			double result = ((t != 0.0) ? ((t != 1.0) ? ((!(t < 0.5)) ? ((2.0 - Math.Pow(2.0, -20.0 * t + 10.0)) / 2.0) : (Math.Pow(2.0, 20.0 * t - 10.0) / 2.0)) : 1.0) : 0.0);
			return result;
		});
		Add(19, "Circ In", (double t) => 1.0 - Math.Sqrt(1.0 - t * t));
		Add(20, "Circ Out", (double t) => Math.Sqrt(1.0 - (t - 1.0) * (t - 1.0)));
		Add(21, "Circ In-Out", (double t) => (!(t < 0.5)) ? ((Math.Sqrt(1.0 - (-2.0 * t + 2.0) * (-2.0 * t + 2.0)) + 1.0) / 2.0) : ((1.0 - Math.Sqrt(1.0 - 2.0 * t * (2.0 * t))) / 2.0));
		Add(22, "Back In", (double t) =>
		{
			double num = 1.70158;
			return (num + 1.0) * t * t * t - num * t * t;
		});
		Add(23, "Back Out", (double t) =>
		{
			double num = 1.70158;
			double num2 = num + 1.0;
			return 1.0 + num2 * Math.Pow(t - 1.0, 3.0) + num * Math.Pow(t - 1.0, 2.0);
		});
		Add(24, "Back In-Out", (double t) =>
		{
			double num = 1.70158 * 1.525;
			return (!(t < 0.5)) ? ((Math.Pow(2.0 * t - 2.0, 2.0) * ((num + 1.0) * (t * 2.0 - 2.0) + num) + 2.0) / 2.0) : (Math.Pow(2.0 * t, 2.0) * ((num + 1.0) * 2.0 * t - num) / 2.0);
		});
		Add(25, "Elastic In", (double t) =>
		{
			double result = ((t != 0.0) ? ((t != 1.0) ? ((0.0 - Math.Pow(2.0, 10.0 * t - 10.0)) * Math.Sin((t * 10.0 - 10.75) * (Math.PI * 2.0) / 3.0)) : 1.0) : 0.0);
			return result;
		});
		Add(26, "Elastic Out", (double t) =>
		{
			double result = ((t != 0.0) ? ((t != 1.0) ? (Math.Pow(2.0, -10.0 * t) * Math.Sin((t * 10.0 - 0.75) * (Math.PI * 2.0) / 3.0) + 1.0) : 1.0) : 0.0);
			return result;
		});
		Add(27, "Elastic In-Out", (double t) =>
		{
			double num = Math.PI * 4.0 / 9.0;
			double result = ((t != 0.0) ? ((t != 1.0) ? ((!(t < 0.5)) ? (Math.Pow(2.0, -20.0 * t + 10.0) * Math.Sin((20.0 * t - 11.125) * num) / 2.0 + 1.0) : ((0.0 - Math.Pow(2.0, 20.0 * t - 10.0) * Math.Sin((20.0 * t - 11.125) * num)) / 2.0)) : 1.0) : 0.0);
			return result;
		});
		Add(28, "Bounce In", (double t) => 1.0 - BounceOut(1.0 - t));
		Add(29, "Bounce Out", BounceOut);
		Add(30, "Bounce In-Out", (double t) => (!(t < 0.5)) ? ((1.0 + BounceOut(2.0 * t - 1.0)) / 2.0) : ((1.0 - BounceOut(1.0 - 2.0 * t)) / 2.0));
		void Add(int id, string name, Func<double, double> fn)
		{
			_easingNames[id] = name;
			_easingFuncs[id] = fn;
		}
	}

	private static double BounceOut(double t)
	{
		double num = 7.5625;
		double num2 = 2.75;
		if (t < 1.0 / num2)
		{
			return num * t * t;
		}
		if (t < 2.0 / num2)
		{
			t -= 1.5 / num2;
			return num * t * t + 0.75;
		}
		if (t < 2.5 / num2)
		{
			t -= 2.25 / num2;
			return num * t * t + 0.9375;
		}
		t -= 2.625 / num2;
		return num * t * t + 63.0 / 64.0;
	}

	public override void UpdateValue()
	{
		Variant propertyValue = GetPropertyValue();
		if (propertyValue.VariantType != Variant.Type.Nil)
		{
			if (propertyValue.VariantType == Variant.Type.Float)
			{
				_currentEasing = (int)propertyValue.AsDouble();
			}
			else if (propertyValue.VariantType == Variant.Type.Vector2)
			{
				_currentEasing = (int)propertyValue.AsVector2().X;
			}
			_currentEasing = Mathf.Clamp(_currentEasing, 0, _easingNames.Count - 1);
			_optionButton.Select(_currentEasing);
			if (GodotObject.IsInstanceValid(_popupOption))
			{
				_popupOption.Select(_currentEasing);
			}
			_preview.QueueRedraw();
			if (GodotObject.IsInstanceValid(_popupCurve))
			{
				_popupCurve.QueueRedraw();
			}
		}
	}

	public override Variant GetValue()
	{
		return _currentEasing;
	}

	private void OnEasingSelected(long index)
	{
		_currentEasing = _optionButton.GetItemId((int)index);
		ValueChange((double)_currentEasing);
		_preview.QueueRedraw();
	}

	private void OnPopupEasingSelected(long index)
	{
		_currentEasing = _popupOption.GetItemId((int)index);
		_optionButton.Select(_currentEasing);
		ValueChange((double)_currentEasing);
		_popupCurve.QueueRedraw();
		_preview.QueueRedraw();
	}

	private void OnPreviewInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton { Pressed: not false } inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Left)
		{
			_popupOption.Select(_currentEasing);
			_popup.Position = new Vector2I((int)GetGlobalMousePosition().X, (int)GetGlobalMousePosition().Y);
			_popup.Popup();
			_popupCurve.QueueRedraw();
		}
	}

	private void OnDrawPreview()
	{
		Rect2 rect = new Rect2(Vector2.Zero, _preview.Size);
		StyleBoxFlat styleBoxFlat = new StyleBoxFlat
		{
			BgColor = new Color(0.1f, 0.1f, 0.1f, 0.8f)
		};
		styleBoxFlat.SetCornerRadiusAll(2);
		_preview.DrawStyleBox(styleBoxFlat, rect);
		if (_easingFuncs.TryGetValue(_currentEasing, out var value))
		{
			List<Vector2> list = new List<Vector2>();
			float num = 4f;
			float num2 = rect.Size.X - num * 2f;
			float num3 = rect.Size.Y - num * 2f;
			for (int i = 0; i < 32; i++)
			{
				float num4 = (float)i / 31f;
				double num5 = value(num4);
				list.Add(new Vector2(num + num4 * num2, num + (float)(1.0 - num5) * num3));
			}
			if (list.Count > 1)
			{
				Vector2[] array = new Vector2[list.Count];
				list.CopyTo(array);
				_preview.DrawPolyline(array, new Color(0.4f, 0.7f, 1f), 1.5f, antialiased: true);
			}
		}
	}

	private void OnDrawPopupCurve()
	{
		Rect2 rect = new Rect2(Vector2.Zero, _popupCurve.Size);
		StyleBoxFlat styleBoxFlat = new StyleBoxFlat
		{
			BgColor = new Color(0.12f, 0.12f, 0.12f)
		};
		styleBoxFlat.SetCornerRadiusAll(3);
		_popupCurve.DrawStyleBox(styleBoxFlat, rect);
		_popupCurve.DrawLine(new Vector2(4f, rect.Size.Y / 2f), new Vector2(rect.Size.X - 4f, rect.Size.Y / 2f), new Color(0.3f, 0.3f, 0.3f, 0.5f), 1f);
		_popupCurve.DrawLine(new Vector2(rect.Size.X / 2f, 4f), new Vector2(rect.Size.X / 2f, rect.Size.Y - 4f), new Color(0.3f, 0.3f, 0.3f, 0.5f), 1f);
		if (_easingFuncs.TryGetValue(_currentEasing, out var value))
		{
			List<Vector2> list = new List<Vector2>();
			float num = 8f;
			float num2 = rect.Size.X - num * 2f;
			float num3 = rect.Size.Y - num * 2f;
			for (int i = 0; i < 64; i++)
			{
				float num4 = (float)i / 63f;
				double num5 = value(num4);
				list.Add(new Vector2(num + num4 * num2, num + (float)(1.0 - num5) * num3));
			}
			if (list.Count > 1)
			{
				Vector2[] array = new Vector2[list.Count];
				list.CopyTo(array);
				_popupCurve.DrawPolyline(array, new Color(0.4f, 0.7f, 1f), 2f, antialiased: true);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitEasingPresets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BounceOut, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "t", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnEasingSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPopupEasingSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPreviewInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnDrawPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDrawPopupCurve, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorEasing>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.InitEasingPresets && args.Count == 0)
		{
			InitEasingPresets();
			ret = default;
			return true;
		}
		if (method == MethodName.BounceOut && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(BounceOut(VariantUtils.ConvertTo<double>(in args[0])));
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
		if (method == MethodName.OnEasingSelected && args.Count == 1)
		{
			OnEasingSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPopupEasingSelected && args.Count == 1)
		{
			OnPopupEasingSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPreviewInput && args.Count == 1)
		{
			OnPreviewInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDrawPreview && args.Count == 0)
		{
			OnDrawPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDrawPopupCurve && args.Count == 0)
		{
			OnDrawPopupCurve();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorEasing>(Create());
			return true;
		}
		if (method == MethodName.BounceOut && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(BounceOut(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.InitEasingPresets)
		{
			return true;
		}
		if (method == MethodName.BounceOut)
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
		if (method == MethodName.OnEasingSelected)
		{
			return true;
		}
		if (method == MethodName.OnPopupEasingSelected)
		{
			return true;
		}
		if (method == MethodName.OnPreviewInput)
		{
			return true;
		}
		if (method == MethodName.OnDrawPreview)
		{
			return true;
		}
		if (method == MethodName.OnDrawPopupCurve)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._optionButton)
		{
			_optionButton = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._preview)
		{
			_preview = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._popup)
		{
			_popup = VariantUtils.ConvertTo<PopupPanel>(in value);
			return true;
		}
		if (name == PropertyName._popupCurve)
		{
			_popupCurve = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._popupOption)
		{
			_popupOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._closeButton)
		{
			_closeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._currentEasing)
		{
			_currentEasing = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._optionButton)
		{
			value = VariantUtils.CreateFrom(in _optionButton);
			return true;
		}
		if (name == PropertyName._preview)
		{
			value = VariantUtils.CreateFrom(in _preview);
			return true;
		}
		if (name == PropertyName._popup)
		{
			value = VariantUtils.CreateFrom(in _popup);
			return true;
		}
		if (name == PropertyName._popupCurve)
		{
			value = VariantUtils.CreateFrom(in _popupCurve);
			return true;
		}
		if (name == PropertyName._popupOption)
		{
			value = VariantUtils.CreateFrom(in _popupOption);
			return true;
		}
		if (name == PropertyName._closeButton)
		{
			value = VariantUtils.CreateFrom(in _closeButton);
			return true;
		}
		if (name == PropertyName._currentEasing)
		{
			value = VariantUtils.CreateFrom(in _currentEasing);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._optionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._preview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._popup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._popupCurve, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._popupOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._closeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentEasing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._optionButton, Variant.From(in _optionButton));
		info.AddProperty(PropertyName._preview, Variant.From(in _preview));
		info.AddProperty(PropertyName._popup, Variant.From(in _popup));
		info.AddProperty(PropertyName._popupCurve, Variant.From(in _popupCurve));
		info.AddProperty(PropertyName._popupOption, Variant.From(in _popupOption));
		info.AddProperty(PropertyName._closeButton, Variant.From(in _closeButton));
		info.AddProperty(PropertyName._currentEasing, Variant.From(in _currentEasing));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._optionButton, out var value))
		{
			_optionButton = value.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._preview, out var value2))
		{
			_preview = value2.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._popup, out var value3))
		{
			_popup = value3.As<PopupPanel>();
		}
		if (info.TryGetProperty(PropertyName._popupCurve, out var value4))
		{
			_popupCurve = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._popupOption, out var value5))
		{
			_popupOption = value5.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._closeButton, out var value6))
		{
			_closeButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._currentEasing, out var value7))
		{
			_currentEasing = value7.As<int>();
		}
	}
}
