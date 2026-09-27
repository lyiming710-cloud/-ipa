using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Asset/Anime/Character/Zombie/Boss/EdgarII/Scene/TowerDefenseZombieBossEdgarII.cs")]
public class TowerDefenseZombieBossEdgarII : TowerDefenseZombie, INetworkSpawnStateReceiver
{
	private enum EdgarAction
	{
		Idle,
		FlyToSmash,
		Smash,
		FlyHome,
		Fire,
		Pulse,
		Summon,
		Entering
	}

	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName ShouldUpdateGridPos = "ShouldUpdateGridPos";

		public static readonly StringName StartNextSkill = "StartNextSkill";

		public static readonly StringName ShouldBeginEntry = "ShouldBeginEntry";

		public static readonly StringName BeginEntry = "BeginEntry";

		public static readonly StringName AdvanceEntry = "AdvanceEntry";

		public static readonly StringName StartIdleRowMove = "StartIdleRowMove";

		public static readonly StringName AdvanceIdleRowMove = "AdvanceIdleRowMove";

		public static readonly StringName ScheduleNextIdleMove = "ScheduleNextIdleMove";

		public static readonly StringName StartSmashFlight = "StartSmashFlight";

		public static readonly StringName StartFlight = "StartFlight";

		public static readonly StringName AdvanceFlight = "AdvanceFlight";

		public static readonly StringName ReturnHome = "ReturnHome";

		public static readonly StringName GetHomeColumn = "GetHomeColumn";

		public static readonly StringName ResolveHomeGridPosition = "ResolveHomeGridPosition";

		public static readonly StringName SetAction = "SetAction";

		public static readonly StringName EnterIdle = "EnterIdle";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName ResolveSmash = "ResolveSmash";

		public static readonly StringName ResolveFireball = "ResolveFireball";

		public static readonly StringName ResolvePulse = "ResolvePulse";

		public static readonly StringName SelectNextSkillByRoll = "SelectNextSkillByRoll";

		public static readonly StringName PickSkillByRoll = "PickSkillByRoll";

		public static readonly StringName GetSkillWeight = "GetSkillWeight";

		public static readonly StringName PlayLegacyPulseEffects = "PlayLegacyPulseEffects";

		public static readonly StringName TryPlayPendingRemotePulseEffects = "TryPlayPendingRemotePulseEffects";

		public static readonly StringName ResolveSummon = "ResolveSummon";

		public static readonly StringName MarkStateDirty = "MarkStateDirty";

		public static readonly StringName AdvancePulseResend = "AdvancePulseResend";

		public static readonly StringName ExportEdgarState = "ExportEdgarState";

		public static readonly StringName ImportEdgarState = "ImportEdgarState";

		public static readonly StringName ImportPulseNetworkState = "ImportPulseNetworkState";

		public static readonly StringName SerializeGridPositions = "SerializeGridPositions";

		public static readonly StringName DeserializeGridPositions = "DeserializeGridPositions";

		public static readonly StringName DeserializeIntegerArray = "DeserializeIntegerArray";

		public static readonly StringName ApplyPresentation = "ApplyPresentation";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpawnState = "ExportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName IsNetworkSpecialMovementActive = "IsNetworkSpecialMovementActive";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _action = "_action";

		public static readonly StringName _idleTimer = "_idleTimer";

		public static readonly StringName _flightTimer = "_flightTimer";

		public static readonly StringName _flightStart = "_flightStart";

		public static readonly StringName _flightEnd = "_flightEnd";

		public static readonly StringName _smashGrid = "_smashGrid";

		public static readonly StringName _idleMoveWaitTimer = "_idleMoveWaitTimer";

		public static readonly StringName _idleMoveWaitDuration = "_idleMoveWaitDuration";

		public static readonly StringName _idleRowMoveActive = "_idleRowMoveActive";

		public static readonly StringName _idleRowMoveTimer = "_idleRowMoveTimer";

		public static readonly StringName _idleRowMoveDuration = "_idleRowMoveDuration";

		public static readonly StringName _idleRowMoveStart = "_idleRowMoveStart";

		public static readonly StringName _idleRowMoveEnd = "_idleRowMoveEnd";

		public static readonly StringName _idleRowMoveTargetRow = "_idleRowMoveTargetRow";

		public static readonly StringName _skillPendingAfterRowMove = "_skillPendingAfterRowMove";

		public static readonly StringName _lastSkill = "_lastSkill";

		public static readonly StringName _skillRepeatCount = "_skillRepeatCount";

		public static readonly StringName _networkRevision = "_networkRevision";

		public static readonly StringName _pulseActionSequence = "_pulseActionSequence";

		public static readonly StringName _remotePulseActionSequence = "_remotePulseActionSequence";

		public static readonly StringName _remotePulseStateInitialized = "_remotePulseStateInitialized";

		public static readonly StringName _remoteNetworkStateInitialized = "_remoteNetworkStateInitialized";

		public static readonly StringName _pulseResendTimer = "_pulseResendTimer";

		public static readonly StringName _pulseResendAccumulator = "_pulseResendAccumulator";

		public static readonly StringName _pulseAffectedCells = "_pulseAffectedCells";

		public static readonly StringName _pulseAffectedCharacterIds = "_pulseAffectedCharacterIds";

		public static readonly StringName _pendingRemotePulseCells = "_pendingRemotePulseCells";

		public static readonly StringName _pendingRemotePulseCharacterIds = "_pendingRemotePulseCharacterIds";

		public static readonly StringName _pendingRemotePulseResolveTimer = "_pendingRemotePulseResolveTimer";

		public static readonly StringName _speedDownEffect = "_speedDownEffect";

		public static readonly StringName _pendingPresentation = "_pendingPresentation";

		public static readonly StringName _pendingPresentationFrame = "_pendingPresentationFrame";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const int SaveVersion = 3;

	private const double IdleDuration = 10.0;

	private const double FlightDuration = 0.75;

	private const double IdleMoveWaitMinimum = 3.0;

	private const double IdleMoveWaitMaximum = 6.0;

	private const double IdleRowMoveDurationPerRow = 0.75;

	private const double PulseResendDuration = 0.75;

	private const double PulseResendInterval = 0.1;

	private const double RemotePulseResolveDuration = 1.0;

	private const string FireballConfigName = "ZombieBossEdgarIIFireball";

	private static readonly string[] SummonGiantPool = new string[16]
	{
		"ZombieGargantuar", "ZombieGargantuarRedEyes", "ZombieGargantuarGloompult", "ZombieGargantuarPresentBox", "ZombieGargantuarWallnutZ", "ZombieSkeletuar", "ZombieGargantuarBucket", "ZombieGargantuarHelmet", "ZombieGargantuarScreendoor", "ZombieGargantuarShield",
		"ZombieGargantuarBlackHelmet", "ZombieGargantuarSpecialHelmet", "ZombieFootballGargantuar", "ZombieGargantuarZamboni", "ZombieGargantuarDiamond", "ZombieDiscoGargantuar"
	};

	private const string SpeedDownEffectPath = "res://Asset/Anime/Character/Zombie/Boss/EdgarII/Effect/EdgarSPDown.tscn";

	private const int SkillWeightTotal = 40;

	private EdgarAction _action;

	private double _idleTimer;

	private double _flightTimer;

	private Vector2 _flightStart;

	private Vector2 _flightEnd;

	private Vector2I _smashGrid = Vector2I.One;

	private double _idleMoveWaitTimer;

	private double _idleMoveWaitDuration;

	private bool _idleRowMoveActive;

