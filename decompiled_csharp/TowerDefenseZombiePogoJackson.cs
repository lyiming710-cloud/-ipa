using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter5/PogoJackson/Scene/TowerDefenseZombiePogoJackson.cs")]
public class TowerDefenseZombiePogoJackson : TowerDefenseZombie, IJackson, INetworkDancerOwner
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName RemoveDancer = "RemoveDancer";

		public static readonly StringName IsSameDancer = "IsSameDancer";

		public static readonly StringName SetNetworkDancer = "SetNetworkDancer";

		public static readonly StringName GetDancer = "GetDancer";

		public static readonly StringName RefreshPendingDancerBatchEligibility = "RefreshPendingDancerBatchEligibility";

		public static readonly StringName ResolvePendingDancerRelations = "ResolvePendingDancerRelations";

		public static readonly StringName ReleaseDancerOwnership = "ReleaseDancerOwnership";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName PogoEntered = "PogoEntered";

		public static readonly StringName PogoProcessing = "PogoProcessing";

		public static readonly StringName PogoExited = "PogoExited";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName SpawnDancer = "SpawnDancer";

		public static readonly StringName SpawnSingleDancer = "SpawnSingleDancer";

		public static readonly StringName CreateDancerSpawnState = "CreateDancerSpawnState";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public static readonly StringName Land = "Land";

		public new static readonly StringName CanBlock = "CanBlock";

		public new static readonly StringName BlockType = "BlockType";

		public new static readonly StringName Block = "Block";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName HasDancingRuntime = "HasDancingRuntime";

		public static readonly StringName HasDancingComponent = "HasDancingComponent";

		public static readonly StringName dancerPacketName = "dancerPacketName";

		public static readonly StringName hasPogo = "hasPogo";

		public static readonly StringName _dancerPacketName = "_dancerPacketName";

		public static readonly StringName _pendingDancerSyncIds = "_pendingDancerSyncIds";

		public static readonly StringName _pendingDancerNodeNames = "_pendingDancerNodeNames";

		public static readonly StringName _pendingDancerResolveRemaining = "_pendingDancerResolveRemaining";

		public static readonly StringName _hasPendingDancerRelations = "_hasPendingDancerRelations";

		public static readonly StringName isJump = "isJump";

		public static readonly StringName _hasPogo = "_hasPogo";

		public static readonly StringName pogoPlant = "pogoPlant";

		public static readonly StringName jumpToPos = "jumpToPos";

		public static readonly StringName jumpWait = "jumpWait";

		public static readonly StringName isSpawn = "isSpawn";

		public static readonly StringName timer = "timer";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const double PendingDancerResolveIntervalSeconds = 0.25;

	private AttackComponent _attackComponent2;

	private DancingComponent _dancingComponent;

	private string _dancerPacketName = "";

	private readonly int[] _pendingDancerSyncIds = new int[4] { -1, -1, -1, -1 };

	private readonly string[] _pendingDancerNodeNames = new string[4] { "", "", "", "" };

	private double _pendingDancerResolveRemaining;

	private bool _hasPendingDancerRelations;

	public bool isJump;

	private bool _hasPogo = true;

	public bool pogoPlant;

	public double jumpToPos;

	public int jumpWait = 1;

	public bool isSpawn;

	public double timer = 5.0;

	private StateHandle _pogoStateHandle;

	private bool _roleStateSignalsConnected;

	private bool HasDancingRuntime
	{
		get
		{
			DancingComponent dancingComponent = _dancingComponent;
			if (dancingComponent == null)
			{
				return false;
			}
			return !dancingComponent.IsReleased;
		}
	}

	private bool HasDancingComponent
	{
		get
		{
			if (HasDancingRuntime)
			{
				return _dancingComponent.Lifecycle == ComponentRuntimeLifecycle.Active;
			}
			return false;
		}
	}

	[Export(PropertyHint.None, "")]
	public string dancerPacketName
	{
		get
		{
			return _dancerPacketName;
		}
		set
		{
			_dancerPacketName = value;
			if (!string.IsNullOrEmpty(value) && HasDancingComponent)
			{
				_dancingComponent.dancerPacketName = value;
			}
		}
	}

	public bool hasPogo
	{
		get
		{
			return _hasPogo;
		}
		set
		{
			_hasPogo = value;
			if (!_hasPogo)
			{
				idleAnimeClip = "Walk";
			}
		}
	}

	public void RemoveDancer(TowerDefenseCharacter dancer)
	{
		for (int i = 0; i < 4; i++)
		{
			bool flag = GodotObject.IsInstanceValid(dancer) && dancer.syncId >= 0 && _pendingDancerSyncIds[i] == dancer.syncId;
			if (IsSameDancer(GetDancer(i), dancer) || flag)
			{
				SetNetworkDancer(i, null);
				break;
			}
		}
	}

	private static bool IsSameDancer(TowerDefenseCharacter candidate, TowerDefenseCharacter dancer)
	{
		if (candidate != dancer)
		{
			if (GodotObject.IsInstanceValid(candidate) && GodotObject.IsInstanceValid(dancer) && candidate.syncId >= 0)
			{
				return candidate.syncId == dancer.syncId;
			}
			return false;
		}
		return true;
	}

	public void SetNetworkDancer(int slot, TowerDefenseCharacter dancer)
	{
		if (slot < 0 || slot >= 4)
		{
			return;
		}
		if (!HasDancingRuntime)
		{
			if (GodotObject.IsInstanceValid(dancer))
			{
				_pendingDancerSyncIds[slot] = dancer.syncId;
				_pendingDancerNodeNames[slot] = "";
			}
			else
			{
				_pendingDancerSyncIds[slot] = -1;
				_pendingDancerNodeNames[slot] = "";
			}
			RefreshPendingDancerBatchEligibility();
		}
		else
		{
			_dancingComponent.SetNetworkDancer(slot, dancer);
			_pendingDancerSyncIds[slot] = -1;
			_pendingDancerNodeNames[slot] = "";
			RefreshPendingDancerBatchEligibility();
		}
	}

	private TowerDefenseCharacter GetDancer(int slot)
	{
		if (!HasDancingRuntime)
		{
			return null;
		}
		return _dancingComponent.GetDancer(slot);
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
		if (!HasDancingRuntime)
		{
			_pendingDancerResolveRemaining = 0.25;
			return;
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
		if (HasDancingRuntime)
		{
			for (int i = 0; i < 4; i++)
			{
				_dancingComponent.SetNetworkDancer(i, null);
			}
		}
		System.Array.Fill(_pendingDancerSyncIds, -1);
		System.Array.Fill(_pendingDancerNodeNames, "");
		RefreshPendingDancerBatchEligibility();
	}

	private void ConnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_pogoStateHandle = StateMachine?.GetStateById("zombie.pogo_jackson.pogo");
			StateHandle pogoStateHandle = _pogoStateHandle;
			if (pogoStateHandle != null && pogoStateHandle.IsValid)
			{
				_pogoStateHandle.Entered += PogoEntered;
				_pogoStateHandle.Exited += PogoExited;
				_pogoStateHandle.PhysicsProcessing += PogoProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			if (_pogoStateHandle != null)
			{
				_pogoStateHandle.Entered -= PogoEntered;
				_pogoStateHandle.Exited -= PogoExited;
				_pogoStateHandle.PhysicsProcessing -= PogoProcessing;
			}
			_pogoStateHandle = null;
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
		if (Engine.IsEditorHint())
		{
			return;
		}
		OnLand += Land;
		_attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
		_dancingComponent = componentManager.GetRuntime<DancingComponent>();
		if (HasDancingComponent)
		{
			sprite.OnAnimeCompleted -= _dancingComponent.AnimeCompleted;
			sprite.OnAnimeEvent -= _dancingComponent.AnimeEvent;
			if (!string.IsNullOrEmpty(dancerPacketName))
			{
				_dancingComponent.dancerPacketName = dancerPacketName;
			}
			else
			{
				_dancingComponent.dancerPacketName = "ZombiePogoDancer";
			}
		}
		ySpeed = -300.0;
		ConnectRoleStateSignals();
		RefreshPendingDancerBatchEligibility();
		if (_hasPendingDancerRelations)
		{
			ResolvePendingDancerRelations();
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

	public void PogoEntered()
	{
		sprite.SetAnimation("Pogo", loop: true, 0.2);
		if (isGround)
		{
			ySpeed = -300.0;
		}
	}

	public void PogoProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
		if (!pogoPlant)
		{
			if (!sprite.pause)
			{
				double num = (((double)globalPositionForPhysicsFrame.X > groundRight) ? 2.0 : 1.0);
				globalPositionForPhysicsFrame.X -= (float)(30.0 * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)Scale.X * num * (double)((!sprite.playBack) ? 1 : (-1)));
				SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
			}
			if (!sprite.pause && attackComponent.CanAttack())
			{
				pogoPlant = true;
				jumpToPos = globalPositionForPhysicsFrame.X - TowerDefenseManager.Instance.GetMapGridSize().X * Scale.X - 10f * Scale.X;
			}
		}
		else if (isJump && _attackComponent2.HasAttackGridTargetCandidates() && _attackComponent2.CanAttack() && GodotObject.IsInstanceValid(_attackComponent2.target) && _attackComponent2.target.instance.height >= TowerDefenseEnum.CHARACTER_HEIGHT.TALL)
		{
			instance.ArmorDelete("Pogo");
			hasPogo = false;
			isJump = false;
			AudioManager.Instance.AudioPlay("Bonk");
			Walk();
		}
		if (!isSpawn && timer <= 0.0 && HasDancingComponent && _dancingComponent.CanSpawnDancer())
		{
			sprite.SetAnimation("PogoPointUp", loop: false);
			sprite.AddAnimation("PogoPointDown", 0.0, loop: false);
			sprite.AddAnimation("Pogo", 0.0, loop: false);
			isSpawn = true;
		}
	}

	public void PogoExited()
	{
	}

	public override void WalkEntered()
	{
		base.WalkEntered();
		if (HasDancingComponent)
		{
			_dancingComponent.OnWalkEntered();
		}
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		if (HasDancingComponent)
		{
			_dancingComponent.OnAttackProcessing(delta);
		}
	}

	public override void Walk()
	{
		if (hasPogo)
		{
			SendStateEvent("ToPogo");
		}
		else if (die)
		{
			SendStateEvent("ToDie");
		}
		else if (!HasDancingComponent || !_dancingComponent.OnWalk())
		{
			SendStateEvent("ToWalk");
		}
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		if (HasDancingComponent)
		{
			_dancingComponent.OnDieProcessing();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!inGame || !TowerDefenseManager.Instance.IsGameRunning() || !HasDancingComponent)
		{
			return;
		}
		if (clip == _dancingComponent.pointDownAnimeClip)
		{
			isSpawn = false;
			Walk();
			_dancingComponent.RequestIdle();
		}
		else if (clip == _dancingComponent.walkAnimeClip)
		{
			_dancingComponent.walkTime--;
			if (!die && !nearDie)
			{
				if (_dancingComponent.walkTime <= 0)
				{
					if (_dancingComponent.CanSpawnDancer())
					{
						Component();
						_dancingComponent.RequestPoint();
					}
					else
					{
						Component();
						_dancingComponent.RequestDance();
					}
				}
			}
			else
			{
				Die();
			}
		}
		else
		{
			if (!(clip == _dancingComponent.armRiseAnimeClip))
			{
				return;
			}
			if (_dancingComponent.armRiseFlipSprite)
			{
				sprite.Scale = new Vector2(0f - sprite.Scale.X, sprite.Scale.Y);
			}
			_dancingComponent.danceTime--;
			if (!die && !nearDie)
			{
				if (_dancingComponent.danceTime <= 0)
				{
					if (_dancingComponent.CanSpawnDancer())
					{
						Component();
						_dancingComponent.RequestPoint();
					}
					else
					{
						Walk();
						_dancingComponent.RequestIdle();
					}
				}
			}
			else
			{
				Die();
			}
		}
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Pogo")
		{
			instance.unUseBuffFlags = 0;
			isJump = false;
			hasPogo = false;
			GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
			if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
			{
				base.groundHeightComponent.handleWaterHeight = true;
			}
			if (inWater)
			{
				groundHeight = 0.0 - waterHeight;
			}
			Walk();
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (!HasDancingComponent || !(command == "spawn") || die || nearDie)
		{
			return;
		}
		SpawnDancer();
		if (GodotObject.IsInstanceValid(_dancingComponent.spotlight))
		{
			_dancingComponent.spotlight.Visible = true;
		}
		if (GodotObject.IsInstanceValid(_dancingComponent.spotlight2))
		{
			_dancingComponent.spotlight2.Visible = true;
		}
		_dancingComponent.ChangeSpotlightColor();
		if (!_dancingComponent.firstSpawn)
		{
			_dancingComponent.firstSpawn = true;
			if (_dancingComponent.spotlightAudioName != "")
			{
				AudioManager.Instance.AudioPlay(_dancingComponent.spotlightAudioName);
			}
		}
	}

	public void SpawnDancer()
	{
		if (!IsInsideComponentBattlefield || !TowerDefenseManager.HasGameplayAuthority || !HasDancingComponent)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(_dancingComponent.dancerPacketName);
		if (GodotObject.IsInstanceValid(packetConfig) && GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(transformPoint))
		{
			Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
			Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			if (gridPos.Y > 1 && !GodotObject.IsInstanceValid(_dancingComponent.GetDancer(0)))
			{
				SpawnSingleDancer(packetConfig, 0, new Vector2(logicalGlobalPosition.X, (float)TowerDefenseManager.GetMapLineY(gridPos.Y - 1)), gridPos - new Vector2I(0, 1));
			}
			if (gridPos.Y < mapGridNum.Y && !GodotObject.IsInstanceValid(_dancingComponent.GetDancer(1)))
			{
				SpawnSingleDancer(packetConfig, 1, new Vector2(logicalGlobalPosition.X, (float)TowerDefenseManager.GetMapLineY(gridPos.Y + 1)), gridPos + new Vector2I(0, 1));
			}
			if (!GodotObject.IsInstanceValid(_dancingComponent.GetDancer(2)))
			{
				SpawnSingleDancer(packetConfig, 2, logicalGlobalPosition - new Vector2(mapGridSize.X * 1.25f, 0f), gridPos - new Vector2I(1, 0));
			}
			if (!GodotObject.IsInstanceValid(_dancingComponent.GetDancer(3)))
			{
				SpawnSingleDancer(packetConfig, 3, logicalGlobalPosition + new Vector2(mapGridSize.X * 1.25f, 0f), gridPos + new Vector2I(1, 0));
			}
		}
	}

	private void SpawnSingleDancer(TowerDefensePacketConfig packetConfig, int slot, Vector2 position, Vector2I dancerGridPos)
	{
		TowerDefenseCharacter dancer = (EconomyOwnerAccountId.IsValid ? packetConfig.Create(EconomyOwnerAccountId, position, dancerGridPos) : packetConfig.Create(position, dancerGridPos));
		if (!GodotObject.IsInstanceValid(dancer))
		{
			return;
		}
		Dictionary dictionary = CreateDancerSpawnState(slot);
		if (dancer is INetworkSpawnStateReceiver networkSpawnStateReceiver)
		{
			networkSpawnStateReceiver.ImportNetworkSpawnState(dictionary);
		}
		TowerDefenseGroundItemBase.characterNode.CallDeferred("add_child", dancer);
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
		TowerDefenseManager.PublishSpawnedCharacter(_dancingComponent.dancerPacketName, dancer, useCreate: true, 0.0, walkAfterSpawn: false, "", dictionary);
		if (dancer is TowerDefenseZombie towerDefenseZombie)
		{
			towerDefenseZombie.CallDeferred("WalkReady");
		}
	}

	private Dictionary CreateDancerSpawnState(int slot)
	{
		return new Dictionary
		{
			["spawn_invisible"] = invisible,
			["spawn_z"] = 600.0,
			["walk_ready_after_spawn"] = true,
			["dancer_parent_sync_id"] = syncId,
			["dancer_slot"] = slot
		};
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		ReleaseDancerOwnership();
	}

	public override Dictionary ExportVariantSave()
	{
		Array<string> array = new Array<string>();
		for (int i = 0; i < 4; i++)
		{
			TowerDefenseCharacter dancer = GetDancer(i);
			array.Add(GodotObject.IsInstanceValid(dancer) ? dancer.Name.ToString() : _pendingDancerNodeNames[i]);
		}
		return new Dictionary
		{
			{ "hasPogo", hasPogo },
			{ "isJump", isJump },
			{ "pogoPlant", pogoPlant },
			{ "jumpToPos", jumpToPos },
			{ "jumpWait", jumpWait },
			{ "isSpawn", isSpawn },
			{ "timer", timer },
			{ "dancerNodeNames", array }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		hasPogo = data.GetValueOrDefault("hasPogo", true).AsBool();
		isJump = data.GetValueOrDefault("isJump", false).AsBool();
		pogoPlant = data.GetValueOrDefault("pogoPlant", false).AsBool();
		jumpToPos = data.GetValueOrDefault("jumpToPos", 0.0).AsDouble();
		jumpWait = data.GetValueOrDefault("jumpWait", 1).AsInt32();
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
		if (_hasPendingDancerRelations)
		{
			ResolvePendingDancerRelations();
		}
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		Array<int> array = new Array<int>();
		for (int i = 0; i < 4; i++)
		{
			TowerDefenseCharacter dancer = GetDancer(i);
			array.Add(GodotObject.IsInstanceValid(dancer) ? dancer.syncId : _pendingDancerSyncIds[i]);
		}
		return new Dictionary
		{
			["rev"] = GetNetworkSpecialStateRevision(),
			["dancerSyncIds"] = array
		};
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		if (!data.ContainsKey("dancerSyncIds"))
		{
			return;
		}
		Godot.Collections.Array array = data["dancerSyncIds"].AsGodotArray();
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		for (int i = 0; i < 4; i++)
		{
			int num = ((i < array.Count) ? array[i].AsInt32() : (-1));
			if (num < 0)
			{
				SetNetworkDancer(i, null);
				continue;
			}
			if (GodotObject.IsInstanceValid(currentControl) && currentControl._syncCharacters.TryGetValue(num, out var value) && GodotObject.IsInstanceValid(value))
			{
				SetNetworkDancer(i, value);
				continue;
			}
			SetNetworkDancer(i, null);
			_pendingDancerSyncIds[i] = num;
			_pendingDancerNodeNames[i] = "";
		}
		RefreshPendingDancerBatchEligibility();
		if (_hasPendingDancerRelations)
		{
			ResolvePendingDancerRelations();
		}
	}

	public override int GetNetworkSpecialStateRevision()
	{
		int num = 17;
		for (int i = 0; i < 4; i++)
		{
			TowerDefenseCharacter dancer = GetDancer(i);
			int num2 = (GodotObject.IsInstanceValid(dancer) ? dancer.syncId : _pendingDancerSyncIds[i]);
			num = num * 31 + num2;
		}
		return num;
	}

	public override void FinalizeProgressRestore()
	{
		base.FinalizeProgressRestore();
		RefreshPendingDancerBatchEligibility();
		if (_hasPendingDancerRelations)
		{
			ResolvePendingDancerRelations();
		}
	}

	public override void InWater()
	{
		base.InWater();
		if (hasPogo)
		{
			groundHeight = 0.0;
			GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
			if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
			{
				base.groundHeightComponent.handleWaterHeight = false;
			}
		}
	}

	public override void OutWater()
	{
		if (hasPogo)
		{
			GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
			if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
			{
				base.groundHeightComponent.handleWaterHeight = true;
			}
		}
		base.OutWater();
		if (hasPogo)
		{
			ySpeed = -300.0;
		}
	}

	public async void Land()
	{
		if (!hasPogo)
		{
			return;
		}
		isJump = false;
		gravity = 490.0;
		ySpeed = -300.0;
		if (!IsRemoteNetworkReplica && pogoPlant)
		{
			isJump = true;
			if (jumpWait > 0)
			{
				jumpWait--;
			}
			else
			{
				jumpWait = 1;
				Tween tween = CreateTween();
				tween.SetEase(Tween.EaseType.InOut);
				tween.SetTrans(Tween.TransitionType.Sine);
				Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
				tween.TweenMethod(to: new Vector2((float)jumpToPos, logicalGlobalPosition.Y), method: Callable.From<Vector2>(SetLogicalGlobalPosition), from: logicalGlobalPosition, duration: 0.5);
				ySpeed = -400.0;
				await ToSignal(tween, Tween.SignalName.Finished);
				pogoPlant = false;
			}
			await ToSignal(GetTree().CreateTimer(0.2, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			isJump = false;
		}
	}

	public override bool CanBlock()
	{
		return hasPogo;
	}

	public override string BlockType()
	{
		return "Jump";
	}

	public override void Block(TowerDefenseCharacter target)
	{
		instance.ArmorDelete("Pogo");
		hasPogo = false;
		isJump = false;
		GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
		if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
		{
			base.groundHeightComponent.handleWaterHeight = true;
		}
		if (inWater)
		{
			groundHeight = 0.0 - waterHeight;
		}
		AudioManager.Instance.AudioPlay("Bonk");
		Walk();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(39)
		{
			new MethodInfo(MethodName.RemoveDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsSameDancer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "candidate", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetNetworkDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetDancer, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPendingDancerBatchEligibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolvePendingDancerRelations, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseDancerOwnership, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PogoEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PogoProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PogoExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnSingleDancer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "dancerGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateDancerSpawnState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Land, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.RemoveDancer && args.Count == 1)
		{
			RemoveDancer(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSameDancer && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSameDancer(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		if (method == MethodName.SetNetworkDancer && args.Count == 2)
		{
			SetNetworkDancer(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDancer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetDancer(VariantUtils.ConvertTo<int>(in args[0])));
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
		if (method == MethodName.PogoEntered && args.Count == 0)
		{
			PogoEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.PogoProcessing && args.Count == 1)
		{
			PogoProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PogoExited && args.Count == 0)
		{
			PogoExited();
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
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnDancer && args.Count == 0)
		{
			SpawnDancer();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnSingleDancer && args.Count == 4)
		{
			SpawnSingleDancer(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<Vector2I>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateDancerSpawnState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CreateDancerSpawnState(VariantUtils.ConvertTo<int>(in args[0])));
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
		if (method == MethodName.Land && args.Count == 0)
		{
			Land();
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
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsSameDancer && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSameDancer(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.RemoveDancer)
		{
			return true;
		}
		if (method == MethodName.IsSameDancer)
		{
			return true;
		}
		if (method == MethodName.SetNetworkDancer)
		{
			return true;
		}
		if (method == MethodName.GetDancer)
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
		if (method == MethodName.PogoEntered)
		{
			return true;
		}
		if (method == MethodName.PogoProcessing)
		{
			return true;
		}
		if (method == MethodName.PogoExited)
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
		if (method == MethodName.DieProcessing)
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
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.SpawnDancer)
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
		if (method == MethodName.InWater)
		{
			return true;
		}
		if (method == MethodName.OutWater)
		{
			return true;
		}
		if (method == MethodName.Land)
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
		if (name == PropertyName.dancerPacketName)
		{
			dancerPacketName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.hasPogo)
		{
			hasPogo = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dancerPacketName)
		{
			_dancerPacketName = VariantUtils.ConvertTo<string>(in value);
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
		if (name == PropertyName.isJump)
		{
			isJump = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasPogo)
		{
			_hasPogo = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.pogoPlant)
		{
			pogoPlant = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.jumpToPos)
		{
			jumpToPos = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.jumpWait)
		{
			jumpWait = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.isSpawn)
		{
			isSpawn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.timer)
		{
			timer = VariantUtils.ConvertTo<double>(in value);
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
		bool from;
		if (name == PropertyName.HasDancingRuntime)
		{
			from = HasDancingRuntime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasDancingComponent)
		{
			from = HasDancingComponent;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.dancerPacketName)
		{
			value = VariantUtils.CreateFrom<string>(dancerPacketName);
			return true;
		}
		if (name == PropertyName.hasPogo)
		{
			from = hasPogo;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._dancerPacketName)
		{
			value = VariantUtils.CreateFrom(in _dancerPacketName);
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
		if (name == PropertyName.isJump)
		{
			value = VariantUtils.CreateFrom(in isJump);
			return true;
		}
		if (name == PropertyName._hasPogo)
		{
			value = VariantUtils.CreateFrom(in _hasPogo);
			return true;
		}
		if (name == PropertyName.pogoPlant)
		{
			value = VariantUtils.CreateFrom(in pogoPlant);
			return true;
		}
		if (name == PropertyName.jumpToPos)
		{
			value = VariantUtils.CreateFrom(in jumpToPos);
			return true;
		}
		if (name == PropertyName.jumpWait)
		{
			value = VariantUtils.CreateFrom(in jumpWait);
			return true;
		}
		if (name == PropertyName.isSpawn)
		{
			value = VariantUtils.CreateFrom(in isSpawn);
			return true;
		}
		if (name == PropertyName.timer)
		{
			value = VariantUtils.CreateFrom(in timer);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasDancingRuntime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasDancingComponent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.dancerPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._dancerPacketName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._pendingDancerSyncIds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._pendingDancerNodeNames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingDancerResolveRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPendingDancerRelations, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isJump, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPogo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasPogo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pogoPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpToPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.jumpWait, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isSpawn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dancerPacketName, Variant.From<string>(dancerPacketName));
		info.AddProperty(PropertyName.hasPogo, Variant.From<bool>(hasPogo));
		info.AddProperty(PropertyName._dancerPacketName, Variant.From(in _dancerPacketName));
		info.AddProperty(PropertyName._pendingDancerResolveRemaining, Variant.From(in _pendingDancerResolveRemaining));
		info.AddProperty(PropertyName._hasPendingDancerRelations, Variant.From(in _hasPendingDancerRelations));
		info.AddProperty(PropertyName.isJump, Variant.From(in isJump));
		info.AddProperty(PropertyName._hasPogo, Variant.From(in _hasPogo));
		info.AddProperty(PropertyName.pogoPlant, Variant.From(in pogoPlant));
		info.AddProperty(PropertyName.jumpToPos, Variant.From(in jumpToPos));
		info.AddProperty(PropertyName.jumpWait, Variant.From(in jumpWait));
		info.AddProperty(PropertyName.isSpawn, Variant.From(in isSpawn));
		info.AddProperty(PropertyName.timer, Variant.From(in timer));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dancerPacketName, out var value))
		{
			dancerPacketName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.hasPogo, out var value2))
		{
			hasPogo = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dancerPacketName, out var value3))
		{
			_dancerPacketName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pendingDancerResolveRemaining, out var value4))
		{
			_pendingDancerResolveRemaining = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._hasPendingDancerRelations, out var value5))
		{
			_hasPendingDancerRelations = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isJump, out var value6))
		{
			isJump = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasPogo, out var value7))
		{
			_hasPogo = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.pogoPlant, out var value8))
		{
			pogoPlant = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.jumpToPos, out var value9))
		{
			jumpToPos = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.jumpWait, out var value10))
		{
			jumpWait = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.isSpawn, out var value11))
		{
			isSpawn = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.timer, out var value12))
		{
			timer = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value13))
		{
			_roleStateSignalsConnected = value13.As<bool>();
		}
	}
}
