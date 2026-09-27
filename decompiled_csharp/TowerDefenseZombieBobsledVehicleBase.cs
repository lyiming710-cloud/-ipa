using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter2/BobsledTeam/Scene/TowerDefenseZombieBobsledVehicleBase.cs")]
public abstract class TowerDefenseZombieBobsledVehicleBase : TowerDefenseZombie, INetworkBobsledOwner, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName TrySetNetworkBobsledPassenger = "TrySetNetworkBobsledPassenger";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName WalkExited = "WalkExited";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName HitpointsEmpty = "HitpointsEmpty";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public static readonly StringName CompleteEntryJump = "CompleteEntryJump";

		public static readonly StringName BeginBreak = "BeginBreak";

		public static readonly StringName GetRidingClip = "GetRidingClip";

		public static readonly StringName GetBreakClip = "GetBreakClip";

		public static readonly StringName OnPhaseChanged = "OnPhaseChanged";

		public static readonly StringName InitializeVehiclePhase = "InitializeVehiclePhase";

		public static readonly StringName ResolveCurrentSpeed = "ResolveCurrentSpeed";

		public static readonly StringName ProcessVehicleAttack = "ProcessVehicleAttack";

		public static readonly StringName ProcessBreaking = "ProcessBreaking";

		public static readonly StringName FinishBreak = "FinishBreak";

		public static readonly StringName SetPhase = "SetPhase";

		public static readonly StringName ApplyPhasePresentation = "ApplyPhasePresentation";

		public static readonly StringName MarkNetworkPhaseDirty = "MarkNetworkPhaseDirty";

		public static readonly StringName ProcessNetworkPhaseResync = "ProcessNetworkPhaseResync";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName ExportNetworkSpawnState = "ExportNetworkSpawnState";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public static readonly StringName TryAcceptNetworkPhaseState = "TryAcceptNetworkPhaseState";

		public static readonly StringName ImportBobsledState = "ImportBobsledState";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName slowSpeed = "slowSpeed";

		public static readonly StringName iceSpeed = "iceSpeed";

		public static readonly StringName requiresIce = "requiresIce";

		public static readonly StringName createsIce = "createsIce";

		public static readonly StringName crushesPlants = "crushesPlants";

		public static readonly StringName usesEntryChoreography = "usesEntryChoreography";

		public static readonly StringName IceCapMarker = "IceCapMarker";

		public static readonly StringName CurrentPhase = "CurrentPhase";

		public static readonly StringName CurrentSpeed = "CurrentSpeed";

		public static readonly StringName HasCapturedIceFront = "HasCapturedIceFront";

		public static readonly StringName CapturedIceFrontX = "CapturedIceFrontX";

		public static readonly StringName UsesBreakAnimationCompletion = "UsesBreakAnimationCompletion";

		public static readonly StringName _phaseInitialized = "_phaseInitialized";

		public static readonly StringName _iceBoostConsumed = "_iceBoostConsumed";

		public static readonly StringName _releaseCompleted = "_releaseCompleted";

		public static readonly StringName _breakVisualRemaining = "_breakVisualRemaining";

		public static readonly StringName _networkPhaseResyncTimer = "_networkPhaseResyncTimer";

		public static readonly StringName _networkSpecialStateRevision = "_networkSpecialStateRevision";

		public static readonly StringName _lastImportedNetworkSpecialStateRevision = "_lastImportedNetworkSpecialStateRevision";

		public static readonly StringName _hasImportedNetworkSpecialState = "_hasImportedNetworkSpecialState";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const double BrokenFrameDuration = 1.0 / 12.0;

	private const double NetworkPhaseResyncInterval = 0.5;

	private const float IceFrontEpsilon = 1f;

	private bool _phaseInitialized;

	private bool _iceBoostConsumed;

	private bool _releaseCompleted;

	private double _breakVisualRemaining;

	private double _networkPhaseResyncTimer;

	private int _networkSpecialStateRevision = 1;

	private int _lastImportedNetworkSpecialStateRevision;

	private bool _hasImportedNetworkSpecialState;

	[Export(PropertyHint.None, "")]
	public double slowSpeed { get; set; } = 30.0;

	[Export(PropertyHint.None, "")]
	public double iceSpeed { get; set; } = 60.0;

	[Export(PropertyHint.None, "")]
	public bool requiresIce { get; set; }

	[Export(PropertyHint.None, "")]
	public bool createsIce { get; set; }

	[Export(PropertyHint.None, "")]
	public bool crushesPlants { get; set; }

	[Export(PropertyHint.None, "")]
	public bool usesEntryChoreography { get; set; }

	protected BobsledTeamComponent TeamComponent { get; private set; }

	protected Marker2D IceCapMarker { get; private set; }

	public BobsledVehiclePhase CurrentPhase { get; private set; } = BobsledVehiclePhase.Riding;

	public double CurrentSpeed { get; private set; }

	public bool HasCapturedIceFront { get; private set; }

	public float CapturedIceFrontX { get; private set; }

	protected abstract bool UsesBreakAnimationCompletion { get; }

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			TeamComponent = componentManager?.GetRuntime<BobsledTeamComponent>();
			IceCapMarker = GetNodeOrNull<Marker2D>("%IceCapMarker");
			if (_phaseInitialized)
			{
				ApplyPhasePresentation();
			}
			else
			{
				InitializeVehiclePhase();
			}
		}
	}

	public bool TrySetNetworkBobsledPassenger(int slot, TowerDefenseCharacter passenger)
	{
		if (TeamComponent == null)
		{
			BobsledTeamComponent bobsledTeamComponent = (TeamComponent = componentManager?.GetRuntime<BobsledTeamComponent>());
		}
		if (TeamComponent == null)
		{
			return false;
		}
		if (TeamComponent.IsSlotReleased(slot))
		{
			TeamComponent.ResolveReleasedNetworkPassenger(slot, passenger);
			return true;
		}
		TeamComponent.SetNetworkPassenger(slot, passenger);
		return TeamComponent.GetPassenger(slot) == passenger;
	}

	public override void WalkEntered()
	{
		if (!isShow)
		{
			InitializeVehiclePhase();
			if (TeamComponent == null)
			{
				BobsledTeamComponent bobsledTeamComponent = (TeamComponent = componentManager?.GetRuntime<BobsledTeamComponent>());
			}
			TeamComponent?.EnsureAutoSpawnPassengers();
			if (GodotObject.IsInstanceValid(sprite) && CurrentPhase != BobsledVehiclePhase.Breaking)
			{
				sprite.SetAnimation(GetRidingClip());
			}
		}
	}

	public override void WalkExited()
	{
	}

	public override void WalkProcessing(double delta)
	{
		if (isShow)
		{
			return;
		}
		InitializeVehiclePhase();
		ProcessNetworkPhaseResync(delta);
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.timeScale = timeScale;
		}
		if (CurrentPhase == BobsledVehiclePhase.Breaking)
		{
			ProcessBreaking(delta);
		}
		else
		{
			if (CurrentPhase == BobsledVehiclePhase.Released || IsRemoteNetworkReplica || !TowerDefenseManager.HasGameplayAuthority || !GodotObject.IsInstanceValid(instance))
			{
				return;
			}
			TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
			if (!GodotObject.IsInstanceValid(towerDefenseManager))
			{
				return;
			}
			if (requiresIce && !towerDefenseManager.TryGetIceCapFrontX(gridPos.Y, out var _))
			{
				BeginBreak(BobsledBreakReason.IceEnded);
				return;
			}
			ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
			CurrentSpeed = ResolveCurrentSpeed(globalPositionForPhysicsFrame.X);
			if (!sprite.pause)
			{
				double num = (sprite.playBack ? 1.0 : (-1.0));
				double num2 = (((double)globalPositionForPhysicsFrame.X > towerDefenseManager.GetMapGroundRight()) ? 2.0 : 1.0);
				globalPositionForPhysicsFrame.X += (float)(CurrentSpeed * delta * timeScale * (double)transformPoint.Scale.X * (double)Scale.X * num * num2);
				SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
			}
			if (usesEntryChoreography && CurrentPhase == BobsledVehiclePhase.EnteringPush && (double)globalPositionForPhysicsFrame.X <= towerDefenseManager.GetMapGroundRight())
			{
				SetPhase(BobsledVehiclePhase.EnteringJump);
			}
			if (requiresIce && !instance.hypnoses && HasCapturedIceFront && globalPositionForPhysicsFrame.X <= CapturedIceFrontX + 1f)
			{
				BeginBreak(BobsledBreakReason.IceEnded);
				return;
			}
			if (createsIce && !instance.hypnoses)
			{
				Vector2 pos = (GodotObject.IsInstanceValid(IceCapMarker) ? GetLogicalGlobalPosition(IceCapMarker) : globalPositionForPhysicsFrame);
				towerDefenseManager.SetIceCapPos(gridPos.Y, pos);
			}
			if (crushesPlants)
			{
				ProcessVehicleAttack();
			}
		}
	}

	public override void DieEntered()
	{
		BeginBreak(BobsledBreakReason.Hitpoints);
	}

	public override void DieProcessing(double delta)
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.timeScale = timeScale;
		}
		ProcessBreaking(delta);
	}

	public override void HitpointsEmpty()
	{
		base.HitpointsEmpty();
		BeginBreak(BobsledBreakReason.Hitpoints);
	}

	public override void AnimeCompleted(string clip)
	{
		if (CurrentPhase == BobsledVehiclePhase.Breaking && clip == GetBreakClip())
		{
			if (UsesBreakAnimationCompletion)
			{
				FinishBreak();
			}
		}
		else
		{
			base.AnimeCompleted(clip);
		}
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		if (!IsHardControlImmune)
		{
			base.Hypnoses(time, canFliter, hypnosesConfig);
		}
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		(TeamComponent ?? componentManager?.GetRuntime<BobsledTeamComponent>())?.SynchronizePassengerHypnosis();
	}

	protected void CompleteEntryJump()
	{
		if (CurrentPhase == BobsledVehiclePhase.EnteringJump)
		{
			SetPhase(BobsledVehiclePhase.Riding);
		}
	}

	protected void BeginBreak(BobsledBreakReason reason)
	{
		BobsledVehiclePhase currentPhase = CurrentPhase;
		if ((uint)(currentPhase - 3) > 1u)
		{
			SetPhase(BobsledVehiclePhase.Breaking);
			HitBoxDestroy();
			AttackComponent attackComponent = base.attackComponent;
			if (attackComponent != null && !attackComponent.IsReleased)
			{
				base.attackComponent.SetAlive(alive: false);
			}
			if (reason == BobsledBreakReason.Spike)
			{
				die = true;
			}
			_breakVisualRemaining = (UsesBreakAnimationCompletion ? (1.0 / 0.0) : (1.0 / 12.0));
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.SetAnimation(GetBreakClip(), loop: false);
			}
		}
	}

	protected virtual string GetRidingClip()
	{
		return walkAnimeClip;
	}

	protected abstract string GetBreakClip();

	protected virtual void OnPhaseChanged(BobsledVehiclePhase phase)
	{
	}

	private void InitializeVehiclePhase()
	{
		if (!_phaseInitialized)
		{
			_phaseInitialized = true;
			BobsledVehiclePhase bobsledVehiclePhase = ((!usesEntryChoreography) ? BobsledVehiclePhase.Riding : BobsledVehiclePhase.EnteringPush);
			TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
			if (GodotObject.IsInstanceValid(towerDefenseManager))
			{
				HasCapturedIceFront = towerDefenseManager.TryGetIceCapFrontX(gridPos.Y, out var frontX);
				CapturedIceFrontX = frontX;
			}
			CurrentSpeed = ResolveCurrentSpeed(GetLogicalGlobalPosition().X);
			if (CurrentPhase == bobsledVehiclePhase)
			{
				ApplyPhasePresentation();
			}
			else
			{
				SetPhase(bobsledVehiclePhase);
			}
		}
	}

	private double ResolveCurrentSpeed(float currentX)
	{
		if (requiresIce)
		{
			return iceSpeed;
		}
		if (!_iceBoostConsumed && HasCapturedIceFront)
		{
			if (currentX > CapturedIceFrontX + 1f)
			{
				return iceSpeed;
			}
			_iceBoostConsumed = true;
		}
		return slowSpeed;
	}

	private void ProcessVehicleAttack()
	{
		AttackComponent attackComponent = base.attackComponent;
		if (attackComponent == null || attackComponent.IsReleased || !base.attackComponent.CanAttack() || !GodotObject.IsInstanceValid(base.attackComponent.target))
		{
			return;
		}
		if (base.attackComponent.target is TowerDefensePlant && GodotObject.IsInstanceValid(base.attackComponent.target.cell) && base.attackComponent.target.cell.HasSpike())
		{
			base.attackComponent.target = base.attackComponent.target.cell.GetSpike();
		}
		TowerDefenseCharacter target = base.attackComponent.target;
		if (!GodotObject.IsInstanceValid(target?.instance))
		{
			return;
		}
		if ((target.instance.physiqueTypeFlags & 0x10) == 0)
		{
			base.attackComponent.SmashAttackCell(((TowerDefenseZombieConfig)config).smashAttack);
			return;
		}
		if (target.instance.spikeHurt >= 0.0)
		{
			double spikeHurt = target.instance.spikeHurt;
			target.Hurt(100000.0, playSplatAudio: true, default, createDamagePart: true, spikeHurt);
		}
		BeginBreak(BobsledBreakReason.Spike);
	}

	private void ProcessBreaking(double delta)
	{
		if (CurrentPhase == BobsledVehiclePhase.Breaking && !UsesBreakAnimationCompletion && TowerDefenseManager.HasGameplayAuthority)
		{
			_breakVisualRemaining -= delta;
			if (_breakVisualRemaining <= 0.0)
			{
				FinishBreak();
			}
		}
	}

	private void FinishBreak()
	{
		if (!_releaseCompleted && TowerDefenseManager.HasGameplayAuthority)
		{
			_releaseCompleted = true;
			TeamComponent?.ReleaseAll();
			SetPhase(BobsledVehiclePhase.Released);
			Destroy();
		}
	}

	private void SetPhase(BobsledVehiclePhase phase)
	{
		if (CurrentPhase != phase)
		{
			CurrentPhase = phase;
			_networkPhaseResyncTimer = 0.0;
			MarkNetworkPhaseDirty();
			ApplyPhasePresentation();
		}
	}

	private void ApplyPhasePresentation()
	{
		OnPhaseChanged(CurrentPhase);
	}

	private void MarkNetworkPhaseDirty()
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

	private void ProcessNetworkPhaseResync(double delta)
	{
		if (usesEntryChoreography && Global.IsMultiplayerMode && TowerDefenseManager.HasGameplayAuthority && CurrentPhase != BobsledVehiclePhase.Released)
		{
			_networkPhaseResyncTimer += delta;
			if (!(_networkPhaseResyncTimer < 0.5))
			{
				_networkPhaseResyncTimer %= 0.5;
				MarkNetworkPhaseDirty();
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["bobsledPhase"] = (int)CurrentPhase,
			["bobsledCurrentSpeed"] = CurrentSpeed,
			["bobsledHasIceFront"] = HasCapturedIceFront,
			["bobsledIceFrontX"] = CapturedIceFrontX,
			["bobsledIceBoostConsumed"] = _iceBoostConsumed,
			["bobsledReleaseCompleted"] = _releaseCompleted,
			["bobsledPhaseInitialized"] = _phaseInitialized,
			["bobsledNetworkRevision"] = _networkSpecialStateRevision
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		if (data != null)
		{
			_networkSpecialStateRevision = Mathf.Max(1, data.GetValueOrDefault("bobsledNetworkRevision", 1).AsInt32());
			ImportBobsledState(data);
		}
	}

	public override Dictionary ExportNetworkSpawnState()
	{
		Dictionary dictionary = ExportVariantSave();
		dictionary["rev"] = _networkSpecialStateRevision;
		return dictionary;
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		if (TryAcceptNetworkPhaseState(data))
		{
			ImportBobsledState(data);
		}
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		return new Dictionary
		{
			["rev"] = _networkSpecialStateRevision,
			["bobsledPhase"] = (int)CurrentPhase
		};
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		if (TryAcceptNetworkPhaseState(data))
		{
			BobsledVehiclePhase bobsledVehiclePhase = (BobsledVehiclePhase)Mathf.Clamp(data.GetValueOrDefault("bobsledPhase", (int)CurrentPhase).AsInt32(), 0, 4);
			bool flag = CurrentPhase != bobsledVehiclePhase;
			CurrentPhase = bobsledVehiclePhase;
			_phaseInitialized = true;
			if (flag)
			{
				ApplyPhasePresentation();
			}
		}
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return _networkSpecialStateRevision;
	}

	private bool TryAcceptNetworkPhaseState(Dictionary data)
	{
		if (data == null)
		{
			return false;
		}
		int num = Mathf.Max(1, data.GetValueOrDefault("rev", _networkSpecialStateRevision).AsInt32());
		if (!TowerDefenseManager.HasGameplayAuthority)
		{
			if (_hasImportedNetworkSpecialState && num <= _lastImportedNetworkSpecialStateRevision)
			{
				return false;
			}
			_hasImportedNetworkSpecialState = true;
			_lastImportedNetworkSpecialStateRevision = num;
		}
		_networkSpecialStateRevision = Mathf.Max(_networkSpecialStateRevision, num);
		return true;
	}

	private void ImportBobsledState(Dictionary data)
	{
		if (data != null)
		{
			CurrentPhase = (BobsledVehiclePhase)Mathf.Clamp(data.GetValueOrDefault("bobsledPhase", (int)CurrentPhase).AsInt32(), 0, 4);
			CurrentSpeed = Mathf.Max(0.0, data.GetValueOrDefault("bobsledCurrentSpeed", CurrentSpeed).AsDouble());
			HasCapturedIceFront = data.GetValueOrDefault("bobsledHasIceFront", HasCapturedIceFront).AsBool();
			CapturedIceFrontX = (float)data.GetValueOrDefault("bobsledIceFrontX", CapturedIceFrontX).AsDouble();
			_iceBoostConsumed = data.GetValueOrDefault("bobsledIceBoostConsumed", _iceBoostConsumed).AsBool();
			_releaseCompleted = data.GetValueOrDefault("bobsledReleaseCompleted", _releaseCompleted).AsBool();
			_phaseInitialized = data.GetValueOrDefault("bobsledPhaseInitialized", true).AsBool();
			ApplyPhasePresentation();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(34)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrySetNetworkBobsledPassenger, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "passenger", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteEntryJump, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginBreak, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetRidingClip, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetBreakClip, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPhaseChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeVehiclePhase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveCurrentSpeed, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "currentX", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessVehicleAttack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessBreaking, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishBreak, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPhase, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPhasePresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkNetworkPhaseDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessNetworkPhaseResync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpawnState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportNetworkSpecialState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpecialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNetworkSpecialStateRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryAcceptNetworkPhaseState, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ImportBobsledState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.TrySetNetworkBobsledPassenger && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TrySetNetworkBobsledPassenger(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		if (method == MethodName.WalkEntered && args.Count == 0)
		{
			WalkEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkExited && args.Count == 0)
		{
			WalkExited();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.HitpointsEmpty && args.Count == 0)
		{
			HitpointsEmpty();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteEntryJump && args.Count == 0)
		{
			CompleteEntryJump();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginBreak && args.Count == 1)
		{
			BeginBreak(VariantUtils.ConvertTo<BobsledBreakReason>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetRidingClip && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetRidingClip());
			return true;
		}
		if (method == MethodName.GetBreakClip && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetBreakClip());
			return true;
		}
		if (method == MethodName.OnPhaseChanged && args.Count == 1)
		{
			OnPhaseChanged(VariantUtils.ConvertTo<BobsledVehiclePhase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeVehiclePhase && args.Count == 0)
		{
			InitializeVehiclePhase();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveCurrentSpeed && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ResolveCurrentSpeed(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.ProcessVehicleAttack && args.Count == 0)
		{
			ProcessVehicleAttack();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessBreaking && args.Count == 1)
		{
			ProcessBreaking(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishBreak && args.Count == 0)
		{
			FinishBreak();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPhase && args.Count == 1)
		{
			SetPhase(VariantUtils.ConvertTo<BobsledVehiclePhase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPhasePresentation && args.Count == 0)
		{
			ApplyPhasePresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkNetworkPhaseDirty && args.Count == 0)
		{
			MarkNetworkPhaseDirty();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessNetworkPhaseResync && args.Count == 1)
		{
			ProcessNetworkPhaseResync(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.ExportNetworkSpawnState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpawnState());
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName.TryAcceptNetworkPhaseState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryAcceptNetworkPhaseState(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.ImportBobsledState && args.Count == 1)
		{
			ImportBobsledState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.TrySetNetworkBobsledPassenger)
		{
			return true;
		}
		if (method == MethodName.WalkEntered)
		{
			return true;
		}
		if (method == MethodName.WalkExited)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
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
		if (method == MethodName.HitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.Hypnoses)
		{
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged)
		{
			return true;
		}
		if (method == MethodName.CompleteEntryJump)
		{
			return true;
		}
		if (method == MethodName.BeginBreak)
		{
			return true;
		}
		if (method == MethodName.GetRidingClip)
		{
			return true;
		}
		if (method == MethodName.GetBreakClip)
		{
			return true;
		}
		if (method == MethodName.OnPhaseChanged)
		{
			return true;
		}
		if (method == MethodName.InitializeVehiclePhase)
		{
			return true;
		}
		if (method == MethodName.ResolveCurrentSpeed)
		{
			return true;
		}
		if (method == MethodName.ProcessVehicleAttack)
		{
			return true;
		}
		if (method == MethodName.ProcessBreaking)
		{
			return true;
		}
		if (method == MethodName.FinishBreak)
		{
			return true;
		}
		if (method == MethodName.SetPhase)
		{
			return true;
		}
		if (method == MethodName.ApplyPhasePresentation)
		{
			return true;
		}
		if (method == MethodName.MarkNetworkPhaseDirty)
		{
			return true;
		}
		if (method == MethodName.ProcessNetworkPhaseResync)
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
		if (method == MethodName.ExportNetworkSpawnState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState)
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
		if (method == MethodName.TryAcceptNetworkPhaseState)
		{
			return true;
		}
		if (method == MethodName.ImportBobsledState)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.slowSpeed)
		{
			slowSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.iceSpeed)
		{
			iceSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.requiresIce)
		{
			requiresIce = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.createsIce)
		{
			createsIce = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.crushesPlants)
		{
			crushesPlants = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.usesEntryChoreography)
		{
			usesEntryChoreography = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IceCapMarker)
		{
			IceCapMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.CurrentPhase)
		{
			CurrentPhase = VariantUtils.ConvertTo<BobsledVehiclePhase>(in value);
			return true;
		}
		if (name == PropertyName.CurrentSpeed)
		{
			CurrentSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.HasCapturedIceFront)
		{
			HasCapturedIceFront = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.CapturedIceFrontX)
		{
			CapturedIceFrontX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._phaseInitialized)
		{
			_phaseInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._iceBoostConsumed)
		{
			_iceBoostConsumed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._releaseCompleted)
		{
			_releaseCompleted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._breakVisualRemaining)
		{
			_breakVisualRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._networkPhaseResyncTimer)
		{
			_networkPhaseResyncTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._networkSpecialStateRevision)
		{
			_networkSpecialStateRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lastImportedNetworkSpecialStateRevision)
		{
			_lastImportedNetworkSpecialStateRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._hasImportedNetworkSpecialState)
		{
			_hasImportedNetworkSpecialState = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		double from;
		if (name == PropertyName.slowSpeed)
		{
			from = slowSpeed;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.iceSpeed)
		{
			from = iceSpeed;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.requiresIce)
		{
			from2 = requiresIce;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.createsIce)
		{
			from2 = createsIce;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.crushesPlants)
		{
			from2 = crushesPlants;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.usesEntryChoreography)
		{
			from2 = usesEntryChoreography;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IceCapMarker)
		{
			value = VariantUtils.CreateFrom<Marker2D>(IceCapMarker);
			return true;
		}
		if (name == PropertyName.CurrentPhase)
		{
			value = VariantUtils.CreateFrom<BobsledVehiclePhase>(CurrentPhase);
			return true;
		}
		if (name == PropertyName.CurrentSpeed)
		{
			from = CurrentSpeed;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.HasCapturedIceFront)
		{
			from2 = HasCapturedIceFront;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CapturedIceFrontX)
		{
			value = VariantUtils.CreateFrom<float>(CapturedIceFrontX);
			return true;
		}
		if (name == PropertyName.UsesBreakAnimationCompletion)
		{
			from2 = UsesBreakAnimationCompletion;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._phaseInitialized)
		{
			value = VariantUtils.CreateFrom(in _phaseInitialized);
			return true;
		}
		if (name == PropertyName._iceBoostConsumed)
		{
			value = VariantUtils.CreateFrom(in _iceBoostConsumed);
			return true;
		}
		if (name == PropertyName._releaseCompleted)
		{
			value = VariantUtils.CreateFrom(in _releaseCompleted);
			return true;
		}
		if (name == PropertyName._breakVisualRemaining)
		{
			value = VariantUtils.CreateFrom(in _breakVisualRemaining);
			return true;
		}
		if (name == PropertyName._networkPhaseResyncTimer)
		{
			value = VariantUtils.CreateFrom(in _networkPhaseResyncTimer);
			return true;
		}
		if (name == PropertyName._networkSpecialStateRevision)
		{
			value = VariantUtils.CreateFrom(in _networkSpecialStateRevision);
			return true;
		}
		if (name == PropertyName._lastImportedNetworkSpecialStateRevision)
		{
			value = VariantUtils.CreateFrom(in _lastImportedNetworkSpecialStateRevision);
			return true;
		}
		if (name == PropertyName._hasImportedNetworkSpecialState)
		{
			value = VariantUtils.CreateFrom(in _hasImportedNetworkSpecialState);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.slowSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.iceSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.requiresIce, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.createsIce, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.crushesPlants, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.usesEntryChoreography, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.IceCapMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CurrentPhase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.CurrentSpeed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasCapturedIceFront, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.CapturedIceFrontX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._phaseInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._iceBoostConsumed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._releaseCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._breakVisualRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._networkPhaseResyncTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._networkSpecialStateRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastImportedNetworkSpecialStateRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasImportedNetworkSpecialState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UsesBreakAnimationCompletion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.slowSpeed, Variant.From<double>(slowSpeed));
		info.AddProperty(PropertyName.iceSpeed, Variant.From<double>(iceSpeed));
		info.AddProperty(PropertyName.requiresIce, Variant.From<bool>(requiresIce));
		info.AddProperty(PropertyName.createsIce, Variant.From<bool>(createsIce));
		info.AddProperty(PropertyName.crushesPlants, Variant.From<bool>(crushesPlants));
		info.AddProperty(PropertyName.usesEntryChoreography, Variant.From<bool>(usesEntryChoreography));
		info.AddProperty(PropertyName.IceCapMarker, Variant.From<Marker2D>(IceCapMarker));
		info.AddProperty(PropertyName.CurrentPhase, Variant.From<BobsledVehiclePhase>(CurrentPhase));
		info.AddProperty(PropertyName.CurrentSpeed, Variant.From<double>(CurrentSpeed));
		info.AddProperty(PropertyName.HasCapturedIceFront, Variant.From<bool>(HasCapturedIceFront));
		info.AddProperty(PropertyName.CapturedIceFrontX, Variant.From<float>(CapturedIceFrontX));
		info.AddProperty(PropertyName._phaseInitialized, Variant.From(in _phaseInitialized));
		info.AddProperty(PropertyName._iceBoostConsumed, Variant.From(in _iceBoostConsumed));
		info.AddProperty(PropertyName._releaseCompleted, Variant.From(in _releaseCompleted));
		info.AddProperty(PropertyName._breakVisualRemaining, Variant.From(in _breakVisualRemaining));
		info.AddProperty(PropertyName._networkPhaseResyncTimer, Variant.From(in _networkPhaseResyncTimer));
		info.AddProperty(PropertyName._networkSpecialStateRevision, Variant.From(in _networkSpecialStateRevision));
		info.AddProperty(PropertyName._lastImportedNetworkSpecialStateRevision, Variant.From(in _lastImportedNetworkSpecialStateRevision));
		info.AddProperty(PropertyName._hasImportedNetworkSpecialState, Variant.From(in _hasImportedNetworkSpecialState));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.slowSpeed, out var value))
		{
			slowSpeed = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.iceSpeed, out var value2))
		{
			iceSpeed = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.requiresIce, out var value3))
		{
			requiresIce = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.createsIce, out var value4))
		{
			createsIce = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.crushesPlants, out var value5))
		{
			crushesPlants = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.usesEntryChoreography, out var value6))
		{
			usesEntryChoreography = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IceCapMarker, out var value7))
		{
			IceCapMarker = value7.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.CurrentPhase, out var value8))
		{
			CurrentPhase = value8.As<BobsledVehiclePhase>();
		}
		if (info.TryGetProperty(PropertyName.CurrentSpeed, out var value9))
		{
			CurrentSpeed = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.HasCapturedIceFront, out var value10))
		{
			HasCapturedIceFront = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.CapturedIceFrontX, out var value11))
		{
			CapturedIceFrontX = value11.As<float>();
		}
		if (info.TryGetProperty(PropertyName._phaseInitialized, out var value12))
		{
			_phaseInitialized = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._iceBoostConsumed, out var value13))
		{
			_iceBoostConsumed = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._releaseCompleted, out var value14))
		{
			_releaseCompleted = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._breakVisualRemaining, out var value15))
		{
			_breakVisualRemaining = value15.As<double>();
		}
		if (info.TryGetProperty(PropertyName._networkPhaseResyncTimer, out var value16))
		{
			_networkPhaseResyncTimer = value16.As<double>();
		}
		if (info.TryGetProperty(PropertyName._networkSpecialStateRevision, out var value17))
		{
			_networkSpecialStateRevision = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastImportedNetworkSpecialStateRevision, out var value18))
		{
			_lastImportedNetworkSpecialStateRevision = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName._hasImportedNetworkSpecialState, out var value19))
		{
			_hasImportedNetworkSpecialState = value19.As<bool>();
		}
	}
}