	private double _idleRowMoveTimer;

	private double _idleRowMoveDuration;

	private Vector2 _idleRowMoveStart;

	private Vector2 _idleRowMoveEnd;

	private int _idleRowMoveTargetRow;

	private bool _skillPendingAfterRowMove;

	private int _lastSkill;

	private int _skillRepeatCount;

	private int _networkRevision;

	private int _pulseActionSequence;

	private int _remotePulseActionSequence;

	private bool _remotePulseStateInitialized;

	private bool _remoteNetworkStateInitialized;

	private double _pulseResendTimer;

	private double _pulseResendAccumulator;

	private readonly Array<Vector2I> _pulseAffectedCells = new Array<Vector2I>();

	private readonly Array<int> _pulseAffectedCharacterIds = new Array<int>();

	private Array<Vector2I> _pendingRemotePulseCells;

	private Array<int> _pendingRemotePulseCharacterIds;

	private double _pendingRemotePulseResolveTimer;

	private PackedScene _speedDownEffect;

	private bool _pendingPresentation;

	private int _pendingPresentationFrame = -1;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && HasValidRuntimeConfiguration)
		{
			TargetRegistrationComponent targetRegistrationComponent = base.targetRegistrationComponent;
			if (targetRegistrationComponent != null && !targetRegistrationComponent.IsReleased)
			{
				base.targetRegistrationComponent.attackGridLineAliasOffset = -1;
			}
			AttackComponent attackComponent = base.attackComponent;
			if (attackComponent != null && !attackComponent.IsReleased)
			{
				base.attackComponent.alive = false;
				base.attackComponent.SetAlive(alive: false);
			}
			_speedDownEffect = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Boss/EdgarII/Effect/EdgarSPDown.tscn", null, ResourceLoader.CacheMode.Reuse);
			TryPlayPendingRemotePulseEffects(0.0);
			if (_pendingPresentation)
			{
				ApplyPresentation();
			}
			else
			{
				EnterIdle(resetTimer: true);
			}
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || !inGame || die || TowerDefenseManager.Instance == null || !TowerDefenseManager.Instance.IsGameRunning())
		{
			return;
		}
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			AdvancePulseResend(delta);
		}
		TryPlayPendingRemotePulseEffects(delta);
		if (_action == EdgarAction.Entering)
		{
			if (TowerDefenseManager.HasGameplayAuthority)
			{
				AdvanceEntry();
			}
			return;
		}
		EdgarAction action = _action;
		if ((action == EdgarAction.FlyToSmash || action == EdgarAction.FlyHome) ? true : false)
		{
			AdvanceFlight(delta);
		}
		if (_action == EdgarAction.Idle && _idleRowMoveActive)
		{
			if (TowerDefenseManager.HasGameplayAuthority)
			{
				_idleTimer += delta;
				if (_idleTimer >= 10.0)
				{
					AdvanceIdleRowMove(Math.Max(0.0, _idleRowMoveDuration - _idleRowMoveTimer));
					_skillPendingAfterRowMove = false;
					_idleTimer = 0.0;
					StartNextSkill();
					return;
				}
			}
			AdvanceIdleRowMove(delta);
		}
		else
		{
			if (!TowerDefenseManager.HasGameplayAuthority || _action != EdgarAction.Idle)
			{
				return;
			}
			_idleTimer += delta;
			if (_idleTimer >= 10.0)
			{
				_idleTimer = 0.0;
				StartNextSkill();
				return;
			}
			_idleMoveWaitTimer += delta;
			if (_idleMoveWaitTimer >= _idleMoveWaitDuration)
			{
				StartIdleRowMove();
			}
		}
	}

	public override void Walk()
	{
		if (_action == EdgarAction.Entering)
		{
			base.Walk();
		}
		else if (ShouldBeginEntry())
		{
			BeginEntry();
		}
		else
		{
			Idle();
		}
	}

	public override bool ShouldUpdateGridPos()
	{
		if (inGame && (gridPos.X <= 0 || _action == EdgarAction.Entering))
		{
			return false;
		}
		return base.ShouldUpdateGridPos();
	}

	private void StartNextSkill()
	{
		int skill = ((_skillRepeatCount >= 2) ? _lastSkill : 0);
		int num = 40 - GetSkillWeight(skill);
		switch (SelectNextSkillByRoll(GD.RandRange(0, num - 1)))
		{
		case 2:
			StartSmashFlight();
			break;
		case 3:
			SetAction(EdgarAction.Fire, "Fire", loop: false);
			break;
		case 4:
			SetAction(EdgarAction.Pulse, "Electronic", loop: false);
			break;
		default:
			SetAction(EdgarAction.Summon, "Summon", loop: false);
			break;
		}
	}

	private bool ShouldBeginEntry()
	{
		if (editorPreviewMode || !TowerDefenseManager.HasGameplayAuthority || gridPos.X > 0 || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return false;
		}
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		if (mapGridNum.X > 0)
		{
			return mapGridNum.Y > 0;
		}
		return false;
	}

	private void BeginEntry()
	{
		_action = EdgarAction.Entering;
		_idleTimer = 0.0;
		_idleRowMoveActive = false;
		_skillPendingAfterRowMove = false;
		sprite?.SetAnimation("Walk");
		base.Walk();
		MarkStateDirty();
	}

	private void AdvanceEntry()
	{
		Vector2I vector2I = ResolveHomeGridPosition(TowerDefenseManager.Instance.GetMapGridNum(), gridPos.Y);
		Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(vector2I);
		if (!(GetLogicalGlobalPosition().X > mapCellPlantPos.X))
		{
			gridPos = vector2I;
			SetLogicalGlobalPosition(mapCellPlantPos);
			EnterIdle(resetTimer: true);
		}
	}

	private void StartIdleRowMove()
	{
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		if (mapGridNum.Y <= 1)
		{
			ScheduleNextIdleMove();
			return;
		}
		int num = Mathf.Clamp(gridPos.Y, 1, mapGridNum.Y);
		int num2 = GD.RandRange(1, mapGridNum.Y - 1);
		if (num2 >= num)
		{
			num2++;
		}
		_idleRowMoveTargetRow = num2;
		_idleRowMoveTimer = 0.0;
		_idleRowMoveDuration = (double)Math.Abs(num2 - num) * 0.75;
		if (_idleTimer + _idleRowMoveDuration >= 10.0)
		{
			_idleMoveWaitTimer = 0.0;
			_idleMoveWaitDuration = Math.Max(0.0, 10.0 - _idleTimer);
			return;
		}
		_idleRowMoveStart = GetLogicalGlobalPosition();
		_idleRowMoveEnd = TowerDefenseManager.GetMapCellPlantPos(new Vector2I(GetHomeColumn(), num2));
		_idleRowMoveActive = true;
		MarkStateDirty();
	}

	private void AdvanceIdleRowMove(double delta)
	{
		_idleRowMoveTimer = Math.Min(_idleRowMoveDuration, _idleRowMoveTimer + delta);
		float weight = ((_idleRowMoveDuration <= 0.0) ? 1f : ((float)(_idleRowMoveTimer / _idleRowMoveDuration)));
		SetLogicalGlobalPosition(_idleRowMoveStart.Lerp(_idleRowMoveEnd, weight));
		if (!(_idleRowMoveTimer < _idleRowMoveDuration))
		{
			SetLogicalGlobalPosition(_idleRowMoveEnd);
			gridPos = new Vector2I(GetHomeColumn(), _idleRowMoveTargetRow);
			_idleRowMoveActive = false;
			_skillPendingAfterRowMove = false;
			ScheduleNextIdleMove();
			MarkStateDirty();
		}
	}

	private void ScheduleNextIdleMove()
	{
		_idleMoveWaitTimer = 0.0;
		_idleMoveWaitDuration = (TowerDefenseManager.HasGameplayAuthority ? GD.RandRange(3.0, 6.0) : 0.0);
	}

	private void StartSmashFlight()
	{
		Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
		int num = Math.Min(4, mapGridNum.X);
		_smashGrid = new Vector2I(GD.RandRange(num, Math.Max(num, mapGridNum.X)), GD.RandRange(1, Math.Max(1, mapGridNum.Y)));
		StartFlight(EdgarAction.FlyToSmash, _smashGrid, "Walk");
	}

	private void StartFlight(EdgarAction action, Vector2I destination, string clip)
	{
		_action = action;
		_flightTimer = 0.0;
		_flightStart = GetLogicalGlobalPosition();
		_flightEnd = TowerDefenseManager.GetMapCellPlantPos(destination);
		gridPos = destination;
		sprite?.SetAnimation(clip);
		MarkStateDirty();
	}

	private void AdvanceFlight(double delta)
	{
		_flightTimer = Math.Min(0.75, _flightTimer + delta);
		float weight = (float)(_flightTimer / 0.75);
		SetLogicalGlobalPosition(_flightStart.Lerp(_flightEnd, weight));
		if (!(_flightTimer < 0.75) && TowerDefenseManager.HasGameplayAuthority)
		{
			SetLogicalGlobalPosition(_flightEnd);
			if (_action == EdgarAction.FlyToSmash)
			{
				SetAction(EdgarAction.Smash, "Smash", loop: false);
			}
			else
			{
				EnterIdle(resetTimer: true);
			}
		}
	}

	private void ReturnHome()
	{
		StartFlight(EdgarAction.FlyHome, new Vector2I(GetHomeColumn(), Mathf.Clamp(_smashGrid.Y, 1, TowerDefenseManager.Instance.GetMapGridNum().Y)), "Walk2");
	}

	private int GetHomeColumn()
	{
		return ResolveHomeGridPosition(TowerDefenseManager.Instance.GetMapGridNum(), gridPos.Y).X;
	}

	internal static Vector2I ResolveHomeGridPosition(Vector2I mapSize, int requestedRow)
	{
		return new Vector2I(Mathf.Clamp(9, 1, Math.Max(1, mapSize.X)), Mathf.Clamp(requestedRow, 1, Math.Max(1, mapSize.Y)));
	}

	private void SetAction(EdgarAction action, string clip, bool loop)
	{
		_action = action;
		sprite?.SetAnimation(clip, loop);
		MarkStateDirty();
	}

	private void EnterIdle(bool resetTimer)
	{
		_action = EdgarAction.Idle;
		Idle();
		if (resetTimer)
		{
			_idleTimer = 0.0;
			_idleRowMoveActive = false;
			_skillPendingAfterRowMove = false;
			ScheduleNextIdleMove();
		}
		sprite?.SetAnimation("Idle");
		MarkStateDirty();
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (!TowerDefenseManager.HasGameplayAuthority || die)
		{
			return;
		}
		switch (command)
		{
		case "smash":
			if (_action == EdgarAction.Smash)
			{
				ResolveSmash();
			}
			break;
		case "fire":
			if (_action == EdgarAction.Fire)
			{
				ResolveFireball();
			}
			break;
		case "pulse":
			if (_action == EdgarAction.Pulse)
			{
				ResolvePulse();
			}
			break;
		case "summon":
			if (_action == EdgarAction.Summon)
			{
				ResolveSummon();
			}
			break;
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!TowerDefenseManager.HasGameplayAuthority || die)
		{
			return;
		}
		switch (_action)
		{
		case EdgarAction.Smash:
			if (clip == "Smash")
			{
				ReturnHome();
			}
			break;
		case EdgarAction.Fire:
			if (!(clip == "Fire"))
			{
				break;
			}
			goto IL_0078;
		case EdgarAction.Pulse:
			if (!(clip == "Electronic"))
			{
				break;
			}
			goto IL_0078;
		case EdgarAction.Summon:
			if (!(clip == "Summon"))
			{
				break;
			}
			goto IL_0078;
		case EdgarAction.FlyHome:
			break;
			IL_0078:
			EnterIdle(resetTimer: true);
			break;
		}
	}

	private void ResolveSmash()
	{
		foreach (TowerDefenseCharacter cleanCharacters in TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList())
		{
			if (cleanCharacters is TowerDefensePlant { die: false, nearDie: false, inGame: not false } towerDefensePlant)
			{
				Vector2I vector2I = towerDefensePlant.gridPos - _smashGrid;
				if (vector2I.X <= 0 && vector2I.X >= -2 && Math.Abs(vector2I.Y) <= 1)
				{
					towerDefensePlant.SmashHurt(10000000.0, playSplatAudio: true, Vector2.Zero);
				}
			}
		}
	}

	private void ResolveFireball()
	{
		TowerDefenseProjectileConfig projectileConfig = TowerDefenseManager.GetProjectileConfig("ZombieBossEdgarIIFireball");
		if (projectileConfig != null)
		{
			Vector2 vector = GetLogicalGlobalPosition() + new Vector2(-55f, 0f);
			double height = GetGroundHeight(vector.Y) - groundHeight;
			Vector2 pos = vector;
			Vector2 velocity = new Vector2(-300f, 0f);
			int collisionFlags = projectileConfig.collisionFlags;
			TowerDefenseEnum.CHARACTER_CAMP cHARACTER_CAMP = camp;
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				flipXOverride = true
			};
			FireComponent.TryCreateProjectilePositionByConfig(this, null, height, pos, velocity, projectileConfig, out var _, collisionFlags, cHARACTER_CAMP, default, overrides);
		}
	}

	private void ResolvePulse()
	{
		HashSet<Vector2I> hashSet = new HashSet<Vector2I>();
		HashSet<int> hashSet2 = new HashSet<int>();
		List<TowerDefensePlant> list = new List<TowerDefensePlant>();
		foreach (TowerDefenseCharacter cleanCharacters in TowerDefenseManager.Instance.characterRegistry.GetCleanCharactersList())
		{
			if (cleanCharacters is TowerDefensePlant towerDefensePlant && !(towerDefensePlant is TowerDefensePlantBowlingBase) && !towerDefensePlant.die && !towerDefensePlant.nearDie && towerDefensePlant.inGame && TowerDefenseManager.Instance.CheckMapGridPosIn(towerDefensePlant.gridPos))
			{
				towerDefensePlant.buff.AddBuff(new TowerDefenseCharacterBuffAttackSpeedDown
				{
					timeScaleValue = 0.5,
					time = 15.0
				});
				hashSet.Add(towerDefensePlant.gridPos);
				list.Add(towerDefensePlant);
				if (towerDefensePlant.syncId >= 0)
				{
					hashSet2.Add(towerDefensePlant.syncId);
				}
			}
		}
		List<Vector2I> list2 = new List<Vector2I>(hashSet);
		list2.Sort((Vector2I left, Vector2I right) =>
		{
			int num = left.Y.CompareTo(right.Y);
			return (num == 0) ? left.X.CompareTo(right.X) : num;
		});
		_pulseAffectedCells.Clear();
		foreach (Vector2I item in list2)
		{
			_pulseAffectedCells.Add(item);
		}
		List<int> list3 = new List<int>(hashSet2);
		list3.Sort();
		_pulseAffectedCharacterIds.Clear();
		foreach (int item2 in list3)
		{
			_pulseAffectedCharacterIds.Add(item2);
		}
		_pulseActionSequence++;
		_pulseResendTimer = 0.75;
		_pulseResendAccumulator = 0.0;
		PlayPulseEffects(list);
		MarkStateDirty();
	}

	private void PlayPulseEffects(IReadOnlyList<TowerDefensePlant> affectedPlants)
	{
		if (_speedDownEffect == null || affectedPlants == null)
		{
			return;
		}
		foreach (TowerDefensePlant affectedPlant in affectedPlants)
		{
			if (!GodotObject.IsInstanceValid(affectedPlant) || affectedPlant.die || affectedPlant.nearDie || !affectedPlant.inGame)
			{
				continue;
			}
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(_speedDownEffect, affectedPlant.gridPos, "Fire");
			if (!GodotObject.IsInstanceValid(towerDefenseEffectSpriteOnce))
			{
				continue;
			}
			Node2D node2D = (GodotObject.IsInstanceValid(affectedPlant.frontEffectNode) ? affectedPlant.frontEffectNode : TowerDefenseManager.GetCharacterNode());
			if (!GodotObject.IsInstanceValid(node2D))
			{
				towerDefenseEffectSpriteOnce.Free();
				continue;
			}
			node2D.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
			if (node2D == affectedPlant.frontEffectNode)
			{
				towerDefenseEffectSpriteOnce.Position = Vector2.Zero;
				towerDefenseEffectSpriteOnce.ZIndex = 0;
			}
			else
			{
				towerDefenseEffectSpriteOnce.GlobalPosition = affectedPlant.GetLogicalGlobalPosition();
			}
		}
	}

	private int SelectNextSkillByRoll(int roll)
	{
		int excludedSkill = ((_skillRepeatCount >= 2) ? _lastSkill : 0);
		int num = PickSkillByRoll(roll, excludedSkill);
		if (num == _lastSkill)
		{
			_skillRepeatCount = Math.Min(2, _skillRepeatCount + 1);
		}
		else
		{
			_lastSkill = num;
			_skillRepeatCount = 1;
		}
		return num;
	}

	private static int PickSkillByRoll(int roll, int excludedSkill)
	{
		int num = 40 - GetSkillWeight(excludedSkill);
		if (roll < 0 || roll >= num)
		{
			throw new ArgumentOutOfRangeException("roll");
		}
		for (int i = 2; i <= 5; i++)
		{
			if (i != excludedSkill)
			{
				int skillWeight = GetSkillWeight(i);
				if (roll < skillWeight)
				{
					return i;
				}
				roll -= skillWeight;
			}
		}
		throw new InvalidOperationException("Edgar II skill weight table is invalid.");
	}

	private static int GetSkillWeight(int skill)
	{
		return skill switch
		{
			2 => 5, 
			3 => 12, 
			4 => 8, 
			5 => 15, 
			_ => 0, 
		};
	}

	private void PlayLegacyPulseEffects(Array<Vector2I> affectedCells)
	{
		if (_speedDownEffect == null || affectedCells == null)
		{
			return;
		}
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(node2D))
		{
			return;
		}
		foreach (Vector2I affectedCell in affectedCells)
		{
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(_speedDownEffect, affectedCell, "Fire");
			if (GodotObject.IsInstanceValid(towerDefenseEffectSpriteOnce))
			{
				node2D.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
				towerDefenseEffectSpriteOnce.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(affectedCell);
			}
		}
	}

	private void TryPlayPendingRemotePulseEffects(double delta)
	{
		if (!IsNodeReady() || _speedDownEffect == null)
		{
			return;
		}
		Array<int> pendingRemotePulseCharacterIds = _pendingRemotePulseCharacterIds;
		if (pendingRemotePulseCharacterIds != null && pendingRemotePulseCharacterIds.Count > 0)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (!GodotObject.IsInstanceValid(currentControl))
			{
				_pendingRemotePulseResolveTimer = Math.Max(0.0, _pendingRemotePulseResolveTimer - delta);
				if (_pendingRemotePulseResolveTimer <= 0.0)
				{
					_pendingRemotePulseCharacterIds = null;
					_pendingRemotePulseCells = null;
				}
				return;
			}
			List<TowerDefensePlant> list = new List<TowerDefensePlant>(_pendingRemotePulseCharacterIds.Count);
			Array<int> array = new Array<int>();
			foreach (int pendingRemotePulseCharacterId in _pendingRemotePulseCharacterIds)
			{
				if (!currentControl._syncCharacters.TryGetValue(pendingRemotePulseCharacterId, out var value) || !(value is TowerDefensePlant item) || !GodotObject.IsInstanceValid(item))
				{
					array.Add(pendingRemotePulseCharacterId);
				}
				else
				{
					list.Add(item);
				}
			}
			PlayPulseEffects(list);
			_pendingRemotePulseResolveTimer = Math.Max(0.0, _pendingRemotePulseResolveTimer - delta);
			_pendingRemotePulseCharacterIds = ((array.Count > 0 && _pendingRemotePulseResolveTimer > 0.0) ? array : null);
			if (_pendingRemotePulseCharacterIds == null)
			{
				_pendingRemotePulseCells = null;
			}
		}
		else if (_pendingRemotePulseCells != null)
		{
			PlayLegacyPulseEffects(_pendingRemotePulseCells);
			_pendingRemotePulseCells = null;
		}
	}

	private void ResolveSummon()
	{
		if (SummonGiantPool.Length == 0)
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(SummonGiantPool[GD.RandRange(0, SummonGiantPool.Length - 1)]);
		if (packetConfig != null)
		{
			Vector2I mapGridNum = TowerDefenseManager.Instance.GetMapGridNum();
			for (int i = 1; i <= mapGridNum.Y; i++)
			{
				packetConfig.Plant(new Vector2I(mapGridNum.X, i), playAudio: false, noLimit: true, default, skipPlacementCheck: true);
			}
		}
	}

	private void MarkStateDirty()
	{
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			_networkRevision++;
		}
	}

	private void AdvancePulseResend(double delta)
	{
		if (!(_pulseResendTimer <= 0.0))
		{
			_pulseResendTimer = Math.Max(0.0, _pulseResendTimer - delta);
			_pulseResendAccumulator += delta;
			if (!(_pulseResendAccumulator < 0.1))
			{
				_pulseResendAccumulator %= 0.1;
				MarkStateDirty();
			}
		}
	}

	private Dictionary ExportEdgarState(bool includePulseState)
	{
		Dictionary dictionary = new Dictionary
		{
			["schemaVersion"] = 3,
			["action"] = (int)_action,
			["idleTimer"] = _idleTimer,
			["flightTimer"] = _flightTimer,
			["flightStartX"] = _flightStart.X,
			["flightStartY"] = _flightStart.Y,
			["flightEndX"] = _flightEnd.X,
			["flightEndY"] = _flightEnd.Y,
			["smashGridX"] = _smashGrid.X,
			["smashGridY"] = _smashGrid.Y,
			["idleMoveWaitTimer"] = _idleMoveWaitTimer,
			["idleMoveWaitDuration"] = _idleMoveWaitDuration,
			["idleRowMoveActive"] = _idleRowMoveActive,
			["idleRowMoveTimer"] = _idleRowMoveTimer,
			["idleRowMoveDuration"] = _idleRowMoveDuration,
			["idleRowMoveStartX"] = _idleRowMoveStart.X,
			["idleRowMoveStartY"] = _idleRowMoveStart.Y,
			["idleRowMoveEndX"] = _idleRowMoveEnd.X,
			["idleRowMoveEndY"] = _idleRowMoveEnd.Y,
			["idleRowMoveTargetRow"] = _idleRowMoveTargetRow,
			["skillPendingAfterRowMove"] = _skillPendingAfterRowMove,
			["lastSkill"] = _lastSkill,
			["skillRepeatCount"] = _skillRepeatCount,
			["animationFrame"] = (GodotObject.IsInstanceValid(sprite) ? sprite.frameIndex : (-1))
		};
		if (includePulseState)
		{
			dictionary["rev"] = _networkRevision;
			dictionary["pulseActionSequence"] = _pulseActionSequence;
			dictionary["pulseAffectedCells"] = SerializeGridPositions(_pulseAffectedCells);
			dictionary["pulseAffectedCharacterIds"] = new Array<int>(_pulseAffectedCharacterIds);
		}
		return dictionary;
	}

	private void ImportEdgarState(Dictionary data, bool includePulseState, bool enforceNetworkRevision)
	{
		if (data == null)
		{
			return;
		}
		bool flag = !IsNodeReady();
		int num = data.GetValueOrDefault("schemaVersion", 1).AsInt32();
		int num2 = (enforceNetworkRevision ? data.GetValueOrDefault("rev", 0).AsInt32() : _networkRevision);
		bool flag2 = !TowerDefenseManager.HasGameplayAuthority;
		if ((flag2 & enforceNetworkRevision) && _remoteNetworkStateInitialized && num2 <= _networkRevision)
		{
			if (includePulseState)
			{
				ImportPulseNetworkState(data);
			}
			return;
		}
		if (flag2 & enforceNetworkRevision)
		{
			_remoteNetworkStateInitialized = true;
		}
		if (enforceNetworkRevision)
		{
			_networkRevision = Math.Max(_networkRevision, num2);
		}
		EdgarAction edgarAction = (EdgarAction)data.GetValueOrDefault("action", (int)_action).AsInt32();
		bool flag3 = edgarAction != _action;
		_action = edgarAction;
		_idleTimer = data.GetValueOrDefault("idleTimer", _idleTimer).AsDouble();
		_flightTimer = data.GetValueOrDefault("flightTimer", _flightTimer).AsDouble();
		_flightStart = new Vector2(data.GetValueOrDefault("flightStartX", _flightStart.X).AsSingle(), data.GetValueOrDefault("flightStartY", _flightStart.Y).AsSingle());
		_flightEnd = new Vector2(data.GetValueOrDefault("flightEndX", _flightEnd.X).AsSingle(), data.GetValueOrDefault("flightEndY", _flightEnd.Y).AsSingle());
		_smashGrid = new Vector2I(data.GetValueOrDefault("smashGridX", _smashGrid.X).AsInt32(), data.GetValueOrDefault("smashGridY", _smashGrid.Y).AsInt32());
		_idleMoveWaitTimer = data.GetValueOrDefault("idleMoveWaitTimer", _idleMoveWaitTimer).AsDouble();
		_idleMoveWaitDuration = data.GetValueOrDefault("idleMoveWaitDuration", _idleMoveWaitDuration).AsDouble();
		_idleRowMoveActive = data.GetValueOrDefault("idleRowMoveActive", false).AsBool();
		_idleRowMoveTimer = data.GetValueOrDefault("idleRowMoveTimer", 0.0).AsDouble();
		_idleRowMoveDuration = data.GetValueOrDefault("idleRowMoveDuration", 0.0).AsDouble();
		_idleRowMoveStart = new Vector2(data.GetValueOrDefault("idleRowMoveStartX", _idleRowMoveStart.X).AsSingle(), data.GetValueOrDefault("idleRowMoveStartY", _idleRowMoveStart.Y).AsSingle());
		_idleRowMoveEnd = new Vector2(data.GetValueOrDefault("idleRowMoveEndX", _idleRowMoveEnd.X).AsSingle(), data.GetValueOrDefault("idleRowMoveEndY", _idleRowMoveEnd.Y).AsSingle());
		_idleRowMoveTargetRow = data.GetValueOrDefault("idleRowMoveTargetRow", _idleRowMoveTargetRow).AsInt32();
		_skillPendingAfterRowMove = data.GetValueOrDefault("skillPendingAfterRowMove", false).AsBool();
		if (num < 2)
		{
			_idleRowMoveActive = false;
			_skillPendingAfterRowMove = false;
			if (_action == EdgarAction.Idle && TowerDefenseManager.HasGameplayAuthority)
			{
				ScheduleNextIdleMove();
			}
		}
		_lastSkill = data.GetValueOrDefault("lastSkill", _lastSkill).AsInt32();
		int lastSkill = _lastSkill;
		if (lastSkill >= 2 && lastSkill <= 5)
		{
			int num3 = ((num < 3) ? 1 : Math.Max(1, _skillRepeatCount));
			_skillRepeatCount = Math.Clamp(data.GetValueOrDefault("skillRepeatCount", num3).AsInt32(), 1, 2);
		}
		else
		{
			_lastSkill = 0;
			_skillRepeatCount = 0;
		}
		_pendingPresentationFrame = data.GetValueOrDefault("animationFrame", _pendingPresentationFrame).AsInt32();
		if (includePulseState & flag2)
		{
			ImportPulseNetworkState(data);
		}
		_pendingPresentation |= flag3 | flag;
		if (_pendingPresentation && IsNodeReady())
		{
			ApplyPresentation();
		}
	}

	private void ImportPulseNetworkState(Dictionary data)
	{
		int num = data.GetValueOrDefault("pulseActionSequence", _remotePulseActionSequence).AsInt32();
		Array<Vector2I> pendingRemotePulseCells = DeserializeGridPositions(data.GetValueOrDefault("pulseAffectedCells", new Array<int>()));
		Array<int> pendingRemotePulseCharacterIds = DeserializeIntegerArray(data.GetValueOrDefault("pulseAffectedCharacterIds", new Array<int>()));
		if (!_remotePulseStateInitialized)
		{
			_remotePulseStateInitialized = true;
			_remotePulseActionSequence = num;
		}
		else if (num > _remotePulseActionSequence)
		{
			_remotePulseActionSequence = num;
			_pendingRemotePulseCharacterIds = pendingRemotePulseCharacterIds;
			_pendingRemotePulseCells = pendingRemotePulseCells;
			_pendingRemotePulseResolveTimer = 1.0;
			TryPlayPendingRemotePulseEffects(0.0);
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

	private static Array<int> DeserializeIntegerArray(Variant value)
	{
		Array<int> array = new Array<int>();
		foreach (Variant item in value.AsGodotArray())
		{
			array.Add(item.AsInt32());
		}
		return array;
	}

	private void ApplyPresentation()
	{
		_pendingPresentation = false;
		if (!die)
		{
			TowerDefenseCharacterInstance towerDefenseCharacterInstance = instance;
			if (towerDefenseCharacterInstance == null || !towerDefenseCharacterInstance.die)
			{
				if (_action == EdgarAction.Entering && TowerDefenseManager.HasGameplayAuthority)
				{
					base.Walk();
				}
				string text = _action switch
				{
					EdgarAction.Entering => "Walk", 
					EdgarAction.FlyToSmash => "Walk", 
					EdgarAction.Smash => "Smash", 
					EdgarAction.FlyHome => "Walk2", 
					EdgarAction.Fire => "Fire", 
					EdgarAction.Pulse => "Electronic", 
					EdgarAction.Summon => "Summon", 
					_ => "Idle", 
				};
				AdobeAnimateSprite adobeAnimateSprite = sprite;
				if (adobeAnimateSprite != null)
				{
					string clipName = text;
					EdgarAction action = _action;
					bool loop = (((uint)action <= 1u || action == EdgarAction.FlyHome || action == EdgarAction.Entering) ? true : false);
					adobeAnimateSprite.SetAnimation(clipName, loop);
				}
				if (GodotObject.IsInstanceValid(sprite) && _pendingPresentationFrame >= 0)
				{
					sprite.frameIndex = Mathf.Clamp(_pendingPresentationFrame, sprite.clipRange.X, Math.Max(sprite.clipRange.X, sprite.clipRange.Y - 1));
					sprite.elapsedTimer = 0.0;
				}
				_pendingPresentationFrame = -1;
				return;
			}
		}
		_pendingPresentationFrame = -1;
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		ImportEdgarState(data, includePulseState: true, enforceNetworkRevision: true);
	}

	public override Dictionary ExportNetworkSpawnState()
	{
		return ExportEdgarState(includePulseState: true);
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		return ExportEdgarState(includePulseState: true);
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		ImportEdgarState(data, includePulseState: true, enforceNetworkRevision: true);
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return _networkRevision;
	}

	public override bool IsNetworkSpecialMovementActive()
	{
		EdgarAction action = _action;
		if ((action != EdgarAction.FlyToSmash && action != EdgarAction.FlyHome) || 1 == 0)
		{
			return _idleRowMoveActive;
		}
		return true;
	}

	public override Dictionary ExportVariantSave()
	{
		return ExportEdgarState(includePulseState: false);
	}

	public override void ImportVariantSave(Dictionary data)
	{
		ImportEdgarState(data, includePulseState: false, enforceNetworkRevision: false);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(47)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldUpdateGridPos, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartNextSkill, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldBeginEntry, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdvanceEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartIdleRowMove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdvanceIdleRowMove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScheduleNextIdleMove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartSmashFlight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartFlight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "action", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "destination", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AdvanceFlight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReturnHome, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetHomeColumn, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveHomeGridPosition, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "mapSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "requestedRow", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "action", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "loop", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnterIdle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "resetTimer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveSmash, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveFireball, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolvePulse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectNextSkillByRoll, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "roll", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PickSkillByRoll, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "roll", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "excludedSkill", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSkillWeight, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "skill", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayLegacyPulseEffects, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "affectedCells", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryPlayPendingRemotePulseEffects, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveSummon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkStateDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdvancePulseResend, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportEdgarState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "includePulseState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportEdgarState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "includePulseState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "enforceNetworkRevision", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportPulseNetworkState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
			new MethodInfo(MethodName.DeserializeIntegerArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldUpdateGridPos && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldUpdateGridPos());
			return true;
		}
		if (method == MethodName.StartNextSkill && args.Count == 0)
		{
			StartNextSkill();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldBeginEntry && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldBeginEntry());
			return true;
		}
		if (method == MethodName.BeginEntry && args.Count == 0)
		{
			BeginEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceEntry && args.Count == 0)
		{
			AdvanceEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.StartIdleRowMove && args.Count == 0)
		{
			StartIdleRowMove();
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceIdleRowMove && args.Count == 1)
		{
			AdvanceIdleRowMove(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleNextIdleMove && args.Count == 0)
		{
			ScheduleNextIdleMove();
			ret = default;
			return true;
		}
		if (method == MethodName.StartSmashFlight && args.Count == 0)
		{
			StartSmashFlight();
			ret = default;
			return true;
		}
		if (method == MethodName.StartFlight && args.Count == 3)
		{
			StartFlight(VariantUtils.ConvertTo<EdgarAction>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceFlight && args.Count == 1)
		{
			AdvanceFlight(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReturnHome && args.Count == 0)
		{
			ReturnHome();
			ret = default;
			return true;
		}
		if (method == MethodName.GetHomeColumn && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetHomeColumn());
			return true;
		}
		if (method == MethodName.ResolveHomeGridPosition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ResolveHomeGridPosition(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.SetAction && args.Count == 3)
		{
			SetAction(VariantUtils.ConvertTo<EdgarAction>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnterIdle && args.Count == 1)
		{
			EnterIdle(VariantUtils.ConvertTo<bool>(in args[0]));
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
		if (method == MethodName.ResolveSmash && args.Count == 0)
		{
			ResolveSmash();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveFireball && args.Count == 0)
		{
			ResolveFireball();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePulse && args.Count == 0)
		{
			ResolvePulse();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectNextSkillByRoll && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(SelectNextSkillByRoll(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.PickSkillByRoll && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(PickSkillByRoll(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.GetSkillWeight && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetSkillWeight(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.PlayLegacyPulseEffects && args.Count == 1)
		{
			PlayLegacyPulseEffects(VariantUtils.ConvertToArray<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryPlayPendingRemotePulseEffects && args.Count == 1)
		{
			TryPlayPendingRemotePulseEffects(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveSummon && args.Count == 0)
		{
			ResolveSummon();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkStateDirty && args.Count == 0)
		{
			MarkStateDirty();
			ret = default;
			return true;
		}
		if (method == MethodName.AdvancePulseResend && args.Count == 1)
		{
			AdvancePulseResend(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExportEdgarState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportEdgarState(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.ImportEdgarState && args.Count == 3)
		{
			ImportEdgarState(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ImportPulseNetworkState && args.Count == 1)
		{
			ImportPulseNetworkState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName.DeserializeIntegerArray && args.Count == 1)
		{
			Array<int> array3 = DeserializeIntegerArray(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
			return true;
		}
		if (method == MethodName.ApplyPresentation && args.Count == 0)
		{
			ApplyPresentation();
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
		if (method == MethodName.ResolveHomeGridPosition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ResolveHomeGridPosition(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.PickSkillByRoll && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(PickSkillByRoll(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.GetSkillWeight && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetSkillWeight(VariantUtils.ConvertTo<int>(in args[0])));
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
		if (method == MethodName.DeserializeIntegerArray && args.Count == 1)
		{
			Array<int> array3 = DeserializeIntegerArray(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.ShouldUpdateGridPos)
		{
			return true;
		}
		if (method == MethodName.StartNextSkill)
		{
			return true;
		}
		if (method == MethodName.ShouldBeginEntry)
		{
			return true;
		}
		if (method == MethodName.BeginEntry)
		{
			return true;
		}
		if (method == MethodName.AdvanceEntry)
		{
			return true;
		}
		if (method == MethodName.StartIdleRowMove)
		{
			return true;
		}
		if (method == MethodName.AdvanceIdleRowMove)
		{
			return true;
		}
		if (method == MethodName.ScheduleNextIdleMove)
		{
			return true;
		}
		if (method == MethodName.StartSmashFlight)
		{
			return true;
		}
		if (method == MethodName.StartFlight)
		{
			return true;
		}
		if (method == MethodName.AdvanceFlight)
		{
			return true;
		}
		if (method == MethodName.ReturnHome)
		{
			return true;
		}
		if (method == MethodName.GetHomeColumn)
		{
			return true;
		}
		if (method == MethodName.ResolveHomeGridPosition)
		{
			return true;
		}
		if (method == MethodName.SetAction)
		{
			return true;
		}
		if (method == MethodName.EnterIdle)
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
		if (method == MethodName.ResolveSmash)
		{
			return true;
		}
		if (method == MethodName.ResolveFireball)
		{
			return true;
		}
		if (method == MethodName.ResolvePulse)
		{
			return true;
		}
		if (method == MethodName.SelectNextSkillByRoll)
		{
			return true;
		}
		if (method == MethodName.PickSkillByRoll)
		{
			return true;
		}
		if (method == MethodName.GetSkillWeight)
		{
			return true;
		}
		if (method == MethodName.PlayLegacyPulseEffects)
		{
			return true;
		}
		if (method == MethodName.TryPlayPendingRemotePulseEffects)
		{
			return true;
		}
		if (method == MethodName.ResolveSummon)
		{
			return true;
		}
		if (method == MethodName.MarkStateDirty)
		{
			return true;
		}
		if (method == MethodName.AdvancePulseResend)
		{
			return true;
		}
		if (method == MethodName.ExportEdgarState)
		{
			return true;
		}
		if (method == MethodName.ImportEdgarState)
		{
			return true;
		}
		if (method == MethodName.ImportPulseNetworkState)
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
		if (method == MethodName.DeserializeIntegerArray)
		{
			return true;
		}
		if (method == MethodName.ApplyPresentation)
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
		if (name == PropertyName._action)
		{
			_action = VariantUtils.ConvertTo<EdgarAction>(in value);
			return true;
		}
		if (name == PropertyName._idleTimer)
		{
			_idleTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._flightTimer)
		{
			_flightTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._flightStart)
		{
			_flightStart = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._flightEnd)
		{
			_flightEnd = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._smashGrid)
		{
			_smashGrid = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._idleMoveWaitTimer)
		{
			_idleMoveWaitTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._idleMoveWaitDuration)
		{
			_idleMoveWaitDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._idleRowMoveActive)
		{
			_idleRowMoveActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._idleRowMoveTimer)
		{
			_idleRowMoveTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._idleRowMoveDuration)
		{
			_idleRowMoveDuration = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._idleRowMoveStart)
		{
			_idleRowMoveStart = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._idleRowMoveEnd)
		{
			_idleRowMoveEnd = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._idleRowMoveTargetRow)
		{
			_idleRowMoveTargetRow = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._skillPendingAfterRowMove)
		{
			_skillPendingAfterRowMove = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._lastSkill)
		{
			_lastSkill = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._skillRepeatCount)
		{
			_skillRepeatCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._networkRevision)
		{
			_networkRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._pulseActionSequence)
		{
			_pulseActionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._remotePulseActionSequence)
		{
			_remotePulseActionSequence = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._remotePulseStateInitialized)
		{
			_remotePulseStateInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._remoteNetworkStateInitialized)
		{
			_remoteNetworkStateInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pulseResendTimer)
		{
			_pulseResendTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._pulseResendAccumulator)
		{
			_pulseResendAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._pendingRemotePulseCells)
		{
			_pendingRemotePulseCells = VariantUtils.ConvertToArray<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._pendingRemotePulseCharacterIds)
		{
			_pendingRemotePulseCharacterIds = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName._pendingRemotePulseResolveTimer)
		{
			_pendingRemotePulseResolveTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._speedDownEffect)
		{
			_speedDownEffect = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName._pendingPresentation)
		{
			_pendingPresentation = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingPresentationFrame)
		{
			_pendingPresentationFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._action)
		{
			value = VariantUtils.CreateFrom(in _action);
			return true;
		}
		if (name == PropertyName._idleTimer)
		{
			value = VariantUtils.CreateFrom(in _idleTimer);
			return true;
		}
		if (name == PropertyName._flightTimer)
		{
			value = VariantUtils.CreateFrom(in _flightTimer);
			return true;
		}
		if (name == PropertyName._flightStart)
		{
			value = VariantUtils.CreateFrom(in _flightStart);
			return true;
		}
		if (name == PropertyName._flightEnd)
		{
			value = VariantUtils.CreateFrom(in _flightEnd);
			return true;
		}
		if (name == PropertyName._smashGrid)
		{
			value = VariantUtils.CreateFrom(in _smashGrid);
			return true;
		}
		if (name == PropertyName._idleMoveWaitTimer)
		{
			value = VariantUtils.CreateFrom(in _idleMoveWaitTimer);
			return true;
		}
		if (name == PropertyName._idleMoveWaitDuration)
		{
			value = VariantUtils.CreateFrom(in _idleMoveWaitDuration);
			return true;
		}
		if (name == PropertyName._idleRowMoveActive)
		{
			value = VariantUtils.CreateFrom(in _idleRowMoveActive);
			return true;
		}
		if (name == PropertyName._idleRowMoveTimer)
		{
			value = VariantUtils.CreateFrom(in _idleRowMoveTimer);
			return true;
		}
		if (name == PropertyName._idleRowMoveDuration)
		{
			value = VariantUtils.CreateFrom(in _idleRowMoveDuration);
			return true;
		}
		if (name == PropertyName._idleRowMoveStart)
		{
			value = VariantUtils.CreateFrom(in _idleRowMoveStart);
			return true;
		}
		if (name == PropertyName._idleRowMoveEnd)
		{
			value = VariantUtils.CreateFrom(in _idleRowMoveEnd);
			return true;
		}
		if (name == PropertyName._idleRowMoveTargetRow)
		{
			value = VariantUtils.CreateFrom(in _idleRowMoveTargetRow);
			return true;
		}
		if (name == PropertyName._skillPendingAfterRowMove)
		{
			value = VariantUtils.CreateFrom(in _skillPendingAfterRowMove);
			return true;
		}
		if (name == PropertyName._lastSkill)
		{
			value = VariantUtils.CreateFrom(in _lastSkill);
			return true;
		}
		if (name == PropertyName._skillRepeatCount)
		{
			value = VariantUtils.CreateFrom(in _skillRepeatCount);
			return true;
		}
		if (name == PropertyName._networkRevision)
		{
			value = VariantUtils.CreateFrom(in _networkRevision);
			return true;
		}
		if (name == PropertyName._pulseActionSequence)
		{
			value = VariantUtils.CreateFrom(in _pulseActionSequence);
			return true;
		}
		if (name == PropertyName._remotePulseActionSequence)
		{
			value = VariantUtils.CreateFrom(in _remotePulseActionSequence);
			return true;
		}
		if (name == PropertyName._remotePulseStateInitialized)
		{
			value = VariantUtils.CreateFrom(in _remotePulseStateInitialized);
			return true;
		}
		if (name == PropertyName._remoteNetworkStateInitialized)
		{
			value = VariantUtils.CreateFrom(in _remoteNetworkStateInitialized);
			return true;
		}
		if (name == PropertyName._pulseResendTimer)
		{
			value = VariantUtils.CreateFrom(in _pulseResendTimer);
			return true;
		}
		if (name == PropertyName._pulseResendAccumulator)
		{
			value = VariantUtils.CreateFrom(in _pulseResendAccumulator);
			return true;
		}
		if (name == PropertyName._pulseAffectedCells)
		{
			value = VariantUtils.CreateFromArray(_pulseAffectedCells);
			return true;
		}
		if (name == PropertyName._pulseAffectedCharacterIds)
		{
			value = VariantUtils.CreateFromArray(_pulseAffectedCharacterIds);
			return true;
		}
		if (name == PropertyName._pendingRemotePulseCells)
		{
			value = VariantUtils.CreateFromArray(_pendingRemotePulseCells);
			return true;
		}
		if (name == PropertyName._pendingRemotePulseCharacterIds)
		{
			value = VariantUtils.CreateFromArray(_pendingRemotePulseCharacterIds);
			return true;
		}
		if (name == PropertyName._pendingRemotePulseResolveTimer)
		{
			value = VariantUtils.CreateFrom(in _pendingRemotePulseResolveTimer);
			return true;
		}
		if (name == PropertyName._speedDownEffect)
		{
			value = VariantUtils.CreateFrom(in _speedDownEffect);
			return true;
		}
		if (name == PropertyName._pendingPresentation)
		{
			value = VariantUtils.CreateFrom(in _pendingPresentation);
			return true;
		}
		if (name == PropertyName._pendingPresentationFrame)
		{
			value = VariantUtils.CreateFrom(in _pendingPresentationFrame);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._action, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._idleTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._flightTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._flightStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._flightEnd, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._smashGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._idleMoveWaitTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._idleMoveWaitDuration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._idleRowMoveActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._idleRowMoveTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._idleRowMoveDuration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._idleRowMoveStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._idleRowMoveEnd, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._idleRowMoveTargetRow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._skillPendingAfterRowMove, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastSkill, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._skillRepeatCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._networkRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pulseActionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._remotePulseActionSequence, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._remotePulseStateInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._remoteNetworkStateInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pulseResendTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pulseResendAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._pulseAffectedCells, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._pulseAffectedCharacterIds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._pendingRemotePulseCells, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._pendingRemotePulseCharacterIds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingRemotePulseResolveTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._speedDownEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingPresentation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pendingPresentationFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._action, Variant.From(in _action));
		info.AddProperty(PropertyName._idleTimer, Variant.From(in _idleTimer));
		info.AddProperty(PropertyName._flightTimer, Variant.From(in _flightTimer));
		info.AddProperty(PropertyName._flightStart, Variant.From(in _flightStart));
		info.AddProperty(PropertyName._flightEnd, Variant.From(in _flightEnd));
		info.AddProperty(PropertyName._smashGrid, Variant.From(in _smashGrid));
		info.AddProperty(PropertyName._idleMoveWaitTimer, Variant.From(in _idleMoveWaitTimer));
		info.AddProperty(PropertyName._idleMoveWaitDuration, Variant.From(in _idleMoveWaitDuration));
		info.AddProperty(PropertyName._idleRowMoveActive, Variant.From(in _idleRowMoveActive));
		info.AddProperty(PropertyName._idleRowMoveTimer, Variant.From(in _idleRowMoveTimer));
		info.AddProperty(PropertyName._idleRowMoveDuration, Variant.From(in _idleRowMoveDuration));
		info.AddProperty(PropertyName._idleRowMoveStart, Variant.From(in _idleRowMoveStart));
		info.AddProperty(PropertyName._idleRowMoveEnd, Variant.From(in _idleRowMoveEnd));
		info.AddProperty(PropertyName._idleRowMoveTargetRow, Variant.From(in _idleRowMoveTargetRow));
		info.AddProperty(PropertyName._skillPendingAfterRowMove, Variant.From(in _skillPendingAfterRowMove));
		info.AddProperty(PropertyName._lastSkill, Variant.From(in _lastSkill));
		info.AddProperty(PropertyName._skillRepeatCount, Variant.From(in _skillRepeatCount));
		info.AddProperty(PropertyName._networkRevision, Variant.From(in _networkRevision));
		info.AddProperty(PropertyName._pulseActionSequence, Variant.From(in _pulseActionSequence));
		info.AddProperty(PropertyName._remotePulseActionSequence, Variant.From(in _remotePulseActionSequence));
		info.AddProperty(PropertyName._remotePulseStateInitialized, Variant.From(in _remotePulseStateInitialized));
		info.AddProperty(PropertyName._remoteNetworkStateInitialized, Variant.From(in _remoteNetworkStateInitialized));
		info.AddProperty(PropertyName._pulseResendTimer, Variant.From(in _pulseResendTimer));
		info.AddProperty(PropertyName._pulseResendAccumulator, Variant.From(in _pulseResendAccumulator));
		info.AddProperty(PropertyName._pendingRemotePulseCells, Variant.CreateFrom(_pendingRemotePulseCells));
		info.AddProperty(PropertyName._pendingRemotePulseCharacterIds, Variant.CreateFrom(_pendingRemotePulseCharacterIds));
		info.AddProperty(PropertyName._pendingRemotePulseResolveTimer, Variant.From(in _pendingRemotePulseResolveTimer));
		info.AddProperty(PropertyName._speedDownEffect, Variant.From(in _speedDownEffect));
		info.AddProperty(PropertyName._pendingPresentation, Variant.From(in _pendingPresentation));
		info.AddProperty(PropertyName._pendingPresentationFrame, Variant.From(in _pendingPresentationFrame));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._action, out var value))
		{
			_action = value.As<EdgarAction>();
		}
		if (info.TryGetProperty(PropertyName._idleTimer, out var value2))
		{
			_idleTimer = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._flightTimer, out var value3))
		{
			_flightTimer = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._flightStart, out var value4))
		{
			_flightStart = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._flightEnd, out var value5))
		{
			_flightEnd = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._smashGrid, out var value6))
		{
			_smashGrid = value6.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._idleMoveWaitTimer, out var value7))
		{
			_idleMoveWaitTimer = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._idleMoveWaitDuration, out var value8))
		{
			_idleMoveWaitDuration = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._idleRowMoveActive, out var value9))
		{
			_idleRowMoveActive = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._idleRowMoveTimer, out var value10))
		{
			_idleRowMoveTimer = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName._idleRowMoveDuration, out var value11))
		{
			_idleRowMoveDuration = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName._idleRowMoveStart, out var value12))
		{
			_idleRowMoveStart = value12.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._idleRowMoveEnd, out var value13))
		{
			_idleRowMoveEnd = value13.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._idleRowMoveTargetRow, out var value14))
		{
			_idleRowMoveTargetRow = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName._skillPendingAfterRowMove, out var value15))
		{
			_skillPendingAfterRowMove = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._lastSkill, out var value16))
		{
			_lastSkill = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName._skillRepeatCount, out var value17))
		{
			_skillRepeatCount = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName._networkRevision, out var value18))
		{
			_networkRevision = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName._pulseActionSequence, out var value19))
		{
			_pulseActionSequence = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName._remotePulseActionSequence, out var value20))
		{
			_remotePulseActionSequence = value20.As<int>();
		}
		if (info.TryGetProperty(PropertyName._remotePulseStateInitialized, out var value21))
		{
			_remotePulseStateInitialized = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._remoteNetworkStateInitialized, out var value22))
		{
			_remoteNetworkStateInitialized = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pulseResendTimer, out var value23))
		{
			_pulseResendTimer = value23.As<double>();
		}
		if (info.TryGetProperty(PropertyName._pulseResendAccumulator, out var value24))
		{
			_pulseResendAccumulator = value24.As<double>();
		}
		if (info.TryGetProperty(PropertyName._pendingRemotePulseCells, out var value25))
		{
			_pendingRemotePulseCells = value25.AsGodotArray<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._pendingRemotePulseCharacterIds, out var value26))
		{
			_pendingRemotePulseCharacterIds = value26.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName._pendingRemotePulseResolveTimer, out var value27))
		{
			_pendingRemotePulseResolveTimer = value27.As<double>();
		}
		if (info.TryGetProperty(PropertyName._speedDownEffect, out var value28))
		{
			_speedDownEffect = value28.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName._pendingPresentation, out var value29))
		{
			_pendingPresentation = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingPresentationFrame, out var value30))
		{
			_pendingPresentationFrame = value30.As<int>();
		}
	}
}
