using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/TowerDefenseLevelNewConfig.cs")]
public class TowerDefenseLevelNewConfig : TowerDefenseLevelBaseConfig
{
	private static class EnumCache<T> where T : struct, Enum
	{
		private static readonly System.Collections.Generic.Dictionary<string, T> _map = BuildMap();

		private static System.Collections.Generic.Dictionary<string, T> BuildMap()
		{
			System.Collections.Generic.Dictionary<string, T> dictionary = new System.Collections.Generic.Dictionary<string, T>(StringComparer.Ordinal);
			T[] values = Enum.GetValues<T>();
			for (int i = 0; i < values.Length; i++)
			{
				T value = values[i];
				dictionary[value.ToString().ToUpperInvariant()] = value;
			}
			return dictionary;
		}

		public static T Parse(string value, T defaultValue)
		{
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}
			if (!_map.TryGetValue(value.ToUpperInvariant(), out var value2))
			{
				return defaultValue;
			}
			return value2;
		}
	}

	public new class MethodName : TowerDefenseLevelBaseConfig.MethodName
	{
		public static readonly StringName HasProcessName = "HasProcessName";

		public static readonly StringName GetDict = "GetDict";

		public static readonly StringName GetArray = "GetArray";

		public new static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : TowerDefenseLevelBaseConfig.PropertyName
	{
		public static readonly StringName data = "data";

		public static readonly StringName _data = "_data";

		public static readonly StringName version = "version";

		public static readonly StringName featureData = "featureData";

		public static readonly StringName processName = "processName";

		public static readonly StringName processData = "processData";
	}

	public new class SignalName : TowerDefenseLevelBaseConfig.SignalName
	{
	}

	private Json _data;

	[Export(PropertyHint.None, "")]
	public StringName version = "1.0";

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<StringName, Dictionary> featureData = new Godot.Collections.Dictionary<StringName, Dictionary>();

	[Export(PropertyHint.None, "")]
	public StringName processName;

	[Export(PropertyHint.None, "")]
	public Dictionary processData = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Json data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			Init();
		}
	}

	private static bool HasProcessName(StringName value)
	{
		if (value != null)
		{
			return !value.IsEmpty;
		}
		return false;
	}

	private static Dictionary GetDict(Dictionary src, string key)
	{
		if (src != null && src.TryGetValue(key, out var value) && value.VariantType == Variant.Type.Dictionary)
		{
			return value.AsGodotDictionary();
		}
		return new Dictionary();
	}

	private static Godot.Collections.Array GetArray(Dictionary src, string key)
	{
		if (src != null && src.TryGetValue(key, out var value) && value.VariantType == Variant.Type.Array)
		{
			return (Godot.Collections.Array)value;
		}
		return new Godot.Collections.Array();
	}

	public override void Init()
	{
		if (!GodotObject.IsInstanceValid(data))
		{
			return;
		}
		Dictionary dictionary = data.Data.AsGodotDictionary();
		name = dictionary.GetValueOrDefault("Name", "").AsString();
		levelName = dictionary.GetValueOrDefault("LevelName", "").AsString();
		description = dictionary.GetValueOrDefault("Description", "").AsString();
		levelNumber = dictionary.GetValueOrDefault("LevelNumber", 0).AsInt32();
		nextLevel = dictionary.GetValueOrDefault("NextLevel", "").AsString();
		homeWorld = EnumCache<GeneralEnum.HOMEWORLD>.Parse(dictionary.GetValueOrDefault("HomeWorld", "NOONE").AsString(), GeneralEnum.HOMEWORLD.NOONE);
		version = dictionary.GetValueOrDefault("Version", "1.0").AsString();
		Dictionary dict = GetDict(dictionary, "Process");
		if (dict.Count > 0)
		{
			processName = dict.GetValueOrDefault("Name", "").AsString();
			processData = GetDict(dict, "Data");
		}
		else
		{
			processName = "";
			processData = new Dictionary();
		}
		featureData.Clear();
		foreach (Variant item in GetArray(dictionary, "Feature"))
		{
			Dictionary dictionary2 = item.AsGodotDictionary();
			StringName key = new StringName(dictionary2.GetValueOrDefault("Name", "").AsString());
			Dictionary dict2 = GetDict(dictionary2, "Data");
			featureData[key] = dict2;
		}
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["Name"] = name,
			["LevelName"] = levelName,
			["Description"] = description,
			["LevelNumber"] = levelNumber,
			["NextLevel"] = nextLevel,
			["HomeWorld"] = Enum.GetName(typeof(GeneralEnum.HOMEWORLD), homeWorld),
			["Version"] = version,
			["Feature"] = new Godot.Collections.Array(),
			["Process"] = new Dictionary()
		};
		foreach (StringName key10 in featureData.Keys)
		{
			((Godot.Collections.Array)dictionary["Feature"]).Add(new Dictionary
			{
				["Name"] = key10,
				["Data"] = featureData[key10]
			});
		}
		if (HasProcessName(processName))
		{
			dictionary["Process"] = new Dictionary
			{
				["Name"] = processName,
				["Data"] = processData
			};
		}
		return dictionary;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.HasProcessName, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDict, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "src", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "src", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasProcessName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProcessName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetDict && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetDict(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetArray && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetArray(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasProcessName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProcessName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetDict && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetDict(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetArray && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetArray(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.HasProcessName)
		{
			return true;
		}
		if (method == MethodName.GetDict)
		{
			return true;
		}
		if (method == MethodName.GetArray)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.data)
		{
			data = VariantUtils.ConvertTo<Json>(in value);
			return true;
		}
		if (name == PropertyName._data)
		{
			_data = VariantUtils.ConvertTo<Json>(in value);
			return true;
		}
		if (name == PropertyName.version)
		{
			version = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.featureData)
		{
			featureData = VariantUtils.ConvertToDictionary<StringName, Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.processName)
		{
			processName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.processData)
		{
			processData = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.data)
		{
			value = VariantUtils.CreateFrom<Json>(data);
			return true;
		}
		if (name == PropertyName._data)
		{
			value = VariantUtils.CreateFrom(in _data);
			return true;
		}
		if (name == PropertyName.version)
		{
			value = VariantUtils.CreateFrom(in version);
			return true;
		}
		if (name == PropertyName.featureData)
		{
			value = VariantUtils.CreateFromDictionary(featureData);
			return true;
		}
		if (name == PropertyName.processName)
		{
			value = VariantUtils.CreateFrom(in processName);
			return true;
		}
		if (name == PropertyName.processData)
		{
			value = VariantUtils.CreateFrom(in processData);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.data, PropertyHint.ResourceType, "JSON", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.version, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.featureData, PropertyHint.TypeString, "21/0:;27/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.processName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.processData, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.data, Variant.From<Json>(data));
		info.AddProperty(PropertyName._data, Variant.From(in _data));
		info.AddProperty(PropertyName.version, Variant.From(in version));
		info.AddProperty(PropertyName.featureData, Variant.CreateFrom(featureData));
		info.AddProperty(PropertyName.processName, Variant.From(in processName));
		info.AddProperty(PropertyName.processData, Variant.From(in processData));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.data, out var value))
		{
			data = value.As<Json>();
		}
		if (info.TryGetProperty(PropertyName._data, out var value2))
		{
			_data = value2.As<Json>();
		}
		if (info.TryGetProperty(PropertyName.version, out var value3))
		{
			version = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.featureData, out var value4))
		{
			featureData = value4.AsGodotDictionary<StringName, Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.processName, out var value5))
		{
			processName = value5.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.processData, out var value6))
		{
			processData = value6.As<Dictionary>();
		}
	}
}
