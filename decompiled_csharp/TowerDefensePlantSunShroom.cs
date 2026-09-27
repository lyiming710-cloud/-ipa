using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter1/SunShroom/Scene/TowerDefensePlantSunShroom.cs")]
public class TowerDefensePlantSunShroom : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName GowUp = "GowUp";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName produceInterval = "produceInterval";

		public static readonly StringName sunNum = "sunNum";

		public static readonly StringName growUpTime = "growUpTime";

		public static readonly StringName growUpSunNum = "growUpSunNum";

		public static readonly StringName produceType = "produceType";

		public static readonly StringName _produceInterval = "_produceInterval";

		public static readonly StringName _sunNum = "_sunNum";

		public static readonly StringName _growUpTime = "_growUpTime";

		public static readonly StringName _growUpSunNum = "_growUpSunNum";

		public static readonly StringName _produceType = "_produceType";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private ProduceComponent produceComponent;

	private GrowUpComponent growUpComponent;

	private double _produceInterval = 25.0;

	private int _sunNum = 25;

	private double _growUpTime = 60.0;

	private int _growUpSunNum = 50;

	private string _produceType = "Sun";

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
			if (IsNodeReady() && this.produceComponent != null)
			{
				ProduceComponent produceComponent = this.produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					this.produceComponent.produceInterval = (float)value;
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
			if (IsNodeReady() && this.produceComponent != null)
			{
				ProduceComponent produceComponent = this.produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					this.produceComponent.num = value;
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
			if (IsNodeReady() && this.growUpComponent != null)
			{
				GrowUpComponent growUpComponent = this.growUpComponent;
				if (growUpComponent != null && !growUpComponent.IsReleased)
				{
					this.growUpComponent.growUpTime[0] = (float)value;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int growUpSunNum
	{
		get
		{
			return _growUpSunNum;
		}
		set
		{
			_growUpSunNum = value;
		}
	}

	[Export(PropertyHint.Enum, "Sun,BrainSun,JalaSun,Coin,QXSun")]
	public string produceType
	{
		get
		{
			return _produceType;
		}
		set
		{
			_produceType = value;
			if (IsNodeReady() && this.produceComponent != null)
			{
				ProduceComponent produceComponent = this.produceComponent;
				if (produceComponent != null && !produceComponent.IsReleased)
				{
					this.produceComponent.produceType = value;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			produceComponent = componentManager.GetRuntime<ProduceComponent>();
			growUpComponent = componentManager.GetRuntime<GrowUpComponent>();
			if (growUpComponent != null)
			{
				growUpComponent.OnGrow += GowUp;
			}
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		GrowUpComponent growUpComponent = this.growUpComponent;
		if (growUpComponent != null && !growUpComponent.IsReleased)
		{
			this.growUpComponent.OnGrow -= GowUp;
		}
	}

	public void GowUp(int reach)
	{
		if (reach == 0)
		{
			produceComponent.num = growUpSunNum;
		}
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		produceComponent.produceType = (instance.hypnoses ? "BrainSun" : "Sun");
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			{ "produceInterval", produceInterval },
			{ "sunNum", sunNum },
			{ "growUpTime", growUpTime },
			{ "growUpSunNum", growUpSunNum }
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		produceInterval = data.GetValueOrDefault("produceInterval", 25.0).AsDouble();
		sunNum = data.GetValueOrDefault("sunNum", 25).AsInt32();
		growUpTime = data.GetValueOrDefault("growUpTime", 60.0).AsDouble();
		growUpSunNum = data.GetValueOrDefault("growUpSunNum", 50).AsInt32();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GowUp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "reach", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (name == PropertyName.growUpSunNum)
		{
			growUpSunNum = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName._growUpSunNum)
		{
			_growUpSunNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._produceType)
		{
			_produceType = VariantUtils.ConvertTo<string>(in value);
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
		int from2;
		if (name == PropertyName.sunNum)
		{
			from2 = sunNum;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.growUpTime)
		{
			from = growUpTime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.growUpSunNum)
		{
			from2 = growUpSunNum;
			value = VariantUtils.CreateFrom(in from2);
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
		if (name == PropertyName._growUpSunNum)
		{
			value = VariantUtils.CreateFrom(in _growUpSunNum);
			return true;
		}
		if (name == PropertyName._produceType)
		{
			value = VariantUtils.CreateFrom(in _produceType);
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
			new PropertyInfo(Variant.Type.Int, PropertyName._growUpSunNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.growUpSunNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._produceType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.produceType, PropertyHint.Enum, "Sun,BrainSun,JalaSun,Coin,QXSun", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.produceInterval, Variant.From<double>(produceInterval));
		info.AddProperty(PropertyName.sunNum, Variant.From<int>(sunNum));
		info.AddProperty(PropertyName.growUpTime, Variant.From<double>(growUpTime));
		info.AddProperty(PropertyName.growUpSunNum, Variant.From<int>(growUpSunNum));
		info.AddProperty(PropertyName.produceType, Variant.From<string>(produceType));
		info.AddProperty(PropertyName._produceInterval, Variant.From(in _produceInterval));
		info.AddProperty(PropertyName._sunNum, Variant.From(in _sunNum));
		info.AddProperty(PropertyName._growUpTime, Variant.From(in _growUpTime));
		info.AddProperty(PropertyName._growUpSunNum, Variant.From(in _growUpSunNum));
		info.AddProperty(PropertyName._produceType, Variant.From(in _produceType));
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
		if (info.TryGetProperty(PropertyName.growUpSunNum, out var value4))
		{
			growUpSunNum = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.produceType, out var value5))
		{
			produceType = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName._produceInterval, out var value6))
		{
			_produceInterval = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._sunNum, out var value7))
		{
			_sunNum = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName._growUpTime, out var value8))
		{
			_growUpTime = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._growUpSunNum, out var value9))
		{
			_growUpSunNum = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName._produceType, out var value10))
		{
			_produceType = value10.As<string>();
		}
	}
}
