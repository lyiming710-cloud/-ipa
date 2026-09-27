using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/Properties/PropertiesBox.cs")]
public class PropertiesBox : VBoxContainer
{
	public delegate void ValueChangedEventHandler(StringName key, Variant newValue);

	public delegate void NumberChangedEventHandler(StringName key, double newValue);

	public delegate void StringChangedEventHandler(StringName key, string newValue);

	public delegate void BoolChangedEventHandler(StringName key, bool newValue);

	public delegate void ColorChangedEventHandler(StringName key, Color newValue);

	public delegate void Vector2ChangedEventHandler(StringName key, Vector2 newValue);

	public delegate void Vector4iChangedEventHandler(StringName key, Vector4I newValue);

	public delegate void ArrayChangedEventHandler(StringName key, Array newValue);

	public delegate void ArrayVector2ChangedEventHandler(StringName key, Array<Vector2> newValue);

	public new class MethodName : VBoxContainer.MethodName
	{
		public static readonly StringName Clear = "Clear";

		public static readonly StringName add_bool = "add_bool";

		public static readonly StringName add_int = "add_int";

		public static readonly StringName add_float = "add_float";

		public static readonly StringName add_string = "add_string";

		public static readonly StringName add_color = "add_color";

		public static readonly StringName add_vector2 = "add_vector2";

		public static readonly StringName add_vector4i = "add_vector4i";

		public static readonly StringName add_enum = "add_enum";

		public static readonly StringName add_flag = "add_flag";

		public static readonly StringName add_array = "add_array";

		public static readonly StringName add_array_vector2 = "add_array_vector2";

		public static readonly StringName add_file_select = "add_file_select";

		public static readonly StringName add_map_select = "add_map_select";

		public static readonly StringName add_room_select = "add_room_select";

		public static readonly StringName add_group = "add_group";

		public static readonly StringName end_group = "end_group";

		public static readonly StringName delete_value = "delete_value";

		public static readonly StringName get_value = "get_value";

		public static readonly StringName get_bool = "get_bool";

		public static readonly StringName get_int = "get_int";

		public static readonly StringName get_float = "get_float";

		public static readonly StringName get_string = "get_string";

		public static readonly StringName get_color = "get_color";

		public static readonly StringName get_vector2 = "get_vector2";

		public static readonly StringName get_option = "get_option";

		public static readonly StringName _get_box = "_get_box";

		public static readonly StringName set_value = "set_value";

		public static readonly StringName _add_property_editor = "_add_property_editor";

		public static readonly StringName _subscribe_value_changed = "_subscribe_value_changed";

		public static readonly StringName _on_value_changed = "_on_value_changed";

		public static readonly StringName _on_number_changed = "_on_number_changed";

		public static readonly StringName _on_string_changed = "_on_string_changed";

		public static readonly StringName _on_bool_changed = "_on_bool_changed";

		public static readonly StringName _on_color_changed = "_on_color_changed";

		public static readonly StringName _on_vector2_changed = "_on_vector2_changed";

		public static readonly StringName _on_vector4i_changed = "_on_vector4i_changed";

		public static readonly StringName _on_array_changed = "_on_array_changed";

		public static readonly StringName _on_array_vector2_changed = "_on_array_vector2_changed";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName groupIndent = "groupIndent";

		public static readonly StringName _keys = "_keys";

		public static readonly StringName _groupStack = "_groupStack";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
	}

	private static Texture2D _iconReload;

	private static PackedScene _propertiesContainerScene;

	private static PackedScene _propertiesBoolScene;

	private static PackedScene _propertiesIntScene;

	private static PackedScene _propertiesFloatScene;

	private static PackedScene _propertiesStringScene;

	private static PackedScene _propertiesMultilineStringScene;

	private static PackedScene _propertiesVector2Scene;

	private static PackedScene _propertiesColorScene;

	private static PackedScene _propertiesEnumScene;

	private static PackedScene _propertiesFlagScene;

	private static PackedScene _propertiesArrayScene;

	private static PackedScene _propertiesArrayVector2Scene;

	private static PackedScene _propertiesVector4iScene;

	public Dictionary _keys = new Dictionary();

	private Array _groupStack = new Array();

	private static Texture2D IconReload => _iconReload ?? (_iconReload = GD.Load<Texture2D>("uid://b261mqo4crf07"));

	private static PackedScene PropertiesContainerScene => _propertiesContainerScene ?? (_propertiesContainerScene = GD.Load<PackedScene>("uid://dvx0agv2rta6e"));

