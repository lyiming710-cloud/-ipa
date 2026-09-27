using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventDieSpawn.cs")]
public class TowerDefenseCharacterEventDieSpawn : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";

		public static readonly StringName Run = "Run";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName packetName = "packetName";

		public static readonly StringName percentage = "percentage";

		public static readonly StringName useGridPos = "useGridPos";

		public static readonly StringName byCamp = "byCamp";

		public static readonly StringName offset = "offset";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string packetName = "";

	[Export(PropertyHint.None, "")]
	public double percentage = 1.0;

	[Export(PropertyHint.None, "")]
	public bool useGridPos = true;

	[Export(PropertyHint.None, "")]
	public bool byCamp;

	[Export(PropertyHint.None, "")]
	public Vector2 offset = Vector2.Zero;

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(pos, target, packetName, percentage, useGridPos, byCamp, offset);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(pos, target, packetName, percentage, useGridPos, byCamp, offset);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(target.GetLogicalGlobalPosition(), target, packetName, percentage, useGridPos, byCamp, offset);
	}

	public override void Init(Dictionary valueDictionary)
	{
		packetName = valueDictionary.GetValueOrDefault("PacketName", Variant.From<string>("")).AsString();
		percentage = valueDictionary.GetValueOrDefault("Percentage", Variant.From<double>(1.0)).AsDouble();
		useGridPos = valueDictionary.GetValueOrDefault("UseGridPos", Variant.From<bool>(true)).AsBool();
		byCamp = valueDictionary.GetValueOrDefault("ByCamp", Variant.From<bool>(false)).AsBool();
		offset = valueDictionary.GetValueOrDefault("Offset", Variant.From<Vector2>(Vector2.Zero)).AsVector2();
	}

	public override Dictionary Export()
	{
		return new Dictionary
		{
			["EventName"] = "DieSpawn",
			["Value"] = new Dictionary
			{
				["PacketName"] = packetName,
				["Percentage"] = percentage,
				["UseGridPos"] = useGridPos,
				["ByCamp"] = byCamp,
				["Offset"] = offset
			}
		};
	}

	public static void Run(Vector2 pos, TowerDefenseCharacter target, string packetName, double percentage, bool useGridPos, bool byCamp, Vector2 offset)
	{
		if ((double)GD.Randf() > percentage || string.IsNullOrEmpty(packetName))
		{
			return;
		}
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(packetName);
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return;
		}
		Vector2 pos2 = pos + offset;
		if (useGridPos)
		{
			TowerDefenseCharacter towerDefenseCharacter = packetConfig.Plant(target.gridPos);
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && byCamp && target.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT)
			{
				towerDefenseCharacter.Hypnoses();
			}
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter2 = packetConfig.Create(pos2, target.gridPos, target.groundHeight);
		if (GodotObject.IsInstanceValid(towerDefenseCharacter2))
		{
			TowerDefenseManager.GetCharacterNode().AddChild(towerDefenseCharacter2, forceReadableName: false, Node.InternalMode.Disabled);
			if (byCamp && target.camp == TowerDefenseEnum.CHARACTER_CAMP.PLANT)
			{
				towerDefenseCharacter2.Hypnoses();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
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
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "percentage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "useGridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "byCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<Vector2>(in args[6]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Run && args.Count == 7)
		{
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<Vector2>(in args[6]));
			ret = default;
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
		if (name == PropertyName.useGridPos)
		{
			useGridPos = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.byCamp)
		{
			byCamp = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.offset)
		{
			offset = VariantUtils.ConvertTo<Vector2>(in value);
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
		if (name == PropertyName.useGridPos)
		{
			value = VariantUtils.CreateFrom(in useGridPos);
			return true;
		}
		if (name == PropertyName.byCamp)
		{
			value = VariantUtils.CreateFrom(in byCamp);
			return true;
		}
		if (name == PropertyName.offset)
		{
			value = VariantUtils.CreateFrom(in offset);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.useGridPos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.byCamp, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.offset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetName, Variant.From(in packetName));
		info.AddProperty(PropertyName.percentage, Variant.From(in percentage));
		info.AddProperty(PropertyName.useGridPos, Variant.From(in useGridPos));
		info.AddProperty(PropertyName.byCamp, Variant.From(in byCamp));
		info.AddProperty(PropertyName.offset, Variant.From(in offset));
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
		if (info.TryGetProperty(PropertyName.useGridPos, out var value3))
		{
			useGridPos = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.byCamp, out var value4))
		{
			byCamp = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.offset, out var value5))
		{
			offset = value5.As<Vector2>();
		}
	}
}
