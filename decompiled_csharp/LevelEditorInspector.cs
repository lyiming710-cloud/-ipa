using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Prefab/GUI/LevelEditor/Inspector/LevelEditorInspector.cs")]
public class LevelEditorInspector : PanelContainer
{
	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Init = "Init";

		public static readonly StringName ReFresh = "ReFresh";

		public static readonly StringName AddProperty = "AddProperty";

		public static readonly StringName GetRestValue = "GetRestValue";

		public static readonly StringName GetCanParseValue = "GetCanParseValue";

		public static readonly StringName GetParseValue = "GetParseValue";

		public static readonly StringName GetTypeString = "GetTypeString";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName ChangeValue = "ChangeValue";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName title = "title";

		public static readonly StringName titleLabel = "titleLabel";

		public static readonly StringName propertiesBox = "propertiesBox";

		public static readonly StringName nowCheckDictionary = "nowCheckDictionary";

		public static readonly StringName nowObj = "nowObj";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private Label titleLabel;

	private PropertiesBox propertiesBox;

	public Dictionary nowCheckDictionary = new Dictionary();

	public GodotObject nowObj;

	[Export(PropertyHint.None, "")]
	public string title { get; set; } = "检查器";

	public override void _Ready()
	{
		titleLabel = GetNode<Label>("%TitleLabel");
		propertiesBox = GetNode<PropertiesBox>("%PropertiesBox");
		propertiesBox.OnValueChanged += ChangeValue;
		titleLabel.Text = title;
	}

	public void Init(GodotObject obj, Dictionary propertiesData)
	{
		ReFresh(propertiesData);
		nowObj = obj;
	}

	public void ReFresh(Dictionary propertiesData)
	{
		Clear();
		foreach (Variant key in propertiesData.Keys)
		{
			string text = key.AsString();
			Dictionary dictionary = propertiesData[text].AsGodotDictionary();
			propertiesBox.add_group(text);
			nowCheckDictionary.Merge(dictionary, overwrite: true);
			foreach (Variant key2 in dictionary.Keys)
			{
				string text2 = key2.AsString();
				AddProperty(dictionary[text2].AsGodotDictionary(), text2, text + "/" + text2);
			}
			propertiesBox.end_group();
		}
	}

