using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Registry/Projectile/Resource/TowerDefenseProjectileCreateData.cs")]
public class TowerDefenseProjectileCreateData : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName InvalidateConfigCache = "InvalidateConfigCache";

		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";

		public static readonly StringName SetbaseDamage = "SetbaseDamage";

		public static readonly StringName SetSize = "SetSize";

		public static readonly StringName SetScale = "SetScale";

		public static readonly StringName SetHitChestsScale = "SetHitChestsScale";

		public static readonly StringName SetHitNutScale = "SetHitNutScale";

		public static readonly StringName SetHitFrozenScale = "SetHitFrozenScale";

		public static readonly StringName SetHitMachineScale = "SetHitMachineScale";

		public static readonly StringName SetDamageFlags = "SetDamageFlags";

		public static readonly StringName SetCollisionFlags = "SetCollisionFlags";

		public static readonly StringName SetFireMethodFlags = "SetFireMethodFlags";

		public static readonly StringName SetCatapultHeight = "SetCatapultHeight";

		public static readonly StringName SetPenetrateNum = "SetPenetrateNum";

		public static readonly StringName SetPenetrateOverBack = "SetPenetrateOverBack";

		public static readonly StringName SetBackOutGround = "SetBackOutGround";

		public static readonly StringName SetBackDuration = "SetBackDuration";

		public static readonly StringName SetRotateFollowVelocity = "SetRotateFollowVelocity";

		public static readonly StringName SetRangeOverride = "SetRangeOverride";

		public static readonly StringName SetRangeType = "SetRangeType";

		public static readonly StringName SetUseRange = "SetUseRange";

		public static readonly StringName SetRangeSize = "SetRangeSize";

		public static readonly StringName SetHitPesontage = "SetHitPesontage";

		public static readonly StringName SetOverrideHitTargetEvent = "SetOverrideHitTargetEvent";

		public static readonly StringName SetOverrideHitCharacterEvent = "SetOverrideHitCharacterEvent";

		public static readonly StringName SetOverrideHitGroundEvent = "SetOverrideHitGroundEvent";

		public static readonly StringName BuildConfig = "BuildConfig";

		public static readonly StringName ApplyOverride = "ApplyOverride";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName skinName = "skinName";

		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName _skinName = "_skinName";

		public static readonly StringName size = "size";

		public static readonly StringName scale = "scale";

		public static readonly StringName baseDamage = "baseDamage";

		public static readonly StringName hitChestsScale = "hitChestsScale";

		public static readonly StringName hitNutScale = "hitNutScale";

		public static readonly StringName hitFrozenScale = "hitFrozenScale";

		public static readonly StringName hitMachineScale = "hitMachineScale";

		public static readonly StringName damageFlags = "damageFlags";

		public static readonly StringName collisionFlags = "collisionFlags";

		public static readonly StringName fireMethodFlags = "fireMethodFlags";

		public static readonly StringName catapultHeight = "catapultHeight";

		public static readonly StringName penetrateNum = "penetrateNum";

		public static readonly StringName penetrateOverBack = "penetrateOverBack";

		public static readonly StringName backOutGround = "backOutGround";

		public static readonly StringName backDuration = "backDuration";

		public static readonly StringName rotateFollowVelocity = "rotateFollowVelocity";

		public static readonly StringName overrideHitChestsScale = "overrideHitChestsScale";

		public static readonly StringName overrideHitNutScale = "overrideHitNutScale";

		public static readonly StringName overrideHitFrozenScale = "overrideHitFrozenScale";

		public static readonly StringName overrideHitMachineScale = "overrideHitMachineScale";

		public static readonly StringName overrideCatapultHeight = "overrideCatapultHeight";

		public static readonly StringName overridePenetrateNum = "overridePenetrateNum";

		public static readonly StringName overridePenetrateOverBack = "overridePenetrateOverBack";

		public static readonly StringName overrideBackOutGround = "overrideBackOutGround";

		public static readonly StringName overrideBackDuration = "overrideBackDuration";

		public static readonly StringName overrideRotateFollowVelocity = "overrideRotateFollowVelocity";

		public static readonly StringName rangeOverride = "rangeOverride";

		public static readonly StringName rangeType = "rangeType";

		public static readonly StringName useRange = "useRange";

		public static readonly StringName rangeSize = "rangeSize";

		public static readonly StringName hitPesontage = "hitPesontage";

		public static readonly StringName overrideHitTargetEvent = "overrideHitTargetEvent";

		public static readonly StringName overrideHitCharacterEvent = "overrideHitCharacterEvent";

		public static readonly StringName overrideHitGroundEvent = "overrideHitGroundEvent";

		public static readonly StringName hitTargetEventList = "hitTargetEventList";

		public static readonly StringName hitCharacterEventList = "hitCharacterEventList";

		public static readonly StringName hitGroundEventList = "hitGroundEventList";

		public static readonly StringName behaviorIds = "behaviorIds";

		public static readonly StringName behaviors = "behaviors";

		public static readonly StringName _cachedConfig = "_cachedConfig";

		public static readonly StringName _cachedRegistrationRevision = "_cachedRegistrationRevision";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public StringName projectileName;

	private StringName _skinName = new StringName("Default");

	[Export(PropertyHint.None, "")]
	public Vector2 size = new Vector2(28f, 28f);

	[Export(PropertyHint.None, "")]
	public Vector2 scale = new Vector2(1f, 1f);

	[Export(PropertyHint.None, "")]
	public double baseDamage = -1.0;

	[Export(PropertyHint.None, "")]
	public double hitChestsScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double hitNutScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double hitFrozenScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double hitMachineScale = 1.0;

	[Export(PropertyHint.None, "")]
	public int damageFlags = 3;

	[Export(PropertyHint.None, "")]
	public int collisionFlags = 1;

	[Export(PropertyHint.None, "")]
	public int fireMethodFlags = 1;

	[Export(PropertyHint.None, "")]
	public double catapultHeight = 300.0;

	[Export(PropertyHint.None, "")]
	public int penetrateNum = 3;

	[Export(PropertyHint.None, "")]
	public bool penetrateOverBack = true;

	[Export(PropertyHint.None, "")]
	public bool backOutGround = true;

	[Export(PropertyHint.None, "")]
	public double backDuration = 1.0;

	[Export(PropertyHint.None, "")]
	public bool rotateFollowVelocity;

	[Export(PropertyHint.None, "")]
	public bool overrideHitChestsScale;

	[Export(PropertyHint.None, "")]
	public bool overrideHitNutScale;

	[Export(PropertyHint.None, "")]
	public bool overrideHitFrozenScale;

	[Export(PropertyHint.None, "")]
	public bool overrideHitMachineScale;

	[Export(PropertyHint.None, "")]
	public bool overrideCatapultHeight;

	[Export(PropertyHint.None, "")]
	public bool overridePenetrateNum;

	[Export(PropertyHint.None, "")]
	public bool overridePenetrateOverBack;

	[Export(PropertyHint.None, "")]
	public bool overrideBackOutGround;

	[Export(PropertyHint.None, "")]
	public bool overrideBackDuration;

	[Export(PropertyHint.None, "")]
	public bool overrideRotateFollowVelocity;

	[Export(PropertyHint.None, "")]
	public bool rangeOverride;

	[Export(PropertyHint.None, "")]
	public string rangeType = "Default";

	[Export(PropertyHint.None, "")]
	public bool useRange;

	[Export(PropertyHint.None, "")]
	public Vector2 rangeSize = new Vector2(0.5f, 0.5f);

	[Export(PropertyHint.None, "")]
	public double hitPesontage = 0.25;

	[Export(PropertyHint.None, "")]
	public bool overrideHitTargetEvent;

	[Export(PropertyHint.None, "")]
	public bool overrideHitCharacterEvent;

	[Export(PropertyHint.None, "")]
	public bool overrideHitGroundEvent;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> hitTargetEventList = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> hitCharacterEventList = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> hitGroundEventList = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<StringName> behaviorIds = new Array<StringName>();

	[Export(PropertyHint.None, "")]
	public Array<ProjectileBehaviorDefinition> behaviors = new Array<ProjectileBehaviorDefinition>();

	private TowerDefenseProjectileConfig _cachedConfig;

	private long _cachedRegistrationRevision = -1L;

	[Export(PropertyHint.None, "")]
	public StringName skinName
	{
		get
		{
			return _skinName;
		}
		set
		{
			if (!(_skinName == value))
			{
				_skinName = value;
				_cachedConfig = null;
			}
		}
	}

	public TowerDefenseProjectileCreateData()
		: this(null)
	{
	}

	public TowerDefenseProjectileCreateData(StringName _projectileName)
	{
		projectileName = _projectileName;
	}

	public void InvalidateConfigCache()
	{
		_cachedConfig = null;
		_cachedRegistrationRevision = -1L;
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		long num = 4L;
		Array<Dictionary> array = new Array<Dictionary>();
		array.Add(new Dictionary
		{
			["name"] = "Flag/Damage",
			["type"] = 2,
			["hint"] = 6,
			["hint_string"] = string.Join(",", Enum.GetNames<TowerDefenseEnum.PROJECTILE_DAMAGE_FLAG>()),
			["usage"] = num
		});
		array.Add(new Dictionary
		{
			["name"] = "Flag/FireMethod",
			["type"] = 2,
			["hint"] = 6,
			["hint_string"] = string.Join(",", Enum.GetNames<TowerDefenseEnum.PROJECTILE_FIRE_METHOD_FLAG>()),
			["usage"] = num
		});
		array.Add(new Dictionary
		{
			["name"] = "Flag/Collision",
			["type"] = 2,
			["hint"] = 6,
			["hint_string"] = string.Join(",", Enum.GetNames<TowerDefenseEnum.CHARACTER_COLLISION_FLAGS>()),
			["usage"] = num
		});
		array.Add(new Dictionary
		{
			["name"] = "Override/Hit Chests Scale",
			["type"] = 1,
			["usage"] = num
		});
		if (overrideHitChestsScale)
		{
			array.Add(new Dictionary
			{
				["name"] = "HitChests/Scale",
				["type"] = 3,
				["usage"] = num
			});
		}
		array.Add(new Dictionary
		{
			["name"] = "Override/Hit Nut Scale",
			["type"] = 1,
			["usage"] = num
		});
		if (overrideHitNutScale)
		{
			array.Add(new Dictionary
			{
				["name"] = "HitNut/Scale",
				["type"] = 3,
				["usage"] = num
			});
		}
		array.Add(new Dictionary
		{
			["name"] = "Override/Hit Frozen Scale",
			["type"] = 1,
			["usage"] = num
		});
		if (overrideHitFrozenScale)
		{
			array.Add(new Dictionary
			{
				["name"] = "HitFrozen/Scale",
				["type"] = 3,
				["usage"] = num
			});
		}
		array.Add(new Dictionary
		{
			["name"] = "Override/Hit Machine Scale",
			["type"] = 1,
			["usage"] = num
		});
		if (overrideHitMachineScale)
		{
			array.Add(new Dictionary
			{
				["name"] = "HitMachine/Scale",
				["type"] = 3,
				["usage"] = num
			});
		}
		array.Add(new Dictionary
		{
			["name"] = "Override/Catapult Height",
			["type"] = 1,
			["usage"] = num
		});
		bool flag = (fireMethodFlags & 2) != 0;
		if (overrideCatapultHeight & flag)
		{
			array.Add(new Dictionary
			{
				["name"] = "Catapult/Height",
				["type"] = 3,
				["usage"] = num
			});
		}
		array.Add(new Dictionary
		{
			["name"] = "Override/Penetrate Num",
			["type"] = 1,
			["usage"] = num
		});
		bool flag2 = (fireMethodFlags & 4) != 0;
		if (overridePenetrateNum & flag2)
		{
			array.Add(new Dictionary
			{
				["name"] = "Penetrate/Num",
				["type"] = 2,
				["usage"] = num
			});
		}
		array.Add(new Dictionary
		{
			["name"] = "Override/Penetrate Over Back",
			["type"] = 1,
			["usage"] = num
		});
		bool flag3 = (fireMethodFlags & 8) != 0;
		if (overridePenetrateOverBack & flag2 & flag3)
		{
			array.Add(new Dictionary
			{
				["name"] = "Penetrate/OverBack",
				["type"] = 1,
				["usage"] = num
			});
		}
		array.Add(new Dictionary
		{
			["name"] = "Override/Back Out Ground",
			["type"] = 1,
			["usage"] = num
		});
		if (overrideBackOutGround & flag3)
		{
			array.Add(new Dictionary
			{
				["name"] = "Back/OutOfGround",
				["type"] = 1,
				["usage"] = num
			});
		}
		array.Add(new Dictionary
		{
			["name"] = "Override/Back Duration",
			["type"] = 1,
			["usage"] = num
		});
		if (overrideBackDuration & flag3)
		{
			array.Add(new Dictionary
			{
				["name"] = "Back/Duration",
				["type"] = 3,
				["usage"] = num
			});
		}
		array.Add(new Dictionary
		{
			["name"] = "Override/Rotate Follow Velocity",
			["type"] = 1,
			["usage"] = num
		});
		if (overrideRotateFollowVelocity)
		{
			array.Add(new Dictionary
			{
				["name"] = "Rotate/FollowVelocity",
				["type"] = 1,
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
			InvalidateConfigCache();
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
		case "Override/Hit Chests Scale":
			overrideHitChestsScale = value.AsBool();
			NotifyPropertyListChanged();
			return true;
		case "Override/Hit Nut Scale":
			overrideHitNutScale = value.AsBool();
			NotifyPropertyListChanged();
			return true;
		case "Override/Hit Frozen Scale":
			overrideHitFrozenScale = value.AsBool();
			NotifyPropertyListChanged();
			return true;
		case "Override/Hit Machine Scale":
			overrideHitMachineScale = value.AsBool();
			NotifyPropertyListChanged();
			return true;
		case "Override/Catapult Height":
			overrideCatapultHeight = value.AsBool();
			NotifyPropertyListChanged();
			return true;
		case "Override/Penetrate Num":
			overridePenetrateNum = value.AsBool();
			NotifyPropertyListChanged();
			return true;
		case "Override/Penetrate Over Back":
			overridePenetrateOverBack = value.AsBool();
			NotifyPropertyListChanged();
			return true;
		case "Override/Back Out Ground":
			overrideBackOutGround = value.AsBool();
			NotifyPropertyListChanged();
			return true;
		case "Override/Back Duration":
			overrideBackDuration = value.AsBool();
			NotifyPropertyListChanged();
			return true;
		case "Override/Rotate Follow Velocity":
			overrideRotateFollowVelocity = value.AsBool();
			NotifyPropertyListChanged();
			return true;
		case "HitChests/Scale":
			hitChestsScale = value.AsDouble();
			return true;
		case "HitNut/Scale":
			hitNutScale = value.AsDouble();
			return true;
		case "HitFrozen/Scale":
			hitFrozenScale = value.AsDouble();
			return true;
		case "HitMachine/Scale":
			hitMachineScale = value.AsDouble();
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
		case "Rotate/FollowVelocity":
			rotateFollowVelocity = value.AsBool();
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
			"Override/Hit Chests Scale" => overrideHitChestsScale, 
			"Override/Hit Nut Scale" => overrideHitNutScale, 
			"Override/Hit Frozen Scale" => overrideHitFrozenScale, 
			"Override/Hit Machine Scale" => overrideHitMachineScale, 
			"Override/Catapult Height" => overrideCatapultHeight, 
			"Override/Penetrate Num" => overridePenetrateNum, 
			"Override/Penetrate Over Back" => overridePenetrateOverBack, 
			"Override/Back Out Ground" => overrideBackOutGround, 
			"Override/Back Duration" => overrideBackDuration, 
			"Override/Rotate Follow Velocity" => overrideRotateFollowVelocity, 
			"HitChests/Scale" => hitChestsScale, 
			"HitNut/Scale" => hitNutScale, 
			"HitFrozen/Scale" => hitFrozenScale, 
			"HitMachine/Scale" => hitMachineScale, 
			"Catapult/Height" => catapultHeight, 
			"Penetrate/Num" => penetrateNum, 
			"Penetrate/OverBack" => penetrateOverBack, 
			"Back/OutOfGround" => backOutGround, 
			"Back/Duration" => backDuration, 
			"Rotate/FollowVelocity" => rotateFollowVelocity, 
			_ => default, 
		};
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		switch (property.ToString())
		{
		case "Catapult/Height":
		case "Flag/FireMethod":
		case "HitChests/Scale":
		case "HitFrozen/Scale":
		case "Override/Hit Chests Scale":
		case "Override/Hit Frozen Scale":
		case "Override/Back Duration":
		case "Override/Hit Nut Scale":
		case "Override/Penetrate Num":
		case "Override/Back Out Ground":
		case "Override/Catapult Height":
		case "Back/OutOfGround":
		case "HitMachine/Scale":
		case "Back/Duration":
		case "Penetrate/Num":
		case "Flag/Damage":
		case "Flag/Collision":
		case "Override/Hit Machine Scale":
		case "Override/Penetrate Over Back":
		case "Override/Rotate Follow Velocity":
		case "HitNut/Scale":
		case "Penetrate/OverBack":
		case "Rotate/FollowVelocity":
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
			"Override/Hit Chests Scale" => false, 
			"Override/Hit Nut Scale" => false, 
			"Override/Hit Frozen Scale" => false, 
			"Override/Hit Machine Scale" => false, 
			"Override/Catapult Height" => false, 
			"Override/Penetrate Num" => false, 
			"Override/Penetrate Over Back" => false, 
			"Override/Back Out Ground" => false, 
			"Override/Back Duration" => false, 
			"Override/Rotate Follow Velocity" => false, 
			"HitChests/Scale" => 1.0, 
			"HitNut/Scale" => 1.0, 
			"HitFrozen/Scale" => 1.0, 
			"HitMachine/Scale" => 1.0, 
			"Catapult/Height" => 400.0, 
			"Penetrate/Num" => 3, 
			"Penetrate/OverBack" => true, 
			"Back/OutOfGround" => true, 
			"Back/Duration" => 1.0, 
			"Rotate/FollowVelocity" => false, 
			_ => default, 
		};
	}

	public void SetbaseDamage(double _baseDamage)
	{
		baseDamage = _baseDamage;
	}

	public void SetSize(Vector2 _size)
	{
		size = _size;
	}

	public void SetScale(Vector2 _scale)
	{
		scale = _scale;
	}

	public void SetHitChestsScale(double _hitChestsScale)
	{
		hitChestsScale = _hitChestsScale;
	}

	public void SetHitNutScale(double _hitNutScale)
	{
		hitNutScale = _hitNutScale;
	}

	public void SetHitFrozenScale(double _hitFrozenScale)
	{
		hitFrozenScale = _hitFrozenScale;
	}

	public void SetHitMachineScale(double _hitMachineScale)
	{
		hitMachineScale = _hitMachineScale;
	}

	public void SetDamageFlags(int _damageFlags)
	{
		damageFlags = _damageFlags;
	}

	public void SetCollisionFlags(int _collisionFlags)
	{
		collisionFlags = _collisionFlags;
	}

	public void SetFireMethodFlags(int _fireMethodFlags)
	{
		fireMethodFlags = _fireMethodFlags;
	}

	public void SetCatapultHeight(double _catapultHeight)
	{
		catapultHeight = _catapultHeight;
	}

	public void SetPenetrateNum(int _penetrateNum)
	{
		penetrateNum = _penetrateNum;
	}

	public void SetPenetrateOverBack(bool _penetrateOverBack)
	{
		penetrateOverBack = _penetrateOverBack;
	}

	public void SetBackOutGround(bool _backOutGround)
	{
		backOutGround = _backOutGround;
	}

	public void SetBackDuration(double _backDuration)
	{
		backDuration = _backDuration;
	}

	public void SetRotateFollowVelocity(bool _rotateFollowVelocity)
	{
		rotateFollowVelocity = _rotateFollowVelocity;
	}

	public void SetRangeOverride(bool _rangeOverride)
	{
		rangeOverride = _rangeOverride;
	}

	public void SetRangeType(string _rangeType)
	{
		rangeType = _rangeType;
	}

	public void SetUseRange(bool _useRange)
	{
		useRange = _useRange;
	}

	public void SetRangeSize(Vector2 _rangeSize)
	{
		rangeSize = _rangeSize;
	}

	public void SetHitPesontage(double _hitPesontage)
	{
		hitPesontage = _hitPesontage;
	}

	public void SetOverrideHitTargetEvent(bool _overrideHitTargetEvent)
	{
		overrideHitTargetEvent = _overrideHitTargetEvent;
	}

	public void SetOverrideHitCharacterEvent(bool _overrideHitCharacterEvent)
	{
		overrideHitCharacterEvent = _overrideHitCharacterEvent;
	}

	public void SetOverrideHitGroundEvent(bool _overrideHitGroundEvent)
	{
		overrideHitGroundEvent = _overrideHitGroundEvent;
	}

	public TowerDefenseProjectileConfig BuildConfig()
	{
		long registrationRevision = TowerDefenseProjectileRegistry.RegistrationRevision;
		if (_cachedConfig != null && !Engine.IsEditorHint() && _cachedRegistrationRevision == registrationRevision)
		{
			return _cachedConfig;
		}
		if (projectileName == null)
		{
			return null;
		}
		TowerDefenseProjectileData projectile = TowerDefenseProjectileRegistry.GetProjectile(projectileName);
		if (projectile != null)
		{
			TowerDefenseProjectileConfig towerDefenseProjectileConfig = new TowerDefenseProjectileConfig();
			towerDefenseProjectileConfig.damageFlags = damageFlags;
			towerDefenseProjectileConfig.collisionFlags = collisionFlags;
			towerDefenseProjectileConfig.fireMethodFlags = fireMethodFlags;
			if (projectile.isFire)
			{
				towerDefenseProjectileConfig.damageFlags |= 4;
			}
			if (projectile.isMagic)
			{
				towerDefenseProjectileConfig.damageFlags |= 64;
			}
			towerDefenseProjectileConfig.name = projectile.name;
			towerDefenseProjectileConfig.isStar = projectile.isStar;
			towerDefenseProjectileConfig.skinName = skinName;
			towerDefenseProjectileConfig.size = projectile.size;
			towerDefenseProjectileConfig.scale = projectile.scale;
			towerDefenseProjectileConfig.projectileScene = projectile.projectileScene;
			towerDefenseProjectileConfig.splatSceneType = projectile.splatSceneType;
			towerDefenseProjectileConfig.splatAudio = projectile.splatAudio;
			towerDefenseProjectileConfig.splatScene = projectile.splatScene;
			towerDefenseProjectileConfig.hitEffect = projectile.hitEffect;
			towerDefenseProjectileConfig.hitTargetEventList = projectile.hitTargetEventList;
			towerDefenseProjectileConfig.hitCharacterEventList = projectile.hitCharacterEventList;
			towerDefenseProjectileConfig.hitGroundEventList = projectile.hitGroundEventList;
			towerDefenseProjectileConfig.blockHurt = projectile.blockHurt;
			towerDefenseProjectileConfig.rotateFollowVelocity = projectile.rotateFollowVelocity;
			towerDefenseProjectileConfig.rotateScale = projectile.rotateScale;
			towerDefenseProjectileConfig.hitBody = projectile.hitBody;
			towerDefenseProjectileConfig.rangeType = projectile.rangeType;
			towerDefenseProjectileConfig.useRange = projectile.useRange;
			towerDefenseProjectileConfig.rangeSize = projectile.rangeSize;
			towerDefenseProjectileConfig.hitPesontage = projectile.hitPesontage;
			towerDefenseProjectileConfig.baseDamage = projectile.baseDamage;
			TowerDefenseProjectileConfig projectileConfig = TowerDefenseManager.GetProjectileConfig(projectileName.ToString());
			if (projectileConfig != null)
			{
				towerDefenseProjectileConfig.penetrateNum = projectileConfig.penetrateNum;
				towerDefenseProjectileConfig.penetrateOverBack = projectileConfig.penetrateOverBack;
				towerDefenseProjectileConfig.backOutGround = projectileConfig.backOutGround;
				towerDefenseProjectileConfig.backDuration = projectileConfig.backDuration;
				towerDefenseProjectileConfig.catapultHeight = projectileConfig.catapultHeight;
				towerDefenseProjectileConfig.splatSceneType = projectileConfig.splatSceneType;
				towerDefenseProjectileConfig.hitChestsScale = projectileConfig.hitChestsScale;
				towerDefenseProjectileConfig.hitNutScale = projectileConfig.hitNutScale;
				towerDefenseProjectileConfig.hitFrozenScale = projectileConfig.hitFrozenScale;
				towerDefenseProjectileConfig.hitMachineScale = projectileConfig.hitMachineScale;
				towerDefenseProjectileConfig.trackSearchInterval = projectileConfig.trackSearchInterval;
				towerDefenseProjectileConfig.useDurabilityBlockingSweep = projectileConfig.useDurabilityBlockingSweep;
				towerDefenseProjectileConfig.durabilityBlockingThreshold = projectileConfig.durabilityBlockingThreshold;
				if (towerDefenseProjectileConfig.projectileScene == null)
				{
					towerDefenseProjectileConfig.projectileScene = projectileConfig.projectileScene;
				}
				if (towerDefenseProjectileConfig.splatScene == null)
				{
					towerDefenseProjectileConfig.splatScene = projectileConfig.splatScene;
				}
				penetrateNum = projectileConfig.penetrateNum;
				penetrateOverBack = projectileConfig.penetrateOverBack;
				backOutGround = projectileConfig.backOutGround;
				backDuration = projectileConfig.backDuration;
				catapultHeight = projectileConfig.catapultHeight;
				hitChestsScale = projectileConfig.hitChestsScale;
				hitNutScale = projectileConfig.hitNutScale;
				hitFrozenScale = projectileConfig.hitFrozenScale;
				hitMachineScale = projectileConfig.hitMachineScale;
				rotateFollowVelocity = projectileConfig.rotateFollowVelocity;
			}
			if (TowerDefenseProjectileRegistry.HasProjectileSkin(projectileName, skinName))
			{
				PackedScene projectileSkinProjectileScene = TowerDefenseProjectileRegistry.GetProjectileSkinProjectileScene(projectileName, skinName);
				if (projectileSkinProjectileScene != null)
				{
					towerDefenseProjectileConfig.projectileScene = projectileSkinProjectileScene;
				}
				PackedScene projectileSkinSplatScene = TowerDefenseProjectileRegistry.GetProjectileSkinSplatScene(projectileName, skinName);
				if (projectileSkinSplatScene != null)
				{
					towerDefenseProjectileConfig.splatScene = projectileSkinSplatScene;
				}
			}
			ApplyOverride(towerDefenseProjectileConfig);
			_cachedConfig = towerDefenseProjectileConfig;
			_cachedRegistrationRevision = registrationRevision;
			return towerDefenseProjectileConfig;
		}
		TowerDefenseProjectileConfig projectileConfig2 = TowerDefenseManager.GetProjectileConfig(projectileName.ToString());
		if (projectileConfig2 != null)
		{
			TowerDefenseProjectileConfig towerDefenseProjectileConfig2 = (TowerDefenseProjectileConfig)projectileConfig2.Duplicate(deep: true);
			if (towerDefenseProjectileConfig2 != null)
			{
				ApplyOverride(towerDefenseProjectileConfig2);
				_cachedConfig = towerDefenseProjectileConfig2;
				_cachedRegistrationRevision = registrationRevision;
				return towerDefenseProjectileConfig2;
			}
		}
		return null;
	}

	public void ApplyOverride(TowerDefenseProjectileConfig config)
	{
		if (baseDamage >= 0.0)
		{
			config.baseDamage = baseDamage;
		}
		config.size = size;
		config.scale = scale;
		if (overrideHitChestsScale)
		{
			config.hitChestsScale = hitChestsScale;
		}
		if (overrideHitNutScale)
		{
			config.hitNutScale = hitNutScale;
		}
		if (overrideHitFrozenScale)
		{
			config.hitFrozenScale = hitFrozenScale;
		}
		if (overrideHitMachineScale)
		{
			config.hitMachineScale = hitMachineScale;
		}
		if (overrideCatapultHeight)
		{
			config.catapultHeight = catapultHeight;
		}
		if (overridePenetrateNum)
		{
			config.penetrateNum = penetrateNum;
		}
		if (overridePenetrateOverBack)
		{
			config.penetrateOverBack = penetrateOverBack;
		}
		if (overrideBackOutGround)
		{
			config.backOutGround = backOutGround;
		}
		if (overrideBackDuration)
		{
			config.backDuration = backDuration;
		}
		if (overrideRotateFollowVelocity)
		{
			config.rotateFollowVelocity = rotateFollowVelocity;
		}
		if (behaviors.Count > 0)
		{
			config.behaviors = behaviors;
		}
		if (behaviorIds.Count > 0)
		{
			config.behaviorIds = behaviorIds;
		}
		if (rangeOverride)
		{
			config.rangeType = rangeType;
			config.useRange = useRange;
			config.rangeSize = rangeSize;
			config.hitPesontage = hitPesontage;
		}
		if (overrideHitTargetEvent)
		{
			config.hitTargetEventList = hitTargetEventList;
		}
		if (overrideHitCharacterEvent)
		{
			config.hitCharacterEventList = hitCharacterEventList;
		}
		if (overrideHitGroundEvent)
		{
			config.hitGroundEventList = hitGroundEventList;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(32)
		{
			new MethodInfo(MethodName.InvalidateConfigCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.SetbaseDamage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_baseDamage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "_size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "_scale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHitChestsScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_hitChestsScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHitNutScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_hitNutScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHitFrozenScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_hitFrozenScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHitMachineScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_hitMachineScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetDamageFlags, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCollisionFlags, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_collisionFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFireMethodFlags, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_fireMethodFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCatapultHeight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_catapultHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPenetrateNum, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_penetrateNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPenetrateOverBack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "_penetrateOverBack", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetBackOutGround, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "_backOutGround", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetBackDuration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_backDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRotateFollowVelocity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "_rotateFollowVelocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRangeOverride, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "_rangeOverride", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRangeType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "_rangeType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetUseRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "_useRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRangeSize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "_rangeSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetHitPesontage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "_hitPesontage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetOverrideHitTargetEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "_overrideHitTargetEvent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetOverrideHitCharacterEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "_overrideHitCharacterEvent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetOverrideHitGroundEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "_overrideHitGroundEvent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyOverride, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.InvalidateConfigCache && args.Count == 0)
		{
			InvalidateConfigCache();
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
		if (method == MethodName.SetbaseDamage && args.Count == 1)
		{
			SetbaseDamage(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSize && args.Count == 1)
		{
			SetSize(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetScale && args.Count == 1)
		{
			SetScale(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHitChestsScale && args.Count == 1)
		{
			SetHitChestsScale(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHitNutScale && args.Count == 1)
		{
			SetHitNutScale(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHitFrozenScale && args.Count == 1)
		{
			SetHitFrozenScale(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHitMachineScale && args.Count == 1)
		{
			SetHitMachineScale(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDamageFlags && args.Count == 1)
		{
			SetDamageFlags(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCollisionFlags && args.Count == 1)
		{
			SetCollisionFlags(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetFireMethodFlags && args.Count == 1)
		{
			SetFireMethodFlags(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCatapultHeight && args.Count == 1)
		{
			SetCatapultHeight(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPenetrateNum && args.Count == 1)
		{
			SetPenetrateNum(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPenetrateOverBack && args.Count == 1)
		{
			SetPenetrateOverBack(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetBackOutGround && args.Count == 1)
		{
			SetBackOutGround(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetBackDuration && args.Count == 1)
		{
			SetBackDuration(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRotateFollowVelocity && args.Count == 1)
		{
			SetRotateFollowVelocity(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRangeOverride && args.Count == 1)
		{
			SetRangeOverride(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRangeType && args.Count == 1)
		{
			SetRangeType(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetUseRange && args.Count == 1)
		{
			SetUseRange(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRangeSize && args.Count == 1)
		{
			SetRangeSize(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetHitPesontage && args.Count == 1)
		{
			SetHitPesontage(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetOverrideHitTargetEvent && args.Count == 1)
		{
			SetOverrideHitTargetEvent(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetOverrideHitCharacterEvent && args.Count == 1)
		{
			SetOverrideHitCharacterEvent(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetOverrideHitGroundEvent && args.Count == 1)
		{
			SetOverrideHitGroundEvent(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildConfig && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileConfig>(BuildConfig());
			return true;
		}
		if (method == MethodName.ApplyOverride && args.Count == 1)
		{
			ApplyOverride(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.InvalidateConfigCache)
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
		if (method == MethodName.SetbaseDamage)
		{
			return true;
		}
		if (method == MethodName.SetSize)
		{
			return true;
		}
		if (method == MethodName.SetScale)
		{
			return true;
		}
		if (method == MethodName.SetHitChestsScale)
		{
			return true;
		}
		if (method == MethodName.SetHitNutScale)
		{
			return true;
		}
		if (method == MethodName.SetHitFrozenScale)
		{
			return true;
		}
		if (method == MethodName.SetHitMachineScale)
		{
			return true;
		}
		if (method == MethodName.SetDamageFlags)
		{
			return true;
		}
		if (method == MethodName.SetCollisionFlags)
		{
			return true;
		}
		if (method == MethodName.SetFireMethodFlags)
		{
			return true;
		}
		if (method == MethodName.SetCatapultHeight)
		{
			return true;
		}
		if (method == MethodName.SetPenetrateNum)
		{
			return true;
		}
		if (method == MethodName.SetPenetrateOverBack)
		{
			return true;
		}
		if (method == MethodName.SetBackOutGround)
		{
			return true;
		}
		if (method == MethodName.SetBackDuration)
		{
			return true;
		}
		if (method == MethodName.SetRotateFollowVelocity)
		{
			return true;
		}
		if (method == MethodName.SetRangeOverride)
		{
			return true;
		}
		if (method == MethodName.SetRangeType)
		{
			return true;
		}
		if (method == MethodName.SetUseRange)
		{
			return true;
		}
		if (method == MethodName.SetRangeSize)
		{
			return true;
		}
		if (method == MethodName.SetHitPesontage)
		{
			return true;
		}
		if (method == MethodName.SetOverrideHitTargetEvent)
		{
			return true;
		}
		if (method == MethodName.SetOverrideHitCharacterEvent)
		{
			return true;
		}
		if (method == MethodName.SetOverrideHitGroundEvent)
		{
			return true;
		}
		if (method == MethodName.BuildConfig)
		{
			return true;
		}
		if (method == MethodName.ApplyOverride)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.skinName)
		{
			skinName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			projectileName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._skinName)
		{
			_skinName = VariantUtils.ConvertTo<StringName>(in value);
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
		if (name == PropertyName.damageFlags)
		{
			damageFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			collisionFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.fireMethodFlags)
		{
			fireMethodFlags = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName.rotateFollowVelocity)
		{
			rotateFollowVelocity = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overrideHitChestsScale)
		{
			overrideHitChestsScale = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overrideHitNutScale)
		{
			overrideHitNutScale = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overrideHitFrozenScale)
		{
			overrideHitFrozenScale = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overrideHitMachineScale)
		{
			overrideHitMachineScale = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overrideCatapultHeight)
		{
			overrideCatapultHeight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overridePenetrateNum)
		{
			overridePenetrateNum = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overridePenetrateOverBack)
		{
			overridePenetrateOverBack = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overrideBackOutGround)
		{
			overrideBackOutGround = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overrideBackDuration)
		{
			overrideBackDuration = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overrideRotateFollowVelocity)
		{
			overrideRotateFollowVelocity = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.rangeOverride)
		{
			rangeOverride = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.overrideHitTargetEvent)
		{
			overrideHitTargetEvent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overrideHitCharacterEvent)
		{
			overrideHitCharacterEvent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.overrideHitGroundEvent)
		{
			overrideHitGroundEvent = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._cachedConfig)
		{
			_cachedConfig = VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in value);
			return true;
		}
		if (name == PropertyName._cachedRegistrationRevision)
		{
			_cachedRegistrationRevision = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.skinName)
		{
			value = VariantUtils.CreateFrom<StringName>(skinName);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom(in projectileName);
			return true;
		}
		if (name == PropertyName._skinName)
		{
			value = VariantUtils.CreateFrom(in _skinName);
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
		if (name == PropertyName.damageFlags)
		{
			value = VariantUtils.CreateFrom(in damageFlags);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			value = VariantUtils.CreateFrom(in collisionFlags);
			return true;
		}
		if (name == PropertyName.fireMethodFlags)
		{
			value = VariantUtils.CreateFrom(in fireMethodFlags);
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
		if (name == PropertyName.rotateFollowVelocity)
		{
			value = VariantUtils.CreateFrom(in rotateFollowVelocity);
			return true;
		}
		if (name == PropertyName.overrideHitChestsScale)
		{
			value = VariantUtils.CreateFrom(in overrideHitChestsScale);
			return true;
		}
		if (name == PropertyName.overrideHitNutScale)
		{
			value = VariantUtils.CreateFrom(in overrideHitNutScale);
			return true;
		}
		if (name == PropertyName.overrideHitFrozenScale)
		{
			value = VariantUtils.CreateFrom(in overrideHitFrozenScale);
			return true;
		}
		if (name == PropertyName.overrideHitMachineScale)
		{
			value = VariantUtils.CreateFrom(in overrideHitMachineScale);
			return true;
		}
		if (name == PropertyName.overrideCatapultHeight)
		{
			value = VariantUtils.CreateFrom(in overrideCatapultHeight);
			return true;
		}
		if (name == PropertyName.overridePenetrateNum)
		{
			value = VariantUtils.CreateFrom(in overridePenetrateNum);
			return true;
		}
		if (name == PropertyName.overridePenetrateOverBack)
		{
			value = VariantUtils.CreateFrom(in overridePenetrateOverBack);
			return true;
		}
		if (name == PropertyName.overrideBackOutGround)
		{
			value = VariantUtils.CreateFrom(in overrideBackOutGround);
			return true;
		}
		if (name == PropertyName.overrideBackDuration)
		{
			value = VariantUtils.CreateFrom(in overrideBackDuration);
			return true;
		}
		if (name == PropertyName.overrideRotateFollowVelocity)
		{
			value = VariantUtils.CreateFrom(in overrideRotateFollowVelocity);
			return true;
		}
		if (name == PropertyName.rangeOverride)
		{
			value = VariantUtils.CreateFrom(in rangeOverride);
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
		if (name == PropertyName.overrideHitTargetEvent)
		{
			value = VariantUtils.CreateFrom(in overrideHitTargetEvent);
			return true;
		}
		if (name == PropertyName.overrideHitCharacterEvent)
		{
			value = VariantUtils.CreateFrom(in overrideHitCharacterEvent);
			return true;
		}
		if (name == PropertyName.overrideHitGroundEvent)
		{
			value = VariantUtils.CreateFrom(in overrideHitGroundEvent);
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
		if (name == PropertyName._cachedConfig)
		{
			value = VariantUtils.CreateFrom(in _cachedConfig);
			return true;
		}
		if (name == PropertyName._cachedRegistrationRevision)
		{
			value = VariantUtils.CreateFrom(in _cachedRegistrationRevision);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.StringName, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName._skinName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.skinName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.size, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.scale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.baseDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitChestsScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitNutScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitFrozenScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitMachineScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.damageFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireMethodFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.catapultHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.penetrateNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.penetrateOverBack, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.backOutGround, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.backDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.rotateFollowVelocity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideHitChestsScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideHitNutScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideHitFrozenScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideHitMachineScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideCatapultHeight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overridePenetrateNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overridePenetrateOverBack, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideBackOutGround, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideBackDuration, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideRotateFollowVelocity, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.rangeOverride, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.rangeType, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.rangeSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitPesontage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideHitTargetEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideHitCharacterEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.overrideHitGroundEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitTargetEventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitCharacterEventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.hitGroundEventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.behaviorIds, PropertyHint.TypeString, "21/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.behaviors, PropertyHint.TypeString, "24/17:ProjectileBehaviorDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._cachedConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedRegistrationRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.skinName, Variant.From<StringName>(skinName));
		info.AddProperty(PropertyName.projectileName, Variant.From(in projectileName));
		info.AddProperty(PropertyName._skinName, Variant.From(in _skinName));
		info.AddProperty(PropertyName.size, Variant.From(in size));
		info.AddProperty(PropertyName.scale, Variant.From(in scale));
		info.AddProperty(PropertyName.baseDamage, Variant.From(in baseDamage));
		info.AddProperty(PropertyName.hitChestsScale, Variant.From(in hitChestsScale));
		info.AddProperty(PropertyName.hitNutScale, Variant.From(in hitNutScale));
		info.AddProperty(PropertyName.hitFrozenScale, Variant.From(in hitFrozenScale));
		info.AddProperty(PropertyName.hitMachineScale, Variant.From(in hitMachineScale));
		info.AddProperty(PropertyName.damageFlags, Variant.From(in damageFlags));
		info.AddProperty(PropertyName.collisionFlags, Variant.From(in collisionFlags));
		info.AddProperty(PropertyName.fireMethodFlags, Variant.From(in fireMethodFlags));
		info.AddProperty(PropertyName.catapultHeight, Variant.From(in catapultHeight));
		info.AddProperty(PropertyName.penetrateNum, Variant.From(in penetrateNum));
		info.AddProperty(PropertyName.penetrateOverBack, Variant.From(in penetrateOverBack));
		info.AddProperty(PropertyName.backOutGround, Variant.From(in backOutGround));
		info.AddProperty(PropertyName.backDuration, Variant.From(in backDuration));
		info.AddProperty(PropertyName.rotateFollowVelocity, Variant.From(in rotateFollowVelocity));
		info.AddProperty(PropertyName.overrideHitChestsScale, Variant.From(in overrideHitChestsScale));
		info.AddProperty(PropertyName.overrideHitNutScale, Variant.From(in overrideHitNutScale));
		info.AddProperty(PropertyName.overrideHitFrozenScale, Variant.From(in overrideHitFrozenScale));
		info.AddProperty(PropertyName.overrideHitMachineScale, Variant.From(in overrideHitMachineScale));
		info.AddProperty(PropertyName.overrideCatapultHeight, Variant.From(in overrideCatapultHeight));
		info.AddProperty(PropertyName.overridePenetrateNum, Variant.From(in overridePenetrateNum));
		info.AddProperty(PropertyName.overridePenetrateOverBack, Variant.From(in overridePenetrateOverBack));
		info.AddProperty(PropertyName.overrideBackOutGround, Variant.From(in overrideBackOutGround));
		info.AddProperty(PropertyName.overrideBackDuration, Variant.From(in overrideBackDuration));
		info.AddProperty(PropertyName.overrideRotateFollowVelocity, Variant.From(in overrideRotateFollowVelocity));
		info.AddProperty(PropertyName.rangeOverride, Variant.From(in rangeOverride));
		info.AddProperty(PropertyName.rangeType, Variant.From(in rangeType));
		info.AddProperty(PropertyName.useRange, Variant.From(in useRange));
		info.AddProperty(PropertyName.rangeSize, Variant.From(in rangeSize));
		info.AddProperty(PropertyName.hitPesontage, Variant.From(in hitPesontage));
		info.AddProperty(PropertyName.overrideHitTargetEvent, Variant.From(in overrideHitTargetEvent));
		info.AddProperty(PropertyName.overrideHitCharacterEvent, Variant.From(in overrideHitCharacterEvent));
		info.AddProperty(PropertyName.overrideHitGroundEvent, Variant.From(in overrideHitGroundEvent));
		info.AddProperty(PropertyName.hitTargetEventList, Variant.CreateFrom(hitTargetEventList));
		info.AddProperty(PropertyName.hitCharacterEventList, Variant.CreateFrom(hitCharacterEventList));
		info.AddProperty(PropertyName.hitGroundEventList, Variant.CreateFrom(hitGroundEventList));
		info.AddProperty(PropertyName.behaviorIds, Variant.CreateFrom(behaviorIds));
		info.AddProperty(PropertyName.behaviors, Variant.CreateFrom(behaviors));
		info.AddProperty(PropertyName._cachedConfig, Variant.From(in _cachedConfig));
		info.AddProperty(PropertyName._cachedRegistrationRevision, Variant.From(in _cachedRegistrationRevision));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.skinName, out var value))
		{
			skinName = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.projectileName, out var value2))
		{
			projectileName = value2.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._skinName, out var value3))
		{
			_skinName = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.size, out var value4))
		{
			size = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.scale, out var value5))
		{
			scale = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.baseDamage, out var value6))
		{
			baseDamage = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitChestsScale, out var value7))
		{
			hitChestsScale = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitNutScale, out var value8))
		{
			hitNutScale = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitFrozenScale, out var value9))
		{
			hitFrozenScale = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitMachineScale, out var value10))
		{
			hitMachineScale = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.damageFlags, out var value11))
		{
			damageFlags = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName.collisionFlags, out var value12))
		{
			collisionFlags = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName.fireMethodFlags, out var value13))
		{
			fireMethodFlags = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName.catapultHeight, out var value14))
		{
			catapultHeight = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.penetrateNum, out var value15))
		{
			penetrateNum = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName.penetrateOverBack, out var value16))
		{
			penetrateOverBack = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.backOutGround, out var value17))
		{
			backOutGround = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.backDuration, out var value18))
		{
			backDuration = value18.As<double>();
		}
		if (info.TryGetProperty(PropertyName.rotateFollowVelocity, out var value19))
		{
			rotateFollowVelocity = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overrideHitChestsScale, out var value20))
		{
			overrideHitChestsScale = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overrideHitNutScale, out var value21))
		{
			overrideHitNutScale = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overrideHitFrozenScale, out var value22))
		{
			overrideHitFrozenScale = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overrideHitMachineScale, out var value23))
		{
			overrideHitMachineScale = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overrideCatapultHeight, out var value24))
		{
			overrideCatapultHeight = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overridePenetrateNum, out var value25))
		{
			overridePenetrateNum = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overridePenetrateOverBack, out var value26))
		{
			overridePenetrateOverBack = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overrideBackOutGround, out var value27))
		{
			overrideBackOutGround = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overrideBackDuration, out var value28))
		{
			overrideBackDuration = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overrideRotateFollowVelocity, out var value29))
		{
			overrideRotateFollowVelocity = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.rangeOverride, out var value30))
		{
			rangeOverride = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.rangeType, out var value31))
		{
			rangeType = value31.As<string>();
		}
		if (info.TryGetProperty(PropertyName.useRange, out var value32))
		{
			useRange = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.rangeSize, out var value33))
		{
			rangeSize = value33.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.hitPesontage, out var value34))
		{
			hitPesontage = value34.As<double>();
		}
		if (info.TryGetProperty(PropertyName.overrideHitTargetEvent, out var value35))
		{
			overrideHitTargetEvent = value35.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overrideHitCharacterEvent, out var value36))
		{
			overrideHitCharacterEvent = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.overrideHitGroundEvent, out var value37))
		{
			overrideHitGroundEvent = value37.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hitTargetEventList, out var value38))
		{
			hitTargetEventList = value38.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.hitCharacterEventList, out var value39))
		{
			hitCharacterEventList = value39.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.hitGroundEventList, out var value40))
		{
			hitGroundEventList = value40.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.behaviorIds, out var value41))
		{
			behaviorIds = value41.AsGodotArray<StringName>();
		}
		if (info.TryGetProperty(PropertyName.behaviors, out var value42))
		{
			behaviors = value42.AsGodotArray<ProjectileBehaviorDefinition>();
		}
		if (info.TryGetProperty(PropertyName._cachedConfig, out var value43))
		{
			_cachedConfig = value43.As<TowerDefenseProjectileConfig>();
		}
		if (info.TryGetProperty(PropertyName._cachedRegistrationRevision, out var value44))
		{
			_cachedRegistrationRevision = value44.As<long>();
		}
	}
}
