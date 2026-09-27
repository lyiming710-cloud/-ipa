using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Progress/Resource/TowerDefenseBattleFeatureProgressConfig.cs")]
public class TowerDefenseBattleFeatureProgressConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public static readonly StringName ParseMode = "ParseMode";

		public static readonly StringName ReadString = "ReadString";

		public static readonly StringName ReadVisibility = "ReadVisibility";

		public static readonly StringName AddText = "AddText";

		public static readonly StringName AddVisibility = "AddVisibility";

		public static readonly StringName AddVisibilityMode = "AddVisibilityMode";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName HasProgressText = "HasProgressText";

		public static readonly StringName HasHideProgressItems = "HasHideProgressItems";

		public static readonly StringName HideProgressItems = "HideProgressItems";

		public static readonly StringName mode = "mode";

		public static readonly StringName levelName = "levelName";

		public static readonly StringName difficultyText = "difficultyText";

		public static readonly StringName survivalText = "survivalText";

		public static readonly StringName progressText = "progressText";

		public static readonly StringName progressValue = "progressValue";

		public static readonly StringName progressMax = "progressMax";

		public static readonly StringName autoProgressResponse = "autoProgressResponse";

		public static readonly StringName progressItemsVisibility = "progressItemsVisibility";

		public static readonly StringName difficultyVisibility = "difficultyVisibility";

		public static readonly StringName levelNameVisibility = "levelNameVisibility";

		public static readonly StringName survivalVisibility = "survivalVisibility";

		public static readonly StringName progressVisibility = "progressVisibility";

		public static readonly StringName progressTextVisibility = "progressTextVisibility";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public const string DefaultProgressText = "{value}/{max}";

	[Export(PropertyHint.None, "")]
	public TowerDefenseProgressMode mode;

	[Export(PropertyHint.None, "")]
	public string levelName = "";

	[Export(PropertyHint.None, "")]
	public string difficultyText = "";

	[Export(PropertyHint.None, "")]
	public string survivalText = "";

	[Export(PropertyHint.None, "")]
	public string progressText = "{value}/{max}";

	[Export(PropertyHint.None, "")]
	public double progressValue;

	[Export(PropertyHint.None, "")]
	public double progressMax = 1.0;

	[Export(PropertyHint.None, "")]
	public double autoProgressResponse = -1.0;

	[Export(PropertyHint.None, "")]
	public TowerDefenseProgressVisibility progressItemsVisibility;

	[Export(PropertyHint.None, "")]
	public TowerDefenseProgressVisibility difficultyVisibility;

	[Export(PropertyHint.None, "")]
	public TowerDefenseProgressVisibility levelNameVisibility;

	[Export(PropertyHint.None, "")]
	public TowerDefenseProgressVisibility survivalVisibility;

	[Export(PropertyHint.None, "")]
	public TowerDefenseProgressVisibility progressVisibility;

	[Export(PropertyHint.None, "")]
	public TowerDefenseProgressVisibility progressTextVisibility;

	public bool HasProgressText { get; private set; }

	public bool HasHideProgressItems
	{
		get
		{
			if (progressItemsVisibility == TowerDefenseProgressVisibility.Auto)
			{
				return mode == TowerDefenseProgressMode.Manual;
			}
			return true;
		}
	}

	public bool HideProgressItems => progressItemsVisibility switch
	{
		TowerDefenseProgressVisibility.Show => false, 
		TowerDefenseProgressVisibility.Hide => true, 
		_ => mode == TowerDefenseProgressMode.Manual, 
	};

	public void Init(Dictionary data)
	{
		if (data == null)
		{
			data = new Dictionary();
		}
		mode = ParseMode(data);
		levelName = ReadString(data, "LevelName");
		difficultyText = ReadString(data, "DifficultyText", "DifficultText");
		survivalText = ReadString(data, "SurvivalText");
		HasProgressText = data.ContainsKey("ProgressText");
		progressText = data.GetValueOrDefault("ProgressText", "{value}/{max}").AsString();
		progressValue = data.GetValueOrDefault("ProgressValue", 0.0).AsDouble();
		double num = data.GetValueOrDefault("ProgressMax", 1.0).AsDouble();
		progressMax = ((num > 0.0) ? num : 1.0);
		autoProgressResponse = data.GetValueOrDefault("AutoProgressResponse", -1.0).AsDouble();
		progressItemsVisibility = ReadVisibility(data, "ProgressItemsVisibility", true, "HideProgressItems");
		difficultyVisibility = ReadVisibility(data, "DifficultyVisibility", false, "ShowDifficulty", "ShowDifficult");
		levelNameVisibility = ReadVisibility(data, "LevelNameVisibility", false, "ShowLevelName");
		survivalVisibility = ReadVisibility(data, "SurvivalVisibility", false, "ShowSurvival");
		progressVisibility = ReadVisibility(data, "ProgressVisibility", false, "ShowProgress");
		progressTextVisibility = ReadVisibility(data, "ProgressTextVisibility", false, "ShowProgressText");
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["Mode"] = mode.ToString(),
			["ManualProgress"] = mode == TowerDefenseProgressMode.Manual,
			["ProgressValue"] = progressValue,
			["ProgressMax"] = ((progressMax > 0.0) ? progressMax : 1.0)
		};
		if (autoProgressResponse >= 0.0)
		{
			dictionary["AutoProgressResponse"] = autoProgressResponse;
		}
		AddText(dictionary, "LevelName", levelName);
		if (!string.IsNullOrEmpty(difficultyText))
		{
			dictionary["DifficultyText"] = difficultyText;
			dictionary["DifficultText"] = difficultyText;
		}
		AddText(dictionary, "SurvivalText", survivalText);
		if (HasProgressText || progressText != "{value}/{max}" || progressTextVisibility != TowerDefenseProgressVisibility.Auto)
		{
			dictionary["ProgressText"] = progressText ?? "";
		}
		AddVisibility(dictionary, "HideProgressItems", progressItemsVisibility, invert: true);
		AddVisibility(dictionary, "ShowDifficulty", difficultyVisibility, invert: false);
		AddVisibility(dictionary, "ShowDifficult", difficultyVisibility, invert: false);
		AddVisibility(dictionary, "ShowLevelName", levelNameVisibility, invert: false);
		AddVisibility(dictionary, "ShowSurvival", survivalVisibility, invert: false);
		AddVisibility(dictionary, "ShowProgress", progressVisibility, invert: false);
		AddVisibility(dictionary, "ShowProgressText", progressTextVisibility, invert: false);
		AddVisibilityMode(dictionary, "ProgressItemsVisibility", progressItemsVisibility);
		AddVisibilityMode(dictionary, "DifficultyVisibility", difficultyVisibility);
		AddVisibilityMode(dictionary, "LevelNameVisibility", levelNameVisibility);
		AddVisibilityMode(dictionary, "SurvivalVisibility", survivalVisibility);
		AddVisibilityMode(dictionary, "ProgressVisibility", progressVisibility);
		AddVisibilityMode(dictionary, "ProgressTextVisibility", progressTextVisibility);
		return dictionary;
	}

	public static bool TryGetVisibility(TowerDefenseProgressVisibility visibility, out bool visible)
	{
		visible = visibility == TowerDefenseProgressVisibility.Show;
		return visibility != TowerDefenseProgressVisibility.Auto;
	}

	private static TowerDefenseProgressMode ParseMode(Dictionary data)
	{
		if (data.TryGetValue("Mode", out var value) && Enum.TryParse<TowerDefenseProgressMode>(value.AsString(), ignoreCase: true, out var result))
		{
			return result;
		}
		if (!data.GetValueOrDefault("ManualProgress", false).AsBool())
		{
			return TowerDefenseProgressMode.Auto;
		}
		return TowerDefenseProgressMode.Manual;
	}

	private static string ReadString(Dictionary data, params string[] keys)
	{
		foreach (string text in keys)
		{
			if (data.TryGetValue(text, out var value))
			{
				return value.AsString();
			}
		}
		return "";
	}

	private static TowerDefenseProgressVisibility ReadVisibility(Dictionary data, string modeKey, bool invert, params string[] keys)
	{
		if (data.TryGetValue(modeKey, out var value) && Enum.TryParse<TowerDefenseProgressVisibility>(value.AsString(), ignoreCase: true, out var result))
		{
			return result;
		}
		foreach (string text in keys)
		{
			if (data.TryGetValue(text, out var value2))
			{
				bool flag = value2.AsBool();
				if (invert)
				{
					flag = !flag;
				}
				if (!flag)
				{
					return TowerDefenseProgressVisibility.Hide;
				}
				return TowerDefenseProgressVisibility.Show;
			}
		}
		return TowerDefenseProgressVisibility.Auto;
	}

	private static void AddText(Dictionary data, string key, string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			data[key] = value;
		}
	}

	private static void AddVisibility(Dictionary data, string key, TowerDefenseProgressVisibility visibility, bool invert)
	{
		if (TryGetVisibility(visibility, out var visible))
		{
			data[key] = (invert ? (!visible) : visible);
		}
	}

	private static void AddVisibilityMode(Dictionary data, string key, TowerDefenseProgressVisibility visibility)
	{
		if (visibility != TowerDefenseProgressVisibility.Auto)
		{
			data[key] = visibility.ToString();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ParseMode, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "keys", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadVisibility, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "modeKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "invert", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "keys", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "visibility", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "invert", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddVisibilityMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "visibility", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.ParseMode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProgressMode>(ParseMode(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadString && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadString(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadVisibility && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProgressVisibility>(ReadVisibility(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<string[]>(in args[3])));
			return true;
		}
		if (method == MethodName.AddText && args.Count == 3)
		{
			AddText(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddVisibility && args.Count == 4)
		{
			AddVisibility(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProgressVisibility>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddVisibilityMode && args.Count == 3)
		{
			AddVisibilityMode(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProgressVisibility>(in args[2]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ParseMode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProgressMode>(ParseMode(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadString && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadString(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadVisibility && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProgressVisibility>(ReadVisibility(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<string[]>(in args[3])));
			return true;
		}
		if (method == MethodName.AddText && args.Count == 3)
		{
			AddText(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddVisibility && args.Count == 4)
		{
			AddVisibility(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProgressVisibility>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddVisibilityMode && args.Count == 3)
		{
			AddVisibilityMode(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProgressVisibility>(in args[2]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.ParseMode)
		{
			return true;
		}
		if (method == MethodName.ReadString)
		{
			return true;
		}
		if (method == MethodName.ReadVisibility)
		{
			return true;
		}
		if (method == MethodName.AddText)
		{
			return true;
		}
		if (method == MethodName.AddVisibility)
		{
			return true;
		}
		if (method == MethodName.AddVisibilityMode)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.HasProgressText)
		{
			HasProgressText = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mode)
		{
			mode = VariantUtils.ConvertTo<TowerDefenseProgressMode>(in value);
			return true;
		}
		if (name == PropertyName.levelName)
		{
			levelName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.difficultyText)
		{
			difficultyText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.survivalText)
		{
			survivalText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.progressText)
		{
			progressText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.progressValue)
		{
			progressValue = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.progressMax)
		{
			progressMax = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.autoProgressResponse)
		{
			autoProgressResponse = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.progressItemsVisibility)
		{
			progressItemsVisibility = VariantUtils.ConvertTo<TowerDefenseProgressVisibility>(in value);
			return true;
		}
		if (name == PropertyName.difficultyVisibility)
		{
			difficultyVisibility = VariantUtils.ConvertTo<TowerDefenseProgressVisibility>(in value);
			return true;
		}
		if (name == PropertyName.levelNameVisibility)
		{
			levelNameVisibility = VariantUtils.ConvertTo<TowerDefenseProgressVisibility>(in value);
			return true;
		}
		if (name == PropertyName.survivalVisibility)
		{
			survivalVisibility = VariantUtils.ConvertTo<TowerDefenseProgressVisibility>(in value);
			return true;
		}
		if (name == PropertyName.progressVisibility)
		{
			progressVisibility = VariantUtils.ConvertTo<TowerDefenseProgressVisibility>(in value);
			return true;
		}
		if (name == PropertyName.progressTextVisibility)
		{
			progressTextVisibility = VariantUtils.ConvertTo<TowerDefenseProgressVisibility>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.HasProgressText)
		{
			from = HasProgressText;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasHideProgressItems)
		{
			from = HasHideProgressItems;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HideProgressItems)
		{
			from = HideProgressItems;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.mode)
		{
			value = VariantUtils.CreateFrom(in mode);
			return true;
		}
		if (name == PropertyName.levelName)
		{
			value = VariantUtils.CreateFrom(in levelName);
			return true;
		}
		if (name == PropertyName.difficultyText)
		{
			value = VariantUtils.CreateFrom(in difficultyText);
			return true;
		}
		if (name == PropertyName.survivalText)
		{
			value = VariantUtils.CreateFrom(in survivalText);
			return true;
		}
		if (name == PropertyName.progressText)
		{
			value = VariantUtils.CreateFrom(in progressText);
			return true;
		}
		if (name == PropertyName.progressValue)
		{
			value = VariantUtils.CreateFrom(in progressValue);
			return true;
		}
		if (name == PropertyName.progressMax)
		{
			value = VariantUtils.CreateFrom(in progressMax);
			return true;
		}
		if (name == PropertyName.autoProgressResponse)
		{
			value = VariantUtils.CreateFrom(in autoProgressResponse);
			return true;
		}
		if (name == PropertyName.progressItemsVisibility)
		{
			value = VariantUtils.CreateFrom(in progressItemsVisibility);
			return true;
		}
		if (name == PropertyName.difficultyVisibility)
		{
			value = VariantUtils.CreateFrom(in difficultyVisibility);
			return true;
		}
		if (name == PropertyName.levelNameVisibility)
		{
			value = VariantUtils.CreateFrom(in levelNameVisibility);
			return true;
		}
		if (name == PropertyName.survivalVisibility)
		{
			value = VariantUtils.CreateFrom(in survivalVisibility);
			return true;
		}
		if (name == PropertyName.progressVisibility)
		{
			value = VariantUtils.CreateFrom(in progressVisibility);
			return true;
		}
		if (name == PropertyName.progressTextVisibility)
		{
			value = VariantUtils.CreateFrom(in progressTextVisibility);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.mode, PropertyHint.Enum, "Auto,Manual", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.levelName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.difficultyText, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.survivalText, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.progressText, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.progressValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.progressMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.autoProgressResponse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.progressItemsVisibility, PropertyHint.Enum, "Auto,Show,Hide", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.difficultyVisibility, PropertyHint.Enum, "Auto,Show,Hide", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.levelNameVisibility, PropertyHint.Enum, "Auto,Show,Hide", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.survivalVisibility, PropertyHint.Enum, "Auto,Show,Hide", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.progressVisibility, PropertyHint.Enum, "Auto,Show,Hide", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.progressTextVisibility, PropertyHint.Enum, "Auto,Show,Hide", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasProgressText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasHideProgressItems, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HideProgressItems, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.HasProgressText, Variant.From<bool>(HasProgressText));
		info.AddProperty(PropertyName.mode, Variant.From(in mode));
		info.AddProperty(PropertyName.levelName, Variant.From(in levelName));
		info.AddProperty(PropertyName.difficultyText, Variant.From(in difficultyText));
		info.AddProperty(PropertyName.survivalText, Variant.From(in survivalText));
		info.AddProperty(PropertyName.progressText, Variant.From(in progressText));
		info.AddProperty(PropertyName.progressValue, Variant.From(in progressValue));
		info.AddProperty(PropertyName.progressMax, Variant.From(in progressMax));
		info.AddProperty(PropertyName.autoProgressResponse, Variant.From(in autoProgressResponse));
		info.AddProperty(PropertyName.progressItemsVisibility, Variant.From(in progressItemsVisibility));
		info.AddProperty(PropertyName.difficultyVisibility, Variant.From(in difficultyVisibility));
		info.AddProperty(PropertyName.levelNameVisibility, Variant.From(in levelNameVisibility));
		info.AddProperty(PropertyName.survivalVisibility, Variant.From(in survivalVisibility));
		info.AddProperty(PropertyName.progressVisibility, Variant.From(in progressVisibility));
		info.AddProperty(PropertyName.progressTextVisibility, Variant.From(in progressTextVisibility));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.HasProgressText, out var value))
		{
			HasProgressText = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mode, out var value2))
		{
			mode = value2.As<TowerDefenseProgressMode>();
		}
		if (info.TryGetProperty(PropertyName.levelName, out var value3))
		{
			levelName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.difficultyText, out var value4))
		{
			difficultyText = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.survivalText, out var value5))
		{
			survivalText = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.progressText, out var value6))
		{
			progressText = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.progressValue, out var value7))
		{
			progressValue = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.progressMax, out var value8))
		{
			progressMax = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.autoProgressResponse, out var value9))
		{
			autoProgressResponse = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.progressItemsVisibility, out var value10))
		{
			progressItemsVisibility = value10.As<TowerDefenseProgressVisibility>();
		}
		if (info.TryGetProperty(PropertyName.difficultyVisibility, out var value11))
		{
			difficultyVisibility = value11.As<TowerDefenseProgressVisibility>();
		}
		if (info.TryGetProperty(PropertyName.levelNameVisibility, out var value12))
		{
			levelNameVisibility = value12.As<TowerDefenseProgressVisibility>();
		}
		if (info.TryGetProperty(PropertyName.survivalVisibility, out var value13))
		{
			survivalVisibility = value13.As<TowerDefenseProgressVisibility>();
		}
		if (info.TryGetProperty(PropertyName.progressVisibility, out var value14))
		{
			progressVisibility = value14.As<TowerDefenseProgressVisibility>();
		}
		if (info.TryGetProperty(PropertyName.progressTextVisibility, out var value15))
		{
			progressTextVisibility = value15.As<TowerDefenseProgressVisibility>();
		}
	}
}
