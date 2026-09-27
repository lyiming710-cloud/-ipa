using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Challenge/Nano/Scene/TowerDefenseZombieNano.cs")]
public class TowerDefenseZombieNano : TowerDefenseZombie
{
	public new class MethodName : TowerDefenseZombie.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnNanoHurt = "OnNanoHurt";

		public static readonly StringName ShowNanoShield = "ShowNanoShield";
	}

	public new class PropertyName : TowerDefenseZombie.PropertyName
	{
		public static readonly StringName _lastTotalHp = "_lastTotalHp";

		public static readonly StringName _hpLossAccumulator = "_hpLossAccumulator";

		public static readonly StringName _activeShield = "_activeShield";
	}

	public new class SignalName : TowerDefenseZombie.SignalName
	{
	}

	private const double HP_LOSS_THRESHOLD = 500.0;

	private static PackedScene _nanoShieldScene;

	private double _lastTotalHp;

	private double _hpLossAccumulator;

	private TowerDefenseEffectSpriteOnce _activeShield;

	private static PackedScene NanoShieldScene => _nanoShieldScene ?? (_nanoShieldScene = GD.Load<PackedScene>("uid://by8jj4embf18f"));

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint() && HasValidRuntimeConfiguration)
		{
			instance.dealHurtReduce = 20.0;
			_lastTotalHp = GetCurrentHitPoint();
			OnBodyHurt += OnNanoHurt;
			OnArmorHurt += OnNanoHurt;
			OnDamageBlocked += ShowNanoShield;
		}
	}

	private void OnNanoHurt(int _num)
	{
		if (sprite.pause || sprite.Get("timeScale").AsDouble() <= 0.0)
		{
			return;
		}
		double currentHitPoint = GetCurrentHitPoint();
		double num = _lastTotalHp - currentHitPoint;
		if (num > 0.0)
		{
			_hpLossAccumulator += num;
			if (_hpLossAccumulator >= 500.0)
			{
				_hpLossAccumulator -= 500.0;
				ChangeLine();
			}
		}
		_lastTotalHp = currentHitPoint;
	}

	private void ShowNanoShield()
	{
		if (!GodotObject.IsInstanceValid(_activeShield) && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.CreateEffectSpriteOnce(NanoShieldScene, gridPos, "Idle");
			Node2D node2D = TowerDefenseManager.GetCharacterNode();
			if (!GodotObject.IsInstanceValid(node2D))
			{
				towerDefenseEffectSpriteOnce.QueueFree();
				return;
			}
			node2D.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
			towerDefenseEffectSpriteOnce.GlobalPosition = GetLogicalGlobalPosition() + new Vector2(GD.RandRange(-30, 30), GD.RandRange(-50, 50));
			_activeShield = towerDefenseEffectSpriteOnce;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnNanoHurt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowNanoShield, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnNanoHurt && args.Count == 1)
		{
			OnNanoHurt(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowNanoShield && args.Count == 0)
		{
			ShowNanoShield();
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
		if (method == MethodName.OnNanoHurt)
		{
			return true;
		}
		if (method == MethodName.ShowNanoShield)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._lastTotalHp)
		{
			_lastTotalHp = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._hpLossAccumulator)
		{
			_hpLossAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._activeShield)
		{
			_activeShield = VariantUtils.ConvertTo<TowerDefenseEffectSpriteOnce>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._lastTotalHp)
		{
			value = VariantUtils.CreateFrom(in _lastTotalHp);
			return true;
		}
		if (name == PropertyName._hpLossAccumulator)
		{
			value = VariantUtils.CreateFrom(in _hpLossAccumulator);
			return true;
		}
		if (name == PropertyName._activeShield)
		{
			value = VariantUtils.CreateFrom(in _activeShield);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName._lastTotalHp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._hpLossAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._activeShield, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._lastTotalHp, Variant.From(in _lastTotalHp));
		info.AddProperty(PropertyName._hpLossAccumulator, Variant.From(in _hpLossAccumulator));
		info.AddProperty(PropertyName._activeShield, Variant.From(in _activeShield));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._lastTotalHp, out var value))
		{
			_lastTotalHp = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._hpLossAccumulator, out var value2))
		{
			_hpLossAccumulator = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName._activeShield, out var value3))
		{
			_activeShield = value3.As<TowerDefenseEffectSpriteOnce>();
		}
	}
}
