using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://addons/AdobeAnimateEditor/Rendering/AdobeAnimateMultiMeshBatcher.cs")]
internal sealed class AdobeAnimateMultiMeshBatcher : Node2D
{
	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName TryAppendPreparedGpuGraphInstance = "TryAppendPreparedGpuGraphInstance";

		public static readonly StringName EnsureCachedCompositeOwnerCapacity = "EnsureCachedCompositeOwnerCapacity";

		public static readonly StringName IsMediaReplaceActive = "IsMediaReplaceActive";

		public static readonly StringName GetCrowdLayerVisibleCount = "GetCrowdLayerVisibleCount";

		public static readonly StringName GetCrowdLayerMaskTexelCount = "GetCrowdLayerMaskTexelCount";

		public static readonly StringName BuildCrowdLayerMaskTexel = "BuildCrowdLayerMaskTexel";

		public static readonly StringName BuildCrowdLayerMaskComponent = "BuildCrowdLayerMaskComponent";

		public static readonly StringName IsLayerVisible = "IsLayerVisible";

		public static readonly StringName BeginFrame = "BeginFrame";

		public static readonly StringName GetVisibleInstanceCountForTest = "GetVisibleInstanceCountForTest";

		public static readonly StringName GetBufferedInstanceCountForTest = "GetBufferedInstanceCountForTest";

		public static readonly StringName GetCompactCrowdRootCountForTest = "GetCompactCrowdRootCountForTest";

		public static readonly StringName GetCompositeCrowdRootCountForTest = "GetCompositeCrowdRootCountForTest";

		public static readonly StringName GetGpuGraphRootCountForTest = "GetGpuGraphRootCountForTest";

		public static readonly StringName GetGpuGraphSlotCountForTest = "GetGpuGraphSlotCountForTest";

		public static readonly StringName GetGpuGraphStateWrittenTexelsForTest = "GetGpuGraphStateWrittenTexelsForTest";

		public static readonly StringName GetCrowdStateWrittenTexelsForTest = "GetCrowdStateWrittenTexelsForTest";

		public static readonly StringName BeginBuffered = "BeginBuffered";

		public static readonly StringName BeginGpuGraphBuffered = "BeginGpuGraphBuffered";

		public static readonly StringName ConfigureBufferedGpuGraphTexture = "ConfigureBufferedGpuGraphTexture";

		public static readonly StringName ConfigureBufferedGpuDynamicOverrideTexture = "ConfigureBufferedGpuDynamicOverrideTexture";

		public static readonly StringName ApplyStandaloneCrowdClock = "ApplyStandaloneCrowdClock";

		public static readonly StringName ResetCrowdFrameState = "ResetCrowdFrameState";

		public static readonly StringName EnsureInstanceDataTexture = "EnsureInstanceDataTexture";

		public static readonly StringName ConfigurePoseTexture = "ConfigurePoseTexture";

		public static readonly StringName ConfigureGpuRenderGraphTexture = "ConfigureGpuRenderGraphTexture";

		public static readonly StringName ConfigureGpuDynamicOverrideTexture = "ConfigureGpuDynamicOverrideTexture";

		public static readonly StringName ApplyPoseTextureToMaterial = "ApplyPoseTextureToMaterial";

		public static readonly StringName ApplyGpuRenderGraphTextureToMaterial = "ApplyGpuRenderGraphTextureToMaterial";

		public static readonly StringName ApplyGpuDynamicOverrideTextureToMaterial = "ApplyGpuDynamicOverrideTextureToMaterial";

		public static readonly StringName ApplyInstanceDataLayoutToMaterial = "ApplyInstanceDataLayoutToMaterial";

		public static readonly StringName NextPowerOfTwo = "NextPowerOfTwo";

		public static readonly StringName CreateUnitQuadMesh = "CreateUnitQuadMesh";

		public static readonly StringName CreateCrowdMesh = "CreateCrowdMesh";

		public static readonly StringName GetCurrentCrowdGroupStateCursor = "GetCurrentCrowdGroupStateCursor";

		public static readonly StringName EndBuffered = "EndBuffered";

		public static readonly StringName EnsureRenderObjects = "EnsureRenderObjects";

		public static readonly StringName EnsureCrowdRenderObjects = "EnsureCrowdRenderObjects";

		public static readonly StringName EnsureCapacity = "EnsureCapacity";

		public static readonly StringName EnsureCrowdCapacity = "EnsureCrowdCapacity";

		public static readonly StringName CommitCrowdGroup = "CommitCrowdGroup";

		public static readonly StringName RollbackCrowdGroup = "RollbackCrowdGroup";

		public static readonly StringName ClearCrowdGroupState = "ClearCrowdGroupState";

		public static readonly StringName TryAppendCrowdInstance = "TryAppendCrowdInstance";

		public static readonly StringName RollbackFailedCrowdInstance = "RollbackFailedCrowdInstance";

		public static readonly StringName UploadBufferedData = "UploadBufferedData";

		public static readonly StringName UploadCrowdBufferedData = "UploadCrowdBufferedData";

		public static readonly StringName UploadCrowdStateOnce = "UploadCrowdStateOnce";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateSharedCrowdMaterial = "CreateSharedCrowdMaterial";

		public static readonly StringName CreateSharedCrowdMesh = "CreateSharedCrowdMesh";

		public static readonly StringName SetSharedCrowdAnimationClock = "SetSharedCrowdAnimationClock";

		public static readonly StringName SetSharedCrowdPhysicsInterpolationFraction = "SetSharedCrowdPhysicsInterpolationFraction";

		public static readonly StringName ClearSharedCrowdStateBinding = "ClearSharedCrowdStateBinding";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName DetachRenderBindingsForOwnerExit = "DetachRenderBindingsForOwnerExit";

		public static readonly StringName WriteShaderPoseInstanceDataBufferInstance = "WriteShaderPoseInstanceDataBufferInstance";

		public static readonly StringName PackHorizontalClip = "PackHorizontalClip";

		public static readonly StringName WriteShaderPoseMultiMeshBufferInstance = "WriteShaderPoseMultiMeshBufferInstance";

		public static readonly StringName WriteCachedCompositeCrowdCommand = "WriteCachedCompositeCrowdCommand";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName EncoderOnly = "EncoderOnly";

		public static readonly StringName _instance = "_instance";

		public static readonly StringName _multiMesh = "_multiMesh";

		public static readonly StringName _material = "_material";

		public static readonly StringName _quadMesh = "_quadMesh";

		public static readonly StringName _crowdInstance = "_crowdInstance";

		public static readonly StringName _crowdMultiMesh = "_crowdMultiMesh";

		public static readonly StringName _crowdMaterial = "_crowdMaterial";

		public static readonly StringName _crowdMesh = "_crowdMesh";

		public static readonly StringName _instanceDataImage = "_instanceDataImage";

		public static readonly StringName _instanceDataTexture = "_instanceDataTexture";

		public static readonly StringName _instanceDataTextureSize = "_instanceDataTextureSize";

		public static readonly StringName _instanceDataTexelCapacity = "_instanceDataTexelCapacity";

		public static readonly StringName _instanceDataBuffer = "_instanceDataBuffer";

		public static readonly StringName _multiMeshBuffer = "_multiMeshBuffer";

		public static readonly StringName _crowdMultiMeshBuffer = "_crowdMultiMeshBuffer";

		public static readonly StringName _boundCrowdStateTexture = "_boundCrowdStateTexture";

		public static readonly StringName _boundCrowdStateTextureSize = "_boundCrowdStateTextureSize";

		public static readonly StringName _crowdGroupArenaMark = "_crowdGroupArenaMark";

		public static readonly StringName _crowdGroupBucketMark = "_crowdGroupBucketMark";

		public static readonly StringName _crowdGroupExpectedBucketEnd = "_crowdGroupExpectedBucketEnd";

		public static readonly StringName _crowdGroupCursorTexel = "_crowdGroupCursorTexel";

		public static readonly StringName _crowdGroupEndTexel = "_crowdGroupEndTexel";

		public static readonly StringName _standaloneCrowdFrameVersion = "_standaloneCrowdFrameVersion";

		public static readonly StringName _cachedCompositeOwnerOrigins = "_cachedCompositeOwnerOrigins";

		public static readonly StringName _capacity = "_capacity";

		public static readonly StringName _crowdCapacity = "_crowdCapacity";

		public static readonly StringName _bufferedCount = "_bufferedCount";

		public static readonly StringName _crowdBufferedCount = "_crowdBufferedCount";

		public static readonly StringName _crowdMeshMaxSlices = "_crowdMeshMaxSlices";

		public static readonly StringName _compactCrowdRootCount = "_compactCrowdRootCount";

		public static readonly StringName _compositeCrowdRootCount = "_compositeCrowdRootCount";

		public static readonly StringName _gpuGraphRootCount = "_gpuGraphRootCount";

		public static readonly StringName _gpuGraphSlotCount = "_gpuGraphSlotCount";

		public static readonly StringName _gpuGraphStateWrittenTexels = "_gpuGraphStateWrittenTexels";

		public static readonly StringName _crowdStateOverflowReported = "_crowdStateOverflowReported";

		public static readonly StringName _atlasArray = "_atlasArray";

		public static readonly StringName _atlasSize = "_atlasSize";

		public static readonly StringName _poseTextureArray = "_poseTextureArray";

		public static readonly StringName _poseTextureSize = "_poseTextureSize";

		public static readonly StringName _gpuRenderGraphTextureArray = "_gpuRenderGraphTextureArray";

		public static readonly StringName _gpuRenderGraphTextureSize = "_gpuRenderGraphTextureSize";

		public static readonly StringName _gpuDynamicOverrideTexture = "_gpuDynamicOverrideTexture";

		public static readonly StringName _gpuDynamicOverrideTextureSize = "_gpuDynamicOverrideTextureSize";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const string ShaderPath = "res://addons/AdobeAnimateEditor/Rendering/AdobeAnimateManagedMultiMesh.gdshader";

	private static readonly StringName AtlasTextureArrayParameter = new StringName("atlasTextureArray");

	private static readonly StringName AtlasSizeParameter = new StringName("atlasSize");

	private static readonly StringName InstanceDataTextureParameter = new StringName("instanceDataTexture");

	private static readonly StringName InstanceDataTextureSizeParameter = new StringName("instanceDataTextureSize");

	private static readonly StringName InstanceDataStrideParameter = new StringName("instanceDataStride");

	private static readonly StringName PoseTextureArrayParameter = new StringName("poseTextureArray");

	private static readonly StringName PoseTextureSizeParameter = new StringName("poseTextureSize");

	private static readonly StringName CrowdStateTextureParameter = new StringName("crowdStateTexture");

	private static readonly StringName CrowdStateTextureSizeParameter = new StringName("crowdStateTextureSize");

	private static readonly StringName GpuRenderGraphTextureArrayParameter = new StringName("gpuRenderGraphTextureArray");

	private static readonly StringName GpuRenderGraphTextureSizeParameter = new StringName("gpuRenderGraphTextureSize");

	private static readonly StringName GpuDynamicOverrideTextureParameter = new StringName("gpuDynamicOverrideTexture");

	private static readonly StringName GpuDynamicOverrideTextureSizeParameter = new StringName("gpuDynamicOverrideTextureSize");

	private static readonly StringName RasterCompositeAtlasParameter = new StringName("rasterCompositeAtlas");

	private static readonly StringName RasterCompositeAtlasSizeParameter = new StringName("rasterCompositeAtlasSize");

	private static readonly StringName RasterCompositeTileSizeParameter = new StringName("rasterCompositeTileSize");

	private static readonly StringName RasterCompositeOriginParameter = new StringName("rasterCompositeOrigin");

	private static readonly StringName RasterCompositeColumnsParameter = new StringName("rasterCompositeColumns");

	private static readonly StringName AnimationClockParameter = new StringName("animationClock");

	private static readonly StringName PhysicsInterpolationFractionParameter = new StringName("physicsInterpolationFraction");

	private const int InitialCapacity = 128;

	private const int MaxDataTextureWidth = 2048;

	private const int InstanceDataTexelsPerItem = 3;

	private const int CrowdVisualOverrideTexelsPerSlot = 2;

	private const int RootMotionStateTexels = 2;

	private const int CrowdStateHeaderTexels = 5;

	private const int CompositeCrowdHeaderTexels = 3;

	private const int CompositeCrowdCommandTexels = 6;

	private const float CompositeCrowdLayoutVersion = 1f;

	private const int CachedCompositeOwnerTexels = 4;

	private const int CachedCompositeCommandTexels = 1;

	private const int CachedCompositeVisualTexels = 2;

	private const float CachedCompositeLayoutVersion = 3f;

	private const float CompositeCommandExplicit = 0f;

	private const float CompositeCommandPose = 1f;

	private const float CompositeCommandPoseVisualOverride = 2f;

	private const int CrowdLayerMaskBitsPerTexel = 64;

	private const int CrowdLayerMaskBitsPerComponent = 16;

	private const int MultiMeshBufferStride = 16;

	private MultiMeshInstance2D _instance;

	private MultiMesh _multiMesh;

	private ShaderMaterial _material;

	private ArrayMesh _quadMesh;

	private MultiMeshInstance2D _crowdInstance;

	private MultiMesh _crowdMultiMesh;

	private ShaderMaterial _crowdMaterial;

	private ArrayMesh _crowdMesh;

	private Image _instanceDataImage;

	private ImageTexture _instanceDataTexture;

	private Vector2I _instanceDataTextureSize = Vector2I.Zero;

	private int _instanceDataTexelCapacity;

	private float[] _instanceDataBuffer;

	private float[] _multiMeshBuffer;

	private float[] _crowdMultiMeshBuffer;

	private readonly AdobeAnimateSharedStateArena _ownedCrowdStateArena = new AdobeAnimateSharedStateArena();

	private AdobeAnimateSharedStateArena _crowdStateArena;

	private TextureLayered _boundCrowdStateTexture;

	private Vector2I _boundCrowdStateTextureSize = Vector2I.Zero;

	private AdobeAnimateZIndexCrowdBucket _activeCrowdBucket;

	private int _crowdGroupArenaMark;

	private int _crowdGroupBucketMark;

	private int _crowdGroupExpectedBucketEnd;

	private int _crowdGroupCursorTexel;

	private int _crowdGroupEndTexel;

	private long _standaloneCrowdFrameVersion;

	private Transform2D[] _cachedCompositeOwnerTransforms;

	private Vector2[] _cachedCompositeOwnerOrigins;

	private int _capacity;

	private int _crowdCapacity;

	private int _bufferedCount;

	private int _crowdBufferedCount;

	private int _crowdMeshMaxSlices;

	private int _compactCrowdRootCount;

	private int _compositeCrowdRootCount;

	private int _gpuGraphRootCount;

	private int _gpuGraphSlotCount;

	private int _gpuGraphStateWrittenTexels;

	private bool _crowdStateOverflowReported;

	private TextureLayered _atlasArray;

	private Vector2 _atlasSize = Vector2.One;

	private TextureLayered _poseTextureArray;

	private Vector2I _poseTextureSize = Vector2I.Zero;

	private Texture2DArray _gpuRenderGraphTextureArray;

	private Vector2I _gpuRenderGraphTextureSize = Vector2I.Zero;

	private ImageTexture _gpuDynamicOverrideTexture;

	private Vector2I _gpuDynamicOverrideTextureSize = Vector2I.Zero;

	internal bool EncoderOnly { get; set; }

	internal static bool TryMeasureCompactCrowd(AdobeAnimateCrowdRenderState state, out AdobeAnimateCrowdLayout layout)
	{
		layout = default;
		AdobeAnimateRuntimeDefinition definition = state.Definition;
		if (definition == null || state.FrameCount <= 0 || definition.MaxFrameSliceCount <= 0)
		{
			return false;
		}
		int crowdLayerVisibleCount = GetCrowdLayerVisibleCount(state.CanUseLayerMask, state.LayerVisible);
		int crowdLayerMaskTexelCount = GetCrowdLayerMaskTexelCount(state.AllLayersVisible, crowdLayerVisibleCount);
		bool flag = HasCrowdVisualOverrides(definition, state.FrameOffset, state.FrameCount, state.HasMediaReplace, state.MediaReplaceUse, state.MediaReplaceRect);
		long num = 5 + crowdLayerMaskTexelCount + (flag ? ((long)state.FrameCount * 2L) : 0);
		if (num <= 0 || num > 2147483647)
		{
			return false;
		}
		layout = new AdobeAnimateCrowdLayout((int)num, definition.MaxFrameSliceCount);
		return true;
	}

	internal static bool TryMeasureCompositeCrowd(int commandCount, out AdobeAnimateCrowdLayout layout)
	{
		layout = default;
		long num = 3 + (long)commandCount * 6L;
		if (commandCount <= 0 || num > 2147483647)
		{
			return false;
		}
		layout = new AdobeAnimateCrowdLayout((int)num, commandCount);
		return true;
	}

	internal static bool TryMeasureCachedCompositeCrowd(int itemCount, int visualOverrideCount, int ownerCount, out AdobeAnimateCrowdLayout layout)
	{
		layout = default;
		if (itemCount <= 0 || ownerCount <= 0 || visualOverrideCount < 0 || visualOverrideCount > itemCount)
		{
			return false;
		}
		long num = 3 + (long)ownerCount * 4L + itemCount + (long)visualOverrideCount * 2L;
		if (num > 2147483647)
		{
			return false;
		}
		layout = new AdobeAnimateCrowdLayout((int)num, itemCount);
		return true;
	}

	internal static bool TryMeasureRasterCompositeCrowd(out AdobeAnimateCrowdLayout layout)
	{
		layout = new AdobeAnimateCrowdLayout(0, 1);
		return true;
	}

	internal int AppendPreparedRasterCompositeFrame(AdobeAnimateCrowdRenderState state)
	{
		if (_activeCrowdBucket == null || state.RasterCompositeData == null)
		{
			return 0;
		}
		return _activeCrowdBucket.AppendRasterComposite(state.GlobalTransform, state.Modulate, state.RasterCompositeTileIndex, state.VerticalClip) ? 1 : 0;
	}

	internal int AppendPreparedRasterCompositeFrame(in AdobeAnimateRasterCompositeRenderState state)
	{
		if (_activeCrowdBucket == null || state.RasterCompositeData == null)
		{
			return 0;
		}
		return _activeCrowdBucket.AppendRasterComposite(state.GlobalTransform, state.Modulate, state.RasterCompositeTileIndex, state.VerticalClip) ? 1 : 0;
	}

