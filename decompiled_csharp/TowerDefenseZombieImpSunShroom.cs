using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Zombie/Chapter1/Imp/Scene/SunShroom/TowerDefenseZombieImpSunShroom.cs")]
public class TowerDefenseZombieImpSunShroom : TowerDefenseZombieImpBase
{
	public new class MethodName : TowerDefenseZombieImpBase.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName AttackProcessing = "AttackProcessing";

		public new static readonly StringName DieProcessing = "DieProcessing";

		public new static readonly StringName InWater = "InWater";

		public new static readonly StringName OutWater = "OutWater";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public new static readonly StringName OnHypnosisStateChanged = "OnHypnosisStateChanged";

		public new static readonly StringName DamagePointReach = "DamagePointReach";

		public static readonly StringName GowUp = "GowUp";

		public new static readonly StringName Purify = "Purify";
	}

	public new class PropertyName : TowerDefenseZombieImpBase.PropertyName
	{
		public static readonly StringName produceInterval = "produceInterval";

		public static readonly StringName sunNum = "sunNum";

		public static readonly StringName growUpTime = "growUpTime";

		public static readonly StringName growUpSunNum = "growUpSunNum";

		public static readonly StringName _produceInterval = "_produceInterval";

		public static readonly StringName _sunNum = "_sunNum";

		public static readonly StringName _growUpTime = "_growUpTime";

		public static readonly StringName _growUpSunNum = "_growUpSunNum";
	}

	public new class SignalName : TowerDefenseZombieImpBase.SignalName
	{
	}

	private ProduceComponent _produceComponent;

	private GrowUpComponent _growUpComponent;

	private double _produceInterval = 10.0;

	private int _sunNum = 15;

	private double _growUpTime = 25.0;

	private int _growUpSunNum = 25;

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
			if (IsNodeReady())
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
			if (IsNodeReady())
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
			if (IsNodeReady())
			{
				GrowUpComponent growUpComponent = _growUpComponent;
				if (growUpComponent != null && !growUpComponent.IsReleased)
				{
					_growUpComponent.growUpTime[0] = (float)_growUpTime;
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
			ConfigureWaterLineVisualLayers("Zombie_duckytube");
			if (TowerDefenseManager.Instance.IsIZMMode())
			{
				instance.hitpointScale = 14.0 / 27.0;
			}
			if (!TowerDefenseManager.GetMapIsNight())
			{
				walkSpeedScale *= 0.5;
			}
			else
			{
				walkSpeedScale *= 1.0;
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

	public override void AttackProcessing(double delta)
	{
		base.AttackProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public override void DieProcessing(double delta)
	{
		base.DieProcessing(delta);
		sprite.timeScale = timeScale * 2.0;
	}

	public override void InWater()
	{
		base.InWater();
		sprite.SetFliter("Zombie_whitewater", open: true);
	}

	public override void OutWater()
	{
		base.OutWater();
		sprite.SetFliter("Zombie_whitewater", open: false);
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		Engine.IsEditorHint();
	}

	protected internal override void OnHypnosisStateChanged()
	{
		base.OnHypnosisStateChanged();
		ProduceComponent produceComponent = _produceComponent;
		if (produceComponent != null && !produceComponent.IsReleased)
		{
			_produceComponent.produceType = (instance.hypnoses ? "Sun" : "BrainSun");
		}
	}

	public override void DamagePointReach(string damangePointName)
	{
		base.DamagePointReach(damangePointName);
		if (damangePointName == "Head")
		{
			DamagePartCreate("Head", ((ZombieImpSunShroomSprite)sprite).head, new Vector2(GD.RandRange(-100, 100), -300f), keepSlotScale: false, new Vector2(-25f, -30f), fromSync: false, null, 0L);
		}
	}

	public void GowUp(int reach)
	{
		if (reach == 0)
		{
			_produceComponent.num = _growUpSunNum;
			if (_growUpComponent.ShouldApplyAuthoritativeGrowthEffects)
			{
				instance.hitpoints += 200.0;
			}
		}
	}

	public override void Purify()
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return;
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			Destroy();
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantSunShroomO");
		if (cell.CanPacketPlant(packetConfig))
		{
			TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(gridPos);
			towerDefenseCharacter.WakeUp();
			if (instance.hypnoses)
			{
				towerDefenseCharacter.Hypnoses();
			}
			if (Global.IsMultiplayerMode && MultiPlayerManager.IsHost)
			{
				TowerDefenseControlNew currentControl = TowerDefenseManager.CurrentControl;
				if (GodotObject.IsInstanceValid(currentControl))
				{
					int nextSyncId = currentControl.GetNextSyncId();
					currentControl.RegisterSyncCharacter(nextSyncId, towerDefenseCharacter);
					MultiPlayerManager.Instance.SendSpawnCharacterAt("PlantSunShroomO", gridPos.X, gridPos.Y, nextSyncId);
				}
			}
		}
		Destroy();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AttackProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DieProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OutWater, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BatchUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnHypnosisStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DamagePointReach, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "damangePointName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GowUp, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "reach", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Purify, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.AttackProcessing && args.Count == 1)
		{
			AttackProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DieProcessing && args.Count == 1)
		{
			DieProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InWater && args.Count == 0)
		{
			InWater();
			ret = default;
			return true;
		}
		if (method == MethodName.OutWater && args.Count == 0)
		{
			OutWater();
			ret = default;
			return true;
		}
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged && args.Count == 0)
		{
			OnHypnosisStateChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.DamagePointReach && args.Count == 1)
		{
			DamagePointReach(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GowUp && args.Count == 1)
		{
			GowUp(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Purify && args.Count == 0)
		{
			Purify();
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
		if (method == MethodName.AttackProcessing)
		{
			return true;
		}
		if (method == MethodName.DieProcessing)
		{
			return true;
		}
		if (method == MethodName.InWater)
		{
			return true;
		}
		if (method == MethodName.OutWater)
		{
			return true;
		}
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.OnHypnosisStateChanged)
		{
			return true;
		}
		if (method == MethodName.DamagePointReach)
		{
			return true;
		}
		if (method == MethodName.GowUp)
		{
			return true;
		}
		if (method == MethodName.Purify)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.produceInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._produceInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.sunNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._sunNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.growUpTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._growUpTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.growUpSunNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._growUpSunNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
		info.AddProperty(PropertyName._produceInterval, Variant.From(in _produceInterval));
		info.AddProperty(PropertyName._sunNum, Variant.From(in _sunNum));
		info.AddProperty(PropertyName._growUpTime, Variant.From(in _growUpTime));
		info.AddProperty(PropertyName._growUpSunNum, Variant.From(in _growUpSunNum));
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
		if (info.TryGetProperty(PropertyName._growUpSunNum, out var value8))
		{
			_growUpSunNum = value8.As<int>();
		}
	}
}
