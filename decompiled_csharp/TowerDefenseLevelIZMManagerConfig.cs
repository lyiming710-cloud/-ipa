using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/IZM/TowerDefenseLevelIZMManagerConfig.cs")]
public class TowerDefenseLevelIZMManagerConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public static readonly StringName ReadDuration = "ReadDuration";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName shuffle = "shuffle";

		public static readonly StringName preSpawnMaxRetryPasses = "preSpawnMaxRetryPasses";

		public static readonly StringName failureCheckIntervalFrames = "failureCheckIntervalFrames";

		public static readonly StringName packetBankExitDelay = "packetBankExitDelay";

		public static readonly StringName enterHouseFadeDuration = "enterHouseFadeDuration";

		public static readonly StringName failureIgnoredZombieName = "failureIgnoredZombieName";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool shuffle = true;

	[Export(PropertyHint.Range, "1,64,1")]
	public int preSpawnMaxRetryPasses = 8;

	[Export(PropertyHint.Range, "1,120,1")]
	public int failureCheckIntervalFrames = 6;

	[Export(PropertyHint.Range, "0,5,0.05")]
	public double packetBankExitDelay = 0.5;

	[Export(PropertyHint.Range, "0,5,0.05")]
	public double enterHouseFadeDuration = 1.0;

	[Export(PropertyHint.None, "")]
	public string failureIgnoredZombieName = "ZombieTarget";

	public void Init(Dictionary data)
	{
		shuffle = data.GetValueOrDefault("Shuffle", true).AsBool();
		preSpawnMaxRetryPasses = Math.Clamp(data.GetValueOrDefault("PreSpawnMaxRetryPasses", 8).AsInt32(), 1, 64);
		failureCheckIntervalFrames = Math.Clamp(data.GetValueOrDefault("FailureCheckIntervalFrames", 6).AsInt32(), 1, 120);
		packetBankExitDelay = ReadDuration(data, "PacketBankExitDelay", 0.5);
		enterHouseFadeDuration = ReadDuration(data, "EnterHouseFadeDuration", 1.0);
		failureIgnoredZombieName = data.GetValueOrDefault("FailureIgnoredZombieName", "ZombieTarget").AsString();
		if (string.IsNullOrWhiteSpace(failureIgnoredZombieName))
		{
			failureIgnoredZombieName = "ZombieTarget";
		}
	}

	public Dictionary Export()
	{
		return new Dictionary
		{
			["Shuffle"] = shuffle,
			["PreSpawnMaxRetryPasses"] = preSpawnMaxRetryPasses,
			["FailureCheckIntervalFrames"] = failureCheckIntervalFrames,
			["PacketBankExitDelay"] = packetBankExitDelay,
			["EnterHouseFadeDuration"] = enterHouseFadeDuration,
			["FailureIgnoredZombieName"] = failureIgnoredZombieName
		};
	}

	private static double ReadDuration(Dictionary data, string key, double fallback)
	{
		double num = data.GetValueOrDefault(key, fallback).AsDouble();
		if (!double.IsFinite(num))
		{
			return fallback;
		}
		return Math.Clamp(num, 0.0, 5.0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadDuration, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ReadDuration && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadDuration(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ReadDuration && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadDuration(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
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
		if (method == MethodName.ReadDuration)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.shuffle)
		{
			shuffle = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.preSpawnMaxRetryPasses)
		{
			preSpawnMaxRetryPasses = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.failureCheckIntervalFrames)
		{
			failureCheckIntervalFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.packetBankExitDelay)
		{
			packetBankExitDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.enterHouseFadeDuration)
		{
			enterHouseFadeDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.failureIgnoredZombieName)
		{
			failureIgnoredZombieName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.shuffle)
		{
			value = VariantUtils.CreateFrom(in shuffle);
			return true;
		}
		if (name == PropertyName.preSpawnMaxRetryPasses)
		{
			value = VariantUtils.CreateFrom(in preSpawnMaxRetryPasses);
			return true;
		}
		if (name == PropertyName.failureCheckIntervalFrames)
		{
			value = VariantUtils.CreateFrom(in failureCheckIntervalFrames);
			return true;
		}
		if (name == PropertyName.packetBankExitDelay)
		{
			value = VariantUtils.CreateFrom(in packetBankExitDelay);
			return true;
		}
		if (name == PropertyName.enterHouseFadeDuration)
		{
			value = VariantUtils.CreateFrom(in enterHouseFadeDuration);
			return true;
		}
		if (name == PropertyName.failureIgnoredZombieName)
		{
			value = VariantUtils.CreateFrom(in failureIgnoredZombieName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.shuffle, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.preSpawnMaxRetryPasses, PropertyHint.Range, "1,64,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.failureCheckIntervalFrames, PropertyHint.Range, "1,120,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.packetBankExitDelay, PropertyHint.Range, "0,5,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.enterHouseFadeDuration, PropertyHint.Range, "0,5,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.failureIgnoredZombieName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.shuffle, Variant.From(in shuffle));
		info.AddProperty(PropertyName.preSpawnMaxRetryPasses, Variant.From(in preSpawnMaxRetryPasses));
		info.AddProperty(PropertyName.failureCheckIntervalFrames, Variant.From(in failureCheckIntervalFrames));
		info.AddProperty(PropertyName.packetBankExitDelay, Variant.From(in packetBankExitDelay));
		info.AddProperty(PropertyName.enterHouseFadeDuration, Variant.From(in enterHouseFadeDuration));
		info.AddProperty(PropertyName.failureIgnoredZombieName, Variant.From(in failureIgnoredZombieName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.shuffle, out var value))
		{
			shuffle = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.preSpawnMaxRetryPasses, out var value2))
		{
			preSpawnMaxRetryPasses = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.failureCheckIntervalFrames, out var value3))
		{
			failureCheckIntervalFrames = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.packetBankExitDelay, out var value4))
		{
			packetBankExitDelay = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.enterHouseFadeDuration, out var value5))
		{
			enterHouseFadeDuration = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.failureIgnoredZombieName, out var value6))
		{
			failureIgnoredZombieName = value6.As<string>();
		}
	}
}
