using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Gold/QueenSunFlower/Scene/TowerDefensePlantQueenSunFlower.cs")]
public class TowerDefensePlantQueenSunFlower : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName OnCustomSwitched = "OnCustomSwitched";

		public new static readonly StringName IdleProcessing = "IdleProcessing";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName produceInterval = "produceInterval";

		public static readonly StringName sunNum = "sunNum";

		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName skinName = "skinName";

		public static readonly StringName eventList = "eventList";

		public static readonly StringName allEventList = "allEventList";

		public static readonly StringName attackInterval = "attackInterval";

		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName fireNum = "fireNum";

		public static readonly StringName _produceInterval = "_produceInterval";

		public static readonly StringName _sunNum = "_sunNum";

		public static readonly StringName currentFireNum = "currentFireNum";

		public static readonly StringName attackTimer = "attackTimer";

		public static readonly StringName _projectileName = "_projectileName";

		public static readonly StringName _skinName = "_skinName";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> eventList = new Array<TowerDefenseCharacterEventBase>();

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> allEventList = new Array<TowerDefenseCharacterEventBase>();

	private static readonly Array<TowerDefenseCharacter> _emptyCharacterList = new Array<TowerDefenseCharacter>();

	private AttackComponent _attackComponent;

	private ProduceComponent _produceComponent;

	private FireComponent _fireComponent;

	[Export(PropertyHint.None, "")]
	public double attackInterval = 3.0;

	[Export(PropertyHint.None, "")]
	public double fireInterval = 3.0;

	[Export(PropertyHint.None, "")]
	public int fireNum = 6;

	private double _produceInterval = 25.0;

	private int _sunNum = 150;

	public int currentFireNum;

	public double attackTimer;

	private string _projectileName = "FirePea";

	private string _skinName = "Default";

	[Export(PropertyHint.None, "")]
	public double produceInterval
	{
		get
		{
			return _produceInterval;
		}
		set
		{
			_produceInterval = value;
			if (IsNodeReady() && _produceComponent != null)
			{
				_produceComponent.produceInterval = (float)_produceInterval;
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int sunNum
	{
		get
		{
			return _sunNum;
		}
		set
		{
			_sunNum = value;
			if (IsNodeReady() && _produceComponent != null)
			{
				_produceComponent.num = _sunNum;
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public string projectileName
	{
		get
		{
			return _projectileName;
		}
		set
		{
			_projectileName = value;
			if (IsNodeReady() && _fireComponent != null)
			{
				((FireComponentProjectileSingle)_fireComponent.fireCheckList[0].projectile).projectileName = _projectileName;
			}
		}
	}

	public string skinName
	{
		get
		{
			return _skinName;
		}
		set
		{
			_skinName = value;
			if (IsNodeReady() && _fireComponent != null)
			{
				((FireComponentProjectileSingle)_fireComponent.fireCheckList[0].projectile).projectileData.skinName = _skinName;
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_attackComponent = componentManager.GetRuntime<AttackComponent>("character.attack.0");
			_produceComponent = componentManager.GetRuntime<ProduceComponent>();
			_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			if (currentCustom.Contains("Custom0"))
			{
				skinName = "Note";
			}
			_attackComponent.SetCheckAreaRectangleSize(0, TowerDefenseManager.Instance.GetMapGridSize() * 2.75f);
			_fireComponent.fireInterval = (float)fireInterval;
		}
	}

	public override void OnCustomSwitched(string customKey)
	{
		if (customKey == "Custom0")
		{
			skinName = "Note";
		}
		else
		{
			skinName = "Default";
		}
	}

	public override void IdleProcessing(double delta)
	{
		base.IdleProcessing(delta);
		sprite.timeScale = timeScale;
		Vector2 pos = default;
		bool flag = false;
		if (Engine.GetPhysicsFrames() % 2 == 0L)
		{
			pos = GetLogicalGlobalPosition();
			flag = true;
			TowerDefenseExplode.CreateExplode(pos, new Vector2(1.3f, 1.3f), allEventList, _emptyCharacterList, TowerDefenseEnum.CHARACTER_CAMP.ALL, -1);
		}
		if (attackTimer >= attackInterval)
		{
			if (_attackComponent.CanAttack())
			{
				if (!flag)
				{
					pos = GetLogicalGlobalPosition();
				}
				TowerDefenseExplode.CreateExplode(pos, new Vector2(1.3f, 1.3f), eventList, _emptyCharacterList, camp, instance.collisionFlags);
				attackTimer = 0.0;
			}
		}
		else
		{
			attackTimer += delta;
		}
		if (_fireComponent.CanFireByData(_fireComponent.fireCheckList[0].projectile.GetProjetile()))
		{
			_fireComponent.Refresh();
			for (int i = 0; i < fireNum; i++)
			{
				Vector2 vector = Vector2.FromAngle((float)Mathf.DegToRad(360.0 / (double)fireNum * (double)i));
				BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
				{
					spawnTweenOffset = vector * 50f,
					spawnTweenDuration = 0.5f,
					spawnTweenEase = Tween.EaseType.Out,
					spawnTweenTrans = Tween.TransitionType.Quart
				};
				_fireComponent.CreateProjectileByData(0, new Vector2(600f, 0f), _fireComponent.fireCheckList[0].projectile.GetProjetile(), -1, camp, Vector2.Zero, overrides);
			}
		}
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		_produceComponent.produceType = (instance.hypnoses ? "BrainSun" : "Sun");
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "attackInterval", attackInterval },
			{ "fireNum", fireNum },
			{ "produceInterval", produceInterval },
			{ "sunNum", sunNum },
			{ "currentFireNum", currentFireNum },
			{ "attackTimer", attackTimer },
			{ "projectileName", projectileName },
			{ "skinName", skinName },
			{ "fireInterval", fireInterval }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		attackInterval = (double)data.GetValueOrDefault("attackInterval", 3.0);
		fireNum = (int)data.GetValueOrDefault("fireNum", 6);
		produceInterval = (double)data.GetValueOrDefault("produceInterval", 25.0);
		sunNum = (int)data.GetValueOrDefault("sunNum", 150);
		currentFireNum = (int)data.GetValueOrDefault("currentFireNum", 0);
		attackTimer = (double)data.GetValueOrDefault("attackTimer", 0.0);
		projectileName = (string)data.GetValueOrDefault("projectileName", "FirePea");
		skinName = (string)data.GetValueOrDefault("skinName", "Default");
		fireInterval = (double)data.GetValueOrDefault("fireInterval", 3.0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCustomSwitched, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "customKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IdleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.OnCustomSwitched && args.Count == 1)
		{
			OnCustomSwitched(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IdleProcessing && args.Count == 1)
		{
			IdleProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
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
		if (method == MethodName.OnCustomSwitched)
		{
			return true;
		}
		if (method == MethodName.IdleProcessing)
		{
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged)
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
		if (name == PropertyName.produceInterval)
		{
			produceInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.sunNum)
		{
			sunNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.skinName)
		{
			skinName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			eventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.allEventList)
		{
			allEventList = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
			return true;
		}
		if (name == PropertyName.attackInterval)
		{
			attackInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireInterval)
		{
			fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.fireNum)
		{
			fireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._produceInterval)
		{
			_produceInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._sunNum)
		{
			_sunNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.currentFireNum)
		{
			currentFireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.attackTimer)
		{
			attackTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			_projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._skinName)
		{
			_skinName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.produceInterval)
		{
			value = VariantUtils.CreateFrom<double>(produceInterval);
			return true;
		}
		if (name == PropertyName.sunNum)
		{
			value = VariantUtils.CreateFrom<int>(sunNum);
			return true;
		}
		string from;
		if (name == PropertyName.projectileName)
		{
			from = projectileName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.skinName)
		{
			from = skinName;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.eventList)
		{
			value = VariantUtils.CreateFromArray(eventList);
			return true;
		}
		if (name == PropertyName.allEventList)
		{
			value = VariantUtils.CreateFromArray(allEventList);
			return true;
		}
		if (name == PropertyName.attackInterval)
		{
			value = VariantUtils.CreateFrom(in attackInterval);
			return true;
		}
		if (name == PropertyName.fireInterval)
		{
			value = VariantUtils.CreateFrom(in fireInterval);
			return true;
		}
		if (name == PropertyName.fireNum)
		{
			value = VariantUtils.CreateFrom(in fireNum);
			return true;
		}
		if (name == PropertyName._produceInterval)
		{
			value = VariantUtils.CreateFrom(in _produceInterval);
			return true;
		}
		if (name == PropertyName._sunNum)
		{
			value = VariantUtils.CreateFrom(in _sunNum);
			return true;
		}
		if (name == PropertyName.currentFireNum)
		{
			value = VariantUtils.CreateFrom(in currentFireNum);
			return true;
		}
		if (name == PropertyName.attackTimer)
		{
			value = VariantUtils.CreateFrom(in attackTimer);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			value = VariantUtils.CreateFrom(in _projectileName);
			return true;
		}
		if (name == PropertyName._skinName)
		{
			value = VariantUtils.CreateFrom(in _skinName);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.eventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.allEventList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._produceInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.produceInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._sunNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.currentFireNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.attackTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._skinName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.skinName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.produceInterval, Variant.From<double>(produceInterval));
		info.AddProperty(PropertyName.sunNum, Variant.From<int>(sunNum));
		info.AddProperty(PropertyName.projectileName, Variant.From<string>(projectileName));
		info.AddProperty(PropertyName.skinName, Variant.From<string>(skinName));
		info.AddProperty(PropertyName.eventList, Variant.CreateFrom(eventList));
		info.AddProperty(PropertyName.allEventList, Variant.CreateFrom(allEventList));
		info.AddProperty(PropertyName.attackInterval, Variant.From(in attackInterval));
		info.AddProperty(PropertyName.fireInterval, Variant.From(in fireInterval));
		info.AddProperty(PropertyName.fireNum, Variant.From(in fireNum));
		info.AddProperty(PropertyName._produceInterval, Variant.From(in _produceInterval));
		info.AddProperty(PropertyName._sunNum, Variant.From(in _sunNum));
		info.AddProperty(PropertyName.currentFireNum, Variant.From(in currentFireNum));
		info.AddProperty(PropertyName.attackTimer, Variant.From(in attackTimer));
		info.AddProperty(PropertyName._projectileName, Variant.From(in _projectileName));
		info.AddProperty(PropertyName._skinName, Variant.From(in _skinName));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.produceInterval, out var value))
		{
			produceInterval = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.sunNum, out var value2))
		{
			sunNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projectileName, out var value3))
		{
			projectileName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.skinName, out var value4))
		{
			skinName = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName.eventList, out var value5))
		{
			eventList = value5.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.allEventList, out var value6))
		{
			allEventList = value6.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
		if (info.TryGetProperty(PropertyName.attackInterval, out var value7))
		{
			attackInterval = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireInterval, out var value8))
		{
			fireInterval = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName.fireNum, out var value9))
		{
			fireNum = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._produceInterval, out var value10))
		{
			_produceInterval = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName._sunNum, out var value11))
		{
			_sunNum = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName.currentFireNum, out var value12))
		{
			currentFireNum = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName.attackTimer, out var value13))
		{
			attackTimer = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName._projectileName, out var value14))
		{
			_projectileName = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName._skinName, out var value15))
		{
			_skinName = value15.As<string>();
		}
	}
}