	public PropertiesBase AddProperty(Dictionary properties, string propertiesShowName, string path = "")
	{
		if (!nowCheckDictionary.ContainsKey(propertiesShowName))
		{
			nowCheckDictionary[propertiesShowName] = properties;
		}
		GodotObject godotObject = properties["Object"].AsGodotObject();
		string text = properties["Type"].AsString();
		string text2 = properties["Property"].AsString();
		Variant restValue = default;
		if (properties.ContainsKey("Rest"))
		{
			restValue = properties["Rest"];
		}
		Variant value = godotObject.GetIndexed(text2);
		bool isTargetObj = true;
		Variant variant = false;
		if (value.VariantType == Variant.Type.Dictionary)
		{
			value = value.AsGodotDictionary()[propertiesShowName];
			isTargetObj = false;
			variant = true;
		}
		if (properties.ContainsKey("Close"))
		{
			variant = properties["Close"];
		}
		PropertiesBase propertiesBase = null;
		switch (text)
		{
		case "Bool":
			propertiesBase = propertiesBox.add_bool(godotObject, text2, propertiesShowName, value.AsBool(), restValue);
			break;
		case "Int":
			propertiesBase = propertiesBox.add_int(godotObject, text2, propertiesShowName, value.AsInt32(), restValue);
			break;
		case "Float":
			propertiesBase = propertiesBox.add_float(godotObject, text2, propertiesShowName, value.AsDouble(), restValue);
			break;
		case "String":
			propertiesBase = propertiesBox.add_string(godotObject, text2, propertiesShowName, value.AsString(), restValue);
			break;
		case "MultilineString":
			propertiesBase = propertiesBox.add_string(godotObject, text2, propertiesShowName, value.AsString(), restValue, isMultiline: true);
			break;
		case "Color":
			if (restValue.VariantType == Variant.Type.String)
			{
				restValue = GetParseValue("Color", restValue.AsString());
			}
			propertiesBase = propertiesBox.add_color(godotObject, text2, propertiesShowName, value.AsColor(), restValue);
			break;
		case "Vector2":
			if (restValue.VariantType == Variant.Type.String)
			{
				restValue = GetParseValue("Vector2", restValue.AsString());
			}
			propertiesBase = propertiesBox.add_vector2(godotObject, text2, propertiesShowName, value.AsVector2(), restValue);
			break;
		case "Vector4i":
			propertiesBase = propertiesBox.add_vector4i(godotObject, text2, propertiesShowName, value.AsVector4I(), restValue);
			break;
		case "Enum":
		{
			Variant hint4 = properties["Hint"];
			propertiesBase = propertiesBox.add_enum(godotObject, text2, propertiesShowName, hint4, value, restValue);
			break;
		}
		case "Flag":
		{
			Variant hint3 = properties["Hint"];
			propertiesBase = propertiesBox.add_flag(godotObject, text2, propertiesShowName, hint3, value.AsInt32(), restValue);
			break;
		}
		case "File":
		{
			Variant hint2 = properties["Hint"];
			propertiesBase = propertiesBox.add_file_select(godotObject, text2, propertiesShowName, hint2, value, restValue);
			break;
		}
		case "Map":
			propertiesBase = propertiesBox.add_map_select(godotObject, text2, propertiesShowName, value, restValue);
			break;
		case "Room":
		{
			Variant hint = properties["Hint"];
			propertiesBase = propertiesBox.add_room_select(godotObject, text2, propertiesShowName, hint, value, restValue);
			break;
		}
		case "Array":
		{
			string text3 = "";
			if (properties.ContainsKey("Hint"))
			{
				text3 = properties["Hint"].AsString();
			}
			string hintType = properties["HintType"].AsString();
			propertiesBase = propertiesBox.add_array(godotObject, text2, propertiesShowName, text3, hintType, godotObject.GetIndexed(text2).AsGodotArray(), restValue);
			break;
		}
		case "ArrayVector2":
			propertiesBase = propertiesBox.add_array_vector2(godotObject, text2, propertiesShowName, value.AsGodotArray<Vector2>(), restValue);
			break;
		}
		if (propertiesBase != null)
		{
			propertiesBase.isTargetObj = isTargetObj;
			propertiesBase.inspector = this;
			propertiesBase.path = path;
			propertiesBase.showCloseButton = variant.AsBool();
		}
		return propertiesBase;
	}

	public static Variant GetRestValue(string type)
	{
		return type switch
		{
			"Bool" => (Variant)false, 
			"Int" => 0, 
			"Float" => 0.0, 
			"String" => "", 
			"Color" => Colors.White, 
			"Vector2" => Vector2.Zero, 
			"Array" => new Godot.Collections.Array(), 
			"ArrayVector2" => new Godot.Collections.Array(), 
			_ => default, 
		};
	}

	public static bool GetCanParseValue(string type)
	{
		switch (type)
		{
		case "Color":
		case "Float":
		case "Bool":
		case "Int":
		case "String":
		case "Vector2":
			return true;
		case "Array":
		case "ArrayVector2":
			return false;
		default:
			return false;
		}
	}

