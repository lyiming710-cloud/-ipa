using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventExplodeHurt.cs")]
public class TowerDefenseCharacterEventExplodeHurt : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public static readonly StringName Run = "Run";

		public static readonly StringName ResolveDamageKind = "ResolveDamageKind";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName type = "type";

		public static readonly StringName DamageKind = "DamageKind";

		public static readonly StringName _type = "_type";

		public static readonly StringName _damageKind = "_damageKind";

		public static readonly StringName num = "num";

		public static readonly StringName burns = "burns";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	private string _type = "Bomb";

	private TowerDefenseEnum.EXPLOSION_DAMAGE_KIND _damageKind = TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB;

	[Export(PropertyHint.None, "")]
	public double num = 1800.0;

	[Export(PropertyHint.None, "")]
	public bool burns = true;

	[Export(PropertyHint.Enum, "Bomb,Jala,Mine")]
	public string type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value ?? "Bomb";
			_damageKind = ResolveDamageKind(_type);
		}
	}

	internal TowerDefenseEnum.EXPLOSION_DAMAGE_KIND DamageKind => _damageKind;

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(pos, _damageKind, target, num);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(pos, _damageKind, target, num * delta);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(projectile.GlobalPosition, _damageKind, target, num);
	}

	public static void Run(Vector2 pos, TowerDefenseEnum.EXPLOSION_DAMAGE_KIND damageKind, TowerDefenseCharacter target, double num = 20.0)
	{
		Vector2 velocity = new Vector2((float)((double)(target.GetLogicalGlobalPosition().X - pos.X) * GD.RandRange(3.0, 6.0)), -1000f);
		target.ExplodeHurt(num, damageKind, playSplatAudio: true, velocity);
		target.AttackDeal(null, "Explode", num);
	}

	private static TowerDefenseEnum.EXPLOSION_DAMAGE_KIND ResolveDamageKind(string type)
	{
		return type switch
		{
			"Bomb" => TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.BOMB, 
			"Jala" => TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.JALA, 
			"Mine" => TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.MINE, 
			_ => TowerDefenseEnum.EXPLOSION_DAMAGE_KIND.OTHER, 
		};
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
				new PropertyInfo(Variant.Type.Vector2, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "damageKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Float, "num", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveDamageKind, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.Run && args.Count == 4)
		{
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveDamageKind && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(ResolveDamageKind(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Run && args.Count == 4)
		{
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in args[1]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveDamageKind && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(ResolveDamageKind(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.ResolveDamageKind)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.type)
		{
			type = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._type)
		{
			_type = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._damageKind)
		{
			_damageKind = VariantUtils.ConvertTo<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(in value);
			return true;
		}
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.burns)
		{
			burns = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.type)
		{
			value = VariantUtils.CreateFrom<string>(type);
			return true;
		}
		if (name == PropertyName.DamageKind)
		{
			value = VariantUtils.CreateFrom<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>(DamageKind);
			return true;
		}
		if (name == PropertyName._type)
		{
			value = VariantUtils.CreateFrom(in _type);
			return true;
		}
		if (name == PropertyName._damageKind)
		{
			value = VariantUtils.CreateFrom(in _damageKind);
			return true;
		}
		if (name == PropertyName.num)
		{
			value = VariantUtils.CreateFrom(in num);
			return true;
		}
		if (name == PropertyName.burns)
		{
			value = VariantUtils.CreateFrom(in burns);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._type, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._damageKind, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.type, PropertyHint.Enum, "Bomb,Jala,Mine", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.num, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.burns, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.DamageKind, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.type, Variant.From<string>(type));
		info.AddProperty(PropertyName._type, Variant.From(in _type));
		info.AddProperty(PropertyName._damageKind, Variant.From(in _damageKind));
		info.AddProperty(PropertyName.num, Variant.From(in num));
		info.AddProperty(PropertyName.burns, Variant.From(in burns));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.type, out var value))
		{
			type = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName._type, out var value2))
		{
			_type = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName._damageKind, out var value3))
		{
			_damageKind = value3.As<TowerDefenseEnum.EXPLOSION_DAMAGE_KIND>();
		}
		if (info.TryGetProperty(PropertyName.num, out var value4))
		{
			num = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.burns, out var value5))
		{
			burns = value5.As<bool>();
		}
	}
}
