using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/Map/Resource/Rule/TowerDefenseMapRuleConfig.cs")]
public class TowerDefenseMapRuleConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName id = "id";

		public static readonly StringName preventsSleep = "preventsSleep";

		public static readonly StringName attackDpsLifestealRatio = "attackDpsLifestealRatio";

		public static readonly StringName packetRules = "packetRules";

		public static readonly StringName characterRules = "characterRules";

		public static readonly StringName zombieColumnSpeedMinCol = "zombieColumnSpeedMinCol";

		public static readonly StringName zombieColumnSpeedMaxCol = "zombieColumnSpeedMaxCol";

		public static readonly StringName zombieColumnSpeedMultiplier = "zombieColumnSpeedMultiplier";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public StringName id = "";

	[Export(PropertyHint.None, "")]
	public bool preventsSleep;

	[Export(PropertyHint.Range, "0,100,0.01,or_greater")]
	public double attackDpsLifestealRatio;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseMapPacketRuleConfig> packetRules = new Array<TowerDefenseMapPacketRuleConfig>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseMapCharacterRuleConfig> characterRules = new Array<TowerDefenseMapCharacterRuleConfig>();

	[ExportGroup("Zombie Column Speed", "")]
	[Export(PropertyHint.Range, "0,50,1")]
	public int zombieColumnSpeedMinCol;

	[Export(PropertyHint.Range, "0,50,1")]
	public int zombieColumnSpeedMaxCol;

	[Export(PropertyHint.Range, "0,100,0.01,or_greater")]
	public double zombieColumnSpeedMultiplier = 1.0;

	public bool TryValidateRuntime(out string reason)
	{
		if (id.IsEmpty)
		{
			reason = "map rule has no id";
			return false;
		}
		if (!double.IsFinite(attackDpsLifestealRatio) || attackDpsLifestealRatio < 0.0)
		{
			reason = $"map rule '{id}' has an invalid DPS lifesteal ratio";
			return false;
		}
		bool flag = zombieColumnSpeedMinCol > 0 || zombieColumnSpeedMaxCol > 0;
		if (flag && (zombieColumnSpeedMinCol <= 0 || zombieColumnSpeedMaxCol <= 0 || zombieColumnSpeedMinCol > zombieColumnSpeedMaxCol || zombieColumnSpeedMaxCol > 50))
		{
			reason = $"map rule '{id}' has an invalid zombie column speed range (min={zombieColumnSpeedMinCol}, max={zombieColumnSpeedMaxCol})";
			return false;
		}
		if (flag && (!double.IsFinite(zombieColumnSpeedMultiplier) || zombieColumnSpeedMultiplier < 0.0))
		{
			reason = $"map rule '{id}' has an invalid zombie column speed multiplier";
			return false;
		}
		if (packetRules != null)
		{
			foreach (TowerDefenseMapPacketRuleConfig packetRule in packetRules)
			{
				if (!GodotObject.IsInstanceValid(packetRule))
				{
					reason = $"map rule '{id}' contains a null or freed packet rule";
					return false;
				}
				if (!packetRule.TryValidateRuntime(out reason))
				{
					reason = $"map rule '{id}': {reason}";
					return false;
				}
			}
		}
		if (characterRules != null)
		{
			foreach (TowerDefenseMapCharacterRuleConfig characterRule in characterRules)
			{
				if (!GodotObject.IsInstanceValid(characterRule))
				{
					reason = $"map rule '{id}' contains a null or freed character rule";
					return false;
				}
				if (!characterRule.TryValidateRuntime(out reason))
				{
					reason = $"map rule '{id}': {reason}";
					return false;
				}
			}
		}
		reason = "";
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.id)
		{
			id = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.preventsSleep)
		{
			preventsSleep = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.attackDpsLifestealRatio)
		{
			attackDpsLifestealRatio = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.packetRules)
		{
			packetRules = VariantUtils.ConvertToArray<TowerDefenseMapPacketRuleConfig>(in value);
			return true;
		}
		if (name == PropertyName.characterRules)
		{
			characterRules = VariantUtils.ConvertToArray<TowerDefenseMapCharacterRuleConfig>(in value);
			return true;
		}
		if (name == PropertyName.zombieColumnSpeedMinCol)
		{
			zombieColumnSpeedMinCol = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.zombieColumnSpeedMaxCol)
		{
			zombieColumnSpeedMaxCol = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.zombieColumnSpeedMultiplier)
		{
			zombieColumnSpeedMultiplier = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.id)
		{
			value = VariantUtils.CreateFrom(in id);
			return true;
		}
		if (name == PropertyName.preventsSleep)
		{
			value = VariantUtils.CreateFrom(in preventsSleep);
			return true;
		}
		if (name == PropertyName.attackDpsLifestealRatio)
		{
			value = VariantUtils.CreateFrom(in attackDpsLifestealRatio);
			return true;
		}
		if (name == PropertyName.packetRules)
		{
			value = VariantUtils.CreateFromArray(packetRules);
			return true;
		}
		if (name == PropertyName.characterRules)
		{
			value = VariantUtils.CreateFromArray(characterRules);
			return true;
		}
		if (name == PropertyName.zombieColumnSpeedMinCol)
		{
			value = VariantUtils.CreateFrom(in zombieColumnSpeedMinCol);
			return true;
		}
		if (name == PropertyName.zombieColumnSpeedMaxCol)
		{
			value = VariantUtils.CreateFrom(in zombieColumnSpeedMaxCol);
			return true;
		}
		if (name == PropertyName.zombieColumnSpeedMultiplier)
		{
			value = VariantUtils.CreateFrom(in zombieColumnSpeedMultiplier);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.id, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.preventsSleep, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackDpsLifestealRatio, PropertyHint.Range, "0,100,0.01,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.packetRules, PropertyHint.TypeString, "24/17:TowerDefenseMapPacketRuleConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.characterRules, PropertyHint.TypeString, "24/17:TowerDefenseMapCharacterRuleConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Zombie Column Speed", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.zombieColumnSpeedMinCol, PropertyHint.Range, "0,50,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.zombieColumnSpeedMaxCol, PropertyHint.Range, "0,50,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.zombieColumnSpeedMultiplier, PropertyHint.Range, "0,100,0.01,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.id, Variant.From(in id));
		info.AddProperty(PropertyName.preventsSleep, Variant.From(in preventsSleep));
		info.AddProperty(PropertyName.attackDpsLifestealRatio, Variant.From(in attackDpsLifestealRatio));
		info.AddProperty(PropertyName.packetRules, Variant.CreateFrom(packetRules));
		info.AddProperty(PropertyName.characterRules, Variant.CreateFrom(characterRules));
		info.AddProperty(PropertyName.zombieColumnSpeedMinCol, Variant.From(in zombieColumnSpeedMinCol));
		info.AddProperty(PropertyName.zombieColumnSpeedMaxCol, Variant.From(in zombieColumnSpeedMaxCol));
		info.AddProperty(PropertyName.zombieColumnSpeedMultiplier, Variant.From(in zombieColumnSpeedMultiplier));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.id, out var value))
		{
			id = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.preventsSleep, out var value2))
		{
			preventsSleep = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.attackDpsLifestealRatio, out var value3))
		{
			attackDpsLifestealRatio = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.packetRules, out var value4))
		{
			packetRules = value4.AsGodotArray<TowerDefenseMapPacketRuleConfig>();
		}
		if (info.TryGetProperty(PropertyName.characterRules, out var value5))
		{
			characterRules = value5.AsGodotArray<TowerDefenseMapCharacterRuleConfig>();
		}
		if (info.TryGetProperty(PropertyName.zombieColumnSpeedMinCol, out var value6))
		{
			zombieColumnSpeedMinCol = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.zombieColumnSpeedMaxCol, out var value7))
		{
			zombieColumnSpeedMaxCol = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.zombieColumnSpeedMultiplier, out var value8))
		{
			zombieColumnSpeedMultiplier = value8.As<double>();
		}
	}
}
