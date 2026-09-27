using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter9/BungiSP/Scene/TowerDefenseZombieBungiSP.cs")]
public class TowerDefenseZombieBungiSP : TowerDefenseZombie, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpawnState = "ExportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public static readonly StringName ExportBungeeNetworkState = "ExportBungeeNetworkState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName IsNetworkSpecialMovementActive = "IsNetworkSpecialMovementActive";

		public new static readonly StringName HitpointsNearDie = "HitpointsNearDie";

		public new static readonly StringName HitpointsEmpty = "HitpointsEmpty";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName Spawn = "Spawn";

		public static readonly StringName DropEntered = "DropEntered";

		public static readonly StringName SpawnTarget = "SpawnTarget";

		public static readonly StringName DropProcessing = "DropProcessing";

		public static readonly StringName DropExited = "DropExited";

		public static readonly StringName ApplyTargetability = "ApplyTargetability";

		public static readonly StringName ApplyCurrentTargetability = "ApplyCurrentTargetability";

		public static readonly StringName GrabEntered = "GrabEntered";

		public static readonly StringName GrabProcessing = "GrabProcessing";

		public static readonly StringName GrabExited = "GrabExited";

		public static readonly StringName RiseEntered = "RiseEntered";

		public static readonly StringName RiseProcessing = "RiseProcessing";

		public static readonly StringName RiseExited = "RiseExited";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName StealAllPlants = "StealAllPlants";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName CanBlock = "CanBlock";

		public new static readonly StringName Block = "Block";

		public static readonly StringName MarkBungeeStateDirty = "MarkBungeeStateDirty";

		public static readonly StringName NextBungeeActionSequence = "NextBungeeActionSequence";

		public static readonly StringName ApplyRemoteActionAudio = "ApplyRemoteActionAudio";

		public static readonly StringName PlayFloopAudioOnce = "PlayFloopAudioOnce";

		public static readonly StringName IsDropPresentationCurrent = "IsDropPresentationCurrent";

		public static readonly StringName CancelDropPresentation = "CancelDropPresentation";

		public static readonly StringName IsRisePresentationCurrent = "IsRisePresentationCurrent";

		public static readonly StringName CancelRisePresentation = "CancelRisePresentation";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName waitGrab = "waitGrab";

		public static readonly StringName hasPlant = "hasPlant";

		public static readonly StringName waitTimer = "waitTimer";

		public static readonly StringName canBlock = "canBlock";

		public static readonly StringName dropTween = "dropTween";

		public static readonly StringName grabOver = "grabOver";

		public static readonly StringName suppressBungeeTarget = "suppressBungeeTarget";

		public static readonly StringName _target = "_target";

		public static readonly StringName _dropPresentationGeneration = "_dropPresentationGeneration";

		public static readonly StringName _risePresentationGeneration = "_risePresentationGeneration";

		public static readonly StringName _riseTween = "_riseTween";

		public static readonly StringName _networkSpecialStateRevision = "_networkSpecialStateRevision";

		public static readonly StringName _hasImportedNetworkState = "_hasImportedNetworkState";

		public static readonly StringName _dropActionSequence = "_dropActionSequence";

		public static readonly StringName _grabActionSequence = "_grabActionSequence";

		public static readonly StringName _remoteDropActionSequence = "_remoteDropActionSequence";

		public static readonly StringName _remoteGrabActionSequence = "_remoteGrabActionSequence";

		public static readonly StringName _remoteDropAudioGeneration = "_remoteDropAudioGeneration";

		public static readonly StringName _remoteActionStateInitialized = "_remoteActionStateInitialized";

		public static readonly StringName _floopAudioPlayed = "_floopAudioPlayed";

		public static readonly StringName _preserveProgressStateOnNextWalk = "_preserveProgressStateOnNextWalk";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private StateHandle _dropStateHandle;

	private StateHandle _grabStateHandle;

	private StateHandle _riseStateHandle;

	private bool _stateSignalsConnected;

	public bool waitGrab;

	public bool hasPlant;

	public double waitTimer;

	public bool canBlock = true;

	public Tween dropTween;

	public bool grabOver;

	public bool suppressBungeeTarget;

	private TowerDefenseBungiTargetSP _target;

	private int _dropPresentationGeneration;

	private int _risePresentationGeneration;

	private Tween _riseTween;

	private int _networkSpecialStateRevision = 1;

	private bool _hasImportedNetworkState;

	private int _dropActionSequence;

	private int _grabActionSequence;

	private int _remoteDropActionSequence;

	private int _remoteGrabActionSequence;

	private int _remoteDropAudioGeneration;

	private bool _remoteActionStateInitialized;

	private bool _floopAudioPlayed;

	private bool _preserveProgressStateOnNextWalk;

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
		_dropStateHandle = StateMachine?.GetStateById("zombie.bungi_sp.drop");
		_grabStateHandle = StateMachine?.GetStateById("zombie.bungi_sp.grab");
		_riseStateHandle = StateMachine?.GetStateById("zombie.bungi_sp.rise");
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
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_dropStateHandle != null)
			{
				_dropStateHandle.Entered -= DropEntered;
				_dropStateHandle.Exited -= DropExited;
				_dropStateHandle.PhysicsProcessing -= DropProcessing;
			}
			if (_grabStateHandle != null)
			{
				_grabStateHandle.Entered -= GrabEntered;
				_grabStateHandle.Exited -= GrabExited;
				_grabStateHandle.PhysicsProcessing -= GrabProcessing;
			}
			if (_riseStateHandle != null)
			{
				_riseStateHandle.Entered -= RiseEntered;
				_riseStateHandle.Exited -= RiseExited;
				_riseStateHandle.PhysicsProcessing -= RiseProcessing;
			}
			_dropStateHandle = null;
			_grabStateHandle = null;
			_riseStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		_remoteDropAudioGeneration++;
		CancelDropPresentation();
		CancelRisePresentation();
		DisconnectStateSignals();
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			targetRegistrationComponent.canCarry = false;
			AddToGroup("Bungi", persistent: true);
			if (TowerDefenseManager.Instance.IsGameRunning() && !_hasImportedNetworkState)
			{
				z = 600.0;
				isGround = false;
			}
			ConnectStateSignals();
			ApplyCurrentTargetability();
		}
	}

	public override void FinalizeProgressRestore()
	{
		string text = CurrentStateHandle?.StableId.ToString() ?? "";
		_preserveProgressStateOnNextWalk = text.StartsWith("zombie.bungi_sp.");
		base.FinalizeProgressRestore();
		ApplyCurrentTargetability();
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		if (data == null)
		{
			return;
		}
		_hasImportedNetworkState = _hasImportedNetworkState || data.ContainsKey("z");
		int num = data.GetValueOrDefault("rev", _networkSpecialStateRevision).AsInt32();
		if (TowerDefenseManager.HasGameplayAuthority || num >= _networkSpecialStateRevision)
		{
			_networkSpecialStateRevision = Mathf.Max(_networkSpecialStateRevision, num);
			suppressBungeeTarget = data.GetValueOrDefault("suppressBungeeTarget", suppressBungeeTarget).AsBool();
			canBlock = data.GetValueOrDefault("canBlock", canBlock).AsBool();
			waitGrab = data.GetValueOrDefault("waitGrab", waitGrab).AsBool();
			hasPlant = data.GetValueOrDefault("hasPlant", hasPlant).AsBool();
			grabOver = data.GetValueOrDefault("grabOver", grabOver).AsBool();
			waitTimer = data.GetValueOrDefault("waitTimer", waitTimer).AsDouble();
			groundHeight = data.GetValueOrDefault("groundHeight", groundHeight).AsDouble();
			isGround = data.GetValueOrDefault("isGround", isGround).AsBool();
			int num2 = data.GetValueOrDefault("dropActionSequence", _dropActionSequence).AsInt32();
			int num3 = data.GetValueOrDefault("grabActionSequence", _grabActionSequence).AsInt32();
			if (!TowerDefenseManager.HasGameplayAuthority)
			{
				ApplyRemoteActionAudio(num2, num3);
				CancelDropPresentation();
				CancelRisePresentation();
				z = data.GetValueOrDefault("z", z).AsDouble();
			}
			_dropActionSequence = Mathf.Max(_dropActionSequence, num2);
			_grabActionSequence = Mathf.Max(_grabActionSequence, num3);
			if (IsNodeReady())
			{
				ApplyCurrentTargetability();
			}
		}
	}

	public override Dictionary ExportNetworkSpawnState()
	{
		return ExportBungeeNetworkState();
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		return ExportBungeeNetworkState();
	}

	private Dictionary ExportBungeeNetworkState()
	{
		return new Dictionary
		{
			["rev"] = _networkSpecialStateRevision,
			["suppressBungeeTarget"] = suppressBungeeTarget,
			["canBlock"] = canBlock,
			["waitGrab"] = waitGrab,
			["hasPlant"] = hasPlant,
			["grabOver"] = grabOver,
			["waitTimer"] = waitTimer,
			["dropActionSequence"] = _dropActionSequence,
			["grabActionSequence"] = _grabActionSequence,
			["z"] = z,
			["groundHeight"] = groundHeight,
			["isGround"] = isGround
		};
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		ImportNetworkSpawnState(data);
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return _networkSpecialStateRevision;
	}

	public override bool IsNetworkSpecialMovementActive()
	{
		return true;
	}

	public override void HitpointsNearDie()
	{
		base.HitpointsNearDie();
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			Destroy();
		}
	}

	public override void HitpointsEmpty()
	{
		base.HitpointsEmpty();
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			Destroy();
		}
	}

	public override void Walk()
	{
		ActivateGameplayProcessing();
		if (_preserveProgressStateOnNextWalk)
		{
			_preserveProgressStateOnNextWalk = false;
		}
		else if (TowerDefenseManager.HasGameplayAuthority)
		{
			MarkBungeeStateDirty();
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
		if (!TowerDefenseManager.HasGameplayAuthority || !waitGrab)
		{
			return;
		}
		if (waitTimer < 3.0)
		{
			if (!sprite.pause)
			{
				waitTimer += delta * timeScale;
			}
			return;
		}
		waitGrab = false;
		foreach (string item in new List<string>(buff.buffDictionary.Keys))
		{
			if (item != "Hypnoses")
			{
				buff.DeleteBuff(item);
			}
		}
		MarkBungeeStateDirty();
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
		foreach (TowerDefenseCharacter cleanCharacters in TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList())
		{
			if (!(cleanCharacters is TowerDefensePlant) || cleanCharacters.isDestroy || cleanCharacters.die || !CanTarget(cleanCharacters) || cleanCharacters is TowerDefensePlantBowlingBase || !cleanCharacters.instance.canBeCollection || list.Contains(cleanCharacters.gridPos))
			{
				continue;
			}
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(cleanCharacters.gridPos);
			bool flag = false;
			foreach (TowerDefenseCharacter character in mapCell.GetCharacterList())
			{
				if (character is TowerDefenseBungiTargetSP)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list2.Add(cleanCharacters);
			}
		}
		if (list2.Count <= 0)
		{
			Destroy();
			gridPos = new Vector2I(GD.RandRange(0, TowerDefenseManager.Instance.GetMapGridNum().X), GD.RandRange(0, TowerDefenseManager.Instance.GetMapGridNum().Y));
		}
		else
		{
			TowerDefenseCharacter towerDefenseCharacter = list2[GD.RandRange(0, list2.Count - 1)];
			SetLogicalGlobalPosition(TowerDefenseManager.GetMapCellPlantPos(towerDefenseCharacter.gridPos));
			gridPos = towerDefenseCharacter.gridPos;
		}
	}

	public void DropEntered()
	{
		int presentationGeneration = ++_dropPresentationGeneration;
		if (TowerDefenseManager.HasGameplayAuthority && !IsProgressRestoreInFlight)
		{
			_dropActionSequence = NextBungeeActionSequence(_dropActionSequence);
			MarkBungeeStateDirty();
		}
		if (!GodotObject.IsInstanceValid(cell))
		{
			if (TowerDefenseManager.HasGameplayAuthority)
			{
				Destroy();
			}
		}
		else
		{
			ApplyTargetability(landed: false);
			z = 600.0;
			isGround = false;
			SpawnTarget();
			CompleteDropPresentationAsync(presentationGeneration);
		}
	}

	private async Task CompleteDropPresentationAsync(int presentationGeneration)
	{
		if (!(await WaitForStateDelayAsync(_dropStateHandle, 1.0)) || !IsDropPresentationCurrent(presentationGeneration))
		{
			return;
		}
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			AudioManager.Instance.AudioPlay("BungeeScream");
		}
		if (!(await WaitForStateDelayAsync(_dropStateHandle, 1.0)) || !IsDropPresentationCurrent(presentationGeneration) || !GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		dropTween = CreateTween();
		dropTween.SetEase(Tween.EaseType.Out);
		dropTween.SetTrans(Tween.TransitionType.Cubic);
		dropTween.TweenProperty(this, "z", cell.GetGroundHeight(), 1.0);
		sprite.SetAnimation("Drop", loop: true, 0.2);
		if (await WaitForStateDelayAsync(_dropStateHandle, 1.0) && IsDropPresentationCurrent(presentationGeneration))
		{
			isGround = true;
			groundHeight = cell.GetGroundHeight();
			z = groundHeight;
			waitGrab = true;
			ApplyTargetability(landed: true);
			if (TowerDefenseManager.HasGameplayAuthority)
			{
				MarkBungeeStateDirty();
				Idle();
			}
		}
	}

	private void SpawnTarget()
	{
		if (suppressBungeeTarget || !TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("BungiTargetSP");
		if (GodotObject.IsInstanceValid(packetConfig))
		{
			TowerDefenseCharacter towerDefenseCharacter = (EconomyOwnerAccountId.IsValid ? packetConfig.Plant(EconomyOwnerAccountId, gridPos, playAudio: false) : packetConfig.Plant(gridPos, playAudio: false));
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				towerDefenseCharacter.camp = camp;
				_target = towerDefenseCharacter as TowerDefenseBungiTargetSP;
				TowerDefenseManager.PublishSpawnedCharacter("BungiTargetSP", towerDefenseCharacter, useCreate: false, 0.0, walkAfterSpawn: false, "", new Dictionary { ["plant_play_audio"] = false });
			}
		}
	}

	public void DropProcessing(double delta)
	{
		sprite.timeScale = timeScale * 2.0;
	}

	public void DropExited()
	{
		CancelDropPresentation();
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

	public void GrabEntered()
	{
		MarkBungeeStateDirty();
		instance.unUseBuffFlags = -9;
		ApplyTargetability(landed: false);
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
		int presentationGeneration = ++_risePresentationGeneration;
		MarkBungeeStateDirty();
		sprite.SetAnimation("Rise", loop: true, 0.2);
		isGround = false;
		ApplyTargetability(landed: false);
		double duration = 1.5;
		if (!canBlock)
		{
			duration = 0.75;
		}
		_riseTween = CreateTween();
		_riseTween.SetEase(Tween.EaseType.Out);
		_riseTween.SetTrans(Tween.TransitionType.Cubic);
		_riseTween.TweenProperty(this, "z", 600, duration);
		CompleteRisePresentationAsync(presentationGeneration, duration);
	}

	private async Task CompleteRisePresentationAsync(int presentationGeneration, double duration)
	{
		if (await WaitForStateDelayAsync(_riseStateHandle, duration) && IsRisePresentationCurrent(presentationGeneration) && TowerDefenseManager.HasGameplayAuthority)
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

	public override void AnimeEvent(string command, Variant argument)
	{
		if (command == "grab")
		{
			StealAllPlants();
		}
	}

	private void StealAllPlants()
	{
		PlayFloopAudioOnce();
		if (!TowerDefenseManager.HasGameplayAuthority || grabOver)
		{
			return;
		}
		grabOver = true;
		_grabActionSequence = NextBungeeActionSequence(_grabActionSequence);
		MarkBungeeStateDirty();
		cell = TowerDefenseManager.GetMapCell(gridPos);
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		foreach (TowerDefenseCharacter item in new List<TowerDefenseCharacter>(cell.characterList))
		{
			if (GodotObject.IsInstanceValid(item) && !item.isDestroy && !item.die && !(item is TowerDefenseGravestone) && !(item is TowerDefenseCrater) && item.camp != camp && item is TowerDefensePlant && !(item is TowerDefensePlantBowlingBase))
			{
				hasPlant = true;
				item.die = true;
				item.Destroy(freeInstance: false);
				if (GodotObject.IsInstanceValid(item.sprite))
				{
					item.sprite.pause = true;
				}
				if (GodotObject.IsInstanceValid(item.shadowSprite))
				{
					item.shadowSprite.Visible = false;
					item.shadowSprite.Texture = null;
				}
				item.DisableGameplayForPermanentEmbeddedVisual();
				item.Reparent(spriteGroup);
			}
		}
		if (hasPlant)
		{
			MarkBungeeStateDirty();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		if (!(clip == "Grab"))
		{
			return;
		}
		StealAllPlants();
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			StateHandle grabStateHandle = _grabStateHandle;
			if (grabStateHandle != null && grabStateHandle.IsActive)
			{
				SendStateEvent("ToRise");
			}
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
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			CancelDropPresentation();
			canBlock = false;
			MarkBungeeStateDirty();
			instance.invincible = true;
			if (GodotObject.IsInstanceValid(_target))
			{
				_target.Blow();
			}
			SendStateEvent("ToRise");
			target.Hurt(100.0);
		}
	}

	private void MarkBungeeStateDirty()
	{
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			_networkSpecialStateRevision++;
			if (_networkSpecialStateRevision <= 0)
			{
				_networkSpecialStateRevision = 1;
			}
		}
	}

	private static int NextBungeeActionSequence(int current)
	{
		current++;
		if (current > 0)
		{
			return current;
		}
		return 1;
	}

	private void ApplyRemoteActionAudio(int dropSequence, int grabSequence)
	{
		if (!_remoteActionStateInitialized)
		{
			_remoteActionStateInitialized = true;
			_remoteDropActionSequence = dropSequence;
			_remoteGrabActionSequence = grabSequence;
			_floopAudioPlayed = grabSequence > 0;
			return;
		}
		if (dropSequence > _remoteDropActionSequence)
		{
			_remoteDropActionSequence = dropSequence;
			PlayRemoteBungeeScreamAfterDelayAsync(dropSequence);
		}
		if (grabSequence > _remoteGrabActionSequence)
		{
			_remoteGrabActionSequence = grabSequence;
			PlayFloopAudioOnce();
		}
	}

	private async Task PlayRemoteBungeeScreamAfterDelayAsync(int actionSequence)
	{
		int generation = ++_remoteDropAudioGeneration;
		await ToSignal(GetTree().CreateTimer(1.0, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		if (generation == _remoteDropAudioGeneration && actionSequence == _remoteDropActionSequence && IsInsideTree() && GodotObject.IsInstanceValid(AudioManager.Instance))
		{
			AudioManager.Instance.AudioPlay("BungeeScream");
		}
	}

	private void PlayFloopAudioOnce()
	{
		if (!_floopAudioPlayed)
		{
			_floopAudioPlayed = true;
			if (GodotObject.IsInstanceValid(AudioManager.Instance))
			{
				AudioManager.Instance.AudioPlay("Floop");
			}
		}
	}

	private bool IsDropPresentationCurrent(int generation)
	{
		if (generation == _dropPresentationGeneration && IsInsideTree())
		{
			return _dropStateHandle?.IsActive ?? false;
		}
		return false;
	}

	private void CancelDropPresentation()
	{
		_dropPresentationGeneration++;
		if (GodotObject.IsInstanceValid(dropTween))
		{
			dropTween.Kill();
		}
		dropTween = null;
	}

	private bool IsRisePresentationCurrent(int generation)
	{
		if (generation == _risePresentationGeneration && IsInsideTree())
		{
			return _riseStateHandle?.IsActive ?? false;
		}
		return false;
	}

	private void CancelRisePresentation()
	{
		_risePresentationGeneration++;
		if (GodotObject.IsInstanceValid(_riseTween))
		{
			_riseTween.Kill();
		}
		_riseTween = null;
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "waitGrab", waitGrab },
			{ "hasPlant", hasPlant },
			{ "waitTimer", waitTimer },
			{ "canBlock", canBlock },
			{ "grabOver", grabOver },
			{ "suppressBungeeTarget", suppressBungeeTarget },
			{ "dropActionSequence", _dropActionSequence },
			{ "grabActionSequence", _grabActionSequence },
			{ "networkSpecialStateRevision", _networkSpecialStateRevision }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		waitGrab = data.GetValueOrDefault("waitGrab", false).AsBool();
		hasPlant = data.GetValueOrDefault("hasPlant", false).AsBool();
		waitTimer = data.GetValueOrDefault("waitTimer", 0.0).AsDouble();
		canBlock = data.GetValueOrDefault("canBlock", true).AsBool();
		grabOver = data.GetValueOrDefault("grabOver", false).AsBool();
		suppressBungeeTarget = data.GetValueOrDefault("suppressBungeeTarget", false).AsBool();
		_dropActionSequence = data.GetValueOrDefault("dropActionSequence", 0).AsInt32();
		_grabActionSequence = data.GetValueOrDefault("grabActionSequence", grabOver ? 1 : 0).AsInt32();
		_floopAudioPlayed = grabOver || _grabActionSequence > 0;
		_networkSpecialStateRevision = data.GetValueOrDefault("networkSpecialStateRevision", 1).AsInt32();
		if (IsNodeReady())
		{
			ApplyCurrentTargetability();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(45)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpawnState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportBungeeNetworkState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsNetworkSpecialMovementActive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.SpawnTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DropProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DropExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyTargetability, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "landed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCurrentTargetability, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.StealAllPlants, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanBlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Block, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.MarkBungeeStateDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NextBungeeActionSequence, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "current", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRemoteActionAudio, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "dropSequence", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "grabSequence", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayFloopAudioOnce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsDropPresentationCurrent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "generation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelDropPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsRisePresentationCurrent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "generation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelRisePresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.FinalizeProgressRestore && args.Count == 0)
		{
			FinalizeProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportNetworkSpawnState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpawnState());
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpecialState());
			return true;
		}
		if (method == MethodName.ExportBungeeNetworkState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportBungeeNetworkState());
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState && args.Count == 1)
		{
			ImportNetworkSpecialState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNetworkSpecialStateRevision());
			return true;
		}
		if (method == MethodName.IsNetworkSpecialMovementActive && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNetworkSpecialMovementActive());
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
		if (method == MethodName.SpawnTarget && args.Count == 0)
		{
			SpawnTarget();
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
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.StealAllPlants && args.Count == 0)
		{
			StealAllPlants();
			ret = default;
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
		if (method == MethodName.MarkBungeeStateDirty && args.Count == 0)
		{
			MarkBungeeStateDirty();
			ret = default;
			return true;
		}
		if (method == MethodName.NextBungeeActionSequence && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(NextBungeeActionSequence(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyRemoteActionAudio && args.Count == 2)
		{
			ApplyRemoteActionAudio(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayFloopAudioOnce && args.Count == 0)
		{
			PlayFloopAudioOnce();
			ret = default;
			return true;
		}
		if (method == MethodName.IsDropPresentationCurrent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDropPresentationCurrent(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CancelDropPresentation && args.Count == 0)
		{
			CancelDropPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.IsRisePresentationCurrent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRisePresentationCurrent(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CancelRisePresentation && args.Count == 0)
		{
			CancelRisePresentation();
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
		if (method == MethodName.NextBungeeActionSequence && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(NextBungeeActionSequence(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		ret = default;
		return false;
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
		if (method == MethodName.FinalizeProgressRestore)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState)
		{
			return true;
		}
		if (method == MethodName.ExportNetworkSpawnState)
		{
			return true;
		}
		if (method == MethodName.ExportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ExportBungeeNetworkState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.GetNetworkSpecialStateRevision)
		{
			return true;
		}
		if (method == MethodName.IsNetworkSpecialMovementActive)
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
		if (method == MethodName.SpawnTarget)
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
		if (method == MethodName.ApplyTargetability)
		{
			return true;
		}
		if (method == MethodName.ApplyCurrentTargetability)
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
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.StealAllPlants)
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
		if (method == MethodName.MarkBungeeStateDirty)
		{
			return true;
		}
		if (method == MethodName.NextBungeeActionSequence)
		{
			return true;
		}
		if (method == MethodName.ApplyRemoteActionAudio)
		{
			return true;
		}
		if (method == MethodName.PlayFloopAudioOnce)
		{
			return true;
		}
		if (method == MethodName.IsDropPresentationCurrent)
		{
			return true;
		}
		if (method == MethodName.CancelDropPresentation)
		{
			return true;
		}
		if (method == MethodName.IsRisePresentationCurrent)
		{
			return true;
		}
		if (method == MethodName.CancelRisePresentation)
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
		if (name == PropertyName.suppressBungeeTarget)
		{
			suppressBungeeTarget = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._target)
		{
			_target = VariantUtils.ConvertTo<TowerDefenseBungiTargetSP>(in value);
			return true;
		}
		if (name == PropertyName._dropPresentationGeneration)
		{
			_dropPresentationGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._risePresentationGeneration)
		{
			_risePresentationGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._riseTween)
		{
			_riseTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._networkSpecialStateRevision)
		{
			_networkSpecialStateRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._hasImportedNetworkState)
		{
			_hasImportedNetworkState = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dropActionSequence)
		{
			_dropActionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._grabActionSequence)
		{
			_grabActionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._remoteDropActionSequence)
		{
			_remoteDropActionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._remoteGrabActionSequence)
		{
			_remoteGrabActionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._remoteDropAudioGeneration)
		{
			_remoteDropAudioGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._remoteActionStateInitialized)
		{
			_remoteActionStateInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._floopAudioPlayed)
		{
			_floopAudioPlayed = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
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
		if (name == PropertyName.suppressBungeeTarget)
		{
			value = VariantUtils.CreateFrom(in suppressBungeeTarget);
			return true;
		}
		if (name == PropertyName._target)
		{
			value = VariantUtils.CreateFrom(in _target);
			return true;
		}
		if (name == PropertyName._dropPresentationGeneration)
		{
			value = VariantUtils.CreateFrom(in _dropPresentationGeneration);
			return true;
		}
		if (name == PropertyName._risePresentationGeneration)
		{
			value = VariantUtils.CreateFrom(in _risePresentationGeneration);
			return true;
		}
		if (name == PropertyName._riseTween)
		{
			value = VariantUtils.CreateFrom(in _riseTween);
			return true;
		}
		if (name == PropertyName._networkSpecialStateRevision)
		{
			value = VariantUtils.CreateFrom(in _networkSpecialStateRevision);
			return true;
		}
		if (name == PropertyName._hasImportedNetworkState)
		{
			value = VariantUtils.CreateFrom(in _hasImportedNetworkState);
			return true;
		}
		if (name == PropertyName._dropActionSequence)
		{
			value = VariantUtils.CreateFrom(in _dropActionSequence);
			return true;
		}
		if (name == PropertyName._grabActionSequence)
		{
			value = VariantUtils.CreateFrom(in _grabActionSequence);
			return true;
		}
		if (name == PropertyName._remoteDropActionSequence)
		{
			value = VariantUtils.CreateFrom(in _remoteDropActionSequence);
			return true;
		}
		if (name == PropertyName._remoteGrabActionSequence)
		{
			value = VariantUtils.CreateFrom(in _remoteGrabActionSequence);
			return true;
		}
		if (name == PropertyName._remoteDropAudioGeneration)
		{
			value = VariantUtils.CreateFrom(in _remoteDropAudioGeneration);
			return true;
		}
		if (name == PropertyName._remoteActionStateInitialized)
		{
			value = VariantUtils.CreateFrom(in _remoteActionStateInitialized);
			return true;
		}
		if (name == PropertyName._floopAudioPlayed)
		{
			value = VariantUtils.CreateFrom(in _floopAudioPlayed);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.waitGrab, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.waitTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canBlock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.dropTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.grabOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.suppressBungeeTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._target, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._dropPresentationGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._risePresentationGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._riseTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._networkSpecialStateRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasImportedNetworkState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._dropActionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._grabActionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._remoteDropActionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._remoteGrabActionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._remoteDropAudioGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._remoteActionStateInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._floopAudioPlayed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preserveProgressStateOnNextWalk, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName.waitGrab, Variant.From(in waitGrab));
		info.AddProperty(PropertyName.hasPlant, Variant.From(in hasPlant));
		info.AddProperty(PropertyName.waitTimer, Variant.From(in waitTimer));
		info.AddProperty(PropertyName.canBlock, Variant.From(in canBlock));
		info.AddProperty(PropertyName.dropTween, Variant.From(in dropTween));
		info.AddProperty(PropertyName.grabOver, Variant.From(in grabOver));
		info.AddProperty(PropertyName.suppressBungeeTarget, Variant.From(in suppressBungeeTarget));
		info.AddProperty(PropertyName._target, Variant.From(in _target));
		info.AddProperty(PropertyName._dropPresentationGeneration, Variant.From(in _dropPresentationGeneration));
		info.AddProperty(PropertyName._risePresentationGeneration, Variant.From(in _risePresentationGeneration));
		info.AddProperty(PropertyName._riseTween, Variant.From(in _riseTween));
		info.AddProperty(PropertyName._networkSpecialStateRevision, Variant.From(in _networkSpecialStateRevision));
		info.AddProperty(PropertyName._hasImportedNetworkState, Variant.From(in _hasImportedNetworkState));
		info.AddProperty(PropertyName._dropActionSequence, Variant.From(in _dropActionSequence));
		info.AddProperty(PropertyName._grabActionSequence, Variant.From(in _grabActionSequence));
		info.AddProperty(PropertyName._remoteDropActionSequence, Variant.From(in _remoteDropActionSequence));
		info.AddProperty(PropertyName._remoteGrabActionSequence, Variant.From(in _remoteGrabActionSequence));
		info.AddProperty(PropertyName._remoteDropAudioGeneration, Variant.From(in _remoteDropAudioGeneration));
		info.AddProperty(PropertyName._remoteActionStateInitialized, Variant.From(in _remoteActionStateInitialized));
		info.AddProperty(PropertyName._floopAudioPlayed, Variant.From(in _floopAudioPlayed));
		info.AddProperty(PropertyName._preserveProgressStateOnNextWalk, Variant.From(in _preserveProgressStateOnNextWalk));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value))
		{
			_stateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.waitGrab, out var value2))
		{
			waitGrab = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hasPlant, out var value3))
		{
			hasPlant = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.waitTimer, out var value4))
		{
			waitTimer = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.canBlock, out var value5))
		{
			canBlock = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dropTween, out var value6))
		{
			dropTween = value6.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName.grabOver, out var value7))
		{
			grabOver = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.suppressBungeeTarget, out var value8))
		{
			suppressBungeeTarget = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._target, out var value9))
		{
			_target = value9.As<TowerDefenseBungiTargetSP>();
		}
		if (info.TryGetProperty(PropertyName._dropPresentationGeneration, out var value10))
		{
			_dropPresentationGeneration = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._risePresentationGeneration, out var value11))
		{
			_risePresentationGeneration = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName._riseTween, out var value12))
		{
			_riseTween = value12.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._networkSpecialStateRevision, out var value13))
		{
			_networkSpecialStateRevision = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName._hasImportedNetworkState, out var value14))
		{
			_hasImportedNetworkState = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dropActionSequence, out var value15))
		{
			_dropActionSequence = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName._grabActionSequence, out var value16))
		{
			_grabActionSequence = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName._remoteDropActionSequence, out var value17))
		{
			_remoteDropActionSequence = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName._remoteGrabActionSequence, out var value18))
		{
			_remoteGrabActionSequence = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName._remoteDropAudioGeneration, out var value19))
		{
			_remoteDropAudioGeneration = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName._remoteActionStateInitialized, out var value20))
		{
			_remoteActionStateInitialized = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._floopAudioPlayed, out var value21))
		{
			_floopAudioPlayed = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._preserveProgressStateOnNextWalk, out var value22))
		{
			_preserveProgressStateOnNextWalk = value22.As<bool>();
		}
	}
}
