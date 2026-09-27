using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Wave/Resource/TowerDefenseLevelSurvivalConfig.cs")]
public class TowerDefenseLevelSurvivalConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Load = "Load";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName json = "json";

		public static readonly StringName _json = "_json";

		public static readonly StringName roundLimit = "roundLimit";

		public static readonly StringName roundDayNightChange = "roundDayNightChange";

		public static readonly StringName pointIncrementPerWave = "pointIncrementPerWave";

		public static readonly StringName pointIncrementPerBigWave = "pointIncrementPerBigWave";

		public static readonly StringName pointIncrementPerRound = "pointIncrementPerRound";

		public static readonly StringName pointBegin = "pointBegin";

		public static readonly StringName pointMax = "pointMax";

		public static readonly StringName pointBigWaveScale = "pointBigWaveScale";

		public static readonly StringName zombiePoolBase = "zombiePoolBase";

		public static readonly StringName zombiePoolRoundAdd = "zombiePoolRoundAdd";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private Json _json;

	[Export(PropertyHint.None, "")]
	public int roundLimit = -1;

	[Export(PropertyHint.None, "")]
	public bool roundDayNightChange;

	[Export(PropertyHint.None, "")]
	public int pointIncrementPerWave = 50;

	[Export(PropertyHint.None, "")]
	public int pointIncrementPerBigWave = 100;

	[Export(PropertyHint.None, "")]
	public int pointIncrementPerRound = 200;

	[Export(PropertyHint.None, "")]
	public int pointBegin = 100;

	[Export(PropertyHint.None, "")]
	public int pointMax = 10000000;

	[Export(PropertyHint.None, "")]
	public double pointBigWaveScale = 1.5;

	[Export(PropertyHint.None, "")]
	public Array zombiePoolBase = new Array { "ZombieNormal", "ZombieNormalCone" };

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelSurvivalZombiePoolRoundAddConfig> zombiePoolRoundAdd = new Array<TowerDefenseLevelSurvivalZombiePoolRoundAddConfig>();

	[Export(PropertyHint.None, "")]
	public Json json
	{
		get
		{
			return _json;
		}
		set
		{
			_json = value;
			if (GodotObject.IsInstanceValid(_json))
			{
				Init();
				NotifyPropertyListChanged();
			}
		}
	}

	public void Init()
	{
		if (GodotObject.IsInstanceValid(json) && json.Data.VariantType == Variant.Type.Dictionary)
		{
			Dictionary data = json.Data.AsGodotDictionary();
			Load(data);
		}
	}

	public void Load(Dictionary data)
	{
		roundLimit = data.GetValueOrDefault("RoundLimit", -1).AsInt32();
		roundDayNightChange = data.GetValueOrDefault("RoundDayNightChange", false).AsBool();
		pointIncrementPerWave = data.GetValueOrDefault("PointIncrementPerWave", 50).AsInt32();
		pointIncrementPerBigWave = data.GetValueOrDefault("PointIncrementPerBigWave", 100).AsInt32();
		pointIncrementPerRound = data.GetValueOrDefault("PointIncrementPerRound", 200).AsInt32();
		pointBegin = data.GetValueOrDefault("Pointregin", 100).AsInt32();
		pointMax = data.GetValueOrDefault("PointMax", 10000000).AsInt32();
		pointBigWaveScale = data.GetValueOrDefault("PointBigWaveScale", 1.5).AsDouble();
		Dictionary dictionary = data.GetValueOrDefault("ZombiePoolSetting", new Dictionary()).AsGodotDictionary();
		zombiePoolBase = (dictionary.ContainsKey("ZombiePoolBase") ? ((Array)dictionary["ZombiePoolBase"]) : new Array());
		zombiePoolRoundAdd.Clear();
		foreach (Variant item in dictionary.ContainsKey("ZombiePoolRoundAdd") ? ((Array)dictionary["ZombiePoolRoundAdd"]) : new Array())
		{
			Dictionary data2 = item.AsGodotDictionary();
			TowerDefenseLevelSurvivalZombiePoolRoundAddConfig towerDefenseLevelSurvivalZombiePoolRoundAddConfig = new TowerDefenseLevelSurvivalZombiePoolRoundAddConfig();
			towerDefenseLevelSurvivalZombiePoolRoundAddConfig.Init(data2);
			zombiePoolRoundAdd.Add(towerDefenseLevelSurvivalZombiePoolRoundAddConfig);
		}
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["PointIncrementPerWave"] = pointIncrementPerWave,
			["PointIncrementPerBigWave"] = pointIncrementPerBigWave,
			["PointIncrementPerRound"] = pointIncrementPerRound,
			["Pointregin"] = pointBegin,
			["PointMax"] = pointMax,
			["PointBigWaveScale"] = pointBigWaveScale,
			["ZombiePoolSetting"] = new Dictionary
			{
				["ZombiePoolBase"] = zombiePoolBase,
				["ZombiePoolRoundAdd"] = new Array()
			}
		};
		foreach (TowerDefenseLevelSurvivalZombiePoolRoundAddConfig item in zombiePoolRoundAdd)
		{
			((Array)((Dictionary)dictionary["ZombiePoolSetting"])["ZombiePoolRoundAdd"]).Add(item.Export());
		}
		return dictionary;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Load, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.Load && args.Count == 1)
		{
			Load(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Load)
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
		if (name == PropertyName.json)
		{
			json = VariantUtils.ConvertTo<Json>(in value);
			return true;
		}
		if (name == PropertyName._json)
		{
			_json = VariantUtils.ConvertTo<Json>(in value);
			return true;
		}
		if (name == PropertyName.roundLimit)
		{
			roundLimit = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.roundDayNightChange)
		{
			roundDayNightChange = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.pointIncrementPerWave)
		{
			pointIncrementPerWave = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.pointIncrementPerBigWave)
		{
			pointIncrementPerBigWave = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.pointIncrementPerRound)
		{
			pointIncrementPerRound = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.pointBegin)
		{
			pointBegin = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.pointMax)
		{
			pointMax = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.pointBigWaveScale)
		{
			pointBigWaveScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.zombiePoolBase)
		{
			zombiePoolBase = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.zombiePoolRoundAdd)
		{
			zombiePoolRoundAdd = VariantUtils.ConvertToArray<TowerDefenseLevelSurvivalZombiePoolRoundAddConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.json)
		{
			value = VariantUtils.CreateFrom<Json>(json);
			return true;
		}
		if (name == PropertyName._json)
		{
			value = VariantUtils.CreateFrom(in _json);
			return true;
		}
		if (name == PropertyName.roundLimit)
		{
			value = VariantUtils.CreateFrom(in roundLimit);
			return true;
		}
		if (name == PropertyName.roundDayNightChange)
		{
			value = VariantUtils.CreateFrom(in roundDayNightChange);
			return true;
		}
		if (name == PropertyName.pointIncrementPerWave)
		{
			value = VariantUtils.CreateFrom(in pointIncrementPerWave);
			return true;
		}
		if (name == PropertyName.pointIncrementPerBigWave)
		{
			value = VariantUtils.CreateFrom(in pointIncrementPerBigWave);
			return true;
		}
		if (name == PropertyName.pointIncrementPerRound)
		{
			value = VariantUtils.CreateFrom(in pointIncrementPerRound);
			return true;
		}
		if (name == PropertyName.pointBegin)
		{
			value = VariantUtils.CreateFrom(in pointBegin);
			return true;
		}
		if (name == PropertyName.pointMax)
		{
			value = VariantUtils.CreateFrom(in pointMax);
			return true;
		}
		if (name == PropertyName.pointBigWaveScale)
		{
			value = VariantUtils.CreateFrom(in pointBigWaveScale);
			return true;
		}
		if (name == PropertyName.zombiePoolBase)
		{
			value = VariantUtils.CreateFrom(in zombiePoolBase);
			return true;
		}
		if (name == PropertyName.zombiePoolRoundAdd)
		{
			value = VariantUtils.CreateFromArray(zombiePoolRoundAdd);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.json, PropertyHint.ResourceType, "JSON", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._json, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.roundLimit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.roundDayNightChange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.pointIncrementPerWave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.pointIncrementPerBigWave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.pointIncrementPerRound, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.pointBegin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.pointMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.pointBigWaveScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.zombiePoolBase, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.zombiePoolRoundAdd, PropertyHint.TypeString, "24/17:TowerDefenseLevelSurvivalZombiePoolRoundAddConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.json, Variant.From<Json>(json));
		info.AddProperty(PropertyName._json, Variant.From(in _json));
		info.AddProperty(PropertyName.roundLimit, Variant.From(in roundLimit));
		info.AddProperty(PropertyName.roundDayNightChange, Variant.From(in roundDayNightChange));
		info.AddProperty(PropertyName.pointIncrementPerWave, Variant.From(in pointIncrementPerWave));
		info.AddProperty(PropertyName.pointIncrementPerBigWave, Variant.From(in pointIncrementPerBigWave));
		info.AddProperty(PropertyName.pointIncrementPerRound, Variant.From(in pointIncrementPerRound));
		info.AddProperty(PropertyName.pointBegin, Variant.From(in pointBegin));
		info.AddProperty(PropertyName.pointMax, Variant.From(in pointMax));
		info.AddProperty(PropertyName.pointBigWaveScale, Variant.From(in pointBigWaveScale));
		info.AddProperty(PropertyName.zombiePoolBase, Variant.From(in zombiePoolBase));
		info.AddProperty(PropertyName.zombiePoolRoundAdd, Variant.CreateFrom(zombiePoolRoundAdd));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.json, out var value))
		{
			json = value.As<Json>();
		}
		if (info.TryGetProperty(PropertyName._json, out var value2))
		{
			_json = value2.As<Json>();
		}
		if (info.TryGetProperty(PropertyName.roundLimit, out var value3))
		{
			roundLimit = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.roundDayNightChange, out var value4))
		{
			roundDayNightChange = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.pointIncrementPerWave, out var value5))
		{
			pointIncrementPerWave = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.pointIncrementPerBigWave, out var value6))
		{
			pointIncrementPerBigWave = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.pointIncrementPerRound, out var value7))
		{
			pointIncrementPerRound = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.pointBegin, out var value8))
		{
			pointBegin = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.pointMax, out var value9))
		{
			pointMax = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.pointBigWaveScale, out var value10))
		{
			pointBigWaveScale = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.zombiePoolBase, out var value11))
		{
			zombiePoolBase = value11.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.zombiePoolRoundAdd, out var value12))
		{
			zombiePoolRoundAdd = value12.AsGodotArray<TowerDefenseLevelSurvivalZombiePoolRoundAddConfig>();
		}
	}
}
