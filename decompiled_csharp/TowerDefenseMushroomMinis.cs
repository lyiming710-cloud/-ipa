using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/GraveStone/MushroomMinis/Scene/TowerDefenseMushroomMinis.cs")]
public class TowerDefenseMushroomMinis : TowerDefenseGravestone
{
	public new class MethodName : TowerDefenseGravestone.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName ApplyProtection = "ApplyProtection";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName CanSleep = "CanSleep";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName OnPlantingBlocked = "OnPlantingBlocked";

		public new static readonly StringName AnimeCompleted = "AnimeCompleted";

		public static readonly StringName HasZombieInCell = "HasZombieInCell";

		public static readonly StringName RefreshReaction = "RefreshReaction";

		public static readonly StringName Celebrate = "Celebrate";

		public static readonly StringName ResetReaction = "ResetReaction";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public new static readonly StringName FinalizeProgressRestore = "FinalizeProgressRestore";

		public new static readonly StringName ActivateGameplayProcessing = "ActivateGameplayProcessing";

		public new static readonly StringName Destroy = "Destroy";

		public new static readonly StringName ClearFromMap = "ClearFromMap";

		public new static readonly StringName ShovelDestroy = "ShovelDestroy";

		public new static readonly StringName DestroyWithVisualDelay = "DestroyWithVisualDelay";

		public new static readonly StringName AshDestroy = "AshDestroy";

		public new static readonly StringName SmashDestroy = "SmashDestroy";

