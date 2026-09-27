using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ChangeProjectileState/ChangeProjectileStateComponentDefinition.cs")]
public class ChangeProjectileStateComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName checkShape = "checkShape";

		public static readonly StringName campFilter = "campFilter";

		public static readonly StringName followCamp = "followCamp";

		public static readonly StringName replaceDamageFlags = "replaceDamageFlags";

		public static readonly StringName replaceFireMethodFlags = "replaceFireMethodFlags";

		public static readonly StringName enableTrackingFromFireMethod = "enableTrackingFromFireMethod";

		public static readonly StringName damageFlags = "damageFlags";

		public static readonly StringName fireMethodFlags = "fireMethodFlags";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public AabbShape2DResource checkShape { get; set; }

	[Export(PropertyHint.None, "")]
	public ChangeProjectileStateComponent.ProjectileCampFilter campFilter { get; set; }

	[Export(PropertyHint.None, "")]
	public bool followCamp { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool replaceDamageFlags { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool replaceFireMethodFlags { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool enableTrackingFromFireMethod { get; set; } = true;

	[Export(PropertyHint.Flags, "HITBODY,HITSHIELD,FIRE,ICE,EXPLOSION,CHILD,PIERCE,SMASH")]
	public int damageFlags { get; set; } = 3;

	[Export(PropertyHint.Flags, "SHOOTER,CATAPULT,THROW,TRACK")]
	public int fireMethodFlags { get; set; } = 1;

	public int CollisionPreviewShapeCount => GodotObject.IsInstanceValid(checkShape) ? 1 : 0;

	public int CollisionPreviewRayCount => 0;

	public AabbShape2DResource GetCollisionPreviewShape(int index)
	{
		if (index != 0)
		{
			return null;
		}
		return checkShape;
	}

	public AabbRay2DResource GetCollisionPreviewRay(int index)
	{
		return null;
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new ChangeProjectileStateComponent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.GetCollisionPreviewShape, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCollisionPreviewRay, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetCollisionPreviewShape && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AabbShape2DResource>(GetCollisionPreviewShape(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCollisionPreviewRay && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AabbRay2DResource>(GetCollisionPreviewRay(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetCollisionPreviewShape)
		{
			return true;
		}
		if (method == MethodName.GetCollisionPreviewRay)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.checkShape)
		{
			checkShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.campFilter)
		{
			campFilter = VariantUtils.ConvertTo<ChangeProjectileStateComponent.ProjectileCampFilter>(in value);
			return true;
		}
		if (name == PropertyName.followCamp)
		{
			followCamp = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.replaceDamageFlags)
		{
			replaceDamageFlags = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.replaceFireMethodFlags)
		{
			replaceFireMethodFlags = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.enableTrackingFromFireMethod)
		{
			enableTrackingFromFireMethod = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.damageFlags)
		{
			damageFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.fireMethodFlags)
		{
			fireMethodFlags = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.checkShape)
		{
			value = VariantUtils.CreateFrom<AabbShape2DResource>(checkShape);
			return true;
		}
		if (name == PropertyName.campFilter)
		{
			value = VariantUtils.CreateFrom<ChangeProjectileStateComponent.ProjectileCampFilter>(campFilter);
			return true;
		}
		bool from;
		if (name == PropertyName.followCamp)
		{
			from = followCamp;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.replaceDamageFlags)
		{
			from = replaceDamageFlags;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.replaceFireMethodFlags)
		{
			from = replaceFireMethodFlags;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.enableTrackingFromFireMethod)
		{
			from = enableTrackingFromFireMethod;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.damageFlags)
		{
			from2 = damageFlags;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.fireMethodFlags)
		{
			from2 = fireMethodFlags;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CollisionPreviewShapeCount)
		{
			from2 = CollisionPreviewShapeCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CollisionPreviewRayCount)
		{
			from2 = CollisionPreviewRayCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.checkShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.campFilter, PropertyHint.Enum, "LegacyFollowCamp,EnemyOnly,AllyOnly,Any", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.followCamp, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.replaceDamageFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.replaceFireMethodFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enableTrackingFromFireMethod, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.damageFlags, PropertyHint.Flags, "HITBODY,HITSHIELD,FIRE,ICE,EXPLOSION,CHILD,PIERCE,SMASH", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.fireMethodFlags, PropertyHint.Flags, "SHOOTER,CATAPULT,THROW,TRACK", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.checkShape, Variant.From<AabbShape2DResource>(checkShape));
		info.AddProperty(PropertyName.campFilter, Variant.From<ChangeProjectileStateComponent.ProjectileCampFilter>(campFilter));
		info.AddProperty(PropertyName.followCamp, Variant.From<bool>(followCamp));
		info.AddProperty(PropertyName.replaceDamageFlags, Variant.From<bool>(replaceDamageFlags));
		info.AddProperty(PropertyName.replaceFireMethodFlags, Variant.From<bool>(replaceFireMethodFlags));
		info.AddProperty(PropertyName.enableTrackingFromFireMethod, Variant.From<bool>(enableTrackingFromFireMethod));
		info.AddProperty(PropertyName.damageFlags, Variant.From<int>(damageFlags));
		info.AddProperty(PropertyName.fireMethodFlags, Variant.From<int>(fireMethodFlags));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.checkShape, out var value))
		{
			checkShape = value.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.campFilter, out var value2))
		{
			campFilter = value2.As<ChangeProjectileStateComponent.ProjectileCampFilter>();
		}
		if (info.TryGetProperty(PropertyName.followCamp, out var value3))
		{
			followCamp = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.replaceDamageFlags, out var value4))
		{
			replaceDamageFlags = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.replaceFireMethodFlags, out var value5))
		{
			replaceFireMethodFlags = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.enableTrackingFromFireMethod, out var value6))
		{
			enableTrackingFromFireMethod = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.damageFlags, out var value7))
		{
			damageFlags = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.fireMethodFlags, out var value8))
		{
			fireMethodFlags = value8.As<int>();
		}
	}
}
