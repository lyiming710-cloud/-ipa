using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Projectile/TowerDefenseProjectileConfig.cs")]
public class TowerDefenseProjectileConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName RebuildHitCharacterEvents = "RebuildHitCharacterEvents";

		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";

		public static readonly StringName CanCollision = "CanCollision";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName NameSN = "NameSN";

		public static readonly StringName hitCharacterEventList = "hitCharacterEventList";

		public static readonly StringName HitCharacterEvents = "HitCharacterEvents";

		public static readonly StringName rangeType = "rangeType";

		public static readonly StringName UsesDefaultDamage = "UsesDefaultDamage";

		public static readonly StringName UsesExplosionDamage = "UsesExplosionDamage";

		public static readonly StringName name = "name";

		public static readonly StringName isStar = "isStar";

		public static readonly StringName _nameSN = "_nameSN";

		public static readonly StringName skinName = "skinName";

		public static readonly StringName size = "size";

		public static readonly StringName scale = "scale";

		public static readonly StringName baseDamage = "baseDamage";

		public static readonly StringName projectileScene = "projectileScene";

		public static readonly StringName projectileObject = "projectileObject";

		public static readonly StringName splatSceneType = "splatSceneType";

		public static readonly StringName splatAudio = "splatAudio";

		public static readonly StringName splatScene = "splatScene";

		public static readonly StringName hitEffect = "hitEffect";

		public static readonly StringName hitTargetEventList = "hitTargetEventList";

		public static readonly StringName _hitCharacterEventList = "_hitCharacterEventList";

		public static readonly StringName _hitCharacterEvents = "_hitCharacterEvents";

		public static readonly StringName hitGroundEventList = "hitGroundEventList";

		public static readonly StringName hitChestsScale = "hitChestsScale";

		public static readonly StringName hitNutScale = "hitNutScale";

		public static readonly StringName hitFrozenScale = "hitFrozenScale";

		public static readonly StringName hitMachineScale = "hitMachineScale";

		public static readonly StringName blockHurt = "blockHurt";

		public static readonly StringName rotateFollowVelocity = "rotateFollowVelocity";

		public static readonly StringName rotateScale = "rotateScale";

		public static readonly StringName hitBody = "hitBody";

		public static readonly StringName _rangeType = "_rangeType";

		public static readonly StringName _usesDefaultDamage = "_usesDefaultDamage";

		public static readonly StringName _usesExplosionDamage = "_usesExplosionDamage";

		public static readonly StringName useRange = "useRange";

		public static readonly StringName rangeSize = "rangeSize";

		public static readonly StringName hitPesontage = "hitPesontage";

		public static readonly StringName trackSearchInterval = "trackSearchInterval";

		public static readonly StringName damageFlags = "damageFlags";

		public static readonly StringName fireMethodFlags = "fireMethodFlags";

		public static readonly StringName collisionFlags = "collisionFlags";

		public static readonly StringName catapultHeight = "catapultHeight";

		public static readonly StringName penetrateNum = "penetrateNum";

		public static readonly StringName penetrateOverBack = "penetrateOverBack";

		public static readonly StringName useDurabilityBlockingSweep = "useDurabilityBlockingSweep";

		public static readonly StringName durabilityBlockingThreshold = "durabilityBlockingThreshold";

		public static readonly StringName backOutGround = "backOutGround";

		public static readonly StringName backDuration = "backDuration";

		public static readonly StringName behaviorIds = "behaviorIds";

		public static readonly StringName behaviors = "behaviors";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string name = "";

	[Export(PropertyHint.None, "")]
	public bool isStar;

	private StringName _nameSN;

	[Export(PropertyHint.None, "")]
	public StringName skinName = new StringName("Default");

	[Export(PropertyHint.None, "")]
	public Vector2 size = new Vector2(28f, 28f);

	[Export(PropertyHint.None, "")]
	public Vector2 scale = new Vector2(1f, 1f);

	[Export(PropertyHint.None, "")]
	public double baseDamage = 20.0;

	[Export(PropertyHint.None, "")]
	public PackedScene projectileScene;

	[Export(PropertyHint.None, "")]
	public ObjectManagerConfig.OBJECT projectileObject;

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

	private Array<TowerDefenseCharacterEventBase> _hitCharacterEventList = new Array<TowerDefenseCharacterEventBase>();

	private TowerDefenseCharacterEventBase[] _hitCharacterEvents = System.Array.Empty<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> hitGroundEventList = new Array<TowerDefenseCharacterEventBase>();

	[ExportGroup("Init", "")]
	[Export(PropertyHint.None, "")]
	public double hitChestsScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double hitNutScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double hitFrozenScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double hitMachineScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double blockHurt = -1.0;

	[Export(PropertyHint.None, "")]
	public bool rotateFollowVelocity;

	[Export(PropertyHint.None, "")]
	public double rotateScale;

	[Export(PropertyHint.None, "")]
	public bool hitBody;

	[ExportGroup("Range", "")]
	private string _rangeType = "Default";

	private bool _usesDefaultDamage = true;

	private bool _usesExplosionDamage;

	[Export(PropertyHint.None, "")]
	public bool useRange;

	[Export(PropertyHint.None, "")]
	public Vector2 rangeSize = new Vector2(0.5f, 0.5f);

	[Export(PropertyHint.None, "")]
	public double hitPesontage = 0.25;

	[ExportGroup("Track", "")]
	[Export(PropertyHint.None, "")]
	public int trackSearchInterval = 15;

	[ExportGroup("", "")]
	[Export(PropertyHint.None, "")]
	public int damageFlags = 3;

	[Export(PropertyHint.None, "")]
	public int fireMethodFlags = 1;

	[Export(PropertyHint.None, "")]
	public int collisionFlags = 9;

	[Export(PropertyHint.None, "")]
	public double catapultHeight = 300.0;

	[Export(PropertyHint.None, "")]
	public int penetrateNum = 3;

	[Export(PropertyHint.None, "")]
	public bool penetrateOverBack = true;

	[Export(PropertyHint.None, "")]
	public bool useDurabilityBlockingSweep;

	[Export(PropertyHint.None, "")]
	public double durabilityBlockingThreshold = 2000.0;

	[Export(PropertyHint.None, "")]
	public bool backOutGround = true;

	[Export(PropertyHint.None, "")]
	public double backDuration = 1.0;

	[ExportCategory("Behavior")]
	[Export(PropertyHint.None, "")]
	public Array<StringName> behaviorIds = new Array<StringName>();

	[Export(PropertyHint.None, "")]
	public Array<ProjectileBehaviorDefinition> behaviors = new Array<ProjectileBehaviorDefinition>();

	public StringName NameSN => _nameSN ?? (_nameSN = new StringName(name));

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> hitCharacterEventList
	{
		get
		{
			return _hitCharacterEventList;
		}
		set
		{
			_hitCharacterEventList = value ?? new Array<TowerDefenseCharacterEventBase>();
			RebuildHitCharacterEvents();
		}
	}

	internal TowerDefenseCharacterEventBase[] HitCharacterEvents => _hitCharacterEvents;

	[Export(PropertyHint.Enum, "Default,Bomb")]
	public string rangeType
	{
		get
		{
			return _rangeType;
		}
		set
		{
			_rangeType = value ?? "Default";
			_usesDefaultDamage = string.Equals(_rangeType, "Default", StringComparison.Ordinal);
			_usesExplosionDamage = string.Equals(_rangeType, "Bomb", StringComparison.Ordinal);
		}
	}

	internal bool UsesDefaultDamage => _usesDefaultDamage;

	internal bool UsesExplosionDamage => _usesExplosionDamage;

	private void RebuildHitCharacterEvents()
	{
		int count = _hitCharacterEventList.Count;
		if (count == 0)
		{
			_hitCharacterEvents = System.Array.Empty<TowerDefenseCharacterEventBase>();
			return;
		}
		_hitCharacterEvents = new TowerDefenseCharacterEventBase[count];
		for (int i = 0; i < count; i++)
		{
			_hitCharacterEvents[i] = _hitCharacterEventList[i];
		}
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		long num = 4L;
		Array<Dictionary> array = new Array<Dictionary>();
		string[] names = Enum.GetNames<TowerDefenseEnum.PROJECTILE_DAMAGE_FLAG>();
		array.Add(new Dictionary
		{
			["name"] = "Flag/Damage",
			["type"] = 2,
			["hint"] = 6,
			["hint_string"] = string.Join(",", names),
			["usage"] = num
		});
		string[] names2 = Enum.GetNames<TowerDefenseEnum.PROJECTILE_FIRE_METHOD_FLAG>();
		array.Add(new Dictionary
		{
			["name"] = "Flag/FireMethod",
			["type"] = 2,
			["hint"] = 6,
			["hint_string"] = string.Join(",", names2),
			["usage"] = num
		});
		string[] names3 = Enum.GetNames<TowerDefenseEnum.CHARACTER_COLLISION_FLAGS>();
		array.Add(new Dictionary
		{
			["name"] = "Flag/Collision",
			["type"] = 2,
			["hint"] = 6,
			["hint_string"] = string.Join(",", names3),
			["usage"] = num
		});
		bool flag = (fireMethodFlags & 2) != 0;
		bool flag2 = (fireMethodFlags & 4) != 0;
		bool flag3 = (fireMethodFlags & 8) != 0;
		if (flag)
		{
			array.Add(new Dictionary
			{
				["name"] = "Catapult/Height",
				["type"] = 3,
				["usage"] = num
			});
		}
		if (flag2)
		{
			array.Add(new Dictionary
			{
				["name"] = "Penetrate/Num",
				["type"] = 2,
				["usage"] = num
			});
			if (flag3)
			{
				array.Add(new Dictionary
				{
					["name"] = "Penetrate/OverBack",
					["type"] = 1,
					["usage"] = num
				});
			}
		}
		if (flag3)
		{
			array.Add(new Dictionary
			{
				["name"] = "Back/OutOfGround",
				["type"] = 1,
				["usage"] = num
			});
			array.Add(new Dictionary
			{
				["name"] = "Back/Duration",
				["type"] = 3,
				["usage"] = num
			});
		}
		return array;
	}

	public override bool _Set(StringName property, Variant value)
	{
		switch (property.ToString())
		{
		case "methods":
			behaviors = ProjectileBehaviorDefinition.ReadLegacyArray(value);
			return true;
		case "Flag/Damage":
			damageFlags = value.AsInt32();
			return true;
		case "Flag/FireMethod":
			fireMethodFlags = value.AsInt32();
			NotifyPropertyListChanged();
			return true;
		case "Flag/Collision":
			collisionFlags = value.AsInt32();
			return true;
		case "Catapult/Height":
			catapultHeight = value.AsDouble();
			return true;
		case "Penetrate/Num":
			penetrateNum = value.AsInt32();
			return true;
		case "Penetrate/OverBack":
			penetrateOverBack = value.AsBool();
			return true;
		case "Back/OutOfGround":
			backOutGround = value.AsBool();
			return true;
		case "Back/Duration":
			backDuration = value.AsDouble();
			return true;
		default:
			return false;
		}
	}

	public override Variant _Get(StringName property)
	{
		return property.ToString() switch
		{
			"Flag/Damage" => (Variant)damageFlags, 
			"Flag/FireMethod" => fireMethodFlags, 
			"Flag/Collision" => collisionFlags, 
			"Catapult/Height" => catapultHeight, 
			"Penetrate/Num" => penetrateNum, 
			"Penetrate/OverBack" => penetrateOverBack, 
			"Back/OutOfGround" => backOutGround, 
			"Back/Duration" => backDuration, 
			_ => default, 
		};
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		switch (property.ToString())
		{
		case "Catapult/Height":
		case "Flag/FireMethod":
		case "Back/Duration":
		case "Penetrate/Num":
		case "Flag/Damage":
		case "Flag/Collision":
		case "Penetrate/OverBack":
		case "Back/OutOfGround":
			return true;
		default:
			return false;
		}
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		return property.ToString() switch
		{
			"Flag/Damage" => (Variant)3, 
			"Flag/FireMethod" => 1, 
			"Flag/Collision" => 1, 
			"Catapult/Height" => 400.0, 
			"Penetrate/Num" => 3, 
			"Penetrate/OverBack" => true, 
			"Back/OutOfGround" => true, 
			"Back/Duration" => 1.0, 
			_ => default, 
		};
	}

	public bool CanCollision(int maskFlags)
	{
		return (maskFlags & collisionFlags) != 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.RebuildHitCharacterEvents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetPropertyList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._Get, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyCanRevert, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyGetRevert, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanCollision, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maskFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RebuildHitCharacterEvents && args.Count == 0)
		{
			RebuildHitCharacterEvents();
			ret = default;
			return true;
		}
		if (method == MethodName._GetPropertyList && args.Count == 0)
		{
			Array<Dictionary> array = _GetPropertyList();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName._Set && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName._Get && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_Get(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyCanRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(_PropertyCanRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyGetRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_PropertyGetRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.CanCollision && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCollision(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.RebuildHitCharacterEvents)
		{
			return true;
		}
		if (method == MethodName._GetPropertyList)
		{
			return true;
		}
		if (method == MethodName._Set)
		{
			return true;
		}
		if (method == MethodName._Get)
		{
			return true;
		}
		if (method == MethodName._PropertyCanRevert)
		{
			return true;
		}
		if (method == MethodName._PropertyGetRevert)
		{
			return true;
		}
		if (method == MethodName.CanCollision)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.hitCharacterEventList)
		{
			hitCharacterEventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.rangeType)
		{
			rangeType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.name)
		{
			this.name = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.isStar)
		{
			isStar = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._nameSN)
		{
			_nameSN = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.skinName)
		{
			skinName = VariantUtils.ConvertTo<StringName>(in value);
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
		if (name == PropertyName.baseDamage)
		{
			baseDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.projectileScene)
		{
			projectileScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.projectileObject)
		{
			projectileObject = VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in value);
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
		if (name == PropertyName._hitCharacterEventList)
		{
			_hitCharacterEventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName._hitCharacterEvents)
		{
			_hitCharacterEvents = VariantUtils.ConvertToSystemArrayOfGodotObject<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.hitGroundEventList)
		{
			hitGroundEventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.hitChestsScale)
		{
			hitChestsScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitNutScale)
		{
			hitNutScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitFrozenScale)
		{
			hitFrozenScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitMachineScale)
		{
			hitMachineScale = VariantUtils.ConvertTo<double>(in value);
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
		if (name == PropertyName._rangeType)
		{
			_rangeType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._usesDefaultDamage)
		{
			_usesDefaultDamage = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._usesExplosionDamage)
		{
			_usesExplosionDamage = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.trackSearchInterval)
		{
			trackSearchInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.damageFlags)
		{
			damageFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.fireMethodFlags)
		{
			fireMethodFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			collisionFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.catapultHeight)
		{
			catapultHeight = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.penetrateNum)
		{
			penetrateNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.penetrateOverBack)
		{
			penetrateOverBack = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useDurabilityBlockingSweep)
		{
			useDurabilityBlockingSweep = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.durabilityBlockingThreshold)
		{
			durabilityBlockingThreshold = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.backOutGround)
		{
			backOutGround = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.backDuration)
		{
			backDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.behaviorIds)
		{
			behaviorIds = VariantUtils.ConvertToArray<StringName>(in value);
			return true;
		}
		if (name == PropertyName.behaviors)
		{
			behaviors = VariantUtils.ConvertToArray<ProjectileBehaviorDefinition>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.NameSN)
		{
			value = VariantUtils.CreateFrom<StringName>(NameSN);
			return true;
		}
		if (name == PropertyName.hitCharacterEventList)
		{
			value = VariantUtils.CreateFromArray(hitCharacterEventList);
			return true;
		}
		if (name == PropertyName.HitCharacterEvents)
		{
			GodotObject[] hitCharacterEvents = HitCharacterEvents;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(hitCharacterEvents);
			return true;
		}
		if (name == PropertyName.rangeType)
		{
			value = VariantUtils.CreateFrom<string>(rangeType);
			return true;
		}
		bool from;
		if (name == PropertyName.UsesDefaultDamage)
		{
			from = UsesDefaultDamage;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.UsesExplosionDamage)
		{
			from = UsesExplosionDamage;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.name)
		{
			value = VariantUtils.CreateFrom(in this.name);
			return true;
		}
		if (name == PropertyName.isStar)
		{
			value = VariantUtils.CreateFrom(in isStar);
			return true;
		}
		if (name == PropertyName._nameSN)
		{
			value = VariantUtils.CreateFrom(in _nameSN);
			return true;
		}
		if (name == PropertyName.skinName)
		{
			value = VariantUtils.CreateFrom(in skinName);
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
		if (name == PropertyName.baseDamage)
		{
			value = VariantUtils.CreateFrom(in baseDamage);
			return true;
		}
		if (name == PropertyName.projectileScene)
		{
			value = VariantUtils.CreateFrom(in projectileScene);
			return true;
		}
		if (name == PropertyName.projectileObject)
		{
			value = VariantUtils.CreateFrom(in projectileObject);
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
		if (name == PropertyName._hitCharacterEventList)
		{
			value = VariantUtils.CreateFromArray(_hitCharacterEventList);
			return true;
		}
		if (name == PropertyName._hitCharacterEvents)
		{
			GodotObject[] hitCharacterEvents = _hitCharacterEvents;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(hitCharacterEvents);
			return true;
		}
		if (name == PropertyName.hitGroundEventList)
		{
			value = VariantUtils.CreateFromArray(hitGroundEventList);
			return true;
		}
		if (name == PropertyName.hitChestsScale)
		{
			value = VariantUtils.CreateFrom(in hitChestsScale);
			return true;
		}
		if (name == PropertyName.hitNutScale)
		{
			value = VariantUtils.CreateFrom(in hitNutScale);
			return true;
		}
		if (name == PropertyName.hitFrozenScale)
		{
			value = VariantUtils.CreateFrom(in hitFrozenScale);
			return true;
		}
		if (name == PropertyName.hitMachineScale)
		{
			value = VariantUtils.CreateFrom(in hitMachineScale);
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
		if (name == PropertyName._rangeType)
		{
			value = VariantUtils.CreateFrom(in _rangeType);
			return true;
		}
		if (name == PropertyName._usesDefaultDamage)
		{
			value = VariantUtils.CreateFrom(in _usesDefaultDamage);
			return true;
		}
		if (name == PropertyName._usesExplosionDamage)
		{
			value = VariantUtils.CreateFrom(in _usesExplosionDamage);
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
		if (name == PropertyName.trackSearchInterval)
		{
			value = VariantUtils.CreateFrom(in trackSearchInterval);
			return true;
		}
		if (name == PropertyName.damageFlags)
		{
			value = VariantUtils.CreateFrom(in damageFlags);
			return true;
		}
		if (name == PropertyName.fireMethodFlags)
		{
			value = VariantUtils.CreateFrom(in fireMethodFlags);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			value = VariantUtils.CreateFrom(in collisionFlags);
			return true;
		}
		if (name == PropertyName.catapultHeight)
		{
			value = VariantUtils.CreateFrom(in catapultHeight);
			return true;
		}
		if (name == PropertyName.penetrateNum)
		{
			value = VariantUtils.CreateFrom(in penetrateNum);
			return true;
		}
		if (name == PropertyName.penetrateOverBack)
		{
			value = VariantUtils.CreateFrom(in penetrateOverBack);
			return true;
		}
		if (name == PropertyName.useDurabilityBlockingSweep)
		{
			value = VariantUtils.CreateFrom(in useDurabilityBlockingSweep);
			return true;
		}
		if (name == PropertyName.durabilityBlockingThreshold)
		{
			value = VariantUtils.CreateFrom(in durabilityBlockingThreshold);
			return true;
		}
		if (name == PropertyName.backOutGround)
		{
			value = VariantUtils.CreateFrom(in backOutGround);
			return true;
		}
		if (name == PropertyName.backDuration)
		{
			value = VariantUtils.CreateFrom(in backDuration);
			return true;
		}
		if (name == PropertyName.behaviorIds)
		{
			value = VariantUtils.CreateFromArray(behaviorIds);
			return true;
		}
		if (name == PropertyName.behaviors)
		{
			value = VariantUtils.CreateFromArray(behaviors);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.isStar, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName._nameSN, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.NameSN, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.skinName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.size, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.scale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.baseDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.projectileScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.projectileObject, PropertyHint.Enum, "NOONE:0,PROJECTILE:1,damagePart:2,GAMECOLLECT:10,SUN:11,SUN_BRAIN:12,SUN_JALAPENO:13,SUN_QX:14,SUN_MAGIC:15,COIN:20,COIN_SILVER:21,COIN_GOLD:22,COIN_DIAMOND:23,COIN_LUCKY_BAG:24,COIN_TQ:25,COIN_YB1:26,COIN_YB2:27,COIN_GOLD_SHARD:28,PARTICLES_SPLASH:201,PARTICLES_RISE_DIRT:202,PARTICLES_ICE_TRAP:203,SHOW_HEALTH_VIEW:301,MAX:1000", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.splatSceneType, PropertyHint.Enum, "Particles,Sprite", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.splatAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.splatScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.hitEffect, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitTargetEventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName._hitCharacterEventList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._hitCharacterEvents, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitCharacterEventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.HitCharacterEvents, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitGroundEventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Init", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitChestsScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitNutScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitFrozenScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitMachineScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.blockHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.rotateFollowVelocity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.rotateScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hitBody, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Range", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._rangeType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._usesDefaultDamage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._usesExplosionDamage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.rangeType, PropertyHint.Enum, "Default,Bomb", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UsesDefaultDamage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UsesExplosionDamage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.rangeSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitPesontage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Track", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.trackSearchInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.damageFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireMethodFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.catapultHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.penetrateNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.penetrateOverBack, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useDurabilityBlockingSweep, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.durabilityBlockingThreshold, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.backOutGround, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.backDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Behavior", PropertyHint.None, "", PropertyUsageFlags.Category, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.behaviorIds, PropertyHint.TypeString, "21/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.behaviors, PropertyHint.TypeString, "24/17:ProjectileBehaviorDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.hitCharacterEventList, Variant.CreateFrom(hitCharacterEventList));
		info.AddProperty(PropertyName.rangeType, Variant.From<string>(rangeType));
		info.AddProperty(PropertyName.name, Variant.From(in name));
		info.AddProperty(PropertyName.isStar, Variant.From(in isStar));
		info.AddProperty(PropertyName._nameSN, Variant.From(in _nameSN));
		info.AddProperty(PropertyName.skinName, Variant.From(in skinName));
		info.AddProperty(PropertyName.size, Variant.From(in size));
		info.AddProperty(PropertyName.scale, Variant.From(in scale));
		info.AddProperty(PropertyName.baseDamage, Variant.From(in baseDamage));
		info.AddProperty(PropertyName.projectileScene, Variant.From(in projectileScene));
		info.AddProperty(PropertyName.projectileObject, Variant.From(in projectileObject));
		info.AddProperty(PropertyName.splatSceneType, Variant.From(in splatSceneType));
		info.AddProperty(PropertyName.splatAudio, Variant.From(in splatAudio));
		info.AddProperty(PropertyName.splatScene, Variant.From(in splatScene));
		info.AddProperty(PropertyName.hitEffect, Variant.From(in hitEffect));
		info.AddProperty(PropertyName.hitTargetEventList, Variant.CreateFrom(hitTargetEventList));
		info.AddProperty(PropertyName._hitCharacterEventList, Variant.CreateFrom(_hitCharacterEventList));
		StringName hitCharacterEvents = PropertyName._hitCharacterEvents;
		GodotObject[] hitCharacterEvents2 = _hitCharacterEvents;
		info.AddProperty(hitCharacterEvents, Variant.CreateFrom(hitCharacterEvents2));
		info.AddProperty(PropertyName.hitGroundEventList, Variant.CreateFrom(hitGroundEventList));
		info.AddProperty(PropertyName.hitChestsScale, Variant.From(in hitChestsScale));
		info.AddProperty(PropertyName.hitNutScale, Variant.From(in hitNutScale));
		info.AddProperty(PropertyName.hitFrozenScale, Variant.From(in hitFrozenScale));
		info.AddProperty(PropertyName.hitMachineScale, Variant.From(in hitMachineScale));
		info.AddProperty(PropertyName.blockHurt, Variant.From(in blockHurt));
		info.AddProperty(PropertyName.rotateFollowVelocity, Variant.From(in rotateFollowVelocity));
		info.AddProperty(PropertyName.rotateScale, Variant.From(in rotateScale));
		info.AddProperty(PropertyName.hitBody, Variant.From(in hitBody));
		info.AddProperty(PropertyName._rangeType, Variant.From(in _rangeType));
		info.AddProperty(PropertyName._usesDefaultDamage, Variant.From(in _usesDefaultDamage));
		info.AddProperty(PropertyName._usesExplosionDamage, Variant.From(in _usesExplosionDamage));
		info.AddProperty(PropertyName.useRange, Variant.From(in useRange));
		info.AddProperty(PropertyName.rangeSize, Variant.From(in rangeSize));
		info.AddProperty(PropertyName.hitPesontage, Variant.From(in hitPesontage));
		info.AddProperty(PropertyName.trackSearchInterval, Variant.From(in trackSearchInterval));
		info.AddProperty(PropertyName.damageFlags, Variant.From(in damageFlags));
		info.AddProperty(PropertyName.fireMethodFlags, Variant.From(in fireMethodFlags));
		info.AddProperty(PropertyName.collisionFlags, Variant.From(in collisionFlags));
		info.AddProperty(PropertyName.catapultHeight, Variant.From(in catapultHeight));
		info.AddProperty(PropertyName.penetrateNum, Variant.From(in penetrateNum));
		info.AddProperty(PropertyName.penetrateOverBack, Variant.From(in penetrateOverBack));
		info.AddProperty(PropertyName.useDurabilityBlockingSweep, Variant.From(in useDurabilityBlockingSweep));
		info.AddProperty(PropertyName.durabilityBlockingThreshold, Variant.From(in durabilityBlockingThreshold));
		info.AddProperty(PropertyName.backOutGround, Variant.From(in backOutGround));
		info.AddProperty(PropertyName.backDuration, Variant.From(in backDuration));
		info.AddProperty(PropertyName.behaviorIds, Variant.CreateFrom(behaviorIds));
		info.AddProperty(PropertyName.behaviors, Variant.CreateFrom(behaviors));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.hitCharacterEventList, out var value))
		{
			hitCharacterEventList = value.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.rangeType, out var value2))
		{
			rangeType = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.name, out var value3))
		{
			name = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.isStar, out var value4))
		{
			isStar = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._nameSN, out var value5))
		{
			_nameSN = value5.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.skinName, out var value6))
		{
			skinName = value6.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.size, out var value7))
		{
			size = value7.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.scale, out var value8))
		{
			scale = value8.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.baseDamage, out var value9))
		{
			baseDamage = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.projectileScene, out var value10))
		{
			projectileScene = value10.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.projectileObject, out var value11))
		{
			projectileObject = value11.As<ObjectManagerConfig.OBJECT>();
		}
		if (info.TryGetProperty(PropertyName.splatSceneType, out var value12))
		{
			splatSceneType = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName.splatAudio, out var value13))
		{
			splatAudio = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName.splatScene, out var value14))
		{
			splatScene = value14.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.hitEffect, out var value15))
		{
			hitEffect = value15.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.hitTargetEventList, out var value16))
		{
			hitTargetEventList = value16.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName._hitCharacterEventList, out var value17))
		{
			_hitCharacterEventList = value17.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName._hitCharacterEvents, out var value18))
		{
			_hitCharacterEvents = value18.AsGodotObjectArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.hitGroundEventList, out var value19))
		{
			hitGroundEventList = value19.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.hitChestsScale, out var value20))
		{
			hitChestsScale = value20.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitNutScale, out var value21))
		{
			hitNutScale = value21.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitFrozenScale, out var value22))
		{
			hitFrozenScale = value22.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitMachineScale, out var value23))
		{
			hitMachineScale = value23.As<double>();
		}
		if (info.TryGetProperty(PropertyName.blockHurt, out var value24))
		{
			blockHurt = value24.As<double>();
		}
		if (info.TryGetProperty(PropertyName.rotateFollowVelocity, out var value25))
		{
			rotateFollowVelocity = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.rotateScale, out var value26))
		{
			rotateScale = value26.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitBody, out var value27))
		{
			hitBody = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rangeType, out var value28))
		{
			_rangeType = value28.As<string>();
		}
		if (info.TryGetProperty(PropertyName._usesDefaultDamage, out var value29))
		{
			_usesDefaultDamage = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._usesExplosionDamage, out var value30))
		{
			_usesExplosionDamage = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useRange, out var value31))
		{
			useRange = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.rangeSize, out var value32))
		{
			rangeSize = value32.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.hitPesontage, out var value33))
		{
			hitPesontage = value33.As<double>();
		}
		if (info.TryGetProperty(PropertyName.trackSearchInterval, out var value34))
		{
			trackSearchInterval = value34.As<int>();
		}
		if (info.TryGetProperty(PropertyName.damageFlags, out var value35))
		{
			damageFlags = value35.As<int>();
		}
		if (info.TryGetProperty(PropertyName.fireMethodFlags, out var value36))
		{
			fireMethodFlags = value36.As<int>();
		}
		if (info.TryGetProperty(PropertyName.collisionFlags, out var value37))
		{
			collisionFlags = value37.As<int>();
		}
		if (info.TryGetProperty(PropertyName.catapultHeight, out var value38))
		{
			catapultHeight = value38.As<double>();
		}
		if (info.TryGetProperty(PropertyName.penetrateNum, out var value39))
		{
			penetrateNum = value39.As<int>();
		}
		if (info.TryGetProperty(PropertyName.penetrateOverBack, out var value40))
		{
			penetrateOverBack = value40.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useDurabilityBlockingSweep, out var value41))
		{
			useDurabilityBlockingSweep = value41.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.durabilityBlockingThreshold, out var value42))
		{
			durabilityBlockingThreshold = value42.As<double>();
		}
		if (info.TryGetProperty(PropertyName.backOutGround, out var value43))
		{
			backOutGround = value43.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.backDuration, out var value44))
		{
			backDuration = value44.As<double>();
		}
		if (info.TryGetProperty(PropertyName.behaviorIds, out var value45))
		{
			behaviorIds = value45.AsGodotArray<StringName>();
		}
		if (info.TryGetProperty(PropertyName.behaviors, out var value46))
		{
			behaviors = value46.AsGodotArray<ProjectileBehaviorDefinition>();
		}
	}
}
