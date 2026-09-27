using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Battle/Feature/PacketPick/Control/PacketPickControl.cs")]
public class PacketPickControl : Node2D
{
	private sealed class PendingPlantRequest
	{
		public TowerDefenseInGamePacketShow Packet;

		public int PacketSyncId;

		public int ExpectedCount;

		public int RejectedCount;

		public ulong DeadlineMsec;

		public SunSpendReceipt SpendReceipt;

		public bool WasAlive;

		public bool WasColdDownOpen;

		public double ColdDown;

		public double ColdDownTimer;
	}

	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName FindColumnJalaVase = "FindColumnJalaVase";

		public static readonly StringName CanPlaceColumnTargetInCell = "CanPlaceColumnTargetInCell";

		public static readonly StringName FindColumnZombie = "FindColumnZombie";

		public static readonly StringName ProcessColumnTargetPlacement = "ProcessColumnTargetPlacement";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName RollbackPendingPlantRequests = "RollbackPendingPlantRequests";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName Init = "Init";

		public static readonly StringName EnsurePreviewRenderMounts = "EnsurePreviewRenderMounts";

		public static readonly StringName CreatePreviewRenderMount = "CreatePreviewRenderMount";

		public static readonly StringName RegisterTool = "RegisterTool";

		public static readonly StringName UnregisterTool = "UnregisterTool";

		public static readonly StringName PruneInvalidTools = "PruneInvalidTools";

		public static readonly StringName SubscribeMultiplayerEvents = "SubscribeMultiplayerEvents";

		public static readonly StringName GetPendingRequestTimeoutMsec = "GetPendingRequestTimeoutMsec";

		public static readonly StringName UpdatePendingRequestProcessing = "UpdatePendingRequestProcessing";

		public static readonly StringName SweepExpiredPlantRequests = "SweepExpiredPlantRequests";

		public static readonly StringName OnMultiplayerMatchStateReceived = "OnMultiplayerMatchStateReceived";

		public static readonly StringName HandlePendingPlantAccepted = "HandlePendingPlantAccepted";

		public static readonly StringName HandlePendingPlantRejected = "HandlePendingPlantRejected";

		public static readonly StringName IsPicking = "IsPicking";

		public static readonly StringName NeedsInputProcessing = "NeedsInputProcessing";

		public static readonly StringName CreatePreviewSprite = "CreatePreviewSprite";

		public static readonly StringName ConfigurePreviewRenderTree = "ConfigurePreviewRenderTree";

		public static readonly StringName ReleasePreviewRenderTree = "ReleasePreviewRenderTree";

		public static readonly StringName ResolvePlacementPreviewZIndex = "ResolvePlacementPreviewZIndex";

		public static readonly StringName SetPlacementPreviewRenderRow = "SetPlacementPreviewRenderRow";

		public static readonly StringName PositionPlacementPreview = "PositionPlacementPreview";

		public static readonly StringName FreePreviewSprites = "FreePreviewSprites";

		public static readonly StringName ApplyArmorSprite = "ApplyArmorSprite";

		public static readonly StringName FindZombieAtPosition = "FindZombieAtPosition";

		public static readonly StringName FindPlantAtPosition = "FindPlantAtPosition";

		public static readonly StringName FindPlantInCell = "FindPlantInCell";

		public static readonly StringName ProcessPacketPick = "ProcessPacketPick";

		public static readonly StringName ShouldPreferCraterRepair = "ShouldPreferCraterRepair";

		public static readonly StringName ProcessTools = "ProcessTools";

		public static readonly StringName PickPacket = "PickPacket";

		public static readonly StringName UpdateSelectionGestureConfirmSuppression = "UpdateSelectionGestureConfirmSuppression";

		public static readonly StringName Release = "Release";

		public static readonly StringName ProcessReleaseInput = "ProcessReleaseInput";

		public static readonly StringName IsPlacementSurfacePoint = "IsPlacementSurfacePoint";

		public static readonly StringName PacketPickRelease = "PacketPickRelease";

		public static readonly StringName DisposeBattleState = "DisposeBattleState";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName mapControl = "mapControl";

		public static readonly StringName mapFeature = "mapFeature";

		public static readonly StringName _config = "_config";

		public static readonly StringName packetPick = "packetPick";

		public static readonly StringName followSprite = "followSprite";

		public static readonly StringName selectPacketTimer = "selectPacketTimer";

		public static readonly StringName _wasPicking = "_wasPicking";

		public static readonly StringName _toolActivateGrace = "_toolActivateGrace";

		public static readonly StringName _suppressSelectionGestureConfirm = "_suppressSelectionGestureConfirm";

		public static readonly StringName _plantRequestSerial = "_plantRequestSerial";

		public static readonly StringName _multiplayerEventsSubscribed = "_multiplayerEventsSubscribed";

		public static readonly StringName _pendingRequestSweepTimer = "_pendingRequestSweepTimer";

		public static readonly StringName _placementPreviewRenderMount = "_placementPreviewRenderMount";

		public static readonly StringName _followPreviewRenderMount = "_followPreviewRenderMount";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const int PlacementPreviewRowTopLayer = 14;

	internal const int FollowPreviewTopZIndex = 4096;

	public TowerDefenseMapControl mapControl;

	public TowerDefenseBattleFeatureMap mapFeature;

	private TowerDefenseBattleFeaturePacketPickConfig _config;

	public TowerDefenseInGamePacketShow packetPick;

	public AdobeAnimateSprite followSprite;

	public List<AdobeAnimateSprite> plantSpriteList = new List<AdobeAnimateSprite>();

	public int selectPacketTimer;

	public List<PacketPickTool> tools = new List<PacketPickTool>();

	private bool _wasPicking;

	private int _toolActivateGrace;

	private bool _suppressSelectionGestureConfirm;

	private readonly System.Collections.Generic.Dictionary<string, PendingPlantRequest> _pendingPlantRequests = new System.Collections.Generic.Dictionary<string, PendingPlantRequest>();

	private readonly List<string> _expiredPlantRequestIds = new List<string>();

	private int _plantRequestSerial;

	private bool _multiplayerEventsSubscribed;

	private double _pendingRequestSweepTimer;

	private Control _placementPreviewRenderMount;

	private Control _followPreviewRenderMount;

