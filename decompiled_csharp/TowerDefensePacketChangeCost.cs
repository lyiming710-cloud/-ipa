using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Registry/Behavior/Card/Cost/TowerDefensePacketChangeCost.cs")]
public class TowerDefensePacketChangeCost : CardBehaviorDefinition
{
	public new class MethodName : CardBehaviorDefinition.MethodName
	{
		public static readonly StringName Execute = "Execute";

		public static readonly StringName ExportSave = "ExportSave";

		public static readonly StringName ImportSave = "ImportSave";
	}

	public new class PropertyName : CardBehaviorDefinition.PropertyName
	{
		public static readonly StringName method = "method";

		public static readonly StringName amontDictionary = "amontDictionary";

		public static readonly StringName key = "key";

		public static readonly StringName lockCost = "lockCost";

		public static readonly StringName skip = "skip";

		public static readonly StringName consumeOnPurchase = "consumeOnPurchase";
	}

	public new class SignalName : CardBehaviorDefinition.SignalName
	{
	}

	[Export(PropertyHint.Enum, "Increase,Decrease,Set")]
	public string method = "Increase";

	[Export(PropertyHint.None, "")]
	public Dictionary amontDictionary = new Dictionary
	{
		[0] = 25,
		[1] = 25,
		[2] = 25,
		[3] = 25,
		[4] = 25,
		[5] = 25,
		[6] = 25,
		[7] = 25,
		[8] = 25
	};

	[Export(PropertyHint.None, "")]
	public string key = "";

	[Export(PropertyHint.None, "")]
	public bool lockCost;

	[Export(PropertyHint.None, "")]
	public bool skip;

	[Export(PropertyHint.None, "")]
	public bool consumeOnPurchase;

	public int Execute(int num, TowerDefensePacketConfig packet)
	{
		CardCostContext context = new CardCostContext(packet, num);
		ModifyCost(ref context);
		return context.Cost;
	}

	public override void ModifyCost(ref CardCostContext context)
	{
		switch (method)
		{
		case "Increase":
			if (context.CanIncrease)
			{
				int type2 = (int)context.Packet.type;
				if (amontDictionary.ContainsKey(type2))
				{
					context.Cost += (int)amontDictionary[type2];
				}
			}
			break;
		case "Decrease":
		{
			int type3 = (int)context.Packet.type;
			if (amontDictionary.ContainsKey(type3))
			{
				context.Cost -= (int)amontDictionary[type3];
			}
			break;
		}
		case "Set":
		{
			int type = (int)context.Packet.type;
			if (amontDictionary.ContainsKey(type))
			{
				context.Cost = (int)amontDictionary[type];
			}
			break;
		}
		}
		if (lockCost)
		{
			context.CanIncrease = false;
		}
		if (skip)
		{
			context.StopCurrentStage = true;
		}
	}

	public Dictionary ExportSave()
	{
		Dictionary dictionary = new Dictionary();
		foreach (Variant key in amontDictionary.Keys)
		{
			dictionary[key] = amontDictionary[key];
		}
		return new Dictionary
		{
			["method"] = method,
			["amontDictionary"] = dictionary,
			["key"] = this.key,
			["lockCost"] = lockCost,
			["skip"] = skip,
			["consumeOnPurchase"] = consumeOnPurchase
		};
	}

	public static TowerDefensePacketChangeCost ImportSave(Dictionary data)
	{
		TowerDefensePacketChangeCost towerDefensePacketChangeCost = new TowerDefensePacketChangeCost();
		towerDefensePacketChangeCost.method = (data.ContainsKey("method") ? ((string)data["method"]) : "Increase");
		Dictionary dictionary = (data.ContainsKey("amontDictionary") ? ((Dictionary)data["amontDictionary"]) : new Dictionary());
		foreach (Variant key in dictionary.Keys)
		{
			towerDefensePacketChangeCost.amontDictionary[key] = dictionary[key];
		}
		towerDefensePacketChangeCost.key = (data.ContainsKey("key") ? ((string)data["key"]) : "");
		towerDefensePacketChangeCost.lockCost = data.ContainsKey("lockCost") && (bool)data["lockCost"];
		towerDefensePacketChangeCost.skip = data.ContainsKey("skip") && (bool)data["skip"];
		towerDefensePacketChangeCost.consumeOnPurchase = data.GetValueOrDefault("consumeOnPurchase", false).AsBool();
		return towerDefensePacketChangeCost;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExportSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportSave, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Execute && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(Execute(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.ExportSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportSave());
			return true;
		}
		if (method == MethodName.ImportSave && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketChangeCost>(ImportSave(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ImportSave && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePacketChangeCost>(ImportSave(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.ExportSave)
		{
			return true;
		}
		if (method == MethodName.ImportSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.method)
		{
			method = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.amontDictionary)
		{
			amontDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.key)
		{
			key = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.lockCost)
		{
			lockCost = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.skip)
		{
			skip = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.consumeOnPurchase)
		{
			consumeOnPurchase = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.method)
		{
			value = VariantUtils.CreateFrom(in method);
			return true;
		}
		if (name == PropertyName.amontDictionary)
		{
			value = VariantUtils.CreateFrom(in amontDictionary);
			return true;
		}
		if (name == PropertyName.key)
		{
			value = VariantUtils.CreateFrom(in key);
			return true;
		}
		if (name == PropertyName.lockCost)
		{
			value = VariantUtils.CreateFrom(in lockCost);
			return true;
		}
		if (name == PropertyName.skip)
		{
			value = VariantUtils.CreateFrom(in skip);
			return true;
		}
		if (name == PropertyName.consumeOnPurchase)
		{
			value = VariantUtils.CreateFrom(in consumeOnPurchase);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.method, PropertyHint.Enum, "Increase,Decrease,Set", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.amontDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.key, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.lockCost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.skip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.consumeOnPurchase, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.method, Variant.From(in method));
		info.AddProperty(PropertyName.amontDictionary, Variant.From(in amontDictionary));
		info.AddProperty(PropertyName.key, Variant.From(in key));
		info.AddProperty(PropertyName.lockCost, Variant.From(in lockCost));
		info.AddProperty(PropertyName.skip, Variant.From(in skip));
		info.AddProperty(PropertyName.consumeOnPurchase, Variant.From(in consumeOnPurchase));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.method, out var value))
		{
			method = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.amontDictionary, out var value2))
		{
			amontDictionary = value2.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.key, out var value3))
		{
			key = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.lockCost, out var value4))
		{
			lockCost = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.skip, out var value5))
		{
			skip = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.consumeOnPurchase, out var value6))
		{
			consumeOnPurchase = value6.As<bool>();
		}
	}
}
