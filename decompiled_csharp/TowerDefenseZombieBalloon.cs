using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Balloon/Scene/TowerDefenseZombieBalloon.cs")]
public class TowerDefenseZombieBalloon : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FlyEntered = "FlyEntered";

		public static readonly StringName FlyProcessing = "FlyProcessing";

		public static readonly StringName FlyExited = "FlyExited";

		public static readonly StringName PopEntered = "PopEntered";

		public static readonly StringName PopProcessing = "PopProcessing";

		public static readonly StringName PopExited = "PopExited";

		public static readonly StringName FlyAttackEntered = "FlyAttackEntered";

		public static readonly StringName FlyAttackProcessing = "FlyAttackProcessing";

		public static readonly StringName FlyAttackExited = "FlyAttackExited";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName Blow = "Blow";

		public static readonly StringName StartLogicalBlowTween = "StartLogicalBlowTween";

		public static readonly StringName CompleteLogicalBlowTween = "CompleteLogicalBlowTween";

		public static readonly StringName CancelLogicalBlowTween = "CancelLogicalBlowTween";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _logicalBlowTween = "_logicalBlowTween";

		public static readonly StringName pop = "pop";

		public static readonly StringName speed = "speed";

		public static readonly StringName audioPlay = "audioPlay";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private StateHandle _flyStateHandle;

	private StateHandle _flyAttackStateHandle;

	private StateHandle _popStateHandle;

	private bool _stateSignalsConnected;

	private Tween _logicalBlowTween;

	public bool pop;

	public double speed = 30.0;

	public bool audioPlay;

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
		_flyStateHandle = StateMachine?.GetStateById("zombie.balloon.fly");
		_flyAttackStateHandle = StateMachine?.GetStateById("zombie.balloon.fly_attack");
		_popStateHandle = StateMachine?.GetStateById("zombie.balloon.pop");
		StateHandle flyStateHandle = _flyStateHandle;
		if (flyStateHandle == null || !flyStateHandle.IsValid)
		{
			return;
		}
		StateHandle flyAttackStateHandle = _flyAttackStateHandle;
		if (flyAttackStateHandle != null && flyAttackStateHandle.IsValid)
		{
			StateHandle popStateHandle = _popStateHandle;
			if (popStateHandle != null && popStateHandle.IsValid)
			{
				_flyStateHandle.Entered += FlyEntered;
				_flyStateHandle.Exited += FlyExited;
				_flyStateHandle.PhysicsProcessing += FlyProcessing;
				_flyAttackStateHandle.Entered += FlyAttackEntered;
				_flyAttackStateHandle.Exited += FlyAttackExited;
				_flyAttackStateHandle.PhysicsProcessing += FlyAttackProcessing;
				_popStateHandle.Entered += PopEntered;
				_popStateHandle.Exited += PopExited;
				_popStateHandle.PhysicsProcessing += PopProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_flyStateHandle != null)
			{
				_flyStateHandle.Entered -= FlyEntered;
				_flyStateHandle.Exited -= FlyExited;
				_flyStateHandle.PhysicsProcessing -= FlyProcessing;
			}
			_flyStateHandle = null;
			if (_flyAttackStateHandle != null)
			{
				_flyAttackStateHandle.Entered -= FlyAttackEntered;
				_flyAttackStateHandle.Exited -= FlyAttackExited;
				_flyAttackStateHandle.PhysicsProcessing -= FlyAttackProcessing;
			}
			_flyAttackStateHandle = null;
			if (_popStateHandle != null)
			{
				_popStateHandle.Entered -= PopEntered;
				_popStateHandle.Exited -= PopExited;
				_popStateHandle.PhysicsProcessing -= PopProcessing;
			}
			_popStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		CancelLogicalBlowTween();
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

	public virtual void FlyEntered()
	{
		sprite.SetAnimation("Idle", loop: true, 0.2);
	}

	public virtual void FlyProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
		if (!sprite.pause)
		{
			globalPositionForPhysicsFrame.X -= (float)(speed * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)Scale.X * (double)((!sprite.playBack) ? 1 : (-1)));
			SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
		}
		if ((double)globalPositionForPhysicsFrame.X < TowerDefenseManager.Instance.GetMapGroundLeft() + 20.0)
		{
			instance.ArmorDelete("Balloon");
			return;
		}
		if (!audioPlay && (double)globalPositionForPhysicsFrame.X < groundRight)
		{
			AudioManager.Instance.AudioPlay("BalloonInflate");
			audioPlay = true;
		}
		if (!sprite.pause && attackComponent.CanAttack())
		{
			SendStateEvent("ToFlyAttack");
		}
	}

	public virtual void FlyExited()
	{
	}

	public virtual void PopEntered()
	{
		sprite.SetAnimation("Pop", loop: false, 0.2);
	}

	public virtual void PopProcessing(double delta)
	{
		sprite.timeScale = timeScale * 2.0;
	}

	public virtual void PopExited()
	{
	}

	public void FlyAttackEntered()
	{
		sprite.SetAnimation("FlyEat", loop: true, 0.2);
		ActivateAttackAfterStateDelay(_flyAttackStateHandle);
	}

	public virtual void FlyAttackProcessing(double delta)
	{
		if (!attackComponent.CanAttack())
		{
			SendStateEvent("ToFly");
		}
		else if (startAttack && !nearDie && !sprite.pause && sprite.timeScale > 0.0 && useAttackDps)
		{
			attackComponent.AttackDpsExecute(delta, ((TowerDefenseZombieConfig)config).attack);
		}
		sprite.timeScale = timeScale * 2.0;
	}

	public virtual void FlyAttackExited()
	{
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

	public override void DieEntered()
	{
		CancelLogicalBlowTween();
		base.DieEntered();
	}

	public override void Walk()
	{
		if (pop)
		{
			SendStateEvent("ToWalk");
		}
		else
		{
			SendStateEvent("ToFly");
		}
	}

	public override void Blow()
	{
		if (!pop)
		{
			HitBoxDestroy();
			StartLogicalBlowTween();
		}
	}

	private void StartLogicalBlowTween()
	{
		CancelLogicalBlowTween();
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		Vector2 targetPosition = logicalGlobalPosition + new Vector2(TowerDefenseManager.Instance.GetMapGridSize().Y * (float)TowerDefenseManager.Instance.GetMapGridNum().Y * 2f, 0f);
		Tween tween = CreateTween();
		_logicalBlowTween = tween;
		tween.TweenMethod(Callable.From<Vector2>(SetLogicalGlobalPosition), logicalGlobalPosition, targetPosition, 1.0);
		tween.TweenCallback(Callable.From(() =>
		{
			CompleteLogicalBlowTween(tween, targetPosition);
		}));
	}

	private void CompleteLogicalBlowTween(Tween tween, Vector2 targetPosition)
	{
		if (_logicalBlowTween == tween && !isDestroy)
		{
			_logicalBlowTween = null;
			SetLogicalGlobalPosition(targetPosition);
			Destroy();
		}
	}

	private void CancelLogicalBlowTween()
	{
		if (GodotObject.IsInstanceValid(_logicalBlowTween))
		{
			_logicalBlowTween.Kill();
		}
		_logicalBlowTween = null;
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Pop")
		{
			Walk();
		}
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Balloon")
		{
			pop = true;
			AudioManager.Instance.AudioPlay("BalloonPop");
			instance.collisionFlags = 1;
			instance.maskFlags = 9;
			if (!GetHasArmor("SpecialHelmet"))
			{
				instance.unUseBuffFlags = 0;
			}
			else
			{
				instance.armorOverrideUnUseBuffFlagSave = 0;
			}
			SendStateEvent("ToPop");
		}
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		if (damangePointName == "Head")
		{
			((ZombieBalloonSprite)sprite).head = false;
			DamagePartCreate("Head", ((ZombieBalloonSprite)sprite).propeller, default, keepSlotScale: true, default, fromSync: false, null, 0L);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["pop"] = pop,
			["speed"] = speed
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		pop = data.GetValueOrDefault("pop", false).AsBool();
		speed = data.GetValueOrDefault("speed", 30.0).AsDouble();
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
			new MethodInfo(MethodName.FlyEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlyProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlyExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlyAttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlyAttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlyAttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Blow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartLogicalBlowTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteLogicalBlowTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tween", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Tween"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "targetPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelLogicalBlowTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.FlyEntered && args.Count == 0)
		{
			FlyEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.FlyProcessing && args.Count == 1)
		{
			FlyProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlyExited && args.Count == 0)
		{
			FlyExited();
			ret = default;
			return true;
		}
		if (method == MethodName.PopEntered && args.Count == 0)
		{
			PopEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.PopProcessing && args.Count == 1)
		{
			PopProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopExited && args.Count == 0)
		{
			PopExited();
			ret = default;
			return true;
		}
		if (method == MethodName.FlyAttackEntered && args.Count == 0)
		{
			FlyAttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.FlyAttackProcessing && args.Count == 1)
		{
			FlyAttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlyAttackExited && args.Count == 0)
		{
			FlyAttackExited();
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
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.Blow && args.Count == 0)
		{
			Blow();
			ret = default;
			return true;
		}
		if (method == MethodName.StartLogicalBlowTween && args.Count == 0)
		{
			StartLogicalBlowTween();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteLogicalBlowTween && args.Count == 2)
		{
			CompleteLogicalBlowTween(VariantUtils.ConvertTo<Tween>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CancelLogicalBlowTween && args.Count == 0)
		{
			CancelLogicalBlowTween();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.FlyEntered)
		{
			return true;
		}
		if (method == MethodName.FlyProcessing)
		{
			return true;
		}
		if (method == MethodName.FlyExited)
		{
			return true;
		}
		if (method == MethodName.PopEntered)
		{
			return true;
		}
		if (method == MethodName.PopProcessing)
		{
			return true;
		}
		if (method == MethodName.PopExited)
		{
			return true;
		}
		if (method == MethodName.FlyAttackEntered)
		{
			return true;
		}
		if (method == MethodName.FlyAttackProcessing)
		{
			return true;
		}
		if (method == MethodName.FlyAttackExited)
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
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.Blow)
		{
			return true;
		}
		if (method == MethodName.StartLogicalBlowTween)
		{
			return true;
		}
		if (method == MethodName.CompleteLogicalBlowTween)
		{
			return true;
		}
		if (method == MethodName.CancelLogicalBlowTween)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
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
		if (name == PropertyName._logicalBlowTween)
		{
			_logicalBlowTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName.pop)
		{
			pop = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.speed)
		{
			speed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.audioPlay)
		{
			audioPlay = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._logicalBlowTween)
		{
			value = VariantUtils.CreateFrom(in _logicalBlowTween);
			return true;
		}
		if (name == PropertyName.pop)
		{
			value = VariantUtils.CreateFrom(in pop);
			return true;
		}
		if (name == PropertyName.speed)
		{
			value = VariantUtils.CreateFrom(in speed);
			return true;
		}
		if (name == PropertyName.audioPlay)
		{
			value = VariantUtils.CreateFrom(in audioPlay);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._logicalBlowTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pop, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.speed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.audioPlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._logicalBlowTween, Variant.From(in _logicalBlowTween));
		info.AddProperty(PropertyName.pop, Variant.From(in pop));
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName.audioPlay, Variant.From(in audioPlay));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value))
		{
			_stateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._logicalBlowTween, out var value2))
		{
			_logicalBlowTween = value2.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName.pop, out var value3))
		{
			pop = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.speed, out var value4))
		{
			speed = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.audioPlay, out var value5))
		{
			audioPlay = value5.As<bool>();
		}
	}
}