	private static PackedScene PropertiesBoolScene => _propertiesBoolScene ?? (_propertiesBoolScene = GD.Load<PackedScene>("uid://drg0fi23ifge4"));

	private static PackedScene PropertiesIntScene => _propertiesIntScene ?? (_propertiesIntScene = GD.Load<PackedScene>("uid://c43mrf03jpt2g"));

	private static PackedScene PropertiesFloatScene => _propertiesFloatScene ?? (_propertiesFloatScene = GD.Load<PackedScene>("uid://ce6nbs0h3lltc"));

	private static PackedScene PropertiesStringScene => _propertiesStringScene ?? (_propertiesStringScene = GD.Load<PackedScene>("uid://m0ep8vt6fdbs"));

	private static PackedScene PropertiesMultilineStringScene => _propertiesMultilineStringScene ?? (_propertiesMultilineStringScene = GD.Load<PackedScene>("uid://bff45o7x3xb2u"));

	private static PackedScene PropertiesVector2Scene => _propertiesVector2Scene ?? (_propertiesVector2Scene = GD.Load<PackedScene>("uid://cbjrnlebs22v0"));

	private static PackedScene PropertiesColorScene => _propertiesColorScene ?? (_propertiesColorScene = GD.Load<PackedScene>("uid://ca1dcepidnu8m"));

	private static PackedScene PropertiesEnumScene => _propertiesEnumScene ?? (_propertiesEnumScene = GD.Load<PackedScene>("uid://c4cxl5ec7iret"));

	private static PackedScene PropertiesFlagScene => _propertiesFlagScene ?? (_propertiesFlagScene = GD.Load<PackedScene>("uid://dwohqr20egmxg"));

	private static PackedScene PropertiesArrayScene => _propertiesArrayScene ?? (_propertiesArrayScene = GD.Load<PackedScene>("uid://b6pvi03p7a6nj"));

	private static PackedScene PropertiesArrayVector2Scene => _propertiesArrayVector2Scene ?? (_propertiesArrayVector2Scene = GD.Load<PackedScene>("uid://s2ng0og2b627"));

	private static PackedScene PropertiesVector4iScene => _propertiesVector4iScene ?? (_propertiesVector4iScene = GD.Load<PackedScene>("uid://dkwhkpg0ys0sj"));

	[Export(PropertyHint.None, "")]
	public double groupIndent { get; set; } = 8.0;

	public event ValueChangedEventHandler OnValueChanged;

	public event NumberChangedEventHandler OnNumberChanged;

	public event StringChangedEventHandler OnStringChanged;

	public event BoolChangedEventHandler OnBoolChanged;

	public event ColorChangedEventHandler OnColorChanged;

	public event Vector2ChangedEventHandler OnVector2Changed;

	public event Vector4iChangedEventHandler OnVector4iChanged;

	public event ArrayChangedEventHandler OnArrayChanged;

	public event ArrayVector2ChangedEventHandler OnArrayVector2Changed;

	public void Clear()
	{
		foreach (Node child in GetChildren())
		{
			child.QueueFree();
		}
		_keys.Clear();
		_groupStack.Clear();
	}

	public PropertiesBool add_bool(GodotObject obj, string propertyName, StringName key, bool value = false, Variant restValue = default(Variant))
	{
		if (restValue.VariantType == Variant.Type.Nil)
		{
			restValue = false;
		}
		PropertiesBool propertiesBool = PropertiesBoolScene.Instantiate<PropertiesBool>(PackedScene.GenEditState.Disabled);
		_add_property_editor(key, propertiesBool, Callable.From((bool v) =>
		{
			_on_bool_changed(v, key);
		}), restValue);
		propertiesBool.value = value;
		propertiesBool.obj = obj;
		propertiesBool.propertyName = propertyName;
		return propertiesBool;
	}

	public PropertiesInt add_int(GodotObject obj, string propertyName, StringName key, int value = 0, Variant restValue = default(Variant))
	{
		if (restValue.VariantType == Variant.Type.Nil)
		{
			restValue = 0;
		}
		PropertiesInt propertiesInt = PropertiesIntScene.Instantiate<PropertiesInt>(PackedScene.GenEditState.Disabled);
		_add_property_editor(key, propertiesInt, Callable.From((int v) =>
		{
			_on_number_changed(v, key);
		}), restValue);
		propertiesInt.value = value;
		propertiesInt.obj = obj;
		propertiesInt.propertyName = propertyName;
		return propertiesInt;
	}

