using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter10/GargantuarGD/Scene/TowerDefenseZombieGargantuarGD.cs")]
public class TowerDefenseZombieGargantuarGD : TowerDefenseZombieGargantuarBase
{
	public new class MethodName : TowerDefenseZombieGargantuarBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectChargeState = "ConnectChargeState";

		public static readonly StringName DisconnectChargeState = "DisconnectChargeState";

		public static readonly StringName UpdateChargeTrigger = "UpdateChargeTrigger";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public static readonly StringName ChargeEntered = "ChargeEntered";

		public static readonly StringName ChargeProcessing = "ChargeProcessing";

		public static readonly StringName ChargeExited = "ChargeExited";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName LimitChargeHurt = "LimitChargeHurt";

		public new static readonly StringName Hurt = "Hurt";

		public new static readonly StringName SkipInvincibleHurt = "SkipInvincibleHurt";

		public new static readonly StringName BowlingHurt = "BowlingHurt";

		public new static readonly StringName SmashHurt = "SmashHurt";

		public new static readonly StringName ExplodeHurt = "ExplodeHurt";

		public new static readonly StringName FlagHurt = "FlagHurt";

		public new static readonly StringName HurtWithAttackConfig = "HurtWithAttackConfig";

		public new static readonly StringName ProjectileHurt = "ProjectileHurt";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombieGargantuarBase.PropertyName
	{
		public static readonly StringName _chargeStateConnected = "_chargeStateConnected";

		public static readonly StringName _chargeArmed = "_chargeArmed";

		public static readonly StringName _charging = "_charging";

		public static readonly StringName _chargeEnding = "_chargeEnding";

		public static readonly StringName _chargeElapsed = "_chargeElapsed";

		public static readonly StringName _chargeRegenAccumulator = "_chargeRegenAccumulator";
	}

	public new class SignalName : TowerDefenseZombieGargantuarBase.SignalName
	{
	}

	private const string ChargeStateId = "zombie.gargantuar_gd.charge";

	private const string MagicClip = "Magic";

	private const string MagicIdleClip = "MagicIdle";

	private const string MagicSmashClip = "MagicSmash";

	private const double ChargeHpRatio = 0.5;

	private const double ChargeDuration = 5.0;

	private const double ChargeRegenPerSecond = 500.0;

	private const double ChargeDamageLimit = 100.0;

	private const double ChargeRegenTickInterval = 1.0;

	private const double ChargeTimerEpsilon = 1E-09;

	private StateHandle _chargeStateHandle;

	private bool _chargeStateConnected;

	private bool _chargeArmed;

	private bool _charging;

	private bool _chargeEnding;

	private double _chargeElapsed;

