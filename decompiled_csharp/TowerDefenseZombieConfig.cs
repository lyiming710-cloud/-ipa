using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Config/TowerDefenseZombieConfig.cs")]
public class TowerDefenseZombieConfig : TowerDefenseCharacterConfig
{
	public new class MethodName : TowerDefenseCharacterConfig.MethodName
	{
	}

	public new class PropertyName : TowerDefenseCharacterConfig.PropertyName
	{
		public static readonly StringName physique = "physique";

		public static readonly StringName attack = "attack";

		public static readonly StringName smashAttack = "smashAttack";

		public static readonly StringName impactAudio = "impactAudio";

		public static readonly StringName preview = "preview";

		public static readonly StringName weight = "weight";

		public static readonly StringName wavePointCost = "wavePointCost";

		public static readonly StringName canSpawnPlantfood = "canSpawnPlantfood";

		public static readonly StringName excludeLineGridType = "excludeLineGridType";

		public static readonly StringName spawnLineNeed = "spawnLineNeed";
	}

	public new class SignalName : TowerDefenseCharacterConfig.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.ZOMBIE_PHYSIQUE physique = TowerDefenseEnum.ZOMBIE_PHYSIQUE.NORMAL;

	[Export(PropertyHint.None, "")]
	public double attack;

	[Export(PropertyHint.None, "")]
	public double smashAttack;

	[Export(PropertyHint.None, "")]
	public string impactAudio = "";

	[ExportCategory("Spawn")]
	[Export(PropertyHint.None, "")]
	public bool preview = true;

	[Export(PropertyHint.None, "")]
	public int weight = 1000;

	[Export(PropertyHint.None, "")]
	public int wavePointCost = 100;

	[Export(PropertyHint.None, "")]
	public bool canSpawnPlantfood = true;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseEnum.PLANTGRIDTYPE> excludeLineGridType = new Array<TowerDefenseEnum.PLANTGRIDTYPE>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseEnum.PLANTGRIDTYPE> spawnLineNeed = new Array<TowerDefenseEnum.PLANTGRIDTYPE>();

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.physique)
		{
			physique = VariantUtils.ConvertTo<TowerDefenseEnum.ZOMBIE_PHYSIQUE>(in value);
			return true;
		}
		if (name == PropertyName.attack)
		{
			attack = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.smashAttack)
		{
			smashAttack = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.impactAudio)
		{
			impactAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.preview)
		{
			preview = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.canSpawnPlantfood)
		{
			canSpawnPlantfood = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.excludeLineGridType)
		{
			excludeLineGridType = VariantUtils.ConvertToArray<TowerDefenseEnum.PLANTGRIDTYPE>(in value);
			return true;
		}
		if (name == PropertyName.spawnLineNeed)
		{
			spawnLineNeed = VariantUtils.ConvertToArray<TowerDefenseEnum.PLANTGRIDTYPE>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.physique)
		{
			value = VariantUtils.CreateFrom(in physique);
			return true;
		}
		if (name == PropertyName.attack)
		{
			value = VariantUtils.CreateFrom(in attack);
			return true;
		}
		if (name == PropertyName.smashAttack)
		{
			value = VariantUtils.CreateFrom(in smashAttack);
			return true;
		}
		if (name == PropertyName.impactAudio)
		{
			value = VariantUtils.CreateFrom(in impactAudio);
			return true;
		}
		if (name == PropertyName.preview)
		{
			value = VariantUtils.CreateFrom(in preview);
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
		if (name == PropertyName.canSpawnPlantfood)
		{
			value = VariantUtils.CreateFrom(in canSpawnPlantfood);
			return true;
		}
		if (name == PropertyName.excludeLineGridType)
		{
			value = VariantUtils.CreateFromArray(excludeLineGridType);
			return true;
		}
		if (name == PropertyName.spawnLineNeed)
		{
			value = VariantUtils.CreateFromArray(spawnLineNeed);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.physique, PropertyHint.Enum, "NOONE,SMALL,NORMAL,MID,HUGE,CAR,BOSS", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.attack, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.smashAttack, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.impactAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Spawn", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.preview, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.weight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.wavePointCost, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canSpawnPlantfood, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.excludeLineGridType, PropertyHint.TypeString, "2/2:ALL:-1,NOONE:0,SOIL:1,GROUND:2,WATER:3,AIR:4,LILYPAD:5,POT:6,SURROUND:7,GRAVESTONE:8,CRATER:9,BRICK:10,ICECAP:11,PLANT:12", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.spawnLineNeed, PropertyHint.TypeString, "2/2:ALL:-1,NOONE:0,SOIL:1,GROUND:2,WATER:3,AIR:4,LILYPAD:5,POT:6,SURROUND:7,GRAVESTONE:8,CRATER:9,BRICK:10,ICECAP:11,PLANT:12", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.physique, Variant.From(in physique));
		info.AddProperty(PropertyName.attack, Variant.From(in attack));
		info.AddProperty(PropertyName.smashAttack, Variant.From(in smashAttack));
		info.AddProperty(PropertyName.impactAudio, Variant.From(in impactAudio));
		info.AddProperty(PropertyName.preview, Variant.From(in preview));
		info.AddProperty(PropertyName.weight, Variant.From(in weight));
		info.AddProperty(PropertyName.wavePointCost, Variant.From(in wavePointCost));
		info.AddProperty(PropertyName.canSpawnPlantfood, Variant.From(in canSpawnPlantfood));
		info.AddProperty(PropertyName.excludeLineGridType, Variant.CreateFrom(excludeLineGridType));
		info.AddProperty(PropertyName.spawnLineNeed, Variant.CreateFrom(spawnLineNeed));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.physique, out var value))
		{
			physique = value.As<TowerDefenseEnum.ZOMBIE_PHYSIQUE>();
		}
		if (info.TryGetProperty(PropertyName.attack, out var value2))
		{
			attack = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.smashAttack, out var value3))
		{
			smashAttack = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.impactAudio, out var value4))
		{
			impactAudio = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.preview, out var value5))
		{
			preview = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.weight, out var value6))
		{
			weight = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.wavePointCost, out var value7))
		{
			wavePointCost = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.canSpawnPlantfood, out var value8))
		{
			canSpawnPlantfood = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.excludeLineGridType, out var value9))
		{
			excludeLineGridType = value9.AsGodotArray<TowerDefenseEnum.PLANTGRIDTYPE>();
		}
		if (info.TryGetProperty(PropertyName.spawnLineNeed, out var value10))
		{
			spawnLineNeed = value10.AsGodotArray<TowerDefenseEnum.PLANTGRIDTYPE>();
		}
	}
}
