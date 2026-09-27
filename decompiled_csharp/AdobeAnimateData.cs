using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://addons/AdobeAnimateEditor/Resource/AdobeAnimateData.cs")]
public class AdobeAnimateData : Resource
{
	private struct AnimElement
	{
		public int MediaId;

		public Transform2D Transform;

		public Color Color;
	}

	private readonly struct EditorDatElement(int mediaId, float xx, float xy, float yx, float yy, float originX, float originY, float alpha)
	{
		public readonly int MediaId = mediaId;

		public readonly float Xx = xx;

		public readonly float Xy = xy;

		public readonly float Yx = yx;

		public readonly float Yy = yy;

		public readonly float OriginX = originX;

		public readonly float OriginY = originY;

		public readonly float Alpha = alpha;
	}

	internal readonly struct EditorDatClip(string name, int start, int end)
	{
		public readonly string Name = name;

		public readonly int Start = start;

		public readonly int End = end;
	}

	internal readonly struct EditorDatEvent(string command, string argument)
	{
		public readonly string Command = command;

		public readonly string Argument = argument;
	}

	internal readonly struct EditorDatEventFrame(int frame, EditorDatEvent[] items)
	{
		public readonly int Frame = frame;

		public readonly EditorDatEvent[] Items = items;
	}

	public sealed class EditorDatHydrationResult
	{
		internal string DatPath = "";

		internal int FrameScale = 1;

		internal double FrameRate;

		internal int FrameMax;

		internal Vector2I AtlasSize;

		internal string[] MediaNames = System.Array.Empty<string>();

		internal Vector4[] MediaRects = System.Array.Empty<Vector4>();

		internal string[] LayerNames = System.Array.Empty<string>();

		internal EditorDatClip[] Clips = System.Array.Empty<EditorDatClip>();

		internal EditorDatEventFrame[] EventFrames = System.Array.Empty<EditorDatEventFrame>();

		internal int[] FrameOffsets = System.Array.Empty<int>();

		internal int[] FrameCounts = System.Array.Empty<int>();

		internal int[] SliceKeys = System.Array.Empty<int>();

		internal int[] SliceMediaIds = System.Array.Empty<int>();

		internal int[] SliceLayerIds = System.Array.Empty<int>();

		internal int[] SliceDrawOrders = System.Array.Empty<int>();

		internal int[] SliceFlags = System.Array.Empty<int>();

		internal float[] SliceTransforms = System.Array.Empty<float>();

		internal float[] SliceAlpha = System.Array.Empty<float>();

		public bool IsSuccess { get; internal set; }

		public string ErrorMessage { get; internal set; } = "";

		public int FrameCount => FrameOffsets?.Length ?? 0;

		public int SliceCount => SliceKeys?.Length ?? 0;
	}

	private readonly struct TimelineMediaIdsReader
	{
		private readonly int[] _packed;

		private readonly Array<int> _typed;

		private readonly Godot.Collections.Array _untyped;

		public int Count => _packed?.Length ?? _typed?.Count ?? _untyped?.Count ?? 0;

		public TimelineMediaIdsReader(int[] packed)
		{
			_packed = packed;
			_typed = null;
			_untyped = null;
		}

		public TimelineMediaIdsReader(Array<int> typed)
		{
			_packed = null;
			_typed = typed;
			_untyped = null;
		}

		public TimelineMediaIdsReader(Godot.Collections.Array untyped)
		{
			_packed = null;
			_typed = null;
			_untyped = untyped;
		}

		public int Get(int index)
		{
			if (_packed != null)
			{
				return _packed[index];
			}
			if (_typed != null)
			{
				return _typed[index];
			}
			return _untyped[index].AsInt32();
		}
	}

	private readonly struct TimelineTransformReader
	{
		private readonly Transform2D[] _packed;

		private readonly Array<Transform2D> _typed;

		private readonly Godot.Collections.Array _untyped;

		public int Count => _packed?.Length ?? _typed?.Count ?? _untyped?.Count ?? 0;

		public TimelineTransformReader(Transform2D[] packed)
		{
			_packed = packed;
			_typed = null;
			_untyped = null;
		}

		public TimelineTransformReader(Array<Transform2D> typed)
		{
			_packed = null;
			_typed = typed;
			_untyped = null;
		}

		public TimelineTransformReader(Godot.Collections.Array untyped)
		{
			_packed = null;
			_typed = null;
			_untyped = untyped;
		}

		public Transform2D Get(int index)
		{
			if (_packed != null)
			{
				return _packed[index];
			}
			if (_typed != null)
			{
				return _typed[index];
			}
			return _untyped[index].As<Transform2D>();
		}
	}

	private sealed class LegacyResourceSnapshot
	{
		public double FrameRate;

		public int FrameMax;

		public Vector2 ImageAtlasSizeCache;

		public Array<Godot.Collections.Array> TimelineData = new Array<Godot.Collections.Array>();

		public Array<Rect2> MediaList = new Array<Rect2>();

		public Array<string> LayerList = new Array<string>();

		public Array<Godot.Collections.Array> Events = new Array<Godot.Collections.Array>();

		public Dictionary Clips = new Dictionary();

		public Dictionary MediaDictionary = new Dictionary();

		public Dictionary LayerDictionary = new Dictionary();

		public Dictionary ReplaceSlotDictionary = new Dictionary();

		public int MaxElement;

		public string FullDataDatPath = "";

		public int FullDataFrameScale;
	}

	public readonly struct FrameSlice
	{
		public int SliceKey { get; }

		public int MediaId { get; }

		public int LayerId { get; }

		public int DrawOrder { get; }

		public int Flags { get; }

		public Transform2D Transform { get; }

		public float Alpha { get; }

		public FrameSlice(int sliceKey, int mediaId, int layerId, int drawOrder, int flags, Transform2D transform, float alpha)
		{
			SliceKey = sliceKey;
			MediaId = mediaId;
			LayerId = layerId;
			DrawOrder = drawOrder;
			Flags = flags;
			Transform = transform;
			Alpha = alpha;
		}
	}

	private delegate string FrameStateMutation(FrameEditState state);

	private sealed class FrameRecord
	{
		public List<FrameSlice> Slices { get; }

		public Godot.Collections.Array Events { get; }

		public FrameRecord(List<FrameSlice> slices, Godot.Collections.Array events)
		{
			Slices = slices ?? new List<FrameSlice>();
			Events = events ?? new Godot.Collections.Array();
		}

		public FrameRecord Clone()
		{
			return new FrameRecord(new List<FrameSlice>(Slices), CloneFrameEditEventFrame(Events));
		}
	}

	private sealed class FrameEditState
	{
		public string AnimeFile = "";

		public int FrameScale = 1;

		public double FrameRate;

		public Vector2 ImageAtlasSizeCache;

		public string EditorResolvedAnimeFilePath = "";

		public string FullDataDatPath = "";

		public int FullDataFrameScale;

		public int FrameMax;

		public int[] FrameOffsets = System.Array.Empty<int>();

		public int[] FrameCounts = System.Array.Empty<int>();

		public int[] SliceKeys = System.Array.Empty<int>();

		public int[] SliceMediaIds = System.Array.Empty<int>();

		public int[] SliceLayerIds = System.Array.Empty<int>();

		public int[] SliceDrawOrders = System.Array.Empty<int>();

		public int[] SliceFlags = System.Array.Empty<int>();

		public float[] SliceTransforms = System.Array.Empty<float>();

		public float[] SliceAlpha = System.Array.Empty<float>();

		public Vector4[] MediaRects = System.Array.Empty<Vector4>();

		public Array<Godot.Collections.Array> TimelineData = new Array<Godot.Collections.Array>();

		public Array<Rect2> MediaList = new Array<Rect2>();

		public Array<string> LayerList = new Array<string>();

		public Dictionary Clips = new Dictionary();

		public Array<Godot.Collections.Array> Events = new Array<Godot.Collections.Array>();

		public Dictionary MediaDictionary = new Dictionary();

		public Dictionary LayerDictionary = new Dictionary();

		public Dictionary ReplaceSlotDictionary = new Dictionary();

		public int MaxElement;

		public bool HasAuthoredPackedFrames;

		public FrameEditState Clone(bool includeLegacyTimeline = true)
		{
			return new FrameEditState
			{
				AnimeFile = AnimeFile,
				FrameScale = FrameScale,
				FrameRate = FrameRate,
				ImageAtlasSizeCache = ImageAtlasSizeCache,
				EditorResolvedAnimeFilePath = EditorResolvedAnimeFilePath,
				FullDataDatPath = FullDataDatPath,
				FullDataFrameScale = FullDataFrameScale,
				FrameMax = FrameMax,
				FrameOffsets = CloneFrameEditArray(FrameOffsets),
				FrameCounts = CloneFrameEditArray(FrameCounts),
				SliceKeys = CloneFrameEditArray(SliceKeys),
				SliceMediaIds = CloneFrameEditArray(SliceMediaIds),
				SliceLayerIds = CloneFrameEditArray(SliceLayerIds),
				SliceDrawOrders = CloneFrameEditArray(SliceDrawOrders),
				SliceFlags = CloneFrameEditArray(SliceFlags),
				SliceTransforms = CloneFrameEditArray(SliceTransforms),
				SliceAlpha = CloneFrameEditArray(SliceAlpha),
				MediaRects = CloneFrameEditArray(MediaRects),
				TimelineData = (includeLegacyTimeline ? CloneFrameEditEvents(TimelineData) : new Array<Godot.Collections.Array>()),
				MediaList = CloneFrameEditRectArray(MediaList),
				LayerList = CloneFrameEditStringArray(LayerList),
				Clips = CloneFrameEditDictionary(Clips),
				Events = CloneFrameEditEvents(Events),
				MediaDictionary = CloneFrameEditDictionary(MediaDictionary),
				LayerDictionary = CloneFrameEditDictionary(LayerDictionary),
				ReplaceSlotDictionary = CloneFrameEditDictionary(ReplaceSlotDictionary),
				MaxElement = MaxElement,
				HasAuthoredPackedFrames = HasAuthoredPackedFrames
			};
		}
	}

	public new class MethodName : Resource.MethodName
	{
		public static readonly StringName SetFrameScaleForDeferredHydration = "SetFrameScaleForDeferredHydration";

		public static readonly StringName BuildTimelineCache = "BuildTimelineCache";

		public static readonly StringName Init = "Init";

		public static readonly StringName InitForModImport = "InitForModImport";

		public static readonly StringName LerpManaged = "LerpManaged";

		public static readonly StringName LoadDatMetadataForModPreview = "LoadDatMetadataForModPreview";

		public static readonly StringName SetAnimeFileForModImport = "SetAnimeFileForModImport";

		public static readonly StringName SetEditorResolvedAnimeFilePath = "SetEditorResolvedAnimeFilePath";

		public static readonly StringName TryInitForEditorAssignedAnimeFile = "TryInitForEditorAssignedAnimeFile";

		public static readonly StringName HasPackedRuntimeData = "HasPackedRuntimeData";

		public static readonly StringName EnsurePackedRuntimeData = "EnsurePackedRuntimeData";

		public static readonly StringName CloneTimelineArray = "CloneTimelineArray";

		public static readonly StringName CloneRectArray = "CloneRectArray";

		public static readonly StringName CloneStringArray = "CloneStringArray";

		public static readonly StringName CloneDictionary = "CloneDictionary";

		public static readonly StringName HasFullDataForDatPath = "HasFullDataForDatPath";

		public static readonly StringName InitInternal = "InitInternal";

		public static readonly StringName BuildPackedRuntimeDataFromTimelineData = "BuildPackedRuntimeDataFromTimelineData";

		public static readonly StringName StripSourceRuntimeData = "StripSourceRuntimeData";

		public static readonly StringName ClearMetadataPreview = "ClearMetadataPreview";

		public static readonly StringName SkipBytes = "SkipBytes";

		public static readonly StringName CreateTransparentFallbackImage = "CreateTransparentFallbackImage";

		public static readonly StringName GetResolvedAnimeFilePath = "GetResolvedAnimeFilePath";

		public static readonly StringName GetAtlasSourceKey = "GetAtlasSourceKey";

		public static readonly StringName HasEmbeddedAtlasSource = "HasEmbeddedAtlasSource";

		public static readonly StringName GetMediaRect = "GetMediaRect";

		public static readonly StringName GetLayerHintString = "GetLayerHintString";

		public static readonly StringName BuildLayerNameArray = "BuildLayerNameArray";

		public static readonly StringName GetLayerNameArrayCount = "GetLayerNameArrayCount";

		public static readonly StringName ResolveAnimeFilePath = "ResolveAnimeFilePath";

		public static readonly StringName TryResolveOwnerBasenameCompanionDat = "TryResolveOwnerBasenameCompanionDat";

		public static readonly StringName NormalizeDatPath = "NormalizeDatPath";

		public static readonly StringName NormalizeAtlasSourcePath = "NormalizeAtlasSourcePath";

		public static readonly StringName IsRelativeFilePath = "IsRelativeFilePath";

		public static readonly StringName ShouldAutoInitAnimeFile = "ShouldAutoInitAnimeFile";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName ClearPackedRuntimeData = "ClearPackedRuntimeData";

		public static readonly StringName HasClip = "HasClip";

		public static readonly StringName GetClip = "GetClip";

		public static readonly StringName CaptureFrameEditSnapshot = "CaptureFrameEditSnapshot";

		public static readonly StringName CaptureFullFrameEditSnapshot = "CaptureFullFrameEditSnapshot";

		public static readonly StringName CaptureFrameEditSnapshotCore = "CaptureFrameEditSnapshotCore";

		public static readonly StringName ApplyFrameEditSnapshot = "ApplyFrameEditSnapshot";

		public static readonly StringName ClearFrameAuthoringFlag = "ClearFrameAuthoringFlag";

		public static readonly StringName SetFrameEditFailure = "SetFrameEditFailure";

		public static readonly StringName InvalidateFrameAuthoringCaches = "InvalidateFrameAuthoringCaches";

		public static readonly StringName RebuildFrameEditEventIndex = "RebuildFrameEditEventIndex";

		public static readonly StringName AdjustClipsForInsert = "AdjustClipsForInsert";

		public static readonly StringName AdjustClipsForDelete = "AdjustClipsForDelete";

		public static readonly StringName CloneFrameEditDictionary = "CloneFrameEditDictionary";

		public static readonly StringName CloneFrameEditEventFrame = "CloneFrameEditEventFrame";

		public static readonly StringName CloneFrameEditEvents = "CloneFrameEditEvents";

		public static readonly StringName CloneFrameEditRectArray = "CloneFrameEditRectArray";

		public static readonly StringName CloneFrameEditStringArray = "CloneFrameEditStringArray";
	}

	public new class PropertyName : Resource.PropertyName
	{
		public static readonly StringName animeFile = "animeFile";

		public static readonly StringName frameRate = "frameRate";

		public static readonly StringName frameScale = "frameScale";

		public static readonly StringName frameMax = "frameMax";

		public static readonly StringName ImageAtlasSizeCache = "ImageAtlasSizeCache";

		public static readonly StringName timelineData = "timelineData";

		public static readonly StringName mediaList = "mediaList";

		public static readonly StringName frameOffsets = "frameOffsets";

		public static readonly StringName frameCounts = "frameCounts";

		public static readonly StringName sliceKeys = "sliceKeys";

		public static readonly StringName sliceMediaIds = "sliceMediaIds";

		public static readonly StringName sliceLayerIds = "sliceLayerIds";

		public static readonly StringName sliceDrawOrders = "sliceDrawOrders";

		public static readonly StringName sliceFlags = "sliceFlags";

		public static readonly StringName sliceTransforms = "sliceTransforms";

		public static readonly StringName sliceAlpha = "sliceAlpha";

		public static readonly StringName mediaRects = "mediaRects";

		public static readonly StringName layerList = "layerList";

		public static readonly StringName events = "events";

		public static readonly StringName clips = "clips";

		public static readonly StringName mediaDictionary = "mediaDictionary";

		public static readonly StringName layerDictionary = "layerDictionary";

		public static readonly StringName replaceSlotDictionary = "replaceSlotDictionary";

		public static readonly StringName rasterCompositeData = "rasterCompositeData";

		public static readonly StringName extraMediaReplaceTexturePaths = "extraMediaReplaceTexturePaths";

		public static readonly StringName maxElement = "maxElement";

		public static readonly StringName EditableFrameCount = "EditableFrameCount";

		public static readonly StringName HasAuthoredPackedFrames = "HasAuthoredPackedFrames";

		public static readonly StringName AuthoringRevision = "AuthoringRevision";

		public static readonly StringName LastFrameEditError = "LastFrameEditError";

		public static readonly StringName _animeFile = "_animeFile";

		public static readonly StringName _frameScale = "_frameScale";

		public static readonly StringName _editorResolvedAnimeFilePath = "_editorResolvedAnimeFilePath";

		public static readonly StringName _fullDataDatPath = "_fullDataDatPath";

		public static readonly StringName _fullDataFrameScale = "_fullDataFrameScale";

		public static readonly StringName _hasAuthoredPackedFrames = "_hasAuthoredPackedFrames";

		public static readonly StringName _authoringRevision = "_authoringRevision";

		public static readonly StringName _imageAtlasSizeCache = "_imageAtlasSizeCache";
	}

	public new class SignalName : Resource.SignalName
	{
	}

	private string _animeFile = "";

	private int _frameScale = 1;

	private string _editorResolvedAnimeFilePath = "";

	private string _fullDataDatPath = "";

	private int _fullDataFrameScale;

	[Export(PropertyHint.None, "")]
	private bool _hasAuthoredPackedFrames;

	[Export(PropertyHint.None, "")]
	private long _authoringRevision;

	private Vector2 _imageAtlasSizeCache = Vector2.Zero;

	private int[][] TimelineMediaIdsCache;

	private Transform2D[][] TimelineTransformsCache;

	public HashSet<int> EventFrameIndices;

	private const int FrameEditSnapshotVersion = 2;

