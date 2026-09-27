using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Gold/VIPNut/Scene/TowerDefensePlantVIPNut.cs")]
public class TowerDefensePlantVIPNut : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName GowUp = "GowUp";

		public new static readonly StringName DestroySet = "DestroySet";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName produceInterval = "produceInterval";

		public static readonly StringName sunNum = "sunNum";

		public static readonly StringName growUpTime = "growUpTime";

		public static readonly StringName produceType = "produceType";

		public static readonly StringName _produceInterval = "_produceInterval";

		public static readonly StringName _sunNum = "_sunNum";

		public static readonly StringName _growUpTime = "_growUpTime";

		public static readonly StringName growUpSunNum = "growUpSunNum";

		public static readonly StringName _produceType = "_produceType";

		public static readonly StringName dieCreateSunNum = "dieCreateSunNum";

		public static readonly StringName over = "over";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string VIP_NUT_SKIN_2_1 = "uid://3lrskq4eia8b";

	private const string VIP_NUT_SKIN_2_2 = "uid://c6lmbuq33tm3";

	private const string VIP_NUT_SKIN_2_3 = "uid://frbgmrgbd7s1";

	private const string VIP_NUT_SKIN_4_1 = "uid://bisp36tuaclyt";

	private const string VIP_NUT_SKIN_4_2 = "uid://jh80dvtmfn58";

	private const string VIP_NUT_SKIN_4_3 = "uid://iywdryeb7h4g";

	private ProduceComponent _produceComponent;

	private GrowUpComponent _growUpComponent;

	private double _produceInterval = 25.0;

	private int _sunNum = 25;

	private double _growUpTime = 60.0;

	[Export(PropertyHint.None, "")]
	public int growUpSunNum = 50;

	private string _produceType = "Sun";

	[Export(PropertyHint.None, "")]
	public int dieCreateSunNum = 200;

	public bool over;

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
				ProduceComponent produceComponent = _produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					_produceComponent.produceInterval = (float)_produceInterval;
				}
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
				ProduceComponent produceComponent = _produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					_produceComponent.num = _sunNum;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public double growUpTime
	{
		get
		{
			return _growUpTime;
		}
		set
		{
			_growUpTime = value;
			if (IsNodeReady() && _growUpComponent != null)
			{
				GrowUpComponent growUpComponent = _growUpComponent;
				if (growUpComponent != null && !growUpComponent.IsReleased && _growUpComponent.growUpTime.Count > 0)
				{
					_growUpComponent.growUpTime[0] = (float)_growUpTime;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public string produceType
	{
		get
		{
			return _produceType;
		}
		set
		{
			_produceType = value;
			if (IsNodeReady() && _produceComponent != null)
			{
				ProduceComponent produceComponent = _produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					_produceComponent.produceType = _produceType;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_produceComponent = componentManager.GetRuntime<ProduceComponent>();
			_growUpComponent = componentManager.GetRuntime<GrowUpComponent>();
			if (_growUpComponent != null)
			{
				_growUpComponent.OnGrow += GowUp;
			}
			_produceComponent.produceInterval = (float)produceInterval;
			_produceComponent.num = sunNum;
			if (_growUpComponent.growUpTime.Count > 0)
			{
				_growUpComponent.growUpTime[0] = (float)growUpTime;
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		GrowUpComponent growUpComponent = _growUpComponent;
		if (growUpComponent != null && !growUpComponent.IsReleased)
		{
			_growUpComponent.OnGrow -= GowUp;
		}
	}

	public void GowUp(int reach)
	{
		if (reach == 0)
		{
			_produceComponent.num = growUpSunNum;
			dieCreateSunNum = 400;
			instance.height = TowerDefenseEnum.CHARACTER_HEIGHT.TALL;
			if (_growUpComponent.ShouldApplyAuthoritativeGrowthEffects)
			{
				Health(4000.0);
				instance.hitpointsSave += 4000.0;
			}
		}
	}

	public override void DestroySet()
	{
		if (over)
		{
			return;
		}
		over = true;
		Vector2 logicalGlobalPosition = GetLogicalGlobalPosition();
		for (int i = 0; (double)i < Mathf.Floor((double)dieCreateSunNum / 50.0); i++)
		{
			if (TowerDefenseManager.Instance.IsIZMMode() || TowerDefenseManager.Instance.IsIZM2Mode() || instance.hypnoses)
			{
				BrainSunCreate(logicalGlobalPosition, 50L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f));
			}
			else
			{
				SunCreate(logicalGlobalPosition, 50L, TowerDefenseEnum.SUN_MOVING_METHOD.GRAVITY, new Vector2((float)GD.RandRange(-50.0, 50.0), -400f));
			}
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
				sprite.SetAtlasReplace("VIPNut_skin2_1.png", "uid://3lrskq4eia8b");
				sprite.SetAtlasReplace("VIPNut_skin4_1.png", "uid://bisp36tuaclyt");
			}
			break;
		case "Damage1":
			if (currentCustom.Contains("Custom0"))
			{
				sprite.SetAtlasReplace("VIPNut_skin2_1.png", "uid://c6lmbuq33tm3");
				sprite.SetAtlasReplace("VIPNut_skin4_1.png", "uid://jh80dvtmfn58");
			}
			break;
		case "Damage2":
			if (currentCustom.Contains("Custom0"))
			{
				sprite.SetAtlasReplace("VIPNut_skin2_1.png", "uid://frbgmrgbd7s1");
				sprite.SetAtlasReplace("VIPNut_skin4_1.png", "uid://iywdryeb7h4g");
			}
			break;
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
			{ "produceInterval", produceInterval },
			{ "sunNum", sunNum },
			{ "growUpTime", growUpTime },
			{ "growUpSunNum", growUpSunNum },
			{ "dieCreateSunNum", dieCreateSunNum },
			{ "over", over }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		produceInterval = (double)data.GetValueOrDefault("produceInterval", 25.0);
		sunNum = (int)data.GetValueOrDefault("sunNum", 25);
		growUpTime = (double)data.GetValueOrDefault("growUpTime", 60.0);
		growUpSunNum = (int)data.GetValueOrDefault("growUpSunNum", 50);
		dieCreateSunNum = (int)data.GetValueOrDefault("dieCreateSunNum", 200);
		over = (bool)data.GetValueOrDefault("over", false);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GowUp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "reach", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DestroySet, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damagePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.GowUp && args.Count == 1)
		{
			GowUp(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DestroySet && args.Count == 0)
		{
			DestroySet();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.GowUp)
		{
			return true;
		}
		if (method == MethodName.DestroySet)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
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
		if (name == PropertyName.growUpTime)
		{
			growUpTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.produceType)
		{
			produceType = VariantUtils.ConvertTo<string>(in value);
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
		if (name == PropertyName._growUpTime)
		{
			_growUpTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.growUpSunNum)
		{
			growUpSunNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._produceType)
		{
			_produceType = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.dieCreateSunNum)
		{
			dieCreateSunNum = VariantUtils.ConvertTo<int>(in value);
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
		double from;
		if (name == PropertyName.produceInterval)
		{
			from = produceInterval;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.sunNum)
		{
			value = VariantUtils.CreateFrom<int>(sunNum);
			return true;
		}
		if (name == PropertyName.growUpTime)
		{
			from = growUpTime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.produceType)
		{
			value = VariantUtils.CreateFrom<string>(produceType);
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
		if (name == PropertyName._growUpTime)
		{
			value = VariantUtils.CreateFrom(in _growUpTime);
			return true;
		}
		if (name == PropertyName.growUpSunNum)
		{
			value = VariantUtils.CreateFrom(in growUpSunNum);
			return true;
		}
		if (name == PropertyName._produceType)
		{
			value = VariantUtils.CreateFrom(in _produceType);
			return true;
		}
		if (name == PropertyName.dieCreateSunNum)
		{
			value = VariantUtils.CreateFrom(in dieCreateSunNum);
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
			new PropertyInfo(Variant.Type.Float, PropertyName._produceInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.produceInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._sunNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._growUpTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.growUpTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.growUpSunNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._produceType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.produceType, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.dieCreateSunNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.produceInterval, Variant.From<double>(produceInterval));
		info.AddProperty(PropertyName.sunNum, Variant.From<int>(sunNum));
		info.AddProperty(PropertyName.growUpTime, Variant.From<double>(growUpTime));
		info.AddProperty(PropertyName.produceType, Variant.From<string>(produceType));
		info.AddProperty(PropertyName._produceInterval, Variant.From(in _produceInterval));
		info.AddProperty(PropertyName._sunNum, Variant.From(in _sunNum));
		info.AddProperty(PropertyName._growUpTime, Variant.From(in _growUpTime));
		info.AddProperty(PropertyName.growUpSunNum, Variant.From(in growUpSunNum));
		info.AddProperty(PropertyName._produceType, Variant.From(in _produceType));
		info.AddProperty(PropertyName.dieCreateSunNum, Variant.From(in dieCreateSunNum));
		info.AddProperty(PropertyName.over, Variant.From(in over));
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
		if (info.TryGetProperty(PropertyName.growUpTime, out var value3))
		{
			growUpTime = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.produceType, out var value4))
		{
			produceType = value4.As<string>();
		}
		if (info.TryGetProperty(PropertyName._produceInterval, out var value5))
		{
			_produceInterval = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._sunNum, out var value6))
		{
			_sunNum = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._growUpTime, out var value7))
		{
			_growUpTime = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName.growUpSunNum, out var value8))
		{
			growUpSunNum = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._produceType, out var value9))
		{
			_produceType = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.dieCreateSunNum, out var value10))
		{
			dieCreateSunNum = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.over, out var value11))
		{
			over = value11.As<bool>();
		}
	}
}
