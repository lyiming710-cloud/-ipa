using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/Pogoball/Scene/TowerDefenseZombiePogoball.cs")]
public class TowerDefenseZombiePogoball : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

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

		public static readonly StringName CompletePogoPlantLandingAsync = "CompletePogoPlantLandingAsync";

		public static readonly StringName SetPogoTweenX = "SetPogoTweenX";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName CanBlock = "CanBlock";

		public new static readonly StringName BlockType = "BlockType";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName Block = "Block";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName eventList = "eventList";

		public static readonly StringName hasPogo = "hasPogo";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName isJump = "isJump";

		public static readonly StringName _hasPogo = "_hasPogo";

		public static readonly StringName pogoPlant = "pogoPlant";

		public static readonly StringName jumpToPos = "jumpToPos";

		public static readonly StringName jumpWait = "jumpWait";

		public static readonly StringName quake = "quake";

		public static readonly StringName moveTween = "moveTween";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private StateHandle _pogoStateHandle;

	private bool _stateSignalsConnected;

	private static PackedScene _QUAKE_SCENE;

	private AttackComponent _attackComponent2;

	public bool isJump;

	private bool _hasPogo = true;

	public bool pogoPlant;

	public double jumpToPos;

	public int jumpWait = 2;

	public bool quake;

	public Tween moveTween;

	private static PackedScene QUAKE_SCENE => _QUAKE_SCENE ?? (_QUAKE_SCENE = GD.Load<PackedScene>("uid://dwcqjpnoyit8f"));

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList { get; set; } = new Array<TowerDefenseCharacterEventBase>();

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
				instance.collisionFlags = 1;
				idleAnimeClip = "Idle";
			}
		}
	}

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_pogoStateHandle = StateMachine?.GetStateById("zombie.pogoball.pogo");
			StateHandle pogoStateHandle = _pogoStateHandle;
			if (pogoStateHandle != null && pogoStateHandle.IsValid)
			{
				_pogoStateHandle.Entered += PogoEntered;
				_pogoStateHandle.Exited += PogoExited;
				_pogoStateHandle.PhysicsProcessing += PogoProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_pogoStateHandle != null)
			{
				_pogoStateHandle.Entered -= PogoEntered;
				_pogoStateHandle.Exited -= PogoExited;
				_pogoStateHandle.PhysicsProcessing -= PogoProcessing;
			}
			_pogoStateHandle = null;
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
			OnLand += Land;
			ySpeed = -300.0;
			ConnectStateSignals();
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
		if (die)
		{
			return;
		}
		sprite.timeScale = timeScale;
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		bool flag = attackComponent.CanAttack();
		if (flag && attackComponent.target is TowerDefensePlant && (attackComponent.target.instance.physiqueTypeFlags & 0x10) != 0)
		{
			if (attackComponent.target.instance.spikeHurt != -1.0)
			{
				TowerDefenseCharacter target = attackComponent.target;
				double spikeHurt = attackComponent.target.instance.spikeHurt;
				target.Hurt(100.0, playSplatAudio: true, default, createDamagePart: true, spikeHurt);
			}
			else
			{
				attackComponent.target.Destroy();
			}
			instance.ArmorDelete("Pogo");
			hasPogo = false;
			isJump = false;
			AudioManager.Instance.AudioPlay("Bonk");
			Walk();
		}
		else
		{
			if (pogoPlant)
			{
				return;
			}
			bool pause = sprite.pause;
			ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
			if (!quake && !pause)
			{
				double num = sprite.timeScale;
				double num2 = (sprite.playBack ? (-1.0) : 1.0);
				double num3 = 30.0 * delta * num * (double)transformPoint.Scale.X * (double)Scale.X * num2;
				if ((double)globalPositionForPhysicsFrame.X > groundRight)
				{
					num3 *= 2.0;
				}
				globalPositionForPhysicsFrame.X -= (float)num3;
				SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
			}
			if (!pause && (flag || attackComponent.CanAttack()))
			{
				quake = false;
				pogoPlant = true;
				float x = Scale.X;
				jumpToPos = globalPositionForPhysicsFrame.X - TowerDefenseManager.Instance.GetMapGridSize().X * x - 10f * x;
				if (_attackComponent2.HasAttackGridTargetCandidates() && _attackComponent2.CanAttack() && GodotObject.IsInstanceValid(_attackComponent2.target) && _attackComponent2.target.instance.height >= TowerDefenseEnum.CHARACTER_HEIGHT.TALL)
				{
					quake = true;
					jumpToPos = globalPositionForPhysicsFrame.X;
				}
			}
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

	public void Land()
	{
		if (die || !hasPogo)
		{
			return;
		}
		isJump = false;
		gravity = 490.0;
		ySpeed = -300.0;
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		if (_attackComponent2.HasAttackGridTargetCandidates() && _attackComponent2.CanAttack() && GodotObject.IsInstanceValid(_attackComponent2.target) && _attackComponent2.target.instance.height >= TowerDefenseEnum.CHARACTER_HEIGHT.TALL)
		{
			quake = true;
		}
		if (quake)
		{
			ySpeed = -400.0;
			bool flag = false;
			if (GodotObject.IsInstanceValid(_attackComponent2.target) && GodotObject.IsInstanceValid(_attackComponent2.target.cell))
			{
				TowerDefenseCharacter surround = _attackComponent2.target.cell.GetSurround();
				if (GodotObject.IsInstanceValid(surround))
				{
					surround.Hurt(100.0);
					flag = true;
				}
			}
			if (!flag)
			{
				TowerDefenseExplode.CreateExplode(GetLogicalGlobalPosition(), new Vector2(1.25f, 0.25f), eventList, new Array<TowerDefenseCharacter>(), camp, instance.collisionFlags);
			}
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(QUAKE_SCENE, gridPos, "Idle");
			towerDefenseEffectSpriteOnce.GlobalPosition = GetLogicalGlobalPosition(transformPoint) - new Vector2(0f, 30f);
			TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
			quake = false;
		}
		if (pogoPlant)
		{
			CompletePogoPlantLandingAsync();
		}
	}

	private async void CompletePogoPlantLandingAsync()
	{
		if (die || !hasPogo || !pogoPlant)
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
			jumpWait = GD.RandRange(2, 3);
			Tween tween = CreateTween();
			tween.SetEase(Tween.EaseType.InOut);
			tween.SetTrans(Tween.TransitionType.Sine);
			tween.TweenMethod(Callable.From<double>(SetPogoTweenX), GetLogicalGlobalPosition().X, jumpToPos, 0.5);
			ySpeed = -400.0;
			quake = true;
			await ToSignal(tween, Tween.SignalName.Finished);
			if (die || !IsInsideTree())
			{
				return;
			}
			pogoPlant = false;
		}
		await ToSignal(GetTree().CreateTimer(0.2, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		if (!die && IsInsideTree())
		{
			isJump = false;
		}
	}

	private void SetPogoTweenX(double value)
	{
		if (!die && IsInsideTree())
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			logicalGlobalPosition.X = (float)value;
			SetLogicalGlobalPosition(logicalGlobalPosition);
		}
	}

	public override void DieEntered()
	{
		base.DieEntered();
		isJump = false;
		pogoPlant = false;
		quake = false;
		ySpeed = 0.0;
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
		if (GodotObject.IsInstanceValid(moveTween))
		{
			if (hasPogo)
			{
				return !moveTween.IsRunning();
			}
			return false;
		}
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
			["isJump"] = isJump,
			["hasPogo"] = hasPogo,
			["pogoPlant"] = pogoPlant,
			["jumpToPos"] = jumpToPos,
			["jumpWait"] = jumpWait,
			["quake"] = quake
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		isJump = data.GetValueOrDefault("isJump", isJump).AsBool();
		hasPogo = data.GetValueOrDefault("hasPogo", hasPogo).AsBool();
		pogoPlant = data.GetValueOrDefault("pogoPlant", pogoPlant).AsBool();
		jumpToPos = data.GetValueOrDefault("jumpToPos", jumpToPos).AsDouble();
		jumpWait = Math.Max(0, data.GetValueOrDefault("jumpWait", jumpWait).AsInt32());
		quake = data.GetValueOrDefault("quake", quake).AsBool();
	}

	public override void Block(TowerDefenseCharacter target)
	{
		pogoPlant = false;
		ySpeed = -500.0;
		target.Hurt(100.0);
		moveTween = CreateTween();
		moveTween.SetEase(Tween.EaseType.Out);
		moveTween.SetTrans(Tween.TransitionType.Cubic);
		float x = GetLogicalGlobalPosition().X;
		moveTween.TweenMethod(Callable.From<double>(SetPogoTweenX), x, (double)x + (double)TowerDefenseManager.Instance.GetMapGridSize().X * 1.5, 2.0);
		AudioManager.Instance.AudioPlay("Bonk");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(21)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.CompletePogoPlantLandingAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPogoTweenX, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.CompletePogoPlantLandingAsync && args.Count == 0)
		{
			CompletePogoPlantLandingAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPogoTweenX && args.Count == 1)
		{
			SetPogoTweenX(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.CompletePogoPlantLandingAsync)
		{
			return true;
		}
		if (method == MethodName.SetPogoTweenX)
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
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.hasPogo)
		{
			hasPogo = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.quake)
		{
			quake = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.moveTween)
		{
			moveTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		if (name == PropertyName.hasPogo)
		{
			value = VariantUtils.CreateFrom<bool>(hasPogo);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
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
		if (name == PropertyName.quake)
		{
			value = VariantUtils.CreateFrom(in quake);
			return true;
		}
		if (name == PropertyName.moveTween)
		{
			value = VariantUtils.CreateFrom(in moveTween);
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
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isJump, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasPogo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasPogo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pogoPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.jumpToPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.jumpWait, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.quake, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.moveTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.hasPogo, Variant.From<bool>(hasPogo));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName.isJump, Variant.From(in isJump));
		info.AddProperty(PropertyName._hasPogo, Variant.From(in _hasPogo));
		info.AddProperty(PropertyName.pogoPlant, Variant.From(in pogoPlant));
		info.AddProperty(PropertyName.jumpToPos, Variant.From(in jumpToPos));
		info.AddProperty(PropertyName.jumpWait, Variant.From(in jumpWait));
		info.AddProperty(PropertyName.quake, Variant.From(in quake));
		info.AddProperty(PropertyName.moveTween, Variant.From(in moveTween));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.eventList, out var value))
		{
			eventList = value.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.hasPogo, out var value2))
		{
			hasPogo = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value3))
		{
			_stateSignalsConnected = value3.As<bool>();
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
		if (info.TryGetProperty(PropertyName.quake, out var value9))
		{
			quake = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.moveTween, out var value10))
		{
			moveTween = value10.As<Tween>();
		}
	}
}
