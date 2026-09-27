using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter5/PogoDancer/Scene/TowerDefenseZombiePogoDancer.cs")]
public class TowerDefenseZombiePogoDancer : TowerDefenseZombie, IDancer, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName GetJackson = "GetJackson";

		public static readonly StringName SetJackson = "SetJackson";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName Walk = "Walk";

		public static readonly StringName PogoEntered = "PogoEntered";

		public static readonly StringName PogoProcessing = "PogoProcessing";

		public static readonly StringName PogoExited = "PogoExited";

		public static readonly StringName DanceEntered = "DanceEntered";

		public static readonly StringName DanceProcessing = "DanceProcessing";

		public static readonly StringName DanceExited = "DanceExited";

		public new static readonly StringName WalkEntered = "WalkEntered";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName Hypnoses = "Hypnoses";

		public static readonly StringName OutJackson = "OutJackson";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public static readonly StringName Land = "Land";

		public new static readonly StringName CanBlock = "CanBlock";

		public new static readonly StringName BlockType = "BlockType";

		public new static readonly StringName Block = "Block";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName hasPogo = "hasPogo";

		public static readonly StringName isJump = "isJump";

		public static readonly StringName _hasPogo = "_hasPogo";

		public static readonly StringName pogoPlant = "pogoPlant";

		public static readonly StringName jumpToPos = "jumpToPos";

		public static readonly StringName jumpWait = "jumpWait";

		public static readonly StringName walkTime = "walkTime";

		public static readonly StringName danceTime = "danceTime";

		public static readonly StringName jackson = "jackson";

		public static readonly StringName _pendingJacksonName = "_pendingJacksonName";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private AttackComponent _attackComponent2;

	public bool isJump;

	private bool _hasPogo = true;

	public bool pogoPlant;

	public double jumpToPos;

	public int jumpWait = 1;

	public int walkTime = 2;

	public int danceTime = 3;

	public TowerDefenseZombie jackson;

	private string _pendingJacksonName = "";

	private StateHandle _danceStateHandle;

	private StateHandle _pogoStateHandle;

	private bool _roleStateSignalsConnected;

	public bool hasPogo
	{
		get
		{
			return _hasPogo;
		}
		set
		{
			_hasPogo = value;
			if (!_hasPogo)
			{
				idleAnimeClip = "Walk";
			}
		}
	}

	public TowerDefenseCharacter GetJackson()
	{
		return jackson;
	}

	public void SetJackson(TowerDefenseCharacter value)
	{
		jackson = value as TowerDefenseZombie;
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		z = data.GetValueOrDefault("spawn_z", z).AsDouble();
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
		_danceStateHandle = StateMachine?.GetStateById("zombie.pogo_dancer.dance");
		_pogoStateHandle = StateMachine?.GetStateById("zombie.pogo_dancer.pogo");
		StateHandle danceStateHandle = _danceStateHandle;
		if (danceStateHandle != null && danceStateHandle.IsValid)
		{
			StateHandle pogoStateHandle = _pogoStateHandle;
			if (pogoStateHandle != null && pogoStateHandle.IsValid)
			{
				_danceStateHandle.Entered += DanceEntered;
				_danceStateHandle.Exited += DanceExited;
				_danceStateHandle.PhysicsProcessing += DanceProcessing;
				_pogoStateHandle.Entered += PogoEntered;
				_pogoStateHandle.Exited += PogoExited;
				_pogoStateHandle.PhysicsProcessing += PogoProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			if (_danceStateHandle != null)
			{
				_danceStateHandle.Entered -= DanceEntered;
				_danceStateHandle.Exited -= DanceExited;
				_danceStateHandle.PhysicsProcessing -= DanceProcessing;
			}
			_danceStateHandle = null;
			if (_pogoStateHandle != null)
			{
				_pogoStateHandle.Entered -= PogoEntered;
				_pogoStateHandle.Exited -= PogoExited;
				_pogoStateHandle.PhysicsProcessing -= PogoProcessing;
			}
			_pogoStateHandle = null;
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
			OnLand += Land;
			_attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
			ySpeed = -300.0;
			ConnectRoleStateSignals();
		}
	}

	public override void Walk()
	{
		if (hasPogo)
		{
			SendStateEvent("ToPogo");
		}
		else
		{
			SendStateEvent("ToWalk");
		}
	}

	public void PogoEntered()
	{
		sprite.SetAnimation("Pogo", loop: true, 0.2);
	}

	public void PogoProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
		if (!pogoPlant)
		{
			if (!sprite.pause)
			{
				double num = (((double)globalPositionForPhysicsFrame.X > groundRight) ? 2.0 : 1.0);
				globalPositionForPhysicsFrame.X -= (float)(30.0 * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)Scale.X * num * (double)((!sprite.playBack) ? 1 : (-1)));
				SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
			}
			if (!sprite.pause && attackComponent.CanAttack())
			{
				pogoPlant = true;
				jumpToPos = globalPositionForPhysicsFrame.X - TowerDefenseManager.Instance.GetMapGridSize().X * Scale.X - 10f * Scale.X;
			}
		}
		else if (isJump && _attackComponent2.HasAttackGridTargetCandidates() && _attackComponent2.CanAttack() && GodotObject.IsInstanceValid(_attackComponent2.target) && _attackComponent2.target.instance.height >= TowerDefenseEnum.CHARACTER_HEIGHT.TALL)
		{
			instance.ArmorDelete("Pogo");
			hasPogo = false;
			isJump = false;
			AudioManager.Instance.AudioPlay("Bonk");
			Walk();
		}
	}

	public void PogoExited()
	{
	}

	public void DanceEntered()
	{
		danceTime = 3;
		sprite.Scale = new Vector2(-1f, sprite.Scale.Y);
		sprite.SetAnimation("ArmRise", loop: true, 0.2);
	}

	public void DanceProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		if (!sprite.pause && attackComponent.CanAttack())
		{
			Attack();
		}
	}

	public void DanceExited()
	{
		sprite.Scale = new Vector2(1f, sprite.Scale.Y);
	}

	public override void WalkEntered()
	{
		base.WalkEntered();
		sprite.Scale = new Vector2(1f, sprite.Scale.Y);
		walkTime = 2;
	}

	public override void WalkProcessing(double delta)
	{
		if (GodotObject.IsInstanceValid(jackson))
		{
			groundMoveComponent.SetAlive(jackson.groundMoveComponent?.Alive ?? false);
		}
		else
		{
			groundMoveComponent.SetAlive(true);
		}
		base.WalkProcessing(delta);
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if ((clip == "Walk" || clip == "ArmRise") && clip != sprite.clip)
		{
			return;
		}
		if (!(clip == "Walk"))
		{
			if (!(clip == "ArmRise"))
			{
				return;
			}
			sprite.Scale = new Vector2(0f - sprite.Scale.X, sprite.Scale.Y);
			danceTime--;
			if (!die && !nearDie)
			{
				if (danceTime > 0)
				{
					return;
				}
				if (GodotObject.IsInstanceValid(jackson))
				{
					if (!GodotObject.IsInstanceValid(jackson))
					{
						return;
					}
					GroundMoveComponent groundMoveComponent = jackson.groundMoveComponent;
					if (groundMoveComponent != null && groundMoveComponent.Alive)
					{
						return;
					}
				}
				Walk();
			}
			else
			{
				OutJackson();
				Die();
			}
		}
		else
		{
			if (TowerDefenseManager.Instance.currentControl != null && !TowerDefenseManager.Instance.currentControl.isGameRunning)
			{
				return;
			}
			walkTime--;
			if (!die && !nearDie)
			{
				if (walkTime > 0)
				{
					return;
				}
				if (GodotObject.IsInstanceValid(jackson))
				{
					if (!GodotObject.IsInstanceValid(jackson))
					{
						return;
					}
					GroundMoveComponent groundMoveComponent2 = jackson.groundMoveComponent;
					if (groundMoveComponent2 != null && groundMoveComponent2.Alive)
					{
						return;
					}
				}
				SendStateEvent("ToDance");
			}
			else
			{
				OutJackson();
				Die();
			}
		}
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Pogo")
		{
			instance.unUseBuffFlags = 0;
			isJump = false;
			hasPogo = false;
			GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
			if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
			{
				base.groundHeightComponent.handleWaterHeight = true;
			}
			if (inWater)
			{
				groundHeight = 0.0 - waterHeight;
			}
			Walk();
		}
	}

	public override void Hypnoses(double time = -1.0, bool canFliter = true, TowerDefenseCharacterBuffHypnoses hypnosesConfig = null)
	{
		base.Hypnoses(time, canFliter, hypnosesConfig);
		OutJackson();
	}

	public void OutJackson()
	{
		if (GodotObject.IsInstanceValid(this.jackson))
		{
			if (!this.jackson.instance.hypnoses && this.jackson is IJackson jackson)
			{
				jackson.RemoveDancer(this);
			}
			this.jackson = null;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary();
		if (GodotObject.IsInstanceValid(jackson))
		{
			dictionary["jacksonNodeName"] = jackson.Name;
		}
		dictionary["hasPogo"] = hasPogo;
		dictionary["isJump"] = isJump;
		dictionary["pogoPlant"] = pogoPlant;
		dictionary["jumpToPos"] = jumpToPos;
		dictionary["jumpWait"] = jumpWait;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		if (data.ContainsKey("jacksonNodeName"))
		{
			_pendingJacksonName = data["jacksonNodeName"].AsString();
		}
		hasPogo = data.GetValueOrDefault("hasPogo", true).AsBool();
		isJump = data.GetValueOrDefault("isJump", false).AsBool();
		pogoPlant = data.GetValueOrDefault("pogoPlant", false).AsBool();
		jumpToPos = data.GetValueOrDefault("jumpToPos", 0.0).AsDouble();
		jumpWait = data.GetValueOrDefault("jumpWait", 1).AsInt32();
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!(_pendingJacksonName != ""))
		{
			return;
		}
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (GodotObject.IsInstanceValid(node2D))
		{
			Node nodeOrNull = node2D.GetNodeOrNull(_pendingJacksonName);
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				jackson = nodeOrNull as TowerDefenseZombie;
			}
		}
		_pendingJacksonName = "";
	}

	public override void InWater()
	{
		base.InWater();
		if (hasPogo)
		{
			groundHeight = 0.0;
			GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
			if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
			{
				base.groundHeightComponent.handleWaterHeight = false;
			}
		}
	}

	public override void OutWater()
	{
		if (hasPogo)
		{
			GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
			if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
			{
				base.groundHeightComponent.handleWaterHeight = true;
			}
		}
		base.OutWater();
		if (hasPogo)
		{
			ySpeed = -300.0;
		}
	}

	public async void Land()
	{
		if (!hasPogo)
		{
			return;
		}
		isJump = false;
		gravity = 490.0;
		ySpeed = -300.0;
		if (!IsRemoteNetworkReplica && pogoPlant)
		{
			isJump = true;
			if (jumpWait > 0)
			{
				jumpWait--;
			}
			else
			{
				jumpWait = 1;
				Tween tween = CreateTween();
				tween.SetEase(Tween.EaseType.InOut);
				tween.SetTrans(Tween.TransitionType.Sine);
				Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
				tween.TweenMethod(to: new Vector2((float)jumpToPos, logicalGlobalPosition.Y), method: Callable.From<Vector2>(SetLogicalGlobalPosition), from: logicalGlobalPosition, duration: 0.5);
				ySpeed = -400.0;
				await ToSignal(tween, Tween.SignalName.Finished);
				pogoPlant = false;
			}
			await ToSignal(GetTree().CreateTimer(0.2, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			isJump = false;
		}
	}

	public override bool CanBlock()
	{
		return hasPogo;
	}

	public override string BlockType()
	{
		return "Jump";
	}

	public override void Block(TowerDefenseCharacter target)
	{
		instance.ArmorDelete("Pogo");
		hasPogo = false;
		isJump = false;
		GroundHeightComponent groundHeightComponent = base.groundHeightComponent;
		if (groundHeightComponent != null && !groundHeightComponent.IsReleased)
		{
			base.groundHeightComponent.handleWaterHeight = true;
		}
		if (inWater)
		{
			groundHeight = 0.0 - waterHeight;
		}
		AudioManager.Instance.AudioPlay("Bonk");
		Walk();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(30)
		{
			new MethodInfo(MethodName.GetJackson, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetJackson, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "value", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PogoEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PogoProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PogoExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DanceEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DanceProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DanceExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Hypnoses, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canFliter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hypnosesConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OutJackson, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Land, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.PogoEntered && args.Count == 0)
		{
			PogoEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.PogoProcessing && args.Count == 1)
		{
			PogoProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PogoExited && args.Count == 0)
		{
			PogoExited();
			ret = default;
			return true;
		}
		if (method == MethodName.DanceEntered && args.Count == 0)
		{
			DanceEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DanceProcessing && args.Count == 1)
		{
			DanceProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DanceExited && args.Count == 0)
		{
			DanceExited();
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
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.Land && args.Count == 0)
		{
			Land();
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
		if (method == MethodName.ImportNetworkSpawnState)
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
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.PogoEntered)
		{
			return true;
		}
		if (method == MethodName.PogoProcessing)
		{
			return true;
		}
		if (method == MethodName.PogoExited)
		{
			return true;
		}
		if (method == MethodName.DanceEntered)
		{
			return true;
		}
		if (method == MethodName.DanceProcessing)
		{
			return true;
		}
		if (method == MethodName.DanceExited)
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
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty)
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
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
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
		if (method == MethodName.Land)
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
		if (name == PropertyName.hasPogo)
		{
			hasPogo = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isJump)
		{
			isJump = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasPogo)
		{
			_hasPogo = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.pogoPlant)
		{
			pogoPlant = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.jumpToPos)
		{
			jumpToPos = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.jumpWait)
		{
			jumpWait = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.walkTime)
		{
			walkTime = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.danceTime)
		{
			danceTime = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.jackson)
		{
			jackson = VariantUtils.ConvertTo<TowerDefenseZombie>(in value);
			return true;
		}
		if (name == PropertyName._pendingJacksonName)
		{
			_pendingJacksonName = VariantUtils.ConvertTo<string>(in value);
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
		if (name == PropertyName.hasPogo)
		{
			value = VariantUtils.CreateFrom<bool>(hasPogo);
			return true;
		}
		if (name == PropertyName.isJump)
		{
			value = VariantUtils.CreateFrom(in isJump);
			return true;
		}
		if (name == PropertyName._hasPogo)
		{
			value = VariantUtils.CreateFrom(in _hasPogo);
			return true;
		}
		if (name == PropertyName.pogoPlant)
		{
			value = VariantUtils.CreateFrom(in pogoPlant);
			return true;
		}
		if (name == PropertyName.jumpToPos)
		{
			value = VariantUtils.CreateFrom(in jumpToPos);
			return true;
		}
		if (name == PropertyName.jumpWait)
		{
			value = VariantUtils.CreateFrom(in jumpWait);
			return true;
		}
		if (name == PropertyName.walkTime)
		{
			value = VariantUtils.CreateFrom(in walkTime);
			return true;
		}
		if (name == PropertyName.danceTime)
		{
			value = VariantUtils.CreateFrom(in danceTime);
			return true;
		}
		if (name == PropertyName.jackson)
		{
			value = VariantUtils.CreateFrom(in jackson);
			return true;
		}
		if (name == PropertyName._pendingJacksonName)
		{
			value = VariantUtils.CreateFrom(in _pendingJacksonName);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.isJump, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPogo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasPogo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pogoPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpToPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.jumpWait, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.walkTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.danceTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.jackson, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingJacksonName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.hasPogo, Variant.From<bool>(hasPogo));
		info.AddProperty(PropertyName.isJump, Variant.From(in isJump));
		info.AddProperty(PropertyName._hasPogo, Variant.From(in _hasPogo));
		info.AddProperty(PropertyName.pogoPlant, Variant.From(in pogoPlant));
		info.AddProperty(PropertyName.jumpToPos, Variant.From(in jumpToPos));
		info.AddProperty(PropertyName.jumpWait, Variant.From(in jumpWait));
		info.AddProperty(PropertyName.walkTime, Variant.From(in walkTime));
		info.AddProperty(PropertyName.danceTime, Variant.From(in danceTime));
		info.AddProperty(PropertyName.jackson, Variant.From(in jackson));
		info.AddProperty(PropertyName._pendingJacksonName, Variant.From(in _pendingJacksonName));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.hasPogo, out var value))
		{
			hasPogo = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isJump, out var value2))
		{
			isJump = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasPogo, out var value3))
		{
			_hasPogo = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.pogoPlant, out var value4))
		{
			pogoPlant = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.jumpToPos, out var value5))
		{
			jumpToPos = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.jumpWait, out var value6))
		{
			jumpWait = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.walkTime, out var value7))
		{
			walkTime = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.danceTime, out var value8))
		{
			danceTime = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.jackson, out var value9))
		{
			jackson = value9.As<TowerDefenseZombie>();
		}
		if (info.TryGetProperty(PropertyName._pendingJacksonName, out var value10))
		{
			_pendingJacksonName = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value11))
		{
			_roleStateSignalsConnected = value11.As<bool>();
		}
	}
}
