using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Gold/SpikeShroom/Scene/TowerDefensePlantSpikeShroom.cs")]
public class TowerDefensePlantSpikeShroom : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public static readonly StringName Attack = "Attack";

		public new static readonly StringName AttackDeal = "AttackDeal";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName _attackMarker = "_attackMarker";

		public static readonly StringName _fireInterval = "_fireInterval";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string SPIKE_SHROOM_SKIN_3_1 = "uid://setqd0doysia";

	private const string SPIKE_SHROOM_SKIN_3_2 = "uid://dtaq2xxqdn4l";

	private const string SPIKE_SHROOM_SKIN_3_3 = "uid://d345b8j5t8rim";

	private const string SPIKE_SHROOM_SKIN_4_1 = "uid://oupumw7ivoj2";

	private const string SPIKE_SHROOM_SKIN_4_2 = "uid://blsvhscujc3wy";

	private const string SPIKE_SHROOM_SKIN_4_3 = "uid://dab5sqapb1pxr";

	private const string SPIKE_SHROOM_SKIN_6_1 = "uid://b4c2b841mg3hk";

	private const string SPIKE_SHROOM_SKIN_6_2 = "uid://d3m54y244cyum";

	private const string SPIKE_SHROOM_SKIN_6_3 = "uid://d35grguid2x18";

	private static PackedScene _SPIKE_SHROOM_ATTACK_EFFECT;

	private static PackedScene _SPIKE_SHROOM_ATTACK_EFFECT_CUSTOM_0;

	private AttackComponent _attackComponent;

	private Marker2D _attackMarker;

	private double _fireInterval = 2.0;

	private static PackedScene SPIKE_SHROOM_ATTACK_EFFECT => _SPIKE_SHROOM_ATTACK_EFFECT ?? (_SPIKE_SHROOM_ATTACK_EFFECT = GD.Load<PackedScene>("uid://b7rfl7fns2fe2"));

	private static PackedScene SPIKE_SHROOM_ATTACK_EFFECT_CUSTOM_0 => _SPIKE_SHROOM_ATTACK_EFFECT_CUSTOM_0 ?? (_SPIKE_SHROOM_ATTACK_EFFECT_CUSTOM_0 = GD.Load<PackedScene>("uid://bac4csfsmibf0"));

	[Export(PropertyHint.None, "")]
	public double fireInterval
	{
		get
		{
			return _fireInterval;
		}
		set
		{
			_fireInterval = value;
			if (IsNodeReady() && _attackComponent != null)
			{
				_attackComponent.attackInterval = _fireInterval;
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			_attackComponent.OnAttack += Attack;
			_attackMarker = GetNode<Marker2D>("%AttackMarker");
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		AttackComponent attackComponent = _attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			_attackComponent.OnAttack -= Attack;
		}
	}

	public override void DamagePointReach(string damagePointName)
	{
		base.DamagePointReach(damagePointName);
		switch (damagePointName)
		{
		case "Damage0":
			if (currentCustom.Contains("Custom0"))
			{
				sprite.SetAtlasReplace("SpikeShroom_skin3_1.png", "uid://setqd0doysia");
				sprite.SetAtlasReplace("SpikeShroom_skin4_1.png", "uid://oupumw7ivoj2");
				sprite.SetAtlasReplace("SpikeShroom_skin6_1.png", "uid://b4c2b841mg3hk");
			}
			break;
		case "Damage1":
			if (currentCustom.Contains("Custom0"))
			{
				sprite.SetAtlasReplace("SpikeShroom_skin3_1.png", "uid://dtaq2xxqdn4l");
				sprite.SetAtlasReplace("SpikeShroom_skin4_1.png", "uid://blsvhscujc3wy");
				sprite.SetAtlasReplace("SpikeShroom_skin6_1.png", "uid://d3m54y244cyum");
			}
			break;
		case "Damage2":
			if (currentCustom.Contains("Custom0"))
			{
				sprite.SetAtlasReplace("SpikeShroom_skin3_1.png", "uid://d345b8j5t8rim");
				sprite.SetAtlasReplace("SpikeShroom_skin4_1.png", "uid://dab5sqapb1pxr");
				sprite.SetAtlasReplace("SpikeShroom_skin6_1.png", "uid://d35grguid2x18");
			}
			break;
		}
	}

	public void Attack()
	{
		AudioManager.Instance.AudioPlay("Spike");
		_attackComponent.AttackEventExecute();
		PackedScene scene = SPIKE_SHROOM_ATTACK_EFFECT;
		if (currentCustom.Contains("Custom0"))
		{
			scene = SPIKE_SHROOM_ATTACK_EFFECT_CUSTOM_0;
		}
		TowerDefenseEffectSpriteOnce node = TowerDefenseManager.CreateEffectSpriteOnce(scene, gridPos, "Idle");
		_attackMarker.AddChild(node, forceReadableName: false, InternalMode.Disabled);
	}

	public override void AttackDeal(TowerDefenseCharacter character, string type, double num)
	{
		base.AttackDeal(character, type, num);
		if (GodotObject.IsInstanceValid(character))
		{
			if (type != "Eat")
			{
				character.Hurt(Mathf.Min(1000.0, num), playSplatAudio: false);
			}
			else
			{
				character.Hurt(Mathf.Min(80.0, num / 3.0), playSplatAudio: false);
			}
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary { { "fireInterval", fireInterval } };
	}

	public override void ImportVariantSave(Dictionary data)
	{
		fireInterval = (double)data.GetValueOrDefault("fireInterval", 2.0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Attack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackDeal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Attack && args.Count == 0)
		{
			Attack();
			ret = default;
			return true;
		}
		if (method == MethodName.AttackDeal && args.Count == 3)
		{
			AttackDeal(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
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
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.Attack)
		{
			return true;
		}
		if (method == MethodName.AttackDeal)
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
		if (name == PropertyName.fireInterval)
		{
			fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._attackMarker)
		{
			_attackMarker = VariantUtils.ConvertTo<Marker2D>(in value);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			_fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.fireInterval)
		{
			value = VariantUtils.CreateFrom<double>(fireInterval);
			return true;
		}
		if (name == PropertyName._attackMarker)
		{
			value = VariantUtils.CreateFrom(in _attackMarker);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			value = VariantUtils.CreateFrom(in _fireInterval);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._attackMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fireInterval, Variant.From<double>(fireInterval));
		info.AddProperty(PropertyName._attackMarker, Variant.From(in _attackMarker));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fireInterval, out var value))
		{
			fireInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._attackMarker, out var value2))
		{
			_attackMarker = value2.As<Marker2D>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value3))
		{
			_fireInterval = value3.As<double>();
		}
	}
}
