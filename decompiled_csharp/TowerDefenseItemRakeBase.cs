using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Item/Rake/Scene/TowerDefenseItemRakeBase.cs")]
public abstract class TowerDefenseItemRakeBase : TowerDefenseItem
{
	public new class MethodName : TowerDefenseItem.MethodName
	{
		public new static readonly StringName PrepareForProgressRestore = "PrepareForProgressRestore";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName ActivateGameplayProcessing = "ActivateGameplayProcessing";

		public static readonly StringName TryAcquireContactTarget = "TryAcquireContactTarget";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName ResumePendingCleanup = "ResumePendingCleanup";

		public static readonly StringName ScheduleCleanupAfterHit = "ScheduleCleanupAfterHit";

		public static readonly StringName ApplyHitEffect = "ApplyHitEffect";
	}

	public new class PropertyName : TowerDefenseItem.PropertyName
	{
		public static readonly StringName over = "over";

		public static readonly StringName _hitCompleted = "_hitCompleted";

		public static readonly StringName _restoredFromProgress = "_restoredFromProgress";

		public static readonly StringName _cleanupScheduled = "_cleanupScheduled";
	}

	public new class SignalName : TowerDefenseItem.SignalName
	{
	}

	protected AttackComponent attackComponent;

	public bool over;

	private System.Collections.Generic.Dictionary<ulong, Rect2> _previousTargetRects = new System.Collections.Generic.Dictionary<ulong, Rect2>();

	private System.Collections.Generic.Dictionary<ulong, Rect2> _currentTargetRects = new System.Collections.Generic.Dictionary<ulong, Rect2>();

	private bool _hitCompleted;

	private bool _restoredFromProgress;

	private bool _cleanupScheduled;

