using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Scene/TowerDefesne/TowerDefenseNew/TowerDefenseControlNew.cs")]
public class TowerDefenseControlNew : TowerDefenseControl
{
	public delegate void ViewBackEventHandler();

	private sealed class FeatureAddTransaction
	{
		public readonly List<StringName> AddedFeatureNames = new List<StringName>();
	}

	public new class MethodName : TowerDefenseControl.MethodName
	{
		public static readonly StringName EmitViewBack = "EmitViewBack";

		public static readonly StringName BeginPendingBattleOperation = "BeginPendingBattleOperation";

		public static readonly StringName IsPendingBattleOperationCurrent = "IsPendingBattleOperationCurrent";

		public static readonly StringName CompletePendingBattleOperation = "CompletePendingBattleOperation";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName CancelBattleLifetimes = "CancelBattleLifetimes";

		public static readonly StringName TrySendBattleStateEvent = "TrySendBattleStateEvent";

		public static readonly StringName SendStateEvent = "SendStateEvent";

		public new static readonly StringName Init = "Init";

		public static readonly StringName OldLevelInit = "OldLevelInit";

		public static readonly StringName NewLevelInit = "NewLevelInit";

		public static readonly StringName AddFeature = "AddFeature";

		public static readonly StringName AddProcessDependenceFeature = "AddProcessDependenceFeature";

		public static readonly StringName IsFeatureConfigured = "IsFeatureConfigured";

		public static readonly StringName DestroyFeatureSafely = "DestroyFeatureSafely";

		public static readonly StringName DestroyProcessSafely = "DestroyProcessSafely";

		public static readonly StringName DestroyAllFeaturesSafely = "DestroyAllFeaturesSafely";

		public static readonly StringName AbortBattleGraphInitialization = "AbortBattleGraphInitialization";

		public static readonly StringName GetConfiguredFeatureData = "GetConfiguredFeatureData";

		public static readonly StringName BuildMissingDependenceFeatureData = "BuildMissingDependenceFeatureData";

		public static readonly StringName ZombieWonLevelFail = "ZombieWonLevelFail";

		public static readonly StringName GetFeature = "GetFeature";

		public static readonly StringName RemoveFeature = "RemoveFeature";

		public static readonly StringName ComponentDependsOnFeature = "ComponentDependsOnFeature";

		public static readonly StringName ContainsFeatureName = "ContainsFeatureName";

		public new static readonly StringName SetProcess = "SetProcess";

		public static readonly StringName HasProcessName = "HasProcessName";

		public static readonly StringName AddNode = "AddNode";

		public static readonly StringName AddUI = "AddUI";

		public static readonly StringName MoveUI = "MoveUI";

		public static readonly StringName AddUIToTopBankContainer = "AddUIToTopBankContainer";

		public static readonly StringName AddUIToTopPropContainer = "AddUIToTopPropContainer";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public new static readonly StringName _UnhandledInput = "_UnhandledInput";

		public static readonly StringName OpenGamePauseDialog = "OpenGamePauseDialog";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName NpcTalkRequiresPreSpawnBeforeTalk = "NpcTalkRequiresPreSpawnBeforeTalk";

		public static readonly StringName GetGameEntryFeaturePriority = "GetGameEntryFeaturePriority";

		public static readonly StringName GameInitEntered = "GameInitEntered";

		public static readonly StringName RejectProgressLoad = "RejectProgressLoad";

		public static readonly StringName GameEntryEntered = "GameEntryEntered";

		public static readonly StringName GameEntryExited = "GameEntryExited";

		public static readonly StringName GameReadyEntered = "GameReadyEntered";

		public static readonly StringName GameReadyExited = "GameReadyExited";

		public static readonly StringName GameRunningEntered = "GameRunningEntered";

		public static readonly StringName GameRunningExited = "GameRunningExited";

		public static readonly StringName GameRunningProcessing = "GameRunningProcessing";

		public static readonly StringName ResumeEntryCharacters = "ResumeEntryCharacters";

		public static readonly StringName GameFailEntered = "GameFailEntered";

		public static readonly StringName OnSceneChange = "OnSceneChange";

		public static readonly StringName TriggerGameFail = "TriggerGameFail";

		public static readonly StringName TriggerGameEntry = "TriggerGameEntry";

		public static readonly StringName TipsPlay = "TipsPlay";

		public static readonly StringName ViewMap = "ViewMap";

		public static readonly StringName GameFail = "GameFail";

		public static readonly StringName GameEntry = "GameEntry";

		public static readonly StringName ProcessZombieCheckArea = "ProcessZombieCheckArea";

		public static readonly StringName HandleZombieEnterHouse = "HandleZombieEnterHouse";

		public static readonly StringName UISwitched = "UISwitched";

		public static readonly StringName PacketUIFront = "PacketUIFront";

		public static readonly StringName ChangeCostAdd = "ChangeCostAdd";

		public static readonly StringName ChangeCostRemove = "ChangeCostRemove";

		public static readonly StringName InitPlayerStatusPanel = "InitPlayerStatusPanel";

		public static readonly StringName GetNextSyncId = "GetNextSyncId";

		public static readonly StringName GetNextPacketSyncId = "GetNextPacketSyncId";

		public static readonly StringName RegisterSyncPacket = "RegisterSyncPacket";

		public static readonly StringName UnregisterSyncPacket = "UnregisterSyncPacket";

		public static readonly StringName RemoveCharacterSyncState = "RemoveCharacterSyncState";

		public static readonly StringName RemoveZombieSyncState = "RemoveZombieSyncState";

		public static readonly StringName RegisterSyncCharacter = "RegisterSyncCharacter";

		public static readonly StringName ApplyPendingDestroyToReadyCharacter = "ApplyPendingDestroyToReadyCharacter";

		public static readonly StringName DetachSyncCharacter = "DetachSyncCharacter";

		public static readonly StringName SyncCharacterInitTimer = "SyncCharacterInitTimer";

		public static readonly StringName SyncCharacterPositionTimer = "SyncCharacterPositionTimer";

		public static readonly StringName OnSyncCharacterDestroy = "OnSyncCharacterDestroy";

		public static readonly StringName CleanupCharacterCell = "CleanupCharacterCell";

		public static readonly StringName ApplyNetworkPauseUi = "ApplyNetworkPauseUi";

		public static readonly StringName ApplyNetworkResumeUi = "ApplyNetworkResumeUi";

		public static readonly StringName ApplyNetworkPauseFromSession = "ApplyNetworkPauseFromSession";

		public static readonly StringName GetOrCreateRemoteCursor = "GetOrCreateRemoteCursor";

		public static readonly StringName RemoveRemoteCursor = "RemoveRemoteCursor";

		public static readonly StringName ApplyRemoteCursorPosition = "ApplyRemoteCursorPosition";

		public static readonly StringName ApplyRemoteCursorPick = "ApplyRemoteCursorPick";

		public static readonly StringName OnChooseReady = "OnChooseReady";

		public static readonly StringName OnChooseOver = "OnChooseOver";

		public static readonly StringName DoChooseOver = "DoChooseOver";

		public static readonly StringName ShowChooseWaitLabel = "ShowChooseWaitLabel";

		public static readonly StringName HideChooseWaitLabel = "HideChooseWaitLabel";

		public static readonly StringName UpdateChooseWaitLabel = "UpdateChooseWaitLabel";

		public static readonly StringName RejectLevelConfiguration = "RejectLevelConfiguration";

		public static readonly StringName ReturnFromRejectedLevel = "ReturnFromRejectedLevel";

		public static readonly StringName PrepareSavedFeatures = "PrepareSavedFeatures";

		public static readonly StringName CompleteProgressRecovery = "CompleteProgressRecovery";

		public static readonly StringName ShowProgressRecoveryNotice = "ShowProgressRecoveryNotice";

		public static readonly StringName ProgressExitDestination = "ProgressExitDestination";
	}

	public new class PropertyName : TowerDefenseControl.PropertyName
	{
		public static readonly StringName IsNetworkPaused = "IsNetworkPaused";

		public static readonly StringName HasPendingBattleOperations = "HasPendingBattleOperations";

		public static readonly StringName GameStateLastSync = "GameStateLastSync";

		public static readonly StringName CharacterLastSyncState = "CharacterLastSyncState";

		public static readonly StringName ComponentLastSyncState = "ComponentLastSyncState";

		public static readonly StringName PendingDestroySyncIds = "PendingDestroySyncIds";

		public static readonly StringName ZombieLastSyncState = "ZombieLastSyncState";

		public static readonly StringName state = "state";

		public static readonly StringName levelControl = "levelControl";

		public static readonly StringName characterCanvasModulate = "characterCanvasModulate";

		public static readonly StringName characterNode = "characterNode";

		public static readonly StringName bankUILayer = "bankUILayer";

		public static readonly StringName uITopBankContainer = "uITopBankContainer";

		public static readonly StringName uITopPropContainer = "uITopPropContainer";

		public static readonly StringName mobileInterval = "mobileInterval";

		public static readonly StringName uiTopAnimationPlayer = "uiTopAnimationPlayer";

		public static readonly StringName _coexistHud = "_coexistHud";

		public static readonly StringName process = "process";

		public static readonly StringName zombieWon = "zombieWon";

		public static readonly StringName zombieCheckArea = "zombieCheckArea";

		public static readonly StringName isGameRunning = "isGameRunning";

		public static readonly StringName isGameFail = "isGameFail";

		public static readonly StringName isInit = "isInit";

		public static readonly StringName failCharacter = "failCharacter";

		public static readonly StringName waitPause = "waitPause";

		public static readonly StringName isView = "isView";

		public static readonly StringName _battleGraphReady = "_battleGraphReady";

		public static readonly StringName _battleShuttingDown = "_battleShuttingDown";

		public static readonly StringName _preserveLevelConfigOnExit = "_preserveLevelConfigOnExit";

		public static readonly StringName _gameRunningEntryReady = "_gameRunningEntryReady";

		public static readonly StringName _pendingDestroySyncIds = "_pendingDestroySyncIds";

		public static readonly StringName _syncIdCounter = "_syncIdCounter";

		public static readonly StringName _packetSyncIdCounter = "_packetSyncIdCounter";

		public static readonly StringName _isNetworkPaused = "_isNetworkPaused";

		public static readonly StringName _zombieLastSyncState = "_zombieLastSyncState";

		public static readonly StringName _chooseOverReceived = "_chooseOverReceived";

		public static readonly StringName _chooseWaitLabel = "_chooseWaitLabel";

		public static readonly StringName _playerStatusPanel = "_playerStatusPanel";

		public static readonly StringName PlayerColors = "PlayerColors";

		public static readonly StringName _gameStateLastSync = "_gameStateLastSync";

		public static readonly StringName _characterLastSyncState = "_characterLastSyncState";

		public static readonly StringName _componentLastSyncState = "_componentLastSyncState";

		public static readonly StringName _levelConfigurationRejected = "_levelConfigurationRejected";

		public static readonly StringName _restoringProgress = "_restoringProgress";

		public static readonly StringName _progressPause = "_progressPause";

		public static readonly StringName _progressNoticeShown = "_progressNoticeShown";
	}

	public new class SignalName : TowerDefenseControl.SignalName
	{
	}

	public StateChart state;

	public TowerDefenseInGameLevelControl levelControl;

	public CanvasModulate characterCanvasModulate;

	public Node2D characterNode;

	public CanvasLayer bankUILayer;

	public HBoxContainer uITopBankContainer;

	public HBoxContainer uITopPropContainer;

	public Control mobileInterval;

	public AnimationPlayer uiTopAnimationPlayer;

	private TowerDefenseCoexistHud _coexistHud;

	public System.Collections.Generic.Dictionary<int, CanvasLayer> uilayerDictionary = new System.Collections.Generic.Dictionary<int, CanvasLayer>();

	public System.Collections.Generic.Dictionary<int, CanvasLayer> layerDictionary = new System.Collections.Generic.Dictionary<int, CanvasLayer>();

	public System.Collections.Generic.Dictionary<StringName, TowerDefenseBattleFeature> featureDictionary = new System.Collections.Generic.Dictionary<StringName, TowerDefenseBattleFeature>();

	private readonly List<StringName> _featureInitializationOrder = new List<StringName>();

	private readonly HashSet<StringName> _featuresBeingAdded = new HashSet<StringName>();

	public TowerDefenseBattleProcess process;

	public List<TowerDefensePacketChangeCost> changeCostList = new List<TowerDefensePacketChangeCost>();

	public TowerDefenseZombieWon zombieWon;

	public AabbArea2D zombieCheckArea;

	private readonly HashSet<TowerDefenseZombie> _zombieCheckOverlaps = new HashSet<TowerDefenseZombie>();

	private readonly HashSet<TowerDefenseZombie> _zombieCheckScratch = new HashSet<TowerDefenseZombie>();

	private readonly List<TowerDefenseCharacter> _zombieCheckCandidates = new List<TowerDefenseCharacter>();

	public bool isGameRunning;

	public bool isGameFail;

	public bool isInit = true;

	public TowerDefenseCharacter failCharacter;

	public bool waitPause;

	public bool isView;

	private bool _battleGraphReady;

	private bool _battleShuttingDown;

	private bool _preserveLevelConfigOnExit;

	private bool _gameRunningEntryReady;

	private Dictionary _pendingDestroySyncIds = new Dictionary();

	private TowerDefenseBattleNetworkHost _battleNetworkHost;

	private System.Collections.Generic.Dictionary<int, int> _zombieSyncMissCount = new System.Collections.Generic.Dictionary<int, int>();

	private int _syncIdCounter;

	internal System.Collections.Generic.Dictionary<int, TowerDefenseCharacter> _syncCharacters = new System.Collections.Generic.Dictionary<int, TowerDefenseCharacter>();

	private int _packetSyncIdCounter;

	private System.Collections.Generic.Dictionary<int, TowerDefenseInGamePacketShow> _syncPackets = new System.Collections.Generic.Dictionary<int, TowerDefenseInGamePacketShow>();

	private bool _isNetworkPaused;

	private System.Collections.Generic.Dictionary<int, Vector2> _zombieTargetPositions = new System.Collections.Generic.Dictionary<int, Vector2>();

	private Dictionary _zombieLastSyncState = new Dictionary();

	private System.Collections.Generic.Dictionary<int, Vector2> _zombieSyncVelocities = new System.Collections.Generic.Dictionary<int, Vector2>();

	private System.Collections.Generic.Dictionary<int, double> _zombieLastSyncTime = new System.Collections.Generic.Dictionary<int, double>();

	private System.Collections.Generic.Dictionary<string, RemoteCursor> _remoteCursors = new System.Collections.Generic.Dictionary<string, RemoteCursor>();

	private HashSet<string> _chooseReadyPeers = new HashSet<string>();

	private bool _chooseOverReceived;

	private RichTextLabel _chooseWaitLabel;

	private PlayerStatusPanel _playerStatusPanel;

	private readonly TowerDefenseBattleOperationTracker _pendingBattleOperations = new TowerDefenseBattleOperationTracker();

	private readonly Color[] PlayerColors = new Color[4]
	{
		new Color(0.3f, 0.7f, 1f),
		new Color(1f, 0.5f, 0.3f),
		new Color(0.5f, 1f, 0.5f),
		new Color(1f, 1f, 0.3f)
	};

	private Dictionary _gameStateLastSync = new Dictionary();

	private Dictionary _characterLastSyncState = new Dictionary();

	private Dictionary _componentLastSyncState = new Dictionary();

	private bool _levelConfigurationRejected;

	private TowerDefenseLevelSaveConfigCSharp _restoringProgress;

	private readonly HashSet<TowerDefenseBattleComponentBase> _progressInitialized = new HashSet<TowerDefenseBattleComponentBase>();

	private DialogBoxBase _progressPause;

	private bool _progressNoticeShown;

	internal IDictionary<int, TowerDefenseInGamePacketShow> SyncPackets => _syncPackets;

	internal bool IsNetworkPaused => _isNetworkPaused;

	public bool HasPendingBattleOperations => _pendingBattleOperations.HasPending;

	internal Dictionary GameStateLastSync => _gameStateLastSync;

	internal Dictionary CharacterLastSyncState => _characterLastSyncState;

	internal Dictionary ComponentLastSyncState => _componentLastSyncState;

	internal Dictionary PendingDestroySyncIds => _pendingDestroySyncIds;

	internal Dictionary ZombieLastSyncState => _zombieLastSyncState;

	internal System.Collections.Generic.Dictionary<int, int> ZombieSyncMissCount => _zombieSyncMissCount;

	internal System.Collections.Generic.Dictionary<int, Vector2> ZombieTargetPositions => _zombieTargetPositions;

	internal System.Collections.Generic.Dictionary<int, Vector2> ZombieSyncVelocities => _zombieSyncVelocities;

	internal System.Collections.Generic.Dictionary<int, double> ZombieLastSyncTime => _zombieLastSyncTime;

