using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Collision/AabbShape2DResource.cs")]
public class AabbShape2DResource : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName GetGeometryHash = "GetGeometryHash";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Enabled = "Enabled";

		public static readonly StringName Geometry = "Geometry";

		public static readonly StringName LocalTransform = "LocalTransform";

		public static readonly StringName DebugDraw = "DebugDraw";

		public static readonly StringName DebugFillColor = "DebugFillColor";

		public static readonly StringName DebugOutlineColor = "DebugOutlineColor";

		public static readonly StringName DebugLineWidth = "DebugLineWidth";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public bool Enabled { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Shape2D Geometry { get; set; }

	[Export(PropertyHint.None, "")]
	public Transform2D LocalTransform { get; set; } = Transform2D.Identity;

	[ExportGroup("Collision Visualization", "")]
	[Export(PropertyHint.None, "")]
	public bool DebugDraw { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Color DebugFillColor { get; set; } = new Color(0.1f, 0.75f, 1f, 0.18f);

	[Export(PropertyHint.None, "")]
	public Color DebugOutlineColor { get; set; } = new Color(0.1f, 0.75f, 1f, 0.85f);

	[Export(PropertyHint.Range, "0.5,12,0.5,or_greater")]
	public float DebugLineWidth { get; set; } = 2f;

	public bool TryGetWorldRect(Transform2D ownerTransform, out Rect2 rect)
	{
		if (!Enabled || !GodotObject.IsInstanceValid(Geometry))
		{
			rect = default;
			return false;
		}
		return AabbShapeUtil.TryComputeShapeWorldRect(Geometry, ownerTransform * LocalTransform, out rect);
	}

	public int GetGeometryHash()
	{
		HashCode hash = default;
		hash.Add(Enabled);
		hash.Add(LocalTransform);
		AabbShapeUtil.AddShapeGeometryHash(ref hash, Geometry);
		return hash.ToHashCode();
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
		if (name == PropertyName.Geometry)
		{
			Geometry = VariantUtils.ConvertTo<Shape2D>(in value);
			return true;
		}
		if (name == PropertyName.LocalTransform)
		{
			LocalTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName.DebugDraw)
		{
			DebugDraw = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.DebugFillColor)
		{
			DebugFillColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.DebugOutlineColor)
		{
			DebugOutlineColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.DebugLineWidth)
		{
			DebugLineWidth = VariantUtils.ConvertTo<float>(in value);
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
		if (name == PropertyName.Geometry)
		{
			value = VariantUtils.CreateFrom<Shape2D>(Geometry);
			return true;
		}
		if (name == PropertyName.LocalTransform)
		{
			value = VariantUtils.CreateFrom<Transform2D>(LocalTransform);
			return true;
		}
		if (name == PropertyName.DebugDraw)
		{
			from = DebugDraw;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Color from2;
		if (name == PropertyName.DebugFillColor)
		{
			from2 = DebugFillColor;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.DebugOutlineColor)
		{
			from2 = DebugOutlineColor;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.DebugLineWidth)
		{
			value = VariantUtils.CreateFrom<float>(DebugLineWidth);
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
			new PropertyInfo(Variant.Type.Object, PropertyName.Geometry, PropertyHint.ResourceType, "Shape2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName.LocalTransform, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Collision Visualization", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.DebugDraw, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.DebugFillColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.DebugOutlineColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.DebugLineWidth, PropertyHint.Range, "0.5,12,0.5,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Enabled, Variant.From<bool>(Enabled));
		info.AddProperty(PropertyName.Geometry, Variant.From<Shape2D>(Geometry));
		info.AddProperty(PropertyName.LocalTransform, Variant.From<Transform2D>(LocalTransform));
		info.AddProperty(PropertyName.DebugDraw, Variant.From<bool>(DebugDraw));
		info.AddProperty(PropertyName.DebugFillColor, Variant.From<Color>(DebugFillColor));
		info.AddProperty(PropertyName.DebugOutlineColor, Variant.From<Color>(DebugOutlineColor));
		info.AddProperty(PropertyName.DebugLineWidth, Variant.From<float>(DebugLineWidth));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Enabled, out var value))
		{
			Enabled = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.Geometry, out var value2))
		{
			Geometry = value2.As<Shape2D>();
		}
		if (info.TryGetProperty(PropertyName.LocalTransform, out var value3))
		{
			LocalTransform = value3.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName.DebugDraw, out var value4))
		{
			DebugDraw = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.DebugFillColor, out var value5))
		{
			DebugFillColor = value5.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.DebugOutlineColor, out var value6))
		{
			DebugOutlineColor = value6.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.DebugLineWidth, out var value7))
		{
			DebugLineWidth = value7.As<float>();
		}
	}
}
