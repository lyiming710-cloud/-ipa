using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Boss/BossDave/Scene/TowerDefenseZombieBossDave.cs")]
public class TowerDefenseZombieBossDave : TowerDefenseZombie, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";

		public new static readonly StringName IsProgressStateMachineDefinitionIdCompatible = "IsProgressStateMachineDefinitionIdCompatible";

		public static readonly StringName ResolveBossRuntimeComponents = "ResolveBossRuntimeComponents";

		public static readonly StringName InitializeBossGameplay = "InitializeBossGameplay";

		public static readonly StringName ConnectDoomShieldSignals = "ConnectDoomShieldSignals";

		public static readonly StringName DisconnectDoomShieldSignals = "DisconnectDoomShieldSignals";

		public static readonly StringName ConnectMissileLifecycleSignals = "ConnectMissileLifecycleSignals";

		public static readonly StringName DisconnectMissileLifecycleSignals = "DisconnectMissileLifecycleSignals";

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

		public static readonly StringName PlantEntered = "PlantEntered";

		public static readonly StringName PlantProcessing = "PlantProcessing";

		public static readonly StringName PlantExited = "PlantExited";

		public static readonly StringName MissileEntered = "MissileEntered";

		public static readonly StringName MissileProcessing = "MissileProcessing";

		public static readonly StringName MissileExited = "MissileExited";

		public static readonly StringName DoomStartEntered = "DoomStartEntered";

		public static readonly StringName DoomStartProcessing = "DoomStartProcessing";

		public static readonly StringName DoomStartExited = "DoomStartExited";

		public static readonly StringName DoomIdleEntered = "DoomIdleEntered";

		public static readonly StringName DoomIdleProcessing = "DoomIdleProcessing";

		public static readonly StringName DoomIdleExited = "DoomIdleExited";

		public static readonly StringName DoomEndEntered = "DoomEndEntered";

		public static readonly StringName DoomEndProcessing = "DoomEndProcessing";

		public static readonly StringName DoomEndExited = "DoomEndExited";

		public static readonly StringName DoomCancelEntered = "DoomCancelEntered";

		public static readonly StringName DoomCancelProcessing = "DoomCancelProcessing";

		public static readonly StringName DoomCancelExited = "DoomCancelExited";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName UpdateHeadAttackPause = "UpdateHeadAttackPause";

		public static readonly StringName BeginHeadAttackPause = "BeginHeadAttackPause";

		public static readonly StringName ReleaseHeadAttackPause = "ReleaseHeadAttackPause";

		public static readonly StringName ApplyRemoteHeadAttackPause = "ApplyRemoteHeadAttackPause";

		public static readonly StringName StateRunning = "StateRunning";

		public static readonly StringName RunSpawnSequence = "RunSpawnSequence";

		public static readonly StringName ZombieSpawn = "ZombieSpawn";

		public static readonly StringName BungeeSpawn = "BungeeSpawn";

		public static readonly StringName RVSpawn = "RVSpawn";

		public static readonly StringName BallSpawn = "BallSpawn";

		public static readonly StringName CreateBallAt = "CreateBallAt";

		public static readonly StringName GetSpawnZombie = "GetSpawnZombie";

		public static readonly StringName SetStateList = "SetStateList";

		public static readonly StringName GetValidStandingSpecials = "GetValidStandingSpecials";

		public static readonly StringName GetAvailableStompIds = "GetAvailableStompIds";

		public static readonly StringName GetBungeeTargetPositions = "GetBungeeTargetPositions";

		public static readonly StringName HasRvTarget = "HasRvTarget";

		public static readonly StringName CanForcePlantOnEmptyCell = "CanForcePlantOnEmptyCell";

		public static readonly StringName IsPlantFootprintCellEmpty = "IsPlantFootprintCellEmpty";

		public static readonly StringName CanSpawnHypnotizedPlant = "CanSpawnHypnotizedPlant";

		public static readonly StringName SpawnHypnotizedPlants = "SpawnHypnotizedPlants";

		public static readonly StringName StartNextCrouchSkill = "StartNextCrouchSkill";

		public static readonly StringName QueueCrouchShowcase = "QueueCrouchShowcase";

		public static readonly StringName SetCrouchPresentation = "SetCrouchPresentation";

		public static readonly StringName PrepareMissileAttack = "PrepareMissileAttack";

		public static readonly StringName SpawnNextMissile = "SpawnNextMissile";

		public static readonly StringName CatchUpRemoteMissileVisuals = "CatchUpRemoteMissileVisuals";

		public static readonly StringName SpawnMissileProjectile = "SpawnMissileProjectile";

		public static readonly StringName OnMissileLifecycleEnded = "OnMissileLifecycleEnded";

		public static readonly StringName IsMissileSlotResolved = "IsMissileSlotResolved";

		public static readonly StringName GetAllMissileSlotsMask = "GetAllMissileSlotsMask";

		public static readonly StringName ResolveMissileSlot = "ResolveMissileSlot";

		public static readonly StringName RefreshMissileCrosshairs = "RefreshMissileCrosshairs";

		public static readonly StringName ClearMissileCrosshairs = "ClearMissileCrosshairs";

		public static readonly StringName EndMissileAttack = "EndMissileAttack";

		public static readonly StringName SetExposedHeadDamageableState = "SetExposedHeadDamageableState";

		public static readonly StringName SetRetractedHeadState = "SetRetractedHeadState";

		public static readonly StringName SetDoomDamageableState = "SetDoomDamageableState";

		public static readonly StringName ApplyBossPresentationForClip = "ApplyBossPresentationForClip";

		public static readonly StringName AdvanceDoomCharge = "AdvanceDoomCharge";

		public static readonly StringName EnsureDoomShield = "EnsureDoomShield";

		public static readonly StringName RemoveDoomShield = "RemoveDoomShield";

		public static readonly StringName OnDoomShieldBroken = "OnDoomShieldBroken";

		public static readonly StringName ApplyPendingLegacyDoomShieldDamage = "ApplyPendingLegacyDoomShieldDamage";

		public static readonly StringName ReleaseDoomAttack = "ReleaseDoomAttack";

		public static readonly StringName PlayDoomReleaseEffect = "PlayDoomReleaseEffect";

		public static readonly StringName FinishStandingSpecial = "FinishStandingSpecial";

		public static readonly StringName MarkBossStateDirty = "MarkBossStateDirty";

		public static readonly StringName IsBossGameplayPaused = "IsBossGameplayPaused";

		public static readonly StringName ExplosionMethod = "ExplosionMethod";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName InWaterDiscardSet = "InWaterDiscardSet";

		public new static readonly StringName OutWaterDiscardSet = "OutWaterDiscardSet";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpawnState = "ExportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName IsNetworkSpecialMovementActive = "IsNetworkSpecialMovementActive";

		public new static readonly StringName OnRemoteNetworkAnimationStart = "OnRemoteNetworkAnimationStart";

		public static readonly StringName ExportBossNetworkState = "ExportBossNetworkState";

		public static readonly StringName ImportBossNetworkState = "ImportBossNetworkState";

		public static readonly StringName SerializeGridPositions = "SerializeGridPositions";

		public static readonly StringName DeserializeGridPositions = "DeserializeGridPositions";

		public static readonly StringName ReadStringArray = "ReadStringArray";

		public static readonly StringName GetSavedBungeeNodeNames = "GetSavedBungeeNodeNames";

		public static readonly StringName ResolveSavedBungeeRelations = "ResolveSavedBungeeRelations";

		public static readonly StringName ApplyPendingBossNetworkPresentation = "ApplyPendingBossNetworkPresentation";

		public static readonly StringName ApplyStagePresentation = "ApplyStagePresentation";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public new static readonly StringName IsHardControlImmune = "IsHardControlImmune";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _bossGameplayInitialized = "_bossGameplayInitialized";

		public static readonly StringName abilityConfig = "abilityConfig";

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

		public static readonly StringName activeCrouchSkill = "activeCrouchSkill";

		public static readonly StringName pendingCrouchShowcases = "pendingCrouchShowcases";

		public static readonly StringName missileTargets = "missileTargets";

		public static readonly StringName missileTargetIndex = "missileTargetIndex";

		public static readonly StringName doomChargeRemaining = "doomChargeRemaining";

		public static readonly StringName _missileAttackActive = "_missileAttackActive";

		public static readonly StringName _plantAttackActive = "_plantAttackActive";

		public static readonly StringName _plantSpawnResolved = "_plantSpawnResolved";

		public static readonly StringName _doomCharging = "_doomCharging";

		public static readonly StringName _doomAttackResolved = "_doomAttackResolved";

		public static readonly StringName _rvAttackArmed = "_rvAttackArmed";

		public static readonly StringName _missileShowcaseQueued = "_missileShowcaseQueued";

		public static readonly StringName _doomShowcaseQueued = "_doomShowcaseQueued";

		public static readonly StringName _doomShieldSignalsConnected = "_doomShieldSignalsConnected";

		public static readonly StringName _removingDoomShield = "_removingDoomShield";

		public static readonly StringName _hasPendingLegacyDoomShieldDamage = "_hasPendingLegacyDoomShieldDamage";

		public static readonly StringName _pendingLegacyDoomShieldDamage = "_pendingLegacyDoomShieldDamage";

		public static readonly StringName _plantActionSequence = "_plantActionSequence";

		public static readonly StringName _missileActionSequence = "_missileActionSequence";

		public static readonly StringName _doomActionSequence = "_doomActionSequence";

		public static readonly StringName _networkSpecialStateRevision = "_networkSpecialStateRevision";

		public static readonly StringName _remoteMissileVisualIndex = "_remoteMissileVisualIndex";

		public static readonly StringName _remoteMissileActionSequence = "_remoteMissileActionSequence";

		public static readonly StringName _clearLegacyMissilePauseOnFinalize = "_clearLegacyMissilePauseOnFinalize";

		public static readonly StringName _spawnLegacyDelayedMissileOnFinalize = "_spawnLegacyDelayedMissileOnFinalize";

		public static readonly StringName _legacyMissileEventFrame = "_legacyMissileEventFrame";

		public static readonly StringName missileResolvedMask = "missileResolvedMask";

		public static readonly StringName _missileCrosshairTrackingActive = "_missileCrosshairTrackingActive";

		public static readonly StringName _missileLifecycleSignalsConnected = "_missileLifecycleSignalsConnected";

		public static readonly StringName _headAttackPauseRemaining = "_headAttackPauseRemaining";

		public static readonly StringName _headAttackPauseSequence = "_headAttackPauseSequence";

		public static readonly StringName _remoteHeadAttackPauseSequence = "_remoteHeadAttackPauseSequence";

		public static readonly StringName _headAttackPauseOwned = "_headAttackPauseOwned";

		public static readonly StringName _preserveProgressStateOnNextWalk = "_preserveProgressStateOnNextWalk";

		public static readonly StringName _clearLegacyHeadAttackPauseOnFinalize = "_clearLegacyHeadAttackPauseOnFinalize";

		public static readonly StringName _networkSyncAccumulator = "_networkSyncAccumulator";

		public static readonly StringName _hasPendingNetworkPresentation = "_hasPendingNetworkPresentation";

		public static readonly StringName _pendingBungeeNodeNames = "_pendingBungeeNodeNames";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const int BossDaveSaveVersion = 4;

	private const string DoomShieldArmorName = "BossDaveDoomShield";

	private StateHandle _headIdleStateHandle;

	private StateHandle _headAttackStateHandle;

	private StateHandle _headExitedStateHandle;

	private StateHandle _spawnStateHandle;

	private StateHandle _stompStateHandle;

	private StateHandle _bungeeStateHandle;

	private StateHandle _rvStateHandle;

	private StateHandle _plantStateHandle;

	private StateHandle _missileStateHandle;

	private StateHandle _doomStartStateHandle;

	private StateHandle _doomIdleStateHandle;

	private StateHandle _doomEndStateHandle;

	private StateHandle _doomCancelStateHandle;

	private bool _stateSignalsConnected;

	private bool _bossGameplayInitialized;

	private static PackedScene _FIRE_BALL;

	private static PackedScene _ICE_BALL;

	private static PackedScene _EXPLOSION;

	private static PackedScene _JACK_MISSILE;

	private static PackedScene _DOOMSHROOM_EXPLOSION;

	private static PackedScene _JACKBOX_EXPLOSION;

	private static Texture2D _CROSSHAIR_TEX;

	private AttackComponent _attackComponent1;

	private AttackComponent _attackComponent2;

	private AttackComponent _attackComponent3;

	private AttackComponent _attackComponent4;

	private AttackComponent _attackComponent5;

	[Export(PropertyHint.None, "")]
	public TowerDefenseZombieBossDaveAbilityConfig abilityConfig;

	public Godot.Collections.Array stateMethodList = new Godot.Collections.Array();

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

	public Godot.Collections.Array bungeeList = new Godot.Collections.Array();

	public int stompId = -1;

	public Vector2I rvPos;

	public bool isRest = true;

	public double restTime = 5.0;

	public double restTimer;

	public int headIdleNum;

	public int stage;

	public string activeCrouchSkill = "";

	public Array<string> pendingCrouchShowcases = new Array<string>();

	public Array<Vector2I> missileTargets = new Array<Vector2I>();

	public int missileTargetIndex;

	public double doomChargeRemaining;

	private bool _missileAttackActive;

	private bool _plantAttackActive;

	private bool _plantSpawnResolved;

	private bool _doomCharging;

	private bool _doomAttackResolved;

	private bool _rvAttackArmed;

	private bool _missileShowcaseQueued;

	private bool _doomShowcaseQueued;

	private bool _doomShieldSignalsConnected;

	private bool _removingDoomShield;

	private bool _hasPendingLegacyDoomShieldDamage;

	private double _pendingLegacyDoomShieldDamage;

	private int _plantActionSequence;

	private int _missileActionSequence;

	private int _doomActionSequence;

	private int _networkSpecialStateRevision;

	private int _remoteMissileVisualIndex;

	private int _remoteMissileActionSequence = -1;

	private bool _clearLegacyMissilePauseOnFinalize;

	private bool _spawnLegacyDelayedMissileOnFinalize;

	private int _legacyMissileEventFrame = -1;

	private int missileResolvedMask;

	private bool _missileCrosshairTrackingActive;

	private bool _missileLifecycleSignalsConnected;

	private double _headAttackPauseRemaining;

	private int _headAttackPauseSequence;

	private int _remoteHeadAttackPauseSequence = -1;

	private bool _headAttackPauseOwned;

	private bool _preserveProgressStateOnNextWalk;

	private bool _clearLegacyHeadAttackPauseOnFinalize;

	private double _networkSyncAccumulator;

	private bool _hasPendingNetworkPresentation;

	private readonly System.Collections.Generic.Dictionary<int, Sprite2D> _missileCrosshairs = new System.Collections.Generic.Dictionary<int, Sprite2D>();

	private Array<string> _pendingBungeeNodeNames = new Array<string>();

	private static PackedScene FIRE_BALL => _FIRE_BALL ?? (_FIRE_BALL = GD.Load<PackedScene>("uid://ulproewvrqwb"));

	private static PackedScene ICE_BALL => _ICE_BALL ?? (_ICE_BALL = GD.Load<PackedScene>("uid://calwfqgd47fn7"));

	private static PackedScene EXPLOSION => _EXPLOSION ?? (_EXPLOSION = GD.Load<PackedScene>("uid://c8xarvk5gxpf0"));

	private static PackedScene JACK_MISSILE => _JACK_MISSILE ?? (_JACK_MISSILE = GD.Load<PackedScene>("uid://bednjhda3x6ky"));

	private static PackedScene DOOMSHROOM_EXPLOSION => _DOOMSHROOM_EXPLOSION ?? (_DOOMSHROOM_EXPLOSION = GD.Load<PackedScene>("uid://0hfxonqijrv0"));

	private static PackedScene JACKBOX_EXPLOSION => _JACKBOX_EXPLOSION ?? (_JACKBOX_EXPLOSION = GD.Load<PackedScene>("uid://cxnt2jbnk48fp"));

	private static Texture2D CROSSHAIR_TEX => _CROSSHAIR_TEX ?? (_CROSSHAIR_TEX = GD.Load<Texture2D>("uid://c07s22oke4426"));

	public override bool IsHardControlImmune => _doomCharging;

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
		_plantStateHandle = StateMachine?.GetStateById("zombie.boss.plant");
		_missileStateHandle = StateMachine?.GetStateById("zombie.boss.missile");
		_doomStartStateHandle = StateMachine?.GetStateById("zombie.boss.doom_start");
		_doomIdleStateHandle = StateMachine?.GetStateById("zombie.boss.doom_idle");
		_doomEndStateHandle = StateMachine?.GetStateById("zombie.boss.doom_end");
		_doomCancelStateHandle = StateMachine?.GetStateById("zombie.boss.doom_cancel");
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
		if (bungeeStateHandle == null || !bungeeStateHandle.IsValid)
		{
			return;
		}
		StateHandle rvStateHandle = _rvStateHandle;
		if (rvStateHandle == null || !rvStateHandle.IsValid)
		{
			return;
		}
		StateHandle plantStateHandle = _plantStateHandle;
		if (plantStateHandle == null || !plantStateHandle.IsValid)
		{
			return;
		}
		StateHandle missileStateHandle = _missileStateHandle;
		if (missileStateHandle == null || !missileStateHandle.IsValid)
		{
			return;
		}
		StateHandle doomStartStateHandle = _doomStartStateHandle;
		if (doomStartStateHandle == null || !doomStartStateHandle.IsValid)
		{
			return;
		}
		StateHandle doomIdleStateHandle = _doomIdleStateHandle;
		if (doomIdleStateHandle == null || !doomIdleStateHandle.IsValid)
		{
			return;
		}
		StateHandle doomEndStateHandle = _doomEndStateHandle;
		if (doomEndStateHandle != null && doomEndStateHandle.IsValid)
		{
			StateHandle doomCancelStateHandle = _doomCancelStateHandle;
			if (doomCancelStateHandle != null && doomCancelStateHandle.IsValid)
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
				_plantStateHandle.Entered += PlantEntered;
				_plantStateHandle.Exited += PlantExited;
				_plantStateHandle.PhysicsProcessing += PlantProcessing;
				_missileStateHandle.Entered += MissileEntered;
				_missileStateHandle.Exited += MissileExited;
				_missileStateHandle.PhysicsProcessing += MissileProcessing;
				_doomStartStateHandle.Entered += DoomStartEntered;
				_doomStartStateHandle.Exited += DoomStartExited;
				_doomStartStateHandle.PhysicsProcessing += DoomStartProcessing;
				_doomIdleStateHandle.Entered += DoomIdleEntered;
				_doomIdleStateHandle.Exited += DoomIdleExited;
				_doomIdleStateHandle.PhysicsProcessing += DoomIdleProcessing;
				_doomEndStateHandle.Entered += DoomEndEntered;
				_doomEndStateHandle.Exited += DoomEndExited;
				_doomEndStateHandle.PhysicsProcessing += DoomEndProcessing;
				_doomCancelStateHandle.Entered += DoomCancelEntered;
				_doomCancelStateHandle.Exited += DoomCancelExited;
				_doomCancelStateHandle.PhysicsProcessing += DoomCancelProcessing;
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
			if (_plantStateHandle != null)
			{
				_plantStateHandle.Entered -= PlantEntered;
				_plantStateHandle.Exited -= PlantExited;
				_plantStateHandle.PhysicsProcessing -= PlantProcessing;
			}
			_plantStateHandle = null;
			if (_missileStateHandle != null)
			{
				_missileStateHandle.Entered -= MissileEntered;
				_missileStateHandle.Exited -= MissileExited;
				_missileStateHandle.PhysicsProcessing -= MissileProcessing;
			}
			_missileStateHandle = null;
			if (_doomStartStateHandle != null)
			{
				_doomStartStateHandle.Entered -= DoomStartEntered;
				_doomStartStateHandle.Exited -= DoomStartExited;
				_doomStartStateHandle.PhysicsProcessing -= DoomStartProcessing;
			}
			_doomStartStateHandle = null;
			if (_doomIdleStateHandle != null)
			{
				_doomIdleStateHandle.Entered -= DoomIdleEntered;
				_doomIdleStateHandle.Exited -= DoomIdleExited;
				_doomIdleStateHandle.PhysicsProcessing -= DoomIdleProcessing;
			}
			_doomIdleStateHandle = null;
			if (_doomEndStateHandle != null)
			{
				_doomEndStateHandle.Entered -= DoomEndEntered;
				_doomEndStateHandle.Exited -= DoomEndExited;
				_doomEndStateHandle.PhysicsProcessing -= DoomEndProcessing;
			}
			_doomEndStateHandle = null;
			if (_doomCancelStateHandle != null)
			{
				_doomCancelStateHandle.Entered -= DoomCancelEntered;
				_doomCancelStateHandle.Exited -= DoomCancelExited;
				_doomCancelStateHandle.PhysicsProcessing -= DoomCancelProcessing;
			}
			_doomCancelStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		DisconnectStateSignals();
		DisconnectDoomShieldSignals();
		DisconnectMissileLifecycleSignals();
		ClearMissileCrosshairs();
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			ResolveBossRuntimeComponents();
			InitializeBossGameplay();
			ConnectMissileLifecycleSignals();
			Callable.From(ApplyPendingBossNetworkPresentation).CallDeferred();
		}
	}

	public override void FinalizeProgressRestore()
	{
		string text = CurrentStateHandle?.StableId.ToString() ?? "";
		_preserveProgressStateOnNextWalk = text.StartsWith("zombie.boss.");
		base.FinalizeProgressRestore();
		ResolveSavedBungeeRelations();
		_hasPendingNetworkPresentation = true;
		ApplyPendingBossNetworkPresentation();
		if (_clearLegacyHeadAttackPauseOnFinalize && _headAttackPauseRemaining <= 0.0 && !_headAttackPauseOwned && spritePause && sprite?.clip == "HeadAttack4")
		{
			spritePause = false;
		}
		_clearLegacyHeadAttackPauseOnFinalize = false;
	}

	protected override bool IsProgressStateMachineDefinitionIdCompatible(string savedDefinitionId, string currentDefinitionId)
	{
		if (!base.IsProgressStateMachineDefinitionIdCompatible(savedDefinitionId, currentDefinitionId))
		{
			if (savedDefinitionId == "builtin.character.zombie.boss")
			{
				return currentDefinitionId == "builtin.character.zombie.boss_dave";
			}
			return false;
		}
		return true;
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
			ConnectDoomShieldSignals();
			RefreshMissileCrosshairs();
			WaitForPhysicsFrame();
		}
	}

	private void ConnectDoomShieldSignals()
	{
		if (!_doomShieldSignalsConnected && GodotObject.IsInstanceValid(instance))
		{
			instance.armorHitpointsEmpty += OnDoomShieldBroken;
			_doomShieldSignalsConnected = true;
		}
	}

	private void DisconnectDoomShieldSignals()
	{
		if (_doomShieldSignalsConnected)
		{
			if (GodotObject.IsInstanceValid(instance))
			{
				instance.armorHitpointsEmpty -= OnDoomShieldBroken;
			}
			_doomShieldSignalsConnected = false;
		}
	}

	private void ConnectMissileLifecycleSignals()
	{
		if (!_missileLifecycleSignalsConnected && GodotObject.IsInstanceValid(BulletField.Instance))
		{
			BulletField.Instance.OnBulletLifecycleEnded += OnMissileLifecycleEnded;
			_missileLifecycleSignalsConnected = true;
		}
	}

	private void DisconnectMissileLifecycleSignals()
	{
		if (_missileLifecycleSignalsConnected)
		{
			if (GodotObject.IsInstanceValid(BulletField.Instance))
			{
				BulletField.Instance.OnBulletLifecycleEnded -= OnMissileLifecycleEnded;
			}
			_missileLifecycleSignalsConnected = false;
		}
	}

	private async Task WaitForPhysicsFrame()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		useEnterAnime = true;
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
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		_networkSyncAccumulator += delta;
		if (_networkSyncAccumulator >= 0.5)
		{
			_networkSyncAccumulator = 0.0;
			MarkBossStateDirty();
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
		if (stateMethodList.Count == 0)
		{
			SetStateList();
		}
		if (stateMethodList.Count != 0)
		{
			restTimer = 0.0;
			stateNow = (string)stateMethodList[0];
			stateMethodList.RemoveAt(0);
			isRest = false;
			if (stateNow != "HeadIdle" && stateMethodList.Count == 0)
			{
				stateMethodList.Add("HeadIdle");
			}
			MarkBossStateDirty();
		}
	}

	public override void Walk()
	{
		if (_preserveProgressStateOnNextWalk)
		{
			_preserveProgressStateOnNextWalk = false;
		}
		else
		{
			Idle();
		}
	}

	public void HeadIdleEntered()
	{
		AudioManager.Instance.AudioPlay("HydraulicShort");
		AudioManager.Instance.AudioPlay("Hydraulic");
		if (TowerDefenseManager.HasGameplayAuthority && !IsProgressRestoreInFlight)
		{
			headAttackRestTimer = 0.0;
		}
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
		if (IsBossGameplayPaused() || !TowerDefenseManager.HasGameplayAuthority || sprite.pause)
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
			StartNextCrouchSkill();
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
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			headAttackOver = true;
		}
		SetTransientRenderZIndex((int)(headAttackLine * 15 + itemLayer), "zomboss-head-attack");
		ZombieBossDave zombieBossDave = sprite as ZombieBossDave;
		if (GodotObject.IsInstanceValid(zombieBossDave))
		{
			zombieBossDave.SetHeadAttack(headAttackLine);
			zombieBossDave.SetHeadAttackBall((activeCrouchSkill == "Fire") ? "Fire" : "Ice");
		}
		sprite.SetAnimation("HeadAttack4", loop: false, 0.2);
	}

	public void HeadAttackProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void HeadAttackExited()
	{
		ReleaseHeadAttackPause(markDirty: false);
	}

	public void HeadExitedEntered()
	{
		SetRetractedHeadState();
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
		if (TowerDefenseManager.HasGameplayAuthority && !IsProgressRestoreInFlight)
		{
			spawnZomie = GetSpawnZombie();
			spawnLine = GD.RandRange(1, TowerDefenseManager.Instance.GetMapGridNum().Y);
			MarkBossStateDirty();
		}
		sprite.SetAnimation("Spawn1", loop: false, 0.2);
		ZombieBossDave zombieBossDave = sprite as ZombieBossDave;
		if (GodotObject.IsInstanceValid(zombieBossDave))
		{
			zombieBossDave.SetSpawn(spawnLine);
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
		if (TowerDefenseManager.HasGameplayAuthority && !IsProgressRestoreInFlight)
		{
			Array<int> availableStompIds = GetAvailableStompIds();
			stompId = ((availableStompIds.Count > 0) ? availableStompIds.PickRandom() : 3);
			MarkBossStateDirty();
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
		if (!IsProgressRestoreInFlight)
		{
			bungeeIsSpawn = false;
		}
		sprite.SetAnimation("BungeeEnter", loop: false, 0.2);
	}

	public void BungeeProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		if (IsBossGameplayPaused() || !TowerDefenseManager.HasGameplayAuthority || !bungeeIsSpawn)
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
		if (TowerDefenseManager.HasGameplayAuthority && !IsProgressRestoreInFlight)
		{
			_rvAttackArmed = false;
			rvPos = new Vector2I((int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().X / 2f - 1f), TowerDefenseManager.Instance.GetMapGridNum().Y - 1);
			MarkBossStateDirty();
		}
		sprite.SetAnimation("RV", loop: false, 0.2);
	}

	public void RVProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void RVExited()
	{
	}

	public void PlantEntered()
	{
		AudioManager.Instance.AudioPlay("HydraulicShort");
		if (TowerDefenseManager.HasGameplayAuthority && !IsProgressRestoreInFlight)
		{
			_plantAttackActive = true;
			_plantSpawnResolved = false;
			_plantActionSequence++;
			MarkBossStateDirty();
		}
		sprite.SetAnimation("PlantEnter", loop: false, 0.2);
	}

	public void PlantProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void PlantExited()
	{
	}

	public void MissileEntered()
	{
		if (TowerDefenseManager.HasGameplayAuthority && !IsProgressRestoreInFlight)
		{
			PrepareMissileAttack();
		}
		SetExposedHeadDamageableState();
		SetCrouchPresentation("Green");
		sprite.SetAnimation("Missile", loop: false, 0.2);
		CatchUpRemoteMissileVisuals(missileTargetIndex);
	}

	public void MissileProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void MissileExited()
	{
		if (TowerDefenseManager.HasGameplayAuthority && !IsProgressRestoreInFlight)
		{
			EndMissileAttack(markDirty: true, missileTargetIndex >= missileTargets.Count);
		}
	}

	public void DoomStartEntered()
	{
		if (TowerDefenseManager.HasGameplayAuthority && !IsProgressRestoreInFlight)
		{
			RemoveDoomShield();
			_doomCharging = true;
			_doomAttackResolved = false;
			doomChargeRemaining = abilityConfig?.doomChargeSeconds ?? 20.0;
			_doomActionSequence++;
			EnsureDoomShield(reset: true);
			MarkBossStateDirty();
		}
		else if (_doomCharging)
		{
			EnsureDoomShield(reset: false);
		}
		SetDoomDamageableState();
		SetCrouchPresentation("Purple");
		sprite.SetAnimation("DoomStart", loop: false, 0.2);
	}

	public void DoomStartProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		AdvanceDoomCharge(delta);
	}

	public void DoomStartExited()
	{
	}

	public void DoomIdleEntered()
	{
		SetDoomDamageableState();
		SetCrouchPresentation("Purple");
		sprite.SetAnimation("DoomIdle", loop: true, 0.2);
	}

	public void DoomIdleProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		AdvanceDoomCharge(delta);
	}

	public void DoomIdleExited()
	{
	}

	public void DoomEndEntered()
	{
		RemoveDoomShield();
		SetExposedHeadDamageableState();
		SetCrouchPresentation("Purple");
		sprite.SetAnimation("DoomEnd", loop: false, 0.2);
	}

	public void DoomEndProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void DoomEndExited()
	{
	}

	public void DoomCancelEntered()
	{
		RemoveDoomShield();
		SetExposedHeadDamageableState();
		SetCrouchPresentation("Purple");
		if (sprite is ZombieBossDave zombieBossDave)
		{
			zombieBossDave.PlayDriverDamage();
		}
		sprite.SetAnimation("DoomCancel", loop: false, 0.2);
	}

	public void DoomCancelProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void DoomCancelExited()
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
		ZombieBossDave zombieBossDave = sprite as ZombieBossDave;
		if (sprite.clip == "HeadIdle" || sprite.clip == "HeadAttack4" || sprite.clip == "Missile" || sprite.clip == "DoomIdle" || sprite.clip == "DoomStart" || sprite.clip == "DoomEnd" || sprite.clip == "DoomCancel")
		{
			flag = true;
		}
		ReleaseHeadAttackPause(markDirty: false);
		_doomCharging = false;
		RemoveDoomShield();
		_plantAttackActive = false;
		EndMissileAttack(markDirty: false);
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			MarkBossStateDirty();
		}
		base.DieEntered();
		if (GodotObject.IsInstanceValid(zombieBossDave))
		{
			zombieBossDave.SetHeadAttack((int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().Y / 2f) + 2, 0.5);
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
		ZombieBossDave zombieBossDave = sprite as ZombieBossDave;
		if (GodotObject.IsInstanceValid(zombieBossDave))
		{
			zombieBossDave.DamagePointSet(damagePointName);
		}
		switch (damagePointName)
		{
		case "Stage1":
			stage = 1;
			restTime = 4.5;
			spawnRestTime = 3.5;
			QueueCrouchShowcase("Missile");
			break;
		case "Stage2":
			stage = 2;
			restTime = 4.0;
			spawnRestTime = 3.0;
			QueueCrouchShowcase("Doom");
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
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			MarkBossStateDirty();
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
		if (sprite is ZombieBossDave zombieBossDave)
		{
			zombieBossDave.AnimeCompleted(clip);
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
					goto IL_02c5;
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
				goto IL_02c5;
			case '3':
				if (!(clip == "Stomp3"))
				{
					break;
				}
				goto IL_02c5;
			case '4':
				{
					if (!(clip == "Stomp4"))
					{
						break;
					}
					goto IL_02c5;
				}
				IL_02c5:
				FinishStandingSpecial();
				break;
			}
			break;
		case 9:
			switch (clip[0])
			{
			case 'H':
				if (clip == "HeadEnter")
				{
					SetExposedHeadDamageableState();
				}
				break;
			case 'D':
				if (clip == "DoomStart" && TowerDefenseManager.HasGameplayAuthority && _doomCharging)
				{
					SendStateEvent("ToDoomIdle");
				}
				break;
			}
			break;
		case 10:
			switch (clip[5])
			{
			default:
				return;
			case 'x':
				if (!(clip == "HeadExited"))
				{
					return;
				}
				FreshZIndex();
				if (TowerDefenseManager.HasGameplayAuthority)
				{
					activeCrouchSkill = "";
					SetStateList();
					isRest = true;
					restTimer = 2.0;
					MarkBossStateDirty();
					if (!die)
					{
						Idle();
					}
				}
				return;
			case 'E':
				if (clip == "PlantEnter")
				{
					sprite.SetAnimation("PlantLeave", loop: false);
				}
				return;
			case 'L':
				if (clip == "PlantLeave")
				{
					if (TowerDefenseManager.HasGameplayAuthority)
					{
						_plantAttackActive = false;
						MarkBossStateDirty();
					}
					FinishStandingSpecial();
				}
				return;
			case 'a':
				break;
			}
			if (!(clip == "DoomCancel"))
			{
				break;
			}
			goto IL_0367;
		case 7:
			switch (clip[0])
			{
			default:
				return;
			case 'M':
				if (clip == "Missile" && TowerDefenseManager.HasGameplayAuthority)
				{
					EndMissileAttack(markDirty: true, preserveInFlightCrosshairs: true);
					SendStateEvent("ToHeadExited");
				}
				return;
			case 'D':
				break;
			}
			if (!(clip == "DoomEnd"))
			{
				break;
			}
			goto IL_0367;
		case 11:
			if (clip == "HeadAttack4" && TowerDefenseManager.HasGameplayAuthority)
			{
				SendStateEvent("ToHeadIdle");
			}
			break;
		case 12:
			if (clip == "BungeeExited")
			{
				FreshZIndex();
				FinishStandingSpecial();
			}
			break;
		case 2:
			if (clip == "RV")
			{
				if (TowerDefenseManager.HasGameplayAuthority && _rvAttackArmed)
				{
					_rvAttackArmed = false;
					MarkBossStateDirty();
				}
				FinishStandingSpecial();
			}
			break;
		case 3:
		case 4:
		case 5:
		case 8:
			break;
			IL_0367:
			if (TowerDefenseManager.HasGameplayAuthority)
			{
				SendStateEvent("ToHeadExited");
			}
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
				if (!(command == "stomp"))
				{
					break;
				}
				AudioManager.Instance.AudioPlay("GargantuarThump");
				if (TowerDefenseManager.HasGameplayAuthority)
				{
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
		case 10:
			switch (command[0])
			{
			default:
				return;
			case 's':
				break;
			case 'h':
				if (command == "headAttack")
				{
					BallSpawn();
					if (TowerDefenseManager.HasGameplayAuthority)
					{
						BeginHeadAttackPause();
					}
				}
				return;
			}
			if (!(command == "spawnPlant"))
			{
				break;
			}
			goto IL_0198;
		case 7:
			switch (command[0])
			{
			case 't':
				if (command == "throwRV")
				{
					ZombieBossDave zombieBossDave = sprite as ZombieBossDave;
					if (GodotObject.IsInstanceValid(zombieBossDave))
					{
						zombieBossDave.SetRVVisible(visible: true);
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
			case 'm':
				if (command == "missile")
				{
					SpawnNextMissile();
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
					if (TowerDefenseManager.HasGameplayAuthority && _rvAttackArmed)
					{
						_attackComponent5.SmashAttackAll(100000.0);
						_rvAttackArmed = false;
						MarkBossStateDirty();
					}
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
		case 9:
			if (!(command == "plantDrop"))
			{
				break;
			}
			goto IL_0198;
		case 14:
			if (command == "headAttackOver")
			{
				ZombieBossDave zombieBossDave2 = sprite as ZombieBossDave;
				if (GodotObject.IsInstanceValid(zombieBossDave2))
				{
					zombieBossDave2.SetHeadAttack((int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().Y / 2f) + 2, 0.5);
				}
			}
			break;
		case 4:
			if (command == "doom")
			{
				ReleaseDoomAttack();
			}
			break;
		case 6:
		case 12:
		case 13:
			break;
			IL_0198:
			SpawnHypnotizedPlants();
			break;
		}
	}

	private void UpdateHeadAttackPause(double delta)
	{
		if (!IsBossGameplayPaused() && !(_headAttackPauseRemaining <= 0.0) && (TowerDefenseManager.HasGameplayAuthority || !(sprite?.clip != "HeadAttack4")))
		{
			_headAttackPauseRemaining = Mathf.Max(0.0, _headAttackPauseRemaining - delta);
			if (!(_headAttackPauseRemaining > 0.0))
			{
				ReleaseHeadAttackPause(TowerDefenseManager.HasGameplayAuthority);
			}
		}
	}

	private void BeginHeadAttackPause()
	{
		_headAttackPauseSequence++;
		if (_headAttackPauseSequence <= 0)
		{
			_headAttackPauseSequence = 1;
		}
		_headAttackPauseRemaining = 1.0;
		_headAttackPauseOwned = true;
		spritePause = true;
		MarkBossStateDirty();
	}

	private void ReleaseHeadAttackPause(bool markDirty)
	{
		bool num = _headAttackPauseRemaining > 0.0 || _headAttackPauseOwned;
		_headAttackPauseRemaining = 0.0;
		if (_headAttackPauseOwned)
		{
			spritePause = false;
		}
		_headAttackPauseOwned = false;
		if (num)
		{
			SetTransientRenderZIndex((int)(headAttackLine * 15 + itemLayer), "zomboss-head-attack-delay");
		}
		if ((num & markDirty) && TowerDefenseManager.HasGameplayAuthority)
		{
			MarkBossStateDirty();
		}
	}

	private void ApplyRemoteHeadAttackPause(int incomingSequence, double incomingRemaining)
	{
		if (incomingSequence < _remoteHeadAttackPauseSequence)
		{
			return;
		}
		if (incomingSequence > _remoteHeadAttackPauseSequence)
		{
			_remoteHeadAttackPauseSequence = incomingSequence;
			_headAttackPauseSequence = incomingSequence;
			if (incomingRemaining > 0.0)
			{
				_headAttackPauseRemaining = incomingRemaining;
				_headAttackPauseOwned = true;
				spritePause = true;
			}
			else
			{
				ReleaseHeadAttackPause(markDirty: false);
			}
		}
		else if (incomingRemaining <= 0.0)
		{
			ReleaseHeadAttackPause(markDirty: false);
		}
		else if (_headAttackPauseOwned)
		{
			_headAttackPauseRemaining = Mathf.Min(_headAttackPauseRemaining, incomingRemaining);
		}
	}

	public void StateRunning(double delta)
	{
		if (IsBossGameplayPaused() || !TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		string text = stateNow;
		if (text == null)
		{
			return;
		}
		switch (text.Length)
		{
		case 5:
			switch (text[1])
			{
			case 'p':
				if (text == "Spawn")
				{
					RunSpawnSequence(delta, spawnZombieNumOnce, completesSpawnGroup: true);
				}
				break;
			case 't':
				if (text == "Stomp")
				{
					SendStateEvent("ToStomp");
					stateNow = "";
					MarkBossStateDirty();
				}
				break;
			}
			break;
		case 18:
			if (text == "SpawnBeforeSpecial")
			{
				RunSpawnSequence(delta, 2, completesSpawnGroup: false);
			}
			break;
		case 17:
			if (text == "SpawnAfterSpecial")
			{
				RunSpawnSequence(delta, 3, completesSpawnGroup: true);
			}
			break;
		case 8:
			if (text == "HeadIdle")
			{
				headAttackOver = false;
				headIdleNum++;
				SendStateEvent("ToHeadIdle");
				stateNow = "";
				MarkBossStateDirty();
			}
			break;
		case 11:
			if (text == "BungeeSpawn")
			{
				SendStateEvent("ToBungee");
				stateNow = "";
				MarkBossStateDirty();
			}
			break;
		case 10:
			if (text == "PlantSpawn")
			{
				SendStateEvent("ToPlant");
				stateNow = "";
				MarkBossStateDirty();
			}
			break;
		case 2:
			if (text == "RV")
			{
				SendStateEvent("ToRV");
				stateNow = "";
				MarkBossStateDirty();
			}
			break;
		case 15:
		{
			if (!(text == "StandingSpecial"))
			{
				break;
			}
			Array<string> validStandingSpecials = GetValidStandingSpecials();
			if (validStandingSpecials.Count == 0)
			{
				stateNow = "";
				isRest = true;
				restTimer = 0.0;
				MarkBossStateDirty();
				break;
			}
			string text2 = validStandingSpecials.PickRandom();
			stateNow = "";
			MarkBossStateDirty();
			switch (text2)
			{
			case "PlantSpawn":
				SendStateEvent("ToPlant");
				break;
			case "BungeeSpawn":
				SendStateEvent("ToBungee");
				break;
			case "Stomp":
				SendStateEvent("ToStomp");
				break;
			case "RV":
				SendStateEvent("ToRV");
				break;
			}
			break;
		}
		}
	}

	private void RunSpawnSequence(double delta, int targetCount, bool completesSpawnGroup)
	{
		if (spawnRestTimer < spawnRestTime)
		{
			spawnRestTimer += delta;
			return;
		}
		SendStateEvent("ToSpawn");
		spawnZombieNumNow++;
		spawnRestTimer = 0.0;
		if (spawnZombieNumNow >= targetCount)
		{
			if (completesSpawnGroup)
			{
				spawnNum++;
			}
			spawnZombieNumNow = 0;
			isRest = true;
			stateNow = "";
			MarkBossStateDirty();
		}
	}

	public void ZombieSpawn()
	{
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(spawnZomie);
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(packetConfig) || !GodotObject.IsInstanceValid(node2D))
		{
			return;
		}
		Vector2 vector = new Vector2((sprite as ZombieBossDave)?.GetSpawnMarkerGlobalPos(this).X ?? GetLogicalGlobalPosition().X, TowerDefenseManager.GetMapCellPlantPos(new Vector2I(0, spawnLine)).Y);
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
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		Array<Vector2I> bungeeTargetPositions = GetBungeeTargetPositions();
		bungeeList.Clear();
		int num = Mathf.Min(abilityConfig?.bungeeCount ?? 3, bungeeTargetPositions.Count);
		double num2 = abilityConfig?.steelBungeeChance ?? 0.1;
		double num3 = abilityConfig?.hypnotistChance ?? 0.25;
		for (int i = 0; i < num; i++)
		{
			Vector2I item = bungeeTargetPositions.PickRandom();
			bungeeTargetPositions.Remove(item);
			double num4 = GD.Randf();
			string packetName;
			if (num4 < num2)
			{
				packetName = "ZombieBungiSP";
			}
			else
			{
				packetName = ((num4 < num2 + num3) ? "ZombieHypnotist" : "ZombieBungi");
			}
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
			if (!GodotObject.IsInstanceValid(packetConfig))
			{
				continue;
			}
			TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(item, playAudio: false);
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				Dictionary dictionary = new Dictionary();
				if (towerDefenseCharacter is TowerDefenseZombieBungi towerDefenseZombieBungi)
				{
					towerDefenseZombieBungi.skipBungeeTarget = true;
					dictionary["skipBungeeTarget"] = true;
				}
				else if (towerDefenseCharacter is TowerDefenseZombieBungiSP towerDefenseZombieBungiSP)
				{
					towerDefenseZombieBungiSP.suppressBungeeTarget = true;
					dictionary["suppressBungeeTarget"] = true;
				}
				bungeeList.Add(towerDefenseCharacter);
				TowerDefenseManager.PublishSpawnedCharacter(packetName, towerDefenseCharacter, useCreate: false, 0.0, walkAfterSpawn: false, "", (dictionary.Count > 0) ? dictionary : null);
			}
		}
		bungeeIsSpawn = true;
		MarkBossStateDirty();
	}

	public void RVSpawn()
	{
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		List<TowerDefenseCharacter> cleanCharactersList = TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList();
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>();
		int num = (int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().X / 2f) - 1;
		foreach (TowerDefenseCharacter item in cleanCharactersList)
		{
			if (item is TowerDefensePlant && item.camp != camp && item.gridPos.X <= num)
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
		if (array.Count == 0)
		{
			_rvAttackArmed = false;
			if (sprite is ZombieBossDave zombieBossDave)
			{
				zombieBossDave.SetRVVisible(visible: false);
			}
			return;
		}
		Vector2I vector2I = array.PickRandom();
		if (vector2I.Y >= TowerDefenseManager.Instance.GetMapGridNum().Y)
		{
			vector2I.Y--;
		}
		rvPos = vector2I;
		_rvAttackArmed = true;
		_attackComponent5.SetCheckAreaShapeWorldOrigin(0, TowerDefenseManager.Instance.GetMapCellPos(rvPos) + TowerDefenseManager.Instance.GetMapGridSize() * new Vector2(1.5f, 1f));
		ZombieBossDave zombieBossDave2 = sprite as ZombieBossDave;
		if (GodotObject.IsInstanceValid(zombieBossDave2))
		{
			zombieBossDave2.SetRVPos(rvPos);
		}
		MarkBossStateDirty();
	}

	public void BallSpawn()
	{
		AudioManager.Instance.AudioPlay("Bossboulderattack");
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		Vector2 vector = new Vector2((sprite as ZombieBossDave)?.GetBallSpawnMarkerGlobalPos(this).X ?? GetLogicalGlobalPosition().X, TowerDefenseManager.GetMapCellPlantPos(new Vector2I(0, headAttackLine)).Y);
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
		Godot.Collections.Array array = abilityConfig?.GetZombieStage(stage) ?? new Godot.Collections.Array();
		if (array.Count == 0)
		{
			return "ZombieNormal";
		}
		Godot.Collections.Array array2 = ((spawnNum >= array.Count) ? array[array.Count - 1].AsGodotArray() : array[spawnNum].AsGodotArray());
		if (array2.Count == 0)
		{
			return "ZombieNormal";
		}
		return TowerDefenseManager.Instance.PickRandomZomie(array2);
	}

	public void SetStateList()
	{
		if (TowerDefenseManager.HasGameplayAuthority && !IsProgressRestoreInFlight)
		{
			if (stage >= 2)
			{
				stateMethodList = new Godot.Collections.Array { "SpawnBeforeSpecial", "StandingSpecial", "SpawnAfterSpecial", "StandingSpecial" };
			}
			else
			{
				stateMethodList = ((headIdleNum == 0) ? new Godot.Collections.Array { "Spawn", "Spawn" } : new Godot.Collections.Array { "Spawn" });
				stateMethodList.Add("StandingSpecial");
			}
			MarkBossStateDirty();
		}
	}

	private Array<string> GetValidStandingSpecials()
	{
		Array<string> array = new Array<string>();
		if (CanSpawnHypnotizedPlant())
		{
			array.Add("PlantSpawn");
		}
		if (stage > 1)
		{
			if (GetBungeeTargetPositions().Count > 0)
			{
				array.Add("BungeeSpawn");
			}
			if (HasRvTarget())
			{
				array.Add("RV");
			}
			if (GetAvailableStompIds().Count > 0)
			{
				array.Add("Stomp");
			}
		}
		return array;
	}

	private Array<int> GetAvailableStompIds()
	{
		Array<int> array = new Array<int>();
		AttackComponent attackComponent = _attackComponent1;
		if (attackComponent != null && attackComponent.CanAttackOnce())
		{
			array.Add(1);
		}
		AttackComponent attackComponent2 = _attackComponent2;
		if (attackComponent2 != null && attackComponent2.CanAttackOnce())
		{
			array.Add(2);
		}
		AttackComponent attackComponent3 = _attackComponent3;
		if (attackComponent3 != null && attackComponent3.CanAttackOnce())
		{
			array.Add(3);
		}
		AttackComponent attackComponent4 = _attackComponent4;
		if (attackComponent4 != null && attackComponent4.CanAttackOnce())
		{
			array.Add(4);
		}
		return array;
	}

	private Array<Vector2I> GetBungeeTargetPositions()
	{
		Array<Vector2I> array = new Array<Vector2I>();
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return array;
		}
		int num = (int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().X / 2f) + 1;
		foreach (TowerDefenseCharacter cleanCharacters in TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList())
		{
			if (cleanCharacters is TowerDefensePlant towerDefensePlant && towerDefensePlant.camp != camp && !(towerDefensePlant is TowerDefensePlantBowlingBase) && towerDefensePlant.instance.canBeCollection && towerDefensePlant.gridPos.X <= num && !array.Contains(towerDefensePlant.gridPos))
			{
				array.Add(towerDefensePlant.gridPos);
			}
		}
		return array;
	}

	private bool HasRvTarget()
	{
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return false;
		}
		int num = (int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().X / 2f) - 1;
		foreach (TowerDefenseCharacter cleanCharacters in TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList())
		{
			if (cleanCharacters is TowerDefensePlant towerDefensePlant && towerDefensePlant.camp != camp && towerDefensePlant.gridPos.X <= num)
			{
				return true;
			}
		}
		return false;
	}

	private List<(string PacketName, Vector2I Position)> GetHypnotizedPlantPlacements()
	{
		List<(string, Vector2I)> list = new List<(string, Vector2I)>();
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return list;
		}
		Godot.Collections.Array obj = abilityConfig?.GetPlantPool(stage) ?? new Godot.Collections.Array();
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		int num = Mathf.Clamp(abilityConfig?.plantColumnMin ?? 6, 1, mapGridNum.X);
		int num2 = Mathf.Clamp(abilityConfig?.plantColumnMax ?? 9, num, mapGridNum.X);
		foreach (Variant item in obj)
		{
			string text = item.AsString();
			TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(text);
			if (!GodotObject.IsInstanceValid(packetConfig))
			{
				continue;
			}
			for (int i = num; i <= num2; i++)
			{
				for (int j = 1; j <= mapGridNum.Y; j++)
				{
					Vector2I vector2I = new Vector2I(i, j);
					if (CanForcePlantOnEmptyCell(packetConfig, vector2I))
					{
						list.Add((text, vector2I));
					}
				}
			}
		}
		return list;
	}

	private static bool CanForcePlantOnEmptyCell(TowerDefensePacketConfig packetConfig, Vector2I position)
	{
		if (!GodotObject.IsInstanceValid(packetConfig) || !(packetConfig.characterConfig is TowerDefensePlantConfig towerDefensePlantConfig) || !IsPlantFootprintCellEmpty(position))
		{
			return false;
		}
		foreach (Vector2I item in towerDefensePlantConfig.extendGrid)
		{
			if (!IsPlantFootprintCellEmpty(position + item))
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsPlantFootprintCellEmpty(Vector2I position)
	{
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(position);
		if (!GodotObject.IsInstanceValid(mapCell))
		{
			return false;
		}
		mapCell.ClearEmpty();
		return mapCell.characterList.Count == 0;
	}

	private bool CanSpawnHypnotizedPlant()
	{
		return GetHypnotizedPlantPlacements().Count > 0;
	}

	private void SpawnHypnotizedPlants()
	{
		if (!TowerDefenseManager.HasGameplayAuthority || _plantSpawnResolved)
		{
			return;
		}
		_plantSpawnResolved = true;
		List<(string, Vector2I)> hypnotizedPlantPlacements = GetHypnotizedPlantPlacements();
		int num = Mathf.Min(abilityConfig?.plantDropCount ?? 3, hypnotizedPlantPlacements.Count);
		int num2 = 0;
		while (num2 < num && hypnotizedPlantPlacements.Count > 0)
		{
			int index = GD.RandRange(0, hypnotizedPlantPlacements.Count - 1);
			(string PacketName, Vector2I Position) candidate = hypnotizedPlantPlacements[index];
			hypnotizedPlantPlacements.RemoveAll(((string PacketName, Vector2I Position) value) => value.Position == candidate.Position);
			if (CanForcePlantOnEmptyCell(TowerDefenseManager.GetPacketConfig(candidate.PacketName), candidate.Position) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance.BungiSpawn(candidate.PacketName, candidate.Position, null, hypnoses: true, -1, skipPlacementCheck: true)))
			{
				num2++;
			}
		}
		MarkBossStateDirty();
	}

	private void StartNextCrouchSkill()
	{
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		string text;
		if (pendingCrouchShowcases.Count > 0)
		{
			text = pendingCrouchShowcases[0];
			pendingCrouchShowcases.RemoveAt(0);
		}
		else
		{
			Array<string> array = new Array<string> { "Fire", "Ice" };
			if (stage >= 1)
			{
				array.Add("Missile");
			}
			if (stage >= 2)
			{
				array.Add("Doom");
			}
			text = array.PickRandom();
		}
		activeCrouchSkill = text;
		headAttackOver = true;
		int y = TowerDefenseManager.Instance.GetMapGridNum().Y;
		bool flag = text == "Doom";
		headAttackLine = (flag ? Mathf.Clamp((int)Mathf.Floor((float)y / 2f) + 2, 1, y) : GD.RandRange(1, y));
		headAttackIsFire = text == "Fire";
		MarkBossStateDirty();
		if (!(text == "Missile"))
		{
			if (text == "Doom")
			{
				SendStateEvent("ToDoomStart");
			}
			else
			{
				SendStateEvent("ToHeadAttack");
			}
		}
		else
		{
			SendStateEvent("ToMissile");
		}
	}

	private void QueueCrouchShowcase(string skill)
	{
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		if (skill == "Missile")
		{
			if (_missileShowcaseQueued)
			{
				return;
			}
			_missileShowcaseQueued = true;
			int num = pendingCrouchShowcases.IndexOf("Doom");
			if (num >= 0)
			{
				pendingCrouchShowcases.Insert(num, skill);
			}
			else
			{
				pendingCrouchShowcases.Add(skill);
			}
		}
		else if (skill == "Doom")
		{
			if (_doomShowcaseQueued)
			{
				return;
			}
			_doomShowcaseQueued = true;
			pendingCrouchShowcases.Add(skill);
		}
		MarkBossStateDirty();
	}

	private void SetCrouchPresentation(string ballKind)
	{
		int y = TowerDefenseManager.Instance.GetMapGridNum().Y;
		int num = ((ballKind == "Purple") ? Mathf.Clamp((int)Mathf.Floor((float)TowerDefenseManager.Instance.GetMapGridNum().Y / 2f) + 2, 1, y) : Mathf.Clamp(headAttackLine, 1, y));
		SetTransientRenderZIndex((int)(num * 15 + itemLayer), "zomboss-crouch-skill");
		if (sprite is ZombieBossDave zombieBossDave)
		{
			zombieBossDave.SetHeadAttack(num);
			zombieBossDave.SetHeadAttackBall(ballKind);
		}
	}

	private void PrepareMissileAttack()
	{
		if (_missileAttackActive && missileTargets.Count > 0)
		{
			RefreshMissileCrosshairs();
			return;
		}
		missileTargets.Clear();
		missileTargetIndex = 0;
		int num = abilityConfig?.missileCount ?? 4;
		List<Vector2I> list = new List<Vector2I>();
		foreach (TowerDefenseCharacter cleanCharacters in TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList())
		{
			if (cleanCharacters is TowerDefensePlant towerDefensePlant && towerDefensePlant.camp != camp && !list.Contains(towerDefensePlant.gridPos))
			{
				list.Add(towerDefensePlant.gridPos);
			}
		}
		while (missileTargets.Count < num && list.Count > 0)
		{
			int index = GD.RandRange(0, list.Count - 1);
			missileTargets.Add(list[index]);
			list.RemoveAt(index);
		}
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		List<Vector2I> list2 = new List<Vector2I>();
		for (int i = 1; i <= mapGridNum.X; i++)
		{
			for (int j = 1; j <= mapGridNum.Y; j++)
			{
				Vector2I item = new Vector2I(i, j);
				if (!missileTargets.Contains(item))
				{
					list2.Add(item);
				}
			}
		}
		while (missileTargets.Count < num && list2.Count > 0)
		{
			int index2 = GD.RandRange(0, list2.Count - 1);
			missileTargets.Add(list2[index2]);
			list2.RemoveAt(index2);
		}
		_missileAttackActive = true;
		_missileCrosshairTrackingActive = true;
		missileResolvedMask = 0;
		_missileActionSequence++;
		ConnectMissileLifecycleSignals();
		RefreshMissileCrosshairs();
		MarkBossStateDirty();
	}

	private void SpawnNextMissile()
	{
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			CatchUpRemoteMissileVisuals(missileTargetIndex);
		}
		else if (_missileAttackActive && missileTargetIndex >= 0 && missileTargetIndex < missileTargets.Count)
		{
			int num = missileTargetIndex;
			bool flag = SpawnMissileProjectile(num, missileTargets[num], suppressGameplay: false);
			missileTargetIndex++;
			if (!flag)
			{
				ResolveMissileSlot(num, markDirty: false);
			}
			RefreshMissileCrosshairs();
			MarkBossStateDirty();
		}
	}

	private void CatchUpRemoteMissileVisuals(int firedCount)
	{
		if (TowerDefenseManager.HasGameplayAuthority || !IsNodeReady() || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance) || !GodotObject.IsInstanceValid(sprite) || (!_missileAttackActive && activeCrouchSkill != "Missile" && sprite.clip != "Missile"))
		{
			return;
		}
		int num = Mathf.Clamp(firedCount, 0, missileTargets.Count);
		while (_remoteMissileVisualIndex < num)
		{
			int remoteMissileVisualIndex = _remoteMissileVisualIndex;
			_remoteMissileVisualIndex++;
			if (!IsMissileSlotResolved(remoteMissileVisualIndex))
			{
				SpawnMissileProjectile(remoteMissileVisualIndex, missileTargets[remoteMissileVisualIndex], suppressGameplay: true);
			}
		}
	}

	private bool SpawnMissileProjectile(int slot, Vector2I targetGrid, bool suppressGameplay)
	{
		TowerDefenseProjectileConfig towerDefenseProjectileConfig = abilityConfig?.missileProjectileConfig;
		if (!GodotObject.IsInstanceValid(towerDefenseProjectileConfig))
		{
			return false;
		}
		Vector2 vector = ((sprite is ZombieBossDave zombieBossDave) ? zombieBossDave.GetBallSpawnMarkerGlobalPos(this) : GetLogicalGlobalPosition());
		BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
		{
			gridYOverride = targetGrid.Y,
			baseDamageOverride = (abilityConfig?.missileDamage ?? 1800.0),
			catapultStartPositionOverride = vector,
			catapultTargetPositionOverride = TowerDefenseManager.GetMapCellPlantPos(targetGrid),
			catapultSkyDrop = true,
			catapultSkyDropAscentHorizontalOffset = 240f,
			catapultSkyDropOffscreenWaitSeconds = 1.5,
			catapultSkyDropVisualRadius = 60f,
			spriteRotationOverride = -(float)Math.PI / 2f,
			lifecycleSubscribed = true,
			lifecycleOwnerSequence = _missileActionSequence,
			lifecycleSlot = slot,
			suppressGameplay = suppressGameplay
		};
		int bulletIndex;
		return FireComponent.TryCreateProjectilePositionByConfig(this, null, 0.0, vector, Vector2.Zero, towerDefenseProjectileConfig, out bulletIndex, towerDefenseProjectileConfig.collisionFlags, camp, Vector2.Zero, overrides);
	}

	private void OnMissileLifecycleEnded(TowerDefenseCharacter source, int ownerSequence, int slot, BulletField.BulletLifecycleTerminalReason reason)
	{
		if (source == this && ownerSequence == _missileActionSequence)
		{
			ResolveMissileSlot(slot, TowerDefenseManager.HasGameplayAuthority);
		}
	}

	private bool IsMissileSlotResolved(int slot)
	{
		if (slot >= 0 && slot < 31)
		{
			return (missileResolvedMask & (1 << slot)) != 0;
		}
		return false;
	}

	private int GetAllMissileSlotsMask()
	{
		if (missileTargets.Count <= 0)
		{
			return 0;
		}
		if (missileTargets.Count < 31)
		{
			return (1 << missileTargets.Count) - 1;
		}
		return -1;
	}

	private void ResolveMissileSlot(int slot, bool markDirty)
	{
		if (slot >= 0 && slot < missileTargets.Count && slot < 31 && !IsMissileSlotResolved(slot))
		{
			missileResolvedMask |= 1 << slot;
			if (_missileCrosshairs.Remove(slot, out var value) && GodotObject.IsInstanceValid(value))
			{
				value.QueueFree();
			}
			if ((missileResolvedMask & GetAllMissileSlotsMask()) == GetAllMissileSlotsMask())
			{
				_missileCrosshairTrackingActive = false;
				ClearMissileCrosshairs();
			}
			if (markDirty)
			{
				MarkBossStateDirty();
			}
		}
	}

	private void RefreshMissileCrosshairs()
	{
		ClearMissileCrosshairs();
		if ((!_missileAttackActive && !_missileCrosshairTrackingActive) || !GodotObject.IsInstanceValid(CROSSHAIR_TEX) || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return;
		}
		for (int i = 0; i < missileTargets.Count; i++)
		{
			if (!IsMissileSlotResolved(i))
			{
				Sprite2D sprite2D = new Sprite2D
				{
					Texture = CROSSHAIR_TEX,
					ZAsRelative = false,
					ZIndex = 2000,
					GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(missileTargets[i])
				};
				TowerDefenseManager.GetCharacterNode().AddChild(sprite2D, forceReadableName: false, InternalMode.Disabled);
				_missileCrosshairs[i] = sprite2D;
			}
		}
	}

	private void ClearMissileCrosshairs()
	{
		foreach (Sprite2D value in _missileCrosshairs.Values)
		{
			if (GodotObject.IsInstanceValid(value))
			{
				value.QueueFree();
			}
		}
		_missileCrosshairs.Clear();
	}

	private void EndMissileAttack(bool markDirty = true, bool preserveInFlightCrosshairs = false)
	{
		bool num = _missileAttackActive || _missileCrosshairTrackingActive;
		_missileAttackActive = false;
		bool flag = missileTargetIndex >= missileTargets.Count;
		if (!preserveInFlightCrosshairs || !flag)
		{
			missileResolvedMask = GetAllMissileSlotsMask();
			_missileCrosshairTrackingActive = false;
			ClearMissileCrosshairs();
		}
		if ((num & markDirty) && TowerDefenseManager.HasGameplayAuthority)
		{
			MarkBossStateDirty();
		}
	}

	private void SetExposedHeadDamageableState()
	{
		instance.invincible = false;
		instance.canBeCollection = true;
		targetRegistrationComponent.canProjectileCheck = true;
		targetRegistrationComponent.allLineCheck = true;
		instance.unUseBuffFlags = -4;
	}

	private void SetRetractedHeadState()
	{
		instance.invincible = true;
		instance.canBeCollection = false;
		targetRegistrationComponent.canProjectileCheck = false;
		targetRegistrationComponent.allLineCheck = false;
		instance.unUseBuffFlags = -1;
		if (buff.BuffHas("Frozen"))
		{
			buff.DeleteBuff("Frozen");
		}
		if (buff.BuffHas("IceSpeedDown"))
		{
			buff.DeleteBuff("IceSpeedDown");
		}
	}

	private void SetDoomDamageableState()
	{
		instance.invincible = false;
		instance.canBeCollection = true;
		targetRegistrationComponent.canProjectileCheck = true;
		targetRegistrationComponent.allLineCheck = true;
		instance.unUseBuffFlags = -1;
		if (buff.BuffHas("Frozen"))
		{
			buff.DeleteBuff("Frozen");
		}
		if (buff.BuffHas("IceSpeedDown"))
		{
			buff.DeleteBuff("IceSpeedDown");
		}
	}

	private void ApplyBossPresentationForClip(string clipName, bool playOneShot)
	{
		if (clipName == null)
		{
			return;
		}
		switch (clipName.Length)
		{
		case 8:
		{
			char c = clipName[0];
			if (c != 'D')
			{
				if (c != 'H' || !(clipName == "HeadIdle"))
				{
					break;
				}
				goto IL_0149;
			}
			if (!(clipName == "DoomIdle"))
			{
				break;
			}
			goto IL_019a;
		}
		case 7:
			switch (clipName[0])
			{
			default:
				return;
			case 'M':
				if (clipName == "Missile")
				{
					RemoveDoomShield();
					SetExposedHeadDamageableState();
					SetCrouchPresentation("Green");
				}
				return;
			case 'D':
				break;
			}
			if (!(clipName == "DoomEnd"))
			{
				break;
			}
			goto IL_01bc;
		case 10:
		{
			char c = clipName[0];
			if (c != 'D')
			{
				if (c != 'H' || !(clipName == "HeadExited"))
				{
					break;
				}
				goto IL_01f9;
			}
			if (!(clipName == "DoomCancel"))
			{
				break;
			}
			goto IL_01bc;
		}
		case 5:
			switch (clipName[0])
			{
			default:
				return;
			case 'E':
				if (!(clipName == "Enter"))
				{
					return;
				}
				break;
			case 'D':
				if (!(clipName == "Death"))
				{
					return;
				}
				break;
			}
			goto IL_01f9;
		case 11:
			if (!(clipName == "HeadAttack4"))
			{
				break;
			}
			goto IL_0149;
		case 9:
			if (!(clipName == "DoomStart"))
			{
				break;
			}
			goto IL_019a;
		case 4:
			if (!(clipName == "Idle"))
			{
				break;
			}
			goto IL_01f9;
		case 6:
			break;
			IL_01bc:
			RemoveDoomShield();
			SetExposedHeadDamageableState();
			SetCrouchPresentation("Purple");
			if (playOneShot && clipName == "DoomCancel" && sprite is ZombieBossDave zombieBossDave)
			{
				zombieBossDave.PlayDriverDamage();
			}
			break;
			IL_0149:
			RemoveDoomShield();
			SetExposedHeadDamageableState();
			if (sprite is ZombieBossDave zombieBossDave2)
			{
				zombieBossDave2.SetHeadAttackBall(headAttackIsFire ? "Fire" : "Ice");
			}
			break;
			IL_01f9:
			RemoveDoomShield();
			SetRetractedHeadState();
			break;
			IL_019a:
			if (_doomCharging)
			{
				EnsureDoomShield(reset: false);
			}
			SetDoomDamageableState();
			SetCrouchPresentation("Purple");
			break;
		}
	}

	private void AdvanceDoomCharge(double delta)
	{
		if (!IsBossGameplayPaused() && TowerDefenseManager.HasGameplayAuthority && _doomCharging && !sprite.pause)
		{
			doomChargeRemaining = Mathf.Max(0.0, doomChargeRemaining - delta);
			if (!(doomChargeRemaining > 0.0))
			{
				_doomCharging = false;
				RemoveDoomShield();
				MarkBossStateDirty();
				SendStateEvent("ToDoomEnd");
			}
		}
	}

	private TowerDefenseArmorInstance EnsureDoomShield(bool reset)
	{
		if (!GodotObject.IsInstanceValid(instance))
		{
			return null;
		}
		TowerDefenseArmorInstance towerDefenseArmorInstance = GetArmorFromName("BossDaveDoomShield");
		if (reset && GodotObject.IsInstanceValid(towerDefenseArmorInstance))
		{
			RemoveDoomShield();
			towerDefenseArmorInstance = null;
		}
		if (!GodotObject.IsInstanceValid(towerDefenseArmorInstance))
		{
			instance.ArmorAdd("BossDaveDoomShield");
			towerDefenseArmorInstance = GetArmorFromName("BossDaveDoomShield");
		}
		if (!GodotObject.IsInstanceValid(towerDefenseArmorInstance))
		{
			return null;
		}
		towerDefenseArmorInstance.hitpointScale = 1.0;
		ApplyPendingLegacyDoomShieldDamage(towerDefenseArmorInstance);
		return towerDefenseArmorInstance;
	}

	private void RemoveDoomShield()
	{
		if (!GodotObject.IsInstanceValid(instance))
		{
			return;
		}
		TowerDefenseArmorInstance armorFromName = GetArmorFromName("BossDaveDoomShield");
		if (!GodotObject.IsInstanceValid(armorFromName) || armorFromName.isRemove)
		{
			return;
		}
		_removingDoomShield = true;
		try
		{
			armorFromName.RemoveArmor(ArmorRemovalReason.Replaced);
			instance.ArmorDestroy(armorFromName);
		}
		finally
		{
			_removingDoomShield = false;
		}
	}

	private void OnDoomShieldBroken(string armorName)
	{
		if (!_removingDoomShield && !(armorName != "BossDaveDoomShield") && TowerDefenseManager.HasGameplayAuthority && _doomCharging)
		{
			_doomCharging = false;
			MarkBossStateDirty();
			SendStateEvent("ToDoomCancel");
		}
	}

	private void ApplyPendingLegacyDoomShieldDamage(TowerDefenseArmorInstance shield)
	{
		if (_hasPendingLegacyDoomShieldDamage && GodotObject.IsInstanceValid(shield))
		{
			double num = Mathf.Max(0.0, _pendingLegacyDoomShieldDamage);
			_hasPendingLegacyDoomShieldDamage = false;
			_pendingLegacyDoomShieldDamage = 0.0;
			double num2 = Mathf.Max(0.0, shield.hitpointsSave - num);
			if (num2 <= 0.0)
			{
				shield.Hurt(shield.hitPoints, playSplatAudio: false, Vector2.Zero, createDamagePart: false, ignoreLimit: true);
				return;
			}
			shield.hitPoints = num2;
			shield.RefreshDamageStageFromHitPoints();
		}
	}

	private void ReleaseDoomAttack()
	{
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			PlayDoomReleaseEffect();
		}
		else
		{
			if (_doomAttackResolved)
			{
				return;
			}
			_doomAttackResolved = true;
			_doomCharging = false;
			RemoveDoomShield();
			PlayDoomReleaseEffect();
			double num = abilityConfig?.doomDamage ?? 1800.0;
			foreach (TowerDefenseCharacter cleanCharacters in TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList())
			{
				if (cleanCharacters is TowerDefensePlant towerDefensePlant && towerDefensePlant.camp != camp && !towerDefensePlant.die)
				{
					towerDefensePlant.Hurt(num);
				}
			}
			MarkBossStateDirty();
		}
	}

	private void PlayDoomReleaseEffect()
	{
		AudioManager.Instance.AudioPlay("Doomshroom");
		Vector2 globalPosition = ((sprite is ZombieBossDave zombieBossDave) ? zombieBossDave.GetBallSpawnMarkerGlobalPos(this) : GetLogicalGlobalPosition());
		if (sprite is ZombieBossDave zombieBossDave2 && zombieBossDave2.TryResolveLayerIdForRender("DoomBall2", out var layerId) && zombieBossDave2.TryGetManagedLayerPositionForRender(layerId, new Vector2(70.5f, 70.5f), out var position))
		{
			globalPosition = zombieBossDave2.GlobalTransform * position;
		}
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(DOOMSHROOM_EXPLOSION, new Vector2I(100, 100));
		TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		towerDefenseEffectParticlesOnce.GlobalPosition = globalPosition;
	}

	private void FinishStandingSpecial()
	{
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			isRest = true;
			MarkBossStateDirty();
			if (!die)
			{
				Idle();
			}
		}
	}

	private void MarkBossStateDirty()
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

	private static bool IsBossGameplayPaused()
	{
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return TowerDefenseManager.Instance.pauseZombie;
		}
		return false;
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

	public void ImportNetworkSpawnState(Dictionary data)
	{
		ImportBossNetworkState(data);
	}

	public override Dictionary ExportNetworkSpawnState()
	{
		return ExportBossNetworkState();
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		return ExportBossNetworkState();
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		ImportBossNetworkState(data);
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return _networkSpecialStateRevision;
	}

	public override bool IsNetworkSpecialMovementActive()
	{
		if (!_plantAttackActive && !_missileAttackActive && !_missileCrosshairTrackingActive)
		{
			return _doomCharging;
		}
		return true;
	}

	public override void OnRemoteNetworkAnimationStart(string clipName)
	{
		if (clipName != "HeadAttack4")
		{
			ReleaseHeadAttackPause(markDirty: false);
		}
		ApplyBossPresentationForClip(clipName, playOneShot: true);
		switch (clipName)
		{
		case "Missile":
			CatchUpRemoteMissileVisuals(missileTargetIndex);
			RefreshMissileCrosshairs();
			break;
		case "HeadExited":
			CatchUpRemoteMissileVisuals(missileTargetIndex);
			RefreshMissileCrosshairs();
			break;
		case "Death":
			_missileCrosshairTrackingActive = false;
			ClearMissileCrosshairs();
			break;
		}
	}

	private Dictionary ExportBossNetworkState()
	{
		return new Dictionary
		{
			["schemaVersion"] = 4,
			["rev"] = _networkSpecialStateRevision,
			["stage"] = stage,
			["stateNow"] = stateNow,
			["stateMethodList"] = stateMethodList,
			["isRest"] = isRest,
			["restTimer"] = restTimer,
			["spawnZombieNumNow"] = spawnZombieNumNow,
			["spawnRestTimer"] = spawnRestTimer,
			["spawnLine"] = spawnLine,
			["spawnZomie"] = spawnZomie ?? "",
			["spawnNum"] = spawnNum,
			["headIdleNum"] = headIdleNum,
			["headAttackOver"] = headAttackOver,
			["headAttackLine"] = headAttackLine,
			["headAttackIsFire"] = headAttackIsFire,
			["headAttackRestTimer"] = headAttackRestTimer,
			["headAttackPauseRemaining"] = _headAttackPauseRemaining,
			["headAttackPauseSequence"] = _headAttackPauseSequence,
			["activeCrouchSkill"] = activeCrouchSkill ?? "",
			["pendingCrouchShowcases"] = pendingCrouchShowcases,
			["missileShowcaseQueued"] = _missileShowcaseQueued,
			["doomShowcaseQueued"] = _doomShowcaseQueued,
			["plantAttackActive"] = _plantAttackActive,
			["plantSpawnResolved"] = _plantSpawnResolved,
			["plantActionSequence"] = _plantActionSequence,
			["missileAttackActive"] = _missileAttackActive,
			["missileTargets"] = SerializeGridPositions(missileTargets),
			["missileTargetIndex"] = missileTargetIndex,
			["missileActionSequence"] = _missileActionSequence,
			["missileResolvedMask"] = missileResolvedMask,
			["missileCrosshairTrackingActive"] = _missileCrosshairTrackingActive,
			["doomCharging"] = _doomCharging,
			["doomChargeRemaining"] = doomChargeRemaining,
			["doomAttackResolved"] = _doomAttackResolved,
			["doomActionSequence"] = _doomActionSequence,
			["bungeeIsSpawn"] = bungeeIsSpawn,
			["rvAttackArmed"] = _rvAttackArmed,
			["stompId"] = stompId,
			["rvPosX"] = rvPos.X,
			["rvPosY"] = rvPos.Y
		};
	}

	private void ImportBossNetworkState(Dictionary data)
	{
		if (data == null)
		{
			return;
		}
		int num = data.GetValueOrDefault("schemaVersion", 0).AsInt32();
		int num2 = data.GetValueOrDefault("rev", 0).AsInt32();
		if (!TowerDefenseManager.HasGameplayAuthority && num2 < _networkSpecialStateRevision)
		{
			return;
		}
		_networkSpecialStateRevision = Mathf.Max(_networkSpecialStateRevision, num2);
		stage = data.GetValueOrDefault("stage", stage).AsInt32();
		stateNow = data.GetValueOrDefault("stateNow", stateNow).AsString();
		stateMethodList = data.GetValueOrDefault("stateMethodList", stateMethodList).AsGodotArray();
		isRest = data.GetValueOrDefault("isRest", isRest).AsBool();
		restTimer = data.GetValueOrDefault("restTimer", restTimer).AsDouble();
		spawnZombieNumNow = data.GetValueOrDefault("spawnZombieNumNow", spawnZombieNumNow).AsInt32();
		spawnRestTimer = data.GetValueOrDefault("spawnRestTimer", spawnRestTimer).AsDouble();
		spawnLine = data.GetValueOrDefault("spawnLine", spawnLine).AsInt32();
		spawnZomie = data.GetValueOrDefault("spawnZomie", spawnZomie ?? "").AsString();
		spawnNum = data.GetValueOrDefault("spawnNum", spawnNum).AsInt32();
		headIdleNum = data.GetValueOrDefault("headIdleNum", headIdleNum).AsInt32();
		headAttackOver = data.GetValueOrDefault("headAttackOver", headAttackOver).AsBool();
		headAttackLine = data.GetValueOrDefault("headAttackLine", headAttackLine).AsInt32();
		headAttackIsFire = data.GetValueOrDefault("headAttackIsFire", headAttackIsFire).AsBool();
		headAttackRestTimer = data.GetValueOrDefault("headAttackRestTimer", headAttackRestTimer).AsDouble();
		double num3 = data.GetValueOrDefault("headAttackPauseRemaining", _headAttackPauseRemaining).AsDouble();
		int num4 = data.GetValueOrDefault("headAttackPauseSequence", _headAttackPauseSequence).AsInt32();
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			_headAttackPauseSequence = num4;
			_headAttackPauseRemaining = num3;
			_headAttackPauseOwned = num3 > 0.0;
		}
		else
		{
			ApplyRemoteHeadAttackPause(num4, num3);
		}
		activeCrouchSkill = data.GetValueOrDefault("activeCrouchSkill", activeCrouchSkill).AsString();
		pendingCrouchShowcases = ReadStringArray(data.GetValueOrDefault("pendingCrouchShowcases", pendingCrouchShowcases));
		_missileShowcaseQueued = data.GetValueOrDefault("missileShowcaseQueued", _missileShowcaseQueued).AsBool();
		_doomShowcaseQueued = data.GetValueOrDefault("doomShowcaseQueued", _doomShowcaseQueued).AsBool();
		_plantAttackActive = data.GetValueOrDefault("plantAttackActive", _plantAttackActive).AsBool();
		_plantSpawnResolved = data.GetValueOrDefault("plantSpawnResolved", _plantSpawnResolved).AsBool();
		_plantActionSequence = data.GetValueOrDefault("plantActionSequence", _plantActionSequence).AsInt32();
		_missileAttackActive = data.GetValueOrDefault("missileAttackActive", _missileAttackActive).AsBool();
		missileTargets = DeserializeGridPositions(data.GetValueOrDefault("missileTargets", SerializeGridPositions(missileTargets)));
		int firedCount = data.GetValueOrDefault("missileTargetIndex", missileTargetIndex).AsInt32();
		int num5 = data.GetValueOrDefault("missileActionSequence", _missileActionSequence).AsInt32();
		missileResolvedMask = data.GetValueOrDefault("missileResolvedMask", missileResolvedMask).AsInt32();
		_missileCrosshairTrackingActive = data.GetValueOrDefault("missileCrosshairTrackingActive", _missileAttackActive).AsBool();
		_missileActionSequence = num5;
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			if (num5 != _remoteMissileActionSequence)
			{
				_remoteMissileActionSequence = num5;
				_remoteMissileVisualIndex = 0;
			}
			CatchUpRemoteMissileVisuals(firedCount);
		}
		missileTargetIndex = firedCount;
		_doomCharging = data.GetValueOrDefault("doomCharging", _doomCharging).AsBool();
		doomChargeRemaining = data.GetValueOrDefault("doomChargeRemaining", doomChargeRemaining).AsDouble();
		if (num < 4 && data.ContainsKey("doomAccumulatedDamage"))
		{
			_pendingLegacyDoomShieldDamage = data["doomAccumulatedDamage"].AsDouble();
			_hasPendingLegacyDoomShieldDamage = _doomCharging;
		}
		_doomAttackResolved = data.GetValueOrDefault("doomAttackResolved", _doomAttackResolved).AsBool();
		_doomActionSequence = data.GetValueOrDefault("doomActionSequence", _doomActionSequence).AsInt32();
		bungeeIsSpawn = data.GetValueOrDefault("bungeeIsSpawn", bungeeIsSpawn).AsBool();
		_rvAttackArmed = data.GetValueOrDefault("rvAttackArmed", _rvAttackArmed).AsBool();
		stompId = data.GetValueOrDefault("stompId", stompId).AsInt32();
		rvPos = new Vector2I(data.GetValueOrDefault("rvPosX", rvPos.X).AsInt32(), data.GetValueOrDefault("rvPosY", rvPos.Y).AsInt32());
		_hasPendingNetworkPresentation = true;
		if (IsNodeReady())
		{
			Callable.From(ApplyPendingBossNetworkPresentation).CallDeferred();
		}
	}

	private static Array<int> SerializeGridPositions(Array<Vector2I> positions)
	{
		Array<int> array = new Array<int>();
		foreach (Vector2I position in positions)
		{
			array.Add(position.X);
			array.Add(position.Y);
		}
		return array;
	}

	private static Array<Vector2I> DeserializeGridPositions(Variant value)
	{
		Array<Vector2I> array = new Array<Vector2I>();
		Godot.Collections.Array array2 = value.AsGodotArray();
		for (int i = 0; i + 1 < array2.Count; i += 2)
		{
			array.Add(new Vector2I(array2[i].AsInt32(), array2[i + 1].AsInt32()));
		}
		return array;
	}

	private static Array<string> ReadStringArray(Variant value)
	{
		Array<string> array = new Array<string>();
		foreach (Variant item in value.AsGodotArray())
		{
			array.Add(item.AsString());
		}
		return array;
	}

	private Array<string> GetSavedBungeeNodeNames()
	{
		Array<string> array = new Array<string>();
		foreach (Variant bungee in bungeeList)
		{
			TowerDefenseCharacter towerDefenseCharacter = bungee.AsGodotObject() as TowerDefenseCharacter;
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				array.Add(towerDefenseCharacter.Name.ToString());
			}
		}
		return array;
	}

	private void ResolveSavedBungeeRelations()
	{
		bungeeList.Clear();
		if (_pendingBungeeNodeNames.Count == 0 || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return;
		}
		foreach (TowerDefenseCharacter cleanCharacters in TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList())
		{
			if (GodotObject.IsInstanceValid(cleanCharacters) && _pendingBungeeNodeNames.Contains(cleanCharacters.Name.ToString()))
			{
				bungeeList.Add(cleanCharacters);
			}
		}
		_pendingBungeeNodeNames.Clear();
	}

	private void ApplyPendingBossNetworkPresentation()
	{
		if (!_hasPendingNetworkPresentation || !IsNodeReady() || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance) || !GodotObject.IsInstanceValid(sprite))
		{
			return;
		}
		_hasPendingNetworkPresentation = false;
		ApplyStagePresentation();
		if (sprite is ZombieBossDave zombieBossDave)
		{
			zombieBossDave.SetRVPos(rvPos);
		}
		ApplyBossPresentationForClip(sprite.clip, playOneShot: false);
		if (_clearLegacyMissilePauseOnFinalize)
		{
			if (_spawnLegacyDelayedMissileOnFinalize && TowerDefenseManager.HasGameplayAuthority && _missileAttackActive && missileTargetIndex < missileTargets.Count)
			{
				SpawnNextMissile();
			}
			if (_legacyMissileEventFrame >= 0 && sprite.clip == "Missile")
			{
				sprite.frameIndex = Mathf.Clamp(_legacyMissileEventFrame + 1, sprite.clipRange.X, Mathf.Max(sprite.clipRange.X, sprite.clipRange.Y - 1));
				sprite.elapsedTimer = 0.0;
			}
			spritePause = false;
			_clearLegacyMissilePauseOnFinalize = false;
			_spawnLegacyDelayedMissileOnFinalize = false;
			_legacyMissileEventFrame = -1;
		}
		if (_headAttackPauseRemaining > 0.0)
		{
			_headAttackPauseOwned = true;
			spritePause = true;
		}
		else
		{
			ReleaseHeadAttackPause(markDirty: false);
		}
		CatchUpRemoteMissileVisuals(missileTargetIndex);
		RefreshMissileCrosshairs();
	}

	private void ApplyStagePresentation()
	{
		if (stage >= 1 && sprite is ZombieBossDave zombieBossDave)
		{
			zombieBossDave.DamagePointSet("Stage1");
		}
		if (stage >= 2 && sprite is ZombieBossDave zombieBossDave2)
		{
			zombieBossDave2.DamagePointSet("Stage2");
		}
		if (stage >= 3 && sprite is ZombieBossDave zombieBossDave3)
		{
			zombieBossDave3.DamagePointSet("Stage3");
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["bossDaveSaveVersion"] = 4,
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
			["headAttackPauseRemaining"] = _headAttackPauseRemaining,
			["headAttackPauseSequence"] = _headAttackPauseSequence,
			["headIdleNum"] = headIdleNum,
			["bungeeIsSpawn"] = bungeeIsSpawn,
			["bungeeNodeNames"] = GetSavedBungeeNodeNames(),
			["rvAttackArmed"] = _rvAttackArmed,
			["stompId"] = stompId,
			["isRest"] = isRest,
			["restTime"] = restTime,
			["restTimer"] = restTimer,
			["stateMethodList"] = stateMethodList,
			["activeCrouchSkill"] = activeCrouchSkill,
			["pendingCrouchShowcases"] = pendingCrouchShowcases,
			["missileShowcaseQueued"] = _missileShowcaseQueued,
			["doomShowcaseQueued"] = _doomShowcaseQueued,
			["plantAttackActive"] = _plantAttackActive,
			["plantSpawnResolved"] = _plantSpawnResolved,
			["plantActionSequence"] = _plantActionSequence,
			["missileAttackActive"] = _missileAttackActive,
			["missileTargets"] = SerializeGridPositions(missileTargets),
			["missileTargetIndex"] = missileTargetIndex,
			["missileActionSequence"] = _missileActionSequence,
			["missileResolvedMask"] = missileResolvedMask,
			["missileCrosshairTrackingActive"] = _missileCrosshairTrackingActive,
			["doomCharging"] = _doomCharging,
			["doomChargeRemaining"] = doomChargeRemaining,
			["doomAttackResolved"] = _doomAttackResolved,
			["doomActionSequence"] = _doomActionSequence,
			["rvPosX"] = rvPos.X,
			["rvPosY"] = rvPos.Y,
			["networkSpecialStateRevision"] = _networkSpecialStateRevision
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		int num = data.GetValueOrDefault("bossDaveSaveVersion", 0).AsInt32();
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
		_headAttackPauseRemaining = data.GetValueOrDefault("headAttackPauseRemaining", 0.0).AsDouble();
		_headAttackPauseSequence = data.GetValueOrDefault("headAttackPauseSequence", 0).AsInt32();
		_headAttackPauseOwned = _headAttackPauseRemaining > 0.0;
		_clearLegacyHeadAttackPauseOnFinalize = !data.ContainsKey("headAttackPauseRemaining");
		headIdleNum = data.GetValueOrDefault("headIdleNum", 0).AsInt32();
		bungeeIsSpawn = data.GetValueOrDefault("bungeeIsSpawn", false).AsBool();
		_pendingBungeeNodeNames = ReadStringArray(data.GetValueOrDefault("bungeeNodeNames", new Array<string>()));
		_rvAttackArmed = data.GetValueOrDefault("rvAttackArmed", false).AsBool();
		stompId = data.GetValueOrDefault("stompId", -1).AsInt32();
		isRest = data.GetValueOrDefault("isRest", true).AsBool();
		restTime = data.GetValueOrDefault("restTime", 5.0).AsDouble();
		restTimer = data.GetValueOrDefault("restTimer", 0.0).AsDouble();
		stateMethodList = data.GetValueOrDefault("stateMethodList", new Godot.Collections.Array()).AsGodotArray();
		activeCrouchSkill = data.GetValueOrDefault("activeCrouchSkill", "").AsString();
		pendingCrouchShowcases = ReadStringArray(data.GetValueOrDefault("pendingCrouchShowcases", new Array<string>()));
		_missileShowcaseQueued = data.GetValueOrDefault("missileShowcaseQueued", false).AsBool();
		_doomShowcaseQueued = data.GetValueOrDefault("doomShowcaseQueued", false).AsBool();
		_plantAttackActive = data.GetValueOrDefault("plantAttackActive", false).AsBool();
		_plantSpawnResolved = data.GetValueOrDefault("plantSpawnResolved", false).AsBool();
		_plantActionSequence = data.GetValueOrDefault("plantActionSequence", 0).AsInt32();
		_missileAttackActive = data.GetValueOrDefault("missileAttackActive", false).AsBool();
		missileTargets = DeserializeGridPositions(data.GetValueOrDefault("missileTargets", new Array<int>()));
		missileTargetIndex = data.GetValueOrDefault("missileTargetIndex", 0).AsInt32();
		_missileActionSequence = data.GetValueOrDefault("missileActionSequence", 0).AsInt32();
		missileResolvedMask = data.GetValueOrDefault("missileResolvedMask", 0).AsInt32();
		_missileCrosshairTrackingActive = data.GetValueOrDefault("missileCrosshairTrackingActive", _missileAttackActive).AsBool();
		_doomCharging = data.GetValueOrDefault("doomCharging", false).AsBool();
		doomChargeRemaining = data.GetValueOrDefault("doomChargeRemaining", 0.0).AsDouble();
		_doomAttackResolved = data.GetValueOrDefault("doomAttackResolved", false).AsBool();
		_doomActionSequence = data.GetValueOrDefault("doomActionSequence", 0).AsInt32();
		rvPos = new Vector2I(data.GetValueOrDefault("rvPosX", 0).AsInt32(), data.GetValueOrDefault("rvPosY", 0).AsInt32());
		_networkSpecialStateRevision = data.GetValueOrDefault("networkSpecialStateRevision", 0).AsInt32();
		if (num < 1)
		{
			pendingCrouchShowcases.Clear();
			_missileShowcaseQueued = stage >= 1;
			_doomShowcaseQueued = stage >= 2;
			if (_missileShowcaseQueued)
			{
				pendingCrouchShowcases.Add("Missile");
			}
			if (_doomShowcaseQueued)
			{
				pendingCrouchShowcases.Add("Doom");
			}
		}
		if (num < 2)
		{
			missileResolvedMask = 0;
			_missileCrosshairTrackingActive = _missileAttackActive;
		}
		if (num < 3)
		{
			double num2 = data.GetValueOrDefault("missileLaunchDelayRemaining", 0.0).AsDouble();
			if (data.GetValueOrDefault("missileHoldActive", num2 > 0.0).AsBool() || num2 > 0.0)
			{
				_clearLegacyMissilePauseOnFinalize = true;
				_spawnLegacyDelayedMissileOnFinalize = _missileAttackActive && missileTargetIndex < missileTargets.Count;
				_legacyMissileEventFrame = data.GetValueOrDefault("missileHoldFrameIndex", 863).AsInt32();
			}
		}
		if (num < 4 && data.ContainsKey("doomAccumulatedDamage"))
		{
			_pendingLegacyDoomShieldDamage = data["doomAccumulatedDamage"].AsDouble();
			_hasPendingLegacyDoomShieldDamage = _doomCharging;
		}
		_hasPendingNetworkPresentation = true;
		if (!IsProgressRestoreInFlight && IsNodeReady())
		{
			Callable.From(ApplyPendingBossNetworkPresentation).CallDeferred();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(134)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsProgressStateMachineDefinitionIdCompatible, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "savedDefinitionId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "currentDefinitionId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveBossRuntimeComponents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitializeBossGameplay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectDoomShieldSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectDoomShieldSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectMissileLifecycleSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectMissileLifecycleSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.PlantEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlantProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlantExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MissileEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MissileProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MissileExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoomStartEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoomStartProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoomStartExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoomIdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoomIdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoomIdleExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoomEndEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoomEndProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoomEndExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoomCancelEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoomCancelProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoomCancelExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.UpdateHeadAttackPause, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginHeadAttackPause, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseHeadAttackPause, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "markDirty", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRemoteHeadAttackPause, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "incomingSequence", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "incomingRemaining", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StateRunning, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunSpawnSequence, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "targetCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "completesSpawnGroup", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.GetValidStandingSpecials, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetAvailableStompIds, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetBungeeTargetPositions, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasRvTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanForcePlantOnEmptyCell, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPlantFootprintCellEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanSpawnHypnotizedPlant, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnHypnotizedPlants, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartNextCrouchSkill, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueCrouchShowcase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "skill", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCrouchPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "ballKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareMissileAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnNextMissile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CatchUpRemoteMissileVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "firedCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnMissileProjectile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "targetGrid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressGameplay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnMissileLifecycleEnded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "ownerSequence", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMissileSlotResolved, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetAllMissileSlotsMask, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveMissileSlot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "markDirty", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshMissileCrosshairs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearMissileCrosshairs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EndMissileAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "markDirty", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "preserveInFlightCrosshairs", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetExposedHeadDamageableState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetRetractedHeadState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetDoomDamageableState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyBossPresentationForClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playOneShot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AdvanceDoomCharge, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureDoomShield, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "reset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveDoomShield, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDoomShieldBroken, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPendingLegacyDoomShieldDamage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shield", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseDoomAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayDoomReleaseEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinishStandingSpecial, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkBossStateDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsBossGameplayPaused, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ExplosionMethod, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWaterDiscardSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWaterDiscardSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpawnState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsNetworkSpecialMovementActive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRemoteNetworkAnimationStart, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportBossNetworkState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportBossNetworkState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SerializeGridPositions, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "positions", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DeserializeGridPositions, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadStringArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSavedBungeeNodeNames, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveSavedBungeeRelations, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyPendingBossNetworkPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyStagePresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.IsProgressStateMachineDefinitionIdCompatible && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProgressStateMachineDefinitionIdCompatible(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.ConnectDoomShieldSignals && args.Count == 0)
		{
			ConnectDoomShieldSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectDoomShieldSignals && args.Count == 0)
		{
			DisconnectDoomShieldSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectMissileLifecycleSignals && args.Count == 0)
		{
			ConnectMissileLifecycleSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectMissileLifecycleSignals && args.Count == 0)
		{
			DisconnectMissileLifecycleSignals();
			ret = default;
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
		if (method == MethodName.PlantEntered && args.Count == 0)
		{
			PlantEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.PlantProcessing && args.Count == 1)
		{
			PlantProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlantExited && args.Count == 0)
		{
			PlantExited();
			ret = default;
			return true;
		}
		if (method == MethodName.MissileEntered && args.Count == 0)
		{
			MissileEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.MissileProcessing && args.Count == 1)
		{
			MissileProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MissileExited && args.Count == 0)
		{
			MissileExited();
			ret = default;
			return true;
		}
		if (method == MethodName.DoomStartEntered && args.Count == 0)
		{
			DoomStartEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DoomStartProcessing && args.Count == 1)
		{
			DoomStartProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DoomStartExited && args.Count == 0)
		{
			DoomStartExited();
			ret = default;
			return true;
		}
		if (method == MethodName.DoomIdleEntered && args.Count == 0)
		{
			DoomIdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DoomIdleProcessing && args.Count == 1)
		{
			DoomIdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DoomIdleExited && args.Count == 0)
		{
			DoomIdleExited();
			ret = default;
			return true;
		}
		if (method == MethodName.DoomEndEntered && args.Count == 0)
		{
			DoomEndEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DoomEndProcessing && args.Count == 1)
		{
			DoomEndProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DoomEndExited && args.Count == 0)
		{
			DoomEndExited();
			ret = default;
			return true;
		}
		if (method == MethodName.DoomCancelEntered && args.Count == 0)
		{
			DoomCancelEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DoomCancelProcessing && args.Count == 1)
		{
			DoomCancelProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DoomCancelExited && args.Count == 0)
		{
			DoomCancelExited();
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
		if (method == MethodName.UpdateHeadAttackPause && args.Count == 1)
		{
			UpdateHeadAttackPause(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginHeadAttackPause && args.Count == 0)
		{
			BeginHeadAttackPause();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseHeadAttackPause && args.Count == 1)
		{
			ReleaseHeadAttackPause(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRemoteHeadAttackPause && args.Count == 2)
		{
			ApplyRemoteHeadAttackPause(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.StateRunning && args.Count == 1)
		{
			StateRunning(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunSpawnSequence && args.Count == 3)
		{
			RunSpawnSequence(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
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
		if (method == MethodName.GetValidStandingSpecials && args.Count == 0)
		{
			Array<string> validStandingSpecials = GetValidStandingSpecials();
			ret = VariantUtils.CreateFromArray(validStandingSpecials);
			return true;
		}
		if (method == MethodName.GetAvailableStompIds && args.Count == 0)
		{
			Array<int> availableStompIds = GetAvailableStompIds();
			ret = VariantUtils.CreateFromArray(availableStompIds);
			return true;
		}
		if (method == MethodName.GetBungeeTargetPositions && args.Count == 0)
		{
			Array<Vector2I> bungeeTargetPositions = GetBungeeTargetPositions();
			ret = VariantUtils.CreateFromArray(bungeeTargetPositions);
			return true;
		}
		if (method == MethodName.HasRvTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasRvTarget());
			return true;
		}
		if (method == MethodName.CanForcePlantOnEmptyCell && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanForcePlantOnEmptyCell(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.IsPlantFootprintCellEmpty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPlantFootprintCellEmpty(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.CanSpawnHypnotizedPlant && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSpawnHypnotizedPlant());
			return true;
		}
		if (method == MethodName.SpawnHypnotizedPlants && args.Count == 0)
		{
			SpawnHypnotizedPlants();
			ret = default;
			return true;
		}
		if (method == MethodName.StartNextCrouchSkill && args.Count == 0)
		{
			StartNextCrouchSkill();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueCrouchShowcase && args.Count == 1)
		{
			QueueCrouchShowcase(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCrouchPresentation && args.Count == 1)
		{
			SetCrouchPresentation(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareMissileAttack && args.Count == 0)
		{
			PrepareMissileAttack();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnNextMissile && args.Count == 0)
		{
			SpawnNextMissile();
			ret = default;
			return true;
		}
		if (method == MethodName.CatchUpRemoteMissileVisuals && args.Count == 1)
		{
			CatchUpRemoteMissileVisuals(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnMissileProjectile && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(SpawnMissileProjectile(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.OnMissileLifecycleEnded && args.Count == 4)
		{
			OnMissileLifecycleEnded(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<BulletField.BulletLifecycleTerminalReason>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsMissileSlotResolved && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMissileSlotResolved(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAllMissileSlotsMask && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetAllMissileSlotsMask());
			return true;
		}
		if (method == MethodName.ResolveMissileSlot && args.Count == 2)
		{
			ResolveMissileSlot(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshMissileCrosshairs && args.Count == 0)
		{
			RefreshMissileCrosshairs();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearMissileCrosshairs && args.Count == 0)
		{
			ClearMissileCrosshairs();
			ret = default;
			return true;
		}
		if (method == MethodName.EndMissileAttack && args.Count == 2)
		{
			EndMissileAttack(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetExposedHeadDamageableState && args.Count == 0)
		{
			SetExposedHeadDamageableState();
			ret = default;
			return true;
		}
		if (method == MethodName.SetRetractedHeadState && args.Count == 0)
		{
			SetRetractedHeadState();
			ret = default;
			return true;
		}
		if (method == MethodName.SetDoomDamageableState && args.Count == 0)
		{
			SetDoomDamageableState();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyBossPresentationForClip && args.Count == 2)
		{
			ApplyBossPresentationForClip(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceDoomCharge && args.Count == 1)
		{
			AdvanceDoomCharge(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureDoomShield && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseArmorInstance>(EnsureDoomShield(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveDoomShield && args.Count == 0)
		{
			RemoveDoomShield();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDoomShieldBroken && args.Count == 1)
		{
			OnDoomShieldBroken(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingLegacyDoomShieldDamage && args.Count == 1)
		{
			ApplyPendingLegacyDoomShieldDamage(VariantUtils.ConvertTo<TowerDefenseArmorInstance>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseDoomAttack && args.Count == 0)
		{
			ReleaseDoomAttack();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayDoomReleaseEffect && args.Count == 0)
		{
			PlayDoomReleaseEffect();
			ret = default;
			return true;
		}
		if (method == MethodName.FinishStandingSpecial && args.Count == 0)
		{
			FinishStandingSpecial();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkBossStateDirty && args.Count == 0)
		{
			MarkBossStateDirty();
			ret = default;
			return true;
		}
		if (method == MethodName.IsBossGameplayPaused && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBossGameplayPaused());
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
		if (method == MethodName.OnRemoteNetworkAnimationStart && args.Count == 1)
		{
			OnRemoteNetworkAnimationStart(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportBossNetworkState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportBossNetworkState());
			return true;
		}
		if (method == MethodName.ImportBossNetworkState && args.Count == 1)
		{
			ImportBossNetworkState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SerializeGridPositions && args.Count == 1)
		{
			Array<int> array = SerializeGridPositions(VariantUtils.ConvertToArray<Vector2I>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.DeserializeGridPositions && args.Count == 1)
		{
			Array<Vector2I> array2 = DeserializeGridPositions(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.ReadStringArray && args.Count == 1)
		{
			Array<string> array3 = ReadStringArray(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
			return true;
		}
		if (method == MethodName.GetSavedBungeeNodeNames && args.Count == 0)
		{
			Array<string> savedBungeeNodeNames = GetSavedBungeeNodeNames();
			ret = VariantUtils.CreateFromArray(savedBungeeNodeNames);
			return true;
		}
		if (method == MethodName.ResolveSavedBungeeRelations && args.Count == 0)
		{
			ResolveSavedBungeeRelations();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingBossNetworkPresentation && args.Count == 0)
		{
			ApplyPendingBossNetworkPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyStagePresentation && args.Count == 0)
		{
			ApplyStagePresentation();
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
		if (method == MethodName.CanForcePlantOnEmptyCell && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanForcePlantOnEmptyCell(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.IsPlantFootprintCellEmpty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPlantFootprintCellEmpty(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.IsBossGameplayPaused && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBossGameplayPaused());
			return true;
		}
		if (method == MethodName.SerializeGridPositions && args.Count == 1)
		{
			Array<int> array = SerializeGridPositions(VariantUtils.ConvertToArray<Vector2I>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.DeserializeGridPositions && args.Count == 1)
		{
			Array<Vector2I> array2 = DeserializeGridPositions(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.ReadStringArray && args.Count == 1)
		{
			Array<string> array3 = ReadStringArray(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
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
		if (method == MethodName.IsProgressStateMachineDefinitionIdCompatible)
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
		if (method == MethodName.ConnectDoomShieldSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectDoomShieldSignals)
		{
			return true;
		}
		if (method == MethodName.ConnectMissileLifecycleSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectMissileLifecycleSignals)
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
		if (method == MethodName.PlantEntered)
		{
			return true;
		}
		if (method == MethodName.PlantProcessing)
		{
			return true;
		}
		if (method == MethodName.PlantExited)
		{
			return true;
		}
		if (method == MethodName.MissileEntered)
		{
			return true;
		}
		if (method == MethodName.MissileProcessing)
		{
			return true;
		}
		if (method == MethodName.MissileExited)
		{
			return true;
		}
		if (method == MethodName.DoomStartEntered)
		{
			return true;
		}
		if (method == MethodName.DoomStartProcessing)
		{
			return true;
		}
		if (method == MethodName.DoomStartExited)
		{
			return true;
		}
		if (method == MethodName.DoomIdleEntered)
		{
			return true;
		}
		if (method == MethodName.DoomIdleProcessing)
		{
			return true;
		}
		if (method == MethodName.DoomIdleExited)
		{
			return true;
		}
		if (method == MethodName.DoomEndEntered)
		{
			return true;
		}
		if (method == MethodName.DoomEndProcessing)
		{
			return true;
		}
		if (method == MethodName.DoomEndExited)
		{
			return true;
		}
		if (method == MethodName.DoomCancelEntered)
		{
			return true;
		}
		if (method == MethodName.DoomCancelProcessing)
		{
			return true;
		}
		if (method == MethodName.DoomCancelExited)
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
		if (method == MethodName.UpdateHeadAttackPause)
		{
			return true;
		}
		if (method == MethodName.BeginHeadAttackPause)
		{
			return true;
		}
		if (method == MethodName.ReleaseHeadAttackPause)
		{
			return true;
		}
		if (method == MethodName.ApplyRemoteHeadAttackPause)
		{
			return true;
		}
		if (method == MethodName.StateRunning)
		{
			return true;
		}
		if (method == MethodName.RunSpawnSequence)
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
		if (method == MethodName.GetValidStandingSpecials)
		{
			return true;
		}
		if (method == MethodName.GetAvailableStompIds)
		{
			return true;
		}
		if (method == MethodName.GetBungeeTargetPositions)
		{
			return true;
		}
		if (method == MethodName.HasRvTarget)
		{
			return true;
		}
		if (method == MethodName.CanForcePlantOnEmptyCell)
		{
			return true;
		}
		if (method == MethodName.IsPlantFootprintCellEmpty)
		{
			return true;
		}
		if (method == MethodName.CanSpawnHypnotizedPlant)
		{
			return true;
		}
		if (method == MethodName.SpawnHypnotizedPlants)
		{
			return true;
		}
		if (method == MethodName.StartNextCrouchSkill)
		{
			return true;
		}
		if (method == MethodName.QueueCrouchShowcase)
		{
			return true;
		}
		if (method == MethodName.SetCrouchPresentation)
		{
			return true;
		}
		if (method == MethodName.PrepareMissileAttack)
		{
			return true;
		}
		if (method == MethodName.SpawnNextMissile)
		{
			return true;
		}
		if (method == MethodName.CatchUpRemoteMissileVisuals)
		{
			return true;
		}
		if (method == MethodName.SpawnMissileProjectile)
		{
			return true;
		}
		if (method == MethodName.OnMissileLifecycleEnded)
		{
			return true;
		}
		if (method == MethodName.IsMissileSlotResolved)
		{
			return true;
		}
		if (method == MethodName.GetAllMissileSlotsMask)
		{
			return true;
		}
		if (method == MethodName.ResolveMissileSlot)
		{
			return true;
		}
		if (method == MethodName.RefreshMissileCrosshairs)
		{
			return true;
		}
		if (method == MethodName.ClearMissileCrosshairs)
		{
			return true;
		}
		if (method == MethodName.EndMissileAttack)
		{
			return true;
		}
		if (method == MethodName.SetExposedHeadDamageableState)
		{
			return true;
		}
		if (method == MethodName.SetRetractedHeadState)
		{
			return true;
		}
		if (method == MethodName.SetDoomDamageableState)
		{
			return true;
		}
		if (method == MethodName.ApplyBossPresentationForClip)
		{
			return true;
		}
		if (method == MethodName.AdvanceDoomCharge)
		{
			return true;
		}
		if (method == MethodName.EnsureDoomShield)
		{
			return true;
		}
		if (method == MethodName.RemoveDoomShield)
		{
			return true;
		}
		if (method == MethodName.OnDoomShieldBroken)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingLegacyDoomShieldDamage)
		{
			return true;
		}
		if (method == MethodName.ReleaseDoomAttack)
		{
			return true;
		}
		if (method == MethodName.PlayDoomReleaseEffect)
		{
			return true;
		}
		if (method == MethodName.FinishStandingSpecial)
		{
			return true;
		}
		if (method == MethodName.MarkBossStateDirty)
		{
			return true;
		}
		if (method == MethodName.IsBossGameplayPaused)
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
		if (method == MethodName.OnRemoteNetworkAnimationStart)
		{
			return true;
		}
		if (method == MethodName.ExportBossNetworkState)
		{
			return true;
		}
		if (method == MethodName.ImportBossNetworkState)
		{
			return true;
		}
		if (method == MethodName.SerializeGridPositions)
		{
			return true;
		}
		if (method == MethodName.DeserializeGridPositions)
		{
			return true;
		}
		if (method == MethodName.ReadStringArray)
		{
			return true;
		}
		if (method == MethodName.GetSavedBungeeNodeNames)
		{
			return true;
		}
		if (method == MethodName.ResolveSavedBungeeRelations)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingBossNetworkPresentation)
		{
			return true;
		}
		if (method == MethodName.ApplyStagePresentation)
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
		if (name == PropertyName.abilityConfig)
		{
			abilityConfig = VariantUtils.ConvertTo<TowerDefenseZombieBossDaveAbilityConfig>(in value);
			return true;
		}
		if (name == PropertyName.stateMethodList)
		{
			stateMethodList = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
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
			bungeeList = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
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
		if (name == PropertyName.activeCrouchSkill)
		{
			activeCrouchSkill = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.pendingCrouchShowcases)
		{
			pendingCrouchShowcases = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.missileTargets)
		{
			missileTargets = VariantUtils.ConvertToArray<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.missileTargetIndex)
		{
			missileTargetIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.doomChargeRemaining)
		{
			doomChargeRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._missileAttackActive)
		{
			_missileAttackActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._plantAttackActive)
		{
			_plantAttackActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._plantSpawnResolved)
		{
			_plantSpawnResolved = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._doomCharging)
		{
			_doomCharging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._doomAttackResolved)
		{
			_doomAttackResolved = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._rvAttackArmed)
		{
			_rvAttackArmed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._missileShowcaseQueued)
		{
			_missileShowcaseQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._doomShowcaseQueued)
		{
			_doomShowcaseQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._doomShieldSignalsConnected)
		{
			_doomShieldSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._removingDoomShield)
		{
			_removingDoomShield = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasPendingLegacyDoomShieldDamage)
		{
			_hasPendingLegacyDoomShieldDamage = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingLegacyDoomShieldDamage)
		{
			_pendingLegacyDoomShieldDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._plantActionSequence)
		{
			_plantActionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._missileActionSequence)
		{
			_missileActionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._doomActionSequence)
		{
			_doomActionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._networkSpecialStateRevision)
		{
			_networkSpecialStateRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._remoteMissileVisualIndex)
		{
			_remoteMissileVisualIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._remoteMissileActionSequence)
		{
			_remoteMissileActionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._clearLegacyMissilePauseOnFinalize)
		{
			_clearLegacyMissilePauseOnFinalize = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._spawnLegacyDelayedMissileOnFinalize)
		{
			_spawnLegacyDelayedMissileOnFinalize = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._legacyMissileEventFrame)
		{
			_legacyMissileEventFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.missileResolvedMask)
		{
			missileResolvedMask = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._missileCrosshairTrackingActive)
		{
			_missileCrosshairTrackingActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._missileLifecycleSignalsConnected)
		{
			_missileLifecycleSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._headAttackPauseRemaining)
		{
			_headAttackPauseRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._headAttackPauseSequence)
		{
			_headAttackPauseSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._remoteHeadAttackPauseSequence)
		{
			_remoteHeadAttackPauseSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._headAttackPauseOwned)
		{
			_headAttackPauseOwned = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._preserveProgressStateOnNextWalk)
		{
			_preserveProgressStateOnNextWalk = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._clearLegacyHeadAttackPauseOnFinalize)
		{
			_clearLegacyHeadAttackPauseOnFinalize = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._networkSyncAccumulator)
		{
			_networkSyncAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._hasPendingNetworkPresentation)
		{
			_hasPendingNetworkPresentation = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingBungeeNodeNames)
		{
			_pendingBungeeNodeNames = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.IsHardControlImmune)
		{
			value = VariantUtils.CreateFrom<bool>(IsHardControlImmune);
			return true;
		}
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
		if (name == PropertyName.abilityConfig)
		{
			value = VariantUtils.CreateFrom(in abilityConfig);
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
		if (name == PropertyName.activeCrouchSkill)
		{
			value = VariantUtils.CreateFrom(in activeCrouchSkill);
			return true;
		}
		if (name == PropertyName.pendingCrouchShowcases)
		{
			value = VariantUtils.CreateFromArray(pendingCrouchShowcases);
			return true;
		}
		if (name == PropertyName.missileTargets)
		{
			value = VariantUtils.CreateFromArray(missileTargets);
			return true;
		}
		if (name == PropertyName.missileTargetIndex)
		{
			value = VariantUtils.CreateFrom(in missileTargetIndex);
			return true;
		}
		if (name == PropertyName.doomChargeRemaining)
		{
			value = VariantUtils.CreateFrom(in doomChargeRemaining);
			return true;
		}
		if (name == PropertyName._missileAttackActive)
		{
			value = VariantUtils.CreateFrom(in _missileAttackActive);
			return true;
		}
		if (name == PropertyName._plantAttackActive)
		{
			value = VariantUtils.CreateFrom(in _plantAttackActive);
			return true;
		}
		if (name == PropertyName._plantSpawnResolved)
		{
			value = VariantUtils.CreateFrom(in _plantSpawnResolved);
			return true;
		}
		if (name == PropertyName._doomCharging)
		{
			value = VariantUtils.CreateFrom(in _doomCharging);
			return true;
		}
		if (name == PropertyName._doomAttackResolved)
		{
			value = VariantUtils.CreateFrom(in _doomAttackResolved);
			return true;
		}
		if (name == PropertyName._rvAttackArmed)
		{
			value = VariantUtils.CreateFrom(in _rvAttackArmed);
			return true;
		}
		if (name == PropertyName._missileShowcaseQueued)
		{
			value = VariantUtils.CreateFrom(in _missileShowcaseQueued);
			return true;
		}
		if (name == PropertyName._doomShowcaseQueued)
		{
			value = VariantUtils.CreateFrom(in _doomShowcaseQueued);
			return true;
		}
		if (name == PropertyName._doomShieldSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _doomShieldSignalsConnected);
			return true;
		}
		if (name == PropertyName._removingDoomShield)
		{
			value = VariantUtils.CreateFrom(in _removingDoomShield);
			return true;
		}
		if (name == PropertyName._hasPendingLegacyDoomShieldDamage)
		{
			value = VariantUtils.CreateFrom(in _hasPendingLegacyDoomShieldDamage);
			return true;
		}
		if (name == PropertyName._pendingLegacyDoomShieldDamage)
		{
			value = VariantUtils.CreateFrom(in _pendingLegacyDoomShieldDamage);
			return true;
		}
		if (name == PropertyName._plantActionSequence)
		{
			value = VariantUtils.CreateFrom(in _plantActionSequence);
			return true;
		}
		if (name == PropertyName._missileActionSequence)
		{
			value = VariantUtils.CreateFrom(in _missileActionSequence);
			return true;
		}
		if (name == PropertyName._doomActionSequence)
		{
			value = VariantUtils.CreateFrom(in _doomActionSequence);
			return true;
		}
		if (name == PropertyName._networkSpecialStateRevision)
		{
			value = VariantUtils.CreateFrom(in _networkSpecialStateRevision);
			return true;
		}
		if (name == PropertyName._remoteMissileVisualIndex)
		{
			value = VariantUtils.CreateFrom(in _remoteMissileVisualIndex);
			return true;
		}
		if (name == PropertyName._remoteMissileActionSequence)
		{
			value = VariantUtils.CreateFrom(in _remoteMissileActionSequence);
			return true;
		}
		if (name == PropertyName._clearLegacyMissilePauseOnFinalize)
		{
			value = VariantUtils.CreateFrom(in _clearLegacyMissilePauseOnFinalize);
			return true;
		}
		if (name == PropertyName._spawnLegacyDelayedMissileOnFinalize)
		{
			value = VariantUtils.CreateFrom(in _spawnLegacyDelayedMissileOnFinalize);
			return true;
		}
		if (name == PropertyName._legacyMissileEventFrame)
		{
			value = VariantUtils.CreateFrom(in _legacyMissileEventFrame);
			return true;
		}
		if (name == PropertyName.missileResolvedMask)
		{
			value = VariantUtils.CreateFrom(in missileResolvedMask);
			return true;
		}
		if (name == PropertyName._missileCrosshairTrackingActive)
		{
			value = VariantUtils.CreateFrom(in _missileCrosshairTrackingActive);
			return true;
		}
		if (name == PropertyName._missileLifecycleSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _missileLifecycleSignalsConnected);
			return true;
		}
		if (name == PropertyName._headAttackPauseRemaining)
		{
			value = VariantUtils.CreateFrom(in _headAttackPauseRemaining);
			return true;
		}
		if (name == PropertyName._headAttackPauseSequence)
		{
			value = VariantUtils.CreateFrom(in _headAttackPauseSequence);
			return true;
		}
		if (name == PropertyName._remoteHeadAttackPauseSequence)
		{
			value = VariantUtils.CreateFrom(in _remoteHeadAttackPauseSequence);
			return true;
		}
		if (name == PropertyName._headAttackPauseOwned)
		{
			value = VariantUtils.CreateFrom(in _headAttackPauseOwned);
			return true;
		}
		if (name == PropertyName._preserveProgressStateOnNextWalk)
		{
			value = VariantUtils.CreateFrom(in _preserveProgressStateOnNextWalk);
			return true;
		}
		if (name == PropertyName._clearLegacyHeadAttackPauseOnFinalize)
		{
			value = VariantUtils.CreateFrom(in _clearLegacyHeadAttackPauseOnFinalize);
			return true;
		}
		if (name == PropertyName._networkSyncAccumulator)
		{
			value = VariantUtils.CreateFrom(in _networkSyncAccumulator);
			return true;
		}
		if (name == PropertyName._hasPendingNetworkPresentation)
		{
			value = VariantUtils.CreateFrom(in _hasPendingNetworkPresentation);
			return true;
		}
		if (name == PropertyName._pendingBungeeNodeNames)
		{
			value = VariantUtils.CreateFromArray(_pendingBungeeNodeNames);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.abilityConfig, PropertyHint.ResourceType, "TowerDefenseZombieBossDaveAbilityConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
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
			new PropertyInfo(Variant.Type.String, PropertyName.activeCrouchSkill, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.pendingCrouchShowcases, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.missileTargets, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.missileTargetIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.doomChargeRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._missileAttackActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._plantAttackActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._plantSpawnResolved, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._doomCharging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._doomAttackResolved, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._rvAttackArmed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._missileShowcaseQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._doomShowcaseQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._doomShieldSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._removingDoomShield, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPendingLegacyDoomShieldDamage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingLegacyDoomShieldDamage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._plantActionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._missileActionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._doomActionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._networkSpecialStateRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._remoteMissileVisualIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._remoteMissileActionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._clearLegacyMissilePauseOnFinalize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._spawnLegacyDelayedMissileOnFinalize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._legacyMissileEventFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.missileResolvedMask, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._missileCrosshairTrackingActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._missileLifecycleSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._headAttackPauseRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._headAttackPauseSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._remoteHeadAttackPauseSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._headAttackPauseOwned, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preserveProgressStateOnNextWalk, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._clearLegacyHeadAttackPauseOnFinalize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._networkSyncAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPendingNetworkPresentation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._pendingBungeeNodeNames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsHardControlImmune, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._bossGameplayInitialized, Variant.From(in _bossGameplayInitialized));
		info.AddProperty(PropertyName.abilityConfig, Variant.From(in abilityConfig));
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
		info.AddProperty(PropertyName.activeCrouchSkill, Variant.From(in activeCrouchSkill));
		info.AddProperty(PropertyName.pendingCrouchShowcases, Variant.CreateFrom(pendingCrouchShowcases));
		info.AddProperty(PropertyName.missileTargets, Variant.CreateFrom(missileTargets));
		info.AddProperty(PropertyName.missileTargetIndex, Variant.From(in missileTargetIndex));
		info.AddProperty(PropertyName.doomChargeRemaining, Variant.From(in doomChargeRemaining));
		info.AddProperty(PropertyName._missileAttackActive, Variant.From(in _missileAttackActive));
		info.AddProperty(PropertyName._plantAttackActive, Variant.From(in _plantAttackActive));
		info.AddProperty(PropertyName._plantSpawnResolved, Variant.From(in _plantSpawnResolved));
		info.AddProperty(PropertyName._doomCharging, Variant.From(in _doomCharging));
		info.AddProperty(PropertyName._doomAttackResolved, Variant.From(in _doomAttackResolved));
		info.AddProperty(PropertyName._rvAttackArmed, Variant.From(in _rvAttackArmed));
		info.AddProperty(PropertyName._missileShowcaseQueued, Variant.From(in _missileShowcaseQueued));
		info.AddProperty(PropertyName._doomShowcaseQueued, Variant.From(in _doomShowcaseQueued));
		info.AddProperty(PropertyName._doomShieldSignalsConnected, Variant.From(in _doomShieldSignalsConnected));
		info.AddProperty(PropertyName._removingDoomShield, Variant.From(in _removingDoomShield));
		info.AddProperty(PropertyName._hasPendingLegacyDoomShieldDamage, Variant.From(in _hasPendingLegacyDoomShieldDamage));
		info.AddProperty(PropertyName._pendingLegacyDoomShieldDamage, Variant.From(in _pendingLegacyDoomShieldDamage));
		info.AddProperty(PropertyName._plantActionSequence, Variant.From(in _plantActionSequence));
		info.AddProperty(PropertyName._missileActionSequence, Variant.From(in _missileActionSequence));
		info.AddProperty(PropertyName._doomActionSequence, Variant.From(in _doomActionSequence));
		info.AddProperty(PropertyName._networkSpecialStateRevision, Variant.From(in _networkSpecialStateRevision));
		info.AddProperty(PropertyName._remoteMissileVisualIndex, Variant.From(in _remoteMissileVisualIndex));
		info.AddProperty(PropertyName._remoteMissileActionSequence, Variant.From(in _remoteMissileActionSequence));
		info.AddProperty(PropertyName._clearLegacyMissilePauseOnFinalize, Variant.From(in _clearLegacyMissilePauseOnFinalize));
		info.AddProperty(PropertyName._spawnLegacyDelayedMissileOnFinalize, Variant.From(in _spawnLegacyDelayedMissileOnFinalize));
		info.AddProperty(PropertyName._legacyMissileEventFrame, Variant.From(in _legacyMissileEventFrame));
		info.AddProperty(PropertyName.missileResolvedMask, Variant.From(in missileResolvedMask));
		info.AddProperty(PropertyName._missileCrosshairTrackingActive, Variant.From(in _missileCrosshairTrackingActive));
		info.AddProperty(PropertyName._missileLifecycleSignalsConnected, Variant.From(in _missileLifecycleSignalsConnected));
		info.AddProperty(PropertyName._headAttackPauseRemaining, Variant.From(in _headAttackPauseRemaining));
		info.AddProperty(PropertyName._headAttackPauseSequence, Variant.From(in _headAttackPauseSequence));
		info.AddProperty(PropertyName._remoteHeadAttackPauseSequence, Variant.From(in _remoteHeadAttackPauseSequence));
		info.AddProperty(PropertyName._headAttackPauseOwned, Variant.From(in _headAttackPauseOwned));
		info.AddProperty(PropertyName._preserveProgressStateOnNextWalk, Variant.From(in _preserveProgressStateOnNextWalk));
		info.AddProperty(PropertyName._clearLegacyHeadAttackPauseOnFinalize, Variant.From(in _clearLegacyHeadAttackPauseOnFinalize));
		info.AddProperty(PropertyName._networkSyncAccumulator, Variant.From(in _networkSyncAccumulator));
		info.AddProperty(PropertyName._hasPendingNetworkPresentation, Variant.From(in _hasPendingNetworkPresentation));
		info.AddProperty(PropertyName._pendingBungeeNodeNames, Variant.CreateFrom(_pendingBungeeNodeNames));
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
		if (info.TryGetProperty(PropertyName.abilityConfig, out var value3))
		{
			abilityConfig = value3.As<TowerDefenseZombieBossDaveAbilityConfig>();
		}
		if (info.TryGetProperty(PropertyName.stateMethodList, out var value4))
		{
			stateMethodList = value4.As<Godot.Collections.Array>();
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
			bungeeList = value21.As<Godot.Collections.Array>();
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
		if (info.TryGetProperty(PropertyName.activeCrouchSkill, out var value29))
		{
			activeCrouchSkill = value29.As<string>();
		}
		if (info.TryGetProperty(PropertyName.pendingCrouchShowcases, out var value30))
		{
			pendingCrouchShowcases = value30.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.missileTargets, out var value31))
		{
			missileTargets = value31.AsGodotArray<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.missileTargetIndex, out var value32))
		{
			missileTargetIndex = value32.As<int>();
		}
		if (info.TryGetProperty(PropertyName.doomChargeRemaining, out var value33))
		{
			doomChargeRemaining = value33.As<double>();
		}
		if (info.TryGetProperty(PropertyName._missileAttackActive, out var value34))
		{
			_missileAttackActive = value34.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._plantAttackActive, out var value35))
		{
			_plantAttackActive = value35.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._plantSpawnResolved, out var value36))
		{
			_plantSpawnResolved = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._doomCharging, out var value37))
		{
			_doomCharging = value37.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._doomAttackResolved, out var value38))
		{
			_doomAttackResolved = value38.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rvAttackArmed, out var value39))
		{
			_rvAttackArmed = value39.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._missileShowcaseQueued, out var value40))
		{
			_missileShowcaseQueued = value40.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._doomShowcaseQueued, out var value41))
		{
			_doomShowcaseQueued = value41.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._doomShieldSignalsConnected, out var value42))
		{
			_doomShieldSignalsConnected = value42.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._removingDoomShield, out var value43))
		{
			_removingDoomShield = value43.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasPendingLegacyDoomShieldDamage, out var value44))
		{
			_hasPendingLegacyDoomShieldDamage = value44.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingLegacyDoomShieldDamage, out var value45))
		{
			_pendingLegacyDoomShieldDamage = value45.As<double>();
		}
		if (info.TryGetProperty(PropertyName._plantActionSequence, out var value46))
		{
			_plantActionSequence = value46.As<int>();
		}
		if (info.TryGetProperty(PropertyName._missileActionSequence, out var value47))
		{
			_missileActionSequence = value47.As<int>();
		}
		if (info.TryGetProperty(PropertyName._doomActionSequence, out var value48))
		{
			_doomActionSequence = value48.As<int>();
		}
		if (info.TryGetProperty(PropertyName._networkSpecialStateRevision, out var value49))
		{
			_networkSpecialStateRevision = value49.As<int>();
		}
		if (info.TryGetProperty(PropertyName._remoteMissileVisualIndex, out var value50))
		{
			_remoteMissileVisualIndex = value50.As<int>();
		}
		if (info.TryGetProperty(PropertyName._remoteMissileActionSequence, out var value51))
		{
			_remoteMissileActionSequence = value51.As<int>();
		}
		if (info.TryGetProperty(PropertyName._clearLegacyMissilePauseOnFinalize, out var value52))
		{
			_clearLegacyMissilePauseOnFinalize = value52.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._spawnLegacyDelayedMissileOnFinalize, out var value53))
		{
			_spawnLegacyDelayedMissileOnFinalize = value53.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._legacyMissileEventFrame, out var value54))
		{
			_legacyMissileEventFrame = value54.As<int>();
		}
		if (info.TryGetProperty(PropertyName.missileResolvedMask, out var value55))
		{
			missileResolvedMask = value55.As<int>();
		}
		if (info.TryGetProperty(PropertyName._missileCrosshairTrackingActive, out var value56))
		{
			_missileCrosshairTrackingActive = value56.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._missileLifecycleSignalsConnected, out var value57))
		{
			_missileLifecycleSignalsConnected = value57.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._headAttackPauseRemaining, out var value58))
		{
			_headAttackPauseRemaining = value58.As<double>();
		}
		if (info.TryGetProperty(PropertyName._headAttackPauseSequence, out var value59))
		{
			_headAttackPauseSequence = value59.As<int>();
		}
		if (info.TryGetProperty(PropertyName._remoteHeadAttackPauseSequence, out var value60))
		{
			_remoteHeadAttackPauseSequence = value60.As<int>();
		}
		if (info.TryGetProperty(PropertyName._headAttackPauseOwned, out var value61))
		{
			_headAttackPauseOwned = value61.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._preserveProgressStateOnNextWalk, out var value62))
		{
			_preserveProgressStateOnNextWalk = value62.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._clearLegacyHeadAttackPauseOnFinalize, out var value63))
		{
			_clearLegacyHeadAttackPauseOnFinalize = value63.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._networkSyncAccumulator, out var value64))
		{
			_networkSyncAccumulator = value64.As<double>();
		}
		if (info.TryGetProperty(PropertyName._hasPendingNetworkPresentation, out var value65))
		{
			_hasPendingNetworkPresentation = value65.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingBungeeNodeNames, out var value66))
		{
			_pendingBungeeNodeNames = value66.AsGodotArray<string>();
		}
	}
}
