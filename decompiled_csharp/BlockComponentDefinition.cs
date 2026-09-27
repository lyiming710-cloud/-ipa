using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/BlockComponent/BlockComponentDefinition.cs")]
public class BlockComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName blockType = "blockType";

		public static readonly StringName extendGrid = "extendGrid";

		public static readonly StringName checkShape = "checkShape";

		public static readonly StringName checkLadder = "checkLadder";

		public static readonly StringName reboundProjectile = "reboundProjectile";

		public static readonly StringName reboundProjectileShape = "reboundProjectileShape";

		public static readonly StringName characterCheckEveryPhysicsFrames = "characterCheckEveryPhysicsFrames";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Array<string> blockType { get; set; } = new Array<string> { "General" };

	[Export(PropertyHint.None, "")]
	public Vector2 extendGrid { get; set; } = Vector2.One;

	[Export(PropertyHint.None, "")]
	public AabbShape2DResource checkShape { get; set; }

	[Export(PropertyHint.None, "")]
	public bool checkLadder { get; set; }

	[Export(PropertyHint.None, "")]
	public bool reboundProjectile { get; set; }

	[Export(PropertyHint.None, "")]
	public AabbShape2DResource reboundProjectileShape { get; set; }

	[ExportGroup("Performance", "")]
	[Export(PropertyHint.Range, "1,60,1")]
	public int characterCheckEveryPhysicsFrames { get; set; } = 1;

	public int CollisionPreviewShapeCount => (GodotObject.IsInstanceValid(checkShape) ? 1 : 0) + ((reboundProjectile && GodotObject.IsInstanceValid(reboundProjectileShape)) ? 1 : 0);

	public int CollisionPreviewRayCount => 0;

	public AabbShape2DResource GetCollisionPreviewShape(int index)
	{
		if (index < 0)
		{
			return null;
		}
		if (GodotObject.IsInstanceValid(checkShape))
		{
			if (index == 0)
			{
				return checkShape;
			}
			index--;
		}
		if (!reboundProjectile || index != 0 || !GodotObject.IsInstanceValid(reboundProjectileShape))
		{
			return null;
		}
		return reboundProjectileShape;
	}

	public AabbRay2DResource GetCollisionPreviewRay(int index)
	{
		return null;
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new BlockComponent();
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
		if (name == PropertyName.blockType)
		{
			blockType = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.extendGrid)
		{
			extendGrid = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.checkShape)
		{
			checkShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.checkLadder)
		{
			checkLadder = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.reboundProjectile)
		{
			reboundProjectile = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.reboundProjectileShape)
		{
			reboundProjectileShape = VariantUtils.ConvertTo<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.characterCheckEveryPhysicsFrames)
		{
			characterCheckEveryPhysicsFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.blockType)
		{
			value = VariantUtils.CreateFromArray(blockType);
			return true;
		}
		if (name == PropertyName.extendGrid)
		{
			value = VariantUtils.CreateFrom<Vector2>(extendGrid);
			return true;
		}
		AabbShape2DResource from;
		if (name == PropertyName.checkShape)
		{
			from = checkShape;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.checkLadder)
		{
			from2 = checkLadder;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.reboundProjectile)
		{
			from2 = reboundProjectile;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.reboundProjectileShape)
		{
			from = reboundProjectileShape;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from3;
		if (name == PropertyName.characterCheckEveryPhysicsFrames)
		{
			from3 = characterCheckEveryPhysicsFrames;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CollisionPreviewShapeCount)
		{
			from3 = CollisionPreviewShapeCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CollisionPreviewRayCount)
		{
			from3 = CollisionPreviewRayCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName.blockType, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.extendGrid, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.checkShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.checkLadder, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.reboundProjectile, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.reboundProjectileShape, PropertyHint.ResourceType, "AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Performance", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.characterCheckEveryPhysicsFrames, PropertyHint.Range, "1,60,1", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.blockType, Variant.CreateFrom(blockType));
		info.AddProperty(PropertyName.extendGrid, Variant.From<Vector2>(extendGrid));
		info.AddProperty(PropertyName.checkShape, Variant.From<AabbShape2DResource>(checkShape));
		info.AddProperty(PropertyName.checkLadder, Variant.From<bool>(checkLadder));
		info.AddProperty(PropertyName.reboundProjectile, Variant.From<bool>(reboundProjectile));
		info.AddProperty(PropertyName.reboundProjectileShape, Variant.From<AabbShape2DResource>(reboundProjectileShape));
		info.AddProperty(PropertyName.characterCheckEveryPhysicsFrames, Variant.From<int>(characterCheckEveryPhysicsFrames));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.blockType, out var value))
		{
			blockType = value.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.extendGrid, out var value2))
		{
			extendGrid = value2.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.checkShape, out var value3))
		{
			checkShape = value3.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.checkLadder, out var value4))
		{
			checkLadder = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.reboundProjectile, out var value5))
		{
			reboundProjectile = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.reboundProjectileShape, out var value6))
		{
			reboundProjectileShape = value6.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.characterCheckEveryPhysicsFrames, out var value7))
		{
			characterCheckEveryPhysicsFrames = value7.As<int>();
		}
	}
}
