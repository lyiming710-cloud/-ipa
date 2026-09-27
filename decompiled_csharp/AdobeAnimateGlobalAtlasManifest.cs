using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://addons/AdobeAnimateEditor/Runtime/AdobeAnimateGlobalAtlasManifest.cs")]
public class AdobeAnimateGlobalAtlasManifest : Resource
{
	public new class MethodName : Resource.MethodName
	{
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName AtlasTextureArrayPath = "AtlasTextureArrayPath";

		public static readonly StringName AtlasTextureArrayLayerSize = "AtlasTextureArrayLayerSize";

		public static readonly StringName AtlasTextureArrayLayerOffset = "AtlasTextureArrayLayerOffset";

		public static readonly StringName AtlasTextureArrayLayerCount = "AtlasTextureArrayLayerCount";

		public static readonly StringName AtlasTextureArrayColumns = "AtlasTextureArrayColumns";

		public static readonly StringName SourceKeys = "SourceKeys";

		public static readonly StringName SourceStarts = "SourceStarts";

		public static readonly StringName SourceCounts = "SourceCounts";

		public static readonly StringName SourceSignatures = "SourceSignatures";

		public static readonly StringName PoseTextureArrayPath = "PoseTextureArrayPath";

		public static readonly StringName PoseTextureArrayLayerSize = "PoseTextureArrayLayerSize";

		public static readonly StringName PoseTextureArrayLayerCount = "PoseTextureArrayLayerCount";

		public static readonly StringName PoseTextureArrayColumns = "PoseTextureArrayColumns";

		public static readonly StringName PoseAtlasPagePaths = "PoseAtlasPagePaths";

		public static readonly StringName SourcePoseAtlasPages = "SourcePoseAtlasPages";

		public static readonly StringName SourcePoseTexelStarts = "SourcePoseTexelStarts";

		public static readonly StringName SourcePoseTexelCounts = "SourcePoseTexelCounts";

		public static readonly StringName SourcePoseSignatures = "SourcePoseSignatures";

		public static readonly StringName MediaAtlasPages = "MediaAtlasPages";

		public static readonly StringName MediaRects = "MediaRects";

		public static readonly StringName ExternalTexturePaths = "ExternalTexturePaths";

		public static readonly StringName ExternalTextureAtlasPages = "ExternalTextureAtlasPages";

		public static readonly StringName ExternalTextureRects = "ExternalTextureRects";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public string AtlasTextureArrayPath { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public Vector2 AtlasTextureArrayLayerSize { get; set; } = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public int AtlasTextureArrayLayerOffset { get; set; }

	[Export(PropertyHint.None, "")]
	public int AtlasTextureArrayLayerCount { get; set; }

	[Export(PropertyHint.None, "")]
	public int AtlasTextureArrayColumns { get; set; }

	[Export(PropertyHint.None, "")]
	public Array<string> SourceKeys { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<int> SourceStarts { get; set; } = new Array<int>();

	[Export(PropertyHint.None, "")]
	public Array<int> SourceCounts { get; set; } = new Array<int>();

	[Export(PropertyHint.None, "")]
	public Array<string> SourceSignatures { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public string PoseTextureArrayPath { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public Vector2I PoseTextureArrayLayerSize { get; set; } = Vector2I.Zero;

	[Export(PropertyHint.None, "")]
	public int PoseTextureArrayLayerCount { get; set; }

	[Export(PropertyHint.None, "")]
	public int PoseTextureArrayColumns { get; set; }

	[Export(PropertyHint.None, "")]
	public Array<string> PoseAtlasPagePaths { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<int> SourcePoseAtlasPages { get; set; } = new Array<int>();

	[Export(PropertyHint.None, "")]
	public Array<int> SourcePoseTexelStarts { get; set; } = new Array<int>();

	[Export(PropertyHint.None, "")]
	public Array<int> SourcePoseTexelCounts { get; set; } = new Array<int>();

	[Export(PropertyHint.None, "")]
	public Array<string> SourcePoseSignatures { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<int> MediaAtlasPages { get; set; } = new Array<int>();

	[Export(PropertyHint.None, "")]
	public Array<Rect2> MediaRects { get; set; } = new Array<Rect2>();

	[Export(PropertyHint.None, "")]
	public Array<string> ExternalTexturePaths { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<int> ExternalTextureAtlasPages { get; set; } = new Array<int>();

	[Export(PropertyHint.None, "")]
	public Array<Rect2> ExternalTextureRects { get; set; } = new Array<Rect2>();

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.AtlasTextureArrayPath)
		{
			AtlasTextureArrayPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.AtlasTextureArrayLayerSize)
		{
			AtlasTextureArrayLayerSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.AtlasTextureArrayLayerOffset)
		{
			AtlasTextureArrayLayerOffset = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.AtlasTextureArrayLayerCount)
		{
			AtlasTextureArrayLayerCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.AtlasTextureArrayColumns)
		{
			AtlasTextureArrayColumns = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.SourceKeys)
		{
			SourceKeys = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.SourceStarts)
		{
			SourceStarts = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.SourceCounts)
		{
			SourceCounts = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.SourceSignatures)
		{
			SourceSignatures = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.PoseTextureArrayPath)
		{
			PoseTextureArrayPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.PoseTextureArrayLayerSize)
		{
			PoseTextureArrayLayerSize = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.PoseTextureArrayLayerCount)
		{
			PoseTextureArrayLayerCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.PoseTextureArrayColumns)
		{
			PoseTextureArrayColumns = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.PoseAtlasPagePaths)
		{
			PoseAtlasPagePaths = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.SourcePoseAtlasPages)
		{
			SourcePoseAtlasPages = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.SourcePoseTexelStarts)
		{
			SourcePoseTexelStarts = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.SourcePoseTexelCounts)
		{
			SourcePoseTexelCounts = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.SourcePoseSignatures)
		{
			SourcePoseSignatures = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.MediaAtlasPages)
		{
			MediaAtlasPages = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.MediaRects)
		{
			MediaRects = VariantUtils.ConvertToArray<Rect2>(in value);
			return true;
		}
		if (name == PropertyName.ExternalTexturePaths)
		{
			ExternalTexturePaths = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.ExternalTextureAtlasPages)
		{
			ExternalTextureAtlasPages = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName.ExternalTextureRects)
		{
			ExternalTextureRects = VariantUtils.ConvertToArray<Rect2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.AtlasTextureArrayPath)
		{
			from = AtlasTextureArrayPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AtlasTextureArrayLayerSize)
		{
			value = VariantUtils.CreateFrom<Vector2>(AtlasTextureArrayLayerSize);
			return true;
		}
		int from2;
		if (name == PropertyName.AtlasTextureArrayLayerOffset)
		{
			from2 = AtlasTextureArrayLayerOffset;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.AtlasTextureArrayLayerCount)
		{
			from2 = AtlasTextureArrayLayerCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.AtlasTextureArrayColumns)
		{
			from2 = AtlasTextureArrayColumns;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.SourceKeys)
		{
			value = VariantUtils.CreateFromArray(SourceKeys);
			return true;
		}
		if (name == PropertyName.SourceStarts)
		{
			value = VariantUtils.CreateFromArray(SourceStarts);
			return true;
		}
		if (name == PropertyName.SourceCounts)
		{
			value = VariantUtils.CreateFromArray(SourceCounts);
			return true;
		}
		if (name == PropertyName.SourceSignatures)
		{
			value = VariantUtils.CreateFromArray(SourceSignatures);
			return true;
		}
		if (name == PropertyName.PoseTextureArrayPath)
		{
			from = PoseTextureArrayPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PoseTextureArrayLayerSize)
		{
			value = VariantUtils.CreateFrom<Vector2I>(PoseTextureArrayLayerSize);
			return true;
		}
		if (name == PropertyName.PoseTextureArrayLayerCount)
		{
			from2 = PoseTextureArrayLayerCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PoseTextureArrayColumns)
		{
			from2 = PoseTextureArrayColumns;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PoseAtlasPagePaths)
		{
			value = VariantUtils.CreateFromArray(PoseAtlasPagePaths);
			return true;
		}
		if (name == PropertyName.SourcePoseAtlasPages)
		{
			value = VariantUtils.CreateFromArray(SourcePoseAtlasPages);
			return true;
		}
		if (name == PropertyName.SourcePoseTexelStarts)
		{
			value = VariantUtils.CreateFromArray(SourcePoseTexelStarts);
			return true;
		}
		if (name == PropertyName.SourcePoseTexelCounts)
		{
			value = VariantUtils.CreateFromArray(SourcePoseTexelCounts);
			return true;
		}
		if (name == PropertyName.SourcePoseSignatures)
		{
			value = VariantUtils.CreateFromArray(SourcePoseSignatures);
			return true;
		}
		if (name == PropertyName.MediaAtlasPages)
		{
			value = VariantUtils.CreateFromArray(MediaAtlasPages);
			return true;
		}
		if (name == PropertyName.MediaRects)
		{
			value = VariantUtils.CreateFromArray(MediaRects);
			return true;
		}
		if (name == PropertyName.ExternalTexturePaths)
		{
			value = VariantUtils.CreateFromArray(ExternalTexturePaths);
			return true;
		}
		if (name == PropertyName.ExternalTextureAtlasPages)
		{
			value = VariantUtils.CreateFromArray(ExternalTextureAtlasPages);
			return true;
		}
		if (name == PropertyName.ExternalTextureRects)
		{
			value = VariantUtils.CreateFromArray(ExternalTextureRects);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.AtlasTextureArrayPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.AtlasTextureArrayLayerSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.AtlasTextureArrayLayerOffset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.AtlasTextureArrayLayerCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.AtlasTextureArrayColumns, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.SourceKeys, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.SourceStarts, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.SourceCounts, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.SourceSignatures, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.PoseTextureArrayPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.PoseTextureArrayLayerSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.PoseTextureArrayLayerCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.PoseTextureArrayColumns, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.PoseAtlasPagePaths, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.SourcePoseAtlasPages, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.SourcePoseTexelStarts, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.SourcePoseTexelCounts, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.SourcePoseSignatures, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.MediaAtlasPages, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.MediaRects, PropertyHint.TypeString, "7/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.ExternalTexturePaths, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.ExternalTextureAtlasPages, PropertyHint.TypeString, "2/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.ExternalTextureRects, PropertyHint.TypeString, "7/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.AtlasTextureArrayPath, Variant.From<string>(AtlasTextureArrayPath));
		info.AddProperty(PropertyName.AtlasTextureArrayLayerSize, Variant.From<Vector2>(AtlasTextureArrayLayerSize));
		info.AddProperty(PropertyName.AtlasTextureArrayLayerOffset, Variant.From<int>(AtlasTextureArrayLayerOffset));
		info.AddProperty(PropertyName.AtlasTextureArrayLayerCount, Variant.From<int>(AtlasTextureArrayLayerCount));
		info.AddProperty(PropertyName.AtlasTextureArrayColumns, Variant.From<int>(AtlasTextureArrayColumns));
		info.AddProperty(PropertyName.SourceKeys, Variant.CreateFrom(SourceKeys));
		info.AddProperty(PropertyName.SourceStarts, Variant.CreateFrom(SourceStarts));
		info.AddProperty(PropertyName.SourceCounts, Variant.CreateFrom(SourceCounts));
		info.AddProperty(PropertyName.SourceSignatures, Variant.CreateFrom(SourceSignatures));
		info.AddProperty(PropertyName.PoseTextureArrayPath, Variant.From<string>(PoseTextureArrayPath));
		info.AddProperty(PropertyName.PoseTextureArrayLayerSize, Variant.From<Vector2I>(PoseTextureArrayLayerSize));
		info.AddProperty(PropertyName.PoseTextureArrayLayerCount, Variant.From<int>(PoseTextureArrayLayerCount));
		info.AddProperty(PropertyName.PoseTextureArrayColumns, Variant.From<int>(PoseTextureArrayColumns));
		info.AddProperty(PropertyName.PoseAtlasPagePaths, Variant.CreateFrom(PoseAtlasPagePaths));
		info.AddProperty(PropertyName.SourcePoseAtlasPages, Variant.CreateFrom(SourcePoseAtlasPages));
		info.AddProperty(PropertyName.SourcePoseTexelStarts, Variant.CreateFrom(SourcePoseTexelStarts));
		info.AddProperty(PropertyName.SourcePoseTexelCounts, Variant.CreateFrom(SourcePoseTexelCounts));
		info.AddProperty(PropertyName.SourcePoseSignatures, Variant.CreateFrom(SourcePoseSignatures));
		info.AddProperty(PropertyName.MediaAtlasPages, Variant.CreateFrom(MediaAtlasPages));
		info.AddProperty(PropertyName.MediaRects, Variant.CreateFrom(MediaRects));
		info.AddProperty(PropertyName.ExternalTexturePaths, Variant.CreateFrom(ExternalTexturePaths));
		info.AddProperty(PropertyName.ExternalTextureAtlasPages, Variant.CreateFrom(ExternalTextureAtlasPages));
		info.AddProperty(PropertyName.ExternalTextureRects, Variant.CreateFrom(ExternalTextureRects));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.AtlasTextureArrayPath, out var value))
		{
			AtlasTextureArrayPath = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.AtlasTextureArrayLayerSize, out var value2))
		{
			AtlasTextureArrayLayerSize = value2.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.AtlasTextureArrayLayerOffset, out var value3))
		{
			AtlasTextureArrayLayerOffset = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.AtlasTextureArrayLayerCount, out var value4))
		{
			AtlasTextureArrayLayerCount = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.AtlasTextureArrayColumns, out var value5))
		{
			AtlasTextureArrayColumns = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.SourceKeys, out var value6))
		{
			SourceKeys = value6.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.SourceStarts, out var value7))
		{
			SourceStarts = value7.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.SourceCounts, out var value8))
		{
			SourceCounts = value8.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.SourceSignatures, out var value9))
		{
			SourceSignatures = value9.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.PoseTextureArrayPath, out var value10))
		{
			PoseTextureArrayPath = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.PoseTextureArrayLayerSize, out var value11))
		{
			PoseTextureArrayLayerSize = value11.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.PoseTextureArrayLayerCount, out var value12))
		{
			PoseTextureArrayLayerCount = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName.PoseTextureArrayColumns, out var value13))
		{
			PoseTextureArrayColumns = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName.PoseAtlasPagePaths, out var value14))
		{
			PoseAtlasPagePaths = value14.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.SourcePoseAtlasPages, out var value15))
		{
			SourcePoseAtlasPages = value15.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.SourcePoseTexelStarts, out var value16))
		{
			SourcePoseTexelStarts = value16.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.SourcePoseTexelCounts, out var value17))
		{
			SourcePoseTexelCounts = value17.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.SourcePoseSignatures, out var value18))
		{
			SourcePoseSignatures = value18.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.MediaAtlasPages, out var value19))
		{
			MediaAtlasPages = value19.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.MediaRects, out var value20))
		{
			MediaRects = value20.AsGodotArray<Rect2>();
		}
		if (info.TryGetProperty(PropertyName.ExternalTexturePaths, out var value21))
		{
			ExternalTexturePaths = value21.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.ExternalTextureAtlasPages, out var value22))
		{
			ExternalTextureAtlasPages = value22.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName.ExternalTextureRects, out var value23))
		{
			ExternalTextureRects = value23.AsGodotArray<Rect2>();
		}
	}
}
