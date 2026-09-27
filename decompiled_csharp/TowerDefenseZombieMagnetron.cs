using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter9/Magnetron/Scene/TowerDefenseZombieMagnetron.cs")]
public class TowerDefenseZombieMagnetron : TowerDefenseZombie, INetworkSpawnStateReceiver
{
	private sealed class PendingCrush
	{
		public TowerDefensePlant Plant;

		public int SyncId = -1;

		public Vector2I Grid = new Vector2I(-1, -1);

		public string NodeName = "";

		public string SaveKey = "";

		public double Remaining;
	}

	private sealed class MoveParticipant
	{
		public TowerDefensePlant Plant;

		public int SyncId = -1;

		public string NodeName = "";

		public string SaveKey = "";
	}

	private sealed class MoveDestinationState
	{
		public Vector2I Grid = new Vector2I(-1, -1);

		public int LegacyCount;

		public readonly List<MoveParticipant> Participants = new List<MoveParticipant>();
	}

	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public static readonly StringName TryBurstTireOnSpike = "TryBurstTireOnSpike";

		public static readonly StringName BurstTireOnSpike = "BurstTireOnSpike";

		public static readonly StringName IsValidSpikeContactTarget = "IsValidSpikeContactTarget";

		public static readonly StringName ProcessAnchored = "ProcessAnchored";

		public static readonly StringName ResumeDriving = "ResumeDriving";

		public static readonly StringName CrushPlantsWhileDriving = "CrushPlantsWhileDriving";

		public static readonly StringName MoveLeft = "MoveLeft";

		public static readonly StringName SetAnchored = "SetAnchored";

		public static readonly StringName SetStopColumn = "SetStopColumn";

		public static readonly StringName GetMapLastColumn = "GetMapLastColumn";

		public static readonly StringName GetTerminalColumn = "GetTerminalColumn";

		public static readonly StringName GetFrontLocalX = "GetFrontLocalX";

		public static readonly StringName GetFrontLogicalX = "GetFrontLogicalX";

		public static readonly StringName GetRootXForFront = "GetRootXForFront";

		public static readonly StringName GetColumnX = "GetColumnX";

		public static readonly StringName GetCurrentFrontColumn = "GetCurrentFrontColumn";

		public static readonly StringName AlignFrontToColumn = "AlignFrontToColumn";

		public static readonly StringName FindNearestEligibleTargetGrid = "FindNearestEligibleTargetGrid";

		public static readonly StringName IsEligibleTarget = "IsEligibleTarget";

		public static readonly StringName IsDraggablePlant = "IsDraggablePlant";

		public static readonly StringName CrushEnemyZombiesInBody = "CrushEnemyZombiesInBody";

		public static readonly StringName CanCrushZombie = "CanCrushZombie";

		public static readonly StringName BeginShooting = "BeginShooting";

		public static readonly StringName CancelShooting = "CancelShooting";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName ExecutePull = "ExecutePull";

		public static readonly StringName QueueCrush = "QueueCrush";

		public static readonly StringName SetPullTargetIdentity = "SetPullTargetIdentity";

		public static readonly StringName ClearPullTargetIdentity = "ClearPullTargetIdentity";

		public static readonly StringName ProcessPendingCrushes = "ProcessPendingCrushes";

		public static readonly StringName DestroyCompletedCrushes = "DestroyCompletedCrushes";

		public static readonly StringName TryDeflateBeforeCompletedCrushes = "TryDeflateBeforeCompletedCrushes";

		public static readonly StringName CanAttemptPlantCrush = "CanAttemptPlantCrush";

		public static readonly StringName CancelPullAfterDeflation = "CancelPullAfterDeflation";

		public static readonly StringName FinishPendingCrushes = "FinishPendingCrushes";

		public static readonly StringName CancelActivePullForRemoval = "CancelActivePullForRemoval";

		public static readonly StringName TryRestoreMoveTweens = "TryRestoreMoveTweens";

		public static readonly StringName AbandonMoveTweenRestore = "AbandonMoveTweenRestore";

		public static readonly StringName TryRestorePullEffect = "TryRestorePullEffect";

		public static readonly StringName CrushUnscheduledPlantsAtTerminal = "CrushUnscheduledPlantsAtTerminal";

		public static readonly StringName DestroyPlantAndReplicate = "DestroyPlantAndReplicate";

		public static readonly StringName CreatePullEffect = "CreatePullEffect";

		public static readonly StringName GetTargetFallbackPosition = "GetTargetFallbackPosition";

		public static readonly StringName GetPullOriginPosition = "GetPullOriginPosition";

		public static readonly StringName PlayDriveAnimation = "PlayDriveAnimation";

		public static readonly StringName MarkStateDirty = "MarkStateDirty";

		public static readonly StringName NextSequence = "NextSequence";

		public new static readonly StringName ExportNetworkSpawnState = "ExportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public static readonly StringName ExportMagnetronState = "ExportMagnetronState";

		public static readonly StringName ExportMoveDestinations = "ExportMoveDestinations";

		public static readonly StringName PrepareMoveTweenRestore = "PrepareMoveTweenRestore";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public static readonly StringName ApplyNetworkState = "ApplyNetworkState";

		public static readonly StringName FindRemoteVisualTarget = "FindRemoteVisualTarget";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName IsNetworkSpecialMovementActive = "IsNetworkSpecialMovementActive";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName ImportPullTargetGrid = "ImportPullTargetGrid";

		public static readonly StringName ImportStopColumn = "ImportStopColumn";

		public static readonly StringName ImportPullTargetIdentity = "ImportPullTargetIdentity";

		public static readonly StringName GetEffectivePullTargetGrid = "GetEffectivePullTargetGrid";

		public static readonly StringName GetLegacyPullTargetGrid = "GetLegacyPullTargetGrid";

		public static readonly StringName GetSavedNodeName = "GetSavedNodeName";

		public static readonly StringName NormalizeSavedNodeName = "NormalizeSavedNodeName";

		public static readonly StringName HasSavedNodeName = "HasSavedNodeName";

		public static readonly StringName ResolvePlant = "ResolvePlant";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName CrushOnLeft = "CrushOnLeft";

		public static readonly StringName pullOriginPath = "pullOriginPath";

		public static readonly StringName frontAnchorPath = "frontAnchorPath";

		public static readonly StringName attackTimer = "attackTimer";

		public static readonly StringName anchored = "anchored";

		public static readonly StringName passedAnchor = "passedAnchor";

		public static readonly StringName stopColumn = "stopColumn";

		public static readonly StringName shooting = "shooting";

		public static readonly StringName pullRemaining = "pullRemaining";

		public static readonly StringName targetGrid = "targetGrid";

		public static readonly StringName pullTargetGrid = "pullTargetGrid";

		public static readonly StringName _pullOrigin = "_pullOrigin";

		public static readonly StringName _frontAnchor = "_frontAnchor";

		public static readonly StringName _pullEffect = "_pullEffect";

		public static readonly StringName _pullTargetSyncId = "_pullTargetSyncId";

		public static readonly StringName _pullTargetNodeName = "_pullTargetNodeName";

		public static readonly StringName _pullTargetSaveKey = "_pullTargetSaveKey";

		public static readonly StringName _moveRestorePending = "_moveRestorePending";

		public static readonly StringName _pullEffectRestorePending = "_pullEffectRestorePending";

		public static readonly StringName _moveRestoreSequence = "_moveRestoreSequence";

		public static readonly StringName _restoredMoveSequence = "_restoredMoveSequence";

		public static readonly StringName _actionSequence = "_actionSequence";

		public static readonly StringName _remoteActionSequence = "_remoteActionSequence";

		public static readonly StringName _networkSpecialStateRevision = "_networkSpecialStateRevision";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	public const double AttackInterval = 10.0;

	public const double PullDuration = 2.0;

	public const double DriveSpeed = 22.0;

	public const string PullEvent = "pull";

	public const string MoveKind = "magnet";

	private static PackedScene _pullEffectScene;

	public const string PullOriginNodeName = "PullOrigin";

	public const string FrontAnchorNodeName = "FrontAnchor";

	[Export(PropertyHint.None, "")]
	public NodePath pullOriginPath;

	[Export(PropertyHint.None, "")]
	public NodePath frontAnchorPath;

	public double attackTimer;

	public bool anchored;

	public bool passedAnchor;

	public int stopColumn = -1;

	public bool shooting;

	public double pullRemaining;

	public Vector2I targetGrid = new Vector2I(-1, -1);

	public Vector2I pullTargetGrid = new Vector2I(-1, -1);

	private Node2D _pullOrigin;

	private Marker2D _frontAnchor;

	private MagnetronPullEffect _pullEffect;

	private readonly List<PendingCrush> _pendingCrushes = new List<PendingCrush>();

	private readonly List<MoveDestinationState> _activeMoveDestinations = new List<MoveDestinationState>();

	private readonly List<MoveDestinationState> _pendingMoveRestores = new List<MoveDestinationState>();

	private int _pullTargetSyncId = -1;

	private string _pullTargetNodeName = "";

	private string _pullTargetSaveKey = "";

	private bool _moveRestorePending;

	private bool _pullEffectRestorePending;

	private int _moveRestoreSequence;

	private int _restoredMoveSequence;

	private int _actionSequence;

	private int _remoteActionSequence;

	private int _networkSpecialStateRevision = 1;

	private static PackedScene PullEffectScene => _pullEffectScene ?? (_pullEffectScene = GD.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter9/Magnetron/Effect/MagnetronPullEffect.tscn"));

	private bool CrushOnLeft
	{
		get
		{
			if (GodotObject.IsInstanceValid(instance))
			{
				return instance.hypnoses;
			}
			return false;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			_pullOrigin = ResolveMarkedNode<Node2D>(pullOriginPath, "PullOrigin", sprite);
			_frontAnchor = ResolveMarkedNode<Marker2D>(frontAnchorPath, "FrontAnchor", this);
		}
	}

	private T ResolveMarkedNode<T>(NodePath exportedPath, string nodeName, Node searchRoot) where T : Node
	{
		if (exportedPath != null && !exportedPath.IsEmpty)
		{
			T nodeOrNull = GetNodeOrNull<T>(exportedPath);
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				return nodeOrNull;
			}
		}
		if (!GodotObject.IsInstanceValid(searchRoot))
		{
			return null;
		}
		return searchRoot.FindChild(nodeName, recursive: true, owned: false) as T;
	}