		public new static readonly StringName BlowBack = "BlowBack";
	}

	public new class PropertyName : TowerDefenseGravestone.PropertyName
	{
		public new static readonly StringName IsPermanentObstacle = "IsPermanentObstacle";

		public static readonly StringName _celebrating = "_celebrating";

		public static readonly StringName _plantingReaction = "_plantingReaction";

		public static readonly StringName _zombiePresent = "_zombiePresent";

		public static readonly StringName _events = "_events";
	}

	public new class SignalName : TowerDefenseGravestone.SignalName
	{
	}

	private bool _celebrating;

	private bool _plantingReaction;

	private bool _zombiePresent;

	private BattleEventBus _events;

	public override bool IsPermanentObstacle => true;

	public override void _Ready()
	{
		rise = false;
		useIdleAnimeReset = false;
		base._Ready();
		if (!Engine.IsEditorHint() && !editorPreviewMode && HasValidRuntimeConfiguration)
		{
			ApplyProtection();
			_events = BattleEventBus.Instance;
			if (GodotObject.IsInstanceValid(_events))
			{
				_events.OnGameVictory += Celebrate;
				_events.OnGameStarted += ResetReaction;
			}
			TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
			_celebrating = currentControl != null && currentControl.levelControl?.awardCreate == true;
			RefreshReaction();
		}
	}

	private void ApplyProtection()
	{
		instance.invincible = true;
		instance.invincibleHurt = true;
		instance.invincibleSmash = true;
		instance.canBeCollection = false;
		instance.canCollection = false;
		instance.maskFlags = 0;
		if (targetRegistrationComponent != null)
		{
			targetRegistrationComponent.canProjectileCheck = false;
			targetRegistrationComponent.canCarry = false;
			targetRegistrationComponent.NotifyTargetStateChanged();
		}
		HitBoxDestroy();
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(_events))
		{
			_events.OnGameVictory -= Celebrate;
			_events.OnGameStarted -= ResetReaction;
		}
		_events = null;
		base._ExitTree();
	}

	public override bool CanSleep()
	{
		return false;
	}

	public override void IdleEntered()
	{
		RefreshReaction();
	}

	public override void IdleProcessing(double delta)
	{
		RefreshReaction();
	}

	public override void OnPlantingBlocked()
	{
		if (!_celebrating && !_plantingReaction && !isDestroy && GodotObject.IsInstanceValid(instance))
		{
			RefreshReaction();
			if (!_zombiePresent)
			{
				_plantingReaction = true;
				RefreshReaction();
			}
		}
	}

	public override void AnimeCompleted(string clip)
	{
		base.AnimeCompleted(clip);
		if (clip == "anim_shooting" && _plantingReaction)
		{
			_plantingReaction = false;
			RefreshReaction();
		}
	}

	private bool HasZombieInCell()
	{
		TowerDefenseBattleCharacterRegistry towerDefenseBattleCharacterRegistry = TowerDefenseManager.Instance?.characterRegistry;
		if (!GodotObject.IsInstanceValid(towerDefenseBattleCharacterRegistry))
		{
			return false;
		}
		foreach (TowerDefenseCharacter charactersForGridWindow in towerDefenseBattleCharacterRegistry.GetCharactersForGridWindowList(gridPos.X, gridPos.Y, 0, 0))
		{
			if (charactersForGridWindow is TowerDefenseZombie { isDestroy: false, die: false, nearDie: false, isGround: not false } towerDefenseZombie && towerDefenseZombie.gridPos == gridPos && GodotObject.IsInstanceValid(towerDefenseZombie.instance) && towerDefenseZombie.instance.hitpoints > 0.0)
			{
				return true;
			}
		}
		return false;
	}

	private void RefreshReaction()
	{
		if (!Engine.IsEditorHint() && GodotObject.IsInstanceValid(sprite) && !isDestroy)
		{
			_zombiePresent = !_celebrating && HasZombieInCell();
			if (_zombiePresent || _celebrating)
			{
				_plantingReaction = false;
			}
			string text;
			if (_celebrating)
			{
				text = "anim_cheer";
			}
			else
			{
				text = ((_zombiePresent || _plantingReaction) ? "anim_shooting" : "anim_idle");
			}
			bool flag = !_plantingReaction;
			if (sprite.clip != text || sprite.loop != flag)
			{
				sprite.SetAnimation(text, flag);
			}
			sprite.timeScale = timeScale;
		}
	}

	private void Celebrate()
	{
		_celebrating = true;
		RefreshReaction();
	}

	private void ResetReaction()
	{
		_celebrating = false;
		_plantingReaction = false;
		RefreshReaction();
	}

	public override void ImportVariantSave(Dictionary data)
	{
		base.ImportVariantSave(data);
		ApplyProtection();
		_plantingReaction = false;
		TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
		_celebrating = currentControl != null && currentControl.levelControl?.awardCreate == true;
		RefreshReaction();
	}

	public override void FinalizeProgressRestore()
	{
		base.FinalizeProgressRestore();
		_plantingReaction = false;
		RefreshReaction();
	}

	public override void ActivateGameplayProcessing()
	{
		base.ActivateGameplayProcessing();
		ApplyProtection();
		RefreshReaction();
	}

	public override void Destroy(bool freeInstance = true)
	{
		if (Global.IsEditor && SceneManager.CurrentScene == "LevelEditorStage")
		{
			base.Destroy(freeInstance);
		}
	}

	public override void ClearFromMap()
	{
		base.Destroy();
	}

	public override void ShovelDestroy()
	{
		Destroy();
	}

	public override void DestroyWithVisualDelay(double delaySeconds)
	{
	}

	public override void AshDestroy()
	{
	}

	public override void SmashDestroy()
	{
	}

	public override void BlowBack(double num, double time = -1.0)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(22)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyProtection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanSleep, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPlantingBlocked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasZombieInCell, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshReaction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Celebrate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetReaction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinalizeProgressRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ActivateGameplayProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Destroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "freeInstance", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearFromMap, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShovelDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroyWithVisualDelay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delaySeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AshDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SmashDestroy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BlowBack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ApplyProtection && args.Count == 0)
		{
			ApplyProtection();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.CanSleep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSleep());
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
		if (method == MethodName.OnPlantingBlocked && args.Count == 0)
		{
			OnPlantingBlocked();
			ret = default;
			return true;
		}
		if (method == MethodName.AnimeCompleted && args.Count == 1)
		{
			AnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasZombieInCell && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasZombieInCell());
			return true;
		}
		if (method == MethodName.RefreshReaction && args.Count == 0)
		{
			RefreshReaction();
			ret = default;
			return true;
		}
		if (method == MethodName.Celebrate && args.Count == 0)
		{
			Celebrate();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetReaction && args.Count == 0)
		{
			ResetReaction();
			ret = default;
			return true;
		}
		if (method == MethodName.ImportVariantSave && args.Count == 1)
		{
			ImportVariantSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinalizeProgressRestore && args.Count == 0)
		{
			FinalizeProgressRestore();
			ret = default;
			return true;
		}
		if (method == MethodName.ActivateGameplayProcessing && args.Count == 0)
		{
			ActivateGameplayProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.Destroy && args.Count == 1)
		{
			Destroy(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearFromMap && args.Count == 0)
		{
			ClearFromMap();
			ret = default;
			return true;
		}
		if (method == MethodName.ShovelDestroy && args.Count == 0)
		{
			ShovelDestroy();
			ret = default;
			return true;
		}
		if (method == MethodName.DestroyWithVisualDelay && args.Count == 1)
		{
			DestroyWithVisualDelay(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AshDestroy && args.Count == 0)
		{
			AshDestroy();
			ret = default;
			return true;
		}
		if (method == MethodName.SmashDestroy && args.Count == 0)
		{
			SmashDestroy();
			ret = default;
			return true;
		}
		if (method == MethodName.BlowBack && args.Count == 2)
		{
			BlowBack(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
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
		if (method == MethodName.ApplyProtection)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.CanSleep)
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
		if (method == MethodName.OnPlantingBlocked)
		{
			return true;
		}
		if (method == MethodName.AnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.HasZombieInCell)
		{
			return true;
		}
		if (method == MethodName.RefreshReaction)
		{
			return true;
		}
		if (method == MethodName.Celebrate)
		{
			return true;
		}
		if (method == MethodName.ResetReaction)
		{
			return true;
		}
		if (method == MethodName.ImportVariantSave)
		{
			return true;
		}
		if (method == MethodName.FinalizeProgressRestore)
		{
			return true;
		}
		if (method == MethodName.ActivateGameplayProcessing)
		{
			return true;
		}
		if (method == MethodName.Destroy)
		{
			return true;
		}
		if (method == MethodName.ClearFromMap)
		{
			return true;
		}
		if (method == MethodName.ShovelDestroy)
		{
			return true;
		}
		if (method == MethodName.DestroyWithVisualDelay)
		{
			return true;
		}
		if (method == MethodName.AshDestroy)
		{
			return true;
		}
		if (method == MethodName.SmashDestroy)
		{
			return true;
		}
		if (method == MethodName.BlowBack)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._celebrating)
		{
			_celebrating = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._plantingReaction)
		{
			_plantingReaction = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._zombiePresent)
		{
			_zombiePresent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._events)
		{
			_events = VariantUtils.ConvertTo<BattleEventBus>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.IsPermanentObstacle)
		{
			value = VariantUtils.CreateFrom<bool>(IsPermanentObstacle);
			return true;
		}
		if (name == PropertyName._celebrating)
		{
			value = VariantUtils.CreateFrom(in _celebrating);
			return true;
		}
		if (name == PropertyName._plantingReaction)
		{
			value = VariantUtils.CreateFrom(in _plantingReaction);
			return true;
		}
		if (name == PropertyName._zombiePresent)
		{
			value = VariantUtils.CreateFrom(in _zombiePresent);
			return true;
		}
		if (name == PropertyName._events)
		{
			value = VariantUtils.CreateFrom(in _events);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsPermanentObstacle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._celebrating, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._plantingReaction, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._zombiePresent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._events, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._celebrating, Variant.From(in _celebrating));
		info.AddProperty(PropertyName._plantingReaction, Variant.From(in _plantingReaction));
		info.AddProperty(PropertyName._zombiePresent, Variant.From(in _zombiePresent));
		info.AddProperty(PropertyName._events, Variant.From(in _events));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._celebrating, out var value))
		{
			_celebrating = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._plantingReaction, out var value2))
		{
			_plantingReaction = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._zombiePresent, out var value3))
		{
			_zombiePresent = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._events, out var value4))
		{
			_events = value4.As<BattleEventBus>();
		}
	}
}
