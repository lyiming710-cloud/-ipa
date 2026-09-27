using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://addons/AdobeAnimateEditor/Node/AdobeAnimatePart.cs")]
public class AdobeAnimatePart : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName CreateAtlasTexturePart = "CreateAtlasTexturePart";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName NotifyManagedSlotVisualChanged = "NotifyManagedSlotVisualChanged";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName PrepareRenderMaterial = "PrepareRenderMaterial";

		public static readonly StringName DrawExternalAtlasTexture = "DrawExternalAtlasTexture";

		public static readonly StringName DrawReplacementPrimitive = "DrawReplacementPrimitive";

		public static readonly StringName DrawQuad = "DrawQuad";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName flashAnimeData = "flashAnimeData";

		public static readonly StringName offset = "offset";

		public static readonly StringName elementList = "elementList";

		public static readonly StringName mediaReplace = "mediaReplace";

		public static readonly StringName mediaReplaceAtlasPaths = "mediaReplaceAtlasPaths";

		public static readonly StringName externalAtlasTexturePath = "externalAtlasTexturePath";

		public static readonly StringName externalAtlasCentered = "externalAtlasCentered";

		public static readonly StringName _flashAnimeData = "_flashAnimeData";

		public static readonly StringName _offset = "_offset";

		public static readonly StringName _elementList = "_elementList";

		public static readonly StringName _mediaReplace = "_mediaReplace";

		public static readonly StringName _mediaReplaceAtlasPaths = "_mediaReplaceAtlasPaths";

		public static readonly StringName _externalAtlasTexturePath = "_externalAtlasTexturePath";

		public static readonly StringName _externalAtlasAllocationResolved = "_externalAtlasAllocationResolved";

		public static readonly StringName _externalAtlasCentered = "_externalAtlasCentered";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const string ArrayShaderPath = "res://addons/AdobeAnimateEditor/Rendering/AdobeAnimatePart.gdshader";

	private static Shader _arrayShader;

	private static ShaderMaterial _sharedArrayMaterial;

	private static ulong _sharedArrayTextureId;

	private AdobeAnimateData _flashAnimeData;

	private Vector2 _offset = Vector2.Zero;

	private Array<Godot.Collections.Array> _elementList = new Array<Godot.Collections.Array>();

	private Array<Texture2D> _mediaReplace = new Array<Texture2D>();

	private Array<string> _mediaReplaceAtlasPaths = new Array<string>();

	private string _externalAtlasTexturePath = string.Empty;

	private AdobeAnimateExternalTextureAtlasAllocation _externalAtlasAllocation;

	private bool _externalAtlasAllocationResolved;

	private bool _externalAtlasCentered = true;

	[Export(PropertyHint.None, "")]
	public AdobeAnimateData flashAnimeData
	{
		get
		{
			return _flashAnimeData;
		}
		set
		{
			_flashAnimeData = value;
			PrepareRenderMaterial();
			QueueRedraw();
		}
	}

	[Export(PropertyHint.None, "")]
	public Vector2 offset
	{
		get
		{
			return _offset;
		}
		set
		{
			_offset = value;
			QueueRedraw();
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<Godot.Collections.Array> elementList
	{
		get
		{
			return _elementList;
		}
		set
		{
			_elementList = value;
			QueueRedraw();
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<Texture2D> mediaReplace
	{
		get
		{
			return _mediaReplace;
		}
		set
		{
			_mediaReplace = value;
			QueueRedraw();
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<string> mediaReplaceAtlasPaths
	{
		get
		{
			return _mediaReplaceAtlasPaths;
		}
		set
		{
			_mediaReplaceAtlasPaths = value ?? new Array<string>();
			QueueRedraw();
		}
	}

	[Export(PropertyHint.File, "*.png,*.webp,*.jpg,*.jpeg,*.svg,*.bmp,*.tga")]
	public string externalAtlasTexturePath
	{
		get
		{
			return _externalAtlasTexturePath;
		}
		set
		{
			string text = value ?? string.Empty;
			if (!string.Equals(_externalAtlasTexturePath, text, StringComparison.Ordinal))
			{
				bool topologyChanged = string.IsNullOrWhiteSpace(_externalAtlasTexturePath) != string.IsNullOrWhiteSpace(text);
				_externalAtlasTexturePath = text;
				_externalAtlasAllocation = default;
				_externalAtlasAllocationResolved = false;
				PrepareRenderMaterial();
				QueueRedraw();
				NotifyManagedSlotVisualChanged(topologyChanged);
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public bool externalAtlasCentered
	{
		get
		{
			return _externalAtlasCentered;
		}
		set
		{
			if (_externalAtlasCentered != value)
			{
				_externalAtlasCentered = value;
				QueueRedraw();
				NotifyManagedSlotVisualChanged(topologyChanged: false);
			}
		}
	}

	public static AdobeAnimatePart CreateAtlasTexturePart(string texturePath, bool centered = true)
	{
		if (string.IsNullOrWhiteSpace(texturePath))
		{
			return null;
		}
		return new AdobeAnimatePart
		{
			externalAtlasTexturePath = texturePath,
			externalAtlasCentered = centered
		};
	}

	public override void _Ready()
	{
		PrepareRenderMaterial();
		QueueRedraw();
	}

	private void NotifyManagedSlotVisualChanged(bool topologyChanged)
	{
		if (GetParent() is AdobeAnimateSlot adobeAnimateSlot && GodotObject.IsInstanceValid(adobeAnimateSlot))
		{
			if (topologyChanged)
			{
				adobeAnimateSlot.MarkManagedSpriteChildrenDirty();
				adobeAnimateSlot.RefreshRuntimeUpdateRequirement(refreshVisibilityWatchers: true);
			}
			else if (GodotObject.IsInstanceValid(adobeAnimateSlot.sprite))
			{
				adobeAnimateSlot.sprite.MarkManagedSlotVisualStateChanged();
			}
		}
	}

	public override void _Draw()
	{
		if (!string.IsNullOrWhiteSpace(_externalAtlasTexturePath))
		{
			DrawExternalAtlasTexture();
		}
		else
		{
			if (_flashAnimeData == null)
			{
				return;
			}
			AdobeAnimateRuntimeDefinition orBuild = AdobeAnimateDefinitionCache.GetOrBuild(_flashAnimeData);
			if (orBuild == null)
			{
				return;
			}
			bool flag = orBuild.UsesAtlasTextureArrayLayout && GodotObject.IsInstanceValid(orBuild.AtlasTextureArray) && orBuild.AtlasTextureArrayRid.IsValid && orBuild.AtlasTextureArraySize.X > 0f && orBuild.AtlasTextureArraySize.Y > 0f;
			if (!flag && !GodotObject.IsInstanceValid(orBuild.BaseAtlas))
			{
				return;
			}
			if (flag)
			{
				PrepareRenderMaterial(orBuild);
			}
			Rect2[] mediaRects = orBuild.MediaRects;
			int count = _mediaReplace.Count;
			int count2 = _elementList.Count;
			for (int i = 0; i < count2; i++)
			{
				Godot.Collections.Array array = _elementList[i];
				if (array.Count < 3)
				{
					continue;
				}
				int num = (int)array[0];
				if (num == 65535 || num < 0 || num >= mediaRects.Length)
				{
					continue;
				}
				Transform2D xform = ((Transform2D)array[1]).Translated(-_offset);
				Color color = (Color)array[2];
				DrawSetTransformMatrix(xform);
				Rect2 rect = mediaRects[num];
				Texture2D texture2D = ((num < count) ? _mediaReplace[num] : null);
				string text = ((num < _mediaReplaceAtlasPaths.Count) ? _mediaReplaceAtlasPaths[num] : string.Empty);
				if (flag)
				{
					AdobeAnimateExternalTextureAtlasAllocation allocation;
					if (GodotObject.IsInstanceValid(texture2D))
					{
						DrawReplacementPrimitive(texture2D, rect, color);
					}
					else if (!string.IsNullOrEmpty(text) && AdobeAnimateGlobalAtlasCache.TryGetReplaceTextureAllocation(text, out allocation) && allocation.UsesTextureArray && allocation.TextureArrayRid == orBuild.AtlasTextureArrayRid)
					{
						DrawExternalAtlasPrimitive(allocation, color, centered: false);
					}
					else
					{
						DrawTextureArrayPrimitive(orBuild, num, rect, color);
					}
				}
				else if (texture2D == null)
				{
					Texture2D texture2D2 = ResolveAtlasTexture(orBuild, num);
					if (texture2D2 != null)
					{
						DrawTextureRectRegion(texture2D2, new Rect2(Vector2.Zero, rect.Size), rect, color);
					}
				}
				else
				{
					Vector2 size = texture2D.GetSize();
					Vector2 size2 = ((size.X > 0f && size.Y > 0f) ? size : rect.Size);
					DrawTextureRectRegion(texture2D, new Rect2(Vector2.Zero, size2), new Rect2(Vector2.Zero, size2), color);
				}
			}
		}
	}

	private void PrepareRenderMaterial()
	{
		if (!string.IsNullOrWhiteSpace(_externalAtlasTexturePath))
		{
			if (TryResolveExternalAtlasAllocation(out var allocation))
			{
				PrepareRenderMaterial(allocation.TextureArray);
			}
		}
		else if (_flashAnimeData != null)
		{
			PrepareRenderMaterial(AdobeAnimateDefinitionCache.GetOrBuild(_flashAnimeData));
		}
	}

	private void PrepareRenderMaterial(AdobeAnimateRuntimeDefinition definition)
	{
		if (definition == null || !definition.UsesAtlasTextureArrayLayout || !GodotObject.IsInstanceValid(definition.AtlasTextureArray))
		{
			Material = null;
		}
		else
		{
			PrepareRenderMaterial(definition.AtlasTextureArray);
		}
	}

	private void PrepareRenderMaterial(TextureLayered textureArray)
	{
		if (!GodotObject.IsInstanceValid(textureArray))
		{
			Material = null;
			return;
		}
		ulong instanceId = textureArray.GetInstanceId();
		if (!GodotObject.IsInstanceValid(_sharedArrayMaterial) || _sharedArrayTextureId != instanceId)
		{
			if (_arrayShader == null)
			{
				_arrayShader = GD.Load<Shader>("res://addons/AdobeAnimateEditor/Rendering/AdobeAnimatePart.gdshader");
			}
			if (!GodotObject.IsInstanceValid(_arrayShader))
			{
				return;
			}
			_sharedArrayMaterial = new ShaderMaterial
			{
				Shader = _arrayShader
			};
			_sharedArrayMaterial.SetShaderParameter("atlas_texture_array", textureArray);
			_sharedArrayTextureId = instanceId;
		}
		Material = _sharedArrayMaterial;
	}

	private void DrawExternalAtlasTexture()
	{
		if (TryResolveExternalAtlasAllocation(out var allocation))
		{
			PrepareRenderMaterial(allocation.TextureArray);
			DrawSetTransformMatrix(Transform2D.Identity);
			DrawExternalAtlasPrimitive(allocation, Colors.White, externalAtlasCentered);
		}
	}

	private bool TryResolveExternalAtlasAllocation(out AdobeAnimateExternalTextureAtlasAllocation allocation)
	{
		if (_externalAtlasAllocationResolved)
		{
			allocation = _externalAtlasAllocation;
			if (allocation.UsesTextureArray && allocation.TextureArrayRid.IsValid)
			{
				return GodotObject.IsInstanceValid(allocation.TextureArray);
			}
			return false;
		}
		_externalAtlasAllocationResolved = true;
		if (!AdobeAnimateGlobalAtlasCache.TryGetReplaceTextureAllocation(_externalAtlasTexturePath, out allocation) || !allocation.UsesTextureArray || !allocation.TextureArrayRid.IsValid || !GodotObject.IsInstanceValid(allocation.TextureArray))
		{
			_externalAtlasAllocation = default;
			return false;
		}
		_externalAtlasAllocation = allocation;
		return true;
	}

	private void DrawExternalAtlasPrimitive(AdobeAnimateExternalTextureAtlasAllocation allocation, Color color, bool centered)
	{
		Rect2 rect = allocation.Rect;
		Vector2 textureArraySize = allocation.TextureArraySize;
		if (!(rect.Size.X <= 0f) && !(rect.Size.Y <= 0f) && !(textureArraySize.X <= 0f) && !(textureArraySize.Y <= 0f))
		{
			float x = rect.Position.X / textureArraySize.X;
			float num = rect.Position.Y / textureArraySize.Y;
			float x2 = (rect.Position.X + rect.Size.X) / textureArraySize.X;
			float num2 = Mathf.Min((rect.Position.Y + rect.Size.Y) / textureArraySize.Y, 0.999999f);
			DrawQuad(rect.Size, color, new Vector2[4]
			{
				new Vector2(x, (float)allocation.AtlasPage + num),
				new Vector2(x, (float)allocation.AtlasPage + num2),
				new Vector2(x2, (float)allocation.AtlasPage + num2),
				new Vector2(x2, (float)allocation.AtlasPage + num)
			}, null, centered);
		}
	}

	private void DrawTextureArrayPrimitive(AdobeAnimateRuntimeDefinition definition, int mediaId, Rect2 mediaRect, Color color)
	{
		Vector2 atlasTextureArraySize = definition.AtlasTextureArraySize;
		if (!(atlasTextureArraySize.X <= 0f) && !(atlasTextureArraySize.Y <= 0f) && !(mediaRect.Size.X <= 0f) && !(mediaRect.Size.Y <= 0f))
		{
			int num = definition.BaseAtlasPage;
			if (definition.MediaAtlasPages != null && mediaId >= 0 && mediaId < definition.MediaAtlasPages.Length)
			{
				num = definition.MediaAtlasPages[mediaId];
			}
			float x = mediaRect.Position.X / atlasTextureArraySize.X;
			float num2 = mediaRect.Position.Y / atlasTextureArraySize.Y;
			float x2 = (mediaRect.Position.X + mediaRect.Size.X) / atlasTextureArraySize.X;
			float a = (mediaRect.Position.Y + mediaRect.Size.Y) / atlasTextureArraySize.Y;
			a = Mathf.Min(a, 0.999999f);
			DrawQuad(mediaRect.Size, color, new Vector2[4]
			{
				new Vector2(x, (float)num + num2),
				new Vector2(x, (float)num + a),
				new Vector2(x2, (float)num + a),
				new Vector2(x2, (float)num + num2)
			});
		}
	}

	private void DrawReplacementPrimitive(Texture2D texture, Rect2 fallbackRect, Color color)
	{
		Vector2 size = texture.GetSize();
		if (size.X <= 0f || size.Y <= 0f)
		{
			size = fallbackRect.Size;
		}
		DrawQuad(size, color, new Vector2[4]
		{
			new Vector2(-1f, 0f),
			new Vector2(-1f, 1f),
			new Vector2(-2f, 1f),
			new Vector2(-2f, 0f)
		}, texture);
	}

	private void DrawQuad(Vector2 size, Color color, Vector2[] uvs, Texture2D texture = null, bool centered = false)
	{
		Vector2 vector = (centered ? (-size / 2f) : Vector2.Zero);
		Vector2[] points = new Vector2[4]
		{
			vector,
			vector + new Vector2(0f, size.Y),
			vector + size,
			vector + new Vector2(size.X, 0f)
		};
		Color[] colors = new Color[4] { color, color, color, color };
		DrawPrimitive(points, colors, uvs, texture);
	}

	private static Texture2D ResolveAtlasTexture(AdobeAnimateRuntimeDefinition definition, int mediaId)
	{
		int num = definition.BaseAtlasPage;
		if (definition.MediaAtlasPages != null && mediaId >= 0 && mediaId < definition.MediaAtlasPages.Length)
		{
			num = definition.MediaAtlasPages[mediaId];
		}
		if (definition.AtlasPages != null && num >= 0 && num < definition.AtlasPages.Length)
		{
			return definition.AtlasPages[num];
		}
		return definition.BaseAtlas;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.CreateAtlasTexturePart, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "texturePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "centered", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifyManagedSlotVisualChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "topologyChanged", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareRenderMaterial, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareRenderMaterial, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "textureArray", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false)
			}, null),
			new MethodInfo(MethodName.DrawExternalAtlasTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawReplacementPrimitive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false),
				new PropertyInfo(Variant.Type.Rect2, "fallbackRect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawQuad, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedVector2Array, "uvs", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "centered", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateAtlasTexturePart && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimatePart>(CreateAtlasTexturePart(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyManagedSlotVisualChanged && args.Count == 1)
		{
			NotifyManagedSlotVisualChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareRenderMaterial && args.Count == 0)
		{
			PrepareRenderMaterial();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareRenderMaterial && args.Count == 1)
		{
			PrepareRenderMaterial(VariantUtils.ConvertTo<TextureLayered>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawExternalAtlasTexture && args.Count == 0)
		{
			DrawExternalAtlasTexture();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawReplacementPrimitive && args.Count == 3)
		{
			DrawReplacementPrimitive(VariantUtils.ConvertTo<Texture2D>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawQuad && args.Count == 5)
		{
			DrawQuad(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Color>(in args[1]), VariantUtils.ConvertTo<Vector2[]>(in args[2]), VariantUtils.ConvertTo<Texture2D>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateAtlasTexturePart && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimatePart>(CreateAtlasTexturePart(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.CreateAtlasTexturePart)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.NotifyManagedSlotVisualChanged)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.PrepareRenderMaterial)
		{
			return true;
		}
		if (method == MethodName.DrawExternalAtlasTexture)
		{
			return true;
		}
		if (method == MethodName.DrawReplacementPrimitive)
		{
			return true;
		}
		if (method == MethodName.DrawQuad)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.flashAnimeData)
		{
			flashAnimeData = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName.offset)
		{
			offset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.elementList)
		{
			elementList = VariantUtils.ConvertToArray<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplace)
		{
			mediaReplace = VariantUtils.ConvertToArray<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasPaths)
		{
			mediaReplaceAtlasPaths = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.externalAtlasTexturePath)
		{
			externalAtlasTexturePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.externalAtlasCentered)
		{
			externalAtlasCentered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._flashAnimeData)
		{
			_flashAnimeData = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._offset)
		{
			_offset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._elementList)
		{
			_elementList = VariantUtils.ConvertToArray<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName._mediaReplace)
		{
			_mediaReplace = VariantUtils.ConvertToArray<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._mediaReplaceAtlasPaths)
		{
			_mediaReplaceAtlasPaths = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName._externalAtlasTexturePath)
		{
			_externalAtlasTexturePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._externalAtlasAllocationResolved)
		{
			_externalAtlasAllocationResolved = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._externalAtlasCentered)
		{
			_externalAtlasCentered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.flashAnimeData)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateData>(flashAnimeData);
			return true;
		}
		if (name == PropertyName.offset)
		{
			value = VariantUtils.CreateFrom<Vector2>(offset);
			return true;
		}
		if (name == PropertyName.elementList)
		{
			value = VariantUtils.CreateFromArray(elementList);
			return true;
		}
		if (name == PropertyName.mediaReplace)
		{
			value = VariantUtils.CreateFromArray(mediaReplace);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasPaths)
		{
			value = VariantUtils.CreateFromArray(mediaReplaceAtlasPaths);
			return true;
		}
		if (name == PropertyName.externalAtlasTexturePath)
		{
			value = VariantUtils.CreateFrom<string>(externalAtlasTexturePath);
			return true;
		}
		if (name == PropertyName.externalAtlasCentered)
		{
			value = VariantUtils.CreateFrom<bool>(externalAtlasCentered);
			return true;
		}
		if (name == PropertyName._flashAnimeData)
		{
			value = VariantUtils.CreateFrom(in _flashAnimeData);
			return true;
		}
		if (name == PropertyName._offset)
		{
			value = VariantUtils.CreateFrom(in _offset);
			return true;
		}
		if (name == PropertyName._elementList)
		{
			value = VariantUtils.CreateFromArray(_elementList);
			return true;
		}
		if (name == PropertyName._mediaReplace)
		{
			value = VariantUtils.CreateFromArray(_mediaReplace);
			return true;
		}
		if (name == PropertyName._mediaReplaceAtlasPaths)
		{
			value = VariantUtils.CreateFromArray(_mediaReplaceAtlasPaths);
			return true;
		}
		if (name == PropertyName._externalAtlasTexturePath)
		{
			value = VariantUtils.CreateFrom(in _externalAtlasTexturePath);
			return true;
		}
		if (name == PropertyName._externalAtlasAllocationResolved)
		{
			value = VariantUtils.CreateFrom(in _externalAtlasAllocationResolved);
			return true;
		}
		if (name == PropertyName._externalAtlasCentered)
		{
			value = VariantUtils.CreateFrom(in _externalAtlasCentered);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._flashAnimeData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._offset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._elementList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._mediaReplace, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._mediaReplaceAtlasPaths, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._externalAtlasTexturePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._externalAtlasAllocationResolved, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._externalAtlasCentered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.flashAnimeData, PropertyHint.ResourceType, "AdobeAnimateData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.offset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.elementList, PropertyHint.TypeString, "28/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.mediaReplace, PropertyHint.TypeString, "24/17:Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.mediaReplaceAtlasPaths, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.externalAtlasTexturePath, PropertyHint.File, "*.png,*.webp,*.jpg,*.jpeg,*.svg,*.bmp,*.tga", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.externalAtlasCentered, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.flashAnimeData, Variant.From<AdobeAnimateData>(flashAnimeData));
		info.AddProperty(PropertyName.offset, Variant.From<Vector2>(offset));
		info.AddProperty(PropertyName.elementList, Variant.CreateFrom(elementList));
		info.AddProperty(PropertyName.mediaReplace, Variant.CreateFrom(mediaReplace));
		info.AddProperty(PropertyName.mediaReplaceAtlasPaths, Variant.CreateFrom(mediaReplaceAtlasPaths));
		info.AddProperty(PropertyName.externalAtlasTexturePath, Variant.From<string>(externalAtlasTexturePath));
		info.AddProperty(PropertyName.externalAtlasCentered, Variant.From<bool>(externalAtlasCentered));
		info.AddProperty(PropertyName._flashAnimeData, Variant.From(in _flashAnimeData));
		info.AddProperty(PropertyName._offset, Variant.From(in _offset));
		info.AddProperty(PropertyName._elementList, Variant.CreateFrom(_elementList));
		info.AddProperty(PropertyName._mediaReplace, Variant.CreateFrom(_mediaReplace));
		info.AddProperty(PropertyName._mediaReplaceAtlasPaths, Variant.CreateFrom(_mediaReplaceAtlasPaths));
		info.AddProperty(PropertyName._externalAtlasTexturePath, Variant.From(in _externalAtlasTexturePath));
		info.AddProperty(PropertyName._externalAtlasAllocationResolved, Variant.From(in _externalAtlasAllocationResolved));
		info.AddProperty(PropertyName._externalAtlasCentered, Variant.From(in _externalAtlasCentered));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.flashAnimeData, out var value))
		{
			flashAnimeData = value.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName.offset, out var value2))
		{
			offset = value2.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.elementList, out var value3))
		{
			elementList = value3.AsGodotArray<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplace, out var value4))
		{
			mediaReplace = value4.AsGodotArray<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplaceAtlasPaths, out var value5))
		{
			mediaReplaceAtlasPaths = value5.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.externalAtlasTexturePath, out var value6))
		{
			externalAtlasTexturePath = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName.externalAtlasCentered, out var value7))
		{
			externalAtlasCentered = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._flashAnimeData, out var value8))
		{
			_flashAnimeData = value8.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._offset, out var value9))
		{
			_offset = value9.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._elementList, out var value10))
		{
			_elementList = value10.AsGodotArray<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName._mediaReplace, out var value11))
		{
			_mediaReplace = value11.AsGodotArray<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._mediaReplaceAtlasPaths, out var value12))
		{
			_mediaReplaceAtlasPaths = value12.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName._externalAtlasTexturePath, out var value13))
		{
			_externalAtlasTexturePath = value13.As<string>();
		}
		if (info.TryGetProperty(PropertyName._externalAtlasAllocationResolved, out var value14))
		{
			_externalAtlasAllocationResolved = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._externalAtlasCentered, out var value15))
		{
			_externalAtlasCentered = value15.As<bool>();
		}
	}
}
