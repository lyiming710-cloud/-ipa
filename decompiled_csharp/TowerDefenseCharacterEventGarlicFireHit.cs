using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/Special/TowerDefenseCharacterEventGarlicFireHit.cs")]
public class TowerDefenseCharacterEventGarlicFireHit : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public static readonly StringName ApplyPoisonDamage = "ApplyPoisonDamage";

		public static readonly StringName GetPoisonDamage = "GetPoisonDamage";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName _runtimeMagicDamage = "_runtimeMagicDamage";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	private const double PoisonDamage = 5.0;

	private bool _runtimeMagicDamage;

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		ApplyPoisonDamage(target, _runtimeMagicDamage);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		if ((!Global.IsMultiplayerMode || MultiPlayerManager.IsHost) && GodotObject.IsInstanceValid(target))
		{
			Vector2 logicalGlobalPosition = target.GetLogicalGlobalPosition();
			TowerDefenseManager instance = TowerDefenseManager.Instance;
			Vector2 mapCellPosCenter = instance.GetMapCellPosCenter(instance.GetMapGridPos(logicalGlobalPosition));
			Vector2 pos = new Vector2(logicalGlobalPosition.X, mapCellPosCenter.Y);
			bool flag = projectile != null && TowerDefenseCharacterInstance.IsMagicDamage(projectile.damageFlags);
			ApplyPoisonDamage(target, flag);
			TowerDefenseCharacterEventBase item = (flag ? new TowerDefenseCharacterEventGarlicFireHit
			{
				_runtimeMagicDamage = true
			} : this);
			TowerDefenseExplode.CreateExplode(eventList: new Array<TowerDefenseCharacterEventBase> { item }, exclude: new Array<TowerDefenseCharacter> { target }, pos: pos, size: projectile.config.rangeSize, camp: projectile.camp, collisionFlags: projectile.collisionFlags);
		}
	}

	private void ApplyPoisonDamage(TowerDefenseCharacter target, bool magicDamage)
	{
		if ((!Global.IsMultiplayerMode || MultiPlayerManager.IsHost) && GodotObject.IsInstanceValid(target) && GodotObject.IsInstanceValid(target.instance) && !target.instance.die && (target.instance.unUseBuffFlags & 0x40) == 0)
		{
			target.instance.DealHurt(GetPoisonDamage(target, magicDamage), playSplatAudio: false);
			ShowHealthComponent showHealthComponent = target.showHealthComponent;
			if (showHealthComponent != null && !showHealthComponent.IsReleased)
			{
				target.showHealthComponent.MarkDirty();
			}
		}
	}

	private static double GetPoisonDamage(TowerDefenseCharacter target, bool magicDamage)
	{
		if (!magicDamage || target.instance.IsMagicPhysique)
		{
			return 5.0;
		}
		return 7.5;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName.Execute, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ExecuteProject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "projectile", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPoisonDamage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "magicDamage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPoisonDamage, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "magicDamage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.ExecuteProject && args.Count == 2)
		{
			ExecuteProject(VariantUtils.ConvertTo<TowerDefenseProjectile>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPoisonDamage && args.Count == 2)
		{
			ApplyPoisonDamage(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPoisonDamage && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(GetPoisonDamage(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetPoisonDamage && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(GetPoisonDamage(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
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
		if (method == MethodName.ExecuteProject)
		{
			return true;
		}
		if (method == MethodName.ApplyPoisonDamage)
		{
			return true;
		}
		if (method == MethodName.GetPoisonDamage)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._runtimeMagicDamage)
		{
			_runtimeMagicDamage = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._runtimeMagicDamage)
		{
			value = VariantUtils.CreateFrom(in _runtimeMagicDamage);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeMagicDamage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._runtimeMagicDamage, Variant.From(in _runtimeMagicDamage));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._runtimeMagicDamage, out var value))
		{
			_runtimeMagicDamage = value.As<bool>();
		}
	}
}
