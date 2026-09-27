using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter4/Discopaper/Scene/TowerDefenseZombieDiscopaper.cs")]
public class TowerDefenseZombieDiscopaper : TowerDefenseZombie, IJackson, INetworkDancerOwner
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName PointEntered = "PointEntered";

		public static readonly StringName PointProcessing = "PointProcessing";

		public static readonly StringName PointExited = "PointExited";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public static readonly StringName GaspEntered = "GaspEntered";

		public static readonly StringName GaspProcessing = "GaspProcessing";

		public static readonly StringName GaspExited = "GaspExited";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName CanWalk = "CanWalk";

		public static readonly StringName CanDancerMoveWithJackson = "CanDancerMoveWithJackson";

		public static readonly StringName CanSpawnDancer = "CanSpawnDancer";

		public static readonly StringName SpawnDancer = "SpawnDancer";

		public static readonly StringName SpawnSingleDancer = "SpawnSingleDancer";

		public static readonly StringName CreateDancerSpawnState = "CreateDancerSpawnState";

		public static readonly StringName ChangeSpotlightColor = "ChangeSpotlightColor";

		public static readonly StringName RemoveDancer = "RemoveDancer";

		public static readonly StringName SetNetworkDancer = "SetNetworkDancer";

		public static readonly StringName RefreshPendingDancerBatchEligibility = "RefreshPendingDancerBatchEligibility";

		public static readonly StringName ResolvePendingDancerRelations = "ResolvePendingDancerRelations";

		public static readonly StringName ReleaseDancerOwnership = "ReleaseDancerOwnership";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName angry = "angry";

		public static readonly StringName _spotlight2 = "_spotlight2";

		public static readonly StringName _spotlight = "_spotlight";

		public static readonly StringName spotlightGrandient = "spotlightGrandient";

		public static readonly StringName walkTime = "walkTime";

		public static readonly StringName _angry = "_angry";

		public static readonly StringName dancerList = "dancerList";

		public static readonly StringName _pendingDancerSyncIds = "_pendingDancerSyncIds";

		public static readonly StringName _pendingDancerNodeNames = "_pendingDancerNodeNames";

		public static readonly StringName _pendingDancerResolveRemaining = "_pendingDancerResolveRemaining";

		public static readonly StringName _hasPendingDancerRelations = "_hasPendingDancerRelations";

		public static readonly StringName dancerPacketName = "dancerPacketName";

		public static readonly StringName firstSpawn = "firstSpawn";

		public static readonly StringName isPointing = "isPointing";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const double PendingDancerResolveIntervalSeconds = 0.25;

	private const string ZOMBIE_PAPER_MADHEAD = "uid://chsy8qvq8nmd";

	private Sprite2D _spotlight2;

	private Sprite2D _spotlight;

	[Export(PropertyHint.None, "")]
	public Gradient spotlightGrandient;

	public int walkTime = 4;

	private bool _angry;

	public Array<TowerDefenseCharacter> dancerList = new Array<TowerDefenseCharacter>();

	private readonly int[] _pendingDancerSyncIds = new int[4] { -1, -1, -1, -1 };

	private readonly string[] _pendingDancerNodeNames = new string[4] { "", "", "", "" };

	private double _pendingDancerResolveRemaining;

	private bool _hasPendingDancerRelations;

	[Export(PropertyHint.None, "")]
	public string dancerPacketName = "ZombieBackuppaper";

	public bool firstSpawn;

	public bool isPointing;

	private StateHandle _gaspStateHandle;

	private StateHandle _pointStateHandle;

	private bool _roleStateSignalsConnected;

	public bool angry
	{
		get
		{
			return _angry;
		}
		set
		{
			_angry = value;
			if (_angry)
			{
				walkAnimeClip = "AngryWalk";
				swimAnimeClip = "AngryWalk";
				sprite.SetAtlasReplace("Zombie_dancer__head.png", "uid://chsy8qvq8nmd");
			}
			else
			{
				walkAnimeClip = "Walk";
				swimAnimeClip = "Walk";
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
		_gaspStateHandle = StateMachine?.GetStateById("zombie.discopaper.gasp");
		_pointStateHandle = StateMachine?.GetStateById("zombie.discopaper.point");
		StateHandle gaspStateHandle = _gaspStateHandle;
		if (gaspStateHandle != null && gaspStateHandle.IsValid)
		{
			StateHandle pointStateHandle = _pointStateHandle;
			if (pointStateHandle != null && pointStateHandle.IsValid)
			{
				_gaspStateHandle.Entered += GaspEntered;
				_gaspStateHandle.Exited += GaspExited;
				_gaspStateHandle.PhysicsProcessing += GaspProcessing;
				_pointStateHandle.Entered += PointEntered;
				_pointStateHandle.Exited += PointExited;
				_pointStateHandle.PhysicsProcessing += PointProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			if (_gaspStateHandle != null)
			{
				_gaspStateHandle.Entered -= GaspEntered;
				_gaspStateHandle.Exited -= GaspExited;
				_gaspStateHandle.PhysicsProcessing -= GaspProcessing;
			}
			_gaspStateHandle = null;
			if (_pointStateHandle != null)
			{
				_pointStateHandle.Entered -= PointEntered;
				_pointStateHandle.Exited -= PointExited;
				_pointStateHandle.PhysicsProcessing -= PointProcessing;
			}
			_pointStateHandle = null;
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
			dancerList.Resize(4);
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
		if (_hasPendingDancerRelations)
		{
			_pendingDancerResolveRemaining -= delta;
			if (!(_pendingDancerResolveRemaining > 0.0))
			{
				ResolvePendingDancerRelations();
			}
		}
	}

	public void PointEntered()
	{
		isPointing = true;
		sprite.SetAnimation("PointUp", loop: false, 0.2);
		sprite.AddAnimation("PointDown", 0.75, loop: false, 0.2);
	}

	public void PointProcessing(double delta)
	{
		sprite.timeScale = timeScale * 0.75;
	}

	public void PointExited()
	{
		isPointing = false;
	}

	public override void WalkEntered()
	{
		base.WalkEntered();
		sprite.Scale = new Vector2(0.8f, sprite.Scale.Y);
		foreach (TowerDefenseCharacter dancer in dancerList)
		{
			if (GodotObject.IsInstanceValid(dancer) && dancer is TowerDefenseZombie towerDefenseZombie)
			{
				towerDefenseZombie.timeScale = timeScale;
				towerDefenseZombie.walkSpeedScale = walkSpeedScale;
			}
		}
		walkTime = 4;
	}

	public override void WalkProcessing(double delta)
	{
		groundMoveComponent.SetAlive(CanWalk());
		base.WalkProcessing(delta);
	}

	public override void Walk()
	{
		SendStateEvent("ToWalk");
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		groundMoveComponent.SetAlive(false);
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public void GaspEntered()
	{
		sprite.SetAnimation("Gasp", loop: false, 0.1);
	}

	public void GaspProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public void GaspExited()
	{
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		switch (clip)
		{
		case "PointDown":
			isPointing = false;
			foreach (TowerDefenseCharacter dancer in dancerList)
			{
				if (GodotObject.IsInstanceValid(dancer) && !dancer.die && !dancer.nearDie)
				{
					((TowerDefenseZombie)dancer).Walk();
				}
			}
			Walk();
			break;
		case "AngryWalk":
			if (angry)
			{
				walkTime--;
			}
			if (!die && !nearDie)
			{
				if (walkTime <= 0 && CanSpawnDancer())
				{
					SendStateEvent("ToPoint");
				}
			}
			else
			{
				Die();
			}
			break;
		case "Gasp":
			AudioManager.Instance.AudioPlay("NewspaperRarrgh");
			sprite.SetAtlasReplace("Zombie_head.png", "uid://chsy8qvq8nmd");
			timeScaleInit = 2.0;
			angry = true;
			SendStateEvent("ToPoint");
			break;
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == "spawn" && !die && !nearDie)
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
	}

	public bool CanWalk()
	{
		foreach (TowerDefenseCharacter dancer in dancerList)
		{
			if (!CanDancerMoveWithJackson(dancer))
			{
				return false;
			}
		}
		return true;
	}

	private static bool CanDancerMoveWithJackson(TowerDefenseCharacter dancer)
	{
		if (!GodotObject.IsInstanceValid(dancer))
		{
			return true;
		}
		if (dancer.die || dancer.nearDie || dancer.isRise)
		{
			return true;
		}
		if (!GodotObject.IsInstanceValid(dancer.sprite))
		{
			return true;
		}
		return dancer.sprite.clip == "Walk";
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
		if (!IsInsideComponentBattlefield || isShow || !TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(dancerPacketName);
		if (GodotObject.IsInstanceValid(packetConfig) && GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(transformPoint))
		{
			Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
			Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			if (gridPos.Y > 1 && !GodotObject.IsInstanceValid(dancerList[0]))
			{
				SpawnSingleDancer(packetConfig, 0, new Vector2(logicalGlobalPosition.X, (float)TowerDefenseManager.GetMapLineY(gridPos.Y - 1)), gridPos - new Vector2I(0, 1));
			}
			if (gridPos.Y < mapGridNum.Y && !GodotObject.IsInstanceValid(dancerList[1]))
			{
				SpawnSingleDancer(packetConfig, 1, new Vector2(logicalGlobalPosition.X, (float)TowerDefenseManager.GetMapLineY(gridPos.Y + 1)), gridPos + new Vector2I(0, 1));
			}
			if (!GodotObject.IsInstanceValid(dancerList[2]))
			{
				SpawnSingleDancer(packetConfig, 2, logicalGlobalPosition - new Vector2(mapGridSize.X * 1.25f, 0f), gridPos - new Vector2I(1, 0));
			}
			if (!GodotObject.IsInstanceValid(dancerList[3]))
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
		TowerDefenseManager.PublishSpawnedCharacter(dancerPacketName, dancer, useCreate: true, 1.5, walkAfterSpawn: false, "", CreateDancerSpawnState(slot));
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(dancer))
			{
				dancer.Rise(1.5);
			}
		}).CallDeferred();
	}

	private Dictionary CreateDancerSpawnState(int slot)
	{
		return new Dictionary
		{
			["spawn_invisible"] = invisible,
			["dancer_parent_sync_id"] = syncId,
			["dancer_slot"] = slot
		};
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

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Paper")
		{
			SendStateEvent("ToGasp");
			AudioManager.Instance.AudioPlay("NewspaperRip");
		}
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
			{ "walkTime", walkTime },
			{ "angry", angry },
			{ "firstSpawn", firstSpawn },
			{ "isPointing", isPointing },
			{ "dancerNodeNames", array }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		walkTime = data.GetValueOrDefault("walkTime", 4).AsInt32();
		angry = data.GetValueOrDefault("angry", false).AsBool();
		firstSpawn = data.GetValueOrDefault("firstSpawn", false).AsBool();
		isPointing = data.GetValueOrDefault("isPointing", false).AsBool();
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(38)
		{
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PointEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PointProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PointExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GaspEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GaspProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GaspExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CanWalk, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanDancerMoveWithJackson, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dancer", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanSpawnDancer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.PointEntered && args.Count == 0)
		{
			PointEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.PointProcessing && args.Count == 1)
		{
			PointProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PointExited && args.Count == 0)
		{
			PointExited();
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
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GaspEntered && args.Count == 0)
		{
			GaspEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.GaspProcessing && args.Count == 1)
		{
			GaspProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GaspExited && args.Count == 0)
		{
			GaspExited();
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
		if (method == MethodName.CanWalk && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanWalk());
			return true;
		}
		if (method == MethodName.CanDancerMoveWithJackson && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanDancerMoveWithJackson(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
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
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CanDancerMoveWithJackson && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanDancerMoveWithJackson(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		ret = default;
		return false;
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
		if (method == MethodName.PointEntered)
		{
			return true;
		}
		if (method == MethodName.PointProcessing)
		{
			return true;
		}
		if (method == MethodName.PointExited)
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
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.GaspEntered)
		{
			return true;
		}
		if (method == MethodName.GaspProcessing)
		{
			return true;
		}
		if (method == MethodName.GaspExited)
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
		if (method == MethodName.CanWalk)
		{
			return true;
		}
		if (method == MethodName.CanDancerMoveWithJackson)
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
		if (method == MethodName.ArmorHitpointsEmpty)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.angry)
		{
			angry = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.walkTime)
		{
			walkTime = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._angry)
		{
			_angry = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.isPointing)
		{
			isPointing = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.angry)
		{
			value = VariantUtils.CreateFrom<bool>(angry);
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
		if (name == PropertyName.walkTime)
		{
			value = VariantUtils.CreateFrom(in walkTime);
			return true;
		}
		if (name == PropertyName._angry)
		{
			value = VariantUtils.CreateFrom(in _angry);
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
		if (name == PropertyName.isPointing)
		{
			value = VariantUtils.CreateFrom(in isPointing);
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
			new PropertyInfo(Variant.Type.Int, PropertyName.walkTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.angry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._angry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.dancerList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._pendingDancerSyncIds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._pendingDancerNodeNames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingDancerResolveRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPendingDancerRelations, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.dancerPacketName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.firstSpawn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isPointing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.angry, Variant.From<bool>(angry));
		info.AddProperty(PropertyName._spotlight2, Variant.From(in _spotlight2));
		info.AddProperty(PropertyName._spotlight, Variant.From(in _spotlight));
		info.AddProperty(PropertyName.spotlightGrandient, Variant.From(in spotlightGrandient));
		info.AddProperty(PropertyName.walkTime, Variant.From(in walkTime));
		info.AddProperty(PropertyName._angry, Variant.From(in _angry));
		info.AddProperty(PropertyName.dancerList, Variant.CreateFrom(dancerList));
		info.AddProperty(PropertyName._pendingDancerResolveRemaining, Variant.From(in _pendingDancerResolveRemaining));
		info.AddProperty(PropertyName._hasPendingDancerRelations, Variant.From(in _hasPendingDancerRelations));
		info.AddProperty(PropertyName.dancerPacketName, Variant.From(in dancerPacketName));
		info.AddProperty(PropertyName.firstSpawn, Variant.From(in firstSpawn));
		info.AddProperty(PropertyName.isPointing, Variant.From(in isPointing));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.angry, out var value))
		{
			angry = value.As<bool>();
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
		if (info.TryGetProperty(PropertyName.walkTime, out var value5))
		{
			walkTime = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._angry, out var value6))
		{
			_angry = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dancerList, out var value7))
		{
			dancerList = value7.AsGodotArray<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._pendingDancerResolveRemaining, out var value8))
		{
			_pendingDancerResolveRemaining = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._hasPendingDancerRelations, out var value9))
		{
			_hasPendingDancerRelations = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.dancerPacketName, out var value10))
		{
			dancerPacketName = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.firstSpawn, out var value11))
		{
			firstSpawn = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isPointing, out var value12))
		{
			isPointing = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value13))
		{
			_roleStateSignalsConnected = value13.As<bool>();
		}
	}
}
