using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://Resource/TowerDefense/Collision/CharacterHitBoxDefinition.cs")]
public class CharacterHitBoxDefinition : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName GetGeometryHash = "GetGeometryHash";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName Size = "Size";

		public static readonly StringName LocalTransform = "LocalTransform";

		public static readonly StringName DefaultEnabled = "DefaultEnabled";

		public static readonly StringName DefaultMonitorable = "DefaultMonitorable";

		public static readonly StringName DebugDraw = "DebugDraw";

		public static readonly StringName DebugFillColor = "DebugFillColor";

		public static readonly StringName DebugOutlineColor = "DebugOutlineColor";

		public static readonly StringName DebugLineWidth = "DebugLineWidth";

		public static readonly StringName HasValidGeometry = "HasValidGeometry";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Vector2 Size { get; set; } = AabbShapeUtil.DefaultAreaSize;

	[Export(PropertyHint.None, "")]
	public Transform2D LocalTransform { get; set; } = Transform2D.Identity;

	[Export(PropertyHint.None, "")]
	public bool DefaultEnabled { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool DefaultMonitorable { get; set; } = true;

	[ExportGroup("Collision Visualization", "")]
	[Export(PropertyHint.None, "")]
	public bool DebugDraw { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public Color DebugFillColor { get; set; } = new Color(0.1f, 0.75f, 1f, 0.18f);

	[Export(PropertyHint.None, "")]
	public Color DebugOutlineColor { get; set; } = new Color(0.1f, 0.75f, 1f, 0.85f);

	[Export(PropertyHint.Range, "0.5,12,0.5,or_greater")]
	public float DebugLineWidth { get; set; } = 2f;

	public bool HasValidGeometry
	{
		get
		{
			if (float.IsFinite(Size.X) && float.IsFinite(Size.Y) && Size.X > 0f)
			{
				return Size.Y > 0f;
			}
			return false;
		}
	}

	public bool TryGetWorldRect(Transform2D ownerTransform, out Rect2 rect)
	{
		if (!HasValidGeometry)
		{
			rect = default;
			return false;
		}
		rect = AabbShapeUtil.ComputeRectangleWorldRect(ownerTransform * LocalTransform, Size);
		return true;
	}

	public int GetGeometryHash()
	{
		HashCode hashCode = default;
		hashCode.Add(Size);
		hashCode.Add(LocalTransform);
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
		if (name == PropertyName.Size)
		{
			Size = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.LocalTransform)
		{
			LocalTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName.DefaultEnabled)
		{
			DefaultEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.DefaultMonitorable)
		{
			DefaultMonitorable = VariantUtils.ConvertTo<bool>(in value);
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
		if (name == PropertyName.Size)
		{
			value = VariantUtils.CreateFrom<Vector2>(Size);
			return true;
		}
		if (name == PropertyName.LocalTransform)
		{
			value = VariantUtils.CreateFrom<Transform2D>(LocalTransform);
			return true;
		}
		bool from;
		if (name == PropertyName.DefaultEnabled)
		{
			from = DefaultEnabled;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DefaultMonitorable)
		{
			from = DefaultMonitorable;
			value = VariantUtils.CreateFrom(in from);
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
		if (name == PropertyName.HasValidGeometry)
		{
			from = HasValidGeometry;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector2, PropertyName.Size, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName.LocalTransform, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.DefaultEnabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.DefaultMonitorable, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Collision Visualization", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.DebugDraw, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.DebugFillColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.DebugOutlineColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.DebugLineWidth, PropertyHint.Range, "0.5,12,0.5,or_greater", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasValidGeometry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Size, Variant.From<Vector2>(Size));
		info.AddProperty(PropertyName.LocalTransform, Variant.From<Transform2D>(LocalTransform));
		info.AddProperty(PropertyName.DefaultEnabled, Variant.From<bool>(DefaultEnabled));
		info.AddProperty(PropertyName.DefaultMonitorable, Variant.From<bool>(DefaultMonitorable));
		info.AddProperty(PropertyName.DebugDraw, Variant.From<bool>(DebugDraw));
		info.AddProperty(PropertyName.DebugFillColor, Variant.From<Color>(DebugFillColor));
		info.AddProperty(PropertyName.DebugOutlineColor, Variant.From<Color>(DebugOutlineColor));
		info.AddProperty(PropertyName.DebugLineWidth, Variant.From<float>(DebugLineWidth));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Size, out var value))
		{
			Size = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.LocalTransform, out var value2))
		{
			LocalTransform = value2.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName.DefaultEnabled, out var value3))
		{
			DefaultEnabled = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.DefaultMonitorable, out var value4))
		{
			DefaultMonitorable = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.DebugDraw, out var value5))
		{
			DebugDraw = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.DebugFillColor, out var value6))
		{
			DebugFillColor = value6.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.DebugOutlineColor, out var value7))
		{
			DebugOutlineColor = value7.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.DebugLineWidth, out var value8))
		{
			DebugLineWidth = value8.As<float>();
		}
	}
}