	[Export(PropertyHint.File, "*.dat")]
	public string animeFile
	{
		get
		{
			return _animeFile;
		}
		set
		{
			_editorResolvedAnimeFilePath = "";
			_animeFile = value;
			if (!_hasAuthoredPackedFrames && value != "" && ShouldAutoInitAnimeFile(value))
			{
				Init();
			}
			else if (!_hasAuthoredPackedFrames && value == "")
			{
				Clear();
			}
			EmitChanged();
			NotifyPropertyListChanged();
		}
	}

	[Export(PropertyHint.None, "")]
	public double frameRate { get; set; } = 30.0;

	[Export(PropertyHint.None, "")]
	public int frameScale
	{
		get
		{
			return _frameScale;
		}
		set
		{
			_frameScale = value;
			if (!_hasAuthoredPackedFrames && _animeFile != "")
			{
				Init();
			}
			EmitChanged();
			NotifyPropertyListChanged();
		}
	}

	[Export(PropertyHint.None, "")]
	public int frameMax { get; set; }

	public Vector2 ImageAtlasSizeCache
	{
		get
		{
			return _imageAtlasSizeCache;
		}
		set
		{
			_imageAtlasSizeCache = value;
		}
	}

	private Array<Godot.Collections.Array> timelineData { get; set; } = new Array<Godot.Collections.Array>();

	private Array<Rect2> mediaList { get; set; } = new Array<Rect2>();

	[Export(PropertyHint.None, "")]
	public int[] frameOffsets { get; set; } = System.Array.Empty<int>();

	[Export(PropertyHint.None, "")]
	public int[] frameCounts { get; set; } = System.Array.Empty<int>();

	[Export(PropertyHint.None, "")]
	public int[] sliceKeys { get; set; } = System.Array.Empty<int>();

	[Export(PropertyHint.None, "")]
	public int[] sliceMediaIds { get; set; } = System.Array.Empty<int>();

	[Export(PropertyHint.None, "")]
	public int[] sliceLayerIds { get; set; } = System.Array.Empty<int>();

	[Export(PropertyHint.None, "")]
	public int[] sliceDrawOrders { get; set; } = System.Array.Empty<int>();

	[Export(PropertyHint.None, "")]
	public int[] sliceFlags { get; set; } = System.Array.Empty<int>();

	[Export(PropertyHint.None, "")]
	public float[] sliceTransforms { get; set; } = System.Array.Empty<float>();

	[Export(PropertyHint.None, "")]
	public float[] sliceAlpha { get; set; } = System.Array.Empty<float>();

	[Export(PropertyHint.None, "")]
	public Vector4[] mediaRects { get; set; } = System.Array.Empty<Vector4>();

	private Array<string> layerList { get; set; } = new Array<string>();

	[Export(PropertyHint.None, "")]
	public Array<Godot.Collections.Array> events { get; set; } = new Array<Godot.Collections.Array>();

	[Export(PropertyHint.None, "")]
	public Dictionary clips { get; set; } = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary mediaDictionary { get; set; } = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary layerDictionary { get; set; } = new Dictionary();

	[Export(PropertyHint.None, "")]
	public Dictionary replaceSlotDictionary { get; set; } = new Dictionary();

	[Export(PropertyHint.None, "")]
	public AdobeAnimateRasterCompositeData rasterCompositeData { get; set; }

	[Export(PropertyHint.File, "*.png,*.webp,*.jpg,*.jpeg,*.svg,*.bmp,*.tga")]
	public Array<string> extraMediaReplaceTexturePaths { get; set; } = new Array<string>();

	private int maxElement { get; set; }

	public int EditableFrameCount => frameOffsets?.Length ?? 0;

	public bool HasAuthoredPackedFrames => _hasAuthoredPackedFrames;

	public long AuthoringRevision => _authoringRevision;

	public string LastFrameEditError { get; private set; } = "";

	public bool SetFrameScaleForDeferredHydration(int value)
	{
		int num = Math.Max(1, value);
		if (_frameScale == num)
		{
			return false;
		}
		_frameScale = num;
		EmitChanged();
		NotifyPropertyListChanged();
		return true;
	}

	private void BuildTimelineCache()
	{
		if (TimelineMediaIdsCache != null && TimelineTransformsCache != null)
		{
			return;
		}
		if (timelineData == null)
		{
			TimelineMediaIdsCache = null;
			TimelineTransformsCache = null;
			return;
		}
		int count = timelineData.Count;
		TimelineMediaIdsCache = new int[count][];
		TimelineTransformsCache = new Transform2D[count][];
		for (int i = 0; i < count; i++)
		{
			Godot.Collections.Array array = timelineData[i];
			if (array.Count < 2)
			{
				TimelineMediaIdsCache[i] = System.Array.Empty<int>();
				TimelineTransformsCache[i] = System.Array.Empty<Transform2D>();
				continue;
			}
			if (!TryReadTimelineMediaIds(array[0], out var mediaIds) || !TryReadTimelineTransforms(array[1], out var transforms))
			{
				TimelineMediaIdsCache[i] = System.Array.Empty<int>();
				TimelineTransformsCache[i] = System.Array.Empty<Transform2D>();
				continue;
			}
			int num = Mathf.Min(mediaIds.Count, transforms.Count);
			int[] array2 = new int[num];
			Transform2D[] array3 = new Transform2D[num];
			for (int j = 0; j < num; j++)
			{
				array2[j] = mediaIds.Get(j);
				array3[j] = transforms.Get(j);
			}
			TimelineMediaIdsCache[i] = array2;
			TimelineTransformsCache[i] = array3;
		}
	}

	private static bool TryReadTimelineMediaIds(Variant value, out TimelineMediaIdsReader mediaIds)
	{
		if (value.VariantType == Variant.Type.PackedInt32Array)
		{
			mediaIds = new TimelineMediaIdsReader(value.As<int[]>());
			return mediaIds.Count > 0;
		}
		if (value.VariantType == Variant.Type.Array)
		{
			try
			{
				Array<int> array = value.AsGodotArray<int>();
				if (array != null)
				{
					mediaIds = new TimelineMediaIdsReader(array);
					return true;
				}
			}
			catch
			{
			}
			Godot.Collections.Array array2 = value.AsGodotArray();
			mediaIds = new TimelineMediaIdsReader(array2);
			return array2 != null;
		}
		try
		{
			int[] array3 = value.As<int[]>();
			mediaIds = new TimelineMediaIdsReader(array3);
			return array3 != null;
		}
		catch
		{
			mediaIds = default;
			return false;
		}
	}

	private static bool TryReadTimelineTransforms(Variant value, out TimelineTransformReader transforms)
	{
		if (value.VariantType == Variant.Type.Array)
		{
			try
			{
				Array<Transform2D> array = value.AsGodotArray<Transform2D>();
				if (array != null)
				{
					transforms = new TimelineTransformReader(array);
					return true;
				}
			}
			catch
			{
			}
			Godot.Collections.Array array2 = value.AsGodotArray();
			transforms = new TimelineTransformReader(array2);
			return array2 != null;
		}
		transforms = default;
		return false;
	}

	public void Init()
	{
		InitInternal(forceRuntime: false);
	}

	public void InitForModImport(string sourceDatPath = "")
	{
		InitInternal(forceRuntime: true, sourceDatPath);
	}

	public Task<EditorDatHydrationResult> BuildEditorDatHydrationAsync(string sourceDatPath = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		return BuildEditorDatHydrationAsync(sourceDatPath, Math.Max(1, _frameScale), cancellationToken);
	}

	public Task<EditorDatHydrationResult> BuildEditorDatHydrationAsync(string sourceDatPath, int requestedFrameScale, CancellationToken cancellationToken = default(CancellationToken))
	{
		string text = ResolveAnimeFilePath(sourceDatPath);
		if (string.IsNullOrWhiteSpace(text))
		{
			return Task.FromResult(FailedEditorDatHydration("未指定 DAT 文件。"));
		}
		string absoluteDatPath = text;
		if (text.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || text.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			absoluteDatPath = ProjectSettings.GlobalizePath(text);
		}
		else if (!Path.IsPathRooted(text))
		{
			absoluteDatPath = ProjectSettings.GlobalizePath(text);
		}
		if (!File.Exists(absoluteDatPath))
		{
			return Task.FromResult(FailedEditorDatHydration("DAT 文件不存在：" + text));
		}
		requestedFrameScale = Math.Max(1, requestedFrameScale);
		string normalizedDatPath = NormalizeDatPath(text);
		return Task.Run(() => ParseEditorDatHydration(absoluteDatPath, normalizedDatPath, requestedFrameScale, cancellationToken), cancellationToken);
	}

	public bool ApplyEditorDatHydration(EditorDatHydrationResult result)
	{
		if (result == null || !result.IsSuccess || result.FrameScale != Math.Max(1, _frameScale))
		{
			return false;
		}
		Clear();
		frameRate = result.FrameRate;
		frameMax = result.FrameMax;
		_imageAtlasSizeCache = new Vector2(result.AtlasSize.X, result.AtlasSize.Y);
		mediaRects = result.MediaRects ?? System.Array.Empty<Vector4>();
		for (int i = 0; i < result.MediaNames.Length; i++)
		{
			mediaDictionary[result.MediaNames[i]] = i;
		}
		for (int j = 0; j < result.LayerNames.Length; j++)
		{
			layerDictionary[result.LayerNames[j]] = j;
		}
		EditorDatClip[] array = result.Clips;
		for (int k = 0; k < array.Length; k++)
		{
			EditorDatClip editorDatClip = array[k];
			clips[editorDatClip.Name] = new Vector2I(editorDatClip.Start, editorDatClip.End);
		}
		events.Resize(Math.Max(0, result.FrameMax));
		EventFrameIndices = new HashSet<int>();
		EditorDatEventFrame[] eventFrames = result.EventFrames;
		for (int k = 0; k < eventFrames.Length; k++)
		{
			EditorDatEventFrame editorDatEventFrame = eventFrames[k];
			if (editorDatEventFrame.Frame >= 0 && editorDatEventFrame.Frame < events.Count)
			{
				Godot.Collections.Array array2 = new Godot.Collections.Array();
				EditorDatEvent[] items = editorDatEventFrame.Items;
				for (int l = 0; l < items.Length; l++)
				{
					EditorDatEvent editorDatEvent = items[l];
					Dictionary dictionary = new Dictionary
					{
						["Command"] = editorDatEvent.Command,
						["Argument"] = editorDatEvent.Argument
					};
					array2.Add(dictionary);
				}
				events[editorDatEventFrame.Frame] = array2;
				EventFrameIndices.Add(editorDatEventFrame.Frame);
			}
		}
		frameOffsets = result.FrameOffsets ?? System.Array.Empty<int>();
		frameCounts = result.FrameCounts ?? System.Array.Empty<int>();
		sliceKeys = result.SliceKeys ?? System.Array.Empty<int>();
		sliceMediaIds = result.SliceMediaIds ?? System.Array.Empty<int>();
		sliceLayerIds = result.SliceLayerIds ?? System.Array.Empty<int>();
		sliceDrawOrders = result.SliceDrawOrders ?? System.Array.Empty<int>();
		sliceFlags = result.SliceFlags ?? System.Array.Empty<int>();
		sliceTransforms = result.SliceTransforms ?? System.Array.Empty<float>();
		sliceAlpha = result.SliceAlpha ?? System.Array.Empty<float>();
		_fullDataDatPath = NormalizeDatPath(result.DatPath);
		_fullDataFrameScale = result.FrameScale;
		EmitChanged();
		NotifyPropertyListChanged();
		return HasPackedRuntimeData();
	}

