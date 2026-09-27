using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

[GlobalClass]
[ScriptPath("res://Core/MultiPlayerManager/MultiPlayerManager.cs")]
public class MultiPlayerManager : Node
{
	public delegate void AllClientsReadyEventHandler();

	public delegate void AllLevelConfigAckedEventHandler();

	public delegate void AllGameEntryAckedEventHandler();

	public delegate void MatchCreatedEventHandler(string matchId);

	public delegate void MatchJoinedEventHandler(string matchId);

	public delegate void MatchLeftEventHandler();

	public delegate void PeerJoinedEventHandler(string username);

	public delegate void PeerLeftEventHandler(string username, string peerId);

	public delegate void MatchStateReceivedEventHandler(string opCode, string data, string senderId);

	public delegate void ConnectionChangedEventHandler(bool connected);

	public delegate void PingUpdatedEventHandler(string peerId, int latencyMs);

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName EmitAllClientsReady = "EmitAllClientsReady";

		public static readonly StringName EmitAllGameEntryAcked = "EmitAllGameEntryAcked";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName GetPeerLatency = "GetPeerLatency";

		public static readonly StringName GetSignalLevel = "GetSignalLevel";

		public static readonly StringName _SendPing = "_SendPing";

		public static readonly StringName _RpcPing = "_RpcPing";

		public static readonly StringName _RpcPong = "_RpcPong";

		public static readonly StringName _RpcPingResult = "_RpcPingResult";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LogOut = "LogOut";

		public static readonly StringName IsConnect = "IsConnect";

		public static readonly StringName CanSendMatchRpc = "CanSendMatchRpc";

		public static readonly StringName GetUserName = "GetUserName";

		public static readonly StringName GetUserDisplayName = "GetUserDisplayName";

		public static readonly StringName GetPeerName = "GetPeerName";

		public static readonly StringName CreateMatch = "CreateMatch";

		public static readonly StringName JoinMatch = "JoinMatch";

		public static readonly StringName LeaveMatch = "LeaveMatch";

		public static readonly StringName SendMatchState = "SendMatchState";

		public static readonly StringName SendMatchStateUnreliable = "SendMatchStateUnreliable";

		public static readonly StringName SendMatchStateToPeer = "SendMatchStateToPeer";

		public static readonly StringName _RpcReceiveMatchState = "_RpcReceiveMatchState";

		public static readonly StringName _RpcReceiveMatchStateUnreliable = "_RpcReceiveMatchStateUnreliable";

		public static readonly StringName _CreateEnvelopeData = "_CreateEnvelopeData";

		public static readonly StringName _TryDispatchEnvelope = "_TryDispatchEnvelope";

		public static readonly StringName _TryHandleUnreliableEnvelope = "_TryHandleUnreliableEnvelope";

		public static readonly StringName IsEnvelopeSenderAuthorized = "IsEnvelopeSenderAuthorized";

		public static readonly StringName RequiresHostAuthority = "RequiresHostAuthority";

		public static readonly StringName NormalizeAuthenticatedUserId = "NormalizeAuthenticatedUserId";

		public static readonly StringName _BroadcastReliableEnvelope = "_BroadcastReliableEnvelope";

		public static readonly StringName _RejectLegacyProtocol = "_RejectLegacyProtocol";

		public static readonly StringName _RejectProtocolVersion = "_RejectProtocolVersion";

		public static readonly StringName _ProcessMatchState = "_ProcessMatchState";

		public static readonly StringName SendSelectLevel = "SendSelectLevel";

		public static readonly StringName SendLevelConfig = "SendLevelConfig";

		public static readonly StringName BuildLevelConfigData = "BuildLevelConfigData";

		public static readonly StringName _ConvertFeatureData = "_ConvertFeatureData";

		public static readonly StringName _SerializeFeatures = "_SerializeFeatures";

		public static readonly StringName _FindLevelUid = "_FindLevelUid";

		public static readonly StringName SendStartGame = "SendStartGame";

		public static readonly StringName SendLateJoinBootstrap = "SendLateJoinBootstrap";

		public static readonly StringName SendPlacePlant = "SendPlacePlant";

		public static readonly StringName SendCommandRejected = "SendCommandRejected";

		public static readonly StringName SendRemovePlant = "SendRemovePlant";

		public static readonly StringName SendUseShovel = "SendUseShovel";

		public static readonly StringName SendMoveCharacter = "SendMoveCharacter";

		public static readonly StringName SendGameStateSync = "SendGameStateSync";

		public static readonly StringName SendSpawnZombie = "SendSpawnZombie";

		public static readonly StringName SendSpawnGrid = "SendSpawnGrid";

		public static readonly StringName SendGameResult = "SendGameResult";

		public static readonly StringName SendCharacterDestroy = "SendCharacterDestroy";

		public static readonly StringName SendCharacterInit = "SendCharacterInit";

		public static readonly StringName SendCharacterStateSync = "SendCharacterStateSync";

		public static readonly StringName SendCharacterPositionSync = "SendCharacterPositionSync";

		public static readonly StringName SendZombieFullSync = "SendZombieFullSync";

		public static readonly StringName SendCursorSync = "SendCursorSync";

		public static readonly StringName SendCursorPickSync = "SendCursorPickSync";

		public static readonly StringName SendChooseReady = "SendChooseReady";

		public static readonly StringName SendChooseOver = "SendChooseOver";

		public static readonly StringName SendVaseBreakRequest = "SendVaseBreakRequest";

		public static readonly StringName SendGemMatchCommand = "SendGemMatchCommand";

		public static readonly StringName SendVaseBreakResult = "SendVaseBreakResult";

		public static readonly StringName SendPacketPick = "SendPacketPick";

		public static readonly StringName SendPause = "SendPause";

		public static readonly StringName SendResume = "SendResume";

		public static readonly StringName SendSpawnCoin = "SendSpawnCoin";

		public static readonly StringName SendSpawnFallingObject = "SendSpawnFallingObject";

		public static readonly StringName SendSpawnCharacterAt = "SendSpawnCharacterAt";

		public static readonly StringName SendConveyorSpawn = "SendConveyorSpawn";

		public static readonly StringName SendClientReady = "SendClientReady";

		public static readonly StringName SendGameEntry = "SendGameEntry";

		public static readonly StringName SendTipsPlay = "SendTipsPlay";

		public static readonly StringName SendDamagePart = "SendDamagePart";

		public static readonly StringName SendDamagePointReach = "SendDamagePointReach";

		public static readonly StringName SendArmorDamagePointReach = "SendArmorDamagePointReach";

		public static readonly StringName SendArmorHitpointsEmpty = "SendArmorHitpointsEmpty";

		public static readonly StringName SendCraterCreate = "SendCraterCreate";

		public static readonly StringName SendCharacterComponentOperation = "SendCharacterComponentOperation";

		public static readonly StringName SendPlantFullSync = "SendPlantFullSync";

		public static readonly StringName SendEventExecute = "SendEventExecute";

		public static readonly StringName SendWaveEventExecute = "SendWaveEventExecute";

		public static readonly StringName ResetClientsReady = "ResetClientsReady";

		public static readonly StringName _ResetSessionState = "_ResetSessionState";

		public static readonly StringName CheckAllClientsReady = "CheckAllClientsReady";

		public static readonly StringName SendLevelConfigAck = "SendLevelConfigAck";

		public static readonly StringName ResetLevelConfigAck = "ResetLevelConfigAck";

		public static readonly StringName CheckAllLevelConfigAcked = "CheckAllLevelConfigAcked";

		public static readonly StringName SendGameEntryAck = "SendGameEntryAck";

		public static readonly StringName ResetGameEntryAck = "ResetGameEntryAck";

		public static readonly StringName CheckAllGameEntryAcked = "CheckAllGameEntryAcked";

		public static readonly StringName GetMatchIdShort = "GetMatchIdShort";

		public static readonly StringName _GetLanIp = "_GetLanIp";

		public static readonly StringName GetAllLanIps = "GetAllLanIps";

		public static readonly StringName _OnPeerConnected = "_OnPeerConnected";

		public static readonly StringName _RpcSendHostInfo = "_RpcSendHostInfo";

		public static readonly StringName _RpcVersionMismatch = "_RpcVersionMismatch";

		public static readonly StringName _RpcSyncMembers = "_RpcSyncMembers";

		public static readonly StringName _RpcSendClientName = "_RpcSendClientName";

		public static readonly StringName _broadcastPeerNames = "_broadcastPeerNames";

		public static readonly StringName _RpcSyncPeerNames = "_RpcSyncPeerNames";

		public static readonly StringName _OnPeerDisconnected = "_OnPeerDisconnected";

		public static readonly StringName ReevaluateHostBarriersAfterPeerLeft = "ReevaluateHostBarriersAfterPeerLeft";

		public static readonly StringName _OnConnectedToServer = "_OnConnectedToServer";

		public static readonly StringName _OnConnectionFailed = "_OnConnectionFailed";

		public static readonly StringName _OnServerDisconnected = "_OnServerDisconnected";

		public static readonly StringName _CleanupConnection = "_CleanupConnection";

		public static readonly StringName _HandleStartGame = "_HandleStartGame";

		public static readonly StringName _LoadLevelById = "_LoadLevelById";

		public static readonly StringName _HandleSelectLevel = "_HandleSelectLevel";

		public static readonly StringName _HandleLevelConfig = "_HandleLevelConfig";

		public static readonly StringName _CreateLevelConfigFromReceived = "_CreateLevelConfigFromReceived";

		public static readonly StringName MatchesAcceptedLevel = "MatchesAcceptedLevel";

		public static readonly StringName IsBattleParticipant = "IsBattleParticipant";

		public static readonly StringName ShowModCompatibilityFailure = "ShowModCompatibilityFailure";

		public static readonly StringName ReceiveModAck = "ReceiveModAck";

		public static readonly StringName ReceiveCompatibleLevelConfig = "ReceiveCompatibleLevelConfig";

		public static readonly StringName CacheCompatibleLevelConfig = "CacheCompatibleLevelConfig";

		public static readonly StringName AcceptCompatibleStart = "AcceptCompatibleStart";

		public static readonly StringName ReadAdmittedRoster = "ReadAdmittedRoster";

		public static readonly StringName ProcessModCompatibility = "ProcessModCompatibility";

		public static readonly StringName ModPeerChanged = "ModPeerChanged";

		public static readonly StringName ReleaseBattleCompatibility = "ReleaseBattleCompatibility";

		public static readonly StringName AllowsModMessage = "AllowsModMessage";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName currentMatchId = "currentMatchId";

		public static readonly StringName isHost = "isHost";

		public static readonly StringName peerId = "peerId";

		public static readonly StringName matchMembers = "matchMembers";

		public static readonly StringName selectedLevelId = "selectedLevelId";

		public static readonly StringName _clientsReady = "_clientsReady";

		public static readonly StringName ClientsReady = "ClientsReady";

		public static readonly StringName _gameEntryAcked = "_gameEntryAcked";

		public static readonly StringName GameEntryAcked = "GameEntryAcked";

		public static readonly StringName GameEntrySent = "GameEntrySent";

		public static readonly StringName CurrentPort = "CurrentPort";

		public static readonly StringName LastModCompatibilityFailure = "LastModCompatibilityFailure";

		public static readonly StringName BattleAdmittedPeers = "BattleAdmittedPeers";

		public static readonly StringName _peer = "_peer";

		public static readonly StringName _playerName = "_playerName";

		public static readonly StringName _receivedLevelConfigJson = "_receivedLevelConfigJson";

		public static readonly StringName _receivedLevelConfigType = "_receivedLevelConfigType";

		public static readonly StringName _receivedLevelEnterMode = "_receivedLevelEnterMode";

		public static readonly StringName _receivedLevelIsBattle = "_receivedLevelIsBattle";

		public static readonly StringName _activeBattleLevelConfigData = "_activeBattleLevelConfigData";

		public static readonly StringName _activeBattleStartData = "_activeBattleStartData";

		public static readonly StringName _gameEntrySent = "_gameEntrySent";

		public static readonly StringName _pingTimer = "_pingTimer";

		public static readonly StringName _reliableSequence = "_reliableSequence";

		public static readonly StringName _unreliableSequence = "_unreliableSequence";

		public static readonly StringName _fallingObjectSpawnSequence = "_fallingObjectSpawnSequence";

		public static readonly StringName _damagePartSpawnSequence = "_damagePartSpawnSequence";

		public static readonly StringName _sessionGeneration = "_sessionGeneration";

		public static readonly StringName _startGameTransitionPending = "_startGameTransitionPending";

		public static readonly StringName _startGameTransitionCompleted = "_startGameTransitionCompleted";

		public static readonly StringName _currentPort = "_currentPort";

		public static readonly StringName _preparingModStart = "_preparingModStart";

		public static readonly StringName _modBattleActive = "_modBattleActive";

		public static readonly StringName _modBattleId = "_modBattleId";

		public static readonly StringName _acceptedModBatch = "_acceptedModBatch";

		public static readonly StringName _pendingRemoteModBatch = "_pendingRemoteModBatch";

		public static readonly StringName _remoteModTicket = "_remoteModTicket";

		public static readonly StringName _remoteModDeadline = "_remoteModDeadline";

