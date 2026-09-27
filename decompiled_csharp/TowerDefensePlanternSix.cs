using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter3/PlanternSix/Scene/TowerDefensePlanternSix.cs")]
public class TowerDefensePlanternSix : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName attackInterval = "attackInterval";

		public static readonly StringName eventList = "eventList";

		public static readonly StringName eventAttackList = "eventAttackList";

		public static readonly StringName light = "light";

		public static readonly StringName attackTimer = "attackTimer";

		public static readonly StringName coldCheckInterval = "coldCheckInterval";

		public static readonly StringName over = "over";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private static readonly string TOWER_DEFENSE_PROJECTILE_EFFECT_PLANTERN_SIX = "uid://nbjvjnrei6vi";

	[Export(PropertyHint.None, "")]
	public double attackInterval = 1.5;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventAttackList = new Array<TowerDefenseCharacterEventBase>();

	private PointLight2D light;

	public double attackTimer;

	public int coldCheckInterval = 2;

	public bool over;

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			light = GetNode<PointLight2D>("%Light");
			AudioManager.Instance.AudioPlay("Plantern");
		}
	}

	public override void BatchUpdate(double delta)
	{
		if (!Engine.IsEditorHint())
		{
			base.BatchUpdate(delta);
			if ((ulong)((long)TowerDefenseProcessModeDispatch.CurrentPhysicsFrame + (long)randFreshIndex) % 30uL == 0L)
			{
				light.Visible = TowerDefenseManager.GetMapIsNight() && GameSaveManager.Instance.GetConfigValue("MapEffect").AsBool();
			}
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		sprite.timeScale = timeScale;
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		if (coldCheckInterval > 0)
		{
			coldCheckInterval--;
		}
		else
		{
			TowerDefenseExplode.CreateExplode(logicalGlobalPosition, new Vector2(1.3f, 1.3f), eventList, null, camp, -1);
			coldCheckInterval = 2;
		}
		if (attackTimer >= attackInterval)
		{
			TowerDefenseExplode.CreateExplode(logicalGlobalPosition, new Vector2(1.3f, 1.3f), eventAttackList, null, camp, instance.collisionFlags);
			attackTimer = 0.0;
		}
		else
		{
			attackTimer += delta;
		}
	}

	public override async void DestroySet()
	{
		if (!over)
		{
			over = true;
			TowerDefenseProjectileEffectBase towerDefenseProjectileEffectBase = (TowerDefenseProjectileEffectBase)GD.Load<PackedScene>(TOWER_DEFENSE_PROJECTILE_EFFECT_PLANTERN_SIX).Instantiate(PackedScene.GenEditState.Disabled);
			int collisionFlag = config?.collisionFlags ?? (-1);
			towerDefenseProjectileEffectBase.Init(gridPos, camp, collisionFlag, null, groundHeight);
			towerDefenseProjectileEffectBase.GlobalPosition = GetLogicalGlobalPosition();
			TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseProjectileEffectBase, forceReadableName: false, InternalMode.Disabled);
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "attackInterval", attackInterval },
			{ "attackTimer", attackTimer },
			{ "coldCheckInterval", coldCheckInterval },
			{ "over", over }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		attackInterval = data.GetValueOrDefault("attackInterval", 1.5).AsDouble();
		attackTimer = data.GetValueOrDefault("attackTimer", 0.0).AsDouble();
		coldCheckInterval = data.GetValueOrDefault("coldCheckInterval", 2).AsInt32();
		over = data.GetValueOrDefault("over", false).AsBool();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
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
		if (name == PropertyName.attackInterval)
		{
			attackInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.eventAttackList)
		{
			eventAttackList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.light)
		{
			light = VariantUtils.ConvertTo<PointLight2D>(in value);
			return true;
		}
		if (name == PropertyName.attackTimer)
		{
			attackTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.coldCheckInterval)
		{
			coldCheckInterval = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.over)
		{
			over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.attackInterval)
		{
			value = VariantUtils.CreateFrom(in attackInterval);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		if (name == PropertyName.eventAttackList)
		{
			value = VariantUtils.CreateFromArray(eventAttackList);
			return true;
		}
		if (name == PropertyName.light)
		{
			value = VariantUtils.CreateFrom(in light);
			return true;
		}
		if (name == PropertyName.attackTimer)
		{
			value = VariantUtils.CreateFrom(in attackTimer);
			return true;
		}
		if (name == PropertyName.coldCheckInterval)
		{
			value = VariantUtils.CreateFrom(in coldCheckInterval);
			return true;
		}
		if (name == PropertyName.over)
		{
			value = VariantUtils.CreateFrom(in over);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.attackInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.eventAttackList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.light, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.coldCheckInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.attackInterval, Variant.From(in attackInterval));
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.eventAttackList, Variant.CreateFrom(eventAttackList));
		info.AddProperty(PropertyName.light, Variant.From(in light));
		info.AddProperty(PropertyName.attackTimer, Variant.From(in attackTimer));
		info.AddProperty(PropertyName.coldCheckInterval, Variant.From(in coldCheckInterval));
		info.AddProperty(PropertyName.over, Variant.From(in over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.attackInterval, out var value))
		{
			attackInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.eventList, out var value2))
		{
			eventList = value2.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.eventAttackList, out var value3))
		{
			eventAttackList = value3.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.light, out var value4))
		{
			light = value4.As<PointLight2D>();
		}
		if (info.TryGetProperty(PropertyName.attackTimer, out var value5))
		{
			attackTimer = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.coldCheckInterval, out var value6))
		{
			coldCheckInterval = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value7))
		{
			over = value7.As<bool>();
		}
	}
}
