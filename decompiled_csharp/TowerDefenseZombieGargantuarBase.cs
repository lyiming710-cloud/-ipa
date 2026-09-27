using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Prefab/TowerDefense/Character/TowerDefenseZombieGargantuarBase.cs")]
public class TowerDefenseZombieGargantuarBase : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ConnectFireStateHandle = "ConnectFireStateHandle";

		public static readonly StringName DisconnectFireStateHandle = "DisconnectFireStateHandle";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName WalkProcessing = "WalkProcessing";

		public new static readonly StringName AttackEntered = "AttackEntered";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName AttackExited = "AttackExited";

		public static readonly StringName FireEntered = "FireEntered";

		public static readonly StringName FireProcessing = "FireProcessing";

		public static readonly StringName FireExited = "FireExited";

		public static readonly StringName Fire = "Fire";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName AnimeEvent = "AnimeEvent";

		public static readonly StringName CaptureSmashCycleTarget = "CaptureSmashCycleTarget";

		public static readonly StringName ShouldCancelSmashForBlowBack = "ShouldCancelSmashForBlowBack";

		public static readonly StringName CancelSmashForBlowBack = "CancelSmashForBlowBack";

		public static readonly StringName IsCommittedSmashTargetValid = "IsCommittedSmashTargetValid";

		public static readonly StringName SetImpFilter = "SetImpFilter";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName impFilter = "impFilter";

		public static readonly StringName fireAnimeClip = "fireAnimeClip";

		public static readonly StringName smashAnimeEvent = "smashAnimeEvent";

		public static readonly StringName impFireEvent = "impFireEvent";

		public static readonly StringName impName = "impName";

		public static readonly StringName impSpawnSlot = "impSpawnSlot";

		public static readonly StringName _impFilter = "_impFilter";

		public static readonly StringName impFliters = "impFliters";

		public static readonly StringName impThrowDamagePointName = "impThrowDamagePointName";

		public static readonly StringName impThrowFlag = "impThrowFlag";

		public static readonly StringName _smashCycleTarget = "_smashCycleTarget";

		public static readonly StringName _fireStateHandleConnected = "_fireStateHandleConnected";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string fireAnimeClip = "Fire";

	[Export(PropertyHint.None, "")]
	public string smashAnimeEvent = "smash";

	[Export(PropertyHint.None, "")]
	public string impFireEvent = "fire";

	[Export(PropertyHint.None, "")]
	public string impName = "";

	[Export(PropertyHint.None, "")]
	public AdobeAnimateSlot impSpawnSlot;

	private string _impFilter = "";

	public Array<string> impFliters = new Array<string>();

	[Export(PropertyHint.None, "")]
	public string impThrowDamagePointName = "ThrowImp";

	public bool impThrowFlag;

	public ImpThrowerComponent impThrowerComponent;

	public GargantuarSmashComponent gargantuarSmashComponent;

	private TowerDefenseCharacter _smashCycleTarget;

	private StateHandle _fireStateHandle;

	private bool _fireStateHandleConnected;

	[Export(PropertyHint.MultilineText, "")]
	public string impFilter
	{
		get
		{
			return _impFilter;
		}
		set
		{
			_impFilter = value;
			impFliters = new Array<string>(_impFilter.Split("&", StringSplitOptions.RemoveEmptyEntries));
		}
	}

	public override void _Ready()
	{
		if (!Engine.IsEditorHint())
		{
			base._Ready();
			if (GodotObject.IsInstanceValid(componentManager))
			{
				impThrowerComponent = componentManager.GetRuntime<ImpThrowerComponent>();
				gargantuarSmashComponent = componentManager.GetRuntime<GargantuarSmashComponent>();
			}
			ConnectFireStateHandle();
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	private void ConnectFireStateHandle()
	{
		if (_fireStateHandleConnected)
		{
			return;
		}
		IStateMachineController stateMachine = StateMachine;
		if (stateMachine != null && stateMachine.IsInitialized)
		{
			_fireStateHandle = StateMachine?.GetStateById("gargantuar.fire");
			StateHandle fireStateHandle = _fireStateHandle;
			if (fireStateHandle != null && fireStateHandle.IsValid)
			{
				_fireStateHandle.Entered += FireEntered;
				_fireStateHandle.Exited += FireExited;
				_fireStateHandle.PhysicsProcessing += FireProcessing;
				_fireStateHandleConnected = true;
			}
		}
	}

	private void DisconnectFireStateHandle()
	{
		if (_fireStateHandleConnected && _fireStateHandle != null)
		{
			_fireStateHandle.Entered -= FireEntered;
			_fireStateHandle.Exited -= FireExited;
			_fireStateHandle.PhysicsProcessing -= FireProcessing;
		}
		_fireStateHandle = null;
		_fireStateHandleConnected = false;
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		if (IsInsideComponentBattlefield && impThrowFlag)
		{
			Fire();
		}
	}

	public override void WalkProcessing(double delta)
	{
		base.WalkProcessing(delta);
		if (IsInsideComponentBattlefield && impThrowFlag)
		{
			Fire();
		}
	}

	public override void AttackEntered()
	{
		CaptureSmashCycleTarget();
		sprite.SetAnimation(attackAnimeClip, loop: true, 0.2);
	}

	public override void AttackProcessing(double delta)
	{
		if (TowerDefenseZombie.IsZombieRewindActive())
		{
			_smashCycleTarget = null;
			Walk();
		}
		else if (ShouldCancelSmashForBlowBack())
		{
			CancelSmashForBlowBack();
		}
		else
		{
			sprite.timeScale = timeScale;
		}
	}

	public override void AttackExited()
	{
		_smashCycleTarget = null;
		base.AttackExited();
	}

	public void FireEntered()
	{
		sprite.SetAnimation(fireAnimeClip, loop: false, 0.2);
	}

	public void FireProcessing(double delta)
	{
		sprite.timeScale = timeScale;
	}

	public void FireExited()
	{
	}

	public void Fire()
	{
		SendStateEvent("ToFire");
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == attackAnimeClip)
		{
			AttackComponent attackComponent = base.attackComponent;
			bool flag = attackComponent != null && !attackComponent.IsReleased && base.attackComponent.CanAttack();
			if (!flag)
			{
				AttackComponent attackComponent2 = base.attackComponent;
				if (attackComponent2 != null && !attackComponent2.IsReleased)
				{
					flag = base.attackComponent.TryRetargetImmediately();
				}
			}
			if (!flag)
			{
				_smashCycleTarget = null;
				Walk();
			}
			else
			{
				CaptureSmashCycleTarget();
			}
		}
		else if (clip == fireAnimeClip)
		{
			Walk();
		}
		else if (clip == dieAnimeClip)
		{
			AudioManager.Instance.AudioPlay("GargantuarThump");
		}
	}

	public override void AnimeEvent(string command, Variant argument)
	{
		base.AnimeEvent(command, argument);
		if (command == smashAnimeEvent)
		{
			if (IsCommittedSmashTargetValid(_smashCycleTarget))
			{
				base.attackComponent.target = _smashCycleTarget;
			}
			else
			{
				AttackComponent attackComponent = base.attackComponent;
				if (attackComponent != null && !attackComponent.IsReleased && base.attackComponent.TryRetargetImmediately())
				{
					_smashCycleTarget = base.attackComponent.target;
				}
				else
				{
					_smashCycleTarget = null;
					AttackComponent attackComponent2 = base.attackComponent;
					if (attackComponent2 != null && !attackComponent2.IsReleased)
					{
						base.attackComponent.target = null;
					}
				}
			}
			gargantuarSmashComponent?.SmashAttack();
			_smashCycleTarget = null;
		}
		else if (command == impFireEvent && impThrowFlag)
		{
			impThrowFlag = false;
			impThrowerComponent?.SetImpFilter();
			impThrowerComponent?.SpawnImp();
		}
	}

	private void CaptureSmashCycleTarget()
	{
		AttackComponent attackComponent = base.attackComponent;
		TowerDefenseCharacter towerDefenseCharacter = ((attackComponent != null && !attackComponent.IsReleased) ? base.attackComponent.target : null);
		_smashCycleTarget = (IsCommittedSmashTargetValid(towerDefenseCharacter) ? towerDefenseCharacter : null);
	}

	private bool ShouldCancelSmashForBlowBack()
	{
		BlowBackComponent blowBackComponent = base.blowBackComponent;
		if (blowBackComponent != null && !blowBackComponent.IsReleased)
		{
			return base.blowBackComponent.blowBack;
		}
		return false;
	}

	private void CancelSmashForBlowBack()
	{
		_smashCycleTarget = null;
		AttackComponent attackComponent = base.attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			base.attackComponent.target = null;
		}
		WalkWithoutTransitionDelay();
	}

	private bool IsCommittedSmashTargetValid(TowerDefenseCharacter candidate)
	{
		if (!GodotObject.IsInstanceValid(candidate) || !GodotObject.IsInstanceValid(candidate.instance))
		{
			return false;
		}
		if (candidate.die || candidate.nearDie || candidate.isDestroy)
		{
			return false;
		}
		if (candidate.instance.invincible || !candidate.instance.canBeCollection)
		{
			return false;
		}
		if (!candidate.HasHitBox)
		{
			return false;
		}
		if (!CanTarget(candidate) || !CanCollision(candidate.instance.maskFlags))
		{
			return false;
		}
		if (!candidate.IsTargetableFromLine(gridPos.Y))
		{
			return false;
		}
		AttackComponent attackComponent = base.attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased && (double)candidate.GetLogicalGlobalPosition().X > base.attackComponent.groundRight)
		{
			return false;
		}
		return true;
	}

	public void SetImpFilter(bool open = false)
	{
		impThrowerComponent?.SetImpFilter(open);
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		if (damagePointName == impThrowDamagePointName)
		{
			TowerDefenseBattleFeatureMap mapFeature = TowerDefenseManager.GetMapFeature();
			if (mapFeature != null && (double)GetLogicalGlobalPosition().X > (double)mapFeature.config.edge.X + (double)TowerDefenseManager.Instance.GetMapGridSize().X * 5.0)
			{
				impThrowFlag = true;
			}
		}
		else if (damagePointName == dieAnimeClip)
		{
			AudioManager.Instance.AudioPlay("GargantuarDeath");
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { ["impThrowFlag"] = impThrowFlag };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		impThrowFlag = data.GetValueOrDefault("impThrowFlag", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(23)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectFireStateHandle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectFireStateHandle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WalkProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttackExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FireEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FireProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FireExited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Fire, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AnimeEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "command", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "argument", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureSmashCycleTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldCancelSmashForBlowBack, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelSmashForBlowBack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsCommittedSmashTargetValid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "candidate", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetImpFilter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ConnectFireStateHandle && args.Count == 0)
		{
			ConnectFireStateHandle();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectFireStateHandle && args.Count == 0)
		{
			DisconnectFireStateHandle();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WalkProcessing && args.Count == 1)
		{
			WalkProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackEntered && args.Count == 0)
		{
			AttackEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttackExited && args.Count == 0)
		{
			AttackExited();
			ret = default;
			return true;
		}
		if (method == MethodName.FireEntered && args.Count == 0)
		{
			FireEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.FireProcessing && args.Count == 1)
		{
			FireProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FireExited && args.Count == 0)
		{
			FireExited();
			ret = default;
			return true;
		}
		if (method == MethodName.Fire && args.Count == 0)
		{
			Fire();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeEvent && args.Count == 2)
		{
			AnimeEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureSmashCycleTarget && args.Count == 0)
		{
			CaptureSmashCycleTarget();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldCancelSmashForBlowBack && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldCancelSmashForBlowBack());
			return true;
		}
		if (method == MethodName.CancelSmashForBlowBack && args.Count == 0)
		{
			CancelSmashForBlowBack();
			ret = default;
			return true;
		}
		if (method == MethodName.IsCommittedSmashTargetValid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCommittedSmashTargetValid(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.SetImpFilter && args.Count == 1)
		{
			SetImpFilter(VariantUtils.ConvertTo<bool>(in args[0]));
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
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ConnectFireStateHandle)
		{
			return true;
		}
		if (method == MethodName.DisconnectFireStateHandle)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.WalkProcessing)
		{
			return true;
		}
		if (method == MethodName.AttackEntered)
		{
			return true;
		}
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.AttackExited)
		{
			return true;
		}
		if (method == MethodName.FireEntered)
		{
			return true;
		}
		if (method == MethodName.FireProcessing)
		{
			return true;
		}
		if (method == MethodName.FireExited)
		{
			return true;
		}
		if (method == MethodName.Fire)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.AnimeEvent)
		{
			return true;
		}
		if (method == MethodName.CaptureSmashCycleTarget)
		{
			return true;
		}
		if (method == MethodName.ShouldCancelSmashForBlowBack)
		{
			return true;
		}
		if (method == MethodName.CancelSmashForBlowBack)
		{
			return true;
		}
		if (method == MethodName.IsCommittedSmashTargetValid)
		{
			return true;
		}
		if (method == MethodName.SetImpFilter)
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
		if (name == PropertyName.impFilter)
		{
			impFilter = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.fireAnimeClip)
		{
			fireAnimeClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.smashAnimeEvent)
		{
			smashAnimeEvent = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.impFireEvent)
		{
			impFireEvent = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.impName)
		{
			impName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.impSpawnSlot)
		{
			impSpawnSlot = VariantUtils.ConvertTo<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._impFilter)
		{
			_impFilter = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.impFliters)
		{
			impFliters = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.impThrowDamagePointName)
		{
			impThrowDamagePointName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.impThrowFlag)
		{
			impThrowFlag = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._smashCycleTarget)
		{
			_smashCycleTarget = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._fireStateHandleConnected)
		{
			_fireStateHandleConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.impFilter)
		{
			value = VariantUtils.CreateFrom<string>(impFilter);
			return true;
		}
		if (name == PropertyName.fireAnimeClip)
		{
			value = VariantUtils.CreateFrom(in fireAnimeClip);
			return true;
		}
		if (name == PropertyName.smashAnimeEvent)
		{
			value = VariantUtils.CreateFrom(in smashAnimeEvent);
			return true;
		}
		if (name == PropertyName.impFireEvent)
		{
			value = VariantUtils.CreateFrom(in impFireEvent);
			return true;
		}
		if (name == PropertyName.impName)
		{
			value = VariantUtils.CreateFrom(in impName);
			return true;
		}
		if (name == PropertyName.impSpawnSlot)
		{
			value = VariantUtils.CreateFrom(in impSpawnSlot);
			return true;
		}
		if (name == PropertyName._impFilter)
		{
			value = VariantUtils.CreateFrom(in _impFilter);
			return true;
		}
		if (name == PropertyName.impFliters)
		{
			value = VariantUtils.CreateFromArray(impFliters);
			return true;
		}
		if (name == PropertyName.impThrowDamagePointName)
		{
			value = VariantUtils.CreateFrom(in impThrowDamagePointName);
			return true;
		}
		if (name == PropertyName.impThrowFlag)
		{
			value = VariantUtils.CreateFrom(in impThrowFlag);
			return true;
		}
		if (name == PropertyName._smashCycleTarget)
		{
			value = VariantUtils.CreateFrom(in _smashCycleTarget);
			return true;
		}
		if (name == PropertyName._fireStateHandleConnected)
		{
			value = VariantUtils.CreateFrom(in _fireStateHandleConnected);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.fireAnimeClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.smashAnimeEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.impFireEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.impName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.impSpawnSlot, PropertyHint.NodeType, "AdobeAnimateSlot", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._impFilter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.impFilter, PropertyHint.MultilineText, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.impFliters, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.impThrowDamagePointName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.impThrowFlag, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._smashCycleTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._fireStateHandleConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.impFilter, Variant.From<string>(impFilter));
		info.AddProperty(PropertyName.fireAnimeClip, Variant.From(in fireAnimeClip));
		info.AddProperty(PropertyName.smashAnimeEvent, Variant.From(in smashAnimeEvent));
		info.AddProperty(PropertyName.impFireEvent, Variant.From(in impFireEvent));
		info.AddProperty(PropertyName.impName, Variant.From(in impName));
		info.AddProperty(PropertyName.impSpawnSlot, Variant.From(in impSpawnSlot));
		info.AddProperty(PropertyName._impFilter, Variant.From(in _impFilter));
		info.AddProperty(PropertyName.impFliters, Variant.CreateFrom(impFliters));
		info.AddProperty(PropertyName.impThrowDamagePointName, Variant.From(in impThrowDamagePointName));
		info.AddProperty(PropertyName.impThrowFlag, Variant.From(in impThrowFlag));
		info.AddProperty(PropertyName._smashCycleTarget, Variant.From(in _smashCycleTarget));
		info.AddProperty(PropertyName._fireStateHandleConnected, Variant.From(in _fireStateHandleConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.impFilter, out var value))
		{
			impFilter = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.fireAnimeClip, out var value2))
		{
			fireAnimeClip = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.smashAnimeEvent, out var value3))
		{
			smashAnimeEvent = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.impFireEvent, out var value4))
		{
			impFireEvent = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.impName, out var value5))
		{
			impName = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.impSpawnSlot, out var value6))
		{
			impSpawnSlot = value6.As<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._impFilter, out var value7))
		{
			_impFilter = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.impFliters, out var value8))
		{
			impFliters = value8.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.impThrowDamagePointName, out var value9))
		{
			impThrowDamagePointName = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.impThrowFlag, out var value10))
		{
			impThrowFlag = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._smashCycleTarget, out var value11))
		{
			_smashCycleTarget = value11.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._fireStateHandleConnected, out var value12))
		{
			_fireStateHandleConnected = value12.As<bool>();
		}
	}
}
