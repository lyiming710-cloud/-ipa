using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter10/TimeMage/Scene/TowerDefenseZombieTimeMage.cs")]
public class TowerDefenseZombieTimeMage : TowerDefenseZombie, INetworkSpawnStateReceiver
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName Walk = "Walk";

		public static readonly StringName CanCast = "CanCast";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public static readonly StringName SpellProcessing = "SpellProcessing";

		public static readonly StringName CastEntered = "CastEntered";

		public static readonly StringName CastProcessing = "CastProcessing";

		public static readonly StringName CastExited = "CastExited";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName Fire = "Fire";

		public static readonly StringName AppearEntered = "AppearEntered";

		public static readonly StringName SpawnAppearanceEffect = "SpawnAppearanceEffect";

		public static readonly StringName AppearProcessing = "AppearProcessing";

		public static readonly StringName ApplyRebornVisual = "ApplyRebornVisual";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName CompleteRewind = "CompleteRewind";

		public new static readonly StringName ExportNetworkSpawnState = "ExportNetworkSpawnState";

		public static readonly StringName ImportNetworkSpawnState = "ImportNetworkSpawnState";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName spellProjectile = "spellProjectile";

		public static readonly StringName spellMuzzle = "spellMuzzle";

		public static readonly StringName reviveEffect = "reviveEffect";

		public static readonly StringName entryStarted = "entryStarted";

		public static readonly StringName spellSpent = "spellSpent";

		public static readonly StringName shooting = "shooting";

		public static readonly StringName shotPending = "shotPending";

		public static readonly StringName spellTimer = "spellTimer";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _preserveCastOnRestore = "_preserveCastOnRestore";

		public static readonly StringName _reborn = "_reborn";

		public static readonly StringName _rewindPending = "_rewindPending";

		public static readonly StringName _rewindSpent = "_rewindSpent";

		public static readonly StringName _appearElapsed = "_appearElapsed";

		public static readonly StringName _appearanceEffectPlayed = "_appearanceEffectPlayed";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseProjectileCreateData spellProjectile;

	[Export(PropertyHint.None, "")]
	public Marker2D spellMuzzle;

	[Export(PropertyHint.None, "")]
	public PackedScene reviveEffect;

	public bool entryStarted;

	public bool spellSpent;

	public bool shooting;

	public bool shotPending;

	public double spellTimer;

	private StateHandle _castStateHandle;

	private bool _stateSignalsConnected;

	private bool _preserveCastOnRestore;

	private bool _reborn;

	private bool _rewindPending;

	private bool _rewindSpent;

	private double _appearElapsed = 1.0;

	private bool _appearanceEffectPlayed;

	private StateHandle _appearState;

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
		_castStateHandle = StateMachine.GetStateById("zombie.mage.cast");
		_appearState = StateMachine.GetStateById("zombie.time_mage.appear");
		StateHandle castStateHandle = _castStateHandle;
		if (castStateHandle != null && castStateHandle.IsValid)
		{
			StateHandle appearState = _appearState;
			if (appearState != null && appearState.IsValid)
			{
				_castStateHandle.Entered += CastEntered;
				_castStateHandle.PhysicsProcessing += CastProcessing;
				_castStateHandle.Exited += CastExited;
				_appearState.Entered += AppearEntered;
				_appearState.PhysicsProcessing += AppearProcessing;
				_stateSignalsConnected = true;
			}
		}
	}

	private void DisconnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			if (_castStateHandle != null)
			{
				_castStateHandle.Entered -= CastEntered;
				_castStateHandle.PhysicsProcessing -= CastProcessing;
				_castStateHandle.Exited -= CastExited;
			}
			_castStateHandle = null;
			if (_appearState != null)
			{
				_appearState.Entered -= AppearEntered;
				_appearState.PhysicsProcessing -= AppearProcessing;
			}
			_appearState = null;
			_stateSignalsConnected = false;
		}
	}

	public override void DamagePointReach(string name)
	{
		base.DamagePointReach(name);
		if (!(name == "Arm"))
		{
			if (!(name == "Head"))
			{
				return;
			}
			if (_reborn || suppressDeathrattles)
			{
				if (damagePartSlot.ContainsKey("Head"))
				{
					DamagePartCreate("Head", null, new Vector2(-60f, -250f), keepSlotScale: true, default, fromSync: false, null, 0L);
				}
			}
			else if (GodotObject.IsInstanceValid(sprite))
			{
				string[] array = new string[4] { "anim_head1", "anim_head2", "anim_innerhat", "anim_outerhat" };
				foreach (string text in array)
				{
					sprite.SetFliter(text, open: true);
				}
			}
		}
		else if (damagePartSlot.ContainsKey("Arm"))
		{
			DamagePartCreate("Arm", null, new Vector2(60f, -200f), keepSlotScale: true, default, fromSync: false, null, 0L);
		}
	}

	public override void _Ready()
	{
		base._Ready();
		ApplyRebornVisual();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			ConnectStateSignals();
		}
	}

	public override void Walk()
	{
		ActivateGameplayProcessing();
		StateHandle castStateHandle = _castStateHandle;
		if (castStateHandle != null && castStateHandle.IsActive && !die && !nearDie)
		{
			return;
		}
		if (_reborn && _appearElapsed < 1.0 && !die)
		{
			StateHandle appearState = _appearState;
			if (appearState != null && appearState.IsValid)
			{
				if (!_appearState.IsActive)
				{
					SendStateEvent("ToMageAppear");
				}
				return;
			}
		}
		entryStarted = true;
		base.Walk();
	}

	private bool CanCast()
	{
		if (Engine.IsEditorHint() || editorPreviewMode || !inGame || die || nearDie || isDestroy)
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(sprite) || sprite.pause)
		{
			return false;
		}
		return TowerDefenseManager.HasGameplayAuthority;
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
		SpellProcessing(delta);
	}

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		SpellProcessing(delta);
	}

	private void SpellProcessing(double delta)
	{
		if (!entryStarted)
		{
			return;
		}
		StateHandle castStateHandle = _castStateHandle;
		if ((castStateHandle == null || !castStateHandle.IsActive) && CanCast())
		{
			spellTimer += delta * timeScale;
			if (spellTimer >= 10.0)
			{
				SendStateEvent("ToMageCast");
			}
		}
	}

	public void CastEntered()
	{
		groundMoveComponent?.SetAlive(false);
		startAttack = false;
		if (_preserveCastOnRestore)
		{
			_preserveCastOnRestore = false;
			return;
		}
		shotPending = true;
		shooting = true;
		sprite.SetAnimation("Shooting", loop: false);
	}

	public void CastProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		if (CanCast() && !shotPending)
		{
			spellTimer += delta * timeScale;
		}
	}

	public void CastExited()
	{
		shotPending = false;
		shooting = false;
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == "fire")
		{
			StateHandle castStateHandle = _castStateHandle;
			if (castStateHandle != null && castStateHandle.IsActive && shooting && shotPending && CanCast())
			{
				shotPending = false;
				spellSpent = true;
				spellTimer = 0.0;
				Fire();
			}
		}
	}

	public void Fire()
	{
		if (spellProjectile != null && TowerDefenseManager.HasGameplayAuthority)
		{
			if (GodotObject.IsInstanceValid(spellMuzzle) && spellMuzzle.GetParent() is AdobeAnimateSlot adobeAnimateSlot)
			{
				adobeAnimateSlot.Update();
			}
			Vector2 pos = (GodotObject.IsInstanceValid(spellMuzzle) ? GetLogicalGlobalPosition(spellMuzzle) : (GetLogicalGlobalPosition() + new Vector2(0f, -25f)));
			bool value = Scale.X * transformPoint.Scale.X * sprite.Scale.X >= 0f;
			double height = groundHeight;
			Vector2 velocity = new Vector2(-240f, 0f);
			TowerDefenseProjectileCreateData projectileData = spellProjectile;
			int collisionFlags = instance.collisionFlags;
			TowerDefenseEnum.CHARACTER_CAMP cHARACTER_CAMP = camp;
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				gridYOverride = gridPos.Y,
				flipXOverride = value,
				deprioritizeDisabledTargets = true
			};
			FireComponent.CreateProjectilePositionByData(this, null, height, pos, velocity, projectileData, collisionFlags, cHARACTER_CAMP, default, overrides);
		}
	}

	public void AppearEntered()
	{
		groundMoveComponent?.SetAlive(false);
		startAttack = false;
		sprite.SetAnimation("Idle");
		ApplyRebornVisual();
		SpawnAppearanceEffect();
	}

	private void SpawnAppearanceEffect()
	{
		if (!_appearanceEffectPlayed && reviveEffect != null && !(_appearElapsed >= 1.0) && GetParent() is Node2D node2D)
		{
			_appearanceEffectPlayed = true;
			Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
			if (!TowerDefenseManager.TryCreateEffectSceneOnceFast(reviveEffect, node2D, gridPos, logicalGlobalPosition, preferSprite: true))
			{
				TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(reviveEffect, gridPos, "Fire");
				node2D.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
				towerDefenseEffectSpriteOnce.GlobalPosition = logicalGlobalPosition;
			}
		}
	}

	public void AppearProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		if (!sprite.pause && inGame && !die && !nearDie)
		{
			_appearElapsed = Math.Min(1.0, _appearElapsed + delta * timeScale);
			ApplyRebornVisual();
			if (_appearElapsed >= 1.0 && TowerDefenseManager.HasGameplayAuthority)
			{
				entryStarted = true;
				base.Walk();
			}
		}
	}

	private void ApplyRebornVisual()
	{
		string text = (dieWaterAnimeClip = ((_reborn || suppressDeathrattles) ? "DeathDeal" : "Death"));
		dieAnimeClip = text;
		if (_reborn && GodotObject.IsInstanceValid(sprite))
		{
			sprite.SetFliter("watch", open: false);
			if (GodotObject.IsInstanceValid(transformPoint))
			{
				transformPoint.Modulate = new Color(1f, 1f, 1f, (float)_appearElapsed);
			}
		}
	}

	public override void DieEntered()
	{
		ApplyRebornVisual();
		if (!_reborn && !_rewindSpent && !suppressDeathrattles && inGame && TowerDefenseManager.HasGameplayAuthority)
		{
			_rewindPending = true;
		}
		base.DieEntered();
	}

	public override void AnimeCompleted(string clip)
	{
		if (!(clip == "Shooting"))
		{
			if (clip == "Death" && _rewindPending && !_rewindSpent && TowerDefenseManager.HasGameplayAuthority)
			{
				CompleteRewind();
			}
		}
		else
		{
			StateHandle castStateHandle = _castStateHandle;
			if (castStateHandle != null && castStateHandle.IsActive && shooting && CanCast())
			{
				base.Walk();
			}
		}
		base.AnimeCompleted(clip);
	}

	private void CompleteRewind()
	{
		_rewindSpent = true;
		_rewindPending = false;
		TowerDefenseManager towerDefenseManager = TowerDefenseManager.Instance;
		Node2D node2D = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(packet))
		{
			return;
		}
		string saveKey = packet.saveKey;
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(saveKey);
		if (!GodotObject.IsInstanceValid(towerDefenseManager) || !GodotObject.IsInstanceValid(node2D) || packetConfig == null || suppressDeathrattles)
		{
			return;
		}
		bool hypnotized = instance.hypnoses;
		double hitpointScale = (GodotObject.IsInstanceValid(instance) ? instance.hitpointScale : 1.0);
		Vector2 bodyScale = (GodotObject.IsInstanceValid(transformPoint) ? transformPoint.Scale : Vector2.One);
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		logicalGlobalPosition.X = (float)(hypnotized ? towerDefenseManager.GetMapGroundLeft() : towerDefenseManager.GetMapGroundRight());
		Vector2I vector2I = new Vector2I(hypnotized ? 1 : towerDefenseManager.GetMapGridNum().X, gridPos.Y);
		TowerDefenseCharacter towerDefenseCharacter = (EconomyOwnerAccountId.IsValid ? packetConfig.Create(EconomyOwnerAccountId, logicalGlobalPosition, vector2I, groundHeight) : packetConfig.Create(logicalGlobalPosition, vector2I, groundHeight));
		TowerDefenseZombieTimeMage next = towerDefenseCharacter as TowerDefenseZombieTimeMage;
		if (next == null)
		{
			towerDefenseCharacter?.QueueFree();
			return;
		}
		next._reborn = true;
		next._rewindSpent = true;
		next._appearElapsed = 0.0;
		next.Ready += () =>
		{
			next.SetHitpointAndScale(hitpointScale, bodyScale);
			if (hypnotized)
			{
				next.Hypnoses();
			}
			else
			{
				next.camp = camp;
			}
			next.WalkReady();
			TowerDefenseBattleFeatureWave.Instance?.AddSpawnCharacter(next);
		};
		node2D.AddChild(next, forceReadableName: false, InternalMode.Disabled);
		TowerDefenseManager.PublishSpawnedCharacter(saveKey, next, useCreate: true, 0.0, walkAfterSpawn: true, "", next.ExportNetworkSpawnState());
	}

	public override Dictionary ExportNetworkSpawnState()
	{
		Dictionary dictionary = base.ExportNetworkSpawnState();
		dictionary["timeMageReborn"] = _reborn;
		dictionary["timeMageAppearElapsed"] = _appearElapsed;
		return dictionary;
	}

	public void ImportNetworkSpawnState(Dictionary data)
	{
		_reborn = data.GetValueOrDefault("timeMageReborn", false).AsBool();
		_appearElapsed = data.GetValueOrDefault("timeMageAppearElapsed", 1.0).AsDouble();
		_rewindSpent |= _reborn;
		ApplyRebornVisual();
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["mageEntryStarted"] = entryStarted;
		dictionary["mageSpellSpent"] = spellSpent;
		dictionary["mageShooting"] = shooting;
		dictionary["mageShotPending"] = shotPending;
		dictionary["mageSpellElapsed"] = spellTimer;
		dictionary.Merge(ExportNetworkSpawnState(), overwrite: true);
		dictionary["timeMageRewindPending"] = _rewindPending;
		dictionary["timeMageRewindSpent"] = _rewindSpent;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		entryStarted = data.GetValueOrDefault("mageEntryStarted", false).AsBool();
		spellSpent = data.GetValueOrDefault("mageSpellSpent", false).AsBool();
		shooting = data.GetValueOrDefault("mageShooting", false).AsBool();
		shotPending = data.GetValueOrDefault("mageShotPending", false).AsBool();
		spellTimer = data.GetValueOrDefault("mageSpellElapsed", 0.0).AsDouble();
		_preserveCastOnRestore = shooting || shotPending;
		ImportNetworkSpawnState(data);
		_rewindPending = data.GetValueOrDefault("timeMageRewindPending", false).AsBool();
		_rewindSpent = data.GetValueOrDefault("timeMageRewindSpent", _reborn).AsBool();
	}

	public override void _ExitTree()
	{
		DisconnectStateSignals();
		base._ExitTree();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(26)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanCast, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpellProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CastEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CastProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CastExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.Fire, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AppearEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnAppearanceEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AppearProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRebornVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CompleteRewind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportNetworkSpawnState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportNetworkSpawnState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.CanCast && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCast());
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
		if (method == MethodName.SpellProcessing && args.Count == 1)
		{
			SpellProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CastEntered && args.Count == 0)
		{
			CastEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.CastProcessing && args.Count == 1)
		{
			CastProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CastExited && args.Count == 0)
		{
			CastExited();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Fire && args.Count == 0)
		{
			Fire();
			ret = default;
			return true;
		}
		if (method == MethodName.AppearEntered && args.Count == 0)
		{
			AppearEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnAppearanceEffect && args.Count == 0)
		{
			SpawnAppearanceEffect();
			ret = default;
			return true;
		}
		if (method == MethodName.AppearProcessing && args.Count == 1)
		{
			AppearProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRebornVisual && args.Count == 0)
		{
			ApplyRebornVisual();
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
		if (method == MethodName.CompleteRewind && args.Count == 0)
		{
			CompleteRewind();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportNetworkSpawnState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportNetworkSpawnState());
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState && args.Count == 1)
		{
			ImportNetworkSpawnState(VariantUtils.ConvertTo<Dictionary>(in args[0]));
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
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
		if (method == MethodName.DamagePointReach)
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
		if (method == MethodName.CanCast)
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
		if (method == MethodName.SpellProcessing)
		{
			return true;
		}
		if (method == MethodName.CastEntered)
		{
			return true;
		}
		if (method == MethodName.CastProcessing)
		{
			return true;
		}
		if (method == MethodName.CastExited)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.Fire)
		{
			return true;
		}
		if (method == MethodName.AppearEntered)
		{
			return true;
		}
		if (method == MethodName.SpawnAppearanceEffect)
		{
			return true;
		}
		if (method == MethodName.AppearProcessing)
		{
			return true;
		}
		if (method == MethodName.ApplyRebornVisual)
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
		if (method == MethodName.CompleteRewind)
		{
			return true;
		}
		if (method == MethodName.ExportNetworkSpawnState)
		{
			return true;
		}
		if (method == MethodName.ImportNetworkSpawnState)
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.spellProjectile)
		{
			spellProjectile = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		if (name == PropertyName.spellMuzzle)
		{
			spellMuzzle = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName.reviveEffect)
		{
			reviveEffect = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.entryStarted)
		{
			entryStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.spellSpent)
		{
			spellSpent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.shooting)
		{
			shooting = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.shotPending)
		{
			shotPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.spellTimer)
		{
			spellTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			_stateSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._preserveCastOnRestore)
		{
			_preserveCastOnRestore = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._reborn)
		{
			_reborn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._rewindPending)
		{
			_rewindPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._rewindSpent)
		{
			_rewindSpent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._appearElapsed)
		{
			_appearElapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._appearanceEffectPlayed)
		{
			_appearanceEffectPlayed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.spellProjectile)
		{
			value = VariantUtils.CreateFrom(in spellProjectile);
			return true;
		}
		if (name == PropertyName.spellMuzzle)
		{
			value = VariantUtils.CreateFrom(in spellMuzzle);
			return true;
		}
		if (name == PropertyName.reviveEffect)
		{
			value = VariantUtils.CreateFrom(in reviveEffect);
			return true;
		}
		if (name == PropertyName.entryStarted)
		{
			value = VariantUtils.CreateFrom(in entryStarted);
			return true;
		}
		if (name == PropertyName.spellSpent)
		{
			value = VariantUtils.CreateFrom(in spellSpent);
			return true;
		}
		if (name == PropertyName.shooting)
		{
			value = VariantUtils.CreateFrom(in shooting);
			return true;
		}
		if (name == PropertyName.shotPending)
		{
			value = VariantUtils.CreateFrom(in shotPending);
			return true;
		}
		if (name == PropertyName.spellTimer)
		{
			value = VariantUtils.CreateFrom(in spellTimer);
			return true;
		}
		if (name == PropertyName._stateSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _stateSignalsConnected);
			return true;
		}
		if (name == PropertyName._preserveCastOnRestore)
		{
			value = VariantUtils.CreateFrom(in _preserveCastOnRestore);
			return true;
		}
		if (name == PropertyName._reborn)
		{
			value = VariantUtils.CreateFrom(in _reborn);
			return true;
		}
		if (name == PropertyName._rewindPending)
		{
			value = VariantUtils.CreateFrom(in _rewindPending);
			return true;
		}
		if (name == PropertyName._rewindSpent)
		{
			value = VariantUtils.CreateFrom(in _rewindSpent);
			return true;
		}
		if (name == PropertyName._appearElapsed)
		{
			value = VariantUtils.CreateFrom(in _appearElapsed);
			return true;
		}
		if (name == PropertyName._appearanceEffectPlayed)
		{
			value = VariantUtils.CreateFrom(in _appearanceEffectPlayed);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.spellProjectile, PropertyHint.ResourceType, "TowerDefenseProjectileCreateData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.spellMuzzle, PropertyHint.NodeType, "Marker2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.reviveEffect, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.entryStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.spellSpent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.shooting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.shotPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.spellTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preserveCastOnRestore, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._reborn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._rewindPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._rewindSpent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._appearElapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._appearanceEffectPlayed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.spellProjectile, Variant.From(in spellProjectile));
		info.AddProperty(PropertyName.spellMuzzle, Variant.From(in spellMuzzle));
		info.AddProperty(PropertyName.reviveEffect, Variant.From(in reviveEffect));
		info.AddProperty(PropertyName.entryStarted, Variant.From(in entryStarted));
		info.AddProperty(PropertyName.spellSpent, Variant.From(in spellSpent));
		info.AddProperty(PropertyName.shooting, Variant.From(in shooting));
		info.AddProperty(PropertyName.shotPending, Variant.From(in shotPending));
		info.AddProperty(PropertyName.spellTimer, Variant.From(in spellTimer));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._preserveCastOnRestore, Variant.From(in _preserveCastOnRestore));
		info.AddProperty(PropertyName._reborn, Variant.From(in _reborn));
		info.AddProperty(PropertyName._rewindPending, Variant.From(in _rewindPending));
		info.AddProperty(PropertyName._rewindSpent, Variant.From(in _rewindSpent));
		info.AddProperty(PropertyName._appearElapsed, Variant.From(in _appearElapsed));
		info.AddProperty(PropertyName._appearanceEffectPlayed, Variant.From(in _appearanceEffectPlayed));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.spellProjectile, out var value))
		{
			spellProjectile = value.As<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName.spellMuzzle, out var value2))
		{
			spellMuzzle = value2.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName.reviveEffect, out var value3))
		{
			reviveEffect = value3.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.entryStarted, out var value4))
		{
			entryStarted = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.spellSpent, out var value5))
		{
			spellSpent = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.shooting, out var value6))
		{
			shooting = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.shotPending, out var value7))
		{
			shotPending = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.spellTimer, out var value8))
		{
			spellTimer = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value9))
		{
			_stateSignalsConnected = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._preserveCastOnRestore, out var value10))
		{
			_preserveCastOnRestore = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._reborn, out var value11))
		{
			_reborn = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rewindPending, out var value12))
		{
			_rewindPending = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._rewindSpent, out var value13))
		{
			_rewindSpent = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._appearElapsed, out var value14))
		{
			_appearElapsed = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName._appearanceEffectPlayed, out var value15))
		{
			_appearanceEffectPlayed = value15.As<bool>();
		}
	}
}
