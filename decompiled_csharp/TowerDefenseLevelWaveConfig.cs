using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/Wave/TowerDefenseLevelWaveConfig.cs")]
public class TowerDefenseLevelWaveConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName dynamicPlantfood = "dynamicPlantfood";

		public static readonly StringName spawn = "spawn";

		public static readonly StringName gridSpawn = "gridSpawn";

		public static readonly StringName dynamic = "dynamic";

		public static readonly StringName eventList = "eventList";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<int> dynamicPlantfood = new Array<int>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelSpawnConfig> spawn = new Array<TowerDefenseLevelSpawnConfig>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelGridSpawnConfig> gridSpawn = new Array<TowerDefenseLevelGridSpawnConfig>();

	[Export(PropertyHint.None, "")]
	public TowerDefenseLevelSpawnDynamicConfig dynamic = new TowerDefenseLevelSpawnDynamicConfig();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelEventBase> eventList = new Array<TowerDefenseLevelEventBase>();

	public void Init(Dictionary waveDictionary)
	{
		if (waveDictionary == null)
		{
			waveDictionary = new Dictionary();
		}
		if (dynamicPlantfood == null)
		{
			dynamicPlantfood = new Array<int>();
		}
		if (spawn == null)
		{
			spawn = new Array<TowerDefenseLevelSpawnConfig>();
		}
		if (gridSpawn == null)
		{
			gridSpawn = new Array<TowerDefenseLevelGridSpawnConfig>();
		}
		if (eventList == null)
		{
			eventList = new Array<TowerDefenseLevelEventBase>();
		}
		dynamicPlantfood.Clear();
		spawn.Clear();
		gridSpawn.Clear();
		eventList.Clear();
		Godot.Collections.Array array = waveDictionary.GetValueOrDefault("DynamicPlantfood", new Godot.Collections.Array()).AsGodotArray();
		if (array != null)
		{
			foreach (Variant item in array)
			{
				dynamicPlantfood.Add(item.AsInt32());
			}
		}
		foreach (Variant item2 in waveDictionary.GetValueOrDefault("Spawn", new Godot.Collections.Array()).AsGodotArray())
		{
			if (item2.VariantType == Variant.Type.Dictionary)
			{
				TowerDefenseLevelSpawnConfig towerDefenseLevelSpawnConfig = new TowerDefenseLevelSpawnConfig();
				towerDefenseLevelSpawnConfig.Init(item2.AsGodotDictionary());
				spawn.Add(towerDefenseLevelSpawnConfig);
			}
		}
		foreach (Variant item3 in waveDictionary.GetValueOrDefault("GridSpawn", new Godot.Collections.Array()).AsGodotArray())
		{
			if (item3.VariantType == Variant.Type.Dictionary)
			{
				TowerDefenseLevelGridSpawnConfig towerDefenseLevelGridSpawnConfig = new TowerDefenseLevelGridSpawnConfig();
				towerDefenseLevelGridSpawnConfig.Init(item3.AsGodotDictionary());
				gridSpawn.Add(towerDefenseLevelGridSpawnConfig);
			}
		}
		Dictionary dynamicDictionary = waveDictionary.GetValueOrDefault("Dynamic", new Dictionary()).AsGodotDictionary();
		dynamic = new TowerDefenseLevelSpawnDynamicConfig();
		dynamic.Init(dynamicDictionary);
		Godot.Collections.Array array2 = waveDictionary.GetValueOrDefault("Event", new Godot.Collections.Array()).AsGodotArray();
		for (int i = 0; i < array2.Count; i++)
		{
			try
			{
				if (array2[i].VariantType == Variant.Type.Dictionary)
				{
					Dictionary dictionary = array2[i].AsGodotDictionary();
					string text = dictionary.GetValueOrDefault("EventName", "").AsString();
					TowerDefenseLevelEventBase towerDefenseLevelEventBase = TowerDefenseLevelEventRegistry.Create(text);
					if (!GodotObject.IsInstanceValid(towerDefenseLevelEventBase))
					{
						GD.PushError($"[WaveConfig] Unknown Event[{i}] '{text}'.");
					}
					else
					{
						Dictionary valueDictionary = dictionary.GetValueOrDefault("Value", new Dictionary()).AsGodotDictionary();
						towerDefenseLevelEventBase.Init(valueDictionary);
						eventList.Add(towerDefenseLevelEventBase);
					}
				}
			}
			catch (Exception value)
			{
				GD.PushError($"[WaveConfig] Failed to load Event[{i}]: {value}");
			}
		}
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["DynamicPlantfood"] = dynamicPlantfood,
			["Spawn"] = new Godot.Collections.Array(),
			["GridSpawn"] = new Godot.Collections.Array(),
			["Dynamic"] = (GodotObject.IsInstanceValid(dynamic) ? dynamic.Export() : new Dictionary()),
			["Event"] = new Godot.Collections.Array()
		};
		foreach (TowerDefenseLevelSpawnConfig item in spawn)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				((Godot.Collections.Array)dictionary["Spawn"]).Add(item.Export());
			}
		}
		foreach (TowerDefenseLevelGridSpawnConfig item2 in gridSpawn)
		{
			if (GodotObject.IsInstanceValid(item2))
			{
				((Godot.Collections.Array)dictionary["GridSpawn"]).Add(item2.Export());
			}
		}
		for (int i = 0; i < eventList.Count; i++)
		{
			try
			{
				if (GodotObject.IsInstanceValid(eventList[i]))
				{
					((Godot.Collections.Array)dictionary["Event"]).Add(eventList[i].Export());
				}
			}
			catch (Exception value)
			{
				GD.PushError($"[WaveConfig] Failed to export Event[{i}]: {value}");
			}
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
				new PropertyInfo(Variant.Type.Dictionary, "waveDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (name == PropertyName.dynamicPlantfood)
		{
			dynamicPlantfood = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.spawn)
		{
			spawn = VariantUtils.ConvertToArray<TowerDefenseLevelSpawnConfig>(in value);
			return true;
		}
		if (name == PropertyName.gridSpawn)
		{
			gridSpawn = VariantUtils.ConvertToArray<TowerDefenseLevelGridSpawnConfig>(in value);
			return true;
		}
		if (name == PropertyName.dynamic)
		{
			dynamic = VariantUtils.ConvertTo<TowerDefenseLevelSpawnDynamicConfig>(in value);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseLevelEventBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.dynamicPlantfood)
		{
			value = VariantUtils.CreateFromArray(dynamicPlantfood);
			return true;
		}
		if (name == PropertyName.spawn)
		{
			value = VariantUtils.CreateFromArray(spawn);
			return true;
		}
		if (name == PropertyName.gridSpawn)
		{
			value = VariantUtils.CreateFromArray(gridSpawn);
			return true;
		}
		if (name == PropertyName.dynamic)
		{
			value = VariantUtils.CreateFrom(in dynamic);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.dynamicPlantfood, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.spawn, PropertyHint.TypeString, "24/17:TowerDefenseLevelSpawnConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.gridSpawn, PropertyHint.TypeString, "24/17:TowerDefenseLevelGridSpawnConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.dynamic, PropertyHint.ResourceType, "TowerDefenseLevelSpawnDynamicConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseLevelEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dynamicPlantfood, Variant.CreateFrom(dynamicPlantfood));
		info.AddProperty(PropertyName.spawn, Variant.CreateFrom(spawn));
		info.AddProperty(PropertyName.gridSpawn, Variant.CreateFrom(gridSpawn));
		info.AddProperty(PropertyName.dynamic, Variant.From(in dynamic));
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dynamicPlantfood, out var value))
		{
			dynamicPlantfood = value.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.spawn, out var value2))
		{
			spawn = value2.AsGodotArray<TowerDefenseLevelSpawnConfig>();
		}
		if (info.TryGetProperty(PropertyName.gridSpawn, out var value3))
		{
			gridSpawn = value3.AsGodotArray<TowerDefenseLevelGridSpawnConfig>();
		}
		if (info.TryGetProperty(PropertyName.dynamic, out var value4))
		{
			dynamic = value4.As<TowerDefenseLevelSpawnDynamicConfig>();
		}
		if (info.TryGetProperty(PropertyName.eventList, out var value5))
		{
			eventList = value5.AsGodotArray<TowerDefenseLevelEventBase>();
		}
	}
}
