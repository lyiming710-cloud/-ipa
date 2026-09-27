using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/Wave/TowerDefenseLevelWaveManagerConfig.cs")]
public class TowerDefenseLevelWaveManagerConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public static readonly StringName WaveDynamicPlantfoodFill = "WaveDynamicPlantfoodFill";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName dynamic = "dynamic";

		public static readonly StringName wave = "wave";

		public static readonly StringName zombieInvisible = "zombieInvisible";

		public static readonly StringName flagZombieUse = "flagZombieUse";

		public static readonly StringName flagZombie = "flagZombie";

		public static readonly StringName flagWaveInterval = "flagWaveInterval";

		public static readonly StringName maxNextWaveHealthPercentage = "maxNextWaveHealthPercentage";

		public static readonly StringName minNextWaveHealthPercentage = "minNextWaveHealthPercentage";

		public static readonly StringName beginCol = "beginCol";

		public static readonly StringName spawnColEnd = "spawnColEnd";

		public static readonly StringName spawnColStart = "spawnColStart";

		public static readonly StringName spawnFrameBudgetMilliseconds = "spawnFrameBudgetMilliseconds";

		public static readonly StringName spawnMaxCharactersPerFrame = "spawnMaxCharactersPerFrame";

		public static readonly StringName spawnOverride = "spawnOverride";

		public static readonly StringName _dynamic = "_dynamic";

		public static readonly StringName _wave = "_wave";

		public static readonly StringName isCustomSurvival = "isCustomSurvival";

		public static readonly StringName survival = "survival";

		public static readonly StringName customSurvival = "customSurvival";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public const double MaximumSpawnFrameBudgetMilliseconds = 16.0;

	[Export(PropertyHint.None, "")]
	public bool zombieInvisible;

	[Export(PropertyHint.None, "")]
	public bool flagZombieUse = true;

	[Export(PropertyHint.None, "")]
	public string flagZombie = "ZombieFlag";

	[Export(PropertyHint.None, "")]
	public int flagWaveInterval = 10;

	[Export(PropertyHint.None, "")]
	public double maxNextWaveHealthPercentage = 0.15;

	[Export(PropertyHint.None, "")]
	public double minNextWaveHealthPercentage = 0.2;

	[Export(PropertyHint.None, "")]
	public double beginCol = 20.0;

	[Export(PropertyHint.None, "")]
	public double spawnColEnd = 20.0;

	[Export(PropertyHint.None, "")]
	public double spawnColStart = 5.0;

	[Export(PropertyHint.Range, "0.25,16,0.25")]
	public double spawnFrameBudgetMilliseconds = 6.0;

	[Export(PropertyHint.Range, "1,64,1,or_greater")]
	public int spawnMaxCharactersPerFrame = 8;

	[Export(PropertyHint.None, "")]
	public TowerDefenseCharacterOverride spawnOverride;

	private Array<TowerDefenseLevelDynamicConfig> _dynamic = new Array<TowerDefenseLevelDynamicConfig> { null, null, null, null, null, null, null };

	private Array<TowerDefenseLevelWaveConfig> _wave = new Array<TowerDefenseLevelWaveConfig>();

	[ExportCategory("Survival")]
	[Export(PropertyHint.None, "")]
	public bool isCustomSurvival;

	[Export(PropertyHint.None, "")]
	public Variant survival = "";

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelSurvivalConfig customSurvival;

	[ExportCategory("Dynamic")]
	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelDynamicConfig> dynamic
	{
		get
		{
			return _dynamic;
		}
		set
		{
			_dynamic = value;
			WaveDynamicPlantfoodFill();
		}
	}

	[ExportCategory("Wave")]
	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelWaveConfig> wave
	{
		get
		{
			return _wave;
		}
		set
		{
			_wave = value;
			WaveDynamicPlantfoodFill();
		}
	}

	public void Init(Dictionary waveManagerData)
	{
		zombieInvisible = waveManagerData.GetValueOrDefault("ZombieInvisible", false).AsBool();
		flagZombieUse = waveManagerData.GetValueOrDefault("FlagZombieUse", true).AsBool();
		flagZombie = waveManagerData.GetValueOrDefault("FlagZombie", "ZombieFlag").AsString();
		flagWaveInterval = waveManagerData.GetValueOrDefault("FlagWaveInterval", 5.0).AsInt32();
		maxNextWaveHealthPercentage = waveManagerData.GetValueOrDefault("MaxNextWaveHealthPercentage", 0.15).AsDouble();
		minNextWaveHealthPercentage = waveManagerData.GetValueOrDefault("MinNextWaveHealthPercentage", 0.2).AsDouble();
		beginCol = waveManagerData.GetValueOrDefault("BeginCol", 20.0).AsDouble();
		spawnColEnd = waveManagerData.GetValueOrDefault("SpawnColEnd", 20.0).AsDouble();
		spawnColStart = waveManagerData.GetValueOrDefault("SpawnColStart", 5.0).AsDouble();
		spawnFrameBudgetMilliseconds = Mathf.Clamp(waveManagerData.GetValueOrDefault("SpawnFrameBudgetMilliseconds", 6.0).AsDouble(), 0.25, 16.0);
		spawnMaxCharactersPerFrame = Mathf.Max(1, waveManagerData.GetValueOrDefault("SpawnMaxCharactersPerFrame", 8).AsInt32());
		if (waveManagerData.ContainsKey("SpawnOverride"))
		{
			spawnOverride = new TowerDefenseCharacterOverride();
			spawnOverride.Init(waveManagerData.GetValueOrDefault("SpawnOverride", new Dictionary()).AsGodotDictionary());
		}
		Array obj = (waveManagerData.ContainsKey("Dynamic") ? ((Array)waveManagerData["Dynamic"]) : new Array());
		dynamic.Clear();
		foreach (Variant item in obj)
		{
			TowerDefenseLevelDynamicConfig towerDefenseLevelDynamicConfig = new TowerDefenseLevelDynamicConfig();
			Dictionary dictionary = item.AsGodotDictionary();
			if (dictionary.Count > 0)
			{
				towerDefenseLevelDynamicConfig.Init(dictionary);
			}
			dynamic.Add(towerDefenseLevelDynamicConfig);
		}
		Array obj2 = (waveManagerData.ContainsKey("Wave") ? ((Array)waveManagerData["Wave"]) : new Array());
		wave.Clear();
		foreach (Variant item2 in obj2)
		{
			TowerDefenseLevelWaveConfig towerDefenseLevelWaveConfig = new TowerDefenseLevelWaveConfig();
			towerDefenseLevelWaveConfig.Init(item2.AsGodotDictionary());
			wave.Add(towerDefenseLevelWaveConfig);
		}
		Variant valueOrDefault = waveManagerData.GetValueOrDefault("Survival", "");
		if (valueOrDefault.VariantType == Variant.Type.Dictionary)
		{
			isCustomSurvival = true;
			customSurvival = new TowerDefenseLevelSurvivalConfig();
			customSurvival.Load(valueOrDefault.AsGodotDictionary());
		}
		else
		{
			isCustomSurvival = false;
			survival = valueOrDefault;
		}
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["ZombieInvisible"] = zombieInvisible,
			["FlagZombieUse"] = flagZombieUse,
			["FlagZombie"] = flagZombie,
			["FlagWaveInterval"] = flagWaveInterval,
			["MaxNextWaveHealthPercentage"] = maxNextWaveHealthPercentage,
			["MinNextWaveHealthPercentage"] = minNextWaveHealthPercentage,
			["BeginCol"] = beginCol,
			["SpawnColEnd"] = spawnColEnd,
			["SpawnColStart"] = spawnColStart,
			["SpawnFrameBudgetMilliseconds"] = spawnFrameBudgetMilliseconds,
			["SpawnMaxCharactersPerFrame"] = spawnMaxCharactersPerFrame,
			["Dynamic"] = new Array(),
			["Wave"] = new Array(),
			["Survival"] = survival
		};
		if (GodotObject.IsInstanceValid(spawnOverride))
		{
			dictionary["SpawnOverride"] = spawnOverride.Export();
		}
		foreach (TowerDefenseLevelDynamicConfig item in dynamic)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				((Array)dictionary["Dynamic"]).Add(item.Export());
			}
			else
			{
				((Array)dictionary["Dynamic"]).Add(new Dictionary());
			}
		}
		foreach (TowerDefenseLevelWaveConfig item2 in wave)
		{
			((Array)dictionary["Wave"]).Add(item2.Export());
		}
		if (isCustomSurvival && GodotObject.IsInstanceValid(customSurvival))
		{
			dictionary["Survival"] = customSurvival.Export();
		}
		return dictionary;
	}

	public void WaveDynamicPlantfoodFill()
	{
		if (wave == null)
		{
			return;
		}
		foreach (Variant item in (Array?)wave)
		{
			if (item.VariantType != Variant.Type.Nil && item.AsGodotObject() is TowerDefenseLevelWaveConfig towerDefenseLevelWaveConfig)
			{
				towerDefenseLevelWaveConfig.dynamicPlantfood.Resize(dynamic.Count);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "waveManagerData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WaveDynamicPlantfoodFill, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.WaveDynamicPlantfoodFill && args.Count == 0)
		{
			WaveDynamicPlantfoodFill();
			ret = default;
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
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.WaveDynamicPlantfoodFill)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.dynamic)
		{
			dynamic = VariantUtils.ConvertToArray<TowerDefenseLevelDynamicConfig>(in value);
			return true;
		}
		if (name == PropertyName.wave)
		{
			wave = VariantUtils.ConvertToArray<TowerDefenseLevelWaveConfig>(in value);
			return true;
		}
		if (name == PropertyName.zombieInvisible)
		{
			zombieInvisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.flagZombieUse)
		{
			flagZombieUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.flagZombie)
		{
			flagZombie = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.flagWaveInterval)
		{
			flagWaveInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.maxNextWaveHealthPercentage)
		{
			maxNextWaveHealthPercentage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.minNextWaveHealthPercentage)
		{
			minNextWaveHealthPercentage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.beginCol)
		{
			beginCol = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spawnColEnd)
		{
			spawnColEnd = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spawnColStart)
		{
			spawnColStart = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spawnFrameBudgetMilliseconds)
		{
			spawnFrameBudgetMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spawnMaxCharactersPerFrame)
		{
			spawnMaxCharactersPerFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.spawnOverride)
		{
			spawnOverride = VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in value);
			return true;
		}
		if (name == PropertyName._dynamic)
		{
			_dynamic = VariantUtils.ConvertToArray<TowerDefenseLevelDynamicConfig>(in value);
			return true;
		}
		if (name == PropertyName._wave)
		{
			_wave = VariantUtils.ConvertToArray<TowerDefenseLevelWaveConfig>(in value);
			return true;
		}
		if (name == PropertyName.isCustomSurvival)
		{
			isCustomSurvival = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.survival)
		{
			survival = VariantUtils.ConvertTo<Variant>(in value);
			return true;
		}
		if (name == PropertyName.customSurvival)
		{
			customSurvival = VariantUtils.ConvertTo<TowerDefenseLevelSurvivalConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.dynamic)
		{
			value = VariantUtils.CreateFromArray(dynamic);
			return true;
		}
		if (name == PropertyName.wave)
		{
			value = VariantUtils.CreateFromArray(wave);
			return true;
		}
		if (name == PropertyName.zombieInvisible)
		{
			value = VariantUtils.CreateFrom(in zombieInvisible);
			return true;
		}
		if (name == PropertyName.flagZombieUse)
		{
			value = VariantUtils.CreateFrom(in flagZombieUse);
			return true;
		}
		if (name == PropertyName.flagZombie)
		{
			value = VariantUtils.CreateFrom(in flagZombie);
			return true;
		}
		if (name == PropertyName.flagWaveInterval)
		{
			value = VariantUtils.CreateFrom(in flagWaveInterval);
			return true;
		}
		if (name == PropertyName.maxNextWaveHealthPercentage)
		{
			value = VariantUtils.CreateFrom(in maxNextWaveHealthPercentage);
			return true;
		}
		if (name == PropertyName.minNextWaveHealthPercentage)
		{
			value = VariantUtils.CreateFrom(in minNextWaveHealthPercentage);
			return true;
		}
		if (name == PropertyName.beginCol)
		{
			value = VariantUtils.CreateFrom(in beginCol);
			return true;
		}
		if (name == PropertyName.spawnColEnd)
		{
			value = VariantUtils.CreateFrom(in spawnColEnd);
			return true;
		}
		if (name == PropertyName.spawnColStart)
		{
			value = VariantUtils.CreateFrom(in spawnColStart);
			return true;
		}
		if (name == PropertyName.spawnFrameBudgetMilliseconds)
		{
			value = VariantUtils.CreateFrom(in spawnFrameBudgetMilliseconds);
			return true;
		}
		if (name == PropertyName.spawnMaxCharactersPerFrame)
		{
			value = VariantUtils.CreateFrom(in spawnMaxCharactersPerFrame);
			return true;
		}
		if (name == PropertyName.spawnOverride)
		{
			value = VariantUtils.CreateFrom(in spawnOverride);
			return true;
		}
		if (name == PropertyName._dynamic)
		{
			value = VariantUtils.CreateFromArray(_dynamic);
			return true;
		}
		if (name == PropertyName._wave)
		{
			value = VariantUtils.CreateFromArray(_wave);
			return true;
		}
		if (name == PropertyName.isCustomSurvival)
		{
			value = VariantUtils.CreateFrom(in isCustomSurvival);
			return true;
		}
		if (name == PropertyName.survival)
		{
			value = VariantUtils.CreateFrom(in survival);
			return true;
		}
		if (name == PropertyName.customSurvival)
		{
			value = VariantUtils.CreateFrom(in customSurvival);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.zombieInvisible, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.flagZombieUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.flagZombie, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.flagWaveInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.maxNextWaveHealthPercentage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.minNextWaveHealthPercentage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.beginCol, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.spawnColEnd, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.spawnColStart, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.spawnFrameBudgetMilliseconds, PropertyHint.Range, "0.25,16,0.25", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.spawnMaxCharactersPerFrame, PropertyHint.Range, "1,64,1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.spawnOverride, PropertyHint.ResourceType, "TowerDefenseCharacterOverride", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Dynamic", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.dynamic, PropertyHint.TypeString, "24/17:TowerDefenseLevelDynamicConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName._dynamic, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, "Wave", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.wave, PropertyHint.TypeString, "24/17:TowerDefenseLevelWaveConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName._wave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, "Survival", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isCustomSurvival, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, PropertyName.survival, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable | PropertyUsageFlags.NilIsVariant, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.customSurvival, PropertyHint.ResourceType, "TowerDefenseLevelSurvivalConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dynamic, Variant.CreateFrom(dynamic));
		info.AddProperty(PropertyName.wave, Variant.CreateFrom(wave));
		info.AddProperty(PropertyName.zombieInvisible, Variant.From(in zombieInvisible));
		info.AddProperty(PropertyName.flagZombieUse, Variant.From(in flagZombieUse));
		info.AddProperty(PropertyName.flagZombie, Variant.From(in flagZombie));
		info.AddProperty(PropertyName.flagWaveInterval, Variant.From(in flagWaveInterval));
		info.AddProperty(PropertyName.maxNextWaveHealthPercentage, Variant.From(in maxNextWaveHealthPercentage));
		info.AddProperty(PropertyName.minNextWaveHealthPercentage, Variant.From(in minNextWaveHealthPercentage));
		info.AddProperty(PropertyName.beginCol, Variant.From(in beginCol));
		info.AddProperty(PropertyName.spawnColEnd, Variant.From(in spawnColEnd));
		info.AddProperty(PropertyName.spawnColStart, Variant.From(in spawnColStart));
		info.AddProperty(PropertyName.spawnFrameBudgetMilliseconds, Variant.From(in spawnFrameBudgetMilliseconds));
		info.AddProperty(PropertyName.spawnMaxCharactersPerFrame, Variant.From(in spawnMaxCharactersPerFrame));
		info.AddProperty(PropertyName.spawnOverride, Variant.From(in spawnOverride));
		info.AddProperty(PropertyName._dynamic, Variant.CreateFrom(_dynamic));
		info.AddProperty(PropertyName._wave, Variant.CreateFrom(_wave));
		info.AddProperty(PropertyName.isCustomSurvival, Variant.From(in isCustomSurvival));
		info.AddProperty(PropertyName.survival, Variant.From(in survival));
		info.AddProperty(PropertyName.customSurvival, Variant.From(in customSurvival));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dynamic, out var value))
		{
			dynamic = value.AsGodotArray<TowerDefenseLevelDynamicConfig>();
		}
		if (info.TryGetProperty(PropertyName.wave, out var value2))
		{
			wave = value2.AsGodotArray<TowerDefenseLevelWaveConfig>();
		}
		if (info.TryGetProperty(PropertyName.zombieInvisible, out var value3))
		{
			zombieInvisible = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.flagZombieUse, out var value4))
		{
			flagZombieUse = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.flagZombie, out var value5))
		{
			flagZombie = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.flagWaveInterval, out var value6))
		{
			flagWaveInterval = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.maxNextWaveHealthPercentage, out var value7))
		{
			maxNextWaveHealthPercentage = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.minNextWaveHealthPercentage, out var value8))
		{
			minNextWaveHealthPercentage = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.beginCol, out var value9))
		{
			beginCol = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spawnColEnd, out var value10))
		{
			spawnColEnd = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spawnColStart, out var value11))
		{
			spawnColStart = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spawnFrameBudgetMilliseconds, out var value12))
		{
			spawnFrameBudgetMilliseconds = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spawnMaxCharactersPerFrame, out var value13))
		{
			spawnMaxCharactersPerFrame = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName.spawnOverride, out var value14))
		{
			spawnOverride = value14.As<TowerDefenseCharacterOverride>();
		}
		if (info.TryGetProperty(PropertyName._dynamic, out var value15))
		{
			_dynamic = value15.AsGodotArray<TowerDefenseLevelDynamicConfig>();
		}
		if (info.TryGetProperty(PropertyName._wave, out var value16))
		{
			_wave = value16.AsGodotArray<TowerDefenseLevelWaveConfig>();
		}
		if (info.TryGetProperty(PropertyName.isCustomSurvival, out var value17))
		{
			isCustomSurvival = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.survival, out var value18))
		{
			survival = value18.As<Variant>();
		}
		if (info.TryGetProperty(PropertyName.customSurvival, out var value19))
		{
			customSurvival = value19.As<TowerDefenseLevelSurvivalConfig>();
		}
	}
}
