using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Star/ShootingStars/Scene/TowerDefensePlantShootingStars.cs")]
public class TowerDefensePlantShootingStars : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName IdleEntered = "IdleEntered";

		public static readonly StringName CompleteEffect = "CompleteEffect";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName CancelActiveEffect = "CancelActiveEffect";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName _effectStarted = "_effectStarted";

		public static readonly StringName _activeEffect = "_activeEffect";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private static PackedScene _shootingStarsEffectScene;

	public string projectileName = "StarFull";

	private bool _effectStarted;

	private TowerDefenseProjectileEffectShootingStars _activeEffect;

	private static PackedScene ShootingStarsEffectScene => _shootingStarsEffectScene ?? (_shootingStarsEffectScene = GD.Load<PackedScene>("res://Prefab/ProjectileEffect/ShootingStars/TowerDefenseProjectileEffectShootingStars.tscn"));

	public override void _Ready()
	{
		base._Ready();
		Engine.IsEditorHint();
	}

	public override void IdleEntered()
	{
		base.IdleEntered();
		if (!Engine.IsEditorHint() && !_effectStarted && GodotObject.IsInstanceValid(TowerDefenseManager.CurrentControl) && TowerDefenseManager.CurrentControl.isGameRunning && inGame)
		{
			_effectStarted = true;
			TowerDefenseProjectileEffectShootingStars towerDefenseProjectileEffectShootingStars = (_activeEffect = ShootingStarsEffectScene.Instantiate<TowerDefenseProjectileEffectShootingStars>(PackedScene.GenEditState.Disabled));
			towerDefenseProjectileEffectShootingStars.ConfigureNetworkVariant(projectileName);
			towerDefenseProjectileEffectShootingStars.Init(gridPos, camp, config.collisionFlags, null, groundHeight);
			towerDefenseProjectileEffectShootingStars.GlobalPosition = GetLogicalGlobalPosition();
			if (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost)
			{
				towerDefenseProjectileEffectShootingStars.OnCompleted += CompleteEffect;
			}
			TowerDefenseGroundItemBase.characterNode.AddChild(towerDefenseProjectileEffectShootingStars, forceReadableName: false, InternalMode.Disabled);
			HitBoxDestroy();
			instance.invincible = true;
		}
	}

	private void CompleteEffect()
	{
		_activeEffect = null;
		if (GodotObject.IsInstanceValid(this) && IsInsideTree() && (!Global.IsMultiplayerMode || MultiPlayerManager.IsHost))
		{
			Destroy();
		}
	}

	public override void DestroySet()
	{
		CancelActiveEffect();
		base.DestroySet();
	}

	public override void _ExitTree()
	{
		CancelActiveEffect();
		base._ExitTree();
	}

	private void CancelActiveEffect()
	{
		TowerDefenseProjectileEffectShootingStars activeEffect = _activeEffect;
		_activeEffect = null;
		if (GodotObject.IsInstanceValid(activeEffect))
		{
			activeEffect.OnCompleted -= CompleteEffect;
			if (!activeEffect.IsQueuedForDeletion())
			{
				activeEffect.QueueFree();
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "projectileName", projectileName } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		projectileName = data.GetValueOrDefault("projectileName", Variant.From<string>("StarFull")).AsString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IdleEntered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelActiveEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.IdleEntered && args.Count == 0)
		{
			IdleEntered();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteEffect && args.Count == 0)
		{
			CompleteEffect();
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelActiveEffect && args.Count == 0)
		{
			CancelActiveEffect();
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
		if (method == MethodName.IdleEntered)
		{
			return true;
		}
		if (method == MethodName.CompleteEffect)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.CancelActiveEffect)
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
		if (name == PropertyName.projectileName)
		{
			projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._effectStarted)
		{
			_effectStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._activeEffect)
		{
			_activeEffect = VariantUtils.ConvertTo<TowerDefenseProjectileEffectShootingStars>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom(in projectileName);
			return true;
		}
		if (name == PropertyName._effectStarted)
		{
			value = VariantUtils.CreateFrom(in _effectStarted);
			return true;
		}
		if (name == PropertyName._activeEffect)
		{
			value = VariantUtils.CreateFrom(in _activeEffect);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._effectStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._activeEffect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.projectileName, Variant.From(in projectileName));
		info.AddProperty(PropertyName._effectStarted, Variant.From(in _effectStarted));
		info.AddProperty(PropertyName._activeEffect, Variant.From(in _activeEffect));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.projectileName, out var value))
		{
			projectileName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._effectStarted, out var value2))
		{
			_effectStarted = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._activeEffect, out var value3))
		{
			_activeEffect = value3.As<TowerDefenseProjectileEffectShootingStars>();
		}
	}
}