	public PropertiesFloat add_float(GodotObject obj, string propertyName, StringName key, double value = 0.0, Variant restValue = default(Variant))
	{
		if (restValue.VariantType == Variant.Type.Nil)
		{
			restValue = 0.0;
		}
		PropertiesFloat propertiesFloat = PropertiesFloatScene.Instantiate<PropertiesFloat>(PackedScene.GenEditState.Disabled);
		_add_property_editor(key, propertiesFloat, Callable.From((double v) =>
		{
			_on_number_changed(v, key);
		}), restValue);
		propertiesFloat.value = value;
		propertiesFloat.obj = obj;
		propertiesFloat.propertyName = propertyName;
		return propertiesFloat;
	}

	public PropertiesBase add_string(GodotObject obj, string propertyName, StringName key, string value = "", Variant restValue = default(Variant), bool isMultiline = false)
	{
		if (restValue.VariantType == Variant.Type.Nil)
		{
			restValue = "";
		}
		if (!isMultiline)
		{
			PropertiesString propertiesString = PropertiesStringScene.Instantiate<PropertiesString>(PackedScene.GenEditState.Disabled);
			_add_property_editor(key, propertiesString, Callable.From((string v) =>
			{
				_on_string_changed(v, key);
			}), restValue);
			propertiesString.value = value;
			propertiesString.obj = obj;
			propertiesString.propertyName = propertyName;
			return propertiesString;
		}
		PropertiesMultilineString propertiesMultilineString = PropertiesMultilineStringScene.Instantiate<PropertiesMultilineString>(PackedScene.GenEditState.Disabled);
		_add_property_editor(key, propertiesMultilineString, Callable.From((string v) =>
		{
			_on_string_changed(v, key);
		}), restValue, inner: false);
		propertiesMultilineString.value = value;
		propertiesMultilineString.obj = obj;
		propertiesMultilineString.propertyName = propertyName;
		return propertiesMultilineString;
	}

	public PropertiesColor add_color(GodotObject obj, string propertyName, StringName key, Color value = default(Color), Variant restValue = default(Variant))
	{
		if (value == default(Color))
		{
			value = Colors.Black;
		}
		if (restValue.VariantType == Variant.Type.Nil)
		{
			restValue = Colors.White;
		}
		PropertiesColor propertiesColor = PropertiesColorScene.Instantiate<PropertiesColor>(PackedScene.GenEditState.Disabled);
		_add_property_editor(key, propertiesColor, Callable.From((Color v) =>
		{
			_on_color_changed(v, key);
		}), restValue);
		propertiesColor.value = value;
		propertiesColor.obj = obj;
		propertiesColor.propertyName = propertyName;
		return propertiesColor;
	}

	public PropertiesVector2 add_vector2(GodotObject obj, string propertyName, StringName key, Vector2 value = default(Vector2), Variant restValue = default(Variant))
	{
		if (value == default(Vector2))
		{
			value = Vector2.Zero;
		}
		if (restValue.VariantType == Variant.Type.Nil)
		{
			restValue = Vector2.Zero;
		}
		PropertiesVector2 propertiesVector = PropertiesVector2Scene.Instantiate<PropertiesVector2>(PackedScene.GenEditState.Disabled);
		_add_property_editor(key, propertiesVector, Callable.From((Vector2 v) =>
		{
			_on_vector2_changed(v, key);
		}), restValue);
		propertiesVector.value = value;
		propertiesVector.obj = obj;
		propertiesVector.propertyName = propertyName;
		return propertiesVector;
	}

	public PropertiesVector4i add_vector4i(GodotObject obj, string propertyName, StringName key, Vector4I value = default(Vector4I), Variant restValue = default(Variant))
	{
		if (value == default(Vector4I))
		{
			value = Vector4I.Zero;
		}
		if (restValue.VariantType == Variant.Type.Nil)
		{
			restValue = Vector4I.Zero;
		}
		PropertiesVector4i propertiesVector4i = PropertiesVector4iScene.Instantiate<PropertiesVector4i>(PackedScene.GenEditState.Disabled);
		_add_property_editor(key, propertiesVector4i, Callable.From((Vector4I v) =>
		{
			_on_vector4i_changed(v, key);
		}), restValue);
		propertiesVector4i.value = value;
		propertiesVector4i.obj = obj;
		propertiesVector4i.propertyName = propertyName;
		return propertiesVector4i;
	}