	private double _chargeRegenAccumulator;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			impThrowFlag = false;
			ConnectChargeState();
		}
	}

	public override void _ExitTree()
	{
		DisconnectChargeState();
		base._ExitTree();
	}

	private void ConnectChargeState()
	{
		if (_chargeStateConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_chargeStateHandle = StateMachine?.GetStateById("zombie.gargantuar_gd.charge");
			StateHandle chargeStateHandle = _chargeStateHandle;
			if (chargeStateHandle != null && chargeStateHandle.IsValid)
			{
				_chargeStateHandle.Entered += ChargeEntered;
				_chargeStateHandle.Exited += ChargeExited;
				_chargeStateHandle.PhysicsProcessing += ChargeProcessing;
				_chargeStateConnected = true;
			}
		}
	}

	private void DisconnectChargeState()
	{
		if (_chargeStateConnected)
		{
			if (_chargeStateHandle != null)
			{
				_chargeStateHandle.Entered -= ChargeEntered;
				_chargeStateHandle.Exited -= ChargeExited;
				_chargeStateHandle.PhysicsProcessing -= ChargeProcessing;
			}
			_chargeStateHandle = null;
			_chargeStateConnected = false;
		}
	}

	private void UpdateChargeTrigger(double delta)
	{
		if (_charging || die || nearDie || isDestroy || !GodotObject.IsInstanceValid(instance))
		{
			return;
		}
		double num = instance.hitpointsSave * 0.5;
		if (!_chargeArmed)
		{
			if (instance.hitpoints >= num)
			{
				_chargeArmed = true;
			}
		}
		else if (instance.hitpoints < num)
		{
			_chargeArmed = false;
			SendStateEvent("ToCharge");
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		UpdateChargeTrigger(delta);
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
		UpdateChargeTrigger(delta);
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		UpdateChargeTrigger(delta);
	}

	public virtual void ChargeEntered()
	{
		_charging = true;
		_chargeEnding = false;
		_chargeElapsed = 0.0;
		_chargeRegenAccumulator = 0.0;
		sprite.SetAnimation("Magic", loop: false, 0.1);
	}

	public virtual void ChargeProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		if (_charging && !_chargeEnding && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost) && GodotObject.IsInstanceValid(instance))
		{
			double num = Mathf.Clamp(delta, 0.0, 5.0 - _chargeElapsed);
			_chargeElapsed += num;
			_chargeRegenAccumulator += num;
			double hitpointsSave = instance.hitpointsSave;
			while (_chargeRegenAccumulator + 1E-09 >= 1.0 && instance.hitpoints < hitpointsSave)
			{
				_chargeRegenAccumulator = Mathf.Max(0.0, _chargeRegenAccumulator - 1.0);
				Health(Mathf.Min(500.0, hitpointsSave - instance.hitpoints));
			}
			if (instance.hitpoints >= hitpointsSave || _chargeElapsed + 1E-09 >= 5.0)
			{
				_chargeEnding = true;
				sprite.SetAnimation("MagicSmash", loop: false, 0.1);
			}
		}
	}

	public virtual void ChargeExited()
	{
		_charging = false;
		_chargeEnding = false;
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Magic" && _charging && !_chargeEnding)
		{
			sprite.SetAnimation("MagicIdle", loop: true, 0.1);
		}
		else if (clip == "MagicSmash" && _charging)
		{
			Walk();
		}
	}

	private double LimitChargeHurt(double num)
	{
		if (!_charging)
		{
			return num;
		}
		return Mathf.Min(num, 100.0);
	}

	public override double Hurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true, double damageLimit = -1.0)
	{
		return base.Hurt(LimitChargeHurt(num), playSplatAudio, velocity, createDamagePart, damageLimit);
	}

	public override double SkipInvincibleHurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true)
	{
		return base.SkipInvincibleHurt(LimitChargeHurt(num), playSplatAudio, velocity, createDamagePart);
	}

	public override double BowlingHurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool hitShield = true, bool createDamagePart = true)
	{
		return base.BowlingHurt(LimitChargeHurt(num), playSplatAudio, velocity, hitShield, createDamagePart);
	}

	public override double SmashHurt(double num, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		return base.SmashHurt(LimitChargeHurt(num), playSplatAudio, velocity);
	}

	public override double ExplodeHurt(double num, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND damageKind = TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		return base.ExplodeHurt(LimitChargeHurt(num), damageKind, playSplatAudio, velocity);
	}

	public override double FlagHurt(double num, int damageFlags, bool playSplatAudio = true, Vector2 velocity = default(Vector2))
	{
		return base.FlagHurt(LimitChargeHurt(num), damageFlags, playSplatAudio, velocity);
	}

	public override double HurtWithAttackConfig(AttackConfig attackConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool createDamagePart = true)
	{
		if (!_charging || !GodotObject.IsInstanceValid(attackConfig))
		{
			return base.HurtWithAttackConfig(attackConfig, playSplatAudio, velocity, createDamagePart);
		}
		double num = attackConfig.num;
		attackConfig.num = Mathf.Min(num, 100.0);
		try
		{
			return base.HurtWithAttackConfig(attackConfig, playSplatAudio, velocity, createDamagePart);
		}
		finally
		{
			attackConfig.num = num;
		}
	}

	public override double ProjectileHurt(TowerDefenseProjectile projectile, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false)
	{
		if (!_charging)
		{
			return base.ProjectileHurt(projectile, projectileConfig, playSplatAudio, velocity, isRange);
		}
		bool flag = GodotObject.IsInstanceValid(projectile);
		bool flag2 = GodotObject.IsInstanceValid(projectileConfig);
		double num = (flag ? projectile.damage : 0.0);
		double num2 = (flag2 ? projectileConfig.baseDamage : 0.0);
		if (flag)
		{
			projectile.damage = Mathf.Min(num, 100.0);
		}
		if (flag2)
		{
			projectileConfig.baseDamage = Mathf.Min(num2, 100.0);
		}
		try
		{
			return base.ProjectileHurt(projectile, projectileConfig, playSplatAudio, velocity, isRange);
		}
		finally
		{
			if (flag && GodotObject.IsInstanceValid(projectile))
			{
				projectile.damage = num;
			}
			if (flag2)
			{
				projectileConfig.baseDamage = num2;
			}
		}
	}

	public override double ProjectileHurt(in ProjectileHitInfo info, TowerDefenseProjectileConfig projectileConfig, bool playSplatAudio = true, Vector2 velocity = default(Vector2), bool isRange = false)
	{
		if (!_charging)
		{
			return base.ProjectileHurt(in info, projectileConfig, playSplatAudio, velocity, isRange);
		}
		ProjectileHitInfo info2 = info;
		info2.damage = Mathf.Min(info.damage, 100.0);
		return base.ProjectileHurt(in info2, projectileConfig, playSplatAudio, velocity, isRange);
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		if (command != smashAnimeEvent)
		{
			base.AnimeEvent(command, argument);
		}
		else if (_charging || (GodotObject.IsInstanceValid(sprite) && sprite.clip == "MagicSmash"))
		{
			gargantuarSmashComponent?.SmashAttack(1);
		}
		else
		{
			base.AnimeEvent(command, argument);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["chargeArmed"] = _chargeArmed;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		_chargeArmed = data.GetValueOrDefault("chargeArmed", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(24)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectChargeState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectChargeState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateChargeTrigger, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
			new MethodInfo(MethodName.ChargeEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ChargeProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChargeExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LimitChargeHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Hurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "damageLimit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SkipInvincibleHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BowlingHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hitShield", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDamagePart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SmashHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExplodeHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlagHurt, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "velocity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
				new PropertyInfo(Variant.Type.Bool, "isRange", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
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
		if (method == MethodName.ConnectChargeState && args.Count == 0)
		{
			ConnectChargeState();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectChargeState && args.Count == 0)
		{
			DisconnectChargeState();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateChargeTrigger && args.Count == 1)
		{
			UpdateChargeTrigger(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.ChargeEntered && args.Count == 0)
		{
			ChargeEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ChargeProcessing && args.Count == 1)
		{
			ChargeProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChargeExited && args.Count == 0)
		{
			ChargeExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LimitChargeHurt && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(LimitChargeHurt(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.Hurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(Hurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<double>(in args[4])));
			return true;
		}
		if (method == MethodName.SkipInvincibleHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(SkipInvincibleHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.BowlingHurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(BowlingHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.SmashHurt && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(SmashHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.ExplodeHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(ExplodeHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.FlagHurt && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(FlagHurt(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.HurtWithAttackConfig && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<double>(HurtWithAttackConfig(VariantUtils.ConvertTo<AttackConfig>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.ProjectileHurt && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<double>(ProjectileHurt(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseProjectileConfig>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
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
		if (method == MethodName.ConnectChargeState)
		{
			return true;
		}
		if (method == MethodName.DisconnectChargeState)
		{
			return true;
		}
		if (method == MethodName.UpdateChargeTrigger)
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
		if (method == MethodName.ChargeEntered)
		{
			return true;
		}
		if (method == MethodName.ChargeProcessing)
		{
			return true;
		}
		if (method == MethodName.ChargeExited)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.LimitChargeHurt)
		{
			return true;
		}
		if (method == MethodName.Hurt)
		{
			return true;
		}
		if (method == MethodName.SkipInvincibleHurt)
		{
			return true;
		}
		if (method == MethodName.BowlingHurt)
		{
			return true;
		}
		if (method == MethodName.SmashHurt)
		{
			return true;
		}
		if (method == MethodName.ExplodeHurt)
		{
			return true;
		}
		if (method == MethodName.FlagHurt)
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
		if (method == MethodName.AnimeEvent)
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
		if (name == PropertyName._chargeStateConnected)
		{
			_chargeStateConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._chargeArmed)
		{
			_chargeArmed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._charging)
		{
			_charging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._chargeEnding)
		{
			_chargeEnding = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._chargeElapsed)
		{
			_chargeElapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._chargeRegenAccumulator)
		{
			_chargeRegenAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._chargeStateConnected)
		{
			value = VariantUtils.CreateFrom(in _chargeStateConnected);
			return true;
		}
		if (name == PropertyName._chargeArmed)
		{
			value = VariantUtils.CreateFrom(in _chargeArmed);
			return true;
		}
		if (name == PropertyName._charging)
		{
			value = VariantUtils.CreateFrom(in _charging);
			return true;
		}
		if (name == PropertyName._chargeEnding)
		{
			value = VariantUtils.CreateFrom(in _chargeEnding);
			return true;
		}
		if (name == PropertyName._chargeElapsed)
		{
			value = VariantUtils.CreateFrom(in _chargeElapsed);
			return true;
		}
		if (name == PropertyName._chargeRegenAccumulator)
		{
			value = VariantUtils.CreateFrom(in _chargeRegenAccumulator);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._chargeStateConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._chargeArmed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._charging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._chargeEnding, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._chargeElapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._chargeRegenAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._chargeStateConnected, Variant.From(in _chargeStateConnected));
		info.AddProperty(PropertyName._chargeArmed, Variant.From(in _chargeArmed));
		info.AddProperty(PropertyName._charging, Variant.From(in _charging));
		info.AddProperty(PropertyName._chargeEnding, Variant.From(in _chargeEnding));
		info.AddProperty(PropertyName._chargeElapsed, Variant.From(in _chargeElapsed));
		info.AddProperty(PropertyName._chargeRegenAccumulator, Variant.From(in _chargeRegenAccumulator));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._chargeStateConnected, out var value))
		{
			_chargeStateConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._chargeArmed, out var value2))
		{
			_chargeArmed = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._charging, out var value3))
		{
			_charging = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._chargeEnding, out var value4))
		{
			_chargeEnding = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._chargeElapsed, out var value5))
		{
			_chargeElapsed = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._chargeRegenAccumulator, out var value6))
		{
			_chargeRegenAccumulator = value6.As<double>();
		}
	}
}