	public override void _ExitTree()
	{
		if ((die || nearDie || isDestroy) && TowerDefenseManager.HasGameplayAuthority)
		{
			FinishPendingCrushes();
		}
		if (GodotObject.IsInstanceValid(_pullEffect))
		{
			_pullEffect.QueueFree();
		}
		_pullEffect = null;
		_pendingCrushes.Clear();
		_activeMoveDestinations.Clear();
		_pendingMoveRestores.Clear();
		base._ExitTree();
	}

	public override void WalkProcessing(double delta)
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		if (die || isDestroy)
		{
			if (TowerDefenseManager.HasGameplayAuthority)
			{
				FinishPendingCrushes();
			}
			CancelActivePullForRemoval();
			return;
		}
		sprite.timeScale = timeScale;
		if (!TryRestoreMoveTweens())
		{
			return;
		}
		TryRestorePullEffect();
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			return;
		}
		ProcessPendingCrushes(delta);
		if (TryBurstTireOnSpike() || TryDeflateBeforeCompletedCrushes())
		{
			return;
		}
		DestroyCompletedCrushes();
		CrushEnemyZombiesInBody();
		if (pullRemaining > 0.0)
		{
			pullRemaining = Math.Max(0.0, pullRemaining - delta);
			if (pullRemaining <= 0.0)
			{
				MarkStateDirty();
				_activeMoveDestinations.Clear();
				ClearPullTargetIdentity();
			}
			return;
		}
		if (nearDie)
		{
			if (shooting)
			{
				CancelShooting();
			}
			return;
		}
		int mapLastColumn = GetMapLastColumn();
		float frontLogicalX = GetFrontLogicalX();
		int currentFrontColumn = GetCurrentFrontColumn();
		if (!passedAnchor)
		{
			float columnX = GetColumnX(CrushOnLeft ? 1 : mapLastColumn);
			if (CrushOnLeft ? (frontLogicalX > columnX + 1f) : (frontLogicalX < columnX - 1f))
			{
				passedAnchor = true;
				MarkStateDirty();
			}
		}
		if (!anchored)
		{
			bool flag = (CrushOnLeft ? (currentFrontColumn > 1) : (currentFrontColumn < mapLastColumn));
			bool flag2 = FindNearestEligibleTargetGrid(currentFrontColumn, includeTerminal: true).X >= 0;
			if (!flag || !flag2)
			{
				ResumeDriving(delta);
				return;
			}
			SetStopColumn(currentFrontColumn);
			AlignFrontToColumn(stopColumn);
			SetAnchored(value: true);
		}
		ProcessAnchored(delta);
	}

	private bool TryBurstTireOnSpike()
	{
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(towerDefenseManager) || towerDefenseManager.characterRegistry == null || !TryGetValidBodyRect(out var rect))
		{
			return false;
		}
		TowerDefenseCharacter spike = null;
		TowerDefenseCharacter[] array = towerDefenseManager.characterRegistry.GetCharactersIntersectingRectListExcludingCamp(rect, camp, gridPos.Y).ToArray();
		foreach (TowerDefenseCharacter towerDefenseCharacter in array)
		{
			if (IsValidSpikeContactTarget(towerDefenseCharacter))
			{
				spike = towerDefenseCharacter;
				break;
			}
			if (GodotObject.IsInstanceValid(towerDefenseCharacter?.cell))
			{
				spike = towerDefenseCharacter.cell.characterList.FirstOrDefault(IsValidSpikeContactTarget);
				if (GodotObject.IsInstanceValid(spike))
				{
					break;
				}
			}
		}
		if (!GodotObject.IsInstanceValid(spike))
		{
			return false;
		}
		return BurstTireOnSpike(spike);
	}

	private bool BurstTireOnSpike(TowerDefenseCharacter spike)
	{
		if (!IsValidSpikeContactTarget(spike))
		{
			return false;
		}
		double spikeHurt = spike.instance.spikeHurt;
		CancelPullAfterDeflation();
		AudioManager.Instance.AudioPlay("ZamboniExplosion");
		if (spikeHurt != -1.0)
		{
			double damageLimit = spikeHurt;
			spike.Hurt(100000.0, playSplatAudio: true, default, createDamagePart: true, damageLimit);
		}
		Die();
		return true;
	}

	private bool IsValidSpikeContactTarget(TowerDefenseCharacter target)
	{
		if (GodotObject.IsInstanceValid(target) && !target.die && !target.nearDie && !target.isDestroy && GodotObject.IsInstanceValid(target.instance) && !target.instance.invincible && target.HasHitBox && CanTarget(target) && CanCollision(target.instance.maskFlags))
		{
			return (target.instance.physiqueTypeFlags & 0x10) != 0;
		}
		return false;
	}

	private void ProcessAnchored(double delta)
	{
		int terminalColumn = GetTerminalColumn();
		AlignFrontToColumn(terminalColumn);
		if (CrushUnscheduledPlantsAtTerminal())
		{
			return;
		}
		Vector2I selectedGrid = FindNearestEligibleTargetGrid(terminalColumn, includeTerminal: false);
		if (selectedGrid.X < 0)
		{
			ResumeDriving(delta);
		}
		else if (!shooting)
		{
			attackTimer += delta;
			if (attackTimer >= 10.0)
			{
				BeginShooting(selectedGrid);
			}
		}
	}

	private void ResumeDriving(double delta)
	{
		if (shooting)
		{
			CancelShooting();
		}
		SetAnchored(value: false);
		SetStopColumn(-1);
		MoveLeft(delta);
		CrushPlantsWhileDriving();
	}

	private void CrushPlantsWhileDriving()
	{
		if (!TryGetValidBodyRect(out var rect))
		{
			return;
		}
		List<TowerDefensePlant> list = (from plant in TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectListExcludingCamp(rect, camp, gridPos.Y, includeAllLineCheck: true).OfType<TowerDefensePlant>()
			where IsDraggablePlant(plant, camp) && CanCollision(plant.instance.maskFlags)
			select plant).ToList();
		if (TryDeflateBeforeCrushes(list.Select((TowerDefensePlant plant) => (Plant: plant, Grid: plant.gridPos)).ToList()))
		{
			return;
		}
		foreach (TowerDefensePlant item in list)
		{
			DestroyPlantAndReplicate(item, smash: true);
		}
	}

	private void MoveLeft(double delta, float clampX = -1f / 0f)
	{
		if (!sprite.pause)
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			logicalGlobalPosition.X += (float)(22.0 * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)Scale.X * (double)(sprite.playBack ? 1f : (-1f)));
			if (!float.IsNegativeInfinity(clampX))
			{
				logicalGlobalPosition.X = Math.Max(logicalGlobalPosition.X, clampX);
			}
			SetLogicalGlobalPosition(logicalGlobalPosition);
		}
	}

	private void SetAnchored(bool value)
	{
		if (anchored != value)
		{
			anchored = value;
			MarkStateDirty();
		}
	}

	private void SetStopColumn(int value)
	{
		int num = ((value < 1) ? (-1) : Mathf.Clamp(value, 1, GetMapLastColumn()));
		if (stopColumn != num)
		{
			stopColumn = num;
			attackTimer = 0.0;
			MarkStateDirty();
		}
	}

	private int GetMapLastColumn()
	{
		return Math.Max(1, TowerDefenseManager.Instance.GetMapGridNum().X);
	}

	private int GetTerminalColumn()
	{
		if (stopColumn >= 1)
		{
			return Mathf.Clamp(stopColumn, 1, GetMapLastColumn());
		}
		if (CrushOnLeft)
		{
			return 1;
		}
		return GetMapLastColumn();
	}

	private float GetFrontLocalX()
	{
		if (!GodotObject.IsInstanceValid(_frontAnchor))
		{
			_frontAnchor = ResolveMarkedNode<Marker2D>(frontAnchorPath, "FrontAnchor", this);
		}
		if (!GodotObject.IsInstanceValid(_frontAnchor))
		{
			return -97f;
		}
		return _frontAnchor.Position.X;
	}

	private float GetFrontLogicalX()
	{
		return GetLogicalGlobalPosition().X + GetFrontLocalX() * Scale.X;
	}

	private float GetRootXForFront(float frontX)
	{
		return frontX - GetFrontLocalX() * Scale.X;
	}

	private float GetColumnX(int column)
	{
		return TowerDefenseManager.GetMapCellPlantPos(new Vector2I(column, gridPos.Y)).X;
	}

	private int GetCurrentFrontColumn()
	{
		Vector2 pos = new Vector2(GetFrontLogicalX(), TowerDefenseManager.GetMapCellPlantPos(gridPos).Y);
		return Mathf.Clamp(TowerDefenseManager.Instance.GetMapGridPos(pos).X, 1, GetMapLastColumn());
	}

	private void AlignFrontToColumn(int column)
	{
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		logicalGlobalPosition.X = GetRootXForFront(GetColumnX(column));
		SetLogicalGlobalPosition(logicalGlobalPosition);
	}

	private Vector2I FindNearestEligibleTargetGrid(int terminalColumn, bool includeTerminal)
	{
		int mapLastColumn = GetMapLastColumn();
		if (CrushOnLeft)
		{
			for (int i = terminalColumn + ((!includeTerminal) ? 1 : 0); i <= mapLastColumn; i++)
			{
				Vector2I candidate = new Vector2I(i, gridPos.Y);
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(candidate);
				if (GodotObject.IsInstanceValid(mapCell) && mapCell.characterList.Any((TowerDefenseCharacter character) => character.gridPos == candidate && IsEligibleTarget(character, camp)))
				{
					return candidate;
				}
			}
			return new Vector2I(-1, -1);
		}
		for (int num = Mathf.Clamp(terminalColumn - ((!includeTerminal) ? 1 : 0), 0, mapLastColumn); num >= 1; num--)
		{
			Vector2I candidate2 = new Vector2I(num, gridPos.Y);
			TowerDefenseCellInstance mapCell2 = TowerDefenseManager.GetMapCell(candidate2);
			if (GodotObject.IsInstanceValid(mapCell2) && mapCell2.characterList.Any((TowerDefenseCharacter character) => character.gridPos == candidate2 && IsEligibleTarget(character, camp)))
			{
				return candidate2;
			}
		}
		return new Vector2I(-1, -1);
	}

	public static bool IsEligibleTarget(TowerDefenseCharacter character, TowerDefenseEnum.CHARACTER_CAMP myCamp)
	{
		if (IsDraggablePlant(character, myCamp))
		{
			return character.instance.height >= TowerDefenseEnum.CHARACTER_HEIGHT.NORMAL;
		}
		return false;
	}

	public static bool IsDraggablePlant(TowerDefenseCharacter character, TowerDefenseEnum.CHARACTER_CAMP myCamp)
	{
		if (character is TowerDefensePlant && GodotObject.IsInstanceValid(character) && !character.die && !character.nearDie && !character.isDestroy && GodotObject.IsInstanceValid(character.instance) && GodotObject.IsInstanceValid(character.config) && character.camp != myCamp && !character.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR) && !character.instance.invincible && !character.instance.invincibleSmash)
		{
			if (!character.instance.canBeCollection)
			{
				return character.instance.hologram;
			}
			return true;
		}
		return false;
	}

	private void CrushEnemyZombiesInBody()
	{
		double num = ((config is TowerDefenseZombieConfig towerDefenseZombieConfig) ? towerDefenseZombieConfig.smashAttack : 0.0);
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(towerDefenseManager) || towerDefenseManager.characterRegistry == null || !TryGetValidBodyRect(out var rect))
		{
			return;
		}
		TowerDefenseCharacter[] array = towerDefenseManager.characterRegistry.GetCharactersIntersectingRectListExcludingCamp(rect, camp, gridPos.Y).ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] is TowerDefenseZombie towerDefenseZombie && CanCrushZombie(towerDefenseZombie, camp))
			{
				towerDefenseZombie.SmashHurt(num, playSplatAudio: true, Vector2.Zero);
			}
		}
	}

	private bool TryGetValidBodyRect(out Rect2 rect)
	{
		if (!HasHitBox || !IsHitBoxEnabled)
		{
			rect = default;
			return false;
		}
		rect = WorldHitRect;
		return true;
	}

	private static bool CanCrushZombie(TowerDefenseZombie zombie, TowerDefenseEnum.CHARACTER_CAMP myCamp)
	{
		if (!GodotObject.IsInstanceValid(zombie) || !GodotObject.IsInstanceValid(zombie.instance))
		{
			return false;
		}
		if (zombie.die || zombie.nearDie || zombie.isDestroy)
		{
			return false;
		}
		if (zombie.camp == myCamp)
		{
			return false;
		}
		if (zombie.instance.invincible || zombie.instance.invincibleSmash)
		{
			return false;
		}
		TowerDefenseCharacterInstance towerDefenseCharacterInstance = zombie.instance;
		if (towerDefenseCharacterInstance != null && towerDefenseCharacterInstance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
		{
			return false;
		}
		int num = zombie.instance?.maskFlags ?? 0;
		if ((num & 1) == 0)
		{
			return false;
		}
		if ((num & 2) != 0)
		{
			return false;
		}
		return true;
	}

	private void BeginShooting(Vector2I selectedGrid)
	{
		attackTimer = 0.0;
		targetGrid = selectedGrid;
		pullTargetGrid = new Vector2I(-1, -1);
		ClearPullTargetIdentity();
		shooting = true;
		MarkStateDirty();
		if (GodotObject.IsInstanceValid(sprite) && sprite.HasClip("anim_shooting"))
		{
			sprite.SetAnimation("anim_shooting", loop: false);
		}
	}

	private void CancelShooting()
	{
		shooting = false;
		targetGrid = new Vector2I(-1, -1);
		pullTargetGrid = new Vector2I(-1, -1);
		ClearPullTargetIdentity();
		MarkStateDirty();
		PlayDriveAnimation();
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (string.Equals(command, "pull", StringComparison.OrdinalIgnoreCase) && TowerDefenseManager.HasGameplayAuthority && shooting && !die && !isDestroy)
		{
			ExecutePull();
		}
	}

	public override void AnimeCompleted(string clipName)
	{
		base.AnimeCompleted(clipName);
		if (clipName == "anim_shooting" && shooting && TowerDefenseManager.HasGameplayAuthority)
		{
			CancelShooting();
		}
	}

	private void ExecutePull()
	{
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(targetGrid);
		TowerDefensePlant towerDefensePlant = (GodotObject.IsInstanceValid(mapCell) ? mapCell.characterList.OfType<TowerDefensePlant>().FirstOrDefault((TowerDefensePlant plant) => plant.gridPos == targetGrid && IsEligibleTarget(plant, camp)) : null);
		shooting = false;
		if (!GodotObject.IsInstanceValid(towerDefensePlant))
		{
			targetGrid = new Vector2I(-1, -1);
			ClearPullTargetIdentity();
			MarkStateDirty();
			PlayDriveAnimation();
			return;
		}
		List<(int, List<TowerDefensePlant>)> list = SnapshotPullChain(targetGrid.X);
		if (list.Count == 0)
		{
			CancelShooting();
			return;
		}
		pullRemaining = 2.0;
		_pullEffectRestorePending = false;
		_activeMoveDestinations.Clear();
		pullTargetGrid = new Vector2I(-1, -1);
		SetPullTargetIdentity(towerDefensePlant);
		PlayDriveAnimation();
		foreach (var item3 in list.OrderByDescending(((int Column, List<TowerDefensePlant> Plants) entry) => entry.Column))
		{
			int item = item3.Item1;
			List<TowerDefensePlant> item2 = item3.Item2;
			Vector2I vector2I = MovePlantGroup(item, item2);
			if (item == targetGrid.X)
			{
				pullTargetGrid = vector2I;
			}
		}
		_actionSequence = NextSequence(_actionSequence);
		MarkStateDirty();
		CreatePullEffect(towerDefensePlant, 2.0);
	}

	private List<(int Column, List<TowerDefensePlant> Plants)> SnapshotPullChain(int startColumn)
	{
		List<(int, List<TowerDefensePlant>)> list = new List<(int, List<TowerDefensePlant>)>();
		HashSet<ulong> hashSet = new HashSet<ulong>();
		int terminalColumn = GetTerminalColumn();
		bool crushOnLeft = CrushOnLeft;
		for (int i = startColumn; crushOnLeft ? (i >= terminalColumn) : (i <= terminalColumn); i += ((!crushOnLeft) ? 1 : (-1)))
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(new Vector2I(i, gridPos.Y));
			if (!GodotObject.IsInstanceValid(mapCell))
			{
				break;
			}
			List<TowerDefensePlant> list2 = new List<TowerDefensePlant>();
			TowerDefenseCharacter[] array = mapCell.characterList.ToArray();
			foreach (TowerDefenseCharacter towerDefenseCharacter in array)
			{
				if (!(towerDefenseCharacter.gridPos != new Vector2I(i, gridPos.Y)) && IsDraggablePlant(towerDefenseCharacter, camp) && hashSet.Add(towerDefenseCharacter.GetInstanceId()))
				{
					list2.Add((TowerDefensePlant)towerDefenseCharacter);
				}
			}
			if (list2.Count == 0)
			{
				break;
			}
			list.Add((i, list2));
		}
		return list;
	}

	private Vector2I MovePlantGroup(int column, List<TowerDefensePlant> plants)
	{
		int terminalColumn = GetTerminalColumn();
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(new Vector2I(column, gridPos.Y));
		if (!GodotObject.IsInstanceValid(mapCell))
		{
			return new Vector2I(-1, -1);
		}
		List<TowerDefensePlant> list = plants.Where((TowerDefensePlant c) => IsDraggablePlant(c, camp)).ToList();
		if (list.Count == 0)
		{
			return new Vector2I(-1, -1);
		}
		if (CrushOnLeft ? (column <= terminalColumn) : (column >= terminalColumn))
		{
			foreach (TowerDefensePlant item in list)
			{
				item.EmitDestroy();
				QueueCrush(item, 2.0);
			}
			return new Vector2I(terminalColumn, gridPos.Y);
		}
		Vector2I destinationGrid = FindPullDestination(column, list);
		TowerDefenseCellInstance mapCell2 = TowerDefenseManager.GetMapCell(destinationGrid);
		if (!GodotObject.IsInstanceValid(mapCell2))
		{
			return new Vector2I(-1, -1);
		}
		foreach (TowerDefensePlant item2 in list)
		{
			Vector2I fromGridPos = item2.gridPos;
			item2.EmitDestroy();
			mapCell.MoveCharacterToCell(item2, mapCell2, 2.0);
			if (Global.IsMultiplayerMode)
			{
				MultiPlayerManager multiPlayerManager = MultiPlayerManager.Instance;
				if (multiPlayerManager != null && multiPlayerManager.isHost && item2.syncId >= 0)
				{
					MultiPlayerManager.Instance.SendMoveCharacter(item2.syncId, fromGridPos, destinationGrid, "magnet", 2.0);
				}
			}
			if (destinationGrid.X == terminalColumn)
			{
				QueueCrush(item2, 2.0);
			}
		}
		if (list.Count > 0)
		{
			MoveDestinationState moveDestinationState = _activeMoveDestinations.FirstOrDefault((MoveDestinationState state) => state.Grid == destinationGrid);
			if (moveDestinationState == null)
			{
				moveDestinationState = new MoveDestinationState
				{
					Grid = destinationGrid
				};
				_activeMoveDestinations.Add(moveDestinationState);
			}
			foreach (TowerDefensePlant item3 in list)
			{
				moveDestinationState.Participants.Add(CreateMoveParticipant(item3));
			}
		}
		return destinationGrid;
	}

	private Vector2I FindPullDestination(int sourceColumn, List<TowerDefensePlant> plants)
	{
		int terminalColumn = GetTerminalColumn();
		if (CrushOnLeft)
		{
			for (int num = sourceColumn - 1; num > terminalColumn; num--)
			{
				Vector2I result = new Vector2I(num, gridPos.Y);
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(result);
				if (GodotObject.IsInstanceValid(mapCell) && mapCell.CanMoveCharactersHere(plants))
				{
					return result;
				}
			}
			return new Vector2I(terminalColumn, gridPos.Y);
		}
		for (int i = sourceColumn + 1; i < terminalColumn; i++)
		{
			Vector2I result2 = new Vector2I(i, gridPos.Y);
			TowerDefenseCellInstance mapCell2 = TowerDefenseManager.GetMapCell(result2);
			if (GodotObject.IsInstanceValid(mapCell2) && mapCell2.CanMoveCharactersHere(plants))
			{
				return result2;
			}
		}
		return new Vector2I(terminalColumn, gridPos.Y);
	}

	private void QueueCrush(TowerDefensePlant plant, double remaining)
	{
		if (GodotObject.IsInstanceValid(plant) && !_pendingCrushes.Any((PendingCrush entry) => entry.Plant == plant))
		{
			_pendingCrushes.Add(new PendingCrush
			{
				Plant = plant,
				SyncId = plant.syncId,
				Grid = plant.gridPos,
				NodeName = GetSavedNodeName(plant),
				SaveKey = (plant.packet?.saveKey ?? ""),
				Remaining = remaining
			});
		}
	}

	private static MoveParticipant CreateMoveParticipant(TowerDefensePlant plant)
	{
		return new MoveParticipant
		{
			Plant = plant,
			SyncId = (plant?.syncId ?? (-1)),
			NodeName = GetSavedNodeName(plant),
			SaveKey = (plant?.packet?.saveKey ?? "")
		};
	}

	private void SetPullTargetIdentity(TowerDefensePlant plant)
	{
		_pullTargetSyncId = plant?.syncId ?? (-1);
		_pullTargetNodeName = GetSavedNodeName(plant);
		_pullTargetSaveKey = plant?.packet?.saveKey ?? "";
	}

	private void ClearPullTargetIdentity()
	{
		_pullTargetSyncId = -1;
		_pullTargetNodeName = "";
		_pullTargetSaveKey = "";
	}

	private void ProcessPendingCrushes(double delta)
	{
		foreach (PendingCrush pendingCrush in _pendingCrushes)
		{
			if (!GodotObject.IsInstanceValid(pendingCrush.Plant))
			{
				pendingCrush.Plant = ResolvePendingCrush(pendingCrush);
			}
			pendingCrush.Remaining = Math.Max(0.0, pendingCrush.Remaining - delta);
		}
	}

	private void DestroyCompletedCrushes()
	{
		for (int num = _pendingCrushes.Count - 1; num >= 0; num--)
		{
			PendingCrush pendingCrush = _pendingCrushes[num];
			if (!(pendingCrush.Remaining > 0.0))
			{
				if (!GodotObject.IsInstanceValid(pendingCrush.Plant))
				{
					pendingCrush.Plant = ResolvePendingCrush(pendingCrush);
				}
				if (!GodotObject.IsInstanceValid(pendingCrush.Plant))
				{
					_pendingCrushes.RemoveAt(num);
				}
				else
				{
					if (GodotObject.IsInstanceValid(pendingCrush.Plant))
					{
						CompletePendingCrush(pendingCrush);
					}
					_pendingCrushes.RemoveAt(num);
				}
			}
		}
	}

	private bool TryDeflateBeforeCompletedCrushes()
	{
		List<(TowerDefensePlant, Vector2I)> list = new List<(TowerDefensePlant, Vector2I)>();
		HashSet<ulong> hashSet = new HashSet<ulong>();
		PendingCrush[] array = _pendingCrushes.ToArray();
		foreach (PendingCrush pendingCrush in array)
		{
			if (!(pendingCrush.Remaining > 0.0))
			{
				if (!GodotObject.IsInstanceValid(pendingCrush.Plant))
				{
					pendingCrush.Plant = ResolvePendingCrush(pendingCrush);
				}
				if (GodotObject.IsInstanceValid(pendingCrush.Plant) && hashSet.Add(pendingCrush.Plant.GetInstanceId()))
				{
					list.Add((pendingCrush.Plant, pendingCrush.Grid));
				}
			}
		}
		return TryDeflateBeforeCrushes(list);
	}

	private bool TryDeflateBeforeCrushes(List<(TowerDefensePlant Plant, Vector2I Grid)> candidates)
	{
		if (candidates.Count == 0 || !TowerDefenseManager.HasGameplayAuthority)
		{
			return false;
		}
		foreach (var candidate in candidates)
		{
			TowerDefensePlant item = candidate.Plant;
			if (IsValidSpikeContactTarget(item))
			{
				return BurstTireOnSpike(item);
			}
		}
		HashSet<Vector2I> hashSet = new HashSet<Vector2I>();
		foreach (var candidate2 in candidates)
		{
			Vector2I item2 = candidate2.Grid;
			if (!hashSet.Add(item2) || !TowerDefenseManager.Instance.CheckMapGridPosIn(item2))
			{
				continue;
			}
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(item2);
			if (!GodotObject.IsInstanceValid(mapCell))
			{
				continue;
			}
			TowerDefenseCharacter[] array = mapCell.characterList.ToArray();
			foreach (TowerDefenseCharacter towerDefenseCharacter in array)
			{
				if (IsValidSpikeContactTarget(towerDefenseCharacter))
				{
					return BurstTireOnSpike(towerDefenseCharacter);
				}
			}
		}
		AttackComponent attackComponent = base.attackComponent;
		if (attackComponent == null || attackComponent.IsReleased)
		{
			return false;
		}
		double num = ((config is TowerDefenseZombieConfig towerDefenseZombieConfig) ? towerDefenseZombieConfig.smashAttack : 0.0);
		foreach (var candidate3 in candidates)
		{
			TowerDefensePlant item3 = candidate3.Plant;
			if (CanAttemptPlantCrush(item3) && base.attackComponent.TryShieldBlockSmash(item3, num, forceLethal: true))
			{
				CancelPullAfterDeflation();
				return true;
			}
		}
		return false;
	}

	private static bool CanAttemptPlantCrush(TowerDefensePlant plant)
	{
		if (GodotObject.IsInstanceValid(plant) && !plant.die && !plant.nearDie && !plant.isDestroy && GodotObject.IsInstanceValid(plant.instance) && !plant.instance.invincible)
		{
			return !plant.instance.invincibleSmash;
		}
		return false;
	}

	private void CancelPullAfterDeflation()
	{
		_pendingCrushes.Clear();
		AttackComponent attackComponent = base.attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			base.attackComponent.target = null;
		}
		shooting = false;
		attackTimer = 0.0;
		targetGrid = new Vector2I(-1, -1);
		CancelActivePullForRemoval();
		MarkStateDirty();
	}

	private void FinishPendingCrushes()
	{
		for (int num = _pendingCrushes.Count - 1; num >= 0; num--)
		{
			PendingCrush pendingCrush = _pendingCrushes[num];
			if (!GodotObject.IsInstanceValid(pendingCrush.Plant))
			{
				pendingCrush.Plant = ResolvePendingCrush(pendingCrush);
			}
			if (!GodotObject.IsInstanceValid(pendingCrush.Plant))
			{
				_pendingCrushes.RemoveAt(num);
			}
			else
			{
				CompletePendingCrush(pendingCrush);
				_pendingCrushes.RemoveAt(num);
			}
		}
	}

	private static void CompletePendingCrush(PendingCrush pending)
	{
		TowerDefensePlant plant = pending.Plant;
		if (GodotObject.IsInstanceValid(plant))
		{
			plant.CancelCellMoveTween();
			if (TowerDefenseManager.Instance.CheckMapGridPosIn(pending.Grid))
			{
				plant.SetLogicalGlobalPosition(TowerDefenseManager.GetMapCellPlantPos(pending.Grid));
			}
			DestroyPlantAndReplicate(plant, smash: true);
		}
	}

	private void CancelActivePullForRemoval()
	{
		pullRemaining = 0.0;
		pullTargetGrid = new Vector2I(-1, -1);
		ClearPullTargetIdentity();
		_pullEffectRestorePending = false;
		_moveRestorePending = false;
		_activeMoveDestinations.Clear();
		_pendingMoveRestores.Clear();
		if (GodotObject.IsInstanceValid(_pullEffect))
		{
			_pullEffect.QueueFree();
		}
		_pullEffect = null;
	}

	private bool TryRestoreMoveTweens()
	{
		if (!_moveRestorePending)
		{
			return true;
		}
		if (pullRemaining <= 0.0 || _pendingMoveRestores.Count == 0)
		{
			_moveRestorePending = false;
			_pendingMoveRestores.Clear();
			return true;
		}
		List<(MoveDestinationState, List<TowerDefensePlant>)> list = new List<(MoveDestinationState, List<TowerDefensePlant>)>();
		HashSet<ulong> hashSet = new HashSet<ulong>();
		foreach (MoveDestinationState pendingMoveRestore in _pendingMoveRestores)
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(pendingMoveRestore.Grid);
			if (!GodotObject.IsInstanceValid(mapCell))
			{
				return AbandonMoveTweenRestore();
			}
			List<TowerDefensePlant> list2 = (from c in mapCell.characterList.OfType<TowerDefensePlant>()
				where IsDraggablePlant(c, camp)
				select c).ToList();
			List<TowerDefensePlant> list3 = new List<TowerDefensePlant>();
			if (pendingMoveRestore.Participants.Count == 0)
			{
				if (list2.Count != pendingMoveRestore.LegacyCount)
				{
					return AbandonMoveTweenRestore();
				}
				list3.AddRange(list2);
			}
			else
			{
				foreach (MoveParticipant participant in pendingMoveRestore.Participants)
				{
					TowerDefensePlant towerDefensePlant = ResolveMoveParticipant(participant, pendingMoveRestore.Grid, list2, hashSet, camp);
					if (!GodotObject.IsInstanceValid(towerDefensePlant))
					{
						return AbandonMoveTweenRestore();
					}
					list3.Add(towerDefensePlant);
					hashSet.Add(towerDefensePlant.GetInstanceId());
				}
			}
			list.Add((pendingMoveRestore, list3));
		}
		_activeMoveDestinations.Clear();
		foreach (var item3 in list)
		{
			MoveDestinationState item = item3.Item1;
			List<TowerDefensePlant> item2 = item3.Item2;
			Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(item.Grid);
			MoveDestinationState moveDestinationState = new MoveDestinationState
			{
				Grid = item.Grid
			};
			foreach (TowerDefensePlant item4 in item2)
			{
				TowerDefenseCellInstance.CreateCharacterCellMoveTween(item4, mapCellPlantPos, pullRemaining);
				moveDestinationState.Participants.Add(CreateMoveParticipant(item4));
			}
			_activeMoveDestinations.Add(moveDestinationState);
		}
		_moveRestorePending = false;
		_restoredMoveSequence = Math.Max(_restoredMoveSequence, _moveRestoreSequence);
		_pendingMoveRestores.Clear();
		return true;
	}

	private bool AbandonMoveTweenRestore()
	{
		_moveRestorePending = false;
		_restoredMoveSequence = Math.Max(_restoredMoveSequence, _moveRestoreSequence);
		_pendingMoveRestores.Clear();
		_activeMoveDestinations.Clear();
		return true;
	}

	private static TowerDefensePlant ResolveMoveParticipant(MoveParticipant participant, Vector2I expectedGrid, List<TowerDefensePlant> candidates, HashSet<ulong> usedPlantIds, TowerDefenseEnum.CHARACTER_CAMP myCamp)
	{
		TowerDefensePlant towerDefensePlant = ResolvePlant(participant.SyncId);
		if (IsMatchingMoveParticipant(towerDefensePlant, participant, expectedGrid, usedPlantIds, myCamp))
		{
			return towerDefensePlant;
		}
		if (!string.IsNullOrEmpty(participant.NodeName))
		{
			TowerDefensePlant result = candidates.FirstOrDefault((TowerDefensePlant candidate) => HasSavedNodeName(candidate, participant.NodeName) && IsMatchingMoveParticipant(candidate, participant, expectedGrid, usedPlantIds, myCamp));
			if (GodotObject.IsInstanceValid(result))
			{
				return result;
			}
		}
		return null;
	}

	private static bool IsMatchingMoveParticipant(TowerDefensePlant plant, MoveParticipant participant, Vector2I expectedGrid, HashSet<ulong> usedPlantIds, TowerDefenseEnum.CHARACTER_CAMP myCamp)
	{
		if (GodotObject.IsInstanceValid(plant) && IsDraggablePlant(plant, myCamp) && plant.gridPos == expectedGrid && !usedPlantIds.Contains(plant.GetInstanceId()))
		{
			if (!string.IsNullOrEmpty(participant.SaveKey))
			{
				return plant.packet?.saveKey == participant.SaveKey;
			}
			return true;
		}
		return false;
	}

	private void TryRestorePullEffect()
	{
		if (_pullEffectRestorePending)
		{
			if (pullRemaining <= 0.0)
			{
				_pullEffectRestorePending = false;
			}
			else if (IsNodeReady())
			{
				CreatePullEffect(FindRemoteVisualTarget(), pullRemaining);
				_pullEffectRestorePending = false;
			}
		}
	}

	private bool CrushUnscheduledPlantsAtTerminal()
	{
		if (pullRemaining > 0.0)
		{
			return false;
		}
		int terminalColumn = GetTerminalColumn();
		Vector2I terminalGrid = new Vector2I(terminalColumn, gridPos.Y);
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(terminalGrid);
		if (!GodotObject.IsInstanceValid(mapCell))
		{
			return false;
		}
		List<TowerDefensePlant> list = (from plant in mapCell.characterList.OfType<TowerDefensePlant>()
			where plant.gridPos == terminalGrid && IsDraggablePlant(plant, camp) && !_pendingCrushes.Any((PendingCrush entry) => entry.Plant == plant)
			group plant by plant.GetInstanceId() into @group
			select @group.First()).ToList();
		List<(TowerDefensePlant, Vector2I)> candidates = list.Select((TowerDefensePlant plant) => (Plant: plant, Grid: terminalGrid)).ToList();
		if (TryDeflateBeforeCrushes(candidates))
		{
			return true;
		}
		foreach (TowerDefensePlant item in list)
		{
			if (CanAttemptPlantCrush(item))
			{
				DestroyPlantAndReplicate(item, smash: true);
			}
		}
		return false;
	}

	private static void DestroyPlantAndReplicate(TowerDefensePlant plant, bool smash)
	{
		if (!GodotObject.IsInstanceValid(plant) || plant.isDestroy || (smash && (plant.instance.invincible || plant.instance.invincibleSmash)))
		{
			return;
		}
		if (Global.IsMultiplayerMode)
		{
			MultiPlayerManager multiPlayerManager = MultiPlayerManager.Instance;
			if (multiPlayerManager != null && multiPlayerManager.isHost && plant.syncId >= 0)
			{
				MultiPlayerManager.Instance.SendCharacterDestroy(plant.syncId, isExplode: false, smash);
			}
		}
		if (smash)
		{
			plant.SmashDestroy();
		}
		else
		{
			plant.Destroy();
		}
	}

	private void CreatePullEffect(TowerDefenseCharacter target, double duration)
	{
		if (GodotObject.IsInstanceValid(PullEffectScene))
		{
			if (GodotObject.IsInstanceValid(_pullEffect))
			{
				_pullEffect.QueueFree();
			}
			_pullEffect = PullEffectScene.Instantiate<MagnetronPullEffect>(PackedScene.GenEditState.Disabled);
			TowerDefenseManager.GetCharacterNode().AddChild(_pullEffect, forceReadableName: false, InternalMode.Disabled);
			_pullEffect.Configure(this, target, GetTargetFallbackPosition(), duration);
		}
	}

	private Vector2 GetTargetFallbackPosition()
	{
		Vector2I effectivePullTargetGrid = GetEffectivePullTargetGrid();
		if (effectivePullTargetGrid.X < 0)
		{
			return GetLogicalGlobalPosition();
		}
		return TowerDefenseManager.GetMapCellPlantPos(effectivePullTargetGrid);
	}

	public Vector2 GetPullOriginPosition()
	{
		if (!GodotObject.IsInstanceValid(_pullOrigin))
		{
			_pullOrigin = ResolveMarkedNode<Node2D>(pullOriginPath, "PullOrigin", sprite);
		}
		if (!GodotObject.IsInstanceValid(_pullOrigin))
		{
			return GetLogicalGlobalPosition() + new Vector2(-45f, -55f);
		}
		return _pullOrigin.GlobalPosition;
	}

	private void PlayDriveAnimation()
	{
		if (GodotObject.IsInstanceValid(sprite) && sprite.HasClip("anim_drive"))
		{
			sprite.SetAnimation("anim_drive");
		}
	}

	private void MarkStateDirty()
	{
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			_networkSpecialStateRevision = NextSequence(_networkSpecialStateRevision);
		}
	}

	private static int NextSequence(int current)
	{
		current++;
		if (current > 0)
		{
			return current;
		}
		return 1;
	}

	public override Dictionary ExportNetworkSpawnState()
	{
		return ExportMagnetronState();
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		return ExportMagnetronState();
	}

	private Dictionary ExportMagnetronState()
	{
		return new Dictionary
		{
			["rev"] = _networkSpecialStateRevision,
			["anchored"] = anchored,
			["passedAnchor"] = passedAnchor,
			["stopColumn"] = stopColumn,
			["attackTimer"] = attackTimer,
			["shooting"] = shooting,
			["pullRemaining"] = pullRemaining,
			["targetX"] = targetGrid.X,
			["targetY"] = targetGrid.Y,
			["pullTargetX"] = pullTargetGrid.X,
			["pullTargetY"] = pullTargetGrid.Y,
			["pullTargetSyncId"] = _pullTargetSyncId,
			["pullTargetNodeName"] = _pullTargetNodeName,
			["pullTargetSaveKey"] = _pullTargetSaveKey,
			["actionSequence"] = _actionSequence,
			["moveDestinations"] = ExportMoveDestinations()
		};
	}

	private Array<Dictionary> ExportMoveDestinations()
	{
		Array<Dictionary> array = new Array<Dictionary>();
		foreach (MoveDestinationState activeMoveDestination in _activeMoveDestinations)
		{
			Array<Dictionary> array2 = new Array<Dictionary>();
			foreach (MoveParticipant participant in activeMoveDestination.Participants)
			{
				array2.Add(new Dictionary
				{
					["syncId"] = participant.SyncId,
					["nodeName"] = participant.NodeName,
					["saveKey"] = participant.SaveKey
				});
			}
			array.Add(new Dictionary
			{
				["x"] = activeMoveDestination.Grid.X,
				["y"] = activeMoveDestination.Grid.Y,
				["count"] = activeMoveDestination.Participants.Count,
				["plants"] = array2
			});
		}
		return array;
	}

	private void PrepareMoveTweenRestore(Dictionary data, int sequence)
	{
		_pendingMoveRestores.Clear();
		_activeMoveDestinations.Clear();
		foreach (Variant item in data.GetValueOrDefault("moveDestinations", new Godot.Collections.Array()).AsGodotArray())
		{
			Dictionary dictionary = item.AsGodotDictionary();
			int num = Math.Max(0, dictionary.GetValueOrDefault("count", 0).AsInt32());
			Vector2I grid = new Vector2I(dictionary.GetValueOrDefault("x", -1).AsInt32(), dictionary.GetValueOrDefault("y", -1).AsInt32());
			if (num <= 0 || grid.X < 1 || grid.Y < 1)
			{
				continue;
			}
			MoveDestinationState moveDestinationState = new MoveDestinationState
			{
				Grid = grid,
				LegacyCount = num
			};
			bool flag = dictionary.ContainsKey("plants");
			foreach (Variant item2 in dictionary.GetValueOrDefault("plants", new Godot.Collections.Array()).AsGodotArray())
			{
				Dictionary dictionary2 = item2.AsGodotDictionary();
				moveDestinationState.Participants.Add(new MoveParticipant
				{
					SyncId = dictionary2.GetValueOrDefault("syncId", -1).AsInt32(),
					NodeName = NormalizeSavedNodeName(dictionary2.GetValueOrDefault("nodeName", "").AsString()),
					SaveKey = dictionary2.GetValueOrDefault("saveKey", "").AsString()
				});
			}
			if (!flag || moveDestinationState.Participants.Count == num)
			{
				_pendingMoveRestores.Add(moveDestinationState);
			}
		}
		_moveRestoreSequence = sequence;
		_moveRestorePending = pullRemaining > 0.0 && _pendingMoveRestores.Count > 0;
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		ApplyNetworkState(data);
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		ApplyNetworkState(data);
	}

	private void ApplyNetworkState(Dictionary data)
	{
		if (data == null)
		{
			return;
		}
		int num = data.GetValueOrDefault("rev", _networkSpecialStateRevision).AsInt32();
		if (!TowerDefenseManager.HasGameplayAuthority && num < _networkSpecialStateRevision)
		{
			return;
		}
		_networkSpecialStateRevision = Math.Max(_networkSpecialStateRevision, num);
		anchored = data.GetValueOrDefault("anchored", anchored).AsBool();
		passedAnchor = data.GetValueOrDefault("passedAnchor", passedAnchor).AsBool();
		attackTimer = data.GetValueOrDefault("attackTimer", attackTimer).AsDouble();
		shooting = data.GetValueOrDefault("shooting", shooting).AsBool();
		pullRemaining = data.GetValueOrDefault("pullRemaining", pullRemaining).AsDouble();
		stopColumn = ImportStopColumn(data, stopColumn);
		targetGrid = new Vector2I(data.GetValueOrDefault("targetX", targetGrid.X).AsInt32(), data.GetValueOrDefault("targetY", targetGrid.Y).AsInt32());
		pullTargetGrid = ImportPullTargetGrid(data);
		ImportPullTargetIdentity(data);
		int num2 = data.GetValueOrDefault("actionSequence", _actionSequence).AsInt32();
		_actionSequence = Math.Max(_actionSequence, num2);
		if (!TowerDefenseManager.HasGameplayAuthority && num2 > _restoredMoveSequence && pullRemaining > 0.0)
		{
			PrepareMoveTweenRestore(data, num2);
		}
		if (!TowerDefenseManager.HasGameplayAuthority && num2 > _remoteActionSequence && pullRemaining > 0.0)
		{
			_remoteActionSequence = num2;
			if (IsNodeReady())
			{
				TowerDefenseCharacter target = FindRemoteVisualTarget();
				CreatePullEffect(target, pullRemaining);
			}
			else
			{
				_pullEffectRestorePending = true;
			}
		}
	}

	private TowerDefenseCharacter FindRemoteVisualTarget()
	{
		Vector2I effectivePullTargetGrid = GetEffectivePullTargetGrid();
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(effectivePullTargetGrid);
		if (!GodotObject.IsInstanceValid(mapCell))
		{
			return null;
		}
		TowerDefensePlant towerDefensePlant = ResolvePlant(_pullTargetSyncId);
		if (GodotObject.IsInstanceValid(towerDefensePlant) && towerDefensePlant.gridPos == effectivePullTargetGrid && IsDraggablePlant(towerDefensePlant, camp))
		{
			return towerDefensePlant;
		}
		if (!string.IsNullOrEmpty(_pullTargetNodeName))
		{
			TowerDefensePlant result = mapCell.characterList.OfType<TowerDefensePlant>().FirstOrDefault((TowerDefensePlant plant) => IsDraggablePlant(plant, camp) && HasSavedNodeName(plant, _pullTargetNodeName) && (string.IsNullOrEmpty(_pullTargetSaveKey) || plant.packet?.saveKey == _pullTargetSaveKey));
			if (GodotObject.IsInstanceValid(result))
			{
				return result;
			}
		}
		return mapCell.characterList.FirstOrDefault((TowerDefenseCharacter c) => IsDraggablePlant(c, camp));
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return _networkSpecialStateRevision;
	}

	public override bool IsNetworkSpecialMovementActive()
	{
		return false;
	}

	public override Dictionary ExportVariantSave()
	{
		Array<Dictionary> array = new Array<Dictionary>();
		foreach (PendingCrush pendingCrush in _pendingCrushes)
		{
			array.Add(new Dictionary
			{
				["syncId"] = pendingCrush.SyncId,
				["gridX"] = pendingCrush.Grid.X,
				["gridY"] = pendingCrush.Grid.Y,
				["nodeName"] = pendingCrush.NodeName,
				["saveKey"] = pendingCrush.SaveKey,
				["remaining"] = pendingCrush.Remaining
			});
		}
		return new Dictionary
		{
			["attackTimer"] = attackTimer,
			["anchored"] = anchored,
			["passedAnchor"] = passedAnchor,
			["stopColumn"] = stopColumn,
			["shooting"] = shooting,
			["pullRemaining"] = pullRemaining,
			["targetX"] = targetGrid.X,
			["targetY"] = targetGrid.Y,
			["pullTargetX"] = pullTargetGrid.X,
			["pullTargetY"] = pullTargetGrid.Y,
			["pullTargetSyncId"] = _pullTargetSyncId,
			["pullTargetNodeName"] = _pullTargetNodeName,
			["pullTargetSaveKey"] = _pullTargetSaveKey,
			["actionSequence"] = _actionSequence,
			["networkSpecialStateRevision"] = _networkSpecialStateRevision,
			["pendingCrushes"] = array,
			["moveDestinations"] = ExportMoveDestinations()
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		attackTimer = data.GetValueOrDefault("attackTimer", 0.0).AsDouble();
		anchored = data.GetValueOrDefault("anchored", false).AsBool();
		passedAnchor = data.GetValueOrDefault("passedAnchor", false).AsBool();
		shooting = data.GetValueOrDefault("shooting", false).AsBool();
		pullRemaining = data.GetValueOrDefault("pullRemaining", 0.0).AsDouble();
		stopColumn = ImportStopColumn(data, -1);
		targetGrid = new Vector2I(data.GetValueOrDefault("targetX", -1).AsInt32(), data.GetValueOrDefault("targetY", -1).AsInt32());
		pullTargetGrid = ImportPullTargetGrid(data);
		ImportPullTargetIdentity(data);
		_actionSequence = data.GetValueOrDefault("actionSequence", 0).AsInt32();
		_networkSpecialStateRevision = data.GetValueOrDefault("networkSpecialStateRevision", 1).AsInt32();
		if (GodotObject.IsInstanceValid(_pullEffect))
		{
			_pullEffect.QueueFree();
		}
		_pullEffect = null;
		_pullEffectRestorePending = pullRemaining > 0.0;
		PrepareMoveTweenRestore(data, _actionSequence);
		_pendingCrushes.Clear();
		foreach (Variant item in data.GetValueOrDefault("pendingCrushes", new Godot.Collections.Array()).AsGodotArray())
		{
			Dictionary dictionary = item.AsGodotDictionary();
			int num = dictionary.GetValueOrDefault("syncId", -1).AsInt32();
			Vector2I grid = new Vector2I(dictionary.GetValueOrDefault("gridX", -1).AsInt32(), dictionary.GetValueOrDefault("gridY", -1).AsInt32());
			if (num >= 0 || (grid.X >= 1 && grid.Y >= 1))
			{
				TowerDefensePlant plant = ResolvePlant(num);
				_pendingCrushes.Add(new PendingCrush
				{
					Plant = plant,
					SyncId = num,
					Grid = grid,
					NodeName = NormalizeSavedNodeName(dictionary.GetValueOrDefault("nodeName", "").AsString()),
					SaveKey = dictionary.GetValueOrDefault("saveKey", "").AsString(),
					Remaining = dictionary.GetValueOrDefault("remaining", 0.0).AsDouble()
				});
			}
		}
	}

	private Vector2I ImportPullTargetGrid(Dictionary data)
	{
		Vector2I legacyPullTargetGrid = GetLegacyPullTargetGrid();
		return new Vector2I(data.GetValueOrDefault("pullTargetX", legacyPullTargetGrid.X).AsInt32(), data.GetValueOrDefault("pullTargetY", legacyPullTargetGrid.Y).AsInt32());
	}

	private int ImportStopColumn(Dictionary data, int existing)
	{
		if (data.ContainsKey("stopColumn"))
		{
			int num = data.GetValueOrDefault("stopColumn", -1).AsInt32();
			if (num >= 1)
			{
				return Mathf.Clamp(num, 1, GetMapLastColumn());
			}
			return -1;
		}
		if (existing >= 1)
		{
			return Mathf.Clamp(existing, 1, GetMapLastColumn());
		}
		if (!anchored && !shooting && !(pullRemaining > 0.0))
		{
			return -1;
		}
		if (!CrushOnLeft)
		{
			return GetMapLastColumn();
		}
		return 1;
	}

	private void ImportPullTargetIdentity(Dictionary data)
	{
		_pullTargetSyncId = data.GetValueOrDefault("pullTargetSyncId", -1).AsInt32();
		_pullTargetNodeName = NormalizeSavedNodeName(data.GetValueOrDefault("pullTargetNodeName", "").AsString());
		_pullTargetSaveKey = data.GetValueOrDefault("pullTargetSaveKey", "").AsString();
	}

	private Vector2I GetEffectivePullTargetGrid()
	{
		if (pullTargetGrid.X < 1 || pullTargetGrid.Y < 1)
		{
			return GetLegacyPullTargetGrid();
		}
		return pullTargetGrid;
	}

	private Vector2I GetLegacyPullTargetGrid()
	{
		if (targetGrid.X < 1 || targetGrid.Y < 1)
		{
			return new Vector2I(-1, -1);
		}
		return new Vector2I(Math.Min(targetGrid.X + 1, GetTerminalColumn()), targetGrid.Y);
	}

	private TowerDefensePlant ResolvePendingCrush(PendingCrush pending)
	{
		TowerDefensePlant result = ResolvePlant(pending.SyncId);
		if (GodotObject.IsInstanceValid(result))
		{
			return result;
		}
		if (!TowerDefenseManager.Instance.CheckMapGridPosIn(pending.Grid))
		{
			return null;
		}
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(pending.Grid);
		if (!GodotObject.IsInstanceValid(mapCell))
		{
			return null;
		}
		List<TowerDefensePlant> source = (from plant in mapCell.characterList.OfType<TowerDefensePlant>()
			where IsDraggablePlant(plant, camp) && (string.IsNullOrEmpty(pending.SaveKey) || plant.packet?.saveKey == pending.SaveKey) && !_pendingCrushes.Any((PendingCrush other) => other != pending && other.Plant == plant)
			select plant).ToList();
		if (!string.IsNullOrEmpty(pending.NodeName))
		{
			return source.FirstOrDefault((TowerDefensePlant plant) => HasSavedNodeName(plant, pending.NodeName));
		}
		return null;
	}

	private static string GetSavedNodeName(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return "";
		}
		return NormalizeSavedNodeName(node.Name.ToString());
	}

	private static string NormalizeSavedNodeName(string nodeName)
	{
		if (!string.IsNullOrEmpty(nodeName))
		{
			return nodeName.ValidateNodeName();
		}
		return "";
	}

	private static bool HasSavedNodeName(Node node, string savedNodeName)
	{
		if (!string.IsNullOrEmpty(savedNodeName))
		{
			return GetSavedNodeName(node) == NormalizeSavedNodeName(savedNodeName);
		}
		return false;
	}

	private static TowerDefensePlant ResolvePlant(int syncId)
	{
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		if (syncId < 0 || !GodotObject.IsInstanceValid(currentControl) || !currentControl._syncCharacters.TryGetValue(syncId, out var value))
		{
			return null;
		}
		return value as TowerDefensePlant;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(73)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryBurstTireOnSpike, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BurstTireOnSpike, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spike", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsValidSpikeContactTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessAnchored, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResumeDriving, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CrushPlantsWhileDriving, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveLeft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "clampX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAnchored, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetStopColumn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMapLastColumn, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetTerminalColumn, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFrontLocalX, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFrontLogicalX, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetRootXForFront, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "frontX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetColumnX, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentFrontColumn, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AlignFrontToColumn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindNearestEligibleTargetGrid, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "terminalColumn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "includeTerminal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsEligibleTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "myCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsDraggablePlant, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "myCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CrushEnemyZombiesInBody, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanCrushZombie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "myCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginShooting, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "selectedGrid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelShooting, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExecutePull, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueCrush, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "remaining", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPullTargetIdentity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearPullTargetIdentity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessPendingCrushes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroyCompletedCrushes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryDeflateBeforeCompletedCrushes, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanAttemptPlantCrush, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.CancelPullAfterDeflation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FinishPendingCrushes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelActivePullForRemoval, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryRestoreMoveTweens, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AbandonMoveTweenRestore, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryRestorePullEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CrushUnscheduledPlantsAtTerminal, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroyPlantAndReplicate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plant", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "smash", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePullEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTargetFallbackPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPullOriginPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayDriveAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkStateDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NextSequence, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "current", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpawnState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportMagnetronState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportMoveDestinations, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareMoveTweenRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sequence", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyNetworkState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindRemoteVisualTarget, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsNetworkSpecialMovementActive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportPullTargetGrid, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportStopColumn, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "existing", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportPullTargetIdentity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetEffectivePullTargetGrid, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetLegacyPullTargetGrid, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSavedNodeName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeSavedNodeName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasSavedNodeName, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "savedNodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolvePlant, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "syncId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryBurstTireOnSpike && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryBurstTireOnSpike());
			return true;
		}
		if (method == MethodName.BurstTireOnSpike && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(BurstTireOnSpike(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.IsValidSpikeContactTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidSpikeContactTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.ProcessAnchored && args.Count == 1)
		{
			ProcessAnchored(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResumeDriving && args.Count == 1)
		{
			ResumeDriving(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CrushPlantsWhileDriving && args.Count == 0)
		{
			CrushPlantsWhileDriving();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveLeft && args.Count == 2)
		{
			MoveLeft(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAnchored && args.Count == 1)
		{
			SetAnchored(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetStopColumn && args.Count == 1)
		{
			SetStopColumn(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetMapLastColumn && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetMapLastColumn());
			return true;
		}
		if (method == MethodName.GetTerminalColumn && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetTerminalColumn());
			return true;
		}
		if (method == MethodName.GetFrontLocalX && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<float>(GetFrontLocalX());
			return true;
		}
		if (method == MethodName.GetFrontLogicalX && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<float>(GetFrontLogicalX());
			return true;
		}
		if (method == MethodName.GetRootXForFront && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(GetRootXForFront(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.GetColumnX && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(GetColumnX(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurrentFrontColumn && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCurrentFrontColumn());
			return true;
		}
		if (method == MethodName.AlignFrontToColumn && args.Count == 1)
		{
			AlignFrontToColumn(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindNearestEligibleTargetGrid && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(FindNearestEligibleTargetGrid(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.IsEligibleTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEligibleTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.IsDraggablePlant && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDraggablePlant(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.CrushEnemyZombiesInBody && args.Count == 0)
		{
			CrushEnemyZombiesInBody();
			ret = default;
			return true;
		}
		if (method == MethodName.CanCrushZombie && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCrushZombie(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.BeginShooting && args.Count == 1)
		{
			BeginShooting(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CancelShooting && args.Count == 0)
		{
			CancelShooting();
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
		if (method == MethodName.ExecutePull && args.Count == 0)
		{
			ExecutePull();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueCrush && args.Count == 2)
		{
			QueueCrush(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPullTargetIdentity && args.Count == 1)
		{
			SetPullTargetIdentity(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPullTargetIdentity && args.Count == 0)
		{
			ClearPullTargetIdentity();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessPendingCrushes && args.Count == 1)
		{
			ProcessPendingCrushes(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyCompletedCrushes && args.Count == 0)
		{
			DestroyCompletedCrushes();
			ret = default;
			return true;
		}
		if (method == MethodName.TryDeflateBeforeCompletedCrushes && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryDeflateBeforeCompletedCrushes());
			return true;
		}
		if (method == MethodName.CanAttemptPlantCrush && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanAttemptPlantCrush(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0])));
			return true;
		}
		if (method == MethodName.CancelPullAfterDeflation && args.Count == 0)
		{
			CancelPullAfterDeflation();
			ret = default;
			return true;
		}
		if (method == MethodName.FinishPendingCrushes && args.Count == 0)
		{
			FinishPendingCrushes();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelActivePullForRemoval && args.Count == 0)
		{
			CancelActivePullForRemoval();
			ret = default;
			return true;
		}
		if (method == MethodName.TryRestoreMoveTweens && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryRestoreMoveTweens());
			return true;
		}
		if (method == MethodName.AbandonMoveTweenRestore && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(AbandonMoveTweenRestore());
			return true;
		}
		if (method == MethodName.TryRestorePullEffect && args.Count == 0)
		{
			TryRestorePullEffect();
			ret = default;
			return true;
		}
		if (method == MethodName.CrushUnscheduledPlantsAtTerminal && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CrushUnscheduledPlantsAtTerminal());
			return true;
		}
		if (method == MethodName.DestroyPlantAndReplicate && args.Count == 2)
		{
			DestroyPlantAndReplicate(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePullEffect && args.Count == 2)
		{
			CreatePullEffect(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetTargetFallbackPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetTargetFallbackPosition());
			return true;
		}
		if (method == MethodName.GetPullOriginPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetPullOriginPosition());
			return true;
		}
		if (method == MethodName.PlayDriveAnimation && args.Count == 0)
		{
			PlayDriveAnimation();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkStateDirty && args.Count == 0)
		{
			MarkStateDirty();
			ret = default;
			return true;
		}
		if (method == MethodName.NextSequence && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(NextSequence(VariantUtils.ConvertTo<int>(in args[0])));
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
		if (method == MethodName.ExportMagnetronState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportMagnetronState());
			return true;
		}
		if (method == MethodName.ExportMoveDestinations && args.Count == 0)
		{
			Array<Dictionary> array = ExportMoveDestinations();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.PrepareMoveTweenRestore && args.Count == 2)
		{
			PrepareMoveTweenRestore(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState && args.Count == 1)
		{
			ImportNetworkSpecialState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyNetworkState && args.Count == 1)
		{
			ApplyNetworkState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindRemoteVisualTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindRemoteVisualTarget());
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
		if (method == MethodName.ImportPullTargetGrid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ImportPullTargetGrid(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.ImportStopColumn && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ImportStopColumn(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ImportPullTargetIdentity && args.Count == 1)
		{
			ImportPullTargetIdentity(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetEffectivePullTargetGrid && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetEffectivePullTargetGrid());
			return true;
		}
		if (method == MethodName.GetLegacyPullTargetGrid && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetLegacyPullTargetGrid());
			return true;
		}
		if (method == MethodName.GetSavedNodeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetSavedNodeName(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeSavedNodeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeSavedNodeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasSavedNodeName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSavedNodeName(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolvePlant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlant>(ResolvePlant(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsEligibleTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEligibleTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.IsDraggablePlant && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDraggablePlant(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.CanCrushZombie && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCrushZombie(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.CanAttemptPlantCrush && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanAttemptPlantCrush(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0])));
			return true;
		}
		if (method == MethodName.DestroyPlantAndReplicate && args.Count == 2)
		{
			DestroyPlantAndReplicate(VariantUtils.ConvertTo<TowerDefensePlant>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.NextSequence && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(NextSequence(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSavedNodeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetSavedNodeName(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeSavedNodeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeSavedNodeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasSavedNodeName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSavedNodeName(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolvePlant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefensePlant>(ResolvePlant(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.TryBurstTireOnSpike)
		{
			return true;
		}
		if (method == MethodName.BurstTireOnSpike)
		{
			return true;
		}
		if (method == MethodName.IsValidSpikeContactTarget)
		{
			return true;
		}
		if (method == MethodName.ProcessAnchored)
		{
			return true;
		}
		if (method == MethodName.ResumeDriving)
		{
			return true;
		}
		if (method == MethodName.CrushPlantsWhileDriving)
		{
			return true;
		}
		if (method == MethodName.MoveLeft)
		{
			return true;
		}
		if (method == MethodName.SetAnchored)
		{
			return true;
		}
		if (method == MethodName.SetStopColumn)
		{
			return true;
		}
		if (method == MethodName.GetMapLastColumn)
		{
			return true;
		}
		if (method == MethodName.GetTerminalColumn)
		{
			return true;
		}
		if (method == MethodName.GetFrontLocalX)
		{
			return true;
		}
		if (method == MethodName.GetFrontLogicalX)
		{
			return true;
		}
		if (method == MethodName.GetRootXForFront)
		{
			return true;
		}
		if (method == MethodName.GetColumnX)
		{
			return true;
		}
		if (method == MethodName.GetCurrentFrontColumn)
		{
			return true;
		}
		if (method == MethodName.AlignFrontToColumn)
		{
			return true;
		}
		if (method == MethodName.FindNearestEligibleTargetGrid)
		{
			return true;
		}
		if (method == MethodName.IsEligibleTarget)
		{
			return true;
		}
		if (method == MethodName.IsDraggablePlant)
		{
			return true;
		}
		if (method == MethodName.CrushEnemyZombiesInBody)
		{
			return true;
		}
		if (method == MethodName.CanCrushZombie)
		{
			return true;
		}
		if (method == MethodName.BeginShooting)
		{
			return true;
		}
		if (method == MethodName.CancelShooting)
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
		if (method == MethodName.ExecutePull)
		{
			return true;
		}
		if (method == MethodName.QueueCrush)
		{
			return true;
		}
		if (method == MethodName.SetPullTargetIdentity)
		{
			return true;
		}
		if (method == MethodName.ClearPullTargetIdentity)
		{
			return true;
		}
		if (method == MethodName.ProcessPendingCrushes)
		{
			return true;
		}
		if (method == MethodName.DestroyCompletedCrushes)
		{
			return true;
		}
		if (method == MethodName.TryDeflateBeforeCompletedCrushes)
		{
			return true;
		}
		if (method == MethodName.CanAttemptPlantCrush)
		{
			return true;
		}
		if (method == MethodName.CancelPullAfterDeflation)
		{
			return true;
		}
		if (method == MethodName.FinishPendingCrushes)
		{
			return true;
		}
		if (method == MethodName.CancelActivePullForRemoval)
		{
			return true;
		}
		if (method == MethodName.TryRestoreMoveTweens)
		{
			return true;
		}
		if (method == MethodName.AbandonMoveTweenRestore)
		{
			return true;
		}
		if (method == MethodName.TryRestorePullEffect)
		{
			return true;
		}
		if (method == MethodName.CrushUnscheduledPlantsAtTerminal)
		{
			return true;
		}
		if (method == MethodName.DestroyPlantAndReplicate)
		{
			return true;
		}
		if (method == MethodName.CreatePullEffect)
		{
			return true;
		}
		if (method == MethodName.GetTargetFallbackPosition)
		{
			return true;
		}
		if (method == MethodName.GetPullOriginPosition)
		{
			return true;
		}
		if (method == MethodName.PlayDriveAnimation)
		{
			return true;
		}
		if (method == MethodName.MarkStateDirty)
		{
			return true;
		}
		if (method == MethodName.NextSequence)
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
		if (method == MethodName.ExportMagnetronState)
		{
			return true;
		}
		if (method == MethodName.ExportMoveDestinations)
		{
			return true;
		}
		if (method == MethodName.PrepareMoveTweenRestore)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpecialState)
		{
			return true;
		}
		if (method == MethodName.ApplyNetworkState)
		{
			return true;
		}
		if (method == MethodName.FindRemoteVisualTarget)
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
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportPullTargetGrid)
		{
			return true;
		}
		if (method == MethodName.ImportStopColumn)
		{
			return true;
		}
		if (method == MethodName.ImportPullTargetIdentity)
		{
			return true;
		}
		if (method == MethodName.GetEffectivePullTargetGrid)
		{
			return true;
		}
		if (method == MethodName.GetLegacyPullTargetGrid)
		{
			return true;
		}
		if (method == MethodName.GetSavedNodeName)
		{
			return true;
		}
		if (method == MethodName.NormalizeSavedNodeName)
		{
			return true;
		}
		if (method == MethodName.HasSavedNodeName)
		{
			return true;
		}
		if (method == MethodName.ResolvePlant)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.pullOriginPath)
		{
			pullOriginPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.frontAnchorPath)
		{
			frontAnchorPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.attackTimer)
		{
			attackTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.anchored)
		{
			anchored = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.passedAnchor)
		{
			passedAnchor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.stopColumn)
		{
			stopColumn = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.shooting)
		{
			shooting = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.pullRemaining)
		{
			pullRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.targetGrid)
		{
			targetGrid = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.pullTargetGrid)
		{
			pullTargetGrid = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._pullOrigin)
		{
			_pullOrigin = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._frontAnchor)
		{
			_frontAnchor = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName._pullEffect)
		{
			_pullEffect = VariantUtils.ConvertTo<MagnetronPullEffect>(in value);
			return true;
		}
		if (name == PropertyName._pullTargetSyncId)
		{
			_pullTargetSyncId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._pullTargetNodeName)
		{
			_pullTargetNodeName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._pullTargetSaveKey)
		{
			_pullTargetSaveKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._moveRestorePending)
		{
			_moveRestorePending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pullEffectRestorePending)
		{
			_pullEffectRestorePending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._moveRestoreSequence)
		{
			_moveRestoreSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._restoredMoveSequence)
		{
			_restoredMoveSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._actionSequence)
		{
			_actionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._remoteActionSequence)
		{
			_remoteActionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._networkSpecialStateRevision)
		{
			_networkSpecialStateRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.CrushOnLeft)
		{
			value = VariantUtils.CreateFrom<bool>(CrushOnLeft);
			return true;
		}
		if (name == PropertyName.pullOriginPath)
		{
			value = VariantUtils.CreateFrom(in pullOriginPath);
			return true;
		}
		if (name == PropertyName.frontAnchorPath)
		{
			value = VariantUtils.CreateFrom(in frontAnchorPath);
			return true;
		}
		if (name == PropertyName.attackTimer)
		{
			value = VariantUtils.CreateFrom(in attackTimer);
			return true;
		}
		if (name == PropertyName.anchored)
		{
			value = VariantUtils.CreateFrom(in anchored);
			return true;
		}
		if (name == PropertyName.passedAnchor)
		{
			value = VariantUtils.CreateFrom(in passedAnchor);
			return true;
		}
		if (name == PropertyName.stopColumn)
		{
			value = VariantUtils.CreateFrom(in stopColumn);
			return true;
		}
		if (name == PropertyName.shooting)
		{
			value = VariantUtils.CreateFrom(in shooting);
			return true;
		}
		if (name == PropertyName.pullRemaining)
		{
			value = VariantUtils.CreateFrom(in pullRemaining);
			return true;
		}
		if (name == PropertyName.targetGrid)
		{
			value = VariantUtils.CreateFrom(in targetGrid);
			return true;
		}
		if (name == PropertyName.pullTargetGrid)
		{
			value = VariantUtils.CreateFrom(in pullTargetGrid);
			return true;
		}
		if (name == PropertyName._pullOrigin)
		{
			value = VariantUtils.CreateFrom(in _pullOrigin);
			return true;
		}
		if (name == PropertyName._frontAnchor)
		{
			value = VariantUtils.CreateFrom(in _frontAnchor);
			return true;
		}
		if (name == PropertyName._pullEffect)
		{
			value = VariantUtils.CreateFrom(in _pullEffect);
			return true;
		}
		if (name == PropertyName._pullTargetSyncId)
		{
			value = VariantUtils.CreateFrom(in _pullTargetSyncId);
			return true;
		}
		if (name == PropertyName._pullTargetNodeName)
		{
			value = VariantUtils.CreateFrom(in _pullTargetNodeName);
			return true;
		}
		if (name == PropertyName._pullTargetSaveKey)
		{
			value = VariantUtils.CreateFrom(in _pullTargetSaveKey);
			return true;
		}
		if (name == PropertyName._moveRestorePending)
		{
			value = VariantUtils.CreateFrom(in _moveRestorePending);
			return true;
		}
		if (name == PropertyName._pullEffectRestorePending)
		{
			value = VariantUtils.CreateFrom(in _pullEffectRestorePending);
			return true;
		}
		if (name == PropertyName._moveRestoreSequence)
		{
			value = VariantUtils.CreateFrom(in _moveRestoreSequence);
			return true;
		}
		if (name == PropertyName._restoredMoveSequence)
		{
			value = VariantUtils.CreateFrom(in _restoredMoveSequence);
			return true;
		}
		if (name == PropertyName._actionSequence)
		{
			value = VariantUtils.CreateFrom(in _actionSequence);
			return true;
		}
		if (name == PropertyName._remoteActionSequence)
		{
			value = VariantUtils.CreateFrom(in _remoteActionSequence);
			return true;
		}
		if (name == PropertyName._networkSpecialStateRevision)
		{
			value = VariantUtils.CreateFrom(in _networkSpecialStateRevision);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.NodePath, PropertyName.pullOriginPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.frontAnchorPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.anchored, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.passedAnchor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.stopColumn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.shooting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.pullRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.targetGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.pullTargetGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pullOrigin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._frontAnchor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pullEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pullTargetSyncId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pullTargetNodeName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pullTargetSaveKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._moveRestorePending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pullEffectRestorePending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._moveRestoreSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._restoredMoveSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._actionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._remoteActionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._networkSpecialStateRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CrushOnLeft, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.pullOriginPath, Variant.From(in pullOriginPath));
		info.AddProperty(PropertyName.frontAnchorPath, Variant.From(in frontAnchorPath));
		info.AddProperty(PropertyName.attackTimer, Variant.From(in attackTimer));
		info.AddProperty(PropertyName.anchored, Variant.From(in anchored));
		info.AddProperty(PropertyName.passedAnchor, Variant.From(in passedAnchor));
		info.AddProperty(PropertyName.stopColumn, Variant.From(in stopColumn));
		info.AddProperty(PropertyName.shooting, Variant.From(in shooting));
		info.AddProperty(PropertyName.pullRemaining, Variant.From(in pullRemaining));
		info.AddProperty(PropertyName.targetGrid, Variant.From(in targetGrid));
		info.AddProperty(PropertyName.pullTargetGrid, Variant.From(in pullTargetGrid));
		info.AddProperty(PropertyName._pullOrigin, Variant.From(in _pullOrigin));
		info.AddProperty(PropertyName._frontAnchor, Variant.From(in _frontAnchor));
		info.AddProperty(PropertyName._pullEffect, Variant.From(in _pullEffect));
		info.AddProperty(PropertyName._pullTargetSyncId, Variant.From(in _pullTargetSyncId));
		info.AddProperty(PropertyName._pullTargetNodeName, Variant.From(in _pullTargetNodeName));
		info.AddProperty(PropertyName._pullTargetSaveKey, Variant.From(in _pullTargetSaveKey));
		info.AddProperty(PropertyName._moveRestorePending, Variant.From(in _moveRestorePending));
		info.AddProperty(PropertyName._pullEffectRestorePending, Variant.From(in _pullEffectRestorePending));
		info.AddProperty(PropertyName._moveRestoreSequence, Variant.From(in _moveRestoreSequence));
		info.AddProperty(PropertyName._restoredMoveSequence, Variant.From(in _restoredMoveSequence));
		info.AddProperty(PropertyName._actionSequence, Variant.From(in _actionSequence));
		info.AddProperty(PropertyName._remoteActionSequence, Variant.From(in _remoteActionSequence));
		info.AddProperty(PropertyName._networkSpecialStateRevision, Variant.From(in _networkSpecialStateRevision));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.pullOriginPath, out var value))
		{
			pullOriginPath = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.frontAnchorPath, out var value2))
		{
			frontAnchorPath = value2.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.attackTimer, out var value3))
		{
			attackTimer = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.anchored, out var value4))
		{
			anchored = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.passedAnchor, out var value5))
		{
			passedAnchor = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.stopColumn, out var value6))
		{
			stopColumn = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.shooting, out var value7))
		{
			shooting = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.pullRemaining, out var value8))
		{
			pullRemaining = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.targetGrid, out var value9))
		{
			targetGrid = value9.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.pullTargetGrid, out var value10))
		{
			pullTargetGrid = value10.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._pullOrigin, out var value11))
		{
			_pullOrigin = value11.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._frontAnchor, out var value12))
		{
			_frontAnchor = value12.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName._pullEffect, out var value13))
		{
			_pullEffect = value13.As<MagnetronPullEffect>();
		}
		if (info.TryGetProperty(PropertyName._pullTargetSyncId, out var value14))
		{
			_pullTargetSyncId = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName._pullTargetNodeName, out var value15))
		{
			_pullTargetNodeName = value15.As<string>();
		}
		if (info.TryGetProperty(PropertyName._pullTargetSaveKey, out var value16))
		{
			_pullTargetSaveKey = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName._moveRestorePending, out var value17))
		{
			_moveRestorePending = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pullEffectRestorePending, out var value18))
		{
			_pullEffectRestorePending = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._moveRestoreSequence, out var value19))
		{
			_moveRestoreSequence = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName._restoredMoveSequence, out var value20))
		{
			_restoredMoveSequence = value20.As<int>();
		}
		if (info.TryGetProperty(PropertyName._actionSequence, out var value21))
		{
			_actionSequence = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName._remoteActionSequence, out var value22))
		{
			_remoteActionSequence = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName._networkSpecialStateRevision, out var value23))
		{
			_networkSpecialStateRevision = value23.As<int>();
		}
	}
}
