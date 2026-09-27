using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventGeneExplode.cs")]
public class TowerDefenseCharacterEventGeneExplode : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public static readonly StringName Run = "Run";

		public static readonly StringName IsProtectedByPumpkinRobot = "IsProtectedByPumpkinRobot";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName plantTransformName = "plantTransformName";

		public static readonly StringName zombieTransformName = "zombieTransformName";

		public static readonly StringName effectScene = "effectScene";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string plantTransformName = "PlantPeaShooterSingle";

	[Export(PropertyHint.None, "")]
	public string zombieTransformName = "ZombieNormal";

	[Export(PropertyHint.None, "")]
	public PackedScene effectScene;

	private static PackedScene _DEFAULT_GENETIC_EFFECT;

	private static PackedScene DEFAULT_GENETIC_EFFECT => _DEFAULT_GENETIC_EFFECT ?? (_DEFAULT_GENETIC_EFFECT = GD.Load<PackedScene>("uid://k8ms8gteeqp6"));

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(target, pos, plantTransformName, zombieTransformName, effectScene ?? DEFAULT_GENETIC_EFFECT);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(target, pos, plantTransformName, zombieTransformName, effectScene ?? DEFAULT_GENETIC_EFFECT);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(target, projectile.GlobalPosition, plantTransformName, zombieTransformName, effectScene ?? DEFAULT_GENETIC_EFFECT);
	}

	public static void Run(TowerDefenseCharacter target, Vector2 pos, string plantTransformName = "PlantPeaShooter", string zombieTransformName = "ZombieNormal", PackedScene effectScene = null)
	{
		if (!GodotObject.IsInstanceValid(target) || target.die || target.nearDie)
		{
			return;
		}
		if (target is TowerDefenseZombie towerDefenseZombie)
		{
			if ((GodotObject.IsInstanceValid(towerDefenseZombie.instance) ? towerDefenseZombie.instance.zombiePhysique : ((towerDefenseZombie.config as TowerDefenseZombieConfig)?.physique ?? TowerDefenseEnum.ZOMBIE_PHYSIQUE.NORMAL)) != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
			{
				target.TransformTo(zombieTransformName, null, effectScene);
			}
		}
		else if (target is TowerDefensePlant)
		{
			if (!IsProtectedByPumpkinRobot(target))
			{
				TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(target.gridPos);
				if (GodotObject.IsInstanceValid(mapCell))
				{
					mapCell.TransformPlantsTo(plantTransformName, effectScene, IsProtectedByPumpkinRobot);
				}
			}
		}
		else
		{
			TowerDefenseCharacterEventExplodeHurt towerDefenseCharacterEventExplodeHurt = new TowerDefenseCharacterEventExplodeHurt();
			towerDefenseCharacterEventExplodeHurt.num = 1800.0;
			towerDefenseCharacterEventExplodeHurt.Execute(pos, target);
		}
	}

	private static bool IsProtectedByPumpkinRobot(TowerDefenseCharacter character)
	{
		if (character is TowerDefensePlantPumpkinRobot)
		{
			return true;
		}
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(character.gridPos);
		if (mapCell == null || !GodotObject.IsInstanceValid(mapCell.characterSurround))
		{
			return false;
		}
		return mapCell.characterSurround is TowerDefensePlantPumpkinRobot;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
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
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "plantTransformName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "zombieTransformName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "effectScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsProtectedByPumpkinRobot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
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
		if (method == MethodName.Run && args.Count == 5)
		{
			Run(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<PackedScene>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsProtectedByPumpkinRobot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProtectedByPumpkinRobot(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Run && args.Count == 5)
		{
			Run(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<PackedScene>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsProtectedByPumpkinRobot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProtectedByPumpkinRobot(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
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
		if (method == MethodName.Run)
		{
			return true;
		}
		if (method == MethodName.IsProtectedByPumpkinRobot)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.plantTransformName)
		{
			plantTransformName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.zombieTransformName)
		{
			zombieTransformName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.effectScene)
		{
			effectScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.plantTransformName)
		{
			value = VariantUtils.CreateFrom(in plantTransformName);
			return true;
		}
		if (name == PropertyName.zombieTransformName)
		{
			value = VariantUtils.CreateFrom(in zombieTransformName);
			return true;
		}
		if (name == PropertyName.effectScene)
		{
			value = VariantUtils.CreateFrom(in effectScene);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.plantTransformName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.zombieTransformName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.effectScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.plantTransformName, Variant.From(in plantTransformName));
		info.AddProperty(PropertyName.zombieTransformName, Variant.From(in zombieTransformName));
		info.AddProperty(PropertyName.effectScene, Variant.From(in effectScene));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.plantTransformName, out var value))
		{
			plantTransformName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.zombieTransformName, out var value2))
		{
			zombieTransformName = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.effectScene, out var value3))
		{
			effectScene = value3.As<PackedScene>();
		}
	}
}
