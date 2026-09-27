using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Conveyor/TowerDefenseConveyorConfig.cs")]
public class TowerDefenseConveyorConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName type = "type";

		public static readonly StringName interval = "interval";

		public static readonly StringName intervalIncreaseEvery = "intervalIncreaseEvery";

		public static readonly StringName intervalMagnification = "intervalMagnification";

		public static readonly StringName maxPacketCount = "maxPacketCount";

		public static readonly StringName packetPrioritySpawnList = "packetPrioritySpawnList";

		public static readonly StringName packetList = "packetList";

		public static readonly StringName waveEvent = "waveEvent";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string type = "Default";

	[Export(PropertyHint.None, "")]
	public double interval = 3.0;

	[Export(PropertyHint.None, "")]
	public int intervalIncreaseEvery = 1000000;

	[Export(PropertyHint.None, "")]
	public double intervalMagnification = 0.5;

	[Export(PropertyHint.None, "")]
	public int maxPacketCount = 14;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelPacketConfig> packetPrioritySpawnList = new Array<TowerDefenseLevelPacketConfig>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseConveyorPacketConfig> packetList = new Array<TowerDefenseConveyorPacketConfig>();

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Array waveEvent = new Godot.Collections.Array
	{
		new Godot.Collections.Array(),
		new Godot.Collections.Array(),
		new Godot.Collections.Array(),
		new Godot.Collections.Array(),
		new Godot.Collections.Array(),
		new Godot.Collections.Array(),
		new Godot.Collections.Array(),
		new Godot.Collections.Array(),
		new Godot.Collections.Array(),
		new Godot.Collections.Array(),
		new Godot.Collections.Array()
	};

	public void Init(Dictionary waveData)
	{
		packetList.Clear();
		packetPrioritySpawnList.Clear();
		type = waveData.GetValueOrDefault("Type", "Default").AsString();
		interval = Math.Max(0.05, waveData.GetValueOrDefault("Interval", 3.0).AsDouble());
		intervalIncreaseEvery = Math.Max(1, waveData.GetValueOrDefault("IntervalIncreaseEvery", 2).AsInt32());
		intervalMagnification = waveData.GetValueOrDefault("IntervalMagnification", 0.5).AsDouble();
		maxPacketCount = Math.Max(1, waveData.GetValueOrDefault("MaxPacketCount", 14).AsInt32());
		foreach (Variant item in waveData.GetValueOrDefault("PacketPrioritySpawnList", new Godot.Collections.Array()).AsGodotArray())
		{
			TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = new TowerDefenseLevelPacketConfig();
			towerDefenseLevelPacketConfig.Init(item);
			packetPrioritySpawnList.Add(towerDefenseLevelPacketConfig);
		}
		foreach (Variant item2 in waveData.GetValueOrDefault("Packet", new Godot.Collections.Array()).AsGodotArray())
		{
			TowerDefenseConveyorPacketConfig towerDefenseConveyorPacketConfig = new TowerDefenseConveyorPacketConfig();
			towerDefenseConveyorPacketConfig.Init(item2.AsGodotDictionary());
			packetList.Add(towerDefenseConveyorPacketConfig);
		}
		waveEvent.Clear();
		foreach (Variant item3 in waveData.GetValueOrDefault("WaveEvent", new Godot.Collections.Array()).AsGodotArray())
		{
			Array<TowerDefenseConveyorEventBase> array = new Array<TowerDefenseConveyorEventBase>();
			foreach (Variant item4 in item3.AsGodotArray())
			{
				Dictionary dictionary = item4.AsGodotDictionary();
				TowerDefenseConveyorEventBase towerDefenseConveyorEventBase = TowerDefenseConveyorEventEnum.EventGet(dictionary.GetValueOrDefault("EventName", "").AsString());
				if (GodotObject.IsInstanceValid(towerDefenseConveyorEventBase))
				{
					towerDefenseConveyorEventBase.Init(dictionary.GetValueOrDefault("Value", new Dictionary()).AsGodotDictionary());
					array.Add(towerDefenseConveyorEventBase);
				}
			}
			waveEvent.Add(array);
		}
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["Type"] = type,
			["Interval"] = interval,
			["IntervalIncreaseEvery"] = intervalIncreaseEvery,
			["IntervalMagnification"] = intervalMagnification,
			["MaxPacketCount"] = maxPacketCount,
			["PacketPrioritySpawnList"] = new Godot.Collections.Array(),
			["Packet"] = new Godot.Collections.Array(),
			["WaveEvent"] = new Godot.Collections.Array()
		};
		foreach (TowerDefenseLevelPacketConfig packetPrioritySpawn in packetPrioritySpawnList)
		{
			((Godot.Collections.Array)dictionary["PacketPrioritySpawnList"]).Add(packetPrioritySpawn.Export());
		}
		foreach (TowerDefenseConveyorPacketConfig packet in packetList)
		{
			((Godot.Collections.Array)dictionary["Packet"]).Add(packet.Export());
		}
		foreach (Variant item in waveEvent)
		{
			Godot.Collections.Array array = new Godot.Collections.Array();
			foreach (Variant item2 in item.AsGodotArray())
			{
				TowerDefenseConveyorEventBase towerDefenseConveyorEventBase = item2.As<TowerDefenseConveyorEventBase>();
				if (towerDefenseConveyorEventBase != null)
				{
					array.Add(towerDefenseConveyorEventBase.Export());
				}
			}
			((Godot.Collections.Array)dictionary["WaveEvent"]).Add(array);
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
				new PropertyInfo(Variant.Type.Dictionary, "waveData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (name == PropertyName.interval)
		{
			interval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.intervalIncreaseEvery)
		{
			intervalIncreaseEvery = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.intervalMagnification)
		{
			intervalMagnification = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.maxPacketCount)
		{
			maxPacketCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.packetPrioritySpawnList)
		{
			packetPrioritySpawnList = VariantUtils.ConvertToArray<TowerDefenseLevelPacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			packetList = VariantUtils.ConvertToArray<TowerDefenseConveyorPacketConfig>(in value);
			return true;
		}
		if (name == PropertyName.waveEvent)
		{
			waveEvent = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
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
		if (name == PropertyName.interval)
		{
			value = VariantUtils.CreateFrom(in interval);
			return true;
		}
		if (name == PropertyName.intervalIncreaseEvery)
		{
			value = VariantUtils.CreateFrom(in intervalIncreaseEvery);
			return true;
		}
		if (name == PropertyName.intervalMagnification)
		{
			value = VariantUtils.CreateFrom(in intervalMagnification);
			return true;
		}
		if (name == PropertyName.maxPacketCount)
		{
			value = VariantUtils.CreateFrom(in maxPacketCount);
			return true;
		}
		if (name == PropertyName.packetPrioritySpawnList)
		{
			value = VariantUtils.CreateFromArray(packetPrioritySpawnList);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			value = VariantUtils.CreateFromArray(packetList);
			return true;
		}
		if (name == PropertyName.waveEvent)
		{
			value = VariantUtils.CreateFrom(in waveEvent);
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
			new PropertyInfo(Variant.Type.Float, PropertyName.interval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.intervalIncreaseEvery, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.intervalMagnification, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxPacketCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetPrioritySpawnList, PropertyHint.TypeString, "24/17:TowerDefenseLevelPacketConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetList, PropertyHint.TypeString, "24/17:TowerDefenseConveyorPacketConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.waveEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.type, Variant.From(in type));
		info.AddProperty(PropertyName.interval, Variant.From(in interval));
		info.AddProperty(PropertyName.intervalIncreaseEvery, Variant.From(in intervalIncreaseEvery));
		info.AddProperty(PropertyName.intervalMagnification, Variant.From(in intervalMagnification));
		info.AddProperty(PropertyName.maxPacketCount, Variant.From(in maxPacketCount));
		info.AddProperty(PropertyName.packetPrioritySpawnList, Variant.CreateFrom(packetPrioritySpawnList));
		info.AddProperty(PropertyName.packetList, Variant.CreateFrom(packetList));
		info.AddProperty(PropertyName.waveEvent, Variant.From(in waveEvent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.type, out var value))
		{
			type = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.interval, out var value2))
		{
			interval = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.intervalIncreaseEvery, out var value3))
		{
			intervalIncreaseEvery = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.intervalMagnification, out var value4))
		{
			intervalMagnification = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.maxPacketCount, out var value5))
		{
			maxPacketCount = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.packetPrioritySpawnList, out var value6))
		{
			packetPrioritySpawnList = value6.AsGodotArray<TowerDefenseLevelPacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.packetList, out var value7))
		{
			packetList = value7.AsGodotArray<TowerDefenseConveyorPacketConfig>();
		}
		if (info.TryGetProperty(PropertyName.waveEvent, out var value8))
		{
			waveEvent = value8.As<Godot.Collections.Array>();
		}
	}
}
