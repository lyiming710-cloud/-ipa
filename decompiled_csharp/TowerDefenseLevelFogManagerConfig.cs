using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/Fog/TowerDefenseLevelFogManagerConfig.cs")]
public class TowerDefenseLevelFogManagerConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName open = "open";

		public static readonly StringName beginColumn = "beginColumn";

		public static readonly StringName extraColumns = "extraColumns";

		public static readonly StringName blowReturnDelay = "blowReturnDelay";

		public static readonly StringName blowDistance = "blowDistance";

		public static readonly StringName blowDuration = "blowDuration";

		public static readonly StringName returnDuration = "returnDuration";

		public static readonly StringName entryDuration = "entryDuration";

		public static readonly StringName entryStartX = "entryStartX";

		public static readonly StringName magicOpen = "magicOpen";

		public static readonly StringName magicColor = "magicColor";

		public static readonly StringName magicChangeInterval = "magicChangeInterval";

		public static readonly StringName magicDamagePercentPerSecond = "magicDamagePercentPerSecond";

		public static readonly StringName magicHealPerSecond = "magicHealPerSecond";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool open;

	[Export(PropertyHint.None, "")]
	public int beginColumn = 5;

	[Export(PropertyHint.None, "")]
	public int extraColumns = 10;

	[Export(PropertyHint.None, "")]
	public double blowReturnDelay = 25.0;

	[Export(PropertyHint.None, "")]
	public float blowDistance = 1400f;

	[Export(PropertyHint.None, "")]
	public double blowDuration = 1.5;

	[Export(PropertyHint.None, "")]
	public double returnDuration = 3.0;

	[Export(PropertyHint.None, "")]
	public double entryDuration = 3.0;

	[Export(PropertyHint.None, "")]
	public float entryStartX = 1350f;

	[ExportCategory("MagicFog")]
	[Export(PropertyHint.None, "")]
	public bool magicOpen;

	[Export(PropertyHint.None, "")]
	public string magicColor = "Random";

	[Export(PropertyHint.None, "")]
	public double magicChangeInterval = 20.0;

	private const double DefaultMagicDamagePercentPerSecond = 0.05;

	private const double DefaultMagicHealPerSecond = 50.0;

	[Export(PropertyHint.None, "")]
	public double magicDamagePercentPerSecond = 0.05;

	[Export(PropertyHint.None, "")]
	public double magicHealPerSecond = 50.0;

	public void Init(Dictionary fogManagerData)
	{
		open = fogManagerData.GetValueOrDefault("Open", false).AsBool();
		beginColumn = fogManagerData.GetValueOrDefault("BeginColumn", 5).AsInt32();
		extraColumns = Mathf.Max(0, fogManagerData.GetValueOrDefault("ExtraColumns", 10).AsInt32());
		blowReturnDelay = Mathf.Max(0.001, fogManagerData.GetValueOrDefault("BlowReturnDelay", 25.0).AsDouble());
		blowDistance = Mathf.Max(0f, (float)fogManagerData.GetValueOrDefault("BlowDistance", 1400.0).AsDouble());
		blowDuration = Mathf.Max(0.001, fogManagerData.GetValueOrDefault("BlowDuration", 1.5).AsDouble());
		returnDuration = Mathf.Max(0.001, fogManagerData.GetValueOrDefault("ReturnDuration", 3.0).AsDouble());
		entryDuration = Mathf.Max(0.001, fogManagerData.GetValueOrDefault("EntryDuration", 3.0).AsDouble());
		entryStartX = (float)fogManagerData.GetValueOrDefault("EntryStartX", 1350.0).AsDouble();
		magicOpen = fogManagerData.GetValueOrDefault("MagicOpen", false).AsBool();
		magicColor = fogManagerData.GetValueOrDefault("MagicColor", "Random").AsString();
		magicDamagePercentPerSecond = Mathf.Max(0.0, fogManagerData.GetValueOrDefault("MagicDamagePercentPerSecond", 0.05).AsDouble());
		magicHealPerSecond = Mathf.Max(0.0, fogManagerData.GetValueOrDefault("MagicHealPerSecond", 50.0).AsDouble());
		magicChangeInterval = Mathf.Max(0.0, fogManagerData.GetValueOrDefault("MagicChangeInterval", 20.0).AsDouble());
	}

	public Dictionary Export()
	{
		return new Dictionary
		{
			["Open"] = open,
			["BeginColumn"] = beginColumn,
			["ExtraColumns"] = extraColumns,
			["BlowReturnDelay"] = blowReturnDelay,
			["BlowDistance"] = blowDistance,
			["BlowDuration"] = blowDuration,
			["ReturnDuration"] = returnDuration,
			["EntryDuration"] = entryDuration,
			["EntryStartX"] = entryStartX,
			["MagicOpen"] = magicOpen,
			["MagicColor"] = magicColor,
			["MagicChangeInterval"] = magicChangeInterval,
			["MagicDamagePercentPerSecond"] = magicDamagePercentPerSecond,
			["MagicHealPerSecond"] = magicHealPerSecond
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "fogManagerData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.open)
		{
			open = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.beginColumn)
		{
			beginColumn = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.extraColumns)
		{
			extraColumns = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.blowReturnDelay)
		{
			blowReturnDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.blowDistance)
		{
			blowDistance = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.blowDuration)
		{
			blowDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.returnDuration)
		{
			returnDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.entryDuration)
		{
			entryDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.entryStartX)
		{
			entryStartX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.magicOpen)
		{
			magicOpen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.magicColor)
		{
			magicColor = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.magicChangeInterval)
		{
			magicChangeInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.magicDamagePercentPerSecond)
		{
			magicDamagePercentPerSecond = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.magicHealPerSecond)
		{
			magicHealPerSecond = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.open)
		{
			value = VariantUtils.CreateFrom(in open);
			return true;
		}
		if (name == PropertyName.beginColumn)
		{
			value = VariantUtils.CreateFrom(in beginColumn);
			return true;
		}
		if (name == PropertyName.extraColumns)
		{
			value = VariantUtils.CreateFrom(in extraColumns);
			return true;
		}
		if (name == PropertyName.blowReturnDelay)
		{
			value = VariantUtils.CreateFrom(in blowReturnDelay);
			return true;
		}
		if (name == PropertyName.blowDistance)
		{
			value = VariantUtils.CreateFrom(in blowDistance);
			return true;
		}
		if (name == PropertyName.blowDuration)
		{
			value = VariantUtils.CreateFrom(in blowDuration);
			return true;
		}
		if (name == PropertyName.returnDuration)
		{
			value = VariantUtils.CreateFrom(in returnDuration);
			return true;
		}
		if (name == PropertyName.entryDuration)
		{
			value = VariantUtils.CreateFrom(in entryDuration);
			return true;
		}
		if (name == PropertyName.entryStartX)
		{
			value = VariantUtils.CreateFrom(in entryStartX);
			return true;
		}
		if (name == PropertyName.magicOpen)
		{
			value = VariantUtils.CreateFrom(in magicOpen);
			return true;
		}
		if (name == PropertyName.magicColor)
		{
			value = VariantUtils.CreateFrom(in magicColor);
			return true;
		}
		if (name == PropertyName.magicChangeInterval)
		{
			value = VariantUtils.CreateFrom(in magicChangeInterval);
			return true;
		}
		if (name == PropertyName.magicDamagePercentPerSecond)
		{
			value = VariantUtils.CreateFrom(in magicDamagePercentPerSecond);
			return true;
		}
		if (name == PropertyName.magicHealPerSecond)
		{
			value = VariantUtils.CreateFrom(in magicHealPerSecond);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.open, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.beginColumn, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.extraColumns, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.blowReturnDelay, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.blowDistance, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.blowDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.returnDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.entryDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.entryStartX, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "MagicFog", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.magicOpen, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.magicColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.magicChangeInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.magicDamagePercentPerSecond, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.magicHealPerSecond, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.open, Variant.From(in open));
		info.AddProperty(PropertyName.beginColumn, Variant.From(in beginColumn));
		info.AddProperty(PropertyName.extraColumns, Variant.From(in extraColumns));
		info.AddProperty(PropertyName.blowReturnDelay, Variant.From(in blowReturnDelay));
		info.AddProperty(PropertyName.blowDistance, Variant.From(in blowDistance));
		info.AddProperty(PropertyName.blowDuration, Variant.From(in blowDuration));
		info.AddProperty(PropertyName.returnDuration, Variant.From(in returnDuration));
		info.AddProperty(PropertyName.entryDuration, Variant.From(in entryDuration));
		info.AddProperty(PropertyName.entryStartX, Variant.From(in entryStartX));
		info.AddProperty(PropertyName.magicOpen, Variant.From(in magicOpen));
		info.AddProperty(PropertyName.magicColor, Variant.From(in magicColor));
		info.AddProperty(PropertyName.magicChangeInterval, Variant.From(in magicChangeInterval));
		info.AddProperty(PropertyName.magicDamagePercentPerSecond, Variant.From(in magicDamagePercentPerSecond));
		info.AddProperty(PropertyName.magicHealPerSecond, Variant.From(in magicHealPerSecond));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.open, out var value))
		{
			open = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.beginColumn, out var value2))
		{
			beginColumn = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.extraColumns, out var value3))
		{
			extraColumns = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.blowReturnDelay, out var value4))
		{
			blowReturnDelay = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.blowDistance, out var value5))
		{
			blowDistance = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName.blowDuration, out var value6))
		{
			blowDuration = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.returnDuration, out var value7))
		{
			returnDuration = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.entryDuration, out var value8))
		{
			entryDuration = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.entryStartX, out var value9))
		{
			entryStartX = value9.As<float>();
		}
		if (info.TryGetProperty(PropertyName.magicOpen, out var value10))
		{
			magicOpen = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.magicColor, out var value11))
		{
			magicColor = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.magicChangeInterval, out var value12))
		{
			magicChangeInterval = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.magicDamagePercentPerSecond, out var value13))
		{
			magicDamagePercentPerSecond = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.magicHealPerSecond, out var value14))
		{
			magicHealPerSecond = value14.As<double>();
		}
	}
}
