using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter4/Angel/Scene/TowerDefenseZombieAngel.cs")]
public class TowerDefenseZombieAngel : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectRoleStateSignals = "ConnectRoleStateSignals";

		public static readonly StringName DisconnectRoleStateSignals = "DisconnectRoleStateSignals";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName ShootingAttackEntered = "ShootingAttackEntered";

		public static readonly StringName ShootingAttackProcessing = "ShootingAttackProcessing";

		public static readonly StringName ShootingAttackExited = "ShootingAttackExited";

		public static readonly StringName ReviveAttackEntered = "ReviveAttackEntered";

		public static readonly StringName ReviveAttackProcessing = "ReviveAttackProcessing";

		public static readonly StringName ReviveAttackExited = "ReviveAttackExited";

		public new static readonly StringName Walk = "Walk";

		public static readonly StringName ScheduleWalkTransitionRetry = "ScheduleWalkTransitionRetry";

		public static readonly StringName RetryWalkTransitionAfterReady = "RetryWalkTransitionAfterReady";

		public static readonly StringName RetryWalkTransitionOnNextFrame = "RetryWalkTransitionOnNextFrame";

		public static readonly StringName CancelWalkTransitionRetry = "CancelWalkTransitionRetry";

		public static readonly StringName DisconnectWalkReadySignal = "DisconnectWalkReadySignal";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName Blow = "Blow";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName DestroySet = "DestroySet";

		public static readonly StringName CreateSelf = "CreateSelf";

		public static readonly StringName CreateDeathCharacter = "CreateDeathCharacter";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _roleStateSignalsConnected = "_roleStateSignalsConnected";

		public static readonly StringName _walkTransitionRetries = "_walkTransitionRetries";

		public static readonly StringName _walkRetryTree = "_walkRetryTree";

		public static readonly StringName _walkReadySignalConnected = "_walkReadySignalConnected";

		public static readonly StringName speed = "speed";

		public static readonly StringName isRevive = "isRevive";

		public static readonly StringName isReviveOver = "isReviveOver";

		public static readonly StringName isBlow = "isBlow";

		public static readonly StringName spawnTimer = "spawnTimer";

		public static readonly StringName invincible = "invincible";

		public static readonly StringName invincibleTimer = "invincibleTimer";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private StateHandle _shootingStateHandle;

	private StateHandle _reviveStateHandle;

	private bool _roleStateSignalsConnected;

	private const int MaxWalkTransitionRetries = 8;

	private int _walkTransitionRetries;

	private SceneTree _walkRetryTree;

	private Action _walkRetryHandler;

	private bool _walkReadySignalConnected;

	private static PackedScene _RIVIVE;

	public double speed = 10.0;

	public bool isRevive;

	public bool isReviveOver;

	public bool isBlow;

	public double spawnTimer = 15.0;

	public bool invincible;

	public double invincibleTimer = 8.0;

	private static PackedScene RIVIVE => _RIVIVE ?? (_RIVIVE = GD.Load<PackedScene>("uid://dbgw1lmiiyypp"));

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
		_shootingStateHandle = StateMachine?.GetStateById("zombie.angel.shooting");
		_reviveStateHandle = StateMachine?.GetStateById("zombie.angel.revive");
		StateHandle shootingStateHandle = _shootingStateHandle;
		if (shootingStateHandle != null && shootingStateHandle.IsValid)
		{
			StateHandle reviveStateHandle = _reviveStateHandle;
			if (reviveStateHandle != null && reviveStateHandle.IsValid)
			{
				_shootingStateHandle.Entered += ShootingAttackEntered;
				_shootingStateHandle.Exited += ShootingAttackExited;
				_shootingStateHandle.PhysicsProcessing += ShootingAttackProcessing;
				_reviveStateHandle.Entered += ReviveAttackEntered;
				_reviveStateHandle.Exited += ReviveAttackExited;
				_reviveStateHandle.PhysicsProcessing += ReviveAttackProcessing;
				_roleStateSignalsConnected = true;
			}
		}
	}

	private void DisconnectRoleStateSignals()
	{
		if (_roleStateSignalsConnected)
		{
			if (_shootingStateHandle != null)
			{
				_shootingStateHandle.Entered -= ShootingAttackEntered;
				_shootingStateHandle.Exited -= ShootingAttackExited;
				_shootingStateHandle.PhysicsProcessing -= ShootingAttackProcessing;
			}
			_shootingStateHandle = null;
			if (_reviveStateHandle != null)
			{
				_reviveStateHandle.Entered -= ReviveAttackEntered;
				_reviveStateHandle.Exited -= ReviveAttackExited;
				_reviveStateHandle.PhysicsProcessing -= ReviveAttackProcessing;
			}
			_reviveStateHandle = null;
			_roleStateSignalsConnected = false;
		}
	}

	public override void _ExitTree()
	{
		CancelWalkTransitionRetry();
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
		if (!Engine.IsEditorHint() && invincible)
		{
			ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
			if (invincibleTimer > 0.0 && GetGlobalPositionForPhysicsFrame(currentPhysicsFrame).X > TowerDefenseManager.Instance.GetMapCellPos(new Vector2I(4, 0)).X)
			{
				invincibleTimer -= delta;
				return;
			}
			sprite.SetFliters(new Godot.Collections.Array { "wing2_1", "wing2_2" }, open: false);
			instance.unUseBuffFlags = 6;
			instance.invincible = false;
			invincible = false;
		}
	}

	public void ShootingAttackEntered()
	{
		sprite.SetAnimation("Shooting", loop: false, 0.2);
	}

	public void ShootingAttackProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public void ShootingAttackExited()
	{
	}

	public void ReviveAttackEntered()
	{
		sprite.SetAnimation("Revive", loop: false, 0.2);
		sprite.SetFliters(new Godot.Collections.Array { "wing2_1", "wing2_2" }, open: true);
		instance.unUseBuffFlags = -1;
		instance.invincible = true;
		invincible = true;
		invincibleTimer = 8.0;
	}

	public void ReviveAttackProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public void ReviveAttackExited()
	{
	}

	public override void Walk()
	{
		if (!IsNodeReady())
		{
			ScheduleWalkTransitionRetry();
			return;
		}
		ActivateGameplayProcessing();
		StateHandle currentStateHandle = CurrentStateHandle;
		if (currentStateHandle == null || !currentStateHandle.IsValid)
		{
			ScheduleWalkTransitionRetry();
		}
		else if (isRevive && !isReviveOver)
		{
			isReviveOver = SendStateEvent("ToRevive") && CurrentStateHandle?.StableId == "zombie.angel.revive";
			if (!isReviveOver)
			{
				ScheduleWalkTransitionRetry();
				return;
			}
			_walkTransitionRetries = 0;
			CancelWalkTransitionRetry();
		}
		else
		{
			base.Walk();
			if (!die && CurrentStateHandle?.StableId != "zombie.walk")
			{
				ScheduleWalkTransitionRetry();
				return;
			}
			_walkTransitionRetries = 0;
			CancelWalkTransitionRetry();
		}
	}

	private void ScheduleWalkTransitionRetry()
	{
		if (!IsNodeReady())
		{
			if (!_walkReadySignalConnected)
			{
				Ready += RetryWalkTransitionAfterReady;
				_walkReadySignalConnected = true;
			}
		}
		else if (_walkRetryHandler == null && _walkTransitionRetries < 8 && IsInsideTree())
		{
			_walkRetryTree = GetTree();
			if (GodotObject.IsInstanceValid(_walkRetryTree))
			{
				_walkRetryHandler = RetryWalkTransitionOnNextFrame;
				_walkRetryTree.ProcessFrame += _walkRetryHandler;
			}
		}
	}

	private void RetryWalkTransitionAfterReady()
	{
		DisconnectWalkReadySignal();
		if (!die && !isDestroy)
		{
			Callable.From(Walk).CallDeferred();
		}
	}

	private void RetryWalkTransitionOnNextFrame()
	{
		CancelWalkTransitionRetry();
		if (IsInsideTree() && !die && !isDestroy)
		{
			_walkTransitionRetries++;
			Walk();
		}
	}

	private void CancelWalkTransitionRetry()
	{
		DisconnectWalkReadySignal();
		if (GodotObject.IsInstanceValid(_walkRetryTree) && _walkRetryHandler != null)
		{
			_walkRetryTree.ProcessFrame -= _walkRetryHandler;
		}
		_walkRetryTree = null;
		_walkRetryHandler = null;
	}

	private void DisconnectWalkReadySignal()
	{
		if (_walkReadySignalConnected)
		{
			Ready -= RetryWalkTransitionAfterReady;
		}
		_walkReadySignalConnected = false;
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
		sprite.timeScale = timeScale;
		if (IsRemoteNetworkReplica)
		{
			return;
		}
		ulong currentPhysicsFrame = TowerDefenseProcessModeDispatch.CurrentPhysicsFrame;
		Vector2 globalPositionForPhysicsFrame = GetGlobalPositionForPhysicsFrame(currentPhysicsFrame);
		if (!sprite.pause)
		{
			globalPositionForPhysicsFrame.X -= (float)(speed * delta * sprite.timeScale * (double)transformPoint.Scale.X * (double)Scale.X * (double)((!sprite.playBack) ? 1 : (-1)));
			SetGlobalPositionForPhysicsFrame(globalPositionForPhysicsFrame, currentPhysicsFrame);
		}
		if (IsInsideComponentBattlefield)
		{
			if (spawnTimer > 0.0)
			{
				spawnTimer -= delta;
			}
			else if (TowerDefenseManager.HasDeathRecord(camp, requireAngelEligible: true))
			{
				spawnTimer = 15.0;
				SendStateEvent("ToShooting");
			}
			else
			{
				spawnTimer = 3.0;
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

	public override async void Blow()
	{
		if (instance.invincible)
		{
			BlowBack(1.0, 1.0);
		}
		isBlow = true;
		HitBoxDestroy();
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		Vector2 vector = new Vector2(logicalGlobalPosition.X + (float)((double)(TowerDefenseManager.Instance.GetMapGridSize().Y * (float)TowerDefenseManager.Instance.GetMapGridNum().Y) * 2.0), logicalGlobalPosition.Y);
		Tween tween = CreateTween();
		tween.TweenMethod(Callable.From<Vector2>(SetLogicalGlobalPosition), logicalGlobalPosition, vector, 1.0);
		await ToSignal(tween, Tween.SignalName.Finished);
		Destroy();
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "speed", speed },
			{ "isRevive", isRevive },
			{ "isReviveOver", isReviveOver },
			{ "isBlow", isBlow },
			{ "spawnTimer", spawnTimer },
			{ "invincible", invincible },
			{ "invincibleTimer", invincibleTimer }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		speed = data.GetValueOrDefault("speed", 10.0).AsDouble();
		isRevive = data.GetValueOrDefault("isRevive", false).AsBool();
		isReviveOver = data.GetValueOrDefault("isReviveOver", false).AsBool();
		isBlow = data.GetValueOrDefault("isBlow", false).AsBool();
		spawnTimer = data.GetValueOrDefault("spawnTimer", 15.0).AsDouble();
		invincible = data.GetValueOrDefault("invincible", false).AsBool();
		invincibleTimer = data.GetValueOrDefault("invincibleTimer", 8.0).AsDouble();
	}

	public override async void DestroySet()
	{
		if (!isRevive)
		{
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			if (isBlow)
			{
				CreateSelf(new Vector2((float)groundRight, logicalGlobalPosition.Y));
			}
			else
			{
				CreateSelf(logicalGlobalPosition);
			}
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	public async void CreateSelf(Vector2 pos)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		TowerDefenseCharacter zombie = packet.Create(pos, gridPos, groundHeight);
		if (zombie is TowerDefenseZombieAngel towerDefenseZombieAngel)
		{
			towerDefenseZombieAngel.isRevive = true;
		}
		TowerDefenseManager.GetCharacterNode().CallDeferred("add_child", zombie);
		if (instance.hypnoses)
		{
			Callable.From(() =>
			{
				zombie.Hypnoses();
			}).CallDeferred();
		}
		double _hitpointScale = instance.hitpointScale;
		Vector2 _scale = transformPoint.Scale;
		Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(zombie))
			{
				if (GodotObject.IsInstanceValid(zombie.instance))
				{
					zombie.instance.hitpointScale = _hitpointScale;
				}
				if (GodotObject.IsInstanceValid(zombie.transformPoint))
				{
					zombie.transformPoint.Scale = _scale;
				}
			}
		}).CallDeferred();
		zombie.invisible = invisible;
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, zombie);
				MultiPlayerManager.Instance.SendSpawnCharacterAt(packet.saveKey, gridPos.X, gridPos.Y, nextSyncId, _hitpointScale, _scale.X, instance.hypnoses, 0.0, useCreate: true, pos.X, pos.Y, walkAfterSpawn: true, groundHeight);
			}
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		((TowerDefenseZombie)zombie).Walk();
	}

	public async void CreateDeathCharacter()
	{
		if ((Global.IsMultiplayerMode && !MultiPlayerManager.IsHost) || !TowerDefenseManager.TryTakeLatestDeathRecord(camp, requireAngelEligible: true, out var record))
		{
			return;
		}
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(RIVIVE, record.GridPosition);
		towerDefenseEffectSpriteOnce.GlobalPosition = record.Position;
		TowerDefenseManager.GetCharacterNode().CallDeferred("add_child", towerDefenseEffectSpriteOnce);
		TowerDefensePacketConfig towerDefensePacketConfig = record.Packet;
		TowerDefenseCharacter character = towerDefensePacketConfig.Create(record.Position, record.GridPosition);
		character.invisible = record.Invisible;
		TowerDefenseManager.GetCharacterNode().CallDeferred("add_child", character);
		if (GodotObject.IsInstanceValid(character.transformPoint))
		{
			character.transformPoint.Scale = (float)record.Scale * Vector2.One;
		}
		if (GodotObject.IsInstanceValid(character.instance))
		{
			character.instance.hitpointScale = record.HitpointScale;
		}
		if (instance.hypnoses)
		{
			character.Hypnoses();
		}
		if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
		{
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			if (GodotObject.IsInstanceValid(currentControl))
			{
				int nextSyncId = currentControl.GetNextSyncId();
				currentControl.RegisterSyncCharacter(nextSyncId, character);
				MultiPlayerManager.Instance.SendSpawnCharacterAt(towerDefensePacketConfig.saveKey, record.GridPosition.X, record.GridPosition.Y, nextSyncId, record.HitpointScale, record.Scale, instance.hypnoses, 0.0, useCreate: true, record.Position.X, record.Position.Y, walkAfterSpawn: true);
			}
		}
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		((TowerDefenseZombie)character).Walk();
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == "shooting")
		{
			CreateDeathCharacter();
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!(clip == "Revive"))
		{
			if (clip == "Shooting")
			{
				Walk();
			}
		}
		else
		{
			Walk();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(28)
		{
			new MethodInfo(MethodName.ConnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectRoleStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShootingAttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShootingAttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShootingAttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReviveAttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReviveAttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReviveAttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleWalkTransitionRetry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RetryWalkTransitionAfterReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RetryWalkTransitionOnNextFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelWalkTransitionRetry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectWalkReadySignal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.Blow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSelf, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateDeathCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ShootingAttackEntered && args.Count == 0)
		{
			ShootingAttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ShootingAttackProcessing && args.Count == 1)
		{
			ShootingAttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShootingAttackExited && args.Count == 0)
		{
			ShootingAttackExited();
			ret = default;
			return true;
		}
		if (method == MethodName.ReviveAttackEntered && args.Count == 0)
		{
			ReviveAttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.ReviveAttackProcessing && args.Count == 1)
		{
			ReviveAttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReviveAttackExited && args.Count == 0)
		{
			ReviveAttackExited();
			ret = default;
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleWalkTransitionRetry && args.Count == 0)
		{
			ScheduleWalkTransitionRetry();
			ret = default;
			return true;
		}
		if (method == MethodName.RetryWalkTransitionAfterReady && args.Count == 0)
		{
			RetryWalkTransitionAfterReady();
			ret = default;
			return true;
		}
		if (method == MethodName.RetryWalkTransitionOnNextFrame && args.Count == 0)
		{
			RetryWalkTransitionOnNextFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelWalkTransitionRetry && args.Count == 0)
		{
			CancelWalkTransitionRetry();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectWalkReadySignal && args.Count == 0)
		{
			DisconnectWalkReadySignal();
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
		if (method == MethodName.Blow && args.Count == 0)
		{
			Blow();
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
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSelf && args.Count == 1)
		{
			CreateSelf(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateDeathCharacter && args.Count == 0)
		{
			CreateDeathCharacter();
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
		if (method == MethodName.ShootingAttackEntered)
		{
			return true;
		}
		if (method == MethodName.ShootingAttackProcessing)
		{
			return true;
		}
		if (method == MethodName.ShootingAttackExited)
		{
			return true;
		}
		if (method == MethodName.ReviveAttackEntered)
		{
			return true;
		}
		if (method == MethodName.ReviveAttackProcessing)
		{
			return true;
		}
		if (method == MethodName.ReviveAttackExited)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.ScheduleWalkTransitionRetry)
		{
			return true;
		}
		if (method == MethodName.RetryWalkTransitionAfterReady)
		{
			return true;
		}
		if (method == MethodName.RetryWalkTransitionOnNextFrame)
		{
			return true;
		}
		if (method == MethodName.CancelWalkTransitionRetry)
		{
			return true;
		}
		if (method == MethodName.DisconnectWalkReadySignal)
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
		if (method == MethodName.Blow)
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
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.CreateSelf)
		{
			return true;
		}
		if (method == MethodName.CreateDeathCharacter)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._roleStateSignalsConnected)
		{
			_roleStateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._walkTransitionRetries)
		{
			_walkTransitionRetries = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._walkRetryTree)
		{
			_walkRetryTree = VariantUtils.ConvertTo<SceneTree>(in value);
			return true;
		}
		if (name == PropertyName._walkReadySignalConnected)
		{
			_walkReadySignalConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.speed)
		{
			speed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.isRevive)
		{
			isRevive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isReviveOver)
		{
			isReviveOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.isBlow)
		{
			isBlow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.spawnTimer)
		{
			spawnTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.invincible)
		{
			invincible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.invincibleTimer)
		{
			invincibleTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._roleStateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _roleStateSignalsConnected);
			return true;
		}
		if (name == PropertyName._walkTransitionRetries)
		{
			value = VariantUtils.CreateFrom(in _walkTransitionRetries);
			return true;
		}
		if (name == PropertyName._walkRetryTree)
		{
			value = VariantUtils.CreateFrom(in _walkRetryTree);
			return true;
		}
		if (name == PropertyName._walkReadySignalConnected)
		{
			value = VariantUtils.CreateFrom(in _walkReadySignalConnected);
			return true;
		}
		if (name == PropertyName.speed)
		{
			value = VariantUtils.CreateFrom(in speed);
			return true;
		}
		if (name == PropertyName.isRevive)
		{
			value = VariantUtils.CreateFrom(in isRevive);
			return true;
		}
		if (name == PropertyName.isReviveOver)
		{
			value = VariantUtils.CreateFrom(in isReviveOver);
			return true;
		}
		if (name == PropertyName.isBlow)
		{
			value = VariantUtils.CreateFrom(in isBlow);
			return true;
		}
		if (name == PropertyName.spawnTimer)
		{
			value = VariantUtils.CreateFrom(in spawnTimer);
			return true;
		}
		if (name == PropertyName.invincible)
		{
			value = VariantUtils.CreateFrom(in invincible);
			return true;
		}
		if (name == PropertyName.invincibleTimer)
		{
			value = VariantUtils.CreateFrom(in invincibleTimer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._roleStateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._walkTransitionRetries, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._walkRetryTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._walkReadySignalConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.speed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isRevive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isReviveOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isBlow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.spawnTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.invincible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.invincibleTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._roleStateSignalsConnected, Variant.From(in _roleStateSignalsConnected));
		info.AddProperty(PropertyName._walkTransitionRetries, Variant.From(in _walkTransitionRetries));
		info.AddProperty(PropertyName._walkRetryTree, Variant.From(in _walkRetryTree));
		info.AddProperty(PropertyName._walkReadySignalConnected, Variant.From(in _walkReadySignalConnected));
		info.AddProperty(PropertyName.speed, Variant.From(in speed));
		info.AddProperty(PropertyName.isRevive, Variant.From(in isRevive));
		info.AddProperty(PropertyName.isReviveOver, Variant.From(in isReviveOver));
		info.AddProperty(PropertyName.isBlow, Variant.From(in isBlow));
		info.AddProperty(PropertyName.spawnTimer, Variant.From(in spawnTimer));
		info.AddProperty(PropertyName.invincible, Variant.From(in invincible));
		info.AddProperty(PropertyName.invincibleTimer, Variant.From(in invincibleTimer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._roleStateSignalsConnected, out var value))
		{
			_roleStateSignalsConnected = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._walkTransitionRetries, out var value2))
		{
			_walkTransitionRetries = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._walkRetryTree, out var value3))
		{
			_walkRetryTree = value3.As<SceneTree>();
		}
		if (info.TryGetProperty(PropertyName._walkReadySignalConnected, out var value4))
		{
			_walkReadySignalConnected = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.speed, out var value5))
		{
			speed = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.isRevive, out var value6))
		{
			isRevive = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isReviveOver, out var value7))
		{
			isReviveOver = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.isBlow, out var value8))
		{
			isBlow = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.spawnTimer, out var value9))
		{
			spawnTimer = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName.invincible, out var value10))
		{
			invincible = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.invincibleTimer, out var value11))
		{
			invincibleTimer = value11.As<double>();
		}
	}
}
