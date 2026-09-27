using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter4/Pogo/Scene/TowerDefenseZombiePogo.cs")]
public class TowerDefenseZombiePogo : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName Walk = "Walk";

		public static readonly StringName PogoEntered = "PogoEntered";

		public static readonly StringName PogoProcessing = "PogoProcessing";

		public static readonly StringName PogoExited = "PogoExited";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public static readonly StringName Land = "Land";

		public static readonly StringName StartJumpMoveTween = "StartJumpMoveTween";

		public static readonly StringName IsCurrentJumpMove = "IsCurrentJumpMove";

		public static readonly StringName CancelJumpMove = "CancelJumpMove";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName CanBlock = "CanBlock";

		public new static readonly StringName BlockType = "BlockType";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName Block = "Block";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName hasPogo = "hasPogo";

		public static readonly StringName _jumpMoveTween = "_jumpMoveTween";

		public static readonly StringName _jumpMoveGeneration = "_jumpMoveGeneration";

		public static readonly StringName isJump = "isJump";

		public static readonly StringName _hasPogo = "_hasPogo";

		public static readonly StringName pogoPlant = "pogoPlant";

		public static readonly StringName jumpToPos = "jumpToPos";

		public static readonly StringName jumpWait = "jumpWait";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private AttackComponent _attackComponent2;

	private Tween _jumpMoveTween;

	private ulong _jumpMoveGeneration;

	public bool isJump;

	private bool _hasPogo = true;

	public bool pogoPlant;

	public double jumpToPos;

	public int jumpWait = 1;

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
				CancelJumpMove();
				isJump = false;
				idleAnimeClip = "Idle";
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
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_pogoStateHandle = StateMachine?.GetStateById("zombie.pogo.pogo");
			StateHandle pogoStateHandle = _pogoStateHandle;
			if (pogoStateHandle != null && pogoStateHandle.IsValid)
			{
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
		CancelJumpMove();
		OnLand -= Land;
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_attackComponent2 = componentManager.GetRuntime<AttackComponent>("character.attack.1");
			OnLand += Land;
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
		if (isGround)
		{
			ySpeed = -300.0;
		}
	}

	public void PogoProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		if (!pogoPlant)
		{
			bool pause = sprite.pause;
			ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
			if (!pause)
			{
				double num = 30.0 * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)Scale.X * (sprite.playBack ? (-1.0) : 1.0);
				if ((double)globalPositionForPhysicsFrame.X > groundRight)
				{
					num *= 2.0;
				}
				globalPositionForPhysicsFrame.X -= (float)num;
				SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
			}
			if (!pause && attackComponent.CanAttack() && !TowerDefenseManager.Instance.backZombie)
			{
				pogoPlant = true;
				float x = Scale.X;
				jumpToPos = globalPositionForPhysicsFrame.X - TowerDefenseManager.Instance.GetMapGridSize().X * x - 10f * x;
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

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Pogo")
		{
			if (instance.ArmorHas("SpecialHelmet"))
			{
				instance.unUseBuffFlags = -1;
			}
			else
			{
				instance.unUseBuffFlags = 0;
			}
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
		ulong jumpMoveGeneration = _jumpMoveGeneration;
		isJump = false;
		gravity = 490.0;
		ySpeed = -300.0;
		if (IsRemoteNetworkReplica || !pogoPlant || TowerDefenseManager.Instance.backZombie)
		{
			return;
		}
		isJump = true;
		if (jumpWait > 0)
		{
			jumpWait--;
		}
		else
		{
			jumpWait = 1;
			StartJumpMoveTween();
			Tween tween = _jumpMoveTween;
			jumpMoveGeneration = _jumpMoveGeneration;
			ySpeed = -400.0;
			await ToSignal(tween, Tween.SignalName.Finished);
			if (!IsCurrentJumpMove(tween, jumpMoveGeneration))
			{
				return;
			}
			_jumpMoveTween = null;
			pogoPlant = false;
		}
		await ToSignal(GetTree().CreateTimer(0.2, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		if (hasPogo && jumpMoveGeneration == _jumpMoveGeneration && IsInsideTree())
		{
			isJump = false;
		}
	}

	private void StartJumpMoveTween()
	{
		CancelJumpMove(clearPogoPlant: false);
		_jumpMoveTween = CreateTween();
		_jumpMoveTween.SetEase(Tween.EaseType.InOut);
		_jumpMoveTween.SetTrans(Tween.TransitionType.Sine);
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		Vector2 vector = new Vector2((float)jumpToPos, logicalGlobalPosition.Y);
		_jumpMoveTween.TweenMethod(Callable.From<Vector2>(SetLogicalGlobalPosition), logicalGlobalPosition, vector, 0.5);
	}

	private bool IsCurrentJumpMove(Tween tween, ulong generation)
	{
		if (hasPogo && generation == _jumpMoveGeneration && _jumpMoveTween == tween && GodotObject.IsInstanceValid(tween))
		{
			return IsInsideTree();
		}
		return false;
	}

	private void CancelJumpMove(bool clearPogoPlant = true)
	{
		_jumpMoveGeneration++;
		Tween jumpMoveTween = _jumpMoveTween;
		_jumpMoveTween = null;
		if (GodotObject.IsInstanceValid(jumpMoveTween))
		{
			jumpMoveTween.Kill();
		}
		if (clearPogoPlant)
		{
			pogoPlant = false;
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (!(command == "pogo"))
		{
			_ = command == "jump_check";
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

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "isJump", isJump },
			{ "hasPogo", hasPogo },
			{ "pogoPlant", pogoPlant },
			{ "jumpToPos", jumpToPos },
			{ "jumpWait", jumpWait }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		isJump = data.GetValueOrDefault("isJump", false).AsBool();
		hasPogo = data.GetValueOrDefault("hasPogo", true).AsBool();
		pogoPlant = data.GetValueOrDefault("pogoPlant", false).AsBool();
		jumpToPos = data.GetValueOrDefault("jumpToPos", 0.0).AsDouble();
		jumpWait = data.GetValueOrDefault("jumpWait", 1).AsInt32();
		if (!hasPogo)
		{
			pogoPlant = false;
			isJump = false;
		}
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
		return new List<MethodInfo>(21)
		{
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
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Land, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartJumpMoveTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsCurrentJumpMove, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "tween", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Tween"), exported: false),
				new PropertyInfo(Variant.Type.Int, "generation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelJumpMove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "clearPogoPlant", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CanBlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlockType, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.StartJumpMoveTween && args.Count == 0)
		{
			StartJumpMoveTween();
			ret = default;
			return true;
		}
		if (method == MethodName.IsCurrentJumpMove && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCurrentJumpMove(VariantUtils.ConvertTo<Tween>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1])));
			return true;
		}
		if (method == MethodName.CancelJumpMove && args.Count == 1)
		{
			CancelJumpMove(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
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
		if (method == MethodName.ArmorHitpointsEmpty)
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
		if (method == MethodName.StartJumpMoveTween)
		{
			return true;
		}
		if (method == MethodName.IsCurrentJumpMove)
		{
			return true;
		}
		if (method == MethodName.CancelJumpMove)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
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
		if (name == PropertyName.hasPogo)
		{
			hasPogo = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._jumpMoveTween)
		{
			_jumpMoveTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._jumpMoveGeneration)
		{
			_jumpMoveGeneration = VariantUtils.ConvertTo<ulong>(in value);
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
		if (name == PropertyName._jumpMoveTween)
		{
			value = VariantUtils.CreateFrom(in _jumpMoveTween);
			return true;
		}
		if (name == PropertyName._jumpMoveGeneration)
		{
			value = VariantUtils.CreateFrom(in _jumpMoveGeneration);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._jumpMoveTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._jumpMoveGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isJump, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPogo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasPogo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pogoPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpToPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.jumpWait, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.hasPogo, Variant.From<bool>(hasPogo));
		info.AddProperty(PropertyName._jumpMoveTween, Variant.From(in _jumpMoveTween));
		info.AddProperty(PropertyName._jumpMoveGeneration, Variant.From(in _jumpMoveGeneration));
		info.AddProperty(PropertyName.isJump, Variant.From(in isJump));
		info.AddProperty(PropertyName._hasPogo, Variant.From(in _hasPogo));
		info.AddProperty(PropertyName.pogoPlant, Variant.From(in pogoPlant));
		info.AddProperty(PropertyName.jumpToPos, Variant.From(in jumpToPos));
		info.AddProperty(PropertyName.jumpWait, Variant.From(in jumpWait));
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
		if (info.TryGetProperty(PropertyName._jumpMoveTween, out var value2))
		{
			_jumpMoveTween = value2.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._jumpMoveGeneration, out var value3))
		{
			_jumpMoveGeneration = value3.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.isJump, out var value4))
		{
			isJump = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasPogo, out var value5))
		{
			_hasPogo = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.pogoPlant, out var value6))
		{
			pogoPlant = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.jumpToPos, out var value7))
		{
			jumpToPos = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.jumpWait, out var value8))
		{
			jumpWait = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value9))
		{
			_roleStateSignalsConnected = value9.As<bool>();
		}
	}
}