	public void AppendCrowdShaderPoseFrame(AdobeAnimateRuntimeDefinition definition, Transform2D transform, Color color, Vector2 poseOffset, bool clipEnabled, float clipUp, float clipDown, int frameOffset, int frameCount, int poseBaseTexel, int poseLayer, float interpolationT, bool allLayersVisible, bool canUseLayerMask, ulong layerMask, Array<bool> layerVisible, bool hasMediaReplace = false, Array<Rect2> mediaReplaceRect = null, Array<bool> mediaReplaceUse = null, Array<int> mediaReplaceAtlasPages = null, Vector2 mediaReplaceAtlasSize = default(Vector2))
	{
		AppendCompactCrowdFrameCore(definition, transform, color, poseOffset, clipEnabled, clipUp, clipDown, frameOffset, frameCount, poseBaseTexel, poseLayer, interpolationT, allLayersVisible, canUseLayerMask, layerMask, layerVisible, hasMediaReplace, mediaReplaceRect, mediaReplaceUse, mediaReplaceAtlasPages, mediaReplaceAtlasSize);
	}

	internal static bool TryAppendSimpleCompactCrowdFrame(AdobeAnimateSharedStateArena stateArena, AdobeAnimateZIndexCrowdBucket bucket, Transform2D transform, Color color, Vector2 poseOffset, int frameOffset, int frameCount, int poseBaseTexel, int poseLayer, float interpolationT)
	{
		if (stateArena == null || bucket == null || frameCount <= 0)
		{
			return false;
		}
		int mark = stateArena.Mark();
		if (!stateArena.TryReserve(5, out var stateBaseTexel))
		{
			return false;
		}
		if (!bucket.TryAppend(transform, color, poseOffset, stateBaseTexel, 2f))
		{
			stateArena.Rollback(mark);
			return false;
		}
		WriteCrowdStateBufferInstance(stateArena.StateBuffer, stateBaseTexel * 4, frameOffset, frameCount, poseBaseTexel, poseLayer, interpolationT, 0f, 0f, clipEnabled: false, allLayersVisible: true, canUseLayerMask: false, 18446744073709551615uL, null, 0, 0, hasVisualOverrides: false, null, null, null, null, Vector2.Zero);
		return true;
	}

	internal int AppendPreparedCompactCrowdFrame(AdobeAnimateCrowdRenderState state)
	{
		if (state.Mode != AdobeAnimateCrowdRenderMode.Compact)
		{
			return 0;
		}
		return AppendCompactCrowdFrameCore(state.Definition, state.GlobalTransform, state.Modulate, state.Offset, state.VerticalClip.Enabled, state.VerticalClip.UpY, state.VerticalClip.DownY, state.FrameOffset, state.FrameCount, state.PoseBaseTexel, state.PoseLayer, state.InterpolationT, state.AllLayersVisible, state.CanUseLayerMask, state.LayerMask, state.LayerVisible, state.HasMediaReplace, state.MediaReplaceRect, state.MediaReplaceUse, state.MediaReplaceAtlasPages, state.MediaReplaceAtlasArraySize, state.RootMotion);
	}

	private int AppendCompactCrowdFrameCore(AdobeAnimateRuntimeDefinition definition, Transform2D transform, Color color, Vector2 poseOffset, bool clipEnabled, float clipUp, float clipDown, int frameOffset, int frameCount, int poseBaseTexel, int poseLayer, float interpolationT, bool allLayersVisible, bool canUseLayerMask, ulong layerMask, Array<bool> layerVisible, bool hasMediaReplace, Array<Rect2> mediaReplaceRect, Array<bool> mediaReplaceUse, Array<int> mediaReplaceAtlasPages, Vector2 mediaReplaceAtlasSize, AdobeAnimateRootMotionState rootMotion = default(AdobeAnimateRootMotionState))
	{
		if (definition == null || frameCount <= 0 || definition.MaxFrameSliceCount <= 0)
		{
			return 0;
		}
		int maxFrameSliceCount = definition.MaxFrameSliceCount;
		int crowdLayerVisibleCount = GetCrowdLayerVisibleCount(canUseLayerMask, layerVisible);
		int crowdLayerMaskTexelCount = GetCrowdLayerMaskTexelCount(allLayersVisible, crowdLayerVisibleCount);
		bool flag = HasCrowdVisualOverrides(definition, frameOffset, frameCount, hasMediaReplace, mediaReplaceUse, mediaReplaceRect);
		long num = 5 + crowdLayerMaskTexelCount + (flag ? ((long)frameCount * 2L) : 0);
		if (num <= 0 || num > 2147483647)
		{
			return 0;
		}
		int stateTexelCount = (int)num;
		if (!TryReserveCrowdStateTexels(stateTexelCount, out var stateBaseTexel))
		{
			return 0;
		}
		if (!TryAppendCrowdInstance(maxFrameSliceCount, transform, color, poseOffset, stateBaseTexel, 2f))
		{
			RollbackFailedCrowdInstance(stateBaseTexel);
			return 0;
		}
		WriteCrowdStateBufferInstance(_crowdStateArena.StateBuffer, stateBaseTexel * 4, frameOffset, frameCount, poseBaseTexel, poseLayer, interpolationT, clipUp, clipDown, clipEnabled, allLayersVisible, canUseLayerMask, layerMask, layerVisible, crowdLayerVisibleCount, crowdLayerMaskTexelCount, flag, definition, mediaReplaceRect, mediaReplaceUse, mediaReplaceAtlasPages, mediaReplaceAtlasSize, rootMotion);
		_compactCrowdRootCount++;
		return 1;
	}

	public int AppendGpuRenderGraphFrame(AdobeAnimateGpuRenderGraphAllocation allocation, AdobeAnimateGpuRenderGraphDefinition graph, Texture2DArray gpuRenderGraphTextureArray, Vector2I gpuRenderGraphTextureSize, ImageTexture gpuDynamicOverrideTexture, Vector2I gpuDynamicOverrideTextureSize, Transform2D rootTransform, IReadOnlyList<AdobeAnimateGpuGraphOwnerState> ownerStates, IReadOnlyList<AdobeAnimateGpuDynamicOverrideAllocation> overrideAllocations)
	{
		if (allocation.Signature == 0L || graph == null || graph.Signature != allocation.Signature || graph.Owners.Length != allocation.OwnerCount || ownerStates == null || overrideAllocations == null || overrideAllocations.Count != ownerStates.Count || allocation.OwnerCount != ownerStates.Count || ownerStates.Count <= 0 || allocation.RenderSlotCount <= 0 || !GodotObject.IsInstanceValid(gpuRenderGraphTextureArray) || gpuRenderGraphTextureSize.X <= 0 || gpuRenderGraphTextureSize.Y <= 0)
		{
			return 0;
		}
		if (GodotObject.IsInstanceValid(_gpuRenderGraphTextureArray) && _gpuRenderGraphTextureArray.GetRid() != gpuRenderGraphTextureArray.GetRid())
		{
			return 0;
		}
		bool flag = false;
		for (int i = 0; i < ownerStates.Count; i++)
		{
			flag |= overrideAllocations[i].Signature != 0;
		}
		if (flag && (!GodotObject.IsInstanceValid(gpuDynamicOverrideTexture) || gpuDynamicOverrideTextureSize.X <= 0 || gpuDynamicOverrideTextureSize.Y <= 0))
		{
			return 0;
		}
		if (_activeCrowdBucket == null)
		{
			ConfigureGpuRenderGraphTexture(gpuRenderGraphTextureArray, gpuRenderGraphTextureSize);
			ConfigureGpuDynamicOverrideTexture(gpuDynamicOverrideTexture, gpuDynamicOverrideTextureSize);
		}
		AdobeAnimateGpuManagedVisualState[] array = new AdobeAnimateGpuManagedVisualState[graph.ManagedVisualBindings.Length];
		for (int j = 0; j < graph.ManagedVisualBindings.Length; j++)
		{
			AdobeAnimateGpuManagedVisualBinding adobeAnimateGpuManagedVisualBinding = graph.ManagedVisualBindings[j];
			if ((uint)adobeAnimateGpuManagedVisualBinding.OwnerIndex >= (uint)ownerStates.Count)
			{
				return 0;
			}
			AdobeAnimateSprite sourceSprite = ownerStates[adobeAnimateGpuManagedVisualBinding.OwnerIndex].SourceSprite;
			if (!GodotObject.IsInstanceValid(sourceSprite))
			{
				return 0;
			}
			if (!(adobeAnimateGpuManagedVisualBinding.SourceKind switch
			{
				AdobeAnimateGpuManagedVisualSourceKind.SlotSprite2D => sourceSprite.TryBuildManagedSlotGpuStateForRender(adobeAnimateGpuManagedVisualBinding.VisualIndex, graph.Owners[adobeAnimateGpuManagedVisualBinding.OwnerIndex].Definition.AtlasTextureArrayRid, out array[j], out var failureReason), 
				AdobeAnimateGpuManagedVisualSourceKind.ExternalVisual => sourceSprite.TryBuildExternalVisualGpuStateForRender(adobeAnimateGpuManagedVisualBinding.VisualIndex, graph.Owners[adobeAnimateGpuManagedVisualBinding.OwnerIndex].Definition.AtlasTextureArrayRid, out array[j], out failureReason), 
				_ => false, 
			}))
			{
				return 0;
			}
		}
		if (!AdobeAnimateGpuGraphStateWriter.TryMeasure(in allocation, ownerStates, 0, ownerStates.Count, array.Length, out var layout))
		{
			return 0;
		}
		return AppendPreparedGpuRenderGraphFrame(in allocation, graph, rootTransform, ownerStates, 0, ownerStates.Count, overrideAllocations, 0, array, 0, array.Length, layout.StateTexelCount);
	}

	internal int AppendPreparedGpuRenderGraphFrame(in AdobeAnimateGpuRenderGraphAllocation allocation, AdobeAnimateGpuRenderGraphDefinition graph, Transform2D rootTransform, IReadOnlyList<AdobeAnimateGpuGraphOwnerState> ownerStates, int ownerStart, int ownerCount, IReadOnlyList<AdobeAnimateGpuDynamicOverrideAllocation> overrideAllocations, int overrideStart, IReadOnlyList<AdobeAnimateGpuManagedVisualState> managedVisualStates, int managedVisualStart, int managedVisualCount, int preparedStateTexelCount, AdobeAnimateRootMotionState rootMotion = default(AdobeAnimateRootMotionState))
	{
		if (allocation.Signature == 0L || graph == null || graph.Signature != allocation.Signature || graph.Owners.Length != allocation.OwnerCount || allocation.RenderSlotCount <= 0 || ownerStates == null || overrideAllocations == null || ownerStart < 0 || ownerCount <= 0 || ownerCount != allocation.OwnerCount || ownerStart > ownerStates.Count - ownerCount || overrideStart < 0 || overrideStart > overrideAllocations.Count - ownerCount || managedVisualStates == null || managedVisualStart < 0 || managedVisualCount != graph.ManagedVisualBindings.Length || managedVisualStart > managedVisualStates.Count - managedVisualCount || preparedStateTexelCount <= 0)
		{
			return 0;
		}
		if (!TryReserveCrowdStateTexels(preparedStateTexelCount, out var stateBaseTexel))
		{
			return 0;
		}
		bool flag = Mathf.IsZeroApprox(rootTransform.Determinant());
		Transform2D transform = (flag ? Transform2D.Identity : rootTransform);
		if (!TryAppendCrowdInstance(allocation.RenderSlotCount, transform, Colors.White, Vector2.Zero, stateBaseTexel, 5f))
		{
			RollbackFailedCrowdInstance(stateBaseTexel);
			return 0;
		}
		AdobeAnimateGpuGraphStateWriter.Write(_crowdStateArena.StateBuffer, stateBaseTexel, in allocation, graph, ownerStates, ownerStart, ownerCount, overrideAllocations, overrideStart, managedVisualStates, managedVisualStart, managedVisualCount, flag, in rootMotion, 0f);
		_gpuGraphRootCount++;
		_gpuGraphSlotCount += allocation.RenderSlotCount;
		_gpuGraphStateWrittenTexels += preparedStateTexelCount;
		return 1;
	}

	internal int AppendPreparedRelocatableGpuRenderGraphFrame(in AdobeAnimateGpuRenderGraphAllocation allocation, Transform2D rootTransform, float[] relocatableState, int ownerCount, int preparedStateTexelCount, bool useAbsoluteTransform, AdobeAnimateGpuGraphOwnerState rootOwnerState, AdobeAnimateRootMotionState rootMotion)
	{
		if (allocation.Signature == 0L || allocation.RenderSlotCount <= 0 || ownerCount <= 0 || ownerCount != allocation.OwnerCount || preparedStateTexelCount <= 0 || relocatableState == null || preparedStateTexelCount > 536870911 || relocatableState.Length < preparedStateTexelCount * 4)
		{
			return 0;
		}
		if (!TryReserveCrowdStateTexels(preparedStateTexelCount, out var stateBaseTexel))
		{
			return 0;
		}
		Transform2D transform = (useAbsoluteTransform ? Transform2D.Identity : rootTransform);
		if (!TryAppendPreparedGpuGraphInstance(allocation.RenderSlotCount, transform, stateBaseTexel) || !AdobeAnimateGpuGraphStateWriter.TryCopyRelocated(relocatableState, preparedStateTexelCount, ownerCount, _crowdStateArena.StateBuffer, stateBaseTexel) || !AdobeAnimateGpuGraphStateWriter.TryPatchRootMotion(_crowdStateArena.StateBuffer, stateBaseTexel, useAbsoluteTransform ? default(AdobeAnimateRootMotionState) : rootMotion) || !AdobeAnimateGpuGraphStateWriter.TryPatchRootOwnerDynamicState(_crowdStateArena.StateBuffer, stateBaseTexel, rootOwnerState, useAbsoluteTransform))
		{
			RollbackFailedCrowdInstance(stateBaseTexel);
			return 0;
		}
		_gpuGraphRootCount++;
		_gpuGraphSlotCount += allocation.RenderSlotCount;
		_gpuGraphStateWrittenTexels += preparedStateTexelCount;
		return 1;
	}

	private bool TryAppendPreparedGpuGraphInstance(int maxQuadCount, Transform2D transform, int stateBaseTexel)
	{
		if (_activeCrowdBucket != null)
		{
			return _activeCrowdBucket.AppendPreparedGpuGraph(transform, stateBaseTexel);
		}
		return TryAppendCrowdInstance(maxQuadCount, transform, Colors.White, Vector2.Zero, stateBaseTexel, 5f);
	}

	public int AppendCompositeCrowdFrame(Transform2D rootTransform, IReadOnlyList<AdobeAnimateDrawItem> items, int start, int count, AdobeAnimateRootMotionState rootMotion = default(AdobeAnimateRootMotionState))
	{
		if (items == null || count <= 0)
		{
			return 0;
		}
		int num = Math.Max(0, start);
		int num2 = Math.Min(items.Count, num + count);
		int num3 = num2 - num;
		if (num3 <= 0)
		{
			return 0;
		}
		int stateTexelCount = 3 + num3 * 6;
		if (!TryReserveCrowdStateTexels(stateTexelCount, out var stateBaseTexel))
		{
			return 0;
		}
		bool flag = Mathf.IsZeroApprox(rootTransform.Determinant());
		Transform2D transform = (flag ? Transform2D.Identity : rootTransform);
		Transform2D transform2D = (flag ? Transform2D.Identity : rootTransform.AffineInverse());
		if (!TryAppendCrowdInstance(num3, transform, Colors.White, Vector2.Zero, stateBaseTexel, 3f))
		{
			RollbackFailedCrowdInstance(stateBaseTexel);
			return 0;
		}
		float[] stateBuffer = _crowdStateArena.StateBuffer;
		int num4 = stateBaseTexel * 4;
		stateBuffer[num4] = num3;
		stateBuffer[num4 + 1] = 6f;
		stateBuffer[num4 + 2] = 1f;
		stateBuffer[num4 + 3] = 0f;
		WriteRootMotionState(stateBuffer, num4 + 4, flag ? default(AdobeAnimateRootMotionState) : rootMotion);
		int num5 = num;
		int num6 = 0;
		while (num5 < num2)
		{
			AdobeAnimateDrawItem item = items[num5];
			Transform2D commandTransform = (flag ? item.Transform : (transform2D * item.Transform));
			WriteCompositeCrowdCommand(stateBuffer, num4 + (3 + num6 * 6) * 4, item, commandTransform);
			num5++;
			num6++;
		}
		_compositeCrowdRootCount++;
		return 1;
	}

	public int AppendCachedCompositeCrowdFrame(Transform2D rootTransform, AdobeAnimateDrawItem[] items, int itemCount, int visualOverrideCount, int[] ownerIndices, int[] poseOffsets, IReadOnlyList<AdobeAnimateCompositePoseRefreshState> ownerStates)
	{
		return AppendPreparedCachedCompositeCrowdFrame(rootTransform, items, itemCount, visualOverrideCount, ownerIndices, poseOffsets, ownerStates, 0, ownerStates?.Count ?? 0);
	}

