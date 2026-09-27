using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter10/GuardNut/Scene/TowerDefensePlantGuardNut.cs")]
public class TowerDefensePlantGuardNut : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Attack = "Attack";

		public new static readonly StringName ArmorHitpointsEmpty = "ArmorHitpointsEmpty";

		public static readonly StringName ApplyShieldBrokenSpeedUp = "ApplyShieldBrokenSpeedUp";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _attackIntervalDefault = "_attackIntervalDefault";

		public static readonly StringName _shieldBroken = "_shieldBroken";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string ShieldArmorName = "GuardNutShield";

	private AttackComponent _attackComponent;

	private double _attackIntervalDefault;

	private bool _shieldBroken;

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint() || editorPreviewMode || !inGame)
		{
			return;
		}
		_attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
		AttackComponent attackComponent = _attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			_attackComponent.OnAttack += Attack;
			_attackIntervalDefault = _attackComponent.attackInterval;
			if (_shieldBroken)
			{
				ApplyShieldBrokenSpeedUp();
			}
		}
	}

	public override void _ExitTree()
	{
		AttackComponent attackComponent = _attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			_attackComponent.OnAttack -= Attack;
		}
		base._ExitTree();
	}

	private void Attack()
	{
		AudioManager.Instance?.AudioPlay("Stab");
		_attackComponent.AttackEventExecute();
	}

	public override void ArmorHitpointsEmpty(string armorName)
	{
		base.ArmorHitpointsEmpty(armorName);
		if (!(armorName != "GuardNutShield"))
		{
			_shieldBroken = true;
			ApplyShieldBrokenSpeedUp();
		}
	}

	private void ApplyShieldBrokenSpeedUp()
	{
		if (_attackIntervalDefault <= 0.0)
		{
			return;
		}
		AttackComponent attackComponent = _attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			double num = _attackIntervalDefault * 0.5;
			if (!(_attackComponent.attackInterval <= num))
			{
				_attackComponent.attackInterval = num;
				_attackComponent.Refresh();
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { ["shieldBroken"] = _shieldBroken };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		if (data.GetValueOrDefault("shieldBroken", false).AsBool())
		{
			_shieldBroken = true;
			ApplyShieldBrokenSpeedUp();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Attack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ArmorHitpointsEmpty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "armorName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyShieldBrokenSpeedUp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.Attack && args.Count == 0)
		{
			Attack();
			ret = default;
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty && args.Count == 1)
		{
			ArmorHitpointsEmpty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyShieldBrokenSpeedUp && args.Count == 0)
		{
			ApplyShieldBrokenSpeedUp();
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
		if (method == MethodName.Attack)
		{
			return true;
		}
		if (method == MethodName.ArmorHitpointsEmpty)
		{
			return true;
		}
		if (method == MethodName.ApplyShieldBrokenSpeedUp)
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
		if (name == PropertyName._attackIntervalDefault)
		{
			_attackIntervalDefault = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._shieldBroken)
		{
			_shieldBroken = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._attackIntervalDefault)
		{
			value = VariantUtils.CreateFrom(in _attackIntervalDefault);
			return true;
		}
		if (name == PropertyName._shieldBroken)
		{
			value = VariantUtils.CreateFrom(in _shieldBroken);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName._attackIntervalDefault, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._shieldBroken, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._attackIntervalDefault, Variant.From(in _attackIntervalDefault));
		info.AddProperty(PropertyName._shieldBroken, Variant.From(in _shieldBroken));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._attackIntervalDefault, out var value))
		{
			_attackIntervalDefault = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._shieldBroken, out var value2))
		{
			_shieldBroken = value2.As<bool>();
		}
	}
}
