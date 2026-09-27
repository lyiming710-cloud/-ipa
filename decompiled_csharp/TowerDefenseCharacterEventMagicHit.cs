using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventMagicHit.cs")]
public class TowerDefenseCharacterEventMagicHit : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public static readonly StringName Run = "Run";

		public static readonly StringName ResolveCasterCamp = "ResolveCasterCamp";

		public static readonly StringName ApplyAreaTimeSlow = "ApplyAreaTimeSlow";

		public static readonly StringName TryApplyTimeSlow = "TryApplyTimeSlow";

		public static readonly StringName BuildGridAreaWorldRect = "BuildGridAreaWorldRect";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName num = "num";

		public static readonly StringName buffList = "buffList";

		public static readonly StringName plantsOnly = "plantsOnly";

		public static readonly StringName allowOtherUnitTargets = "allowOtherUnitTargets";

		public static readonly StringName slowRangeColumns = "slowRangeColumns";

		public static readonly StringName slowRangeLines = "slowRangeLines";

		public static readonly StringName slowTime = "slowTime";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double num = 60.0;

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterBuffConfig> buffList = new Array<TowerDefenseCharacterBuffConfig>();

	[Export(PropertyHint.None, "")]
	public bool plantsOnly;

	[Export(PropertyHint.None, "")]
	public bool allowOtherUnitTargets;

	[Export(PropertyHint.None, "")]
	public int slowRangeColumns;

	[Export(PropertyHint.None, "")]
	public int slowRangeLines;

	[Export(PropertyHint.None, "")]
	public double slowTime;

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(pos, target, num, buffList, plantsOnly, slowRangeColumns, slowRangeLines, slowTime, TowerDefenseEnum.CHARACTER_CAMP.ALL, allowOtherUnitTargets);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(pos, target, num, buffList, plantsOnly, slowRangeColumns, slowRangeLines, slowTime, TowerDefenseEnum.CHARACTER_CAMP.ALL, allowOtherUnitTargets);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		bool blockEffects = projectile != null && GodotObject.IsInstanceValid(target) && target.ProjectileEffectsBlocked(projectile.damageFlags, projectile.fireMethodFlags);
		Run(projectile.GlobalPosition, target, num, buffList, plantsOnly, slowRangeColumns, slowRangeLines, slowTime, projectile.camp, allowOtherUnitTargets, blockEffects);
	}

	public static void Run(Vector2 pos, TowerDefenseCharacter target, double num, Array<TowerDefenseCharacterBuffConfig> buffList, bool plantsOnly = false, int slowRangeColumns = 0, int slowRangeLines = 0, double slowTime = 0.0, TowerDefenseEnum.CHARACTER_CAMP casterCamp = TowerDefenseEnum.CHARACTER_CAMP.ALL, bool allowOtherUnitTargets = false, bool blockEffects = false)
	{
		if (!TowerDefenseManager.HasGameplayAuthority || !GodotObject.IsInstanceValid(target) || !GodotObject.IsInstanceValid(target.instance) || target.die || target.isDestroy || (plantsOnly && !(target is TowerDefensePlant)))
		{
			return;
		}
		if (allowOtherUnitTargets)
		{
			if (target is TowerDefenseGravestone || target is TowerDefenseVase)
			{
				return;
			}
		}
		else if (!(target is TowerDefensePlant) && !(target is TowerDefenseZombie))
		{
			return;
		}
		if (target is TowerDefenseZombie && target.instance.zombiePhysique == TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS)
		{
			return;
		}
		Vector2I gridPos = target.gridPos;
		int damageFlags = 67;
		TowerDefenseCharacterEventHurt.Run(pos, target, num, damageFlags);
		if (!blockEffects)
		{
			if (slowTime > 0.0)
			{
				ApplyAreaTimeSlow(gridPos, slowRangeColumns, slowRangeLines, slowTime, ResolveCasterCamp(casterCamp, target), target);
			}
			if (!target.die && !target.nearDie && !target.isDestroy)
			{
				TowerDefenseCharacterEventAddBuff.Run(target, buffList);
			}
		}
	}

	private static TowerDefenseEnum.CHARACTER_CAMP ResolveCasterCamp(TowerDefenseEnum.CHARACTER_CAMP casterCamp, TowerDefenseCharacter target)
	{
		if (casterCamp == TowerDefenseEnum.CHARACTER_CAMP.PLANT || casterCamp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
		{
			return casterCamp;
		}
		if (target.camp != TowerDefenseEnum.CHARACTER_CAMP.PLANT)
		{
			return TowerDefenseEnum.CHARACTER_CAMP.PLANT;
		}
		return TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE;
	}

	public static void ApplyAreaTimeSlow(Vector2I center, int rangeColumns, int rangeLines, double time, TowerDefenseEnum.CHARACTER_CAMP casterCamp, TowerDefenseCharacter centerTarget = null)
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance) || !GodotObject.IsInstanceValid(instance.characterRegistry))
		{
			return;
		}
		if (GodotObject.IsInstanceValid(centerTarget))
		{
			TryApplyTimeSlow(centerTarget, casterCamp, time);
		}
		Rect2 checkRect = BuildGridAreaWorldRect(instance, center, Mathf.Max(0, rangeColumns), Mathf.Max(0, rangeLines));
		List<TowerDefenseCharacter> list = new List<TowerDefenseCharacter>(instance.characterRegistry.GetCharactersIntersectingRectListExcludingCamp(checkRect, casterCamp));
		for (int i = 0; i < list.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = list[i];
			if (towerDefenseCharacter != centerTarget)
			{
				TryApplyTimeSlow(towerDefenseCharacter, casterCamp, time);
			}
		}
	}

	private static void TryApplyTimeSlow(TowerDefenseCharacter candidate, TowerDefenseEnum.CHARACTER_CAMP casterCamp, double time)
	{
		if (GodotObject.IsInstanceValid(candidate) && candidate.camp != casterCamp && (candidate is TowerDefensePlant || candidate is TowerDefenseZombie) && !(candidate is TowerDefensePlantBowlingBase) && GodotObject.IsInstanceValid(candidate.instance) && !candidate.die && !candidate.nearDie && !candidate.isDestroy)
		{
			BuffComponent buff = candidate.buff;
			if (buff != null && !buff.IsReleased && (!(candidate is TowerDefenseZombie) || candidate.instance.zombiePhysique != TowerDefenseEnum.ZOMBIE_PHYSIQUE.BOSS))
			{
				TowerDefenseCharacterBuffTimeMagic towerDefenseCharacterBuffTimeMagic = new TowerDefenseCharacterBuffTimeMagic();
				towerDefenseCharacterBuffTimeMagic.time = time;
				candidate.buff.AddBuff(towerDefenseCharacterBuffTimeMagic);
			}
		}
	}

	private static Rect2 BuildGridAreaWorldRect(TowerDefenseManager manager, Vector2I center, int rangeColumns, int rangeLines)
	{
		Vector2 mapGridSize = manager.GetMapGridSize();
		Vector2 mapCellPos = manager.GetMapCellPos(new Vector2I(center.X - rangeColumns, center.Y - rangeLines));
		Vector2 size = new Vector2(mapGridSize.X * (float)(rangeColumns * 2 + 1), mapGridSize.Y * (float)(rangeLines * 2 + 1));
		return new Rect2(mapCellPos, size);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
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
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "buffList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "plantsOnly", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "slowRangeColumns", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "slowRangeLines", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "slowTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "casterCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "allowOtherUnitTargets", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "blockEffects", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveCasterCamp, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "casterCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyAreaTimeSlow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "center", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rangeColumns", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rangeLines", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "casterCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "centerTarget", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryApplyTimeSlow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "candidate", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "casterCamp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "time", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildGridAreaWorldRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "center", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rangeColumns", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rangeLines", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Run && args.Count == 11)
		{
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertToArray<TowerDefenseCharacterBuffConfig>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<double>(in args[7]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[8]), VariantUtils.ConvertTo<bool>(in args[9]), VariantUtils.ConvertTo<bool>(in args[10]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveCasterCamp && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEnum.CHARACTER_CAMP>(ResolveCasterCamp(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyAreaTimeSlow && args.Count == 6)
		{
			ApplyAreaTimeSlow(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[4]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryApplyTimeSlow && args.Count == 3)
		{
			TryApplyTimeSlow(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildGridAreaWorldRect && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Rect2>(BuildGridAreaWorldRect(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Run && args.Count == 11)
		{
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertToArray<TowerDefenseCharacterBuffConfig>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<double>(in args[7]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[8]), VariantUtils.ConvertTo<bool>(in args[9]), VariantUtils.ConvertTo<bool>(in args[10]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveCasterCamp && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEnum.CHARACTER_CAMP>(ResolveCasterCamp(VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyAreaTimeSlow && args.Count == 6)
		{
			ApplyAreaTimeSlow(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[4]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryApplyTimeSlow && args.Count == 3)
		{
			TryApplyTimeSlow(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.CHARACTER_CAMP>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildGridAreaWorldRect && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Rect2>(BuildGridAreaWorldRect(VariantUtils.ConvertTo<TowerDefenseManager>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
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
		if (method == MethodName.ResolveCasterCamp)
		{
			return true;
		}
		if (method == MethodName.ApplyAreaTimeSlow)
		{
			return true;
		}
		if (method == MethodName.TryApplyTimeSlow)
		{
			return true;
		}
		if (method == MethodName.BuildGridAreaWorldRect)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.buffList)
		{
			buffList = VariantUtils.ConvertToArray<TowerDefenseCharacterBuffConfig>(in value);
			return true;
		}
		if (name == PropertyName.plantsOnly)
		{
			plantsOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.allowOtherUnitTargets)
		{
			allowOtherUnitTargets = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.slowRangeColumns)
		{
			slowRangeColumns = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.slowRangeLines)
		{
			slowRangeLines = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.slowTime)
		{
			slowTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.num)
		{
			value = VariantUtils.CreateFrom(in num);
			return true;
		}
		if (name == PropertyName.buffList)
		{
			value = VariantUtils.CreateFromArray(buffList);
			return true;
		}
		if (name == PropertyName.plantsOnly)
		{
			value = VariantUtils.CreateFrom(in plantsOnly);
			return true;
		}
		if (name == PropertyName.allowOtherUnitTargets)
		{
			value = VariantUtils.CreateFrom(in allowOtherUnitTargets);
			return true;
		}
		if (name == PropertyName.slowRangeColumns)
		{
			value = VariantUtils.CreateFrom(in slowRangeColumns);
			return true;
		}
		if (name == PropertyName.slowRangeLines)
		{
			value = VariantUtils.CreateFrom(in slowRangeLines);
			return true;
		}
		if (name == PropertyName.slowTime)
		{
			value = VariantUtils.CreateFrom(in slowTime);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.buffList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterBuffConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.plantsOnly, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.allowOtherUnitTargets, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.slowRangeColumns, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.slowRangeLines, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.slowTime, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.num, Variant.From(in num));
		info.AddProperty(PropertyName.buffList, Variant.CreateFrom(buffList));
		info.AddProperty(PropertyName.plantsOnly, Variant.From(in plantsOnly));
		info.AddProperty(PropertyName.allowOtherUnitTargets, Variant.From(in allowOtherUnitTargets));
		info.AddProperty(PropertyName.slowRangeColumns, Variant.From(in slowRangeColumns));
		info.AddProperty(PropertyName.slowRangeLines, Variant.From(in slowRangeLines));
		info.AddProperty(PropertyName.slowTime, Variant.From(in slowTime));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.num, out var value))
		{
			num = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.buffList, out var value2))
		{
			buffList = value2.AsGodotArray<TowerDefenseCharacterBuffConfig>();
		}
		if (info.TryGetProperty(PropertyName.plantsOnly, out var value3))
		{
			plantsOnly = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.allowOtherUnitTargets, out var value4))
		{
			allowOtherUnitTargets = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.slowRangeColumns, out var value5))
		{
			slowRangeColumns = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.slowRangeLines, out var value6))
		{
			slowRangeLines = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.slowTime, out var value7))
		{
			slowTime = value7.As<double>();
		}
	}
}
