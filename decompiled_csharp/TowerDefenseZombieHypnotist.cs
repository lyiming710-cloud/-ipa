using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter5/Hypnotist/Scene/TowerDefenseZombieHypnotist.cs")]
public class TowerDefenseZombieHypnotist : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";

		public new static readonly StringName HitpointsNearDie = "HitpointsNearDie";

		public new static readonly StringName HitpointsEmpty = "HitpointsEmpty";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName Spawn = "Spawn";

		public static readonly StringName DropEntered = "DropEntered";

		public static readonly StringName ApplyDroppingTargetability = "ApplyDroppingTargetability";

		public static readonly StringName ApplyLandedTargetability = "ApplyLandedTargetability";

		public static readonly StringName ApplyTargetability = "ApplyTargetability";

		public static readonly StringName ApplyCurrentTargetability = "ApplyCurrentTargetability";

		public static readonly StringName DropProcessing = "DropProcessing";

		public static readonly StringName DropExited = "DropExited";

		public static readonly StringName CancelDropPresentation = "CancelDropPresentation";

		public static readonly StringName GrabEntered = "GrabEntered";

		public static readonly StringName GrabProcessing = "GrabProcessing";

		public static readonly StringName GrabExited = "GrabExited";

		public static readonly StringName RiseEntered = "RiseEntered";

		public static readonly StringName RiseProcessing = "RiseProcessing";

		public static readonly StringName RiseExited = "RiseExited";

		public static readonly StringName CancelRisePresentation = "CancelRisePresentation";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName StealTargets = "StealTargets";

		public static readonly StringName CanStealPlant = "CanStealPlant";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName CanBlock = "CanBlock";

		public new static readonly StringName Block = "Block";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName singleEvent = "singleEvent";

		public static readonly StringName rangeEvent = "rangeEvent";

		public static readonly StringName waitGrab = "waitGrab";

		public static readonly StringName hasPlant = "hasPlant";

		public static readonly StringName _bungeeTarget = "_bungeeTarget";

		public static readonly StringName waitTimer = "waitTimer";

		public static readonly StringName canBlock = "canBlock";

		public static readonly StringName dropTween = "dropTween";

		public static readonly StringName grabOver = "grabOver";

		public static readonly StringName _targetDropTween = "_targetDropTween";

		public static readonly StringName _riseTween = "_riseTween";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName _preserveProgressStateOnNextWalk = "_preserveProgressStateOnNextWalk";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> singleEvent = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> rangeEvent = new Array<TowerDefenseCharacterEventBase>();

	public bool waitGrab;

	public bool hasPlant;

	private Sprite2D _bungeeTarget;

	public double waitTimer;

	public bool canBlock = true;

	public Tween dropTween;

	public bool grabOver;

	private Tween _targetDropTween;

	private Tween _riseTween;

	private StateHandle _dropStateHandle;

	private StateHandle _grabStateHandle;

	private StateHandle _riseStateHandle;

	private bool _roleStateSignalsConnected;

	private bool _preserveProgressStateOnNextWalk;

	private void ConnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine == null || !stateMachine.IsInitialized)
		{
			return;
		}
		_dropStateHandle = StateMachine?.GetStateById("zombie.hypnotist.drop");
		_grabStateHandle = StateMachine?.GetStateById("zombie.hypnotist.grab");
		_riseStateHandle = StateMachine?.GetStateById("zombie.hypnotist.rise");
		StateHandle dropStateHandle = _dropStateHandle;
		if (dropStateHandle == null || !dropStateHandle.IsValid)
		{
			return;
		}
		StateHandle grabStateHandle = _grabStateHandle;
		if (grabStateHandle != null && grabStateHandle.IsValid)
		{
			StateHandle riseStateHandle = _riseStateHandle;
			if (riseStateHandle != null && riseStateHandle.IsValid)
			{
				_dropStateHandle.Entered += DropEntered;
				_dropStateHandle.Exited += DropExited;
				_dropStateHandle.PhysicsProcessing += DropProcessing;
				_grabStateHandle.Entered += GrabEntered;
				_grabStateHandle.Exited += GrabExited;
				_grabStateHandle.PhysicsProcessing += GrabProcessing;
				_riseStateHandle.Entered += RiseEntered;
				_riseStateHandle.Exited += RiseExited;
				_riseStateHandle.PhysicsProcessing += RiseProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			if (_dropStateHandle != null)
			{
				_dropStateHandle.Entered -= DropEntered;
				_dropStateHandle.Exited -= DropExited;
				_dropStateHandle.PhysicsProcessing -= DropProcessing;
			}
			_dropStateHandle = null;
			if (_grabStateHandle != null)
			{
				_grabStateHandle.Entered -= GrabEntered;
				_grabStateHandle.Exited -= GrabExited;
				_grabStateHandle.PhysicsProcessing -= GrabProcessing;
			}
			_grabStateHandle = null;
			if (_riseStateHandle != null)
			{
				_riseStateHandle.Entered -= RiseEntered;
				_riseStateHandle.Exited -= RiseExited;
				_riseStateHandle.PhysicsProcessing -= RiseProcessing;
			}
			_riseStateHandle = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		CancelDropPresentation();
		CancelRisePresentation();
		DisconnectRoleStateSignals();
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_bungeeTarget = GetNode<Sprite2D>("%BungeeTarget");
			targetRegistrationComponent.canCarry = false;
			ApplyDroppingTargetability();
			AddToGroup("Bungi", persistent: true);
			if (TowerDefenseManager.Instance.IsGameRunning())
			{
				z = 600.0;
				isGround = false;
			}
			ConnectRoleStateSignals();
		}
	}

	public override void FinalizeProgressRestore()
	{
		string text = CurrentStateHandle?.StableId.ToString() ?? "";
		_preserveProgressStateOnNextWalk = text.StartsWith("zombie.hypnotist.");
		base.FinalizeProgressRestore();
		ApplyCurrentTargetability();
	}

	public override void HitpointsNearDie()
	{
		base.HitpointsNearDie();
		Destroy();
	}

	public override void HitpointsEmpty()
	{
		base.HitpointsEmpty();
		Destroy();
	}

	public override void Walk()
	{
		if (_preserveProgressStateOnNextWalk)
		{
			_preserveProgressStateOnNextWalk = false;
		}
		else
		{
			SendStateEvent("ToDrop");
		}
	}

	public override void IdleEntered()
	{
		base.IdleEntered();
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (!waitGrab)
		{
			return;
		}
		if (waitTimer < 10.0)
		{
			if (!sprite.pause)
			{
				waitTimer += delta * timeScale;
			}
			return;
		}
		foreach (string item in new List<string>(buff.buffDictionary.Keys))
		{
			if (item != "Hypnoses")
			{
				buff.DeleteBuff(item);
			}
		}
		SendStateEvent("ToGrab");
	}

	public override void Spawn()
	{
		z = 600.0;
		isGround = false;
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		List<Vector2I> list = new List<Vector2I>();
		foreach (Node item in GetTree().GetNodesInGroup("Bungi"))
		{
			if (item != this && ((TowerDefenseCharacter)item).gridPos != new Vector2I(-1, -1))
			{
				list.Add(((TowerDefenseCharacter)item).gridPos);
			}
		}
		List<TowerDefenseCharacter> list2 = new List<TowerDefenseCharacter>();
		foreach (Node item2 in GetTree().GetNodesInGroup("Plant"))
		{
			TowerDefenseCharacter towerDefenseCharacter = item2 as TowerDefenseCharacter;
			if (CanStealPlant(towerDefenseCharacter) && !list.Contains(towerDefenseCharacter.gridPos))
			{
				list2.Add(towerDefenseCharacter);
			}
		}
		if (list2.Count <= 0)
		{
			Destroy();
			gridPos = new Vector2I(GD.RandRange(0, TowerDefenseManager.Instance.GetMapGridNum().X), GD.RandRange(0, TowerDefenseManager.Instance.GetMapGridNum().Y));
		}
		else
		{
			TowerDefenseCharacter towerDefenseCharacter2 = list2[(int)(GD.Randi() % list2.Count)];
			SetLogicalGlobalPosition(TowerDefenseManager.GetMapCellPlantPos(towerDefenseCharacter2.gridPos));
			gridPos = towerDefenseCharacter2.gridPos;
		}
	}

	public void DropEntered()
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			Destroy();
			return;
		}
		ApplyDroppingTargetability();
		z = 600.0;
		isGround = false;
		_bungeeTarget.Visible = true;
		_targetDropTween = CreateTween();
		_targetDropTween.SetEase(Tween.EaseType.Out);
		_targetDropTween.SetTrans(Tween.TransitionType.Cubic);
		_targetDropTween.TweenProperty(_bungeeTarget, "position", new Vector2(0f, 15f - (float)cell.GetGroundHeight()), 0.5).From(new Vector2(0f, -585f));
		CompleteDropPresentationAsync(_dropStateHandle.ActivationGeneration);
	}

	private async Task CompleteDropPresentationAsync(ulong activationGeneration)
	{
		if (!(await WaitForStateDelayAsync(_dropStateHandle, 1.0)))
		{
			return;
		}
		AudioManager.Instance.AudioPlay("BungeeScream");
		if (await WaitForStateDelayAsync(_dropStateHandle, 1.0))
		{
			dropTween = CreateTween();
			dropTween.SetEase(Tween.EaseType.Out);
			dropTween.SetTrans(Tween.TransitionType.Cubic);
			dropTween.TweenProperty(this, "z", cell.GetGroundHeight(), 1.0);
			sprite.SetAnimation("Drop", loop: true, 0.2);
			if (await WaitForStateDelayAsync(_dropStateHandle, 1.0) && IsStateActivationCurrent(_dropStateHandle, activationGeneration))
			{
				isGround = true;
				groundHeight = cell.GetGroundHeight();
				z = groundHeight;
				waitGrab = true;
				ApplyLandedTargetability();
				Idle();
				TowerDefenseExplode.CreateExplode(GetLogicalGlobalPosition(), new Vector2(0.25f, 0.25f), singleEvent, new Array<TowerDefenseCharacter>(), camp, instance.collisionFlags);
				InvalidateHitBoxBounds();
			}
		}
	}

	private void ApplyDroppingTargetability()
	{
		ApplyTargetability(landed: false);
	}

	private void ApplyLandedTargetability()
	{
		ApplyTargetability(landed: true);
	}

	private void ApplyTargetability(bool landed)
	{
		SetHitBoxSuppressed(HitBoxSuppressionReason.Scripted, !landed);
		SetHitBoxMonitorSuppressed(HitBoxSuppressionReason.Scripted, !landed);
		instance.invincible = !landed;
		instance.canBeCollection = landed;
		targetRegistrationComponent.canProjectileCheck = landed;
		instance.maskFlags = (landed ? 1 : 0);
	}

	private void ApplyCurrentTargetability()
	{
		if (!IsNodeReady() || !GodotObject.IsInstanceValid(instance))
		{
			return;
		}
		TargetRegistrationComponent targetRegistrationComponent = base.targetRegistrationComponent;
		if (targetRegistrationComponent == null || targetRegistrationComponent.IsReleased)
		{
			return;
		}
		int num;
		if (isGround)
		{
			StateHandle grabStateHandle = _grabStateHandle;
			if (grabStateHandle == null || !grabStateHandle.IsActive)
			{
				StateHandle riseStateHandle = _riseStateHandle;
				num = ((riseStateHandle == null || !riseStateHandle.IsActive) ? 1 : 0);
				goto IL_0061;
			}
		}
		num = 0;
		goto IL_0061;
		IL_0061:
		bool landed = (byte)num != 0;
		ApplyTargetability(landed);
	}

	public void DropProcessing(double delta)
	{
		sprite.timeScale = timeScale * 2.0;
	}

	public void DropExited()
	{
		CancelDropPresentation();
	}

	private void CancelDropPresentation()
	{
		if (GodotObject.IsInstanceValid(_targetDropTween))
		{
			_targetDropTween.Kill();
		}
		if (GodotObject.IsInstanceValid(dropTween))
		{
			dropTween.Kill();
		}
		_targetDropTween = null;
		dropTween = null;
	}

	public void GrabEntered()
	{
		instance.unUseBuffFlags = -1;
		ApplyDroppingTargetability();
		sprite.SetAnimation("Grab", loop: false, 0.2);
	}

	public void GrabProcessing(double delta)
	{
		sprite.timeScale = timeScale * 2.0;
	}

	public void GrabExited()
	{
	}

	public void RiseEntered()
	{
		sprite.SetAnimation("Rise", loop: true, 0.2);
		isGround = false;
		ApplyDroppingTargetability();
		double duration = 1.5;
		if (!canBlock)
		{
			duration = 0.75;
		}
		_riseTween = CreateTween();
		_riseTween.SetParallel();
		_riseTween.SetEase(Tween.EaseType.Out);
		_riseTween.SetTrans(Tween.TransitionType.Cubic);
		_riseTween.TweenProperty(_bungeeTarget, "position", new Vector2(0f, -585f), duration);
		_riseTween.TweenProperty(this, "z", 600, duration);
		CompleteRisePresentationAsync(_riseStateHandle.ActivationGeneration, duration);
	}

	private async Task CompleteRisePresentationAsync(ulong activationGeneration, double duration)
	{
		if (await WaitForStateDelayAsync(_riseStateHandle, duration) && IsStateActivationCurrent(_riseStateHandle, activationGeneration))
		{
			Destroy();
		}
	}

	public void RiseProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void RiseExited()
	{
		CancelRisePresentation();
	}

	private void CancelRisePresentation()
	{
		if (GodotObject.IsInstanceValid(_riseTween))
		{
			_riseTween.Kill();
		}
		_riseTween = null;
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		if (command == "grab")
		{
			StealTargets();
		}
	}

	private void StealTargets()
	{
		if (!TowerDefenseManager.HasGameplayAuthority || grabOver)
		{
			return;
		}
		grabOver = true;
		AudioManager.Instance.AudioPlay("Floop");
		cell = TowerDefenseManager.GetMapCell(gridPos);
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>(cell.characterList);
		foreach (TowerDefenseCharacter item in list)
		{
			if (CanStealPlant(item))
			{
				hasPlant = true;
				if (item is TowerDefensePlant)
				{
					item.die = true;
				}
				item.Destroy(freeInstance: false);
				if (item is TowerDefenseZombie towerDefenseZombie)
				{
					towerDefenseZombie.spritePause = true;
				}
				else
				{
					item.sprite.pause = true;
				}
				item.shadowSprite.Visible = false;
				item.shadowSprite.Texture = null;
				item.DisableGameplayForPermanentEmbeddedVisual();
				item.Reparent(spriteGroup);
			}
		}
		if (hasPlant)
		{
			TowerDefenseExplode.CreateExplode(GetLogicalGlobalPosition(), new Vector2(1.25f, 1.25f), rangeEvent, new Array<TowerDefenseCharacter>(list), camp, instance.collisionFlags);
		}
	}

	private bool CanStealPlant(TowerDefenseCharacter target)
	{
		if (GodotObject.IsInstanceValid(target) && target is TowerDefensePlant && !(target is TowerDefensePlantBowlingBase) && !target.isDestroy && !target.die && GodotObject.IsInstanceValid(target.instance) && target.instance.canBeCollection && CanTarget(target))
		{
			return !target.IsHardControlImmune;
		}
		return false;
	}

	public override void AnimeCompleted(string clip)
	{
		if (clip == "Grab")
		{
			StealTargets();
			SendStateEvent("ToRise");
		}
	}

	public override bool CanBlock()
	{
		if (canBlock)
		{
			return z <= 50.0;
		}
		return false;
	}

	public override void Block(TowerDefenseCharacter target)
	{
		if (GodotObject.IsInstanceValid(dropTween))
		{
			dropTween.Kill();
		}
		canBlock = false;
		instance.invincible = true;
		SendStateEvent("ToRise");
		target.Hurt(100.0);
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "waitGrab", waitGrab },
			{ "hasPlant", hasPlant },
			{ "waitTimer", waitTimer },
			{ "canBlock", canBlock },
			{ "grabOver", grabOver }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		waitGrab = data.GetValueOrDefault("waitGrab", false).AsBool();
		hasPlant = data.GetValueOrDefault("hasPlant", false).AsBool();
		waitTimer = data.GetValueOrDefault("waitTimer", 0.0).AsDouble();
		canBlock = data.GetValueOrDefault("canBlock", true).AsBool();
		grabOver = data.GetValueOrDefault("grabOver", false).AsBool();
		if (IsNodeReady())
		{
			ApplyCurrentTargetability();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(34)
		{
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitpointsNearDie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Spawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DropEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyDroppingTargetability, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyLandedTargetability, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyTargetability, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "landed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCurrentTargetability, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DropProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DropExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelDropPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GrabEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GrabProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GrabExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RiseEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RiseProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RiseExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelRisePresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.StealTargets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanStealPlant, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanBlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Block, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.ConnectRoleStateSignals && args.Count == 0)
		{
			ConnectRoleStateSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals && args.Count == 0)
		{
			DisconnectRoleStateSignals();
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
		if (method == MethodName.FinalizeProgressRestore && args.Count == 0)
		{
			FinalizeProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.HitpointsNearDie && args.Count == 0)
		{
			HitpointsNearDie();
			ret = default;
			return true;
		}
		if (method == MethodName.HitpointsEmpty && args.Count == 0)
		{
			HitpointsEmpty();
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Spawn && args.Count == 0)
		{
			Spawn();
			ret = default;
			return true;
		}
		if (method == MethodName.DropEntered && args.Count == 0)
		{
			DropEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyDroppingTargetability && args.Count == 0)
		{
			ApplyDroppingTargetability();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyLandedTargetability && args.Count == 0)
		{
			ApplyLandedTargetability();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyTargetability && args.Count == 1)
		{
			ApplyTargetability(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCurrentTargetability && args.Count == 0)
		{
			ApplyCurrentTargetability();
			ret = default;
			return true;
		}
		if (method == MethodName.DropProcessing && args.Count == 1)
		{
			DropProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DropExited && args.Count == 0)
		{
			DropExited();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelDropPresentation && args.Count == 0)
		{
			CancelDropPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.GrabEntered && args.Count == 0)
		{
			GrabEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.GrabProcessing && args.Count == 1)
		{
			GrabProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GrabExited && args.Count == 0)
		{
			GrabExited();
			ret = default;
			return true;
		}
		if (method == MethodName.RiseEntered && args.Count == 0)
		{
			RiseEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.RiseProcessing && args.Count == 1)
		{
			RiseProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RiseExited && args.Count == 0)
		{
			RiseExited();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelRisePresentation && args.Count == 0)
		{
			CancelRisePresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.StealTargets && args.Count == 0)
		{
			StealTargets();
			ret = default;
			return true;
		}
		if (method == MethodName.CanStealPlant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanStealPlant(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanBlock && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanBlock());
			return true;
		}
		if (method == MethodName.Block && args.Count == 1)
		{
			Block(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.ConnectRoleStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectRoleStateSignals)
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
		if (method == MethodName.FinalizeProgressRestore)
		{
			return true;
		}
		if (method == MethodName.HitpointsNearDie)
		{
			return true;
		}
		if (method == MethodName.HitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.Spawn)
		{
			return true;
		}
		if (method == MethodName.DropEntered)
		{
			return true;
		}
		if (method == MethodName.ApplyDroppingTargetability)
		{
			return true;
		}
		if (method == MethodName.ApplyLandedTargetability)
		{
			return true;
		}
		if (method == MethodName.ApplyTargetability)
		{
			return true;
		}
		if (method == MethodName.ApplyCurrentTargetability)
		{
			return true;
		}
		if (method == MethodName.DropProcessing)
		{
			return true;
		}
		if (method == MethodName.DropExited)
		{
			return true;
		}
		if (method == MethodName.CancelDropPresentation)
		{
			return true;
		}
		if (method == MethodName.GrabEntered)
		{
			return true;
		}
		if (method == MethodName.GrabProcessing)
		{
			return true;
		}
		if (method == MethodName.GrabExited)
		{
			return true;
		}
		if (method == MethodName.RiseEntered)
		{
			return true;
		}
		if (method == MethodName.RiseProcessing)
		{
			return true;
		}
		if (method == MethodName.RiseExited)
		{
			return true;
		}
		if (method == MethodName.CancelRisePresentation)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.StealTargets)
		{
			return true;
		}
		if (method == MethodName.CanStealPlant)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.CanBlock)
		{
			return true;
		}
		if (method == MethodName.Block)
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
		if (name == PropertyName.singleEvent)
		{
			singleEvent = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.rangeEvent)
		{
			rangeEvent = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.waitGrab)
		{
			waitGrab = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.hasPlant)
		{
			hasPlant = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._bungeeTarget)
		{
			_bungeeTarget = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.waitTimer)
		{
			waitTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.canBlock)
		{
			canBlock = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.dropTween)
		{
			dropTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName.grabOver)
		{
			grabOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._targetDropTween)
		{
			_targetDropTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._riseTween)
		{
			_riseTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._preserveProgressStateOnNextWalk)
		{
			_preserveProgressStateOnNextWalk = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.singleEvent)
		{
			value = VariantUtils.CreateFromArray(singleEvent);
			return true;
		}
		if (name == PropertyName.rangeEvent)
		{
			value = VariantUtils.CreateFromArray(rangeEvent);
			return true;
		}
		if (name == PropertyName.waitGrab)
		{
			value = VariantUtils.CreateFrom(in waitGrab);
			return true;
		}
		if (name == PropertyName.hasPlant)
		{
			value = VariantUtils.CreateFrom(in hasPlant);
			return true;
		}
		if (name == PropertyName._bungeeTarget)
		{
			value = VariantUtils.CreateFrom(in _bungeeTarget);
			return true;
		}
		if (name == PropertyName.waitTimer)
		{
			value = VariantUtils.CreateFrom(in waitTimer);
			return true;
		}
		if (name == PropertyName.canBlock)
		{
			value = VariantUtils.CreateFrom(in canBlock);
			return true;
		}
		if (name == PropertyName.dropTween)
		{
			value = VariantUtils.CreateFrom(in dropTween);
			return true;
		}
		if (name == PropertyName.grabOver)
		{
			value = VariantUtils.CreateFrom(in grabOver);
			return true;
		}
		if (name == PropertyName._targetDropTween)
		{
			value = VariantUtils.CreateFrom(in _targetDropTween);
			return true;
		}
		if (name == PropertyName._riseTween)
		{
			value = VariantUtils.CreateFrom(in _riseTween);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName._preserveProgressStateOnNextWalk)
		{
			value = VariantUtils.CreateFrom(in _preserveProgressStateOnNextWalk);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.singleEvent, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.rangeEvent, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.waitGrab, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bungeeTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.waitTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canBlock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.dropTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.grabOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetDropTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._riseTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preserveProgressStateOnNextWalk, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.singleEvent, Variant.CreateFrom(singleEvent));
		info.AddProperty(PropertyName.rangeEvent, Variant.CreateFrom(rangeEvent));
		info.AddProperty(PropertyName.waitGrab, Variant.From(in waitGrab));
		info.AddProperty(PropertyName.hasPlant, Variant.From(in hasPlant));
		info.AddProperty(PropertyName._bungeeTarget, Variant.From(in _bungeeTarget));
		info.AddProperty(PropertyName.waitTimer, Variant.From(in waitTimer));
		info.AddProperty(PropertyName.canBlock, Variant.From(in canBlock));
		info.AddProperty(PropertyName.dropTween, Variant.From(in dropTween));
		info.AddProperty(PropertyName.grabOver, Variant.From(in grabOver));
		info.AddProperty(PropertyName._targetDropTween, Variant.From(in _targetDropTween));
		info.AddProperty(PropertyName._riseTween, Variant.From(in _riseTween));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName._preserveProgressStateOnNextWalk, Variant.From(in _preserveProgressStateOnNextWalk));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.singleEvent, out var value))
		{
			singleEvent = value.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.rangeEvent, out var value2))
		{
			rangeEvent = value2.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.waitGrab, out var value3))
		{
			waitGrab = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hasPlant, out var value4))
		{
			hasPlant = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._bungeeTarget, out var value5))
		{
			_bungeeTarget = value5.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.waitTimer, out var value6))
		{
			waitTimer = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.canBlock, out var value7))
		{
			canBlock = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dropTween, out var value8))
		{
			dropTween = value8.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName.grabOver, out var value9))
		{
			grabOver = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._targetDropTween, out var value10))
		{
			_targetDropTween = value10.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._riseTween, out var value11))
		{
			_riseTween = value11.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value12))
		{
			_roleStateSignalsConnected = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._preserveProgressStateOnNextWalk, out var value13))
		{
			_preserveProgressStateOnNextWalk = value13.As<bool>();
		}
	}
}
