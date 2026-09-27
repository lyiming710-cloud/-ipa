using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Boss/Boss/Scene/TowerDefenseZombieBoss.cs")]
public class TowerDefenseZombieBoss : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ResolveBossRuntimeComponents = "ResolveBossRuntimeComponents";

		public static readonly StringName InitializeBossGameplay = "InitializeBossGameplay";

		public static readonly StringName IsBossGameplayPaused = "IsBossGameplayPaused";

		public static readonly StringName ShouldFreezeBossGameplay = "ShouldFreezeBossGameplay";

		public static readonly StringName ApplyBossGlobalPausePlayback = "ApplyBossGlobalPausePlayback";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName Walk = "Walk";

		public static readonly StringName HeadIdleEntered = "HeadIdleEntered";

		public static readonly StringName HeadIdleProcessing = "HeadIdleProcessing";

		public static readonly StringName HeadIdleExited = "HeadIdleExited";

		public static readonly StringName HeadAttackEntered = "HeadAttackEntered";

		public static readonly StringName HeadAttackProcessing = "HeadAttackProcessing";

		public static readonly StringName HeadAttackExited = "HeadAttackExited";

		public static readonly StringName HeadExitedEntered = "HeadExitedEntered";

		public static readonly StringName HeadExitedProcessing = "HeadExitedProcessing";

		public static readonly StringName HeadExitedExited = "HeadExitedExited";

		public static readonly StringName SpawnEntered = "SpawnEntered";

		public static readonly StringName SpawnProcessing = "SpawnProcessing";

		public static readonly StringName SpawnExited = "SpawnExited";

		public static readonly StringName StompEntered = "StompEntered";

		public static readonly StringName StompProcessing = "StompProcessing";

		public static readonly StringName StompExited = "StompExited";

		public static readonly StringName BungeeEntered = "BungeeEntered";

		public static readonly StringName BungeeProcessing = "BungeeProcessing";

		public static readonly StringName BungeeExited = "BungeeExited";

		public static readonly StringName RVEntered = "RVEntered";

		public static readonly StringName RVProcessing = "RVProcessing";

		public static readonly StringName RVExited = "RVExited";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName BeginHeadAttackPause = "BeginHeadAttackPause";

		public static readonly StringName UpdateHeadAttackPause = "UpdateHeadAttackPause";

		public static readonly StringName ReleaseHeadAttackPause = "ReleaseHeadAttackPause";

		public static readonly StringName StateRunning = "StateRunning";

		public static readonly StringName ZombieSpawn = "ZombieSpawn";

		public static readonly StringName BungeeSpawn = "BungeeSpawn";

		public static readonly StringName RVSpawn = "RVSpawn";

		public static readonly StringName BallSpawn = "BallSpawn";

		public static readonly StringName CreateBallAt = "CreateBallAt";

		public static readonly StringName GetSpawnZombie = "GetSpawnZombie";

		public static readonly StringName SetStateList = "SetStateList";

		public static readonly StringName ExplosionMethod = "ExplosionMethod";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName InWaterDiscardSet = "InWaterDiscardSet";

		public new static readonly StringName OutWaterDiscardSet = "OutWaterDiscardSet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _bossGameplayInitialized = "_bossGameplayInitialized";

		public static readonly StringName stageSpawnList = "stageSpawnList";

		public static readonly StringName stateMethodList = "stateMethodList";

		public static readonly StringName useEnterAnime = "useEnterAnime";

		public static readonly StringName stateNow = "stateNow";

		public static readonly StringName spawnZombieNumOnce = "spawnZombieNumOnce";

		public static readonly StringName spawnZombieNumNow = "spawnZombieNumNow";

		public static readonly StringName spawnTime = "spawnTime";

		public static readonly StringName spawnLine = "spawnLine";

		public static readonly StringName spawnRestTime = "spawnRestTime";

		public static readonly StringName spawnRestTimer = "spawnRestTimer";

		public static readonly StringName spawnZomie = "spawnZomie";

		public static readonly StringName spawnNum = "spawnNum";

		public static readonly StringName headAttackOver = "headAttackOver";

		public static readonly StringName headAttackLine = "headAttackLine";

		public static readonly StringName headAttackIsFire = "headAttackIsFire";

		public static readonly StringName headAttackRestTime = "headAttackRestTime";

		public static readonly StringName headAttackRestTimer = "headAttackRestTimer";

		public static readonly StringName bungeeIsSpawn = "bungeeIsSpawn";

		public static readonly StringName bungeeList = "bungeeList";

		public static readonly StringName stompId = "stompId";

		public static readonly StringName rvPos = "rvPos";

		public static readonly StringName isRest = "isRest";

		public static readonly StringName restTime = "restTime";

		public static readonly StringName restTimer = "restTimer";

		public static readonly StringName headIdleNum = "headIdleNum";

		public static readonly StringName stage = "stage";

		public static readonly StringName _headAttackPauseRemaining = "_headAttackPauseRemaining";

		public static readonly StringName _headAttackPauseOwned = "_headAttackPauseOwned";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private StateHandle _headIdleStateHandle;

	private StateHandle _headAttackStateHandle;

	private StateHandle _headExitedStateHandle;

	private StateHandle _spawnStateHandle;

	private StateHandle _stompStateHandle;

	private StateHandle _bungeeStateHandle;

	private StateHandle _rvStateHandle;

	private bool _stateSignalsConnected;

	private bool _bossGameplayInitialized;

	private static PackedScene _FIRE_BALL;

	private static PackedScene _ICE_BALL;

	private static PackedScene _EXPLOSION;

	private AttackComponent _attackComponent1;

	private AttackComponent _attackComponent2;

	private AttackComponent _attackComponent3;

	private AttackComponent _attackComponent4;

	private AttackComponent _attackComponent5;

	public Array stageSpawnList = new Array
	{
		new Array
		{
			new Array { "ZombieNormal" },
			new Array { "ZombieNormalCone" },
			new Array { "ZombieNormalBucket" },
			new Array { "ZombieNormalScreendoor", "ZombiePolevaulter", "ZombieSleeper", "ZombieFootball", "ZombieJackbox", "ZombiePogo", "ZombieLadder", "ZombieGargantuar", "ZombieZamboni", "ZombieCatapult" }
		},
		new Array
		{
			new Array { "ZombieNormalScreendoor", "ZombiePolevaulter", "ZombieSleeper", "ZombieFootball", "ZombieJackbox", "ZombiePogo", "ZombieLadder", "ZombieGargantuar", "ZombieZamboni", "ZombieCatapult" }
		},
		new Array
		{
			new Array { "ZombieNormalScreendoor", "ZombiePolevaulter", "ZombieSleeper", "ZombieFootball", "ZombieJackbox", "ZombiePogo", "ZombieLadder", "ZombieGargantuar", "ZombieZamboni", "ZombieCatapult" }
		},
		new Array
		{
			new Array { "ZombieNormalScreendoor", "ZombiePolevaulter", "ZombieSleeper", "ZombieFootball", "ZombieJackbox", "ZombiePogo", "ZombieLadder", "ZombieGargantuar", "ZombieZamboni", "ZombieCatapult" }
		}
	};

	public Array stateMethodList = new Array();

	public bool useEnterAnime = true;

	public string stateNow = "";

	public int spawnZombieNumOnce = 5;

	public int spawnZombieNumNow;

	public double spawnTime = 5.0;

	public int spawnLine = 1;

	public double spawnRestTime = 4.0;

	public double spawnRestTimer;

	public string spawnZomie;

	public int spawnNum;

	public bool headAttackOver;

	public int headAttackLine = 1;

	public bool headAttackIsFire = true;

	public double headAttackRestTime = 10.0;

	public double headAttackRestTimer;

	public bool bungeeIsSpawn;

	public Array bungeeList = new Array();

	public int stompId = -1;

	public Vector2I rvPos;

	public bool isRest = true;

	public double restTime = 5.0;

	public double restTimer;

	public int headIdleNum;

	public int stage;

	private double _headAttackPauseRemaining;

	private bool _headAttackPauseOwned;

	private static PackedScene FIRE_BALL => _FIRE_BALL ?? (_FIRE_BALL = GD.Load<PackedScene>("uid://ulproewvrqwb"));

	private static PackedScene ICE_BALL => _ICE_BALL ?? (_ICE_BALL = GD.Load<PackedScene>("uid://calwfqgd47fn7"));

	private static PackedScene EXPLOSION => _EXPLOSION ?? (_EXPLOSION = GD.Load<PackedScene>("uid://c8xarvk5gxpf0"));

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
		_headIdleStateHandle = StateMachine?.GetStateById("zombie.boss.head_idle");
		_headAttackStateHandle = StateMachine?.GetStateById("zombie.boss.head_attack");
		_headExitedStateHandle = StateMachine?.GetStateById("zombie.boss.head_exited");
		_spawnStateHandle = StateMachine?.GetStateById("zombie.boss.spawn");
		_stompStateHandle = StateMachine?.GetStateById("zombie.boss.stomp");
		_bungeeStateHandle = StateMachine?.GetStateById("zombie.boss.bungee");
		_rvStateHandle = StateMachine?.GetStateById("zombie.boss.rv");
		StateHandle headIdleStateHandle = _headIdleStateHandle;
		if (headIdleStateHandle == null || !headIdleStateHandle.IsValid)
		{
			return;
		}
		StateHandle headAttackStateHandle = _headAttackStateHandle;
		if (headAttackStateHandle == null || !headAttackStateHandle.IsValid)
		{
			return;
		}
		StateHandle headExitedStateHandle = _headExitedStateHandle;
		if (headExitedStateHandle == null || !headExitedStateHandle.IsValid)
		{
			return;
		}
		StateHandle spawnStateHandle = _spawnStateHandle;
		if (spawnStateHandle == null || !spawnStateHandle.IsValid)
		{
			return;
		}
		StateHandle stompStateHandle = _stompStateHandle;
		if (stompStateHandle == null || !stompStateHandle.IsValid)
		{
			return;
		}
		StateHandle bungeeStateHandle = _bungeeStateHandle;
		if (bungeeStateHandle != null && bungeeStateHandle.IsValid)
		{
			StateHandle rvStateHandle = _rvStateHandle;
			if (rvStateHandle != null && rvStateHandle.IsValid)
			{
				_headIdleStateHandle.Entered += HeadIdleEntered;
				_headIdleStateHandle.Exited += HeadIdleExited;
				_headIdleStateHandle.PhysicsProcessing += HeadIdleProcessing;
				_headAttackStateHandle.Entered += HeadAttackEntered;
				_headAttackStateHandle.Exited += HeadAttackExited;
				_headAttackStateHandle.PhysicsProcessing += HeadAttackProcessing;
				_headExitedStateHandle.Entered += HeadExitedEntered;
				_headExitedStateHandle.Exited += HeadExitedExited;
				_headExitedStateHandle.PhysicsProcessing += HeadExitedProcessing;
				_spawnStateHandle.Entered += SpawnEntered;
				_spawnStateHandle.Exited += SpawnExited;
				_spawnStateHandle.PhysicsProcessing += SpawnProcessing;
				_stompStateHandle.Entered += StompEntered;
				_stompStateHandle.Exited += StompExited;
				_stompStateHandle.PhysicsProcessing += StompProcessing;
				_bungeeStateHandle.Entered += BungeeEntered;
				_bungeeStateHandle.Exited += BungeeExited;
				_bungeeStateHandle.PhysicsProcessing += BungeeProcessing;
				_rvStateHandle.Entered += RVEntered;
				_rvStateHandle.Exited += RVExited;
				_rvStateHandle.PhysicsProcessing += RVProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_headIdleStateHandle != null)
			{
				_headIdleStateHandle.Entered -= HeadIdleEntered;
				_headIdleStateHandle.Exited -= HeadIdleExited;
				_headIdleStateHandle.PhysicsProcessing -= HeadIdleProcessing;
			}
			_headIdleStateHandle = null;
			if (_headAttackStateHandle != null)
			{
				_headAttackStateHandle.Entered -= HeadAttackEntered;
				_headAttackStateHandle.Exited -= HeadAttackExited;
				_headAttackStateHandle.PhysicsProcessing -= HeadAttackProcessing;
			}
			_headAttackStateHandle = null;
			if (_headExitedStateHandle != null)
			{
				_headExitedStateHandle.Entered -= HeadExitedEntered;
				_headExitedStateHandle.Exited -= HeadExitedExited;
				_headExitedStateHandle.PhysicsProcessing -= HeadExitedProcessing;
			}
			_headExitedStateHandle = null;
			if (_spawnStateHandle != null)
			{
				_spawnStateHandle.Entered -= SpawnEntered;
				_spawnStateHandle.Exited -= SpawnExited;
				_spawnStateHandle.PhysicsProcessing -= SpawnProcessing;
			}
			_spawnStateHandle = null;
			if (_stompStateHandle != null)
			{
				_stompStateHandle.Entered -= StompEntered;
				_stompStateHandle.Exited -= StompExited;
				_stompStateHandle.PhysicsProcessing -= StompProcessing;
			}
			_stompStateHandle = null;
			if (_bungeeStateHandle != null)
			{
				_bungeeStateHandle.Entered -= BungeeEntered;
				_bungeeStateHandle.Exited -= BungeeExited;
				_bungeeStateHandle.PhysicsProcessing -= BungeeProcessing;
			}
			_bungeeStateHandle = null;
			if (_rvStateHandle != null)
			{
				_rvStateHandle.Entered -= RVEntered;
				_rvStateHandle.Exited -= RVExited;
				_rvStateHandle.PhysicsProcessing -= RVProcessing;
			}
			_rvStateHandle = null;
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
			ResolveBossRuntimeComponents();
			InitializeBossGameplay();
		}
	}

	private void ResolveBossRuntimeComponents()
	{
		_attackComponent1 = componentManager.GetRuntime<AttackComponent>("character.attack.0");
		_attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
		_attackComponent3 = componentManager.GetRuntime<AttackComponent>("character.attack.2");
		_attackComponent4 = componentManager.GetRuntime<AttackComponent>("character.attack.3");
		_attackComponent5 = componentManager.GetRuntime<AttackComponent>("character.attack.4");
	}

	private void InitializeBossGameplay()
	{
		if (!_bossGameplayInitialized && inGame && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			_bossGameplayInitialized = true;
			Vector2 size = TowerDefenseManager.Instance.GetMapGridSize() * new Vector2((float)TowerDefenseManager.Instance.GetMapGridNum().X / 2f, TowerDefenseManager.Instance.GetMapGridNum().Y) / new Vector2(1f, 2.5f);
			_attackComponent1.SetCheckAreaRectangleSize(0, size);
			_attackComponent2.SetCheckAreaRectangleSize(0, size);
			_attackComponent3.SetCheckAreaRectangleSize(0, size);
			_attackComponent4.SetCheckAreaRectangleSize(0, size);
			_attackComponent5.SetCheckAreaRectangleSize(0, TowerDefenseManager.Instance.GetMapGridSize() * new Vector2(3f, 2f));
			Vector2 vector = TowerDefenseManager.Instance.GetMapGridSize() * new Vector2((float)TowerDefenseManager.Instance.GetMapGridNum().X / 2f, TowerDefenseManager.Instance.GetMapGridNum().Y) / new Vector2(1f, 4f);
			_attackComponent1.SetCheckAreaShapeLocalOrigin(0, new Vector2(-100f, (0f - vector.Y) * 1.5f));
			_attackComponent2.SetCheckAreaShapeLocalOrigin(0, new Vector2(-100f, (0f - vector.Y) * 0.5f));
			_attackComponent3.SetCheckAreaShapeLocalOrigin(0, new Vector2(-100f, vector.Y * 0.5f));
			_attackComponent4.SetCheckAreaShapeLocalOrigin(0, new Vector2(-100f, vector.Y * 1.5f));
			gridPos = new Vector2I(TowerDefenseManager.Instance.GetMapGridNum().X, (int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().Y / 2f) + 1);
			SetLogicalGlobalPosition(TowerDefenseManager.GetMapCellPlantPos(gridPos));
			SetStateList();
			instance.invincible = true;
			instance.canBeCollection = false;
			targetRegistrationComponent.allLineCheck = false;
			targetRegistrationComponent.canProjectileCheck = false;
			instance.unUseBuffFlags = -1;
			ConnectStateSignals();
			WaitForPhysicsFrame();
		}
	}

	private async Task WaitForPhysicsFrame()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		useEnterAnime = true;
	}

	private static bool IsBossGameplayPaused()
	{
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(towerDefenseManager))
		{
			return towerDefenseManager.pauseZombie;
		}
		return false;
	}

	private bool ShouldFreezeBossGameplay()
	{
		if (Engine.IsEditorHint() || !inGame)
		{
			return false;
		}
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (GodotObject.IsInstanceValid(towerDefenseManager) && towerDefenseManager.IsGameRunning())
		{
			return towerDefenseManager.pauseZombie;
		}
		return false;
	}

	private void ApplyBossGlobalPausePlayback()
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
			sprite.pause = true;
			sprite.playBack = GodotObject.IsInstanceValid(towerDefenseManager) && (towerDefenseManager.backZombie || isGarlicBird);
		}
	}

	public override void BatchUpdate(double delta)
	{
		if (ShouldFreezeBossGameplay())
		{
			ApplyBossGlobalPausePlayback();
			return;
		}
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || !inGame || !TowerDefenseManager.Instance.IsGameRunning() || IsBossGameplayPaused())
		{
			return;
		}
		UpdateHeadAttackPause(delta);
		if (Engine.GetPhysicsFrames() % 5 == 0L)
		{
			SetSpriteGroupShaderParameter("discardDownPos", 10000.0);
		}
		if (!isRest)
		{
			return;
		}
		if (restTimer < restTime)
		{
			restTimer += delta;
			return;
		}
		restTimer = 0.0;
		stateNow = (string)stateMethodList[0];
		stateMethodList.RemoveAt(0);
		isRest = false;
		if (stateNow != "HeadIdle" && stateMethodList.Count == 0)
		{
			stateMethodList.Add("HeadIdle");
		}
	}

	public override void Walk()
	{
		Idle();
	}

	public void HeadIdleEntered()
	{
		AudioManager.Instance.AudioPlay("HydraulicShort");
		AudioManager.Instance.AudioPlay("Hydraulic");
		headAttackRestTimer = 0.0;
		if (!headAttackOver)
		{
			sprite.SetAnimation("HeadEnter", loop: false, 0.2);
			sprite.AddAnimation("HeadIdle", 0.0);
		}
		else
		{
			sprite.SetAnimation("HeadIdle", loop: true, 0.2);
		}
	}

	public void HeadIdleProcessing(double delta)
	{
		SetTransientRenderZIndex(1000, "zomboss-head-idle");
		sprite.timeScale = timeScale * 1.0;
		if (IsBossGameplayPaused() || sprite.pause)
		{
			return;
		}
		if (headAttackRestTimer < headAttackRestTime)
		{
			headAttackRestTimer += delta * timeScale;
			return;
		}
		headAttackRestTimer = 0.0;
		if (!headAttackOver)
		{
			SendStateEvent("ToHeadAttack");
		}
		else
		{
			SendStateEvent("ToHeadExited");
		}
	}

	public void HeadIdleExited()
	{
	}

	public void HeadAttackEntered()
	{
		headAttackOver = true;
		headAttackLine = GD.RandRange(1, TowerDefenseManager.Instance.GetMapGridNum().Y);
		headAttackIsFire = (double)GD.Randf() > 0.5;
		SetTransientRenderZIndex((int)(headAttackLine * 15 + itemLayer), "zomboss-head-attack");
		ZombieBoss zombieBoss = sprite as ZombieBoss;
		if (GodotObject.IsInstanceValid(zombieBoss))
		{
			zombieBoss.SetHeadAttack(headAttackLine);
			zombieBoss.SetHeadAttackBall(headAttackIsFire);
		}
		sprite.SetAnimation("HeadAttack4", loop: false, 0.2);
	}

	public void HeadAttackProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void HeadAttackExited()
	{
		ReleaseHeadAttackPause(refreshLayer: false);
	}

	public void HeadExitedEntered()
	{
		instance.invincible = true;
		instance.canBeCollection = false;
		targetRegistrationComponent.canProjectileCheck = false;
		instance.unUseBuffFlags = -1;
		targetRegistrationComponent.allLineCheck = false;
		if (buff.BuffHas("Frozen"))
		{
			buff.DeleteBuff("Frozen");
		}
		if (buff.BuffHas("IceSpeedDown"))
		{
			buff.DeleteBuff("IceSpeedDown");
		}
		sprite.SetAnimation("HeadExited", loop: false);
	}

	public void HeadExitedProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void HeadExitedExited()
	{
	}

	public void SpawnEntered()
	{
		AudioManager.Instance.AudioPlay("HydraulicShort");
		spawnZomie = GetSpawnZombie();
		spawnLine = GD.RandRange(1, TowerDefenseManager.Instance.GetMapGridNum().Y);
		sprite.SetAnimation("Spawn1", loop: false, 0.2);
		ZombieBoss zombieBoss = sprite as ZombieBoss;
		if (GodotObject.IsInstanceValid(zombieBoss))
		{
			zombieBoss.SetSpawn(spawnLine);
		}
	}

	public void SpawnProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void SpawnExited()
	{
	}

	public void StompEntered()
	{
		AudioManager.Instance.AudioPlay("HydraulicShort");
		Array<int> array = new Array<int>();
		if (_attackComponent1.CanAttackOnce())
		{
			array.Add(1);
		}
		if (_attackComponent2.CanAttackOnce())
		{
			array.Add(2);
		}
		if (_attackComponent3.CanAttackOnce())
		{
			array.Add(3);
		}
		if (_attackComponent4.CanAttackOnce())
		{
			array.Add(4);
		}
		if (array.Count > 0)
		{
			stompId = array.PickRandom();
		}
		else
		{
			stompId = 3;
		}
		switch (stompId)
		{
		case 1:
			sprite.SetAnimation("Stomp1", loop: false, 0.2);
			break;
		case 2:
			sprite.SetAnimation("Stomp2", loop: false, 0.2);
			break;
		case 3:
			sprite.SetAnimation("Stomp3", loop: false, 0.2);
			break;
		case 4:
			sprite.SetAnimation("Stomp4", loop: false, 0.2);
			break;
		}
	}

	public void StompProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void StompExited()
	{
	}

	public void BungeeEntered()
	{
		AudioManager.Instance.AudioPlay("HydraulicShort");
		bungeeIsSpawn = false;
		sprite.SetAnimation("BungeeEnter", loop: false, 0.2);
	}

	public void BungeeProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		if (!bungeeIsSpawn)
		{
			return;
		}
		for (int num = bungeeList.Count - 1; num >= 0; num--)
		{
			if (!GodotObject.IsInstanceValid(bungeeList[num].AsGodotObject()))
			{
				bungeeList.RemoveAt(num);
				break;
			}
		}
		if (bungeeList.Count <= 0)
		{
			sprite.SetAnimation("BungeeExited", loop: false, 0.2);
			bungeeIsSpawn = false;
		}
	}

	public void BungeeExited()
	{
	}

	public void RVEntered()
	{
		AudioManager.Instance.AudioPlay("HydraulicShort");
		rvPos = new Vector2I((int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().X / 2f - 1f), TowerDefenseManager.Instance.GetMapGridNum().Y - 1);
		sprite.SetAnimation("RV", loop: false, 0.2);
	}

	public void RVProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void RVExited()
	{
	}

	public override void IdleEntered()
	{
		if (!inGame)
		{
			sprite.SetAnimation("HeadIdle");
			return;
		}
		instance.invincible = true;
		instance.canBeCollection = false;
		if (useEnterAnime)
		{
			sprite.SetAnimation("Enter", loop: false, 0.2);
			sprite.AddAnimation("Idle", 0.0);
			useEnterAnime = false;
		}
		else
		{
			sprite.SetAnimation("Idle", loop: true, 0.2);
		}
	}

	public override void IdleProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		StateRunning(delta);
	}

	public override void DieEntered()
	{
		bool flag = false;
		ZombieBoss zombieBoss = sprite as ZombieBoss;
		if (sprite.clip == "HeadIdle" || sprite.clip == "HeadAttack4")
		{
			flag = true;
		}
		ReleaseHeadAttackPause(refreshLayer: false);
		base.DieEntered();
		if (GodotObject.IsInstanceValid(zombieBoss))
		{
			zombieBoss.SetHeadAttack((int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().Y / 2f) + 2, 0.5);
		}
		spritePause = false;
		SetTransientRenderZIndex((int)(headAttackLine * 15 + itemLayer), "zomboss-death");
		if (flag)
		{
			sprite.SetAnimation("HeadExited", loop: false, 0.2);
			sprite.AddAnimation("Death", 0.0, loop: false);
		}
		else
		{
			sprite.SetAnimation("Death", loop: false, 0.2);
		}
	}

	public override void DieProcessing(double delta)
	{
		sprite.timeScale = timeScale * 0.5;
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		ZombieBoss zombieBoss = sprite as ZombieBoss;
		if (GodotObject.IsInstanceValid(zombieBoss))
		{
			zombieBoss.DamagePointSet(damagePointName);
		}
		switch (damagePointName)
		{
		case "Stage1":
			stage = 1;
			restTime = 4.5;
			spawnRestTime = 3.5;
			break;
		case "Stage2":
			stage = 2;
			restTime = 4.0;
			spawnRestTime = 3.0;
			break;
		case "Stage3":
			stage = 3;
			ExplosionMethod();
			break;
		case "Death":
			timeScaleInit = 3.0;
			AudioManager.Instance.AudioPlay("Bossexplosion");
			break;
		}
	}

	public override void AnimeCompleted(string clip)
	{
		ZombieDeathComponent zombieDeathComponent = base.zombieDeathComponent;
		if (zombieDeathComponent != null && !zombieDeathComponent.IsReleased && base.zombieDeathComponent.IsLandDeathAnimationClip(clip))
		{
			RemoveFromGroup("Zombie");
			return;
		}
		if (die)
		{
			HitBoxDestroy();
			Die();
		}
		if (sprite is ZombieBoss zombieBoss)
		{
			zombieBoss.AnimeCompleted(clip);
		}
		if (clip == null)
		{
			return;
		}
		switch (clip.Length)
		{
		case 6:
			switch (clip[5])
			{
			case '1':
				if (!(clip == "Spawn1"))
				{
					if (!(clip == "Stomp1"))
					{
						break;
					}
					goto IL_0219;
				}
				if (!die)
				{
					Idle();
				}
				break;
			case '2':
				if (!(clip == "Stomp2"))
				{
					break;
				}
				goto IL_0219;
			case '3':
				if (!(clip == "Stomp3"))
				{
					break;
				}
				goto IL_0219;
			case '4':
				{
					if (!(clip == "Stomp4"))
					{
						break;
					}
					goto IL_0219;
				}
				IL_0219:
				isRest = true;
				if (!die)
				{
					Idle();
				}
				break;
			}
			break;
		case 9:
			if (clip == "HeadEnter")
			{
				instance.invincible = false;
				instance.canBeCollection = true;
				targetRegistrationComponent.canProjectileCheck = true;
				targetRegistrationComponent.allLineCheck = true;
				instance.unUseBuffFlags = -4;
			}
			break;
		case 10:
			if (clip == "HeadExited")
			{
				FreshZIndex();
				SetStateList();
				isRest = true;
				restTimer = 2.0;
				if (!die)
				{
					Idle();
				}
			}
			break;
		case 11:
			if (clip == "HeadAttack4")
			{
				SendStateEvent("ToHeadIdle");
			}
			break;
		case 12:
			if (clip == "BungeeExited")
			{
				FreshZIndex();
				isRest = true;
				if (!die)
				{
					Idle();
				}
			}
			break;
		case 2:
			if (clip == "RV")
			{
				isRest = true;
				if (!die)
				{
					Idle();
				}
			}
			break;
		case 3:
		case 4:
		case 5:
		case 7:
		case 8:
			break;
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == null)
		{
			return;
		}
		switch (command.Length)
		{
		case 5:
			switch (command[1])
			{
			case 'p':
				if (command == "spawn")
				{
					ZombieSpawn();
				}
				break;
			case 't':
				if (command == "stomp")
				{
					AudioManager.Instance.AudioPlay("GargantuarThump");
					switch (stompId)
					{
					case 1:
						_attackComponent1.SmashAttackAll(100000.0);
						break;
					case 2:
						_attackComponent2.SmashAttackAll(100000.0);
						break;
					case 3:
						_attackComponent3.SmashAttackAll(100000.0);
						break;
					case 4:
						_attackComponent4.SmashAttackAll(100000.0);
						break;
					}
				}
				break;
			}
			break;
		case 7:
			switch (command[0])
			{
			case 't':
				if (command == "throwRV")
				{
					ZombieBoss zombieBoss2 = sprite as ZombieBoss;
					if (GodotObject.IsInstanceValid(zombieBoss2))
					{
						zombieBoss2.SetRVVisible(visible: true);
					}
					RVSpawn();
				}
				break;
			case 'e':
				if (command == "explode")
				{
					ExplosionMethod();
				}
				break;
			}
			break;
		case 8:
			switch (command[0])
			{
			case 'R':
				if (command == "RVAttack")
				{
					AudioManager.Instance.AudioPlay("RVThrow");
					_attackComponent5.SmashAttackAll(100000.0);
				}
				break;
			case 'f':
				if (command == "footstep")
				{
					AudioManager.Instance.AudioPlay("GargantuarThump");
				}
				break;
			}
			break;
		case 11:
			if (command == "spawnBubgee")
			{
				BungeeSpawn();
			}
			break;
		case 10:
			if (command == "headAttack")
			{
				BallSpawn();
				BeginHeadAttackPause();
			}
			break;
		case 14:
			if (command == "headAttackOver")
			{
				ZombieBoss zombieBoss = sprite as ZombieBoss;
				if (GodotObject.IsInstanceValid(zombieBoss))
				{
					zombieBoss.SetHeadAttack((int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().Y / 2f) + 2, 0.5);
				}
			}
			break;
		case 6:
		case 9:
		case 12:
		case 13:
			break;
		}
	}

	private void BeginHeadAttackPause()
	{
		_headAttackPauseRemaining = 1.0;
		_headAttackPauseOwned = true;
		spritePause = true;
	}

	private void UpdateHeadAttackPause(double delta)
	{
		if (!IsBossGameplayPaused() && !(_headAttackPauseRemaining <= 0.0))
		{
			_headAttackPauseRemaining = Mathf.Max(0.0, _headAttackPauseRemaining - delta);
			if (!(_headAttackPauseRemaining > 0.0))
			{
				ReleaseHeadAttackPause(refreshLayer: true);
			}
		}
	}

	private void ReleaseHeadAttackPause(bool refreshLayer)
	{
		bool num = _headAttackPauseRemaining > 0.0 || _headAttackPauseOwned;
		_headAttackPauseRemaining = 0.0;
		if (_headAttackPauseOwned)
		{
			spritePause = false;
		}
		_headAttackPauseOwned = false;
		if (num && refreshLayer)
		{
			SetTransientRenderZIndex((int)(headAttackLine * 15 + itemLayer), "zomboss-head-attack-delay");
		}
	}

	public void StateRunning(double delta)
	{
		if (IsBossGameplayPaused())
		{
			return;
		}
		switch (stateNow)
		{
		case "Spawn":
			if (spawnRestTimer < spawnRestTime)
			{
				spawnRestTimer += delta;
				break;
			}
			SendStateEvent("ToSpawn");
			spawnZombieNumNow++;
			spawnRestTimer = 0.0;
			if (spawnZombieNumNow >= spawnZombieNumOnce)
			{
				spawnNum++;
				spawnZombieNumNow = 0;
				isRest = true;
				stateNow = "";
			}
			break;
		case "HeadIdle":
			headAttackOver = false;
			headIdleNum++;
			SendStateEvent("ToHeadIdle");
			stateNow = "";
			break;
		case "BungeeSpawn":
			SendStateEvent("ToBungee");
			stateNow = "";
			break;
		case "Stomp":
			SendStateEvent("ToStomp");
			stateNow = "";
			break;
		case "RV":
			SendStateEvent("ToRV");
			stateNow = "";
			break;
		}
	}

	public void ZombieSpawn()
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(spawnZomie);
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(packetConfig) || !GodotObject.IsInstanceValid(node2D))
		{
			return;
		}
		Vector2 vector = new Vector2((sprite as ZombieBoss)?.GetSpawnMarkerGlobalPos(this).X ?? GetLogicalGlobalPosition().X, TowerDefenseManager.GetMapCellPlantPos(new Vector2I(0, spawnLine)).Y);
		Vector2I mapGridPos = TowerDefenseManager.Instance.GetMapGridPos(vector);
		mapGridPos.Y = spawnLine;
		TowerDefenseCharacter towerDefenseCharacter = packetConfig.Create(node2D.ToLocal(vector), mapGridPos);
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			return;
		}
		towerDefenseCharacter.EnableComponentGameplayUntilBattlefieldEntry();
		node2D.AddChild(towerDefenseCharacter, forceReadableName: false, InternalMode.Disabled);
		towerDefenseCharacter.gridPos = mapGridPos;
		towerDefenseCharacter.SetLogicalGlobalPosition(vector);
		ShadowComponent shadowComponent = towerDefenseCharacter.shadowComponent;
		if (shadowComponent != null && !shadowComponent.IsReleased)
		{
			towerDefenseCharacter.shadowComponent.Init();
			towerDefenseCharacter.shadowComponent.UpdateShadow();
		}
		towerDefenseCharacter.CallDeferred("WalkReady");
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
				MultiPlayerManager.Instance.SendSpawnCharacterAt(spawnZomie, mapGridPos.X, mapGridPos.Y, nextSyncId, 1.0, 1.0, hypnoses: false, 0.0, useCreate: true, vector.X, vector.Y, walkAfterSpawn: true, 0.0, "", new Dictionary
				{
					["component_gameplay_until_battlefield_entry"] = true,
					["walk_ready_after_spawn"] = true
				});
			}
		}
	}

	public void BungeeSpawn()
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		List<TowerDefenseCharacter> cleanCharactersList = TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList();
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
		int num = (int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().X / 2f) + 1;
		foreach (TowerDefenseCharacter item2 in cleanCharactersList)
		{
			if (item2 is TowerDefensePlant && item2.gridPos.X <= num)
			{
				list.Add(item2);
			}
		}
		Array<Vector2I> array = new Array<Vector2I>();
		foreach (TowerDefenseCharacter item3 in list)
		{
			if (!array.Contains(item3.gridPos))
			{
				array.Add(item3.gridPos);
			}
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ZombieBungi");
		Array<Vector2I> array2 = new Array<Vector2I>();
		for (int i = 0; i < Mathf.Min(3, array.Count); i++)
		{
			Vector2I item = array.PickRandom();
			array.Remove(item);
			array2.Add(item);
			TowerDefenseZombieBungi towerDefenseZombieBungi = packetConfig.Plant(item, playAudio: false) as TowerDefenseZombieBungi;
			towerDefenseZombieBungi.skipBungeeTarget = true;
			bungeeList.Add(towerDefenseZombieBungi);
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
			{
				TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
				if (GodotObject.IsInstanceValid(currentControl))
				{
					int nextSyncId = currentControl.GetNextSyncId();
					currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseZombieBungi);
					MultiPlayerManager.Instance.SendSpawnCharacterAt("ZombieBungi", item.X, item.Y, nextSyncId);
				}
			}
		}
		bungeeIsSpawn = true;
	}

	public void RVSpawn()
	{
		List<TowerDefenseCharacter> cleanCharactersList = TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList();
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
		int num = (int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().X / 2f) - 1;
		foreach (TowerDefenseCharacter item in cleanCharactersList)
		{
			if (item is TowerDefensePlant && item.gridPos.X <= num)
			{
				list.Add(item);
			}
		}
		Array<Vector2I> array = new Array<Vector2I>();
		foreach (TowerDefenseCharacter item2 in list)
		{
			if (!array.Contains(item2.gridPos))
			{
				array.Add(item2.gridPos);
			}
		}
		Vector2I vector2I = ((array.Count <= 0) ? new Vector2I(1, 2) : array.PickRandom());
		if (vector2I.Y >= TowerDefenseManager.Instance.GetMapGridNum().Y)
		{
			vector2I.Y--;
		}
		rvPos = vector2I;
		_attackComponent5.SetCheckAreaShapeWorldOrigin(0, TowerDefenseManager.Instance.GetMapCellPos(rvPos) + TowerDefenseManager.Instance.GetMapGridSize() * new Vector2(1.5f, 0.5f));
		ZombieBoss zombieBoss = sprite as ZombieBoss;
		if (GodotObject.IsInstanceValid(zombieBoss))
		{
			zombieBoss.SetRVPos(rvPos);
		}
	}

	public void BallSpawn()
	{
		AudioManager.Instance.AudioPlay("Bossboulderattack");
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		Vector2 vector = new Vector2((sprite as ZombieBoss)?.GetBallSpawnMarkerGlobalPos(this).X ?? GetLogicalGlobalPosition().X, TowerDefenseManager.GetMapCellPlantPos(new Vector2I(0, headAttackLine)).Y);
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(node2D))
		{
			return;
		}
		TowerDefensePacketConfig packetConfig;
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce;
		if (headAttackIsFire)
		{
			packetConfig = TowerDefenseManager.GetPacketConfig("ItemFireball");
			TowerDefenseCharacter character = CreateBallAt(packetConfig, node2D, vector);
			if (!GodotObject.IsInstanceValid(character))
			{
				return;
			}
			towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(FIRE_BALL, new Vector2I(100, headAttackLine));
			node2D.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
			towerDefenseEffectParticlesOnce.GlobalPosition = vector;
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
			{
				TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
				if (GodotObject.IsInstanceValid(currentControl))
				{
					int nextSyncId = currentControl.GetNextSyncId();
					currentControl.RegisterSyncCharacter(nextSyncId, character);
					MultiPlayerManager.Instance.SendSpawnCharacterAt("ItemFireball", 100, headAttackLine, nextSyncId, 1.0, 1.0, hypnoses: false, 0.0, useCreate: true, vector.X, vector.Y);
				}
			}
			return;
		}
		packetConfig = TowerDefenseManager.GetPacketConfig("ItemIceball");
		TowerDefenseCharacter character2 = CreateBallAt(packetConfig, node2D, vector);
		if (!GodotObject.IsInstanceValid(character2))
		{
			return;
		}
		towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(ICE_BALL, new Vector2I(100, headAttackLine));
		node2D.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		towerDefenseEffectParticlesOnce.GlobalPosition = vector;
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl2 = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl2))
			{
				int nextSyncId2 = currentControl2.GetNextSyncId();
				currentControl2.RegisterSyncCharacter(nextSyncId2, character2);
				MultiPlayerManager.Instance.SendSpawnCharacterAt("ItemIceball", 100, headAttackLine, nextSyncId2, 1.0, 1.0, hypnoses: false, 0.0, useCreate: true, vector.X, vector.Y);
			}
		}
	}

	private TowerDefenseCharacter CreateBallAt(TowerDefensePacketConfig packetConfig, Node2D characterNode, Vector2 ballPos)
	{
		if (!GodotObject.IsInstanceValid(packetConfig) || !GodotObject.IsInstanceValid(characterNode))
		{
			return null;
		}
		TowerDefenseCharacter towerDefenseCharacter = packetConfig.Create(characterNode.ToLocal(ballPos), new Vector2I(100, headAttackLine));
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			return null;
		}
		characterNode.AddChild(towerDefenseCharacter, forceReadableName: false, InternalMode.Disabled);
		towerDefenseCharacter.SetLogicalGlobalPosition(ballPos);
		return towerDefenseCharacter;
	}

	public string GetSpawnZombie()
	{
		Array array = new Array();
		Array array2 = ((stage >= stageSpawnList.Count) ? ((Array)stageSpawnList[stageSpawnList.Count - 1]) : ((Array)stageSpawnList[stage]));
		array = ((spawnNum >= array2.Count) ? ((Array)array2[array2.Count - 1]) : ((Array)array2[spawnNum]));
		return TowerDefenseManager.Instance.PickRandomZomie(array);
	}

	public void SetStateList()
	{
		switch (headIdleNum)
		{
		case 0:
			stateMethodList = new Array { "Spawn", "Spawn" };
			break;
		case 1:
			stateMethodList = new Array { "Spawn" };
			break;
		default:
			stateMethodList = new Array { "Spawn" };
			break;
		}
		if (stage > 1)
		{
			if ((double)GD.Randf() > 0.25)
			{
				stateMethodList.Insert(0, "BungeeSpawn");
			}
			else
			{
				stateMethodList.Insert(0, "RV");
			}
			if (_attackComponent1.CanAttackOnce() || _attackComponent2.CanAttackOnce() || _attackComponent3.CanAttackOnce() || _attackComponent4.CanAttackOnce())
			{
				stateMethodList.Insert(0, "Stomp");
			}
		}
	}

	public void ExplosionMethod()
	{
		AudioManager.Instance.AudioPlay("ZamboniExplosion");
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(EXPLOSION, new Vector2I(100, 100));
		TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		towerDefenseEffectParticlesOnce.Position = Position - new Vector2(GD.RandRange(-50, 50), GD.RandRange(-100, 100));
	}

	public override void InWater()
	{
	}

	public override void OutWater()
	{
	}

	public override void InWaterDiscardSet()
	{
	}

	public override void OutWaterDiscardSet()
	{
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["stage"] = stage,
			["stateNow"] = stateNow,
			["useEnterAnime"] = useEnterAnime,
			["spawnZombieNumOnce"] = spawnZombieNumOnce,
			["spawnZombieNumNow"] = spawnZombieNumNow,
			["spawnTime"] = spawnTime,
			["spawnLine"] = spawnLine,
			["spawnRestTime"] = spawnRestTime,
			["spawnRestTimer"] = spawnRestTimer,
			["spawnZomie"] = spawnZomie,
			["spawnNum"] = spawnNum,
			["headAttackOver"] = headAttackOver,
			["headAttackLine"] = headAttackLine,
			["headAttackIsFire"] = headAttackIsFire,
			["headAttackRestTime"] = headAttackRestTime,
			["headAttackRestTimer"] = headAttackRestTimer,
			["headIdleNum"] = headIdleNum,
			["bungeeIsSpawn"] = bungeeIsSpawn,
			["stompId"] = stompId,
			["isRest"] = isRest,
			["restTime"] = restTime,
			["restTimer"] = restTimer,
			["stateMethodList"] = stateMethodList
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		stage = data.GetValueOrDefault("stage", 0).AsInt32();
		stateNow = data.GetValueOrDefault("stateNow", "").AsString();
		useEnterAnime = data.GetValueOrDefault("useEnterAnime", true).AsBool();
		spawnZombieNumOnce = data.GetValueOrDefault("spawnZombieNumOnce", 5).AsInt32();
		spawnZombieNumNow = data.GetValueOrDefault("spawnZombieNumNow", 0).AsInt32();
		spawnTime = data.GetValueOrDefault("spawnTime", 5).AsDouble();
		spawnLine = data.GetValueOrDefault("spawnLine", 1).AsInt32();
		spawnRestTime = data.GetValueOrDefault("spawnRestTime", 4.0).AsDouble();
		spawnRestTimer = data.GetValueOrDefault("spawnRestTimer", 0.0).AsDouble();
		spawnZomie = data.GetValueOrDefault("spawnZomie", "").AsString();
		spawnNum = data.GetValueOrDefault("spawnNum", 0).AsInt32();
		headAttackOver = data.GetValueOrDefault("headAttackOver", false).AsBool();
		headAttackLine = data.GetValueOrDefault("headAttackLine", 1).AsInt32();
		headAttackIsFire = data.GetValueOrDefault("headAttackIsFire", true).AsBool();
		headAttackRestTime = data.GetValueOrDefault("headAttackRestTime", 10.0).AsDouble();
		headAttackRestTimer = data.GetValueOrDefault("headAttackRestTimer", 0.0).AsDouble();
		headIdleNum = data.GetValueOrDefault("headIdleNum", 0).AsInt32();
		bungeeIsSpawn = data.GetValueOrDefault("bungeeIsSpawn", false).AsBool();
		stompId = data.GetValueOrDefault("stompId", -1).AsInt32();
		isRest = data.GetValueOrDefault("isRest", true).AsBool();
		restTime = data.GetValueOrDefault("restTime", 5.0).AsDouble();
		restTimer = data.GetValueOrDefault("restTimer", 0.0).AsDouble();
		stateMethodList = data.GetValueOrDefault("stateMethodList", new Array()).AsGodotArray();
		if (stage >= 1 && GodotObject.IsInstanceValid(sprite) && sprite is ZombieBoss zombieBoss)
		{
			zombieBoss.DamagePointSet("Stage1");
		}
		if (stage >= 2 && GodotObject.IsInstanceValid(sprite) && sprite is ZombieBoss zombieBoss2)
		{
			zombieBoss2.DamagePointSet("Stage2");
		}
		if (stage >= 3 && GodotObject.IsInstanceValid(sprite) && sprite is ZombieBoss zombieBoss3)
		{
			zombieBoss3.DamagePointSet("Stage3");
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(57)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveBossRuntimeComponents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitializeBossGameplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsBossGameplayPaused, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ShouldFreezeBossGameplay, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyBossGlobalPausePlayback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HeadIdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HeadIdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HeadIdleExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HeadAttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HeadAttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HeadAttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HeadExitedEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HeadExitedProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HeadExitedExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StompEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StompProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StompExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BungeeEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BungeeProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BungeeExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RVEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RVProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RVExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.BeginHeadAttackPause, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateHeadAttackPause, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseHeadAttackPause, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "refreshLayer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StateRunning, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ZombieSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BungeeSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RVSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BallSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateBallAt, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "characterNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "ballPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSpawnZombie, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetStateList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExplosionMethod, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWaterDiscardSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWaterDiscardSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.ResolveBossRuntimeComponents && args.Count == 0)
		{
			ResolveBossRuntimeComponents();
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeBossGameplay && args.Count == 0)
		{
			InitializeBossGameplay();
			ret = default;
			return true;
		}
		if (method == MethodName.IsBossGameplayPaused && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBossGameplayPaused());
			return true;
		}
		if (method == MethodName.ShouldFreezeBossGameplay && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldFreezeBossGameplay());
			return true;
		}
		if (method == MethodName.ApplyBossGlobalPausePlayback && args.Count == 0)
		{
			ApplyBossGlobalPausePlayback();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.HeadIdleEntered && args.Count == 0)
		{
			HeadIdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.HeadIdleProcessing && args.Count == 1)
		{
			HeadIdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HeadIdleExited && args.Count == 0)
		{
			HeadIdleExited();
			ret = default;
			return true;
		}
		if (method == MethodName.HeadAttackEntered && args.Count == 0)
		{
			HeadAttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.HeadAttackProcessing && args.Count == 1)
		{
			HeadAttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HeadAttackExited && args.Count == 0)
		{
			HeadAttackExited();
			ret = default;
			return true;
		}
		if (method == MethodName.HeadExitedEntered && args.Count == 0)
		{
			HeadExitedEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.HeadExitedProcessing && args.Count == 1)
		{
			HeadExitedProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HeadExitedExited && args.Count == 0)
		{
			HeadExitedExited();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnEntered && args.Count == 0)
		{
			SpawnEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnProcessing && args.Count == 1)
		{
			SpawnProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnExited && args.Count == 0)
		{
			SpawnExited();
			ret = default;
			return true;
		}
		if (method == MethodName.StompEntered && args.Count == 0)
		{
			StompEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.StompProcessing && args.Count == 1)
		{
			StompProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StompExited && args.Count == 0)
		{
			StompExited();
			ret = default;
			return true;
		}
		if (method == MethodName.BungeeEntered && args.Count == 0)
		{
			BungeeEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.BungeeProcessing && args.Count == 1)
		{
			BungeeProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BungeeExited && args.Count == 0)
		{
			BungeeExited();
			ret = default;
			return true;
		}
		if (method == MethodName.RVEntered && args.Count == 0)
		{
			RVEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.RVProcessing && args.Count == 1)
		{
			RVProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RVExited && args.Count == 0)
		{
			RVExited();
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
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.BeginHeadAttackPause && args.Count == 0)
		{
			BeginHeadAttackPause();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateHeadAttackPause && args.Count == 1)
		{
			UpdateHeadAttackPause(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseHeadAttackPause && args.Count == 1)
		{
			ReleaseHeadAttackPause(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StateRunning && args.Count == 1)
		{
			StateRunning(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ZombieSpawn && args.Count == 0)
		{
			ZombieSpawn();
			ret = default;
			return true;
		}
		if (method == MethodName.BungeeSpawn && args.Count == 0)
		{
			BungeeSpawn();
			ret = default;
			return true;
		}
		if (method == MethodName.RVSpawn && args.Count == 0)
		{
			RVSpawn();
			ret = default;
			return true;
		}
		if (method == MethodName.BallSpawn && args.Count == 0)
		{
			BallSpawn();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateBallAt && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(CreateBallAt(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<Node2D>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.GetSpawnZombie && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetSpawnZombie());
			return true;
		}
		if (method == MethodName.SetStateList && args.Count == 0)
		{
			SetStateList();
			ret = default;
			return true;
		}
		if (method == MethodName.ExplosionMethod && args.Count == 0)
		{
			ExplosionMethod();
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
		if (method == MethodName.InWaterDiscardSet && args.Count == 0)
		{
			InWaterDiscardSet();
			ret = default;
			return true;
		}
		if (method == MethodName.OutWaterDiscardSet && args.Count == 0)
		{
			OutWaterDiscardSet();
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
		if (method == MethodName.IsBossGameplayPaused && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBossGameplayPaused());
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
		if (method == MethodName.ResolveBossRuntimeComponents)
		{
			return true;
		}
		if (method == MethodName.InitializeBossGameplay)
		{
			return true;
		}
		if (method == MethodName.IsBossGameplayPaused)
		{
			return true;
		}
		if (method == MethodName.ShouldFreezeBossGameplay)
		{
			return true;
		}
		if (method == MethodName.ApplyBossGlobalPausePlayback)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.HeadIdleEntered)
		{
			return true;
		}
		if (method == MethodName.HeadIdleProcessing)
		{
			return true;
		}
		if (method == MethodName.HeadIdleExited)
		{
			return true;
		}
		if (method == MethodName.HeadAttackEntered)
		{
			return true;
		}
		if (method == MethodName.HeadAttackProcessing)
		{
			return true;
		}
		if (method == MethodName.HeadAttackExited)
		{
			return true;
		}
		if (method == MethodName.HeadExitedEntered)
		{
			return true;
		}
		if (method == MethodName.HeadExitedProcessing)
		{
			return true;
		}
		if (method == MethodName.HeadExitedExited)
		{
			return true;
		}
		if (method == MethodName.SpawnEntered)
		{
			return true;
		}
		if (method == MethodName.SpawnProcessing)
		{
			return true;
		}
		if (method == MethodName.SpawnExited)
		{
			return true;
		}
		if (method == MethodName.StompEntered)
		{
			return true;
		}
		if (method == MethodName.StompProcessing)
		{
			return true;
		}
		if (method == MethodName.StompExited)
		{
			return true;
		}
		if (method == MethodName.BungeeEntered)
		{
			return true;
		}
		if (method == MethodName.BungeeProcessing)
		{
			return true;
		}
		if (method == MethodName.BungeeExited)
		{
			return true;
		}
		if (method == MethodName.RVEntered)
		{
			return true;
		}
		if (method == MethodName.RVProcessing)
		{
			return true;
		}
		if (method == MethodName.RVExited)
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
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
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
		if (method == MethodName.BeginHeadAttackPause)
		{
			return true;
		}
		if (method == MethodName.UpdateHeadAttackPause)
		{
			return true;
		}
		if (method == MethodName.ReleaseHeadAttackPause)
		{
			return true;
		}
		if (method == MethodName.StateRunning)
		{
			return true;
		}
		if (method == MethodName.ZombieSpawn)
		{
			return true;
		}
		if (method == MethodName.BungeeSpawn)
		{
			return true;
		}
		if (method == MethodName.RVSpawn)
		{
			return true;
		}
		if (method == MethodName.BallSpawn)
		{
			return true;
		}
		if (method == MethodName.CreateBallAt)
		{
			return true;
		}
		if (method == MethodName.GetSpawnZombie)
		{
			return true;
		}
		if (method == MethodName.SetStateList)
		{
			return true;
		}
		if (method == MethodName.ExplosionMethod)
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
		if (method == MethodName.InWaterDiscardSet)
		{
			return true;
		}
		if (method == MethodName.OutWaterDiscardSet)
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
		if (name == PropertyName._bossGameplayInitialized)
		{
			_bossGameplayInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.stageSpawnList)
		{
			stageSpawnList = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.stateMethodList)
		{
			stateMethodList = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.useEnterAnime)
		{
			useEnterAnime = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.stateNow)
		{
			stateNow = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.spawnZombieNumOnce)
		{
			spawnZombieNumOnce = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.spawnZombieNumNow)
		{
			spawnZombieNumNow = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.spawnTime)
		{
			spawnTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spawnLine)
		{
			spawnLine = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.spawnRestTime)
		{
			spawnRestTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spawnRestTimer)
		{
			spawnRestTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.spawnZomie)
		{
			spawnZomie = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.spawnNum)
		{
			spawnNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.headAttackOver)
		{
			headAttackOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.headAttackLine)
		{
			headAttackLine = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.headAttackIsFire)
		{
			headAttackIsFire = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.headAttackRestTime)
		{
			headAttackRestTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.headAttackRestTimer)
		{
			headAttackRestTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.bungeeIsSpawn)
		{
			bungeeIsSpawn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.bungeeList)
		{
			bungeeList = VariantUtils.ConvertTo<Array>(in value);
			return true;
		}
		if (name == PropertyName.stompId)
		{
			stompId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.rvPos)
		{
			rvPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.isRest)
		{
			isRest = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.restTime)
		{
			restTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.restTimer)
		{
			restTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.headIdleNum)
		{
			headIdleNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.stage)
		{
			stage = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._headAttackPauseRemaining)
		{
			_headAttackPauseRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._headAttackPauseOwned)
		{
			_headAttackPauseOwned = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName._bossGameplayInitialized)
		{
			value = VariantUtils.CreateFrom(in _bossGameplayInitialized);
			return true;
		}
		if (name == PropertyName.stageSpawnList)
		{
			value = VariantUtils.CreateFrom(in stageSpawnList);
			return true;
		}
		if (name == PropertyName.stateMethodList)
		{
			value = VariantUtils.CreateFrom(in stateMethodList);
			return true;
		}
		if (name == PropertyName.useEnterAnime)
		{
			value = VariantUtils.CreateFrom(in useEnterAnime);
			return true;
		}
		if (name == PropertyName.stateNow)
		{
			value = VariantUtils.CreateFrom(in stateNow);
			return true;
		}
		if (name == PropertyName.spawnZombieNumOnce)
		{
			value = VariantUtils.CreateFrom(in spawnZombieNumOnce);
			return true;
		}
		if (name == PropertyName.spawnZombieNumNow)
		{
			value = VariantUtils.CreateFrom(in spawnZombieNumNow);
			return true;
		}
		if (name == PropertyName.spawnTime)
		{
			value = VariantUtils.CreateFrom(in spawnTime);
			return true;
		}
		if (name == PropertyName.spawnLine)
		{
			value = VariantUtils.CreateFrom(in spawnLine);
			return true;
		}
		if (name == PropertyName.spawnRestTime)
		{
			value = VariantUtils.CreateFrom(in spawnRestTime);
			return true;
		}
		if (name == PropertyName.spawnRestTimer)
		{
			value = VariantUtils.CreateFrom(in spawnRestTimer);
			return true;
		}
		if (name == PropertyName.spawnZomie)
		{
			value = VariantUtils.CreateFrom(in spawnZomie);
			return true;
		}
		if (name == PropertyName.spawnNum)
		{
			value = VariantUtils.CreateFrom(in spawnNum);
			return true;
		}
		if (name == PropertyName.headAttackOver)
		{
			value = VariantUtils.CreateFrom(in headAttackOver);
			return true;
		}
		if (name == PropertyName.headAttackLine)
		{
			value = VariantUtils.CreateFrom(in headAttackLine);
			return true;
		}
		if (name == PropertyName.headAttackIsFire)
		{
			value = VariantUtils.CreateFrom(in headAttackIsFire);
			return true;
		}
		if (name == PropertyName.headAttackRestTime)
		{
			value = VariantUtils.CreateFrom(in headAttackRestTime);
			return true;
		}
		if (name == PropertyName.headAttackRestTimer)
		{
			value = VariantUtils.CreateFrom(in headAttackRestTimer);
			return true;
		}
		if (name == PropertyName.bungeeIsSpawn)
		{
			value = VariantUtils.CreateFrom(in bungeeIsSpawn);
			return true;
		}
		if (name == PropertyName.bungeeList)
		{
			value = VariantUtils.CreateFrom(in bungeeList);
			return true;
		}
		if (name == PropertyName.stompId)
		{
			value = VariantUtils.CreateFrom(in stompId);
			return true;
		}
		if (name == PropertyName.rvPos)
		{
			value = VariantUtils.CreateFrom(in rvPos);
			return true;
		}
		if (name == PropertyName.isRest)
		{
			value = VariantUtils.CreateFrom(in isRest);
			return true;
		}
		if (name == PropertyName.restTime)
		{
			value = VariantUtils.CreateFrom(in restTime);
			return true;
		}
		if (name == PropertyName.restTimer)
		{
			value = VariantUtils.CreateFrom(in restTimer);
			return true;
		}
		if (name == PropertyName.headIdleNum)
		{
			value = VariantUtils.CreateFrom(in headIdleNum);
			return true;
		}
		if (name == PropertyName.stage)
		{
			value = VariantUtils.CreateFrom(in stage);
			return true;
		}
		if (name == PropertyName._headAttackPauseRemaining)
		{
			value = VariantUtils.CreateFrom(in _headAttackPauseRemaining);
			return true;
		}
		if (name == PropertyName._headAttackPauseOwned)
		{
			value = VariantUtils.CreateFrom(in _headAttackPauseOwned);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName._bossGameplayInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.stageSpawnList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.stateMethodList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useEnterAnime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.stateNow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.spawnZombieNumOnce, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.spawnZombieNumNow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.spawnTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.spawnLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.spawnRestTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.spawnRestTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.spawnZomie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.spawnNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.headAttackOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.headAttackLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.headAttackIsFire, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.headAttackRestTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.headAttackRestTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.bungeeIsSpawn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.bungeeList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.stompId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.rvPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isRest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.restTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.restTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.headIdleNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.stage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._headAttackPauseRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._headAttackPauseOwned, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._bossGameplayInitialized, Variant.From(in _bossGameplayInitialized));
		info.AddProperty(PropertyName.stageSpawnList, Variant.From(in stageSpawnList));
		info.AddProperty(PropertyName.stateMethodList, Variant.From(in stateMethodList));
		info.AddProperty(PropertyName.useEnterAnime, Variant.From(in useEnterAnime));
		info.AddProperty(PropertyName.stateNow, Variant.From(in stateNow));
		info.AddProperty(PropertyName.spawnZombieNumOnce, Variant.From(in spawnZombieNumOnce));
		info.AddProperty(PropertyName.spawnZombieNumNow, Variant.From(in spawnZombieNumNow));
		info.AddProperty(PropertyName.spawnTime, Variant.From(in spawnTime));
		info.AddProperty(PropertyName.spawnLine, Variant.From(in spawnLine));
		info.AddProperty(PropertyName.spawnRestTime, Variant.From(in spawnRestTime));
		info.AddProperty(PropertyName.spawnRestTimer, Variant.From(in spawnRestTimer));
		info.AddProperty(PropertyName.spawnZomie, Variant.From(in spawnZomie));
		info.AddProperty(PropertyName.spawnNum, Variant.From(in spawnNum));
		info.AddProperty(PropertyName.headAttackOver, Variant.From(in headAttackOver));
		info.AddProperty(PropertyName.headAttackLine, Variant.From(in headAttackLine));
		info.AddProperty(PropertyName.headAttackIsFire, Variant.From(in headAttackIsFire));
		info.AddProperty(PropertyName.headAttackRestTime, Variant.From(in headAttackRestTime));
		info.AddProperty(PropertyName.headAttackRestTimer, Variant.From(in headAttackRestTimer));
		info.AddProperty(PropertyName.bungeeIsSpawn, Variant.From(in bungeeIsSpawn));
		info.AddProperty(PropertyName.bungeeList, Variant.From(in bungeeList));
		info.AddProperty(PropertyName.stompId, Variant.From(in stompId));
		info.AddProperty(PropertyName.rvPos, Variant.From(in rvPos));
		info.AddProperty(PropertyName.isRest, Variant.From(in isRest));
		info.AddProperty(PropertyName.restTime, Variant.From(in restTime));
		info.AddProperty(PropertyName.restTimer, Variant.From(in restTimer));
		info.AddProperty(PropertyName.headIdleNum, Variant.From(in headIdleNum));
		info.AddProperty(PropertyName.stage, Variant.From(in stage));
		info.AddProperty(PropertyName._headAttackPauseRemaining, Variant.From(in _headAttackPauseRemaining));
		info.AddProperty(PropertyName._headAttackPauseOwned, Variant.From(in _headAttackPauseOwned));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value))
		{
			_stateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._bossGameplayInitialized, out var value2))
		{
			_bossGameplayInitialized = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.stageSpawnList, out var value3))
		{
			stageSpawnList = value3.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.stateMethodList, out var value4))
		{
			stateMethodList = value4.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.useEnterAnime, out var value5))
		{
			useEnterAnime = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.stateNow, out var value6))
		{
			stateNow = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.spawnZombieNumOnce, out var value7))
		{
			spawnZombieNumOnce = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.spawnZombieNumNow, out var value8))
		{
			spawnZombieNumNow = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.spawnTime, out var value9))
		{
			spawnTime = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spawnLine, out var value10))
		{
			spawnLine = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.spawnRestTime, out var value11))
		{
			spawnRestTime = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spawnRestTimer, out var value12))
		{
			spawnRestTimer = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName.spawnZomie, out var value13))
		{
			spawnZomie = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName.spawnNum, out var value14))
		{
			spawnNum = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName.headAttackOver, out var value15))
		{
			headAttackOver = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.headAttackLine, out var value16))
		{
			headAttackLine = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName.headAttackIsFire, out var value17))
		{
			headAttackIsFire = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.headAttackRestTime, out var value18))
		{
			headAttackRestTime = value18.As<double>();
		}
		if (info.TryGetProperty(PropertyName.headAttackRestTimer, out var value19))
		{
			headAttackRestTimer = value19.As<double>();
		}
		if (info.TryGetProperty(PropertyName.bungeeIsSpawn, out var value20))
		{
			bungeeIsSpawn = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.bungeeList, out var value21))
		{
			bungeeList = value21.As<Array>();
		}
		if (info.TryGetProperty(PropertyName.stompId, out var value22))
		{
			stompId = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName.rvPos, out var value23))
		{
			rvPos = value23.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.isRest, out var value24))
		{
			isRest = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.restTime, out var value25))
		{
			restTime = value25.As<double>();
		}
		if (info.TryGetProperty(PropertyName.restTimer, out var value26))
		{
			restTimer = value26.As<double>();
		}
		if (info.TryGetProperty(PropertyName.headIdleNum, out var value27))
		{
			headIdleNum = value27.As<int>();
		}
		if (info.TryGetProperty(PropertyName.stage, out var value28))
		{
			stage = value28.As<int>();
		}
		if (info.TryGetProperty(PropertyName._headAttackPauseRemaining, out var value29))
		{
			_headAttackPauseRemaining = value29.As<double>();
		}
		if (info.TryGetProperty(PropertyName._headAttackPauseOwned, out var value30))
		{
			_headAttackPauseOwned = value30.As<bool>();
		}
	}
}
