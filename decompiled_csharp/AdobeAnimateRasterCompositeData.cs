using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://addons/AdobeAnimateEditor/Resource/AdobeAnimateRasterCompositeData.cs")]
public class AdobeAnimateRasterCompositeData : Resource
{
	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName AllowsMediaReplaceMask = "AllowsMediaReplaceMask";

		public static readonly StringName ResolveVariant = "ResolveVariant";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName atlas = "atlas";

		public static readonly StringName tileSize = "tileSize";

		public static readonly StringName origin = "origin";

		public static readonly StringName columns = "columns";

		public static readonly StringName variantFrameStride = "variantFrameStride";

		public static readonly StringName layerVisibilitySignatures = "layerVisibilitySignatures";

		public static readonly StringName ignoredMediaReplaceMask = "ignoredMediaReplaceMask";

		public static readonly StringName clipTiles = "clipTiles";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	[Export(PropertyHint.None, "")]
	public Texture2D atlas { get; set; }

	[Export(PropertyHint.None, "")]
	public Vector2I tileSize { get; set; }

	[Export(PropertyHint.None, "")]
	public Vector2 origin { get; set; }

	[Export(PropertyHint.None, "")]
	public int columns { get; set; } = 1;

	[Export(PropertyHint.None, "")]
	public int variantFrameStride { get; set; }

	[Export(PropertyHint.None, "")]
	public long[] layerVisibilitySignatures { get; set; } = System.Array.Empty<long>();

	[Export(PropertyHint.None, "")]
	public long ignoredMediaReplaceMask { get; set; }

	[Export(PropertyHint.None, "")]
	public Godot.Collections.Dictionary<string, Vector3I> clipTiles { get; set; } = new Godot.Collections.Dictionary<string, Vector3I>();

	public bool AllowsMediaReplaceMask(ulong activeMask, bool hasOverflow)
	{
		if (hasOverflow)
		{
			return false;
		}
		ulong num = (ulong)ignoredMediaReplaceMask;
		return (activeMask & ~num) == 0;
	}

	public bool TryResolveTile(string clip, Vector2I clipRange, float frameFloat, ulong layerVisibilitySignature, out int tileIndex)
	{
		tileIndex = 0;
		if (!TryResolveSequence(clip, clipRange, layerVisibilitySignature, out var tileBase, out var clipStart, out var frameCount))
		{
			return false;
		}
		int num = Math.Clamp(Mathf.FloorToInt(frameFloat) - clipStart, 0, frameCount - 1);
		tileIndex = tileBase + num;
		return tileIndex >= 0;
	}

	public bool TryResolveSequence(string clip, Vector2I clipRange, ulong layerVisibilitySignature, out int tileBase, out int clipStart, out int frameCount)
	{
		tileBase = 0;
		clipStart = 0;
		frameCount = 0;
		if (!GodotObject.IsInstanceValid(atlas) || tileSize.X <= 0 || tileSize.Y <= 0 || columns <= 0 || variantFrameStride <= 0 || string.IsNullOrEmpty(clip) || clipTiles == null || !clipTiles.TryGetValue(clip, out var value))
		{
			return false;
		}
		clipStart = value.X;
		frameCount = value.Y;
		int z = value.Z;
		if (frameCount <= 0 || clipRange.X != clipStart || clipRange.Y != clipStart + frameCount)
		{
			return false;
		}
		int num = ResolveVariant(layerVisibilitySignature);
		if (num < 0)
		{
			return false;
		}
		tileBase = num * variantFrameStride + z;
		return tileBase >= 0;
	}

	private int ResolveVariant(ulong signature)
	{
		long[] array = layerVisibilitySignatures;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == (long)signature)
			{
				return i;
			}
		}
		return -1;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName.AllowsMediaReplaceMask, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "activeMask", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hasOverflow", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveVariant, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "signature", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AllowsMediaReplaceMask && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AllowsMediaReplaceMask(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolveVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveVariant(VariantUtils.ConvertTo<ulong>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.AllowsMediaReplaceMask)
		{
			return true;
		}
		if (method == MethodName.ResolveVariant)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.atlas)
		{
			atlas = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.tileSize)
		{
			tileSize = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.origin)
		{
			origin = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.columns)
		{
			columns = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.variantFrameStride)
		{
			variantFrameStride = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.layerVisibilitySignatures)
		{
			layerVisibilitySignatures = VariantUtils.ConvertTo<long[]>(in value);
			return true;
		}
		if (name == PropertyName.ignoredMediaReplaceMask)
		{
			ignoredMediaReplaceMask = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName.clipTiles)
		{
			clipTiles = VariantUtils.ConvertToDictionary<string, Vector3I>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.atlas)
		{
			value = VariantUtils.CreateFrom<Texture2D>(atlas);
			return true;
		}
		if (name == PropertyName.tileSize)
		{
			value = VariantUtils.CreateFrom<Vector2I>(tileSize);
			return true;
		}
		if (name == PropertyName.origin)
		{
			value = VariantUtils.CreateFrom<Vector2>(origin);
			return true;
		}
		int from;
		if (name == PropertyName.columns)
		{
			from = columns;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.variantFrameStride)
		{
			from = variantFrameStride;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.layerVisibilitySignatures)
		{
			value = VariantUtils.CreateFrom<long[]>(layerVisibilitySignatures);
			return true;
		}
		if (name == PropertyName.ignoredMediaReplaceMask)
		{
			value = VariantUtils.CreateFrom<long>(ignoredMediaReplaceMask);
			return true;
		}
		if (name == PropertyName.clipTiles)
		{
			value = VariantUtils.CreateFromDictionary(clipTiles);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.atlas, PropertyHint.ResourceType, "Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.tileSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.origin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.columns, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.variantFrameStride, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedInt64Array, PropertyName.layerVisibilitySignatures, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ignoredMediaReplaceMask, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.clipTiles, PropertyHint.TypeString, "4/0:;10/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.atlas, Variant.From<Texture2D>(atlas));
		info.AddProperty(PropertyName.tileSize, Variant.From<Vector2I>(tileSize));
		info.AddProperty(PropertyName.origin, Variant.From<Vector2>(origin));
		info.AddProperty(PropertyName.columns, Variant.From<int>(columns));
		info.AddProperty(PropertyName.variantFrameStride, Variant.From<int>(variantFrameStride));
		info.AddProperty(PropertyName.layerVisibilitySignatures, Variant.From<long[]>(layerVisibilitySignatures));
		info.AddProperty(PropertyName.ignoredMediaReplaceMask, Variant.From<long>(ignoredMediaReplaceMask));
		info.AddProperty(PropertyName.clipTiles, Variant.CreateFrom(clipTiles));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.atlas, out var value))
		{
			atlas = value.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.tileSize, out var value2))
		{
			tileSize = value2.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.origin, out var value3))
		{
			origin = value3.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.columns, out var value4))
		{
			columns = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.variantFrameStride, out var value5))
		{
			variantFrameStride = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.layerVisibilitySignatures, out var value6))
		{
			layerVisibilitySignatures = value6.As<long[]>();
		}
		if (info.TryGetProperty(PropertyName.ignoredMediaReplaceMask, out var value7))
		{
			ignoredMediaReplaceMask = value7.As<long>();
		}
		if (info.TryGetProperty(PropertyName.clipTiles, out var value8))
		{
			clipTiles = value8.AsGodotDictionary<string, Vector3I>();
		}
	}
}
