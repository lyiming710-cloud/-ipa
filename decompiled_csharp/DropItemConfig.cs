using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/DropItem/DropItemConfig.cs")]
public class DropItemConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Id = "Id";

		public static readonly StringName Name = "Name";

		public static readonly StringName Scene = "Scene";

		public static readonly StringName PoolMaxNum = "PoolMaxNum";

		public static readonly StringName Category = "Category";

		public static readonly StringName Value = "Value";

		public static readonly StringName FallAudio = "FallAudio";

		public static readonly StringName PickAudio = "PickAudio";

		public static readonly StringName CoinObjectId = "CoinObjectId";

		public static readonly StringName Handler = "Handler";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public ObjectManagerConfig.OBJECT Id;

	[Export(PropertyHint.None, "")]
	public StringName Name = "";

	[Export(PropertyHint.None, "")]
	public PackedScene Scene;

	[Export(PropertyHint.None, "")]
	public int PoolMaxNum = 100;

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.DROP_ITEM_CATEGORY Category = TowerDefenseEnum.DROP_ITEM_CATEGORY.NOONE;

	[Export(PropertyHint.None, "")]
	public int Value;

	[Export(PropertyHint.None, "")]
	public string FallAudio = "CoinFall";

	[Export(PropertyHint.None, "")]
	public string PickAudio = "CoinPick";

	[Export(PropertyHint.None, "")]
	public int CoinObjectId = -1;

	[Export(PropertyHint.None, "")]
	public DropItemHandler Handler;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Id)
		{
			Id = VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in value);
			return true;
		}
		if (name == PropertyName.Name)
		{
			Name = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.Scene)
		{
			Scene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.PoolMaxNum)
		{
			PoolMaxNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.Category)
		{
			Category = VariantUtils.ConvertTo<TowerDefenseEnum.DROP_ITEM_CATEGORY>(in value);
			return true;
		}
		if (name == PropertyName.Value)
		{
			Value = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.FallAudio)
		{
			FallAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.PickAudio)
		{
			PickAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.CoinObjectId)
		{
			CoinObjectId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.Handler)
		{
			Handler = VariantUtils.ConvertTo<DropItemHandler>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Id)
		{
			value = VariantUtils.CreateFrom(in Id);
			return true;
		}
		if (name == PropertyName.Name)
		{
			value = VariantUtils.CreateFrom(in Name);
			return true;
		}
		if (name == PropertyName.Scene)
		{
			value = VariantUtils.CreateFrom(in Scene);
			return true;
		}
		if (name == PropertyName.PoolMaxNum)
		{
			value = VariantUtils.CreateFrom(in PoolMaxNum);
			return true;
		}
		if (name == PropertyName.Category)
		{
			value = VariantUtils.CreateFrom(in Category);
			return true;
		}
		if (name == PropertyName.Value)
		{
			value = VariantUtils.CreateFrom(in Value);
			return true;
		}
		if (name == PropertyName.FallAudio)
		{
			value = VariantUtils.CreateFrom(in FallAudio);
			return true;
		}
		if (name == PropertyName.PickAudio)
		{
			value = VariantUtils.CreateFrom(in PickAudio);
			return true;
		}
		if (name == PropertyName.CoinObjectId)
		{
			value = VariantUtils.CreateFrom(in CoinObjectId);
			return true;
		}
		if (name == PropertyName.Handler)
		{
			value = VariantUtils.CreateFrom(in Handler);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.Id, PropertyHint.Enum, "NOONE:0,PROJECTILE:1,damagePart:2,GAMECOLLECT:10,SUN:11,SUN_BRAIN:12,SUN_JALAPENO:13,SUN_QX:14,SUN_MAGIC:15,COIN:20,COIN_SILVER:21,COIN_GOLD:22,COIN_DIAMOND:23,COIN_LUCKY_BAG:24,COIN_TQ:25,COIN_YB1:26,COIN_YB2:27,COIN_GOLD_SHARD:28,PARTICLES_SPLASH:201,PARTICLES_RISE_DIRT:202,PARTICLES_ICE_TRAP:203,SHOW_HEALTH_VIEW:301,MAX:1000", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName.Name, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.Scene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.PoolMaxNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.Category, PropertyHint.Enum, "NOONE:-1,SUN:0,COIN:1,SPECIAL:2", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.Value, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.FallAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.PickAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CoinObjectId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.Handler, PropertyHint.ResourceType, "DropItemHandler", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Id, Variant.From(in Id));
		info.AddProperty(PropertyName.Name, Variant.From(in Name));
		info.AddProperty(PropertyName.Scene, Variant.From(in Scene));
		info.AddProperty(PropertyName.PoolMaxNum, Variant.From(in PoolMaxNum));
		info.AddProperty(PropertyName.Category, Variant.From(in Category));
		info.AddProperty(PropertyName.Value, Variant.From(in Value));
		info.AddProperty(PropertyName.FallAudio, Variant.From(in FallAudio));
		info.AddProperty(PropertyName.PickAudio, Variant.From(in PickAudio));
		info.AddProperty(PropertyName.CoinObjectId, Variant.From(in CoinObjectId));
		info.AddProperty(PropertyName.Handler, Variant.From(in Handler));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Id, out var value))
		{
			Id = value.As<ObjectManagerConfig.OBJECT>();
		}
		if (info.TryGetProperty(PropertyName.Name, out var value2))
		{
			Name = value2.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.Scene, out var value3))
		{
			Scene = value3.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.PoolMaxNum, out var value4))
		{
			PoolMaxNum = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Category, out var value5))
		{
			Category = value5.As<TowerDefenseEnum.DROP_ITEM_CATEGORY>();
		}
		if (info.TryGetProperty(PropertyName.Value, out var value6))
		{
			Value = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.FallAudio, out var value7))
		{
			FallAudio = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.PickAudio, out var value8))
		{
			PickAudio = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName.CoinObjectId, out var value9))
		{
			CoinObjectId = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.Handler, out var value10))
		{
			Handler = value10.As<DropItemHandler>();
		}
	}
}
