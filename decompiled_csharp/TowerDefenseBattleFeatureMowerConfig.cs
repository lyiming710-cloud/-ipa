using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Mower/Resource/TowerDefenseBattleFeatureMowerConfig.cs")]
public class TowerDefenseBattleFeatureMowerConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public static readonly StringName ReadRequiredName = "ReadRequiredName";

		public static readonly StringName ReadFinite = "ReadFinite";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName mowerPacketName = "mowerPacketName";

		public static readonly StringName waterMowerPacketName = "waterMowerPacketName";

		public static readonly StringName targetZombiePacketName = "targetZombiePacketName";

		public static readonly StringName previewSpriteName = "previewSpriteName";

		public static readonly StringName mowerSpawnOffsetX = "mowerSpawnOffsetX";

		public static readonly StringName targetSpawnOffsetX = "targetSpawnOffsetX";

		public static readonly StringName previewGroundOffset = "previewGroundOffset";

		public static readonly StringName previewTweenDuration = "previewTweenDuration";

		public static readonly StringName previewScale = "previewScale";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string mowerPacketName = "";

	[Export(PropertyHint.None, "")]
	public string waterMowerPacketName = "MowerPoolCleaner";

	[Export(PropertyHint.None, "")]
	public string targetZombiePacketName = "ZombieTarget";

	[Export(PropertyHint.None, "")]
	public string previewSpriteName = "MowerDefault";

	[Export(PropertyHint.None, "")]
	public double mowerSpawnOffsetX = 10.0;

	[Export(PropertyHint.None, "")]
	public double targetSpawnOffsetX = 40.0;

	[Export(PropertyHint.None, "")]
	public double previewGroundOffset = 10.0;

	[Export(PropertyHint.Range, "0,5,0.05")]
	public double previewTweenDuration = 0.5;

	[Export(PropertyHint.Range, "0.1,5,0.1")]
	public double previewScale = 1.0;

	public void Init(Dictionary data)
	{
		if (data == null)
		{
			data = new Dictionary();
		}
		mowerPacketName = data.GetValueOrDefault("MowerPacketName", "").AsString();
		waterMowerPacketName = ReadRequiredName(data, "WaterMowerPacketName", "MowerPoolCleaner");
		targetZombiePacketName = ReadRequiredName(data, "TargetZombiePacketName", "ZombieTarget");
		previewSpriteName = ReadRequiredName(data, "PreviewSpriteName", "MowerDefault");
		mowerSpawnOffsetX = ReadFinite(data, "MowerSpawnOffsetX", 10.0, -200.0, 200.0);
		targetSpawnOffsetX = ReadFinite(data, "TargetSpawnOffsetX", 40.0, -500.0, 500.0);
		previewGroundOffset = ReadFinite(data, "PreviewGroundOffset", 10.0, -200.0, 200.0);
		previewTweenDuration = ReadFinite(data, "PreviewTweenDuration", 0.5, 0.0, 5.0);
		previewScale = ReadFinite(data, "PreviewScale", 1.0, 0.1, 5.0);
	}

	public Dictionary Export()
	{
		return new Dictionary
		{
			["MowerPacketName"] = mowerPacketName,
			["WaterMowerPacketName"] = waterMowerPacketName,
			["TargetZombiePacketName"] = targetZombiePacketName,
			["PreviewSpriteName"] = previewSpriteName,
			["MowerSpawnOffsetX"] = mowerSpawnOffsetX,
			["TargetSpawnOffsetX"] = targetSpawnOffsetX,
			["PreviewGroundOffset"] = previewGroundOffset,
			["PreviewTweenDuration"] = previewTweenDuration,
			["PreviewScale"] = previewScale
		};
	}

	private static string ReadRequiredName(Dictionary data, string key, string fallback)
	{
		string text = data.GetValueOrDefault(key, fallback).AsString();
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return fallback;
	}

	private static double ReadFinite(Dictionary data, string key, double fallback, double minimum, double maximum)
	{
		double num = data.GetValueOrDefault(key, fallback).AsDouble();
		if (!double.IsFinite(num))
		{
			return fallback;
		}
		return Math.Clamp(num, minimum, maximum);
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
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadRequiredName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadFinite, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
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
		if (method == MethodName.ReadRequiredName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(ReadRequiredName(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadFinite && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(ReadFinite(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<double>(in args[4])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ReadRequiredName && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(ReadRequiredName(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadFinite && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(ReadFinite(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<double>(in args[4])));
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
		if (method == MethodName.ReadRequiredName)
		{
			return true;
		}
		if (method == MethodName.ReadFinite)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.mowerPacketName)
		{
			mowerPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.waterMowerPacketName)
		{
			waterMowerPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.targetZombiePacketName)
		{
			targetZombiePacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.previewSpriteName)
		{
			previewSpriteName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.mowerSpawnOffsetX)
		{
			mowerSpawnOffsetX = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.targetSpawnOffsetX)
		{
			targetSpawnOffsetX = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.previewGroundOffset)
		{
			previewGroundOffset = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.previewTweenDuration)
		{
			previewTweenDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.previewScale)
		{
			previewScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.mowerPacketName)
		{
			value = VariantUtils.CreateFrom(in mowerPacketName);
			return true;
		}
		if (name == PropertyName.waterMowerPacketName)
		{
			value = VariantUtils.CreateFrom(in waterMowerPacketName);
			return true;
		}
		if (name == PropertyName.targetZombiePacketName)
		{
			value = VariantUtils.CreateFrom(in targetZombiePacketName);
			return true;
		}
		if (name == PropertyName.previewSpriteName)
		{
			value = VariantUtils.CreateFrom(in previewSpriteName);
			return true;
		}
		if (name == PropertyName.mowerSpawnOffsetX)
		{
			value = VariantUtils.CreateFrom(in mowerSpawnOffsetX);
			return true;
		}
		if (name == PropertyName.targetSpawnOffsetX)
		{
			value = VariantUtils.CreateFrom(in targetSpawnOffsetX);
			return true;
		}
		if (name == PropertyName.previewGroundOffset)
		{
			value = VariantUtils.CreateFrom(in previewGroundOffset);
			return true;
		}
		if (name == PropertyName.previewTweenDuration)
		{
			value = VariantUtils.CreateFrom(in previewTweenDuration);
			return true;
		}
		if (name == PropertyName.previewScale)
		{
			value = VariantUtils.CreateFrom(in previewScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.mowerPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.waterMowerPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.targetZombiePacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.previewSpriteName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.mowerSpawnOffsetX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.targetSpawnOffsetX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.previewGroundOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.previewTweenDuration, PropertyHint.Range, "0,5,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.previewScale, PropertyHint.Range, "0.1,5,0.1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mowerPacketName, Variant.From(in mowerPacketName));
		info.AddProperty(PropertyName.waterMowerPacketName, Variant.From(in waterMowerPacketName));
		info.AddProperty(PropertyName.targetZombiePacketName, Variant.From(in targetZombiePacketName));
		info.AddProperty(PropertyName.previewSpriteName, Variant.From(in previewSpriteName));
		info.AddProperty(PropertyName.mowerSpawnOffsetX, Variant.From(in mowerSpawnOffsetX));
		info.AddProperty(PropertyName.targetSpawnOffsetX, Variant.From(in targetSpawnOffsetX));
		info.AddProperty(PropertyName.previewGroundOffset, Variant.From(in previewGroundOffset));
		info.AddProperty(PropertyName.previewTweenDuration, Variant.From(in previewTweenDuration));
		info.AddProperty(PropertyName.previewScale, Variant.From(in previewScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mowerPacketName, out var value))
		{
			mowerPacketName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.waterMowerPacketName, out var value2))
		{
			waterMowerPacketName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.targetZombiePacketName, out var value3))
		{
			targetZombiePacketName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.previewSpriteName, out var value4))
		{
			previewSpriteName = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.mowerSpawnOffsetX, out var value5))
		{
			mowerSpawnOffsetX = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.targetSpawnOffsetX, out var value6))
		{
			targetSpawnOffsetX = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.previewGroundOffset, out var value7))
		{
			previewGroundOffset = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.previewTweenDuration, out var value8))
		{
			previewTweenDuration = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.previewScale, out var value9))
		{
			previewScale = value9.As<double>();
		}
	}
}