	public override void PrepareForProgressRestore()
	{
		base.PrepareForProgressRestore();
		_restoredFromProgress = true;
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			DestroyHitBoxRuntime();
			TargetRegistrationComponent targetRegistrationComponent = base.targetRegistrationComponent;
			if (targetRegistrationComponent != null && !targetRegistrationComponent.IsReleased)
			{
				base.targetRegistrationComponent.canProjectileCheck = false;
			}
		}
	}

	public override void IdleEntered()
	{
		sprite.SetAnimation("Idle", loop: false, 0.2);
		sprite.pause = true;
	}

	public override void IdleProcessing(double delta)
	{
		sprite.timeScale = timeScale;
		if (inGame && GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) && TowerDefenseManager.CurrentControl.isGameRunning && !over && TryAcquireContactTarget())
		{
			AudioManager.Instance.AudioPlay("Bonk");
			sprite.pause = false;
			over = true;
		}
	}

	public override void ActivateGameplayProcessing()
	{
		bool num = !over && GodotObject.IsInstanceValid(sprite) && sprite.pause;
		base.ActivateGameplayProcessing();
		if (num && !over && GodotObject.IsInstanceValid(sprite))
		{
			sprite.pause = true;
		}
	}

	private bool TryAcquireContactTarget()
	{
		AttackComponent attackComponent = this.attackComponent;
		if (attackComponent != null && attackComponent.CanAttackOnce())
		{
			return true;
		}
		if (this.attackComponent == null || !this.attackComponent.TryGetCheckAreaWorldRect(out var worldRect) || !GodotObject.IsInstanceValid(TowerDefenseManager.Instance?.characterRegistry))
		{
			return false;
		}
		_currentTargetRects.Clear();
		List<TowerDefenseCharacter> charactersForLineList = TowerDefenseManager.Instance.characterRegistry.GetCharactersForLineList(gridPos.Y);
		for (int i = 0; i < charactersForLineList.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = charactersForLineList[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && towerDefenseCharacter != this && towerDefenseCharacter.HasHitBox)
			{
				Rect2 worldHitRect = towerDefenseCharacter.WorldHitRect;
				ulong instanceId = towerDefenseCharacter.GetInstanceId();
				_currentTargetRects[instanceId] = worldHitRect;
				bool flag = AabbShapeUtil.Intersects(worldRect, worldHitRect);
				bool flag2 = _previousTargetRects.TryGetValue(instanceId, out var value) && value != worldHitRect && AabbShapeUtil.Intersects(worldRect, AabbShapeUtil.Union(value, worldHitRect));
				if ((flag | flag2) && this.attackComponent.TryCommitContactTarget(towerDefenseCharacter))
				{
					_previousTargetRects.Clear();
					_currentTargetRects.Clear();
					return true;
				}
			}
		}
		System.Collections.Generic.Dictionary<ulong, Rect2> currentTargetRects = _currentTargetRects;
		System.Collections.Generic.Dictionary<ulong, Rect2> previousTargetRects = _previousTargetRects;
		_previousTargetRects = currentTargetRects;
		_currentTargetRects = previousTargetRects;
		return false;
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (!(clip != "Idle") && !_hitCompleted)
		{
			_hitCompleted = true;
			ApplyHitEffect();
			ScheduleCleanupAfterHit();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "over", over },
			{ "hitCompleted", _hitCompleted }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		_hitCompleted = data.GetValueOrDefault("hitCompleted", false).AsBool();
		over = data.GetValueOrDefault("over", false).AsBool() || _hitCompleted;
		if (_restoredFromProgress && _hitCompleted)
		{
			CallDeferred("ResumePendingCleanup");
		}
	}

	private void ResumePendingCleanup()
	{
		ScheduleCleanupAfterHit();
	}

	private async void ScheduleCleanupAfterHit()
	{
		if (_cleanupScheduled)
		{
			return;
		}
		_cleanupScheduled = true;
		SceneTree tree = GetTree();
		if (!GodotObject.IsInstanceValid(tree))
		{
			Destroy();
			return;
		}
		await ToSignal(tree.CreateTimer(0.5, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		if (GodotObject.IsInstanceValid(this) && !IsQueuedForDeletion())
		{
			Destroy();
		}
	}

	protected abstract void ApplyHitEffect();

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName.PrepareForProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ActivateGameplayProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryAcquireContactTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResumePendingCleanup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleCleanupAfterHit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyHitEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
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
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ActivateGameplayProcessing && args.Count == 0)
		{
			ActivateGameplayProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.TryAcquireContactTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryAcquireContactTarget());
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.ResumePendingCleanup && args.Count == 0)
		{
			ResumePendingCleanup();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleCleanupAfterHit && args.Count == 0)
		{
			ScheduleCleanupAfterHit();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyHitEffect && args.Count == 0)
		{
			ApplyHitEffect();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.PrepareForProgressRestore)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.ActivateGameplayProcessing)
		{
			return true;
		}
		if (method == MethodName.TryAcquireContactTarget)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
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
		if (method == MethodName.ResumePendingCleanup)
		{
			return true;
		}
		if (method == MethodName.ScheduleCleanupAfterHit)
		{
			return true;
		}
		if (method == MethodName.ApplyHitEffect)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hitCompleted)
		{
			_hitCompleted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._restoredFromProgress)
		{
			_restoredFromProgress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cleanupScheduled)
		{
			_cleanupScheduled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		if (name == PropertyName._hitCompleted)
		{
			value = VariantUtils.CreateFrom(in _hitCompleted);
			return true;
		}
		if (name == PropertyName._restoredFromProgress)
		{
			value = VariantUtils.CreateFrom(in _restoredFromProgress);
			return true;
		}
		if (name == PropertyName._cleanupScheduled)
		{
			value = VariantUtils.CreateFrom(in _cleanupScheduled);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hitCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._restoredFromProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cleanupScheduled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.over, Variant.From(in over));
		info.AddProperty(PropertyName._hitCompleted, Variant.From(in _hitCompleted));
		info.AddProperty(PropertyName._restoredFromProgress, Variant.From(in _restoredFromProgress));
		info.AddProperty(PropertyName._cleanupScheduled, Variant.From(in _cleanupScheduled));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.over, out var value))
		{
			over = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hitCompleted, out var value2))
		{
			_hitCompleted = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._restoredFromProgress, out var value3))
		{
			_restoredFromProgress = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cleanupScheduled, out var value4))
		{
			_cleanupScheduled = value4.As<bool>();
		}
	}
}
