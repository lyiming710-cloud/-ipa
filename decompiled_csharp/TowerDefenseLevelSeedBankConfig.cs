using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/SeedBank/Resource/TowerDefenseLevelSeedBankConfig.cs")]
public class TowerDefenseLevelSeedBankConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName Export = "Export";

		public static readonly StringName AddPacketConfig = "AddPacketConfig";

		public static readonly StringName AddFallbackPacketConfig = "AddFallbackPacketConfig";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName plantColumn = "plantColumn";

		public static readonly StringName packetColdDownStart = "packetColdDownStart";

		public static readonly StringName packetColdDownUse = "packetColdDownUse";

		public static readonly StringName method = "method";

		public static readonly StringName packetList = "packetList";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool plantColumn;

	[Export(PropertyHint.None, "")]
	public bool packetColdDownStart = true;

	[Export(PropertyHint.None, "")]
	public bool packetColdDownUse = true;

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.LEVEL_SEEDBANK_METHOD method = TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseLevelPacketConfig> packetList = new Array<TowerDefenseLevelPacketConfig>();

	public void Init(Dictionary data)
	{
		packetList.Clear();
		TowerDefenseLevelConfig towerDefenseLevelConfig = TowerDefenseManager.Instance?.currentLevelConfig as TowerDefenseLevelConfig;
		bool flag = GodotObject.IsInstanceValid(towerDefenseLevelConfig);
		method = (flag ? towerDefenseLevelConfig.packetBankMethod : TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.NOONE);
		string value = data.GetValueOrDefault("Method", "").AsString();
		if (!string.IsNullOrEmpty(value) && Enum.TryParse<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(value, ignoreCase: true, out var result))
		{
			method = result;
		}
		plantColumn = data.GetValueOrDefault("PlantColumn", flag && towerDefenseLevelConfig.plantColumn).AsBool();
		packetColdDownStart = data.GetValueOrDefault("ColdDownStart", !flag || towerDefenseLevelConfig.packetColdDownStart).AsBool();
		packetColdDownUse = data.GetValueOrDefault("ColdDownUse", !flag || towerDefenseLevelConfig.packetColdDownUse).AsBool();
		Godot.Collections.Array array = data.GetValueOrDefault("Packet", new Godot.Collections.Array()).AsGodotArray();
		if (array.Count > 0)
		{
			foreach (Variant item in array)
			{
				AddPacketConfig(item);
			}
			return;
		}
		if (!flag || (method != TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.CHOOSE && method != TowerDefenseEnum.LEVEL_SEEDBANK_METHOD.PRESET))
		{
			return;
		}
		foreach (Variant packetBank in towerDefenseLevelConfig.packetBankList)
		{
			AddFallbackPacketConfig(packetBank);
		}
	}

	public Dictionary Export()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseLevelPacketConfig packet in packetList)
		{
			if (GodotObject.IsInstanceValid(packet))
			{
				array.Add(packet.Export());
			}
		}
		return new Dictionary
		{
			["Method"] = method.ToString(),
			["PlantColumn"] = plantColumn,
			["ColdDownStart"] = packetColdDownStart,
			["ColdDownUse"] = packetColdDownUse,
			["Packet"] = array
		};
	}

	private void AddPacketConfig(Variant packetDataVariant)
	{
		TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = new TowerDefenseLevelPacketConfig();
		towerDefenseLevelPacketConfig.Init(packetDataVariant);
		packetList.Add(towerDefenseLevelPacketConfig);
	}

	private void AddFallbackPacketConfig(Variant packetDataVariant)
	{
		if (packetDataVariant.VariantType == Variant.Type.Object && packetDataVariant.AsGodotObject() is TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig)
		{
			TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig2 = towerDefenseLevelPacketConfig.Duplicate(deep: true) as TowerDefenseLevelPacketConfig;
			packetList.Add(GodotObject.IsInstanceValid(towerDefenseLevelPacketConfig2) ? towerDefenseLevelPacketConfig2 : towerDefenseLevelPacketConfig);
		}
		else
		{
			AddPacketConfig(packetDataVariant);
		}
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
			new MethodInfo(MethodName.AddPacketConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "packetDataVariant", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AddFallbackPacketConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "packetDataVariant", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
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
		if (method == MethodName.AddPacketConfig && args.Count == 1)
		{
			AddPacketConfig(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddFallbackPacketConfig && args.Count == 1)
		{
			AddFallbackPacketConfig(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
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
		if (method == MethodName.AddPacketConfig)
		{
			return true;
		}
		if (method == MethodName.AddFallbackPacketConfig)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.plantColumn)
		{
			plantColumn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packetColdDownStart)
		{
			packetColdDownStart = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.packetColdDownUse)
		{
			packetColdDownUse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.method)
		{
			method = VariantUtils.ConvertTo<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>(in value);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			packetList = VariantUtils.ConvertToArray<TowerDefenseLevelPacketConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.plantColumn)
		{
			value = VariantUtils.CreateFrom(in plantColumn);
			return true;
		}
		if (name == PropertyName.packetColdDownStart)
		{
			value = VariantUtils.CreateFrom(in packetColdDownStart);
			return true;
		}
		if (name == PropertyName.packetColdDownUse)
		{
			value = VariantUtils.CreateFrom(in packetColdDownUse);
			return true;
		}
		if (name == PropertyName.method)
		{
			value = VariantUtils.CreateFrom(in method);
			return true;
		}
		if (name == PropertyName.packetList)
		{
			value = VariantUtils.CreateFromArray(packetList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.plantColumn, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.packetColdDownStart, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.packetColdDownUse, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.method, PropertyHint.Enum, "NOONE,CHOOSE,PRESET,CONVEYOR,RAIN", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetList, PropertyHint.TypeString, "24/17:TowerDefenseLevelPacketConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.plantColumn, Variant.From(in plantColumn));
		info.AddProperty(PropertyName.packetColdDownStart, Variant.From(in packetColdDownStart));
		info.AddProperty(PropertyName.packetColdDownUse, Variant.From(in packetColdDownUse));
		info.AddProperty(PropertyName.method, Variant.From(in method));
		info.AddProperty(PropertyName.packetList, Variant.CreateFrom(packetList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.plantColumn, out var value))
		{
			plantColumn = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packetColdDownStart, out var value2))
		{
			packetColdDownStart = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.packetColdDownUse, out var value3))
		{
			packetColdDownUse = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.method, out var value4))
		{
			method = value4.As<TowerDefenseEnum.LEVEL_SEEDBANK_METHOD>();
		}
		if (info.TryGetProperty(PropertyName.packetList, out var value5))
		{
			packetList = value5.AsGodotArray<TowerDefenseLevelPacketConfig>();
		}
	}
}
