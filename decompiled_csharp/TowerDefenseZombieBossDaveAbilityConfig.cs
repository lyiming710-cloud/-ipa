using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Boss/BossDave/Config/TowerDefenseZombieBossDaveAbilityConfig.cs")]
public class TowerDefenseZombieBossDaveAbilityConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName GetZombieStage = "GetZombieStage";

		public static readonly StringName GetPlantPool = "GetPlantPool";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName plantDropCount = "plantDropCount";

		public static readonly StringName plantColumnMin = "plantColumnMin";

		public static readonly StringName plantColumnMax = "plantColumnMax";

		public static readonly StringName bungeeCount = "bungeeCount";

		public static readonly StringName steelBungeeChance = "steelBungeeChance";

		public static readonly StringName hypnotistChance = "hypnotistChance";

		public static readonly StringName missileCount = "missileCount";

		public static readonly StringName missileDamage = "missileDamage";

		public static readonly StringName missileProjectileConfig = "missileProjectileConfig";

		public static readonly StringName doomChargeSeconds = "doomChargeSeconds";

		public static readonly StringName doomDamage = "doomDamage";

		public static readonly StringName stageZombiePools = "stageZombiePools";

		public static readonly StringName stagePlantPools = "stagePlantPools";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[ExportGroup("Placement", "")]
	[Export(PropertyHint.None, "")]
	public int plantDropCount = 3;

	[Export(PropertyHint.None, "")]
	public int plantColumnMin = 6;

	[Export(PropertyHint.None, "")]
	public int plantColumnMax = 9;

	[ExportGroup("Bungee", "")]
	[Export(PropertyHint.None, "")]
	public int bungeeCount = 3;

	[Export(PropertyHint.Range, "0,1,0.01")]
	public double steelBungeeChance = 0.1;

	[Export(PropertyHint.Range, "0,1,0.01")]
	public double hypnotistChance = 0.25;

	[ExportGroup("Missile", "")]
	[Export(PropertyHint.None, "")]
	public int missileCount = 4;

	[Export(PropertyHint.None, "")]
	public double missileDamage = 1800.0;

	[Export(PropertyHint.None, "")]
	public TowerDefenseProjectileConfig missileProjectileConfig;

	[ExportGroup("Doom", "")]
	[Export(PropertyHint.None, "")]
	public double doomChargeSeconds = 20.0;

	[Export(PropertyHint.None, "")]
	public double doomDamage = 1800.0;

	[ExportGroup("Stage Pools", "")]
	[Export(PropertyHint.None, "")]
	public Array stageZombiePools = new Array();

	[Export(PropertyHint.None, "")]
	public Array stagePlantPools = new Array();

	public Array GetZombieStage(int stage)
	{
		if (stageZombiePools.Count == 0)
		{
			return new Array();
		}
		int index = Mathf.Clamp(stage, 0, stageZombiePools.Count - 1);
		return stageZombiePools[index].AsGodotArray();
	}

	public Array GetPlantPool(int stage)
	{
		if (stagePlantPools.Count == 0)
		{
			return new Array();
		}
		int index = Mathf.Clamp(stage, 0, stagePlantPools.Count - 1);
		return stagePlantPools[index].AsGodotArray();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.GetZombieStage, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPlantPool, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetZombieStage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Array>(GetZombieStage(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPlantPool && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Array>(GetPlantPool(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetZombieStage)
		{
			return true;
		}
		if (method == MethodName.GetPlantPool)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.plantDropCount)
		{
			plantDropCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.plantColumnMin)
		{
			plantColumnMin = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.plantColumnMax)
		{
			plantColumnMax = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.bungeeCount)
		{
			bungeeCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.steelBungeeChance)
		{
			steelBungeeChance = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hypnotistChance)
		{
			hypnotistChance = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.missileCount)
		{
			missileCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.missileDamage)
		{
			missileDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.missileProjectileConfig)
		{
			missileProjectileConfig = VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in value);
			return true;
		}
		if (name == PropertyName.doomChargeSeconds)
		{
			doomChargeSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.doomDamage)
		{
			doomDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.stageZombiePools)
		{
			stageZombiePools = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.stagePlantPools)
		{
			stagePlantPools = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.plantDropCount)
		{
			value = VariantUtils.CreateFrom(in plantDropCount);
			return true;
		}
		if (name == PropertyName.plantColumnMin)
		{
			value = VariantUtils.CreateFrom(in plantColumnMin);
			return true;
		}
		if (name == PropertyName.plantColumnMax)
		{
			value = VariantUtils.CreateFrom(in plantColumnMax);
			return true;
		}
		if (name == PropertyName.bungeeCount)
		{
			value = VariantUtils.CreateFrom(in bungeeCount);
			return true;
		}
		if (name == PropertyName.steelBungeeChance)
		{
			value = VariantUtils.CreateFrom(in steelBungeeChance);
			return true;
		}
		if (name == PropertyName.hypnotistChance)
		{
			value = VariantUtils.CreateFrom(in hypnotistChance);
			return true;
		}
		if (name == PropertyName.missileCount)
		{
			value = VariantUtils.CreateFrom(in missileCount);
			return true;
		}
		if (name == PropertyName.missileDamage)
		{
			value = VariantUtils.CreateFrom(in missileDamage);
			return true;
		}
		if (name == PropertyName.missileProjectileConfig)
		{
			value = VariantUtils.CreateFrom(in missileProjectileConfig);
			return true;
		}
		if (name == PropertyName.doomChargeSeconds)
		{
			value = VariantUtils.CreateFrom(in doomChargeSeconds);
			return true;
		}
		if (name == PropertyName.doomDamage)
		{
			value = VariantUtils.CreateFrom(in doomDamage);
			return true;
		}
		if (name == PropertyName.stageZombiePools)
		{
			value = VariantUtils.CreateFrom(in stageZombiePools);
			return true;
		}
		if (name == PropertyName.stagePlantPools)
		{
			value = VariantUtils.CreateFrom(in stagePlantPools);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Nil, "Placement", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.plantDropCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.plantColumnMin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.plantColumnMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Bungee", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.bungeeCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.steelBungeeChance, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hypnotistChance, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Missile", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.missileCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.missileDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.missileProjectileConfig, PropertyHint.ResourceType, "TowerDefenseProjectileConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Doom", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.doomChargeSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.doomDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Stage Pools", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.stageZombiePools, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.stagePlantPools, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.plantDropCount, Variant.From(in plantDropCount));
		info.AddProperty(PropertyName.plantColumnMin, Variant.From(in plantColumnMin));
		info.AddProperty(PropertyName.plantColumnMax, Variant.From(in plantColumnMax));
		info.AddProperty(PropertyName.bungeeCount, Variant.From(in bungeeCount));
		info.AddProperty(PropertyName.steelBungeeChance, Variant.From(in steelBungeeChance));
		info.AddProperty(PropertyName.hypnotistChance, Variant.From(in hypnotistChance));
		info.AddProperty(PropertyName.missileCount, Variant.From(in missileCount));
		info.AddProperty(PropertyName.missileDamage, Variant.From(in missileDamage));
		info.AddProperty(PropertyName.missileProjectileConfig, Variant.From(in missileProjectileConfig));
		info.AddProperty(PropertyName.doomChargeSeconds, Variant.From(in doomChargeSeconds));
		info.AddProperty(PropertyName.doomDamage, Variant.From(in doomDamage));
		info.AddProperty(PropertyName.stageZombiePools, Variant.From(in stageZombiePools));
		info.AddProperty(PropertyName.stagePlantPools, Variant.From(in stagePlantPools));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.plantDropCount, out var value))
		{
			plantDropCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.plantColumnMin, out var value2))
		{
			plantColumnMin = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.plantColumnMax, out var value3))
		{
			plantColumnMax = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.bungeeCount, out var value4))
		{
			bungeeCount = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.steelBungeeChance, out var value5))
		{
			steelBungeeChance = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hypnotistChance, out var value6))
		{
			hypnotistChance = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.missileCount, out var value7))
		{
			missileCount = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.missileDamage, out var value8))
		{
			missileDamage = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.missileProjectileConfig, out var value9))
		{
			missileProjectileConfig = value9.As<TowerDefenseProjectileConfig>();
		}
		if (info.TryGetProperty(PropertyName.doomChargeSeconds, out var value10))
		{
			doomChargeSeconds = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.doomDamage, out var value11))
		{
			doomDamage = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.stageZombiePools, out var value12))
		{
			stageZombiePools = value12.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.stagePlantPools, out var value13))
		{
			stagePlantPools = value13.As<Array>();
		}
	}
}