	public PropertiesEnum add_enum(GodotObject obj, string propertyName, StringName key, Variant hint, Variant value, Variant restValue)
	{
		PropertiesEnum propertiesEnum = PropertiesEnumScene.Instantiate<PropertiesEnum>(PackedScene.GenEditState.Disabled);
		Dictionary dictionary = new Dictionary();
		if (hint.VariantType == Variant.Type.Dictionary)
		{
			Dictionary dictionary2 = hint.AsGodotDictionary();
			Variant[] array = dictionary2.Keys.ToArray();
			foreach (Variant key2 in array)
			{
				dictionary[key2] = dictionary2[key2];
			}
		}
		if (hint.VariantType == Variant.Type.Array)
		{
			Array array2 = hint.AsGodotArray();
			for (int j = 0; j < array2.Count; j++)
			{
				Variant key3 = array2[j];
				dictionary[key3] = j;
			}
		}
		_add_property_editor(key, propertiesEnum, Callable.From((Variant v) =>
		{
			_on_value_changed(v, key);
		}), restValue);
		propertiesEnum.SetHintDictionary(dictionary);
		propertiesEnum.value = value;
		propertiesEnum.obj = obj;
		propertiesEnum.propertyName = propertyName;
		return propertiesEnum;
	}

	public PropertiesFlag add_flag(GodotObject obj, string propertyName, StringName key, Variant hint, int value = 0, Variant restValue = default(Variant))
	{
		if (restValue.VariantType == Variant.Type.Nil)
		{
			restValue = 0;
		}
		PropertiesFlag propertiesFlag = PropertiesFlagScene.Instantiate<PropertiesFlag>(PackedScene.GenEditState.Disabled);
		_add_property_editor(key, propertiesFlag, Callable.From((int v) =>
		{
			_on_number_changed(v, key);
		}), restValue);
		propertiesFlag.hintDictionary = (Dictionary)hint;
		propertiesFlag.value = value;
		propertiesFlag.obj = obj;
		propertiesFlag.propertyName = propertyName;
		return propertiesFlag;
	}

	public PropertiesArray add_array(GodotObject obj, string propertyName, StringName key, Variant hint, string hintType, Array value = null, Variant restValue = default(Variant))
	{
		if (value == null)
		{
			value = new Array();
		}
		if (restValue.VariantType == Variant.Type.Nil)
		{
			restValue = Colors.White;
		}
		PropertiesArray propertiesArray = PropertiesArrayScene.Instantiate<PropertiesArray>(PackedScene.GenEditState.Disabled);
		_add_property_editor(key, propertiesArray, Callable.From((Array v) =>
		{
			_on_array_changed(v, key);
		}), restValue, inner: false);
		propertiesArray.hint = hint;
		propertiesArray.hintType = hintType;
		propertiesArray.value = value;
		propertiesArray.obj = obj;
		propertiesArray.propertyName = propertyName;
		return propertiesArray;
	}

	public PropertiesArrayVector2 add_array_vector2(GodotObject obj, string propertyName, StringName key, Array<Vector2> value = null, Variant restValue = default(Variant))
	{
		if (value == null)
		{
			value = new Array<Vector2>();
		}
		if (restValue.VariantType == Variant.Type.Nil)
		{
			restValue = new Array();
		}
		PropertiesArrayVector2 propertiesArrayVector = PropertiesArrayVector2Scene.Instantiate<PropertiesArrayVector2>(PackedScene.GenEditState.Disabled);
		_add_property_editor(key, propertiesArrayVector, Callable.From((Array<Vector2> v) =>
		{
			_on_array_vector2_changed(v, key);
		}), restValue, inner: false);
		propertiesArrayVector.value = value;
		propertiesArrayVector.obj = obj;
		propertiesArrayVector.propertyName = propertyName;
		return propertiesArrayVector;
	}

	public PropertiesBase add_file_select(GodotObject obj, string propertyName, StringName key, Variant hint, Variant value, Variant restValue)
	{
		return add_string(obj, propertyName, key, value.AsString(), restValue);
	}

	public PropertiesBase add_map_select(GodotObject obj, string propertyName, StringName key, Variant value, Variant restValue)
	{
		return add_string(obj, propertyName, key, value.AsString(), restValue);
	}

	public PropertiesBase add_room_select(GodotObject obj, string propertyName, StringName key, Variant hint, Variant value, Variant restValue)
	{
		return add_string(obj, propertyName, key, value.AsString(), restValue);
	}

