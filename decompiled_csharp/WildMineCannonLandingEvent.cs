using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Registry/Projectile/Config/WildMineCannon/WildMineCannonLandingEvent.cs")]
public class WildMineCannonLandingEvent : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public new static readonly StringName ExecuteGroundProject = "ExecuteGroundProject";

		public static readonly StringName Run = "Run";

		public static readonly StringName CanPlantOriginalMine = "CanPlantOriginalMine";

		public static readonly StringName IsInsidePlantRegion = "IsInsidePlantRegion";

		public static readonly StringName CellHasEnemyZombie = "CellHasEnemyZombie";

		public static readonly StringName CellHasEnemyCharacter = "CellHasEnemyCharacter";

		public static readonly StringName IsEnemyCharacter = "IsEnemyCharacter";

		public static readonly StringName IsEnemyCamp = "IsEnemyCamp";

		public static readonly StringName Explode = "Explode";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName packetName = "packetName";

		public static readonly StringName explosionEffect = "explosionEffect";

		public static readonly StringName explosionAudio = "explosionAudio";

		public static readonly StringName explosionSize = "explosionSize";

		public static readonly StringName explosionEvents = "explosionEvents";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string packetName = "PlantPotatoMine";

	[Export(PropertyHint.None, "")]
	public PackedScene explosionEffect;

	[Export(PropertyHint.None, "")]
	public string explosionAudio = "MineExplosion";

	[Export(PropertyHint.None, "")]
	public Vector2 explosionSize = new Vector2(1f, 0.25f);

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterEventBase> explosionEvents = new Array<TowerDefenseCharacterEventBase>();

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(target.gridPos, target.camp, packetName, explosionEffect, explosionAudio, explosionSize, explosionEvents);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(target.gridPos, target.camp, packetName, explosionEffect, explosionAudio, explosionSize, explosionEvents);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(target.gridPos, projectile.camp, packetName, explosionEffect, explosionAudio, explosionSize, explosionEvents);
	}

	public override void ExecuteGroundProject(TowerDefenseProjectile projectile, Vector2 position, Vector2I gridPos)
	{
		Run(gridPos, projectile.camp, packetName, explosionEffect, explosionAudio, explosionSize, explosionEvents);
	}

	public static void Run(Vector2I gridPos, TowerDefenseEnum.CHARACTER_CAMP camp, string _packetName, PackedScene _explosionEffect, string _explosionAudio, Vector2 _explosionSize, Array<TowerDefenseCharacterEventBase> _explosionEvents)
	{
		if (Global.IsMultiplayerMode && !MultiPlayerManager.IsHost)
		{
			return;
		}
		TowerDefenseCellInstance mapCell = TowerDefenseManager.GetMapCell(gridPos);
		TowerDefensePacketConfig packetConfig = TowerDefenseManager.GetPacketConfig(_packetName);
		if (CanPlantOriginalMine(mapCell, camp, packetConfig))
		{
			TowerDefenseCharacter spawned = packetConfig.Plant(gridPos);
			if (GodotObject.IsInstanceValid(spawned))
			{
				Callable.From(() =>
				{
					if (GodotObject.IsInstanceValid(spawned) && !spawned.isDestroy)
					{
						if (spawned.camp != camp)
						{
							spawned.Hypnoses();
						}
						spawned.componentManager?.GetRuntime<PotatoComponent>()?.ReadyCharge();
					}
				}).CallDeferred();
				return;
			}
		}
		Explode(gridPos, camp, _explosionEffect, _explosionAudio, _explosionSize, _explosionEvents);
	}

	public static bool CanPlantOriginalMine(TowerDefenseCellInstance cell, TowerDefenseEnum.CHARACTER_CAMP camp, TowerDefensePacketConfig packetConfig)
	{
		if (!IsInsidePlantRegion(cell))
		{
			return false;
		}
		if (CellHasEnemyCharacter(cell, camp))
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(packetConfig))
		{
			return false;
		}
		return cell.CanPacketPlant(packetConfig);
	}

	public static bool IsInsidePlantRegion(TowerDefenseCellInstance cell)
	{
		if (!GodotObject.IsInstanceValid(cell))
		{
			return false;
		}
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return false;
		}
		if (instance.CheckMapGridPosIn(cell.gridPos))
		{
			return GodotObject.IsInstanceValid(TowerDefenseManager.GetMapCell(cell.gridPos));
		}
		return false;
	}

	public static bool CellHasEnemyZombie(TowerDefenseCellInstance cell, TowerDefenseEnum.CHARACTER_CAMP camp)
	{
		TowerDefenseBattleCharacterRegistry towerDefenseBattleCharacterRegistry = TowerDefenseManager.Instance?.characterRegistry;
		if (towerDefenseBattleCharacterRegistry == null)
		{
			return false;
		}
		foreach (TowerDefenseCharacter charactersForGridWindow in towerDefenseBattleCharacterRegistry.GetCharactersForGridWindowList(cell.gridPos.X, cell.gridPos.Y, 0, 0))
		{
			if (charactersForGridWindow is TowerDefenseZombie towerDefenseZombie && towerDefenseZombie.camp != camp && !towerDefenseZombie.die && !towerDefenseZombie.isDestroy)
			{
				return true;
			}
		}
		return false;
	}

	public static bool CellHasEnemyCharacter(TowerDefenseCellInstance cell, TowerDefenseEnum.CHARACTER_CAMP camp)
	{
		TowerDefenseBattleCharacterRegistry towerDefenseBattleCharacterRegistry = TowerDefenseManager.Instance?.characterRegistry;
		if (towerDefenseBattleCharacterRegistry == null)
		{
			return false;
		}
		foreach (TowerDefenseCharacter charactersForGridWindow in towerDefenseBattleCharacterRegistry.GetCharactersForGridWindowList(cell.gridPos.X, cell.gridPos.Y, 0, 0))
		{
			if (IsEnemyCharacter(charactersForGridWindow, camp))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsEnemyCharacter(TowerDefenseCharacter character, TowerDefenseEnum.CHARACTER_CAMP camp)
	{
		if (!GodotObject.IsInstanceValid(character) || character.die || character.isDestroy)
		{
			return false;
		}
		return IsEnemyCamp(character.camp, camp);
	}

	private static bool IsEnemyCamp(TowerDefenseEnum.CHARACTER_CAMP characterCamp, TowerDefenseEnum.CHARACTER_CAMP camp)
	{
		if (characterCamp != TowerDefenseEnum.CHARACTER_CAMP.PLANT && characterCamp != TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
		{
			return false;
		}
		return characterCamp != camp;
	}

	private static void Explode(Vector2I gridPos, TowerDefenseEnum.CHARACTER_CAMP camp, PackedScene explosionEffect, string explosionAudio, Vector2 explosionSize, Array<TowerDefenseCharacterEventBase> explosionEvents)
	{
		Vector2 mapCellPlantPos = TowerDefenseManager.GetMapCellPlantPos(gridPos);
		if (explosionEvents != null && explosionEvents.Count > 0)
		{
			TowerDefenseExplode.CreateExplode(mapCellPlantPos, explosionSize, explosionEvents, new Array<TowerDefenseCharacter>(), camp, -1);
		}
		if (GodotObject.IsInstanceValid(explosionEffect))
		{
			Node2D characterNode = TowerDefenseManager.GetCharacterNode();
			TowerDefenseEffectParticlesOnce towerDefenseEffectParticlesOnce = TowerDefenseManager.CreateEffectParticlesOnce(explosionEffect, gridPos);
			if (GodotObject.IsInstanceValid(towerDefenseEffectParticlesOnce) && GodotObject.IsInstanceValid(characterNode))
			{
				characterNode.AddChild(towerDefenseEffectParticlesOnce, forceReadableName: false, Node.InternalMode.Disabled);
				towerDefenseEffectParticlesOnce.GlobalPosition = mapCellPlantPos;
			}
		}
		if (!string.IsNullOrEmpty(explosionAudio))
		{
			AudioManager.Instance?.AudioPlay(explosionAudio);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
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
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "_packetName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "_explosionEffect", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.String, "_explosionAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "_explosionSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "_explosionEvents", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanPlantOriginalMine, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "packetConfig", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsInsidePlantRegion, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CellHasEnemyZombie, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CellHasEnemyCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsEnemyCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsEnemyCamp, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "characterCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Explode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "gridPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "camp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "explosionEffect", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.String, "explosionAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "explosionSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "explosionEvents", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Run && args.Count == 7)
		{
			Run(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<PackedScene>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<Vector2>(in args[5]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanPlantOriginalMine && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(CanPlantOriginalMine(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[2])));
			return true;
		}
		if (method == MethodName.IsInsidePlantRegion && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsInsidePlantRegion(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.CellHasEnemyZombie && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CellHasEnemyZombie(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.CellHasEnemyCharacter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CellHasEnemyCharacter(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.IsEnemyCharacter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEnemyCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.IsEnemyCamp && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEnemyCamp(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.Explode && args.Count == 6)
		{
			Explode(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<PackedScene>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[5]));
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
			Run(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<PackedScene>(in args[3]), VariantUtils.ConvertTo<string>(in args[4]), VariantUtils.ConvertTo<Vector2>(in args[5]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanPlantOriginalMine && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(CanPlantOriginalMine(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[2])));
			return true;
		}
		if (method == MethodName.IsInsidePlantRegion && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsInsidePlantRegion(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0])));
			return true;
		}
		if (method == MethodName.CellHasEnemyZombie && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CellHasEnemyZombie(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.CellHasEnemyCharacter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CellHasEnemyCharacter(VariantUtils.ConvertTo<TowerDefenseCellInstance>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.IsEnemyCharacter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEnemyCharacter(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.IsEnemyCamp && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEnemyCamp(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1])));
			return true;
		}
		if (method == MethodName.Explode && args.Count == 6)
		{
			Explode(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<PackedScene>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in args[5]));
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
		if (method == MethodName.CanPlantOriginalMine)
		{
			return true;
		}
		if (method == MethodName.IsInsidePlantRegion)
		{
			return true;
		}
		if (method == MethodName.CellHasEnemyZombie)
		{
			return true;
		}
		if (method == MethodName.CellHasEnemyCharacter)
		{
			return true;
		}
		if (method == MethodName.IsEnemyCharacter)
		{
			return true;
		}
		if (method == MethodName.IsEnemyCamp)
		{
			return true;
		}
		if (method == MethodName.Explode)
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
		if (name == PropertyName.explosionEffect)
		{
			explosionEffect = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.explosionAudio)
		{
			explosionAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.explosionSize)
		{
			explosionSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.explosionEvents)
		{
			explosionEvents = VariantUtils.ConvertToArray<TowerDefenseCharacterEventBase>(in value);
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
		if (name == PropertyName.explosionEffect)
		{
			value = VariantUtils.CreateFrom(in explosionEffect);
			return true;
		}
		if (name == PropertyName.explosionAudio)
		{
			value = VariantUtils.CreateFrom(in explosionAudio);
			return true;
		}
		if (name == PropertyName.explosionSize)
		{
			value = VariantUtils.CreateFrom(in explosionSize);
			return true;
		}
		if (name == PropertyName.explosionEvents)
		{
			value = VariantUtils.CreateFromArray(explosionEvents);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.explosionEffect, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.explosionAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.explosionSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.explosionEvents, PropertyHint.TypeString, "24/17:TowerDefenseCharacterEventBase", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.packetName, Variant.From(in packetName));
		info.AddProperty(PropertyName.explosionEffect, Variant.From(in explosionEffect));
		info.AddProperty(PropertyName.explosionAudio, Variant.From(in explosionAudio));
		info.AddProperty(PropertyName.explosionSize, Variant.From(in explosionSize));
		info.AddProperty(PropertyName.explosionEvents, Variant.CreateFrom(explosionEvents));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.packetName, out var value))
		{
			packetName = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.explosionEffect, out var value2))
		{
			explosionEffect = value2.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.explosionAudio, out var value3))
		{
			explosionAudio = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.explosionSize, out var value4))
		{
			explosionSize = value4.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.explosionEvents, out var value5))
		{
			explosionEvents = value5.AsGodotArray<TowerDefenseCharacterEventBase>();
		}
	}
}
