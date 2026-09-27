using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PacketBank/Resource/Packet/Override/TowerDefensePacketOverride.cs")]
public class TowerDefensePacketOverride : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public new static readonly StringName _Set = "_Set";

		public static readonly StringName ImportActions = "ImportActions";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName type = "type";

		public static readonly StringName costRise = "costRise";

		public static readonly StringName cost = "cost";

		public static readonly StringName costMultiple = "costMultiple";

		public static readonly StringName packetCooldown = "packetCooldown";

		public static readonly StringName startingCooldown = "startingCooldown";

		public static readonly StringName weight = "weight";

		public static readonly StringName wavePointCost = "wavePointCost";

		public static readonly StringName pressedActions = "pressedActions";

		public static readonly StringName useSucceededActions = "useSucceededActions";

		public static readonly StringName plantCover = "plantCover";

		public static readonly StringName characterOverride = "characterOverride";

		public static readonly StringName islimitGridNum = "islimitGridNum";

		public static readonly StringName coverCanDirectPlant = "coverCanDirectPlant";

		public static readonly StringName hypnoses = "hypnoses";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[ExportCategory("Total")]
	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.PACKET_TYPE type = TowerDefenseEnum.PACKET_TYPE.NOONE;

	[ExportCategory("Total")]
	[Export(PropertyHint.None, "")]
	public int costRise = -1;

	[Export(PropertyHint.None, "")]
	public int cost = -1;

	[Export(PropertyHint.None, "")]
	public double costMultiple = -1.0;

	[Export(PropertyHint.None, "")]
	public double packetCooldown = -1.0;

	[Export(PropertyHint.None, "")]
	public double startingCooldown = -1.0;

	[ExportCategory("Zombie")]
	[Export(PropertyHint.None, "")]
	public int weight = -1;

	[Export(PropertyHint.None, "")]
	public int wavePointCost = -1;

	[ExportCategory("Action Behavior")]
	[Export(PropertyHint.None, "")]
	public Array<CardActionBehaviorDefinition> pressedActions = new Array<CardActionBehaviorDefinition>();

	[Export(PropertyHint.None, "")]
	public Array<CardActionBehaviorDefinition> useSucceededActions = new Array<CardActionBehaviorDefinition>();

	[ExportCategory("Character")]
	[Export(PropertyHint.None, "")]
	public Array<string> plantCover = new Array<string>();

	[Export(PropertyHint.None, "")]
	public TowerDefenseCharacterOverride characterOverride = new TowerDefenseCharacterOverride();

	[Export(PropertyHint.None, "")]
	public bool islimitGridNum = true;

	[Export(PropertyHint.None, "")]
	public bool coverCanDirectPlant;

	[Export(PropertyHint.None, "")]
	public bool hypnoses;

	public void Init(Dictionary data)
	{
		if (Enum.TryParse<TowerDefenseEnum.PACKET_TYPE>(data.GetValueOrDefault("Type", "NOONE").AsString().ToUpper(), out var result))
		{
			type = result;
		}
		costRise = data.GetValueOrDefault("CostRise", -1).AsInt32();
		cost = data.GetValueOrDefault("Cost", data.GetValueOrDefault("cost", -1)).AsInt32();
		costMultiple = data.GetValueOrDefault("CostMultiple", -1.0).AsDouble();
		packetCooldown = data.GetValueOrDefault("PacketCooldown", -1.0).AsDouble();
		startingCooldown = data.GetValueOrDefault("StartingCooldown", -1.0).AsDouble();
		weight = data.GetValueOrDefault("Weight", -1).AsInt32();
		wavePointCost = data.GetValueOrDefault("WavePointCost", -1).AsInt32();
		plantCover = new Array<string>();
		foreach (Variant item in data.GetValueOrDefault("PlantCover", new Godot.Collections.Array()).AsGodotArray())
		{
			plantCover.Add(item.AsString());
		}
		pressedActions = ImportActions(data, "PressedActions", "EventPress", CardActionBehaviorTrigger.Pressed);
		useSucceededActions = ImportActions(data, "UseSucceededActions", "EventPlant", CardActionBehaviorTrigger.UseSucceeded);
		characterOverride = new TowerDefenseCharacterOverride();
		characterOverride.Init(data.GetValueOrDefault("CharacterOverride", new Dictionary()).AsGodotDictionary());
		islimitGridNum = data.GetValueOrDefault("IslimitGridNum", true).AsBool();
		coverCanDirectPlant = data.GetValueOrDefault("CoverCanDirectPlant", false).AsBool();
		hypnoses = data.GetValueOrDefault("Hypnoses", false).AsBool();
	}

	public Dictionary Export()
	{
		if (plantCover == null)
		{
			plantCover = new Array<string>();
		}
		if (pressedActions == null)
		{
			pressedActions = new Array<CardActionBehaviorDefinition>();
		}
		if (useSucceededActions == null)
		{
			useSucceededActions = new Array<CardActionBehaviorDefinition>();
		}
		Dictionary dictionary = (GodotObject.IsInstanceValid(characterOverride) ? characterOverride.Export() : new Dictionary());
		Dictionary dictionary2 = new Dictionary
		{
			["Type"] = Enum.GetName(typeof(TowerDefenseEnum.PACKET_TYPE), type),
			["CostRise"] = costRise,
			["Cost"] = cost,
			["CostMultiple"] = costMultiple,
			["PacketCooldown"] = packetCooldown,
			["StartingCooldown"] = startingCooldown,
			["Weight"] = weight,
			["WavePointCost"] = wavePointCost,
			["PressedActions"] = new Godot.Collections.Array(),
			["UseSucceededActions"] = new Godot.Collections.Array(),
			["PlantCover"] = plantCover,
			["CharacterOverride"] = dictionary,
			["IslimitGridNum"] = islimitGridNum,
			["CoverCanDirectPlant"] = coverCanDirectPlant,
			["Hypnoses"] = hypnoses
		};
		foreach (CardActionBehaviorDefinition pressedAction in pressedActions)
		{
			if (GodotObject.IsInstanceValid(pressedAction))
			{
				((Godot.Collections.Array)dictionary2["PressedActions"]).Add(pressedAction.ExportConfiguration());
			}
		}
		foreach (CardActionBehaviorDefinition useSucceededAction in useSucceededActions)
		{
			if (GodotObject.IsInstanceValid(useSucceededAction))
			{
				((Godot.Collections.Array)dictionary2["UseSucceededActions"]).Add(useSucceededAction.ExportConfiguration());
			}
		}
		return dictionary2;
	}

	public override bool _Set(StringName property, Variant value)
	{
		string text = property.ToString();
		if (!(text == "eventPress"))
		{
			if (text == "eventPlant")
			{
				useSucceededActions = CardActionBehaviorDefinition.ReadLegacyArray(value, CardActionBehaviorTrigger.UseSucceeded);
				return true;
			}
			return false;
		}
		pressedActions = CardActionBehaviorDefinition.ReadLegacyArray(value, CardActionBehaviorTrigger.Pressed);
		return true;
	}

	private static Array<CardActionBehaviorDefinition> ImportActions(Dictionary data, string currentKey, string legacyKey, CardActionBehaviorTrigger trigger)
	{
		Array<CardActionBehaviorDefinition> array = new Array<CardActionBehaviorDefinition>();
		foreach (Variant item in data.GetValueOrDefault(currentKey, data.GetValueOrDefault(legacyKey, new Godot.Collections.Array())).AsGodotArray())
		{
			Dictionary dictionary = item.AsGodotDictionary();
			CardActionBehaviorDefinition cardActionBehaviorDefinition = CardActionBehaviorFactory.Create(dictionary.GetValueOrDefault("ActionId", dictionary.GetValueOrDefault("EventName", "")).AsString());
			if (GodotObject.IsInstanceValid(cardActionBehaviorDefinition))
			{
				Dictionary data2 = dictionary.GetValueOrDefault("Configuration", dictionary.GetValueOrDefault("Value", new Dictionary())).AsGodotDictionary();
				cardActionBehaviorDefinition.ImportConfiguration(data2);
				cardActionBehaviorDefinition.triggerFlags = (int)trigger;
				array.Add(cardActionBehaviorDefinition);
			}
		}
		return array;
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
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportActions, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "currentKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "legacyKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "trigger", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Set && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.ImportActions && args.Count == 4)
		{
			Array<CardActionBehaviorDefinition> array = ImportActions(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<CardActionBehaviorTrigger>(in args[3]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ImportActions && args.Count == 4)
		{
			Array<CardActionBehaviorDefinition> array = ImportActions(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<CardActionBehaviorTrigger>(in args[3]));
			ret = VariantUtils.CreateFromArray(array);
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
		if (method == MethodName._Set)
		{
			return true;
		}
		if (method == MethodName.ImportActions)
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
			type = VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in value);
			return true;
		}
		if (name == PropertyName.costRise)
		{
			costRise = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.cost)
		{
			cost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.costMultiple)
		{
			costMultiple = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.packetCooldown)
		{
			packetCooldown = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.startingCooldown)
		{
			startingCooldown = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.weight)
		{
			weight = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.wavePointCost)
		{
			wavePointCost = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.pressedActions)
		{
			pressedActions = VariantUtils.ConvertToArray<CardActionBehaviorDefinition>(in value);
			return true;
		}
		if (name == PropertyName.useSucceededActions)
		{
			useSucceededActions = VariantUtils.ConvertToArray<CardActionBehaviorDefinition>(in value);
			return true;
		}
		if (name == PropertyName.plantCover)
		{
			plantCover = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.characterOverride)
		{
			characterOverride = VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in value);
			return true;
		}
		if (name == PropertyName.islimitGridNum)
		{
			islimitGridNum = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.coverCanDirectPlant)
		{
			coverCanDirectPlant = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hypnoses)
		{
			hypnoses = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.costRise)
		{
			value = VariantUtils.CreateFrom(in costRise);
			return true;
		}
		if (name == PropertyName.cost)
		{
			value = VariantUtils.CreateFrom(in cost);
			return true;
		}
		if (name == PropertyName.costMultiple)
		{
			value = VariantUtils.CreateFrom(in costMultiple);
			return true;
		}
		if (name == PropertyName.packetCooldown)
		{
			value = VariantUtils.CreateFrom(in packetCooldown);
			return true;
		}
		if (name == PropertyName.startingCooldown)
		{
			value = VariantUtils.CreateFrom(in startingCooldown);
			return true;
		}
		if (name == PropertyName.weight)
		{
			value = VariantUtils.CreateFrom(in weight);
			return true;
		}
		if (name == PropertyName.wavePointCost)
		{
			value = VariantUtils.CreateFrom(in wavePointCost);
			return true;
		}
		if (name == PropertyName.pressedActions)
		{
			value = VariantUtils.CreateFromArray(pressedActions);
			return true;
		}
		if (name == PropertyName.useSucceededActions)
		{
			value = VariantUtils.CreateFromArray(useSucceededActions);
			return true;
		}
		if (name == PropertyName.plantCover)
		{
			value = VariantUtils.CreateFromArray(plantCover);
			return true;
		}
		if (name == PropertyName.characterOverride)
		{
			value = VariantUtils.CreateFrom(in characterOverride);
			return true;
		}
		if (name == PropertyName.islimitGridNum)
		{
			value = VariantUtils.CreateFrom(in islimitGridNum);
			return true;
		}
		if (name == PropertyName.coverCanDirectPlant)
		{
			value = VariantUtils.CreateFrom(in coverCanDirectPlant);
			return true;
		}
		if (name == PropertyName.hypnoses)
		{
			value = VariantUtils.CreateFrom(in hypnoses);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Total", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.type, PropertyHint.Enum, "NOONE:-1,WHITE:0,GOLD:1,DIAMOND:2,COLOUR:3,STAR:4,ORIGINAL:5,ZOMBIE:6,COVER:7,GRAY:8", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Total", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.costRise, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.cost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.costMultiple, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.packetCooldown, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.startingCooldown, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Zombie", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.weight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.wavePointCost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Action Behavior", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.pressedActions, PropertyHint.TypeString, "24/17:CardActionBehaviorDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.useSucceededActions, PropertyHint.TypeString, "24/17:CardActionBehaviorDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Character", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.plantCover, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterOverride, PropertyHint.ResourceType, "TowerDefenseCharacterOverride", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.islimitGridNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.coverCanDirectPlant, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hypnoses, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.type, Variant.From(in type));
		info.AddProperty(PropertyName.costRise, Variant.From(in costRise));
		info.AddProperty(PropertyName.cost, Variant.From(in cost));
		info.AddProperty(PropertyName.costMultiple, Variant.From(in costMultiple));
		info.AddProperty(PropertyName.packetCooldown, Variant.From(in packetCooldown));
		info.AddProperty(PropertyName.startingCooldown, Variant.From(in startingCooldown));
		info.AddProperty(PropertyName.weight, Variant.From(in weight));
		info.AddProperty(PropertyName.wavePointCost, Variant.From(in wavePointCost));
		info.AddProperty(PropertyName.pressedActions, Variant.CreateFrom(pressedActions));
		info.AddProperty(PropertyName.useSucceededActions, Variant.CreateFrom(useSucceededActions));
		info.AddProperty(PropertyName.plantCover, Variant.CreateFrom(plantCover));
		info.AddProperty(PropertyName.characterOverride, Variant.From(in characterOverride));
		info.AddProperty(PropertyName.islimitGridNum, Variant.From(in islimitGridNum));
		info.AddProperty(PropertyName.coverCanDirectPlant, Variant.From(in coverCanDirectPlant));
		info.AddProperty(PropertyName.hypnoses, Variant.From(in hypnoses));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.type, out var value))
		{
			type = value.As<TowerDefenseEnum.PACKET_TYPE>();
		}
		if (info.TryGetProperty(PropertyName.costRise, out var value2))
		{
			costRise = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.cost, out var value3))
		{
			cost = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.costMultiple, out var value4))
		{
			costMultiple = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.packetCooldown, out var value5))
		{
			packetCooldown = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.startingCooldown, out var value6))
		{
			startingCooldown = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.weight, out var value7))
		{
			weight = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.wavePointCost, out var value8))
		{
			wavePointCost = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.pressedActions, out var value9))
		{
			pressedActions = value9.AsGodotArray<CardActionBehaviorDefinition>();
		}
		if (info.TryGetProperty(PropertyName.useSucceededActions, out var value10))
		{
			useSucceededActions = value10.AsGodotArray<CardActionBehaviorDefinition>();
		}
		if (info.TryGetProperty(PropertyName.plantCover, out var value11))
		{
			plantCover = value11.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.characterOverride, out var value12))
		{
			characterOverride = value12.As<TowerDefenseCharacterOverride>();
		}
		if (info.TryGetProperty(PropertyName.islimitGridNum, out var value13))
		{
			islimitGridNum = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.coverCanDirectPlant, out var value14))
		{
			coverCanDirectPlant = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hypnoses, out var value15))
		{
			hypnoses = value15.As<bool>();
		}
	}
}
