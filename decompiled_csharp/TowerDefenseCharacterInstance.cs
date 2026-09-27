using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Instance/TowerDefenseCharacterInstance.cs")]
public class TowerDefenseCharacterInstance : Resource
{
	private readonly struct ArmorLayerResult(double remainingDamage, bool passedToBody)
	{
		public readonly double RemainingDamage = remainingDamage;

		public readonly bool PassedToBody = passedToBody;
	}

	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName IsMagicDamage = "IsMagicDamage";

		public static readonly StringName ProjectileEffectsBlockedByArmor = "ProjectileEffectsBlockedByArmor";

		public static readonly StringName GetProjectileDamageableDurability = "GetProjectileDamageableDurability";

		public static readonly StringName EmitHitpointsNearDie = "EmitHitpointsNearDie";

		public static readonly StringName EmitHitpointsEmpty = "EmitHitpointsEmpty";

		public static readonly StringName ClearHitpointsEmptyListeners = "ClearHitpointsEmptyListeners";

		public static readonly StringName _Init = "_Init";

		public static readonly StringName HurtWithAttackConfig = "HurtWithAttackConfig";

		public static readonly StringName ProjectileHurt = "ProjectileHurt";

		public static readonly StringName IsMeteorProjectile = "IsMeteorProjectile";

		public static readonly StringName FlagHurt = "FlagHurt";

		public static readonly StringName ExplodeHurt = "ExplodeHurt";

		public static readonly StringName ExplodeHurtCore = "ExplodeHurtCore";

		public static readonly StringName HasExplosionShieldInCell = "HasExplosionShieldInCell";

		public static readonly StringName CreateExplosionAshEffect = "CreateExplosionAshEffect";

		public static readonly StringName DeferNetworkExplosionDestroy = "DeferNetworkExplosionDestroy";

		public static readonly StringName SkipInvincibleHurt = "SkipInvincibleHurt";

		public static readonly StringName Hurt = "Hurt";

		public static readonly StringName SmashHurtApply = "SmashHurtApply";

		public static readonly StringName SkipInvincibleDealHurt = "SkipInvincibleDealHurt";

		public static readonly StringName CanSkipStableBodyDamagePostProcessing = "CanSkipStableBodyDamagePostProcessing";

		public static readonly StringName DealHurt = "DealHurt";

		public static readonly StringName Health = "Health";

		public static readonly StringName RefreshArmorRuntimeIndex = "RefreshArmorRuntimeIndex";

		public static readonly StringName AddArmorRuntimeReferences = "AddArmorRuntimeReferences";

		public static readonly StringName RemoveArmorRuntimeReferences = "RemoveArmorRuntimeReferences";

		public static readonly StringName ApplyDealHurtReduce = "ApplyDealHurtReduce";

		public static readonly StringName _ShieldAbsorbDamage = "_ShieldAbsorbDamage";

		public static readonly StringName ArmorHas = "ArmorHas";

		public static readonly StringName HasBurstWheelArmor = "HasBurstWheelArmor";

		public static readonly StringName ArmorAdd = "ArmorAdd";

		public static readonly StringName IsExclusiveWearableHelmet = "IsExclusiveWearableHelmet";

		public static readonly StringName ArmorDelete = "ArmorDelete";

		public static readonly StringName ArmorDamagePointReachHandler = "ArmorDamagePointReachHandler";

		public static readonly StringName ArmorDestroy = "ArmorDestroy";

		public static readonly StringName ArmorClear = "ArmorClear";

		public static readonly StringName ArmorDraw = "ArmorDraw";

		public static readonly StringName RefeshHitPoint = "RefeshHitPoint";

		public static readonly StringName DieMethod = "DieMethod";

		public static readonly StringName SetDamageStage = "SetDamageStage";

		public static readonly StringName RefreshDamagePoint = "RefreshDamagePoint";

		public static readonly StringName ExportSave = "ExportSave";

		public static readonly StringName ImportSave = "ImportSave";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName IsMagicPhysique = "IsMagicPhysique";

		public static readonly StringName maskFlags = "maskFlags";

		public static readonly StringName hitpointScale = "hitpointScale";

		public static readonly StringName ArmorCount = "ArmorCount";

		public static readonly StringName HasAnyArmor = "HasAnyArmor";

		public static readonly StringName HasShieldArmor = "HasShieldArmor";

		public static readonly StringName HasHelmetArmor = "HasHelmetArmor";

		public static readonly StringName HasBodyArmor = "HasBodyArmor";

		public static readonly StringName HasHeadCoverArmor = "HasHeadCoverArmor";

		public static readonly StringName character = "character";

		public static readonly StringName config = "config";

		public static readonly StringName zombiePhysique = "zombiePhysique";

		public static readonly StringName explosionHurt = "explosionHurt";

		public static readonly StringName smashHurt = "smashHurt";

		public static readonly StringName dragHurt = "dragHurt";

		public static readonly StringName spikeHurt = "spikeHurt";

		public static readonly StringName biteHurt = "biteHurt";

		public static readonly StringName explosionHurtSave = "explosionHurtSave";

		public static readonly StringName hitpointsSave = "hitpointsSave";

		public static readonly StringName hitpoints = "hitpoints";

		public static readonly StringName height = "height";

		public static readonly StringName collisionFlags = "collisionFlags";

		public static readonly StringName _maskFlags = "_maskFlags";

		public static readonly StringName unUseBuffFlags = "unUseBuffFlags";

		public static readonly StringName physiqueTypeFlags = "physiqueTypeFlags";

		public static readonly StringName elementFlags = "elementFlags";

		public static readonly StringName hitpointsNearDeath = "hitpointsNearDeath";

		public static readonly StringName hitpointsBase = "hitpointsBase";

		public static readonly StringName impactAudio = "impactAudio";

		public static readonly StringName ashScene = "ashScene";

		public static readonly StringName armorData = "armorData";

		public static readonly StringName customData = "customData";

		public static readonly StringName isChests = "isChests";

		public static readonly StringName dealHurtScale = "dealHurtScale";

		public static readonly StringName dealHurtReduce = "dealHurtReduce";

		public static readonly StringName skipDealHurtReduce = "skipDealHurtReduce";

		public static readonly StringName damagePointData = "damagePointData";

		public static readonly StringName damagePoints = "damagePoints";

		public static readonly StringName damagePointIndex = "damagePointIndex";

		public static readonly StringName _damagePointPercentages = "_damagePointPercentages";

		public static readonly StringName _damagePointNames = "_damagePointNames";

		public static readonly StringName _damagePointAudios = "_damagePointAudios";

		public static readonly StringName _networkExplodeDestroyDeferred = "_networkExplodeDestroyDeferred";

		public static readonly StringName canCollection = "canCollection";

		public static readonly StringName canBeCollection = "canBeCollection";

		public static readonly StringName keepArmor = "keepArmor";

		public static readonly StringName keepAlive = "keepAlive";

		public static readonly StringName damageHitpointsFloor = "damageHitpointsFloor";

		public static readonly StringName die = "die";

		public static readonly StringName nearDie = "nearDie";

		public static readonly StringName invincible = "invincible";

		public static readonly StringName invincibleHurt = "invincibleHurt";

		public static readonly StringName invincibleSmash = "invincibleSmash";

		public static readonly StringName sleep = "sleep";

		public static readonly StringName wakeUp = "wakeUp";

		public static readonly StringName hypnoses = "hypnoses";

		public static readonly StringName hologram = "hologram";

		public static readonly StringName hitpointScaleSave = "hitpointScaleSave";

		public static readonly StringName _hitpointScale = "_hitpointScale";

		public static readonly StringName armorOverrideUnUseBuffFlagSave = "armorOverrideUnUseBuffFlagSave";

		public static readonly StringName armorList = "armorList";

		public static readonly StringName armorShield = "armorShield";

		public static readonly StringName armorHelm = "armorHelm";

		public static readonly StringName armorBody = "armorBody";

		public static readonly StringName armorHeadCover = "armorHeadCover";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	public const double MagicDamageScaleAgainstNonMagic = 1.5;

	public TowerDefenseCharacter character;

	[Export(PropertyHint.None, "")]
	public TowerDefenseCharacterConfig config;

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.ZOMBIE_PHYSIQUE zombiePhysique = TowerDefenseEnum.ZOMBIE_PHYSIQUE.NORMAL;

	[Export(PropertyHint.None, "")]
	public double explosionHurt = -1.0;

	[Export(PropertyHint.None, "")]
	public double smashHurt = -1.0;

	[Export(PropertyHint.None, "")]
	public double dragHurt = -1.0;

	[Export(PropertyHint.None, "")]
	public double spikeHurt = -1.0;

	[Export(PropertyHint.None, "")]
	public double biteHurt = -1.0;

	public double explosionHurtSave = -1.0;

	[Export(PropertyHint.None, "")]
	public double hitpointsSave = 200.0;

	[Export(PropertyHint.None, "")]
	public double hitpoints = 200.0;

	[Export(PropertyHint.None, "")]
	public TowerDefenseEnum.CHARACTER_HEIGHT height = TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL;

	[Export(PropertyHint.None, "")]
	public int collisionFlags = 1;

	private int _maskFlags = 1;

	[Export(PropertyHint.None, "")]
	public int unUseBuffFlags;

	[Export(PropertyHint.None, "")]
	public int physiqueTypeFlags;

	[Export(PropertyHint.None, "")]
	public int elementFlags;

	[Export(PropertyHint.None, "")]
	public double hitpointsNearDeath;

	[Export(PropertyHint.None, "")]
	public double hitpointsBase = 300.0;

	[Export(PropertyHint.None, "")]
	public string impactAudio = "";

	[Export(PropertyHint.None, "")]
	public PackedScene ashScene;

	[Export(PropertyHint.None, "")]
	public CharacterArmorData armorData;

	[Export(PropertyHint.None, "")]
	public CharacterCustomData customData;

	[Export(PropertyHint.None, "")]
	public bool isChests;

	[Export(PropertyHint.None, "")]
	public double dealHurtScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double dealHurtReduce;

	public bool skipDealHurtReduce;

	[Export(PropertyHint.None, "")]
	public CharacterDamagePointData damagePointData;

	[Export(PropertyHint.None, "")]
	public Array<Dictionary> damagePoints = new Array<Dictionary>();

	[Export(PropertyHint.None, "")]
	public int damagePointIndex;

	private double[] _damagePointPercentages = System.Array.Empty<double>();

	private string[] _damagePointNames = System.Array.Empty<string>();

	private string[] _damagePointAudios = System.Array.Empty<string>();

	private bool _networkExplodeDestroyDeferred;

	[Export(PropertyHint.None, "")]
	public bool canCollection = true;

	[Export(PropertyHint.None, "")]
	public bool canBeCollection = true;

	[Export(PropertyHint.None, "")]
	public bool keepArmor;

	[Export(PropertyHint.None, "")]
	public bool keepAlive;

	public double damageHitpointsFloor;

	[Export(PropertyHint.None, "")]
	public bool die;

	[Export(PropertyHint.None, "")]
	public bool nearDie;

	[Export(PropertyHint.None, "")]
	public bool invincible;

	[Export(PropertyHint.None, "")]
	public bool invincibleHurt;

	[Export(PropertyHint.None, "")]
	public bool invincibleSmash;

	[Export(PropertyHint.None, "")]
	public bool sleep;

	[Export(PropertyHint.None, "")]
	public bool wakeUp;

	[Export(PropertyHint.None, "")]
	public bool hypnoses;

	[Export(PropertyHint.None, "")]
	public bool hologram;

	[Export(PropertyHint.None, "")]
	public double hitpointScaleSave = 1.0;

	private double _hitpointScale = 1.0;

	[Export(PropertyHint.None, "")]
	public int armorOverrideUnUseBuffFlagSave;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseArmorInstance> armorList = new Array<TowerDefenseArmorInstance>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseArmorInstance> armorShield = new Array<TowerDefenseArmorInstance>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseArmorInstance> armorHelm = new Array<TowerDefenseArmorInstance>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseArmorInstance> armorBody = new Array<TowerDefenseArmorInstance>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseArmorInstance> armorHeadCover = new Array<TowerDefenseArmorInstance>();

	private List<TowerDefenseArmorInstance> _armorRuntime;

	private List<TowerDefenseArmorInstance> _armorShieldRuntime;

	private List<TowerDefenseArmorInstance> _armorHelmRuntime;

	private List<TowerDefenseArmorInstance> _armorBodyRuntime;

	private List<TowerDefenseArmorInstance> _armorHeadCoverRuntime;

	public bool IsMagicPhysique => (physiqueTypeFlags & 0x1000) != 0;

