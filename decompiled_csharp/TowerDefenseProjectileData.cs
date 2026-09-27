using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Projectile/Data/TowerDefenseProjectileData.cs")]
public class TowerDefenseProjectileData : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName name = "name";

		public static readonly StringName baseDamage = "baseDamage";

		public static readonly StringName size = "size";

		public static readonly StringName scale = "scale";

		public static readonly StringName projectileScene = "projectileScene";

		public static readonly StringName splatSceneType = "splatSceneType";

		public static readonly StringName splatAudio = "splatAudio";

		public static readonly StringName splatScene = "splatScene";

		public static readonly StringName hitEffect = "hitEffect";

		public static readonly StringName hitTargetEventList = "hitTargetEventList";

		public static readonly StringName hitCharacterEventList = "hitCharacterEventList";

		public static readonly StringName hitGroundEventList = "hitGroundEventList";

		public static readonly StringName blockHurt = "blockHurt";

		public static readonly StringName rotateFollowVelocity = "rotateFollowVelocity";

		public static readonly StringName rotateScale = "rotateScale";

		public static readonly StringName hitBody = "hitBody";

		public static readonly StringName rangeType = "rangeType";

		public static readonly StringName useRange = "useRange";

		public static readonly StringName rangeSize = "rangeSize";

		public static readonly StringName hitPesontage = "hitPesontage";

		public static readonly StringName isFire = "isFire";

		public static readonly StringName isStar = "isStar";

		public static readonly StringName isMagic = "isMagic";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string name = "";

	[Export(PropertyHint.None, "")]
	public double baseDamage = 20.0;

	[Export(PropertyHint.None, "")]
	public Vector2 size = new Vector2(28f, 28f);

	[Export(PropertyHint.None, "")]
	public Vector2 scale = new Vector2(1f, 1f);

	[Export(PropertyHint.None, "")]
	public PackedScene projectileScene;

	[Export(PropertyHint.Enum, "Particles,Sprite")]
	public string splatSceneType = "Particles";

	[Export(PropertyHint.None, "")]
	public string splatAudio = "SplatNormal";

	[Export(PropertyHint.None, "")]
	public PackedScene splatScene;

	[Export(PropertyHint.None, "")]
	public PackedScene hitEffect;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> hitTargetEventList = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> hitCharacterEventList = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> hitGroundEventList = new Array<TowerDefenseCharacterEventBase>();

	[ExportGroup("Init", "")]
	[Export(PropertyHint.None, "")]
	public double blockHurt = -1.0;

	[Export(PropertyHint.None, "")]
	public bool rotateFollowVelocity;

	[Export(PropertyHint.None, "")]
	public double rotateScale;

	[Export(PropertyHint.None, "")]
	public bool hitBody;

	[ExportGroup("Range", "")]
	[Export(PropertyHint.Enum, "Default,Bomb")]
	public string rangeType = "Default";

	[Export(PropertyHint.None, "")]
	public bool useRange;

	[Export(PropertyHint.None, "")]
	public Vector2 rangeSize = new Vector2(0.5f, 0.5f);

	[Export(PropertyHint.None, "")]
	public double hitPesontage = 0.25;

	[ExportGroup("Flag", "")]
	[Export(PropertyHint.None, "")]
	public bool isFire;

	[Export(PropertyHint.None, "")]
	public bool isStar;

	[Export(PropertyHint.None, "")]
	public bool isMagic;

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.name)
		{
			this.name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.baseDamage)
		{
			baseDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.size)
		{
			size = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.scale)
		{
			scale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.projectileScene)
		{
			projectileScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.splatSceneType)
		{
			splatSceneType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.splatAudio)
		{
			splatAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.splatScene)
		{
			splatScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.hitEffect)
		{
			hitEffect = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.hitTargetEventList)
		{
			hitTargetEventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.hitCharacterEventList)
		{
			hitCharacterEventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.hitGroundEventList)
		{
			hitGroundEventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.blockHurt)
		{
			blockHurt = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.rotateFollowVelocity)
		{
			rotateFollowVelocity = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.rotateScale)
		{
			rotateScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitBody)
		{
			hitBody = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.rangeType)
		{
			rangeType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.useRange)
		{
			useRange = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.rangeSize)
		{
			rangeSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.hitPesontage)
		{
			hitPesontage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.isFire)
		{
			isFire = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isStar)
		{
			isStar = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isMagic)
		{
			isMagic = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.name)
		{
			value = VariantUtils.CreateFrom(in this.name);
			return true;
		}
		if (name == PropertyName.baseDamage)
		{
			value = VariantUtils.CreateFrom(in baseDamage);
			return true;
		}
		if (name == PropertyName.size)
		{
			value = VariantUtils.CreateFrom(in size);
			return true;
		}
		if (name == PropertyName.scale)
		{
			value = VariantUtils.CreateFrom(in scale);
			return true;
		}
		if (name == PropertyName.projectileScene)
		{
			value = VariantUtils.CreateFrom(in projectileScene);
			return true;
		}
		if (name == PropertyName.splatSceneType)
		{
			value = VariantUtils.CreateFrom(in splatSceneType);
			return true;
		}
		if (name == PropertyName.splatAudio)
		{
			value = VariantUtils.CreateFrom(in splatAudio);
			return true;
		}
		if (name == PropertyName.splatScene)
		{
			value = VariantUtils.CreateFrom(in splatScene);
			return true;
		}
		if (name == PropertyName.hitEffect)
		{
			value = VariantUtils.CreateFrom(in hitEffect);
			return true;
		}
		if (name == PropertyName.hitTargetEventList)
		{
			value = VariantUtils.CreateFromArray(hitTargetEventList);
			return true;
		}
		if (name == PropertyName.hitCharacterEventList)
		{
			value = VariantUtils.CreateFromArray(hitCharacterEventList);
			return true;
		}
		if (name == PropertyName.hitGroundEventList)
		{
			value = VariantUtils.CreateFromArray(hitGroundEventList);
			return true;
		}
		if (name == PropertyName.blockHurt)
		{
			value = VariantUtils.CreateFrom(in blockHurt);
			return true;
		}
		if (name == PropertyName.rotateFollowVelocity)
		{
			value = VariantUtils.CreateFrom(in rotateFollowVelocity);
			return true;
		}
		if (name == PropertyName.rotateScale)
		{
			value = VariantUtils.CreateFrom(in rotateScale);
			return true;
		}
		if (name == PropertyName.hitBody)
		{
			value = VariantUtils.CreateFrom(in hitBody);
			return true;
		}
		if (name == PropertyName.rangeType)
		{
			value = VariantUtils.CreateFrom(in rangeType);
			return true;
		}
		if (name == PropertyName.useRange)
		{
			value = VariantUtils.CreateFrom(in useRange);
			return true;
		}
		if (name == PropertyName.rangeSize)
		{
			value = VariantUtils.CreateFrom(in rangeSize);
			return true;
		}
		if (name == PropertyName.hitPesontage)
		{
			value = VariantUtils.CreateFrom(in hitPesontage);
			return true;
		}
		if (name == PropertyName.isFire)
		{
			value = VariantUtils.CreateFrom(in isFire);
			return true;
		}
		if (name == PropertyName.isStar)
		{
			value = VariantUtils.CreateFrom(in isStar);
			return true;
		}
		if (name == PropertyName.isMagic)
		{
			value = VariantUtils.CreateFrom(in isMagic);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.name, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.baseDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.size, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.scale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.projectileScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.splatSceneType, PropertyHint.Enum, "Particles,Sprite", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.splatAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.splatScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.hitEffect, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitTargetEventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitCharacterEventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitGroundEventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Init", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.blockHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.rotateFollowVelocity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.rotateScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hitBody, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Range", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.rangeType, PropertyHint.Enum, "Default,Bomb", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.rangeSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitPesontage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Flag", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isFire, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isStar, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isMagic, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.name, Variant.From(in name));
		info.AddProperty(PropertyName.baseDamage, Variant.From(in baseDamage));
		info.AddProperty(PropertyName.size, Variant.From(in size));
		info.AddProperty(PropertyName.scale, Variant.From(in scale));
		info.AddProperty(PropertyName.projectileScene, Variant.From(in projectileScene));
		info.AddProperty(PropertyName.splatSceneType, Variant.From(in splatSceneType));
		info.AddProperty(PropertyName.splatAudio, Variant.From(in splatAudio));
		info.AddProperty(PropertyName.splatScene, Variant.From(in splatScene));
		info.AddProperty(PropertyName.hitEffect, Variant.From(in hitEffect));
		info.AddProperty(PropertyName.hitTargetEventList, Variant.CreateFrom(hitTargetEventList));
		info.AddProperty(PropertyName.hitCharacterEventList, Variant.CreateFrom(hitCharacterEventList));
		info.AddProperty(PropertyName.hitGroundEventList, Variant.CreateFrom(hitGroundEventList));
		info.AddProperty(PropertyName.blockHurt, Variant.From(in blockHurt));
		info.AddProperty(PropertyName.rotateFollowVelocity, Variant.From(in rotateFollowVelocity));
		info.AddProperty(PropertyName.rotateScale, Variant.From(in rotateScale));
		info.AddProperty(PropertyName.hitBody, Variant.From(in hitBody));
		info.AddProperty(PropertyName.rangeType, Variant.From(in rangeType));
		info.AddProperty(PropertyName.useRange, Variant.From(in useRange));
		info.AddProperty(PropertyName.rangeSize, Variant.From(in rangeSize));
		info.AddProperty(PropertyName.hitPesontage, Variant.From(in hitPesontage));
		info.AddProperty(PropertyName.isFire, Variant.From(in isFire));
		info.AddProperty(PropertyName.isStar, Variant.From(in isStar));
		info.AddProperty(PropertyName.isMagic, Variant.From(in isMagic));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.name, out var value))
		{
			name = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.baseDamage, out var value2))
		{
			baseDamage = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.size, out var value3))
		{
			size = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.scale, out var value4))
		{
			scale = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.projectileScene, out var value5))
		{
			projectileScene = value5.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.splatSceneType, out var value6))
		{
			splatSceneType = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.splatAudio, out var value7))
		{
			splatAudio = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.splatScene, out var value8))
		{
			splatScene = value8.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.hitEffect, out var value9))
		{
			hitEffect = value9.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.hitTargetEventList, out var value10))
		{
			hitTargetEventList = value10.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.hitCharacterEventList, out var value11))
		{
			hitCharacterEventList = value11.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.hitGroundEventList, out var value12))
		{
			hitGroundEventList = value12.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.blockHurt, out var value13))
		{
			blockHurt = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.rotateFollowVelocity, out var value14))
		{
			rotateFollowVelocity = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.rotateScale, out var value15))
		{
			rotateScale = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitBody, out var value16))
		{
			hitBody = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.rangeType, out var value17))
		{
			rangeType = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName.useRange, out var value18))
		{
			useRange = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.rangeSize, out var value19))
		{
			rangeSize = value19.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.hitPesontage, out var value20))
		{
			hitPesontage = value20.As<double>();
		}
		if (info.TryGetProperty(PropertyName.isFire, out var value21))
		{
			isFire = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isStar, out var value22))
		{
			isStar = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isMagic, out var value23))
		{
			isMagic = value23.As<bool>();
		}
	}
}
