using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[GlobalClass]
[ScriptPath("res://addons/AdobeAnimateEditor/Node/AdobeAnimateSprite.cs")]
public class AdobeAnimateSprite : Node2D
{
	private struct OwnedChildBinding
	{
		public AdobeAnimateSlot Slot;

		public AdobeAnimateSprite Sprite;

		public AdobeAnimateSlot OwnerSlot;
	}

	private struct InsertedSprite
	{
		public AdobeAnimateSprite sprite;

		public int layerId;
	}

	private readonly struct LayerActiveCacheKey(AdobeAnimateRuntimeDefinition definition, int layerCount, int startFrame, int endFrameExclusive) : IEquatable<LayerActiveCacheKey>
	{
		public AdobeAnimateRuntimeDefinition Definition { get; } = definition;

		public int LayerCount { get; } = layerCount;

		public int StartFrame { get; } = startFrame;

		public int EndFrameExclusive { get; } = endFrameExclusive;

		public bool Equals(LayerActiveCacheKey other)
		{
			if (Definition == other.Definition && LayerCount == other.LayerCount && StartFrame == other.StartFrame)
			{
				return EndFrameExclusive == other.EndFrameExclusive;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is LayerActiveCacheKey other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Definition, LayerCount, StartFrame, EndFrameExclusive);
		}
	}

	public delegate void AnimeCompletedEventHandler(string clip);

	public delegate void AnimeStopedEventHandler(string clip);

	public delegate void AnimeStartedEventHandler(string clip);

	public delegate void AnimeEventEventHandler(string command, Variant argument);

	public delegate void AnimeBlendCompletedEventHandler(string clip);

	internal enum EffectOnceGpuAdvanceResult
	{
		Invalidated,
		Running,
		Completed
	}

	private readonly struct EffectiveAncestorModulateCacheEntry(Node renderMountParent, Color value)
	{
		public Node RenderMountParent { get; } = renderMountParent;

		public Color Value { get; } = value;
	}

	private readonly struct StaticAtlasPathLayoutKey(int cacheVersion, ulong dataInstanceId, long authoringRevision, int mediaCount, int textureCount, int useCount, int pathCount, int effectiveLimit, int activeCount, ulong slotSignature) : IEquatable<StaticAtlasPathLayoutKey>
	{
		public int CacheVersion { get; } = cacheVersion;

		public ulong DataInstanceId { get; } = dataInstanceId;

		public long AuthoringRevision { get; } = authoringRevision;

		public int MediaCount { get; } = mediaCount;

		public int TextureCount { get; } = textureCount;

		public int UseCount { get; } = useCount;

		public int PathCount { get; } = pathCount;

		public int EffectiveLimit { get; } = effectiveLimit;

		public int ActiveCount { get; } = activeCount;

		public ulong SlotSignature { get; } = slotSignature;

		public bool Equals(StaticAtlasPathLayoutKey other)
		{
			if (CacheVersion == other.CacheVersion && DataInstanceId == other.DataInstanceId && AuthoringRevision == other.AuthoringRevision && MediaCount == other.MediaCount && TextureCount == other.TextureCount && UseCount == other.UseCount && PathCount == other.PathCount && EffectiveLimit == other.EffectiveLimit && ActiveCount == other.ActiveCount)
			{
				return SlotSignature == other.SlotSignature;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is StaticAtlasPathLayoutKey other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(CacheVersion, DataInstanceId, AuthoringRevision, MediaCount, TextureCount, UseCount, PathCount, HashCode.Combine(EffectiveLimit, ActiveCount, SlotSignature));
		}
	}

	private sealed class StaticAtlasPathLayoutCacheEntry
	{
		public StaticAtlasPathLayoutKey Key { get; }

		public AdobeAnimateData Data { get; }

		public bool[] Active { get; }

		public string[] Paths { get; }

		public Rect2[] Rects { get; }

		public int[] Pages { get; }

		public TextureLayered TextureArray { get; }

		public Rid TextureArrayRid { get; }

		public Vector2 TextureArraySize { get; }

		public StaticAtlasPathLayoutCacheEntry(in StaticAtlasPathLayoutKey key, AdobeAnimateData data, bool[] active, string[] paths, Rect2[] rects, int[] pages, TextureLayered textureArray, Rid textureArrayRid, Vector2 textureArraySize)
		{
			Key = key;
			Data = data;
			Active = active;
			Paths = paths;
			Rects = rects;
			Pages = pages;
			TextureArray = textureArray;
			TextureArrayRid = textureArrayRid;
			TextureArraySize = textureArraySize;
		}
	}

	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName ResolveCrowdFrameInterpolation = "ResolveCrowdFrameInterpolation";

		public static readonly StringName BuildCompositeCacheStaticSignature = "BuildCompositeCacheStaticSignature";

		public static readonly StringName CanSkipCrowdRenderFromCachedCulling = "CanSkipCrowdRenderFromCachedCulling";

		public static readonly StringName ScheduleNextGpuGraphCrowdStateAudit = "ScheduleNextGpuGraphCrowdStateAudit";

		public static readonly StringName EmitAnimeCompleted = "EmitAnimeCompleted";

		public static readonly StringName MarkEffectOnceBatchEligibilityChanged = "MarkEffectOnceBatchEligibilityChanged";

		public static readonly StringName GetEffectOnceBatchPlaybackRevision = "GetEffectOnceBatchPlaybackRevision";

		public static readonly StringName IsEffectOnceGpuBatchPlaybackPrepared = "IsEffectOnceGpuBatchPlaybackPrepared";

		public static readonly StringName GetEffectOnceGpuBatchElapsedFrame = "GetEffectOnceGpuBatchElapsedFrame";

		public static readonly StringName AdvanceEffectOnceGpuPlayback = "AdvanceEffectOnceGpuPlayback";

		public static readonly StringName IsEffectOnceGpuCallbackStateCurrent = "IsEffectOnceGpuCallbackStateCurrent";

		public static readonly StringName TryImportEffectOnceGpuPlaybackPositionWithoutEvents = "TryImportEffectOnceGpuPlaybackPositionWithoutEvents";

		public static readonly StringName GetEffectOnceBatchVisualRevision = "GetEffectOnceBatchVisualRevision";

		public static readonly StringName GetEffectOnceBatchEligibilityRevision = "GetEffectOnceBatchEligibilityRevision";

		public static readonly StringName InvalidateEffectOnceBatchRenderState = "InvalidateEffectOnceBatchRenderState";

		public static readonly StringName MarkEffectOnceBatchRenderStateDirty = "MarkEffectOnceBatchRenderStateDirty";

		public static readonly StringName SetRenderClipControl = "SetRenderClipControl";

		public static readonly StringName ClearRenderClipControl = "ClearRenderClipControl";

		public static readonly StringName InvalidateGpuRenderGraphForOffsetChange = "InvalidateGpuRenderGraphForOffsetChange";

		public static readonly StringName SetPlaybackBlocked = "SetPlaybackBlocked";

		public static readonly StringName CommitPlaybackControlChange = "CommitPlaybackControlChange";

		public static readonly StringName SetParentPlaybackStopped = "SetParentPlaybackStopped";

		public static readonly StringName RefreshRuntimeChildPlaybackControl = "RefreshRuntimeChildPlaybackControl";

		public static readonly StringName GetEffectOnceGpuBatchSuppressionToken = "GetEffectOnceGpuBatchSuppressionToken";

		public static readonly StringName BeginEffectOnceGpuBatchSuppression = "BeginEffectOnceGpuBatchSuppression";

		public static readonly StringName EndEffectOnceGpuBatchSuppression = "EndEffectOnceGpuBatchSuppression";

		public static readonly StringName SetFrozenPreview = "SetFrozenPreview";

		public static readonly StringName EnsureFrozenPreviewRenderSubmission = "EnsureFrozenPreviewRenderSubmission";

		public static readonly StringName CommitPlaybackDirectionChange = "CommitPlaybackDirectionChange";

		public static readonly StringName GetEffectOnceBatchLayerVisible = "GetEffectOnceBatchLayerVisible";

		public static readonly StringName GetLayerVisibleForInternalRead = "GetLayerVisibleForInternalRead";

		public static readonly StringName GetLayerVisibleCountForRender = "GetLayerVisibleCountForRender";

		public static readonly StringName GetMediaReplaceForInternalRead = "GetMediaReplaceForInternalRead";

		public static readonly StringName GetMediaReplaceUseForInternalRead = "GetMediaReplaceUseForInternalRead";

		public static readonly StringName GetEffectOnceBatchMediaReplaceUse = "GetEffectOnceBatchMediaReplaceUse";

		public static readonly StringName BeginEffectOnceBatchEligibilityTracking = "BeginEffectOnceBatchEligibilityTracking";

		public static readonly StringName EndEffectOnceBatchEligibilityTracking = "EndEffectOnceBatchEligibilityTracking";

		public static readonly StringName GetEffectOnceBatchArrayExposureRevision = "GetEffectOnceBatchArrayExposureRevision";

		public static readonly StringName MarkEffectOnceBatchArrayExposure = "MarkEffectOnceBatchArrayExposure";

		public static readonly StringName MarkRenderStaticLayerArrayExposure = "MarkRenderStaticLayerArrayExposure";

		public static readonly StringName MarkRenderStaticMediaArrayExposure = "MarkRenderStaticMediaArrayExposure";

		public static readonly StringName ArmRenderStaticArrayAuditAfterExposure = "ArmRenderStaticArrayAuditAfterExposure";

		public static readonly StringName SetRenderColorMultiplier = "SetRenderColorMultiplier";

		public static readonly StringName CanUseGpuHitFlashEnvelope = "CanUseGpuHitFlashEnvelope";

		public static readonly StringName StartGpuHitFlashEnvelope = "StartGpuHitFlashEnvelope";

		public static readonly StringName ClearGpuHitFlashEnvelopes = "ClearGpuHitFlashEnvelopes";

		public static readonly StringName SetRenderGrayscale = "SetRenderGrayscale";

		public static readonly StringName IsRenderGrayscaleEnabled = "IsRenderGrayscaleEnabled";

		public static readonly StringName SetRenderSelfModulate = "SetRenderSelfModulate";

		public static readonly StringName NotifyAncestorModulateChangedForRender = "NotifyAncestorModulateChangedForRender";

		public static readonly StringName ApplyComposedModulate = "ApplyComposedModulate";

		public static readonly StringName RequestRuntimeRenderSubmissionRetry = "RequestRuntimeRenderSubmissionRetry";

		public static readonly StringName MarkRuntimeRenderSubmissionConsumed = "MarkRuntimeRenderSubmissionConsumed";

		public static readonly StringName MarkRuntimeDisplayTicked = "MarkRuntimeDisplayTicked";

		public static readonly StringName WasRuntimeDisplayTicked = "WasRuntimeDisplayTicked";

		public static readonly StringName SetRuntimeManagerDispatchActive = "SetRuntimeManagerDispatchActive";

		public static readonly StringName InvalidateRuntimePauseDispatchCache = "InvalidateRuntimePauseDispatchCache";

		public new static readonly StringName _GetPropertyList = "_GetPropertyList";

		public static readonly StringName AddParentSpriteFollowLayerProperty = "AddParentSpriteFollowLayerProperty";

		public static readonly StringName AddParentSpriteInsertLayerProperty = "AddParentSpriteInsertLayerProperty";

		public static readonly StringName AddParentSpriteLayerProperty = "AddParentSpriteLayerProperty";

		public static readonly StringName BuildParentSpriteLayerHintString = "BuildParentSpriteLayerHintString";

		public static readonly StringName ResolveParentSpriteLayerInspectorData = "ResolveParentSpriteLayerInspectorData";

		public static readonly StringName ResolveParentSpriteForLayerInspector = "ResolveParentSpriteForLayerInspector";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName SealPackedSceneStaticArraysInSubtree = "SealPackedSceneStaticArraysInSubtree";

		public static readonly StringName SealPackedSceneStaticArrays = "SealPackedSceneStaticArrays";

		public new static readonly StringName _Set = "_Set";

		public new static readonly StringName _Get = "_Get";

		public new static readonly StringName _PropertyCanRevert = "_PropertyCanRevert";

		public new static readonly StringName _PropertyGetRevert = "_PropertyGetRevert";

		public static readonly StringName IsParentSpriteFollowLayerProperty = "IsParentSpriteFollowLayerProperty";

		public static readonly StringName IsParentSpriteInsertLayerProperty = "IsParentSpriteInsertLayerProperty";

		public static readonly StringName SetParentSpriteFollowLayer = "SetParentSpriteFollowLayer";

		public static readonly StringName SetParentSpriteInsertLayer = "SetParentSpriteInsertLayer";

		public static readonly StringName CanRun = "CanRun";

		public static readonly StringName TryDisableProcess = "TryDisableProcess";

		public static readonly StringName UpdateVisibilityCache = "UpdateVisibilityCache";

		public static readonly StringName OnSelfVisibilityChanged = "OnSelfVisibilityChanged";

		public static readonly StringName SetProcessEnabled = "SetProcessEnabled";

		public static readonly StringName ConnectVisibilitySignals = "ConnectVisibilitySignals";

		public static readonly StringName DisconnectVisibilitySignals = "DisconnectVisibilitySignals";

		public static readonly StringName StopRuntimeTickKeepRender = "StopRuntimeTickKeepRender";

		public static readonly StringName RefreshProcessScheduling = "RefreshProcessScheduling";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public static readonly StringName GetCachedInstanceIdForRender = "GetCachedInstanceIdForRender";

		public static readonly StringName ConsumeRuntimeGpuPrepareRequiredForRender = "ConsumeRuntimeGpuPrepareRequiredForRender";

		public static readonly StringName HasPreparedGpuRenderGraph = "HasPreparedGpuRenderGraph";

		public static readonly StringName GetPreparedGpuRenderGraphQuadCount = "GetPreparedGpuRenderGraphQuadCount";

		public static readonly StringName MarkGpuRenderGraphPrepared = "MarkGpuRenderGraphPrepared";

		public static readonly StringName HasPreparedGpuRenderGraphState = "HasPreparedGpuRenderGraphState";

		public static readonly StringName ClearPreparedGpuRootCacheForRender = "ClearPreparedGpuRootCacheForRender";

		public static readonly StringName InvalidateGpuRenderGraphPreparation = "InvalidateGpuRenderGraphPreparation";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName GetEffectOnceBatchRenderTransform = "GetEffectOnceBatchRenderTransform";

		public static readonly StringName GetEffectOnceBatchEffectiveZIndex = "GetEffectOnceBatchEffectiveZIndex";

		public static readonly StringName GetEffectOnceBatchRenderRevision = "GetEffectOnceBatchRenderRevision";

		public static readonly StringName BuildCrowdFilterDebugReport = "BuildCrowdFilterDebugReport";

		public static readonly StringName PrintCrowdFilterDebugReport = "PrintCrowdFilterDebugReport";

		public static readonly StringName IsLayerVisibleFromArrayForDebug = "IsLayerVisibleFromArrayForDebug";

		public static readonly StringName GetLayerNameForDebug = "GetLayerNameForDebug";

		public static readonly StringName GetMediaNameForDebug = "GetMediaNameForDebug";

		public static readonly StringName FormatDebugRect = "FormatDebugRect";

		public static readonly StringName FormatDebugColor = "FormatDebugColor";

		public static readonly StringName HasExternalVisualsForRender = "HasExternalVisualsForRender";

		public static readonly StringName HasExternalVisualsInOwnedGraphForRender = "HasExternalVisualsInOwnedGraphForRender";

		public static readonly StringName GetExternalVisualStateVersionForRender = "GetExternalVisualStateVersionForRender";

		public static readonly StringName GetExternalVisualTopologyVersionForRender = "GetExternalVisualTopologyVersionForRender";

		public static readonly StringName GetCpuVisualSignatureForRender = "GetCpuVisualSignatureForRender";

		public static readonly StringName EnsureExternalVisualGpuStateCache = "EnsureExternalVisualGpuStateCache";

		public static readonly StringName ResetExternalVisualGpuStateCache = "ResetExternalVisualGpuStateCache";

		public static readonly StringName BeginExternalVisualPreparationForRender = "BeginExternalVisualPreparationForRender";

		public static readonly StringName MarkExternalVisualPreparedForRender = "MarkExternalVisualPreparedForRender";

		public static readonly StringName CommitExternalVisualCrowdFrame = "CommitExternalVisualCrowdFrame";

		public static readonly StringName HideExternalVisualCpuFrame = "HideExternalVisualCpuFrame";

		public static readonly StringName RestoreExternalVisualNativeFallbacks = "RestoreExternalVisualNativeFallbacks";

		public static readonly StringName InvalidateExternalVisualTopology = "InvalidateExternalVisualTopology";

		public static readonly StringName PositiveModulo = "PositiveModulo";

		public static readonly StringName CanReuseRuntimeGpuClockPose = "CanReuseRuntimeGpuClockPose";

		public static readonly StringName ComputeCanReuseRuntimeGpuClockPose = "ComputeCanReuseRuntimeGpuClockPose";

		public static readonly StringName RequiresDisplayFrameVisualTick = "RequiresDisplayFrameVisualTick";

		public static readonly StringName ComputeRequiresDisplayFrameVisualTick = "ComputeRequiresDisplayFrameVisualTick";

		public static readonly StringName InvalidateRuntimeDisplayVisualRequirementCache = "InvalidateRuntimeDisplayVisualRequirementCache";

		public static readonly StringName RequiresDisplayCadenceRuntimeChildUpdate = "RequiresDisplayCadenceRuntimeChildUpdate";

		public static readonly StringName RequestAnimationPoseRedraw = "RequestAnimationPoseRedraw";

		public static readonly StringName CanReuseCachedCulledPose = "CanReuseCachedCulledPose";

		public static readonly StringName InvalidateRuntimeCrowdCullingForRefresh = "InvalidateRuntimeCrowdCullingForRefresh";

		public static readonly StringName InvalidateRuntimeCrowdCullingIfCachedOffscreen = "InvalidateRuntimeCrowdCullingIfCachedOffscreen";

		public static readonly StringName HasRuntimeCrowdViewportChanged = "HasRuntimeCrowdViewportChanged";

		public static readonly StringName DisableRuntimeGpuClockInterpolationForOwnedGraph = "DisableRuntimeGpuClockInterpolationForOwnedGraph";

		public static readonly StringName InvalidateRuntimeRenderAfterGpuCacheReset = "InvalidateRuntimeRenderAfterGpuCacheReset";

		public static readonly StringName HasAnyRequestedMediaReplace = "HasAnyRequestedMediaReplace";

		public static readonly StringName UpdateChild = "UpdateChild";

		public static readonly StringName EmitFrameEvents = "EmitFrameEvents";

		public static readonly StringName CacheChildren = "CacheChildren";

		public static readonly StringName MarkSlotRuntimeUpdateCacheDirty = "MarkSlotRuntimeUpdateCacheDirty";

		public static readonly StringName SyncRuntimeChildState = "SyncRuntimeChildState";

		public static readonly StringName ApplyRuntimeParentState = "ApplyRuntimeParentState";

		public static readonly StringName ApplyRuntimeFollowVisibility = "ApplyRuntimeFollowVisibility";

		public static readonly StringName ResetRuntimeFollowVisibilityCache = "ResetRuntimeFollowVisibilityCache";

		public static readonly StringName GetRuntimeSlotChildren = "GetRuntimeSlotChildren";

		public static readonly StringName AutoInsertChildSprites = "AutoInsertChildSprites";

		public static readonly StringName SetMultimeshModulate = "SetMultimeshModulate";

		public static readonly StringName HasClip = "HasClip";

		public static readonly StringName UpdateChildPoseImmediate = "UpdateChildPoseImmediate";

		public static readonly StringName SetClip = "SetClip";

		public static readonly StringName ResetAnimation = "ResetAnimation";

		public static readonly StringName SetAnimation = "SetAnimation";

		public static readonly StringName AddAnimation = "AddAnimation";

		public static readonly StringName ApplyImmediateAnimation = "ApplyImmediateAnimation";

		public static readonly StringName GetProgress = "GetProgress";

		public static readonly StringName SetFliter = "SetFliter";

		public static readonly StringName GetFliter = "GetFliter";

		public static readonly StringName SetFliters = "SetFliters";

		public static readonly StringName SetFliterRecursive = "SetFliterRecursive";

		public static readonly StringName SetFlitersRecursive = "SetFlitersRecursive";

		public static readonly StringName ApplyFliterToChildSprites = "ApplyFliterToChildSprites";

		public static readonly StringName ApplyFlitersToChildSprites = "ApplyFlitersToChildSprites";

		public static readonly StringName IsMatchingRecursiveFilterSprite = "IsMatchingRecursiveFilterSprite";

		public static readonly StringName GetRecursiveFilterSourceKey = "GetRecursiveFilterSourceKey";

		public static readonly StringName SetReplace = "SetReplace";

		public static readonly StringName SetAtlasReplace = "SetAtlasReplace";

		public static readonly StringName GetAtlasReplacePath = "GetAtlasReplacePath";

		public static readonly StringName GetReplace = "GetReplace";

		public static readonly StringName SetVerticalClip = "SetVerticalClip";

		public static readonly StringName SetDiscardUpPos = "SetDiscardUpPos";

		public static readonly StringName SetDiscardDownPos = "SetDiscardDownPos";

		public static readonly StringName SetSpriteGroupShaderParameter = "SetSpriteGroupShaderParameter";

		public static readonly StringName InsertSpriteAtLayer = "InsertSpriteAtLayer";

		public static readonly StringName RefreshChildInsertLayerForEditor = "RefreshChildInsertLayerForEditor";

		public static readonly StringName TrackInsertedSpriteAtLayer = "TrackInsertedSpriteAtLayer";

		public static readonly StringName UntrackInsertedSpriteForEditor = "UntrackInsertedSpriteForEditor";

		public static readonly StringName OnSpriteChildParentChanged = "OnSpriteChildParentChanged";

		public static readonly StringName RemoveInsertedSprite = "RemoveInsertedSprite";

		public static readonly StringName RemoveAllInsertedSprites = "RemoveAllInsertedSprites";

		public static readonly StringName ExportSpriteSave = "ExportSpriteSave";

		public static readonly StringName ImportSpriteSave = "ImportSpriteSave";

		public static readonly StringName ResetCachedRenderFrameEffectiveAncestorModulates = "ResetCachedRenderFrameEffectiveAncestorModulates";

		public static readonly StringName GetCachedEffectiveRenderAncestorModulate = "GetCachedEffectiveRenderAncestorModulate";

		public static readonly StringName MarkRenderOrderDebugForRender = "MarkRenderOrderDebugForRender";

		public static readonly StringName HasRenderOrderDebugPendingForRender = "HasRenderOrderDebugPendingForRender";

		public static readonly StringName InvalidateInheritedRenderOrderForRender = "InvalidateInheritedRenderOrderForRender";

		public static readonly StringName BuildRenderOrderParentChainDebug = "BuildRenderOrderParentChainDebug";

		public static readonly StringName GetEffectiveZIndexForRender = "GetEffectiveZIndexForRender";

		public static readonly StringName GetCachedEffectiveZIndexForRender = "GetCachedEffectiveZIndexForRender";

		public static readonly StringName GetEffectiveZIndex = "GetEffectiveZIndex";

		public static readonly StringName GetEffectiveTreeOrderPathForRender = "GetEffectiveTreeOrderPathForRender";

		public static readonly StringName RefreshTreeOrderPathWatcher = "RefreshTreeOrderPathWatcher";

		public static readonly StringName OnTreeOrderHierarchyChanged = "OnTreeOrderHierarchyChanged";

		public static readonly StringName AreTreeOrderPathsEqual = "AreTreeOrderPathsEqual";

		public static readonly StringName InvalidateTreeOrderPathCache = "InvalidateTreeOrderPathCache";

		public static readonly StringName InvalidateRuntimeTreeOrderForTopologyChange = "InvalidateRuntimeTreeOrderForTopologyChange";

		public static readonly StringName DisconnectTreeOrderPathWatchers = "DisconnectTreeOrderPathWatchers";

		public static readonly StringName BuildEffectiveTreeOrderPathForRender = "BuildEffectiveTreeOrderPathForRender";

		public static readonly StringName GetEffectiveRenderSortBandForRender = "GetEffectiveRenderSortBandForRender";

		public static readonly StringName ComputeEffectiveRenderSortBand = "ComputeEffectiveRenderSortBand";

		public static readonly StringName GetShowBehindParentSortBandBias = "GetShowBehindParentSortBandBias";

		public static readonly StringName GetStaggeredRescanInterval = "GetStaggeredRescanInterval";

		public static readonly StringName ComputeLayerVisibilitySignature = "ComputeLayerVisibilitySignature";

		public static readonly StringName BuildMediaReplaceSignature = "BuildMediaReplaceSignature";

		public static readonly StringName ComputeMediaReplaceBoundsGrow = "ComputeMediaReplaceBoundsGrow";

		public static readonly StringName HasActiveMediaReplace = "HasActiveMediaReplace";

		public static readonly StringName GetMediaReplaceAtlasLayer = "GetMediaReplaceAtlasLayer";

		public static readonly StringName HasValidMediaReplaceAtlas = "HasValidMediaReplaceAtlas";

		public static readonly StringName GetMediaReplaceAtlasRid = "GetMediaReplaceAtlasRid";

		public static readonly StringName MarkLayerStateChanged = "MarkLayerStateChanged";

		public static readonly StringName OnPartSnapshotCreated = "OnPartSnapshotCreated";

		public static readonly StringName RequestNodeRedraw = "RequestNodeRedraw";

		public static readonly StringName CreateActiveMediaReplaceSnapshotForRender = "CreateActiveMediaReplaceSnapshotForRender";

		public static readonly StringName CreateActiveMediaReplaceAtlasPathSnapshotForRender = "CreateActiveMediaReplaceAtlasPathSnapshotForRender";

		public static readonly StringName GetActiveMediaReplacementForRender = "GetActiveMediaReplacementForRender";

		public static readonly StringName GetActiveMediaReplacementAtlasPathForRender = "GetActiveMediaReplacementAtlasPathForRender";

		public static readonly StringName GetMediaSizeForRender = "GetMediaSizeForRender";

		public static readonly StringName ResetManagedPoseTrackCache = "ResetManagedPoseTrackCache";

		public static readonly StringName LerpPoseTransform = "LerpPoseTransform";

		public static readonly StringName HasInsertedSpritesForRender = "HasInsertedSpritesForRender";

		public static readonly StringName HasSpriteChildrenForRender = "HasSpriteChildrenForRender";

		public static readonly StringName NeedsDrawOrderSortBandsForRender = "NeedsDrawOrderSortBandsForRender";

		public static readonly StringName GetNeedsDrawOrderSortBandsForRender = "GetNeedsDrawOrderSortBandsForRender";

		public static readonly StringName HasManagedSlotSpritesForRender = "HasManagedSlotSpritesForRender";

		public static readonly StringName GetSpriteChildrenForRender = "GetSpriteChildrenForRender";

		public static readonly StringName IsEmptyHiddenGpuGraphPlaceholderForRender = "IsEmptyHiddenGpuGraphPlaceholderForRender";

		public static readonly StringName PrepareDetachedGpuGraphWarmup = "PrepareDetachedGpuGraphWarmup";

		public static readonly StringName RefreshManagedSlotSpriteOwnerCache = "RefreshManagedSlotSpriteOwnerCache";

		public static readonly StringName MarkManagedSlotSpriteCacheDirty = "MarkManagedSlotSpriteCacheDirty";

		public static readonly StringName EnsureManagedSlotGpuStateCache = "EnsureManagedSlotGpuStateCache";

		public static readonly StringName ResetManagedSlotGpuStateCache = "ResetManagedSlotGpuStateCache";

		public static readonly StringName MarkManagedSlotVisualStateChanged = "MarkManagedSlotVisualStateChanged";

		public static readonly StringName GetManagedSlotGpuStateRescanInterval = "GetManagedSlotGpuStateRescanInterval";

		public static readonly StringName DisconnectManagedSlotGpuStateWatchers = "DisconnectManagedSlotGpuStateWatchers";

		public static readonly StringName RefreshManagedSlotSpriteCacheForRender = "RefreshManagedSlotSpriteCacheForRender";

		public static readonly StringName GetRenderSortRootForRender = "GetRenderSortRootForRender";

		public static readonly StringName OwnsSpriteChildForRender = "OwnsSpriteChildForRender";

		public static readonly StringName FindParentSpriteAncestor = "FindParentSpriteAncestor";

		public static readonly StringName ShouldFallbackSlotPoseToDrawOrder = "ShouldFallbackSlotPoseToDrawOrder";

		public static readonly StringName LayerDictionaryContainsId = "LayerDictionaryContainsId";

		public static readonly StringName ResolveLayerSortBand = "ResolveLayerSortBand";

		public static readonly StringName ResolveManagedSpriteLayerSortBand = "ResolveManagedSpriteLayerSortBand";

		public static readonly StringName ResolveChildSortBand = "ResolveChildSortBand";

		public static readonly StringName IsSpriteChildOwnedBySlot = "IsSpriteChildOwnedBySlot";

		public static readonly StringName ResolveSpriteChildRenderLayer = "ResolveSpriteChildRenderLayer";

		public static readonly StringName ResolveSpriteChildInsertLayer = "ResolveSpriteChildInsertLayer";

		public static readonly StringName ResolveTopInsertLayerId = "ResolveTopInsertLayerId";

		public static readonly StringName ResolveSpriteChildFollowLayer = "ResolveSpriteChildFollowLayer";

		public static readonly StringName ApplyChildRenderSort = "ApplyChildRenderSort";

		public static readonly StringName ClearInsertedChildRenderSort = "ClearInsertedChildRenderSort";

		public static readonly StringName ApplyKnownGlobalTranslationForRender = "ApplyKnownGlobalTranslationForRender";

		public static readonly StringName TryConsumeKnownAncestorTranslationNotification = "TryConsumeKnownAncestorTranslationNotification";

		public static readonly StringName NotifySlotOwnedSpriteTransformChangedForRender = "NotifySlotOwnedSpriteTransformChangedForRender";

		public static readonly StringName NotifyLocalRenderTransformChangedForTree = "NotifyLocalRenderTransformChangedForTree";

		public static readonly StringName GetCachedGlobalTransformForRender = "GetCachedGlobalTransformForRender";

		public static readonly StringName IsContinuousRenderRootMotion = "IsContinuousRenderRootMotion";

		public static readonly StringName RefreshRenderMountCache = "RefreshRenderMountCache";

		public static readonly StringName ScheduleNextRenderMountAudit = "ScheduleNextRenderMountAudit";

		public static readonly StringName GetCachedRenderMountParent = "GetCachedRenderMountParent";

		public static readonly StringName GetEffectiveRenderModulate = "GetEffectiveRenderModulate";

		public static readonly StringName GetCachedRenderFrameModulate = "GetCachedRenderFrameModulate";

		public static readonly StringName GetCachedOwnRenderFrameModulate = "GetCachedOwnRenderFrameModulate";

		public static readonly StringName GetCachedOwnRenderFrameSelfModulate = "GetCachedOwnRenderFrameSelfModulate";

		public static readonly StringName EnsureCachedRenderLocalModulates = "EnsureCachedRenderLocalModulates";

		public static readonly StringName ScheduleNextRenderLocalModulateAudit = "ScheduleNextRenderLocalModulateAudit";

		public static readonly StringName RefreshEffectiveRenderModulateAncestorCache = "RefreshEffectiveRenderModulateAncestorCache";

		public static readonly StringName ResetEffectiveRenderModulateAncestorCache = "ResetEffectiveRenderModulateAncestorCache";

		public static readonly StringName IsInSubViewportRenderTarget = "IsInSubViewportRenderTarget";

		public static readonly StringName ShouldUseGlobalRuntimeManager = "ShouldUseGlobalRuntimeManager";

		public static readonly StringName CommitRuntimeNativeCanvasTakeover = "CommitRuntimeNativeCanvasTakeover";

		public static readonly StringName RecoverRuntimeNativeCanvasAfterMissedTransaction = "RecoverRuntimeNativeCanvasAfterMissedTransaction";

		public static readonly StringName RestoreRuntimeNativeCanvasLayer = "RestoreRuntimeNativeCanvasLayer";

		public static readonly StringName InvalidateRuntimeNativeCanvasSuppressionEligibility = "InvalidateRuntimeNativeCanvasSuppressionEligibility";

		public static readonly StringName CanSuppressRuntimeNativeCanvasTree = "CanSuppressRuntimeNativeCanvasTree";

		public static readonly StringName IsRuntimeNativeCanvasBranchCrowdManaged = "IsRuntimeNativeCanvasBranchCrowdManaged";

		public static readonly StringName NeedsDrawItemSortForRender = "NeedsDrawItemSortForRender";

		public static readonly StringName RequiresInternalDrawItemSortForRender = "RequiresInternalDrawItemSortForRender";

		public static readonly StringName ResolveRenderViewport = "ResolveRenderViewport";

		public static readonly StringName BeginViewportWorldRectRenderFrame = "BeginViewportWorldRectRenderFrame";

		public static readonly StringName EndViewportWorldRectRenderFrame = "EndViewportWorldRectRenderFrame";

		public static readonly StringName ResetCachedViewportWorldRectsForFrame = "ResetCachedViewportWorldRectsForFrame";

		public static readonly StringName GetCachedViewportWorldRect = "GetCachedViewportWorldRect";

		public static readonly StringName IsValidBounds = "IsValidBounds";

		public static readonly StringName MergeBounds = "MergeBounds";

		public static readonly StringName TransformRect = "TransformRect";

		public static readonly StringName GetRenderFrameFloat = "GetRenderFrameFloat";

		public static readonly StringName IsClipBlendActiveForRender = "IsClipBlendActiveForRender";

		public static readonly StringName GetPlaybackClipEndExclusive = "GetPlaybackClipEndExclusive";

		public static readonly StringName DoesLoopTerminalFrameAliasFirstFrame = "DoesLoopTerminalFrameAliasFirstFrame";

		public static readonly StringName CacheRenderSnapshotState = "CacheRenderSnapshotState";

		public static readonly StringName RefreshRenderStaticStateCache = "RefreshRenderStaticStateCache";

		public static readonly StringName RenderStaticArrayAuditSnapshotsMatch = "RenderStaticArrayAuditSnapshotsMatch";

		public static readonly StringName RenderStaticMediaAtlasPathAuditSnapshotMatches = "RenderStaticMediaAtlasPathAuditSnapshotMatches";

		public static readonly StringName CaptureRenderStaticArrayAuditSnapshots = "CaptureRenderStaticArrayAuditSnapshots";

		public static readonly StringName ShouldAuditRenderStaticArrays = "ShouldAuditRenderStaticArrays";

		public static readonly StringName RequiresRenderStaticArrayAuditForRuntimeManager = "RequiresRenderStaticArrayAuditForRuntimeManager";

		public static readonly StringName QueueRenderStaticArrayAuditIfDue = "QueueRenderStaticArrayAuditIfDue";

		public static readonly StringName ShouldAuditRenderStaticMediaArrays = "ShouldAuditRenderStaticMediaArrays";

		public static readonly StringName ScheduleNextRenderStaticStateAudit = "ScheduleNextRenderStaticStateAudit";

		public static readonly StringName GetDataRuntimeLayerCount = "GetDataRuntimeLayerCount";

		public static readonly StringName GetLayerDictionaryLayerCount = "GetLayerDictionaryLayerCount";

		public static readonly StringName LayerHasSlicesInCurrentClip = "LayerHasSlicesInCurrentClip";

		public static readonly StringName EnsureLayerActiveCache = "EnsureLayerActiveCache";

		public static readonly StringName IsMediaReplacedCached = "IsMediaReplacedCached";

		public static readonly StringName InvalidateRenderSnapshotCache = "InvalidateRenderSnapshotCache";

		public static readonly StringName QueueUpdateMediaReplace = "QueueUpdateMediaReplace";

		public static readonly StringName UpdateMediaReplace = "UpdateMediaReplace";

		public static readonly StringName UpdateMediaReplaceData = "UpdateMediaReplaceData";

		public static readonly StringName CreateMediaReplaceAtlas = "CreateMediaReplaceAtlas";

		public new static readonly StringName _Draw = "_Draw";

		public static readonly StringName IsRenderedByParentSpriteForRender = "IsRenderedByParentSpriteForRender";

		public static readonly StringName SubmitRuntimeManagerFirstFrameHandoff = "SubmitRuntimeManagerFirstFrameHandoff";

		public static readonly StringName TrySubmitRuntimeRenderFailureHandoff = "TrySubmitRuntimeRenderFailureHandoff";

		public static readonly StringName ReleaseRuntimeManagerFirstFrameHandoff = "ReleaseRuntimeManagerFirstFrameHandoff";

		public static readonly StringName ReleaseRuntimeManagerFirstFrameHandoffTree = "ReleaseRuntimeManagerFirstFrameHandoffTree";

		public static readonly StringName ReleaseForcedCpuPoseData = "ReleaseForcedCpuPoseData";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName RunBatchedProcessUpdate = "RunBatchedProcessUpdate";

		public static readonly StringName RunDisplayFrameVisualUpdate = "RunDisplayFrameVisualUpdate";

		public static readonly StringName BatchProcessUpdate = "BatchProcessUpdate";

		public static readonly StringName RunDefaultBatchedProcessUpdate = "RunDefaultBatchedProcessUpdate";

		public static readonly StringName RunBatchedProcessExtension = "RunBatchedProcessExtension";

		public static readonly StringName TryFastLoopProcessUpdate = "TryFastLoopProcessUpdate";

		public static readonly StringName EmitFastLoopCallbacks = "EmitFastLoopCallbacks";

		public static readonly StringName HasRuntimeChildUpdates = "HasRuntimeChildUpdates";

		public static readonly StringName RefreshRuntimeChildUpdatePresence = "RefreshRuntimeChildUpdatePresence";

		public static readonly StringName ShouldSkipRuntimeChildUpdateForViewport = "ShouldSkipRuntimeChildUpdateForViewport";

		public static readonly StringName ShouldUseGpuGraphVisualAttachment = "ShouldUseGpuGraphVisualAttachment";

		public static readonly StringName RuntimeChildUpdatesAreVisualOnly = "RuntimeChildUpdatesAreVisualOnly";

		public static readonly StringName IsRuntimeChildUpdateNearViewport = "IsRuntimeChildUpdateNearViewport";

		public static readonly StringName UpdateInvisibleChild = "UpdateInvisibleChild";

		public static readonly StringName FileChange = "FileChange";

		public static readonly StringName OnFlashAnimeDataChanged = "OnFlashAnimeDataChanged";

		public static readonly StringName ApplyFlashAnimeDataChange = "ApplyFlashAnimeDataChange";

		public static readonly StringName DeferFlashAnimeDataChangeUntilReady = "DeferFlashAnimeDataChangeUntilReady";

		public static readonly StringName ApplyPendingFlashAnimeDataChange = "ApplyPendingFlashAnimeDataChange";

		public static readonly StringName PrepareFlashAnimeDataSerializedOverrides = "PrepareFlashAnimeDataSerializedOverrides";

		public static readonly StringName HasUsableFlashAnimeData = "HasUsableFlashAnimeData";

		public static readonly StringName TryHydrateFlashAnimeDataBeforeFileChange = "TryHydrateFlashAnimeDataBeforeFileChange";

		public static readonly StringName RefreshTransformNotificationMode = "RefreshTransformNotificationMode";

		public static readonly StringName RefreshTransformNotificationModeForRenderTree = "RefreshTransformNotificationModeForRenderTree";

		public static readonly StringName MarkRenderTransformDirty = "MarkRenderTransformDirty";

		public static readonly StringName ClearRetainedRootMotionForPause = "ClearRetainedRootMotionForPause";

		public static readonly StringName ClearRetainedRootMotionForStoppedMovement = "ClearRetainedRootMotionForStoppedMovement";

		public static readonly StringName CanInterpolateRenderRootMotion = "CanInterpolateRenderRootMotion";

		public static readonly StringName HasCurrentPhysicsRootMotion = "HasCurrentPhysicsRootMotion";

		public static readonly StringName NotifyAncestorTransformChangedForRender = "NotifyAncestorTransformChangedForRender";

		public static readonly StringName NotifyAncestorTranslatedForRender = "NotifyAncestorTranslatedForRender";

		public static readonly StringName PrimeAncestorTranslationForRender = "PrimeAncestorTranslationForRender";

		public static readonly StringName InvalidateRuntimeRenderMountForAncestorChange = "InvalidateRuntimeRenderMountForAncestorChange";

		public static readonly StringName InvalidateProgressRestorePresentation = "InvalidateProgressRestorePresentation";

		public static readonly StringName InvalidateStaticAtlasPathLayoutConfigurationSnapshot = "InvalidateStaticAtlasPathLayoutConfigurationSnapshot";

		public static readonly StringName CaptureStaticAtlasPathLayoutConfigurationSnapshot = "CaptureStaticAtlasPathLayoutConfigurationSnapshot";

		public static readonly StringName InvalidateStaticAtlasPathLayoutForNodeMutation = "InvalidateStaticAtlasPathLayoutForNodeMutation";

		public static readonly StringName PrepareStaticAtlasPathLayoutForExplicitUpdate = "PrepareStaticAtlasPathLayoutForExplicitUpdate";

		public static readonly StringName InvalidateStaticAtlasPathLayoutCacheForGpuReset = "InvalidateStaticAtlasPathLayoutCacheForGpuReset";

		public static readonly StringName ClearStaticAtlasPathLayoutCacheForTests = "ClearStaticAtlasPathLayoutCacheForTests";

		public static readonly StringName TryApplyStaticAtlasPathLayoutCacheForTests = "TryApplyStaticAtlasPathLayoutCacheForTests";

		public static readonly StringName CanReuseAppliedStaticAtlasPathLayoutForTests = "CanReuseAppliedStaticAtlasPathLayoutForTests";

		public static readonly StringName CreateMediaReplaceAtlasWithoutStaticCacheForTests = "CreateMediaReplaceAtlasWithoutStaticCacheForTests";

		public static readonly StringName CorruptStaticAtlasPathLayoutEntryForTests = "CorruptStaticAtlasPathLayoutEntryForTests";

		public static readonly StringName TryApplyStaticAtlasPathLayoutCache = "TryApplyStaticAtlasPathLayoutCache";

		public static readonly StringName TryPublishStaticAtlasPathLayoutCache = "TryPublishStaticAtlasPathLayoutCache";

		public static readonly StringName TryReuseAppliedStaticAtlasPathLayout = "TryReuseAppliedStaticAtlasPathLayout";

		public static readonly StringName RefreshStaticAtlasPathLayoutCacheVersion = "RefreshStaticAtlasPathLayoutCacheVersion";

		public static readonly StringName ConfigureGpuClockForBareTest = "ConfigureGpuClockForBareTest";

		public static readonly StringName ApplyRuntimeParentStateForBareTest = "ApplyRuntimeParentStateForBareTest";

		public static readonly StringName ClearRenderSubmissionForBareTest = "ClearRenderSubmissionForBareTest";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public new static readonly StringName Modulate = "Modulate";

		public new static readonly StringName SelfModulate = "SelfModulate";

		public new static readonly StringName ZIndex = "ZIndex";

		public new static readonly StringName ZAsRelative = "ZAsRelative";

		public static readonly StringName atlasProfileOverride = "atlasProfileOverride";

		public static readonly StringName flashAnimeData = "flashAnimeData";

		public static readonly StringName preview = "preview";

		public static readonly StringName forceLocalRender = "forceLocalRender";

		public static readonly StringName forceCpuPoseRender = "forceCpuPoseRender";

		public static readonly StringName suppressEditorRenderSubmission = "suppressEditorRenderSubmission";

		public static readonly StringName invisible = "invisible";

		public static readonly StringName offset = "offset";

		public static readonly StringName timeScale = "timeScale";

		public static readonly StringName deduplicateLoopTerminalFrame = "deduplicateLoopTerminalFrame";

		public static readonly StringName IsFrozenPreview = "IsFrozenPreview";

		public static readonly StringName pause = "pause";

		public static readonly StringName IsPlaybackStopped = "IsPlaybackStopped";

		public static readonly StringName IsEffectOnceGpuSuppressed = "IsEffectOnceGpuSuppressed";

		public static readonly StringName runtimeViewportCullingEnabled = "runtimeViewportCullingEnabled";

		public static readonly StringName playBack = "playBack";

		public static readonly StringName ManagedPoseRevision = "ManagedPoseRevision";

		public static readonly StringName clip = "clip";

		public static readonly StringName layerVisible = "layerVisible";

		public static readonly StringName mediaReplace = "mediaReplace";

		public static readonly StringName mediaReplaceUse = "mediaReplaceUse";

		public static readonly StringName mediaReplaceAtlasPaths = "mediaReplaceAtlasPaths";

		public static readonly StringName parentSprite = "parentSprite";

		public static readonly StringName meshColor = "meshColor";

		public static readonly StringName IsRuntimeActive = "IsRuntimeActive";

		public static readonly StringName IsRuntimeTickPaused = "IsRuntimeTickPaused";

		public static readonly StringName IsRuntimeInsideTree = "IsRuntimeInsideTree";

		public static readonly StringName IsRuntimeVisibleInTree = "IsRuntimeVisibleInTree";

		public static readonly StringName IsRuntimeReadyCached = "IsRuntimeReadyCached";

		public static readonly StringName IsRuntimeDisplayTickActive = "IsRuntimeDisplayTickActive";

		public static readonly StringName NeedsRuntimeRenderSubmission = "NeedsRuntimeRenderSubmission";

		public static readonly StringName UsesRuntimeGpuClockInterpolation = "UsesRuntimeGpuClockInterpolation";

		public static readonly StringName PrefersCachedGpuGraphCrowdRenderState = "PrefersCachedGpuGraphCrowdRenderState";

		public static readonly StringName RuntimeManagerDispatchActive = "RuntimeManagerDispatchActive";

		public static readonly StringName ShouldDispatchProcessCallback = "ShouldDispatchProcessCallback";

		public static readonly StringName RequiresDisplayFrameBatchedProcessExtension = "RequiresDisplayFrameBatchedProcessExtension";

		public static readonly StringName NeedsRenderSubmissionForBareTest = "NeedsRenderSubmissionForBareTest";

		public static readonly StringName PlaybackRevisionForBareTest = "PlaybackRevisionForBareTest";

		public static readonly StringName _effectOnceBatchVisualRevision = "_effectOnceBatchVisualRevision";

		public static readonly StringName _effectOnceBatchEligibilityRevision = "_effectOnceBatchEligibilityRevision";

		public static readonly StringName _flashAnimeData = "_flashAnimeData";

		public static readonly StringName _flashAnimeDataChangePending = "_flashAnimeDataChangePending";

		public static readonly StringName _atlasProfileOverride = "_atlasProfileOverride";

		public static readonly StringName _preview = "_preview";

		public static readonly StringName _forceLocalRender = "_forceLocalRender";

		public static readonly StringName _renderClipControl = "_renderClipControl";

		public static readonly StringName _forceCpuPoseRender = "_forceCpuPoseRender";

		public static readonly StringName _invisible = "_invisible";

		public static readonly StringName normalAlpha = "normalAlpha";

		public static readonly StringName _offset = "_offset";

		public static readonly StringName offsetRotate = "offsetRotate";

		public static readonly StringName _timeScale = "_timeScale";

		public static readonly StringName trueFrameRate = "trueFrameRate";

		public static readonly StringName refreshEveryFlame = "refreshEveryFlame";

		public static readonly StringName skipLastFrame = "skipLastFrame";

		public static readonly StringName _deduplicateLoopTerminalFrame = "_deduplicateLoopTerminalFrame";

		public static readonly StringName usePos = "usePos";

		public static readonly StringName useRotate = "useRotate";

		public static readonly StringName useFollowVisible = "useFollowVisible";

		public static readonly StringName blendTimeInit = "blendTimeInit";

		public static readonly StringName _pause = "_pause";

		public static readonly StringName _playBack = "_playBack";

		public static readonly StringName _playbackBlocked = "_playbackBlocked";

		public static readonly StringName _parentPlaybackStopped = "_parentPlaybackStopped";

		public static readonly StringName _frozenPreview = "_frozenPreview";

		public static readonly StringName _effectOnceGpuSuppressed = "_effectOnceGpuSuppressed";

		public static readonly StringName _effectOnceGpuSuppressionToken = "_effectOnceGpuSuppressionToken";

		public static readonly StringName _applyingImmediateAnimation = "_applyingImmediateAnimation";

		public static readonly StringName keepRenderSubmittedWhenPaused = "keepRenderSubmittedWhenPaused";

		public static readonly StringName _runtimeViewportCullingEnabled = "_runtimeViewportCullingEnabled";

		public static readonly StringName onlyDraw = "onlyDraw";

		public static readonly StringName elapsedTimer = "elapsedTimer";

		public static readonly StringName refreshTimer = "refreshTimer";

		public static readonly StringName blend = "blend";

		public static readonly StringName blendTime = "blendTime";

		public static readonly StringName blendTimer = "blendTimer";

		public static readonly StringName _blendFromFrameFloat = "_blendFromFrameFloat";

		public static readonly StringName frameIndex = "frameIndex";

		public static readonly StringName frameRate = "frameRate";

		public static readonly StringName refreshEveryFrame = "refreshEveryFrame";

		public static readonly StringName loop = "loop";

		public static readonly StringName _clip = "_clip";

		public static readonly StringName _playbackRevision = "_playbackRevision";

		public static readonly StringName _managedPoseRevision = "_managedPoseRevision";

		public static readonly StringName clipRange = "clipRange";

		public static readonly StringName clipOver = "clipOver";

		public static readonly StringName _layerVisible = "_layerVisible";

		public static readonly StringName _effectOnceBatchArrayExposureRevision = "_effectOnceBatchArrayExposureRevision";

		public static readonly StringName _effectOnceBatchEligibilityTracking = "_effectOnceBatchEligibilityTracking";

		public static readonly StringName mediaReplaceAtlas = "mediaReplaceAtlas";

		public static readonly StringName mediaReplaceAtlasArray = "mediaReplaceAtlasArray";

		public static readonly StringName mediaReplaceAtlasArraySize = "mediaReplaceAtlasArraySize";

		public static readonly StringName mediaReplaceAtlasUsesTextureArray = "mediaReplaceAtlasUsesTextureArray";

		public static readonly StringName mediaReplaceRect = "mediaReplaceRect";

		public static readonly StringName mediaReplaceAtlasPages = "mediaReplaceAtlasPages";

		public static readonly StringName _mediaReplace = "_mediaReplace";

		public static readonly StringName _mediaReplaceUse = "_mediaReplaceUse";

		public static readonly StringName _mediaReplaceAtlasPaths = "_mediaReplaceAtlasPaths";

		public static readonly StringName mediaReplaceAtlasShared = "mediaReplaceAtlasShared";

		public static readonly StringName canvasItem = "canvasItem";

		public static readonly StringName meshTexture = "meshTexture";

		public static readonly StringName mesh = "mesh";

		public static readonly StringName needMediaReplaceUpdate = "needMediaReplaceUpdate";

		public static readonly StringName _canRun = "_canRun";

		public static readonly StringName initClip = "initClip";

		public static readonly StringName _parentSprite = "_parentSprite";

		public static readonly StringName _parentSpriteResolved = "_parentSpriteResolved";

		public static readonly StringName followParentSpriteLayerId = "followParentSpriteLayerId";

		public static readonly StringName insertLayerId = "insertLayerId";

		public static readonly StringName _meshColor = "_meshColor";

		public static readonly StringName _renderColorMultiplier = "_renderColorMultiplier";

		public static readonly StringName _runtimeGpuGraphActive = "_runtimeGpuGraphActive";

		public static readonly StringName _hasCachedGpuGraphCrowdState = "_hasCachedGpuGraphCrowdState";

		public static readonly StringName _cachedGpuGraphCrowdStatePhysicsFrame = "_cachedGpuGraphCrowdStatePhysicsFrame";

		public static readonly StringName _hasCachedRasterCompositeRenderState = "_hasCachedRasterCompositeRenderState";

		public static readonly StringName _gpuGraphCrowdStaticStateDirty = "_gpuGraphCrowdStaticStateDirty";

		public static readonly StringName _gpuGraphCrowdPresentationStateDirty = "_gpuGraphCrowdPresentationStateDirty";

		public static readonly StringName _gpuGraphCrowdStateNextAuditPhysicsFrame = "_gpuGraphCrowdStateNextAuditPhysicsFrame";

		public static readonly StringName _renderGrayscale = "_renderGrayscale";

		public static readonly StringName _autoInserting = "_autoInserting";

		public static readonly StringName _slotChildren = "_slotChildren";

		public static readonly StringName _runtimeSlotChildren = "_runtimeSlotChildren";

		public static readonly StringName _managedSlotGpuStateAtlasRids = "_managedSlotGpuStateAtlasRids";

		public static readonly StringName _managedSlotGpuStateRescanCountdowns = "_managedSlotGpuStateRescanCountdowns";

		public static readonly StringName _managedSlotGpuStateWatchedSprites = "_managedSlotGpuStateWatchedSprites";

		public static readonly StringName _managedSlotGpuStateWatchedParts = "_managedSlotGpuStateWatchedParts";

		public static readonly StringName _runtimeSlotChildrenDirty = "_runtimeSlotChildrenDirty";

		public static readonly StringName _runtimeHasChildUpdates = "_runtimeHasChildUpdates";

		public static readonly StringName _runtimeChildUpdatesVisualOnlyDirty = "_runtimeChildUpdatesVisualOnlyDirty";

		public static readonly StringName _runtimeChildUpdatesVisualOnlyCached = "_runtimeChildUpdatesVisualOnlyCached";

		public static readonly StringName _managedSlotSpritesDirty = "_managedSlotSpritesDirty";

		public static readonly StringName _spriteChildren = "_spriteChildren";

		public static readonly StringName _spriteChildOwnerSlots = "_spriteChildOwnerSlots";

		public static readonly StringName _hasChildren = "_hasChildren";

		public static readonly StringName _runtimeChildViewportFrame = "_runtimeChildViewportFrame";

		public static readonly StringName _runtimeChildViewportVisible = "_runtimeChildViewportVisible";

		public static readonly StringName _lastUpdateFrame = "_lastUpdateFrame";

		public static readonly StringName _isVisibleInTree = "_isVisibleInTree";

		public static readonly StringName _runtimeInsideTree = "_runtimeInsideTree";

		public static readonly StringName _processEnabled = "_processEnabled";

		public static readonly StringName _usingRuntimeManager = "_usingRuntimeManager";

		public static readonly StringName _cachedInstanceIdForRender = "_cachedInstanceIdForRender";

		public static readonly StringName _gpuGraphPreparedGeneration = "_gpuGraphPreparedGeneration";

		public static readonly StringName _gpuGraphPreparedSignature = "_gpuGraphPreparedSignature";

		public static readonly StringName _gpuGraphPreparedQuadCount = "_gpuGraphPreparedQuadCount";

		public static readonly StringName _dispatchingBatchedProcess = "_dispatchingBatchedProcess";

		public static readonly StringName _displayFrameVisualDispatchActive = "_displayFrameVisualDispatchActive";

		public static readonly StringName _runtimeDisplayTickVersion = "_runtimeDisplayTickVersion";

		public static readonly StringName _runtimeReadyCached = "_runtimeReadyCached";

		public static readonly StringName _runtimeNativeCanvasSuppressed = "_runtimeNativeCanvasSuppressed";

		public static readonly StringName _runtimeNativeCanvasPublishedFrameVersion = "_runtimeNativeCanvasPublishedFrameVersion";

		public static readonly StringName _runtimeManagerFirstFrameHandoffActive = "_runtimeManagerFirstFrameHandoffActive";

		public static readonly StringName _runtimeNativeCanvasVisibilityLayer = "_runtimeNativeCanvasVisibilityLayer";

		public static readonly StringName _runtimeNativeCanvasSuppressionEligibilityKnown = "_runtimeNativeCanvasSuppressionEligibilityKnown";

		public static readonly StringName _runtimeNativeCanvasSuppressionEligible = "_runtimeNativeCanvasSuppressionEligible";

		public static readonly StringName _runtimeParentStateSource = "_runtimeParentStateSource";

		public static readonly StringName _runtimeParentStateValidationCountdown = "_runtimeParentStateValidationCountdown";

		public static readonly StringName _runtimeFollowVisibilityKnown = "_runtimeFollowVisibilityKnown";

		public static readonly StringName _runtimeFollowVisibilityValue = "_runtimeFollowVisibilityValue";

		public static readonly StringName _needsRenderSubmission = "_needsRenderSubmission";

		public static readonly StringName _runtimeRenderSubmissionRetryRequested = "_runtimeRenderSubmissionRetryRequested";

		public static readonly StringName _runtimeGpuPrepareRequired = "_runtimeGpuPrepareRequired";

		public static readonly StringName _visibilityParentCanvas = "_visibilityParentCanvas";

		public static readonly StringName _selfVisibilityChangedConnected = "_selfVisibilityChangedConnected";

		public static readonly StringName _parentVisibilityChangedConnected = "_parentVisibilityChangedConnected";

		public static readonly StringName _cachedRenderMountParent = "_cachedRenderMountParent";

		public static readonly StringName _cachedViewport = "_cachedViewport";

		public static readonly StringName _renderMountNextAuditPhysicsFrame = "_renderMountNextAuditPhysicsFrame";

		public static readonly StringName _renderModulateMountParent = "_renderModulateMountParent";

		public static readonly StringName _renderModulateAncestorCacheReady = "_renderModulateAncestorCacheReady";

		public static readonly StringName _renderModulateAncestorRescanCountdown = "_renderModulateAncestorRescanCountdown";

		public static readonly StringName _cachedCanvasLayer = "_cachedCanvasLayer";

		public static readonly StringName _cachedManagedPoseTrackKey = "_cachedManagedPoseTrackKey";

		public static readonly StringName _cachedManagedPoseTrackUseLayerId = "_cachedManagedPoseTrackUseLayerId";

		public static readonly StringName _nextAnimDelayTimer = "_nextAnimDelayTimer";

		public static readonly StringName _renderStateCached = "_renderStateCached";

		public static readonly StringName _renderStaticStateCached = "_renderStaticStateCached";

		public static readonly StringName _renderStaticLayerArrayEscaped = "_renderStaticLayerArrayEscaped";

		public static readonly StringName _renderStaticMediaArraysEscaped = "_renderStaticMediaArraysEscaped";

		public static readonly StringName _renderStaticArrayAuditSubmissionPending = "_renderStaticArrayAuditSubmissionPending";

		public static readonly StringName _renderStaticStateNextAuditPhysicsFrame = "_renderStaticStateNextAuditPhysicsFrame";

		public static readonly StringName _renderStaticAuditLayerVisibleSnapshot = "_renderStaticAuditLayerVisibleSnapshot";

		public static readonly StringName _renderStaticAuditMediaReplaceUseSnapshot = "_renderStaticAuditMediaReplaceUseSnapshot";

		public static readonly StringName _renderStaticAuditMediaReplaceSnapshot = "_renderStaticAuditMediaReplaceSnapshot";

		public static readonly StringName _renderStaticAuditMediaReplaceRectSnapshot = "_renderStaticAuditMediaReplaceRectSnapshot";

		public static readonly StringName _renderStaticAuditMediaReplacePageSnapshot = "_renderStaticAuditMediaReplacePageSnapshot";

		public static readonly StringName _renderStaticAuditMediaReplaceAtlasPathSnapshot = "_renderStaticAuditMediaReplaceAtlasPathSnapshot";

		public static readonly StringName _cachedGpuGraphRootOwnerPlaybackRevision = "_cachedGpuGraphRootOwnerPlaybackRevision";

		public static readonly StringName _hasCachedGpuGraphNestedOwnerState = "_hasCachedGpuGraphNestedOwnerState";

		public static readonly StringName _cachedGpuGraphNestedOwnerPlaybackRevision = "_cachedGpuGraphNestedOwnerPlaybackRevision";

		public static readonly StringName _gpuGraphNestedOwnerStateRebaseClockSeconds = "_gpuGraphNestedOwnerStateRebaseClockSeconds";

		public static readonly StringName _cachedClip = "_cachedClip";

		public static readonly StringName _cachedClipRange = "_cachedClipRange";

		public static readonly StringName _cachedFrameFloat = "_cachedFrameFloat";

		public static readonly StringName _cachedGlobalTransform = "_cachedGlobalTransform";

		public static readonly StringName _renderGlobalTransform = "_renderGlobalTransform";

		public static readonly StringName _renderPreviousGlobalTransform = "_renderPreviousGlobalTransform";

		public static readonly StringName _renderGlobalTransformCached = "_renderGlobalTransformCached";

		public static readonly StringName _renderGlobalTransformDirty = "_renderGlobalTransformDirty";

		public static readonly StringName _renderTransformChangedInPhysicsFrame = "_renderTransformChangedInPhysicsFrame";

		public static readonly StringName _runtimePhysicsAncestorTransformPending = "_runtimePhysicsAncestorTransformPending";

		public static readonly StringName _knownAncestorTranslationNotificationPending = "_knownAncestorTranslationNotificationPending";

		public static readonly StringName _renderRootMotionEnabled = "_renderRootMotionEnabled";

		public static readonly StringName _renderTreePaused = "_renderTreePaused";

		public static readonly StringName _renderGlobalTransformPhysicsFrame = "_renderGlobalTransformPhysicsFrame";

		public static readonly StringName _cachedModulate = "_cachedModulate";

		public static readonly StringName _cachedOffset = "_cachedOffset";

		public static readonly StringName _cachedMediaReplaceAtlas = "_cachedMediaReplaceAtlas";

		public static readonly StringName _cachedMediaReplaceAtlasArray = "_cachedMediaReplaceAtlasArray";

		public static readonly StringName _cachedMediaReplaceAtlasRid = "_cachedMediaReplaceAtlasRid";

		public static readonly StringName _cachedMediaReplaceAtlasArrayRid = "_cachedMediaReplaceAtlasArrayRid";

		public static readonly StringName _cachedMediaReplaceAtlasShared = "_cachedMediaReplaceAtlasShared";

		public static readonly StringName _cachedMediaReplaceAtlasUsesTextureArray = "_cachedMediaReplaceAtlasUsesTextureArray";

		public static readonly StringName _cachedRenderMountParentSubmit = "_cachedRenderMountParentSubmit";

		public static readonly StringName _cachedCanvasLayerSubmit = "_cachedCanvasLayerSubmit";

		public static readonly StringName _cachedZIndex = "_cachedZIndex";

		public static readonly StringName _cachedEffectiveZIndex = "_cachedEffectiveZIndex";

		public static readonly StringName _effectiveZIndexCached = "_effectiveZIndexCached";

		public static readonly StringName _effectiveZIndexCacheLocalZ = "_effectiveZIndexCacheLocalZ";

		public static readonly StringName _effectiveZIndexCacheZAsRelative = "_effectiveZIndexCacheZAsRelative";

		public static readonly StringName _effectiveZIndexRescanCountdown = "_effectiveZIndexRescanCountdown";

		public static readonly StringName _effectOnceBatchRenderTransform = "_effectOnceBatchRenderTransform";

		public static readonly StringName _effectOnceBatchEffectiveZIndex = "_effectOnceBatchEffectiveZIndex";

		public static readonly StringName _effectOnceBatchRenderRevision = "_effectOnceBatchRenderRevision";

		public static readonly StringName _effectOnceBatchObservedLocalZIndex = "_effectOnceBatchObservedLocalZIndex";

		public static readonly StringName _effectOnceBatchObservedZAsRelative = "_effectOnceBatchObservedZAsRelative";

		public static readonly StringName _effectOnceBatchRenderNextAuditFrame = "_effectOnceBatchRenderNextAuditFrame";

		public static readonly StringName _effectOnceBatchRenderStateDirty = "_effectOnceBatchRenderStateDirty";

		public static readonly StringName _cachedEffectiveRenderSortBand = "_cachedEffectiveRenderSortBand";

		public static readonly StringName _effectiveRenderSortBandCached = "_effectiveRenderSortBandCached";

		public static readonly StringName _effectiveRenderSortBandCacheLocalSortBand = "_effectiveRenderSortBandCacheLocalSortBand";

		public static readonly StringName _effectiveRenderSortBandRescanCountdown = "_effectiveRenderSortBandRescanCountdown";

		public static readonly StringName _cachedEffectiveTreeOrderPath = "_cachedEffectiveTreeOrderPath";

		public static readonly StringName _treeOrderPathRescanCountdown = "_treeOrderPathRescanCountdown";

		public static readonly StringName _treeOrderWatchedParent = "_treeOrderWatchedParent";

		public static readonly StringName _renderSortBand = "_renderSortBand";

		public static readonly StringName _cachedSortBand = "_cachedSortBand";

		public static readonly StringName _cachedUseDrawOrderSortBands = "_cachedUseDrawOrderSortBands";

		public static readonly StringName _drawOrderSortBandCacheDirty = "_drawOrderSortBandCacheDirty";

		public static readonly StringName _needsDrawOrderSortBandsCached = "_needsDrawOrderSortBandsCached";

		public static readonly StringName _cachedRefreshEveryFrame = "_cachedRefreshEveryFrame";

		public static readonly StringName _cachedLayerMask = "_cachedLayerMask";

		public static readonly StringName _cachedAllLayersVisible = "_cachedAllLayersVisible";

		public static readonly StringName _cachedConfiguredLayersAllVisible = "_cachedConfiguredLayersAllVisible";

		public static readonly StringName _cachedCanUseLayerMask = "_cachedCanUseLayerMask";

		public static readonly StringName _cachedLayerVisibleCount = "_cachedLayerVisibleCount";

		public static readonly StringName _cachedLayerVisibleValueCount = "_cachedLayerVisibleValueCount";

		public static readonly StringName _cachedLayerVisibleSignature = "_cachedLayerVisibleSignature";

		public static readonly StringName _cachedRasterCompositeSequenceData = "_cachedRasterCompositeSequenceData";

		public static readonly StringName _cachedRasterCompositeSequenceClip = "_cachedRasterCompositeSequenceClip";

		public static readonly StringName _cachedRasterCompositeSequenceRange = "_cachedRasterCompositeSequenceRange";

		public static readonly StringName _cachedRasterCompositeSequenceLayerSignature = "_cachedRasterCompositeSequenceLayerSignature";

		public static readonly StringName _cachedRasterCompositeSequenceTileBase = "_cachedRasterCompositeSequenceTileBase";

		public static readonly StringName _cachedRasterCompositeSequenceClipStart = "_cachedRasterCompositeSequenceClipStart";

		public static readonly StringName _cachedRasterCompositeSequenceFrameCount = "_cachedRasterCompositeSequenceFrameCount";

		public static readonly StringName _cachedRasterCompositeSequenceValid = "_cachedRasterCompositeSequenceValid";

		public static readonly StringName _mediaReplaceStateVersion = "_mediaReplaceStateVersion";

		public static readonly StringName _cachedMediaReplaceStateVersion = "_cachedMediaReplaceStateVersion";

		public static readonly StringName _cachedMediaReplaceLimit = "_cachedMediaReplaceLimit";

		public static readonly StringName _cachedHasMediaReplace = "_cachedHasMediaReplace";

		public static readonly StringName _cachedAnyRequestedMediaReplace = "_cachedAnyRequestedMediaReplace";

		public static readonly StringName _cachedMediaReplaceUseMask = "_cachedMediaReplaceUseMask";

		public static readonly StringName _cachedMediaReplaceUseMaskOverflow = "_cachedMediaReplaceUseMaskOverflow";

		public static readonly StringName _cachedMediaReplaceAtlasPageCount = "_cachedMediaReplaceAtlasPageCount";

		public static readonly StringName _cachedMediaReplaceAtlasPageValues = "_cachedMediaReplaceAtlasPageValues";

		public static readonly StringName _cachedMediaReplaceSignature = "_cachedMediaReplaceSignature";

		public static readonly StringName _cachedMediaReplaceBoundsGrow = "_cachedMediaReplaceBoundsGrow";

		public static readonly StringName _layerStateVersion = "_layerStateVersion";

		public static readonly StringName _cachedLayerStateVersion = "_cachedLayerStateVersion";

		public static readonly StringName _runtimeManagerDispatchActive = "_runtimeManagerDispatchActive";

		public static readonly StringName _displayFrameGpuClockPoseReusable = "_displayFrameGpuClockPoseReusable";

		public static readonly StringName _runtimeDisplayVisualRequirementCached = "_runtimeDisplayVisualRequirementCached";

		public static readonly StringName _cachedRuntimeDisplayVisualRequirement = "_cachedRuntimeDisplayVisualRequirement";

		public static readonly StringName _cachedRuntimeDisplayGpuClockPoseReusable = "_cachedRuntimeDisplayGpuClockPoseReusable";

		public static readonly StringName _runtimeDisplayVisualRequirementValidationCountdown = "_runtimeDisplayVisualRequirementValidationCountdown";

		public static readonly StringName _runtimeGpuClockPoseReuseCached = "_runtimeGpuClockPoseReuseCached";

		public static readonly StringName _cachedRuntimeGpuClockPoseReusable = "_cachedRuntimeGpuClockPoseReusable";

		public static readonly StringName _runtimeGpuClockPoseReuseValidationCountdown = "_runtimeGpuClockPoseReuseValidationCountdown";

		public static readonly StringName _runtimeGpuClockInterpolationActive = "_runtimeGpuClockInterpolationActive";

		public static readonly StringName _runtimeGpuClockFramesPerSecond = "_runtimeGpuClockFramesPerSecond";

		public static readonly StringName _runtimeGpuClockClipStart = "_runtimeGpuClockClipStart";

		public static readonly StringName _runtimeGpuClockClipEndExclusive = "_runtimeGpuClockClipEndExclusive";

		public static readonly StringName _runtimeGpuClockLoop = "_runtimeGpuClockLoop";

		public static readonly StringName _hasCachedRenderSnapshot = "_hasCachedRenderSnapshot";

		public static readonly StringName _loopTerminalAliasCacheRange = "_loopTerminalAliasCacheRange";

		public static readonly StringName _loopTerminalAliasCacheHasValue = "_loopTerminalAliasCacheHasValue";

		public static readonly StringName _loopTerminalAliasCacheValue = "_loopTerminalAliasCacheValue";

		public static readonly StringName _playbackClipEndCacheRange = "_playbackClipEndCacheRange";

		public static readonly StringName _playbackClipEndCacheLoop = "_playbackClipEndCacheLoop";

		public static readonly StringName _playbackClipEndCachePlayBack = "_playbackClipEndCachePlayBack";

		public static readonly StringName _playbackClipEndCacheHasValue = "_playbackClipEndCacheHasValue";

		public static readonly StringName _playbackClipEndCacheValue = "_playbackClipEndCacheValue";

		public static readonly StringName _renderOrderDebugPending = "_renderOrderDebugPending";

		public static readonly StringName _renderOrderDebugReason = "_renderOrderDebugReason";

		public static readonly StringName _cachedOwnRenderFrameModulateVersion = "_cachedOwnRenderFrameModulateVersion";

		public static readonly StringName _cachedOwnRenderFrameModulate = "_cachedOwnRenderFrameModulate";

		public static readonly StringName _cachedOwnRenderFrameSelfModulateVersion = "_cachedOwnRenderFrameSelfModulateVersion";

		public static readonly StringName _cachedOwnRenderFrameSelfModulate = "_cachedOwnRenderFrameSelfModulate";

		public static readonly StringName _cachedRenderLocalModulateReady = "_cachedRenderLocalModulateReady";

		public static readonly StringName _cachedRenderLocalModulate = "_cachedRenderLocalModulate";

		public static readonly StringName _cachedRenderLocalSelfModulateReady = "_cachedRenderLocalSelfModulateReady";

		public static readonly StringName _cachedRenderLocalSelfModulate = "_cachedRenderLocalSelfModulate";

		public static readonly StringName _renderLocalModulateRescanCountdown = "_renderLocalModulateRescanCountdown";

		public static readonly StringName _renderLocalModulateAuditTransactionVersion = "_renderLocalModulateAuditTransactionVersion";

		public static readonly StringName _runtimeRenderBoundsCached = "_runtimeRenderBoundsCached";

		public static readonly StringName _cachedRuntimeRenderBoundsVisible = "_cachedRuntimeRenderBoundsVisible";

		public static readonly StringName _cachedRuntimeRenderBoundsStrictVisible = "_cachedRuntimeRenderBoundsStrictVisible";

		public static readonly StringName _cachedRuntimeRenderBoundsCullingEnabled = "_cachedRuntimeRenderBoundsCullingEnabled";

		public static readonly StringName _cachedRuntimeRenderBoundsTransform = "_cachedRuntimeRenderBoundsTransform";

		public static readonly StringName _cachedRuntimeRenderBoundsOffset = "_cachedRuntimeRenderBoundsOffset";

		public static readonly StringName _cachedRuntimeRenderBoundsMountParent = "_cachedRuntimeRenderBoundsMountParent";

		public static readonly StringName _cachedRuntimeRenderBoundsViewport = "_cachedRuntimeRenderBoundsViewport";

		public static readonly StringName _cachedRuntimeRenderBoundsViewportWorldRect = "_cachedRuntimeRenderBoundsViewportWorldRect";

		public static readonly StringName _cachedRuntimeRenderBoundsRelativeRect = "_cachedRuntimeRenderBoundsRelativeRect";

		public static readonly StringName _cachedRuntimeRenderBoundsHasRelativeRect = "_cachedRuntimeRenderBoundsHasRelativeRect";

		public static readonly StringName _runtimeRenderBoundsRescanCountdown = "_runtimeRenderBoundsRescanCountdown";

		public static readonly StringName _externalVisualPreparationFrame = "_externalVisualPreparationFrame";

		public static readonly StringName _externalVisualGpuStateAtlasRids = "_externalVisualGpuStateAtlasRids";

		public static readonly StringName _externalVisualGpuStateFailures = "_externalVisualGpuStateFailures";

		public static readonly StringName _staticAtlasPathLayoutRuntimeMutationSeen = "_staticAtlasPathLayoutRuntimeMutationSeen";

		public static readonly StringName _bypassStaticAtlasPathLayoutCacheForTests = "_bypassStaticAtlasPathLayoutCacheForTests";

		public static readonly StringName _staticAtlasPathLayoutGpuResetPending = "_staticAtlasPathLayoutGpuResetPending";

		public static readonly StringName _staticAtlasPathSnapshotData = "_staticAtlasPathSnapshotData";

		public static readonly StringName _staticAtlasPathSnapshotDataInstanceId = "_staticAtlasPathSnapshotDataInstanceId";

		public static readonly StringName _staticAtlasPathSnapshotAuthoringRevision = "_staticAtlasPathSnapshotAuthoringRevision";

		public static readonly StringName _staticAtlasPathSnapshotMediaCount = "_staticAtlasPathSnapshotMediaCount";

		public static readonly StringName _staticAtlasPathSnapshotTextureCount = "_staticAtlasPathSnapshotTextureCount";

		public static readonly StringName _staticAtlasPathSnapshotUseCount = "_staticAtlasPathSnapshotUseCount";

		public static readonly StringName _staticAtlasPathSnapshotPathCount = "_staticAtlasPathSnapshotPathCount";

		public static readonly StringName _staticAtlasPathSnapshotEffectiveLimit = "_staticAtlasPathSnapshotEffectiveLimit";

		public static readonly StringName _staticAtlasPathSnapshotActiveCount = "_staticAtlasPathSnapshotActiveCount";

		public static readonly StringName _staticAtlasPathSnapshotSlotSignature = "_staticAtlasPathSnapshotSlotSignature";

		public static readonly StringName _staticAtlasPathSnapshotPaths = "_staticAtlasPathSnapshotPaths";

		public static readonly StringName _staticAtlasPathSnapshotValid = "_staticAtlasPathSnapshotValid";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	public const int EmptyMediaId = 65535;

	internal const long ParentLayerSortBandStride = 1000000L;

	internal const long NestedLayerSortBandStride = 1L;

	internal const int InsertedLayerSortBandStride = 4;

	internal const long InsertedChildSortBandOffset = 1L;

	internal const long ManagedSpriteSortBandOffset = 1L;

	internal const long CharacterSortBandStride = 1000000000000L;

	private const long ShowBehindParentSortBandOffset = -1000000L;

	private const int RenderStateRescanInterval = 60;

	private const int RenderMountAuditPhysicsInterval = 60;

	private const int ManagedSlotGpuStateRescanInterval = 30;

	private const int EffectiveZIndexRescanInterval = 20;

	private const int EffectiveRenderLocalModulateRescanInterval = 60;

	private const int EffectOnceBatchNativeAuditInterval = 256;

	private const int GpuGraphCrowdStateAuditPhysicsInterval = 240;

	private const float GpuGraphNestedOwnerStateRebaseSeconds = 1f;

	private const int GpuGraphNestedOwnerStateRebaseBuckets = 60;

	private const float GpuGraphNestedOwnerPhaseTolerance = 0.001f;

	private const int EffectiveRenderModulateAncestorRescanInterval = 60;

	private const int TreeOrderFallbackRescanInterval = 60;

	private const int RuntimeRenderBoundsRescanInterval = 12;

	private const float RuntimeChildViewportPadding = 256f;

	private const float RuntimeCrowdViewportPrefetchPadding = 256f;

	private const float AtlasArrayLayerPackScale = 4096f;

	private const string ParentSpriteFollowLayerPropertyName = "Parent Sprite/Follow Layer";

	private const string ParentSpriteInsertLayerPropertyName = "Parent Sprite/Insert Layer";

	private const string ParentSpriteInsertTopLayerHint = "Top:-1";

	private const string ParentSpriteFallbackLayerHint = "0";

	private const string LegacyParentSpriteLayerPropertyName = "Layer";

	private static readonly bool CachedEditorHint = Engine.IsEditorHint();

	private const int LayerActiveCacheMaxEntries = 256;

	private static readonly System.Collections.Generic.Dictionary<LayerActiveCacheKey, bool[]> LayerActiveInClipCache = new System.Collections.Generic.Dictionary<LayerActiveCacheKey, bool[]>();

	private int _effectOnceBatchVisualRevision;

	private int _effectOnceBatchEligibilityRevision;

	private AdobeAnimateData _flashAnimeData;

	private Action _fileChangeAction;

	private bool _flashAnimeDataChangePending;

	private AdobeAnimateAtlasProfile _atlasProfileOverride;

	private bool _preview = true;

	private bool _forceLocalRender;

	private Control _renderClipControl;

	private bool _forceCpuPoseRender;

	[ExportGroup("Preset", "")]
	private bool _invisible;

	[Export(PropertyHint.None, "")]
	public bool normalAlpha;

	private Vector2 _offset = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public double offsetRotate;

	private double _timeScale = 1.0;

	[Export(PropertyHint.None, "")]
	public double trueFrameRate = 30.0;

	[Export(PropertyHint.None, "")]
	public bool refreshEveryFlame;

	[Export(PropertyHint.None, "")]
	public bool skipLastFrame = true;

	private bool _deduplicateLoopTerminalFrame = true;

	[Export(PropertyHint.None, "")]
	public bool usePos = true;

	[Export(PropertyHint.None, "")]
	public bool useRotate = true;

	[Export(PropertyHint.None, "")]
	public bool useFollowVisible;

	[ExportGroup("Animation", "")]
	[Export(PropertyHint.None, "")]
	public double blendTimeInit;

	[ExportGroup("", "")]
	private bool _pause;

	private bool _playBack;

	private bool _playbackBlocked;

	private bool _parentPlaybackStopped;

	private bool _frozenPreview;

	private bool _effectOnceGpuSuppressed;

	private int _effectOnceGpuSuppressionToken;

	private bool _applyingImmediateAnimation;

	public bool keepRenderSubmittedWhenPaused;

	private bool _runtimeViewportCullingEnabled = true;

	public bool onlyDraw;

	public double elapsedTimer;

	public double refreshTimer;

	public bool blend;

	public double blendTime;

	public double blendTimer;

	private float _blendFromFrameFloat;

	public int frameIndex;

	public double frameRate;

	public bool refreshEveryFrame;

	public bool loop = true;

	private string _clip = "";

	private ulong _playbackRevision;

	private ulong _managedPoseRevision;

	public Vector2I clipRange = Vector2I.Zero;

	public bool clipOver;

	private Array<bool> _layerVisible = new Array<bool>();

	private int _effectOnceBatchArrayExposureRevision;

	private bool _effectOnceBatchEligibilityTracking;

	public Texture2D mediaReplaceAtlas;

	public TextureLayered mediaReplaceAtlasArray;

	public Vector2 mediaReplaceAtlasArraySize;

	public bool mediaReplaceAtlasUsesTextureArray;

	public Array<Rect2> mediaReplaceRect = new Array<Rect2>();

	public Array<int> mediaReplaceAtlasPages = new Array<int>();

	private Array<Texture2D> _mediaReplace = new Array<Texture2D>();

	private Array<bool> _mediaReplaceUse = new Array<bool>();

	private Array<string> _mediaReplaceAtlasPaths = new Array<string>();

	public bool mediaReplaceAtlasShared;

	public List<AdobeAnimateTrack> track = new List<AdobeAnimateTrack>();

	public Rid canvasItem;

	public Rid meshTexture;

	public ArrayMesh mesh;

	public bool needMediaReplaceUpdate;

	private bool _canRun = true;

	public bool initClip;

	private AdobeAnimateSprite _parentSprite;

	private bool _parentSpriteResolved;

	[Export(PropertyHint.None, "")]
	public int followParentSpriteLayerId;

	[Export(PropertyHint.None, "")]
	public int insertLayerId = -1;

	private Color _meshColor = Colors.White;

	private Color _renderColorMultiplier = Colors.White;

	private AdobeAnimateGpuHitFlashState _gpuBrightFlash;

	private AdobeAnimateGpuHitFlashState _gpuWhiteFlash;

	private bool _runtimeGpuGraphActive;

	private bool _hasCachedGpuGraphCrowdState;

	private AdobeAnimateCrowdRenderState _cachedGpuGraphCrowdState;

	private ulong _cachedGpuGraphCrowdStatePhysicsFrame = 18446744073709551615uL;

	private bool _hasCachedRasterCompositeRenderState;

	private AdobeAnimateRasterCompositeRenderState _cachedRasterCompositeRenderState;

	private bool _gpuGraphCrowdStaticStateDirty;

	private bool _gpuGraphCrowdPresentationStateDirty;

	private ulong _gpuGraphCrowdStateNextAuditPhysicsFrame = 18446744073709551615uL;

	private bool _renderGrayscale;

	private readonly List<InsertedSprite> _insertedSprites = new List<InsertedSprite>();

	private bool _autoInserting;

	private AdobeAnimateSlot[] _slotChildren = System.Array.Empty<AdobeAnimateSlot>();

	private AdobeAnimateSlot[] _runtimeSlotChildren = System.Array.Empty<AdobeAnimateSlot>();

	private AdobeAnimateManagedSlotSprite[] _managedSlotSprites = System.Array.Empty<AdobeAnimateManagedSlotSprite>();

	private ulong[] _managedSlotGpuStateSignatures = System.Array.Empty<ulong>();

	private Rid[] _managedSlotGpuStateAtlasRids = System.Array.Empty<Rid>();

	private AdobeAnimateGpuManagedVisualState[] _managedSlotGpuStates = System.Array.Empty<AdobeAnimateGpuManagedVisualState>();

	private ulong[] _managedSlotGpuStateFastSignatures = System.Array.Empty<ulong>();

	private int[] _managedSlotGpuStateRescanCountdowns = System.Array.Empty<int>();

	private bool[] _managedSlotGpuStateKnown = System.Array.Empty<bool>();

	private bool[] _managedSlotGpuStateValid = System.Array.Empty<bool>();

	private Sprite2D[] _managedSlotGpuStateWatchedSprites = System.Array.Empty<Sprite2D>();

	private AdobeAnimatePart[] _managedSlotGpuStateWatchedParts = System.Array.Empty<AdobeAnimatePart>();

	private bool _runtimeSlotChildrenDirty = true;

	private bool _runtimeHasChildUpdates;

	private bool _runtimeChildUpdatesVisualOnlyDirty = true;

	private bool _runtimeChildUpdatesVisualOnlyCached;

	private bool _managedSlotSpritesDirty = true;

	private AdobeAnimateSprite[] _spriteChildren = System.Array.Empty<AdobeAnimateSprite>();

	private AdobeAnimateSlot[] _spriteChildOwnerSlots = System.Array.Empty<AdobeAnimateSlot>();

	private bool _hasChildren;

	private long _runtimeChildViewportFrame = -1L;

	private bool _runtimeChildViewportVisible = true;

	private int _lastUpdateFrame = -1;

	private bool _isVisibleInTree = true;

	private bool _runtimeInsideTree;

	private bool _processEnabled = true;

	private bool _usingRuntimeManager;

	private ulong _cachedInstanceIdForRender;

	private ulong _gpuGraphPreparedGeneration;

	private ulong _gpuGraphPreparedSignature;

	private int _gpuGraphPreparedQuadCount = 1;

	private object _preparedGpuRootCacheForRender;

	private bool _dispatchingBatchedProcess;

	private bool _displayFrameVisualDispatchActive;

	private ulong _runtimeDisplayTickVersion;

	private bool _runtimeReadyCached;

	private bool _runtimeNativeCanvasSuppressed;

	private long _runtimeNativeCanvasPublishedFrameVersion = -9223372036854775808L;

	private bool _runtimeManagerFirstFrameHandoffActive;

	private uint _runtimeNativeCanvasVisibilityLayer = 1u;

	private bool _runtimeNativeCanvasSuppressionEligibilityKnown;

	private bool _runtimeNativeCanvasSuppressionEligible;

	private const int RuntimeParentStateValidationSkipUpdates = 30;

	private AdobeAnimateSprite _runtimeParentStateSource;

	private int _runtimeParentStateValidationCountdown;

	private bool _runtimeFollowVisibilityKnown;

	private bool _runtimeFollowVisibilityValue;

	private bool _needsRenderSubmission = true;

	private bool _runtimeRenderSubmissionRetryRequested;

	private bool _runtimeGpuPrepareRequired = true;

	private static bool MissingMediaReplaceAtlasWarningPrinted;

	private CanvasItem _visibilityParentCanvas;

	private bool _selfVisibilityChangedConnected;

	private bool _parentVisibilityChangedConnected;

	private VerticalClipState _verticalClip = VerticalClipState.Disabled;

	private Node _cachedRenderMountParent;

	private Viewport _cachedViewport;

	private ulong _renderMountNextAuditPhysicsFrame;

	private readonly List<CanvasItem> _renderModulateAncestors = new List<CanvasItem>();

	private Node _renderModulateMountParent;

	private bool _renderModulateAncestorCacheReady;

	private int _renderModulateAncestorRescanCountdown;

	private int _cachedCanvasLayer;

	private AdobeAnimateRuntimeDefinition _runtimeDefinition;

	private AdobeAnimateRuntimeDefinition _cachedManagedPoseTrackDefinition;

	private int _cachedManagedPoseTrackKey = -1;

	private bool _cachedManagedPoseTrackUseLayerId;

	private PackedCpuPoseSample[] _cachedManagedPoseTrack = System.Array.Empty<PackedCpuPoseSample>();

	private double _nextAnimDelayTimer = -1.0;

	private AdobeAnimateTrack _pendingTrackConfig;

	private bool _renderStateCached;

	private bool _renderStaticStateCached;

	private bool _renderStaticLayerArrayEscaped = true;

	private bool _renderStaticMediaArraysEscaped = true;

	private bool _renderStaticArrayAuditSubmissionPending;

	private ulong _renderStaticStateNextAuditPhysicsFrame = 18446744073709551615uL;

	private Array<bool> _renderStaticAuditLayerVisibleSnapshot;

	private Array<bool> _renderStaticAuditMediaReplaceUseSnapshot;

	private Array<Texture2D> _renderStaticAuditMediaReplaceSnapshot;

	private Array<Rect2> _renderStaticAuditMediaReplaceRectSnapshot;

	private Array<int> _renderStaticAuditMediaReplacePageSnapshot;

	private Array<string> _renderStaticAuditMediaReplaceAtlasPathSnapshot;

	private AdobeAnimateGpuGraphOwnerStateTemplate _cachedGpuGraphOwnerStateTemplate;

	private ulong _cachedGpuGraphRootOwnerPlaybackRevision;

	private AdobeAnimateGpuGraphOwnerState _cachedGpuGraphRootOwnerState;

	private bool _hasCachedGpuGraphNestedOwnerState;

	private AdobeAnimateGpuGraphOwnerState _cachedGpuGraphNestedOwnerState;

	private ulong _cachedGpuGraphNestedOwnerPlaybackRevision;

	private float _gpuGraphNestedOwnerStateRebaseClockSeconds;

	private string _cachedClip = "";

	private Vector2I _cachedClipRange;

	private float _cachedFrameFloat;

	private Transform2D _cachedGlobalTransform;

	private Transform2D _renderGlobalTransform;

	private Transform2D _renderPreviousGlobalTransform;

	private bool _renderGlobalTransformCached;

	private bool _renderGlobalTransformDirty = true;

	private bool _renderTransformChangedInPhysicsFrame;

	private bool _runtimePhysicsAncestorTransformPending;

	private bool _knownAncestorTranslationNotificationPending;

	private bool _renderRootMotionEnabled;

	private bool _renderTreePaused;

	private ulong _renderGlobalTransformPhysicsFrame;

	private const float RenderMotionTeleportDistance = 96f;

	private Color _cachedModulate;

	private Vector2 _cachedOffset;

	private Texture2D _cachedMediaReplaceAtlas;

	private TextureLayered _cachedMediaReplaceAtlasArray;

	private Rid _cachedMediaReplaceAtlasRid;

	private Rid _cachedMediaReplaceAtlasArrayRid;

	private bool _cachedMediaReplaceAtlasShared;

	private bool _cachedMediaReplaceAtlasUsesTextureArray;

	private Node _cachedRenderMountParentSubmit;

	private int _cachedCanvasLayerSubmit;

	private int _cachedZIndex;

	private int _cachedEffectiveZIndex;

	private bool _effectiveZIndexCached;

	private int _effectiveZIndexCacheLocalZ;

	private bool _effectiveZIndexCacheZAsRelative;

	private int _effectiveZIndexRescanCountdown;

	private Transform2D _effectOnceBatchRenderTransform;

	private int _effectOnceBatchEffectiveZIndex;

	private int _effectOnceBatchRenderRevision;

	private int _effectOnceBatchObservedLocalZIndex = -2147483648;

	private bool _effectOnceBatchObservedZAsRelative;

	private ulong _effectOnceBatchRenderNextAuditFrame = 18446744073709551615uL;

	private bool _effectOnceBatchRenderStateDirty = true;

	private long _cachedEffectiveRenderSortBand;

	private bool _effectiveRenderSortBandCached;

	private long _effectiveRenderSortBandCacheLocalSortBand;

	private int _effectiveRenderSortBandRescanCountdown;

	private int[] _cachedEffectiveTreeOrderPath = System.Array.Empty<int>();

	private int _treeOrderPathRescanCountdown;

	private Node _treeOrderWatchedParent;

	private long _renderSortBand;

	private long _cachedSortBand;

	private bool _cachedUseDrawOrderSortBands;

	private bool _drawOrderSortBandCacheDirty = true;

	private bool _needsDrawOrderSortBandsCached;

	private bool _cachedRefreshEveryFrame;

	private ulong _cachedLayerMask;

	private bool _cachedAllLayersVisible;

	private bool _cachedConfiguredLayersAllVisible;

	private bool _cachedCanUseLayerMask;

	private int _cachedLayerVisibleCount;

	private int _cachedLayerVisibleValueCount;

	private bool[] _cachedLayerVisibleValues = System.Array.Empty<bool>();

	private ulong _cachedLayerVisibleSignature;

	private AdobeAnimateRasterCompositeData _cachedRasterCompositeSequenceData;

	private string _cachedRasterCompositeSequenceClip = "";

	private Vector2I _cachedRasterCompositeSequenceRange;

	private ulong _cachedRasterCompositeSequenceLayerSignature;

	private int _cachedRasterCompositeSequenceTileBase;

	private int _cachedRasterCompositeSequenceClipStart;

	private int _cachedRasterCompositeSequenceFrameCount;

	private bool _cachedRasterCompositeSequenceValid;

	private bool[] _cachedLayerActiveInCurrentClip = System.Array.Empty<bool>();

	private int _mediaReplaceStateVersion;

	private int _cachedMediaReplaceStateVersion;

	private int _cachedMediaReplaceLimit;

	private bool _cachedHasMediaReplace;

	private bool _cachedAnyRequestedMediaReplace;

	private ulong _cachedMediaReplaceUseMask;

	private bool _cachedMediaReplaceUseMaskOverflow;

	private int _cachedMediaReplaceAtlasPageCount;

	private bool[] _cachedMediaReplaceUseValues = System.Array.Empty<bool>();

	private Rect2[] _cachedMediaReplaceRects = System.Array.Empty<Rect2>();

	private int[] _cachedMediaReplaceAtlasPageValues = System.Array.Empty<int>();

	private ulong _cachedMediaReplaceSignature;

	private float _cachedMediaReplaceBoundsGrow;

	private VerticalClipState _cachedVerticalClip;

	private int _layerStateVersion;

	private int _cachedLayerStateVersion;

	private bool _runtimeManagerDispatchActive = true;

	private bool _displayFrameGpuClockPoseReusable;

	private const int RuntimeDisplayVisualRequirementValidationSkipUpdates = 30;

	private const int RuntimeGpuClockPoseReuseValidationSkipCalls = 8;

	private bool _runtimeDisplayVisualRequirementCached;

	private bool _cachedRuntimeDisplayVisualRequirement;

	private bool _cachedRuntimeDisplayGpuClockPoseReusable;

	private int _runtimeDisplayVisualRequirementValidationCountdown;

	private bool _runtimeGpuClockPoseReuseCached;

	private bool _cachedRuntimeGpuClockPoseReusable;

	private int _runtimeGpuClockPoseReuseValidationCountdown;

	private bool _runtimeGpuClockInterpolationActive;

	private float _runtimeGpuClockFramesPerSecond;

	private float _runtimeGpuClockClipStart;

	private float _runtimeGpuClockClipEndExclusive;

	private bool _runtimeGpuClockLoop;

	private bool _hasCachedRenderSnapshot;

	private AdobeAnimateRenderSnapshot _cachedRenderSnapshot;

	private AdobeAnimateRuntimeDefinition _loopTerminalAliasCacheDefinition;

	private Vector2I _loopTerminalAliasCacheRange;

	private bool _loopTerminalAliasCacheHasValue;

	private bool _loopTerminalAliasCacheValue;

	private AdobeAnimateRuntimeDefinition _playbackClipEndCacheDefinition;

	private Vector2I _playbackClipEndCacheRange;

	private bool _playbackClipEndCacheLoop;

	private bool _playbackClipEndCachePlayBack;

	private bool _playbackClipEndCacheHasValue;

	private int _playbackClipEndCacheValue;

	private bool _renderOrderDebugPending;

	private string _renderOrderDebugReason = "";

	private static readonly bool DebugRenderOrderTrace = false;

	private const int DebugRenderOrderTraceLimit = 800;

	private static int _debugRenderOrderTraceCount;

	private static readonly System.Collections.Generic.Dictionary<ulong, Rect2> CachedViewportWorldRects = new System.Collections.Generic.Dictionary<ulong, Rect2>();

	private static long CachedViewportWorldRectsFrame = -9223372036854775808L;

	private static int CachedViewportWorldRectRenderScopeDepth;

	private static long CachedRenderColorTransactionVersion;

	private long _cachedOwnRenderFrameModulateVersion = -9223372036854775808L;

	private Color _cachedOwnRenderFrameModulate;

	private long _cachedOwnRenderFrameSelfModulateVersion = -9223372036854775808L;

	private Color _cachedOwnRenderFrameSelfModulate;

	private bool _cachedRenderLocalModulateReady;

	private Color _cachedRenderLocalModulate;

	private bool _cachedRenderLocalSelfModulateReady;

	private Color _cachedRenderLocalSelfModulate;

	private int _renderLocalModulateRescanCountdown;

	private long _renderLocalModulateAuditTransactionVersion = -9223372036854775808L;

	private static Viewport CachedViewportWorldRectLastViewport;

	private static Rect2 CachedViewportWorldRectLastValue;

	private static readonly System.Collections.Generic.Dictionary<CanvasItem, Color> CachedRenderFrameModulates = new System.Collections.Generic.Dictionary<CanvasItem, Color>(ReferenceEqualityComparer.Instance);

	private static CanvasItem CachedRenderFrameLastModulateItem;

	private static Color CachedRenderFrameLastModulateValue;

	private bool _runtimeRenderBoundsCached;

	private bool _cachedRuntimeRenderBoundsVisible;

	private bool _cachedRuntimeRenderBoundsStrictVisible;

	private bool _cachedRuntimeRenderBoundsCullingEnabled;

	private AdobeAnimateRuntimeDefinition _cachedRuntimeRenderBoundsDefinition;

	private Transform2D _cachedRuntimeRenderBoundsTransform;

	private Vector2 _cachedRuntimeRenderBoundsOffset;

	private Node _cachedRuntimeRenderBoundsMountParent;

	private Viewport _cachedRuntimeRenderBoundsViewport;

	private Rect2 _cachedRuntimeRenderBoundsViewportWorldRect;

	private Rect2 _cachedRuntimeRenderBoundsRelativeRect;

	private bool _cachedRuntimeRenderBoundsHasRelativeRect;

	private int _runtimeRenderBoundsRescanCountdown;

	private readonly AdobeAnimateExternalVisualRegistry _externalVisuals = new AdobeAnimateExternalVisualRegistry();

	private long _externalVisualPreparationFrame = -9223372036854775808L;

	private ulong[] _externalVisualGpuStateSignatures = System.Array.Empty<ulong>();

	private ulong[] _externalVisualGpuStateVersions = System.Array.Empty<ulong>();

	private Rid[] _externalVisualGpuStateAtlasRids = System.Array.Empty<Rid>();

	private AdobeAnimateGpuManagedVisualState[] _externalVisualGpuStates = System.Array.Empty<AdobeAnimateGpuManagedVisualState>();

	private string[] _externalVisualGpuStateFailures = System.Array.Empty<string>();

	private bool[] _externalVisualGpuStateKnown = System.Array.Empty<bool>();

	private bool[] _externalVisualGpuStateValid = System.Array.Empty<bool>();

	private static readonly System.Collections.Generic.Dictionary<CanvasItem, EffectiveAncestorModulateCacheEntry> CachedRenderFrameEffectiveAncestorModulates = new System.Collections.Generic.Dictionary<CanvasItem, EffectiveAncestorModulateCacheEntry>();

	private const int StaticAtlasPathLayoutCacheCapacity = 256;

	private static readonly System.Collections.Generic.Dictionary<StaticAtlasPathLayoutKey, StaticAtlasPathLayoutCacheEntry> StaticAtlasPathLayoutCache = new System.Collections.Generic.Dictionary<StaticAtlasPathLayoutKey, StaticAtlasPathLayoutCacheEntry>();

	private static int StaticAtlasPathLayoutCacheVersion = -2147483648;

	private StaticAtlasPathLayoutCacheEntry _appliedStaticAtlasPathLayout;

	private bool _staticAtlasPathLayoutRuntimeMutationSeen;

	private bool _bypassStaticAtlasPathLayoutCacheForTests;

	private bool _staticAtlasPathLayoutGpuResetPending;

	private AdobeAnimateData _staticAtlasPathSnapshotData;

	private ulong _staticAtlasPathSnapshotDataInstanceId;

	private long _staticAtlasPathSnapshotAuthoringRevision;

	private int _staticAtlasPathSnapshotMediaCount;

	private int _staticAtlasPathSnapshotTextureCount;

	private int _staticAtlasPathSnapshotUseCount;

	private int _staticAtlasPathSnapshotPathCount;

	private int _staticAtlasPathSnapshotEffectiveLimit;

	private int _staticAtlasPathSnapshotActiveCount;

	private ulong _staticAtlasPathSnapshotSlotSignature;

	private bool[] _staticAtlasPathSnapshotActive = System.Array.Empty<bool>();

	private string[] _staticAtlasPathSnapshotPaths = System.Array.Empty<string>();

	private bool _staticAtlasPathSnapshotValid;

	private static readonly AdobeAnimateRuntimeDefinition SharedGpuClockBareTestDefinition = new AdobeAnimateRuntimeDefinition
	{
		Frames = new PackedFrame[1]
		{
			new PackedFrame(0, 0)
		},
		RuntimeLayerCount = 0
	};

	protected static bool IsEditorContext => CachedEditorHint;

	public new Color Modulate
	{
		get
		{
			return base.Modulate;
		}
		set
		{
			if (!_cachedRenderLocalModulateReady || !(_cachedRenderLocalModulate == value))
			{
				base.Modulate = value;
				_cachedRenderLocalModulate = value;
				_cachedRenderLocalModulateReady = true;
				_gpuGraphCrowdPresentationStateDirty = true;
				ScheduleNextRenderLocalModulateAudit();
				_effectOnceBatchVisualRevision++;
				MarkEffectOnceBatchEligibilityChanged();
			}
		}
	}

	public new Color SelfModulate
	{
		get
		{
			return base.SelfModulate;
		}
		set
		{
			if (!_cachedRenderLocalSelfModulateReady || !(_cachedRenderLocalSelfModulate == value))
			{
				base.SelfModulate = value;
				_cachedRenderLocalSelfModulate = value;
				_cachedRenderLocalSelfModulateReady = true;
				_gpuGraphCrowdPresentationStateDirty = true;
				ScheduleNextRenderLocalModulateAudit();
				_effectOnceBatchVisualRevision++;
				MarkEffectOnceBatchEligibilityChanged();
			}
		}
	}

	public new int ZIndex
	{
		get
		{
			return base.ZIndex;
		}
		set
		{
			if (base.ZIndex != value)
			{
				base.ZIndex = value;
				InvalidateEffectOnceBatchRenderState();
			}
		}
	}

	public new bool ZAsRelative
	{
		get
		{
			return base.ZAsRelative;
		}
		set
		{
			if (base.ZAsRelative != value)
			{
				base.ZAsRelative = value;
				InvalidateEffectOnceBatchRenderState();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public AdobeAnimateAtlasProfile atlasProfileOverride
	{
		get
		{
			return _atlasProfileOverride;
		}
		set
		{
			if (_atlasProfileOverride != value)
			{
				_atlasProfileOverride = value;
				MarkEffectOnceBatchEligibilityChanged();
				InvalidateStaticAtlasPathLayoutForNodeMutation();
				_runtimeDefinition = null;
				meshTexture = default;
				InvalidateRenderSnapshotCache(force: true);
				if (IsInsideTree())
				{
					AdobeAnimateRenderManager.InvalidateGpuRenderGraph(GetRenderSortRootForRender(), AdobeAnimateGpuGraphInvalidationReason.OwnerDefinition);
					RequestNodeRedraw();
					CanRun();
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public AdobeAnimateData flashAnimeData
	{
		get
		{
			return _flashAnimeData;
		}
		set
		{
			if (_flashAnimeData == value)
			{
				return;
			}
			InvalidateStaticAtlasPathLayoutConfigurationSnapshot();
			_managedPoseRevision++;
			if (_flashAnimeData != null && _fileChangeAction != null)
			{
				_flashAnimeData.Changed -= _fileChangeAction;
			}
			_flashAnimeData = value;
			MarkEffectOnceBatchEligibilityChanged();
			if (DeferFlashAnimeDataChangeUntilReady())
			{
				PrepareFlashAnimeDataSerializedOverrides();
				_flashAnimeDataChangePending = true;
			}
			else
			{
				ApplyFlashAnimeDataChange(invalidateDefinitionCache: false);
				InvalidateRenderSnapshotCache(force: true);
			}
			if (_flashAnimeData != null)
			{
				if (_fileChangeAction == null)
				{
					_fileChangeAction = OnFlashAnimeDataChanged;
				}
				_flashAnimeData.Changed += _fileChangeAction;
			}
			if (CachedEditorHint)
			{
				NotifyPropertyListChanged();
			}
			if (IsInsideTree())
			{
				AdobeAnimateRenderManager.InvalidateGpuRenderGraph(GetRenderSortRootForRender(), AdobeAnimateGpuGraphInvalidationReason.OwnerDefinition);
				RequestNodeRedraw();
				CanRun();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public bool preview
	{
		get
		{
			return _preview;
		}
		set
		{
			if (_preview != value)
			{
				_preview = value;
				CanRun();
				RequestNodeRedraw();
			}
		}
	}

	public bool forceLocalRender
	{
		get
		{
			return _forceLocalRender;
		}
		set
		{
			if (_forceLocalRender != value)
			{
				AdobeAnimateRenderManager.ReleaseImmediateSubmission(this);
				RestoreRuntimeNativeCanvasLayer();
				_forceLocalRender = value;
				InvalidateRenderSnapshotCache(force: true);
				if (IsInsideTree())
				{
					RefreshRenderMountCache();
					RefreshTransformNotificationModeForRenderTree();
					SetProcessEnabled(_canRun && !clipOver);
				}
				RequestNodeRedraw();
			}
		}
	}

	public bool forceCpuPoseRender
	{
		get
		{
			return _forceCpuPoseRender;
		}
		set
		{
			if (_forceCpuPoseRender != value)
			{
				_forceCpuPoseRender = value;
				InvalidateRenderSnapshotCache(force: true);
				RequestNodeRedraw();
			}
		}
	}

	public bool suppressEditorRenderSubmission { get; set; }

	[Export(PropertyHint.None, "")]
	public bool invisible
	{
		get
		{
			return _invisible;
		}
		set
		{
			if (_invisible != value)
			{
				_invisible = value;
				CanRun();
				RequestNodeRedraw();
			}
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
			if (!(_offset == value))
			{
				_offset = value;
				MarkEffectOnceBatchRenderStateDirty();
				_managedPoseRevision++;
				if (IsInsideTree())
				{
					UpdateChildPoseImmediate();
					InvalidateGpuRenderGraphForOffsetChange();
				}
				InvalidateRenderSnapshotCache();
				RequestNodeRedraw();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public double timeScale
	{
		get
		{
			return _timeScale;
		}
		set
		{
			if (!Mathf.IsEqualApprox((float)_timeScale, (float)value))
			{
				bool isPlaybackStopped = IsPlaybackStopped;
				_timeScale = value;
				_playbackRevision++;
				if (CommitPlaybackControlChange(isPlaybackStopped))
				{
					RefreshRuntimeChildPlaybackControl();
				}
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public bool deduplicateLoopTerminalFrame
	{
		get
		{
			return _deduplicateLoopTerminalFrame;
		}
		set
		{
			if (_deduplicateLoopTerminalFrame != value)
			{
				_deduplicateLoopTerminalFrame = value;
				_playbackClipEndCacheHasValue = false;
			}
		}
	}

	public bool IsFrozenPreview => _frozenPreview;

	[Export(PropertyHint.None, "")]
	public bool pause
	{
		get
		{
			return _pause;
		}
		set
		{
			if (_pause != value)
			{
				bool isPlaybackStopped = IsPlaybackStopped;
				_pause = value;
				CommitPlaybackControlChange(isPlaybackStopped);
				RefreshRuntimeChildPlaybackControl();
			}
		}
	}

	private bool IsPlaybackStopped
	{
		get
		{
			if (!_pause && !_playbackBlocked && !_parentPlaybackStopped)
			{
				return Math.Abs(_timeScale) <= 1E-06;
			}
			return true;
		}
	}

	internal bool IsEffectOnceGpuSuppressed => _effectOnceGpuSuppressed;

	[Export(PropertyHint.None, "")]
	public bool runtimeViewportCullingEnabled
	{
		get
		{
			return _runtimeViewportCullingEnabled;
		}
		set
		{
			if (_runtimeViewportCullingEnabled != value)
			{
				_runtimeViewportCullingEnabled = value;
				InvalidateRenderSnapshotCache(force: true);
				RequestNodeRedraw();
			}
		}
	}

	[Export(PropertyHint.None, "")]
	public bool playBack
	{
		get
		{
			return _playBack;
		}
		set
		{
			if (_playBack != value)
			{
				_playBack = value;
				_playbackRevision++;
				CommitPlaybackDirectionChange();
			}
		}
	}

	internal ulong ManagedPoseRevision => _managedPoseRevision;

	[Export(PropertyHint.None, "")]
	public string clip
	{
		get
		{
			return _clip;
		}
		set
		{
			SetClip(value);
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<bool> layerVisible
	{
		get
		{
			MarkEffectOnceBatchArrayExposure();
			MarkRenderStaticLayerArrayExposure();
			return _layerVisible;
		}
		set
		{
			MarkEffectOnceBatchArrayExposure();
			_layerVisible = value ?? new Array<bool>();
			MarkRenderStaticLayerArrayExposure();
			MarkLayerStateChanged();
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<Texture2D> mediaReplace
	{
		get
		{
			MarkRenderStaticMediaArrayExposure();
			return _mediaReplace;
		}
		set
		{
			_mediaReplace = value ?? new Array<Texture2D>();
			MarkRenderStaticMediaArrayExposure();
			CaptureStaticAtlasPathLayoutConfigurationSnapshot();
			QueueUpdateMediaReplace();
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<bool> mediaReplaceUse
	{
		get
		{
			MarkEffectOnceBatchArrayExposure();
			MarkRenderStaticMediaArrayExposure();
			return _mediaReplaceUse;
		}
		set
		{
			MarkEffectOnceBatchArrayExposure();
			_mediaReplaceUse = value ?? new Array<bool>();
			MarkRenderStaticMediaArrayExposure();
			MarkEffectOnceBatchEligibilityChanged();
			CaptureStaticAtlasPathLayoutConfigurationSnapshot();
			QueueUpdateMediaReplace();
		}
	}

	[Export(PropertyHint.None, "")]
	public Array<string> mediaReplaceAtlasPaths
	{
		get
		{
			MarkRenderStaticMediaArrayExposure();
			return _mediaReplaceAtlasPaths;
		}
		set
		{
			_mediaReplaceAtlasPaths = value ?? new Array<string>();
			MarkRenderStaticMediaArrayExposure();
			if (_mediaReplaceUse.Count < _mediaReplaceAtlasPaths.Count)
			{
				_mediaReplaceUse.Resize(_mediaReplaceAtlasPaths.Count);
			}
			for (int i = 0; i < _mediaReplaceAtlasPaths.Count; i++)
			{
				if (!string.IsNullOrWhiteSpace(_mediaReplaceAtlasPaths[i]))
				{
					_mediaReplaceUse[i] = true;
				}
			}
			CaptureStaticAtlasPathLayoutConfigurationSnapshot();
			InvalidateStaticAtlasPathLayoutForNodeMutation();
			QueueUpdateMediaReplace();
		}
	}

	[Export(PropertyHint.None, "")]
	public AdobeAnimateSprite parentSprite
	{
		get
		{
			return _parentSprite;
		}
		set
		{
			_parentSpriteResolved = true;
			if (_parentSprite == value)
			{
				if (!CachedEditorHint)
				{
					SetParentPlaybackStopped(GodotObject.IsInstanceValid(_parentSprite) && _parentSprite.IsPlaybackStopped);
				}
				return;
			}
			AdobeAnimateSprite adobeAnimateSprite = _parentSprite;
			_parentSprite = value;
			if (!CachedEditorHint)
			{
				SetParentPlaybackStopped(GodotObject.IsInstanceValid(_parentSprite) && _parentSprite.IsPlaybackStopped);
			}
			RefreshTransformNotificationMode();
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.OnSpriteChildParentChanged(this);
			}
			if (GodotObject.IsInstanceValid(_parentSprite))
			{
				_parentSprite.MarkManagedSlotSpriteCacheDirty();
			}
			AdobeAnimateRuntimeManager.InvalidateRenderRoots();
			InvalidateTreeOrderPathCache();
			InvalidateRenderSnapshotCache();
			RequestNodeRedraw();
			NotifyPropertyListChanged();
		}
	}

	[Export(PropertyHint.None, "")]
	public Color meshColor
	{
		get
		{
			return _meshColor;
		}
		set
		{
			if (!(_meshColor == value))
			{
				_meshColor = value;
				ApplyComposedModulate();
				_needsRenderSubmission = true;
				if (IsNodeReady())
				{
					SetMultimeshModulate(this);
					RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, staticStateDirty: false);
				}
			}
		}
	}

	internal bool IsRuntimeActive
	{
		get
		{
			if (!_effectOnceGpuSuppressed && _canRun)
			{
				return _processEnabled;
			}
			return false;
		}
	}

	internal bool IsRuntimeTickPaused
	{
		get
		{
			if (!IsPlaybackStopped)
			{
				return _effectOnceGpuSuppressed;
			}
			return true;
		}
	}

	internal bool IsRuntimeInsideTree => _runtimeInsideTree;

	internal bool IsRuntimeVisibleInTree => _isVisibleInTree;

	public bool IsRuntimeReadyCached => _runtimeReadyCached;

	internal bool IsRuntimeDisplayTickActive
	{
		get
		{
			if (_usingRuntimeManager && !_effectOnceGpuSuppressed && _canRun && _processEnabled)
			{
				return !IsPlaybackStopped;
			}
			return false;
		}
	}

	internal bool NeedsRuntimeRenderSubmission
	{
		get
		{
			if (!_effectOnceGpuSuppressed)
			{
				return _needsRenderSubmission;
			}
			return false;
		}
	}

	internal bool UsesRuntimeGpuClockInterpolation => _runtimeGpuClockInterpolationActive;

	internal bool PrefersCachedGpuGraphCrowdRenderState
	{
		get
		{
			if (_runtimeGpuGraphActive && _hasCachedGpuGraphCrowdState)
			{
				return _cachedGpuGraphCrowdState.Mode == AdobeAnimateCrowdRenderMode.GpuGraph;
			}
			return false;
		}
	}

	internal bool RuntimeManagerDispatchActive => _runtimeManagerDispatchActive;

	protected bool ShouldDispatchProcessCallback
	{
		get
		{
			if (_usingRuntimeManager)
			{
				return _dispatchingBatchedProcess;
			}
			return true;
		}
	}

	protected virtual bool RequiresDisplayFrameBatchedProcessExtension => false;

	internal static int StaticAtlasPathLayoutCacheEntryCountForTests => StaticAtlasPathLayoutCache.Count;

	internal bool NeedsRenderSubmissionForBareTest => _needsRenderSubmission;

	internal ulong PlaybackRevisionForBareTest => _playbackRevision;

	public event AnimeCompletedEventHandler OnAnimeCompleted;

	public event AnimeStopedEventHandler OnAnimeStoped;

	public event AnimeStartedEventHandler OnAnimeStarted;

	public event AnimeEventEventHandler OnAnimeEvent;

	public event AnimeBlendCompletedEventHandler OnAnimeBlendCompleted;

	internal event Action EffectOnceBatchEligibilityChanged;

	internal event Action EffectOnceBatchRenderStateChanged;

	private AdobeAnimateCrowdRenderStateResult TryBuildCompositeCrowdRenderState(AdobeAnimateRuntimeDefinition definition, Node renderMountParent, Transform2D globalTransform, out AdobeAnimateCrowdRenderState state)
	{
		state = null;
		if (_effectOnceGpuSuppressed)
		{
			return AdobeAnimateCrowdRenderStateResult.Skipped;
		}
		Transform2D transform2D = AdobeAnimateRenderManager.ToRenderMountLocalTransform(renderMountParent, globalTransform);
		ResolveRenderPresentationForRender(renderMountParent, out var modulate, out var effectiveZIndex);
		bool needsDrawItemSort = NeedsDrawItemSortForRender();
		AdobeAnimateRenderSnapshot snapshot = new AdobeAnimateRenderSnapshot(this, _flashAnimeData, definition, _clip, clipRange, GetRenderFrameFloat(), transform2D, offset, modulate, _layerVisible, _cachedAllLayersVisible, _cachedCanUseLayerMask, _cachedLayerMask, _cachedLayerVisibleValueCount, _cachedLayerVisibleValues, mediaReplaceAtlas, mediaReplaceRect, _mediaReplaceUse, _cachedHasMediaReplace, _cachedMediaReplaceLimit, _cachedMediaReplaceUseMask, _cachedMediaReplaceUseMaskOverflow, mediaReplaceAtlasPages, _cachedMediaReplaceAtlasPageCount, _cachedMediaReplaceUseValues, _cachedMediaReplaceRects, _cachedMediaReplaceAtlasPageValues, mediaReplaceAtlasArray, mediaReplaceAtlasArraySize, mediaReplaceAtlasUsesTextureArray, _verticalClip, renderMountParent, _cachedCanvasLayer, effectiveZIndex, System.Array.Empty<int>(), needsDrawItemSort, GetClipBlendStateForRender());
		_needsRenderSubmission = false;
		if (!AdobeAnimateRenderManager.TryResolveSnapshotAtlas(snapshot, out var atlasArray, out var atlasSize))
		{
			return AdobeAnimateCrowdRenderStateResult.Skipped;
		}
		TextureLayered textureLayered = (CanUseCrowdShaderPoseDefinition(definition) ? definition.GpuPoseTextureArray : null);
		Vector2I poseTextureSize = (GodotObject.IsInstanceValid(textureLayered) ? definition.GpuPoseTextureSize : Vector2I.Zero);
		state = AdobeAnimateCrowdRenderState.CreateComposite(snapshot, atlasArray, atlasSize, textureLayered, poseTextureSize, BuildRootMotionStateForRender(renderMountParent, transform2D));
		return AdobeAnimateCrowdRenderStateResult.Submitted;
	}

	private bool CanUseCrowdShaderPoseDefinition(AdobeAnimateRuntimeDefinition definition)
	{
		if (definition != null && definition.Frames != null && definition.Frames.Length != 0 && definition.SliceMetadata != null && definition.UsesGpuPoseTextureArray && definition.GpuPoseTextureRid.IsValid && definition.GpuPoseTextureSize.X > 0 && definition.GpuPoseTextureSize.Y > 0)
		{
			return GodotObject.IsInstanceValid(definition.GpuPoseTextureArray);
		}
		return false;
	}

	private int ResolveCrowdFrameIndex(AdobeAnimateRuntimeDefinition definition, float frameFloat)
	{
		if (definition == null || definition.Frames == null || definition.Frames.Length == 0)
		{
			return 0;
		}
		if (clipRange != Vector2I.Zero)
		{
			frameFloat = Mathf.Clamp(frameFloat, clipRange.X, Math.Max(clipRange.X, clipRange.Y - 1));
		}
		return Mathf.Clamp(Mathf.FloorToInt(frameFloat), 0, definition.Frames.Length - 1);
	}

	private static float ResolveCrowdFrameInterpolation(float frameFloat)
	{
		return Mathf.Clamp(frameFloat - Mathf.Floor(frameFloat), 0f, 1f);
	}

	internal bool TryCollectCompositeCacheOwnersForRender(List<AdobeAnimateSprite> output)
	{
		if (output == null)
		{
			return false;
		}
		return TryCollectCompositeCacheOwnersForRender(output, 0);
	}

	private bool TryCollectCompositeCacheOwnersForRender(List<AdobeAnimateSprite> output, int depth)
	{
		if (depth > 32 || !GodotObject.IsInstanceValid(this))
		{
			return false;
		}
		if (_effectOnceGpuSuppressed)
		{
			return false;
		}
		if (_flashAnimeData == null || !_isVisibleInTree)
		{
			return true;
		}
		if (output.Contains(this))
		{
			return false;
		}
		if (HasManagedSlotSpritesForRender())
		{
			return false;
		}
		output.Add(this);
		AdobeAnimateSprite[] spriteChildrenForRender = GetSpriteChildrenForRender();
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildrenForRender)
		{
			if (!GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				return false;
			}
			if (!adobeAnimateSprite.TryCollectCompositeCacheOwnersForRender(output, depth + 1))
			{
				return false;
			}
		}
		return true;
	}

	internal bool TryBuildCompositePoseRefreshState(Node renderMountParent, out AdobeAnimateCompositePoseRefreshState state)
	{
		state = default;
		if (_effectOnceGpuSuppressed || _flashAnimeData == null || !_isVisibleInTree)
		{
			return false;
		}
		if (needMediaReplaceUpdate)
		{
			UpdateMediaReplaceData();
		}
		RefreshRenderStaticStateCache();
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (!CanUseCrowdShaderPoseDefinition(runtimeDefinition))
		{
			return false;
		}
		float renderFrameFloat = GetRenderFrameFloat();
		int num = ResolveCrowdFrameIndex(runtimeDefinition, renderFrameFloat);
		PackedFrame packedFrame = runtimeDefinition.Frames[num];
		ulong frameLayoutSignature = ((runtimeDefinition.FrameLayoutSignatures != null && num < runtimeDefinition.FrameLayoutSignatures.Length) ? runtimeDefinition.FrameLayoutSignatures[num] : (((ulong)(uint)packedFrame.Offset << 32) | (uint)packedFrame.Count));
		Transform2D globalTransform = AdobeAnimateRenderManager.ToRenderMountLocalTransform(renderMountParent, GetCachedGlobalTransformForRender());
		ResolveRenderPresentationForRender(renderMountParent, out var modulate, out var _);
		state = new AdobeAnimateCompositePoseRefreshState(runtimeDefinition, globalTransform, modulate, offset, _verticalClip, packedFrame.Offset, packedFrame.Count, runtimeDefinition.GpuPoseTextureBaseTexel, runtimeDefinition.GpuPoseTextureLayer, ResolveCrowdFrameInterpolation(renderFrameFloat), frameLayoutSignature, BuildCompositeCacheStaticSignature());
		return true;
	}

	private ulong BuildCompositeCacheStaticSignature()
	{
		ulong value = 1469598103934665603uL;
		Add(ref value, GetCachedInstanceIdForRender());
		Add(ref value, GodotObject.IsInstanceValid(_flashAnimeData) ? _flashAnimeData.GetInstanceId() : 0);
		Add(ref value, (uint)_layerStateVersion);
		Add(ref value, (uint)_mediaReplaceStateVersion);
		Add(ref value, _cachedLayerVisibleSignature);
		Add(ref value, _cachedMediaReplaceSignature);
		Add(ref value, (uint)StringComparer.Ordinal.GetHashCode(_clip ?? ""));
		Add(ref value, (uint)insertLayerId);
		Add(ref value, (uint)followParentSpriteLayerId);
		Add(ref value, (ulong)_spriteChildren.Length);
		for (int i = 0; i < _spriteChildren.Length; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _spriteChildren[i];
			AdobeAnimateSlot adobeAnimateSlot = ((i < _spriteChildOwnerSlots.Length) ? _spriteChildOwnerSlots[i] : null);
			Add(ref value, GodotObject.IsInstanceValid(adobeAnimateSprite) ? adobeAnimateSprite.GetInstanceId() : 0);
			Add(ref value, GodotObject.IsInstanceValid(adobeAnimateSlot) ? adobeAnimateSlot.GetInstanceId() : 0);
			Add(ref value, (ulong)((GodotObject.IsInstanceValid(adobeAnimateSprite) && adobeAnimateSprite._isVisibleInTree) ? 1 : 0));
			Add(ref value, GodotObject.IsInstanceValid(adobeAnimateSprite) ? ((uint)adobeAnimateSprite.insertLayerId) : 0);
			Add(ref value, GodotObject.IsInstanceValid(adobeAnimateSprite) ? ((uint)adobeAnimateSprite.followParentSpriteLayerId) : 0);
		}
		return value;
		static void Add(ref ulong reference, ulong component)
		{
			reference ^= component;
			reference *= 1099511628211uL;
		}
	}

	internal bool CanSkipCrowdRenderFromCachedCulling()
	{
		if (_effectOnceGpuSuppressed || !_usingRuntimeManager || _flashAnimeData == null || !_isVisibleInTree || !_runtimeRenderBoundsCached || _cachedRuntimeRenderBoundsVisible || _runtimeRenderBoundsRescanCountdown <= 0 || _renderGlobalTransformDirty || !_renderGlobalTransformCached || _cachedRuntimeRenderBoundsTransform != _renderGlobalTransform || _cachedRuntimeRenderBoundsOffset != offset || _cachedRuntimeRenderBoundsDefinition != _runtimeDefinition || _cachedRuntimeRenderBoundsMountParent != _cachedRenderMountParent || HasRuntimeCrowdViewportChanged())
		{
			return false;
		}
		_runtimeRenderBoundsRescanCountdown--;
		return true;
	}

	internal bool TryGetCachedOffscreenCrowdWarmupPlacement(out Node renderMountParent, out int effectiveZIndex)
	{
		renderMountParent = null;
		effectiveZIndex = 0;
		if (_effectOnceGpuSuppressed || !_usingRuntimeManager || !_isVisibleInTree || !IsInsideTree() || !_runtimeRenderBoundsCached || _cachedRuntimeRenderBoundsVisible || !GodotObject.IsInstanceValid(_cachedRenderMountParent))
		{
			return false;
		}
		renderMountParent = _cachedRenderMountParent;
		effectiveZIndex = GetEffectiveZIndexForRender();
		return true;
	}

	internal AdobeAnimateCrowdRenderStateResult TryBuildCrowdRenderState(out AdobeAnimateCrowdRenderState state)
	{
		return TryBuildCrowdRenderState((float)AdobeAnimateRuntimeManager.AnimationClockSeconds, out state);
	}

	internal AdobeAnimateCrowdRenderStateResult TryBuildCrowdRenderState(float animationClockSeconds, out AdobeAnimateCrowdRenderState state)
	{
		state = null;
		bool prefersCachedGpuGraphCrowdRenderState = PrefersCachedGpuGraphCrowdRenderState;
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		bool num = prefersCachedGpuGraphCrowdRenderState && TryReuseGpuGraphCrowdRenderStateForCurrentPhysicsFrame(out state);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectStateSamePhysicsFrame, startBytes);
		if (num)
		{
			return AdobeAnimateCrowdRenderStateResult.Submitted;
		}
		long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
		AdobeAnimateCrowdRenderStateResult result = TryBuildCrowdRenderStateCore(out state, cpuDrivenPose: false, animationClockSeconds, prefersCachedGpuGraphCrowdRenderState);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectStateBuild, startBytes2);
		return result;
	}

	internal bool TryReuseGpuGraphCrowdRenderStateForCurrentPhysicsFrame(out AdobeAnimateCrowdRenderState state)
	{
		state = null;
		ulong physicsFrameForRender = AdobeAnimateRenderManager.GetPhysicsFrameForRender();
		if (!_hasCachedGpuGraphCrowdState || !_runtimeGpuGraphActive || _cachedGpuGraphCrowdStatePhysicsFrame != physicsFrameForRender || _cachedGpuGraphCrowdState.Mode != AdobeAnimateCrowdRenderMode.GpuGraph || _needsRenderSubmission || _gpuGraphCrowdStaticStateDirty || _renderGlobalTransformDirty || _renderRootMotionEnabled || needMediaReplaceUpdate || !_runtimeGpuClockInterpolationActive || IsClipBlendActiveForRender() || !AdobeAnimateRenderManager.IsGpuGraphStateCurrent(this, _cachedGpuGraphCrowdState.GpuGraphAllocation.Signature) || !AdobeAnimateRenderManager.TryRefreshGpuGraphStateAtlasBinding(_cachedGpuGraphCrowdState))
		{
			return false;
		}
		state = _cachedGpuGraphCrowdState;
		return true;
	}

	internal bool TryBuildRasterCompositeRenderState(out AdobeAnimateRasterCompositeRenderState state)
	{
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		state = default;
		_runtimeGpuGraphActive = false;
		if (_effectOnceGpuSuppressed || !_usingRuntimeManager || CachedEditorHint || _flashAnimeData == null || !_isVisibleInTree || !AdobeAnimateRenderManager.RasterCompositeEnabled)
		{
			return false;
		}
		if (TryReuseRasterCompositeRenderState(out state))
		{
			TowerDefensePerfProfiler.End("adobeAnimate.render.crowdState.rasterCompactCached", startTicks, 1);
			return true;
		}
		AdobeAnimateRasterCompositeData rasterCompositeData = _flashAnimeData.rasterCompositeData;
		if (!GodotObject.IsInstanceValid(rasterCompositeData))
		{
			return false;
		}
		_hasCachedGpuGraphCrowdState = false;
		_cachedGpuGraphCrowdState = null;
		_gpuGraphCrowdStaticStateDirty = false;
		if (needMediaReplaceUpdate)
		{
			UpdateMediaReplaceData();
		}
		RefreshRenderStaticStateCache();
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (!HasRenderableAtlas(runtimeDefinition))
		{
			return false;
		}
		Node cachedRenderMountParent = GetCachedRenderMountParent();
		Transform2D cachedGlobalTransformForRender = GetCachedGlobalTransformForRender();
		if (!IsRuntimeCrowdRenderBoundsVisible(runtimeDefinition, cachedGlobalTransformForRender, cachedRenderMountParent) || !TryBuildRasterCompositeRenderStateCore(runtimeDefinition, rasterCompositeData, cachedRenderMountParent, cachedGlobalTransformForRender, out state))
		{
			return false;
		}
		_cachedRasterCompositeRenderState = state;
		_hasCachedRasterCompositeRenderState = true;
		TowerDefensePerfProfiler.End("adobeAnimate.render.crowdState.rasterCompact", startTicks, 1);
		return true;
	}

	private bool TryReuseRasterCompositeRenderState(out AdobeAnimateRasterCompositeRenderState state)
	{
		state = default;
		if (!_hasCachedRasterCompositeRenderState || needMediaReplaceUpdate || IsClipBlendActiveForRender() || RefreshRenderStaticStateCache())
		{
			return false;
		}
		AdobeAnimateRasterCompositeData rasterCompositeData = _cachedRasterCompositeRenderState.RasterCompositeData;
		if (rasterCompositeData != _flashAnimeData?.rasterCompositeData || !TryResolveRasterCompositeTile(rasterCompositeData, GetRenderFrameFloat(), out var tileIndex))
		{
			return false;
		}
		Node cachedRenderMountParent = GetCachedRenderMountParent();
		if (cachedRenderMountParent != _cachedRasterCompositeRenderState.RenderMountParent)
		{
			return false;
		}
		AdobeAnimateRuntimeDefinition definition = _cachedRasterCompositeRenderState.Definition;
		Transform2D cachedGlobalTransformForRender = GetCachedGlobalTransformForRender();
		if (!IsRuntimeCrowdRenderBoundsVisible(definition, cachedGlobalTransformForRender, cachedRenderMountParent))
		{
			return false;
		}
		Color modulate = GetEffectiveRenderModulate(cachedRenderMountParent);
		int effectiveZIndexForRender = GetEffectiveZIndexForRender();
		Transform2D globalTransform = AdobeAnimateRenderManager.ToRenderMountLocalTransform(cachedRenderMountParent, cachedGlobalTransformForRender);
		state = _cachedRasterCompositeRenderState.WithDynamicState(in globalTransform, in modulate, effectiveZIndexForRender, tileIndex);
		_cachedRasterCompositeRenderState = state;
		_needsRenderSubmission = false;
		return true;
	}

	internal AdobeAnimateCrowdRenderStateResult TryBuildCpuPoseCrowdRenderState(out AdobeAnimateCrowdRenderState state)
	{
		return TryBuildCpuPoseCrowdRenderState((float)AdobeAnimateRuntimeManager.AnimationClockSeconds, out state);
	}

	internal AdobeAnimateCrowdRenderStateResult TryBuildCpuPoseCrowdRenderState(float animationClockSeconds, out AdobeAnimateCrowdRenderState state)
	{
		return TryBuildCrowdRenderStateCore(out state, cpuDrivenPose: true, animationClockSeconds, preferCachedGpuGraph: false);
	}

	private AdobeAnimateCrowdRenderStateResult TryBuildCrowdRenderStateCore(out AdobeAnimateCrowdRenderState state, bool cpuDrivenPose, float animationClockSeconds, bool preferCachedGpuGraph)
	{
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		state = null;
		_runtimeGpuGraphActive = false;
		if (_effectOnceGpuSuppressed || !_usingRuntimeManager || CachedEditorHint)
		{
			TowerDefensePerfProfiler.End("adobeAnimate.render.crowdState.unsupported", startTicks);
			return AdobeAnimateCrowdRenderStateResult.NotSupported;
		}
		if (_flashAnimeData == null || !_isVisibleInTree)
		{
			TowerDefensePerfProfiler.End("adobeAnimate.render.crowdState.skip", startTicks);
			return AdobeAnimateCrowdRenderStateResult.Skipped;
		}
		bool flag = !preferCachedGpuGraph && AdobeAnimateRenderManager.RasterCompositeEnabled && GodotObject.IsInstanceValid(_flashAnimeData.rasterCompositeData);
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		bool num = !cpuDrivenPose && !flag && TryReuseGpuGraphCrowdRenderState(animationClockSeconds, out state);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectStateGpuReuse, startBytes);
		if (num)
		{
			_runtimeGpuGraphActive = true;
			TowerDefensePerfProfiler.End("adobeAnimate.render.crowdState.gpuGraphCached", startTicks, 1);
			return AdobeAnimateCrowdRenderStateResult.Submitted;
		}
		AdobeAnimateCrowdRenderState cachedGpuGraphCrowdState = _cachedGpuGraphCrowdState;
		AdobeAnimateCrowdRenderState reusableState = ((cachedGpuGraphCrowdState != null && cachedGpuGraphCrowdState.Mode == AdobeAnimateCrowdRenderMode.GpuGraph) ? _cachedGpuGraphCrowdState : null);
		_hasCachedGpuGraphCrowdState = false;
		_cachedGpuGraphCrowdState = null;
		_gpuGraphCrowdStaticStateDirty = false;
		if (needMediaReplaceUpdate)
		{
			UpdateMediaReplaceData();
		}
		if (RefreshRenderStaticStateCache())
		{
			_runtimeGpuPrepareRequired = true;
		}
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (!HasRenderableAtlas(runtimeDefinition))
		{
			TowerDefensePerfProfiler.End("adobeAnimate.render.crowdState.noDefinition", startTicks);
			return AdobeAnimateCrowdRenderStateResult.Skipped;
		}
		Node cachedRenderMountParent = GetCachedRenderMountParent();
		Transform2D cachedGlobalTransformForRender = GetCachedGlobalTransformForRender();
		if (!IsRuntimeCrowdRenderBoundsVisible(runtimeDefinition, cachedGlobalTransformForRender, cachedRenderMountParent))
		{
			TowerDefensePerfProfiler.End("adobeAnimate.render.crowdState.culled", startTicks);
			return AdobeAnimateCrowdRenderStateResult.Skipped;
		}
		if (flag)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.raster.candidate", 1);
			if (TryBuildRasterCompositeCrowdRenderState(runtimeDefinition, cachedRenderMountParent, cachedGlobalTransformForRender, out state))
			{
				TowerDefensePerfProfiler.End("adobeAnimate.render.crowdState.rasterComposite", startTicks, 1);
				return AdobeAnimateCrowdRenderStateResult.Submitted;
			}
		}
		if (AdobeAnimateRenderManager.GpuRenderGraphEnabled && CanUseCrowdShaderPoseDefinition(runtimeDefinition) && (_cachedAllLayersVisible || _cachedCanUseLayerMask || _cachedLayerVisibleCount > 0) && CanUseGpuGraphMediaReplace(runtimeDefinition))
		{
			long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
			bool flag2 = TryBuildGpuGraphCrowdRenderState(runtimeDefinition, cachedRenderMountParent, cachedGlobalTransformForRender, enableGpuClock: true, animationClockSeconds, reusableState, out state);
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectStateGpuBuild, startBytes2);
			if (flag2)
			{
				_runtimeGpuGraphActive = true;
				_cachedGpuGraphCrowdState = state;
				_hasCachedGpuGraphCrowdState = true;
				_cachedGpuGraphCrowdStatePhysicsFrame = AdobeAnimateRenderManager.GetPhysicsFrameForRender();
				_cachedGpuGraphRootOwnerPlaybackRevision = _playbackRevision;
				_gpuGraphCrowdPresentationStateDirty = false;
				ScheduleNextGpuGraphCrowdStateAudit();
				TowerDefensePerfProfiler.End(cpuDrivenPose ? "adobeAnimate.render.crowdState.cpuDrivenGraph" : "adobeAnimate.render.crowdState.gpuGraph", startTicks, 1);
				return AdobeAnimateCrowdRenderStateResult.Submitted;
			}
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphFallback", 1);
		}
		bool flag3 = IsClipBlendActiveForRender();
		DisableRuntimeGpuClockInterpolationForOwnedGraph();
		if (!CanUseCrowdShaderPoseDefinition(runtimeDefinition) || flag3 || RequiresInternalDrawItemSortForRender() || !_cachedAllLayersVisible || _cachedHasMediaReplace || _cachedAnyRequestedMediaReplace || runtimeDefinition.MaxFrameSliceCount <= 0 || (mediaReplaceAtlasUsesTextureArray && !GodotObject.IsInstanceValid(mediaReplaceAtlasArray)))
		{
			AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult = TryBuildCompositeCrowdRenderState(runtimeDefinition, cachedRenderMountParent, cachedGlobalTransformForRender, out state);
			TowerDefensePerfProfiler.End((adobeAnimateCrowdRenderStateResult == AdobeAnimateCrowdRenderStateResult.Submitted) ? "adobeAnimate.render.crowdState.composite" : "adobeAnimate.render.crowdState.compositeSkip", startTicks, (adobeAnimateCrowdRenderStateResult == AdobeAnimateCrowdRenderStateResult.Submitted) ? 1 : 0);
			return adobeAnimateCrowdRenderStateResult;
		}
		Transform2D transform2D = AdobeAnimateRenderManager.ToRenderMountLocalTransform(cachedRenderMountParent, cachedGlobalTransformForRender);
		ResolveRenderPresentationForRender(cachedRenderMountParent, out var modulate, out var effectiveZIndex);
		float renderFrameFloat = GetRenderFrameFloat();
		int num2 = ResolveCrowdFrameIndex(runtimeDefinition, renderFrameFloat);
		PackedFrame packedFrame = runtimeDefinition.Frames[num2];
		if (packedFrame.Count <= 0)
		{
			TowerDefensePerfProfiler.End("adobeAnimate.render.crowdState.skip", startTicks);
			return AdobeAnimateCrowdRenderStateResult.Skipped;
		}
		ref AdobeAnimateCrowdRenderState reference = ref state;
		AdobeAnimateRuntimeDefinition definition = runtimeDefinition;
		TextureLayered atlasArray = (_cachedHasMediaReplace ? mediaReplaceAtlasArray : runtimeDefinition.AtlasTextureArray);
		Vector2 atlasArraySize;
		if (_cachedHasMediaReplace && mediaReplaceAtlasArraySize.X > 0f && mediaReplaceAtlasArraySize.Y > 0f)
		{
			atlasArraySize = mediaReplaceAtlasArraySize;
		}
		else
		{
			atlasArraySize = ((runtimeDefinition.AtlasTextureArraySize.X > 0f && runtimeDefinition.AtlasTextureArraySize.Y > 0f) ? runtimeDefinition.AtlasTextureArraySize : Vector2.One);
		}
		reference = new AdobeAnimateCrowdRenderState(definition, atlasArray, atlasArraySize, runtimeDefinition.GpuPoseTextureArray, runtimeDefinition.GpuPoseTextureSize, cachedRenderMountParent, transform2D, modulate, offset, _verticalClip, packedFrame.Offset, packedFrame.Count, runtimeDefinition.GpuPoseTextureBaseTexel, runtimeDefinition.GpuPoseTextureLayer, ResolveCrowdFrameInterpolation(renderFrameFloat), _cachedAllLayersVisible, _cachedCanUseLayerMask, _cachedLayerMask, _layerVisible, effectiveZIndex, _cachedHasMediaReplace, mediaReplaceRect, _mediaReplaceUse, mediaReplaceAtlasPages, mediaReplaceAtlasArraySize, BuildRootMotionStateForRender(cachedRenderMountParent, transform2D));
		TowerDefensePerfProfiler.End("adobeAnimate.render.crowdState.build", startTicks, 1);
		return AdobeAnimateCrowdRenderStateResult.Submitted;
	}

	private bool TryBuildRasterCompositeCrowdRenderState(AdobeAnimateRuntimeDefinition definition, Node renderMountParent, Transform2D globalTransform, out AdobeAnimateCrowdRenderState state)
	{
		state = null;
		AdobeAnimateRasterCompositeData raster = _flashAnimeData?.rasterCompositeData;
		if (!TryBuildRasterCompositeRenderStateCore(definition, raster, renderMountParent, globalTransform, out var state2))
		{
			return false;
		}
		state = AdobeAnimateCrowdRenderState.CreateRasterComposite(state2.Definition, state2.RenderMountParent, state2.GlobalTransform, state2.Modulate, state2.EffectiveZIndex, state2.VerticalClip, state2.RasterCompositeData, state2.RasterCompositeTileIndex);
		return true;
	}

	private bool TryBuildRasterCompositeRenderStateCore(AdobeAnimateRuntimeDefinition definition, AdobeAnimateRasterCompositeData raster, Node renderMountParent, Transform2D globalTransform, out AdobeAnimateRasterCompositeRenderState state)
	{
		state = default;
		if (!GodotObject.IsInstanceValid(raster))
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.raster.reject.data", 1);
			return false;
		}
		if (IsClipBlendActiveForRender())
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.raster.reject.blend", 1);
			return false;
		}
		if (!raster.AllowsMediaReplaceMask(_cachedMediaReplaceUseMask, _cachedMediaReplaceUseMaskOverflow))
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.raster.reject.media", 1);
			return false;
		}
		if (GetSpriteChildrenForRender().Length != 0)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.raster.reject.children", 1);
			return false;
		}
		if (HasManagedSlotSpritesForRender())
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.raster.reject.slots", 1);
			return false;
		}
		if (HasExternalVisualsForRender())
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.raster.reject.external", 1);
			return false;
		}
		if (RequiresInternalDrawItemSortForRender())
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.raster.reject.sort", 1);
			return false;
		}
		if (!TryResolveRasterCompositeTile(raster, GetRenderFrameFloat(), out var tileIndex))
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.raster.reject.pose", 1);
			return false;
		}
		Transform2D globalTransform2 = AdobeAnimateRenderManager.ToRenderMountLocalTransform(renderMountParent, globalTransform);
		Color effectiveRenderModulate = GetEffectiveRenderModulate(renderMountParent);
		int effectiveZIndexForRender = GetEffectiveZIndexForRender();
		state = new AdobeAnimateRasterCompositeRenderState(definition, renderMountParent, globalTransform2, effectiveRenderModulate, _verticalClip, effectiveZIndexForRender, raster, tileIndex);
		_needsRenderSubmission = false;
		return true;
	}

	private bool TryResolveRasterCompositeTile(AdobeAnimateRasterCompositeData raster, float frameFloat, out int tileIndex)
	{
		if (_cachedRasterCompositeSequenceData != raster || _cachedRasterCompositeSequenceClip != _clip || _cachedRasterCompositeSequenceRange != clipRange || _cachedRasterCompositeSequenceLayerSignature != _cachedLayerVisibleSignature)
		{
			_cachedRasterCompositeSequenceData = raster;
			_cachedRasterCompositeSequenceClip = _clip;
			_cachedRasterCompositeSequenceRange = clipRange;
			_cachedRasterCompositeSequenceLayerSignature = _cachedLayerVisibleSignature;
			_cachedRasterCompositeSequenceValid = raster.TryResolveSequence(_clip, clipRange, _cachedLayerVisibleSignature, out _cachedRasterCompositeSequenceTileBase, out _cachedRasterCompositeSequenceClipStart, out _cachedRasterCompositeSequenceFrameCount);
		}
		if (!_cachedRasterCompositeSequenceValid)
		{
			tileIndex = 0;
			return false;
		}
		int num = Math.Clamp(Mathf.FloorToInt(frameFloat) - _cachedRasterCompositeSequenceClipStart, 0, _cachedRasterCompositeSequenceFrameCount - 1);
		tileIndex = _cachedRasterCompositeSequenceTileBase + num;
		return tileIndex >= 0;
	}

	private bool TryReuseGpuGraphCrowdRenderState(float animationClockSeconds, out AdobeAnimateCrowdRenderState state)
	{
		state = null;
		if (!_hasCachedGpuGraphCrowdState)
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderCollectStateGpuRejectCache);
			return false;
		}
		if (_cachedGpuGraphCrowdState.Mode != AdobeAnimateCrowdRenderMode.GpuGraph)
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderCollectStateGpuRejectCache);
			return false;
		}
		if (RefreshRenderStaticStateCache())
		{
			_gpuGraphCrowdStaticStateDirty = true;
			_runtimeGpuPrepareRequired = true;
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderCollectStateGpuRejectStatic);
			return false;
		}
		if (_needsRenderSubmission && _gpuGraphCrowdStaticStateDirty)
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderCollectStateGpuRejectStatic);
			return false;
		}
		if (!AdobeAnimateRenderManager.IsGpuGraphStateCurrent(this, _cachedGpuGraphCrowdState.GpuGraphAllocation.Signature))
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderCollectStateGpuRejectGraph);
			return false;
		}
		if (!AdobeAnimateRenderManager.TryRefreshGpuGraphStateAtlasBinding(_cachedGpuGraphCrowdState))
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderCollectStateGpuRejectGraph);
			return false;
		}
		Node cachedRenderMountParent = GetCachedRenderMountParent();
		if (cachedRenderMountParent == null || cachedRenderMountParent != _cachedGpuGraphCrowdState.RenderMountParent || (_forceLocalRender && !GodotObject.IsInstanceValid(cachedRenderMountParent)))
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderCollectStateGpuRejectMount);
			return false;
		}
		Color modulate = _cachedGpuGraphCrowdState.Modulate;
		int num = _cachedGpuGraphCrowdState.EffectiveZIndex;
		ulong physicsFrameForRender = AdobeAnimateRenderManager.GetPhysicsFrameForRender();
		bool gpuGraphCrowdPresentationStateDirty = _gpuGraphCrowdPresentationStateDirty;
		bool flag = physicsFrameForRender >= _gpuGraphCrowdStateNextAuditPhysicsFrame;
		if (gpuGraphCrowdPresentationStateDirty | flag)
		{
			modulate = GetEffectiveRenderModulate(cachedRenderMountParent);
			num = GetEffectiveZIndexForRender();
			if (flag)
			{
				ScheduleNextGpuGraphCrowdStateAudit();
			}
			bool flag2 = modulate != _cachedGpuGraphCrowdState.Modulate || num != _cachedGpuGraphCrowdState.EffectiveZIndex;
			if (!gpuGraphCrowdPresentationStateDirty & flag2)
			{
				TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderCollectStateGpuRejectPresentation);
				return false;
			}
		}
		AdobeAnimateRuntimeDefinition definition = _cachedGpuGraphCrowdState.Definition;
		Transform2D cachedGlobalTransformForRender = GetCachedGlobalTransformForRender();
		if (!IsRuntimeCrowdRenderBoundsVisible(definition, cachedGlobalTransformForRender, cachedRenderMountParent))
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderCollectStateGpuRejectBounds);
			return false;
		}
		Transform2D globalTransform = AdobeAnimateRenderManager.ToRenderMountLocalTransform(cachedRenderMountParent, cachedGlobalTransformForRender);
		AdobeAnimateGpuGraphOwnerState gpuGraphRootOwnerState;
		if (_cachedGpuGraphRootOwnerPlaybackRevision != _playbackRevision || !SameGpuHitFlashState(_cachedGpuGraphCrowdState.GpuGraphRootOwnerState.GpuBrightFlash, _gpuBrightFlash) || !SameGpuHitFlashState(_cachedGpuGraphCrowdState.GpuGraphRootOwnerState.GpuWhiteFlash, _gpuWhiteFlash) || IsClipBlendActiveForRender())
		{
			gpuGraphRootOwnerState = (_cachedGpuGraphRootOwnerState = BuildGpuGraphOwnerStateForRender(definition, globalTransform, modulate, GetRenderFrameFloat(), enableGpuClock: true, animationClockSeconds, _cachedGpuGraphRootOwnerState));
			_cachedGpuGraphRootOwnerPlaybackRevision = _playbackRevision;
		}
		else
		{
			gpuGraphRootOwnerState = _cachedGpuGraphCrowdState.GpuGraphRootOwnerState.WithPresentation(in globalTransform, in modulate);
		}
		AdobeAnimateRootMotionState rootMotion = BuildRootMotionStateForRender(cachedRenderMountParent, globalTransform);
		state = _cachedGpuGraphCrowdState.WithGpuGraphDynamicState(in globalTransform, in modulate, num, gpuGraphRootOwnerState, in rootMotion);
		_cachedGpuGraphCrowdState = state;
		_cachedGpuGraphCrowdStatePhysicsFrame = AdobeAnimateRenderManager.GetPhysicsFrameForRender();
		_gpuGraphCrowdStaticStateDirty = false;
		_gpuGraphCrowdPresentationStateDirty = false;
		return true;
	}

	private void ScheduleNextGpuGraphCrowdStateAudit()
	{
		ulong physicsFrameForRender = AdobeAnimateRenderManager.GetPhysicsFrameForRender();
		_gpuGraphCrowdStateNextAuditPhysicsFrame = physicsFrameForRender + (ulong)GetStaggeredRescanInterval(240);
	}

	public void EmitAnimeCompleted(string clip)
	{
		OnAnimeCompleted?.Invoke(clip);
	}

	private void MarkEffectOnceBatchEligibilityChanged()
	{
		_effectOnceBatchEligibilityRevision++;
		EffectOnceBatchEligibilityChanged?.Invoke();
	}

	internal ulong GetEffectOnceBatchPlaybackRevision()
	{
		return _playbackRevision;
	}

	internal bool IsEffectOnceGpuBatchPlaybackPrepared(string resolvedClip, int expectedClipStart = -2147483648, int expectedClipEndExclusive = -2147483648, int expectedPlaybackFrameCount = -1, float expectedFrameRate = -1f)
	{
		int num = GetPlaybackClipEndExclusive() - clipRange.X;
		if (!string.IsNullOrEmpty(resolvedClip) && string.Equals(_clip, resolvedClip, StringComparison.Ordinal) && _canRun && !loop && !playBack && !blend && !clipOver && track.Count == 0 && _nextAnimDelayTimer <= 0.0 && _pendingTrackConfig == null && !refreshEveryFrame && !refreshEveryFlame && Math.Abs(timeScale - 1.0) <= 0.0001 && num > 1 && (expectedClipStart == -2147483648 || clipRange.X == expectedClipStart) && (expectedClipEndExclusive == -2147483648 || GetPlaybackClipEndExclusive() == expectedClipEndExclusive) && (expectedPlaybackFrameCount < 0 || num == expectedPlaybackFrameCount) && frameRate > 0.0)
		{
			if (!(expectedFrameRate < 0f))
			{
				return Math.Abs(frameRate - (double)expectedFrameRate) <= 0.0001;
			}
			return true;
		}
		return false;
	}

	internal float GetEffectOnceGpuBatchElapsedFrame()
	{
		int playbackClipEndExclusive = GetPlaybackClipEndExclusive();
		int num = Math.Max(1, playbackClipEndExclusive - clipRange.X);
		return (float)Math.Clamp((double)(frameIndex - clipRange.X) + elapsedTimer, 0.0, Math.Max(0.0, (double)num - 0.0001));
	}

	internal EffectOnceGpuAdvanceResult AdvanceEffectOnceGpuPlayback(double delta, int suppressionToken, string expectedClip, ulong expectedPlaybackRevision, double expectedElapsedFrame, int expectedClipStart, int expectedClipEndExclusive, int expectedPlaybackFrameCount, float expectedFrameRate)
	{
		if (!_effectOnceGpuSuppressed || suppressionToken == 0 || suppressionToken != _effectOnceGpuSuppressionToken || _playbackRevision != expectedPlaybackRevision || clipRange.X != expectedClipStart || GetPlaybackClipEndExclusive() != expectedClipEndExclusive || Math.Abs((double)GetEffectOnceGpuBatchElapsedFrame() - expectedElapsedFrame) > 0.001 || !IsEffectOnceGpuBatchPlaybackPrepared(expectedClip, expectedClipStart, expectedClipEndExclusive, expectedPlaybackFrameCount, expectedFrameRate))
		{
			return EffectOnceGpuAdvanceResult.Invalidated;
		}
		int x = clipRange.X;
		int playbackClipEndExclusive = GetPlaybackClipEndExclusive();
		bool flag = false;
		refreshTimer += delta * Math.Max(10.0, trueFrameRate);
		if (refreshTimer >= 1.0)
		{
			flag = true;
			refreshTimer = 0.0;
		}
		elapsedTimer += delta * Math.Abs(timeScale) * frameRate;
		while (elapsedTimer >= 1.0)
		{
			elapsedTimer--;
			EmitFrameEvents(frameIndex);
			if (!IsEffectOnceGpuCallbackStateCurrent(suppressionToken, expectedClip, expectedPlaybackRevision))
			{
				return EffectOnceGpuAdvanceResult.Invalidated;
			}
			frameIndex++;
			if (frameIndex >= playbackClipEndExclusive)
			{
				EmitFrameEvents(frameIndex);
				if (!IsEffectOnceGpuCallbackStateCurrent(suppressionToken, expectedClip, expectedPlaybackRevision))
				{
					return EffectOnceGpuAdvanceResult.Invalidated;
				}
				string text = _clip;
				clipOver = true;
				frameIndex = Math.Max(x, playbackClipEndExclusive - 1);
				elapsedTimer = 0.0;
				_lastUpdateFrame = frameIndex;
				_managedPoseRevision++;
				NextAnimation();
				if (!GodotObject.IsInstanceValid(this))
				{
					return EffectOnceGpuAdvanceResult.Completed;
				}
				if (!CachedEditorHint)
				{
					OnAnimeCompleted?.Invoke(text);
				}
				return EffectOnceGpuAdvanceResult.Completed;
			}
		}
		if (flag)
		{
			_lastUpdateFrame = frameIndex;
		}
		_managedPoseRevision++;
		return EffectOnceGpuAdvanceResult.Running;
	}

	private bool IsEffectOnceGpuCallbackStateCurrent(int suppressionToken, string expectedClip, ulong expectedPlaybackRevision)
	{
		if (GodotObject.IsInstanceValid(this) && _effectOnceGpuSuppressed && suppressionToken == _effectOnceGpuSuppressionToken && _playbackRevision == expectedPlaybackRevision)
		{
			return string.Equals(_clip, expectedClip, StringComparison.Ordinal);
		}
		return false;
	}

	internal bool TryImportEffectOnceGpuPlaybackPositionWithoutEvents(string expectedClip, float targetElapsedFrame)
	{
		if (!IsEffectOnceGpuBatchPlaybackPrepared(expectedClip))
		{
			return false;
		}
		int x = clipRange.X;
		int num = GetPlaybackClipEndExclusive() - x;
		if (num <= 0)
		{
			return false;
		}
		double num2 = Math.Clamp(targetElapsedFrame, 0.0, Math.Max(0.0, (double)num - 0.0001));
		_managedPoseRevision++;
		refreshTimer = 0.0;
		int num3 = Math.Clamp((int)Math.Floor(num2), 0, num - 1);
		frameIndex = x + num3;
		elapsedTimer = num2 - (double)num3;
		_lastUpdateFrame = frameIndex;
		return true;
	}

	internal int GetEffectOnceBatchVisualRevision()
	{
		return _effectOnceBatchVisualRevision;
	}

	internal int GetEffectOnceBatchEligibilityRevision()
	{
		return _effectOnceBatchEligibilityRevision;
	}

	internal void InvalidateEffectOnceBatchRenderState()
	{
		MarkEffectOnceBatchRenderStateDirty(invalidateZ: true);
	}

	private void MarkEffectOnceBatchRenderStateDirty(bool invalidateZ = false)
	{
		_effectOnceBatchRenderStateDirty = true;
		_effectOnceBatchRenderRevision++;
		EffectOnceBatchRenderStateChanged?.Invoke();
		if (invalidateZ)
		{
			_effectiveZIndexCached = false;
			_effectiveZIndexRescanCountdown = 0;
		}
	}

	public void SetRenderClipControl(Control control)
	{
		if (_renderClipControl != control)
		{
			AdobeAnimateRenderManager.ReleaseImmediateSubmission(this);
			_renderClipControl = (GodotObject.IsInstanceValid(control) ? control : null);
			InvalidateRenderSnapshotCache(force: true);
			if (IsInsideTree())
			{
				RefreshRenderMountCache();
			}
			RequestNodeRedraw();
		}
	}

	public void ClearRenderClipControl()
	{
		SetRenderClipControl(null);
	}

	private void InvalidateGpuRenderGraphForOffsetChange()
	{
	}

	internal void SetPlaybackBlocked(bool blocked)
	{
		if (_playbackBlocked != blocked)
		{
			bool isPlaybackStopped = IsPlaybackStopped;
			_playbackBlocked = blocked;
			if (CommitPlaybackControlChange(isPlaybackStopped))
			{
				RefreshRuntimeChildPlaybackControl();
			}
		}
	}

	private bool CommitPlaybackControlChange(bool wasPlaybackStopped)
	{
		bool isPlaybackStopped = IsPlaybackStopped;
		MarkEffectOnceBatchEligibilityChanged();
		InvalidateRuntimeDisplayVisualRequirementCache();
		InvalidateRenderSnapshotCache();
		if (isPlaybackStopped)
		{
			ClearRetainedRootMotionForPause();
		}
		if (wasPlaybackStopped != isPlaybackStopped)
		{
			_playbackRevision++;
			InvalidateRuntimePauseDispatchCache();
		}
		if (!isPlaybackStopped && _canRun && !_processEnabled)
		{
			SetProcessEnabled(enabled: true);
		}
		RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, staticStateDirty: false);
		return wasPlaybackStopped != isPlaybackStopped;
	}

	private void SetParentPlaybackStopped(bool stopped)
	{
		if (_parentPlaybackStopped != stopped)
		{
			bool isPlaybackStopped = IsPlaybackStopped;
			_parentPlaybackStopped = stopped;
			if (CommitPlaybackControlChange(isPlaybackStopped))
			{
				RefreshRuntimeChildPlaybackControl();
			}
		}
	}

	private void RefreshRuntimeChildPlaybackControl()
	{
		if (CachedEditorHint)
		{
			return;
		}
		for (int i = 0; i < _spriteChildren.Length; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _spriteChildren[i];
			if (GodotObject.IsInstanceValid(adobeAnimateSprite) && adobeAnimateSprite.IsRuntimeInsideTree)
			{
				adobeAnimateSprite.ApplyRuntimeParentState(this);
			}
		}
	}

	internal int GetEffectOnceGpuBatchSuppressionToken()
	{
		if (!_effectOnceGpuSuppressed)
		{
			return 0;
		}
		return _effectOnceGpuSuppressionToken;
	}

	internal int BeginEffectOnceGpuBatchSuppression()
	{
		_effectOnceGpuSuppressionToken++;
		if (_effectOnceGpuSuppressionToken == 0)
		{
			_effectOnceGpuSuppressionToken = 1;
		}
		_effectOnceGpuSuppressed = true;
		_runtimeRenderSubmissionRetryRequested = false;
		_needsRenderSubmission = false;
		InvalidateRuntimeDisplayVisualRequirementCache();
		AdobeAnimateRenderManager.ReleaseImmediateSubmission(this);
		AdobeAnimateRenderManager.InvalidateGpuRenderGraph(GetRenderSortRootForRender(), AdobeAnimateGpuGraphInvalidationReason.OwnerDefinition);
		SetProcessEnabled(enabled: false);
		QueueRedraw();
		return _effectOnceGpuSuppressionToken;
	}

	internal bool EndEffectOnceGpuBatchSuppression(int suppressionToken)
	{
		if (!_effectOnceGpuSuppressed || suppressionToken == 0 || suppressionToken != _effectOnceGpuSuppressionToken)
		{
			return false;
		}
		_effectOnceGpuSuppressed = false;
		InvalidateRuntimeDisplayVisualRequirementCache();
		InvalidateRenderSnapshotCache(force: true);
		_needsRenderSubmission = true;
		if (IsInsideTree())
		{
			AdobeAnimateRenderManager.InvalidateGpuRenderGraph(GetRenderSortRootForRender(), AdobeAnimateGpuGraphInvalidationReason.OwnerDefinition);
			RefreshProcessScheduling();
			RequestNodeRedraw();
		}
		return true;
	}

	public void SetFrozenPreview(bool frozen)
	{
		if (_frozenPreview != frozen)
		{
			if (frozen)
			{
				_frozenPreview = true;
				keepRenderSubmittedWhenPaused = true;
				pause = true;
				_needsRenderSubmission = true;
				RequestNodeRedraw();
				StopRuntimeTickKeepRender();
			}
			else
			{
				_frozenPreview = false;
				keepRenderSubmittedWhenPaused = false;
				pause = false;
				_needsRenderSubmission = true;
				CanRun();
				SetProcessEnabled(_canRun && !clipOver);
				RequestNodeRedraw();
			}
		}
	}

	public void EnsureFrozenPreviewRenderSubmission()
	{
		if (_frozenPreview && IsInsideTree())
		{
			UpdateVisibilityCache();
			if (_isVisibleInTree)
			{
				RequestNodeRedraw();
			}
		}
	}

	private void CommitPlaybackDirectionChange()
	{
		MarkEffectOnceBatchEligibilityChanged();
		InvalidateRuntimeDisplayVisualRequirementCache();
		InvalidateRenderSnapshotCache();
		RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, staticStateDirty: false);
	}

	internal Array<bool> GetEffectOnceBatchLayerVisible()
	{
		return _layerVisible;
	}

	internal Array<bool> GetLayerVisibleForInternalRead()
	{
		return _layerVisible;
	}

	internal int GetLayerVisibleCountForRender()
	{
		if (!_renderStaticStateCached)
		{
			return _layerVisible?.Count ?? 0;
		}
		return _cachedLayerVisibleValueCount;
	}

	internal Array<Texture2D> GetMediaReplaceForInternalRead()
	{
		return _mediaReplace;
	}

	internal Array<bool> GetMediaReplaceUseForInternalRead()
	{
		return _mediaReplaceUse;
	}

	internal Array<bool> GetEffectOnceBatchMediaReplaceUse()
	{
		return _mediaReplaceUse;
	}

	internal int BeginEffectOnceBatchEligibilityTracking()
	{
		_effectOnceBatchEligibilityTracking = true;
		return _effectOnceBatchArrayExposureRevision;
	}

	internal void EndEffectOnceBatchEligibilityTracking()
	{
		_effectOnceBatchEligibilityTracking = false;
	}

	internal int GetEffectOnceBatchArrayExposureRevision()
	{
		return _effectOnceBatchArrayExposureRevision;
	}

	private void MarkEffectOnceBatchArrayExposure()
	{
		if (_effectOnceBatchEligibilityTracking)
		{
			_effectOnceBatchArrayExposureRevision++;
			MarkEffectOnceBatchEligibilityChanged();
		}
	}

	private void MarkRenderStaticLayerArrayExposure()
	{
		if (!_renderStaticLayerArrayEscaped)
		{
			_renderStaticLayerArrayEscaped = true;
			ArmRenderStaticArrayAuditAfterExposure();
		}
	}

	private void MarkRenderStaticMediaArrayExposure()
	{
		if (!_renderStaticMediaArraysEscaped)
		{
			_renderStaticMediaArraysEscaped = true;
			QueueUpdateMediaReplace();
			ArmRenderStaticArrayAuditAfterExposure();
		}
	}

	private void ArmRenderStaticArrayAuditAfterExposure()
	{
		AdobeAnimateRuntimeManager.SetRenderStaticArrayAuditRegistration(this, enabled: true);
		if (_renderStaticStateCached)
		{
			_renderStaticStateCached = false;
			_renderStaticArrayAuditSubmissionPending = true;
			InvalidateRuntimeCrowdCullingForRefresh();
			RequestNodeRedraw();
		}
	}

	public void SetRenderColorMultiplier(Color multiplier)
	{
		if (!(_renderColorMultiplier == multiplier))
		{
			_renderColorMultiplier = multiplier;
			ApplyComposedModulate();
			_needsRenderSubmission = true;
			if (IsNodeReady())
			{
				RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, staticStateDirty: false);
			}
		}
	}

	internal bool CanUseGpuHitFlashEnvelope()
	{
		if (!_usingRuntimeManager || CachedEditorHint || !_isVisibleInTree || !IsInsideTree())
		{
			return false;
		}
		if (_runtimeGpuGraphActive)
		{
			return true;
		}
		AdobeAnimateSprite renderSortRootForRender = GetRenderSortRootForRender();
		if (!GodotObject.IsInstanceValid(renderSortRootForRender) || renderSortRootForRender == this || !renderSortRootForRender._runtimeGpuGraphActive || !renderSortRootForRender._hasCachedGpuGraphCrowdState)
		{
			return false;
		}
		AdobeAnimateSprite[] array = renderSortRootForRender._cachedGpuGraphCrowdState?.GpuGraphOwners;
		for (int i = 0; i < (array?.Length ?? 0); i++)
		{
			if (array[i] == this)
			{
				return true;
			}
		}
		return false;
	}

	internal void StartGpuHitFlashEnvelope(bool white, float startTime, float strength, float duration)
	{
		AdobeAnimateGpuHitFlashState adobeAnimateGpuHitFlashState = new AdobeAnimateGpuHitFlashState(startTime, strength, duration);
		if (white)
		{
			_gpuWhiteFlash = adobeAnimateGpuHitFlashState;
		}
		else
		{
			_gpuBrightFlash = adobeAnimateGpuHitFlashState;
		}
		RequestNodeRedraw(preferDisplayCadence: true, transformOnly: false, staticStateDirty: false);
	}

	internal void ClearGpuHitFlashEnvelopes(bool clearBright, bool clearWhite)
	{
		if ((clearBright && _gpuBrightFlash.Enabled) || (clearWhite && _gpuWhiteFlash.Enabled))
		{
			if (clearBright)
			{
				_gpuBrightFlash = default;
			}
			if (clearWhite)
			{
				_gpuWhiteFlash = default;
			}
			RequestNodeRedraw(preferDisplayCadence: true, transformOnly: false, staticStateDirty: false);
		}
	}

	public void SetRenderGrayscale(bool enabled)
	{
		if (_renderGrayscale != enabled)
		{
			_renderGrayscale = enabled;
			InvalidateRenderSnapshotCache(force: true);
			if (IsNodeReady())
			{
				RequestNodeRedraw();
			}
		}
	}

	public bool IsRenderGrayscaleEnabled()
	{
		return _renderGrayscale;
	}

	public void SetRenderSelfModulate(Color selfModulate)
	{
		if (!(SelfModulate == selfModulate))
		{
			SelfModulate = selfModulate;
			_needsRenderSubmission = true;
			if (IsNodeReady())
			{
				RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, staticStateDirty: false);
			}
		}
	}

	internal void NotifyAncestorModulateChangedForRender()
	{
		_gpuGraphCrowdPresentationStateDirty = true;
		_needsRenderSubmission = true;
		if (IsNodeReady())
		{
			RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, staticStateDirty: false);
		}
	}

	private void ApplyComposedModulate()
	{
		Modulate = _meshColor * _renderColorMultiplier;
	}

	internal void RequestRuntimeRenderSubmissionRetry()
	{
		_runtimeRenderSubmissionRetryRequested = true;
		_needsRenderSubmission = true;
	}

	internal bool MarkRuntimeRenderSubmissionConsumed()
	{
		if (_runtimeRenderSubmissionRetryRequested)
		{
			_runtimeRenderSubmissionRetryRequested = false;
			_needsRenderSubmission = true;
			return true;
		}
		_needsRenderSubmission = false;
		return false;
	}

	internal void MarkRuntimeDisplayTicked(ulong version)
	{
		_runtimeDisplayTickVersion = version;
	}

	internal bool WasRuntimeDisplayTicked(ulong version)
	{
		return _runtimeDisplayTickVersion == version;
	}

	internal void SetRuntimeManagerDispatchActive(bool active)
	{
		if (_runtimeManagerDispatchActive != active)
		{
			_runtimeManagerDispatchActive = active;
			RestoreRuntimeNativeCanvasLayer();
			InvalidateRuntimeDisplayVisualRequirementCache();
			RequestNodeRedraw();
		}
	}

	internal void InvalidateRuntimePauseDispatchCache()
	{
		AdobeAnimateRuntimeManager.NotifyPauseDispatchInvalidated(this);
	}

	public override Array<Dictionary> _GetPropertyList()
	{
		Array<Dictionary> array = new Array<Dictionary>();
		if (_flashAnimeData == null || _flashAnimeData.clips == null || _flashAnimeData.layerDictionary == null || _flashAnimeData.mediaDictionary == null)
		{
			return array;
		}
		long num = 4L;
		if (_flashAnimeData.clips.Count > 0)
		{
			array.Add(new Dictionary
			{
				{ "name", "Animation/Clip" },
				{ "type", 4 },
				{ "hint", 2 },
				{
					"hint_string",
					"Null," + string.Join(",", _flashAnimeData.clips.Keys)
				},
				{ "usage", num }
			});
		}
		foreach (Variant key in _flashAnimeData.layerDictionary.Keys)
		{
			array.Add(new Dictionary
			{
				{
					"name",
					$"Animation/LayerVisible/{key}"
				},
				{ "type", 1 },
				{ "usage", num }
			});
		}
		foreach (Variant key2 in _flashAnimeData.mediaDictionary.Keys)
		{
			array.Add(new Dictionary
			{
				{
					"name",
					$"Animation/MediaReplace/{key2}"
				},
				{ "type", 4 },
				{ "hint", 13 },
				{ "hint_string", "*.png,*.webp,*.jpg,*.jpeg,*.svg,*.bmp,*.tga" },
				{ "usage", num }
			});
		}
		AddParentSpriteFollowLayerProperty(array, num);
		AddParentSpriteInsertLayerProperty(array, num);
		return array;
	}

	private void AddParentSpriteFollowLayerProperty(Array<Dictionary> properties, long usage)
	{
		AddParentSpriteLayerProperty(properties, usage, "Parent Sprite/Follow Layer", includeTopOption: false);
	}

	private void AddParentSpriteInsertLayerProperty(Array<Dictionary> properties, long usage)
	{
		AddParentSpriteLayerProperty(properties, usage, "Parent Sprite/Insert Layer", includeTopOption: true);
	}

	private void AddParentSpriteLayerProperty(Array<Dictionary> properties, long usage, string propertyName, bool includeTopOption)
	{
		properties.Add(new Dictionary
		{
			{ "name", propertyName },
			{ "type", 2 },
			{ "hint", 2 },
			{
				"hint_string",
				BuildParentSpriteLayerHintString(includeTopOption)
			},
			{ "usage", usage }
		});
	}

	private string BuildParentSpriteLayerHintString(bool includeTopOption)
	{
		string text = "0";
		AdobeAnimateData adobeAnimateData = ResolveParentSpriteLayerInspectorData();
		if (adobeAnimateData != null && adobeAnimateData.layerDictionary != null && adobeAnimateData.layerDictionary.Count > 0)
		{
			text = adobeAnimateData.GetLayerHintString();
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "0";
		}
		if (!includeTopOption)
		{
			return text;
		}
		return "Top:-1," + text;
	}

	private AdobeAnimateData ResolveParentSpriteLayerInspectorData()
	{
		AdobeAnimateData adobeAnimateData = ResolveParentSpriteForLayerInspector()?.flashAnimeData;
		if (adobeAnimateData != null && adobeAnimateData.layerDictionary != null && adobeAnimateData.layerDictionary.Count > 0)
		{
			return adobeAnimateData;
		}
		return _flashAnimeData;
	}

	private AdobeAnimateSprite ResolveParentSpriteForLayerInspector()
	{
		if (!_parentSpriteResolved || (_parentSprite != null && !GodotObject.IsInstanceValid(_parentSprite)))
		{
			parentSprite = FindParentSpriteAncestor();
		}
		return _parentSprite;
	}

	public override void _Notification(int what)
	{
		base._Notification(what);
		if ((long)what == 20)
		{
			SealPackedSceneStaticArraysInSubtree(this);
		}
		if ((long)what == 14)
		{
			_renderTreePaused = true;
			bool flag = ClearRetainedRootMotionForPause();
			InvalidateRuntimePauseDispatchCache();
			if (flag)
			{
				RequestNodeRedraw();
			}
		}
		else if ((long)what == 15)
		{
			_renderTreePaused = false;
			InvalidateRuntimePauseDispatchCache();
		}
		else if ((long)what == 28 || (long)what == 29)
		{
			InvalidateRuntimePauseDispatchCache();
		}
		if ((long)what == 2000)
		{
			MarkEffectOnceBatchRenderStateDirty();
			AdobeAnimateSprite renderSortRootForRender = GetRenderSortRootForRender();
			if (GodotObject.IsInstanceValid(renderSortRootForRender) && renderSortRootForRender != this && renderSortRootForRender.ShouldUseGlobalRuntimeManager())
			{
				if (!renderSortRootForRender.TryConsumeKnownAncestorTranslationNotification())
				{
					renderSortRootForRender.MarkRenderTransformDirty();
				}
			}
			else if (!TryConsumeKnownAncestorTranslationNotification())
			{
				MarkRenderTransformDirty();
			}
		}
		else if ((long)what == 18)
		{
			if (_usingRuntimeManager)
			{
				AdobeAnimateRuntimeManager.InvalidateRenderRoots();
			}
			MarkEffectOnceBatchRenderStateDirty(invalidateZ: true);
			InvalidateRuntimePauseDispatchCache();
			_renderMountNextAuditPhysicsFrame = 0uL;
			ResetEffectiveRenderModulateAncestorCache();
			InvalidateTreeOrderPathCache();
			_parentSpriteResolved = false;
			parentSprite = FindParentSpriteAncestor();
		}
		else if ((long)what == 19)
		{
			if (_usingRuntimeManager)
			{
				AdobeAnimateRuntimeManager.InvalidateRenderRoots();
			}
			MarkEffectOnceBatchRenderStateDirty(invalidateZ: true);
			InvalidateRuntimePauseDispatchCache();
			_renderMountNextAuditPhysicsFrame = 0uL;
			ResetEffectiveRenderModulateAncestorCache();
			InvalidateTreeOrderPathCache();
			_parentSpriteResolved = true;
			parentSprite = null;
		}
	}

	internal static void SealPackedSceneStaticArraysInSubtree(Node root)
	{
		if (GodotObject.IsInstanceValid(root))
		{
			if (root is AdobeAnimateSprite adobeAnimateSprite)
			{
				adobeAnimateSprite.SealPackedSceneStaticArrays();
			}
			int childCount = root.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				SealPackedSceneStaticArraysInSubtree(root.GetChild(i));
			}
		}
	}

	private void SealPackedSceneStaticArrays()
	{
		_layerVisible = _layerVisible?.Duplicate() ?? new Array<bool>();
		_mediaReplace = _mediaReplace?.Duplicate() ?? new Array<Texture2D>();
		_mediaReplaceUse = _mediaReplaceUse?.Duplicate() ?? new Array<bool>();
		_mediaReplaceAtlasPaths = _mediaReplaceAtlasPaths?.Duplicate() ?? new Array<string>();
		_renderStaticLayerArrayEscaped = false;
		_renderStaticMediaArraysEscaped = false;
		_renderStaticArrayAuditSubmissionPending = false;
		_renderStaticStateNextAuditPhysicsFrame = 18446744073709551615uL;
		_renderStaticAuditLayerVisibleSnapshot = null;
		_renderStaticAuditMediaReplaceUseSnapshot = null;
		_renderStaticAuditMediaReplaceSnapshot = null;
		_renderStaticAuditMediaReplaceRectSnapshot = null;
		_renderStaticAuditMediaReplacePageSnapshot = null;
		_renderStaticAuditMediaReplaceAtlasPathSnapshot = null;
		_renderStaticStateCached = false;
		AdobeAnimateRuntimeManager.SetRenderStaticArrayAuditRegistration(this, enabled: false);
	}

	public override bool _Set(StringName property, Variant value)
	{
		if (property == (StringName)"Animation/Clip")
		{
			_clip = value.AsString();
			if (_flashAnimeData != null && _flashAnimeData.clips != null && _flashAnimeData.clips.Count > 0)
			{
				SetClip(value.AsString());
			}
			return true;
		}
		string text = property.ToString();
		if (text.StartsWith("Animation/LayerVisible"))
		{
			string text2 = text.TrimPrefix("Animation/LayerVisible/");
			if (_flashAnimeData != null && _flashAnimeData.layerDictionary != null && _flashAnimeData.layerDictionary.ContainsKey(text2))
			{
				int num = (int)_flashAnimeData.layerDictionary[text2];
				if (num >= 0 && num < _layerVisible.Count)
				{
					bool flag = value.AsBool();
					if (_layerVisible[num] != flag)
					{
						_layerVisible[num] = flag;
						MarkLayerStateChanged();
					}
				}
			}
			return true;
		}
		if (text.StartsWith("Animation/MediaReplace"))
		{
			string text3 = text.TrimPrefix("Animation/MediaReplace/");
			if (_flashAnimeData != null && _flashAnimeData.mediaDictionary != null && _flashAnimeData.mediaDictionary.ContainsKey(text3))
			{
				int num2 = (int)_flashAnimeData.mediaDictionary[text3];
				if (num2 >= 0 && num2 < _mediaReplace.Count)
				{
					string text4 = value.AsString();
					bool flag2 = !string.IsNullOrWhiteSpace(text4);
					if (!GodotObject.IsInstanceValid(_mediaReplace[num2]) && num2 < _mediaReplaceAtlasPaths.Count && _mediaReplaceAtlasPaths[num2] == text4 && num2 < _mediaReplaceUse.Count && _mediaReplaceUse[num2] == flag2)
					{
						return true;
					}
					_mediaReplace[num2] = null;
					if (num2 < _mediaReplaceAtlasPaths.Count)
					{
						_mediaReplaceAtlasPaths[num2] = text4;
					}
					_mediaReplaceUse[num2] = flag2;
					InvalidateStaticAtlasPathLayoutConfigurationSnapshot();
					InvalidateStaticAtlasPathLayoutForNodeMutation();
					QueueUpdateMediaReplace();
				}
			}
			return true;
		}
		if (IsParentSpriteFollowLayerProperty(property))
		{
			SetParentSpriteFollowLayer(value.AsInt32());
			return true;
		}
		if (IsParentSpriteInsertLayerProperty(property))
		{
			SetParentSpriteInsertLayer(value.AsInt32());
			return true;
		}
		return false;
	}

	public override Variant _Get(StringName property)
	{
		if (property == (StringName)"Animation/Clip")
		{
			return _clip;
		}
		string text = property.ToString();
		if (text.StartsWith("Animation/LayerVisible"))
		{
			if (_flashAnimeData == null || _flashAnimeData.layerDictionary == null)
			{
				return default;
			}
			string text2 = text.TrimPrefix("Animation/LayerVisible/");
			if (!_flashAnimeData.layerDictionary.ContainsKey(text2))
			{
				return default;
			}
			int num = (int)_flashAnimeData.layerDictionary[text2];
			return num >= 0 && num < _layerVisible.Count && _layerVisible[num];
		}
		if (text.StartsWith("Animation/MediaReplace"))
		{
			if (_flashAnimeData == null || _flashAnimeData.mediaDictionary == null)
			{
				return default;
			}
			string text3 = text.TrimPrefix("Animation/MediaReplace/");
			if (!_flashAnimeData.mediaDictionary.ContainsKey(text3))
			{
				return default;
			}
			int num2 = (int)_flashAnimeData.mediaDictionary[text3];
			return (num2 >= 0 && num2 < _mediaReplaceAtlasPaths.Count) ? _mediaReplaceAtlasPaths[num2] : null;
		}
		if (IsParentSpriteFollowLayerProperty(property))
		{
			return followParentSpriteLayerId;
		}
		if (IsParentSpriteInsertLayerProperty(property))
		{
			return insertLayerId;
		}
		return default;
	}

	public override bool _PropertyCanRevert(StringName property)
	{
		if (_flashAnimeData == null || _flashAnimeData.clips == null)
		{
			return false;
		}
		if (property == (StringName)"Animation/Clip" && _flashAnimeData.clips.Count > 0)
		{
			return _clip != _flashAnimeData.clips.Keys.ElementAt(0).ToString();
		}
		string text = property.ToString();
		if (!text.StartsWith("Animation/LayerVisible") && !text.StartsWith("Animation/MediaReplace") && !IsParentSpriteFollowLayerProperty(property))
		{
			return IsParentSpriteInsertLayerProperty(property);
		}
		return true;
	}

	public override Variant _PropertyGetRevert(StringName property)
	{
		if (_flashAnimeData == null || _flashAnimeData.clips == null)
		{
			return default;
		}
		if (property == (StringName)"Animation/Clip" && _flashAnimeData.clips.Count > 0)
		{
			return _flashAnimeData.clips.Keys.ElementAt(0);
		}
		string text = property.ToString();
		if (text.StartsWith("Animation/LayerVisible"))
		{
			return true;
		}
		if (text.StartsWith("Animation/MediaReplace"))
		{
			return default;
		}
		if (IsParentSpriteFollowLayerProperty(property))
		{
			return 0;
		}
		if (IsParentSpriteInsertLayerProperty(property))
		{
			return -1;
		}
		return default;
	}

	private static bool IsParentSpriteFollowLayerProperty(StringName property)
	{
		if (!(property == (StringName)"Parent Sprite/Follow Layer"))
		{
			return property == (StringName)"Layer";
		}
		return true;
	}

	private static bool IsParentSpriteInsertLayerProperty(StringName property)
	{
		return property == (StringName)"Parent Sprite/Insert Layer";
	}

	private void SetParentSpriteFollowLayer(int layerId)
	{
		followParentSpriteLayerId = layerId;
		AdobeAnimateSprite adobeAnimateSprite = ResolveParentSpriteForLayerInspector();
		if (GodotObject.IsInstanceValid(adobeAnimateSprite) && adobeAnimateSprite != this)
		{
			adobeAnimateSprite.UpdateChild();
			adobeAnimateSprite.InvalidateRenderSnapshotCache();
			adobeAnimateSprite.RequestNodeRedraw();
		}
		InvalidateRenderSnapshotCache();
		RequestNodeRedraw();
	}

	private void SetParentSpriteInsertLayer(int layerId)
	{
		insertLayerId = layerId;
		AdobeAnimateSprite adobeAnimateSprite = ResolveParentSpriteForLayerInspector();
		if (GodotObject.IsInstanceValid(adobeAnimateSprite) && adobeAnimateSprite != this)
		{
			adobeAnimateSprite.RefreshChildInsertLayerForEditor(this);
			return;
		}
		InvalidateRenderSnapshotCache();
		RequestNodeRedraw();
	}

	public bool CanRun()
	{
		bool canRun = _canRun;
		_canRun = false;
		if (_effectOnceGpuSuppressed)
		{
			TryDisableProcess(canRun);
			return false;
		}
		if (_frozenPreview)
		{
			TryDisableProcess(canRun);
			return false;
		}
		if (!_invisible && !_isVisibleInTree)
		{
			TryDisableProcess(canRun);
			return false;
		}
		if (!HasUsableFlashAnimeData())
		{
			TryDisableProcess(canRun);
			return false;
		}
		if (CachedEditorHint && !_preview)
		{
			TryDisableProcess(canRun);
			return false;
		}
		_canRun = true;
		if (canRun != _canRun && IsNodeReady() && !clipOver)
		{
			SetProcessEnabled(enabled: true);
		}
		return true;
	}

	private void TryDisableProcess(bool prevCanRun)
	{
		if (prevCanRun && IsNodeReady())
		{
			SetProcessEnabled(enabled: false);
		}
	}

	private void UpdateVisibilityCache()
	{
		bool flag = IsVisibleInTree();
		if (_isVisibleInTree != flag)
		{
			_isVisibleInTree = flag;
			AdobeAnimateRuntimeManager.NotifyRenderRootVisibilityChanged(this);
			if (!flag)
			{
				ReleaseRuntimeManagerFirstFrameHandoff();
				AdobeAnimateRenderManager.ReleaseImmediateSubmission(this);
			}
			CanRun();
			RequestNodeRedraw();
		}
	}

	private void OnSelfVisibilityChanged()
	{
		UpdateVisibilityCache();
		if (_runtimeFollowVisibilityKnown && Visible != _runtimeFollowVisibilityValue)
		{
			Visible = _runtimeFollowVisibilityValue;
		}
	}

	private void SetProcessEnabled(bool enabled)
	{
		if ((_frozenPreview || _effectOnceGpuSuppressed) & enabled)
		{
			enabled = false;
		}
		InvalidateRuntimePauseDispatchCache();
		InvalidateRuntimeDisplayVisualRequirementCache();
		_processEnabled = enabled;
		if (!enabled)
		{
			ReleaseRuntimeManagerFirstFrameHandoff();
			RestoreRuntimeNativeCanvasLayer();
			if (_usingRuntimeManager)
			{
				AdobeAnimateRuntimeManager.Unregister(this);
				_usingRuntimeManager = false;
			}
			SetProcess(enable: false);
			SetPhysicsProcess(enable: false);
			return;
		}
		if (ShouldUseGlobalRuntimeManager() && IsInsideTree())
		{
			bool flag = !_usingRuntimeManager;
			ReleaseRuntimeManagerFirstFrameHandoff();
			AdobeAnimateRenderManager.ReleaseImmediateSubmission(this);
			_usingRuntimeManager = AdobeAnimateRuntimeManager.Register(this);
			if (_usingRuntimeManager & flag)
			{
				SubmitRuntimeManagerFirstFrameHandoff();
			}
		}
		else if (_usingRuntimeManager)
		{
			AdobeAnimateRuntimeManager.Unregister(this);
			_usingRuntimeManager = false;
		}
		if (_usingRuntimeManager)
		{
			SetProcess(enable: false);
			SetPhysicsProcess(enable: false);
			_needsRenderSubmission = true;
		}
		else
		{
			SetProcess(enable: true);
			SetPhysicsProcess(enable: false);
		}
	}

	private void ConnectVisibilitySignals()
	{
		if (!_selfVisibilityChangedConnected)
		{
			VisibilityChanged += OnSelfVisibilityChanged;
			_selfVisibilityChangedConnected = true;
		}
		if (!(GetParent() is CanvasItem canvasItem))
		{
			return;
		}
		if (_visibilityParentCanvas != canvasItem)
		{
			if (_parentVisibilityChangedConnected && GodotObject.IsInstanceValid(_visibilityParentCanvas))
			{
				_visibilityParentCanvas.VisibilityChanged -= UpdateVisibilityCache;
			}
			_parentVisibilityChangedConnected = false;
			_visibilityParentCanvas = canvasItem;
		}
		if (!_parentVisibilityChangedConnected)
		{
			_visibilityParentCanvas.VisibilityChanged += UpdateVisibilityCache;
			_parentVisibilityChangedConnected = true;
		}
	}

	private void DisconnectVisibilitySignals()
	{
		if (_parentVisibilityChangedConnected && GodotObject.IsInstanceValid(_visibilityParentCanvas))
		{
			_visibilityParentCanvas.VisibilityChanged -= UpdateVisibilityCache;
		}
		_parentVisibilityChangedConnected = false;
		_visibilityParentCanvas = null;
		if (_selfVisibilityChangedConnected)
		{
			VisibilityChanged -= OnSelfVisibilityChanged;
		}
		_selfVisibilityChangedConnected = false;
	}

	private void StopRuntimeTickKeepRender()
	{
		_needsRenderSubmission = true;
		SetProcessEnabled(enabled: false);
		if (!_usingRuntimeManager)
		{
			QueueRedraw();
		}
	}

	public void RefreshProcessScheduling()
	{
		CanRun();
		SetProcessEnabled(_canRun && !clipOver);
	}

	public override void _EnterTree()
	{
		_runtimeInsideTree = true;
		_runtimeParentStateSource = null;
		_runtimeParentStateValidationCountdown = 0;
		ResetRuntimeFollowVisibilityCache();
		InvalidateRuntimeDisplayVisualRequirementCache();
		_renderTreePaused = GetTree()?.Paused ?? false;
		_cachedInstanceIdForRender = GetInstanceId();
		parentSprite = FindParentSpriteAncestor();
		InvalidateRuntimePauseDispatchCache();
		RefreshTransformNotificationMode();
		MarkRenderTransformDirty();
		ConnectVisibilitySignals();
		RefreshRenderMountCache();
		if (IsNodeReady())
		{
			_runtimeReadyCached = true;
			_isVisibleInTree = IsVisibleInTree();
			RefreshProcessScheduling();
		}
	}

	internal ulong GetCachedInstanceIdForRender()
	{
		if (_cachedInstanceIdForRender == 0L)
		{
			_cachedInstanceIdForRender = GetInstanceId();
		}
		return _cachedInstanceIdForRender;
	}

	internal bool ConsumeRuntimeGpuPrepareRequiredForRender()
	{
		bool runtimeGpuPrepareRequired = _runtimeGpuPrepareRequired;
		_runtimeGpuPrepareRequired = false;
		return runtimeGpuPrepareRequired;
	}

	internal bool HasPreparedGpuRenderGraph(ulong generation)
	{
		if (generation != 0L)
		{
			return _gpuGraphPreparedGeneration == generation;
		}
		return false;
	}

	internal int GetPreparedGpuRenderGraphQuadCount(ulong generation)
	{
		if (!HasPreparedGpuRenderGraph(generation))
		{
			return 1;
		}
		return _gpuGraphPreparedQuadCount;
	}

	internal void MarkGpuRenderGraphPrepared(ulong generation, ulong signature, int quadCount)
	{
		_gpuGraphPreparedGeneration = generation;
		_gpuGraphPreparedSignature = signature;
		_gpuGraphPreparedQuadCount = Math.Max(1, quadCount);
	}

	internal bool HasPreparedGpuRenderGraphState(ulong generation, ulong signature)
	{
		if (HasPreparedGpuRenderGraph(generation) && signature != 0L)
		{
			return _gpuGraphPreparedSignature == signature;
		}
		return false;
	}

	internal object GetPreparedGpuRootCacheForRender()
	{
		return _preparedGpuRootCacheForRender;
	}

	internal void StorePreparedGpuRootCacheForRender(object entry)
	{
		_preparedGpuRootCacheForRender = entry;
	}

	internal void ClearPreparedGpuRootCacheForRender()
	{
		_preparedGpuRootCacheForRender = null;
	}

	internal void InvalidateGpuRenderGraphPreparation()
	{
		_gpuGraphPreparedGeneration = 0uL;
		_gpuGraphPreparedSignature = 0uL;
		_gpuGraphPreparedQuadCount = 1;
		_preparedGpuRootCacheForRender = null;
		_hasCachedGpuGraphCrowdState = false;
		_cachedGpuGraphCrowdState = null;
		_cachedGpuGraphCrowdStatePhysicsFrame = 18446744073709551615uL;
		_hasCachedRasterCompositeRenderState = false;
		_cachedRasterCompositeRenderState = default;
		_gpuGraphCrowdStaticStateDirty = false;
		_gpuGraphCrowdPresentationStateDirty = false;
		_gpuGraphCrowdStateNextAuditPhysicsFrame = 18446744073709551615uL;
	}

	public override void _Ready()
	{
		_isVisibleInTree = IsVisibleInTree();
		canvasItem = GetCanvasItem();
		RefreshRenderMountCache();
		ApplyPendingFlashAnimeDataChange();
		TryHydrateFlashAnimeDataBeforeFileChange();
		CanRun();
		parentSprite = FindParentSpriteAncestor();
		frameIndex = clipRange.X;
		elapsedTimer = GD.RandRange(0.1, 1.0);
		clipOver = false;
		if (!HasUsableFlashAnimeData())
		{
			SetProcessEnabled(enabled: false);
			_runtimeReadyCached = true;
			return;
		}
		if (_clip != "" && _flashAnimeData.clips.ContainsKey(_clip))
		{
			clipRange = _flashAnimeData.GetClip(_clip);
			frameIndex = clipRange.X;
		}
		if (HasAnyRequestedMediaReplace() || needMediaReplaceUpdate)
		{
			UpdateMediaReplaceData();
		}
		onlyDraw = false;
		meshTexture = default;
		mesh = null;
		Material = null;
		ApplyComposedModulate();
		CacheChildren();
		RefreshTransformNotificationModeForRenderTree();
		UpdateChildPoseImmediate();
		_processEnabled = false;
		SetProcessEnabled(_canRun && !clipOver);
		RequestNodeRedraw();
		_runtimeReadyCached = true;
	}

	public override void _ExitTree()
	{
		_effectOnceGpuSuppressed = false;
		_effectOnceGpuSuppressionToken++;
		if (_effectOnceGpuSuppressionToken == 0)
		{
			_effectOnceGpuSuppressionToken = 1;
		}
		_runtimeReadyCached = false;
		_runtimeRenderSubmissionRetryRequested = false;
		_runtimeParentStateSource = null;
		_runtimeParentStateValidationCountdown = 0;
		ResetRuntimeFollowVisibilityCache();
		InvalidateRuntimeDisplayVisualRequirementCache();
		_runtimeInsideTree = false;
		_isVisibleInTree = false;
		RestoreRuntimeNativeCanvasLayer();
		RestoreExternalVisualNativeFallbacks((long)Engine.GetProcessFrames());
		ReleaseRuntimeManagerFirstFrameHandoff();
		AdobeAnimateRenderManager.ReleaseImmediateSubmission(this);
		AdobeAnimateRenderManager.InvalidateGpuRenderGraph(this, AdobeAnimateGpuGraphInvalidationReason.OwnerExit);
		InvalidateGpuRenderGraphPreparation();
		InvalidateRuntimePauseDispatchCache();
		SetNotifyTransform(enable: false);
		_renderGlobalTransformCached = false;
		_renderGlobalTransformDirty = true;
		_renderTransformChangedInPhysicsFrame = false;
		_runtimePhysicsAncestorTransformPending = false;
		_knownAncestorTranslationNotificationPending = false;
		_renderRootMotionEnabled = false;
		_renderTreePaused = false;
		_renderGlobalTransformPhysicsFrame = 0uL;
		if (_usingRuntimeManager)
		{
			AdobeAnimateRuntimeManager.Unregister(this);
			_usingRuntimeManager = false;
		}
		DisconnectVisibilitySignals();
		_cachedRenderMountParent = null;
		_cachedViewport = null;
		_renderMountNextAuditPhysicsFrame = 0uL;
		ResetEffectiveRenderModulateAncestorCache();
		DisconnectTreeOrderPathWatchers();
		_treeOrderPathRescanCountdown = 0;
		DisconnectManagedSlotGpuStateWatchers();
		_cachedCanvasLayer = 0;
		_runtimeDefinition = null;
	}

	internal void GetEffectOnceBatchEligibilityVersions(out int layerStateVersion, out int mediaReplaceStateVersion)
	{
		layerStateVersion = _layerStateVersion;
		mediaReplaceStateVersion = _mediaReplaceStateVersion;
	}

	internal Transform2D GetEffectOnceBatchRenderTransform()
	{
		GetEffectOnceBatchRenderState(out var transform, out var _);
		return transform;
	}

	internal int GetEffectOnceBatchEffectiveZIndex()
	{
		GetEffectOnceBatchRenderState(out var _, out var effectiveZIndex);
		return effectiveZIndex;
	}

	internal int GetEffectOnceBatchRenderRevision(ulong processFrame)
	{
		if (_effectOnceBatchRenderStateDirty || processFrame < _effectOnceBatchRenderNextAuditFrame)
		{
			return _effectOnceBatchRenderRevision;
		}
		int zIndex = ZIndex;
		bool zAsRelative = ZAsRelative;
		if (_effectOnceBatchObservedLocalZIndex != zIndex || _effectOnceBatchObservedZAsRelative != zAsRelative)
		{
			_effectOnceBatchObservedLocalZIndex = zIndex;
			_effectOnceBatchObservedZAsRelative = zAsRelative;
			MarkEffectOnceBatchRenderStateDirty(invalidateZ: true);
			return _effectOnceBatchRenderRevision;
		}
		_effectiveZIndexCached = false;
		int cachedEffectiveZIndexForRender = GetCachedEffectiveZIndexForRender();
		_effectOnceBatchRenderNextAuditFrame = processFrame + 256;
		if (cachedEffectiveZIndexForRender == _effectOnceBatchEffectiveZIndex)
		{
			return _effectOnceBatchRenderRevision;
		}
		_effectOnceBatchEffectiveZIndex = cachedEffectiveZIndexForRender;
		_effectOnceBatchRenderRevision++;
		EffectOnceBatchRenderStateChanged?.Invoke();
		return _effectOnceBatchRenderRevision;
	}

	internal void GetEffectOnceBatchRenderState(out Transform2D transform, out int effectiveZIndex)
	{
		GetEffectOnceBatchRenderState(Engine.GetProcessFrames(), out transform, out effectiveZIndex);
	}

	internal void GetEffectOnceBatchRenderState(ulong processFrame, out Transform2D transform, out int effectiveZIndex)
	{
		if (_effectOnceBatchRenderStateDirty)
		{
			Transform2D effectOnceBatchRenderTransform = GetCachedGlobalTransformForRender();
			if (offset != Vector2.Zero)
			{
				effectOnceBatchRenderTransform = effectOnceBatchRenderTransform.TranslatedLocal(offset);
			}
			_effectOnceBatchRenderTransform = effectOnceBatchRenderTransform;
			_effectOnceBatchEffectiveZIndex = GetCachedEffectiveZIndexForRender();
			_effectOnceBatchObservedLocalZIndex = _effectiveZIndexCacheLocalZ;
			_effectOnceBatchObservedZAsRelative = _effectiveZIndexCacheZAsRelative;
			_effectOnceBatchRenderNextAuditFrame = processFrame + 1 + GetCachedInstanceIdForRender() % 256;
			_effectOnceBatchRenderStateDirty = false;
		}
		else if (processFrame >= _effectOnceBatchRenderNextAuditFrame)
		{
			_effectiveZIndexCached = false;
			_effectOnceBatchEffectiveZIndex = GetCachedEffectiveZIndexForRender();
			_effectOnceBatchRenderNextAuditFrame = processFrame + 256;
		}
		transform = _effectOnceBatchRenderTransform;
		effectiveZIndex = _effectOnceBatchEffectiveZIndex;
	}

	public string BuildCrowdFilterDebugReport(string label = "", int maxFrameSlices = 256)
	{
		StringBuilder stringBuilder = new StringBuilder(8192);
		string value = (IsInsideTree() ? GetPath().ToString() : ((string?)Name));
		if (string.IsNullOrEmpty(value))
		{
			value = "<unnamed>";
		}
		if (needMediaReplaceUpdate)
		{
			UpdateMediaReplaceData();
		}
		RefreshRenderStaticStateCache(force: true);
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		bool value2 = runtimeDefinition != null;
		bool value3 = HasManagedSlotSpritesForRender();
		bool needsDrawOrderSortBandsForRender = GetNeedsDrawOrderSortBandsForRender();
		bool value4 = runtimeDefinition != null && runtimeDefinition.UsesGpuPoseTextureArray && runtimeDefinition.GpuPoseTextureRid.IsValid && GodotObject.IsInstanceValid(runtimeDefinition.GpuPoseTextureArray);
		Transform2D cachedGlobalTransformForRender = GetCachedGlobalTransformForRender();
		Transform2D transform2D = AdobeAnimateRenderManager.ToRenderMountLocalTransform(GetCachedRenderMountParent(), cachedGlobalTransformForRender);
		bool flag = AdobeAnimateGpuRenderGraphBuilder.TryBuild(this, out var _, out var _, out var failureReason);
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder3 = stringBuilder2;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(38, 2, stringBuilder2);
		handler.AppendLiteral("[AdobeAnimateRenderDebug] label=");
		handler.AppendFormatted(label);
		handler.AppendLiteral(" path=");
		handler.AppendFormatted(value);
		stringBuilder3.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder4 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(57, 6, stringBuilder2);
		handler.AppendLiteral("  data=");
		handler.AppendFormatted(_flashAnimeData?.ResourcePath ?? "<null>");
		handler.AppendLiteral(" clip=");
		handler.AppendFormatted(_clip);
		handler.AppendLiteral(" clipRange=");
		handler.AppendFormatted(clipRange);
		handler.AppendLiteral(" frameIndex=");
		handler.AppendFormatted(frameIndex);
		handler.AppendLiteral(" elapsed=");
		handler.AppendFormatted(elapsedTimer, "F4");
		handler.AppendLiteral(" frameFloat=");
		handler.AppendFormatted(GetRenderFrameFloat(), "F4");
		stringBuilder4.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder5 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(44, 6, stringBuilder2);
		handler.AppendLiteral("  transform global=(");
		handler.AppendFormatted(cachedGlobalTransformForRender.Origin.X, "F2");
		handler.AppendLiteral(",");
		handler.AppendFormatted(cachedGlobalTransformForRender.Origin.Y, "F2");
		handler.AppendLiteral(") render=(");
		handler.AppendFormatted(transform2D.Origin.X, "F2");
		handler.AppendLiteral(",");
		handler.AppendFormatted(transform2D.Origin.Y, "F2");
		handler.AppendLiteral(") offset=(");
		handler.AppendFormatted(offset.X, "F2");
		handler.AppendLiteral(",");
		handler.AppendFormatted(offset.Y, "F2");
		handler.AppendLiteral(")");
		stringBuilder5.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder6 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(114, 7, stringBuilder2);
		handler.AppendLiteral("  runtime canRun=");
		handler.AppendFormatted(_canRun);
		handler.AppendLiteral(" processEnabled=");
		handler.AppendFormatted(_processEnabled);
		handler.AppendLiteral(" pause=");
		handler.AppendFormatted(_pause);
		handler.AppendLiteral(" usingRuntimeManager=");
		handler.AppendFormatted(_usingRuntimeManager);
		handler.AppendLiteral(" visibleInTree=");
		handler.AppendFormatted(_isVisibleInTree);
		handler.AppendLiteral(" refreshEveryFrame=");
		handler.AppendFormatted(refreshEveryFrame);
		handler.AppendLiteral(" refreshEveryFlame=");
		handler.AppendFormatted(refreshEveryFlame);
		stringBuilder6.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder7 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(141, 9, stringBuilder2);
		handler.AppendLiteral("  gpuPose capable=");
		handler.AppendFormatted(value4);
		handler.AppendLiteral(" hasDefinition=");
		handler.AppendFormatted(value2);
		handler.AppendLiteral(" atlasArrayRid=");
		handler.AppendFormatted(runtimeDefinition?.AtlasTextureArrayRid.IsValid ?? false);
		handler.AppendLiteral(" ");
		handler.AppendLiteral("poseRid=");
		handler.AppendFormatted(runtimeDefinition?.GpuPoseTextureRid.IsValid ?? false);
		handler.AppendLiteral(" poseTextureArrayValid=");
		handler.AppendFormatted(GodotObject.IsInstanceValid(runtimeDefinition?.GpuPoseTextureArray));
		handler.AppendLiteral(" ");
		handler.AppendLiteral("maxSlices=");
		handler.AppendFormatted(runtimeDefinition?.MaxFrameSliceCount ?? 0);
		handler.AppendLiteral(" ");
		handler.AppendLiteral("repeatedMedia=");
		handler.AppendFormatted(runtimeDefinition?.HasRepeatedMediaPerFrame ?? false);
		handler.AppendLiteral(" managedSlotSprites=");
		handler.AppendFormatted(value3);
		handler.AppendLiteral(" ");
		handler.AppendLiteral("drawOrderSort=");
		handler.AppendFormatted(needsDrawOrderSortBandsForRender);
		stringBuilder7.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder8 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(30, 2, stringBuilder2);
		handler.AppendLiteral("  gpuGraph buildable=");
		handler.AppendFormatted(flag);
		handler.AppendLiteral(" failure=");
		handler.AppendFormatted(flag ? "<none>" : failureReason);
		stringBuilder8.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder9 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(59, 5, stringBuilder2);
		handler.AppendLiteral("  layer count=");
		handler.AppendFormatted(_cachedLayerVisibleCount);
		handler.AppendLiteral(" allVisible=");
		handler.AppendFormatted(_cachedAllLayersVisible);
		handler.AppendLiteral(" canUseMask=");
		handler.AppendFormatted(_cachedCanUseLayerMask);
		handler.AppendLiteral(" ");
		handler.AppendLiteral("mask=0x");
		handler.AppendFormatted(_cachedLayerMask, "X16");
		handler.AppendLiteral(" signature=0x");
		handler.AppendFormatted(_cachedLayerVisibleSignature, "X16");
		stringBuilder9.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder10 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(76, 6, stringBuilder2);
		handler.AppendLiteral("  mediaReplace has=");
		handler.AppendFormatted(_cachedHasMediaReplace);
		handler.AppendLiteral(" sharedAtlas=");
		handler.AppendFormatted(mediaReplaceAtlasShared);
		handler.AppendLiteral(" atlasValid=");
		handler.AppendFormatted(_cachedMediaReplaceAtlasRid.IsValid);
		handler.AppendLiteral(" ");
		handler.AppendLiteral("limit=");
		handler.AppendFormatted(_cachedMediaReplaceLimit);
		handler.AppendLiteral(" signature=0x");
		handler.AppendFormatted(_cachedMediaReplaceSignature, "X16");
		handler.AppendLiteral(" boundsGrow=");
		handler.AppendFormatted(_cachedMediaReplaceBoundsGrow, "F2");
		stringBuilder10.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder11 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(33, 3, stringBuilder2);
		handler.AppendLiteral("  verticalClip enabled=");
		handler.AppendFormatted(_verticalClip.Enabled);
		handler.AppendLiteral(" up=");
		handler.AppendFormatted(_verticalClip.UpY, "F2");
		handler.AppendLiteral(" down=");
		handler.AppendFormatted(_verticalClip.DownY, "F2");
		stringBuilder11.AppendLine(ref handler);
		AppendCrowdFilterBoundsDebug(stringBuilder, runtimeDefinition);
		AppendCrowdFilterLayerDebug(stringBuilder);
		AppendCrowdFilterMediaDebug(stringBuilder);
		AppendCrowdFilterFrameDebug(stringBuilder, runtimeDefinition, maxFrameSlices);
		return stringBuilder.ToString();
	}

	public void PrintCrowdFilterDebugReport(string label = "", int maxFrameSlices = 256)
	{
		GD.Print(BuildCrowdFilterDebugReport(label, maxFrameSlices));
	}

	internal void GetRuntimeCrowdCullingDebugState(out bool cached, out bool cachedVisible, out bool currentVisible, out bool currentVisibleWithPrefetch, out bool nativeCanvasSuppressed)
	{
		cached = _runtimeRenderBoundsCached;
		cachedVisible = _cachedRuntimeRenderBoundsVisible;
		currentVisible = true;
		currentVisibleWithPrefetch = true;
		nativeCanvasSuppressed = _runtimeNativeCanvasSuppressed;
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (runtimeDefinition == null)
		{
			return;
		}
		Node cachedRenderMountParent = GetCachedRenderMountParent();
		Viewport viewport = ResolveRenderViewport(cachedRenderMountParent);
		if (GodotObject.IsInstanceValid(viewport))
		{
			Rect2 cachedViewportWorldRect = GetCachedViewportWorldRect(viewport);
			if (!(cachedViewportWorldRect.Size.X <= 0f) && !(cachedViewportWorldRect.Size.Y <= 0f))
			{
				Rect2 b = TransformRect(rect: new Rect2(runtimeDefinition.LocalBounds.Position + offset, runtimeDefinition.LocalBounds.Size), transform: GetCachedGlobalTransformForRender());
				currentVisible = cachedViewportWorldRect.Intersects(b, includeBorders: true);
				currentVisibleWithPrefetch = cachedViewportWorldRect.Grow(256f).Intersects(b, includeBorders: true);
			}
		}
	}

	private void AppendCrowdFilterBoundsDebug(StringBuilder sb, AdobeAnimateRuntimeDefinition definition)
	{
		if (definition == null)
		{
			sb.AppendLine("  bounds: <no definition>");
			return;
		}
		Node cachedRenderMountParent = GetCachedRenderMountParent();
		Viewport viewport = ((GodotObject.IsInstanceValid(cachedRenderMountParent) && cachedRenderMountParent is Viewport viewport2) ? viewport2 : GetViewport());
		Rect2 rect = (GodotObject.IsInstanceValid(viewport) ? GetCachedViewportWorldRect(viewport) : default(Rect2));
		Rect2 renderLocalBounds = GetRenderLocalBounds(definition);
		Rect2 rect2 = new Rect2(renderLocalBounds.Position + offset, renderLocalBounds.Size);
		Rect2 rect3 = TransformRect(GetCachedGlobalTransformForRender(), rect2);
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(50, 5, sb);
		handler.AppendLiteral("  bounds local=");
		handler.AppendFormatted(FormatDebugRect(renderLocalBounds));
		handler.AppendLiteral(" offset=");
		handler.AppendFormatted(FormatDebugRect(rect2));
		handler.AppendLiteral(" ");
		handler.AppendLiteral("render=");
		handler.AppendFormatted(FormatDebugRect(rect3));
		handler.AppendLiteral(" viewport=");
		handler.AppendFormatted(FormatDebugRect(rect));
		handler.AppendLiteral(" visible=");
		handler.AppendFormatted(rect.Intersects(rect3, includeBorders: true));
		sb.AppendLine(ref handler);
	}

	private void AppendCrowdFilterLayerDebug(StringBuilder sb)
	{
		sb.AppendLine("  layers:");
		if (_cachedLayerVisibleCount <= 0)
		{
			sb.AppendLine("    <none>");
			return;
		}
		for (int i = 0; i < _cachedLayerVisibleCount; i++)
		{
			bool flag = IsLayerVisibleFromArrayForDebug(i);
			bool flag2 = LayerHasSlicesInCurrentClip(i);
			bool value = flag || !flag2;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(43, 5, sb);
			handler.AppendLiteral("    [");
			handler.AppendFormatted(i);
			handler.AppendLiteral("] ");
			handler.AppendFormatted(GetLayerNameForDebug(i));
			handler.AppendLiteral(" visible=");
			handler.AppendFormatted(flag);
			handler.AppendLiteral(" activeClip=");
			handler.AppendFormatted(flag2);
			handler.AppendLiteral(" renderVisible=");
			handler.AppendFormatted(value);
			sb.AppendLine(ref handler);
		}
	}

	private void AppendCrowdFilterMediaDebug(StringBuilder sb)
	{
		sb.AppendLine("  mediaReplace:");
		if (_cachedMediaReplaceLimit <= 0)
		{
			sb.AppendLine("    <none>");
			return;
		}
		bool flag = false;
		for (int i = 0; i < _cachedMediaReplaceLimit; i++)
		{
			if (_mediaReplaceUse != null && i < _mediaReplaceUse.Count && _mediaReplaceUse[i])
			{
				flag = true;
				Texture2D texture2D = ((_mediaReplace != null && i < _mediaReplace.Count) ? _mediaReplace[i] : null);
				Rect2 rect = ((mediaReplaceRect != null && i < mediaReplaceRect.Count) ? mediaReplaceRect[i] : default(Rect2));
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(32, 5, sb);
				handler.AppendLiteral("    [");
				handler.AppendFormatted(i);
				handler.AppendLiteral("] ");
				handler.AppendFormatted(GetMediaNameForDebug(i));
				handler.AppendLiteral(" texValid=");
				handler.AppendFormatted(GodotObject.IsInstanceValid(texture2D));
				handler.AppendLiteral(" ");
				handler.AppendLiteral("texPath=");
				handler.AppendFormatted(texture2D?.ResourcePath ?? "<null>");
				handler.AppendLiteral(" rect=");
				handler.AppendFormatted(FormatDebugRect(rect));
				sb.AppendLine(ref handler);
			}
		}
		if (!flag)
		{
			sb.AppendLine("    <no active media replacements>");
		}
	}

	private void AppendCrowdFilterFrameDebug(StringBuilder sb, AdobeAnimateRuntimeDefinition definition, int maxFrameSlices)
	{
		sb.AppendLine("  currentFrameSlices:");
		if (definition == null || definition.Frames == null || definition.SliceMetadata == null || definition.Frames.Length == 0)
		{
			sb.AppendLine("    <no definition/frame data>");
			return;
		}
		Image gpuPoseDebugImage = GetGpuPoseDebugImage(definition, out var info);
		StringBuilder stringBuilder = sb;
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(48, 4, stringBuilder);
		handler.AppendLiteral("    poseTexture ");
		handler.AppendFormatted(info);
		handler.AppendLiteral(" baked=");
		handler.AppendFormatted(definition.UsesBakedGpuPoseTexture);
		handler.AppendLiteral(" ");
		handler.AppendLiteral("baseTexel=");
		handler.AppendFormatted(definition.GpuPoseTextureBaseTexel);
		handler.AppendLiteral(" expectedSize=");
		handler.AppendFormatted(definition.GpuPoseTextureSize);
		stringBuilder2.AppendLine(ref handler);
		float num = GetRenderFrameFloat();
		if (clipRange != Vector2I.Zero)
		{
			num = Mathf.Clamp(num, clipRange.X, Math.Max(clipRange.X, clipRange.Y - 1));
		}
		int num2 = Mathf.Clamp(Mathf.FloorToInt(num), 0, definition.Frames.Length - 1);
		float value = Mathf.Clamp(num - (float)num2, 0f, 1f);
		PackedFrame packedFrame = definition.Frames[num2];
		int num3 = Math.Max(0, packedFrame.Count);
		int num4 = ((maxFrameSlices <= 0) ? num3 : Math.Min(num3, maxFrameSlices));
		stringBuilder = sb;
		StringBuilder stringBuilder3 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(41, 5, stringBuilder);
		handler.AppendLiteral("    frame=");
		handler.AppendFormatted(num2);
		handler.AppendLiteral(" alpha=");
		handler.AppendFormatted(value, "F4");
		handler.AppendLiteral(" offset=");
		handler.AppendFormatted(packedFrame.Offset);
		handler.AppendLiteral(" count=");
		handler.AppendFormatted(num3);
		handler.AppendLiteral(" printed=");
		handler.AppendFormatted(num4);
		stringBuilder3.AppendLine(ref handler);
		int num5 = Math.Min(packedFrame.Offset + num4, definition.SliceMetadata.Length);
		int num6 = packedFrame.Offset;
		int num7 = 0;
		while (num6 < num5)
		{
			ref PackedSliceMetadata reference = ref definition.SliceMetadata[num6];
			int layerId = reference.LayerId;
			int mediaId = reference.MediaId;
			bool flag = IsLayerVisibleFromArrayForDebug(layerId);
			bool value2 = flag || !LayerHasSlicesInCurrentClip(layerId);
			bool flag2 = IsMediaReplacedCached(mediaId);
			Rect2 rect = ((mediaId < definition.MediaRects.Length) ? definition.MediaRects[mediaId] : default(Rect2));
			Rect2 rect2 = ((flag2 && mediaReplaceRect != null && mediaId < mediaReplaceRect.Count) ? mediaReplaceRect[mediaId] : default(Rect2));
			int num8 = definition.GpuPoseTextureBaseTexel + num6 * 5;
			bool flag3 = TryReadGpuPoseTexelForDebug(gpuPoseDebugImage, num8 + 1, out var color);
			bool flag4 = TryReadGpuPoseTexelForDebug(gpuPoseDebugImage, num8 + 2, out var color2);
			int value3 = (flag3 ? Mathf.RoundToInt(color.A) : (-1));
			stringBuilder = sb;
			StringBuilder stringBuilder4 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(174, 18, stringBuilder);
			handler.AppendLiteral("    slot=");
			handler.AppendFormatted(num7);
			handler.AppendLiteral(" source=");
			handler.AppendFormatted(num6);
			handler.AppendLiteral(" sliceKey=");
			handler.AppendFormatted(reference.SliceKey);
			handler.AppendLiteral(" layer=");
			handler.AppendFormatted(layerId);
			handler.AppendLiteral(":");
			handler.AppendFormatted(GetLayerNameForDebug(layerId));
			handler.AppendLiteral(" ");
			handler.AppendLiteral("drawOrder=");
			handler.AppendFormatted(reference.DrawOrder);
			handler.AppendLiteral(" visible=");
			handler.AppendFormatted(flag);
			handler.AppendLiteral(" renderVisible=");
			handler.AppendFormatted(value2);
			handler.AppendLiteral(" media=");
			handler.AppendFormatted(mediaId);
			handler.AppendLiteral(":");
			handler.AppendFormatted(GetMediaNameForDebug(mediaId));
			handler.AppendLiteral(" ");
			handler.AppendLiteral("replaced=");
			handler.AppendFormatted(flag2);
			handler.AppendLiteral(" baseRect=");
			handler.AppendFormatted(FormatDebugRect(rect));
			handler.AppendLiteral(" replaceRect=");
			handler.AppendFormatted(FormatDebugRect(rect2));
			handler.AppendLiteral(" alpha=");
			handler.AppendFormatted(flag3 ? color.B : 0f, "F3");
			handler.AppendLiteral(" ");
			handler.AppendLiteral("poseBaseTexel=");
			handler.AppendFormatted(num8);
			handler.AppendLiteral(" poseTransform=");
			handler.AppendFormatted(flag3 ? FormatDebugColor(color) : "<unreadable>");
			handler.AppendLiteral(" ");
			handler.AppendLiteral("poseAtlasLayer=");
			handler.AppendFormatted(value3);
			handler.AppendLiteral(" poseRect=");
			handler.AppendFormatted(flag4 ? FormatDebugColor(color2) : "<unreadable>");
			stringBuilder4.AppendLine(ref handler);
			num6++;
			num7++;
		}
		if (num4 < num3)
		{
			stringBuilder = sb;
			StringBuilder stringBuilder5 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(25, 1, stringBuilder);
			handler.AppendLiteral("    ... truncated ");
			handler.AppendFormatted(num3 - num4);
			handler.AppendLiteral(" slices");
			stringBuilder5.AppendLine(ref handler);
		}
	}

	private static Image GetGpuPoseDebugImage(AdobeAnimateRuntimeDefinition definition, out string info)
	{
		info = "image=<none>";
		if (definition == null)
		{
			info = "image=<no definition>";
			return null;
		}
		Image image = (GodotObject.IsInstanceValid(definition.GpuPoseTexture) ? definition.GpuPoseTexture.GetImage() : null);
		string text = definition.GpuPoseTexture?.ResourcePath ?? "<runtime>";
		if ((image == null || image.IsEmpty()) && definition.UsesGpuPoseTextureArray)
		{
			text = definition.GpuPoseTextureArray?.ResourcePath ?? "<runtime-array>";
			if (GodotObject.IsInstanceValid(definition.GpuPoseTextureArray) && definition.GpuPoseTextureLayer >= 0 && definition.GpuPoseTextureLayer < definition.GpuPoseTextureArray.GetLayers())
			{
				image = definition.GpuPoseTextureArray.GetLayerData(definition.GpuPoseTextureLayer);
			}
		}
		if (image == null || image.IsEmpty())
		{
			info = "image=<empty> texturePath=" + text;
			return null;
		}
		info = $"image=({image.GetWidth()}x{image.GetHeight()} {image.GetFormat()}) texturePath={text}";
		return image;
	}

	private static bool TryReadGpuPoseTexelForDebug(Image image, int texelIndex, out Color color)
	{
		color = default;
		if (image == null || image.IsEmpty() || texelIndex < 0)
		{
			return false;
		}
		int width = image.GetWidth();
		int height = image.GetHeight();
		if (width <= 0 || height <= 0)
		{
			return false;
		}
		int num = texelIndex / width;
		int num2 = texelIndex - num * width;
		if (num < 0 || num >= height || num2 < 0 || num2 >= width)
		{
			return false;
		}
		color = image.GetPixel(num2, num);
		return true;
	}

	private bool IsLayerVisibleFromArrayForDebug(int layerId)
	{
		if (_layerVisible != null && layerId >= 0 && layerId < _layerVisible.Count)
		{
			return _layerVisible[layerId];
		}
		return true;
	}

	private string GetLayerNameForDebug(int layerId)
	{
		if (_flashAnimeData?.layerDictionary == null)
		{
			return "<no-layer-dictionary>";
		}
		foreach (Variant key in _flashAnimeData.layerDictionary.Keys)
		{
			if ((int)_flashAnimeData.layerDictionary[key] == layerId)
			{
				return key.ToString();
			}
		}
		return "<unknown>";
	}

	private string GetMediaNameForDebug(int mediaId)
	{
		if (_flashAnimeData?.mediaDictionary == null)
		{
			return "<no-media-dictionary>";
		}
		foreach (Variant key in _flashAnimeData.mediaDictionary.Keys)
		{
			if ((int)_flashAnimeData.mediaDictionary[key] == mediaId)
			{
				return key.ToString();
			}
		}
		return "<unknown>";
	}

	private static string FormatDebugRect(Rect2 rect)
	{
		return $"({rect.Position.X:F2},{rect.Position.Y:F2},{rect.Size.X:F2},{rect.Size.Y:F2})";
	}

	private static string FormatDebugColor(Color color)
	{
		return $"({color.R:F6},{color.G:F6},{color.B:F6},{color.A:F6})";
	}

	public AdobeAnimateExternalVisualHandle RegisterExternalVisual(Sprite2D sprite, AdobeAnimateExternalVisualDescriptor descriptor)
	{
		if (!GodotObject.IsInstanceValid(sprite))
		{
			return AdobeAnimateExternalVisualHandle.Invalid;
		}
		if (descriptor.AttachmentMode == AdobeAnimateExternalVisualAttachmentMode.Slot && !GodotObject.IsInstanceValid(descriptor.Slot))
		{
			return AdobeAnimateExternalVisualHandle.Invalid;
		}
		Transform2D registeredWorldTransform = sprite.GlobalTransform;
		Transform2D globalTransform = GlobalTransform;
		Transform2D transform2D = ((descriptor.AttachmentMode != AdobeAnimateExternalVisualAttachmentMode.Slot) ? (globalTransform.AffineInverse() * registeredWorldTransform) : (descriptor.Slot.GlobalTransform.AffineInverse() * registeredWorldTransform));
		Transform2D transform = transform2D;
		AdobeAnimateExternalVisualHandle result = _externalVisuals.Register(sprite, in descriptor, in transform, in registeredWorldTransform);
		if (result.IsValid)
		{
			InvalidateExternalVisualTopology();
		}
		return result;
	}

	public bool UnregisterExternalVisual(AdobeAnimateExternalVisualHandle handle)
	{
		if (!_externalVisuals.Unregister(handle))
		{
			return false;
		}
		InvalidateExternalVisualTopology();
		return true;
	}

	public bool SetExternalVisualVisible(AdobeAnimateExternalVisualHandle handle, bool visible)
	{
		return ApplyExternalVisualStateChange(() => _externalVisuals.SetVisible(handle, visible));
	}

	public bool SetExternalVisualTexture(AdobeAnimateExternalVisualHandle handle, Texture2D texture)
	{
		return ApplyExternalVisualStateChange(() => _externalVisuals.SetTexture(handle, texture));
	}

	public bool SetExternalVisualTransform(AdobeAnimateExternalVisualHandle handle, Transform2D transform)
	{
		return ApplyExternalVisualStateChange(() => _externalVisuals.SetTransform(handle, in transform));
	}

	public bool SetExternalVisualModulate(AdobeAnimateExternalVisualHandle handle, Color modulate)
	{
		return ApplyExternalVisualStateChange(() => _externalVisuals.SetModulate(handle, in modulate));
	}

	internal ReadOnlySpan<int> GetActiveExternalVisualIndicesForRender()
	{
		return _externalVisuals.GetActiveIndices();
	}

	internal bool HasExternalVisualsForRender()
	{
		return _externalVisuals.ActiveCount > 0;
	}

	internal void GetExternalVisualDiagnosticStats(out int activeVisible, out int nativeFallback)
	{
		activeVisible = 0;
		nativeFallback = 0;
		AccumulateExternalVisualDiagnosticStats(ref activeVisible, ref nativeFallback, 0);
	}

	private void AccumulateExternalVisualDiagnosticStats(ref int activeVisible, ref int nativeFallback, int depth)
	{
		_externalVisuals.GetDiagnosticStats(out var activeVisible2, out var nativeFallback2);
		activeVisible += activeVisible2;
		nativeFallback += nativeFallback2;
		if (depth >= 8)
		{
			return;
		}
		AdobeAnimateSprite[] spriteChildrenForRender = GetSpriteChildrenForRender();
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildrenForRender)
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.AccumulateExternalVisualDiagnosticStats(ref activeVisible, ref nativeFallback, depth + 1);
			}
		}
	}

	internal bool HasExternalVisualsInOwnedGraphForRender()
	{
		return HasExternalVisualsInOwnedGraphForRender(0);
	}

	private bool HasExternalVisualsInOwnedGraphForRender(int depth)
	{
		if (_externalVisuals.ActiveCount > 0)
		{
			return true;
		}
		if (depth >= 8)
		{
			return false;
		}
		AdobeAnimateSprite[] spriteChildrenForRender = GetSpriteChildrenForRender();
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildrenForRender)
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite) && adobeAnimateSprite.HasExternalVisualsInOwnedGraphForRender(depth + 1))
			{
				return true;
			}
		}
		return false;
	}

	internal ulong GetExternalVisualStateVersionForRender()
	{
		return _externalVisuals.StateVersion;
	}

	internal ulong GetExternalVisualTopologyVersionForRender()
	{
		return _externalVisuals.TopologyVersion;
	}

	internal ulong GetCpuVisualSignatureForRender()
	{
		ulong hash = 1469598103934665603uL;
		AccumulateCpuVisualSignatureForRender(ref hash, 0);
		return hash;
	}

	private void AccumulateCpuVisualSignatureForRender(ref ulong hash, int depth)
	{
		AddCpuVisualSignature(ref hash, _externalVisuals.TopologyVersion);
		AddCpuVisualSignature(ref hash, _externalVisuals.StateVersion);
		AdobeAnimateManagedSlotSprite[] managedSlotSpritesForRender = GetManagedSlotSpritesForRender();
		AddCpuVisualSignature(ref hash, (ulong)managedSlotSpritesForRender.Length);
		for (int i = 0; i < managedSlotSpritesForRender.Length; i++)
		{
			AdobeAnimateManagedSlotSprite pair = managedSlotSpritesForRender[i];
			AddCpuVisualSignature(ref hash, AdobeAnimateManagedSprite2D.BuildTopologySignature(in pair));
			AddCpuVisualSignature(ref hash, AdobeAnimateManagedSprite2D.BuildGpuStateSignature(in pair));
			CanvasItem visual = pair.Visual;
			Material material = (GodotObject.IsInstanceValid(visual) ? visual.Material : null);
			AddCpuVisualSignature(ref hash, GodotObject.IsInstanceValid(material) ? material.GetInstanceId() : 0);
		}
		ReadOnlySpan<int> activeIndices = _externalVisuals.GetActiveIndices();
		AddCpuVisualSignature(ref hash, (ulong)activeIndices.Length);
		for (int j = 0; j < activeIndices.Length; j++)
		{
			if (_externalVisuals.TryGet(activeIndices[j], out var snapshot))
			{
				AddCpuVisualSignature(ref hash, AdobeAnimateManagedSprite2D.BuildExternalGpuStateSignature(in snapshot, snapshot.Transform));
				Material material2 = (GodotObject.IsInstanceValid(snapshot.Sprite) ? snapshot.Sprite.Material : null);
				AddCpuVisualSignature(ref hash, GodotObject.IsInstanceValid(material2) ? material2.GetInstanceId() : 0);
			}
		}
		if (depth >= 8)
		{
			return;
		}
		AdobeAnimateSprite[] spriteChildrenForRender = GetSpriteChildrenForRender();
		AddCpuVisualSignature(ref hash, (ulong)spriteChildrenForRender.Length);
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildrenForRender)
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.AccumulateCpuVisualSignatureForRender(ref hash, depth + 1);
			}
		}
	}

	private static void AddCpuVisualSignature(ref ulong hash, ulong value)
	{
		hash ^= value;
		hash *= 1099511628211uL;
	}

	internal bool TryGetExternalVisualForRender(int visualIndex, out AdobeAnimateExternalVisualSnapshot visual)
	{
		return _externalVisuals.TryGet(visualIndex, out visual);
	}

	internal bool TryGetExternalVisualForRender(AdobeAnimateExternalVisualHandle handle, out AdobeAnimateExternalVisualSnapshot visual)
	{
		return _externalVisuals.TryGet(handle, out visual);
	}

	internal bool TryBuildExternalVisualGpuStateForRender(int visualIndex, Rid expectedAtlasArrayRid, out AdobeAnimateGpuManagedVisualState state, out string failureReason)
	{
		state = default;
		failureReason = "";
		if (!_externalVisuals.TryGet(visualIndex, out var snapshot))
		{
			failureReason = $"external visual index {visualIndex} is out of range";
			return false;
		}
		EnsureExternalVisualGpuStateCache(visualIndex + 1);
		ulong stateVersion = _externalVisuals.StateVersion;
		if (!snapshot.Visible && TryGetCachedExternalVisualGpuState(visualIndex, stateVersion, expectedAtlasArrayRid, out state, out failureReason, out var valid))
		{
			_externalVisuals.MarkPrepared(visualIndex, _externalVisualPreparationFrame, valid);
			return valid;
		}
		Transform2D localTransform = ((snapshot.Descriptor.AttachmentMode == AdobeAnimateExternalVisualAttachmentMode.World) ? (GetCachedGlobalTransformForRender().AffineInverse() * snapshot.RegisteredWorldTransform) : snapshot.Transform);
		ulong num = AdobeAnimateManagedSprite2D.BuildExternalGpuStateSignature(in snapshot, in localTransform);
		if (_externalVisualGpuStateKnown[visualIndex] && _externalVisualGpuStateSignatures[visualIndex] == num && _externalVisualGpuStateVersions[visualIndex] == stateVersion && _externalVisualGpuStateAtlasRids[visualIndex] == expectedAtlasArrayRid)
		{
			state = _externalVisualGpuStates[visualIndex];
			failureReason = _externalVisualGpuStateFailures[visualIndex] ?? "";
			bool flag = _externalVisualGpuStateValid[visualIndex];
			_externalVisuals.MarkPrepared(visualIndex, _externalVisualPreparationFrame, flag);
			return flag;
		}
		bool flag2 = AdobeAnimateManagedSprite2D.TryBuildExternalGpuState(in snapshot, in localTransform, expectedAtlasArrayRid, out state, out failureReason);
		_externalVisualGpuStateSignatures[visualIndex] = num;
		_externalVisualGpuStateVersions[visualIndex] = stateVersion;
		_externalVisualGpuStateAtlasRids[visualIndex] = expectedAtlasArrayRid;
		_externalVisualGpuStates[visualIndex] = state;
		_externalVisualGpuStateFailures[visualIndex] = failureReason;
		_externalVisualGpuStateKnown[visualIndex] = true;
		_externalVisualGpuStateValid[visualIndex] = flag2;
		_externalVisuals.MarkPrepared(visualIndex, _externalVisualPreparationFrame, flag2);
		return flag2;
	}

	private bool TryGetCachedExternalVisualGpuState(int visualIndex, ulong stateVersion, Rid expectedAtlasArrayRid, out AdobeAnimateGpuManagedVisualState state, out string failureReason, out bool valid)
	{
		state = default;
		failureReason = "";
		valid = false;
		if (!_externalVisualGpuStateKnown[visualIndex] || _externalVisualGpuStateVersions[visualIndex] != stateVersion || _externalVisualGpuStateAtlasRids[visualIndex] != expectedAtlasArrayRid)
		{
			return false;
		}
		state = _externalVisualGpuStates[visualIndex];
		failureReason = _externalVisualGpuStateFailures[visualIndex] ?? "";
		valid = _externalVisualGpuStateValid[visualIndex];
		return true;
	}

	private void EnsureExternalVisualGpuStateCache(int count)
	{
		if (_externalVisualGpuStates.Length < count)
		{
			int newSize = Math.Max(count, Math.Max(4, _externalVisualGpuStates.Length * 2));
			System.Array.Resize(ref _externalVisualGpuStateSignatures, newSize);
			System.Array.Resize(ref _externalVisualGpuStateVersions, newSize);
			System.Array.Resize(ref _externalVisualGpuStateAtlasRids, newSize);
			System.Array.Resize(ref _externalVisualGpuStates, newSize);
			System.Array.Resize(ref _externalVisualGpuStateFailures, newSize);
			System.Array.Resize(ref _externalVisualGpuStateKnown, newSize);
			System.Array.Resize(ref _externalVisualGpuStateValid, newSize);
		}
	}

	private void ResetExternalVisualGpuStateCache()
	{
		_externalVisualGpuStateSignatures = System.Array.Empty<ulong>();
		_externalVisualGpuStateVersions = System.Array.Empty<ulong>();
		_externalVisualGpuStateAtlasRids = System.Array.Empty<Rid>();
		_externalVisualGpuStates = System.Array.Empty<AdobeAnimateGpuManagedVisualState>();
		_externalVisualGpuStateFailures = System.Array.Empty<string>();
		_externalVisualGpuStateKnown = System.Array.Empty<bool>();
		_externalVisualGpuStateValid = System.Array.Empty<bool>();
	}

	internal void BeginExternalVisualPreparationForRender(long frameVersion)
	{
		_externalVisualPreparationFrame = frameVersion;
	}

	internal void MarkExternalVisualPreparedForRender(int visualIndex, long frameVersion, bool preparedForCrowd)
	{
		_externalVisualPreparationFrame = frameVersion;
		_externalVisuals.MarkPrepared(visualIndex, frameVersion, preparedForCrowd);
	}

	internal void CommitExternalVisualCrowdFrame(long frameVersion)
	{
		CommitExternalVisualCrowdFrame(frameVersion, 0);
	}

	internal void CommitExternalVisualCpuFrame(long frameVersion, ReadOnlySpan<AdobeAnimateCpuNativeSpriteItem> nativeItems)
	{
		CommitExternalVisualCpuFrame(frameVersion, nativeItems, 0);
	}

	internal void HideExternalVisualCpuFrame(long frameVersion)
	{
		HideExternalVisualCpuFrame(frameVersion, 0);
	}

	internal void RestoreExternalVisualNativeFallbacks(long frameVersion)
	{
		RestoreExternalVisualNativeFallbacks(frameVersion, 0);
	}

	private void CommitExternalVisualCrowdFrame(long frameVersion, int depth)
	{
		_externalVisuals.CommitCrowdFrame(frameVersion);
		if (depth >= 8)
		{
			return;
		}
		AdobeAnimateSprite[] spriteChildren = _spriteChildren;
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildren)
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.CommitExternalVisualCrowdFrame(frameVersion, depth + 1);
			}
		}
	}

	private void CommitExternalVisualCpuFrame(long frameVersion, ReadOnlySpan<AdobeAnimateCpuNativeSpriteItem> nativeItems, int depth)
	{
		_externalVisuals.CommitCpuFrame(frameVersion, nativeItems);
		if (depth >= 8)
		{
			return;
		}
		AdobeAnimateSprite[] spriteChildren = _spriteChildren;
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildren)
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.CommitExternalVisualCpuFrame(frameVersion, nativeItems, depth + 1);
			}
		}
	}

	private void HideExternalVisualCpuFrame(long frameVersion, int depth)
	{
		_externalVisuals.HideCpuFrame(frameVersion);
		if (depth >= 8)
		{
			return;
		}
		AdobeAnimateSprite[] spriteChildren = _spriteChildren;
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildren)
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.HideExternalVisualCpuFrame(frameVersion, depth + 1);
			}
		}
	}

	private void RestoreExternalVisualNativeFallbacks(long frameVersion, int depth)
	{
		_externalVisuals.RestoreNativeFallbacks();
		if (depth >= 8)
		{
			return;
		}
		AdobeAnimateSprite[] spriteChildren = _spriteChildren;
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildren)
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.RestoreExternalVisualNativeFallbacks(frameVersion, depth + 1);
			}
		}
	}

	private bool ApplyExternalVisualStateChange(Func<bool> change)
	{
		ulong stateVersion = _externalVisuals.StateVersion;
		if (!change())
		{
			return false;
		}
		if (_externalVisuals.StateVersion != stateVersion)
		{
			InvalidateRenderSnapshotCache();
			RequestNodeRedraw();
		}
		return true;
	}

	private void InvalidateExternalVisualTopology()
	{
		InvalidateRuntimeNativeCanvasSuppressionEligibility();
		ResetExternalVisualGpuStateCache();
		AdobeAnimateRenderManager.InvalidateGpuRenderGraph(GetRenderSortRootForRender(), AdobeAnimateGpuGraphInvalidationReason.ExternalVisualTopology);
		InvalidateRenderSnapshotCache(force: true);
		RequestNodeRedraw();
	}

	private bool TryBuildGpuGraphCrowdRenderState(AdobeAnimateRuntimeDefinition definition, Node renderMountParent, Transform2D globalTransform, bool enableGpuClock, float animationClockSeconds, AdobeAnimateCrowdRenderState reusableState, out AdobeAnimateCrowdRenderState state)
	{
		state = null;
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
		if (!AdobeAnimateRenderManager.TryResolveGpuRenderGraph(this, out var graph, out var graphOwners, out var allocation, out var textureArray, out var textureSize))
		{
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectStateGpuBuildResolve, startBytes);
			TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.gpuGraph.resolveFailed", in probe);
			return false;
		}
		TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.gpuGraph.resolve", in probe, graphOwners?.Length ?? 0);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectStateGpuBuildResolve, startBytes);
		if (graph == null || graphOwners == null || graphOwners.Length != graph.Owners.Length || graphOwners.Length != allocation.OwnerCount)
		{
			return false;
		}
		long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
		TowerDefensePerfProfiler.SpikeProbe probe2 = TowerDefensePerfProfiler.BeginSpikeProbe();
		float renderFrameFloat = GetRenderFrameFloat();
		int num = ResolveCrowdFrameIndex(definition, renderFrameFloat);
		Transform2D globalTransform2 = AdobeAnimateRenderManager.ToRenderMountLocalTransform(renderMountParent, globalTransform);
		Color modulate = GetEffectiveRenderModulate(renderMountParent);
		int effectiveZIndexForRender = GetEffectiveZIndexForRender();
		TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.gpuGraph.presentation", in probe2, graphOwners.Length);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectStateGpuBuildPresentation, startBytes2);
		long startBytes3 = TowerDefenseAllocationTelemetry.Begin();
		TowerDefensePerfProfiler.SpikeProbe probe3 = TowerDefensePerfProfiler.BeginSpikeProbe();
		AdobeAnimateGpuGraphOwnerState gpuGraphRootOwnerState = (_cachedGpuGraphRootOwnerState = BuildGpuGraphOwnerStateForRender(definition, globalTransform2, modulate, renderFrameFloat, enableGpuClock, animationClockSeconds, _cachedGpuGraphRootOwnerState));
		TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.gpuGraph.rootState", in probe3, graphOwners.Length);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectStateGpuBuildRootState, startBytes3);
		long startBytes4 = TowerDefenseAllocationTelemetry.Begin();
		TowerDefensePerfProfiler.SpikeProbe probe4 = TowerDefensePerfProfiler.BeginSpikeProbe();
		state = ((reusableState != null) ? reusableState.ResetGpuGraph(definition, definition.AtlasTextureArray, (definition.AtlasTextureArraySize.X > 0f && definition.AtlasTextureArraySize.Y > 0f) ? definition.AtlasTextureArraySize : Vector2.One, definition.GpuPoseTextureArray, definition.GpuPoseTextureSize, in allocation, textureArray, in textureSize, renderMountParent, in globalTransform2, in modulate, offset, in _verticalClip, num, ResolveCrowdFrameInterpolation(renderFrameFloat), _cachedAllLayersVisible, _cachedCanUseLayerMask, _cachedLayerMask, _layerVisible, effectiveZIndexForRender, graphOwners, gpuGraphRootOwnerState, BuildRootMotionStateForRender(renderMountParent, globalTransform2)) : AdobeAnimateCrowdRenderState.CreateGpuGraph(definition, definition.AtlasTextureArray, (definition.AtlasTextureArraySize.X > 0f && definition.AtlasTextureArraySize.Y > 0f) ? definition.AtlasTextureArraySize : Vector2.One, definition.GpuPoseTextureArray, definition.GpuPoseTextureSize, allocation, textureArray, textureSize, renderMountParent, globalTransform2, modulate, offset, _verticalClip, num, ResolveCrowdFrameInterpolation(renderFrameFloat), _cachedAllLayersVisible, _cachedCanUseLayerMask, _cachedLayerMask, _layerVisible, effectiveZIndexForRender, graphOwners, gpuGraphRootOwnerState, BuildRootMotionStateForRender(renderMountParent, globalTransform2)));
		TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.gpuGraph.createState", in probe4, graphOwners.Length);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectStateGpuBuildCreateState, startBytes4);
		return state.Mode == AdobeAnimateCrowdRenderMode.GpuGraph;
	}

	internal bool TryGetGpuGraphOwnerDefinitionForRender(out AdobeAnimateRuntimeDefinition definition)
	{
		string failureReason;
		return TryGetGpuGraphOwnerDefinitionForRender(out definition, out failureReason);
	}

	internal bool TryGetGpuGraphOwnerDefinitionForRender(out AdobeAnimateRuntimeDefinition definition, out string failureReason)
	{
		definition = null;
		failureReason = "";
		if (_effectOnceGpuSuppressed)
		{
			failureReason = "owner is internally suppressed by the effect-once GPU batch";
			return false;
		}
		if (!GodotObject.IsInstanceValid(this))
		{
			failureReason = "owner instance is invalid";
			return false;
		}
		if (_flashAnimeData == null)
		{
			failureReason = "owner has no AdobeAnimateData";
			return false;
		}
		if (needMediaReplaceUpdate)
		{
			UpdateMediaReplaceData();
		}
		if (RefreshRenderStaticStateCache())
		{
			_runtimeGpuPrepareRequired = true;
		}
		definition = GetRuntimeDefinition();
		if (!CanUseCrowdShaderPoseDefinition(definition))
		{
			failureReason = "runtime definition has no usable GPU pose texture";
			return false;
		}
		if (!_cachedAllLayersVisible && !_cachedCanUseLayerMask && _cachedLayerVisibleCount <= 0)
		{
			failureReason = "layer visibility cannot be represented by the GPU mask";
			return false;
		}
		if (!CanUseGpuGraphMediaReplace(definition))
		{
			failureReason = "media replacement is not backed by the shared texture array";
			return false;
		}
		return true;
	}

	internal bool TryBuildGpuGraphOwnerStateForRender(Node renderMountParent, bool enableGpuClock, out AdobeAnimateGpuGraphOwnerState state)
	{
		return TryBuildGpuGraphOwnerStateForRender(renderMountParent, enableGpuClock, (float)AdobeAnimateRuntimeManager.AnimationClockSeconds, out state);
	}

	internal bool TryBuildGpuGraphOwnerStateForRender(Node renderMountParent, bool enableGpuClock, float animationClockSeconds, out AdobeAnimateGpuGraphOwnerState state)
	{
		return TryBuildGpuGraphOwnerStateForRender(renderMountParent, enableGpuClock, animationClockSeconds, GetEffectiveRenderModulate(renderMountParent), out state);
	}

	internal bool TryBuildGpuGraphOwnerStateForRender(Node renderMountParent, bool enableGpuClock, float animationClockSeconds, in Color renderModulate, out AdobeAnimateGpuGraphOwnerState state)
	{
		state = null;
		if (!TryGetGpuGraphOwnerDefinitionForRender(out var definition))
		{
			return false;
		}
		float renderFrameFloat = GetRenderFrameFloat();
		Transform2D renderTransform = AdobeAnimateRenderManager.ToRenderMountLocalTransform(renderMountParent, GetCachedGlobalTransformForRender());
		state = BuildGpuGraphOwnerStateForRender(definition, renderTransform, renderModulate, renderFrameFloat, enableGpuClock, animationClockSeconds, _cachedGpuGraphRootOwnerState);
		_cachedGpuGraphRootOwnerState = state;
		return true;
	}

	internal bool TryBuildGpuGraphNestedOwnerStateForRender(Node renderMountParent, bool enableGpuClock, float animationClockSeconds, out AdobeAnimateGpuGraphOwnerState state)
	{
		return TryBuildGpuGraphNestedOwnerStateForRender(renderMountParent, enableGpuClock, animationClockSeconds, GetEffectiveRenderModulate(renderMountParent), out state);
	}

	internal bool TryReusePreparedGpuGraphNestedOwnerStateForRender(AdobeAnimateGpuGraphOwnerState preparedState, float animationClockSeconds, in Color renderModulate, out AdobeAnimateGpuGraphOwnerState state)
	{
		state = null;
		if (!_hasCachedGpuGraphNestedOwnerState || preparedState != _cachedGpuGraphNestedOwnerState)
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshNestedRejectCache);
			return false;
		}
		bool flag = _renderStaticStateNextAuditPhysicsFrame != 18446744073709551615uL && AdobeAnimateRenderManager.GetPhysicsFrameForRender() >= _renderStaticStateNextAuditPhysicsFrame;
		if ((_gpuGraphCrowdStaticStateDirty || animationClockSeconds >= _gpuGraphNestedOwnerStateRebaseClockSeconds) | flag)
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshNestedRejectSchedule);
			return false;
		}
		if (!_runtimeGpuClockInterpolationActive || _effectOnceGpuSuppressed || needMediaReplaceUpdate || !_runtimeInsideTree || _flashAnimeData == null || !_renderStaticStateCached)
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshNestedRejectRuntime);
			return false;
		}
		if (_cachedLayerStateVersion != _layerStateVersion || _cachedMediaReplaceStateVersion != _mediaReplaceStateVersion || _cachedGpuGraphNestedOwnerPlaybackRevision != _playbackRevision)
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshNestedRejectVersion);
			return false;
		}
		if (preparedState.Visible != _isVisibleInTree || preparedState.Modulate != renderModulate || !SameGpuHitFlashState(preparedState.GpuBrightFlash, _gpuBrightFlash) || !SameGpuHitFlashState(preparedState.GpuWhiteFlash, _gpuWhiteFlash) || IsClipBlendActiveForRender())
		{
			TowerDefenseAllocationTelemetry.Sample(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshNestedRejectPresentation);
			return false;
		}
		state = preparedState;
		return true;
	}

	internal bool TryBuildGpuGraphNestedOwnerStateForRender(Node renderMountParent, bool enableGpuClock, float animationClockSeconds, in Color renderModulate, out AdobeAnimateGpuGraphOwnerState state)
	{
		state = null;
		if (TryReuseGpuGraphNestedOwnerStateForRender(enableGpuClock, animationClockSeconds, in renderModulate, out state))
		{
			_gpuGraphCrowdStaticStateDirty = false;
			return true;
		}
		if (!TryGetGpuGraphOwnerDefinitionForRender(out var definition))
		{
			return false;
		}
		state = BuildGpuGraphOwnerStateForRender(definition, Transform2D.Identity, renderModulate, GetRenderFrameFloat(), enableGpuClock, animationClockSeconds, _cachedGpuGraphNestedOwnerState);
		CacheGpuGraphNestedOwnerStateForRender(state, animationClockSeconds);
		return true;
	}

	private bool TryReuseGpuGraphNestedOwnerStateForRender(bool enableGpuClock, float animationClockSeconds, in Color renderModulate, out AdobeAnimateGpuGraphOwnerState state)
	{
		state = null;
		if (!_hasCachedGpuGraphNestedOwnerState || !enableGpuClock || !_runtimeGpuClockInterpolationActive || _effectOnceGpuSuppressed || needMediaReplaceUpdate || !_runtimeInsideTree || _flashAnimeData == null || !_renderStaticStateCached || _cachedLayerStateVersion != _layerStateVersion || _cachedMediaReplaceStateVersion != _mediaReplaceStateVersion || _cachedGpuGraphNestedOwnerPlaybackRevision != _playbackRevision || IsClipBlendActiveForRender())
		{
			return false;
		}
		if (_renderStaticStateNextAuditPhysicsFrame != 18446744073709551615uL && AdobeAnimateRenderManager.GetPhysicsFrameForRender() >= _renderStaticStateNextAuditPhysicsFrame && RefreshRenderStaticStateCache())
		{
			return false;
		}
		AdobeAnimateGpuGraphOwnerState cachedGpuGraphNestedOwnerState = _cachedGpuGraphNestedOwnerState;
		AdobeAnimateRuntimeDefinition adobeAnimateRuntimeDefinition = cachedGpuGraphNestedOwnerState?.Definition;
		if (cachedGpuGraphNestedOwnerState == null || adobeAnimateRuntimeDefinition == null || !CanUseCrowdShaderPoseDefinition(adobeAnimateRuntimeDefinition) || _runtimeDefinition != adobeAnimateRuntimeDefinition || cachedGpuGraphNestedOwnerState.StaticState != _cachedGpuGraphOwnerStateTemplate || !IsGpuGraphOwnerStateTemplateCurrentForRender(cachedGpuGraphNestedOwnerState.StaticState, adobeAnimateRuntimeDefinition) || cachedGpuGraphNestedOwnerState.Visible != _isVisibleInTree || !SameGpuHitFlashState(cachedGpuGraphNestedOwnerState.GpuBrightFlash, _gpuBrightFlash) || !SameGpuHitFlashState(cachedGpuGraphNestedOwnerState.GpuWhiteFlash, _gpuWhiteFlash) || !CanUseGpuGraphMediaReplace(adobeAnimateRuntimeDefinition))
		{
			return false;
		}
		float renderFrameFloat = GetRenderFrameFloat();
		AdobeAnimateGpuClockState gpuClock = BuildRuntimeGpuClockState(renderFrameFloat, animationClockSeconds);
		if (!SameGpuClockConfiguration(cachedGpuGraphNestedOwnerState.GpuClock, gpuClock) || !GpuClockPhaseMatches(cachedGpuGraphNestedOwnerState.GpuClock, animationClockSeconds, renderFrameFloat))
		{
			return false;
		}
		if (animationClockSeconds >= _gpuGraphNestedOwnerStateRebaseClockSeconds)
		{
			state = cachedGpuGraphNestedOwnerState.ResetDynamicState(cachedGpuGraphNestedOwnerState.StaticState, cachedGpuGraphNestedOwnerState.GlobalTransform, in renderModulate, cachedGpuGraphNestedOwnerState.FrameIndex, cachedGpuGraphNestedOwnerState.InterpolationT, _isVisibleInTree, cachedGpuGraphNestedOwnerState.ClipBlend, in gpuClock, in _gpuBrightFlash, in _gpuWhiteFlash);
			CacheGpuGraphNestedOwnerStateForRender(state, animationClockSeconds);
			return true;
		}
		state = ((cachedGpuGraphNestedOwnerState.Modulate == renderModulate) ? cachedGpuGraphNestedOwnerState : cachedGpuGraphNestedOwnerState.WithModulate(renderModulate));
		_cachedGpuGraphNestedOwnerState = state;
		return true;
	}

	private void CacheGpuGraphNestedOwnerStateForRender(AdobeAnimateGpuGraphOwnerState state, float animationClockSeconds)
	{
		if (state == null)
		{
			_cachedGpuGraphNestedOwnerState = null;
			_hasCachedGpuGraphNestedOwnerState = false;
			return;
		}
		_cachedGpuGraphNestedOwnerState = state;
		if (!state.GpuClock.Enabled || state.ClipBlend.Enabled)
		{
			_hasCachedGpuGraphNestedOwnerState = false;
			return;
		}
		_cachedGpuGraphNestedOwnerPlaybackRevision = _playbackRevision;
		_gpuGraphCrowdStaticStateDirty = false;
		_gpuGraphNestedOwnerStateRebaseClockSeconds = animationClockSeconds + 1f + (float)(GetCachedInstanceIdForRender() % 60) / 60f;
		_hasCachedGpuGraphNestedOwnerState = true;
	}

	private static bool SameGpuClockConfiguration(AdobeAnimateGpuClockState cached, AdobeAnimateGpuClockState current)
	{
		if (cached.Enabled && current.Enabled && Mathf.IsEqualApprox(cached.FramesPerSecond, current.FramesPerSecond) && Mathf.IsEqualApprox(cached.ClipStart, current.ClipStart) && Mathf.IsEqualApprox(cached.ClipEndExclusive, current.ClipEndExclusive))
		{
			return cached.Loop == current.Loop;
		}
		return false;
	}

	private static bool GpuClockPhaseMatches(AdobeAnimateGpuClockState cached, float animationClockSeconds, float currentFrameFloat)
	{
		float num = Math.Max(1f, cached.ClipEndExclusive - cached.ClipStart);
		float num2 = cached.StartFrame + (animationClockSeconds - cached.StartTime) * cached.FramesPerSecond;
		if (cached.Loop)
		{
			num2 = cached.ClipStart + PositiveModulo(num2 - cached.ClipStart, num);
			currentFrameFloat = cached.ClipStart + PositiveModulo(currentFrameFloat - cached.ClipStart, num);
			float num3 = Math.Abs(num2 - currentFrameFloat);
			num3 = Math.Min(num3, num - num3);
			return num3 <= 0.001f;
		}
		num2 = Mathf.Clamp(num2, cached.ClipStart, Math.Max(cached.ClipStart, cached.ClipEndExclusive - 1f));
		return Math.Abs(num2 - currentFrameFloat) <= 0.001f;
	}

	private static float PositiveModulo(float value, float modulus)
	{
		float num = value % modulus;
		if (!(num < 0f))
		{
			return num;
		}
		return num + modulus;
	}

	private static bool SameGpuHitFlashState(AdobeAnimateGpuHitFlashState left, AdobeAnimateGpuHitFlashState right)
	{
		if (left.Enabled == right.Enabled && Mathf.IsEqualApprox(left.StartTime, right.StartTime) && Mathf.IsEqualApprox(left.Strength, right.Strength))
		{
			return Mathf.IsEqualApprox(left.Duration, right.Duration);
		}
		return false;
	}

	internal bool TryBuildGpuGraphOwnerStateForRender(Node renderMountParent, out AdobeAnimateGpuGraphOwnerState state)
	{
		return TryBuildGpuGraphOwnerStateForRender(renderMountParent, enableGpuClock: true, out state);
	}

	private AdobeAnimateGpuGraphOwnerState BuildGpuGraphOwnerStateForRender(AdobeAnimateRuntimeDefinition definition, Transform2D renderTransform, Color renderModulate, float frameFloat, bool enableGpuClock, float animationClockSeconds, AdobeAnimateGpuGraphOwnerState reusableState = null)
	{
		AdobeAnimateGpuClockState gpuClock = (enableGpuClock ? BuildRuntimeGpuClockState(frameFloat, animationClockSeconds) : default(AdobeAnimateGpuClockState));
		CacheRuntimeGpuClockConfiguration(gpuClock);
		AdobeAnimateGpuGraphOwnerStateTemplate orCreateGpuGraphOwnerStateTemplateForRender = GetOrCreateGpuGraphOwnerStateTemplateForRender(definition);
		int num = 0;
		float interpolationT = 0f;
		if (!gpuClock.Enabled)
		{
			num = ResolveCrowdFrameIndex(definition, frameFloat);
			interpolationT = ResolveCrowdFrameInterpolation(frameFloat);
		}
		AdobeAnimateClipBlendState clipBlend = GetClipBlendStateForRender();
		return reusableState?.ResetDynamicState(orCreateGpuGraphOwnerStateTemplateForRender, in renderTransform, in renderModulate, num, interpolationT, _isVisibleInTree, in clipBlend, in gpuClock, in _gpuBrightFlash, in _gpuWhiteFlash) ?? new AdobeAnimateGpuGraphOwnerState(orCreateGpuGraphOwnerStateTemplateForRender, renderTransform, renderModulate, num, interpolationT, _isVisibleInTree, clipBlend, gpuClock, _gpuBrightFlash, _gpuWhiteFlash);
	}

	private AdobeAnimateGpuGraphOwnerStateTemplate GetOrCreateGpuGraphOwnerStateTemplateForRender(AdobeAnimateRuntimeDefinition definition)
	{
		AdobeAnimateGpuGraphOwnerStateTemplate cachedGpuGraphOwnerStateTemplate = _cachedGpuGraphOwnerStateTemplate;
		if (IsGpuGraphOwnerStateTemplateCurrentForRender(cachedGpuGraphOwnerStateTemplate, definition))
		{
			return cachedGpuGraphOwnerStateTemplate;
		}
		int layerCount = Math.Max(_cachedLayerVisibleCount, definition?.RuntimeLayerCount ?? 0);
		int mediaCount = Math.Max(_cachedMediaReplaceLimit, (definition?.MediaRects?.Length).GetValueOrDefault());
		return _cachedGpuGraphOwnerStateTemplate = new AdobeAnimateGpuGraphOwnerStateTemplate(this, definition, offset, _verticalClip, _cachedAllLayersVisible, _cachedCanUseLayerMask, _cachedLayerMask, _layerVisible, layerCount, _cachedHasMediaReplace, mediaReplaceRect, _mediaReplaceUse, mediaReplaceAtlasPages, mediaReplaceAtlasArraySize, mediaCount, _cachedMediaReplaceSignature);
	}

	private bool IsGpuGraphOwnerStateTemplateCurrentForRender(AdobeAnimateGpuGraphOwnerStateTemplate cached, AdobeAnimateRuntimeDefinition definition)
	{
		int num = Math.Max(_cachedLayerVisibleCount, definition?.RuntimeLayerCount ?? 0);
		int num2 = Math.Max(_cachedMediaReplaceLimit, (definition?.MediaRects?.Length).GetValueOrDefault());
		if (cached != null && cached.SourceSprite == this && cached.Definition == definition && cached.Offset == offset && SameVerticalClip(cached.VerticalClip, _verticalClip) && cached.AllLayersVisible == _cachedAllLayersVisible && cached.CanUseLayerMask == _cachedCanUseLayerMask && cached.LayerMask == _cachedLayerMask && cached.LayerVisible == _layerVisible && cached.LayerCount == num && cached.HasMediaReplace == _cachedHasMediaReplace && cached.MediaReplaceRect == mediaReplaceRect && cached.MediaReplaceUse == _mediaReplaceUse && cached.MediaReplaceAtlasPages == mediaReplaceAtlasPages && cached.MediaReplaceAtlasSize == mediaReplaceAtlasArraySize && cached.MediaCount == num2)
		{
			return cached.MediaReplaceSignature == _cachedMediaReplaceSignature;
		}
		return false;
	}

	private static bool SameVerticalClip(VerticalClipState left, VerticalClipState right)
	{
		if (left.Enabled == right.Enabled && Mathf.IsEqualApprox(left.UpY, right.UpY))
		{
			return Mathf.IsEqualApprox(left.DownY, right.DownY);
		}
		return false;
	}

	private void CacheRuntimeGpuClockConfiguration(AdobeAnimateGpuClockState gpuClock)
	{
		bool num = _runtimeGpuClockInterpolationActive != gpuClock.Enabled || !Mathf.IsEqualApprox(_runtimeGpuClockFramesPerSecond, gpuClock.FramesPerSecond) || !Mathf.IsEqualApprox(_runtimeGpuClockClipStart, gpuClock.ClipStart) || !Mathf.IsEqualApprox(_runtimeGpuClockClipEndExclusive, gpuClock.ClipEndExclusive) || _runtimeGpuClockLoop != gpuClock.Loop;
		_runtimeGpuClockInterpolationActive = gpuClock.Enabled;
		_runtimeGpuClockFramesPerSecond = gpuClock.FramesPerSecond;
		_runtimeGpuClockClipStart = gpuClock.ClipStart;
		_runtimeGpuClockClipEndExclusive = gpuClock.ClipEndExclusive;
		_runtimeGpuClockLoop = gpuClock.Loop;
		if (num)
		{
			InvalidateRuntimeDisplayVisualRequirementCache();
		}
	}

	private bool CanReuseRuntimeGpuClockPose()
	{
		if (_runtimeGpuClockPoseReuseCached && _runtimeGpuClockPoseReuseValidationCountdown > 0)
		{
			_runtimeGpuClockPoseReuseValidationCountdown--;
			return _cachedRuntimeGpuClockPoseReusable;
		}
		bool flag = ComputeCanReuseRuntimeGpuClockPose();
		_runtimeGpuClockPoseReuseCached = true;
		_cachedRuntimeGpuClockPoseReusable = flag;
		_runtimeGpuClockPoseReuseValidationCountdown = 8;
		return flag;
	}

	private bool ComputeCanReuseRuntimeGpuClockPose()
	{
		if (!_runtimeGpuClockInterpolationActive)
		{
			return false;
		}
		int x = clipRange.X;
		int playbackClipEndExclusive = GetPlaybackClipEndExclusive();
		double num = frameRate * Math.Abs(timeScale) * (playBack ? (-1.0) : 1.0);
		if (_usingRuntimeManager && !IsPlaybackStopped && _runtimeManagerDispatchActive && !clipOver && _nextAnimDelayTimer <= 0.0 && playbackClipEndExclusive - x > 1 && Math.Abs(num) > 1E-06 && Mathf.IsEqualApprox((float)num, _runtimeGpuClockFramesPerSecond) && Mathf.IsEqualApprox(x, _runtimeGpuClockClipStart) && Mathf.IsEqualApprox(playbackClipEndExclusive, _runtimeGpuClockClipEndExclusive))
		{
			return loop == _runtimeGpuClockLoop;
		}
		return false;
	}

	internal bool RequiresDisplayFrameVisualTick()
	{
		if (_effectOnceGpuSuppressed)
		{
			return false;
		}
		if (_runtimeDisplayVisualRequirementCached && _runtimeDisplayVisualRequirementValidationCountdown > 0)
		{
			_runtimeDisplayVisualRequirementValidationCountdown--;
			_displayFrameGpuClockPoseReusable = _cachedRuntimeDisplayGpuClockPoseReusable;
			return _cachedRuntimeDisplayVisualRequirement;
		}
		bool flag = ComputeRequiresDisplayFrameVisualTick();
		_runtimeDisplayVisualRequirementCached = true;
		_cachedRuntimeDisplayVisualRequirement = flag;
		_cachedRuntimeDisplayGpuClockPoseReusable = _displayFrameGpuClockPoseReusable;
		_runtimeDisplayVisualRequirementValidationCountdown = 30;
		return flag;
	}

	private bool ComputeRequiresDisplayFrameVisualTick()
	{
		_displayFrameGpuClockPoseReusable = false;
		if (CanReuseCachedCulledPose())
		{
			return false;
		}
		if (RequiresDisplayFrameBatchedProcessExtension)
		{
			_displayFrameGpuClockPoseReusable = CanReuseRuntimeGpuClockPose();
			return true;
		}
		if (IsClipBlendActiveForRender())
		{
			return true;
		}
		_displayFrameGpuClockPoseReusable = CanReuseRuntimeGpuClockPose();
		if (!_displayFrameGpuClockPoseReusable)
		{
			return true;
		}
		return RequiresDisplayCadenceRuntimeChildUpdate();
	}

	private void InvalidateRuntimeDisplayVisualRequirementCache()
	{
		_runtimeDisplayVisualRequirementCached = false;
		_runtimeDisplayVisualRequirementValidationCountdown = 0;
		_runtimeGpuClockPoseReuseCached = false;
		_runtimeGpuClockPoseReuseValidationCountdown = 0;
	}

	private bool RequiresDisplayCadenceRuntimeChildUpdate()
	{
		if (!HasRuntimeChildUpdates() || ShouldSkipRuntimeChildUpdateForViewport())
		{
			return false;
		}
		return Mathf.Max(10.0, trueFrameRate) > 30.0;
	}

	private void RequestAnimationPoseRedraw()
	{
		if ((IsClipBlendActiveForRender() || !CanReuseRuntimeGpuClockPose()) && !CanReuseCachedCulledPose())
		{
			RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, staticStateDirty: false);
		}
	}

	private bool CanReuseCachedCulledPose()
	{
		AdobeAnimateSprite adobeAnimateSprite = ((_parentSprite == null) ? this : GetRenderSortRootForRender());
		if (adobeAnimateSprite == null || !adobeAnimateSprite.IsRuntimeInsideTree)
		{
			return false;
		}
		if (adobeAnimateSprite._usingRuntimeManager && adobeAnimateSprite._flashAnimeData != null && adobeAnimateSprite._isVisibleInTree && adobeAnimateSprite._runtimeRenderBoundsCached && !adobeAnimateSprite._cachedRuntimeRenderBoundsVisible && !adobeAnimateSprite._renderGlobalTransformDirty && adobeAnimateSprite._renderGlobalTransformCached && adobeAnimateSprite._cachedRuntimeRenderBoundsTransform == adobeAnimateSprite._renderGlobalTransform && adobeAnimateSprite._cachedRuntimeRenderBoundsOffset == adobeAnimateSprite.offset && adobeAnimateSprite._cachedRuntimeRenderBoundsDefinition == adobeAnimateSprite._runtimeDefinition)
		{
			return adobeAnimateSprite._cachedRuntimeRenderBoundsMountParent == adobeAnimateSprite._cachedRenderMountParent;
		}
		return false;
	}

	internal void InvalidateRuntimeCrowdCullingForRefresh()
	{
		_runtimeRenderBoundsRescanCountdown = 0;
		InvalidateRuntimeDisplayVisualRequirementCache();
	}

	internal bool InvalidateRuntimeCrowdCullingIfCachedOffscreen()
	{
		if (!_runtimeRenderBoundsCached || _cachedRuntimeRenderBoundsVisible)
		{
			return false;
		}
		if (!HasRuntimeCrowdViewportChanged())
		{
			return false;
		}
		_runtimeRenderBoundsRescanCountdown = 0;
		InvalidateRuntimeDisplayVisualRequirementCache();
		return true;
	}

	private bool HasRuntimeCrowdViewportChanged()
	{
		if (!_runtimeViewportCullingEnabled)
		{
			return false;
		}
		Viewport viewport = ResolveRenderViewport(_cachedRuntimeRenderBoundsMountParent);
		if (!GodotObject.IsInstanceValid(viewport) || !GodotObject.IsInstanceValid(_cachedRuntimeRenderBoundsViewport) || viewport != _cachedRuntimeRenderBoundsViewport)
		{
			return true;
		}
		return GetCachedViewportWorldRect(viewport) != _cachedRuntimeRenderBoundsViewportWorldRect;
	}

	internal void DisableRuntimeGpuClockInterpolationForOwnedGraph()
	{
		if (_runtimeGpuClockInterpolationActive)
		{
			InvalidateRuntimeDisplayVisualRequirementCache();
		}
		_runtimeGpuClockInterpolationActive = false;
		for (int i = 0; i < _spriteChildren.Length; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _spriteChildren[i];
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.DisableRuntimeGpuClockInterpolationForOwnedGraph();
			}
		}
	}

	internal void InvalidateRuntimeRenderAfterGpuCacheReset()
	{
		InvalidateStaticAtlasPathLayoutForNodeMutation(markRuntimeMutation: false);
		_staticAtlasPathLayoutGpuResetPending = true;
		needMediaReplaceUpdate = true;
		_displayFrameGpuClockPoseReusable = false;
		DisableRuntimeGpuClockInterpolationForOwnedGraph();
		InvalidateRuntimeCrowdCullingForRefresh();
		InvalidateRenderSnapshotCache(force: true);
		_needsRenderSubmission = true;
	}

	private AdobeAnimateGpuClockState BuildRuntimeGpuClockState(float frameFloat, float animationClockSeconds)
	{
		int x = clipRange.X;
		int playbackClipEndExclusive = GetPlaybackClipEndExclusive();
		double num = frameRate * Math.Abs(timeScale) * (playBack ? (-1.0) : 1.0);
		return new AdobeAnimateGpuClockState(_usingRuntimeManager && !IsPlaybackStopped && _runtimeManagerDispatchActive && !clipOver && _nextAnimDelayTimer <= 0.0 && playbackClipEndExclusive - x > 1 && Math.Abs(num) > 1E-06, animationClockSeconds, frameFloat, (float)num, x, playbackClipEndExclusive, loop);
	}

	private bool CanUseGpuGraphMediaReplace(AdobeAnimateRuntimeDefinition definition)
	{
		if (!_cachedAnyRequestedMediaReplace)
		{
			return !_cachedHasMediaReplace;
		}
		if (!_cachedHasMediaReplace || !mediaReplaceAtlasShared || !mediaReplaceAtlasUsesTextureArray || !GodotObject.IsInstanceValid(mediaReplaceAtlasArray) || definition == null || !definition.AtlasTextureArrayRid.IsValid)
		{
			return false;
		}
		if (_cachedMediaReplaceAtlasArrayRid == definition.AtlasTextureArrayRid && mediaReplaceAtlasArraySize.X > 0f)
		{
			return mediaReplaceAtlasArraySize.Y > 0f;
		}
		return false;
	}

	private bool HasAnyRequestedMediaReplace()
	{
		Array<bool> array = _mediaReplaceUse;
		int num = array?.Count ?? 0;
		if (num <= 0)
		{
			return false;
		}
		for (int i = 0; i < num; i++)
		{
			if (array[i])
			{
				return true;
			}
		}
		return false;
	}

	internal bool TryGetGpuGraphChildAttachmentForRender(AdobeAnimateSprite child, out AdobeAnimateGpuAttachmentKind attachmentKind, out int attachmentKey)
	{
		attachmentKind = AdobeAnimateGpuAttachmentKind.Root;
		attachmentKey = -1;
		if (!TryGetGpuGraphChildAttachmentForRender(child, out var attachment))
		{
			return false;
		}
		attachmentKind = attachment.Kind;
		attachmentKey = attachment.Key;
		return true;
	}

	internal bool TryGetGpuGraphChildAttachmentForRender(AdobeAnimateSprite child, out AdobeAnimateGpuAttachmentSettings attachment)
	{
		attachment = default;
		if (!GodotObject.IsInstanceValid(child))
		{
			return false;
		}
		AdobeAnimateSprite[] spriteChildrenForRender = GetSpriteChildrenForRender();
		for (int i = 0; i < spriteChildrenForRender.Length; i++)
		{
			if (spriteChildrenForRender[i] != child)
			{
				continue;
			}
			int num = ResolveSpriteChildFollowLayer(child, i);
			if (num < 0)
			{
				return false;
			}
			if (IsSpriteChildOwnedBySlot(i))
			{
				AdobeAnimateSlot adobeAnimateSlot = _spriteChildOwnerSlots[i];
				if (!GodotObject.IsInstanceValid(adobeAnimateSlot) || adobeAnimateSlot.mode != 0)
				{
					return false;
				}
				attachment = new AdobeAnimateGpuAttachmentSettings(AdobeAnimateGpuAttachmentKind.Slot, num, -1, usePos: true, adobeAnimateSlot.useRotate, adobeAnimateSlot.useScale, adobeAnimateSlot.useSkew, offset, adobeAnimateSlot.offset, 0f, adobeAnimateSlot.useFollowVisible, child.Transform);
				return true;
			}
			Transform2D localTransform = new Transform2D(child.useRotate ? 0f : child.Rotation, child.Scale, child.Skew, child.usePos ? Vector2.Zero : child.Position);
			attachment = new AdobeAnimateGpuAttachmentSettings(AdobeAnimateGpuAttachmentKind.FollowLayer, num, -1, child.usePos, child.useRotate, useScale: false, useSkew: false, offset, Vector2.Zero, (float)child.offsetRotate, child.useFollowVisible, localTransform);
			return true;
		}
		return false;
	}

	public void UpdateChild()
	{
		if (!_hasChildren || _flashAnimeData == null)
		{
			return;
		}
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (runtimeDefinition == null || runtimeDefinition.Frames == null || runtimeDefinition.Frames.Length == 0)
		{
			return;
		}
		bool cachedEditorHint = CachedEditorHint;
		AdobeAnimateSlot[] array = (cachedEditorHint ? _slotChildren : GetRuntimeSlotChildren());
		int i;
		for (i = 0; i < array.Length; i++)
		{
			AdobeAnimateSlot adobeAnimateSlot = array[i];
			if (adobeAnimateSlot == null || !adobeAnimateSlot.IsRuntimeInsideTree || adobeAnimateSlot.mode != 0)
			{
				continue;
			}
			int slotLayerId = adobeAnimateSlot.followSlotId - 1;
			if (!TryGetInterpolatedSlotPoseFast(slotLayerId, out var mediaId, out var transform) || mediaId == 65535 || transform == Transform2D.Identity)
			{
				if (adobeAnimateSlot.useFollowVisible)
				{
					adobeAnimateSlot.ApplyRuntimeFollowVisibility(visible: false);
				}
				continue;
			}
			Transform2D transform2 = transform.Translated(offset).TranslatedLocal(adobeAnimateSlot.offset);
			if (!adobeAnimateSlot.useRotate)
			{
				transform2 = transform2.RotatedLocal(0f - transform2.Rotation);
			}
			if (!adobeAnimateSlot.useScale)
			{
				Vector2 scale = transform2.Scale;
				if (scale.X != 0f && scale.Y != 0f)
				{
					transform2 = transform2.ScaledLocal(Vector2.One / scale);
				}
			}
			adobeAnimateSlot.ApplyRuntimeTransform(transform2);
			if (!adobeAnimateSlot.useSkew)
			{
				adobeAnimateSlot.Skew = 0f;
			}
			if (adobeAnimateSlot.useFollowVisible)
			{
				adobeAnimateSlot.ApplyRuntimeFollowVisibility(visible: true);
			}
		}
		for (int j = 0; j < _spriteChildren.Length; j++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _spriteChildren[j];
			if (adobeAnimateSprite == null || !adobeAnimateSprite.IsRuntimeInsideTree)
			{
				continue;
			}
			if (!cachedEditorHint)
			{
				adobeAnimateSprite.ApplyRuntimeParentState(this);
			}
			bool isInserted = TryGetInsertedLayerForChild(adobeAnimateSprite, out var layerId);
			int num = ResolveSpriteChildRenderLayer(adobeAnimateSprite, j, isInserted, layerId);
			int num2 = ResolveSpriteChildFollowLayer(adobeAnimateSprite, j);
			if (num < 0 && num2 < 0)
			{
				if (adobeAnimateSprite.useFollowVisible)
				{
					adobeAnimateSprite.ApplyRuntimeFollowVisibility(visible: false);
				}
				continue;
			}
			if (IsSpriteChildOwnedBySlot(j))
			{
				if (cachedEditorHint && num >= 0)
				{
					ApplyChildRenderSort(adobeAnimateSprite, num);
				}
				continue;
			}
			if (num2 < 0 || (!TryGetInterpolatedLayerPose(num2, out i, out var transform3) && !TryGetInterpolatedDrawOrderPose(num2, out i, out transform3)))
			{
				if (adobeAnimateSprite.useFollowVisible)
				{
					adobeAnimateSprite.ApplyRuntimeFollowVisibility(visible: false);
				}
				continue;
			}
			if (transform3 == Transform2D.Identity)
			{
				if (adobeAnimateSprite.useFollowVisible)
				{
					adobeAnimateSprite.ApplyRuntimeFollowVisibility(visible: false);
				}
				continue;
			}
			if (adobeAnimateSprite.useFollowVisible)
			{
				adobeAnimateSprite.ApplyRuntimeFollowVisibility(visible: true);
			}
			if (adobeAnimateSprite.usePos)
			{
				bool flag = false;
				Vector2 vector = transform3.Origin + offset;
				if (adobeAnimateSprite.Position != vector)
				{
					adobeAnimateSprite.Position = vector;
					flag = true;
				}
				if (adobeAnimateSprite.useRotate)
				{
					float num3 = (float)((double)transform3.Rotation + adobeAnimateSprite.offsetRotate);
					if (!Mathf.IsEqualApprox(adobeAnimateSprite.Rotation, num3))
					{
						adobeAnimateSprite.Rotation = num3;
						flag = true;
					}
				}
				if (flag)
				{
					adobeAnimateSprite.NotifyAncestorTransformChangedForRender();
				}
			}
			if (cachedEditorHint && num >= 0)
			{
				ApplyChildRenderSort(adobeAnimateSprite, num);
			}
		}
	}

	private void EmitFrameEvents(int frameIdx)
	{
		if (_flashAnimeData == null || frameIdx < 0)
		{
			return;
		}
		HashSet<int> hashSet = _flashAnimeData.EventFrameIndices;
		if (hashSet == null)
		{
			hashSet = new HashSet<int>();
			int count = _flashAnimeData.events.Count;
			for (int i = 0; i < count; i++)
			{
				Godot.Collections.Array array = _flashAnimeData.events[i];
				if (array != null && array.Count > 0)
				{
					hashSet.Add(i);
				}
			}
			_flashAnimeData.EventFrameIndices = hashSet;
		}
		if (!hashSet.Contains(frameIdx))
		{
			return;
		}
		if (frameIdx >= _flashAnimeData.events.Count)
		{
			hashSet.Remove(frameIdx);
			return;
		}
		Godot.Collections.Array array2 = _flashAnimeData.events[frameIdx];
		if (array2 == null || array2.Count <= 0)
		{
			return;
		}
		foreach (Variant item in array2)
		{
			Dictionary dictionary = (Dictionary)item;
			OnAnimeEvent?.Invoke(dictionary["Command"].AsString(), dictionary["Argument"]);
		}
	}

	private void CacheChildren()
	{
		OwnedChildBinding[] bindings = ArrayPool<OwnedChildBinding>.Shared.Rent(Math.Max(4, GetChildCount()));
		int bindingCount = 0;
		int slotCount = 0;
		int spriteCount = 0;
		try
		{
			CollectOwnedChildBindings(this, collectSpriteChildren: true, null, !IsInsideTree(), ref bindings, ref bindingCount, ref slotCount, ref spriteCount);
			_slotChildren = ((slotCount > 0) ? new AdobeAnimateSlot[slotCount] : System.Array.Empty<AdobeAnimateSlot>());
			_spriteChildren = ((spriteCount > 0) ? new AdobeAnimateSprite[spriteCount] : System.Array.Empty<AdobeAnimateSprite>());
			_spriteChildOwnerSlots = ((spriteCount > 0) ? new AdobeAnimateSlot[spriteCount] : System.Array.Empty<AdobeAnimateSlot>());
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < bindingCount; i++)
			{
				ref OwnedChildBinding reference = ref bindings[i];
				if (reference.Slot != null)
				{
					_slotChildren[num++] = reference.Slot;
					continue;
				}
				_spriteChildren[num2] = reference.Sprite;
				_spriteChildOwnerSlots[num2++] = reference.OwnerSlot;
			}
		}
		finally
		{
			ArrayPool<OwnedChildBinding>.Shared.Return(bindings, clearArray: true);
		}
		_hasChildren = _slotChildren.Length != 0 || _spriteChildren.Length != 0;
		AutoInsertChildSprites();
	}

	private void CollectOwnedChildBindings(Node parent, bool collectSpriteChildren, AdobeAnimateSlot ownerSlot, bool refreshSlotRequirements, ref OwnedChildBinding[] bindings, ref int bindingCount, ref int slotCount, ref int spriteCount)
	{
		int childCount = parent.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			Node child = parent.GetChild(i);
			if (child is AdobeAnimateSlot adobeAnimateSlot)
			{
				adobeAnimateSlot.sprite = this;
				if (refreshSlotRequirements)
				{
					adobeAnimateSlot.RefreshRuntimeUpdateRequirement();
				}
				AddOwnedChildBinding(ref bindings, ref bindingCount, new OwnedChildBinding
				{
					Slot = adobeAnimateSlot
				});
				slotCount++;
				CollectOwnedChildBindings(adobeAnimateSlot, collectSpriteChildren: true, adobeAnimateSlot, refreshSlotRequirements, ref bindings, ref bindingCount, ref slotCount, ref spriteCount);
			}
			else if (child is AdobeAnimateSprite sprite)
			{
				if (collectSpriteChildren)
				{
					AddOwnedChildBinding(ref bindings, ref bindingCount, new OwnedChildBinding
					{
						Sprite = sprite,
						OwnerSlot = ownerSlot
					});
					spriteCount++;
				}
			}
			else if (child.GetChildCount() > 0)
			{
				CollectOwnedChildBindings(child, ownerSlot != null, ownerSlot, refreshSlotRequirements, ref bindings, ref bindingCount, ref slotCount, ref spriteCount);
			}
		}
	}

	private static void AddOwnedChildBinding(ref OwnedChildBinding[] bindings, ref int bindingCount, OwnedChildBinding binding)
	{
		if (bindingCount == bindings.Length)
		{
			OwnedChildBinding[] array = ArrayPool<OwnedChildBinding>.Shared.Rent(bindings.Length * 2);
			System.Array.Copy(bindings, array, bindingCount);
			ArrayPool<OwnedChildBinding>.Shared.Return(bindings, clearArray: true);
			bindings = array;
		}
		bindings[bindingCount++] = binding;
	}

	private void CollectOwnedChildren(Node parent, List<AdobeAnimateSlot> slots, List<AdobeAnimateSprite> sprites, List<AdobeAnimateSlot> spriteOwnerSlots, bool collectSpriteChildren, AdobeAnimateSlot ownerSlot)
	{
		int childCount = parent.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			Node child = parent.GetChild(i);
			if (child is AdobeAnimateSlot adobeAnimateSlot)
			{
				adobeAnimateSlot.sprite = this;
				adobeAnimateSlot.RefreshRuntimeUpdateRequirement();
				slots.Add(adobeAnimateSlot);
				CollectOwnedChildren(adobeAnimateSlot, slots, sprites, spriteOwnerSlots, collectSpriteChildren: true, adobeAnimateSlot);
			}
			else if (child is AdobeAnimateSprite item)
			{
				if (collectSpriteChildren)
				{
					sprites.Add(item);
					spriteOwnerSlots.Add(ownerSlot);
				}
			}
			else if (child.GetChildCount() > 0)
			{
				CollectOwnedChildren(child, slots, sprites, spriteOwnerSlots, ownerSlot != null, ownerSlot);
			}
		}
	}

	internal void MarkSlotRuntimeUpdateCacheDirty()
	{
		_runtimeSlotChildrenDirty = true;
		_runtimeChildUpdatesVisualOnlyDirty = true;
		RefreshRuntimeChildUpdatePresence();
		InvalidateRuntimeDisplayVisualRequirementCache();
	}

	protected void SyncRuntimeChildState(AdobeAnimateSprite child)
	{
		if (child != null && child.IsRuntimeInsideTree)
		{
			if (CachedEditorHint)
			{
				child.pause = pause;
				child.LightMask = LightMask;
			}
			else
			{
				child.ApplyRuntimeParentState(this);
			}
		}
	}

	private void ApplyRuntimeParentState(AdobeAnimateSprite parent)
	{
		if (_runtimeParentStateSource != parent)
		{
			_runtimeParentStateSource = parent;
			_runtimeParentStateValidationCountdown = 0;
		}
		SetParentPlaybackStopped(parent.IsPlaybackStopped);
		if (_pause != parent._pause)
		{
			pause = parent._pause;
		}
		if (_runtimeParentStateValidationCountdown > 0)
		{
			_runtimeParentStateValidationCountdown--;
			return;
		}
		int lightMask = parent.LightMask;
		if (LightMask != lightMask)
		{
			LightMask = lightMask;
		}
		_runtimeParentStateValidationCountdown = 30;
	}

	private void ApplyRuntimeFollowVisibility(bool visible)
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

	private AdobeAnimateSlot[] GetRuntimeSlotChildren()
	{
		if (!_runtimeSlotChildrenDirty)
		{
			return _runtimeSlotChildren;
		}
		int num = 0;
		for (int i = 0; i < _slotChildren.Length; i++)
		{
			AdobeAnimateSlot adobeAnimateSlot = _slotChildren[i];
			if (adobeAnimateSlot != null && adobeAnimateSlot.RequiresRuntimeUpdate)
			{
				num++;
			}
		}
		if (num == _slotChildren.Length)
		{
			_runtimeSlotChildren = _slotChildren;
			_runtimeSlotChildrenDirty = false;
			return _runtimeSlotChildren;
		}
		AdobeAnimateSlot[] array = new AdobeAnimateSlot[num];
		int num2 = 0;
		for (int j = 0; j < _slotChildren.Length; j++)
		{
			AdobeAnimateSlot adobeAnimateSlot2 = _slotChildren[j];
			if (adobeAnimateSlot2 != null && adobeAnimateSlot2.RequiresRuntimeUpdate)
			{
				array[num2++] = adobeAnimateSlot2;
			}
		}
		_runtimeSlotChildren = array;
		_runtimeSlotChildrenDirty = false;
		return _runtimeSlotChildren;
	}

	private void AutoInsertChildSprites()
	{
		if (_autoInserting || _layerVisible.Count == 0 || _spriteChildren.Length == 0)
		{
			return;
		}
		List<(AdobeAnimateSprite, int)> list = new List<(AdobeAnimateSprite, int)>();
		AdobeAnimateSprite[] spriteChildren = _spriteChildren;
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildren)
		{
			if (adobeAnimateSprite == null || !GodotObject.IsInstanceValid(adobeAnimateSprite) || adobeAnimateSprite._parentSprite != this || adobeAnimateSprite.GetParent() != this)
			{
				continue;
			}
			bool flag = false;
			foreach (InsertedSprite insertedSprite in _insertedSprites)
			{
				if (insertedSprite.sprite == adobeAnimateSprite)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				int num = adobeAnimateSprite.insertLayerId;
				if (num >= 0 && num < _layerVisible.Count)
				{
					list.Add((adobeAnimateSprite, num));
				}
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		_autoInserting = true;
		try
		{
			foreach (var (childSprite, layerId) in list)
			{
				InsertSpriteAtLayer(childSprite, layerId);
			}
		}
		finally
		{
			_autoInserting = false;
		}
	}

	public void SetMultimeshModulate(Node parent)
	{
		ApplyComposedModulate();
		int childCount = parent.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			Node child = parent.GetChild(i);
			if (child is AdobeAnimateSprite adobeAnimateSprite)
			{
				adobeAnimateSprite.meshColor = _meshColor;
			}
			if (child.GetChildCount() > 0)
			{
				SetMultimeshModulate(child);
			}
		}
	}

	private bool TryGetRuntimeClipRange(string clipName, out Vector2I range)
	{
		range = default;
		if (CachedEditorHint || !GodotObject.IsInstanceValid(_flashAnimeData))
		{
			return false;
		}
		return GetRuntimeDefinition()?.TryGetClipRange(clipName, out range) ?? false;
	}

	public bool HasClip(string clipName)
	{
		if (!GodotObject.IsInstanceValid(_flashAnimeData))
		{
			return false;
		}
		if (!CachedEditorHint)
		{
			AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
			if (runtimeDefinition != null && runtimeDefinition.HasClip(clipName))
			{
				return true;
			}
		}
		return _flashAnimeData.HasClip(clipName);
	}

	private void UpdateChildPoseImmediate()
	{
		if (_hasChildren)
		{
			UpdateChild();
			_lastUpdateFrame = frameIndex;
		}
	}

	public virtual void SetClip(string clipName)
	{
		_playbackRevision++;
		_managedPoseRevision++;
		MarkEffectOnceBatchEligibilityChanged();
		InvalidateRuntimeDisplayVisualRequirementCache();
		_needsRenderSubmission = true;
		string text = _clip;
		Vector2I vector2I = clipRange;
		bool flag = _renderStaticStateCached && _cachedConfiguredLayersAllVisible && _cachedLayerStateVersion == _layerStateVersion && _cachedMediaReplaceStateVersion == _mediaReplaceStateVersion;
		float renderFrameFloat = GetRenderFrameFloat();
		_clip = clipName;
		if (GodotObject.IsInstanceValid(_flashAnimeData))
		{
			if (CachedEditorHint)
			{
				blendTime = blendTimeInit;
			}
			Vector2I vector2I2 = (TryGetRuntimeClipRange(_clip, out var range) ? range : _flashAnimeData.GetClip(_clip));
			vector2I2.Y = Mathf.Clamp(vector2I2.Y, -1, _flashAnimeData.frameMax);
			if (vector2I2 != Vector2I.One * -1)
			{
				int num = Mathf.Clamp(frameIndex, clipRange.X, clipRange.Y);
				if (initClip && (!skipLastFrame || (num != clipRange.X && num != clipRange.Y)))
				{
					blendTimer = 0.0;
					if (blendTime > 0.0 && HasClip(text))
					{
						_blendFromFrameFloat = renderFrameFloat;
						blend = true;
					}
					else
					{
						blend = false;
						blendTime = 0.0;
						blendTimer = 0.0;
					}
				}
				clipRange = vector2I2;
				frameIndex = clipRange.X;
				elapsedTimer = 0.0;
				_lastUpdateFrame = -1;
				if (_applyingImmediateAnimation)
				{
					clipOver = false;
				}
				else
				{
					SetDeferred("clipOver", false);
				}
				if (!_processEnabled)
				{
					SetProcessEnabled(enabled: true);
				}
			}
		}
		initClip = true;
		UpdateChildPoseImmediate();
		if (text != _clip || vector2I != clipRange)
		{
			if (flag)
			{
				RefreshLayerActiveInCurrentClipCache(_cachedLayerVisibleCount, GetRuntimeDefinition());
			}
			else
			{
				_renderStaticStateCached = false;
			}
		}
		InvalidateRenderSnapshotCache();
		InvalidateRuntimeCrowdCullingForRefresh();
		RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, !flag);
	}

	public virtual void ResetAnimation()
	{
		_playbackRevision++;
		_managedPoseRevision++;
		MarkEffectOnceBatchEligibilityChanged();
		elapsedTimer = 0.0;
		frameIndex = clipRange.X;
		_lastUpdateFrame = -1;
		UpdateChildPoseImmediate();
		InvalidateRenderSnapshotCache();
		RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, staticStateDirty: false);
		AdobeAnimateSprite[] spriteChildren = _spriteChildren;
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildren)
		{
			if (adobeAnimateSprite != null && GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.ResetAnimation();
			}
		}
		if (!_processEnabled)
		{
			SetProcessEnabled(enabled: true);
		}
	}

	public void SetAnimation(string clipName, bool loop = true, double blendTime = 0.0)
	{
		MarkEffectOnceBatchEligibilityChanged();
		track.Clear();
		_nextAnimDelayTimer = -1.0;
		_pendingTrackConfig = null;
		if (!string.IsNullOrEmpty(clipName) && clipName.IndexOf('&') < 0)
		{
			clipOver = false;
			if (!_processEnabled)
			{
				SetProcessEnabled(enabled: true);
			}
			ApplyImmediateAnimation(clipName, loop, blendTime);
			return;
		}
		AddAnimation(clipName, 0.0, loop, blendTime);
		clipOver = false;
		if (!_processEnabled)
		{
			SetProcessEnabled(enabled: true);
		}
		NextAnimation();
	}

	public void AddAnimation(string clipName, double delay, bool loop = true, double blendTime = 0.0)
	{
		string[] array = clipName.Split("&", StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 0)
		{
			string text = array[GD.RandRange(0, array.Length - 1)];
			track.Insert(0, new AdobeAnimateTrack(text, delay, loop, blendTime));
			MarkEffectOnceBatchEligibilityChanged();
		}
	}

	public AdobeAnimateTrack NextAnimation()
	{
		if (track.Count <= 0)
		{
			if (!CachedEditorHint)
			{
				OnAnimeStoped?.Invoke(_clip);
			}
			return null;
		}
		AdobeAnimateTrack adobeAnimateTrack = track[track.Count - 1];
		track.RemoveAt(track.Count - 1);
		if (adobeAnimateTrack != null)
		{
			if (adobeAnimateTrack.delay != 0.0)
			{
				_nextAnimDelayTimer = adobeAnimateTrack.delay;
				_pendingTrackConfig = adobeAnimateTrack;
				if (!_processEnabled)
				{
					SetProcessEnabled(enabled: true);
				}
				return adobeAnimateTrack;
			}
			_nextAnimDelayTimer = -1.0;
			_pendingTrackConfig = null;
			ApplyNextAnimation(adobeAnimateTrack);
		}
		else if (!CachedEditorHint)
		{
			OnAnimeStoped?.Invoke(_clip);
		}
		return adobeAnimateTrack;
	}

	private void ApplyNextAnimation(AdobeAnimateTrack trackConfig)
	{
		if (GodotObject.IsInstanceValid(this) && trackConfig != null)
		{
			ApplyImmediateAnimation(trackConfig.clip, trackConfig.loop, trackConfig.blendTime);
		}
	}

	private void ApplyImmediateAnimation(string clipName, bool nextLoop, double nextBlendTime)
	{
		blendTime = nextBlendTime;
		if (CachedEditorHint)
		{
			blendTime = blendTimeInit;
		}
		loop = nextLoop;
		bool applyingImmediateAnimation = _applyingImmediateAnimation;
		_applyingImmediateAnimation = true;
		try
		{
			SetClip(clipName);
		}
		finally
		{
			_applyingImmediateAnimation = applyingImmediateAnimation;
		}
		if (!CachedEditorHint)
		{
			OnAnimeStarted?.Invoke(_clip);
			clipOver = false;
			if (!_processEnabled)
			{
				SetProcessEnabled(enabled: true);
			}
		}
	}

	public double GetProgress()
	{
		double num = (double)(clipRange.Y - clipRange.X) + 1.0;
		if (num <= 0.0)
		{
			return 0.0;
		}
		return (double)(frameIndex - clipRange.X) / num;
	}

	public void SetFliter(StringName layerName, bool open)
	{
		if (_flashAnimeData != null && _flashAnimeData.layerDictionary != null && _flashAnimeData.layerDictionary.ContainsKey(layerName))
		{
			int num = (int)_flashAnimeData.layerDictionary[layerName];
			if (num >= 0 && num < _layerVisible.Count)
			{
				_layerVisible[num] = open;
				MarkLayerStateChanged();
			}
		}
	}

	public bool GetFliter(StringName layerName)
	{
		if (_flashAnimeData == null || _flashAnimeData.layerDictionary == null)
		{
			return false;
		}
		if (_flashAnimeData.layerDictionary.ContainsKey(layerName))
		{
			int num = (int)_flashAnimeData.layerDictionary[layerName];
			if (num >= 0 && num < _layerVisible.Count)
			{
				return _layerVisible[num];
			}
		}
		return false;
	}

	public void SetFliters(Godot.Collections.Array layerNameList, bool open)
	{
		foreach (Variant layerName in layerNameList)
		{
			SetFliter(layerName.AsStringName(), open);
		}
	}

	public void SetFliterRecursive(StringName layerName, bool open)
	{
		SetFliter(layerName, open);
		string recursiveFilterSourceKey = GetRecursiveFilterSourceKey(_flashAnimeData);
		if (!string.IsNullOrEmpty(recursiveFilterSourceKey))
		{
			ApplyFliterToChildSprites(this, recursiveFilterSourceKey, layerName, open);
		}
	}

	public void SetFlitersRecursive(Godot.Collections.Array layerNameList, bool open)
	{
		SetFliters(layerNameList, open);
		string recursiveFilterSourceKey = GetRecursiveFilterSourceKey(_flashAnimeData);
		if (!string.IsNullOrEmpty(recursiveFilterSourceKey))
		{
			ApplyFlitersToChildSprites(this, recursiveFilterSourceKey, layerNameList, open);
		}
	}

	private static void ApplyFliterToChildSprites(Node node, string sourceKey, StringName layerName, bool open)
	{
		int childCount = node.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			Node child = node.GetChild(i);
			if (child is AdobeAnimateSprite adobeAnimateSprite && IsMatchingRecursiveFilterSprite(adobeAnimateSprite, sourceKey))
			{
				adobeAnimateSprite.SetFliter(layerName, open);
			}
			ApplyFliterToChildSprites(child, sourceKey, layerName, open);
		}
	}

	private static void ApplyFlitersToChildSprites(Node node, string sourceKey, Godot.Collections.Array layerNameList, bool open)
	{
		int childCount = node.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			Node child = node.GetChild(i);
			if (child is AdobeAnimateSprite adobeAnimateSprite && IsMatchingRecursiveFilterSprite(adobeAnimateSprite, sourceKey))
			{
				adobeAnimateSprite.SetFliters(layerNameList, open);
			}
			ApplyFlitersToChildSprites(child, sourceKey, layerNameList, open);
		}
	}

	private static bool IsMatchingRecursiveFilterSprite(AdobeAnimateSprite sprite, string sourceKey)
	{
		if (!string.IsNullOrEmpty(sourceKey))
		{
			return string.Equals(GetRecursiveFilterSourceKey(sprite?._flashAnimeData), sourceKey, StringComparison.Ordinal);
		}
		return false;
	}

	private static string GetRecursiveFilterSourceKey(AdobeAnimateData data)
	{
		return data?.GetAtlasSourceKey() ?? "";
	}

	public void SetReplace(StringName mediaName, PackedScene scene)
	{
		SetReplace(mediaName, (Resource)scene);
	}

	public void SetReplace(StringName mediaName, Resource resource)
	{
		SetReplace(mediaName, resource as Texture2D);
	}

	public void SetReplace(StringName mediaName, Texture2D texture)
	{
		if (_flashAnimeData == null || _flashAnimeData.mediaDictionary == null || !_flashAnimeData.mediaDictionary.ContainsKey(mediaName))
		{
			return;
		}
		int num = (int)_flashAnimeData.mediaDictionary[mediaName];
		if (num >= 0 && num < _mediaReplace.Count)
		{
			_mediaReplace[num] = texture;
			if (num < _mediaReplaceAtlasPaths.Count)
			{
				_mediaReplaceAtlasPaths[num] = string.Empty;
			}
			_mediaReplaceUse[num] = GodotObject.IsInstanceValid(texture);
			CaptureStaticAtlasPathLayoutConfigurationSnapshot();
			InvalidateStaticAtlasPathLayoutForNodeMutation();
			QueueUpdateMediaReplace();
		}
	}

	public bool SetAtlasReplace(StringName mediaName, string textureReference, bool queueUpdate = true)
	{
		if (_flashAnimeData == null || _flashAnimeData.mediaDictionary == null || !_flashAnimeData.mediaDictionary.ContainsKey(mediaName))
		{
			return false;
		}
		int num = (int)_flashAnimeData.mediaDictionary[mediaName];
		if (num < 0 || num >= _mediaReplace.Count || num >= _mediaReplaceUse.Count || num >= _mediaReplaceAtlasPaths.Count)
		{
			return false;
		}
		string value = textureReference ?? string.Empty;
		_mediaReplace[num] = null;
		_mediaReplaceAtlasPaths[num] = value;
		_mediaReplaceUse[num] = !string.IsNullOrWhiteSpace(value);
		CaptureStaticAtlasPathLayoutConfigurationSnapshot();
		InvalidateStaticAtlasPathLayoutForNodeMutation();
		if (queueUpdate)
		{
			QueueUpdateMediaReplace();
		}
		return true;
	}

	public string GetAtlasReplacePath(StringName mediaName)
	{
		if (_flashAnimeData == null || _flashAnimeData.mediaDictionary == null || !_flashAnimeData.mediaDictionary.ContainsKey(mediaName))
		{
			return string.Empty;
		}
		int num = (int)_flashAnimeData.mediaDictionary[mediaName];
		if (num < 0 || num >= _mediaReplaceAtlasPaths.Count)
		{
			return string.Empty;
		}
		return _mediaReplaceAtlasPaths[num];
	}

	public Texture2D GetReplace(StringName mediaName)
	{
		if (_flashAnimeData == null || _flashAnimeData.mediaDictionary == null)
		{
			return null;
		}
		if (_flashAnimeData.mediaDictionary.ContainsKey(mediaName))
		{
			int num = (int)_flashAnimeData.mediaDictionary[mediaName];
			if (num >= 0 && num < _mediaReplace.Count)
			{
				return _mediaReplace[num];
			}
		}
		return null;
	}

	public void SetVerticalClip(bool enabled, float upY, float downY)
	{
		if (_verticalClip.Enabled != enabled || !Mathf.IsEqualApprox(_verticalClip.UpY, upY) || !Mathf.IsEqualApprox(_verticalClip.DownY, downY))
		{
			_verticalClip.Enabled = enabled;
			_verticalClip.UpY = upY;
			_verticalClip.DownY = downY;
			InvalidateRenderSnapshotCache();
			RequestNodeRedraw();
		}
	}

	public void SetVerticalClip(float upY, float downY)
	{
		SetVerticalClip(enabled: true, upY, downY);
	}

	public VerticalClipState GetVerticalClipState()
	{
		return _verticalClip;
	}

	public void SetDiscardUpPos(float value)
	{
		if (!_verticalClip.Enabled || !Mathf.IsEqualApprox(_verticalClip.UpY, value))
		{
			_verticalClip.Enabled = true;
			_verticalClip.UpY = value;
			InvalidateRenderSnapshotCache();
			RequestNodeRedraw();
		}
	}

	public void SetDiscardDownPos(float value)
	{
		bool flag = !Mathf.IsEqualApprox(_verticalClip.UpY, -10000f) || !Mathf.IsEqualApprox(value, 10000f);
		if (_verticalClip.Enabled != flag || !Mathf.IsEqualApprox(_verticalClip.DownY, value))
		{
			_verticalClip.Enabled = flag;
			_verticalClip.DownY = value;
			InvalidateRenderSnapshotCache();
			RequestNodeRedraw();
		}
	}

	public void SetSpriteGroupShaderParameter(string property, Variant value)
	{
		if (!(property == "discardUpPos"))
		{
			if (property == "discardDownPos")
			{
				SetDiscardDownPos((float)value.AsDouble());
			}
		}
		else
		{
			SetDiscardUpPos((float)value.AsDouble());
		}
	}

	public void InsertSpriteAtLayer(AdobeAnimateSprite childSprite, int layerId)
	{
		if (childSprite != null && childSprite != this && layerId >= 0 && layerId < _layerVisible.Count)
		{
			childSprite.insertLayerId = layerId;
			TrackInsertedSpriteAtLayer(childSprite, layerId);
		}
	}

	private void RefreshChildInsertLayerForEditor(AdobeAnimateSprite child)
	{
		if (child != null && GodotObject.IsInstanceValid(child) && child != this)
		{
			int num = ResolveSpriteChildInsertLayer(child);
			if (num >= 0)
			{
				TrackInsertedSpriteAtLayer(child, num);
			}
			else if (!UntrackInsertedSpriteForEditor(child))
			{
				ClearInsertedChildRenderSort(child);
				child.InvalidateRenderSnapshotCache();
				child.RequestNodeRedraw();
				InvalidateRenderSnapshotCache();
				RequestNodeRedraw();
			}
		}
	}

	private void TrackInsertedSpriteAtLayer(AdobeAnimateSprite childSprite, int layerId)
	{
		if (childSprite == null || childSprite == this || layerId < 0 || layerId > _layerVisible.Count)
		{
			return;
		}
		if (childSprite.GetParent() != this)
		{
			AddChild(childSprite, forceReadableName: false, InternalMode.Disabled);
		}
		for (int i = 0; i < _insertedSprites.Count; i++)
		{
			if (_insertedSprites[i].sprite == childSprite)
			{
				_insertedSprites[i] = new InsertedSprite
				{
					sprite = childSprite,
					layerId = layerId
				};
				ApplyChildRenderSort(childSprite, layerId);
				CacheChildren();
				InvalidateRenderSnapshotCache();
				RequestNodeRedraw();
				return;
			}
		}
		_insertedSprites.Add(new InsertedSprite
		{
			sprite = childSprite,
			layerId = layerId
		});
		ApplyChildRenderSort(childSprite, layerId);
		CacheChildren();
		InvalidateRenderSnapshotCache();
		RequestNodeRedraw();
	}

	private bool UntrackInsertedSpriteForEditor(AdobeAnimateSprite childSprite)
	{
		for (int i = 0; i < _insertedSprites.Count; i++)
		{
			if (_insertedSprites[i].sprite == childSprite)
			{
				_insertedSprites.RemoveAt(i);
				ClearInsertedChildRenderSort(childSprite);
				CacheChildren();
				childSprite.InvalidateRenderSnapshotCache();
				childSprite.RequestNodeRedraw();
				InvalidateRenderSnapshotCache();
				RequestNodeRedraw();
				return true;
			}
		}
		return false;
	}

	private void OnSpriteChildParentChanged(AdobeAnimateSprite childSprite)
	{
		bool flag = false;
		for (int num = _insertedSprites.Count - 1; num >= 0; num--)
		{
			if (_insertedSprites[num].sprite == childSprite)
			{
				_insertedSprites.RemoveAt(num);
				flag = true;
			}
		}
		if (flag && GodotObject.IsInstanceValid(childSprite))
		{
			ClearInsertedChildRenderSort(childSprite);
		}
		MarkManagedSlotSpriteCacheDirty();
	}

	public void RemoveInsertedSprite(AdobeAnimateSprite childSprite)
	{
		int num = -1;
		for (int i = 0; i < _insertedSprites.Count; i++)
		{
			if (_insertedSprites[i].sprite == childSprite)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			InsertedSprite insertedSprite = _insertedSprites[num];
			if (GodotObject.IsInstanceValid(insertedSprite.sprite))
			{
				ClearInsertedChildRenderSort(insertedSprite.sprite);
				RemoveChild(insertedSprite.sprite);
			}
			_insertedSprites.RemoveAt(num);
			CacheChildren();
			InvalidateRenderSnapshotCache();
			RequestNodeRedraw();
		}
	}

	public void RemoveAllInsertedSprites()
	{
		foreach (InsertedSprite insertedSprite in _insertedSprites)
		{
			if (GodotObject.IsInstanceValid(insertedSprite.sprite))
			{
				ClearInsertedChildRenderSort(insertedSprite.sprite);
				RemoveChild(insertedSprite.sprite);
			}
		}
		_insertedSprites.Clear();
		CacheChildren();
		InvalidateRenderSnapshotCache();
		RequestNodeRedraw();
	}

	public Dictionary ExportSpriteSave()
	{
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (AdobeAnimateTrack item in track)
		{
			array.Add(new Dictionary
			{
				{ "clip", item.clip },
				{ "delay", item.delay },
				{ "loop", item.loop },
				{ "blendTime", item.blendTime }
			});
		}
		return new Dictionary
		{
			{ "clip", _clip },
			{ "clipRangeX", clipRange.X },
			{ "clipRangeY", clipRange.Y },
			{ "frameIndex", frameIndex },
			{ "elapsedTimer", elapsedTimer },
			{ "loop", loop },
			{ "clipOver", clipOver },
			{ "timeScale", timeScale },
			{ "pause", _pause },
			{ "playBack", playBack },
			{ "blend", blend },
			{ "blendTime", blendTime },
			{ "blendTimer", blendTimer },
			{ "blendFromFrameFloat", _blendFromFrameFloat },
			{ "track", array },
			{
				"layerVisible",
				_layerVisible.Duplicate(deep: true)
			},
			{ "initClip", initClip }
		};
	}

	public void ImportSpriteSave(Dictionary data)
	{
		if (data.Count == 0)
		{
			return;
		}
		string text = (data.ContainsKey("clip") ? data["clip"].AsString() : _clip);
		int x = (data.ContainsKey("clipRangeX") ? data["clipRangeX"].AsInt32() : clipRange.X);
		int y = (data.ContainsKey("clipRangeY") ? data["clipRangeY"].AsInt32() : clipRange.Y);
		int num = (data.ContainsKey("frameIndex") ? data["frameIndex"].AsInt32() : frameIndex);
		double num2 = (data.ContainsKey("elapsedTimer") ? data["elapsedTimer"].AsDouble() : elapsedTimer);
		bool flag = (data.ContainsKey("loop") ? data["loop"].AsBool() : loop);
		bool flag2 = (data.ContainsKey("clipOver") ? data["clipOver"].AsBool() : clipOver);
		double num3 = (data.ContainsKey("timeScale") ? data["timeScale"].AsDouble() : timeScale);
		bool flag3 = (data.ContainsKey("pause") ? data["pause"].AsBool() : _pause);
		bool flag4 = (data.ContainsKey("playBack") ? data["playBack"].AsBool() : playBack);
		bool flag5 = (data.ContainsKey("blend") ? data["blend"].AsBool() : blend);
		double num4 = (data.ContainsKey("blendTime") ? data["blendTime"].AsDouble() : blendTime);
		double num5 = (data.ContainsKey("blendTimer") ? data["blendTimer"].AsDouble() : blendTimer);
		float blendFromFrameFloat = (data.ContainsKey("blendFromFrameFloat") ? data["blendFromFrameFloat"].AsSingle() : ((float)num + (float)num2));
		bool flag6 = (data.ContainsKey("initClip") ? data["initClip"].AsBool() : initClip);
		Array<bool> array = (data.ContainsKey("layerVisible") ? data["layerVisible"].AsGodotArray<bool>() : new Array<bool>());
		Godot.Collections.Array obj = (data.ContainsKey("track") ? data["track"].AsGodotArray() : new Godot.Collections.Array());
		_clip = text;
		clipRange = new Vector2I(x, y);
		frameIndex = num;
		elapsedTimer = num2;
		loop = flag;
		clipOver = flag2;
		timeScale = num3;
		pause = flag3;
		playBack = flag4;
		blend = flag5;
		blendTime = num4;
		blendTimer = num5;
		_blendFromFrameFloat = blendFromFrameFloat;
		initClip = flag6;
		if (array.Count == _layerVisible.Count)
		{
			layerVisible = array;
			MarkLayerStateChanged();
		}
		track.Clear();
		foreach (Variant item2 in obj)
		{
			Dictionary dictionary = item2.AsGodotDictionary();
			AdobeAnimateTrack item = new AdobeAnimateTrack(dictionary.ContainsKey("clip") ? dictionary["clip"].AsString() : "", dictionary.ContainsKey("delay") ? dictionary["delay"].AsDouble() : 0.0, !dictionary.ContainsKey("loop") || dictionary["loop"].AsBool(), dictionary.ContainsKey("blendTime") ? dictionary["blendTime"].AsDouble() : 0.0);
			track.Add(item);
		}
		if (!clipOver || track.Count > 0)
		{
			SetProcessEnabled(enabled: true);
		}
		InvalidateRenderSnapshotCache(force: true);
		RequestNodeRedraw();
	}

	private static void ResetCachedRenderFrameEffectiveAncestorModulates()
	{
		CachedRenderFrameEffectiveAncestorModulates.Clear();
	}

	private Color GetCachedEffectiveRenderAncestorModulate(Node renderMountParent)
	{
		int count = _renderModulateAncestors.Count;
		if (count == 0)
		{
			return Colors.White;
		}
		if (CachedViewportWorldRectRenderScopeDepth <= 0)
		{
			Color left = Colors.White;
			for (int i = 0; i < count; i++)
			{
				left = MultiplyRenderColor(in left, GetCachedRenderFrameModulate(_renderModulateAncestors[i]));
			}
			return left;
		}
		Color right = Colors.White;
		int num = count;
		for (int j = 0; j < count; j++)
		{
			CanvasItem key = _renderModulateAncestors[j];
			if (CachedRenderFrameEffectiveAncestorModulates.TryGetValue(key, out var value) && value.RenderMountParent == renderMountParent)
			{
				right = value.Value;
				num = j;
				break;
			}
		}
		for (int num2 = num - 1; num2 >= 0; num2--)
		{
			CanvasItem key2 = _renderModulateAncestors[num2];
			right = MultiplyRenderColor(GetCachedRenderFrameModulate(key2), in right);
			CachedRenderFrameEffectiveAncestorModulates[key2] = new EffectiveAncestorModulateCacheEntry(renderMountParent, right);
		}
		return right;
	}

	internal void MarkRenderOrderDebugForRender(string reason)
	{
		if (DebugRenderOrderTrace)
		{
			_renderOrderDebugPending = true;
			_renderOrderDebugReason = reason ?? "";
		}
	}

	internal bool HasRenderOrderDebugPendingForRender()
	{
		if (DebugRenderOrderTrace)
		{
			return _renderOrderDebugPending;
		}
		return false;
	}

	internal void InvalidateInheritedRenderOrderForRender(string reason = "")
	{
		_gpuGraphCrowdPresentationStateDirty = true;
		MarkEffectOnceBatchRenderStateDirty(invalidateZ: true);
		_effectiveRenderSortBandCached = false;
		_effectiveRenderSortBandRescanCountdown = 0;
		_treeOrderPathRescanCountdown = 0;
		MarkRenderOrderDebugForRender(reason);
		InvalidateRenderSnapshotCache();
	}

	internal void PrintRenderOrderDebugForRender(string phase, int effectiveZIndex, long sortBand, Node renderMountParent, AdobeAnimateRuntimeDefinition definition, string extra = "", bool consume = true)
	{
		if (!DebugRenderOrderTrace || !_renderOrderDebugPending)
		{
			return;
		}
		if (_debugRenderOrderTraceCount >= 800)
		{
			if (consume)
			{
				_renderOrderDebugPending = false;
			}
			return;
		}
		_debugRenderOrderTraceCount++;
		string text = (IsInsideTree() ? GetPath().ToString() : Name.ToString());
		string value = ((GodotObject.IsInstanceValid(renderMountParent) && renderMountParent.IsInsideTree()) ? renderMountParent.GetPath().ToString() : (renderMountParent?.Name.ToString() ?? "<null>"));
		string value2 = definition?.Source?.ResourcePath ?? _flashAnimeData?.ResourcePath ?? "<null>";
		Transform2D cachedGlobalTransformForRender = GetCachedGlobalTransformForRender();
		GD.Print($"[AdobeAnimateRenderOrderDebug] #{_debugRenderOrderTraceCount} phase={phase} path={text} reason=\"{_renderOrderDebugReason}\" localZ={ZIndex} zRel={ZAsRelative} behind={ShowBehindParent} effectiveZ={effectiveZIndex} cachedEffectiveZ={_cachedEffectiveZIndex} zCache={_effectiveZIndexCached} sortBand={sortBand} localSortBand={_renderSortBand} sortCache={_effectiveRenderSortBandCached} runtimeManager={_usingRuntimeManager} snapshotCache={_hasCachedRenderSnapshot} needsSubmit={_needsRenderSubmission} drawOrderSort={GetNeedsDrawOrderSortBandsForRender()} managedSlots={HasManagedSlotSpritesForRender()} childSprites={HasSpriteChildrenForRender()} clip={_clip} frame={GetRenderFrameFloat():F3} global=({cachedGlobalTransformForRender.Origin.X:F1},{cachedGlobalTransformForRender.Origin.Y:F1}) mount={value} canvas={_cachedCanvasLayer} data={value2} extra={extra}");
		GD.Print("[AdobeAnimateRenderOrderDebug.ParentChain] path=" + text + " chain=" + BuildRenderOrderParentChainDebug());
		if (consume)
		{
			_renderOrderDebugPending = false;
		}
	}

	private string BuildRenderOrderParentChainDebug()
	{
		StringBuilder stringBuilder = new StringBuilder(512);
		CanvasItem canvasItem = this;
		int num = 0;
		while (GodotObject.IsInstanceValid(canvasItem) && num++ < 16)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" <- ");
			}
			stringBuilder.Append(canvasItem.Name);
			stringBuilder.Append("(type=");
			stringBuilder.Append(canvasItem.GetType().Name);
			stringBuilder.Append(",z=");
			stringBuilder.Append(canvasItem.ZIndex);
			stringBuilder.Append(",rel=");
			stringBuilder.Append(canvasItem.ZAsRelative);
			stringBuilder.Append(",behind=");
			stringBuilder.Append(canvasItem.ShowBehindParent);
			stringBuilder.Append(')');
			if (!canvasItem.ZAsRelative)
			{
				break;
			}
			canvasItem = canvasItem.GetParent() as CanvasItem;
		}
		if (num >= 16 && GodotObject.IsInstanceValid(canvasItem))
		{
			stringBuilder.Append(" <- ...");
		}
		return stringBuilder.ToString();
	}

	private int GetEffectiveZIndexForRender()
	{
		if (!_usingRuntimeManager)
		{
			return GetEffectiveZIndex();
		}
		return GetCachedEffectiveZIndexForRender();
	}

	private int GetCachedEffectiveZIndexForRender()
	{
		if (_effectiveZIndexCached && _effectiveZIndexRescanCountdown > 0)
		{
			_effectiveZIndexRescanCountdown--;
			return _cachedEffectiveZIndex;
		}
		int zIndex = ZIndex;
		bool zAsRelative = ZAsRelative;
		_cachedEffectiveZIndex = GetEffectiveZIndex(zIndex, zAsRelative);
		_effectiveZIndexCacheLocalZ = zIndex;
		_effectiveZIndexCacheZAsRelative = zAsRelative;
		_effectiveZIndexCached = true;
		_effectiveZIndexRescanCountdown = GetStaggeredRescanInterval(20);
		return _cachedEffectiveZIndex;
	}

	private int GetEffectiveZIndex(int localZ, bool localZAsRelative)
	{
		long num = localZ;
		if (!localZAsRelative)
		{
			return (int)Math.Clamp(num, -4096L, 4096L);
		}
		CanvasItem canvasItem = GetParent() as CanvasItem;
		while (GodotObject.IsInstanceValid(canvasItem))
		{
			num += canvasItem.ZIndex;
			if (!canvasItem.ZAsRelative)
			{
				break;
			}
			canvasItem = canvasItem.GetParent() as CanvasItem;
		}
		return (int)Math.Clamp(num, -4096L, 4096L);
	}

	private int GetEffectiveZIndex()
	{
		long num = 0L;
		CanvasItem canvasItem = this;
		while (GodotObject.IsInstanceValid(canvasItem))
		{
			num += canvasItem.ZIndex;
			if (!canvasItem.ZAsRelative)
			{
				break;
			}
			canvasItem = canvasItem.GetParent() as CanvasItem;
		}
		return (int)Math.Clamp(num, -4096L, 4096L);
	}

	internal int[] GetEffectiveTreeOrderPathForRender()
	{
		if (!_usingRuntimeManager && CachedEditorHint)
		{
			AdobeAnimateSprite renderSortRootForRender = GetRenderSortRootForRender();
			Node cachedRenderMountParent = GetCachedRenderMountParent();
			return BuildEffectiveTreeOrderPathForRender(renderSortRootForRender, cachedRenderMountParent);
		}
		if (_treeOrderPathRescanCountdown > 0 && _cachedEffectiveTreeOrderPath != null)
		{
			_treeOrderPathRescanCountdown--;
			return _cachedEffectiveTreeOrderPath;
		}
		AdobeAnimateSprite renderSortRootForRender2 = GetRenderSortRootForRender();
		Node cachedRenderMountParent2 = GetCachedRenderMountParent();
		RefreshTreeOrderPathWatcher(renderSortRootForRender2);
		int[] cachedEffectiveTreeOrderPath = _cachedEffectiveTreeOrderPath;
		int[] array = BuildEffectiveTreeOrderPathForRender(renderSortRootForRender2, cachedRenderMountParent2);
		bool flag = !AreTreeOrderPathsEqual(cachedEffectiveTreeOrderPath, array);
		_cachedEffectiveTreeOrderPath = (flag ? array : cachedEffectiveTreeOrderPath);
		_treeOrderPathRescanCountdown = GetStaggeredRescanInterval(60);
		if ((_usingRuntimeManager && cachedEffectiveTreeOrderPath != null) & flag)
		{
			AdobeAnimateRuntimeManager.InvalidateRenderRoots();
		}
		return _cachedEffectiveTreeOrderPath;
	}

	private void RefreshTreeOrderPathWatcher(Node root)
	{
		Node node = (GodotObject.IsInstanceValid(root) ? root.GetParent() : null);
		if (_treeOrderWatchedParent != node)
		{
			DisconnectTreeOrderPathWatchers();
			_treeOrderWatchedParent = node;
			if (GodotObject.IsInstanceValid(_treeOrderWatchedParent))
			{
				_treeOrderWatchedParent.ChildOrderChanged += OnTreeOrderHierarchyChanged;
			}
		}
	}

	private void OnTreeOrderHierarchyChanged()
	{
		InvalidateTreeOrderPathCache();
		if (_usingRuntimeManager)
		{
			AdobeAnimateRuntimeManager.InvalidateRenderRoots();
		}
	}

	private static bool AreTreeOrderPathsEqual(int[] left, int[] right)
	{
		if (left == right)
		{
			return true;
		}
		if (left == null || right == null || left.Length != right.Length)
		{
			return false;
		}
		for (int i = 0; i < left.Length; i++)
		{
			if (left[i] != right[i])
			{
				return false;
			}
		}
		return true;
	}

	private void InvalidateTreeOrderPathCache()
	{
		_treeOrderPathRescanCountdown = 0;
	}

	internal void InvalidateRuntimeTreeOrderForTopologyChange()
	{
		InvalidateTreeOrderPathCache();
	}

	private void DisconnectTreeOrderPathWatchers()
	{
		if (GodotObject.IsInstanceValid(_treeOrderWatchedParent))
		{
			_treeOrderWatchedParent.ChildOrderChanged -= OnTreeOrderHierarchyChanged;
		}
		_treeOrderWatchedParent = null;
	}

	private static int[] BuildEffectiveTreeOrderPathForRender(Node node, Node renderMountParent)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return System.Array.Empty<int>();
		}
		List<int> list = new List<int>();
		Node node2 = node;
		int num = 0;
		while (GodotObject.IsInstanceValid(node2) && node2 != renderMountParent && num++ < 128)
		{
			list.Add(Math.Max(0, node2.GetIndex()));
			node2 = node2.GetParent();
		}
		if (list.Count == 0)
		{
			list.Add(Math.Max(0, node.GetIndex()));
		}
		list.Reverse();
		return list.ToArray();
	}

	internal long GetEffectiveRenderSortBandForRender()
	{
		if (!_usingRuntimeManager)
		{
			return ComputeEffectiveRenderSortBand();
		}
		long renderSortBand = _renderSortBand;
		if (!_effectiveRenderSortBandCached || renderSortBand != _effectiveRenderSortBandCacheLocalSortBand || _effectiveRenderSortBandRescanCountdown <= 0)
		{
			_cachedEffectiveRenderSortBand = ComputeEffectiveRenderSortBand();
			_effectiveRenderSortBandCacheLocalSortBand = renderSortBand;
			_effectiveRenderSortBandCached = true;
			_effectiveRenderSortBandRescanCountdown = GetStaggeredRescanInterval(20);
			return _cachedEffectiveRenderSortBand;
		}
		_effectiveRenderSortBandRescanCountdown--;
		return _cachedEffectiveRenderSortBand;
	}

	private long ComputeEffectiveRenderSortBand()
	{
		return _renderSortBand + GetShowBehindParentSortBandBias();
	}

	private long GetShowBehindParentSortBandBias()
	{
		long num = 0L;
		CanvasItem canvasItem = this;
		int num2 = 0;
		while (GodotObject.IsInstanceValid(canvasItem) && num2++ < 64)
		{
			if (canvasItem.ShowBehindParent)
			{
				num += -1000000;
			}
			if (!canvasItem.ZAsRelative)
			{
				break;
			}
			canvasItem = canvasItem.GetParent() as CanvasItem;
		}
		return num;
	}

	private int GetStaggeredRescanInterval(int baseInterval)
	{
		int num = Math.Max(1, baseInterval);
		return num + (int)(GetCachedInstanceIdForRender() % (ulong)num);
	}

	private static ulong ComputeLayerVisibilitySignature(Array<bool> values, int valueCount)
	{
		if (values == null)
		{
			return 0uL;
		}
		ulong num = 1469598103934665603uL;
		for (int i = 0; i < valueCount; i++)
		{
			num ^= (ulong)(values[i] ? 1 : 0);
			num *= 1099511628211L;
		}
		return num;
	}

	private void DisableUnsharedMediaReplaceTextures(List<int> mediaIds)
	{
		if (mediaIds == null || mediaIds.Count == 0)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < mediaIds.Count; i++)
		{
			int num2 = mediaIds[i];
			if ((uint)num2 < (uint)(_mediaReplaceUse?.Count ?? 0) && _mediaReplaceUse[num2])
			{
				_mediaReplaceUse[num2] = false;
				if ((uint)num2 < (uint)(mediaReplaceRect?.Count ?? 0))
				{
					mediaReplaceRect[num2] = default;
				}
				if ((uint)num2 < (uint)(mediaReplaceAtlasPages?.Count ?? 0))
				{
					mediaReplaceAtlasPages[num2] = 0;
				}
				num++;
			}
		}
		if (num > 0)
		{
			TowerDefensePerfProfiler.Sample("adobeAnimate.mediaReplaceAtlas.missingSharedAtlas", num);
			if (!MissingMediaReplaceAtlasWarningPrinted)
			{
				MissingMediaReplaceAtlasWarningPrinted = true;
				GD.PushWarning("AdobeAnimate media replacement texture is missing from the shared Texture2DArray atlas; replacement drawing was disabled for that sprite.");
			}
		}
	}

	private bool TryUseSharedMediaReplaceAtlas(List<Texture2D> textures, List<string> atlasPaths, List<int> mediaIds)
	{
		if (textures == null || atlasPaths == null || mediaIds == null || textures.Count == 0 || textures.Count != atlasPaths.Count || textures.Count != mediaIds.Count)
		{
			return false;
		}
		Rect2[] array = new Rect2[textures.Count];
		int[] array2 = new int[textures.Count];
		TextureLayered textureLayered = null;
		Rid rid = default;
		Vector2 vector = default;
		for (int i = 0; i < textures.Count; i++)
		{
			Texture2D texture2D = textures[i];
			string text = atlasPaths[i];
			AdobeAnimateExternalTextureAtlasAllocation allocation;
			if (!string.IsNullOrEmpty(text))
			{
				if (!AdobeAnimateGlobalAtlasCache.TryGetReplaceTextureAllocation(text, out allocation))
				{
					return false;
				}
			}
			else
			{
				if (!GodotObject.IsInstanceValid(texture2D))
				{
					return false;
				}
				Vector2 size = texture2D.GetSize();
				if (size.X <= 0f || size.Y <= 0f)
				{
					return false;
				}
				if (!AdobeAnimateGlobalAtlasCache.TryGetReplaceTextureAllocation(texture2D, new Rect2(Vector2.Zero, size), out allocation))
				{
					return false;
				}
			}
			if (!allocation.UsesTextureArray || !GodotObject.IsInstanceValid(allocation.TextureArray) || !allocation.TextureArrayRid.IsValid)
			{
				return false;
			}
			if (!rid.IsValid)
			{
				rid = allocation.TextureArrayRid;
				textureLayered = allocation.TextureArray;
				vector = allocation.TextureArraySize;
			}
			else if (allocation.TextureArrayRid != rid)
			{
				return false;
			}
			array[i] = allocation.Rect;
			array2[i] = allocation.AtlasPage;
		}
		mediaReplaceAtlas = null;
		mediaReplaceAtlasArray = textureLayered;
		mediaReplaceAtlasArraySize = vector;
		mediaReplaceAtlasUsesTextureArray = true;
		mediaReplaceAtlasShared = true;
		for (int j = 0; j < mediaIds.Count; j++)
		{
			int num = mediaIds[j];
			if ((uint)num < (uint)mediaReplaceRect.Count)
			{
				mediaReplaceRect[num] = array[j];
				mediaReplaceAtlasPages[num] = array2[j];
			}
		}
		TowerDefensePerfProfiler.Sample("adobeAnimate.mediaReplaceAtlas.sharedAtlas", mediaIds.Count);
		return true;
	}

	private static ulong BuildMediaReplaceSignature(Array<Texture2D> textures, int textureCount, Array<bool> uses, int useCount, Array<Rect2> rects, int rectCount, int limit, Array<int> pages, int pageCount, bool usesTextureArray)
	{
		if (textures == null || uses == null || rects == null || limit <= 0)
		{
			return 0uL;
		}
		ulong num = 1469598103934665603uL;
		num ^= (ulong)(usesTextureArray ? 1 : 0);
		num *= 1099511628211L;
		int num2 = Math.Min(limit, Math.Min(textureCount, Math.Min(useCount, rectCount)));
		for (int i = 0; i < num2; i++)
		{
			bool flag = uses[i];
			num ^= (ulong)(flag ? 1 : 0);
			num *= 1099511628211L;
			if (flag)
			{
				Texture2D texture2D = textures[i];
				num ^= (GodotObject.IsInstanceValid(texture2D) ? texture2D.GetInstanceId() : 0);
				num *= 1099511628211L;
				Rect2 rect = rects[i];
				num ^= (ulong)((usesTextureArray && pages != null && i < pageCount) ? pages[i] : 0).GetHashCode();
				num *= 1099511628211L;
				num ^= (ulong)rect.Position.X.GetHashCode();
				num *= 1099511628211L;
				num ^= (ulong)rect.Position.Y.GetHashCode();
				num *= 1099511628211L;
				num ^= (ulong)rect.Size.X.GetHashCode();
				num *= 1099511628211L;
				num ^= (ulong)rect.Size.Y.GetHashCode();
				num *= 1099511628211L;
			}
		}
		return num;
	}

	private static float ComputeMediaReplaceBoundsGrow(Array<bool> uses, int useCount, Array<Rect2> rects, int rectCount, int limit)
	{
		if (uses == null || rects == null || limit <= 0)
		{
			return 0f;
		}
		int num = Math.Min(limit, Math.Min(useCount, rectCount));
		float num2 = 0f;
		for (int i = 0; i < num; i++)
		{
			if (uses[i])
			{
				Rect2 rect = rects[i];
				if (!(rect.Size.X <= 0f) && !(rect.Size.Y <= 0f))
				{
					num2 = Math.Max(num2, Math.Max(rect.Size.X, rect.Size.Y));
				}
			}
		}
		return num2;
	}

	private static bool HasActiveMediaReplace(Array<bool> uses, int useCount, int limit)
	{
		if (uses == null || limit <= 0)
		{
			return false;
		}
		int num = Math.Min(limit, useCount);
		for (int i = 0; i < num; i++)
		{
			if (uses[i])
			{
				return true;
			}
		}
		return false;
	}

	private int GetMediaReplaceAtlasLayer()
	{
		if (!mediaReplaceAtlasUsesTextureArray || mediaReplaceAtlasPages == null)
		{
			return 0;
		}
		for (int i = 0; i < mediaReplaceAtlasPages.Count; i++)
		{
			if (_mediaReplaceUse != null && i < _mediaReplaceUse.Count && _mediaReplaceUse[i])
			{
				return Math.Max(0, mediaReplaceAtlasPages[i]);
			}
		}
		return 0;
	}

	private int GetMediaReplaceAtlasLayer(int mediaId)
	{
		if (!mediaReplaceAtlasUsesTextureArray || mediaReplaceAtlasPages == null || mediaId < 0 || mediaId >= mediaReplaceAtlasPages.Count)
		{
			return 0;
		}
		return Math.Max(0, mediaReplaceAtlasPages[mediaId]);
	}

	private bool HasValidMediaReplaceAtlas()
	{
		if (!mediaReplaceAtlasUsesTextureArray)
		{
			return GodotObject.IsInstanceValid(mediaReplaceAtlas);
		}
		return GodotObject.IsInstanceValid(mediaReplaceAtlasArray);
	}

	private Rid GetMediaReplaceAtlasRid()
	{
		if (!mediaReplaceAtlasUsesTextureArray || !GodotObject.IsInstanceValid(mediaReplaceAtlasArray))
		{
			if (!GodotObject.IsInstanceValid(mediaReplaceAtlas))
			{
				return default;
			}
			return mediaReplaceAtlas.GetRid();
		}
		return mediaReplaceAtlasArray.GetRid();
	}

	private void MarkLayerStateChanged()
	{
		_layerStateVersion++;
		MarkEffectOnceBatchEligibilityChanged();
		InvalidateRenderSnapshotCache();
		RequestNodeRedraw();
	}

	internal virtual void OnPartSnapshotCreated(AdobeAnimateSlot slot, Array<StringName> layers, AdobeAnimatePart part)
	{
	}

	private void RequestNodeRedraw(bool preferDisplayCadence = false, bool transformOnly = false, bool staticStateDirty = true)
	{
		if (staticStateDirty && !transformOnly)
		{
			_gpuGraphCrowdStaticStateDirty = true;
		}
		if (_effectOnceGpuSuppressed)
		{
			return;
		}
		if (_usingRuntimeManager && !CachedEditorHint && _parentSprite != null)
		{
			AdobeAnimateSprite renderSortRootForRender = GetRenderSortRootForRender();
			if (GodotObject.IsInstanceValid(renderSortRootForRender) && renderSortRootForRender != this)
			{
				renderSortRootForRender.RequestNodeRedraw(preferDisplayCadence, transformOnly, staticStateDirty);
				return;
			}
		}
		if (_usingRuntimeManager && CanReuseCachedCulledPose())
		{
			return;
		}
		_needsRenderSubmission = true;
		if (_usingRuntimeManager)
		{
			if (preferDisplayCadence)
			{
				AdobeAnimateRuntimeManager.NotifyDisplayRenderSubmission(this);
			}
			else
			{
				AdobeAnimateRuntimeManager.NotifyRenderSubmission(this);
			}
		}
		else
		{
			QueueRedraw();
		}
	}

	internal bool TryGetFrameDrawOrderPose(int frame, int drawOrder, out int mediaId, out Transform2D transform)
	{
		return TryGetFramePose(frame, drawOrder, useLayerId: false, out mediaId, out transform);
	}

	internal bool TryGetInterpolatedDrawOrderPose(int drawOrder, out int mediaId, out Transform2D transform)
	{
		return TryGetInterpolatedPose(drawOrder, useLayerId: false, out mediaId, out transform);
	}

	private bool TryGetFrameLayerPose(int frame, int layerId, out int mediaId, out Transform2D transform)
	{
		return TryGetFramePose(frame, layerId, useLayerId: true, out mediaId, out transform);
	}

	internal bool TryGetFrameLayerPoseForRender(int frame, int layerId, out int mediaId, out Transform2D transform)
	{
		return TryGetFrameLayerPose(frame, layerId, out mediaId, out transform);
	}

	internal bool TryGetInterpolatedLayerPoseForRender(int layerId, out int mediaId, out Transform2D transform)
	{
		return TryGetInterpolatedPose(layerId, useLayerId: true, out mediaId, out transform);
	}

	internal void AppendInterpolatedLayerElementsForRender(HashSet<int> selectedLayerIds, Vector2 anchorOrigin, Array<Godot.Collections.Array> output)
	{
		if (selectedLayerIds == null || selectedLayerIds.Count == 0 || output == null || !TryGetRenderPoseWindow(out var definition, out var frame, out var interpolationT))
		{
			return;
		}
		int num = Math.Max(0, frame.Offset);
		int num2 = Math.Min(definition.SliceMetadata.Length, num + Math.Max(0, frame.Count));
		for (int i = num; i < num2; i++)
		{
			int layerId = definition.SliceMetadata[i].LayerId;
			if (selectedLayerIds.Contains(layerId) && (layerId < 0 || layerId >= _layerVisible.Count || _layerVisible[layerId]) && AdobeAnimateDefinitionCache.TryGetInterpolatedSlicePose(definition, i, interpolationT, out var mediaId, out var transform, out var alpha) && mediaId != 65535)
			{
				transform = transform.Translated(offset);
				transform.Origin -= anchorOrigin;
				BlendLayerElementFromPreviousClip(definition, frame, i, anchorOrigin, ref transform, ref alpha);
				output.Add(new Godot.Collections.Array
				{
					mediaId,
					transform,
					new Color(1f, 1f, 1f, alpha)
				});
			}
		}
	}

	internal int AppendInterpolatedLayerElementsForRender(HashSet<int> selectedLayerIds, Vector2 anchorOrigin, IAdobeAnimateInterpolatedElementSink sink)
	{
		if (selectedLayerIds == null || selectedLayerIds.Count == 0 || sink == null || !TryGetRenderPoseWindow(out var definition, out var frame, out var interpolationT))
		{
			return 0;
		}
		int num = 0;
		int num2 = Math.Max(0, frame.Offset);
		int num3 = Math.Min(definition.SliceMetadata.Length, num2 + Math.Max(0, frame.Count));
		for (int i = num2; i < num3; i++)
		{
			int layerId = definition.SliceMetadata[i].LayerId;
			if (selectedLayerIds.Contains(layerId) && (layerId < 0 || layerId >= _layerVisible.Count || _layerVisible[layerId]) && AdobeAnimateDefinitionCache.TryGetInterpolatedSlicePose(definition, i, interpolationT, out var mediaId, out var transform, out var alpha) && mediaId != 65535)
			{
				transform = transform.Translated(offset);
				transform.Origin -= anchorOrigin;
				BlendLayerElementFromPreviousClip(definition, frame, i, anchorOrigin, ref transform, ref alpha);
				if (sink.TryAppendInterpolatedElement(definition, this, mediaId, transform, new Color(1f, 1f, 1f, alpha)))
				{
					num++;
				}
			}
		}
		return num;
	}

	internal bool TryCreateVisibleFrameElementsForRender(out AdobeAnimateData animeData, out Array<Texture2D> mediaReplace, out Array<string> mediaReplaceAtlasPaths, out Array<Godot.Collections.Array> elements)
	{
		animeData = _flashAnimeData;
		mediaReplace = null;
		mediaReplaceAtlasPaths = null;
		elements = null;
		if (!GodotObject.IsInstanceValid(animeData) || _layerVisible == null || _layerVisible.Count == 0)
		{
			return false;
		}
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = 0; i < _layerVisible.Count; i++)
		{
			if (_layerVisible[i])
			{
				hashSet.Add(i);
			}
		}
		if (hashSet.Count == 0)
		{
			return false;
		}
		mediaReplace = CreateActiveMediaReplaceSnapshotForRender();
		mediaReplaceAtlasPaths = CreateActiveMediaReplaceAtlasPathSnapshotForRender();
		elements = new Array<Godot.Collections.Array>();
		AppendInterpolatedLayerElementsForRender(hashSet, Vector2.Zero, elements);
		return elements.Count > 0;
	}

	private bool TryGetRenderPoseWindow(out AdobeAnimateRuntimeDefinition definition, out PackedFrame frame, out float interpolationT)
	{
		definition = GetRuntimeDefinition();
		frame = default;
		interpolationT = 0f;
		if (definition == null || definition.Frames == null || definition.Frames.Length == 0 || definition.SliceMetadata == null)
		{
			return false;
		}
		float num = GetRenderFrameFloat();
		if (clipRange != Vector2I.Zero)
		{
			num = Mathf.Clamp(num, clipRange.X, Math.Max(clipRange.X, clipRange.Y - 1));
		}
		int num2 = Mathf.Clamp(Mathf.FloorToInt(num), 0, definition.Frames.Length - 1);
		interpolationT = Mathf.Clamp(num - Mathf.Floor(num), 0f, 1f);
		frame = definition.Frames[num2];
		return true;
	}

	internal Array<Texture2D> CreateActiveMediaReplaceSnapshotForRender()
	{
		Array<Texture2D> array = _mediaReplace?.Duplicate() ?? new Array<Texture2D>();
		for (int i = 0; i < array.Count; i++)
		{
			if (_mediaReplaceUse == null || i >= _mediaReplaceUse.Count || !_mediaReplaceUse[i] || !GodotObject.IsInstanceValid(array[i]))
			{
				array[i] = null;
			}
		}
		return array;
	}

	internal Array<string> CreateActiveMediaReplaceAtlasPathSnapshotForRender()
	{
		Array<string> array = _mediaReplaceAtlasPaths?.Duplicate() ?? new Array<string>();
		for (int i = 0; i < array.Count; i++)
		{
			if (_mediaReplaceUse == null || i >= _mediaReplaceUse.Count || !_mediaReplaceUse[i] || string.IsNullOrWhiteSpace(array[i]))
			{
				array[i] = string.Empty;
			}
		}
		return array;
	}

	internal Texture2D GetActiveMediaReplacementForRender(int mediaId)
	{
		if (mediaId < 0 || _mediaReplaceUse == null || mediaId >= _mediaReplaceUse.Count || !_mediaReplaceUse[mediaId] || _mediaReplace == null || mediaId >= _mediaReplace.Count || !GodotObject.IsInstanceValid(_mediaReplace[mediaId]))
		{
			return null;
		}
		return _mediaReplace[mediaId];
	}

	internal string GetActiveMediaReplacementAtlasPathForRender(int mediaId)
	{
		if (mediaId < 0 || _mediaReplaceUse == null || mediaId >= _mediaReplaceUse.Count || !_mediaReplaceUse[mediaId] || _mediaReplaceAtlasPaths == null || mediaId >= _mediaReplaceAtlasPaths.Count || string.IsNullOrWhiteSpace(_mediaReplaceAtlasPaths[mediaId]))
		{
			return string.Empty;
		}
		return _mediaReplaceAtlasPaths[mediaId];
	}

	internal Vector2 GetMediaSizeForRender(int mediaId)
	{
		if (mediaId >= 0 && _mediaReplaceUse != null && mediaId < _mediaReplaceUse.Count && _mediaReplaceUse[mediaId] && _mediaReplace != null && mediaId < _mediaReplace.Count && GodotObject.IsInstanceValid(_mediaReplace[mediaId]))
		{
			Vector2 size = _mediaReplace[mediaId].GetSize();
			if (size.X > 0f && size.Y > 0f)
			{
				return size;
			}
		}
		if (mediaId >= 0 && _mediaReplaceUse != null && mediaId < _mediaReplaceUse.Count && _mediaReplaceUse[mediaId] && mediaId < _mediaReplaceAtlasPaths.Count && !string.IsNullOrWhiteSpace(_mediaReplaceAtlasPaths[mediaId]) && AdobeAnimateGlobalAtlasCache.TryGetReplaceTextureAllocation(_mediaReplaceAtlasPaths[mediaId], out var allocation) && allocation.Rect.Size.X > 0f && allocation.Rect.Size.Y > 0f)
		{
			return allocation.Rect.Size;
		}
		if (_flashAnimeData == null)
		{
			return Vector2.Zero;
		}
		return _flashAnimeData.GetMediaRect(mediaId).Size;
	}

	internal bool TryGetFrameSlotPose(int frame, int slotLayerId, out int mediaId, out Transform2D transform)
	{
		if (slotLayerId < 0)
		{
			mediaId = 65535;
			transform = Transform2D.Identity;
			return false;
		}
		if (TryGetFrameLayerPose(frame, slotLayerId, out mediaId, out transform))
		{
			return true;
		}
		if (ShouldFallbackSlotPoseToDrawOrder(slotLayerId))
		{
			return TryGetFrameDrawOrderPose(frame, slotLayerId, out mediaId, out transform);
		}
		return false;
	}

	private bool TryGetFrameLayerDrawOrder(int frame, int layerId, out int drawOrder)
	{
		drawOrder = -1;
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (runtimeDefinition == null || runtimeDefinition.Frames == null || runtimeDefinition.SliceMetadata == null || runtimeDefinition.Frames.Length == 0 || layerId < 0)
		{
			return false;
		}
		int num = Mathf.Clamp(frame, 0, runtimeDefinition.Frames.Length - 1);
		PackedFrame frame2 = runtimeDefinition.Frames[num];
		if (AdobeAnimateDefinitionCache.TryGetFrameLayerMaxDrawOrder(runtimeDefinition, num, frame2, layerId, out drawOrder))
		{
			return true;
		}
		int num2 = frame2.Offset + frame2.Count;
		for (int i = frame2.Offset; i < num2; i++)
		{
			PackedSliceMetadata packedSliceMetadata = runtimeDefinition.SliceMetadata[i];
			if (packedSliceMetadata.LayerId == layerId)
			{
				drawOrder = Math.Max(drawOrder, packedSliceMetadata.DrawOrder);
			}
		}
		return drawOrder >= 0;
	}

	private bool TryGetInterpolatedLayerPose(int layerId, out int mediaId, out Transform2D transform)
	{
		return TryGetInterpolatedPose(layerId, useLayerId: true, out mediaId, out transform);
	}

	internal bool TryGetInterpolatedSlotPose(int slotLayerId, out int mediaId, out Transform2D transform)
	{
		if (slotLayerId < 0)
		{
			mediaId = 65535;
			transform = Transform2D.Identity;
			return false;
		}
		if (TryGetInterpolatedLayerPose(slotLayerId, out mediaId, out transform))
		{
			return true;
		}
		if (ShouldFallbackSlotPoseToDrawOrder(slotLayerId))
		{
			return TryGetInterpolatedDrawOrderPose(slotLayerId, out mediaId, out transform);
		}
		return false;
	}

	private bool TryGetInterpolatedPose(int key, bool useLayerId, out int mediaId, out Transform2D transform)
	{
		mediaId = 65535;
		transform = Transform2D.Identity;
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (runtimeDefinition == null || runtimeDefinition.Frames == null || runtimeDefinition.Frames.Length == 0)
		{
			return false;
		}
		if (!TryGetCachedManagedPoseTrack(runtimeDefinition, key, useLayerId, out var array))
		{
			return false;
		}
		if (!TryGetPoseAtFrameFloat(runtimeDefinition, array, GetRenderFrameFloat(), out mediaId, out transform))
		{
			return false;
		}
		BlendPoseTransformFromPreviousClip(runtimeDefinition, array, ref transform);
		return true;
	}

	private bool TryGetCachedManagedPoseTrack(AdobeAnimateRuntimeDefinition definition, int key, bool useLayerId, out PackedCpuPoseSample[] track)
	{
		if (_cachedManagedPoseTrackDefinition == definition && _cachedManagedPoseTrackKey == key && _cachedManagedPoseTrackUseLayerId == useLayerId && _cachedManagedPoseTrack.Length == definition.Frames.Length)
		{
			track = _cachedManagedPoseTrack;
			return true;
		}
		if (!AdobeAnimateDefinitionCache.TryGetCpuPoseTrack(definition, key, useLayerId, out track))
		{
			return false;
		}
		_cachedManagedPoseTrackDefinition = definition;
		_cachedManagedPoseTrackKey = key;
		_cachedManagedPoseTrackUseLayerId = useLayerId;
		_cachedManagedPoseTrack = track;
		return true;
	}

	private void ResetManagedPoseTrackCache()
	{
		_cachedManagedPoseTrackDefinition = null;
		_cachedManagedPoseTrackKey = -1;
		_cachedManagedPoseTrackUseLayerId = false;
		_cachedManagedPoseTrack = System.Array.Empty<PackedCpuPoseSample>();
	}

	private static bool TryGetPoseAtFrameFloat(AdobeAnimateRuntimeDefinition definition, PackedCpuPoseSample[] track, float frameFloat, out int mediaId, out Transform2D transform)
	{
		mediaId = 65535;
		transform = Transform2D.Identity;
		if (definition == null || definition.Frames == null || definition.Frames.Length == 0 || track == null || track.Length != definition.Frames.Length)
		{
			return false;
		}
		int num = Mathf.Clamp(Mathf.FloorToInt(frameFloat), 0, definition.Frames.Length - 1);
		ref PackedCpuPoseSample reference = ref track[num];
		if (!reference.Valid)
		{
			return false;
		}
		mediaId = reference.MediaId;
		Transform2D source = (transform = reference.Transform);
		float num2 = Mathf.Clamp(frameFloat - Mathf.Floor(frameFloat), 0f, 1f);
		int num3 = Mathf.Clamp(num + 1, 0, definition.Frames.Length - 1);
		if (num2 <= 0f || num3 == num || !track[num3].Valid || track[num3].MediaId != mediaId)
		{
			return true;
		}
		transform = LerpPoseTransform(source, track[num3].Transform, num2);
		return true;
	}

	private void BlendPoseTransformFromPreviousClip(AdobeAnimateRuntimeDefinition definition, PackedCpuPoseSample[] track, ref Transform2D targetTransform)
	{
		AdobeAnimateClipBlendState clipBlendStateForRender = GetClipBlendStateForRender();
		if (clipBlendStateForRender.Enabled && !(clipBlendStateForRender.Weight >= 1f) && TryGetPoseAtFrameFloat(definition, track, clipBlendStateForRender.FromFrameFloat, out var _, out var transform))
		{
			targetTransform = LerpPoseTransform(transform, targetTransform, clipBlendStateForRender.Weight);
		}
	}

	private void BlendLayerElementFromPreviousClip(AdobeAnimateRuntimeDefinition definition, PackedFrame targetFrame, int targetSourceIndex, Vector2 anchorOrigin, ref Transform2D targetTransform, ref float targetAlpha)
	{
		AdobeAnimateClipBlendState clipBlendStateForRender = GetClipBlendStateForRender();
		if (!clipBlendStateForRender.Enabled || clipBlendStateForRender.Weight >= 1f || definition?.Frames == null || definition.Frames.Length == 0 || targetSourceIndex < 0)
		{
			return;
		}
		int num = Mathf.Clamp(Mathf.FloorToInt(clipBlendStateForRender.FromFrameFloat), 0, definition.Frames.Length - 1);
		PackedFrame sourceFrame = definition.Frames[num];
		if (AdobeAnimateDefinitionCache.TryGetMatchingSliceIndexInFrame(definition, sourceFrame, targetFrame, targetSourceIndex, out var sourceAbsoluteIndex))
		{
			float interpolationT = Mathf.Clamp(clipBlendStateForRender.FromFrameFloat - Mathf.Floor(clipBlendStateForRender.FromFrameFloat), 0f, 1f);
			if (AdobeAnimateDefinitionCache.TryGetInterpolatedSlicePose(definition, sourceAbsoluteIndex, interpolationT, out var mediaId, out var transform, out var alpha) && mediaId != 65535)
			{
				transform = transform.Translated(offset);
				transform.Origin -= anchorOrigin;
				targetTransform = LerpPoseTransform(transform, targetTransform, clipBlendStateForRender.Weight);
				targetAlpha = Mathf.Lerp(alpha, targetAlpha, clipBlendStateForRender.Weight);
			}
		}
	}

	private static Transform2D LerpPoseTransform(Transform2D source, Transform2D target, float weight)
	{
		float num = Mathf.Clamp(weight, 0f, 1f);
		return new Transform2D(source.X + (target.X - source.X) * num, source.Y + (target.Y - source.Y) * num, source.Origin + (target.Origin - source.Origin) * num);
	}

	private bool TryGetFramePose(int frame, int key, bool useLayerId, out int mediaId, out Transform2D transform)
	{
		mediaId = 65535;
		transform = Transform2D.Identity;
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (runtimeDefinition == null || runtimeDefinition.Frames == null || runtimeDefinition.SliceMetadata == null || runtimeDefinition.Frames.Length == 0 || key < 0)
		{
			return false;
		}
		return TryGetFramePose(runtimeDefinition, frame, key, useLayerId, out mediaId, out transform);
	}

	private static bool TryGetFramePose(AdobeAnimateRuntimeDefinition definition, int frame, int key, bool useLayerId, out int mediaId, out Transform2D transform)
	{
		mediaId = 65535;
		transform = Transform2D.Identity;
		if (definition == null || definition.Frames == null || definition.SliceMetadata == null || definition.Frames.Length == 0 || key < 0)
		{
			return false;
		}
		return AdobeAnimateDefinitionCache.TryGetCpuPoseSample(definition, Mathf.Clamp(frame, 0, definition.Frames.Length - 1), key, useLayerId, out mediaId, out transform);
	}

	private static Transform2D CreateSliceTransform(PackedSlicePose slice)
	{
		return new Transform2D(new Vector2(slice.Xx, slice.Xy), new Vector2(slice.Yx, slice.Yy), new Vector2(slice.Ox, slice.Oy));
	}

	private bool HasInsertedSpritesForRender()
	{
		for (int i = 0; i < _insertedSprites.Count; i++)
		{
			AdobeAnimateSprite sprite = _insertedSprites[i].sprite;
			if (GodotObject.IsInstanceValid(sprite) && sprite._parentSprite == this)
			{
				return true;
			}
		}
		return false;
	}

	internal bool HasSpriteChildrenForRender()
	{
		if (_managedSlotSpritesDirty)
		{
			RefreshManagedSlotSpriteOwnerCache();
		}
		for (int i = 0; i < _spriteChildren.Length; i++)
		{
			if (GodotObject.IsInstanceValid(_spriteChildren[i]))
			{
				return true;
			}
		}
		return false;
	}

	private bool NeedsDrawOrderSortBandsForRender()
	{
		if (!HasInsertedSpritesForRender() && !HasManagedSlotSpritesForRender())
		{
			return HasSpriteChildrenForRender();
		}
		return true;
	}

	private bool GetNeedsDrawOrderSortBandsForRender()
	{
		if (_drawOrderSortBandCacheDirty || _managedSlotSpritesDirty)
		{
			_needsDrawOrderSortBandsCached = NeedsDrawOrderSortBandsForRender();
			_drawOrderSortBandCacheDirty = false;
		}
		return _needsDrawOrderSortBandsCached;
	}

	internal bool HasManagedSlotSpritesForRender()
	{
		return GetManagedSlotSpritesForRender().Length != 0;
	}

	internal AdobeAnimateManagedSlotSprite[] GetManagedSlotSpritesForRender()
	{
		if (!_managedSlotSpritesDirty)
		{
			return _managedSlotSprites;
		}
		RefreshManagedSlotSpriteOwnerCache();
		List<AdobeAnimateManagedSlotSprite> list = new List<AdobeAnimateManagedSlotSprite>();
		for (int i = 0; i < _slotChildren.Length; i++)
		{
			AdobeAnimateSlot adobeAnimateSlot = _slotChildren[i];
			if (adobeAnimateSlot != null && GodotObject.IsInstanceValid(adobeAnimateSlot))
			{
				adobeAnimateSlot.TryCollectManagedSprite2DChildren(list);
			}
		}
		_managedSlotSprites = ((list.Count > 0) ? list.ToArray() : System.Array.Empty<AdobeAnimateManagedSlotSprite>());
		RefreshManagedSlotGpuStateWatchers(_managedSlotSprites);
		ResetManagedSlotGpuStateCache();
		_managedSlotSpritesDirty = false;
		return _managedSlotSprites;
	}

	internal AdobeAnimateSprite[] GetSpriteChildrenForRender()
	{
		if (_managedSlotSpritesDirty)
		{
			RefreshManagedSlotSpriteOwnerCache();
		}
		return _spriteChildren ?? System.Array.Empty<AdobeAnimateSprite>();
	}

	internal bool IsEmptyHiddenGpuGraphPlaceholderForRender()
	{
		if (_flashAnimeData == null && !Visible && GetChildCount() == 0)
		{
			return !HasExternalVisualsForRender();
		}
		return false;
	}

	internal void PrepareDetachedGpuGraphWarmup()
	{
		if (!GodotObject.IsInstanceValid(this) || IsInsideTree())
		{
			return;
		}
		ApplyPendingFlashAnimeDataChange();
		TryHydrateFlashAnimeDataBeforeFileChange();
		if (HasUsableFlashAnimeData())
		{
			if (HasAnyRequestedMediaReplace())
			{
				UpdateMediaReplaceData();
			}
			parentSprite = FindParentSpriteAncestor();
			CacheChildren();
			RefreshRenderStaticStateCache(force: true);
			GetRuntimeDefinition();
		}
	}

	private void RefreshManagedSlotSpriteOwnerCache()
	{
		if (GodotObject.IsInstanceValid(this))
		{
			List<AdobeAnimateSlot> list = new List<AdobeAnimateSlot>();
			List<AdobeAnimateSprite> list2 = new List<AdobeAnimateSprite>();
			List<AdobeAnimateSlot> list3 = new List<AdobeAnimateSlot>();
			CollectOwnedChildren(this, list, list2, list3, collectSpriteChildren: true, null);
			if (!SameSlotChildren(_slotChildren, list))
			{
				_slotChildren = list.ToArray();
				MarkSlotRuntimeUpdateCacheDirty();
			}
			if (!SameSpriteChildren(_spriteChildren, list2) || !SameSpriteChildOwnerSlots(_spriteChildOwnerSlots, list3))
			{
				_spriteChildren = list2.ToArray();
				_spriteChildOwnerSlots = list3.ToArray();
				_drawOrderSortBandCacheDirty = true;
			}
			_hasChildren = _slotChildren.Length != 0 || _spriteChildren.Length != 0;
			RefreshRuntimeChildUpdatePresence();
		}
	}

	private static bool SameSlotChildren(AdobeAnimateSlot[] cached, List<AdobeAnimateSlot> current)
	{
		if (cached == null || cached.Length != current.Count)
		{
			return false;
		}
		for (int i = 0; i < cached.Length; i++)
		{
			if (cached[i] != current[i])
			{
				return false;
			}
		}
		return true;
	}

	private static bool SameSpriteChildren(AdobeAnimateSprite[] cached, List<AdobeAnimateSprite> current)
	{
		if (cached == null || cached.Length != current.Count)
		{
			return false;
		}
		for (int i = 0; i < cached.Length; i++)
		{
			if (cached[i] != current[i])
			{
				return false;
			}
		}
		return true;
	}

	private static bool SameSpriteChildOwnerSlots(AdobeAnimateSlot[] cached, List<AdobeAnimateSlot> current)
	{
		if (cached == null || cached.Length != current.Count)
		{
			return false;
		}
		for (int i = 0; i < cached.Length; i++)
		{
			if (cached[i] != current[i])
			{
				return false;
			}
		}
		return true;
	}

	internal void MarkManagedSlotSpriteCacheDirty()
	{
		InvalidateRuntimeNativeCanvasSuppressionEligibility();
		_managedSlotSpritesDirty = true;
		DisconnectManagedSlotGpuStateWatchers();
		ResetManagedSlotGpuStateCache();
		_drawOrderSortBandCacheDirty = true;
		AdobeAnimateRenderManager.InvalidateGpuRenderGraph(GetRenderSortRootForRender(), AdobeAnimateGpuGraphInvalidationReason.ManagedSlotTopology);
		InvalidateRenderSnapshotCache();
		RequestNodeRedraw();
	}

	internal bool TryBuildManagedSlotGpuStateForRender(int managedSpriteIndex, Rid expectedAtlasArrayRid, out AdobeAnimateGpuManagedVisualState state, out string failureReason)
	{
		state = default;
		failureReason = "";
		AdobeAnimateManagedSlotSprite[] managedSlotSpritesForRender = GetManagedSlotSpritesForRender();
		if ((uint)managedSpriteIndex >= (uint)managedSlotSpritesForRender.Length)
		{
			failureReason = $"managed Slot visual index {managedSpriteIndex} is out of range";
			return false;
		}
		EnsureManagedSlotGpuStateCache(managedSlotSpritesForRender.Length);
		AdobeAnimateManagedSlotSprite pair = managedSlotSpritesForRender[managedSpriteIndex];
		ulong num = AdobeAnimateManagedSprite2D.BuildGpuStateFastSignature(in pair);
		bool flag = _managedSlotGpuStateAtlasRids[managedSpriteIndex] == expectedAtlasArrayRid;
		if ((_managedSlotGpuStateKnown[managedSpriteIndex] & flag) && _managedSlotGpuStateFastSignatures[managedSpriteIndex] == num && _managedSlotGpuStateRescanCountdowns[managedSpriteIndex] > 0)
		{
			_managedSlotGpuStateRescanCountdowns[managedSpriteIndex]--;
			state = _managedSlotGpuStates[managedSpriteIndex];
			if (!_managedSlotGpuStateValid[managedSpriteIndex])
			{
				failureReason = "cached managed Slot visual GPU state is invalid";
			}
			return _managedSlotGpuStateValid[managedSpriteIndex];
		}
		ulong num2 = AdobeAnimateManagedSprite2D.BuildGpuStateSignature(in pair);
		if ((_managedSlotGpuStateKnown[managedSpriteIndex] && _managedSlotGpuStateSignatures[managedSpriteIndex] == num2) & flag)
		{
			_managedSlotGpuStateFastSignatures[managedSpriteIndex] = num;
			_managedSlotGpuStateRescanCountdowns[managedSpriteIndex] = GetManagedSlotGpuStateRescanInterval(managedSpriteIndex);
			state = _managedSlotGpuStates[managedSpriteIndex];
			if (!_managedSlotGpuStateValid[managedSpriteIndex])
			{
				failureReason = "cached managed Slot visual GPU state is invalid";
			}
			return _managedSlotGpuStateValid[managedSpriteIndex];
		}
		bool flag2 = AdobeAnimateManagedSprite2D.TryBuildGpuState(in pair, expectedAtlasArrayRid, out state, out failureReason);
		if (flag2)
		{
			_managedSlotGpuStateKnown[managedSpriteIndex] = true;
			_managedSlotGpuStateValid[managedSpriteIndex] = true;
			_managedSlotGpuStateSignatures[managedSpriteIndex] = num2;
			_managedSlotGpuStateFastSignatures[managedSpriteIndex] = num;
			_managedSlotGpuStateRescanCountdowns[managedSpriteIndex] = GetManagedSlotGpuStateRescanInterval(managedSpriteIndex);
			_managedSlotGpuStateAtlasRids[managedSpriteIndex] = expectedAtlasArrayRid;
			_managedSlotGpuStates[managedSpriteIndex] = state;
			return flag2;
		}
		_managedSlotGpuStateKnown[managedSpriteIndex] = false;
		_managedSlotGpuStateRescanCountdowns[managedSpriteIndex] = 0;
		return flag2;
	}

	private void EnsureManagedSlotGpuStateCache(int count)
	{
		if (_managedSlotGpuStates.Length != count)
		{
			_managedSlotGpuStateSignatures = new ulong[count];
			_managedSlotGpuStateAtlasRids = new Rid[count];
			_managedSlotGpuStates = new AdobeAnimateGpuManagedVisualState[count];
			_managedSlotGpuStateFastSignatures = new ulong[count];
			_managedSlotGpuStateRescanCountdowns = new int[count];
			_managedSlotGpuStateKnown = new bool[count];
			_managedSlotGpuStateValid = new bool[count];
		}
	}

	private void ResetManagedSlotGpuStateCache()
	{
		_managedSlotGpuStateSignatures = System.Array.Empty<ulong>();
		_managedSlotGpuStateAtlasRids = System.Array.Empty<Rid>();
		_managedSlotGpuStates = System.Array.Empty<AdobeAnimateGpuManagedVisualState>();
		_managedSlotGpuStateFastSignatures = System.Array.Empty<ulong>();
		_managedSlotGpuStateRescanCountdowns = System.Array.Empty<int>();
		_managedSlotGpuStateKnown = System.Array.Empty<bool>();
		_managedSlotGpuStateValid = System.Array.Empty<bool>();
	}

	internal void MarkManagedSlotVisualStateChanged()
	{
		if (_managedSlotGpuStateKnown.Length != 0)
		{
			System.Array.Fill(_managedSlotGpuStateKnown, value: false);
		}
		InvalidateRenderSnapshotCache();
		RequestNodeRedraw();
	}

	private int GetManagedSlotGpuStateRescanInterval(int managedSpriteIndex)
	{
		return GetStaggeredRescanInterval(30) + managedSpriteIndex % 30;
	}

	private void RefreshManagedSlotGpuStateWatchers(AdobeAnimateManagedSlotSprite[] managedSprites)
	{
		DisconnectManagedSlotGpuStateWatchers();
		if (managedSprites == null || managedSprites.Length == 0)
		{
			return;
		}
		List<Sprite2D> list = new List<Sprite2D>(managedSprites.Length);
		List<AdobeAnimatePart> list2 = new List<AdobeAnimatePart>(managedSprites.Length);
		for (int i = 0; i < managedSprites.Length; i++)
		{
			Sprite2D sprite = managedSprites[i].Sprite;
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.TextureChanged += MarkManagedSlotVisualStateChanged;
				sprite.FrameChanged += MarkManagedSlotVisualStateChanged;
				sprite.ItemRectChanged += MarkManagedSlotVisualStateChanged;
				sprite.VisibilityChanged += MarkManagedSlotVisualStateChanged;
				list.Add(sprite);
				continue;
			}
			AdobeAnimatePart atlasPart = managedSprites[i].AtlasPart;
			if (GodotObject.IsInstanceValid(atlasPart))
			{
				atlasPart.ItemRectChanged += MarkManagedSlotVisualStateChanged;
				atlasPart.VisibilityChanged += MarkManagedSlotVisualStateChanged;
				list2.Add(atlasPart);
			}
		}
		_managedSlotGpuStateWatchedSprites = ((list.Count > 0) ? list.ToArray() : System.Array.Empty<Sprite2D>());
		_managedSlotGpuStateWatchedParts = ((list2.Count > 0) ? list2.ToArray() : System.Array.Empty<AdobeAnimatePart>());
	}

	private void DisconnectManagedSlotGpuStateWatchers()
	{
		for (int i = 0; i < _managedSlotGpuStateWatchedSprites.Length; i++)
		{
			Sprite2D sprite2D = _managedSlotGpuStateWatchedSprites[i];
			if (GodotObject.IsInstanceValid(sprite2D))
			{
				sprite2D.TextureChanged -= MarkManagedSlotVisualStateChanged;
				sprite2D.FrameChanged -= MarkManagedSlotVisualStateChanged;
				sprite2D.ItemRectChanged -= MarkManagedSlotVisualStateChanged;
				sprite2D.VisibilityChanged -= MarkManagedSlotVisualStateChanged;
			}
		}
		_managedSlotGpuStateWatchedSprites = System.Array.Empty<Sprite2D>();
		for (int j = 0; j < _managedSlotGpuStateWatchedParts.Length; j++)
		{
			AdobeAnimatePart adobeAnimatePart = _managedSlotGpuStateWatchedParts[j];
			if (GodotObject.IsInstanceValid(adobeAnimatePart))
			{
				adobeAnimatePart.ItemRectChanged -= MarkManagedSlotVisualStateChanged;
				adobeAnimatePart.VisibilityChanged -= MarkManagedSlotVisualStateChanged;
			}
		}
		_managedSlotGpuStateWatchedParts = System.Array.Empty<AdobeAnimatePart>();
	}

	internal void RefreshManagedSlotSpriteCacheForRender()
	{
		MarkManagedSlotSpriteCacheDirty();
		GetManagedSlotSpritesForRender();
	}

	internal AdobeAnimateSprite GetRenderSortRootForRender()
	{
		AdobeAnimateSprite adobeAnimateSprite = this;
		AdobeAnimateSprite adobeAnimateSprite2 = _parentSprite;
		int num = 0;
		while (GodotObject.IsInstanceValid(adobeAnimateSprite2) && adobeAnimateSprite2.OwnsSpriteChildForRender(adobeAnimateSprite) && num++ < 64)
		{
			adobeAnimateSprite = adobeAnimateSprite2;
			adobeAnimateSprite2 = adobeAnimateSprite2._parentSprite;
		}
		return adobeAnimateSprite;
	}

	private bool OwnsSpriteChildForRender(AdobeAnimateSprite child)
	{
		if (!GodotObject.IsInstanceValid(child) || child._parentSprite != this)
		{
			return false;
		}
		for (int i = 0; i < _spriteChildren.Length; i++)
		{
			if (_spriteChildren[i] == child)
			{
				return true;
			}
		}
		for (int j = 0; j < _insertedSprites.Count; j++)
		{
			if (_insertedSprites[j].sprite == child)
			{
				return true;
			}
		}
		return false;
	}

	private AdobeAnimateSprite FindParentSpriteAncestor()
	{
		Node parent = GetParent();
		int num = 0;
		while (GodotObject.IsInstanceValid(parent) && num++ < 64)
		{
			if (parent is AdobeAnimateSprite result)
			{
				return result;
			}
			parent = parent.GetParent();
		}
		return null;
	}

	internal bool TryGetManagedSlotTransformForRender(AdobeAnimateSlot slot, out Transform2D transform)
	{
		transform = Transform2D.Identity;
		if (slot == null || !GodotObject.IsInstanceValid(slot) || slot.mode != 0)
		{
			return false;
		}
		int slotLayerId = slot.followSlotId - 1;
		if (!TryGetInterpolatedSlotPoseFast(slotLayerId, out var mediaId, out var transform2) || mediaId == 65535 || transform2 == Transform2D.Identity)
		{
			return false;
		}
		Transform2D transform2D = transform2.Translated(offset).TranslatedLocal(slot.offset);
		if (!slot.useRotate)
		{
			transform2D = transform2D.RotatedLocal(0f - transform2D.Rotation);
		}
		if (!slot.useScale)
		{
			Vector2 scale = transform2D.Scale;
			if (scale.X != 0f && scale.Y != 0f)
			{
				transform2D = transform2D.ScaledLocal(Vector2.One / scale);
			}
		}
		if (!slot.useSkew)
		{
			transform2D = new Transform2D(transform2D.Rotation, transform2D.Scale, 0f, transform2D.Origin);
		}
		transform = transform2D;
		return true;
	}

	internal bool TryGetManagedSlotPositionForRender(AdobeAnimateSlot slot, out Vector2 position)
	{
		position = Vector2.Zero;
		if (slot == null || !GodotObject.IsInstanceValid(slot) || slot.mode != 0)
		{
			return false;
		}
		int slotLayerId = slot.followSlotId - 1;
		if (!TryGetInterpolatedSlotPoseFast(slotLayerId, out var mediaId, out var transform) || mediaId == 65535 || transform == Transform2D.Identity)
		{
			return false;
		}
		position = transform.Translated(offset).TranslatedLocal(slot.offset).Origin;
		return true;
	}

	internal bool TryResolveLayerIdForRender(StringName layerName, out int layerId)
	{
		layerId = -1;
		if (layerName == null || layerName.IsEmpty || _flashAnimeData?.layerDictionary == null || !_flashAnimeData.layerDictionary.ContainsKey(layerName))
		{
			return false;
		}
		layerId = _flashAnimeData.layerDictionary[layerName].AsInt32();
		return layerId >= 0;
	}

	internal bool TryGetManagedLayerPositionForRender(int layerId, Vector2 localOffset, out Vector2 position)
	{
		position = Vector2.Zero;
		if (!TryGetInterpolatedSlotPoseFast(layerId, out var mediaId, out var transform) || mediaId == 65535 || transform == Transform2D.Identity)
		{
			return false;
		}
		position = transform.Translated(offset).TranslatedLocal(localOffset).Origin;
		return true;
	}

	private bool TryGetInterpolatedSlotPoseFast(int slotLayerId, out int mediaId, out Transform2D transform)
	{
		if (slotLayerId < 0)
		{
			mediaId = 65535;
			transform = Transform2D.Identity;
			return false;
		}
		if (TryGetInterpolatedLayerPose(slotLayerId, out mediaId, out transform))
		{
			return true;
		}
		if (ShouldFallbackSlotPoseToDrawOrder(slotLayerId))
		{
			return TryGetInterpolatedDrawOrderPoseFast(slotLayerId, out mediaId, out transform);
		}
		return false;
	}

	private bool ShouldFallbackSlotPoseToDrawOrder(int slotLayerId)
	{
		if (slotLayerId < 0)
		{
			return false;
		}
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (runtimeDefinition != null)
		{
			return slotLayerId >= runtimeDefinition.RuntimeLayerCount;
		}
		return !LayerDictionaryContainsId(slotLayerId);
	}

	private bool LayerDictionaryContainsId(int layerId)
	{
		Dictionary dictionary = _flashAnimeData?.layerDictionary;
		if (dictionary == null || dictionary.Count == 0)
		{
			return false;
		}
		foreach (Variant key in dictionary.Keys)
		{
			if (dictionary[key].AsInt32() == layerId)
			{
				return true;
			}
		}
		return false;
	}

	private bool TryGetInterpolatedDrawOrderPoseFast(int drawOrder, out int mediaId, out Transform2D transform)
	{
		return TryGetInterpolatedPose(drawOrder, useLayerId: false, out mediaId, out transform);
	}

	internal bool TryGetFrameDrawOrderForRender(int frame, int layerId, out int drawOrder)
	{
		return TryGetFrameLayerDrawOrder(frame, layerId, out drawOrder);
	}

	internal static long ResolveLayerSortBand(long parentSortBand, int drawOrder, long offset = 0L)
	{
		int num = Math.Max(0, drawOrder);
		long num2 = ((parentSortBand == 0L) ? 1000000 : 1);
		return parentSortBand + num * num2 + offset;
	}

	internal static long ResolveManagedSpriteLayerSortBand(long parentSortBand, int drawOrder, long characterSortBand)
	{
		int num = Math.Max(0, drawOrder);
		long num2 = ((parentSortBand == 0L) ? 1000000 : 1);
		return characterSortBand + parentSortBand + num * num2 + 1;
	}

	private long ResolveChildSortBand(int layerId)
	{
		if (!TryGetFrameLayerDrawOrder(frameIndex, layerId, out var drawOrder))
		{
			drawOrder = Math.Max(0, layerId);
		}
		return ResolveLayerSortBand(_renderSortBand, drawOrder, 1L);
	}

	private bool TryGetInsertedLayerForChild(AdobeAnimateSprite child, out int layerId)
	{
		if (!GodotObject.IsInstanceValid(child) || child._parentSprite != this)
		{
			layerId = -1;
			return false;
		}
		for (int i = 0; i < _insertedSprites.Count; i++)
		{
			InsertedSprite insertedSprite = _insertedSprites[i];
			if (insertedSprite.sprite == child)
			{
				layerId = insertedSprite.layerId;
				return true;
			}
		}
		layerId = -1;
		return false;
	}

	internal bool TryGetChildRenderLayerForRender(AdobeAnimateSprite child, out int layerId)
	{
		if (TryGetInsertedLayerForChild(child, out layerId))
		{
			return true;
		}
		if (TryGetTrackedChildRenderLayerForRender(child, out layerId))
		{
			return layerId >= 0;
		}
		if (GodotObject.IsInstanceValid(child) && child._parentSprite == this)
		{
			layerId = ResolveSpriteChildInsertLayer(child);
			return layerId >= 0;
		}
		layerId = -1;
		return false;
	}

	private bool TryGetTrackedChildRenderLayerForRender(AdobeAnimateSprite child, out int layerId)
	{
		for (int i = 0; i < _spriteChildren.Length; i++)
		{
			if (_spriteChildren[i] == child)
			{
				bool isInserted = TryGetInsertedLayerForChild(child, out var layerId2);
				layerId = ResolveSpriteChildRenderLayer(child, i, isInserted, layerId2);
				return true;
			}
		}
		layerId = -1;
		return false;
	}

	private bool IsSpriteChildOwnedBySlot(int index)
	{
		if (index >= 0 && index < _spriteChildOwnerSlots.Length)
		{
			return GodotObject.IsInstanceValid(_spriteChildOwnerSlots[index]);
		}
		return false;
	}

	private int ResolveSpriteChildRenderLayer(AdobeAnimateSprite child, int childIndex, bool isInserted, int insertedLayerId)
	{
		if (isInserted)
		{
			return insertedLayerId;
		}
		return ResolveSpriteChildInsertLayer(child);
	}

	private int ResolveSpriteChildInsertLayer(AdobeAnimateSprite child)
	{
		if (!GodotObject.IsInstanceValid(child))
		{
			return -1;
		}
		if (child.insertLayerId >= 0)
		{
			return child.insertLayerId;
		}
		return ResolveTopInsertLayerId();
	}

	private int ResolveTopInsertLayerId()
	{
		int layerVisibleCountForRender = GetLayerVisibleCountForRender();
		if (layerVisibleCountForRender <= 0)
		{
			return -1;
		}
		return layerVisibleCountForRender;
	}

	private int ResolveSpriteChildFollowLayer(AdobeAnimateSprite child, int childIndex)
	{
		if (!GodotObject.IsInstanceValid(child))
		{
			return -1;
		}
		if (IsSpriteChildOwnedBySlot(childIndex))
		{
			return _spriteChildOwnerSlots[childIndex].followSlotId - 1;
		}
		return child.followParentSpriteLayerId;
	}

	private void ApplyChildRenderSort(AdobeAnimateSprite child, int layerId)
	{
		if (child != null && child.IsRuntimeInsideTree)
		{
			long num = ResolveChildSortBand(layerId);
			if (child._renderSortBand != num)
			{
				child._renderSortBand = num;
				child.InvalidateRenderSnapshotCache();
				child.RequestNodeRedraw();
			}
		}
	}

	private static void ClearInsertedChildRenderSort(AdobeAnimateSprite child)
	{
		if (child != null && child.IsRuntimeInsideTree && child._renderSortBand != 0L)
		{
			child._renderSortBand = 0L;
			child.InvalidateRenderSnapshotCache();
			child.RequestNodeRedraw();
		}
	}

	private void ResolveRenderPresentationForRender(Node renderMountParent, out Color modulate, out int effectiveZIndex)
	{
		modulate = GetEffectiveRenderModulate(renderMountParent);
		effectiveZIndex = GetEffectiveZIndexForRender();
	}

	private void ApplyKnownGlobalTranslationForRender(Vector2 globalDelta, ulong physicsFrame)
	{
		if (!_renderGlobalTransformCached || _renderGlobalTransformDirty)
		{
			MarkRenderTransformDirty(changedInPhysicsFrame: true);
			return;
		}
		Transform2D renderGlobalTransform = _renderGlobalTransform;
		Transform2D transform2D = renderGlobalTransform;
		transform2D.Origin += globalDelta;
		bool flag = _renderRootMotionEnabled && physicsFrame == _renderGlobalTransformPhysicsFrame;
		Transform2D transform2D2 = (flag ? _renderPreviousGlobalTransform : renderGlobalTransform);
		bool flag2 = CanInterpolateRenderRootMotion() && (flag || physicsFrame != _renderGlobalTransformPhysicsFrame) && IsContinuousRenderRootMotion(transform2D2, transform2D);
		_renderPreviousGlobalTransform = (flag2 ? transform2D2 : transform2D);
		_renderGlobalTransform = transform2D;
		_renderRootMotionEnabled = flag2;
		_renderGlobalTransformPhysicsFrame = physicsFrame;
		_renderTransformChangedInPhysicsFrame = false;
		_runtimePhysicsAncestorTransformPending = false;
		InvalidateRenderSnapshotCache();
		RequestNodeRedraw(preferDisplayCadence: true, transformOnly: true);
		_knownAncestorTranslationNotificationPending = true;
	}

	private bool TryConsumeKnownAncestorTranslationNotification()
	{
		if (!_knownAncestorTranslationNotificationPending)
		{
			return false;
		}
		_knownAncestorTranslationNotificationPending = false;
		return true;
	}

	internal void NotifySlotOwnedSpriteTransformChangedForRender(AdobeAnimateSlot ownerSlot)
	{
		if (!GodotObject.IsInstanceValid(ownerSlot))
		{
			return;
		}
		for (int i = 0; i < _spriteChildren.Length; i++)
		{
			if (i < _spriteChildOwnerSlots.Length && _spriteChildOwnerSlots[i] == ownerSlot)
			{
				AdobeAnimateSprite adobeAnimateSprite = _spriteChildren[i];
				if (GodotObject.IsInstanceValid(adobeAnimateSprite))
				{
					adobeAnimateSprite.NotifyAncestorTransformChangedForRender();
				}
			}
		}
	}

	private void NotifyLocalRenderTransformChangedForTree(bool changedInPhysicsFrame)
	{
		if (changedInPhysicsFrame)
		{
			_runtimePhysicsAncestorTransformPending = true;
		}
		MarkRenderTransformDirty(changedInPhysicsFrame);
		AdobeAnimateSprite[] spriteChildrenForRender = GetSpriteChildrenForRender();
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildrenForRender)
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.NotifyLocalRenderTransformChangedForTree(changedInPhysicsFrame);
			}
		}
	}

	private Transform2D GetCachedGlobalTransformForRender()
	{
		_knownAncestorTranslationNotificationPending = false;
		if (!_renderGlobalTransformCached || _renderGlobalTransformDirty)
		{
			Transform2D globalTransform = GlobalTransform;
			ulong physicsFrameForRender = AdobeAnimateRenderManager.GetPhysicsFrameForRender();
			bool flag = CanInterpolateRenderRootMotion() && _renderGlobalTransformCached && _renderTransformChangedInPhysicsFrame && physicsFrameForRender != _renderGlobalTransformPhysicsFrame && IsContinuousRenderRootMotion(_renderGlobalTransform, globalTransform);
			_renderPreviousGlobalTransform = (flag ? _renderGlobalTransform : globalTransform);
			_renderGlobalTransform = globalTransform;
			_renderRootMotionEnabled = flag;
			_renderGlobalTransformPhysicsFrame = physicsFrameForRender;
			_renderGlobalTransformCached = true;
			_renderGlobalTransformDirty = false;
			_renderTransformChangedInPhysicsFrame = false;
			_runtimePhysicsAncestorTransformPending = false;
		}
		return _renderGlobalTransform;
	}

	private static bool IsContinuousRenderRootMotion(Transform2D previous, Transform2D current)
	{
		if (Mathf.IsZeroApprox(previous.Determinant()) || Mathf.IsZeroApprox(current.Determinant()) || previous.Origin.DistanceSquaredTo(current.Origin) > 9216f)
		{
			return false;
		}
		if (previous.X.DistanceSquaredTo(current.X) <= 0.25f)
		{
			return previous.Y.DistanceSquaredTo(current.Y) <= 0.25f;
		}
		return false;
	}

	private AdobeAnimateRootMotionState BuildRootMotionStateForRender(Node renderMountParent, Transform2D currentRenderTransform)
	{
		int physicsTicksPerSecondForRender = AdobeAnimateRenderManager.GetPhysicsTicksPerSecondForRender();
		int frameLimitForRender = AdobeAnimateRenderManager.GetFrameLimitForRender();
		if (!CanInterpolateRenderRootMotion() || !HasCurrentPhysicsRootMotion() || (frameLimitForRender > 0 && frameLimitForRender <= physicsTicksPerSecondForRender) || Mathf.IsZeroApprox(currentRenderTransform.Determinant()))
		{
			return default;
		}
		Transform2D transform2D = AdobeAnimateRenderManager.ToRenderMountLocalTransform(renderMountParent, _renderPreviousGlobalTransform);
		return new AdobeAnimateRootMotionState(currentRenderTransform.AffineInverse() * transform2D, enabled: true);
	}

	private void RefreshRenderMountCache()
	{
		Node cachedRenderMountParent = _cachedRenderMountParent;
		Node mountParent;
		Viewport viewport;
		if (_usingRuntimeManager)
		{
			AdobeAnimateRenderManager.ResolveRuntimeRenderMount(this, out mountParent, out _cachedCanvasLayer, out viewport);
		}
		else
		{
			AdobeAnimateRenderManager.ResolveRenderMount(this, out mountParent, out _cachedCanvasLayer);
			viewport = GetViewport();
		}
		_cachedRenderMountParent = ((_forceLocalRender && GodotObject.IsInstanceValid(_renderClipControl) && _renderClipControl.GetViewport() == viewport) ? _renderClipControl : mountParent);
		_cachedViewport = viewport;
		ScheduleNextRenderMountAudit();
		ResetEffectiveRenderModulateAncestorCache();
		if (cachedRenderMountParent != _cachedRenderMountParent)
		{
			InvalidateTreeOrderPathCache();
		}
		InvalidateRenderSnapshotCache();
	}

	private void ScheduleNextRenderMountAudit()
	{
		if (!_usingRuntimeManager)
		{
			_renderMountNextAuditPhysicsFrame = 0uL;
		}
		else
		{
			_renderMountNextAuditPhysicsFrame = AdobeAnimateRenderManager.GetPhysicsFrameForRender() + (ulong)GetStaggeredRescanInterval(60);
		}
	}

	private Node GetCachedRenderMountParent()
	{
		if (!_usingRuntimeManager)
		{
			if (!GodotObject.IsInstanceValid(_cachedRenderMountParent) || _cachedViewport != GetViewport())
			{
				RefreshRenderMountCache();
			}
			return _cachedRenderMountParent;
		}
		if (_cachedRenderMountParent == null || AdobeAnimateRenderManager.GetPhysicsFrameForRender() >= _renderMountNextAuditPhysicsFrame)
		{
			RefreshRenderMountCache();
		}
		return _cachedRenderMountParent;
	}

	private Color GetEffectiveRenderModulate(Node renderMountParent)
	{
		if (!_renderModulateAncestorCacheReady || _renderModulateAncestorRescanCountdown <= 0 || _renderModulateMountParent != renderMountParent)
		{
			RefreshEffectiveRenderModulateAncestorCache(GetParent(), renderMountParent);
		}
		else
		{
			_renderModulateAncestorRescanCountdown--;
		}
		Color result = MultiplyRenderColor(MultiplyRenderColor(GetCachedOwnRenderFrameModulate(), GetCachedOwnRenderFrameSelfModulate()), GetCachedEffectiveRenderAncestorModulate(renderMountParent));
		if (_renderGrayscale && result.A > 0f)
		{
			result.A = 0f - result.A;
		}
		return result;
	}

	private static Color GetCachedRenderFrameModulate(CanvasItem canvasItem)
	{
		if (CachedViewportWorldRectRenderScopeDepth <= 0)
		{
			return canvasItem.Modulate;
		}
		if (canvasItem is AdobeAnimateSprite adobeAnimateSprite)
		{
			return adobeAnimateSprite.GetCachedOwnRenderFrameModulate();
		}
		if (CachedRenderFrameLastModulateItem == canvasItem)
		{
			return CachedRenderFrameLastModulateValue;
		}
		if (!CachedRenderFrameModulates.TryGetValue(canvasItem, out var value))
		{
			value = canvasItem.Modulate;
			CachedRenderFrameModulates[canvasItem] = value;
		}
		CachedRenderFrameLastModulateItem = canvasItem;
		CachedRenderFrameLastModulateValue = value;
		return value;
	}

	private Color GetCachedOwnRenderFrameModulate()
	{
		if (CachedViewportWorldRectRenderScopeDepth <= 0)
		{
			return Modulate;
		}
		long cachedRenderColorTransactionVersion = CachedRenderColorTransactionVersion;
		if (_cachedOwnRenderFrameModulateVersion != cachedRenderColorTransactionVersion)
		{
			EnsureCachedRenderLocalModulates(cachedRenderColorTransactionVersion);
			_cachedOwnRenderFrameModulate = _cachedRenderLocalModulate;
			_cachedOwnRenderFrameModulateVersion = cachedRenderColorTransactionVersion;
		}
		return _cachedOwnRenderFrameModulate;
	}

	private Color GetCachedOwnRenderFrameSelfModulate()
	{
		if (CachedViewportWorldRectRenderScopeDepth <= 0)
		{
			return SelfModulate;
		}
		long cachedRenderColorTransactionVersion = CachedRenderColorTransactionVersion;
		if (_cachedOwnRenderFrameSelfModulateVersion != cachedRenderColorTransactionVersion)
		{
			EnsureCachedRenderLocalModulates(cachedRenderColorTransactionVersion);
			_cachedOwnRenderFrameSelfModulate = _cachedRenderLocalSelfModulate;
			_cachedOwnRenderFrameSelfModulateVersion = cachedRenderColorTransactionVersion;
		}
		return _cachedOwnRenderFrameSelfModulate;
	}

	private void EnsureCachedRenderLocalModulates(long transactionVersion)
	{
		if (_renderLocalModulateAuditTransactionVersion == transactionVersion)
		{
			return;
		}
		_renderLocalModulateAuditTransactionVersion = transactionVersion;
		if (_cachedRenderLocalModulateReady && _cachedRenderLocalSelfModulateReady && _renderLocalModulateRescanCountdown > 0)
		{
			_renderLocalModulateRescanCountdown--;
			return;
		}
		Color modulate = base.Modulate;
		Color selfModulate = base.SelfModulate;
		bool num = (_cachedRenderLocalModulateReady && _cachedRenderLocalModulate != modulate) || (_cachedRenderLocalSelfModulateReady && _cachedRenderLocalSelfModulate != selfModulate);
		_cachedRenderLocalModulate = modulate;
		_cachedRenderLocalSelfModulate = selfModulate;
		_cachedRenderLocalModulateReady = true;
		_cachedRenderLocalSelfModulateReady = true;
		ScheduleNextRenderLocalModulateAudit();
		if (num)
		{
			_effectOnceBatchVisualRevision++;
			MarkEffectOnceBatchEligibilityChanged();
		}
	}

	private void ScheduleNextRenderLocalModulateAudit()
	{
		_renderLocalModulateRescanCountdown = GetStaggeredRescanInterval(60);
	}

	private void RefreshEffectiveRenderModulateAncestorCache(Node directParent, Node renderMountParent)
	{
		_renderModulateAncestors.Clear();
		Node node = directParent;
		while (GodotObject.IsInstanceValid(node) && node != renderMountParent)
		{
			if (node is CanvasItem item)
			{
				_renderModulateAncestors.Add(item);
			}
			node = node.GetParent();
		}
		_renderModulateMountParent = renderMountParent;
		_renderModulateAncestorCacheReady = true;
		_renderModulateAncestorRescanCountdown = GetStaggeredRescanInterval(60);
	}

	private void ResetEffectiveRenderModulateAncestorCache()
	{
		_renderModulateAncestors.Clear();
		_renderModulateMountParent = null;
		_renderModulateAncestorCacheReady = false;
		_renderModulateAncestorRescanCountdown = 0;
	}

	private static Color MultiplyRenderColor(in Color left, in Color right)
	{
		return new Color(left.R * right.R, left.G * right.G, left.B * right.B, left.A * right.A);
	}

	private bool IsInSubViewportRenderTarget()
	{
		return GetViewport() is SubViewport;
	}

	private bool ShouldUseGlobalRuntimeManager()
	{
		if (AdobeAnimateRuntimeManager.UseRuntimeManager && !CachedEditorHint && !IsInSubViewportRenderTarget())
		{
			return !_forceLocalRender;
		}
		return false;
	}

	internal void CommitRuntimeNativeCanvasTakeover(long frameVersion)
	{
		_runtimeNativeCanvasPublishedFrameVersion = frameVersion;
		ReleaseRuntimeManagerFirstFrameHandoffTree();
		if (!_runtimeNativeCanvasSuppressed && _usingRuntimeManager && _runtimeInsideTree && !_forceLocalRender && CanSuppressRuntimeNativeCanvasTree())
		{
			uint visibilityLayer = VisibilityLayer;
			if (visibilityLayer != 0)
			{
				_runtimeNativeCanvasVisibilityLayer = visibilityLayer;
				VisibilityLayer = 0u;
				_runtimeNativeCanvasSuppressed = true;
			}
		}
	}

	internal bool RecoverRuntimeNativeCanvasAfterMissedTransaction(long frameVersion)
	{
		if (!_runtimeNativeCanvasSuppressed || _runtimeNativeCanvasPublishedFrameVersion == frameVersion)
		{
			return false;
		}
		RestoreRuntimeNativeCanvasLayer();
		RequestRuntimeRenderSubmissionRetry();
		return true;
	}

	internal void RestoreRuntimeNativeCanvasLayer()
	{
		if (_runtimeNativeCanvasSuppressed)
		{
			uint runtimeNativeCanvasVisibilityLayer = _runtimeNativeCanvasVisibilityLayer;
			_runtimeNativeCanvasSuppressed = false;
			VisibilityLayer = runtimeNativeCanvasVisibilityLayer;
		}
	}

	private void InvalidateRuntimeNativeCanvasSuppressionEligibility()
	{
		_runtimeNativeCanvasSuppressionEligibilityKnown = false;
		_runtimeNativeCanvasSuppressionEligible = false;
		RestoreRuntimeNativeCanvasLayer();
	}

	private bool CanSuppressRuntimeNativeCanvasTree()
	{
		if (_runtimeNativeCanvasSuppressionEligibilityKnown)
		{
			return _runtimeNativeCanvasSuppressionEligible;
		}
		_runtimeNativeCanvasSuppressionEligible = IsRuntimeNativeCanvasBranchCrowdManaged(this);
		_runtimeNativeCanvasSuppressionEligibilityKnown = true;
		return _runtimeNativeCanvasSuppressionEligible;
	}

	private static bool IsRuntimeNativeCanvasBranchCrowdManaged(Node parent)
	{
		int childCount = parent.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			Node child = parent.GetChild(i);
			if (child is CanvasItem canvasItem && (!(canvasItem is Node2D node2D) || !node2D.IsSetAsTopLevel()))
			{
				bool num = canvasItem.GetType() == typeof(Node2D) || canvasItem is AdobeAnimateSlot;
				bool flag = canvasItem is Sprite2D && AdobeAnimateManagedSprite2D.IsCrowdManaged(canvasItem);
				bool flag2 = canvasItem is AdobeAnimateSprite adobeAnimateSprite && adobeAnimateSprite._usingRuntimeManager;
				if (!num && !flag && !flag2)
				{
					return false;
				}
				if (!IsRuntimeNativeCanvasBranchCrowdManaged(child))
				{
					return false;
				}
			}
		}
		return true;
	}

	private AdobeAnimateRuntimeDefinition GetRuntimeDefinition()
	{
		if (_runtimeDefinition == null || _runtimeDefinition.Source != _flashAnimeData || _runtimeDefinition.AtlasProfile != _atlasProfileOverride)
		{
			AdobeAnimateRuntimeDefinition adobeAnimateRuntimeDefinition = ((_atlasProfileOverride == null) ? AdobeAnimateDefinitionCache.GetOrBuild(_flashAnimeData) : AdobeAnimateDefinitionCache.GetOrBuild(_flashAnimeData, _atlasProfileOverride));
			if (_runtimeDefinition != adobeAnimateRuntimeDefinition)
			{
				ResetManagedPoseTrackCache();
			}
			_runtimeDefinition = adobeAnimateRuntimeDefinition;
			meshTexture = _runtimeDefinition?.BaseAtlasRid ?? default(Rid);
		}
		return _runtimeDefinition;
	}

	internal bool TryBuildOrderedFallbackSnapshotFromCrowdState(AdobeAnimateCrowdRenderState state, out AdobeAnimateRenderSnapshot snapshot, out bool culled)
	{
		snapshot = default;
		culled = false;
		if (state == null || _effectOnceGpuSuppressed || _flashAnimeData == null || !_isVisibleInTree || !HasRenderableAtlas(state.Definition))
		{
			return false;
		}
		if (_usingRuntimeManager && !IsOrderedFallbackRenderBoundsVisibleFromCrowdCache(state.Definition, state.RenderMountParent))
		{
			culled = true;
			return false;
		}
		snapshot = new AdobeAnimateRenderSnapshot(this, _flashAnimeData, state.Definition, _clip, clipRange, GetRenderFrameFloat(), state.GlobalTransform, state.Offset, state.Modulate, _layerVisible, _cachedAllLayersVisible, _cachedCanUseLayerMask, _cachedLayerMask, _cachedLayerVisibleValueCount, _cachedLayerVisibleValues, mediaReplaceAtlas, mediaReplaceRect, _mediaReplaceUse, _cachedHasMediaReplace, _cachedMediaReplaceLimit, _cachedMediaReplaceUseMask, _cachedMediaReplaceUseMaskOverflow, mediaReplaceAtlasPages, _cachedMediaReplaceAtlasPageCount, _cachedMediaReplaceUseValues, _cachedMediaReplaceRects, _cachedMediaReplaceAtlasPageValues, mediaReplaceAtlasArray, mediaReplaceAtlasArraySize, mediaReplaceAtlasUsesTextureArray, state.VerticalClip, state.RenderMountParent, _cachedCanvasLayer, state.EffectiveZIndex, System.Array.Empty<int>(), NeedsDrawItemSortForRender(), GetClipBlendStateForRender());
		_needsRenderSubmission = false;
		return true;
	}

	private bool IsOrderedFallbackRenderBoundsVisibleFromCrowdCache(AdobeAnimateRuntimeDefinition definition, Node renderMountParent)
	{
		Transform2D cachedGlobalTransformForRender = GetCachedGlobalTransformForRender();
		if (_runtimeRenderBoundsCached && _cachedRuntimeRenderBoundsCullingEnabled == _runtimeViewportCullingEnabled && _cachedRuntimeRenderBoundsDefinition == definition && _cachedRuntimeRenderBoundsTransform == cachedGlobalTransformForRender && _cachedRuntimeRenderBoundsOffset == offset && _cachedRuntimeRenderBoundsMountParent == renderMountParent)
		{
			return _cachedRuntimeRenderBoundsStrictVisible;
		}
		return IsRenderBoundsVisible(definition, cachedGlobalTransformForRender, renderMountParent);
	}

	internal bool TryBuildRenderSnapshot(out AdobeAnimateRenderSnapshot snapshot, bool allowUnchanged = true)
	{
		bool culled;
		return TryBuildRenderSnapshot(out snapshot, out culled, allowUnchanged);
	}

	internal bool TryBuildRenderSnapshot(out AdobeAnimateRenderSnapshot snapshot, out bool culled, bool allowUnchanged = true)
	{
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		snapshot = default;
		culled = false;
		if (_effectOnceGpuSuppressed || _flashAnimeData == null || !_isVisibleInTree)
		{
			TowerDefensePerfProfiler.End("adobeAnimate.render.snapshotBuild.skip", startTicks);
			return false;
		}
		if (needMediaReplaceUpdate)
		{
			UpdateMediaReplaceData();
		}
		RefreshRenderStaticStateCache();
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (!HasRenderableAtlas(runtimeDefinition))
		{
			TowerDefensePerfProfiler.End("adobeAnimate.render.snapshotBuild.noDefinition", startTicks);
			return false;
		}
		Node cachedRenderMountParent = GetCachedRenderMountParent();
		Transform2D cachedGlobalTransformForRender = GetCachedGlobalTransformForRender();
		Transform2D globalTransform = AdobeAnimateRenderManager.ToRenderMountLocalTransform(cachedRenderMountParent, cachedGlobalTransformForRender);
		if (_usingRuntimeManager && !IsRenderBoundsVisible(runtimeDefinition, cachedGlobalTransformForRender, cachedRenderMountParent))
		{
			culled = true;
			TowerDefensePerfProfiler.End("adobeAnimate.render.snapshotBuild.culled", startTicks);
			return false;
		}
		bool needsDrawItemSort = NeedsDrawItemSortForRender();
		ResolveRenderPresentationForRender(cachedRenderMountParent, out var modulate, out var effectiveZIndex);
		int[] treeOrderPath = ((_usingRuntimeManager && !CachedEditorHint) ? System.Array.Empty<int>() : GetEffectiveTreeOrderPathForRender());
		snapshot = new AdobeAnimateRenderSnapshot(this, _flashAnimeData, runtimeDefinition, _clip, clipRange, GetRenderFrameFloat(), globalTransform, offset, modulate, _layerVisible, _cachedAllLayersVisible, _cachedCanUseLayerMask, _cachedLayerMask, _cachedLayerVisibleValueCount, _cachedLayerVisibleValues, mediaReplaceAtlas, mediaReplaceRect, _mediaReplaceUse, _cachedHasMediaReplace, _cachedMediaReplaceLimit, _cachedMediaReplaceUseMask, _cachedMediaReplaceUseMaskOverflow, mediaReplaceAtlasPages, _cachedMediaReplaceAtlasPageCount, _cachedMediaReplaceUseValues, _cachedMediaReplaceRects, _cachedMediaReplaceAtlasPageValues, mediaReplaceAtlasArray, mediaReplaceAtlasArraySize, mediaReplaceAtlasUsesTextureArray, _verticalClip, cachedRenderMountParent, _cachedCanvasLayer, effectiveZIndex, treeOrderPath, needsDrawItemSort, GetClipBlendStateForRender());
		_needsRenderSubmission = false;
		TowerDefensePerfProfiler.End("adobeAnimate.render.snapshotBuild", startTicks, 1);
		return true;
	}

	internal bool NeedsDrawItemSortForRender()
	{
		bool flag = RequiresInternalDrawItemSortForRender();
		if (_usingRuntimeManager && !CachedEditorHint && !flag)
		{
			return false;
		}
		return flag;
	}

	private bool RequiresInternalDrawItemSortForRender()
	{
		if (!GetNeedsDrawOrderSortBandsForRender() && GetSpriteChildrenForRender().Length == 0)
		{
			return HasManagedSlotSpritesForRender();
		}
		return true;
	}

	private bool IsRenderBoundsVisible(AdobeAnimateRuntimeDefinition definition, Transform2D globalTransform, Node renderMountParent, bool useDefinitionBounds = false, float viewportPadding = 0f)
	{
		if (!_runtimeViewportCullingEnabled || definition == null)
		{
			return true;
		}
		Viewport viewport = ResolveRenderViewport(renderMountParent);
		if (!GodotObject.IsInstanceValid(viewport))
		{
			return true;
		}
		Rect2 rect = GetCachedViewportWorldRect(viewport);
		if (rect.Size.X <= 0f || rect.Size.Y <= 0f)
		{
			return true;
		}
		if (viewportPadding > 0f)
		{
			rect = rect.Grow(viewportPadding);
		}
		Rect2 rect2 = ((useDefinitionBounds && IsValidBounds(definition.LocalBounds)) ? definition.LocalBounds : GetRenderLocalBounds(definition));
		rect2.Position += offset;
		Rect2 b = TransformRect(globalTransform, rect2);
		return rect.Intersects(b, includeBorders: true);
	}

	private Viewport ResolveRenderViewport(Node renderMountParent)
	{
		if (GodotObject.IsInstanceValid(renderMountParent) && renderMountParent is Viewport result)
		{
			return result;
		}
		if (GodotObject.IsInstanceValid(_cachedViewport))
		{
			return _cachedViewport;
		}
		_cachedViewport = GetViewport();
		return _cachedViewport;
	}

	private bool IsRuntimeCrowdRenderBoundsVisible(AdobeAnimateRuntimeDefinition definition, Transform2D globalTransform, Node renderMountParent)
	{
		if (definition == null)
		{
			return true;
		}
		Viewport viewport = ResolveRenderViewport(renderMountParent);
		Rect2 rect = (GodotObject.IsInstanceValid(viewport) ? GetCachedViewportWorldRect(viewport) : default(Rect2));
		bool flag = _runtimeRenderBoundsCached && _cachedRuntimeRenderBoundsCullingEnabled == _runtimeViewportCullingEnabled && _cachedRuntimeRenderBoundsDefinition == definition && _cachedRuntimeRenderBoundsOffset == offset && _cachedRuntimeRenderBoundsMountParent == renderMountParent && _cachedRuntimeRenderBoundsViewport == viewport && _cachedRuntimeRenderBoundsViewportWorldRect == rect;
		if (flag && _runtimeRenderBoundsRescanCountdown > 0 && _cachedRuntimeRenderBoundsTransform == globalTransform)
		{
			_runtimeRenderBoundsRescanCountdown--;
			return _cachedRuntimeRenderBoundsVisible;
		}
		bool cachedRuntimeRenderBoundsHasRelativeRect = false;
		Rect2 rect2 = default;
		bool flag2 = true;
		bool cachedRuntimeRenderBoundsStrictVisible = true;
		if (_runtimeViewportCullingEnabled && GodotObject.IsInstanceValid(viewport) && rect.Size.X > 0f && rect.Size.Y > 0f)
		{
			Rect2 rect3;
			if (flag && _cachedRuntimeRenderBoundsHasRelativeRect && _cachedRuntimeRenderBoundsTransform.X == globalTransform.X && _cachedRuntimeRenderBoundsTransform.Y == globalTransform.Y)
			{
				rect2 = _cachedRuntimeRenderBoundsRelativeRect;
				rect3 = rect2;
				rect3.Position += globalTransform.Origin;
			}
			else
			{
				Rect2 rect4 = (IsValidBounds(definition.LocalBounds) ? definition.LocalBounds : GetRenderLocalBounds(definition));
				rect4.Position += offset;
				rect3 = TransformRect(globalTransform, rect4);
				rect2 = rect3;
				rect2.Position -= globalTransform.Origin;
			}
			cachedRuntimeRenderBoundsHasRelativeRect = true;
			cachedRuntimeRenderBoundsStrictVisible = rect.Intersects(rect3, includeBorders: true);
			flag2 = rect.Grow(256f).Intersects(rect3, includeBorders: true);
		}
		_runtimeRenderBoundsCached = true;
		_cachedRuntimeRenderBoundsVisible = flag2;
		_cachedRuntimeRenderBoundsStrictVisible = cachedRuntimeRenderBoundsStrictVisible;
		_cachedRuntimeRenderBoundsCullingEnabled = _runtimeViewportCullingEnabled;
		_cachedRuntimeRenderBoundsDefinition = definition;
		_cachedRuntimeRenderBoundsTransform = globalTransform;
		_cachedRuntimeRenderBoundsOffset = offset;
		_cachedRuntimeRenderBoundsMountParent = renderMountParent;
		_cachedRuntimeRenderBoundsViewport = viewport;
		_cachedRuntimeRenderBoundsViewportWorldRect = rect;
		_cachedRuntimeRenderBoundsRelativeRect = rect2;
		_cachedRuntimeRenderBoundsHasRelativeRect = cachedRuntimeRenderBoundsHasRelativeRect;
		_runtimeRenderBoundsRescanCountdown = GetStaggeredRescanInterval(12);
		return flag2;
	}

	private Rect2 GetRenderLocalBounds(AdobeAnimateRuntimeDefinition definition)
	{
		if (definition == null)
		{
			return new Rect2(new Vector2(-128f, -128f), new Vector2(256f, 256f));
		}
		Rect2 localBounds = definition.LocalBounds;
		Rect2[] frameLocalBounds = definition.FrameLocalBounds;
		if (frameLocalBounds == null || frameLocalBounds.Length == 0)
		{
			return localBounds;
		}
		int num = Mathf.Clamp(frameIndex, 0, frameLocalBounds.Length - 1);
		if (TryGetLoopTerminalAliasFrame(out var terminalFrame, out var aliasFrame) && num >= terminalFrame)
		{
			num = Mathf.Clamp(aliasFrame, 0, frameLocalBounds.Length - 1);
		}
		Rect2 rect = (IsValidBounds(frameLocalBounds[num]) ? frameLocalBounds[num] : localBounds);
		int num2 = Mathf.Clamp(num + ((!playBack) ? 1 : (-1)), 0, frameLocalBounds.Length - 1);
		if (num2 != num && IsValidBounds(frameLocalBounds[num2]))
		{
			rect = MergeBounds(rect, frameLocalBounds[num2]);
		}
		if (IsClipBlendActiveForRender())
		{
			int num3 = Mathf.Clamp(Mathf.FloorToInt(_blendFromFrameFloat), 0, frameLocalBounds.Length - 1);
			if (IsValidBounds(frameLocalBounds[num3]))
			{
				rect = MergeBounds(rect, frameLocalBounds[num3]);
			}
			int num4 = Mathf.Clamp(num3 + 1, 0, frameLocalBounds.Length - 1);
			if (num4 != num3 && IsValidBounds(frameLocalBounds[num4]))
			{
				rect = MergeBounds(rect, frameLocalBounds[num4]);
			}
		}
		if (!IsValidBounds(rect))
		{
			return localBounds;
		}
		return rect;
	}

	internal static void BeginViewportWorldRectRenderFrame(long frameVersion)
	{
		if (CachedViewportWorldRectRenderScopeDepth == 0)
		{
			CachedRenderColorTransactionVersion++;
			CachedRenderFrameModulates.Clear();
			ResetCachedRenderFrameEffectiveAncestorModulates();
			CachedRenderFrameLastModulateItem = null;
			CachedRenderFrameLastModulateValue = default;
		}
		CachedViewportWorldRectRenderScopeDepth++;
		ResetCachedViewportWorldRectsForFrame(frameVersion);
	}

	internal static void EndViewportWorldRectRenderFrame()
	{
		if (CachedViewportWorldRectRenderScopeDepth > 0)
		{
			CachedViewportWorldRectRenderScopeDepth--;
		}
		if (CachedViewportWorldRectRenderScopeDepth == 0)
		{
			CachedRenderFrameModulates.Clear();
			ResetCachedRenderFrameEffectiveAncestorModulates();
			CachedRenderFrameLastModulateItem = null;
			CachedRenderFrameLastModulateValue = default;
		}
	}

	private static void ResetCachedViewportWorldRectsForFrame(long frame)
	{
		if (CachedViewportWorldRectsFrame != frame)
		{
			CachedViewportWorldRectsFrame = frame;
			CachedViewportWorldRects.Clear();
			CachedViewportWorldRectLastViewport = null;
		}
	}

	private static Rect2 GetCachedViewportWorldRect(Viewport viewport)
	{
		if (CachedViewportWorldRectRenderScopeDepth <= 0)
		{
			ResetCachedViewportWorldRectsForFrame((long)Engine.GetProcessFrames());
		}
		if (CachedViewportWorldRectLastViewport == viewport)
		{
			return CachedViewportWorldRectLastValue;
		}
		ulong instanceId = viewport.GetInstanceId();
		if (CachedViewportWorldRects.TryGetValue(instanceId, out var value))
		{
			CachedViewportWorldRectLastViewport = viewport;
			CachedViewportWorldRectLastValue = value;
			return value;
		}
		Rect2 visibleRect = viewport.GetVisibleRect();
		if (visibleRect.Size.X <= 0f || visibleRect.Size.Y <= 0f)
		{
			CachedViewportWorldRects[instanceId] = visibleRect;
			CachedViewportWorldRectLastViewport = viewport;
			CachedViewportWorldRectLastValue = visibleRect;
			return visibleRect;
		}
		Transform2D transform2D = viewport.GetCanvasTransform().AffineInverse();
		Vector2 position = transform2D * visibleRect.Position;
		Vector2 to = transform2D * (visibleRect.Position + new Vector2(visibleRect.Size.X, 0f));
		Vector2 to2 = transform2D * (visibleRect.Position + visibleRect.Size);
		Vector2 to3 = transform2D * (visibleRect.Position + new Vector2(0f, visibleRect.Size.Y));
		Rect2 rect = new Rect2(position, Vector2.Zero).Expand(to).Expand(to2).Expand(to3)
			.Abs();
		CachedViewportWorldRects[instanceId] = rect;
		CachedViewportWorldRectLastViewport = viewport;
		CachedViewportWorldRectLastValue = rect;
		return rect;
	}

	private static bool IsValidBounds(Rect2 bounds)
	{
		if (bounds.Size.X > 0f)
		{
			return bounds.Size.Y > 0f;
		}
		return false;
	}

	private static Rect2 MergeBounds(Rect2 left, Rect2 right)
	{
		if (!IsValidBounds(left))
		{
			return right;
		}
		if (!IsValidBounds(right))
		{
			return left;
		}
		Vector2 position = right.Position;
		Vector2 to = right.Position + right.Size;
		return left.Expand(position).Expand(new Vector2(to.X, position.Y)).Expand(to)
			.Expand(new Vector2(position.X, to.Y))
			.Abs();
	}

	private static Rect2 TransformRect(Transform2D transform, Rect2 rect)
	{
		Vector2 position = transform * rect.Position;
		Vector2 to = transform * (rect.Position + new Vector2(rect.Size.X, 0f));
		Vector2 to2 = transform * (rect.Position + rect.Size);
		Vector2 to3 = transform * (rect.Position + new Vector2(0f, rect.Size.Y));
		return new Rect2(position, Vector2.Zero).Expand(to).Expand(to2).Expand(to3)
			.Abs();
	}

	private float GetRenderFrameFloat()
	{
		if (TryGetLoopTerminalAliasFrame(out var terminalFrame, out var aliasFrame) && frameIndex >= terminalFrame)
		{
			return aliasFrame;
		}
		return (float)frameIndex + (float)elapsedTimer;
	}

	private bool IsClipBlendActiveForRender()
	{
		if (blend)
		{
			return blendTime > 0.0;
		}
		return false;
	}

	private AdobeAnimateClipBlendState GetClipBlendStateForRender()
	{
		if (!IsClipBlendActiveForRender())
		{
			return default;
		}
		return new AdobeAnimateClipBlendState(enabled: true, _blendFromFrameFloat, (float)(blendTimer / blendTime));
	}

	private int GetPlaybackClipEndExclusive()
	{
		if (_playbackClipEndCacheHasValue && _playbackClipEndCacheDefinition == _runtimeDefinition && _playbackClipEndCacheRange == clipRange && _playbackClipEndCacheLoop == loop && _playbackClipEndCachePlayBack == playBack)
		{
			return _playbackClipEndCacheValue;
		}
		int num = (TryGetLoopTerminalAliasFrame(out var terminalFrame, out var _) ? Math.Max(clipRange.X + 1, terminalFrame) : clipRange.Y);
		_playbackClipEndCacheDefinition = _runtimeDefinition;
		_playbackClipEndCacheRange = clipRange;
		_playbackClipEndCacheLoop = loop;
		_playbackClipEndCachePlayBack = playBack;
		_playbackClipEndCacheValue = num;
		_playbackClipEndCacheHasValue = true;
		return num;
	}

	private bool TryGetLoopTerminalAliasFrame(out int terminalFrame, out int aliasFrame)
	{
		terminalFrame = clipRange.Y - 1;
		aliasFrame = clipRange.X;
		if (loop && deduplicateLoopTerminalFrame && !playBack && clipRange.Y - clipRange.X > 1)
		{
			return DoesLoopTerminalFrameAliasFirstFrame(terminalFrame, aliasFrame);
		}
		return false;
	}

	private bool DoesLoopTerminalFrameAliasFirstFrame(int terminalFrame, int aliasFrame)
	{
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (runtimeDefinition == null)
		{
			return false;
		}
		if (_loopTerminalAliasCacheHasValue && _loopTerminalAliasCacheDefinition == runtimeDefinition && _loopTerminalAliasCacheRange == clipRange)
		{
			return _loopTerminalAliasCacheValue;
		}
		bool flag = FramesHaveMatchingPose(runtimeDefinition, terminalFrame, aliasFrame);
		_loopTerminalAliasCacheDefinition = runtimeDefinition;
		_loopTerminalAliasCacheRange = clipRange;
		_loopTerminalAliasCacheHasValue = true;
		_loopTerminalAliasCacheValue = flag;
		return flag;
	}

	private static bool FramesHaveMatchingPose(AdobeAnimateRuntimeDefinition definition, int leftFrame, int rightFrame)
	{
		if (definition == null || definition.Frames == null)
		{
			return false;
		}
		if (leftFrame < 0 || rightFrame < 0 || leftFrame >= definition.Frames.Length || rightFrame >= definition.Frames.Length)
		{
			return false;
		}
		if (definition.FramePoseSignatures != null && leftFrame < definition.FramePoseSignatures.Length && rightFrame < definition.FramePoseSignatures.Length)
		{
			return definition.FramePoseSignatures[leftFrame] == definition.FramePoseSignatures[rightFrame];
		}
		if (!AdobeAnimateDefinitionCache.EnsureCpuPoseData(definition))
		{
			return false;
		}
		PackedFrame packedFrame = definition.Frames[leftFrame];
		PackedFrame packedFrame2 = definition.Frames[rightFrame];
		if (packedFrame.Count != packedFrame2.Count || packedFrame.Offset < 0 || packedFrame2.Offset < 0)
		{
			return false;
		}
		if (packedFrame.Offset + packedFrame.Count > definition.Slices.Length || packedFrame2.Offset + packedFrame2.Count > definition.Slices.Length)
		{
			return false;
		}
		for (int i = 0; i < packedFrame.Count; i++)
		{
			if (!PackedSlicePoseMatches(definition.Slices[packedFrame.Offset + i], definition.Slices[packedFrame2.Offset + i]))
			{
				return false;
			}
		}
		return true;
	}

	private static bool PackedSlicePoseMatches(PackedSlicePose left, PackedSlicePose right)
	{
		if (left.SliceKey == right.SliceKey && left.MediaId == right.MediaId && left.LayerId == right.LayerId && left.DrawOrder == right.DrawOrder && left.Flags == right.Flags && Mathf.IsEqualApprox(left.Xx, right.Xx) && Mathf.IsEqualApprox(left.Xy, right.Xy) && Mathf.IsEqualApprox(left.Yx, right.Yx) && Mathf.IsEqualApprox(left.Yy, right.Yy) && Mathf.IsEqualApprox(left.Ox, right.Ox) && Mathf.IsEqualApprox(left.Oy, right.Oy))
		{
			return Mathf.IsEqualApprox(left.Alpha, right.Alpha);
		}
		return false;
	}

	private void CacheRenderSnapshotState(float frameFloat, Transform2D globalTransform, Color modulate, Node renderMountParent, int effectiveZIndex, long sortBand, bool useDrawOrderSortBands)
	{
		_cachedClip = _clip;
		_cachedClipRange = clipRange;
		_cachedFrameFloat = frameFloat;
		_cachedGlobalTransform = globalTransform;
		_cachedModulate = modulate;
		_cachedOffset = offset;
		_cachedMediaReplaceAtlas = mediaReplaceAtlas;
		_cachedMediaReplaceAtlasArray = mediaReplaceAtlasArray;
		ref Rid cachedMediaReplaceAtlasRid = ref _cachedMediaReplaceAtlasRid;
		Rid rid;
		if (mediaReplaceAtlasUsesTextureArray && GodotObject.IsInstanceValid(mediaReplaceAtlasArray))
		{
			rid = mediaReplaceAtlasArray.GetRid();
		}
		else
		{
			rid = (GodotObject.IsInstanceValid(mediaReplaceAtlas) ? mediaReplaceAtlas.GetRid() : default(Rid));
		}
		cachedMediaReplaceAtlasRid = rid;
		_cachedMediaReplaceAtlasArrayRid = (GodotObject.IsInstanceValid(mediaReplaceAtlasArray) ? mediaReplaceAtlasArray.GetRid() : default(Rid));
		_cachedMediaReplaceAtlasShared = mediaReplaceAtlasShared;
		_cachedMediaReplaceAtlasUsesTextureArray = mediaReplaceAtlasUsesTextureArray;
		_cachedRenderMountParentSubmit = renderMountParent;
		_cachedCanvasLayerSubmit = _cachedCanvasLayer;
		_cachedZIndex = ZIndex;
		_cachedEffectiveZIndex = effectiveZIndex;
		_cachedSortBand = sortBand;
		_cachedUseDrawOrderSortBands = useDrawOrderSortBands;
		_cachedRefreshEveryFrame = refreshEveryFrame;
		_cachedVerticalClip = _verticalClip;
		_needsRenderSubmission = false;
		_renderStateCached = true;
	}

	private bool RefreshRenderStaticStateCache(bool force = false)
	{
		ulong physicsFrameForRender = AdobeAnimateRenderManager.GetPhysicsFrameForRender();
		bool flag = _cachedLayerStateVersion != _layerStateVersion || _cachedMediaReplaceStateVersion != _mediaReplaceStateVersion;
		bool flag2 = ShouldAuditRenderStaticArrays() && physicsFrameForRender >= _renderStaticStateNextAuditPhysicsFrame;
		if (!((force || !_renderStaticStateCached) | flag | flag2))
		{
			return false;
		}
		if (((!force && _renderStaticStateCached && !flag) & flag2) && RenderStaticArrayAuditSnapshotsMatch())
		{
			ScheduleNextRenderStaticStateAudit(physicsFrameForRender);
			return false;
		}
		int cachedLayerVisibleCount = _cachedLayerVisibleCount;
		ulong cachedLayerMask = _cachedLayerMask;
		bool cachedAllLayersVisible = _cachedAllLayersVisible;
		bool cachedCanUseLayerMask = _cachedCanUseLayerMask;
		ulong cachedLayerVisibleSignature = _cachedLayerVisibleSignature;
		int cachedMediaReplaceLimit = _cachedMediaReplaceLimit;
		bool cachedHasMediaReplace = _cachedHasMediaReplace;
		bool cachedAnyRequestedMediaReplace = _cachedAnyRequestedMediaReplace;
		ulong cachedMediaReplaceSignature = _cachedMediaReplaceSignature;
		float cachedMediaReplaceBoundsGrow = _cachedMediaReplaceBoundsGrow;
		Rid cachedMediaReplaceAtlasRid = _cachedMediaReplaceAtlasRid;
		Array<bool> array = _layerVisible;
		int num = (_cachedLayerVisibleValueCount = array?.Count ?? 0);
		if (_cachedLayerVisibleValues.Length != num)
		{
			System.Array.Resize(ref _cachedLayerVisibleValues, num);
		}
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		_cachedLayerVisibleCount = GetRuntimeLayerVisibleCount(num, runtimeDefinition);
		_cachedLayerVisibleSignature = ComputeLayerVisibilitySignature(array, num);
		RefreshLayerActiveInCurrentClipCache(_cachedLayerVisibleCount, runtimeDefinition);
		_cachedLayerStateVersion = _layerStateVersion;
		_cachedAllLayersVisible = true;
		_cachedConfiguredLayersAllVisible = true;
		_cachedCanUseLayerMask = true;
		_cachedLayerMask = 0uL;
		if (_cachedLayerVisibleCount > 0)
		{
			for (int i = 0; i < _cachedLayerVisibleCount; i++)
			{
				bool flag3 = i >= num || array[i];
				if (i < num)
				{
					_cachedLayerVisibleValues[i] = flag3;
				}
				_cachedConfiguredLayersAllVisible &= flag3;
				_cachedAllLayersVisible &= flag3;
				if (i >= 64)
				{
					_cachedCanUseLayerMask = false;
				}
				else if (flag3)
				{
					_cachedLayerMask |= (ulong)(1L << i);
				}
			}
		}
		Array<bool> array2 = _mediaReplaceUse;
		int num2 = array2?.Count ?? 0;
		_cachedAnyRequestedMediaReplace = false;
		_cachedMediaReplaceUseMask = 0uL;
		_cachedMediaReplaceUseMaskOverflow = false;
		for (int j = 0; j < num2; j++)
		{
			if (array2[j])
			{
				_cachedAnyRequestedMediaReplace = true;
				if (j < 64)
				{
					_cachedMediaReplaceUseMask |= (ulong)(1L << j);
				}
				else
				{
					_cachedMediaReplaceUseMaskOverflow = true;
				}
			}
		}
		Array<Texture2D> array3 = _mediaReplace;
		int textureCount = array3?.Count ?? 0;
		Array<Rect2> array4 = mediaReplaceRect;
		int num3 = array4?.Count ?? 0;
		Array<int> array5 = mediaReplaceAtlasPages;
		int num4 = (_cachedMediaReplaceAtlasPageCount = array5?.Count ?? 0);
		if (_cachedMediaReplaceUseValues.Length != num2)
		{
			System.Array.Resize(ref _cachedMediaReplaceUseValues, num2);
		}
		if (_cachedMediaReplaceRects.Length != num3)
		{
			System.Array.Resize(ref _cachedMediaReplaceRects, num3);
		}
		if (_cachedMediaReplaceAtlasPageValues.Length != num4)
		{
			System.Array.Resize(ref _cachedMediaReplaceAtlasPageValues, num4);
		}
		for (int k = 0; k < num2; k++)
		{
			_cachedMediaReplaceUseValues[k] = array2[k];
		}
		for (int l = 0; l < num3; l++)
		{
			_cachedMediaReplaceRects[l] = array4[l];
		}
		for (int m = 0; m < num4; m++)
		{
			_cachedMediaReplaceAtlasPageValues[m] = array5[m];
		}
		if (HasValidMediaReplaceAtlas())
		{
			_cachedMediaReplaceAtlasRid = GetMediaReplaceAtlasRid();
			_cachedMediaReplaceAtlasArrayRid = (GodotObject.IsInstanceValid(mediaReplaceAtlasArray) ? mediaReplaceAtlasArray.GetRid() : default(Rid));
			_cachedMediaReplaceLimit = Math.Min(num2, num3);
			_cachedHasMediaReplace = HasActiveMediaReplace(array2, num2, _cachedMediaReplaceLimit);
			_cachedMediaReplaceSignature = BuildMediaReplaceSignature(array3, textureCount, array2, num2, array4, num3, _cachedMediaReplaceLimit, array5, num4, mediaReplaceAtlasUsesTextureArray);
			_cachedMediaReplaceBoundsGrow = ComputeMediaReplaceBoundsGrow(array2, num2, array4, num3, _cachedMediaReplaceLimit);
		}
		else
		{
			_cachedMediaReplaceAtlasRid = default;
			_cachedMediaReplaceAtlasArrayRid = default;
			_cachedMediaReplaceLimit = 0;
			_cachedHasMediaReplace = false;
			_cachedMediaReplaceSignature = 0uL;
			_cachedMediaReplaceBoundsGrow = 0f;
		}
		_cachedMediaReplaceStateVersion = _mediaReplaceStateVersion;
		_renderStaticStateCached = true;
		CaptureRenderStaticArrayAuditSnapshots();
		ScheduleNextRenderStaticStateAudit(physicsFrameForRender);
		if (!force && cachedLayerVisibleCount == _cachedLayerVisibleCount && cachedLayerMask == _cachedLayerMask && cachedAllLayersVisible == _cachedAllLayersVisible && cachedCanUseLayerMask == _cachedCanUseLayerMask && cachedLayerVisibleSignature == _cachedLayerVisibleSignature && cachedMediaReplaceLimit == _cachedMediaReplaceLimit && cachedHasMediaReplace == _cachedHasMediaReplace && cachedAnyRequestedMediaReplace == _cachedAnyRequestedMediaReplace && cachedMediaReplaceSignature == _cachedMediaReplaceSignature && !(cachedMediaReplaceAtlasRid != _cachedMediaReplaceAtlasRid))
		{
			return !Mathf.IsEqualApprox(cachedMediaReplaceBoundsGrow, _cachedMediaReplaceBoundsGrow);
		}
		return true;
	}

	private bool RenderStaticArrayAuditSnapshotsMatch()
	{
		if (_renderStaticLayerArrayEscaped && !GodotArraysEqual(_layerVisible, _renderStaticAuditLayerVisibleSnapshot))
		{
			return false;
		}
		if (!ShouldAuditRenderStaticMediaArrays())
		{
			return true;
		}
		bool flag = RenderStaticMediaAtlasPathAuditSnapshotMatches();
		return (GodotArraysEqual(_mediaReplaceUse, _renderStaticAuditMediaReplaceUseSnapshot) && GodotArraysEqual(_mediaReplace, _renderStaticAuditMediaReplaceSnapshot) && GodotArraysEqual(mediaReplaceRect, _renderStaticAuditMediaReplaceRectSnapshot) && GodotArraysEqual(mediaReplaceAtlasPages, _renderStaticAuditMediaReplacePageSnapshot)) & flag;
	}

	private bool RenderStaticMediaAtlasPathAuditSnapshotMatches()
	{
		if (GodotArraysEqual(_mediaReplaceAtlasPaths, _renderStaticAuditMediaReplaceAtlasPathSnapshot))
		{
			return true;
		}
		InvalidateStaticAtlasPathLayoutConfigurationSnapshot();
		InvalidateStaticAtlasPathLayoutForNodeMutation();
		QueueUpdateMediaReplace();
		return false;
	}

	private void CaptureRenderStaticArrayAuditSnapshots()
	{
		if (_renderStaticLayerArrayEscaped)
		{
			ReplaceGodotArraySnapshot(ref _renderStaticAuditLayerVisibleSnapshot, _layerVisible);
		}
		else
		{
			_renderStaticAuditLayerVisibleSnapshot = null;
		}
		if (ShouldAuditRenderStaticMediaArrays())
		{
			ReplaceGodotArraySnapshot(ref _renderStaticAuditMediaReplaceUseSnapshot, _mediaReplaceUse);
			ReplaceGodotArraySnapshot(ref _renderStaticAuditMediaReplaceSnapshot, _mediaReplace);
			ReplaceGodotArraySnapshot(ref _renderStaticAuditMediaReplaceRectSnapshot, mediaReplaceRect);
			ReplaceGodotArraySnapshot(ref _renderStaticAuditMediaReplacePageSnapshot, mediaReplaceAtlasPages);
			ReplaceGodotArraySnapshot(ref _renderStaticAuditMediaReplaceAtlasPathSnapshot, _mediaReplaceAtlasPaths);
		}
		else
		{
			_renderStaticAuditMediaReplaceUseSnapshot = null;
			_renderStaticAuditMediaReplaceSnapshot = null;
			_renderStaticAuditMediaReplaceRectSnapshot = null;
			_renderStaticAuditMediaReplacePageSnapshot = null;
			_renderStaticAuditMediaReplaceAtlasPathSnapshot = null;
		}
	}

	private bool ShouldAuditRenderStaticArrays()
	{
		if (!_renderStaticLayerArrayEscaped)
		{
			return ShouldAuditRenderStaticMediaArrays();
		}
		return true;
	}

	internal bool RequiresRenderStaticArrayAuditForRuntimeManager()
	{
		return ShouldAuditRenderStaticArrays();
	}

	internal bool QueueRenderStaticArrayAuditIfDue(ulong physicsFrame)
	{
		if (_renderStaticArrayAuditSubmissionPending || _renderStaticStateNextAuditPhysicsFrame == 18446744073709551615uL || physicsFrame < _renderStaticStateNextAuditPhysicsFrame)
		{
			return false;
		}
		if (_renderStaticStateCached && _cachedLayerStateVersion == _layerStateVersion && _cachedMediaReplaceStateVersion == _mediaReplaceStateVersion && RenderStaticArrayAuditSnapshotsMatch())
		{
			ScheduleNextRenderStaticStateAudit(physicsFrame);
			return false;
		}
		_renderStaticArrayAuditSubmissionPending = true;
		AdobeAnimateSprite renderSortRootForRender = GetRenderSortRootForRender();
		if (GodotObject.IsInstanceValid(renderSortRootForRender))
		{
			renderSortRootForRender.InvalidateRuntimeCrowdCullingForRefresh();
		}
		else
		{
			InvalidateRuntimeCrowdCullingForRefresh();
		}
		RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, staticStateDirty: false);
		return true;
	}

	private bool ShouldAuditRenderStaticMediaArrays()
	{
		if (!_renderStaticMediaArraysEscaped)
		{
			return _cachedAnyRequestedMediaReplace;
		}
		return true;
	}

	private static bool GodotArraysEqual<[MustBeVariant] T>(Array<T> values, Array<T> snapshot)
	{
		if (values == null || snapshot == null)
		{
			if (values == null)
			{
				return snapshot == null;
			}
			return false;
		}
		return values.RecursiveEqual(snapshot);
	}

	private static void ReplaceGodotArraySnapshot<[MustBeVariant] T>(ref Array<T> snapshot, Array<T> values)
	{
		snapshot = values?.Duplicate();
	}

	private void ScheduleNextRenderStaticStateAudit(ulong physicsFrame)
	{
		_renderStaticArrayAuditSubmissionPending = false;
		if (!ShouldAuditRenderStaticArrays())
		{
			_renderStaticStateNextAuditPhysicsFrame = 18446744073709551615uL;
			AdobeAnimateRuntimeManager.SetRenderStaticArrayAuditRegistration(this, enabled: false);
		}
		else
		{
			_renderStaticStateNextAuditPhysicsFrame = physicsFrame + (ulong)GetStaggeredRescanInterval(60);
			AdobeAnimateRuntimeManager.SetRenderStaticArrayAuditRegistration(this, enabled: true);
		}
	}

	private int GetRuntimeLayerVisibleCount(int layerValueCount, AdobeAnimateRuntimeDefinition definition)
	{
		int val = layerValueCount;
		if (definition != null)
		{
			return Math.Max(val, definition.RuntimeLayerCount);
		}
		return Math.Max(val, GetLayerDictionaryLayerCount(_flashAnimeData?.layerDictionary));
	}

	private int GetDataRuntimeLayerCount()
	{
		int num = GetLayerDictionaryLayerCount(_flashAnimeData?.layerDictionary);
		AdobeAnimateRuntimeDefinition runtimeDefinition = GetRuntimeDefinition();
		if (runtimeDefinition != null)
		{
			num = Math.Max(num, runtimeDefinition.RuntimeLayerCount);
		}
		return num;
	}

	private static int GetLayerDictionaryLayerCount(Dictionary dictionary)
	{
		if (dictionary == null || dictionary.Count == 0)
		{
			return 0;
		}
		int num = 0;
		foreach (Variant key in dictionary.Keys)
		{
			int num2 = dictionary[key].AsInt32();
			if (num2 >= 0)
			{
				num = Math.Max(num, num2 + 1);
			}
		}
		return num;
	}

	private bool LayerHasSlicesInCurrentClip(int layerId)
	{
		if (layerId < 0 || layerId >= _cachedLayerVisibleCount)
		{
			return true;
		}
		if (layerId < _cachedLayerActiveInCurrentClip.Length)
		{
			return _cachedLayerActiveInCurrentClip[layerId];
		}
		return true;
	}

	private void RefreshLayerActiveInCurrentClipCache(int layerCount, AdobeAnimateRuntimeDefinition definition)
	{
		if (layerCount <= 0)
		{
			return;
		}
		if (definition == null || definition.Frames == null || definition.SliceMetadata == null)
		{
			EnsureLayerActiveCache(layerCount, defaultValue: true);
			return;
		}
		int num = definition.Frames.Length;
		if (num <= 0)
		{
			EnsureLayerActiveCache(layerCount, defaultValue: true);
			System.Array.Fill(_cachedLayerActiveInCurrentClip, value: true, 0, layerCount);
			return;
		}
		int num2 = 0;
		int num3 = num;
		if (clipRange != Vector2I.Zero)
		{
			num2 = Mathf.Clamp(clipRange.X, 0, num);
			num3 = Mathf.Clamp(clipRange.Y, num2, num);
		}
		LayerActiveCacheKey key = new LayerActiveCacheKey(definition, layerCount, num2, num3);
		if (LayerActiveInClipCache.TryGetValue(key, out var value) && value != null && value.Length >= layerCount)
		{
			EnsureLayerActiveCache(layerCount, defaultValue: false);
			System.Array.Copy(value, _cachedLayerActiveInCurrentClip, layerCount);
			return;
		}
		bool[] array = new bool[layerCount];
		PackedFrame[] frames = definition.Frames;
		PackedSliceMetadata[] sliceMetadata = definition.SliceMetadata;
		for (int i = num2; i < num3; i++)
		{
			PackedFrame packedFrame = frames[i];
			int num4 = packedFrame.Offset;
			int count = packedFrame.Count;
			if (num4 < 0 || count <= 0 || num4 >= sliceMetadata.Length)
			{
				continue;
			}
			int num5 = Math.Min(num4 + count, sliceMetadata.Length);
			for (int j = num4; j < num5; j++)
			{
				int layerId = sliceMetadata[j].LayerId;
				if (layerId >= 0 && layerId < layerCount)
				{
					array[layerId] = true;
				}
			}
		}
		if (LayerActiveInClipCache.Count >= 256)
		{
			LayerActiveInClipCache.Clear();
		}
		LayerActiveInClipCache[key] = array;
		EnsureLayerActiveCache(layerCount, defaultValue: false);
		System.Array.Copy(array, _cachedLayerActiveInCurrentClip, layerCount);
	}

	private void EnsureLayerActiveCache(int layerCount, bool defaultValue)
	{
		if (layerCount > 0)
		{
			if (_cachedLayerActiveInCurrentClip.Length < layerCount)
			{
				System.Array.Resize(ref _cachedLayerActiveInCurrentClip, layerCount);
			}
			System.Array.Fill(_cachedLayerActiveInCurrentClip, defaultValue, 0, layerCount);
		}
	}

	private bool IsMediaReplacedCached(int mediaId)
	{
		if (_cachedHasMediaReplace && mediaId >= 0 && mediaId < _cachedMediaReplaceLimit && _mediaReplaceUse != null)
		{
			return _mediaReplaceUse[mediaId];
		}
		return false;
	}

	private void InvalidateRenderSnapshotCache(bool force = false)
	{
		_needsRenderSubmission = true;
		if (force)
		{
			InvalidateRuntimeDisplayVisualRequirementCache();
			_hasCachedRenderSnapshot = false;
			_cachedRenderSnapshot = default;
			_renderStateCached = false;
			_renderStaticStateCached = false;
			_runtimeRenderBoundsCached = false;
			_runtimeRenderBoundsRescanCountdown = 0;
			_loopTerminalAliasCacheHasValue = false;
			_playbackClipEndCacheHasValue = false;
			_effectiveZIndexCached = false;
			_effectiveRenderSortBandCached = false;
			_treeOrderPathRescanCountdown = 0;
		}
	}

	public void QueueUpdateMediaReplace()
	{
		if (!needMediaReplaceUpdate)
		{
			InvalidateStaticAtlasPathLayoutForNodeMutation();
			needMediaReplaceUpdate = true;
			MarkEffectOnceBatchEligibilityChanged();
			if (IsInsideTree())
			{
				CallDeferred(MethodName.UpdateMediaReplace);
			}
		}
	}

	public void UpdateMediaReplace()
	{
		if (needMediaReplaceUpdate)
		{
			needMediaReplaceUpdate = false;
			UpdateMediaReplaceData();
		}
	}

	public void UpdateMediaReplaceData()
	{
		PrepareStaticAtlasPathLayoutForExplicitUpdate();
		needMediaReplaceUpdate = false;
		CreateMediaReplaceAtlas();
		_mediaReplaceStateVersion++;
		MarkEffectOnceBatchEligibilityChanged();
		InvalidateRenderSnapshotCache();
		RequestNodeRedraw();
	}

	public void CreateMediaReplaceAtlas()
	{
		if (!_bypassStaticAtlasPathLayoutCacheForTests && TryReuseAppliedStaticAtlasPathLayout())
		{
			return;
		}
		mediaReplaceAtlasShared = false;
		mediaReplaceAtlas = null;
		mediaReplaceAtlasArray = null;
		mediaReplaceAtlasArraySize = default;
		mediaReplaceAtlasUsesTextureArray = false;
		if (_flashAnimeData == null || _flashAnimeData.mediaDictionary == null || (!_bypassStaticAtlasPathLayoutCacheForTests && TryApplyStaticAtlasPathLayoutCache()))
		{
			return;
		}
		List<Texture2D> list = new List<Texture2D>();
		List<string> list2 = new List<string>();
		List<int> list3 = new List<int>();
		int num = Math.Min(_mediaReplace.Count, _mediaReplaceUse.Count);
		for (int i = 0; i < num; i++)
		{
			if (_mediaReplaceUse[i])
			{
				Texture2D texture2D = _mediaReplace[i];
				string text = ((i < _mediaReplaceAtlasPaths.Count) ? _mediaReplaceAtlasPaths[i] : string.Empty);
				if (!GodotObject.IsInstanceValid(texture2D) && string.IsNullOrEmpty(text))
				{
					_mediaReplaceUse[i] = false;
					continue;
				}
				list.Add(texture2D);
				list2.Add(text);
				list3.Add(i);
			}
		}
		mediaReplaceRect.Clear();
		mediaReplaceRect.Resize(_flashAnimeData.mediaDictionary.Count);
		mediaReplaceAtlasPages.Clear();
		mediaReplaceAtlasPages.Resize(_flashAnimeData.mediaDictionary.Count);
		for (int j = 0; j < mediaReplaceRect.Count; j++)
		{
			mediaReplaceRect[j] = default;
			mediaReplaceAtlasPages[j] = 0;
		}
		if (list.Count == 0)
		{
			mediaReplaceAtlas = null;
		}
		else if (!TryUseSharedMediaReplaceAtlas(list, list2, list3))
		{
			DisableUnsharedMediaReplaceTextures(list3);
		}
		else if (!_bypassStaticAtlasPathLayoutCacheForTests)
		{
			TryPublishStaticAtlasPathLayoutCache();
		}
	}

	public override void _Draw()
	{
		if (_effectOnceGpuSuppressed || suppressEditorRenderSubmission || (_usingRuntimeManager && !CachedEditorHint && !_forceLocalRender) || IsRenderedByParentSpriteForRender() || !TryBuildRenderSnapshot(out var snapshot))
		{
			return;
		}
		if (CachedEditorHint)
		{
			if (_preview)
			{
				SubmitRuntimeRenderSnapshot(snapshot);
			}
		}
		else if (IsInSubViewportRenderTarget())
		{
			SubmitRuntimeRenderSnapshot(snapshot);
		}
		else if (_forceLocalRender)
		{
			SubmitRuntimeRenderSnapshot(snapshot);
		}
		else if (!_usingRuntimeManager)
		{
			SubmitRuntimeRenderSnapshot(snapshot);
		}
	}

	internal bool IsRenderedByParentSpriteForRender()
	{
		AdobeAnimateSprite adobeAnimateSprite = ResolveParentSpriteForLayerInspector();
		if (adobeAnimateSprite != null && adobeAnimateSprite != this)
		{
			return adobeAnimateSprite.OwnsSpriteChildForRender(this);
		}
		return false;
	}

	private void SubmitRuntimeRenderSnapshot(AdobeAnimateRenderSnapshot snapshot)
	{
		if (_forceCpuPoseRender)
		{
			AdobeAnimateRenderManager.SubmitCpuPose(snapshot, immediateFlush: true);
		}
		else
		{
			AdobeAnimateRenderManager.Submit(snapshot, immediateFlush: true);
		}
	}

	private void SubmitRuntimeManagerFirstFrameHandoff()
	{
		if (_usingRuntimeManager && _runtimeInsideTree && !_forceLocalRender && !IsRenderedByParentSpriteForRender() && AdobeAnimateRenderManager.HasPreparedRenderMount(this) && TryBuildRenderSnapshot(out var snapshot))
		{
			_runtimeManagerFirstFrameHandoffActive = AdobeAnimateRenderManager.SubmitCpuPose(snapshot, immediateFlush: true);
		}
	}

	internal bool TrySubmitRuntimeRenderFailureHandoff()
	{
		if (!_usingRuntimeManager || !_runtimeInsideTree || _forceLocalRender || IsRenderedByParentSpriteForRender() || !AdobeAnimateRenderManager.HasPreparedRenderMount(this))
		{
			return false;
		}
		if (!TryBuildRenderSnapshot(out var snapshot))
		{
			return false;
		}
		bool flag = AdobeAnimateRenderManager.SubmitCpuPose(snapshot);
		_runtimeManagerFirstFrameHandoffActive |= flag;
		RequestRuntimeRenderSubmissionRetry();
		return flag;
	}

	private void ReleaseRuntimeManagerFirstFrameHandoff()
	{
		if (_runtimeManagerFirstFrameHandoffActive)
		{
			_runtimeManagerFirstFrameHandoffActive = false;
			AdobeAnimateRenderManager.ReleaseImmediateSubmission(this);
			if (_runtimeDefinition != null)
			{
				AdobeAnimateDefinitionCache.ReleaseCpuPoseDataIfBaked(_runtimeDefinition);
			}
		}
	}

	private void ReleaseRuntimeManagerFirstFrameHandoffTree()
	{
		ReleaseRuntimeManagerFirstFrameHandoff();
		AdobeAnimateSprite[] spriteChildrenForRender = GetSpriteChildrenForRender();
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildrenForRender)
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.ReleaseRuntimeManagerFirstFrameHandoffTree();
			}
		}
	}

	public void ReleaseForcedCpuPoseData()
	{
		if (_forceCpuPoseRender)
		{
			_forceCpuPoseRender = false;
			if (_runtimeDefinition != null)
			{
				AdobeAnimateDefinitionCache.ReleaseCpuPoseDataIfBaked(_runtimeDefinition);
			}
			InvalidateRenderSnapshotCache(force: true);
			RequestNodeRedraw();
		}
	}

	private static bool HasRenderableAtlas(AdobeAnimateRuntimeDefinition definition)
	{
		if (definition != null && definition.Frames != null && definition.SliceMetadata != null && GodotObject.IsInstanceValid(definition.AtlasTextureArray))
		{
			return definition.AtlasTextureArrayRid.IsValid;
		}
		return false;
	}

	public override void _Process(double delta)
	{
		if (!_effectOnceGpuSuppressed && (!_usingRuntimeManager || _dispatchingBatchedProcess))
		{
			RunDefaultBatchedProcessUpdate(delta);
		}
	}

	public void RunBatchedProcessUpdate(double delta)
	{
		if (_effectOnceGpuSuppressed)
		{
			return;
		}
		_dispatchingBatchedProcess = true;
		try
		{
			RunDefaultBatchedProcessUpdate(delta);
		}
		finally
		{
			_dispatchingBatchedProcess = false;
		}
	}

	internal void RunDisplayFrameVisualUpdate(double delta)
	{
		if (_effectOnceGpuSuppressed)
		{
			return;
		}
		_displayFrameVisualDispatchActive = true;
		try
		{
			RunDefaultBatchedProcessUpdate(delta, _displayFrameGpuClockPoseReusable);
		}
		finally
		{
			_displayFrameVisualDispatchActive = false;
		}
	}

	public void BatchProcessUpdate(double delta)
	{
		BatchProcessUpdate(delta, reuseRuntimeGpuClockPose: false);
	}

	internal void RunDefaultBatchedProcessUpdate(double delta)
	{
		RunDefaultBatchedProcessUpdate(delta, reuseRuntimeGpuClockPose: false);
	}

	private void RunDefaultBatchedProcessUpdate(double delta, bool reuseRuntimeGpuClockPose)
	{
		BatchProcessUpdate(delta, reuseRuntimeGpuClockPose);
		RunBatchedProcessExtension(delta);
	}

	protected virtual void RunBatchedProcessExtension(double delta)
	{
	}

	private void BatchProcessUpdate(double delta, bool reuseRuntimeGpuClockPose)
	{
		if (_effectOnceGpuSuppressed)
		{
			return;
		}
		_managedPoseRevision++;
		long startTicks = TowerDefensePerfProfiler.BeginHotPath();
		if (!_canRun)
		{
			return;
		}
		if (IsPlaybackStopped)
		{
			if (keepRenderSubmittedWhenPaused && !_usingRuntimeManager)
			{
				QueueRedraw();
			}
			return;
		}
		if (TryFastLoopProcessUpdate(delta, reuseRuntimeGpuClockPose))
		{
			TowerDefensePerfProfiler.End("adobeAnimate.sprite.fastLoop", startTicks, 1);
			return;
		}
		int num = frameIndex;
		double num2 = elapsedTimer;
		bool flag = clipOver;
		bool flag2 = blend;
		double num3 = blendTimer;
		if (_nextAnimDelayTimer > 0.0)
		{
			_nextAnimDelayTimer -= delta;
			if (_nextAnimDelayTimer <= 0.0)
			{
				_nextAnimDelayTimer = -1.0;
				ApplyNextAnimation(_pendingTrackConfig);
				_pendingTrackConfig = null;
			}
			TowerDefensePerfProfiler.End("adobeAnimate.sprite.nextDelay", startTicks, 1);
			return;
		}
		bool flag3 = _displayFrameVisualDispatchActive;
		int playbackClipEndExclusive = GetPlaybackClipEndExclusive();
		bool flag4 = playbackClipEndExclusive - clipRange.X <= 1;
		if (!clipOver)
		{
			if (!flag4)
			{
				refreshTimer += delta * Mathf.Max(10.0, trueFrameRate);
				if (refreshTimer >= 1.0)
				{
					flag3 = true;
					refreshTimer = 0.0;
				}
			}
			else
			{
				frameIndex = clipRange.X;
				flag3 = true;
				refreshTimer = 0.0;
			}
			if (!playBack)
			{
				if (!flag4)
				{
					elapsedTimer += delta * Mathf.Abs(timeScale) * frameRate;
				}
				if (refreshEveryFrame || refreshEveryFlame || elapsedTimer >= 1.0)
				{
					flag3 = true;
				}
				while (elapsedTimer >= 1.0)
				{
					elapsedTimer--;
					EmitFrameEvents(frameIndex);
					frameIndex++;
					if (frameIndex < playbackClipEndExclusive)
					{
						continue;
					}
					EmitFrameEvents(frameIndex);
					if (!loop)
					{
						clipOver = true;
						string text = _clip;
						frameIndex = Math.Max(clipRange.X, playbackClipEndExclusive - 1);
						elapsedTimer = 0.0;
						NextAnimation();
						if (!CachedEditorHint)
						{
							OnAnimeCompleted?.Invoke(text);
						}
					}
					else
					{
						if (!CachedEditorHint)
						{
							OnAnimeCompleted?.Invoke(_clip);
						}
						frameIndex = clipRange.X;
						elapsedTimer = 0.0;
						flag3 = true;
					}
				}
			}
			else
			{
				if (!flag4)
				{
					elapsedTimer -= delta * Mathf.Abs(timeScale) * frameRate;
				}
				if (refreshEveryFrame || refreshEveryFlame || elapsedTimer <= -1.0)
				{
					flag3 = true;
				}
				while (elapsedTimer <= -1.0)
				{
					elapsedTimer++;
					EmitFrameEvents(frameIndex);
					frameIndex--;
					if (frameIndex >= clipRange.X)
					{
						continue;
					}
					if (!loop)
					{
						clipOver = true;
						string text2 = _clip;
						frameIndex = clipRange.X;
						elapsedTimer = 0.0;
						NextAnimation();
						if (!CachedEditorHint)
						{
							OnAnimeCompleted?.Invoke(text2);
						}
					}
					else
					{
						if (!CachedEditorHint)
						{
							OnAnimeCompleted?.Invoke(_clip);
						}
						frameIndex = Math.Max(clipRange.X, clipRange.Y - 1);
						elapsedTimer = 0.0;
						flag3 = true;
					}
				}
			}
			if (!clipOver && blend)
			{
				blendTimer = Mathf.Clamp(blendTimer + delta * (double)Mathf.Abs(Mathf.Sign(timeScale)), 0.0, blendTime);
				if (blendTimer >= blendTime)
				{
					OnAnimeBlendCompleted?.Invoke(_clip);
					blendTime = 0.0;
					blendTimer = 0.0;
					blend = false;
					flag3 = true;
				}
			}
		}
		if (clipOver)
		{
			_needsRenderSubmission = true;
			RequestNodeRedraw();
			if (track.Count <= 0 && _nextAnimDelayTimer <= 0.0)
			{
				StopRuntimeTickKeepRender();
			}
			return;
		}
		if (flag3)
		{
			bool flag5 = frameIndex != _lastUpdateFrame;
			if (flag5)
			{
				_lastUpdateFrame = frameIndex;
			}
			if (_hasChildren && !ShouldSkipRuntimeChildUpdateForViewport())
			{
				long startTicks2 = TowerDefensePerfProfiler.BeginHotPath();
				UpdateChild();
				TowerDefensePerfProfiler.End("adobeAnimate.sprite.updateChild", startTicks2, 1);
			}
			refreshTimer = 0.0;
			if (_invisible & flag5)
			{
				CallDeferred(MethodName.UpdateInvisibleChild);
			}
		}
		if (frameIndex != num || !Mathf.IsEqualApprox((float)elapsedTimer, (float)num2) || clipOver != flag || blend != flag2 || !Mathf.IsEqualApprox((float)blendTimer, (float)num3))
		{
			RequestAnimationPoseRedraw();
		}
		TowerDefensePerfProfiler.End("adobeAnimate.sprite.fullProcess", startTicks, 1);
	}

	private bool TryFastLoopProcessUpdate(double delta, bool reuseRuntimeGpuClockPose)
	{
		if (clipOver || playBack || !loop || blend || _nextAnimDelayTimer > 0.0 || refreshEveryFrame || refreshEveryFlame || track.Count > 0)
		{
			return false;
		}
		bool flag = HasRuntimeChildUpdates();
		int x = clipRange.X;
		int playbackClipEndExclusive = GetPlaybackClipEndExclusive();
		int num = playbackClipEndExclusive - x;
		if (num <= 1)
		{
			if (frameIndex != x || elapsedTimer != 0.0)
			{
				frameIndex = x;
				elapsedTimer = 0.0;
				if (!reuseRuntimeGpuClockPose)
				{
					RequestAnimationPoseRedraw();
				}
			}
			if (flag && _lastUpdateFrame != frameIndex && !ShouldSkipRuntimeChildUpdateForViewport())
			{
				_lastUpdateFrame = frameIndex;
				long startTicks = TowerDefensePerfProfiler.BeginHotPath();
				UpdateChild();
				TowerDefensePerfProfiler.End("adobeAnimate.sprite.fastSlotUpdate", startTicks, 1);
			}
			return true;
		}
		double num2 = delta * Math.Abs(timeScale) * frameRate;
		if (num2 <= 0.0)
		{
			return true;
		}
		int num3 = frameIndex;
		double num4 = elapsedTimer;
		int num5 = (int)Math.Floor(num4 + num2);
		double num6 = (double)(frameIndex - x) + elapsedTimer + num2;
		if (num6 >= (double)num || num6 < 0.0)
		{
			num6 %= (double)num;
			if (num6 < 0.0)
			{
				num6 += (double)num;
			}
		}
		if (num5 > 0 && (OnAnimeEvent != null || OnAnimeCompleted != null))
		{
			ulong playbackRevision = _playbackRevision;
			if (EmitFastLoopCallbacks(num3, num5, x, playbackClipEndExclusive, playbackRevision))
			{
				return true;
			}
		}
		int num7 = (int)num6;
		frameIndex = x + num7;
		elapsedTimer = num6 - (double)num7;
		if (!reuseRuntimeGpuClockPose && (frameIndex != num3 || !Mathf.IsEqualApprox((float)elapsedTimer, (float)num4)))
		{
			RequestAnimationPoseRedraw();
		}
		if (flag)
		{
			refreshTimer += delta * Mathf.Max(10.0, trueFrameRate);
			if (_displayFrameVisualDispatchActive || refreshTimer >= 1.0)
			{
				_lastUpdateFrame = frameIndex;
				if (!ShouldSkipRuntimeChildUpdateForViewport())
				{
					long startTicks2 = TowerDefensePerfProfiler.BeginHotPath();
					UpdateChild();
					TowerDefensePerfProfiler.End("adobeAnimate.sprite.fastSlotUpdate", startTicks2, 1);
				}
				refreshTimer = 0.0;
			}
		}
		return true;
	}

	private bool EmitFastLoopCallbacks(int startFrame, int crossedFrames, int clipStart, int clipEnd, ulong playbackRevision)
	{
		int num = clipEnd - clipStart;
		if (num <= 0)
		{
			return false;
		}
		int num2 = Math.Min(crossedFrames, num * 4);
		int num3 = Mathf.Clamp(startFrame - clipStart, 0, num - 1);
		for (int i = 0; i < num2; i++)
		{
			int frameIdx = clipStart + num3;
			EmitFrameEvents(frameIdx);
			if (_playbackRevision != playbackRevision)
			{
				return true;
			}
			num3++;
			if (num3 >= num)
			{
				EmitFrameEvents(clipEnd);
				if (_playbackRevision != playbackRevision)
				{
					return true;
				}
				if (!CachedEditorHint)
				{
					OnAnimeCompleted?.Invoke(_clip);
				}
				if (_playbackRevision != playbackRevision)
				{
					return true;
				}
				num3 = 0;
			}
		}
		if (crossedFrames > num2)
		{
			int num4 = (crossedFrames - num2) / num;
			for (int j = 0; j < num4; j++)
			{
				if (!CachedEditorHint)
				{
					OnAnimeCompleted?.Invoke(_clip);
				}
				if (_playbackRevision != playbackRevision)
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool HasRuntimeChildUpdates()
	{
		if (!CachedEditorHint)
		{
			return _runtimeHasChildUpdates;
		}
		return _hasChildren;
	}

	private void RefreshRuntimeChildUpdatePresence()
	{
		if (_spriteChildren.Length != 0)
		{
			_runtimeHasChildUpdates = true;
			return;
		}
		for (int i = 0; i < _slotChildren.Length; i++)
		{
			AdobeAnimateSlot adobeAnimateSlot = _slotChildren[i];
			if (adobeAnimateSlot != null && adobeAnimateSlot.RequiresRuntimeUpdate)
			{
				_runtimeHasChildUpdates = true;
				return;
			}
		}
		_runtimeHasChildUpdates = false;
	}

	private bool ShouldSkipRuntimeChildUpdateForViewport()
	{
		if (ShouldUseGpuGraphVisualAttachment())
		{
			return true;
		}
		if (CachedEditorHint || !_hasChildren || !RuntimeChildUpdatesAreVisualOnly())
		{
			return false;
		}
		return !IsRuntimeChildUpdateNearViewport();
	}

	private bool ShouldUseGpuGraphVisualAttachment()
	{
		if (CachedEditorHint || !_usingRuntimeManager || (Global.Instance != null && Global.Instance.adobeAnimateRenderBackend != AdobeAnimateRenderBackend.GpuCrowd && Global.Instance.adobeAnimateRenderBackend != AdobeAnimateRenderBackend.CpuPose) || !_hasChildren || !RuntimeChildUpdatesAreVisualOnly())
		{
			return false;
		}
		AdobeAnimateSprite renderSortRootForRender = GetRenderSortRootForRender();
		if (GodotObject.IsInstanceValid(renderSortRootForRender) && !renderSortRootForRender._forceCpuPoseRender)
		{
			return AdobeAnimateRenderManager.IsGpuGraphReady(renderSortRootForRender);
		}
		return false;
	}

	private bool RuntimeChildUpdatesAreVisualOnly()
	{
		if (!_runtimeChildUpdatesVisualOnlyDirty)
		{
			return _runtimeChildUpdatesVisualOnlyCached;
		}
		bool flag = _spriteChildren.Length != 0;
		AdobeAnimateSlot[] runtimeSlotChildren = GetRuntimeSlotChildren();
		foreach (AdobeAnimateSlot adobeAnimateSlot in runtimeSlotChildren)
		{
			if (adobeAnimateSlot != null && GodotObject.IsInstanceValid(adobeAnimateSlot))
			{
				if (!adobeAnimateSlot.RuntimeUpdateVisualOnly)
				{
					flag = false;
					break;
				}
				flag = true;
			}
		}
		_runtimeChildUpdatesVisualOnlyCached = flag;
		_runtimeChildUpdatesVisualOnlyDirty = false;
		return flag;
	}

	private bool IsRuntimeChildUpdateNearViewport()
	{
		long processFrames = (long)Engine.GetProcessFrames();
		if (_runtimeChildViewportFrame == processFrames)
		{
			return _runtimeChildViewportVisible;
		}
		_runtimeChildViewportFrame = processFrames;
		_runtimeChildViewportVisible = true;
		Viewport viewport = GetViewport();
		if (!GodotObject.IsInstanceValid(viewport))
		{
			return true;
		}
		Rect2 visibleRect = viewport.GetVisibleRect();
		if (visibleRect.Size.X <= 0f || visibleRect.Size.Y <= 0f)
		{
			return true;
		}
		Vector2 origin = GetGlobalTransformWithCanvas().Origin;
		Vector2 vector = new Vector2(256f, 256f);
		_runtimeChildViewportVisible = new Rect2(visibleRect.Position - vector, visibleRect.Size + vector * 2f).HasPoint(origin);
		return _runtimeChildViewportVisible;
	}

	public void UpdateInvisibleChild()
	{
		UpdateChild();
	}

	public void FileChange()
	{
		ApplyFlashAnimeDataChange(invalidateDefinitionCache: true);
	}

	private void OnFlashAnimeDataChanged()
	{
		ApplyFlashAnimeDataChange(invalidateDefinitionCache: true);
	}

	private void ApplyFlashAnimeDataChange(bool invalidateDefinitionCache)
	{
		InvalidateStaticAtlasPathLayoutForNodeMutation();
		_managedPoseRevision++;
		_runtimeDefinition = null;
		meshTexture = default;
		if (_flashAnimeData == null)
		{
			return;
		}
		TryHydrateFlashAnimeDataBeforeFileChange();
		if (invalidateDefinitionCache)
		{
			AdobeAnimateDefinitionCache.Invalidate(_flashAnimeData);
		}
		if (!HasUsableFlashAnimeData())
		{
			return;
		}
		frameIndex = 0;
		frameRate = _flashAnimeData.frameRate;
		int layerDictionaryLayerCount = GetLayerDictionaryLayerCount(_flashAnimeData.layerDictionary);
		if (_layerVisible.Count != layerDictionaryLayerCount)
		{
			_layerVisible.Clear();
			_layerVisible.Resize(layerDictionaryLayerCount);
			for (int i = 0; i < _layerVisible.Count; i++)
			{
				_layerVisible[i] = true;
			}
			_mediaReplace.Clear();
			_mediaReplace.Resize(_flashAnimeData.mediaDictionary.Count);
			_mediaReplaceUse.Clear();
			_mediaReplaceUse.Resize(_flashAnimeData.mediaDictionary.Count);
			_mediaReplaceAtlasPaths.Clear();
			_mediaReplaceAtlasPaths.Resize(_flashAnimeData.mediaDictionary.Count);
			MarkLayerStateChanged();
		}
		else if (_mediaReplaceAtlasPaths.Count != _flashAnimeData.mediaDictionary.Count)
		{
			_mediaReplaceAtlasPaths.Clear();
			_mediaReplaceAtlasPaths.Resize(_flashAnimeData.mediaDictionary.Count);
		}
		mediaReplaceRect.Clear();
		mediaReplaceRect.Resize(_flashAnimeData.mediaDictionary.Count);
		for (int j = 0; j < mediaReplaceRect.Count; j++)
		{
			mediaReplaceRect[j] = default;
		}
		_mediaReplaceStateVersion++;
		MarkEffectOnceBatchEligibilityChanged();
		onlyDraw = false;
		mesh = null;
		Material = null;
		ICollection<Variant> keys = _flashAnimeData.clips.Keys;
		if (keys.Count > 0 && string.IsNullOrEmpty(_clip))
		{
			using IEnumerator<Variant> enumerator = keys.GetEnumerator();
			if (enumerator.MoveNext())
			{
				_clip = enumerator.Current.ToString();
			}
		}
		InvalidateRenderSnapshotCache(force: true);
	}

	private bool DeferFlashAnimeDataChangeUntilReady()
	{
		if (!IsInsideTree())
		{
			return !CachedEditorHint;
		}
		return false;
	}

	private void ApplyPendingFlashAnimeDataChange()
	{
		if (_flashAnimeDataChangePending)
		{
			_flashAnimeDataChangePending = false;
			ApplyFlashAnimeDataChange(invalidateDefinitionCache: false);
		}
	}

	private void PrepareFlashAnimeDataSerializedOverrides()
	{
		_runtimeDefinition = null;
		meshTexture = default;
		if (_flashAnimeData == null)
		{
			return;
		}
		TryHydrateFlashAnimeDataBeforeFileChange();
		if (!HasUsableFlashAnimeData())
		{
			return;
		}
		frameRate = _flashAnimeData.frameRate;
		int layerDictionaryLayerCount = GetLayerDictionaryLayerCount(_flashAnimeData.layerDictionary);
		if (_layerVisible.Count != layerDictionaryLayerCount)
		{
			_layerVisible.Clear();
			_layerVisible.Resize(layerDictionaryLayerCount);
			for (int i = 0; i < _layerVisible.Count; i++)
			{
				_layerVisible[i] = true;
			}
		}
		int count = _flashAnimeData.mediaDictionary.Count;
		if (_mediaReplace.Count != count)
		{
			_mediaReplace.Clear();
			_mediaReplace.Resize(count);
		}
		if (_mediaReplaceUse.Count != count)
		{
			_mediaReplaceUse.Clear();
			_mediaReplaceUse.Resize(count);
		}
		if (_mediaReplaceAtlasPaths.Count != count)
		{
			_mediaReplaceAtlasPaths.Clear();
			_mediaReplaceAtlasPaths.Resize(count);
		}
	}

	private bool HasUsableFlashAnimeData()
	{
		if (_flashAnimeData == null || _flashAnimeData.layerDictionary == null || _flashAnimeData.mediaDictionary == null || _flashAnimeData.clips == null)
		{
			return false;
		}
		return _flashAnimeData.HasPackedRuntimeData();
	}

	private void TryHydrateFlashAnimeDataBeforeFileChange()
	{
		if (_flashAnimeData != null && (_flashAnimeData.clips == null || _flashAnimeData.clips.Count == 0 || !_flashAnimeData.HasPackedRuntimeData()))
		{
			_flashAnimeData.EnsurePackedRuntimeData();
			if (!_flashAnimeData.HasPackedRuntimeData())
			{
				_flashAnimeData.TryInitForEditorAssignedAnimeFile();
			}
		}
	}

	private void RefreshTransformNotificationMode()
	{
		if (_runtimeInsideTree)
		{
			AdobeAnimateSprite renderSortRootForRender = GetRenderSortRootForRender();
			bool flag = _parentSprite != null && GodotObject.IsInstanceValid(renderSortRootForRender) && renderSortRootForRender != this && renderSortRootForRender.ShouldUseGlobalRuntimeManager();
			SetNotifyTransform(CachedEditorHint || !flag);
		}
	}

	private void RefreshTransformNotificationModeForRenderTree()
	{
		RefreshTransformNotificationMode();
		MarkRenderTransformDirty();
		AdobeAnimateSprite[] spriteChildrenForRender = GetSpriteChildrenForRender();
		foreach (AdobeAnimateSprite adobeAnimateSprite in spriteChildrenForRender)
		{
			if (GodotObject.IsInstanceValid(adobeAnimateSprite))
			{
				adobeAnimateSprite.RefreshTransformNotificationModeForRenderTree();
			}
		}
	}

	private void MarkRenderTransformDirty(bool changedInPhysicsFrame = false)
	{
		_knownAncestorTranslationNotificationPending = false;
		bool flag = changedInPhysicsFrame || _runtimePhysicsAncestorTransformPending || TowerDefenseProcessModeDispatch.IsCharacterPhysicsBatchDispatchActive || Engine.IsInPhysicsFrame();
		if (!changedInPhysicsFrame)
		{
			_runtimePhysicsAncestorTransformPending = false;
		}
		if (_renderGlobalTransformDirty)
		{
			if (flag)
			{
				_renderTransformChangedInPhysicsFrame = true;
			}
		}
		else
		{
			_renderGlobalTransformDirty = true;
			_renderTransformChangedInPhysicsFrame = flag;
			InvalidateRenderSnapshotCache();
			RequestNodeRedraw(flag, transformOnly: true);
		}
	}

	private bool ClearRetainedRootMotionForPause()
	{
		return ClearRetainedRootMotionForStoppedMovement();
	}

	internal bool ClearRetainedRootMotionForStoppedMovement()
	{
		Transform2D globalTransform = GlobalTransform;
		if (!_renderRootMotionEnabled && _renderGlobalTransformCached && !_renderGlobalTransformDirty && !(_renderGlobalTransform != globalTransform))
		{
			return false;
		}
		_renderRootMotionEnabled = false;
		_renderGlobalTransform = globalTransform;
		_renderPreviousGlobalTransform = globalTransform;
		_renderGlobalTransformCached = true;
		_renderGlobalTransformDirty = false;
		_renderTransformChangedInPhysicsFrame = false;
		_runtimePhysicsAncestorTransformPending = false;
		_renderGlobalTransformPhysicsFrame = Engine.GetPhysicsFrames();
		if (_hasCachedGpuGraphCrowdState)
		{
			_cachedGpuGraphCrowdState.ClearRootMotionForRender();
		}
		InvalidateRenderSnapshotCache(force: true);
		RequestNodeRedraw();
		return true;
	}

	private bool CanInterpolateRenderRootMotion()
	{
		if (!IsPlaybackStopped)
		{
			return !_renderTreePaused;
		}
		return false;
	}

	private bool HasCurrentPhysicsRootMotion()
	{
		if (!_renderRootMotionEnabled)
		{
			return false;
		}
		if (AdobeAnimateRenderManager.GetPhysicsFrameForRender() == _renderGlobalTransformPhysicsFrame)
		{
			return true;
		}
		_renderRootMotionEnabled = false;
		_renderPreviousGlobalTransform = _renderGlobalTransform;
		return false;
	}

	internal void NotifyAncestorTransformChangedForRender(bool changedInPhysicsFrame = false)
	{
		AdobeAnimateSprite renderSortRootForRender = GetRenderSortRootForRender();
		if (GodotObject.IsInstanceValid(renderSortRootForRender) && renderSortRootForRender != this && renderSortRootForRender.ShouldUseGlobalRuntimeManager())
		{
			if (changedInPhysicsFrame)
			{
				renderSortRootForRender._runtimePhysicsAncestorTransformPending = true;
			}
			renderSortRootForRender.MarkRenderTransformDirty(changedInPhysicsFrame);
		}
		else
		{
			NotifyLocalRenderTransformChangedForTree(changedInPhysicsFrame);
		}
	}

	internal void NotifyAncestorTranslatedForRender(Vector2 globalDelta, ulong physicsFrame)
	{
		if (globalDelta == Vector2.Zero)
		{
			return;
		}
		if (_parentSprite == null)
		{
			ApplyKnownGlobalTranslationForRender(globalDelta, physicsFrame);
			return;
		}
		AdobeAnimateSprite renderSortRootForRender = GetRenderSortRootForRender();
		if (GodotObject.IsInstanceValid(renderSortRootForRender) && renderSortRootForRender != this && renderSortRootForRender.ShouldUseGlobalRuntimeManager())
		{
			renderSortRootForRender.ApplyKnownGlobalTranslationForRender(globalDelta, physicsFrame);
		}
		else
		{
			ApplyKnownGlobalTranslationForRender(globalDelta, physicsFrame);
		}
	}

	internal void PrimeAncestorTranslationForRender()
	{
		AdobeAnimateSprite renderSortRootForRender = GetRenderSortRootForRender();
		if (GodotObject.IsInstanceValid(renderSortRootForRender))
		{
			renderSortRootForRender.GetCachedGlobalTransformForRender();
		}
	}

	internal void InvalidateRuntimeRenderMountForAncestorChange()
	{
		_renderMountNextAuditPhysicsFrame = 0uL;
		ResetEffectiveRenderModulateAncestorCache();
		InvalidateTreeOrderPathCache();
		RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, staticStateDirty: false);
	}

	internal void InvalidateProgressRestorePresentation()
	{
		AdobeAnimateSprite renderSortRootForRender = GetRenderSortRootForRender();
		if (GodotObject.IsInstanceValid(renderSortRootForRender))
		{
			renderSortRootForRender.InvalidateRuntimeCrowdCullingForRefresh();
			renderSortRootForRender.InvalidateRenderSnapshotCache(force: true);
			renderSortRootForRender.MarkRenderTransformDirty();
			renderSortRootForRender._needsRenderSubmission = true;
			renderSortRootForRender.RequestNodeRedraw(preferDisplayCadence: false, transformOnly: false, staticStateDirty: false);
		}
	}

	private void InvalidateStaticAtlasPathLayoutConfigurationSnapshot()
	{
		_staticAtlasPathSnapshotValid = false;
		_staticAtlasPathSnapshotData = null;
	}

	private void CaptureStaticAtlasPathLayoutConfigurationSnapshot()
	{
		_staticAtlasPathSnapshotValid = false;
		if (_flashAnimeData == null || _flashAnimeData.mediaDictionary == null)
		{
			return;
		}
		int count = _flashAnimeData.mediaDictionary.Count;
		int num = _mediaReplace?.Count ?? 0;
		int num2 = _mediaReplaceUse?.Count ?? 0;
		int num3 = _mediaReplaceAtlasPaths?.Count ?? 0;
		int num4 = Math.Min(num, num2);
		if (count <= 0 || num != count || num2 != count || num3 != count || num4 != count)
		{
			return;
		}
		if (_staticAtlasPathSnapshotActive.Length != num4)
		{
			_staticAtlasPathSnapshotActive = new bool[num4];
			_staticAtlasPathSnapshotPaths = new string[num4];
		}
		ulong num5 = 1469598103934665603uL;
		int num6 = 0;
		for (int i = 0; i < num4; i++)
		{
			bool flag = _mediaReplaceUse[i];
			string text = _mediaReplaceAtlasPaths[i] ?? string.Empty;
			_staticAtlasPathSnapshotActive[i] = flag;
			_staticAtlasPathSnapshotPaths[i] = text;
			num5 ^= (ulong)(flag ? 1 : 0);
			num5 *= 1099511628211L;
			for (int j = 0; j < text.Length; j++)
			{
				num5 ^= text[j];
				num5 *= 1099511628211L;
			}
			num5 ^= 0xFF;
			num5 *= 1099511628211L;
			if (flag)
			{
				num6++;
				Texture2D texture2D = _mediaReplace[i];
				if ((texture2D != null && GodotObject.IsInstanceValid(texture2D)) || string.IsNullOrWhiteSpace(text))
				{
					return;
				}
			}
		}
		if (num6 > 0)
		{
			_staticAtlasPathSnapshotData = _flashAnimeData;
			_staticAtlasPathSnapshotDataInstanceId = _flashAnimeData.GetInstanceId();
			_staticAtlasPathSnapshotAuthoringRevision = _flashAnimeData.AuthoringRevision;
			_staticAtlasPathSnapshotMediaCount = count;
			_staticAtlasPathSnapshotTextureCount = num;
			_staticAtlasPathSnapshotUseCount = num2;
			_staticAtlasPathSnapshotPathCount = num3;
			_staticAtlasPathSnapshotEffectiveLimit = num4;
			_staticAtlasPathSnapshotActiveCount = num6;
			_staticAtlasPathSnapshotSlotSignature = num5;
			_staticAtlasPathSnapshotValid = true;
		}
	}

	private void InvalidateStaticAtlasPathLayoutForNodeMutation(bool markRuntimeMutation = true)
	{
		_appliedStaticAtlasPathLayout = null;
		if (markRuntimeMutation && _runtimeReadyCached)
		{
			_staticAtlasPathLayoutRuntimeMutationSeen = true;
		}
	}

	private void PrepareStaticAtlasPathLayoutForExplicitUpdate()
	{
		bool staticAtlasPathLayoutGpuResetPending = _staticAtlasPathLayoutGpuResetPending;
		_staticAtlasPathLayoutGpuResetPending = false;
		if (!_runtimeReadyCached)
		{
			InvalidateStaticAtlasPathLayoutForNodeMutation(markRuntimeMutation: false);
			CaptureStaticAtlasPathLayoutConfigurationSnapshot();
		}
		else
		{
			InvalidateStaticAtlasPathLayoutForNodeMutation(!staticAtlasPathLayoutGpuResetPending);
		}
	}

	internal static void InvalidateStaticAtlasPathLayoutCacheForGpuReset()
	{
		StaticAtlasPathLayoutCache.Clear();
		StaticAtlasPathLayoutCacheVersion = -2147483648;
	}

	internal static void ClearStaticAtlasPathLayoutCacheForTests()
	{
		InvalidateStaticAtlasPathLayoutCacheForGpuReset();
	}

	internal bool TryApplyStaticAtlasPathLayoutCacheForTests()
	{
		return TryApplyStaticAtlasPathLayoutCache();
	}

	internal bool CanReuseAppliedStaticAtlasPathLayoutForTests()
	{
		return TryReuseAppliedStaticAtlasPathLayout();
	}

	internal void CreateMediaReplaceAtlasWithoutStaticCacheForTests()
	{
		bool bypassStaticAtlasPathLayoutCacheForTests = _bypassStaticAtlasPathLayoutCacheForTests;
		_bypassStaticAtlasPathLayoutCacheForTests = true;
		try
		{
			CreateMediaReplaceAtlas();
		}
		finally
		{
			_bypassStaticAtlasPathLayoutCacheForTests = bypassStaticAtlasPathLayoutCacheForTests;
		}
	}

	internal bool CorruptStaticAtlasPathLayoutEntryForTests()
	{
		if (!TryBuildStaticAtlasPathLayoutKey(out var key) || !StaticAtlasPathLayoutCache.TryGetValue(key, out var value))
		{
			return false;
		}
		Texture2DArray texture2DArray = new Texture2DArray();
		Rid rid = texture2DArray.GetRid();
		texture2DArray.Dispose();
		StaticAtlasPathLayoutCacheEntry staticAtlasPathLayoutCacheEntry = new StaticAtlasPathLayoutCacheEntry(in key, value.Data, value.Active, value.Paths, value.Rects, value.Pages, texture2DArray, rid, value.TextureArraySize);
		StaticAtlasPathLayoutCache[key] = staticAtlasPathLayoutCacheEntry;
		_appliedStaticAtlasPathLayout = staticAtlasPathLayoutCacheEntry;
		return true;
	}

	private bool TryApplyStaticAtlasPathLayoutCache()
	{
		if (!TryBuildStaticAtlasPathLayoutKey(out var key))
		{
			return false;
		}
		RefreshStaticAtlasPathLayoutCacheVersion(key.CacheVersion);
		if (!StaticAtlasPathLayoutCache.TryGetValue(key, out var value))
		{
			return false;
		}
		if (!VerifyStaticAtlasPathLayoutEntry(value, in key))
		{
			StaticAtlasPathLayoutCache.Remove(key);
			return false;
		}
		ApplyStaticAtlasPathLayoutEntry(value);
		return true;
	}

	private bool TryPublishStaticAtlasPathLayoutCache()
	{
		if (!mediaReplaceAtlasShared || !mediaReplaceAtlasUsesTextureArray || !GodotObject.IsInstanceValid(mediaReplaceAtlasArray) || !TryBuildStaticAtlasPathLayoutKey(out var key))
		{
			return false;
		}
		Rid rid = mediaReplaceAtlasArray.GetRid();
		if (!rid.IsValid)
		{
			return false;
		}
		RefreshStaticAtlasPathLayoutCacheVersion(key.CacheVersion);
		if (StaticAtlasPathLayoutCache.TryGetValue(key, out var value))
		{
			if (VerifyStaticAtlasPathLayoutEntry(value, in key))
			{
				_appliedStaticAtlasPathLayout = value;
				return true;
			}
			StaticAtlasPathLayoutCache.Remove(key);
		}
		bool[] array = new bool[key.EffectiveLimit];
		string[] array2 = new string[key.EffectiveLimit];
		Rect2[] array3 = new Rect2[key.MediaCount];
		int[] array4 = new int[key.MediaCount];
		for (int i = 0; i < key.EffectiveLimit; i++)
		{
			array[i] = _staticAtlasPathSnapshotActive[i];
			array2[i] = _staticAtlasPathSnapshotPaths[i];
		}
		for (int j = 0; j < key.MediaCount; j++)
		{
			array3[j] = mediaReplaceRect[j];
			array4[j] = mediaReplaceAtlasPages[j];
		}
		if (StaticAtlasPathLayoutCache.Count >= 256)
		{
			StaticAtlasPathLayoutCache.Clear();
		}
		StaticAtlasPathLayoutCacheEntry staticAtlasPathLayoutCacheEntry = new StaticAtlasPathLayoutCacheEntry(in key, _flashAnimeData, array, array2, array3, array4, mediaReplaceAtlasArray, rid, mediaReplaceAtlasArraySize);
		StaticAtlasPathLayoutCache.Add(key, staticAtlasPathLayoutCacheEntry);
		_appliedStaticAtlasPathLayout = staticAtlasPathLayoutCacheEntry;
		return true;
	}

	private bool TryBuildStaticAtlasPathLayoutKey(out StaticAtlasPathLayoutKey key)
	{
		key = default;
		if (_staticAtlasPathLayoutRuntimeMutationSeen || _atlasProfileOverride != null || _flashAnimeData == null || !_staticAtlasPathSnapshotValid || _staticAtlasPathSnapshotData != _flashAnimeData || _staticAtlasPathSnapshotAuthoringRevision != _flashAnimeData.AuthoringRevision)
		{
			return false;
		}
		int count = _flashAnimeData.mediaDictionary.Count;
		int num = _mediaReplace?.Count ?? 0;
		int num2 = _mediaReplaceUse?.Count ?? 0;
		int num3 = _mediaReplaceAtlasPaths?.Count ?? 0;
		int num4 = Math.Min(num, num2);
		if (count != _staticAtlasPathSnapshotMediaCount || num != _staticAtlasPathSnapshotTextureCount || num2 != _staticAtlasPathSnapshotUseCount || num3 != _staticAtlasPathSnapshotPathCount || num4 != _staticAtlasPathSnapshotEffectiveLimit)
		{
			return false;
		}
		key = new StaticAtlasPathLayoutKey(AdobeAnimateGlobalAtlasCache.CacheVersion, _staticAtlasPathSnapshotDataInstanceId, _staticAtlasPathSnapshotAuthoringRevision, count, num, num2, num3, num4, _staticAtlasPathSnapshotActiveCount, _staticAtlasPathSnapshotSlotSignature);
		return true;
	}

	private bool VerifyStaticAtlasPathLayoutEntry(StaticAtlasPathLayoutCacheEntry entry, in StaticAtlasPathLayoutKey key)
	{
		if (entry == null || !entry.Key.Equals(key) || entry.Data != _flashAnimeData || entry.Active.Length != key.EffectiveLimit || entry.Paths.Length != key.EffectiveLimit || entry.Rects.Length != key.MediaCount || entry.Pages.Length != key.MediaCount || !GodotObject.IsInstanceValid(entry.TextureArray) || !entry.TextureArrayRid.IsValid || entry.TextureArray.GetRid() != entry.TextureArrayRid)
		{
			return false;
		}
		for (int i = 0; i < key.EffectiveLimit; i++)
		{
			bool flag = _staticAtlasPathSnapshotActive[i];
			string b = _staticAtlasPathSnapshotPaths[i];
			if (entry.Active[i] != flag || !string.Equals(entry.Paths[i], b, StringComparison.Ordinal))
			{
				return false;
			}
		}
		return true;
	}

	private bool TryReuseAppliedStaticAtlasPathLayout()
	{
		StaticAtlasPathLayoutCacheEntry appliedStaticAtlasPathLayout = _appliedStaticAtlasPathLayout;
		if (appliedStaticAtlasPathLayout != null && !_staticAtlasPathLayoutRuntimeMutationSeen && appliedStaticAtlasPathLayout.Data == _flashAnimeData && appliedStaticAtlasPathLayout.Key.CacheVersion == AdobeAnimateGlobalAtlasCache.CacheVersion && appliedStaticAtlasPathLayout.Key.AuthoringRevision == (_flashAnimeData?.AuthoringRevision ?? (-9223372036854775808L)) && GodotObject.IsInstanceValid(appliedStaticAtlasPathLayout.TextureArray) && appliedStaticAtlasPathLayout.TextureArrayRid.IsValid && appliedStaticAtlasPathLayout.TextureArray.GetRid() == appliedStaticAtlasPathLayout.TextureArrayRid && mediaReplaceAtlasArray == appliedStaticAtlasPathLayout.TextureArray && mediaReplaceAtlasUsesTextureArray)
		{
			return mediaReplaceAtlasShared;
		}
		return false;
	}

	private void ApplyStaticAtlasPathLayoutEntry(StaticAtlasPathLayoutCacheEntry entry)
	{
		int mediaCount = entry.Key.MediaCount;
		if (mediaReplaceRect.Count != mediaCount)
		{
			mediaReplaceRect.Clear();
			mediaReplaceRect.Resize(mediaCount);
		}
		if (mediaReplaceAtlasPages.Count != mediaCount)
		{
			mediaReplaceAtlasPages.Clear();
			mediaReplaceAtlasPages.Resize(mediaCount);
		}
		for (int i = 0; i < mediaCount; i++)
		{
			mediaReplaceRect[i] = entry.Rects[i];
			mediaReplaceAtlasPages[i] = entry.Pages[i];
		}
		mediaReplaceAtlas = null;
		mediaReplaceAtlasArray = entry.TextureArray;
		mediaReplaceAtlasArraySize = entry.TextureArraySize;
		mediaReplaceAtlasUsesTextureArray = true;
		mediaReplaceAtlasShared = true;
		_appliedStaticAtlasPathLayout = entry;
	}

	private static void RefreshStaticAtlasPathLayoutCacheVersion(int cacheVersion)
	{
		if (StaticAtlasPathLayoutCacheVersion != cacheVersion)
		{
			StaticAtlasPathLayoutCache.Clear();
			StaticAtlasPathLayoutCacheVersion = cacheVersion;
		}
	}

	internal AdobeAnimateGpuClockState BuildRuntimeGpuClockStateForBareTest(float frameFloat, float animationClockSeconds)
	{
		return BuildRuntimeGpuClockState(frameFloat, animationClockSeconds);
	}

	internal AdobeAnimateGpuClockState BuildGpuGraphClockForBareTest(float frameFloat, float animationClockSeconds, bool enableGpuClock)
	{
		return BuildGpuGraphOwnerStateForRender(SharedGpuClockBareTestDefinition, Transform2D.Identity, Colors.White, frameFloat, enableGpuClock, animationClockSeconds).GpuClock;
	}

	internal void ConfigureGpuClockForBareTest(bool paused, bool reachedClipEnd, double delaySeconds, int clipStart, int clipEndExclusive, double framesPerSecond, double timeScaleValue, bool reverse, bool shouldLoop, bool dispatchActive = true)
	{
		_usingRuntimeManager = true;
		_pause = paused;
		_playbackBlocked = false;
		_parentPlaybackStopped = false;
		_runtimeManagerDispatchActive = dispatchActive;
		clipOver = reachedClipEnd;
		_nextAnimDelayTimer = delaySeconds;
		clipRange = new Vector2I(clipStart, clipEndExclusive);
		frameRate = framesPerSecond;
		timeScale = timeScaleValue;
		playBack = reverse;
		loop = shouldLoop;
	}

	internal void ApplyRuntimeParentStateForBareTest(AdobeAnimateSprite parent)
	{
		ApplyRuntimeParentState(parent);
	}

	internal void ClearRenderSubmissionForBareTest()
	{
		_needsRenderSubmission = false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(374)
		{
			new MethodInfo(MethodName.ResolveCrowdFrameInterpolation, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "frameFloat", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCompositeCacheStaticSignature, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanSkipCrowdRenderFromCachedCulling, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleNextGpuGraphCrowdStateAudit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitAnimeCompleted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MarkEffectOnceBatchEligibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectOnceBatchPlaybackRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsEffectOnceGpuBatchPlaybackPrepared, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resolvedClip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedClipStart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedClipEndExclusive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedPlaybackFrameCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "expectedFrameRate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetEffectOnceGpuBatchElapsedFrame, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AdvanceEffectOnceGpuPlayback, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "suppressionToken", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "expectedClip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedPlaybackRevision", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "expectedElapsedFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedClipStart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedClipEndExclusive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedPlaybackFrameCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "expectedFrameRate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsEffectOnceGpuCallbackStateCurrent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "suppressionToken", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "expectedClip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "expectedPlaybackRevision", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryImportEffectOnceGpuPlaybackPositionWithoutEvents, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "expectedClip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "targetElapsedFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetEffectOnceBatchVisualRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectOnceBatchEligibilityRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateEffectOnceBatchRenderState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkEffectOnceBatchRenderStateDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "invalidateZ", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRenderClipControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearRenderClipControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateGpuRenderGraphForOffsetChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPlaybackBlocked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "blocked", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitPlaybackControlChange, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "wasPlaybackStopped", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetParentPlaybackStopped, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "stopped", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshRuntimeChildPlaybackControl, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectOnceGpuBatchSuppressionToken, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginEffectOnceGpuBatchSuppression, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EndEffectOnceGpuBatchSuppression, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "suppressionToken", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFrozenPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "frozen", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureFrozenPreviewRenderSubmission, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitPlaybackDirectionChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectOnceBatchLayerVisible, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetLayerVisibleForInternalRead, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetLayerVisibleCountForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMediaReplaceForInternalRead, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMediaReplaceUseForInternalRead, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectOnceBatchMediaReplaceUse, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginEffectOnceBatchEligibilityTracking, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EndEffectOnceBatchEligibilityTracking, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectOnceBatchArrayExposureRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkEffectOnceBatchArrayExposure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkRenderStaticLayerArrayExposure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkRenderStaticMediaArrayExposure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ArmRenderStaticArrayAuditAfterExposure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetRenderColorMultiplier, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "multiplier", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanUseGpuHitFlashEnvelope, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartGpuHitFlashEnvelope, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "white", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "startTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "strength", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "duration", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearGpuHitFlashEnvelopes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "clearBright", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "clearWhite", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRenderGrayscale, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRenderGrayscaleEnabled, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetRenderSelfModulate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "selfModulate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyAncestorModulateChangedForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyComposedModulate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestRuntimeRenderSubmissionRetry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkRuntimeRenderSubmissionConsumed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkRuntimeDisplayTicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "version", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WasRuntimeDisplayTicked, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "version", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetRuntimeManagerDispatchActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InvalidateRuntimePauseDispatchCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetPropertyList, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddParentSpriteFollowLayerProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "properties", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "usage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddParentSpriteInsertLayerProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "properties", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "usage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddParentSpriteLayerProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "properties", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "usage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "includeTopOption", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildParentSpriteLayerHintString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "includeTopOption", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveParentSpriteLayerInspectorData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveParentSpriteForLayerInspector, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SealPackedSceneStaticArraysInSubtree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SealPackedSceneStaticArrays, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName.IsParentSpriteFollowLayerProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsParentSpriteInsertLayerProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetParentSpriteFollowLayer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetParentSpriteInsertLayer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanRun, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryDisableProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "prevCanRun", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateVisibilityCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSelfVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetProcessEnabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectVisibilitySignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectVisibilitySignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopRuntimeTickKeepRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshProcessScheduling, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCachedInstanceIdForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConsumeRuntimeGpuPrepareRequiredForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasPreparedGpuRenderGraph, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "generation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPreparedGpuRenderGraphQuadCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "generation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MarkGpuRenderGraphPrepared, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "generation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "signature", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "quadCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasPreparedGpuRenderGraphState, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "generation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "signature", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearPreparedGpuRootCacheForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateGpuRenderGraphPreparation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectOnceBatchRenderTransform, new PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectOnceBatchEffectiveZIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectOnceBatchRenderRevision, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "processFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCrowdFilterDebugReport, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "maxFrameSlices", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrintCrowdFilterDebugReport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "maxFrameSlices", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsLayerVisibleFromArrayForDebug, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLayerNameForDebug, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMediaNameForDebug, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatDebugRect, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatDebugColor, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasExternalVisualsForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasExternalVisualsInOwnedGraphForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasExternalVisualsInOwnedGraphForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "depth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetExternalVisualStateVersionForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetExternalVisualTopologyVersionForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCpuVisualSignatureForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureExternalVisualGpuStateCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetExternalVisualGpuStateCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginExternalVisualPreparationForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MarkExternalVisualPreparedForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "visualIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "preparedForCrowd", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitExternalVisualCrowdFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HideExternalVisualCpuFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreExternalVisualNativeFallbacks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitExternalVisualCrowdFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "depth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HideExternalVisualCpuFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "depth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreExternalVisualNativeFallbacks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "depth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InvalidateExternalVisualTopology, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PositiveModulo, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "modulus", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanReuseRuntimeGpuClockPose, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComputeCanReuseRuntimeGpuClockPose, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequiresDisplayFrameVisualTick, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComputeRequiresDisplayFrameVisualTick, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateRuntimeDisplayVisualRequirementCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequiresDisplayCadenceRuntimeChildUpdate, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestAnimationPoseRedraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanReuseCachedCulledPose, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateRuntimeCrowdCullingForRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateRuntimeCrowdCullingIfCachedOffscreen, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasRuntimeCrowdViewportChanged, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisableRuntimeGpuClockInterpolationForOwnedGraph, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateRuntimeRenderAfterGpuCacheReset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasAnyRequestedMediaReplace, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateChild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitFrameEvents, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameIdx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CacheChildren, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkSlotRuntimeUpdateCacheDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncRuntimeChildState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRuntimeParentState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRuntimeFollowVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetRuntimeFollowVisibilityCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetRuntimeSlotChildren, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AutoInsertChildSprites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetMultimeshModulate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasClip, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateChildPoseImmediate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "loop", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "blendTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "loop", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "blendTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyImmediateAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "clipName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "nextLoop", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "nextBlendTime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetProgress, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetFliter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "layerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFliter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "layerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFliters, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "layerNameList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFliterRecursive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "layerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFlitersRecursive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "layerNameList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyFliterToChildSprites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "sourceKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "layerName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyFlitersToChildSprites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "sourceKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "layerNameList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "open", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMatchingRecursiveFilterSprite, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "sourceKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetRecursiveFilterSourceKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetReplace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "mediaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetAtlasReplace, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "mediaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "textureReference", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "queueUpdate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetAtlasReplacePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "mediaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetReplace, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "mediaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetVerticalClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "upY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "downY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetVerticalClip, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "upY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "downY", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetDiscardUpPos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetDiscardDownPos, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSpriteGroupShaderParameter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.InsertSpriteAtLayer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "childSprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshChildInsertLayerForEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.TrackInsertedSpriteAtLayer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "childSprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UntrackInsertedSpriteForEditor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "childSprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnSpriteChildParentChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "childSprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveInsertedSprite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "childSprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveAllInsertedSprites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExportSpriteSave, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ImportSpriteSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "data", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetCachedRenderFrameEffectiveAncestorModulates, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetCachedEffectiveRenderAncestorModulate, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "renderMountParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.MarkRenderOrderDebugForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasRenderOrderDebugPendingForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateInheritedRenderOrderForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildRenderOrderParentChainDebug, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectiveZIndexForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCachedEffectiveZIndexForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectiveZIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "localZ", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "localZAsRelative", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetEffectiveZIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectiveTreeOrderPathForRender, new PropertyInfo(Variant.Type.PackedInt32Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshTreeOrderPathWatcher, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnTreeOrderHierarchyChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AreTreeOrderPathsEqual, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedInt32Array, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedInt32Array, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InvalidateTreeOrderPathCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateRuntimeTreeOrderForTopologyChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectTreeOrderPathWatchers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildEffectiveTreeOrderPathForRender, new PropertyInfo(Variant.Type.PackedInt32Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "renderMountParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetEffectiveRenderSortBandForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComputeEffectiveRenderSortBand, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetShowBehindParentSortBandBias, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetStaggeredRescanInterval, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "baseInterval", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ComputeLayerVisibilitySignature, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "values", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "valueCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildMediaReplaceSignature, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "textures", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "textureCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "uses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "useCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "rects", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rectCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "limit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "pages", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "pageCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "usesTextureArray", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ComputeMediaReplaceBoundsGrow, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "uses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "useCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "rects", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rectCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "limit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasActiveMediaReplace, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "uses", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "useCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "limit", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMediaReplaceAtlasLayer, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMediaReplaceAtlasLayer, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasValidMediaReplaceAtlas, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMediaReplaceAtlasRid, new PropertyInfo(Variant.Type.Rid, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkLayerStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPartSnapshotCreated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "slot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Array, "layers", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "part", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RequestNodeRedraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "preferDisplayCadence", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "transformOnly", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "staticStateDirty", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateActiveMediaReplaceSnapshotForRender, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateActiveMediaReplaceAtlasPathSnapshotForRender, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetActiveMediaReplacementForRender, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetActiveMediaReplacementAtlasPathForRender, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetMediaSizeForRender, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetManagedPoseTrackCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LerpPoseTransform, new PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Transform2D, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "weight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasInsertedSpritesForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasSpriteChildrenForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NeedsDrawOrderSortBandsForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetNeedsDrawOrderSortBandsForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasManagedSlotSpritesForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSpriteChildrenForRender, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsEmptyHiddenGpuGraphPlaceholderForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareDetachedGpuGraphWarmup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshManagedSlotSpriteOwnerCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkManagedSlotSpriteCacheDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureManagedSlotGpuStateCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetManagedSlotGpuStateCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkManagedSlotVisualStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetManagedSlotGpuStateRescanInterval, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "managedSpriteIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisconnectManagedSlotGpuStateWatchers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshManagedSlotSpriteCacheForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetRenderSortRootForRender, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OwnsSpriteChildForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindParentSpriteAncestor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldFallbackSlotPoseToDrawOrder, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "slotLayerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LayerDictionaryContainsId, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveLayerSortBand, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "parentSortBand", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "drawOrder", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveManagedSpriteLayerSortBand, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "parentSortBand", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "drawOrder", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "characterSortBand", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveChildSortBand, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSpriteChildOwnedBySlot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveSpriteChildRenderLayer, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "childIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isInserted", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "insertedLayerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveSpriteChildInsertLayer, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveTopInsertLayerId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveSpriteChildFollowLayer, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "childIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyChildRenderSort, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearInsertedChildRenderSort, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyKnownGlobalTranslationForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "globalDelta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryConsumeKnownAncestorTranslationNotification, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifySlotOwnedSpriteTransformChangedForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "ownerSlot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyLocalRenderTransformChangedForTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "changedInPhysicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCachedGlobalTransformForRender, new PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsContinuousRenderRootMotion, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Transform2D, "previous", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "current", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshRenderMountCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleNextRenderMountAudit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCachedRenderMountParent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEffectiveRenderModulate, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "renderMountParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCachedRenderFrameModulate, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "canvasItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CanvasItem"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCachedOwnRenderFrameModulate, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCachedOwnRenderFrameSelfModulate, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureCachedRenderLocalModulates, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "transactionVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScheduleNextRenderLocalModulateAudit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshEffectiveRenderModulateAncestorCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "directParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "renderMountParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResetEffectiveRenderModulateAncestorCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsInSubViewportRenderTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldUseGlobalRuntimeManager, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitRuntimeNativeCanvasTakeover, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RecoverRuntimeNativeCanvasAfterMissedTransaction, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreRuntimeNativeCanvasLayer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateRuntimeNativeCanvasSuppressionEligibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanSuppressRuntimeNativeCanvasTree, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsRuntimeNativeCanvasBranchCrowdManaged, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.NeedsDrawItemSortForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequiresInternalDrawItemSortForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveRenderViewport, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Viewport"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "renderMountParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.BeginViewportWorldRectRenderFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EndViewportWorldRectRenderFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ResetCachedViewportWorldRectsForFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCachedViewportWorldRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "viewport", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Viewport"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsValidBounds, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "bounds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MergeBounds, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TransformRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Transform2D, "transform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetRenderFrameFloat, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsClipBlendActiveForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPlaybackClipEndExclusive, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DoesLoopTerminalFrameAliasFirstFrame, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "terminalFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "aliasFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CacheRenderSnapshotState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "frameFloat", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "globalTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "modulate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "renderMountParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "effectiveZIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sortBand", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "useDrawOrderSortBands", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshRenderStaticStateCache, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "force", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenderStaticArrayAuditSnapshotsMatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderStaticMediaAtlasPathAuditSnapshotMatches, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CaptureRenderStaticArrayAuditSnapshots, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldAuditRenderStaticArrays, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequiresRenderStaticArrayAuditForRuntimeManager, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueRenderStaticArrayAuditIfDue, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldAuditRenderStaticMediaArrays, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScheduleNextRenderStaticStateAudit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDataRuntimeLayerCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetLayerDictionaryLayerCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LayerHasSlicesInCurrentClip, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "layerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureLayerActiveCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "layerCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsMediaReplacedCached, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mediaId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InvalidateRenderSnapshotCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "force", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueUpdateMediaReplace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateMediaReplace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateMediaReplaceData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMediaReplaceAtlas, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Draw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsRenderedByParentSpriteForRender, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SubmitRuntimeManagerFirstFrameHandoff, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrySubmitRuntimeRenderFailureHandoff, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseRuntimeManagerFirstFrameHandoff, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseRuntimeManagerFirstFrameHandoffTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseForcedCpuPoseData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunBatchedProcessUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunDisplayFrameVisualUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchProcessUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunDefaultBatchedProcessUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunDefaultBatchedProcessUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "reuseRuntimeGpuClockPose", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunBatchedProcessExtension, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchProcessUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "reuseRuntimeGpuClockPose", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryFastLoopProcessUpdate, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "reuseRuntimeGpuClockPose", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmitFastLoopCallbacks, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "startFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "crossedFrames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "clipStart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "clipEnd", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "playbackRevision", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasRuntimeChildUpdates, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshRuntimeChildUpdatePresence, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldSkipRuntimeChildUpdateForViewport, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldUseGpuGraphVisualAttachment, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RuntimeChildUpdatesAreVisualOnly, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsRuntimeChildUpdateNearViewport, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateInvisibleChild, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FileChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnFlashAnimeDataChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyFlashAnimeDataChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "invalidateDefinitionCache", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DeferFlashAnimeDataChangeUntilReady, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyPendingFlashAnimeDataChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareFlashAnimeDataSerializedOverrides, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasUsableFlashAnimeData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryHydrateFlashAnimeDataBeforeFileChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshTransformNotificationMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshTransformNotificationModeForRenderTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkRenderTransformDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "changedInPhysicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearRetainedRootMotionForPause, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearRetainedRootMotionForStoppedMovement, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanInterpolateRenderRootMotion, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCurrentPhysicsRootMotion, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifyAncestorTransformChangedForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "changedInPhysicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyAncestorTranslatedForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "globalDelta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrimeAncestorTranslationForRender, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateRuntimeRenderMountForAncestorChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateProgressRestorePresentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateStaticAtlasPathLayoutConfigurationSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CaptureStaticAtlasPathLayoutConfigurationSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateStaticAtlasPathLayoutForNodeMutation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "markRuntimeMutation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareStaticAtlasPathLayoutForExplicitUpdate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateStaticAtlasPathLayoutCacheForGpuReset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ClearStaticAtlasPathLayoutCacheForTests, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.TryApplyStaticAtlasPathLayoutCacheForTests, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanReuseAppliedStaticAtlasPathLayoutForTests, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateMediaReplaceAtlasWithoutStaticCacheForTests, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CorruptStaticAtlasPathLayoutEntryForTests, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryApplyStaticAtlasPathLayoutCache, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryPublishStaticAtlasPathLayoutCache, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryReuseAppliedStaticAtlasPathLayout, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshStaticAtlasPathLayoutCacheVersion, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "cacheVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureGpuClockForBareTest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "paused", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "reachedClipEnd", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delaySeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "clipStart", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "clipEndExclusive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "framesPerSecond", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "timeScaleValue", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "reverse", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "shouldLoop", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "dispatchActive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRuntimeParentStateForBareTest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearRenderSubmissionForBareTest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResolveCrowdFrameInterpolation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(ResolveCrowdFrameInterpolation(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCompositeCacheStaticSignature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(BuildCompositeCacheStaticSignature());
			return true;
		}
		if (method == MethodName.CanSkipCrowdRenderFromCachedCulling && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSkipCrowdRenderFromCachedCulling());
			return true;
		}
		if (method == MethodName.ScheduleNextGpuGraphCrowdStateAudit && args.Count == 0)
		{
			ScheduleNextGpuGraphCrowdStateAudit();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitAnimeCompleted && args.Count == 1)
		{
			EmitAnimeCompleted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MarkEffectOnceBatchEligibilityChanged && args.Count == 0)
		{
			MarkEffectOnceBatchEligibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchPlaybackRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetEffectOnceBatchPlaybackRevision());
			return true;
		}
		if (method == MethodName.IsEffectOnceGpuBatchPlaybackPrepared && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEffectOnceGpuBatchPlaybackPrepared(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<float>(in args[4])));
			return true;
		}
		if (method == MethodName.GetEffectOnceGpuBatchElapsedFrame && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<float>(GetEffectOnceGpuBatchElapsedFrame());
			return true;
		}
		if (method == MethodName.AdvanceEffectOnceGpuPlayback && args.Count == 9)
		{
			ret = VariantUtils.CreateFrom<EffectOnceGpuAdvanceResult>(AdvanceEffectOnceGpuPlayback(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<ulong>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertTo<int>(in args[7]), VariantUtils.ConvertTo<float>(in args[8])));
			return true;
		}
		if (method == MethodName.IsEffectOnceGpuCallbackStateCurrent && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEffectOnceGpuCallbackStateCurrent(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<ulong>(in args[2])));
			return true;
		}
		if (method == MethodName.TryImportEffectOnceGpuPlaybackPositionWithoutEvents && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryImportEffectOnceGpuPlaybackPositionWithoutEvents(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchVisualRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetEffectOnceBatchVisualRevision());
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchEligibilityRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetEffectOnceBatchEligibilityRevision());
			return true;
		}
		if (method == MethodName.InvalidateEffectOnceBatchRenderState && args.Count == 0)
		{
			InvalidateEffectOnceBatchRenderState();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkEffectOnceBatchRenderStateDirty && args.Count == 1)
		{
			MarkEffectOnceBatchRenderStateDirty(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRenderClipControl && args.Count == 1)
		{
			SetRenderClipControl(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearRenderClipControl && args.Count == 0)
		{
			ClearRenderClipControl();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateGpuRenderGraphForOffsetChange && args.Count == 0)
		{
			InvalidateGpuRenderGraphForOffsetChange();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPlaybackBlocked && args.Count == 1)
		{
			SetPlaybackBlocked(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitPlaybackControlChange && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CommitPlaybackControlChange(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.SetParentPlaybackStopped && args.Count == 1)
		{
			SetParentPlaybackStopped(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshRuntimeChildPlaybackControl && args.Count == 0)
		{
			RefreshRuntimeChildPlaybackControl();
			ret = default;
			return true;
		}
		if (method == MethodName.GetEffectOnceGpuBatchSuppressionToken && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetEffectOnceGpuBatchSuppressionToken());
			return true;
		}
		if (method == MethodName.BeginEffectOnceGpuBatchSuppression && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(BeginEffectOnceGpuBatchSuppression());
			return true;
		}
		if (method == MethodName.EndEffectOnceGpuBatchSuppression && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(EndEffectOnceGpuBatchSuppression(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.SetFrozenPreview && args.Count == 1)
		{
			SetFrozenPreview(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureFrozenPreviewRenderSubmission && args.Count == 0)
		{
			EnsureFrozenPreviewRenderSubmission();
			ret = default;
			return true;
		}
		if (method == MethodName.CommitPlaybackDirectionChange && args.Count == 0)
		{
			CommitPlaybackDirectionChange();
			ret = default;
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchLayerVisible && args.Count == 0)
		{
			Array<bool> effectOnceBatchLayerVisible = GetEffectOnceBatchLayerVisible();
			ret = VariantUtils.CreateFromArray(effectOnceBatchLayerVisible);
			return true;
		}
		if (method == MethodName.GetLayerVisibleForInternalRead && args.Count == 0)
		{
			Array<bool> layerVisibleForInternalRead = GetLayerVisibleForInternalRead();
			ret = VariantUtils.CreateFromArray(layerVisibleForInternalRead);
			return true;
		}
		if (method == MethodName.GetLayerVisibleCountForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetLayerVisibleCountForRender());
			return true;
		}
		if (method == MethodName.GetMediaReplaceForInternalRead && args.Count == 0)
		{
			Array<Texture2D> mediaReplaceForInternalRead = GetMediaReplaceForInternalRead();
			ret = VariantUtils.CreateFromArray(mediaReplaceForInternalRead);
			return true;
		}
		if (method == MethodName.GetMediaReplaceUseForInternalRead && args.Count == 0)
		{
			Array<bool> mediaReplaceUseForInternalRead = GetMediaReplaceUseForInternalRead();
			ret = VariantUtils.CreateFromArray(mediaReplaceUseForInternalRead);
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchMediaReplaceUse && args.Count == 0)
		{
			Array<bool> effectOnceBatchMediaReplaceUse = GetEffectOnceBatchMediaReplaceUse();
			ret = VariantUtils.CreateFromArray(effectOnceBatchMediaReplaceUse);
			return true;
		}
		if (method == MethodName.BeginEffectOnceBatchEligibilityTracking && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(BeginEffectOnceBatchEligibilityTracking());
			return true;
		}
		if (method == MethodName.EndEffectOnceBatchEligibilityTracking && args.Count == 0)
		{
			EndEffectOnceBatchEligibilityTracking();
			ret = default;
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchArrayExposureRevision && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetEffectOnceBatchArrayExposureRevision());
			return true;
		}
		if (method == MethodName.MarkEffectOnceBatchArrayExposure && args.Count == 0)
		{
			MarkEffectOnceBatchArrayExposure();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkRenderStaticLayerArrayExposure && args.Count == 0)
		{
			MarkRenderStaticLayerArrayExposure();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkRenderStaticMediaArrayExposure && args.Count == 0)
		{
			MarkRenderStaticMediaArrayExposure();
			ret = default;
			return true;
		}
		if (method == MethodName.ArmRenderStaticArrayAuditAfterExposure && args.Count == 0)
		{
			ArmRenderStaticArrayAuditAfterExposure();
			ret = default;
			return true;
		}
		if (method == MethodName.SetRenderColorMultiplier && args.Count == 1)
		{
			SetRenderColorMultiplier(VariantUtils.ConvertTo<Color>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanUseGpuHitFlashEnvelope && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanUseGpuHitFlashEnvelope());
			return true;
		}
		if (method == MethodName.StartGpuHitFlashEnvelope && args.Count == 4)
		{
			StartGpuHitFlashEnvelope(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearGpuHitFlashEnvelopes && args.Count == 2)
		{
			ClearGpuHitFlashEnvelopes(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRenderGrayscale && args.Count == 1)
		{
			SetRenderGrayscale(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsRenderGrayscaleEnabled && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRenderGrayscaleEnabled());
			return true;
		}
		if (method == MethodName.SetRenderSelfModulate && args.Count == 1)
		{
			SetRenderSelfModulate(VariantUtils.ConvertTo<Color>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyAncestorModulateChangedForRender && args.Count == 0)
		{
			NotifyAncestorModulateChangedForRender();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyComposedModulate && args.Count == 0)
		{
			ApplyComposedModulate();
			ret = default;
			return true;
		}
		if (method == MethodName.RequestRuntimeRenderSubmissionRetry && args.Count == 0)
		{
			RequestRuntimeRenderSubmissionRetry();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkRuntimeRenderSubmissionConsumed && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(MarkRuntimeRenderSubmissionConsumed());
			return true;
		}
		if (method == MethodName.MarkRuntimeDisplayTicked && args.Count == 1)
		{
			MarkRuntimeDisplayTicked(VariantUtils.ConvertTo<ulong>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WasRuntimeDisplayTicked && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(WasRuntimeDisplayTicked(VariantUtils.ConvertTo<ulong>(in args[0])));
			return true;
		}
		if (method == MethodName.SetRuntimeManagerDispatchActive && args.Count == 1)
		{
			SetRuntimeManagerDispatchActive(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateRuntimePauseDispatchCache && args.Count == 0)
		{
			InvalidateRuntimePauseDispatchCache();
			ret = default;
			return true;
		}
		if (method == MethodName._GetPropertyList && args.Count == 0)
		{
			Array<Dictionary> array = _GetPropertyList();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.AddParentSpriteFollowLayerProperty && args.Count == 2)
		{
			AddParentSpriteFollowLayerProperty(VariantUtils.ConvertToArray<Dictionary>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddParentSpriteInsertLayerProperty && args.Count == 2)
		{
			AddParentSpriteInsertLayerProperty(VariantUtils.ConvertToArray<Dictionary>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddParentSpriteLayerProperty && args.Count == 4)
		{
			AddParentSpriteLayerProperty(VariantUtils.ConvertToArray<Dictionary>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildParentSpriteLayerHintString && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildParentSpriteLayerHintString(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveParentSpriteLayerInspectorData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateData>(ResolveParentSpriteLayerInspectorData());
			return true;
		}
		if (method == MethodName.ResolveParentSpriteForLayerInspector && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(ResolveParentSpriteForLayerInspector());
			return true;
		}
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SealPackedSceneStaticArraysInSubtree && args.Count == 1)
		{
			SealPackedSceneStaticArraysInSubtree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SealPackedSceneStaticArrays && args.Count == 0)
		{
			SealPackedSceneStaticArrays();
			ret = default;
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
		if (method == MethodName.IsParentSpriteFollowLayerProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsParentSpriteFollowLayerProperty(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.IsParentSpriteInsertLayerProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsParentSpriteInsertLayerProperty(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.SetParentSpriteFollowLayer && args.Count == 1)
		{
			SetParentSpriteFollowLayer(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetParentSpriteInsertLayer && args.Count == 1)
		{
			SetParentSpriteInsertLayer(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanRun && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanRun());
			return true;
		}
		if (method == MethodName.TryDisableProcess && args.Count == 1)
		{
			TryDisableProcess(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateVisibilityCache && args.Count == 0)
		{
			UpdateVisibilityCache();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSelfVisibilityChanged && args.Count == 0)
		{
			OnSelfVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.SetProcessEnabled && args.Count == 1)
		{
			SetProcessEnabled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectVisibilitySignals && args.Count == 0)
		{
			ConnectVisibilitySignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectVisibilitySignals && args.Count == 0)
		{
			DisconnectVisibilitySignals();
			ret = default;
			return true;
		}
		if (method == MethodName.StopRuntimeTickKeepRender && args.Count == 0)
		{
			StopRuntimeTickKeepRender();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshProcessScheduling && args.Count == 0)
		{
			RefreshProcessScheduling();
			ret = default;
			return true;
		}
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCachedInstanceIdForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetCachedInstanceIdForRender());
			return true;
		}
		if (method == MethodName.ConsumeRuntimeGpuPrepareRequiredForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ConsumeRuntimeGpuPrepareRequiredForRender());
			return true;
		}
		if (method == MethodName.HasPreparedGpuRenderGraph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPreparedGpuRenderGraph(VariantUtils.ConvertTo<ulong>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPreparedGpuRenderGraphQuadCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetPreparedGpuRenderGraphQuadCount(VariantUtils.ConvertTo<ulong>(in args[0])));
			return true;
		}
		if (method == MethodName.MarkGpuRenderGraphPrepared && args.Count == 3)
		{
			MarkGpuRenderGraphPrepared(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasPreparedGpuRenderGraphState && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPreparedGpuRenderGraphState(VariantUtils.ConvertTo<ulong>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1])));
			return true;
		}
		if (method == MethodName.ClearPreparedGpuRootCacheForRender && args.Count == 0)
		{
			ClearPreparedGpuRootCacheForRender();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateGpuRenderGraphPreparation && args.Count == 0)
		{
			InvalidateGpuRenderGraphPreparation();
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
		if (method == MethodName.GetEffectOnceBatchRenderTransform && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(GetEffectOnceBatchRenderTransform());
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchEffectiveZIndex && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetEffectOnceBatchEffectiveZIndex());
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchRenderRevision && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetEffectOnceBatchRenderRevision(VariantUtils.ConvertTo<ulong>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCrowdFilterDebugReport && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCrowdFilterDebugReport(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.PrintCrowdFilterDebugReport && args.Count == 2)
		{
			PrintCrowdFilterDebugReport(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsLayerVisibleFromArrayForDebug && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLayerVisibleFromArrayForDebug(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLayerNameForDebug && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetLayerNameForDebug(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMediaNameForDebug && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetMediaNameForDebug(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatDebugRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatDebugRect(VariantUtils.ConvertTo<Rect2>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatDebugColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatDebugColor(VariantUtils.ConvertTo<Color>(in args[0])));
			return true;
		}
		if (method == MethodName.HasExternalVisualsForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasExternalVisualsForRender());
			return true;
		}
		if (method == MethodName.HasExternalVisualsInOwnedGraphForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasExternalVisualsInOwnedGraphForRender());
			return true;
		}
		if (method == MethodName.HasExternalVisualsInOwnedGraphForRender && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasExternalVisualsInOwnedGraphForRender(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetExternalVisualStateVersionForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetExternalVisualStateVersionForRender());
			return true;
		}
		if (method == MethodName.GetExternalVisualTopologyVersionForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetExternalVisualTopologyVersionForRender());
			return true;
		}
		if (method == MethodName.GetCpuVisualSignatureForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetCpuVisualSignatureForRender());
			return true;
		}
		if (method == MethodName.EnsureExternalVisualGpuStateCache && args.Count == 1)
		{
			EnsureExternalVisualGpuStateCache(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetExternalVisualGpuStateCache && args.Count == 0)
		{
			ResetExternalVisualGpuStateCache();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginExternalVisualPreparationForRender && args.Count == 1)
		{
			BeginExternalVisualPreparationForRender(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MarkExternalVisualPreparedForRender && args.Count == 3)
		{
			MarkExternalVisualPreparedForRender(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitExternalVisualCrowdFrame && args.Count == 1)
		{
			CommitExternalVisualCrowdFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HideExternalVisualCpuFrame && args.Count == 1)
		{
			HideExternalVisualCpuFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreExternalVisualNativeFallbacks && args.Count == 1)
		{
			RestoreExternalVisualNativeFallbacks(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitExternalVisualCrowdFrame && args.Count == 2)
		{
			CommitExternalVisualCrowdFrame(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HideExternalVisualCpuFrame && args.Count == 2)
		{
			HideExternalVisualCpuFrame(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreExternalVisualNativeFallbacks && args.Count == 2)
		{
			RestoreExternalVisualNativeFallbacks(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateExternalVisualTopology && args.Count == 0)
		{
			InvalidateExternalVisualTopology();
			ret = default;
			return true;
		}
		if (method == MethodName.PositiveModulo && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(PositiveModulo(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.CanReuseRuntimeGpuClockPose && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanReuseRuntimeGpuClockPose());
			return true;
		}
		if (method == MethodName.ComputeCanReuseRuntimeGpuClockPose && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ComputeCanReuseRuntimeGpuClockPose());
			return true;
		}
		if (method == MethodName.RequiresDisplayFrameVisualTick && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RequiresDisplayFrameVisualTick());
			return true;
		}
		if (method == MethodName.ComputeRequiresDisplayFrameVisualTick && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ComputeRequiresDisplayFrameVisualTick());
			return true;
		}
		if (method == MethodName.InvalidateRuntimeDisplayVisualRequirementCache && args.Count == 0)
		{
			InvalidateRuntimeDisplayVisualRequirementCache();
			ret = default;
			return true;
		}
		if (method == MethodName.RequiresDisplayCadenceRuntimeChildUpdate && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RequiresDisplayCadenceRuntimeChildUpdate());
			return true;
		}
		if (method == MethodName.RequestAnimationPoseRedraw && args.Count == 0)
		{
			RequestAnimationPoseRedraw();
			ret = default;
			return true;
		}
		if (method == MethodName.CanReuseCachedCulledPose && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanReuseCachedCulledPose());
			return true;
		}
		if (method == MethodName.InvalidateRuntimeCrowdCullingForRefresh && args.Count == 0)
		{
			InvalidateRuntimeCrowdCullingForRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateRuntimeCrowdCullingIfCachedOffscreen && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(InvalidateRuntimeCrowdCullingIfCachedOffscreen());
			return true;
		}
		if (method == MethodName.HasRuntimeCrowdViewportChanged && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasRuntimeCrowdViewportChanged());
			return true;
		}
		if (method == MethodName.DisableRuntimeGpuClockInterpolationForOwnedGraph && args.Count == 0)
		{
			DisableRuntimeGpuClockInterpolationForOwnedGraph();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateRuntimeRenderAfterGpuCacheReset && args.Count == 0)
		{
			InvalidateRuntimeRenderAfterGpuCacheReset();
			ret = default;
			return true;
		}
		if (method == MethodName.HasAnyRequestedMediaReplace && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasAnyRequestedMediaReplace());
			return true;
		}
		if (method == MethodName.UpdateChild && args.Count == 0)
		{
			UpdateChild();
			ret = default;
			return true;
		}
		if (method == MethodName.EmitFrameEvents && args.Count == 1)
		{
			EmitFrameEvents(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CacheChildren && args.Count == 0)
		{
			CacheChildren();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkSlotRuntimeUpdateCacheDirty && args.Count == 0)
		{
			MarkSlotRuntimeUpdateCacheDirty();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncRuntimeChildState && args.Count == 1)
		{
			SyncRuntimeChildState(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRuntimeParentState && args.Count == 1)
		{
			ApplyRuntimeParentState(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
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
		if (method == MethodName.GetRuntimeSlotChildren && args.Count == 0)
		{
			AdobeAnimateSlot[] runtimeSlotChildren = GetRuntimeSlotChildren();
			GodotObject[] array2 = runtimeSlotChildren;
			ret = VariantUtils.CreateFromSystemArrayOfGodotObject(array2);
			return true;
		}
		if (method == MethodName.AutoInsertChildSprites && args.Count == 0)
		{
			AutoInsertChildSprites();
			ret = default;
			return true;
		}
		if (method == MethodName.SetMultimeshModulate && args.Count == 1)
		{
			SetMultimeshModulate(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasClip(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateChildPoseImmediate && args.Count == 0)
		{
			UpdateChildPoseImmediate();
			ret = default;
			return true;
		}
		if (method == MethodName.SetClip && args.Count == 1)
		{
			SetClip(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetAnimation && args.Count == 0)
		{
			ResetAnimation();
			ret = default;
			return true;
		}
		if (method == MethodName.SetAnimation && args.Count == 3)
		{
			SetAnimation(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddAnimation && args.Count == 4)
		{
			AddAnimation(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<double>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyImmediateAnimation && args.Count == 3)
		{
			ApplyImmediateAnimation(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetProgress && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetProgress());
			return true;
		}
		if (method == MethodName.SetFliter && args.Count == 2)
		{
			SetFliter(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetFliter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(GetFliter(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.SetFliters && args.Count == 2)
		{
			SetFliters(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetFliterRecursive && args.Count == 2)
		{
			SetFliterRecursive(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetFlitersRecursive && args.Count == 2)
		{
			SetFlitersRecursive(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyFliterToChildSprites && args.Count == 4)
		{
			ApplyFliterToChildSprites(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyFlitersToChildSprites && args.Count == 4)
		{
			ApplyFlitersToChildSprites(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsMatchingRecursiveFilterSprite && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMatchingRecursiveFilterSprite(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetRecursiveFilterSourceKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetRecursiveFilterSourceKey(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.SetReplace && args.Count == 2)
		{
			SetReplace(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<PackedScene>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAtlasReplace && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(SetAtlasReplace(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.GetAtlasReplacePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetAtlasReplacePath(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetReplace && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetReplace(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.SetVerticalClip && args.Count == 3)
		{
			SetVerticalClip(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetVerticalClip && args.Count == 2)
		{
			SetVerticalClip(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDiscardUpPos && args.Count == 1)
		{
			SetDiscardUpPos(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetDiscardDownPos && args.Count == 1)
		{
			SetDiscardDownPos(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSpriteGroupShaderParameter && args.Count == 2)
		{
			SetSpriteGroupShaderParameter(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InsertSpriteAtLayer && args.Count == 2)
		{
			InsertSpriteAtLayer(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshChildInsertLayerForEditor && args.Count == 1)
		{
			RefreshChildInsertLayerForEditor(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TrackInsertedSpriteAtLayer && args.Count == 2)
		{
			TrackInsertedSpriteAtLayer(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UntrackInsertedSpriteForEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(UntrackInsertedSpriteForEditor(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.OnSpriteChildParentChanged && args.Count == 1)
		{
			OnSpriteChildParentChanged(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveInsertedSprite && args.Count == 1)
		{
			RemoveInsertedSprite(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveAllInsertedSprites && args.Count == 0)
		{
			RemoveAllInsertedSprites();
			ret = default;
			return true;
		}
		if (method == MethodName.ExportSpriteSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(ExportSpriteSave());
			return true;
		}
		if (method == MethodName.ImportSpriteSave && args.Count == 1)
		{
			ImportSpriteSave(VariantUtils.ConvertTo<Dictionary>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetCachedRenderFrameEffectiveAncestorModulates && args.Count == 0)
		{
			ResetCachedRenderFrameEffectiveAncestorModulates();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCachedEffectiveRenderAncestorModulate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetCachedEffectiveRenderAncestorModulate(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.MarkRenderOrderDebugForRender && args.Count == 1)
		{
			MarkRenderOrderDebugForRender(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasRenderOrderDebugPendingForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasRenderOrderDebugPendingForRender());
			return true;
		}
		if (method == MethodName.InvalidateInheritedRenderOrderForRender && args.Count == 1)
		{
			InvalidateInheritedRenderOrderForRender(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildRenderOrderParentChainDebug && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildRenderOrderParentChainDebug());
			return true;
		}
		if (method == MethodName.GetEffectiveZIndexForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetEffectiveZIndexForRender());
			return true;
		}
		if (method == MethodName.GetCachedEffectiveZIndexForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCachedEffectiveZIndexForRender());
			return true;
		}
		if (method == MethodName.GetEffectiveZIndex && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetEffectiveZIndex(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetEffectiveZIndex && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetEffectiveZIndex());
			return true;
		}
		if (method == MethodName.GetEffectiveTreeOrderPathForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int[]>(GetEffectiveTreeOrderPathForRender());
			return true;
		}
		if (method == MethodName.RefreshTreeOrderPathWatcher && args.Count == 1)
		{
			RefreshTreeOrderPathWatcher(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTreeOrderHierarchyChanged && args.Count == 0)
		{
			OnTreeOrderHierarchyChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.AreTreeOrderPathsEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AreTreeOrderPathsEqual(VariantUtils.ConvertTo<int[]>(in args[0]), VariantUtils.ConvertTo<int[]>(in args[1])));
			return true;
		}
		if (method == MethodName.InvalidateTreeOrderPathCache && args.Count == 0)
		{
			InvalidateTreeOrderPathCache();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateRuntimeTreeOrderForTopologyChange && args.Count == 0)
		{
			InvalidateRuntimeTreeOrderForTopologyChange();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectTreeOrderPathWatchers && args.Count == 0)
		{
			DisconnectTreeOrderPathWatchers();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildEffectiveTreeOrderPathForRender && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int[]>(BuildEffectiveTreeOrderPathForRender(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.GetEffectiveRenderSortBandForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetEffectiveRenderSortBandForRender());
			return true;
		}
		if (method == MethodName.ComputeEffectiveRenderSortBand && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(ComputeEffectiveRenderSortBand());
			return true;
		}
		if (method == MethodName.GetShowBehindParentSortBandBias && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetShowBehindParentSortBandBias());
			return true;
		}
		if (method == MethodName.GetStaggeredRescanInterval && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetStaggeredRescanInterval(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ComputeLayerVisibilitySignature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<ulong>(ComputeLayerVisibilitySignature(VariantUtils.ConvertToArray<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildMediaReplaceSignature && args.Count == 10)
		{
			ret = VariantUtils.CreateFrom<ulong>(BuildMediaReplaceSignature(VariantUtils.ConvertToArray<Texture2D>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertToArray<bool>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertToArray<Rect2>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertToArray<int>(in args[7]), VariantUtils.ConvertTo<int>(in args[8]), VariantUtils.ConvertTo<bool>(in args[9])));
			return true;
		}
		if (method == MethodName.ComputeMediaReplaceBoundsGrow && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<float>(ComputeMediaReplaceBoundsGrow(VariantUtils.ConvertToArray<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertToArray<Rect2>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4])));
			return true;
		}
		if (method == MethodName.HasActiveMediaReplace && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(HasActiveMediaReplace(VariantUtils.ConvertToArray<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.GetMediaReplaceAtlasLayer && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetMediaReplaceAtlasLayer());
			return true;
		}
		if (method == MethodName.GetMediaReplaceAtlasLayer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetMediaReplaceAtlasLayer(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.HasValidMediaReplaceAtlas && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasValidMediaReplaceAtlas());
			return true;
		}
		if (method == MethodName.GetMediaReplaceAtlasRid && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Rid>(GetMediaReplaceAtlasRid());
			return true;
		}
		if (method == MethodName.MarkLayerStateChanged && args.Count == 0)
		{
			MarkLayerStateChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPartSnapshotCreated && args.Count == 3)
		{
			OnPartSnapshotCreated(VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[0]), VariantUtils.ConvertToArray<StringName>(in args[1]), VariantUtils.ConvertTo<AdobeAnimatePart>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RequestNodeRedraw && args.Count == 3)
		{
			RequestNodeRedraw(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateActiveMediaReplaceSnapshotForRender && args.Count == 0)
		{
			Array<Texture2D> array3 = CreateActiveMediaReplaceSnapshotForRender();
			ret = VariantUtils.CreateFromArray(array3);
			return true;
		}
		if (method == MethodName.CreateActiveMediaReplaceAtlasPathSnapshotForRender && args.Count == 0)
		{
			Array<string> array4 = CreateActiveMediaReplaceAtlasPathSnapshotForRender();
			ret = VariantUtils.CreateFromArray(array4);
			return true;
		}
		if (method == MethodName.GetActiveMediaReplacementForRender && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetActiveMediaReplacementForRender(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetActiveMediaReplacementAtlasPathForRender && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetActiveMediaReplacementAtlasPathForRender(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetMediaSizeForRender && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetMediaSizeForRender(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ResetManagedPoseTrackCache && args.Count == 0)
		{
			ResetManagedPoseTrackCache();
			ret = default;
			return true;
		}
		if (method == MethodName.LerpPoseTransform && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(LerpPoseTransform(VariantUtils.ConvertTo<Transform2D>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.HasInsertedSpritesForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasInsertedSpritesForRender());
			return true;
		}
		if (method == MethodName.HasSpriteChildrenForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSpriteChildrenForRender());
			return true;
		}
		if (method == MethodName.NeedsDrawOrderSortBandsForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(NeedsDrawOrderSortBandsForRender());
			return true;
		}
		if (method == MethodName.GetNeedsDrawOrderSortBandsForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(GetNeedsDrawOrderSortBandsForRender());
			return true;
		}
		if (method == MethodName.HasManagedSlotSpritesForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasManagedSlotSpritesForRender());
			return true;
		}
		if (method == MethodName.GetSpriteChildrenForRender && args.Count == 0)
		{
			AdobeAnimateSprite[] spriteChildrenForRender = GetSpriteChildrenForRender();
			GodotObject[] array2 = spriteChildrenForRender;
			ret = VariantUtils.CreateFromSystemArrayOfGodotObject(array2);
			return true;
		}
		if (method == MethodName.IsEmptyHiddenGpuGraphPlaceholderForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEmptyHiddenGpuGraphPlaceholderForRender());
			return true;
		}
		if (method == MethodName.PrepareDetachedGpuGraphWarmup && args.Count == 0)
		{
			PrepareDetachedGpuGraphWarmup();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshManagedSlotSpriteOwnerCache && args.Count == 0)
		{
			RefreshManagedSlotSpriteOwnerCache();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkManagedSlotSpriteCacheDirty && args.Count == 0)
		{
			MarkManagedSlotSpriteCacheDirty();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureManagedSlotGpuStateCache && args.Count == 1)
		{
			EnsureManagedSlotGpuStateCache(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetManagedSlotGpuStateCache && args.Count == 0)
		{
			ResetManagedSlotGpuStateCache();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkManagedSlotVisualStateChanged && args.Count == 0)
		{
			MarkManagedSlotVisualStateChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.GetManagedSlotGpuStateRescanInterval && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetManagedSlotGpuStateRescanInterval(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.DisconnectManagedSlotGpuStateWatchers && args.Count == 0)
		{
			DisconnectManagedSlotGpuStateWatchers();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshManagedSlotSpriteCacheForRender && args.Count == 0)
		{
			RefreshManagedSlotSpriteCacheForRender();
			ret = default;
			return true;
		}
		if (method == MethodName.GetRenderSortRootForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(GetRenderSortRootForRender());
			return true;
		}
		if (method == MethodName.OwnsSpriteChildForRender && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(OwnsSpriteChildForRender(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.FindParentSpriteAncestor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSprite>(FindParentSpriteAncestor());
			return true;
		}
		if (method == MethodName.ShouldFallbackSlotPoseToDrawOrder && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldFallbackSlotPoseToDrawOrder(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.LayerDictionaryContainsId && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(LayerDictionaryContainsId(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveLayerSortBand && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<long>(ResolveLayerSortBand(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<long>(in args[2])));
			return true;
		}
		if (method == MethodName.ResolveManagedSpriteLayerSortBand && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<long>(ResolveManagedSpriteLayerSortBand(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<long>(in args[2])));
			return true;
		}
		if (method == MethodName.ResolveChildSortBand && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<long>(ResolveChildSortBand(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSpriteChildOwnedBySlot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSpriteChildOwnedBySlot(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveSpriteChildRenderLayer && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveSpriteChildRenderLayer(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		if (method == MethodName.ResolveSpriteChildInsertLayer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveSpriteChildInsertLayer(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveTopInsertLayerId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveTopInsertLayerId());
			return true;
		}
		if (method == MethodName.ResolveSpriteChildFollowLayer && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveSpriteChildFollowLayer(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyChildRenderSort && args.Count == 2)
		{
			ApplyChildRenderSort(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearInsertedChildRenderSort && args.Count == 1)
		{
			ClearInsertedChildRenderSort(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyKnownGlobalTranslationForRender && args.Count == 2)
		{
			ApplyKnownGlobalTranslationForRender(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryConsumeKnownAncestorTranslationNotification && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryConsumeKnownAncestorTranslationNotification());
			return true;
		}
		if (method == MethodName.NotifySlotOwnedSpriteTransformChangedForRender && args.Count == 1)
		{
			NotifySlotOwnedSpriteTransformChangedForRender(VariantUtils.ConvertTo<AdobeAnimateSlot>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyLocalRenderTransformChangedForTree && args.Count == 1)
		{
			NotifyLocalRenderTransformChangedForTree(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCachedGlobalTransformForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(GetCachedGlobalTransformForRender());
			return true;
		}
		if (method == MethodName.IsContinuousRenderRootMotion && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsContinuousRenderRootMotion(VariantUtils.ConvertTo<Transform2D>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1])));
			return true;
		}
		if (method == MethodName.RefreshRenderMountCache && args.Count == 0)
		{
			RefreshRenderMountCache();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleNextRenderMountAudit && args.Count == 0)
		{
			ScheduleNextRenderMountAudit();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCachedRenderMountParent && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(GetCachedRenderMountParent());
			return true;
		}
		if (method == MethodName.GetEffectiveRenderModulate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetEffectiveRenderModulate(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCachedRenderFrameModulate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetCachedRenderFrameModulate(VariantUtils.ConvertTo<CanvasItem>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCachedOwnRenderFrameModulate && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Color>(GetCachedOwnRenderFrameModulate());
			return true;
		}
		if (method == MethodName.GetCachedOwnRenderFrameSelfModulate && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Color>(GetCachedOwnRenderFrameSelfModulate());
			return true;
		}
		if (method == MethodName.EnsureCachedRenderLocalModulates && args.Count == 1)
		{
			EnsureCachedRenderLocalModulates(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleNextRenderLocalModulateAudit && args.Count == 0)
		{
			ScheduleNextRenderLocalModulateAudit();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEffectiveRenderModulateAncestorCache && args.Count == 2)
		{
			RefreshEffectiveRenderModulateAncestorCache(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetEffectiveRenderModulateAncestorCache && args.Count == 0)
		{
			ResetEffectiveRenderModulateAncestorCache();
			ret = default;
			return true;
		}
		if (method == MethodName.IsInSubViewportRenderTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsInSubViewportRenderTarget());
			return true;
		}
		if (method == MethodName.ShouldUseGlobalRuntimeManager && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldUseGlobalRuntimeManager());
			return true;
		}
		if (method == MethodName.CommitRuntimeNativeCanvasTakeover && args.Count == 1)
		{
			CommitRuntimeNativeCanvasTakeover(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RecoverRuntimeNativeCanvasAfterMissedTransaction && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RecoverRuntimeNativeCanvasAfterMissedTransaction(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.RestoreRuntimeNativeCanvasLayer && args.Count == 0)
		{
			RestoreRuntimeNativeCanvasLayer();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateRuntimeNativeCanvasSuppressionEligibility && args.Count == 0)
		{
			InvalidateRuntimeNativeCanvasSuppressionEligibility();
			ret = default;
			return true;
		}
		if (method == MethodName.CanSuppressRuntimeNativeCanvasTree && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanSuppressRuntimeNativeCanvasTree());
			return true;
		}
		if (method == MethodName.IsRuntimeNativeCanvasBranchCrowdManaged && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRuntimeNativeCanvasBranchCrowdManaged(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.NeedsDrawItemSortForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(NeedsDrawItemSortForRender());
			return true;
		}
		if (method == MethodName.RequiresInternalDrawItemSortForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RequiresInternalDrawItemSortForRender());
			return true;
		}
		if (method == MethodName.ResolveRenderViewport && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Viewport>(ResolveRenderViewport(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.BeginViewportWorldRectRenderFrame && args.Count == 1)
		{
			BeginViewportWorldRectRenderFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EndViewportWorldRectRenderFrame && args.Count == 0)
		{
			EndViewportWorldRectRenderFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetCachedViewportWorldRectsForFrame && args.Count == 1)
		{
			ResetCachedViewportWorldRectsForFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCachedViewportWorldRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetCachedViewportWorldRect(VariantUtils.ConvertTo<Viewport>(in args[0])));
			return true;
		}
		if (method == MethodName.IsValidBounds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidBounds(VariantUtils.ConvertTo<Rect2>(in args[0])));
			return true;
		}
		if (method == MethodName.MergeBounds && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2>(MergeBounds(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1])));
			return true;
		}
		if (method == MethodName.TransformRect && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2>(TransformRect(VariantUtils.ConvertTo<Transform2D>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1])));
			return true;
		}
		if (method == MethodName.GetRenderFrameFloat && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<float>(GetRenderFrameFloat());
			return true;
		}
		if (method == MethodName.IsClipBlendActiveForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsClipBlendActiveForRender());
			return true;
		}
		if (method == MethodName.GetPlaybackClipEndExclusive && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPlaybackClipEndExclusive());
			return true;
		}
		if (method == MethodName.DoesLoopTerminalFrameAliasFirstFrame && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(DoesLoopTerminalFrameAliasFirstFrame(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.CacheRenderSnapshotState && args.Count == 7)
		{
			CacheRenderSnapshotState(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]), VariantUtils.ConvertTo<Node>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<long>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshRenderStaticStateCache && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RefreshRenderStaticStateCache(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.RenderStaticArrayAuditSnapshotsMatch && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RenderStaticArrayAuditSnapshotsMatch());
			return true;
		}
		if (method == MethodName.RenderStaticMediaAtlasPathAuditSnapshotMatches && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RenderStaticMediaAtlasPathAuditSnapshotMatches());
			return true;
		}
		if (method == MethodName.CaptureRenderStaticArrayAuditSnapshots && args.Count == 0)
		{
			CaptureRenderStaticArrayAuditSnapshots();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldAuditRenderStaticArrays && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldAuditRenderStaticArrays());
			return true;
		}
		if (method == MethodName.RequiresRenderStaticArrayAuditForRuntimeManager && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RequiresRenderStaticArrayAuditForRuntimeManager());
			return true;
		}
		if (method == MethodName.QueueRenderStaticArrayAuditIfDue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(QueueRenderStaticArrayAuditIfDue(VariantUtils.ConvertTo<ulong>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldAuditRenderStaticMediaArrays && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldAuditRenderStaticMediaArrays());
			return true;
		}
		if (method == MethodName.ScheduleNextRenderStaticStateAudit && args.Count == 1)
		{
			ScheduleNextRenderStaticStateAudit(VariantUtils.ConvertTo<ulong>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDataRuntimeLayerCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetDataRuntimeLayerCount());
			return true;
		}
		if (method == MethodName.GetLayerDictionaryLayerCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetLayerDictionaryLayerCount(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.LayerHasSlicesInCurrentClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(LayerHasSlicesInCurrentClip(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.EnsureLayerActiveCache && args.Count == 2)
		{
			EnsureLayerActiveCache(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsMediaReplacedCached && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMediaReplacedCached(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.InvalidateRenderSnapshotCache && args.Count == 1)
		{
			InvalidateRenderSnapshotCache(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.QueueUpdateMediaReplace && args.Count == 0)
		{
			QueueUpdateMediaReplace();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMediaReplace && args.Count == 0)
		{
			UpdateMediaReplace();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMediaReplaceData && args.Count == 0)
		{
			UpdateMediaReplaceData();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateMediaReplaceAtlas && args.Count == 0)
		{
			CreateMediaReplaceAtlas();
			ret = default;
			return true;
		}
		if (method == MethodName._Draw && args.Count == 0)
		{
			_Draw();
			ret = default;
			return true;
		}
		if (method == MethodName.IsRenderedByParentSpriteForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRenderedByParentSpriteForRender());
			return true;
		}
		if (method == MethodName.SubmitRuntimeManagerFirstFrameHandoff && args.Count == 0)
		{
			SubmitRuntimeManagerFirstFrameHandoff();
			ret = default;
			return true;
		}
		if (method == MethodName.TrySubmitRuntimeRenderFailureHandoff && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TrySubmitRuntimeRenderFailureHandoff());
			return true;
		}
		if (method == MethodName.ReleaseRuntimeManagerFirstFrameHandoff && args.Count == 0)
		{
			ReleaseRuntimeManagerFirstFrameHandoff();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseRuntimeManagerFirstFrameHandoffTree && args.Count == 0)
		{
			ReleaseRuntimeManagerFirstFrameHandoffTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseForcedCpuPoseData && args.Count == 0)
		{
			ReleaseForcedCpuPoseData();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunBatchedProcessUpdate && args.Count == 1)
		{
			RunBatchedProcessUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunDisplayFrameVisualUpdate && args.Count == 1)
		{
			RunDisplayFrameVisualUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchProcessUpdate && args.Count == 1)
		{
			BatchProcessUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunDefaultBatchedProcessUpdate && args.Count == 1)
		{
			RunDefaultBatchedProcessUpdate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunDefaultBatchedProcessUpdate && args.Count == 2)
		{
			RunDefaultBatchedProcessUpdate(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunBatchedProcessExtension && args.Count == 1)
		{
			RunBatchedProcessExtension(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchProcessUpdate && args.Count == 2)
		{
			BatchProcessUpdate(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryFastLoopProcessUpdate && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryFastLoopProcessUpdate(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.EmitFastLoopCallbacks && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<bool>(EmitFastLoopCallbacks(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<ulong>(in args[4])));
			return true;
		}
		if (method == MethodName.HasRuntimeChildUpdates && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasRuntimeChildUpdates());
			return true;
		}
		if (method == MethodName.RefreshRuntimeChildUpdatePresence && args.Count == 0)
		{
			RefreshRuntimeChildUpdatePresence();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldSkipRuntimeChildUpdateForViewport && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipRuntimeChildUpdateForViewport());
			return true;
		}
		if (method == MethodName.ShouldUseGpuGraphVisualAttachment && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldUseGpuGraphVisualAttachment());
			return true;
		}
		if (method == MethodName.RuntimeChildUpdatesAreVisualOnly && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RuntimeChildUpdatesAreVisualOnly());
			return true;
		}
		if (method == MethodName.IsRuntimeChildUpdateNearViewport && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRuntimeChildUpdateNearViewport());
			return true;
		}
		if (method == MethodName.UpdateInvisibleChild && args.Count == 0)
		{
			UpdateInvisibleChild();
			ret = default;
			return true;
		}
		if (method == MethodName.FileChange && args.Count == 0)
		{
			FileChange();
			ret = default;
			return true;
		}
		if (method == MethodName.OnFlashAnimeDataChanged && args.Count == 0)
		{
			OnFlashAnimeDataChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyFlashAnimeDataChange && args.Count == 1)
		{
			ApplyFlashAnimeDataChange(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DeferFlashAnimeDataChangeUntilReady && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(DeferFlashAnimeDataChangeUntilReady());
			return true;
		}
		if (method == MethodName.ApplyPendingFlashAnimeDataChange && args.Count == 0)
		{
			ApplyPendingFlashAnimeDataChange();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareFlashAnimeDataSerializedOverrides && args.Count == 0)
		{
			PrepareFlashAnimeDataSerializedOverrides();
			ret = default;
			return true;
		}
		if (method == MethodName.HasUsableFlashAnimeData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasUsableFlashAnimeData());
			return true;
		}
		if (method == MethodName.TryHydrateFlashAnimeDataBeforeFileChange && args.Count == 0)
		{
			TryHydrateFlashAnimeDataBeforeFileChange();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshTransformNotificationMode && args.Count == 0)
		{
			RefreshTransformNotificationMode();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshTransformNotificationModeForRenderTree && args.Count == 0)
		{
			RefreshTransformNotificationModeForRenderTree();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkRenderTransformDirty && args.Count == 1)
		{
			MarkRenderTransformDirty(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearRetainedRootMotionForPause && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ClearRetainedRootMotionForPause());
			return true;
		}
		if (method == MethodName.ClearRetainedRootMotionForStoppedMovement && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ClearRetainedRootMotionForStoppedMovement());
			return true;
		}
		if (method == MethodName.CanInterpolateRenderRootMotion && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanInterpolateRenderRootMotion());
			return true;
		}
		if (method == MethodName.HasCurrentPhysicsRootMotion && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCurrentPhysicsRootMotion());
			return true;
		}
		if (method == MethodName.NotifyAncestorTransformChangedForRender && args.Count == 1)
		{
			NotifyAncestorTransformChangedForRender(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyAncestorTranslatedForRender && args.Count == 2)
		{
			NotifyAncestorTranslatedForRender(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrimeAncestorTranslationForRender && args.Count == 0)
		{
			PrimeAncestorTranslationForRender();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateRuntimeRenderMountForAncestorChange && args.Count == 0)
		{
			InvalidateRuntimeRenderMountForAncestorChange();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateProgressRestorePresentation && args.Count == 0)
		{
			InvalidateProgressRestorePresentation();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateStaticAtlasPathLayoutConfigurationSnapshot && args.Count == 0)
		{
			InvalidateStaticAtlasPathLayoutConfigurationSnapshot();
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureStaticAtlasPathLayoutConfigurationSnapshot && args.Count == 0)
		{
			CaptureStaticAtlasPathLayoutConfigurationSnapshot();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateStaticAtlasPathLayoutForNodeMutation && args.Count == 1)
		{
			InvalidateStaticAtlasPathLayoutForNodeMutation(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareStaticAtlasPathLayoutForExplicitUpdate && args.Count == 0)
		{
			PrepareStaticAtlasPathLayoutForExplicitUpdate();
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateStaticAtlasPathLayoutCacheForGpuReset && args.Count == 0)
		{
			InvalidateStaticAtlasPathLayoutCacheForGpuReset();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearStaticAtlasPathLayoutCacheForTests && args.Count == 0)
		{
			ClearStaticAtlasPathLayoutCacheForTests();
			ret = default;
			return true;
		}
		if (method == MethodName.TryApplyStaticAtlasPathLayoutCacheForTests && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryApplyStaticAtlasPathLayoutCacheForTests());
			return true;
		}
		if (method == MethodName.CanReuseAppliedStaticAtlasPathLayoutForTests && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanReuseAppliedStaticAtlasPathLayoutForTests());
			return true;
		}
		if (method == MethodName.CreateMediaReplaceAtlasWithoutStaticCacheForTests && args.Count == 0)
		{
			CreateMediaReplaceAtlasWithoutStaticCacheForTests();
			ret = default;
			return true;
		}
		if (method == MethodName.CorruptStaticAtlasPathLayoutEntryForTests && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CorruptStaticAtlasPathLayoutEntryForTests());
			return true;
		}
		if (method == MethodName.TryApplyStaticAtlasPathLayoutCache && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryApplyStaticAtlasPathLayoutCache());
			return true;
		}
		if (method == MethodName.TryPublishStaticAtlasPathLayoutCache && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryPublishStaticAtlasPathLayoutCache());
			return true;
		}
		if (method == MethodName.TryReuseAppliedStaticAtlasPathLayout && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryReuseAppliedStaticAtlasPathLayout());
			return true;
		}
		if (method == MethodName.RefreshStaticAtlasPathLayoutCacheVersion && args.Count == 1)
		{
			RefreshStaticAtlasPathLayoutCacheVersion(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureGpuClockForBareTest && args.Count == 10)
		{
			ConfigureGpuClockForBareTest(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<double>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]), VariantUtils.ConvertTo<double>(in args[5]), VariantUtils.ConvertTo<double>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]), VariantUtils.ConvertTo<bool>(in args[8]), VariantUtils.ConvertTo<bool>(in args[9]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyRuntimeParentStateForBareTest && args.Count == 1)
		{
			ApplyRuntimeParentStateForBareTest(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearRenderSubmissionForBareTest && args.Count == 0)
		{
			ClearRenderSubmissionForBareTest();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.ResolveCrowdFrameInterpolation && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<float>(ResolveCrowdFrameInterpolation(VariantUtils.ConvertTo<float>(in args[0])));
			return true;
		}
		if (method == MethodName.SealPackedSceneStaticArraysInSubtree && args.Count == 1)
		{
			SealPackedSceneStaticArraysInSubtree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsParentSpriteFollowLayerProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsParentSpriteFollowLayerProperty(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.IsParentSpriteInsertLayerProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsParentSpriteInsertLayerProperty(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatDebugRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatDebugRect(VariantUtils.ConvertTo<Rect2>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatDebugColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatDebugColor(VariantUtils.ConvertTo<Color>(in args[0])));
			return true;
		}
		if (method == MethodName.PositiveModulo && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(PositiveModulo(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<float>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyFliterToChildSprites && args.Count == 4)
		{
			ApplyFliterToChildSprites(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyFlitersToChildSprites && args.Count == 4)
		{
			ApplyFlitersToChildSprites(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Godot.Collections.Array>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsMatchingRecursiveFilterSprite && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsMatchingRecursiveFilterSprite(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetRecursiveFilterSourceKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetRecursiveFilterSourceKey(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.ResetCachedRenderFrameEffectiveAncestorModulates && args.Count == 0)
		{
			ResetCachedRenderFrameEffectiveAncestorModulates();
			ret = default;
			return true;
		}
		if (method == MethodName.AreTreeOrderPathsEqual && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AreTreeOrderPathsEqual(VariantUtils.ConvertTo<int[]>(in args[0]), VariantUtils.ConvertTo<int[]>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildEffectiveTreeOrderPathForRender && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int[]>(BuildEffectiveTreeOrderPathForRender(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.ComputeLayerVisibilitySignature && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<ulong>(ComputeLayerVisibilitySignature(VariantUtils.ConvertToArray<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildMediaReplaceSignature && args.Count == 10)
		{
			ret = VariantUtils.CreateFrom<ulong>(BuildMediaReplaceSignature(VariantUtils.ConvertToArray<Texture2D>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertToArray<bool>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertToArray<Rect2>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<int>(in args[6]), VariantUtils.ConvertToArray<int>(in args[7]), VariantUtils.ConvertTo<int>(in args[8]), VariantUtils.ConvertTo<bool>(in args[9])));
			return true;
		}
		if (method == MethodName.ComputeMediaReplaceBoundsGrow && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<float>(ComputeMediaReplaceBoundsGrow(VariantUtils.ConvertToArray<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertToArray<Rect2>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4])));
			return true;
		}
		if (method == MethodName.HasActiveMediaReplace && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(HasActiveMediaReplace(VariantUtils.ConvertToArray<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.LerpPoseTransform && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(LerpPoseTransform(VariantUtils.ConvertTo<Transform2D>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.ResolveLayerSortBand && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<long>(ResolveLayerSortBand(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<long>(in args[2])));
			return true;
		}
		if (method == MethodName.ResolveManagedSpriteLayerSortBand && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<long>(ResolveManagedSpriteLayerSortBand(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<long>(in args[2])));
			return true;
		}
		if (method == MethodName.ClearInsertedChildRenderSort && args.Count == 1)
		{
			ClearInsertedChildRenderSort(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsContinuousRenderRootMotion && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsContinuousRenderRootMotion(VariantUtils.ConvertTo<Transform2D>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCachedRenderFrameModulate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetCachedRenderFrameModulate(VariantUtils.ConvertTo<CanvasItem>(in args[0])));
			return true;
		}
		if (method == MethodName.IsRuntimeNativeCanvasBranchCrowdManaged && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRuntimeNativeCanvasBranchCrowdManaged(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.BeginViewportWorldRectRenderFrame && args.Count == 1)
		{
			BeginViewportWorldRectRenderFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EndViewportWorldRectRenderFrame && args.Count == 0)
		{
			EndViewportWorldRectRenderFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetCachedViewportWorldRectsForFrame && args.Count == 1)
		{
			ResetCachedViewportWorldRectsForFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCachedViewportWorldRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetCachedViewportWorldRect(VariantUtils.ConvertTo<Viewport>(in args[0])));
			return true;
		}
		if (method == MethodName.IsValidBounds && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidBounds(VariantUtils.ConvertTo<Rect2>(in args[0])));
			return true;
		}
		if (method == MethodName.MergeBounds && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2>(MergeBounds(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1])));
			return true;
		}
		if (method == MethodName.TransformRect && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2>(TransformRect(VariantUtils.ConvertTo<Transform2D>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1])));
			return true;
		}
		if (method == MethodName.GetLayerDictionaryLayerCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetLayerDictionaryLayerCount(VariantUtils.ConvertTo<Dictionary>(in args[0])));
			return true;
		}
		if (method == MethodName.InvalidateStaticAtlasPathLayoutCacheForGpuReset && args.Count == 0)
		{
			InvalidateStaticAtlasPathLayoutCacheForGpuReset();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearStaticAtlasPathLayoutCacheForTests && args.Count == 0)
		{
			ClearStaticAtlasPathLayoutCacheForTests();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshStaticAtlasPathLayoutCacheVersion && args.Count == 1)
		{
			RefreshStaticAtlasPathLayoutCacheVersion(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.ResolveCrowdFrameInterpolation)
		{
			return true;
		}
		if (method == MethodName.BuildCompositeCacheStaticSignature)
		{
			return true;
		}
		if (method == MethodName.CanSkipCrowdRenderFromCachedCulling)
		{
			return true;
		}
		if (method == MethodName.ScheduleNextGpuGraphCrowdStateAudit)
		{
			return true;
		}
		if (method == MethodName.EmitAnimeCompleted)
		{
			return true;
		}
		if (method == MethodName.MarkEffectOnceBatchEligibilityChanged)
		{
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchPlaybackRevision)
		{
			return true;
		}
		if (method == MethodName.IsEffectOnceGpuBatchPlaybackPrepared)
		{
			return true;
		}
		if (method == MethodName.GetEffectOnceGpuBatchElapsedFrame)
		{
			return true;
		}
		if (method == MethodName.AdvanceEffectOnceGpuPlayback)
		{
			return true;
		}
		if (method == MethodName.IsEffectOnceGpuCallbackStateCurrent)
		{
			return true;
		}
		if (method == MethodName.TryImportEffectOnceGpuPlaybackPositionWithoutEvents)
		{
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchVisualRevision)
		{
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchEligibilityRevision)
		{
			return true;
		}
		if (method == MethodName.InvalidateEffectOnceBatchRenderState)
		{
			return true;
		}
		if (method == MethodName.MarkEffectOnceBatchRenderStateDirty)
		{
			return true;
		}
		if (method == MethodName.SetRenderClipControl)
		{
			return true;
		}
		if (method == MethodName.ClearRenderClipControl)
		{
			return true;
		}
		if (method == MethodName.InvalidateGpuRenderGraphForOffsetChange)
		{
			return true;
		}
		if (method == MethodName.SetPlaybackBlocked)
		{
			return true;
		}
		if (method == MethodName.CommitPlaybackControlChange)
		{
			return true;
		}
		if (method == MethodName.SetParentPlaybackStopped)
		{
			return true;
		}
		if (method == MethodName.RefreshRuntimeChildPlaybackControl)
		{
			return true;
		}
		if (method == MethodName.GetEffectOnceGpuBatchSuppressionToken)
		{
			return true;
		}
		if (method == MethodName.BeginEffectOnceGpuBatchSuppression)
		{
			return true;
		}
		if (method == MethodName.EndEffectOnceGpuBatchSuppression)
		{
			return true;
		}
		if (method == MethodName.SetFrozenPreview)
		{
			return true;
		}
		if (method == MethodName.EnsureFrozenPreviewRenderSubmission)
		{
			return true;
		}
		if (method == MethodName.CommitPlaybackDirectionChange)
		{
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchLayerVisible)
		{
			return true;
		}
		if (method == MethodName.GetLayerVisibleForInternalRead)
		{
			return true;
		}
		if (method == MethodName.GetLayerVisibleCountForRender)
		{
			return true;
		}
		if (method == MethodName.GetMediaReplaceForInternalRead)
		{
			return true;
		}
		if (method == MethodName.GetMediaReplaceUseForInternalRead)
		{
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchMediaReplaceUse)
		{
			return true;
		}
		if (method == MethodName.BeginEffectOnceBatchEligibilityTracking)
		{
			return true;
		}
		if (method == MethodName.EndEffectOnceBatchEligibilityTracking)
		{
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchArrayExposureRevision)
		{
			return true;
		}
		if (method == MethodName.MarkEffectOnceBatchArrayExposure)
		{
			return true;
		}
		if (method == MethodName.MarkRenderStaticLayerArrayExposure)
		{
			return true;
		}
		if (method == MethodName.MarkRenderStaticMediaArrayExposure)
		{
			return true;
		}
		if (method == MethodName.ArmRenderStaticArrayAuditAfterExposure)
		{
			return true;
		}
		if (method == MethodName.SetRenderColorMultiplier)
		{
			return true;
		}
		if (method == MethodName.CanUseGpuHitFlashEnvelope)
		{
			return true;
		}
		if (method == MethodName.StartGpuHitFlashEnvelope)
		{
			return true;
		}
		if (method == MethodName.ClearGpuHitFlashEnvelopes)
		{
			return true;
		}
		if (method == MethodName.SetRenderGrayscale)
		{
			return true;
		}
		if (method == MethodName.IsRenderGrayscaleEnabled)
		{
			return true;
		}
		if (method == MethodName.SetRenderSelfModulate)
		{
			return true;
		}
		if (method == MethodName.NotifyAncestorModulateChangedForRender)
		{
			return true;
		}
		if (method == MethodName.ApplyComposedModulate)
		{
			return true;
		}
		if (method == MethodName.RequestRuntimeRenderSubmissionRetry)
		{
			return true;
		}
		if (method == MethodName.MarkRuntimeRenderSubmissionConsumed)
		{
			return true;
		}
		if (method == MethodName.MarkRuntimeDisplayTicked)
		{
			return true;
		}
		if (method == MethodName.WasRuntimeDisplayTicked)
		{
			return true;
		}
		if (method == MethodName.SetRuntimeManagerDispatchActive)
		{
			return true;
		}
		if (method == MethodName.InvalidateRuntimePauseDispatchCache)
		{
			return true;
		}
		if (method == MethodName._GetPropertyList)
		{
			return true;
		}
		if (method == MethodName.AddParentSpriteFollowLayerProperty)
		{
			return true;
		}
		if (method == MethodName.AddParentSpriteInsertLayerProperty)
		{
			return true;
		}
		if (method == MethodName.AddParentSpriteLayerProperty)
		{
			return true;
		}
		if (method == MethodName.BuildParentSpriteLayerHintString)
		{
			return true;
		}
		if (method == MethodName.ResolveParentSpriteLayerInspectorData)
		{
			return true;
		}
		if (method == MethodName.ResolveParentSpriteForLayerInspector)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.SealPackedSceneStaticArraysInSubtree)
		{
			return true;
		}
		if (method == MethodName.SealPackedSceneStaticArrays)
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
		if (method == MethodName.IsParentSpriteFollowLayerProperty)
		{
			return true;
		}
		if (method == MethodName.IsParentSpriteInsertLayerProperty)
		{
			return true;
		}
		if (method == MethodName.SetParentSpriteFollowLayer)
		{
			return true;
		}
		if (method == MethodName.SetParentSpriteInsertLayer)
		{
			return true;
		}
		if (method == MethodName.CanRun)
		{
			return true;
		}
		if (method == MethodName.TryDisableProcess)
		{
			return true;
		}
		if (method == MethodName.UpdateVisibilityCache)
		{
			return true;
		}
		if (method == MethodName.OnSelfVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.SetProcessEnabled)
		{
			return true;
		}
		if (method == MethodName.ConnectVisibilitySignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectVisibilitySignals)
		{
			return true;
		}
		if (method == MethodName.StopRuntimeTickKeepRender)
		{
			return true;
		}
		if (method == MethodName.RefreshProcessScheduling)
		{
			return true;
		}
		if (method == MethodName._EnterTree)
		{
			return true;
		}
		if (method == MethodName.GetCachedInstanceIdForRender)
		{
			return true;
		}
		if (method == MethodName.ConsumeRuntimeGpuPrepareRequiredForRender)
		{
			return true;
		}
		if (method == MethodName.HasPreparedGpuRenderGraph)
		{
			return true;
		}
		if (method == MethodName.GetPreparedGpuRenderGraphQuadCount)
		{
			return true;
		}
		if (method == MethodName.MarkGpuRenderGraphPrepared)
		{
			return true;
		}
		if (method == MethodName.HasPreparedGpuRenderGraphState)
		{
			return true;
		}
		if (method == MethodName.ClearPreparedGpuRootCacheForRender)
		{
			return true;
		}
		if (method == MethodName.InvalidateGpuRenderGraphPreparation)
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
		if (method == MethodName.GetEffectOnceBatchRenderTransform)
		{
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchEffectiveZIndex)
		{
			return true;
		}
		if (method == MethodName.GetEffectOnceBatchRenderRevision)
		{
			return true;
		}
		if (method == MethodName.BuildCrowdFilterDebugReport)
		{
			return true;
		}
		if (method == MethodName.PrintCrowdFilterDebugReport)
		{
			return true;
		}
		if (method == MethodName.IsLayerVisibleFromArrayForDebug)
		{
			return true;
		}
		if (method == MethodName.GetLayerNameForDebug)
		{
			return true;
		}
		if (method == MethodName.GetMediaNameForDebug)
		{
			return true;
		}
		if (method == MethodName.FormatDebugRect)
		{
			return true;
		}
		if (method == MethodName.FormatDebugColor)
		{
			return true;
		}
		if (method == MethodName.HasExternalVisualsForRender)
		{
			return true;
		}
		if (method == MethodName.HasExternalVisualsInOwnedGraphForRender)
		{
			return true;
		}
		if (method == MethodName.GetExternalVisualStateVersionForRender)
		{
			return true;
		}
		if (method == MethodName.GetExternalVisualTopologyVersionForRender)
		{
			return true;
		}
		if (method == MethodName.GetCpuVisualSignatureForRender)
		{
			return true;
		}
		if (method == MethodName.EnsureExternalVisualGpuStateCache)
		{
			return true;
		}
		if (method == MethodName.ResetExternalVisualGpuStateCache)
		{
			return true;
		}
		if (method == MethodName.BeginExternalVisualPreparationForRender)
		{
			return true;
		}
		if (method == MethodName.MarkExternalVisualPreparedForRender)
		{
			return true;
		}
		if (method == MethodName.CommitExternalVisualCrowdFrame)
		{
			return true;
		}
		if (method == MethodName.HideExternalVisualCpuFrame)
		{
			return true;
		}
		if (method == MethodName.RestoreExternalVisualNativeFallbacks)
		{
			return true;
		}
		if (method == MethodName.InvalidateExternalVisualTopology)
		{
			return true;
		}
		if (method == MethodName.PositiveModulo)
		{
			return true;
		}
		if (method == MethodName.CanReuseRuntimeGpuClockPose)
		{
			return true;
		}
		if (method == MethodName.ComputeCanReuseRuntimeGpuClockPose)
		{
			return true;
		}
		if (method == MethodName.RequiresDisplayFrameVisualTick)
		{
			return true;
		}
		if (method == MethodName.ComputeRequiresDisplayFrameVisualTick)
		{
			return true;
		}
		if (method == MethodName.InvalidateRuntimeDisplayVisualRequirementCache)
		{
			return true;
		}
		if (method == MethodName.RequiresDisplayCadenceRuntimeChildUpdate)
		{
			return true;
		}
		if (method == MethodName.RequestAnimationPoseRedraw)
		{
			return true;
		}
		if (method == MethodName.CanReuseCachedCulledPose)
		{
			return true;
		}
		if (method == MethodName.InvalidateRuntimeCrowdCullingForRefresh)
		{
			return true;
		}
		if (method == MethodName.InvalidateRuntimeCrowdCullingIfCachedOffscreen)
		{
			return true;
		}
		if (method == MethodName.HasRuntimeCrowdViewportChanged)
		{
			return true;
		}
		if (method == MethodName.DisableRuntimeGpuClockInterpolationForOwnedGraph)
		{
			return true;
		}
		if (method == MethodName.InvalidateRuntimeRenderAfterGpuCacheReset)
		{
			return true;
		}
		if (method == MethodName.HasAnyRequestedMediaReplace)
		{
			return true;
		}
		if (method == MethodName.UpdateChild)
		{
			return true;
		}
		if (method == MethodName.EmitFrameEvents)
		{
			return true;
		}
		if (method == MethodName.CacheChildren)
		{
			return true;
		}
		if (method == MethodName.MarkSlotRuntimeUpdateCacheDirty)
		{
			return true;
		}
		if (method == MethodName.SyncRuntimeChildState)
		{
			return true;
		}
		if (method == MethodName.ApplyRuntimeParentState)
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
		if (method == MethodName.GetRuntimeSlotChildren)
		{
			return true;
		}
		if (method == MethodName.AutoInsertChildSprites)
		{
			return true;
		}
		if (method == MethodName.SetMultimeshModulate)
		{
			return true;
		}
		if (method == MethodName.HasClip)
		{
			return true;
		}
		if (method == MethodName.UpdateChildPoseImmediate)
		{
			return true;
		}
		if (method == MethodName.SetClip)
		{
			return true;
		}
		if (method == MethodName.ResetAnimation)
		{
			return true;
		}
		if (method == MethodName.SetAnimation)
		{
			return true;
		}
		if (method == MethodName.AddAnimation)
		{
			return true;
		}
		if (method == MethodName.ApplyImmediateAnimation)
		{
			return true;
		}
		if (method == MethodName.GetProgress)
		{
			return true;
		}
		if (method == MethodName.SetFliter)
		{
			return true;
		}
		if (method == MethodName.GetFliter)
		{
			return true;
		}
		if (method == MethodName.SetFliters)
		{
			return true;
		}
		if (method == MethodName.SetFliterRecursive)
		{
			return true;
		}
		if (method == MethodName.SetFlitersRecursive)
		{
			return true;
		}
		if (method == MethodName.ApplyFliterToChildSprites)
		{
			return true;
		}
		if (method == MethodName.ApplyFlitersToChildSprites)
		{
			return true;
		}
		if (method == MethodName.IsMatchingRecursiveFilterSprite)
		{
			return true;
		}
		if (method == MethodName.GetRecursiveFilterSourceKey)
		{
			return true;
		}
		if (method == MethodName.SetReplace)
		{
			return true;
		}
		if (method == MethodName.SetAtlasReplace)
		{
			return true;
		}
		if (method == MethodName.GetAtlasReplacePath)
		{
			return true;
		}
		if (method == MethodName.GetReplace)
		{
			return true;
		}
		if (method == MethodName.SetVerticalClip)
		{
			return true;
		}
		if (method == MethodName.SetDiscardUpPos)
		{
			return true;
		}
		if (method == MethodName.SetDiscardDownPos)
		{
			return true;
		}
		if (method == MethodName.SetSpriteGroupShaderParameter)
		{
			return true;
		}
		if (method == MethodName.InsertSpriteAtLayer)
		{
			return true;
		}
		if (method == MethodName.RefreshChildInsertLayerForEditor)
		{
			return true;
		}
		if (method == MethodName.TrackInsertedSpriteAtLayer)
		{
			return true;
		}
		if (method == MethodName.UntrackInsertedSpriteForEditor)
		{
			return true;
		}
		if (method == MethodName.OnSpriteChildParentChanged)
		{
			return true;
		}
		if (method == MethodName.RemoveInsertedSprite)
		{
			return true;
		}
		if (method == MethodName.RemoveAllInsertedSprites)
		{
			return true;
		}
		if (method == MethodName.ExportSpriteSave)
		{
			return true;
		}
		if (method == MethodName.ImportSpriteSave)
		{
			return true;
		}
		if (method == MethodName.ResetCachedRenderFrameEffectiveAncestorModulates)
		{
			return true;
		}
		if (method == MethodName.GetCachedEffectiveRenderAncestorModulate)
		{
			return true;
		}
		if (method == MethodName.MarkRenderOrderDebugForRender)
		{
			return true;
		}
		if (method == MethodName.HasRenderOrderDebugPendingForRender)
		{
			return true;
		}
		if (method == MethodName.InvalidateInheritedRenderOrderForRender)
		{
			return true;
		}
		if (method == MethodName.BuildRenderOrderParentChainDebug)
		{
			return true;
		}
		if (method == MethodName.GetEffectiveZIndexForRender)
		{
			return true;
		}
		if (method == MethodName.GetCachedEffectiveZIndexForRender)
		{
			return true;
		}
		if (method == MethodName.GetEffectiveZIndex)
		{
			return true;
		}
		if (method == MethodName.GetEffectiveTreeOrderPathForRender)
		{
			return true;
		}
		if (method == MethodName.RefreshTreeOrderPathWatcher)
		{
			return true;
		}
		if (method == MethodName.OnTreeOrderHierarchyChanged)
		{
			return true;
		}
		if (method == MethodName.AreTreeOrderPathsEqual)
		{
			return true;
		}
		if (method == MethodName.InvalidateTreeOrderPathCache)
		{
			return true;
		}
		if (method == MethodName.InvalidateRuntimeTreeOrderForTopologyChange)
		{
			return true;
		}
		if (method == MethodName.DisconnectTreeOrderPathWatchers)
		{
			return true;
		}
		if (method == MethodName.BuildEffectiveTreeOrderPathForRender)
		{
			return true;
		}
		if (method == MethodName.GetEffectiveRenderSortBandForRender)
		{
			return true;
		}
		if (method == MethodName.ComputeEffectiveRenderSortBand)
		{
			return true;
		}
		if (method == MethodName.GetShowBehindParentSortBandBias)
		{
			return true;
		}
		if (method == MethodName.GetStaggeredRescanInterval)
		{
			return true;
		}
		if (method == MethodName.ComputeLayerVisibilitySignature)
		{
			return true;
		}
		if (method == MethodName.BuildMediaReplaceSignature)
		{
			return true;
		}
		if (method == MethodName.ComputeMediaReplaceBoundsGrow)
		{
			return true;
		}
		if (method == MethodName.HasActiveMediaReplace)
		{
			return true;
		}
		if (method == MethodName.GetMediaReplaceAtlasLayer)
		{
			return true;
		}
		if (method == MethodName.HasValidMediaReplaceAtlas)
		{
			return true;
		}
		if (method == MethodName.GetMediaReplaceAtlasRid)
		{
			return true;
		}
		if (method == MethodName.MarkLayerStateChanged)
		{
			return true;
		}
		if (method == MethodName.OnPartSnapshotCreated)
		{
			return true;
		}
		if (method == MethodName.RequestNodeRedraw)
		{
			return true;
		}
		if (method == MethodName.CreateActiveMediaReplaceSnapshotForRender)
		{
			return true;
		}
		if (method == MethodName.CreateActiveMediaReplaceAtlasPathSnapshotForRender)
		{
			return true;
		}
		if (method == MethodName.GetActiveMediaReplacementForRender)
		{
			return true;
		}
		if (method == MethodName.GetActiveMediaReplacementAtlasPathForRender)
		{
			return true;
		}
		if (method == MethodName.GetMediaSizeForRender)
		{
			return true;
		}
		if (method == MethodName.ResetManagedPoseTrackCache)
		{
			return true;
		}
		if (method == MethodName.LerpPoseTransform)
		{
			return true;
		}
		if (method == MethodName.HasInsertedSpritesForRender)
		{
			return true;
		}
		if (method == MethodName.HasSpriteChildrenForRender)
		{
			return true;
		}
		if (method == MethodName.NeedsDrawOrderSortBandsForRender)
		{
			return true;
		}
		if (method == MethodName.GetNeedsDrawOrderSortBandsForRender)
		{
			return true;
		}
		if (method == MethodName.HasManagedSlotSpritesForRender)
		{
			return true;
		}
		if (method == MethodName.GetSpriteChildrenForRender)
		{
			return true;
		}
		if (method == MethodName.IsEmptyHiddenGpuGraphPlaceholderForRender)
		{
			return true;
		}
		if (method == MethodName.PrepareDetachedGpuGraphWarmup)
		{
			return true;
		}
		if (method == MethodName.RefreshManagedSlotSpriteOwnerCache)
		{
			return true;
		}
		if (method == MethodName.MarkManagedSlotSpriteCacheDirty)
		{
			return true;
		}
		if (method == MethodName.EnsureManagedSlotGpuStateCache)
		{
			return true;
		}
		if (method == MethodName.ResetManagedSlotGpuStateCache)
		{
			return true;
		}
		if (method == MethodName.MarkManagedSlotVisualStateChanged)
		{
			return true;
		}
		if (method == MethodName.GetManagedSlotGpuStateRescanInterval)
		{
			return true;
		}
		if (method == MethodName.DisconnectManagedSlotGpuStateWatchers)
		{
			return true;
		}
		if (method == MethodName.RefreshManagedSlotSpriteCacheForRender)
		{
			return true;
		}
		if (method == MethodName.GetRenderSortRootForRender)
		{
			return true;
		}
		if (method == MethodName.OwnsSpriteChildForRender)
		{
			return true;
		}
		if (method == MethodName.FindParentSpriteAncestor)
		{
			return true;
		}
		if (method == MethodName.ShouldFallbackSlotPoseToDrawOrder)
		{
			return true;
		}
		if (method == MethodName.LayerDictionaryContainsId)
		{
			return true;
		}
		if (method == MethodName.ResolveLayerSortBand)
		{
			return true;
		}
		if (method == MethodName.ResolveManagedSpriteLayerSortBand)
		{
			return true;
		}
		if (method == MethodName.ResolveChildSortBand)
		{
			return true;
		}
		if (method == MethodName.IsSpriteChildOwnedBySlot)
		{
			return true;
		}
		if (method == MethodName.ResolveSpriteChildRenderLayer)
		{
			return true;
		}
		if (method == MethodName.ResolveSpriteChildInsertLayer)
		{
			return true;
		}
		if (method == MethodName.ResolveTopInsertLayerId)
		{
			return true;
		}
		if (method == MethodName.ResolveSpriteChildFollowLayer)
		{
			return true;
		}
		if (method == MethodName.ApplyChildRenderSort)
		{
			return true;
		}
		if (method == MethodName.ClearInsertedChildRenderSort)
		{
			return true;
		}
		if (method == MethodName.ApplyKnownGlobalTranslationForRender)
		{
			return true;
		}
		if (method == MethodName.TryConsumeKnownAncestorTranslationNotification)
		{
			return true;
		}
		if (method == MethodName.NotifySlotOwnedSpriteTransformChangedForRender)
		{
			return true;
		}
		if (method == MethodName.NotifyLocalRenderTransformChangedForTree)
		{
			return true;
		}
		if (method == MethodName.GetCachedGlobalTransformForRender)
		{
			return true;
		}
		if (method == MethodName.IsContinuousRenderRootMotion)
		{
			return true;
		}
		if (method == MethodName.RefreshRenderMountCache)
		{
			return true;
		}
		if (method == MethodName.ScheduleNextRenderMountAudit)
		{
			return true;
		}
		if (method == MethodName.GetCachedRenderMountParent)
		{
			return true;
		}
		if (method == MethodName.GetEffectiveRenderModulate)
		{
			return true;
		}
		if (method == MethodName.GetCachedRenderFrameModulate)
		{
			return true;
		}
		if (method == MethodName.GetCachedOwnRenderFrameModulate)
		{
			return true;
		}
		if (method == MethodName.GetCachedOwnRenderFrameSelfModulate)
		{
			return true;
		}
		if (method == MethodName.EnsureCachedRenderLocalModulates)
		{
			return true;
		}
		if (method == MethodName.ScheduleNextRenderLocalModulateAudit)
		{
			return true;
		}
		if (method == MethodName.RefreshEffectiveRenderModulateAncestorCache)
		{
			return true;
		}
		if (method == MethodName.ResetEffectiveRenderModulateAncestorCache)
		{
			return true;
		}
		if (method == MethodName.IsInSubViewportRenderTarget)
		{
			return true;
		}
		if (method == MethodName.ShouldUseGlobalRuntimeManager)
		{
			return true;
		}
		if (method == MethodName.CommitRuntimeNativeCanvasTakeover)
		{
			return true;
		}
		if (method == MethodName.RecoverRuntimeNativeCanvasAfterMissedTransaction)
		{
			return true;
		}
		if (method == MethodName.RestoreRuntimeNativeCanvasLayer)
		{
			return true;
		}
		if (method == MethodName.InvalidateRuntimeNativeCanvasSuppressionEligibility)
		{
			return true;
		}
		if (method == MethodName.CanSuppressRuntimeNativeCanvasTree)
		{
			return true;
		}
		if (method == MethodName.IsRuntimeNativeCanvasBranchCrowdManaged)
		{
			return true;
		}
		if (method == MethodName.NeedsDrawItemSortForRender)
		{
			return true;
		}
		if (method == MethodName.RequiresInternalDrawItemSortForRender)
		{
			return true;
		}
		if (method == MethodName.ResolveRenderViewport)
		{
			return true;
		}
		if (method == MethodName.BeginViewportWorldRectRenderFrame)
		{
			return true;
		}
		if (method == MethodName.EndViewportWorldRectRenderFrame)
		{
			return true;
		}
		if (method == MethodName.ResetCachedViewportWorldRectsForFrame)
		{
			return true;
		}
		if (method == MethodName.GetCachedViewportWorldRect)
		{
			return true;
		}
		if (method == MethodName.IsValidBounds)
		{
			return true;
		}
		if (method == MethodName.MergeBounds)
		{
			return true;
		}
		if (method == MethodName.TransformRect)
		{
			return true;
		}
		if (method == MethodName.GetRenderFrameFloat)
		{
			return true;
		}
		if (method == MethodName.IsClipBlendActiveForRender)
		{
			return true;
		}
		if (method == MethodName.GetPlaybackClipEndExclusive)
		{
			return true;
		}
		if (method == MethodName.DoesLoopTerminalFrameAliasFirstFrame)
		{
			return true;
		}
		if (method == MethodName.CacheRenderSnapshotState)
		{
			return true;
		}
		if (method == MethodName.RefreshRenderStaticStateCache)
		{
			return true;
		}
		if (method == MethodName.RenderStaticArrayAuditSnapshotsMatch)
		{
			return true;
		}
		if (method == MethodName.RenderStaticMediaAtlasPathAuditSnapshotMatches)
		{
			return true;
		}
		if (method == MethodName.CaptureRenderStaticArrayAuditSnapshots)
		{
			return true;
		}
		if (method == MethodName.ShouldAuditRenderStaticArrays)
		{
			return true;
		}
		if (method == MethodName.RequiresRenderStaticArrayAuditForRuntimeManager)
		{
			return true;
		}
		if (method == MethodName.QueueRenderStaticArrayAuditIfDue)
		{
			return true;
		}
		if (method == MethodName.ShouldAuditRenderStaticMediaArrays)
		{
			return true;
		}
		if (method == MethodName.ScheduleNextRenderStaticStateAudit)
		{
			return true;
		}
		if (method == MethodName.GetDataRuntimeLayerCount)
		{
			return true;
		}
		if (method == MethodName.GetLayerDictionaryLayerCount)
		{
			return true;
		}
		if (method == MethodName.LayerHasSlicesInCurrentClip)
		{
			return true;
		}
		if (method == MethodName.EnsureLayerActiveCache)
		{
			return true;
		}
		if (method == MethodName.IsMediaReplacedCached)
		{
			return true;
		}
		if (method == MethodName.InvalidateRenderSnapshotCache)
		{
			return true;
		}
		if (method == MethodName.QueueUpdateMediaReplace)
		{
			return true;
		}
		if (method == MethodName.UpdateMediaReplace)
		{
			return true;
		}
		if (method == MethodName.UpdateMediaReplaceData)
		{
			return true;
		}
		if (method == MethodName.CreateMediaReplaceAtlas)
		{
			return true;
		}
		if (method == MethodName._Draw)
		{
			return true;
		}
		if (method == MethodName.IsRenderedByParentSpriteForRender)
		{
			return true;
		}
		if (method == MethodName.SubmitRuntimeManagerFirstFrameHandoff)
		{
			return true;
		}
		if (method == MethodName.TrySubmitRuntimeRenderFailureHandoff)
		{
			return true;
		}
		if (method == MethodName.ReleaseRuntimeManagerFirstFrameHandoff)
		{
			return true;
		}
		if (method == MethodName.ReleaseRuntimeManagerFirstFrameHandoffTree)
		{
			return true;
		}
		if (method == MethodName.ReleaseForcedCpuPoseData)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.RunBatchedProcessUpdate)
		{
			return true;
		}
		if (method == MethodName.RunDisplayFrameVisualUpdate)
		{
			return true;
		}
		if (method == MethodName.BatchProcessUpdate)
		{
			return true;
		}
		if (method == MethodName.RunDefaultBatchedProcessUpdate)
		{
			return true;
		}
		if (method == MethodName.RunBatchedProcessExtension)
		{
			return true;
		}
		if (method == MethodName.TryFastLoopProcessUpdate)
		{
			return true;
		}
		if (method == MethodName.EmitFastLoopCallbacks)
		{
			return true;
		}
		if (method == MethodName.HasRuntimeChildUpdates)
		{
			return true;
		}
		if (method == MethodName.RefreshRuntimeChildUpdatePresence)
		{
			return true;
		}
		if (method == MethodName.ShouldSkipRuntimeChildUpdateForViewport)
		{
			return true;
		}
		if (method == MethodName.ShouldUseGpuGraphVisualAttachment)
		{
			return true;
		}
		if (method == MethodName.RuntimeChildUpdatesAreVisualOnly)
		{
			return true;
		}
		if (method == MethodName.IsRuntimeChildUpdateNearViewport)
		{
			return true;
		}
		if (method == MethodName.UpdateInvisibleChild)
		{
			return true;
		}
		if (method == MethodName.FileChange)
		{
			return true;
		}
		if (method == MethodName.OnFlashAnimeDataChanged)
		{
			return true;
		}
		if (method == MethodName.ApplyFlashAnimeDataChange)
		{
			return true;
		}
		if (method == MethodName.DeferFlashAnimeDataChangeUntilReady)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingFlashAnimeDataChange)
		{
			return true;
		}
		if (method == MethodName.PrepareFlashAnimeDataSerializedOverrides)
		{
			return true;
		}
		if (method == MethodName.HasUsableFlashAnimeData)
		{
			return true;
		}
		if (method == MethodName.TryHydrateFlashAnimeDataBeforeFileChange)
		{
			return true;
		}
		if (method == MethodName.RefreshTransformNotificationMode)
		{
			return true;
		}
		if (method == MethodName.RefreshTransformNotificationModeForRenderTree)
		{
			return true;
		}
		if (method == MethodName.MarkRenderTransformDirty)
		{
			return true;
		}
		if (method == MethodName.ClearRetainedRootMotionForPause)
		{
			return true;
		}
		if (method == MethodName.ClearRetainedRootMotionForStoppedMovement)
		{
			return true;
		}
		if (method == MethodName.CanInterpolateRenderRootMotion)
		{
			return true;
		}
		if (method == MethodName.HasCurrentPhysicsRootMotion)
		{
			return true;
		}
		if (method == MethodName.NotifyAncestorTransformChangedForRender)
		{
			return true;
		}
		if (method == MethodName.NotifyAncestorTranslatedForRender)
		{
			return true;
		}
		if (method == MethodName.PrimeAncestorTranslationForRender)
		{
			return true;
		}
		if (method == MethodName.InvalidateRuntimeRenderMountForAncestorChange)
		{
			return true;
		}
		if (method == MethodName.InvalidateProgressRestorePresentation)
		{
			return true;
		}
		if (method == MethodName.InvalidateStaticAtlasPathLayoutConfigurationSnapshot)
		{
			return true;
		}
		if (method == MethodName.CaptureStaticAtlasPathLayoutConfigurationSnapshot)
		{
			return true;
		}
		if (method == MethodName.InvalidateStaticAtlasPathLayoutForNodeMutation)
		{
			return true;
		}
		if (method == MethodName.PrepareStaticAtlasPathLayoutForExplicitUpdate)
		{
			return true;
		}
		if (method == MethodName.InvalidateStaticAtlasPathLayoutCacheForGpuReset)
		{
			return true;
		}
		if (method == MethodName.ClearStaticAtlasPathLayoutCacheForTests)
		{
			return true;
		}
		if (method == MethodName.TryApplyStaticAtlasPathLayoutCacheForTests)
		{
			return true;
		}
		if (method == MethodName.CanReuseAppliedStaticAtlasPathLayoutForTests)
		{
			return true;
		}
		if (method == MethodName.CreateMediaReplaceAtlasWithoutStaticCacheForTests)
		{
			return true;
		}
		if (method == MethodName.CorruptStaticAtlasPathLayoutEntryForTests)
		{
			return true;
		}
		if (method == MethodName.TryApplyStaticAtlasPathLayoutCache)
		{
			return true;
		}
		if (method == MethodName.TryPublishStaticAtlasPathLayoutCache)
		{
			return true;
		}
		if (method == MethodName.TryReuseAppliedStaticAtlasPathLayout)
		{
			return true;
		}
		if (method == MethodName.RefreshStaticAtlasPathLayoutCacheVersion)
		{
			return true;
		}
		if (method == MethodName.ConfigureGpuClockForBareTest)
		{
			return true;
		}
		if (method == MethodName.ApplyRuntimeParentStateForBareTest)
		{
			return true;
		}
		if (method == MethodName.ClearRenderSubmissionForBareTest)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Modulate)
		{
			Modulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.SelfModulate)
		{
			SelfModulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName.ZIndex)
		{
			ZIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ZAsRelative)
		{
			ZAsRelative = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.atlasProfileOverride)
		{
			atlasProfileOverride = VariantUtils.ConvertTo<AdobeAnimateAtlasProfile>(in value);
			return true;
		}
		if (name == PropertyName.flashAnimeData)
		{
			flashAnimeData = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName.preview)
		{
			preview = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.forceLocalRender)
		{
			forceLocalRender = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.forceCpuPoseRender)
		{
			forceCpuPoseRender = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.suppressEditorRenderSubmission)
		{
			suppressEditorRenderSubmission = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.invisible)
		{
			invisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.offset)
		{
			offset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			timeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.deduplicateLoopTerminalFrame)
		{
			deduplicateLoopTerminalFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.pause)
		{
			pause = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.runtimeViewportCullingEnabled)
		{
			runtimeViewportCullingEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.playBack)
		{
			playBack = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.clip)
		{
			clip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.layerVisible)
		{
			layerVisible = VariantUtils.ConvertToArray<bool>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplace)
		{
			mediaReplace = VariantUtils.ConvertToArray<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplaceUse)
		{
			mediaReplaceUse = VariantUtils.ConvertToArray<bool>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasPaths)
		{
			mediaReplaceAtlasPaths = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.parentSprite)
		{
			parentSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName.meshColor)
		{
			meshColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceBatchVisualRevision)
		{
			_effectOnceBatchVisualRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceBatchEligibilityRevision)
		{
			_effectOnceBatchEligibilityRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._flashAnimeData)
		{
			_flashAnimeData = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._flashAnimeDataChangePending)
		{
			_flashAnimeDataChangePending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._atlasProfileOverride)
		{
			_atlasProfileOverride = VariantUtils.ConvertTo<AdobeAnimateAtlasProfile>(in value);
			return true;
		}
		if (name == PropertyName._preview)
		{
			_preview = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._forceLocalRender)
		{
			_forceLocalRender = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderClipControl)
		{
			_renderClipControl = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._forceCpuPoseRender)
		{
			_forceCpuPoseRender = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._invisible)
		{
			_invisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.normalAlpha)
		{
			normalAlpha = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._offset)
		{
			_offset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.offsetRotate)
		{
			offsetRotate = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._timeScale)
		{
			_timeScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.trueFrameRate)
		{
			trueFrameRate = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.refreshEveryFlame)
		{
			refreshEveryFlame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.skipLastFrame)
		{
			skipLastFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._deduplicateLoopTerminalFrame)
		{
			_deduplicateLoopTerminalFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.usePos)
		{
			usePos = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useRotate)
		{
			useRotate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.useFollowVisible)
		{
			useFollowVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.blendTimeInit)
		{
			blendTimeInit = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._pause)
		{
			_pause = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._playBack)
		{
			_playBack = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._playbackBlocked)
		{
			_playbackBlocked = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._parentPlaybackStopped)
		{
			_parentPlaybackStopped = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._frozenPreview)
		{
			_frozenPreview = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceGpuSuppressed)
		{
			_effectOnceGpuSuppressed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceGpuSuppressionToken)
		{
			_effectOnceGpuSuppressionToken = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._applyingImmediateAnimation)
		{
			_applyingImmediateAnimation = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.keepRenderSubmittedWhenPaused)
		{
			keepRenderSubmittedWhenPaused = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeViewportCullingEnabled)
		{
			_runtimeViewportCullingEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.onlyDraw)
		{
			onlyDraw = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.elapsedTimer)
		{
			elapsedTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.refreshTimer)
		{
			refreshTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.blend)
		{
			blend = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.blendTime)
		{
			blendTime = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.blendTimer)
		{
			blendTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._blendFromFrameFloat)
		{
			_blendFromFrameFloat = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.frameIndex)
		{
			frameIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.frameRate)
		{
			frameRate = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.refreshEveryFrame)
		{
			refreshEveryFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.loop)
		{
			loop = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._clip)
		{
			_clip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._playbackRevision)
		{
			_playbackRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._managedPoseRevision)
		{
			_managedPoseRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.clipRange)
		{
			clipRange = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.clipOver)
		{
			clipOver = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._layerVisible)
		{
			_layerVisible = VariantUtils.ConvertToArray<bool>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceBatchArrayExposureRevision)
		{
			_effectOnceBatchArrayExposureRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceBatchEligibilityTracking)
		{
			_effectOnceBatchEligibilityTracking = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlas)
		{
			mediaReplaceAtlas = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasArray)
		{
			mediaReplaceAtlasArray = VariantUtils.ConvertTo<TextureLayered>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasArraySize)
		{
			mediaReplaceAtlasArraySize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasUsesTextureArray)
		{
			mediaReplaceAtlasUsesTextureArray = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplaceRect)
		{
			mediaReplaceRect = VariantUtils.ConvertToArray<Rect2>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasPages)
		{
			mediaReplaceAtlasPages = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName._mediaReplace)
		{
			_mediaReplace = VariantUtils.ConvertToArray<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._mediaReplaceUse)
		{
			_mediaReplaceUse = VariantUtils.ConvertToArray<bool>(in value);
			return true;
		}
		if (name == PropertyName._mediaReplaceAtlasPaths)
		{
			_mediaReplaceAtlasPaths = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasShared)
		{
			mediaReplaceAtlasShared = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.canvasItem)
		{
			canvasItem = VariantUtils.ConvertTo<Rid>(in value);
			return true;
		}
		if (name == PropertyName.meshTexture)
		{
			meshTexture = VariantUtils.ConvertTo<Rid>(in value);
			return true;
		}
		if (name == PropertyName.mesh)
		{
			mesh = VariantUtils.ConvertTo<ArrayMesh>(in value);
			return true;
		}
		if (name == PropertyName.needMediaReplaceUpdate)
		{
			needMediaReplaceUpdate = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._canRun)
		{
			_canRun = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.initClip)
		{
			initClip = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._parentSprite)
		{
			_parentSprite = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._parentSpriteResolved)
		{
			_parentSpriteResolved = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.followParentSpriteLayerId)
		{
			followParentSpriteLayerId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.insertLayerId)
		{
			insertLayerId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._meshColor)
		{
			_meshColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._renderColorMultiplier)
		{
			_renderColorMultiplier = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._runtimeGpuGraphActive)
		{
			_runtimeGpuGraphActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasCachedGpuGraphCrowdState)
		{
			_hasCachedGpuGraphCrowdState = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedGpuGraphCrowdStatePhysicsFrame)
		{
			_cachedGpuGraphCrowdStatePhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._hasCachedRasterCompositeRenderState)
		{
			_hasCachedRasterCompositeRenderState = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphCrowdStaticStateDirty)
		{
			_gpuGraphCrowdStaticStateDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphCrowdPresentationStateDirty)
		{
			_gpuGraphCrowdPresentationStateDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphCrowdStateNextAuditPhysicsFrame)
		{
			_gpuGraphCrowdStateNextAuditPhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._renderGrayscale)
		{
			_renderGrayscale = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._autoInserting)
		{
			_autoInserting = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._slotChildren)
		{
			_slotChildren = VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._runtimeSlotChildren)
		{
			_runtimeSlotChildren = VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._managedSlotGpuStateAtlasRids)
		{
			_managedSlotGpuStateAtlasRids = VariantUtils.ConvertTo<Rid[]>(in value);
			return true;
		}
		if (name == PropertyName._managedSlotGpuStateRescanCountdowns)
		{
			_managedSlotGpuStateRescanCountdowns = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName._managedSlotGpuStateWatchedSprites)
		{
			_managedSlotGpuStateWatchedSprites = VariantUtils.ConvertToSystemArrayOfGodotObject<Sprite2D>(in value);
			return true;
		}
		if (name == PropertyName._managedSlotGpuStateWatchedParts)
		{
			_managedSlotGpuStateWatchedParts = VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimatePart>(in value);
			return true;
		}
		if (name == PropertyName._runtimeSlotChildrenDirty)
		{
			_runtimeSlotChildrenDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeHasChildUpdates)
		{
			_runtimeHasChildUpdates = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeChildUpdatesVisualOnlyDirty)
		{
			_runtimeChildUpdatesVisualOnlyDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeChildUpdatesVisualOnlyCached)
		{
			_runtimeChildUpdatesVisualOnlyCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._managedSlotSpritesDirty)
		{
			_managedSlotSpritesDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._spriteChildren)
		{
			_spriteChildren = VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._spriteChildOwnerSlots)
		{
			_spriteChildOwnerSlots = VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._hasChildren)
		{
			_hasChildren = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeChildViewportFrame)
		{
			_runtimeChildViewportFrame = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._runtimeChildViewportVisible)
		{
			_runtimeChildViewportVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._lastUpdateFrame)
		{
			_lastUpdateFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._isVisibleInTree)
		{
			_isVisibleInTree = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeInsideTree)
		{
			_runtimeInsideTree = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._processEnabled)
		{
			_processEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._usingRuntimeManager)
		{
			_usingRuntimeManager = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedInstanceIdForRender)
		{
			_cachedInstanceIdForRender = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphPreparedGeneration)
		{
			_gpuGraphPreparedGeneration = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphPreparedSignature)
		{
			_gpuGraphPreparedSignature = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphPreparedQuadCount)
		{
			_gpuGraphPreparedQuadCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._dispatchingBatchedProcess)
		{
			_dispatchingBatchedProcess = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._displayFrameVisualDispatchActive)
		{
			_displayFrameVisualDispatchActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeDisplayTickVersion)
		{
			_runtimeDisplayTickVersion = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._runtimeReadyCached)
		{
			_runtimeReadyCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeNativeCanvasSuppressed)
		{
			_runtimeNativeCanvasSuppressed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeNativeCanvasPublishedFrameVersion)
		{
			_runtimeNativeCanvasPublishedFrameVersion = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._runtimeManagerFirstFrameHandoffActive)
		{
			_runtimeManagerFirstFrameHandoffActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeNativeCanvasVisibilityLayer)
		{
			_runtimeNativeCanvasVisibilityLayer = VariantUtils.ConvertTo<uint>(in value);
			return true;
		}
		if (name == PropertyName._runtimeNativeCanvasSuppressionEligibilityKnown)
		{
			_runtimeNativeCanvasSuppressionEligibilityKnown = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeNativeCanvasSuppressionEligible)
		{
			_runtimeNativeCanvasSuppressionEligible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeParentStateSource)
		{
			_runtimeParentStateSource = VariantUtils.ConvertTo<AdobeAnimateSprite>(in value);
			return true;
		}
		if (name == PropertyName._runtimeParentStateValidationCountdown)
		{
			_runtimeParentStateValidationCountdown = VariantUtils.ConvertTo<int>(in value);
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
		if (name == PropertyName._needsRenderSubmission)
		{
			_needsRenderSubmission = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeRenderSubmissionRetryRequested)
		{
			_runtimeRenderSubmissionRetryRequested = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeGpuPrepareRequired)
		{
			_runtimeGpuPrepareRequired = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._visibilityParentCanvas)
		{
			_visibilityParentCanvas = VariantUtils.ConvertTo<CanvasItem>(in value);
			return true;
		}
		if (name == PropertyName._selfVisibilityChangedConnected)
		{
			_selfVisibilityChangedConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._parentVisibilityChangedConnected)
		{
			_parentVisibilityChangedConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRenderMountParent)
		{
			_cachedRenderMountParent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._cachedViewport)
		{
			_cachedViewport = VariantUtils.ConvertTo<Viewport>(in value);
			return true;
		}
		if (name == PropertyName._renderMountNextAuditPhysicsFrame)
		{
			_renderMountNextAuditPhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._renderModulateMountParent)
		{
			_renderModulateMountParent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._renderModulateAncestorCacheReady)
		{
			_renderModulateAncestorCacheReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderModulateAncestorRescanCountdown)
		{
			_renderModulateAncestorRescanCountdown = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedCanvasLayer)
		{
			_cachedCanvasLayer = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedManagedPoseTrackKey)
		{
			_cachedManagedPoseTrackKey = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedManagedPoseTrackUseLayerId)
		{
			_cachedManagedPoseTrackUseLayerId = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._nextAnimDelayTimer)
		{
			_nextAnimDelayTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._renderStateCached)
		{
			_renderStateCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderStaticStateCached)
		{
			_renderStaticStateCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderStaticLayerArrayEscaped)
		{
			_renderStaticLayerArrayEscaped = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderStaticMediaArraysEscaped)
		{
			_renderStaticMediaArraysEscaped = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderStaticArrayAuditSubmissionPending)
		{
			_renderStaticArrayAuditSubmissionPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderStaticStateNextAuditPhysicsFrame)
		{
			_renderStaticStateNextAuditPhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._renderStaticAuditLayerVisibleSnapshot)
		{
			_renderStaticAuditLayerVisibleSnapshot = VariantUtils.ConvertToArray<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderStaticAuditMediaReplaceUseSnapshot)
		{
			_renderStaticAuditMediaReplaceUseSnapshot = VariantUtils.ConvertToArray<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderStaticAuditMediaReplaceSnapshot)
		{
			_renderStaticAuditMediaReplaceSnapshot = VariantUtils.ConvertToArray<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._renderStaticAuditMediaReplaceRectSnapshot)
		{
			_renderStaticAuditMediaReplaceRectSnapshot = VariantUtils.ConvertToArray<Rect2>(in value);
			return true;
		}
		if (name == PropertyName._renderStaticAuditMediaReplacePageSnapshot)
		{
			_renderStaticAuditMediaReplacePageSnapshot = VariantUtils.ConvertToArray<int>(in value);
			return true;
		}
		if (name == PropertyName._renderStaticAuditMediaReplaceAtlasPathSnapshot)
		{
			_renderStaticAuditMediaReplaceAtlasPathSnapshot = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName._cachedGpuGraphRootOwnerPlaybackRevision)
		{
			_cachedGpuGraphRootOwnerPlaybackRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._hasCachedGpuGraphNestedOwnerState)
		{
			_hasCachedGpuGraphNestedOwnerState = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedGpuGraphNestedOwnerPlaybackRevision)
		{
			_cachedGpuGraphNestedOwnerPlaybackRevision = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphNestedOwnerStateRebaseClockSeconds)
		{
			_gpuGraphNestedOwnerStateRebaseClockSeconds = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._cachedClip)
		{
			_cachedClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._cachedClipRange)
		{
			_cachedClipRange = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._cachedFrameFloat)
		{
			_cachedFrameFloat = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._cachedGlobalTransform)
		{
			_cachedGlobalTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._renderGlobalTransform)
		{
			_renderGlobalTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._renderPreviousGlobalTransform)
		{
			_renderPreviousGlobalTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._renderGlobalTransformCached)
		{
			_renderGlobalTransformCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderGlobalTransformDirty)
		{
			_renderGlobalTransformDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderTransformChangedInPhysicsFrame)
		{
			_renderTransformChangedInPhysicsFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimePhysicsAncestorTransformPending)
		{
			_runtimePhysicsAncestorTransformPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._knownAncestorTranslationNotificationPending)
		{
			_knownAncestorTranslationNotificationPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderRootMotionEnabled)
		{
			_renderRootMotionEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderTreePaused)
		{
			_renderTreePaused = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderGlobalTransformPhysicsFrame)
		{
			_renderGlobalTransformPhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._cachedModulate)
		{
			_cachedModulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._cachedOffset)
		{
			_cachedOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlas)
		{
			_cachedMediaReplaceAtlas = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasArray)
		{
			_cachedMediaReplaceAtlasArray = VariantUtils.ConvertTo<TextureLayered>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasRid)
		{
			_cachedMediaReplaceAtlasRid = VariantUtils.ConvertTo<Rid>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasArrayRid)
		{
			_cachedMediaReplaceAtlasArrayRid = VariantUtils.ConvertTo<Rid>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasShared)
		{
			_cachedMediaReplaceAtlasShared = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasUsesTextureArray)
		{
			_cachedMediaReplaceAtlasUsesTextureArray = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRenderMountParentSubmit)
		{
			_cachedRenderMountParentSubmit = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._cachedCanvasLayerSubmit)
		{
			_cachedCanvasLayerSubmit = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedZIndex)
		{
			_cachedZIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedEffectiveZIndex)
		{
			_cachedEffectiveZIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._effectiveZIndexCached)
		{
			_effectiveZIndexCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._effectiveZIndexCacheLocalZ)
		{
			_effectiveZIndexCacheLocalZ = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._effectiveZIndexCacheZAsRelative)
		{
			_effectiveZIndexCacheZAsRelative = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._effectiveZIndexRescanCountdown)
		{
			_effectiveZIndexRescanCountdown = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceBatchRenderTransform)
		{
			_effectOnceBatchRenderTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceBatchEffectiveZIndex)
		{
			_effectOnceBatchEffectiveZIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceBatchRenderRevision)
		{
			_effectOnceBatchRenderRevision = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceBatchObservedLocalZIndex)
		{
			_effectOnceBatchObservedLocalZIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceBatchObservedZAsRelative)
		{
			_effectOnceBatchObservedZAsRelative = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceBatchRenderNextAuditFrame)
		{
			_effectOnceBatchRenderNextAuditFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._effectOnceBatchRenderStateDirty)
		{
			_effectOnceBatchRenderStateDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedEffectiveRenderSortBand)
		{
			_cachedEffectiveRenderSortBand = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._effectiveRenderSortBandCached)
		{
			_effectiveRenderSortBandCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._effectiveRenderSortBandCacheLocalSortBand)
		{
			_effectiveRenderSortBandCacheLocalSortBand = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._effectiveRenderSortBandRescanCountdown)
		{
			_effectiveRenderSortBandRescanCountdown = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedEffectiveTreeOrderPath)
		{
			_cachedEffectiveTreeOrderPath = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName._treeOrderPathRescanCountdown)
		{
			_treeOrderPathRescanCountdown = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._treeOrderWatchedParent)
		{
			_treeOrderWatchedParent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._renderSortBand)
		{
			_renderSortBand = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._cachedSortBand)
		{
			_cachedSortBand = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._cachedUseDrawOrderSortBands)
		{
			_cachedUseDrawOrderSortBands = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._drawOrderSortBandCacheDirty)
		{
			_drawOrderSortBandCacheDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._needsDrawOrderSortBandsCached)
		{
			_needsDrawOrderSortBandsCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRefreshEveryFrame)
		{
			_cachedRefreshEveryFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedLayerMask)
		{
			_cachedLayerMask = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._cachedAllLayersVisible)
		{
			_cachedAllLayersVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedConfiguredLayersAllVisible)
		{
			_cachedConfiguredLayersAllVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedCanUseLayerMask)
		{
			_cachedCanUseLayerMask = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedLayerVisibleCount)
		{
			_cachedLayerVisibleCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedLayerVisibleValueCount)
		{
			_cachedLayerVisibleValueCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedLayerVisibleSignature)
		{
			_cachedLayerVisibleSignature = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceData)
		{
			_cachedRasterCompositeSequenceData = VariantUtils.ConvertTo<AdobeAnimateRasterCompositeData>(in value);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceClip)
		{
			_cachedRasterCompositeSequenceClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceRange)
		{
			_cachedRasterCompositeSequenceRange = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceLayerSignature)
		{
			_cachedRasterCompositeSequenceLayerSignature = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceTileBase)
		{
			_cachedRasterCompositeSequenceTileBase = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceClipStart)
		{
			_cachedRasterCompositeSequenceClipStart = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceFrameCount)
		{
			_cachedRasterCompositeSequenceFrameCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceValid)
		{
			_cachedRasterCompositeSequenceValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._mediaReplaceStateVersion)
		{
			_mediaReplaceStateVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceStateVersion)
		{
			_cachedMediaReplaceStateVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceLimit)
		{
			_cachedMediaReplaceLimit = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedHasMediaReplace)
		{
			_cachedHasMediaReplace = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedAnyRequestedMediaReplace)
		{
			_cachedAnyRequestedMediaReplace = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceUseMask)
		{
			_cachedMediaReplaceUseMask = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceUseMaskOverflow)
		{
			_cachedMediaReplaceUseMaskOverflow = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasPageCount)
		{
			_cachedMediaReplaceAtlasPageCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasPageValues)
		{
			_cachedMediaReplaceAtlasPageValues = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceSignature)
		{
			_cachedMediaReplaceSignature = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceBoundsGrow)
		{
			_cachedMediaReplaceBoundsGrow = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._layerStateVersion)
		{
			_layerStateVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cachedLayerStateVersion)
		{
			_cachedLayerStateVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._runtimeManagerDispatchActive)
		{
			_runtimeManagerDispatchActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._displayFrameGpuClockPoseReusable)
		{
			_displayFrameGpuClockPoseReusable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeDisplayVisualRequirementCached)
		{
			_runtimeDisplayVisualRequirementCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeDisplayVisualRequirement)
		{
			_cachedRuntimeDisplayVisualRequirement = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeDisplayGpuClockPoseReusable)
		{
			_cachedRuntimeDisplayGpuClockPoseReusable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeDisplayVisualRequirementValidationCountdown)
		{
			_runtimeDisplayVisualRequirementValidationCountdown = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockPoseReuseCached)
		{
			_runtimeGpuClockPoseReuseCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeGpuClockPoseReusable)
		{
			_cachedRuntimeGpuClockPoseReusable = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockPoseReuseValidationCountdown)
		{
			_runtimeGpuClockPoseReuseValidationCountdown = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockInterpolationActive)
		{
			_runtimeGpuClockInterpolationActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockFramesPerSecond)
		{
			_runtimeGpuClockFramesPerSecond = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockClipStart)
		{
			_runtimeGpuClockClipStart = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockClipEndExclusive)
		{
			_runtimeGpuClockClipEndExclusive = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockLoop)
		{
			_runtimeGpuClockLoop = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasCachedRenderSnapshot)
		{
			_hasCachedRenderSnapshot = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._loopTerminalAliasCacheRange)
		{
			_loopTerminalAliasCacheRange = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._loopTerminalAliasCacheHasValue)
		{
			_loopTerminalAliasCacheHasValue = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._loopTerminalAliasCacheValue)
		{
			_loopTerminalAliasCacheValue = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._playbackClipEndCacheRange)
		{
			_playbackClipEndCacheRange = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._playbackClipEndCacheLoop)
		{
			_playbackClipEndCacheLoop = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._playbackClipEndCachePlayBack)
		{
			_playbackClipEndCachePlayBack = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._playbackClipEndCacheHasValue)
		{
			_playbackClipEndCacheHasValue = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._playbackClipEndCacheValue)
		{
			_playbackClipEndCacheValue = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._renderOrderDebugPending)
		{
			_renderOrderDebugPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderOrderDebugReason)
		{
			_renderOrderDebugReason = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._cachedOwnRenderFrameModulateVersion)
		{
			_cachedOwnRenderFrameModulateVersion = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._cachedOwnRenderFrameModulate)
		{
			_cachedOwnRenderFrameModulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._cachedOwnRenderFrameSelfModulateVersion)
		{
			_cachedOwnRenderFrameSelfModulateVersion = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._cachedOwnRenderFrameSelfModulate)
		{
			_cachedOwnRenderFrameSelfModulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._cachedRenderLocalModulateReady)
		{
			_cachedRenderLocalModulateReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRenderLocalModulate)
		{
			_cachedRenderLocalModulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._cachedRenderLocalSelfModulateReady)
		{
			_cachedRenderLocalSelfModulateReady = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRenderLocalSelfModulate)
		{
			_cachedRenderLocalSelfModulate = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._renderLocalModulateRescanCountdown)
		{
			_renderLocalModulateRescanCountdown = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._renderLocalModulateAuditTransactionVersion)
		{
			_renderLocalModulateAuditTransactionVersion = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._runtimeRenderBoundsCached)
		{
			_runtimeRenderBoundsCached = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsVisible)
		{
			_cachedRuntimeRenderBoundsVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsStrictVisible)
		{
			_cachedRuntimeRenderBoundsStrictVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsCullingEnabled)
		{
			_cachedRuntimeRenderBoundsCullingEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsTransform)
		{
			_cachedRuntimeRenderBoundsTransform = VariantUtils.ConvertTo<Transform2D>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsOffset)
		{
			_cachedRuntimeRenderBoundsOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsMountParent)
		{
			_cachedRuntimeRenderBoundsMountParent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsViewport)
		{
			_cachedRuntimeRenderBoundsViewport = VariantUtils.ConvertTo<Viewport>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsViewportWorldRect)
		{
			_cachedRuntimeRenderBoundsViewportWorldRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsRelativeRect)
		{
			_cachedRuntimeRenderBoundsRelativeRect = VariantUtils.ConvertTo<Rect2>(in value);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsHasRelativeRect)
		{
			_cachedRuntimeRenderBoundsHasRelativeRect = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeRenderBoundsRescanCountdown)
		{
			_runtimeRenderBoundsRescanCountdown = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._externalVisualPreparationFrame)
		{
			_externalVisualPreparationFrame = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._externalVisualGpuStateAtlasRids)
		{
			_externalVisualGpuStateAtlasRids = VariantUtils.ConvertTo<Rid[]>(in value);
			return true;
		}
		if (name == PropertyName._externalVisualGpuStateFailures)
		{
			_externalVisualGpuStateFailures = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathLayoutRuntimeMutationSeen)
		{
			_staticAtlasPathLayoutRuntimeMutationSeen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._bypassStaticAtlasPathLayoutCacheForTests)
		{
			_bypassStaticAtlasPathLayoutCacheForTests = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathLayoutGpuResetPending)
		{
			_staticAtlasPathLayoutGpuResetPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotData)
		{
			_staticAtlasPathSnapshotData = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotDataInstanceId)
		{
			_staticAtlasPathSnapshotDataInstanceId = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotAuthoringRevision)
		{
			_staticAtlasPathSnapshotAuthoringRevision = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotMediaCount)
		{
			_staticAtlasPathSnapshotMediaCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotTextureCount)
		{
			_staticAtlasPathSnapshotTextureCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotUseCount)
		{
			_staticAtlasPathSnapshotUseCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotPathCount)
		{
			_staticAtlasPathSnapshotPathCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotEffectiveLimit)
		{
			_staticAtlasPathSnapshotEffectiveLimit = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotActiveCount)
		{
			_staticAtlasPathSnapshotActiveCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotSlotSignature)
		{
			_staticAtlasPathSnapshotSlotSignature = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotPaths)
		{
			_staticAtlasPathSnapshotPaths = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotValid)
		{
			_staticAtlasPathSnapshotValid = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		Color from;
		if (name == PropertyName.Modulate)
		{
			from = Modulate;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SelfModulate)
		{
			from = SelfModulate;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ZIndex)
		{
			value = VariantUtils.CreateFrom<int>(ZIndex);
			return true;
		}
		bool from2;
		if (name == PropertyName.ZAsRelative)
		{
			from2 = ZAsRelative;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.atlasProfileOverride)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateAtlasProfile>(atlasProfileOverride);
			return true;
		}
		if (name == PropertyName.flashAnimeData)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateData>(flashAnimeData);
			return true;
		}
		if (name == PropertyName.preview)
		{
			from2 = preview;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.forceLocalRender)
		{
			from2 = forceLocalRender;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.forceCpuPoseRender)
		{
			from2 = forceCpuPoseRender;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.suppressEditorRenderSubmission)
		{
			from2 = suppressEditorRenderSubmission;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.invisible)
		{
			from2 = invisible;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.offset)
		{
			value = VariantUtils.CreateFrom<Vector2>(offset);
			return true;
		}
		if (name == PropertyName.timeScale)
		{
			value = VariantUtils.CreateFrom<double>(timeScale);
			return true;
		}
		if (name == PropertyName.deduplicateLoopTerminalFrame)
		{
			from2 = deduplicateLoopTerminalFrame;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsFrozenPreview)
		{
			from2 = IsFrozenPreview;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.pause)
		{
			from2 = pause;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsPlaybackStopped)
		{
			from2 = IsPlaybackStopped;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsEffectOnceGpuSuppressed)
		{
			from2 = IsEffectOnceGpuSuppressed;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.runtimeViewportCullingEnabled)
		{
			from2 = runtimeViewportCullingEnabled;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.playBack)
		{
			from2 = playBack;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		ulong from3;
		if (name == PropertyName.ManagedPoseRevision)
		{
			from3 = ManagedPoseRevision;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.clip)
		{
			value = VariantUtils.CreateFrom<string>(clip);
			return true;
		}
		if (name == PropertyName.layerVisible)
		{
			value = VariantUtils.CreateFromArray(layerVisible);
			return true;
		}
		if (name == PropertyName.mediaReplace)
		{
			value = VariantUtils.CreateFromArray(mediaReplace);
			return true;
		}
		if (name == PropertyName.mediaReplaceUse)
		{
			value = VariantUtils.CreateFromArray(mediaReplaceUse);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasPaths)
		{
			value = VariantUtils.CreateFromArray(mediaReplaceAtlasPaths);
			return true;
		}
		if (name == PropertyName.parentSprite)
		{
			value = VariantUtils.CreateFrom<AdobeAnimateSprite>(parentSprite);
			return true;
		}
		if (name == PropertyName.meshColor)
		{
			from = meshColor;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsRuntimeActive)
		{
			from2 = IsRuntimeActive;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsRuntimeTickPaused)
		{
			from2 = IsRuntimeTickPaused;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsRuntimeInsideTree)
		{
			from2 = IsRuntimeInsideTree;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsRuntimeVisibleInTree)
		{
			from2 = IsRuntimeVisibleInTree;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsRuntimeReadyCached)
		{
			from2 = IsRuntimeReadyCached;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsRuntimeDisplayTickActive)
		{
			from2 = IsRuntimeDisplayTickActive;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.NeedsRuntimeRenderSubmission)
		{
			from2 = NeedsRuntimeRenderSubmission;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.UsesRuntimeGpuClockInterpolation)
		{
			from2 = UsesRuntimeGpuClockInterpolation;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PrefersCachedGpuGraphCrowdRenderState)
		{
			from2 = PrefersCachedGpuGraphCrowdRenderState;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.RuntimeManagerDispatchActive)
		{
			from2 = RuntimeManagerDispatchActive;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ShouldDispatchProcessCallback)
		{
			from2 = ShouldDispatchProcessCallback;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.RequiresDisplayFrameBatchedProcessExtension)
		{
			from2 = RequiresDisplayFrameBatchedProcessExtension;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.NeedsRenderSubmissionForBareTest)
		{
			from2 = NeedsRenderSubmissionForBareTest;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PlaybackRevisionForBareTest)
		{
			from3 = PlaybackRevisionForBareTest;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName._effectOnceBatchVisualRevision)
		{
			value = VariantUtils.CreateFrom(in _effectOnceBatchVisualRevision);
			return true;
		}
		if (name == PropertyName._effectOnceBatchEligibilityRevision)
		{
			value = VariantUtils.CreateFrom(in _effectOnceBatchEligibilityRevision);
			return true;
		}
		if (name == PropertyName._flashAnimeData)
		{
			value = VariantUtils.CreateFrom(in _flashAnimeData);
			return true;
		}
		if (name == PropertyName._flashAnimeDataChangePending)
		{
			value = VariantUtils.CreateFrom(in _flashAnimeDataChangePending);
			return true;
		}
		if (name == PropertyName._atlasProfileOverride)
		{
			value = VariantUtils.CreateFrom(in _atlasProfileOverride);
			return true;
		}
		if (name == PropertyName._preview)
		{
			value = VariantUtils.CreateFrom(in _preview);
			return true;
		}
		if (name == PropertyName._forceLocalRender)
		{
			value = VariantUtils.CreateFrom(in _forceLocalRender);
			return true;
		}
		if (name == PropertyName._renderClipControl)
		{
			value = VariantUtils.CreateFrom(in _renderClipControl);
			return true;
		}
		if (name == PropertyName._forceCpuPoseRender)
		{
			value = VariantUtils.CreateFrom(in _forceCpuPoseRender);
			return true;
		}
		if (name == PropertyName._invisible)
		{
			value = VariantUtils.CreateFrom(in _invisible);
			return true;
		}
		if (name == PropertyName.normalAlpha)
		{
			value = VariantUtils.CreateFrom(in normalAlpha);
			return true;
		}
		if (name == PropertyName._offset)
		{
			value = VariantUtils.CreateFrom(in _offset);
			return true;
		}
		if (name == PropertyName.offsetRotate)
		{
			value = VariantUtils.CreateFrom(in offsetRotate);
			return true;
		}
		if (name == PropertyName._timeScale)
		{
			value = VariantUtils.CreateFrom(in _timeScale);
			return true;
		}
		if (name == PropertyName.trueFrameRate)
		{
			value = VariantUtils.CreateFrom(in trueFrameRate);
			return true;
		}
		if (name == PropertyName.refreshEveryFlame)
		{
			value = VariantUtils.CreateFrom(in refreshEveryFlame);
			return true;
		}
		if (name == PropertyName.skipLastFrame)
		{
			value = VariantUtils.CreateFrom(in skipLastFrame);
			return true;
		}
		if (name == PropertyName._deduplicateLoopTerminalFrame)
		{
			value = VariantUtils.CreateFrom(in _deduplicateLoopTerminalFrame);
			return true;
		}
		if (name == PropertyName.usePos)
		{
			value = VariantUtils.CreateFrom(in usePos);
			return true;
		}
		if (name == PropertyName.useRotate)
		{
			value = VariantUtils.CreateFrom(in useRotate);
			return true;
		}
		if (name == PropertyName.useFollowVisible)
		{
			value = VariantUtils.CreateFrom(in useFollowVisible);
			return true;
		}
		if (name == PropertyName.blendTimeInit)
		{
			value = VariantUtils.CreateFrom(in blendTimeInit);
			return true;
		}
		if (name == PropertyName._pause)
		{
			value = VariantUtils.CreateFrom(in _pause);
			return true;
		}
		if (name == PropertyName._playBack)
		{
			value = VariantUtils.CreateFrom(in _playBack);
			return true;
		}
		if (name == PropertyName._playbackBlocked)
		{
			value = VariantUtils.CreateFrom(in _playbackBlocked);
			return true;
		}
		if (name == PropertyName._parentPlaybackStopped)
		{
			value = VariantUtils.CreateFrom(in _parentPlaybackStopped);
			return true;
		}
		if (name == PropertyName._frozenPreview)
		{
			value = VariantUtils.CreateFrom(in _frozenPreview);
			return true;
		}
		if (name == PropertyName._effectOnceGpuSuppressed)
		{
			value = VariantUtils.CreateFrom(in _effectOnceGpuSuppressed);
			return true;
		}
		if (name == PropertyName._effectOnceGpuSuppressionToken)
		{
			value = VariantUtils.CreateFrom(in _effectOnceGpuSuppressionToken);
			return true;
		}
		if (name == PropertyName._applyingImmediateAnimation)
		{
			value = VariantUtils.CreateFrom(in _applyingImmediateAnimation);
			return true;
		}
		if (name == PropertyName.keepRenderSubmittedWhenPaused)
		{
			value = VariantUtils.CreateFrom(in keepRenderSubmittedWhenPaused);
			return true;
		}
		if (name == PropertyName._runtimeViewportCullingEnabled)
		{
			value = VariantUtils.CreateFrom(in _runtimeViewportCullingEnabled);
			return true;
		}
		if (name == PropertyName.onlyDraw)
		{
			value = VariantUtils.CreateFrom(in onlyDraw);
			return true;
		}
		if (name == PropertyName.elapsedTimer)
		{
			value = VariantUtils.CreateFrom(in elapsedTimer);
			return true;
		}
		if (name == PropertyName.refreshTimer)
		{
			value = VariantUtils.CreateFrom(in refreshTimer);
			return true;
		}
		if (name == PropertyName.blend)
		{
			value = VariantUtils.CreateFrom(in blend);
			return true;
		}
		if (name == PropertyName.blendTime)
		{
			value = VariantUtils.CreateFrom(in blendTime);
			return true;
		}
		if (name == PropertyName.blendTimer)
		{
			value = VariantUtils.CreateFrom(in blendTimer);
			return true;
		}
		if (name == PropertyName._blendFromFrameFloat)
		{
			value = VariantUtils.CreateFrom(in _blendFromFrameFloat);
			return true;
		}
		if (name == PropertyName.frameIndex)
		{
			value = VariantUtils.CreateFrom(in frameIndex);
			return true;
		}
		if (name == PropertyName.frameRate)
		{
			value = VariantUtils.CreateFrom(in frameRate);
			return true;
		}
		if (name == PropertyName.refreshEveryFrame)
		{
			value = VariantUtils.CreateFrom(in refreshEveryFrame);
			return true;
		}
		if (name == PropertyName.loop)
		{
			value = VariantUtils.CreateFrom(in loop);
			return true;
		}
		if (name == PropertyName._clip)
		{
			value = VariantUtils.CreateFrom(in _clip);
			return true;
		}
		if (name == PropertyName._playbackRevision)
		{
			value = VariantUtils.CreateFrom(in _playbackRevision);
			return true;
		}
		if (name == PropertyName._managedPoseRevision)
		{
			value = VariantUtils.CreateFrom(in _managedPoseRevision);
			return true;
		}
		if (name == PropertyName.clipRange)
		{
			value = VariantUtils.CreateFrom(in clipRange);
			return true;
		}
		if (name == PropertyName.clipOver)
		{
			value = VariantUtils.CreateFrom(in clipOver);
			return true;
		}
		if (name == PropertyName._layerVisible)
		{
			value = VariantUtils.CreateFromArray(_layerVisible);
			return true;
		}
		if (name == PropertyName._effectOnceBatchArrayExposureRevision)
		{
			value = VariantUtils.CreateFrom(in _effectOnceBatchArrayExposureRevision);
			return true;
		}
		if (name == PropertyName._effectOnceBatchEligibilityTracking)
		{
			value = VariantUtils.CreateFrom(in _effectOnceBatchEligibilityTracking);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlas)
		{
			value = VariantUtils.CreateFrom(in mediaReplaceAtlas);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasArray)
		{
			value = VariantUtils.CreateFrom(in mediaReplaceAtlasArray);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasArraySize)
		{
			value = VariantUtils.CreateFrom(in mediaReplaceAtlasArraySize);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasUsesTextureArray)
		{
			value = VariantUtils.CreateFrom(in mediaReplaceAtlasUsesTextureArray);
			return true;
		}
		if (name == PropertyName.mediaReplaceRect)
		{
			value = VariantUtils.CreateFromArray(mediaReplaceRect);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasPages)
		{
			value = VariantUtils.CreateFromArray(mediaReplaceAtlasPages);
			return true;
		}
		if (name == PropertyName._mediaReplace)
		{
			value = VariantUtils.CreateFromArray(_mediaReplace);
			return true;
		}
		if (name == PropertyName._mediaReplaceUse)
		{
			value = VariantUtils.CreateFromArray(_mediaReplaceUse);
			return true;
		}
		if (name == PropertyName._mediaReplaceAtlasPaths)
		{
			value = VariantUtils.CreateFromArray(_mediaReplaceAtlasPaths);
			return true;
		}
		if (name == PropertyName.mediaReplaceAtlasShared)
		{
			value = VariantUtils.CreateFrom(in mediaReplaceAtlasShared);
			return true;
		}
		if (name == PropertyName.canvasItem)
		{
			value = VariantUtils.CreateFrom(in canvasItem);
			return true;
		}
		if (name == PropertyName.meshTexture)
		{
			value = VariantUtils.CreateFrom(in meshTexture);
			return true;
		}
		if (name == PropertyName.mesh)
		{
			value = VariantUtils.CreateFrom(in mesh);
			return true;
		}
		if (name == PropertyName.needMediaReplaceUpdate)
		{
			value = VariantUtils.CreateFrom(in needMediaReplaceUpdate);
			return true;
		}
		if (name == PropertyName._canRun)
		{
			value = VariantUtils.CreateFrom(in _canRun);
			return true;
		}
		if (name == PropertyName.initClip)
		{
			value = VariantUtils.CreateFrom(in initClip);
			return true;
		}
		if (name == PropertyName._parentSprite)
		{
			value = VariantUtils.CreateFrom(in _parentSprite);
			return true;
		}
		if (name == PropertyName._parentSpriteResolved)
		{
			value = VariantUtils.CreateFrom(in _parentSpriteResolved);
			return true;
		}
		if (name == PropertyName.followParentSpriteLayerId)
		{
			value = VariantUtils.CreateFrom(in followParentSpriteLayerId);
			return true;
		}
		if (name == PropertyName.insertLayerId)
		{
			value = VariantUtils.CreateFrom(in insertLayerId);
			return true;
		}
		if (name == PropertyName._meshColor)
		{
			value = VariantUtils.CreateFrom(in _meshColor);
			return true;
		}
		if (name == PropertyName._renderColorMultiplier)
		{
			value = VariantUtils.CreateFrom(in _renderColorMultiplier);
			return true;
		}
		if (name == PropertyName._runtimeGpuGraphActive)
		{
			value = VariantUtils.CreateFrom(in _runtimeGpuGraphActive);
			return true;
		}
		if (name == PropertyName._hasCachedGpuGraphCrowdState)
		{
			value = VariantUtils.CreateFrom(in _hasCachedGpuGraphCrowdState);
			return true;
		}
		if (name == PropertyName._cachedGpuGraphCrowdStatePhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _cachedGpuGraphCrowdStatePhysicsFrame);
			return true;
		}
		if (name == PropertyName._hasCachedRasterCompositeRenderState)
		{
			value = VariantUtils.CreateFrom(in _hasCachedRasterCompositeRenderState);
			return true;
		}
		if (name == PropertyName._gpuGraphCrowdStaticStateDirty)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphCrowdStaticStateDirty);
			return true;
		}
		if (name == PropertyName._gpuGraphCrowdPresentationStateDirty)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphCrowdPresentationStateDirty);
			return true;
		}
		if (name == PropertyName._gpuGraphCrowdStateNextAuditPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphCrowdStateNextAuditPhysicsFrame);
			return true;
		}
		if (name == PropertyName._renderGrayscale)
		{
			value = VariantUtils.CreateFrom(in _renderGrayscale);
			return true;
		}
		if (name == PropertyName._autoInserting)
		{
			value = VariantUtils.CreateFrom(in _autoInserting);
			return true;
		}
		if (name == PropertyName._slotChildren)
		{
			GodotObject[] slotChildren = _slotChildren;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(slotChildren);
			return true;
		}
		if (name == PropertyName._runtimeSlotChildren)
		{
			GodotObject[] slotChildren = _runtimeSlotChildren;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(slotChildren);
			return true;
		}
		if (name == PropertyName._managedSlotGpuStateAtlasRids)
		{
			value = VariantUtils.CreateFrom(in _managedSlotGpuStateAtlasRids);
			return true;
		}
		if (name == PropertyName._managedSlotGpuStateRescanCountdowns)
		{
			value = VariantUtils.CreateFrom(in _managedSlotGpuStateRescanCountdowns);
			return true;
		}
		if (name == PropertyName._managedSlotGpuStateWatchedSprites)
		{
			GodotObject[] slotChildren = _managedSlotGpuStateWatchedSprites;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(slotChildren);
			return true;
		}
		if (name == PropertyName._managedSlotGpuStateWatchedParts)
		{
			GodotObject[] slotChildren = _managedSlotGpuStateWatchedParts;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(slotChildren);
			return true;
		}
		if (name == PropertyName._runtimeSlotChildrenDirty)
		{
			value = VariantUtils.CreateFrom(in _runtimeSlotChildrenDirty);
			return true;
		}
		if (name == PropertyName._runtimeHasChildUpdates)
		{
			value = VariantUtils.CreateFrom(in _runtimeHasChildUpdates);
			return true;
		}
		if (name == PropertyName._runtimeChildUpdatesVisualOnlyDirty)
		{
			value = VariantUtils.CreateFrom(in _runtimeChildUpdatesVisualOnlyDirty);
			return true;
		}
		if (name == PropertyName._runtimeChildUpdatesVisualOnlyCached)
		{
			value = VariantUtils.CreateFrom(in _runtimeChildUpdatesVisualOnlyCached);
			return true;
		}
		if (name == PropertyName._managedSlotSpritesDirty)
		{
			value = VariantUtils.CreateFrom(in _managedSlotSpritesDirty);
			return true;
		}
		if (name == PropertyName._spriteChildren)
		{
			GodotObject[] slotChildren = _spriteChildren;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(slotChildren);
			return true;
		}
		if (name == PropertyName._spriteChildOwnerSlots)
		{
			GodotObject[] slotChildren = _spriteChildOwnerSlots;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(slotChildren);
			return true;
		}
		if (name == PropertyName._hasChildren)
		{
			value = VariantUtils.CreateFrom(in _hasChildren);
			return true;
		}
		if (name == PropertyName._runtimeChildViewportFrame)
		{
			value = VariantUtils.CreateFrom(in _runtimeChildViewportFrame);
			return true;
		}
		if (name == PropertyName._runtimeChildViewportVisible)
		{
			value = VariantUtils.CreateFrom(in _runtimeChildViewportVisible);
			return true;
		}
		if (name == PropertyName._lastUpdateFrame)
		{
			value = VariantUtils.CreateFrom(in _lastUpdateFrame);
			return true;
		}
		if (name == PropertyName._isVisibleInTree)
		{
			value = VariantUtils.CreateFrom(in _isVisibleInTree);
			return true;
		}
		if (name == PropertyName._runtimeInsideTree)
		{
			value = VariantUtils.CreateFrom(in _runtimeInsideTree);
			return true;
		}
		if (name == PropertyName._processEnabled)
		{
			value = VariantUtils.CreateFrom(in _processEnabled);
			return true;
		}
		if (name == PropertyName._usingRuntimeManager)
		{
			value = VariantUtils.CreateFrom(in _usingRuntimeManager);
			return true;
		}
		if (name == PropertyName._cachedInstanceIdForRender)
		{
			value = VariantUtils.CreateFrom(in _cachedInstanceIdForRender);
			return true;
		}
		if (name == PropertyName._gpuGraphPreparedGeneration)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphPreparedGeneration);
			return true;
		}
		if (name == PropertyName._gpuGraphPreparedSignature)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphPreparedSignature);
			return true;
		}
		if (name == PropertyName._gpuGraphPreparedQuadCount)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphPreparedQuadCount);
			return true;
		}
		if (name == PropertyName._dispatchingBatchedProcess)
		{
			value = VariantUtils.CreateFrom(in _dispatchingBatchedProcess);
			return true;
		}
		if (name == PropertyName._displayFrameVisualDispatchActive)
		{
			value = VariantUtils.CreateFrom(in _displayFrameVisualDispatchActive);
			return true;
		}
		if (name == PropertyName._runtimeDisplayTickVersion)
		{
			value = VariantUtils.CreateFrom(in _runtimeDisplayTickVersion);
			return true;
		}
		if (name == PropertyName._runtimeReadyCached)
		{
			value = VariantUtils.CreateFrom(in _runtimeReadyCached);
			return true;
		}
		if (name == PropertyName._runtimeNativeCanvasSuppressed)
		{
			value = VariantUtils.CreateFrom(in _runtimeNativeCanvasSuppressed);
			return true;
		}
		if (name == PropertyName._runtimeNativeCanvasPublishedFrameVersion)
		{
			value = VariantUtils.CreateFrom(in _runtimeNativeCanvasPublishedFrameVersion);
			return true;
		}
		if (name == PropertyName._runtimeManagerFirstFrameHandoffActive)
		{
			value = VariantUtils.CreateFrom(in _runtimeManagerFirstFrameHandoffActive);
			return true;
		}
		if (name == PropertyName._runtimeNativeCanvasVisibilityLayer)
		{
			value = VariantUtils.CreateFrom(in _runtimeNativeCanvasVisibilityLayer);
			return true;
		}
		if (name == PropertyName._runtimeNativeCanvasSuppressionEligibilityKnown)
		{
			value = VariantUtils.CreateFrom(in _runtimeNativeCanvasSuppressionEligibilityKnown);
			return true;
		}
		if (name == PropertyName._runtimeNativeCanvasSuppressionEligible)
		{
			value = VariantUtils.CreateFrom(in _runtimeNativeCanvasSuppressionEligible);
			return true;
		}
		if (name == PropertyName._runtimeParentStateSource)
		{
			value = VariantUtils.CreateFrom(in _runtimeParentStateSource);
			return true;
		}
		if (name == PropertyName._runtimeParentStateValidationCountdown)
		{
			value = VariantUtils.CreateFrom(in _runtimeParentStateValidationCountdown);
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
		if (name == PropertyName._needsRenderSubmission)
		{
			value = VariantUtils.CreateFrom(in _needsRenderSubmission);
			return true;
		}
		if (name == PropertyName._runtimeRenderSubmissionRetryRequested)
		{
			value = VariantUtils.CreateFrom(in _runtimeRenderSubmissionRetryRequested);
			return true;
		}
		if (name == PropertyName._runtimeGpuPrepareRequired)
		{
			value = VariantUtils.CreateFrom(in _runtimeGpuPrepareRequired);
			return true;
		}
		if (name == PropertyName._visibilityParentCanvas)
		{
			value = VariantUtils.CreateFrom(in _visibilityParentCanvas);
			return true;
		}
		if (name == PropertyName._selfVisibilityChangedConnected)
		{
			value = VariantUtils.CreateFrom(in _selfVisibilityChangedConnected);
			return true;
		}
		if (name == PropertyName._parentVisibilityChangedConnected)
		{
			value = VariantUtils.CreateFrom(in _parentVisibilityChangedConnected);
			return true;
		}
		if (name == PropertyName._cachedRenderMountParent)
		{
			value = VariantUtils.CreateFrom(in _cachedRenderMountParent);
			return true;
		}
		if (name == PropertyName._cachedViewport)
		{
			value = VariantUtils.CreateFrom(in _cachedViewport);
			return true;
		}
		if (name == PropertyName._renderMountNextAuditPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _renderMountNextAuditPhysicsFrame);
			return true;
		}
		if (name == PropertyName._renderModulateMountParent)
		{
			value = VariantUtils.CreateFrom(in _renderModulateMountParent);
			return true;
		}
		if (name == PropertyName._renderModulateAncestorCacheReady)
		{
			value = VariantUtils.CreateFrom(in _renderModulateAncestorCacheReady);
			return true;
		}
		if (name == PropertyName._renderModulateAncestorRescanCountdown)
		{
			value = VariantUtils.CreateFrom(in _renderModulateAncestorRescanCountdown);
			return true;
		}
		if (name == PropertyName._cachedCanvasLayer)
		{
			value = VariantUtils.CreateFrom(in _cachedCanvasLayer);
			return true;
		}
		if (name == PropertyName._cachedManagedPoseTrackKey)
		{
			value = VariantUtils.CreateFrom(in _cachedManagedPoseTrackKey);
			return true;
		}
		if (name == PropertyName._cachedManagedPoseTrackUseLayerId)
		{
			value = VariantUtils.CreateFrom(in _cachedManagedPoseTrackUseLayerId);
			return true;
		}
		if (name == PropertyName._nextAnimDelayTimer)
		{
			value = VariantUtils.CreateFrom(in _nextAnimDelayTimer);
			return true;
		}
		if (name == PropertyName._renderStateCached)
		{
			value = VariantUtils.CreateFrom(in _renderStateCached);
			return true;
		}
		if (name == PropertyName._renderStaticStateCached)
		{
			value = VariantUtils.CreateFrom(in _renderStaticStateCached);
			return true;
		}
		if (name == PropertyName._renderStaticLayerArrayEscaped)
		{
			value = VariantUtils.CreateFrom(in _renderStaticLayerArrayEscaped);
			return true;
		}
		if (name == PropertyName._renderStaticMediaArraysEscaped)
		{
			value = VariantUtils.CreateFrom(in _renderStaticMediaArraysEscaped);
			return true;
		}
		if (name == PropertyName._renderStaticArrayAuditSubmissionPending)
		{
			value = VariantUtils.CreateFrom(in _renderStaticArrayAuditSubmissionPending);
			return true;
		}
		if (name == PropertyName._renderStaticStateNextAuditPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _renderStaticStateNextAuditPhysicsFrame);
			return true;
		}
		if (name == PropertyName._renderStaticAuditLayerVisibleSnapshot)
		{
			value = VariantUtils.CreateFromArray(_renderStaticAuditLayerVisibleSnapshot);
			return true;
		}
		if (name == PropertyName._renderStaticAuditMediaReplaceUseSnapshot)
		{
			value = VariantUtils.CreateFromArray(_renderStaticAuditMediaReplaceUseSnapshot);
			return true;
		}
		if (name == PropertyName._renderStaticAuditMediaReplaceSnapshot)
		{
			value = VariantUtils.CreateFromArray(_renderStaticAuditMediaReplaceSnapshot);
			return true;
		}
		if (name == PropertyName._renderStaticAuditMediaReplaceRectSnapshot)
		{
			value = VariantUtils.CreateFromArray(_renderStaticAuditMediaReplaceRectSnapshot);
			return true;
		}
		if (name == PropertyName._renderStaticAuditMediaReplacePageSnapshot)
		{
			value = VariantUtils.CreateFromArray(_renderStaticAuditMediaReplacePageSnapshot);
			return true;
		}
		if (name == PropertyName._renderStaticAuditMediaReplaceAtlasPathSnapshot)
		{
			value = VariantUtils.CreateFromArray(_renderStaticAuditMediaReplaceAtlasPathSnapshot);
			return true;
		}
		if (name == PropertyName._cachedGpuGraphRootOwnerPlaybackRevision)
		{
			value = VariantUtils.CreateFrom(in _cachedGpuGraphRootOwnerPlaybackRevision);
			return true;
		}
		if (name == PropertyName._hasCachedGpuGraphNestedOwnerState)
		{
			value = VariantUtils.CreateFrom(in _hasCachedGpuGraphNestedOwnerState);
			return true;
		}
		if (name == PropertyName._cachedGpuGraphNestedOwnerPlaybackRevision)
		{
			value = VariantUtils.CreateFrom(in _cachedGpuGraphNestedOwnerPlaybackRevision);
			return true;
		}
		if (name == PropertyName._gpuGraphNestedOwnerStateRebaseClockSeconds)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphNestedOwnerStateRebaseClockSeconds);
			return true;
		}
		if (name == PropertyName._cachedClip)
		{
			value = VariantUtils.CreateFrom(in _cachedClip);
			return true;
		}
		if (name == PropertyName._cachedClipRange)
		{
			value = VariantUtils.CreateFrom(in _cachedClipRange);
			return true;
		}
		if (name == PropertyName._cachedFrameFloat)
		{
			value = VariantUtils.CreateFrom(in _cachedFrameFloat);
			return true;
		}
		if (name == PropertyName._cachedGlobalTransform)
		{
			value = VariantUtils.CreateFrom(in _cachedGlobalTransform);
			return true;
		}
		if (name == PropertyName._renderGlobalTransform)
		{
			value = VariantUtils.CreateFrom(in _renderGlobalTransform);
			return true;
		}
		if (name == PropertyName._renderPreviousGlobalTransform)
		{
			value = VariantUtils.CreateFrom(in _renderPreviousGlobalTransform);
			return true;
		}
		if (name == PropertyName._renderGlobalTransformCached)
		{
			value = VariantUtils.CreateFrom(in _renderGlobalTransformCached);
			return true;
		}
		if (name == PropertyName._renderGlobalTransformDirty)
		{
			value = VariantUtils.CreateFrom(in _renderGlobalTransformDirty);
			return true;
		}
		if (name == PropertyName._renderTransformChangedInPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _renderTransformChangedInPhysicsFrame);
			return true;
		}
		if (name == PropertyName._runtimePhysicsAncestorTransformPending)
		{
			value = VariantUtils.CreateFrom(in _runtimePhysicsAncestorTransformPending);
			return true;
		}
		if (name == PropertyName._knownAncestorTranslationNotificationPending)
		{
			value = VariantUtils.CreateFrom(in _knownAncestorTranslationNotificationPending);
			return true;
		}
		if (name == PropertyName._renderRootMotionEnabled)
		{
			value = VariantUtils.CreateFrom(in _renderRootMotionEnabled);
			return true;
		}
		if (name == PropertyName._renderTreePaused)
		{
			value = VariantUtils.CreateFrom(in _renderTreePaused);
			return true;
		}
		if (name == PropertyName._renderGlobalTransformPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _renderGlobalTransformPhysicsFrame);
			return true;
		}
		if (name == PropertyName._cachedModulate)
		{
			value = VariantUtils.CreateFrom(in _cachedModulate);
			return true;
		}
		if (name == PropertyName._cachedOffset)
		{
			value = VariantUtils.CreateFrom(in _cachedOffset);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlas)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceAtlas);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasArray)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceAtlasArray);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasRid)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceAtlasRid);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasArrayRid)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceAtlasArrayRid);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasShared)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceAtlasShared);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasUsesTextureArray)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceAtlasUsesTextureArray);
			return true;
		}
		if (name == PropertyName._cachedRenderMountParentSubmit)
		{
			value = VariantUtils.CreateFrom(in _cachedRenderMountParentSubmit);
			return true;
		}
		if (name == PropertyName._cachedCanvasLayerSubmit)
		{
			value = VariantUtils.CreateFrom(in _cachedCanvasLayerSubmit);
			return true;
		}
		if (name == PropertyName._cachedZIndex)
		{
			value = VariantUtils.CreateFrom(in _cachedZIndex);
			return true;
		}
		if (name == PropertyName._cachedEffectiveZIndex)
		{
			value = VariantUtils.CreateFrom(in _cachedEffectiveZIndex);
			return true;
		}
		if (name == PropertyName._effectiveZIndexCached)
		{
			value = VariantUtils.CreateFrom(in _effectiveZIndexCached);
			return true;
		}
		if (name == PropertyName._effectiveZIndexCacheLocalZ)
		{
			value = VariantUtils.CreateFrom(in _effectiveZIndexCacheLocalZ);
			return true;
		}
		if (name == PropertyName._effectiveZIndexCacheZAsRelative)
		{
			value = VariantUtils.CreateFrom(in _effectiveZIndexCacheZAsRelative);
			return true;
		}
		if (name == PropertyName._effectiveZIndexRescanCountdown)
		{
			value = VariantUtils.CreateFrom(in _effectiveZIndexRescanCountdown);
			return true;
		}
		if (name == PropertyName._effectOnceBatchRenderTransform)
		{
			value = VariantUtils.CreateFrom(in _effectOnceBatchRenderTransform);
			return true;
		}
		if (name == PropertyName._effectOnceBatchEffectiveZIndex)
		{
			value = VariantUtils.CreateFrom(in _effectOnceBatchEffectiveZIndex);
			return true;
		}
		if (name == PropertyName._effectOnceBatchRenderRevision)
		{
			value = VariantUtils.CreateFrom(in _effectOnceBatchRenderRevision);
			return true;
		}
		if (name == PropertyName._effectOnceBatchObservedLocalZIndex)
		{
			value = VariantUtils.CreateFrom(in _effectOnceBatchObservedLocalZIndex);
			return true;
		}
		if (name == PropertyName._effectOnceBatchObservedZAsRelative)
		{
			value = VariantUtils.CreateFrom(in _effectOnceBatchObservedZAsRelative);
			return true;
		}
		if (name == PropertyName._effectOnceBatchRenderNextAuditFrame)
		{
			value = VariantUtils.CreateFrom(in _effectOnceBatchRenderNextAuditFrame);
			return true;
		}
		if (name == PropertyName._effectOnceBatchRenderStateDirty)
		{
			value = VariantUtils.CreateFrom(in _effectOnceBatchRenderStateDirty);
			return true;
		}
		if (name == PropertyName._cachedEffectiveRenderSortBand)
		{
			value = VariantUtils.CreateFrom(in _cachedEffectiveRenderSortBand);
			return true;
		}
		if (name == PropertyName._effectiveRenderSortBandCached)
		{
			value = VariantUtils.CreateFrom(in _effectiveRenderSortBandCached);
			return true;
		}
		if (name == PropertyName._effectiveRenderSortBandCacheLocalSortBand)
		{
			value = VariantUtils.CreateFrom(in _effectiveRenderSortBandCacheLocalSortBand);
			return true;
		}
		if (name == PropertyName._effectiveRenderSortBandRescanCountdown)
		{
			value = VariantUtils.CreateFrom(in _effectiveRenderSortBandRescanCountdown);
			return true;
		}
		if (name == PropertyName._cachedEffectiveTreeOrderPath)
		{
			value = VariantUtils.CreateFrom(in _cachedEffectiveTreeOrderPath);
			return true;
		}
		if (name == PropertyName._treeOrderPathRescanCountdown)
		{
			value = VariantUtils.CreateFrom(in _treeOrderPathRescanCountdown);
			return true;
		}
		if (name == PropertyName._treeOrderWatchedParent)
		{
			value = VariantUtils.CreateFrom(in _treeOrderWatchedParent);
			return true;
		}
		if (name == PropertyName._renderSortBand)
		{
			value = VariantUtils.CreateFrom(in _renderSortBand);
			return true;
		}
		if (name == PropertyName._cachedSortBand)
		{
			value = VariantUtils.CreateFrom(in _cachedSortBand);
			return true;
		}
		if (name == PropertyName._cachedUseDrawOrderSortBands)
		{
			value = VariantUtils.CreateFrom(in _cachedUseDrawOrderSortBands);
			return true;
		}
		if (name == PropertyName._drawOrderSortBandCacheDirty)
		{
			value = VariantUtils.CreateFrom(in _drawOrderSortBandCacheDirty);
			return true;
		}
		if (name == PropertyName._needsDrawOrderSortBandsCached)
		{
			value = VariantUtils.CreateFrom(in _needsDrawOrderSortBandsCached);
			return true;
		}
		if (name == PropertyName._cachedRefreshEveryFrame)
		{
			value = VariantUtils.CreateFrom(in _cachedRefreshEveryFrame);
			return true;
		}
		if (name == PropertyName._cachedLayerMask)
		{
			value = VariantUtils.CreateFrom(in _cachedLayerMask);
			return true;
		}
		if (name == PropertyName._cachedAllLayersVisible)
		{
			value = VariantUtils.CreateFrom(in _cachedAllLayersVisible);
			return true;
		}
		if (name == PropertyName._cachedConfiguredLayersAllVisible)
		{
			value = VariantUtils.CreateFrom(in _cachedConfiguredLayersAllVisible);
			return true;
		}
		if (name == PropertyName._cachedCanUseLayerMask)
		{
			value = VariantUtils.CreateFrom(in _cachedCanUseLayerMask);
			return true;
		}
		if (name == PropertyName._cachedLayerVisibleCount)
		{
			value = VariantUtils.CreateFrom(in _cachedLayerVisibleCount);
			return true;
		}
		if (name == PropertyName._cachedLayerVisibleValueCount)
		{
			value = VariantUtils.CreateFrom(in _cachedLayerVisibleValueCount);
			return true;
		}
		if (name == PropertyName._cachedLayerVisibleSignature)
		{
			value = VariantUtils.CreateFrom(in _cachedLayerVisibleSignature);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceData)
		{
			value = VariantUtils.CreateFrom(in _cachedRasterCompositeSequenceData);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceClip)
		{
			value = VariantUtils.CreateFrom(in _cachedRasterCompositeSequenceClip);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceRange)
		{
			value = VariantUtils.CreateFrom(in _cachedRasterCompositeSequenceRange);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceLayerSignature)
		{
			value = VariantUtils.CreateFrom(in _cachedRasterCompositeSequenceLayerSignature);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceTileBase)
		{
			value = VariantUtils.CreateFrom(in _cachedRasterCompositeSequenceTileBase);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceClipStart)
		{
			value = VariantUtils.CreateFrom(in _cachedRasterCompositeSequenceClipStart);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceFrameCount)
		{
			value = VariantUtils.CreateFrom(in _cachedRasterCompositeSequenceFrameCount);
			return true;
		}
		if (name == PropertyName._cachedRasterCompositeSequenceValid)
		{
			value = VariantUtils.CreateFrom(in _cachedRasterCompositeSequenceValid);
			return true;
		}
		if (name == PropertyName._mediaReplaceStateVersion)
		{
			value = VariantUtils.CreateFrom(in _mediaReplaceStateVersion);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceStateVersion)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceStateVersion);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceLimit)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceLimit);
			return true;
		}
		if (name == PropertyName._cachedHasMediaReplace)
		{
			value = VariantUtils.CreateFrom(in _cachedHasMediaReplace);
			return true;
		}
		if (name == PropertyName._cachedAnyRequestedMediaReplace)
		{
			value = VariantUtils.CreateFrom(in _cachedAnyRequestedMediaReplace);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceUseMask)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceUseMask);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceUseMaskOverflow)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceUseMaskOverflow);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasPageCount)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceAtlasPageCount);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceAtlasPageValues)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceAtlasPageValues);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceSignature)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceSignature);
			return true;
		}
		if (name == PropertyName._cachedMediaReplaceBoundsGrow)
		{
			value = VariantUtils.CreateFrom(in _cachedMediaReplaceBoundsGrow);
			return true;
		}
		if (name == PropertyName._layerStateVersion)
		{
			value = VariantUtils.CreateFrom(in _layerStateVersion);
			return true;
		}
		if (name == PropertyName._cachedLayerStateVersion)
		{
			value = VariantUtils.CreateFrom(in _cachedLayerStateVersion);
			return true;
		}
		if (name == PropertyName._runtimeManagerDispatchActive)
		{
			value = VariantUtils.CreateFrom(in _runtimeManagerDispatchActive);
			return true;
		}
		if (name == PropertyName._displayFrameGpuClockPoseReusable)
		{
			value = VariantUtils.CreateFrom(in _displayFrameGpuClockPoseReusable);
			return true;
		}
		if (name == PropertyName._runtimeDisplayVisualRequirementCached)
		{
			value = VariantUtils.CreateFrom(in _runtimeDisplayVisualRequirementCached);
			return true;
		}
		if (name == PropertyName._cachedRuntimeDisplayVisualRequirement)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeDisplayVisualRequirement);
			return true;
		}
		if (name == PropertyName._cachedRuntimeDisplayGpuClockPoseReusable)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeDisplayGpuClockPoseReusable);
			return true;
		}
		if (name == PropertyName._runtimeDisplayVisualRequirementValidationCountdown)
		{
			value = VariantUtils.CreateFrom(in _runtimeDisplayVisualRequirementValidationCountdown);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockPoseReuseCached)
		{
			value = VariantUtils.CreateFrom(in _runtimeGpuClockPoseReuseCached);
			return true;
		}
		if (name == PropertyName._cachedRuntimeGpuClockPoseReusable)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeGpuClockPoseReusable);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockPoseReuseValidationCountdown)
		{
			value = VariantUtils.CreateFrom(in _runtimeGpuClockPoseReuseValidationCountdown);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockInterpolationActive)
		{
			value = VariantUtils.CreateFrom(in _runtimeGpuClockInterpolationActive);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockFramesPerSecond)
		{
			value = VariantUtils.CreateFrom(in _runtimeGpuClockFramesPerSecond);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockClipStart)
		{
			value = VariantUtils.CreateFrom(in _runtimeGpuClockClipStart);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockClipEndExclusive)
		{
			value = VariantUtils.CreateFrom(in _runtimeGpuClockClipEndExclusive);
			return true;
		}
		if (name == PropertyName._runtimeGpuClockLoop)
		{
			value = VariantUtils.CreateFrom(in _runtimeGpuClockLoop);
			return true;
		}
		if (name == PropertyName._hasCachedRenderSnapshot)
		{
			value = VariantUtils.CreateFrom(in _hasCachedRenderSnapshot);
			return true;
		}
		if (name == PropertyName._loopTerminalAliasCacheRange)
		{
			value = VariantUtils.CreateFrom(in _loopTerminalAliasCacheRange);
			return true;
		}
		if (name == PropertyName._loopTerminalAliasCacheHasValue)
		{
			value = VariantUtils.CreateFrom(in _loopTerminalAliasCacheHasValue);
			return true;
		}
		if (name == PropertyName._loopTerminalAliasCacheValue)
		{
			value = VariantUtils.CreateFrom(in _loopTerminalAliasCacheValue);
			return true;
		}
		if (name == PropertyName._playbackClipEndCacheRange)
		{
			value = VariantUtils.CreateFrom(in _playbackClipEndCacheRange);
			return true;
		}
		if (name == PropertyName._playbackClipEndCacheLoop)
		{
			value = VariantUtils.CreateFrom(in _playbackClipEndCacheLoop);
			return true;
		}
		if (name == PropertyName._playbackClipEndCachePlayBack)
		{
			value = VariantUtils.CreateFrom(in _playbackClipEndCachePlayBack);
			return true;
		}
		if (name == PropertyName._playbackClipEndCacheHasValue)
		{
			value = VariantUtils.CreateFrom(in _playbackClipEndCacheHasValue);
			return true;
		}
		if (name == PropertyName._playbackClipEndCacheValue)
		{
			value = VariantUtils.CreateFrom(in _playbackClipEndCacheValue);
			return true;
		}
		if (name == PropertyName._renderOrderDebugPending)
		{
			value = VariantUtils.CreateFrom(in _renderOrderDebugPending);
			return true;
		}
		if (name == PropertyName._renderOrderDebugReason)
		{
			value = VariantUtils.CreateFrom(in _renderOrderDebugReason);
			return true;
		}
		if (name == PropertyName._cachedOwnRenderFrameModulateVersion)
		{
			value = VariantUtils.CreateFrom(in _cachedOwnRenderFrameModulateVersion);
			return true;
		}
		if (name == PropertyName._cachedOwnRenderFrameModulate)
		{
			value = VariantUtils.CreateFrom(in _cachedOwnRenderFrameModulate);
			return true;
		}
		if (name == PropertyName._cachedOwnRenderFrameSelfModulateVersion)
		{
			value = VariantUtils.CreateFrom(in _cachedOwnRenderFrameSelfModulateVersion);
			return true;
		}
		if (name == PropertyName._cachedOwnRenderFrameSelfModulate)
		{
			value = VariantUtils.CreateFrom(in _cachedOwnRenderFrameSelfModulate);
			return true;
		}
		if (name == PropertyName._cachedRenderLocalModulateReady)
		{
			value = VariantUtils.CreateFrom(in _cachedRenderLocalModulateReady);
			return true;
		}
		if (name == PropertyName._cachedRenderLocalModulate)
		{
			value = VariantUtils.CreateFrom(in _cachedRenderLocalModulate);
			return true;
		}
		if (name == PropertyName._cachedRenderLocalSelfModulateReady)
		{
			value = VariantUtils.CreateFrom(in _cachedRenderLocalSelfModulateReady);
			return true;
		}
		if (name == PropertyName._cachedRenderLocalSelfModulate)
		{
			value = VariantUtils.CreateFrom(in _cachedRenderLocalSelfModulate);
			return true;
		}
		if (name == PropertyName._renderLocalModulateRescanCountdown)
		{
			value = VariantUtils.CreateFrom(in _renderLocalModulateRescanCountdown);
			return true;
		}
		if (name == PropertyName._renderLocalModulateAuditTransactionVersion)
		{
			value = VariantUtils.CreateFrom(in _renderLocalModulateAuditTransactionVersion);
			return true;
		}
		if (name == PropertyName._runtimeRenderBoundsCached)
		{
			value = VariantUtils.CreateFrom(in _runtimeRenderBoundsCached);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsVisible)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeRenderBoundsVisible);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsStrictVisible)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeRenderBoundsStrictVisible);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsCullingEnabled)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeRenderBoundsCullingEnabled);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsTransform)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeRenderBoundsTransform);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsOffset)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeRenderBoundsOffset);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsMountParent)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeRenderBoundsMountParent);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsViewport)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeRenderBoundsViewport);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsViewportWorldRect)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeRenderBoundsViewportWorldRect);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsRelativeRect)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeRenderBoundsRelativeRect);
			return true;
		}
		if (name == PropertyName._cachedRuntimeRenderBoundsHasRelativeRect)
		{
			value = VariantUtils.CreateFrom(in _cachedRuntimeRenderBoundsHasRelativeRect);
			return true;
		}
		if (name == PropertyName._runtimeRenderBoundsRescanCountdown)
		{
			value = VariantUtils.CreateFrom(in _runtimeRenderBoundsRescanCountdown);
			return true;
		}
		if (name == PropertyName._externalVisualPreparationFrame)
		{
			value = VariantUtils.CreateFrom(in _externalVisualPreparationFrame);
			return true;
		}
		if (name == PropertyName._externalVisualGpuStateAtlasRids)
		{
			value = VariantUtils.CreateFrom(in _externalVisualGpuStateAtlasRids);
			return true;
		}
		if (name == PropertyName._externalVisualGpuStateFailures)
		{
			value = VariantUtils.CreateFrom(in _externalVisualGpuStateFailures);
			return true;
		}
		if (name == PropertyName._staticAtlasPathLayoutRuntimeMutationSeen)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathLayoutRuntimeMutationSeen);
			return true;
		}
		if (name == PropertyName._bypassStaticAtlasPathLayoutCacheForTests)
		{
			value = VariantUtils.CreateFrom(in _bypassStaticAtlasPathLayoutCacheForTests);
			return true;
		}
		if (name == PropertyName._staticAtlasPathLayoutGpuResetPending)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathLayoutGpuResetPending);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotData)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotData);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotDataInstanceId)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotDataInstanceId);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotAuthoringRevision)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotAuthoringRevision);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotMediaCount)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotMediaCount);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotTextureCount)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotTextureCount);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotUseCount)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotUseCount);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotPathCount)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotPathCount);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotEffectiveLimit)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotEffectiveLimit);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotActiveCount)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotActiveCount);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotSlotSignature)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotSlotSignature);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotPaths)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotPaths);
			return true;
		}
		if (name == PropertyName._staticAtlasPathSnapshotValid)
		{
			value = VariantUtils.CreateFrom(in _staticAtlasPathSnapshotValid);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName._effectOnceBatchVisualRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._effectOnceBatchEligibilityRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName.Modulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName.SelfModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ZIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ZAsRelative, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._flashAnimeData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._flashAnimeDataChangePending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._atlasProfileOverride, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.atlasProfileOverride, PropertyHint.ResourceType, "AdobeAnimateAtlasProfile", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.flashAnimeData, PropertyHint.ResourceType, "AdobeAnimateData", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._preview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.preview, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._forceLocalRender, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renderClipControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.forceLocalRender, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._forceCpuPoseRender, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.forceCpuPoseRender, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.suppressEditorRenderSubmission, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Nil, "Preset", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._invisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.invisible, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.normalAlpha, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._offset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.offset, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.offsetRotate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName._timeScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.timeScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.trueFrameRate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.refreshEveryFlame, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.skipLastFrame, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._deduplicateLoopTerminalFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.deduplicateLoopTerminalFrame, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.usePos, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useRotate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.useFollowVisible, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "Animation", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.blendTimeInit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Group, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._pause, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._playBack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._playbackBlocked, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._parentPlaybackStopped, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._frozenPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._effectOnceGpuSuppressed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._effectOnceGpuSuppressionToken, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._applyingImmediateAnimation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsFrozenPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.pause, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsPlaybackStopped, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.keepRenderSubmittedWhenPaused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsEffectOnceGpuSuppressed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeViewportCullingEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.runtimeViewportCullingEnabled, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.playBack, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.onlyDraw, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.elapsedTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.refreshTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.blend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.blendTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.blendTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._blendFromFrameFloat, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.frameIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.frameRate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.refreshEveryFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.loop, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._clip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._playbackRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._managedPoseRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ManagedPoseRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.clip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName.clipRange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.clipOver, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._layerVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._effectOnceBatchArrayExposureRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._effectOnceBatchEligibilityTracking, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.layerVisible, PropertyHint.TypeString, "1/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.mediaReplaceAtlas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mediaReplaceAtlasArray, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.mediaReplaceAtlasArraySize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mediaReplaceAtlasUsesTextureArray, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.mediaReplaceRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.mediaReplaceAtlasPages, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._mediaReplace, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.mediaReplace, PropertyHint.TypeString, "24/17:Texture2D", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName._mediaReplaceUse, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.mediaReplaceUse, PropertyHint.TypeString, "1/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Array, PropertyName._mediaReplaceAtlasPaths, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName.mediaReplaceAtlasPaths, PropertyHint.TypeString, "4/0:", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.mediaReplaceAtlasShared, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rid, PropertyName.canvasItem, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rid, PropertyName.meshTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.mesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.needMediaReplaceUpdate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._canRun, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.initClip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._parentSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._parentSpriteResolved, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.parentSprite, PropertyHint.NodeType, "AdobeAnimateSprite", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.followParentSpriteLayerId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.insertLayerId, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName._meshColor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._renderColorMultiplier, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeGpuGraphActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasCachedGpuGraphCrowdState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedGpuGraphCrowdStatePhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasCachedRasterCompositeRenderState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._gpuGraphCrowdStaticStateDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._gpuGraphCrowdPresentationStateDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphCrowdStateNextAuditPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderGrayscale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName.meshColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._autoInserting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._slotChildren, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._runtimeSlotChildren, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._managedSlotGpuStateAtlasRids, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._managedSlotGpuStateRescanCountdowns, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._managedSlotGpuStateWatchedSprites, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._managedSlotGpuStateWatchedParts, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeSlotChildrenDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeHasChildUpdates, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeChildUpdatesVisualOnlyDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeChildUpdatesVisualOnlyCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._managedSlotSpritesDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._spriteChildren, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._spriteChildOwnerSlots, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasChildren, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._runtimeChildViewportFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeChildViewportVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastUpdateFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isVisibleInTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeInsideTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._processEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._usingRuntimeManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedInstanceIdForRender, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphPreparedGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphPreparedSignature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphPreparedQuadCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dispatchingBatchedProcess, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._displayFrameVisualDispatchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._runtimeDisplayTickVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeReadyCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeNativeCanvasSuppressed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._runtimeNativeCanvasPublishedFrameVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeManagerFirstFrameHandoffActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._runtimeNativeCanvasVisibilityLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeNativeCanvasSuppressionEligibilityKnown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeNativeCanvasSuppressionEligible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeParentStateSource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._runtimeParentStateValidationCountdown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeFollowVisibilityKnown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeFollowVisibilityValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._needsRenderSubmission, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeRenderSubmissionRetryRequested, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeGpuPrepareRequired, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._visibilityParentCanvas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._selfVisibilityChangedConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._parentVisibilityChangedConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cachedRenderMountParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cachedViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renderMountNextAuditPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renderModulateMountParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderModulateAncestorCacheReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renderModulateAncestorRescanCountdown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedCanvasLayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedManagedPoseTrackKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedManagedPoseTrackUseLayerId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._nextAnimDelayTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderStateCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderStaticStateCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderStaticLayerArrayEscaped, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderStaticMediaArraysEscaped, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderStaticArrayAuditSubmissionPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renderStaticStateNextAuditPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._renderStaticAuditLayerVisibleSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._renderStaticAuditMediaReplaceUseSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._renderStaticAuditMediaReplaceSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._renderStaticAuditMediaReplaceRectSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._renderStaticAuditMediaReplacePageSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._renderStaticAuditMediaReplaceAtlasPathSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedGpuGraphRootOwnerPlaybackRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasCachedGpuGraphNestedOwnerState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedGpuGraphNestedOwnerPlaybackRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._gpuGraphNestedOwnerStateRebaseClockSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._cachedClip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._cachedClipRange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cachedFrameFloat, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._cachedGlobalTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._renderGlobalTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._renderPreviousGlobalTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderGlobalTransformCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderGlobalTransformDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderTransformChangedInPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimePhysicsAncestorTransformPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._knownAncestorTranslationNotificationPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderRootMotionEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderTreePaused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renderGlobalTransformPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._cachedModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._cachedOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cachedMediaReplaceAtlas, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cachedMediaReplaceAtlasArray, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rid, PropertyName._cachedMediaReplaceAtlasRid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rid, PropertyName._cachedMediaReplaceAtlasArrayRid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedMediaReplaceAtlasShared, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedMediaReplaceAtlasUsesTextureArray, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cachedRenderMountParentSubmit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedCanvasLayerSubmit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedZIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedEffectiveZIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._effectiveZIndexCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._effectiveZIndexCacheLocalZ, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._effectiveZIndexCacheZAsRelative, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._effectiveZIndexRescanCountdown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._effectOnceBatchRenderTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._effectOnceBatchEffectiveZIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._effectOnceBatchRenderRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._effectOnceBatchObservedLocalZIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._effectOnceBatchObservedZAsRelative, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._effectOnceBatchRenderNextAuditFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._effectOnceBatchRenderStateDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedEffectiveRenderSortBand, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._effectiveRenderSortBandCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._effectiveRenderSortBandCacheLocalSortBand, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._effectiveRenderSortBandRescanCountdown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._cachedEffectiveTreeOrderPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._treeOrderPathRescanCountdown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._treeOrderWatchedParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renderSortBand, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedSortBand, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedUseDrawOrderSortBands, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._drawOrderSortBandCacheDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._needsDrawOrderSortBandsCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedRefreshEveryFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedLayerMask, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedAllLayersVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedConfiguredLayersAllVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedCanUseLayerMask, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedLayerVisibleCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedLayerVisibleValueCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedLayerVisibleSignature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cachedRasterCompositeSequenceData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._cachedRasterCompositeSequenceClip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._cachedRasterCompositeSequenceRange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedRasterCompositeSequenceLayerSignature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedRasterCompositeSequenceTileBase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedRasterCompositeSequenceClipStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedRasterCompositeSequenceFrameCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedRasterCompositeSequenceValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._mediaReplaceStateVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedMediaReplaceStateVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedMediaReplaceLimit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedHasMediaReplace, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedAnyRequestedMediaReplace, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedMediaReplaceUseMask, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedMediaReplaceUseMaskOverflow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedMediaReplaceAtlasPageCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._cachedMediaReplaceAtlasPageValues, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedMediaReplaceSignature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cachedMediaReplaceBoundsGrow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._layerStateVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedLayerStateVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeManagerDispatchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._displayFrameGpuClockPoseReusable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeDisplayVisualRequirementCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedRuntimeDisplayVisualRequirement, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedRuntimeDisplayGpuClockPoseReusable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._runtimeDisplayVisualRequirementValidationCountdown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeGpuClockPoseReuseCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedRuntimeGpuClockPoseReusable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._runtimeGpuClockPoseReuseValidationCountdown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeGpuClockInterpolationActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._runtimeGpuClockFramesPerSecond, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._runtimeGpuClockClipStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._runtimeGpuClockClipEndExclusive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeGpuClockLoop, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasCachedRenderSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._loopTerminalAliasCacheRange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._loopTerminalAliasCacheHasValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._loopTerminalAliasCacheValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._playbackClipEndCacheRange, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._playbackClipEndCacheLoop, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._playbackClipEndCachePlayBack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._playbackClipEndCacheHasValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._playbackClipEndCacheValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderOrderDebugPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._renderOrderDebugReason, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedOwnRenderFrameModulateVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._cachedOwnRenderFrameModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cachedOwnRenderFrameSelfModulateVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._cachedOwnRenderFrameSelfModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedRenderLocalModulateReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._cachedRenderLocalModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedRenderLocalSelfModulateReady, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Color, PropertyName._cachedRenderLocalSelfModulate, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renderLocalModulateRescanCountdown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renderLocalModulateAuditTransactionVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeRenderBoundsCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedRuntimeRenderBoundsVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedRuntimeRenderBoundsStrictVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedRuntimeRenderBoundsCullingEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Transform2D, PropertyName._cachedRuntimeRenderBoundsTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._cachedRuntimeRenderBoundsOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cachedRuntimeRenderBoundsMountParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cachedRuntimeRenderBoundsViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._cachedRuntimeRenderBoundsViewportWorldRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName._cachedRuntimeRenderBoundsRelativeRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._cachedRuntimeRenderBoundsHasRelativeRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._runtimeRenderBoundsRescanCountdown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsRuntimeActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsRuntimeTickPaused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsRuntimeInsideTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsRuntimeVisibleInTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsRuntimeReadyCached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsRuntimeDisplayTickActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.NeedsRuntimeRenderSubmission, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UsesRuntimeGpuClockInterpolation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.PrefersCachedGpuGraphCrowdRenderState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RuntimeManagerDispatchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ShouldDispatchProcessCallback, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RequiresDisplayFrameBatchedProcessExtension, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._externalVisualPreparationFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._externalVisualGpuStateAtlasRids, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._externalVisualGpuStateFailures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._staticAtlasPathLayoutRuntimeMutationSeen, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._bypassStaticAtlasPathLayoutCacheForTests, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._staticAtlasPathLayoutGpuResetPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._staticAtlasPathSnapshotData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._staticAtlasPathSnapshotDataInstanceId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._staticAtlasPathSnapshotAuthoringRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._staticAtlasPathSnapshotMediaCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._staticAtlasPathSnapshotTextureCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._staticAtlasPathSnapshotUseCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._staticAtlasPathSnapshotPathCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._staticAtlasPathSnapshotEffectiveLimit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._staticAtlasPathSnapshotActiveCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._staticAtlasPathSnapshotSlotSignature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._staticAtlasPathSnapshotPaths, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._staticAtlasPathSnapshotValid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.NeedsRenderSubmissionForBareTest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PlaybackRevisionForBareTest, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Modulate, Variant.From<Color>(Modulate));
		info.AddProperty(PropertyName.SelfModulate, Variant.From<Color>(SelfModulate));
		info.AddProperty(PropertyName.ZIndex, Variant.From<int>(ZIndex));
		info.AddProperty(PropertyName.ZAsRelative, Variant.From<bool>(ZAsRelative));
		info.AddProperty(PropertyName.atlasProfileOverride, Variant.From<AdobeAnimateAtlasProfile>(atlasProfileOverride));
		info.AddProperty(PropertyName.flashAnimeData, Variant.From<AdobeAnimateData>(flashAnimeData));
		info.AddProperty(PropertyName.preview, Variant.From<bool>(preview));
		info.AddProperty(PropertyName.forceLocalRender, Variant.From<bool>(forceLocalRender));
		info.AddProperty(PropertyName.forceCpuPoseRender, Variant.From<bool>(forceCpuPoseRender));
		info.AddProperty(PropertyName.suppressEditorRenderSubmission, Variant.From<bool>(suppressEditorRenderSubmission));
		info.AddProperty(PropertyName.invisible, Variant.From<bool>(invisible));
		info.AddProperty(PropertyName.offset, Variant.From<Vector2>(offset));
		info.AddProperty(PropertyName.timeScale, Variant.From<double>(timeScale));
		info.AddProperty(PropertyName.deduplicateLoopTerminalFrame, Variant.From<bool>(deduplicateLoopTerminalFrame));
		info.AddProperty(PropertyName.pause, Variant.From<bool>(pause));
		info.AddProperty(PropertyName.runtimeViewportCullingEnabled, Variant.From<bool>(runtimeViewportCullingEnabled));
		info.AddProperty(PropertyName.playBack, Variant.From<bool>(playBack));
		info.AddProperty(PropertyName.clip, Variant.From<string>(clip));
		info.AddProperty(PropertyName.layerVisible, Variant.CreateFrom(layerVisible));
		info.AddProperty(PropertyName.mediaReplace, Variant.CreateFrom(mediaReplace));
		info.AddProperty(PropertyName.mediaReplaceUse, Variant.CreateFrom(mediaReplaceUse));
		info.AddProperty(PropertyName.mediaReplaceAtlasPaths, Variant.CreateFrom(mediaReplaceAtlasPaths));
		info.AddProperty(PropertyName.parentSprite, Variant.From<AdobeAnimateSprite>(parentSprite));
		info.AddProperty(PropertyName.meshColor, Variant.From<Color>(meshColor));
		info.AddProperty(PropertyName._effectOnceBatchVisualRevision, Variant.From(in _effectOnceBatchVisualRevision));
		info.AddProperty(PropertyName._effectOnceBatchEligibilityRevision, Variant.From(in _effectOnceBatchEligibilityRevision));
		info.AddProperty(PropertyName._flashAnimeData, Variant.From(in _flashAnimeData));
		info.AddProperty(PropertyName._flashAnimeDataChangePending, Variant.From(in _flashAnimeDataChangePending));
		info.AddProperty(PropertyName._atlasProfileOverride, Variant.From(in _atlasProfileOverride));
		info.AddProperty(PropertyName._preview, Variant.From(in _preview));
		info.AddProperty(PropertyName._forceLocalRender, Variant.From(in _forceLocalRender));
		info.AddProperty(PropertyName._renderClipControl, Variant.From(in _renderClipControl));
		info.AddProperty(PropertyName._forceCpuPoseRender, Variant.From(in _forceCpuPoseRender));
		info.AddProperty(PropertyName._invisible, Variant.From(in _invisible));
		info.AddProperty(PropertyName.normalAlpha, Variant.From(in normalAlpha));
		info.AddProperty(PropertyName._offset, Variant.From(in _offset));
		info.AddProperty(PropertyName.offsetRotate, Variant.From(in offsetRotate));
		info.AddProperty(PropertyName._timeScale, Variant.From(in _timeScale));
		info.AddProperty(PropertyName.trueFrameRate, Variant.From(in trueFrameRate));
		info.AddProperty(PropertyName.refreshEveryFlame, Variant.From(in refreshEveryFlame));
		info.AddProperty(PropertyName.skipLastFrame, Variant.From(in skipLastFrame));
		info.AddProperty(PropertyName._deduplicateLoopTerminalFrame, Variant.From(in _deduplicateLoopTerminalFrame));
		info.AddProperty(PropertyName.usePos, Variant.From(in usePos));
		info.AddProperty(PropertyName.useRotate, Variant.From(in useRotate));
		info.AddProperty(PropertyName.useFollowVisible, Variant.From(in useFollowVisible));
		info.AddProperty(PropertyName.blendTimeInit, Variant.From(in blendTimeInit));
		info.AddProperty(PropertyName._pause, Variant.From(in _pause));
		info.AddProperty(PropertyName._playBack, Variant.From(in _playBack));
		info.AddProperty(PropertyName._playbackBlocked, Variant.From(in _playbackBlocked));
		info.AddProperty(PropertyName._parentPlaybackStopped, Variant.From(in _parentPlaybackStopped));
		info.AddProperty(PropertyName._frozenPreview, Variant.From(in _frozenPreview));
		info.AddProperty(PropertyName._effectOnceGpuSuppressed, Variant.From(in _effectOnceGpuSuppressed));
		info.AddProperty(PropertyName._effectOnceGpuSuppressionToken, Variant.From(in _effectOnceGpuSuppressionToken));
		info.AddProperty(PropertyName._applyingImmediateAnimation, Variant.From(in _applyingImmediateAnimation));
		info.AddProperty(PropertyName.keepRenderSubmittedWhenPaused, Variant.From(in keepRenderSubmittedWhenPaused));
		info.AddProperty(PropertyName._runtimeViewportCullingEnabled, Variant.From(in _runtimeViewportCullingEnabled));
		info.AddProperty(PropertyName.onlyDraw, Variant.From(in onlyDraw));
		info.AddProperty(PropertyName.elapsedTimer, Variant.From(in elapsedTimer));
		info.AddProperty(PropertyName.refreshTimer, Variant.From(in refreshTimer));
		info.AddProperty(PropertyName.blend, Variant.From(in blend));
		info.AddProperty(PropertyName.blendTime, Variant.From(in blendTime));
		info.AddProperty(PropertyName.blendTimer, Variant.From(in blendTimer));
		info.AddProperty(PropertyName._blendFromFrameFloat, Variant.From(in _blendFromFrameFloat));
		info.AddProperty(PropertyName.frameIndex, Variant.From(in frameIndex));
		info.AddProperty(PropertyName.frameRate, Variant.From(in frameRate));
		info.AddProperty(PropertyName.refreshEveryFrame, Variant.From(in refreshEveryFrame));
		info.AddProperty(PropertyName.loop, Variant.From(in loop));
		info.AddProperty(PropertyName._clip, Variant.From(in _clip));
		info.AddProperty(PropertyName._playbackRevision, Variant.From(in _playbackRevision));
		info.AddProperty(PropertyName._managedPoseRevision, Variant.From(in _managedPoseRevision));
		info.AddProperty(PropertyName.clipRange, Variant.From(in clipRange));
		info.AddProperty(PropertyName.clipOver, Variant.From(in clipOver));
		info.AddProperty(PropertyName._layerVisible, Variant.CreateFrom(_layerVisible));
		info.AddProperty(PropertyName._effectOnceBatchArrayExposureRevision, Variant.From(in _effectOnceBatchArrayExposureRevision));
		info.AddProperty(PropertyName._effectOnceBatchEligibilityTracking, Variant.From(in _effectOnceBatchEligibilityTracking));
		info.AddProperty(PropertyName.mediaReplaceAtlas, Variant.From(in mediaReplaceAtlas));
		info.AddProperty(PropertyName.mediaReplaceAtlasArray, Variant.From(in mediaReplaceAtlasArray));
		info.AddProperty(PropertyName.mediaReplaceAtlasArraySize, Variant.From(in mediaReplaceAtlasArraySize));
		info.AddProperty(PropertyName.mediaReplaceAtlasUsesTextureArray, Variant.From(in mediaReplaceAtlasUsesTextureArray));
		info.AddProperty(PropertyName.mediaReplaceRect, Variant.CreateFrom(mediaReplaceRect));
		info.AddProperty(PropertyName.mediaReplaceAtlasPages, Variant.CreateFrom(mediaReplaceAtlasPages));
		info.AddProperty(PropertyName._mediaReplace, Variant.CreateFrom(_mediaReplace));
		info.AddProperty(PropertyName._mediaReplaceUse, Variant.CreateFrom(_mediaReplaceUse));
		info.AddProperty(PropertyName._mediaReplaceAtlasPaths, Variant.CreateFrom(_mediaReplaceAtlasPaths));
		info.AddProperty(PropertyName.mediaReplaceAtlasShared, Variant.From(in mediaReplaceAtlasShared));
		info.AddProperty(PropertyName.canvasItem, Variant.From(in canvasItem));
		info.AddProperty(PropertyName.meshTexture, Variant.From(in meshTexture));
		info.AddProperty(PropertyName.mesh, Variant.From(in mesh));
		info.AddProperty(PropertyName.needMediaReplaceUpdate, Variant.From(in needMediaReplaceUpdate));
		info.AddProperty(PropertyName._canRun, Variant.From(in _canRun));
		info.AddProperty(PropertyName.initClip, Variant.From(in initClip));
		info.AddProperty(PropertyName._parentSprite, Variant.From(in _parentSprite));
		info.AddProperty(PropertyName._parentSpriteResolved, Variant.From(in _parentSpriteResolved));
		info.AddProperty(PropertyName.followParentSpriteLayerId, Variant.From(in followParentSpriteLayerId));
		info.AddProperty(PropertyName.insertLayerId, Variant.From(in insertLayerId));
		info.AddProperty(PropertyName._meshColor, Variant.From(in _meshColor));
		info.AddProperty(PropertyName._renderColorMultiplier, Variant.From(in _renderColorMultiplier));
		info.AddProperty(PropertyName._runtimeGpuGraphActive, Variant.From(in _runtimeGpuGraphActive));
		info.AddProperty(PropertyName._hasCachedGpuGraphCrowdState, Variant.From(in _hasCachedGpuGraphCrowdState));
		info.AddProperty(PropertyName._cachedGpuGraphCrowdStatePhysicsFrame, Variant.From(in _cachedGpuGraphCrowdStatePhysicsFrame));
		info.AddProperty(PropertyName._hasCachedRasterCompositeRenderState, Variant.From(in _hasCachedRasterCompositeRenderState));
		info.AddProperty(PropertyName._gpuGraphCrowdStaticStateDirty, Variant.From(in _gpuGraphCrowdStaticStateDirty));
		info.AddProperty(PropertyName._gpuGraphCrowdPresentationStateDirty, Variant.From(in _gpuGraphCrowdPresentationStateDirty));
		info.AddProperty(PropertyName._gpuGraphCrowdStateNextAuditPhysicsFrame, Variant.From(in _gpuGraphCrowdStateNextAuditPhysicsFrame));
		info.AddProperty(PropertyName._renderGrayscale, Variant.From(in _renderGrayscale));
		info.AddProperty(PropertyName._autoInserting, Variant.From(in _autoInserting));
		StringName slotChildren = PropertyName._slotChildren;
		GodotObject[] slotChildren2 = _slotChildren;
		info.AddProperty(slotChildren, Variant.CreateFrom(slotChildren2));
		StringName runtimeSlotChildren = PropertyName._runtimeSlotChildren;
		slotChildren2 = _runtimeSlotChildren;
		info.AddProperty(runtimeSlotChildren, Variant.CreateFrom(slotChildren2));
		info.AddProperty(PropertyName._managedSlotGpuStateAtlasRids, Variant.From(in _managedSlotGpuStateAtlasRids));
		info.AddProperty(PropertyName._managedSlotGpuStateRescanCountdowns, Variant.From(in _managedSlotGpuStateRescanCountdowns));
		StringName managedSlotGpuStateWatchedSprites = PropertyName._managedSlotGpuStateWatchedSprites;
		slotChildren2 = _managedSlotGpuStateWatchedSprites;
		info.AddProperty(managedSlotGpuStateWatchedSprites, Variant.CreateFrom(slotChildren2));
		StringName managedSlotGpuStateWatchedParts = PropertyName._managedSlotGpuStateWatchedParts;
		slotChildren2 = _managedSlotGpuStateWatchedParts;
		info.AddProperty(managedSlotGpuStateWatchedParts, Variant.CreateFrom(slotChildren2));
		info.AddProperty(PropertyName._runtimeSlotChildrenDirty, Variant.From(in _runtimeSlotChildrenDirty));
		info.AddProperty(PropertyName._runtimeHasChildUpdates, Variant.From(in _runtimeHasChildUpdates));
		info.AddProperty(PropertyName._runtimeChildUpdatesVisualOnlyDirty, Variant.From(in _runtimeChildUpdatesVisualOnlyDirty));
		info.AddProperty(PropertyName._runtimeChildUpdatesVisualOnlyCached, Variant.From(in _runtimeChildUpdatesVisualOnlyCached));
		info.AddProperty(PropertyName._managedSlotSpritesDirty, Variant.From(in _managedSlotSpritesDirty));
		StringName spriteChildren = PropertyName._spriteChildren;
		slotChildren2 = _spriteChildren;
		info.AddProperty(spriteChildren, Variant.CreateFrom(slotChildren2));
		StringName spriteChildOwnerSlots = PropertyName._spriteChildOwnerSlots;
		slotChildren2 = _spriteChildOwnerSlots;
		info.AddProperty(spriteChildOwnerSlots, Variant.CreateFrom(slotChildren2));
		info.AddProperty(PropertyName._hasChildren, Variant.From(in _hasChildren));
		info.AddProperty(PropertyName._runtimeChildViewportFrame, Variant.From(in _runtimeChildViewportFrame));
		info.AddProperty(PropertyName._runtimeChildViewportVisible, Variant.From(in _runtimeChildViewportVisible));
		info.AddProperty(PropertyName._lastUpdateFrame, Variant.From(in _lastUpdateFrame));
		info.AddProperty(PropertyName._isVisibleInTree, Variant.From(in _isVisibleInTree));
		info.AddProperty(PropertyName._runtimeInsideTree, Variant.From(in _runtimeInsideTree));
		info.AddProperty(PropertyName._processEnabled, Variant.From(in _processEnabled));
		info.AddProperty(PropertyName._usingRuntimeManager, Variant.From(in _usingRuntimeManager));
		info.AddProperty(PropertyName._cachedInstanceIdForRender, Variant.From(in _cachedInstanceIdForRender));
		info.AddProperty(PropertyName._gpuGraphPreparedGeneration, Variant.From(in _gpuGraphPreparedGeneration));
		info.AddProperty(PropertyName._gpuGraphPreparedSignature, Variant.From(in _gpuGraphPreparedSignature));
		info.AddProperty(PropertyName._gpuGraphPreparedQuadCount, Variant.From(in _gpuGraphPreparedQuadCount));
		info.AddProperty(PropertyName._dispatchingBatchedProcess, Variant.From(in _dispatchingBatchedProcess));
		info.AddProperty(PropertyName._displayFrameVisualDispatchActive, Variant.From(in _displayFrameVisualDispatchActive));
		info.AddProperty(PropertyName._runtimeDisplayTickVersion, Variant.From(in _runtimeDisplayTickVersion));
		info.AddProperty(PropertyName._runtimeReadyCached, Variant.From(in _runtimeReadyCached));
		info.AddProperty(PropertyName._runtimeNativeCanvasSuppressed, Variant.From(in _runtimeNativeCanvasSuppressed));
		info.AddProperty(PropertyName._runtimeNativeCanvasPublishedFrameVersion, Variant.From(in _runtimeNativeCanvasPublishedFrameVersion));
		info.AddProperty(PropertyName._runtimeManagerFirstFrameHandoffActive, Variant.From(in _runtimeManagerFirstFrameHandoffActive));
		info.AddProperty(PropertyName._runtimeNativeCanvasVisibilityLayer, Variant.From(in _runtimeNativeCanvasVisibilityLayer));
		info.AddProperty(PropertyName._runtimeNativeCanvasSuppressionEligibilityKnown, Variant.From(in _runtimeNativeCanvasSuppressionEligibilityKnown));
		info.AddProperty(PropertyName._runtimeNativeCanvasSuppressionEligible, Variant.From(in _runtimeNativeCanvasSuppressionEligible));
		info.AddProperty(PropertyName._runtimeParentStateSource, Variant.From(in _runtimeParentStateSource));
		info.AddProperty(PropertyName._runtimeParentStateValidationCountdown, Variant.From(in _runtimeParentStateValidationCountdown));
		info.AddProperty(PropertyName._runtimeFollowVisibilityKnown, Variant.From(in _runtimeFollowVisibilityKnown));
		info.AddProperty(PropertyName._runtimeFollowVisibilityValue, Variant.From(in _runtimeFollowVisibilityValue));
		info.AddProperty(PropertyName._needsRenderSubmission, Variant.From(in _needsRenderSubmission));
		info.AddProperty(PropertyName._runtimeRenderSubmissionRetryRequested, Variant.From(in _runtimeRenderSubmissionRetryRequested));
		info.AddProperty(PropertyName._runtimeGpuPrepareRequired, Variant.From(in _runtimeGpuPrepareRequired));
		info.AddProperty(PropertyName._visibilityParentCanvas, Variant.From(in _visibilityParentCanvas));
		info.AddProperty(PropertyName._selfVisibilityChangedConnected, Variant.From(in _selfVisibilityChangedConnected));
		info.AddProperty(PropertyName._parentVisibilityChangedConnected, Variant.From(in _parentVisibilityChangedConnected));
		info.AddProperty(PropertyName._cachedRenderMountParent, Variant.From(in _cachedRenderMountParent));
		info.AddProperty(PropertyName._cachedViewport, Variant.From(in _cachedViewport));
		info.AddProperty(PropertyName._renderMountNextAuditPhysicsFrame, Variant.From(in _renderMountNextAuditPhysicsFrame));
		info.AddProperty(PropertyName._renderModulateMountParent, Variant.From(in _renderModulateMountParent));
		info.AddProperty(PropertyName._renderModulateAncestorCacheReady, Variant.From(in _renderModulateAncestorCacheReady));
		info.AddProperty(PropertyName._renderModulateAncestorRescanCountdown, Variant.From(in _renderModulateAncestorRescanCountdown));
		info.AddProperty(PropertyName._cachedCanvasLayer, Variant.From(in _cachedCanvasLayer));
		info.AddProperty(PropertyName._cachedManagedPoseTrackKey, Variant.From(in _cachedManagedPoseTrackKey));
		info.AddProperty(PropertyName._cachedManagedPoseTrackUseLayerId, Variant.From(in _cachedManagedPoseTrackUseLayerId));
		info.AddProperty(PropertyName._nextAnimDelayTimer, Variant.From(in _nextAnimDelayTimer));
		info.AddProperty(PropertyName._renderStateCached, Variant.From(in _renderStateCached));
		info.AddProperty(PropertyName._renderStaticStateCached, Variant.From(in _renderStaticStateCached));
		info.AddProperty(PropertyName._renderStaticLayerArrayEscaped, Variant.From(in _renderStaticLayerArrayEscaped));
		info.AddProperty(PropertyName._renderStaticMediaArraysEscaped, Variant.From(in _renderStaticMediaArraysEscaped));
		info.AddProperty(PropertyName._renderStaticArrayAuditSubmissionPending, Variant.From(in _renderStaticArrayAuditSubmissionPending));
		info.AddProperty(PropertyName._renderStaticStateNextAuditPhysicsFrame, Variant.From(in _renderStaticStateNextAuditPhysicsFrame));
		info.AddProperty(PropertyName._renderStaticAuditLayerVisibleSnapshot, Variant.CreateFrom(_renderStaticAuditLayerVisibleSnapshot));
		info.AddProperty(PropertyName._renderStaticAuditMediaReplaceUseSnapshot, Variant.CreateFrom(_renderStaticAuditMediaReplaceUseSnapshot));
		info.AddProperty(PropertyName._renderStaticAuditMediaReplaceSnapshot, Variant.CreateFrom(_renderStaticAuditMediaReplaceSnapshot));
		info.AddProperty(PropertyName._renderStaticAuditMediaReplaceRectSnapshot, Variant.CreateFrom(_renderStaticAuditMediaReplaceRectSnapshot));
		info.AddProperty(PropertyName._renderStaticAuditMediaReplacePageSnapshot, Variant.CreateFrom(_renderStaticAuditMediaReplacePageSnapshot));
		info.AddProperty(PropertyName._renderStaticAuditMediaReplaceAtlasPathSnapshot, Variant.CreateFrom(_renderStaticAuditMediaReplaceAtlasPathSnapshot));
		info.AddProperty(PropertyName._cachedGpuGraphRootOwnerPlaybackRevision, Variant.From(in _cachedGpuGraphRootOwnerPlaybackRevision));
		info.AddProperty(PropertyName._hasCachedGpuGraphNestedOwnerState, Variant.From(in _hasCachedGpuGraphNestedOwnerState));
		info.AddProperty(PropertyName._cachedGpuGraphNestedOwnerPlaybackRevision, Variant.From(in _cachedGpuGraphNestedOwnerPlaybackRevision));
		info.AddProperty(PropertyName._gpuGraphNestedOwnerStateRebaseClockSeconds, Variant.From(in _gpuGraphNestedOwnerStateRebaseClockSeconds));
		info.AddProperty(PropertyName._cachedClip, Variant.From(in _cachedClip));
		info.AddProperty(PropertyName._cachedClipRange, Variant.From(in _cachedClipRange));
		info.AddProperty(PropertyName._cachedFrameFloat, Variant.From(in _cachedFrameFloat));
		info.AddProperty(PropertyName._cachedGlobalTransform, Variant.From(in _cachedGlobalTransform));
		info.AddProperty(PropertyName._renderGlobalTransform, Variant.From(in _renderGlobalTransform));
		info.AddProperty(PropertyName._renderPreviousGlobalTransform, Variant.From(in _renderPreviousGlobalTransform));
		info.AddProperty(PropertyName._renderGlobalTransformCached, Variant.From(in _renderGlobalTransformCached));
		info.AddProperty(PropertyName._renderGlobalTransformDirty, Variant.From(in _renderGlobalTransformDirty));
		info.AddProperty(PropertyName._renderTransformChangedInPhysicsFrame, Variant.From(in _renderTransformChangedInPhysicsFrame));
		info.AddProperty(PropertyName._runtimePhysicsAncestorTransformPending, Variant.From(in _runtimePhysicsAncestorTransformPending));
		info.AddProperty(PropertyName._knownAncestorTranslationNotificationPending, Variant.From(in _knownAncestorTranslationNotificationPending));
		info.AddProperty(PropertyName._renderRootMotionEnabled, Variant.From(in _renderRootMotionEnabled));
		info.AddProperty(PropertyName._renderTreePaused, Variant.From(in _renderTreePaused));
		info.AddProperty(PropertyName._renderGlobalTransformPhysicsFrame, Variant.From(in _renderGlobalTransformPhysicsFrame));
		info.AddProperty(PropertyName._cachedModulate, Variant.From(in _cachedModulate));
		info.AddProperty(PropertyName._cachedOffset, Variant.From(in _cachedOffset));
		info.AddProperty(PropertyName._cachedMediaReplaceAtlas, Variant.From(in _cachedMediaReplaceAtlas));
		info.AddProperty(PropertyName._cachedMediaReplaceAtlasArray, Variant.From(in _cachedMediaReplaceAtlasArray));
		info.AddProperty(PropertyName._cachedMediaReplaceAtlasRid, Variant.From(in _cachedMediaReplaceAtlasRid));
		info.AddProperty(PropertyName._cachedMediaReplaceAtlasArrayRid, Variant.From(in _cachedMediaReplaceAtlasArrayRid));
		info.AddProperty(PropertyName._cachedMediaReplaceAtlasShared, Variant.From(in _cachedMediaReplaceAtlasShared));
		info.AddProperty(PropertyName._cachedMediaReplaceAtlasUsesTextureArray, Variant.From(in _cachedMediaReplaceAtlasUsesTextureArray));
		info.AddProperty(PropertyName._cachedRenderMountParentSubmit, Variant.From(in _cachedRenderMountParentSubmit));
		info.AddProperty(PropertyName._cachedCanvasLayerSubmit, Variant.From(in _cachedCanvasLayerSubmit));
		info.AddProperty(PropertyName._cachedZIndex, Variant.From(in _cachedZIndex));
		info.AddProperty(PropertyName._cachedEffectiveZIndex, Variant.From(in _cachedEffectiveZIndex));
		info.AddProperty(PropertyName._effectiveZIndexCached, Variant.From(in _effectiveZIndexCached));
		info.AddProperty(PropertyName._effectiveZIndexCacheLocalZ, Variant.From(in _effectiveZIndexCacheLocalZ));
		info.AddProperty(PropertyName._effectiveZIndexCacheZAsRelative, Variant.From(in _effectiveZIndexCacheZAsRelative));
		info.AddProperty(PropertyName._effectiveZIndexRescanCountdown, Variant.From(in _effectiveZIndexRescanCountdown));
		info.AddProperty(PropertyName._effectOnceBatchRenderTransform, Variant.From(in _effectOnceBatchRenderTransform));
		info.AddProperty(PropertyName._effectOnceBatchEffectiveZIndex, Variant.From(in _effectOnceBatchEffectiveZIndex));
		info.AddProperty(PropertyName._effectOnceBatchRenderRevision, Variant.From(in _effectOnceBatchRenderRevision));
		info.AddProperty(PropertyName._effectOnceBatchObservedLocalZIndex, Variant.From(in _effectOnceBatchObservedLocalZIndex));
		info.AddProperty(PropertyName._effectOnceBatchObservedZAsRelative, Variant.From(in _effectOnceBatchObservedZAsRelative));
		info.AddProperty(PropertyName._effectOnceBatchRenderNextAuditFrame, Variant.From(in _effectOnceBatchRenderNextAuditFrame));
		info.AddProperty(PropertyName._effectOnceBatchRenderStateDirty, Variant.From(in _effectOnceBatchRenderStateDirty));
		info.AddProperty(PropertyName._cachedEffectiveRenderSortBand, Variant.From(in _cachedEffectiveRenderSortBand));
		info.AddProperty(PropertyName._effectiveRenderSortBandCached, Variant.From(in _effectiveRenderSortBandCached));
		info.AddProperty(PropertyName._effectiveRenderSortBandCacheLocalSortBand, Variant.From(in _effectiveRenderSortBandCacheLocalSortBand));
		info.AddProperty(PropertyName._effectiveRenderSortBandRescanCountdown, Variant.From(in _effectiveRenderSortBandRescanCountdown));
		info.AddProperty(PropertyName._cachedEffectiveTreeOrderPath, Variant.From(in _cachedEffectiveTreeOrderPath));
		info.AddProperty(PropertyName._treeOrderPathRescanCountdown, Variant.From(in _treeOrderPathRescanCountdown));
		info.AddProperty(PropertyName._treeOrderWatchedParent, Variant.From(in _treeOrderWatchedParent));
		info.AddProperty(PropertyName._renderSortBand, Variant.From(in _renderSortBand));
		info.AddProperty(PropertyName._cachedSortBand, Variant.From(in _cachedSortBand));
		info.AddProperty(PropertyName._cachedUseDrawOrderSortBands, Variant.From(in _cachedUseDrawOrderSortBands));
		info.AddProperty(PropertyName._drawOrderSortBandCacheDirty, Variant.From(in _drawOrderSortBandCacheDirty));
		info.AddProperty(PropertyName._needsDrawOrderSortBandsCached, Variant.From(in _needsDrawOrderSortBandsCached));
		info.AddProperty(PropertyName._cachedRefreshEveryFrame, Variant.From(in _cachedRefreshEveryFrame));
		info.AddProperty(PropertyName._cachedLayerMask, Variant.From(in _cachedLayerMask));
		info.AddProperty(PropertyName._cachedAllLayersVisible, Variant.From(in _cachedAllLayersVisible));
		info.AddProperty(PropertyName._cachedConfiguredLayersAllVisible, Variant.From(in _cachedConfiguredLayersAllVisible));
		info.AddProperty(PropertyName._cachedCanUseLayerMask, Variant.From(in _cachedCanUseLayerMask));
		info.AddProperty(PropertyName._cachedLayerVisibleCount, Variant.From(in _cachedLayerVisibleCount));
		info.AddProperty(PropertyName._cachedLayerVisibleValueCount, Variant.From(in _cachedLayerVisibleValueCount));
		info.AddProperty(PropertyName._cachedLayerVisibleSignature, Variant.From(in _cachedLayerVisibleSignature));
		info.AddProperty(PropertyName._cachedRasterCompositeSequenceData, Variant.From(in _cachedRasterCompositeSequenceData));
		info.AddProperty(PropertyName._cachedRasterCompositeSequenceClip, Variant.From(in _cachedRasterCompositeSequenceClip));
		info.AddProperty(PropertyName._cachedRasterCompositeSequenceRange, Variant.From(in _cachedRasterCompositeSequenceRange));
		info.AddProperty(PropertyName._cachedRasterCompositeSequenceLayerSignature, Variant.From(in _cachedRasterCompositeSequenceLayerSignature));
		info.AddProperty(PropertyName._cachedRasterCompositeSequenceTileBase, Variant.From(in _cachedRasterCompositeSequenceTileBase));
		info.AddProperty(PropertyName._cachedRasterCompositeSequenceClipStart, Variant.From(in _cachedRasterCompositeSequenceClipStart));
		info.AddProperty(PropertyName._cachedRasterCompositeSequenceFrameCount, Variant.From(in _cachedRasterCompositeSequenceFrameCount));
		info.AddProperty(PropertyName._cachedRasterCompositeSequenceValid, Variant.From(in _cachedRasterCompositeSequenceValid));
		info.AddProperty(PropertyName._mediaReplaceStateVersion, Variant.From(in _mediaReplaceStateVersion));
		info.AddProperty(PropertyName._cachedMediaReplaceStateVersion, Variant.From(in _cachedMediaReplaceStateVersion));
		info.AddProperty(PropertyName._cachedMediaReplaceLimit, Variant.From(in _cachedMediaReplaceLimit));
		info.AddProperty(PropertyName._cachedHasMediaReplace, Variant.From(in _cachedHasMediaReplace));
		info.AddProperty(PropertyName._cachedAnyRequestedMediaReplace, Variant.From(in _cachedAnyRequestedMediaReplace));
		info.AddProperty(PropertyName._cachedMediaReplaceUseMask, Variant.From(in _cachedMediaReplaceUseMask));
		info.AddProperty(PropertyName._cachedMediaReplaceUseMaskOverflow, Variant.From(in _cachedMediaReplaceUseMaskOverflow));
		info.AddProperty(PropertyName._cachedMediaReplaceAtlasPageCount, Variant.From(in _cachedMediaReplaceAtlasPageCount));
		info.AddProperty(PropertyName._cachedMediaReplaceAtlasPageValues, Variant.From(in _cachedMediaReplaceAtlasPageValues));
		info.AddProperty(PropertyName._cachedMediaReplaceSignature, Variant.From(in _cachedMediaReplaceSignature));
		info.AddProperty(PropertyName._cachedMediaReplaceBoundsGrow, Variant.From(in _cachedMediaReplaceBoundsGrow));
		info.AddProperty(PropertyName._layerStateVersion, Variant.From(in _layerStateVersion));
		info.AddProperty(PropertyName._cachedLayerStateVersion, Variant.From(in _cachedLayerStateVersion));
		info.AddProperty(PropertyName._runtimeManagerDispatchActive, Variant.From(in _runtimeManagerDispatchActive));
		info.AddProperty(PropertyName._displayFrameGpuClockPoseReusable, Variant.From(in _displayFrameGpuClockPoseReusable));
		info.AddProperty(PropertyName._runtimeDisplayVisualRequirementCached, Variant.From(in _runtimeDisplayVisualRequirementCached));
		info.AddProperty(PropertyName._cachedRuntimeDisplayVisualRequirement, Variant.From(in _cachedRuntimeDisplayVisualRequirement));
		info.AddProperty(PropertyName._cachedRuntimeDisplayGpuClockPoseReusable, Variant.From(in _cachedRuntimeDisplayGpuClockPoseReusable));
		info.AddProperty(PropertyName._runtimeDisplayVisualRequirementValidationCountdown, Variant.From(in _runtimeDisplayVisualRequirementValidationCountdown));
		info.AddProperty(PropertyName._runtimeGpuClockPoseReuseCached, Variant.From(in _runtimeGpuClockPoseReuseCached));
		info.AddProperty(PropertyName._cachedRuntimeGpuClockPoseReusable, Variant.From(in _cachedRuntimeGpuClockPoseReusable));
		info.AddProperty(PropertyName._runtimeGpuClockPoseReuseValidationCountdown, Variant.From(in _runtimeGpuClockPoseReuseValidationCountdown));
		info.AddProperty(PropertyName._runtimeGpuClockInterpolationActive, Variant.From(in _runtimeGpuClockInterpolationActive));
		info.AddProperty(PropertyName._runtimeGpuClockFramesPerSecond, Variant.From(in _runtimeGpuClockFramesPerSecond));
		info.AddProperty(PropertyName._runtimeGpuClockClipStart, Variant.From(in _runtimeGpuClockClipStart));
		info.AddProperty(PropertyName._runtimeGpuClockClipEndExclusive, Variant.From(in _runtimeGpuClockClipEndExclusive));
		info.AddProperty(PropertyName._runtimeGpuClockLoop, Variant.From(in _runtimeGpuClockLoop));
		info.AddProperty(PropertyName._hasCachedRenderSnapshot, Variant.From(in _hasCachedRenderSnapshot));
		info.AddProperty(PropertyName._loopTerminalAliasCacheRange, Variant.From(in _loopTerminalAliasCacheRange));
		info.AddProperty(PropertyName._loopTerminalAliasCacheHasValue, Variant.From(in _loopTerminalAliasCacheHasValue));
		info.AddProperty(PropertyName._loopTerminalAliasCacheValue, Variant.From(in _loopTerminalAliasCacheValue));
		info.AddProperty(PropertyName._playbackClipEndCacheRange, Variant.From(in _playbackClipEndCacheRange));
		info.AddProperty(PropertyName._playbackClipEndCacheLoop, Variant.From(in _playbackClipEndCacheLoop));
		info.AddProperty(PropertyName._playbackClipEndCachePlayBack, Variant.From(in _playbackClipEndCachePlayBack));
		info.AddProperty(PropertyName._playbackClipEndCacheHasValue, Variant.From(in _playbackClipEndCacheHasValue));
		info.AddProperty(PropertyName._playbackClipEndCacheValue, Variant.From(in _playbackClipEndCacheValue));
		info.AddProperty(PropertyName._renderOrderDebugPending, Variant.From(in _renderOrderDebugPending));
		info.AddProperty(PropertyName._renderOrderDebugReason, Variant.From(in _renderOrderDebugReason));
		info.AddProperty(PropertyName._cachedOwnRenderFrameModulateVersion, Variant.From(in _cachedOwnRenderFrameModulateVersion));
		info.AddProperty(PropertyName._cachedOwnRenderFrameModulate, Variant.From(in _cachedOwnRenderFrameModulate));
		info.AddProperty(PropertyName._cachedOwnRenderFrameSelfModulateVersion, Variant.From(in _cachedOwnRenderFrameSelfModulateVersion));
		info.AddProperty(PropertyName._cachedOwnRenderFrameSelfModulate, Variant.From(in _cachedOwnRenderFrameSelfModulate));
		info.AddProperty(PropertyName._cachedRenderLocalModulateReady, Variant.From(in _cachedRenderLocalModulateReady));
		info.AddProperty(PropertyName._cachedRenderLocalModulate, Variant.From(in _cachedRenderLocalModulate));
		info.AddProperty(PropertyName._cachedRenderLocalSelfModulateReady, Variant.From(in _cachedRenderLocalSelfModulateReady));
		info.AddProperty(PropertyName._cachedRenderLocalSelfModulate, Variant.From(in _cachedRenderLocalSelfModulate));
		info.AddProperty(PropertyName._renderLocalModulateRescanCountdown, Variant.From(in _renderLocalModulateRescanCountdown));
		info.AddProperty(PropertyName._renderLocalModulateAuditTransactionVersion, Variant.From(in _renderLocalModulateAuditTransactionVersion));
		info.AddProperty(PropertyName._runtimeRenderBoundsCached, Variant.From(in _runtimeRenderBoundsCached));
		info.AddProperty(PropertyName._cachedRuntimeRenderBoundsVisible, Variant.From(in _cachedRuntimeRenderBoundsVisible));
		info.AddProperty(PropertyName._cachedRuntimeRenderBoundsStrictVisible, Variant.From(in _cachedRuntimeRenderBoundsStrictVisible));
		info.AddProperty(PropertyName._cachedRuntimeRenderBoundsCullingEnabled, Variant.From(in _cachedRuntimeRenderBoundsCullingEnabled));
		info.AddProperty(PropertyName._cachedRuntimeRenderBoundsTransform, Variant.From(in _cachedRuntimeRenderBoundsTransform));
		info.AddProperty(PropertyName._cachedRuntimeRenderBoundsOffset, Variant.From(in _cachedRuntimeRenderBoundsOffset));
		info.AddProperty(PropertyName._cachedRuntimeRenderBoundsMountParent, Variant.From(in _cachedRuntimeRenderBoundsMountParent));
		info.AddProperty(PropertyName._cachedRuntimeRenderBoundsViewport, Variant.From(in _cachedRuntimeRenderBoundsViewport));
		info.AddProperty(PropertyName._cachedRuntimeRenderBoundsViewportWorldRect, Variant.From(in _cachedRuntimeRenderBoundsViewportWorldRect));
		info.AddProperty(PropertyName._cachedRuntimeRenderBoundsRelativeRect, Variant.From(in _cachedRuntimeRenderBoundsRelativeRect));
		info.AddProperty(PropertyName._cachedRuntimeRenderBoundsHasRelativeRect, Variant.From(in _cachedRuntimeRenderBoundsHasRelativeRect));
		info.AddProperty(PropertyName._runtimeRenderBoundsRescanCountdown, Variant.From(in _runtimeRenderBoundsRescanCountdown));
		info.AddProperty(PropertyName._externalVisualPreparationFrame, Variant.From(in _externalVisualPreparationFrame));
		info.AddProperty(PropertyName._externalVisualGpuStateAtlasRids, Variant.From(in _externalVisualGpuStateAtlasRids));
		info.AddProperty(PropertyName._externalVisualGpuStateFailures, Variant.From(in _externalVisualGpuStateFailures));
		info.AddProperty(PropertyName._staticAtlasPathLayoutRuntimeMutationSeen, Variant.From(in _staticAtlasPathLayoutRuntimeMutationSeen));
		info.AddProperty(PropertyName._bypassStaticAtlasPathLayoutCacheForTests, Variant.From(in _bypassStaticAtlasPathLayoutCacheForTests));
		info.AddProperty(PropertyName._staticAtlasPathLayoutGpuResetPending, Variant.From(in _staticAtlasPathLayoutGpuResetPending));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotData, Variant.From(in _staticAtlasPathSnapshotData));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotDataInstanceId, Variant.From(in _staticAtlasPathSnapshotDataInstanceId));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotAuthoringRevision, Variant.From(in _staticAtlasPathSnapshotAuthoringRevision));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotMediaCount, Variant.From(in _staticAtlasPathSnapshotMediaCount));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotTextureCount, Variant.From(in _staticAtlasPathSnapshotTextureCount));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotUseCount, Variant.From(in _staticAtlasPathSnapshotUseCount));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotPathCount, Variant.From(in _staticAtlasPathSnapshotPathCount));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotEffectiveLimit, Variant.From(in _staticAtlasPathSnapshotEffectiveLimit));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotActiveCount, Variant.From(in _staticAtlasPathSnapshotActiveCount));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotSlotSignature, Variant.From(in _staticAtlasPathSnapshotSlotSignature));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotPaths, Variant.From(in _staticAtlasPathSnapshotPaths));
		info.AddProperty(PropertyName._staticAtlasPathSnapshotValid, Variant.From(in _staticAtlasPathSnapshotValid));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Modulate, out var value))
		{
			Modulate = value.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.SelfModulate, out var value2))
		{
			SelfModulate = value2.As<Color>();
		}
		if (info.TryGetProperty(PropertyName.ZIndex, out var value3))
		{
			ZIndex = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ZAsRelative, out var value4))
		{
			ZAsRelative = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.atlasProfileOverride, out var value5))
		{
			atlasProfileOverride = value5.As<AdobeAnimateAtlasProfile>();
		}
		if (info.TryGetProperty(PropertyName.flashAnimeData, out var value6))
		{
			flashAnimeData = value6.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName.preview, out var value7))
		{
			preview = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.forceLocalRender, out var value8))
		{
			forceLocalRender = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.forceCpuPoseRender, out var value9))
		{
			forceCpuPoseRender = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.suppressEditorRenderSubmission, out var value10))
		{
			suppressEditorRenderSubmission = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.invisible, out var value11))
		{
			invisible = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.offset, out var value12))
		{
			offset = value12.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.timeScale, out var value13))
		{
			timeScale = value13.As<double>();
		}
		if (info.TryGetProperty(PropertyName.deduplicateLoopTerminalFrame, out var value14))
		{
			deduplicateLoopTerminalFrame = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.pause, out var value15))
		{
			pause = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.runtimeViewportCullingEnabled, out var value16))
		{
			runtimeViewportCullingEnabled = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.playBack, out var value17))
		{
			playBack = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.clip, out var value18))
		{
			clip = value18.As<string>();
		}
		if (info.TryGetProperty(PropertyName.layerVisible, out var value19))
		{
			layerVisible = value19.AsGodotArray<bool>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplace, out var value20))
		{
			mediaReplace = value20.AsGodotArray<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplaceUse, out var value21))
		{
			mediaReplaceUse = value21.AsGodotArray<bool>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplaceAtlasPaths, out var value22))
		{
			mediaReplaceAtlasPaths = value22.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.parentSprite, out var value23))
		{
			parentSprite = value23.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName.meshColor, out var value24))
		{
			meshColor = value24.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceBatchVisualRevision, out var value25))
		{
			_effectOnceBatchVisualRevision = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceBatchEligibilityRevision, out var value26))
		{
			_effectOnceBatchEligibilityRevision = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName._flashAnimeData, out var value27))
		{
			_flashAnimeData = value27.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._flashAnimeDataChangePending, out var value28))
		{
			_flashAnimeDataChangePending = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._atlasProfileOverride, out var value29))
		{
			_atlasProfileOverride = value29.As<AdobeAnimateAtlasProfile>();
		}
		if (info.TryGetProperty(PropertyName._preview, out var value30))
		{
			_preview = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._forceLocalRender, out var value31))
		{
			_forceLocalRender = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderClipControl, out var value32))
		{
			_renderClipControl = value32.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._forceCpuPoseRender, out var value33))
		{
			_forceCpuPoseRender = value33.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._invisible, out var value34))
		{
			_invisible = value34.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.normalAlpha, out var value35))
		{
			normalAlpha = value35.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._offset, out var value36))
		{
			_offset = value36.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.offsetRotate, out var value37))
		{
			offsetRotate = value37.As<double>();
		}
		if (info.TryGetProperty(PropertyName._timeScale, out var value38))
		{
			_timeScale = value38.As<double>();
		}
		if (info.TryGetProperty(PropertyName.trueFrameRate, out var value39))
		{
			trueFrameRate = value39.As<double>();
		}
		if (info.TryGetProperty(PropertyName.refreshEveryFlame, out var value40))
		{
			refreshEveryFlame = value40.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.skipLastFrame, out var value41))
		{
			skipLastFrame = value41.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._deduplicateLoopTerminalFrame, out var value42))
		{
			_deduplicateLoopTerminalFrame = value42.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.usePos, out var value43))
		{
			usePos = value43.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useRotate, out var value44))
		{
			useRotate = value44.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.useFollowVisible, out var value45))
		{
			useFollowVisible = value45.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.blendTimeInit, out var value46))
		{
			blendTimeInit = value46.As<double>();
		}
		if (info.TryGetProperty(PropertyName._pause, out var value47))
		{
			_pause = value47.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._playBack, out var value48))
		{
			_playBack = value48.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._playbackBlocked, out var value49))
		{
			_playbackBlocked = value49.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._parentPlaybackStopped, out var value50))
		{
			_parentPlaybackStopped = value50.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._frozenPreview, out var value51))
		{
			_frozenPreview = value51.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceGpuSuppressed, out var value52))
		{
			_effectOnceGpuSuppressed = value52.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceGpuSuppressionToken, out var value53))
		{
			_effectOnceGpuSuppressionToken = value53.As<int>();
		}
		if (info.TryGetProperty(PropertyName._applyingImmediateAnimation, out var value54))
		{
			_applyingImmediateAnimation = value54.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.keepRenderSubmittedWhenPaused, out var value55))
		{
			keepRenderSubmittedWhenPaused = value55.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeViewportCullingEnabled, out var value56))
		{
			_runtimeViewportCullingEnabled = value56.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.onlyDraw, out var value57))
		{
			onlyDraw = value57.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.elapsedTimer, out var value58))
		{
			elapsedTimer = value58.As<double>();
		}
		if (info.TryGetProperty(PropertyName.refreshTimer, out var value59))
		{
			refreshTimer = value59.As<double>();
		}
		if (info.TryGetProperty(PropertyName.blend, out var value60))
		{
			blend = value60.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.blendTime, out var value61))
		{
			blendTime = value61.As<double>();
		}
		if (info.TryGetProperty(PropertyName.blendTimer, out var value62))
		{
			blendTimer = value62.As<double>();
		}
		if (info.TryGetProperty(PropertyName._blendFromFrameFloat, out var value63))
		{
			_blendFromFrameFloat = value63.As<float>();
		}
		if (info.TryGetProperty(PropertyName.frameIndex, out var value64))
		{
			frameIndex = value64.As<int>();
		}
		if (info.TryGetProperty(PropertyName.frameRate, out var value65))
		{
			frameRate = value65.As<double>();
		}
		if (info.TryGetProperty(PropertyName.refreshEveryFrame, out var value66))
		{
			refreshEveryFrame = value66.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.loop, out var value67))
		{
			loop = value67.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._clip, out var value68))
		{
			_clip = value68.As<string>();
		}
		if (info.TryGetProperty(PropertyName._playbackRevision, out var value69))
		{
			_playbackRevision = value69.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._managedPoseRevision, out var value70))
		{
			_managedPoseRevision = value70.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.clipRange, out var value71))
		{
			clipRange = value71.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.clipOver, out var value72))
		{
			clipOver = value72.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._layerVisible, out var value73))
		{
			_layerVisible = value73.AsGodotArray<bool>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceBatchArrayExposureRevision, out var value74))
		{
			_effectOnceBatchArrayExposureRevision = value74.As<int>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceBatchEligibilityTracking, out var value75))
		{
			_effectOnceBatchEligibilityTracking = value75.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplaceAtlas, out var value76))
		{
			mediaReplaceAtlas = value76.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplaceAtlasArray, out var value77))
		{
			mediaReplaceAtlasArray = value77.As<TextureLayered>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplaceAtlasArraySize, out var value78))
		{
			mediaReplaceAtlasArraySize = value78.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplaceAtlasUsesTextureArray, out var value79))
		{
			mediaReplaceAtlasUsesTextureArray = value79.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplaceRect, out var value80))
		{
			mediaReplaceRect = value80.AsGodotArray<Rect2>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplaceAtlasPages, out var value81))
		{
			mediaReplaceAtlasPages = value81.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName._mediaReplace, out var value82))
		{
			_mediaReplace = value82.AsGodotArray<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._mediaReplaceUse, out var value83))
		{
			_mediaReplaceUse = value83.AsGodotArray<bool>();
		}
		if (info.TryGetProperty(PropertyName._mediaReplaceAtlasPaths, out var value84))
		{
			_mediaReplaceAtlasPaths = value84.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName.mediaReplaceAtlasShared, out var value85))
		{
			mediaReplaceAtlasShared = value85.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.canvasItem, out var value86))
		{
			canvasItem = value86.As<Rid>();
		}
		if (info.TryGetProperty(PropertyName.meshTexture, out var value87))
		{
			meshTexture = value87.As<Rid>();
		}
		if (info.TryGetProperty(PropertyName.mesh, out var value88))
		{
			mesh = value88.As<ArrayMesh>();
		}
		if (info.TryGetProperty(PropertyName.needMediaReplaceUpdate, out var value89))
		{
			needMediaReplaceUpdate = value89.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._canRun, out var value90))
		{
			_canRun = value90.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.initClip, out var value91))
		{
			initClip = value91.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._parentSprite, out var value92))
		{
			_parentSprite = value92.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._parentSpriteResolved, out var value93))
		{
			_parentSpriteResolved = value93.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.followParentSpriteLayerId, out var value94))
		{
			followParentSpriteLayerId = value94.As<int>();
		}
		if (info.TryGetProperty(PropertyName.insertLayerId, out var value95))
		{
			insertLayerId = value95.As<int>();
		}
		if (info.TryGetProperty(PropertyName._meshColor, out var value96))
		{
			_meshColor = value96.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._renderColorMultiplier, out var value97))
		{
			_renderColorMultiplier = value97.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._runtimeGpuGraphActive, out var value98))
		{
			_runtimeGpuGraphActive = value98.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasCachedGpuGraphCrowdState, out var value99))
		{
			_hasCachedGpuGraphCrowdState = value99.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedGpuGraphCrowdStatePhysicsFrame, out var value100))
		{
			_cachedGpuGraphCrowdStatePhysicsFrame = value100.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._hasCachedRasterCompositeRenderState, out var value101))
		{
			_hasCachedRasterCompositeRenderState = value101.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphCrowdStaticStateDirty, out var value102))
		{
			_gpuGraphCrowdStaticStateDirty = value102.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphCrowdPresentationStateDirty, out var value103))
		{
			_gpuGraphCrowdPresentationStateDirty = value103.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphCrowdStateNextAuditPhysicsFrame, out var value104))
		{
			_gpuGraphCrowdStateNextAuditPhysicsFrame = value104.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._renderGrayscale, out var value105))
		{
			_renderGrayscale = value105.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._autoInserting, out var value106))
		{
			_autoInserting = value106.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._slotChildren, out var value107))
		{
			_slotChildren = value107.AsGodotObjectArray<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._runtimeSlotChildren, out var value108))
		{
			_runtimeSlotChildren = value108.AsGodotObjectArray<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._managedSlotGpuStateAtlasRids, out var value109))
		{
			_managedSlotGpuStateAtlasRids = value109.As<Rid[]>();
		}
		if (info.TryGetProperty(PropertyName._managedSlotGpuStateRescanCountdowns, out var value110))
		{
			_managedSlotGpuStateRescanCountdowns = value110.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName._managedSlotGpuStateWatchedSprites, out var value111))
		{
			_managedSlotGpuStateWatchedSprites = value111.AsGodotObjectArray<Sprite2D>();
		}
		if (info.TryGetProperty(PropertyName._managedSlotGpuStateWatchedParts, out var value112))
		{
			_managedSlotGpuStateWatchedParts = value112.AsGodotObjectArray<AdobeAnimatePart>();
		}
		if (info.TryGetProperty(PropertyName._runtimeSlotChildrenDirty, out var value113))
		{
			_runtimeSlotChildrenDirty = value113.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeHasChildUpdates, out var value114))
		{
			_runtimeHasChildUpdates = value114.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeChildUpdatesVisualOnlyDirty, out var value115))
		{
			_runtimeChildUpdatesVisualOnlyDirty = value115.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeChildUpdatesVisualOnlyCached, out var value116))
		{
			_runtimeChildUpdatesVisualOnlyCached = value116.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._managedSlotSpritesDirty, out var value117))
		{
			_managedSlotSpritesDirty = value117.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._spriteChildren, out var value118))
		{
			_spriteChildren = value118.AsGodotObjectArray<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._spriteChildOwnerSlots, out var value119))
		{
			_spriteChildOwnerSlots = value119.AsGodotObjectArray<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._hasChildren, out var value120))
		{
			_hasChildren = value120.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeChildViewportFrame, out var value121))
		{
			_runtimeChildViewportFrame = value121.As<long>();
		}
		if (info.TryGetProperty(PropertyName._runtimeChildViewportVisible, out var value122))
		{
			_runtimeChildViewportVisible = value122.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._lastUpdateFrame, out var value123))
		{
			_lastUpdateFrame = value123.As<int>();
		}
		if (info.TryGetProperty(PropertyName._isVisibleInTree, out var value124))
		{
			_isVisibleInTree = value124.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeInsideTree, out var value125))
		{
			_runtimeInsideTree = value125.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._processEnabled, out var value126))
		{
			_processEnabled = value126.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._usingRuntimeManager, out var value127))
		{
			_usingRuntimeManager = value127.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedInstanceIdForRender, out var value128))
		{
			_cachedInstanceIdForRender = value128.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphPreparedGeneration, out var value129))
		{
			_gpuGraphPreparedGeneration = value129.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphPreparedSignature, out var value130))
		{
			_gpuGraphPreparedSignature = value130.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphPreparedQuadCount, out var value131))
		{
			_gpuGraphPreparedQuadCount = value131.As<int>();
		}
		if (info.TryGetProperty(PropertyName._dispatchingBatchedProcess, out var value132))
		{
			_dispatchingBatchedProcess = value132.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._displayFrameVisualDispatchActive, out var value133))
		{
			_displayFrameVisualDispatchActive = value133.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeDisplayTickVersion, out var value134))
		{
			_runtimeDisplayTickVersion = value134.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._runtimeReadyCached, out var value135))
		{
			_runtimeReadyCached = value135.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeNativeCanvasSuppressed, out var value136))
		{
			_runtimeNativeCanvasSuppressed = value136.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeNativeCanvasPublishedFrameVersion, out var value137))
		{
			_runtimeNativeCanvasPublishedFrameVersion = value137.As<long>();
		}
		if (info.TryGetProperty(PropertyName._runtimeManagerFirstFrameHandoffActive, out var value138))
		{
			_runtimeManagerFirstFrameHandoffActive = value138.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeNativeCanvasVisibilityLayer, out var value139))
		{
			_runtimeNativeCanvasVisibilityLayer = value139.As<uint>();
		}
		if (info.TryGetProperty(PropertyName._runtimeNativeCanvasSuppressionEligibilityKnown, out var value140))
		{
			_runtimeNativeCanvasSuppressionEligibilityKnown = value140.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeNativeCanvasSuppressionEligible, out var value141))
		{
			_runtimeNativeCanvasSuppressionEligible = value141.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeParentStateSource, out var value142))
		{
			_runtimeParentStateSource = value142.As<AdobeAnimateSprite>();
		}
		if (info.TryGetProperty(PropertyName._runtimeParentStateValidationCountdown, out var value143))
		{
			_runtimeParentStateValidationCountdown = value143.As<int>();
		}
		if (info.TryGetProperty(PropertyName._runtimeFollowVisibilityKnown, out var value144))
		{
			_runtimeFollowVisibilityKnown = value144.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeFollowVisibilityValue, out var value145))
		{
			_runtimeFollowVisibilityValue = value145.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._needsRenderSubmission, out var value146))
		{
			_needsRenderSubmission = value146.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeRenderSubmissionRetryRequested, out var value147))
		{
			_runtimeRenderSubmissionRetryRequested = value147.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeGpuPrepareRequired, out var value148))
		{
			_runtimeGpuPrepareRequired = value148.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._visibilityParentCanvas, out var value149))
		{
			_visibilityParentCanvas = value149.As<CanvasItem>();
		}
		if (info.TryGetProperty(PropertyName._selfVisibilityChangedConnected, out var value150))
		{
			_selfVisibilityChangedConnected = value150.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._parentVisibilityChangedConnected, out var value151))
		{
			_parentVisibilityChangedConnected = value151.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRenderMountParent, out var value152))
		{
			_cachedRenderMountParent = value152.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._cachedViewport, out var value153))
		{
			_cachedViewport = value153.As<Viewport>();
		}
		if (info.TryGetProperty(PropertyName._renderMountNextAuditPhysicsFrame, out var value154))
		{
			_renderMountNextAuditPhysicsFrame = value154.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._renderModulateMountParent, out var value155))
		{
			_renderModulateMountParent = value155.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._renderModulateAncestorCacheReady, out var value156))
		{
			_renderModulateAncestorCacheReady = value156.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderModulateAncestorRescanCountdown, out var value157))
		{
			_renderModulateAncestorRescanCountdown = value157.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedCanvasLayer, out var value158))
		{
			_cachedCanvasLayer = value158.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedManagedPoseTrackKey, out var value159))
		{
			_cachedManagedPoseTrackKey = value159.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedManagedPoseTrackUseLayerId, out var value160))
		{
			_cachedManagedPoseTrackUseLayerId = value160.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._nextAnimDelayTimer, out var value161))
		{
			_nextAnimDelayTimer = value161.As<double>();
		}
		if (info.TryGetProperty(PropertyName._renderStateCached, out var value162))
		{
			_renderStateCached = value162.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderStaticStateCached, out var value163))
		{
			_renderStaticStateCached = value163.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderStaticLayerArrayEscaped, out var value164))
		{
			_renderStaticLayerArrayEscaped = value164.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderStaticMediaArraysEscaped, out var value165))
		{
			_renderStaticMediaArraysEscaped = value165.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderStaticArrayAuditSubmissionPending, out var value166))
		{
			_renderStaticArrayAuditSubmissionPending = value166.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderStaticStateNextAuditPhysicsFrame, out var value167))
		{
			_renderStaticStateNextAuditPhysicsFrame = value167.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._renderStaticAuditLayerVisibleSnapshot, out var value168))
		{
			_renderStaticAuditLayerVisibleSnapshot = value168.AsGodotArray<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderStaticAuditMediaReplaceUseSnapshot, out var value169))
		{
			_renderStaticAuditMediaReplaceUseSnapshot = value169.AsGodotArray<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderStaticAuditMediaReplaceSnapshot, out var value170))
		{
			_renderStaticAuditMediaReplaceSnapshot = value170.AsGodotArray<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._renderStaticAuditMediaReplaceRectSnapshot, out var value171))
		{
			_renderStaticAuditMediaReplaceRectSnapshot = value171.AsGodotArray<Rect2>();
		}
		if (info.TryGetProperty(PropertyName._renderStaticAuditMediaReplacePageSnapshot, out var value172))
		{
			_renderStaticAuditMediaReplacePageSnapshot = value172.AsGodotArray<int>();
		}
		if (info.TryGetProperty(PropertyName._renderStaticAuditMediaReplaceAtlasPathSnapshot, out var value173))
		{
			_renderStaticAuditMediaReplaceAtlasPathSnapshot = value173.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName._cachedGpuGraphRootOwnerPlaybackRevision, out var value174))
		{
			_cachedGpuGraphRootOwnerPlaybackRevision = value174.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._hasCachedGpuGraphNestedOwnerState, out var value175))
		{
			_hasCachedGpuGraphNestedOwnerState = value175.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedGpuGraphNestedOwnerPlaybackRevision, out var value176))
		{
			_cachedGpuGraphNestedOwnerPlaybackRevision = value176.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphNestedOwnerStateRebaseClockSeconds, out var value177))
		{
			_gpuGraphNestedOwnerStateRebaseClockSeconds = value177.As<float>();
		}
		if (info.TryGetProperty(PropertyName._cachedClip, out var value178))
		{
			_cachedClip = value178.As<string>();
		}
		if (info.TryGetProperty(PropertyName._cachedClipRange, out var value179))
		{
			_cachedClipRange = value179.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._cachedFrameFloat, out var value180))
		{
			_cachedFrameFloat = value180.As<float>();
		}
		if (info.TryGetProperty(PropertyName._cachedGlobalTransform, out var value181))
		{
			_cachedGlobalTransform = value181.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._renderGlobalTransform, out var value182))
		{
			_renderGlobalTransform = value182.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._renderPreviousGlobalTransform, out var value183))
		{
			_renderPreviousGlobalTransform = value183.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._renderGlobalTransformCached, out var value184))
		{
			_renderGlobalTransformCached = value184.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderGlobalTransformDirty, out var value185))
		{
			_renderGlobalTransformDirty = value185.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderTransformChangedInPhysicsFrame, out var value186))
		{
			_renderTransformChangedInPhysicsFrame = value186.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimePhysicsAncestorTransformPending, out var value187))
		{
			_runtimePhysicsAncestorTransformPending = value187.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._knownAncestorTranslationNotificationPending, out var value188))
		{
			_knownAncestorTranslationNotificationPending = value188.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderRootMotionEnabled, out var value189))
		{
			_renderRootMotionEnabled = value189.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderTreePaused, out var value190))
		{
			_renderTreePaused = value190.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderGlobalTransformPhysicsFrame, out var value191))
		{
			_renderGlobalTransformPhysicsFrame = value191.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._cachedModulate, out var value192))
		{
			_cachedModulate = value192.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._cachedOffset, out var value193))
		{
			_cachedOffset = value193.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceAtlas, out var value194))
		{
			_cachedMediaReplaceAtlas = value194.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceAtlasArray, out var value195))
		{
			_cachedMediaReplaceAtlasArray = value195.As<TextureLayered>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceAtlasRid, out var value196))
		{
			_cachedMediaReplaceAtlasRid = value196.As<Rid>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceAtlasArrayRid, out var value197))
		{
			_cachedMediaReplaceAtlasArrayRid = value197.As<Rid>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceAtlasShared, out var value198))
		{
			_cachedMediaReplaceAtlasShared = value198.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceAtlasUsesTextureArray, out var value199))
		{
			_cachedMediaReplaceAtlasUsesTextureArray = value199.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRenderMountParentSubmit, out var value200))
		{
			_cachedRenderMountParentSubmit = value200.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._cachedCanvasLayerSubmit, out var value201))
		{
			_cachedCanvasLayerSubmit = value201.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedZIndex, out var value202))
		{
			_cachedZIndex = value202.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedEffectiveZIndex, out var value203))
		{
			_cachedEffectiveZIndex = value203.As<int>();
		}
		if (info.TryGetProperty(PropertyName._effectiveZIndexCached, out var value204))
		{
			_effectiveZIndexCached = value204.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._effectiveZIndexCacheLocalZ, out var value205))
		{
			_effectiveZIndexCacheLocalZ = value205.As<int>();
		}
		if (info.TryGetProperty(PropertyName._effectiveZIndexCacheZAsRelative, out var value206))
		{
			_effectiveZIndexCacheZAsRelative = value206.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._effectiveZIndexRescanCountdown, out var value207))
		{
			_effectiveZIndexRescanCountdown = value207.As<int>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceBatchRenderTransform, out var value208))
		{
			_effectOnceBatchRenderTransform = value208.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceBatchEffectiveZIndex, out var value209))
		{
			_effectOnceBatchEffectiveZIndex = value209.As<int>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceBatchRenderRevision, out var value210))
		{
			_effectOnceBatchRenderRevision = value210.As<int>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceBatchObservedLocalZIndex, out var value211))
		{
			_effectOnceBatchObservedLocalZIndex = value211.As<int>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceBatchObservedZAsRelative, out var value212))
		{
			_effectOnceBatchObservedZAsRelative = value212.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceBatchRenderNextAuditFrame, out var value213))
		{
			_effectOnceBatchRenderNextAuditFrame = value213.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._effectOnceBatchRenderStateDirty, out var value214))
		{
			_effectOnceBatchRenderStateDirty = value214.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedEffectiveRenderSortBand, out var value215))
		{
			_cachedEffectiveRenderSortBand = value215.As<long>();
		}
		if (info.TryGetProperty(PropertyName._effectiveRenderSortBandCached, out var value216))
		{
			_effectiveRenderSortBandCached = value216.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._effectiveRenderSortBandCacheLocalSortBand, out var value217))
		{
			_effectiveRenderSortBandCacheLocalSortBand = value217.As<long>();
		}
		if (info.TryGetProperty(PropertyName._effectiveRenderSortBandRescanCountdown, out var value218))
		{
			_effectiveRenderSortBandRescanCountdown = value218.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedEffectiveTreeOrderPath, out var value219))
		{
			_cachedEffectiveTreeOrderPath = value219.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName._treeOrderPathRescanCountdown, out var value220))
		{
			_treeOrderPathRescanCountdown = value220.As<int>();
		}
		if (info.TryGetProperty(PropertyName._treeOrderWatchedParent, out var value221))
		{
			_treeOrderWatchedParent = value221.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._renderSortBand, out var value222))
		{
			_renderSortBand = value222.As<long>();
		}
		if (info.TryGetProperty(PropertyName._cachedSortBand, out var value223))
		{
			_cachedSortBand = value223.As<long>();
		}
		if (info.TryGetProperty(PropertyName._cachedUseDrawOrderSortBands, out var value224))
		{
			_cachedUseDrawOrderSortBands = value224.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._drawOrderSortBandCacheDirty, out var value225))
		{
			_drawOrderSortBandCacheDirty = value225.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._needsDrawOrderSortBandsCached, out var value226))
		{
			_needsDrawOrderSortBandsCached = value226.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRefreshEveryFrame, out var value227))
		{
			_cachedRefreshEveryFrame = value227.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedLayerMask, out var value228))
		{
			_cachedLayerMask = value228.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._cachedAllLayersVisible, out var value229))
		{
			_cachedAllLayersVisible = value229.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedConfiguredLayersAllVisible, out var value230))
		{
			_cachedConfiguredLayersAllVisible = value230.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedCanUseLayerMask, out var value231))
		{
			_cachedCanUseLayerMask = value231.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedLayerVisibleCount, out var value232))
		{
			_cachedLayerVisibleCount = value232.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedLayerVisibleValueCount, out var value233))
		{
			_cachedLayerVisibleValueCount = value233.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedLayerVisibleSignature, out var value234))
		{
			_cachedLayerVisibleSignature = value234.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._cachedRasterCompositeSequenceData, out var value235))
		{
			_cachedRasterCompositeSequenceData = value235.As<AdobeAnimateRasterCompositeData>();
		}
		if (info.TryGetProperty(PropertyName._cachedRasterCompositeSequenceClip, out var value236))
		{
			_cachedRasterCompositeSequenceClip = value236.As<string>();
		}
		if (info.TryGetProperty(PropertyName._cachedRasterCompositeSequenceRange, out var value237))
		{
			_cachedRasterCompositeSequenceRange = value237.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._cachedRasterCompositeSequenceLayerSignature, out var value238))
		{
			_cachedRasterCompositeSequenceLayerSignature = value238.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._cachedRasterCompositeSequenceTileBase, out var value239))
		{
			_cachedRasterCompositeSequenceTileBase = value239.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedRasterCompositeSequenceClipStart, out var value240))
		{
			_cachedRasterCompositeSequenceClipStart = value240.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedRasterCompositeSequenceFrameCount, out var value241))
		{
			_cachedRasterCompositeSequenceFrameCount = value241.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedRasterCompositeSequenceValid, out var value242))
		{
			_cachedRasterCompositeSequenceValid = value242.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mediaReplaceStateVersion, out var value243))
		{
			_mediaReplaceStateVersion = value243.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceStateVersion, out var value244))
		{
			_cachedMediaReplaceStateVersion = value244.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceLimit, out var value245))
		{
			_cachedMediaReplaceLimit = value245.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedHasMediaReplace, out var value246))
		{
			_cachedHasMediaReplace = value246.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedAnyRequestedMediaReplace, out var value247))
		{
			_cachedAnyRequestedMediaReplace = value247.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceUseMask, out var value248))
		{
			_cachedMediaReplaceUseMask = value248.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceUseMaskOverflow, out var value249))
		{
			_cachedMediaReplaceUseMaskOverflow = value249.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceAtlasPageCount, out var value250))
		{
			_cachedMediaReplaceAtlasPageCount = value250.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceAtlasPageValues, out var value251))
		{
			_cachedMediaReplaceAtlasPageValues = value251.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceSignature, out var value252))
		{
			_cachedMediaReplaceSignature = value252.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._cachedMediaReplaceBoundsGrow, out var value253))
		{
			_cachedMediaReplaceBoundsGrow = value253.As<float>();
		}
		if (info.TryGetProperty(PropertyName._layerStateVersion, out var value254))
		{
			_layerStateVersion = value254.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cachedLayerStateVersion, out var value255))
		{
			_cachedLayerStateVersion = value255.As<int>();
		}
		if (info.TryGetProperty(PropertyName._runtimeManagerDispatchActive, out var value256))
		{
			_runtimeManagerDispatchActive = value256.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._displayFrameGpuClockPoseReusable, out var value257))
		{
			_displayFrameGpuClockPoseReusable = value257.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeDisplayVisualRequirementCached, out var value258))
		{
			_runtimeDisplayVisualRequirementCached = value258.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeDisplayVisualRequirement, out var value259))
		{
			_cachedRuntimeDisplayVisualRequirement = value259.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeDisplayGpuClockPoseReusable, out var value260))
		{
			_cachedRuntimeDisplayGpuClockPoseReusable = value260.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeDisplayVisualRequirementValidationCountdown, out var value261))
		{
			_runtimeDisplayVisualRequirementValidationCountdown = value261.As<int>();
		}
		if (info.TryGetProperty(PropertyName._runtimeGpuClockPoseReuseCached, out var value262))
		{
			_runtimeGpuClockPoseReuseCached = value262.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeGpuClockPoseReusable, out var value263))
		{
			_cachedRuntimeGpuClockPoseReusable = value263.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeGpuClockPoseReuseValidationCountdown, out var value264))
		{
			_runtimeGpuClockPoseReuseValidationCountdown = value264.As<int>();
		}
		if (info.TryGetProperty(PropertyName._runtimeGpuClockInterpolationActive, out var value265))
		{
			_runtimeGpuClockInterpolationActive = value265.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeGpuClockFramesPerSecond, out var value266))
		{
			_runtimeGpuClockFramesPerSecond = value266.As<float>();
		}
		if (info.TryGetProperty(PropertyName._runtimeGpuClockClipStart, out var value267))
		{
			_runtimeGpuClockClipStart = value267.As<float>();
		}
		if (info.TryGetProperty(PropertyName._runtimeGpuClockClipEndExclusive, out var value268))
		{
			_runtimeGpuClockClipEndExclusive = value268.As<float>();
		}
		if (info.TryGetProperty(PropertyName._runtimeGpuClockLoop, out var value269))
		{
			_runtimeGpuClockLoop = value269.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasCachedRenderSnapshot, out var value270))
		{
			_hasCachedRenderSnapshot = value270.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._loopTerminalAliasCacheRange, out var value271))
		{
			_loopTerminalAliasCacheRange = value271.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._loopTerminalAliasCacheHasValue, out var value272))
		{
			_loopTerminalAliasCacheHasValue = value272.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._loopTerminalAliasCacheValue, out var value273))
		{
			_loopTerminalAliasCacheValue = value273.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._playbackClipEndCacheRange, out var value274))
		{
			_playbackClipEndCacheRange = value274.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._playbackClipEndCacheLoop, out var value275))
		{
			_playbackClipEndCacheLoop = value275.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._playbackClipEndCachePlayBack, out var value276))
		{
			_playbackClipEndCachePlayBack = value276.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._playbackClipEndCacheHasValue, out var value277))
		{
			_playbackClipEndCacheHasValue = value277.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._playbackClipEndCacheValue, out var value278))
		{
			_playbackClipEndCacheValue = value278.As<int>();
		}
		if (info.TryGetProperty(PropertyName._renderOrderDebugPending, out var value279))
		{
			_renderOrderDebugPending = value279.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderOrderDebugReason, out var value280))
		{
			_renderOrderDebugReason = value280.As<string>();
		}
		if (info.TryGetProperty(PropertyName._cachedOwnRenderFrameModulateVersion, out var value281))
		{
			_cachedOwnRenderFrameModulateVersion = value281.As<long>();
		}
		if (info.TryGetProperty(PropertyName._cachedOwnRenderFrameModulate, out var value282))
		{
			_cachedOwnRenderFrameModulate = value282.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._cachedOwnRenderFrameSelfModulateVersion, out var value283))
		{
			_cachedOwnRenderFrameSelfModulateVersion = value283.As<long>();
		}
		if (info.TryGetProperty(PropertyName._cachedOwnRenderFrameSelfModulate, out var value284))
		{
			_cachedOwnRenderFrameSelfModulate = value284.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._cachedRenderLocalModulateReady, out var value285))
		{
			_cachedRenderLocalModulateReady = value285.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRenderLocalModulate, out var value286))
		{
			_cachedRenderLocalModulate = value286.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._cachedRenderLocalSelfModulateReady, out var value287))
		{
			_cachedRenderLocalSelfModulateReady = value287.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRenderLocalSelfModulate, out var value288))
		{
			_cachedRenderLocalSelfModulate = value288.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._renderLocalModulateRescanCountdown, out var value289))
		{
			_renderLocalModulateRescanCountdown = value289.As<int>();
		}
		if (info.TryGetProperty(PropertyName._renderLocalModulateAuditTransactionVersion, out var value290))
		{
			_renderLocalModulateAuditTransactionVersion = value290.As<long>();
		}
		if (info.TryGetProperty(PropertyName._runtimeRenderBoundsCached, out var value291))
		{
			_runtimeRenderBoundsCached = value291.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeRenderBoundsVisible, out var value292))
		{
			_cachedRuntimeRenderBoundsVisible = value292.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeRenderBoundsStrictVisible, out var value293))
		{
			_cachedRuntimeRenderBoundsStrictVisible = value293.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeRenderBoundsCullingEnabled, out var value294))
		{
			_cachedRuntimeRenderBoundsCullingEnabled = value294.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeRenderBoundsTransform, out var value295))
		{
			_cachedRuntimeRenderBoundsTransform = value295.As<Transform2D>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeRenderBoundsOffset, out var value296))
		{
			_cachedRuntimeRenderBoundsOffset = value296.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeRenderBoundsMountParent, out var value297))
		{
			_cachedRuntimeRenderBoundsMountParent = value297.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeRenderBoundsViewport, out var value298))
		{
			_cachedRuntimeRenderBoundsViewport = value298.As<Viewport>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeRenderBoundsViewportWorldRect, out var value299))
		{
			_cachedRuntimeRenderBoundsViewportWorldRect = value299.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeRenderBoundsRelativeRect, out var value300))
		{
			_cachedRuntimeRenderBoundsRelativeRect = value300.As<Rect2>();
		}
		if (info.TryGetProperty(PropertyName._cachedRuntimeRenderBoundsHasRelativeRect, out var value301))
		{
			_cachedRuntimeRenderBoundsHasRelativeRect = value301.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeRenderBoundsRescanCountdown, out var value302))
		{
			_runtimeRenderBoundsRescanCountdown = value302.As<int>();
		}
		if (info.TryGetProperty(PropertyName._externalVisualPreparationFrame, out var value303))
		{
			_externalVisualPreparationFrame = value303.As<long>();
		}
		if (info.TryGetProperty(PropertyName._externalVisualGpuStateAtlasRids, out var value304))
		{
			_externalVisualGpuStateAtlasRids = value304.As<Rid[]>();
		}
		if (info.TryGetProperty(PropertyName._externalVisualGpuStateFailures, out var value305))
		{
			_externalVisualGpuStateFailures = value305.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathLayoutRuntimeMutationSeen, out var value306))
		{
			_staticAtlasPathLayoutRuntimeMutationSeen = value306.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._bypassStaticAtlasPathLayoutCacheForTests, out var value307))
		{
			_bypassStaticAtlasPathLayoutCacheForTests = value307.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathLayoutGpuResetPending, out var value308))
		{
			_staticAtlasPathLayoutGpuResetPending = value308.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotData, out var value309))
		{
			_staticAtlasPathSnapshotData = value309.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotDataInstanceId, out var value310))
		{
			_staticAtlasPathSnapshotDataInstanceId = value310.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotAuthoringRevision, out var value311))
		{
			_staticAtlasPathSnapshotAuthoringRevision = value311.As<long>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotMediaCount, out var value312))
		{
			_staticAtlasPathSnapshotMediaCount = value312.As<int>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotTextureCount, out var value313))
		{
			_staticAtlasPathSnapshotTextureCount = value313.As<int>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotUseCount, out var value314))
		{
			_staticAtlasPathSnapshotUseCount = value314.As<int>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotPathCount, out var value315))
		{
			_staticAtlasPathSnapshotPathCount = value315.As<int>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotEffectiveLimit, out var value316))
		{
			_staticAtlasPathSnapshotEffectiveLimit = value316.As<int>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotActiveCount, out var value317))
		{
			_staticAtlasPathSnapshotActiveCount = value317.As<int>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotSlotSignature, out var value318))
		{
			_staticAtlasPathSnapshotSlotSignature = value318.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotPaths, out var value319))
		{
			_staticAtlasPathSnapshotPaths = value319.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName._staticAtlasPathSnapshotValid, out var value320))
		{
			_staticAtlasPathSnapshotValid = value320.As<bool>();
		}
	}
}
