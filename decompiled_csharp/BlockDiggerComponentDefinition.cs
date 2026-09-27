using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/BlockDiggerComponent/BlockDiggerComponentDefinition.cs")]
public class BlockDiggerComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName checkShape = "checkShape";

		public static readonly StringName blockSameCamp = "blockSameCamp";

		public static readonly StringName characterCheckEveryPhysicsFrames = "characterCheckEveryPhysicsFrames";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public AabbShape2DResource checkShape { get; set; }

	[Export(PropertyHint.None, "")]
	public bool blockSameCamp { get; set; }

	[ExportGroup("Performance", "")]
	[Export(PropertyHint.Range, "1,60,1")]
	public int characterCheckEveryPhysicsFrames { get; set; } = 1;

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
		return new BlockDiggerComponent();
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
		if (name == PropertyName.blockSameCamp)
		{
			blockSameCamp = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.checkShape)
		{
			value = VariantUtils.CreateFrom<AabbShape2DResource>(checkShape);
			return true;
		}
		if (name == PropertyName.blockSameCamp)
		{
			value = VariantUtils.CreateFrom<bool>(blockSameCamp);
			return true;
		}
		int from;
		if (name == PropertyName.characterCheckEveryPhysicsFrames)
		{
			from = characterCheckEveryPhysicsFrames;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CollisionPreviewShapeCount)
		{
			from = CollisionPreviewShapeCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CollisionPreviewRayCount)
		{
			from = CollisionPreviewRayCount;
			value = VariantUtils.CreateFrom(in from);
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
			new PropertyInfo(Variant.Type.Bool, PropertyName.blockSameCamp, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
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
		info.AddProperty(PropertyName.checkShape, Variant.From<AabbShape2DResource>(checkShape));
		info.AddProperty(PropertyName.blockSameCamp, Variant.From<bool>(blockSameCamp));
		info.AddProperty(PropertyName.characterCheckEveryPhysicsFrames, Variant.From<int>(characterCheckEveryPhysicsFrames));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.checkShape, out var value))
		{
			checkShape = value.As<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.blockSameCamp, out var value2))
		{
			blockSameCamp = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.characterCheckEveryPhysicsFrames, out var value3))
		{
			characterCheckEveryPhysicsFrames = value3.As<int>();
		}
	}
}
