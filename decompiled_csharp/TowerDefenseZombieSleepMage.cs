using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter10/SleepMage/Scene/TowerDefenseZombieSleepMage.cs")]
public class TowerDefenseZombieSleepMage : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public static readonly StringName ConnectStateSignals = "ConnectStateSignals";

		public static readonly StringName DisconnectStateSignals = "DisconnectStateSignals";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName CanCast = "CanCast";

		public new static readonly StringName Walk = "Walk";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public static readonly StringName TryStartCast = "TryStartCast";

		public static readonly StringName HasEnteredField = "HasEnteredField";

		public static readonly StringName CastEntered = "CastEntered";

		public static readonly StringName CastProcessing = "CastProcessing";

		public static readonly StringName CastExited = "CastExited";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName Fire = "Fire";

		public static readonly StringName EnterSleep = "EnterSleep";

		public static readonly StringName ApplySleepHeadVisual = "ApplySleepHeadVisual";

		public static readonly StringName RestoreHeadVisual = "RestoreHeadVisual";

		public static readonly StringName ProcessSleep = "ProcessSleep";

		public new static readonly StringName OnWakeUp = "OnWakeUp";

		public static readonly StringName WakeUpFromSleep = "WakeUpFromSleep";

		public static readonly StringName ApplyAwakenBlessing = "ApplyAwakenBlessing";

		public static readonly StringName ShowSleepIndicator = "ShowSleepIndicator";

		public static readonly StringName HideSleepIndicator = "HideSleepIndicator";

		public new static readonly StringName DieEntered = "DieEntered";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName spellProjectile = "spellProjectile";

		public static readonly StringName spellMuzzle = "spellMuzzle";

		public static readonly StringName spellSpent = "spellSpent";

		public static readonly StringName shooting = "shooting";

		public static readonly StringName shotPending = "shotPending";

		public static readonly StringName chantTimer = "chantTimer";

		public static readonly StringName entryTimer = "entryTimer";

		public static readonly StringName enteredField = "enteredField";

		public static readonly StringName sleepTriggered = "sleepTriggered";

		public static readonly StringName sleeping = "sleeping";

		public static readonly StringName sleepTimer = "sleepTimer";

		public static readonly StringName awakenApplied = "awakenApplied";

		public static readonly StringName _sleepHeadApplied = "_sleepHeadApplied";

		public static readonly StringName _stateSignalsConnected = "_stateSignalsConnected";

		public static readonly StringName _preserveCastOnRestore = "_preserveCastOnRestore";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const double EntryCastDelay = 5.0;

	private const double SleepDuration = 10.0;

	private const double AwakenHealHitpoints = 1000.0;

	private const double AwakenSpeedScale = 2.0;

	private const string HeadMediaName = "Zombie_SleepMage_head.png";

	private const string SleepHeadTexture = "uid://4xy3e7owb7hi";

	[Export(PropertyHint.None, "")]
	public TowerDefenseProjectileCreateData spellProjectile;

	[Export(PropertyHint.None, "")]
	public Marker2D spellMuzzle;

	public bool spellSpent;

	public bool shooting;

	public bool shotPending;

	public double chantTimer;

	public double entryTimer;

	public bool enteredField;

	public bool sleepTriggered;

	public bool sleeping;

	public double sleepTimer;

	public bool awakenApplied;

	private bool _sleepHeadApplied;

	private StateHandle _castStateHandle;

	private bool _stateSignalsConnected;

	private bool _preserveCastOnRestore;

	private void ConnectStateSignals()
	{
		if (_stateSignalsConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_castStateHandle = StateMachine.GetStateById("zombie.mage.cast");
			StateHandle castStateHandle = _castStateHandle;
			if (castStateHandle != null && castStateHandle.IsValid)
			{
				_castStateHandle.Entered += CastEntered;
				_castStateHandle.PhysicsProcessing += CastProcessing;
				_castStateHandle.Exited += CastExited;
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
			_stateSignalsConnected = false;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode)
		{
			ConnectStateSignals();
			if (sleeping)
			{
				ShowSleepIndicator();
				ApplySleepHeadVisual();
			}
		}
	}

	public override void _ExitTree()
	{
		DisconnectStateSignals();
		base._ExitTree();
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

	public override void Walk()
	{
		ActivateGameplayProcessing();
		StateHandle castStateHandle = _castStateHandle;
		if ((castStateHandle == null || !castStateHandle.IsActive || die || nearDie) && !sleeping)
		{
			base.Walk();
		}
	}

	public override void WalkProcessing(double delta)
	{
		if (sleeping)
		{
			ProcessSleep(delta);
			return;
		}
		TryStartCast(delta);
		base.WalkProcessing(delta);
	}

	private void TryStartCast(double delta)
	{
		if (spellSpent || sleepTriggered || sleeping)
		{
			return;
		}
		StateHandle castStateHandle = _castStateHandle;
		if (castStateHandle != null && castStateHandle.IsValid && !_castStateHandle.IsActive && CanCast() && HasEnteredField())
		{
			entryTimer += delta;
			if (!(entryTimer < 5.0))
			{
				SendStateEvent("ToMageCast");
			}
		}
	}

	private bool HasEnteredField()
	{
		if (enteredField)
		{
			return true;
		}
		if (!inGame || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return false;
		}
		if (!IsInsideComponentBattlefield)
		{
			return false;
		}
		enteredField = true;
		return true;
	}

	private bool TryFindSpellTarget(out TowerDefenseCharacter target)
	{
		target = null;
		if (!GodotObject.IsInstanceValid(instance) || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			return false;
		}
		TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
		if (!GodotObject.IsInstanceValid(mapFeature) || mapFeature.rect.Size.X <= 0f)
		{
			return false;
		}
		List<TowerDefenseCharacter> characterTargetFromRectList = TowerDefenseManager.Instance.GetCharacterTargetFromRectList(this, mapFeature.rect, checkLine: true);
		TowerDefenseCharacter towerDefenseCharacter = null;
		for (int i = 0; i < characterTargetFromRectList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter2 = characterTargetFromRectList[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter2) && !towerDefenseCharacter2.die && !towerDefenseCharacter2.isDestroy)
			{
				if (towerDefenseCharacter2 is TowerDefensePlant)
				{
					target = towerDefenseCharacter2;
					return true;
				}
				if (towerDefenseCharacter == null)
				{
					towerDefenseCharacter = towerDefenseCharacter2;
				}
			}
		}
		target = towerDefenseCharacter;
		return GodotObject.IsInstanceValid(target);
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
		shooting = false;
		chantTimer = 0.0;
		sprite.SetAnimation("Up", loop: false);
	}

	public void CastProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		if (sleeping)
		{
			ProcessSleep(delta);
		}
		else if (CanCast() && !shooting)
		{
			chantTimer += delta * timeScale;
			if (chantTimer >= 5.0)
			{
				shooting = true;
				sprite.SetAnimation("Shooting", loop: false);
			}
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
				Fire();
			}
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		StateHandle castStateHandle = _castStateHandle;
		if (castStateHandle == null || !castStateHandle.IsActive)
		{
			return;
		}
		if (!(clip == "Up"))
		{
			if (clip == "Shooting" && shooting && !die && !nearDie && !isDestroy)
			{
				EnterSleep();
			}
		}
		else if (!shooting)
		{
			sprite.SetAnimation("Magic");
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
			TryFindSpellTarget(out var target);
			TowerDefenseCharacter target2 = target;
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
			FireComponent.CreateProjectilePositionByData(this, target2, height, pos, velocity, projectileData, collisionFlags, cHARACTER_CAMP, default, overrides);
		}
	}

	private void EnterSleep()
	{
		if (!sleepTriggered)
		{
			sleepTriggered = true;
			sleeping = true;
			sleepTimer = 0.0;
			shooting = false;
			shotPending = false;
			startAttack = false;
			groundMoveComponent?.SetAlive(false);
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.SetAnimation(string.IsNullOrEmpty(sleepAnimeClip) ? idleAnimeClip : sleepAnimeClip);
			}
			ApplySleepHeadVisual();
			ShowSleepIndicator();
		}
	}

	private void ApplySleepHeadVisual()
	{
		if (!_sleepHeadApplied && GodotObject.IsInstanceValid(sprite))
		{
			_sleepHeadApplied = true;
			sprite.SetAtlasReplace("Zombie_SleepMage_head.png", "uid://4xy3e7owb7hi");
		}
	}

	private void RestoreHeadVisual()
	{
		if (_sleepHeadApplied)
		{
			_sleepHeadApplied = false;
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.SetAtlasReplace("Zombie_SleepMage_head.png", string.Empty);
			}
		}
	}

	private void ProcessSleep(double delta)
	{
		if (sleeping && TowerDefenseManager.HasGameplayAuthority)
		{
			sleepTimer += delta;
			if (!(sleepTimer < 10.0))
			{
				WakeUpFromSleep();
			}
		}
	}

	protected override void OnWakeUp()
	{
		base.OnWakeUp();
		if (sleeping)
		{
			WakeUpFromSleep();
		}
	}

	private void WakeUpFromSleep()
	{
		if (sleeping)
		{
			sleeping = false;
			sleepTimer = 0.0;
			RestoreHeadVisual();
			HideSleepIndicator();
			ApplyAwakenBlessing();
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.SetAnimation(walkAnimeClip);
			}
			base.Walk();
		}
	}

	private void ApplyAwakenBlessing()
	{
		if (!awakenApplied)
		{
			awakenApplied = true;
			if (GodotObject.IsInstanceValid(instance) && !instance.die && !instance.nearDie && TowerDefenseManager.HasGameplayAuthority)
			{
				Health(1000.0);
				timeScaleInit *= 2.0;
			}
		}
	}

	private void ShowSleepIndicator()
	{
		SleepComponent sleepRuntime = GetSleepRuntime();
		if (sleepRuntime != null)
		{
			sleepRuntime.ShowScriptedSleepIndicator();
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.QueueRedraw();
			}
		}
	}

	private void HideSleepIndicator()
	{
		GetSleepRuntime()?.HideScriptedSleepIndicator();
	}

	public override void DieEntered()
	{
		RestoreHeadVisual();
		HideSleepIndicator();
		base.DieEntered();
	}

	public override void DamagePointReach(string name)
	{
		base.DamagePointReach(name);
		if (!(name == "Arm"))
		{
			if (name == "Head" && damagePartSlot.ContainsKey("Head"))
			{
				DamagePartCreate("Head", null, new Vector2(-60f, -250f), keepSlotScale: true, default, fromSync: false, null, 0L);
			}
		}
		else if (damagePartSlot.ContainsKey("Arm"))
		{
			DamagePartCreate("Arm", null, new Vector2(60f, -200f), keepSlotScale: true, default, fromSync: false, null, 0L);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		Dictionary dictionary = base.ExportVariantSave();
		dictionary["mageSpellSpent"] = spellSpent;
		dictionary["mageShooting"] = shooting;
		dictionary["mageShotPending"] = shotPending;
		dictionary["mageChantElapsed"] = chantTimer;
		dictionary["mageEntryElapsed"] = entryTimer;
		dictionary["mageEnteredField"] = enteredField;
		dictionary["mageSleepTriggered"] = sleepTriggered;
		dictionary["mageSleeping"] = sleeping;
		dictionary["mageSleepElapsed"] = sleepTimer;
		dictionary["mageAwakenApplied"] = awakenApplied;
		return dictionary;
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		spellSpent = data.GetValueOrDefault("mageSpellSpent", false).AsBool();
		shooting = data.GetValueOrDefault("mageShooting", false).AsBool();
		shotPending = data.GetValueOrDefault("mageShotPending", false).AsBool();
		chantTimer = data.GetValueOrDefault("mageChantElapsed", 0.0).AsDouble();
		entryTimer = data.GetValueOrDefault("mageEntryElapsed", 0.0).AsDouble();
		enteredField = data.GetValueOrDefault("mageEnteredField", false).AsBool();
		sleepTriggered = data.GetValueOrDefault("mageSleepTriggered", false).AsBool();
		sleeping = data.GetValueOrDefault("mageSleeping", false).AsBool();
		sleepTimer = data.GetValueOrDefault("mageSleepElapsed", 0.0).AsDouble();
		awakenApplied = data.GetValueOrDefault("mageAwakenApplied", false).AsBool();
		_preserveCastOnRestore = shooting || shotPending;
		if (sleeping)
		{
			ApplySleepHeadVisual();
			ShowSleepIndicator();
		}
		else
		{
			RestoreHeadVisual();
			HideSleepIndicator();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(28)
		{
			new MethodInfo(MethodName.ConnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectStateSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanCast, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Walk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryStartCast, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasEnteredField, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Fire, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnterSleep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplySleepHeadVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreHeadVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessSleep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnWakeUp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WakeUpFromSleep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyAwakenBlessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowSleepIndicator, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideSleepIndicator, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DieEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CanCast && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCast());
			return true;
		}
		if (method == MethodName.Walk && args.Count == 0)
		{
			Walk();
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryStartCast && args.Count == 1)
		{
			TryStartCast(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasEnteredField && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasEnteredField());
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
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Fire && args.Count == 0)
		{
			Fire();
			ret = default;
			return true;
		}
		if (method == MethodName.EnterSleep && args.Count == 0)
		{
			EnterSleep();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySleepHeadVisual && args.Count == 0)
		{
			ApplySleepHeadVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreHeadVisual && args.Count == 0)
		{
			RestoreHeadVisual();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessSleep && args.Count == 1)
		{
			ProcessSleep(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnWakeUp && args.Count == 0)
		{
			OnWakeUp();
			ret = default;
			return true;
		}
		if (method == MethodName.WakeUpFromSleep && args.Count == 0)
		{
			WakeUpFromSleep();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyAwakenBlessing && args.Count == 0)
		{
			ApplyAwakenBlessing();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowSleepIndicator && args.Count == 0)
		{
			ShowSleepIndicator();
			ret = default;
			return true;
		}
		if (method == MethodName.HideSleepIndicator && args.Count == 0)
		{
			HideSleepIndicator();
			ret = default;
			return true;
		}
		if (method == MethodName.DieEntered && args.Count == 0)
		{
			DieEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.ConnectStateSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectStateSignals)
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
		if (method == MethodName.CanCast)
		{
			return true;
		}
		if (method == MethodName.Walk)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.TryStartCast)
		{
			return true;
		}
		if (method == MethodName.HasEnteredField)
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
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.Fire)
		{
			return true;
		}
		if (method == MethodName.EnterSleep)
		{
			return true;
		}
		if (method == MethodName.ApplySleepHeadVisual)
		{
			return true;
		}
		if (method == MethodName.RestoreHeadVisual)
		{
			return true;
		}
		if (method == MethodName.ProcessSleep)
		{
			return true;
		}
		if (method == MethodName.OnWakeUp)
		{
			return true;
		}
		if (method == MethodName.WakeUpFromSleep)
		{
			return true;
		}
		if (method == MethodName.ApplyAwakenBlessing)
		{
			return true;
		}
		if (method == MethodName.ShowSleepIndicator)
		{
			return true;
		}
		if (method == MethodName.HideSleepIndicator)
		{
			return true;
		}
		if (method == MethodName.DieEntered)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
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
		if (name == PropertyName.chantTimer)
		{
			chantTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.entryTimer)
		{
			entryTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.enteredField)
		{
			enteredField = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sleepTriggered)
		{
			sleepTriggered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sleeping)
		{
			sleeping = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.sleepTimer)
		{
			sleepTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.awakenApplied)
		{
			awakenApplied = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sleepHeadApplied)
		{
			_sleepHeadApplied = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.chantTimer)
		{
			value = VariantUtils.CreateFrom(in chantTimer);
			return true;
		}
		if (name == PropertyName.entryTimer)
		{
			value = VariantUtils.CreateFrom(in entryTimer);
			return true;
		}
		if (name == PropertyName.enteredField)
		{
			value = VariantUtils.CreateFrom(in enteredField);
			return true;
		}
		if (name == PropertyName.sleepTriggered)
		{
			value = VariantUtils.CreateFrom(in sleepTriggered);
			return true;
		}
		if (name == PropertyName.sleeping)
		{
			value = VariantUtils.CreateFrom(in sleeping);
			return true;
		}
		if (name == PropertyName.sleepTimer)
		{
			value = VariantUtils.CreateFrom(in sleepTimer);
			return true;
		}
		if (name == PropertyName.awakenApplied)
		{
			value = VariantUtils.CreateFrom(in awakenApplied);
			return true;
		}
		if (name == PropertyName._sleepHeadApplied)
		{
			value = VariantUtils.CreateFrom(in _sleepHeadApplied);
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.spellProjectile, PropertyHint.ResourceType, "TowerDefenseProjectileCreateData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.spellMuzzle, PropertyHint.NodeType, "Marker2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.spellSpent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.shooting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.shotPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.chantTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.entryTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enteredField, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.sleepTriggered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.sleeping, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.sleepTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.awakenApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sleepHeadApplied, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preserveCastOnRestore, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.spellProjectile, Variant.From(in spellProjectile));
		info.AddProperty(PropertyName.spellMuzzle, Variant.From(in spellMuzzle));
		info.AddProperty(PropertyName.spellSpent, Variant.From(in spellSpent));
		info.AddProperty(PropertyName.shooting, Variant.From(in shooting));
		info.AddProperty(PropertyName.shotPending, Variant.From(in shotPending));
		info.AddProperty(PropertyName.chantTimer, Variant.From(in chantTimer));
		info.AddProperty(PropertyName.entryTimer, Variant.From(in entryTimer));
		info.AddProperty(PropertyName.enteredField, Variant.From(in enteredField));
		info.AddProperty(PropertyName.sleepTriggered, Variant.From(in sleepTriggered));
		info.AddProperty(PropertyName.sleeping, Variant.From(in sleeping));
		info.AddProperty(PropertyName.sleepTimer, Variant.From(in sleepTimer));
		info.AddProperty(PropertyName.awakenApplied, Variant.From(in awakenApplied));
		info.AddProperty(PropertyName._sleepHeadApplied, Variant.From(in _sleepHeadApplied));
		info.AddProperty(PropertyName._stateSignalsConnected, Variant.From(in _stateSignalsConnected));
		info.AddProperty(PropertyName._preserveCastOnRestore, Variant.From(in _preserveCastOnRestore));
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
		if (info.TryGetProperty(PropertyName.spellSpent, out var value3))
		{
			spellSpent = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.shooting, out var value4))
		{
			shooting = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.shotPending, out var value5))
		{
			shotPending = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.chantTimer, out var value6))
		{
			chantTimer = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName.entryTimer, out var value7))
		{
			entryTimer = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.enteredField, out var value8))
		{
			enteredField = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sleepTriggered, out var value9))
		{
			sleepTriggered = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sleeping, out var value10))
		{
			sleeping = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.sleepTimer, out var value11))
		{
			sleepTimer = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName.awakenApplied, out var value12))
		{
			awakenApplied = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sleepHeadApplied, out var value13))
		{
			_sleepHeadApplied = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._stateSignalsConnected, out var value14))
		{
			_stateSignalsConnected = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._preserveCastOnRestore, out var value15))
		{
			_preserveCastOnRestore = value15.As<bool>();
		}
	}
}
