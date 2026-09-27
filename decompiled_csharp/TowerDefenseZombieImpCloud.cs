using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter8/ImpCloud/Scene/TowerDefenseZombieImpCloud.cs")]
public class TowerDefenseZombieImpCloud : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName FlyEntered = "FlyEntered";

		public static readonly StringName FlyProcessing = "FlyProcessing";

		public static readonly StringName FlyExited = "FlyExited";

		public static readonly StringName FlyAttackEntered = "FlyAttackEntered";

		public static readonly StringName FlyAttackProcessing = "FlyAttackProcessing";

		public static readonly StringName FlyAttackExited = "FlyAttackExited";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName Attack = "Attack";

		public new static readonly StringName Blow = "Blow";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName ThrowSpikeBall = "ThrowSpikeBall";

		public static readonly StringName CreateSpikeBallVisual = "CreateSpikeBallVisual";

		public static readonly StringName OnSpikeBallLanded = "OnSpikeBallLanded";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName DieEntered = "DieEntered";

		public static readonly StringName HideCloudLayers = "HideCloudLayers";

		public static readonly StringName CreateFleeingCloud = "CreateFleeingCloud";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName speed = "speed";

		public static readonly StringName _shootTimer = "_shootTimer";

		public static readonly StringName _isShooting = "_isShooting";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const float SHOOT_INTERVAL = 8f;

	private const float SPIKE_BALL_FALL_TIME = 0.6f;

	private static PackedScene _IMITATER_CLOUD;

	private static PackedScene _CLOUD_SPRITE;

	private const string ARM_BONE_TEXTURE = "uid://bdvdk4frcvxu7";

	public double speed = 30.0;

	private double _shootTimer;

	private bool _isShooting;

	private StateHandle _flyStateHandle;

	private StateHandle _flyAttackStateHandle;

	private bool _roleStateSignalsConnected;

	private static PackedScene IMITATER_CLOUD => _IMITATER_CLOUD ?? (_IMITATER_CLOUD = GD.Load<PackedScene>("uid://djvfnrjg7vtqn"));

	private static PackedScene CLOUD_SPRITE => _CLOUD_SPRITE ?? (_CLOUD_SPRITE = GD.Load<PackedScene>("uid://csds4e7ibk87a"));

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
		_flyStateHandle = StateMachine?.GetStateById("zombie.imp_cloud.fly");
		_flyAttackStateHandle = StateMachine?.GetStateById("zombie.imp_cloud.fly_attack");
		StateHandle flyStateHandle = _flyStateHandle;
		if (flyStateHandle != null && flyStateHandle.IsValid)
		{
			StateHandle flyAttackStateHandle = _flyAttackStateHandle;
			if (flyAttackStateHandle != null && flyAttackStateHandle.IsValid)
			{
				_flyStateHandle.Entered += FlyEntered;
				_flyStateHandle.Exited += FlyExited;
				_flyStateHandle.PhysicsProcessing += FlyProcessing;
				_flyAttackStateHandle.Entered += FlyAttackEntered;
				_flyAttackStateHandle.Exited += FlyAttackExited;
				_flyAttackStateHandle.PhysicsProcessing += FlyAttackProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			if (_flyStateHandle != null)
			{
				_flyStateHandle.Entered -= FlyEntered;
				_flyStateHandle.Exited -= FlyExited;
				_flyStateHandle.PhysicsProcessing -= FlyProcessing;
			}
			_flyStateHandle = null;
			if (_flyAttackStateHandle != null)
			{
				_flyAttackStateHandle.Entered -= FlyAttackEntered;
				_flyAttackStateHandle.Exited -= FlyAttackExited;
				_flyAttackStateHandle.PhysicsProcessing -= FlyAttackProcessing;
			}
			_flyAttackStateHandle = null;
			_roleStateSignalsConnected = false;
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
			ConnectRoleStateSignals();
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || !TowerDefenseManager.Instance.IsGameRunning() || !inGame || !IsInsideComponentBattlefield || die || nearDie || _isShooting || sprite.pause)
		{
			return;
		}
		if (_shootTimer < 8.0)
		{
			_shootTimer += delta * timeScale;
			return;
		}
		_shootTimer = 0.0;
		if (!attackComponent.CanAttack())
		{
			_isShooting = true;
			sprite.SetAnimation("Shooting", loop: false, 0.2);
		}
	}

	public void FlyEntered()
	{
		_isShooting = false;
		sprite.SetAnimation("Idle", loop: true, 0.2);
	}

	public void FlyProcessing(double delta)
	{
		if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
		{
			sprite.timeScale = timeScale;
			if (!sprite.pause && !_isShooting)
			{
				ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
				Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
				globalPositionForPhysicsFrame.X -= (float)(speed * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)Scale.X * (double)((!sprite.playBack) ? 1 : (-1)));
				SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
			}
			if (!sprite.pause && !_isShooting && attackComponent.CanAttack())
			{
				SendStateEvent("ToFlyAttack");
			}
		}
	}

	public void FlyExited()
	{
	}

	public void FlyAttackEntered()
	{
		sprite.SetAnimation("Eat", loop: true, 0.2);
		ActivateAttackAfterStateDelay(_flyAttackStateHandle);
	}

	public void FlyAttackProcessing(double delta)
	{
		if (!attackComponent.CanAttack())
		{
			SendStateEvent("ToFly");
		}
		else if (startAttack && !nearDie && !sprite.pause && sprite.timeScale > 0.0 && useAttackDps)
		{
			attackComponent.AttackDpsExecute(delta, ((TowerDefenseZombieConfig)config).attack);
		}
		sprite.timeScale = timeScale * 2.0;
	}

	public void FlyAttackExited()
	{
	}

	public override void Walk()
	{
		SendStateEvent("ToFly");
	}

	public override void Attack()
	{
		SendStateEvent("ToFlyAttack");
	}

	public override void Blow()
	{
		BlowBack(1.0, 1.0);
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == "fire")
		{
			ThrowSpikeBall();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "Shooting")
		{
			_isShooting = false;
			if (!die)
			{
				Walk();
			}
		}
	}

	private void ThrowSpikeBall()
	{
		if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
		{
			Node2D ball = CreateSpikeBallVisual();
			ball.GlobalPosition = GetLogicalGlobalPosition(sprite) + new Vector2(0f, -20f);
			TowerDefenseGroundItemBase.characterNode.AddChild(ball, forceReadableName: false, InternalMode.Disabled);
			float y = TowerDefenseManager.GetMapCellPlantPos(gridPos).Y;
			bool cachedInGame = inGame;
			bool cachedHypnoses = GodotObject.IsInstanceValid(instance) && instance.hypnoses;
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(ball, "global_position:y", y, 0.6000000238418579);
			tween.TweenCallback(Callable.From(() =>
			{
				OnSpikeBallLanded(ball, cachedInGame, cachedHypnoses);
			}));
		}
	}

	private Node2D CreateSpikeBallVisual()
	{
		AdobeAnimateData flashAnimeData = sprite.flashAnimeData;
		if (!GodotObject.IsInstanceValid(flashAnimeData) || !flashAnimeData.mediaDictionary.TryGetValue("Zombie_cloud_ball.png", out var value))
		{
			return new Node2D();
		}
		int num = value.AsInt32();
		AdobeAnimateRuntimeDefinition orBuild = AdobeAnimateDefinitionCache.GetOrBuild(flashAnimeData);
		if (orBuild == null || num < 0 || num >= orBuild.MediaRects.Length)
		{
			return new Node2D();
		}
		Vector2 size = orBuild.MediaRects[num].Size;
		Array item = new Array
		{
			num,
			Transform2D.Identity.Translated(-size / 2f),
			Colors.White
		};
		return new AdobeAnimatePart
		{
			flashAnimeData = flashAnimeData,
			mediaReplace = sprite.CreateActiveMediaReplaceSnapshotForRender(),
			offset = Vector2.Zero,
			elementList = new Array<Array> { item }
		};
	}

	private void OnSpikeBallLanded(Node2D ball, bool cachedInGame, bool cachedHypnoses)
	{
		if (!GodotObject.IsInstanceValid(ball))
		{
			return;
		}
		Vector2 globalPosition = ball.GlobalPosition;
		Vector2I mapGridPos = TowerDefenseManager.Instance.GetMapGridPos(globalPosition);
		ball.QueueFree();
		if (!TowerDefenseManager.Instance.IsGameRunning() || !cachedInGame)
		{
			return;
		}
		TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(IMITATER_CLOUD, mapGridPos);
		towerDefenseEffectParticlesOnce.GlobalPosition = globalPosition;
		TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, InternalMode.Disabled);
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("ZombieImpDiggerSpike");
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(mapGridPos);
		if (GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			towerDefenseCharacter.SetLogicalGlobalPosition(globalPosition);
			if (cachedHypnoses)
			{
				towerDefenseCharacter.Hypnoses();
			}
		}
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		if (damangePointName == "Arm")
		{
			sprite.SetFliter("lower2", open: false);
			sprite.SetAtlasReplace("Zombie_cloud_arm1.png", "uid://bdvdk4frcvxu7");
		}
	}

	public override void DieEntered()
	{
		base.DieEntered();
		CreateFleeingCloud();
		HideCloudLayers();
	}

	private void HideCloudLayers()
	{
		sprite.SetFliters(new Array { "front" }, open: false);
	}

	private void CreateFleeingCloud()
	{
		if (!Engine.IsEditorHint())
		{
			Node node = CLOUD_SPRITE.Instantiate(PackedScene.GenEditState.Disabled);
			TowerDefenseGroundItemBase.characterNode.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			Node2D cloudNode2D = (Node2D)node;
			cloudNode2D.GlobalPosition = GetLogicalGlobalPosition(sprite);
			cloudNode2D.Scale = transformPoint.Scale * Scale;
			if (node is AdobeAnimateSpriteBase adobeAnimateSpriteBase)
			{
				adobeAnimateSpriteBase.SetAnimation("Flee", loop: true, 0.2);
				adobeAnimateSpriteBase.SetFliters(new Array
				{
					"Zombie_imp_innerarm_upper", "Zombie_imp_innerarm_lower", "Zombie_imp_innerleg_foot", "Zombie_imp_innerleg_lower", "Zombie_imp_innerleg_upper", "Zombie_imp_body2", "Zombie_imp_body1", "Zombie_imp_outerleg_foot", "Zombie_imp_outerleg_lower", "Zombie_imp_outerleg_upper",
					"Zombie_imp_outerarm_upper", "Zombie_outerarm_lower", "anim_head1", "anim_head2", "glasses", "ball", "ball2", "spike1", "spike2", "spike3",
					"spike4", "spike5", "spike6", "spike7", "spike8", "spike9"
				}, open: false);
			}
			Tween tween = GetTree().CreateTween();
			float num = ((!((((double)GD.Randf() > 0.5) ? 1f : (-1f)) > 0f)) ? ((float)(TowerDefenseManager.Instance.GetMapGroundLeft() - 200.0)) : ((float)(TowerDefenseManager.Instance.GetMapGroundRight() + 200.0)));
			float num2 = cloudNode2D.GlobalPosition.Y - 500f;
			double duration = 2.5;
			tween.SetParallel();
			tween.TweenProperty(cloudNode2D, "global_position:x", num, duration).SetEase(Tween.EaseType.In);
			tween.TweenProperty(cloudNode2D, "global_position:y", num2, duration).SetEase(Tween.EaseType.In);
			tween.Chain().TweenCallback(Callable.From(() =>
			{
				cloudNode2D.QueueFree();
			}));
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "speed", speed },
			{ "shootTimer", _shootTimer }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		speed = data.GetValueOrDefault("speed", 30.0).AsDouble();
		_shootTimer = data.GetValueOrDefault("shootTimer", 0.0).AsDouble();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(25)
		{
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlyEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlyProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlyExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlyAttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlyAttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlyAttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Attack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Blow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ThrowSpikeBall, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSpikeBallVisual, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSpikeBallLanded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "ball", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "cachedInGame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "cachedHypnoses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideCloudLayers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateFleeingCloud, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.FlyEntered && args.Count == 0)
		{
			FlyEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.FlyProcessing && args.Count == 1)
		{
			FlyProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlyExited && args.Count == 0)
		{
			FlyExited();
			ret = default;
			return true;
		}
		if (method == MethodName.FlyAttackEntered && args.Count == 0)
		{
			FlyAttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.FlyAttackProcessing && args.Count == 1)
		{
			FlyAttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlyAttackExited && args.Count == 0)
		{
			FlyAttackExited();
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.Attack && args.Count == 0)
		{
			Attack();
			ret = default;
			return true;
		}
		if (method == MethodName.Blow && args.Count == 0)
		{
			Blow();
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
		if (method == MethodName.ThrowSpikeBall && args.Count == 0)
		{
			ThrowSpikeBall();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSpikeBallVisual && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node2D>(CreateSpikeBallVisual());
			return true;
		}
		if (method == MethodName.OnSpikeBallLanded && args.Count == 3)
		{
			OnSpikeBallLanded(VariantUtils.ConvertTo<Node2D>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.HideCloudLayers && args.Count == 0)
		{
			HideCloudLayers();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateFleeingCloud && args.Count == 0)
		{
			CreateFleeingCloud();
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
		if (method == MethodName.FlyEntered)
		{
			return true;
		}
		if (method == MethodName.FlyProcessing)
		{
			return true;
		}
		if (method == MethodName.FlyExited)
		{
			return true;
		}
		if (method == MethodName.FlyAttackEntered)
		{
			return true;
		}
		if (method == MethodName.FlyAttackProcessing)
		{
			return true;
		}
		if (method == MethodName.FlyAttackExited)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.Attack)
		{
			return true;
		}
		if (method == MethodName.Blow)
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
		if (method == MethodName.ThrowSpikeBall)
		{
			return true;
		}
		if (method == MethodName.CreateSpikeBallVisual)
		{
			return true;
		}
		if (method == MethodName.OnSpikeBallLanded)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.HideCloudLayers)
		{
			return true;
		}
		if (method == MethodName.CreateFleeingCloud)
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
		if (name == PropertyName.speed)
		{
			speed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._shootTimer)
		{
			_shootTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._isShooting)
		{
			_isShooting = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.speed)
		{
			value = VariantUtils.CreateFrom(in speed);
			return true;
		}
		if (name == PropertyName._shootTimer)
		{
			value = VariantUtils.CreateFrom(in _shootTimer);
			return true;
		}
		if (name == PropertyName._isShooting)
		{
			value = VariantUtils.CreateFrom(in _isShooting);
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
			new PropertyInfo(Variant.Type.Float, PropertyName.speed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._shootTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isShooting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName._shootTimer, Variant.From(in _shootTimer));
		info.AddProperty(PropertyName._isShooting, Variant.From(in _isShooting));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.speed, out var value))
		{
			speed = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._shootTimer, out var value2))
		{
			_shootTimer = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._isShooting, out var value3))
		{
			_isShooting = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value4))
		{
			_roleStateSignalsConnected = value4.As<bool>();
		}
	}
}
