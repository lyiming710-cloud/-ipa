using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/SwanRider/Scene/TowerDefenseZombieSwanRider.cs")]
public class TowerDefenseZombieSwanRider : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName ReleaseCarriedCharacter = "ReleaseCarriedCharacter";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSaveWhenEmpty = "ImportVariantSaveWhenEmpty";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName ProcessCarryCandidate = "ProcessCarryCandidate";

		public static readonly StringName TryCarryZombieAsync = "TryCarryZombieAsync";

		public static readonly StringName TryCarryZombie = "TryCarryZombie";

		public static readonly StringName FlyEntered = "FlyEntered";

		public static readonly StringName FlyProcessing = "FlyProcessing";

		public static readonly StringName FlyExited = "FlyExited";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public new static readonly StringName Blow = "Blow";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _carryOriginalMaskFlags = "_carryOriginalMaskFlags";

		public static readonly StringName _hasCarryOriginalMaskFlags = "_hasCarryOriginalMaskFlags";

		public static readonly StringName useCone = "useCone";

		public static readonly StringName useBucket = "useBucket";

		public static readonly StringName over = "over";

		public static readonly StringName isFly = "isFly";

		public static readonly StringName speed = "speed";

		public static readonly StringName isFlying = "isFlying";

		public static readonly StringName mowerKill = "mowerKill";

		public static readonly StringName carryCharacter = "carryCharacter";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const string CarryOriginalMaskFlagsSaveKey = "carryOriginalMaskFlags";

	private StateHandle _flyStateHandle;

	private bool _stateSignalsConnected;

	private int _carryOriginalMaskFlags;

	private bool _hasCarryOriginalMaskFlags;

	public bool useCone;

	public bool useBucket;

	public bool over;

	public bool isFly;

	public double speed = 30.0;

	public bool isFlying;

	public bool mowerKill;

	public TowerDefenseCharacter carryCharacter;

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_flyStateHandle = StateMachine?.GetStateById("zombie.swan_rider.fly");
			StateHandle flyStateHandle = _flyStateHandle;
			if (flyStateHandle != null && flyStateHandle.IsValid)
			{
				_flyStateHandle.Entered += FlyEntered;
				_flyStateHandle.Exited += FlyExited;
				_flyStateHandle.Processing += FlyProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_flyStateHandle != null)
			{
				_flyStateHandle.Entered -= FlyEntered;
				_flyStateHandle.Exited -= FlyExited;
				_flyStateHandle.Processing -= FlyProcessing;
			}
			_flyStateHandle = null;
			_stateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		if (!Engine.IsEditorHint())
		{
			ReleaseCarriedCharacter();
		}
		DisconnectStateSignals();
		base._ExitTree();
	}

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint())
		{
			return;
		}
		ConnectStateSignals();
		if (!IsProgressRestoreInFlight)
		{
			double num = GD.Randf();
			if (num < 0.1)
			{
				useBucket = true;
				sprite.SetFliters((Array?)new Array<string> { "anim_bucket" }, open: true);
			}
			else if (num < 0.3)
			{
				useCone = true;
				sprite.SetFliters((Array?)new Array<string> { "anim_cone" }, open: true);
			}
		}
		targetRegistrationComponent.canCarry = false;
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) && TowerDefenseManager.CurrentControl.isGameRunning && inGame && !over && !die && !nearDie)
		{
			if (GodotObject.IsInstanceValid(carryCharacter))
			{
				ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
				Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
				Vector2 globalPositionForPhysicsFrame2 = carryCharacter.GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
				globalPositionForPhysicsFrame2.X = globalPositionForPhysicsFrame.X + (instance.hypnoses ? (-30f) : 30f);
				carryCharacter.SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame2, currentPhysicsFrame);
				carryCharacter.groundHeight = z + 60.0;
				carryCharacter.z = z + 60.0;
			}
			else
			{
				ProcessCarryCandidate();
			}
		}
	}

	public override void WalkProcessing(double delta)
	{
		sprite.timeScale = timeScale * walkSpeedScale;
		if (attackComponent.CanAttack())
		{
			if (GodotObject.IsInstanceValid(attackComponent.target.cell) && attackComponent.target.cell.HasSpike())
			{
				attackComponent.target = attackComponent.target.cell.GetSpike();
			}
			if ((attackComponent.target.instance.physiqueTypeFlags & 0x10) == 0)
			{
				attackComponent.SmashAttackCell(((TowerDefenseZombieConfig)config).smashAttack);
			}
			else
			{
				if (attackComponent.target.instance.spikeHurt != -1.0)
				{
					TowerDefenseCharacter target = attackComponent.target;
					double spikeHurt = attackComponent.target.instance.spikeHurt;
					target.Hurt(100000.0, playSplatAudio: true, default, createDamagePart: true, spikeHurt);
				}
				Die();
			}
		}
		if (instance.hitpoints <= 1000.0 && !isFly)
		{
			if (inWater)
			{
				sprite.SetAnimation("RiseWater", loop: false, 0.2);
			}
			else
			{
				sprite.SetAnimation("Rise", loop: false, 0.2);
			}
			isFly = true;
		}
	}

	public override void DieEntered()
	{
		base.DieEntered();
	}

	public override async void DestroySet()
	{
		if (over)
		{
			return;
		}
		over = true;
		HitBoxDestroy();
		sprite.SetFliters((Array?)new Array<string>
		{
			"anim_bucket", "anim_cone", "anim_hair", "Zombie_outerarm_lower", "Zombie_outerarm_upper", "Zombie_outerarm_hand", "anim_head2", "Zombie_tie", "Zombie_body", "Zombie_outerleg_lower",
			"Zombie_outerleg_foot", "Zombie_outerleg_upper", "Zombie_innerleg_foot", "Zombie_innerleg_lower", "Zombie_innerleg_upper", "anim_head1", "Zombie_neck", "anim_innerarm1", "anim_innerarm2", "anim_innerarm3"
		}, open: false);
		ReleaseCarriedCharacter();
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		if (mowerKill)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			return;
		}
		string packetName = "ZombieNormal";
		if (useCone)
		{
			packetName = "ZombieNormalCone";
		}
		if (useBucket)
		{
			packetName = "ZombieNormalBucket";
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
		double height = groundHeight;
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		TowerDefenseCharacter zombie = packetConfig.Create(logicalGlobalPosition, gridPos, height);
		TowerDefenseManager.GetCharacterNode().CallDeferred("add_child", zombie);
		double hitpointScale = instance.hitpointScale;
		Vector2 scale = transformPoint.Scale;
		zombie.CallDeferred("SetHitpointAndScale", hitpointScale, scale);
		if (instance.hypnoses)
		{
			Callable.From(() =>
			{
				if (GodotObject.IsInstanceValid(zombie))
				{
					zombie.Hypnoses();
				}
			}).CallDeferred();
		}
		zombie.CallDeferred("WalkReady");
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, zombie);
				MultiPlayerManager.Instance.SendSpawnCharacterAt(packetName, gridPos.X, gridPos.Y, nextSyncId, hitpointScale, scale.X, instance.hypnoses, 0.0, useCreate: true, logicalGlobalPosition.X, logicalGlobalPosition.Y, walkAfterSpawn: true, height);
			}
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

	private void ReleaseCarriedCharacter()
	{
		TowerDefenseCharacter towerDefenseCharacter = carryCharacter;
		if (TowerDefenseZombieCarryRelation.Release(this, ref carryCharacter, groundHeight) && _hasCarryOriginalMaskFlags && GodotObject.IsInstanceValid(towerDefenseCharacter) && GodotObject.IsInstanceValid(towerDefenseCharacter.instance))
		{
			towerDefenseCharacter.instance.maskFlags = _carryOriginalMaskFlags;
		}
		_carryOriginalMaskFlags = 0;
		_hasCarryOriginalMaskFlags = false;
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary();
		TowerDefenseZombieCarryRelation.Export(dictionary, carryCharacter);
		if (GodotObject.IsInstanceValid(carryCharacter) && _hasCarryOriginalMaskFlags)
		{
			dictionary["carryOriginalMaskFlags"] = _carryOriginalMaskFlags;
		}
		dictionary["useCone"] = useCone;
		dictionary["useBucket"] = useBucket;
		dictionary["isFly"] = isFly;
		dictionary["isFlying"] = isFlying;
		dictionary["over"] = over;
		dictionary["speed"] = speed;
		dictionary["mowerKill"] = mowerKill;
		return dictionary;
	}

	public override bool ImportVariantSaveWhenEmpty()
	{
		return true;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		useCone = data.GetValueOrDefault("useCone", false).AsBool();
		useBucket = data.GetValueOrDefault("useBucket", false).AsBool();
		isFly = data.GetValueOrDefault("isFly", false).AsBool();
		isFlying = data.GetValueOrDefault("isFlying", false).AsBool();
		over = data.GetValueOrDefault("over", false).AsBool();
		speed = data.GetValueOrDefault("speed", 30.0).AsDouble();
		mowerKill = data.GetValueOrDefault("mowerKill", false).AsBool();
		sprite.SetFliters((Array?)new Array<string> { "anim_bucket" }, useBucket);
		sprite.SetFliters((Array?)new Array<string> { "anim_cone" }, useCone);
		TowerDefenseZombie towerDefenseZombie = TowerDefenseZombieCarryRelation.Resolve(data);
		int value = (data.ContainsKey("carryOriginalMaskFlags") ? data["carryOriginalMaskFlags"].AsInt32() : (towerDefenseZombie?.config?.maskFlags ?? 1));
		if (!GodotObject.IsInstanceValid(towerDefenseZombie) || towerDefenseZombie == this || !AttachCarriedZombie(towerDefenseZombie, value))
		{
			ReleaseCarriedCharacter();
		}
	}

	private void ProcessCarryCandidate()
	{
		if (!TryGetActiveWorldHitRect(out var rect))
		{
			return;
		}
		List<TowerDefenseCharacter> charactersIntersectingRectListForCamp = TowerDefenseManager.Instance.characterRegistry.GetCharactersIntersectingRectListForCamp(rect, camp);
		for (int i = 0; i < charactersIntersectingRectListForCamp.Count; i++)
		{
			if (charactersIntersectingRectListForCamp[i] is TowerDefenseZombie zombie)
			{
				TryCarryZombieAsync(zombie);
				if (GodotObject.IsInstanceValid(carryCharacter))
				{
					break;
				}
			}
		}
	}

	private async void TryCarryZombieAsync(TowerDefenseZombie zombie)
	{
		if (TryCarryZombie(zombie) && !isFly)
		{
			if (inWater)
			{
				sprite.SetAnimation("RiseWater", loop: false, 0.2);
			}
			else
			{
				sprite.SetAnimation("Rise", loop: false, 0.2);
			}
			isFly = true;
			await ToSignal(GetTree().CreateTimer(0.3, processAlways: false), SceneTreeTimer.SignalName.Timeout);
			isFlying = true;
		}
	}

	private bool TryCarryZombie(TowerDefenseZombie zombie)
	{
		if (over)
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) || !TowerDefenseManager.CurrentControl.isGameRunning)
		{
			return false;
		}
		if (!inGame)
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(carryCharacter))
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(zombie))
		{
			return false;
		}
		if (zombie == this)
		{
			return false;
		}
		if (zombie.isRise)
		{
			return false;
		}
		if (zombie.camp != camp)
		{
			return false;
		}
		if (!zombie.CanCollision(instance.maskFlags))
		{
			return false;
		}
		if (zombie.instance.zombiePhysique > TowerDefenseEnum.ZOMBIE_PHYSIQUE.NORMAL)
		{
			return false;
		}
		if (!zombie.targetRegistrationComponent.canCarry)
		{
			return false;
		}
		return AttachCarriedZombie(zombie);
	}

	private bool AttachCarriedZombie(TowerDefenseZombie zombie, int? originalMaskFlags = null)
	{
		if (!TowerDefenseZombieCarryRelation.Attach(this, ref carryCharacter, zombie, z + 60.0))
		{
			return false;
		}
		_carryOriginalMaskFlags = originalMaskFlags ?? zombie.instance.maskFlags;
		_hasCarryOriginalMaskFlags = true;
		zombie.instance.maskFlags = 2;
		return true;
	}

	public void FlyEntered()
	{
		sprite.SetAnimation("Fly");
		instance.collisionFlags = 2;
		instance.maskFlags = 2;
	}

	public void FlyProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		if (!sprite.pause)
		{
			if (isFlying)
			{
				double num = sprite.timeScale;
				double num2 = (sprite.playBack ? (-1.0) : 1.0);
				ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
				Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
				globalPositionForPhysicsFrame.X -= (float)(speed * delta * num * (double)transformPoint.Scale.X * (double)Scale.X * num2);
				SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
			}
			if (attackComponent.CanAttack() && GodotObject.IsInstanceValid(attackComponent.target) && attackComponent.target is TowerDefenseZombie)
			{
				attackComponent.AttackExecute(((TowerDefenseZombieConfig)config).smashAttack);
			}
		}
	}

	public void FlyExited()
	{
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		if (TowerDefenseManager.HasGameplayAuthority)
		{
			ReleaseCarriedCharacter();
		}
	}

	public override void Blow()
	{
		if (isFly)
		{
			BlowBack(1.0, 1.0);
		}
	}

	public override void Walk()
	{
		if (die)
		{
			SendStateEvent("ToDie");
		}
		else if (isFly)
		{
			SendStateEvent("ToFly");
		}
		else
		{
			SendStateEvent("ToWalk");
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (((!die && !isDestroy) || (!(clip == "Rise") && !(clip == "RiseWater"))) && (clip == "Rise" || clip == "RiseWater"))
		{
			isFlying = true;
			Walk();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(22)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseCarriedCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSaveWhenEmpty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessCarryCandidate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryCarryZombieAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryCarryZombie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "zombie", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FlyEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlyProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlyExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Blow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
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
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseCarriedCharacter && args.Count == 0)
		{
			ReleaseCarriedCharacter();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportVariantSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportVariantSave());
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ImportVariantSaveWhenEmpty());
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessCarryCandidate && args.Count == 0)
		{
			ProcessCarryCandidate();
			ret = default;
			return true;
		}
		if (method == MethodName.TryCarryZombieAsync && args.Count == 1)
		{
			TryCarryZombieAsync(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryCarryZombie && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryCarryZombie(VariantUtils.ConvertTo<TowerDefenseZombie>(in args[0])));
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
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.Blow && args.Count == 0)
		{
			Blow();
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.BatchUpdate)
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
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.ReleaseCarriedCharacter)
		{
			return true;
		}
		if (method == MethodName.ExportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSaveWhenEmpty)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.ProcessCarryCandidate)
		{
			return true;
		}
		if (method == MethodName.TryCarryZombieAsync)
		{
			return true;
		}
		if (method == MethodName.TryCarryZombie)
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
		if (method == MethodName.OnHypnosisStateChanged)
		{
			return true;
		}
		if (method == MethodName.Blow)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._carryOriginalMaskFlags)
		{
			_carryOriginalMaskFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._hasCarryOriginalMaskFlags)
		{
			_hasCarryOriginalMaskFlags = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useCone)
		{
			useCone = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useBucket)
		{
			useBucket = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isFly)
		{
			isFly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.speed)
		{
			speed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.isFlying)
		{
			isFlying = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mowerKill)
		{
			mowerKill = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.carryCharacter)
		{
			carryCharacter = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName._carryOriginalMaskFlags)
		{
			value = VariantUtils.CreateFrom(in _carryOriginalMaskFlags);
			return true;
		}
		if (name == PropertyName._hasCarryOriginalMaskFlags)
		{
			value = VariantUtils.CreateFrom(in _hasCarryOriginalMaskFlags);
			return true;
		}
		if (name == PropertyName.useCone)
		{
			value = VariantUtils.CreateFrom(in useCone);
			return true;
		}
		if (name == PropertyName.useBucket)
		{
			value = VariantUtils.CreateFrom(in useBucket);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName.isFly)
		{
			value = VariantUtils.CreateFrom(in isFly);
			return true;
		}
		if (name == PropertyName.speed)
		{
			value = VariantUtils.CreateFrom(in speed);
			return true;
		}
		if (name == PropertyName.isFlying)
		{
			value = VariantUtils.CreateFrom(in isFlying);
			return true;
		}
		if (name == PropertyName.mowerKill)
		{
			value = VariantUtils.CreateFrom(in mowerKill);
			return true;
		}
		if (name == PropertyName.carryCharacter)
		{
			value = VariantUtils.CreateFrom(in carryCharacter);
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
			new PropertyInfo(Variant.Type.Int, PropertyName._carryOriginalMaskFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasCarryOriginalMaskFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useCone, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useBucket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isFly, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.speed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isFlying, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mowerKill, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.carryCharacter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._carryOriginalMaskFlags, Variant.From(in _carryOriginalMaskFlags));
		info.AddProperty(PropertyName._hasCarryOriginalMaskFlags, Variant.From(in _hasCarryOriginalMaskFlags));
		info.AddProperty(PropertyName.useCone, Variant.From(in useCone));
		info.AddProperty(PropertyName.useBucket, Variant.From(in useBucket));
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName.isFly, Variant.From(in isFly));
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName.isFlying, Variant.From(in isFlying));
		info.AddProperty(PropertyName.mowerKill, Variant.From(in mowerKill));
		info.AddProperty(PropertyName.carryCharacter, Variant.From(in carryCharacter));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value))
		{
			_stateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._carryOriginalMaskFlags, out var value2))
		{
			_carryOriginalMaskFlags = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._hasCarryOriginalMaskFlags, out var value3))
		{
			_hasCarryOriginalMaskFlags = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useCone, out var value4))
		{
			useCone = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useBucket, out var value5))
		{
			useBucket = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value6))
		{
			over = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isFly, out var value7))
		{
			isFly = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.speed, out var value8))
		{
			speed = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.isFlying, out var value9))
		{
			isFlying = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mowerKill, out var value10))
		{
			mowerKill = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.carryCharacter, out var value11))
		{
			carryCharacter = value11.As<TowerDefenseCharacter>();
		}
	}
}
