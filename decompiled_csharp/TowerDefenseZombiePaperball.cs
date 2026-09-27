using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/Paperball/Scene/TowerDefenseZombiePaperball.cs")]
public class TowerDefenseZombiePaperball : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public static readonly StringName GaspEntered = "GaspEntered";

		public static readonly StringName GaspProcessing = "GaspProcessing";

		public static readonly StringName GaspExited = "GaspExited";

		public static readonly StringName RunEntered = "RunEntered";

		public static readonly StringName RunProcessing = "RunProcessing";

		public static readonly StringName RunExited = "RunExited";

		public static readonly StringName ImpactEntered = "ImpactEntered";

		public static readonly StringName ImpactProcessing = "ImpactProcessing";

		public static readonly StringName ImpactExited = "ImpactExited";

		public static readonly StringName RestEntered = "RestEntered";

		public static readonly StringName RestProcessing = "RestProcessing";

		public static readonly StringName RestExited = "RestExited";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName AttackEntered = "AttackEntered";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName angry = "angry";

		public static readonly StringName canRun = "canRun";

		public static readonly StringName isRun = "isRun";

		public static readonly StringName isGrap = "isGrap";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private StateHandle _gaspStateHandle;

	private StateHandle _runStateHandle;

	private StateHandle _impactStateHandle;

	private StateHandle _restStateHandle;

	private bool _stateSignalsConnected;

	private const string ZOMBIE_PAPER_MADHEAD = "uid://bbu3gjf3ww3e7";

	private static PackedScene _HAMMER_EXPLOSION;

	public bool angry;

	public bool canRun = true;

	public bool isRun;

	public bool isGrap;

	private static PackedScene HAMMER_EXPLOSION => _HAMMER_EXPLOSION ?? (_HAMMER_EXPLOSION = GD.Load<PackedScene>("res://Prefab/Particles/Explosion/Hammer/HammerExplosion.tscn"));

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine == null || !stateMachine.IsInitialized)
		{
			return;
		}
		_gaspStateHandle = StateMachine?.GetStateById("zombie.paperball.gasp");
		_runStateHandle = StateMachine?.GetStateById("zombie.paperball.run");
		_impactStateHandle = StateMachine?.GetStateById("zombie.paperball.impact");
		_restStateHandle = StateMachine?.GetStateById("zombie.paperball.rest");
		StateHandle gaspStateHandle = _gaspStateHandle;
		if (gaspStateHandle == null || !gaspStateHandle.IsValid)
		{
			return;
		}
		StateHandle runStateHandle = _runStateHandle;
		if (runStateHandle == null || !runStateHandle.IsValid)
		{
			return;
		}
		StateHandle impactStateHandle = _impactStateHandle;
		if (impactStateHandle != null && impactStateHandle.IsValid)
		{
			StateHandle restStateHandle = _restStateHandle;
			if (restStateHandle != null && restStateHandle.IsValid)
			{
				_gaspStateHandle.Entered += GaspEntered;
				_gaspStateHandle.Exited += GaspExited;
				_gaspStateHandle.PhysicsProcessing += GaspProcessing;
				_runStateHandle.Entered += RunEntered;
				_runStateHandle.Exited += RunExited;
				_runStateHandle.PhysicsProcessing += RunProcessing;
				_impactStateHandle.Entered += ImpactEntered;
				_impactStateHandle.Exited += ImpactExited;
				_impactStateHandle.PhysicsProcessing += ImpactProcessing;
				_restStateHandle.Entered += RestEntered;
				_restStateHandle.Exited += RestExited;
				_restStateHandle.PhysicsProcessing += RestProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_gaspStateHandle != null)
			{
				_gaspStateHandle.Entered -= GaspEntered;
				_gaspStateHandle.Exited -= GaspExited;
				_gaspStateHandle.PhysicsProcessing -= GaspProcessing;
			}
			_gaspStateHandle = null;
			if (_runStateHandle != null)
			{
				_runStateHandle.Entered -= RunEntered;
				_runStateHandle.Exited -= RunExited;
				_runStateHandle.PhysicsProcessing -= RunProcessing;
			}
			_runStateHandle = null;
			if (_impactStateHandle != null)
			{
				_impactStateHandle.Entered -= ImpactEntered;
				_impactStateHandle.Exited -= ImpactExited;
				_impactStateHandle.PhysicsProcessing -= ImpactProcessing;
			}
			_impactStateHandle = null;
			if (_restStateHandle != null)
			{
				_restStateHandle.Entered -= RestEntered;
				_restStateHandle.Exited -= RestExited;
				_restStateHandle.PhysicsProcessing -= RestProcessing;
			}
			_restStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			ConnectStateSignals();
		}
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public void GaspEntered()
	{
		isGrap = true;
		sprite.SetAnimation("Gasp", loop: false, 0.1);
	}

	public void GaspProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public void GaspExited()
	{
		isGrap = false;
	}

	public void RunEntered()
	{
		isRun = true;
		sprite.SetAnimation("AngryRun");
		ActivateGroundMovementAfterStateDelay(_runStateHandle);
	}

	public void RunProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		if (attackComponent.CanAttackIgnoringLadderFilter())
		{
			SendStateEvent("ToImpact");
		}
	}

	public void RunExited()
	{
		isRun = false;
		groundMoveComponent.SetAlive(false);
	}

	public void ImpactEntered()
	{
		sprite.SetAnimation("Impact", loop: false);
	}

	public void ImpactProcessing(double delta)
	{
		sprite.timeScale = timeScale * 0.25;
	}

	public void ImpactExited()
	{
	}

	public void RestEntered()
	{
		sprite.SetAnimation("Rest", loop: false);
		sprite.AddAnimation("Up", 0.0, loop: false);
	}

	public void RestProcessing(double delta)
	{
		sprite.timeScale = timeScale * 0.5;
	}

	public void RestExited()
	{
	}

	public override void WalkEntered()
	{
		if (!angry)
		{
			sprite.SetAnimation(walkAnimeClip, loop: true, 0.2);
		}
		else
		{
			if (canRun)
			{
				SendStateEvent("ToRun");
				canRun = false;
				return;
			}
			sprite.SetAnimation("AngryWalk");
		}
		ActivateGroundMovementAfterStateDelay(WalkStateHandle);
	}

	public override void AttackEntered()
	{
		if (!angry)
		{
			sprite.SetAnimation(attackAnimeClip, loop: true, 0.2);
		}
		else
		{
			sprite.SetAnimation("AngryEat", loop: true, 0.2);
		}
		ActivateAttackAfterStateDelay(AttackStateHandle);
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (!(armorName == "Paper"))
		{
			if (armorName == "Helmet")
			{
				if (isRun)
				{
					Walk();
				}
				canRun = false;
			}
		}
		else
		{
			SendStateEvent("ToGasp");
			AudioManager.Instance.AudioPlay("NewspaperRip");
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		switch (clip)
		{
		case "Gasp":
			AudioManager.Instance.AudioPlay("NewspaperRarrgh");
			sprite.SetAtlasReplace("Zombie_head.png", "uid://bbu3gjf3ww3e7");
			timeScaleInit = 3.0;
			angry = true;
			isGrap = false;
			Walk();
			break;
		case "Impact":
			SendStateEvent("ToRest");
			break;
		case "Up":
			Walk();
			break;
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (!(command == "impact") || !attackComponent.CanAttack())
		{
			return;
		}
		AudioManager.Instance.AudioPlay("Bonk");
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(HAMMER_EXPLOSION, gridPos);
		towerDefenseEffectParticlesOnce.GlobalPosition = logicalGlobalPosition;
		TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		TowerDefenseCharacter target = attackComponent.target;
		if (GodotObject.IsInstanceValid(target))
		{
			target.AttackDeal(this, attackComponent.attackType, 500.0);
			if (GodotObject.IsInstanceValid(target.cell))
			{
				target.cell.AttackDeal(this, attackComponent.attackType, 500.0);
			}
			target.Hurt(500.0, playSplatAudio: true, Vector2.Zero);
		}
		Tween tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.SetProcessMode(Tween.TweenProcessMode.Physics);
		tween.TweenMethod(Callable.From<Vector2>(SetLogicalGlobalPosition), logicalGlobalPosition, logicalGlobalPosition + new Vector2(50f * Scale.X, 0f), 0.1);
	}

	public override void Walk()
	{
		if (!isGrap)
		{
			base.Walk();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "angry", angry },
			{ "canRun", canRun },
			{ "isRun", isRun },
			{ "isGrap", isGrap }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		angry = data.GetValueOrDefault("angry", false).AsBool();
		canRun = data.GetValueOrDefault("canRun", true).AsBool();
		isRun = data.GetValueOrDefault("isRun", false).AsBool();
		isGrap = data.GetValueOrDefault("isGrap", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(26)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GaspEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GaspProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GaspExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImpactEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImpactProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImpactExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
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
		if (method == MethodName.GaspEntered && args.Count == 0)
		{
			GaspEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.GaspProcessing && args.Count == 1)
		{
			GaspProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GaspExited && args.Count == 0)
		{
			GaspExited();
			ret = default;
			return true;
		}
		if (method == MethodName.RunEntered && args.Count == 0)
		{
			RunEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.RunProcessing && args.Count == 1)
		{
			RunProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunExited && args.Count == 0)
		{
			RunExited();
			ret = default;
			return true;
		}
		if (method == MethodName.ImpactEntered && args.Count == 0)
		{
			ImpactEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ImpactProcessing && args.Count == 1)
		{
			ImpactProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ImpactExited && args.Count == 0)
		{
			ImpactExited();
			ret = default;
			return true;
		}
		if (method == MethodName.RestEntered && args.Count == 0)
		{
			RestEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.RestProcessing && args.Count == 1)
		{
			RestProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestExited && args.Count == 0)
		{
			RestExited();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkEntered && args.Count == 0)
		{
			WalkEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackEntered && args.Count == 0)
		{
			AttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
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
		if (method == MethodName.ConnectStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectStateSignals)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Ready)
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
		if (method == MethodName.GaspEntered)
		{
			return true;
		}
		if (method == MethodName.GaspProcessing)
		{
			return true;
		}
		if (method == MethodName.GaspExited)
		{
			return true;
		}
		if (method == MethodName.RunEntered)
		{
			return true;
		}
		if (method == MethodName.RunProcessing)
		{
			return true;
		}
		if (method == MethodName.RunExited)
		{
			return true;
		}
		if (method == MethodName.ImpactEntered)
		{
			return true;
		}
		if (method == MethodName.ImpactProcessing)
		{
			return true;
		}
		if (method == MethodName.ImpactExited)
		{
			return true;
		}
		if (method == MethodName.RestEntered)
		{
			return true;
		}
		if (method == MethodName.RestProcessing)
		{
			return true;
		}
		if (method == MethodName.RestExited)
		{
			return true;
		}
		if (method == MethodName.WalkEntered)
		{
			return true;
		}
		if (method == MethodName.AttackEntered)
		{
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.Walk)
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
		if (name == PropertyName.angry)
		{
			angry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.canRun)
		{
			canRun = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isRun)
		{
			isRun = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isGrap)
		{
			isGrap = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.angry)
		{
			value = VariantUtils.CreateFrom(in angry);
			return true;
		}
		if (name == PropertyName.canRun)
		{
			value = VariantUtils.CreateFrom(in canRun);
			return true;
		}
		if (name == PropertyName.isRun)
		{
			value = VariantUtils.CreateFrom(in isRun);
			return true;
		}
		if (name == PropertyName.isGrap)
		{
			value = VariantUtils.CreateFrom(in isGrap);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.angry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canRun, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isRun, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isGrap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName.angry, Variant.From(in angry));
		info.AddProperty(PropertyName.canRun, Variant.From(in canRun));
		info.AddProperty(PropertyName.isRun, Variant.From(in isRun));
		info.AddProperty(PropertyName.isGrap, Variant.From(in isGrap));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value))
		{
			_stateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.angry, out var value2))
		{
			angry = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.canRun, out var value3))
		{
			canRun = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isRun, out var value4))
		{
			isRun = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isGrap, out var value5))
		{
			isGrap = value5.As<bool>();
		}
	}
}
