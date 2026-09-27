using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Level/PreSpawn/TowerDefenseLevelPreSpawnConfig.cs")]
public class TowerDefenseLevelPreSpawnConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName Init = "Init";

		public static readonly StringName SpawnCharacter = "SpawnCharacter";

		public static readonly StringName FindPreSpawnPlantTarget = "FindPreSpawnPlantTarget";

		public static readonly StringName Export = "Export";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName packetName = "packetName";

		public static readonly StringName gridPos = "gridPos";

		public static readonly StringName characterOverride = "characterOverride";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string packetName = "";

	[Export(PropertyHint.None, "")]
	public Vector2I gridPos;

	[Export(PropertyHint.None, "")]
	public TowerDefenseCharacterOverride characterOverride;

	public void Init(Dictionary spawnDictionary)
	{
		packetName = spawnDictionary.GetValueOrDefault("Name", Variant.From<string>("")).AsString();
		Array array = spawnDictionary.GetValueOrDefault("GridPos", new Array { 0, 0 }).AsGodotArray();
		gridPos = new Vector2I(array[0].AsInt32(), array[1].AsInt32());
		characterOverride = new TowerDefenseCharacterOverride();
		characterOverride.Init(spawnDictionary.GetValueOrDefault("CharacterOverride", new Dictionary()).AsGodotDictionary());
	}

	public TowerDefenseCharacter SpawnCharacter(Vector2I _gridPos = default(Vector2I), bool editorPreviewMode = false)
	{
		if (_gridPos == default(Vector2I))
		{
			_gridPos = gridPos;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return null;
		}
		bool editorPreviewMode2;
		if (packetConfig.characterConfig.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.PLANT))
		{
			TowerDefenseCharacter towerDefenseCharacter = FindPreSpawnPlantTarget(TowerDefenseManager.GetMapCell(_gridPos), packetConfig);
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				return null;
			}
			editorPreviewMode2 = editorPreviewMode;
			return packetConfig.PlantOnPlant(towerDefenseCharacter, playAudio: false, default, editorPreviewMode2);
		}
		Vector2I vector2I = _gridPos;
		editorPreviewMode2 = editorPreviewMode;
		return packetConfig.Plant(vector2I, playAudio: false, noLimit: true, default, skipPlacementCheck: false, editorPreviewMode2);
	}

	private static TowerDefenseCharacter FindPreSpawnPlantTarget(TowerDefenseCellInstance cell, TowerDefensePacketConfig packet)
	{
		if (!GodotObject.IsInstanceValid(cell) || !GodotObject.IsInstanceValid(packet))
		{
			return null;
		}
		cell.ClearEmpty();
		foreach (TowerDefenseCharacter character in cell.characterList)
		{
			if (GodotObject.IsInstanceValid(character) && character is TowerDefensePlant && character.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT && !character.die && !character.nearDie && GodotObject.IsInstanceValid(character.instance) && !character.instance.invincible && !character.instance.hologram && character.instance.canBeCollection && !character.config.plantGridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.PLANT))
			{
				return character;
			}
		}
		return null;
	}

	public Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["Name"] = packetName,
			["GridPos"] = new Array { gridPos.X, gridPos.Y }
		};
		if (GodotObject.IsInstanceValid(characterOverride))
		{
			dictionary["CharacterOverride"] = characterOverride.Export();
		}
		return dictionary;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "spawnDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "_gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "editorPreviewMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindPreSpawnPlantTarget, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnCharacter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(SpawnCharacter(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.FindPreSpawnPlantTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindPreSpawnPlantTarget(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FindPreSpawnPlantTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(FindPreSpawnPlantTarget(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.SpawnCharacter)
		{
			return true;
		}
		if (method == MethodName.FindPreSpawnPlantTarget)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.packetName)
		{
			packetName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			gridPos = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.characterOverride)
		{
			characterOverride = VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.packetName)
		{
			value = VariantUtils.CreateFrom(in packetName);
			return true;
		}
		if (name == PropertyName.gridPos)
		{
			value = VariantUtils.CreateFrom(in gridPos);
			return true;
		}
		if (name == PropertyName.characterOverride)
		{
			value = VariantUtils.CreateFrom(in characterOverride);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.packetName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.gridPos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterOverride, PropertyHint.ResourceType, "TowerDefenseCharacterOverride", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetName, Variant.From(in packetName));
		info.AddProperty(PropertyName.gridPos, Variant.From(in gridPos));
		info.AddProperty(PropertyName.characterOverride, Variant.From(in characterOverride));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packetName, out var value))
		{
			packetName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.gridPos, out var value2))
		{
			gridPos = value2.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.characterOverride, out var value3))
		{
			characterOverride = value3.As<TowerDefenseCharacterOverride>();
		}
	}
}