	internal List<TowerDefenseInGamePacketShow.ColumnPlacement> CollectColumnPlacements(int column, int? limitPlantGridNum = null)
	{
		List<TowerDefenseInGamePacketShow.ColumnPlacement> list = new List<TowerDefenseInGamePacketShow.ColumnPlacement>();
		if (column < 1 || column > mapFeature.config.gridNum.X)
		{
			return list;
		}
		TowerDefensePacketConfig config = packetPick.config;
		ColumnPlantGridBudget columnPlantGridBudget = new ColumnPlantGridBudget(config, limitPlantGridNum);
		bool flag = config.characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.PLANT);
		bool flag2 = false;
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in config.characterConfig.plantGridType)
		{
			flag2 |= item != TowerDefenseEnum.PLANTGRIDTYPE.PLANT;
		}
		bool hypnoses = config.GetHypnoses();
		TowerDefenseEnum.CHARACTER_CAMP targetCamp = ((!hypnoses) ? TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE : TowerDefenseEnum.CHARACTER_CAMP.PLANT);
		Godot.Collections.Array array = (config.canPlaceOnZombie ? TowerDefenseManager.Instance.GetZombie() : null);
		HashSet<TowerDefenseCharacter> hashSet = new HashSet<TowerDefenseCharacter>();
		for (int i = 1; i <= mapFeature.config.gridNum.Y; i++)
		{
			Vector2I vector2I = new Vector2I(column, i);
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(vector2I);
			if (!GodotObject.IsInstanceValid(mapCell) || GodotObject.IsInstanceValid(mapCell.FindPlantingBlocker(config)))
			{
				continue;
			}
			bool flag3 = CanPlaceColumnTargetInCell(mapCell);
			bool flag4 = ShouldPreferCraterRepair(mapCell);
			if ((flag3 & flag4) && columnPlantGridBudget.CanPlace(vector2I))
			{
				list.Add(new TowerDefenseInGamePacketShow.ColumnPlacement(vector2I, null, "grid"));
				columnPlantGridBudget.Reserve(vector2I);
				continue;
			}
			if (array != null && !flag4)
			{
				TowerDefenseCharacter towerDefenseCharacter = FindColumnZombie(array, vector2I, targetCamp);
				if (GodotObject.IsInstanceValid(towerDefenseCharacter) && hashSet.Add(towerDefenseCharacter))
				{
					list.Add(new TowerDefenseInGamePacketShow.ColumnPlacement(vector2I, towerDefenseCharacter, "zombie"));
					continue;
				}
			}
			if (!flag3)
			{
				continue;
			}
			TowerDefensePlantJalaVase towerDefensePlantJalaVase = FindColumnJalaVase(mapCell, config);
			if (GodotObject.IsInstanceValid(towerDefensePlantJalaVase))
			{
				if (hashSet.Add(towerDefensePlantJalaVase))
				{
					list.Add(new TowerDefenseInGamePacketShow.ColumnPlacement(vector2I, towerDefensePlantJalaVase, "jala_vase"));
				}
			}
			else if (flag)
			{
				if (!flag2 || mapCell.CanPacketPlant(config))
				{
					TowerDefenseCharacter towerDefenseCharacter2 = FindPlantInCell(mapCell, hypnoses);
					if (GodotObject.IsInstanceValid(towerDefenseCharacter2) && hashSet.Add(towerDefenseCharacter2))
					{
						list.Add(new TowerDefenseInGamePacketShow.ColumnPlacement(vector2I, towerDefenseCharacter2, "plant"));
					}
				}
			}
			else if (mapCell.CanPacketPlant(config) && columnPlantGridBudget.CanPlace(vector2I))
			{
				list.Add(new TowerDefenseInGamePacketShow.ColumnPlacement(vector2I, null, "grid"));
				columnPlantGridBudget.Reserve(vector2I);
			}
		}
		return list;
	}

	private static TowerDefensePlantJalaVase FindColumnJalaVase(TowerDefenseCellInstance cell, TowerDefensePacketConfig config)
	{
		foreach (TowerDefenseCharacter character in cell.characterList)
		{
			if (character is TowerDefensePlantJalaVase towerDefensePlantJalaVase && GodotObject.IsInstanceValid(towerDefensePlantJalaVase) && towerDefensePlantJalaVase.CanAddJala(config))
			{
				return towerDefensePlantJalaVase;
			}
		}
		return null;
	}

	private bool CanPlaceColumnTargetInCell(TowerDefenseCellInstance cell)
	{
		TowerDefensePacketConfig config = packetPick.config;
		if (TowerDefenseManager.Instance.IsIZMMode() && config.izmPlantAllCell)
		{
			return true;
		}
		if (mapFeature.stripeRow == -1)
		{
			return true;
		}
		if ((config.characterConfig is TowerDefensePlantConfig || config.izmPlantLeft) && cell.gridPos.X > mapFeature.stripeRow)
		{
			return false;
		}
		if (config.characterConfig is TowerDefenseZombieConfig && !config.izmPlantLeft)
		{
			return cell.gridPos.X > mapFeature.stripeRow;
		}
		return true;
	}

	private static TowerDefenseCharacter FindColumnZombie(Godot.Collections.Array zombies, Vector2I gridPosition, TowerDefenseEnum.CHARACTER_CAMP targetCamp)
	{
		TowerDefenseCharacter result = null;
		double num = 1.0 / 0.0;
		Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(gridPosition);
		foreach (Variant zombie in zombies)
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)zombie;
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter) || towerDefenseCharacter.isDestroy || towerDefenseCharacter.die || towerDefenseCharacter.nearDie || !GodotObject.IsInstanceValid(towerDefenseCharacter.instance) || towerDefenseCharacter.instance.invincible || !towerDefenseCharacter.instance.canBeCollection || towerDefenseCharacter.camp != targetCamp || towerDefenseCharacter.gridPos.Y != gridPosition.Y)
			{
				continue;
			}
			Vector2 logicalGlobalPosition = towerDefenseCharacter.GetLogicalGlobalPosition();
			if (TowerDefenseManager.Instance.GetMapGridPos(logicalGlobalPosition).X == gridPosition.X)
			{
				double num2 = logicalGlobalPosition.DistanceSquaredTo(mapCellPlantPos);
				if (num2 < num)
				{
					num = num2;
					result = towerDefenseCharacter;
				}
			}
		}
		return result;
	}

	private bool ProcessColumnTargetPlacement(Vector2I gridPosition, bool suppressPlacementConfirm, int limitPlantGridNum)
	{
		if (!TowerDefenseManager.Instance.CheckMapGridPosIn(gridPosition))
		{
			return false;
		}
		List<TowerDefenseInGamePacketShow.ColumnPlacement> list = CollectColumnPlacements(gridPosition.X, limitPlantGridNum);
		foreach (TowerDefenseInGamePacketShow.ColumnPlacement item in list)
		{
			int num = item.GridPos.Y - 1;
			if (num < plantSpriteList.Count)
			{
				Vector2 mapWorldPosition = (GodotObject.IsInstanceValid(item.Target) ? item.Target.GetLogicalGlobalPosition() : (TowerDefenseManager.GetMapCellPlantPos(item.GridPos) - new Vector2(0f, (float)mapFeature.GetGroundHeight(TowerDefenseManager.GetMapCell(item.GridPos)))));
				plantSpriteList[num].Visible = true;
				PositionPlacementPreview(plantSpriteList[num], mapWorldPosition, item.GridPos.Y);
			}
		}
		if (suppressPlacementConfirm || !mapControl.IsConfirmInput())
		{
			return false;
		}
		if (list.Count == 0 || !ProcessColumnPlacements(list))
		{
			TowerDefenseManager.GetMapCell(gridPosition)?.NotifyBlockedPlanting(packetPick.config);
			return false;
		}
		if (!Global.IsEditor || SceneManager.CurrentScene != "LevelEditorStage")
		{
			Release();
		}
		else
		{
			LevelEditorMapEditor.instance.levelConfig.canExport = false;
		}
		return true;
	}

	internal bool ProcessColumnPlacements(IReadOnlyList<TowerDefenseInGamePacketShow.ColumnPlacement> placements)
	{
		TowerDefenseInGamePacketShow packet = packetPick;
		if (!Global.IsMultiplayerMode)
		{
			return packet.PlantColumnTargets(placements).Count > 0;
		}
		if (MultiPlayerManager.Instance == null)
		{
			return false;
		}
		List<TowerDefenseInGamePacketShow.ColumnPlacement> list = new List<TowerDefenseInGamePacketShow.ColumnPlacement>();
		foreach (TowerDefenseInGamePacketShow.ColumnPlacement placement in placements)
		{
			TowerDefenseCharacter target = placement.Target;
			if (placement.Kind == "grid" || (GodotObject.IsInstanceValid(target) && target.syncId >= 0))
			{
				list.Add(placement);
			}
		}
		if (list.Count == 0)
		{
			return false;
		}
		string overrideData = (GodotObject.IsInstanceValid(packet.config._override) ? Json.Stringify(packet.config._override.Export()) : "");
		bool hypnoses = packet.config.GetHypnoses();
		if (!MultiPlayerManager.Instance.isHost)
		{
			if (!TryBeginPendingPlantRequest(packet, list.Count, out var requestId))
			{
				return false;
			}
			if (packet.HasMeta("packet_sync_id"))
			{
				packet.SetMeta("packet_pending_plant", requestId);
			}
			foreach (TowerDefenseInGamePacketShow.ColumnPlacement item in list)
			{
				MultiPlayerManager.Instance.SendPlacePlant(packet.config.saveKey, item.GridPos.X, item.GridPos.Y, -1, overrideData, requestId, "", item.Kind, item.Target?.syncId ?? (-1), hypnoses);
			}
			return true;
		}
		return packet.PlantColumnTargets(list, (TowerDefenseInGamePacketShow.ColumnPlacement placement, TowerDefenseCharacter character) =>
		{
			int num = character.syncId;
			if (placement.Kind != "jala_vase")
			{
				num = TowerDefenseManager.Instance.currentControl.GetNextSyncId();
				TowerDefenseManager.Instance.currentControl.RegisterSyncCharacter(num, character);
			}
			MultiPlayerManager.Instance.SendPlacePlant(packet.config.saveKey, placement.GridPos.X, placement.GridPos.Y, num, overrideData, "", "", placement.Kind, placement.Target?.syncId ?? (-1), hypnoses);
		}).Count > 0;
	}

	public override void _Ready()
	{
		SetProcess(_pendingPlantRequests.Count > 0);
		SubscribeMultiplayerEvents();
	}

	public override void _ExitTree()
	{
		DisposeBattleState();
		base._ExitTree();
	}

	private void RollbackPendingPlantRequests()
	{
		foreach (PendingPlantRequest value in _pendingPlantRequests.Values)
		{
			value.SpendReceipt?.TryRollback();
			if (GodotObject.IsInstanceValid(value.Packet) && value.Packet.HasMeta("packet_pending_plant"))
			{
				value.Packet.RemoveMeta("packet_pending_plant");
			}
		}
		_pendingPlantRequests.Clear();
		_expiredPlantRequestIds.Clear();
		_pendingRequestSweepTimer = 0.0;
		SetProcess(enable: false);
	}

	public override void _Process(double delta)
	{
		if (_pendingPlantRequests.Count == 0)
		{
			_pendingRequestSweepTimer = 0.0;
			return;
		}
		_pendingRequestSweepTimer += delta;
		if (!(_pendingRequestSweepTimer < (_config?.pendingRequestSweepIntervalSeconds ?? 0.5)))
		{
			_pendingRequestSweepTimer = 0.0;
			SweepExpiredPlantRequests(Time.GetTicksMsec());
		}
	}

	public void Init(TowerDefenseMapControl _mapControl, TowerDefenseBattleFeatureMap _mapFeature, TowerDefenseBattleFeaturePacketPickConfig config = null)
	{
		mapControl = _mapControl;
		mapFeature = _mapFeature;
		_config = config;
		EnsurePreviewRenderMounts();
		SubscribeMultiplayerEvents();
	}

	private void EnsurePreviewRenderMounts()
	{
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(characterNode) || !GodotObject.IsInstanceValid(mapControl?.spriteNode))
		{
			GD.PushError("PacketPick preview render mounts require both the battle character node and map overlay node.");
			return;
		}
		if (!GodotObject.IsInstanceValid(_placementPreviewRenderMount))
		{
			_placementPreviewRenderMount = CreatePreviewRenderMount("PacketPickPlacementPreviewRenderMount");
			characterNode.AddChild(_placementPreviewRenderMount, forceReadableName: false, InternalMode.Disabled);
		}
		if (!GodotObject.IsInstanceValid(_followPreviewRenderMount))
		{
			_followPreviewRenderMount = CreatePreviewRenderMount("PacketPickFollowPreviewRenderMount");
			mapControl.spriteNode.AddChild(_followPreviewRenderMount, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private static Control CreatePreviewRenderMount(string nodeName)
	{
		return new Control
		{
			Name = nodeName,
			MouseFilter = Control.MouseFilterEnum.Ignore
		};
	}

	public void RegisterTool(PacketPickTool tool)
	{
		PruneInvalidTools();
		if (GodotObject.IsInstanceValid(tool) && !tools.Contains(tool))
		{
			tools.Add(tool);
		}
	}

	public void UnregisterTool(PacketPickTool tool)
	{
		if (GodotObject.IsInstanceValid(tool))
		{
			tool.ToolReset();
		}
		tools.Remove(tool);
		PruneInvalidTools();
	}

	private void PruneInvalidTools()
	{
		tools.RemoveAll((PacketPickTool tool) => !GodotObject.IsInstanceValid(tool));
	}

	private void SubscribeMultiplayerEvents()
	{
		if (!_multiplayerEventsSubscribed && MultiPlayerManager.Instance != null)
		{
			MultiPlayerManager.Instance.OnMatchStateReceived += OnMultiplayerMatchStateReceived;
			_multiplayerEventsSubscribed = true;
		}
	}

	private bool TryBeginPendingPlantRequest(TowerDefenseInGamePacketShow packet, int expectedCount, out string requestId)
	{
		requestId = "";
		SubscribeMultiplayerEvents();
		if (MultiPlayerManager.Instance == null || !GodotObject.IsInstanceValid(packet))
		{
			return false;
		}
		string[] array = new string[5]
		{
			MultiPlayerManager.Instance.peerId,
			":",
			Time.GetTicksMsec().ToString(),
			":",
			null
		};
		array[4] = (++_plantRequestSerial).ToString();
		requestId = string.Concat(array);
		PendingPlantRequest pendingPlantRequest = new PendingPlantRequest
		{
			Packet = packet,
			PacketSyncId = (packet.HasMeta("packet_sync_id") ? packet.GetMeta("packet_sync_id").AsInt32() : (-1)),
			ExpectedCount = Math.Max(1, expectedCount),
			DeadlineMsec = Time.GetTicksMsec() + GetPendingRequestTimeoutMsec(),
			WasAlive = packet.alive,
			WasColdDownOpen = packet.coldDownOpen,
			ColdDown = packet.coldDown,
			ColdDownTimer = packet.coldDownTimer
		};
		if (!packet.TryBeginPendingUse(out pendingPlantRequest.SpendReceipt))
		{
			requestId = "";
			return false;
		}
		_pendingPlantRequests[requestId] = pendingPlantRequest;
		UpdatePendingRequestProcessing();
		return true;
	}

	private ulong GetPendingRequestTimeoutMsec()
	{
		return (ulong)Math.Ceiling(Math.Clamp(_config?.pendingRequestTimeoutSeconds ?? 15.0, 1.0, 120.0) * 1000.0);
	}

	private void UpdatePendingRequestProcessing()
	{
		SetProcess(_pendingPlantRequests.Count > 0);
	}

	private void SweepExpiredPlantRequests(ulong nowMsec)
	{
		_expiredPlantRequestIds.Clear();
		foreach (KeyValuePair<string, PendingPlantRequest> pendingPlantRequest in _pendingPlantRequests)
		{
			if (nowMsec >= pendingPlantRequest.Value.DeadlineMsec)
			{
				_expiredPlantRequestIds.Add(pendingPlantRequest.Key);
			}
		}
		foreach (string expiredPlantRequestId in _expiredPlantRequestIds)
		{
			if (_pendingPlantRequests.TryGetValue(expiredPlantRequestId, out var value))
			{
				FinalizePendingPlantRejected(expiredPlantRequestId, value);
			}
		}
		_expiredPlantRequestIds.Clear();
	}

	private void OnMultiplayerMatchStateReceived(string opCode, string data, string senderId)
	{
		if ((opCode != "place_plant" && opCode != "command_rejected") || MultiPlayerManager.Instance == null || MultiPlayerManager.Instance.isHost || senderId != "1")
		{
			return;
		}
		Variant variant = Json.ParseString(data);
		if (variant.VariantType != Variant.Type.Dictionary)
		{
			return;
		}
		Dictionary dictionary = variant.AsGodotDictionary();
		string text = dictionary.GetValueOrDefault("request_id", "").AsString();
		if (text == "" || !_pendingPlantRequests.ContainsKey(text))
		{
			return;
		}
		if (opCode == "place_plant")
		{
			HandlePendingPlantAccepted(text);
		}
		else if (!(dictionary.GetValueOrDefault("op_code", "").AsString() != "place_plant"))
		{
			string text2 = dictionary.GetValueOrDefault("target_user_id", "").AsString();
			if (MultiPlayerManager.Instance == null || !(text2 != "") || !(text2 != MultiPlayerManager.Instance.peerId))
			{
				HandlePendingPlantRejected(text);
			}
		}
	}

	private void HandlePendingPlantAccepted(string requestId)
	{
		if (_pendingPlantRequests.TryGetValue(requestId, out var value))
		{
			_pendingPlantRequests.Remove(requestId);
			UpdatePendingRequestProcessing();
			if (GodotObject.IsInstanceValid(value.Packet))
			{
				value.Packet.TryCommitPendingUse(value.SpendReceipt);
			}
			else
			{
				value.SpendReceipt?.TryCommit();
			}
			if (value.PacketSyncId >= 0)
			{
				MultiPlayerManager.Instance?.SendPacketPick(value.PacketSyncId);
			}
			if (GodotObject.IsInstanceValid(value.Packet) && value.Packet.HasMeta("packet_pending_plant"))
			{
				value.Packet.RemoveMeta("packet_pending_plant");
			}
		}
	}

	private void HandlePendingPlantRejected(string requestId)
	{
		if (_pendingPlantRequests.TryGetValue(requestId, out var value))
		{
			value.RejectedCount++;
			if (value.RejectedCount >= value.ExpectedCount)
			{
				FinalizePendingPlantRejected(requestId, value);
			}
		}
	}

	private void FinalizePendingPlantRejected(string requestId, PendingPlantRequest pending)
	{
		_pendingPlantRequests.Remove(requestId);
		UpdatePendingRequestProcessing();
		if (pending.PacketSyncId >= 0)
		{
			MultiPlayerManager.Instance?.SendPacketPick(pending.PacketSyncId, "unlock");
		}
		pending.SpendReceipt?.TryRollback();
		TowerDefenseInGamePacketShow packet = pending.Packet;
		if (GodotObject.IsInstanceValid(packet) && !packet.IsQueuedForDeletion())
		{
			if (packet.HasMeta("packet_pending_plant"))
			{
				packet.RemoveMeta("packet_pending_plant");
			}
			packet.alive = pending.WasAlive;
			packet.select = false;
			packet.coldDown = pending.ColdDown;
			packet.coldDownOpen = pending.WasColdDownOpen;
			packet.coldDownTimer = pending.ColdDownTimer;
			if (packet.HasMeta("packet_planted"))
			{
				packet.RemoveMeta("packet_planted");
			}
		}
	}

	public bool IsPicking()
	{
		PruneInvalidTools();
		foreach (PacketPickTool tool in tools)
		{
			if (tool.IsPicking())
			{
				return true;
			}
		}
		return GodotObject.IsInstanceValid(packetPick);
	}

	public bool NeedsInputProcessing()
	{
		if (!IsPicking() && !_wasPicking)
		{
			return _toolActivateGrace > 0;
		}
		return true;
	}

	public AdobeAnimateSprite CreatePreviewSprite(TowerDefensePacketConfig packetConfig)
	{
		AdobeAnimateSprite packetSprite = TowerDefenseManager.GetPacketSprite(packetConfig);
		if (!GodotObject.IsInstanceValid(packetSprite))
		{
			return null;
		}
		ConfigurePreviewRenderTree(packetSprite, _placementPreviewRenderMount);
		Color modulate = packetSprite.Modulate;
		modulate.A = 0.5f;
		packetSprite.Modulate = modulate;
		packetSprite.ZIndex = ResolvePlacementPreviewZIndex(0);
		packetSprite.Position = new Vector2(-100f, -100f);
		if (packetConfig.packetFlip)
		{
			packetSprite.Scale = new Vector2(0f - packetSprite.Scale.X, packetSprite.Scale.Y);
		}
		Color meshColor = packetSprite.meshColor;
		meshColor.A = 0.5f;
		packetSprite.meshColor = meshColor;
		return packetSprite;
	}

	private static void ConfigurePreviewRenderTree(Node node, Control renderMount)
	{
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.LightMask = 0;
			adobeAnimateSprite.forceLocalRender = true;
			adobeAnimateSprite.forceCpuPoseRender = true;
			adobeAnimateSprite.SetRenderClipControl(renderMount);
		}
		for (int i = 0; i < node.GetChildCount(); i++)
		{
			ConfigurePreviewRenderTree(node.GetChild(i), renderMount);
		}
	}

	private static void ReleasePreviewRenderTree(Node node)
	{
		if (node is AdobeAnimateSprite adobeAnimateSprite)
		{
			adobeAnimateSprite.ClearRenderClipControl();
			adobeAnimateSprite.ReleaseForcedCpuPoseData();
		}
		for (int i = 0; i < node.GetChildCount(); i++)
		{
			ReleasePreviewRenderTree(node.GetChild(i));
		}
	}

	internal static int ResolvePlacementPreviewZIndex(int gridRow)
	{
		return (int)Math.Clamp((long)gridRow * 15L + 14, -4096L, 4096L);
	}

	internal static void SetPlacementPreviewRenderRow(AdobeAnimateSprite previewSprite, int gridRow)
	{
		if (GodotObject.IsInstanceValid(previewSprite))
		{
			previewSprite.ZIndex = ResolvePlacementPreviewZIndex(gridRow);
		}
	}

	private void PositionPlacementPreview(AdobeAnimateSprite previewSprite, Vector2 mapWorldPosition, int gridRow)
	{
		if (GodotObject.IsInstanceValid(previewSprite) && GodotObject.IsInstanceValid(mapControl))
		{
			SetPlacementPreviewRenderRow(previewSprite, gridRow);
			mapControl.PositionMapOverlayAtWorldPoint(previewSprite, mapWorldPosition, Vector2.Zero);
		}
	}

	public void FreePreviewSprites()
	{
		if (GodotObject.IsInstanceValid(followSprite))
		{
			ReleasePreviewRenderTree(followSprite);
			followSprite.QueueFree();
			followSprite = null;
		}
		foreach (AdobeAnimateSprite plantSprite in plantSpriteList)
		{
			if (GodotObject.IsInstanceValid(plantSprite))
			{
				ReleasePreviewRenderTree(plantSprite);
				plantSprite.QueueFree();
			}
		}
		plantSpriteList.Clear();
	}

	public void ApplyArmorSprite(AdobeAnimateSprite sprite, ArmorSlotConfig slotConfig, TowerDefenseArmorTypeData typeData)
	{
		CharacterArmorData.CreateArmorPartNode(sprite, slotConfig, typeData);
	}

	public TowerDefenseCharacter FindZombieAtPosition(Vector2 mousePos, TowerDefenseEnum.CHARACTER_CAMP targetCamp = TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
	{
		Godot.Collections.Array zombie = TowerDefenseManager.Instance.GetZombie();
		TowerDefenseCharacter result = null;
		Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
		double num = (double)Mathf.Min(mapGridSize.X, mapGridSize.Y) * (_config?.characterTargetRadiusScale ?? 0.6);
		num *= num;
		foreach (Variant item in zombie)
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && !towerDefenseCharacter.die && !towerDefenseCharacter.nearDie && !towerDefenseCharacter.instance.invincible && towerDefenseCharacter.instance.canBeCollection && towerDefenseCharacter.camp == targetCamp)
			{
				double num2 = towerDefenseCharacter.GetLogicalGlobalPosition().DistanceSquaredTo(mousePos);
				if (num2 < num)
				{
					num = num2;
					result = towerDefenseCharacter;
				}
			}
		}
		return result;
	}

	public TowerDefenseCharacter FindPlantAtPosition(Vector2 mousePos, TowerDefenseEnum.CHARACTER_CAMP targetCamp = TowerDefenseEnum.CHARACTER_CAMP.PLANT)
	{
		Godot.Collections.Array plant = TowerDefenseManager.Instance.GetPlant();
		TowerDefenseCharacter result = null;
		Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
		double num = (double)Mathf.Min(mapGridSize.X, mapGridSize.Y) * (_config?.characterTargetRadiusScale ?? 0.6);
		num *= num;
		foreach (Variant item in plant)
		{
			TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item;
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && !towerDefenseCharacter.die && !towerDefenseCharacter.nearDie && !towerDefenseCharacter.instance.invincible && towerDefenseCharacter.instance.canBeCollection && towerDefenseCharacter.camp == targetCamp)
			{
				double num2 = towerDefenseCharacter.GetLogicalGlobalPosition().DistanceSquaredTo(mousePos);
				if (num2 < num)
				{
					num = num2;
					result = towerDefenseCharacter;
				}
			}
		}
		return result;
	}

	public TowerDefenseCharacter FindPlantInCell(TowerDefenseCellInstance cell, bool hypnoses)
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return null;
		}
		cell.ClearEmpty();
		foreach (TowerDefenseCharacter character in cell.characterList)
		{
			if (GodotObject.IsInstanceValid(character) && character is TowerDefensePlant && !character.die && !character.nearDie && GodotObject.IsInstanceValid(character.instance) && !character.instance.invincible && !character.instance.hologram && character.instance.canBeCollection)
			{
				return character;
			}
		}
		return null;
	}

	private bool ProcessMultiplayerCharacterPlacement(IReadOnlyList<TowerDefenseCharacter> targets, string placementKind, bool hypnoses)
	{
		if (MultiPlayerManager.Instance == null || !GodotObject.IsInstanceValid(packetPick) || targets == null)
		{
			return false;
		}
		string overrideData = "";
		if (GodotObject.IsInstanceValid(packetPick.config._override))
		{
			overrideData = Json.Stringify(packetPick.config._override.Export());
		}
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>(targets.Count);
		foreach (TowerDefenseCharacter target in targets)
		{
			if (GodotObject.IsInstanceValid(target) && target.syncId >= 0)
			{
				list.Add(target);
			}
		}
		if (list.Count == 0)
		{
			GD.PushWarning($"Packet {packetPick.config.saveKey} cannot use {placementKind} placement without a synchronized target.");
			return false;
		}
		if (!MultiPlayerManager.Instance.isHost)
		{
			if (!TryBeginPendingPlantRequest(packetPick, list.Count, out var requestId))
			{
				return false;
			}
			if (packetPick.HasMeta("packet_sync_id"))
			{
				packetPick.SetMeta("packet_pending_plant", requestId);
			}
			foreach (TowerDefenseCharacter item in list)
			{
				MultiPlayerManager.Instance.SendPlacePlant(packetPick.config.saveKey, item.gridPos.X, item.gridPos.Y, -1, overrideData, requestId, "", placementKind, item.syncId, hypnoses);
			}
			return true;
		}
		int num = 0;
		foreach (TowerDefenseCharacter item2 in list)
		{
			int syncId = item2.syncId;
			int nextSyncId = TowerDefenseManager.Instance.currentControl.GetNextSyncId();
			bool useSun = num == 0;
			TowerDefenseCharacter towerDefenseCharacter = ((placementKind == "plant") ? packetPick.PlantOnPlant(item2, useSun, executeEvent: false) : packetPick.PlantOnZombie(item2, hypnoses, useSun, executeEvent: false));
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				TowerDefenseManager.Instance.currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
				MultiPlayerManager.Instance.SendPlacePlant(packetPick.config.saveKey, item2.gridPos.X, item2.gridPos.Y, nextSyncId, overrideData, "", "", placementKind, syncId, hypnoses);
				num++;
			}
		}
		if (num > 0)
		{
			packetPick.NotifyUseBehaviorSucceeded();
		}
		return num > 0;
	}

	public bool ProcessPacketPick(TowerDefenseCellInstance cell, Vector2I gridPos, Vector2 mousePos)
	{
		bool flag = UpdateSelectionGestureConfirmSuppression();
		int num = -1;
		if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance.currentControl) && TowerDefenseManager.Instance.currentControl.levelConfig is TowerDefenseLevelConfig towerDefenseLevelConfig)
		{
			num = towerDefenseLevelConfig.limitGridPlantNum;
		}
		if (selectPacketTimer > 0)
		{
			selectPacketTimer--;
			return true;
		}
		if (followSprite != null)
		{
			followSprite.Visible = true;
			if (GodotObject.IsInstanceValid(mapControl) && GodotObject.IsInstanceValid(mapControl.spriteNode))
			{
				mapControl.PositionMapOverlayAtWorldPoint(followSprite, mousePos, new Vector2(0f, -20f));
			}
		}
		foreach (AdobeAnimateSprite plantSprite in plantSpriteList)
		{
			plantSprite.Visible = false;
		}
		if (mapFeature.isPlantColumn)
		{
			return ProcessColumnTargetPlacement(gridPos, flag, num);
		}
		if (GodotObject.IsInstanceValid(cell) && GodotObject.IsInstanceValid(cell.FindPlantingBlocker(packetPick.config)))
		{
			if (!flag && mapControl.IsConfirmInput())
			{
				cell.NotifyBlockedPlanting(packetPick.config);
			}
			return false;
		}
		bool flag2 = false;
		bool flag3 = ShouldPreferCraterRepair(cell);
		if (packetPick.config.canPlaceOnZombie && !flag3)
		{
			bool hypnoses = packetPick.config.GetHypnoses();
			TowerDefenseEnum.CHARACTER_CAMP targetCamp = ((!hypnoses) ? TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE : TowerDefenseEnum.CHARACTER_CAMP.PLANT);
			TowerDefenseCharacter towerDefenseCharacter = FindZombieAtPosition(mousePos, targetCamp);
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				flag2 = true;
				if (plantSpriteList.Count > 0)
				{
					plantSpriteList[0].Visible = true;
					PositionPlacementPreview(plantSpriteList[0], towerDefenseCharacter.GetLogicalGlobalPosition(), towerDefenseCharacter.gridPos.Y);
				}
				if (!flag && mapControl.IsConfirmInput())
				{
					if (Global.Instance.isMultiplayerMode)
					{
						ProcessMultiplayerCharacterPlacement(new TowerDefenseCharacter[1] { towerDefenseCharacter }, "zombie", hypnoses);
					}
					else
					{
						packetPick.PlantOnZombie(towerDefenseCharacter, hypnoses);
					}
					if (!Global.Instance.isEditor || SceneManager.Instance.currentScene != "LevelEditorStage")
					{
						Release();
					}
					else
					{
						LevelEditorMapEditor.instance.levelConfig.canExport = false;
					}
					return true;
				}
			}
		}
		if (!flag2 && GodotObject.IsInstanceValid(cell))
		{
			bool flag4 = true;
			bool flag5 = packetPick.config.characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.PLANT);
			bool flag6 = false;
			foreach (TowerDefenseEnum.PLANTGRIDTYPE item in packetPick.config.characterConfig.plantGridType)
			{
				if (item != TowerDefenseEnum.PLANTGRIDTYPE.PLANT)
				{
					flag6 = true;
					break;
				}
			}
			if (!TowerDefenseManager.Instance.CheckMapGridPosIn(gridPos))
			{
				flag4 = false;
			}
			if (packetPick.config.IsLimitGridNum() && num != -1 && num <= mapFeature.GetCellPlantNum() && !cell.HasPlant())
			{
				flag4 = false;
			}
			if (packetPick.config.characterConfig is TowerDefenseZombieConfig)
			{
				flag4 = true;
			}
			if (!flag5 && !cell.CanPacketPlant(packetPick.config))
			{
				flag4 = false;
			}
			if ((packetPick.config.characterConfig is TowerDefensePlantConfig || packetPick.config.characterConfig is TowerDefenseZombieConfig) && mapFeature.stripeRow != -1)
			{
				if ((packetPick.config.characterConfig is TowerDefensePlantConfig || packetPick.config.izmPlantLeft) && mapFeature.stripeRow < gridPos.X)
				{
					flag4 = false;
				}
				if (packetPick.config.characterConfig is TowerDefenseZombieConfig && !packetPick.config.izmPlantLeft && mapFeature.stripeRow >= gridPos.X)
				{
					flag4 = false;
				}
			}
			if (TowerDefenseManager.Instance.IsIZMMode() && packetPick.config.izmPlantAllCell)
			{
				flag4 = true;
			}
			if (flag4 & flag5)
			{
				bool hypnoses2 = packetPick.config.GetHypnoses();
				if (!flag6 || cell.CanPacketPlant(packetPick.config))
				{
					TowerDefenseCharacter towerDefenseCharacter2 = FindPlantInCell(cell, hypnoses2);
					if (GodotObject.IsInstanceValid(towerDefenseCharacter2))
					{
						if (plantSpriteList.Count > 0)
						{
							plantSpriteList[0].Visible = true;
							PositionPlacementPreview(plantSpriteList[0], towerDefenseCharacter2.GetLogicalGlobalPosition(), towerDefenseCharacter2.gridPos.Y);
						}
						if (!flag && mapControl.IsConfirmInput())
						{
							if (Global.Instance.isMultiplayerMode)
							{
								ProcessMultiplayerCharacterPlacement(new TowerDefenseCharacter[1] { towerDefenseCharacter2 }, "plant", hypnoses2);
							}
							else
							{
								packetPick.PlantOnPlant(towerDefenseCharacter2);
							}
							if (!Global.Instance.isEditor || SceneManager.Instance.currentScene != "LevelEditorStage")
							{
								Release();
							}
							else
							{
								LevelEditorMapEditor.instance.levelConfig.canExport = false;
							}
							return true;
						}
					}
				}
			}
			else if (flag4)
			{
				Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(gridPos);
				double groundHeight = mapFeature.GetGroundHeight(cell);
				if (plantSpriteList.Count > 0)
				{
					plantSpriteList[0].Visible = true;
					PositionPlacementPreview(plantSpriteList[0], mapCellPlantPos - new Vector2(0f, (float)groundHeight), gridPos.Y);
				}
				if (!flag && mapControl.IsConfirmInput())
				{
					if (Global.Instance.isMultiplayerMode)
					{
						string overrideData = "";
						if (GodotObject.IsInstanceValid(packetPick.config._override))
						{
							overrideData = Json.Stringify(packetPick.config._override.Export());
						}
						if (MultiPlayerManager.Instance.isHost)
						{
							int nextSyncId = TowerDefenseManager.Instance.currentControl.GetNextSyncId();
							TowerDefenseCharacter towerDefenseCharacter3 = packetPick.Plant(gridPos);
							if (GodotObject.IsInstanceValid(towerDefenseCharacter3))
							{
								TowerDefenseManager.Instance.currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter3);
								MultiPlayerManager.Instance.SendPlacePlant(packetPick.config.saveKey, gridPos.X, gridPos.Y, nextSyncId, overrideData);
							}
						}
						else
						{
							if (!TryBeginPendingPlantRequest(packetPick, 1, out var requestId))
							{
								Release();
								return true;
							}
							if (packetPick.HasMeta("packet_sync_id"))
							{
								packetPick.SetMeta("packet_pending_plant", requestId);
							}
							MultiPlayerManager.Instance.SendPlacePlant(packetPick.config.saveKey, gridPos.X, gridPos.Y, -1, overrideData, requestId);
						}
						if (!Global.Instance.isEditor || SceneManager.Instance.currentScene != "LevelEditorStage")
						{
							Release();
						}
						else
						{
							LevelEditorMapEditor.instance.levelConfig.canExport = false;
						}
					}
					else
					{
						packetPick.Plant(gridPos);
						if (!Global.Instance.isEditor || SceneManager.Instance.currentScene != "LevelEditorStage")
						{
							Release();
						}
						else
						{
							LevelEditorMapEditor.instance.levelConfig.canExport = false;
						}
					}
				}
			}
		}
		return false;
	}

	private bool ShouldPreferCraterRepair(TowerDefenseCellInstance cell)
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(packetPick) || !GodotObject.IsInstanceValid(packetPick.config) || !GodotObject.IsInstanceValid(packetPick.config.characterConfig))
		{
			return false;
		}
		if (!packetPick.config.characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.CRATER))
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(cell.GetRepairableCrater()))
		{
			return false;
		}
		return cell.CanPacketPlant(packetPick.config);
	}

	public void ProcessTools(TowerDefenseCellInstance cell, Vector2I gridPos, Vector2 mousePos)
	{
		foreach (PacketPickTool tool in tools)
		{
			if (tool.IsPicking())
			{
				tool.ProcessPick(cell, gridPos, mousePos);
			}
		}
		bool flag = IsPicking();
		if (flag && !_wasPicking)
		{
			_toolActivateGrace = _config?.toolActivationGraceFrames ?? 5;
		}
		_wasPicking = flag;
	}

	public void PickPacket(TowerDefenseInGamePacketShow _packet)
	{
		TowerDefenseCharacterConfig characterConfig = _packet.config.characterConfig;
		selectPacketTimer = _config?.packetSelectionDebounceFrames ?? 5;
		foreach (PacketPickTool tool in tools)
		{
			tool.ToolReset();
		}
		if (_packet.select)
		{
			EnsurePreviewRenderMounts();
			if (!GodotObject.IsInstanceValid(_placementPreviewRenderMount) || !GodotObject.IsInstanceValid(_followPreviewRenderMount))
			{
				GD.PushError("PacketPick cannot create previews before both render mounts are ready.");
				_packet.Reset();
				return;
			}
			if (GodotObject.IsInstanceValid(packetPick) && packetPick != _packet)
			{
				PacketPickRelease();
			}
			packetPick = _packet;
			_suppressSelectionGestureConfirm = Input.IsActionPressed("Press");
			if (Global.Instance.isMultiplayerMode && _packet.HasMeta("packet_sync_id"))
			{
				int syncId = _packet.GetMeta("packet_sync_id").AsInt32();
				MultiPlayerManager.Instance.SendPacketPick(syncId, "lock");
			}
			Godot.Collections.Array character = TowerDefenseManager.Instance.GetCharacter();
			foreach (Variant item in character)
			{
				((TowerDefenseCharacter)(GodotObject)item).SetSpriteGroupShaderParameter("cover", false);
			}
			foreach (Variant item2 in character)
			{
				TowerDefenseCharacter towerDefenseCharacter = (TowerDefenseCharacter)(GodotObject)item2;
				bool flag = true;
				if (towerDefenseCharacter is TowerDefensePlant && GodotObject.IsInstanceValid(towerDefenseCharacter.cell) && !towerDefenseCharacter.cell.CanPacketPlant(packetPick.config))
				{
					flag = false;
				}
				if (flag && towerDefenseCharacter is TowerDefensePlant && GodotObject.IsInstanceValid(towerDefenseCharacter.cell) && characterConfig is TowerDefensePlantConfig towerDefensePlantConfig && towerDefensePlantConfig.extendCoverDictionary.Count > 0)
				{
					foreach (Vector2I key in towerDefensePlantConfig.extendCoverDictionary.Keys)
					{
						Vector2I vector2I = key;
						TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(towerDefenseCharacter.cell.gridPos + vector2I);
						if (!GodotObject.IsInstanceValid(mapCell))
						{
							continue;
						}
						foreach (TowerDefenseCharacter character2 in mapCell.GetCharacterList())
						{
							if (character2.config.name == towerDefensePlantConfig.extendCoverDictionary[key])
							{
								character2.SetSpriteGroupShaderParameter("cover", true);
							}
						}
					}
				}
				if (flag)
				{
					Array<string> plantCover = packetPick.config.GetPlantCover();
					bool flag2 = plantCover.Count > 0 && plantCover.Contains(towerDefenseCharacter.config.name);
					if (!flag2 && characterConfig.plantCoverSelf)
					{
						flag2 = characterConfig.name == towerDefenseCharacter.config.name;
					}
					if (flag2)
					{
						towerDefenseCharacter.SetSpriteGroupShaderParameter("cover", true);
					}
				}
			}
			FreePreviewSprites();
			followSprite = TowerDefenseManager.GetPacketSprite(_packet.config);
			if (GodotObject.IsInstanceValid(followSprite))
			{
				ConfigurePreviewRenderTree(followSprite, _followPreviewRenderMount);
				followSprite.ZIndex = 4096;
				followSprite.Position = new Vector2(-100f, -100f);
				if (_packet.config.packetFlip)
				{
					followSprite.Scale = new Vector2(0f - followSprite.Scale.X, followSprite.Scale.Y);
				}
				int num = ((!mapFeature.isPlantColumn) ? 1 : mapFeature.config.gridNum.Y);
				for (int i = 0; i < num; i++)
				{
					AdobeAnimateSprite adobeAnimateSprite = CreatePreviewSprite(_packet.config);
					if (GodotObject.IsInstanceValid(adobeAnimateSprite))
					{
						plantSpriteList.Add(adobeAnimateSprite);
					}
				}
				if (characterConfig.armorData != null && _packet.config.initArmor.Count > 0)
				{
					foreach (string item3 in _packet.config.initArmor)
					{
						ArmorSlotConfig slotConfig = characterConfig.armorData.GetSlotConfig(item3);
						TowerDefenseArmorTypeData typeData = characterConfig.armorData.GetTypeData(item3);
						if (typeData == null)
						{
							continue;
						}
						string replaceMethod = slotConfig.replaceMethod;
						if (!(replaceMethod == "Media"))
						{
							if (!(replaceMethod == "Sprite"))
							{
								continue;
							}
							ApplyArmorSprite(followSprite, slotConfig, typeData);
							foreach (AdobeAnimateSprite plantSprite in plantSpriteList)
							{
								ApplyArmorSprite(plantSprite, slotConfig, typeData);
							}
							continue;
						}
						characterConfig.armorData.OpenArmorFliters(followSprite, item3);
						characterConfig.armorData.SetArmorReplace(followSprite, item3, 0);
						foreach (AdobeAnimateSprite plantSprite2 in plantSpriteList)
						{
							characterConfig.armorData.OpenArmorFliters(plantSprite2, item3);
							characterConfig.armorData.SetArmorReplace(plantSprite2, item3, 0);
						}
					}
				}
				if (characterConfig.customData != null)
				{
					Dictionary dictionary = GameSaveManager.Instance.GetTowerDefensePacketValue(_packet.config.saveKey).GetValueOrDefault("Key", new Dictionary()).AsGodotDictionary();
					if (dictionary.GetValueOrDefault("Custom", "").AsString() != "")
					{
						characterConfig.customData.SetCustomFliters(followSprite, dictionary["Custom"].AsString());
						foreach (AdobeAnimateSprite plantSprite3 in plantSpriteList)
						{
							characterConfig.customData.SetCustomFliters(plantSprite3, dictionary["Custom"].AsString());
						}
					}
				}
				ConfigurePreviewRenderTree(followSprite, _followPreviewRenderMount);
				foreach (AdobeAnimateSprite plantSprite4 in plantSpriteList)
				{
					ConfigurePreviewRenderTree(plantSprite4, _placementPreviewRenderMount);
				}
				mapControl.spriteNode.AddChild(followSprite, forceReadableName: false, InternalMode.Disabled);
				{
					foreach (AdobeAnimateSprite plantSprite5 in plantSpriteList)
					{
						mapControl.spriteNode.AddChild(plantSprite5, forceReadableName: false, InternalMode.Disabled);
					}
					return;
				}
			}
			GD.PushWarning("Packet " + _packet.config.saveKey + " has no preview sprite and cannot be picked.");
			PacketPickRelease();
		}
		else if (_packet.canPressPutBack)
		{
			Release();
		}
	}

	private bool UpdateSelectionGestureConfirmSuppression()
	{
		if (!_suppressSelectionGestureConfirm)
		{
			return false;
		}
		bool flag = Input.IsActionPressed("Press");
		bool flag2 = Input.IsActionJustReleased("Press");
		if (!flag)
		{
			_suppressSelectionGestureConfirm = false;
		}
		return flag | flag2;
	}

	public void Release()
	{
		foreach (PacketPickTool tool in tools)
		{
			tool.ToolRelease();
		}
		PacketPickRelease();
	}

	public void ProcessReleaseInput(Vector2 mousePos)
	{
		if (_toolActivateGrace > 0)
		{
			_toolActivateGrace--;
		}
		else if (!Global.Instance.isEditor || SceneManager.Instance.currentScene != "LevelEditorStage")
		{
			if (Global.Instance.isMobile)
			{
				if (Input.IsActionJustPressed("Press") && IsPicking() && GodotObject.IsInstanceValid(mapControl) && !IsPlacementSurfacePoint(mousePos))
				{
					Release();
				}
				if (!Input.IsActionJustReleased("Press") || IsPicking())
				{
					return;
				}
				foreach (AdobeAnimateSprite plantSprite in plantSpriteList)
				{
					if (GodotObject.IsInstanceValid(plantSprite))
					{
						plantSprite.Visible = false;
					}
				}
				if (GodotObject.IsInstanceValid(followSprite))
				{
					followSprite.Visible = false;
				}
				{
					foreach (PacketPickTool tool in tools)
					{
						Node2D mapSprite = tool.GetMapSprite();
						if (GodotObject.IsInstanceValid(mapSprite))
						{
							mapSprite.Visible = false;
						}
					}
					return;
				}
			}
			if (Input.IsActionJustPressed("Release"))
			{
				Release();
			}
		}
		else if (Input.IsActionJustPressed("Release"))
		{
			Release();
		}
	}

	private bool IsPlacementSurfacePoint(Vector2 mousePos)
	{
		if (!GodotObject.IsInstanceValid(mapFeature))
		{
			return false;
		}
		if (mapFeature.groundRect.HasPoint(mousePos))
		{
			return true;
		}
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return false;
		}
		Vector2I mapGridPosFromMouse = instance.GetMapGridPosFromMouse(mousePos);
		if (instance.CheckMapGridPosIn(mapGridPosFromMouse))
		{
			return GodotObject.IsInstanceValid(TowerDefenseManager.GetMapCell(mapGridPosFromMouse));
		}
		return false;
	}

	public void PacketPickRelease()
	{
		if (GodotObject.IsInstanceValid(packetPick))
		{
			if (Global.Instance.isMultiplayerMode && packetPick.HasMeta("packet_sync_id"))
			{
				int syncId = packetPick.GetMeta("packet_sync_id").AsInt32();
				if (!packetPick.HasMeta("packet_pending_plant"))
				{
					if (packetPick.HasMeta("packet_planted"))
					{
						MultiPlayerManager.Instance.SendPacketPick(syncId);
					}
					else
					{
						MultiPlayerManager.Instance.SendPacketPick(syncId, "unlock");
					}
				}
			}
			foreach (Variant item in TowerDefenseManager.Instance.GetCharacter())
			{
				((TowerDefenseCharacter)(GodotObject)item).SetSpriteGroupShaderParameter("cover", false);
			}
			packetPick.Reset();
			packetPick = null;
		}
		_suppressSelectionGestureConfirm = false;
		FreePreviewSprites();
	}

	public void DisposeBattleState()
	{
		if (_multiplayerEventsSubscribed && MultiPlayerManager.Instance != null)
		{
			MultiPlayerManager.Instance.OnMatchStateReceived -= OnMultiplayerMatchStateReceived;
		}
		_multiplayerEventsSubscribed = false;
		RollbackPendingPlantRequests();
		foreach (PacketPickTool tool in tools)
		{
			if (GodotObject.IsInstanceValid(tool))
			{
				tool.ToolReset();
			}
		}
		tools.Clear();
		if (GodotObject.IsInstanceValid(packetPick))
		{
			packetPick.Reset();
		}
		packetPick = null;
		FreePreviewSprites();
		if (GodotObject.IsInstanceValid(_placementPreviewRenderMount))
		{
			_placementPreviewRenderMount.QueueFree();
			_placementPreviewRenderMount = null;
		}
		if (GodotObject.IsInstanceValid(_followPreviewRenderMount))
		{
			_followPreviewRenderMount.QueueFree();
			_followPreviewRenderMount = null;
		}
		mapControl = null;
		mapFeature = null;
		_config = null;
		_wasPicking = false;
		_toolActivateGrace = 0;
		_suppressSelectionGestureConfirm = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(44)
		{
			new MethodInfo(MethodName.FindColumnJalaVase, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanPlaceColumnTargetInCell, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindColumnZombie, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "zombies", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "targetCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessColumnTargetPlacement, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressPlacementConfirm", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "limitPlantGridNum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RollbackPendingPlantRequests, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_mapControl", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "_mapFeature", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsurePreviewRenderMounts, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreatePreviewRenderMount, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterTool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tool", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.UnregisterTool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tool", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.PruneInvalidTools, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SubscribeMultiplayerEvents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPendingRequestTimeoutMsec, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePendingRequestProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SweepExpiredPlantRequests, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nowMsec", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnMultiplayerMatchStateReceived, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "opCode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "senderId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HandlePendingPlantAccepted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "requestId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HandlePendingPlantRejected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "requestId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPicking, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NeedsInputProcessing, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreatePreviewSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigurePreviewRenderTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "renderMount", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReleasePreviewRenderTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolvePlacementPreviewZIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "gridRow", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPlacementPreviewRenderRow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "previewSprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "gridRow", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PositionPlacementPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "previewSprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mapWorldPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gridRow", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FreePreviewSprites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyArmorSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "slotConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "typeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindZombieAtPosition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "targetCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindPlantAtPosition, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "targetCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindPlantInCell, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "hypnoses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessPacketPick, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldPreferCraterRepair, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessTools, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PickPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateSelectionGestureConfirmSuppression, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Release, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessReleaseInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPlacementSurfacePoint, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "mousePos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PacketPickRelease, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeBattleState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FindColumnJalaVase && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlantJalaVase>(FindColumnJalaVase(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.CanPlaceColumnTargetInCell && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanPlaceColumnTargetInCell(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.FindColumnZombie && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindColumnZombie(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[2])));
			return true;
		}
		if (method == MethodName.ProcessColumnTargetPlacement && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ProcessColumnTargetPlacement(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
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
		if (method == MethodName.RollbackPendingPlantRequests && args.Count == 0)
		{
			RollbackPendingPlantRequests();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 3)
		{
			Init(VariantUtils.ConvertTo<TowerDefenseMapControl>(in args[0]), VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in args[1]), VariantUtils.ConvertTo<TowerDefenseBattleFeaturePacketPickConfig>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsurePreviewRenderMounts && args.Count == 0)
		{
			EnsurePreviewRenderMounts();
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePreviewRenderMount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreatePreviewRenderMount(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterTool && args.Count == 1)
		{
			RegisterTool(VariantUtils.ConvertTo<PacketPickTool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnregisterTool && args.Count == 1)
		{
			UnregisterTool(VariantUtils.ConvertTo<PacketPickTool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PruneInvalidTools && args.Count == 0)
		{
			PruneInvalidTools();
			ret = default;
			return true;
		}
		if (method == MethodName.SubscribeMultiplayerEvents && args.Count == 0)
		{
			SubscribeMultiplayerEvents();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPendingRequestTimeoutMsec && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetPendingRequestTimeoutMsec());
			return true;
		}
		if (method == MethodName.UpdatePendingRequestProcessing && args.Count == 0)
		{
			UpdatePendingRequestProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.SweepExpiredPlantRequests && args.Count == 1)
		{
			SweepExpiredPlantRequests(VariantUtils.ConvertTo<ulong>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnMultiplayerMatchStateReceived && args.Count == 3)
		{
			OnMultiplayerMatchStateReceived(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandlePendingPlantAccepted && args.Count == 1)
		{
			HandlePendingPlantAccepted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandlePendingPlantRejected && args.Count == 1)
		{
			HandlePendingPlantRejected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsPicking && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPicking());
			return true;
		}
		if (method == MethodName.NeedsInputProcessing && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(NeedsInputProcessing());
			return true;
		}
		if (method == MethodName.CreatePreviewSprite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(CreatePreviewSprite(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ConfigurePreviewRenderTree && args.Count == 2)
		{
			ConfigurePreviewRenderTree(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleasePreviewRenderTree && args.Count == 1)
		{
			ReleasePreviewRenderTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePlacementPreviewZIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolvePlacementPreviewZIndex(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetPlacementPreviewRenderRow && args.Count == 2)
		{
			SetPlacementPreviewRenderRow(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PositionPlacementPreview && args.Count == 3)
		{
			PositionPlacementPreview(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FreePreviewSprites && args.Count == 0)
		{
			FreePreviewSprites();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyArmorSprite && args.Count == 3)
		{
			ApplyArmorSprite(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<ArmorSlotConfig>(in args[1]), VariantUtils.ConvertTo<TowerDefenseArmorTypeData>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindZombieAtPosition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindZombieAtPosition(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.FindPlantAtPosition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindPlantAtPosition(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.FindPlantInCell && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindPlantInCell(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ProcessPacketPick && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ProcessPacketPick(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.ShouldPreferCraterRepair && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldPreferCraterRepair(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.ProcessTools && args.Count == 3)
		{
			ProcessTools(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.PickPacket && args.Count == 1)
		{
			PickPacket(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSelectionGestureConfirmSuppression && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(UpdateSelectionGestureConfirmSuppression());
			return true;
		}
		if (method == MethodName.Release && args.Count == 0)
		{
			Release();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessReleaseInput && args.Count == 1)
		{
			ProcessReleaseInput(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsPlacementSurfacePoint && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPlacementSurfacePoint(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.PacketPickRelease && args.Count == 0)
		{
			PacketPickRelease();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeBattleState && args.Count == 0)
		{
			DisposeBattleState();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FindColumnJalaVase && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlantJalaVase>(FindColumnJalaVase(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.FindColumnZombie && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindColumnZombie(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[2])));
			return true;
		}
		if (method == MethodName.CreatePreviewRenderMount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreatePreviewRenderMount(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ConfigurePreviewRenderTree && args.Count == 2)
		{
			ConfigurePreviewRenderTree(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleasePreviewRenderTree && args.Count == 1)
		{
			ReleasePreviewRenderTree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePlacementPreviewZIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolvePlacementPreviewZIndex(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetPlacementPreviewRenderRow && args.Count == 2)
		{
			SetPlacementPreviewRenderRow(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.FindColumnJalaVase)
		{
			return true;
		}
		if (method == MethodName.CanPlaceColumnTargetInCell)
		{
			return true;
		}
		if (method == MethodName.FindColumnZombie)
		{
			return true;
		}
		if (method == MethodName.ProcessColumnTargetPlacement)
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
		if (method == MethodName.RollbackPendingPlantRequests)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.EnsurePreviewRenderMounts)
		{
			return true;
		}
		if (method == MethodName.CreatePreviewRenderMount)
		{
			return true;
		}
		if (method == MethodName.RegisterTool)
		{
			return true;
		}
		if (method == MethodName.UnregisterTool)
		{
			return true;
		}
		if (method == MethodName.PruneInvalidTools)
		{
			return true;
		}
		if (method == MethodName.SubscribeMultiplayerEvents)
		{
			return true;
		}
		if (method == MethodName.GetPendingRequestTimeoutMsec)
		{
			return true;
		}
		if (method == MethodName.UpdatePendingRequestProcessing)
		{
			return true;
		}
		if (method == MethodName.SweepExpiredPlantRequests)
		{
			return true;
		}
		if (method == MethodName.OnMultiplayerMatchStateReceived)
		{
			return true;
		}
		if (method == MethodName.HandlePendingPlantAccepted)
		{
			return true;
		}
		if (method == MethodName.HandlePendingPlantRejected)
		{
			return true;
		}
		if (method == MethodName.IsPicking)
		{
			return true;
		}
		if (method == MethodName.NeedsInputProcessing)
		{
			return true;
		}
		if (method == MethodName.CreatePreviewSprite)
		{
			return true;
		}
		if (method == MethodName.ConfigurePreviewRenderTree)
		{
			return true;
		}
		if (method == MethodName.ReleasePreviewRenderTree)
		{
			return true;
		}
		if (method == MethodName.ResolvePlacementPreviewZIndex)
		{
			return true;
		}
		if (method == MethodName.SetPlacementPreviewRenderRow)
		{
			return true;
		}
		if (method == MethodName.PositionPlacementPreview)
		{
			return true;
		}
		if (method == MethodName.FreePreviewSprites)
		{
			return true;
		}
		if (method == MethodName.ApplyArmorSprite)
		{
			return true;
		}
		if (method == MethodName.FindZombieAtPosition)
		{
			return true;
		}
		if (method == MethodName.FindPlantAtPosition)
		{
			return true;
		}
		if (method == MethodName.FindPlantInCell)
		{
			return true;
		}
		if (method == MethodName.ProcessPacketPick)
		{
			return true;
		}
		if (method == MethodName.ShouldPreferCraterRepair)
		{
			return true;
		}
		if (method == MethodName.ProcessTools)
		{
			return true;
		}
		if (method == MethodName.PickPacket)
		{
			return true;
		}
		if (method == MethodName.UpdateSelectionGestureConfirmSuppression)
		{
			return true;
		}
		if (method == MethodName.Release)
		{
			return true;
		}
		if (method == MethodName.ProcessReleaseInput)
		{
			return true;
		}
		if (method == MethodName.IsPlacementSurfacePoint)
		{
			return true;
		}
		if (method == MethodName.PacketPickRelease)
		{
			return true;
		}
		if (method == MethodName.DisposeBattleState)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.mapControl)
		{
			mapControl = VariantUtils.ConvertTo<TowerDefenseMapControl>(in value);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			mapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName._config)
		{
			_config = VariantUtils.ConvertTo<TowerDefenseBattleFeaturePacketPickConfig>(in value);
			return true;
		}
		if (name == PropertyName.packetPick)
		{
			packetPick = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName.followSprite)
		{
			followSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.selectPacketTimer)
		{
			selectPacketTimer = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._wasPicking)
		{
			_wasPicking = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._toolActivateGrace)
		{
			_toolActivateGrace = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._suppressSelectionGestureConfirm)
		{
			_suppressSelectionGestureConfirm = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._plantRequestSerial)
		{
			_plantRequestSerial = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._multiplayerEventsSubscribed)
		{
			_multiplayerEventsSubscribed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingRequestSweepTimer)
		{
			_pendingRequestSweepTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._placementPreviewRenderMount)
		{
			_placementPreviewRenderMount = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._followPreviewRenderMount)
		{
			_followPreviewRenderMount = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.mapControl)
		{
			value = VariantUtils.CreateFrom(in mapControl);
			return true;
		}
		if (name == PropertyName.mapFeature)
		{
			value = VariantUtils.CreateFrom(in mapFeature);
			return true;
		}
		if (name == PropertyName._config)
		{
			value = VariantUtils.CreateFrom(in _config);
			return true;
		}
		if (name == PropertyName.packetPick)
		{
			value = VariantUtils.CreateFrom(in packetPick);
			return true;
		}
		if (name == PropertyName.followSprite)
		{
			value = VariantUtils.CreateFrom(in followSprite);
			return true;
		}
		if (name == PropertyName.selectPacketTimer)
		{
			value = VariantUtils.CreateFrom(in selectPacketTimer);
			return true;
		}
		if (name == PropertyName._wasPicking)
		{
			value = VariantUtils.CreateFrom(in _wasPicking);
			return true;
		}
		if (name == PropertyName._toolActivateGrace)
		{
			value = VariantUtils.CreateFrom(in _toolActivateGrace);
			return true;
		}
		if (name == PropertyName._suppressSelectionGestureConfirm)
		{
			value = VariantUtils.CreateFrom(in _suppressSelectionGestureConfirm);
			return true;
		}
		if (name == PropertyName._plantRequestSerial)
		{
			value = VariantUtils.CreateFrom(in _plantRequestSerial);
			return true;
		}
		if (name == PropertyName._multiplayerEventsSubscribed)
		{
			value = VariantUtils.CreateFrom(in _multiplayerEventsSubscribed);
			return true;
		}
		if (name == PropertyName._pendingRequestSweepTimer)
		{
			value = VariantUtils.CreateFrom(in _pendingRequestSweepTimer);
			return true;
		}
		if (name == PropertyName._placementPreviewRenderMount)
		{
			value = VariantUtils.CreateFrom(in _placementPreviewRenderMount);
			return true;
		}
		if (name == PropertyName._followPreviewRenderMount)
		{
			value = VariantUtils.CreateFrom(in _followPreviewRenderMount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.mapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.packetPick, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.followSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.selectPacketTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._wasPicking, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._toolActivateGrace, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._suppressSelectionGestureConfirm, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._plantRequestSerial, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._multiplayerEventsSubscribed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingRequestSweepTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._placementPreviewRenderMount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._followPreviewRenderMount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.mapControl, Variant.From(in mapControl));
		info.AddProperty(PropertyName.mapFeature, Variant.From(in mapFeature));
		info.AddProperty(PropertyName._config, Variant.From(in _config));
		info.AddProperty(PropertyName.packetPick, Variant.From(in packetPick));
		info.AddProperty(PropertyName.followSprite, Variant.From(in followSprite));
		info.AddProperty(PropertyName.selectPacketTimer, Variant.From(in selectPacketTimer));
		info.AddProperty(PropertyName._wasPicking, Variant.From(in _wasPicking));
		info.AddProperty(PropertyName._toolActivateGrace, Variant.From(in _toolActivateGrace));
		info.AddProperty(PropertyName._suppressSelectionGestureConfirm, Variant.From(in _suppressSelectionGestureConfirm));
		info.AddProperty(PropertyName._plantRequestSerial, Variant.From(in _plantRequestSerial));
		info.AddProperty(PropertyName._multiplayerEventsSubscribed, Variant.From(in _multiplayerEventsSubscribed));
		info.AddProperty(PropertyName._pendingRequestSweepTimer, Variant.From(in _pendingRequestSweepTimer));
		info.AddProperty(PropertyName._placementPreviewRenderMount, Variant.From(in _placementPreviewRenderMount));
		info.AddProperty(PropertyName._followPreviewRenderMount, Variant.From(in _followPreviewRenderMount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.mapControl, out var value))
		{
			mapControl = value.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName.mapFeature, out var value2))
		{
			mapFeature = value2.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName._config, out var value3))
		{
			_config = value3.As<TowerDefenseBattleFeaturePacketPickConfig>();
		}
		if (info.TryGetProperty(PropertyName.packetPick, out var value4))
		{
			packetPick = value4.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName.followSprite, out var value5))
		{
			followSprite = value5.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.selectPacketTimer, out var value6))
		{
			selectPacketTimer = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._wasPicking, out var value7))
		{
			_wasPicking = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._toolActivateGrace, out var value8))
		{
			_toolActivateGrace = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._suppressSelectionGestureConfirm, out var value9))
		{
			_suppressSelectionGestureConfirm = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._plantRequestSerial, out var value10))
		{
			_plantRequestSerial = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName._multiplayerEventsSubscribed, out var value11))
		{
			_multiplayerEventsSubscribed = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingRequestSweepTimer, out var value12))
		{
			_pendingRequestSweepTimer = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName._placementPreviewRenderMount, out var value13))
		{
			_placementPreviewRenderMount = value13.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._followPreviewRenderMount, out var value14))
		{
			_followPreviewRenderMount = value14.As<Control>();
		}
	}
}
