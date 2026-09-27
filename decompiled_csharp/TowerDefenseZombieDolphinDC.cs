using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter4/DolphinDC/Scene/TowerDefenseZombieDolphinDC.cs")]
public class TowerDefenseZombieDolphinDC : TowerDefenseZombie, IDancer, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName GetJackson = "GetJackson";

		public static readonly StringName SetJackson = "SetJackson";

		public static readonly StringName ApplyDolphinAnimationContract = "ApplyDolphinAnimationContract";

		public static readonly StringName ApplyDolphinRuntimeState = "ApplyDolphinRuntimeState";

		public static readonly StringName ApplyPendingNetworkSpawnState = "ApplyPendingNetworkSpawnState";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName Walk = "Walk";

		public static readonly StringName RunEntered = "RunEntered";

		public static readonly StringName RunProcessing = "RunProcessing";

		public static readonly StringName RunExited = "RunExited";

		public static readonly StringName JumpEntered = "JumpEntered";

		public static readonly StringName JumpProcessing = "JumpProcessing";

		public static readonly StringName JumpExited = "JumpExited";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName AnimeStarted = "AnimeStarted";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName Rise = "Rise";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ExportNetworkSpecialState = "ExportNetworkSpecialState";

		public new static readonly StringName ImportNetworkSpecialState = "ImportNetworkSpecialState";

		public new static readonly StringName GetNetworkSpecialStateRevision = "GetNetworkSpecialStateRevision";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public static readonly StringName OutJackson = "OutJackson";

		public new static readonly StringName CanBlock = "CanBlock";

		public new static readonly StringName BlockType = "BlockType";

		public new static readonly StringName Block = "Block";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName dolphin = "dolphin";

		public static readonly StringName _jackson = "_jackson";

		public static readonly StringName _pendingJacksonName = "_pendingJacksonName";

		public static readonly StringName _hasPendingNetworkSpawnState = "_hasPendingNetworkSpawnState";

		public static readonly StringName _pendingSpawnDolphin = "_pendingSpawnDolphin";

		public static readonly StringName _pendingSpawnInSwimPlay = "_pendingSpawnInSwimPlay";

		public static readonly StringName _dolphin = "_dolphin";

		public static readonly StringName jumpMove = "jumpMove";

		public static readonly StringName isJump = "isJump";

		public static readonly StringName isJumpInWater = "isJumpInWater";

		public static readonly StringName isBlock = "isBlock";

		public static readonly StringName audioPlay = "audioPlay";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private AttackComponent _attackComponent2;

	private TowerDefenseCharacter _jackson;

	private string _pendingJacksonName = "";

	private bool _hasPendingNetworkSpawnState;

	private bool _pendingSpawnDolphin = true;

	private bool _pendingSpawnInSwimPlay;

	private bool _dolphin = true;

	public bool jumpMove;

	public bool isJump;

	public bool isJumpInWater;

	public bool isBlock;

	public bool audioPlay;

	private StateHandle _jumpStateHandle;

	private StateHandle _runStateHandle;

	private bool _roleStateSignalsConnected;

	public bool dolphin
	{
		get
		{
			return _dolphin;
		}
		set
		{
			_dolphin = value;
			ApplyDolphinAnimationContract();
			if (IsNodeReady())
			{
				ApplyDolphinRuntimeState();
			}
		}
	}

	public TowerDefenseCharacter GetJackson()
	{
		return _jackson;
	}

	public void SetJackson(TowerDefenseCharacter value)
	{
		_jackson = value;
	}

	private void ApplyDolphinAnimationContract()
	{
		if (!_dolphin)
		{
			useAttackDps = true;
			idleAnimeClip = "Walk";
			walkAnimeClip = "Walk";
			attackAnimeClip = "Eat";
			dieAnimeClip = "Death";
		}
	}

	private void ApplyDolphinRuntimeState()
	{
		ApplyDolphinAnimationContract();
		if (!_dolphin)
		{
			AttackComponent attackComponent = base.attackComponent;
			if (attackComponent != null && attackComponent.Lifecycle == ComponentRuntimeLifecycle.Active && GodotObject.IsInstanceValid(spriteGroup) && GodotObject.IsInstanceValid(sprite))
			{
				base.attackComponent.attackType = "Eat";
				waterHeight = 48.0;
				groundHeight = 0.0 - waterHeight;
				z = groundHeight;
				spriteGroup.Position = new Vector2(spriteGroup.Position.X, 0f - (float)z);
				sprite.offset = new Vector2(-40f, -97f);
			}
		}
	}

	private void ApplyPendingNetworkSpawnState()
	{
		if (_hasPendingNetworkSpawnState)
		{
			_dolphin = _pendingSpawnDolphin;
			inSwimPlay = _pendingSpawnInSwimPlay;
			_hasPendingNetworkSpawnState = false;
		}
		ApplyDolphinRuntimeState();
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
		_jumpStateHandle = StateMachine?.GetStateById("zombie.dolphin_dc.jump");
		_runStateHandle = StateMachine?.GetStateById("zombie.dolphin_dc.run");
		StateHandle jumpStateHandle = _jumpStateHandle;
		if (jumpStateHandle != null && jumpStateHandle.IsValid)
		{
			StateHandle runStateHandle = _runStateHandle;
			if (runStateHandle != null && runStateHandle.IsValid)
			{
				_jumpStateHandle.Entered += JumpEntered;
				_jumpStateHandle.Exited += JumpExited;
				_jumpStateHandle.PhysicsProcessing += JumpProcessing;
				_runStateHandle.Entered += RunEntered;
				_runStateHandle.Exited += RunExited;
				_runStateHandle.PhysicsProcessing += RunProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			if (_jumpStateHandle != null)
			{
				_jumpStateHandle.Entered -= JumpEntered;
				_jumpStateHandle.Exited -= JumpExited;
				_jumpStateHandle.PhysicsProcessing -= JumpProcessing;
			}
			_jumpStateHandle = null;
			if (_runStateHandle != null)
			{
				_runStateHandle.Entered -= RunEntered;
				_runStateHandle.Exited -= RunExited;
				_runStateHandle.PhysicsProcessing -= RunProcessing;
			}
			_runStateHandle = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		if (IsQueuedForDeletion())
		{
			OutJackson();
		}
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
			sprite.OnAnimeStarted += AnimeStarted;
			ApplyPendingNetworkSpawnState();
			ConnectRoleStateSignals();
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (_pendingJacksonName == "")
		{
			return;
		}
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (GodotObject.IsInstanceValid(node2D))
		{
			TowerDefenseCharacter nodeOrNull = node2D.GetNodeOrNull<TowerDefenseCharacter>(new NodePath(_pendingJacksonName));
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				_pendingJacksonName = "";
				SetJackson(nodeOrNull);
			}
		}
	}

	public override void WalkEntered()
	{
		inSwimPlay = false;
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
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		if (!audioPlay && (double)GetGlobalPositionForPhysicsFrame(currentPhysicsFrame).X < TowerDefenseManager.Instance.GetMapGroundRight())
		{
			AudioManager.Instance.AudioPlay("DolphinAppears");
			audioPlay = true;
		}
		if (!dolphin && attackComponent.CanAttack() && GodotObject.IsInstanceValid(attackComponent.target) && attackComponent.target is TowerDefenseVase)
		{
			attackComponent.SmashAttackCell(((TowerDefenseZombieConfig)config).smashAttack);
		}
	}

	public override void Walk()
	{
		if (dolphin && inWater)
		{
			SendStateEvent("ToRun");
		}
		else
		{
			SendStateEvent("ToWalk");
		}
	}

	public void RunEntered()
	{
		if (inWater)
		{
			if (!inSwimPlay && inSwimAnimeClip != "")
			{
				sprite.SetAnimation(inSwimAnimeClip, loop: false, 0.2);
				sprite.AddAnimation("DolphinRun", 0.0);
				inSwimPlay = true;
			}
			else
			{
				sprite.SetAnimation("DolphinRun");
				inSwimPlay = false;
			}
		}
		else
		{
			sprite.SetAnimation(walkAnimeClip);
		}
		groundMoveComponent.SetAlive(true);
	}

	public void RunProcessing(double delta)
	{
		if (sprite.clip == inSwimAnimeClip)
		{
			sprite.timeScale = timeScale * walkSpeedScale * 2.0;
		}
		else
		{
			sprite.timeScale = timeScale * walkSpeedScale * 1.0;
		}
		if (!nearDie && !TowerDefenseManager.Instance.backZombie && sprite.clip == "DolphinRun" && _attackComponent2.HasAttackGridTargetCandidates() && _attackComponent2.CanAttack() && GodotObject.IsInstanceValid(_attackComponent2.target))
		{
			SendStateEvent("ToJump");
		}
	}

	public void RunExited()
	{
		groundMoveComponent.SetAlive(false);
	}

	public void JumpEntered()
	{
		shadowSprite.Visible = false;
		sprite.SetAnimation("DolphinJump", loop: false, 0.2);
		instance.collisionFlags = 0;
		instance.maskFlags = 0;
	}

	public void JumpProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
		if (!TowerDefenseManager.Instance.backZombie)
		{
			globalPositionForPhysicsFrame.X -= Scale.X * (float)delta * (float)sprite.timeScale * 16f;
			SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
		}
		if (isJump)
		{
			TowerDefenseCharacter towerDefenseCharacter = _attackComponent2?.target;
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && GodotObject.IsInstanceValid(towerDefenseCharacter.instance) && !towerDefenseCharacter.die && !towerDefenseCharacter.nearDie && !towerDefenseCharacter.isDestroy && towerDefenseCharacter.instance.height >= TowerDefenseEnum.CHARACTER_HEIGHT.TALL)
			{
				dolphin = false;
				globalPositionForPhysicsFrame.X = towerDefenseCharacter.GetLogicalGlobalPosition().X + 40f;
				SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
				gridPos = TowerDefenseManager.Instance.GetMapGridPos(globalPositionForPhysicsFrame);
				waterHeight = 48.0;
				groundHeight = 0.0 - waterHeight;
				z = groundHeight;
				spriteGroup.Position = new Vector2(spriteGroup.Position.X, 0f - (float)z);
				sprite.offset = new Vector2(-40f, sprite.offset.Y);
				isBlock = true;
				AudioManager.Instance.AudioPlay("Bonk");
				Walk();
			}
		}
	}

	public void JumpExited()
	{
		isJump = false;
		if (!inWater)
		{
			shadowSprite.Visible = !invisible;
		}
		instance.collisionFlags = 1;
		instance.maskFlags = 1;
	}

	public override void InWater()
	{
		base.InWater();
		if (!dolphin)
		{
			sprite.offset = new Vector2(24f, -97f);
		}
		useAttackDps = true;
	}

	public override void OutWater()
	{
		base.OutWater();
		CreateTween().TweenProperty(sprite, "offset", new Vector2(-40f, -92f), 0.25);
		if (dolphin && !IsRemoteNetworkReplica)
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			logicalGlobalPosition.X -= Scale.X * transformPoint.Scale.X * 40f;
			SetLogicalGlobalPosition(logicalGlobalPosition);
		}
		useAttackDps = !dolphin;
		waterHeight = 0.0;
	}

	public override void DieEntered()
	{
		base.DieEntered();
		sprite.offset = new Vector2(-40f, -92f);
		if (inWater)
		{
			waterHeight = 60.0;
			groundHeight = -60.0;
			z = -60.0;
			Tween tween = CreateTween();
			tween.SetParallel();
			tween.SetEase(Tween.EaseType.Out);
			tween.SetTrans(Tween.TransitionType.Cubic);
			tween.TweenProperty(this, "groundHeight", -100.0, 1.0);
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		switch (command)
		{
		case "hit":
			if (!die && !nearDie && !sprite.pause)
			{
				attackComponent.AttackExecute(((TowerDefenseZombieConfig)config).smashAttack);
			}
			break;
		case "audio":
			AudioManager.Instance.AudioPlay("DolphinreforeJumping");
			break;
		case "check":
			isJump = true;
			break;
		case "jumpOver":
			dolphin = false;
			break;
		}
	}

	public void AnimeStarted(string clip)
	{
		if (clip == "DolphinRun")
		{
			sprite.offset = new Vector2(24f, sprite.offset.Y);
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!(clip == "DolphinJump"))
		{
			if (clip == "JumpInWater")
			{
				if (!IsRemoteNetworkReplica && !TowerDefenseManager.Instance.backZombie)
				{
					Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
					logicalGlobalPosition.X -= Scale.X * transformPoint.Scale.X * 64f;
					SetLogicalGlobalPosition(logicalGlobalPosition);
				}
				sprite.offset = new Vector2(24f, sprite.offset.Y);
			}
		}
		else if (isJump)
		{
			jumpMove = true;
			waterHeight = 35.0;
			groundHeight = 0.0 - waterHeight;
			z = groundHeight;
			spriteGroup.Position = new Vector2(spriteGroup.Position.X, 0f - (float)z);
			sprite.offset = new Vector2(-40f, sprite.offset.Y);
			if (!IsRemoteNetworkReplica && !TowerDefenseManager.Instance.backZombie)
			{
				Vector2 logicalGlobalPosition2 = GetLogicalGlobalPosition();
				logicalGlobalPosition2.X -= Scale.X * transformPoint.Scale.X * 124f;
				SetLogicalGlobalPosition(logicalGlobalPosition2);
			}
			sprite.QueueRedraw();
			Walk();
		}
	}

	public override async void Rise(double duration = -1.0, double delay = 0.0, bool createDirt = true, bool changeState = true, double from = 150.0, bool emitRiseOver = true)
	{
		if (duration < 0.0)
		{
			duration = GD.RandRange(0.4, 0.6);
		}
		base.Rise(duration, delay, createDirt, changeState, from, emitRiseOver);
		TaskCompletionSource<bool> tcs;
		if (!changeState)
		{
			tcs = new TaskCompletionSource<bool>();
			OnRiseOver += RiseOverHandler;
			try
			{
				await tcs.Task;
			}
			finally
			{
				OnRiseOver -= RiseOverHandler;
			}
			Walk();
		}
		void RiseOverHandler()
		{
			tcs.TrySetResult(result: true);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary
		{
			{ "dolphin", dolphin },
			{ "jumpMove", jumpMove },
			{ "isJump", isJump },
			{ "isJumpInWater", isJumpInWater },
			{ "isBlock", isBlock }
		};
		if (GodotObject.IsInstanceValid(_jackson))
		{
			dictionary["jacksonNodeName"] = _jackson.Name;
		}
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		dolphin = data.GetValueOrDefault("dolphin", true).AsBool();
		jumpMove = data.GetValueOrDefault("jumpMove", false).AsBool();
		isJump = data.GetValueOrDefault("isJump", false).AsBool();
		isJumpInWater = data.GetValueOrDefault("isJumpInWater", false).AsBool();
		isBlock = data.GetValueOrDefault("isBlock", false).AsBool();
		_pendingJacksonName = data.GetValueOrDefault("jacksonNodeName", "").AsString();
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		_pendingSpawnDolphin = data.GetValueOrDefault("dolphin", dolphin).AsBool();
		_pendingSpawnInSwimPlay = data.GetValueOrDefault("in_swim_play", inSwimPlay).AsBool();
		_hasPendingNetworkSpawnState = true;
		if (IsNodeReady())
		{
			ApplyPendingNetworkSpawnState();
		}
	}

	public override Dictionary ExportNetworkSpecialState()
	{
		return new Dictionary
		{
			["rev"] = GetNetworkSpecialStateRevision(),
			["dolphin"] = dolphin,
			["inSwimPlay"] = inSwimPlay
		};
	}

	public override void ImportNetworkSpecialState(Dictionary data)
	{
		dolphin = data.GetValueOrDefault("dolphin", dolphin).AsBool();
		inSwimPlay = data.GetValueOrDefault("inSwimPlay", inSwimPlay).AsBool();
	}

	public override int GetNetworkSpecialStateRevision()
	{
		return (dolphin ? 1 : 0) | (inSwimPlay ? 2 : 0);
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		OutJackson();
	}

	public void OutJackson()
	{
		if (GodotObject.IsInstanceValid(_jackson) && _jackson is TowerDefenseZombie towerDefenseZombie && GodotObject.IsInstanceValid(towerDefenseZombie.instance) && !towerDefenseZombie.instance.hypnoses && _jackson is IJackson jackson)
		{
			jackson.RemoveDancer(this);
		}
		_jackson = null;
		_pendingJacksonName = "";
	}

	public override bool CanBlock()
	{
		return dolphin;
	}

	public override string BlockType()
	{
		return "Jump";
	}

	public override void Block(TowerDefenseCharacter target)
	{
		dolphin = false;
		waterHeight = 48.0;
		groundHeight = 0.0 - waterHeight;
		z = groundHeight;
		spriteGroup.Position = new Vector2(spriteGroup.Position.X, 0f - (float)z);
		sprite.offset = new Vector2(-40f, sprite.offset.Y);
		isBlock = true;
		AudioManager.Instance.AudioPlay("Bonk");
		Walk();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(37)
		{
			new MethodInfo(MethodName.GetJackson, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetJackson, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "value", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyDolphinAnimationContract, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyDolphinRuntimeState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyPendingNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.JumpExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeStarted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Rise, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "createDirt", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "changeState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "emitRiseOver", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
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
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OutJackson, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanBlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlockType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Block, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetJackson && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(GetJackson());
			return true;
		}
		if (method == MethodName.SetJackson && args.Count == 1)
		{
			SetJackson(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyDolphinAnimationContract && args.Count == 0)
		{
			ApplyDolphinAnimationContract();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyDolphinRuntimeState && args.Count == 0)
		{
			ApplyDolphinRuntimeState();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingNetworkSpawnState && args.Count == 0)
		{
			ApplyPendingNetworkSpawnState();
			ret = default;
			return true;
		}
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
		if (method == MethodName.JumpProcessing && args.Count == 1)
		{
			JumpProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.JumpExited && args.Count == 0)
		{
			JumpExited();
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
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeStarted && args.Count == 1)
		{
			AnimeStarted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Rise && args.Count == 6)
		{
			Rise(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]));
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
		if (method == MethodName.Hypnoses && args.Count == 3)
		{
			Hypnoses(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffHypnoses>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OutJackson && args.Count == 0)
		{
			OutJackson();
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
		if (method == MethodName.Block && args.Count == 1)
		{
			Block(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetJackson)
		{
			return true;
		}
		if (method == MethodName.SetJackson)
		{
			return true;
		}
		if (method == MethodName.ApplyDolphinAnimationContract)
		{
			return true;
		}
		if (method == MethodName.ApplyDolphinRuntimeState)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingNetworkSpawnState)
		{
			return true;
		}
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
		if (method == MethodName.RunEntered)
		{
			return true;
		}
		if (method == MethodName.RunProcessing)
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
		if (method == MethodName.JumpProcessing)
		{
			return true;
		}
		if (method == MethodName.JumpExited)
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
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.AnimeStarted)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.Rise)
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
		if (method == MethodName.Hypnoses)
		{
			return true;
		}
		if (method == MethodName.OutJackson)
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
		if (method == MethodName.Block)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.dolphin)
		{
			dolphin = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._jackson)
		{
			_jackson = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._pendingJacksonName)
		{
			_pendingJacksonName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._hasPendingNetworkSpawnState)
		{
			_hasPendingNetworkSpawnState = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingSpawnDolphin)
		{
			_pendingSpawnDolphin = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingSpawnInSwimPlay)
		{
			_pendingSpawnInSwimPlay = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dolphin)
		{
			_dolphin = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.isJumpInWater)
		{
			isJumpInWater = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isBlock)
		{
			isBlock = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.audioPlay)
		{
			audioPlay = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.dolphin)
		{
			value = VariantUtils.CreateFrom<bool>(dolphin);
			return true;
		}
		if (name == PropertyName._jackson)
		{
			value = VariantUtils.CreateFrom(in _jackson);
			return true;
		}
		if (name == PropertyName._pendingJacksonName)
		{
			value = VariantUtils.CreateFrom(in _pendingJacksonName);
			return true;
		}
		if (name == PropertyName._hasPendingNetworkSpawnState)
		{
			value = VariantUtils.CreateFrom(in _hasPendingNetworkSpawnState);
			return true;
		}
		if (name == PropertyName._pendingSpawnDolphin)
		{
			value = VariantUtils.CreateFrom(in _pendingSpawnDolphin);
			return true;
		}
		if (name == PropertyName._pendingSpawnInSwimPlay)
		{
			value = VariantUtils.CreateFrom(in _pendingSpawnInSwimPlay);
			return true;
		}
		if (name == PropertyName._dolphin)
		{
			value = VariantUtils.CreateFrom(in _dolphin);
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
		if (name == PropertyName.isJumpInWater)
		{
			value = VariantUtils.CreateFrom(in isJumpInWater);
			return true;
		}
		if (name == PropertyName.isBlock)
		{
			value = VariantUtils.CreateFrom(in isBlock);
			return true;
		}
		if (name == PropertyName.audioPlay)
		{
			value = VariantUtils.CreateFrom(in audioPlay);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._jackson, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingJacksonName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPendingNetworkSpawnState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingSpawnDolphin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pendingSpawnInSwimPlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.dolphin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dolphin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.jumpMove, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isJump, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isJumpInWater, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isBlock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.audioPlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.dolphin, Variant.From<bool>(dolphin));
		info.AddProperty(PropertyName._jackson, Variant.From(in _jackson));
		info.AddProperty(PropertyName._pendingJacksonName, Variant.From(in _pendingJacksonName));
		info.AddProperty(PropertyName._hasPendingNetworkSpawnState, Variant.From(in _hasPendingNetworkSpawnState));
		info.AddProperty(PropertyName._pendingSpawnDolphin, Variant.From(in _pendingSpawnDolphin));
		info.AddProperty(PropertyName._pendingSpawnInSwimPlay, Variant.From(in _pendingSpawnInSwimPlay));
		info.AddProperty(PropertyName._dolphin, Variant.From(in _dolphin));
		info.AddProperty(PropertyName.jumpMove, Variant.From(in jumpMove));
		info.AddProperty(PropertyName.isJump, Variant.From(in isJump));
		info.AddProperty(PropertyName.isJumpInWater, Variant.From(in isJumpInWater));
		info.AddProperty(PropertyName.isBlock, Variant.From(in isBlock));
		info.AddProperty(PropertyName.audioPlay, Variant.From(in audioPlay));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.dolphin, out var value))
		{
			dolphin = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._jackson, out var value2))
		{
			_jackson = value2.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._pendingJacksonName, out var value3))
		{
			_pendingJacksonName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._hasPendingNetworkSpawnState, out var value4))
		{
			_hasPendingNetworkSpawnState = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingSpawnDolphin, out var value5))
		{
			_pendingSpawnDolphin = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingSpawnInSwimPlay, out var value6))
		{
			_pendingSpawnInSwimPlay = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dolphin, out var value7))
		{
			_dolphin = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.jumpMove, out var value8))
		{
			jumpMove = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isJump, out var value9))
		{
			isJump = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isJumpInWater, out var value10))
		{
			isJumpInWater = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isBlock, out var value11))
		{
			isBlock = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.audioPlay, out var value12))
		{
			audioPlay = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value13))
		{
			_roleStateSignalsConnected = value13.As<bool>();
		}
	}
}
