using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventProjectileCreate.cs")]
public class TowerDefenseCharacterEventProjectileCreate : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public new static readonly StringName ExecuteGroundProject = "ExecuteGroundProject";

		public new static readonly StringName Init = "Init";

		public new static readonly StringName Export = "Export";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName projectileName = "projectileName";

		public static readonly StringName projectileData = "projectileData";

		public static readonly StringName createNum = "createNum";

		public static readonly StringName createGridToRange = "createGridToRange";

		public static readonly StringName createSpeedToRange = "createSpeedToRange";

		public static readonly StringName useFall = "useFall";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public TowerDefenseProjectileCreateData projectileData;

	[Export(PropertyHint.None, "")]
	public int createNum = 8;

	[Export(PropertyHint.None, "")]
	public Vector4I createGridToRange = new Vector4I(-1, -1, 1, 1);

	[Export(PropertyHint.None, "")]
	public Vector2 createSpeedToRange = new Vector2(-100f, 100f);

	[Export(PropertyHint.None, "")]
	public bool useFall;

	public string projectileName
	{
		get
		{
			if (projectileData != null)
			{
				return projectileData.projectileName.ToString();
			}
			return "";
		}
	}

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(target, projectileData, createNum, createGridToRange, createSpeedToRange, useFall);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(target, projectileData, createNum, createGridToRange, createSpeedToRange, useFall);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(target, projectileData, createNum, createGridToRange, createSpeedToRange, useFall, projectile.collisionFlags, projectile.camp);
	}

	public override void ExecuteGroundProject(TowerDefenseProjectile projectile, Vector2 position, Vector2I gridPos)
	{
		RunAtPosition(position, gridPos, projectileData, createNum, createGridToRange, createSpeedToRange, useFall, projectile.collisionFlags, projectile.camp);
	}

	public override void Init(Dictionary valueDictionary)
	{
		string text = valueDictionary.GetValueOrDefault("ProjectileName", "").AsString();
		if (text != "")
		{
			projectileData = new TowerDefenseProjectileCreateData(new StringName(text));
		}
		createNum = valueDictionary.GetValueOrDefault("CreateNum", 8).AsInt32();
		createGridToRange = valueDictionary.GetValueOrDefault("CreateGridToRange", new Vector4I(-1, -1, 1, 1)).AsVector4I();
		createSpeedToRange = valueDictionary.GetValueOrDefault("CreateSpeedToRange", new Vector2(-100f, 100f)).AsVector2();
		useFall = valueDictionary.GetValueOrDefault("useFall", false).AsBool();
	}

	public override Dictionary Export()
	{
		return new Dictionary
		{
			["EventName"] = "ProjectileCreate",
			["Value"] = new Dictionary
			{
				["ProjectileName"] = projectileName,
				["CreateNum"] = createNum,
				["CreateSpeedToRange"] = createSpeedToRange,
				["CreateGridToRange"] = createGridToRange,
				["useFall"] = useFall
			}
		};
	}

	public static void Run(TowerDefenseCharacter target, TowerDefenseProjectileCreateData _projectileData, int _createNum = 8, Vector4I? _createGridToRange = null, Vector2? _createToRange = null, bool _useFall = false, int _collisionFlags = -1, TowerDefenseEnum.CHARACTER_CAMP _camp = TowerDefenseEnum.CHARACTER_CAMP.ALL)
	{
		if (GodotObject.IsInstanceValid(target))
		{
			RunAtPosition(target.GetLogicalGlobalPosition(), target.gridPos, _projectileData, _createNum, _createGridToRange, _createToRange, _useFall, _collisionFlags, _camp);
		}
	}

	public static void RunAtPosition(Vector2 position, Vector2I gridPos, TowerDefenseProjectileCreateData _projectileData, int _createNum = 8, Vector4I? _createGridToRange = null, Vector2? _createToRange = null, bool _useFall = false, int _collisionFlags = -1, TowerDefenseEnum.CHARACTER_CAMP _camp = TowerDefenseEnum.CHARACTER_CAMP.ALL)
	{
		Vector4I vector4I = _createGridToRange ?? new Vector4I(-1, -1, 1, 1);
		Vector2 vector = _createToRange ?? new Vector2(-100f, 100f);
		for (int i = 0; i < _createNum; i++)
		{
			Vector2I vector2I = new Vector2I(GD.RandRange(vector4I.X, vector4I.Z), GD.RandRange(vector4I.Y, vector4I.W));
			Vector2 vector2 = new Vector2((float)GD.RandRange(vector.X, vector.Y), vector2I.Y * 100);
			BulletFieldSpawnOverrides overrides = new BulletFieldSpawnOverrides
			{
				gridYOverride = gridPos.Y + vector2I.Y
			};
			if (_useFall)
			{
				overrides.useFall = true;
				overrides.zOverride = 600.0;
				overrides.ySpeedOverride = 400.0;
			}
			else
			{
				overrides.useGravity = true;
				overrides.ySpeedOverride = -800.0;
				overrides.gravityOverride = 980.0;
			}
			FireComponent.CreateProjectilePosition(null, null, 0.0, position, vector2 / 1.5f, _projectileData, _collisionFlags, _camp, default, overrides);
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
			new MethodInfo(MethodName.ExecuteGroundProject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "valueDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Export, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ExecuteGroundProject && args.Count == 3)
		{
			ExecuteGroundProject(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]));
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
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
		if (method == MethodName.ExecuteGroundProject)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.projectileData)
		{
			projectileData = VariantUtils.ConvertTo<TowerDefenseProjectileCreateData>(in value);
			return true;
		}
		if (name == PropertyName.createNum)
		{
			createNum = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.createGridToRange)
		{
			createGridToRange = VariantUtils.ConvertTo<Vector4I>(in value);
			return true;
		}
		if (name == PropertyName.createSpeedToRange)
		{
			createSpeedToRange = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.useFall)
		{
			useFall = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.projectileName)
		{
			value = VariantUtils.CreateFrom<string>(projectileName);
			return true;
		}
		if (name == PropertyName.projectileData)
		{
			value = VariantUtils.CreateFrom(in projectileData);
			return true;
		}
		if (name == PropertyName.createNum)
		{
			value = VariantUtils.CreateFrom(in createNum);
			return true;
		}
		if (name == PropertyName.createGridToRange)
		{
			value = VariantUtils.CreateFrom(in createGridToRange);
			return true;
		}
		if (name == PropertyName.createSpeedToRange)
		{
			value = VariantUtils.CreateFrom(in createSpeedToRange);
			return true;
		}
		if (name == PropertyName.useFall)
		{
			value = VariantUtils.CreateFrom(in useFall);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.projectileData, PropertyHint.ResourceType, "TowerDefenseProjectileCreateData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.createNum, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector4I, PropertyName.createGridToRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.createSpeedToRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useFall, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.projectileName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.projectileData, Variant.From(in projectileData));
		info.AddProperty(PropertyName.createNum, Variant.From(in createNum));
		info.AddProperty(PropertyName.createGridToRange, Variant.From(in createGridToRange));
		info.AddProperty(PropertyName.createSpeedToRange, Variant.From(in createSpeedToRange));
		info.AddProperty(PropertyName.useFall, Variant.From(in useFall));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.projectileData, out var value))
		{
			projectileData = value.As<TowerDefenseProjectileCreateData>();
		}
		if (info.TryGetProperty(PropertyName.createNum, out var value2))
		{
			createNum = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.createGridToRange, out var value3))
		{
			createGridToRange = value3.As<Vector4I>();
		}
		if (info.TryGetProperty(PropertyName.createSpeedToRange, out var value4))
		{
			createSpeedToRange = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.useFall, out var value5))
		{
			useFall = value5.As<bool>();
		}
	}
}
