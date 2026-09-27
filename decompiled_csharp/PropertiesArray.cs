using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/Properties/Type/Array/PropertiesArray.cs")]
public class PropertiesArray : PropertiesBase
{
	public delegate void ValueChangedEventHandler(Array value);

	public new class MethodName : PropertiesBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ValueChange = "ValueChange";

		public new static readonly StringName CanSetValue = "CanSetValue";

		public static readonly StringName SetValue = "SetValue";

		public static readonly StringName FoldButtonPressed = "FoldButtonPressed";

		public static readonly StringName AddButtonPressed = "AddButtonPressed";

		public static readonly StringName AddEditor = "AddEditor";

		public static readonly StringName AddPropertyEditor = "AddPropertyEditor";

		public static readonly StringName AddBool = "AddBool";

		public static readonly StringName AddInt = "AddInt";

		public static readonly StringName AddFloat = "AddFloat";

		public static readonly StringName AddString = "AddString";

		public static readonly StringName AddMultilineString = "AddMultilineString";

		public static readonly StringName AddVector2 = "AddVector2";

		public static readonly StringName AddColor = "AddColor";

		public static readonly StringName AddEnum = "AddEnum";

		public static readonly StringName AddFlag = "AddFlag";

		public static readonly StringName AddArray = "AddArray";
	}

	public new class PropertyName : PropertiesBase.PropertyName
	{
		public static readonly StringName value = "value";

		public static readonly StringName foldButton = "foldButton";

		public static readonly StringName foldContainer = "foldContainer";

		public static readonly StringName propertiesContainer = "propertiesContainer";

		public static readonly StringName hintType = "hintType";

		public static readonly StringName hint = "hint";

		public static readonly StringName boxList = "boxList";

		public static readonly StringName timer = "timer";

		public static readonly StringName _value = "_value";
	}

	public new class SignalName : PropertiesBase.SignalName
	{
	}

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

	private Button foldButton;

	private VBoxContainer foldContainer;

	private VBoxContainer propertiesContainer;

	public string hintType = "Int";

	public Variant hint;

	public Array<LevelEditorPropertiesContainer> boxList = new Array<LevelEditorPropertiesContainer>();

	public double timer;

	private Array _value = new Array();

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

	public Array value
	{
		get
		{
			return _value;
		}
		set
		{
			if (!IsNodeReady())
			{
				return;
			}
			_value = value;
			foldButton.Text = $"数组( 大小 {_value.Count} )";
			foreach (LevelEditorPropertiesContainer box in boxList)
			{
				if (box != null && GodotObject.IsInstanceValid(box))
				{
					box.QueueFree();
				}
			}
			boxList.Clear();
			for (int i = 0; i < _value.Count; i++)
			{
				Variant variant = _value[i];
				AddEditor(i, variant);
			}
		}
	}

	public event ValueChangedEventHandler OnValueChanged;

	public override void _Ready()
	{
		foldButton = GetNode<Button>("%FoldButton");
		foldContainer = GetNode<VBoxContainer>("%FoldContainer");
		propertiesContainer = GetNode<VBoxContainer>("%PropertiesContainer");
		foldButton.Pressed += FoldButtonPressed;
		GetNode<Button>("%AddButton").Pressed += AddButtonPressed;
	}

	public void ValueChange(Variant _value)
	{
		Array array = new Array();
		foreach (LevelEditorPropertiesContainer box in boxList)
		{
			if (box != null && GodotObject.IsInstanceValid(box) && box.propertyEditor != null)
			{
				array.Add(box.propertyEditor.GetValue());
			}
		}
		_value = array;
		OnValueChanged?.Invoke(array);
	}

	public override bool CanSetValue()
	{
		foreach (LevelEditorPropertiesContainer box in boxList)
		{
			if (box != null && GodotObject.IsInstanceValid(box) && box.propertyEditor != null && !box.propertyEditor.CanSetValue())
			{
				return false;
			}
		}
		timer += GetProcessDeltaTime();
		if (timer > 0.2)
		{
			timer = 0.0;
			return true;
		}
		return false;
	}

	public void SetValue(Array newValue)
	{
		value = newValue;
	}

	public void FoldButtonPressed()
	{
		foldContainer.Visible = foldButton.ButtonPressed;
	}

	public void AddButtonPressed()
	{
		AddEditor(value.Count, default);
		ValueChange(default);
	}

	public void AddEditor(int id, Variant _value)
	{
		string text = hintType;
		if (text == null)
		{
			return;
		}
		switch (text.Length)
		{
		case 4:
			switch (text[0])
			{
			case 'B':
				if (text == "Bool")
				{
					AddBool(id, _value);
				}
				break;
			case 'E':
				if (text == "Enum")
				{
					AddEnum(id, _value);
				}
				break;
			case 'F':
				if (text == "Flag")
				{
					AddFlag(id, _value);
				}
				break;
			case 'C':
			case 'D':
				break;
			}
			break;
		case 5:
			switch (text[0])
			{
			case 'F':
				if (text == "Float")
				{
					AddFloat(id, _value);
				}
				break;
			case 'C':
				if (text == "Color")
				{
					AddColor(id, _value);
				}
				break;
			case 'A':
				if (text == "Array")
				{
					AddArray(id, _value);
				}
				break;
			}
			break;
		case 3:
			if (text == "Int")
			{
				AddInt(id, _value);
			}
			break;
		case 6:
			if (text == "String")
			{
				AddString(id, _value);
			}
			break;
		case 15:
			if (text == "MultilineString")
			{
				AddMultilineString(id, _value);
			}
			break;
		case 7:
			if (text == "Vector2")
			{
				AddVector2(id, _value);
			}
			break;
		}
	}

	public void AddPropertyEditor(int index, Variant _value, Control editor, bool inner = true)
	{
		LevelEditorPropertiesContainer levelEditorPropertiesContainer = PropertiesContainerScene.Instantiate<LevelEditorPropertiesContainer>(PackedScene.GenEditState.Disabled);
		(editor as PropertiesBase).isTargetObj = false;
		propertiesContainer.AddChild(levelEditorPropertiesContainer, forceReadableName: false, InternalMode.Disabled);
		boxList.Add(levelEditorPropertiesContainer);
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
		levelEditorPropertiesContainer.restValue = default;
		levelEditorPropertiesContainer.keyLabel.Text = index.ToString();
		levelEditorPropertiesContainer.propertyEditor = editor as PropertiesBase;
		if (editor is PropertiesBase propertiesBase)
		{
			propertiesBase.obj = obj;
			propertiesBase.propertyName = propertyName;
		}
		if (editor is PropertiesBool propertiesBool)
		{
			propertiesBool.OnValueChanged += (bool v) =>
			{
				ValueChange(v);
			};
		}
		else if (editor is PropertiesInt propertiesInt)
		{
			propertiesInt.OnValueChanged += (int v) =>
			{
				ValueChange(v);
			};
		}
		else if (editor is PropertiesFloat propertiesFloat)
		{
			propertiesFloat.OnValueChanged += (double v) =>
			{
				ValueChange(v);
			};
		}
		else if (editor is PropertiesString propertiesString)
		{
			propertiesString.OnValueChanged += (string v) =>
			{
				ValueChange(v);
			};
		}
		else if (editor is PropertiesMultilineString propertiesMultilineString)
		{
			propertiesMultilineString.OnValueChanged += (string v) =>
			{
				ValueChange(v);
			};
		}
		else if (editor is PropertiesVector2 propertiesVector)
		{
			propertiesVector.OnValueChanged += (Vector2 v) =>
			{
				ValueChange(v);
			};
		}
		else if (editor is PropertiesColor propertiesColor)
		{
			propertiesColor.OnValueChanged += (Color v) =>
			{
				ValueChange(v);
			};
		}
		else if (editor is PropertiesVector4i propertiesVector4i)
		{
			propertiesVector4i.OnValueChanged += (Vector4I v) =>
			{
				ValueChange(v);
			};
		}
		else if (editor is PropertiesEnum propertiesEnum)
		{
			propertiesEnum.OnValueChanged += (Variant v) =>
			{
				ValueChange(v);
			};
		}
		else if (editor is PropertiesFlag propertiesFlag)
		{
			propertiesFlag.OnValueChanged += (int v) =>
			{
				ValueChange(v);
			};
		}
		else if (editor is PropertiesArray propertiesArray)
		{
			propertiesArray.OnValueChanged += (Array v) =>
			{
				ValueChange(v);
			};
		}
		else if (editor is PropertiesArrayVector2 propertiesArrayVector)
		{
			propertiesArrayVector.OnValueChanged += (Array<Vector2> v) =>
			{
				ValueChange(v);
			};
		}
	}

	public PropertiesBool AddBool(int index, Variant _value = default(Variant))
	{
		if (_value.VariantType == Variant.Type.Nil)
		{
			_value = false;
		}
		PropertiesBool propertiesBool = PropertiesBoolScene.Instantiate<PropertiesBool>(PackedScene.GenEditState.Disabled);
		AddPropertyEditor(index, _value, propertiesBool);
		propertiesBool.value = _value.AsBool();
		return propertiesBool;
	}

	public PropertiesInt AddInt(int index, Variant _value = default(Variant))
	{
		if (_value.VariantType == Variant.Type.Nil)
		{
			_value = 0;
		}
		PropertiesInt propertiesInt = PropertiesIntScene.Instantiate<PropertiesInt>(PackedScene.GenEditState.Disabled);
		AddPropertyEditor(index, _value, propertiesInt);
		propertiesInt.value = _value.AsInt32();
		return propertiesInt;
	}

	public PropertiesFloat AddFloat(int index, Variant _value = default(Variant))
	{
		if (_value.VariantType == Variant.Type.Nil)
		{
			_value = 0.0;
		}
		PropertiesFloat propertiesFloat = PropertiesFloatScene.Instantiate<PropertiesFloat>(PackedScene.GenEditState.Disabled);
		AddPropertyEditor(index, _value, propertiesFloat);
		propertiesFloat.value = _value.AsDouble();
		return propertiesFloat;
	}

	public PropertiesString AddString(int index, Variant _value = default(Variant))
	{
		if (_value.VariantType == Variant.Type.Nil)
		{
			_value = "";
		}
		PropertiesString propertiesString = PropertiesStringScene.Instantiate<PropertiesString>(PackedScene.GenEditState.Disabled);
		AddPropertyEditor(index, _value, propertiesString);
		propertiesString.value = _value.AsString();
		return propertiesString;
	}

	public PropertiesMultilineString AddMultilineString(int index, Variant _value = default(Variant))
	{
		if (_value.VariantType == Variant.Type.Nil)
		{
			_value = "";
		}
		PropertiesMultilineString propertiesMultilineString = PropertiesMultilineStringScene.Instantiate<PropertiesMultilineString>(PackedScene.GenEditState.Disabled);
		AddPropertyEditor(index, _value, propertiesMultilineString, inner: false);
		propertiesMultilineString.value = _value.AsString();
		return propertiesMultilineString;
	}

	public PropertiesVector2 AddVector2(int index, Variant _value = default(Variant))
	{
		if (_value.VariantType == Variant.Type.Nil)
		{
			_value = Vector2.Zero;
		}
		PropertiesVector2 propertiesVector = PropertiesVector2Scene.Instantiate<PropertiesVector2>(PackedScene.GenEditState.Disabled);
		AddPropertyEditor(index, _value, propertiesVector);
		propertiesVector.value = _value.AsVector2();
		return propertiesVector;
	}

	public PropertiesColor AddColor(int index, Variant _value = default(Variant))
	{
		if (_value.VariantType == Variant.Type.Nil)
		{
			_value = Colors.Black;
		}
		PropertiesColor propertiesColor = PropertiesColorScene.Instantiate<PropertiesColor>(PackedScene.GenEditState.Disabled);
		AddPropertyEditor(index, _value, propertiesColor);
		propertiesColor.value = _value.AsColor();
		return propertiesColor;
	}

	public PropertiesEnum AddEnum(int index, Variant _value = default(Variant))
	{
		if (_value.VariantType == Variant.Type.Nil)
		{
			_value = 0;
		}
		PropertiesEnum propertiesEnum = PropertiesEnumScene.Instantiate<PropertiesEnum>(PackedScene.GenEditState.Disabled);
		AddPropertyEditor(index, _value, propertiesEnum);
		Dictionary dictionary = new Dictionary();
		if (hint.VariantType == Variant.Type.Dictionary)
		{
			Dictionary dictionary2 = hint.AsGodotDictionary();
			Variant[] array = dictionary2.Keys.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				Variant variant = array[i];
				if (dictionary2[variant].VariantType == Variant.Type.Int)
				{
					dictionary[variant] = dictionary2[variant];
				}
				if (dictionary2[variant].VariantType == Variant.Type.String)
				{
					dictionary[variant] = i;
				}
			}
		}
		if (hint.VariantType == Variant.Type.Array)
		{
			Array array2 = hint.AsGodotArray();
			for (int j = 0; j < array2.Count; j++)
			{
				Variant variant2 = array2[j];
				dictionary[variant2] = j;
			}
		}
		propertiesEnum.SetHintDictionary(dictionary);
		propertiesEnum.value = _value.AsInt32();
		return propertiesEnum;
	}

	public PropertiesFlag AddFlag(int index, Variant _value = default(Variant))
	{
		if (_value.VariantType == Variant.Type.Nil)
		{
			_value = 0;
		}
		PropertiesFlag propertiesFlag = PropertiesFlagScene.Instantiate<PropertiesFlag>(PackedScene.GenEditState.Disabled);
		AddPropertyEditor(index, _value, propertiesFlag);
		propertiesFlag.hintDictionary = hint.AsGodotDictionary();
		propertiesFlag.value = _value.AsInt32();
		return propertiesFlag;
	}

	public PropertiesArray AddArray(int index, Variant _value = default(Variant))
	{
		if (_value.VariantType == Variant.Type.Nil)
		{
			_value = new Array();
		}
		PropertiesArray propertiesArray = PropertiesArrayScene.Instantiate<PropertiesArray>(PackedScene.GenEditState.Disabled);
		AddPropertyEditor(index, _value, propertiesArray, inner: false);
		propertiesArray.hintType = hintType;
		propertiesArray.value = _value.AsGodotArray();
		return propertiesArray;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CanSetValue, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FoldButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddPropertyEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "inner", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddBool, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddInt, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddFloat, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddString, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddMultilineString, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddVector2, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddColor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddEnum, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddFlag, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddArray, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "_value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
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
		if (method == MethodName.ValueChange && args.Count == 1)
		{
			ValueChange(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanSetValue && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSetValue());
			return true;
		}
		if (method == MethodName.SetValue && args.Count == 1)
		{
			SetValue(VariantUtils.ConvertTo<Array>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FoldButtonPressed && args.Count == 0)
		{
			FoldButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.AddButtonPressed && args.Count == 0)
		{
			AddButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.AddEditor && args.Count == 2)
		{
			AddEditor(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddPropertyEditor && args.Count == 4)
		{
			AddPropertyEditor(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<Control>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddBool && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PropertiesBool>(AddBool(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.AddInt && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PropertiesInt>(AddInt(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.AddFloat && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PropertiesFloat>(AddFloat(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.AddString && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PropertiesString>(AddString(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.AddMultilineString && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PropertiesMultilineString>(AddMultilineString(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.AddVector2 && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PropertiesVector2>(AddVector2(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.AddColor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PropertiesColor>(AddColor(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.AddEnum && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PropertiesEnum>(AddEnum(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.AddFlag && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PropertiesFlag>(AddFlag(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.AddArray && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<PropertiesArray>(AddArray(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
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
		if (method == MethodName.ValueChange)
		{
			return true;
		}
		if (method == MethodName.CanSetValue)
		{
			return true;
		}
		if (method == MethodName.SetValue)
		{
			return true;
		}
		if (method == MethodName.FoldButtonPressed)
		{
			return true;
		}
		if (method == MethodName.AddButtonPressed)
		{
			return true;
		}
		if (method == MethodName.AddEditor)
		{
			return true;
		}
		if (method == MethodName.AddPropertyEditor)
		{
			return true;
		}
		if (method == MethodName.AddBool)
		{
			return true;
		}
		if (method == MethodName.AddInt)
		{
			return true;
		}
		if (method == MethodName.AddFloat)
		{
			return true;
		}
		if (method == MethodName.AddString)
		{
			return true;
		}
		if (method == MethodName.AddMultilineString)
		{
			return true;
		}
		if (method == MethodName.AddVector2)
		{
			return true;
		}
		if (method == MethodName.AddColor)
		{
			return true;
		}
		if (method == MethodName.AddEnum)
		{
			return true;
		}
		if (method == MethodName.AddFlag)
		{
			return true;
		}
		if (method == MethodName.AddArray)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.value)
		{
			this.value = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.foldButton)
		{
			foldButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName.foldContainer)
		{
			foldContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.propertiesContainer)
		{
			propertiesContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.hintType)
		{
			hintType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.hint)
		{
			hint = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.boxList)
		{
			boxList = VariantUtils.ConvertToArray<LevelEditorPropertiesContainer>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._value)
		{
			_value = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.value)
		{
			value = VariantUtils.CreateFrom<Array>(this.value);
			return true;
		}
		if (name == PropertyName.foldButton)
		{
			value = VariantUtils.CreateFrom(in foldButton);
			return true;
		}
		if (name == PropertyName.foldContainer)
		{
			value = VariantUtils.CreateFrom(in foldContainer);
			return true;
		}
		if (name == PropertyName.propertiesContainer)
		{
			value = VariantUtils.CreateFrom(in propertiesContainer);
			return true;
		}
		if (name == PropertyName.hintType)
		{
			value = VariantUtils.CreateFrom(in hintType);
			return true;
		}
		if (name == PropertyName.hint)
		{
			value = VariantUtils.CreateFrom(in hint);
			return true;
		}
		if (name == PropertyName.boxList)
		{
			value = VariantUtils.CreateFromArray(boxList);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName._value)
		{
			value = VariantUtils.CreateFrom(in _value);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.foldButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.foldContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.propertiesContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.hintType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, PropertyName.hint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.boxList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._value, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.value, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.value, Variant.From<Array>(value));
		info.AddProperty(PropertyName.foldButton, Variant.From(in foldButton));
		info.AddProperty(PropertyName.foldContainer, Variant.From(in foldContainer));
		info.AddProperty(PropertyName.propertiesContainer, Variant.From(in propertiesContainer));
		info.AddProperty(PropertyName.hintType, Variant.From(in hintType));
		info.AddProperty(PropertyName.hint, Variant.From(in hint));
		info.AddProperty(PropertyName.boxList, Variant.CreateFrom(boxList));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName._value, Variant.From(in _value));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.value, out var variant))
		{
			value = variant.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.foldButton, out var variant2))
		{
			foldButton = variant2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName.foldContainer, out var variant3))
		{
			foldContainer = variant3.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.propertiesContainer, out var variant4))
		{
			propertiesContainer = variant4.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.hintType, out var variant5))
		{
			hintType = variant5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.hint, out var variant6))
		{
			hint = variant6.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.boxList, out var variant7))
		{
			boxList = variant7.AsGodotArray<LevelEditorPropertiesContainer>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var variant8))
		{
			timer = variant8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._value, out var variant9))
		{
			_value = variant9.As<Array>();
		}
	}
}
