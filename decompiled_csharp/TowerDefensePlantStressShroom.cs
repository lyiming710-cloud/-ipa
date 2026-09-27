using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter7/StressShroom/Scene/TowerDefensePlantStressShroom.cs")]
public class TowerDefensePlantStressShroom : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName Attack = "Attack";

		public new static readonly StringName Cover = "Cover";

		public static readonly StringName LevelSet = "LevelSet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName LevelSetVisual = "LevelSetVisual";

		public static readonly StringName DuplicateRuntimeMutableAttackResources = "DuplicateRuntimeMutableAttackResources";

		public static readonly StringName SetAttackAreaDimensions = "SetAttackAreaDimensions";

		public static readonly StringName SetAttackDamageForLevel = "SetAttackDamageForLevel";

		public static readonly StringName GetAttackDamageForLevel = "GetAttackDamageForLevel";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName _hurtEvent = "_hurtEvent";

		public static readonly StringName _fireParticles = "_fireParticles";

		public static readonly StringName _fireParticles2 = "_fireParticles2";

		public static readonly StringName _fireParticles3 = "_fireParticles3";

		public static readonly StringName _fireParticles4 = "_fireParticles4";

		public static readonly StringName _fireInterval = "_fireInterval";

		public static readonly StringName level = "level";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const float AttackRangeGridCells = 6.5f;

	private const double BaseAttackDamage = 20.0;

	private const double AttackDamagePerLevel = 20.0;

	private const int MaxLevel = 4;

	private AttackComponent _attackComponent;

	private TowerDefenseCharacterEventHurt _hurtEvent;

	private GpuParticles2D _fireParticles;

	private GpuParticles2D _fireParticles2;

	private GpuParticles2D _fireParticles3;

	private GpuParticles2D _fireParticles4;

	private double _fireInterval = 2.0;

	[Export(PropertyHint.None, "")]
	public int level = 1;

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
				AttackComponent attackComponent = _attackComponent;
				if (attackComponent != null && !attackComponent.IsReleased)
				{
					_attackComponent.attackInterval = (float)value;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			_fireParticles = GetNode<GpuParticles2D>("%FireParticles");
			_fireParticles2 = GetNode<GpuParticles2D>("%FireParticles2");
			_fireParticles3 = GetNode<GpuParticles2D>("%FireParticles3");
			_fireParticles4 = GetNode<GpuParticles2D>("%FireParticles4");
			_attackComponent.OnAttack += Attack;
			_attackComponent.checkEachShape = true;
			DuplicateRuntimeMutableAttackResources();
			SetAttackAreaDimensions();
			SetAttackDamageForLevel(level);
			_fireParticles.ProcessMaterial = (Material)_fireParticles.ProcessMaterial.DuplicateDeep(Resource.DeepDuplicateMode.Internal);
			_fireParticles2.ProcessMaterial = _fireParticles.ProcessMaterial;
			_fireParticles3.ProcessMaterial = _fireParticles.ProcessMaterial;
			_fireParticles4.ProcessMaterial = _fireParticles.ProcessMaterial;
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

	public void Attack()
	{
		_fireParticles.Restart();
		_fireParticles2.Restart();
		_fireParticles3.Restart();
		_fireParticles4.Restart();
		AudioManager.Instance.AudioPlay("Fume");
		_attackComponent.AttackEventExecute();
	}

	public override void Cover(TowerDefenseCharacter character)
	{
		if (character.config.name == "PlantStressShroom" && character is TowerDefensePlantStressShroom towerDefensePlantStressShroom)
		{
			level = Mathf.Clamp(towerDefensePlantStressShroom.level + 1, 1, 4);
			LevelSet(level);
			if (character.instance.wakeUp)
			{
				instance.wakeUp = true;
			}
		}
	}

	public void LevelSet(int lv)
	{
		lv = Mathf.Clamp(lv, 1, 4);
		SetAttackDamageForLevel(lv);
		switch (lv)
		{
		case 2:
			sprite.SetFliters(new Array { "Upgrade1_eyebrow", "Upgrade1_helmet", "Upgrade2_barrel" }, open: true);
			instance.hitpoints += 300.0;
			instance.hitpointsSave += 300.0;
			_attackComponent.attackEventName = "fire";
			_attackComponent.attackAnimeClips = "Fire2";
			_fireParticles.Scale *= 1.25f;
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMax *= 1.25f;
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMin *= 1.25f;
			idleAnimeClip = "Idle2";
			sleepAnimeClip = "Sleep2";
			break;
		case 3:
			sprite.SetFliters(new Array { "Upgrade1_eyebrow", "Upgrade1_eyebrow2", "Upgrade1_helmet", "Upgrade2_helmet", "Upgrade2_barrel", "Upgrade2_barrel2", "Upgrade2_face" }, open: true);
			instance.hitpoints += 600.0;
			instance.hitpointsSave += 600.0;
			_attackComponent.attackEventName = "fire";
			_attackComponent.attackAnimeClips = "Fire3";
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMax *= 1.5f;
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMin *= 1.5f;
			idleAnimeClip = "Idle3";
			sleepAnimeClip = "Sleep3";
			break;
		case 4:
			sprite.SetFliters(new Array { "Upgrade1_eyebrow", "Upgrade1_eyebrow2", "Upgrade1_helmet", "Upgrade2_helmet", "Upgrade2_barrel", "Upgrade2_barrel2", "Upgrade2_face" }, open: true);
			instance.hitpoints += 900.0;
			instance.hitpointsSave += 900.0;
			_attackComponent.attackEventName = "fire";
			_attackComponent.attackAnimeClips = "Fire4";
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMax *= 1.75f;
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMin *= 1.75f;
			idleAnimeClip = "Idle4";
			sleepAnimeClip = "Sleep4";
			break;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["level"] = level,
			["fireInterval"] = fireInterval
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		level = ((!data.ContainsKey("level")) ? 1 : data["level"].AsInt32());
		if (level > 1)
		{
			LevelSetVisual(level);
		}
		fireInterval = (data.ContainsKey("fireInterval") ? data["fireInterval"].AsDouble() : 2.0);
	}

	public void LevelSetVisual(int lv)
	{
		lv = Mathf.Clamp(lv, 1, 4);
		SetAttackDamageForLevel(lv);
		switch (lv)
		{
		case 2:
			sprite.SetFliters(new Array { "Upgrade1_eyebrow", "Upgrade1_helmet", "Upgrade2_barrel" }, open: true);
			_attackComponent.attackEventName = "fire";
			_attackComponent.attackAnimeClips = "Fire2";
			_fireParticles.Scale *= 1.25f;
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMax *= 1.25f;
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMin *= 1.25f;
			idleAnimeClip = "Idle2";
			sleepAnimeClip = "Sleep2";
			break;
		case 3:
			sprite.SetFliters(new Array { "Upgrade1_eyebrow", "Upgrade1_eyebrow2", "Upgrade1_helmet", "Upgrade2_helmet", "Upgrade2_barrel", "Upgrade2_barrel2", "Upgrade2_face" }, open: true);
			_attackComponent.attackEventName = "fire";
			_attackComponent.attackAnimeClips = "Fire3";
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMax *= 1.5f;
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMin *= 1.5f;
			idleAnimeClip = "Idle3";
			sleepAnimeClip = "Sleep3";
			break;
		case 4:
			sprite.SetFliters(new Array { "Upgrade1_eyebrow", "Upgrade1_eyebrow2", "Upgrade1_helmet", "Upgrade2_helmet", "Upgrade2_barrel", "Upgrade2_barrel2", "Upgrade2_face" }, open: true);
			_attackComponent.attackEventName = "fire";
			_attackComponent.attackAnimeClips = "Fire4";
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMax *= 1.75f;
			((ParticleProcessMaterial)_fireParticles.ProcessMaterial).ScaleMin *= 1.75f;
			idleAnimeClip = "Idle4";
			sleepAnimeClip = "Sleep4";
			break;
		}
	}

	private void DuplicateRuntimeMutableAttackResources()
	{
		if (_attackComponent.eventList.Count != 0 && _attackComponent.eventList[0] is TowerDefenseCharacterEventHurt towerDefenseCharacterEventHurt)
		{
			_hurtEvent = towerDefenseCharacterEventHurt.Duplicate(deep: true) as TowerDefenseCharacterEventHurt;
			if (_hurtEvent != null)
			{
				_attackComponent.eventList[0] = _hurtEvent;
				_attackComponent.RefreshEventExecutionCache();
			}
		}
	}

	private void SetAttackAreaDimensions()
	{
		AttackComponent attackComponent = _attackComponent;
		if (attackComponent != null && !attackComponent.IsReleased)
		{
			Vector2 mapGridSize = TowerDefenseManager.Instance.GetMapGridSize();
			float length = mapGridSize.X * 6.5f;
			float length2 = mapGridSize.Y * 6.5f;
			_attackComponent.SetCheckAreaSegmentLengthX(0, length);
			_attackComponent.SetCheckAreaSegmentLengthX(1, length2);
			_attackComponent.SetCheckAreaSegmentLengthX(2, length2);
			_attackComponent.SetCheckAreaSegmentLengthX(3, length);
			_attackComponent.SetCheckAreaSegmentLengthX(4, length2);
			_attackComponent.SetCheckAreaSegmentLengthX(5, length2);
		}
	}

	private void SetAttackDamageForLevel(int lv)
	{
		if (_hurtEvent == null)
		{
			_hurtEvent = ((_attackComponent.eventList.Count > 0) ? (_attackComponent.eventList[0] as TowerDefenseCharacterEventHurt) : null);
		}
		if (_hurtEvent != null)
		{
			_hurtEvent.num = GetAttackDamageForLevel(lv);
		}
	}

	private static double GetAttackDamageForLevel(int lv)
	{
		lv = Mathf.Clamp(lv, 1, 4);
		return 20.0 + (double)(lv - 1) * 20.0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Attack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Cover, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.LevelSet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "lv", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExportVariantSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportVariantSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LevelSetVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "lv", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DuplicateRuntimeMutableAttackResources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetAttackAreaDimensions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetAttackDamageForLevel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "lv", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetAttackDamageForLevel, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "lv", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Cover && args.Count == 1)
		{
			Cover(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LevelSet && args.Count == 1)
		{
			LevelSet(VariantUtils.ConvertTo<int>(in args[0]));
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
		if (method == MethodName.LevelSetVisual && args.Count == 1)
		{
			LevelSetVisual(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DuplicateRuntimeMutableAttackResources && args.Count == 0)
		{
			DuplicateRuntimeMutableAttackResources();
			ret = default;
			return true;
		}
		if (method == MethodName.SetAttackAreaDimensions && args.Count == 0)
		{
			SetAttackAreaDimensions();
			ret = default;
			return true;
		}
		if (method == MethodName.SetAttackDamageForLevel && args.Count == 1)
		{
			SetAttackDamageForLevel(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetAttackDamageForLevel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetAttackDamageForLevel(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetAttackDamageForLevel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetAttackDamageForLevel(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		ret = default;
		return false;
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
		if (method == MethodName.Cover)
		{
			return true;
		}
		if (method == MethodName.LevelSet)
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
		if (method == MethodName.LevelSetVisual)
		{
			return true;
		}
		if (method == MethodName.DuplicateRuntimeMutableAttackResources)
		{
			return true;
		}
		if (method == MethodName.SetAttackAreaDimensions)
		{
			return true;
		}
		if (method == MethodName.SetAttackDamageForLevel)
		{
			return true;
		}
		if (method == MethodName.GetAttackDamageForLevel)
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
		if (name == PropertyName._hurtEvent)
		{
			_hurtEvent = VariantUtils.ConvertTo<TowerDefenseCharacterEventHurt>(in value);
			return true;
		}
		if (name == PropertyName._fireParticles)
		{
			_fireParticles = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._fireParticles2)
		{
			_fireParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._fireParticles3)
		{
			_fireParticles3 = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._fireParticles4)
		{
			_fireParticles4 = VariantUtils.ConvertTo<GpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			_fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.level)
		{
			level = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName._hurtEvent)
		{
			value = VariantUtils.CreateFrom(in _hurtEvent);
			return true;
		}
		if (name == PropertyName._fireParticles)
		{
			value = VariantUtils.CreateFrom(in _fireParticles);
			return true;
		}
		if (name == PropertyName._fireParticles2)
		{
			value = VariantUtils.CreateFrom(in _fireParticles2);
			return true;
		}
		if (name == PropertyName._fireParticles3)
		{
			value = VariantUtils.CreateFrom(in _fireParticles3);
			return true;
		}
		if (name == PropertyName._fireParticles4)
		{
			value = VariantUtils.CreateFrom(in _fireParticles4);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			value = VariantUtils.CreateFrom(in _fireInterval);
			return true;
		}
		if (name == PropertyName.level)
		{
			value = VariantUtils.CreateFrom(in level);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._hurtEvent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fireParticles, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fireParticles2, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fireParticles3, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fireParticles4, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.level, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fireInterval, Variant.From<double>(fireInterval));
		info.AddProperty(PropertyName._hurtEvent, Variant.From(in _hurtEvent));
		info.AddProperty(PropertyName._fireParticles, Variant.From(in _fireParticles));
		info.AddProperty(PropertyName._fireParticles2, Variant.From(in _fireParticles2));
		info.AddProperty(PropertyName._fireParticles3, Variant.From(in _fireParticles3));
		info.AddProperty(PropertyName._fireParticles4, Variant.From(in _fireParticles4));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
		info.AddProperty(PropertyName.level, Variant.From(in level));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.fireInterval, out var value))
		{
			fireInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._hurtEvent, out var value2))
		{
			_hurtEvent = value2.As<TowerDefenseCharacterEventHurt>();
		}
		if (info.TryGetProperty(PropertyName._fireParticles, out var value3))
		{
			_fireParticles = value3.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._fireParticles2, out var value4))
		{
			_fireParticles2 = value4.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._fireParticles3, out var value5))
		{
			_fireParticles3 = value5.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._fireParticles4, out var value6))
		{
			_fireParticles4 = value6.As<GpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value7))
		{
			_fireInterval = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.level, out var value8))
		{
			level = value8.As<int>();
		}
	}
}
