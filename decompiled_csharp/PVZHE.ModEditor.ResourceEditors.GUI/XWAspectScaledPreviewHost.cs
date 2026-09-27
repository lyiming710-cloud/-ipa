using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAspectScaledPreviewHost.cs")]
public class XWAspectScaledPreviewHost : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ApplyResponsiveLayout = "ApplyResponsiveLayout";

		public static readonly StringName PreviewFitsWithinBounds = "PreviewFitsWithinBounds";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName DesignSize = "DesignSize";

		public static readonly StringName AllowUpscale = "AllowUpscale";

		public static readonly StringName PreviewScale = "PreviewScale";

		public static readonly StringName PreviewVisualRect = "PreviewVisualRect";

		public static readonly StringName _viewportContainer = "_viewportContainer";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private SubViewportContainer _viewportContainer;

	[Export(PropertyHint.None, "")]
	public Vector2 DesignSize { get; set; } = new Vector2(1080f, 600f);

	[Export(PropertyHint.None, "")]
	public bool AllowUpscale { get; set; }

	public float PreviewScale { get; private set; } = 1f;

	public Rect2 PreviewVisualRect { get; private set; }

	public override void _Ready()
	{
		_viewportContainer = GetNodeOrNull<SubViewportContainer>("ViewportContainer");
		Resized += ApplyResponsiveLayout;
		CallDeferred("ApplyResponsiveLayout");
	}

	public override void _ExitTree()
	{
		Resized -= ApplyResponsiveLayout;
		base._ExitTree();
	}

	public void ApplyResponsiveLayout()
	{
		if (GodotObject.IsInstanceValid(_viewportContainer) && !(Size.X <= 0f) && !(Size.Y <= 0f) && !(DesignSize.X <= 0f) && !(DesignSize.Y <= 0f))
		{
			float a = Mathf.Min(Size.X / DesignSize.X, Size.Y / DesignSize.Y);
			if (!AllowUpscale)
			{
				a = Mathf.Min(a, 1f);
			}
			PreviewScale = Mathf.Max(a, 0.05f);
			Vector2 vector = DesignSize * PreviewScale;
			Vector2 position = (Size - vector) * 0.5f;
			_viewportContainer.Size = DesignSize;
			_viewportContainer.Scale = Vector2.One * PreviewScale;
			_viewportContainer.Position = position;
			PreviewVisualRect = new Rect2(position, vector);
		}
	}

	public bool PreviewFitsWithinBounds(float tolerance = 1f)
	{
		if (PreviewVisualRect.Position.X >= 0f - tolerance && PreviewVisualRect.Position.Y >= 0f - tolerance && PreviewVisualRect.End.X <= Size.X + tolerance)
		{
			return PreviewVisualRect.End.Y <= Size.Y + tolerance;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(4)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyResponsiveLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviewFitsWithinBounds, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "tolerance", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyResponsiveLayout && args.Count == 0)
		{
			ApplyResponsiveLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewFitsWithinBounds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(PreviewFitsWithinBounds(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ApplyResponsiveLayout)
		{
			return true;
		}
		if (method == MethodName.PreviewFitsWithinBounds)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.DesignSize)
		{
			DesignSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.AllowUpscale)
		{
			AllowUpscale = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.PreviewScale)
		{
			PreviewScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.PreviewVisualRect)
		{
			PreviewVisualRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName._viewportContainer)
		{
			_viewportContainer = VariantUtils.ConvertTo<SubViewportContainer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.DesignSize)
		{
			value = VariantUtils.CreateFrom<Vector2>(DesignSize);
			return true;
		}
		if (name == PropertyName.AllowUpscale)
		{
			value = VariantUtils.CreateFrom<bool>(AllowUpscale);
			return true;
		}
		if (name == PropertyName.PreviewScale)
		{
			value = VariantUtils.CreateFrom<float>(PreviewScale);
			return true;
		}
		if (name == PropertyName.PreviewVisualRect)
		{
			value = VariantUtils.CreateFrom<Rect2>(PreviewVisualRect);
			return true;
		}
		if (name == PropertyName._viewportContainer)
		{
			value = VariantUtils.CreateFrom(in _viewportContainer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Vector2, PropertyName.DesignSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AllowUpscale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._viewportContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.PreviewScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.PreviewVisualRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.DesignSize, Variant.From<Vector2>(DesignSize));
		info.AddProperty(PropertyName.AllowUpscale, Variant.From<bool>(AllowUpscale));
		info.AddProperty(PropertyName.PreviewScale, Variant.From<float>(PreviewScale));
		info.AddProperty(PropertyName.PreviewVisualRect, Variant.From<Rect2>(PreviewVisualRect));
		info.AddProperty(PropertyName._viewportContainer, Variant.From(in _viewportContainer));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.DesignSize, out var value))
		{
			DesignSize = value.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.AllowUpscale, out var value2))
		{
			AllowUpscale = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.PreviewScale, out var value3))
		{
			PreviewScale = value3.As<float>();
		}
		if (info.TryGetProperty(PropertyName.PreviewVisualRect, out var value4))
		{
			PreviewVisualRect = value4.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName._viewportContainer, out var value5))
		{
			_viewportContainer = value5.As<SubViewportContainer>();
		}
	}
}