	public XWModLevelIdentity ModLevelIdentity { get; private set; }

	public event ViewBackEventHandler OnViewBack;

	public void EmitViewBack()
	{
		OnViewBack?.Invoke();
	}

	public int BeginPendingBattleOperation()
	{
		return _pendingBattleOperations.Begin();
	}

	public bool IsPendingBattleOperationCurrent(int operationId)
	{
		return _pendingBattleOperations.IsCurrent(operationId);
	}

	public void CompletePendingBattleOperation(int operationId)
	{
		_pendingBattleOperations.Complete(operationId);
	}

	public override void _Ready()
	{
		state = GetNode<StateChart>("%State");
		levelControl = GetNode<TowerDefenseInGameLevelControl>("%TowerDefenseInGameLevelControl");
		characterCanvasModulate = GetNode<CanvasModulate>("%CharacterCanvasModulate");
		characterNode = GetNode<Node2D>("%CharacterNode");
		bankUILayer = GetNode<CanvasLayer>("%BankUILayer");
		uITopBankContainer = GetNode<HBoxContainer>("%UITopBankContainer");
		uITopPropContainer = GetNode<HBoxContainer>("%UITopPropContainer");
		mobileInterval = GetNode<Control>("%MobileInterval");
		uiTopAnimationPlayer = GetNode<AnimationPlayer>("%UITopAnimationPlayer");
		zombieWon = GetNode<TowerDefenseZombieWon>("%TowerDefenseZombieWon");
		zombieCheckArea = GetNode<AabbArea2D>("%ZombieCheckArea");
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			ResourceManager.Instance.RequireFullGameplayResourcesReady("TowerDefenseControlNew");
			TowerDefenseBattleRegistry.Init();
			TowerDefenseManager.Instance.pausePacket = false;
			TowerDefenseManager.Instance.pauseZombie = false;
			TowerDefenseManager.Instance.backPacket = false;
			TowerDefenseManager.Instance.backZombie = false;
			TowerDefenseManager.ClearDeathRecords();
			DropItemRegistry.Reset();
			_coexistHud = new TowerDefenseCoexistHud
			{
				Battle = this
			};
			uITopBankContainer.AddChild(_coexistHud, forceReadableName: false, InternalMode.Disabled);
			BattleEventBus.Instance.OnUiSwitched += UISwitched;
			BattleEventBus.Instance.OnPacketUIFront += PacketUIFront;
			SceneManager.Instance.OnSceneChange += OnSceneChange;
			PhonkComponent.phonkEnabled = GameSaveManager.Instance.GetConfigValue("PhonkEnabled").AsBool();
			PhonkComponent.phonkIntensity = (float)GameSaveManager.Instance.GetConfigValue("PhonkIntensity").AsDouble();
			if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance.currentLevelConfig))
			{
				Init(TowerDefenseManager.Instance.currentLevelConfig);
			}
			else
			{
				GD.PushError("[TowerDefenseControlNew] currentLevelConfig 无效!");
			}
			if (!_battleGraphReady)
			{
				GD.PushError("[TowerDefenseControlNew] Battle graph initialization failed; state startup was cancelled.");
				SceneManager.Instance?.CompleteSceneLoading();
				return;
			}
			UISwitched(GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool());
			GetNode<StateChartState>("State/State/CompoundState/GameInit").OnStateEntered += GameInitEntered;
			StateChartState node = GetNode<StateChartState>("State/State/CompoundState/GameEntry");
			node.OnStateEntered += GameEntryEntered;
			node.OnStateExited += GameEntryExited;
			StateChartState node2 = GetNode<StateChartState>("State/State/CompoundState/GameReady");
			node2.OnStateEntered += GameReadyEntered;
			node2.OnStateExited += GameReadyExited;
			StateChartState node3 = GetNode<StateChartState>("State/State/CompoundState/GameRunning");
			node3.OnStateEntered += GameRunningEntered;
			node3.OnStateExited += GameRunningExited;
			node3.OnStatePhysicsProcessing += GameRunningProcessing;
			GetNode<StateChartState>("State/State/CompoundState/GameFail").OnStateEntered += GameFailEntered;
			CallDeferred("SendStateEvent");
		}
	}

	public override void _ExitTree()
	{
		GameSaveManager.Instance?.EndProgressRecovery(this);
		_restoringProgress = null;
		_battleShuttingDown = true;
		CancelBattleLifetimes();
		_pendingBattleOperations.Clear();
		if (GodotObject.IsInstanceValid(SceneManager.Instance))
		{
			SceneManager.Instance.OnSceneChange -= OnSceneChange;
		}
		if (GodotObject.IsInstanceValid(BattleEventBus.Instance))
		{
			BattleEventBus.Instance.OnUiSwitched -= UISwitched;
			BattleEventBus.Instance.OnPacketUIFront -= PacketUIFront;
			if (_battleNetworkHost != null)
			{
				BattleEventBus.Instance.OnGameVictory -= _battleNetworkHost.HandleLocalVictory;
				BattleEventBus.Instance.OnGameFailed -= _battleNetworkHost.HandleLocalFailed;
			}
		}
		if (GodotObject.IsInstanceValid(MultiPlayerManager.Instance) && _battleNetworkHost != null)
		{
			MultiPlayerManager.Instance.OnMatchStateReceived -= _battleNetworkHost.HandleLegacyMessage;
			MultiPlayerManager.Instance.OnPeerLeft -= _battleNetworkHost.HandlePeerLeft;
		}
		_battleNetworkHost?.Dispose();
		_battleNetworkHost = null;
		DestroyProcessSafely(process, "scene exit");
		process = null;
		DestroyAllFeaturesSafely("scene exit");
		if (GodotObject.IsInstanceValid(ProjectileUpdateManager.Instance))
		{
			ProjectileUpdateManager.Instance.Clear();
		}
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance.characterRegistry))
		{
			TowerDefenseManager.Instance.characterRegistry.Clear();
		}
		TowerDefenseManager.ClearDeathRecords();
		AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
		TowerDefenseCharacterBuffHypnoses.CleanupBattleState();
		TowerDefenseGroundItemBase.ClearStaticBattleReferences();
		DragMoveComponent.ClearCurrentMoveCharacter();
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			TowerDefenseManager.Instance.ReleaseBattleReferences(this, _preserveLevelConfigOnExit);
		}
		levelConfig = null;
		levelControl = null;
		characterNode = null;
		MultiPlayerManager.Instance?.ReleaseBattleCompatibility();
	}

	private void CancelBattleLifetimes()
	{
		process?.CancelLifetime();
		foreach (TowerDefenseBattleFeature value in featureDictionary.Values)
		{
			value?.CancelLifetime();
		}
	}

	private void TrySendBattleStateEvent(StringName eventName)
	{
		if (!_battleShuttingDown && GodotObject.IsInstanceValid(this) && GodotObject.IsInstanceValid(state))
		{
			state.SendEvent(eventName);
		}
	}

	private async void SendStateEvent()
	{
		if (state == null)
		{
			GD.PushError("[SendStateEvent] state null!");
			return;
		}
		for (int i = 0; i < state.GetChildCount(); i++)
		{
			state.GetChild(i);
		}
		await ToSignal(Engine.GetMainLoop(), "process_frame");
		await ToSignal(Engine.GetMainLoop(), "process_frame");
		CompoundState compoundState = ((state.GetChildCount() > 0) ? (state.GetChild(0) as CompoundState) : null);
		if (compoundState != null)
		{
			for (int j = 0; j < compoundState.GetChildCount(); j++)
			{
				_ = compoundState.GetChild(j) is StateChartState;
			}
		}
		else
		{
			GD.PushError("[SendStateEvent] CompoundState null 或类型不匹配!");
		}
		try
		{
			TrySendBattleStateEvent("ToGameInit");
		}
		catch (Exception value)
		{
			GD.PushError($"[SendStateEvent] SendEvent 异常: {value}");
		}
	}

	public override void Init(TowerDefenseLevelBaseConfig _levelConfig)
	{
		ModLevelIdentity = ((Global.Instance.enterLevelMode == "ModLevel") ? XWModLevelSession.Current : null);
		if (Global.Instance.enterLevelMode == "ModLevel" && (ModLevelIdentity == null || ModLevelIdentity.LevelSaveKey != _levelConfig.name))
		{
			throw new InvalidOperationException("Mod 关卡身份与启动配置不匹配");
		}
		if (Global.IsMultiplayerMode && _battleNetworkHost == null)
		{
			_battleNetworkHost = TowerDefenseBattleNetworkHost.TryCreate(new TowerDefenseBattleNetworkContext(this));
		}
		base.Init(_levelConfig);
		if (levelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig)
		{
			Global.TimeScale = towerDefenseLevelConfig.baseTimeScale;
		}
		if (levelConfig.name != "")
		{
			if (!Global.IsMultiplayerMode && GameSaveManager.Instance.CanUseLevelProgressInCurrentMode() && GameSaveManager.Instance.HasLevelProgress(levelConfig.name, ModLevelIdentity) && TowerDefenseManager.GetGameMethod() != TowerDefenseEnum.LEVEL_FINISH_METHOD.QUIZ)
			{
				hasProgress = true;
			}
			if (!Global.IsMultiplayerMode)
			{
				Dictionary dictionary = ((ModLevelIdentity == null) ? GameSaveManager.Instance.GetLevelValue(levelConfig.name) : XWModPlayerProgressService.GetLevel(ModLevelIdentity));
				Dictionary dictionary2 = dictionary.GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
				int num = dictionary2.GetValueOrDefault("Play", 0).AsInt32();
				dictionary2["Play"] = num + 1;
				dictionary["Key"] = dictionary2;
				if (ModLevelIdentity != null)
				{
					XWModPlayerProgressService.SetLevel(ModLevelIdentity, dictionary);
				}
				else
				{
					GameSaveManager.Instance.SetLevelValue(levelConfig.name, dictionary);
					GameSaveManager.Instance.Save();
				}
			}
		}
		bool flag;
		try
		{
			flag = ((!(levelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig2)) ? (levelConfig is TowerDefenseLevelNewConfig towerDefenseLevelNewConfig && NewLevelInit(towerDefenseLevelNewConfig)) : OldLevelInit(towerDefenseLevelConfig2));
		}
		catch (Exception value)
		{
			GD.PushError($"[BattleGraph] Initialization threw: {value}");
			flag = false;
		}
		if (!flag)
		{
			AbortBattleGraphInitialization();
			return;
		}
		_battleGraphReady = true;
		if (CommandManager.Instance.debugOpenGlove)
		{
			AddFeature("Glove", new Dictionary());
		}
		if (Global.IsMultiplayerMode)
		{
			if (_battleNetworkHost != null)
			{
				MultiPlayerManager.Instance.OnMatchStateReceived += _battleNetworkHost.HandleLegacyMessage;
				MultiPlayerManager.Instance.OnPeerLeft += _battleNetworkHost.HandlePeerLeft;
				BattleEventBus.Instance.OnGameVictory += _battleNetworkHost.HandleLocalVictory;
				BattleEventBus.Instance.OnGameFailed += _battleNetworkHost.HandleLocalFailed;
				_battleNetworkHost.RequestFullSnapshot();
			}
			InitPlayerStatusPanel();
		}
	}

	public bool OldLevelInit(TowerDefenseLevelConfig _levelConfig)
	{
		if (GodotObject.IsInstanceValid(_levelConfig.data))
		{
			_levelConfig.ExportToFeatureProcess();
		}
		foreach (StringName key in _levelConfig.featureData.Keys)
		{
			if (!AddFeature(key, _levelConfig.featureData[key]))
			{
				return false;
			}
		}
		if (HasProcessName(_levelConfig.processName))
		{
			if (_levelConfig.processName == (StringName)"Wave" && _levelConfig.finishMethod == TowerDefenseEnum.LEVEL_FINISH_METHOD.IZM2)
			{
				if (!SetProcess("IZM2", _levelConfig.processData))
				{
					return false;
				}
			}
			else if (!SetProcess(_levelConfig.processName, _levelConfig.processData))
			{
				return false;
			}
		}
		foreach (TowerDefenseBattleFeature value in featureDictionary.Values)
		{
			value.OnReady();
		}
		process?.OnReady();
		levelControl?.Init(_levelConfig);
		return true;
	}

	public bool NewLevelInit(TowerDefenseLevelNewConfig _levelConfig)
	{
		foreach (StringName key in _levelConfig.featureData.Keys)
		{
			if (!AddFeature(key, _levelConfig.featureData[key]))
			{
				return false;
			}
		}
		if (HasProcessName(_levelConfig.processName) && !SetProcess(_levelConfig.processName, _levelConfig.processData))
		{
			return false;
		}
		foreach (TowerDefenseBattleFeature value in featureDictionary.Values)
		{
			value.OnReady();
		}
		process?.OnReady();
		return true;
	}

	public bool AddFeature(StringName featureName, Dictionary data)
	{
		if (_battleShuttingDown)
		{
			return false;
		}
		if (featureDictionary.ContainsKey(featureName))
		{
			return true;
		}
		FeatureAddTransaction transaction = new FeatureAddTransaction();
		if (TryAddFeature(featureName, data, transaction, out var failureReason))
		{
			return true;
		}
		RollbackFeatureAddTransaction(transaction);
		GD.PushError($"[AddFeature] Failed to add {featureName}: {failureReason}");
		return false;
	}

	private bool TryAddFeature(StringName featureName, Dictionary data, FeatureAddTransaction transaction, out string failureReason)
	{
		failureReason = "";
		if (featureDictionary.ContainsKey(featureName))
		{
			return true;
		}
		if (!_featuresBeingAdded.Add(featureName))
		{
			failureReason = $"circular feature dependency at {featureName}";
			return false;
		}
		TowerDefenseBattleFeature towerDefenseBattleFeature = null;
		try
		{
			towerDefenseBattleFeature = TowerDefenseBattleRegistry.GetFeature(featureName);
			if (towerDefenseBattleFeature == null)
			{
				failureReason = $"feature {featureName} is not registered";
				return false;
			}
			if (towerDefenseBattleFeature.dependenceData != null)
			{
				foreach (StringName requiredFeatureName in towerDefenseBattleFeature.dependenceData.RequiredFeatureNames)
				{
					if (!TryAddDependenceFeature(requiredFeatureName, transaction, out var failureReason2))
					{
						failureReason = $"dependency {requiredFeatureName} for {featureName} failed: {failureReason2}";
						return false;
					}
				}
				foreach (StringName item in towerDefenseBattleFeature.dependenceData.ConfiguredOptionalFeatureNames ?? new Array<StringName>())
				{
					if (!TryAddConfiguredOptionalFeature(item, transaction, out var failureReason3))
					{
						failureReason = $"configured optional dependency {item} for {featureName} failed: {failureReason3}";
						return false;
					}
				}
			}
			towerDefenseBattleFeature.control = this;
			towerDefenseBattleFeature.Init(data ?? new Dictionary());
			featureDictionary[featureName] = towerDefenseBattleFeature;
			_featureInitializationOrder.Add(featureName);
			transaction.AddedFeatureNames.Add(featureName);
			return true;
		}
		catch (Exception value)
		{
			if (featureDictionary.TryGetValue(featureName, out var value2) && value2 == towerDefenseBattleFeature)
			{
				featureDictionary.Remove(featureName);
				_featureInitializationOrder.Remove(featureName);
			}
			DestroyFeatureSafely(featureName, towerDefenseBattleFeature, "initialization failure");
			failureReason = $"{towerDefenseBattleFeature?.GetType().Name ?? "unknown feature"} initialization threw: {value}";
			return false;
		}
		finally
		{
			_featuresBeingAdded.Remove(featureName);
		}
	}

	public void AddProcessDependenceFeature()
	{
		if (process != null)
		{
			FeatureAddTransaction transaction = new FeatureAddTransaction();
			if (!TryAddProcessDependenceFeatures(process, transaction, out var failureReason))
			{
				RollbackFeatureAddTransaction(transaction);
				GD.PushError("[ProcessDependencies] Failed for " + process.GetType().Name + ": " + failureReason);
			}
		}
	}

	private bool TryAddProcessDependenceFeatures(TowerDefenseBattleProcess targetProcess, FeatureAddTransaction transaction, out string failureReason)
	{
		failureReason = "";
		if (targetProcess == null || targetProcess.dependenceData == null)
		{
			return true;
		}
		foreach (StringName requiredFeatureName in targetProcess.dependenceData.RequiredFeatureNames)
		{
			if (!TryAddDependenceFeature(requiredFeatureName, transaction, out failureReason))
			{
				return false;
			}
		}
		foreach (StringName item in targetProcess.dependenceData.ConfiguredOptionalFeatureNames ?? new Array<StringName>())
		{
			if (!TryAddConfiguredOptionalFeature(item, transaction, out failureReason))
			{
				return false;
			}
		}
		return true;
	}

	private bool TryAddDependenceFeature(StringName featureName, FeatureAddTransaction transaction, out string failureReason)
	{
		failureReason = "";
		if (featureDictionary.ContainsKey(featureName))
		{
			return true;
		}
		try
		{
			return TryAddFeature(featureName, GetConfiguredFeatureData(featureName), transaction, out failureReason);
		}
		catch (Exception value)
		{
			failureReason = $"failed to build configuration for {featureName}: {value}";
			return false;
		}
	}

	private bool TryAddConfiguredOptionalFeature(StringName featureName, FeatureAddTransaction transaction, out string failureReason)
	{
		failureReason = "";
		if (featureDictionary.ContainsKey(featureName) || !IsFeatureConfigured(featureName))
		{
			return true;
		}
		return TryAddDependenceFeature(featureName, transaction, out failureReason);
	}

	private bool IsFeatureConfigured(StringName featureName)
	{
		TowerDefenseLevelBaseConfig towerDefenseLevelBaseConfig = levelConfig;
		if (!(towerDefenseLevelBaseConfig is TowerDefenseLevelConfig towerDefenseLevelConfig))
		{
			if (towerDefenseLevelBaseConfig is TowerDefenseLevelNewConfig towerDefenseLevelNewConfig)
			{
				return towerDefenseLevelNewConfig.featureData.ContainsKey(featureName);
			}
			return false;
		}
		return towerDefenseLevelConfig.featureData.ContainsKey(featureName);
	}

	private void RollbackFeatureAddTransaction(FeatureAddTransaction transaction)
	{
		for (int num = transaction.AddedFeatureNames.Count - 1; num >= 0; num--)
		{
			StringName stringName = transaction.AddedFeatureNames[num];
			if (featureDictionary.Remove(stringName, out var value))
			{
				_featureInitializationOrder.Remove(stringName);
				DestroyFeatureSafely(stringName, value, "dependency rollback");
			}
		}
		transaction.AddedFeatureNames.Clear();
	}

	private static void DestroyFeatureSafely(StringName featureName, TowerDefenseBattleFeature feature, string context)
	{
		if (feature == null)
		{
			return;
		}
		try
		{
			feature.CancelLifetime();
			feature.Destroy();
		}
		catch (Exception value)
		{
			GD.PushError($"[FeatureCleanup] Failed to destroy {featureName} during {context}: {value}");
		}
		finally
		{
			feature.control = null;
		}
	}

	private static void DestroyProcessSafely(TowerDefenseBattleProcess targetProcess, string context)
	{
		if (targetProcess == null)
		{
			return;
		}
		try
		{
			targetProcess.CancelLifetime();
			targetProcess.Destroy();
		}
		catch (Exception value)
		{
			GD.PushError($"[ProcessCleanup] Failed to destroy {targetProcess.GetType().Name} during {context}: {value}");
		}
		finally
		{
			targetProcess.control = null;
		}
	}

	private void DestroyAllFeaturesSafely(string context)
	{
		for (int num = _featureInitializationOrder.Count - 1; num >= 0; num--)
		{
			StringName stringName = _featureInitializationOrder[num];
			if (featureDictionary.Remove(stringName, out var value))
			{
				DestroyFeatureSafely(stringName, value, context);
			}
		}
		_featureInitializationOrder.Clear();
		foreach (KeyValuePair<StringName, TowerDefenseBattleFeature> item in featureDictionary.ToList())
		{
			featureDictionary.Remove(item.Key);
			DestroyFeatureSafely(item.Key, item.Value, context);
		}
	}

	private void AbortBattleGraphInitialization()
	{
		_battleGraphReady = false;
		DestroyProcessSafely(process, "battle graph abort");
		process = null;
		DestroyAllFeaturesSafely("battle graph abort");
	}

	internal Dictionary GetConfiguredFeatureData(StringName featureName)
	{
		if (levelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig && towerDefenseLevelConfig.featureData.TryGetValue(featureName, out var value))
		{
			return value;
		}
		if (levelConfig is TowerDefenseLevelNewConfig towerDefenseLevelNewConfig && towerDefenseLevelNewConfig.featureData.TryGetValue(featureName, out var value2))
		{
			return value2;
		}
		return BuildMissingDependenceFeatureData(featureName);
	}

	private Dictionary BuildMissingDependenceFeatureData(StringName featureName)
	{
		Dictionary dictionary = new Dictionary();
		if (!(levelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig))
		{
			return dictionary;
		}
		string text = featureName.ToString();
		if (!(text == "PacketBank"))
		{
			if (text == "SeedBank")
			{
				dictionary["Method"] = Enum.GetName(typeof(TowerDefenseEnum.LEVEL_SEEDBANK_METHOD), towerDefenseLevelConfig.packetBankMethod);
				dictionary["PlantColumn"] = towerDefenseLevelConfig.plantColumn;
				dictionary["ColdDownStart"] = towerDefenseLevelConfig.packetColdDownStart;
				dictionary["ColdDownUse"] = towerDefenseLevelConfig.packetColdDownUse;
				dictionary["Packet"] = new Godot.Collections.Array();
				foreach (Variant packetBank in towerDefenseLevelConfig.packetBankList)
				{
					if (packetBank.VariantType == Variant.Type.Object)
					{
						TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig = packetBank.As<TowerDefenseLevelPacketConfig>();
						if (towerDefenseLevelPacketConfig != null)
						{
							((Godot.Collections.Array)dictionary["Packet"]).Add(towerDefenseLevelPacketConfig.Export());
						}
					}
					else if (packetBank.VariantType == Variant.Type.String)
					{
						((Godot.Collections.Array)dictionary["Packet"]).Add(packetBank);
					}
				}
			}
		}
		else
		{
			dictionary["PacketBankName"] = (string.IsNullOrEmpty(towerDefenseLevelConfig.packetBank) ? "GeneralPlant" : towerDefenseLevelConfig.packetBank);
		}
		return dictionary;
	}

	public void ZombieWonLevelFail(bool playAnime = true)
	{
		if (GodotObject.IsInstanceValid(zombieWon))
		{
			zombieWon.LevelFail(playAnime);
		}
	}

	public TowerDefenseBattleFeature GetFeature(StringName featureName)
	{
		if (!featureDictionary.TryGetValue(featureName, out var value))
		{
			return null;
		}
		return value;
	}

	public T GetFeature<T>(StringName featureName) where T : TowerDefenseBattleFeature
	{
		if (!featureDictionary.TryGetValue(featureName, out var value))
		{
			return null;
		}
		return value as T;
	}

	public bool TryGetFeature<T>(StringName featureName, out T feature) where T : TowerDefenseBattleFeature
	{
		feature = GetFeature<T>(featureName);
		return feature != null;
	}

	public bool RemoveFeature(StringName featureName)
	{
		if (!CanRemoveFeature(featureName, out var failureReason))
		{
			GD.PushWarning($"[RemoveFeature] Rejected {featureName}: {failureReason}");
			return false;
		}
		if (!featureDictionary.Remove(featureName, out var value))
		{
			return false;
		}
		_featureInitializationOrder.Remove(featureName);
		DestroyFeatureSafely(featureName, value, "feature removal");
		return true;
	}

	private bool CanRemoveFeature(StringName featureName, out string failureReason)
	{
		failureReason = "";
		if (!featureDictionary.ContainsKey(featureName))
		{
			return true;
		}
		if (ComponentDependsOnFeature(process, featureName))
		{
			failureReason = "active Process " + process.GetType().Name + " depends on it";
			return false;
		}
		foreach (KeyValuePair<StringName, TowerDefenseBattleFeature> item in featureDictionary)
		{
			if (!(item.Key == featureName) && ComponentDependsOnFeature(item.Value, featureName))
			{
				failureReason = $"active Feature {item.Key} depends on it";
				return false;
			}
		}
		return true;
	}

	private static bool ComponentDependsOnFeature(TowerDefenseBattleComponentBase component, StringName featureName)
	{
		if (component?.dependenceData == null)
		{
			return false;
		}
		if (!ContainsFeatureName(component.dependenceData.RequiredFeatureNames, featureName))
		{
			return ContainsFeatureName(component.dependenceData.ConfiguredOptionalFeatureNames, featureName);
		}
		return true;
	}

	private static bool ContainsFeatureName(Array<StringName> featureNames, StringName featureName)
	{
		if (featureNames == null)
		{
			return false;
		}
		foreach (StringName featureName2 in featureNames)
		{
			if (featureName2 == featureName)
			{
				return true;
			}
		}
		return false;
	}

	public bool SetProcess(StringName processName, Dictionary data = null)
	{
		if (_battleShuttingDown)
		{
			return false;
		}
		if (!HasProcessName(processName))
		{
			GD.PushError("[SetProcess] Process name is empty.");
			return false;
		}
		if (data == null)
		{
			data = new Dictionary();
		}
		TowerDefenseBattleProcess towerDefenseBattleProcess = TowerDefenseBattleRegistry.GetProcess(processName);
		if (towerDefenseBattleProcess == null)
		{
			return false;
		}
		towerDefenseBattleProcess.control = this;
		FeatureAddTransaction transaction = new FeatureAddTransaction();
		if (!TryAddProcessDependenceFeatures(towerDefenseBattleProcess, transaction, out var failureReason))
		{
			RollbackFeatureAddTransaction(transaction);
			DestroyProcessSafely(towerDefenseBattleProcess, "dependency failure");
			GD.PushError($"[SetProcess] Failed to resolve dependencies for {processName}: {failureReason}");
			return false;
		}
		try
		{
			towerDefenseBattleProcess.Init(data);
			TowerDefenseBattleProcess towerDefenseBattleProcess2 = process;
			process = towerDefenseBattleProcess;
			if (towerDefenseBattleProcess2 != null && towerDefenseBattleProcess2 != towerDefenseBattleProcess)
			{
				DestroyProcessSafely(towerDefenseBattleProcess2, "process replacement");
			}
			return true;
		}
		catch (Exception value)
		{
			RollbackFeatureAddTransaction(transaction);
			DestroyProcessSafely(towerDefenseBattleProcess, "initialization failure");
			GD.PushError($"[SetProcess] Failed to initialize {processName} ({towerDefenseBattleProcess.GetType().Name}): {value}");
			return false;
		}
	}

	private static bool HasProcessName(StringName processName)
	{
		if (processName != null)
		{
			return !processName.IsEmpty;
		}
		return false;
	}

	public bool CanCreateProgressSave(out string reason)
	{
		if (HasPendingBattleOperations)
		{
			reason = "a transient battle operation is still running";
			return false;
		}
		foreach (KeyValuePair<StringName, TowerDefenseBattleFeature> item in featureDictionary)
		{
			if (item.Value is ITowerDefenseProgressSaveGuard towerDefenseProgressSaveGuard && !towerDefenseProgressSaveGuard.CanSaveProgress(out var reason2))
			{
				reason = $"Feature[{item.Key}]: {reason2}";
				return false;
			}
		}
		if (process is ITowerDefenseProgressSaveGuard towerDefenseProgressSaveGuard2 && !towerDefenseProgressSaveGuard2.CanSaveProgress(out var reason3))
		{
			reason = "Process[" + process.GetType().Name + "]: " + reason3;
			return false;
		}
		reason = "";
		return true;
	}

	public void AddNode(Node node, int layerId = -1)
	{
		if (!layerDictionary.ContainsKey(layerId))
		{
			CanvasLayer canvasLayer = new CanvasLayer();
			canvasLayer.Layer = layerId;
			canvasLayer.FollowViewportEnabled = true;
			AddChild(canvasLayer, forceReadableName: false, InternalMode.Disabled);
			layerDictionary[layerId] = canvasLayer;
		}
		layerDictionary[layerId].AddChild(node, forceReadableName: false, InternalMode.Disabled);
	}

	public void AddUI(Node node, int layerId)
	{
		if (!uilayerDictionary.ContainsKey(layerId))
		{
			CanvasLayer canvasLayer = new CanvasLayer();
			canvasLayer.Layer = layerId;
			canvasLayer.FollowViewportEnabled = false;
			AddChild(canvasLayer, forceReadableName: false, InternalMode.Disabled);
			uilayerDictionary[layerId] = canvasLayer;
		}
		uilayerDictionary[layerId].AddChild(node, forceReadableName: false, InternalMode.Disabled);
	}

	public void MoveUI(Node node, int layerId)
	{
		if (!uilayerDictionary.ContainsKey(layerId))
		{
			CanvasLayer canvasLayer = new CanvasLayer();
			canvasLayer.Layer = layerId;
			canvasLayer.FollowViewportEnabled = false;
			AddChild(canvasLayer, forceReadableName: false, InternalMode.Disabled);
			uilayerDictionary[layerId] = canvasLayer;
		}
		CanvasLayer newParent = uilayerDictionary[layerId];
		node.Reparent(newParent);
	}

	public void AddUIToTopBankContainer(Node node)
	{
		if (_coexistHud != null)
		{
			_coexistHud.RegisterBank(node);
		}
		else
		{
			uITopBankContainer?.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public void AddUIToTopPropContainer(Node node)
	{
		uITopPropContainer?.AddChild(node, forceReadableName: false, InternalMode.Disabled);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (isGameRunning)
		{
			TowerDefenseManager.Instance.runGameTime += delta;
			if (!waitPause && !Global.IsMultiplayerMode && !GetTree().Paused && !GameSaveManager.Instance.GetConfigValue("Backgrounder").AsBool() && !DisplayServer.WindowIsFocused())
			{
				OpenGamePauseDialog();
			}
		}
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		if (isGameRunning && !waitPause && !GetTree().Paused && inputEvent.IsActionPressed("Pause"))
		{
			GetViewport().SetInputAsHandled();
			if (!Global.IsMultiplayerMode)
			{
				OpenGamePauseDialog();
			}
			else if (!_isNetworkPaused)
			{
				_battleNetworkHost?.ApplyPause(paused: true);
				MultiPlayerManager.Instance.SendPause();
			}
		}
	}

	private void OpenGamePauseDialog()
	{
		AudioManager.Instance.AudioPlay("Pause", AudioManagerEnum.TYPE.SFX, 0.0, once: true, pauseAlive: true);
		DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("Pause");
		waitPause = true;
		dialogBoxBase.OnClose += () =>
		{
			waitPause = false;
		};
	}

	public override void _Input(InputEvent event_)
	{
		if (process != null)
		{
			process.InputProcess(event_);
		}
		if (event_.IsActionPressed("SpeedUp") && !(event_ is InputEventKey { Echo: not false }) && !Global.IsMultiplayerMode)
		{
			checkBox2X.ButtonPressed = !checkBox2X.ButtonPressed;
		}
		if (Input.IsActionJustPressed("PacketUIFront"))
		{
			GameSaveManager.Instance.SetConfigValue("PacketUIFront", !GameSaveManager.Instance.GetConfigValue("PacketUIFront").AsBool());
			BattleEventBus.Instance.EmitPacketUIFront(GameSaveManager.Instance.GetConfigValue("PacketUIFront").AsBool());
			GameSaveManager.Instance.SaveGameConfig();
		}
		if (Input.IsActionJustPressed("ShowPlantHealth"))
		{
			GameSaveManager.Instance.SetConfigValue("ShowPlantHealth", !GameSaveManager.Instance.GetConfigValue("ShowPlantHealth").AsBool());
			BattleEventBus.Instance.EmitShowPlantHealth(GameSaveManager.Instance.GetConfigValue("ShowPlantHealth").AsBool());
			GameSaveManager.Instance.SaveGameConfig();
		}
		if (Input.IsActionJustPressed("ShowZombieHealth"))
		{
			GameSaveManager.Instance.SetConfigValue("ShowZombieHealth", !GameSaveManager.Instance.GetConfigValue("ShowZombieHealth").AsBool());
			BattleEventBus.Instance.EmitShowZombieHealth(GameSaveManager.Instance.GetConfigValue("ShowZombieHealth").AsBool());
			GameSaveManager.Instance.SaveGameConfig();
		}
		if (Global.IsMultiplayerMode && GodotObject.IsInstanceValid(_playerStatusPanel))
		{
			if (Input.IsActionPressed("ShowPlayerStatus"))
			{
				_playerStatusPanel.Call("ShowPanel");
			}
			else
			{
				_playerStatusPanel.Call("HidePanel");
			}
		}
	}

	private async Task InvokeLifecycleWithProgress(Func<TowerDefenseBattleComponentBase, Task> fresh, Func<TowerDefenseBattleComponentBase, Task> fromProgress)
	{
		_progressInitialized.Clear();
		foreach (KeyValuePair<StringName, TowerDefenseBattleFeature> item in featureDictionary.ToList())
		{
			await InitializeProgressComponent(item.Value, _restoringProgress.TryGetFeatureSaveData(item.Key.ToString(), out var _), fresh, fromProgress);
		}
		if (process != null)
		{
			await InitializeProgressComponent(process, _restoringProgress.HasProcessSaveData, fresh, fromProgress);
		}
	}

	private async Task InitializeProgressComponent(TowerDefenseBattleComponentBase component, bool hasSection, Func<TowerDefenseBattleComponentBase, Task> fresh, Func<TowerDefenseBattleComponentBase, Task> fromProgress)
	{
		_ = 1;
		try
		{
			if (!hasSection || !component.CanLoadProgress())
			{
				await fresh(component);
				return;
			}
			_progressInitialized.Add(component);
			await fromProgress(component);
		}
		catch (Exception ex)
		{
			_restoringProgress.RestoreReport.Record("Initialization", component.GetType().Name + ": " + ex.Message);
		}
	}

	private async Task InvokeLifecycle(Func<TowerDefenseBattleComponentBase, Task> action)
	{
		foreach (TowerDefenseBattleFeature feature in featureDictionary.Values.ToList())
		{
			try
			{
				await action(feature);
			}
			catch (Exception value)
			{
				GD.PushError($"[InvokeLifecycle] 组件异常: {feature.GetType().Name}, {value}");
			}
		}
		if (process != null)
		{
			try
			{
				await action(process);
			}
			catch (Exception value2)
			{
				GD.PushError($"[InvokeLifecycle] Process 异常: {process.GetType().Name}, {value2}");
			}
		}
	}

	private async Task InvokeGameStartLifecycle(bool fromProgress)
	{
		IEnumerable<TowerDefenseBattleComponentBase> source = featureDictionary.Values.Cast<TowerDefenseBattleComponentBase>();
		if (process != null)
		{
			source = source.Append(process);
		}
		IEnumerable<TowerDefenseBattleComponentBase> enumerable = from item in source.Select((TowerDefenseBattleComponentBase component, int index) => new { component, index })
			orderby item.component.gameStartPriority, item.index
			select item.component;
		foreach (TowerDefenseBattleComponentBase item in enumerable)
		{
			await InvokeGameStartComponent(item, fromProgress && _progressInitialized.Contains(item));
		}
	}

	private async Task InvokeGameStartComponent(TowerDefenseBattleComponentBase component, bool fromProgress)
	{
		_ = 1;
		try
		{
			if (!fromProgress || !component.CanLoadProgress())
			{
				await component.GameStart();
			}
			else
			{
				await component.GameStartFromProgress();
			}
		}
		catch (Exception ex)
		{
			if (_restoringProgress != null)
			{
				_restoringProgress.RestoreReport.Record("Initialization", ex.Message);
				return;
			}
			GD.PushError($"[InvokeGameStartLifecycle] 组件异常: {component.GetType().Name}, {ex}");
		}
	}

	private async Task InvokeGameEntryLifecycle()
	{
		bool npcTalkRequiresPreSpawnBeforeTalk = NpcTalkRequiresPreSpawnBeforeTalk();
		IEnumerable<TowerDefenseBattleFeature> enumerable = from item in featureDictionary.Values.Select((TowerDefenseBattleFeature feature2, int index) => new
			{
				feature = feature2,
				index = index
			})
			orderby GetGameEntryFeaturePriority(item.feature, npcTalkRequiresPreSpawnBeforeTalk), item.index
			select item.feature;
		foreach (TowerDefenseBattleFeature feature in enumerable)
		{
			try
			{
				await feature.GameEntry();
			}
			catch (Exception value)
			{
				GD.PushError($"[InvokeGameEntryLifecycle] 组件异常: {feature.GetType().Name}, {value}");
			}
		}
		if (process != null)
		{
			try
			{
				await process.GameEntry();
			}
			catch (Exception value2)
			{
				GD.PushError($"[InvokeGameEntryLifecycle] Process 异常: {process.GetType().Name}, {value2}");
			}
		}
	}

	private bool NpcTalkRequiresPreSpawnBeforeTalk()
	{
		if (!featureDictionary.TryGetValue("NpcTalk", out var value))
		{
			return false;
		}
		if (value is TowerDefenseBattleFeatureNpcTalk towerDefenseBattleFeatureNpcTalk)
		{
			return towerDefenseBattleFeatureNpcTalk.ContainsEmbeddedTutorial();
		}
		return false;
	}

	private static int GetGameEntryFeaturePriority(TowerDefenseBattleFeature feature, bool npcTalkRequiresPreSpawnBeforeTalk)
	{
		if (!(feature is TowerDefenseBattleFeaturePreSpawn))
		{
			if (feature is TowerDefenseBattleFeatureNpcTalk)
			{
				return 20;
			}
			return 10;
		}
		if (npcTalkRequiresPreSpawnBeforeTalk)
		{
			return 0;
		}
		return 30;
	}

	public async void GameInitEntered()
	{
		SceneManager.Instance?.UpdateLoadingStatus("正在初始化关卡");
		try
		{
			if (GodotObject.IsInstanceValid(ResourceManager.Instance))
			{
				await ResourceManager.Instance.EnsureGameplayAtlasesReadyAsync();
			}
		}
		catch (Exception value)
		{
			GD.PushError($"[State] Gameplay atlas readiness failed: {value}");
			SceneManager.Instance?.UpdateLoadingStatus("战斗动画资源加载失败");
			return;
		}
		try
		{
			TowerDefenseLevelSaveConfigCSharp progressSave = null;
			if (Global.IsMultiplayerMode)
			{
				await XWModEnvironmentService.EnsureReadyAsync();
			}
			if (hasProgress && !GameSaveManager.Instance.TryGetLoadableLevelProgress(levelConfig.name, out progressSave, out var reason, ModLevelIdentity))
			{
				RejectProgressLoad(reason);
				return;
			}
			if (hasProgress)
			{
				SceneManager.Instance?.UpdateLoadingStatus("正在准备存档关卡");
				_restoringProgress = progressSave;
				PrepareSavedFeatures();
				await InvokeLifecycleWithProgress((TowerDefenseBattleComponentBase c) => c.GameInit(), (TowerDefenseBattleComponentBase c) => c.GameInitFromProgress());
				await ShowLoadingStatusForNextFrameAsync("正在读取存档");
				if (!progressSave.Load(resetReport: false))
				{
					RejectProgressLoad("当前关卡的基础结构无法初始化。");
					return;
				}
			}
			else
			{
				SceneManager.Instance?.UpdateLoadingStatus("正在加载关卡角色资源");
				await InvokeLifecycle((TowerDefenseBattleComponentBase c) => c.GameInit());
			}
		}
		catch (Exception ex)
		{
			GD.PushError($"[State] GameInitEntered 异常: {ex}");
			if (hasProgress)
			{
				RejectProgressLoad("存档恢复失败：" + ex.Message);
				return;
			}
		}
		if (!_levelConfigurationRejected)
		{
			SceneManager.Instance?.UpdateLoadingStatus("正在进入关卡");
			SceneManager.Instance?.CompleteSceneLoading();
			TrySendBattleStateEvent("ToGameEntry");
		}
	}

	private void RejectProgressLoad(string reason)
	{
		isGameRunning = false;
		SceneManager.Instance?.CompleteSceneLoading();
		GD.PushWarning("[ProgressLoad] " + reason);
		DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("DialogBoxTips");
		dialogBoxBase.Set("text", "无法恢复关卡进度，原存档已保留。\n" + reason.Replace("[", "[lb]"));
		dialogBoxBase.OnClose += () =>
		{
			string text;
			switch (Global.Instance.enterLevelMode)
			{
			case "ModLevel":
				text = "LevelChoose";
				break;
			case "LevelChoose":
				if (Global.Instance.currentLevelChoose != "TryLevel")
				{
					text = "LevelChoose";
					break;
				}
				goto default;
			case "DiyLevel":
			case "LoadLevel":
			case "OnlineLevel":
				text = "LevelEditorStage";
				break;
			default:
				text = "MainMenu";
				break;
			}
			string scene = text;
			SceneManager.Instance.ChangeScene(scene);
		};
	}

	private async Task ShowLoadingStatusForNextFrameAsync(string status)
	{
		SceneManager.Instance?.UpdateLoadingStatus(status);
		SceneTree tree = GetTree();
		if (GodotObject.IsInstanceValid(tree))
		{
			await ToSignal(tree, SceneTree.SignalName.ProcessFrame);
			await ToSignal(tree, SceneTree.SignalName.ProcessFrame);
		}
	}

	public async void GameEntryEntered()
	{
		isGameRunning = false;
		try
		{
			TaskCompletionSource<bool> gameEntryAckedTcs;
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost && MultiPlayerManager.Instance.GameEntrySent)
			{
				MultiPlayerManager.Instance.ResetGameEntryAck();
				if (!MultiPlayerManager.Instance.CheckAllGameEntryAcked())
				{
					gameEntryAckedTcs = new TaskCompletionSource<bool>();
					MultiPlayerManager.Instance.OnAllGameEntryAcked += OnAllGameEntryAckedHandler;
					try
					{
						await gameEntryAckedTcs.Task;
					}
					finally
					{
						MultiPlayerManager.Instance.OnAllGameEntryAcked -= OnAllGameEntryAckedHandler;
					}
				}
			}
			await InvokeGameEntryLifecycle();
			if (!isInit)
			{
				levelControl.awardCreate = false;
			}
			void OnAllGameEntryAckedHandler()
			{
				gameEntryAckedTcs.TrySetResult(result: true);
			}
		}
		catch (Exception value)
		{
			GD.PushError($"[State] GameEntryEntered 异常: {value}");
		}
		TrySendBattleStateEvent("ToGameReady");
	}

	public void GameEntryExited()
	{
	}

	public async void GameReadyEntered()
	{
		_ = 1;
		try
		{
			TaskCompletionSource<bool> tcs;
			if (Global.IsMultiplayerMode)
			{
				if (!_chooseReadyPeers.Contains(MultiPlayerManager.Instance.peerId))
				{
					_chooseReadyPeers.Add(MultiPlayerManager.Instance.peerId);
				}
				ShowChooseWaitLabel();
				MultiPlayerManager.Instance.SendChooseReady();
				MultiPlayerManager.Instance.ResetClientsReady();
				if (MultiPlayerManager.IsHost && !_chooseOverReceived)
				{
					bool flag = true;
					foreach (Variant battleAdmittedPeer in MultiPlayerManager.Instance.BattleAdmittedPeers)
					{
						if (!_chooseReadyPeers.Contains(battleAdmittedPeer.AsString()))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						MultiPlayerManager.Instance.SendChooseOver();
						DoChooseOver();
					}
				}
				if (!_chooseOverReceived)
				{
					tcs = new TaskCompletionSource<bool>();
					OnViewBack += ViewBackHandler;
					try
					{
						await tcs.Task;
					}
					finally
					{
						OnViewBack -= ViewBackHandler;
					}
				}
				HideChooseWaitLabel();
			}
			await InvokeLifecycle((TowerDefenseBattleComponentBase c) => c.GameReady());
			void ViewBackHandler()
			{
				tcs.TrySetResult(result: true);
			}
		}
		catch (Exception value)
		{
			GD.PushError($"[State] GameReadyEntered 异常: {value}");
		}
		TrySendBattleStateEvent("ToGameRunning");
	}

	public void GameReadyExited()
	{
	}

	public async void GameRunningEntered()
	{
		isGameRunning = true;
		isInit = false;
		_gameRunningEntryReady = false;
		try
		{
			bool flag = hasProgress;
			if (Global.IsMultiplayerMode)
			{
				_chooseReadyPeers.Clear();
				_chooseOverReceived = false;
			}
			if (levelConfig is TowerDefenseLevelConfig { finishMethod: TowerDefenseEnum.LEVEL_FINISH_METHOD.QUIZ })
			{
				isGameRunning = false;
			}
			if (flag)
			{
				_progressPause = DialogManager.Instance.DialogCreate("Pause");
				if (GodotObject.IsInstanceValid(_progressPause))
				{
					_progressPause.ProcessMode = ProcessModeEnum.Disabled;
				}
			}
			buttonPause.Visible = true;
			optionButton.Visible = true;
			if (Global.IsMultiplayerMode)
			{
				checkBox2X.Visible = false;
				if (checkBox2X.ButtonPressed)
				{
					checkBox2X.ButtonPressed = false;
				}
			}
			else
			{
				checkBox2X.Visible = true;
			}
			if (!flag)
			{
				ResumeEntryCharacters();
			}
			if (!flag)
			{
				await InvokeGameStartLifecycle(fromProgress: false);
			}
			else
			{
				await InvokeGameStartLifecycle(fromProgress: true);
				ResumeEntryCharacters();
				hasProgress = false;
			}
		}
		catch (Exception value)
		{
			GD.PushError($"[GameRunningEntered] 异常: {value}");
		}
		_gameRunningEntryReady = true;
		CompleteProgressRecovery();
	}

	public void GameRunningExited()
	{
		isGameRunning = false;
		_gameRunningEntryReady = false;
	}

	public void GameRunningProcessing(double delta)
	{
		if (!_gameRunningEntryReady)
		{
			return;
		}
		if (!TowerDefensePerfProfiler.Enabled)
		{
			ProcessZombieCheckArea();
			foreach (TowerDefenseBattleFeature value in featureDictionary.Values)
			{
				value.Process(delta);
			}
			process?.PhysicsProcess(delta);
			if (!Global.IsMultiplayerMode || !MultiPlayerManager.Instance.IsConnect())
			{
				return;
			}
			foreach (TowerDefenseBattleFeature value2 in featureDictionary.Values)
			{
				value2.SyncProcess(delta);
			}
			process?.SyncProcess(delta);
			_battleNetworkHost?.Process(delta);
			return;
		}
		long startTicks = TowerDefensePerfProfiler.Begin();
		ProcessZombieCheckArea();
		TowerDefensePerfProfiler.End("control.zombieCheckArea", startTicks, _zombieCheckCandidates.Count);
		long startTicks2 = TowerDefensePerfProfiler.Begin();
		foreach (TowerDefenseBattleFeature value3 in featureDictionary.Values)
		{
			long startTicks3 = TowerDefensePerfProfiler.Begin();
			value3.Process(delta);
			TowerDefensePerfProfiler.End("control.featureProcess." + value3.GetType().Name, startTicks3);
		}
		TowerDefensePerfProfiler.End("control.featureProcess", startTicks2, featureDictionary.Count);
		if (process != null)
		{
			long startTicks4 = TowerDefensePerfProfiler.Begin();
			process.PhysicsProcess(delta);
			TowerDefensePerfProfiler.End("control.processPhysics." + process.GetType().Name, startTicks4);
			TowerDefensePerfProfiler.End("control.processPhysics", startTicks4, 1);
		}
		if (!Global.IsMultiplayerMode || !MultiPlayerManager.Instance.IsConnect())
		{
			return;
		}
		long startTicks5 = TowerDefensePerfProfiler.Begin();
		foreach (TowerDefenseBattleFeature value4 in featureDictionary.Values)
		{
			long startTicks6 = TowerDefensePerfProfiler.Begin();
			value4.SyncProcess(delta);
			TowerDefensePerfProfiler.End("control.featureSyncProcess." + value4.GetType().Name, startTicks6);
		}
		TowerDefensePerfProfiler.End("control.featureSyncProcess", startTicks5, featureDictionary.Count);
		if (process != null)
		{
			long startTicks7 = TowerDefensePerfProfiler.Begin();
			process.SyncProcess(delta);
			TowerDefensePerfProfiler.End("control.processSync." + process.GetType().Name, startTicks7);
			TowerDefensePerfProfiler.End("control.processSync", startTicks7, 1);
		}
		if (_battleNetworkHost != null)
		{
			long startTicks8 = TowerDefensePerfProfiler.Begin();
			_battleNetworkHost.Process(delta);
			TowerDefensePerfProfiler.End("control.networkHostProcess", startTicks8);
		}
	}

	private void ResumeEntryCharacters()
	{
		foreach (Variant item in TowerDefenseManager.Instance.GetCharacter())
		{
			if (item.AsGodotObject() is TowerDefenseCharacter towerDefenseCharacter)
			{
				towerDefenseCharacter.ProcessMode = ProcessModeEnum.Inherit;
				IStateMachineController stateMachine = towerDefenseCharacter.StateMachine;
				if (stateMachine != null && stateMachine.IsInitialized)
				{
					towerDefenseCharacter.SetMainStateMachineDispatchEnabled(enabled: true);
				}
			}
		}
		foreach (Node item2 in GetTree().GetNodesInGroup("Gravestone"))
		{
			if (item2 is TowerDefenseGravestone { inGame: not false, isDestroy: false } towerDefenseGravestone)
			{
				towerDefenseGravestone.ProcessMode = ProcessModeEnum.Inherit;
				IStateMachineController stateMachine2 = towerDefenseGravestone.StateMachine;
				if (stateMachine2 != null && stateMachine2.IsInitialized)
				{
					towerDefenseGravestone.SetMainStateMachineDispatchEnabled(enabled: true);
				}
			}
		}
		foreach (Variant item3 in TowerDefenseManager.Instance.GetZombie())
		{
			if (item3.AsGodotObject() is TowerDefenseZombie towerDefenseZombie)
			{
				towerDefenseZombie.Walk();
			}
		}
	}

	public void GameFailEntered()
	{
		isGameFail = true;
		isGameRunning = false;
		foreach (TowerDefenseBattleFeature value in featureDictionary.Values)
		{
			value.GameFail();
		}
		process?.GameFail(failCharacter);
	}

	private void OnSceneChange(string sceneName)
	{
		_preserveLevelConfigOnExit = sceneName == "TowerDefense" || (sceneName == "LevelEditorStage" && Global.IsEditor && Global.Instance?.enterLevelMode == "DiyLevel");
		if (GodotObject.IsInstanceValid(this))
		{
			_battleShuttingDown = true;
			CancelBattleLifetimes();
			isGameRunning = false;
			ProcessMode = ProcessModeEnum.Disabled;
		}
	}

	public void TriggerGameFail(TowerDefenseCharacter enterCharacter)
	{
		failCharacter = enterCharacter;
		TrySendBattleStateEvent("ToGameFail");
	}

	public void TriggerGameEntry()
	{
		TrySendBattleStateEvent("ToGameEntry");
	}

	public async Task ReadySetPlantPlay()
	{
		if (levelControl != null)
		{
			await levelControl.ReadySetPlantPlay();
		}
	}

	public void TipsPlay(string text, double duration = 2.0)
	{
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			MultiPlayerManager.Instance.SendTipsPlay(text, duration);
		}
		if (levelControl != null)
		{
			levelControl.TipsPlay(text, duration);
		}
	}

	public void ViewMap()
	{
		process?.ViewMap();
	}

	public void GameFail(TowerDefenseCharacter enterCharacter)
	{
		failCharacter = enterCharacter;
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			MultiPlayerManager.Instance.SendGameResult(victory: false);
		}
		foreach (Variant item in TowerDefenseManager.Instance.GetCharacter())
		{
			if (item.AsGodotObject() is TowerDefenseCharacter towerDefenseCharacter && towerDefenseCharacter != enterCharacter)
			{
				towerDefenseCharacter.ProcessMode = ProcessModeEnum.Disabled;
			}
		}
		TrySendBattleStateEvent("ToGameFail");
	}

	public void GameEntry()
	{
		TriggerGameEntry();
	}

	private void ProcessZombieCheckArea()
	{
		if (!GodotObject.IsInstanceValid(zombieCheckArea) || zombieCheckArea.ProcessMode == ProcessModeEnum.Disabled)
		{
			_zombieCheckOverlaps.Clear();
		}
		else
		{
			if (TowerDefenseManager.Instance == null || TowerDefenseManager.Instance.characterRegistry == null)
			{
				return;
			}
			Rect2 rect = AabbShapeUtil.ComputeAreaWorldRect(zombieCheckArea);
			TowerDefenseManager.Instance.characterRegistry.FillCharactersForRectGridWindowList(rect, _zombieCheckCandidates);
			_zombieCheckScratch.Clear();
			for (int i = 0; i < _zombieCheckCandidates.Count; i++)
			{
				if (_zombieCheckCandidates[i] is TowerDefenseZombie towerDefenseZombie && AabbShapeUtil.Intersects(rect, towerDefenseZombie.WorldHitRect))
				{
					_zombieCheckScratch.Add(towerDefenseZombie);
					if (!_zombieCheckOverlaps.Contains(towerDefenseZombie))
					{
						HandleZombieEnterHouse(towerDefenseZombie);
					}
				}
			}
			_zombieCheckOverlaps.RemoveWhere((TowerDefenseZombie zombie) => !GodotObject.IsInstanceValid(zombie) || !_zombieCheckScratch.Contains(zombie));
			foreach (TowerDefenseZombie item in _zombieCheckScratch)
			{
				_zombieCheckOverlaps.Add(item);
			}
		}
	}

	private void HandleZombieEnterHouse(TowerDefenseZombie zombie)
	{
		if (!GodotObject.IsInstanceValid(zombie) || zombie.instance.die || zombie.instance.nearDie || zombie.instance.hypnoses || zombie.Scale.X < 0f)
		{
			return;
		}
		foreach (TowerDefenseBattleFeature value in featureDictionary.Values)
		{
			value.ZombieEnterHouse(zombie);
		}
		process?.ZombieEnterHouse(zombie);
	}

	public void UISwitched(bool shown)
	{
		if (shown)
		{
			bankUILayer.Layer = 3;
			mobileInterval.CustomMinimumSize = new Vector2(400f, mobileInterval.CustomMinimumSize.Y);
		}
		else
		{
			bankUILayer.Layer = ((!GameSaveManager.Instance.GetConfigValue("PacketUIFront").AsBool()) ? 1 : 3);
			mobileInterval.CustomMinimumSize = new Vector2(0f, mobileInterval.CustomMinimumSize.Y);
		}
	}

	public void PacketUIFront(bool open)
	{
		if (open)
		{
			bankUILayer.Layer = 3;
		}
		else
		{
			bankUILayer.Layer = ((!GameSaveManager.Instance.GetConfigValue("MobilePreset").AsBool()) ? 1 : 3);
		}
	}

	public bool ChangeCostAdd(TowerDefensePacketChangeCost changeCost)
	{
		if (changeCost.key != "")
		{
			foreach (TowerDefensePacketChangeCost changeCost2 in changeCostList)
			{
				if (changeCost2.key == changeCost.key)
				{
					return false;
				}
			}
		}
		changeCostList.Add(changeCost);
		return true;
	}

	public bool ChangeCostRemove(TowerDefensePacketChangeCost changeCost)
	{
		int num = changeCostList.IndexOf(changeCost);
		if (num == -1)
		{
			return false;
		}
		changeCostList.RemoveAt(num);
		return true;
	}

	private void InitPlayerStatusPanel()
	{
		CanvasLayer nodeOrNull = GetNodeOrNull<CanvasLayer>("GUITop");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			PackedScene packedScene = GD.Load<PackedScene>("uid://dg77jwfvccr4t");
			_playerStatusPanel = packedScene.Instantiate<PlayerStatusPanel>(PackedScene.GenEditState.Disabled);
			nodeOrNull.AddChild(_playerStatusPanel, forceReadableName: false, InternalMode.Disabled);
		}
	}

	public int GetNextSyncId()
	{
		if (_battleNetworkHost != null)
		{
			return _battleNetworkHost.Entities.NextCharacterSyncId();
		}
		_syncIdCounter++;
		return _syncIdCounter;
	}

	public int GetNextPacketSyncId()
	{
		if (_battleNetworkHost != null)
		{
			return _battleNetworkHost.Entities.NextPacketSyncId();
		}
		_packetSyncIdCounter++;
		return _packetSyncIdCounter;
	}

	public void RegisterSyncPacket(int syncId, TowerDefenseInGamePacketShow packet)
	{
		_syncPackets[syncId] = packet;
		_battleNetworkHost?.Entities.RegisterPacket(syncId, packet);
	}

	public void UnregisterSyncPacket(int syncId)
	{
		_syncPackets.Remove(syncId);
		_battleNetworkHost?.Entities.UnregisterPacket(syncId);
	}

	internal bool TryGetSyncPacket(int syncId, out TowerDefenseInGamePacketShow packet)
	{
		if (_syncPackets.TryGetValue(syncId, out packet) && GodotObject.IsInstanceValid(packet))
		{
			return true;
		}
		packet = null;
		UnregisterSyncPacket(syncId);
		return false;
	}

	internal void RemoveCharacterSyncState(int syncId)
	{
		_syncCharacters.Remove(syncId);
		_characterLastSyncState.Remove(syncId);
		string value = syncId + ":";
		List<Variant> list = new List<Variant>();
		foreach (Variant key in _componentLastSyncState.Keys)
		{
			if (key.AsString().StartsWith(value, StringComparison.Ordinal))
			{
				list.Add(key);
			}
		}
		foreach (Variant item in list)
		{
			_componentLastSyncState.Remove(item);
		}
		_battleNetworkHost?.Entities.UnregisterCharacter(syncId);
	}

	internal void RemoveZombieSyncState(int syncId)
	{
		_zombieLastSyncState.Remove(syncId);
		_zombieTargetPositions.Remove(syncId);
		_zombieSyncVelocities.Remove(syncId);
		_zombieLastSyncTime.Remove(syncId);
		_zombieSyncMissCount.Remove(syncId);
	}

	public void RegisterSyncCharacter(int syncIdVal, TowerDefenseCharacter character)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, TowerDefenseCharacter> syncCharacter in _syncCharacters)
		{
			if (syncCharacter.Key != syncIdVal && syncCharacter.Value == character)
			{
				list.Add(syncCharacter.Key);
			}
		}
		foreach (int item in list)
		{
			RemoveCharacterSyncState(item);
			RemoveZombieSyncState(item);
		}
		if (_syncCharacters.TryGetValue(syncIdVal, out var value) && value != character)
		{
			_battleNetworkHost?.CharacterState.DiscardPendingOwnerState(syncIdVal);
			if (GodotObject.IsInstanceValid(value))
			{
				value.OnDestroy -= OnSyncCharacterDestroy;
				CleanupCharacterCell(value);
			}
			RemoveCharacterSyncState(syncIdVal);
			RemoveZombieSyncState(syncIdVal);
			if (GodotObject.IsInstanceValid(value) && !value.isDestroy)
			{
				if (value.IsNodeReady())
				{
					DestroyComponent destroyComponent = value.destroyComponent;
					if (destroyComponent != null && !destroyComponent.IsReleased)
					{
						value.destroyComponent.isRemoteDestroy = true;
						value.Destroy();
						goto IL_018a;
					}
				}
				value.skipDestroySet = true;
				if (!value.IsQueuedForDeletion())
				{
					value.QueueFree();
				}
			}
		}
		goto IL_018a;
		IL_018a:
		character.syncId = syncIdVal;
		_syncCharacters[syncIdVal] = character;
		_battleNetworkHost?.Entities.RegisterCharacter(syncIdVal, character);
		_battleNetworkHost?.CharacterState.BindPendingDestroyTarget(syncIdVal, character);
		_battleNetworkHost?.Events.BindPendingCharacterGeneration(syncIdVal, character);
		character.OnDestroy -= OnSyncCharacterDestroy;
		character.OnDestroy += OnSyncCharacterDestroy;
		Dictionary pendingData;
		if (_pendingDestroySyncIds.ContainsKey(syncIdVal))
		{
			pendingData = _pendingDestroySyncIds[syncIdVal].AsGodotDictionary();
			_pendingDestroySyncIds.Remove(syncIdVal);
			if (character.IsNodeReady())
			{
				ApplyPendingDestroyToReadyCharacter(syncIdVal, character, pendingData);
			}
			else
			{
				character.Ready += ApplyWhenReady;
			}
		}
		else if (Global.IsMultiplayerMode && MultiPlayerManager.Instance.isHost)
		{
			CallDeferred("SyncCharacterInitTimer", syncIdVal);
			CallDeferred("SyncCharacterPositionTimer", syncIdVal);
		}
		void ApplyWhenReady()
		{
			character.Ready -= ApplyWhenReady;
			ApplyPendingDestroyToReadyCharacter(syncIdVal, character, pendingData);
		}
	}

	private void ApplyPendingDestroyToReadyCharacter(int syncIdVal, TowerDefenseCharacter character, Dictionary pendingData)
	{
		if (pendingData == null || !GodotObject.IsInstanceValid(character) || character.isDestroy || !character.IsNodeReady() || !_syncCharacters.TryGetValue(syncIdVal, out var value) || value != character)
		{
			return;
		}
		character.OnDestroy -= OnSyncCharacterDestroy;
		RemoveCharacterSyncState(syncIdVal);
		RemoveZombieSyncState(syncIdVal);
		CleanupCharacterCell(character);
		bool flag = pendingData.GetValueOrDefault("is_explode", false).AsBool();
		bool isSmash = pendingData.GetValueOrDefault("is_smash", false).AsBool();
		if (flag && GodotObject.IsInstanceValid(character.instance) && character.instance.ashScene != null && !character.inWater)
		{
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(character.instance.ashScene, character.gridPos, "Idle");
			Node2D node2D = TowerDefenseManager.GetCharacterNode();
			towerDefenseEffectSpriteOnce.GlobalPosition = character.GetLogicalGlobalPosition(character.sprite);
			towerDefenseEffectSpriteOnce.Scale = character.Scale * character.transformPoint.Scale;
			node2D.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
			towerDefenseEffectSpriteOnce.ZIndex -= 6;
		}
		character.isExplode = flag;
		character.isSmash = isSmash;
		DestroyComponent destroyComponent = character.destroyComponent;
		if (destroyComponent != null && !destroyComponent.IsReleased)
		{
			character.destroyComponent.isRemoteDestroy = true;
			character.Destroy();
			return;
		}
		character.skipDestroySet = true;
		if (!character.IsQueuedForDeletion())
		{
			character.QueueFree();
		}
	}

	internal void DetachSyncCharacter(TowerDefenseCharacter character)
	{
		if (GodotObject.IsInstanceValid(character) && character.syncId >= 0)
		{
			_syncCharacters.Remove(character.syncId);
			_battleNetworkHost?.Entities.UnregisterCharacter(character.syncId);
			RemoveZombieSyncState(character.syncId);
			character.OnDestroy -= OnSyncCharacterDestroy;
		}
	}

	private async void SyncCharacterInitTimer(int syncIdVal)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		if (!_syncCharacters.ContainsKey(syncIdVal))
		{
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter = _syncCharacters[syncIdVal];
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter2 = towerDefenseCharacter;
		if (towerDefenseCharacter2 == null || towerDefenseCharacter2.isDestroy)
		{
			return;
		}
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter2.sprite))
		{
			await ToSignal(towerDefenseCharacter2, Node.SignalName.Ready);
		}
		if (!_syncCharacters.ContainsKey(syncIdVal))
		{
			return;
		}
		towerDefenseCharacter = _syncCharacters[syncIdVal];
		if (GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			towerDefenseCharacter2 = towerDefenseCharacter;
			if (towerDefenseCharacter2 != null && !towerDefenseCharacter2.isDestroy)
			{
				_battleNetworkHost?.CharacterState?.SendInitialState(syncIdVal);
			}
		}
	}

	private async void SyncCharacterPositionTimer(int syncIdVal)
	{
		await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);
		if (!_syncCharacters.ContainsKey(syncIdVal))
		{
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter = _syncCharacters[syncIdVal];
		if (GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			TowerDefenseCharacter towerDefenseCharacter2 = towerDefenseCharacter;
			if (towerDefenseCharacter2 != null && !towerDefenseCharacter2.isDestroy && !(towerDefenseCharacter2 is TowerDefenseZombie))
			{
				_battleNetworkHost?.CharacterState?.SendPosition(syncIdVal);
			}
		}
	}

	private void OnSyncCharacterDestroy(TowerDefenseCharacter character)
	{
		if (character.syncId >= 0 && _syncCharacters.TryGetValue(character.syncId, out var value) && value == character)
		{
			RemoveCharacterSyncState(character.syncId);
			RemoveZombieSyncState(character.syncId);
			if (Global.IsMultiplayerMode && MultiPlayerManager.Instance.isHost)
			{
				_battleNetworkHost?.CharacterState?.SendDestroy(character);
			}
		}
	}

	public void CleanupCharacterCell(TowerDefenseCharacter character)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return;
		}
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(character.gridPos);
		if (GodotObject.IsInstanceValid(mapCell) && mapCell.characterList.Contains(character))
		{
			character.OnDestroy -= mapCell.CharacterDestroy;
			mapCell.CharacterDestroy(character);
		}
		if (!GodotObject.IsInstanceValid(character.config) || !(character.config is TowerDefensePlantConfig towerDefensePlantConfig))
		{
			return;
		}
		foreach (Vector2I item in towerDefensePlantConfig.extendGrid)
		{
			TowerDefenseCellInstance mapCell2 = TowerDefenseManager.GetMapCell(character.gridPos + item);
			if (GodotObject.IsInstanceValid(mapCell2) && mapCell2 != mapCell && mapCell2.characterList.Contains(character))
			{
				character.OnDestroy -= mapCell2.CharacterDestroy;
				mapCell2.CharacterDestroy(character);
			}
		}
	}

	private void ApplyNetworkPauseUi()
	{
		if (!_isNetworkPaused)
		{
			_isNetworkPaused = true;
			AudioManager.Instance.AudioPlay("Pause", AudioManagerEnum.TYPE.SFX, 0.0, once: true, pauseAlive: true);
			DialogManager.Instance.DialogCreate("BattlePause");
			if (GodotObject.IsInstanceValid(buttonPause))
			{
				buttonPause.ButtonPressed = true;
			}
		}
	}

	private void ApplyNetworkResumeUi()
	{
		if (!_isNetworkPaused)
		{
			return;
		}
		_isNetworkPaused = false;
		CanvasLayer canvasLayer = (CanvasLayer)(GodotObject)DialogManager.Instance.Get("_dialogLayer");
		if (GodotObject.IsInstanceValid(canvasLayer))
		{
			foreach (Node child in canvasLayer.GetChildren())
			{
				if (child is CommandConsole commandConsole)
				{
					commandConsole.Close();
					return;
				}
			}
		}
		if (GodotObject.IsInstanceValid(buttonPause))
		{
			buttonPause.ButtonPressed = false;
		}
	}

	public void ApplyNetworkPauseFromSession(bool paused)
	{
		if (paused)
		{
			ApplyNetworkPauseUi();
		}
		else
		{
			ApplyNetworkResumeUi();
		}
	}

	private RemoteCursor GetOrCreateRemoteCursor(string userId)
	{
		if (_remoteCursors.ContainsKey(userId))
		{
			RemoteCursor remoteCursor = _remoteCursors[userId];
			if (GodotObject.IsInstanceValid(remoteCursor))
			{
				return remoteCursor;
			}
			_remoteCursors.Remove(userId);
		}
		CanvasLayer nodeOrNull = GetNodeOrNull<CanvasLayer>("GUITop");
		if (!GodotObject.IsInstanceValid(nodeOrNull))
		{
			return null;
		}
		RemoteCursor remoteCursor2 = GD.Load<PackedScene>("res://Scene/TowerDefesne/TowerDefenseNew/RemoteCursor.tscn").Instantiate<RemoteCursor>(PackedScene.GenEditState.Disabled);
		nodeOrNull.AddChild(remoteCursor2, forceReadableName: false, InternalMode.Disabled);
		int num = MultiPlayerManager.Instance.BattleAdmittedPeers.IndexOf(userId);
		if (num < 0)
		{
			num = _remoteCursors.Count;
		}
		string peerName = MultiPlayerManager.Instance.GetPeerName(userId);
		remoteCursor2.SetPlayerInfo(num, peerName);
		_remoteCursors[userId] = remoteCursor2;
		return remoteCursor2;
	}

	internal void RemoveRemoteCursor(string userId)
	{
		if (_remoteCursors.ContainsKey(userId))
		{
			RemoteCursor remoteCursor = _remoteCursors[userId];
			_remoteCursors.Remove(userId);
			if (GodotObject.IsInstanceValid(remoteCursor))
			{
				remoteCursor.QueueFree();
			}
		}
	}

	internal void ApplyRemoteCursorPosition(string userId, Vector2 position)
	{
		RemoteCursor orCreateRemoteCursor = GetOrCreateRemoteCursor(userId);
		if (GodotObject.IsInstanceValid(orCreateRemoteCursor))
		{
			orCreateRemoteCursor.UpdatePosition(position.X, position.Y);
		}
	}

	internal void ApplyRemoteCursorPick(string userId, string pickType, string pickName)
	{
		RemoteCursor orCreateRemoteCursor = GetOrCreateRemoteCursor(userId);
		if (GodotObject.IsInstanceValid(orCreateRemoteCursor))
		{
			orCreateRemoteCursor.UpdatePick(pickType, pickName);
		}
	}

	internal void OnChooseReady(Dictionary data)
	{
		string text = data.GetValueOrDefault("user_id", "").AsString();
		if (!MultiPlayerManager.Instance.IsBattleParticipant(text) || _chooseReadyPeers.Contains(text))
		{
			return;
		}
		_chooseReadyPeers.Add(text);
		UpdateChooseWaitLabel();
		if (!MultiPlayerManager.Instance.isHost)
		{
			return;
		}
		if (_chooseReadyPeers.Contains(MultiPlayerManager.Instance.peerId))
		{
			bool flag = true;
			foreach (Variant battleAdmittedPeer in MultiPlayerManager.Instance.BattleAdmittedPeers)
			{
				if (!_chooseReadyPeers.Contains(battleAdmittedPeer.AsString()))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				MultiPlayerManager.Instance.SendChooseOver();
				DoChooseOver();
			}
		}
		else if (_chooseReadyPeers.Count >= MultiPlayerManager.Instance.BattleAdmittedPeers.Count)
		{
			MultiPlayerManager.Instance.SendChooseOver();
			DoChooseOver();
		}
	}

	internal void OnChooseOver()
	{
		if (!_chooseReadyPeers.Contains(MultiPlayerManager.Instance.peerId))
		{
			_chooseReadyPeers.Add(MultiPlayerManager.Instance.peerId);
		}
		DoChooseOver();
	}

	private void DoChooseOver()
	{
		_chooseOverReceived = true;
		EmitViewBack();
	}

	private void ShowChooseWaitLabel()
	{
		CanvasLayer nodeOrNull = GetNodeOrNull<CanvasLayer>("GUITop");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			_chooseWaitLabel = new RichTextLabel();
			_chooseWaitLabel.BbcodeEnabled = true;
			_chooseWaitLabel.FitContent = true;
			_chooseWaitLabel.ZIndex = 3000;
			_chooseWaitLabel.ZAsRelative = false;
			_chooseWaitLabel.MouseFilter = Control.MouseFilterEnum.Ignore;
			_chooseWaitLabel.AddThemeFontSizeOverride("normal_font_size", 22);
			_chooseWaitLabel.Position = new Vector2(GetViewport().GetVisibleRect().Size.X / 2f - 150f, 80f);
			_chooseWaitLabel.Size = new Vector2(300f, 200f);
			nodeOrNull.AddChild(_chooseWaitLabel, forceReadableName: false, InternalMode.Disabled);
			UpdateChooseWaitLabel();
		}
	}

	private void HideChooseWaitLabel()
	{
		if (GodotObject.IsInstanceValid(_chooseWaitLabel))
		{
			_chooseWaitLabel.QueueFree();
		}
		_chooseWaitLabel = null;
	}

	private void UpdateChooseWaitLabel()
	{
		if (!GodotObject.IsInstanceValid(_chooseWaitLabel))
		{
			return;
		}
		string text = "[center]";
		bool flag = false;
		for (int i = 0; i < MultiPlayerManager.Instance.BattleAdmittedPeers.Count; i++)
		{
			string text2 = MultiPlayerManager.Instance.BattleAdmittedPeers[i].AsString();
			if (!_chooseReadyPeers.Contains(text2))
			{
				Color color = PlayerColors[i % PlayerColors.Length];
				string value = color.ToHtml(includeAlpha: false);
				string peerName = MultiPlayerManager.Instance.GetPeerName(text2);
				text += $"[color=#{value}]等待{peerName}选卡[/color]\n";
				flag = true;
			}
		}
		if (!flag)
		{
			text += "所有玩家已就绪";
		}
		text += "[/center]";
		_chooseWaitLabel.Text = text;
	}

	public void RejectLevelConfiguration(string reason)
	{
		if (_levelConfigurationRejected)
		{
			return;
		}
		_levelConfigurationRejected = true;
		isGameRunning = false;
		GD.PushWarning("[LevelConfiguration] " + reason);
		Callable.From(() =>
		{
			if (IsInsideTree())
			{
				AbortBattleGraphInitialization();
				SceneManager.Instance?.CompleteSceneLoading();
				DialogBoxBase dialogBoxBase = DialogManager.Instance.DialogCreate("DialogBoxTips");
				dialogBoxBase.Set("text", "关卡配置不可用，未记录通关或失败。\n" + reason.Replace("[", "[lb]"));
				dialogBoxBase.OnClose += ReturnFromRejectedLevel;
			}
		}).CallDeferred();
	}

	private void ReturnFromRejectedLevel()
	{
		string text;
		switch (Global.Instance.enterLevelMode)
		{
		case "ModLevel":
			text = "LevelChoose";
			break;
		case "LevelChoose":
			if (Global.Instance.currentLevelChoose != "TryLevel")
			{
				text = "LevelChoose";
				break;
			}
			goto default;
		case "DiyLevel":
		case "LoadLevel":
		case "OnlineLevel":
			text = "LevelEditorStage";
			break;
		default:
			text = "MainMenu";
			break;
		}
		string scene = text;
		SceneManager.Instance.ChangeScene(scene);
	}

	private void PrepareSavedFeatures()
	{
		foreach (KeyValuePair<Variant, Variant> item in _restoringProgress.featureSave ?? new Dictionary())
		{
			Variant.Type variantType = item.Key.VariantType;
			if ((variantType != Variant.Type.String && variantType != Variant.Type.StringName) || 1 == 0)
			{
				continue;
			}
			StringName name = item.Key.AsStringName();
			if (featureDictionary.ContainsKey(name))
			{
				continue;
			}
			_restoringProgress.RestoreReport.Try("Feature", () =>
			{
				FeatureAddTransaction transaction = new FeatureAddTransaction();
				if (!TryAddFeature(name, GetConfiguredFeatureData(name), transaction, out var failureReason))
				{
					RollbackFeatureAddTransaction(transaction);
					_restoringProgress.RestoreReport.Record("Feature", failureReason);
				}
			});
		}
	}

	private void CompleteProgressRecovery()
	{
		if (_restoringProgress == null || _progressNoticeShown)
		{
			return;
		}
		_progressNoticeShown = true;
		GameSaveManager.ProgressRecoverySession progressRecovery = GameSaveManager.Instance.GetProgressRecovery(this);
		if (progressRecovery == null)
		{
			return;
		}
		if (!_restoringProgress.RestoreReport.HasIssues && !progressRecovery.UsedBackup && progressRecovery.BackupAvailable)
		{
			GameSaveManager.Instance.AcceptProgressRecovery(this);
			if (GodotObject.IsInstanceValid(_progressPause))
			{
				_progressPause.ProcessMode = ProcessModeEnum.Always;
			}
		}
		else
		{
			ShowProgressRecoveryNotice("");
		}
	}

	private void ShowProgressRecoveryNotice(string failure)
	{
		if (_battleShuttingDown || !IsInsideTree())
		{
			return;
		}
		GameSaveManager.ProgressRecoverySession session = GameSaveManager.Instance.GetProgressRecovery(this);
		if (session == null)
		{
			return;
		}
		DialogBoxChoose dialogBoxChoose = DialogManager.Instance.DialogCreate("DialogBoxChoose") as DialogBoxChoose;
		if (!GodotObject.IsInstanceValid(dialogBoxChoose))
		{
			GD.PushWarning("[ProgressLoad] 恢复提示无法创建，原始备份继续保留。");
			GameSaveManager.Instance.AcceptProgressRecovery(this);
			if (GodotObject.IsInstanceValid(_progressPause))
			{
				_progressPause.ProcessMode = ProcessModeEnum.Always;
			}
			return;
		}
		if (GodotObject.IsInstanceValid(_progressPause))
		{
			_progressPause.SetProcessInput(enable: false);
		}
		dialogBoxChoose.text = "[center]关卡进度已恢复[/center]\n" + (session.UsedBackup ? "主存档无法读取，已从备份续档。\n" : "") + (_restoringProgress.RestoreReport.HasIssues ? "部分内容无法恢复，已跳过或使用当前默认状态。\n" : "") + (session.BackupAvailable ? "可以继续游戏，或退出关卡并恢复备份。" : "备份未能建立，原存档暂不会被覆盖。可以继续游戏，或退出并保留原档。") + (string.IsNullOrEmpty(failure) ? "" : ("\n恢复备份失败，可重试：" + failure.Replace("[", "[lb]")));
		dialogBoxChoose.GetNode<BaseButton>("%TrueButton").Set("text", "继续游戏");
		dialogBoxChoose.GetNode<BaseButton>("%FalseButton").Set("text", session.BackupAvailable ? "退出并恢复" : "退出关卡");
		bool exit = false;
		string retryReason = null;
		dialogBoxChoose.OnChooseFalse += () =>
		{
			if (session.BackupAvailable && !GameSaveManager.Instance.TryRestoreProgressBackup(this, out var reason))
			{
				retryReason = reason;
			}
			else
			{
				GameSaveManager.Instance.DiscardProgressRecovery(this);
				exit = true;
			}
		};
		_003C_003Ec__DisplayClass204_0 CS_0024_003C_003E8__locals0;
		dialogBoxChoose.OnClose += () =>
		{
			if (!_battleShuttingDown)
			{
				if (retryReason != null)
				{
					Callable.From(() =>
					{
						ShowProgressRecoveryNotice((string)(object)CS_0024_003C_003E8__locals0);
					}).CallDeferred();
				}
				else if (exit)
				{
					isGameRunning = false;
					SceneManager.Instance.ChangeScene(ProgressExitDestination());
				}
				else
				{
					GameSaveManager.Instance.AcceptProgressRecovery(this);
					if (GodotObject.IsInstanceValid(_progressPause) && !_progressPause.IsQueuedForDeletion())
					{
						_progressPause.CloseDialog();
					}
				}
			}
		};
	}

	private static string ProgressExitDestination()
	{
		switch (Global.Instance.enterLevelMode)
		{
		case "ModLevel":
			return "LevelChoose";
		case "LevelChoose":
			if (Global.Instance.currentLevelChoose != "TryLevel")
			{
				return "LevelChoose";
			}
			break;
		case "DiyLevel":
		case "LoadLevel":
		case "OnlineLevel":
			return "LevelEditorStage";
		}
		return "MainMenu";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(96)
		{
			new MethodInfo(MethodName.EmitViewBack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginPendingBattleOperation, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsPendingBattleOperationCurrent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "operationId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CompletePendingBattleOperation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "operationId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelBattleLifetimes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrySendBattleStateEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendStateEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_levelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OldLevelInit, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_levelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.NewLevelInit, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_levelConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddFeature, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddProcessDependenceFeature, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsFeatureConfigured, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroyFeatureSafely, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "feature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "context", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroyProcessSafely, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "targetProcess", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "context", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroyAllFeaturesSafely, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "context", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AbortBattleGraphInitialization, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetConfiguredFeatureData, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildMissingDependenceFeatureData, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ZombieWonLevelFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "playAnime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFeature, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveFeature, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ComponentDependsOnFeature, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "component", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ContainsFeatureName, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "featureNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "featureName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProcess, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "processName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasProcessName, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "processName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddUIToTopBankContainer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddUIToTopPropContainer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._UnhandledInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenGamePauseDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.NpcTalkRequiresPreSpawnBeforeTalk, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGameEntryFeaturePriority, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "feature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "npcTalkRequiresPreSpawnBeforeTalk", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GameInitEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RejectProgressLoad, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GameEntryEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameEntryExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameReadyEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameReadyExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameRunningEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameRunningExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameRunningProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResumeEntryCharacters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameFailEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSceneChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sceneName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TriggerGameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "enterCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.TriggerGameEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TipsPlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ViewMap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GameFail, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "enterCharacter", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GameEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessZombieCheckArea, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleZombieEnterHouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.UISwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "shown", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PacketUIFront, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChangeCostAdd, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "changeCost", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ChangeCostRemove, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "changeCost", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.InitPlayerStatusPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetNextSyncId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetNextPacketSyncId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterSyncPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.UnregisterSyncPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveCharacterSyncState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveZombieSyncState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterSyncCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncIdVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPendingDestroyToReadyCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncIdVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "pendingData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DetachSyncCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SyncCharacterInitTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncIdVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SyncCharacterPositionTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncIdVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSyncCharacterDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CleanupCharacterCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyNetworkPauseUi, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyNetworkResumeUi, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyNetworkPauseFromSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "paused", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetOrCreateRemoteCursor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "userId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveRemoteCursor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "userId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRemoteCursorPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "userId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRemoteCursorPick, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "userId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "pickType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "pickName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnChooseReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnChooseOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoChooseOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowChooseWaitLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideChooseWaitLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateChooseWaitLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RejectLevelConfiguration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReturnFromRejectedLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareSavedFeatures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteProgressRecovery, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowProgressRecoveryNotice, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "failure", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProgressExitDestination, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EmitViewBack && args.Count == 0)
		{
			EmitViewBack();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginPendingBattleOperation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(BeginPendingBattleOperation());
			return true;
		}
		if (method == MethodName.IsPendingBattleOperationCurrent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPendingBattleOperationCurrent(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CompletePendingBattleOperation && args.Count == 1)
		{
			CompletePendingBattleOperation(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
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
		if (method == MethodName.CancelBattleLifetimes && args.Count == 0)
		{
			CancelBattleLifetimes();
			ret = default;
			return true;
		}
		if (method == MethodName.TrySendBattleStateEvent && args.Count == 1)
		{
			TrySendBattleStateEvent(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendStateEvent && args.Count == 0)
		{
			SendStateEvent();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseLevelBaseConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OldLevelInit && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(OldLevelInit(VariantUtils.ConvertTo<TowerDefenseLevelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.NewLevelInit && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(NewLevelInit(VariantUtils.ConvertTo<TowerDefenseLevelNewConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.AddFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AddFeature(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1])));
			return true;
		}
		if (method == MethodName.AddProcessDependenceFeature && args.Count == 0)
		{
			AddProcessDependenceFeature();
			ret = default;
			return true;
		}
		if (method == MethodName.IsFeatureConfigured && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFeatureConfigured(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.DestroyFeatureSafely && args.Count == 3)
		{
			DestroyFeatureSafely(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeature>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyProcessSafely && args.Count == 2)
		{
			DestroyProcessSafely(VariantUtils.ConvertTo<TowerDefenseBattleProcess>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyAllFeaturesSafely && args.Count == 1)
		{
			DestroyAllFeaturesSafely(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AbortBattleGraphInitialization && args.Count == 0)
		{
			AbortBattleGraphInitialization();
			ret = default;
			return true;
		}
		if (method == MethodName.GetConfiguredFeatureData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(GetConfiguredFeatureData(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildMissingDependenceFeatureData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(BuildMissingDependenceFeatureData(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.ZombieWonLevelFail && args.Count == 1)
		{
			ZombieWonLevelFail(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetFeature && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseBattleFeature>(GetFeature(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveFeature && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveFeature(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.ComponentDependsOnFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ComponentDependsOnFeature(VariantUtils.ConvertTo<TowerDefenseBattleComponentBase>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.ContainsFeatureName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ContainsFeatureName(VariantUtils.ConvertToArray<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.SetProcess && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetProcess(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Dictionary>(in args[1])));
			return true;
		}
		if (method == MethodName.HasProcessName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProcessName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.AddNode && args.Count == 2)
		{
			AddNode(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddUI && args.Count == 2)
		{
			AddUI(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveUI && args.Count == 2)
		{
			MoveUI(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddUIToTopBankContainer && args.Count == 1)
		{
			AddUIToTopBankContainer(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddUIToTopPropContainer && args.Count == 1)
		{
			AddUIToTopPropContainer(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._UnhandledInput && args.Count == 1)
		{
			_UnhandledInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenGamePauseDialog && args.Count == 0)
		{
			OpenGamePauseDialog();
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NpcTalkRequiresPreSpawnBeforeTalk && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(NpcTalkRequiresPreSpawnBeforeTalk());
			return true;
		}
		if (method == MethodName.GetGameEntryFeaturePriority && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetGameEntryFeaturePriority(VariantUtils.ConvertTo<TowerDefenseBattleFeature>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GameInitEntered && args.Count == 0)
		{
			GameInitEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.RejectProgressLoad && args.Count == 1)
		{
			RejectProgressLoad(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GameEntryEntered && args.Count == 0)
		{
			GameEntryEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.GameEntryExited && args.Count == 0)
		{
			GameEntryExited();
			ret = default;
			return true;
		}
		if (method == MethodName.GameReadyEntered && args.Count == 0)
		{
			GameReadyEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.GameReadyExited && args.Count == 0)
		{
			GameReadyExited();
			ret = default;
			return true;
		}
		if (method == MethodName.GameRunningEntered && args.Count == 0)
		{
			GameRunningEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.GameRunningExited && args.Count == 0)
		{
			GameRunningExited();
			ret = default;
			return true;
		}
		if (method == MethodName.GameRunningProcessing && args.Count == 1)
		{
			GameRunningProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResumeEntryCharacters && args.Count == 0)
		{
			ResumeEntryCharacters();
			ret = default;
			return true;
		}
		if (method == MethodName.GameFailEntered && args.Count == 0)
		{
			GameFailEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSceneChange && args.Count == 1)
		{
			OnSceneChange(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TriggerGameFail && args.Count == 1)
		{
			TriggerGameFail(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TriggerGameEntry && args.Count == 0)
		{
			TriggerGameEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.TipsPlay && args.Count == 2)
		{
			TipsPlay(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ViewMap && args.Count == 0)
		{
			ViewMap();
			ret = default;
			return true;
		}
		if (method == MethodName.GameFail && args.Count == 1)
		{
			GameFail(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GameEntry && args.Count == 0)
		{
			GameEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessZombieCheckArea && args.Count == 0)
		{
			ProcessZombieCheckArea();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleZombieEnterHouse && args.Count == 1)
		{
			HandleZombieEnterHouse(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UISwitched && args.Count == 1)
		{
			UISwitched(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PacketUIFront && args.Count == 1)
		{
			PacketUIFront(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChangeCostAdd && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ChangeCostAdd(VariantUtils.ConvertTo<TowerDefensePacketChangeCost>(in args[0])));
			return true;
		}
		if (method == MethodName.ChangeCostRemove && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ChangeCostRemove(VariantUtils.ConvertTo<TowerDefensePacketChangeCost>(in args[0])));
			return true;
		}
		if (method == MethodName.InitPlayerStatusPanel && args.Count == 0)
		{
			InitPlayerStatusPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.GetNextSyncId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNextSyncId());
			return true;
		}
		if (method == MethodName.GetNextPacketSyncId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetNextPacketSyncId());
			return true;
		}
		if (method == MethodName.RegisterSyncPacket && args.Count == 2)
		{
			RegisterSyncPacket(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnregisterSyncPacket && args.Count == 1)
		{
			UnregisterSyncPacket(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveCharacterSyncState && args.Count == 1)
		{
			RemoveCharacterSyncState(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveZombieSyncState && args.Count == 1)
		{
			RemoveZombieSyncState(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterSyncCharacter && args.Count == 2)
		{
			RegisterSyncCharacter(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingDestroyToReadyCharacter && args.Count == 3)
		{
			ApplyPendingDestroyToReadyCharacter(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<Dictionary>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DetachSyncCharacter && args.Count == 1)
		{
			DetachSyncCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncCharacterInitTimer && args.Count == 1)
		{
			SyncCharacterInitTimer(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SyncCharacterPositionTimer && args.Count == 1)
		{
			SyncCharacterPositionTimer(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSyncCharacterDestroy && args.Count == 1)
		{
			OnSyncCharacterDestroy(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CleanupCharacterCell && args.Count == 1)
		{
			CleanupCharacterCell(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyNetworkPauseUi && args.Count == 0)
		{
			ApplyNetworkPauseUi();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyNetworkResumeUi && args.Count == 0)
		{
			ApplyNetworkResumeUi();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyNetworkPauseFromSession && args.Count == 1)
		{
			ApplyNetworkPauseFromSession(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetOrCreateRemoteCursor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<RemoteCursor>(GetOrCreateRemoteCursor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RemoveRemoteCursor && args.Count == 1)
		{
			RemoveRemoteCursor(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRemoteCursorPosition && args.Count == 2)
		{
			ApplyRemoteCursorPosition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRemoteCursorPick && args.Count == 3)
		{
			ApplyRemoteCursorPick(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnChooseReady && args.Count == 1)
		{
			OnChooseReady(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnChooseOver && args.Count == 0)
		{
			OnChooseOver();
			ret = default;
			return true;
		}
		if (method == MethodName.DoChooseOver && args.Count == 0)
		{
			DoChooseOver();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowChooseWaitLabel && args.Count == 0)
		{
			ShowChooseWaitLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.HideChooseWaitLabel && args.Count == 0)
		{
			HideChooseWaitLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateChooseWaitLabel && args.Count == 0)
		{
			UpdateChooseWaitLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.RejectLevelConfiguration && args.Count == 1)
		{
			RejectLevelConfiguration(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReturnFromRejectedLevel && args.Count == 0)
		{
			ReturnFromRejectedLevel();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareSavedFeatures && args.Count == 0)
		{
			PrepareSavedFeatures();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteProgressRecovery && args.Count == 0)
		{
			CompleteProgressRecovery();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowProgressRecoveryNotice && args.Count == 1)
		{
			ShowProgressRecoveryNotice(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProgressExitDestination && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(ProgressExitDestination());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.DestroyFeatureSafely && args.Count == 3)
		{
			DestroyFeatureSafely(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeature>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyProcessSafely && args.Count == 2)
		{
			DestroyProcessSafely(VariantUtils.ConvertTo<TowerDefenseBattleProcess>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ComponentDependsOnFeature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ComponentDependsOnFeature(VariantUtils.ConvertTo<TowerDefenseBattleComponentBase>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.ContainsFeatureName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ContainsFeatureName(VariantUtils.ConvertToArray<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.HasProcessName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasProcessName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetGameEntryFeaturePriority && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetGameEntryFeaturePriority(VariantUtils.ConvertTo<TowerDefenseBattleFeature>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ProgressExitDestination && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(ProgressExitDestination());
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EmitViewBack)
		{
			return true;
		}
		if (method == MethodName.BeginPendingBattleOperation)
		{
			return true;
		}
		if (method == MethodName.IsPendingBattleOperationCurrent)
		{
			return true;
		}
		if (method == MethodName.CompletePendingBattleOperation)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.CancelBattleLifetimes)
		{
			return true;
		}
		if (method == MethodName.TrySendBattleStateEvent)
		{
			return true;
		}
		if (method == MethodName.SendStateEvent)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.OldLevelInit)
		{
			return true;
		}
		if (method == MethodName.NewLevelInit)
		{
			return true;
		}
		if (method == MethodName.AddFeature)
		{
			return true;
		}
		if (method == MethodName.AddProcessDependenceFeature)
		{
			return true;
		}
		if (method == MethodName.IsFeatureConfigured)
		{
			return true;
		}
		if (method == MethodName.DestroyFeatureSafely)
		{
			return true;
		}
		if (method == MethodName.DestroyProcessSafely)
		{
			return true;
		}
		if (method == MethodName.DestroyAllFeaturesSafely)
		{
			return true;
		}
		if (method == MethodName.AbortBattleGraphInitialization)
		{
			return true;
		}
		if (method == MethodName.GetConfiguredFeatureData)
		{
			return true;
		}
		if (method == MethodName.BuildMissingDependenceFeatureData)
		{
			return true;
		}
		if (method == MethodName.ZombieWonLevelFail)
		{
			return true;
		}
		if (method == MethodName.GetFeature)
		{
			return true;
		}
		if (method == MethodName.RemoveFeature)
		{
			return true;
		}
		if (method == MethodName.ComponentDependsOnFeature)
		{
			return true;
		}
		if (method == MethodName.ContainsFeatureName)
		{
			return true;
		}
		if (method == MethodName.SetProcess)
		{
			return true;
		}
		if (method == MethodName.HasProcessName)
		{
			return true;
		}
		if (method == MethodName.AddNode)
		{
			return true;
		}
		if (method == MethodName.AddUI)
		{
			return true;
		}
		if (method == MethodName.MoveUI)
		{
			return true;
		}
		if (method == MethodName.AddUIToTopBankContainer)
		{
			return true;
		}
		if (method == MethodName.AddUIToTopPropContainer)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName._UnhandledInput)
		{
			return true;
		}
		if (method == MethodName.OpenGamePauseDialog)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName.NpcTalkRequiresPreSpawnBeforeTalk)
		{
			return true;
		}
		if (method == MethodName.GetGameEntryFeaturePriority)
		{
			return true;
		}
		if (method == MethodName.GameInitEntered)
		{
			return true;
		}
		if (method == MethodName.RejectProgressLoad)
		{
			return true;
		}
		if (method == MethodName.GameEntryEntered)
		{
			return true;
		}
		if (method == MethodName.GameEntryExited)
		{
			return true;
		}
		if (method == MethodName.GameReadyEntered)
		{
			return true;
		}
		if (method == MethodName.GameReadyExited)
		{
			return true;
		}
		if (method == MethodName.GameRunningEntered)
		{
			return true;
		}
		if (method == MethodName.GameRunningExited)
		{
			return true;
		}
		if (method == MethodName.GameRunningProcessing)
		{
			return true;
		}
		if (method == MethodName.ResumeEntryCharacters)
		{
			return true;
		}
		if (method == MethodName.GameFailEntered)
		{
			return true;
		}
		if (method == MethodName.OnSceneChange)
		{
			return true;
		}
		if (method == MethodName.TriggerGameFail)
		{
			return true;
		}
		if (method == MethodName.TriggerGameEntry)
		{
			return true;
		}
		if (method == MethodName.TipsPlay)
		{
			return true;
		}
		if (method == MethodName.ViewMap)
		{
			return true;
		}
		if (method == MethodName.GameFail)
		{
			return true;
		}
		if (method == MethodName.GameEntry)
		{
			return true;
		}
		if (method == MethodName.ProcessZombieCheckArea)
		{
			return true;
		}
		if (method == MethodName.HandleZombieEnterHouse)
		{
			return true;
		}
		if (method == MethodName.UISwitched)
		{
			return true;
		}
		if (method == MethodName.PacketUIFront)
		{
			return true;
		}
		if (method == MethodName.ChangeCostAdd)
		{
			return true;
		}
		if (method == MethodName.ChangeCostRemove)
		{
			return true;
		}
		if (method == MethodName.InitPlayerStatusPanel)
		{
			return true;
		}
		if (method == MethodName.GetNextSyncId)
		{
			return true;
		}
		if (method == MethodName.GetNextPacketSyncId)
		{
			return true;
		}
		if (method == MethodName.RegisterSyncPacket)
		{
			return true;
		}
		if (method == MethodName.UnregisterSyncPacket)
		{
			return true;
		}
		if (method == MethodName.RemoveCharacterSyncState)
		{
			return true;
		}
		if (method == MethodName.RemoveZombieSyncState)
		{
			return true;
		}
		if (method == MethodName.RegisterSyncCharacter)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingDestroyToReadyCharacter)
		{
			return true;
		}
		if (method == MethodName.DetachSyncCharacter)
		{
			return true;
		}
		if (method == MethodName.SyncCharacterInitTimer)
		{
			return true;
		}
		if (method == MethodName.SyncCharacterPositionTimer)
		{
			return true;
		}
		if (method == MethodName.OnSyncCharacterDestroy)
		{
			return true;
		}
		if (method == MethodName.CleanupCharacterCell)
		{
			return true;
		}
		if (method == MethodName.ApplyNetworkPauseUi)
		{
			return true;
		}
		if (method == MethodName.ApplyNetworkResumeUi)
		{
			return true;
		}
		if (method == MethodName.ApplyNetworkPauseFromSession)
		{
			return true;
		}
		if (method == MethodName.GetOrCreateRemoteCursor)
		{
			return true;
		}
		if (method == MethodName.RemoveRemoteCursor)
		{
			return true;
		}
		if (method == MethodName.ApplyRemoteCursorPosition)
		{
			return true;
		}
		if (method == MethodName.ApplyRemoteCursorPick)
		{
			return true;
		}
		if (method == MethodName.OnChooseReady)
		{
			return true;
		}
		if (method == MethodName.OnChooseOver)
		{
			return true;
		}
		if (method == MethodName.DoChooseOver)
		{
			return true;
		}
		if (method == MethodName.ShowChooseWaitLabel)
		{
			return true;
		}
		if (method == MethodName.HideChooseWaitLabel)
		{
			return true;
		}
		if (method == MethodName.UpdateChooseWaitLabel)
		{
			return true;
		}
		if (method == MethodName.RejectLevelConfiguration)
		{
			return true;
		}
		if (method == MethodName.ReturnFromRejectedLevel)
		{
			return true;
		}
		if (method == MethodName.PrepareSavedFeatures)
		{
			return true;
		}
		if (method == MethodName.CompleteProgressRecovery)
		{
			return true;
		}
		if (method == MethodName.ShowProgressRecoveryNotice)
		{
			return true;
		}
		if (method == MethodName.ProgressExitDestination)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.state)
		{
			state = VariantUtils.ConvertTo<StateChart>(in value);
			return true;
		}
		if (name == PropertyName.levelControl)
		{
			levelControl = VariantUtils.ConvertTo<TowerDefenseInGameLevelControl>(in value);
			return true;
		}
		if (name == PropertyName.characterCanvasModulate)
		{
			characterCanvasModulate = VariantUtils.ConvertTo<CanvasModulate>(in value);
			return true;
		}
		if (name == PropertyName.characterNode)
		{
			characterNode = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName.bankUILayer)
		{
			bankUILayer = VariantUtils.ConvertTo<CanvasLayer>(in value);
			return true;
		}
		if (name == PropertyName.uITopBankContainer)
		{
			uITopBankContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.uITopPropContainer)
		{
			uITopPropContainer = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.mobileInterval)
		{
			mobileInterval = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName.uiTopAnimationPlayer)
		{
			uiTopAnimationPlayer = VariantUtils.ConvertTo<AnimationPlayer>(in value);
			return true;
		}
		if (name == PropertyName._coexistHud)
		{
			_coexistHud = VariantUtils.ConvertTo<TowerDefenseCoexistHud>(in value);
			return true;
		}
		if (name == PropertyName.process)
		{
			process = VariantUtils.ConvertTo<TowerDefenseBattleProcess>(in value);
			return true;
		}
		if (name == PropertyName.zombieWon)
		{
			zombieWon = VariantUtils.ConvertTo<TowerDefenseZombieWon>(in value);
			return true;
		}
		if (name == PropertyName.zombieCheckArea)
		{
			zombieCheckArea = VariantUtils.ConvertTo<AabbArea2D>(in value);
			return true;
		}
		if (name == PropertyName.isGameRunning)
		{
			isGameRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isGameFail)
		{
			isGameFail = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isInit)
		{
			isInit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.failCharacter)
		{
			failCharacter = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName.waitPause)
		{
			waitPause = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isView)
		{
			isView = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._battleGraphReady)
		{
			_battleGraphReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._battleShuttingDown)
		{
			_battleShuttingDown = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._preserveLevelConfigOnExit)
		{
			_preserveLevelConfigOnExit = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._gameRunningEntryReady)
		{
			_gameRunningEntryReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingDestroySyncIds)
		{
			_pendingDestroySyncIds = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._syncIdCounter)
		{
			_syncIdCounter = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._packetSyncIdCounter)
		{
			_packetSyncIdCounter = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._isNetworkPaused)
		{
			_isNetworkPaused = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._zombieLastSyncState)
		{
			_zombieLastSyncState = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._chooseOverReceived)
		{
			_chooseOverReceived = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._chooseWaitLabel)
		{
			_chooseWaitLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName._playerStatusPanel)
		{
			_playerStatusPanel = VariantUtils.ConvertTo<PlayerStatusPanel>(in value);
			return true;
		}
		if (name == PropertyName._gameStateLastSync)
		{
			_gameStateLastSync = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._characterLastSyncState)
		{
			_characterLastSyncState = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._componentLastSyncState)
		{
			_componentLastSyncState = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._levelConfigurationRejected)
		{
			_levelConfigurationRejected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._restoringProgress)
		{
			_restoringProgress = VariantUtils.ConvertTo<TowerDefenseLevelSaveConfigCSharp>(in value);
			return true;
		}
		if (name == PropertyName._progressPause)
		{
			_progressPause = VariantUtils.ConvertTo<DialogBoxBase>(in value);
			return true;
		}
		if (name == PropertyName._progressNoticeShown)
		{
			_progressNoticeShown = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.IsNetworkPaused)
		{
			from = IsNetworkPaused;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasPendingBattleOperations)
		{
			from = HasPendingBattleOperations;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Dictionary from2;
		if (name == PropertyName.GameStateLastSync)
		{
			from2 = GameStateLastSync;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CharacterLastSyncState)
		{
			from2 = CharacterLastSyncState;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ComponentLastSyncState)
		{
			from2 = ComponentLastSyncState;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PendingDestroySyncIds)
		{
			from2 = PendingDestroySyncIds;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ZombieLastSyncState)
		{
			from2 = ZombieLastSyncState;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.state)
		{
			value = VariantUtils.CreateFrom(in state);
			return true;
		}
		if (name == PropertyName.levelControl)
		{
			value = VariantUtils.CreateFrom(in levelControl);
			return true;
		}
		if (name == PropertyName.characterCanvasModulate)
		{
			value = VariantUtils.CreateFrom(in characterCanvasModulate);
			return true;
		}
		if (name == PropertyName.characterNode)
		{
			value = VariantUtils.CreateFrom(in characterNode);
			return true;
		}
		if (name == PropertyName.bankUILayer)
		{
			value = VariantUtils.CreateFrom(in bankUILayer);
			return true;
		}
		if (name == PropertyName.uITopBankContainer)
		{
			value = VariantUtils.CreateFrom(in uITopBankContainer);
			return true;
		}
		if (name == PropertyName.uITopPropContainer)
		{
			value = VariantUtils.CreateFrom(in uITopPropContainer);
			return true;
		}
		if (name == PropertyName.mobileInterval)
		{
			value = VariantUtils.CreateFrom(in mobileInterval);
			return true;
		}
		if (name == PropertyName.uiTopAnimationPlayer)
		{
			value = VariantUtils.CreateFrom(in uiTopAnimationPlayer);
			return true;
		}
		if (name == PropertyName._coexistHud)
		{
			value = VariantUtils.CreateFrom(in _coexistHud);
			return true;
		}
		if (name == PropertyName.process)
		{
			value = VariantUtils.CreateFrom(in process);
			return true;
		}
		if (name == PropertyName.zombieWon)
		{
			value = VariantUtils.CreateFrom(in zombieWon);
			return true;
		}
		if (name == PropertyName.zombieCheckArea)
		{
			value = VariantUtils.CreateFrom(in zombieCheckArea);
			return true;
		}
		if (name == PropertyName.isGameRunning)
		{
			value = VariantUtils.CreateFrom(in isGameRunning);
			return true;
		}
		if (name == PropertyName.isGameFail)
		{
			value = VariantUtils.CreateFrom(in isGameFail);
			return true;
		}
		if (name == PropertyName.isInit)
		{
			value = VariantUtils.CreateFrom(in isInit);
			return true;
		}
		if (name == PropertyName.failCharacter)
		{
			value = VariantUtils.CreateFrom(in failCharacter);
			return true;
		}
		if (name == PropertyName.waitPause)
		{
			value = VariantUtils.CreateFrom(in waitPause);
			return true;
		}
		if (name == PropertyName.isView)
		{
			value = VariantUtils.CreateFrom(in isView);
			return true;
		}
		if (name == PropertyName._battleGraphReady)
		{
			value = VariantUtils.CreateFrom(in _battleGraphReady);
			return true;
		}
		if (name == PropertyName._battleShuttingDown)
		{
			value = VariantUtils.CreateFrom(in _battleShuttingDown);
			return true;
		}
		if (name == PropertyName._preserveLevelConfigOnExit)
		{
			value = VariantUtils.CreateFrom(in _preserveLevelConfigOnExit);
			return true;
		}
		if (name == PropertyName._gameRunningEntryReady)
		{
			value = VariantUtils.CreateFrom(in _gameRunningEntryReady);
			return true;
		}
		if (name == PropertyName._pendingDestroySyncIds)
		{
			value = VariantUtils.CreateFrom(in _pendingDestroySyncIds);
			return true;
		}
		if (name == PropertyName._syncIdCounter)
		{
			value = VariantUtils.CreateFrom(in _syncIdCounter);
			return true;
		}
		if (name == PropertyName._packetSyncIdCounter)
		{
			value = VariantUtils.CreateFrom(in _packetSyncIdCounter);
			return true;
		}
		if (name == PropertyName._isNetworkPaused)
		{
			value = VariantUtils.CreateFrom(in _isNetworkPaused);
			return true;
		}
		if (name == PropertyName._zombieLastSyncState)
		{
			value = VariantUtils.CreateFrom(in _zombieLastSyncState);
			return true;
		}
		if (name == PropertyName._chooseOverReceived)
		{
			value = VariantUtils.CreateFrom(in _chooseOverReceived);
			return true;
		}
		if (name == PropertyName._chooseWaitLabel)
		{
			value = VariantUtils.CreateFrom(in _chooseWaitLabel);
			return true;
		}
		if (name == PropertyName._playerStatusPanel)
		{
			value = VariantUtils.CreateFrom(in _playerStatusPanel);
			return true;
		}
		if (name == PropertyName.PlayerColors)
		{
			value = VariantUtils.CreateFrom(in PlayerColors);
			return true;
		}
		if (name == PropertyName._gameStateLastSync)
		{
			value = VariantUtils.CreateFrom(in _gameStateLastSync);
			return true;
		}
		if (name == PropertyName._characterLastSyncState)
		{
			value = VariantUtils.CreateFrom(in _characterLastSyncState);
			return true;
		}
		if (name == PropertyName._componentLastSyncState)
		{
			value = VariantUtils.CreateFrom(in _componentLastSyncState);
			return true;
		}
		if (name == PropertyName._levelConfigurationRejected)
		{
			value = VariantUtils.CreateFrom(in _levelConfigurationRejected);
			return true;
		}
		if (name == PropertyName._restoringProgress)
		{
			value = VariantUtils.CreateFrom(in _restoringProgress);
			return true;
		}
		if (name == PropertyName._progressPause)
		{
			value = VariantUtils.CreateFrom(in _progressPause);
			return true;
		}
		if (name == PropertyName._progressNoticeShown)
		{
			value = VariantUtils.CreateFrom(in _progressNoticeShown);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.state, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.levelControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterCanvasModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.bankUILayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.uITopBankContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.uITopPropContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mobileInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.uiTopAnimationPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._coexistHud, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.process, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.zombieWon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.zombieCheckArea, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isGameRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isGameFail, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isInit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.failCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.waitPause, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isView, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._battleGraphReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._battleShuttingDown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preserveLevelConfigOnExit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._gameRunningEntryReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._pendingDestroySyncIds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._syncIdCounter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._packetSyncIdCounter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isNetworkPaused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsNetworkPaused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._zombieLastSyncState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._chooseOverReceived, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._chooseWaitLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._playerStatusPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasPendingBattleOperations, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedColorArray, PropertyName.PlayerColors, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._gameStateLastSync, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._characterLastSyncState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._componentLastSyncState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.GameStateLastSync, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.CharacterLastSyncState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.ComponentLastSyncState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.PendingDestroySyncIds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.ZombieLastSyncState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._levelConfigurationRejected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._restoringProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._progressPause, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._progressNoticeShown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.state, Variant.From(in state));
		info.AddProperty(PropertyName.levelControl, Variant.From(in levelControl));
		info.AddProperty(PropertyName.characterCanvasModulate, Variant.From(in characterCanvasModulate));
		info.AddProperty(PropertyName.characterNode, Variant.From(in characterNode));
		info.AddProperty(PropertyName.bankUILayer, Variant.From(in bankUILayer));
		info.AddProperty(PropertyName.uITopBankContainer, Variant.From(in uITopBankContainer));
		info.AddProperty(PropertyName.uITopPropContainer, Variant.From(in uITopPropContainer));
		info.AddProperty(PropertyName.mobileInterval, Variant.From(in mobileInterval));
		info.AddProperty(PropertyName.uiTopAnimationPlayer, Variant.From(in uiTopAnimationPlayer));
		info.AddProperty(PropertyName._coexistHud, Variant.From(in _coexistHud));
		info.AddProperty(PropertyName.process, Variant.From(in process));
		info.AddProperty(PropertyName.zombieWon, Variant.From(in zombieWon));
		info.AddProperty(PropertyName.zombieCheckArea, Variant.From(in zombieCheckArea));
		info.AddProperty(PropertyName.isGameRunning, Variant.From(in isGameRunning));
		info.AddProperty(PropertyName.isGameFail, Variant.From(in isGameFail));
		info.AddProperty(PropertyName.isInit, Variant.From(in isInit));
		info.AddProperty(PropertyName.failCharacter, Variant.From(in failCharacter));
		info.AddProperty(PropertyName.waitPause, Variant.From(in waitPause));
		info.AddProperty(PropertyName.isView, Variant.From(in isView));
		info.AddProperty(PropertyName._battleGraphReady, Variant.From(in _battleGraphReady));
		info.AddProperty(PropertyName._battleShuttingDown, Variant.From(in _battleShuttingDown));
		info.AddProperty(PropertyName._preserveLevelConfigOnExit, Variant.From(in _preserveLevelConfigOnExit));
		info.AddProperty(PropertyName._gameRunningEntryReady, Variant.From(in _gameRunningEntryReady));
		info.AddProperty(PropertyName._pendingDestroySyncIds, Variant.From(in _pendingDestroySyncIds));
		info.AddProperty(PropertyName._syncIdCounter, Variant.From(in _syncIdCounter));
		info.AddProperty(PropertyName._packetSyncIdCounter, Variant.From(in _packetSyncIdCounter));
		info.AddProperty(PropertyName._isNetworkPaused, Variant.From(in _isNetworkPaused));
		info.AddProperty(PropertyName._zombieLastSyncState, Variant.From(in _zombieLastSyncState));
		info.AddProperty(PropertyName._chooseOverReceived, Variant.From(in _chooseOverReceived));
		info.AddProperty(PropertyName._chooseWaitLabel, Variant.From(in _chooseWaitLabel));
		info.AddProperty(PropertyName._playerStatusPanel, Variant.From(in _playerStatusPanel));
		info.AddProperty(PropertyName._gameStateLastSync, Variant.From(in _gameStateLastSync));
		info.AddProperty(PropertyName._characterLastSyncState, Variant.From(in _characterLastSyncState));
		info.AddProperty(PropertyName._componentLastSyncState, Variant.From(in _componentLastSyncState));
		info.AddProperty(PropertyName._levelConfigurationRejected, Variant.From(in _levelConfigurationRejected));
		info.AddProperty(PropertyName._restoringProgress, Variant.From(in _restoringProgress));
		info.AddProperty(PropertyName._progressPause, Variant.From(in _progressPause));
		info.AddProperty(PropertyName._progressNoticeShown, Variant.From(in _progressNoticeShown));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.state, out var value))
		{
			state = value.As<StateChart>();
		}
		if (info.TryGetProperty(PropertyName.levelControl, out var value2))
		{
			levelControl = value2.As<TowerDefenseInGameLevelControl>();
		}
		if (info.TryGetProperty(PropertyName.characterCanvasModulate, out var value3))
		{
			characterCanvasModulate = value3.As<CanvasModulate>();
		}
		if (info.TryGetProperty(PropertyName.characterNode, out var value4))
		{
			characterNode = value4.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName.bankUILayer, out var value5))
		{
			bankUILayer = value5.As<CanvasLayer>();
		}
		if (info.TryGetProperty(PropertyName.uITopBankContainer, out var value6))
		{
			uITopBankContainer = value6.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.uITopPropContainer, out var value7))
		{
			uITopPropContainer = value7.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.mobileInterval, out var value8))
		{
			mobileInterval = value8.As<Control>();
		}
		if (info.TryGetProperty(PropertyName.uiTopAnimationPlayer, out var value9))
		{
			uiTopAnimationPlayer = value9.As<AnimationPlayer>();
		}
		if (info.TryGetProperty(PropertyName._coexistHud, out var value10))
		{
			_coexistHud = value10.As<TowerDefenseCoexistHud>();
		}
		if (info.TryGetProperty(PropertyName.process, out var value11))
		{
			process = value11.As<TowerDefenseBattleProcess>();
		}
		if (info.TryGetProperty(PropertyName.zombieWon, out var value12))
		{
			zombieWon = value12.As<TowerDefenseZombieWon>();
		}
		if (info.TryGetProperty(PropertyName.zombieCheckArea, out var value13))
		{
			zombieCheckArea = value13.As<AabbArea2D>();
		}
		if (info.TryGetProperty(PropertyName.isGameRunning, out var value14))
		{
			isGameRunning = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isGameFail, out var value15))
		{
			isGameFail = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isInit, out var value16))
		{
			isInit = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.failCharacter, out var value17))
		{
			failCharacter = value17.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName.waitPause, out var value18))
		{
			waitPause = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isView, out var value19))
		{
			isView = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._battleGraphReady, out var value20))
		{
			_battleGraphReady = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._battleShuttingDown, out var value21))
		{
			_battleShuttingDown = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._preserveLevelConfigOnExit, out var value22))
		{
			_preserveLevelConfigOnExit = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._gameRunningEntryReady, out var value23))
		{
			_gameRunningEntryReady = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingDestroySyncIds, out var value24))
		{
			_pendingDestroySyncIds = value24.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._syncIdCounter, out var value25))
		{
			_syncIdCounter = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName._packetSyncIdCounter, out var value26))
		{
			_packetSyncIdCounter = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName._isNetworkPaused, out var value27))
		{
			_isNetworkPaused = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._zombieLastSyncState, out var value28))
		{
			_zombieLastSyncState = value28.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._chooseOverReceived, out var value29))
		{
			_chooseOverReceived = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._chooseWaitLabel, out var value30))
		{
			_chooseWaitLabel = value30.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName._playerStatusPanel, out var value31))
		{
			_playerStatusPanel = value31.As<PlayerStatusPanel>();
		}
		if (info.TryGetProperty(PropertyName._gameStateLastSync, out var value32))
		{
			_gameStateLastSync = value32.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._characterLastSyncState, out var value33))
		{
			_characterLastSyncState = value33.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._componentLastSyncState, out var value34))
		{
			_componentLastSyncState = value34.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._levelConfigurationRejected, out var value35))
		{
			_levelConfigurationRejected = value35.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._restoringProgress, out var value36))
		{
			_restoringProgress = value36.As<TowerDefenseLevelSaveConfigCSharp>();
		}
		if (info.TryGetProperty(PropertyName._progressPause, out var value37))
		{
			_progressPause = value37.As<DialogBoxBase>();
		}
		if (info.TryGetProperty(PropertyName._progressNoticeShown, out var value38))
		{
			_progressNoticeShown = value38.As<bool>();
		}
	}
}
