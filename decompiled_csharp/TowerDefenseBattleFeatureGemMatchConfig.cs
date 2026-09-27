using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/GemMatch/Resource/TowerDefenseBattleFeatureGemMatchConfig.cs")]
public class TowerDefenseBattleFeatureGemMatchConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName GetUpgradeTarget = "GetUpgradeTarget";

		public static readonly StringName GetUpgradeCost = "GetUpgradeCost";

		public static readonly StringName GetUpgradeSource = "GetUpgradeSource";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName boardRows = "boardRows";

		public static readonly StringName boardCols = "boardCols";

		public static readonly StringName plantList = "plantList";

		public static readonly StringName sunPerMatch = "sunPerMatch";

		public static readonly StringName matchValuePerMatch = "matchValuePerMatch";

		public static readonly StringName clearMatchValue = "clearMatchValue";

		public static readonly StringName clearMatchCount = "clearMatchCount";

		public static readonly StringName fallSpeed = "fallSpeed";

		public static readonly StringName swapDuration = "swapDuration";

		public static readonly StringName matchResolveDelay = "matchResolveDelay";

		public static readonly StringName fallSpawnHeight = "fallSpawnHeight";

		public static readonly StringName fallRowDelay = "fallRowDelay";

		public static readonly StringName minimumFallDuration = "minimumFallDuration";

		public static readonly StringName dragThreshold = "dragThreshold";

		public static readonly StringName inputRadius = "inputRadius";

		public static readonly StringName plantUpgradeList = "plantUpgradeList";

		public static readonly StringName fillHoleCost = "fillHoleCost";

		public static readonly StringName refreshBoardPacketKey = "refreshBoardPacketKey";

		public static readonly StringName refreshBoardCost = "refreshBoardCost";

		public static readonly StringName _upgradeMap = "_upgradeMap";

		public static readonly StringName _upgradeSourceMap = "_upgradeSourceMap";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public int boardRows = 5;

	[Export(PropertyHint.None, "")]
	public int boardCols = 9;

	[Export(PropertyHint.None, "")]
	public Array<StringName> plantList = new Array<StringName>();

	[Export(PropertyHint.None, "")]
	public int sunPerMatch = 25;

	[Export(PropertyHint.None, "")]
	public int matchValuePerMatch = 25;

	[Export(PropertyHint.None, "")]
	public int clearMatchValue;

	[Export(PropertyHint.None, "")]
	public int clearMatchCount;

	[Export(PropertyHint.None, "")]
	public double fallSpeed = 300.0;

	[Export(PropertyHint.None, "")]
	public double swapDuration = 0.15;

	[Export(PropertyHint.None, "")]
	public double matchResolveDelay = 0.05;

	[Export(PropertyHint.None, "")]
	public float fallSpawnHeight = 100f;

	[Export(PropertyHint.None, "")]
	public double fallRowDelay = 0.075;

	[Export(PropertyHint.None, "")]
	public double minimumFallDuration = 0.15;

	[Export(PropertyHint.None, "")]
	public float dragThreshold = 10f;

	[Export(PropertyHint.None, "")]
	public float inputRadius = 60f;

	[Export(PropertyHint.None, "")]
	public Array<Dictionary> plantUpgradeList = new Array<Dictionary>();

	[Export(PropertyHint.None, "")]
	public int fillHoleCost = 200;

	[Export(PropertyHint.None, "")]
	public string refreshBoardPacketKey = "PlantBlover";

	[Export(PropertyHint.None, "")]
	public int refreshBoardCost = 500;

	private Dictionary _upgradeMap = new Dictionary();

	private Dictionary _upgradeSourceMap = new Dictionary();

	public void Init(Dictionary data)
	{
		plantList.Clear();
		plantUpgradeList.Clear();
		_upgradeMap.Clear();
		_upgradeSourceMap.Clear();
		boardRows = Math.Max(1, data.GetValueOrDefault("boardRows", 5).AsInt32());
		boardCols = Math.Max(1, data.GetValueOrDefault("boardCols", 9).AsInt32());
		foreach (Variant item2 in data.ContainsKey("plantList") ? ((Godot.Collections.Array)data["plantList"]) : new Godot.Collections.Array())
		{
			StringName stringName = new StringName((string)item2);
			if (stringName != (StringName)"" && !plantList.Contains(stringName))
			{
				plantList.Add(stringName);
			}
		}
		sunPerMatch = Math.Max(0, data.GetValueOrDefault("sunPerMatch", 25).AsInt32());
		matchValuePerMatch = Math.Max(0, data.GetValueOrDefault("matchValuePerMatch", sunPerMatch).AsInt32());
		clearMatchValue = Math.Max(0, data.GetValueOrDefault("clearMatchValue", 0).AsInt32());
		clearMatchCount = Math.Max(0, data.GetValueOrDefault("clearMatchCount", clearMatchValue).AsInt32());
		fallSpeed = Math.Max(1.0, data.GetValueOrDefault("fallSpeed", 300.0).AsDouble());
		swapDuration = Math.Max(0.01, data.GetValueOrDefault("swapDuration", 0.15).AsDouble());
		matchResolveDelay = Math.Max(0.0, data.GetValueOrDefault("matchResolveDelay", 0.05).AsDouble());
		fallSpawnHeight = Math.Max(1f, (float)data.GetValueOrDefault("fallSpawnHeight", 100.0).AsDouble());
		fallRowDelay = Math.Max(0.0, data.GetValueOrDefault("fallRowDelay", 0.075).AsDouble());
		minimumFallDuration = Math.Max(0.01, data.GetValueOrDefault("minimumFallDuration", 0.15).AsDouble());
		dragThreshold = Math.Max(0f, (float)data.GetValueOrDefault("dragThreshold", 10.0).AsDouble());
		inputRadius = Math.Max(1f, (float)data.GetValueOrDefault("inputRadius", 60.0).AsDouble());
		fillHoleCost = Math.Max(0, data.GetValueOrDefault("fillHoleCost", 200).AsInt32());
		refreshBoardPacketKey = data.GetValueOrDefault("refreshBoardPacketKey", "PlantBlover").AsString();
		refreshBoardCost = Math.Max(0, data.GetValueOrDefault("refreshBoardCost", 500).AsInt32());
		foreach (Variant item3 in data.ContainsKey("plantUpgradeList") ? ((Godot.Collections.Array)data["plantUpgradeList"]) : new Godot.Collections.Array())
		{
			Dictionary dictionary = (Dictionary)item3;
			StringName stringName2 = new StringName(dictionary.ContainsKey("from") ? ((string)dictionary["from"]) : "");
			StringName stringName3 = new StringName(dictionary.ContainsKey("to") ? ((string)dictionary["to"]) : "");
			int num = Math.Max(0, dictionary.ContainsKey("cost") ? dictionary["cost"].AsInt32() : 0);
			if (stringName2 != (StringName)"" && stringName3 != (StringName)"")
			{
				Dictionary item = new Dictionary
				{
					["from"] = stringName2,
					["to"] = stringName3,
					["cost"] = num
				};
				plantUpgradeList.Add(item);
				Dictionary dictionary2 = new Dictionary();
				dictionary2["to"] = stringName3;
				dictionary2["cost"] = num;
				_upgradeMap[stringName2] = dictionary2;
				_upgradeSourceMap[stringName3] = stringName2;
			}
		}
	}

	public StringName GetUpgradeTarget(StringName characterKey)
	{
		if (_upgradeMap.ContainsKey(characterKey))
		{
			Dictionary dictionary = (Dictionary)_upgradeMap[characterKey];
			if (!dictionary.ContainsKey("to"))
			{
				return "";
			}
			return new StringName((string)dictionary["to"]);
		}
		return "";
	}

	public int GetUpgradeCost(StringName characterKey)
	{
		if (_upgradeMap.ContainsKey(characterKey))
		{
			Dictionary dictionary = (Dictionary)_upgradeMap[characterKey];
			if (!dictionary.ContainsKey("cost"))
			{
				return 0;
			}
			return dictionary["cost"].AsInt32();
		}
		return 0;
	}

	public StringName GetUpgradeSource(StringName targetKey)
	{
		if (!_upgradeSourceMap.TryGetValue(targetKey, out var value))
		{
			return new StringName();
		}
		return value.AsStringName();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetUpgradeTarget, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "characterKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetUpgradeCost, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "characterKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetUpgradeSource, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "targetKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.GetUpgradeTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(GetUpgradeTarget(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetUpgradeCost && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetUpgradeCost(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetUpgradeSource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StringName>(GetUpgradeSource(VariantUtils.ConvertTo<StringName>(in args[0])));
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
		if (method == MethodName.GetUpgradeTarget)
		{
			return true;
		}
		if (method == MethodName.GetUpgradeCost)
		{
			return true;
		}
		if (method == MethodName.GetUpgradeSource)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.boardRows)
		{
			boardRows = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.boardCols)
		{
			boardCols = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.plantList)
		{
			plantList = VariantUtils.ConvertToArray<StringName>(in value);
			return true;
		}
		if (name == PropertyName.sunPerMatch)
		{
			sunPerMatch = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.matchValuePerMatch)
		{
			matchValuePerMatch = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.clearMatchValue)
		{
			clearMatchValue = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.clearMatchCount)
		{
			clearMatchCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.fallSpeed)
		{
			fallSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.swapDuration)
		{
			swapDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.matchResolveDelay)
		{
			matchResolveDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fallSpawnHeight)
		{
			fallSpawnHeight = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.fallRowDelay)
		{
			fallRowDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.minimumFallDuration)
		{
			minimumFallDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dragThreshold)
		{
			dragThreshold = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.inputRadius)
		{
			inputRadius = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.plantUpgradeList)
		{
			plantUpgradeList = VariantUtils.ConvertToArray<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.fillHoleCost)
		{
			fillHoleCost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.refreshBoardPacketKey)
		{
			refreshBoardPacketKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.refreshBoardCost)
		{
			refreshBoardCost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._upgradeMap)
		{
			_upgradeMap = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._upgradeSourceMap)
		{
			_upgradeSourceMap = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.boardRows)
		{
			value = VariantUtils.CreateFrom(in boardRows);
			return true;
		}
		if (name == PropertyName.boardCols)
		{
			value = VariantUtils.CreateFrom(in boardCols);
			return true;
		}
		if (name == PropertyName.plantList)
		{
			value = VariantUtils.CreateFromArray(plantList);
			return true;
		}
		if (name == PropertyName.sunPerMatch)
		{
			value = VariantUtils.CreateFrom(in sunPerMatch);
			return true;
		}
		if (name == PropertyName.matchValuePerMatch)
		{
			value = VariantUtils.CreateFrom(in matchValuePerMatch);
			return true;
		}
		if (name == PropertyName.clearMatchValue)
		{
			value = VariantUtils.CreateFrom(in clearMatchValue);
			return true;
		}
		if (name == PropertyName.clearMatchCount)
		{
			value = VariantUtils.CreateFrom(in clearMatchCount);
			return true;
		}
		if (name == PropertyName.fallSpeed)
		{
			value = VariantUtils.CreateFrom(in fallSpeed);
			return true;
		}
		if (name == PropertyName.swapDuration)
		{
			value = VariantUtils.CreateFrom(in swapDuration);
			return true;
		}
		if (name == PropertyName.matchResolveDelay)
		{
			value = VariantUtils.CreateFrom(in matchResolveDelay);
			return true;
		}
		if (name == PropertyName.fallSpawnHeight)
		{
			value = VariantUtils.CreateFrom(in fallSpawnHeight);
			return true;
		}
		if (name == PropertyName.fallRowDelay)
		{
			value = VariantUtils.CreateFrom(in fallRowDelay);
			return true;
		}
		if (name == PropertyName.minimumFallDuration)
		{
			value = VariantUtils.CreateFrom(in minimumFallDuration);
			return true;
		}
		if (name == PropertyName.dragThreshold)
		{
			value = VariantUtils.CreateFrom(in dragThreshold);
			return true;
		}
		if (name == PropertyName.inputRadius)
		{
			value = VariantUtils.CreateFrom(in inputRadius);
			return true;
		}
		if (name == PropertyName.plantUpgradeList)
		{
			value = VariantUtils.CreateFromArray(plantUpgradeList);
			return true;
		}
		if (name == PropertyName.fillHoleCost)
		{
			value = VariantUtils.CreateFrom(in fillHoleCost);
			return true;
		}
		if (name == PropertyName.refreshBoardPacketKey)
		{
			value = VariantUtils.CreateFrom(in refreshBoardPacketKey);
			return true;
		}
		if (name == PropertyName.refreshBoardCost)
		{
			value = VariantUtils.CreateFrom(in refreshBoardCost);
			return true;
		}
		if (name == PropertyName._upgradeMap)
		{
			value = VariantUtils.CreateFrom(in _upgradeMap);
			return true;
		}
		if (name == PropertyName._upgradeSourceMap)
		{
			value = VariantUtils.CreateFrom(in _upgradeSourceMap);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.boardRows, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.boardCols, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.plantList, PropertyHint.TypeString, "21/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunPerMatch, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.matchValuePerMatch, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.clearMatchValue, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.clearMatchCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fallSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.swapDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.matchResolveDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fallSpawnHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fallRowDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.minimumFallDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dragThreshold, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.inputRadius, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.plantUpgradeList, PropertyHint.TypeString, "27/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.fillHoleCost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.refreshBoardPacketKey, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.refreshBoardCost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._upgradeMap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._upgradeSourceMap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.boardRows, Variant.From(in boardRows));
		info.AddProperty(PropertyName.boardCols, Variant.From(in boardCols));
		info.AddProperty(PropertyName.plantList, Variant.CreateFrom(plantList));
		info.AddProperty(PropertyName.sunPerMatch, Variant.From(in sunPerMatch));
		info.AddProperty(PropertyName.matchValuePerMatch, Variant.From(in matchValuePerMatch));
		info.AddProperty(PropertyName.clearMatchValue, Variant.From(in clearMatchValue));
		info.AddProperty(PropertyName.clearMatchCount, Variant.From(in clearMatchCount));
		info.AddProperty(PropertyName.fallSpeed, Variant.From(in fallSpeed));
		info.AddProperty(PropertyName.swapDuration, Variant.From(in swapDuration));
		info.AddProperty(PropertyName.matchResolveDelay, Variant.From(in matchResolveDelay));
		info.AddProperty(PropertyName.fallSpawnHeight, Variant.From(in fallSpawnHeight));
		info.AddProperty(PropertyName.fallRowDelay, Variant.From(in fallRowDelay));
		info.AddProperty(PropertyName.minimumFallDuration, Variant.From(in minimumFallDuration));
		info.AddProperty(PropertyName.dragThreshold, Variant.From(in dragThreshold));
		info.AddProperty(PropertyName.inputRadius, Variant.From(in inputRadius));
		info.AddProperty(PropertyName.plantUpgradeList, Variant.CreateFrom(plantUpgradeList));
		info.AddProperty(PropertyName.fillHoleCost, Variant.From(in fillHoleCost));
		info.AddProperty(PropertyName.refreshBoardPacketKey, Variant.From(in refreshBoardPacketKey));
		info.AddProperty(PropertyName.refreshBoardCost, Variant.From(in refreshBoardCost));
		info.AddProperty(PropertyName._upgradeMap, Variant.From(in _upgradeMap));
		info.AddProperty(PropertyName._upgradeSourceMap, Variant.From(in _upgradeSourceMap));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.boardRows, out var value))
		{
			boardRows = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.boardCols, out var value2))
		{
			boardCols = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.plantList, out var value3))
		{
			plantList = value3.AsGodotArray<StringName>();
		}
		if (info.TryGetProperty(PropertyName.sunPerMatch, out var value4))
		{
			sunPerMatch = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.matchValuePerMatch, out var value5))
		{
			matchValuePerMatch = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.clearMatchValue, out var value6))
		{
			clearMatchValue = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.clearMatchCount, out var value7))
		{
			clearMatchCount = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.fallSpeed, out var value8))
		{
			fallSpeed = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.swapDuration, out var value9))
		{
			swapDuration = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.matchResolveDelay, out var value10))
		{
			matchResolveDelay = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fallSpawnHeight, out var value11))
		{
			fallSpawnHeight = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName.fallRowDelay, out var value12))
		{
			fallRowDelay = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.minimumFallDuration, out var value13))
		{
			minimumFallDuration = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dragThreshold, out var value14))
		{
			dragThreshold = value14.As<float>();
		}
		if (info.TryGetProperty(PropertyName.inputRadius, out var value15))
		{
			inputRadius = value15.As<float>();
		}
		if (info.TryGetProperty(PropertyName.plantUpgradeList, out var value16))
		{
			plantUpgradeList = value16.AsGodotArray<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.fillHoleCost, out var value17))
		{
			fillHoleCost = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName.refreshBoardPacketKey, out var value18))
		{
			refreshBoardPacketKey = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName.refreshBoardCost, out var value19))
		{
			refreshBoardCost = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName._upgradeMap, out var value20))
		{
			_upgradeMap = value20.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._upgradeSourceMap, out var value21))
		{
			_upgradeSourceMap = value21.As<Dictionary>();
		}
	}
}
