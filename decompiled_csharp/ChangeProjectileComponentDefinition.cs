using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/ChangeProjectile/ChangeProjectileComponentDefinition.cs")]
public class ChangeProjectileComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName changeName = "changeName";

		public static readonly StringName changeAudio = "changeAudio";

		public static readonly StringName checkShape = "checkShape";

		public static readonly StringName isStar = "isStar";

		public static readonly StringName trackToStraight = "trackToStraight";

		public static readonly StringName starDirections = "starDirections";

		public static readonly StringName starProjectileSpeed = "starProjectileSpeed";

		public static readonly StringName exclusionPhysicsFrames = "exclusionPhysicsFrames";

		public static readonly StringName structExcludeCleanupIntervalFrames = "structExcludeCleanupIntervalFrames";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public StringName changeName { get; set; }

	[Export(PropertyHint.None, "")]
	public string changeAudio { get; set; } = string.Empty;

	[Export(PropertyHint.None, "")]
	public AabbShape2DResource checkShape { get; set; }

	[Export(PropertyHint.None, "")]
	public bool isStar { get; set; }

	[Export(PropertyHint.None, "")]
	public bool trackToStraight { get; set; }

	[ExportGroup("Star Split", "")]
	[Export(PropertyHint.None, "")]
	public Array<float> starDirections { get; set; } = new Array<float> { 330f, 270f, 180f, 90f, 30f };

	[Export(PropertyHint.None, "")]
	public float starProjectileSpeed { get; set; } = 500f;

	[Export(PropertyHint.Range, "1,60,1")]
	public int exclusionPhysicsFrames { get; set; } = 2;

	[Export(PropertyHint.Range, "1,600,1")]
	public int structExcludeCleanupIntervalFrames { get; set; } = 60;

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
		return new ChangeProjectileComponent();
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
		if (name == PropertyName.changeName)
		{
			changeName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.changeAudio)
		{
			changeAudio = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.checkShape)
		{
			checkShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.isStar)
		{
			isStar = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.trackToStraight)
		{
			trackToStraight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.starDirections)
		{
			starDirections = VariantUtils.ConvertToArray<float>(in value);
			return true;
		}
		if (name == PropertyName.starProjectileSpeed)
		{
			starProjectileSpeed = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.exclusionPhysicsFrames)
		{
			exclusionPhysicsFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.structExcludeCleanupIntervalFrames)
		{
			structExcludeCleanupIntervalFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.changeName)
		{
			value = VariantUtils.CreateFrom<StringName>(changeName);
			return true;
		}
		if (name == PropertyName.changeAudio)
		{
			value = VariantUtils.CreateFrom<string>(changeAudio);
			return true;
		}
		if (name == PropertyName.checkShape)
		{
			value = VariantUtils.CreateFrom<AabbShape2DResource>(checkShape);
			return true;
		}
		bool from;
		if (name == PropertyName.isStar)
		{
			from = isStar;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.trackToStraight)
		{
			from = trackToStraight;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.starDirections)
		{
			value = VariantUtils.CreateFromArray(starDirections);
			return true;
		}
		if (name == PropertyName.starProjectileSpeed)
		{
			value = VariantUtils.CreateFrom<float>(starProjectileSpeed);
			return true;
		}
		int from2;
		if (name == PropertyName.exclusionPhysicsFrames)
		{
			from2 = exclusionPhysicsFrames;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.structExcludeCleanupIntervalFrames)
		{
			from2 = structExcludeCleanupIntervalFrames;
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
			new PropertyInfo(Variant.Type.StringName, PropertyName.changeName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.changeAudio, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.checkShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.isStar, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.trackToStraight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Star Split", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.starDirections, PropertyHint.TypeString, "3/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.starProjectileSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.exclusionPhysicsFrames, PropertyHint.Range, "1,60,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.structExcludeCleanupIntervalFrames, PropertyHint.Range, "1,600,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.changeName, Variant.From<StringName>(changeName));
		info.AddProperty(PropertyName.changeAudio, Variant.From<string>(changeAudio));
		info.AddProperty(PropertyName.checkShape, Variant.From<AabbShape2DResource>(checkShape));
		info.AddProperty(PropertyName.isStar, Variant.From<bool>(isStar));
		info.AddProperty(PropertyName.trackToStraight, Variant.From<bool>(trackToStraight));
		info.AddProperty(PropertyName.starDirections, Variant.CreateFrom(starDirections));
		info.AddProperty(PropertyName.starProjectileSpeed, Variant.From<float>(starProjectileSpeed));
		info.AddProperty(PropertyName.exclusionPhysicsFrames, Variant.From<int>(exclusionPhysicsFrames));
		info.AddProperty(PropertyName.structExcludeCleanupIntervalFrames, Variant.From<int>(structExcludeCleanupIntervalFrames));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.changeName, out var value))
		{
			changeName = value.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.changeAudio, out var value2))
		{
			changeAudio = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.checkShape, out var value3))
		{
			checkShape = value3.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.isStar, out var value4))
		{
			isStar = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.trackToStraight, out var value5))
		{
			trackToStraight = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.starDirections, out var value6))
		{
			starDirections = value6.AsGodotArray<float>();
		}
		if (info.TryGetProperty(PropertyName.starProjectileSpeed, out var value7))
		{
			starProjectileSpeed = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.exclusionPhysicsFrames, out var value8))
		{
			exclusionPhysicsFrames = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.structExcludeCleanupIntervalFrames, out var value9))
		{
			structExcludeCleanupIntervalFrames = value9.As<int>();
		}
	}
}
