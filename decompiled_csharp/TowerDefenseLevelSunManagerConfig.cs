using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/Sun/TowerDefenseLevelSunManagerConfig.cs")]
public class TowerDefenseLevelSunManagerConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName open = "open";

		public static readonly StringName type = "type";

		public static readonly StringName begin = "begin";

		public static readonly StringName spawnInterval = "spawnInterval";

		public static readonly StringName spawnNum = "spawnNum";

		public static readonly StringName movingMethod = "movingMethod";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool open = true;

	[Export(PropertyHint.None, "")]
	public string type = "Normal";

	[Export(PropertyHint.None, "")]
	public long begin = 300L;

	[Export(PropertyHint.None, "")]
	public double spawnInterval = 12.0;

	[Export(PropertyHint.None, "")]
	public long spawnNum = 50L;

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.SUN_MOVING_METHOD movingMethod;

	public void Init(Dictionary sunManagerData)
	{
		open = sunManagerData.GetValueOrDefault("Open", true).AsBool();
		type = sunManagerData.GetValueOrDefault("Type", "Normal").AsString();
		begin = sunManagerData.GetValueOrDefault("Begin", 300).AsInt64();
		spawnInterval = sunManagerData.GetValueOrDefault("SpawnInterval", 12.0).AsDouble();
		spawnNum = sunManagerData.GetValueOrDefault("SpawnNum", 50).AsInt64();
		movingMethod = (TowerDefenseEnum.SUN_MOVING_METHOD)Enum.Parse(typeof(TowerDefenseEnum.SUN_MOVING_METHOD), sunManagerData.GetValueOrDefault("MovingMethod", "LAND").AsString().ToUpper());
	}

	public Dictionary Export()
	{
		return new Dictionary
		{
			["Open"] = open,
			["Type"] = type,
			["Begin"] = begin,
			["SpawnInterval"] = spawnInterval,
			["SpawnNum"] = spawnNum,
			["MovingMethod"] = Enum.GetName(typeof(TowerDefenseEnum.SUN_MOVING_METHOD), movingMethod)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "sunManagerData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (name == PropertyName.type)
		{
			type = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.begin)
		{
			begin = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.spawnInterval)
		{
			spawnInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spawnNum)
		{
			spawnNum = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.movingMethod)
		{
			movingMethod = VariantUtils.ConvertTo<TowerDefenseEnum.SUN_MOVING_METHOD>(in value);
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
		if (name == PropertyName.type)
		{
			value = VariantUtils.CreateFrom(in type);
			return true;
		}
		if (name == PropertyName.begin)
		{
			value = VariantUtils.CreateFrom(in begin);
			return true;
		}
		if (name == PropertyName.spawnInterval)
		{
			value = VariantUtils.CreateFrom(in spawnInterval);
			return true;
		}
		if (name == PropertyName.spawnNum)
		{
			value = VariantUtils.CreateFrom(in spawnNum);
			return true;
		}
		if (name == PropertyName.movingMethod)
		{
			value = VariantUtils.CreateFrom(in movingMethod);
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
			new PropertyInfo(Variant.Type.String, PropertyName.type, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.begin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.spawnInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.spawnNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.movingMethod, PropertyHint.Enum, "LAND,GRAVITY,MOVING", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.open, Variant.From(in open));
		info.AddProperty(PropertyName.type, Variant.From(in type));
		info.AddProperty(PropertyName.begin, Variant.From(in begin));
		info.AddProperty(PropertyName.spawnInterval, Variant.From(in spawnInterval));
		info.AddProperty(PropertyName.spawnNum, Variant.From(in spawnNum));
		info.AddProperty(PropertyName.movingMethod, Variant.From(in movingMethod));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.open, out var value))
		{
			open = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.type, out var value2))
		{
			type = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.begin, out var value3))
		{
			begin = value3.As<long>();
		}
		if (info.TryGetProperty(PropertyName.spawnInterval, out var value4))
		{
			spawnInterval = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spawnNum, out var value5))
		{
			spawnNum = value5.As<long>();
		}
		if (info.TryGetProperty(PropertyName.movingMethod, out var value6))
		{
			movingMethod = value6.As<TowerDefenseEnum.SUN_MOVING_METHOD>();
		}
	}
}
