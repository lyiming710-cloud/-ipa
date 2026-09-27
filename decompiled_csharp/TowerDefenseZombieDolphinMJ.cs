using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter4/DolphinMJ/Scene/TowerDefenseZombieDolphinMJ.cs")]
public class TowerDefenseZombieDolphinMJ : TowerDefenseZombie, IJackson, INetworkDancerOwner
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName Walk = "Walk";

		public static readonly StringName RunEntered = "RunEntered";

		public static readonly StringName RunProcessing = "RunProcessing";

		public static readonly StringName RunExited = "RunExited";

		public static readonly StringName JumpEntered = "JumpEntered";

		public static readonly StringName JumpProcessing = "JumpProcessing";

		public static readonly StringName JumpExited = "JumpExited";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName AnimeStarted = "AnimeStarted";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName CanSpawnDancer = "CanSpawnDancer";

		public static readonly StringName SpawnDancer = "SpawnDancer";

		public static readonly StringName CanReplaceDancer = "CanReplaceDancer";

		public static readonly StringName SpawnSingleDancer = "SpawnSingleDancer";

		public static readonly StringName CreateDancerSpawnState = "CreateDancerSpawnState";

		public static readonly StringName ChangeSpotlightColor = "ChangeSpotlightColor";

		public static readonly StringName RemoveDancer = "RemoveDancer";

		public static readonly StringName SetNetworkDancer = "SetNetworkDancer";

		public static readonly StringName RefreshPendingDancerBatchEligibility = "RefreshPendingDancerBatchEligibility";

		public static readonly StringName ResolvePendingDancerRelations = "ResolvePendingDancerRelations";

		public static readonly StringName ReleaseDancerOwnership = "ReleaseDancerOwnership";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";

		public new static readonly StringName CanBlock = "CanBlock";

		public new static readonly StringName BlockType = "BlockType";

		public new static readonly StringName Block = "Block";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName dolphin = "dolphin";

		public static readonly StringName _spotlight2 = "_spotlight2";

		public static readonly StringName _spotlight = "_spotlight";

		public static readonly StringName spotlightGrandient = "spotlightGrandient";

		public static readonly StringName _dolphin = "_dolphin";

		public static readonly StringName jumpMove = "jumpMove";

		public static readonly StringName isJump = "isJump";

		public static readonly StringName isJumpInWater = "isJumpInWater";

		public static readonly StringName isBlock = "isBlock";

		public static readonly StringName audioPlay = "audioPlay";

		public static readonly StringName timer = "timer";

		public static readonly StringName dancerList = "dancerList";

		public static readonly StringName _pendingDancerSyncIds = "_pendingDancerSyncIds";

		public static readonly StringName _pendingDancerNodeNames = "_pendingDancerNodeNames";

		public static readonly StringName _pendingDancerResolveRemaining = "_pendingDancerResolveRemaining";

		public static readonly StringName _hasPendingDancerRelations = "_hasPendingDancerRelations";

		public static readonly StringName dancerPacketName = "dancerPacketName";

		public static readonly StringName firstSpawn = "firstSpawn";

		public static readonly StringName isSpawn = "isSpawn";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const double PendingDancerResolveIntervalSeconds = 0.25;

	private Sprite2D _spotlight2;

	private Sprite2D _spotlight;

	private AttackComponent _attackComponent2;

	[Export(PropertyHint.None, "")]
	public Gradient spotlightGrandient;

	private bool _dolphin = true;

	public bool jumpMove;

	public bool isJump;

	public bool isJumpInWater;

	public bool isBlock;

	public bool audioPlay;

	public double timer = 5.0;

	public Array<TowerDefenseCharacter> dancerList = new Array<TowerDefenseCharacter>();

	private readonly int[] _pendingDancerSyncIds = new int[4] { -1, -1, -1, -1 };

	private readonly string[] _pendingDancerNodeNames = new string[4] { "", "", "", "" };

	private double _pendingDancerResolveRemaining;

	private bool _hasPendingDancerRelations;

	[Export(PropertyHint.None, "")]
	public string dancerPacketName = "ZombieDolphinDC";

	public bool firstSpawn;

	public bool isSpawn;

	private StateHandle _jumpStateHandle;

	private StateHandle _runStateHandle;

	private bool _roleStateSignalsConnected;

	public bool dolphin
	{
		get
		{
			return _dolphin;
		}
		set
		{
			_dolphin = value;
			if (!_dolphin)
			{
				useAttackDps = true;
				attackComponent.attackType = "Eat";
				useAttackDps = true;
				walkAnimeClip = "Walk";
				attackAnimeClip = "Eat";
				dieAnimeClip = "Death";
			}
		}
	}

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
		_jumpStateHandle = StateMachine?.GetStateById("zombie.dolphin_mj.jump");
		_runStateHandle = StateMachine?.GetStateById("zombie.dolphin_mj.run");
		StateHandle jumpStateHandle = _jumpStateHandle;
		if (jumpStateHandle != null && jumpStateHandle.IsValid)
		{
			StateHandle runStateHandle = _runStateHandle;
			if (runStateHandle != null && runStateHandle.IsValid)
			{
				_jumpStateHandle.Entered += JumpEntered;
				_jumpStateHandle.Exited += JumpExited;
				_jumpStateHandle.PhysicsProcessing += JumpProcessing;
				_runStateHandle.Entered += RunEntered;
				_runStateHandle.Exited += RunExited;
				_runStateHandle.PhysicsProcessing += RunProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			if (_jumpStateHandle != null)
			{
				_jumpStateHandle.Entered -= JumpEntered;
				_jumpStateHandle.Exited -= JumpExited;
				_jumpStateHandle.PhysicsProcessing -= JumpProcessing;
			}
			_jumpStateHandle = null;
			if (_runStateHandle != null)
			{
				_runStateHandle.Entered -= RunEntered;
				_runStateHandle.Exited -= RunExited;
				_runStateHandle.PhysicsProcessing -= RunProcessing;
			}
			_runStateHandle = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		if (IsQueuedForDeletion())
		{
			ReleaseDancerOwnership();
		}
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_spotlight2 = GetNode<Sprite2D>("%Spotlight2");
			_spotlight = GetNode<Sprite2D>("%Spotlight");
			_attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
			dancerList.Resize(4);
			sprite.OnAnimeStarted += AnimeStarted;
			ConnectRoleStateSignals();
			RefreshPendingDancerBatchEligibility();
			if (_hasPendingDancerRelations)
			{
				ResolvePendingDancerRelations();
			}
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint())
		{
			return;
		}
		if (IsInsideComponentBattlefield && timer > 0.0)
		{
			timer -= delta;
		}
		if (_hasPendingDancerRelations)
		{
			_pendingDancerResolveRemaining -= delta;
			if (!(_pendingDancerResolveRemaining > 0.0))
			{
				ResolvePendingDancerRelations();
			}
		}
	}

	public override void WalkEntered()
	{
		if (jumpMove)
		{
			jumpMove = false;
			if (inWater)
			{
				sprite.SetAnimation(swimAnimeClip);
			}
			else
			{
				sprite.SetAnimation(walkAnimeClip);
			}
		}
		else if (isBlock)
		{
			isBlock = false;
			if (inWater)
			{
				sprite.SetAnimation(swimAnimeClip);
			}
			else
			{
				sprite.SetAnimation(walkAnimeClip);
			}
		}
		else if (inWater)
		{
			sprite.SetAnimation(swimAnimeClip, loop: true, 0.2);
		}
		else
		{
			sprite.SetAnimation(walkAnimeClip, loop: true, 0.2);
		}
		ActivateGroundMovementAfterStateDelay(WalkStateHandle);
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		if (!audioPlay && (double)GetGlobalPositionForPhysicsFrame(currentPhysicsFrame).X < TowerDefenseManager.Instance.GetMapGroundRight())
		{
			AudioManager.Instance.AudioPlay("DolphinAppears");
			audioPlay = true;
		}
		if (!isSpawn && timer <= 0.0 && CanSpawnDancer())
		{
			if (inWater)
			{
				sprite.SetAnimation("PointUp2", loop: false);
				sprite.AddAnimation("PointDown2", 0.75, loop: false);
			}
			else if (dolphin)
			{
				sprite.SetAnimation("PointUp3", loop: false);
				sprite.AddAnimation("PointDown3", 0.75, loop: false);
			}
			else
			{
				sprite.SetAnimation("PointUp4", loop: false);
				sprite.AddAnimation("PointDown4", 0.75, loop: false);
			}
			isSpawn = true;
		}
		if (!dolphin && attackComponent.CanAttack() && GodotObject.IsInstanceValid(attackComponent.target) && attackComponent.target is TowerDefenseVase)
		{
			attackComponent.SmashAttackCell(((TowerDefenseZombieConfig)config).smashAttack);
		}
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		if (!isSpawn && timer <= 0.0 && CanSpawnDancer())
		{
			if (inWater)
			{
				sprite.SetAnimation("PointUp2", loop: false);
				sprite.AddAnimation("PointDown2", 0.75, loop: false);
			}
			else if (dolphin)
			{
				sprite.SetAnimation("PointUp3", loop: false);
				sprite.AddAnimation("PointDown3", 0.75, loop: false);
			}
			else
			{
				sprite.SetAnimation("PointUp4", loop: false);
				sprite.AddAnimation("PointDown4", 0.75, loop: false);
			}
			isSpawn = true;
		}
	}

	public override void Walk()
	{
		if (dolphin && inWater)
		{
			SendStateEvent("ToRun");
		}
		else
		{
			SendStateEvent("ToWalk");
		}
	}

	public void RunEntered()
	{
		if (inWater)
		{
			if (!inSwimPlay && inSwimAnimeClip != "")
			{
				sprite.SetAnimation(inSwimAnimeClip, loop: false, 0.2);
				sprite.AddAnimation("DolphinRun", 0.0);
				inSwimPlay = true;
			}
			else
			{
				sprite.SetAnimation("DolphinRun");
			}
		}
		else
		{
			sprite.SetAnimation(walkAnimeClip, loop: true, 0.2);
		}
		groundMoveComponent.SetAlive(true);
	}

	public void RunProcessing(double delta)
	{
		if (sprite.clip == inSwimAnimeClip)
		{
			sprite.timeScale = timeScale * walkSpeedScale;
		}
		else
		{
			sprite.timeScale = timeScale * walkSpeedScale * 0.5;
		}
		if (!nearDie && !TowerDefenseManager.Instance.backZombie)
		{
			if (sprite.clip != inSwimAnimeClip && (sprite.clip == "DolphinRun" || sprite.clip == "PointUp1" || sprite.clip == "PointDown1") && _attackComponent2.HasAttackGridTargetCandidates() && _attackComponent2.CanAttack() && GodotObject.IsInstanceValid(_attackComponent2.target))
			{
				SendStateEvent("ToJump");
			}
			if (!isSpawn && timer <= 0.0 && CanSpawnDancer())
			{
				groundMoveComponent.SetAlive(false);
				sprite.SetAnimation("PointUp1", loop: false);
				isSpawn = true;
				CompleteDancerSpawnPresentationAsync();
			}
		}
	}

	private async Task CompleteDancerSpawnPresentationAsync()
	{
		if (await WaitForStatePhysicsFramesAsync(_runStateHandle, 2))
		{
			GroundMoveComponent groundMoveComponent = base.groundMoveComponent;
			if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
			{
				base.groundMoveComponent.SetAlive(true);
			}
			sprite.AddAnimation("PointDown1", 0.0, loop: false);
			sprite.AddAnimation("DolphinRun", 0.0);
		}
	}

	public void RunExited()
	{
		groundMoveComponent.SetAlive(false);
	}

	public void JumpEntered()
	{
		shadowSprite.Visible = false;
		sprite.SetAnimation("DolphinJump", loop: false, 0.2);
		instance.collisionFlags = 0;
		instance.maskFlags = 0;
	}

	public void JumpProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
		if (!TowerDefenseManager.Instance.backZombie)
		{
			globalPositionForPhysicsFrame.X -= Scale.X * (float)delta * (float)sprite.timeScale * 16f;
			SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
		}
		if (isJump)
		{
			TowerDefenseCharacter towerDefenseCharacter = _attackComponent2?.target;
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && GodotObject.IsInstanceValid(towerDefenseCharacter.instance) && !towerDefenseCharacter.die && !towerDefenseCharacter.nearDie && !towerDefenseCharacter.isDestroy && towerDefenseCharacter.instance.height >= TowerDefenseEnum.CHARACTER_HEIGHT.TALL)
			{
				dolphin = false;
				globalPositionForPhysicsFrame.X = towerDefenseCharacter.GetLogicalGlobalPosition().X + 40f;
				SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
				gridPos = TowerDefenseManager.Instance.GetMapGridPos(globalPositionForPhysicsFrame);
				waterHeight = 48.0;
				groundHeight = 0.0 - waterHeight;
				z = groundHeight;
				spriteGroup.Position = new Vector2(spriteGroup.Position.X, 0f - (float)z);
				sprite.offset = new Vector2(-40f, sprite.offset.Y);
				isBlock = true;
				AudioManager.Instance.AudioPlay("Bonk");
				Walk();
			}
		}
	}

	public void JumpExited()
	{
		isJump = false;
		if (!inWater)
		{
			shadowSprite.Visible = !invisible;
		}
		instance.collisionFlags = 1;
		instance.maskFlags = 1;
	}

	public override void InWater()
	{
		base.InWater();
		if (!dolphin)
		{
			sprite.offset = new Vector2(24f, -97f);
		}
		useAttackDps = true;
	}

	public override void OutWater()
	{
		base.OutWater();
		CreateTween().TweenProperty(sprite, "offset", new Vector2(-40f, -92f), 0.25);
		if (dolphin && !IsRemoteNetworkReplica)
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			logicalGlobalPosition.X -= Scale.X * transformPoint.Scale.X * 30f;
			SetLogicalGlobalPosition(logicalGlobalPosition);
		}
		useAttackDps = !dolphin;
		waterHeight = 0.0;
	}

	public override void DieEntered()
	{
		base.DieEntered();
		sprite.offset = new Vector2(-40f, -92f);
		if (inWater)
		{
			waterHeight = 60.0;
			groundHeight = -60.0;
			z = -60.0;
			Tween tween = CreateTween();
			tween.SetParallel();
			tween.SetEase(Tween.EaseType.Out);
			tween.SetTrans(Tween.TransitionType.Cubic);
			tween.TweenProperty(this, "groundHeight", -100.0, 1.0);
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		switch (command)
		{
		case "hit":
			if (!die && !nearDie && !sprite.pause)
			{
				attackComponent.AttackExecute(((TowerDefenseZombieConfig)config).smashAttack);
			}
			break;
		case "audio":
			AudioManager.Instance.AudioPlay("DolphinreforeJumping");
			break;
		case "check":
			isJump = true;
			break;
		case "jumpOver":
			dolphin = false;
			break;
		case "spawn":
			if (!die && !nearDie)
			{
				SpawnDancer();
				_spotlight.Visible = true;
				_spotlight2.Visible = true;
				ChangeSpotlightColor();
				if (!firstSpawn)
				{
					firstSpawn = true;
					AudioManager.Instance.AudioPlay("Dancer");
				}
			}
			break;
		}
	}

	public void AnimeStarted(string clip)
	{
		if (clip == "DolphinRun")
		{
			sprite.offset = new Vector2(24f, sprite.offset.Y);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		switch (clip)
		{
		case "PointDown1":
			isSpawn = false;
			break;
		case "PointDown2":
		case "PointDown3":
		case "PointDown4":
			isSpawn = false;
			Walk();
			break;
		case "DolphinJump":
			if (isJump)
			{
				jumpMove = true;
				waterHeight = 48.0;
				groundHeight = 0.0 - waterHeight;
				z = groundHeight;
				spriteGroup.Position = new Vector2(spriteGroup.Position.X, 0f - (float)z);
				sprite.offset = new Vector2(-40f, sprite.offset.Y);
				if (!IsRemoteNetworkReplica && !TowerDefenseManager.Instance.backZombie)
				{
					Vector2 logicalGlobalPosition2 = GetLogicalGlobalPosition();
					logicalGlobalPosition2.X -= Scale.X * transformPoint.Scale.X * 98f;
					SetLogicalGlobalPosition(logicalGlobalPosition2);
				}
				sprite.QueueRedraw();
				Walk();
			}
			break;
		case "JumpInWater":
			if (!IsRemoteNetworkReplica && !TowerDefenseManager.Instance.backZombie)
			{
				Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
				logicalGlobalPosition.X -= Scale.X * transformPoint.Scale.X * 64f;
				SetLogicalGlobalPosition(logicalGlobalPosition);
			}
			sprite.offset = new Vector2(24f, sprite.offset.Y);
			break;
		}
	}

	public bool CanSpawnDancer()
	{
		if (!IsInsideComponentBattlefield)
		{
			return false;
		}
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		if (gridPos.Y > 1 && !GodotObject.IsInstanceValid(dancerList[0]))
		{
			return true;
		}
		if (gridPos.Y < mapGridNum.Y && !GodotObject.IsInstanceValid(dancerList[1]))
		{
			return true;
		}
		if (!GodotObject.IsInstanceValid(dancerList[2]))
		{
			return true;
		}
		if (!GodotObject.IsInstanceValid(dancerList[3]))
		{
			return true;
		}
		return false;
	}

	public void SpawnDancer()
	{
		if (!IsInsideComponentBattlefield || !TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(dancerPacketName);
		if (GodotObject.IsInstanceValid(packetConfig) && GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(transformPoint))
		{
			Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
			Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			if (gridPos.Y > 1 && CanReplaceDancer(0))
			{
				SpawnSingleDancer(packetConfig, 0, new Vector2(logicalGlobalPosition.X, (float)TowerDefenseManager.GetMapLineY(gridPos.Y - 1)), gridPos - new Vector2I(0, 1));
			}
			if (gridPos.Y < mapGridNum.Y && CanReplaceDancer(1))
			{
				SpawnSingleDancer(packetConfig, 1, new Vector2(logicalGlobalPosition.X, (float)TowerDefenseManager.GetMapLineY(gridPos.Y + 1)), gridPos + new Vector2I(0, 1));
			}
			if (CanReplaceDancer(2))
			{
				SpawnSingleDancer(packetConfig, 2, logicalGlobalPosition - new Vector2(mapGridSize.X * 1.25f, 0f), gridPos - new Vector2I(1, 0));
			}
			if (CanReplaceDancer(3))
			{
				SpawnSingleDancer(packetConfig, 3, logicalGlobalPosition + new Vector2(mapGridSize.X * 1.25f, 0f), gridPos + new Vector2I(1, 0));
			}
		}
	}

	private bool CanReplaceDancer(int slot)
	{
		TowerDefenseCharacter towerDefenseCharacter = ((dancerList.Count > slot) ? dancerList[slot] : null);
		if (GodotObject.IsInstanceValid(towerDefenseCharacter) && !towerDefenseCharacter.die)
		{
			return towerDefenseCharacter.nearDie;
		}
		return true;
	}

	private void SpawnSingleDancer(TowerDefensePacketConfig packetConfig, int slot, Vector2 position, Vector2I dancerGridPos)
	{
		TowerDefenseCharacter dancer = (EconomyOwnerAccountId.IsValid ? packetConfig.Create(EconomyOwnerAccountId, position, dancerGridPos) : packetConfig.Create(position, dancerGridPos));
		if (!GodotObject.IsInstanceValid(dancer))
		{
			return;
		}
		bool isDolphinDancer = dancerPacketName == "ZombieDolphinDC";
		Dictionary dictionary = CreateDancerSpawnState(slot, isDolphinDancer);
		if (dancer is INetworkSpawnStateReceiver networkSpawnStateReceiver)
		{
			networkSpawnStateReceiver.ImportNetworkSpawnState(dictionary);
		}
		TowerDefenseManager.GetCharacterNode().CallDeferred("add_child", dancer);
		dancer.CallDeferred("SetHitpointAndScale", instance.hitpointScale, transformPoint.Scale);
		dancer.SetDeferred("invisible", invisible);
		if (instance.hypnoses)
		{
			Callable.From(() =>
			{
				if (GodotObject.IsInstanceValid(dancer))
				{
					dancer.Hypnoses();
				}
			}).CallDeferred();
		}
		SetNetworkDancer(slot, dancer);
		TowerDefenseManager.PublishSpawnedCharacter(dancerPacketName, dancer, useCreate: true, 1.5, walkAfterSpawn: false, "", dictionary);
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(dancer))
			{
				if (isDolphinDancer)
				{
					dancer.Rise(1.5, 0.0, createDirt: true, changeState: false);
				}
				else
				{
					dancer.Rise(1.5);
				}
			}
		}).CallDeferred();
	}

	private Dictionary CreateDancerSpawnState(int slot, bool isDolphinDancer)
	{
		Dictionary dictionary = new Dictionary
		{
			["spawn_invisible"] = invisible,
			["dancer_parent_sync_id"] = syncId,
			["dancer_slot"] = slot
		};
		if (isDolphinDancer)
		{
			dictionary["rise_change_state"] = false;
			dictionary["dolphin"] = dolphin;
			dictionary["in_swim_play"] = true;
		}
		return dictionary;
	}

	public void ChangeSpotlightColor()
	{
		Color modulate = spotlightGrandient.Sample(GD.Randf());
		_spotlight.Modulate = modulate;
		_spotlight2.Modulate = modulate;
		GetTree().CreateTimer(3.0, processAlways: false).Timeout += ChangeSpotlightColor;
	}

	public void RemoveDancer(TowerDefenseCharacter dancer)
	{
		int num = dancerList.IndexOf(dancer);
		if (num != -1)
		{
			SetNetworkDancer(num, null);
		}
	}

	public void SetNetworkDancer(int slot, TowerDefenseCharacter dancer)
	{
		if (slot >= 0 && slot < 4)
		{
			if (dancerList.Count != 4)
			{
				dancerList.Resize(4);
			}
			TowerDefenseCharacter towerDefenseCharacter = dancerList[slot];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && towerDefenseCharacter != dancer && towerDefenseCharacter is IDancer dancer2)
			{
				dancer2.SetJackson(null);
			}
			dancerList[slot] = dancer;
			if (GodotObject.IsInstanceValid(dancer) && dancer is IDancer dancer3)
			{
				dancer3.SetJackson(this);
			}
			_pendingDancerSyncIds[slot] = -1;
			_pendingDancerNodeNames[slot] = "";
			RefreshPendingDancerBatchEligibility();
		}
	}

	private void RefreshPendingDancerBatchEligibility()
	{
		bool flag = false;
		for (int i = 0; i < 4; i++)
		{
			if (_pendingDancerSyncIds[i] >= 0 || _pendingDancerNodeNames[i] != "")
			{
				flag = true;
				break;
			}
		}
		if (_hasPendingDancerRelations != flag)
		{
			_hasPendingDancerRelations = flag;
			_pendingDancerResolveRemaining = (flag ? 0.25 : 0.0);
		}
	}

	private void ResolvePendingDancerRelations()
	{
		if (!_hasPendingDancerRelations)
		{
			return;
		}
		if (dancerList.Count != 4)
		{
			dancerList.Resize(4);
		}
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		for (int i = 0; i < 4; i++)
		{
			int num = _pendingDancerSyncIds[i];
			if (num >= 0 && GodotObject.IsInstanceValid(currentControl) && currentControl._syncCharacters.TryGetValue(num, out var value) && GodotObject.IsInstanceValid(value))
			{
				SetNetworkDancer(i, value);
				continue;
			}
			string text = _pendingDancerNodeNames[i];
			if (!(text == "") && GodotObject.IsInstanceValid(node2D))
			{
				TowerDefenseCharacter nodeOrNull = node2D.GetNodeOrNull<TowerDefenseCharacter>(new NodePath(text));
				if (GodotObject.IsInstanceValid(nodeOrNull))
				{
					SetNetworkDancer(i, nodeOrNull);
				}
			}
		}
		_pendingDancerResolveRemaining = (_hasPendingDancerRelations ? 0.25 : 0.0);
	}

	private void ReleaseDancerOwnership()
	{
		for (int i = 0; i < 4; i++)
		{
			SetNetworkDancer(i, null);
		}
		System.Array.Fill(_pendingDancerSyncIds, -1);
		System.Array.Fill(_pendingDancerNodeNames, "");
		RefreshPendingDancerBatchEligibility();
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		ReleaseDancerOwnership();
		dancerList.Clear();
		dancerList.Resize(4);
	}

	public override Dictionary ExportVariantSave()
	{
		Array<string> array = new Array<string>();
		for (int i = 0; i < 4; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = ((dancerList.Count > i) ? dancerList[i] : null);
			array.Add(GodotObject.IsInstanceValid(towerDefenseCharacter) ? towerDefenseCharacter.Name.ToString() : _pendingDancerNodeNames[i]);
		}
		return new Dictionary
		{
			{ "dolphin", dolphin },
			{ "jumpMove", jumpMove },
			{ "isJump", isJump },
			{ "isBlock", isBlock },
			{ "firstSpawn", firstSpawn },
			{ "isSpawn", isSpawn },
			{ "timer", timer },
			{ "dancerNodeNames", array }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		dolphin = data.GetValueOrDefault("dolphin", true).AsBool();
		jumpMove = data.GetValueOrDefault("jumpMove", false).AsBool();
		isJump = data.GetValueOrDefault("isJump", false).AsBool();
		isBlock = data.GetValueOrDefault("isBlock", false).AsBool();
		firstSpawn = data.GetValueOrDefault("firstSpawn", false).AsBool();
		isSpawn = data.GetValueOrDefault("isSpawn", false).AsBool();
		timer = data.GetValueOrDefault("timer", 5.0).AsDouble();
		if (data.ContainsKey("dancerNodeNames"))
		{
			Godot.Collections.Array array = data["dancerNodeNames"].AsGodotArray();
			for (int i = 0; i < 4; i++)
			{
				string text = ((i < array.Count) ? array[i].AsString() : "");
				SetNetworkDancer(i, null);
				_pendingDancerNodeNames[i] = text;
			}
		}
		RefreshPendingDancerBatchEligibility();
		ResolvePendingDancerRelations();
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		Array<int> array = new Array<int>();
		for (int i = 0; i < 4; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = ((dancerList.Count > i) ? dancerList[i] : null);
			array.Add(GodotObject.IsInstanceValid(towerDefenseCharacter) ? towerDefenseCharacter.syncId : _pendingDancerSyncIds[i]);
		}
		return new Dictionary
		{
			["rev"] = GetNetworkSpecialStateRevision(),
			["dancerSyncIds"] = array
		};
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		if (data.ContainsKey("dancerSyncIds"))
		{
			Godot.Collections.Array array = data["dancerSyncIds"].AsGodotArray();
			for (int i = 0; i < 4; i++)
			{
				int num = ((i < array.Count) ? array[i].AsInt32() : (-1));
				SetNetworkDancer(i, null);
				_pendingDancerSyncIds[i] = num;
			}
			RefreshPendingDancerBatchEligibility();
			ResolvePendingDancerRelations();
		}
	}

	public override int GetNetworkSpecialStateRevision()
	{
		int num = 17;
		for (int i = 0; i < 4; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = ((dancerList.Count > i) ? dancerList[i] : null);
			int num2 = (GodotObject.IsInstanceValid(towerDefenseCharacter) ? towerDefenseCharacter.syncId : _pendingDancerSyncIds[i]);
			num = num * 31 + num2;
		}
		return num;
	}

	public override void FinalizeProgressRestore()
	{
		base.FinalizeProgressRestore();
		ResolvePendingDancerRelations();
	}

	public override bool CanBlock()
	{
		return dolphin;
	}

	public override string BlockType()
	{
		return "Jump";
	}

	public override void Block(TowerDefenseCharacter target)
	{
		dolphin = false;
		waterHeight = 48.0;
		groundHeight = 0.0 - waterHeight;
		z = groundHeight;
		spriteGroup.Position = new Vector2(spriteGroup.Position.X, 0f - (float)z);
		sprite.offset = new Vector2(-40f, sprite.offset.Y);
		isBlock = true;
		AudioManager.Instance.AudioPlay("Bonk");
		Walk();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(42)
		{
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.JumpExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeStarted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanSpawnDancer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanReplaceDancer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnSingleDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "dancerGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateDancerSpawnState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isDolphinDancer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChangeSpotlightColor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetNetworkDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPendingDancerBatchEligibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolvePendingDancerRelations, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseDancerOwnership, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
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
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanBlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlockType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Block, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WalkEntered && args.Count == 0)
		{
			WalkEntered();
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
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
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
		if (method == MethodName.JumpEntered && args.Count == 0)
		{
			JumpEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.JumpProcessing && args.Count == 1)
		{
			JumpProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.JumpExited && args.Count == 0)
		{
			JumpExited();
			ret = default;
			return true;
		}
		if (method == MethodName.InWater && args.Count == 0)
		{
			InWater();
			ret = default;
			return true;
		}
		if (method == MethodName.OutWater && args.Count == 0)
		{
			OutWater();
			ret = default;
			return true;
		}
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeStarted && args.Count == 1)
		{
			AnimeStarted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanSpawnDancer && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSpawnDancer());
			return true;
		}
		if (method == MethodName.SpawnDancer && args.Count == 0)
		{
			SpawnDancer();
			ret = default;
			return true;
		}
		if (method == MethodName.CanReplaceDancer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanReplaceDancer(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SpawnSingleDancer && args.Count == 4)
		{
			SpawnSingleDancer(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<Vector2I>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateDancerSpawnState && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreateDancerSpawnState(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ChangeSpotlightColor && args.Count == 0)
		{
			ChangeSpotlightColor();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveDancer && args.Count == 1)
		{
			RemoveDancer(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetNetworkDancer && args.Count == 2)
		{
			SetNetworkDancer(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPendingDancerBatchEligibility && args.Count == 0)
		{
			RefreshPendingDancerBatchEligibility();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePendingDancerRelations && args.Count == 0)
		{
			ResolvePendingDancerRelations();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseDancerOwnership && args.Count == 0)
		{
			ReleaseDancerOwnership();
			ret = default;
			return true;
		}
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
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
		if (method == MethodName.FinalizeProgressRestore && args.Count == 0)
		{
			FinalizeProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.CanBlock && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanBlock());
			return true;
		}
		if (method == MethodName.BlockType && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BlockType());
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.WalkEntered)
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
		if (method == MethodName.Walk)
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
		if (method == MethodName.JumpEntered)
		{
			return true;
		}
		if (method == MethodName.JumpProcessing)
		{
			return true;
		}
		if (method == MethodName.JumpExited)
		{
			return true;
		}
		if (method == MethodName.InWater)
		{
			return true;
		}
		if (method == MethodName.OutWater)
		{
			return true;
		}
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.AnimeStarted)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.CanSpawnDancer)
		{
			return true;
		}
		if (method == MethodName.SpawnDancer)
		{
			return true;
		}
		if (method == MethodName.CanReplaceDancer)
		{
			return true;
		}
		if (method == MethodName.SpawnSingleDancer)
		{
			return true;
		}
		if (method == MethodName.CreateDancerSpawnState)
		{
			return true;
		}
		if (method == MethodName.ChangeSpotlightColor)
		{
			return true;
		}
		if (method == MethodName.RemoveDancer)
		{
			return true;
		}
		if (method == MethodName.SetNetworkDancer)
		{
			return true;
		}
		if (method == MethodName.RefreshPendingDancerBatchEligibility)
		{
			return true;
		}
		if (method == MethodName.ResolvePendingDancerRelations)
		{
			return true;
		}
		if (method == MethodName.ReleaseDancerOwnership)
		{
			return true;
		}
		if (method == MethodName.Hypnoses)
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
		if (method == MethodName.FinalizeProgressRestore)
		{
			return true;
		}
		if (method == MethodName.CanBlock)
		{
			return true;
		}
		if (method == MethodName.BlockType)
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
		if (name == PropertyName.dolphin)
		{
			dolphin = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._spotlight2)
		{
			_spotlight2 = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName._spotlight)
		{
			_spotlight = VariantUtils.ConvertTo<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName.spotlightGrandient)
		{
			spotlightGrandient = VariantUtils.ConvertTo<Gradient>(in value);
			return true;
		}
		if (name == PropertyName._dolphin)
		{
			_dolphin = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.jumpMove)
		{
			jumpMove = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isJump)
		{
			isJump = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isJumpInWater)
		{
			isJumpInWater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isBlock)
		{
			isBlock = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.audioPlay)
		{
			audioPlay = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dancerList)
		{
			dancerList = VariantUtils.ConvertToArray<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._pendingDancerResolveRemaining)
		{
			_pendingDancerResolveRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._hasPendingDancerRelations)
		{
			_hasPendingDancerRelations = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.dancerPacketName)
		{
			dancerPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.firstSpawn)
		{
			firstSpawn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isSpawn)
		{
			isSpawn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.dolphin)
		{
			value = VariantUtils.CreateFrom<bool>(dolphin);
			return true;
		}
		if (name == PropertyName._spotlight2)
		{
			value = VariantUtils.CreateFrom(in _spotlight2);
			return true;
		}
		if (name == PropertyName._spotlight)
		{
			value = VariantUtils.CreateFrom(in _spotlight);
			return true;
		}
		if (name == PropertyName.spotlightGrandient)
		{
			value = VariantUtils.CreateFrom(in spotlightGrandient);
			return true;
		}
		if (name == PropertyName._dolphin)
		{
			value = VariantUtils.CreateFrom(in _dolphin);
			return true;
		}
		if (name == PropertyName.jumpMove)
		{
			value = VariantUtils.CreateFrom(in jumpMove);
			return true;
		}
		if (name == PropertyName.isJump)
		{
			value = VariantUtils.CreateFrom(in isJump);
			return true;
		}
		if (name == PropertyName.isJumpInWater)
		{
			value = VariantUtils.CreateFrom(in isJumpInWater);
			return true;
		}
		if (name == PropertyName.isBlock)
		{
			value = VariantUtils.CreateFrom(in isBlock);
			return true;
		}
		if (name == PropertyName.audioPlay)
		{
			value = VariantUtils.CreateFrom(in audioPlay);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
			return true;
		}
		if (name == PropertyName.dancerList)
		{
			value = VariantUtils.CreateFromArray(dancerList);
			return true;
		}
		if (name == PropertyName._pendingDancerSyncIds)
		{
			value = VariantUtils.CreateFrom(in _pendingDancerSyncIds);
			return true;
		}
		if (name == PropertyName._pendingDancerNodeNames)
		{
			value = VariantUtils.CreateFrom(in _pendingDancerNodeNames);
			return true;
		}
		if (name == PropertyName._pendingDancerResolveRemaining)
		{
			value = VariantUtils.CreateFrom(in _pendingDancerResolveRemaining);
			return true;
		}
		if (name == PropertyName._hasPendingDancerRelations)
		{
			value = VariantUtils.CreateFrom(in _hasPendingDancerRelations);
			return true;
		}
		if (name == PropertyName.dancerPacketName)
		{
			value = VariantUtils.CreateFrom(in dancerPacketName);
			return true;
		}
		if (name == PropertyName.firstSpawn)
		{
			value = VariantUtils.CreateFrom(in firstSpawn);
			return true;
		}
		if (name == PropertyName.isSpawn)
		{
			value = VariantUtils.CreateFrom(in isSpawn);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._spotlight2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spotlight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.spotlightGrandient, PropertyHint.ResourceType, "Gradient", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.dolphin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dolphin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.jumpMove, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isJump, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isJumpInWater, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isBlock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.audioPlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.dancerList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._pendingDancerSyncIds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._pendingDancerNodeNames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingDancerResolveRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPendingDancerRelations, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.dancerPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.firstSpawn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isSpawn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dolphin, Variant.From<bool>(dolphin));
		info.AddProperty(PropertyName._spotlight2, Variant.From(in _spotlight2));
		info.AddProperty(PropertyName._spotlight, Variant.From(in _spotlight));
		info.AddProperty(PropertyName.spotlightGrandient, Variant.From(in spotlightGrandient));
		info.AddProperty(PropertyName._dolphin, Variant.From(in _dolphin));
		info.AddProperty(PropertyName.jumpMove, Variant.From(in jumpMove));
		info.AddProperty(PropertyName.isJump, Variant.From(in isJump));
		info.AddProperty(PropertyName.isJumpInWater, Variant.From(in isJumpInWater));
		info.AddProperty(PropertyName.isBlock, Variant.From(in isBlock));
		info.AddProperty(PropertyName.audioPlay, Variant.From(in audioPlay));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName.dancerList, Variant.CreateFrom(dancerList));
		info.AddProperty(PropertyName._pendingDancerResolveRemaining, Variant.From(in _pendingDancerResolveRemaining));
		info.AddProperty(PropertyName._hasPendingDancerRelations, Variant.From(in _hasPendingDancerRelations));
		info.AddProperty(PropertyName.dancerPacketName, Variant.From(in dancerPacketName));
		info.AddProperty(PropertyName.firstSpawn, Variant.From(in firstSpawn));
		info.AddProperty(PropertyName.isSpawn, Variant.From(in isSpawn));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dolphin, out var value))
		{
			dolphin = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._spotlight2, out var value2))
		{
			_spotlight2 = value2.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName._spotlight, out var value3))
		{
			_spotlight = value3.As<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName.spotlightGrandient, out var value4))
		{
			spotlightGrandient = value4.As<Gradient>();
		}
		if (info.TryGetProperty(PropertyName._dolphin, out var value5))
		{
			_dolphin = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.jumpMove, out var value6))
		{
			jumpMove = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isJump, out var value7))
		{
			isJump = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isJumpInWater, out var value8))
		{
			isJumpInWater = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isBlock, out var value9))
		{
			isBlock = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.audioPlay, out var value10))
		{
			audioPlay = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value11))
		{
			timer = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dancerList, out var value12))
		{
			dancerList = value12.AsGodotArray<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._pendingDancerResolveRemaining, out var value13))
		{
			_pendingDancerResolveRemaining = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName._hasPendingDancerRelations, out var value14))
		{
			_hasPendingDancerRelations = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dancerPacketName, out var value15))
		{
			dancerPacketName = value15.As<string>();
		}
		if (info.TryGetProperty(PropertyName.firstSpawn, out var value16))
		{
			firstSpawn = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isSpawn, out var value17))
		{
			isSpawn = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value18))
		{
			_roleStateSignalsConnected = value18.As<bool>();
		}
	}
}
