using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/GraveStone/BungiTargetSP/Scene/TowerDefenseBungiTargetSP.cs")]
public class TowerDefenseBungiTargetSP : TowerDefenseGravestone
{
	public new class MethodName : TowerDefenseGravestone.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName Blow = "Blow";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName DestroySet = "DestroySet";
	}

	public new class PropertyName : TowerDefenseGravestone.PropertyName
	{
		public static readonly StringName lifetime = "lifetime";

		public static readonly StringName _blowStarted = "_blowStarted";
	}

	public new class SignalName : TowerDefenseGravestone.SignalName
	{
	}

	private const string DisableMetaKey = "disabled_by_bungi_target_sp";

	private const string RefCountMetaKey = "character_disabled_ref_count";

	private const string SavedPauseMetaKey = "bungi_target_sp_saved_pause";

	public double lifetime = 15.0;

	private bool _blowStarted;

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint() || editorPreviewMode)
		{
			return;
		}
		instance.invincible = true;
		AddToGroup("BungiTargetSP", persistent: true);
		RemoveFromGroup("Gravestone");
		isGround = false;
		z = 600.0;
		Tween tween = CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "z", 0.0, 0.5);
		tween.TweenCallback(Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(this))
			{
				z = 0.0;
				groundHeight = 0.0;
				isGround = true;
			}
		}));
		Timer timer = new Timer
		{
			WaitTime = lifetime,
			OneShot = true
		};
		AddChild(timer, forceReadableName: false, InternalMode.Disabled);
		timer.Timeout += () =>
		{
			if (GodotObject.IsInstanceValid(this))
			{
				Blow();
			}
		};
		timer.Start();
		if (GodotObject.IsInstanceValid(BattleEventBus.Instance))
		{
			BattleEventBus.Instance.OnBlowAllEffectEmit += Blow;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (GodotObject.IsInstanceValid(BattleEventBus.Instance))
		{
			BattleEventBus.Instance.OnBlowAllEffectEmit -= Blow;
		}
	}

	public override async void Blow()
	{
		if (GodotObject.IsInstanceValid(this) && !_blowStarted)
		{
			_blowStarted = true;
			HitBoxDestroy();
			Tween tween = CreateTween();
			tween.TweenProperty(this, "modulate", Colors.Transparent, 1.0);
			await ToSignal(tween, Tween.SignalName.Finished);
			Destroy();
		}
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (Engine.IsEditorHint() || editorPreviewMode || !GodotObject.IsInstanceValid(cell) || nearDie || die)
		{
			return;
		}
		foreach (TowerDefenseCharacter character in cell.GetCharacterList())
		{
			if (!(character is TowerDefensePlant towerDefensePlant) || !CanTarget(towerDefensePlant) || towerDefensePlant is TowerDefensePlantBowlingBase || (character.instance.maskFlags & 2) != 0 || towerDefensePlant.instance.invincible)
			{
				continue;
			}
			if (!towerDefensePlant.HasMeta("disabled_by_bungi_target_sp"))
			{
				towerDefensePlant.SetMeta("disabled_by_bungi_target_sp", true);
				int num = towerDefensePlant.GetMeta("character_disabled_ref_count", 0).AsInt32() + 1;
				towerDefensePlant.SetMeta("character_disabled_ref_count", num);
			}
			if (GodotObject.IsInstanceValid(towerDefensePlant.sprite))
			{
				if (!towerDefensePlant.HasMeta("bungi_target_sp_saved_pause"))
				{
					towerDefensePlant.SetMeta("bungi_target_sp_saved_pause", towerDefensePlant.sprite.pause);
				}
				towerDefensePlant.sprite.pause = true;
			}
			towerDefensePlant.characterDisabled = true;
		}
	}

	public override void DestroySet()
	{
		if (GodotObject.IsInstanceValid(cell))
		{
			foreach (TowerDefenseCharacter character in cell.GetCharacterList())
			{
				if (!(character is TowerDefensePlant towerDefensePlant) || towerDefensePlant is TowerDefensePlantBowlingBase || !towerDefensePlant.HasMeta("disabled_by_bungi_target_sp"))
				{
					continue;
				}
				towerDefensePlant.RemoveMeta("disabled_by_bungi_target_sp");
				int num = towerDefensePlant.GetMeta("character_disabled_ref_count", 1).AsInt32() - 1;
				if (num <= 0)
				{
					towerDefensePlant.RemoveMeta("character_disabled_ref_count");
					towerDefensePlant.characterDisabled = false;
					if (GodotObject.IsInstanceValid(towerDefensePlant.sprite) && towerDefensePlant.HasMeta("bungi_target_sp_saved_pause"))
					{
						towerDefensePlant.sprite.pause = towerDefensePlant.GetMeta("bungi_target_sp_saved_pause").AsBool();
						towerDefensePlant.RemoveMeta("bungi_target_sp_saved_pause");
					}
				}
				else
				{
					towerDefensePlant.SetMeta("character_disabled_ref_count", num);
				}
			}
		}
		base.DestroySet();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Blow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.Blow && args.Count == 0)
		{
			Blow();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
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
		if (method == MethodName.Blow)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.lifetime)
		{
			lifetime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._blowStarted)
		{
			_blowStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.lifetime)
		{
			value = VariantUtils.CreateFrom(in lifetime);
			return true;
		}
		if (name == PropertyName._blowStarted)
		{
			value = VariantUtils.CreateFrom(in _blowStarted);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.lifetime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._blowStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.lifetime, Variant.From(in lifetime));
		info.AddProperty(PropertyName._blowStarted, Variant.From(in _blowStarted));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.lifetime, out var value))
		{
			lifetime = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._blowStarted, out var value2))
		{
			_blowStarted = value2.As<bool>();
		}
	}
}
