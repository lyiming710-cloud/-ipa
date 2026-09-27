using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://addons/AdobeAnimateEditor/Node/AdobeAnimateSlot.cs")]
public class AdobeAnimateSlot : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName RefreshRuntimeUpdateRequirement = "RefreshRuntimeUpdateRequirement";

		public static readonly StringName ApplyRuntimeFollowVisibility = "ApplyRuntimeFollowVisibility";

		public static readonly StringName ResetRuntimeFollowVisibilityCache = "ResetRuntimeFollowVisibilityCache";

		public static readonly StringName RefreshRuntimeDependencyVisibilityWatchers = "RefreshRuntimeDependencyVisibilityWatchers";

		public static readonly StringName CollectRuntimeDependencyVisibilityWatchers = "CollectRuntimeDependencyVisibilityWatchers";

		public static readonly StringName ConnectRuntimeDependencyVisibilityWatcher = "ConnectRuntimeDependencyVisibilityWatcher";

		public static readonly StringName DisconnectRuntimeDependencyVisibilityWatchers = "DisconnectRuntimeDependencyVisibilityWatchers";

		public static readonly StringName OnRuntimeDependencyVisibilityChanged = "OnRuntimeDependencyVisibilityChanged";

		public static readonly StringName MarkManagedSpriteChildrenDirty = "MarkManagedSpriteChildrenDirty";

		public static readonly StringName ResolveDrawLayerId = "ResolveDrawLayerId";

		public static readonly StringName ApplyRuntimeTransform = "ApplyRuntimeTransform";

		public static readonly StringName AddRuntimeVisualOffset = "AddRuntimeVisualOffset";

		public static readonly StringName SetRuntimeVisualOffset = "SetRuntimeVisualOffset";

		public static readonly StringName CommitRuntimeTransform = "CommitRuntimeTransform";

		public static readonly StringName Update = "Update";

		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";

		public static readonly StringName CreatePart = "CreatePart";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName sprite = "sprite";

		public static readonly StringName mode = "mode";

		public static readonly StringName followSlotId = "followSlotId";

		public static readonly StringName drawLayerId = "drawLayerId";

		public static readonly StringName offset = "offset";

		public static readonly StringName useRotate = "useRotate";

		public static readonly StringName useScale = "useScale";

		public static readonly StringName useSkew = "useSkew";

		public static readonly StringName useFollowVisible = "useFollowVisible";

		public static readonly StringName updateAllFrame = "updateAllFrame";

		public static readonly StringName RequiresRuntimeUpdate = "RequiresRuntimeUpdate";

		public static readonly StringName RuntimeUpdateVisualOnly = "RuntimeUpdateVisualOnly";

		public static readonly StringName IsRuntimeInsideTree = "IsRuntimeInsideTree";

		public static readonly StringName CachedPosition = "CachedPosition";

		public static readonly StringName RuntimePositionVersion = "RuntimePositionVersion";

		public static readonly StringName _runtimeInsideTree = "_runtimeInsideTree";

		public static readonly StringName _runtimeFollowVisibilityKnown = "_runtimeFollowVisibilityKnown";

		public static readonly StringName _runtimeFollowVisibilityValue = "_runtimeFollowVisibilityValue";

		public static readonly StringName _mode = "_mode";

		public static readonly StringName _followSlotId = "_followSlotId";

		public static readonly StringName _drawLayerId = "_drawLayerId";

		public static readonly StringName _updateAllFrame = "_updateAllFrame";

		public static readonly StringName _runtimeBaseTransform = "_runtimeBaseTransform";

		public static readonly StringName _runtimeVisualOffset = "_runtimeVisualOffset";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	public const int EmptyMediaId = 65535;

	public const int TopDrawLayerId = -2;

	private const int ChildOrderChangedNotification = 24;

	private List<CanvasItem> _runtimeDependencyVisibilityWatchers;

	private bool _runtimeInsideTree;

	private bool _runtimeFollowVisibilityKnown;

	private bool _runtimeFollowVisibilityValue;

	private HashSet<int> _partSnapshotLayerIds;

	private int _mode;

	private int _followSlotId;

	private int _drawLayerId = -1;

	private bool _updateAllFrame;

	private Transform2D _runtimeBaseTransform = Transform2D.Identity;

	private Vector2 _runtimeVisualOffset = Vector2.Zero;

	public AdobeAnimateSprite sprite { get; set; }

	[Export(PropertyHint.Enum, "Follow,Drive")]
	public int mode
	{
		get
		{
			return _mode;
		}
		set
		{
			if (_mode != value)
			{
				_mode = value;
				MarkManagedSpriteChildrenDirty();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int followSlotId
	{
		get
		{
			return _followSlotId;
		}
		set
		{
			if (_followSlotId != value)
			{
				_followSlotId = value;
				MarkManagedSpriteChildrenDirty();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public int drawLayerId
	{
		get
		{
			return _drawLayerId;
		}
		set
		{
			if (_drawLayerId != value)
			{
				_drawLayerId = value;
				MarkManagedSpriteChildrenDirty();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public Vector2 offset { get; set; } = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public bool useRotate { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool useScale { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool useSkew { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool useFollowVisible { get; set; }

	[Export(PropertyHint.None, "")]
	public bool updateAllFrame
	{
		get
		{
			return _updateAllFrame;
		}
		set
		{
			if (_updateAllFrame != value)
			{
				_updateAllFrame = value;
				RefreshRuntimeUpdateRequirement();
			}
		}
	}

	internal bool RequiresRuntimeUpdate { get; private set; }

	internal bool RuntimeUpdateVisualOnly { get; private set; }

	internal bool IsRuntimeInsideTree => _runtimeInsideTree;

	public Vector2 CachedPosition { get; private set; } = Vector2.Zero;

	public int RuntimePositionVersion { get; private set; }

	public override void _EnterTree()
	{
		_runtimeInsideTree = true;
	}

	public override void _Ready()
	{
		ResetRuntimeFollowVisibilityCache();
		if (GetParent() is AdobeAnimateSprite adobeAnimateSprite)
		{
			sprite = adobeAnimateSprite;
		}
		if (drawLayerId == -1)
		{
			drawLayerId = followSlotId;
		}
		_runtimeBaseTransform = Transform;
		CachedPosition = Position;
		RefreshRuntimeUpdateRequirement(refreshVisibilityWatchers: true);
	}

	public override void _ExitTree()
	{
		if (_runtimeVisualOffset != Vector2.Zero)
		{
			SetRuntimeVisualOffset(Vector2.Zero);
		}
		_runtimeInsideTree = false;
		ResetRuntimeFollowVisibilityCache();
		DisconnectRuntimeDependencyVisibilityWatchers();
	}

	public override void _Notification(int what)
	{
		if (what == 24)
		{
			RefreshRuntimeUpdateRequirement(refreshVisibilityWatchers: true);
			MarkManagedSpriteChildrenDirty();
		}
		else if ((long)what == 31)
		{
			OnRuntimeDependencyVisibilityChanged();
		}
	}

	internal void RefreshRuntimeUpdateRequirement(bool refreshVisibilityWatchers = false)
	{
		if (refreshVisibilityWatchers)
		{
			RefreshRuntimeDependencyVisibilityWatchers();
		}
		AnalyzeRuntimeDependentChildren(out var requiresRuntimeUpdate, out var visualOnly);
		if (RequiresRuntimeUpdate != requiresRuntimeUpdate || RuntimeUpdateVisualOnly != visualOnly)
		{
			RequiresRuntimeUpdate = requiresRuntimeUpdate;
			RuntimeUpdateVisualOnly = visualOnly;
			if (sprite != null && GodotObject.IsInstanceValid(sprite))
			{
				sprite.MarkSlotRuntimeUpdateCacheDirty();
			}
			if (GetParent() is AdobeAnimateSlot adobeAnimateSlot && GodotObject.IsInstanceValid(adobeAnimateSlot))
			{
				adobeAnimateSlot.RefreshRuntimeUpdateRequirement();
			}
		}
	}

	internal void ApplyRuntimeFollowVisibility(bool visible)
	{
		if (!_runtimeFollowVisibilityKnown || _runtimeFollowVisibilityValue != visible)
		{
			_runtimeFollowVisibilityKnown = true;
			_runtimeFollowVisibilityValue = visible;
			if (Visible != visible)
			{
				Visible = visible;
			}
		}
	}

	private void ResetRuntimeFollowVisibilityCache()
	{
		_runtimeFollowVisibilityKnown = false;
	}

	private void AnalyzeRuntimeDependentChildren(out bool requiresRuntimeUpdate, out bool visualOnly)
	{
		requiresRuntimeUpdate = false;
		visualOnly = true;
		int childCount = GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			Node child = GetChild(i);
			if (ChildRequiresRuntimeUpdate(child, out var visualOnly2))
			{
				requiresRuntimeUpdate = true;
				if (!visualOnly2)
				{
					visualOnly = false;
				}
			}
		}
		visualOnly = requiresRuntimeUpdate & visualOnly;
	}

	private bool ChildRequiresRuntimeUpdate(Node child, out bool visualOnly)
	{
		visualOnly = true;
		if (child == null || !GodotObject.IsInstanceValid(child))
		{
			return false;
		}
		if (child is Sprite2D)
		{
			return false;
		}
		if (child is AdobeAnimatePart adobeAnimatePart && !string.IsNullOrWhiteSpace(adobeAnimatePart.externalAtlasTexturePath))
		{
			return false;
		}
		if (child is AdobeAnimateSprite adobeAnimateSprite)
		{
			return adobeAnimateSprite.Visible;
		}
		if (child is AdobeAnimateSlot adobeAnimateSlot)
		{
			adobeAnimateSlot.RefreshRuntimeUpdateRequirement();
			if (!adobeAnimateSlot.Visible && !adobeAnimateSlot.useFollowVisible)
			{
				return false;
			}
			visualOnly = adobeAnimateSlot.RuntimeUpdateVisualOnly;
			return adobeAnimateSlot.RequiresRuntimeUpdate;
		}
		visualOnly = false;
		return true;
	}

	private void RefreshRuntimeDependencyVisibilityWatchers()
	{
		DisconnectRuntimeDependencyVisibilityWatchers();
		CollectRuntimeDependencyVisibilityWatchers(this);
	}

	private void CollectRuntimeDependencyVisibilityWatchers(Node parent)
	{
		int childCount = parent.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			Node child = parent.GetChild(i);
			if (child is CanvasItem canvasItem && !(child is Sprite2D))
			{
				ConnectRuntimeDependencyVisibilityWatcher(canvasItem);
			}
			if (!(child is Sprite2D) && child.GetChildCount() > 0)
			{
				CollectRuntimeDependencyVisibilityWatchers(child);
			}
		}
	}

	private void ConnectRuntimeDependencyVisibilityWatcher(CanvasItem canvasItem)
	{
		if (canvasItem == null || !GodotObject.IsInstanceValid(canvasItem))
		{
			return;
		}
		List<CanvasItem> runtimeDependencyVisibilityWatchers = _runtimeDependencyVisibilityWatchers;
		if (runtimeDependencyVisibilityWatchers == null || !runtimeDependencyVisibilityWatchers.Contains(canvasItem))
		{
			canvasItem.VisibilityChanged += OnRuntimeDependencyVisibilityChanged;
			if (_runtimeDependencyVisibilityWatchers == null)
			{
				_runtimeDependencyVisibilityWatchers = new List<CanvasItem>();
			}
			_runtimeDependencyVisibilityWatchers.Add(canvasItem);
		}
	}

	private void DisconnectRuntimeDependencyVisibilityWatchers()
	{
		if (_runtimeDependencyVisibilityWatchers == null)
		{
			return;
		}
		for (int i = 0; i < _runtimeDependencyVisibilityWatchers.Count; i++)
		{
			CanvasItem canvasItem = _runtimeDependencyVisibilityWatchers[i];
			if (GodotObject.IsInstanceValid(canvasItem))
			{
				canvasItem.VisibilityChanged -= OnRuntimeDependencyVisibilityChanged;
			}
		}
		_runtimeDependencyVisibilityWatchers.Clear();
	}

	private void OnRuntimeDependencyVisibilityChanged()
	{
		if (_runtimeFollowVisibilityKnown && Visible != _runtimeFollowVisibilityValue)
		{
			Visible = _runtimeFollowVisibilityValue;
			return;
		}
		RefreshRuntimeUpdateRequirement();
		if (sprite != null && GodotObject.IsInstanceValid(sprite))
		{
			sprite.MarkManagedSlotVisualStateChanged();
		}
	}

	internal void MarkManagedSpriteChildrenDirty()
	{
		if (sprite != null && GodotObject.IsInstanceValid(sprite))
		{
			sprite.MarkManagedSlotSpriteCacheDirty();
		}
	}

	internal bool TryCollectManagedSprite2DChildren(List<AdobeAnimateManagedSlotSprite> output)
	{
		bool result = false;
		int childCount = GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			Node child = GetChild(i);
			if (child is Sprite2D sprite2D && GodotObject.IsInstanceValid(sprite2D))
			{
				AdobeAnimateManagedSprite2D.MarkManaged(sprite2D);
				output.Add(new AdobeAnimateManagedSlotSprite(this, sprite2D));
				result = true;
			}
			else if (child is AdobeAnimatePart adobeAnimatePart && GodotObject.IsInstanceValid(adobeAnimatePart) && !string.IsNullOrWhiteSpace(adobeAnimatePart.externalAtlasTexturePath))
			{
				AdobeAnimateManagedSprite2D.MarkManaged(adobeAnimatePart);
				output.Add(new AdobeAnimateManagedSlotSprite(this, adobeAnimatePart));
				result = true;
			}
		}
		return result;
	}

	internal int ResolveDrawLayerId()
	{
		if (drawLayerId == -2)
		{
			int num = (GodotObject.IsInstanceValid(sprite) ? sprite.GetLayerVisibleCountForRender() : 0);
			if (num > 0)
			{
				return num + 1;
			}
			return Mathf.Max(1, followSlotId);
		}
		if (drawLayerId < 0)
		{
			return followSlotId;
		}
		return drawLayerId;
	}

	public void ApplyRuntimeTransform(Transform2D transform)
	{
		_runtimeBaseTransform = transform;
		transform.Origin += _runtimeVisualOffset;
		CommitRuntimeTransform(transform);
	}

	public void AddRuntimeVisualOffset(Vector2 offset)
	{
		SetRuntimeVisualOffset(_runtimeVisualOffset + offset);
	}

	public void SetRuntimeVisualOffset(Vector2 offset)
	{
		if (!(_runtimeVisualOffset == offset))
		{
			_runtimeBaseTransform = Transform;
			_runtimeBaseTransform.Origin -= _runtimeVisualOffset;
			_runtimeVisualOffset = offset;
			Transform2D runtimeBaseTransform = _runtimeBaseTransform;
			runtimeBaseTransform.Origin += _runtimeVisualOffset;
			CommitRuntimeTransform(runtimeBaseTransform);
		}
	}

	private void CommitRuntimeTransform(Transform2D transform)
	{
		if (!(Transform == transform))
		{
			Transform = transform;
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.NotifySlotOwnedSpriteTransformChangedForRender(this);
			}
			Vector2 origin = transform.Origin;
			if (!(origin == CachedPosition))
			{
				CachedPosition = origin;
				RuntimePositionVersion++;
			}
		}
	}

	public void Update()
	{
		if (mode != 0 || sprite == null || !sprite.TryGetInterpolatedSlotPose(followSlotId - 1, out var mediaId, out var transform) || mediaId == 65535)
		{
			return;
		}
		transform = transform.Translated(sprite.offset);
		if (!(transform != Transform2D.Identity))
		{
			return;
		}
		Transform2D transform2 = transform.TranslatedLocal(offset);
		if (!useRotate)
		{
			transform2 = transform2.RotatedLocal(0f - transform2.Rotation);
		}
		if (!useScale)
		{
			Vector2 scale = transform2.Scale;
			if (scale.X != 0f && scale.Y != 0f)
			{
				transform2 = transform2.ScaledLocal(Vector2.One / scale);
			}
		}
		ApplyRuntimeTransform(transform2);
		if (!useSkew)
		{
			Skew = 0f;
		}
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		long num = 4L;
		Array<Dictionary> array = new Array<Dictionary>();
		if (sprite == null || sprite.flashAnimeData == null)
		{
			return array;
		}
		if (sprite.flashAnimeData.layerDictionary.Count > 0)
		{
			array.Add(new Dictionary
			{
				{ "name", "Layer" },
				{ "type", 2 },
				{ "hint", 2 },
				{
					"hint_string",
					sprite.flashAnimeData.GetLayerHintString(includeNoone: true)
				},
				{ "usage", num }
			});
			array.Add(new Dictionary
			{
				{ "name", "Draw Layer" },
				{ "type", 2 },
				{ "hint", 2 },
				{
					"hint_string",
					$"{sprite.flashAnimeData.GetLayerHintString(includeNoone: true)},Top:{-2}"
				},
				{ "usage", num }
			});
		}
		return array;
	}

	public override bool _Set(StringName property, Variant value)
	{
		if (property == (StringName)"Layer")
		{
			followSlotId = value.AsInt32();
			if (drawLayerId == -1)
			{
				drawLayerId = followSlotId;
			}
			return true;
		}
		if (property == (StringName)"Draw Layer")
		{
			drawLayerId = value.AsInt32();
			MarkManagedSpriteChildrenDirty();
			return true;
		}
		return false;
	}

	public override Variant _Get(StringName property)
	{
		if (property == (StringName)"Layer")
		{
			return followSlotId;
		}
		if (property == (StringName)"Draw Layer")
		{
			return (drawLayerId == -2) ? (-2) : ResolveDrawLayerId();
		}
		return default;
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		if (property == (StringName)"Layer")
		{
			return true;
		}
		if (property == (StringName)"Draw Layer")
		{
			return true;
		}
		return false;
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		if (property == (StringName)"Layer")
		{
			return 0;
		}
		if (property == (StringName)"Draw Layer")
		{
			return followSlotId;
		}
		return default;
	}

	public AdobeAnimatePart CreatePart(Array<StringName> layerList)
	{
		if (!TryCreatePartSnapshot(layerList, out var animeData, out var mediaReplace, out var mediaReplaceAtlasPaths, out var elementOutputList))
		{
			return null;
		}
		AdobeAnimatePart adobeAnimatePart = new AdobeAnimatePart
		{
			flashAnimeData = animeData,
			mediaReplace = mediaReplace,
			mediaReplaceAtlasPaths = mediaReplaceAtlasPaths,
			offset = Vector2.Zero,
			elementList = elementOutputList
		};
		sprite.OnPartSnapshotCreated(this, layerList, adobeAnimatePart);
		return adobeAnimatePart;
	}

	public bool TryCreatePartSnapshot(Array<StringName> layerList, out AdobeAnimateData animeData, out Array<Texture2D> mediaReplace, out Array<string> mediaReplaceAtlasPaths, out Array<Array> elementOutputList)
	{
		animeData = null;
		mediaReplace = null;
		mediaReplaceAtlasPaths = null;
		elementOutputList = null;
		if (sprite == null || !GodotObject.IsInstanceValid(sprite) || !GodotObject.IsInstanceValid(sprite.flashAnimeData))
		{
			return false;
		}
		Update();
		(_partSnapshotLayerIds ?? (_partSnapshotLayerIds = new HashSet<int>())).Clear();
		if (layerList != null)
		{
			foreach (StringName layer in layerList)
			{
				if (sprite.flashAnimeData.layerDictionary.TryGetValue(layer, out var value))
				{
					_partSnapshotLayerIds.Add((int)value);
				}
			}
		}
		animeData = sprite.flashAnimeData;
		mediaReplace = sprite.CreateActiveMediaReplaceSnapshotForRender();
		mediaReplaceAtlasPaths = sprite.CreateActiveMediaReplaceAtlasPathSnapshotForRender();
		elementOutputList = new Array<Array>();
		sprite.AppendInterpolatedLayerElementsForRender(_partSnapshotLayerIds, Transform.Origin, elementOutputList);
		return elementOutputList.Count > 0;
	}

	internal bool TryAppendPartElementsForRender(Array<StringName> layerList, IAdobeAnimateInterpolatedElementSink sink)
	{
		if (sprite == null || !GodotObject.IsInstanceValid(sprite) || !GodotObject.IsInstanceValid(sprite.flashAnimeData) || sink == null)
		{
			return false;
		}
		Update();
		(_partSnapshotLayerIds ?? (_partSnapshotLayerIds = new HashSet<int>())).Clear();
		if (layerList != null)
		{
			foreach (StringName layer in layerList)
			{
				if (sprite.flashAnimeData.layerDictionary.TryGetValue(layer, out var value))
				{
					_partSnapshotLayerIds.Add((int)value);
				}
			}
		}
		return sprite.AppendInterpolatedLayerElementsForRender(_partSnapshotLayerIds, Transform.Origin, sink) > 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(25)
		{
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshRuntimeUpdateRequirement, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "refreshVisibilityWatchers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRuntimeFollowVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetRuntimeFollowVisibilityCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshRuntimeDependencyVisibilityWatchers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CollectRuntimeDependencyVisibilityWatchers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectRuntimeDependencyVisibilityWatcher, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvasItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CanvasItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisconnectRuntimeDependencyVisibilityWatchers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRuntimeDependencyVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkManagedSpriteChildrenDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveDrawLayerId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyRuntimeTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Transform2D, "transform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddRuntimeVisualOffset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeVisualOffset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitRuntimeTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Transform2D, "transform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Update, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetPropertyList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Set, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._Get, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyCanRevert, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._PropertyGetRevert, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePart, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "layerList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
			ret = default;
			return true;
		}
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
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshRuntimeUpdateRequirement && args.Count == 1)
		{
			RefreshRuntimeUpdateRequirement(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRuntimeFollowVisibility && args.Count == 1)
		{
			ApplyRuntimeFollowVisibility(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetRuntimeFollowVisibilityCache && args.Count == 0)
		{
			ResetRuntimeFollowVisibilityCache();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshRuntimeDependencyVisibilityWatchers && args.Count == 0)
		{
			RefreshRuntimeDependencyVisibilityWatchers();
			ret = default;
			return true;
		}
		if (method == MethodName.CollectRuntimeDependencyVisibilityWatchers && args.Count == 1)
		{
			CollectRuntimeDependencyVisibilityWatchers(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectRuntimeDependencyVisibilityWatcher && args.Count == 1)
		{
			ConnectRuntimeDependencyVisibilityWatcher(VariantUtils.ConvertTo<CanvasItem>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectRuntimeDependencyVisibilityWatchers && args.Count == 0)
		{
			DisconnectRuntimeDependencyVisibilityWatchers();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRuntimeDependencyVisibilityChanged && args.Count == 0)
		{
			OnRuntimeDependencyVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkManagedSpriteChildrenDirty && args.Count == 0)
		{
			MarkManagedSpriteChildrenDirty();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveDrawLayerId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveDrawLayerId());
			return true;
		}
		if (method == MethodName.ApplyRuntimeTransform && args.Count == 1)
		{
			ApplyRuntimeTransform(VariantUtils.ConvertTo<Transform2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddRuntimeVisualOffset && args.Count == 1)
		{
			AddRuntimeVisualOffset(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRuntimeVisualOffset && args.Count == 1)
		{
			SetRuntimeVisualOffset(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitRuntimeTransform && args.Count == 1)
		{
			CommitRuntimeTransform(VariantUtils.ConvertTo<Transform2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Update && args.Count == 0)
		{
			Update();
			ret = default;
			return true;
		}
		if (method == MethodName._GetPropertyList && args.Count == 0)
		{
			Array<Dictionary> array = _GetPropertyList();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName._Set && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_Set(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName._Get && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_Get(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyCanRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(_PropertyCanRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._PropertyGetRevert && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_PropertyGetRevert(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.CreatePart && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimatePart>(CreatePart(VariantUtils.ConvertToArray<StringName>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._EnterTree)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.RefreshRuntimeUpdateRequirement)
		{
			return true;
		}
		if (method == MethodName.ApplyRuntimeFollowVisibility)
		{
			return true;
		}
		if (method == MethodName.ResetRuntimeFollowVisibilityCache)
		{
			return true;
		}
		if (method == MethodName.RefreshRuntimeDependencyVisibilityWatchers)
		{
			return true;
		}
		if (method == MethodName.CollectRuntimeDependencyVisibilityWatchers)
		{
			return true;
		}
		if (method == MethodName.ConnectRuntimeDependencyVisibilityWatcher)
		{
			return true;
		}
		if (method == MethodName.DisconnectRuntimeDependencyVisibilityWatchers)
		{
			return true;
		}
		if (method == MethodName.OnRuntimeDependencyVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.MarkManagedSpriteChildrenDirty)
		{
			return true;
		}
		if (method == MethodName.ResolveDrawLayerId)
		{
			return true;
		}
		if (method == MethodName.ApplyRuntimeTransform)
		{
			return true;
		}
		if (method == MethodName.AddRuntimeVisualOffset)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeVisualOffset)
		{
			return true;
		}
		if (method == MethodName.CommitRuntimeTransform)
		{
			return true;
		}
		if (method == MethodName.Update)
		{
			return true;
		}
		if (method == MethodName._GetPropertyList)
		{
			return true;
		}
		if (method == MethodName._Set)
		{
			return true;
		}
		if (method == MethodName._Get)
		{
			return true;
		}
		if (method == MethodName._PropertyCanRevert)
		{
			return true;
		}
		if (method == MethodName._PropertyGetRevert)
		{
			return true;
		}
		if (method == MethodName.CreatePart)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.sprite)
		{
			sprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.mode)
		{
			mode = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.followSlotId)
		{
			followSlotId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.drawLayerId)
		{
			drawLayerId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.offset)
		{
			offset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.useRotate)
		{
			useRotate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useScale)
		{
			useScale = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useSkew)
		{
			useSkew = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useFollowVisible)
		{
			useFollowVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.updateAllFrame)
		{
			updateAllFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.RequiresRuntimeUpdate)
		{
			RequiresRuntimeUpdate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeUpdateVisualOnly)
		{
			RuntimeUpdateVisualOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.CachedPosition)
		{
			CachedPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.RuntimePositionVersion)
		{
			RuntimePositionVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._runtimeInsideTree)
		{
			_runtimeInsideTree = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeFollowVisibilityKnown)
		{
			_runtimeFollowVisibilityKnown = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeFollowVisibilityValue)
		{
			_runtimeFollowVisibilityValue = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._mode)
		{
			_mode = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._followSlotId)
		{
			_followSlotId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._drawLayerId)
		{
			_drawLayerId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._updateAllFrame)
		{
			_updateAllFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeBaseTransform)
		{
			_runtimeBaseTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._runtimeVisualOffset)
		{
			_runtimeVisualOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.sprite)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateSprite>(sprite);
			return true;
		}
		int from;
		if (name == PropertyName.mode)
		{
			from = mode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.followSlotId)
		{
			from = followSlotId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.drawLayerId)
		{
			from = drawLayerId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		Vector2 from2;
		if (name == PropertyName.offset)
		{
			from2 = offset;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		bool from3;
		if (name == PropertyName.useRotate)
		{
			from3 = useRotate;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.useScale)
		{
			from3 = useScale;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.useSkew)
		{
			from3 = useSkew;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.useFollowVisible)
		{
			from3 = useFollowVisible;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.updateAllFrame)
		{
			from3 = updateAllFrame;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.RequiresRuntimeUpdate)
		{
			from3 = RequiresRuntimeUpdate;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.RuntimeUpdateVisualOnly)
		{
			from3 = RuntimeUpdateVisualOnly;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.IsRuntimeInsideTree)
		{
			from3 = IsRuntimeInsideTree;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CachedPosition)
		{
			from2 = CachedPosition;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.RuntimePositionVersion)
		{
			from = RuntimePositionVersion;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._runtimeInsideTree)
		{
			value = VariantUtils.CreateFrom(in _runtimeInsideTree);
			return true;
		}
		if (name == PropertyName._runtimeFollowVisibilityKnown)
		{
			value = VariantUtils.CreateFrom(in _runtimeFollowVisibilityKnown);
			return true;
		}
		if (name == PropertyName._runtimeFollowVisibilityValue)
		{
			value = VariantUtils.CreateFrom(in _runtimeFollowVisibilityValue);
			return true;
		}
		if (name == PropertyName._mode)
		{
			value = VariantUtils.CreateFrom(in _mode);
			return true;
		}
		if (name == PropertyName._followSlotId)
		{
			value = VariantUtils.CreateFrom(in _followSlotId);
			return true;
		}
		if (name == PropertyName._drawLayerId)
		{
			value = VariantUtils.CreateFrom(in _drawLayerId);
			return true;
		}
		if (name == PropertyName._updateAllFrame)
		{
			value = VariantUtils.CreateFrom(in _updateAllFrame);
			return true;
		}
		if (name == PropertyName._runtimeBaseTransform)
		{
			value = VariantUtils.CreateFrom(in _runtimeBaseTransform);
			return true;
		}
		if (name == PropertyName._runtimeVisualOffset)
		{
			value = VariantUtils.CreateFrom(in _runtimeVisualOffset);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.sprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeInsideTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeFollowVisibilityKnown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeFollowVisibilityValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._mode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.mode, PropertyHint.Enum, "Follow,Drive", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._followSlotId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.followSlotId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._drawLayerId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.drawLayerId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.offset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useRotate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useSkew, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useFollowVisible, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updateAllFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.updateAllFrame, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RequiresRuntimeUpdate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RuntimeUpdateVisualOnly, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsRuntimeInsideTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.CachedPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RuntimePositionVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._runtimeBaseTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._runtimeVisualOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.sprite, Variant.From<AdobeAnimateSprite>(sprite));
		info.AddProperty(PropertyName.mode, Variant.From<int>(mode));
		info.AddProperty(PropertyName.followSlotId, Variant.From<int>(followSlotId));
		info.AddProperty(PropertyName.drawLayerId, Variant.From<int>(drawLayerId));
		info.AddProperty(PropertyName.offset, Variant.From<Vector2>(offset));
		info.AddProperty(PropertyName.useRotate, Variant.From<bool>(useRotate));
		info.AddProperty(PropertyName.useScale, Variant.From<bool>(useScale));
		info.AddProperty(PropertyName.useSkew, Variant.From<bool>(useSkew));
		info.AddProperty(PropertyName.useFollowVisible, Variant.From<bool>(useFollowVisible));
		info.AddProperty(PropertyName.updateAllFrame, Variant.From<bool>(updateAllFrame));
		info.AddProperty(PropertyName.RequiresRuntimeUpdate, Variant.From<bool>(RequiresRuntimeUpdate));
		info.AddProperty(PropertyName.RuntimeUpdateVisualOnly, Variant.From<bool>(RuntimeUpdateVisualOnly));
		info.AddProperty(PropertyName.CachedPosition, Variant.From<Vector2>(CachedPosition));
		info.AddProperty(PropertyName.RuntimePositionVersion, Variant.From<int>(RuntimePositionVersion));
		info.AddProperty(PropertyName._runtimeInsideTree, Variant.From(in _runtimeInsideTree));
		info.AddProperty(PropertyName._runtimeFollowVisibilityKnown, Variant.From(in _runtimeFollowVisibilityKnown));
		info.AddProperty(PropertyName._runtimeFollowVisibilityValue, Variant.From(in _runtimeFollowVisibilityValue));
		info.AddProperty(PropertyName._mode, Variant.From(in _mode));
		info.AddProperty(PropertyName._followSlotId, Variant.From(in _followSlotId));
		info.AddProperty(PropertyName._drawLayerId, Variant.From(in _drawLayerId));
		info.AddProperty(PropertyName._updateAllFrame, Variant.From(in _updateAllFrame));
		info.AddProperty(PropertyName._runtimeBaseTransform, Variant.From(in _runtimeBaseTransform));
		info.AddProperty(PropertyName._runtimeVisualOffset, Variant.From(in _runtimeVisualOffset));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.sprite, out var value))
		{
			sprite = value.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.mode, out var value2))
		{
			mode = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.followSlotId, out var value3))
		{
			followSlotId = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.drawLayerId, out var value4))
		{
			drawLayerId = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.offset, out var value5))
		{
			offset = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.useRotate, out var value6))
		{
			useRotate = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useScale, out var value7))
		{
			useScale = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useSkew, out var value8))
		{
			useSkew = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useFollowVisible, out var value9))
		{
			useFollowVisible = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.updateAllFrame, out var value10))
		{
			updateAllFrame = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.RequiresRuntimeUpdate, out var value11))
		{
			RequiresRuntimeUpdate = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeUpdateVisualOnly, out var value12))
		{
			RuntimeUpdateVisualOnly = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.CachedPosition, out var value13))
		{
			CachedPosition = value13.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.RuntimePositionVersion, out var value14))
		{
			RuntimePositionVersion = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName._runtimeInsideTree, out var value15))
		{
			_runtimeInsideTree = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeFollowVisibilityKnown, out var value16))
		{
			_runtimeFollowVisibilityKnown = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeFollowVisibilityValue, out var value17))
		{
			_runtimeFollowVisibilityValue = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mode, out var value18))
		{
			_mode = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName._followSlotId, out var value19))
		{
			_followSlotId = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName._drawLayerId, out var value20))
		{
			_drawLayerId = value20.As<int>();
		}
		if (info.TryGetProperty(PropertyName._updateAllFrame, out var value21))
		{
			_updateAllFrame = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeBaseTransform, out var value22))
		{
			_runtimeBaseTransform = value22.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._runtimeVisualOffset, out var value23))
		{
			_runtimeVisualOffset = value23.As<Vector2>();
		}
	}
}
