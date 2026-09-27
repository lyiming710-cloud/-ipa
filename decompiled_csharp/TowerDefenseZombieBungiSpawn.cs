using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter5/Bungi/Scene/TowerDefenseZombieBungiSpawn.cs")]
public class TowerDefenseZombieBungiSpawn : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Set = "_Set";

		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName PrepareForProgressRestore = "PrepareForProgressRestore";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName CompleteSpawnOperations = "CompleteSpawnOperations";

		public static readonly StringName AreSpawnOperationsCurrent = "AreSpawnOperationsCurrent";

		public static readonly StringName AbortSpawn = "AbortSpawn";

		public static readonly StringName CleanupUnplacedPayload = "CleanupUnplacedPayload";

		public static readonly StringName SuspendPayloadVerticalPhysics = "SuspendPayloadVerticalPhysics";

		public static readonly StringName RestorePayloadVerticalPhysics = "RestorePayloadVerticalPhysics";

		public static readonly StringName SetCarriedZ = "SetCarriedZ";

		public new static readonly StringName HitpointsNearDie = "HitpointsNearDie";

		public new static readonly StringName HitpointsEmpty = "HitpointsEmpty";

		public new static readonly StringName Walk = "Walk";

		public static readonly StringName DropEntered = "DropEntered";

		public static readonly StringName DropProcessing = "DropProcessing";

		public static readonly StringName DropExited = "DropExited";

		public static readonly StringName CancelDropPresentation = "CancelDropPresentation";

		public static readonly StringName RiseEntered = "RiseEntered";

		public static readonly StringName RiseProcessing = "RiseProcessing";

		public static readonly StringName RiseExited = "RiseExited";

		public static readonly StringName CancelRisePresentation = "CancelRisePresentation";

		public new static readonly StringName CanBlock = "CanBlock";

		public new static readonly StringName Block = "Block";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName PutCharacter = "PutCharacter";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName @override = "override";

		public static readonly StringName characterName = "characterName";

		public static readonly StringName override_ = "override_";

		public static readonly StringName canBlock = "canBlock";

		public static readonly StringName waveOperationId = "waveOperationId";

		public static readonly StringName waveOperationOwner = "waveOperationOwner";

		public static readonly StringName battleOperationOwner = "battleOperationOwner";

		public static readonly StringName battleOperationId = "battleOperationId";

		public static readonly StringName dropTween = "dropTween";

		public static readonly StringName character = "character";

		public static readonly StringName _riseTween = "_riseTween";

		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName _payloadPlaced = "_payloadPlaced";

		public static readonly StringName _restoredFromProgress = "_restoredFromProgress";

		public static readonly StringName _payloadGravitySuspended = "_payloadGravitySuspended";

		public static readonly StringName _payloadGravityUseBeforeCarry = "_payloadGravityUseBeforeCarry";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string characterName = "";

	public TowerDefenseCharacterOverride override_;

	public bool canBlock = true;

	[Export(PropertyHint.None, "")]
	public int waveOperationId = -1;

	public TowerDefenseBattleFeatureWave waveOperationOwner;

	public TowerDefenseControlNew battleOperationOwner;

	public int battleOperationId = -1;

	public Tween dropTween;

	public TowerDefenseCharacter character;

	private Tween _riseTween;

	private StateHandle _dropStateHandle;

	private StateHandle _riseStateHandle;

	private bool _roleStateSignalsConnected;

	private bool _payloadPlaced;

	private bool _restoredFromProgress;

	private bool _payloadGravitySuspended;

	private bool _payloadGravityUseBeforeCarry = true;

	[Export(PropertyHint.None, "")]
	public TowerDefenseCharacterOverride @override
	{
		get
		{
			return override_;
		}
		set
		{
			override_ = value;
		}
	}

	public override bool _Set(StringName property, Variant value)
	{
		if (property.ToString() == "override_")
		{
			override_ = value.As<TowerDefenseCharacterOverride>();
			return true;
		}
		return base._Set(property, value);
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
		_dropStateHandle = StateMachine?.GetStateById("zombie.bungi_spawn.drop");
		_riseStateHandle = StateMachine?.GetStateById("zombie.bungi_spawn.rise");
		StateHandle dropStateHandle = _dropStateHandle;
		if (dropStateHandle != null && dropStateHandle.IsValid)
		{
			StateHandle riseStateHandle = _riseStateHandle;
			if (riseStateHandle != null && riseStateHandle.IsValid)
			{
				_dropStateHandle.Entered += DropEntered;
				_dropStateHandle.Exited += DropExited;
				_dropStateHandle.PhysicsProcessing += DropProcessing;
				_riseStateHandle.Entered += RiseEntered;
				_riseStateHandle.Exited += RiseExited;
				_riseStateHandle.PhysicsProcessing += RiseProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			if (_dropStateHandle != null)
			{
				_dropStateHandle.Entered -= DropEntered;
				_dropStateHandle.Exited -= DropExited;
				_dropStateHandle.PhysicsProcessing -= DropProcessing;
			}
			_dropStateHandle = null;
			if (_riseStateHandle != null)
			{
				_riseStateHandle.Entered -= RiseEntered;
				_riseStateHandle.Exited -= RiseExited;
				_riseStateHandle.PhysicsProcessing -= RiseProcessing;
			}
			_riseStateHandle = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void PrepareForProgressRestore()
	{
		base.PrepareForProgressRestore();
		_restoredFromProgress = true;
	}

	public override async void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint())
		{
			return;
		}
		ConnectRoleStateSignals();
		targetRegistrationComponent.canCarry = false;
		AddToGroup("Bungi", persistent: true);
		z = 600.0;
		isGround = false;
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (!AreSpawnOperationsCurrent())
		{
			AbortSpawn();
			return;
		}
		if (_restoredFromProgress)
		{
			if (!GodotObject.IsInstanceValid(character))
			{
				AbortSpawn();
			}
			else if (!_payloadPlaced)
			{
				SuspendPayloadVerticalPhysics();
			}
			return;
		}
		shadowSprite.Visible = !invisible;
		if (Global.Instance.isMultiplayerMode && !MultiPlayerManager.Instance.isHost)
		{
			Walk();
			return;
		}
		try
		{
			if (characterName != "")
			{
				TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(characterName);
				if (GodotObject.IsInstanceValid(packetConfig))
				{
					character = packetConfig.Create(GetLogicalGlobalPosition(), gridPos);
					if (!GodotObject.IsInstanceValid(character))
					{
						throw new InvalidOperationException("Failed to create bungee payload '" + characterName + "'.");
					}
					SuspendPayloadVerticalPhysics();
					character.z = 600.0;
					character.isGround = false;
					TowerDefenseGroundItemBase.characterNode.AddChild(character, forceReadableName: false, InternalMode.Disabled);
					if (instance.hypnoses)
					{
						character.Hypnoses();
					}
					character.instance.canBeCollection = false;
					character.shadowSprite.Visible = false;
					character.PreSpawn();
					if (character is TowerDefenseZombie { attackComponent: var attackComponent } towerDefenseZombie)
					{
						if (attackComponent != null && !attackComponent.IsReleased)
						{
							towerDefenseZombie.attackComponent.alive = false;
						}
						towerDefenseZombie.instance.collisionFlags = 0;
						towerDefenseZombie.instance.maskFlags = 0;
						if (towerDefenseZombie.inWater)
						{
							WaterInteractionComponent waterInteractionComponent = towerDefenseZombie.waterInteractionComponent;
							if (waterInteractionComponent != null && !waterInteractionComponent.IsReleased)
							{
								if (GodotObject.IsInstanceValid(towerDefenseZombie.waterInteractionComponent.activeTween))
								{
									towerDefenseZombie.waterInteractionComponent.activeTween.Kill();
								}
								towerDefenseZombie.waterInteractionComponent.isInWater = false;
							}
							towerDefenseZombie.SetSpriteGroupShaderParameter("discardDownPos", 10000.0);
							if (GodotObject.IsInstanceValid(towerDefenseZombie.waterLineSprite))
							{
								towerDefenseZombie.waterLineSprite.Visible = false;
							}
							if (GodotObject.IsInstanceValid(towerDefenseZombie.duckytobeSprite))
							{
								towerDefenseZombie.duckytobeSprite.Visible = false;
							}
						}
					}
					if (GodotObject.IsInstanceValid(override_))
					{
						override_.ExecuteCharacter(character);
					}
					character.SetLogicalGlobalPosition(GetLogicalGlobalPosition());
					if (character is TowerDefenseZombie towerDefenseZombie2 && GodotObject.IsInstanceValid(waveOperationOwner))
					{
						waveOperationOwner.AddSpawnCharacter(towerDefenseZombie2);
					}
				}
			}
			if (!GodotObject.IsInstanceValid(character))
			{
				throw new InvalidOperationException("Invalid bungee payload '" + characterName + "'.");
			}
			Walk();
		}
		catch (Exception value)
		{
			GD.PushError($"[BungiSpawn] Failed to initialize '{characterName}': {value}");
			AbortSpawn();
		}
	}

	public override void _ExitTree()
	{
		CancelDropPresentation();
		CancelRisePresentation();
		DisconnectRoleStateSignals();
		CleanupUnplacedPayload();
		CompleteSpawnOperations();
		base._ExitTree();
	}

	private void CompleteSpawnOperations()
	{
		if (battleOperationId >= 0 && GodotObject.IsInstanceValid(battleOperationOwner))
		{
			battleOperationOwner.CompletePendingBattleOperation(battleOperationId);
		}
		if (waveOperationId >= 0 && GodotObject.IsInstanceValid(waveOperationOwner))
		{
			waveOperationOwner.CompletePendingSpawnOperation(waveOperationId);
		}
		battleOperationId = -1;
		waveOperationId = -1;
		battleOperationOwner = null;
		waveOperationOwner = null;
	}

	private bool AreSpawnOperationsCurrent()
	{
		if (battleOperationId >= 0 && (!GodotObject.IsInstanceValid(battleOperationOwner) || !battleOperationOwner.IsPendingBattleOperationCurrent(battleOperationId)))
		{
			return false;
		}
		if (waveOperationId >= 0 && (!GodotObject.IsInstanceValid(waveOperationOwner) || !waveOperationOwner.IsPendingSpawnOperationCurrent(waveOperationId)))
		{
			return false;
		}
		return true;
	}

	private void AbortSpawn()
	{
		CleanupUnplacedPayload();
		CompleteSpawnOperations();
		if (!IsQueuedForDeletion())
		{
			QueueFree();
		}
	}

	private void CleanupUnplacedPayload()
	{
		if (!_payloadPlaced && GodotObject.IsInstanceValid(character) && !character.IsQueuedForDeletion())
		{
			TowerDefenseCharacter towerDefenseCharacter = character;
			character = null;
			if (towerDefenseCharacter.GetParent() == this)
			{
				towerDefenseCharacter.QueueFree();
			}
			else
			{
				towerDefenseCharacter.Destroy();
			}
		}
	}

	private void SuspendPayloadVerticalPhysics()
	{
		if (!_payloadPlaced && GodotObject.IsInstanceValid(character))
		{
			if (!_payloadGravitySuspended)
			{
				_payloadGravityUseBeforeCarry = character.gravityUse;
				_payloadGravitySuspended = true;
			}
			character.gravityUse = false;
			character.ySpeed = 0.0;
		}
	}

	private void RestorePayloadVerticalPhysics()
	{
		if (GodotObject.IsInstanceValid(character))
		{
			if (_payloadGravitySuspended)
			{
				character.gravityUse = _payloadGravityUseBeforeCarry;
			}
			character.ySpeed = 0.0;
			_payloadGravitySuspended = false;
		}
	}

	private void SetCarriedZ(double height)
	{
		z = height;
		if (GodotObject.IsInstanceValid(character))
		{
			character.SetLogicalGlobalPosition(GetLogicalGlobalPosition());
			character.z = height;
		}
	}

	public override void HitpointsNearDie()
	{
		base.HitpointsNearDie();
		Destroy();
	}

	public override void HitpointsEmpty()
	{
		base.HitpointsEmpty();
		Destroy();
	}

	public override void Walk()
	{
		ActivateGameplayProcessing();
		SendStateEvent("ToDrop");
	}

	public void DropEntered()
	{
		AudioManager.Instance.AudioPlay("BungeeScream");
		z = 600.0;
		isGround = false;
		if (GodotObject.IsInstanceValid(character))
		{
			SuspendPayloadVerticalPhysics();
			character.SetLogicalGlobalPosition(GetLogicalGlobalPosition());
			character.z = 600.0;
			character.isGround = false;
		}
		dropTween = CreateTween();
		dropTween.SetParallel();
		dropTween.SetEase(Tween.EaseType.Out);
		dropTween.SetTrans(Tween.TransitionType.Cubic);
		double num = 0.0;
		if (GodotObject.IsInstanceValid(cell))
		{
			num = cell.GetGroundHeight();
		}
		dropTween.TweenMethod(Callable.From<double>(SetCarriedZ), 600.0, num, 0.75);
		sprite.SetAnimation("Drop", loop: true, 0.2);
		CompleteDropAsync();
	}

	private async Task CompleteDropAsync()
	{
		if (await WaitForStateDelayAsync(_dropStateHandle, 0.75))
		{
			isGround = true;
			if (PutCharacter())
			{
				SendStateEvent("ToRise");
			}
		}
	}

	public void DropProcessing(double delta)
	{
		sprite.timeScale = timeScale * 2.0;
	}

	public void DropExited()
	{
		CancelDropPresentation();
	}

	private void CancelDropPresentation()
	{
		if (GodotObject.IsInstanceValid(dropTween))
		{
			dropTween.Kill();
		}
		dropTween = null;
	}

	public void RiseEntered()
	{
		sprite.SetAnimation("Rise", loop: true, 0.2);
		isGround = false;
		double duration = 1.5;
		if (!canBlock)
		{
			duration = 0.75;
		}
		_riseTween = CreateTween();
		_riseTween.SetParallel();
		_riseTween.SetEase(Tween.EaseType.Out);
		_riseTween.SetTrans(Tween.TransitionType.Cubic);
		if (!canBlock)
		{
			_riseTween.TweenMethod(Callable.From<double>(SetCarriedZ), z, 600.0, duration);
		}
		else
		{
			_riseTween.TweenProperty(this, "z", 600, duration);
		}
		CompleteRiseAsync(duration);
	}

	private async Task CompleteRiseAsync(double duration)
	{
		if (await WaitForStateDelayAsync(_riseStateHandle, duration))
		{
			Destroy();
		}
	}

	public void RiseProcessing(double delta)
	{
		sprite.timeScale = timeScale * 1.0;
	}

	public void RiseExited()
	{
		CancelRisePresentation();
	}

	private void CancelRisePresentation()
	{
		if (GodotObject.IsInstanceValid(_riseTween))
		{
			_riseTween.Kill();
		}
		_riseTween = null;
	}

	public override bool CanBlock()
	{
		if (canBlock)
		{
			return z <= 50.0;
		}
		return false;
	}

	public override void Block(TowerDefenseCharacter target)
	{
		CancelDropPresentation();
		canBlock = false;
		instance.invincible = true;
		SendStateEvent("ToRise");
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = new Dictionary
		{
			{ "characterName", characterName },
			{ "canBlock", canBlock },
			{ "payloadPlaced", _payloadPlaced },
			{ "payloadGravitySuspended", _payloadGravitySuspended },
			{ "payloadGravityUseBeforeCarry", _payloadGravityUseBeforeCarry }
		};
		if (GodotObject.IsInstanceValid(character))
		{
			dictionary["payloadNodeName"] = character.Name;
		}
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		characterName = data.GetValueOrDefault("characterName", characterName).AsString();
		canBlock = data.GetValueOrDefault("canBlock", canBlock).AsBool();
		_payloadPlaced = data.GetValueOrDefault("payloadPlaced", false).AsBool();
		_payloadGravitySuspended = data.GetValueOrDefault("payloadGravitySuspended", false).AsBool();
		_payloadGravityUseBeforeCarry = data.GetValueOrDefault("payloadGravityUseBeforeCarry", true).AsBool();
		character = null;
		string text = data.GetValueOrDefault("payloadNodeName", "").AsString();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(node2D))
		{
			return;
		}
		TowerDefenseCharacter nodeOrNull = node2D.GetNodeOrNull<TowerDefenseCharacter>(new NodePath(text));
		if (GodotObject.IsInstanceValid(nodeOrNull) && nodeOrNull != this)
		{
			character = nodeOrNull;
			if (!_payloadPlaced)
			{
				SuspendPayloadVerticalPhysics();
			}
		}
	}

	public bool PutCharacter()
	{
		if (!AreSpawnOperationsCurrent() || !GodotObject.IsInstanceValid(character))
		{
			AbortSpawn();
			return false;
		}
		try
		{
			character.gridPos = gridPos;
			RestorePayloadVerticalPhysics();
			if (character.GetParent() != TowerDefenseGroundItemBase.characterNode)
			{
				character.Reparent(TowerDefenseGroundItemBase.characterNode);
			}
			character.ActivateGameplayProcessing();
			if (character is TowerDefenseZombie { groundMoveComponent: { IsReleased: false } } towerDefenseZombie)
			{
				towerDefenseZombie.groundMoveComponent.ConnectGroundNodeSignals();
			}
			character.isGround = true;
			if (GodotObject.IsInstanceValid(character.cell))
			{
				character.groundHeight = character.cell.GetGroundHeight();
				character.z = character.groundHeight;
			}
			if (GodotObject.IsInstanceValid(cell) && !cell.isWater)
			{
				character.shadowSprite.Visible = !character.invisible;
			}
			character.instance.canBeCollection = true;
			if (character is TowerDefenseZombie { attackComponent: var attackComponent } towerDefenseZombie2)
			{
				if (attackComponent != null && !attackComponent.IsReleased)
				{
					towerDefenseZombie2.attackComponent.alive = true;
				}
				towerDefenseZombie2.instance.collisionFlags = 1;
				towerDefenseZombie2.instance.maskFlags = 1;
			}
			Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(gridPos);
			character.SetLogicalGlobalPosition(mapCellPlantPos);
			character.shadowSprite.Scale = character.shadowComponent.saveShadowScale;
			character.shadowComponent.saveShadowPosition = mapCellPlantPos + new Vector2(12f, 36f);
			character.shadowSprite.GlobalPosition = mapCellPlantPos + new Vector2(12f, 36f);
			if (GodotObject.IsInstanceValid(cell) && cell.isWater && character is TowerDefenseZombie { inWater: not false, waterInteractionComponent: var waterInteractionComponent } towerDefenseZombie3)
			{
				if (waterInteractionComponent != null && !waterInteractionComponent.IsReleased)
				{
					towerDefenseZombie3.waterInteractionComponent.InWaterDiscardSet();
				}
				if (GodotObject.IsInstanceValid(towerDefenseZombie3.waterLineSprite))
				{
					towerDefenseZombie3.waterLineSprite.Visible = true;
				}
				if (GodotObject.IsInstanceValid(towerDefenseZombie3.duckytobeSprite))
				{
					towerDefenseZombie3.duckytobeSprite.Visible = true;
				}
			}
			if (GodotObject.IsInstanceValid(cell) && character is TowerDefensePlant towerDefensePlant)
			{
				cell.CharacterPlant(towerDefensePlant.packet, towerDefensePlant);
			}
			if (TowerDefenseManager.Instance != null)
			{
				TowerDefenseManager.Instance.CharacterRegister(character);
			}
			if (TowerDefenseManager.Instance.IsGameRunning() && character is TowerDefenseZombie towerDefenseZombie4)
			{
				towerDefenseZombie4.Walk();
			}
			_payloadPlaced = true;
			CompleteSpawnOperations();
			return true;
		}
		catch (Exception value)
		{
			GD.PushError($"[BungiSpawn] Failed to place '{characterName}': {value}");
			AbortSpawn();
			return false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(29)
		{
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareForProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteSpawnOperations, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AreSpawnOperationsCurrent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AbortSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CleanupUnplacedPayload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SuspendPayloadVerticalPhysics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestorePayloadVerticalPhysics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCarriedZ, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "height", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HitpointsNearDie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DropEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DropProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DropExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelDropPresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RiseEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RiseProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RiseExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelRisePresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanBlock, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Block, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PutCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Set && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
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
		if (method == MethodName.PrepareForProgressRestore && args.Count == 0)
		{
			PrepareForProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteSpawnOperations && args.Count == 0)
		{
			CompleteSpawnOperations();
			ret = default;
			return true;
		}
		if (method == MethodName.AreSpawnOperationsCurrent && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(AreSpawnOperationsCurrent());
			return true;
		}
		if (method == MethodName.AbortSpawn && args.Count == 0)
		{
			AbortSpawn();
			ret = default;
			return true;
		}
		if (method == MethodName.CleanupUnplacedPayload && args.Count == 0)
		{
			CleanupUnplacedPayload();
			ret = default;
			return true;
		}
		if (method == MethodName.SuspendPayloadVerticalPhysics && args.Count == 0)
		{
			SuspendPayloadVerticalPhysics();
			ret = default;
			return true;
		}
		if (method == MethodName.RestorePayloadVerticalPhysics && args.Count == 0)
		{
			RestorePayloadVerticalPhysics();
			ret = default;
			return true;
		}
		if (method == MethodName.SetCarriedZ && args.Count == 1)
		{
			SetCarriedZ(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HitpointsNearDie && args.Count == 0)
		{
			HitpointsNearDie();
			ret = default;
			return true;
		}
		if (method == MethodName.HitpointsEmpty && args.Count == 0)
		{
			HitpointsEmpty();
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.DropEntered && args.Count == 0)
		{
			DropEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DropProcessing && args.Count == 1)
		{
			DropProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DropExited && args.Count == 0)
		{
			DropExited();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelDropPresentation && args.Count == 0)
		{
			CancelDropPresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.RiseEntered && args.Count == 0)
		{
			RiseEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.RiseProcessing && args.Count == 1)
		{
			RiseProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RiseExited && args.Count == 0)
		{
			RiseExited();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelRisePresentation && args.Count == 0)
		{
			CancelRisePresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.CanBlock && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanBlock());
			return true;
		}
		if (method == MethodName.Block && args.Count == 1)
		{
			Block(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
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
		if (method == MethodName.PutCharacter && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(PutCharacter());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Set)
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
		if (method == MethodName.PrepareForProgressRestore)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.CompleteSpawnOperations)
		{
			return true;
		}
		if (method == MethodName.AreSpawnOperationsCurrent)
		{
			return true;
		}
		if (method == MethodName.AbortSpawn)
		{
			return true;
		}
		if (method == MethodName.CleanupUnplacedPayload)
		{
			return true;
		}
		if (method == MethodName.SuspendPayloadVerticalPhysics)
		{
			return true;
		}
		if (method == MethodName.RestorePayloadVerticalPhysics)
		{
			return true;
		}
		if (method == MethodName.SetCarriedZ)
		{
			return true;
		}
		if (method == MethodName.HitpointsNearDie)
		{
			return true;
		}
		if (method == MethodName.HitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.DropEntered)
		{
			return true;
		}
		if (method == MethodName.DropProcessing)
		{
			return true;
		}
		if (method == MethodName.DropExited)
		{
			return true;
		}
		if (method == MethodName.CancelDropPresentation)
		{
			return true;
		}
		if (method == MethodName.RiseEntered)
		{
			return true;
		}
		if (method == MethodName.RiseProcessing)
		{
			return true;
		}
		if (method == MethodName.RiseExited)
		{
			return true;
		}
		if (method == MethodName.CancelRisePresentation)
		{
			return true;
		}
		if (method == MethodName.CanBlock)
		{
			return true;
		}
		if (method == MethodName.Block)
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
		if (method == MethodName.PutCharacter)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.@override)
		{
			@override = VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in value);
			return true;
		}
		if (name == PropertyName.characterName)
		{
			characterName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.override_)
		{
			override_ = VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in value);
			return true;
		}
		if (name == PropertyName.canBlock)
		{
			canBlock = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.waveOperationId)
		{
			waveOperationId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.waveOperationOwner)
		{
			waveOperationOwner = VariantUtils.ConvertTo<TowerDefenseBattleFeatureWave>(in value);
			return true;
		}
		if (name == PropertyName.battleOperationOwner)
		{
			battleOperationOwner = VariantUtils.ConvertTo<TowerDefenseControlNew>(in value);
			return true;
		}
		if (name == PropertyName.battleOperationId)
		{
			battleOperationId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.dropTween)
		{
			dropTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName.character)
		{
			character = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._riseTween)
		{
			_riseTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._payloadPlaced)
		{
			_payloadPlaced = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._restoredFromProgress)
		{
			_restoredFromProgress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._payloadGravitySuspended)
		{
			_payloadGravitySuspended = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._payloadGravityUseBeforeCarry)
		{
			_payloadGravityUseBeforeCarry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.@override)
		{
			value = VariantUtils.CreateFrom<TowerDefenseCharacterOverride>(@override);
			return true;
		}
		if (name == PropertyName.characterName)
		{
			value = VariantUtils.CreateFrom(in characterName);
			return true;
		}
		if (name == PropertyName.override_)
		{
			value = VariantUtils.CreateFrom(in override_);
			return true;
		}
		if (name == PropertyName.canBlock)
		{
			value = VariantUtils.CreateFrom(in canBlock);
			return true;
		}
		if (name == PropertyName.waveOperationId)
		{
			value = VariantUtils.CreateFrom(in waveOperationId);
			return true;
		}
		if (name == PropertyName.waveOperationOwner)
		{
			value = VariantUtils.CreateFrom(in waveOperationOwner);
			return true;
		}
		if (name == PropertyName.battleOperationOwner)
		{
			value = VariantUtils.CreateFrom(in battleOperationOwner);
			return true;
		}
		if (name == PropertyName.battleOperationId)
		{
			value = VariantUtils.CreateFrom(in battleOperationId);
			return true;
		}
		if (name == PropertyName.dropTween)
		{
			value = VariantUtils.CreateFrom(in dropTween);
			return true;
		}
		if (name == PropertyName.character)
		{
			value = VariantUtils.CreateFrom(in character);
			return true;
		}
		if (name == PropertyName._riseTween)
		{
			value = VariantUtils.CreateFrom(in _riseTween);
			return true;
		}
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName._payloadPlaced)
		{
			value = VariantUtils.CreateFrom(in _payloadPlaced);
			return true;
		}
		if (name == PropertyName._restoredFromProgress)
		{
			value = VariantUtils.CreateFrom(in _restoredFromProgress);
			return true;
		}
		if (name == PropertyName._payloadGravitySuspended)
		{
			value = VariantUtils.CreateFrom(in _payloadGravitySuspended);
			return true;
		}
		if (name == PropertyName._payloadGravityUseBeforeCarry)
		{
			value = VariantUtils.CreateFrom(in _payloadGravityUseBeforeCarry);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.characterName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.override_, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.@override, PropertyHint.ResourceType, "TowerDefenseCharacterOverride", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.canBlock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.waveOperationId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.waveOperationOwner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.battleOperationOwner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.battleOperationId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.dropTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.character, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._riseTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._payloadPlaced, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._restoredFromProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._payloadGravitySuspended, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._payloadGravityUseBeforeCarry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.@override, Variant.From<TowerDefenseCharacterOverride>(@override));
		info.AddProperty(PropertyName.characterName, Variant.From(in characterName));
		info.AddProperty(PropertyName.override_, Variant.From(in override_));
		info.AddProperty(PropertyName.canBlock, Variant.From(in canBlock));
		info.AddProperty(PropertyName.waveOperationId, Variant.From(in waveOperationId));
		info.AddProperty(PropertyName.waveOperationOwner, Variant.From(in waveOperationOwner));
		info.AddProperty(PropertyName.battleOperationOwner, Variant.From(in battleOperationOwner));
		info.AddProperty(PropertyName.battleOperationId, Variant.From(in battleOperationId));
		info.AddProperty(PropertyName.dropTween, Variant.From(in dropTween));
		info.AddProperty(PropertyName.character, Variant.From(in character));
		info.AddProperty(PropertyName._riseTween, Variant.From(in _riseTween));
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName._payloadPlaced, Variant.From(in _payloadPlaced));
		info.AddProperty(PropertyName._restoredFromProgress, Variant.From(in _restoredFromProgress));
		info.AddProperty(PropertyName._payloadGravitySuspended, Variant.From(in _payloadGravitySuspended));
		info.AddProperty(PropertyName._payloadGravityUseBeforeCarry, Variant.From(in _payloadGravityUseBeforeCarry));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.@override, out var value))
		{
			@override = value.As<TowerDefenseCharacterOverride>();
		}
		if (info.TryGetProperty(PropertyName.characterName, out var value2))
		{
			characterName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.override_, out var value3))
		{
			override_ = value3.As<TowerDefenseCharacterOverride>();
		}
		if (info.TryGetProperty(PropertyName.canBlock, out var value4))
		{
			canBlock = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.waveOperationId, out var value5))
		{
			waveOperationId = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.waveOperationOwner, out var value6))
		{
			waveOperationOwner = value6.As<TowerDefenseBattleFeatureWave>();
		}
		if (info.TryGetProperty(PropertyName.battleOperationOwner, out var value7))
		{
			battleOperationOwner = value7.As<TowerDefenseControlNew>();
		}
		if (info.TryGetProperty(PropertyName.battleOperationId, out var value8))
		{
			battleOperationId = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.dropTween, out var value9))
		{
			dropTween = value9.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName.character, out var value10))
		{
			character = value10.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._riseTween, out var value11))
		{
			_riseTween = value11.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value12))
		{
			_roleStateSignalsConnected = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._payloadPlaced, out var value13))
		{
			_payloadPlaced = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._restoredFromProgress, out var value14))
		{
			_restoredFromProgress = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._payloadGravitySuspended, out var value15))
		{
			_payloadGravitySuspended = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._payloadGravityUseBeforeCarry, out var value16))
		{
			_payloadGravityUseBeforeCarry = value16.As<bool>();
		}
	}
}