	internal int AppendPreparedCachedCompositeCrowdFrame(Transform2D rootTransform, AdobeAnimateDrawItem[] items, int itemCount, int visualOverrideCount, int[] ownerIndices, int[] poseOffsets, IReadOnlyList<AdobeAnimateCompositePoseRefreshState> ownerStates, int ownerStart, int ownerCount, AdobeAnimateRootMotionState rootMotion = default(AdobeAnimateRootMotionState))
	{
		if (items == null || ownerIndices == null || poseOffsets == null || ownerStates == null || itemCount <= 0 || itemCount > items.Length || itemCount > ownerIndices.Length || itemCount > poseOffsets.Length || visualOverrideCount < 0 || visualOverrideCount > itemCount || ownerStart < 0 || ownerCount <= 0 || ownerStart > ownerStates.Count - ownerCount)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < itemCount; i++)
		{
			int num2 = ownerIndices[i];
			if (num2 < 0 || num2 >= ownerCount)
			{
				return 0;
			}
			if (items[i].UseVisualOverride)
			{
				num++;
			}
		}
		if (num != visualOverrideCount)
		{
			return 0;
		}
		long num3 = 3 + (long)ownerCount * 4L + itemCount + (long)visualOverrideCount * 2L;
		if (num3 <= 0 || num3 > 2147483647)
		{
			return 0;
		}
		int stateTexelCount = (int)num3;
		if (!TryReserveCrowdStateTexels(stateTexelCount, out var stateBaseTexel))
		{
			return 0;
		}
		bool flag = Mathf.IsZeroApprox(rootTransform.Determinant());
		Transform2D transform = (flag ? Transform2D.Identity : rootTransform);
		Transform2D transform2D = (flag ? Transform2D.Identity : rootTransform.AffineInverse());
		EnsureCachedCompositeOwnerCapacity(ownerCount);
		for (int j = 0; j < ownerCount; j++)
		{
			AdobeAnimateCompositePoseRefreshState adobeAnimateCompositePoseRefreshState = ownerStates[ownerStart + j];
			Transform2D transform2D2 = (flag ? adobeAnimateCompositePoseRefreshState.GlobalTransform : (transform2D * adobeAnimateCompositePoseRefreshState.GlobalTransform));
			_cachedCompositeOwnerTransforms[j] = transform2D2;
			_cachedCompositeOwnerOrigins[j] = transform2D2.Origin + transform2D2.X * adobeAnimateCompositePoseRefreshState.Offset.X + transform2D2.Y * adobeAnimateCompositePoseRefreshState.Offset.Y;
		}
		if (!TryAppendCrowdInstance(itemCount, transform, Colors.White, Vector2.Zero, stateBaseTexel, 4f))
		{
			RollbackFailedCrowdInstance(stateBaseTexel);
			return 0;
		}
		float[] stateBuffer = _crowdStateArena.StateBuffer;
		int num4 = stateBaseTexel * 4;
		stateBuffer[num4] = itemCount;
		stateBuffer[num4 + 1] = 1f;
		stateBuffer[num4 + 2] = 3f;
		stateBuffer[num4 + 3] = ownerCount;
		WriteRootMotionState(stateBuffer, num4 + 4, flag ? default(AdobeAnimateRootMotionState) : rootMotion);
		for (int k = 0; k < ownerCount; k++)
		{
			WriteCachedCompositeCrowdOwnerState(stateBuffer, num4 + (3 + k * 4) * 4, _cachedCompositeOwnerTransforms[k], _cachedCompositeOwnerOrigins[k], ownerStates[ownerStart + k]);
		}
		int num5 = 3 + ownerCount * 4;
		int num6 = num5 + itemCount;
		int num7 = 0;
		for (int l = 0; l < itemCount; l++)
		{
			int num8 = ownerIndices[l];
			AdobeAnimateCompositePoseRefreshState adobeAnimateCompositePoseRefreshState2 = ownerStates[ownerStart + num8];
			int poseTexel = adobeAnimateCompositePoseRefreshState2.PoseBaseTexel + (adobeAnimateCompositePoseRefreshState2.FrameOffset + poseOffsets[l]) * 5;
			AdobeAnimateDrawItem item = items[l];
			WriteCachedCompositeCrowdCommand(stateBuffer, num4 + (num5 + l) * 4, poseTexel, num8, item.UseVisualOverride ? (num7 + 1) : 0);
			if (item.UseVisualOverride)
			{
				WriteCachedCompositeCrowdVisualOverride(stateBuffer, num4 + (num6 + num7 * 2) * 4, item);
				num7++;
			}
		}
		_compositeCrowdRootCount++;
		return 1;
	}

	private void EnsureCachedCompositeOwnerCapacity(int required)
	{
		if (_cachedCompositeOwnerTransforms == null || _cachedCompositeOwnerTransforms.Length < required)
		{
			int newSize = Math.Max(4, NextPowerOfTwo(required));
			System.Array.Resize(ref _cachedCompositeOwnerTransforms, newSize);
			System.Array.Resize(ref _cachedCompositeOwnerOrigins, newSize);
		}
	}

	private static void WriteCrowdStateBufferInstance(float[] buffer, int offset, int frameOffset, int frameCount, int poseBaseTexel, int poseLayer, float interpolationT, float clipUp, float clipDown, bool clipEnabled, bool allLayersVisible, bool canUseLayerMask, ulong layerMask, Array<bool> layerVisible, int layerVisibleCount, int maskTexelCount, bool hasVisualOverrides, AdobeAnimateRuntimeDefinition definition, Array<Rect2> mediaReplaceRect, Array<bool> mediaReplaceUse, Array<int> mediaReplaceAtlasPages, Vector2 mediaReplaceAtlasSize, AdobeAnimateRootMotionState rootMotion = default(AdobeAnimateRootMotionState))
	{
		buffer[offset] = Math.Max(0, frameOffset);
		buffer[offset + 1] = Math.Max(0, frameCount);
		buffer[offset + 2] = Math.Max(0, poseBaseTexel);
		buffer[offset + 3] = Math.Max(0, poseLayer);
		buffer[offset + 4] = Mathf.Clamp(interpolationT, 0f, 1f);
		buffer[offset + 5] = clipUp;
		buffer[offset + 6] = clipDown;
		buffer[offset + 7] = (clipEnabled ? 1f : 0f);
		buffer[offset + 8] = (allLayersVisible ? 0f : ((float)Math.Max(0, layerVisibleCount)));
		buffer[offset + 9] = Math.Max(0, maskTexelCount);
		buffer[offset + 10] = (hasVisualOverrides ? 1f : 0f);
		buffer[offset + 11] = 0f;
		WriteRootMotionState(buffer, offset + 12, in rootMotion);
		for (int i = 0; i < maskTexelCount; i++)
		{
			Color color = BuildCrowdLayerMaskTexel(canUseLayerMask, layerMask, layerVisible, layerVisibleCount, i);
			int num = offset + (5 + i) * 4;
			buffer[num] = color.R;
			buffer[num + 1] = color.G;
			buffer[num + 2] = color.B;
			buffer[num + 3] = color.A;
		}
		if (hasVisualOverrides)
		{
			WriteCrowdVisualOverrideData(buffer, offset, 5 + maskTexelCount, definition, frameOffset, frameCount, mediaReplaceRect, mediaReplaceUse, mediaReplaceAtlasPages, mediaReplaceAtlasSize);
		}
	}

	private static bool HasCrowdVisualOverrides(AdobeAnimateRuntimeDefinition definition, int frameOffset, int frameCount, bool hasMediaReplace, Array<bool> mediaReplaceUse, Array<Rect2> mediaReplaceRect)
	{
		if (!hasMediaReplace || definition?.SliceMetadata == null || mediaReplaceUse == null || mediaReplaceRect == null)
		{
			return false;
		}
		int num = Math.Max(0, frameOffset);
		int num2 = Math.Min(definition.SliceMetadata.Length, num + Math.Max(0, frameCount));
		for (int i = num; i < num2; i++)
		{
			if (IsMediaReplaceActive(definition.SliceMetadata[i].MediaId, mediaReplaceUse, mediaReplaceRect))
			{
				return true;
			}
		}
		return false;
	}

	private static void WriteCrowdVisualOverrideData(float[] buffer, int offset, int visualOverrideTexelOffset, AdobeAnimateRuntimeDefinition definition, int frameOffset, int frameCount, Array<Rect2> mediaReplaceRect, Array<bool> mediaReplaceUse, Array<int> mediaReplaceAtlasPages, Vector2 mediaReplaceAtlasSize)
	{
		if (definition?.SliceMetadata == null)
		{
			return;
		}
		Vector2 vector = ((mediaReplaceAtlasSize.X > 0f && mediaReplaceAtlasSize.Y > 0f) ? mediaReplaceAtlasSize : Vector2.One);
		int num = Math.Max(0, frameOffset);
		int num2 = Math.Max(0, frameCount);
		for (int i = 0; i < num2; i++)
		{
			int num3 = num + i;
			if (num3 >= 0 && num3 < definition.SliceMetadata.Length)
			{
				int mediaId = definition.SliceMetadata[num3].MediaId;
				if (IsMediaReplaceActive(mediaId, mediaReplaceUse, mediaReplaceRect))
				{
					Rect2 rect = mediaReplaceRect[mediaId];
					int num4 = ((mediaReplaceAtlasPages != null && mediaId < mediaReplaceAtlasPages.Count) ? Math.Max(0, mediaReplaceAtlasPages[mediaId]) : 0);
					int num5 = offset + (visualOverrideTexelOffset + i * 2) * 4;
					buffer[num5] = rect.Position.X / vector.X;
					buffer[num5 + 1] = rect.Position.Y / vector.Y;
					buffer[num5 + 2] = rect.Size.X / vector.X;
					buffer[num5 + 3] = rect.Size.Y / vector.Y;
					buffer[num5 + 4] = num4;
					buffer[num5 + 5] = 1f;
					buffer[num5 + 6] = 0f;
					buffer[num5 + 7] = 0f;
				}
			}
		}
	}

	private static bool IsMediaReplaceActive(int mediaId, Array<bool> mediaReplaceUse, Array<Rect2> mediaReplaceRect)
	{
		if (mediaId >= 0 && mediaReplaceUse != null && mediaId < mediaReplaceUse.Count && mediaReplaceUse[mediaId] && mediaReplaceRect != null && mediaId < mediaReplaceRect.Count && mediaReplaceRect[mediaId].Size.X > 0f)
		{
			return mediaReplaceRect[mediaId].Size.Y > 0f;
		}
		return false;
	}

	private static int GetCrowdLayerVisibleCount(bool canUseLayerMask, Array<bool> layerVisible)
	{
		if (layerVisible != null && layerVisible.Count > 0)
		{
			return layerVisible.Count;
		}
		if (!canUseLayerMask)
		{
			return 0;
		}
		return 64;
	}

	private static int GetCrowdLayerMaskTexelCount(bool allLayersVisible, int layerVisibleCount)
	{
		if (allLayersVisible || layerVisibleCount <= 0)
		{
			return 0;
		}
		return (layerVisibleCount + 64 - 1) / 64;
	}

	private static Color BuildCrowdLayerMaskTexel(bool canUseLayerMask, ulong layerMask, Array<bool> layerVisible, int layerVisibleCount, int maskTexelIndex)
	{
		int num = maskTexelIndex * 64;
		return new Color(BuildCrowdLayerMaskComponent(canUseLayerMask, layerMask, layerVisible, layerVisibleCount, num), BuildCrowdLayerMaskComponent(canUseLayerMask, layerMask, layerVisible, layerVisibleCount, num + 16), BuildCrowdLayerMaskComponent(canUseLayerMask, layerMask, layerVisible, layerVisibleCount, num + 32), BuildCrowdLayerMaskComponent(canUseLayerMask, layerMask, layerVisible, layerVisibleCount, num + 48));
	}

	private static float BuildCrowdLayerMaskComponent(bool canUseLayerMask, ulong layerMask, Array<bool> layerVisible, int layerVisibleCount, int layerBase)
	{
		if (layerBase >= layerVisibleCount)
		{
			return 0f;
		}
		if (canUseLayerMask && layerBase < 64)
		{
			return (int)(ushort)((layerMask >> layerBase) & 0xFFFF);
		}
		int num = 0;
		int num2 = Math.Min(layerVisibleCount, layerBase + 16);
		for (int i = layerBase; i < num2; i++)
		{
			if (layerVisible == null || i >= layerVisible.Count || layerVisible[i])
			{
				num |= 1 << i - layerBase;
			}
		}
		return num;
	}

	private static bool IsLayerVisible(ulong layerMask, int layerId)
	{
		if (layerId >= 0 && layerId < 64)
		{
			return (layerMask & (ulong)(1L << layerId)) != 0;
		}
		return true;
	}

	public void BeginFrame(TextureLayered atlasArray, Vector2 atlasSize)
	{
		BeginFrame(atlasArray, atlasSize, preserveVisibleInstances: false);
	}

	public void BeginFrame(TextureLayered atlasArray, Vector2 atlasSize, bool preserveVisibleInstances)
	{
		EnsureRenderObjects();
		Vector2 vector = ((atlasSize.X > 0f && atlasSize.Y > 0f) ? atlasSize : Vector2.One);
		bool num = _atlasArray != atlasArray || _atlasSize != vector;
		_atlasArray = atlasArray;
		_atlasSize = vector;
		if (num)
		{
			_material.SetShaderParameter(AtlasTextureArrayParameter, _atlasArray);
			_material.SetShaderParameter(AtlasSizeParameter, _atlasSize);
			if (_crowdMaterial != null)
			{
				_crowdMaterial.SetShaderParameter(AtlasTextureArrayParameter, _atlasArray);
				_crowdMaterial.SetShaderParameter(AtlasSizeParameter, _atlasSize);
			}
		}
		if (!preserveVisibleInstances)
		{
			if (_multiMesh != null)
			{
				_multiMesh.VisibleInstanceCount = 0;
			}
			if (_crowdMultiMesh != null)
			{
				_crowdMultiMesh.VisibleInstanceCount = 0;
			}
		}
	}

	public void Flush(IReadOnlyList<AdobeAnimateDrawItem> items, TextureLayered poseTextureArray, Vector2I poseTextureSize)
	{
		EnsureRenderObjects();
		ConfigurePoseTexture(poseTextureArray, poseTextureSize);
		if (items == null || items.Count == 0)
		{
			if (_multiMesh != null)
			{
				_multiMesh.VisibleInstanceCount = 0;
			}
			if (_crowdMultiMesh != null)
			{
				_crowdMultiMesh.VisibleInstanceCount = 0;
			}
		}
		else
		{
			int count = items.Count;
			BeginBuffered(poseTextureArray, poseTextureSize);
			for (int i = 0; i < count; i++)
			{
				AppendBuffered(items[i]);
			}
			EndBuffered();
		}
	}

	public int GetVisibleInstanceCountForTest()
	{
		return (_multiMesh?.VisibleInstanceCount ?? 0) + (_crowdMultiMesh?.VisibleInstanceCount ?? 0);
	}

	public int GetBufferedInstanceCountForTest()
	{
		return _bufferedCount + _crowdBufferedCount;
	}

	public int GetCompactCrowdRootCountForTest()
	{
		return _compactCrowdRootCount;
	}

	public int GetCompositeCrowdRootCountForTest()
	{
		return _compositeCrowdRootCount;
	}

	public int GetGpuGraphRootCountForTest()
	{
		return _gpuGraphRootCount;
	}

	public int GetGpuGraphSlotCountForTest()
	{
		return _gpuGraphSlotCount;
	}

	public int GetGpuGraphStateWrittenTexelsForTest()
	{
		return _gpuGraphStateWrittenTexels;
	}

	public int GetCrowdStateWrittenTexelsForTest()
	{
		return _crowdStateArena?.WrittenTexels ?? 0;
	}

	public void BeginBuffered(TextureLayered poseTextureArray, Vector2I poseTextureSize)
	{
		BeginBuffered(poseTextureArray, poseTextureSize, _ownedCrowdStateArena, ++_standaloneCrowdFrameVersion);
	}

	internal void BeginGpuGraphBuffered(TextureLayered poseTextureArray, Vector2I poseTextureSize, Texture2DArray gpuRenderGraphTextureArray, Vector2I gpuRenderGraphTextureSize)
	{
		BeginBuffered(poseTextureArray, poseTextureSize);
		ConfigureBufferedGpuGraphTexture(gpuRenderGraphTextureArray, gpuRenderGraphTextureSize);
	}

	internal void BeginGpuGraphBuffered(TextureLayered poseTextureArray, Vector2I poseTextureSize, Texture2DArray gpuRenderGraphTextureArray, Vector2I gpuRenderGraphTextureSize, AdobeAnimateSharedStateArena crowdStateArena, long frameVersion)
	{
		BeginBuffered(poseTextureArray, poseTextureSize, crowdStateArena, frameVersion);
		ConfigureBufferedGpuGraphTexture(gpuRenderGraphTextureArray, gpuRenderGraphTextureSize);
	}

	internal void ConfigureBufferedGpuGraphTexture(Texture2DArray gpuRenderGraphTextureArray, Vector2I gpuRenderGraphTextureSize)
	{
		ConfigureGpuRenderGraphTexture(gpuRenderGraphTextureArray, gpuRenderGraphTextureSize);
	}

	internal void ConfigureBufferedGpuDynamicOverrideTexture(ImageTexture gpuDynamicOverrideTexture, Vector2I gpuDynamicOverrideTextureSize)
	{
		ConfigureGpuDynamicOverrideTexture(gpuDynamicOverrideTexture, gpuDynamicOverrideTextureSize);
	}

	internal void ApplyStandaloneCrowdClock(float seconds, float physicsInterpolationFraction)
	{
		ShaderMaterial crowdMaterial = _crowdMaterial;
		if (GodotObject.IsInstanceValid(crowdMaterial))
		{
			SetSharedCrowdAnimationClock(crowdMaterial, seconds);
			SetSharedCrowdPhysicsInterpolationFraction(crowdMaterial, physicsInterpolationFraction);
		}
	}

	internal void BeginBuffered(TextureLayered poseTextureArray, Vector2I poseTextureSize, AdobeAnimateSharedStateArena crowdStateArena, long frameVersion)
	{
		EnsureRenderObjects();
		ConfigurePoseTexture(poseTextureArray, poseTextureSize);
		_crowdStateArena = crowdStateArena ?? _ownedCrowdStateArena;
		_crowdStateArena.BeginFrame(frameVersion);
		ResetCrowdFrameState();
	}

	internal void BeginSharedCrowdFrame(AdobeAnimateSharedStateArena arena, long frameVersion)
	{
		if (arena == null)
		{
			throw new ArgumentNullException("arena");
		}
		_crowdStateArena = arena;
		_crowdStateArena.BeginFrame(frameVersion);
		ResetCrowdFrameState();
	}

	private void ResetCrowdFrameState()
	{
		ClearCrowdGroupState();
		_bufferedCount = 0;
		_crowdBufferedCount = 0;
		_compactCrowdRootCount = 0;
		_compositeCrowdRootCount = 0;
		_gpuGraphRootCount = 0;
		_gpuGraphSlotCount = 0;
		_gpuGraphStateWrittenTexels = 0;
		_gpuRenderGraphTextureArray = null;
		_gpuRenderGraphTextureSize = Vector2I.Zero;
		_gpuDynamicOverrideTexture = null;
		_gpuDynamicOverrideTextureSize = Vector2I.Zero;
		_crowdStateOverflowReported = false;
	}

	public void AppendBuffered(in AdobeAnimateDrawItem item)
	{
		int bufferedCount = _bufferedCount;
		EnsureCapacity(bufferedCount + 1);
		EnsureInstanceDataTexture(Math.Max(1, (bufferedCount + 1) * 3));
		WriteMultiMeshBufferInstance(_multiMeshBuffer, bufferedCount * 16, item, bufferedCount);
		WriteInstanceDataBufferInstance(_instanceDataBuffer, bufferedCount * 3 * 4, item, _poseTextureSize.X);
		_bufferedCount = bufferedCount + 1;
	}

	public void AppendBuffered(IReadOnlyList<AdobeAnimateDrawItem> items, int start, int count)
	{
		if (items != null && count > 0)
		{
			int num = Math.Min(items.Count, start + count);
			for (int i = Math.Max(0, start); i < num; i++)
			{
				AppendBuffered(items[i]);
			}
		}
	}

	public int AppendShaderPoseFrame(Transform2D transform, Color color, Vector2 poseOffset, bool clipEnabled, float clipUp, float clipDown, AdobeAnimateRuntimeDefinition definition, int start, int count, bool allLayersVisible, ulong layerMask, int poseBaseTexel, int poseLayer, float interpolationT)
	{
		PackedSliceMetadata[] array = definition?.SliceMetadata;
		if (array == null || count <= 0)
		{
			return 0;
		}
		int num = Math.Max(0, start);
		int num2 = Math.Min(array.Length, start + count);
		if (num2 <= num)
		{
			return 0;
		}
		int num3 = num2 - num;
		int num4 = _bufferedCount + num3;
		EnsureCapacity(num4);
		EnsureInstanceDataTexture(Math.Max(1, num4 * 3));
		int num5 = 0;
		poseBaseTexel = Math.Max(0, poseBaseTexel);
		poseLayer = Math.Max(0, poseLayer);
		interpolationT = Mathf.Clamp(interpolationT, 0f, 1f);
		for (int i = num; i < num2; i++)
		{
			ref PackedSliceMetadata reference = ref array[i];
			if (AdobeAnimateDefinitionCache.IsBaseSliceRenderable(definition, reference.MediaId) && (allLayersVisible || IsLayerVisible(layerMask, reference.LayerId)))
			{
				int num6 = _bufferedCount + num5;
				WriteShaderPoseMultiMeshBufferInstance(_multiMeshBuffer, num6 * 16, transform, color, poseOffset, poseLayer, interpolationT, num6);
				WriteShaderPoseInstanceDataBufferInstance(_instanceDataBuffer, num6 * 3 * 4, poseBaseTexel + i * 5, _poseTextureSize.X, clipUp, clipDown, clipEnabled);
				num5++;
			}
		}
		_bufferedCount += num5;
		return num5;
	}

	private static void WriteInstanceDataBufferInstance(float[] buffer, int offset, AdobeAnimateDrawItem item, int poseTextureWidth)
	{
		if (item.UseShaderPose)
		{
			WriteShaderPoseInstanceDataBufferInstance(buffer, offset, item.PoseTexel, poseTextureWidth, item.ClipUp, item.ClipDown, item.ClipEnabled, item.UseVisualOverride, item.UvRect, item.AtlasLayer, item.ClipLeft, item.ClipRight);
			return;
		}
		buffer[offset] = item.UvRect.Position.X;
		buffer[offset + 1] = item.UvRect.Position.Y;
		buffer[offset + 2] = item.UvRect.Size.X;
		buffer[offset + 3] = item.UvRect.Size.Y;
		buffer[offset + 4] = item.AtlasLayer;
		buffer[offset + 5] = item.ClipUp;
		buffer[offset + 6] = item.ClipDown;
		buffer[offset + 7] = (item.ClipEnabled ? 1f : 0f);
		buffer[offset + 8] = item.ClipLeft;
		buffer[offset + 9] = item.ClipRight;
		buffer[offset + 10] = 0f;
		buffer[offset + 11] = 0f;
	}

	private void EnsureInstanceDataTexture(int requiredTexels)
	{
		if (_instanceDataImage == null || !GodotObject.IsInstanceValid(_instanceDataTexture) || _instanceDataTexelCapacity < requiredTexels || _instanceDataTextureSize.X <= 0 || _instanceDataTextureSize.Y <= 0)
		{
			_instanceDataTexelCapacity = Math.Max(1, NextPowerOfTwo(requiredTexels));
			int num = Math.Min(2048, _instanceDataTexelCapacity);
			int num2 = Math.Max(1, (_instanceDataTexelCapacity + num - 1) / num);
			_instanceDataTextureSize = new Vector2I(num, num2);
			_instanceDataTexelCapacity = num * num2;
			System.Array.Resize(ref _instanceDataBuffer, _instanceDataTexelCapacity * 4);
			_instanceDataImage = Image.CreateEmpty(num, num2, useMipmaps: false, Image.Format.Rgbaf);
			_instanceDataImage.Fill(new Color(0f, 0f, 0f, 0f));
			_instanceDataTexture = ImageTexture.CreateFromImage(_instanceDataImage);
		}
	}

	private void ConfigurePoseTexture(TextureLayered poseTextureArray, Vector2I poseTextureSize)
	{
		if (_poseTextureArray != poseTextureArray || !(_poseTextureSize == poseTextureSize))
		{
			_poseTextureArray = poseTextureArray;
			_poseTextureSize = poseTextureSize;
			ApplyPoseTextureToMaterial(_material);
			if (_crowdMaterial != null)
			{
				ApplyPoseTextureToMaterial(_crowdMaterial);
			}
		}
	}

	private void ConfigureGpuRenderGraphTexture(Texture2DArray textureArray, Vector2I textureSize)
	{
		if (!GodotObject.IsInstanceValid(_gpuRenderGraphTextureArray) || !GodotObject.IsInstanceValid(textureArray) || !(_gpuRenderGraphTextureArray.GetRid() == textureArray.GetRid()) || !(_gpuRenderGraphTextureSize == textureSize))
		{
			_gpuRenderGraphTextureArray = textureArray;
			_gpuRenderGraphTextureSize = textureSize;
			ApplyGpuRenderGraphTextureToMaterial(_crowdMaterial);
		}
	}

	private void ConfigureGpuDynamicOverrideTexture(ImageTexture texture, Vector2I textureSize)
	{
		if (!GodotObject.IsInstanceValid(_gpuDynamicOverrideTexture) || !GodotObject.IsInstanceValid(texture) || !(_gpuDynamicOverrideTexture.GetRid() == texture.GetRid()) || !(_gpuDynamicOverrideTextureSize == textureSize))
		{
			_gpuDynamicOverrideTexture = texture;
			_gpuDynamicOverrideTextureSize = textureSize;
			ApplyGpuDynamicOverrideTextureToMaterial(_crowdMaterial);
		}
	}

	private void ApplyPoseTextureToMaterial(ShaderMaterial material)
	{
		if (material != null)
		{
			TextureLayered poseTextureArray = _poseTextureArray;
			Vector2I poseTextureSize = _poseTextureSize;
			if (GodotObject.IsInstanceValid(poseTextureArray) && poseTextureSize.X > 0 && poseTextureSize.Y > 0)
			{
				material.SetShaderParameter(PoseTextureArrayParameter, poseTextureArray);
				material.SetShaderParameter(PoseTextureSizeParameter, new Vector2(poseTextureSize.X, poseTextureSize.Y));
			}
			else
			{
				material.SetShaderParameter(PoseTextureSizeParameter, Vector2.One);
			}
		}
	}

	private void ApplyGpuRenderGraphTextureToMaterial(ShaderMaterial material)
	{
		if (material != null)
		{
			if (GodotObject.IsInstanceValid(_gpuRenderGraphTextureArray) && _gpuRenderGraphTextureSize.X > 0 && _gpuRenderGraphTextureSize.Y > 0)
			{
				material.SetShaderParameter(GpuRenderGraphTextureArrayParameter, _gpuRenderGraphTextureArray);
				material.SetShaderParameter(GpuRenderGraphTextureSizeParameter, new Vector2(_gpuRenderGraphTextureSize.X, _gpuRenderGraphTextureSize.Y));
			}
			else
			{
				material.SetShaderParameter(GpuRenderGraphTextureSizeParameter, Vector2.One);
			}
		}
	}

	private void ApplyGpuDynamicOverrideTextureToMaterial(ShaderMaterial material)
	{
		if (material != null)
		{
			if (GodotObject.IsInstanceValid(_gpuDynamicOverrideTexture) && _gpuDynamicOverrideTextureSize.X > 0 && _gpuDynamicOverrideTextureSize.Y > 0)
			{
				material.SetShaderParameter(GpuDynamicOverrideTextureParameter, _gpuDynamicOverrideTexture);
				material.SetShaderParameter(GpuDynamicOverrideTextureSizeParameter, new Vector2(_gpuDynamicOverrideTextureSize.X, _gpuDynamicOverrideTextureSize.Y));
			}
			else
			{
				material.SetShaderParameter(GpuDynamicOverrideTextureSizeParameter, Vector2.One);
			}
		}
	}

	private static void ApplyInstanceDataLayoutToMaterial(ShaderMaterial material)
	{
		material?.SetShaderParameter(InstanceDataStrideParameter, 3f);
	}

	private static int NextPowerOfTwo(int value)
	{
		int num;
		for (num = 1; num < value; num <<= 1)
		{
		}
		return num;
	}

	private static ArrayMesh CreateUnitQuadMesh()
	{
		Vector3[] array = new Vector3[4]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 1f, 0f),
			new Vector3(1f, 1f, 0f),
			new Vector3(1f, 0f, 0f)
		};
		Vector2[] array2 = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
		int[] array3 = new int[6] { 0, 1, 2, 2, 3, 0 };
		Godot.Collections.Array array4 = new Godot.Collections.Array();
		array4.Resize(13);
		array4[0] = array;
		array4[4] = array2;
		array4[12] = array3;
		ArrayMesh arrayMesh = new ArrayMesh();
		arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, array4, null, null, (Mesh.ArrayFormat)0L);
		return arrayMesh;
	}

	private static ArrayMesh CreateCrowdMesh(int maxSlices)
	{
		maxSlices = Math.Max(1, maxSlices);
		Vector3[] array = new Vector3[maxSlices * 4];
		Vector2[] array2 = new Vector2[maxSlices * 4];
		int[] array3 = new int[maxSlices * 6];
		for (int i = 0; i < maxSlices; i++)
		{
			int num = i * 4;
			int num2 = i * 6;
			float num3 = (float)i * 2f;
			array[num] = new Vector3(0f, num3 + 0f, 0f);
			array[num + 1] = new Vector3(0f, num3 + 1f, 0f);
			array[num + 2] = new Vector3(1f, num3 + 1f, 0f);
			array[num + 3] = new Vector3(1f, num3 + 0f, 0f);
			array2[num] = new Vector2(0f, 0f);
			array2[num + 1] = new Vector2(0f, 1f);
			array2[num + 2] = new Vector2(1f, 1f);
			array2[num + 3] = new Vector2(1f, 0f);
			array3[num2] = num;
			array3[num2 + 1] = num + 1;
			array3[num2 + 2] = num + 2;
			array3[num2 + 3] = num + 2;
			array3[num2 + 4] = num + 3;
			array3[num2 + 5] = num;
		}
		Godot.Collections.Array array4 = new Godot.Collections.Array();
		array4.Resize(13);
		array4[0] = array;
		array4[4] = array2;
		array4[12] = array3;
		ArrayMesh arrayMesh = new ArrayMesh();
		arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, array4, null, null, (Mesh.ArrayFormat)0L);
		return arrayMesh;
	}

	internal int GetCurrentCrowdGroupStateCursor()
	{
		if (_activeCrowdBucket == null)
		{
			return -1;
		}
		return _crowdGroupCursorTexel;
	}

	public void EndBuffered()
	{
		if (_bufferedCount <= 0 && _multiMesh != null)
		{
			_multiMesh.VisibleInstanceCount = 0;
		}
		if (_crowdBufferedCount <= 0 && _crowdMultiMesh != null)
		{
			_crowdMultiMesh.VisibleInstanceCount = 0;
		}
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		if (_bufferedCount > 0)
		{
			UploadBufferedData(_bufferedCount);
			_multiMesh.VisibleInstanceCount = _bufferedCount;
		}
		if (_crowdBufferedCount > 0)
		{
			if (UploadCrowdStateOnce())
			{
				UploadCrowdBufferedData(_crowdBufferedCount);
				_crowdMultiMesh.VisibleInstanceCount = _crowdBufferedCount;
			}
			else
			{
				_crowdMultiMesh.VisibleInstanceCount = 0;
			}
		}
		TowerDefensePerfProfiler.End("adobeAnimate.render.batch.uploadBuffers", startTicks, _bufferedCount + _crowdBufferedCount);
	}

	private void EnsureRenderObjects()
	{
		if (_quadMesh == null)
		{
			_quadMesh = CreateUnitQuadMesh();
		}
		if (_multiMesh == null)
		{
			_multiMesh = new MultiMesh
			{
				TransformFormat = MultiMesh.TransformFormatEnum.Transform2D,
				Mesh = _quadMesh,
				UseColors = true,
				UseCustomData = true,
				InstanceCount = 0,
				VisibleInstanceCount = 0
			};
			AdobeAnimateZIndexCrowdBucket.ApplyShaderDrivenCustomAabb(_multiMesh);
		}
		if (_material == null)
		{
			_material = new ShaderMaterial
			{
				Shader = GD.Load<Shader>("res://addons/AdobeAnimateEditor/Rendering/AdobeAnimateManagedMultiMesh.gdshader")
			};
			ApplyInstanceDataLayoutToMaterial(_material);
			_material.SetShaderParameter(AtlasTextureArrayParameter, _atlasArray);
			_material.SetShaderParameter(AtlasSizeParameter, _atlasSize);
			ApplyPoseTextureToMaterial(_material);
		}
		if (_instance == null)
		{
			_instance = new MultiMeshInstance2D
			{
				Name = "AdobeAnimateUnifiedMultiMesh",
				Multimesh = _multiMesh,
				Material = _material
			};
		}
		if (_instance.GetParent() == null)
		{
			AddChild(_instance, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void EnsureCrowdRenderObjects(int maxSlices)
	{
		maxSlices = Math.Max(1, maxSlices);
		if (_crowdMeshMaxSlices >= maxSlices && _crowdMesh != null && _crowdMultiMesh != null && _crowdMaterial != null && _crowdInstance != null)
		{
			return;
		}
		if (_crowdMesh == null || _crowdMeshMaxSlices < maxSlices)
		{
			_crowdMesh = CreateCrowdMesh(maxSlices);
			_crowdMeshMaxSlices = maxSlices;
			if (_crowdMultiMesh != null)
			{
				_crowdMultiMesh.Mesh = _crowdMesh;
			}
		}
		if (_crowdMultiMesh == null)
		{
			_crowdMultiMesh = new MultiMesh
			{
				TransformFormat = MultiMesh.TransformFormatEnum.Transform2D,
				Mesh = _crowdMesh,
				UseColors = true,
				UseCustomData = true,
				InstanceCount = 0,
				VisibleInstanceCount = 0
			};
			AdobeAnimateZIndexCrowdBucket.ApplyShaderDrivenCustomAabb(_crowdMultiMesh);
		}
		if (_crowdMaterial == null)
		{
			_crowdMaterial = new ShaderMaterial
			{
				Shader = GD.Load<Shader>("res://addons/AdobeAnimateEditor/Rendering/AdobeAnimateManagedMultiMesh.gdshader")
			};
			ApplyInstanceDataLayoutToMaterial(_crowdMaterial);
			_crowdMaterial.SetShaderParameter(AtlasTextureArrayParameter, _atlasArray);
			_crowdMaterial.SetShaderParameter(AtlasSizeParameter, _atlasSize);
			ApplyPoseTextureToMaterial(_crowdMaterial);
			ApplyGpuRenderGraphTextureToMaterial(_crowdMaterial);
			ApplyGpuDynamicOverrideTextureToMaterial(_crowdMaterial);
		}
		if (_crowdInstance == null)
		{
			_crowdInstance = new MultiMeshInstance2D
			{
				Name = "AdobeAnimateUnifiedCrowdMultiMesh",
				Multimesh = _crowdMultiMesh,
				Material = _crowdMaterial
			};
			AddChild(_crowdInstance, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void EnsureCapacity(int count)
	{
		if (count > _capacity)
		{
			_capacity = Math.Max(128, NextPowerOfTwo(count));
			_multiMesh.InstanceCount = _capacity;
			System.Array.Resize(ref _multiMeshBuffer, _capacity * 16);
		}
	}

	private void EnsureCrowdCapacity(int count)
	{
		if (count > _crowdCapacity)
		{
			_crowdCapacity = Math.Max(128, NextPowerOfTwo(count));
			_crowdMultiMesh.InstanceCount = _crowdCapacity;
			System.Array.Resize(ref _crowdMultiMeshBuffer, _crowdCapacity * 16);
		}
	}

	internal bool BeginCrowdGroup(AdobeAnimateZIndexCrowdBucket bucket, int totalStateTexels, int expectedRootCount)
	{
		if (bucket == null || _crowdStateArena == null || _activeCrowdBucket != null || totalStateTexels < 0 || expectedRootCount < 0)
		{
			return false;
		}
		_crowdGroupArenaMark = _crowdStateArena.Mark();
		_crowdGroupBucketMark = bucket.Mark();
		if (_crowdGroupBucketMark > 2147483647 - expectedRootCount)
		{
			ClearCrowdGroupState();
			return false;
		}
		_crowdGroupExpectedBucketEnd = _crowdGroupBucketMark + expectedRootCount;
		if (!_crowdStateArena.TryReserve(totalStateTexels, out _crowdGroupCursorTexel))
		{
			ClearCrowdGroupState();
			return false;
		}
		_crowdGroupEndTexel = _crowdGroupCursorTexel + totalStateTexels;
		_activeCrowdBucket = bucket;
		return true;
	}

	internal bool CommitCrowdGroup()
	{
		if (_activeCrowdBucket == null || _crowdGroupCursorTexel != _crowdGroupEndTexel || _activeCrowdBucket.InstanceCount != _crowdGroupExpectedBucketEnd)
		{
			RollbackCrowdGroup();
			return false;
		}
		ClearCrowdGroupState();
		return true;
	}

	internal void RollbackCrowdGroup()
	{
		_crowdStateArena?.Rollback(_crowdGroupArenaMark);
		_activeCrowdBucket?.Rollback(_crowdGroupBucketMark);
		ClearCrowdGroupState();
	}

	private void ClearCrowdGroupState()
	{
		_activeCrowdBucket = null;
		_crowdGroupArenaMark = -1;
		_crowdGroupBucketMark = -1;
		_crowdGroupExpectedBucketEnd = -1;
		_crowdGroupCursorTexel = -1;
		_crowdGroupEndTexel = -1;
	}

	private bool TryReserveCrowdStateTexels(int stateTexelCount, out int stateBaseTexel)
	{
		stateBaseTexel = -1;
		if (_activeCrowdBucket != null)
		{
			if (stateTexelCount < 0 || _crowdGroupCursorTexel < 0 || _crowdGroupCursorTexel > _crowdGroupEndTexel - stateTexelCount)
			{
				return false;
			}
			stateBaseTexel = _crowdGroupCursorTexel;
			_crowdGroupCursorTexel += stateTexelCount;
			return true;
		}
		if (_crowdStateArena != null && _crowdStateArena.TryReserve(stateTexelCount, out stateBaseTexel))
		{
			return true;
		}
		if (!_crowdStateOverflowReported)
		{
			_crowdStateOverflowReported = true;
			GD.PushError("AdobeAnimate Crowd state arena could not reserve the requested RGBAF texels.");
		}
		return false;
	}

	private bool TryAppendCrowdInstance(int maxQuadCount, Transform2D transform, Color color, Vector2 poseOffset, int stateBaseTexel, float mode)
	{
		if (_activeCrowdBucket != null)
		{
			return _activeCrowdBucket.AppendPrepared(transform, color, poseOffset, stateBaseTexel, mode);
		}
		EnsureCrowdRenderObjects(maxQuadCount);
		int crowdBufferedCount = _crowdBufferedCount;
		EnsureCrowdCapacity(crowdBufferedCount + 1);
		AdobeAnimateZIndexCrowdBucket.WriteInstanceBuffer(_crowdMultiMeshBuffer, crowdBufferedCount * 16, transform, color, poseOffset, stateBaseTexel, mode);
		_crowdBufferedCount = crowdBufferedCount + 1;
		return true;
	}

	private void RollbackFailedCrowdInstance(int standaloneStateMark)
	{
		if (_activeCrowdBucket != null)
		{
			RollbackCrowdGroup();
		}
		else
		{
			_crowdStateArena?.Rollback(standaloneStateMark);
		}
	}

	private void UploadBufferedData(int count)
	{
		int length = _instanceDataTextureSize.X * _instanceDataTextureSize.Y * 4;
		ReadOnlySpan<byte> data = MemoryMarshal.AsBytes(_instanceDataBuffer.AsSpan(0, length));
		_instanceDataImage.SetData(_instanceDataTextureSize.X, _instanceDataTextureSize.Y, useMipmaps: false, Image.Format.Rgbaf, data);
		_instanceDataTexture.Update(_instanceDataImage);
		_material.SetShaderParameter(InstanceDataTextureParameter, _instanceDataTexture);
		_material.SetShaderParameter(InstanceDataTextureSizeParameter, new Vector2(_instanceDataTextureSize.X, _instanceDataTextureSize.Y));
		if (_multiMesh.InstanceCount != count)
		{
			_multiMesh.InstanceCount = count;
		}
		RenderingServer.MultimeshSetBuffer(_multiMesh.GetRid(), _multiMeshBuffer.AsSpan(0, count * 16));
	}

	private void UploadCrowdBufferedData(int count)
	{
		if (_crowdMultiMesh.InstanceCount != count)
		{
			_crowdMultiMesh.InstanceCount = count;
		}
		RenderingServer.MultimeshSetBuffer(_crowdMultiMesh.GetRid(), _crowdMultiMeshBuffer.AsSpan(0, count * 16));
	}

	private bool UploadCrowdStateOnce()
	{
		if (_crowdStateArena == null || !_crowdStateArena.UploadOnce())
		{
			return false;
		}
		TextureLayered texture = _crowdStateArena.Texture;
		Vector2I textureSize = _crowdStateArena.TextureSize;
		if (_boundCrowdStateTexture != texture)
		{
			_crowdMaterial.SetShaderParameter(CrowdStateTextureParameter, texture);
			_boundCrowdStateTexture = texture;
		}
		if (_boundCrowdStateTextureSize != textureSize)
		{
			_crowdMaterial.SetShaderParameter(CrowdStateTextureSizeParameter, new Vector2(textureSize.X, textureSize.Y));
			_boundCrowdStateTextureSize = textureSize;
		}
		return true;
	}

	public override void _Ready()
	{
		if (!EncoderOnly)
		{
			EnsureRenderObjects();
		}
	}

	internal static ShaderMaterial CreateSharedCrowdMaterial()
	{
		Shader shader = GD.Load<Shader>("res://addons/AdobeAnimateEditor/Rendering/AdobeAnimateManagedMultiMesh.gdshader");
		if (!GodotObject.IsInstanceValid(shader))
		{
			return null;
		}
		ShaderMaterial shaderMaterial = new ShaderMaterial
		{
			Shader = shader
		};
		ApplyInstanceDataLayoutToMaterial(shaderMaterial);
		return shaderMaterial;
	}

	internal static ArrayMesh CreateSharedCrowdMesh(int maxQuadCount)
	{
		return CreateCrowdMesh(Math.Max(1, maxQuadCount));
	}

	internal static void SetSharedCrowdAnimationClock(ShaderMaterial material, float seconds)
	{
		material.SetShaderParameter(AnimationClockParameter, Math.Max(0f, seconds));
	}

	internal static void SetSharedCrowdPhysicsInterpolationFraction(ShaderMaterial material, float fraction)
	{
		material.SetShaderParameter(PhysicsInterpolationFractionParameter, Mathf.Clamp(fraction, 0f, 1f));
	}

	internal static void ApplySharedCrowdBindings(ShaderMaterial material, in AdobeAnimateCrowdResourceSignature signature, AdobeAnimateSharedStateArena arena)
	{
		if (GodotObject.IsInstanceValid(material) && arena != null)
		{
			material.SetShaderParameter(AtlasTextureArrayParameter, signature.VisualAtlas);
			material.SetShaderParameter(AtlasSizeParameter, (signature.VisualAtlasSize.X > 0f && signature.VisualAtlasSize.Y > 0f) ? signature.VisualAtlasSize : Vector2.One);
			material.SetShaderParameter(PoseTextureArrayParameter, signature.PoseTexture);
			material.SetShaderParameter(PoseTextureSizeParameter, (signature.PoseTextureSize.X > 0 && signature.PoseTextureSize.Y > 0) ? new Vector2(signature.PoseTextureSize.X, signature.PoseTextureSize.Y) : Vector2.One);
			material.SetShaderParameter(GpuRenderGraphTextureArrayParameter, signature.GpuGraphTexture);
			material.SetShaderParameter(GpuRenderGraphTextureSizeParameter, (signature.GpuGraphTextureSize.X > 0 && signature.GpuGraphTextureSize.Y > 0) ? new Vector2(signature.GpuGraphTextureSize.X, signature.GpuGraphTextureSize.Y) : Vector2.One);
			material.SetShaderParameter(GpuDynamicOverrideTextureParameter, signature.DynamicOverrideTexture);
			material.SetShaderParameter(GpuDynamicOverrideTextureSizeParameter, (signature.DynamicOverrideTextureSize.X > 0 && signature.DynamicOverrideTextureSize.Y > 0) ? new Vector2(signature.DynamicOverrideTextureSize.X, signature.DynamicOverrideTextureSize.Y) : Vector2.One);
			material.SetShaderParameter(RasterCompositeAtlasParameter, signature.RasterCompositeAtlas);
			material.SetShaderParameter(RasterCompositeAtlasSizeParameter, (signature.RasterCompositeAtlasSize.X > 0 && signature.RasterCompositeAtlasSize.Y > 0) ? new Vector2(signature.RasterCompositeAtlasSize.X, signature.RasterCompositeAtlasSize.Y) : Vector2.One);
			material.SetShaderParameter(RasterCompositeTileSizeParameter, (signature.RasterCompositeTileSize.X > 0 && signature.RasterCompositeTileSize.Y > 0) ? new Vector2(signature.RasterCompositeTileSize.X, signature.RasterCompositeTileSize.Y) : Vector2.One);
			material.SetShaderParameter(RasterCompositeOriginParameter, signature.RasterCompositeOrigin);
			material.SetShaderParameter(RasterCompositeColumnsParameter, Math.Max(1, signature.RasterCompositeColumns));
			material.SetShaderParameter(CrowdStateTextureParameter, arena.Texture);
			material.SetShaderParameter(CrowdStateTextureSizeParameter, new Vector2(arena.TextureSize.X, arena.TextureSize.Y));
		}
	}

	internal static void ClearSharedCrowdStateBinding(ShaderMaterial material)
	{
		if (GodotObject.IsInstanceValid(material))
		{
			material.SetShaderParameter(CrowdStateTextureParameter, default);
			material.SetShaderParameter(CrowdStateTextureSizeParameter, Vector2.One);
		}
	}

	public override void _ExitTree()
	{
		DetachRenderBindingsForOwnerExit();
		_crowdStateArena = null;
		_ownedCrowdStateArena.Dispose();
	}

	internal void DetachRenderBindingsForOwnerExit()
	{
		if (GodotObject.IsInstanceValid(_instance))
		{
			_instance.Material = null;
			_instance.Multimesh = null;
		}
		if (GodotObject.IsInstanceValid(_crowdInstance))
		{
			_crowdInstance.Material = null;
			_crowdInstance.Multimesh = null;
		}
		ClearSharedCrowdStateBinding(_crowdMaterial);
		_boundCrowdStateTexture = null;
		_boundCrowdStateTextureSize = Vector2I.Zero;
	}

	private static void WriteMultiMeshBufferInstance(float[] buffer, int offset, AdobeAnimateDrawItem item, int itemIndex)
	{
		float num = Math.Max(0.0001f, item.Size.X);
		float num2 = Math.Max(0.0001f, item.Size.Y);
		Vector2 vector = (item.UseShaderPose ? item.Transform.X : (item.Transform.X / num));
		Vector2 vector2 = (item.UseShaderPose ? item.Transform.Y : (item.Transform.Y / num2));
		Vector2 vector3 = (item.UseShaderPose ? (item.Transform.Origin + item.Transform.X * item.PoseOffset.X + item.Transform.Y * item.PoseOffset.Y) : item.Transform.Origin);
		Color color = item.Color;
		buffer[offset] = vector.X;
		buffer[offset + 1] = vector2.X;
		buffer[offset + 2] = 0f;
		buffer[offset + 3] = vector3.X;
		buffer[offset + 4] = vector.Y;
		buffer[offset + 5] = vector2.Y;
		buffer[offset + 6] = 0f;
		buffer[offset + 7] = vector3.Y;
		buffer[offset + 8] = color.R;
		buffer[offset + 9] = color.G;
		buffer[offset + 10] = color.B;
		buffer[offset + 11] = color.A;
		buffer[offset + 12] = itemIndex;
		buffer[offset + 13] = (item.UseShaderPose ? ((float)item.PoseLayer) : num);
		buffer[offset + 14] = (item.UseShaderPose ? item.PoseFrameT : num2);
		buffer[offset + 15] = (item.UseShaderPose ? 1f : 0f);
	}

	private static void WriteShaderPoseInstanceDataBufferInstance(float[] buffer, int offset, int poseTexel, int poseTextureWidth, float clipUp, float clipDown, bool clipEnabled, bool useVisualOverride = false, Rect2 visualUvRect = default(Rect2), int visualAtlasLayer = 0, float clipLeft = 0f, float clipRight = 0f)
	{
		int num = Math.Max(0, poseTexel);
		int num2 = Math.Max(1, poseTextureWidth);
		int num3 = num % num2;
		int num4 = num / num2;
		buffer[offset] = num3;
		buffer[offset + 1] = num4;
		buffer[offset + 2] = clipUp;
		buffer[offset + 3] = clipDown;
		buffer[offset + 4] = visualUvRect.Position.X;
		buffer[offset + 5] = visualUvRect.Position.Y;
		buffer[offset + 6] = visualUvRect.Size.X;
		buffer[offset + 7] = visualUvRect.Size.Y;
		buffer[offset + 8] = Math.Max(0, visualAtlasLayer);
		buffer[offset + 9] = (useVisualOverride ? 1f : 0f);
		buffer[offset + 10] = (clipEnabled ? 1f : 0f);
		buffer[offset + 11] = PackHorizontalClip(clipEnabled, clipLeft, clipRight);
	}

	private static float PackHorizontalClip(bool clipEnabled, float clipLeft, float clipRight)
	{
		if (!clipEnabled || clipRight <= clipLeft)
		{
			return 0f;
		}
		int num = Mathf.Clamp(Mathf.FloorToInt(clipLeft), 0, 4095);
		int num2 = Mathf.Clamp(Mathf.CeilToInt(clipRight), 0, 4095);
		return num * 4096 + num2;
	}

	private static void WriteShaderPoseMultiMeshBufferInstance(float[] buffer, int offset, Transform2D transform, Color color, Vector2 poseOffset, int poseLayer, float interpolationT, int itemIndex)
	{
		Vector2 x = transform.X;
		Vector2 y = transform.Y;
		Vector2 vector = transform.Origin + transform.X * poseOffset.X + transform.Y * poseOffset.Y;
		buffer[offset] = x.X;
		buffer[offset + 1] = y.X;
		buffer[offset + 2] = 0f;
		buffer[offset + 3] = vector.X;
		buffer[offset + 4] = x.Y;
		buffer[offset + 5] = y.Y;
		buffer[offset + 6] = 0f;
		buffer[offset + 7] = vector.Y;
		buffer[offset + 8] = color.R;
		buffer[offset + 9] = color.G;
		buffer[offset + 10] = color.B;
		buffer[offset + 11] = color.A;
		buffer[offset + 12] = itemIndex;
		buffer[offset + 13] = Math.Max(0, poseLayer);
		buffer[offset + 14] = Mathf.Clamp(interpolationT, 0f, 1f);
		buffer[offset + 15] = 1f;
	}

	private static void WriteCompositeCrowdCommand(float[] buffer, int offset, AdobeAnimateDrawItem item, Transform2D commandTransform)
	{
		float num = Math.Max(0.0001f, item.Size.X);
		float num2 = Math.Max(0.0001f, item.Size.Y);
		Vector2 vector = (item.UseShaderPose ? commandTransform.X : (commandTransform.X / num));
		Vector2 vector2 = (item.UseShaderPose ? commandTransform.Y : (commandTransform.Y / num2));
		Vector2 vector3 = (item.UseShaderPose ? (commandTransform.Origin + commandTransform.X * item.PoseOffset.X + commandTransform.Y * item.PoseOffset.Y) : commandTransform.Origin);
		float num3;
		if (item.UseShaderPose)
		{
			num3 = (item.UseVisualOverride ? 2f : 1f);
		}
		else
		{
			num3 = 0f;
		}
		buffer[offset] = Math.Max(0, item.PoseTexel);
		buffer[offset + 1] = Math.Max(0, item.PoseLayer);
		buffer[offset + 2] = Mathf.Clamp(item.PoseFrameT, 0f, 1f);
		buffer[offset + 3] = num3;
		buffer[offset + 4] = vector.X;
		buffer[offset + 5] = vector.Y;
		buffer[offset + 6] = vector2.X;
		buffer[offset + 7] = vector2.Y;
		buffer[offset + 8] = vector3.X;
		buffer[offset + 9] = vector3.Y;
		buffer[offset + 10] = num;
		buffer[offset + 11] = num2;
		buffer[offset + 12] = item.UvRect.Position.X;
		buffer[offset + 13] = item.UvRect.Position.Y;
		buffer[offset + 14] = item.UvRect.Size.X;
		buffer[offset + 15] = item.UvRect.Size.Y;
		buffer[offset + 16] = Math.Max(0, item.AtlasLayer);
		buffer[offset + 17] = item.ClipUp;
		buffer[offset + 18] = item.ClipDown;
		buffer[offset + 19] = (item.ClipEnabled ? 1f : 0f);
		buffer[offset + 20] = item.Color.R;
		buffer[offset + 21] = item.Color.G;
		buffer[offset + 22] = item.Color.B;
		buffer[offset + 23] = item.Color.A;
	}

	private static void WriteCachedCompositeCrowdOwnerState(float[] buffer, int offset, Transform2D relativeTransform, Vector2 origin, AdobeAnimateCompositePoseRefreshState ownerState)
	{
		buffer[offset] = relativeTransform.X.X;
		buffer[offset + 1] = relativeTransform.X.Y;
		buffer[offset + 2] = relativeTransform.Y.X;
		buffer[offset + 3] = relativeTransform.Y.Y;
		buffer[offset + 4] = origin.X;
		buffer[offset + 5] = origin.Y;
		buffer[offset + 6] = Math.Max(0, ownerState.PoseLayer);
		buffer[offset + 7] = Mathf.Clamp(ownerState.InterpolationT, 0f, 1f);
		buffer[offset + 8] = ownerState.VerticalClip.UpY;
		buffer[offset + 9] = ownerState.VerticalClip.DownY;
		buffer[offset + 10] = (ownerState.VerticalClip.Enabled ? 1f : 0f);
		buffer[offset + 11] = 0f;
		buffer[offset + 12] = ownerState.Modulate.R;
		buffer[offset + 13] = ownerState.Modulate.G;
		buffer[offset + 14] = ownerState.Modulate.B;
		buffer[offset + 15] = ownerState.Modulate.A;
	}

	private static void WriteCachedCompositeCrowdCommand(float[] buffer, int offset, int poseTexel, int ownerIndex, int visualOverrideIndexPlusOne)
	{
		buffer[offset] = Math.Max(0, poseTexel);
		buffer[offset + 1] = ownerIndex;
		buffer[offset + 2] = visualOverrideIndexPlusOne;
		buffer[offset + 3] = 0f;
	}

	private static void WriteCachedCompositeCrowdVisualOverride(float[] buffer, int offset, AdobeAnimateDrawItem item)
	{
		buffer[offset] = Math.Max(0.0001f, item.Size.X);
		buffer[offset + 1] = Math.Max(0.0001f, item.Size.Y);
		buffer[offset + 2] = item.UvRect.Position.X;
		buffer[offset + 3] = item.UvRect.Position.Y;
		buffer[offset + 4] = item.UvRect.Size.X;
		buffer[offset + 5] = item.UvRect.Size.Y;
		buffer[offset + 6] = Math.Max(0, item.AtlasLayer);
		buffer[offset + 7] = 0f;
	}

	private static void WriteRootMotionState(float[] buffer, int offset, in AdobeAnimateRootMotionState state)
	{
		Transform2D transform2D = (state.Enabled ? state.PreviousRelativeTransform : Transform2D.Identity);
		buffer[offset] = transform2D.X.X;
		buffer[offset + 1] = transform2D.X.Y;
		buffer[offset + 2] = transform2D.Y.X;
		buffer[offset + 3] = transform2D.Y.Y;
		buffer[offset + 4] = transform2D.Origin.X;
		buffer[offset + 5] = transform2D.Origin.Y;
		buffer[offset + 6] = (state.Enabled ? 1f : 0f);
		buffer[offset + 7] = 0f;
	}

	private static void LegacyGpuRenderGraphStateUnused(float[] buffer, int stateBaseTexel, AdobeAnimateGpuRenderGraphAllocation allocation, AdobeAnimateGpuRenderGraphDefinition graph, IReadOnlyList<AdobeAnimateGpuGraphOwnerState> ownerStates, int ownerStart, int ownerCount, IReadOnlyList<AdobeAnimateGpuDynamicOverrideAllocation> overrideAllocations, int overrideStart, IReadOnlyList<AdobeAnimateGpuManagedVisualState> managedVisualStates, int managedVisualStart, int managedVisualCount, bool useAbsoluteTransform, AdobeAnimateRootMotionState rootMotion = default(AdobeAnimateRootMotionState))
	{
		int num = stateBaseTexel * 4;
		buffer[num] = allocation.BaseTexel;
		buffer[num + 1] = allocation.Page;
		buffer[num + 2] = allocation.RenderSlotCount;
		buffer[num + 3] = 7f;
		buffer[num + 4] = allocation.OwnerCount;
		int num2 = stateBaseTexel + 4 + ownerCount * 10;
		buffer[num + 5] = num2;
		int num3 = num2 + ownerCount;
		int num4 = 0;
		for (int i = 0; i < ownerCount; i++)
		{
			num4 += GpuGraphLayerMaskTexelCount(ownerStates[ownerStart + i]);
		}
		int num5 = num3 + num4;
		buffer[num + 6] = ((managedVisualCount > 0) ? ((float)num5) : (-1f));
		buffer[num + 7] = managedVisualCount;
		WriteRootMotionState(buffer, num + 8, useAbsoluteTransform ? default(AdobeAnimateRootMotionState) : rootMotion);
		for (int j = 0; j < ownerCount; j++)
		{
			AdobeAnimateGpuGraphOwnerState adobeAnimateGpuGraphOwnerState = ownerStates[ownerStart + j];
			AdobeAnimateGpuDynamicOverrideAllocation adobeAnimateGpuDynamicOverrideAllocation = overrideAllocations[overrideStart + j];
			int num6 = GpuGraphLayerMaskTexelCount(adobeAnimateGpuGraphOwnerState);
			Transform2D transform2D = ((useAbsoluteTransform && j == 0) ? adobeAnimateGpuGraphOwnerState.GlobalTransform : Transform2D.Identity);
			int num7 = num + (4 + j * 10) * 4;
			buffer[num7] = transform2D.X.X;
			buffer[num7 + 1] = transform2D.X.Y;
			buffer[num7 + 2] = transform2D.Y.X;
			buffer[num7 + 3] = transform2D.Y.Y;
			buffer[num7 + 4] = transform2D.Origin.X;
			buffer[num7 + 5] = transform2D.Origin.Y;
			buffer[num7 + 6] = adobeAnimateGpuGraphOwnerState.FrameIndex;
			buffer[num7 + 7] = adobeAnimateGpuGraphOwnerState.InterpolationT;
			buffer[num7 + 8] = adobeAnimateGpuGraphOwnerState.Offset.X;
			buffer[num7 + 9] = adobeAnimateGpuGraphOwnerState.Offset.Y;
			buffer[num7 + 10] = adobeAnimateGpuGraphOwnerState.VerticalClip.UpY;
			buffer[num7 + 11] = adobeAnimateGpuGraphOwnerState.VerticalClip.DownY;
			buffer[num7 + 12] = (adobeAnimateGpuGraphOwnerState.VerticalClip.Enabled ? 1f : 0f);
			buffer[num7 + 13] = (adobeAnimateGpuGraphOwnerState.Visible ? 1f : 0f);
			buffer[num7 + 14] = ((num6 > 0) ? ((float)num3) : (-1f));
			buffer[num7 + 15] = ((num6 > 0) ? ((float)adobeAnimateGpuGraphOwnerState.LayerCount) : 0f);
			buffer[num7 + 16] = adobeAnimateGpuGraphOwnerState.Modulate.R;
			buffer[num7 + 17] = adobeAnimateGpuGraphOwnerState.Modulate.G;
			buffer[num7 + 18] = adobeAnimateGpuGraphOwnerState.Modulate.B;
			buffer[num7 + 19] = adobeAnimateGpuGraphOwnerState.Modulate.A;
			buffer[num7 + 20] = adobeAnimateGpuGraphOwnerState.GpuClock.StartTime;
			buffer[num7 + 21] = adobeAnimateGpuGraphOwnerState.GpuClock.StartFrame;
			buffer[num7 + 22] = adobeAnimateGpuGraphOwnerState.GpuClock.FramesPerSecond;
			buffer[num7 + 23] = (adobeAnimateGpuGraphOwnerState.GpuClock.Enabled ? 1f : 0f);
			buffer[num7 + 24] = adobeAnimateGpuGraphOwnerState.GpuClock.ClipStart;
			buffer[num7 + 25] = adobeAnimateGpuGraphOwnerState.GpuClock.ClipEndExclusive;
			buffer[num7 + 26] = (adobeAnimateGpuGraphOwnerState.GpuClock.Loop ? 1f : 0f);
			buffer[num7 + 27] = 0f;
			float fromFrameFloat = adobeAnimateGpuGraphOwnerState.ClipBlend.FromFrameFloat;
			buffer[num7 + 29] = Mathf.Clamp(fromFrameFloat - (buffer[num7 + 28] = Mathf.Floor(fromFrameFloat)), 0f, 1f);
			buffer[num7 + 30] = adobeAnimateGpuGraphOwnerState.ClipBlend.Weight;
			buffer[num7 + 31] = (adobeAnimateGpuGraphOwnerState.ClipBlend.Enabled ? 1f : 0f);
			int num8 = (num2 + j) * 4;
			buffer[num8] = ((num6 > 0) ? ((float)num3) : (-1f));
			buffer[num8 + 1] = ((num6 > 0) ? ((float)adobeAnimateGpuGraphOwnerState.LayerCount) : 0f);
			buffer[num8 + 2] = ((adobeAnimateGpuDynamicOverrideAllocation.Signature != 0L) ? ((float)adobeAnimateGpuDynamicOverrideAllocation.BaseTexel) : (-1f));
			buffer[num8 + 3] = ((adobeAnimateGpuDynamicOverrideAllocation.Signature != 0L) ? ((float)adobeAnimateGpuDynamicOverrideAllocation.MediaCount) : 0f);
			if (num6 > 0)
			{
				WriteGpuGraphLayerMask(buffer, num3, adobeAnimateGpuGraphOwnerState, num6);
				num3 += num6;
			}
		}
		for (int k = 0; k < managedVisualCount; k++)
		{
			WriteGpuManagedVisualState(buffer, num5 + k * 7, managedVisualStates[managedVisualStart + k]);
		}
	}

	private static void WriteGpuManagedVisualState(float[] buffer, int baseTexel, in AdobeAnimateGpuManagedVisualState state)
	{
		int num = baseTexel * 4;
		buffer[num] = Math.Max(0.0001f, state.SourceSize.X);
		buffer[num + 1] = Math.Max(0.0001f, state.SourceSize.Y);
		buffer[num + 2] = Math.Max(0, state.AtlasLayer);
		buffer[num + 3] = (state.Visible ? 1f : 0f);
		buffer[num + 4] = state.UvRect.Position.X;
		buffer[num + 5] = state.UvRect.Position.Y;
		buffer[num + 6] = state.UvRect.Size.X;
		buffer[num + 7] = state.UvRect.Size.Y;
		buffer[num + 8] = state.DrawOrigin.X;
		buffer[num + 9] = state.DrawOrigin.Y;
		buffer[num + 10] = (state.FlipH ? (-1f) : 1f);
		buffer[num + 11] = (state.FlipV ? (-1f) : 1f);
		buffer[num + 12] = state.LocalTransform.X.X;
		buffer[num + 13] = state.LocalTransform.X.Y;
		buffer[num + 14] = state.LocalTransform.Y.X;
		buffer[num + 15] = state.LocalTransform.Y.Y;
		buffer[num + 16] = state.LocalTransform.Origin.X;
		buffer[num + 17] = state.LocalTransform.Origin.Y;
		buffer[num + 18] = state.SlotOffset.X;
		buffer[num + 19] = state.SlotOffset.Y;
		buffer[num + 20] = state.Modulate.R;
		buffer[num + 21] = state.Modulate.G;
		buffer[num + 22] = state.Modulate.B;
		buffer[num + 23] = state.Modulate.A;
		buffer[num + 24] = (state.UseRotate ? 1f : 0f);
		buffer[num + 25] = (state.UseScale ? 1f : 0f);
		buffer[num + 26] = (state.UseSkew ? 1f : 0f);
		buffer[num + 27] = (state.Blink ? 1f : 0f);
	}

	private static int GpuGraphLayerMaskTexelCount(AdobeAnimateGpuGraphOwnerState ownerState)
	{
		return GetCrowdLayerMaskTexelCount(ownerState.AllLayersVisible, ownerState.LayerCount);
	}

	private static void WriteGpuGraphLayerMask(float[] buffer, int ownerMaskBaseTexel, AdobeAnimateGpuGraphOwnerState ownerState, int maskTexelCount)
	{
		for (int i = 0; i < maskTexelCount; i++)
		{
			Color color = BuildCrowdLayerMaskTexel(ownerState.CanUseLayerMask, ownerState.LayerMask, ownerState.LayerVisible, ownerState.LayerCount, i);
			int num = (ownerMaskBaseTexel + i) * 4;
			buffer[num] = color.R;
			buffer[num + 1] = color.G;
			buffer[num + 2] = color.B;
			buffer[num + 3] = color.A;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(61)
		{
			new MethodInfo(MethodName.TryAppendPreparedGpuGraphInstance, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maxQuadCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "transform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "stateBaseTexel", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCachedCompositeOwnerCapacity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "required", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMediaReplaceActive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "mediaReplaceUse", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "mediaReplaceRect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCrowdLayerVisibleCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "canUseLayerMask", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "layerVisible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCrowdLayerMaskTexelCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "allLayersVisible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "layerVisibleCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCrowdLayerMaskTexel, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "canUseLayerMask", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "layerMask", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "layerVisible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "layerVisibleCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "maskTexelIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCrowdLayerMaskComponent, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "canUseLayerMask", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "layerMask", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "layerVisible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "layerVisibleCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "layerBase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsLayerVisible, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "layerMask", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "atlasArray", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "atlasSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "atlasArray", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "atlasSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "preserveVisibleInstances", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetVisibleInstanceCountForTest, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetBufferedInstanceCountForTest, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCompactCrowdRootCountForTest, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCompositeCrowdRootCountForTest, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGpuGraphRootCountForTest, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGpuGraphSlotCountForTest, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGpuGraphStateWrittenTexelsForTest, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCrowdStateWrittenTexelsForTest, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginBuffered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "poseTextureArray", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "poseTextureSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginGpuGraphBuffered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "poseTextureArray", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "poseTextureSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "gpuRenderGraphTextureArray", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2DArray"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gpuRenderGraphTextureSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureBufferedGpuGraphTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "gpuRenderGraphTextureArray", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2DArray"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gpuRenderGraphTextureSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureBufferedGpuDynamicOverrideTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "gpuDynamicOverrideTexture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ImageTexture"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "gpuDynamicOverrideTextureSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyStandaloneCrowdClock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "seconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "physicsInterpolationFraction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetCrowdFrameState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureInstanceDataTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "requiredTexels", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigurePoseTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "poseTextureArray", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "poseTextureSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureGpuRenderGraphTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "textureArray", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2DArray"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "textureSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureGpuDynamicOverrideTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ImageTexture"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "textureSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPoseTextureToMaterial, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "material", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ShaderMaterial"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyGpuRenderGraphTextureToMaterial, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "material", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ShaderMaterial"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyGpuDynamicOverrideTextureToMaterial, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "material", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ShaderMaterial"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyInstanceDataLayoutToMaterial, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "material", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ShaderMaterial"), exported: false)
			}, null),
			new MethodInfo(MethodName.NextPowerOfTwo, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateUnitQuadMesh, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ArrayMesh"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateCrowdMesh, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ArrayMesh"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maxSlices", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentCrowdGroupStateCursor, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EndBuffered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureRenderObjects, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureCrowdRenderObjects, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maxSlices", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCapacity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCrowdCapacity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitCrowdGroup, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RollbackCrowdGroup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearCrowdGroupState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryAppendCrowdInstance, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maxQuadCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "transform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "poseOffset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "stateBaseTexel", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RollbackFailedCrowdInstance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "standaloneStateMark", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UploadBufferedData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UploadCrowdBufferedData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UploadCrowdStateOnce, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSharedCrowdMaterial, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ShaderMaterial"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateSharedCrowdMesh, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ArrayMesh"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "maxQuadCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSharedCrowdAnimationClock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "material", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ShaderMaterial"), exported: false),
				new PropertyInfo(Variant.Type.Float, "seconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSharedCrowdPhysicsInterpolationFraction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "material", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ShaderMaterial"), exported: false),
				new PropertyInfo(Variant.Type.Float, "fraction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearSharedCrowdStateBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "material", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ShaderMaterial"), exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DetachRenderBindingsForOwnerExit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.WriteShaderPoseInstanceDataBufferInstance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedFloat32Array, "buffer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "poseTexel", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "poseTextureWidth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "clipUp", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "clipDown", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "clipEnabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "useVisualOverride", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "visualUvRect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "visualAtlasLayer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "clipLeft", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "clipRight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PackHorizontalClip, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "clipEnabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "clipLeft", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "clipRight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WriteShaderPoseMultiMeshBufferInstance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedFloat32Array, "buffer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "transform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "poseOffset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "poseLayer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "interpolationT", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "itemIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WriteCachedCompositeCrowdCommand, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedFloat32Array, "buffer", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "poseTexel", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "ownerIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "visualOverrideIndexPlusOne", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.TryAppendPreparedGpuGraphInstance && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(TryAppendPreparedGpuGraphInstance(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.EnsureCachedCompositeOwnerCapacity && args.Count == 1)
		{
			EnsureCachedCompositeOwnerCapacity(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsMediaReplaceActive && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMediaReplaceActive(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertToArray<bool>(in args[1]), VariantUtils.ConvertToArray<Rect2>(in args[2])));
			return true;
		}
		if (method == MethodName.GetCrowdLayerVisibleCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetCrowdLayerVisibleCount(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertToArray<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCrowdLayerMaskTexelCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetCrowdLayerMaskTexelCount(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildCrowdLayerMaskTexel && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<Color>(BuildCrowdLayerMaskTexel(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]), VariantUtils.ConvertToArray<bool>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4])));
			return true;
		}
		if (method == MethodName.BuildCrowdLayerMaskComponent && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<float>(BuildCrowdLayerMaskComponent(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]), VariantUtils.ConvertToArray<bool>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4])));
			return true;
		}
		if (method == MethodName.IsLayerVisible && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLayerVisible(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.BeginFrame && args.Count == 2)
		{
			BeginFrame(VariantUtils.ConvertTo<TextureLayered>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginFrame && args.Count == 3)
		{
			BeginFrame(VariantUtils.ConvertTo<TextureLayered>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetVisibleInstanceCountForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetVisibleInstanceCountForTest());
			return true;
		}
		if (method == MethodName.GetBufferedInstanceCountForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetBufferedInstanceCountForTest());
			return true;
		}
		if (method == MethodName.GetCompactCrowdRootCountForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCompactCrowdRootCountForTest());
			return true;
		}
		if (method == MethodName.GetCompositeCrowdRootCountForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCompositeCrowdRootCountForTest());
			return true;
		}
		if (method == MethodName.GetGpuGraphRootCountForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetGpuGraphRootCountForTest());
			return true;
		}
		if (method == MethodName.GetGpuGraphSlotCountForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetGpuGraphSlotCountForTest());
			return true;
		}
		if (method == MethodName.GetGpuGraphStateWrittenTexelsForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetGpuGraphStateWrittenTexelsForTest());
			return true;
		}
		if (method == MethodName.GetCrowdStateWrittenTexelsForTest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCrowdStateWrittenTexelsForTest());
			return true;
		}
		if (method == MethodName.BeginBuffered && args.Count == 2)
		{
			BeginBuffered(VariantUtils.ConvertTo<TextureLayered>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginGpuGraphBuffered && args.Count == 4)
		{
			BeginGpuGraphBuffered(VariantUtils.ConvertTo<TextureLayered>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]), VariantUtils.ConvertTo<Texture2DArray>(in args[2]), VariantUtils.ConvertTo<Vector2I>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureBufferedGpuGraphTexture && args.Count == 2)
		{
			ConfigureBufferedGpuGraphTexture(VariantUtils.ConvertTo<Texture2DArray>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureBufferedGpuDynamicOverrideTexture && args.Count == 2)
		{
			ConfigureBufferedGpuDynamicOverrideTexture(VariantUtils.ConvertTo<ImageTexture>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyStandaloneCrowdClock && args.Count == 2)
		{
			ApplyStandaloneCrowdClock(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetCrowdFrameState && args.Count == 0)
		{
			ResetCrowdFrameState();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureInstanceDataTexture && args.Count == 1)
		{
			EnsureInstanceDataTexture(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigurePoseTexture && args.Count == 2)
		{
			ConfigurePoseTexture(VariantUtils.ConvertTo<TextureLayered>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureGpuRenderGraphTexture && args.Count == 2)
		{
			ConfigureGpuRenderGraphTexture(VariantUtils.ConvertTo<Texture2DArray>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureGpuDynamicOverrideTexture && args.Count == 2)
		{
			ConfigureGpuDynamicOverrideTexture(VariantUtils.ConvertTo<ImageTexture>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPoseTextureToMaterial && args.Count == 1)
		{
			ApplyPoseTextureToMaterial(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyGpuRenderGraphTextureToMaterial && args.Count == 1)
		{
			ApplyGpuRenderGraphTextureToMaterial(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyGpuDynamicOverrideTextureToMaterial && args.Count == 1)
		{
			ApplyGpuDynamicOverrideTextureToMaterial(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyInstanceDataLayoutToMaterial && args.Count == 1)
		{
			ApplyInstanceDataLayoutToMaterial(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NextPowerOfTwo && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(NextPowerOfTwo(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateUnitQuadMesh && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ArrayMesh>(CreateUnitQuadMesh());
			return true;
		}
		if (method == MethodName.CreateCrowdMesh && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ArrayMesh>(CreateCrowdMesh(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurrentCrowdGroupStateCursor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCurrentCrowdGroupStateCursor());
			return true;
		}
		if (method == MethodName.EndBuffered && args.Count == 0)
		{
			EndBuffered();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureRenderObjects && args.Count == 0)
		{
			EnsureRenderObjects();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCrowdRenderObjects && args.Count == 1)
		{
			EnsureCrowdRenderObjects(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCapacity && args.Count == 1)
		{
			EnsureCapacity(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCrowdCapacity && args.Count == 1)
		{
			EnsureCrowdCapacity(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitCrowdGroup && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CommitCrowdGroup());
			return true;
		}
		if (method == MethodName.RollbackCrowdGroup && args.Count == 0)
		{
			RollbackCrowdGroup();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearCrowdGroupState && args.Count == 0)
		{
			ClearCrowdGroupState();
			ret = default;
			return true;
		}
		if (method == MethodName.TryAppendCrowdInstance && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<bool>(TryAppendCrowdInstance(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<float>(in args[5])));
			return true;
		}
		if (method == MethodName.RollbackFailedCrowdInstance && args.Count == 1)
		{
			RollbackFailedCrowdInstance(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UploadBufferedData && args.Count == 1)
		{
			UploadBufferedData(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UploadCrowdBufferedData && args.Count == 1)
		{
			UploadCrowdBufferedData(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UploadCrowdStateOnce && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(UploadCrowdStateOnce());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSharedCrowdMaterial && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ShaderMaterial>(CreateSharedCrowdMaterial());
			return true;
		}
		if (method == MethodName.CreateSharedCrowdMesh && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ArrayMesh>(CreateSharedCrowdMesh(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSharedCrowdAnimationClock && args.Count == 2)
		{
			SetSharedCrowdAnimationClock(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSharedCrowdPhysicsInterpolationFraction && args.Count == 2)
		{
			SetSharedCrowdPhysicsInterpolationFraction(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearSharedCrowdStateBinding && args.Count == 1)
		{
			ClearSharedCrowdStateBinding(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.DetachRenderBindingsForOwnerExit && args.Count == 0)
		{
			DetachRenderBindingsForOwnerExit();
			ret = default;
			return true;
		}
		if (method == MethodName.WriteShaderPoseInstanceDataBufferInstance && args.Count == 12)
		{
			WriteShaderPoseInstanceDataBufferInstance(VariantUtils.ConvertTo<float[]>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]), VariantUtils.ConvertTo<float>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]), VariantUtils.ConvertTo<Rect2>(in args[8]), VariantUtils.ConvertTo<int>(in args[9]), VariantUtils.ConvertTo<float>(in args[10]), VariantUtils.ConvertTo<float>(in args[11]));
			ret = default;
			return true;
		}
		if (method == MethodName.PackHorizontalClip && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<float>(PackHorizontalClip(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.WriteShaderPoseMultiMeshBufferInstance && args.Count == 8)
		{
			WriteShaderPoseMultiMeshBufferInstance(VariantUtils.ConvertTo<float[]>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Transform2D>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<float>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]));
			ret = default;
			return true;
		}
		if (method == MethodName.WriteCachedCompositeCrowdCommand && args.Count == 5)
		{
			WriteCachedCompositeCrowdCommand(VariantUtils.ConvertTo<float[]>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsMediaReplaceActive && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMediaReplaceActive(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertToArray<bool>(in args[1]), VariantUtils.ConvertToArray<Rect2>(in args[2])));
			return true;
		}
		if (method == MethodName.GetCrowdLayerVisibleCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetCrowdLayerVisibleCount(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertToArray<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCrowdLayerMaskTexelCount && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetCrowdLayerMaskTexelCount(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildCrowdLayerMaskTexel && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<Color>(BuildCrowdLayerMaskTexel(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]), VariantUtils.ConvertToArray<bool>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4])));
			return true;
		}
		if (method == MethodName.BuildCrowdLayerMaskComponent && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<float>(BuildCrowdLayerMaskComponent(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]), VariantUtils.ConvertToArray<bool>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4])));
			return true;
		}
		if (method == MethodName.IsLayerVisible && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLayerVisible(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyInstanceDataLayoutToMaterial && args.Count == 1)
		{
			ApplyInstanceDataLayoutToMaterial(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NextPowerOfTwo && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(NextPowerOfTwo(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateUnitQuadMesh && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ArrayMesh>(CreateUnitQuadMesh());
			return true;
		}
		if (method == MethodName.CreateCrowdMesh && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ArrayMesh>(CreateCrowdMesh(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateSharedCrowdMaterial && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ShaderMaterial>(CreateSharedCrowdMaterial());
			return true;
		}
		if (method == MethodName.CreateSharedCrowdMesh && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ArrayMesh>(CreateSharedCrowdMesh(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSharedCrowdAnimationClock && args.Count == 2)
		{
			SetSharedCrowdAnimationClock(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSharedCrowdPhysicsInterpolationFraction && args.Count == 2)
		{
			SetSharedCrowdPhysicsInterpolationFraction(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearSharedCrowdStateBinding && args.Count == 1)
		{
			ClearSharedCrowdStateBinding(VariantUtils.ConvertTo<ShaderMaterial>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WriteShaderPoseInstanceDataBufferInstance && args.Count == 12)
		{
			WriteShaderPoseInstanceDataBufferInstance(VariantUtils.ConvertTo<float[]>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]), VariantUtils.ConvertTo<float>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]), VariantUtils.ConvertTo<Rect2>(in args[8]), VariantUtils.ConvertTo<int>(in args[9]), VariantUtils.ConvertTo<float>(in args[10]), VariantUtils.ConvertTo<float>(in args[11]));
			ret = default;
			return true;
		}
		if (method == MethodName.PackHorizontalClip && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<float>(PackHorizontalClip(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.WriteShaderPoseMultiMeshBufferInstance && args.Count == 8)
		{
			WriteShaderPoseMultiMeshBufferInstance(VariantUtils.ConvertTo<float[]>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Transform2D>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<float>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]));
			ret = default;
			return true;
		}
		if (method == MethodName.WriteCachedCompositeCrowdCommand && args.Count == 5)
		{
			WriteCachedCompositeCrowdCommand(VariantUtils.ConvertTo<float[]>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.TryAppendPreparedGpuGraphInstance)
		{
			return true;
		}
		if (method == MethodName.EnsureCachedCompositeOwnerCapacity)
		{
			return true;
		}
		if (method == MethodName.IsMediaReplaceActive)
		{
			return true;
		}
		if (method == MethodName.GetCrowdLayerVisibleCount)
		{
			return true;
		}
		if (method == MethodName.GetCrowdLayerMaskTexelCount)
		{
			return true;
		}
		if (method == MethodName.BuildCrowdLayerMaskTexel)
		{
			return true;
		}
		if (method == MethodName.BuildCrowdLayerMaskComponent)
		{
			return true;
		}
		if (method == MethodName.IsLayerVisible)
		{
			return true;
		}
		if (method == MethodName.BeginFrame)
		{
			return true;
		}
		if (method == MethodName.GetVisibleInstanceCountForTest)
		{
			return true;
		}
		if (method == MethodName.GetBufferedInstanceCountForTest)
		{
			return true;
		}
		if (method == MethodName.GetCompactCrowdRootCountForTest)
		{
			return true;
		}
		if (method == MethodName.GetCompositeCrowdRootCountForTest)
		{
			return true;
		}
		if (method == MethodName.GetGpuGraphRootCountForTest)
		{
			return true;
		}
		if (method == MethodName.GetGpuGraphSlotCountForTest)
		{
			return true;
		}
		if (method == MethodName.GetGpuGraphStateWrittenTexelsForTest)
		{
			return true;
		}
		if (method == MethodName.GetCrowdStateWrittenTexelsForTest)
		{
			return true;
		}
		if (method == MethodName.BeginBuffered)
		{
			return true;
		}
		if (method == MethodName.BeginGpuGraphBuffered)
		{
			return true;
		}
		if (method == MethodName.ConfigureBufferedGpuGraphTexture)
		{
			return true;
		}
		if (method == MethodName.ConfigureBufferedGpuDynamicOverrideTexture)
		{
			return true;
		}
		if (method == MethodName.ApplyStandaloneCrowdClock)
		{
			return true;
		}
		if (method == MethodName.ResetCrowdFrameState)
		{
			return true;
		}
		if (method == MethodName.EnsureInstanceDataTexture)
		{
			return true;
		}
		if (method == MethodName.ConfigurePoseTexture)
		{
			return true;
		}
		if (method == MethodName.ConfigureGpuRenderGraphTexture)
		{
			return true;
		}
		if (method == MethodName.ConfigureGpuDynamicOverrideTexture)
		{
			return true;
		}
		if (method == MethodName.ApplyPoseTextureToMaterial)
		{
			return true;
		}
		if (method == MethodName.ApplyGpuRenderGraphTextureToMaterial)
		{
			return true;
		}
		if (method == MethodName.ApplyGpuDynamicOverrideTextureToMaterial)
		{
			return true;
		}
		if (method == MethodName.ApplyInstanceDataLayoutToMaterial)
		{
			return true;
		}
		if (method == MethodName.NextPowerOfTwo)
		{
			return true;
		}
		if (method == MethodName.CreateUnitQuadMesh)
		{
			return true;
		}
		if (method == MethodName.CreateCrowdMesh)
		{
			return true;
		}
		if (method == MethodName.GetCurrentCrowdGroupStateCursor)
		{
			return true;
		}
		if (method == MethodName.EndBuffered)
		{
			return true;
		}
		if (method == MethodName.EnsureRenderObjects)
		{
			return true;
		}
		if (method == MethodName.EnsureCrowdRenderObjects)
		{
			return true;
		}
		if (method == MethodName.EnsureCapacity)
		{
			return true;
		}
		if (method == MethodName.EnsureCrowdCapacity)
		{
			return true;
		}
		if (method == MethodName.CommitCrowdGroup)
		{
			return true;
		}
		if (method == MethodName.RollbackCrowdGroup)
		{
			return true;
		}
		if (method == MethodName.ClearCrowdGroupState)
		{
			return true;
		}
		if (method == MethodName.TryAppendCrowdInstance)
		{
			return true;
		}
		if (method == MethodName.RollbackFailedCrowdInstance)
		{
			return true;
		}
		if (method == MethodName.UploadBufferedData)
		{
			return true;
		}
		if (method == MethodName.UploadCrowdBufferedData)
		{
			return true;
		}
		if (method == MethodName.UploadCrowdStateOnce)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.CreateSharedCrowdMaterial)
		{
			return true;
		}
		if (method == MethodName.CreateSharedCrowdMesh)
		{
			return true;
		}
		if (method == MethodName.SetSharedCrowdAnimationClock)
		{
			return true;
		}
		if (method == MethodName.SetSharedCrowdPhysicsInterpolationFraction)
		{
			return true;
		}
		if (method == MethodName.ClearSharedCrowdStateBinding)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.DetachRenderBindingsForOwnerExit)
		{
			return true;
		}
		if (method == MethodName.WriteShaderPoseInstanceDataBufferInstance)
		{
			return true;
		}
		if (method == MethodName.PackHorizontalClip)
		{
			return true;
		}
		if (method == MethodName.WriteShaderPoseMultiMeshBufferInstance)
		{
			return true;
		}
		if (method == MethodName.WriteCachedCompositeCrowdCommand)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.EncoderOnly)
		{
			EncoderOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._instance)
		{
			_instance = VariantUtils.ConvertTo<MultiMeshInstance2D>(in value);
			return true;
		}
		if (name == PropertyName._multiMesh)
		{
			_multiMesh = VariantUtils.ConvertTo<MultiMesh>(in value);
			return true;
		}
		if (name == PropertyName._material)
		{
			_material = VariantUtils.ConvertTo<ShaderMaterial>(in value);
			return true;
		}
		if (name == PropertyName._quadMesh)
		{
			_quadMesh = VariantUtils.ConvertTo<ArrayMesh>(in value);
			return true;
		}
		if (name == PropertyName._crowdInstance)
		{
			_crowdInstance = VariantUtils.ConvertTo<MultiMeshInstance2D>(in value);
			return true;
		}
		if (name == PropertyName._crowdMultiMesh)
		{
			_crowdMultiMesh = VariantUtils.ConvertTo<MultiMesh>(in value);
			return true;
		}
		if (name == PropertyName._crowdMaterial)
		{
			_crowdMaterial = VariantUtils.ConvertTo<ShaderMaterial>(in value);
			return true;
		}
		if (name == PropertyName._crowdMesh)
		{
			_crowdMesh = VariantUtils.ConvertTo<ArrayMesh>(in value);
			return true;
		}
		if (name == PropertyName._instanceDataImage)
		{
			_instanceDataImage = VariantUtils.ConvertTo<Image>(in value);
			return true;
		}
		if (name == PropertyName._instanceDataTexture)
		{
			_instanceDataTexture = VariantUtils.ConvertTo<ImageTexture>(in value);
			return true;
		}
		if (name == PropertyName._instanceDataTextureSize)
		{
			_instanceDataTextureSize = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._instanceDataTexelCapacity)
		{
			_instanceDataTexelCapacity = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._instanceDataBuffer)
		{
			_instanceDataBuffer = VariantUtils.ConvertTo<float[]>(in value);
			return true;
		}
		if (name == PropertyName._multiMeshBuffer)
		{
			_multiMeshBuffer = VariantUtils.ConvertTo<float[]>(in value);
			return true;
		}
		if (name == PropertyName._crowdMultiMeshBuffer)
		{
			_crowdMultiMeshBuffer = VariantUtils.ConvertTo<float[]>(in value);
			return true;
		}
		if (name == PropertyName._boundCrowdStateTexture)
		{
			_boundCrowdStateTexture = VariantUtils.ConvertTo<TextureLayered>(in value);
			return true;
		}
		if (name == PropertyName._boundCrowdStateTextureSize)
		{
			_boundCrowdStateTextureSize = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._crowdGroupArenaMark)
		{
			_crowdGroupArenaMark = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._crowdGroupBucketMark)
		{
			_crowdGroupBucketMark = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._crowdGroupExpectedBucketEnd)
		{
			_crowdGroupExpectedBucketEnd = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._crowdGroupCursorTexel)
		{
			_crowdGroupCursorTexel = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._crowdGroupEndTexel)
		{
			_crowdGroupEndTexel = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._standaloneCrowdFrameVersion)
		{
			_standaloneCrowdFrameVersion = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._cachedCompositeOwnerOrigins)
		{
			_cachedCompositeOwnerOrigins = VariantUtils.ConvertTo<Vector2[]>(in value);
			return true;
		}
		if (name == PropertyName._capacity)
		{
			_capacity = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._crowdCapacity)
		{
			_crowdCapacity = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._bufferedCount)
		{
			_bufferedCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._crowdBufferedCount)
		{
			_crowdBufferedCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._crowdMeshMaxSlices)
		{
			_crowdMeshMaxSlices = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._compactCrowdRootCount)
		{
			_compactCrowdRootCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._compositeCrowdRootCount)
		{
			_compositeCrowdRootCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphRootCount)
		{
			_gpuGraphRootCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphSlotCount)
		{
			_gpuGraphSlotCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphStateWrittenTexels)
		{
			_gpuGraphStateWrittenTexels = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._crowdStateOverflowReported)
		{
			_crowdStateOverflowReported = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._atlasArray)
		{
			_atlasArray = VariantUtils.ConvertTo<TextureLayered>(in value);
			return true;
		}
		if (name == PropertyName._atlasSize)
		{
			_atlasSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._poseTextureArray)
		{
			_poseTextureArray = VariantUtils.ConvertTo<TextureLayered>(in value);
			return true;
		}
		if (name == PropertyName._poseTextureSize)
		{
			_poseTextureSize = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._gpuRenderGraphTextureArray)
		{
			_gpuRenderGraphTextureArray = VariantUtils.ConvertTo<Texture2DArray>(in value);
			return true;
		}
		if (name == PropertyName._gpuRenderGraphTextureSize)
		{
			_gpuRenderGraphTextureSize = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._gpuDynamicOverrideTexture)
		{
			_gpuDynamicOverrideTexture = VariantUtils.ConvertTo<ImageTexture>(in value);
			return true;
		}
		if (name == PropertyName._gpuDynamicOverrideTextureSize)
		{
			_gpuDynamicOverrideTextureSize = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.EncoderOnly)
		{
			value = VariantUtils.CreateFrom<bool>(EncoderOnly);
			return true;
		}
		if (name == PropertyName._instance)
		{
			value = VariantUtils.CreateFrom(in _instance);
			return true;
		}
		if (name == PropertyName._multiMesh)
		{
			value = VariantUtils.CreateFrom(in _multiMesh);
			return true;
		}
		if (name == PropertyName._material)
		{
			value = VariantUtils.CreateFrom(in _material);
			return true;
		}
		if (name == PropertyName._quadMesh)
		{
			value = VariantUtils.CreateFrom(in _quadMesh);
			return true;
		}
		if (name == PropertyName._crowdInstance)
		{
			value = VariantUtils.CreateFrom(in _crowdInstance);
			return true;
		}
		if (name == PropertyName._crowdMultiMesh)
		{
			value = VariantUtils.CreateFrom(in _crowdMultiMesh);
			return true;
		}
		if (name == PropertyName._crowdMaterial)
		{
			value = VariantUtils.CreateFrom(in _crowdMaterial);
			return true;
		}
		if (name == PropertyName._crowdMesh)
		{
			value = VariantUtils.CreateFrom(in _crowdMesh);
			return true;
		}
		if (name == PropertyName._instanceDataImage)
		{
			value = VariantUtils.CreateFrom(in _instanceDataImage);
			return true;
		}
		if (name == PropertyName._instanceDataTexture)
		{
			value = VariantUtils.CreateFrom(in _instanceDataTexture);
			return true;
		}
		if (name == PropertyName._instanceDataTextureSize)
		{
			value = VariantUtils.CreateFrom(in _instanceDataTextureSize);
			return true;
		}
		if (name == PropertyName._instanceDataTexelCapacity)
		{
			value = VariantUtils.CreateFrom(in _instanceDataTexelCapacity);
			return true;
		}
		if (name == PropertyName._instanceDataBuffer)
		{
			value = VariantUtils.CreateFrom(in _instanceDataBuffer);
			return true;
		}
		if (name == PropertyName._multiMeshBuffer)
		{
			value = VariantUtils.CreateFrom(in _multiMeshBuffer);
			return true;
		}
		if (name == PropertyName._crowdMultiMeshBuffer)
		{
			value = VariantUtils.CreateFrom(in _crowdMultiMeshBuffer);
			return true;
		}
		if (name == PropertyName._boundCrowdStateTexture)
		{
			value = VariantUtils.CreateFrom(in _boundCrowdStateTexture);
			return true;
		}
		if (name == PropertyName._boundCrowdStateTextureSize)
		{
			value = VariantUtils.CreateFrom(in _boundCrowdStateTextureSize);
			return true;
		}
		if (name == PropertyName._crowdGroupArenaMark)
		{
			value = VariantUtils.CreateFrom(in _crowdGroupArenaMark);
			return true;
		}
		if (name == PropertyName._crowdGroupBucketMark)
		{
			value = VariantUtils.CreateFrom(in _crowdGroupBucketMark);
			return true;
		}
		if (name == PropertyName._crowdGroupExpectedBucketEnd)
		{
			value = VariantUtils.CreateFrom(in _crowdGroupExpectedBucketEnd);
			return true;
		}
		if (name == PropertyName._crowdGroupCursorTexel)
		{
			value = VariantUtils.CreateFrom(in _crowdGroupCursorTexel);
			return true;
		}
		if (name == PropertyName._crowdGroupEndTexel)
		{
			value = VariantUtils.CreateFrom(in _crowdGroupEndTexel);
			return true;
		}
		if (name == PropertyName._standaloneCrowdFrameVersion)
		{
			value = VariantUtils.CreateFrom(in _standaloneCrowdFrameVersion);
			return true;
		}
		if (name == PropertyName._cachedCompositeOwnerOrigins)
		{
			value = VariantUtils.CreateFrom(in _cachedCompositeOwnerOrigins);
			return true;
		}
		if (name == PropertyName._capacity)
		{
			value = VariantUtils.CreateFrom(in _capacity);
			return true;
		}
		if (name == PropertyName._crowdCapacity)
		{
			value = VariantUtils.CreateFrom(in _crowdCapacity);
			return true;
		}
		if (name == PropertyName._bufferedCount)
		{
			value = VariantUtils.CreateFrom(in _bufferedCount);
			return true;
		}
		if (name == PropertyName._crowdBufferedCount)
		{
			value = VariantUtils.CreateFrom(in _crowdBufferedCount);
			return true;
		}
		if (name == PropertyName._crowdMeshMaxSlices)
		{
			value = VariantUtils.CreateFrom(in _crowdMeshMaxSlices);
			return true;
		}
		if (name == PropertyName._compactCrowdRootCount)
		{
			value = VariantUtils.CreateFrom(in _compactCrowdRootCount);
			return true;
		}
		if (name == PropertyName._compositeCrowdRootCount)
		{
			value = VariantUtils.CreateFrom(in _compositeCrowdRootCount);
			return true;
		}
		if (name == PropertyName._gpuGraphRootCount)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphRootCount);
			return true;
		}
		if (name == PropertyName._gpuGraphSlotCount)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphSlotCount);
			return true;
		}
		if (name == PropertyName._gpuGraphStateWrittenTexels)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphStateWrittenTexels);
			return true;
		}
		if (name == PropertyName._crowdStateOverflowReported)
		{
			value = VariantUtils.CreateFrom(in _crowdStateOverflowReported);
			return true;
		}
		if (name == PropertyName._atlasArray)
		{
			value = VariantUtils.CreateFrom(in _atlasArray);
			return true;
		}
		if (name == PropertyName._atlasSize)
		{
			value = VariantUtils.CreateFrom(in _atlasSize);
			return true;
		}
		if (name == PropertyName._poseTextureArray)
		{
			value = VariantUtils.CreateFrom(in _poseTextureArray);
			return true;
		}
		if (name == PropertyName._poseTextureSize)
		{
			value = VariantUtils.CreateFrom(in _poseTextureSize);
			return true;
		}
		if (name == PropertyName._gpuRenderGraphTextureArray)
		{
			value = VariantUtils.CreateFrom(in _gpuRenderGraphTextureArray);
			return true;
		}
		if (name == PropertyName._gpuRenderGraphTextureSize)
		{
			value = VariantUtils.CreateFrom(in _gpuRenderGraphTextureSize);
			return true;
		}
		if (name == PropertyName._gpuDynamicOverrideTexture)
		{
			value = VariantUtils.CreateFrom(in _gpuDynamicOverrideTexture);
			return true;
		}
		if (name == PropertyName._gpuDynamicOverrideTextureSize)
		{
			value = VariantUtils.CreateFrom(in _gpuDynamicOverrideTextureSize);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._instance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._multiMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._material, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._quadMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._crowdInstance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._crowdMultiMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._crowdMaterial, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._crowdMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._instanceDataImage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._instanceDataTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._instanceDataTextureSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._instanceDataTexelCapacity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedFloat32Array, PropertyName._instanceDataBuffer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedFloat32Array, PropertyName._multiMeshBuffer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedFloat32Array, PropertyName._crowdMultiMeshBuffer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._boundCrowdStateTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._boundCrowdStateTextureSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdGroupArenaMark, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdGroupBucketMark, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdGroupExpectedBucketEnd, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdGroupCursorTexel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdGroupEndTexel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._standaloneCrowdFrameVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedVector2Array, PropertyName._cachedCompositeOwnerOrigins, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._capacity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdCapacity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._bufferedCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdBufferedCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdMeshMaxSlices, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._compactCrowdRootCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._compositeCrowdRootCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphRootCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphSlotCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphStateWrittenTexels, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._crowdStateOverflowReported, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._atlasArray, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._atlasSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._poseTextureArray, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._poseTextureSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gpuRenderGraphTextureArray, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._gpuRenderGraphTextureSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gpuDynamicOverrideTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._gpuDynamicOverrideTextureSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EncoderOnly, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.EncoderOnly, Variant.From<bool>(EncoderOnly));
		info.AddProperty(PropertyName._instance, Variant.From(in _instance));
		info.AddProperty(PropertyName._multiMesh, Variant.From(in _multiMesh));
		info.AddProperty(PropertyName._material, Variant.From(in _material));
		info.AddProperty(PropertyName._quadMesh, Variant.From(in _quadMesh));
		info.AddProperty(PropertyName._crowdInstance, Variant.From(in _crowdInstance));
		info.AddProperty(PropertyName._crowdMultiMesh, Variant.From(in _crowdMultiMesh));
		info.AddProperty(PropertyName._crowdMaterial, Variant.From(in _crowdMaterial));
		info.AddProperty(PropertyName._crowdMesh, Variant.From(in _crowdMesh));
		info.AddProperty(PropertyName._instanceDataImage, Variant.From(in _instanceDataImage));
		info.AddProperty(PropertyName._instanceDataTexture, Variant.From(in _instanceDataTexture));
		info.AddProperty(PropertyName._instanceDataTextureSize, Variant.From(in _instanceDataTextureSize));
		info.AddProperty(PropertyName._instanceDataTexelCapacity, Variant.From(in _instanceDataTexelCapacity));
		info.AddProperty(PropertyName._instanceDataBuffer, Variant.From(in _instanceDataBuffer));
		info.AddProperty(PropertyName._multiMeshBuffer, Variant.From(in _multiMeshBuffer));
		info.AddProperty(PropertyName._crowdMultiMeshBuffer, Variant.From(in _crowdMultiMeshBuffer));
		info.AddProperty(PropertyName._boundCrowdStateTexture, Variant.From(in _boundCrowdStateTexture));
		info.AddProperty(PropertyName._boundCrowdStateTextureSize, Variant.From(in _boundCrowdStateTextureSize));
		info.AddProperty(PropertyName._crowdGroupArenaMark, Variant.From(in _crowdGroupArenaMark));
		info.AddProperty(PropertyName._crowdGroupBucketMark, Variant.From(in _crowdGroupBucketMark));
		info.AddProperty(PropertyName._crowdGroupExpectedBucketEnd, Variant.From(in _crowdGroupExpectedBucketEnd));
		info.AddProperty(PropertyName._crowdGroupCursorTexel, Variant.From(in _crowdGroupCursorTexel));
		info.AddProperty(PropertyName._crowdGroupEndTexel, Variant.From(in _crowdGroupEndTexel));
		info.AddProperty(PropertyName._standaloneCrowdFrameVersion, Variant.From(in _standaloneCrowdFrameVersion));
		info.AddProperty(PropertyName._cachedCompositeOwnerOrigins, Variant.From(in _cachedCompositeOwnerOrigins));
		info.AddProperty(PropertyName._capacity, Variant.From(in _capacity));
		info.AddProperty(PropertyName._crowdCapacity, Variant.From(in _crowdCapacity));
		info.AddProperty(PropertyName._bufferedCount, Variant.From(in _bufferedCount));
		info.AddProperty(PropertyName._crowdBufferedCount, Variant.From(in _crowdBufferedCount));
		info.AddProperty(PropertyName._crowdMeshMaxSlices, Variant.From(in _crowdMeshMaxSlices));
		info.AddProperty(PropertyName._compactCrowdRootCount, Variant.From(in _compactCrowdRootCount));
		info.AddProperty(PropertyName._compositeCrowdRootCount, Variant.From(in _compositeCrowdRootCount));
		info.AddProperty(PropertyName._gpuGraphRootCount, Variant.From(in _gpuGraphRootCount));
		info.AddProperty(PropertyName._gpuGraphSlotCount, Variant.From(in _gpuGraphSlotCount));
		info.AddProperty(PropertyName._gpuGraphStateWrittenTexels, Variant.From(in _gpuGraphStateWrittenTexels));
		info.AddProperty(PropertyName._crowdStateOverflowReported, Variant.From(in _crowdStateOverflowReported));
		info.AddProperty(PropertyName._atlasArray, Variant.From(in _atlasArray));
		info.AddProperty(PropertyName._atlasSize, Variant.From(in _atlasSize));
		info.AddProperty(PropertyName._poseTextureArray, Variant.From(in _poseTextureArray));
		info.AddProperty(PropertyName._poseTextureSize, Variant.From(in _poseTextureSize));
		info.AddProperty(PropertyName._gpuRenderGraphTextureArray, Variant.From(in _gpuRenderGraphTextureArray));
		info.AddProperty(PropertyName._gpuRenderGraphTextureSize, Variant.From(in _gpuRenderGraphTextureSize));
		info.AddProperty(PropertyName._gpuDynamicOverrideTexture, Variant.From(in _gpuDynamicOverrideTexture));
		info.AddProperty(PropertyName._gpuDynamicOverrideTextureSize, Variant.From(in _gpuDynamicOverrideTextureSize));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.EncoderOnly, out var value))
		{
			EncoderOnly = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._instance, out var value2))
		{
			_instance = value2.As<MultiMeshInstance2D>();
		}
		if (info.TryGetProperty(PropertyName._multiMesh, out var value3))
		{
			_multiMesh = value3.As<MultiMesh>();
		}
		if (info.TryGetProperty(PropertyName._material, out var value4))
		{
			_material = value4.As<ShaderMaterial>();
		}
		if (info.TryGetProperty(PropertyName._quadMesh, out var value5))
		{
			_quadMesh = value5.As<ArrayMesh>();
		}
		if (info.TryGetProperty(PropertyName._crowdInstance, out var value6))
		{
			_crowdInstance = value6.As<MultiMeshInstance2D>();
		}
		if (info.TryGetProperty(PropertyName._crowdMultiMesh, out var value7))
		{
			_crowdMultiMesh = value7.As<MultiMesh>();
		}
		if (info.TryGetProperty(PropertyName._crowdMaterial, out var value8))
		{
			_crowdMaterial = value8.As<ShaderMaterial>();
		}
		if (info.TryGetProperty(PropertyName._crowdMesh, out var value9))
		{
			_crowdMesh = value9.As<ArrayMesh>();
		}
		if (info.TryGetProperty(PropertyName._instanceDataImage, out var value10))
		{
			_instanceDataImage = value10.As<Image>();
		}
		if (info.TryGetProperty(PropertyName._instanceDataTexture, out var value11))
		{
			_instanceDataTexture = value11.As<ImageTexture>();
		}
		if (info.TryGetProperty(PropertyName._instanceDataTextureSize, out var value12))
		{
			_instanceDataTextureSize = value12.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._instanceDataTexelCapacity, out var value13))
		{
			_instanceDataTexelCapacity = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName._instanceDataBuffer, out var value14))
		{
			_instanceDataBuffer = value14.As<float[]>();
		}
		if (info.TryGetProperty(PropertyName._multiMeshBuffer, out var value15))
		{
			_multiMeshBuffer = value15.As<float[]>();
		}
		if (info.TryGetProperty(PropertyName._crowdMultiMeshBuffer, out var value16))
		{
			_crowdMultiMeshBuffer = value16.As<float[]>();
		}
		if (info.TryGetProperty(PropertyName._boundCrowdStateTexture, out var value17))
		{
			_boundCrowdStateTexture = value17.As<TextureLayered>();
		}
		if (info.TryGetProperty(PropertyName._boundCrowdStateTextureSize, out var value18))
		{
			_boundCrowdStateTextureSize = value18.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._crowdGroupArenaMark, out var value19))
		{
			_crowdGroupArenaMark = value19.As<int>();
		}
		if (info.TryGetProperty(PropertyName._crowdGroupBucketMark, out var value20))
		{
			_crowdGroupBucketMark = value20.As<int>();
		}
		if (info.TryGetProperty(PropertyName._crowdGroupExpectedBucketEnd, out var value21))
		{
			_crowdGroupExpectedBucketEnd = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName._crowdGroupCursorTexel, out var value22))
		{
			_crowdGroupCursorTexel = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName._crowdGroupEndTexel, out var value23))
		{
			_crowdGroupEndTexel = value23.As<int>();
		}
		if (info.TryGetProperty(PropertyName._standaloneCrowdFrameVersion, out var value24))
		{
			_standaloneCrowdFrameVersion = value24.As<long>();
		}
		if (info.TryGetProperty(PropertyName._cachedCompositeOwnerOrigins, out var value25))
		{
			_cachedCompositeOwnerOrigins = value25.As<Vector2[]>();
		}
		if (info.TryGetProperty(PropertyName._capacity, out var value26))
		{
			_capacity = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName._crowdCapacity, out var value27))
		{
			_crowdCapacity = value27.As<int>();
		}
		if (info.TryGetProperty(PropertyName._bufferedCount, out var value28))
		{
			_bufferedCount = value28.As<int>();
		}
		if (info.TryGetProperty(PropertyName._crowdBufferedCount, out var value29))
		{
			_crowdBufferedCount = value29.As<int>();
		}
		if (info.TryGetProperty(PropertyName._crowdMeshMaxSlices, out var value30))
		{
			_crowdMeshMaxSlices = value30.As<int>();
		}
		if (info.TryGetProperty(PropertyName._compactCrowdRootCount, out var value31))
		{
			_compactCrowdRootCount = value31.As<int>();
		}
		if (info.TryGetProperty(PropertyName._compositeCrowdRootCount, out var value32))
		{
			_compositeCrowdRootCount = value32.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphRootCount, out var value33))
		{
			_gpuGraphRootCount = value33.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphSlotCount, out var value34))
		{
			_gpuGraphSlotCount = value34.As<int>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphStateWrittenTexels, out var value35))
		{
			_gpuGraphStateWrittenTexels = value35.As<int>();
		}
		if (info.TryGetProperty(PropertyName._crowdStateOverflowReported, out var value36))
		{
			_crowdStateOverflowReported = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._atlasArray, out var value37))
		{
			_atlasArray = value37.As<TextureLayered>();
		}
		if (info.TryGetProperty(PropertyName._atlasSize, out var value38))
		{
			_atlasSize = value38.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._poseTextureArray, out var value39))
		{
			_poseTextureArray = value39.As<TextureLayered>();
		}
		if (info.TryGetProperty(PropertyName._poseTextureSize, out var value40))
		{
			_poseTextureSize = value40.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._gpuRenderGraphTextureArray, out var value41))
		{
			_gpuRenderGraphTextureArray = value41.As<Texture2DArray>();
		}
		if (info.TryGetProperty(PropertyName._gpuRenderGraphTextureSize, out var value42))
		{
			_gpuRenderGraphTextureSize = value42.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._gpuDynamicOverrideTexture, out var value43))
		{
			_gpuDynamicOverrideTexture = value43.As<ImageTexture>();
		}
		if (info.TryGetProperty(PropertyName._gpuDynamicOverrideTextureSize, out var value44))
		{
			_gpuDynamicOverrideTextureSize = value44.As<Vector2I>();
		}
	}
}