	public static Variant GetParseValue(string type, string str)
	{
		switch (type)
		{
		case "Bool":
			return str == "true";
		case "Int":
			return str.ToInt();
		case "Float":
			return str.ToFloat();
		case "String":
			return str;
		case "Color":
		{
			string[] array3 = str.Split(",", StringSplitOptions.RemoveEmptyEntries);
			float[] array4 = new float[array3.Length];
			for (int j = 0; j < array3.Length; j++)
			{
				array4[j] = array3[j].ToFloat();
			}
			return new Color(array4[0], array4[1], array4[2], array4[3]);
		}
		case "Vector2":
		{
			string[] array = str.Split(",", StringSplitOptions.RemoveEmptyEntries);
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i].ToFloat();
			}
			return new Vector2(array2[0], array2[1]);
		}
		default:
			return default;
		}
	}

	public static string GetTypeString(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		Variant.Type num = variantType - 1;
		if ((ulong)num <= 4uL)
		{
			switch ((int)num)
			{
			case 0:
				return "Bool";
			case 1:
				return "Int";
			case 2:
				return "Float";
			case 3:
				return "String";
			case 4:
				return "Vector2";
			}
		}
		switch (variantType)
		{
		case Variant.Type.Color:
			return "Color";
		case Variant.Type.Array:
		{
			Godot.Collections.Array array = value.AsGodotArray();
			if (array.Count > 0 && array[0].VariantType == Variant.Type.Vector2)
			{
				return "ArrayVector2";
			}
			return "Array";
		}
		default:
			return "";
		}
	}

	public void Clear()
	{
		nowObj = null;
		propertiesBox.Clear();
		nowCheckDictionary.Clear();
	}

	public void ChangeValue(StringName key, Variant newValue)
	{
		Dictionary dictionary = nowCheckDictionary[key].AsGodotDictionary();
		GodotObject godotObject = dictionary["Object"].AsGodotObject();
		string text = dictionary["Property"].AsString();
		PropertiesBase propertiesBase = propertiesBox._keys[key].As<PropertiesBase>();
		Variant indexed = godotObject.GetIndexed(text);
		if (!propertiesBase.isTargetObj)
		{
			indexed.AsGodotDictionary()[key] = newValue;
		}
		else
		{
			godotObject.SetIndexed(text, newValue);
		}
		if (dictionary.ContainsKey("Set"))
		{
			dictionary["Set"].AsCallable().Call();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(10)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "propertiesData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReFresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "propertiesData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddProperty, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "properties", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "propertiesShowName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetRestValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCanParseValue, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetParseValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "str", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTypeString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ChangeValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "newValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
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
		if (method == MethodName.Init && args.Count == 2)
		{
			Init(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReFresh && args.Count == 1)
		{
			ReFresh(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddProperty && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<PropertiesBase>(AddProperty(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.GetRestValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetRestValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCanParseValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(GetCanParseValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetParseValue && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetParseValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetTypeString && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetTypeString(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.ChangeValue && args.Count == 2)
		{
			ChangeValue(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetRestValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetRestValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCanParseValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(GetCanParseValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetParseValue && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetParseValue(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetTypeString && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetTypeString(VariantUtils.ConvertTo<Variant>(in args[0])));
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
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.ReFresh)
		{
			return true;
		}
		if (method == MethodName.AddProperty)
		{
			return true;
		}
		if (method == MethodName.GetRestValue)
		{
			return true;
		}
		if (method == MethodName.GetCanParseValue)
		{
			return true;
		}
		if (method == MethodName.GetParseValue)
		{
			return true;
		}
		if (method == MethodName.GetTypeString)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.ChangeValue)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.title)
		{
			title = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.titleLabel)
		{
			titleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName.propertiesBox)
		{
			propertiesBox = VariantUtils.ConvertTo<PropertiesBox>(in value);
			return true;
		}
		if (name == PropertyName.nowCheckDictionary)
		{
			nowCheckDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.nowObj)
		{
			nowObj = VariantUtils.ConvertTo<GodotObject>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.title)
		{
			value = VariantUtils.CreateFrom<string>(title);
			return true;
		}
		if (name == PropertyName.titleLabel)
		{
			value = VariantUtils.CreateFrom(in titleLabel);
			return true;
		}
		if (name == PropertyName.propertiesBox)
		{
			value = VariantUtils.CreateFrom(in propertiesBox);
			return true;
		}
		if (name == PropertyName.nowCheckDictionary)
		{
			value = VariantUtils.CreateFrom(in nowCheckDictionary);
			return true;
		}
		if (name == PropertyName.nowObj)
		{
			value = VariantUtils.CreateFrom(in nowObj);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.title, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.titleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.propertiesBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.nowCheckDictionary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.nowObj, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.title, Variant.From<string>(title));
		info.AddProperty(PropertyName.titleLabel, Variant.From(in titleLabel));
		info.AddProperty(PropertyName.propertiesBox, Variant.From(in propertiesBox));
		info.AddProperty(PropertyName.nowCheckDictionary, Variant.From(in nowCheckDictionary));
		info.AddProperty(PropertyName.nowObj, Variant.From(in nowObj));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.title, out var value))
		{
			title = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.titleLabel, out var value2))
		{
			titleLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName.propertiesBox, out var value3))
		{
			propertiesBox = value3.As<PropertiesBox>();
		}
		if (info.TryGetProperty(PropertyName.nowCheckDictionary, out var value4))
		{
			nowCheckDictionary = value4.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.nowObj, out var value5))
		{
			nowObj = value5.As<GodotObject>();
		}
	}
}
