using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.AdobeAnimateEditor.Inspector;

[Tool]
[ScriptPath("res://addons/AdobeAnimateEditor/Inspector/AdobeAnimateCpuPreviewCanvas.cs")]
public class AdobeAnimateCpuPreviewCanvas : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Notification = "_Notification";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName BindSource = "BindSource";

		public static readonly StringName SetPreviewActive = "SetPreviewActive";

		public static readonly StringName UpdateProcessingState = "UpdateProcessingState";

		public new static readonly StringName _Draw = "_Draw";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName HasRenderableSnapshot = "HasRenderableSnapshot";

		public static readonly StringName LastDrawItemCount = "LastDrawItemCount";

		public static readonly StringName _source = "_source";

		public static readonly StringName _previewActive = "_previewActive";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private readonly List<AdobeAnimateDrawItem> _drawItems = new List<AdobeAnimateDrawItem>();

	private readonly List<AdobeAnimateDrawItem> _boundsDrawItems = new List<AdobeAnimateDrawItem>();

	private readonly Dictionary<int, Texture2D> _arrayLayerTextures = new Dictionary<int, Texture2D>();

	private AdobeAnimateSprite _source;

	private bool _previewActive = true;

	public bool HasRenderableSnapshot { get; private set; }

	public int LastDrawItemCount { get; private set; }

	public override void _Ready()
	{
		UpdateProcessingState();
	}

	public override void _Notification(int what)
	{
		base._Notification(what);
		if ((long)what == 31 && IsNodeReady())
		{
			UpdateProcessingState();
		}
	}

	public override void _Process(double delta)
	{
		if (GodotObject.IsInstanceValid(_source))
		{
			QueueRedraw();
		}
	}

	public void BindSource(AdobeAnimateSprite source)
	{
		if (_source != source)
		{
			_arrayLayerTextures.Clear();
		}
		_source = (GodotObject.IsInstanceValid(source) ? source : null);
		UpdateProcessingState();
		QueueRedraw();
	}

	public void SetPreviewActive(bool active)
	{
		if (_previewActive != active)
		{
			_previewActive = active;
			UpdateProcessingState();
		}
	}

	public bool TryGetLocalRenderBounds(out Rect2 bounds)
	{
		bounds = default;
		if (!GodotObject.IsInstanceValid(_source) || !_source.TryBuildRenderSnapshot(out var snapshot) || snapshot.Definition == null)
		{
			return false;
		}
		_boundsDrawItems.Clear();
		AdobeAnimateDrawItemBuilder.Build(snapshot, _boundsDrawItems);
		if (_boundsDrawItems.Count == 0)
		{
			return false;
		}
		Transform2D transform2D = snapshot.GlobalTransform.AffineInverse();
		bool hasPoint = false;
		Vector2 min = Vector2.Zero;
		Vector2 max = Vector2.Zero;
		for (int i = 0; i < _boundsDrawItems.Count; i++)
		{
			AdobeAnimateDrawItem adobeAnimateDrawItem = _boundsDrawItems[i];
			if (!(adobeAnimateDrawItem.Color.A <= 0.001f))
			{
				Transform2D transform2D2 = transform2D * adobeAnimateDrawItem.Transform;
				IncludeBoundsPoint(ref hasPoint, ref min, ref max, transform2D2 * Vector2.Zero);
				IncludeBoundsPoint(ref hasPoint, ref min, ref max, transform2D2 * Vector2.Right);
				IncludeBoundsPoint(ref hasPoint, ref min, ref max, transform2D2 * Vector2.Down);
				IncludeBoundsPoint(ref hasPoint, ref min, ref max, transform2D2 * Vector2.One);
			}
		}
		if (!hasPoint)
		{
			return false;
		}
		Vector2 size = max - min;
		if (size.X <= 0.001f || size.Y <= 0.001f)
		{
			return false;
		}
		bounds = new Rect2(min, size);
		return true;
	}

	private static void IncludeBoundsPoint(ref bool hasPoint, ref Vector2 min, ref Vector2 max, Vector2 point)
	{
		if (float.IsFinite(point.X) && float.IsFinite(point.Y))
		{
			if (!hasPoint)
			{
				min = point;
				max = point;
				hasPoint = true;
			}
			else
			{
				min = new Vector2(Mathf.Min(min.X, point.X), Mathf.Min(min.Y, point.Y));
				max = new Vector2(Mathf.Max(max.X, point.X), Mathf.Max(max.Y, point.Y));
			}
		}
	}

	private void UpdateProcessingState()
	{
		SetProcess(_previewActive && IsVisibleInTree() && GodotObject.IsInstanceValid(_source));
	}

	public override void _Draw()
	{
		HasRenderableSnapshot = false;
		LastDrawItemCount = 0;
		if (!GodotObject.IsInstanceValid(_source) || !_source.TryBuildRenderSnapshot(out var snapshot) || snapshot.Definition == null)
		{
			return;
		}
		_drawItems.Clear();
		AdobeAnimateDrawItemBuilder.Build(snapshot, _drawItems);
		if (snapshot.NeedsDrawItemSort)
		{
			_drawItems.Sort(AdobeAnimateDrawItemComparer.Instance);
		}
		LastDrawItemCount = _drawItems.Count;
		HasRenderableSnapshot = _drawItems.Count > 0;
		for (int i = 0; i < _drawItems.Count; i++)
		{
			AdobeAnimateDrawItem adobeAnimateDrawItem = _drawItems[i];
			Texture2D texture2D = ResolveAtlas(snapshot.Definition, adobeAnimateDrawItem.AtlasLayer);
			if (GodotObject.IsInstanceValid(texture2D) && !(adobeAnimateDrawItem.Color.A <= 0.001f))
			{
				Vector2 vector = ResolveAtlasSize(snapshot.Definition, texture2D);
				Rect2 srcRect = new Rect2(adobeAnimateDrawItem.UvRect.Position * vector, adobeAnimateDrawItem.UvRect.Size * vector);
				if (!(srcRect.Size.X <= 0.001f) && !(srcRect.Size.Y <= 0.001f))
				{
					DrawSetTransformMatrix(adobeAnimateDrawItem.Transform);
					DrawTextureRectRegion(texture2D, new Rect2(Vector2.Zero, Vector2.One), srcRect, adobeAnimateDrawItem.Color);
				}
			}
		}
		DrawSetTransformMatrix(Transform2D.Identity);
	}

	private Texture2D ResolveAtlas(AdobeAnimateRuntimeDefinition definition, int layer)
	{
		if (definition?.AtlasPages != null && layer >= 0 && layer < definition.AtlasPages.Length && GodotObject.IsInstanceValid(definition.AtlasPages[layer]))
		{
			return definition.AtlasPages[layer];
		}
		if (definition != null && definition.UsesAtlasTextureArrayLayout && GodotObject.IsInstanceValid(definition.AtlasTextureArray))
		{
			if (_arrayLayerTextures.TryGetValue(layer, out var value) && GodotObject.IsInstanceValid(value))
			{
				return value;
			}
			try
			{
				Image layerData = definition.AtlasTextureArray.GetLayerData(layer);
				if (GodotObject.IsInstanceValid(layerData) && !layerData.IsEmpty())
				{
					ImageTexture imageTexture = ImageTexture.CreateFromImage(layerData);
					_arrayLayerTextures[layer] = imageTexture;
					return imageTexture;
				}
			}
			catch
			{
			}
		}
		return definition?.BaseAtlas;
	}

	private static Vector2 ResolveAtlasSize(AdobeAnimateRuntimeDefinition definition, Texture2D atlas)
	{
		if (definition != null && definition.UsesAtlasTextureArrayLayout && definition.AtlasTextureArraySize.X > 0f && definition.AtlasTextureArraySize.Y > 0f)
		{
			return definition.AtlasTextureArraySize;
		}
		if (!GodotObject.IsInstanceValid(atlas))
		{
			return Vector2.One;
		}
		return atlas.GetSize();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindSource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetPreviewActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateProcessingState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindSource && args.Count == 1)
		{
			BindSource(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPreviewActive && args.Count == 1)
		{
			SetPreviewActive(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateProcessingState && args.Count == 0)
		{
			UpdateProcessingState();
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
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
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.BindSource)
		{
			return true;
		}
		if (method == MethodName.SetPreviewActive)
		{
			return true;
		}
		if (method == MethodName.UpdateProcessingState)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.HasRenderableSnapshot)
		{
			HasRenderableSnapshot = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.LastDrawItemCount)
		{
			LastDrawItemCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._source)
		{
			_source = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._previewActive)
		{
			_previewActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.HasRenderableSnapshot)
		{
			value = VariantUtils.CreateFrom<bool>(HasRenderableSnapshot);
			return true;
		}
		if (name == PropertyName.LastDrawItemCount)
		{
			value = VariantUtils.CreateFrom<int>(LastDrawItemCount);
			return true;
		}
		if (name == PropertyName._source)
		{
			value = VariantUtils.CreateFrom(in _source);
			return true;
		}
		if (name == PropertyName._previewActive)
		{
			value = VariantUtils.CreateFrom(in _previewActive);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._source, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._previewActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasRenderableSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LastDrawItemCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.HasRenderableSnapshot, Variant.From<bool>(HasRenderableSnapshot));
		info.AddProperty(PropertyName.LastDrawItemCount, Variant.From<int>(LastDrawItemCount));
		info.AddProperty(PropertyName._source, Variant.From(in _source));
		info.AddProperty(PropertyName._previewActive, Variant.From(in _previewActive));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.HasRenderableSnapshot, out var value))
		{
			HasRenderableSnapshot = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.LastDrawItemCount, out var value2))
		{
			LastDrawItemCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._source, out var value3))
		{
			_source = value3.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._previewActive, out var value4))
		{
			_previewActive = value4.As<bool>();
		}
	}
}
