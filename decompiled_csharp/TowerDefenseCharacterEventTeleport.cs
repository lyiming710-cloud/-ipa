using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/Special/TowerDefenseCharacterEventTeleport.cs")]
public class TowerDefenseCharacterEventTeleport : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public new static readonly StringName ExecuteGroundProject = "ExecuteGroundProject";

		public static readonly StringName Run = "Run";

		public static readonly StringName TeleportZombie = "TeleportZombie";

		public static readonly StringName TeleportAwayFromHouse = "TeleportAwayFromHouse";

		public static readonly StringName TeleportTowardHouse = "TeleportTowardHouse";

		public static readonly StringName EraseTarget = "EraseTarget";

		public static readonly StringName SpawnTeleportEffect = "SpawnTeleportEffect";

		public static readonly StringName SpawnEraseEffect = "SpawnEraseEffect";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName teleportEffectScene = "teleportEffectScene";

		public static readonly StringName eraseEffectScene = "eraseEffectScene";

		public static readonly StringName bossDamage = "bossDamage";

		public static readonly StringName hpPercentDamage = "hpPercentDamage";

		public static readonly StringName eraseHpRatio = "eraseHpRatio";

		public static readonly StringName teleportLength = "teleportLength";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public PackedScene teleportEffectScene;

	[Export(PropertyHint.None, "")]
	public PackedScene eraseEffectScene;

	[Export(PropertyHint.None, "")]
	public double bossDamage = 500.0;

	[Export(PropertyHint.None, "")]
	public double hpPercentDamage = 0.05;

	[Export(PropertyHint.None, "")]
	public double eraseHpRatio = 0.1;

	[Export(PropertyHint.None, "")]
	public int teleportLength = 1;

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(pos, target, teleportEffectScene, eraseEffectScene, bossDamage, hpPercentDamage, eraseHpRatio, teleportLength);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(pos, target, teleportEffectScene, eraseEffectScene, bossDamage * delta, hpPercentDamage * delta, eraseHpRatio, teleportLength);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(projectile.GlobalPosition, target, teleportEffectScene, eraseEffectScene, bossDamage, hpPercentDamage, eraseHpRatio, teleportLength);
	}

	public override void ExecuteGroundProject(TowerDefenseProjectile projectile, Vector2 position, Vector2I gridPos)
	{
		Run(projectile.GlobalPosition, null, teleportEffectScene, eraseEffectScene, bossDamage, hpPercentDamage, eraseHpRatio, teleportLength);
	}

	public static void Run(Vector2 pos, TowerDefenseCharacter target, PackedScene teleportEffectScene, PackedScene eraseEffectScene, double bossDamage, double hpPercentDamage, double eraseHpRatio, int teleportLength)
	{
		if (!GodotObject.IsInstanceValid(target) || !GodotObject.IsInstanceValid(target.instance) || target.isDestroy)
		{
			return;
		}
		Vector2I gridPos = target.gridPos;
		double totalHitPoint = target.GetTotalHitPoint();
		double currentHitPoint = target.GetCurrentHitPoint();
		double num = ((totalHitPoint > 0.0) ? (currentHitPoint / totalHitPoint) : 0.0);
		bool flag = target is TowerDefenseZombie && target.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS;
		bool flag2 = GodotObject.IsInstanceValid(target.instance) && target.instance.hitpoints > target.instance.hitpointsSave;
		bool flag3 = !flag && !flag2 && num <= eraseHpRatio;
		SpawnTeleportEffect(gridPos, teleportEffectScene);
		if (flag3)
		{
			SpawnEraseEffect(gridPos, eraseEffectScene);
		}
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		if (flag)
		{
			TowerDefenseManager.Instance.damagePipeline.ApplyHurt(target, bossDamage, playSplatAudio: false);
			return;
		}
		if (flag3)
		{
			EraseTarget(target, eraseEffectScene);
			return;
		}
		double num2 = totalHitPoint * hpPercentDamage;
		TowerDefenseManager.Instance.damagePipeline.ApplyHurt(target, num2, playSplatAudio: false);
		if (target is TowerDefenseZombie && GodotObject.IsInstanceValid(target) && !target.isDestroy)
		{
			bool reverseDirection = GodotObject.IsInstanceValid(target.instance) && target.instance.hypnoses;
			TeleportZombie(target, reverseDirection, teleportLength);
		}
	}

	private static void TeleportZombie(TowerDefenseCharacter target, bool reverseDirection, int teleportLength)
	{
		int num = Mathf.Max(0, teleportLength);
		if (num != 0 && GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
		{
			if (!reverseDirection)
			{
				TeleportAwayFromHouse(target, num);
			}
			else
			{
				TeleportTowardHouse(target, num);
			}
		}
	}

	private static void TeleportAwayFromHouse(TowerDefenseCharacter target, int length)
	{
		if (target.gridPos.X >= 1)
		{
			Vector2I gridPos = (target.gridPos = new Vector2I(target.gridPos.X + length, target.gridPos.Y));
			target.SetLogicalGlobalPosition(TowerDefenseManager.GetMapCellPlantPos(gridPos));
		}
	}

	private static void TeleportTowardHouse(TowerDefenseCharacter target, int length)
	{
		Vector2I gridPos = (target.gridPos = new Vector2I(target.gridPos.X - length, target.gridPos.Y));
		target.SetLogicalGlobalPosition(TowerDefenseManager.GetMapCellPlantPos(gridPos));
	}

	private static void EraseTarget(TowerDefenseCharacter target, PackedScene eraseEffectScene)
	{
		if (GodotObject.IsInstanceValid(target))
		{
			target.skipDestroySet = true;
			TowerDefenseCellInstance towerDefenseCellInstance = (GodotObject.IsInstanceValid(target.cell) ? target.cell : TowerDefenseManager.GetMapCell(target.gridPos));
			if (GodotObject.IsInstanceValid(towerDefenseCellInstance))
			{
				towerDefenseCellInstance.RemoveCharacter(target);
			}
			target.Destroy(freeInstance: false);
			if (GodotObject.IsInstanceValid(TowerDefenseManager.Instance))
			{
				TowerDefenseManager.Instance.CharacterUnregister(target);
			}
			target.RemoveFromGroup("Character");
			if (!target.IsQueuedForDeletion())
			{
				target.QueueFree();
			}
		}
	}

	private static void SpawnTeleportEffect(Vector2I gridPos, PackedScene teleportEffectScene)
	{
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		if (GodotObject.IsInstanceValid(characterNode) && GodotObject.IsInstanceValid(teleportEffectScene))
		{
			TowerDefenseEffectBase towerDefenseEffectBase = TowerDefenseManager.CreateEffectSpriteOnce(teleportEffectScene, gridPos, "Idle");
			if (GodotObject.IsInstanceValid(towerDefenseEffectBase))
			{
				characterNode.AddChild(towerDefenseEffectBase, forceReadableName: false, Node.InternalMode.Disabled);
				towerDefenseEffectBase.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(gridPos);
			}
		}
	}

	private static void SpawnEraseEffect(Vector2I gridPos, PackedScene eraseEffectScene)
	{
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		if (GodotObject.IsInstanceValid(characterNode) && GodotObject.IsInstanceValid(eraseEffectScene))
		{
			TowerDefenseEffectBase towerDefenseEffectBase = TowerDefenseManager.CreateEffectParticlesOnce(eraseEffectScene, gridPos);
			if (GodotObject.IsInstanceValid(towerDefenseEffectBase))
			{
				characterNode.AddChild(towerDefenseEffectBase, forceReadableName: false, Node.InternalMode.Disabled);
				towerDefenseEffectBase.GlobalPosition = TowerDefenseManager.GetMapCellPlantPos(gridPos);
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
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
			new MethodInfo(MethodName.Run, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "teleportEffectScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Object, "eraseEffectScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Float, "bossDamage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "hpPercentDamage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "eraseHpRatio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "teleportLength", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TeleportZombie, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "reverseDirection", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "teleportLength", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TeleportAwayFromHouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "length", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TeleportTowardHouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "length", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EraseTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "eraseEffectScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnTeleportEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "teleportEffectScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnEraseEffect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "eraseEffectScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
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
		if (method == MethodName.ExecuteGroundProject && args.Count == 3)
		{
			ExecuteGroundProject(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2I>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.Run && args.Count == 8)
		{
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<PackedScene>(in args[2]), VariantUtils.ConvertTo<PackedScene>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]));
			ret = default;
			return true;
		}
		if (method == MethodName.TeleportZombie && args.Count == 3)
		{
			TeleportZombie(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.TeleportAwayFromHouse && args.Count == 2)
		{
			TeleportAwayFromHouse(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TeleportTowardHouse && args.Count == 2)
		{
			TeleportTowardHouse(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EraseTarget && args.Count == 2)
		{
			EraseTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnTeleportEffect && args.Count == 2)
		{
			SpawnTeleportEffect(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnEraseEffect && args.Count == 2)
		{
			SpawnEraseEffect(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Run && args.Count == 8)
		{
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<PackedScene>(in args[2]), VariantUtils.ConvertTo<PackedScene>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]));
			ret = default;
			return true;
		}
		if (method == MethodName.TeleportZombie && args.Count == 3)
		{
			TeleportZombie(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.TeleportAwayFromHouse && args.Count == 2)
		{
			TeleportAwayFromHouse(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TeleportTowardHouse && args.Count == 2)
		{
			TeleportTowardHouse(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EraseTarget && args.Count == 2)
		{
			EraseTarget(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnTeleportEffect && args.Count == 2)
		{
			SpawnTeleportEffect(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnEraseEffect && args.Count == 2)
		{
			SpawnEraseEffect(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1]));
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
		if (method == MethodName.ExecuteGroundProject)
		{
			return true;
		}
		if (method == MethodName.Run)
		{
			return true;
		}
		if (method == MethodName.TeleportZombie)
		{
			return true;
		}
		if (method == MethodName.TeleportAwayFromHouse)
		{
			return true;
		}
		if (method == MethodName.TeleportTowardHouse)
		{
			return true;
		}
		if (method == MethodName.EraseTarget)
		{
			return true;
		}
		if (method == MethodName.SpawnTeleportEffect)
		{
			return true;
		}
		if (method == MethodName.SpawnEraseEffect)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.teleportEffectScene)
		{
			teleportEffectScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.eraseEffectScene)
		{
			eraseEffectScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.bossDamage)
		{
			bossDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.hpPercentDamage)
		{
			hpPercentDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.eraseHpRatio)
		{
			eraseHpRatio = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.teleportLength)
		{
			teleportLength = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.teleportEffectScene)
		{
			value = VariantUtils.CreateFrom(in teleportEffectScene);
			return true;
		}
		if (name == PropertyName.eraseEffectScene)
		{
			value = VariantUtils.CreateFrom(in eraseEffectScene);
			return true;
		}
		if (name == PropertyName.bossDamage)
		{
			value = VariantUtils.CreateFrom(in bossDamage);
			return true;
		}
		if (name == PropertyName.hpPercentDamage)
		{
			value = VariantUtils.CreateFrom(in hpPercentDamage);
			return true;
		}
		if (name == PropertyName.eraseHpRatio)
		{
			value = VariantUtils.CreateFrom(in eraseHpRatio);
			return true;
		}
		if (name == PropertyName.teleportLength)
		{
			value = VariantUtils.CreateFrom(in teleportLength);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.teleportEffectScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.eraseEffectScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.bossDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.hpPercentDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.eraseHpRatio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.teleportLength, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.teleportEffectScene, Variant.From(in teleportEffectScene));
		info.AddProperty(PropertyName.eraseEffectScene, Variant.From(in eraseEffectScene));
		info.AddProperty(PropertyName.bossDamage, Variant.From(in bossDamage));
		info.AddProperty(PropertyName.hpPercentDamage, Variant.From(in hpPercentDamage));
		info.AddProperty(PropertyName.eraseHpRatio, Variant.From(in eraseHpRatio));
		info.AddProperty(PropertyName.teleportLength, Variant.From(in teleportLength));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.teleportEffectScene, out var value))
		{
			teleportEffectScene = value.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.eraseEffectScene, out var value2))
		{
			eraseEffectScene = value2.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.bossDamage, out var value3))
		{
			bossDamage = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.hpPercentDamage, out var value4))
		{
			hpPercentDamage = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.eraseHpRatio, out var value5))
		{
			eraseHpRatio = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName.teleportLength, out var value6))
		{
			teleportLength = value6.As<int>();
		}
	}
}
