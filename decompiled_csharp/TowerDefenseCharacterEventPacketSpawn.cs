using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventPacketSpawn.cs")]
public class TowerDefenseCharacterEventPacketSpawn : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";

		public static readonly StringName Run = "Run";

		public static readonly StringName SpawnZombieAtTarget = "SpawnZombieAtTarget";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName packetName = "packetName";

		public static readonly StringName percentage = "percentage";

		public static readonly StringName dieSpawn = "dieSpawn";

		public static readonly StringName byCamp = "byCamp";

		public static readonly StringName characterOverride = "characterOverride";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string packetName = "";

	[Export(PropertyHint.None, "")]
	public double percentage = 1.0;

	[Export(PropertyHint.None, "")]
	public bool dieSpawn;

	[Export(PropertyHint.None, "")]
	public bool byCamp;

	[Export(PropertyHint.None, "")]
	public TowerDefenseCharacterOverride characterOverride;

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(target, packetName, percentage, dieSpawn, byCamp, target.camp, characterOverride);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(target, packetName, percentage, dieSpawn, byCamp, target.camp, characterOverride);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(target, packetName, percentage, dieSpawn, byCamp, projectile.camp, characterOverride);
	}

	public override void Init(Dictionary valueDictionary)
	{
		packetName = valueDictionary.GetValueOrDefault("PacketName", "").AsString();
		percentage = valueDictionary.GetValueOrDefault("Percentage", 1.0).AsDouble();
		dieSpawn = valueDictionary.GetValueOrDefault("DieSpawn", false).AsBool();
		byCamp = valueDictionary.GetValueOrDefault("ByCamp", false).AsBool();
		Dictionary dictionary = valueDictionary.GetValueOrDefault("CharacterOverride", new Dictionary()).AsGodotDictionary();
		if (dictionary.Count > 0)
		{
			characterOverride = new TowerDefenseCharacterOverride();
			characterOverride.Init(dictionary);
		}
	}

	public override Dictionary Export()
	{
		Dictionary dictionary = new Dictionary
		{
			["EventName"] = "PacketSpawn",
			["Value"] = new Dictionary
			{
				["PacketName"] = packetName,
				["Percentage"] = percentage,
				["DieSpawn"] = dieSpawn,
				["ByCamp"] = byCamp
			}
		};
		if (GodotObject.IsInstanceValid(characterOverride))
		{
			((Dictionary)dictionary["Value"])["CharacterOverride"] = characterOverride.Export();
		}
		return dictionary;
	}

	public static void Run(TowerDefenseCharacter target, string _packetName, double _percentage, bool _dieSpawn = false, bool _byCamp = false, TowerDefenseEnum.CHARACTER_CAMP _sourceCamp = TowerDefenseEnum.CHARACTER_CAMP.PLANT, TowerDefenseCharacterOverride _characterOverride = null)
	{
		if ((double)GD.Randf() > _percentage || (_dieSpawn && (!_dieSpawn || (!target.instance.die && !target.instance.nearDie))))
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(_packetName);
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return;
		}
		if (GodotObject.IsInstanceValid(_characterOverride))
		{
			if (!GodotObject.IsInstanceValid(packetConfig._override))
			{
				packetConfig._override = new TowerDefensePacketOverride();
			}
			packetConfig._override.characterOverride = _characterOverride;
		}
		TowerDefenseCharacter towerDefenseCharacter = ((!(packetConfig.characterConfig is TowerDefenseZombieConfig)) ? packetConfig.Plant(target.gridPos) : SpawnZombieAtTarget(packetConfig, target));
		if (GodotObject.IsInstanceValid(towerDefenseCharacter) && _byCamp && towerDefenseCharacter.camp != _sourceCamp)
		{
			towerDefenseCharacter.Hypnoses();
		}
	}

	private static TowerDefenseCharacter SpawnZombieAtTarget(TowerDefensePacketConfig packetConfig, TowerDefenseCharacter target)
	{
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(characterNode))
		{
			return null;
		}
		TowerDefenseCharacter towerDefenseCharacter = packetConfig.Create(Vector2.Zero, target.gridPos, target.groundHeight);
		if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			return null;
		}
		characterNode.AddChild(towerDefenseCharacter, forceReadableName: false, Node.InternalMode.Disabled);
		if (!(towerDefenseCharacter.gridPos != target.gridPos))
		{
			towerDefenseCharacter.SetLogicalGlobalPosition(target.GetLogicalGlobalPosition());
		}
		if (packetConfig.packetFlip)
		{
			towerDefenseCharacter.Scale = new Vector2(0f - towerDefenseCharacter.Scale.X, towerDefenseCharacter.Scale.Y);
		}
		if (packetConfig.GetHypnoses())
		{
			towerDefenseCharacter.Hypnoses();
		}
		if (GodotObject.IsInstanceValid(packetConfig._override?.characterOverride))
		{
			packetConfig._override.characterOverride.ExecuteCharacter(towerDefenseCharacter);
		}
		packetConfig.IsZombieWalk(towerDefenseCharacter);
		ShadowComponent shadowComponent = towerDefenseCharacter.shadowComponent;
		if (shadowComponent != null && !shadowComponent.IsReleased)
		{
			towerDefenseCharacter.shadowComponent.Init();
			towerDefenseCharacter.shadowComponent.UpdateShadow();
		}
		return towerDefenseCharacter;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteDps, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteProject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "valueDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "_packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "_percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_dieSpawn", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_byCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "_sourceCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_characterOverride", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnZombieAtTarget, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Execute && args.Count == 2)
		{
			Execute(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteDps && args.Count == 3)
		{
			ExecuteDps(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExecuteProject && args.Count == 2)
		{
			ExecuteProject(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Export && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(Export());
			return true;
		}
		if (method == MethodName.Run && args.Count == 7)
		{
			Run(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[5]), VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnZombieAtTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(SpawnZombieAtTarget(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Run && args.Count == 7)
		{
			Run(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[5]), VariantUtils.ConvertTo<TowerDefenseCharacterOverride>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnZombieAtTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(SpawnZombieAtTarget(VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Execute)
		{
			return true;
		}
		if (method == MethodName.ExecuteDps)
		{
			return true;
		}
		if (method == MethodName.ExecuteProject)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.Export)
		{
			return true;
		}
		if (method == MethodName.Run)
		{
			return true;
		}
		if (method == MethodName.SpawnZombieAtTarget)
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
		if (name == PropertyName.percentage)
		{
			percentage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.dieSpawn)
		{
			dieSpawn = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.byCamp)
		{
			byCamp = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.percentage)
		{
			value = VariantUtils.CreateFrom(in percentage);
			return true;
		}
		if (name == PropertyName.dieSpawn)
		{
			value = VariantUtils.CreateFrom(in dieSpawn);
			return true;
		}
		if (name == PropertyName.byCamp)
		{
			value = VariantUtils.CreateFrom(in byCamp);
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
			new PropertyInfo(Variant.Type.Float, PropertyName.percentage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.dieSpawn, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.byCamp, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.characterOverride, PropertyHint.ResourceType, "TowerDefenseCharacterOverride", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetName, Variant.From(in packetName));
		info.AddProperty(PropertyName.percentage, Variant.From(in percentage));
		info.AddProperty(PropertyName.dieSpawn, Variant.From(in dieSpawn));
		info.AddProperty(PropertyName.byCamp, Variant.From(in byCamp));
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
		if (info.TryGetProperty(PropertyName.percentage, out var value2))
		{
			percentage = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.dieSpawn, out var value3))
		{
			dieSpawn = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.byCamp, out var value4))
		{
			byCamp = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.characterOverride, out var value5))
		{
			characterOverride = value5.As<TowerDefenseCharacterOverride>();
		}
	}
}
