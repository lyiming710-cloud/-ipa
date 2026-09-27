using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://Script/Component/TowerDefense/Character/CharacterAabbAreaComponent/CharacterAabbAreaComponentDefinition.cs")]
public class CharacterAabbAreaComponentDefinition : CharacterComponentDefinition, ICharacterComponentCollisionPreviewProvider
{
	public new class MethodName : CharacterComponentDefinition.MethodName
	{
		public static readonly StringName GetCollisionPreviewShape = "GetCollisionPreviewShape";

		public static readonly StringName GetCollisionPreviewRay = "GetCollisionPreviewRay";
	}

	public new class PropertyName : CharacterComponentDefinition.PropertyName
	{
		public static readonly StringName enabled = "enabled";

		public static readonly StringName shapeResources = "shapeResources";

		public static readonly StringName collisionLayer = "collisionLayer";

		public static readonly StringName collisionMask = "collisionMask";

		public static readonly StringName monitoring = "monitoring";

		public static readonly StringName anchorPath = "anchorPath";

		public static readonly StringName localTransform = "localTransform";

		public static readonly StringName CollisionPreviewShapeCount = "CollisionPreviewShapeCount";

		public static readonly StringName CollisionPreviewRayCount = "CollisionPreviewRayCount";
	}

	public new class SignalName : CharacterComponentDefinition.SignalName
	{
	}

	[ExportGroup("Area", "")]
	[Export(PropertyHint.None, "")]
	public bool enabled { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Array<AabbShape2DResource> shapeResources { get; set; } = new Array<AabbShape2DResource>();

	[Export(PropertyHint.Layers2DPhysics, "")]
	public uint collisionLayer { get; set; } = 1u;

	[Export(PropertyHint.Layers2DPhysics, "")]
	public uint collisionMask { get; set; } = 1u;

	[Export(PropertyHint.None, "")]
	public bool monitoring { get; set; } = true;

	[ExportGroup("Owner-relative Transform", "")]
	[Export(PropertyHint.None, "")]
	public NodePath anchorPath { get; set; } = new NodePath();

	[Export(PropertyHint.None, "")]
	public Transform2D localTransform { get; set; } = Transform2D.Identity;

	public int CollisionPreviewShapeCount => shapeResources?.Count ?? 0;

	public int CollisionPreviewRayCount => 0;

	public AabbShape2DResource GetCollisionPreviewShape(int index)
	{
		if (index < 0 || index >= (shapeResources?.Count ?? 0))
		{
			return null;
		}
		return shapeResources[index];
	}

	public AabbRay2DResource GetCollisionPreviewRay(int index)
	{
		return null;
	}

	public override CharacterComponentRuntime CreateRuntime()
	{
		return new CharacterAabbAreaComponent();
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
		if (name == PropertyName.enabled)
		{
			enabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.shapeResources)
		{
			shapeResources = VariantUtils.ConvertToArray<AabbShape2DResource>(in value);
			return true;
		}
		if (name == PropertyName.collisionLayer)
		{
			collisionLayer = VariantUtils.ConvertTo<uint>(in value);
			return true;
		}
		if (name == PropertyName.collisionMask)
		{
			collisionMask = VariantUtils.ConvertTo<uint>(in value);
			return true;
		}
		if (name == PropertyName.monitoring)
		{
			monitoring = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.anchorPath)
		{
			anchorPath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.localTransform)
		{
			localTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.enabled)
		{
			from = enabled;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.shapeResources)
		{
			value = VariantUtils.CreateFromArray(shapeResources);
			return true;
		}
		uint from2;
		if (name == PropertyName.collisionLayer)
		{
			from2 = collisionLayer;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.collisionMask)
		{
			from2 = collisionMask;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.monitoring)
		{
			from = monitoring;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.anchorPath)
		{
			value = VariantUtils.CreateFrom<NodePath>(anchorPath);
			return true;
		}
		if (name == PropertyName.localTransform)
		{
			value = VariantUtils.CreateFrom<Transform2D>(localTransform);
			return true;
		}
		int from3;
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
			new PropertyInfo(Variant.Type.Nil, "Area", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.enabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.shapeResources, PropertyHint.TypeString, "24/17:AabbShape2DResource", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionLayer, PropertyHint.Layers2DPhysics, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.collisionMask, PropertyHint.Layers2DPhysics, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.monitoring, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Owner-relative Transform", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.anchorPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName.localTransform, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewShapeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollisionPreviewRayCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.enabled, Variant.From<bool>(enabled));
		info.AddProperty(PropertyName.shapeResources, Variant.CreateFrom(shapeResources));
		info.AddProperty(PropertyName.collisionLayer, Variant.From<uint>(collisionLayer));
		info.AddProperty(PropertyName.collisionMask, Variant.From<uint>(collisionMask));
		info.AddProperty(PropertyName.monitoring, Variant.From<bool>(monitoring));
		info.AddProperty(PropertyName.anchorPath, Variant.From<NodePath>(anchorPath));
		info.AddProperty(PropertyName.localTransform, Variant.From<Transform2D>(localTransform));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.enabled, out var value))
		{
			enabled = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.shapeResources, out var value2))
		{
			shapeResources = value2.AsGodotArray<AabbShape2DResource>();
		}
		if (info.TryGetProperty(PropertyName.collisionLayer, out var value3))
		{
			collisionLayer = value3.As<uint>();
		}
		if (info.TryGetProperty(PropertyName.collisionMask, out var value4))
		{
			collisionMask = value4.As<uint>();
		}
		if (info.TryGetProperty(PropertyName.monitoring, out var value5))
		{
			monitoring = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.anchorPath, out var value6))
		{
			anchorPath = value6.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.localTransform, out var value7))
		{
			localTransform = value7.As<Transform2D>();
		}
	}
}
