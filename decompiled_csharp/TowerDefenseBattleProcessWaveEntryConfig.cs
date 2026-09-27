using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Process/Resource/TowerDefenseBattleProcessWaveEntryConfig.cs")]
public class TowerDefenseBattleProcessWaveEntryConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public static readonly StringName ReadDuration = "ReadDuration";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName mowerUse = "mowerUse";

		public static readonly StringName entryBroadcastDuration = "entryBroadcastDuration";

		public static readonly StringName cameraTravelDuration = "cameraTravelDuration";

		public static readonly StringName packetBankExitDelay = "packetBankExitDelay";

		public static readonly StringName entryLabelDuration = "entryLabelDuration";

		public static readonly StringName debugEnterHouseFadeDuration = "debugEnterHouseFadeDuration";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool mowerUse;

	[Export(PropertyHint.Range, "0,15,0.05")]
	public double entryBroadcastDuration = 4.0;

	[Export(PropertyHint.Range, "0,10,0.05")]
	public double cameraTravelDuration = 1.5;

	[Export(PropertyHint.Range, "0,5,0.05")]
	public double packetBankExitDelay = 0.5;

	[Export(PropertyHint.Range, "0,10,0.05")]
	public double entryLabelDuration = 2.0;

	[Export(PropertyHint.Range, "0,5,0.05")]
	public double debugEnterHouseFadeDuration = 1.0;

	public virtual void Init(Dictionary data)
	{
		if (data == null)
		{
			data = new Dictionary();
		}
		mowerUse = data.GetValueOrDefault("MowerUse", false).AsBool();
		entryBroadcastDuration = ReadDuration(data, "EntryBroadcastDuration", 4.0, 15.0);
		cameraTravelDuration = ReadDuration(data, "CameraTravelDuration", 1.5, 10.0);
		packetBankExitDelay = ReadDuration(data, "PacketBankExitDelay", 0.5, 5.0);
		entryLabelDuration = ReadDuration(data, "EntryLabelDuration", 2.0, 10.0);
		debugEnterHouseFadeDuration = ReadDuration(data, "DebugEnterHouseFadeDuration", 1.0, 5.0);
	}

	public virtual Dictionary Export()
	{
		return new Dictionary
		{
			["MowerUse"] = mowerUse,
			["EntryBroadcastDuration"] = entryBroadcastDuration,
			["CameraTravelDuration"] = cameraTravelDuration,
			["PacketBankExitDelay"] = packetBankExitDelay,
			["EntryLabelDuration"] = entryLabelDuration,
			["DebugEnterHouseFadeDuration"] = debugEnterHouseFadeDuration
		};
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
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
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
		if (name == PropertyName.mowerUse)
		{
			mowerUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.entryBroadcastDuration)
		{
			entryBroadcastDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.cameraTravelDuration)
		{
			cameraTravelDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.packetBankExitDelay)
		{
			packetBankExitDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.entryLabelDuration)
		{
			entryLabelDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.debugEnterHouseFadeDuration)
		{
			debugEnterHouseFadeDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.mowerUse)
		{
			value = VariantUtils.CreateFrom(in mowerUse);
			return true;
		}
		if (name == PropertyName.entryBroadcastDuration)
		{
			value = VariantUtils.CreateFrom(in entryBroadcastDuration);
			return true;
		}
		if (name == PropertyName.cameraTravelDuration)
		{
			value = VariantUtils.CreateFrom(in cameraTravelDuration);
			return true;
		}
		if (name == PropertyName.packetBankExitDelay)
		{
			value = VariantUtils.CreateFrom(in packetBankExitDelay);
			return true;
		}
		if (name == PropertyName.entryLabelDuration)
		{
			value = VariantUtils.CreateFrom(in entryLabelDuration);
			return true;
		}
		if (name == PropertyName.debugEnterHouseFadeDuration)
		{
			value = VariantUtils.CreateFrom(in debugEnterHouseFadeDuration);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.mowerUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.entryBroadcastDuration, PropertyHint.Range, "0,15,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.cameraTravelDuration, PropertyHint.Range, "0,10,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.packetBankExitDelay, PropertyHint.Range, "0,5,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.entryLabelDuration, PropertyHint.Range, "0,10,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.debugEnterHouseFadeDuration, PropertyHint.Range, "0,5,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mowerUse, Variant.From(in mowerUse));
		info.AddProperty(PropertyName.entryBroadcastDuration, Variant.From(in entryBroadcastDuration));
		info.AddProperty(PropertyName.cameraTravelDuration, Variant.From(in cameraTravelDuration));
		info.AddProperty(PropertyName.packetBankExitDelay, Variant.From(in packetBankExitDelay));
		info.AddProperty(PropertyName.entryLabelDuration, Variant.From(in entryLabelDuration));
		info.AddProperty(PropertyName.debugEnterHouseFadeDuration, Variant.From(in debugEnterHouseFadeDuration));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mowerUse, out var value))
		{
			mowerUse = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.entryBroadcastDuration, out var value2))
		{
			entryBroadcastDuration = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.cameraTravelDuration, out var value3))
		{
			cameraTravelDuration = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.packetBankExitDelay, out var value4))
		{
			packetBankExitDelay = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.entryLabelDuration, out var value5))
		{
			entryLabelDuration = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.debugEnterHouseFadeDuration, out var value6))
		{
			debugEnterHouseFadeDuration = value6.As<double>();
		}
	}
}