		public static readonly StringName _battleSceneDeadline = "_battleSceneDeadline";
	}

	public new class SignalName : Node.SignalName
	{
	}

	public const int DEFAULT_PORT = 7777;

	public const int MAX_PLAYERS = 4;

	private ENetMultiplayerPeer _peer;

	private string _playerName = "";

	private string _receivedLevelConfigJson = "";

	private string _receivedLevelConfigType = "";

	private string _receivedLevelEnterMode = "";

	private bool _receivedLevelIsBattle;

	private string _activeBattleLevelConfigData = "";

	private string _activeBattleStartData = "";

	private bool _gameEntrySent;

	private double _pingTimer;

	private readonly NetworkMessageRouter _networkMessageRouter = new NetworkMessageRouter();

	private long _reliableSequence;

	private long _unreliableSequence;

	private long _fallingObjectSpawnSequence;

	private long _damagePartSpawnSequence;

	private readonly System.Collections.Generic.Dictionary<string, long> _lastUnreliableSequenceByStream = new System.Collections.Generic.Dictionary<string, long>();

	private long _sessionGeneration;

	private bool _startGameTransitionPending;

	private bool _startGameTransitionCompleted;

	public const double PING_INTERVAL = 2.0;

	private int _currentPort = 7777;

	private XWModLevelIdentity _acceptedLevelIdentity;

	private readonly System.Collections.Generic.Dictionary<string, ModCompatibilityBatch> _modBatches = new System.Collections.Generic.Dictionary<string, ModCompatibilityBatch>(StringComparer.Ordinal);

	private readonly HashSet<string> _battleAdmitted = new HashSet<string>(StringComparer.Ordinal);

	private ModCompatibilityBatch _hostModBatch;

	private XWModEnvironmentService.EnvironmentLease _modEnvironmentLease;

	private bool _preparingModStart;

	private bool _modBattleActive;

	private string _modBattleId = "";

	private string _acceptedModBatch = "";

	private string _pendingRemoteModBatch = "";

	private long _remoteModTicket;

	private long _remoteModDeadline;

	private long _battleSceneDeadline;

	private TaskCompletionSource<bool> _modSceneAccepted;

	public static MultiPlayerManager Instance { get; private set; }

	public string currentMatchId { get; set; } = "";

	public bool isHost { get; set; }

	public static bool IsHost
	{
		get
		{
			if (Instance != null)
			{
				return Instance.isHost;
			}
			return false;
		}
	}

	public string peerId { get; set; } = "";

	public Godot.Collections.Array matchMembers { get; set; } = new Godot.Collections.Array();

	public string selectedLevelId { get; set; } = "";

	private System.Collections.Generic.Dictionary<int, string> _peerNames { get; set; } = new System.Collections.Generic.Dictionary<int, string>();

	private Godot.Collections.Array _clientsReady { get; set; } = new Godot.Collections.Array();

	public Godot.Collections.Array ClientsReady => _clientsReady;

	private List<string> _levelConfigAcked { get; set; } = new List<string>();

	private Godot.Collections.Array _gameEntryAcked { get; set; } = new Godot.Collections.Array();

	public Godot.Collections.Array GameEntryAcked => _gameEntryAcked;

	public bool GameEntrySent => _gameEntrySent;

	private System.Collections.Generic.Dictionary<string, double> _pingSendTimes { get; set; } = new System.Collections.Generic.Dictionary<string, double>();

	private System.Collections.Generic.Dictionary<string, int> _peerLatencies { get; set; } = new System.Collections.Generic.Dictionary<string, int>();

	public int CurrentPort => _currentPort;

	public string LastModCompatibilityFailure { get; private set; } = "";

	public Godot.Collections.Array BattleAdmittedPeers => new Godot.Collections.Array(((IEnumerable<string>)_battleAdmitted.OrderBy((string id) => id, StringComparer.Ordinal)).Select((Func<string, Variant>)((string id) => id)));

	public event AllClientsReadyEventHandler OnAllClientsReady;

	public event AllLevelConfigAckedEventHandler OnAllLevelConfigAcked;

	public event AllGameEntryAckedEventHandler OnAllGameEntryAcked;

	public event MatchCreatedEventHandler OnMatchCreated;

	public event MatchJoinedEventHandler OnMatchJoined;

	public event MatchLeftEventHandler OnMatchLeft;

	public event PeerJoinedEventHandler OnPeerJoined;

	public event PeerLeftEventHandler OnPeerLeft;

	public event MatchStateReceivedEventHandler OnMatchStateReceived;

	public event Action<NetMessageContext> OnNetworkMessageReceived;

	public event ConnectionChangedEventHandler OnConnectionChanged;

	public event PingUpdatedEventHandler OnPingUpdated;

	public event Action<string> OnModCompatibilityFailure;

	public void EmitAllClientsReady()
	{
		OnAllClientsReady?.Invoke();
	}

	public void EmitAllGameEntryAcked()
	{
		OnAllGameEntryAcked?.Invoke();
	}

	public override void _Process(double delta)
	{
		ProcessModCompatibility();
		if (!(currentMatchId == "") && _peer != null)
		{
			_pingTimer += delta;
			if (_pingTimer >= 2.0)
			{
				_pingTimer = 0.0;
				_SendPing();
			}
		}
	}

	public int GetPeerLatency(string peerIdStr)
	{
		return _peerLatencies.GetValueOrDefault(peerIdStr, 0);
	}

	public int GetSignalLevel(string peerIdStr)
	{
		int valueOrDefault = _peerLatencies.GetValueOrDefault(peerIdStr, 999);
		if (valueOrDefault <= 50)
		{
			return 5;
		}
		if (valueOrDefault <= 100)
		{
			return 4;
		}
		if (valueOrDefault <= 200)
		{
			return 3;
		}
		if (valueOrDefault <= 400)
		{
			return 2;
		}
		if (valueOrDefault <= 800)
		{
			return 1;
		}
		return 0;
	}

	private void _SendPing()
	{
		double value = Time.GetTicksMsec();
		string text = peerId + "_" + value;
		_pingSendTimes[text] = value;
		double num = Time.GetTicksMsec();
		List<string> list = new List<string>();
		foreach (string key in _pingSendTimes.Keys)
		{
			if (num - _pingSendTimes[key] > 10000.0)
			{
				list.Add(key);
			}
		}
		foreach (string item in list)
		{
			_pingSendTimes.Remove(item);
		}
		if (isHost)
		{
			_peerLatencies[peerId] = 0;
			OnPingUpdated?.Invoke(peerId, 0);
			{
				foreach (Variant matchMember in matchMembers)
				{
					string s = (string)matchMember;
					if (int.Parse(s) != 1)
					{
						RpcId(int.Parse(s), "_RpcPing", text);
					}
				}
				return;
			}
		}
		RpcId(1L, "_RpcPing", text);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void _RpcPing(string pingId)
	{
		string s = Multiplayer.GetRemoteSenderId().ToString();
		RpcId(int.Parse(s), "_RpcPong", pingId);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void _RpcPong(string pingId)
	{
		string key = Multiplayer.GetRemoteSenderId().ToString();
		if (!_pingSendTimes.ContainsKey(pingId))
		{
			return;
		}
		double num = _pingSendTimes[pingId];
		int num2 = (int)((double)Time.GetTicksMsec() - num);
		if (!isHost)
		{
			_pingSendTimes.Remove(pingId);
		}
		_peerLatencies[key] = num2;
		OnPingUpdated?.Invoke(key, num2);
		if (!isHost)
		{
			return;
		}
		Dictionary dictionary = new Dictionary();
		dictionary[peerId] = 0;
		foreach (Variant matchMember in matchMembers)
		{
			string text = (string)matchMember;
			dictionary[text] = _peerLatencies.GetValueOrDefault(text, 0);
		}
		Rpc("_RpcPingResult", Json.Stringify(dictionary));
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void _RpcPingResult(string data)
	{
		Variant variant = Json.ParseString(data);
		if (variant.VariantType != Variant.Type.Dictionary)
		{
			return;
		}
		Dictionary dictionary = (Dictionary)variant;
		foreach (Variant key in dictionary.Keys)
		{
			string text = (string)key;
			_peerLatencies[text] = (int)(long)dictionary[text];
			OnPingUpdated?.Invoke(text, (int)(long)dictionary[text]);
		}
	}

	public override void _Ready()
	{
		Instance = this;
		_networkMessageRouter.SetFallback(_HandleNetworkMessageFallback);
		Multiplayer.PeerConnected += _OnPeerConnected;
		Multiplayer.PeerDisconnected += _OnPeerDisconnected;
		Multiplayer.ConnectedToServer += _OnConnectedToServer;
		Multiplayer.ConnectionFailed += _OnConnectionFailed;
		Multiplayer.ServerDisconnected += _OnServerDisconnected;
	}

	public bool LogOut()
	{
		if (currentMatchId != "" || _peer != null)
		{
			LeaveMatch();
		}
		_playerName = "";
		peerId = "";
		matchMembers.Clear();
		_peerNames.Clear();
		_pingSendTimes.Clear();
		_peerLatencies.Clear();
		_lastUnreliableSequenceByStream.Clear();
		_ResetSessionState();
		return true;
	}

	public bool IsConnect()
	{
		if (_playerName != "")
		{
			return CanSendMatchRpc();
		}
		return false;
	}

	private bool CanSendMatchRpc()
	{
		if (currentMatchId == "")
		{
			return false;
		}
		MultiplayerPeer multiplayerPeer = Multiplayer?.MultiplayerPeer;
		if (GodotObject.IsInstanceValid(multiplayerPeer) && !(multiplayerPeer is OfflineMultiplayerPeer))
		{
			return multiplayerPeer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Connected;
		}
		return false;
	}

	public string GetUserName()
	{
		return _playerName;
	}

	public string GetUserDisplayName()
	{
		return _playerName;
	}

	public string GetPeerName(string peerIdStr)
	{
		if (!int.TryParse(peerIdStr, out var result))
		{
			return "P?";
		}
		if (_peerNames.ContainsKey(result))
		{
			return _peerNames[result];
		}
		return "P" + peerIdStr;
	}

	public bool CreateMatch()
	{
		if (currentMatchId != "" || _peer != null)
		{
			return false;
		}
		_currentPort = 7777;
		_peer = new ENetMultiplayerPeer();
		Error error = _peer.CreateServer(_currentPort, 4);
		if (error != Error.Ok)
		{
			_currentPort = 7778;
			_peer = new ENetMultiplayerPeer();
			error = _peer.CreateServer(_currentPort, 4);
		}
		if (error != Error.Ok)
		{
			BroadCastManager.Instance.BroadCastFloatCreate("CREATE_MATCH_FAILED", Colors.Red);
			_peer = null;
			return false;
		}
		Multiplayer.MultiplayerPeer = _peer;
		isHost = true;
		currentMatchId = "host";
		peerId = Multiplayer.GetUniqueId().ToString();
		_playerName = ((GameSaveManager.Instance.GetUserCurrent() != "") ? GameSaveManager.Instance.GetUserCurrent() : "Host");
		Global.Instance.isMultiplayerMode = true;
		Global.Instance.isMultiplayerHost = true;
		matchMembers.Clear();
		matchMembers.Add(peerId);
		_peerNames.Clear();
		_sessionGeneration++;
		_startGameTransitionPending = false;
		_startGameTransitionCompleted = false;
		_ResetSessionState();
		OnMatchCreated?.Invoke(currentMatchId);
		BroadCastManager.Instance.BroadCastFloatCreate("ROOM_CREATED", Colors.Green);
		return true;
	}

	public bool JoinMatch(string address)
	{
		if (currentMatchId != "" || _peer != null || !TryParseMatchEndpoint(address, out var host, out var port))
		{
			BroadCastManager.Instance.BroadCastFloatCreate("JOIN_MATCH_FAILED", Colors.Red);
			return false;
		}
		_peer = new ENetMultiplayerPeer();
		if (_peer.CreateClient(host, port) != Error.Ok)
		{
			BroadCastManager.Instance.BroadCastFloatCreate("JOIN_MATCH_FAILED", Colors.Red);
			_peer = null;
			return false;
		}
		Multiplayer.MultiplayerPeer = _peer;
		isHost = false;
		currentMatchId = address;
		peerId = Multiplayer.GetUniqueId().ToString();
		_playerName = ((GameSaveManager.Instance.GetUserCurrent() != "") ? GameSaveManager.Instance.GetUserCurrent() : "Client");
		Global.Instance.isMultiplayerMode = true;
		Global.Instance.isMultiplayerHost = false;
		matchMembers.Clear();
		matchMembers.Add(peerId);
		_peerNames.Clear();
		_sessionGeneration++;
		_startGameTransitionPending = false;
		_startGameTransitionCompleted = false;
		_ResetSessionState();
		return true;
	}

	internal static bool TryParseMatchEndpoint(string address, out string host, out int port)
	{
		host = "";
		port = 7777;
		if (string.IsNullOrWhiteSpace(address) || address.Length > 512)
		{
			return false;
		}
		string text = address.Trim();
		if (text.StartsWith("[", StringComparison.Ordinal))
		{
			int num = text.IndexOf(']');
			if (num <= 1)
			{
				return false;
			}
			host = text.Substring(1, num - 1);
			string text2 = text;
			int num2 = num + 1;
			string text3 = text2.Substring(num2, text2.Length - num2);
			if (text3.Length == 0)
			{
				return true;
			}
			if (text3.StartsWith(":", StringComparison.Ordinal))
			{
				text2 = text3;
				return TryParsePort(text2.Substring(1, text2.Length - 1), out port);
			}
			return false;
		}
		int num3 = text.IndexOf(':');
		int num4 = text.LastIndexOf(':');
		if (num3 < 0)
		{
			host = text;
			return host.Length > 0;
		}
		if (num3 != num4)
		{
			host = text;
			return true;
		}
		host = text.Substring(0, num4);
		if (host.Length > 0)
		{
			string text2 = text;
			int num2 = num4 + 1;
			return TryParsePort(text2.Substring(num2, text2.Length - num2), out port);
		}
		return false;
	}

	private static bool TryParsePort(string value, out int port)
	{
		if (int.TryParse(value, out port))
		{
			int num = port;
			if (num >= 1)
			{
				return num <= 65535;
			}
			return false;
		}
		return false;
	}

	public void LeaveMatch()
	{
		_CleanupConnection();
	}

	public void SendMatchState(string opCode, string data)
	{
		if (!CanSendMatchRpc())
		{
			return;
		}
		string text = _CreateEnvelopeData(opCode, data, NetDeliveryMode.Reliable, peerId);
		if (!(text == ""))
		{
			if (isHost)
			{
				Rpc("_RpcReceiveMatchState", "__net_v2", text);
				_ProcessMatchState(opCode, data, peerId);
			}
			else
			{
				RpcId(1L, "_RpcReceiveMatchState", "__net_v2", text);
			}
		}
	}

	public void SendMatchStateUnreliable(string opCode, string data)
	{
		if (!CanSendMatchRpc())
		{
			return;
		}
		string text = _CreateEnvelopeData(opCode, data, NetDeliveryMode.Unreliable, peerId);
		if (!(text == ""))
		{
			if (isHost)
			{
				Rpc("_RpcReceiveMatchStateUnreliable", "__net_v2", text);
				_ProcessMatchState(opCode, data, peerId);
			}
			else
			{
				RpcId(1L, "_RpcReceiveMatchStateUnreliable", "__net_v2", text);
			}
		}
	}

	public void SendMatchStateToPeer(string opCode, string data, string targetPeerId)
	{
		if (isHost && !string.IsNullOrWhiteSpace(targetPeerId) && !(targetPeerId == peerId))
		{
			NetMessageType netMessageType = NetProtocol.FromLegacyOpCode(opCode);
			if (netMessageType == NetMessageType.Unknown)
			{
				GD.PushWarning($"Cannot send unknown network opcode '{opCode}' to peer {targetPeerId}.");
			}
			else
			{
				SendEnvelope(new NetMessageEnvelope
				{
					message_type = netMessageType,
					payload = (data ?? ""),
					sender_peer_id = peerId
				}, NetDeliveryMode.Reliable, targetPeerId);
			}
		}
	}

	public void SendEnvelope(NetMessageEnvelope envelope, NetDeliveryMode deliveryMode, string targetPeerId = "")
	{
		if (!CanSendMatchRpc() || envelope == null)
		{
			return;
		}
		_PrepareOutgoingEnvelope(envelope, deliveryMode);
		if (!NetProtocolSerializer.TrySerialize(envelope, out var data))
		{
			return;
		}
		string text = ((deliveryMode == NetDeliveryMode.Unreliable) ? "_RpcReceiveMatchStateUnreliable" : "_RpcReceiveMatchState");
		bool flag = int.TryParse(targetPeerId, out var result);
		if (isHost)
		{
			if (flag && targetPeerId != peerId)
			{
				RpcId(result, text, "__net_v2", data);
			}
			else if (!flag)
			{
				Rpc(text, "__net_v2", data);
			}
			if (!flag || targetPeerId == peerId)
			{
				_DispatchLocalEnvelope(envelope, deliveryMode);
			}
		}
		else
		{
			RpcId(1L, text, "__net_v2", data);
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void _RpcReceiveMatchState(string opCode, string data)
	{
		string text = Multiplayer.GetRemoteSenderId().ToString();
		if (!_TryDispatchEnvelope(opCode, data, text, NetDeliveryMode.Reliable))
		{
			_RejectLegacyProtocol(text);
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void _RpcReceiveMatchStateUnreliable(string opCode, string data)
	{
		string text = Multiplayer.GetRemoteSenderId().ToString();
		if (!_TryHandleUnreliableEnvelope(opCode, data, text))
		{
			_RejectLegacyProtocol(text);
		}
	}

	private string _CreateEnvelopeData(string opCode, string data, NetDeliveryMode deliveryMode, string senderPeerId, bool forwarded = false)
	{
		if (!NetProtocolSerializer.TrySerialize(new NetMessageEnvelope
		{
			protocol_version = 4,
			message_type = NetProtocol.FromLegacyOpCode(opCode),
			sender_peer_id = senderPeerId,
			sequence = ((deliveryMode == NetDeliveryMode.Unreliable) ? (++_unreliableSequence) : (++_reliableSequence)),
			game_tick = ((TowerDefenseManager.Instance != null) ? TowerDefenseManager.Instance.runGameTime : ((double)Time.GetTicksMsec() / 1000.0)),
			payload_format = "json",
			payload = (data ?? ""),
			forwarded = forwarded
		}, out var data2))
		{
			return "";
		}
		return data2;
	}

	private void _PrepareOutgoingEnvelope(NetMessageEnvelope envelope, NetDeliveryMode deliveryMode)
	{
		envelope.protocol_version = 4;
		if (string.IsNullOrEmpty(envelope.sender_peer_id))
		{
			envelope.sender_peer_id = peerId;
		}
		if (envelope.sequence <= 0)
		{
			envelope.sequence = ((deliveryMode == NetDeliveryMode.Unreliable) ? (++_unreliableSequence) : (++_reliableSequence));
		}
		else if (deliveryMode == NetDeliveryMode.Unreliable)
		{
			_unreliableSequence = Math.Max(_unreliableSequence, envelope.sequence);
		}
		else
		{
			_reliableSequence = Math.Max(_reliableSequence, envelope.sequence);
		}
		if (envelope.game_tick <= 0.0)
		{
			envelope.game_tick = ((TowerDefenseManager.Instance != null) ? TowerDefenseManager.Instance.runGameTime : ((double)Time.GetTicksMsec() / 1000.0));
		}
		if (string.IsNullOrEmpty(envelope.payload_format))
		{
			envelope.payload_format = "json";
		}
	}

	private void _DispatchLocalEnvelope(NetMessageEnvelope envelope, NetDeliveryMode deliveryMode)
	{
		NetMessageContext netMessageContext = new NetMessageContext(envelope, peerId, deliveryMode);
		OnNetworkMessageReceived?.Invoke(netMessageContext);
		_networkMessageRouter.Dispatch(netMessageContext);
	}

	private bool _TryDispatchEnvelope(string opCode, string data, string rpcSenderId, NetDeliveryMode deliveryMode)
	{
		if (opCode != "__net_v2")
		{
			return false;
		}
		if (!NetProtocolSerializer.TryDeserialize(data, out var envelope))
		{
			return true;
		}
		if (envelope.protocol_version != 4)
		{
			_RejectProtocolVersion(rpcSenderId, envelope.protocol_version);
			return true;
		}
		if (!IsEnvelopeSenderAuthorized(envelope.message_type, rpcSenderId) || !AllowsModMessage(envelope.message_type, rpcSenderId))
		{
			return true;
		}
		NetMessageContext netMessageContext = new NetMessageContext(envelope, rpcSenderId, deliveryMode);
		OnNetworkMessageReceived?.Invoke(netMessageContext);
		_networkMessageRouter.Dispatch(netMessageContext);
		return true;
	}

	private bool _TryHandleUnreliableEnvelope(string opCode, string data, string rpcSenderId)
	{
		if (opCode != "__net_v2")
		{
			return false;
		}
		if (!NetProtocolSerializer.TryDeserialize(data, out var envelope))
		{
			return true;
		}
		if (envelope.protocol_version != 4)
		{
			_RejectProtocolVersion(rpcSenderId, envelope.protocol_version);
			return true;
		}
		if (!IsEnvelopeSenderAuthorized(envelope.message_type, rpcSenderId) || !AllowsModMessage(envelope.message_type, rpcSenderId))
		{
			return true;
		}
		if (isHost && envelope.message_type == NetMessageType.CursorState)
		{
			envelope.payload = NormalizeAuthenticatedUserId(envelope.payload, rpcSenderId);
		}
		NetMessageContext netMessageContext = new NetMessageContext(envelope, rpcSenderId, NetDeliveryMode.Unreliable);
		string key = netMessageContext.AuthenticatedSenderPeerId + ":" + envelope.message_type;
		if (_lastUnreliableSequenceByStream.TryGetValue(key, out var value) && envelope.sequence <= value)
		{
			return true;
		}
		_lastUnreliableSequenceByStream[key] = envelope.sequence;
		OnNetworkMessageReceived?.Invoke(netMessageContext);
		string text = NetProtocol.ToLegacyOpCode(envelope.message_type);
		if (text == "")
		{
			return true;
		}
		if (isHost)
		{
			envelope.forwarded = true;
			if (!NetProtocolSerializer.TrySerialize(envelope, out var data2))
			{
				return true;
			}
			Rpc("_RpcReceiveMatchStateUnreliable", "__net_v2", data2);
		}
		OnMatchStateReceived?.Invoke(text, envelope.payload, netMessageContext.AuthenticatedSenderPeerId);
		return true;
	}

	private static bool IsEnvelopeSenderAuthorized(NetMessageType messageType, string rpcSenderId)
	{
		if (!RequiresHostAuthority(messageType))
		{
			return true;
		}
		return rpcSenderId == "1";
	}

	private static bool RequiresHostAuthority(NetMessageType messageType)
	{
		switch (messageType)
		{
		case NetMessageType.StartGame:
		case NetMessageType.CommandRejected:
		case NetMessageType.GameStateSync:
		case NetMessageType.GameResult:
		case NetMessageType.SpawnZombie:
		case NetMessageType.SpawnGrid:
		case NetMessageType.CharacterDestroy:
		case NetMessageType.CharacterInit:
		case NetMessageType.CharacterStateDelta:
		case NetMessageType.CharacterPositionSync:
		case NetMessageType.Pause:
		case NetMessageType.Resume:
		case NetMessageType.SelectLevel:
		case NetMessageType.LevelConfig:
		case NetMessageType.ChooseOver:
		case NetMessageType.VaseBreak:
		case NetMessageType.PacketSpawn:
		case NetMessageType.SpawnCoin:
		case NetMessageType.SpawnFallingObject:
		case NetMessageType.ZombieStateDelta:
		case NetMessageType.SpawnCharacterAt:
		case NetMessageType.ConveyorSpawn:
		case NetMessageType.GameEntry:
		case NetMessageType.TipsPlay:
		case NetMessageType.DamagePart:
		case NetMessageType.DamagePointReach:
		case NetMessageType.ArmorDamagePointReach:
		case NetMessageType.ArmorHitpointsEmpty:
		case NetMessageType.CraterCreate:
		case NetMessageType.PlantGridSnapshot:
		case NetMessageType.EventExecute:
		case NetMessageType.WaveEventExecute:
		case NetMessageType.BattleCharacterRoster:
		case NetMessageType.BattlePacketSnapshot:
		case NetMessageType.BattleSnapshotEntitiesReady:
		case NetMessageType.BattleSnapshotComplete:
		case NetMessageType.ProjectileEffectSpawn:
		case NetMessageType.CharacterComponentOperation:
			return true;
		default:
			return false;
		}
	}

	private static string NormalizeAuthenticatedUserId(string data, string authenticatedSenderId)
	{
		Variant variant = Json.ParseString(data);
		if (variant.VariantType != Variant.Type.Dictionary)
		{
			return data;
		}
		Dictionary dictionary = variant.AsGodotDictionary();
		dictionary["user_id"] = authenticatedSenderId;
		return Json.Stringify(dictionary);
	}

	private void _HandleNetworkMessageFallback(NetMessageContext context)
	{
		if (context != null)
		{
			string legacyOpCode = context.LegacyOpCode;
			if (!(legacyOpCode == ""))
			{
				_ProcessMatchState(legacyOpCode, context.Payload, context.AuthenticatedSenderPeerId);
			}
		}
	}

	private void _BroadcastReliableEnvelope(string opCode, string data, string senderPeerId, bool forwarded = true)
	{
		string text = _CreateEnvelopeData(opCode, data, NetDeliveryMode.Reliable, senderPeerId, forwarded);
		if (!(text == ""))
		{
			Rpc("_RpcReceiveMatchState", "__net_v2", text);
		}
	}

	private void _RejectLegacyProtocol(string senderId)
	{
		if (isHost && int.TryParse(senderId, out var result) && result != 1)
		{
			_peer?.DisconnectPeer(result);
		}
	}

	private void _RejectProtocolVersion(string senderId, int remoteProtocolVersion)
	{
		BroadCastManager.Instance.BroadCastFloatCreate($"Protocol mismatch: local v{4}, remote v{remoteProtocolVersion}", Colors.Red);
		if (isHost && int.TryParse(senderId, out var result) && result != 1)
		{
			_peer?.DisconnectPeer(result);
		}
	}

	public void _ProcessMatchState(string opCode, string data, string senderId)
	{
		if (!IsEnvelopeSenderAuthorized(NetProtocol.FromLegacyOpCode(opCode), senderId) || !AllowsModMessage(NetProtocol.FromLegacyOpCode(opCode), senderId))
		{
			return;
		}
		if (isHost && senderId != peerId && (opCode == "choose_ready" || opCode == "cursor_pick_sync"))
		{
			data = NormalizeAuthenticatedUserId(data, senderId);
		}
		switch (opCode)
		{
		case "start_game":
			if (senderId == "1")
			{
				_HandleStartGame(data);
			}
			return;
		case "select_level":
			if (!isHost && senderId == "1")
			{
				_HandleSelectLevel(data);
			}
			return;
		case "level_config":
			if (!isHost && senderId == "1")
			{
				_HandleLevelConfig(data);
			}
			return;
		case "level_config_ack":
			ReceiveModAck(senderId, data);
			return;
		case "choose_ready":
			if (isHost && senderId != peerId)
			{
				_BroadcastReliableEnvelope(opCode, data, senderId);
			}
			OnMatchStateReceived?.Invoke(opCode, data, senderId);
			return;
		case "choose_over":
			OnMatchStateReceived?.Invoke(opCode, data, senderId);
			return;
		case "use_shovel":
		case "remove_plant":
		case "packet_pick":
		case "place_plant":
		case "move_character":
		case "vase_break_request":
		case "gem_match_command":
			OnMatchStateReceived?.Invoke(opCode, data, senderId);
			return;
		case "command_rejected":
			OnMatchStateReceived?.Invoke(opCode, data, senderId);
			return;
		case "client_ready":
		case "game_entry_ack":
			if (isHost && senderId != peerId)
			{
				OnMatchStateReceived?.Invoke(opCode, data, senderId);
			}
			return;
		case "wave_event_execute":
		case "projectile_effect_spawn":
		case "event_execute":
		case "character_component_operation":
			if (!isHost)
			{
				OnMatchStateReceived?.Invoke(opCode, data, senderId);
			}
			return;
		case "battle_snapshot_request":
			if (isHost && senderId != peerId)
			{
				OnMatchStateReceived?.Invoke(opCode, data, senderId);
			}
			return;
		}
		if (senderId != peerId)
		{
			if (isHost)
			{
				_BroadcastReliableEnvelope(opCode, data, senderId);
			}
			OnMatchStateReceived?.Invoke(opCode, data, senderId);
		}
	}

	public void SendSelectLevel(string levelId)
	{
		if (isHost)
		{
			_hostModBatch?.Cancel("关卡选择变化，请重新确认。");
			selectedLevelId = levelId;
			XWModLevelIdentity selectedLevelIdentity = GetSelectedLevelIdentity();
			string difficulty = selectedLevelIdentity.Difficulty;
			string text = (selectedLevelIdentity.IsMod ? "" : _FindLevelUid(levelId, difficulty));
			Dictionary dictionary = new Dictionary
			{
				{ "level_id", levelId },
				{ "user_id", peerId },
				{
					"level_identity",
					selectedLevelIdentity.ToDictionary()
				},
				{ "difficult", difficulty },
				{ "level_uid", text }
			};
			SendMatchState("select_level", Json.Stringify(dictionary));
		}
	}

	public void SendLevelConfig()
	{
		StartCompatibleGameAsync();
	}

	private string BuildLevelConfigData()
	{
		TowerDefenseLevelBaseConfig currentLevelConfig = TowerDefenseManager.Instance.currentLevelConfig;
		if (!GodotObject.IsInstanceValid(currentLevelConfig))
		{
			return "";
		}
		string text = "";
		string text2 = "";
		if (currentLevelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig)
		{
			text = "TowerDefenseLevelConfig";
			if (towerDefenseLevelConfig.data != null && towerDefenseLevelConfig.data.Data.VariantType == Variant.Type.Dictionary)
			{
				text2 = Json.Stringify((Dictionary)towerDefenseLevelConfig.data.Data);
			}
			else
			{
				towerDefenseLevelConfig.ExportToFeatureProcess();
				text2 = Json.Stringify(new Dictionary
				{
					{
						"Feature",
						_SerializeFeatures(_ConvertFeatureData(towerDefenseLevelConfig.featureData))
					},
					{
						"Process",
						new Dictionary
						{
							{ "Name", towerDefenseLevelConfig.processName },
							{ "Data", towerDefenseLevelConfig.processData }
						}
					},
					{ "Name", towerDefenseLevelConfig.name },
					{ "LevelName", towerDefenseLevelConfig.levelName },
					{ "Description", towerDefenseLevelConfig.description },
					{ "LevelNumber", towerDefenseLevelConfig.levelNumber },
					{ "baseTimeScale", towerDefenseLevelConfig.baseTimeScale }
				});
			}
		}
		else
		{
			if (!(currentLevelConfig is TowerDefenseLevelNewConfig towerDefenseLevelNewConfig))
			{
				return "";
			}
			text = "TowerDefenseLevelNewConfig";
			text2 = ((towerDefenseLevelNewConfig.data == null || towerDefenseLevelNewConfig.data.Data.VariantType != Variant.Type.Dictionary) ? Json.Stringify(new Dictionary
			{
				{
					"Feature",
					_SerializeFeatures(_ConvertFeatureData(towerDefenseLevelNewConfig.featureData))
				},
				{
					"Process",
					new Dictionary
					{
						{ "Name", towerDefenseLevelNewConfig.processName },
						{ "Data", towerDefenseLevelNewConfig.processData }
					}
				},
				{ "Name", towerDefenseLevelNewConfig.name },
				{ "LevelName", towerDefenseLevelNewConfig.levelName },
				{ "Description", towerDefenseLevelNewConfig.description },
				{ "LevelNumber", towerDefenseLevelNewConfig.levelNumber },
				{ "Version", towerDefenseLevelNewConfig.version }
			}) : Json.Stringify((Dictionary)towerDefenseLevelNewConfig.data.Data));
		}
		return Json.Stringify(new Dictionary
		{
			{ "config_type", text },
			{
				"level_identity",
				GetSelectedLevelIdentity().ToDictionary()
			},
			{ "config_json", text2 },
			{
				"enter_level_mode",
				Global.Instance.enterLevelMode
			},
			{
				"enter_level_is_battle",
				Global.Instance.enterLevelIsBattle
			}
		});
	}

	private Dictionary _ConvertFeatureData(Godot.Collections.Dictionary<StringName, Dictionary> featureData)
	{
		Dictionary dictionary = new Dictionary();
		foreach (KeyValuePair<StringName, Dictionary> featureDatum in featureData)
		{
			dictionary[featureDatum.Key] = featureDatum.Value;
		}
		return dictionary;
	}

	private Godot.Collections.Array _SerializeFeatures(Dictionary featureData)
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (Variant key in featureData.Keys)
		{
			string text = (string)key;
			array.Add(new Dictionary
			{
				{ "Name", text },
				{
					"Data",
					featureData[text]
				}
			});
		}
		return array;
	}

	private string _FindLevelUid(string levelId, string difficult)
	{
		foreach (string key in ResourceManager.Instance.LEVELS.Keys)
		{
			Dictionary dictionary = ResourceManager.Instance.LEVELS[key];
			if (dictionary == null || !dictionary.ContainsKey("Chapter"))
			{
				continue;
			}
			foreach (Variant item in (Godot.Collections.Array)dictionary["Chapter"])
			{
				Dictionary dictionary2 = (Dictionary)item;
				if (!dictionary2.ContainsKey("Level"))
				{
					continue;
				}
				foreach (Variant item2 in (Godot.Collections.Array)dictionary2["Level"])
				{
					Dictionary dictionary3 = (Dictionary)item2;
					if ((string)dictionary3.GetValueOrDefault("SaveKey", "") != levelId)
					{
						continue;
					}
					Dictionary dictionary4 = (Dictionary)dictionary3.GetValueOrDefault("Level", new Dictionary());
					if (dictionary4 != null && dictionary4.Count > 0)
					{
						string text = (string)dictionary4.GetValueOrDefault(difficult, "");
						if (text != "" && ResourceLoader.Exists(text))
						{
							return text;
						}
						text = (string)dictionary4.GetValueOrDefault("Normal", "");
						if (text != "" && ResourceLoader.Exists(text))
						{
							return text;
						}
					}
				}
			}
		}
		return "";
	}

	public void SendStartGame()
	{
		if (!isHost)
		{
			return;
		}
		ModCompatibilityBatch hostModBatch = _hostModBatch;
		if (hostModBatch == null || !hostModBatch.Accepted)
		{
			return;
		}
		XWModEnvironmentService.EnvironmentLease modEnvironmentLease = _modEnvironmentLease;
		if (modEnvironmentLease != null && modEnvironmentLease.IsCurrent && !(_acceptedModBatch != _hostModBatch.Id))
		{
			XWModLevelIdentity acceptedLevelIdentity = _acceptedLevelIdentity;
			if (!(acceptedLevelIdentity == null))
			{
				string difficulty = acceptedLevelIdentity.Difficulty;
				string text = "";
				Dictionary dictionary = new Dictionary
				{
					{ "config_batch_id", _acceptedModBatch },
					{
						"level_identity",
						acceptedLevelIdentity.ToDictionary()
					},
					{
						"snapshot_sha256",
						_modEnvironmentLease.Status.Snapshot.Sha256
					},
					{ "battle_id", _modBattleId },
					{ "admitted_peers", BattleAdmittedPeers },
					{ "level_id", acceptedLevelIdentity.LevelSaveKey },
					{ "difficult", difficulty },
					{ "level_uid", text }
				};
				_activeBattleStartData = Json.Stringify(dictionary);
				SendMatchState("start_game", _activeBattleStartData);
			}
		}
	}

	private void SendLateJoinBootstrap(string targetPeerId)
	{
		if (isHost && targetPeerId != "")
		{
			SendCompatibleLateJoinAsync(targetPeerId);
		}
	}

	public void SendPlacePlant(string plantName, int gridX, int gridY, int syncId = -1, string overrideData = "", string requestId = "", string ownerPeerId = "", string placementKind = "grid", int targetSyncId = -1, bool hypnoses = false)
	{
		string text = (string.IsNullOrWhiteSpace(ownerPeerId) ? peerId : ownerPeerId);
		Dictionary dictionary = new Dictionary
		{
			{ "plant_name", plantName },
			{ "grid_x", gridX },
			{ "grid_y", gridY },
			{ "sync_id", syncId },
			{ "user_id", peerId },
			{ "owner_peer_id", text },
			{ "override_data", overrideData },
			{ "placement_kind", placementKind }
		};
		if (targetSyncId >= 0)
		{
			dictionary["target_sync_id"] = targetSyncId;
			dictionary["hypnoses"] = hypnoses;
		}
		if (requestId != "")
		{
			dictionary["request_id"] = requestId;
		}
		if (isHost && placementKind == "jala_vase" && TowerDefenseManager.Instance.currentControl._syncCharacters.TryGetValue(targetSyncId, out var value) && value is TowerDefensePlantJalaVase towerDefensePlantJalaVase && GodotObject.IsInstanceValid(towerDefensePlantJalaVase))
		{
			dictionary["target_state"] = towerDefensePlantJalaVase.ExportVariantSave();
		}
		SendMatchState("place_plant", Json.Stringify(dictionary));
	}

	public void SendCommandRejected(string targetPeerId, string rejectedOpCode, string requestId, string reason)
	{
		if (isHost && !(targetPeerId == ""))
		{
			Dictionary dictionary = new Dictionary
			{
				{ "op_code", rejectedOpCode },
				{ "request_id", requestId },
				{ "reason", reason },
				{ "target_user_id", targetPeerId },
				{ "user_id", peerId }
			};
			SendEnvelope(new NetMessageEnvelope
			{
				message_type = NetMessageType.CommandRejected,
				payload = Json.Stringify(dictionary)
			}, NetDeliveryMode.Reliable, targetPeerId);
		}
	}

	public void SendRemovePlant(int gridX, int gridY)
	{
		Dictionary dictionary = new Dictionary
		{
			{ "grid_x", gridX },
			{ "grid_y", gridY },
			{ "user_id", peerId }
		};
		SendMatchState("remove_plant", Json.Stringify(dictionary));
	}

	public void SendUseShovel(int gridX, int gridY)
	{
		Dictionary dictionary = new Dictionary
		{
			{ "grid_x", gridX },
			{ "grid_y", gridY },
			{ "user_id", peerId }
		};
		SendMatchState("use_shovel", Json.Stringify(dictionary));
	}

	public void SendMoveCharacter(int syncId, Vector2I fromGridPos, Vector2I toGridPos, string moveKind = "drag", double duration = 0.5)
	{
		if (syncId >= 0 && !(fromGridPos == toGridPos))
		{
			moveKind = (string.IsNullOrWhiteSpace(moveKind) ? "drag" : moveKind);
			duration = ((moveKind == "magnet") ? 2.0 : 0.5);
			Dictionary dictionary = new Dictionary
			{
				{ "sync_id", syncId },
				{ "from_grid_x", fromGridPos.X },
				{ "from_grid_y", fromGridPos.Y },
				{ "to_grid_x", toGridPos.X },
				{ "to_grid_y", toGridPos.Y },
				{ "move_kind", moveKind },
				{ "duration", duration },
				{ "user_id", peerId }
			};
			SendMatchState("move_character", Json.Stringify(dictionary));
		}
	}

	public void SendGameStateSync(Dictionary state)
	{
		if (isHost)
		{
			SendMatchState("game_state_sync", Json.Stringify(state));
		}
	}

	public void SendSpawnZombie(string zombieName, int line, double offsetX, int syncId, string spawnOverrideData = "", string spawnConfigOverrideData = "")
	{
		if (isHost)
		{
			Dictionary dictionary = new Dictionary
			{
				{ "zombie_name", zombieName },
				{ "line", line },
				{ "offset_x", offsetX },
				{ "sync_id", syncId },
				{ "user_id", peerId },
				{ "spawn_override", spawnOverrideData },
				{ "spawn_config_override", spawnConfigOverrideData }
			};
			SendMatchState("spawn_zombie", Json.Stringify(dictionary));
		}
	}

	public void SendSpawnGrid(string packetName, int gridX, int gridY, int syncId)
	{
		if (isHost)
		{
			Dictionary dictionary = new Dictionary
			{
				{ "packet_name", packetName },
				{ "grid_x", gridX },
				{ "grid_y", gridY },
				{ "sync_id", syncId },
				{ "user_id", peerId }
			};
			SendMatchState("spawn_grid", Json.Stringify(dictionary));
		}
	}

	public void SendGameResult(bool victory)
	{
		if (isHost)
		{
			SendMatchState("game_result", MatchStateSerializer.Serialize(new GameResultDto
			{
				victory = victory
			}));
		}
	}

	public void SendCharacterDestroy(int syncId, bool isExplode = false, bool isSmash = false)
	{
		if (isHost)
		{
			SendMatchState("character_destroy", MatchStateSerializer.Serialize(new CharacterDestroyDto
			{
				sync_id = syncId,
				is_explode = isExplode,
				is_smash = isSmash
			}));
		}
	}

	public void SendCharacterInit(int syncId, double posX, double posY, double hp, bool die, string clipName = "", bool loopAnim = true, double blendTimeVal = 0.0, int frameIndexVal = 0, double timeScaleVal = 1.0, double walkSpeedScaleVal = 1.0)
	{
		if (isHost)
		{
			Dictionary dictionary = new Dictionary
			{
				{ "sync_id", syncId },
				{ "x", posX },
				{ "y", posY },
				{ "hp", hp },
				{ "die", die },
				{ "clip", clipName },
				{ "loop", loopAnim },
				{ "blendTime", blendTimeVal },
				{ "frame", frameIndexVal },
				{ "timeScale", timeScaleVal },
				{ "walkSpeedScale", walkSpeedScaleVal }
			};
			SendMatchState("character_init", Json.Stringify(dictionary));
		}
	}

	public void SendCharacterStateSync(string charactersData)
	{
		if (isHost)
		{
			SendMatchState("character_state_sync", charactersData);
		}
	}

	public void SendCharacterPositionSync(int syncId, double posX, double posY)
	{
		if (isHost)
		{
			SendMatchStateUnreliable("character_position_sync", Json.Stringify(new Dictionary
			{
				{ "sync_id", syncId },
				{ "x", posX },
				{ "y", posY }
			}));
		}
	}

	public void SendZombieFullSync(string zombiesData)
	{
		if (isHost)
		{
			SendMatchStateUnreliable("zombie_full_sync", zombiesData);
		}
	}

	public void SendCursorSync(double posX, double posY)
	{
		SendMatchStateUnreliable("cursor_sync", Json.Stringify(new Dictionary
		{
			{ "x", posX },
			{ "y", posY },
			{ "user_id", peerId }
		}));
	}

	public void SendCursorPickSync(string pickType, string pickName)
	{
		SendMatchState("cursor_pick_sync", Json.Stringify(new Dictionary
		{
			{ "pickType", pickType },
			{ "pickName", pickName },
			{ "user_id", peerId }
		}));
	}

	public void SendChooseReady()
	{
		SendMatchState("choose_ready", Json.Stringify(new Dictionary { { "user_id", peerId } }));
	}

	public void SendChooseOver()
	{
		if (isHost)
		{
			SendMatchState("choose_over", Json.Stringify(new Dictionary()));
		}
	}

	public void SendVaseBreakRequest(int gridX, int gridY)
	{
		Dictionary dictionary = new Dictionary
		{
			{ "grid_x", gridX },
			{ "grid_y", gridY },
			{ "user_id", peerId }
		};
		SendMatchState("vase_break_request", Json.Stringify(dictionary));
	}

	public void SendGemMatchCommand(string action, Vector2I from = default(Vector2I), Vector2I to = default(Vector2I), string upgradeKey = "")
	{
		if (!string.IsNullOrWhiteSpace(action))
		{
			Dictionary dictionary = new Dictionary
			{
				["action"] = action,
				["from_x"] = from.X,
				["from_y"] = from.Y,
				["to_x"] = to.X,
				["to_y"] = to.Y,
				["upgrade_key"] = upgradeKey ?? "",
				["user_id"] = peerId
			};
			SendMatchState("gem_match_command", Json.Stringify(dictionary));
		}
	}

	public void SendVaseBreakResult(Dictionary breakData)
	{
		if (isHost)
		{
			SendMatchState("vase_break", Json.Stringify(breakData));
		}
	}

	public void SendPacketSpawn(int syncId, string packetName, double posX, double posY, double aliveTime, bool isFall, bool useCost, double velocityX, double velocityY, int zIndex, double fallHeight = 0.0, TowerDefensePacketConfig packetConfig = null, EconomyAccountId sunAccountId = default(EconomyAccountId))
	{
		if (isHost)
		{
			Dictionary dictionary = new Dictionary
			{
				{ "sync_id", syncId },
				{ "packet_name", packetName },
				{ "pos_x", posX },
				{ "pos_y", posY },
				{ "alive_time", aliveTime },
				{ "is_fall", isFall },
				{ "use_cost", useCost },
				{ "velocity_x", velocityX },
				{ "velocity_y", velocityY },
				{ "z_index", zIndex },
				{ "fall_height", fallHeight },
				{
					"sun_account",
					sunAccountId.ToString()
				}
			};
			if (GodotObject.IsInstanceValid(packetConfig))
			{
				TowerDefensePacketRuntimeState.Write(packetConfig, dictionary, "packet_override", "can_change_cost", "change_cost_list");
			}
			SendMatchState("packet_spawn", Json.Stringify(dictionary));
		}
	}

	public void SendPacketPick(int syncId, string pickType = "remove")
	{
		Dictionary dictionary = new Dictionary
		{
			{ "sync_id", syncId },
			{ "pick_type", pickType },
			{ "user_id", peerId }
		};
		SendMatchState("packet_pick", Json.Stringify(dictionary));
	}

	public void SendPause()
	{
		SendMatchState("pause", Json.Stringify(new Dictionary { { "user_id", peerId } }));
	}

	public void SendResume()
	{
		SendMatchState("resume", Json.Stringify(new Dictionary { { "user_id", peerId } }));
	}

	public void SendSpawnCoin(double posX, double posY, int num, double velocityX, double velocityY, double gravity, double height, bool collect)
	{
		if (isHost)
		{
			Dictionary dictionary = new Dictionary
			{
				{ "pos_x", posX },
				{ "pos_y", posY },
				{ "num", num },
				{ "velocity_x", velocityX },
				{ "velocity_y", velocityY },
				{ "gravity", gravity },
				{ "height", height },
				{ "collect", collect }
			};
			SendMatchState("spawn_coin", Json.Stringify(dictionary));
		}
	}

	public void SendSpawnFallingObject(ObjectManagerConfig.OBJECT objectId, double posX, double posY, double velocityX, double velocityY, double gravity, double height, int gridX, int gridY)
	{
		if (isHost && objectId != ObjectManagerConfig.OBJECT.NOONE)
		{
			long num = ++_fallingObjectSpawnSequence;
			Dictionary dictionary = new Dictionary
			{
				{
					"object_id",
					(int)objectId
				},
				{ "spawn_sequence", num },
				{ "pos_x", posX },
				{ "pos_y", posY },
				{ "velocity_x", velocityX },
				{ "velocity_y", velocityY },
				{ "gravity", gravity },
				{ "height", height },
				{ "grid_x", gridX },
				{ "grid_y", gridY }
			};
			SendMatchState("spawn_falling_object", Json.Stringify(dictionary));
		}
	}

	public void SendSpawnCharacterAt(string packetName, int gridX, int gridY, int syncId, double hitpointScale = 1.0, double scaleVal = 1.0, bool hypnoses = false, double riseDuration = 0.0, bool useCreate = false, double posX = 0.0, double posY = 0.0, bool walkAfterSpawn = false, double groundHeight = 0.0, string sizeVal = "", Dictionary spawnState = null, string economyOwner = "")
	{
		if (isHost)
		{
			Dictionary dictionary = new Dictionary
			{
				{ "packet_name", packetName },
				{ "grid_x", gridX },
				{ "grid_y", gridY },
				{ "sync_id", syncId },
				{ "hitpoint_scale", hitpointScale },
				{ "scale", scaleVal },
				{ "hypnoses", hypnoses },
				{ "rise_duration", riseDuration },
				{ "use_create", useCreate },
				{ "pos_x", posX },
				{ "pos_y", posY },
				{ "walk_after_spawn", walkAfterSpawn },
				{ "ground_height", groundHeight },
				{ "size", sizeVal },
				{ "economy_owner", economyOwner },
				{ "user_id", peerId }
			};
			if (spawnState != null && spawnState.Count > 0)
			{
				dictionary["spawn_state"] = spawnState;
			}
			SendMatchState("spawn_character_at", Json.Stringify(dictionary));
		}
	}

	public void SendConveyorSpawn(string packetName, string packetType)
	{
		if (isHost)
		{
			Dictionary dictionary = new Dictionary
			{
				{ "packet_name", packetName },
				{ "packet_type", packetType },
				{ "user_id", peerId }
			};
			SendMatchState("conveyor_spawn", Json.Stringify(dictionary));
		}
	}

	public void SendClientReady()
	{
		if (!isHost)
		{
			SendMatchState("client_ready", MatchStateSerializer.Serialize(new UserIdDto
			{
				user_id = peerId
			}));
		}
	}

	public void SendGameEntry(int roundNum)
	{
		if (isHost)
		{
			_gameEntrySent = true;
			SendMatchState("game_entry", MatchStateSerializer.Serialize(new GameEntryDto
			{
				round_num = roundNum
			}));
		}
	}

	public void SendTipsPlay(string text, double duration)
	{
		if (isHost)
		{
			SendMatchState("tips_play", MatchStateSerializer.Serialize(new TipsPlayDto
			{
				text = text,
				duration = duration
			}));
		}
	}

	public long SendDamagePart(int syncId, string partName, double posX, double posY, double velocityX, double velocityY, long sequence = 0L)
	{
		if (!isHost)
		{
			return 0L;
		}
		sequence = ((sequence > 0) ? sequence : (++_damagePartSpawnSequence));
		SendMatchState("damage_part", MatchStateSerializer.Serialize(new DamagePartDto
		{
			sync_id = syncId,
			part_name = partName,
			px = posX,
			py = posY,
			vx = velocityX,
			vy = velocityY,
			seq = sequence
		}));
		return sequence;
	}

	public void SendDamagePointReach(int syncId, string damagePointName)
	{
		if (isHost)
		{
			SendMatchState("damage_point_reach", MatchStateSerializer.Serialize(new DamagePointReachDto
			{
				sync_id = syncId,
				damage_point_name = damagePointName
			}));
		}
	}

	public void SendArmorDamagePointReach(int syncId, string armorName, int stage)
	{
		if (isHost)
		{
			SendMatchState("armor_damage_point_reach", MatchStateSerializer.Serialize(new ArmorDamagePointReachDto
			{
				sync_id = syncId,
				armor_name = armorName,
				stage = stage
			}));
		}
	}

	public void SendArmorHitpointsEmpty(int syncId, string armorName)
	{
		if (isHost)
		{
			SendMatchState("armor_hitpoints_empty", MatchStateSerializer.Serialize(new ArmorHitpointsEmptyDto
			{
				sync_id = syncId,
				armor_name = armorName
			}));
		}
	}

	public void SendCraterCreate(int gridX, int gridY, string craterName)
	{
		if (isHost)
		{
			SendMatchState("crater_create", MatchStateSerializer.Serialize(new CraterCreateDto
			{
				grid_x = gridX,
				grid_y = gridY,
				crater_name = craterName
			}));
		}
	}

	public void SendProjectileEffectSpawn(ProjectileEffectSpawnDto dto)
	{
		if (isHost && dto != null)
		{
			SendMatchState("projectile_effect_spawn", MatchStateSerializer.Serialize(dto));
		}
	}

	public void SendCharacterComponentOperation(int ownerSyncId, string componentInstanceId, string componentTypeId, long sequence, string operationName, Dictionary operationData)
	{
		if (isHost && ownerSyncId >= 0 && !string.IsNullOrWhiteSpace(componentInstanceId) && !string.IsNullOrWhiteSpace(componentTypeId) && !string.IsNullOrWhiteSpace(operationName) && sequence >= 0)
		{
			Dictionary dictionary = new Dictionary
			{
				{ "sync_id", ownerSyncId },
				{ "component_instance_id", componentInstanceId },
				{ "component_type_id", componentTypeId },
				{ "sequence", sequence },
				{ "operation_name", operationName },
				{
					"data",
					operationData?.Duplicate(deep: true) ?? new Dictionary()
				}
			};
			SendMatchState("character_component_operation", Json.Stringify(dictionary));
		}
	}

	public void SendPlantFullSync(string plantsData)
	{
		if (isHost)
		{
			SendMatchState("plant_full_sync", plantsData);
		}
	}

	public void SendEventExecute(string phase, string eventsData)
	{
		if (isHost)
		{
			SendMatchState("event_execute", Json.Stringify(new Dictionary
			{
				{ "phase", phase },
				{ "events", eventsData }
			}));
		}
	}

	public void SendWaveEventExecute(int waveId, int eventId, string eventsData)
	{
		if (isHost)
		{
			SendMatchState("wave_event_execute", Json.Stringify(new Dictionary
			{
				{ "wave_id", waveId },
				{ "event_id", eventId },
				{ "events", eventsData }
			}));
		}
	}

	public void ResetClientsReady()
	{
		_clientsReady.Clear();
	}

	private void _ResetSessionState()
	{
		ReleaseBattleCompatibility();
		_preparingModStart = false;
		_hostModBatch = null;
		_clientsReady.Clear();
		_levelConfigAcked.Clear();
		_gameEntryAcked.Clear();
		_gameEntrySent = false;
		selectedLevelId = "";
		_receivedLevelConfigJson = "";
		_receivedLevelConfigType = "";
		_receivedLevelEnterMode = "";
		_receivedLevelIsBattle = false;
		_activeBattleLevelConfigData = "";
		_activeBattleStartData = "";
		_fallingObjectSpawnSequence = 0L;
		_damagePartSpawnSequence = 0L;
		_pingTimer = 0.0;
	}

	public bool CheckAllClientsReady()
	{
		Godot.Collections.Array battleAdmittedPeers = BattleAdmittedPeers;
		battleAdmittedPeers.Remove("1");
		if (battleAdmittedPeers.Count == 0)
		{
			return true;
		}
		foreach (Variant item in battleAdmittedPeers)
		{
			string text = (string)item;
			if (!_clientsReady.Contains(text))
			{
				return false;
			}
		}
		return true;
	}

	public void SendLevelConfigAck()
	{
	}

	public void ResetLevelConfigAck()
	{
		_hostModBatch?.Cancel("配置确认已重置，请重新开始。");
		_levelConfigAcked.Clear();
	}

	public bool CheckAllLevelConfigAcked()
	{
		ModCompatibilityBatch hostModBatch = _hostModBatch;
		if (hostModBatch != null && hostModBatch.Accepted)
		{
			return _modEnvironmentLease?.IsCurrent ?? false;
		}
		return false;
	}

	public void SendGameEntryAck()
	{
		if (!isHost)
		{
			SendMatchState("game_entry_ack", MatchStateSerializer.Serialize(new UserIdDto
			{
				user_id = peerId
			}));
		}
	}

	public void ResetGameEntryAck()
	{
		_gameEntryAcked.Clear();
		_gameEntrySent = false;
	}

	public bool CheckAllGameEntryAcked()
	{
		Godot.Collections.Array battleAdmittedPeers = BattleAdmittedPeers;
		battleAdmittedPeers.Remove("1");
		if (battleAdmittedPeers.Count == 0)
		{
			return true;
		}
		foreach (Variant item in battleAdmittedPeers)
		{
			string text = (string)item;
			if (!_gameEntryAcked.Contains(text))
			{
				return false;
			}
		}
		return true;
	}

	public string GetMatchIdShort()
	{
		if (currentMatchId == "")
		{
			return "";
		}
		if (isHost)
		{
			return _GetLanIp() + ":" + _currentPort;
		}
		return currentMatchId;
	}

	private string _GetLanIp()
	{
		string[] localAddresses = IP.GetLocalAddresses();
		List<string> list = new List<string>();
		string[] array = localAddresses;
		foreach (string text in array)
		{
			if (text == "127.0.0.1" || text.Contains(":") || text.StartsWith("169.254"))
			{
				continue;
			}
			if (text.StartsWith("172."))
			{
				int num = int.Parse(text.Split(".")[1]);
				if (num >= 16 && num <= 31)
				{
					continue;
				}
			}
			list.Add(text);
		}
		foreach (string item in list)
		{
			if (item.StartsWith("192.168."))
			{
				return item;
			}
		}
		foreach (string item2 in list)
		{
			if (item2.StartsWith("10."))
			{
				return item2;
			}
		}
		if (list.Count > 0)
		{
			return list[0];
		}
		return "127.0.0.1";
	}

	public string[] GetAllLanIps()
	{
		string[] localAddresses = IP.GetLocalAddresses();
		List<string> list = new List<string>();
		string[] array = localAddresses;
		foreach (string text in array)
		{
			if (text == "127.0.0.1" || text.Contains(":") || text.StartsWith("169.254"))
			{
				continue;
			}
			if (text.StartsWith("172."))
			{
				int num = int.Parse(text.Split(".")[1]);
				if (num >= 16 && num <= 31)
				{
					continue;
				}
			}
			list.Add(text);
		}
		return list.ToArray();
	}

	private void _OnPeerConnected(long id)
	{
		ModPeerChanged(id.ToString(), disconnected: false);
		if (!isHost)
		{
			return;
		}
		if (matchMembers.Count >= 4)
		{
			_peer.DisconnectPeer((int)id);
			return;
		}
		if (!matchMembers.Contains(id.ToString()))
		{
			matchMembers.Add(id.ToString());
		}
		OnPeerJoined?.Invoke("Player");
		Dictionary dictionary = new Dictionary();
		dictionary["__protocol_version"] = 4;
		foreach (int key in _peerNames.Keys)
		{
			dictionary[key.ToString()] = _peerNames[key];
		}
		RpcId((int)id, "_RpcSendHostInfo", _playerName, matchMembers, Global.Instance.version, dictionary);
		foreach (Variant matchMember in matchMembers)
		{
			int num = int.Parse((string)matchMember);
			if (num != 1 && num != (int)id)
			{
				RpcId(num, "_RpcSyncMembers", matchMembers);
			}
		}
		SendLateJoinBootstrap(id.ToString());
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void _RpcSendHostInfo(string hostName, Godot.Collections.Array members, string hostVersion, Dictionary namesData)
	{
		_peerNames.Clear();
		_peerNames[1] = hostName;
		int num = namesData.GetValueOrDefault("__protocol_version", 1).AsInt32();
		if (num != 4)
		{
			BroadCastManager.Instance.BroadCastFloatCreate($"Protocol mismatch: local v{4}, host v{num}", Colors.Red);
			RpcId(1L, "_RpcVersionMismatch", $"protocol:{4}");
			_CleanupConnection();
			return;
		}
		foreach (Variant key in namesData.Keys)
		{
			string text = (string)key;
			if (!(text == "__protocol_version"))
			{
				_peerNames[int.Parse(text)] = (string)namesData[text];
			}
		}
		matchMembers = members;
		if (hostVersion != Global.Instance.version)
		{
			BroadCastManager.Instance.BroadCastFloatCreate("版本不一致，无法连接", Colors.Red);
			RpcId(1L, "_RpcVersionMismatch", Global.Instance.version);
			_CleanupConnection();
		}
		else
		{
			OnPeerJoined?.Invoke(hostName);
			RpcId(1L, "_RpcSendClientName", _playerName);
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void _RpcVersionMismatch(string clientVersion)
	{
		long num = Multiplayer.GetRemoteSenderId();
		string valueOrDefault = _peerNames.GetValueOrDefault((int)num, "Client");
		BroadCastManager.Instance.BroadCastFloatCreate(valueOrDefault + " 版本不一致(" + clientVersion + ")，已断开", Colors.Red);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void _RpcSyncMembers(Godot.Collections.Array members)
	{
		matchMembers = members;
		OnPeerJoined?.Invoke("Player");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void _RpcSendClientName(string playerName)
	{
		long num = Multiplayer.GetRemoteSenderId();
		_peerNames[(int)num] = playerName;
		OnPeerJoined?.Invoke(playerName);
		_broadcastPeerNames();
	}

	private void _broadcastPeerNames()
	{
		if (!isHost)
		{
			return;
		}
		Dictionary dictionary = new Dictionary();
		foreach (int key in _peerNames.Keys)
		{
			dictionary[key.ToString()] = _peerNames[key];
		}
		dictionary["1"] = _playerName;
		Rpc("_RpcSyncPeerNames", dictionary);
	}

	[Rpc(MultiplayerApi.RpcMode.Authority)]
	public void _RpcSyncPeerNames(Dictionary namesData)
	{
		_peerNames.Clear();
		foreach (Variant key in namesData.Keys)
		{
			string text = (string)key;
			_peerNames[int.Parse(text)] = (string)namesData[text];
		}
	}

	private void _OnPeerDisconnected(long id)
	{
		string valueOrDefault = _peerNames.GetValueOrDefault((int)id, "Player");
		string text = id.ToString();
		ModPeerChanged(text, disconnected: true);
		matchMembers.Remove(text);
		_peerNames.Remove((int)id);
		_clientsReady.Remove(text);
		_levelConfigAcked.Remove(text);
		_gameEntryAcked.Remove(text);
		_peerLatencies.Remove(text);
		OnPeerLeft?.Invoke(valueOrDefault, text);
		ReevaluateHostBarriersAfterPeerLeft();
	}

	private void ReevaluateHostBarriersAfterPeerLeft()
	{
		if (isHost)
		{
			bool flag = CheckAllClientsReady();
			bool flag2 = CheckAllLevelConfigAcked();
			bool flag3 = _gameEntrySent && CheckAllGameEntryAcked();
			if (flag)
			{
				EmitAllClientsReady();
			}
			if (flag2)
			{
				OnAllLevelConfigAcked?.Invoke();
			}
			if (flag3)
			{
				EmitAllGameEntryAcked();
			}
		}
	}

	private void _OnConnectedToServer()
	{
		OnMatchJoined?.Invoke(currentMatchId);
		BroadCastManager.Instance.BroadCastFloatCreate("JOINED_ROOM", Colors.Green);
	}

	private void _OnConnectionFailed()
	{
		BroadCastManager.Instance.BroadCastFloatCreate("JOIN_MATCH_FAILED", Colors.Red);
		_CleanupConnection();
	}

	private void _OnServerDisconnected()
	{
		BroadCastManager.Instance.BroadCastFloatCreate("HOST_DISCONNECTED", Colors.Red);
		_CleanupConnection();
	}

	private void _CleanupConnection()
	{
		if (currentMatchId != "" || _peer != null || isHost || (Global.Instance != null && Global.Instance.isMultiplayerMode))
		{
			_sessionGeneration++;
			_startGameTransitionPending = false;
			_startGameTransitionCompleted = false;
			if (_peer != null)
			{
				_peer.Close();
			}
			Multiplayer.MultiplayerPeer = new OfflineMultiplayerPeer();
			_peer = null;
			currentMatchId = "";
			isHost = false;
			_currentPort = 7777;
			_playerName = "";
			matchMembers.Clear();
			_peerNames.Clear();
			_pingSendTimes.Clear();
			_peerLatencies.Clear();
			_lastUnreliableSequenceByStream.Clear();
			_ResetSessionState();
			if (Global.Instance != null)
			{
				Global.Instance.isMultiplayerMode = false;
				Global.Instance.isMultiplayerHost = false;
			}
			OnConnectionChanged?.Invoke(connected: false);
			OnMatchLeft?.Invoke();
		}
	}

	public void _HandleStartGame(string data)
	{
		if (!_startGameTransitionPending && !_startGameTransitionCompleted && !(currentMatchId == "") && _peer != null && AcceptCompatibleStart(data))
		{
			_modSceneAccepted = new TaskCompletionSource<bool>();
			_startGameTransitionPending = true;
			long sessionGeneration = _sessionGeneration;
			HandleStartGameAsync(data, sessionGeneration);
		}
	}

	private async Task HandleStartGameAsync(string data, long sessionGeneration)
	{
		TaskCompletionSource<bool> completion = _modSceneAccepted;
		try
		{
			Variant variant = Json.ParseString(data);
			if (variant.VariantType != Variant.Type.Dictionary)
			{
				return;
			}
			Dictionary dictionary = (Dictionary)variant;
			XWModLevelIdentity identity = _acceptedLevelIdentity;
			selectedLevelId = identity.LevelSaveKey;
			string difficulty = identity.Difficulty;
			string text = (string)dictionary.GetValueOrDefault("level_uid", "");
			Global.Instance.enterLevelMode = ((_receivedLevelEnterMode != "") ? _receivedLevelEnterMode : "Multiplayer");
			Global.Instance.enterLevelId = selectedLevelId;
			Global.Instance.enterLevelIsBattle = _receivedLevelIsBattle;
			Global.Instance.isMultiplayerMode = true;
			Global.Instance.isMultiplayerHost = isHost;
			XWModLevelSession.Select(identity);
			if (identity.IsMod)
			{
				if (!XWModContentCatalog.TryResolve(identity, out var config, out var error))
				{
					throw new InvalidOperationException(error);
				}
				TowerDefenseManager.Instance.currentLevelConfig = config;
				Global.Instance.currentLevelChoose = "__ModLevels";
				Global.Instance.enterLevelMode = "ModLevel";
			}
			else if (!isHost)
			{
				TowerDefenseManager.Instance.currentLevelConfig = null;
				if (_receivedLevelConfigJson != "")
				{
					_CreateLevelConfigFromReceived();
				}
				else if (text != "" && ResourceLoader.Exists(text))
				{
					TowerDefenseManager.Instance.currentLevelConfig = GD.Load<TowerDefenseLevelConfig>(text);
				}
				else
				{
					_LoadLevelById(selectedLevelId, difficulty);
				}
			}
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			if (sessionGeneration != _sessionGeneration || currentMatchId == "" || _peer == null || !IsInsideTree())
			{
				return;
			}
			XWModEnvironmentService.EnvironmentLease modEnvironmentLease = _modEnvironmentLease;
			bool accepted;
			if (modEnvironmentLease != null && modEnvironmentLease.IsCurrent && _modBattleActive)
			{
				TowerDefenseLevelBaseConfig currentLevelConfig = TowerDefenseManager.Instance.currentLevelConfig;
				if (!GodotObject.IsInstanceValid(currentLevelConfig) || (currentLevelConfig.name != identity.LevelSaveKey && (!(currentLevelConfig.name == "") || !(identity.LevelSaveKey == "__session"))))
				{
					throw new InvalidOperationException("开战配置与已确认的关卡身份不一致。");
				}
				_startGameTransitionCompleted = true;
				accepted = false;
				SceneManager.Instance.OnSceneChange += SceneAccepted;
				try
				{
					SceneManager.Instance.ChangeScene("TowerDefense");
				}
				finally
				{
					SceneManager.Instance.OnSceneChange -= SceneAccepted;
				}
				if (!accepted)
				{
					throw new InvalidOperationException("场景切换未被受理，请退出后重新加入。");
				}
				completion?.TrySetResult(result: true);
			}
			void SceneAccepted(string scene)
			{
				if (scene == "TowerDefense")
				{
					accepted = true;
				}
			}
		}
		catch (Exception ex)
		{
			if (sessionGeneration == _sessionGeneration)
			{
				_CleanupConnection();
				ShowModCompatibilityFailure("多人场景切换失败: " + ex.Message);
			}
		}
		finally
		{
			completion?.TrySetResult(result: false);
			if (sessionGeneration == _sessionGeneration)
			{
				_startGameTransitionPending = false;
			}
		}
	}

	private void _LoadLevelById(string levelId, string difficult = "Normal")
	{
		foreach (string key in ResourceManager.Instance.LEVELS.Keys)
		{
			Dictionary dictionary = ResourceManager.Instance.LEVELS[key];
			if (dictionary == null || !dictionary.ContainsKey("Chapter"))
			{
				continue;
			}
			foreach (Variant item in (Godot.Collections.Array)dictionary["Chapter"])
			{
				Dictionary dictionary2 = (Dictionary)item;
				if (!dictionary2.ContainsKey("Level"))
				{
					continue;
				}
				foreach (Variant item2 in (Godot.Collections.Array)dictionary2["Level"])
				{
					Dictionary dictionary3 = (Dictionary)item2;
					if (!dictionary3.ContainsKey("SaveKey") || !((string)dictionary3["SaveKey"] == levelId))
					{
						continue;
					}
					Dictionary dictionary4 = (Dictionary)dictionary3.GetValueOrDefault("Level", new Dictionary());
					if (dictionary4 != null && dictionary4.Count > 0)
					{
						string text = (string)dictionary4.GetValueOrDefault(difficult, "");
						if (text == "" || !ResourceLoader.Exists(text))
						{
							text = (string)dictionary4.GetValueOrDefault("Normal", "");
						}
						if (text != "" && ResourceLoader.Exists(text))
						{
							TowerDefenseManager.Instance.currentLevelConfig = GD.Load<TowerDefenseLevelConfig>(text);
						}
					}
					return;
				}
			}
		}
	}

	private void _HandleSelectLevel(string data)
	{
		Variant variant = Json.ParseString(data);
		if (variant.VariantType == Variant.Type.Dictionary)
		{
			Dictionary dictionary = (Dictionary)variant;
			if (ReadLevelIdentity(dictionary, out var identity) && !(dictionary.GetValueOrDefault("level_id", "").AsString() != identity.LevelSaveKey) && !(dictionary.GetValueOrDefault("difficult", "").AsString() != identity.Difficulty))
			{
				selectedLevelId = identity.LevelSaveKey;
			}
		}
	}

	private void _HandleLevelConfig(string data)
	{
		ReceiveCompatibleLevelConfig(data);
	}

	private void _CreateLevelConfigFromReceived()
	{
		if (_receivedLevelConfigJson == "")
		{
			return;
		}
		string receivedLevelConfigType = _receivedLevelConfigType;
		if (!(receivedLevelConfigType == "TowerDefenseLevelConfig"))
		{
			if (receivedLevelConfigType == "TowerDefenseLevelNewConfig")
			{
				Json json = new Json();
				if (json.Parse(_receivedLevelConfigJson) == Error.Ok)
				{
					TowerDefenseLevelNewConfig towerDefenseLevelNewConfig = new TowerDefenseLevelNewConfig();
					towerDefenseLevelNewConfig.data = json;
					TowerDefenseManager.Instance.currentLevelConfig = towerDefenseLevelNewConfig;
				}
			}
		}
		else
		{
			Json json2 = new Json();
			if (json2.Parse(_receivedLevelConfigJson) == Error.Ok)
			{
				TowerDefenseLevelConfig towerDefenseLevelConfig = new TowerDefenseLevelConfig();
				towerDefenseLevelConfig.data = json2;
				TowerDefenseManager.Instance.currentLevelConfig = towerDefenseLevelConfig;
			}
		}
		_receivedLevelConfigJson = "";
		_receivedLevelConfigType = "";
		_receivedLevelEnterMode = "";
		_receivedLevelIsBattle = false;
	}

	private XWModLevelIdentity GetSelectedLevelIdentity()
	{
		TowerDefenseLevelBaseConfig currentLevelConfig = TowerDefenseManager.Instance.currentLevelConfig;
		if (Global.Instance.enterLevelMode == "ModLevel")
		{
			XWModLevelIdentity current = XWModLevelSession.Current;
			if (current == null || current.LevelSaveKey != currentLevelConfig.name)
			{
				throw new InvalidOperationException("Mod 选关身份与配置不一致。");
			}
			return current;
		}
		string text = Global.Instance.currentLevelChoose;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = Global.Instance.enterLevelMode;
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			text = "Multiplayer";
		}
		string text2 = currentLevelConfig?.name;
		if (string.IsNullOrWhiteSpace(text2))
		{
			text2 = (string.IsNullOrWhiteSpace(selectedLevelId) ? "__session" : selectedLevelId);
		}
		string text3 = GameSaveManager.Instance.GetKeyValue("CurrentDifficult").AsString();
		bool flag;
		switch (text3)
		{
		case "Normal":
		case "Difficult":
		case "Ultimate":
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (!flag)
		{
			text3 = "Normal";
		}
		return new XWModLevelIdentity("", text, text2, text3);
	}

	private static bool ReadLevelIdentity(Dictionary data, out XWModLevelIdentity identity)
	{
		return XWModLevelIdentity.TryParse(data.GetValueOrDefault("level_identity"), out identity);
	}

	private bool MatchesAcceptedLevel(Dictionary data)
	{
		if (ReadLevelIdentity(data, out var identity))
		{
			return identity == _acceptedLevelIdentity;
		}
		return false;
	}

	internal static bool ConfigMatchesIdentity(Dictionary data, XWModLevelIdentity identity)
	{
		Variant variant = Json.ParseString(data.GetValueOrDefault("config_json", "").AsString());
		if (variant.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		if (!variant.AsGodotDictionary().TryGetValue("Name", out var value) || value.VariantType != Variant.Type.String)
		{
			return false;
		}
		string text = value.AsString();
		if (!(text == identity.LevelSaveKey))
		{
			if (!identity.IsMod && text.Length == 0)
			{
				return identity.LevelSaveKey == "__session";
			}
			return false;
		}
		return true;
	}

	public bool IsBattleParticipant(string id)
	{
		if (_modBattleActive)
		{
			return _battleAdmitted.Contains(id);
		}
		return false;
	}

	private void ShowModCompatibilityFailure(string reason)
	{
		LastModCompatibilityFailure = ((reason.Length > 4096) ? (reason.Substring(0, 4096) + "…") : reason);
		GD.PushWarning("[ModCompatibility] " + LastModCompatibilityFailure);
		OnModCompatibilityFailure?.Invoke(LastModCompatibilityFailure);
		if (_modBattleActive)
		{
			BroadCastManager.Instance?.BroadCastFloatCreate(LastModCompatibilityFailure, Colors.Red);
		}
		else if (GodotObject.IsInstanceValid(DialogManager.Instance))
		{
			DialogManager.Instance.DialogCreate("DialogBoxTips").Set("text", LastModCompatibilityFailure.Replace("[", "[lb]"));
		}
	}

	public async Task<bool> StartCompatibleGameAsync()
	{
		if (!isHost || !CanSendMatchRpc() || _modBattleActive)
		{
			return false;
		}
		if (_preparingModStart)
		{
			_hostModBatch?.Cancel("本次开战已取消，请重新开始确认。");
			return false;
		}
		_preparingModStart = true;
		long session = _sessionGeneration;
		ModCompatibilityBatch batch = null;
		bool started = false;
		try
		{
			using CancellationTokenSource timeout = new CancellationTokenSource(TimeSpan.FromSeconds(15L));
			XWModEnvironmentStatus xWModEnvironmentStatus = await XWModEnvironmentService.EnsureReadyAsync(timeout.Token);
			if (session != _sessionGeneration || !isHost || !CanSendMatchRpc())
			{
				return false;
			}
			_modEnvironmentLease = XWModEnvironmentService.TryAcquireEnvironment(xWModEnvironmentStatus.Generation);
			if (_modEnvironmentLease == null)
			{
				throw new InvalidOperationException("Mod 环境尚未就绪：" + xWModEnvironmentStatus.Reason);
			}
			batch = (_hostModBatch = new ModCompatibilityBatch(from id in matchMembers
				select id.AsString() into id
				where id != peerId
				select id, xWModEnvironmentStatus.Snapshot.Sha256, System.Environment.TickCount64, 15000, GetSelectedLevelIdentity().Canonical));
			_modBatches.Add(batch.Id, batch);
			_levelConfigAcked.Clear();
			string raw = BuildLevelConfigData();
			if (raw == "")
			{
				throw new InvalidOperationException("关卡配置不可用。");
			}
			Dictionary dictionary = Json.ParseString(raw).AsGodotDictionary();
			dictionary["config_batch_id"] = batch.Id;
			dictionary["mod_snapshot"] = XWModEnvironmentService.WriteSnapshot(xWModEnvironmentStatus.Snapshot);
			_activeBattleLevelConfigData = Json.Stringify(dictionary);
			CacheCompatibleLevelConfig(dictionary);
			foreach (string candidate in batch.Candidates)
			{
				SendMatchStateToPeer("level_config", _activeBattleLevelConfigData, candidate);
			}
			if (!(await batch.Completion.Task.WaitAsync(TimeSpan.FromSeconds(15L))) || !batch.Accepted)
			{
				throw new InvalidOperationException(batch.Failure);
			}
			if (session != _sessionGeneration || !_modEnvironmentLease.IsCurrent || System.Environment.TickCount64 >= batch.Deadline || !batch.Candidates.SetEquals(from id in matchMembers
				select id.AsString() into id
				where id != peerId
				select id) || BuildLevelConfigData() != raw)
			{
				throw new InvalidOperationException("成员、关卡配置或 Mod 环境已变化，请重新确认。");
			}
			_acceptedModBatch = batch.Id;
			_modBattleId = batch.Id;
			_battleAdmitted.UnionWith(batch.Candidates);
			_battleAdmitted.Add(peerId);
			_modBattleActive = true;
			SendStartGame();
			TaskCompletionSource<bool> modSceneAccepted = _modSceneAccepted;
			bool flag = modSceneAccepted != null;
			if (flag)
			{
				flag = await modSceneAccepted.Task;
			}
			started = flag && session == _sessionGeneration && (_modEnvironmentLease?.IsCurrent ?? false);
			return started;
		}
		catch (Exception ex) when ((ex is TimeoutException || ex is OperationCanceledException || ex is InvalidOperationException) ? true : false)
		{
			if (session == _sessionGeneration)
			{
				bool flag2 = ((ex is TimeoutException || ex is OperationCanceledException) ? true : false);
				ShowModCompatibilityFailure(flag2 ? "Mod 环境确认超时，未开始战斗。" : ex.Message);
			}
			return false;
		}
		finally
		{
			if (batch != null)
			{
				_modBatches.Remove(batch.Id);
				if (!started)
				{
					CancelRemoteModBatch(batch, (batch.Failure == "") ? "本次开战已取消。" : batch.Failure);
				}
			}
			if (session == _sessionGeneration)
			{
				_hostModBatch = null;
				_preparingModStart = false;
				if (!started)
				{
					ReleaseBattleCompatibility();
				}
			}
		}
	}

	private void CancelRemoteModBatch(ModCompatibilityBatch batch, string reason)
	{
		batch.Cancel(reason);
		Dictionary dictionary = new Dictionary
		{
			["config_batch_id"] = batch.Id,
			["cancelled"] = true,
			["reason"] = reason
		};
		foreach (string candidate in batch.Candidates)
		{
			if (matchMembers.Contains(candidate))
			{
				SendMatchStateToPeer("level_config", Json.Stringify(dictionary), candidate);
			}
		}
	}

	private void ReceiveModAck(string sender, string raw)
	{
		if (!isHost || !matchMembers.Contains(sender))
		{
			return;
		}
		Variant variant = Json.ParseString(raw);
		if (variant.VariantType != Variant.Type.Dictionary)
		{
			return;
		}
		Dictionary dictionary = variant.AsGodotDictionary();
		string text = dictionary.GetValueOrDefault("config_batch_id", "").AsString();
		if (!_modBatches.TryGetValue(text, out var value))
		{
			return;
		}
		bool accepted = dictionary.TryGetValue("accepted", out var value2) && value2.VariantType == Variant.Type.Bool && value2.AsBool() && dictionary.TryGetValue("differences", out var value3) && value3.VariantType == Variant.Type.Array && value3.AsGodotArray().Count == 0;
		string reason = dictionary.GetValueOrDefault("reason", "快照或确认字段无效").AsString();
		if (ReadLevelIdentity(dictionary, out var identity) && value.Confirm(sender, text, accepted, dictionary.GetValueOrDefault("snapshot_sha256", "").AsString(), reason, System.Environment.TickCount64, identity.Canonical))
		{
			if (!_levelConfigAcked.Contains(sender))
			{
				_levelConfigAcked.Add(sender);
			}
			if (value.Accepted)
			{
				OnAllLevelConfigAcked?.Invoke();
			}
		}
	}

	private void ReceiveCompatibleLevelConfig(string raw)
	{
		if (isHost)
		{
			return;
		}
		Variant variant = Json.ParseString(raw);
		if (variant.VariantType != Variant.Type.Dictionary)
		{
			return;
		}
		Dictionary dictionary = variant.AsGodotDictionary();
		if (dictionary.GetValueOrDefault("roster_only", false).AsBool())
		{
			if (_modBattleActive && dictionary.GetValueOrDefault("battle_id", "").AsString() == _modBattleId && MatchesAcceptedLevel(dictionary))
			{
				ReadAdmittedRoster(dictionary);
			}
			return;
		}
		string text = dictionary.GetValueOrDefault("config_batch_id", "").AsString();
		if (dictionary.GetValueOrDefault("cancelled", false).AsBool())
		{
			if (!_modBattleActive && text != "" && text == _pendingRemoteModBatch)
			{
				ReleaseBattleCompatibility();
				ShowModCompatibilityFailure(dictionary.GetValueOrDefault("reason", "本次开战已取消。").AsString());
			}
		}
		else if (!_modBattleActive)
		{
			ReleaseBattleCompatibility();
			_pendingRemoteModBatch = text;
			ValidateRemoteModConfigAsync(dictionary, ++_remoteModTicket, _sessionGeneration);
		}
	}

	private async Task ValidateRemoteModConfigAsync(Dictionary data, long ticket, long session)
	{
		string batch = data.GetValueOrDefault("config_batch_id", "").AsString();
		string reason = "Mod 配置字段缺失或格式无效。";
		Godot.Collections.Array differences = new Godot.Collections.Array();
		bool accepted = false;
		string digest = "";
		try
		{
			using CancellationTokenSource timeout = new CancellationTokenSource(TimeSpan.FromSeconds(15L));
			XWModEnvironmentStatus xWModEnvironmentStatus = await XWModEnvironmentService.EnsureReadyAsync(timeout.Token);
			if (ticket != _remoteModTicket || session != _sessionGeneration)
			{
				return;
			}
			digest = xWModEnvironmentStatus.Snapshot?.Sha256 ?? "";
			string text = data.GetValueOrDefault("config_type", "").AsString();
			bool flag = Guid.TryParseExact(batch, "N", out var _);
			if (flag)
			{
				bool flag2 = ((text == "TowerDefenseLevelConfig" || text == "TowerDefenseLevelNewConfig") ? true : false);
				flag = flag2;
			}
			if (flag && ReadLevelIdentity(data, out var identity) && ConfigMatchesIdentity(data, identity) && data.TryGetValue("enter_level_mode", out var value) && value.VariantType == Variant.Type.String && identity.IsMod == (value.AsString() == "ModLevel") && data.TryGetValue("enter_level_is_battle", out var value2) && value2.VariantType == Variant.Type.Bool && Json.ParseString(data.GetValueOrDefault("config_json", "").AsString()).VariantType == Variant.Type.Dictionary && data.TryGetValue("mod_snapshot", out var value3) && value3.VariantType == Variant.Type.Dictionary && XWModEnvironmentService.TryReadSnapshot(value3.AsGodotDictionary(), out var snapshot, out reason))
			{
				if (identity.IsMod && (!XWModContentCatalog.TryResolve(identity, out var config, out reason) || config.GetType().Name != text))
				{
					reason = "Mod 关卡身份或配置类型不一致：" + reason;
				}
				else if (!xWModEnvironmentStatus.Ready || xWModEnvironmentStatus.Snapshot == null)
				{
					reason = "Mod 环境尚未就绪：" + xWModEnvironmentStatus.Reason;
				}
				else
				{
					IReadOnlyList<XWModSnapshotDifference> readOnlyList = XWModEnvironmentService.CompareSnapshots(snapshot, xWModEnvironmentStatus.Snapshot);
					foreach (XWModSnapshotDifference item in readOnlyList)
					{
						differences.Add(new Dictionary
						{
							["id"] = item.Id,
							["field"] = item.Field,
							["expected"] = item.Expected,
							["actual"] = item.Actual
						});
					}
					reason = XWModEnvironmentService.DescribeDifferences(readOnlyList);
					if (readOnlyList.Count == 0)
					{
						_modEnvironmentLease = XWModEnvironmentService.TryAcquireEnvironment(xWModEnvironmentStatus.Generation);
						accepted = _modEnvironmentLease != null;
						if (!accepted)
						{
							reason = "Mod 环境正在改变，请重试。";
						}
					}
				}
			}
			if (accepted)
			{
				_acceptedModBatch = batch;
				_remoteModDeadline = System.Environment.TickCount64 + 30000;
				CacheCompatibleLevelConfig(data);
			}
		}
		catch (Exception ex)
		{
			reason = "Mod 环境检查失败：" + ex.Message;
		}
		if (ticket == _remoteModTicket && session == _sessionGeneration)
		{
			Dictionary dictionary = new Dictionary
			{
				["config_batch_id"] = batch,
				["accepted"] = accepted,
				["snapshot_sha256"] = digest,
				["level_identity"] = data.GetValueOrDefault("level_identity"),
				["differences"] = differences,
				["reason"] = reason
			};
			SendMatchState("level_config_ack", Json.Stringify(dictionary));
			if (!accepted)
			{
				ShowModCompatibilityFailure(reason);
			}
		}
	}

	private void CacheCompatibleLevelConfig(Dictionary data)
	{
		if (!ReadLevelIdentity(data, out _acceptedLevelIdentity))
		{
			throw new InvalidOperationException("关卡身份缺失或格式无效。");
		}
		if (!ConfigMatchesIdentity(data, _acceptedLevelIdentity))
		{
			throw new InvalidOperationException("关卡身份与实际配置名称不一致。");
		}
		_receivedLevelConfigType = data.GetValueOrDefault("config_type", "").AsString();
		_receivedLevelConfigJson = data.GetValueOrDefault("config_json", "").AsString();
		_receivedLevelEnterMode = data.GetValueOrDefault("enter_level_mode", "").AsString();
		_receivedLevelIsBattle = data.GetValueOrDefault("enter_level_is_battle", false).AsBool();
	}

	private bool AcceptCompatibleStart(string raw)
	{
		Variant variant = Json.ParseString(raw);
		if (variant.VariantType == Variant.Type.Dictionary)
		{
			XWModEnvironmentService.EnvironmentLease modEnvironmentLease = _modEnvironmentLease;
			if (modEnvironmentLease != null && modEnvironmentLease.IsCurrent)
			{
				Dictionary dictionary = variant.AsGodotDictionary();
				if (_acceptedModBatch == "" || dictionary.GetValueOrDefault("config_batch_id", "").AsString() != _acceptedModBatch || !MatchesAcceptedLevel(dictionary) || dictionary.GetValueOrDefault("level_id", "").AsString() != _acceptedLevelIdentity.LevelSaveKey || dictionary.GetValueOrDefault("difficult", "").AsString() != _acceptedLevelIdentity.Difficulty || dictionary.GetValueOrDefault("snapshot_sha256", "").AsString() != _modEnvironmentLease.Status.Snapshot.Sha256 || (!isHost && System.Environment.TickCount64 >= _remoteModDeadline))
				{
					return false;
				}
				if (!ReadAdmittedRoster(dictionary) || !_battleAdmitted.Contains(peerId))
				{
					return false;
				}
				_modBattleId = dictionary.GetValueOrDefault("battle_id", "").AsString();
				if (_modBattleId == "")
				{
					return false;
				}
				_modBattleActive = true;
				_battleSceneDeadline = System.Environment.TickCount64 + 60000;
				return true;
			}
		}
		return false;
	}

	private bool ReadAdmittedRoster(Dictionary data)
	{
		if (!data.TryGetValue("admitted_peers", out var value) || value.VariantType != Variant.Type.Array)
		{
			return false;
		}
		string[] array = (from variant in value.AsGodotArray()
			select variant.AsString()).ToArray();
		if (array.Length > 4 || !Enumerable.Contains(array, "1") || array.Distinct().Count() != array.Length || array.Any((string id) => !matchMembers.Contains(id)))
		{
			return false;
		}
		_battleAdmitted.Clear();
		_battleAdmitted.UnionWith(array);
		return true;
	}

	private async Task SendCompatibleLateJoinAsync(string peer)
	{
		if (!_modBattleActive)
		{
			return;
		}
		XWModEnvironmentService.EnvironmentLease modEnvironmentLease = _modEnvironmentLease;
		if (modEnvironmentLease == null || !modEnvironmentLease.IsCurrent || _activeBattleLevelConfigData == "")
		{
			return;
		}
		long session = _sessionGeneration;
		ModCompatibilityBatch batch = new ModCompatibilityBatch(new string[1] { peer }, _modEnvironmentLease.Status.Snapshot.Sha256, System.Environment.TickCount64, 15000, _acceptedLevelIdentity.Canonical);
		_modBatches.Add(batch.Id, batch);
		bool joined = false;
		try
		{
			Dictionary dictionary = Json.ParseString(_activeBattleLevelConfigData).AsGodotDictionary();
			dictionary["config_batch_id"] = batch.Id;
			SendMatchStateToPeer("level_config", Json.Stringify(dictionary), peer);
			if (!(await batch.Completion.Task.WaitAsync(TimeSpan.FromSeconds(15L))) || !batch.Accepted)
			{
				throw new InvalidOperationException(batch.Failure);
			}
			if (session != _sessionGeneration || !_modBattleActive)
			{
				return;
			}
			XWModEnvironmentService.EnvironmentLease modEnvironmentLease2 = _modEnvironmentLease;
			if (modEnvironmentLease2 == null || !modEnvironmentLease2.IsCurrent || !matchMembers.Contains(peer) || System.Environment.TickCount64 >= batch.Deadline)
			{
				return;
			}
			_battleAdmitted.Add(peer);
			Dictionary dictionary2 = Json.ParseString(_activeBattleStartData).AsGodotDictionary();
			dictionary2["config_batch_id"] = batch.Id;
			dictionary2["admitted_peers"] = BattleAdmittedPeers;
			Dictionary dictionary3 = new Dictionary
			{
				["roster_only"] = true,
				["battle_id"] = _modBattleId,
				["level_identity"] = _acceptedLevelIdentity.ToDictionary(),
				["admitted_peers"] = BattleAdmittedPeers
			};
			foreach (string item in _battleAdmitted.Where((string id) => id != peerId && id != peer))
			{
				SendMatchStateToPeer("level_config", Json.Stringify(dictionary3), item);
			}
			SendMatchStateToPeer("start_game", Json.Stringify(dictionary2), peer);
			joined = true;
		}
		catch (Exception ex) when ((ex is TimeoutException || ex is InvalidOperationException) ? true : false)
		{
			if (session == _sessionGeneration)
			{
				ShowModCompatibilityFailure("成员 " + peer + " 未进入战斗：" + ex.Message);
			}
		}
		finally
		{
			_modBatches.Remove(batch.Id);
			if (!joined && session == _sessionGeneration)
			{
				CancelRemoteModBatch(batch, (batch.Failure == "") ? "晚加入确认已取消或超时。" : batch.Failure);
			}
		}
	}

	private void ProcessModCompatibility()
	{
		if (_modBattleActive && _battleSceneDeadline != 0L)
		{
			TowerDefenseControlNew towerDefenseControlNew = TowerDefenseManager.Instance?.currentControl;
			if (GodotObject.IsInstanceValid(towerDefenseControlNew) && towerDefenseControlNew.IsInsideTree())
			{
				_battleSceneDeadline = 0L;
			}
			else if (System.Environment.TickCount64 >= _battleSceneDeadline)
			{
				_CleanupConnection();
				SceneManager.Instance?.ChangeScene("MainMenu");
				ShowModCompatibilityFailure("联机场景加载超时，会话已结束。");
				return;
			}
		}
		if (_modEnvironmentLease != null && !_modEnvironmentLease.IsCurrent)
		{
			if (_modBattleActive)
			{
				_CleanupConnection();
				SceneManager.Instance?.ChangeScene("MainMenu");
				ShowModCompatibilityFailure("联机 Mod 环境意外变化，已终止本次会话。");
			}
			else
			{
				_hostModBatch?.Cancel("Mod 环境变化，请重新确认。");
				if (!isHost)
				{
					ReleaseBattleCompatibility();
				}
			}
		}
		if (!isHost && !_modBattleActive && _acceptedModBatch != "" && System.Environment.TickCount64 >= _remoteModDeadline)
		{
			ReleaseBattleCompatibility();
			ShowModCompatibilityFailure("等待开战超时，请由主机重新发起确认。");
		}
	}

	private void ModPeerChanged(string peer, bool disconnected)
	{
		_hostModBatch?.Cancel("房间成员变化，请重新确认。");
		if (!disconnected)
		{
			return;
		}
		_battleAdmitted.Remove(peer);
		ModCompatibilityBatch[] array = _modBatches.Values.ToArray();
		foreach (ModCompatibilityBatch modCompatibilityBatch in array)
		{
			if (modCompatibilityBatch.Candidates.Contains(peer))
			{
				modCompatibilityBatch.Cancel("成员已断线。");
			}
		}
	}

	public void ReleaseBattleCompatibility()
	{
		_modSceneAccepted?.TrySetResult(result: false);
		_modSceneAccepted = null;
		_remoteModTicket++;
		ModCompatibilityBatch[] array = _modBatches.Values.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Cancel("会话或战斗已结束。");
		}
		_modBatches.Clear();
		_modEnvironmentLease?.Dispose();
		_modEnvironmentLease = null;
		_battleAdmitted.Clear();
		_acceptedModBatch = (_pendingRemoteModBatch = (_modBattleId = ""));
		_acceptedLevelIdentity = null;
		_modBattleActive = false;
		_battleSceneDeadline = 0L;
		_activeBattleStartData = (_activeBattleLevelConfigData = "");
		_startGameTransitionPending = (_startGameTransitionCompleted = false);
	}

	private bool AllowsModMessage(NetMessageType type, string authenticatedSender)
	{
		if ((type == NetMessageType.StartGame || (uint)(type - 25) <= 1u || type == NetMessageType.LevelConfigAck) ? true : false)
		{
			return true;
		}
		if (!isHost)
		{
			return IsBattleParticipant(peerId);
		}
		if (!(authenticatedSender == peerId))
		{
			return IsBattleParticipant(authenticatedSender);
		}
		return true;
	}

	public MultiPlayerManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/MultiPlayerManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(124)
		{
			new MethodInfo(MethodName.EmitAllClientsReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitAllGameEntryAcked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPeerLatency, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "peerIdStr", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSignalLevel, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "peerIdStr", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._SendPing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._RpcPing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "pingId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RpcPong, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "pingId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RpcPingResult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LogOut, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsConnect, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanSendMatchRpc, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetUserName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetUserDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPeerName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "peerIdStr", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateMatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JoinMatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "address", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LeaveMatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendMatchState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "opCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendMatchStateUnreliable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "opCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendMatchStateToPeer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "opCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "targetPeerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RpcReceiveMatchState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "opCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RpcReceiveMatchStateUnreliable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "opCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._CreateEnvelopeData, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "opCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "deliveryMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "senderPeerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "forwarded", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._TryDispatchEnvelope, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "opCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "rpcSenderId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "deliveryMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._TryHandleUnreliableEnvelope, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "opCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "rpcSenderId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsEnvelopeSenderAuthorized, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "messageType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "rpcSenderId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RequiresHostAuthority, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "messageType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeAuthenticatedUserId, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "authenticatedSenderId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._BroadcastReliableEnvelope, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "opCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "senderPeerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "forwarded", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RejectLegacyProtocol, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "senderId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RejectProtocolVersion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "senderId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "remoteProtocolVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ProcessMatchState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "opCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "senderId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendSelectLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendLevelConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildLevelConfigData, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ConvertFeatureData, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "featureData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._SerializeFeatures, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "featureData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._FindLevelUid, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "difficult", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendStartGame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendLateJoinBootstrap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "targetPeerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendPlacePlant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "plantName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "overrideData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "requestId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "ownerPeerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "placementKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "targetSyncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hypnoses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendCommandRejected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "targetPeerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "rejectedOpCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "requestId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendRemovePlant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "gridX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendUseShovel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "gridX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendMoveCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "fromGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "toGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "moveKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendGameStateSync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "state", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendSpawnZombie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "zombieName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "offsetX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "spawnOverrideData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "spawnConfigOverrideData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendSpawnGrid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendGameResult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "victory", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendCharacterDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isExplode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isSmash", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendCharacterInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "hp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "die", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "loopAnim", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "blendTimeVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "frameIndexVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "timeScaleVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "walkSpeedScaleVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendCharacterStateSync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "charactersData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendCharacterPositionSync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendZombieFullSync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "zombiesData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendCursorSync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "posX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendCursorPickSync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "pickType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "pickName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendChooseReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendChooseOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendVaseBreakRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "gridX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendGemMatchCommand, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "action", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "upgradeKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendVaseBreakResult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "breakData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendPacketPick, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "pickType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendPause, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendResume, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendSpawnCoin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "posX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "velocityX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "velocityY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "collect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendSpawnFallingObject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "objectId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "velocityX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "velocityY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "gravity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendSpawnCharacterAt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "hitpointScale", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "scaleVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hypnoses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "riseDuration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "useCreate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "walkAfterSpawn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "groundHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sizeVal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "spawnState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "economyOwner", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendConveyorSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendClientReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendGameEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "roundNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendTipsPlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendDamagePart, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "partName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "posY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "velocityX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "velocityY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sequence", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendDamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendArmorDamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "stage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendCraterCreate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "gridX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "craterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendCharacterComponentOperation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "ownerSyncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "componentInstanceId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "componentTypeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sequence", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "operationName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "operationData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendPlantFullSync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "plantsData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendEventExecute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "eventsData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendWaveEventExecute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "waveId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "eventId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "eventsData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetClientsReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ResetSessionState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckAllClientsReady, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendLevelConfigAck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetLevelConfigAck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckAllLevelConfigAcked, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendGameEntryAck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetGameEntryAck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CheckAllGameEntryAcked, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMatchIdShort, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetLanIp, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetAllLanIps, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnPeerConnected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RpcSendHostInfo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "hostName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "members", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "hostVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Dictionary, "namesData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RpcVersionMismatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clientVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RpcSyncMembers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "members", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RpcSendClientName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "playerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._broadcastPeerNames, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._RpcSyncPeerNames, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "namesData", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnPeerDisconnected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReevaluateHostBarriersAfterPeerLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnConnectedToServer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnConnectionFailed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnServerDisconnected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._CleanupConnection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._HandleStartGame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._LoadLevelById, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "levelId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "difficult", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._HandleSelectLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._HandleLevelConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._CreateLevelConfigFromReceived, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MatchesAcceptedLevel, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsBattleParticipant, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowModCompatibilityFailure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReceiveModAck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sender", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "raw", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReceiveCompatibleLevelConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "raw", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CacheCompatibleLevelConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AcceptCompatibleStart, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "raw", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadAdmittedRoster, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessModCompatibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ModPeerChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "peer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "disconnected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseBattleCompatibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AllowsModMessage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "authenticatedSender", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EmitAllClientsReady && args.Count == 0)
		{
			EmitAllClientsReady();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitAllGameEntryAcked && args.Count == 0)
		{
			EmitAllGameEntryAcked();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPeerLatency && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetPeerLatency(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSignalLevel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetSignalLevel(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName._SendPing && args.Count == 0)
		{
			_SendPing();
			ret = default;
			return true;
		}
		if (method == MethodName._RpcPing && args.Count == 1)
		{
			_RpcPing(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._RpcPong && args.Count == 1)
		{
			_RpcPong(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._RpcPingResult && args.Count == 1)
		{
			_RpcPingResult(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.LogOut && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(LogOut());
			return true;
		}
		if (method == MethodName.IsConnect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsConnect());
			return true;
		}
		if (method == MethodName.CanSendMatchRpc && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSendMatchRpc());
			return true;
		}
		if (method == MethodName.GetUserName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetUserName());
			return true;
		}
		if (method == MethodName.GetUserDisplayName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetUserDisplayName());
			return true;
		}
		if (method == MethodName.GetPeerName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPeerName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateMatch && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CreateMatch());
			return true;
		}
		if (method == MethodName.JoinMatch && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(JoinMatch(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LeaveMatch && args.Count == 0)
		{
			LeaveMatch();
			ret = default;
			return true;
		}
		if (method == MethodName.SendMatchState && args.Count == 2)
		{
			SendMatchState(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendMatchStateUnreliable && args.Count == 2)
		{
			SendMatchStateUnreliable(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendMatchStateToPeer && args.Count == 3)
		{
			SendMatchStateToPeer(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName._RpcReceiveMatchState && args.Count == 2)
		{
			_RpcReceiveMatchState(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._RpcReceiveMatchStateUnreliable && args.Count == 2)
		{
			_RpcReceiveMatchStateUnreliable(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._CreateEnvelopeData && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<string>(_CreateEnvelopeData(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<NetDeliveryMode>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4])));
			return true;
		}
		if (method == MethodName._TryDispatchEnvelope && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(_TryDispatchEnvelope(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<NetDeliveryMode>(in args[3])));
			return true;
		}
		if (method == MethodName._TryHandleUnreliableEnvelope && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(_TryHandleUnreliableEnvelope(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.IsEnvelopeSenderAuthorized && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEnvelopeSenderAuthorized(VariantUtils.ConvertTo<NetMessageType>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.RequiresHostAuthority && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RequiresHostAuthority(VariantUtils.ConvertTo<NetMessageType>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeAuthenticatedUserId && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeAuthenticatedUserId(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName._BroadcastReliableEnvelope && args.Count == 4)
		{
			_BroadcastReliableEnvelope(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName._RejectLegacyProtocol && args.Count == 1)
		{
			_RejectLegacyProtocol(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._RejectProtocolVersion && args.Count == 2)
		{
			_RejectProtocolVersion(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._ProcessMatchState && args.Count == 3)
		{
			_ProcessMatchState(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendSelectLevel && args.Count == 1)
		{
			SendSelectLevel(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendLevelConfig && args.Count == 0)
		{
			SendLevelConfig();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildLevelConfigData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildLevelConfigData());
			return true;
		}
		if (method == MethodName._ConvertFeatureData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(_ConvertFeatureData(VariantUtils.ConvertToDictionary<StringName, Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName._SerializeFeatures && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(_SerializeFeatures(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName._FindLevelUid && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(_FindLevelUid(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SendStartGame && args.Count == 0)
		{
			SendStartGame();
			ret = default;
			return true;
		}
		if (method == MethodName.SendLateJoinBootstrap && args.Count == 1)
		{
			SendLateJoinBootstrap(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendPlacePlant && args.Count == 10)
		{
			SendPlacePlant(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]), VariantUtils.ConvertTo<string>(in args[6]), VariantUtils.ConvertTo<string>(in args[7]), VariantUtils.ConvertTo<int>(in args[8]), VariantUtils.ConvertTo<bool>(in args[9]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendCommandRejected && args.Count == 4)
		{
			SendCommandRejected(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendRemovePlant && args.Count == 2)
		{
			SendRemovePlant(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendUseShovel && args.Count == 2)
		{
			SendUseShovel(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendMoveCharacter && args.Count == 5)
		{
			SendMoveCharacter(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendGameStateSync && args.Count == 1)
		{
			SendGameStateSync(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendSpawnZombie && args.Count == 6)
		{
			SendSpawnZombie(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendSpawnGrid && args.Count == 4)
		{
			SendSpawnGrid(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendGameResult && args.Count == 1)
		{
			SendGameResult(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendCharacterDestroy && args.Count == 3)
		{
			SendCharacterDestroy(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendCharacterInit && args.Count == 11)
		{
			SendCharacterInit(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<string>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<double>(in args[7]), VariantUtils.ConvertTo<int>(in args[8]), VariantUtils.ConvertTo<double>(in args[9]), VariantUtils.ConvertTo<double>(in args[10]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendCharacterStateSync && args.Count == 1)
		{
			SendCharacterStateSync(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendCharacterPositionSync && args.Count == 3)
		{
			SendCharacterPositionSync(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendZombieFullSync && args.Count == 1)
		{
			SendZombieFullSync(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendCursorSync && args.Count == 2)
		{
			SendCursorSync(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendCursorPickSync && args.Count == 2)
		{
			SendCursorPickSync(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendChooseReady && args.Count == 0)
		{
			SendChooseReady();
			ret = default;
			return true;
		}
		if (method == MethodName.SendChooseOver && args.Count == 0)
		{
			SendChooseOver();
			ret = default;
			return true;
		}
		if (method == MethodName.SendVaseBreakRequest && args.Count == 2)
		{
			SendVaseBreakRequest(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendGemMatchCommand && args.Count == 4)
		{
			SendGemMatchCommand(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendVaseBreakResult && args.Count == 1)
		{
			SendVaseBreakResult(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendPacketPick && args.Count == 2)
		{
			SendPacketPick(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendPause && args.Count == 0)
		{
			SendPause();
			ret = default;
			return true;
		}
		if (method == MethodName.SendResume && args.Count == 0)
		{
			SendResume();
			ret = default;
			return true;
		}
		if (method == MethodName.SendSpawnCoin && args.Count == 8)
		{
			SendSpawnCoin(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendSpawnFallingObject && args.Count == 9)
		{
			SendSpawnFallingObject(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]), VariantUtils.ConvertTo<int>(in args[8]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendSpawnCharacterAt && args.Count == 16)
		{
			SendSpawnCharacterAt(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<double>(in args[7]), VariantUtils.ConvertTo<bool>(in args[8]), VariantUtils.ConvertTo<double>(in args[9]), VariantUtils.ConvertTo<double>(in args[10]), VariantUtils.ConvertTo<bool>(in args[11]), VariantUtils.ConvertTo<double>(in args[12]), VariantUtils.ConvertTo<string>(in args[13]), VariantUtils.ConvertTo<Dictionary>(in args[14]), VariantUtils.ConvertTo<string>(in args[15]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendConveyorSpawn && args.Count == 2)
		{
			SendConveyorSpawn(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendClientReady && args.Count == 0)
		{
			SendClientReady();
			ret = default;
			return true;
		}
		if (method == MethodName.SendGameEntry && args.Count == 1)
		{
			SendGameEntry(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendTipsPlay && args.Count == 2)
		{
			SendTipsPlay(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendDamagePart && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<long>(SendDamagePart(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<long>(in args[6])));
			return true;
		}
		if (method == MethodName.SendDamagePointReach && args.Count == 2)
		{
			SendDamagePointReach(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendArmorDamagePointReach && args.Count == 3)
		{
			SendArmorDamagePointReach(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendArmorHitpointsEmpty && args.Count == 2)
		{
			SendArmorHitpointsEmpty(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendCraterCreate && args.Count == 3)
		{
			SendCraterCreate(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendCharacterComponentOperation && args.Count == 6)
		{
			SendCharacterComponentOperation(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<long>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<Dictionary>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendPlantFullSync && args.Count == 1)
		{
			SendPlantFullSync(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendEventExecute && args.Count == 2)
		{
			SendEventExecute(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendWaveEventExecute && args.Count == 3)
		{
			SendWaveEventExecute(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetClientsReady && args.Count == 0)
		{
			ResetClientsReady();
			ret = default;
			return true;
		}
		if (method == MethodName._ResetSessionState && args.Count == 0)
		{
			_ResetSessionState();
			ret = default;
			return true;
		}
		if (method == MethodName.CheckAllClientsReady && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckAllClientsReady());
			return true;
		}
		if (method == MethodName.SendLevelConfigAck && args.Count == 0)
		{
			SendLevelConfigAck();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetLevelConfigAck && args.Count == 0)
		{
			ResetLevelConfigAck();
			ret = default;
			return true;
		}
		if (method == MethodName.CheckAllLevelConfigAcked && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckAllLevelConfigAcked());
			return true;
		}
		if (method == MethodName.SendGameEntryAck && args.Count == 0)
		{
			SendGameEntryAck();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetGameEntryAck && args.Count == 0)
		{
			ResetGameEntryAck();
			ret = default;
			return true;
		}
		if (method == MethodName.CheckAllGameEntryAcked && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CheckAllGameEntryAcked());
			return true;
		}
		if (method == MethodName.GetMatchIdShort && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetMatchIdShort());
			return true;
		}
		if (method == MethodName._GetLanIp && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(_GetLanIp());
			return true;
		}
		if (method == MethodName.GetAllLanIps && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetAllLanIps());
			return true;
		}
		if (method == MethodName._OnPeerConnected && args.Count == 1)
		{
			_OnPeerConnected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._RpcSendHostInfo && args.Count == 4)
		{
			_RpcSendHostInfo(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<Dictionary>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName._RpcVersionMismatch && args.Count == 1)
		{
			_RpcVersionMismatch(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._RpcSyncMembers && args.Count == 1)
		{
			_RpcSyncMembers(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._RpcSendClientName && args.Count == 1)
		{
			_RpcSendClientName(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._broadcastPeerNames && args.Count == 0)
		{
			_broadcastPeerNames();
			ret = default;
			return true;
		}
		if (method == MethodName._RpcSyncPeerNames && args.Count == 1)
		{
			_RpcSyncPeerNames(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnPeerDisconnected && args.Count == 1)
		{
			_OnPeerDisconnected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReevaluateHostBarriersAfterPeerLeft && args.Count == 0)
		{
			ReevaluateHostBarriersAfterPeerLeft();
			ret = default;
			return true;
		}
		if (method == MethodName._OnConnectedToServer && args.Count == 0)
		{
			_OnConnectedToServer();
			ret = default;
			return true;
		}
		if (method == MethodName._OnConnectionFailed && args.Count == 0)
		{
			_OnConnectionFailed();
			ret = default;
			return true;
		}
		if (method == MethodName._OnServerDisconnected && args.Count == 0)
		{
			_OnServerDisconnected();
			ret = default;
			return true;
		}
		if (method == MethodName._CleanupConnection && args.Count == 0)
		{
			_CleanupConnection();
			ret = default;
			return true;
		}
		if (method == MethodName._HandleStartGame && args.Count == 1)
		{
			_HandleStartGame(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._LoadLevelById && args.Count == 2)
		{
			_LoadLevelById(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._HandleSelectLevel && args.Count == 1)
		{
			_HandleSelectLevel(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._HandleLevelConfig && args.Count == 1)
		{
			_HandleLevelConfig(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._CreateLevelConfigFromReceived && args.Count == 0)
		{
			_CreateLevelConfigFromReceived();
			ret = default;
			return true;
		}
		if (method == MethodName.MatchesAcceptedLevel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MatchesAcceptedLevel(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.IsBattleParticipant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBattleParticipant(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShowModCompatibilityFailure && args.Count == 1)
		{
			ShowModCompatibilityFailure(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReceiveModAck && args.Count == 2)
		{
			ReceiveModAck(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReceiveCompatibleLevelConfig && args.Count == 1)
		{
			ReceiveCompatibleLevelConfig(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CacheCompatibleLevelConfig && args.Count == 1)
		{
			CacheCompatibleLevelConfig(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AcceptCompatibleStart && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(AcceptCompatibleStart(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadAdmittedRoster && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReadAdmittedRoster(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.ProcessModCompatibility && args.Count == 0)
		{
			ProcessModCompatibility();
			ret = default;
			return true;
		}
		if (method == MethodName.ModPeerChanged && args.Count == 2)
		{
			ModPeerChanged(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseBattleCompatibility && args.Count == 0)
		{
			ReleaseBattleCompatibility();
			ret = default;
			return true;
		}
		if (method == MethodName.AllowsModMessage && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AllowsModMessage(VariantUtils.ConvertTo<NetMessageType>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsEnvelopeSenderAuthorized && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEnvelopeSenderAuthorized(VariantUtils.ConvertTo<NetMessageType>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.RequiresHostAuthority && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RequiresHostAuthority(VariantUtils.ConvertTo<NetMessageType>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeAuthenticatedUserId && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeAuthenticatedUserId(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EmitAllClientsReady)
		{
			return true;
		}
		if (method == MethodName.EmitAllGameEntryAcked)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.GetPeerLatency)
		{
			return true;
		}
		if (method == MethodName.GetSignalLevel)
		{
			return true;
		}
		if (method == MethodName._SendPing)
		{
			return true;
		}
		if (method == MethodName._RpcPing)
		{
			return true;
		}
		if (method == MethodName._RpcPong)
		{
			return true;
		}
		if (method == MethodName._RpcPingResult)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.LogOut)
		{
			return true;
		}
		if (method == MethodName.IsConnect)
		{
			return true;
		}
		if (method == MethodName.CanSendMatchRpc)
		{
			return true;
		}
		if (method == MethodName.GetUserName)
		{
			return true;
		}
		if (method == MethodName.GetUserDisplayName)
		{
			return true;
		}
		if (method == MethodName.GetPeerName)
		{
			return true;
		}
		if (method == MethodName.CreateMatch)
		{
			return true;
		}
		if (method == MethodName.JoinMatch)
		{
			return true;
		}
		if (method == MethodName.LeaveMatch)
		{
			return true;
		}
		if (method == MethodName.SendMatchState)
		{
			return true;
		}
		if (method == MethodName.SendMatchStateUnreliable)
		{
			return true;
		}
		if (method == MethodName.SendMatchStateToPeer)
		{
			return true;
		}
		if (method == MethodName._RpcReceiveMatchState)
		{
			return true;
		}
		if (method == MethodName._RpcReceiveMatchStateUnreliable)
		{
			return true;
		}
		if (method == MethodName._CreateEnvelopeData)
		{
			return true;
		}
		if (method == MethodName._TryDispatchEnvelope)
		{
			return true;
		}
		if (method == MethodName._TryHandleUnreliableEnvelope)
		{
			return true;
		}
		if (method == MethodName.IsEnvelopeSenderAuthorized)
		{
			return true;
		}
		if (method == MethodName.RequiresHostAuthority)
		{
			return true;
		}
		if (method == MethodName.NormalizeAuthenticatedUserId)
		{
			return true;
		}
		if (method == MethodName._BroadcastReliableEnvelope)
		{
			return true;
		}
		if (method == MethodName._RejectLegacyProtocol)
		{
			return true;
		}
		if (method == MethodName._RejectProtocolVersion)
		{
			return true;
		}
		if (method == MethodName._ProcessMatchState)
		{
			return true;
		}
		if (method == MethodName.SendSelectLevel)
		{
			return true;
		}
		if (method == MethodName.SendLevelConfig)
		{
			return true;
		}
		if (method == MethodName.BuildLevelConfigData)
		{
			return true;
		}
		if (method == MethodName._ConvertFeatureData)
		{
			return true;
		}
		if (method == MethodName._SerializeFeatures)
		{
			return true;
		}
		if (method == MethodName._FindLevelUid)
		{
			return true;
		}
		if (method == MethodName.SendStartGame)
		{
			return true;
		}
		if (method == MethodName.SendLateJoinBootstrap)
		{
			return true;
		}
		if (method == MethodName.SendPlacePlant)
		{
			return true;
		}
		if (method == MethodName.SendCommandRejected)
		{
			return true;
		}
		if (method == MethodName.SendRemovePlant)
		{
			return true;
		}
		if (method == MethodName.SendUseShovel)
		{
			return true;
		}
		if (method == MethodName.SendMoveCharacter)
		{
			return true;
		}
		if (method == MethodName.SendGameStateSync)
		{
			return true;
		}
		if (method == MethodName.SendSpawnZombie)
		{
			return true;
		}
		if (method == MethodName.SendSpawnGrid)
		{
			return true;
		}
		if (method == MethodName.SendGameResult)
		{
			return true;
		}
		if (method == MethodName.SendCharacterDestroy)
		{
			return true;
		}
		if (method == MethodName.SendCharacterInit)
		{
			return true;
		}
		if (method == MethodName.SendCharacterStateSync)
		{
			return true;
		}
		if (method == MethodName.SendCharacterPositionSync)
		{
			return true;
		}
		if (method == MethodName.SendZombieFullSync)
		{
			return true;
		}
		if (method == MethodName.SendCursorSync)
		{
			return true;
		}
		if (method == MethodName.SendCursorPickSync)
		{
			return true;
		}
		if (method == MethodName.SendChooseReady)
		{
			return true;
		}
		if (method == MethodName.SendChooseOver)
		{
			return true;
		}
		if (method == MethodName.SendVaseBreakRequest)
		{
			return true;
		}
		if (method == MethodName.SendGemMatchCommand)
		{
			return true;
		}
		if (method == MethodName.SendVaseBreakResult)
		{
			return true;
		}
		if (method == MethodName.SendPacketPick)
		{
			return true;
		}
		if (method == MethodName.SendPause)
		{
			return true;
		}
		if (method == MethodName.SendResume)
		{
			return true;
		}
		if (method == MethodName.SendSpawnCoin)
		{
			return true;
		}
		if (method == MethodName.SendSpawnFallingObject)
		{
			return true;
		}
		if (method == MethodName.SendSpawnCharacterAt)
		{
			return true;
		}
		if (method == MethodName.SendConveyorSpawn)
		{
			return true;
		}
		if (method == MethodName.SendClientReady)
		{
			return true;
		}
		if (method == MethodName.SendGameEntry)
		{
			return true;
		}
		if (method == MethodName.SendTipsPlay)
		{
			return true;
		}
		if (method == MethodName.SendDamagePart)
		{
			return true;
		}
		if (method == MethodName.SendDamagePointReach)
		{
			return true;
		}
		if (method == MethodName.SendArmorDamagePointReach)
		{
			return true;
		}
		if (method == MethodName.SendArmorHitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.SendCraterCreate)
		{
			return true;
		}
		if (method == MethodName.SendCharacterComponentOperation)
		{
			return true;
		}
		if (method == MethodName.SendPlantFullSync)
		{
			return true;
		}
		if (method == MethodName.SendEventExecute)
		{
			return true;
		}
		if (method == MethodName.SendWaveEventExecute)
		{
			return true;
		}
		if (method == MethodName.ResetClientsReady)
		{
			return true;
		}
		if (method == MethodName._ResetSessionState)
		{
			return true;
		}
		if (method == MethodName.CheckAllClientsReady)
		{
			return true;
		}
		if (method == MethodName.SendLevelConfigAck)
		{
			return true;
		}
		if (method == MethodName.ResetLevelConfigAck)
		{
			return true;
		}
		if (method == MethodName.CheckAllLevelConfigAcked)
		{
			return true;
		}
		if (method == MethodName.SendGameEntryAck)
		{
			return true;
		}
		if (method == MethodName.ResetGameEntryAck)
		{
			return true;
		}
		if (method == MethodName.CheckAllGameEntryAcked)
		{
			return true;
		}
		if (method == MethodName.GetMatchIdShort)
		{
			return true;
		}
		if (method == MethodName._GetLanIp)
		{
			return true;
		}
		if (method == MethodName.GetAllLanIps)
		{
			return true;
		}
		if (method == MethodName._OnPeerConnected)
		{
			return true;
		}
		if (method == MethodName._RpcSendHostInfo)
		{
			return true;
		}
		if (method == MethodName._RpcVersionMismatch)
		{
			return true;
		}
		if (method == MethodName._RpcSyncMembers)
		{
			return true;
		}
		if (method == MethodName._RpcSendClientName)
		{
			return true;
		}
		if (method == MethodName._broadcastPeerNames)
		{
			return true;
		}
		if (method == MethodName._RpcSyncPeerNames)
		{
			return true;
		}
		if (method == MethodName._OnPeerDisconnected)
		{
			return true;
		}
		if (method == MethodName.ReevaluateHostBarriersAfterPeerLeft)
		{
			return true;
		}
		if (method == MethodName._OnConnectedToServer)
		{
			return true;
		}
		if (method == MethodName._OnConnectionFailed)
		{
			return true;
		}
		if (method == MethodName._OnServerDisconnected)
		{
			return true;
		}
		if (method == MethodName._CleanupConnection)
		{
			return true;
		}
		if (method == MethodName._HandleStartGame)
		{
			return true;
		}
		if (method == MethodName._LoadLevelById)
		{
			return true;
		}
		if (method == MethodName._HandleSelectLevel)
		{
			return true;
		}
		if (method == MethodName._HandleLevelConfig)
		{
			return true;
		}
		if (method == MethodName._CreateLevelConfigFromReceived)
		{
			return true;
		}
		if (method == MethodName.MatchesAcceptedLevel)
		{
			return true;
		}
		if (method == MethodName.IsBattleParticipant)
		{
			return true;
		}
		if (method == MethodName.ShowModCompatibilityFailure)
		{
			return true;
		}
		if (method == MethodName.ReceiveModAck)
		{
			return true;
		}
		if (method == MethodName.ReceiveCompatibleLevelConfig)
		{
			return true;
		}
		if (method == MethodName.CacheCompatibleLevelConfig)
		{
			return true;
		}
		if (method == MethodName.AcceptCompatibleStart)
		{
			return true;
		}
		if (method == MethodName.ReadAdmittedRoster)
		{
			return true;
		}
		if (method == MethodName.ProcessModCompatibility)
		{
			return true;
		}
		if (method == MethodName.ModPeerChanged)
		{
			return true;
		}
		if (method == MethodName.ReleaseBattleCompatibility)
		{
			return true;
		}
		if (method == MethodName.AllowsModMessage)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.currentMatchId)
		{
			currentMatchId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.isHost)
		{
			isHost = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.peerId)
		{
			peerId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.matchMembers)
		{
			matchMembers = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName.selectedLevelId)
		{
			selectedLevelId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._clientsReady)
		{
			_clientsReady = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName._gameEntryAcked)
		{
			_gameEntryAcked = VariantUtils.ConvertTo<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName.LastModCompatibilityFailure)
		{
			LastModCompatibilityFailure = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._peer)
		{
			_peer = VariantUtils.ConvertTo<ENetMultiplayerPeer>(in value);
			return true;
		}
		if (name == PropertyName._playerName)
		{
			_playerName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._receivedLevelConfigJson)
		{
			_receivedLevelConfigJson = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._receivedLevelConfigType)
		{
			_receivedLevelConfigType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._receivedLevelEnterMode)
		{
			_receivedLevelEnterMode = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._receivedLevelIsBattle)
		{
			_receivedLevelIsBattle = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._activeBattleLevelConfigData)
		{
			_activeBattleLevelConfigData = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._activeBattleStartData)
		{
			_activeBattleStartData = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._gameEntrySent)
		{
			_gameEntrySent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pingTimer)
		{
			_pingTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._reliableSequence)
		{
			_reliableSequence = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._unreliableSequence)
		{
			_unreliableSequence = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._fallingObjectSpawnSequence)
		{
			_fallingObjectSpawnSequence = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._damagePartSpawnSequence)
		{
			_damagePartSpawnSequence = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._sessionGeneration)
		{
			_sessionGeneration = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._startGameTransitionPending)
		{
			_startGameTransitionPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._startGameTransitionCompleted)
		{
			_startGameTransitionCompleted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._currentPort)
		{
			_currentPort = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._preparingModStart)
		{
			_preparingModStart = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._modBattleActive)
		{
			_modBattleActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._modBattleId)
		{
			_modBattleId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._acceptedModBatch)
		{
			_acceptedModBatch = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._pendingRemoteModBatch)
		{
			_pendingRemoteModBatch = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._remoteModTicket)
		{
			_remoteModTicket = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._remoteModDeadline)
		{
			_remoteModDeadline = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._battleSceneDeadline)
		{
			_battleSceneDeadline = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.currentMatchId)
		{
			from = currentMatchId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.isHost)
		{
			from2 = isHost;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.peerId)
		{
			from = peerId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Godot.Collections.Array from3;
		if (name == PropertyName.matchMembers)
		{
			from3 = matchMembers;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.selectedLevelId)
		{
			from = selectedLevelId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._clientsReady)
		{
			from3 = _clientsReady;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.ClientsReady)
		{
			from3 = ClientsReady;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName._gameEntryAcked)
		{
			from3 = _gameEntryAcked;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.GameEntryAcked)
		{
			from3 = GameEntryAcked;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.GameEntrySent)
		{
			from2 = GameEntrySent;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CurrentPort)
		{
			value = VariantUtils.CreateFrom<int>(CurrentPort);
			return true;
		}
		if (name == PropertyName.LastModCompatibilityFailure)
		{
			from = LastModCompatibilityFailure;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.BattleAdmittedPeers)
		{
			from3 = BattleAdmittedPeers;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName._peer)
		{
			value = VariantUtils.CreateFrom(in _peer);
			return true;
		}
		if (name == PropertyName._playerName)
		{
			value = VariantUtils.CreateFrom(in _playerName);
			return true;
		}
		if (name == PropertyName._receivedLevelConfigJson)
		{
			value = VariantUtils.CreateFrom(in _receivedLevelConfigJson);
			return true;
		}
		if (name == PropertyName._receivedLevelConfigType)
		{
			value = VariantUtils.CreateFrom(in _receivedLevelConfigType);
			return true;
		}
		if (name == PropertyName._receivedLevelEnterMode)
		{
			value = VariantUtils.CreateFrom(in _receivedLevelEnterMode);
			return true;
		}
		if (name == PropertyName._receivedLevelIsBattle)
		{
			value = VariantUtils.CreateFrom(in _receivedLevelIsBattle);
			return true;
		}
		if (name == PropertyName._activeBattleLevelConfigData)
		{
			value = VariantUtils.CreateFrom(in _activeBattleLevelConfigData);
			return true;
		}
		if (name == PropertyName._activeBattleStartData)
		{
			value = VariantUtils.CreateFrom(in _activeBattleStartData);
			return true;
		}
		if (name == PropertyName._gameEntrySent)
		{
			value = VariantUtils.CreateFrom(in _gameEntrySent);
			return true;
		}
		if (name == PropertyName._pingTimer)
		{
			value = VariantUtils.CreateFrom(in _pingTimer);
			return true;
		}
		if (name == PropertyName._reliableSequence)
		{
			value = VariantUtils.CreateFrom(in _reliableSequence);
			return true;
		}
		if (name == PropertyName._unreliableSequence)
		{
			value = VariantUtils.CreateFrom(in _unreliableSequence);
			return true;
		}
		if (name == PropertyName._fallingObjectSpawnSequence)
		{
			value = VariantUtils.CreateFrom(in _fallingObjectSpawnSequence);
			return true;
		}
		if (name == PropertyName._damagePartSpawnSequence)
		{
			value = VariantUtils.CreateFrom(in _damagePartSpawnSequence);
			return true;
		}
		if (name == PropertyName._sessionGeneration)
		{
			value = VariantUtils.CreateFrom(in _sessionGeneration);
			return true;
		}
		if (name == PropertyName._startGameTransitionPending)
		{
			value = VariantUtils.CreateFrom(in _startGameTransitionPending);
			return true;
		}
		if (name == PropertyName._startGameTransitionCompleted)
		{
			value = VariantUtils.CreateFrom(in _startGameTransitionCompleted);
			return true;
		}
		if (name == PropertyName._currentPort)
		{
			value = VariantUtils.CreateFrom(in _currentPort);
			return true;
		}
		if (name == PropertyName._preparingModStart)
		{
			value = VariantUtils.CreateFrom(in _preparingModStart);
			return true;
		}
		if (name == PropertyName._modBattleActive)
		{
			value = VariantUtils.CreateFrom(in _modBattleActive);
			return true;
		}
		if (name == PropertyName._modBattleId)
		{
			value = VariantUtils.CreateFrom(in _modBattleId);
			return true;
		}
		if (name == PropertyName._acceptedModBatch)
		{
			value = VariantUtils.CreateFrom(in _acceptedModBatch);
			return true;
		}
		if (name == PropertyName._pendingRemoteModBatch)
		{
			value = VariantUtils.CreateFrom(in _pendingRemoteModBatch);
			return true;
		}
		if (name == PropertyName._remoteModTicket)
		{
			value = VariantUtils.CreateFrom(in _remoteModTicket);
			return true;
		}
		if (name == PropertyName._remoteModDeadline)
		{
			value = VariantUtils.CreateFrom(in _remoteModDeadline);
			return true;
		}
		if (name == PropertyName._battleSceneDeadline)
		{
			value = VariantUtils.CreateFrom(in _battleSceneDeadline);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._peer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._playerName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.currentMatchId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.peerId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.matchMembers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.selectedLevelId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._receivedLevelConfigJson, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._receivedLevelConfigType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._receivedLevelEnterMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._receivedLevelIsBattle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._activeBattleLevelConfigData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._activeBattleStartData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._clientsReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.ClientsReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._gameEntryAcked, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.GameEntryAcked, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._gameEntrySent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.GameEntrySent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pingTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._reliableSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._unreliableSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._fallingObjectSpawnSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._damagePartSpawnSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._sessionGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._startGameTransitionPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._startGameTransitionCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentPort, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CurrentPort, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preparingModStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._modBattleActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._modBattleId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._acceptedModBatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingRemoteModBatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._remoteModTicket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._remoteModDeadline, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._battleSceneDeadline, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.LastModCompatibilityFailure, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.BattleAdmittedPeers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.currentMatchId, Variant.From<string>(currentMatchId));
		info.AddProperty(PropertyName.isHost, Variant.From<bool>(isHost));
		info.AddProperty(PropertyName.peerId, Variant.From<string>(peerId));
		info.AddProperty(PropertyName.matchMembers, Variant.From<Godot.Collections.Array>(matchMembers));
		info.AddProperty(PropertyName.selectedLevelId, Variant.From<string>(selectedLevelId));
		info.AddProperty(PropertyName._clientsReady, Variant.From<Godot.Collections.Array>(_clientsReady));
		info.AddProperty(PropertyName._gameEntryAcked, Variant.From<Godot.Collections.Array>(_gameEntryAcked));
		info.AddProperty(PropertyName.LastModCompatibilityFailure, Variant.From<string>(LastModCompatibilityFailure));
		info.AddProperty(PropertyName._peer, Variant.From(in _peer));
		info.AddProperty(PropertyName._playerName, Variant.From(in _playerName));
		info.AddProperty(PropertyName._receivedLevelConfigJson, Variant.From(in _receivedLevelConfigJson));
		info.AddProperty(PropertyName._receivedLevelConfigType, Variant.From(in _receivedLevelConfigType));
		info.AddProperty(PropertyName._receivedLevelEnterMode, Variant.From(in _receivedLevelEnterMode));
		info.AddProperty(PropertyName._receivedLevelIsBattle, Variant.From(in _receivedLevelIsBattle));
		info.AddProperty(PropertyName._activeBattleLevelConfigData, Variant.From(in _activeBattleLevelConfigData));
		info.AddProperty(PropertyName._activeBattleStartData, Variant.From(in _activeBattleStartData));
		info.AddProperty(PropertyName._gameEntrySent, Variant.From(in _gameEntrySent));
		info.AddProperty(PropertyName._pingTimer, Variant.From(in _pingTimer));
		info.AddProperty(PropertyName._reliableSequence, Variant.From(in _reliableSequence));
		info.AddProperty(PropertyName._unreliableSequence, Variant.From(in _unreliableSequence));
		info.AddProperty(PropertyName._fallingObjectSpawnSequence, Variant.From(in _fallingObjectSpawnSequence));
		info.AddProperty(PropertyName._damagePartSpawnSequence, Variant.From(in _damagePartSpawnSequence));
		info.AddProperty(PropertyName._sessionGeneration, Variant.From(in _sessionGeneration));
		info.AddProperty(PropertyName._startGameTransitionPending, Variant.From(in _startGameTransitionPending));
		info.AddProperty(PropertyName._startGameTransitionCompleted, Variant.From(in _startGameTransitionCompleted));
		info.AddProperty(PropertyName._currentPort, Variant.From(in _currentPort));
		info.AddProperty(PropertyName._preparingModStart, Variant.From(in _preparingModStart));
		info.AddProperty(PropertyName._modBattleActive, Variant.From(in _modBattleActive));
		info.AddProperty(PropertyName._modBattleId, Variant.From(in _modBattleId));
		info.AddProperty(PropertyName._acceptedModBatch, Variant.From(in _acceptedModBatch));
		info.AddProperty(PropertyName._pendingRemoteModBatch, Variant.From(in _pendingRemoteModBatch));
		info.AddProperty(PropertyName._remoteModTicket, Variant.From(in _remoteModTicket));
		info.AddProperty(PropertyName._remoteModDeadline, Variant.From(in _remoteModDeadline));
		info.AddProperty(PropertyName._battleSceneDeadline, Variant.From(in _battleSceneDeadline));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.currentMatchId, out var value))
		{
			currentMatchId = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.isHost, out var value2))
		{
			isHost = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.peerId, out var value3))
		{
			peerId = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.matchMembers, out var value4))
		{
			matchMembers = value4.As<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName.selectedLevelId, out var value5))
		{
			selectedLevelId = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName._clientsReady, out var value6))
		{
			_clientsReady = value6.As<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName._gameEntryAcked, out var value7))
		{
			_gameEntryAcked = value7.As<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName.LastModCompatibilityFailure, out var value8))
		{
			LastModCompatibilityFailure = value8.As<string>();
		}
		if (info.TryGetProperty(PropertyName._peer, out var value9))
		{
			_peer = value9.As<ENetMultiplayerPeer>();
		}
		if (info.TryGetProperty(PropertyName._playerName, out var value10))
		{
			_playerName = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName._receivedLevelConfigJson, out var value11))
		{
			_receivedLevelConfigJson = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName._receivedLevelConfigType, out var value12))
		{
			_receivedLevelConfigType = value12.As<string>();
		}
		if (info.TryGetProperty(PropertyName._receivedLevelEnterMode, out var value13))
		{
			_receivedLevelEnterMode = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName._receivedLevelIsBattle, out var value14))
		{
			_receivedLevelIsBattle = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._activeBattleLevelConfigData, out var value15))
		{
			_activeBattleLevelConfigData = value15.As<string>();
		}
		if (info.TryGetProperty(PropertyName._activeBattleStartData, out var value16))
		{
			_activeBattleStartData = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName._gameEntrySent, out var value17))
		{
			_gameEntrySent = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pingTimer, out var value18))
		{
			_pingTimer = value18.As<double>();
		}
		if (info.TryGetProperty(PropertyName._reliableSequence, out var value19))
		{
			_reliableSequence = value19.As<long>();
		}
		if (info.TryGetProperty(PropertyName._unreliableSequence, out var value20))
		{
			_unreliableSequence = value20.As<long>();
		}
		if (info.TryGetProperty(PropertyName._fallingObjectSpawnSequence, out var value21))
		{
			_fallingObjectSpawnSequence = value21.As<long>();
		}
		if (info.TryGetProperty(PropertyName._damagePartSpawnSequence, out var value22))
		{
			_damagePartSpawnSequence = value22.As<long>();
		}
		if (info.TryGetProperty(PropertyName._sessionGeneration, out var value23))
		{
			_sessionGeneration = value23.As<long>();
		}
		if (info.TryGetProperty(PropertyName._startGameTransitionPending, out var value24))
		{
			_startGameTransitionPending = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._startGameTransitionCompleted, out var value25))
		{
			_startGameTransitionCompleted = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._currentPort, out var value26))
		{
			_currentPort = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName._preparingModStart, out var value27))
		{
			_preparingModStart = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._modBattleActive, out var value28))
		{
			_modBattleActive = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._modBattleId, out var value29))
		{
			_modBattleId = value29.As<string>();
		}
		if (info.TryGetProperty(PropertyName._acceptedModBatch, out var value30))
		{
			_acceptedModBatch = value30.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pendingRemoteModBatch, out var value31))
		{
			_pendingRemoteModBatch = value31.As<string>();
		}
		if (info.TryGetProperty(PropertyName._remoteModTicket, out var value32))
		{
			_remoteModTicket = value32.As<long>();
		}
		if (info.TryGetProperty(PropertyName._remoteModDeadline, out var value33))
		{
			_remoteModDeadline = value33.As<long>();
		}
		if (info.TryGetProperty(PropertyName._battleSceneDeadline, out var value34))
		{
			_battleSceneDeadline = value34.As<long>();
		}
	}
}
