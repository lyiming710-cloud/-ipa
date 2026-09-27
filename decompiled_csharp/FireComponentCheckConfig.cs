using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Script/Component/TowerDefense/Character/FireComponent/Resource/FireComponentCheckConfig.cs")]
public class FireComponentCheckConfig : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName GetProjectile = "GetProjectile";

		public static readonly StringName GetProjetile = "GetProjetile";

		public static readonly StringName GetCollisionFlags = "GetCollisionFlags";

		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName useParentCollision = "useParentCollision";

		public static readonly StringName projectile = "projectile";

		public static readonly StringName _useParentCollision = "_useParentCollision";

		public static readonly StringName collisionFlags = "collisionFlags";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public FireComponentProjectileResource projectile;

	private bool _useParentCollision = true;

	[Export(PropertyHint.None, "")]
	public int collisionFlags;

	[Export(PropertyHint.None, "")]
	public bool useParentCollision
	{
		get
		{
			return _useParentCollision;
		}
		set
		{
			if (_useParentCollision != value)
			{
				_useParentCollision = value;
				NotifyPropertyListChanged();
			}
		}
	}

	public bool CanFire(FireComponent fireComponent)
	{
		if (GodotObject.IsInstanceValid(projectile) && fireComponent != null && !fireComponent.IsReleased)
		{
			return projectile.CanFire(fireComponent, GetCollisionFlags());
		}
		return false;
	}

	public TowerDefenseProjectileCreateData GetProjectile()
	{
		if (!GodotObject.IsInstanceValid(projectile))
		{
			return null;
		}
		return projectile.GetProjectile();
	}

	public TowerDefenseProjectileCreateData GetProjetile()
	{
		return GetProjectile();
	}

	public int GetCollisionFlags()
	{
		if (!useParentCollision)
		{
			return collisionFlags;
		}
		return -1;
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		long num = 4L;
		Array<Dictionary> array = new Array<Dictionary>();
		if (!useParentCollision)
		{
			array.Add(new Dictionary
			{
				{ "name", "Flag/Collision" },
				{ "type", 2 },
				{ "hint", 6 },
				{
					"hint_string",
					string.Join(",", Enum.GetNames<TowerDefenseEnum.CHARACTER_COLLISION_FLAGS>())
				},
				{ "usage", num }
			});
		}
		return array;
	}

	public override bool _Set(StringName property, Variant value)
	{
		if ((string?)property == "Flag/Collision")
		{
			collisionFlags = value.AsInt32();
			return true;
		}
		return false;
	}

	public override Variant _Get(StringName property)
	{
		if ((string?)property == "Flag/Collision")
		{
			return Variant.From(in collisionFlags);
		}
		return default;
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		if ((string?)property == "Flag/Collision")
		{
			return true;
		}
		return false;
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		if ((string?)property == "Flag/Collision")
		{
			return Variant.From<int>(0);
		}
		return default;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.GetProjectile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetProjetile, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCollisionFlags, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
		if (method == MethodName.GetProjectile && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileCreateData>(GetProjectile());
			return true;
		}
		if (method == MethodName.GetProjetile && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseProjectileCreateData>(GetProjetile());
			return true;
		}
		if (method == MethodName.GetCollisionFlags && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCollisionFlags());
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
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetProjectile)
		{
			return true;
		}
		if (method == MethodName.GetProjetile)
		{
			return true;
		}
		if (method == MethodName.GetCollisionFlags)
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
		if (name == PropertyName.useParentCollision)
		{
			useParentCollision = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.projectile)
		{
			projectile = VariantUtils.ConvertTo<FireComponentProjectileResource>(in value);
			return true;
		}
		if (name == PropertyName._useParentCollision)
		{
			_useParentCollision = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.useParentCollision)
		{
			value = VariantUtils.CreateFrom<bool>(useParentCollision);
			return true;
		}
		if (name == PropertyName.projectile)
		{
			value = VariantUtils.CreateFrom(in projectile);
			return true;
		}
		if (name == PropertyName._useParentCollision)
		{
			value = VariantUtils.CreateFrom(in _useParentCollision);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.projectile, PropertyHint.ResourceType, "FireComponentProjectileResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._useParentCollision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useParentCollision, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.useParentCollision, Variant.From<bool>(useParentCollision));
		info.AddProperty(PropertyName.projectile, Variant.From(in projectile));
		info.AddProperty(PropertyName._useParentCollision, Variant.From(in _useParentCollision));
		info.AddProperty(PropertyName.collisionFlags, Variant.From(in collisionFlags));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.useParentCollision, out var value))
		{
			useParentCollision = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.projectile, out var value2))
		{
			projectile = value2.As<FireComponentProjectileResource>();
		}
		if (info.TryGetProperty(PropertyName._useParentCollision, out var value3))
		{
			_useParentCollision = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.collisionFlags, out var value4))
		{
			collisionFlags = value4.As<int>();
		}
	}
}