	private static EditorDatHydrationResult ParseEditorDatHydration(string absoluteDatPath, string datPath, int frameScale, CancellationToken cancellationToken)
	{
		try
		{
			using FileStream fileStream = new FileStream(absoluteDatPath, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
			using BinaryReader binaryReader = new BinaryReader(fileStream, Encoding.UTF8, leaveOpen: false);
			cancellationToken.ThrowIfCancellationRequested();
			double num = binaryReader.ReadSingle();
			if (num <= 0.01)
			{
				num = 24.0;
			}
			int num2 = binaryReader.ReadUInt16();
			Vector2I atlasSize = new Vector2I(binaryReader.ReadUInt16(), binaryReader.ReadUInt16());
			long byteCount = binaryReader.ReadInt64();
			SkipManagedBytes(fileStream, byteCount);
			int num3 = binaryReader.ReadUInt16();
			string[] array = new string[num3];
			Vector4[] array2 = new Vector4[num3];
			for (int i = 0; i < num3; i++)
			{
				array[i] = ReadManagedPascalString(binaryReader);
				array2[i] = new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
			}
			int num4 = binaryReader.ReadUInt16();
			string[] array3 = new string[num4];
			EditorDatElement[][][] array4 = new EditorDatElement[num4][][];
			int[] array5 = new int[num4];
			for (int j = 0; j < num4; j++)
			{
				cancellationToken.ThrowIfCancellationRequested();
				array3[j] = ReadManagedPascalString(binaryReader);
				EditorDatElement[][] array6 = new EditorDatElement[num2][];
				int num5 = 0;
				for (int k = 0; k < num2; k++)
				{
					int num6 = binaryReader.ReadUInt16();
					num5 = Math.Max(num5, num6);
					EditorDatElement[] array7 = new EditorDatElement[num6];
					for (int l = 0; l < num6; l++)
					{
						int mediaId = binaryReader.ReadUInt16();
						float xx = binaryReader.ReadSingle();
						float xy = binaryReader.ReadSingle();
						float yx = binaryReader.ReadSingle();
						float yy = binaryReader.ReadSingle();
						float originX = binaryReader.ReadSingle();
						float originY = binaryReader.ReadSingle();
						float alpha = (float)(binaryReader.ReadUInt32() & 0xFF) / 255f;
						array7[l] = new EditorDatElement(mediaId, xx, xy, yx, yy, originX, originY, alpha);
					}
					array6[k] = array7;
				}
				array4[j] = array6;
				array5[j] = num5;
			}
			int num7 = binaryReader.ReadUInt16();
			EditorDatClip[] array8 = new EditorDatClip[num7];
			for (int m = 0; m < num7; m++)
			{
				string name = ReadManagedPascalString(binaryReader);
				array8[m] = new EditorDatClip(name, binaryReader.ReadUInt16() * frameScale, binaryReader.ReadUInt16() * frameScale);
			}
			int num8 = binaryReader.ReadUInt16();
			EditorDatEventFrame[] array9 = new EditorDatEventFrame[num8];
			for (int n = 0; n < num8; n++)
			{
				int frame = binaryReader.ReadUInt16() * frameScale;
				int num9 = binaryReader.ReadUInt16();
				EditorDatEvent[] array10 = new EditorDatEvent[num9];
				for (int num10 = 0; num10 < num9; num10++)
				{
					array10[num10] = new EditorDatEvent(ReadManagedPascalString(binaryReader), ReadManagedPascalString(binaryReader));
				}
				array9[n] = new EditorDatEventFrame(frame, array10);
			}
			int num11 = ((num2 > 0) ? checked(num2 * frameScale - frameScale + 1) : 0);
			List<int> list = new List<int>(num11);
			List<int> list2 = new List<int>(num11);
			int num12 = (int)Math.Min(2147483647L, fileStream.Length / 30 * frameScale);
			List<int> list3 = new List<int>(num12);
			List<int> list4 = new List<int>(num12);
			List<int> list5 = new List<int>(num12);
			List<int> list6 = new List<int>(num12);
			List<int> list7 = new List<int>(num12);
			List<float> list8 = new List<float>(checked(num12 * 6));
			List<float> list9 = new List<float>(num12);
			int num13 = 0;
			for (int num14 = 0; num14 < num11; num14++)
			{
				if ((num14 & 0x1F) == 0)
				{
					cancellationToken.ThrowIfCancellationRequested();
				}
				list.Add(num13);
				int num15 = 0;
				int num16 = 0;
				int num17 = num14 / frameScale;
				int num18 = num14 % frameScale;
				float weight = (float)num18 / (float)frameScale;
				for (int num19 = 0; num19 < array4.Length; num19++)
				{
					EditorDatElement[] array11 = array4[num19][num17];
					EditorDatElement[] array12 = ((num17 + 1 < num2) ? array4[num19][num17 + 1] : array11);
					int num20 = array5[num19];
					for (int num21 = 0; num21 < num20; num21++)
					{
						if (num21 < array11.Length)
						{
							EditorDatElement editorDatElement = array11[num21];
							float num22 = editorDatElement.Xx;
							float num23 = editorDatElement.Xy;
							float num24 = editorDatElement.Yx;
							float num25 = editorDatElement.Yy;
							float num26 = editorDatElement.OriginX;
							float num27 = editorDatElement.OriginY;
							float num28 = editorDatElement.Alpha;
							if (num18 > 0 && array12.Length == array11.Length && array12[num21].MediaId != 65535)
							{
								EditorDatElement editorDatElement2 = array12[num21];
								num22 = LerpManaged(num22, editorDatElement2.Xx, weight);
								num23 = LerpManaged(num23, editorDatElement2.Xy, weight);
								num24 = LerpManaged(num24, editorDatElement2.Yx, weight);
								num25 = LerpManaged(num25, editorDatElement2.Yy, weight);
								num26 = LerpManaged(num26, editorDatElement2.OriginX, weight);
								num27 = LerpManaged(num27, editorDatElement2.OriginY, weight);
								num28 = LerpManaged(num28, editorDatElement2.Alpha, weight);
							}
							if (editorDatElement.MediaId != 65535)
							{
								list3.Add((num19 << 16) ^ (num21 & 0xFFFF));
								list4.Add(editorDatElement.MediaId);
								list5.Add(num19);
								list6.Add(num16);
								list7.Add(0);
								list8.Add(num22);
								list8.Add(num23);
								list8.Add(num24);
								list8.Add(num25);
								list8.Add(num26);
								list8.Add(num27);
								list9.Add(num28);
								num15++;
								num13++;
							}
						}
						num16++;
					}
				}
				list2.Add(num15);
			}
			return new EditorDatHydrationResult
			{
				IsSuccess = (num11 > 0 && list3.Count > 0),
				ErrorMessage = ((num11 > 0 && list3.Count > 0) ? "" : "DAT 中没有可用的动画帧。"),
				DatPath = datPath,
				FrameScale = frameScale,
				FrameRate = num * (double)frameScale,
				FrameMax = num11,
				AtlasSize = atlasSize,
				MediaNames = array,
				MediaRects = array2,
				LayerNames = array3,
				Clips = array8,
				EventFrames = array9,
				FrameOffsets = list.ToArray(),
				FrameCounts = list2.ToArray(),
				SliceKeys = list3.ToArray(),
				SliceMediaIds = list4.ToArray(),
				SliceLayerIds = list5.ToArray(),
				SliceDrawOrders = list6.ToArray(),
				SliceFlags = list7.ToArray(),
				SliceTransforms = list8.ToArray(),
				SliceAlpha = list9.ToArray()
			};
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			return FailedEditorDatHydration(ex2.Message);
		}
	}

	private static EditorDatHydrationResult FailedEditorDatHydration(string message)
	{
		return new EditorDatHydrationResult
		{
			IsSuccess = false,
			ErrorMessage = (message ?? "DAT 读取失败。")
		};
	}

	private static string ReadManagedPascalString(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		if (num < 0 || num > reader.BaseStream.Length - reader.BaseStream.Position)
		{
			throw new InvalidDataException($"DAT 字符串长度无效：{num}");
		}
		return Encoding.UTF8.GetString(reader.ReadBytes(num));
	}

	private static void SkipManagedBytes(Stream stream, long byteCount)
	{
		if (byteCount < 0 || byteCount > stream.Length - stream.Position)
		{
			throw new InvalidDataException($"DAT 数据块长度无效：{byteCount}");
		}
		stream.Seek(byteCount, SeekOrigin.Current);
	}

	private static float LerpManaged(float from, float to, float weight)
	{
		return from + (to - from) * weight;
	}

	public bool LoadDatMetadataForModPreview(string sourceDatPath = "")
	{
		string path = ResolveAnimeFilePath(sourceDatPath);
		if (!Godot.FileAccess.FileExists(path))
		{
			return false;
		}
		Godot.FileAccess fileAccess = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
		if (fileAccess == null)
		{
			return false;
		}
		ClearMetadataPreview();
		frameRate = fileAccess.GetFloat();
		if (frameRate <= 0.01)
		{
			frameRate = 24.0;
		}
		frameMax = fileAccess.Get16();
		int num = frameMax;
		Vector2I vector2I = new Vector2I(fileAccess.Get16(), fileAccess.Get16());
		_imageAtlasSizeCache = new Vector2(vector2I.X, vector2I.Y);
		int num2 = (int)fileAccess.Get64();
		SkipBytes(fileAccess, num2);
		int num3 = fileAccess.Get16();
		for (int i = 0; i < num3; i++)
		{
			string pascalString = fileAccess.GetPascalString();
			Rect2 item = new Rect2(fileAccess.GetFloat(), fileAccess.GetFloat(), fileAccess.GetFloat(), fileAccess.GetFloat());
			mediaDictionary[pascalString] = i;
			mediaList.Add(item);
		}
		int num4 = fileAccess.Get16();
		for (int j = 0; j < num4; j++)
		{
			string pascalString2 = fileAccess.GetPascalString();
			layerDictionary[pascalString2] = j;
			layerList.Add(pascalString2);
			int num5 = 0;
			for (int k = 0; k < num; k++)
			{
				int num6 = fileAccess.Get16();
				num5 = Mathf.Max(num5, num6);
				SkipBytes(fileAccess, (long)num6 * 30L);
			}
			maxElement += num5;
		}
		int num7 = fileAccess.Get16();
		for (int l = 0; l < num7; l++)
		{
			string pascalString3 = fileAccess.GetPascalString();
			Vector2I vector2I2 = new Vector2I(fileAccess.Get16(), fileAccess.Get16()) * _frameScale;
			clips[pascalString3] = vector2I2;
		}
		int num8 = fileAccess.Get16();
		events.Resize(Mathf.Max(0, num * _frameScale - _frameScale + 1));
		EventFrameIndices = new HashSet<int>();
		for (int m = 0; m < num8; m++)
		{
			int num9 = fileAccess.Get16() * _frameScale;
			int num10 = fileAccess.Get16();
			Godot.Collections.Array array = new Godot.Collections.Array();
			for (int n = 0; n < num10; n++)
			{
				Dictionary dictionary = new Dictionary();
				dictionary["Command"] = fileAccess.GetPascalString();
				dictionary["Argument"] = fileAccess.GetPascalString();
				array.Add(dictionary);
			}
			if (num9 >= 0 && num9 < events.Count)
			{
				events[num9] = array;
				EventFrameIndices.Add(num9);
			}
		}
		fileAccess.Close();
		frameMax = num * _frameScale - _frameScale + 1;
		frameRate *= _frameScale;
		EmitChanged();
		NotifyPropertyListChanged();
		return true;
	}

	public void SetAnimeFileForModImport(string importedAnimeFile)
	{
		_editorResolvedAnimeFilePath = "";
		_animeFile = importedAnimeFile ?? "";
		EmitChanged();
		NotifyPropertyListChanged();
	}

	public void SetEditorResolvedAnimeFilePath(string resolvedAnimeFilePath)
	{
		_editorResolvedAnimeFilePath = NormalizeDatPath(resolvedAnimeFilePath);
	}

	public bool TryInitForEditorAssignedAnimeFile(string sourceDatPath = "")
	{
		string text = ResolveAnimeFilePath(sourceDatPath);
		if (string.IsNullOrWhiteSpace(text) || !Godot.FileAccess.FileExists(text))
		{
			return false;
		}
		if (HasFullDataForDatPath(text))
		{
			return true;
		}
		InitForModImport(text);
		return HasPackedRuntimeData();
	}

	public bool HasPackedRuntimeData()
	{
		int num = sliceKeys?.Length ?? (-1);
		if (num >= 0 && frameOffsets != null && frameOffsets.Length != 0 && frameMax == frameOffsets.Length && frameCounts != null && frameCounts.Length == frameOffsets.Length && sliceMediaIds != null && sliceMediaIds.Length == num && sliceLayerIds != null && sliceLayerIds.Length == num && sliceDrawOrders != null && sliceDrawOrders.Length == num && sliceFlags != null && sliceFlags.Length == num && sliceTransforms != null && sliceTransforms.Length == num * 6 && sliceAlpha != null)
		{
			return sliceAlpha.Length == num;
		}
		return false;
	}

	public bool EnsurePackedRuntimeData()
	{
		if (HasPackedRuntimeData())
		{
			return true;
		}
		LegacyResourceSnapshot snapshot = CaptureLegacySnapshot();
		if (!TryInitForEditorAssignedAnimeFile())
		{
			RestoreLegacySnapshot(snapshot);
			return BuildPackedRuntimeDataFromTimelineData();
		}
		if (HasPackedRuntimeData())
		{
			return true;
		}
		RestoreLegacySnapshot(snapshot);
		return BuildPackedRuntimeDataFromTimelineData();
	}

	private LegacyResourceSnapshot CaptureLegacySnapshot()
	{
		return new LegacyResourceSnapshot
		{
			FrameRate = frameRate,
			FrameMax = frameMax,
			ImageAtlasSizeCache = _imageAtlasSizeCache,
			TimelineData = CloneTimelineArray(timelineData),
			MediaList = CloneRectArray(mediaList),
			LayerList = CloneStringArray(layerList),
			Events = CloneTimelineArray(events),
			Clips = CloneDictionary(clips),
			MediaDictionary = CloneDictionary(mediaDictionary),
			LayerDictionary = CloneDictionary(layerDictionary),
			ReplaceSlotDictionary = CloneDictionary(replaceSlotDictionary),
			MaxElement = maxElement,
			FullDataDatPath = _fullDataDatPath,
			FullDataFrameScale = _fullDataFrameScale
		};
	}

	private void RestoreLegacySnapshot(LegacyResourceSnapshot snapshot)
	{
		if (snapshot != null)
		{
			frameRate = snapshot.FrameRate;
			frameMax = snapshot.FrameMax;
			_imageAtlasSizeCache = snapshot.ImageAtlasSizeCache;
			timelineData = CloneTimelineArray(snapshot.TimelineData);
			mediaList = CloneRectArray(snapshot.MediaList);
			layerList = CloneStringArray(snapshot.LayerList);
			events = CloneTimelineArray(snapshot.Events);
			clips = CloneDictionary(snapshot.Clips);
			mediaDictionary = CloneDictionary(snapshot.MediaDictionary);
			layerDictionary = CloneDictionary(snapshot.LayerDictionary);
			replaceSlotDictionary = CloneDictionary(snapshot.ReplaceSlotDictionary);
			maxElement = snapshot.MaxElement;
			_fullDataDatPath = snapshot.FullDataDatPath;
			_fullDataFrameScale = snapshot.FullDataFrameScale;
			TimelineMediaIdsCache = null;
			TimelineTransformsCache = null;
			EventFrameIndices = null;
			ClearPackedRuntimeData();
		}
	}

	private static Array<Godot.Collections.Array> CloneTimelineArray(Array<Godot.Collections.Array> source)
	{
		Array<Godot.Collections.Array> array = new Array<Godot.Collections.Array>();
		if (source == null)
		{
			return array;
		}
		foreach (Godot.Collections.Array item in source)
		{
			array.Add(item);
		}
		return array;
	}

	private static Array<Rect2> CloneRectArray(Array<Rect2> source)
	{
		Array<Rect2> array = new Array<Rect2>();
		if (source == null)
		{
			return array;
		}
		foreach (Rect2 item in source)
		{
			array.Add(item);
		}
		return array;
	}

	private static Array<string> CloneStringArray(Array<string> source)
	{
		Array<string> array = new Array<string>();
		if (source == null)
		{
			return array;
		}
		foreach (string item in source)
		{
			array.Add(item);
		}
		return array;
	}

	private static Dictionary CloneDictionary(Dictionary source)
	{
		Dictionary dictionary = new Dictionary();
		if (source == null)
		{
			return dictionary;
		}
		foreach (Variant key in source.Keys)
		{
			dictionary[key] = source[key];
		}
		return dictionary;
	}

	private bool HasFullDataForDatPath(string datPath)
	{
		if (HasPackedRuntimeData() && _fullDataDatPath == NormalizeDatPath(datPath))
		{
			return _fullDataFrameScale == _frameScale;
		}
		return false;
	}

	private void InitInternal(bool forceRuntime, string sourceDatPath = "")
	{
		if (!Engine.IsEditorHint() && !forceRuntime)
		{
			return;
		}
		Clear();
		string path = ResolveAnimeFilePath(sourceDatPath);
		if (!Godot.FileAccess.FileExists(path))
		{
			return;
		}
		Godot.FileAccess fileAccess = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
		if (fileAccess == null)
		{
			return;
		}
		frameRate = fileAccess.GetFloat();
		if (frameRate <= 0.01)
		{
			frameRate = 24.0;
		}
		frameMax = fileAccess.Get16();
		Vector2I vector2I = new Vector2I(fileAccess.Get16(), fileAccess.Get16());
		int num = (int)fileAccess.Get64();
		SkipBytes(fileAccess, num);
		_imageAtlasSizeCache = new Vector2(vector2I.X, vector2I.Y);
		int num2 = fileAccess.Get16();
		for (int i = 0; i < num2; i++)
		{
			string pascalString = fileAccess.GetPascalString();
			Rect2 item = new Rect2(fileAccess.GetFloat(), fileAccess.GetFloat(), fileAccess.GetFloat(), fileAccess.GetFloat());
			mediaDictionary[pascalString] = i;
			mediaList.Add(item);
		}
		int num3 = fileAccess.Get16();
		List<List<List<AnimElement>>> list = new List<List<List<AnimElement>>>(num3);
		for (int j = 0; j < num3; j++)
		{
			string pascalString2 = fileAccess.GetPascalString();
			layerDictionary[pascalString2] = j;
			layerList.Add(pascalString2);
			List<List<AnimElement>> list2 = new List<List<AnimElement>>(frameMax);
			for (int k = 0; k < frameMax; k++)
			{
				int num4 = fileAccess.Get16();
				List<AnimElement> list3 = new List<AnimElement>(num4);
				for (int l = 0; l < num4; l++)
				{
					int mediaId = fileAccess.Get16();
					Transform2D transform = new Transform2D(new Vector2(fileAccess.GetFloat(), fileAccess.GetFloat()), new Vector2(fileAccess.GetFloat(), fileAccess.GetFloat()), new Vector2(fileAccess.GetFloat(), fileAccess.GetFloat()));
					uint num5 = fileAccess.Get32();
					Color color = new Color((float)((num5 >> 24) & 0xFF) / 255f, (float)((num5 >> 16) & 0xFF) / 255f, (float)((num5 >> 8) & 0xFF) / 255f, (float)(num5 & 0xFF) / 255f);
					list3.Add(new AnimElement
					{
						MediaId = mediaId,
						Transform = transform,
						Color = color
					});
				}
				list2.Add(list3);
			}
			list.Add(list2);
		}
		int num6 = fileAccess.Get16();
		for (int m = 0; m < num6; m++)
		{
			string pascalString3 = fileAccess.GetPascalString();
			Vector2I vector2I2 = new Vector2I(fileAccess.Get16(), fileAccess.Get16()) * _frameScale;
			clips[pascalString3] = vector2I2;
		}
		int num7 = fileAccess.Get16();
		events.Resize(Mathf.Max(0, frameMax * _frameScale - _frameScale + 1));
		EventFrameIndices = new HashSet<int>();
		for (int n = 0; n < num7; n++)
		{
			int num8 = fileAccess.Get16() * _frameScale;
			int num9 = fileAccess.Get16();
			Godot.Collections.Array array = new Godot.Collections.Array();
			for (int num10 = 0; num10 < num9; num10++)
			{
				Dictionary dictionary = new Dictionary();
				dictionary["Command"] = fileAccess.GetPascalString();
				dictionary["Argument"] = fileAccess.GetPascalString();
				array.Add(dictionary);
			}
			if (num8 >= 0 && num8 < events.Count)
			{
				events[num8] = array;
				EventFrameIndices.Add(num8);
			}
		}
		fileAccess.Close();
		List<List<List<AnimElement>>> list4 = new List<List<List<AnimElement>>>(list.Count);
		List<int> list5 = new List<int>(list.Count);
		maxElement = 0;
		for (int num11 = 0; num11 < list.Count; num11++)
		{
			List<List<AnimElement>> list6 = list[num11];
			List<List<AnimElement>> list7 = new List<List<AnimElement>>(list6.Count * _frameScale);
			int num12 = Mathf.Max(0, list6[list6.Count - 1].Count);
			for (int num13 = 0; num13 < list6.Count - 1; num13++)
			{
				List<AnimElement> list8 = list6[num13];
				List<AnimElement> list9 = list6[num13 + 1];
				list7.Add(list8);
				num12 = Mathf.Max(num12, list8.Count);
				for (int num14 = 0; num14 < _frameScale - 1; num14++)
				{
					List<AnimElement> list10 = new List<AnimElement>(list8.Count);
					for (int num15 = 0; num15 < list8.Count; num15++)
					{
						float weight = (float)(num14 + 1) / (float)_frameScale;
						AnimElement item2 = list8[num15];
						if (list9.Count == list8.Count)
						{
							AnimElement animElement = list9[num15];
							if (animElement.MediaId != 65535)
							{
								Transform2D transform2 = item2.Transform;
								Transform2D transform3 = animElement.Transform;
								Transform2D transform4 = new Transform2D(transform2.X.Lerp(transform3.X, weight), transform2.Y.Lerp(transform3.Y, weight), transform2.Origin.Lerp(transform3.Origin, weight));
								Color color2 = item2.Color;
								Color color3 = animElement.Color;
								list10.Add(new AnimElement
								{
									MediaId = item2.MediaId,
									Transform = transform4,
									Color = color2.Lerp(color3, weight)
								});
							}
							else
							{
								list10.Add(item2);
							}
						}
						else
						{
							list10.Add(item2);
						}
					}
					list7.Add(list10);
				}
			}
			list7.Add(list6[list6.Count - 1]);
			list5.Add(num12);
			maxElement += num12;
			list4.Add(list7);
		}
		frameMax = frameMax * _frameScale - _frameScale + 1;
		frameRate *= _frameScale;
		BuildPackedRuntimeData(list4, list5);
		StripSourceRuntimeData();
		_fullDataDatPath = NormalizeDatPath(path);
		_fullDataFrameScale = _frameScale;
	}

	private void BuildPackedRuntimeData(List<List<List<AnimElement>>> timeline, List<int> layerFrameSizeList)
	{
		ClearPackedRuntimeData();
		List<Vector4> list = new List<Vector4>(mediaList.Count);
		for (int i = 0; i < mediaList.Count; i++)
		{
			Rect2 rect = mediaList[i];
			list.Add(new Vector4(rect.Position.X, rect.Position.Y, rect.Size.X, rect.Size.Y));
		}
		mediaRects = list.ToArray();
		if (timeline == null || timeline.Count == 0 || frameMax <= 0)
		{
			return;
		}
		List<int> list2 = new List<int>(frameMax);
		List<int> list3 = new List<int>(frameMax);
		List<int> list4 = new List<int>();
		List<int> list5 = new List<int>();
		List<int> list6 = new List<int>();
		List<int> list7 = new List<int>();
		List<int> list8 = new List<int>();
		List<float> list9 = new List<float>();
		List<float> list10 = new List<float>();
		int num = 0;
		for (int j = 0; j < frameMax; j++)
		{
			list2.Add(num);
			int num2 = 0;
			int num3 = 0;
			for (int k = 0; k < timeline.Count; k++)
			{
				List<AnimElement> list11 = ((j < timeline[k].Count) ? timeline[k][j] : null);
				int num4 = ((k < layerFrameSizeList.Count) ? layerFrameSizeList[k] : (list11?.Count ?? 0));
				for (int l = 0; l < num4; l++)
				{
					if (list11 != null && l < list11.Count)
					{
						AnimElement animElement = list11[l];
						if (animElement.MediaId != 65535)
						{
							Transform2D transform = animElement.Transform;
							list4.Add((k << 16) ^ (l & 0xFFFF));
							list5.Add(animElement.MediaId);
							list6.Add(k);
							list7.Add(num3);
							list8.Add(0);
							list9.Add(transform.X.X);
							list9.Add(transform.X.Y);
							list9.Add(transform.Y.X);
							list9.Add(transform.Y.Y);
							list9.Add(transform.Origin.X);
							list9.Add(transform.Origin.Y);
							list10.Add(animElement.Color.A);
							num2++;
							num++;
						}
					}
					num3++;
				}
			}
			list3.Add(num2);
		}
		frameOffsets = list2.ToArray();
		frameCounts = list3.ToArray();
		sliceKeys = list4.ToArray();
		sliceMediaIds = list5.ToArray();
		sliceLayerIds = list6.ToArray();
		sliceDrawOrders = list7.ToArray();
		sliceFlags = list8.ToArray();
		sliceTransforms = list9.ToArray();
		sliceAlpha = list10.ToArray();
	}

	private bool BuildPackedRuntimeDataFromTimelineData()
	{
		if (timelineData == null || timelineData.Count == 0)
		{
			return false;
		}
		BuildTimelineCache();
		if (TimelineMediaIdsCache == null || TimelineTransformsCache == null)
		{
			return false;
		}
		ClearPackedRuntimeData();
		List<Vector4> list = new List<Vector4>(mediaList.Count);
		for (int i = 0; i < mediaList.Count; i++)
		{
			Rect2 rect = mediaList[i];
			list.Add(new Vector4(rect.Position.X, rect.Position.Y, rect.Size.X, rect.Size.Y));
		}
		mediaRects = list.ToArray();
		int num = Mathf.Min(TimelineMediaIdsCache.Length, TimelineTransformsCache.Length);
		if (num <= 0)
		{
			return false;
		}
		List<int> list2 = new List<int>(num);
		List<int> list3 = new List<int>(num);
		List<int> list4 = new List<int>();
		List<int> list5 = new List<int>();
		List<int> list6 = new List<int>();
		List<int> list7 = new List<int>();
		List<int> list8 = new List<int>();
		List<float> list9 = new List<float>();
		List<float> list10 = new List<float>();
		int num2 = 0;
		for (int j = 0; j < num; j++)
		{
			list2.Add(num2);
			int num3 = 0;
			int[] array = TimelineMediaIdsCache[j] ?? System.Array.Empty<int>();
			Transform2D[] array2 = TimelineTransformsCache[j] ?? System.Array.Empty<Transform2D>();
			int num4 = Mathf.Min(array.Length, array2.Length);
			for (int k = 0; k < num4; k++)
			{
				int num5 = array[k];
				if (num5 != 65535 && num5 >= 0)
				{
					Transform2D transform2D = array2[k];
					list4.Add(k);
					list5.Add(num5);
					list6.Add(k);
					list7.Add(k);
					list8.Add(0);
					list9.Add(transform2D.X.X);
					list9.Add(transform2D.X.Y);
					list9.Add(transform2D.Y.X);
					list9.Add(transform2D.Y.Y);
					list9.Add(transform2D.Origin.X);
					list9.Add(transform2D.Origin.Y);
					list10.Add(1f);
					num3++;
					num2++;
				}
			}
			list3.Add(num3);
		}
		frameOffsets = list2.ToArray();
		frameCounts = list3.ToArray();
		sliceKeys = list4.ToArray();
		sliceMediaIds = list5.ToArray();
		sliceLayerIds = list6.ToArray();
		sliceDrawOrders = list7.ToArray();
		sliceFlags = list8.ToArray();
		sliceTransforms = list9.ToArray();
		sliceAlpha = list10.ToArray();
		bool flag = HasPackedRuntimeData();
		if (flag)
		{
			StripSourceRuntimeData();
		}
		return flag;
	}

	public void StripSourceRuntimeData()
	{
		timelineData.Clear();
		mediaList.Clear();
		layerList.Clear();
		maxElement = 0;
		TimelineMediaIdsCache = null;
		TimelineTransformsCache = null;
		_imageAtlasSizeCache = Vector2.Zero;
	}

	private void ClearMetadataPreview()
	{
		_imageAtlasSizeCache = Vector2.Zero;
		mediaList.Clear();
		mediaRects = System.Array.Empty<Vector4>();
		layerList.Clear();
		clips.Clear();
		events.Clear();
		mediaDictionary.Clear();
		layerDictionary.Clear();
		replaceSlotDictionary.Clear();
		ClearPackedRuntimeData();
		maxElement = 0;
		TimelineMediaIdsCache = null;
		TimelineTransformsCache = null;
		EventFrameIndices = null;
	}

	private static void SkipBytes(Godot.FileAccess file, long byteCount)
	{
		if (file != null && byteCount > 0)
		{
			file.Seek(file.GetPosition() + (ulong)byteCount);
		}
	}

	private static Image ReadEmbeddedImageAtlasFromDat(Godot.FileAccess file, Vector2I imageAtlasSize, int imageAtlasDataBufferLength, out Vector2I decodedAtlasSize)
	{
		decodedAtlasSize = imageAtlasSize;
		if (file == null || imageAtlasSize.X <= 0 || imageAtlasSize.Y <= 0 || imageAtlasDataBufferLength <= 0)
		{
			decodedAtlasSize = Vector2I.One;
			return CreateTransparentFallbackImage();
		}
		byte[] buffer = file.GetBuffer(imageAtlasDataBufferLength);
		long num = (long)imageAtlasSize.X * (long)imageAtlasSize.Y * 4;
		if (buffer == null || buffer.Length != num)
		{
			decodedAtlasSize = Vector2I.One;
			return CreateTransparentFallbackImage();
		}
		return Image.CreateFromData(imageAtlasSize.X, imageAtlasSize.Y, useMipmaps: false, Image.Format.Rgba8, buffer);
	}

	private static Image CreateTransparentFallbackImage()
	{
		return Image.CreateFromData(1, 1, useMipmaps: false, Image.Format.Rgba8, new byte[4]);
	}

	public string GetResolvedAnimeFilePath(string sourceDatPath = "")
	{
		return ResolveAnimeFilePath(sourceDatPath);
	}

	public string GetAtlasSourceKey()
	{
		string text = ResolveAnimeFilePath();
		if (!string.IsNullOrWhiteSpace(text))
		{
			return NormalizeAtlasSourcePath(text);
		}
		if (!string.IsNullOrWhiteSpace(ResourcePath))
		{
			return NormalizeAtlasSourcePath(ResourcePath);
		}
		return $"instance:{GetInstanceId()}";
	}

	public bool HasEmbeddedAtlasSource()
	{
		string text = ResolveAnimeFilePath();
		if (!string.IsNullOrWhiteSpace(text))
		{
			return Godot.FileAccess.FileExists(text);
		}
		return false;
	}

	public bool TryReadEmbeddedAtlasImage(out Image image, out Vector2I imageAtlasSize)
	{
		image = null;
		imageAtlasSize = Vector2I.Zero;
		string text = ResolveAnimeFilePath();
		if (string.IsNullOrWhiteSpace(text) || !Godot.FileAccess.FileExists(text))
		{
			return false;
		}
		Godot.FileAccess fileAccess = Godot.FileAccess.Open(text, Godot.FileAccess.ModeFlags.Read);
		if (fileAccess == null)
		{
			return false;
		}
		fileAccess.GetFloat();
		fileAccess.Get16();
		imageAtlasSize = new Vector2I(fileAccess.Get16(), fileAccess.Get16());
		int imageAtlasDataBufferLength = (int)fileAccess.Get64();
		image = ReadEmbeddedImageAtlasFromDat(fileAccess, imageAtlasSize, imageAtlasDataBufferLength, out var decodedAtlasSize);
		fileAccess.Close();
		imageAtlasSize = decodedAtlasSize;
		_imageAtlasSizeCache = new Vector2(imageAtlasSize.X, imageAtlasSize.Y);
		if (image != null && imageAtlasSize.X > 0)
		{
			return imageAtlasSize.Y > 0;
		}
		return false;
	}

	public Rect2 GetMediaRect(int mediaId)
	{
		if (mediaRects != null && mediaId >= 0 && mediaId < mediaRects.Length)
		{
			Vector4 vector = mediaRects[mediaId];
			return new Rect2(vector.X, vector.Y, vector.Z, vector.W);
		}
		if (mediaId >= 0 && mediaId < mediaList.Count)
		{
			return mediaList[mediaId];
		}
		return default;
	}

	public string GetLayerHintString(bool includeNoone = false)
	{
		string[] value = BuildLayerNameArray();
		string text = string.Join(",", value);
		if (!includeNoone)
		{
			return text;
		}
		return "Noone," + text;
	}

	private string[] BuildLayerNameArray()
	{
		int layerNameArrayCount = GetLayerNameArrayCount();
		if (layerNameArrayCount <= 0)
		{
			return System.Array.Empty<string>();
		}
		string[] array = new string[layerNameArrayCount];
		if (layerDictionary != null)
		{
			foreach (Variant key in layerDictionary.Keys)
			{
				int num = layerDictionary[key].AsInt32();
				if (num >= 0 && num < array.Length)
				{
					array[num] = key.AsString();
				}
			}
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (string.IsNullOrEmpty(array[i]) && layerList != null && i < layerList.Count)
			{
				array[i] = layerList[i];
			}
			if (string.IsNullOrEmpty(array[i]))
			{
				array[i] = i.ToString();
			}
		}
		return array;
	}

	private int GetLayerNameArrayCount()
	{
		int num = 0;
		if (layerDictionary != null)
		{
			foreach (Variant key in layerDictionary.Keys)
			{
				int num2 = layerDictionary[key].AsInt32();
				if (num2 >= 0)
				{
					num = Mathf.Max(num, num2 + 1);
				}
			}
		}
		if (layerList != null)
		{
			num = Mathf.Max(num, layerList.Count);
		}
		return num;
	}

	private string ResolveAnimeFilePath(string sourceDatPath = "")
	{
		string path = (string.IsNullOrWhiteSpace(sourceDatPath) ? _animeFile : sourceDatPath);
		path = NormalizeDatPath(path);
		if (Godot.FileAccess.FileExists(path))
		{
			return path;
		}
		if (!string.IsNullOrWhiteSpace(ResourcePath) && IsRelativeFilePath(path))
		{
			string text = ResourcePath.GetBaseDir().PathJoin(path);
			if (Godot.FileAccess.FileExists(text))
			{
				return text;
			}
		}
		string text2 = TryResolveOwnerBasenameCompanionDat(path);
		if (!string.IsNullOrWhiteSpace(text2))
		{
			return text2;
		}
		string text3 = NormalizeDatPath(_editorResolvedAnimeFilePath);
		if (string.IsNullOrWhiteSpace(sourceDatPath) && IsRelativeFilePath(path) && !string.IsNullOrWhiteSpace(text3) && string.Equals(path.GetFile(), text3.GetFile(), StringComparison.OrdinalIgnoreCase) && Godot.FileAccess.FileExists(text3))
		{
			return text3;
		}
		string text4 = NormalizeDatPath(_fullDataDatPath);
		if (string.IsNullOrWhiteSpace(sourceDatPath) && IsRelativeFilePath(path) && _fullDataFrameScale == _frameScale && !string.IsNullOrWhiteSpace(text4) && string.Equals(path.GetFile(), text4.GetFile(), StringComparison.OrdinalIgnoreCase) && Godot.FileAccess.FileExists(text4))
		{
			return text4;
		}
		return path;
	}

	private string TryResolveOwnerBasenameCompanionDat(string missingPath)
	{
		if (string.IsNullOrWhiteSpace(ResourcePath))
		{
			return "";
		}
		string baseName = ResourcePath.GetFile().GetBaseName();
		if (string.IsNullOrWhiteSpace(baseName))
		{
			return "";
		}
		string text = ResourcePath.GetBaseDir().PathJoin(baseName + ".dat");
		if (!Godot.FileAccess.FileExists(text))
		{
			return "";
		}
		return text;
	}

	private static string NormalizeDatPath(string path)
	{
		return (path ?? "").Replace('\\', '/');
	}

	private static string NormalizeAtlasSourcePath(string path)
	{
		return ProjectSettings.LocalizePath(NormalizeDatPath(path));
	}

	private static bool IsRelativeFilePath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("user://", StringComparison.OrdinalIgnoreCase) || path.Contains("://"))
		{
			return false;
		}
		return !Path.IsPathRooted(path);
	}

	private static bool ShouldAutoInitAnimeFile(string path)
	{
		if (!Engine.IsEditorHint())
		{
			return true;
		}
		return !IsRelativeFilePath(path);
	}

	public void Clear()
	{
		_hasAuthoredPackedFrames = false;
		_imageAtlasSizeCache = Vector2.Zero;
		_fullDataDatPath = "";
		_fullDataFrameScale = 0;
		mediaList.Clear();
		mediaRects = System.Array.Empty<Vector4>();
		layerList.Clear();
		clips.Clear();
		events.Clear();
		mediaDictionary.Clear();
		layerDictionary.Clear();
		replaceSlotDictionary.Clear();
		timelineData.Clear();
		ClearPackedRuntimeData();
		TimelineMediaIdsCache = null;
		TimelineTransformsCache = null;
		EventFrameIndices = null;
	}

	private void ClearPackedRuntimeData()
	{
		frameOffsets = System.Array.Empty<int>();
		frameCounts = System.Array.Empty<int>();
		sliceKeys = System.Array.Empty<int>();
		sliceMediaIds = System.Array.Empty<int>();
		sliceLayerIds = System.Array.Empty<int>();
		sliceDrawOrders = System.Array.Empty<int>();
		sliceFlags = System.Array.Empty<int>();
		sliceTransforms = System.Array.Empty<float>();
		sliceAlpha = System.Array.Empty<float>();
	}

	public bool HasClip(StringName clipName)
	{
		return clips.ContainsKey(clipName);
	}

	public Vector2I GetClip(StringName clipName)
	{
		if (clips.TryGetValue(clipName, out var value))
		{
			return (Vector2I)value;
		}
		return Vector2I.One * -1;
	}

	public Dictionary CaptureFrameEditSnapshot()
	{
		return CaptureFrameEditSnapshotCore(includeLegacyTimeline: false);
	}

	public Dictionary CaptureFullFrameEditSnapshot()
	{
		return CaptureFrameEditSnapshotCore(includeLegacyTimeline: true);
	}

	private Dictionary CaptureFrameEditSnapshotCore(bool includeLegacyTimeline)
	{
		FrameEditState frameEditState = CaptureFrameEditState(includeLegacyTimeline);
		return new Dictionary
		{
			["version"] = 2,
			["animeFile"] = frameEditState.AnimeFile,
			["frameScale"] = frameEditState.FrameScale,
			["frameRate"] = frameEditState.FrameRate,
			["imageAtlasSizeCache"] = frameEditState.ImageAtlasSizeCache,
			["editorResolvedAnimeFilePath"] = frameEditState.EditorResolvedAnimeFilePath,
			["fullDataDatPath"] = frameEditState.FullDataDatPath,
			["fullDataFrameScale"] = frameEditState.FullDataFrameScale,
			["frameMax"] = frameEditState.FrameMax,
			["frameOffsets"] = frameEditState.FrameOffsets,
			["frameCounts"] = frameEditState.FrameCounts,
			["sliceKeys"] = frameEditState.SliceKeys,
			["sliceMediaIds"] = frameEditState.SliceMediaIds,
			["sliceLayerIds"] = frameEditState.SliceLayerIds,
			["sliceDrawOrders"] = frameEditState.SliceDrawOrders,
			["sliceFlags"] = frameEditState.SliceFlags,
			["sliceTransforms"] = frameEditState.SliceTransforms,
			["sliceAlpha"] = frameEditState.SliceAlpha,
			["mediaRects"] = frameEditState.MediaRects,
			["timelineData"] = CloneFrameEditEvents(frameEditState.TimelineData),
			["mediaList"] = CloneFrameEditRectArray(frameEditState.MediaList),
			["layerList"] = CloneFrameEditStringArray(frameEditState.LayerList),
			["clips"] = CloneFrameEditDictionary(frameEditState.Clips),
			["events"] = CloneFrameEditEvents(frameEditState.Events),
			["mediaDictionary"] = CloneFrameEditDictionary(frameEditState.MediaDictionary),
			["layerDictionary"] = CloneFrameEditDictionary(frameEditState.LayerDictionary),
			["replaceSlotDictionary"] = CloneFrameEditDictionary(frameEditState.ReplaceSlotDictionary),
			["maxElement"] = frameEditState.MaxElement,
			["hasAuthoredPackedFrames"] = frameEditState.HasAuthoredPackedFrames
		};
	}

	public void ApplyFrameEditSnapshot(Dictionary snapshot)
	{
		TryRestoreFrameEditSnapshot(snapshot, out var _);
	}

	public bool TryRestoreFrameEditSnapshot(Dictionary snapshot, out string error)
	{
		if (!TryDecodeFrameEditSnapshot(snapshot, out var state, out error))
		{
			return SetFrameEditFailure(error);
		}
		if (!TryValidateFrameEditState(state, out error))
		{
			return SetFrameEditFailure(error);
		}
		FrameEditState state2 = CaptureFrameEditState(includeLegacyTimeline: true);
		try
		{
			ApplyFrameEditState(state, state.HasAuthoredPackedFrames, incrementRevision: true);
			LastFrameEditError = "";
			return true;
		}
		catch (Exception ex)
		{
			RestoreFrameEditStateWithoutNotification(state2);
			error = "Restoring the frame snapshot failed: " + ex.Message;
			return SetFrameEditFailure(error);
		}
	}

	public bool TryGetFrameSlices(int frameIndex, out FrameSlice[] slices, out string error)
	{
		slices = System.Array.Empty<FrameSlice>();
		FrameEditState frameEditState = CaptureFrameEditState();
		if (!TryValidateFrameEditState(frameEditState, out error))
		{
			return SetFrameEditFailure(error);
		}
		if (frameIndex < 0 || frameIndex >= frameEditState.FrameOffsets.Length)
		{
			error = $"Frame index {frameIndex} is outside 0..{Math.Max(0, frameEditState.FrameOffsets.Length - 1)}.";
			return SetFrameEditFailure(error);
		}
		int num = frameEditState.FrameOffsets[frameIndex];
		int num2 = frameEditState.FrameCounts[frameIndex];
		slices = new FrameSlice[num2];
		for (int i = 0; i < num2; i++)
		{
			slices[i] = ReadFrameSlice(frameEditState, num + i);
		}
		LastFrameEditError = "";
		error = "";
		return true;
	}

	public bool TryGetFrameSlice(int frameIndex, int sliceKey, out FrameSlice slice, out string error)
	{
		slice = default;
		if (!TryGetFrameSlices(frameIndex, out var slices, out error))
		{
			return false;
		}
		for (int i = 0; i < slices.Length; i++)
		{
			if (slices[i].SliceKey == sliceKey)
			{
				slice = slices[i];
				return true;
			}
		}
		error = $"Frame {frameIndex} has no slice with key {sliceKey}.";
		return SetFrameEditFailure(error);
	}

	public bool TryCopyInsertFrame(int sourceFrameIndex, int insertIndex, out string error)
	{
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!TryCreateFrameRecords(state, out var frames, out var error2))
			{
				return error2;
			}
			if (sourceFrameIndex < 0 || sourceFrameIndex >= frames.Count)
			{
				return $"Source frame {sourceFrameIndex} is outside the timeline.";
			}
			if (insertIndex < 0 || insertIndex > frames.Count)
			{
				return $"Insert index {insertIndex} is outside 0..{frames.Count}.";
			}
			frames.Insert(insertIndex, frames[sourceFrameIndex].Clone());
			AdjustClipsForInsert(state.Clips, insertIndex, null);
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryDeleteFrame(int frameIndex, out string error)
	{
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!TryCreateFrameRecords(state, out var frames, out var error2))
			{
				return error2;
			}
			if (frameIndex < 0 || frameIndex >= frames.Count)
			{
				return $"Frame {frameIndex} is outside the timeline.";
			}
			if (frames.Count <= 1)
			{
				return "The final timeline frame cannot be deleted.";
			}
			frames.RemoveAt(frameIndex);
			AdjustClipsForDelete(state.Clips, frameIndex, frames.Count);
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryMoveFrame(int sourceFrameIndex, int destinationFrameIndex, out string error)
	{
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!TryCreateFrameRecords(state, out var frames, out var error2))
			{
				return error2;
			}
			if (sourceFrameIndex < 0 || sourceFrameIndex >= frames.Count)
			{
				return $"Source frame {sourceFrameIndex} is outside the timeline.";
			}
			if (destinationFrameIndex < 0 || destinationFrameIndex >= frames.Count)
			{
				return $"Destination frame {destinationFrameIndex} is outside the timeline.";
			}
			if (sourceFrameIndex == destinationFrameIndex)
			{
				return "";
			}
			FrameRecord item = frames[sourceFrameIndex];
			frames.RemoveAt(sourceFrameIndex);
			frames.Insert(destinationFrameIndex, item);
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryCopyInsertClipFrame(StringName clipName, int sourceLocalIndex, int insertLocalIndex, out string error)
	{
		string selectedClip = clipName.ToString();
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!TryGetClipRange(state, selectedClip, out var range, out var error2))
			{
				return error2;
			}
			int num = range.Y - range.X;
			if (sourceLocalIndex < 0 || sourceLocalIndex >= num)
			{
				return $"Clip-local source frame {sourceLocalIndex} is outside 0..{num - 1}.";
			}
			if (insertLocalIndex < 0 || insertLocalIndex > num)
			{
				return $"Clip-local insert index {insertLocalIndex} is outside 0..{num}.";
			}
			if (!TryCreateFrameRecords(state, out var frames, out error2))
			{
				return error2;
			}
			int index = range.X + sourceLocalIndex;
			int num2 = range.X + insertLocalIndex;
			frames.Insert(num2, frames[index].Clone());
			AdjustClipsForInsert(state.Clips, num2, selectedClip);
			state.Clips[selectedClip] = new Vector2I(range.X, range.Y + 1);
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryDeleteClipFrame(StringName clipName, int localFrameIndex, out string error)
	{
		string selectedClip = clipName.ToString();
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!TryGetClipRange(state, selectedClip, out var range, out var error2))
			{
				return error2;
			}
			int num = range.Y - range.X;
			if (localFrameIndex < 0 || localFrameIndex >= num)
			{
				return $"Clip-local frame {localFrameIndex} is outside 0..{num - 1}.";
			}
			if (num <= 1)
			{
				return "Clip '" + selectedClip + "' must retain at least one frame.";
			}
			if (!TryCreateFrameRecords(state, out var frames, out error2))
			{
				return error2;
			}
			int num2 = range.X + localFrameIndex;
			frames.RemoveAt(num2);
			AdjustClipsForDelete(state.Clips, num2, frames.Count);
			state.Clips[selectedClip] = new Vector2I(range.X, range.Y - 1);
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryMoveClipFrame(StringName clipName, int sourceLocalIndex, int destinationLocalIndex, out string error)
	{
		string selectedClip = clipName.ToString();
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!TryGetClipRange(state, selectedClip, out var range, out var error2))
			{
				return error2;
			}
			int num = range.Y - range.X;
			if (sourceLocalIndex < 0 || sourceLocalIndex >= num)
			{
				return $"Clip-local source frame {sourceLocalIndex} is outside 0..{num - 1}.";
			}
			if (destinationLocalIndex < 0 || destinationLocalIndex >= num)
			{
				return $"Clip-local destination frame {destinationLocalIndex} is outside 0..{num - 1}.";
			}
			if (!TryCreateFrameRecords(state, out var frames, out error2))
			{
				return error2;
			}
			if (sourceLocalIndex == destinationLocalIndex)
			{
				return "";
			}
			int index = range.X + sourceLocalIndex;
			int index2 = range.X + destinationLocalIndex;
			FrameRecord item = frames[index];
			frames.RemoveAt(index);
			frames.Insert(index2, item);
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryUpdateFrameSlice(int frameIndex, int sliceKey, int mediaId, int layerId, Transform2D transform, float alpha, out string error)
	{
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!TryCreateFrameRecords(state, out var frames, out var error2))
			{
				return error2;
			}
			if (frameIndex < 0 || frameIndex >= frames.Count)
			{
				return $"Frame {frameIndex} is outside the timeline.";
			}
			int num = FindSliceKey(frames[frameIndex].Slices, sliceKey);
			if (num < 0)
			{
				return $"Frame {frameIndex} has no slice with key {sliceKey}.";
			}
			FrameSlice frameSlice = frames[frameIndex].Slices[num];
			FrameSlice frameSlice2 = new FrameSlice(sliceKey, mediaId, layerId, frameSlice.DrawOrder, frameSlice.Flags, transform, alpha);
			if (!IsFiniteFrameSlice(frameSlice2))
			{
				return "Slice transform and alpha values must be finite.";
			}
			frames[frameIndex].Slices[num] = frameSlice2;
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryUpdateFrameSlice(int frameIndex, int sliceKey, FrameSlice replacement, out string error)
	{
		return TryMutateFrames((FrameEditState state) =>
		{
			if (replacement.SliceKey != sliceKey)
			{
				return "A slice replacement must retain the located slice key.";
			}
			if (!IsFiniteFrameSlice(replacement))
			{
				return "Slice transform and alpha values must be finite.";
			}
			if (!TryCreateFrameRecords(state, out var frames, out var error2))
			{
				return error2;
			}
			if (frameIndex < 0 || frameIndex >= frames.Count)
			{
				return $"Frame {frameIndex} is outside the timeline.";
			}
			int num = FindSliceKey(frames[frameIndex].Slices, sliceKey);
			if (num < 0)
			{
				return $"Frame {frameIndex} has no slice with key {sliceKey}.";
			}
			frames[frameIndex].Slices[num] = replacement;
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryAddFrameSlice(int frameIndex, FrameSlice slice, out string error)
	{
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!IsFiniteFrameSlice(slice))
			{
				return "Slice transform and alpha values must be finite.";
			}
			if (!TryCreateFrameRecords(state, out var frames, out var error2))
			{
				return error2;
			}
			if (frameIndex < 0 || frameIndex >= frames.Count)
			{
				return $"Frame {frameIndex} is outside the timeline.";
			}
			if (FindSliceKey(frames[frameIndex].Slices, slice.SliceKey) >= 0)
			{
				return $"Frame {frameIndex} already contains slice key {slice.SliceKey}.";
			}
			frames[frameIndex].Slices.Add(slice);
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryUpsertFrameSlice(int frameIndex, FrameSlice slice, out string error)
	{
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!IsFiniteFrameSlice(slice))
			{
				return "Slice transform and alpha values must be finite.";
			}
			if (!TryCreateFrameRecords(state, out var frames, out var error2))
			{
				return error2;
			}
			if (frameIndex < 0 || frameIndex >= frames.Count)
			{
				return $"Frame {frameIndex} is outside the timeline.";
			}
			int num = FindSliceKey(frames[frameIndex].Slices, slice.SliceKey);
			if (num >= 0)
			{
				frames[frameIndex].Slices[num] = slice;
			}
			else
			{
				frames[frameIndex].Slices.Add(slice);
			}
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryCopyFrameSlice(int sourceFrameIndex, int destinationFrameIndex, int sliceKey, out string error)
	{
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!TryCreateFrameRecords(state, out var frames, out var error2))
			{
				return error2;
			}
			if (!TryResolveFrameSliceMove(frames, sourceFrameIndex, destinationFrameIndex, sliceKey, out var sourceLocalIndex, out error2))
			{
				return error2;
			}
			if (FindSliceKey(frames[destinationFrameIndex].Slices, sliceKey) >= 0)
			{
				return $"Frame {destinationFrameIndex} already contains slice key {sliceKey}.";
			}
			frames[destinationFrameIndex].Slices.Add(frames[sourceFrameIndex].Slices[sourceLocalIndex]);
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryMoveFrameSlice(int sourceFrameIndex, int destinationFrameIndex, int sliceKey, out string error)
	{
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!TryCreateFrameRecords(state, out var frames, out var error2))
			{
				return error2;
			}
			if (!TryResolveFrameSliceMove(frames, sourceFrameIndex, destinationFrameIndex, sliceKey, out var sourceLocalIndex, out error2))
			{
				return error2;
			}
			if (sourceFrameIndex == destinationFrameIndex)
			{
				return "";
			}
			if (FindSliceKey(frames[destinationFrameIndex].Slices, sliceKey) >= 0)
			{
				return $"Frame {destinationFrameIndex} already contains slice key {sliceKey}.";
			}
			FrameSlice item = frames[sourceFrameIndex].Slices[sourceLocalIndex];
			frames[sourceFrameIndex].Slices.RemoveAt(sourceLocalIndex);
			frames[destinationFrameIndex].Slices.Add(item);
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryRemoveFrameSlice(int frameIndex, int sliceKey, out string error)
	{
		return TryMutateFrames((FrameEditState state) =>
		{
			if (!TryCreateFrameRecords(state, out var frames, out var error2))
			{
				return error2;
			}
			if (frameIndex < 0 || frameIndex >= frames.Count)
			{
				return $"Frame {frameIndex} is outside the timeline.";
			}
			int num = FindSliceKey(frames[frameIndex].Slices, sliceKey);
			if (num < 0)
			{
				return $"Frame {frameIndex} has no slice with key {sliceKey}.";
			}
			frames[frameIndex].Slices.RemoveAt(num);
			RebuildPackedArrays(state, frames);
			return "";
		}, out error);
	}

	public bool TryReimportSourceFrames(out string error)
	{
		FrameEditState state = CaptureFrameEditState(includeLegacyTimeline: true);
		LegacyResourceSnapshot snapshot = CaptureLegacySnapshot();
		string text = ResolveAnimeFilePath();
		if (string.IsNullOrWhiteSpace(text) || !Godot.FileAccess.FileExists(text))
		{
			error = "The assigned DAT source cannot be resolved.";
			return SetFrameEditFailure(error);
		}
		try
		{
			EditorDatHydrationResult result = BuildEditorDatHydrationAsync(text).GetAwaiter().GetResult();
			if (result == null || !result.IsSuccess)
			{
				throw new InvalidOperationException(result?.ErrorMessage ?? "The DAT source could not be parsed.");
			}
			_hasAuthoredPackedFrames = false;
			if (!ApplyEditorDatHydration(result) || !HasPackedRuntimeData())
			{
				throw new InvalidOperationException("The DAT source did not produce valid packed frames.");
			}
			_authoringRevision++;
			InvalidateFrameAuthoringCaches();
			EmitChanged();
			NotifyPropertyListChanged();
			LastFrameEditError = "";
			error = "";
			return true;
		}
		catch (Exception ex)
		{
			RestoreLegacySnapshot(snapshot);
			RestoreFrameEditStateWithoutNotification(state);
			error = "DAT re-import failed: " + ex.Message;
			return SetFrameEditFailure(error);
		}
	}

	public void ClearFrameAuthoringFlag()
	{
		if (_hasAuthoredPackedFrames)
		{
			_hasAuthoredPackedFrames = false;
			_authoringRevision++;
			EmitChanged();
			NotifyPropertyListChanged();
		}
	}

	public bool ValidatePackedFrameEditingState(out string error)
	{
		bool flag = TryValidateFrameEditState(CaptureFrameEditState(), out error);
		if (flag)
		{
			LastFrameEditError = "";
			return flag;
		}
		LastFrameEditError = error;
		return flag;
	}

	private bool TryMutateFrames(FrameStateMutation mutation, out string error)
	{
		FrameEditState frameEditState = CaptureFrameEditState(includeLegacyTimeline: false, cloneContainers: false);
		if (!TryValidateFrameEditState(frameEditState, out error))
		{
			return SetFrameEditFailure(error);
		}
		FrameEditState state = frameEditState.Clone(includeLegacyTimeline: false);
		NormalizeFrameEditEvents(state);
		try
		{
			string text = mutation?.Invoke(state) ?? "No frame mutation was supplied.";
			if (!string.IsNullOrEmpty(text))
			{
				error = text;
				return SetFrameEditFailure(error);
			}
			if (!TryValidateFrameEditState(state, out error))
			{
				return SetFrameEditFailure(error);
			}
			ApplyFrameEditState(state, authored: true, incrementRevision: true);
			LastFrameEditError = "";
			error = "";
			return true;
		}
		catch (Exception ex)
		{
			RestoreFrameEditStateWithoutNotification(frameEditState);
			error = "Frame mutation failed atomically: " + ex.Message;
			return SetFrameEditFailure(error);
		}
	}

	private bool SetFrameEditFailure(string error)
	{
		LastFrameEditError = error ?? "Unknown frame editing error.";
		return false;
	}

	private FrameEditState CaptureFrameEditState(bool includeLegacyTimeline = false, bool cloneContainers = true)
	{
		return new FrameEditState
		{
			AnimeFile = _animeFile,
			FrameScale = _frameScale,
			FrameRate = frameRate,
			ImageAtlasSizeCache = _imageAtlasSizeCache,
			EditorResolvedAnimeFilePath = _editorResolvedAnimeFilePath,
			FullDataDatPath = _fullDataDatPath,
			FullDataFrameScale = _fullDataFrameScale,
			FrameMax = frameMax,
			FrameOffsets = (cloneContainers ? CloneFrameEditArray(frameOffsets) : frameOffsets),
			FrameCounts = (cloneContainers ? CloneFrameEditArray(frameCounts) : frameCounts),
			SliceKeys = (cloneContainers ? CloneFrameEditArray(sliceKeys) : sliceKeys),
			SliceMediaIds = (cloneContainers ? CloneFrameEditArray(sliceMediaIds) : sliceMediaIds),
			SliceLayerIds = (cloneContainers ? CloneFrameEditArray(sliceLayerIds) : sliceLayerIds),
			SliceDrawOrders = (cloneContainers ? CloneFrameEditArray(sliceDrawOrders) : sliceDrawOrders),
			SliceFlags = (cloneContainers ? CloneFrameEditArray(sliceFlags) : sliceFlags),
			SliceTransforms = (cloneContainers ? CloneFrameEditArray(sliceTransforms) : sliceTransforms),
			SliceAlpha = (cloneContainers ? CloneFrameEditArray(sliceAlpha) : sliceAlpha),
			MediaRects = (cloneContainers ? CloneFrameEditArray(mediaRects) : mediaRects),
			TimelineData = ((!cloneContainers) ? timelineData : (includeLegacyTimeline ? CloneFrameEditEvents(timelineData) : new Array<Godot.Collections.Array>())),
			MediaList = (cloneContainers ? CloneFrameEditRectArray(mediaList) : mediaList),
			LayerList = (cloneContainers ? CloneFrameEditStringArray(layerList) : layerList),
			Clips = (cloneContainers ? CloneFrameEditDictionary(clips) : clips),
			Events = (cloneContainers ? CloneFrameEditEvents(events) : events),
			MediaDictionary = (cloneContainers ? CloneFrameEditDictionary(mediaDictionary) : mediaDictionary),
			LayerDictionary = (cloneContainers ? CloneFrameEditDictionary(layerDictionary) : layerDictionary),
			ReplaceSlotDictionary = (cloneContainers ? CloneFrameEditDictionary(replaceSlotDictionary) : replaceSlotDictionary),
			MaxElement = maxElement,
			HasAuthoredPackedFrames = _hasAuthoredPackedFrames
		};
	}

	private void ApplyFrameEditState(FrameEditState state, bool authored, bool incrementRevision)
	{
		AdobeAnimateDefinitionCache.Invalidate(this);
		_animeFile = state.AnimeFile ?? "";
		_frameScale = state.FrameScale;
		frameRate = state.FrameRate;
		_imageAtlasSizeCache = state.ImageAtlasSizeCache;
		_editorResolvedAnimeFilePath = state.EditorResolvedAnimeFilePath ?? "";
		_fullDataDatPath = state.FullDataDatPath ?? "";
		_fullDataFrameScale = state.FullDataFrameScale;
		frameMax = state.FrameMax;
		frameOffsets = CloneFrameEditArray(state.FrameOffsets);
		frameCounts = CloneFrameEditArray(state.FrameCounts);
		sliceKeys = CloneFrameEditArray(state.SliceKeys);
		sliceMediaIds = CloneFrameEditArray(state.SliceMediaIds);
		sliceLayerIds = CloneFrameEditArray(state.SliceLayerIds);
		sliceDrawOrders = CloneFrameEditArray(state.SliceDrawOrders);
		sliceFlags = CloneFrameEditArray(state.SliceFlags);
		sliceTransforms = CloneFrameEditArray(state.SliceTransforms);
		sliceAlpha = CloneFrameEditArray(state.SliceAlpha);
		mediaRects = CloneFrameEditArray(state.MediaRects);
		timelineData = CloneFrameEditEvents(state.TimelineData);
		mediaList = CloneFrameEditRectArray(state.MediaList);
		layerList = CloneFrameEditStringArray(state.LayerList);
		clips = CloneFrameEditDictionary(state.Clips);
		events = CloneFrameEditEvents(state.Events);
		mediaDictionary = CloneFrameEditDictionary(state.MediaDictionary);
		layerDictionary = CloneFrameEditDictionary(state.LayerDictionary);
		replaceSlotDictionary = CloneFrameEditDictionary(state.ReplaceSlotDictionary);
		maxElement = state.MaxElement;
		_hasAuthoredPackedFrames = authored;
		if (incrementRevision)
		{
			_authoringRevision++;
		}
		RebuildFrameEditEventIndex();
		InvalidateFrameAuthoringCaches();
		EmitChanged();
		NotifyPropertyListChanged();
	}

	private void RestoreFrameEditStateWithoutNotification(FrameEditState state)
	{
		if (state != null)
		{
			AdobeAnimateDefinitionCache.Invalidate(this);
			_animeFile = state.AnimeFile ?? "";
			_frameScale = state.FrameScale;
			frameRate = state.FrameRate;
			_imageAtlasSizeCache = state.ImageAtlasSizeCache;
			_editorResolvedAnimeFilePath = state.EditorResolvedAnimeFilePath ?? "";
			_fullDataDatPath = state.FullDataDatPath ?? "";
			_fullDataFrameScale = state.FullDataFrameScale;
			frameMax = state.FrameMax;
			frameOffsets = CloneFrameEditArray(state.FrameOffsets);
			frameCounts = CloneFrameEditArray(state.FrameCounts);
			sliceKeys = CloneFrameEditArray(state.SliceKeys);
			sliceMediaIds = CloneFrameEditArray(state.SliceMediaIds);
			sliceLayerIds = CloneFrameEditArray(state.SliceLayerIds);
			sliceDrawOrders = CloneFrameEditArray(state.SliceDrawOrders);
			sliceFlags = CloneFrameEditArray(state.SliceFlags);
			sliceTransforms = CloneFrameEditArray(state.SliceTransforms);
			sliceAlpha = CloneFrameEditArray(state.SliceAlpha);
			mediaRects = CloneFrameEditArray(state.MediaRects);
			timelineData = CloneFrameEditEvents(state.TimelineData);
			mediaList = CloneFrameEditRectArray(state.MediaList);
			layerList = CloneFrameEditStringArray(state.LayerList);
			clips = CloneFrameEditDictionary(state.Clips);
			events = CloneFrameEditEvents(state.Events);
			mediaDictionary = CloneFrameEditDictionary(state.MediaDictionary);
			layerDictionary = CloneFrameEditDictionary(state.LayerDictionary);
			replaceSlotDictionary = CloneFrameEditDictionary(state.ReplaceSlotDictionary);
			maxElement = state.MaxElement;
			_hasAuthoredPackedFrames = state.HasAuthoredPackedFrames;
			RebuildFrameEditEventIndex();
			InvalidateFrameAuthoringCaches();
		}
	}

	private void InvalidateFrameAuthoringCaches()
	{
		TimelineMediaIdsCache = null;
		TimelineTransformsCache = null;
		AdobeAnimateDefinitionCache.Invalidate(this);
		AdobeAnimateRuntimeManager.InvalidateRenderRoots();
	}

	private void RebuildFrameEditEventIndex()
	{
		EventFrameIndices = new HashSet<int>();
		if (events == null)
		{
			return;
		}
		for (int i = 0; i < events.Count; i++)
		{
			Godot.Collections.Array array = events[i];
			if (array != null && array.Count > 0)
			{
				EventFrameIndices.Add(i);
			}
		}
	}

	private static bool TryValidateFrameEditState(FrameEditState state, out string error)
	{
		if (state == null)
		{
			error = "Frame state is null.";
			return false;
		}
		if (state.FrameOffsets == null || state.FrameCounts == null)
		{
			error = "Frame offset/count arrays are missing.";
			return false;
		}
		if (state.FrameOffsets.Length != state.FrameCounts.Length)
		{
			error = "Frame offset/count arrays have different lengths.";
			return false;
		}
		if (state.FrameMax != state.FrameOffsets.Length)
		{
			error = $"frameMax ({state.FrameMax}) does not match packed frame count ({state.FrameOffsets.Length}).";
			return false;
		}
		int num = state.SliceKeys?.Length ?? (-1);
		if (num < 0 || state.SliceMediaIds == null || state.SliceMediaIds.Length != num || state.SliceLayerIds == null || state.SliceLayerIds.Length != num || state.SliceDrawOrders == null || state.SliceDrawOrders.Length != num || state.SliceFlags == null || state.SliceFlags.Length != num || state.SliceAlpha == null || state.SliceAlpha.Length != num || state.SliceTransforms == null || state.SliceTransforms.Length != num * 6)
		{
			error = "Packed per-slice arrays do not share one exact slice count (transform must be 6N).";
			return false;
		}
		int num2 = 0;
		for (int i = 0; i < state.FrameOffsets.Length; i++)
		{
			int num3 = state.FrameCounts[i];
			if (num3 < 0 || state.FrameOffsets[i] != num2 || num3 > num - num2)
			{
				error = $"Frame {i} breaks contiguous offset/count invariants.";
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			for (int j = 0; j < num3; j++)
			{
				int num4 = num2 + j;
				if (!hashSet.Add(state.SliceKeys[num4]))
				{
					error = $"Frame {i} contains duplicate slice key {state.SliceKeys[num4]}.";
					return false;
				}
				if (!IsFiniteFrameSlice(ReadFrameSlice(state, num4)))
				{
					error = $"Frame {i} contains a non-finite transform or alpha.";
					return false;
				}
			}
			num2 += num3;
		}
		if (num2 != num)
		{
			error = "Frame ranges do not cover every packed slice exactly once.";
			return false;
		}
		if (state.Events == null || state.Events.Count != state.FrameOffsets.Length)
		{
			error = "Event frame count does not match packed frame count.";
			return false;
		}
		if (!TryValidateClipRanges(state, out error))
		{
			return false;
		}
		error = "";
		return true;
	}

	private static bool TryValidateClipRanges(FrameEditState state, out string error)
	{
		if (state.Clips == null)
		{
			error = "Clip dictionary is null.";
			return false;
		}
		foreach (Variant key in state.Clips.Keys)
		{
			Variant variant = state.Clips[key];
			if (variant.VariantType != Variant.Type.Vector2I)
			{
				error = "Clip '" + key.AsString() + "' does not contain a Vector2I range.";
				return false;
			}
			Vector2I value = variant.AsVector2I();
			if (value.X < 0 || value.Y <= value.X || value.Y > state.FrameOffsets.Length)
			{
				error = $"Clip '{key.AsString()}' range {value} is outside [0,{state.FrameOffsets.Length}).";
				return false;
			}
		}
		error = "";
		return true;
	}

	private static bool TryCreateFrameRecords(FrameEditState state, out List<FrameRecord> frames, out string error)
	{
		if (state?.FrameOffsets == null || state.FrameCounts == null)
		{
			frames = new List<FrameRecord>();
			error = "Frame state is missing prevalidated offset/count arrays.";
			return false;
		}
		frames = new List<FrameRecord>(state.FrameOffsets.Length);
		for (int i = 0; i < state.FrameOffsets.Length; i++)
		{
			int num = state.FrameOffsets[i];
			int num2 = state.FrameCounts[i];
			List<FrameSlice> list = new List<FrameSlice>(num2);
			for (int j = 0; j < num2; j++)
			{
				list.Add(ReadFrameSlice(state, num + j));
			}
			frames.Add(new FrameRecord(list, CloneFrameEditEventFrame(state.Events[i])));
		}
		error = "";
		return true;
	}

	private static void RebuildPackedArrays(FrameEditState state, List<FrameRecord> frames)
	{
		List<int> list = new List<int>(frames.Count);
		List<int> list2 = new List<int>(frames.Count);
		List<int> list3 = new List<int>();
		List<int> list4 = new List<int>();
		List<int> list5 = new List<int>();
		List<int> list6 = new List<int>();
		List<int> list7 = new List<int>();
		List<float> list8 = new List<float>();
		List<float> list9 = new List<float>();
		Array<Godot.Collections.Array> array = new Array<Godot.Collections.Array>();
		for (int i = 0; i < frames.Count; i++)
		{
			FrameRecord frameRecord = frames[i];
			list.Add(list3.Count);
			list2.Add(frameRecord.Slices.Count);
			foreach (FrameSlice slice in frameRecord.Slices)
			{
				list3.Add(slice.SliceKey);
				list4.Add(slice.MediaId);
				list5.Add(slice.LayerId);
				list6.Add(slice.DrawOrder);
				list7.Add(slice.Flags);
				list8.Add(slice.Transform.X.X);
				list8.Add(slice.Transform.X.Y);
				list8.Add(slice.Transform.Y.X);
				list8.Add(slice.Transform.Y.Y);
				list8.Add(slice.Transform.Origin.X);
				list8.Add(slice.Transform.Origin.Y);
				list9.Add(slice.Alpha);
			}
			array.Add(CloneFrameEditEventFrame(frameRecord.Events));
		}
		state.FrameMax = frames.Count;
		state.FrameOffsets = list.ToArray();
		state.FrameCounts = list2.ToArray();
		state.SliceKeys = list3.ToArray();
		state.SliceMediaIds = list4.ToArray();
		state.SliceLayerIds = list5.ToArray();
		state.SliceDrawOrders = list6.ToArray();
		state.SliceFlags = list7.ToArray();
		state.SliceTransforms = list8.ToArray();
		state.SliceAlpha = list9.ToArray();
		state.Events = array;
	}

	private static FrameSlice ReadFrameSlice(FrameEditState state, int absoluteIndex)
	{
		int num = absoluteIndex * 6;
		return new FrameSlice(state.SliceKeys[absoluteIndex], state.SliceMediaIds[absoluteIndex], state.SliceLayerIds[absoluteIndex], state.SliceDrawOrders[absoluteIndex], state.SliceFlags[absoluteIndex], new Transform2D(state.SliceTransforms[num], state.SliceTransforms[num + 1], state.SliceTransforms[num + 2], state.SliceTransforms[num + 3], state.SliceTransforms[num + 4], state.SliceTransforms[num + 5]), state.SliceAlpha[absoluteIndex]);
	}

	private static bool IsFiniteFrameSlice(FrameSlice slice)
	{
		Transform2D transform = slice.Transform;
		if (float.IsFinite(transform.X.X) && float.IsFinite(transform.X.Y) && float.IsFinite(transform.Y.X) && float.IsFinite(transform.Y.Y) && float.IsFinite(transform.Origin.X) && float.IsFinite(transform.Origin.Y))
		{
			return float.IsFinite(slice.Alpha);
		}
		return false;
	}

	private static int FindSliceKey(List<FrameSlice> slices, int sliceKey)
	{
		for (int i = 0; i < slices.Count; i++)
		{
			if (slices[i].SliceKey == sliceKey)
			{
				return i;
			}
		}
		return -1;
	}

	private static bool TryResolveFrameSliceMove(List<FrameRecord> frames, int sourceFrameIndex, int destinationFrameIndex, int sliceKey, out int sourceLocalIndex, out string error)
	{
		sourceLocalIndex = -1;
		if (sourceFrameIndex < 0 || sourceFrameIndex >= frames.Count)
		{
			error = $"Source frame {sourceFrameIndex} is outside the timeline.";
			return false;
		}
		if (destinationFrameIndex < 0 || destinationFrameIndex >= frames.Count)
		{
			error = $"Destination frame {destinationFrameIndex} is outside the timeline.";
			return false;
		}
		sourceLocalIndex = FindSliceKey(frames[sourceFrameIndex].Slices, sliceKey);
		if (sourceLocalIndex < 0)
		{
			error = $"Frame {sourceFrameIndex} has no slice with key {sliceKey}.";
			return false;
		}
		error = "";
		return true;
	}

	private static bool TryGetClipRange(FrameEditState state, string clipName, out Vector2I range, out string error)
	{
		range = default;
		if (string.IsNullOrWhiteSpace(clipName) || state.Clips == null || !state.Clips.TryGetValue(clipName, out var value))
		{
			error = "Clip '" + clipName + "' does not exist.";
			return false;
		}
		if (value.VariantType != Variant.Type.Vector2I)
		{
			error = "Clip '" + clipName + "' has an invalid range value.";
			return false;
		}
		range = value.AsVector2I();
		if (range.X < 0 || range.Y <= range.X || range.Y > state.FrameOffsets.Length)
		{
			error = $"Clip '{clipName}' range {range} is outside the timeline.";
			return false;
		}
		error = "";
		return true;
	}

	private static void AdjustClipsForInsert(Dictionary clipDictionary, int insertIndex, string targetClip)
	{
		if (clipDictionary == null)
		{
			return;
		}
		foreach (Variant key in clipDictionary.Keys)
		{
			string a = key.AsString();
			if (string.IsNullOrEmpty(targetClip) || !string.Equals(a, targetClip, StringComparison.Ordinal))
			{
				Vector2I vector2I = clipDictionary[key].AsVector2I();
				if (insertIndex <= vector2I.X)
				{
					vector2I += Vector2I.One;
				}
				else if (insertIndex < vector2I.Y)
				{
					vector2I.Y++;
				}
				clipDictionary[key] = vector2I;
			}
		}
	}

	private static void AdjustClipsForDelete(Dictionary clipDictionary, int deletedIndex, int remainingFrameCount)
	{
		if (clipDictionary == null)
		{
			return;
		}
		foreach (Variant key in clipDictionary.Keys)
		{
			Vector2I vector2I = clipDictionary[key].AsVector2I();
			if (deletedIndex < vector2I.X)
			{
				vector2I -= Vector2I.One;
			}
			else if (deletedIndex >= vector2I.X && deletedIndex < vector2I.Y)
			{
				if (vector2I.Y - vector2I.X > 1)
				{
					vector2I.Y--;
				}
				else
				{
					int num = Math.Clamp(deletedIndex, 0, Math.Max(0, remainingFrameCount - 1));
					vector2I = new Vector2I(num, num + 1);
				}
			}
			clipDictionary[key] = vector2I;
		}
	}

	private static void NormalizeFrameEditEvents(FrameEditState state)
	{
		if (state.Events == null)
		{
			state.Events = new Array<Godot.Collections.Array>();
		}
		while (state.Events.Count < state.FrameOffsets.Length)
		{
			state.Events.Add(new Godot.Collections.Array());
		}
		while (state.Events.Count > state.FrameOffsets.Length)
		{
			state.Events.RemoveAt(state.Events.Count - 1);
		}
		for (int i = 0; i < state.Events.Count; i++)
		{
			Array<Godot.Collections.Array> array = state.Events;
			int index = i;
			if (array[index] == null)
			{
				Godot.Collections.Array array2 = (array[index] = new Godot.Collections.Array());
			}
		}
	}

	private static bool TryDecodeFrameEditSnapshot(Dictionary snapshot, out FrameEditState state, out string error)
	{
		state = null;
		if (snapshot == null || !TryReadSnapshotInt(snapshot, "version", out var value) || value != 2)
		{
			error = "Frame snapshot version is missing or unsupported.";
			return false;
		}
		if (!TryReadSnapshotInt(snapshot, "frameMax", out var value2) || !TryReadSnapshotString(snapshot, "animeFile", out var value3) || !TryReadSnapshotInt(snapshot, "frameScale", out var value4) || !TryReadSnapshotDouble(snapshot, "frameRate", out var value5) || !TryReadSnapshotVector2(snapshot, "imageAtlasSizeCache", out var value6) || !TryReadSnapshotString(snapshot, "editorResolvedAnimeFilePath", out var value7) || !TryReadSnapshotString(snapshot, "fullDataDatPath", out var value8) || !TryReadSnapshotInt(snapshot, "fullDataFrameScale", out var value9) || !TryReadSnapshotIntArray(snapshot, "frameOffsets", out var value10) || !TryReadSnapshotIntArray(snapshot, "frameCounts", out var value11) || !TryReadSnapshotIntArray(snapshot, "sliceKeys", out var value12) || !TryReadSnapshotIntArray(snapshot, "sliceMediaIds", out var value13) || !TryReadSnapshotIntArray(snapshot, "sliceLayerIds", out var value14) || !TryReadSnapshotIntArray(snapshot, "sliceDrawOrders", out var value15) || !TryReadSnapshotIntArray(snapshot, "sliceFlags", out var value16) || !TryReadSnapshotFloatArray(snapshot, "sliceTransforms", out var value17) || !TryReadSnapshotFloatArray(snapshot, "sliceAlpha", out var value18) || !TryReadSnapshotVector4Array(snapshot, "mediaRects", out var value19) || !TryReadSnapshotEvents(snapshot, "timelineData", out var value20) || !TryReadSnapshotRectArray(snapshot, "mediaList", out var value21) || !TryReadSnapshotStringArray(snapshot, "layerList", out var value22) || !TryReadSnapshotDictionary(snapshot, "clips", out var value23) || !TryReadSnapshotEvents(snapshot, "events", out var value24) || !TryReadSnapshotDictionary(snapshot, "mediaDictionary", out var value25) || !TryReadSnapshotDictionary(snapshot, "layerDictionary", out var value26) || !TryReadSnapshotDictionary(snapshot, "replaceSlotDictionary", out var value27) || !TryReadSnapshotInt(snapshot, "maxElement", out var value28))
		{
			error = "Frame snapshot is missing a required packed field or has a wrong Variant type.";
			return false;
		}
		bool hasAuthoredPackedFrames = snapshot.TryGetValue("hasAuthoredPackedFrames", out var value29) && value29.VariantType == Variant.Type.Bool && value29.AsBool();
		state = new FrameEditState
		{
			AnimeFile = value3,
			FrameScale = value4,
			FrameRate = value5,
			ImageAtlasSizeCache = value6,
			EditorResolvedAnimeFilePath = value7,
			FullDataDatPath = value8,
			FullDataFrameScale = value9,
			FrameMax = value2,
			FrameOffsets = value10,
			FrameCounts = value11,
			SliceKeys = value12,
			SliceMediaIds = value13,
			SliceLayerIds = value14,
			SliceDrawOrders = value15,
			SliceFlags = value16,
			SliceTransforms = value17,
			SliceAlpha = value18,
			MediaRects = value19,
			TimelineData = value20,
			MediaList = value21,
			LayerList = value22,
			Clips = value23,
			Events = value24,
			MediaDictionary = value25,
			LayerDictionary = value26,
			ReplaceSlotDictionary = value27,
			MaxElement = value28,
			HasAuthoredPackedFrames = hasAuthoredPackedFrames
		};
		error = "";
		return true;
	}

	private static bool TryReadSnapshotInt(Dictionary snapshot, string key, out int value)
	{
		value = 0;
		if (!snapshot.TryGetValue(key, out var value2) || (value2.VariantType != Variant.Type.Int && value2.VariantType != Variant.Type.Float))
		{
			return false;
		}
		value = value2.AsInt32();
		return true;
	}

	private static bool TryReadSnapshotDouble(Dictionary snapshot, string key, out double value)
	{
		value = 0.0;
		if (!snapshot.TryGetValue(key, out var value2) || (value2.VariantType != Variant.Type.Float && value2.VariantType != Variant.Type.Int))
		{
			return false;
		}
		value = value2.AsDouble();
		return double.IsFinite(value);
	}

	private static bool TryReadSnapshotString(Dictionary snapshot, string key, out string value)
	{
		value = "";
		if (!snapshot.TryGetValue(key, out var value2) || value2.VariantType != Variant.Type.String)
		{
			return false;
		}
		value = value2.AsString();
		return true;
	}

	private static bool TryReadSnapshotVector2(Dictionary snapshot, string key, out Vector2 value)
	{
		value = Vector2.Zero;
		if (!snapshot.TryGetValue(key, out var value2) || value2.VariantType != Variant.Type.Vector2)
		{
			return false;
		}
		value = value2.AsVector2();
		if (float.IsFinite(value.X))
		{
			return float.IsFinite(value.Y);
		}
		return false;
	}

	private static bool TryReadSnapshotIntArray(Dictionary snapshot, string key, out int[] value)
	{
		value = System.Array.Empty<int>();
		if (!snapshot.TryGetValue(key, out var value2) || value2.VariantType != Variant.Type.PackedInt32Array)
		{
			return false;
		}
		value = CloneFrameEditArray(value2.AsInt32Array());
		return true;
	}

	private static bool TryReadSnapshotFloatArray(Dictionary snapshot, string key, out float[] value)
	{
		value = System.Array.Empty<float>();
		if (!snapshot.TryGetValue(key, out var value2) || value2.VariantType != Variant.Type.PackedFloat32Array)
		{
			return false;
		}
		value = CloneFrameEditArray(value2.AsFloat32Array());
		return true;
	}

	private static bool TryReadSnapshotVector4Array(Dictionary snapshot, string key, out Vector4[] value)
	{
		value = System.Array.Empty<Vector4>();
		if (!snapshot.TryGetValue(key, out var value2) || value2.VariantType != Variant.Type.PackedVector4Array)
		{
			return false;
		}
		value = CloneFrameEditArray(value2.AsVector4Array());
		return true;
	}

	private static bool TryReadSnapshotDictionary(Dictionary snapshot, string key, out Dictionary value)
	{
		value = new Dictionary();
		if (!snapshot.TryGetValue(key, out var value2) || value2.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		value = CloneFrameEditDictionary(value2.AsGodotDictionary());
		return true;
	}

	private static bool TryReadSnapshotEvents(Dictionary snapshot, string key, out Array<Godot.Collections.Array> value)
	{
		value = new Array<Godot.Collections.Array>();
		if (!snapshot.TryGetValue(key, out var value2) || value2.VariantType != Variant.Type.Array)
		{
			return false;
		}
		foreach (Variant item in value2.AsGodotArray())
		{
			if (item.VariantType == Variant.Type.Nil)
			{
				value.Add(new Godot.Collections.Array());
				continue;
			}
			if (item.VariantType == Variant.Type.Array)
			{
				value.Add(CloneFrameEditEventFrame(item.AsGodotArray()));
				continue;
			}
			return false;
		}
		return true;
	}

	private static bool TryReadSnapshotRectArray(Dictionary snapshot, string key, out Array<Rect2> value)
	{
		value = new Array<Rect2>();
		if (!snapshot.TryGetValue(key, out var value2) || value2.VariantType != Variant.Type.Array)
		{
			return false;
		}
		foreach (Variant item in value2.AsGodotArray())
		{
			if (item.VariantType != Variant.Type.Rect2)
			{
				return false;
			}
			value.Add(item.AsRect2());
		}
		return true;
	}

	private static bool TryReadSnapshotStringArray(Dictionary snapshot, string key, out Array<string> value)
	{
		value = new Array<string>();
		if (!snapshot.TryGetValue(key, out var value2) || value2.VariantType != Variant.Type.Array)
		{
			return false;
		}
		foreach (Variant item in value2.AsGodotArray())
		{
			if (item.VariantType != Variant.Type.String)
			{
				return false;
			}
			value.Add(item.AsString());
		}
		return true;
	}

	private static T[] CloneFrameEditArray<T>(T[] source)
	{
		if (source != null)
		{
			return (T[])source.Clone();
		}
		return System.Array.Empty<T>();
	}

	private static Dictionary CloneFrameEditDictionary(Dictionary source)
	{
		if (source != null)
		{
			return source.Duplicate(deep: true);
		}
		return new Dictionary();
	}

	private static Godot.Collections.Array CloneFrameEditEventFrame(Godot.Collections.Array source)
	{
		if (source != null)
		{
			return source.Duplicate(deep: true);
		}
		return new Godot.Collections.Array();
	}

	private static Array<Godot.Collections.Array> CloneFrameEditEvents(Array<Godot.Collections.Array> source)
	{
		Array<Godot.Collections.Array> array = new Array<Godot.Collections.Array>();
		if (source == null)
		{
			return array;
		}
		for (int i = 0; i < source.Count; i++)
		{
			array.Add(CloneFrameEditEventFrame(source[i]));
		}
		return array;
	}

	private static Array<Rect2> CloneFrameEditRectArray(Array<Rect2> source)
	{
		Array<Rect2> array = new Array<Rect2>();
		if (source == null)
		{
			return array;
		}
		for (int i = 0; i < source.Count; i++)
		{
			array.Add(source[i]);
		}
		return array;
	}

	private static Array<string> CloneFrameEditStringArray(Array<string> source)
	{
		Array<string> array = new Array<string>();
		if (source == null)
		{
			return array;
		}
		for (int i = 0; i < source.Count; i++)
		{
			array.Add(source[i]);
		}
		return array;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(54)
		{
			new MethodInfo(MethodName.SetFrameScaleForDeferredHydration, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildTimelineCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitForModImport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourceDatPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LerpManaged, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "weight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadDatMetadataForModPreview, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourceDatPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAnimeFileForModImport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "importedAnimeFile", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetEditorResolvedAnimeFilePath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resolvedAnimeFilePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryInitForEditorAssignedAnimeFile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourceDatPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasPackedRuntimeData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsurePackedRuntimeData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloneTimelineArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneRectArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneStringArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasFullDataForDatPath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "datPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitInternal, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "forceRuntime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "sourceDatPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildPackedRuntimeDataFromTimelineData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StripSourceRuntimeData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearMetadataPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SkipBytes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "file", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("FileAccess"), exported: false),
				new PropertyInfo(Variant.Type.Int, "byteCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateTransparentFallbackImage, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Image"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetResolvedAnimeFilePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourceDatPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetAtlasSourceKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasEmbeddedAtlasSource, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMediaRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLayerHintString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "includeNoone", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildLayerNameArray, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetLayerNameArrayCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveAnimeFilePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourceDatPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryResolveOwnerBasenameCompanionDat, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "missingPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeDatPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeAtlasSourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRelativeFilePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldAutoInitAnimeFile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearPackedRuntimeData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasClip, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetClip, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureFrameEditSnapshot, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CaptureFullFrameEditSnapshot, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CaptureFrameEditSnapshotCore, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "includeLegacyTimeline", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyFrameEditSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearFrameAuthoringFlag, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetFrameEditFailure, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InvalidateFrameAuthoringCaches, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildFrameEditEventIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdjustClipsForInsert, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "clipDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "insertIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "targetClip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AdjustClipsForDelete, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "clipDictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "deletedIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "remainingFrameCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneFrameEditDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneFrameEditEventFrame, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneFrameEditEvents, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneFrameEditRectArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneFrameEditStringArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SetFrameScaleForDeferredHydration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetFrameScaleForDeferredHydration(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildTimelineCache && args.Count == 0)
		{
			BuildTimelineCache();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 0)
		{
			Init();
			ret = default;
			return true;
		}
		if (method == MethodName.InitForModImport && args.Count == 1)
		{
			InitForModImport(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LerpManaged && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<float>(LerpManaged(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.LoadDatMetadataForModPreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(LoadDatMetadataForModPreview(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetAnimeFileForModImport && args.Count == 1)
		{
			SetAnimeFileForModImport(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetEditorResolvedAnimeFilePath && args.Count == 1)
		{
			SetEditorResolvedAnimeFilePath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryInitForEditorAssignedAnimeFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryInitForEditorAssignedAnimeFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasPackedRuntimeData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPackedRuntimeData());
			return true;
		}
		if (method == MethodName.EnsurePackedRuntimeData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsurePackedRuntimeData());
			return true;
		}
		if (method == MethodName.CloneTimelineArray && args.Count == 1)
		{
			Array<Godot.Collections.Array> array = CloneTimelineArray(VariantUtils.ConvertToArray<Godot.Collections.Array>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CloneRectArray && args.Count == 1)
		{
			Array<Rect2> array2 = CloneRectArray(VariantUtils.ConvertToArray<Rect2>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.CloneStringArray && args.Count == 1)
		{
			Array<string> array3 = CloneStringArray(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
			return true;
		}
		if (method == MethodName.CloneDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CloneDictionary(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.HasFullDataForDatPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFullDataForDatPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.InitInternal && args.Count == 2)
		{
			InitInternal(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildPackedRuntimeDataFromTimelineData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(BuildPackedRuntimeDataFromTimelineData());
			return true;
		}
		if (method == MethodName.StripSourceRuntimeData && args.Count == 0)
		{
			StripSourceRuntimeData();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearMetadataPreview && args.Count == 0)
		{
			ClearMetadataPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.SkipBytes && args.Count == 2)
		{
			SkipBytes(VariantUtils.ConvertTo<Godot.FileAccess>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateTransparentFallbackImage && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Image>(CreateTransparentFallbackImage());
			return true;
		}
		if (method == MethodName.GetResolvedAnimeFilePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetResolvedAnimeFilePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAtlasSourceKey && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetAtlasSourceKey());
			return true;
		}
		if (method == MethodName.HasEmbeddedAtlasSource && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasEmbeddedAtlasSource());
			return true;
		}
		if (method == MethodName.GetMediaRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetMediaRect(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLayerHintString && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetLayerHintString(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildLayerNameArray && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(BuildLayerNameArray());
			return true;
		}
		if (method == MethodName.GetLayerNameArrayCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetLayerNameArrayCount());
			return true;
		}
		if (method == MethodName.ResolveAnimeFilePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveAnimeFilePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.TryResolveOwnerBasenameCompanionDat && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(TryResolveOwnerBasenameCompanionDat(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeDatPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeDatPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeAtlasSourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeAtlasSourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsRelativeFilePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRelativeFilePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldAutoInitAnimeFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldAutoInitAnimeFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPackedRuntimeData && args.Count == 0)
		{
			ClearPackedRuntimeData();
			ret = default;
			return true;
		}
		if (method == MethodName.HasClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasClip(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetClip(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.CaptureFrameEditSnapshot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CaptureFrameEditSnapshot());
			return true;
		}
		if (method == MethodName.CaptureFullFrameEditSnapshot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CaptureFullFrameEditSnapshot());
			return true;
		}
		if (method == MethodName.CaptureFrameEditSnapshotCore && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CaptureFrameEditSnapshotCore(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyFrameEditSnapshot && args.Count == 1)
		{
			ApplyFrameEditSnapshot(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearFrameAuthoringFlag && args.Count == 0)
		{
			ClearFrameAuthoringFlag();
			ret = default;
			return true;
		}
		if (method == MethodName.SetFrameEditFailure && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetFrameEditFailure(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.InvalidateFrameAuthoringCaches && args.Count == 0)
		{
			InvalidateFrameAuthoringCaches();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildFrameEditEventIndex && args.Count == 0)
		{
			RebuildFrameEditEventIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.AdjustClipsForInsert && args.Count == 3)
		{
			AdjustClipsForInsert(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AdjustClipsForDelete && args.Count == 3)
		{
			AdjustClipsForDelete(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloneFrameEditDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CloneFrameEditDictionary(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.CloneFrameEditEventFrame && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(CloneFrameEditEventFrame(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0])));
			return true;
		}
		if (method == MethodName.CloneFrameEditEvents && args.Count == 1)
		{
			Array<Godot.Collections.Array> array4 = CloneFrameEditEvents(VariantUtils.ConvertToArray<Godot.Collections.Array>(in args[0]));
			ret = VariantUtils.CreateFromArray(array4);
			return true;
		}
		if (method == MethodName.CloneFrameEditRectArray && args.Count == 1)
		{
			Array<Rect2> array5 = CloneFrameEditRectArray(VariantUtils.ConvertToArray<Rect2>(in args[0]));
			ret = VariantUtils.CreateFromArray(array5);
			return true;
		}
		if (method == MethodName.CloneFrameEditStringArray && args.Count == 1)
		{
			Array<string> array6 = CloneFrameEditStringArray(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array6);
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LerpManaged && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<float>(LerpManaged(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.CloneTimelineArray && args.Count == 1)
		{
			Array<Godot.Collections.Array> array = CloneTimelineArray(VariantUtils.ConvertToArray<Godot.Collections.Array>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CloneRectArray && args.Count == 1)
		{
			Array<Rect2> array2 = CloneRectArray(VariantUtils.ConvertToArray<Rect2>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.CloneStringArray && args.Count == 1)
		{
			Array<string> array3 = CloneStringArray(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
			return true;
		}
		if (method == MethodName.CloneDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CloneDictionary(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.SkipBytes && args.Count == 2)
		{
			SkipBytes(VariantUtils.ConvertTo<Godot.FileAccess>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateTransparentFallbackImage && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Image>(CreateTransparentFallbackImage());
			return true;
		}
		if (method == MethodName.NormalizeDatPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeDatPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeAtlasSourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeAtlasSourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsRelativeFilePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRelativeFilePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldAutoInitAnimeFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldAutoInitAnimeFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AdjustClipsForInsert && args.Count == 3)
		{
			AdjustClipsForInsert(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AdjustClipsForDelete && args.Count == 3)
		{
			AdjustClipsForDelete(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloneFrameEditDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(CloneFrameEditDictionary(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.CloneFrameEditEventFrame && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(CloneFrameEditEventFrame(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0])));
			return true;
		}
		if (method == MethodName.CloneFrameEditEvents && args.Count == 1)
		{
			Array<Godot.Collections.Array> array4 = CloneFrameEditEvents(VariantUtils.ConvertToArray<Godot.Collections.Array>(in args[0]));
			ret = VariantUtils.CreateFromArray(array4);
			return true;
		}
		if (method == MethodName.CloneFrameEditRectArray && args.Count == 1)
		{
			Array<Rect2> array5 = CloneFrameEditRectArray(VariantUtils.ConvertToArray<Rect2>(in args[0]));
			ret = VariantUtils.CreateFromArray(array5);
			return true;
		}
		if (method == MethodName.CloneFrameEditStringArray && args.Count == 1)
		{
			Array<string> array6 = CloneFrameEditStringArray(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array6);
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.SetFrameScaleForDeferredHydration)
		{
			return true;
		}
		if (method == MethodName.BuildTimelineCache)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.InitForModImport)
		{
			return true;
		}
		if (method == MethodName.LerpManaged)
		{
			return true;
		}
		if (method == MethodName.LoadDatMetadataForModPreview)
		{
			return true;
		}
		if (method == MethodName.SetAnimeFileForModImport)
		{
			return true;
		}
		if (method == MethodName.SetEditorResolvedAnimeFilePath)
		{
			return true;
		}
		if (method == MethodName.TryInitForEditorAssignedAnimeFile)
		{
			return true;
		}
		if (method == MethodName.HasPackedRuntimeData)
		{
			return true;
		}
		if (method == MethodName.EnsurePackedRuntimeData)
		{
			return true;
		}
		if (method == MethodName.CloneTimelineArray)
		{
			return true;
		}
		if (method == MethodName.CloneRectArray)
		{
			return true;
		}
		if (method == MethodName.CloneStringArray)
		{
			return true;
		}
		if (method == MethodName.CloneDictionary)
		{
			return true;
		}
		if (method == MethodName.HasFullDataForDatPath)
		{
			return true;
		}
		if (method == MethodName.InitInternal)
		{
			return true;
		}
		if (method == MethodName.BuildPackedRuntimeDataFromTimelineData)
		{
			return true;
		}
		if (method == MethodName.StripSourceRuntimeData)
		{
			return true;
		}
		if (method == MethodName.ClearMetadataPreview)
		{
			return true;
		}
		if (method == MethodName.SkipBytes)
		{
			return true;
		}
		if (method == MethodName.CreateTransparentFallbackImage)
		{
			return true;
		}
		if (method == MethodName.GetResolvedAnimeFilePath)
		{
			return true;
		}
		if (method == MethodName.GetAtlasSourceKey)
		{
			return true;
		}
		if (method == MethodName.HasEmbeddedAtlasSource)
		{
			return true;
		}
		if (method == MethodName.GetMediaRect)
		{
			return true;
		}
		if (method == MethodName.GetLayerHintString)
		{
			return true;
		}
		if (method == MethodName.BuildLayerNameArray)
		{
			return true;
		}
		if (method == MethodName.GetLayerNameArrayCount)
		{
			return true;
		}
		if (method == MethodName.ResolveAnimeFilePath)
		{
			return true;
		}
		if (method == MethodName.TryResolveOwnerBasenameCompanionDat)
		{
			return true;
		}
		if (method == MethodName.NormalizeDatPath)
		{
			return true;
		}
		if (method == MethodName.NormalizeAtlasSourcePath)
		{
			return true;
		}
		if (method == MethodName.IsRelativeFilePath)
		{
			return true;
		}
		if (method == MethodName.ShouldAutoInitAnimeFile)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.ClearPackedRuntimeData)
		{
			return true;
		}
		if (method == MethodName.HasClip)
		{
			return true;
		}
		if (method == MethodName.GetClip)
		{
			return true;
		}
		if (method == MethodName.CaptureFrameEditSnapshot)
		{
			return true;
		}
		if (method == MethodName.CaptureFullFrameEditSnapshot)
		{
			return true;
		}
		if (method == MethodName.CaptureFrameEditSnapshotCore)
		{
			return true;
		}
		if (method == MethodName.ApplyFrameEditSnapshot)
		{
			return true;
		}
		if (method == MethodName.ClearFrameAuthoringFlag)
		{
			return true;
		}
		if (method == MethodName.SetFrameEditFailure)
		{
			return true;
		}
		if (method == MethodName.InvalidateFrameAuthoringCaches)
		{
			return true;
		}
		if (method == MethodName.RebuildFrameEditEventIndex)
		{
			return true;
		}
		if (method == MethodName.AdjustClipsForInsert)
		{
			return true;
		}
		if (method == MethodName.AdjustClipsForDelete)
		{
			return true;
		}
		if (method == MethodName.CloneFrameEditDictionary)
		{
			return true;
		}
		if (method == MethodName.CloneFrameEditEventFrame)
		{
			return true;
		}
		if (method == MethodName.CloneFrameEditEvents)
		{
			return true;
		}
		if (method == MethodName.CloneFrameEditRectArray)
		{
			return true;
		}
		if (method == MethodName.CloneFrameEditStringArray)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.animeFile)
		{
			animeFile = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.frameRate)
		{
			frameRate = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.frameScale)
		{
			frameScale = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.frameMax)
		{
			frameMax = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ImageAtlasSizeCache)
		{
			ImageAtlasSizeCache = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.timelineData)
		{
			timelineData = VariantUtils.ConvertToArray<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName.mediaList)
		{
			mediaList = VariantUtils.ConvertToArray<Rect2>(in value);
			return true;
		}
		if (name == PropertyName.frameOffsets)
		{
			frameOffsets = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName.frameCounts)
		{
			frameCounts = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName.sliceKeys)
		{
			sliceKeys = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName.sliceMediaIds)
		{
			sliceMediaIds = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName.sliceLayerIds)
		{
			sliceLayerIds = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName.sliceDrawOrders)
		{
			sliceDrawOrders = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName.sliceFlags)
		{
			sliceFlags = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName.sliceTransforms)
		{
			sliceTransforms = VariantUtils.ConvertTo<float[]>(in value);
			return true;
		}
		if (name == PropertyName.sliceAlpha)
		{
			sliceAlpha = VariantUtils.ConvertTo<float[]>(in value);
			return true;
		}
		if (name == PropertyName.mediaRects)
		{
			mediaRects = VariantUtils.ConvertTo<Vector4[]>(in value);
			return true;
		}
		if (name == PropertyName.layerList)
		{
			layerList = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.events)
		{
			events = VariantUtils.ConvertToArray<Godot.Collections.Array>(in value);
			return true;
		}
		if (name == PropertyName.clips)
		{
			clips = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.mediaDictionary)
		{
			mediaDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.layerDictionary)
		{
			layerDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.replaceSlotDictionary)
		{
			replaceSlotDictionary = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.rasterCompositeData)
		{
			rasterCompositeData = VariantUtils.ConvertTo<AdobeAnimateRasterCompositeData>(in value);
			return true;
		}
		if (name == PropertyName.extraMediaReplaceTexturePaths)
		{
			extraMediaReplaceTexturePaths = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.maxElement)
		{
			maxElement = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.LastFrameEditError)
		{
			LastFrameEditError = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._animeFile)
		{
			_animeFile = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._frameScale)
		{
			_frameScale = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._editorResolvedAnimeFilePath)
		{
			_editorResolvedAnimeFilePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._fullDataDatPath)
		{
			_fullDataDatPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._fullDataFrameScale)
		{
			_fullDataFrameScale = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._hasAuthoredPackedFrames)
		{
			_hasAuthoredPackedFrames = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._authoringRevision)
		{
			_authoringRevision = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._imageAtlasSizeCache)
		{
			_imageAtlasSizeCache = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.animeFile)
		{
			from = animeFile;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.frameRate)
		{
			value = VariantUtils.CreateFrom<double>(frameRate);
			return true;
		}
		int from2;
		if (name == PropertyName.frameScale)
		{
			from2 = frameScale;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.frameMax)
		{
			from2 = frameMax;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ImageAtlasSizeCache)
		{
			value = VariantUtils.CreateFrom<Vector2>(ImageAtlasSizeCache);
			return true;
		}
		if (name == PropertyName.timelineData)
		{
			value = VariantUtils.CreateFromArray(timelineData);
			return true;
		}
		if (name == PropertyName.mediaList)
		{
			value = VariantUtils.CreateFromArray(mediaList);
			return true;
		}
		int[] from3;
		if (name == PropertyName.frameOffsets)
		{
			from3 = frameOffsets;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.frameCounts)
		{
			from3 = frameCounts;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.sliceKeys)
		{
			from3 = sliceKeys;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.sliceMediaIds)
		{
			from3 = sliceMediaIds;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.sliceLayerIds)
		{
			from3 = sliceLayerIds;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.sliceDrawOrders)
		{
			from3 = sliceDrawOrders;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.sliceFlags)
		{
			from3 = sliceFlags;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		float[] from4;
		if (name == PropertyName.sliceTransforms)
		{
			from4 = sliceTransforms;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.sliceAlpha)
		{
			from4 = sliceAlpha;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.mediaRects)
		{
			value = VariantUtils.CreateFrom<Vector4[]>(mediaRects);
			return true;
		}
		if (name == PropertyName.layerList)
		{
			value = VariantUtils.CreateFromArray(layerList);
			return true;
		}
		if (name == PropertyName.events)
		{
			value = VariantUtils.CreateFromArray(events);
			return true;
		}
		Dictionary from5;
		if (name == PropertyName.clips)
		{
			from5 = clips;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.mediaDictionary)
		{
			from5 = mediaDictionary;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.layerDictionary)
		{
			from5 = layerDictionary;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.replaceSlotDictionary)
		{
			from5 = replaceSlotDictionary;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.rasterCompositeData)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateRasterCompositeData>(rasterCompositeData);
			return true;
		}
		if (name == PropertyName.extraMediaReplaceTexturePaths)
		{
			value = VariantUtils.CreateFromArray(extraMediaReplaceTexturePaths);
			return true;
		}
		if (name == PropertyName.maxElement)
		{
			from2 = maxElement;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.EditableFrameCount)
		{
			from2 = EditableFrameCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.HasAuthoredPackedFrames)
		{
			value = VariantUtils.CreateFrom<bool>(HasAuthoredPackedFrames);
			return true;
		}
		if (name == PropertyName.AuthoringRevision)
		{
			value = VariantUtils.CreateFrom<long>(AuthoringRevision);
			return true;
		}
		if (name == PropertyName.LastFrameEditError)
		{
			from = LastFrameEditError;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._animeFile)
		{
			value = VariantUtils.CreateFrom(in _animeFile);
			return true;
		}
		if (name == PropertyName._frameScale)
		{
			value = VariantUtils.CreateFrom(in _frameScale);
			return true;
		}
		if (name == PropertyName._editorResolvedAnimeFilePath)
		{
			value = VariantUtils.CreateFrom(in _editorResolvedAnimeFilePath);
			return true;
		}
		if (name == PropertyName._fullDataDatPath)
		{
			value = VariantUtils.CreateFrom(in _fullDataDatPath);
			return true;
		}
		if (name == PropertyName._fullDataFrameScale)
		{
			value = VariantUtils.CreateFrom(in _fullDataFrameScale);
			return true;
		}
		if (name == PropertyName._hasAuthoredPackedFrames)
		{
			value = VariantUtils.CreateFrom(in _hasAuthoredPackedFrames);
			return true;
		}
		if (name == PropertyName._authoringRevision)
		{
			value = VariantUtils.CreateFrom(in _authoringRevision);
			return true;
		}
		if (name == PropertyName._imageAtlasSizeCache)
		{
			value = VariantUtils.CreateFrom(in _imageAtlasSizeCache);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName._animeFile, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._editorResolvedAnimeFilePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._fullDataDatPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._fullDataFrameScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasAuthoredPackedFrames, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._authoringRevision, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.animeFile, PropertyHint.File, "*.dat", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.frameRate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.frameScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.frameMax, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._imageAtlasSizeCache, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.ImageAtlasSizeCache, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.timelineData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.mediaList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.frameOffsets, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.frameCounts, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.sliceKeys, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.sliceMediaIds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.sliceLayerIds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.sliceDrawOrders, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName.sliceFlags, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedFloat32Array, PropertyName.sliceTransforms, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedFloat32Array, PropertyName.sliceAlpha, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.PackedVector4Array, PropertyName.mediaRects, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.layerList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.events, PropertyHint.TypeString, "28/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.clips, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.mediaDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.layerDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.replaceSlotDictionary, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.rasterCompositeData, PropertyHint.ResourceType, "AdobeAnimateRasterCompositeData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName.extraMediaReplaceTexturePaths, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.maxElement, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.EditableFrameCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasAuthoredPackedFrames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AuthoringRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.LastFrameEditError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.animeFile, Variant.From<string>(animeFile));
		info.AddProperty(PropertyName.frameRate, Variant.From<double>(frameRate));
		info.AddProperty(PropertyName.frameScale, Variant.From<int>(frameScale));
		info.AddProperty(PropertyName.frameMax, Variant.From<int>(frameMax));
		info.AddProperty(PropertyName.ImageAtlasSizeCache, Variant.From<Vector2>(ImageAtlasSizeCache));
		info.AddProperty(PropertyName.timelineData, Variant.CreateFrom(timelineData));
		info.AddProperty(PropertyName.mediaList, Variant.CreateFrom(mediaList));
		info.AddProperty(PropertyName.frameOffsets, Variant.From<int[]>(frameOffsets));
		info.AddProperty(PropertyName.frameCounts, Variant.From<int[]>(frameCounts));
		info.AddProperty(PropertyName.sliceKeys, Variant.From<int[]>(sliceKeys));
		info.AddProperty(PropertyName.sliceMediaIds, Variant.From<int[]>(sliceMediaIds));
		info.AddProperty(PropertyName.sliceLayerIds, Variant.From<int[]>(sliceLayerIds));
		info.AddProperty(PropertyName.sliceDrawOrders, Variant.From<int[]>(sliceDrawOrders));
		info.AddProperty(PropertyName.sliceFlags, Variant.From<int[]>(sliceFlags));
		info.AddProperty(PropertyName.sliceTransforms, Variant.From<float[]>(sliceTransforms));
		info.AddProperty(PropertyName.sliceAlpha, Variant.From<float[]>(sliceAlpha));
		info.AddProperty(PropertyName.mediaRects, Variant.From<Vector4[]>(mediaRects));
		info.AddProperty(PropertyName.layerList, Variant.CreateFrom(layerList));
		info.AddProperty(PropertyName.events, Variant.CreateFrom(events));
		info.AddProperty(PropertyName.clips, Variant.From<Dictionary>(clips));
		info.AddProperty(PropertyName.mediaDictionary, Variant.From<Dictionary>(mediaDictionary));
		info.AddProperty(PropertyName.layerDictionary, Variant.From<Dictionary>(layerDictionary));
		info.AddProperty(PropertyName.replaceSlotDictionary, Variant.From<Dictionary>(replaceSlotDictionary));
		info.AddProperty(PropertyName.rasterCompositeData, Variant.From<AdobeAnimateRasterCompositeData>(rasterCompositeData));
		info.AddProperty(PropertyName.extraMediaReplaceTexturePaths, Variant.CreateFrom(extraMediaReplaceTexturePaths));
		info.AddProperty(PropertyName.maxElement, Variant.From<int>(maxElement));
		info.AddProperty(PropertyName.LastFrameEditError, Variant.From<string>(LastFrameEditError));
		info.AddProperty(PropertyName._animeFile, Variant.From(in _animeFile));
		info.AddProperty(PropertyName._frameScale, Variant.From(in _frameScale));
		info.AddProperty(PropertyName._editorResolvedAnimeFilePath, Variant.From(in _editorResolvedAnimeFilePath));
		info.AddProperty(PropertyName._fullDataDatPath, Variant.From(in _fullDataDatPath));
		info.AddProperty(PropertyName._fullDataFrameScale, Variant.From(in _fullDataFrameScale));
		info.AddProperty(PropertyName._hasAuthoredPackedFrames, Variant.From(in _hasAuthoredPackedFrames));
		info.AddProperty(PropertyName._authoringRevision, Variant.From(in _authoringRevision));
		info.AddProperty(PropertyName._imageAtlasSizeCache, Variant.From(in _imageAtlasSizeCache));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.animeFile, out var value))
		{
			animeFile = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.frameRate, out var value2))
		{
			frameRate = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.frameScale, out var value3))
		{
			frameScale = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.frameMax, out var value4))
		{
			frameMax = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ImageAtlasSizeCache, out var value5))
		{
			ImageAtlasSizeCache = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.timelineData, out var value6))
		{
			timelineData = value6.AsGodotArray<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName.mediaList, out var value7))
		{
			mediaList = value7.AsGodotArray<Rect2>();
		}
		if (info.TryGetProperty(PropertyName.frameOffsets, out var value8))
		{
			frameOffsets = value8.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName.frameCounts, out var value9))
		{
			frameCounts = value9.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName.sliceKeys, out var value10))
		{
			sliceKeys = value10.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName.sliceMediaIds, out var value11))
		{
			sliceMediaIds = value11.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName.sliceLayerIds, out var value12))
		{
			sliceLayerIds = value12.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName.sliceDrawOrders, out var value13))
		{
			sliceDrawOrders = value13.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName.sliceFlags, out var value14))
		{
			sliceFlags = value14.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName.sliceTransforms, out var value15))
		{
			sliceTransforms = value15.As<float[]>();
		}
		if (info.TryGetProperty(PropertyName.sliceAlpha, out var value16))
		{
			sliceAlpha = value16.As<float[]>();
		}
		if (info.TryGetProperty(PropertyName.mediaRects, out var value17))
		{
			mediaRects = value17.As<Vector4[]>();
		}
		if (info.TryGetProperty(PropertyName.layerList, out var value18))
		{
			layerList = value18.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.events, out var value19))
		{
			events = value19.AsGodotArray<Godot.Collections.Array>();
		}
		if (info.TryGetProperty(PropertyName.clips, out var value20))
		{
			clips = value20.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.mediaDictionary, out var value21))
		{
			mediaDictionary = value21.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.layerDictionary, out var value22))
		{
			layerDictionary = value22.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.replaceSlotDictionary, out var value23))
		{
			replaceSlotDictionary = value23.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.rasterCompositeData, out var value24))
		{
			rasterCompositeData = value24.As<AdobeAnimateRasterCompositeData>();
		}
		if (info.TryGetProperty(PropertyName.extraMediaReplaceTexturePaths, out var value25))
		{
			extraMediaReplaceTexturePaths = value25.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.maxElement, out var value26))
		{
			maxElement = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName.LastFrameEditError, out var value27))
		{
			LastFrameEditError = value27.As<string>();
		}
		if (info.TryGetProperty(PropertyName._animeFile, out var value28))
		{
			_animeFile = value28.As<string>();
		}
		if (info.TryGetProperty(PropertyName._frameScale, out var value29))
		{
			_frameScale = value29.As<int>();
		}
		if (info.TryGetProperty(PropertyName._editorResolvedAnimeFilePath, out var value30))
		{
			_editorResolvedAnimeFilePath = value30.As<string>();
		}
		if (info.TryGetProperty(PropertyName._fullDataDatPath, out var value31))
		{
			_fullDataDatPath = value31.As<string>();
		}
		if (info.TryGetProperty(PropertyName._fullDataFrameScale, out var value32))
		{
			_fullDataFrameScale = value32.As<int>();
		}
		if (info.TryGetProperty(PropertyName._hasAuthoredPackedFrames, out var value33))
		{
			_hasAuthoredPackedFrames = value33.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._authoringRevision, out var value34))
		{
			_authoringRevision = value34.As<long>();
		}
		if (info.TryGetProperty(PropertyName._imageAtlasSizeCache, out var value35))
		{
			_imageAtlasSizeCache = value35.As<Vector2>();
		}
	}
}
