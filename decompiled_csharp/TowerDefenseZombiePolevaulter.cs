using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Polevaulter/Scene/TowerDefenseZombiePolevaulter.cs")]
public class TowerDefenseZombiePolevaulter : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public static readonly StringName RunEntered = "RunEntered";

		public static readonly StringName RunProcessing = "RunProcessing";

		public static readonly StringName ShouldCheckJumpTargetThisFrame = "ShouldCheckJumpTargetThisFrame";

		public static readonly StringName RunExited = "RunExited";

		public static readonly StringName JumpEntered = "JumpEntered";

		public static readonly StringName ApplyRemoteJumpStartFromSync = "ApplyRemoteJumpStartFromSync";

		public static readonly StringName JumpProcessing = "JumpProcessing";

		public static readonly StringName ResolveTallJumpBlocker = "ResolveTallJumpBlocker";

		public static readonly StringName IsTallJumpBlocker = "IsTallJumpBlocker";

		public static readonly StringName StopAtTallJumpBlocker = "StopAtTallJumpBlocker";

		public static readonly StringName JumpExited = "JumpExited";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName HitBoxDestroy = "HitBoxDestroy";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName CanBlock = "CanBlock";

		public new static readonly StringName BlockType = "BlockType";

		public static readonly StringName IsJumpVisualActive = "IsJumpVisualActive";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName IsNetworkSpecialMovementActive = "IsNetworkSpecialMovementActive";

		public new static readonly StringName OnRemoteNetworkAnimationStart = "OnRemoteNetworkAnimationStart";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName Block = "Block";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName GlobalPositionX = "GlobalPositionX";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName jumpOver = "jumpOver";

		public static readonly StringName jumpMove = "jumpMove";

		public static readonly StringName isJump = "isJump";

		public static readonly StringName isBlock = "isBlock";

		public static readonly StringName jumpTarget = "jumpTarget";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private StateHandle _runStateHandle;

	private StateHandle _jumpStateHandle;

	private bool _stateSignalsConnected;

	private AttackComponent _attackComponent2;

	private const int JumpTargetCheckStride = 2;

	public bool jumpOver;

	public bool jumpMove;

	public bool isJump;

	public bool isBlock;

	private TowerDefenseCharacter jumpTarget;

	private static bool IsRemoteClient
	{
		get
		{
			if (Global.IsMultiplayerMode)
			{
				return !MultiPlayerManager.IsHost;
			}
			return false;
		}
	}

	private float GlobalPositionX
	{
		get
		{
			return GetLogicalGlobalPosition().X;
		}
		set
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			logicalGlobalPosition.X = value;
			SetLogicalGlobalPosition(logicalGlobalPosition);
		}
	}

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
		_runStateHandle = StateMachine?.GetStateById("zombie.polevaulter.run");
		_jumpStateHandle = StateMachine?.GetStateById("zombie.polevaulter.jump");
		StateHandle runStateHandle = _runStateHandle;
		if (runStateHandle != null && runStateHandle.IsValid)
		{
			StateHandle jumpStateHandle = _jumpStateHandle;
			if (jumpStateHandle != null && jumpStateHandle.IsValid)
			{
				_runStateHandle.Entered += RunEntered;
				_runStateHandle.Exited += RunExited;
				_runStateHandle.PhysicsProcessing += RunProcessing;
				_jumpStateHandle.Entered += JumpEntered;
				_jumpStateHandle.Exited += JumpExited;
				_jumpStateHandle.PhysicsProcessing += JumpProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_runStateHandle != null)
			{
				_runStateHandle.Entered -= RunEntered;
				_runStateHandle.Exited -= RunExited;
				_runStateHandle.PhysicsProcessing -= RunProcessing;
			}
			_runStateHandle = null;
			if (_jumpStateHandle != null)
			{
				_jumpStateHandle.Entered -= JumpEntered;
				_jumpStateHandle.Exited -= JumpExited;
				_jumpStateHandle.PhysicsProcessing -= JumpProcessing;
			}
			_jumpStateHandle = null;
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
			_attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
			ConfigureWaterLineVisualLayers("Zombie_duckytube", "Zombie_duckytube1");
			ConnectStateSignals();
		}
	}

	public override void WalkEntered()
	{
		if (jumpMove)
		{
			jumpMove = false;
			if (inWater)
			{
				sprite.SetAnimation(swimAnimeClip);
			}
			else
			{
				sprite.SetAnimation(walkAnimeClip);
			}
		}
		else if (isBlock)
		{
			isBlock = false;
			if (inWater)
			{
				sprite.SetAnimation(swimAnimeClip);
			}
			else
			{
				sprite.SetAnimation(walkAnimeClip);
			}
		}
		else if (inWater)
		{
			sprite.SetAnimation(swimAnimeClip, loop: true, 0.2);
		}
		else
		{
			sprite.SetAnimation(walkAnimeClip, loop: true, 0.2);
		}
		ActivateGroundMovementAfterStateDelay(WalkStateHandle);
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
		if (jumpOver)
		{
			if (inWater)
			{
				sprite.timeScale = timeScale * walkSpeedScale * 1.0;
			}
			else
			{
				sprite.timeScale = timeScale * walkSpeedScale * 0.5;
			}
		}
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public virtual void RunEntered()
	{
		if (inWater)
		{
			sprite.SetAnimation("SwimRun", loop: true, 0.2);
		}
		else
		{
			sprite.SetAnimation("Run", loop: true, 0.2);
		}
		groundMoveComponent.SetAlive(true);
	}

	public virtual void RunProcessing(double delta)
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			sprite.timeScale = timeScale * walkSpeedScale * 3.0;
			if (!sprite.pause && !(sprite.timeScale <= 0.0) && !IsRemoteClient && !nearDie && !TowerDefenseManager.Instance.backZombie && ShouldCheckJumpTargetThisFrame() && _attackComponent2.HasAttackGridTargetCandidates() && _attackComponent2.CanAttack() && GodotObject.IsInstanceValid(_attackComponent2.target))
			{
				SendStateEvent("ToJump");
			}
		}
	}

	private bool ShouldCheckJumpTargetThisFrame()
	{
		return (ulong)((long)Engine.GetPhysicsFrames() + (long)randFreshIndex) % 2uL == 0;
	}

	public virtual void RunExited()
	{
		groundMoveComponent.SetAlive(false);
	}

	public virtual void JumpEntered()
	{
		shadowSprite.Visible = false;
		if (inWater)
		{
			sprite.SetAnimation("SwimJump", loop: false, 0.2);
		}
		else
		{
			sprite.SetAnimation("Jump", loop: false, 0.2);
		}
		instance.collisionFlags = 0;
		instance.maskFlags = 0;
		jumpTarget = _attackComponent2?.target;
	}

	public void ApplyRemoteJumpStartFromSync()
	{
		if (!IsRemoteClient)
		{
			return;
		}
		GroundMoveComponent groundMoveComponent = base.groundMoveComponent;
		if (groundMoveComponent != null && !groundMoveComponent.IsReleased)
		{
			base.groundMoveComponent.SetAlive(false);
		}
		if (!IsJumpVisualActive())
		{
			IStateMachineController stateMachine = StateMachine;
			if (stateMachine != null && stateMachine.IsInitialized)
			{
				SendStateEvent("ToJump");
			}
		}
	}

	public virtual void JumpProcessing(double delta)
	{
		sprite.timeScale = timeScale * 2.0;
		if (isJump && !IsRemoteClient)
		{
			TowerDefenseCharacter blocker = ResolveTallJumpBlocker();
			if (GodotObject.IsInstanceValid(blocker))
			{
				StopAtTallJumpBlocker(blocker);
			}
		}
	}

	private TowerDefenseCharacter ResolveTallJumpBlocker()
	{
		if (IsTallJumpBlocker(jumpTarget))
		{
			return jumpTarget;
		}
		AttackComponent attackComponent = _attackComponent2;
		if (attackComponent == null || attackComponent.IsReleased || TowerDefenseManager.Instance == null || !_attackComponent2.TryGetCheckAreaWorldRect(out var worldRect))
		{
			return null;
		}
		foreach (TowerDefenseCharacter characterTargetLineFromRectWithCollisionFlag in TowerDefenseManager.Instance.GetCharacterTargetLineFromRectWithCollisionFlags(this, 1, worldRect))
		{
			if (IsTallJumpBlocker(characterTargetLineFromRectWithCollisionFlag))
			{
				return characterTargetLineFromRectWithCollisionFlag;
			}
		}
		return null;
	}

	private static bool IsTallJumpBlocker(TowerDefenseCharacter candidate)
	{
		if (!GodotObject.IsInstanceValid(candidate) || !GodotObject.IsInstanceValid(candidate.instance))
		{
			return false;
		}
		if (candidate.instance.height < TowerDefenseEnum.CHARACTER_HEIGHT.TALL)
		{
			return false;
		}
		if (candidate is TowerDefensePlant && GodotObject.IsInstanceValid(candidate.cell) && GodotObject.IsInstanceValid(candidate.cell.characterLadder))
		{
			return false;
		}
		return true;
	}

	private void StopAtTallJumpBlocker(TowerDefenseCharacter blocker)
	{
		jumpOver = true;
		GlobalPositionX = blocker.GetLogicalGlobalPosition().X + 40f;
		gridPos = TowerDefenseManager.Instance.GetMapGridPos(GetLogicalGlobalPosition());
		isBlock = true;
		AudioManager.Instance.AudioPlay("Bonk");
		Walk();
	}

	public virtual void JumpExited()
	{
		isJump = false;
		jumpTarget = null;
		if (!inWater)
		{
			shadowSprite.Visible = !invisible;
		}
		instance.collisionFlags = 1;
		instance.maskFlags = 1;
	}

	public override void Walk()
	{
		if (jumpOver)
		{
			SendStateEvent("ToWalk");
		}
		else
		{
			SendStateEvent("ToRun");
		}
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		if (damangePointName == "Head")
		{
			sprite.SetFliters(new Array { "Zombie_polevaulter_innerarm_lower", "Zombie_polevaulter_innerarm_upper", "Zombie_polevaulter_innerhand", "Zombie_polevaulter_pole", "Zombie_polevaulter_pole2", "Zombie_polevaulter_pole 复制", "Zombie_polevaulter_pole2 复制" }, open: false);
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		switch (command)
		{
		case "audio":
			AudioManager.Instance.AudioPlay("Polevault");
			break;
		case "check":
			isJump = true;
			break;
		case "jumpOver":
			jumpOver = true;
			break;
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!(clip == "Jump"))
		{
			if (clip == "SwimJump" && !(clip != sprite.clip))
			{
				jumpMove = true;
				if (!TowerDefenseManager.Instance.backZombie)
				{
					GlobalPositionX -= (float)((double)(Scale.X * transformPoint.Scale.X) * 148.0);
				}
				gridPos = TowerDefenseManager.Instance.GetMapGridPos(GetLogicalGlobalPosition());
				sprite.QueueRedraw();
				Walk();
			}
		}
		else if (!(clip != sprite.clip))
		{
			jumpMove = true;
			if (!TowerDefenseManager.Instance.backZombie)
			{
				GlobalPositionX -= (float)((double)(Scale.X * transformPoint.Scale.X) * 148.0);
			}
			gridPos = TowerDefenseManager.Instance.GetMapGridPos(GetLogicalGlobalPosition());
			sprite.QueueRedraw();
			Walk();
		}
	}

	public override void HitBoxDestroy()
	{
		base.HitBoxDestroy();
		_attackComponent2?.SetCheckAreaEnabled(enabled: false);
	}

	public override void InWater()
	{
		base.InWater();
		sprite.SetFliters(new Array { "Zombie_whitewater", "Zombie_whitewater1" }, open: true);
	}

	public override void OutWater()
	{
		base.OutWater();
		sprite.SetFliters(new Array { "Zombie_whitewater", "Zombie_whitewater1" }, open: false);
	}

	public override bool CanBlock()
	{
		return !jumpOver;
	}

	public override string BlockType()
	{
		return "Jump";
	}

	private bool IsJumpVisualActive()
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			if (!(sprite.clip == "Jump"))
			{
				return sprite.clip == "SwimJump";
			}
			return true;
		}
		return false;
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		return new Dictionary
		{
			["rev"] = GetNetworkSpecialStateRevision(),
			["jumpOver"] = jumpOver,
			["jumpMove"] = jumpMove,
			["isJump"] = isJump,
			["isBlock"] = isBlock
		};
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		jumpOver = data.GetValueOrDefault("jumpOver", jumpOver).AsBool();
		jumpMove = data.GetValueOrDefault("jumpMove", jumpMove).AsBool();
		isJump = data.GetValueOrDefault("isJump", isJump).AsBool();
		isBlock = data.GetValueOrDefault("isBlock", isBlock).AsBool();
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return (int)((jumpOver ? 1u : 0u) | (uint)(jumpMove ? 2 : 0) | (uint)(isJump ? 4 : 0)) | (isBlock ? 8 : 0);
	}

	public override bool IsNetworkSpecialMovementActive()
	{
		if (!isJump)
		{
			return IsJumpVisualActive();
		}
		return true;
	}

	public override void OnRemoteNetworkAnimationStart(string clipName)
	{
		if (clipName == "Jump" || clipName == "SwimJump")
		{
			ApplyRemoteJumpStartFromSync();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "jumpOver", jumpOver },
			{ "jumpMove", jumpMove },
			{ "isJump", isJump },
			{ "isBlock", isBlock }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		jumpOver = (bool)data.GetValueOrDefault("jumpOver", false);
		jumpMove = (bool)data.GetValueOrDefault("jumpMove", false);
		isJump = (bool)data.GetValueOrDefault("isJump", false);
		isBlock = (bool)data.GetValueOrDefault("isBlock", false);
	}

	public override void Block(TowerDefenseCharacter target)
	{
		jumpOver = true;
		isBlock = true;
		isJump = false;
		jumpTarget = null;
		AudioManager.Instance.AudioPlay("Bonk");
		Walk();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(37)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldCheckJumpTargetThisFrame, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyRemoteJumpStartFromSync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveTallJumpBlocker, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsTallJumpBlocker, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "candidate", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.StopAtTallJumpBlocker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "blocker", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.JumpExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.HitBoxDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanBlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlockType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsJumpVisualActive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Block, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunEntered && args.Count == 0)
		{
			RunEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.RunProcessing && args.Count == 1)
		{
			RunProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldCheckJumpTargetThisFrame && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldCheckJumpTargetThisFrame());
			return true;
		}
		if (method == MethodName.RunExited && args.Count == 0)
		{
			RunExited();
			ret = default;
			return true;
		}
		if (method == MethodName.JumpEntered && args.Count == 0)
		{
			JumpEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRemoteJumpStartFromSync && args.Count == 0)
		{
			ApplyRemoteJumpStartFromSync();
			ret = default;
			return true;
		}
		if (method == MethodName.JumpProcessing && args.Count == 1)
		{
			JumpProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveTallJumpBlocker && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(ResolveTallJumpBlocker());
			return true;
		}
		if (method == MethodName.IsTallJumpBlocker && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTallJumpBlocker(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.StopAtTallJumpBlocker && args.Count == 1)
		{
			StopAtTallJumpBlocker(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.JumpExited && args.Count == 0)
		{
			JumpExited();
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.HitBoxDestroy && args.Count == 0)
		{
			HitBoxDestroy();
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
		if (method == MethodName.IsJumpVisualActive && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsJumpVisualActive());
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
		if (method == MethodName.IsTallJumpBlocker && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTallJumpBlocker(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
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
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.RunEntered)
		{
			return true;
		}
		if (method == MethodName.RunProcessing)
		{
			return true;
		}
		if (method == MethodName.ShouldCheckJumpTargetThisFrame)
		{
			return true;
		}
		if (method == MethodName.RunExited)
		{
			return true;
		}
		if (method == MethodName.JumpEntered)
		{
			return true;
		}
		if (method == MethodName.ApplyRemoteJumpStartFromSync)
		{
			return true;
		}
		if (method == MethodName.JumpProcessing)
		{
			return true;
		}
		if (method == MethodName.ResolveTallJumpBlocker)
		{
			return true;
		}
		if (method == MethodName.IsTallJumpBlocker)
		{
			return true;
		}
		if (method == MethodName.StopAtTallJumpBlocker)
		{
			return true;
		}
		if (method == MethodName.JumpExited)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
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
		if (method == MethodName.HitBoxDestroy)
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
		if (method == MethodName.CanBlock)
		{
			return true;
		}
		if (method == MethodName.BlockType)
		{
			return true;
		}
		if (method == MethodName.IsJumpVisualActive)
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
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
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
		if (name == PropertyName.GlobalPositionX)
		{
			GlobalPositionX = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.jumpOver)
		{
			jumpOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.jumpMove)
		{
			jumpMove = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isJump)
		{
			isJump = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isBlock)
		{
			isBlock = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.jumpTarget)
		{
			jumpTarget = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.GlobalPositionX)
		{
			value = VariantUtils.CreateFrom<float>(GlobalPositionX);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName.jumpOver)
		{
			value = VariantUtils.CreateFrom(in jumpOver);
			return true;
		}
		if (name == PropertyName.jumpMove)
		{
			value = VariantUtils.CreateFrom(in jumpMove);
			return true;
		}
		if (name == PropertyName.isJump)
		{
			value = VariantUtils.CreateFrom(in isJump);
			return true;
		}
		if (name == PropertyName.isBlock)
		{
			value = VariantUtils.CreateFrom(in isBlock);
			return true;
		}
		if (name == PropertyName.jumpTarget)
		{
			value = VariantUtils.CreateFrom(in jumpTarget);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.jumpOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.jumpMove, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isJump, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isBlock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.jumpTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.GlobalPositionX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.GlobalPositionX, Variant.From<float>(GlobalPositionX));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName.jumpOver, Variant.From(in jumpOver));
		info.AddProperty(PropertyName.jumpMove, Variant.From(in jumpMove));
		info.AddProperty(PropertyName.isJump, Variant.From(in isJump));
		info.AddProperty(PropertyName.isBlock, Variant.From(in isBlock));
		info.AddProperty(PropertyName.jumpTarget, Variant.From(in jumpTarget));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.GlobalPositionX, out var value))
		{
			GlobalPositionX = value.As<float>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value2))
		{
			_stateSignalsConnected = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.jumpOver, out var value3))
		{
			jumpOver = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.jumpMove, out var value4))
		{
			jumpMove = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isJump, out var value5))
		{
			isJump = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isBlock, out var value6))
		{
			isBlock = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.jumpTarget, out var value7))
		{
			jumpTarget = value7.As<TowerDefenseCharacter>();
		}
	}
}
