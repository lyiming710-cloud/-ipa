using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://Asset/Anime/Character/Plant/Other/HypnoPot/Scene/TowerDefensePlantHypnoPot.cs")]
public class TowerDefensePlantHypnoPot : TowerDefensePlant
{
	public new class MethodName : TowerDefensePlant.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName BatchUpdate = "BatchUpdate";

		public static readonly StringName TrySpawnHypnoShroom = "TrySpawnHypnoShroom";

		public new static readonly StringName AttackDeal = "AttackDeal";

		public new static readonly StringName ExportVariantSave = "ExportVariantSave";

		public new static readonly StringName ImportVariantSave = "ImportVariantSave";
	}

	public new class PropertyName : TowerDefensePlant.PropertyName
	{
		public static readonly StringName _spawnTimer = "_spawnTimer";

		public static readonly StringName _over = "_over";
	}

	public new class SignalName : TowerDefensePlant.SignalName
	{
	}

	private const string HypnoShroomPacketName = "PlantHypnoShroom";

	private const string HypnoSeedPacketName = "PlantHypnoSeed";

	private const double SpawnInterval = 25.0;

	private double _spawnTimer;

	private bool _over;

	public override void _Ready()
	{
		base._Ready();
		Engine.IsEditorHint();
	}

	public override void BatchUpdate(double delta)
	{
		base.BatchUpdate(delta);
		if (!Engine.IsEditorHint() && inGame && GodotObject.IsInstanceValid(instance) && !instance.sleep)
		{
			_spawnTimer += (float)delta;
			if (_spawnTimer >= 25.0)
			{
				_spawnTimer -= 25.0;
				TrySpawnHypnoShroom();
			}
		}
	}

	private void TrySpawnHypnoShroom()
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		Vector2I vector2I = gridPos;
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig("PlantHypnoShroom");
		if (GodotObject.IsInstanceValid(packetConfig))
		{
			TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(vector2I);
			if (GodotObject.IsInstanceValid(mapCell) && mapCell.CanPacketPlant(packetConfig))
			{
				TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(vector2I);
				if (GodotObject.IsInstanceValid(towerDefenseCharacter) && instance.hypnoses)
				{
					towerDefenseCharacter.Hypnoses();
				}
				return;
			}
		}
		TowerDefensePacketConfig packetConfig2 = TowerDefenseManager.GetPacketConfig("PlantHypnoSeed");
		if (!GodotObject.IsInstanceValid(packetConfig2))
		{
			return;
		}
		if (instance.hypnoses)
		{
			if (!GodotObject.IsInstanceValid(packetConfig2._override))
			{
				packetConfig2._override = new TowerDefensePacketOverride();
			}
			packetConfig2._override.hypnoses = true;
		}
		SpawnPacket(packetConfig2, GetLogicalGlobalPosition(), 15.0, isFall: false);
	}

	public override void AttackDeal(TowerDefenseCharacter character, string type, double num)
	{
		base.AttackDeal(character, type, num);
		if (instance.sleep || _over)
		{
			return;
		}
		if (GodotObject.IsInstanceValid(character) && (character.instance.unUseBuffFlags & 8) != 0)
		{
			SkipInvincibleHurt(num);
		}
		else if (type == "Eat")
		{
			_over = true;
			if (GodotObject.IsInstanceValid(character))
			{
				character.Hypnoses();
			}
			Destroy();
		}
	}

	public override Dictionary ExportVariantSave()
	{
		return new Dictionary
		{
			["over"] = _over,
			["spawnTimer"] = _spawnTimer
		};
	}

	public override void ImportVariantSave(Dictionary data)
	{
		_over = data.GetValueOrDefault("over", false).AsBool();
		_spawnTimer = data.GetValueOrDefault("spawnTimer", 0.0).AsDouble();
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
			new MethodInfo(MethodName.TrySpawnHypnoShroom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.BatchUpdate && args.Count == 1)
		{
			BatchUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TrySpawnHypnoShroom && args.Count == 0)
		{
			TrySpawnHypnoShroom();
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
		if (method == MethodName.BatchUpdate)
		{
			return true;
		}
		if (method == MethodName.TrySpawnHypnoShroom)
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
		if (name == PropertyName._spawnTimer)
		{
			_spawnTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._over)
		{
			_over = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._spawnTimer)
		{
			value = VariantUtils.CreateFrom(in _spawnTimer);
			return true;
		}
		if (name == PropertyName._over)
		{
			value = VariantUtils.CreateFrom(in _over);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName._spawnTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._over, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._spawnTimer, Variant.From(in _spawnTimer));
		info.AddProperty(PropertyName._over, Variant.From(in _over));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._spawnTimer, out var value))
		{
			_spawnTimer = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName._over, out var value2))
		{
			_over = value2.As<bool>();
		}
	}
}
