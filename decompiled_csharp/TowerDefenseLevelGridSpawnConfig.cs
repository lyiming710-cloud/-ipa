using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Wave/Resource/TowerDefenseLevelGridSpawnConfig.cs")]
public class TowerDefenseLevelGridSpawnConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public new static readonly StringName _Set = "_Set";

		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName @override = "override";

		public static readonly StringName packet = "packet";

		public static readonly StringName gridPos = "gridPos";

		public static readonly StringName overrideVal = "overrideVal";

		public static readonly StringName spawnEvent = "spawnEvent";

		public static readonly StringName dieEvent = "dieEvent";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string packet = "";

	[Export(PropertyHint.None, "")]
	public Vector2I gridPos;

	public TowerDefenseCharacterOverride overrideVal;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> spawnEvent = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> dieEvent = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public TowerDefenseCharacterOverride @override
	{
		get
		{
			return overrideVal;
		}
		set
		{
			overrideVal = value;
		}
	}

	public override bool _Set(StringName property, Variant value)
	{
		if (property.ToString() == "overrideVal")
		{
			overrideVal = value.As<TowerDefenseCharacterOverride>();
			return true;
		}
		return false;
	}

	public void Init(Dictionary spawnDictionary)
	{
		packet = spawnDictionary.GetValueOrDefault("Packet", "").AsString();
		Array array = (spawnDictionary.ContainsKey("GridPos") ? ((Array)spawnDictionary["GridPos"]) : new Array { 0, 0 });
		gridPos = new Vector2I(array[0].AsInt32(), array[1].AsInt32());
		foreach (Variant item in spawnDictionary.ContainsKey("SpawnEvent") ? ((Array)spawnDictionary["SpawnEvent"]) : new Array())
		{
			Dictionary dictionary = item.AsGodotDictionary();
			string text = dictionary.GetValueOrDefault("EventName", "").AsString();
			if (text != "")
			{
				TowerDefenseCharacterEventBase towerDefenseCharacterEventBase = TowerDefenseCharacterEventMathine.EventGet(text);
				Dictionary valueDictionary = (dictionary.ContainsKey("Value") ? dictionary["Value"].AsGodotDictionary() : new Dictionary());
				towerDefenseCharacterEventBase.Init(valueDictionary);
				spawnEvent.Add(towerDefenseCharacterEventBase);
			}
		}
		foreach (Variant item2 in spawnDictionary.ContainsKey("DieEvent") ? ((Array)spawnDictionary["DieEvent"]) : new Array())
		{
			Dictionary dictionary2 = item2.AsGodotDictionary();
			string text2 = dictionary2.GetValueOrDefault("EventName", "").AsString();
			if (text2 != "")
			{
				TowerDefenseCharacterEventBase towerDefenseCharacterEventBase2 = TowerDefenseCharacterEventMathine.EventGet(text2);
				Dictionary valueDictionary2 = (dictionary2.ContainsKey("Value") ? dictionary2["Value"].AsGodotDictionary() : new Dictionary());
				towerDefenseCharacterEventBase2.Init(valueDictionary2);
				dieEvent.Add(towerDefenseCharacterEventBase2);
			}
		}
		Dictionary dictionary3 = (spawnDictionary.ContainsKey("Override") ? spawnDictionary["Override"].AsGodotDictionary() : new Dictionary());
		if (dictionary3.Count > 0)
		{
			overrideVal = new TowerDefenseCharacterOverride();
			overrideVal.Init(dictionary3);
		}
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["Packet"] = packet,
			["GridPos"] = new Array { gridPos.X, gridPos.Y },
			["SpawnEvent"] = new Array(),
			["DieEvent"] = new Array()
		};
		if (GodotObject.IsInstanceValid(overrideVal))
		{
			dictionary["Override"] = overrideVal.Export();
		}
		foreach (TowerDefenseCharacterEventBase item in spawnEvent)
		{
			((Array)dictionary["SpawnEvent"]).Add(item.Export());
		}
		foreach (TowerDefenseCharacterEventBase item2 in dieEvent)
		{
			((Array)dictionary["DieEvent"]).Add(item2.Export());
		}
		return dictionary;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "spawnDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Set && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
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
		if (method == MethodName._Set)
		{
			return true;
		}
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
		if (name == PropertyName.@override)
		{
			@override = VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in value);
			return true;
		}
		if (name == PropertyName.packet)
		{
			packet = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			gridPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.overrideVal)
		{
			overrideVal = VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in value);
			return true;
		}
		if (name == PropertyName.spawnEvent)
		{
			spawnEvent = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.dieEvent)
		{
			dieEvent = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.@override)
		{
			value = VariantUtils.CreateFrom<TowerDefenseCharacterOverride>(@override);
			return true;
		}
		if (name == PropertyName.packet)
		{
			value = VariantUtils.CreateFrom(in packet);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			value = VariantUtils.CreateFrom(in gridPos);
			return true;
		}
		if (name == PropertyName.overrideVal)
		{
			value = VariantUtils.CreateFrom(in overrideVal);
			return true;
		}
		if (name == PropertyName.spawnEvent)
		{
			value = VariantUtils.CreateFromArray(spawnEvent);
			return true;
		}
		if (name == PropertyName.dieEvent)
		{
			value = VariantUtils.CreateFromArray(dieEvent);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.packet, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridPos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.overrideVal, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.@override, PropertyHint.ResourceType, "TowerDefenseCharacterOverride", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.spawnEvent, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.dieEvent, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.@override, Variant.From<TowerDefenseCharacterOverride>(@override));
		info.AddProperty(PropertyName.packet, Variant.From(in packet));
		info.AddProperty(PropertyName.gridPos, Variant.From(in gridPos));
		info.AddProperty(PropertyName.overrideVal, Variant.From(in overrideVal));
		info.AddProperty(PropertyName.spawnEvent, Variant.CreateFrom(spawnEvent));
		info.AddProperty(PropertyName.dieEvent, Variant.CreateFrom(dieEvent));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.@override, out var value))
		{
			@override = value.As<TowerDefenseCharacterOverride>();
		}
		if (info.TryGetProperty(PropertyName.packet, out var value2))
		{
			packet = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.gridPos, out var value3))
		{
			gridPos = value3.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.overrideVal, out var value4))
		{
			overrideVal = value4.As<TowerDefenseCharacterOverride>();
		}
		if (info.TryGetProperty(PropertyName.spawnEvent, out var value5))
		{
			spawnEvent = value5.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.dieEvent, out var value6))
		{
			dieEvent = value6.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
	}
}
