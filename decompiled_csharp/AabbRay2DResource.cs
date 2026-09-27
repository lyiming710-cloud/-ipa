using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Collision/AabbRay2DResource.cs")]
public class AabbRay2DResource : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName GetGeometryHash = "GetGeometryHash";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Enabled = "Enabled";

		public static readonly StringName LocalTransform = "LocalTransform";

		public static readonly StringName TargetPosition = "TargetPosition";

		public static readonly StringName DebugDraw = "DebugDraw";

		public static readonly StringName DebugColor = "DebugColor";

		public static readonly StringName DebugLineWidth = "DebugLineWidth";

		public static readonly StringName DebugEndpointRadius = "DebugEndpointRadius";

		public static readonly StringName DebugArrowSize = "DebugArrowSize";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool Enabled { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Transform2D LocalTransform { get; set; } = Transform2D.Identity;

	[Export(PropertyHint.None, "")]
	public Vector2 TargetPosition { get; set; } = new Vector2(2000f, 0f);

	[ExportGroup("Collision Visualization", "")]
	[Export(PropertyHint.None, "")]
	public bool DebugDraw { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Color DebugColor { get; set; } = new Color(0.2f, 1f, 0.25f, 0.8f);

	[Export(PropertyHint.Range, "0.5,12,0.5,or_greater")]
	public float DebugLineWidth { get; set; } = 2f;

	[Export(PropertyHint.Range, "1,64,1,or_greater")]
	public float DebugEndpointRadius { get; set; } = 5f;

	[Export(PropertyHint.Range, "1,128,1,or_greater")]
	public float DebugArrowSize { get; set; } = 14f;

	public bool TryGetWorldSegment(Transform2D ownerTransform, out Vector2 origin, out Vector2 target)
	{
		return TryGetWorldSegment(ownerTransform, TargetPosition, out origin, out target);
	}

	public bool TryGetWorldSegment(Transform2D ownerTransform, Vector2 runtimeTargetPosition, out Vector2 origin, out Vector2 target)
	{
		return TryGetWorldSegment(ownerTransform, LocalTransform, runtimeTargetPosition, out origin, out target);
	}

	public bool TryGetWorldSegment(Transform2D ownerTransform, Transform2D runtimeLocalTransform, Vector2 runtimeTargetPosition, out Vector2 origin, out Vector2 target)
	{
		Transform2D transform2D = ownerTransform * runtimeLocalTransform;
		origin = transform2D.Origin;
		target = transform2D * runtimeTargetPosition;
		if (Enabled && runtimeTargetPosition.IsFinite())
		{
			return (target - origin).LengthSquared() >= 1E-06f;
		}
		return false;
	}

	public int GetGeometryHash()
	{
		HashCode hashCode = default;
		hashCode.Add(Enabled);
		hashCode.Add(LocalTransform);
		hashCode.Add(TargetPosition);
		return hashCode.ToHashCode();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(MethodName.GetGeometryHash, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetGeometryHash && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetGeometryHash());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.GetGeometryHash)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Enabled)
		{
			Enabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.LocalTransform)
		{
			LocalTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName.TargetPosition)
		{
			TargetPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.DebugDraw)
		{
			DebugDraw = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.DebugColor)
		{
			DebugColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.DebugLineWidth)
		{
			DebugLineWidth = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.DebugEndpointRadius)
		{
			DebugEndpointRadius = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.DebugArrowSize)
		{
			DebugArrowSize = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.Enabled)
		{
			from = Enabled;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.LocalTransform)
		{
			value = VariantUtils.CreateFrom<Transform2D>(LocalTransform);
			return true;
		}
		if (name == PropertyName.TargetPosition)
		{
			value = VariantUtils.CreateFrom<Vector2>(TargetPosition);
			return true;
		}
		if (name == PropertyName.DebugDraw)
		{
			from = DebugDraw;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DebugColor)
		{
			value = VariantUtils.CreateFrom<Color>(DebugColor);
			return true;
		}
		float from2;
		if (name == PropertyName.DebugLineWidth)
		{
			from2 = DebugLineWidth;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.DebugEndpointRadius)
		{
			from2 = DebugEndpointRadius;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.DebugArrowSize)
		{
			from2 = DebugArrowSize;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.Enabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName.LocalTransform, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.TargetPosition, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Collision Visualization", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.DebugDraw, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.DebugColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.DebugLineWidth, PropertyHint.Range, "0.5,12,0.5,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.DebugEndpointRadius, PropertyHint.Range, "1,64,1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.DebugArrowSize, PropertyHint.Range, "1,128,1,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Enabled, Variant.From<bool>(Enabled));
		info.AddProperty(PropertyName.LocalTransform, Variant.From<Transform2D>(LocalTransform));
		info.AddProperty(PropertyName.TargetPosition, Variant.From<Vector2>(TargetPosition));
		info.AddProperty(PropertyName.DebugDraw, Variant.From<bool>(DebugDraw));
		info.AddProperty(PropertyName.DebugColor, Variant.From<Color>(DebugColor));
		info.AddProperty(PropertyName.DebugLineWidth, Variant.From<float>(DebugLineWidth));
		info.AddProperty(PropertyName.DebugEndpointRadius, Variant.From<float>(DebugEndpointRadius));
		info.AddProperty(PropertyName.DebugArrowSize, Variant.From<float>(DebugArrowSize));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Enabled, out var value))
		{
			Enabled = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.LocalTransform, out var value2))
		{
			LocalTransform = value2.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName.TargetPosition, out var value3))
		{
			TargetPosition = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.DebugDraw, out var value4))
		{
			DebugDraw = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.DebugColor, out var value5))
		{
			DebugColor = value5.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.DebugLineWidth, out var value6))
		{
			DebugLineWidth = value6.As<float>();
		}
		if (info.TryGetProperty(PropertyName.DebugEndpointRadius, out var value7))
		{
			DebugEndpointRadius = value7.As<float>();
		}
		if (info.TryGetProperty(PropertyName.DebugArrowSize, out var value8))
		{
			DebugArrowSize = value8.As<float>();
		}
	}
}
