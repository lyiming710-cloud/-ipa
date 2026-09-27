using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/FallingObjects/WeightItem/FallingObjectWeightItemConfig.cs")]
public class FallingObjectWeightItemConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName empty = "empty";

		public static readonly StringName item = "item";

		public static readonly StringName weight = "weight";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool empty;

	[Export(PropertyHint.None, "")]
	public ObjectManagerConfig.OBJECT item;

	[Export(PropertyHint.None, "")]
	public int weight = 100;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.empty)
		{
			empty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.item)
		{
			item = VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in value);
			return true;
		}
		if (name == PropertyName.weight)
		{
			weight = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.empty)
		{
			value = VariantUtils.CreateFrom(in empty);
			return true;
		}
		if (name == PropertyName.item)
		{
			value = VariantUtils.CreateFrom(in item);
			return true;
		}
		if (name == PropertyName.weight)
		{
			value = VariantUtils.CreateFrom(in weight);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.empty, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.item, PropertyHint.Enum, "NOONE:0,PROJECTILE:1,damagePart:2,GAMECOLLECT:10,SUN:11,SUN_BRAIN:12,SUN_JALAPENO:13,SUN_QX:14,SUN_MAGIC:15,COIN:20,COIN_SILVER:21,COIN_GOLD:22,COIN_DIAMOND:23,COIN_LUCKY_BAG:24,COIN_TQ:25,COIN_YB1:26,COIN_YB2:27,COIN_GOLD_SHARD:28,PARTICLES_SPLASH:201,PARTICLES_RISE_DIRT:202,PARTICLES_ICE_TRAP:203,SHOW_HEALTH_VIEW:301,MAX:1000", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.weight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.empty, Variant.From(in empty));
		info.AddProperty(PropertyName.item, Variant.From(in item));
		info.AddProperty(PropertyName.weight, Variant.From(in weight));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.empty, out var value))
		{
			empty = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.item, out var value2))
		{
			item = value2.As<ObjectManagerConfig.OBJECT>();
		}
		if (info.TryGetProperty(PropertyName.weight, out var value3))
		{
			weight = value3.As<int>();
		}
	}
}
