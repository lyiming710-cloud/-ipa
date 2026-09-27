using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/Spawn/TowerDefenseLevelSpawnConfig.cs")]
public class TowerDefenseLevelSpawnConfig : Resource
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

		public static readonly StringName zombie = "zombie";

		public static readonly StringName line = "line";

		public static readonly StringName num = "num";

		public static readonly StringName overrideVal = "overrideVal";

		public static readonly StringName spawnEvent = "spawnEvent";

		public static readonly StringName dieEvent = "dieEvent";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string zombie = "";

	[Export(PropertyHint.None, "")]
	public int line = -1;

	[Export(PropertyHint.None, "")]
	public int num = 1;

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
		zombie = spawnDictionary.GetValueOrDefault("Zombie", "").AsString();
		line = spawnDictionary.GetValueOrDefault("Line", -1).AsInt32();
		num = spawnDictionary.GetValueOrDefault("Num", 1).AsInt32();
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
			["Zombie"] = zombie,
			["Line"] = line,
			["Num"] = num,
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
		if (name == PropertyName.zombie)
		{
			zombie = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.line)
		{
			line = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName.zombie)
		{
			value = VariantUtils.CreateFrom(in zombie);
			return true;
		}
		if (name == PropertyName.line)
		{
			value = VariantUtils.CreateFrom(in line);
			return true;
		}
		if (name == PropertyName.num)
		{
			value = VariantUtils.CreateFrom(in num);
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
			new PropertyInfo(Variant.Type.String, PropertyName.zombie, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.line, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
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
		info.AddProperty(PropertyName.zombie, Variant.From(in zombie));
		info.AddProperty(PropertyName.line, Variant.From(in line));
		info.AddProperty(PropertyName.num, Variant.From(in num));
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
		if (info.TryGetProperty(PropertyName.zombie, out var value2))
		{
			zombie = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.line, out var value3))
		{
			line = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.num, out var value4))
		{
			num = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.overrideVal, out var value5))
		{
			overrideVal = value5.As<TowerDefenseCharacterOverride>();
		}
		if (info.TryGetProperty(PropertyName.spawnEvent, out var value6))
		{
			spawnEvent = value6.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.dieEvent, out var value7))
		{
			dieEvent = value7.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
	}
}
