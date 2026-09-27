using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Process/Vase/Resource/TowerDefenseLevelVaseManagerConfig.cs")]
public class TowerDefenseLevelVaseManagerConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName shuffle = "shuffle";

		public static readonly StringName mowerUse = "mowerUse";

		public static readonly StringName packetBankMethod = "packetBankMethod";

		public static readonly StringName packetBankExitDelay = "packetBankExitDelay";

		public static readonly StringName vaseList = "vaseList";

		public static readonly StringName vaseFillList = "vaseFillList";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool shuffle = true;

	[Export(PropertyHint.None, "")]
	public bool mowerUse;

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.LEVEL_SEEDBANK_METHOD packetBankMethod;

	[Export(PropertyHint.Range, "0,5,0.05")]
	public double packetBankExitDelay = 0.5;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelVaseConfig> vaseList = new Array<TowerDefenseLevelVaseConfig>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelVaseFillConfig> vaseFillList = new Array<TowerDefenseLevelVaseFillConfig>();

	public void Init(Dictionary data)
	{
		vaseList.Clear();
		vaseFillList.Clear();
		shuffle = data.GetValueOrDefault("Shuffle", false).AsBool();
		mowerUse = data.GetValueOrDefault("MowerUse", false).AsBool();
		Variant valueOrDefault = data.GetValueOrDefault("PacketBankMethod", 0);
		packetBankMethod = ((valueOrDefault.VariantType == Variant.Type.String && Enum.TryParse<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(valueOrDefault.AsString(), ignoreCase: true, out var result)) ? result : ((TowerDefenseEnum.LEVEL_SEEDBANK_METHOD)valueOrDefault.AsInt32()));
		double num = data.GetValueOrDefault("PacketBankExitDelay", 0.5).AsDouble();
		packetBankExitDelay = (double.IsFinite(num) ? Math.Clamp(num, 0.0, 5.0) : 0.5);
		foreach (Variant item in data.ContainsKey("Vase") ? ((Godot.Collections.Array)data["Vase"]) : new Godot.Collections.Array())
		{
			Dictionary data2 = item.AsGodotDictionary();
			TowerDefenseLevelVaseConfig towerDefenseLevelVaseConfig = new TowerDefenseLevelVaseConfig();
			towerDefenseLevelVaseConfig.Init(data2);
			vaseList.Add(towerDefenseLevelVaseConfig);
		}
		foreach (Variant item2 in data.ContainsKey("VaseFill") ? ((Godot.Collections.Array)data["VaseFill"]) : new Godot.Collections.Array())
		{
			Dictionary data3 = item2.AsGodotDictionary();
			TowerDefenseLevelVaseFillConfig towerDefenseLevelVaseFillConfig = new TowerDefenseLevelVaseFillConfig();
			towerDefenseLevelVaseFillConfig.Init(data3);
			vaseFillList.Add(towerDefenseLevelVaseFillConfig);
		}
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["Shuffle"] = shuffle,
			["MowerUse"] = mowerUse,
			["PacketBankMethod"] = (int)packetBankMethod,
			["PacketBankExitDelay"] = packetBankExitDelay,
			["Vase"] = new Godot.Collections.Array(),
			["VaseFill"] = new Godot.Collections.Array()
		};
		foreach (TowerDefenseLevelVaseConfig vase in vaseList)
		{
			((Godot.Collections.Array)dictionary["Vase"]).Add(vase.Export());
		}
		foreach (TowerDefenseLevelVaseFillConfig vaseFill in vaseFillList)
		{
			((Godot.Collections.Array)dictionary["VaseFill"]).Add(vaseFill.Export());
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
		if (name == PropertyName.shuffle)
		{
			shuffle = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mowerUse)
		{
			mowerUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packetBankMethod)
		{
			packetBankMethod = VariantUtils.ConvertTo<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(in value);
			return true;
		}
		if (name == PropertyName.packetBankExitDelay)
		{
			packetBankExitDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.vaseList)
		{
			vaseList = VariantUtils.ConvertToArray<TowerDefenseLevelVaseConfig>(in value);
			return true;
		}
		if (name == PropertyName.vaseFillList)
		{
			vaseFillList = VariantUtils.ConvertToArray<TowerDefenseLevelVaseFillConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.shuffle)
		{
			value = VariantUtils.CreateFrom(in shuffle);
			return true;
		}
		if (name == PropertyName.mowerUse)
		{
			value = VariantUtils.CreateFrom(in mowerUse);
			return true;
		}
		if (name == PropertyName.packetBankMethod)
		{
			value = VariantUtils.CreateFrom(in packetBankMethod);
			return true;
		}
		if (name == PropertyName.packetBankExitDelay)
		{
			value = VariantUtils.CreateFrom(in packetBankExitDelay);
			return true;
		}
		if (name == PropertyName.vaseList)
		{
			value = VariantUtils.CreateFromArray(vaseList);
			return true;
		}
		if (name == PropertyName.vaseFillList)
		{
			value = VariantUtils.CreateFromArray(vaseFillList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.shuffle, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mowerUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.packetBankMethod, PropertyHint.Enum, "NOONE,CHOOSE,PRESET,CONVEYOR,RAIN", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.packetBankExitDelay, PropertyHint.Range, "0,5,0.05", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.vaseList, PropertyHint.TypeString, "24/17:TowerDefenseLevelVaseConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.vaseFillList, PropertyHint.TypeString, "24/17:TowerDefenseLevelVaseFillConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.shuffle, Variant.From(in shuffle));
		info.AddProperty(PropertyName.mowerUse, Variant.From(in mowerUse));
		info.AddProperty(PropertyName.packetBankMethod, Variant.From(in packetBankMethod));
		info.AddProperty(PropertyName.packetBankExitDelay, Variant.From(in packetBankExitDelay));
		info.AddProperty(PropertyName.vaseList, Variant.CreateFrom(vaseList));
		info.AddProperty(PropertyName.vaseFillList, Variant.CreateFrom(vaseFillList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.shuffle, out var value))
		{
			shuffle = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mowerUse, out var value2))
		{
			mowerUse = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packetBankMethod, out var value3))
		{
			packetBankMethod = value3.As<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>();
		}
		if (info.TryGetProperty(PropertyName.packetBankExitDelay, out var value4))
		{
			packetBankExitDelay = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.vaseList, out var value5))
		{
			vaseList = value5.AsGodotArray<TowerDefenseLevelVaseConfig>();
		}
		if (info.TryGetProperty(PropertyName.vaseFillList, out var value6))
		{
			vaseFillList = value6.AsGodotArray<TowerDefenseLevelVaseFillConfig>();
		}
	}
}
