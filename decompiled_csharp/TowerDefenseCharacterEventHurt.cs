using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Character/Event/General/TowerDefenseCharacterEventHurt.cs")]
public class TowerDefenseCharacterEventHurt : TowerDefenseCharacterEventBase
{
	public new class MethodName : TowerDefenseCharacterEventBase.MethodName
	{
		public new static readonly StringName Execute = "Execute";

		public new static readonly StringName ExecuteDps = "ExecuteDps";

		public new static readonly StringName ExecuteProject = "ExecuteProject";

		public static readonly StringName Run = "Run";

		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";
	}

	public new class PropertyName : TowerDefenseCharacterEventBase.PropertyName
	{
		public static readonly StringName num = "num";

		public static readonly StringName playSplatAudio = "playSplatAudio";

		public static readonly StringName damageFlags = "damageFlags";

		public static readonly StringName collisionFlags = "collisionFlags";
	}

	public new class SignalName : TowerDefenseCharacterEventBase.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double num = 20.0;

	[Export(PropertyHint.None, "")]
	public bool playSplatAudio = true;

	[Export(PropertyHint.None, "")]
	public int damageFlags = 3;

	[Export(PropertyHint.None, "")]
	public int collisionFlags = 9;

	public override void Execute(Vector2 pos, TowerDefenseCharacter target)
	{
		Run(pos, target, num, damageFlags, playSplatAudio);
	}

	public override void ExecuteDps(Vector2 pos, TowerDefenseCharacter target, double delta)
	{
		Run(pos, target, num * delta, damageFlags, playSplatAudio);
	}

	public override void ExecuteProject(TowerDefenseProjectile projectile, TowerDefenseCharacter target)
	{
		Run(projectile.GlobalPosition, target, num, damageFlags, playSplatAudio);
	}

	public static void Run(Vector2 pos, TowerDefenseCharacter target, double num = 20.0, int damageFlags = 3, bool playSplatAudio = true)
	{
		target.FlagHurt(num, damageFlags, playSplatAudio);
		target.AttackDeal(null, "Default", num);
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		long num = 4L;
		return new Array<Dictionary>
		{
			new Dictionary
			{
				["name"] = "Flag/Damage",
				["type"] = 2,
				["hint"] = 6,
				["hint_string"] = string.Join(",", Enum.GetNames<TowerDefenseEnum.PROJECTILE_DAMAGE_FLAG>()),
				["usage"] = num
			},
			new Dictionary
			{
				["name"] = "Flag/Collision",
				["type"] = 2,
				["hint"] = 6,
				["hint_string"] = string.Join(",", Enum.GetNames<TowerDefenseEnum.CHARACTER_COLLISION_FLAGS>()),
				["usage"] = num
			}
		};
	}

	public override bool _Set(StringName property, Variant value)
	{
		string text = property.ToString();
		if (!(text == "Flag/Damage"))
		{
			if (text == "Flag/Collision")
			{
				collisionFlags = value.AsInt32();
				return true;
			}
			return false;
		}
		damageFlags = value.AsInt32();
		return true;
	}

	public override Variant _Get(StringName property)
	{
		string text = property.ToString();
		if (!(text == "Flag/Damage"))
		{
			if (text == "Flag/Collision")
			{
				return Variant.From(in collisionFlags);
			}
			return default;
		}
		return Variant.From(in damageFlags);
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		string text = property.ToString();
		if (text == "Flag/Damage" || text == "Flag/Collision")
		{
			return true;
		}
		return false;
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		string text = property.ToString();
		if (!(text == "Flag/Damage"))
		{
			if (text == "Flag/Collision")
			{
				return Variant.From<int>(9);
			}
			return default;
		}
		return Variant.From<int>(3);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
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
				new PropertyInfo(Variant.Type.Int, "damageFlags", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "playSplatAudio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GetPropertyList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._Get, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyCanRevert, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyGetRevert, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName._GetPropertyList && args.Count == 0)
		{
			Array<Dictionary> array = _GetPropertyList();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName._Set && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName._Get && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_Get(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyCanRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(_PropertyCanRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyGetRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_PropertyGetRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Run && args.Count == 5)
		{
			Run(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]));
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
		if (method == MethodName._GetPropertyList)
		{
			return true;
		}
		if (method == MethodName._Set)
		{
			return true;
		}
		if (method == MethodName._Get)
		{
			return true;
		}
		if (method == MethodName._PropertyCanRevert)
		{
			return true;
		}
		if (method == MethodName._PropertyGetRevert)
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
		if (name == PropertyName.playSplatAudio)
		{
			playSplatAudio = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.damageFlags)
		{
			damageFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			collisionFlags = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName.playSplatAudio)
		{
			value = VariantUtils.CreateFrom(in playSplatAudio);
			return true;
		}
		if (name == PropertyName.damageFlags)
		{
			value = VariantUtils.CreateFrom(in damageFlags);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			value = VariantUtils.CreateFrom(in collisionFlags);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.playSplatAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.damageFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.num, Variant.From(in num));
		info.AddProperty(PropertyName.playSplatAudio, Variant.From(in playSplatAudio));
		info.AddProperty(PropertyName.damageFlags, Variant.From(in damageFlags));
		info.AddProperty(PropertyName.collisionFlags, Variant.From(in collisionFlags));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.num, out var value))
		{
			num = value.As<double>();
		}
		if (info.TryGetProperty(PropertyName.playSplatAudio, out var value2))
		{
			playSplatAudio = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.damageFlags, out var value3))
		{
			damageFlags = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.collisionFlags, out var value4))
		{
			collisionFlags = value4.As<int>();
		}
	}
}