	[Export(PropertyHint.None, "")]
	public int maskFlags
	{
		get
		{
			return _maskFlags;
		}
		set
		{
			if (_maskFlags != value)
			{
				_maskFlags = value;
				if (GodotObject.IsInstanceValid(character))
				{
					character.targetRegistrationComponent?.NotifyTargetStateChanged();
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public double hitpointScale
	{
		get
		{
			return _hitpointScale;
		}
		set
		{
			if (_hitpointScale == value)
			{
				return;
			}
			_hitpointScale = value;
			hitpointsSave *= value / hitpointScaleSave;
			hitpoints *= value / hitpointScaleSave;
			foreach (TowerDefenseArmorInstance armor in armorList)
			{
				if (!armor.isRemove)
				{
					armor.hitpointScale = _hitpointScale;
				}
			}
			hitpointScaleSave = _hitpointScale;
			TowerDefenseCharacter towerDefenseCharacter = character;
			if (towerDefenseCharacter != null && towerDefenseCharacter.showHealthComponent?.IsReleased == false)
			{
				character.showHealthComponent.MarkDirty();
			}
		}
	}

	internal int ArmorCount => _armorRuntime?.Count ?? 0;

	internal bool HasAnyArmor => ArmorCount != 0;

	internal bool HasShieldArmor => (_armorShieldRuntime?.Count ?? 0) != 0;

	internal bool HasHelmetArmor => (_armorHelmRuntime?.Count ?? 0) != 0;

	internal bool HasBodyArmor => (_armorBodyRuntime?.Count ?? 0) != 0;

	internal bool HasHeadCoverArmor => (_armorHeadCoverRuntime?.Count ?? 0) != 0;

	public event Action<string> damagePointReach;

	public event Action<string, int> armorDamagePointReach;

	public event Action hitpointsNearDie;

	public event Action hitpointsEmpty;

	public event Action<string> armorHitpointsEmpty;

	public static bool IsMagicDamage(int damageFlags)
	{
		return (damageFlags & 0x40) != 0;
	}

	internal bool ProjectileEffectsBlockedByArmor(int damageFlags, int fireMethodFlags)
	{
		if ((damageFlags & 0x10) != 0)
		{
			return false;
		}
		if ((fireMethodFlags & 0x40) == 0 && (damageFlags & 1) != 0 && HasBlockingProjectileArmor(_armorShieldRuntime))
		{
			return true;
		}
		if ((damageFlags & 8) != 0 && HasBlockingProjectileArmor(_armorHeadCoverRuntime))
		{
			return true;
		}
		if ((damageFlags & 2) != 0 && (HasBlockingProjectileArmor(_armorHelmRuntime) || HasBlockingProjectileArmor(_armorBodyRuntime)))
		{
			return true;
		}
		return false;
	}

	private static bool HasBlockingProjectileArmor(List<TowerDefenseArmorInstance> armors)
	{
		int num = armors?.Count ?? 0;
		for (int i = 0; i < num; i++)
		{
			TowerDefenseArmorInstance towerDefenseArmorInstance = armors[i];
			if (towerDefenseArmorInstance != null && !towerDefenseArmorInstance.isRemove && (towerDefenseArmorInstance.armorMethodFlags & 0x2000) != 0)
			{
				return true;
			}
		}
		return false;
	}

	internal double GetProjectileDamageableDurability(int damageFlags)
	{
		double num = 0.0;
		if ((damageFlags & 8) != 0)
		{
			num += SumActiveArmorDurability(_armorHeadCoverRuntime);
		}
		if ((damageFlags & 1) != 0)
		{
			num += SumActiveArmorDurability(_armorShieldRuntime);
		}
		if ((damageFlags & 2) != 0)
		{
			num += SumActiveArmorDurability(_armorHelmRuntime);
			num += SumActiveArmorDurability(_armorBodyRuntime);
			num += Math.Max(0.0, hitpoints);
		}
		return num;
	}

	private static double SumActiveArmorDurability(List<TowerDefenseArmorInstance> armors)
	{
		double num = 0.0;
		int num2 = armors?.Count ?? 0;
		for (int i = 0; i < num2; i++)
		{
			TowerDefenseArmorInstance towerDefenseArmorInstance = armors[i];
			if (towerDefenseArmorInstance != null && !towerDefenseArmorInstance.isRemove)
			{
				num += Math.Max(0.0, towerDefenseArmorInstance.hitPoints);
			}
		}
		return num;
	}

	public void EmitHitpointsNearDie()
	{
		hitpointsNearDie?.Invoke();
	}

	public void EmitHitpointsEmpty()
	{
		hitpointsEmpty?.Invoke();
	}

	public void ClearHitpointsEmptyListeners()
	{
		hitpointsEmpty = null;
	}

	public void _Init(TowerDefenseCharacter _character, TowerDefenseCharacterConfig _config)
	{
		character = _character;
		config = _config;
		RefreshArmorRuntimeIndex();
		explosionHurt = config.explosionHurt;
		explosionHurtSave = config.explosionHurt;
		smashHurt = config.smashHurt;
		dragHurt = config.dragHurt;
		spikeHurt = config.spikeHurt;
		biteHurt = config.biteHurt;
		hitpointsNearDeath = config.hitpointsNearDeath;
		hitpointsBase = config.hitpoints;
		hitpoints = hitpointsBase + hitpointsNearDeath;
		hitpointsSave = hitpoints;
		height = config.height;
		ashScene = config.ashScene;
		armorData = config.armorData;
		customData = config.customData;
		collisionFlags = config.collisionFlags;
		maskFlags = config.maskFlags;
		unUseBuffFlags = config.unUseBuffFlags;
		physiqueTypeFlags = config.physiqueTypeFlags;
		elementFlags = config.elementFlags;
		damagePointData = config.damagePointData;
		_damagePointPercentages = System.Array.Empty<double>();
		_damagePointNames = System.Array.Empty<string>();
		_damagePointAudios = System.Array.Empty<string>();
		if (damagePointData != null)
		{
			Array<CharacterDamagePointConfig> damagePointList = damagePointData.damagePointList;
			int count = damagePointList.Count;
			_damagePointPercentages = new double[count];
			_damagePointNames = new string[count];
			_damagePointAudios = new string[count];
			for (int i = 0; i < count; i++)
			{
				CharacterDamagePointConfig characterDamagePointConfig = damagePointList[i];
				_damagePointPercentages[i] = characterDamagePointConfig.damagePersontage;
				_damagePointNames[i] = characterDamagePointConfig.damagePointName;
				_damagePointAudios[i] = characterDamagePointConfig.damageAudio;
				Dictionary item = new Dictionary
				{
					["Persontage"] = characterDamagePointConfig.damagePersontage,
					["Name"] = characterDamagePointConfig.damagePointName,
					["DamageAudio"] = characterDamagePointConfig.damageAudio
				};
				damagePoints.Add(item);
			}
		}
		if (_config is TowerDefenseZombieConfig towerDefenseZombieConfig)
		{
			zombiePhysique = towerDefenseZombieConfig.physique;
			impactAudio = towerDefenseZombieConfig.impactAudio;
		}
		if (_config is TowerDefenseGravestoneConfig towerDefenseGravestoneConfig)
		{
			isChests = towerDefenseGravestoneConfig.isChests;
		}
		foreach (string item2 in character.currentArmor)
		{
			ArmorAdd(item2);
		}
	}

	private ArmorLayerResult ProcessArmorLayer(List<TowerDefenseArmorInstance> armorList, double num, bool playSplatAudio, Vector2 velocity, bool createDamagePart, bool isRange, bool skipBodyInvincible, int projectileHeight = -1)
	{
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		int num2 = 0;
		bool passedToBody = false;
		int num3 = 0;
		while (num3 < armorList.Count)
		{
			TowerDefenseArmorInstance towerDefenseArmorInstance = armorList[num3];
			num2++;
			if (towerDefenseArmorInstance == null || towerDefenseArmorInstance.isRemove)
			{
				num3++;
				continue;
			}
			if (projectileHeight >= 0 && projectileHeight > (int)towerDefenseArmorInstance.typeData.height)
			{
				num3++;
				continue;
			}
			if ((towerDefenseArmorInstance.armorMethodFlags & 0x20) != 0)
			{
				num = 0.0;
			}
			if ((towerDefenseArmorInstance.armorMethodFlags & 0x100) != 0)
			{
				if (skipBodyInvincible)
				{
					SkipInvincibleDealHurt(num, playSplatAudio, velocity, createDamagePart);
				}
				else
				{
					DealHurt(num, playSplatAudio, velocity, createDamagePart);
				}
				passedToBody = true;
			}
			if (isRange)
			{
				towerDefenseArmorInstance.Hurt(num, playSplatAudio, velocity, createDamagePart);
			}
			else
			{
				num = towerDefenseArmorInstance.Hurt(num, playSplatAudio, velocity, createDamagePart);
			}
			if (num3 < armorList.Count && armorList[num3] == towerDefenseArmorInstance)
			{
				num3++;
			}
			if (num <= 0.0)
			{
				break;
			}
		}
		TowerDefensePerfProfiler.End("damage.armor", startTicks, num2);
		return new ArmorLayerResult(num, passedToBody);
	}

	public double HurtWithAttackConfig(AttackConfig attackConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true)
	{
		if (HasAnyArmor)
		{
			return FlagHurt(attackConfig.num * attackConfig.armorAttackScale, attackConfig.damageFlags, playSplatAudio, velocity, createDamagePart);
		}
		return FlagHurt(attackConfig.num * attackConfig.attackScale, attackConfig.damageFlags, playSplatAudio, velocity, createDamagePart);
	}

	public double ProjectileHurt(TowerDefenseProjectile projectile, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false, bool createDamagePart = true)
	{
		ProjectileHitInfo info = new ProjectileHitInfo
		{
			damage = projectileConfig.baseDamage,
			damageFlags = projectileConfig.damageFlags,
			position = default,
			projectileHeight = TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL,
			config = projectileConfig,
			fireCharacter = null,
			camp = TowerDefenseEnum.CHARACTER_CAMP.NOONE,
			collisionFlags = 0,
			gridPos = Vector2I.Zero,
			height = 0.0,
			onSourceDespawn = null
		};
		if (GodotObject.IsInstanceValid(projectile))
		{
			info.damage = projectile.damage;
			info.damageFlags = projectile.damageFlags;
			info.useRuntimeOverrides = true;
			info.fireMethodFlags = projectile.fireMethodFlags;
			info.position = projectile.GlobalPosition;
			info.projectileHeight = projectile.projectileHeight;
			info.onSourceDespawn = projectile.Over;
		}
		return ProjectileHurt(in info, projectileConfig, playSplatAudio, velocity, isRange, createDamagePart);
	}

	private static bool IsMeteorProjectile(TowerDefenseProjectileConfig projectileConfig)
	{
		bool flag = projectileConfig != null;
		if (flag)
		{
			string name = projectileConfig.name;
			bool flag2 = ((name == "MeteorStar" || name == "MeteorStarS") ? true : false);
			flag = flag2;
		}
		return flag;
	}

	private bool TryApplyStableProjectileBodyHit(in ProjectileHitInfo info, TowerDefenseProjectileConfig projectileConfig, double num, int damageFlags, int fireMethodFlags, bool hasSourceBullet, bool playSplatAudio, Vector2 velocity, bool isRange, bool createDamagePart, out double result)
	{
		result = 0.0;
		if ((!hasSourceBullet | isRange) || !projectileConfig.UsesDefaultDamage || (damageFlags & -65) != 2 || HasHelmetArmor || HasBodyArmor || invincible)
		{
			return false;
		}
		bool flag = (fireMethodFlags & 4) != 0;
		bool flag2 = (fireMethodFlags & 0x40) != 0;
		bool num2 = projectileConfig.useRange || (flag && !flag2);
		bool flag3 = character is TowerDefensePlant;
		bool hasIncomingDamageModifiers = character.buff.HasIncomingDamageModifiers;
		if (num2 && (HasShieldArmor | flag3 | hasIncomingDamageModifiers))
		{
			return false;
		}
		if (flag)
		{
			List<TowerDefenseArmorInstance> armorRuntime = _armorRuntime;
			int num3 = armorRuntime?.Count ?? 0;
			for (int i = 0; i < num3; i++)
			{
				TowerDefenseArmorInstance towerDefenseArmorInstance = armorRuntime[i];
				if (!towerDefenseArmorInstance.isRemove && (towerDefenseArmorInstance.armorMethodFlags & 0x400) != 0)
				{
					return false;
				}
			}
			if ((physiqueTypeFlags & 0x400) != 0)
			{
				return false;
			}
		}
		if ((fireMethodFlags & 1) != 0)
		{
			float num4 = (info.hasTargetTransform ? info.targetOriginX : character.GetLogicalGlobalPosition().X);
			float num5 = (info.hasTargetTransform ? info.targetScaleX : character.Scale.X);
			float num6 = info.position.X - num4;
			if ((num6 > 30f && num5 > 0f) || (num6 < -30f && num5 < 0f))
			{
				return false;
			}
		}
		if (hasIncomingDamageModifiers)
		{
			num = character.buff.SetAttackNum(num);
		}
		num *= dealHurtScale;
		num = ApplyDealHurtReduce(num);
		if (flag3)
		{
			num = _ShieldAbsorbDamage(num);
		}
		ref double reference = ref result;
		double num7;
		if (num <= 0.0)
		{
			num7 = 0.0;
		}
		else
		{
			num7 = ((hasIncomingDamageModifiers | flag3) ? DealHurt(num, playSplatAudio, velocity, createDamagePart) : SkipInvincibleDealHurt(num, playSplatAudio, velocity, createDamagePart));
		}
		reference = num7;
		return true;
	}

	public double ProjectileHurt(in ProjectileHitInfo info, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false, bool createDamagePart = true)
	{
		if (die)
		{
			return 1000000.0;
		}
		double num = projectileConfig.baseDamage;
		int num2 = projectileConfig.damageFlags;
		bool flag = info.onSourceDespawn != null;
		if (flag || info.useRuntimeOverrides)
		{
			num = info.damage;
			num2 = info.damageFlags;
		}
		int num3 = (info.useRuntimeOverrides ? info.fireMethodFlags : projectileConfig.fireMethodFlags);
		bool flag2 = false;
		if (isChests)
		{
			num *= projectileConfig.hitChestsScale;
		}
		if ((physiqueTypeFlags & 1) != 0)
		{
			num *= projectileConfig.hitNutScale;
		}
		if (character.buff.HasActiveBuffs && character.buff.BuffHas("Frozen"))
		{
			num *= projectileConfig.hitFrozenScale;
		}
		if ((physiqueTypeFlags & 0x800) != 0)
		{
			num *= projectileConfig.hitMachineScale;
		}
		if (IsMagicDamage(num2) && !IsMagicPhysique)
		{
			num *= 1.5;
		}
		if (isRange)
		{
			num *= projectileConfig.hitPesontage;
		}
		else if (projectileConfig.useRange && (num2 & 1) == 0)
		{
			flag2 = true;
		}
		if (TryApplyStableProjectileBodyHit(in info, projectileConfig, num, num2, num3, flag, playSplatAudio, velocity, isRange, createDamagePart, out var result))
		{
			return result;
		}
		bool flag3 = (num3 & 4) != 0;
		bool flag4 = (num3 & 0x40) != 0;
		if (flag3)
		{
			flag2 = !flag4;
			if (flag)
			{
				List<TowerDefenseArmorInstance> armorRuntime = _armorRuntime;
				int num4 = armorRuntime?.Count ?? 0;
				for (int i = 0; i < num4; i++)
				{
					TowerDefenseArmorInstance towerDefenseArmorInstance = armorRuntime[i];
					if (!towerDefenseArmorInstance.isRemove && (towerDefenseArmorInstance.armorMethodFlags & 0x400) != 0)
					{
						info.onSourceDespawn?.Invoke();
						if ((num2 & 1) == 0)
						{
							num2 |= 1;
							flag2 = false;
						}
					}
				}
				if ((physiqueTypeFlags & 0x400) != 0)
				{
					info.onSourceDespawn?.Invoke();
					if ((num2 & 1) == 0)
					{
						num2 |= 1;
						flag2 = false;
					}
				}
			}
		}
		if (flag2 && (invincible || HasShieldArmor || character is TowerDefensePlant || character.buff.HasIncomingDamageModifiers))
		{
			FlagHurt(num, 1, playSplatAudio, velocity, createDamagePart, isRange);
		}
		if (flag && (num3 & 1) != 0)
		{
			float num5 = (info.hasTargetTransform ? info.targetOriginX : character.GetLogicalGlobalPosition().X);
			float num6 = (info.hasTargetTransform ? info.targetScaleX : character.Scale.X);
			float num7 = info.position.X - num5;
			if ((num7 > 30f && num6 > 0f) || (num7 < -30f && num6 < 0f))
			{
				num2 ^= 1;
			}
		}
		if (projectileConfig.UsesExplosionDamage)
		{
			if (!isRange && IsMeteorProjectile(projectileConfig))
			{
				if (flag)
				{
					return FlagHurt(num, num2, playSplatAudio, velocity, createDamagePart, isRange, (int)info.projectileHeight);
				}
				return FlagHurt(num, num2, playSplatAudio, velocity, createDamagePart, isRange);
			}
			return ExplodeHurtCore(num, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, playSplatAudio, velocity);
		}
		if (projectileConfig.UsesDefaultDamage & flag)
		{
			return FlagHurt(num, num2, playSplatAudio, velocity, createDamagePart, isRange, (int)info.projectileHeight);
		}
		return FlagHurt(num, num2, playSplatAudio, velocity, createDamagePart, isRange);
	}

	public double FlagHurt(double num, int damageFlags, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true, bool isRange = false, int projectileHeight = 2)
	{
		damageFlags &= -65;
		if (invincible)
		{
			character.EmitBodyHurt(0);
			return 0.0;
		}
		if (die)
		{
			return 1000000.0;
		}
		num = character.buff.SetAttackNum(num);
		num *= dealHurtScale;
		num = ApplyDealHurtReduce(num);
		num = _ShieldAbsorbDamage(num);
		if (num <= 0.0)
		{
			return 0.0;
		}
		if ((damageFlags & 0x10) != 0)
		{
			return DealHurt(num, playSplatAudio, velocity, createDamagePart);
		}
		if (!isRange && damageFlags == 2 && !HasHelmetArmor && !HasBodyArmor)
		{
			return DealHurt(num, playSplatAudio, velocity, createDamagePart);
		}
		bool flag = false;
		if ((damageFlags & 4) != 0 && character.buff.BuffHas("RedHeat"))
		{
			num *= 2.0;
		}
		if (num > 0.0 && (isRange || (damageFlags & 8) != 0) && HasHeadCoverArmor)
		{
			ArmorLayerResult armorLayerResult = ProcessArmorLayer(_armorHeadCoverRuntime, num, playSplatAudio, velocity, createDamagePart, isRange, skipBodyInvincible: false, projectileHeight);
			num = armorLayerResult.RemainingDamage;
			flag = flag || armorLayerResult.PassedToBody;
		}
		if (num > 0.0 && (isRange || (damageFlags & 1) != 0) && HasShieldArmor)
		{
			ArmorLayerResult armorLayerResult2 = ProcessArmorLayer(_armorShieldRuntime, num, playSplatAudio, velocity, createDamagePart, isRange, skipBodyInvincible: false, projectileHeight);
			num = armorLayerResult2.RemainingDamage;
			flag = flag || armorLayerResult2.PassedToBody;
		}
		if (damageFlags == 1)
		{
			return 0.0;
		}
		if (num > 0.0 && (damageFlags & 2) != 0)
		{
			if ((damageFlags & 4) != 0)
			{
				character.buff.ApplyFireHit();
			}
			if (HasHelmetArmor)
			{
				ArmorLayerResult armorLayerResult3 = ProcessArmorLayer(_armorHelmRuntime, num, playSplatAudio, velocity, createDamagePart, isRange: false, skipBodyInvincible: false);
				num = armorLayerResult3.RemainingDamage;
				flag = flag || armorLayerResult3.PassedToBody;
			}
			if (HasBodyArmor)
			{
				ArmorLayerResult armorLayerResult4 = ProcessArmorLayer(_armorBodyRuntime, num, playSplatAudio, velocity, createDamagePart, isRange: false, skipBodyInvincible: false);
				num = armorLayerResult4.RemainingDamage;
				flag = flag || armorLayerResult4.PassedToBody;
			}
		}
		if (num > 0.0 && !flag)
		{
			num = DealHurt(num, playSplatAudio, velocity, createDamagePart);
		}
		return num;
	}

	public double ExplodeHurt(double num, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND damageKind = TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		return ExplodeHurtCore(num, damageKind, playSplatAudio, velocity);
	}

	private double ExplodeHurtCore(double num, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND damageKind, bool playSplatAudio, Vector2 velocity)
	{
		if (invincible)
		{
			character.EmitBodyHurt(0);
			return 0.0;
		}
		num *= dealHurtScale;
		num = ApplyDealHurtReduce(num);
		bool flag = character is TowerDefensePlant;
		if (flag && num >= 1800.0)
		{
			num = 10000000.0;
		}
		if (flag && num > 0.0 && GodotObject.IsInstanceValid(character.cell) && HasExplosionShieldInCell(character))
		{
			character.EmitBodyHurt(0);
			return 0.0;
		}
		if (flag && GodotObject.IsInstanceValid(character.cell))
		{
			TowerDefenseCharacter slot = character.cell.GetSlot(character);
			if (GodotObject.IsInstanceValid(slot) && GodotObject.IsInstanceValid(slot.instance))
			{
				double num2 = ((explosionHurt < 0.0) ? num : Mathf.Min(explosionHurt, num));
				if (slot.instance.hitpoints - num2 > 0.0 && hitpoints - num2 <= 0.0)
				{
					return num2;
				}
			}
		}
		num = _ShieldAbsorbDamage(num);
		if (num <= 0.0)
		{
			return 0.0;
		}
		if (explosionHurt >= 0.0)
		{
			num = Mathf.Min(explosionHurt, num);
		}
		if (damageKind == TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.JALA)
		{
			character.buff.ApplyFireHit();
			if (character.buff.BuffHas("RedHeat"))
			{
				num *= 2.0;
			}
		}
		if (HasHeadCoverArmor)
		{
			num = ProcessExplosionArmorLayer(_armorHeadCoverRuntime, num, playSplatAudio, velocity, consumeOverflow: false);
		}
		if (num > 0.0 && HasShieldArmor)
		{
			num = ProcessExplosionArmorLayer(_armorShieldRuntime, num, playSplatAudio, velocity, consumeOverflow: false);
		}
		if (num > 0.0 && HasHelmetArmor)
		{
			num = ProcessExplosionArmorLayer(_armorHelmRuntime, num, playSplatAudio, velocity, consumeOverflow: true);
		}
		if (num > 0.0 && HasBodyArmor)
		{
			num = ProcessExplosionArmorLayer(_armorBodyRuntime, num, playSplatAudio, velocity, consumeOverflow: true);
		}
		bool createDamagePart = damageKind == TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.OTHER;
		bool flag2 = false;
		if (num != 0.0)
		{
			bool num3 = hitpoints > 0.0 && !die;
			num = DealHurt(num, playSplatAudio, velocity, createDamagePart);
			flag2 = num3 && die && hitpoints <= 0.0;
		}
		if (num > 0.0)
		{
			EmitHitpointsNearDie();
			nearDie = true;
			if (!((Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) & flag))
			{
				DieMethod();
			}
		}
		if (((num > 0.0) | flag2) && (!(character is TowerDefenseZombie) || zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS) && damageKind != TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.OTHER)
		{
			CreateExplosionAshEffect();
			DeferNetworkExplosionDestroy(damageKind);
		}
		return num;
	}

	private bool HasExplosionShieldInCell(TowerDefenseCharacter target)
	{
		if (target.cell == null)
		{
			return false;
		}
		List<TowerDefenseCharacter> characterList = target.cell.GetCharacterList();
		for (int i = 0; i < characterList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = characterList[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && !towerDefenseCharacter.isDestroy && towerDefenseCharacter.config != null && towerDefenseCharacter.config.name == "PlantPumpkinRobot" && towerDefenseCharacter.camp == target.camp)
			{
				return true;
			}
		}
		return false;
	}

	private void CreateExplosionAshEffect()
	{
		if (!character.inWater && ashScene != null)
		{
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(ashScene, character.gridPos, "Idle");
			Node2D characterNode = TowerDefenseManager.GetCharacterNode();
			towerDefenseEffectSpriteOnce.GlobalPosition = character.GetLogicalGlobalPosition(character.sprite);
			towerDefenseEffectSpriteOnce.Scale = character.Scale * character.transformPoint.Scale;
			characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, Node.InternalMode.Disabled);
			towerDefenseEffectSpriteOnce.ZIndex -= 6;
		}
	}

	private void DeferNetworkExplosionDestroy(TowerDefenseEnum.EXPLOSION_DAMAGE_KIND damageKind)
	{
		if (!Global.IsMultiplayerMode || !MultiPlayerManager.IsHost || character.syncId < 0 || damageKind == TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.OTHER)
		{
			character.Destroy();
		}
		else
		{
			if (_networkExplodeDestroyDeferred)
			{
				return;
			}
			_networkExplodeDestroyDeferred = true;
			TowerDefenseCharacter target = character;
			bool explosionFlag = target.isExplode;
			Callable.From(() =>
			{
				_networkExplodeDestroyDeferred = false;
				if (!GodotObject.IsInstanceValid(target) || target.isDestroy)
				{
					return;
				}
				bool isExplode = target.isExplode;
				target.isExplode = explosionFlag;
				try
				{
					target.Destroy();
				}
				finally
				{
					if (GodotObject.IsInstanceValid(target))
					{
						target.isExplode = isExplode;
					}
				}
			}).CallDeferred();
		}
	}

	private static double ProcessExplosionArmorLayer(List<TowerDefenseArmorInstance> armorList, double num, bool playSplatAudio, Vector2 velocity, bool consumeOverflow)
	{
		int num2 = 0;
		while (num2 < armorList.Count)
		{
			TowerDefenseArmorInstance towerDefenseArmorInstance = armorList[num2];
			if (towerDefenseArmorInstance == null || towerDefenseArmorInstance.isRemove)
			{
				num2++;
				continue;
			}
			double explodePersontage = towerDefenseArmorInstance.typeData.explodePersontage;
			double num3 = towerDefenseArmorInstance.DealHurt(num * explodePersontage, playSplatAudio, velocity);
			if (explodePersontage == 0.0)
			{
				return 0.0;
			}
			if (consumeOverflow)
			{
				num = num3;
			}
			if (num2 < armorList.Count && armorList[num2] == towerDefenseArmorInstance)
			{
				num2++;
			}
			if (num <= 0.0)
			{
				break;
			}
		}
		return num;
	}

	public double SkipInvincibleHurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool hitShield = true, bool createDamagePart = true)
	{
		if (die)
		{
			return 1000000.0;
		}
		num *= dealHurtScale;
		num = ApplyDealHurtReduce(num);
		num = _ShieldAbsorbDamage(num);
		if (num <= 0.0)
		{
			return 0.0;
		}
		bool flag = false;
		if (num > 0.0)
		{
			if (hitShield && HasShieldArmor)
			{
				ArmorLayerResult armorLayerResult = ProcessArmorLayer(_armorShieldRuntime, num, playSplatAudio, velocity, createDamagePart, isRange: false, skipBodyInvincible: true);
				num = armorLayerResult.RemainingDamage;
				flag = flag || armorLayerResult.PassedToBody;
			}
			if (HasHelmetArmor)
			{
				ArmorLayerResult armorLayerResult2 = ProcessArmorLayer(_armorHelmRuntime, num, playSplatAudio, velocity, createDamagePart, isRange: false, skipBodyInvincible: true);
				num = armorLayerResult2.RemainingDamage;
				flag = flag || armorLayerResult2.PassedToBody;
			}
			if (HasBodyArmor)
			{
				ArmorLayerResult armorLayerResult3 = ProcessArmorLayer(_armorBodyRuntime, num, playSplatAudio, velocity, createDamagePart, isRange: false, skipBodyInvincible: true);
				num = armorLayerResult3.RemainingDamage;
				flag = flag || armorLayerResult3.PassedToBody;
			}
			if (num > 0.0 && !flag)
			{
				num = SkipInvincibleDealHurt(num, playSplatAudio, velocity, createDamagePart);
			}
		}
		return num;
	}

	public double Hurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool hitShield = true, bool createDamagePart = true, double damageLimit = -1.0)
	{
		if (invincible)
		{
			character.EmitBodyHurt(0);
			return 0.0;
		}
		if (invincibleHurt)
		{
			return 0.0;
		}
		if (die)
		{
			return 1000000.0;
		}
		num *= dealHurtScale;
		num = ApplyDealHurtReduce(num);
		num = _ShieldAbsorbDamage(num);
		if (num <= 0.0)
		{
			return 0.0;
		}
		if (damageLimit >= 0.0)
		{
			num = Mathf.Min(num, ApplyDealHurtReduce(damageLimit * dealHurtScale));
		}
		bool flag = false;
		if (num > 0.0)
		{
			if (hitShield && HasShieldArmor)
			{
				ArmorLayerResult armorLayerResult = ProcessArmorLayer(_armorShieldRuntime, num, playSplatAudio, velocity, createDamagePart, isRange: false, skipBodyInvincible: false);
				num = armorLayerResult.RemainingDamage;
				flag = flag || armorLayerResult.PassedToBody;
			}
			if (HasHelmetArmor)
			{
				ArmorLayerResult armorLayerResult2 = ProcessArmorLayer(_armorHelmRuntime, num, playSplatAudio, velocity, createDamagePart, isRange: false, skipBodyInvincible: false);
				num = armorLayerResult2.RemainingDamage;
				flag = flag || armorLayerResult2.PassedToBody;
			}
			if (HasBodyArmor)
			{
				ArmorLayerResult armorLayerResult3 = ProcessArmorLayer(_armorBodyRuntime, num, playSplatAudio, velocity, createDamagePart, isRange: false, skipBodyInvincible: false);
				num = armorLayerResult3.RemainingDamage;
				flag = flag || armorLayerResult3.PassedToBody;
			}
			if (num > 0.0 && !flag)
			{
				num = DealHurt(num, playSplatAudio, velocity, createDamagePart);
			}
		}
		return num;
	}

	public double SmashHurtApply(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		if (invincible)
		{
			character.EmitBodyHurt(0);
			return 0.0;
		}
		if (invincibleSmash)
		{
			ComponentManager componentManager = character.componentManager;
			if (componentManager != null && componentManager.GetRuntime<PotatoComponent>()?.TryExplodeOnSmash() == true && GodotObject.IsInstanceValid(character.cell))
			{
				character.cell.smashAbsorbedFrame = Engine.GetPhysicsFrames();
			}
			return 0.0;
		}
		if (character is TowerDefensePlant)
		{
			double num2 = 100000.0;
			if (smashHurt != -1.0)
			{
				num2 = Mathf.Min(smashHurt, num2);
			}
			if (GodotObject.IsInstanceValid(character.cell))
			{
				TowerDefenseCharacter slot = character.cell.GetSlot(character);
				if (GodotObject.IsInstanceValid(slot) && slot.instance.hitpoints - num2 > 0.0 && character.instance.hitpoints - num2 <= 0.0)
				{
					return num;
				}
			}
		}
		character.isSmash = true;
		if (character is TowerDefensePlant)
		{
			num = 100000.0;
		}
		num = Hurt(num, playSplatAudio, velocity, hitShield: true, createDamagePart: true, smashHurt);
		character.isSmash = false;
		return num;
	}

	public double SkipInvincibleDealHurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true)
	{
		if (character is TowerDefenseGravestone { IsPermanentObstacle: not false })
		{
			return num;
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost && character is TowerDefenseZombie)
		{
			return num;
		}
		TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
		if (character is TowerDefenseZombie && playSplatAudio)
		{
			if (impactAudio != "")
			{
				AudioManager.Instance.AudioPlay(impactAudio);
			}
			else
			{
				AudioManager.Instance.AudioPlay("SplatNormal");
			}
		}
		TowerDefensePerfProfiler.EndSpikeProbe("damage.body.audio", in probe, playSplatAudio ? 1 : 0);
		TowerDefensePerfProfiler.End("damage.body.audio", probe.StartTicks, playSplatAudio ? 1 : 0);
		TowerDefensePerfProfiler.SpikeProbe probe2 = TowerDefensePerfProfiler.BeginSpikeProbe();
		if (damageHitpointsFloor > 0.0 && num > 0.0 && hitpoints > 0.0 && !die)
		{
			num = Math.Min(num, Math.Max(0.0, hitpoints - damageHitpointsFloor));
		}
		if (hitpoints > num)
		{
			double num2 = num;
			character.EmitBodyHurt((int)num);
			hitpoints -= num;
			num = 0.0;
			if (num2 > 0.0 && CanSkipStableBodyDamagePostProcessing())
			{
				TowerDefensePerfProfiler.EndSpikeProbe("damage.body.applyStable", in probe2, 1);
				return 0.0;
			}
		}
		else
		{
			character.EmitBodyHurt((int)hitpoints);
			num -= hitpoints;
			hitpoints = 0.0;
		}
		TowerDefensePerfProfiler.EndSpikeProbe("damage.body.apply", in probe2, 1);
		TowerDefensePerfProfiler.SpikeProbe probe3 = TowerDefensePerfProfiler.BeginSpikeProbe();
		if (damagePointData != null)
		{
			double num3 = (hitpoints - hitpointsNearDeath) / (hitpointsSave - hitpointsNearDeath);
			if ((uint)damagePointIndex < (uint)_damagePointPercentages.Length)
			{
				while ((uint)damagePointIndex < (uint)_damagePointPercentages.Length && num3 <= _damagePointPercentages[damagePointIndex])
				{
					string text = _damagePointNames[damagePointIndex];
					string text2 = _damagePointAudios[damagePointIndex];
					if (createDamagePart)
					{
						TowerDefensePerfProfiler.SpikeProbe probe4 = TowerDefensePerfProfiler.BeginSpikeProbe();
						velocity = ((!(velocity == default(Vector2))) ? new Vector2(velocity.X * (float)GD.RandRange(0.75, 1.25), velocity.Y * (float)GD.RandRange(0.75, 1.25)) : new Vector2((float)GD.RandRange(-100.0, 100.0) * (float)GD.RandRange(0.75, 1.25), -300f * (float)GD.RandRange(0.75, 1.25)));
						character.DamagePartCreate(text, null, velocity, keepSlotScale: true, default, fromSync: false, null, 0L);
						TowerDefensePerfProfiler.End("damage.stage.part", probe4.StartTicks, 1);
						TowerDefensePerfProfiler.EndSpikeProbe("damage.stage.part", in probe4, 1, 0.5);
					}
					TowerDefensePerfProfiler.SpikeProbe probe5 = TowerDefensePerfProfiler.BeginSpikeProbe();
					damagePointData.CreateEffect(character.sprite, text, character.gridPos);
					TowerDefensePerfProfiler.End("damage.stage.effect", probe5.StartTicks, 1);
					TowerDefensePerfProfiler.EndSpikeProbe("damage.stage.effect", in probe5, 1, 0.5);
					if (playSplatAudio && text2 != "")
					{
						long startTicks = TowerDefensePerfProfiler.BeginHotPath();
						AudioManager.Instance.AudioPlay(text2);
						TowerDefensePerfProfiler.End("damage.stage.audio", startTicks, 1);
					}
					long startTicks2 = TowerDefensePerfProfiler.BeginHotPath();
					SetDamageStage(damagePointIndex);
					TowerDefensePerfProfiler.End("damage.stage.state", startTicks2, 1);
					damagePointIndex++;
					if (damagePointIndex >= damagePoints.Count)
					{
						break;
					}
				}
			}
		}
		TowerDefensePerfProfiler.EndSpikeProbe("damage.body.damageStages", in probe3, damagePointIndex);
		TowerDefensePerfProfiler.End("damage.body.damageStages", probe3.StartTicks, damagePointIndex);
		TowerDefensePerfProfiler.SpikeProbe probe6 = TowerDefensePerfProfiler.BeginSpikeProbe();
		if (!nearDie && hitpoints <= hitpointsNearDeath)
		{
			if (!keepArmor)
			{
				long startTicks3 = TowerDefensePerfProfiler.BeginHotPath();
				ArmorClear();
				TowerDefensePerfProfiler.End("damage.terminal.armorClear", startTicks3, 1);
			}
			TowerDefensePerfProfiler.SpikeProbe probe7 = TowerDefensePerfProfiler.BeginSpikeProbe();
			EmitHitpointsNearDie();
			TowerDefensePerfProfiler.End("damage.terminal.nearDieCallbacks", probe7.StartTicks, 1);
			TowerDefensePerfProfiler.EndSpikeProbe("damage.terminal.nearDieCallbacks", in probe7, 1, 0.5);
			nearDie = true;
		}
		if (!keepAlive && hitpoints <= 0.0)
		{
			if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost && character is TowerDefensePlant)
			{
				hitpoints = 1.0;
				return num;
			}
			if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost && character is TowerDefenseZombie)
			{
				hitpoints = 1.0;
			}
			else
			{
				long startTicks4 = TowerDefensePerfProfiler.BeginHotPath();
				DieMethod();
				TowerDefensePerfProfiler.End("damage.terminal.dieMethod", startTicks4, 1);
			}
			TowerDefensePerfProfiler.SpikeProbe probe8 = TowerDefensePerfProfiler.BeginSpikeProbe();
			EmitHitpointsEmpty();
			TowerDefensePerfProfiler.End("damage.terminal.emptyCallbacks", probe8.StartTicks, 1);
			TowerDefensePerfProfiler.EndSpikeProbe("damage.terminal.emptyCallbacks", in probe8, 1, 0.5);
		}
		TowerDefensePerfProfiler.EndSpikeProbe("damage.body.terminal", in probe6, (hitpoints <= 0.0) ? 1 : 0);
		TowerDefensePerfProfiler.End("damage.body.terminal", probe6.StartTicks, (hitpoints <= 0.0) ? 1 : 0);
		return num;
	}

	private bool CanSkipStableBodyDamagePostProcessing()
	{
		if (hitpoints <= 0.0 || (!nearDie && hitpoints <= hitpointsNearDeath))
		{
			return false;
		}
		if (damagePointData == null || (uint)damagePointIndex >= (uint)_damagePointPercentages.Length)
		{
			return true;
		}
		double num = hitpointsSave - hitpointsNearDeath;
		if (!(num > 0.0))
		{
			return false;
		}
		double num2 = hitpointsNearDeath + num * _damagePointPercentages[damagePointIndex];
		return hitpoints > num2;
	}

	public double DealHurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true)
	{
		if (invincible)
		{
			character.EmitBodyHurt(0);
			return 0.0;
		}
		return SkipInvincibleDealHurt(num, playSplatAudio, velocity, createDamagePart);
	}

	public void Health(double num)
	{
		hitpoints += num;
		RefreshDamagePoint();
	}

	internal void RefreshArmorRuntimeIndex()
	{
		RebuildArmorRuntimeList(armorList, ref _armorRuntime);
		RebuildArmorRuntimeList(armorShield, ref _armorShieldRuntime);
		RebuildArmorRuntimeList(armorHelm, ref _armorHelmRuntime);
		RebuildArmorRuntimeList(armorBody, ref _armorBodyRuntime);
		RebuildArmorRuntimeList(armorHeadCover, ref _armorHeadCoverRuntime);
	}

	private static void RebuildArmorRuntimeList(Array<TowerDefenseArmorInstance> source, ref List<TowerDefenseArmorInstance> runtime)
	{
		runtime?.Clear();
		int num = source?.Count ?? 0;
		if (num != 0)
		{
			if (runtime == null)
			{
				runtime = new List<TowerDefenseArmorInstance>(num);
			}
			for (int i = 0; i < num; i++)
			{
				runtime.Add(source[i]);
			}
		}
	}

	private void AddArmorRuntimeReferences(TowerDefenseArmorInstance instance, int flags)
	{
		(_armorRuntime ?? (_armorRuntime = new List<TowerDefenseArmorInstance>(1))).Add(instance);
		if ((flags & 4) != 0)
		{
			(_armorShieldRuntime ?? (_armorShieldRuntime = new List<TowerDefenseArmorInstance>(1))).Add(instance);
		}
		if ((flags & 8) != 0)
		{
			(_armorHelmRuntime ?? (_armorHelmRuntime = new List<TowerDefenseArmorInstance>(1))).Add(instance);
		}
		if ((flags & 2) != 0)
		{
			(_armorBodyRuntime ?? (_armorBodyRuntime = new List<TowerDefenseArmorInstance>(1))).Add(instance);
		}
		if ((flags & 0x800) != 0)
		{
			(_armorHeadCoverRuntime ?? (_armorHeadCoverRuntime = new List<TowerDefenseArmorInstance>(1))).Add(instance);
		}
	}

	internal void RemoveArmorRuntimeReferences(TowerDefenseArmorInstance instance)
	{
		armorList.Remove(instance);
		_armorRuntime?.Remove(instance);
		int num = instance?.armorMethodFlags ?? 0;
		if ((num & 4) != 0)
		{
			armorShield.Remove(instance);
			_armorShieldRuntime?.Remove(instance);
		}
		if ((num & 8) != 0)
		{
			armorHelm.Remove(instance);
			_armorHelmRuntime?.Remove(instance);
		}
		if ((num & 2) != 0)
		{
			armorBody.Remove(instance);
			_armorBodyRuntime?.Remove(instance);
		}
		if ((num & 0x800) != 0)
		{
			armorHeadCover.Remove(instance);
			_armorHeadCoverRuntime?.Remove(instance);
		}
	}

	private double ApplyDealHurtReduce(double num)
	{
		if (skipDealHurtReduce)
		{
			return num;
		}
		if (dealHurtReduce > 0.0 && num > 0.0)
		{
			num = Math.Max(0.0, num - dealHurtReduce);
			if (num <= 0.0)
			{
				character.EmitDamageBlocked();
			}
		}
		return num;
	}

	private double _ShieldAbsorbDamage(double num)
	{
		if (num <= 0.0)
		{
			return 0.0;
		}
		if (!(character is TowerDefensePlant))
		{
			return num;
		}
		if (!GodotObject.IsInstanceValid(character.cell))
		{
			return num;
		}
		TowerDefenseItemSheild itemShield = character.cell.itemShield;
		if (!GodotObject.IsInstanceValid(itemShield) || itemShield == character)
		{
			return num;
		}
		if (itemShield.instance.hypnoses != character.instance.hypnoses)
		{
			return num;
		}
		if (hitpoints - num <= 0.0 && itemShield.ShieldBlockLethal())
		{
			character.EmitBodyHurt(0);
			return 0.0;
		}
		return itemShield.ShieldAbsorbDamage(num);
	}

	public bool ArmorHas(string armorName)
	{
		foreach (TowerDefenseArmorInstance armor in armorList)
		{
			if (armor.slotConfig.armorName == armorName)
			{
				return true;
			}
		}
		return false;
	}

	internal bool HasBurstWheelArmor()
	{
		if (_armorRuntime == null)
		{
			return false;
		}
		for (int i = 0; i < _armorRuntime.Count; i++)
		{
			TowerDefenseArmorInstance towerDefenseArmorInstance = _armorRuntime[i];
			if (towerDefenseArmorInstance != null && !towerDefenseArmorInstance.isRemove && !(towerDefenseArmorInstance.hitPoints <= 0.0) && (towerDefenseArmorInstance.armorMethodFlags & 0x1000) != 0)
			{
				return true;
			}
		}
		return false;
	}

	public void ArmorAdd(string armorName)
	{
		if (!GodotObject.IsInstanceValid(armorData) || string.IsNullOrEmpty(armorName) || ArmorHas(armorName))
		{
			return;
		}
		ArmorSlotConfig orCreateSlotConfig = armorData.GetOrCreateSlotConfig(armorName);
		if (orCreateSlotConfig == null)
		{
			return;
		}
		TowerDefenseArmorTypeData armorType = TowerDefenseArmorRegistry.GetArmorType(armorName);
		if (IsExclusiveWearableHelmet(armorType))
		{
			foreach (TowerDefenseArmorInstance item in armorList.Duplicate())
			{
				if (IsExclusiveWearableHelmet(item?.typeData))
				{
					item.RemoveArmor(ArmorRemovalReason.Replaced);
					ArmorDestroy(item);
				}
			}
		}
		if (armorName == "SpecialHelmet")
		{
			armorOverrideUnUseBuffFlagSave = unUseBuffFlags;
			unUseBuffFlags = -1;
		}
		TowerDefenseArmorInstance towerDefenseArmorInstance = new TowerDefenseArmorInstance(character, orCreateSlotConfig);
		towerDefenseArmorInstance.remove += ArmorDestroy;
		towerDefenseArmorInstance.hitpointsEmpty += ArmorDestroy;
		towerDefenseArmorInstance.damagePointReach += ArmorDamagePointReachHandler;
		towerDefenseArmorInstance.hitpointScale = hitpointScale;
		armorList.Add(towerDefenseArmorInstance);
		if (armorType != null && (armorType.armorMethodFlags & 4) != 0)
		{
			armorShield.Add(towerDefenseArmorInstance);
		}
		if (armorType != null && (armorType.armorMethodFlags & 8) != 0)
		{
			armorHelm.Add(towerDefenseArmorInstance);
		}
		if (armorType != null && (armorType.armorMethodFlags & 2) != 0)
		{
			armorBody.Add(towerDefenseArmorInstance);
		}
		if (armorType != null && (armorType.armorMethodFlags & 0x800) != 0)
		{
			armorHeadCover.Add(towerDefenseArmorInstance);
		}
		if (armorType != null && (armorType.armorMethodFlags & 0x80) != 0 && !character.damagePart.ContainsKey(armorName))
		{
			character.damagePart[armorName] = orCreateSlotConfig;
		}
		AddArmorRuntimeReferences(towerDefenseArmorInstance, towerDefenseArmorInstance.armorMethodFlags);
	}

	private static bool IsExclusiveWearableHelmet(TowerDefenseArmorTypeData typeData)
	{
		if (!GodotObject.IsInstanceValid(typeData))
		{
			return false;
		}
		int armorMethodFlags = typeData.armorMethodFlags;
		if ((armorMethodFlags & 8) != 0 && (armorMethodFlags & 0x80) != 0)
		{
			return (armorMethodFlags & 2) == 0;
		}
		return false;
	}

	public void ArmorDelete(string armorName, bool createDamagePart = true)
	{
		foreach (TowerDefenseArmorInstance item in armorList.Duplicate())
		{
			if (item.slotConfig.armorName == armorName)
			{
				item.Hurt(10000000.0, playSplatAudio: false, Vector2.Zero, createDamagePart);
			}
		}
	}

	public void ArmorDamagePointReachHandler(TowerDefenseArmorInstance instance, int stage)
	{
		armorDamagePointReach?.Invoke(instance.slotConfig.armorName, stage);
	}

	public void ArmorDestroy(TowerDefenseArmorInstance instance)
	{
		if (instance.slotConfig.armorName == "SpecialHelmet")
		{
			unUseBuffFlags = armorOverrideUnUseBuffFlagSave;
		}
		instance.isRemove = true;
		RemoveArmorRuntimeReferences(instance);
		armorHitpointsEmpty?.Invoke(instance.slotConfig.armorName);
	}

	public void ArmorClear()
	{
		foreach (TowerDefenseArmorInstance item in armorList.Duplicate())
		{
			item.Hurt(10000000.0, playSplatAudio: false, Vector2.Zero, createDamagePart: true, ignoreLimit: true);
		}
	}

	public TowerDefenseMagnet ArmorDraw(TowerDefenseArmorInstance instance)
	{
		if (!GodotObject.IsInstanceValid(instance) || instance.isRemove || !armorList.Contains(instance))
		{
			return null;
		}
		TowerDefenseMagnet towerDefenseMagnet = instance.Draw();
		if (!GodotObject.IsInstanceValid(towerDefenseMagnet))
		{
			return null;
		}
		character.EmitArmorHurt((int)instance.hitPoints);
		return towerDefenseMagnet;
	}

	public void RefeshHitPoint()
	{
		hitpoints = hitpointsBase;
	}

	public void DieMethod()
	{
		collisionFlags = 0;
		maskFlags = 4;
		if (!keepArmor)
		{
			ArmorClear();
		}
		die = true;
	}

	public void SetDamageStage(int index)
	{
		string text = (string)damagePoints[damagePointIndex]["Name"];
		damagePointData.SetDamagePointFliters(character.sprite, text);
		if (customData != null)
		{
			foreach (string item in character.currentCustom)
			{
				customData.SetDamagePoint(character.sprite, item, index);
			}
		}
		damagePointReach?.Invoke(text);
	}

	public void RefreshDamagePoint()
	{
		if (damagePointData == null || damagePointIndex == 0 || character is TowerDefenseZombie || character is TowerDefenseZombieGargantuarBase || !GodotObject.IsInstanceValid(character) || !GodotObject.IsInstanceValid(character.sprite))
		{
			return;
		}
		double num = (hitpoints - hitpointsNearDeath) / (hitpointsSave - hitpointsNearDeath);
		int num2 = 0;
		if (hitpoints < hitpointsSave)
		{
			for (int i = 0; i < damagePoints.Count && num <= (double)damagePoints[i]["Persontage"]; i++)
			{
				num2 = i + 1;
			}
		}
		if (num2 == damagePointIndex)
		{
			return;
		}
		damagePointData.ClearDamagePointAll(character.sprite);
		if (customData != null)
		{
			foreach (string item in character.currentCustom)
			{
				customData.ClearDamagePoint(character.sprite, item);
			}
		}
		for (int j = 0; j < num2; j++)
		{
			string damagePointName = (string)damagePoints[j]["Name"];
			damagePointData.SetDamagePointFliters(character.sprite, damagePointName);
			if (customData == null)
			{
				continue;
			}
			foreach (string item2 in character.currentCustom)
			{
				customData.SetDamagePoint(character.sprite, item2, j);
			}
		}
		damagePointIndex = num2;
	}

	public Dictionary ExportSave()
	{
		Dictionary dictionary = new Dictionary
		{
			["hitpoints"] = hitpoints,
			["hitpointsSave"] = hitpointsSave,
			["hitpointScale"] = hitpointScale,
			["hitpointScaleSave"] = hitpointScaleSave,
			["nearDie"] = nearDie,
			["die"] = die,
			["invincible"] = invincible,
			["invincibleHurt"] = invincibleHurt,
			["invincibleSmash"] = invincibleSmash,
			["sleep"] = sleep,
			["wakeUp"] = wakeUp,
			["hypnoses"] = hypnoses,
			["hologram"] = hologram,
			["damagePointIndex"] = damagePointIndex,
			["dealHurtScale"] = dealHurtScale,
			["dealHurtReduce"] = dealHurtReduce,
			["collisionFlags"] = collisionFlags,
			["maskFlags"] = maskFlags,
			["unUseBuffFlags"] = unUseBuffFlags,
			["armorOverrideUnUseBuffFlagSave"] = armorOverrideUnUseBuffFlagSave,
			["explosionHurt"] = explosionHurt,
			["explosionHurtSave"] = explosionHurtSave,
			["smashHurt"] = smashHurt,
			["dragHurt"] = dragHurt,
			["spikeHurt"] = spikeHurt,
			["biteHurt"] = biteHurt,
			["canCollection"] = canCollection,
			["canBeCollection"] = canBeCollection,
			["keepArmor"] = keepArmor,
			["keepAlive"] = keepAlive,
			["physiqueTypeFlags"] = physiqueTypeFlags,
			["elementFlags"] = elementFlags,
			["hitpointsNearDeath"] = hitpointsNearDeath,
			["hitpointsBase"] = hitpointsBase,
			["zombiePhysique"] = (int)zombiePhysique,
			["height"] = (int)height,
			["isChests"] = isChests
		};
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (TowerDefenseArmorInstance armor in armorList)
		{
			if (!armor.isRemove)
			{
				array.Add(armor.ExportSave());
			}
		}
		dictionary["armorList"] = array;
		return dictionary;
	}

	public void ImportSave(Dictionary data)
	{
		hitpointScaleSave = (data.ContainsKey("hitpointScaleSave") ? data["hitpointScaleSave"].AsDouble() : hitpointScaleSave);
		hitpointScale = (data.ContainsKey("hitpointScale") ? data["hitpointScale"].AsDouble() : hitpointScale);
		hitpointsSave = (data.ContainsKey("hitpointsSave") ? data["hitpointsSave"].AsDouble() : hitpointsSave);
		hitpoints = (data.ContainsKey("hitpoints") ? data["hitpoints"].AsDouble() : hitpoints);
		nearDie = (data.ContainsKey("nearDie") ? data["nearDie"].AsBool() : nearDie);
		die = (data.ContainsKey("die") ? data["die"].AsBool() : die);
		invincible = (data.ContainsKey("invincible") ? data["invincible"].AsBool() : invincible);
		invincibleHurt = (data.ContainsKey("invincibleHurt") ? data["invincibleHurt"].AsBool() : invincibleHurt);
		invincibleSmash = (data.ContainsKey("invincibleSmash") ? data["invincibleSmash"].AsBool() : invincibleSmash);
		sleep = (data.ContainsKey("sleep") ? data["sleep"].AsBool() : sleep);
		wakeUp = (data.ContainsKey("wakeUp") ? data["wakeUp"].AsBool() : wakeUp);
		hypnoses = (data.ContainsKey("hypnoses") ? data["hypnoses"].AsBool() : hypnoses);
		hologram = (data.ContainsKey("hologram") ? data["hologram"].AsBool() : hologram);
		damagePointIndex = (data.ContainsKey("damagePointIndex") ? data["damagePointIndex"].AsInt32() : damagePointIndex);
		dealHurtScale = (data.ContainsKey("dealHurtScale") ? data["dealHurtScale"].AsDouble() : dealHurtScale);
		dealHurtReduce = (data.ContainsKey("dealHurtReduce") ? data["dealHurtReduce"].AsDouble() : dealHurtReduce);
		collisionFlags = (data.ContainsKey("collisionFlags") ? data["collisionFlags"].AsInt32() : collisionFlags);
		maskFlags = (data.ContainsKey("maskFlags") ? data["maskFlags"].AsInt32() : maskFlags);
		unUseBuffFlags = (data.ContainsKey("unUseBuffFlags") ? data["unUseBuffFlags"].AsInt32() : unUseBuffFlags);
		armorOverrideUnUseBuffFlagSave = (data.ContainsKey("armorOverrideUnUseBuffFlagSave") ? data["armorOverrideUnUseBuffFlagSave"].AsInt32() : armorOverrideUnUseBuffFlagSave);
		explosionHurt = (data.ContainsKey("explosionHurt") ? data["explosionHurt"].AsDouble() : explosionHurt);
		explosionHurtSave = (data.ContainsKey("explosionHurtSave") ? data["explosionHurtSave"].AsDouble() : explosionHurtSave);
		smashHurt = (data.ContainsKey("smashHurt") ? data["smashHurt"].AsDouble() : smashHurt);
		dragHurt = (data.ContainsKey("dragHurt") ? data["dragHurt"].AsDouble() : dragHurt);
		spikeHurt = (data.ContainsKey("spikeHurt") ? data["spikeHurt"].AsDouble() : spikeHurt);
		biteHurt = (data.ContainsKey("biteHurt") ? data["biteHurt"].AsDouble() : biteHurt);
		canCollection = (data.ContainsKey("canCollection") ? data["canCollection"].AsBool() : canCollection);
		canBeCollection = (data.ContainsKey("canBeCollection") ? data["canBeCollection"].AsBool() : canBeCollection);
		keepArmor = (data.ContainsKey("keepArmor") ? data["keepArmor"].AsBool() : keepArmor);
		keepAlive = (data.ContainsKey("keepAlive") ? data["keepAlive"].AsBool() : keepAlive);
		physiqueTypeFlags = (data.ContainsKey("physiqueTypeFlags") ? data["physiqueTypeFlags"].AsInt32() : physiqueTypeFlags);
		elementFlags = (GodotObject.IsInstanceValid(config) ? config.elementFlags : elementFlags);
		hitpointsNearDeath = (data.ContainsKey("hitpointsNearDeath") ? data["hitpointsNearDeath"].AsDouble() : hitpointsNearDeath);
		hitpointsBase = (data.ContainsKey("hitpointsBase") ? data["hitpointsBase"].AsDouble() : hitpointsBase);
		zombiePhysique = (data.ContainsKey("zombiePhysique") ? ((TowerDefenseEnum.ZOMBIE_PHYSIQUE)data["zombiePhysique"].AsInt32()) : zombiePhysique);
		height = (data.ContainsKey("height") ? ((TowerDefenseEnum.CHARACTER_HEIGHT)data["height"].AsInt32()) : height);
		isChests = (data.ContainsKey("isChests") ? data["isChests"].AsBool() : isChests);
		if (!data.ContainsKey("armorList"))
		{
			return;
		}
		foreach (Variant item in data["armorList"].AsGodotArray())
		{
			Dictionary dictionary = item.AsGodotDictionary();
			string text = (dictionary.ContainsKey("armorName") ? dictionary["armorName"].AsString() : "");
			if (text == "")
			{
				continue;
			}
			foreach (TowerDefenseArmorInstance armor in armorList)
			{
				if (armor.slotConfig.armorName == text)
				{
					armor.ImportSave(dictionary);
					break;
				}
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(43)
		{
			new MethodInfo(MethodName.IsMagicDamage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProjectileEffectsBlockedByArmor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fireMethodFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProjectileDamageableDurability, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitHitpointsNearDie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearHitpointsEmptyListeners, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "_config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HurtWithAttackConfig, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "attackConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProjectileHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMeteorProjectile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FlagHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "projectileHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExplodeHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExplodeHurtCore, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasExplosionShieldInCell, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateExplosionAshEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DeferNetworkExplosionDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "damageKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SkipInvincibleHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hitShield", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Hurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hitShield", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "damageLimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SmashHurtApply, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SkipInvincibleDealHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanSkipStableBodyDamagePostProcessing, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DealHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Health, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshArmorRuntimeIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddArmorRuntimeReferences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "flags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveArmorRuntimeReferences, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyDealHurtReduce, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ShieldAbsorbDamage, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorHas, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasBurstWheelArmor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ArmorAdd, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsExclusiveWearableHelmet, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "typeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorDelete, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorDamagePointReachHandler, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorClear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ArmorDraw, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefeshHitPoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieMethod, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetDamageStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshDamagePoint, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsMagicDamage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMagicDamage(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ProjectileEffectsBlockedByArmor && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ProjectileEffectsBlockedByArmor(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.GetProjectileDamageableDurability && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetProjectileDamageableDurability(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.EmitHitpointsNearDie && args.Count == 0)
		{
			EmitHitpointsNearDie();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitHitpointsEmpty && args.Count == 0)
		{
			EmitHitpointsEmpty();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearHitpointsEmptyListeners && args.Count == 0)
		{
			ClearHitpointsEmptyListeners();
			ret = default;
			return true;
		}
		if (method == MethodName._Init && args.Count == 2)
		{
			_Init(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HurtWithAttackConfig && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(HurtWithAttackConfig(VariantUtils.ConvertTo<AttackConfig>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.ProjectileHurt && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<double>(ProjectileHurt(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5])));
			return true;
		}
		if (method == MethodName.IsMeteorProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMeteorProjectile(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FlagHurt && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<double>(FlagHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<int>(in args[6])));
			return true;
		}
		if (method == MethodName.ExplodeHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ExplodeHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.ExplodeHurtCore && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ExplodeHurtCore(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.HasExplosionShieldInCell && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasExplosionShieldInCell(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateExplosionAshEffect && args.Count == 0)
		{
			CreateExplosionAshEffect();
			ret = default;
			return true;
		}
		if (method == MethodName.DeferNetworkExplosionDestroy && args.Count == 1)
		{
			DeferNetworkExplosionDestroy(VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SkipInvincibleHurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(SkipInvincibleHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.Hurt && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<double>(Hurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<double>(in args[5])));
			return true;
		}
		if (method == MethodName.SmashHurtApply && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(SmashHurtApply(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.SkipInvincibleDealHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(SkipInvincibleDealHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.CanSkipStableBodyDamagePostProcessing && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSkipStableBodyDamagePostProcessing());
			return true;
		}
		if (method == MethodName.DealHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(DealHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.Health && args.Count == 1)
		{
			Health(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshArmorRuntimeIndex && args.Count == 0)
		{
			RefreshArmorRuntimeIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.AddArmorRuntimeReferences && args.Count == 2)
		{
			AddArmorRuntimeReferences(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveArmorRuntimeReferences && args.Count == 1)
		{
			RemoveArmorRuntimeReferences(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyDealHurtReduce && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ApplyDealHurtReduce(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName._ShieldAbsorbDamage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(_ShieldAbsorbDamage(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.ArmorHas && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ArmorHas(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasBurstWheelArmor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasBurstWheelArmor());
			return true;
		}
		if (method == MethodName.ArmorAdd && args.Count == 1)
		{
			ArmorAdd(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsExclusiveWearableHelmet && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsExclusiveWearableHelmet(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0])));
			return true;
		}
		if (method == MethodName.ArmorDelete && args.Count == 2)
		{
			ArmorDelete(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorDamagePointReachHandler && args.Count == 2)
		{
			ArmorDamagePointReachHandler(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorDestroy && args.Count == 1)
		{
			ArmorDestroy(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorClear && args.Count == 0)
		{
			ArmorClear();
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorDraw && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseMagnet>(ArmorDraw(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.RefeshHitPoint && args.Count == 0)
		{
			RefeshHitPoint();
			ret = default;
			return true;
		}
		if (method == MethodName.DieMethod && args.Count == 0)
		{
			DieMethod();
			ret = default;
			return true;
		}
		if (method == MethodName.SetDamageStage && args.Count == 1)
		{
			SetDamageStage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDamagePoint && args.Count == 0)
		{
			RefreshDamagePoint();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportSave());
			return true;
		}
		if (method == MethodName.ImportSave && args.Count == 1)
		{
			ImportSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsMagicDamage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMagicDamage(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsMeteorProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMeteorProjectile(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.IsExclusiveWearableHelmet && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsExclusiveWearableHelmet(VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.IsMagicDamage)
		{
			return true;
		}
		if (method == MethodName.ProjectileEffectsBlockedByArmor)
		{
			return true;
		}
		if (method == MethodName.GetProjectileDamageableDurability)
		{
			return true;
		}
		if (method == MethodName.EmitHitpointsNearDie)
		{
			return true;
		}
		if (method == MethodName.EmitHitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.ClearHitpointsEmptyListeners)
		{
			return true;
		}
		if (method == MethodName._Init)
		{
			return true;
		}
		if (method == MethodName.HurtWithAttackConfig)
		{
			return true;
		}
		if (method == MethodName.ProjectileHurt)
		{
			return true;
		}
		if (method == MethodName.IsMeteorProjectile)
		{
			return true;
		}
		if (method == MethodName.FlagHurt)
		{
			return true;
		}
		if (method == MethodName.ExplodeHurt)
		{
			return true;
		}
		if (method == MethodName.ExplodeHurtCore)
		{
			return true;
		}
		if (method == MethodName.HasExplosionShieldInCell)
		{
			return true;
		}
		if (method == MethodName.CreateExplosionAshEffect)
		{
			return true;
		}
		if (method == MethodName.DeferNetworkExplosionDestroy)
		{
			return true;
		}
		if (method == MethodName.SkipInvincibleHurt)
		{
			return true;
		}
		if (method == MethodName.Hurt)
		{
			return true;
		}
		if (method == MethodName.SmashHurtApply)
		{
			return true;
		}
		if (method == MethodName.SkipInvincibleDealHurt)
		{
			return true;
		}
		if (method == MethodName.CanSkipStableBodyDamagePostProcessing)
		{
			return true;
		}
		if (method == MethodName.DealHurt)
		{
			return true;
		}
		if (method == MethodName.Health)
		{
			return true;
		}
		if (method == MethodName.RefreshArmorRuntimeIndex)
		{
			return true;
		}
		if (method == MethodName.AddArmorRuntimeReferences)
		{
			return true;
		}
		if (method == MethodName.RemoveArmorRuntimeReferences)
		{
			return true;
		}
		if (method == MethodName.ApplyDealHurtReduce)
		{
			return true;
		}
		if (method == MethodName._ShieldAbsorbDamage)
		{
			return true;
		}
		if (method == MethodName.ArmorHas)
		{
			return true;
		}
		if (method == MethodName.HasBurstWheelArmor)
		{
			return true;
		}
		if (method == MethodName.ArmorAdd)
		{
			return true;
		}
		if (method == MethodName.IsExclusiveWearableHelmet)
		{
			return true;
		}
		if (method == MethodName.ArmorDelete)
		{
			return true;
		}
		if (method == MethodName.ArmorDamagePointReachHandler)
		{
			return true;
		}
		if (method == MethodName.ArmorDestroy)
		{
			return true;
		}
		if (method == MethodName.ArmorClear)
		{
			return true;
		}
		if (method == MethodName.ArmorDraw)
		{
			return true;
		}
		if (method == MethodName.RefeshHitPoint)
		{
			return true;
		}
		if (method == MethodName.DieMethod)
		{
			return true;
		}
		if (method == MethodName.SetDamageStage)
		{
			return true;
		}
		if (method == MethodName.RefreshDamagePoint)
		{
			return true;
		}
		if (method == MethodName.ExportSave)
		{
			return true;
		}
		if (method == MethodName.ImportSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.maskFlags)
		{
			maskFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.hitpointScale)
		{
			hitpointScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.character)
		{
			character = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.config)
		{
			config = VariantUtils.ConvertTo<TowerDefenseCharacterConfig>(in value);
			return true;
		}
		if (name == PropertyName.zombiePhysique)
		{
			zombiePhysique = VariantUtils.ConvertTo<TowerDefenseEnum.ZOMBIE_PHYSIQUE>(in value);
			return true;
		}
		if (name == PropertyName.explosionHurt)
		{
			explosionHurt = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.smashHurt)
		{
			smashHurt = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dragHurt)
		{
			dragHurt = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spikeHurt)
		{
			spikeHurt = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.biteHurt)
		{
			biteHurt = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.explosionHurtSave)
		{
			explosionHurtSave = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitpointsSave)
		{
			hitpointsSave = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitpoints)
		{
			hitpoints = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.height)
		{
			height = VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_HEIGHT>(in value);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			collisionFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._maskFlags)
		{
			_maskFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.unUseBuffFlags)
		{
			unUseBuffFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.physiqueTypeFlags)
		{
			physiqueTypeFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.elementFlags)
		{
			elementFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.hitpointsNearDeath)
		{
			hitpointsNearDeath = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hitpointsBase)
		{
			hitpointsBase = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.impactAudio)
		{
			impactAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ashScene)
		{
			ashScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.armorData)
		{
			armorData = VariantUtils.ConvertTo<CharacterArmorData>(in value);
			return true;
		}
		if (name == PropertyName.customData)
		{
			customData = VariantUtils.ConvertTo<CharacterCustomData>(in value);
			return true;
		}
		if (name == PropertyName.isChests)
		{
			isChests = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.dealHurtScale)
		{
			dealHurtScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dealHurtReduce)
		{
			dealHurtReduce = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.skipDealHurtReduce)
		{
			skipDealHurtReduce = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.damagePointData)
		{
			damagePointData = VariantUtils.ConvertTo<CharacterDamagePointData>(in value);
			return true;
		}
		if (name == PropertyName.damagePoints)
		{
			damagePoints = VariantUtils.ConvertToArray<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.damagePointIndex)
		{
			damagePointIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._damagePointPercentages)
		{
			_damagePointPercentages = VariantUtils.ConvertTo<double[]>(in value);
			return true;
		}
		if (name == PropertyName._damagePointNames)
		{
			_damagePointNames = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName._damagePointAudios)
		{
			_damagePointAudios = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName._networkExplodeDestroyDeferred)
		{
			_networkExplodeDestroyDeferred = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.canCollection)
		{
			canCollection = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.canBeCollection)
		{
			canBeCollection = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.keepArmor)
		{
			keepArmor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.keepAlive)
		{
			keepAlive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.damageHitpointsFloor)
		{
			damageHitpointsFloor = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.die)
		{
			die = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.nearDie)
		{
			nearDie = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.invincible)
		{
			invincible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.invincibleHurt)
		{
			invincibleHurt = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.invincibleSmash)
		{
			invincibleSmash = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sleep)
		{
			sleep = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.wakeUp)
		{
			wakeUp = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hypnoses)
		{
			hypnoses = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hologram)
		{
			hologram = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hitpointScaleSave)
		{
			hitpointScaleSave = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._hitpointScale)
		{
			_hitpointScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.armorOverrideUnUseBuffFlagSave)
		{
			armorOverrideUnUseBuffFlagSave = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.armorList)
		{
			armorList = VariantUtils.ConvertToArray<TowerDefenseArmorInstance>(in value);
			return true;
		}
		if (name == PropertyName.armorShield)
		{
			armorShield = VariantUtils.ConvertToArray<TowerDefenseArmorInstance>(in value);
			return true;
		}
		if (name == PropertyName.armorHelm)
		{
			armorHelm = VariantUtils.ConvertToArray<TowerDefenseArmorInstance>(in value);
			return true;
		}
		if (name == PropertyName.armorBody)
		{
			armorBody = VariantUtils.ConvertToArray<TowerDefenseArmorInstance>(in value);
			return true;
		}
		if (name == PropertyName.armorHeadCover)
		{
			armorHeadCover = VariantUtils.ConvertToArray<TowerDefenseArmorInstance>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.IsMagicPhysique)
		{
			from = IsMagicPhysique;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.maskFlags)
		{
			from2 = maskFlags;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.hitpointScale)
		{
			value = VariantUtils.CreateFrom<double>(hitpointScale);
			return true;
		}
		if (name == PropertyName.ArmorCount)
		{
			from2 = ArmorCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.HasAnyArmor)
		{
			from = HasAnyArmor;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasShieldArmor)
		{
			from = HasShieldArmor;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasHelmetArmor)
		{
			from = HasHelmetArmor;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasBodyArmor)
		{
			from = HasBodyArmor;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasHeadCoverArmor)
		{
			from = HasHeadCoverArmor;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.character)
		{
			value = VariantUtils.CreateFrom(in character);
			return true;
		}
		if (name == PropertyName.config)
		{
			value = VariantUtils.CreateFrom(in config);
			return true;
		}
		if (name == PropertyName.zombiePhysique)
		{
			value = VariantUtils.CreateFrom(in zombiePhysique);
			return true;
		}
		if (name == PropertyName.explosionHurt)
		{
			value = VariantUtils.CreateFrom(in explosionHurt);
			return true;
		}
		if (name == PropertyName.smashHurt)
		{
			value = VariantUtils.CreateFrom(in smashHurt);
			return true;
		}
		if (name == PropertyName.dragHurt)
		{
			value = VariantUtils.CreateFrom(in dragHurt);
			return true;
		}
		if (name == PropertyName.spikeHurt)
		{
			value = VariantUtils.CreateFrom(in spikeHurt);
			return true;
		}
		if (name == PropertyName.biteHurt)
		{
			value = VariantUtils.CreateFrom(in biteHurt);
			return true;
		}
		if (name == PropertyName.explosionHurtSave)
		{
			value = VariantUtils.CreateFrom(in explosionHurtSave);
			return true;
		}
		if (name == PropertyName.hitpointsSave)
		{
			value = VariantUtils.CreateFrom(in hitpointsSave);
			return true;
		}
		if (name == PropertyName.hitpoints)
		{
			value = VariantUtils.CreateFrom(in hitpoints);
			return true;
		}
		if (name == PropertyName.height)
		{
			value = VariantUtils.CreateFrom(in height);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			value = VariantUtils.CreateFrom(in collisionFlags);
			return true;
		}
		if (name == PropertyName._maskFlags)
		{
			value = VariantUtils.CreateFrom(in _maskFlags);
			return true;
		}
		if (name == PropertyName.unUseBuffFlags)
		{
			value = VariantUtils.CreateFrom(in unUseBuffFlags);
			return true;
		}
		if (name == PropertyName.physiqueTypeFlags)
		{
			value = VariantUtils.CreateFrom(in physiqueTypeFlags);
			return true;
		}
		if (name == PropertyName.elementFlags)
		{
			value = VariantUtils.CreateFrom(in elementFlags);
			return true;
		}
		if (name == PropertyName.hitpointsNearDeath)
		{
			value = VariantUtils.CreateFrom(in hitpointsNearDeath);
			return true;
		}
		if (name == PropertyName.hitpointsBase)
		{
			value = VariantUtils.CreateFrom(in hitpointsBase);
			return true;
		}
		if (name == PropertyName.impactAudio)
		{
			value = VariantUtils.CreateFrom(in impactAudio);
			return true;
		}
		if (name == PropertyName.ashScene)
		{
			value = VariantUtils.CreateFrom(in ashScene);
			return true;
		}
		if (name == PropertyName.armorData)
		{
			value = VariantUtils.CreateFrom(in armorData);
			return true;
		}
		if (name == PropertyName.customData)
		{
			value = VariantUtils.CreateFrom(in customData);
			return true;
		}
		if (name == PropertyName.isChests)
		{
			value = VariantUtils.CreateFrom(in isChests);
			return true;
		}
		if (name == PropertyName.dealHurtScale)
		{
			value = VariantUtils.CreateFrom(in dealHurtScale);
			return true;
		}
		if (name == PropertyName.dealHurtReduce)
		{
			value = VariantUtils.CreateFrom(in dealHurtReduce);
			return true;
		}
		if (name == PropertyName.skipDealHurtReduce)
		{
			value = VariantUtils.CreateFrom(in skipDealHurtReduce);
			return true;
		}
		if (name == PropertyName.damagePointData)
		{
			value = VariantUtils.CreateFrom(in damagePointData);
			return true;
		}
		if (name == PropertyName.damagePoints)
		{
			value = VariantUtils.CreateFromArray(damagePoints);
			return true;
		}
		if (name == PropertyName.damagePointIndex)
		{
			value = VariantUtils.CreateFrom(in damagePointIndex);
			return true;
		}
		if (name == PropertyName._damagePointPercentages)
		{
			value = VariantUtils.CreateFrom(in _damagePointPercentages);
			return true;
		}
		if (name == PropertyName._damagePointNames)
		{
			value = VariantUtils.CreateFrom(in _damagePointNames);
			return true;
		}
		if (name == PropertyName._damagePointAudios)
		{
			value = VariantUtils.CreateFrom(in _damagePointAudios);
			return true;
		}
		if (name == PropertyName._networkExplodeDestroyDeferred)
		{
			value = VariantUtils.CreateFrom(in _networkExplodeDestroyDeferred);
			return true;
		}
		if (name == PropertyName.canCollection)
		{
			value = VariantUtils.CreateFrom(in canCollection);
			return true;
		}
		if (name == PropertyName.canBeCollection)
		{
			value = VariantUtils.CreateFrom(in canBeCollection);
			return true;
		}
		if (name == PropertyName.keepArmor)
		{
			value = VariantUtils.CreateFrom(in keepArmor);
			return true;
		}
		if (name == PropertyName.keepAlive)
		{
			value = VariantUtils.CreateFrom(in keepAlive);
			return true;
		}
		if (name == PropertyName.damageHitpointsFloor)
		{
			value = VariantUtils.CreateFrom(in damageHitpointsFloor);
			return true;
		}
		if (name == PropertyName.die)
		{
			value = VariantUtils.CreateFrom(in die);
			return true;
		}
		if (name == PropertyName.nearDie)
		{
			value = VariantUtils.CreateFrom(in nearDie);
			return true;
		}
		if (name == PropertyName.invincible)
		{
			value = VariantUtils.CreateFrom(in invincible);
			return true;
		}
		if (name == PropertyName.invincibleHurt)
		{
			value = VariantUtils.CreateFrom(in invincibleHurt);
			return true;
		}
		if (name == PropertyName.invincibleSmash)
		{
			value = VariantUtils.CreateFrom(in invincibleSmash);
			return true;
		}
		if (name == PropertyName.sleep)
		{
			value = VariantUtils.CreateFrom(in sleep);
			return true;
		}
		if (name == PropertyName.wakeUp)
		{
			value = VariantUtils.CreateFrom(in wakeUp);
			return true;
		}
		if (name == PropertyName.hypnoses)
		{
			value = VariantUtils.CreateFrom(in hypnoses);
			return true;
		}
		if (name == PropertyName.hologram)
		{
			value = VariantUtils.CreateFrom(in hologram);
			return true;
		}
		if (name == PropertyName.hitpointScaleSave)
		{
			value = VariantUtils.CreateFrom(in hitpointScaleSave);
			return true;
		}
		if (name == PropertyName._hitpointScale)
		{
			value = VariantUtils.CreateFrom(in _hitpointScale);
			return true;
		}
		if (name == PropertyName.armorOverrideUnUseBuffFlagSave)
		{
			value = VariantUtils.CreateFrom(in armorOverrideUnUseBuffFlagSave);
			return true;
		}
		if (name == PropertyName.armorList)
		{
			value = VariantUtils.CreateFromArray(armorList);
			return true;
		}
		if (name == PropertyName.armorShield)
		{
			value = VariantUtils.CreateFromArray(armorShield);
			return true;
		}
		if (name == PropertyName.armorHelm)
		{
			value = VariantUtils.CreateFromArray(armorHelm);
			return true;
		}
		if (name == PropertyName.armorBody)
		{
			value = VariantUtils.CreateFromArray(armorBody);
			return true;
		}
		if (name == PropertyName.armorHeadCover)
		{
			value = VariantUtils.CreateFromArray(armorHeadCover);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsMagicPhysique, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.character, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.config, PropertyHint.ResourceType, "TowerDefenseCharacterConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.zombiePhysique, PropertyHint.Enum, "NOONE,SMALL,NORMAL,MID,HUGE,CAR,BOSS", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.explosionHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.smashHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dragHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.spikeHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.biteHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.explosionHurtSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpointsSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpoints, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.height, PropertyHint.Enum, "GROUND,LOW,NORMAL,TALL", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._maskFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.maskFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.unUseBuffFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.physiqueTypeFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.elementFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpointsNearDeath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpointsBase, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.impactAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.ashScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.armorData, PropertyHint.ResourceType, "CharacterArmorData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.customData, PropertyHint.ResourceType, "CharacterCustomData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isChests, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dealHurtScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.dealHurtReduce, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.skipDealHurtReduce, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.damagePointData, PropertyHint.ResourceType, "CharacterDamagePointData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.damagePoints, PropertyHint.TypeString, "27/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.damagePointIndex, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedFloat64Array, PropertyName._damagePointPercentages, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._damagePointNames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._damagePointAudios, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._networkExplodeDestroyDeferred, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canCollection, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canBeCollection, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.keepArmor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.keepAlive, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.damageHitpointsFloor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.die, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.nearDie, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.invincible, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.invincibleHurt, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.invincibleSmash, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.sleep, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.wakeUp, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hypnoses, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hologram, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpointScaleSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._hitpointScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.hitpointScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.armorOverrideUnUseBuffFlagSave, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.armorList, PropertyHint.TypeString, "24/17:TowerDefenseArmorInstance", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.armorShield, PropertyHint.TypeString, "24/17:TowerDefenseArmorInstance", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.armorHelm, PropertyHint.TypeString, "24/17:TowerDefenseArmorInstance", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.armorBody, PropertyHint.TypeString, "24/17:TowerDefenseArmorInstance", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.armorHeadCover, PropertyHint.TypeString, "24/17:TowerDefenseArmorInstance", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ArmorCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasAnyArmor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasShieldArmor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasHelmetArmor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasBodyArmor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasHeadCoverArmor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.maskFlags, Variant.From<int>(maskFlags));
		info.AddProperty(PropertyName.hitpointScale, Variant.From<double>(hitpointScale));
		info.AddProperty(PropertyName.character, Variant.From(in character));
		info.AddProperty(PropertyName.config, Variant.From(in config));
		info.AddProperty(PropertyName.zombiePhysique, Variant.From(in zombiePhysique));
		info.AddProperty(PropertyName.explosionHurt, Variant.From(in explosionHurt));
		info.AddProperty(PropertyName.smashHurt, Variant.From(in smashHurt));
		info.AddProperty(PropertyName.dragHurt, Variant.From(in dragHurt));
		info.AddProperty(PropertyName.spikeHurt, Variant.From(in spikeHurt));
		info.AddProperty(PropertyName.biteHurt, Variant.From(in biteHurt));
		info.AddProperty(PropertyName.explosionHurtSave, Variant.From(in explosionHurtSave));
		info.AddProperty(PropertyName.hitpointsSave, Variant.From(in hitpointsSave));
		info.AddProperty(PropertyName.hitpoints, Variant.From(in hitpoints));
		info.AddProperty(PropertyName.height, Variant.From(in height));
		info.AddProperty(PropertyName.collisionFlags, Variant.From(in collisionFlags));
		info.AddProperty(PropertyName._maskFlags, Variant.From(in _maskFlags));
		info.AddProperty(PropertyName.unUseBuffFlags, Variant.From(in unUseBuffFlags));
		info.AddProperty(PropertyName.physiqueTypeFlags, Variant.From(in physiqueTypeFlags));
		info.AddProperty(PropertyName.elementFlags, Variant.From(in elementFlags));
		info.AddProperty(PropertyName.hitpointsNearDeath, Variant.From(in hitpointsNearDeath));
		info.AddProperty(PropertyName.hitpointsBase, Variant.From(in hitpointsBase));
		info.AddProperty(PropertyName.impactAudio, Variant.From(in impactAudio));
		info.AddProperty(PropertyName.ashScene, Variant.From(in ashScene));
		info.AddProperty(PropertyName.armorData, Variant.From(in armorData));
		info.AddProperty(PropertyName.customData, Variant.From(in customData));
		info.AddProperty(PropertyName.isChests, Variant.From(in isChests));
		info.AddProperty(PropertyName.dealHurtScale, Variant.From(in dealHurtScale));
		info.AddProperty(PropertyName.dealHurtReduce, Variant.From(in dealHurtReduce));
		info.AddProperty(PropertyName.skipDealHurtReduce, Variant.From(in skipDealHurtReduce));
		info.AddProperty(PropertyName.damagePointData, Variant.From(in damagePointData));
		info.AddProperty(PropertyName.damagePoints, Variant.CreateFrom(damagePoints));
		info.AddProperty(PropertyName.damagePointIndex, Variant.From(in damagePointIndex));
		info.AddProperty(PropertyName._damagePointPercentages, Variant.From(in _damagePointPercentages));
		info.AddProperty(PropertyName._damagePointNames, Variant.From(in _damagePointNames));
		info.AddProperty(PropertyName._damagePointAudios, Variant.From(in _damagePointAudios));
		info.AddProperty(PropertyName._networkExplodeDestroyDeferred, Variant.From(in _networkExplodeDestroyDeferred));
		info.AddProperty(PropertyName.canCollection, Variant.From(in canCollection));
		info.AddProperty(PropertyName.canBeCollection, Variant.From(in canBeCollection));
		info.AddProperty(PropertyName.keepArmor, Variant.From(in keepArmor));
		info.AddProperty(PropertyName.keepAlive, Variant.From(in keepAlive));
		info.AddProperty(PropertyName.damageHitpointsFloor, Variant.From(in damageHitpointsFloor));
		info.AddProperty(PropertyName.die, Variant.From(in die));
		info.AddProperty(PropertyName.nearDie, Variant.From(in nearDie));
		info.AddProperty(PropertyName.invincible, Variant.From(in invincible));
		info.AddProperty(PropertyName.invincibleHurt, Variant.From(in invincibleHurt));
		info.AddProperty(PropertyName.invincibleSmash, Variant.From(in invincibleSmash));
		info.AddProperty(PropertyName.sleep, Variant.From(in sleep));
		info.AddProperty(PropertyName.wakeUp, Variant.From(in wakeUp));
		info.AddProperty(PropertyName.hypnoses, Variant.From(in hypnoses));
		info.AddProperty(PropertyName.hologram, Variant.From(in hologram));
		info.AddProperty(PropertyName.hitpointScaleSave, Variant.From(in hitpointScaleSave));
		info.AddProperty(PropertyName._hitpointScale, Variant.From(in _hitpointScale));
		info.AddProperty(PropertyName.armorOverrideUnUseBuffFlagSave, Variant.From(in armorOverrideUnUseBuffFlagSave));
		info.AddProperty(PropertyName.armorList, Variant.CreateFrom(armorList));
		info.AddProperty(PropertyName.armorShield, Variant.CreateFrom(armorShield));
		info.AddProperty(PropertyName.armorHelm, Variant.CreateFrom(armorHelm));
		info.AddProperty(PropertyName.armorBody, Variant.CreateFrom(armorBody));
		info.AddProperty(PropertyName.armorHeadCover, Variant.CreateFrom(armorHeadCover));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.maskFlags, out var value))
		{
			maskFlags = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.hitpointScale, out var value2))
		{
			hitpointScale = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.character, out var value3))
		{
			character = value3.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.config, out var value4))
		{
			config = value4.As<TowerDefenseCharacterConfig>();
		}
		if (info.TryGetProperty(PropertyName.zombiePhysique, out var value5))
		{
			zombiePhysique = value5.As<TowerDefenseEnum.ZOMBIE_PHYSIQUE>();
		}
		if (info.TryGetProperty(PropertyName.explosionHurt, out var value6))
		{
			explosionHurt = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.smashHurt, out var value7))
		{
			smashHurt = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dragHurt, out var value8))
		{
			dragHurt = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spikeHurt, out var value9))
		{
			spikeHurt = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.biteHurt, out var value10))
		{
			biteHurt = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName.explosionHurtSave, out var value11))
		{
			explosionHurtSave = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitpointsSave, out var value12))
		{
			hitpointsSave = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitpoints, out var value13))
		{
			hitpoints = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.height, out var value14))
		{
			height = value14.As<TowerDefenseEnum.CHARACTER_HEIGHT>();
		}
		if (info.TryGetProperty(PropertyName.collisionFlags, out var value15))
		{
			collisionFlags = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName._maskFlags, out var value16))
		{
			_maskFlags = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName.unUseBuffFlags, out var value17))
		{
			unUseBuffFlags = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName.physiqueTypeFlags, out var value18))
		{
			physiqueTypeFlags = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName.elementFlags, out var value19))
		{
			elementFlags = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName.hitpointsNearDeath, out var value20))
		{
			hitpointsNearDeath = value20.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hitpointsBase, out var value21))
		{
			hitpointsBase = value21.As<double>();
		}
		if (info.TryGetProperty(PropertyName.impactAudio, out var value22))
		{
			impactAudio = value22.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ashScene, out var value23))
		{
			ashScene = value23.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.armorData, out var value24))
		{
			armorData = value24.As<CharacterArmorData>();
		}
		if (info.TryGetProperty(PropertyName.customData, out var value25))
		{
			customData = value25.As<CharacterCustomData>();
		}
		if (info.TryGetProperty(PropertyName.isChests, out var value26))
		{
			isChests = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dealHurtScale, out var value27))
		{
			dealHurtScale = value27.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dealHurtReduce, out var value28))
		{
			dealHurtReduce = value28.As<double>();
		}
		if (info.TryGetProperty(PropertyName.skipDealHurtReduce, out var value29))
		{
			skipDealHurtReduce = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.damagePointData, out var value30))
		{
			damagePointData = value30.As<CharacterDamagePointData>();
		}
		if (info.TryGetProperty(PropertyName.damagePoints, out var value31))
		{
			damagePoints = value31.AsGodotArray<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.damagePointIndex, out var value32))
		{
			damagePointIndex = value32.As<int>();
		}
		if (info.TryGetProperty(PropertyName._damagePointPercentages, out var value33))
		{
			_damagePointPercentages = value33.As<double[]>();
		}
		if (info.TryGetProperty(PropertyName._damagePointNames, out var value34))
		{
			_damagePointNames = value34.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName._damagePointAudios, out var value35))
		{
			_damagePointAudios = value35.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName._networkExplodeDestroyDeferred, out var value36))
		{
			_networkExplodeDestroyDeferred = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.canCollection, out var value37))
		{
			canCollection = value37.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.canBeCollection, out var value38))
		{
			canBeCollection = value38.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.keepArmor, out var value39))
		{
			keepArmor = value39.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.keepAlive, out var value40))
		{
			keepAlive = value40.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.damageHitpointsFloor, out var value41))
		{
			damageHitpointsFloor = value41.As<double>();
		}
		if (info.TryGetProperty(PropertyName.die, out var value42))
		{
			die = value42.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.nearDie, out var value43))
		{
			nearDie = value43.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.invincible, out var value44))
		{
			invincible = value44.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.invincibleHurt, out var value45))
		{
			invincibleHurt = value45.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.invincibleSmash, out var value46))
		{
			invincibleSmash = value46.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sleep, out var value47))
		{
			sleep = value47.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.wakeUp, out var value48))
		{
			wakeUp = value48.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hypnoses, out var value49))
		{
			hypnoses = value49.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hologram, out var value50))
		{
			hologram = value50.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hitpointScaleSave, out var value51))
		{
			hitpointScaleSave = value51.As<double>();
		}
		if (info.TryGetProperty(PropertyName._hitpointScale, out var value52))
		{
			_hitpointScale = value52.As<double>();
		}
		if (info.TryGetProperty(PropertyName.armorOverrideUnUseBuffFlagSave, out var value53))
		{
			armorOverrideUnUseBuffFlagSave = value53.As<int>();
		}
		if (info.TryGetProperty(PropertyName.armorList, out var value54))
		{
			armorList = value54.AsGodotArray<TowerDefenseArmorInstance>();
		}
		if (info.TryGetProperty(PropertyName.armorShield, out var value55))
		{
			armorShield = value55.AsGodotArray<TowerDefenseArmorInstance>();
		}
		if (info.TryGetProperty(PropertyName.armorHelm, out var value56))
		{
			armorHelm = value56.AsGodotArray<TowerDefenseArmorInstance>();
		}
		if (info.TryGetProperty(PropertyName.armorBody, out var value57))
		{
			armorBody = value57.AsGodotArray<TowerDefenseArmorInstance>();
		}
		if (info.TryGetProperty(PropertyName.armorHeadCover, out var value58))
		{
			armorHeadCover = value58.AsGodotArray<TowerDefenseArmorInstance>();
		}
	}
}
