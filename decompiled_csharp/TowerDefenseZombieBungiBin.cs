using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/BungiBin/Scene/TowerDefenseZombieBungiBin.cs")]
public class TowerDefenseZombieBungiBin : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName HitpointsNearDie = "HitpointsNearDie";

		public new static readonly StringName HitpointsEmpty = "HitpointsEmpty";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName Spawn = "Spawn";

		public static readonly StringName DropEntered = "DropEntered";

		public static readonly StringName DropProcessing = "DropProcessing";

		public static readonly StringName DropExited = "DropExited";

		public static readonly StringName CancelDropPresentation = "CancelDropPresentation";

		public static readonly StringName DownEntered = "DownEntered";

		public static readonly StringName DownProcessing = "DownProcessing";

		public static readonly StringName DownExited = "DownExited";

		public static readonly StringName DownIdleEntered = "DownIdleEntered";

		public static readonly StringName DownIdleProcessing = "DownIdleProcessing";

		public static readonly StringName DownIdleExited = "DownIdleExited";

		public static readonly StringName GrabEntered = "GrabEntered";

		public static readonly StringName GrabProcessing = "GrabProcessing";

		public static readonly StringName GrabExited = "GrabExited";

		public static readonly StringName RiseEntered = "RiseEntered";

		public static readonly StringName RiseProcessing = "RiseProcessing";

		public static readonly StringName RiseExited = "RiseExited";

		public static readonly StringName CancelRisePresentation = "CancelRisePresentation";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName CanBlock = "CanBlock";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public static readonly StringName CapturePayloadDescriptor = "CapturePayloadDescriptor";

		public static readonly StringName AttachCapturedPayload = "AttachCapturedPayload";

		public static readonly StringName WriteCapturedPayloadState = "WriteCapturedPayloadState";

		public static readonly StringName ReadCapturedPayloadState = "ReadCapturedPayloadState";

		public static readonly StringName EnsureCapturedPayloadVisual = "EnsureCapturedPayloadVisual";

		public static readonly StringName CreateCapturedPayloadVisual = "CreateCapturedPayloadVisual";

		public static readonly StringName RemoveCapturedPayloadVisualClone = "RemoveCapturedPayloadVisualClone";

		public new static readonly StringName Block = "Block";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName waitGrab = "waitGrab";

		public static readonly StringName waitDown = "waitDown";

		public static readonly StringName hasPlant = "hasPlant";

		public static readonly StringName _bungeeTarget = "_bungeeTarget";

		public static readonly StringName waitGrabTimer = "waitGrabTimer";

		public static readonly StringName waitDownTimer = "waitDownTimer";

		public static readonly StringName canBlock = "canBlock";

		public static readonly StringName dropTween = "dropTween";

		public static readonly StringName grabOver = "grabOver";

		public static readonly StringName downOver = "downOver";

		public static readonly StringName _targetDropTween = "_targetDropTween";

		public static readonly StringName _riseTween = "_riseTween";

		public static readonly StringName _capturedPayloadNode = "_capturedPayloadNode";

		public static readonly StringName _capturedPayloadIsVisualClone = "_capturedPayloadIsVisualClone";

		public static readonly StringName _capturedPayloadPacketName = "_capturedPayloadPacketName";

		public static readonly StringName _capturedPayloadClip = "_capturedPayloadClip";

		public static readonly StringName _capturedPayloadFrame = "_capturedPayloadFrame";

		public static readonly StringName _capturedPayloadScale = "_capturedPayloadScale";

		public static readonly StringName _capturedPayloadCustom = "_capturedPayloadCustom";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private StateHandle _dropStateHandle;

	private StateHandle _grabStateHandle;

	private StateHandle _riseStateHandle;

	private StateHandle _downStateHandle;

	private StateHandle _downIdleStateHandle;

	private bool _stateSignalsConnected;

	public bool waitGrab;

	public bool waitDown;

	public bool hasPlant;

	private Sprite2D _bungeeTarget;

	public double waitGrabTimer;

	public double waitDownTimer;

	public bool canBlock = true;

	public Tween dropTween;

	public bool grabOver;

	public bool downOver;

	private Tween _targetDropTween;

	private Tween _riseTween;

	private Node2D _capturedPayloadNode;

	private bool _capturedPayloadIsVisualClone;

	private string _capturedPayloadPacketName = "";

	private string _capturedPayloadClip = "";

	private int _capturedPayloadFrame;

	private Vector2 _capturedPayloadScale = Vector2.One;

	private Array<string> _capturedPayloadCustom = new Array<string>();

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
		_dropStateHandle = StateMachine?.GetStateById("zombie.bungi_bin.drop");
		_grabStateHandle = StateMachine?.GetStateById("zombie.bungi_bin.grab");
		_riseStateHandle = StateMachine?.GetStateById("zombie.bungi_bin.rise");
		_downStateHandle = StateMachine?.GetStateById("zombie.bungi_bin.down");
		_downIdleStateHandle = StateMachine?.GetStateById("zombie.bungi_bin.down_idle");
		StateHandle dropStateHandle = _dropStateHandle;
		if (dropStateHandle == null || !dropStateHandle.IsValid)
		{
			return;
		}
		StateHandle grabStateHandle = _grabStateHandle;
		if (grabStateHandle == null || !grabStateHandle.IsValid)
		{
			return;
		}
		StateHandle riseStateHandle = _riseStateHandle;
		if (riseStateHandle == null || !riseStateHandle.IsValid)
		{
			return;
		}
		StateHandle downStateHandle = _downStateHandle;
		if (downStateHandle != null && downStateHandle.IsValid)
		{
			StateHandle downIdleStateHandle = _downIdleStateHandle;
			if (downIdleStateHandle != null && downIdleStateHandle.IsValid)
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
				_downStateHandle.Entered += DownEntered;
				_downStateHandle.Exited += DownExited;
				_downStateHandle.PhysicsProcessing += DownProcessing;
				_downIdleStateHandle.Entered += DownIdleEntered;
				_downIdleStateHandle.Exited += DownIdleExited;
				_downIdleStateHandle.PhysicsProcessing += DownIdleProcessing;
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
			if (_downStateHandle != null)
			{
				_downStateHandle.Entered -= DownEntered;
				_downStateHandle.Exited -= DownExited;
				_downStateHandle.PhysicsProcessing -= DownProcessing;
			}
			if (_downIdleStateHandle != null)
			{
				_downIdleStateHandle.Entered -= DownIdleEntered;
				_downIdleStateHandle.Exited -= DownIdleExited;
				_downIdleStateHandle.PhysicsProcessing -= DownIdleProcessing;
			}
			_dropStateHandle = null;
			_grabStateHandle = null;
			_riseStateHandle = null;
			_downStateHandle = null;
			_downIdleStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		CancelDropPresentation();
		CancelRisePresentation();
		DisconnectStateSignals();
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_bungeeTarget = GetNode<Sprite2D>("%BungeeTarget");
			targetRegistrationComponent.canProjectileCheck = false;
			targetRegistrationComponent.canCarry = false;
			instance.maskFlags = 0;
			AddToGroup("Bungi", persistent: true);
			if (TowerDefenseManager.Instance.IsGameRunning())
			{
				z = 600.0;
				isGround = false;
			}
			ConnectStateSignals();
			EnsureCapturedPayloadVisual();
		}
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
		ActivateGameplayProcessing();
		SendStateEvent("ToDrop");
	}

	public override void IdleEntered()
	{
		base.IdleEntered();
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (!waitDown)
		{
			return;
		}
		if (waitDownTimer < 3.0)
		{
			if (!sprite.pause)
			{
				waitDownTimer += delta * timeScale;
			}
			return;
		}
		waitDown = false;
		foreach (string item in new List<string>(buff.buffDictionary.Keys))
		{
			if (item != "Hypnoses")
			{
				buff.DeleteBuff(item);
			}
		}
		SendStateEvent("ToDown");
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
		List<TowerDefenseCharacter> cleanCharactersList = TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList();
		List<TowerDefenseCharacter> list2 = new List<TowerDefenseCharacter>();
		foreach (TowerDefenseCharacter item2 in cleanCharactersList)
		{
			if (!(item2 is TowerDefensePlant) || item2.isDestroy || item2.die || !CanTarget(item2) || item2 is TowerDefensePlantBowlingBase || !item2.instance.canBeCollection || list.Contains(item2.gridPos))
			{
				continue;
			}
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(item2.gridPos);
			bool flag = false;
			foreach (TowerDefenseCharacter character in mapCell.GetCharacterList())
			{
				if (character.config.name == "TrashBin")
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list2.Add(item2);
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
		if (!GodotObject.IsInstanceValid(cell))
		{
			Destroy();
			return;
		}
		instance.canBeCollection = false;
		instance.invincible = true;
		z = 600.0;
		isGround = false;
		_bungeeTarget.Visible = true;
		_targetDropTween = CreateTween();
		_targetDropTween.SetEase(Tween.EaseType.Out);
		_targetDropTween.SetTrans(Tween.TransitionType.Cubic);
		_targetDropTween.TweenProperty(_bungeeTarget, "position", new Vector2(0f, (float)(15.0 - cell.GetGroundHeight())), 0.5).From(new Vector2(0f, -585f));
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
				waitGrab = false;
				waitDown = true;
				instance.invincible = false;
				instance.canBeCollection = true;
				targetRegistrationComponent.canProjectileCheck = true;
				instance.maskFlags = 1;
				Idle();
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

	public void DownEntered()
	{
		waitGrab = true;
		instance.unUseBuffFlags = -9;
		targetRegistrationComponent.canProjectileCheck = false;
		instance.invincible = true;
		sprite.SetAnimation("Down", loop: false, 0.2);
	}

	public void DownProcessing(double delta)
	{
		sprite.timeScale = timeScale * 2.0;
	}

	public void DownExited()
	{
	}

	public void DownIdleEntered()
	{
		instance.invincible = false;
		instance.canBeCollection = true;
		targetRegistrationComponent.canProjectileCheck = true;
		instance.maskFlags = 1;
		sprite.timeScale = timeScale;
		sprite.SetAnimation("DownIdle", loop: true, 0.2);
	}

	public void DownIdleProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		if (!waitGrab)
		{
			return;
		}
		if (waitGrabTimer < 5.0)
		{
			if (!sprite.pause)
			{
				waitGrabTimer += delta * timeScale;
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
		SendStateEvent("ToGrab");
	}

	public void DownIdleExited()
	{
	}

	public void GrabEntered()
	{
		instance.unUseBuffFlags = -9;
		targetRegistrationComponent.canProjectileCheck = false;
		instance.invincible = true;
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
		if (!(command == "down"))
		{
			if (!(command == "grab") || grabOver)
			{
				return;
			}
			grabOver = true;
			AudioManager.Instance.AudioPlay("Floop");
			if (!TowerDefenseManager.HasGameplayAuthority)
			{
				return;
			}
			cell = TowerDefenseManager.GetMapCell(gridPos);
			if (!GodotObject.IsInstanceValid(cell))
			{
				return;
			}
			List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
			foreach (TowerDefenseCharacter item in new List<TowerDefenseCharacter>(cell.GetCharacterList()))
			{
				if (item.config.name == "TrashBin")
				{
					CapturePayloadDescriptor(item);
					AttachCapturedPayload(item);
				}
				else if (item.camp != camp && item is TowerDefensePlant && !(item is TowerDefensePlantBowlingBase))
				{
					list.Add(item);
				}
			}
			if (!hasPlant)
			{
				return;
			}
			{
				foreach (TowerDefenseCharacter item2 in list)
				{
					item2.Destroy();
				}
				return;
			}
		}
		if (downOver)
		{
			return;
		}
		downOver = true;
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("TrashBin");
		if (GodotObject.IsInstanceValid(packetConfig) && cell.CanPacketPlant(packetConfig))
		{
			TowerDefenseCharacter towerDefenseCharacter = (EconomyOwnerAccountId.IsValid ? packetConfig.Plant(EconomyOwnerAccountId, gridPos, playAudio: false) : packetConfig.Plant(gridPos, playAudio: false));
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				towerDefenseCharacter.camp = camp;
				TowerDefenseManager.PublishSpawnedCharacter("TrashBin", towerDefenseCharacter, useCreate: false, 0.0, walkAfterSpawn: false, "", new Dictionary { ["plant_play_audio"] = false });
			}
		}
	}

	public override void AnimeCompleted(string clip)
	{
		if (!(clip == "Down"))
		{
			if (clip == "Grab")
			{
				AnimeEvent("grab", default);
				SendStateEvent("ToRise");
			}
		}
		else
		{
			AnimeEvent("down", default);
			SendStateEvent("ToDownIdle");
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

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary
		{
			["waitGrab"] = waitGrab,
			["hasPlant"] = hasPlant,
			["waitDownTimer"] = waitDownTimer,
			["waitGrabTimer"] = waitGrabTimer,
			["canBlock"] = canBlock,
			["grabOver"] = grabOver,
			["waitDown"] = waitDown,
			["downOver"] = downOver
		};
		WriteCapturedPayloadState(dictionary);
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		waitGrab = data.GetValueOrDefault("waitGrab", false).AsBool();
		hasPlant = data.GetValueOrDefault("hasPlant", false).AsBool();
		waitDownTimer = data.GetValueOrDefault("waitDownTimer", 0.0).AsDouble();
		waitGrabTimer = data.GetValueOrDefault("waitGrabTimer", 0.0).AsDouble();
		canBlock = data.GetValueOrDefault("canBlock", true).AsBool();
		grabOver = data.GetValueOrDefault("grabOver", false).AsBool();
		waitDown = data.GetValueOrDefault("waitDown", false).AsBool();
		downOver = data.GetValueOrDefault("downOver", false).AsBool();
		ReadCapturedPayloadState(data);
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		Dictionary dictionary = new Dictionary
		{
			["rev"] = GetNetworkSpecialStateRevision(),
			["hasPlant"] = hasPlant
		};
		WriteCapturedPayloadState(dictionary);
		return dictionary;
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		if (data != null)
		{
			hasPlant = data.GetValueOrDefault("hasPlant", hasPlant).AsBool();
			ReadCapturedPayloadState(data);
		}
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return hasPlant ? 1 : 0;
	}

	private void CapturePayloadDescriptor(TowerDefenseCharacter target)
	{
		if (GodotObject.IsInstanceValid(target))
		{
			RemoveCapturedPayloadVisualClone();
			_capturedPayloadNode = target;
			_capturedPayloadIsVisualClone = false;
			hasPlant = true;
			_capturedPayloadPacketName = (GodotObject.IsInstanceValid(target.packet) ? target.packet.saveKey : "");
			_capturedPayloadClip = (GodotObject.IsInstanceValid(target.sprite) ? target.sprite.clip : "");
			_capturedPayloadFrame = (GodotObject.IsInstanceValid(target.sprite) ? target.sprite.frameIndex : 0);
			_capturedPayloadScale = target.Scale;
			_capturedPayloadCustom = ((target.currentCustom != null) ? target.currentCustom.Duplicate() : new Array<string>());
		}
	}

	private void AttachCapturedPayload(TowerDefenseCharacter target)
	{
		if (GodotObject.IsInstanceValid(target) && GodotObject.IsInstanceValid(spriteGroup))
		{
			target.die = true;
			target.Destroy(freeInstance: false);
			if (GodotObject.IsInstanceValid(target.sprite))
			{
				target.sprite.pause = true;
			}
			if (GodotObject.IsInstanceValid(target.shadowSprite))
			{
				target.shadowSprite.Visible = false;
				target.shadowSprite.Texture = null;
			}
			target.DisableGameplayForPermanentEmbeddedVisual();
			target.Reparent(spriteGroup);
		}
	}

	private void WriteCapturedPayloadState(Dictionary data)
	{
		data["capturedPayloadPacket"] = _capturedPayloadPacketName;
		data["capturedPayloadClip"] = _capturedPayloadClip;
		data["capturedPayloadFrame"] = _capturedPayloadFrame;
		data["capturedPayloadScale"] = _capturedPayloadScale;
		data["capturedPayloadCustom"] = _capturedPayloadCustom;
	}

	private void ReadCapturedPayloadState(Dictionary data)
	{
		_capturedPayloadPacketName = data.GetValueOrDefault("capturedPayloadPacket", _capturedPayloadPacketName).AsString();
		_capturedPayloadClip = data.GetValueOrDefault("capturedPayloadClip", _capturedPayloadClip).AsString();
		_capturedPayloadFrame = data.GetValueOrDefault("capturedPayloadFrame", _capturedPayloadFrame).AsInt32();
		_capturedPayloadScale = data.GetValueOrDefault("capturedPayloadScale", _capturedPayloadScale).AsVector2();
		_capturedPayloadCustom = data.GetValueOrDefault("capturedPayloadCustom", new Array<string>()).AsGodotArray<string>();
		EnsureCapturedPayloadVisual();
	}

	private void EnsureCapturedPayloadVisual()
	{
		if (!hasPlant)
		{
			RemoveCapturedPayloadVisualClone();
		}
		else if (!GodotObject.IsInstanceValid(_capturedPayloadNode) && IsNodeReady() && GodotObject.IsInstanceValid(spriteGroup) && !string.IsNullOrEmpty(_capturedPayloadPacketName))
		{
			CreateCapturedPayloadVisual();
		}
	}

	private void CreateCapturedPayloadVisual()
	{
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(_capturedPayloadPacketName);
		if (!GodotObject.IsInstanceValid(packetConfig) || !GodotObject.IsInstanceValid(packetConfig.characterConfig) || string.IsNullOrEmpty(packetConfig.characterConfig.name))
		{
			return;
		}
		PackedScene chacraterScene = TowerDefenseManager.GetChacraterScene(packetConfig.characterConfig.name);
		if (!GodotObject.IsInstanceValid(chacraterScene))
		{
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter = chacraterScene.Instantiate<TowerDefenseCharacter>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			return;
		}
		towerDefenseCharacter.packet = packetConfig;
		towerDefenseCharacter.config = packetConfig.characterConfig;
		if (_capturedPayloadCustom.Count > 0)
		{
			towerDefenseCharacter.SetCustoms(_capturedPayloadCustom);
		}
		Node2D nodeOrNull = towerDefenseCharacter.GetNodeOrNull<Node2D>("%SpriteGroup");
		AdobeAnimateSprite adobeAnimateSprite = towerDefenseCharacter.sprite;
		if (!GodotObject.IsInstanceValid(nodeOrNull))
		{
			towerDefenseCharacter.Free();
			return;
		}
		towerDefenseCharacter.RemoveChild(nodeOrNull);
		towerDefenseCharacter.Free();
		nodeOrNull.Name = "CapturedPayloadVisual";
		nodeOrNull.Position = Vector2.Zero;
		nodeOrNull.Scale = new Vector2(nodeOrNull.Scale.X * _capturedPayloadScale.X, nodeOrNull.Scale.Y * _capturedPayloadScale.Y);
		spriteGroup.AddChild(nodeOrNull, forceReadableName: false, InternalMode.Disabled);
		if (GodotObject.IsInstanceValid(adobeAnimateSprite))
		{
			if (!string.IsNullOrEmpty(_capturedPayloadClip))
			{
				adobeAnimateSprite.SetAnimation(_capturedPayloadClip);
			}
			adobeAnimateSprite.frameIndex = Math.Max(0, _capturedPayloadFrame);
			adobeAnimateSprite.pause = true;
		}
		_capturedPayloadNode = nodeOrNull;
		_capturedPayloadIsVisualClone = true;
	}

	private void RemoveCapturedPayloadVisualClone()
	{
		if (_capturedPayloadIsVisualClone && GodotObject.IsInstanceValid(_capturedPayloadNode))
		{
			_capturedPayloadNode.QueueFree();
		}
		_capturedPayloadNode = null;
		_capturedPayloadIsVisualClone = false;
	}

	public override void Block(TowerDefenseCharacter target)
	{
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		if (GodotObject.IsInstanceValid(dropTween))
		{
			dropTween.Kill();
		}
		canBlock = false;
		instance.invincible = true;
		if (target.gridPos != gridPos)
		{
			sprite.SetFliter("bin", open: false);
			if (TowerDefenseManager.HasGameplayAuthority)
			{
				TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("TrashBin");
				if (!GodotObject.IsInstanceValid(packetConfig))
				{
					SendStateEvent("ToRise");
					return;
				}
				if (cell.CanPacketPlant(packetConfig))
				{
					TowerDefenseCharacter character = (EconomyOwnerAccountId.IsValid ? packetConfig.Plant(EconomyOwnerAccountId, gridPos) : packetConfig.Plant(gridPos));
					if (GodotObject.IsInstanceValid(character))
					{
						TowerDefenseManager.PublishSpawnedCharacter("TrashBin", character, useCreate: false);
					}
				}
			}
		}
		SendStateEvent("ToRise");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(43)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.DropProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DropExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelDropPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DownEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DownProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DownExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DownIdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DownIdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DownIdleExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanBlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CapturePayloadDescriptor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AttachCapturedPayload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.WriteCapturedPayloadState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadCapturedPayloadState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCapturedPayloadVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateCapturedPayloadVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveCapturedPayloadVisualClone, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Block, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.DownEntered && args.Count == 0)
		{
			DownEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DownProcessing && args.Count == 1)
		{
			DownProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DownExited && args.Count == 0)
		{
			DownExited();
			ret = default;
			return true;
		}
		if (method == MethodName.DownIdleEntered && args.Count == 0)
		{
			DownIdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DownIdleProcessing && args.Count == 1)
		{
			DownIdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DownIdleExited && args.Count == 0)
		{
			DownIdleExited();
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
		if (method == MethodName.ExportNetworkSpecialState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpecialState());
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
		if (method == MethodName.CapturePayloadDescriptor && args.Count == 1)
		{
			CapturePayloadDescriptor(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttachCapturedPayload && args.Count == 1)
		{
			AttachCapturedPayload(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WriteCapturedPayloadState && args.Count == 1)
		{
			WriteCapturedPayloadState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadCapturedPayloadState && args.Count == 1)
		{
			ReadCapturedPayloadState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCapturedPayloadVisual && args.Count == 0)
		{
			EnsureCapturedPayloadVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCapturedPayloadVisual && args.Count == 0)
		{
			CreateCapturedPayloadVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveCapturedPayloadVisualClone && args.Count == 0)
		{
			RemoveCapturedPayloadVisualClone();
			ret = default;
			return true;
		}
		if (method == MethodName.Block && args.Count == 1)
		{
			Block(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.DownEntered)
		{
			return true;
		}
		if (method == MethodName.DownProcessing)
		{
			return true;
		}
		if (method == MethodName.DownExited)
		{
			return true;
		}
		if (method == MethodName.DownIdleEntered)
		{
			return true;
		}
		if (method == MethodName.DownIdleProcessing)
		{
			return true;
		}
		if (method == MethodName.DownIdleExited)
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
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.CanBlock)
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
		if (method == MethodName.ExportNetworkSpecialState)
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
		if (method == MethodName.CapturePayloadDescriptor)
		{
			return true;
		}
		if (method == MethodName.AttachCapturedPayload)
		{
			return true;
		}
		if (method == MethodName.WriteCapturedPayloadState)
		{
			return true;
		}
		if (method == MethodName.ReadCapturedPayloadState)
		{
			return true;
		}
		if (method == MethodName.EnsureCapturedPayloadVisual)
		{
			return true;
		}
		if (method == MethodName.CreateCapturedPayloadVisual)
		{
			return true;
		}
		if (method == MethodName.RemoveCapturedPayloadVisualClone)
		{
			return true;
		}
		if (method == MethodName.Block)
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
		if (name == PropertyName.waitDown)
		{
			waitDown = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.waitGrabTimer)
		{
			waitGrabTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.waitDownTimer)
		{
			waitDownTimer = VariantUtils.ConvertTo<double>(in value);
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
		if (name == PropertyName.downOver)
		{
			downOver = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._capturedPayloadNode)
		{
			_capturedPayloadNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._capturedPayloadIsVisualClone)
		{
			_capturedPayloadIsVisualClone = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._capturedPayloadPacketName)
		{
			_capturedPayloadPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._capturedPayloadClip)
		{
			_capturedPayloadClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._capturedPayloadFrame)
		{
			_capturedPayloadFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._capturedPayloadScale)
		{
			_capturedPayloadScale = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._capturedPayloadCustom)
		{
			_capturedPayloadCustom = VariantUtils.ConvertToArray<string>(in value);
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
		if (name == PropertyName.waitDown)
		{
			value = VariantUtils.CreateFrom(in waitDown);
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
		if (name == PropertyName.waitGrabTimer)
		{
			value = VariantUtils.CreateFrom(in waitGrabTimer);
			return true;
		}
		if (name == PropertyName.waitDownTimer)
		{
			value = VariantUtils.CreateFrom(in waitDownTimer);
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
		if (name == PropertyName.downOver)
		{
			value = VariantUtils.CreateFrom(in downOver);
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
		if (name == PropertyName._capturedPayloadNode)
		{
			value = VariantUtils.CreateFrom(in _capturedPayloadNode);
			return true;
		}
		if (name == PropertyName._capturedPayloadIsVisualClone)
		{
			value = VariantUtils.CreateFrom(in _capturedPayloadIsVisualClone);
			return true;
		}
		if (name == PropertyName._capturedPayloadPacketName)
		{
			value = VariantUtils.CreateFrom(in _capturedPayloadPacketName);
			return true;
		}
		if (name == PropertyName._capturedPayloadClip)
		{
			value = VariantUtils.CreateFrom(in _capturedPayloadClip);
			return true;
		}
		if (name == PropertyName._capturedPayloadFrame)
		{
			value = VariantUtils.CreateFrom(in _capturedPayloadFrame);
			return true;
		}
		if (name == PropertyName._capturedPayloadScale)
		{
			value = VariantUtils.CreateFrom(in _capturedPayloadScale);
			return true;
		}
		if (name == PropertyName._capturedPayloadCustom)
		{
			value = VariantUtils.CreateFromArray(_capturedPayloadCustom);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.waitDown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bungeeTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.waitGrabTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.waitDownTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canBlock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.dropTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.grabOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.downOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._targetDropTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._riseTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._capturedPayloadNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._capturedPayloadIsVisualClone, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._capturedPayloadPacketName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._capturedPayloadClip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._capturedPayloadFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._capturedPayloadScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._capturedPayloadCustom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName.waitGrab, Variant.From(in waitGrab));
		info.AddProperty(PropertyName.waitDown, Variant.From(in waitDown));
		info.AddProperty(PropertyName.hasPlant, Variant.From(in hasPlant));
		info.AddProperty(PropertyName._bungeeTarget, Variant.From(in _bungeeTarget));
		info.AddProperty(PropertyName.waitGrabTimer, Variant.From(in waitGrabTimer));
		info.AddProperty(PropertyName.waitDownTimer, Variant.From(in waitDownTimer));
		info.AddProperty(PropertyName.canBlock, Variant.From(in canBlock));
		info.AddProperty(PropertyName.dropTween, Variant.From(in dropTween));
		info.AddProperty(PropertyName.grabOver, Variant.From(in grabOver));
		info.AddProperty(PropertyName.downOver, Variant.From(in downOver));
		info.AddProperty(PropertyName._targetDropTween, Variant.From(in _targetDropTween));
		info.AddProperty(PropertyName._riseTween, Variant.From(in _riseTween));
		info.AddProperty(PropertyName._capturedPayloadNode, Variant.From(in _capturedPayloadNode));
		info.AddProperty(PropertyName._capturedPayloadIsVisualClone, Variant.From(in _capturedPayloadIsVisualClone));
		info.AddProperty(PropertyName._capturedPayloadPacketName, Variant.From(in _capturedPayloadPacketName));
		info.AddProperty(PropertyName._capturedPayloadClip, Variant.From(in _capturedPayloadClip));
		info.AddProperty(PropertyName._capturedPayloadFrame, Variant.From(in _capturedPayloadFrame));
		info.AddProperty(PropertyName._capturedPayloadScale, Variant.From(in _capturedPayloadScale));
		info.AddProperty(PropertyName._capturedPayloadCustom, Variant.CreateFrom(_capturedPayloadCustom));
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
		if (info.TryGetProperty(PropertyName.waitDown, out var value3))
		{
			waitDown = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.hasPlant, out var value4))
		{
			hasPlant = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._bungeeTarget, out var value5))
		{
			_bungeeTarget = value5.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.waitGrabTimer, out var value6))
		{
			waitGrabTimer = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.waitDownTimer, out var value7))
		{
			waitDownTimer = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.canBlock, out var value8))
		{
			canBlock = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dropTween, out var value9))
		{
			dropTween = value9.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName.grabOver, out var value10))
		{
			grabOver = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.downOver, out var value11))
		{
			downOver = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._targetDropTween, out var value12))
		{
			_targetDropTween = value12.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._riseTween, out var value13))
		{
			_riseTween = value13.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._capturedPayloadNode, out var value14))
		{
			_capturedPayloadNode = value14.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._capturedPayloadIsVisualClone, out var value15))
		{
			_capturedPayloadIsVisualClone = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._capturedPayloadPacketName, out var value16))
		{
			_capturedPayloadPacketName = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName._capturedPayloadClip, out var value17))
		{
			_capturedPayloadClip = value17.As<string>();
		}
		if (info.TryGetProperty(PropertyName._capturedPayloadFrame, out var value18))
		{
			_capturedPayloadFrame = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName._capturedPayloadScale, out var value19))
		{
			_capturedPayloadScale = value19.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._capturedPayloadCustom, out var value20))
		{
			_capturedPayloadCustom = value20.AsGodotArray<string>();
		}
	}
}
