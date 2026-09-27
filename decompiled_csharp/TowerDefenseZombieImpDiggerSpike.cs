using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter8/ImpDiggerSpike/Scene/TowerDefenseZombieImpDiggerSpike.cs")]
public class TowerDefenseZombieImpDiggerSpike : TowerDefenseZombie, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public static readonly StringName ApplyNetworkDrillSpawnState = "ApplyNetworkDrillSpawnState";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName DigEntered = "DigEntered";

		public static readonly StringName DigProcessing = "DigProcessing";

		public static readonly StringName DigExited = "DigExited";

		public static readonly StringName DrillEntered = "DrillEntered";

		public static readonly StringName DrillProcessing = "DrillProcessing";

		public static readonly StringName DrillExited = "DrillExited";

		public static readonly StringName LandEntered = "LandEntered";

		public static readonly StringName LandProcessing = "LandProcessing";

		public static readonly StringName LandExited = "LandExited";

		public static readonly StringName UpEntered = "UpEntered";

		public static readonly StringName UpExited = "UpExited";

		public static readonly StringName SetDigShadowVisible = "SetDigShadowVisible";

		public static readonly StringName UpProcessing = "UpProcessing";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public static readonly StringName CreateEffect = "CreateEffect";

		public new static readonly StringName CanDiggerBlock = "CanDiggerBlock";

		public new static readonly StringName BlockDigger = "BlockDigger";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName speed = "speed";

		public static readonly StringName digOver = "digOver";

		public static readonly StringName discardDownPos = "discardDownPos";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName _networkDrillSpawn = "_networkDrillSpawn";

		public static readonly StringName _upWalkDelayRemaining = "_upWalkDelayRemaining";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private static PackedScene _DIGGER_RISING_DIRT;

	public double speed = 50.0;

	public bool digOver;

	public double discardDownPos = 10000.0;

	private StateHandle _digStateHandle;

	private StateHandle _drillStateHandle;

	private StateHandle _landStateHandle;

	private StateHandle _upStateHandle;

	private bool _roleStateSignalsConnected;

	private bool _networkDrillSpawn;

	private double _upWalkDelayRemaining = -1.0;

	private static PackedScene DIGGER_RISING_DIRT => _DIGGER_RISING_DIRT ?? (_DIGGER_RISING_DIRT = GD.Load<PackedScene>("uid://bjev0ulao283j"));

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
		_digStateHandle = StateMachine?.GetStateById("zombie.imp_digger_spike.dig");
		_drillStateHandle = StateMachine?.GetStateById("zombie.imp_digger_spike.drill");
		_landStateHandle = StateMachine?.GetStateById("zombie.imp_digger_spike.land");
		_upStateHandle = StateMachine?.GetStateById("zombie.imp_digger_spike.up");
		StateHandle digStateHandle = _digStateHandle;
		if (digStateHandle == null || !digStateHandle.IsValid)
		{
			return;
		}
		StateHandle drillStateHandle = _drillStateHandle;
		if (drillStateHandle == null || !drillStateHandle.IsValid)
		{
			return;
		}
		StateHandle landStateHandle = _landStateHandle;
		if (landStateHandle != null && landStateHandle.IsValid)
		{
			StateHandle upStateHandle = _upStateHandle;
			if (upStateHandle != null && upStateHandle.IsValid)
			{
				_digStateHandle.Entered += DigEntered;
				_digStateHandle.Exited += DigExited;
				_digStateHandle.PhysicsProcessing += DigProcessing;
				_drillStateHandle.Entered += DrillEntered;
				_drillStateHandle.Exited += DrillExited;
				_drillStateHandle.PhysicsProcessing += DrillProcessing;
				_landStateHandle.Entered += LandEntered;
				_landStateHandle.Exited += LandExited;
				_landStateHandle.PhysicsProcessing += LandProcessing;
				_upStateHandle.Entered += UpEntered;
				_upStateHandle.Exited += UpExited;
				_upStateHandle.PhysicsProcessing += UpProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			if (_digStateHandle != null)
			{
				_digStateHandle.Entered -= DigEntered;
				_digStateHandle.Exited -= DigExited;
				_digStateHandle.PhysicsProcessing -= DigProcessing;
			}
			_digStateHandle = null;
			if (_drillStateHandle != null)
			{
				_drillStateHandle.Entered -= DrillEntered;
				_drillStateHandle.Exited -= DrillExited;
				_drillStateHandle.PhysicsProcessing -= DrillProcessing;
			}
			_drillStateHandle = null;
			if (_landStateHandle != null)
			{
				_landStateHandle.Entered -= LandEntered;
				_landStateHandle.Exited -= LandExited;
				_landStateHandle.PhysicsProcessing -= LandProcessing;
			}
			_landStateHandle = null;
			if (_upStateHandle != null)
			{
				_upStateHandle.Entered -= UpEntered;
				_upStateHandle.Exited -= UpExited;
				_upStateHandle.PhysicsProcessing -= UpProcessing;
			}
			_upStateHandle = null;
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
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			instance.collisionFlags = 17;
			instance.maskFlags = 16;
			ConnectRoleStateSignals();
			if (_networkDrillSpawn)
			{
				CallDeferred("ApplyNetworkDrillSpawnState");
			}
		}
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		if (data != null)
		{
			_networkDrillSpawn = data.GetValueOrDefault("drill_spawn", false).AsBool();
			if (_networkDrillSpawn && IsNodeReady())
			{
				CallDeferred("ApplyNetworkDrillSpawnState");
			}
		}
	}

	private void ApplyNetworkDrillSpawnState()
	{
		if (_networkDrillSpawn && !die && !nearDie)
		{
			_networkDrillSpawn = false;
			digOver = true;
			SendStateEvent("ToDrill");
			instance.ArmorDelete("Pick");
			isRise = false;
			instance.collisionFlags = 1;
			instance.maskFlags = 9;
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && TowerDefenseManager.Instance.IsGameRunning() && inGame && ((long)Engine.GetPhysicsFrames() + (long)randFreshIndex) % 30 == 0L && !inWater)
		{
			SetSpriteGroupShaderParameter("discardDownPos", discardDownPos);
		}
	}

	public void DigEntered()
	{
		attackAnimeClip = "Eat2";
		dieAnimeClip = "Death2";
		dieWaterAnimeClip = "Death2";
		instance.collisionFlags = 17;
		instance.maskFlags = 16;
		sprite.SetAnimation("Dig");
		SetDigShadowVisible(visible: false);
	}

	public virtual void DigProcessing(double delta)
	{
		if (IsRemoteNetworkReplica)
		{
			sprite.timeScale = timeScale;
			return;
		}
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
		if (attackComponent.CanAttack() && !nearDie && !sprite.pause && !digOver)
		{
			SendStateEvent("ToUp");
		}
		else if (!sprite.pause)
		{
			double num = speed * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)((!sprite.playBack) ? 1 : (-1));
			if ((double)globalPositionForPhysicsFrame.X > TowerDefenseManager.Instance.GetMapGroundRight())
			{
				num *= 2.0;
			}
			globalPositionForPhysicsFrame.X -= (float)num;
			SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
		}
		sprite.timeScale = timeScale * 1.0;
		if (!digOver && (double)globalPositionForPhysicsFrame.X < TowerDefenseManager.Instance.GetMapGroundLeft() + 30.0)
		{
			digOver = true;
			SendStateEvent("ToDrill");
			instance.ArmorDelete("Pick");
		}
	}

	public void DigExited()
	{
		instance.unUseBuffFlags = 0;
	}

	public void DrillEntered()
	{
		attackAnimeClip = "Eat";
		dieAnimeClip = "Death";
		dieWaterAnimeClip = "Death";
		instance.collisionFlags = 1;
		instance.maskFlags = 9;
		if (!inWater)
		{
			SetDigShadowVisible(!invisible);
			CreateEffect();
		}
		Rise(0.5, 0.0, createDirt: false, changeState: false);
		sprite.SetAnimation("Drill", loop: true, 0.2);
	}

	public virtual void DrillProcessing(double delta)
	{
		sprite.timeScale = timeScale * 2.0;
	}

	public void DrillExited()
	{
	}

	public void LandEntered()
	{
		attackAnimeClip = "Eat";
		dieAnimeClip = "Death";
		dieWaterAnimeClip = "Death";
		sprite.SetAnimation("Land", loop: false);
	}

	public virtual void LandProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void LandExited()
	{
	}

	public void UpEntered()
	{
		Transform2D screenTransform = GetViewport().GetScreenTransform();
		screenTransform.Origin = Vector2.Zero;
		discardDownPos = (screenTransform * (GetLogicalGlobalPosition(spriteGroup) + new Vector2(0f, 53f))).Y;
		SetSpriteGroupShaderParameter("discardDownPos", discardDownPos);
		instance.collisionFlags = 1;
		instance.maskFlags = 9;
		if (!inWater)
		{
			SetDigShadowVisible(!invisible);
		}
		attackAnimeClip = "Eat2";
		dieAnimeClip = "Death2";
		dieWaterAnimeClip = "Death2";
		sprite.SetAnimation("Up", loop: false);
		SetSpriteGroupShaderParameter("discardDownPos", discardDownPos);
		_upWalkDelayRemaining = -1.0;
		CompleteUpEntryAsync();
	}

	private async Task CompleteUpEntryAsync()
	{
		if (await WaitForStateDelayAsync(_upStateHandle, 0.2))
		{
			sprite.SetAnimation(attackAnimeClip, loop: true, 0.2);
			ActivateAttackAfterStateDelay(_upStateHandle);
		}
	}

	public void UpExited()
	{
		_upWalkDelayRemaining = -1.0;
		startAttack = false;
		discardDownPos = 10000.0;
		SetSpriteGroupShaderParameter("discardDownPos", discardDownPos);
	}

	private void SetDigShadowVisible(bool visible)
	{
		ShadowComponent shadowComponent = base.shadowComponent;
		if (shadowComponent != null && !shadowComponent.IsReleased)
		{
			base.shadowComponent.SetShadowVisible(visible);
		}
		else
		{
			shadowSprite.Visible = visible;
		}
	}

	public void UpProcessing(double delta)
	{
		if (!nearDie && !die && !digOver && !sprite.pause && !attackComponent.CanAttack())
		{
			if (_upWalkDelayRemaining < 0.0)
			{
				_upWalkDelayRemaining = 0.2;
			}
			else
			{
				_upWalkDelayRemaining -= delta;
			}
			if (_upWalkDelayRemaining <= 0.0 && !attackComponent.CanAttack())
			{
				_upWalkDelayRemaining = -1.0;
				Walk();
			}
		}
		else
		{
			_upWalkDelayRemaining = -1.0;
			if (startAttack && !nearDie && !sprite.pause && sprite.timeScale > 0.0 && useAttackDps)
			{
				attackComponent.AttackDpsExecute(delta, ((TowerDefenseZombieConfig)config).attack);
			}
		}
		sprite.timeScale = timeScale * 2.0;
	}

	public override void Walk()
	{
		if (die)
		{
			SendStateEvent("ToDie");
		}
		else if (digOver)
		{
			base.Walk();
		}
		else
		{
			SendStateEvent("ToDig");
		}
	}

	public override void DieEntered()
	{
		base.DieEntered();
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!(clip == "Drill"))
		{
			if (clip == "Land")
			{
				WalkWithoutTransitionDelay();
			}
		}
		else
		{
			SendStateEvent("ToLand");
		}
	}

	public override async void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (armorName == "Pick")
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
			if (!digOver && !nearDie && !die)
			{
				digOver = true;
				SendStateEvent("ToDrill");
			}
		}
	}

	public void CreateEffect()
	{
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(DIGGER_RISING_DIRT, gridPos);
		towerDefenseEffectSpriteOnce.GlobalPosition = GetLogicalGlobalPosition(shadowSprite) - new Vector2(15f, 0f);
		TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
	}

	public override bool CanDiggerBlock()
	{
		return !digOver;
	}

	public override void BlockDigger(TowerDefenseCharacter target)
	{
		digOver = true;
		SendStateEvent("ToDrill");
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["speed"] = speed;
		dictionary["digOver"] = digOver;
		dictionary["discardDownPos"] = discardDownPos;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		speed = data.GetValueOrDefault("speed", 50.0).AsDouble();
		digOver = data.GetValueOrDefault("digOver", false).AsBool();
		discardDownPos = data.GetValueOrDefault("discardDownPos", 10000.0).AsDouble();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(29)
		{
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyNetworkDrillSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DigEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DigProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DigExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrillEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrillProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrillExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LandEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LandProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LandExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetDigShadowVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanDiggerBlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlockDigger, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
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
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyNetworkDrillSpawnState && args.Count == 0)
		{
			ApplyNetworkDrillSpawnState();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DigEntered && args.Count == 0)
		{
			DigEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DigProcessing && args.Count == 1)
		{
			DigProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DigExited && args.Count == 0)
		{
			DigExited();
			ret = default;
			return true;
		}
		if (method == MethodName.DrillEntered && args.Count == 0)
		{
			DrillEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DrillProcessing && args.Count == 1)
		{
			DrillProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrillExited && args.Count == 0)
		{
			DrillExited();
			ret = default;
			return true;
		}
		if (method == MethodName.LandEntered && args.Count == 0)
		{
			LandEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.LandProcessing && args.Count == 1)
		{
			LandProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LandExited && args.Count == 0)
		{
			LandExited();
			ret = default;
			return true;
		}
		if (method == MethodName.UpEntered && args.Count == 0)
		{
			UpEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.UpExited && args.Count == 0)
		{
			UpExited();
			ret = default;
			return true;
		}
		if (method == MethodName.SetDigShadowVisible && args.Count == 1)
		{
			SetDigShadowVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpProcessing && args.Count == 1)
		{
			UpProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
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
		if (method == MethodName.CreateEffect && args.Count == 0)
		{
			CreateEffect();
			ret = default;
			return true;
		}
		if (method == MethodName.CanDiggerBlock && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanDiggerBlock());
			return true;
		}
		if (method == MethodName.BlockDigger && args.Count == 1)
		{
			BlockDigger(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.ImportNetworkSpawnState)
		{
			return true;
		}
		if (method == MethodName.ApplyNetworkDrillSpawnState)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.DigEntered)
		{
			return true;
		}
		if (method == MethodName.DigProcessing)
		{
			return true;
		}
		if (method == MethodName.DigExited)
		{
			return true;
		}
		if (method == MethodName.DrillEntered)
		{
			return true;
		}
		if (method == MethodName.DrillProcessing)
		{
			return true;
		}
		if (method == MethodName.DrillExited)
		{
			return true;
		}
		if (method == MethodName.LandEntered)
		{
			return true;
		}
		if (method == MethodName.LandProcessing)
		{
			return true;
		}
		if (method == MethodName.LandExited)
		{
			return true;
		}
		if (method == MethodName.UpEntered)
		{
			return true;
		}
		if (method == MethodName.UpExited)
		{
			return true;
		}
		if (method == MethodName.SetDigShadowVisible)
		{
			return true;
		}
		if (method == MethodName.UpProcessing)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.DieEntered)
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
		if (method == MethodName.CreateEffect)
		{
			return true;
		}
		if (method == MethodName.CanDiggerBlock)
		{
			return true;
		}
		if (method == MethodName.BlockDigger)
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
		if (name == PropertyName.digOver)
		{
			digOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.discardDownPos)
		{
			discardDownPos = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._networkDrillSpawn)
		{
			_networkDrillSpawn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._upWalkDelayRemaining)
		{
			_upWalkDelayRemaining = VariantUtils.ConvertTo<double>(in value);
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
		if (name == PropertyName.digOver)
		{
			value = VariantUtils.CreateFrom(in digOver);
			return true;
		}
		if (name == PropertyName.discardDownPos)
		{
			value = VariantUtils.CreateFrom(in discardDownPos);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName._networkDrillSpawn)
		{
			value = VariantUtils.CreateFrom(in _networkDrillSpawn);
			return true;
		}
		if (name == PropertyName._upWalkDelayRemaining)
		{
			value = VariantUtils.CreateFrom(in _upWalkDelayRemaining);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.digOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.discardDownPos, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._networkDrillSpawn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._upWalkDelayRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName.digOver, Variant.From(in digOver));
		info.AddProperty(PropertyName.discardDownPos, Variant.From(in discardDownPos));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName._networkDrillSpawn, Variant.From(in _networkDrillSpawn));
		info.AddProperty(PropertyName._upWalkDelayRemaining, Variant.From(in _upWalkDelayRemaining));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.speed, out var value))
		{
			speed = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.digOver, out var value2))
		{
			digOver = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.discardDownPos, out var value3))
		{
			discardDownPos = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value4))
		{
			_roleStateSignalsConnected = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._networkDrillSpawn, out var value5))
		{
			_networkDrillSpawn = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._upWalkDelayRemaining, out var value6))
		{
			_upWalkDelayRemaining = value6.As<double>();
		}
	}
}
