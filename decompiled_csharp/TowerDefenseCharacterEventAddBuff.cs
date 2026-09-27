using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventAddBuff.cs")]
public class TowerDefenseCharacterEventAddBuff : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public static readonly StringName Run = "Run";

		public static readonly StringName TryShieldBlockCharmBuff = "TryShieldBlockCharmBuff";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName buffList = "buffList";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<TowerDefenseCharacterBuffConfig> buffList = new Array<TowerDefenseCharacterBuffConfig>();

	private static Texture2D _BUTTER_BLUEBERRY_SPLAT;

	private static Texture2D BUTTER_BLUEBERRY_SPLAT => _BUTTER_BLUEBERRY_SPLAT ?? (_BUTTER_BLUEBERRY_SPLAT = GD.Load<Texture2D>("uid://1xg4dycli5xw"));

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(target, buffList);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(target, buffList);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(target, buffList, projectile);
	}

	public static void Run(TowerDefenseCharacter target, Array<TowerDefenseCharacterBuffConfig> buffList, TowerDefenseProjectile projectile = null)
	{
		if (!GodotObject.IsInstanceValid(target) || (projectile != null && target.ProjectileEffectsBlocked(projectile.damageFlags, projectile.fireMethodFlags)))
		{
			return;
		}
		bool flag = projectile != null && projectile.config?.skinName == new StringName("Blueberry");
		foreach (TowerDefenseCharacterBuffConfig buff in buffList)
		{
			if (buff == null || TryShieldBlockCharmBuff(target, buff))
			{
				continue;
			}
			if (buff is TowerDefenseCharacterBuffHypnoses towerDefenseCharacterBuffHypnoses)
			{
				target.Hypnoses(towerDefenseCharacterBuffHypnoses.time);
				continue;
			}
			if (buff is TowerDefenseCharacterBuffFireHit source)
			{
				target.buff.ApplyFireHit(source);
				continue;
			}
			TowerDefenseCharacterBuffConfig towerDefenseCharacterBuffConfig = buff.CreateRuntimeInstance();
			if (towerDefenseCharacterBuffConfig != null)
			{
				if (flag && towerDefenseCharacterBuffConfig is TowerDefenseCharacterBuffButter towerDefenseCharacterBuffButter)
				{
					towerDefenseCharacterBuffButter.splatTexture = BUTTER_BLUEBERRY_SPLAT;
				}
				target.buff.AddBuff(towerDefenseCharacterBuffConfig);
			}
		}
	}

	private static bool TryShieldBlockCharmBuff(TowerDefenseCharacter target, TowerDefenseCharacterBuffConfig buff)
	{
		if (!(buff is TowerDefenseCharacterBuffSleep) && !(buff is TowerDefenseCharacterBuffHypnoses))
		{
			return false;
		}
		if (!(target is TowerDefensePlant))
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(target.cell) || !GodotObject.IsInstanceValid(target.cell.itemShield))
		{
			return false;
		}
		TowerDefenseItemSheild itemShield = target.cell.itemShield;
		if (itemShield == target)
		{
			return false;
		}
		if (itemShield.instance.hypnoses != target.instance.hypnoses)
		{
			return false;
		}
		return itemShield.ShieldBlockCharm();
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
				new PropertyInfo(Variant.Type.Array, "buffList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryShieldBlockCharmBuff, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "buff", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.Run && args.Count == 3)
		{
			Run(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseCharacterBuffConfig>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryShieldBlockCharmBuff && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryShieldBlockCharmBuff(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Run && args.Count == 3)
		{
			Run(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertToArray<TowerDefenseCharacterBuffConfig>(in args[1]), VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryShieldBlockCharmBuff && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryShieldBlockCharmBuff(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacterBuffConfig>(in args[1])));
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
		if (method == MethodName.TryShieldBlockCharmBuff)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.buffList)
		{
			buffList = VariantUtils.ConvertToArray<TowerDefenseCharacterBuffConfig>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.buffList)
		{
			value = VariantUtils.CreateFromArray(buffList);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.buffList, PropertyHint.TypeString, "24/17:TowerDefenseCharacterBuffConfig", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.buffList, Variant.CreateFrom(buffList));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.buffList, out var value))
		{
			buffList = value.AsGodotArray<TowerDefenseCharacterBuffConfig>();
		}
	}
}
