using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Attack/AttackConfig.cs")]
public class AttackConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName damageFlags = "damageFlags";

		public static readonly StringName collisionFlags = "collisionFlags";

		public static readonly StringName num = "num";

		public static readonly StringName attackScale = "attackScale";

		public static readonly StringName armorAttackScale = "armorAttackScale";

		public static readonly StringName _damageFlags = "_damageFlags";

		public static readonly StringName _collisionFlags = "_collisionFlags";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public double num = 20.0;

	[Export(PropertyHint.None, "")]
	public double attackScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double armorAttackScale = 1.0;

	private int _damageFlags = 3;

	private int _collisionFlags = 9;

	[Export(PropertyHint.None, "")]
	public int damageFlags
	{
		get
		{
			return _damageFlags;
		}
		set
		{
			_damageFlags = value;
		}
	}

	[Export(PropertyHint.None, "")]
	public int collisionFlags
	{
		get
		{
			return _collisionFlags;
		}
		set
		{
			_collisionFlags = value;
		}
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
				_collisionFlags = value.AsInt32();
				return true;
			}
			return false;
		}
		_damageFlags = value.AsInt32();
		return true;
	}

	public override Variant _Get(StringName property)
	{
		string text = property.ToString();
		if (!(text == "Flag/Damage"))
		{
			if (text == "Flag/Collision")
			{
				return _collisionFlags;
			}
			return default;
		}
		return _damageFlags;
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
				return 9;
			}
			return default;
		}
		return 3;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(5)
		{
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
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
		if (name == PropertyName.num)
		{
			num = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.attackScale)
		{
			attackScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.armorAttackScale)
		{
			armorAttackScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._damageFlags)
		{
			_damageFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._collisionFlags)
		{
			_collisionFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.damageFlags)
		{
			from = damageFlags;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.collisionFlags)
		{
			from = collisionFlags;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.num)
		{
			value = VariantUtils.CreateFrom(in num);
			return true;
		}
		if (name == PropertyName.attackScale)
		{
			value = VariantUtils.CreateFrom(in attackScale);
			return true;
		}
		if (name == PropertyName.armorAttackScale)
		{
			value = VariantUtils.CreateFrom(in armorAttackScale);
			return true;
		}
		if (name == PropertyName._damageFlags)
		{
			value = VariantUtils.CreateFrom(in _damageFlags);
			return true;
		}
		if (name == PropertyName._collisionFlags)
		{
			value = VariantUtils.CreateFrom(in _collisionFlags);
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
			new PropertyInfo(Variant.Type.Float, PropertyName.attackScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.armorAttackScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._damageFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.damageFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._collisionFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.damageFlags, Variant.From<int>(damageFlags));
		info.AddProperty(PropertyName.collisionFlags, Variant.From<int>(collisionFlags));
		info.AddProperty(PropertyName.num, Variant.From(in num));
		info.AddProperty(PropertyName.attackScale, Variant.From(in attackScale));
		info.AddProperty(PropertyName.armorAttackScale, Variant.From(in armorAttackScale));
		info.AddProperty(PropertyName._damageFlags, Variant.From(in _damageFlags));
		info.AddProperty(PropertyName._collisionFlags, Variant.From(in _collisionFlags));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.damageFlags, out var value))
		{
			damageFlags = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.collisionFlags, out var value2))
		{
			collisionFlags = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.num, out var value3))
		{
			num = value3.As<double>();
		}
		if (info.TryGetProperty(PropertyName.attackScale, out var value4))
		{
			attackScale = value4.As<double>();
		}
		if (info.TryGetProperty(PropertyName.armorAttackScale, out var value5))
		{
			armorAttackScale = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._damageFlags, out var value6))
		{
			_damageFlags = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._collisionFlags, out var value7))
		{
			_collisionFlags = value7.As<int>();
		}
	}
}
