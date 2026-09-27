using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventSmashHurt.cs")]
public class TowerDefenseCharacterEventSmashHurt : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public static readonly StringName Run = "Run";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName num = "num";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double num = 1800.0;

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(pos, target, num);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(pos, target, num * delta);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(projectile.GlobalPosition, target, num);
	}

	public static void Run(Vector2 pos, TowerDefenseCharacter target, double _num = 20.0)
	{
		Vector2 velocity = new Vector2((target.GetLogicalGlobalPosition().X - pos.X) * (float)GD.RandRange(3.0, 6.0), -1000f);
		target.SmashHurt(_num, playSplatAudio: true, velocity);
		target.AttackDeal(null, "Smash", _num);
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
				new PropertyInfo(Variant.Type.Float, "_num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Run && args.Count == 3)
		{
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
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
		if (method == MethodName.Run)
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
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Float, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.num, Variant.From(in num));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.num, out var value))
		{
			num = value.As<double>();
		}
	}
}
