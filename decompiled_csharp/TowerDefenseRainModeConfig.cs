using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/RainMode/TowerDefenseRainModeConfig.cs")]
public class TowerDefenseRainModeConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName type = "type";

		public static readonly StringName aliveTime = "aliveTime";

		public static readonly StringName interval = "interval";

		public static readonly StringName packetList = "packetList";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string type = "Default";

	[Export(PropertyHint.None, "")]
	public double aliveTime = 30.0;

	[Export(PropertyHint.None, "")]
	public double interval = 3.0;

	[Export(PropertyHint.None, "")]
	public Array packetList = new Array();

	public void Init(Dictionary data)
	{
		packetList.Clear();
		type = data.GetValueOrDefault("Type", "Default").AsString();
		aliveTime = data.GetValueOrDefault("AliveTime", 15.0).AsDouble();
		interval = data.GetValueOrDefault("Interval", 3.0).AsDouble();
		foreach (Variant item in data.ContainsKey("Packet") ? ((Array)data["Packet"]) : new Array())
		{
			Dictionary data2 = item.AsGodotDictionary();
			TowerDefenseRainModePacketConfig towerDefenseRainModePacketConfig = new TowerDefenseRainModePacketConfig();
			towerDefenseRainModePacketConfig.Init(data2);
			packetList.Add(towerDefenseRainModePacketConfig);
		}
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["Type"] = type,
			["AliveTime"] = aliveTime,
			["Interval"] = interval,
			["Packet"] = new Array()
		};
		foreach (Variant packet in packetList)
		{
			TowerDefenseRainModePacketConfig towerDefenseRainModePacketConfig = (TowerDefenseRainModePacketConfig)(GodotObject)packet;
			((Array)dictionary["Packet"]).Add(towerDefenseRainModePacketConfig.Export());
		}
		return dictionary;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (name == PropertyName.type)
		{
			type = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.aliveTime)
		{
			aliveTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.interval)
		{
			interval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			packetList = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.type)
		{
			value = VariantUtils.CreateFrom(in type);
			return true;
		}
		if (name == PropertyName.aliveTime)
		{
			value = VariantUtils.CreateFrom(in aliveTime);
			return true;
		}
		if (name == PropertyName.interval)
		{
			value = VariantUtils.CreateFrom(in interval);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			value = VariantUtils.CreateFrom(in packetList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.type, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.aliveTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.interval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetList, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.type, Variant.From(in type));
		info.AddProperty(PropertyName.aliveTime, Variant.From(in aliveTime));
		info.AddProperty(PropertyName.interval, Variant.From(in interval));
		info.AddProperty(PropertyName.packetList, Variant.From(in packetList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.type, out var value))
		{
			type = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.aliveTime, out var value2))
		{
			aliveTime = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.interval, out var value3))
		{
			interval = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.packetList, out var value4))
		{
			packetList = value4.As<Array>();
		}
	}
}
