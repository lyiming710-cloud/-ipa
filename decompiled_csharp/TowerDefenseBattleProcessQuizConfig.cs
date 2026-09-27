using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Process/Quiz/Resource/TowerDefenseBattleProcessQuizConfig.cs")]
public class TowerDefenseBattleProcessQuizConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName ResolveStripColumn = "ResolveStripColumn";

		public static readonly StringName Export = "Export";

		public static readonly StringName ReadName = "ReadName";

		public static readonly StringName ReadDuration = "ReadDuration";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName autoStripColumn = "autoStripColumn";

		public static readonly StringName stripColumn = "stripColumn";

		public static readonly StringName potPacketName = "potPacketName";

		public static readonly StringName lilyPadPacketName = "lilyPadPacketName";

		public static readonly StringName presentBoxPacketName = "presentBoxPacketName";

		public static readonly StringName zombieVasePacketName = "zombieVasePacketName";

		public static readonly StringName presentBoxPacketBank = "presentBoxPacketBank";

		public static readonly StringName zombieVasePacketBank = "zombieVasePacketBank";

		public static readonly StringName settlementLineDelay = "settlementLineDelay";

		public static readonly StringName settlementSummaryDelay = "settlementSummaryDelay";

		public static readonly StringName coinSpawnInterval = "coinSpawnInterval";

		public static readonly StringName coinFlightDelay = "coinFlightDelay";

		public static readonly StringName coinCollectDelay = "coinCollectDelay";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool autoStripColumn = true;

	[Export(PropertyHint.Range, "1,64,1")]
	public int stripColumn = 5;

	[Export(PropertyHint.None, "")]
	public string potPacketName = "PlantPot";

	[Export(PropertyHint.None, "")]
	public string lilyPadPacketName = "PlantLilyPad";

	[Export(PropertyHint.None, "")]
	public string presentBoxPacketName = "PlantPresentBox";

	[Export(PropertyHint.None, "")]
	public string zombieVasePacketName = "VaseZombie";

	[Export(PropertyHint.None, "")]
	public string presentBoxPacketBank = "PresentBoxNoAshPlant";

	[Export(PropertyHint.None, "")]
	public string zombieVasePacketBank = "QuizZombie";

	[Export(PropertyHint.Range, "0,10,0.05")]
	public double settlementLineDelay = 1.0;

	[Export(PropertyHint.Range, "0,10,0.05")]
	public double settlementSummaryDelay = 2.0;

	[Export(PropertyHint.Range, "0,2,0.01")]
	public double coinSpawnInterval = 0.1;

	[Export(PropertyHint.Range, "0,10,0.05")]
	public double coinFlightDelay = 1.0;

	[Export(PropertyHint.Range, "0,10,0.05")]
	public double coinCollectDelay = 0.5;

	public void Init(Dictionary data)
	{
		if (data == null)
		{
			data = new Dictionary();
		}
		autoStripColumn = data.GetValueOrDefault("AutoStripColumn", true).AsBool();
		stripColumn = Math.Clamp(data.GetValueOrDefault("StripColumn", 5).AsInt32(), 1, 64);
		potPacketName = ReadName(data, "PotPacketName", "PlantPot");
		lilyPadPacketName = ReadName(data, "LilyPadPacketName", "PlantLilyPad");
		presentBoxPacketName = ReadName(data, "PresentBoxPacketName", "PlantPresentBox");
		zombieVasePacketName = ReadName(data, "ZombieVasePacketName", "VaseZombie");
		presentBoxPacketBank = ReadName(data, "PresentBoxPacketBank", "PresentBoxNoAshPlant");
		zombieVasePacketBank = ReadName(data, "ZombieVasePacketBank", "QuizZombie");
		settlementLineDelay = ReadDuration(data, "SettlementLineDelay", 1.0, 10.0);
		settlementSummaryDelay = ReadDuration(data, "SettlementSummaryDelay", 2.0, 10.0);
		coinSpawnInterval = ReadDuration(data, "CoinSpawnInterval", 0.1, 2.0);
		coinFlightDelay = ReadDuration(data, "CoinFlightDelay", 1.0, 10.0);
		coinCollectDelay = ReadDuration(data, "CoinCollectDelay", 0.5, 10.0);
	}

	public int ResolveStripColumn(int mapColumnCount)
	{
		if (mapColumnCount <= 0)
		{
			return 1;
		}
		return Math.Clamp(autoStripColumn ? (Mathf.FloorToInt((float)mapColumnCount / 2f) + 1) : stripColumn, 1, mapColumnCount);
	}

	public Dictionary Export()
	{
		return new Dictionary
		{
			["AutoStripColumn"] = autoStripColumn,
			["StripColumn"] = stripColumn,
			["PotPacketName"] = potPacketName,
			["LilyPadPacketName"] = lilyPadPacketName,
			["PresentBoxPacketName"] = presentBoxPacketName,
			["ZombieVasePacketName"] = zombieVasePacketName,
			["PresentBoxPacketBank"] = presentBoxPacketBank,
			["ZombieVasePacketBank"] = zombieVasePacketBank,
			["SettlementLineDelay"] = settlementLineDelay,
			["SettlementSummaryDelay"] = settlementSummaryDelay,
			["CoinSpawnInterval"] = coinSpawnInterval,
			["CoinFlightDelay"] = coinFlightDelay,
			["CoinCollectDelay"] = coinCollectDelay
		};
	}

	private static string ReadName(Dictionary data, string key, string fallback)
	{
		string text = data.GetValueOrDefault(key, fallback).AsString();
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return fallback;
	}

	private static double ReadDuration(Dictionary data, string key, double fallback, double maximum)
	{
		double num = data.GetValueOrDefault(key, fallback).AsDouble();
		if (!double.IsFinite(num))
		{
			return fallback;
		}
		return Math.Clamp(num, 0.0, maximum);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveStripColumn, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mapColumnCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadDuration, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ResolveStripColumn && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveStripColumn(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.ReadName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(ReadName(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadDuration && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ReadDuration(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ReadName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(ReadName(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadDuration && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ReadDuration(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3])));
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
		if (method == MethodName.ResolveStripColumn)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.ReadName)
		{
			return true;
		}
		if (method == MethodName.ReadDuration)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.autoStripColumn)
		{
			autoStripColumn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.stripColumn)
		{
			stripColumn = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.potPacketName)
		{
			potPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.lilyPadPacketName)
		{
			lilyPadPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.presentBoxPacketName)
		{
			presentBoxPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.zombieVasePacketName)
		{
			zombieVasePacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.presentBoxPacketBank)
		{
			presentBoxPacketBank = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.zombieVasePacketBank)
		{
			zombieVasePacketBank = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.settlementLineDelay)
		{
			settlementLineDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.settlementSummaryDelay)
		{
			settlementSummaryDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.coinSpawnInterval)
		{
			coinSpawnInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.coinFlightDelay)
		{
			coinFlightDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.coinCollectDelay)
		{
			coinCollectDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.autoStripColumn)
		{
			value = VariantUtils.CreateFrom(in autoStripColumn);
			return true;
		}
		if (name == PropertyName.stripColumn)
		{
			value = VariantUtils.CreateFrom(in stripColumn);
			return true;
		}
		if (name == PropertyName.potPacketName)
		{
			value = VariantUtils.CreateFrom(in potPacketName);
			return true;
		}
		if (name == PropertyName.lilyPadPacketName)
		{
			value = VariantUtils.CreateFrom(in lilyPadPacketName);
			return true;
		}
		if (name == PropertyName.presentBoxPacketName)
		{
			value = VariantUtils.CreateFrom(in presentBoxPacketName);
			return true;
		}
		if (name == PropertyName.zombieVasePacketName)
		{
			value = VariantUtils.CreateFrom(in zombieVasePacketName);
			return true;
		}
		if (name == PropertyName.presentBoxPacketBank)
		{
			value = VariantUtils.CreateFrom(in presentBoxPacketBank);
			return true;
		}
		if (name == PropertyName.zombieVasePacketBank)
		{
			value = VariantUtils.CreateFrom(in zombieVasePacketBank);
			return true;
		}
		if (name == PropertyName.settlementLineDelay)
		{
			value = VariantUtils.CreateFrom(in settlementLineDelay);
			return true;
		}
		if (name == PropertyName.settlementSummaryDelay)
		{
			value = VariantUtils.CreateFrom(in settlementSummaryDelay);
			return true;
		}
		if (name == PropertyName.coinSpawnInterval)
		{
			value = VariantUtils.CreateFrom(in coinSpawnInterval);
			return true;
		}
		if (name == PropertyName.coinFlightDelay)
		{
			value = VariantUtils.CreateFrom(in coinFlightDelay);
			return true;
		}
		if (name == PropertyName.coinCollectDelay)
		{
			value = VariantUtils.CreateFrom(in coinCollectDelay);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.autoStripColumn, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.stripColumn, PropertyHint.Range, "1,64,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.potPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.lilyPadPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.presentBoxPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.zombieVasePacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.presentBoxPacketBank, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.zombieVasePacketBank, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.settlementLineDelay, PropertyHint.Range, "0,10,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.settlementSummaryDelay, PropertyHint.Range, "0,10,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.coinSpawnInterval, PropertyHint.Range, "0,2,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.coinFlightDelay, PropertyHint.Range, "0,10,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.coinCollectDelay, PropertyHint.Range, "0,10,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.autoStripColumn, Variant.From(in autoStripColumn));
		info.AddProperty(PropertyName.stripColumn, Variant.From(in stripColumn));
		info.AddProperty(PropertyName.potPacketName, Variant.From(in potPacketName));
		info.AddProperty(PropertyName.lilyPadPacketName, Variant.From(in lilyPadPacketName));
		info.AddProperty(PropertyName.presentBoxPacketName, Variant.From(in presentBoxPacketName));
		info.AddProperty(PropertyName.zombieVasePacketName, Variant.From(in zombieVasePacketName));
		info.AddProperty(PropertyName.presentBoxPacketBank, Variant.From(in presentBoxPacketBank));
		info.AddProperty(PropertyName.zombieVasePacketBank, Variant.From(in zombieVasePacketBank));
		info.AddProperty(PropertyName.settlementLineDelay, Variant.From(in settlementLineDelay));
		info.AddProperty(PropertyName.settlementSummaryDelay, Variant.From(in settlementSummaryDelay));
		info.AddProperty(PropertyName.coinSpawnInterval, Variant.From(in coinSpawnInterval));
		info.AddProperty(PropertyName.coinFlightDelay, Variant.From(in coinFlightDelay));
		info.AddProperty(PropertyName.coinCollectDelay, Variant.From(in coinCollectDelay));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.autoStripColumn, out var value))
		{
			autoStripColumn = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.stripColumn, out var value2))
		{
			stripColumn = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.potPacketName, out var value3))
		{
			potPacketName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.lilyPadPacketName, out var value4))
		{
			lilyPadPacketName = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.presentBoxPacketName, out var value5))
		{
			presentBoxPacketName = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.zombieVasePacketName, out var value6))
		{
			zombieVasePacketName = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.presentBoxPacketBank, out var value7))
		{
			presentBoxPacketBank = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.zombieVasePacketBank, out var value8))
		{
			zombieVasePacketBank = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.settlementLineDelay, out var value9))
		{
			settlementLineDelay = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.settlementSummaryDelay, out var value10))
		{
			settlementSummaryDelay = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.coinSpawnInterval, out var value11))
		{
			coinSpawnInterval = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.coinFlightDelay, out var value12))
		{
			coinFlightDelay = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.coinCollectDelay, out var value13))
		{
			coinCollectDelay = value13.As<double>();
		}
	}
}
