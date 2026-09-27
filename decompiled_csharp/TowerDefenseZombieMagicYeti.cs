using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter10/MagicYeti/Scene/TowerDefenseZombieMagicYeti.cs")]
public class TowerDefenseZombieMagicYeti : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public static readonly StringName UpdatePassive = "UpdatePassive";

		public static readonly StringName UpdateRageTrigger = "UpdateRageTrigger";

		public static readonly StringName RageEntered = "RageEntered";

		public static readonly StringName RageProcessing = "RageProcessing";

		public static readonly StringName RageExited = "RageExited";

		public static readonly StringName ApplyRageStats = "ApplyRageStats";

		public static readonly StringName ApplyAngryAnimationSet = "ApplyAngryAnimationSet";

		public static readonly StringName ApplyAngryAttackProfile = "ApplyAngryAttackProfile";

		public static readonly StringName ApplySpeedScale = "ApplySpeedScale";

		public static readonly StringName ApplyRageSpeedUp = "ApplyRageSpeedUp";

		public static readonly StringName PlayRageScaleTween = "PlayRageScaleTween";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName TriggerAngrySmash = "TriggerAngrySmash";

		public static readonly StringName PlayEatAudio = "PlayEatAudio";

		public static readonly StringName PlayPlantDefeatAudioIfAny = "PlayPlantDefeatAudioIfAny";

		public new static readonly StringName ProjectileHurt = "ProjectileHurt";

		public new static readonly StringName HurtWithAttackConfig = "HurtWithAttackConfig";

		public new static readonly StringName FlagHurt = "FlagHurt";

		public static readonly StringName GetChainTriggerHitpoints = "GetChainTriggerHitpoints";

		public static readonly StringName TriggerChainAfterDamage = "TriggerChainAfterDamage";

		public static readonly StringName IsMagicFlags = "IsMagicFlags";

		public static readonly StringName IsMagicProjectile = "IsMagicProjectile";

		public static readonly StringName TriggerChain = "TriggerChain";

		public static readonly StringName HideChainIfNeeded = "HideChainIfNeeded";

		public static readonly StringName HideChain = "HideChain";

		public static readonly StringName SyncLockLayers = "SyncLockLayers";

		public static readonly StringName OnSpriteAnimeStarted = "OnSpriteAnimeStarted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _rageEntered = "_rageEntered";

		public static readonly StringName _rageRequested = "_rageRequested";

		public static readonly StringName _rageStateActive = "_rageStateActive";

		public static readonly StringName _rageSpeedApplied = "_rageSpeedApplied";

		public static readonly StringName _rageHealApplied = "_rageHealApplied";

		public static readonly StringName _keepAliveBeforeRage = "_keepAliveBeforeRage";

		public static readonly StringName _rageElapsed = "_rageElapsed";

		public static readonly StringName _aliveTime = "_aliveTime";

		public static readonly StringName _chainTimer = "_chainTimer";

		public static readonly StringName _chainActive = "_chainActive";

		public static readonly StringName _appliedSpeedScale = "_appliedSpeedScale";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const string RageStateId = "zombie.magic_yeti.rage";

	private const string RageStateEvent = "ToRage";

	private const string AngryClip = "Angry";

	private const string IdleAngryClip = "IdleAngry";

	private const string EatAngryClip = "EatAngry";

	private const string DeathAngryClip = "DeathAngry";

	private const string AngryWalkClip = "WalkAngry";

	private static readonly StringName[] LockLayers = new StringName[9] { "lock1", "lock2_1", "lock2_2", "lock3_1", "lock3_2", "lock4_1", "lock4_2", "lock5_1", "lock5_2" };

	private const double RageTriggerTime = 30.0;

	private const double RageTriggerHitpoints = 1000.0;

	private const double RageBonusHitpoints = 2000.0;

	private const double NormalSpeedScale = 1.5;

	private const double RageSpeedScale = 3.0;

	private const double RageScaleMultiplier = 1.25;

	private const string SmashAttackType = "Smash";

	private const double ChainDuration = 10.0;

	private const double ChainSpeedScale = 0.5;

	private const double RageAnimeFallbackTime = 3.0;

	private StateHandle _rageStateHandle;

	private bool _stateSignalsConnected;

	private bool _rageEntered;

	private bool _rageRequested;

	private bool _rageStateActive;

	private bool _rageSpeedApplied;

	private bool _rageHealApplied;

	private bool _keepAliveBeforeRage;

	private double _rageElapsed;

	private double _aliveTime;

	private double _chainTimer;

	private bool _chainActive;

	private double _appliedSpeedScale = 1.0;

	public override void _Ready()
	{
		base._Ready();
		SyncLockLayers();
		ApplySpeedScale();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			ConnectStateSignals();
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.OnAnimeStarted += OnSpriteAnimeStarted;
			}
			SyncLockLayers();
		}
	}

	public override void _ExitTree()
	{
		DisconnectStateSignals();
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.OnAnimeStarted -= OnSpriteAnimeStarted;
		}
		base._ExitTree();
	}

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_rageStateHandle = StateMachine.GetStateById("zombie.magic_yeti.rage");
			StateHandle rageStateHandle = _rageStateHandle;
			if (rageStateHandle != null && rageStateHandle.IsValid)
			{
				_rageStateHandle.Entered += RageEntered;
				_rageStateHandle.Exited += RageExited;
				_rageStateHandle.PhysicsProcessing += RageProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_rageStateHandle != null)
			{
				_rageStateHandle.Entered -= RageEntered;
				_rageStateHandle.Exited -= RageExited;
				_rageStateHandle.PhysicsProcessing -= RageProcessing;
			}
			_rageStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		UpdatePassive(delta);
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
		UpdatePassive(delta);
	}

	public override void AttackProcessing(double delta)
	{
		if (!_rageEntered)
		{
			base.AttackProcessing(delta);
			UpdatePassive(delta);
		}
		else if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
		{
			if (!attackComponent.CanAttack())
			{
				Walk();
			}
			sprite.timeScale = timeScale * 2.0;
			UpdatePassive(delta);
		}
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		HideChain();
	}

	private void UpdatePassive(double delta)
	{
		if (!_stateSignalsConnected)
		{
			ConnectStateSignals();
		}
		_aliveTime += delta;
		HideChainIfNeeded(delta);
		UpdateRageTrigger();
		SyncLockLayers();
	}

	private void UpdateRageTrigger()
	{
		if (!_rageEntered && !_rageRequested && !die && !nearDie && !isDestroy && GodotObject.IsInstanceValid(instance) && (!(_aliveTime < 30.0) || !(instance.hitpoints > 1000.0)))
		{
			_rageRequested = SendStateEvent("ToRage");
		}
	}

	public virtual void RageEntered()
	{
		_rageEntered = true;
		_rageStateActive = true;
		_rageElapsed = 0.0;
		if (GodotObject.IsInstanceValid(instance))
		{
			_keepAliveBeforeRage = instance.keepAlive;
			instance.keepAlive = true;
			if (instance.hitpoints < 1.0)
			{
				instance.hitpoints = 1.0;
			}
		}
		ApplyRageStats();
		PlayRageScaleTween();
		sprite.SetAnimation("Angry", loop: false, 0.1);
	}

	public virtual void RageProcessing(double delta)
	{
		HideChainIfNeeded(delta);
		SyncLockLayers();
		if (GodotObject.IsInstanceValid(instance) && instance.hitpoints < 1.0)
		{
			instance.hitpoints = 1.0;
		}
		sprite.timeScale = timeScale;
		_rageElapsed += delta;
		if (_rageElapsed < 3.0)
		{
			return;
		}
		ApplyRageSpeedUp();
		Walk();
		if (_rageStateActive && _rageElapsed >= 6.0)
		{
			_rageStateActive = false;
			if (GodotObject.IsInstanceValid(instance))
			{
				instance.keepAlive = _keepAliveBeforeRage;
			}
		}
	}

	public virtual void RageExited()
	{
		_rageStateActive = false;
		_rageElapsed = 0.0;
		ApplyRageSpeedUp();
		if (GodotObject.IsInstanceValid(instance) && !instance.die)
		{
			instance.keepAlive = _keepAliveBeforeRage;
			if (instance.hitpoints < 1.0)
			{
				instance.hitpoints = 1.0;
			}
		}
	}

	private void ApplyRageStats()
	{
		if (GodotObject.IsInstanceValid(instance))
		{
			if (!_rageHealApplied)
			{
				_rageHealApplied = true;
				instance.hitpointsSave += 2000.0;
				double num = instance.hitpointsSave - instance.hitpoints;
				if (num > 0.0)
				{
					Health(num);
				}
			}
			instance.unUseBuffFlags |= 1;
			showHealthComponent?.MarkDirty();
		}
		buff?.DeleteBuff("IceSpeedDown");
		ApplyAngryAnimationSet();
		ApplyAngryAttackProfile();
		ApplySpeedScale();
	}

	private void ApplyAngryAnimationSet()
	{
		walkAnimeClip = "WalkAngry";
		swimAnimeClip = "WalkAngry";
		idleAnimeClip = "IdleAngry";
		attackAnimeClip = "EatAngry";
		attackWaterAnimeClip = "EatAngry";
		dieAnimeClip = "DeathAngry";
		dieWaterAnimeClip = "DeathAngry";
	}

	private void ApplyAngryAttackProfile()
	{
		AttackComponent attackComponent = base.attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			base.attackComponent.attackType = "Smash";
		}
	}

	private void ApplySpeedScale()
	{
		double num = 1.5;
		if (_chainActive)
		{
			num = 0.5;
		}
		else if (_rageEntered && _rageSpeedApplied)
		{
			num = 3.0;
		}
		if (!Mathf.IsEqualApprox(num, _appliedSpeedScale))
		{
			double num2 = num / _appliedSpeedScale;
			_appliedSpeedScale = num;
			timeScaleInit *= num2;
		}
	}

	private void ApplyRageSpeedUp()
	{
		if (!_rageSpeedApplied)
		{
			_rageSpeedApplied = true;
			ApplySpeedScale();
		}
	}

	private void PlayRageScaleTween()
	{
		if (GodotObject.IsInstanceValid(transformPoint))
		{
			Tween tween = CreateTween();
			tween.SetEase(Tween.EaseType.Out);
			tween.SetTrans(Tween.TransitionType.Quint);
			tween.TweenProperty(transformPoint, "scale", transformPoint.Scale * 1.25f, 0.5);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Angry" && _rageStateActive)
		{
			ApplyRageSpeedUp();
			Walk();
		}
		else if (clip == "EatAngry" && _rageEntered && !_rageStateActive)
		{
			TriggerAngrySmash();
		}
	}

	private void TriggerAngrySmash()
	{
		if (!die && !nearDie && !isDestroy && startAttack)
		{
			AttackComponent attackComponent = base.attackComponent;
			if (attackComponent != null && !attackComponent.IsReleased && base.attackComponent.CanAttack() && GodotObject.IsInstanceValid(sprite) && !sprite.pause && !(sprite.timeScale <= 0.0))
			{
				base.attackComponent.AttackExecute(((TowerDefenseZombieConfig)config).smashAttack);
				PlayEatAudio();
				PlayPlantDefeatAudioIfAny();
			}
		}
	}

	private void PlayEatAudio()
	{
		AttackComponent attackComponent = base.attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			string eatAudio = base.attackComponent.eatAudio;
			if (!string.IsNullOrEmpty(eatAudio) && GodotObject.IsInstanceValid(AudioManager.Instance))
			{
				AudioManager.Instance.AudioPlay(eatAudio);
			}
		}
	}

	private void PlayPlantDefeatAudioIfAny()
	{
		AttackComponent attackComponent = base.attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased && base.attackComponent.playPlantDefeatAudio && base.attackComponent.target is TowerDefensePlant { instance: not null } towerDefensePlant && towerDefensePlant.instance.die)
		{
			string plantDefeatAudio = base.attackComponent.plantDefeatAudio;
			if (!string.IsNullOrEmpty(plantDefeatAudio) && GodotObject.IsInstanceValid(AudioManager.Instance))
			{
				AudioManager.Instance.AudioPlay(plantDefeatAudio);
			}
		}
	}

	public override double ProjectileHurt(TowerDefenseProjectile projectile, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false)
	{
		bool magic = (GodotObject.IsInstanceValid(projectile) ? IsMagicFlags(projectile.damageFlags) : IsMagicProjectile(projectileConfig));
		double chainTriggerHitpoints = GetChainTriggerHitpoints(magic);
		double result = base.ProjectileHurt(projectile, projectileConfig, playSplatAudio, velocity, isRange);
		TriggerChainAfterDamage(chainTriggerHitpoints);
		return result;
	}

	public override double ProjectileHurt(in ProjectileHitInfo info, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false)
	{
		bool magic = ((info.useRuntimeOverrides || info.onSourceDespawn != null) ? IsMagicFlags(info.damageFlags) : IsMagicProjectile(projectileConfig));
		double chainTriggerHitpoints = GetChainTriggerHitpoints(magic);
		double result = base.ProjectileHurt(in info, projectileConfig, playSplatAudio, velocity, isRange);
		TriggerChainAfterDamage(chainTriggerHitpoints);
		return result;
	}

	public override double HurtWithAttackConfig(AttackConfig attackConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true)
	{
		double chainTriggerHitpoints = GetChainTriggerHitpoints(GodotObject.IsInstanceValid(attackConfig) && IsMagicFlags(attackConfig.damageFlags));
		double result = base.HurtWithAttackConfig(attackConfig, playSplatAudio, velocity, createDamagePart);
		TriggerChainAfterDamage(chainTriggerHitpoints);
		return result;
	}

	public override double FlagHurt(double num, int damageFlags, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		double chainTriggerHitpoints = GetChainTriggerHitpoints(IsMagicFlags(damageFlags));
		double result = base.FlagHurt(num, damageFlags, playSplatAudio, velocity);
		TriggerChainAfterDamage(chainTriggerHitpoints);
		return result;
	}

	private double GetChainTriggerHitpoints(bool magic)
	{
		if (!magic || !_rageEntered || die || nearDie || isDestroy || (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) || !GodotObject.IsInstanceValid(instance))
		{
			return -1.0;
		}
		return instance.hitpoints;
	}

	private void TriggerChainAfterDamage(double hitpointsBeforeHit)
	{
		if (hitpointsBeforeHit > 0.0 && GodotObject.IsInstanceValid(instance) && instance.hitpoints < hitpointsBeforeHit && !instance.die && !die && !nearDie && !isDestroy)
		{
			TriggerChain();
		}
	}

	private static bool IsMagicFlags(int damageFlags)
	{
		return (damageFlags & 0x40) != 0;
	}

	private static bool IsMagicProjectile(TowerDefenseProjectileConfig projectileConfig)
	{
		if (GodotObject.IsInstanceValid(projectileConfig))
		{
			return IsMagicFlags(projectileConfig.damageFlags);
		}
		return false;
	}

	private void TriggerChain()
	{
		if (_rageEntered)
		{
			if (!_chainActive)
			{
				_chainActive = true;
				ApplySpeedScale();
			}
			_chainTimer = 10.0;
			SyncLockLayers();
		}
	}

	private void HideChainIfNeeded(double delta)
	{
		if (_chainActive && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
		{
			_chainTimer -= delta;
			if (_chainTimer <= 0.0)
			{
				HideChain();
			}
		}
	}

	private void HideChain()
	{
		if (!_chainActive)
		{
			SyncLockLayers();
			return;
		}
		_chainActive = false;
		_chainTimer = 0.0;
		ApplySpeedScale();
		SyncLockLayers();
	}

	private void SyncLockLayers()
	{
		if (!GodotObject.IsInstanceValid(sprite))
		{
			return;
		}
		bool flag = _rageEntered && _chainActive && _chainTimer > 0.0 && !die && !nearDie && !isDestroy;
		StringName[] lockLayers = LockLayers;
		foreach (StringName layerName in lockLayers)
		{
			if (sprite.GetFliter(layerName) != flag)
			{
				sprite.SetFliter(layerName, flag);
			}
		}
	}

	private void OnSpriteAnimeStarted(string clip)
	{
		SyncLockLayers();
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["rageEntered"] = _rageEntered;
		dictionary["rageSpeedApplied"] = _rageSpeedApplied;
		dictionary["rageHealApplied"] = _rageHealApplied;
		dictionary["appliedSpeedScale"] = _appliedSpeedScale;
		dictionary["aliveTime"] = _aliveTime;
		dictionary["chainActive"] = _chainActive;
		dictionary["chainTimer"] = _chainTimer;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		_aliveTime = data.GetValueOrDefault("aliveTime", 0.0).AsDouble();
		_rageHealApplied = data.GetValueOrDefault("rageHealApplied", false).AsBool();
		_rageEntered = data.GetValueOrDefault("rageEntered", false).AsBool();
		_rageSpeedApplied = data.GetValueOrDefault("rageSpeedApplied", _rageEntered).AsBool();
		_rageRequested = _rageEntered;
		_chainTimer = Mathf.Max(0.0, data.GetValueOrDefault("chainTimer", 0.0).AsDouble());
		_chainActive = _rageEntered && _chainTimer > 0.0 && data.GetValueOrDefault("chainActive", false).AsBool();
		if (!_chainActive)
		{
			_chainTimer = 0.0;
		}
		if (_rageEntered)
		{
			ApplyAngryAnimationSet();
			ApplyAngryAttackProfile();
		}
		ref double appliedSpeedScale = ref _appliedSpeedScale;
		Dictionary dictionary = data;
		Variant key = "appliedSpeedScale";
		double num;
		if (_chainActive)
		{
			num = 0.5;
		}
		else
		{
			num = (_rageSpeedApplied ? 3.0 : 1.5);
		}
		appliedSpeedScale = dictionary.GetValueOrDefault(key, num).AsDouble();
		ApplySpeedScale();
		SyncLockLayers();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(37)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdatePassive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateRageTrigger, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RageEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RageProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RageExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyRageStats, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyAngryAnimationSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyAngryAttackProfile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplySpeedScale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyRageSpeedUp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayRageScaleTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TriggerAngrySmash, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayEatAudio, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayPlantDefeatAudioIfAny, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProjectileHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HurtWithAttackConfig, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "attackConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlagHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetChainTriggerHitpoints, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "magic", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TriggerChainAfterDamage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "hitpointsBeforeHit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMagicFlags, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMagicProjectile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectileConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.TriggerChain, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideChainIfNeeded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HideChain, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncLockLayers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSpriteAnimeStarted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectStateSignals && args.Count == 0)
		{
			ConnectStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectStateSignals && args.Count == 0)
		{
			DisconnectStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePassive && args.Count == 1)
		{
			UpdatePassive(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateRageTrigger && args.Count == 0)
		{
			UpdateRageTrigger();
			ret = default;
			return true;
		}
		if (method == MethodName.RageEntered && args.Count == 0)
		{
			RageEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.RageProcessing && args.Count == 1)
		{
			RageProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RageExited && args.Count == 0)
		{
			RageExited();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRageStats && args.Count == 0)
		{
			ApplyRageStats();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyAngryAnimationSet && args.Count == 0)
		{
			ApplyAngryAnimationSet();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyAngryAttackProfile && args.Count == 0)
		{
			ApplyAngryAttackProfile();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySpeedScale && args.Count == 0)
		{
			ApplySpeedScale();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRageSpeedUp && args.Count == 0)
		{
			ApplyRageSpeedUp();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayRageScaleTween && args.Count == 0)
		{
			PlayRageScaleTween();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TriggerAngrySmash && args.Count == 0)
		{
			TriggerAngrySmash();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayEatAudio && args.Count == 0)
		{
			PlayEatAudio();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayPlantDefeatAudioIfAny && args.Count == 0)
		{
			PlayPlantDefeatAudioIfAny();
			ret = default;
			return true;
		}
		if (method == MethodName.ProjectileHurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(ProjectileHurt(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.HurtWithAttackConfig && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(HurtWithAttackConfig(VariantUtils.ConvertTo<AttackConfig>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.FlagHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(FlagHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.GetChainTriggerHitpoints && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetChainTriggerHitpoints(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.TriggerChainAfterDamage && args.Count == 1)
		{
			TriggerChainAfterDamage(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsMagicFlags && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMagicFlags(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsMagicProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMagicProjectile(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.TriggerChain && args.Count == 0)
		{
			TriggerChain();
			ret = default;
			return true;
		}
		if (method == MethodName.HideChainIfNeeded && args.Count == 1)
		{
			HideChainIfNeeded(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HideChain && args.Count == 0)
		{
			HideChain();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncLockLayers && args.Count == 0)
		{
			SyncLockLayers();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSpriteAnimeStarted && args.Count == 1)
		{
			OnSpriteAnimeStarted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsMagicFlags && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMagicFlags(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsMagicProjectile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMagicProjectile(VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ConnectStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectStateSignals)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.UpdatePassive)
		{
			return true;
		}
		if (method == MethodName.UpdateRageTrigger)
		{
			return true;
		}
		if (method == MethodName.RageEntered)
		{
			return true;
		}
		if (method == MethodName.RageProcessing)
		{
			return true;
		}
		if (method == MethodName.RageExited)
		{
			return true;
		}
		if (method == MethodName.ApplyRageStats)
		{
			return true;
		}
		if (method == MethodName.ApplyAngryAnimationSet)
		{
			return true;
		}
		if (method == MethodName.ApplyAngryAttackProfile)
		{
			return true;
		}
		if (method == MethodName.ApplySpeedScale)
		{
			return true;
		}
		if (method == MethodName.ApplyRageSpeedUp)
		{
			return true;
		}
		if (method == MethodName.PlayRageScaleTween)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.TriggerAngrySmash)
		{
			return true;
		}
		if (method == MethodName.PlayEatAudio)
		{
			return true;
		}
		if (method == MethodName.PlayPlantDefeatAudioIfAny)
		{
			return true;
		}
		if (method == MethodName.ProjectileHurt)
		{
			return true;
		}
		if (method == MethodName.HurtWithAttackConfig)
		{
			return true;
		}
		if (method == MethodName.FlagHurt)
		{
			return true;
		}
		if (method == MethodName.GetChainTriggerHitpoints)
		{
			return true;
		}
		if (method == MethodName.TriggerChainAfterDamage)
		{
			return true;
		}
		if (method == MethodName.IsMagicFlags)
		{
			return true;
		}
		if (method == MethodName.IsMagicProjectile)
		{
			return true;
		}
		if (method == MethodName.TriggerChain)
		{
			return true;
		}
		if (method == MethodName.HideChainIfNeeded)
		{
			return true;
		}
		if (method == MethodName.HideChain)
		{
			return true;
		}
		if (method == MethodName.SyncLockLayers)
		{
			return true;
		}
		if (method == MethodName.OnSpriteAnimeStarted)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._rageEntered)
		{
			_rageEntered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._rageRequested)
		{
			_rageRequested = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._rageStateActive)
		{
			_rageStateActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._rageSpeedApplied)
		{
			_rageSpeedApplied = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._rageHealApplied)
		{
			_rageHealApplied = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._keepAliveBeforeRage)
		{
			_keepAliveBeforeRage = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._rageElapsed)
		{
			_rageElapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._aliveTime)
		{
			_aliveTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._chainTimer)
		{
			_chainTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._chainActive)
		{
			_chainActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._appliedSpeedScale)
		{
			_appliedSpeedScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName._rageEntered)
		{
			value = VariantUtils.CreateFrom(in _rageEntered);
			return true;
		}
		if (name == PropertyName._rageRequested)
		{
			value = VariantUtils.CreateFrom(in _rageRequested);
			return true;
		}
		if (name == PropertyName._rageStateActive)
		{
			value = VariantUtils.CreateFrom(in _rageStateActive);
			return true;
		}
		if (name == PropertyName._rageSpeedApplied)
		{
			value = VariantUtils.CreateFrom(in _rageSpeedApplied);
			return true;
		}
		if (name == PropertyName._rageHealApplied)
		{
			value = VariantUtils.CreateFrom(in _rageHealApplied);
			return true;
		}
		if (name == PropertyName._keepAliveBeforeRage)
		{
			value = VariantUtils.CreateFrom(in _keepAliveBeforeRage);
			return true;
		}
		if (name == PropertyName._rageElapsed)
		{
			value = VariantUtils.CreateFrom(in _rageElapsed);
			return true;
		}
		if (name == PropertyName._aliveTime)
		{
			value = VariantUtils.CreateFrom(in _aliveTime);
			return true;
		}
		if (name == PropertyName._chainTimer)
		{
			value = VariantUtils.CreateFrom(in _chainTimer);
			return true;
		}
		if (name == PropertyName._chainActive)
		{
			value = VariantUtils.CreateFrom(in _chainActive);
			return true;
		}
		if (name == PropertyName._appliedSpeedScale)
		{
			value = VariantUtils.CreateFrom(in _appliedSpeedScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._rageEntered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._rageRequested, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._rageStateActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._rageSpeedApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._rageHealApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._keepAliveBeforeRage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._rageElapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._aliveTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._chainTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._chainActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._appliedSpeedScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._rageEntered, Variant.From(in _rageEntered));
		info.AddProperty(PropertyName._rageRequested, Variant.From(in _rageRequested));
		info.AddProperty(PropertyName._rageStateActive, Variant.From(in _rageStateActive));
		info.AddProperty(PropertyName._rageSpeedApplied, Variant.From(in _rageSpeedApplied));
		info.AddProperty(PropertyName._rageHealApplied, Variant.From(in _rageHealApplied));
		info.AddProperty(PropertyName._keepAliveBeforeRage, Variant.From(in _keepAliveBeforeRage));
		info.AddProperty(PropertyName._rageElapsed, Variant.From(in _rageElapsed));
		info.AddProperty(PropertyName._aliveTime, Variant.From(in _aliveTime));
		info.AddProperty(PropertyName._chainTimer, Variant.From(in _chainTimer));
		info.AddProperty(PropertyName._chainActive, Variant.From(in _chainActive));
		info.AddProperty(PropertyName._appliedSpeedScale, Variant.From(in _appliedSpeedScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value))
		{
			_stateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rageEntered, out var value2))
		{
			_rageEntered = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rageRequested, out var value3))
		{
			_rageRequested = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rageStateActive, out var value4))
		{
			_rageStateActive = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rageSpeedApplied, out var value5))
		{
			_rageSpeedApplied = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rageHealApplied, out var value6))
		{
			_rageHealApplied = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._keepAliveBeforeRage, out var value7))
		{
			_keepAliveBeforeRage = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rageElapsed, out var value8))
		{
			_rageElapsed = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._aliveTime, out var value9))
		{
			_aliveTime = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName._chainTimer, out var value10))
		{
			_chainTimer = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName._chainActive, out var value11))
		{
			_chainActive = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._appliedSpeedScale, out var value12))
		{
			_appliedSpeedScale = value12.As<double>();
		}
	}
}
