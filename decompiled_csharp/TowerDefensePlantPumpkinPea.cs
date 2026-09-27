using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Chapter7/PumpkinPea/Scene/TowerDefensePlantPumpkinPea.cs")]
public class TowerDefensePlantPumpkinPea : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName Cover = "Cover";

		public static readonly StringName LevelSet = "LevelSet";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";

		public static readonly StringName LevelSetVisual = "LevelSetVisual";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName fireInterval = "fireInterval";

		public static readonly StringName fireNum = "fireNum";

		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName _fireInterval = "_fireInterval";

		public static readonly StringName _fireNum = "_fireNum";

		public static readonly StringName _projectileName = "_projectileName";

		public static readonly StringName level = "level";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private FireComponent _fireComponent;

	private double _fireInterval = 2.0;

	private int _fireNum = 1;

	private string _projectileName = "Pea";

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
			if (IsNodeReady() && _fireComponent != null)
			{
				FireComponent fireComponent = _fireComponent;
				if (fireComponent != null && !fireComponent.IsReleased)
				{
					_fireComponent.fireInterval = (float)value;
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int fireNum
	{
		get
		{
			return _fireNum;
		}
		set
		{
			_fireNum = value;
			if (IsNodeReady() && _fireComponent != null)
			{
				FireComponent fireComponent = _fireComponent;
				if (fireComponent != null && !fireComponent.IsReleased)
				{
					_fireComponent.fireNum = value;
				}
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
				FireComponent fireComponent = _fireComponent;
				if (fireComponent != null && !fireComponent.IsReleased && _fireComponent.fireCheckList.Count > 0)
				{
					((FireComponentProjectileSingle)_fireComponent.fireCheckList[0].projectile).projectileName = value;
				}
			}
		}
	}

	public override void _Ready()
	{
		base._Ready();
		if (!Engine.IsEditorHint())
		{
			_fireComponent = componentManager.GetRuntime<FireComponent>("character.fire");
			if (!inGame && !editorMapPreviewMode)
			{
				((PumpkinPeaSprite)sprite).back.ZIndex = 0;
			}
		}
	}

	public override void Cover(TowerDefenseCharacter character)
	{
		if (character.config.name == "PlantPumpkinPea" && character is TowerDefensePlantPumpkinPea towerDefensePlantPumpkinPea)
		{
			level = Mathf.Clamp(towerDefensePlantPumpkinPea.level + 1, 1, 3);
			LevelSet(level);
		}
	}

	public void LevelSet(int lv)
	{
		switch (lv)
		{
		case 2:
			sprite.SetFliters(new Array { "Upgrade1_eyebrow", "Upgrade1_helmet", "Upgrade2_barrel" }, open: true);
			instance.hitpoints += 2000.0;
			instance.hitpointsSave += 2000.0;
			_fireComponent.fireEventName = "fire&fire2";
			break;
		case 3:
			sprite.SetFliters(new Array { "Upgrade1_eyebrow", "Upgrade1_eyebrow2", "Upgrade1_helmet", "Upgrade2_helmet", "Upgrade2_barrel", "Upgrade2_barrel2", "Upgrade2_face" }, open: true);
			instance.hitpoints += 4000.0;
			instance.hitpointsSave += 4000.0;
			_fireComponent.fireEventName = "fire&fire2&fire3";
			break;
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["fireNum"] = fireNum,
			["projectileName"] = projectileName,
			["level"] = level,
			["fireInterval"] = fireInterval
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		fireNum = ((!data.ContainsKey("fireNum")) ? 1 : data["fireNum"].AsInt32());
		projectileName = (data.ContainsKey("projectileName") ? data["projectileName"].AsString() : "Pea");
		level = ((!data.ContainsKey("level")) ? 1 : data["level"].AsInt32());
		if (level > 1)
		{
			LevelSetVisual(level);
		}
		fireInterval = (data.ContainsKey("fireInterval") ? data["fireInterval"].AsDouble() : 2.0);
	}

	public void LevelSetVisual(int lv)
	{
		switch (lv)
		{
		case 2:
			sprite.SetFliters(new Array { "Upgrade1_eyebrow", "Upgrade1_helmet", "Upgrade2_barrel" }, open: true);
			_fireComponent.fireEventName = "fire&fire2";
			break;
		case 3:
			sprite.SetFliters(new Array { "Upgrade1_eyebrow", "Upgrade1_helmet", "Upgrade1_helmet2", "Upgrade2_helmet", "Upgrade2_barrel", "Upgrade2_barrel2", "Upgrade2_face" }, open: true);
			_fireComponent.fireEventName = "fire&fire2&fire3";
			break;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
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
		if (name == PropertyName.fireNum)
		{
			fireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			projectileName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			_fireInterval = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._fireNum)
		{
			_fireNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			_projectileName = VariantUtils.ConvertTo<string>(in value);
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
		if (name == PropertyName.fireNum)
		{
			value = VariantUtils.CreateFrom<int>(fireNum);
			return true;
		}
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom<string>(projectileName);
			return true;
		}
		if (name == PropertyName._fireInterval)
		{
			value = VariantUtils.CreateFrom(in _fireInterval);
			return true;
		}
		if (name == PropertyName._fireNum)
		{
			value = VariantUtils.CreateFrom(in _fireNum);
			return true;
		}
		if (name == PropertyName._projectileName)
		{
			value = VariantUtils.CreateFrom(in _projectileName);
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
			new PropertyInfo(Variant.Type.Float, PropertyName.fireInterval, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._fireInterval, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._fireNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.level, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.fireInterval, Variant.From<double>(fireInterval));
		info.AddProperty(PropertyName.fireNum, Variant.From<int>(fireNum));
		info.AddProperty(PropertyName.projectileName, Variant.From<string>(projectileName));
		info.AddProperty(PropertyName._fireInterval, Variant.From(in _fireInterval));
		info.AddProperty(PropertyName._fireNum, Variant.From(in _fireNum));
		info.AddProperty(PropertyName._projectileName, Variant.From(in _projectileName));
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
		if (info.TryGetProperty(PropertyName.fireNum, out var value2))
		{
			fireNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.projectileName, out var value3))
		{
			projectileName = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName._fireInterval, out var value4))
		{
			_fireInterval = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName._fireNum, out var value5))
		{
			_fireNum = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._projectileName, out var value6))
		{
			_projectileName = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.level, out var value7))
		{
			level = value7.As<int>();
		}
	}
}