	public void add_group(string label)
	{
		Button button = new Button();
		VBoxContainer vBoxContainer = new VBoxContainer();
		HBoxContainer hBoxContainer = new HBoxContainer();
		Control control = new Control();
		VBoxContainer vBoxContainer2 = new VBoxContainer();
		button.FocusMode = FocusModeEnum.None;
		button.Text = label;
		button.ToggleMode = true;
		button.ButtonPressed = true;
		button.Alignment = HorizontalAlignment.Left;
		button.IconAlignment = HorizontalAlignment.Left;
		button.Icon = GetThemeIcon("arrow", "Tree");
		button.AddThemeFontSizeOverride("font_size", 16);
		button.Toggled += hBoxContainer.SetVisible;
		button.LightMask = 0;
		hBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddThemeConstantOverride("separation", 0);
		hBoxContainer.LightMask = 0;
		control.CustomMinimumSize = new Vector2((float)groupIndent, 0f);
		control.LightMask = 0;
		vBoxContainer2.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		vBoxContainer2.LightMask = 0;
		vBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.LightMask = 0;
		hBoxContainer.AddChild(control, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(vBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		_get_box().AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		_groupStack.Add(vBoxContainer2);
	}

	public void end_group()
	{
		if (_groupStack.Count >= 1)
		{
			_groupStack.RemoveAt(_groupStack.Count - 1);
		}
	}

	public void delete_value(StringName key)
	{
		PropertiesBase propertiesBase = _keys[key].As<PropertiesBase>();
		_keys.Remove(key);
		propertiesBase.container.QueueFree();
		propertiesBase.QueueFree();
	}

	public Variant get_value(StringName key)
	{
		Variant result = _keys[key];
		if (result.VariantType == Variant.Type.Object && result.As<PropertiesBase>() != null)
		{
			return result.As<PropertiesBase>().GetValue();
		}
		return result;
	}

	public bool get_bool(StringName key)
	{
		return _keys[key].As<PropertiesBool>().value;
	}

	public int get_int(StringName key)
	{
		return _keys[key].As<PropertiesInt>().value;
	}

	public double get_float(StringName key)
	{
		return _keys[key].As<PropertiesFloat>().value;
	}

	public string get_string(StringName key)
	{
		return _keys[key].As<PropertiesString>().value;
	}

	public Color get_color(StringName key)
	{
		return _keys[key].As<PropertiesColor>().value;
	}

	public Vector2 get_vector2(StringName key)
	{
		return _keys[key].As<PropertiesVector2>().value;
	}

	public int get_option(StringName key)
	{
		return get_int(key);
	}

	private Control _get_box()
	{
		if (_groupStack.Count == 0)
		{
			return this;
		}
		return _groupStack[_groupStack.Count - 1].As<Control>();
	}

	public void set_value(StringName key, Variant value)
	{
		if (!_keys.ContainsKey(key))
		{
			return;
		}
		PropertiesBase propertiesBase = _keys[key].As<PropertiesBase>();
		if (propertiesBase == null || !propertiesBase.HasFocus())
		{
			if (propertiesBase is PropertiesBool propertiesBool)
			{
				propertiesBool.SetValue(value.AsBool());
			}
			else if (propertiesBase is PropertiesInt propertiesInt)
			{
				propertiesInt.SetValue(value.AsInt32());
			}
			else if (propertiesBase is PropertiesFloat propertiesFloat)
			{
				propertiesFloat.SetValue(value.AsDouble());
			}
			else if (propertiesBase is PropertiesString propertiesString)
			{
				propertiesString.SetValue(value.AsString());
			}
			else if (propertiesBase is PropertiesMultilineString propertiesMultilineString)
			{
				propertiesMultilineString.SetValue(value.AsString());
			}
			else if (propertiesBase is PropertiesVector2 propertiesVector)
			{
				propertiesVector.SetValue(value.AsVector2());
			}
			else if (propertiesBase is PropertiesVector4i propertiesVector4i)
			{
				propertiesVector4i.SetValue(value.AsVector4I());
			}
			else if (propertiesBase is PropertiesColor propertiesColor)
			{
				propertiesColor.SetValue(value.AsColor());
			}
			else if (propertiesBase is PropertiesEnum propertiesEnum)
			{
				propertiesEnum.SetValue(value);
			}
			else if (propertiesBase is PropertiesFlag propertiesFlag)
			{
				propertiesFlag.SetValue(value.AsInt32());
			}
			else if (propertiesBase is PropertiesArray propertiesArray)
			{
				propertiesArray.SetValue(value.AsGodotArray());
			}
			else if (propertiesBase is PropertiesArrayVector2 propertiesArrayVector)
			{
				propertiesArrayVector.SetValue(value.AsGodotArray<Vector2>());
			}
		}
	}

	private void _add_property_editor(StringName key, Control editor, Callable signalHandler, Variant restValue = default(Variant), bool inner = true)
	{
		_keys[key] = editor;
		PropertiesBase propertiesBase = editor as PropertiesBase;
		if (propertiesBase != null)
		{
			propertiesBase.key = key;
			propertiesBase.propertiesBox = this;
		}
		LevelEditorPropertiesContainer levelEditorPropertiesContainer = (propertiesBase.container = PropertiesContainerScene.Instantiate<LevelEditorPropertiesContainer>(PackedScene.GenEditState.Disabled));
		_get_box().AddChild(levelEditorPropertiesContainer, forceReadableName: false, InternalMode.Disabled);
		if (inner)
		{
			editor.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			levelEditorPropertiesContainer.innerContainer.AddChild(editor, forceReadableName: false, InternalMode.Disabled);
		}
		else
		{
			editor.SizeFlagsVertical = SizeFlags.ExpandFill;
			levelEditorPropertiesContainer.outerContainer.AddChild(editor, forceReadableName: false, InternalMode.Disabled);
		}
		levelEditorPropertiesContainer.keyLabel.Text = key;
		levelEditorPropertiesContainer.restValue = restValue;
		levelEditorPropertiesContainer.propertyEditor = propertiesBase;
		if (restValue.VariantType != Variant.Type.Nil)
		{
			levelEditorPropertiesContainer.refreshButton.Pressed += () =>
			{
				signalHandler.Call(restValue, key);
			};
		}
		_subscribe_value_changed(editor, key);
	}

	private void _subscribe_value_changed(Control editor, StringName key)
	{
		if (editor is PropertiesBool propertiesBool)
		{
			propertiesBool.OnValueChanged += (bool v) =>
			{
				_on_bool_changed(v, key);
			};
		}
		else if (editor is PropertiesInt propertiesInt)
		{
			propertiesInt.OnValueChanged += (int v) =>
			{
				_on_number_changed(v, key);
			};
		}
		else if (editor is PropertiesFloat propertiesFloat)
		{
			propertiesFloat.OnValueChanged += (double v) =>
			{
				_on_number_changed(v, key);
			};
		}
		else if (editor is PropertiesString propertiesString)
		{
			propertiesString.OnValueChanged += (string v) =>
			{
				_on_string_changed(v, key);
			};
		}
		else if (editor is PropertiesMultilineString propertiesMultilineString)
		{
			propertiesMultilineString.OnValueChanged += (string v) =>
			{
				_on_string_changed(v, key);
			};
		}
		else if (editor is PropertiesVector2 propertiesVector)
		{
			propertiesVector.OnValueChanged += (Vector2 v) =>
			{
				_on_vector2_changed(v, key);
			};
		}
		else if (editor is PropertiesVector4i propertiesVector4i)
		{
			propertiesVector4i.OnValueChanged += (Vector4I v) =>
			{
				_on_vector4i_changed(v, key);
			};
		}
		else if (editor is PropertiesColor propertiesColor)
		{
			propertiesColor.OnValueChanged += (Color v) =>
			{
				_on_color_changed(v, key);
			};
		}
		else if (editor is PropertiesEnum propertiesEnum)
		{
			propertiesEnum.OnValueChanged += (Variant v) =>
			{
				_on_value_changed(v, key);
			};
		}
		else if (editor is PropertiesFlag propertiesFlag)
		{
			propertiesFlag.OnValueChanged += (int v) =>
			{
				_on_number_changed(v, key);
			};
		}
		else if (editor is PropertiesArray propertiesArray)
		{
			propertiesArray.OnValueChanged += (Array v) =>
			{
				_on_array_changed(v, key);
			};
		}
		else if (editor is PropertiesArrayVector2 propertiesArrayVector)
		{
			propertiesArrayVector.OnValueChanged += (Array<Vector2> v) =>
			{
				_on_array_vector2_changed(v, key);
			};
		}
	}

	private void _on_value_changed(Variant value, StringName key)
	{
		OnValueChanged?.Invoke(key, value);
	}

	private void _on_number_changed(double value, StringName key)
	{
		OnNumberChanged?.Invoke(key, value);
		OnValueChanged?.Invoke(key, value);
	}

	private void _on_number_changed(int value, StringName key)
	{
		OnNumberChanged?.Invoke(key, value);
		OnValueChanged?.Invoke(key, value);
	}

	private void _on_string_changed(string value, StringName key)
	{
		OnStringChanged?.Invoke(key, value);
		OnValueChanged?.Invoke(key, value);
	}

	private void _on_bool_changed(bool value, StringName key)
	{
		OnBoolChanged?.Invoke(key, value);
		OnValueChanged?.Invoke(key, value);
	}

	private void _on_color_changed(Color value, StringName key)
	{
		OnColorChanged?.Invoke(key, value);
		OnValueChanged?.Invoke(key, value);
	}

	private void _on_vector2_changed(Vector2 value, StringName key)
	{
		OnVector2Changed?.Invoke(key, value);
		OnValueChanged?.Invoke(key, value);
	}

	private void _on_vector4i_changed(Vector4I value, StringName key)
	{
		OnVector4iChanged?.Invoke(key, value);
		OnValueChanged?.Invoke(key, value);
	}

	private void _on_array_changed(Array value, StringName key)
	{
		OnArrayChanged?.Invoke(key, value);
		OnValueChanged?.Invoke(key, value);
	}

	private void _on_array_vector2_changed(Array<Vector2> value, StringName key)
	{
		OnArrayVector2Changed?.Invoke(key, value);
		OnValueChanged?.Invoke(key, value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(39)
		{
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.add_bool, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_int, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_float, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_string, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isMultiline", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.add_color, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_vector2, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_vector4i, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector4I, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_enum, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "hint", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_flag, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "hint", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_array, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "hint", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.String, "hintType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_array_vector2, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_file_select, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "hint", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_map_select, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_room_select, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "hint", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.add_group, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.end_group, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.delete_value, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.get_value, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.get_bool, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.get_int, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.get_float, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.get_string, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.get_color, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.get_vector2, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.get_option, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._get_box, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.set_value, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._add_property_editor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Callable, "signalHandler", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "restValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Bool, "inner", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._subscribe_value_changed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._on_value_changed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._on_number_changed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._on_string_changed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._on_bool_changed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._on_color_changed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._on_vector2_changed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._on_vector4i_changed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector4I, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._on_array_changed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._on_array_vector2_changed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.add_bool && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<PropertiesBool>(add_bool(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4])));
			return true;
		}
		if (method == MethodName.add_int && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<PropertiesInt>(add_int(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4])));
			return true;
		}
		if (method == MethodName.add_float && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<PropertiesFloat>(add_float(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4])));
			return true;
		}
		if (method == MethodName.add_string && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<PropertiesBase>(add_string(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.add_color && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<PropertiesColor>(add_color(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4])));
			return true;
		}
		if (method == MethodName.add_vector2 && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<PropertiesVector2>(add_vector2(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4])));
			return true;
		}
		if (method == MethodName.add_vector4i && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<PropertiesVector4i>(add_vector4i(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Vector4I>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4])));
			return true;
		}
		if (method == MethodName.add_enum && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<PropertiesEnum>(add_enum(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4]), VariantUtils.ConvertTo<Variant>(in args[5])));
			return true;
		}
		if (method == MethodName.add_flag && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<PropertiesFlag>(add_flag(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<Variant>(in args[5])));
			return true;
		}
		if (method == MethodName.add_array && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<PropertiesArray>(add_array(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<Array>(in args[5]), VariantUtils.ConvertTo<Variant>(in args[6])));
			return true;
		}
		if (method == MethodName.add_array_vector2 && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<PropertiesArrayVector2>(add_array_vector2(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertToArray<Vector2>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4])));
			return true;
		}
		if (method == MethodName.add_file_select && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<PropertiesBase>(add_file_select(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4]), VariantUtils.ConvertTo<Variant>(in args[5])));
			return true;
		}
		if (method == MethodName.add_map_select && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<PropertiesBase>(add_map_select(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4])));
			return true;
		}
		if (method == MethodName.add_room_select && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<PropertiesBase>(add_room_select(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]), VariantUtils.ConvertTo<Variant>(in args[4]), VariantUtils.ConvertTo<Variant>(in args[5])));
			return true;
		}
		if (method == MethodName.add_group && args.Count == 1)
		{
			add_group(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.end_group && args.Count == 0)
		{
			end_group();
			ret = default;
			return true;
		}
		if (method == MethodName.delete_value && args.Count == 1)
		{
			delete_value(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.get_value && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(get_value(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.get_bool && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(get_bool(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.get_int && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(get_int(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.get_float && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(get_float(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.get_string && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(get_string(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.get_color && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(get_color(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.get_vector2 && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(get_vector2(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.get_option && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(get_option(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._get_box && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(_get_box());
			return true;
		}
		if (method == MethodName.set_value && args.Count == 2)
		{
			set_value(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._add_property_editor && args.Count == 5)
		{
			_add_property_editor(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]), VariantUtils.ConvertTo<Callable>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName._subscribe_value_changed && args.Count == 2)
		{
			_subscribe_value_changed(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._on_value_changed && args.Count == 2)
		{
			_on_value_changed(VariantUtils.ConvertTo<Variant>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._on_number_changed && args.Count == 2)
		{
			_on_number_changed(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._on_string_changed && args.Count == 2)
		{
			_on_string_changed(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._on_bool_changed && args.Count == 2)
		{
			_on_bool_changed(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._on_color_changed && args.Count == 2)
		{
			_on_color_changed(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._on_vector2_changed && args.Count == 2)
		{
			_on_vector2_changed(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._on_vector4i_changed && args.Count == 2)
		{
			_on_vector4i_changed(VariantUtils.ConvertTo<Vector4I>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._on_array_changed && args.Count == 2)
		{
			_on_array_changed(VariantUtils.ConvertTo<Array>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._on_array_vector2_changed && args.Count == 2)
		{
			_on_array_vector2_changed(VariantUtils.ConvertToArray<Vector2>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.add_bool)
		{
			return true;
		}
		if (method == MethodName.add_int)
		{
			return true;
		}
		if (method == MethodName.add_float)
		{
			return true;
		}
		if (method == MethodName.add_string)
		{
			return true;
		}
		if (method == MethodName.add_color)
		{
			return true;
		}
		if (method == MethodName.add_vector2)
		{
			return true;
		}
		if (method == MethodName.add_vector4i)
		{
			return true;
		}
		if (method == MethodName.add_enum)
		{
			return true;
		}
		if (method == MethodName.add_flag)
		{
			return true;
		}
		if (method == MethodName.add_array)
		{
			return true;
		}
		if (method == MethodName.add_array_vector2)
		{
			return true;
		}
		if (method == MethodName.add_file_select)
		{
			return true;
		}
		if (method == MethodName.add_map_select)
		{
			return true;
		}
		if (method == MethodName.add_room_select)
		{
			return true;
		}
		if (method == MethodName.add_group)
		{
			return true;
		}
		if (method == MethodName.end_group)
		{
			return true;
		}
		if (method == MethodName.delete_value)
		{
			return true;
		}
		if (method == MethodName.get_value)
		{
			return true;
		}
		if (method == MethodName.get_bool)
		{
			return true;
		}
		if (method == MethodName.get_int)
		{
			return true;
		}
		if (method == MethodName.get_float)
		{
			return true;
		}
		if (method == MethodName.get_string)
		{
			return true;
		}
		if (method == MethodName.get_color)
		{
			return true;
		}
		if (method == MethodName.get_vector2)
		{
			return true;
		}
		if (method == MethodName.get_option)
		{
			return true;
		}
		if (method == MethodName._get_box)
		{
			return true;
		}
		if (method == MethodName.set_value)
		{
			return true;
		}
		if (method == MethodName._add_property_editor)
		{
			return true;
		}
		if (method == MethodName._subscribe_value_changed)
		{
			return true;
		}
		if (method == MethodName._on_value_changed)
		{
			return true;
		}
		if (method == MethodName._on_number_changed)
		{
			return true;
		}
		if (method == MethodName._on_string_changed)
		{
			return true;
		}
		if (method == MethodName._on_bool_changed)
		{
			return true;
		}
		if (method == MethodName._on_color_changed)
		{
			return true;
		}
		if (method == MethodName._on_vector2_changed)
		{
			return true;
		}
		if (method == MethodName._on_vector4i_changed)
		{
			return true;
		}
		if (method == MethodName._on_array_changed)
		{
			return true;
		}
		if (method == MethodName._on_array_vector2_changed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.groupIndent)
		{
			groupIndent = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._keys)
		{
			_keys = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._groupStack)
		{
			_groupStack = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.groupIndent)
		{
			value = VariantUtils.CreateFrom<double>(groupIndent);
			return true;
		}
		if (name == PropertyName._keys)
		{
			value = VariantUtils.CreateFrom(in _keys);
			return true;
		}
		if (name == PropertyName._groupStack)
		{
			value = VariantUtils.CreateFrom(in _groupStack);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.groupIndent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._keys, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._groupStack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.groupIndent, Variant.From<double>(groupIndent));
		info.AddProperty(PropertyName._keys, Variant.From(in _keys));
		info.AddProperty(PropertyName._groupStack, Variant.From(in _groupStack));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.groupIndent, out var value))
		{
			groupIndent = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._keys, out var value2))
		{
			_keys = value2.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._groupStack, out var value3))
		{
			_groupStack = value3.As<Array>();
		}
	}
}
