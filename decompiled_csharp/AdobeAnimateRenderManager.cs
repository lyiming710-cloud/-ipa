using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://addons/AdobeAnimateEditor/Rendering/AdobeAnimateRenderManager.cs")]
public sealed class AdobeAnimateRenderManager : Node2D
{
	private readonly struct CrowdSubmission(AdobeAnimateSprite sprite, AdobeAnimateCrowdRenderStateResult result, int stateIndex, int rasterStateIndex, int fallbackSnapshotIndex, int[] treeOrderPath, ulong stableOrder, bool refreshGpuDynamicState, bool forceGpuPrepare)
	{
		public readonly AdobeAnimateSprite Sprite = sprite;

		public readonly AdobeAnimateCrowdRenderStateResult Result = result;

		public readonly int StateIndex = stateIndex;

		public readonly int RasterStateIndex = rasterStateIndex;

		public readonly int FallbackSnapshotIndex = fallbackSnapshotIndex;

		public readonly int[] TreeOrderPath = treeOrderPath ?? Array.Empty<int>();

		public readonly ulong StableOrder = stableOrder;

		public readonly bool RefreshGpuDynamicState = refreshGpuDynamicState;

		public readonly bool ForceGpuPrepare = forceGpuPrepare;
	}

	private sealed class CrowdSubmissionTreeOrderComparer : IComparer<CrowdSubmission>
	{
		public static readonly CrowdSubmissionTreeOrderComparer Instance = new CrowdSubmissionTreeOrderComparer();

		public int Compare(CrowdSubmission left, CrowdSubmission right)
		{
			return CompareTreeOrderForRender(left.TreeOrderPath, left.StableOrder, right.TreeOrderPath, right.StableOrder);
		}
	}

	private sealed class CrowdFrameGroup
	{
		public long FrameVersion = -9223372036854775808L;

		public int EffectiveZIndex;

		public readonly List<CrowdSubmission> Submissions = new List<CrowdSubmission>();

		public readonly List<AdobeAnimateCrowdRenderState> States = new List<AdobeAnimateCrowdRenderState>();

		public readonly List<AdobeAnimateRasterCompositeRenderState> RasterStates = new List<AdobeAnimateRasterCompositeRenderState>();

		public readonly List<AdobeAnimateRenderSnapshot> FallbackSnapshots = new List<AdobeAnimateRenderSnapshot>();

		public readonly List<PreparedCrowdRoot> PreparedRoots = new List<PreparedCrowdRoot>();

		public readonly List<PreparedCrowdRootDetails> PreparedRootDetails = new List<PreparedCrowdRootDetails>();

		public readonly List<AdobeAnimateCrowdResourceRequirements> ResourceRequirements = new List<AdobeAnimateCrowdResourceRequirements>();

		public readonly List<int> RenderedFallbackSubmissionIndices = new List<int>();

		public bool UseSnapshotFallback;

		public bool ForceCpuPose;

		public bool SubmissionsTreeOrdered;

		public bool RequiresDynamicOverride;

		public bool SignatureConflict;

		public bool CrowdEncoded;

		public bool CrowdPublished;

		public bool FallbackPrepared;

		public bool FallbackPublished;

		public bool FallbackFailed;

		public bool HideCrowdBucketAfterFallback;

		public int TotalStateTexels;

		public int MaxQuadCount;

		public int CrowdBucketMark;

		public AdobeAnimateZIndexCrowdBucket CrowdBucket;

		public AdobeAnimateOrderedSnapshotFallbackBucket FallbackBucket;

		public void ResetForFrame()
		{
			Submissions.Clear();
			States.Clear();
			RasterStates.Clear();
			FallbackSnapshots.Clear();
			PreparedRoots.Clear();
			PreparedRootDetails.Clear();
			ResourceRequirements.Clear();
			RenderedFallbackSubmissionIndices.Clear();
			UseSnapshotFallback = false;
			ForceCpuPose = false;
			SubmissionsTreeOrdered = true;
			RequiresDynamicOverride = false;
			SignatureConflict = false;
			CrowdEncoded = false;
			CrowdPublished = false;
			FallbackPrepared = false;
			FallbackPublished = false;
			FallbackFailed = false;
			HideCrowdBucketAfterFallback = false;
			TotalStateTexels = 0;
			MaxQuadCount = 0;
			CrowdBucketMark = 0;
			CrowdBucket = null;
			FallbackBucket = null;
		}
	}

	private readonly struct PreparedCrowdRoot(int submissionIndex, in PreparedCrowdRootScratch prepared, int detailIndex)
	{
		public int SubmissionIndex { get; } = submissionIndex;

		public AdobeAnimateCrowdLayout Layout { get; } = prepared.Layout;

		public int GpuOwnerCount { get; } = prepared.GpuOwnerCount;

		public float[] RelocatableGpuState { get; } = prepared.RelocatableGpuState;

		public bool RelocatableGpuUseAbsoluteTransform { get; } = prepared.RelocatableGpuUseAbsoluteTransform;

		public int DetailIndex { get; } = detailIndex;
	}

	private readonly struct PreparedCrowdRootDetails(in PreparedCrowdRootScratch prepared)
	{
		public int DrawItemStart { get; } = prepared.DrawItemStart;

		public int DrawItemCount { get; } = prepared.DrawItemCount;

		public int GpuOwnerStart { get; } = prepared.GpuOwnerStart;

		public int GpuOverrideStart { get; } = prepared.GpuOverrideStart;

		public int GpuManagedVisualStart { get; } = prepared.GpuManagedVisualStart;

		public int GpuManagedVisualCount { get; } = prepared.GpuManagedVisualCount;

		public int CompositeOwnerStart { get; } = prepared.CompositeOwnerStart;

		public int CompositeOwnerCount { get; } = prepared.CompositeOwnerCount;

		public bool HasExternalVisuals { get; } = prepared.HasExternalVisuals;

		public CompositeDrawCacheEntry CachedComposite { get; } = prepared.CachedComposite;
	}

	private struct PreparedCrowdRootScratch
	{
		public AdobeAnimateCrowdLayout Layout;

		public int DrawItemStart;

		public int DrawItemCount;

		public int GpuOwnerStart;

		public int GpuOwnerCount;

		public int GpuOverrideStart;

		public int GpuManagedVisualStart;

		public int GpuManagedVisualCount;

		public float[] RelocatableGpuState;

		public bool RelocatableGpuUseAbsoluteTransform;

		public int CompositeOwnerStart;

		public int CompositeOwnerCount;

		public bool HasExternalVisuals;

		public CompositeDrawCacheEntry CachedComposite;

		public readonly bool RequiresDetailStorage
		{
			get
			{
				if (!HasExternalVisuals)
				{
					if (RelocatableGpuState == null)
					{
						if (DrawItemCount == 0 && GpuOwnerCount == 0 && GpuManagedVisualCount == 0 && CompositeOwnerCount == 0)
						{
							return CachedComposite != null;
						}
						return true;
					}
					return false;
				}
				return true;
			}
		}
	}

	private sealed class PreparedGpuRootCacheEntry
	{
		public AdobeAnimateGpuRenderGraphDefinition Graph;

		public AdobeAnimateSprite[] Owners = Array.Empty<AdobeAnimateSprite>();

		public AdobeAnimateGpuGraphOwnerState[] OwnerStates = Array.Empty<AdobeAnimateGpuGraphOwnerState>();

		public AdobeAnimateGpuManagedVisualState[] ManagedVisualStates = Array.Empty<AdobeAnimateGpuManagedVisualState>();

		public float[] RelocatableState = Array.Empty<float>();

		public bool RelocatableUseAbsoluteTransform;

		public AdobeAnimateCrowdLayout Layout;

		public bool RequiresDynamicOverride;

		public bool HasExternalVisuals;

		public AdobeAnimateSprite Root { get; }

		public PreparedGpuRootCacheEntry(AdobeAnimateSprite root)
		{
			Root = root;
		}
	}

	private readonly struct DeferredImmediateSubmission(AdobeAnimateRenderSnapshot snapshot, bool immediateFlush, bool forceCpuPose)
	{
		public AdobeAnimateRenderSnapshot Snapshot { get; } = snapshot;

		public bool ImmediateFlush { get; } = immediateFlush;

		public bool ForceCpuPose { get; } = forceCpuPose;
	}

	private sealed class GpuGraphRootCacheEntry
	{
		public AdobeAnimateSprite Root { get; }

		public AdobeAnimateGpuRenderGraphDefinition Graph { get; }

		public AdobeAnimateSprite[] Owners { get; }

		public bool OffscreenStatePrepared { get; set; }

		public GpuGraphRootCacheEntry(AdobeAnimateSprite root, AdobeAnimateGpuRenderGraphDefinition graph, AdobeAnimateSprite[] owners)
		{
			Root = root;
			Graph = graph;
			Owners = owners ?? Array.Empty<AdobeAnimateSprite>();
		}
	}

	private sealed class CrowdMeshResource
	{
		public ArrayMesh Mesh { get; }

		public int QuadCapacity { get; }

		public int Version { get; }

		public CrowdMeshResource(ArrayMesh mesh, int quadCapacity, int version)
		{
			Mesh = mesh;
			QuadCapacity = quadCapacity;
			Version = version;
		}
	}

	private sealed class PendingGpuGraphInvalidation
	{
		public AdobeAnimateSprite Root { get; }

		public bool OwnerDefinition { get; private set; }

		public bool ManagedSlotTopology { get; private set; }

		public bool ExternalVisualTopology { get; private set; }

		public PendingGpuGraphInvalidation(AdobeAnimateSprite root)
		{
			Root = root;
		}

		public void Include(AdobeAnimateGpuGraphInvalidationReason reason)
		{
			switch (reason)
			{
			case AdobeAnimateGpuGraphInvalidationReason.OwnerDefinition:
				OwnerDefinition = true;
				break;
			case AdobeAnimateGpuGraphInvalidationReason.ManagedSlotTopology:
				ManagedSlotTopology = true;
				break;
			case AdobeAnimateGpuGraphInvalidationReason.ExternalVisualTopology:
				ExternalVisualTopology = true;
				break;
			}
		}
	}

	private sealed class CompositeDrawCacheEntry
	{
		public AdobeAnimateSprite Root;

		public AdobeAnimateDrawItem[] Items = Array.Empty<AdobeAnimateDrawItem>();

		public AdobeAnimateSprite[] Owners = Array.Empty<AdobeAnimateSprite>();

		public AdobeAnimateRuntimeDefinition[] Definitions = Array.Empty<AdobeAnimateRuntimeDefinition>();

		public ulong[] FrameLayoutSignatures = Array.Empty<ulong>();

		public ulong[] StaticSignatures = Array.Empty<ulong>();

		public int[] OwnerIndices = Array.Empty<int>();

		public int[] PoseOffsets = Array.Empty<int>();

		public int ItemCount;

		public int VisualOverrideCount;

		public int OwnerCount;
	}

	private struct AggregateAccumulator
	{
		public int ActiveMounts;

		public int ActiveZBuckets;

		public int CrowdRoots;

		public int CrowdStateTexels;

		public int CrowdStateCapacityTexels;

		public int ResourceSignatures;

		public int StateTextureUploads;

		public int StateTextureUpdatedLayers;

		public long StateTextureUploadedBytes;

		public int MultiMeshUploads;

		public long RdSubmittedBatches;

		public long RdAppliedBatches;

		public long RdQueuedUploads;

		public long RdAppliedUploads;

		public long RdUploadedBytes;

		public long RdBufferRidRefreshes;

		public long RdFailedBatches;

		public long RdBufferUpdateFailures;

		public long RdInvalidTargets;

		public long RdStaleDrops;

		public long RdQueueDepth;

		public long RdMaximumQueueDepth;

		public long RdLastQueuedFrameVersion;

		public long RdLastAppliedFrameVersion;

		public int SignatureConflictBuckets;

		public int FallbackRuns;

		public int FallbackRoots;

		public int ArenaGrowthCount;

		public int CrowdMeshQuadCapacity;

		public int CrowdMeshVersion;

		public int MeshRebinds;

		public int CpuRoots;

		public int CpuFallbackRoots;

		public AdobeAnimateCrowdAggregateStats ToStats()
		{
			return new AdobeAnimateCrowdAggregateStats
			{
				ActiveMounts = ActiveMounts,
				ActiveZBuckets = ActiveZBuckets,
				CrowdRoots = CrowdRoots,
				CrowdStateTexels = CrowdStateTexels,
				CrowdStateCapacityTexels = CrowdStateCapacityTexels,
				ResourceSignatures = ResourceSignatures,
				StateTextureUploads = StateTextureUploads,
				StateTextureUpdatedLayers = StateTextureUpdatedLayers,
				StateTextureUploadedBytes = StateTextureUploadedBytes,
				MultiMeshUploads = MultiMeshUploads,
				RdSubmittedBatches = RdSubmittedBatches,
				RdAppliedBatches = RdAppliedBatches,
				RdQueuedUploads = RdQueuedUploads,
				RdAppliedUploads = RdAppliedUploads,
				RdUploadedBytes = RdUploadedBytes,
				RdBufferRidRefreshes = RdBufferRidRefreshes,
				RdFailedBatches = RdFailedBatches,
				RdBufferUpdateFailures = RdBufferUpdateFailures,
				RdInvalidTargets = RdInvalidTargets,
				RdStaleDrops = RdStaleDrops,
				RdQueueDepth = RdQueueDepth,
				RdMaximumQueueDepth = RdMaximumQueueDepth,
				RdLastQueuedFrameVersion = RdLastQueuedFrameVersion,
				RdLastAppliedFrameVersion = RdLastAppliedFrameVersion,
				SignatureConflictBuckets = SignatureConflictBuckets,
				FallbackRuns = FallbackRuns,
				FallbackRoots = FallbackRoots,
				ArenaGrowthCount = ArenaGrowthCount,
				CrowdMeshQuadCapacity = CrowdMeshQuadCapacity,
				CrowdMeshVersion = CrowdMeshVersion,
				MeshRebinds = MeshRebinds,
				CpuRoots = CpuRoots,
				CpuFallbackRoots = CpuFallbackRoots
			};
		}
	}

	public new class MethodName : Node2D.MethodName
	{
		public static readonly StringName IsValidTextureForFrame = "IsValidTextureForFrame";

		public static readonly StringName RollbackPreparedScratch = "RollbackPreparedScratch";

		public static readonly StringName RebuildResourceBuilderFromEncodedGroups = "RebuildResourceBuilderFromEncodedGroups";

		public static readonly StringName FinalizeFrameResources = "FinalizeFrameResources";

		public static readonly StringName CountEncodedCrowdGroups = "CountEncodedCrowdGroups";

		public static readonly StringName ConvertAllCrowdGroupsToFallback = "ConvertAllCrowdGroupsToFallback";

		public static readonly StringName PruneCompositeDrawCacheIfNeeded = "PruneCompositeDrawCacheIfNeeded";

		public static readonly StringName IsSameTextureArray = "IsSameTextureArray";

		public static readonly StringName RebuildPublishedCrowdMaterials = "RebuildPublishedCrowdMaterials";

		public static readonly StringName GetPublishedDynamicOverrideBindingCount = "GetPublishedDynamicOverrideBindingCount";

		public static readonly StringName RebuildPublishedCrowdMaterial = "RebuildPublishedCrowdMaterial";

		public static readonly StringName HaveSameGpuGraphOwners = "HaveSameGpuGraphOwners";

		public static readonly StringName PrepareGpuRenderGraphAtlasForFrame = "PrepareGpuRenderGraphAtlasForFrame";

		public static readonly StringName IsGpuGraphPreparedForTransaction = "IsGpuGraphPreparedForTransaction";

		public static readonly StringName IsGpuGraphReady = "IsGpuGraphReady";

		public static readonly StringName IsGpuGraphStateCurrent = "IsGpuGraphStateCurrent";

		public static readonly StringName InvalidateGpuRenderGraph = "InvalidateGpuRenderGraph";

		public static readonly StringName FlushPendingGpuGraphInvalidations = "FlushPendingGpuGraphInvalidations";

		public static readonly StringName PruneInvalidGpuGraphRoots = "PruneInvalidGpuGraphRoots";

		public static readonly StringName MaintainGpuGraphDefinitions = "MaintainGpuGraphDefinitions";

		public static readonly StringName ClearGpuRenderGraphCaches = "ClearGpuRenderGraphCaches";

		public static readonly StringName AbortRuntimeFrame = "AbortRuntimeFrame";

		public static readonly StringName PrepareSharedCrowdResources = "PrepareSharedCrowdResources";

		public static readonly StringName EnsureCrowdMaterialMatchesFinalSignature = "EnsureCrowdMaterialMatchesFinalSignature";

		public static readonly StringName BindCurrentCrowdBuckets = "BindCurrentCrowdBuckets";

		public static readonly StringName GetCrowdMeshCapacityClass = "GetCrowdMeshCapacityClass";

		public static readonly StringName CapacityForCrowdMeshSlots = "CapacityForCrowdMeshSlots";

		public static readonly StringName PublishImmediateSnapshotBuckets = "PublishImmediateSnapshotBuckets";

		public static readonly StringName HideRuntimeOutputForFailureHandoff = "HideRuntimeOutputForFailureHandoff";

		public static readonly StringName HideAllOutput = "HideAllOutput";

		public static readonly StringName HideAndDisposeOwnedResources = "HideAndDisposeOwnedResources";

		public static readonly StringName ResolveCanvasLayer = "ResolveCanvasLayer";

		public static readonly StringName WarmupRenderMount = "WarmupRenderMount";

		public static readonly StringName HasPreparedRenderMount = "HasPreparedRenderMount";

		public static readonly StringName TryBeginDetachedGpuRenderGraphWarmupBatch = "TryBeginDetachedGpuRenderGraphWarmupBatch";

		public static readonly StringName CommitDetachedGpuRenderGraphWarmupBatch = "CommitDetachedGpuRenderGraphWarmupBatch";

		public static readonly StringName CancelDetachedGpuRenderGraphWarmupBatch = "CancelDetachedGpuRenderGraphWarmupBatch";

		public static readonly StringName WarmupDetachedGpuRenderGraphs = "WarmupDetachedGpuRenderGraphs";

		public static readonly StringName PrepareDetachedGpuSprites = "PrepareDetachedGpuSprites";

		public static readonly StringName ResolveDetachedGpuRenderRoots = "ResolveDetachedGpuRenderRoots";

		public static readonly StringName RemoveManagerFromStaticLists = "RemoveManagerFromStaticLists";

		public static readonly StringName RunGlobalBucketMaintenance = "RunGlobalBucketMaintenance";

		public static readonly StringName ReclaimIdleBuckets = "ReclaimIdleBuckets";

		public static readonly StringName ResolveMountParent = "ResolveMountParent";

		public static readonly StringName ToRenderMountLocalTransform = "ToRenderMountLocalTransform";

		public static readonly StringName BeginRenderMountTransformFrame = "BeginRenderMountTransformFrame";

		public static readonly StringName EndRenderMountTransformFrame = "EndRenderMountTransformFrame";

		public static readonly StringName FindCanvasLayerAncestor = "FindCanvasLayerAncestor";

		public static readonly StringName GetRenderFrameVersion = "GetRenderFrameVersion";

		public static readonly StringName GetLifecycleTick = "GetLifecycleTick";

		public static readonly StringName BeginRuntimeFrame = "BeginRuntimeFrame";

		public static readonly StringName EnsureCrowdWriter = "EnsureCrowdWriter";

		public static readonly StringName TryAccumulateCrowdCapacityWarmup = "TryAccumulateCrowdCapacityWarmup";

		public static readonly StringName TryPrepareCachedOffscreenGpuGraphState = "TryPrepareCachedOffscreenGpuGraphState";

		public static readonly StringName HasPreparedOffscreenGpuGraphState = "HasPreparedOffscreenGpuGraphState";

		public static readonly StringName AccumulateCrowdCapacityWarmup = "AccumulateCrowdCapacityWarmup";

		public static readonly StringName ApplyCrowdCapacityWarmup = "ApplyCrowdCapacityWarmup";

		public static readonly StringName WarmupSharedCrowdResources = "WarmupSharedCrowdResources";

		public static readonly StringName EncodeRuntimeFrame = "EncodeRuntimeFrame";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName DisableIdleProcess = "DisableIdleProcess";

		public static readonly StringName ReleaseImmediateSubmission = "ReleaseImmediateSubmission";

		public static readonly StringName InjectRenderTransactionFailureForTests = "InjectRenderTransactionFailureForTests";

		public static readonly StringName ThrowInjectedRenderTransactionFailure = "ThrowInjectedRenderTransactionFailure";

		public static readonly StringName PublishDirtyImmediateSubmissionManagers = "PublishDirtyImmediateSubmissionManagers";

		public static readonly StringName CompleteSuccessfulFrameTransaction = "CompleteSuccessfulFrameTransaction";

		public static readonly StringName RestoreSpriteAfterFailedTransaction = "RestoreSpriteAfterFailedTransaction";

		public static readonly StringName GetPhysicsFrameForRender = "GetPhysicsFrameForRender";

		public static readonly StringName GetPhysicsTicksPerSecondForRender = "GetPhysicsTicksPerSecondForRender";

		public static readonly StringName GetFrameLimitForRender = "GetFrameLimitForRender";

		public static readonly StringName BeginFrameTransaction = "BeginFrameTransaction";

		public static readonly StringName EncodeFrameTransaction = "EncodeFrameTransaction";

		public static readonly StringName FreezeFrameTransactionResources = "FreezeFrameTransactionResources";

		public static readonly StringName PublishFrameTransaction = "PublishFrameTransaction";

		public static readonly StringName PublishGpuTransaction = "PublishGpuTransaction";

		public static readonly StringName SetAnimationClock = "SetAnimationClock";

		public static readonly StringName ApplyCrowdClockIfNeeded = "ApplyCrowdClockIfNeeded";

		public static readonly StringName AddTransactionManager = "AddTransactionManager";

		public static readonly StringName IsLiveRegisteredManager = "IsLiveRegisteredManager";

		public static readonly StringName FlushDeferredImmediateSubmissions = "FlushDeferredImmediateSubmissions";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName CompareTreeOrderForRender = "CompareTreeOrderForRender";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName CrowdMeshVersion = "CrowdMeshVersion";

		public static readonly StringName _crowdWriter = "_crowdWriter";

		public static readonly StringName _crowdMaterial = "_crowdMaterial";

		public static readonly StringName _crowdMesh = "_crowdMesh";

		public static readonly StringName _crowdMeshQuadCapacity = "_crowdMeshQuadCapacity";

		public static readonly StringName _crowdMeshVersion = "_crowdMeshVersion";

		public static readonly StringName _runtimeFrameVersion = "_runtimeFrameVersion";

		public static readonly StringName _lifecycleTick = "_lifecycleTick";

		public static readonly StringName _transactionFrameMarker = "_transactionFrameMarker";

		public static readonly StringName _currentFrameMarker = "_currentFrameMarker";

		public static readonly StringName _appliedAnimationClockVersion = "_appliedAnimationClockVersion";

		public static readonly StringName _mountId = "_mountId";

		public static readonly StringName _mountParent = "_mountParent";

		public static readonly StringName _animationClockMaterial = "_animationClockMaterial";

		public static readonly StringName _crowdMaterialStateTextureRid = "_crowdMaterialStateTextureRid";

		public static readonly StringName _crowdMaterialStateTextureSize = "_crowdMaterialStateTextureSize";

		public static readonly StringName _crowdMaterialStateTextureLayers = "_crowdMaterialStateTextureLayers";

		public static readonly StringName _hasFinalResourceSignature = "_hasFinalResourceSignature";

		public static readonly StringName _hasCrowdMaterialResourceSignature = "_hasCrowdMaterialResourceSignature";

		public static readonly StringName _hasCurrentOutput = "_hasCurrentOutput";

		public static readonly StringName _hadCurrentOutputAtFrameBegin = "_hadCurrentOutputAtFrameBegin";

		public static readonly StringName _compositeCachePruneCountdown = "_compositeCachePruneCountdown";

		public static readonly StringName _frameSignatureConflictBuckets = "_frameSignatureConflictBuckets";

		public static readonly StringName _frameMeshRebinds = "_frameMeshRebinds";

		public static readonly StringName _framePublishedCrowdRoots = "_framePublishedCrowdRoots";

		public static readonly StringName _framePublishedZBuckets = "_framePublishedZBuckets";

		public static readonly StringName _frameStateTexels = "_frameStateTexels";

		public static readonly StringName _frameStateUploads = "_frameStateUploads";

		public static readonly StringName _frameStateUpdatedLayers = "_frameStateUpdatedLayers";

		public static readonly StringName _frameStateUploadedBytes = "_frameStateUploadedBytes";

		public static readonly StringName _frameMultiMeshUploads = "_frameMultiMeshUploads";

		public static readonly StringName _frameFallbackRuns = "_frameFallbackRuns";

		public static readonly StringName _frameFallbackRoots = "_frameFallbackRoots";

		public static readonly StringName _crowdCapacityWarmupFrame = "_crowdCapacityWarmupFrame";

		public static readonly StringName _crowdCapacityWarmupMaxQuadCount = "_crowdCapacityWarmupMaxQuadCount";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	public const int BucketBufferStride = 16;

	private const int BucketReclaimIntervalFrames = 120;

	private const int CrowdBucketPoolLimit = 256;

	private const int CharacterCrowdStableGpuCapacityLimit = 65536;

	private const int GpuGraphRootPruneIntervalFrames = 120;

	private const int GpuGraphDefinitionMaintenanceIntervalFrames = 900;

	private const int GpuGraphDefinitionIdleRetentionFrames = 3600;

	private const int GpuGraphDefinitionMinimumPruneCount = 32;

	private const int GpuGraphDefinitionMinimumPruneDivisor = 8;

	private const long GpuGraphDefinitionCompactionMinimumBytes = 134217728L;

	private const int OffscreenGraphBuildRootBudget = 4;

	private const int OffscreenStateWarmupRootBudget = 8;

	private static readonly Dictionary<ulong, AdobeAnimateRenderManager> Instances = new Dictionary<ulong, AdobeAnimateRenderManager>();

	private static readonly List<AdobeAnimateRenderManager> PreviousActiveManagers = new List<AdobeAnimateRenderManager>();

	private static readonly List<AdobeAnimateRenderManager> CurrentActiveManagers = new List<AdobeAnimateRenderManager>();

	private static readonly List<AdobeAnimateRenderManager> TransactionManagers = new List<AdobeAnimateRenderManager>();

	private static readonly List<AdobeAnimateRenderManager> CrowdCapacityWarmupManagers = new List<AdobeAnimateRenderManager>();

	private static readonly List<DeferredImmediateSubmission> DeferredImmediateSubmissions = new List<DeferredImmediateSubmission>();

	private static readonly HashSet<AdobeAnimateRenderManager> DirtyImmediateSubmissionManagers = new HashSet<AdobeAnimateRenderManager>();

	private static readonly Dictionary<ulong, AdobeAnimateGpuRenderGraphDefinition> GpuGraphDefinitions = new Dictionary<ulong, AdobeAnimateGpuRenderGraphDefinition>();

	private static readonly Dictionary<ulong, GpuGraphRootCacheEntry> GpuGraphRoots = new Dictionary<ulong, GpuGraphRootCacheEntry>();

	private static readonly Dictionary<ulong, long> GpuGraphDefinitionLastUsedFrames = new Dictionary<ulong, long>();

	private static readonly Dictionary<ulong, PendingGpuGraphInvalidation> PendingGpuGraphInvalidations = new Dictionary<ulong, PendingGpuGraphInvalidation>();

	private static readonly HashSet<ulong> GpuGraphEverBuiltRootIds = new HashSet<ulong>();

	private static readonly HashSet<ulong> ActiveGpuGraphDefinitionSignatures = new HashSet<ulong>();

	private static readonly List<ulong> GpuGraphRootRemoveScratch = new List<ulong>();

	private static readonly List<ulong> GpuGraphDefinitionRemoveScratch = new List<ulong>();

	private static readonly List<ulong> PendingGpuGraphInvalidationScratch = new List<ulong>();

	private static readonly HashSet<string> GpuManagedVisualFailureReasonsLogged = new HashSet<string>(StringComparer.Ordinal);

	private static readonly AdobeAnimateGpuRenderGraphAtlas GpuGraphAtlas = new AdobeAnimateGpuRenderGraphAtlas();

	private static readonly AdobeAnimateGpuDynamicOverrideAtlas GpuDynamicOverrideAtlas = new AdobeAnimateGpuDynamicOverrideAtlas();

	private static bool RuntimeTransactionInProgress;

	private static AdobeAnimateRenderBackend TransactionBackend;

	private static long GpuGraphAtlasPreparedFrame = -9223372036854775808L;

	private static long LastBucketMaintenanceTick = -9223372036854775808L;

	private static long LastGpuGraphRootPruneTick = -9223372036854775808L;

	private static long LastGpuGraphDefinitionMaintenanceTick = -9223372036854775808L;

	private static long CachedRenderMountTransformFrame = -9223372036854775808L;

	private static CanvasItem CachedRenderMountTransformItem;

	private static Transform2D CachedRenderMountInverseTransform;

	private static ulong GpuGraphPreparationGeneration = 1uL;

	private static int GpuGraphMaxRenderSlotCount = 1;

	private static AdobeAnimateCrowdAggregateStats AggregateRenderStats;

	private static float CurrentAnimationClockSeconds;

	private static float CurrentPhysicsInterpolationFraction = 1f;

	private static long CurrentAnimationClockVersion;

	private static ulong CurrentRenderPhysicsFrame;

	private static int CurrentRenderPhysicsTicksPerSecond = 60;

	private static int CurrentRenderFrameLimit;

	private static bool RenderTransactionRecoveryInProgress;

	internal const int CrowdMeshCapacityQuantum = 8;

	private readonly AdobeAnimateSharedStateArena _crowdStateArena = new AdobeAnimateSharedStateArena();

	private readonly AdobeAnimateSharedStateArena _fallbackStateArena = new AdobeAnimateSharedStateArena();

	private readonly AdobeAnimateCrowdResourceSignatureBuilder _resourceSignatureBuilder = new AdobeAnimateCrowdResourceSignatureBuilder();

	private readonly Dictionary<int, CrowdFrameGroup> _frameGroupsByZ = new Dictionary<int, CrowdFrameGroup>();

	private readonly List<CrowdFrameGroup> _frameGroupsInSubmissionOrder = new List<CrowdFrameGroup>();

	private readonly Dictionary<int, AdobeAnimateZIndexCrowdBucket> _crowdBuckets = new Dictionary<int, AdobeAnimateZIndexCrowdBucket>();

	private readonly Dictionary<int, CrowdMeshResource> _crowdMeshesByQuadCapacity = new Dictionary<int, CrowdMeshResource>();

	private readonly Dictionary<int, int> _crowdCapacityWarmupCountsByZ = new Dictionary<int, int>();

	private readonly Dictionary<int, AdobeAnimateOrderedSnapshotFallbackBucket> _fallbackBuckets = new Dictionary<int, AdobeAnimateOrderedSnapshotFallbackBucket>();

	private readonly Dictionary<int, AdobeAnimateOrderedSnapshotFallbackBucket> _immediateSnapshotBuckets = new Dictionary<int, AdobeAnimateOrderedSnapshotFallbackBucket>();

	private readonly Stack<AdobeAnimateZIndexCrowdBucket> _crowdBucketPool = new Stack<AdobeAnimateZIndexCrowdBucket>();

	private readonly List<int> _bucketKeysToRemove = new List<int>();

	private List<AdobeAnimateZIndexCrowdBucket> _previousTouchedCrowdBuckets = new List<AdobeAnimateZIndexCrowdBucket>();

	private List<AdobeAnimateZIndexCrowdBucket> _currentTouchedCrowdBuckets = new List<AdobeAnimateZIndexCrowdBucket>();

	private List<AdobeAnimateOrderedSnapshotFallbackBucket> _previousTouchedFallbackBuckets = new List<AdobeAnimateOrderedSnapshotFallbackBucket>();

	private List<AdobeAnimateOrderedSnapshotFallbackBucket> _currentTouchedFallbackBuckets = new List<AdobeAnimateOrderedSnapshotFallbackBucket>();

	private readonly List<AdobeAnimateDrawItem> _preparedDrawItems = new List<AdobeAnimateDrawItem>();

	private readonly List<AdobeAnimateGpuGraphOwnerState> _preparedGpuOwners = new List<AdobeAnimateGpuGraphOwnerState>();

	private readonly List<AdobeAnimateGpuDynamicOverrideAllocation> _preparedGpuOverrides = new List<AdobeAnimateGpuDynamicOverrideAllocation>();

	private readonly List<AdobeAnimateGpuManagedVisualState> _preparedGpuManagedVisuals = new List<AdobeAnimateGpuManagedVisualState>();

	private readonly List<AdobeAnimateCompositePoseRefreshState> _preparedCompositeOwners = new List<AdobeAnimateCompositePoseRefreshState>();

	private readonly List<AdobeAnimateDrawItem> _unifiedDrawItems = new List<AdobeAnimateDrawItem>();

	private readonly HashSet<AdobeAnimateSprite> _drawItemVisitedSprites = new HashSet<AdobeAnimateSprite>();

	private readonly Dictionary<ulong, CompositeDrawCacheEntry> _compositeDrawCache = new Dictionary<ulong, CompositeDrawCacheEntry>();

	private readonly List<AdobeAnimateSprite> _compositeOwnerScratch = new List<AdobeAnimateSprite>();

	private readonly List<AdobeAnimateCompositePoseRefreshState> _compositeOwnerStateScratch = new List<AdobeAnimateCompositePoseRefreshState>();

	private readonly List<ulong> _compositeCacheRemoveScratch = new List<ulong>();

	private readonly Dictionary<TextureLayered, bool> _frameTextureValidity = new Dictionary<TextureLayered, bool>(ReferenceEqualityComparer.Instance);

	private readonly Dictionary<Texture2D, bool> _frameTexture2DValidity = new Dictionary<Texture2D, bool>(ReferenceEqualityComparer.Instance);

	private AdobeAnimateMultiMeshBatcher _crowdWriter;

	private ShaderMaterial _crowdMaterial;

	private ArrayMesh _crowdMesh;

	private int _crowdMeshQuadCapacity;

	private int _crowdMeshVersion;

	private long _runtimeFrameVersion = -9223372036854775808L;

	private long _lifecycleTick = -9223372036854775808L;

	private long _transactionFrameMarker = -9223372036854775808L;

	private long _currentFrameMarker = -9223372036854775808L;

	private long _appliedAnimationClockVersion = -9223372036854775808L;

	private ulong _mountId;

	private Node _mountParent;

	private ShaderMaterial _animationClockMaterial;

	private AdobeAnimateCrowdResourceSignature _finalResourceSignature;

	private AdobeAnimateCrowdResourceSignature _crowdMaterialResourceSignature;

	private Rid _crowdMaterialStateTextureRid;

	private Vector2I _crowdMaterialStateTextureSize;

	private int _crowdMaterialStateTextureLayers;

	private bool _hasFinalResourceSignature;

	private bool _hasCrowdMaterialResourceSignature;

	private bool _hasCurrentOutput;

	private bool _hadCurrentOutputAtFrameBegin;

	private int _compositeCachePruneCountdown = 240;

	private int _frameSignatureConflictBuckets;

	private int _frameMeshRebinds;

	private int _framePublishedCrowdRoots;

	private int _framePublishedZBuckets;

	private int _frameStateTexels;

	private int _frameStateUploads;

	private int _frameStateUpdatedLayers;

	private int _frameStateUploadedBytes;

	private int _frameMultiMeshUploads;

	private int _frameFallbackRuns;

	private int _frameFallbackRoots;

	private long _crowdCapacityWarmupFrame = -9223372036854775808L;

	private int _crowdCapacityWarmupMaxQuadCount;

	public static bool RasterCompositeEnabled { get; set; } = true;

	internal static long LastCompletedRuntimeTransactionVersion { get; private set; } = -9223372036854775808L;

	public static bool GpuRenderGraphEnabled { get; set; } = true;

	internal static bool CrowdMeshCapacityClassesEnabled { get; set; } = true;

	public static int GpuRenderGraphAtlasPageCount => GpuGraphAtlas.PageCount;

	public static int GpuRenderGraphAtlasLayerCapacity => GpuGraphAtlas.LayerCapacity;

	public static long GpuRenderGraphAtlasEstimatedBytes => GpuGraphAtlas.EstimatedTextureBytes;

	public static int GpuRenderGraphAtlasAllocationCount => GpuGraphAtlas.AllocationCount;

	public static int GpuRenderGraphDefinitionCount => GpuGraphDefinitions.Count;

	public static int GpuRenderGraphAtlasWrittenTexels => GpuGraphAtlas.WrittenTexels;

	public static long GpuRenderGraphBuildCount { get; private set; }

	public static long GpuRenderGraphInitialBuildCount { get; private set; }

	public static long GpuRenderGraphRebuildCount { get; private set; }

	public static long GpuRenderGraphOwnerExitInvalidationCount { get; private set; }

	public static long GpuRenderGraphOwnerDefinitionInvalidationCount { get; private set; }

	public static long GpuRenderGraphManagedSlotInvalidationCount { get; private set; }

	public static long GpuRenderGraphExternalVisualInvalidationCount { get; private set; }

	internal static long RenderTransactionCountForTests { get; private set; }

	internal static long RetainedGpuDynamicStateCountForTests { get; private set; }

	internal static long FailedRenderTransactionCountForTests { get; private set; }

	internal static int FailureHandoffSubmittedRootCountForTests { get; private set; }

	internal static bool FailureHandoffCompleteForTests { get; private set; }

	internal int CrowdMeshVersion => _crowdMeshVersion;

	internal static float AnimationClockSecondsForBareTest => CurrentAnimationClockSeconds;

	private bool TryPrepareCompositeRoot(AdobeAnimateCrowdRenderState state, long frameVersion, out PreparedCrowdRootScratch prepared)
	{
		prepared = default;
		bool flag = state.Snapshot.Sprite.HasExternalVisualsInOwnedGraphForRender();
		if (!state.Snapshot.HasMediaReplace && !flag && TryAppendCachedCompositeDrawItems(state, out var entry))
		{
			int count = _preparedCompositeOwners.Count;
			_preparedCompositeOwners.AddRange(_compositeOwnerStateScratch);
			if (!AdobeAnimateMultiMeshBatcher.TryMeasureCachedCompositeCrowd(entry.ItemCount, entry.VisualOverrideCount, _compositeOwnerStateScratch.Count, out var layout))
			{
				return false;
			}
			prepared.Layout = layout;
			prepared.CachedComposite = entry;
			prepared.CompositeOwnerStart = count;
			prepared.CompositeOwnerCount = _compositeOwnerStateScratch.Count;
			prepared.HasExternalVisuals = false;
			return true;
		}
		_unifiedDrawItems.Clear();
		int num = AdobeAnimateDrawItemBuilder.Build(state.Snapshot, _unifiedDrawItems, state.PoseTextureArray, frameVersion, _drawItemVisitedSprites);
		if (num > 1 && (state.Snapshot.NeedsDrawItemSort | flag) && !AreDrawItemsSorted(_unifiedDrawItems, 0, num))
		{
			_unifiedDrawItems.Sort(0, num, AdobeAnimateDrawItemComparer.Instance);
		}
		if (num <= 0)
		{
			return false;
		}
		CompositeDrawCacheEntry compositeDrawCacheEntry = (state.Snapshot.HasMediaReplace ? null : StoreCompositeDrawItemCache(state, num));
		if (compositeDrawCacheEntry != null)
		{
			int count2 = _preparedCompositeOwners.Count;
			_preparedCompositeOwners.AddRange(_compositeOwnerStateScratch);
			if (!AdobeAnimateMultiMeshBatcher.TryMeasureCachedCompositeCrowd(compositeDrawCacheEntry.ItemCount, compositeDrawCacheEntry.VisualOverrideCount, _compositeOwnerStateScratch.Count, out var layout2))
			{
				return false;
			}
			prepared.Layout = layout2;
			prepared.CachedComposite = compositeDrawCacheEntry;
			prepared.CompositeOwnerStart = count2;
			prepared.CompositeOwnerCount = _compositeOwnerStateScratch.Count;
			prepared.HasExternalVisuals = flag;
			return true;
		}
		int count3 = _preparedDrawItems.Count;
		_preparedDrawItems.AddRange(_unifiedDrawItems);
		if (!AdobeAnimateMultiMeshBatcher.TryMeasureCompositeCrowd(num, out var layout3))
		{
			return false;
		}
		prepared.Layout = layout3;
		prepared.DrawItemStart = count3;
		prepared.DrawItemCount = num;
		prepared.HasExternalVisuals = flag;
		return true;
	}

	private bool ValidateCrowdState(AdobeAnimateCrowdRenderState state)
	{
		if (state == null || state.RenderMountParent != _mountParent || state.Definition == null || state.Definition.Frames == null || state.Definition.Frames.Length == 0 || !IsValidTextureForFrame(state.AtlasArray) || state.AtlasArraySize.X <= 0f || state.AtlasArraySize.Y <= 0f)
		{
			return false;
		}
		if (state.Mode == AdobeAnimateCrowdRenderMode.Compact)
		{
			if (state.FrameCount > 0)
			{
				return IsValidTextureForFrame(state.PoseTextureArray, state.PoseTextureSize);
			}
			return false;
		}
		if (state.Mode == AdobeAnimateCrowdRenderMode.GpuGraph)
		{
			if (state.GpuGraphAllocation.Signature != 0L && state.GpuGraphAllocation.RenderSlotCount > 0 && IsValidTextureForFrame(state.PoseTextureArray, state.PoseTextureSize))
			{
				return IsValidTextureForFrame(state.GpuGraphTextureArray, state.GpuGraphTextureSize);
			}
			return false;
		}
		if (state.Mode == AdobeAnimateCrowdRenderMode.RasterComposite)
		{
			AdobeAnimateRasterCompositeData rasterCompositeData = state.RasterCompositeData;
			if (GodotObject.IsInstanceValid(rasterCompositeData) && IsValidTextureForFrame(rasterCompositeData.atlas) && rasterCompositeData.tileSize.X > 0 && rasterCompositeData.tileSize.Y > 0)
			{
				return rasterCompositeData.columns > 0;
			}
			return false;
		}
		if (state.Mode == AdobeAnimateCrowdRenderMode.Composite)
		{
			return GodotObject.IsInstanceValid(state.Snapshot.Sprite);
		}
		return false;
	}

	private bool ValidateRasterCompositeState(in AdobeAnimateRasterCompositeRenderState state)
	{
		AdobeAnimateRuntimeDefinition definition = state.Definition;
		AdobeAnimateRasterCompositeData rasterCompositeData = state.RasterCompositeData;
		if (state.RenderMountParent == _mountParent && definition != null && definition.Frames != null && definition.Frames.Length != 0 && IsValidTextureForFrame(definition.AtlasTextureArray) && definition.AtlasTextureArraySize.X > 0f && definition.AtlasTextureArraySize.Y > 0f && GodotObject.IsInstanceValid(rasterCompositeData) && IsValidTextureForFrame(rasterCompositeData.atlas) && rasterCompositeData.tileSize.X > 0 && rasterCompositeData.tileSize.Y > 0)
		{
			return rasterCompositeData.columns > 0;
		}
		return false;
	}

	private bool TryBuildResourceRequirements(CrowdFrameGroup group, in CrowdSubmission submission, bool requiresDynamicOverride, out AdobeAnimateCrowdResourceRequirements requirements)
	{
		if ((uint)submission.RasterStateIndex < (uint)group.RasterStates.Count)
		{
			ref AdobeAnimateRasterCompositeRenderState reference = ref CollectionsMarshal.AsSpan(group.RasterStates)[submission.RasterStateIndex];
			AdobeAnimateRuntimeDefinition definition = reference.Definition;
			if (definition == null)
			{
				requirements = default;
				return false;
			}
			requirements = new AdobeAnimateCrowdResourceRequirements(definition.AtlasTextureArray, definition.AtlasTextureArraySize, requiresPoseTexture: false, null, Vector2I.Zero, requiresGpuGraphTexture: false, null, Vector2I.Zero, requiresDynamicOverride: false, null, reference.RasterCompositeData);
			return true;
		}
		if ((uint)submission.StateIndex >= (uint)group.States.Count)
		{
			requirements = default;
			return false;
		}
		AdobeAnimateCrowdRenderState state = CollectionsMarshal.AsSpan(group.States)[submission.StateIndex];
		requirements = BuildResourceRequirements(state, requiresDynamicOverride);
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool CanReusePreviousGpuGraphResourceRequirements(CrowdFrameGroup group, in CrowdSubmission submission, bool requiresDynamicOverride)
	{
		if (group.ResourceRequirements.Count == 0 || (uint)submission.RasterStateIndex < (uint)group.RasterStates.Count || (uint)submission.StateIndex >= (uint)group.States.Count)
		{
			return false;
		}
		AdobeAnimateCrowdRenderState adobeAnimateCrowdRenderState = CollectionsMarshal.AsSpan(group.States)[submission.StateIndex];
		if (adobeAnimateCrowdRenderState == null || adobeAnimateCrowdRenderState.Mode != AdobeAnimateCrowdRenderMode.GpuGraph)
		{
			return false;
		}
		Span<AdobeAnimateCrowdResourceRequirements> span = CollectionsMarshal.AsSpan(group.ResourceRequirements);
		ref AdobeAnimateCrowdResourceRequirements reference = ref span[span.Length - 1];
		if (reference.VisualAtlas == adobeAnimateCrowdRenderState.AtlasArray && reference.VisualAtlasSize == adobeAnimateCrowdRenderState.AtlasArraySize && reference.RequiresPoseTexture && reference.PoseTexture == adobeAnimateCrowdRenderState.PoseTextureArray && reference.PoseTextureSize == adobeAnimateCrowdRenderState.PoseTextureSize && reference.RequiresGpuGraphTexture && reference.GpuGraphTexture == adobeAnimateCrowdRenderState.GpuGraphTextureArray && reference.GpuGraphTextureSize == adobeAnimateCrowdRenderState.GpuGraphTextureSize && reference.RequiresDynamicOverride == requiresDynamicOverride && reference.DynamicOverrideProvider == (requiresDynamicOverride ? GpuDynamicOverrideAtlas : null))
		{
			return reference.RasterCompositeData == null;
		}
		return false;
	}

	private AdobeAnimateCrowdResourceRequirements BuildResourceRequirements(AdobeAnimateCrowdRenderState state, bool requiresDynamicOverride)
	{
		bool requiresPoseTexture = IsValidTextureForFrame(state.PoseTextureArray, state.PoseTextureSize);
		bool requiresGpuGraphTexture = state.Mode == AdobeAnimateCrowdRenderMode.GpuGraph;
		return new AdobeAnimateCrowdResourceRequirements(state.AtlasArray, state.AtlasArraySize, requiresPoseTexture, state.PoseTextureArray, state.PoseTextureSize, requiresGpuGraphTexture, state.GpuGraphTextureArray, state.GpuGraphTextureSize, requiresDynamicOverride, requiresDynamicOverride ? GpuDynamicOverrideAtlas : null, (state.Mode == AdobeAnimateCrowdRenderMode.RasterComposite) ? state.RasterCompositeData : null);
	}

	private bool IsValidTextureForFrame(TextureLayered texture, Vector2I size)
	{
		if (size.X > 0 && size.Y > 0)
		{
			return IsValidTextureForFrame(texture);
		}
		return false;
	}

	private bool IsValidTextureForFrame(TextureLayered texture)
	{
		if (texture == null)
		{
			return false;
		}
		if (_frameTextureValidity.TryGetValue(texture, out var value))
		{
			return value;
		}
		value = GodotObject.IsInstanceValid(texture) && texture.GetRid().IsValid;
		_frameTextureValidity[texture] = value;
		return value;
	}

	private bool IsValidTextureForFrame(Texture2D texture)
	{
		if (texture == null)
		{
			return false;
		}
		if (_frameTexture2DValidity.TryGetValue(texture, out var value))
		{
			return value;
		}
		value = GodotObject.IsInstanceValid(texture) && texture.GetRid().IsValid;
		_frameTexture2DValidity[texture] = value;
		return value;
	}

	private void RollbackPreparedScratch(int drawItemMark, int gpuOwnerMark, int gpuOverrideMark, int gpuManagedVisualMark, int compositeOwnerMark)
	{
		RemoveTail(_preparedDrawItems, drawItemMark);
		RemoveTail(_preparedGpuOwners, gpuOwnerMark);
		RemoveTail(_preparedGpuOverrides, gpuOverrideMark);
		RemoveTail(_preparedGpuManagedVisuals, gpuManagedVisualMark);
		RemoveTail(_preparedCompositeOwners, compositeOwnerMark);
	}

	private static void RemoveTail<T>(List<T> list, int mark)
	{
		if (mark >= 0 && mark < list.Count)
		{
			list.RemoveRange(mark, list.Count - mark);
		}
	}

	private bool TryEncodeCrowdGroup(CrowdFrameGroup group)
	{
		AdobeAnimateZIndexCrowdBucket orCreateCrowdBucket = GetOrCreateCrowdBucket(group.EffectiveZIndex);
		if (orCreateCrowdBucket == null)
		{
			return false;
		}
		orCreateCrowdBucket.BeginFrame(_runtimeFrameVersion, _lifecycleTick);
		if (!_currentTouchedCrowdBuckets.Contains(orCreateCrowdBucket))
		{
			_currentTouchedCrowdBuckets.Add(orCreateCrowdBucket);
		}
		group.CrowdBucket = orCreateCrowdBucket;
		group.CrowdBucketMark = orCreateCrowdBucket.Mark();
		if (!_crowdWriter.BeginCrowdGroup(orCreateCrowdBucket, group.TotalStateTexels, group.PreparedRoots.Count))
		{
			return false;
		}
		Span<PreparedCrowdRoot> span = CollectionsMarshal.AsSpan(group.PreparedRoots);
		for (int i = 0; i < span.Length; i++)
		{
			if (AppendPreparedCrowdRoot(group, in span[i]) != 1)
			{
				_crowdWriter.RollbackCrowdGroup();
				group.HideCrowdBucketAfterFallback = true;
				RebuildResourceBuilderFromEncodedGroups(_runtimeFrameVersion);
				return false;
			}
		}
		if (!_crowdWriter.CommitCrowdGroup())
		{
			group.HideCrowdBucketAfterFallback = true;
			RebuildResourceBuilderFromEncodedGroups(_runtimeFrameVersion);
			return false;
		}
		group.CrowdEncoded = true;
		return true;
	}

	private int AppendPreparedCrowdRoot(CrowdFrameGroup group, in PreparedCrowdRoot root)
	{
		if ((uint)root.SubmissionIndex >= (uint)group.Submissions.Count)
		{
			return 0;
		}
		ref CrowdSubmission reference = ref CollectionsMarshal.AsSpan(group.Submissions)[root.SubmissionIndex];
		if ((uint)reference.RasterStateIndex < (uint)group.RasterStates.Count)
		{
			ref AdobeAnimateRasterCompositeRenderState state = ref CollectionsMarshal.AsSpan(group.RasterStates)[reference.RasterStateIndex];
			return _crowdWriter.AppendPreparedRasterCompositeFrame(in state);
		}
		if ((uint)reference.StateIndex >= (uint)group.States.Count)
		{
			return 0;
		}
		AdobeAnimateCrowdRenderState adobeAnimateCrowdRenderState = CollectionsMarshal.AsSpan(group.States)[reference.StateIndex];
		if (adobeAnimateCrowdRenderState.Mode == AdobeAnimateCrowdRenderMode.Compact)
		{
			return _crowdWriter.AppendPreparedCompactCrowdFrame(adobeAnimateCrowdRenderState);
		}
		if (adobeAnimateCrowdRenderState.Mode == AdobeAnimateCrowdRenderMode.RasterComposite)
		{
			return _crowdWriter.AppendPreparedRasterCompositeFrame(adobeAnimateCrowdRenderState);
		}
		if (adobeAnimateCrowdRenderState.Mode == AdobeAnimateCrowdRenderMode.GpuGraph)
		{
			int num;
			if (root.RelocatableGpuState != null && root.RelocatableGpuUseAbsoluteTransform == adobeAnimateCrowdRenderState.GpuGraphUseAbsoluteTransform)
			{
				num = _crowdWriter.AppendPreparedRelocatableGpuRenderGraphFrame(adobeAnimateCrowdRenderState.GpuGraphAllocation, adobeAnimateCrowdRenderState.GlobalTransform, root.RelocatableGpuState, root.GpuOwnerCount, root.Layout.StateTexelCount, root.RelocatableGpuUseAbsoluteTransform, adobeAnimateCrowdRenderState.GpuGraphRootOwnerState, adobeAnimateCrowdRenderState.RootMotion);
			}
			else
			{
				if ((uint)root.DetailIndex >= (uint)group.PreparedRootDetails.Count)
				{
					return 0;
				}
				ref PreparedCrowdRootDetails reference2 = ref CollectionsMarshal.AsSpan(group.PreparedRootDetails)[root.DetailIndex];
				if (!GpuGraphDefinitions.TryGetValue(adobeAnimateCrowdRenderState.GpuGraphAllocation.Signature, out var value))
				{
					return 0;
				}
				num = _crowdWriter.AppendPreparedGpuRenderGraphFrame(adobeAnimateCrowdRenderState.GpuGraphAllocation, value, adobeAnimateCrowdRenderState.GlobalTransform, _preparedGpuOwners, reference2.GpuOwnerStart, root.GpuOwnerCount, _preparedGpuOverrides, reference2.GpuOverrideStart, _preparedGpuManagedVisuals, reference2.GpuManagedVisualStart, reference2.GpuManagedVisualCount, root.Layout.StateTexelCount, adobeAnimateCrowdRenderState.RootMotion);
			}
			if (TowerDefensePerfProfiler.DetailedHotPathMetrics)
			{
				if (num > 0)
				{
					TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphRoots", num);
					TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphSlots", adobeAnimateCrowdRenderState.GpuGraphAllocation.RenderSlotCount);
				}
				else
				{
					TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphFallback", 1);
				}
			}
			return num;
		}
		if ((uint)root.DetailIndex >= (uint)group.PreparedRootDetails.Count)
		{
			return 0;
		}
		ref PreparedCrowdRootDetails reference3 = ref CollectionsMarshal.AsSpan(group.PreparedRootDetails)[root.DetailIndex];
		if (reference3.CachedComposite != null)
		{
			CompositeDrawCacheEntry cachedComposite = reference3.CachedComposite;
			return _crowdWriter.AppendPreparedCachedCompositeCrowdFrame(adobeAnimateCrowdRenderState.GlobalTransform, cachedComposite.Items, cachedComposite.ItemCount, cachedComposite.VisualOverrideCount, cachedComposite.OwnerIndices, cachedComposite.PoseOffsets, _preparedCompositeOwners, reference3.CompositeOwnerStart, reference3.CompositeOwnerCount, adobeAnimateCrowdRenderState.RootMotion);
		}
		return _crowdWriter.AppendCompositeCrowdFrame(adobeAnimateCrowdRenderState.GlobalTransform, _preparedDrawItems, reference3.DrawItemStart, reference3.DrawItemCount, adobeAnimateCrowdRenderState.RootMotion);
	}

	private AdobeAnimateZIndexCrowdBucket GetOrCreateCrowdBucket(int effectiveZIndex)
	{
		if (_crowdBuckets.TryGetValue(effectiveZIndex, out var value))
		{
			return value;
		}
		if (!GodotObject.IsInstanceValid(_mountParent))
		{
			return null;
		}
		value = ((_crowdBucketPool.Count > 0) ? _crowdBucketPool.Pop() : new AdobeAnimateZIndexCrowdBucket());
		value.Attach(_mountParent, effectiveZIndex);
		_crowdBuckets.Add(effectiveZIndex, value);
		return value;
	}

	private bool TryPrepareOrderedFallback(CrowdFrameGroup group, long frameVersion, bool rebuildResourceSignature)
	{
		if (group.FallbackPrepared)
		{
			return true;
		}
		group.RenderedFallbackSubmissionIndices.Clear();
		if (group.CrowdEncoded && group.CrowdBucket != null)
		{
			group.CrowdBucket.Rollback(group.CrowdBucketMark);
			group.HideCrowdBucketAfterFallback = true;
			group.CrowdEncoded = false;
		}
		group.UseSnapshotFallback = true;
		AdobeAnimateOrderedSnapshotFallbackBucket orCreateFallbackBucket = GetOrCreateFallbackBucket(group.EffectiveZIndex);
		if (orCreateFallbackBucket == null)
		{
			return false;
		}
		orCreateFallbackBucket.BeginFrame(frameVersion, _lifecycleTick, _fallbackStateArena);
		Span<CrowdSubmission> span = CollectionsMarshal.AsSpan(group.Submissions);
		for (int i = 0; i < span.Length; i++)
		{
			ref CrowdSubmission reference = ref span[i];
			if ((uint)reference.StateIndex < (uint)group.States.Count && TryAppendOrderedRetainedGpuGraph(orCreateFallbackBucket, group.States[reference.StateIndex], in reference, frameVersion))
			{
				group.RenderedFallbackSubmissionIndices.Add(i);
				continue;
			}
			if (GodotObject.IsInstanceValid(reference.Sprite))
			{
				reference.Sprite.DisableRuntimeGpuClockInterpolationForOwnedGraph();
			}
			if ((uint)reference.StateIndex < (uint)group.States.Count && orCreateFallbackBucket.TryAppendSimpleCrowdState(group.States[reference.StateIndex], reference.Sprite))
			{
				group.RenderedFallbackSubmissionIndices.Add(i);
				continue;
			}
			AdobeAnimateRenderSnapshot snapshot;
			if ((uint)reference.StateIndex < (uint)group.States.Count && group.States[reference.StateIndex].Mode == AdobeAnimateCrowdRenderMode.Composite && GodotObject.IsInstanceValid(group.States[reference.StateIndex].Snapshot.Sprite))
			{
				snapshot = group.States[reference.StateIndex].Snapshot;
			}
			else if ((uint)reference.FallbackSnapshotIndex < (uint)group.FallbackSnapshots.Count)
			{
				snapshot = group.FallbackSnapshots[reference.FallbackSnapshotIndex];
			}
			else
			{
				if (!GodotObject.IsInstanceValid(reference.Sprite))
				{
					orCreateFallbackBucket.Hide();
					group.FallbackFailed = true;
					if (TowerDefensePerfProfiler.DetailedHotPathMetrics)
					{
						TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.crowd.fallbackSnapshotFailed", 1);
					}
					return false;
				}
				if (!(((uint)reference.StateIndex >= (uint)group.States.Count) ? reference.Sprite.TryBuildRenderSnapshot(out snapshot, out var culled) : reference.Sprite.TryBuildOrderedFallbackSnapshotFromCrowdState(group.States[reference.StateIndex], out snapshot, out culled)))
				{
					if (!culled)
					{
						orCreateFallbackBucket.Hide();
						group.FallbackFailed = true;
						if (TowerDefensePerfProfiler.DetailedHotPathMetrics)
						{
							TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.crowd.fallbackSnapshotFailed", 1);
						}
						return false;
					}
					continue;
				}
			}
			if (!orCreateFallbackBucket.TryAppendSnapshot(in snapshot, group.ForceCpuPose))
			{
				orCreateFallbackBucket.Hide();
				group.FallbackFailed = true;
				return false;
			}
			group.RenderedFallbackSubmissionIndices.Add(i);
		}
		group.FallbackBucket = orCreateFallbackBucket;
		group.FallbackPrepared = true;
		if (!_currentTouchedFallbackBuckets.Contains(orCreateFallbackBucket))
		{
			_currentTouchedFallbackBuckets.Add(orCreateFallbackBucket);
		}
		if (rebuildResourceSignature)
		{
			RebuildResourceBuilderFromEncodedGroups(frameVersion);
		}
		return true;
	}

	private bool TryAppendOrderedRetainedGpuGraph(AdobeAnimateOrderedSnapshotFallbackBucket fallback, AdobeAnimateCrowdRenderState state, in CrowdSubmission submission, long frameVersion)
	{
		if (!submission.RefreshGpuDynamicState && fallback.TryAppendRetainedGpuGraphState(state, submission.Sprite))
		{
			return true;
		}
		if (state == null || state.Mode != AdobeAnimateCrowdRenderMode.GpuGraph || !GodotObject.IsInstanceValid(submission.Sprite) || !TryPrepareGpuGraphRoot(state, submission.Sprite, submission.StableOrder, submission.RefreshGpuDynamicState, submission.ForceGpuPrepare, frameVersion, CurrentAnimationClockSeconds, out var prepared, out var requiresDynamicOverride))
		{
			return false;
		}
		AdobeAnimateGpuRenderGraphDefinition value;
		if (!requiresDynamicOverride && prepared.RelocatableGpuState != null && prepared.Layout.StateTexelCount > 0 && prepared.Layout.StateTexelCount <= 536870911 && prepared.RelocatableGpuState.Length == prepared.Layout.StateTexelCount * 4)
		{
			state.StorePreparedRelocatableGpuState(in prepared.Layout, prepared.RelocatableGpuState, prepared.RelocatableGpuUseAbsoluteTransform, prepared.HasExternalVisuals);
		}
		else if ((state.RootMotion.Enabled | requiresDynamicOverride) && GpuGraphDefinitions.TryGetValue(state.GpuGraphAllocation.Signature, out value) && fallback.TryAppendPreparedGpuGraphState(state, submission.Sprite, value, _preparedGpuOwners, prepared.GpuOwnerStart, prepared.GpuOwnerCount, _preparedGpuOverrides, prepared.GpuOverrideStart, _preparedGpuManagedVisuals, prepared.GpuManagedVisualStart, prepared.GpuManagedVisualCount, prepared.Layout.StateTexelCount, requiresDynamicOverride ? GpuDynamicOverrideAtlas.Texture : null, requiresDynamicOverride ? GpuDynamicOverrideAtlas.TextureSize : Vector2I.Zero, requiresDynamicOverride))
		{
			return true;
		}
		return fallback.TryAppendRetainedGpuGraphState(state, submission.Sprite);
	}

	private AdobeAnimateOrderedSnapshotFallbackBucket GetOrCreateFallbackBucket(int effectiveZIndex)
	{
		if (_fallbackBuckets.TryGetValue(effectiveZIndex, out var value))
		{
			return value;
		}
		if (!GodotObject.IsInstanceValid(_mountParent))
		{
			return null;
		}
		value = new AdobeAnimateOrderedSnapshotFallbackBucket();
		value.Attach(_mountParent, effectiveZIndex);
		_fallbackBuckets.Add(effectiveZIndex, value);
		return value;
	}

	private void RebuildResourceBuilderFromEncodedGroups(long frameVersion)
	{
		_resourceSignatureBuilder.Reset();
		for (int i = 0; i < _frameGroupsInSubmissionOrder.Count; i++)
		{
			CrowdFrameGroup crowdFrameGroup = _frameGroupsInSubmissionOrder[i];
			if (!crowdFrameGroup.CrowdEncoded)
			{
				continue;
			}
			for (int j = 0; j < crowdFrameGroup.ResourceRequirements.Count; j++)
			{
				if (!_resourceSignatureBuilder.TryInclude(crowdFrameGroup.ResourceRequirements[j]))
				{
					MarkSignatureConflict(crowdFrameGroup);
					TryPrepareOrderedFallback(crowdFrameGroup, frameVersion, rebuildResourceSignature: false);
					RebuildResourceBuilderFromEncodedGroups(frameVersion);
					return;
				}
			}
		}
	}

	private void MarkSignatureConflict(CrowdFrameGroup group)
	{
		if (!group.SignatureConflict)
		{
			group.SignatureConflict = true;
			_frameSignatureConflictBuckets++;
		}
	}

	private void FinalizeFrameResources(long frameVersion, bool globalFreezeSucceeded)
	{
		if (!globalFreezeSucceeded)
		{
			for (int i = 0; i < _frameGroupsInSubmissionOrder.Count; i++)
			{
				CrowdFrameGroup crowdFrameGroup = _frameGroupsInSubmissionOrder[i];
				if (crowdFrameGroup.CrowdEncoded && crowdFrameGroup.RequiresDynamicOverride)
				{
					TryPrepareOrderedFallback(crowdFrameGroup, frameVersion, rebuildResourceSignature: false);
				}
			}
		}
		RebuildResourceBuilderFromEncodedGroups(frameVersion);
		_hasFinalResourceSignature = false;
		if (CountEncodedCrowdGroups() == 0)
		{
			return;
		}
		ImageTexture dynamicOverrideTexture = (_resourceSignatureBuilder.RequiresDynamicOverride ? GpuDynamicOverrideAtlas.Texture : null);
		Vector2I dynamicOverrideTextureSize = (_resourceSignatureBuilder.RequiresDynamicOverride ? GpuDynamicOverrideAtlas.TextureSize : Vector2I.Zero);
		if (_resourceSignatureBuilder.TryFreeze(dynamicOverrideTexture, dynamicOverrideTextureSize, out _finalResourceSignature))
		{
			_hasFinalResourceSignature = true;
			return;
		}
		bool flag = false;
		for (int j = 0; j < _frameGroupsInSubmissionOrder.Count; j++)
		{
			CrowdFrameGroup crowdFrameGroup2 = _frameGroupsInSubmissionOrder[j];
			if (crowdFrameGroup2.CrowdEncoded && crowdFrameGroup2.RequiresDynamicOverride)
			{
				flag |= TryPrepareOrderedFallback(crowdFrameGroup2, frameVersion, rebuildResourceSignature: false);
			}
		}
		if (flag)
		{
			RebuildResourceBuilderFromEncodedGroups(frameVersion);
			if (CountEncodedCrowdGroups() == 0)
			{
				return;
			}
			if (_resourceSignatureBuilder.TryFreeze(null, Vector2I.Zero, out _finalResourceSignature))
			{
				_hasFinalResourceSignature = true;
				return;
			}
		}
		ConvertAllCrowdGroupsToFallback(frameVersion);
		RebuildResourceBuilderFromEncodedGroups(frameVersion);
	}

	private int CountEncodedCrowdGroups()
	{
		int num = 0;
		for (int i = 0; i < _frameGroupsInSubmissionOrder.Count; i++)
		{
			if (_frameGroupsInSubmissionOrder[i].CrowdEncoded)
			{
				num++;
			}
		}
		return num;
	}

	private void ConvertAllCrowdGroupsToFallback(long frameVersion)
	{
		for (int i = 0; i < _frameGroupsInSubmissionOrder.Count; i++)
		{
			CrowdFrameGroup crowdFrameGroup = _frameGroupsInSubmissionOrder[i];
			if (crowdFrameGroup.CrowdEncoded)
			{
				TryPrepareOrderedFallback(crowdFrameGroup, frameVersion, rebuildResourceSignature: false);
			}
		}
		_hasFinalResourceSignature = false;
	}

	private bool TryAppendCachedCompositeDrawItems(AdobeAnimateCrowdRenderState crowdState, out CompositeDrawCacheEntry entry)
	{
		entry = null;
		AdobeAnimateSprite sprite = crowdState.Snapshot.Sprite;
		if (!GodotObject.IsInstanceValid(sprite) || !_compositeDrawCache.TryGetValue(sprite.GetCachedInstanceIdForRender(), out entry) || !GodotObject.IsInstanceValid(entry.Root) || entry.Root != sprite || entry.ItemCount <= 0)
		{
			return false;
		}
		_compositeOwnerScratch.Clear();
		if (!sprite.TryCollectCompositeCacheOwnersForRender(_compositeOwnerScratch) || _compositeOwnerScratch.Count != entry.OwnerCount)
		{
			return false;
		}
		_compositeOwnerStateScratch.Clear();
		for (int i = 0; i < entry.OwnerCount; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _compositeOwnerScratch[i];
			if (adobeAnimateSprite != entry.Owners[i] || !adobeAnimateSprite.TryBuildCompositePoseRefreshState(crowdState.RenderMountParent, out var state) || state.Definition != entry.Definitions[i] || state.FrameLayoutSignature != entry.FrameLayoutSignatures[i] || state.StaticSignature != entry.StaticSignatures[i])
			{
				return false;
			}
			_compositeOwnerStateScratch.Add(state);
		}
		if (TowerDefensePerfProfiler.DetailedHotPathMetrics)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.compositeCacheHit", entry.ItemCount);
		}
		return true;
	}

	private CompositeDrawCacheEntry StoreCompositeDrawItemCache(AdobeAnimateCrowdRenderState crowdState, int built)
	{
		AdobeAnimateSprite sprite = crowdState.Snapshot.Sprite;
		if (!GodotObject.IsInstanceValid(sprite) || built <= 0 || built > _unifiedDrawItems.Count)
		{
			return null;
		}
		for (int i = 0; i < built; i++)
		{
			AdobeAnimateDrawItem adobeAnimateDrawItem = _unifiedDrawItems[i];
			if (!adobeAnimateDrawItem.UseShaderPose || !GodotObject.IsInstanceValid(adobeAnimateDrawItem.Owner))
			{
				_compositeDrawCache.Remove(sprite.GetCachedInstanceIdForRender());
				return null;
			}
		}
		int num = 0;
		for (int j = 0; j < built; j++)
		{
			if (_unifiedDrawItems[j].UseVisualOverride)
			{
				num++;
			}
		}
		_compositeOwnerScratch.Clear();
		if (!sprite.TryCollectCompositeCacheOwnersForRender(_compositeOwnerScratch) || _compositeOwnerScratch.Count == 0)
		{
			_compositeDrawCache.Remove(sprite.GetCachedInstanceIdForRender());
			return null;
		}
		_compositeOwnerStateScratch.Clear();
		for (int k = 0; k < _compositeOwnerScratch.Count; k++)
		{
			if (!_compositeOwnerScratch[k].TryBuildCompositePoseRefreshState(crowdState.RenderMountParent, out var state))
			{
				_compositeDrawCache.Remove(sprite.GetCachedInstanceIdForRender());
				return null;
			}
			_compositeOwnerStateScratch.Add(state);
		}
		ulong cachedInstanceIdForRender = sprite.GetCachedInstanceIdForRender();
		if (!_compositeDrawCache.TryGetValue(cachedInstanceIdForRender, out var value))
		{
			value = new CompositeDrawCacheEntry();
			_compositeDrawCache.Add(cachedInstanceIdForRender, value);
		}
		value.Root = sprite;
		EnsureCompositeCacheCapacity(ref value.Items, built);
		EnsureCompositeCacheCapacity(ref value.Owners, _compositeOwnerScratch.Count);
		EnsureCompositeCacheCapacity(ref value.Definitions, _compositeOwnerScratch.Count);
		EnsureCompositeCacheCapacity(ref value.FrameLayoutSignatures, _compositeOwnerScratch.Count);
		EnsureCompositeCacheCapacity(ref value.StaticSignatures, _compositeOwnerScratch.Count);
		EnsureCompositeCacheCapacity(ref value.OwnerIndices, built);
		EnsureCompositeCacheCapacity(ref value.PoseOffsets, built);
		_unifiedDrawItems.CopyTo(0, value.Items, 0, built);
		for (int l = 0; l < _compositeOwnerScratch.Count; l++)
		{
			AdobeAnimateCompositePoseRefreshState adobeAnimateCompositePoseRefreshState = _compositeOwnerStateScratch[l];
			value.Owners[l] = _compositeOwnerScratch[l];
			value.Definitions[l] = adobeAnimateCompositePoseRefreshState.Definition;
			value.FrameLayoutSignatures[l] = adobeAnimateCompositePoseRefreshState.FrameLayoutSignature;
			value.StaticSignatures[l] = adobeAnimateCompositePoseRefreshState.StaticSignature;
		}
		value.OwnerCount = _compositeOwnerScratch.Count;
		for (int m = 0; m < built; m++)
		{
			if (!TryFindCompositeOwnerIndex(value, value.Items[m].Owner, out var ownerIndex))
			{
				_compositeDrawCache.Remove(cachedInstanceIdForRender);
				return null;
			}
			value.OwnerIndices[m] = ownerIndex;
			AdobeAnimateCompositePoseRefreshState adobeAnimateCompositePoseRefreshState2 = _compositeOwnerStateScratch[ownerIndex];
			int num2 = value.Items[m].PoseTexel - adobeAnimateCompositePoseRefreshState2.PoseBaseTexel;
			if (num2 < 0 || num2 % 5 != 0)
			{
				_compositeDrawCache.Remove(cachedInstanceIdForRender);
				return null;
			}
			int num3 = num2 / 5 - adobeAnimateCompositePoseRefreshState2.FrameOffset;
			if (num3 < 0 || num3 >= adobeAnimateCompositePoseRefreshState2.FrameCount)
			{
				_compositeDrawCache.Remove(cachedInstanceIdForRender);
				return null;
			}
			value.PoseOffsets[m] = num3;
		}
		value.ItemCount = built;
		value.VisualOverrideCount = num;
		if (TowerDefensePerfProfiler.DetailedHotPathMetrics)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.compositeCacheMiss", 1);
		}
		return value;
	}

	private static bool TryFindCompositeOwnerIndex(CompositeDrawCacheEntry entry, AdobeAnimateSprite owner, out int ownerIndex)
	{
		for (int i = 0; i < entry.OwnerCount; i++)
		{
			if (entry.Owners[i] == owner)
			{
				ownerIndex = i;
				return true;
			}
		}
		ownerIndex = -1;
		return false;
	}

	private static void EnsureCompositeCacheCapacity<T>(ref T[] array, int required)
	{
		if (array == null || array.Length < required)
		{
			int num;
			for (num = 4; num < required; num <<= 1)
			{
			}
			Array.Resize(ref array, num);
		}
	}

	private void PruneCompositeDrawCacheIfNeeded()
	{
		if (--_compositeCachePruneCountdown > 0 || _compositeDrawCache.Count == 0)
		{
			return;
		}
		_compositeCachePruneCountdown = 240;
		_compositeCacheRemoveScratch.Clear();
		foreach (KeyValuePair<ulong, CompositeDrawCacheEntry> item in _compositeDrawCache)
		{
			if (!GodotObject.IsInstanceValid(item.Value.Root))
			{
				_compositeCacheRemoveScratch.Add(item.Key);
			}
		}
		for (int i = 0; i < _compositeCacheRemoveScratch.Count; i++)
		{
			_compositeDrawCache.Remove(_compositeCacheRemoveScratch[i]);
		}
		_compositeCacheRemoveScratch.Clear();
	}

	private static bool AreDrawItemsSorted(List<AdobeAnimateDrawItem> items, int start, int count)
	{
		if (items == null || count <= 1)
		{
			return true;
		}
		int num = Math.Min(items.Count, start + count);
		if (start < 0 || start >= num)
		{
			return true;
		}
		AdobeAnimateDrawItemComparer instance = AdobeAnimateDrawItemComparer.Instance;
		for (int i = start + 1; i < num; i++)
		{
			if (instance.Compare(items[i - 1], items[i]) > 0)
			{
				return false;
			}
		}
		return true;
	}

	internal static bool TryResolveSnapshotAtlas(AdobeAnimateRenderSnapshot snapshot, out TextureLayered atlasArray, out Vector2 atlasSize)
	{
		atlasArray = null;
		atlasSize = Vector2.One;
		if (snapshot.HasMediaReplace && snapshot.MediaReplaceAtlasUsesTextureArray && GodotObject.IsInstanceValid(snapshot.MediaReplaceAtlasArray))
		{
			atlasArray = snapshot.MediaReplaceAtlasArray;
			atlasSize = ((snapshot.MediaReplaceAtlasArraySize.X > 0f && snapshot.MediaReplaceAtlasArraySize.Y > 0f) ? snapshot.MediaReplaceAtlasArraySize : Vector2.One);
			return true;
		}
		AdobeAnimateRuntimeDefinition definition = snapshot.Definition;
		if (definition != null && GodotObject.IsInstanceValid(definition.AtlasTextureArray) && definition.AtlasTextureArrayRid.IsValid)
		{
			atlasArray = definition.AtlasTextureArray;
			atlasSize = ((definition.AtlasTextureArraySize.X > 0f && definition.AtlasTextureArraySize.Y > 0f) ? definition.AtlasTextureArraySize : Vector2.One);
			return true;
		}
		if (snapshot.MediaReplaceAtlasUsesTextureArray && GodotObject.IsInstanceValid(snapshot.MediaReplaceAtlasArray))
		{
			atlasArray = snapshot.MediaReplaceAtlasArray;
			atlasSize = ((snapshot.MediaReplaceAtlasArraySize.X > 0f && snapshot.MediaReplaceAtlasArraySize.Y > 0f) ? snapshot.MediaReplaceAtlasArraySize : Vector2.One);
			return true;
		}
		return false;
	}

	internal static bool TryResolveSnapshotPoseTexture(AdobeAnimateRenderSnapshot snapshot, out TextureLayered poseTextureArray, out Vector2I poseTextureSize)
	{
		poseTextureArray = null;
		poseTextureSize = Vector2I.Zero;
		AdobeAnimateRuntimeDefinition definition = snapshot.Definition;
		if (definition == null || !definition.UsesGpuPoseTextureArray || !definition.GpuPoseTextureRid.IsValid || definition.GpuPoseTextureSize.X <= 0 || definition.GpuPoseTextureSize.Y <= 0 || !GodotObject.IsInstanceValid(definition.GpuPoseTextureArray))
		{
			return false;
		}
		poseTextureArray = definition.GpuPoseTextureArray;
		poseTextureSize = definition.GpuPoseTextureSize;
		return true;
	}

	private static bool IsSameTextureArray(TextureLayered left, TextureLayered right)
	{
		if (left == right)
		{
			return true;
		}
		if (!GodotObject.IsInstanceValid(left) || !GodotObject.IsInstanceValid(right))
		{
			return false;
		}
		Rid rid = left.GetRid();
		Rid rid2 = right.GetRid();
		if (rid.IsValid && rid2.IsValid)
		{
			return rid == rid2;
		}
		return false;
	}

	internal static void RebuildPublishedCrowdMaterials()
	{
		foreach (AdobeAnimateRenderManager value in Instances.Values)
		{
			if (GodotObject.IsInstanceValid(value))
			{
				value.RebuildPublishedCrowdMaterial();
			}
		}
	}

	internal static int GetPublishedDynamicOverrideBindingCount()
	{
		int num = 0;
		foreach (AdobeAnimateRenderManager value in Instances.Values)
		{
			if (GodotObject.IsInstanceValid(value) && value._hasCurrentOutput && value._hasCrowdMaterialResourceSignature && GodotObject.IsInstanceValid(value._crowdMaterialResourceSignature.DynamicOverrideTexture))
			{
				num++;
			}
		}
		return num;
	}

	private void RebuildPublishedCrowdMaterial()
	{
		if (!_hasCurrentOutput || !_hasFinalResourceSignature)
		{
			return;
		}
		TextureLayered texture = _crowdStateArena.Texture;
		if (!GodotObject.IsInstanceValid(texture) || !texture.GetRid().IsValid)
		{
			return;
		}
		ShaderMaterial shaderMaterial = AdobeAnimateMultiMeshBatcher.CreateSharedCrowdMaterial();
		if (!GodotObject.IsInstanceValid(shaderMaterial))
		{
			return;
		}
		AdobeAnimateMultiMeshBatcher.ApplySharedCrowdBindings(shaderMaterial, in _finalResourceSignature, _crowdStateArena);
		AdobeAnimateMultiMeshBatcher.SetSharedCrowdAnimationClock(shaderMaterial, CurrentAnimationClockSeconds);
		AdobeAnimateMultiMeshBatcher.SetSharedCrowdPhysicsInterpolationFraction(shaderMaterial, CurrentPhysicsInterpolationFraction);
		_crowdMaterial = shaderMaterial;
		_crowdMaterialResourceSignature = _finalResourceSignature;
		_crowdMaterialStateTextureRid = texture.GetRid();
		_crowdMaterialStateTextureSize = _crowdStateArena.TextureSize;
		_crowdMaterialStateTextureLayers = _crowdStateArena.TextureLayerCount;
		_hasCrowdMaterialResourceSignature = true;
		_animationClockMaterial = shaderMaterial;
		_appliedAnimationClockVersion = CurrentAnimationClockVersion;
		foreach (AdobeAnimateZIndexCrowdBucket value in _crowdBuckets.Values)
		{
			value.ReplaceCanvasMaterial(shaderMaterial);
		}
	}

	private bool TryPrepareCrowdGroup(CrowdFrameGroup group, long frameVersion, float animationClockSeconds)
	{
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		if ((!group.SubmissionsTreeOrdered || AdobeAnimateRuntimeManager.HasInvalidatedRenderRootOrder) && group.Submissions.Count > 1 && !AreCrowdSubmissionsSorted(group.Submissions))
		{
			group.Submissions.Sort(CrowdSubmissionTreeOrderComparer.Instance);
		}
		int count = _preparedDrawItems.Count;
		int count2 = _preparedGpuOwners.Count;
		int count3 = _preparedGpuOverrides.Count;
		int count4 = _preparedGpuManagedVisuals.Count;
		int count5 = _preparedCompositeOwners.Count;
		group.PreparedRoots.Clear();
		group.PreparedRootDetails.Clear();
		group.ResourceRequirements.Clear();
		group.TotalStateTexels = 0;
		group.MaxQuadCount = 0;
		group.RequiresDynamicOverride = false;
		Span<CrowdSubmission> span = CollectionsMarshal.AsSpan(group.Submissions);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeGroupSetup, startBytes);
		for (int i = 0; i < span.Length; i++)
		{
			ref CrowdSubmission reference = ref span[i];
			long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
			PreparedCrowdRootScratch prepared = default;
			bool requiresDynamicOverride = false;
			bool num = reference.Result == AdobeAnimateCrowdRenderStateResult.Submitted && TryPrepareCrowdSubmission(group, in reference, frameVersion, animationClockSeconds, out prepared, out requiresDynamicOverride);
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeSubmission, startBytes2);
			if (!num)
			{
				RollbackPreparedScratch(count, count2, count3, count4, count5);
				RebuildResourceBuilderFromEncodedGroups(frameVersion);
				return false;
			}
			long startBytes3 = TowerDefenseAllocationTelemetry.Begin();
			if (!CanReusePreviousGpuGraphResourceRequirements(group, in reference, requiresDynamicOverride))
			{
				if (!TryBuildResourceRequirements(group, in reference, requiresDynamicOverride, out var requirements))
				{
					RollbackPreparedScratch(count, count2, count3, count4, count5);
					RebuildResourceBuilderFromEncodedGroups(frameVersion);
					TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeResource, startBytes3);
					return false;
				}
				bool flag = group.ResourceRequirements.Count == 0;
				if (!flag)
				{
					Span<AdobeAnimateCrowdResourceRequirements> span2 = CollectionsMarshal.AsSpan(group.ResourceRequirements);
					flag = !requirements.IsExactMatch(in span2[span2.Length - 1]);
				}
				if (flag)
				{
					group.ResourceRequirements.Add(requirements);
					if (!_resourceSignatureBuilder.TryInclude(in requirements))
					{
						MarkSignatureConflict(group);
						RollbackPreparedScratch(count, count2, count3, count4, count5);
						RebuildResourceBuilderFromEncodedGroups(frameVersion);
						TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeResource, startBytes3);
						return false;
					}
				}
			}
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeResource, startBytes3);
			long startBytes4 = TowerDefenseAllocationTelemetry.Begin();
			long num2 = (long)group.TotalStateTexels + (long)prepared.Layout.StateTexelCount;
			if (num2 > 2147483647)
			{
				RollbackPreparedScratch(count, count2, count3, count4, count5);
				RebuildResourceBuilderFromEncodedGroups(frameVersion);
				TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeAppend, startBytes4);
				return false;
			}
			group.TotalStateTexels = (int)num2;
			group.MaxQuadCount = Math.Max(group.MaxQuadCount, prepared.Layout.QuadCount);
			group.RequiresDynamicOverride |= requiresDynamicOverride;
			int detailIndex = -1;
			if (prepared.RequiresDetailStorage)
			{
				detailIndex = group.PreparedRootDetails.Count;
				group.PreparedRootDetails.Add(new PreparedCrowdRootDetails(in prepared));
			}
			group.PreparedRoots.Add(new PreparedCrowdRoot(i, in prepared, detailIndex));
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeAppend, startBytes4);
		}
		return group.PreparedRoots.Count > 0;
	}

	private bool TryPrepareCrowdSubmission(CrowdFrameGroup group, in CrowdSubmission submission, long frameVersion, float animationClockSeconds, out PreparedCrowdRootScratch prepared, out bool requiresDynamicOverride)
	{
		if ((uint)submission.RasterStateIndex < (uint)group.RasterStates.Count)
		{
			ref AdobeAnimateRasterCompositeRenderState state = ref CollectionsMarshal.AsSpan(group.RasterStates)[submission.RasterStateIndex];
			prepared = default;
			requiresDynamicOverride = false;
			if (!ValidateRasterCompositeState(in state) || !AdobeAnimateMultiMeshBatcher.TryMeasureRasterCompositeCrowd(out var layout))
			{
				return false;
			}
			prepared.Layout = layout;
			return true;
		}
		if ((uint)submission.StateIndex >= (uint)group.States.Count)
		{
			prepared = default;
			requiresDynamicOverride = false;
			return false;
		}
		AdobeAnimateCrowdRenderState state2 = CollectionsMarshal.AsSpan(group.States)[submission.StateIndex];
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		bool flag = TryPrepareRetainedGpuGraphRoot(state2, submission.RefreshGpuDynamicState, submission.ForceGpuPrepare, out prepared);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeRetainedSubmission, startBytes);
		if (flag)
		{
			requiresDynamicOverride = false;
			return true;
		}
		long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
		bool result = TryPrepareCrowdRoot(state2, submission.Sprite, submission.StableOrder, submission.RefreshGpuDynamicState, submission.ForceGpuPrepare, frameVersion, animationClockSeconds, out prepared, out requiresDynamicOverride);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeDynamicSubmission, startBytes2);
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private bool TryPrepareRetainedGpuGraphRoot(AdobeAnimateCrowdRenderState state, bool refreshGpuDynamicState, bool forceGpuPrepare, out PreparedCrowdRootScratch prepared)
	{
		prepared = default;
		if ((refreshGpuDynamicState | forceGpuPrepare) || state == null || state.Mode != AdobeAnimateCrowdRenderMode.GpuGraph || state.RenderMountParent != _mountParent || state.Definition?.Frames == null || state.Definition.Frames.Length == 0 || state.GpuGraphAllocation.Signature == 0L || state.GpuGraphAllocation.RenderSlotCount <= 0 || state.GpuGraphOwners == null || state.GpuGraphOwners.Length == 0 || state.GpuGraphOwners.Length != state.GpuGraphAllocation.OwnerCount || state.PreparedRelocatableGpuRequiresDynamicRefresh)
		{
			return false;
		}
		AdobeAnimateCrowdLayout preparedGpuLayout = state.PreparedGpuLayout;
		float[] preparedRelocatableGpuState = state.PreparedRelocatableGpuState;
		bool gpuGraphUseAbsoluteTransform = state.GpuGraphUseAbsoluteTransform;
		if (preparedGpuLayout.StateTexelCount <= 0 || preparedGpuLayout.StateTexelCount > 536870911 || preparedRelocatableGpuState == null || preparedRelocatableGpuState.Length != preparedGpuLayout.StateTexelCount * 4 || state.PreparedRelocatableGpuUseAbsoluteTransform != gpuGraphUseAbsoluteTransform)
		{
			return false;
		}
		prepared.Layout = preparedGpuLayout;
		prepared.GpuOwnerCount = state.GpuGraphOwners.Length;
		prepared.RelocatableGpuState = preparedRelocatableGpuState;
		prepared.RelocatableGpuUseAbsoluteTransform = gpuGraphUseAbsoluteTransform;
		return true;
	}

	private bool TryPrepareCrowdRoot(AdobeAnimateCrowdRenderState state, AdobeAnimateSprite sprite, ulong stableOrder, bool refreshGpuDynamicState, bool forceGpuPrepare, long frameVersion, float animationClockSeconds, out PreparedCrowdRootScratch prepared, out bool requiresDynamicOverride)
	{
		prepared = default;
		requiresDynamicOverride = false;
		if (!ValidateCrowdState(state))
		{
			return false;
		}
		if (state.Mode == AdobeAnimateCrowdRenderMode.Compact)
		{
			if (!AdobeAnimateMultiMeshBatcher.TryMeasureCompactCrowd(state, out var layout))
			{
				return false;
			}
			prepared.Layout = layout;
			return true;
		}
		if (state.Mode == AdobeAnimateCrowdRenderMode.GpuGraph)
		{
			return TryPrepareGpuGraphRoot(state, sprite, stableOrder, refreshGpuDynamicState, forceGpuPrepare, frameVersion, animationClockSeconds, out prepared, out requiresDynamicOverride);
		}
		if (state.Mode == AdobeAnimateCrowdRenderMode.Composite)
		{
			return TryPrepareCompositeRoot(state, frameVersion, out prepared);
		}
		if (state.Mode == AdobeAnimateCrowdRenderMode.RasterComposite)
		{
			if (!AdobeAnimateMultiMeshBatcher.TryMeasureRasterCompositeCrowd(out var layout2))
			{
				return false;
			}
			prepared.Layout = layout2;
			return true;
		}
		return false;
	}

	private static bool AreGpuGraphOwnerDefinitionsEquivalent(AdobeAnimateRuntimeDefinition currentDefinition, AdobeAnimateRuntimeDefinition graphDefinition)
	{
		if (currentDefinition == graphDefinition)
		{
			return true;
		}
		if (currentDefinition == null || graphDefinition == null || currentDefinition.GpuPoseSignature == 0L || currentDefinition.GpuPoseSignature != graphDefinition.GpuPoseSignature || currentDefinition.GpuPoseTextureRid != graphDefinition.GpuPoseTextureRid || currentDefinition.GpuPoseTextureBaseTexel != graphDefinition.GpuPoseTextureBaseTexel || currentDefinition.GpuPoseTextureLayer != graphDefinition.GpuPoseTextureLayer || currentDefinition.GpuPoseTextureSize != graphDefinition.GpuPoseTextureSize || currentDefinition.AtlasTextureArrayRid != graphDefinition.AtlasTextureArrayRid || currentDefinition.AtlasTextureArraySize != graphDefinition.AtlasTextureArraySize || currentDefinition.UsesAtlasTextureArrayLayout != graphDefinition.UsesAtlasTextureArrayLayout || currentDefinition.RuntimeLayerCount != graphDefinition.RuntimeLayerCount || (currentDefinition.Frames?.Length ?? 0) != (graphDefinition.Frames?.Length ?? 0))
		{
			return false;
		}
		if (AreGpuLocalSlotsEquivalent(currentDefinition.GpuRenderLocalSlots, graphDefinition.GpuRenderLocalSlots))
		{
			return AreGpuFrameSlotsEquivalent(currentDefinition.GpuFrameSlotLookup, graphDefinition.GpuFrameSlotLookup);
		}
		return false;
	}

	private static bool AreGpuLocalSlotsEquivalent(AdobeAnimateGpuLocalSlot[] currentSlots, AdobeAnimateGpuLocalSlot[] graphSlots)
	{
		if (currentSlots == graphSlots)
		{
			return true;
		}
		if (currentSlots == null || graphSlots == null || currentSlots.Length != graphSlots.Length)
		{
			return false;
		}
		for (int i = 0; i < currentSlots.Length; i++)
		{
			AdobeAnimateGpuLocalSlot adobeAnimateGpuLocalSlot = currentSlots[i];
			AdobeAnimateGpuLocalSlot adobeAnimateGpuLocalSlot2 = graphSlots[i];
			if (adobeAnimateGpuLocalSlot.SliceKey != adobeAnimateGpuLocalSlot2.SliceKey || adobeAnimateGpuLocalSlot.Occurrence != adobeAnimateGpuLocalSlot2.Occurrence || adobeAnimateGpuLocalSlot.LayerId != adobeAnimateGpuLocalSlot2.LayerId || adobeAnimateGpuLocalSlot.DrawOrder != adobeAnimateGpuLocalSlot2.DrawOrder)
			{
				return false;
			}
		}
		return true;
	}

	private static bool AreGpuFrameSlotsEquivalent(AdobeAnimateGpuFrameSlotEntry[] currentSlots, AdobeAnimateGpuFrameSlotEntry[] graphSlots)
	{
		if (currentSlots == graphSlots)
		{
			return true;
		}
		if (currentSlots == null || graphSlots == null || currentSlots.Length != graphSlots.Length)
		{
			return false;
		}
		for (int i = 0; i < currentSlots.Length; i++)
		{
			AdobeAnimateGpuFrameSlotEntry adobeAnimateGpuFrameSlotEntry = currentSlots[i];
			AdobeAnimateGpuFrameSlotEntry adobeAnimateGpuFrameSlotEntry2 = graphSlots[i];
			if (adobeAnimateGpuFrameSlotEntry.PoseTexel != adobeAnimateGpuFrameSlotEntry2.PoseTexel || adobeAnimateGpuFrameSlotEntry.MediaId != adobeAnimateGpuFrameSlotEntry2.MediaId || adobeAnimateGpuFrameSlotEntry.LayerId != adobeAnimateGpuFrameSlotEntry2.LayerId || adobeAnimateGpuFrameSlotEntry.Visible != adobeAnimateGpuFrameSlotEntry2.Visible)
			{
				return false;
			}
		}
		return true;
	}

	private bool TryPrepareGpuGraphRoot(AdobeAnimateCrowdRenderState state, AdobeAnimateSprite sprite, ulong stableOrder, bool refreshGpuDynamicState, bool forceGpuPrepare, long frameVersion, float animationClockSeconds, out PreparedCrowdRootScratch prepared, out bool requiresDynamicOverride)
	{
		prepared = default;
		requiresDynamicOverride = false;
		if (state.GpuGraphAllocation.Signature == 0L || state.GpuGraphOwners == null || state.GpuGraphOwners.Length != state.GpuGraphAllocation.OwnerCount || state.GpuGraphOwners.Length == 0)
		{
			return false;
		}
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		bool num = !forceGpuPrepare && TryAppendCachedPreparedGpuRoot(state, sprite, stableOrder, refreshGpuDynamicState, frameVersion, animationClockSeconds, out prepared, out requiresDynamicOverride);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeGpuCachedSubmission, startBytes);
		if (num)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphPrepare.cached", 1);
			return true;
		}
		if (forceGpuPrepare)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphPrepare.forced", 1);
		}
		if (!GpuGraphDefinitions.TryGetValue(state.GpuGraphAllocation.Signature, out var value) || value == null || value.Owners.Length != state.GpuGraphOwners.Length)
		{
			return false;
		}
		PreparedGpuRootCacheEntry preparedGpuRootCacheEntry = sprite.GetPreparedGpuRootCacheForRender() as PreparedGpuRootCacheEntry;
		if (preparedGpuRootCacheEntry == null || preparedGpuRootCacheEntry.Root != sprite || preparedGpuRootCacheEntry.Graph != value || !HaveSameGpuGraphOwners(preparedGpuRootCacheEntry.Owners, state.GpuGraphOwners) || preparedGpuRootCacheEntry.OwnerStates.Length != state.GpuGraphOwners.Length || preparedGpuRootCacheEntry.ManagedVisualStates.Length != value.ManagedVisualBindings.Length)
		{
			preparedGpuRootCacheEntry = null;
		}
		state.ClearPreparedRelocatableGpuState();
		sprite.ClearPreparedGpuRootCacheForRender();
		TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphPrepare.full", 1);
		int count = _preparedGpuOwners.Count;
		int count2 = _preparedGpuOverrides.Count;
		int count3 = _preparedGpuManagedVisuals.Count;
		for (int i = 0; i < state.GpuGraphOwners.Length; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = state.GpuGraphOwners[i];
			AdobeAnimateGpuGraphOwnerState state2;
			if (i == 0 && state.GpuGraphRootOwnerState?.Definition != null)
			{
				state2 = state.GpuGraphRootOwnerState;
			}
			else
			{
				if (adobeAnimateSprite == null)
				{
					return false;
				}
				if (!adobeAnimateSprite.TryBuildGpuGraphNestedOwnerStateForRender(state.RenderMountParent, enableGpuClock: true, animationClockSeconds, out state2))
				{
					return false;
				}
			}
			if (!AreGpuGraphOwnerDefinitionsEquivalent(state2.Definition, value.Owners[i].Definition))
			{
				return false;
			}
			_preparedGpuOwners.Add(state2);
			if (!state2.HasMediaReplace)
			{
				_preparedGpuOverrides.Add(default);
				continue;
			}
			if (!GpuDynamicOverrideAtlas.TryGetOrAdd(frameVersion, state2, out var allocation))
			{
				return false;
			}
			_preparedGpuOverrides.Add(allocation);
			requiresDynamicOverride = true;
		}
		int num2 = state.GpuGraphOwners.Length;
		bool hasExternalVisuals = false;
		for (int j = 0; j < value.ManagedVisualBindings.Length; j++)
		{
			AdobeAnimateGpuManagedVisualBinding adobeAnimateGpuManagedVisualBinding = value.ManagedVisualBindings[j];
			if ((uint)adobeAnimateGpuManagedVisualBinding.OwnerIndex >= (uint)num2)
			{
				return false;
			}
			AdobeAnimateGpuGraphOwnerState adobeAnimateGpuGraphOwnerState = _preparedGpuOwners[count + adobeAnimateGpuManagedVisualBinding.OwnerIndex];
			if (adobeAnimateGpuGraphOwnerState == null)
			{
				return false;
			}
			AdobeAnimateSprite sourceSprite = adobeAnimateGpuGraphOwnerState.SourceSprite;
			if (!GodotObject.IsInstanceValid(sourceSprite))
			{
				return false;
			}
			bool flag;
			AdobeAnimateGpuManagedVisualState state3;
			string failureReason;
			switch (adobeAnimateGpuManagedVisualBinding.SourceKind)
			{
			case AdobeAnimateGpuManagedVisualSourceKind.SlotSprite2D:
				flag = sourceSprite.TryBuildManagedSlotGpuStateForRender(adobeAnimateGpuManagedVisualBinding.VisualIndex, value.Owners[adobeAnimateGpuManagedVisualBinding.OwnerIndex].Definition.AtlasTextureArrayRid, out state3, out failureReason);
				break;
			case AdobeAnimateGpuManagedVisualSourceKind.ExternalVisual:
			{
				hasExternalVisuals = true;
				sourceSprite.BeginExternalVisualPreparationForRender(frameVersion);
				long startTicks = TowerDefensePerfProfiler.BeginHotPath();
				flag = sourceSprite.TryBuildExternalVisualGpuStateForRender(adobeAnimateGpuManagedVisualBinding.VisualIndex, value.Owners[adobeAnimateGpuManagedVisualBinding.OwnerIndex].Definition.AtlasTextureArrayRid, out state3, out failureReason);
				TowerDefensePerfProfiler.End("adobeAnimate.externalVisual.encode", startTicks, 1);
				break;
			}
			default:
				state3 = default;
				flag = false;
				failureReason = $"unsupported managed visual source {adobeAnimateGpuManagedVisualBinding.SourceKind}";
				break;
			}
			if (!flag)
			{
				string item = $"{adobeAnimateGpuManagedVisualBinding.SourceKind}:{failureReason}";
				if (GpuManagedVisualFailureReasonsLogged.Add(item))
				{
					GD.PushWarning($"Adobe Animate GPU managed visual fell back to native rendering: source={adobeAnimateGpuManagedVisualBinding.SourceKind} owner={sourceSprite.GetPath()} reason={failureReason}");
				}
				return false;
			}
			_preparedGpuManagedVisuals.Add(state3);
		}
		if (!AdobeAnimateGpuGraphStateWriter.TryMeasure(state.GpuGraphAllocation, _preparedGpuOwners, count, num2, value.ManagedVisualBindings.Length, out var layout))
		{
			return false;
		}
		prepared.Layout = new AdobeAnimateCrowdLayout(layout.StateTexelCount, layout.QuadCount);
		prepared.GpuOwnerStart = count;
		prepared.GpuOwnerCount = num2;
		prepared.GpuOverrideStart = count2;
		prepared.GpuManagedVisualStart = count3;
		prepared.GpuManagedVisualCount = value.ManagedVisualBindings.Length;
		prepared.HasExternalVisuals = hasExternalVisuals;
		StorePreparedGpuRootCache(state, sprite, stableOrder, value, in prepared, requiresDynamicOverride, hasExternalVisuals, preparedGpuRootCacheEntry);
		prepared.RelocatableGpuState = state.PreparedRelocatableGpuState;
		prepared.RelocatableGpuUseAbsoluteTransform = state.PreparedRelocatableGpuUseAbsoluteTransform;
		return true;
	}

	private bool TryAppendCachedPreparedGpuRoot(AdobeAnimateCrowdRenderState state, AdobeAnimateSprite sprite, ulong stableOrder, bool refreshGpuDynamicState, long frameVersion, float animationClockSeconds, out PreparedCrowdRootScratch prepared, out bool requiresDynamicOverride)
	{
		prepared = default;
		requiresDynamicOverride = false;
		bool gpuGraphUseAbsoluteTransform = state.GpuGraphUseAbsoluteTransform;
		if (!(sprite.GetPreparedGpuRootCacheForRender() is PreparedGpuRootCacheEntry preparedGpuRootCacheEntry))
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphPrepare.miss.entry", 1);
			return false;
		}
		if (preparedGpuRootCacheEntry.Root != sprite)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphPrepare.miss.root", 1);
			return false;
		}
		if (preparedGpuRootCacheEntry.Graph == null || preparedGpuRootCacheEntry.Graph.Signature != state.GpuGraphAllocation.Signature || preparedGpuRootCacheEntry.Graph.Owners.Length != state.GpuGraphAllocation.OwnerCount)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphPrepare.miss.graph", 1);
			return false;
		}
		if (!HaveSameGpuGraphOwners(preparedGpuRootCacheEntry.Owners, state.GpuGraphOwners))
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphPrepare.miss.owners", 1);
			return false;
		}
		if (preparedGpuRootCacheEntry.OwnerStates.Length != state.GpuGraphOwners.Length)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphPrepare.miss.ownerStates", 1);
			return false;
		}
		if (preparedGpuRootCacheEntry.ManagedVisualStates.Length != preparedGpuRootCacheEntry.Graph.ManagedVisualBindings.Length)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphPrepare.miss.visuals", 1);
			return false;
		}
		if (refreshGpuDynamicState || preparedGpuRootCacheEntry.RequiresDynamicOverride || preparedGpuRootCacheEntry.HasExternalVisuals)
		{
			long startBytes = TowerDefenseAllocationTelemetry.Begin();
			bool flag = TryRefreshPreparedGpuRootDynamicState(state, preparedGpuRootCacheEntry, frameVersion, animationClockSeconds, out requiresDynamicOverride, out var retainedPreviousDynamicState);
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeGpuRefresh, startBytes);
			if (!flag)
			{
				return false;
			}
			if (retainedPreviousDynamicState)
			{
				sprite.RequestRuntimeRenderSubmissionRetry();
			}
		}
		prepared.HasExternalVisuals = preparedGpuRootCacheEntry.HasExternalVisuals;
		if (preparedGpuRootCacheEntry.RelocatableState.Length == preparedGpuRootCacheEntry.Layout.StateTexelCount * 4 && preparedGpuRootCacheEntry.RelocatableUseAbsoluteTransform == gpuGraphUseAbsoluteTransform)
		{
			if (preparedGpuRootCacheEntry.Layout.QuadCount != state.GpuGraphAllocation.RenderSlotCount || !AdobeAnimateGpuGraphStateWriter.TryRefreshGraphAllocation(preparedGpuRootCacheEntry.RelocatableState, 0, state.GpuGraphAllocation, out var changed))
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphPrepare.miss.atlasBinding", 1);
				return false;
			}
			if (changed)
			{
				state.StorePreparedRelocatableGpuState(in preparedGpuRootCacheEntry.Layout, preparedGpuRootCacheEntry.RelocatableState, preparedGpuRootCacheEntry.RelocatableUseAbsoluteTransform, preparedGpuRootCacheEntry.RequiresDynamicOverride || preparedGpuRootCacheEntry.HasExternalVisuals);
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphPrepare.atlasBindingPatched", 1);
			}
			prepared.Layout = preparedGpuRootCacheEntry.Layout;
			prepared.GpuOwnerCount = preparedGpuRootCacheEntry.OwnerStates.Length;
			prepared.RelocatableGpuState = preparedGpuRootCacheEntry.RelocatableState;
			prepared.RelocatableGpuUseAbsoluteTransform = gpuGraphUseAbsoluteTransform;
			return true;
		}
		int count = _preparedGpuOwners.Count;
		int count2 = _preparedGpuOverrides.Count;
		for (int i = 0; i < preparedGpuRootCacheEntry.OwnerStates.Length; i++)
		{
			_preparedGpuOwners.Add(preparedGpuRootCacheEntry.OwnerStates[i]);
			_preparedGpuOverrides.Add(default);
		}
		int count3 = _preparedGpuManagedVisuals.Count;
		for (int j = 0; j < preparedGpuRootCacheEntry.ManagedVisualStates.Length; j++)
		{
			_preparedGpuManagedVisuals.Add(preparedGpuRootCacheEntry.ManagedVisualStates[j]);
		}
		prepared.Layout = preparedGpuRootCacheEntry.Layout;
		prepared.GpuOwnerStart = count;
		prepared.GpuOwnerCount = preparedGpuRootCacheEntry.OwnerStates.Length;
		prepared.GpuOverrideStart = count2;
		prepared.GpuManagedVisualStart = count3;
		prepared.GpuManagedVisualCount = preparedGpuRootCacheEntry.ManagedVisualStates.Length;
		return true;
	}

	private bool TryRefreshPreparedGpuRootDynamicState(AdobeAnimateCrowdRenderState state, PreparedGpuRootCacheEntry entry, long frameVersion, float animationClockSeconds, out bool requiresDynamicOverride, out bool retainedPreviousDynamicState)
	{
		requiresDynamicOverride = false;
		retainedPreviousDynamicState = false;
		AdobeAnimateGpuRenderGraphDefinition graph = entry.Graph;
		int num = entry.OwnerStates.Length;
		if (graph == null || num <= 0 || state.GpuGraphOwners.Length != num || entry.RelocatableState.Length != entry.Layout.StateTexelCount * 4)
		{
			return false;
		}
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		for (int i = 0; i < num; i++)
		{
			long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
			AdobeAnimateSprite adobeAnimateSprite = state.GpuGraphOwners[i];
			AdobeAnimateGpuGraphOwnerState state2 = null;
			bool flag = false;
			long startBytes3 = TowerDefenseAllocationTelemetry.Begin();
			if (i == 0 && state.GpuGraphRootOwnerState?.Definition != null)
			{
				state2 = state.GpuGraphRootOwnerState;
				flag = true;
			}
			else if (adobeAnimateSprite != null)
			{
				long startBytes4 = TowerDefenseAllocationTelemetry.Begin();
				flag = adobeAnimateSprite.TryBuildGpuGraphNestedOwnerStateForRender(state.RenderMountParent, enableGpuClock: true, animationClockSeconds, out state2);
				TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshNestedBuild, startBytes4);
			}
			TowerDefenseAllocationTelemetry.End((i == 0) ? TowerDefenseAllocationMetric.RenderEncodeGpuRefreshRootState : TowerDefenseAllocationMetric.RenderEncodeGpuRefreshNestedState, startBytes3);
			long startBytes5 = TowerDefenseAllocationTelemetry.Begin();
			bool num2 = flag && state2 != null && AreGpuGraphOwnerDefinitionsEquivalent(state2.Definition, graph.Owners[i].Definition);
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshStateValidation, startBytes5);
			if (!num2)
			{
				state2 = entry.OwnerStates[i];
				if (state2 == null || !AreGpuGraphOwnerDefinitionsEquivalent(state2.Definition, graph.Owners[i].Definition) || !GodotObject.IsInstanceValid(state2.SourceSprite))
				{
					return false;
				}
				retainedPreviousDynamicState = true;
			}
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshOwnerState, startBytes2);
			long startBytes6 = TowerDefenseAllocationTelemetry.Begin();
			bool flag2 = AdobeAnimateGpuGraphStateWriter.TryPatchOwnerDynamicState(entry.RelocatableState, 0, i, state2, entry.RelocatableUseAbsoluteTransform);
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshStatePatch, startBytes6);
			if (!flag2)
			{
				return false;
			}
			AdobeAnimateGpuDynamicOverrideAllocation allocation = default;
			if (state2.HasMediaReplace)
			{
				if (!GpuDynamicOverrideAtlas.TryGetOrAdd(frameVersion, state2, out allocation))
				{
					return false;
				}
				requiresDynamicOverride = true;
			}
			long startBytes7 = TowerDefenseAllocationTelemetry.Begin();
			bool flag3 = AdobeAnimateGpuGraphStateWriter.TryPatchOwnerDynamicOverride(entry.RelocatableState, 0, num, i, in allocation);
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshOverridePatch, startBytes7);
			if (!flag3)
			{
				return false;
			}
			entry.OwnerStates[i] = state2;
		}
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshOwners, startBytes);
		long startBytes8 = TowerDefenseAllocationTelemetry.Begin();
		for (int j = 0; j < graph.ManagedVisualBindings.Length; j++)
		{
			AdobeAnimateGpuManagedVisualBinding adobeAnimateGpuManagedVisualBinding = graph.ManagedVisualBindings[j];
			if ((uint)adobeAnimateGpuManagedVisualBinding.OwnerIndex >= (uint)num)
			{
				return false;
			}
			AdobeAnimateSprite adobeAnimateSprite2 = entry.OwnerStates[adobeAnimateGpuManagedVisualBinding.OwnerIndex]?.SourceSprite;
			if (!GodotObject.IsInstanceValid(adobeAnimateSprite2))
			{
				return false;
			}
			bool flag4;
			AdobeAnimateGpuManagedVisualState state3;
			string failureReason;
			switch (adobeAnimateGpuManagedVisualBinding.SourceKind)
			{
			case AdobeAnimateGpuManagedVisualSourceKind.SlotSprite2D:
				flag4 = adobeAnimateSprite2.TryBuildManagedSlotGpuStateForRender(adobeAnimateGpuManagedVisualBinding.VisualIndex, graph.Owners[adobeAnimateGpuManagedVisualBinding.OwnerIndex].Definition.AtlasTextureArrayRid, out state3, out failureReason);
				break;
			case AdobeAnimateGpuManagedVisualSourceKind.ExternalVisual:
			{
				adobeAnimateSprite2.BeginExternalVisualPreparationForRender(frameVersion);
				long startTicks = TowerDefensePerfProfiler.BeginHotPath();
				flag4 = adobeAnimateSprite2.TryBuildExternalVisualGpuStateForRender(adobeAnimateGpuManagedVisualBinding.VisualIndex, graph.Owners[adobeAnimateGpuManagedVisualBinding.OwnerIndex].Definition.AtlasTextureArrayRid, out state3, out failureReason);
				TowerDefensePerfProfiler.End("adobeAnimate.externalVisual.encode", startTicks, 1);
				break;
			}
			default:
				return false;
			}
			if (!flag4)
			{
				state3 = entry.ManagedVisualStates[j];
				retainedPreviousDynamicState = true;
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.gpuGraphRefresh.retainedManagedVisual", 1);
			}
			if (!AdobeAnimateGpuGraphStateWriter.TryPatchManagedVisualState(entry.RelocatableState, 0, j, in state3))
			{
				return false;
			}
			entry.ManagedVisualStates[j] = state3;
		}
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeGpuRefreshExternalVisuals, startBytes8);
		entry.RequiresDynamicOverride = requiresDynamicOverride;
		return true;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool HaveSameGpuGraphOwners(AdobeAnimateSprite[] left, AdobeAnimateSprite[] right)
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

	private void StorePreparedGpuRootCache(AdobeAnimateCrowdRenderState state, AdobeAnimateSprite sprite, ulong stableOrder, AdobeAnimateGpuRenderGraphDefinition graph, in PreparedCrowdRootScratch prepared, bool requiresDynamicOverride, bool hasExternalVisuals, PreparedGpuRootCacheEntry reusableEntry)
	{
		PreparedGpuRootCacheEntry preparedGpuRootCacheEntry = reusableEntry;
		if (preparedGpuRootCacheEntry == null || preparedGpuRootCacheEntry.Root != sprite)
		{
			preparedGpuRootCacheEntry = new PreparedGpuRootCacheEntry(sprite);
		}
		sprite.StorePreparedGpuRootCacheForRender(preparedGpuRootCacheEntry);
		preparedGpuRootCacheEntry.Graph = graph;
		preparedGpuRootCacheEntry.Owners = state.GpuGraphOwners;
		preparedGpuRootCacheEntry.Layout = prepared.Layout;
		preparedGpuRootCacheEntry.RequiresDynamicOverride = requiresDynamicOverride;
		preparedGpuRootCacheEntry.HasExternalVisuals = hasExternalVisuals;
		if (preparedGpuRootCacheEntry.OwnerStates.Length != prepared.GpuOwnerCount)
		{
			preparedGpuRootCacheEntry.OwnerStates = new AdobeAnimateGpuGraphOwnerState[prepared.GpuOwnerCount];
		}
		for (int i = 0; i < prepared.GpuOwnerCount; i++)
		{
			preparedGpuRootCacheEntry.OwnerStates[i] = _preparedGpuOwners[prepared.GpuOwnerStart + i];
		}
		if (preparedGpuRootCacheEntry.ManagedVisualStates.Length != prepared.GpuManagedVisualCount)
		{
			preparedGpuRootCacheEntry.ManagedVisualStates = new AdobeAnimateGpuManagedVisualState[prepared.GpuManagedVisualCount];
		}
		for (int j = 0; j < prepared.GpuManagedVisualCount; j++)
		{
			preparedGpuRootCacheEntry.ManagedVisualStates[j] = _preparedGpuManagedVisuals[prepared.GpuManagedVisualStart + j];
		}
		if (prepared.Layout.StateTexelCount > 536870911)
		{
			preparedGpuRootCacheEntry.RelocatableState = Array.Empty<float>();
			return;
		}
		int num = prepared.Layout.StateTexelCount * 4;
		if (preparedGpuRootCacheEntry.RelocatableState.Length != num)
		{
			preparedGpuRootCacheEntry.RelocatableState = new float[num];
		}
		preparedGpuRootCacheEntry.RelocatableUseAbsoluteTransform = state.GpuGraphUseAbsoluteTransform;
		AdobeAnimateGpuGraphStateWriter.Write(preparedGpuRootCacheEntry.RelocatableState, 0, state.GpuGraphAllocation, graph, _preparedGpuOwners, prepared.GpuOwnerStart, prepared.GpuOwnerCount, _preparedGpuOverrides, prepared.GpuOverrideStart, _preparedGpuManagedVisuals, prepared.GpuManagedVisualStart, prepared.GpuManagedVisualCount, preparedGpuRootCacheEntry.RelocatableUseAbsoluteTransform, default(AdobeAnimateRootMotionState), 0f);
		state.StorePreparedRelocatableGpuState(in preparedGpuRootCacheEntry.Layout, preparedGpuRootCacheEntry.RelocatableState, preparedGpuRootCacheEntry.RelocatableUseAbsoluteTransform, requiresDynamicOverride | hasExternalVisuals);
	}

	private static void PrepareGpuRenderGraphAtlasForFrame(long renderFrame)
	{
		if (!GpuRenderGraphEnabled || GpuGraphAtlasPreparedFrame == renderFrame)
		{
			return;
		}
		if (GpuGraphAtlas.AllocationCount >= GpuGraphDefinitions.Count)
		{
			GpuGraphAtlasPreparedFrame = renderFrame;
			return;
		}
		foreach (AdobeAnimateGpuRenderGraphDefinition value in GpuGraphDefinitions.Values)
		{
			GpuGraphAtlas.TryGetOrAdd(value, out var _);
		}
		GpuGraphAtlasPreparedFrame = renderFrame;
	}

	private static bool IsGpuGraphPreparedForTransaction(AdobeAnimateSprite root)
	{
		if (!GpuRenderGraphEnabled || root == null)
		{
			return false;
		}
		return root.HasPreparedGpuRenderGraph(GpuGraphPreparationGeneration);
	}

	internal static bool TryResolveGpuRenderGraph(AdobeAnimateSprite root, out AdobeAnimateGpuRenderGraphDefinition graph, out AdobeAnimateSprite[] graphOwners, out AdobeAnimateGpuRenderGraphAllocation allocation, out Texture2DArray textureArray, out Vector2I textureSize, bool allowTransactionalAtlasAdd = false)
	{
		graph = null;
		graphOwners = Array.Empty<AdobeAnimateSprite>();
		allocation = default;
		textureArray = null;
		textureSize = Vector2I.Zero;
		if (!GpuRenderGraphEnabled || root == null)
		{
			return false;
		}
		ulong cachedInstanceIdForRender = root.GetCachedInstanceIdForRender();
		if ((!GpuGraphRoots.TryGetValue(cachedInstanceIdForRender, out var value) || value.Root != root) && !TryBuildAndCacheGpuRenderGraphRoot(root, cachedInstanceIdForRender, out value))
		{
			return false;
		}
		graph = value.Graph;
		graphOwners = value.Owners;
		if (!GpuGraphAtlas.TryGet(graph.Signature, out allocation))
		{
			if (RuntimeTransactionInProgress && !allowTransactionalAtlasAdd)
			{
				return false;
			}
			if (!GpuGraphAtlas.TryGetOrAdd(graph, out allocation))
			{
				return false;
			}
		}
		textureArray = GpuGraphAtlas.TextureArray;
		textureSize = GpuGraphAtlas.PageSize;
		if (textureArray != null && textureSize.X > 0)
		{
			return textureSize.Y > 0;
		}
		return false;
	}

	private static bool TryBuildAndCacheGpuRenderGraphRoot(AdobeAnimateSprite root, ulong rootId, out GpuGraphRootCacheEntry rootEntry)
	{
		rootEntry = null;
		if (!GodotObject.IsInstanceValid(root) || !AdobeAnimateGpuRenderGraphBuilder.TryBuild(root, GpuGraphDefinitions, out var graph, out var ownerSprites))
		{
			return false;
		}
		GpuRenderGraphBuildCount++;
		if (GpuGraphEverBuiltRootIds.Add(rootId))
		{
			GpuRenderGraphInitialBuildCount++;
		}
		else
		{
			GpuRenderGraphRebuildCount++;
		}
		if (GpuGraphDefinitions.TryGetValue(graph.Signature, out var value))
		{
			graph = value;
		}
		else
		{
			GpuGraphDefinitions.Add(graph.Signature, graph);
		}
		GpuGraphDefinitionLastUsedFrames[graph.Signature] = GetLifecycleTick();
		GpuGraphMaxRenderSlotCount = Math.Max(GpuGraphMaxRenderSlotCount, graph.RenderSlots.Length);
		rootEntry = new GpuGraphRootCacheEntry(root, graph, ownerSprites);
		GpuGraphRoots[rootId] = rootEntry;
		root.MarkGpuRenderGraphPrepared(GpuGraphPreparationGeneration, graph.Signature, graph.RenderSlots.Length);
		return true;
	}

	internal static bool IsGpuGraphReady(AdobeAnimateSprite root)
	{
		if (GpuRenderGraphEnabled && root != null && root.IsRuntimeInsideTree)
		{
			return root.HasPreparedGpuRenderGraph(GpuGraphPreparationGeneration);
		}
		return false;
	}

	internal static bool IsGpuGraphStateCurrent(AdobeAnimateSprite root, ulong signature)
	{
		if (GpuRenderGraphEnabled && root != null && root.IsRuntimeInsideTree)
		{
			return root.HasPreparedGpuRenderGraphState(GpuGraphPreparationGeneration, signature);
		}
		return false;
	}

	internal static bool TryRefreshGpuGraphStateAtlasBinding(AdobeAnimateCrowdRenderState state)
	{
		if (!GpuRenderGraphEnabled || state == null || state.Mode != AdobeAnimateCrowdRenderMode.GpuGraph)
		{
			return false;
		}
		Texture2DArray textureArray = GpuGraphAtlas.TextureArray;
		Vector2I textureSize = GpuGraphAtlas.PageSize;
		if (state.GpuGraphTextureArray == textureArray && state.GpuGraphTextureSize == textureSize && textureSize.X > 0 && textureSize.Y > 0)
		{
			return true;
		}
		if (!GodotObject.IsInstanceValid(textureArray) || !textureArray.GetRid().IsValid || textureSize.X <= 0 || textureSize.Y <= 0)
		{
			return false;
		}
		if (!GpuGraphAtlas.TryGet(state.GpuGraphAllocation.Signature, out var allocation))
		{
			return false;
		}
		return state.RefreshGpuGraphAtlasBinding(in allocation, textureArray, in textureSize);
	}

	internal static void InvalidateGpuRenderGraph(AdobeAnimateSprite root, AdobeAnimateGpuGraphInvalidationReason reason)
	{
		if (!GodotObject.IsInstanceValid(root) || (reason == AdobeAnimateGpuGraphInvalidationReason.OwnerExit && !root.IsQueuedForDeletion()))
		{
			return;
		}
		root.InvalidateGpuRenderGraphPreparation();
		ulong cachedInstanceIdForRender = root.GetCachedInstanceIdForRender();
		GpuGraphRootCacheEntry value;
		if (reason == AdobeAnimateGpuGraphInvalidationReason.OwnerExit)
		{
			PendingGpuGraphInvalidations.Remove(cachedInstanceIdForRender);
			if (GpuGraphRoots.Remove(cachedInstanceIdForRender))
			{
				GpuRenderGraphOwnerExitInvalidationCount++;
			}
		}
		else if (GpuGraphRoots.TryGetValue(cachedInstanceIdForRender, out value) && value.Root == root)
		{
			if (!PendingGpuGraphInvalidations.TryGetValue(cachedInstanceIdForRender, out var value2) || value2.Root != root)
			{
				value2 = new PendingGpuGraphInvalidation(root);
				PendingGpuGraphInvalidations[cachedInstanceIdForRender] = value2;
			}
			value2.Include(reason);
		}
	}

	private static void FlushPendingGpuGraphInvalidations()
	{
		if (PendingGpuGraphInvalidations.Count == 0)
		{
			return;
		}
		PendingGpuGraphInvalidationScratch.Clear();
		foreach (ulong key in PendingGpuGraphInvalidations.Keys)
		{
			PendingGpuGraphInvalidationScratch.Add(key);
		}
		for (int i = 0; i < PendingGpuGraphInvalidationScratch.Count; i++)
		{
			ulong num = PendingGpuGraphInvalidationScratch[i];
			if (!PendingGpuGraphInvalidations.TryGetValue(num, out var value))
			{
				continue;
			}
			PendingGpuGraphInvalidations.Remove(num);
			AdobeAnimateSprite root = value.Root;
			if (GodotObject.IsInstanceValid(root) && GpuGraphRoots.TryGetValue(num, out var value2) && value2.Root == root)
			{
				GpuGraphRoots.Remove(num);
				if (value.OwnerDefinition)
				{
					GpuRenderGraphOwnerDefinitionInvalidationCount++;
				}
				if (value.ManagedSlotTopology)
				{
					GpuRenderGraphManagedSlotInvalidationCount++;
				}
				if (value.ExternalVisualTopology)
				{
					GpuRenderGraphExternalVisualInvalidationCount++;
				}
				TryBuildAndCacheGpuRenderGraphRoot(root, num, out var _);
			}
		}
		PendingGpuGraphInvalidationScratch.Clear();
	}

	private static void PruneInvalidGpuGraphRoots()
	{
		long lifecycleTick = GetLifecycleTick();
		if (LastGpuGraphRootPruneTick != -9223372036854775808L && lifecycleTick - LastGpuGraphRootPruneTick < 120)
		{
			return;
		}
		LastGpuGraphRootPruneTick = lifecycleTick;
		GpuGraphRootRemoveScratch.Clear();
		foreach (KeyValuePair<ulong, GpuGraphRootCacheEntry> gpuGraphRoot in GpuGraphRoots)
		{
			AdobeAnimateSprite root = gpuGraphRoot.Value.Root;
			if (!GodotObject.IsInstanceValid(root) || (!root.IsInsideTree() && root.IsQueuedForDeletion()))
			{
				GpuGraphRootRemoveScratch.Add(gpuGraphRoot.Key);
			}
		}
		for (int i = 0; i < GpuGraphRootRemoveScratch.Count; i++)
		{
			ulong key = GpuGraphRootRemoveScratch[i];
			GpuGraphRoots.Remove(key);
			PendingGpuGraphInvalidations.Remove(key);
		}
		GpuGraphRootRemoveScratch.Clear();
	}

	private static void MaintainGpuGraphDefinitions()
	{
		long lifecycleTick = GetLifecycleTick();
		bool flag = false;
		if (!flag && LastGpuGraphDefinitionMaintenanceTick != -9223372036854775808L && lifecycleTick - LastGpuGraphDefinitionMaintenanceTick < 900)
		{
			return;
		}
		LastGpuGraphDefinitionMaintenanceTick = lifecycleTick;
		ActiveGpuGraphDefinitionSignatures.Clear();
		foreach (GpuGraphRootCacheEntry value2 in GpuGraphRoots.Values)
		{
			ulong valueOrDefault = (value2?.Graph?.Signature).GetValueOrDefault();
			if (valueOrDefault != 0L)
			{
				ActiveGpuGraphDefinitionSignatures.Add(valueOrDefault);
				GpuGraphDefinitionLastUsedFrames[valueOrDefault] = lifecycleTick;
			}
		}
		long num = Math.Max(0L, lifecycleTick - 3600);
		GpuGraphDefinitionRemoveScratch.Clear();
		foreach (ulong key2 in GpuGraphDefinitions.Keys)
		{
			if (!ActiveGpuGraphDefinitionSignatures.Contains(key2))
			{
				long value;
				if (flag)
				{
					GpuGraphDefinitionRemoveScratch.Add(key2);
				}
				else if (!GpuGraphDefinitionLastUsedFrames.TryGetValue(key2, out value))
				{
					GpuGraphDefinitionLastUsedFrames[key2] = lifecycleTick;
				}
				else if (value < num)
				{
					GpuGraphDefinitionRemoveScratch.Add(key2);
				}
			}
		}
		ActiveGpuGraphDefinitionSignatures.Clear();
		int num2 = Math.Max(32, GpuGraphDefinitions.Count / 8);
		if (GpuGraphDefinitionRemoveScratch.Count == 0 || (!flag && (GpuGraphAtlas.EstimatedTextureBytes < 134217728 || GpuGraphDefinitionRemoveScratch.Count < num2)))
		{
			GpuGraphDefinitionRemoveScratch.Clear();
			return;
		}
		int count = GpuGraphDefinitionRemoveScratch.Count;
		for (int i = 0; i < GpuGraphDefinitionRemoveScratch.Count; i++)
		{
			ulong key = GpuGraphDefinitionRemoveScratch[i];
			GpuGraphDefinitions.Remove(key);
			GpuGraphDefinitionLastUsedFrames.Remove(key);
		}
		GpuGraphDefinitionRemoveScratch.Clear();
		GpuGraphAtlas.ResetForTests();
		GpuGraphAtlasPreparedFrame = -9223372036854775808L;
		GpuGraphMaxRenderSlotCount = 1;
		foreach (AdobeAnimateGpuRenderGraphDefinition value3 in GpuGraphDefinitions.Values)
		{
			GpuGraphMaxRenderSlotCount = Math.Max(GpuGraphMaxRenderSlotCount, value3.RenderSlots.Length);
		}
		TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.graphAtlas.prunedDefinitions", count);
		TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.graphAtlas.retainedDefinitions", GpuGraphDefinitions.Count);
	}

	public static void ClearGpuRenderGraphCaches(bool notifyRuntime = true)
	{
		GpuGraphPreparationGeneration = ((GpuGraphPreparationGeneration == 18446744073709551615uL) ? 1 : (GpuGraphPreparationGeneration + 1));
		GpuGraphDefinitions.Clear();
		GpuGraphRoots.Clear();
		PendingGpuGraphInvalidations.Clear();
		GpuGraphDefinitionLastUsedFrames.Clear();
		GpuGraphEverBuiltRootIds.Clear();
		ActiveGpuGraphDefinitionSignatures.Clear();
		GpuGraphAtlas.ResetForTests();
		GpuDynamicOverrideAtlas.ResetForTests();
		GpuGraphAtlasPreparedFrame = -9223372036854775808L;
		GpuGraphMaxRenderSlotCount = 1;
		GpuRenderGraphBuildCount = 0L;
		GpuRenderGraphInitialBuildCount = 0L;
		GpuRenderGraphRebuildCount = 0L;
		GpuRenderGraphOwnerExitInvalidationCount = 0L;
		GpuRenderGraphOwnerDefinitionInvalidationCount = 0L;
		GpuRenderGraphManagedSlotInvalidationCount = 0L;
		GpuRenderGraphExternalVisualInvalidationCount = 0L;
		LastBucketMaintenanceTick = -9223372036854775808L;
		LastGpuGraphRootPruneTick = -9223372036854775808L;
		LastGpuGraphDefinitionMaintenanceTick = -9223372036854775808L;
		GpuGraphRootRemoveScratch.Clear();
		GpuGraphDefinitionRemoveScratch.Clear();
		PendingGpuGraphInvalidationScratch.Clear();
		GpuManagedVisualFailureReasonsLogged.Clear();
		AggregateRenderStats = default;
		if (notifyRuntime)
		{
			AdobeAnimateRuntimeManager.NotifyGpuRenderCachesCleared();
		}
	}

	private void PublishRuntimeFrame(long frameVersion, bool resourcesFrozen, AdobeAnimateMultiMeshRdUploadDispatcher.Batch rdBatch)
	{
		if (CountEncodedCrowdGroups() > 0 && !_hasFinalResourceSignature)
		{
			ConvertAllCrowdGroupsToFallback(frameVersion);
		}
		long startTicks = TowerDefensePerfProfiler.Begin();
		PrepareSharedCrowdResources(frameVersion);
		TowerDefensePerfProfiler.End("adobeAnimate.render.crowd.prepareShared", startTicks, _frameGroupsInSubmissionOrder.Count);
		int num = CountEncodedCrowdGroups();
		long startTicks2 = TowerDefensePerfProfiler.Begin();
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		bool flag = num == 0 || _crowdStateArena.UploadOnce();
		if (flag && num > 0)
		{
			_frameStateTexels = _crowdStateArena.WrittenTexels;
			_frameStateUploads = _crowdStateArena.UploadCountThisFrame;
			_frameStateUpdatedLayers = _crowdStateArena.UpdatedLayerCountThisFrame;
			_frameStateUploadedBytes = _crowdStateArena.UploadedBytesThisFrame;
			AdobeAnimateMultiMeshBatcher.ApplySharedCrowdBindings(_crowdMaterial, in _finalResourceSignature, _crowdStateArena);
		}
		TowerDefensePerfProfiler.End("adobeAnimate.render.crowd.uploadState", startTicks2, _frameStateUploads);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.StateTextureUpload, startBytes);
		long startTicks3 = TowerDefensePerfProfiler.Begin();
		long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
		if (!flag)
		{
			ConvertAllCrowdGroupsToFallback(frameVersion);
		}
		bool flag2 = false;
		for (int i = 0; i < _frameGroupsInSubmissionOrder.Count; i++)
		{
			CrowdFrameGroup crowdFrameGroup = _frameGroupsInSubmissionOrder[i];
			if (!crowdFrameGroup.CrowdEncoded)
			{
				continue;
			}
			long startBytes3 = TowerDefenseAllocationTelemetry.Begin();
			bool num2 = crowdFrameGroup.CrowdBucket != null && crowdFrameGroup.CrowdBucket.Publish(rdBatch, 65536);
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.MultiMeshUpload, startBytes3);
			if (num2)
			{
				crowdFrameGroup.CrowdPublished = true;
				crowdFrameGroup.HideCrowdBucketAfterFallback = false;
				if (!flag2)
				{
					flag2 = true;
				}
			}
			else
			{
				TryPrepareOrderedFallback(crowdFrameGroup, frameVersion, rebuildResourceSignature: false);
			}
		}
		if (flag)
		{
			for (int j = 0; j < _frameGroupsInSubmissionOrder.Count; j++)
			{
				CrowdFrameGroup crowdFrameGroup2 = _frameGroupsInSubmissionOrder[j];
				if (crowdFrameGroup2.CrowdPublished)
				{
					CommitPreparedRuntimeNativeCanvasTakeover(crowdFrameGroup2, frameVersion);
					long startBytes4 = TowerDefenseAllocationTelemetry.Begin();
					CommitPreparedExternalVisualCrowdFrame(crowdFrameGroup2, frameVersion);
					TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.ExternalVisualCommit, startBytes4);
					_framePublishedZBuckets++;
					_framePublishedCrowdRoots += crowdFrameGroup2.PreparedRoots.Count;
					_frameMultiMeshUploads++;
				}
			}
		}
		for (int k = 0; k < _frameGroupsInSubmissionOrder.Count; k++)
		{
			CrowdFrameGroup crowdFrameGroup3 = _frameGroupsInSubmissionOrder[k];
			if (crowdFrameGroup3.FallbackPrepared && crowdFrameGroup3.FallbackBucket != null)
			{
				crowdFrameGroup3.FallbackBucket.ApplyAnimationClock(CurrentAnimationClockSeconds, CurrentPhysicsInterpolationFraction);
				if (!crowdFrameGroup3.FallbackBucket.TryPublish())
				{
					crowdFrameGroup3.FallbackBucket.Hide();
					continue;
				}
				crowdFrameGroup3.FallbackPublished = true;
				HideCrowdBucketAfterPublishedFallback(crowdFrameGroup3, rdBatch);
				RestoreUnrenderedFallbackNativeCanvasLayers(crowdFrameGroup3);
				CommitRenderedFallbackRuntimeNativeCanvasTakeover(crowdFrameGroup3, frameVersion);
				long startBytes5 = TowerDefenseAllocationTelemetry.Begin();
				CommitExternalVisualCrowdFrame(crowdFrameGroup3, frameVersion);
				TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.ExternalVisualCommit, startBytes5);
				_frameFallbackRuns += crowdFrameGroup3.FallbackBucket.ActiveRunCount;
				_frameFallbackRoots += crowdFrameGroup3.FallbackBucket.RootCount;
			}
		}
		for (int l = 0; l < _frameGroupsInSubmissionOrder.Count; l++)
		{
			CrowdFrameGroup crowdFrameGroup4 = _frameGroupsInSubmissionOrder[l];
			if (!crowdFrameGroup4.CrowdPublished && !crowdFrameGroup4.FallbackPublished)
			{
				RestoreRuntimeNativeCanvasLayers(crowdFrameGroup4);
				RestoreExternalVisualNativeFallbacks(crowdFrameGroup4, frameVersion);
			}
		}
		TowerDefensePerfProfiler.End("adobeAnimate.render.crowd.uploadBuckets", startTicks3, _frameMultiMeshUploads + _frameFallbackRuns);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.BucketPublish, startBytes2);
		HideUntouchedBuckets(frameVersion, rdBatch);
		_hasCurrentOutput = _framePublishedZBuckets > 0 || _frameFallbackRoots > 0;
	}

	private void AbortRuntimeFrame(long frameVersion)
	{
		for (int i = 0; i < _frameGroupsInSubmissionOrder.Count; i++)
		{
			CrowdFrameGroup crowdFrameGroup = _frameGroupsInSubmissionOrder[i];
			RestoreRuntimeNativeCanvasLayers(crowdFrameGroup);
			RestoreExternalVisualNativeFallbacks(crowdFrameGroup, frameVersion);
			crowdFrameGroup.ResetForFrame();
		}
		foreach (AdobeAnimateZIndexCrowdBucket value in _crowdBuckets.Values)
		{
			value.AbortFrame(frameVersion);
		}
		foreach (AdobeAnimateOrderedSnapshotFallbackBucket value2 in _fallbackBuckets.Values)
		{
			value2.AbortFrame(frameVersion);
		}
		_frameGroupsInSubmissionOrder.Clear();
		_preparedDrawItems.Clear();
		_preparedGpuOwners.Clear();
		_preparedGpuOverrides.Clear();
		_preparedGpuManagedVisuals.Clear();
		_preparedCompositeOwners.Clear();
		_unifiedDrawItems.Clear();
		_frameTextureValidity.Clear();
		_frameTexture2DValidity.Clear();
		_resourceSignatureBuilder.Reset();
		_currentTouchedCrowdBuckets.Clear();
		_currentTouchedFallbackBuckets.Clear();
		_crowdStateArena.AbortFrame(frameVersion);
		_fallbackStateArena.AbortFrame(frameVersion);
		_runtimeFrameVersion = -9223372036854775808L;
		_transactionFrameMarker = -9223372036854775808L;
		_currentFrameMarker = -9223372036854775808L;
		_hasFinalResourceSignature = false;
		_hasCurrentOutput = _hadCurrentOutputAtFrameBegin;
		_frameSignatureConflictBuckets = 0;
		_frameMeshRebinds = 0;
		_framePublishedCrowdRoots = 0;
		_framePublishedZBuckets = 0;
		_frameStateTexels = 0;
		_frameStateUploads = 0;
		_frameStateUpdatedLayers = 0;
		_frameStateUploadedBytes = 0;
		_frameMultiMeshUploads = 0;
		_frameFallbackRuns = 0;
		_frameFallbackRoots = 0;
	}

	private static void CommitExternalVisualCrowdFrame(CrowdFrameGroup group, long frameVersion)
	{
		Span<CrowdSubmission> span = CollectionsMarshal.AsSpan(group.Submissions);
		for (int i = 0; i < span.Length; i++)
		{
			AdobeAnimateSprite sprite = span[i].Sprite;
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.CommitExternalVisualCrowdFrame(frameVersion);
			}
		}
	}

	private static void CommitPreparedRuntimeNativeCanvasTakeover(CrowdFrameGroup group, long frameVersion)
	{
		Span<PreparedCrowdRoot> span = CollectionsMarshal.AsSpan(group.PreparedRoots);
		Span<CrowdSubmission> span2 = CollectionsMarshal.AsSpan(group.Submissions);
		for (int i = 0; i < span.Length; i++)
		{
			int submissionIndex = span[i].SubmissionIndex;
			if ((uint)submissionIndex < (uint)span2.Length)
			{
				AdobeAnimateSprite sprite = span2[submissionIndex].Sprite;
				if (GodotObject.IsInstanceValid(sprite))
				{
					sprite.CommitRuntimeNativeCanvasTakeover(frameVersion);
				}
			}
		}
	}

	private static void RestoreRuntimeNativeCanvasLayers(CrowdFrameGroup group)
	{
		Span<CrowdSubmission> span = CollectionsMarshal.AsSpan(group.Submissions);
		for (int i = 0; i < span.Length; i++)
		{
			AdobeAnimateSprite sprite = span[i].Sprite;
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.RestoreRuntimeNativeCanvasLayer();
			}
		}
	}

	private static void RestoreUnrenderedFallbackNativeCanvasLayers(CrowdFrameGroup group)
	{
		Span<CrowdSubmission> span = CollectionsMarshal.AsSpan(group.Submissions);
		Span<int> span2 = CollectionsMarshal.AsSpan(group.RenderedFallbackSubmissionIndices);
		int i = 0;
		for (int j = 0; j < span.Length; j++)
		{
			for (; i < span2.Length && span2[i] < j; i++)
			{
			}
			if (i < span2.Length && span2[i] == j)
			{
				i++;
				continue;
			}
			AdobeAnimateSprite sprite = span[j].Sprite;
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.RestoreRuntimeNativeCanvasLayer();
			}
		}
	}

	private static void CommitRenderedFallbackRuntimeNativeCanvasTakeover(CrowdFrameGroup group, long frameVersion)
	{
		Span<CrowdSubmission> span = CollectionsMarshal.AsSpan(group.Submissions);
		Span<int> span2 = CollectionsMarshal.AsSpan(group.RenderedFallbackSubmissionIndices);
		for (int i = 0; i < span2.Length; i++)
		{
			int num = span2[i];
			if ((uint)num < (uint)span.Length)
			{
				AdobeAnimateSprite sprite = span[num].Sprite;
				if (GodotObject.IsInstanceValid(sprite))
				{
					sprite.CommitRuntimeNativeCanvasTakeover(frameVersion);
				}
			}
		}
	}

	private static void CommitPreparedExternalVisualCrowdFrame(CrowdFrameGroup group, long frameVersion)
	{
		Span<PreparedCrowdRoot> span = CollectionsMarshal.AsSpan(group.PreparedRoots);
		Span<CrowdSubmission> span2 = CollectionsMarshal.AsSpan(group.Submissions);
		for (int i = 0; i < span.Length; i++)
		{
			ref PreparedCrowdRoot reference = ref span[i];
			if ((uint)reference.DetailIndex < (uint)group.PreparedRootDetails.Count && CollectionsMarshal.AsSpan(group.PreparedRootDetails)[reference.DetailIndex].HasExternalVisuals && (uint)reference.SubmissionIndex < (uint)span2.Length)
			{
				AdobeAnimateSprite sprite = span2[reference.SubmissionIndex].Sprite;
				if (GodotObject.IsInstanceValid(sprite))
				{
					sprite.CommitExternalVisualCrowdFrame(frameVersion);
				}
			}
		}
	}

	private static void RestoreExternalVisualNativeFallbacks(CrowdFrameGroup group, long frameVersion)
	{
		Span<CrowdSubmission> span = CollectionsMarshal.AsSpan(group.Submissions);
		for (int i = 0; i < span.Length; i++)
		{
			AdobeAnimateSprite sprite = span[i].Sprite;
			if (GodotObject.IsInstanceValid(sprite))
			{
				sprite.RestoreExternalVisualNativeFallbacks(frameVersion);
			}
		}
	}

	private void PrepareSharedCrowdResources(long frameVersion)
	{
		if (CountEncodedCrowdGroups() == 0)
		{
			return;
		}
		if (!_crowdStateArena.WarmupTexture() || !EnsureCrowdMaterialMatchesFinalSignature())
		{
			ConvertAllCrowdGroupsToFallback(frameVersion);
			return;
		}
		ApplyCrowdClockIfNeeded();
		int num = 0;
		if (!CrowdMeshCapacityClassesEnabled)
		{
			for (int i = 0; i < _frameGroupsInSubmissionOrder.Count; i++)
			{
				CrowdFrameGroup crowdFrameGroup = _frameGroupsInSubmissionOrder[i];
				if (crowdFrameGroup.CrowdEncoded)
				{
					num = Math.Max(num, crowdFrameGroup.MaxQuadCount);
				}
			}
		}
		long startTicks = TowerDefensePerfProfiler.Begin();
		BindCurrentCrowdBuckets(frameVersion, num);
		TowerDefensePerfProfiler.End("adobeAnimate.render.crowd.bindBuckets", startTicks, _frameGroupsInSubmissionOrder.Count);
	}

	private bool EnsureCrowdMaterialMatchesFinalSignature()
	{
		if (!_hasFinalResourceSignature)
		{
			return false;
		}
		TextureLayered texture = _crowdStateArena.Texture;
		if (!GodotObject.IsInstanceValid(texture) || !texture.GetRid().IsValid)
		{
			return false;
		}
		Rid rid = texture.GetRid();
		if (GodotObject.IsInstanceValid(_crowdMaterial) && _hasCrowdMaterialResourceSignature && _crowdMaterialResourceSignature == _finalResourceSignature && _crowdMaterialStateTextureRid == rid && _crowdMaterialStateTextureSize == _crowdStateArena.TextureSize && _crowdMaterialStateTextureLayers == _crowdStateArena.TextureLayerCount)
		{
			return true;
		}
		ShaderMaterial shaderMaterial = AdobeAnimateMultiMeshBatcher.CreateSharedCrowdMaterial();
		if (!GodotObject.IsInstanceValid(shaderMaterial))
		{
			return false;
		}
		AdobeAnimateMultiMeshBatcher.ApplySharedCrowdBindings(shaderMaterial, in _finalResourceSignature, _crowdStateArena);
		AdobeAnimateMultiMeshBatcher.SetSharedCrowdAnimationClock(shaderMaterial, CurrentAnimationClockSeconds);
		AdobeAnimateMultiMeshBatcher.SetSharedCrowdPhysicsInterpolationFraction(shaderMaterial, CurrentPhysicsInterpolationFraction);
		_crowdMaterial = shaderMaterial;
		_crowdMaterialResourceSignature = _finalResourceSignature;
		_crowdMaterialStateTextureRid = rid;
		_crowdMaterialStateTextureSize = _crowdStateArena.TextureSize;
		_crowdMaterialStateTextureLayers = _crowdStateArena.TextureLayerCount;
		_hasCrowdMaterialResourceSignature = true;
		_animationClockMaterial = shaderMaterial;
		_appliedAnimationClockVersion = CurrentAnimationClockVersion;
		return true;
	}

	private void BindCurrentCrowdBuckets(long frameVersion, int sharedRequiredQuadCapacity)
	{
		for (int i = 0; i < _frameGroupsInSubmissionOrder.Count; i++)
		{
			CrowdFrameGroup crowdFrameGroup = _frameGroupsInSubmissionOrder[i];
			if (!crowdFrameGroup.CrowdEncoded || crowdFrameGroup.CrowdBucket == null)
			{
				continue;
			}
			int requiredQuadCapacity = ((sharedRequiredQuadCapacity > 0) ? sharedRequiredQuadCapacity : crowdFrameGroup.MaxQuadCount);
			if (!TryGetOrCreateCrowdMesh(requiredQuadCapacity, out var resource))
			{
				TryPrepareOrderedFallback(crowdFrameGroup, frameVersion, rebuildResourceSignature: false);
				continue;
			}
			bool flag = crowdFrameGroup.CrowdBucket.BoundMeshVersion != resource.Version;
			if (!crowdFrameGroup.CrowdBucket.TryPreparePublication(resource.Mesh, resource.Version, _crowdMaterial))
			{
				TryPrepareOrderedFallback(crowdFrameGroup, frameVersion, rebuildResourceSignature: false);
			}
			else if (flag)
			{
				_frameMeshRebinds++;
			}
		}
	}

	private bool TryGetOrCreateCrowdMesh(int requiredQuadCapacity, out CrowdMeshResource resource)
	{
		resource = null;
		int crowdMeshCapacityClass = GetCrowdMeshCapacityClass(requiredQuadCapacity);
		if (_crowdMeshesByQuadCapacity.TryGetValue(crowdMeshCapacityClass, out resource) && resource != null && GodotObject.IsInstanceValid(resource.Mesh))
		{
			return true;
		}
		ArrayMesh arrayMesh = AdobeAnimateMultiMeshBatcher.CreateSharedCrowdMesh(crowdMeshCapacityClass);
		if (!GodotObject.IsInstanceValid(arrayMesh))
		{
			return false;
		}
		int num = _crowdMeshVersion + 1;
		if (num <= 0)
		{
			num = 1;
		}
		_crowdMeshVersion = num;
		resource?.Mesh?.Dispose();
		resource = new CrowdMeshResource(arrayMesh, crowdMeshCapacityClass, num);
		_crowdMeshesByQuadCapacity[crowdMeshCapacityClass] = resource;
		if (crowdMeshCapacityClass >= _crowdMeshQuadCapacity)
		{
			_crowdMesh = arrayMesh;
			_crowdMeshQuadCapacity = crowdMeshCapacityClass;
		}
		return true;
	}

	private static int GetCrowdMeshCapacityClass(int requiredQuadCapacity)
	{
		return CapacityForCrowdMeshSlots(requiredQuadCapacity);
	}

	internal static int CapacityForCrowdMeshSlots(int requiredSlots)
	{
		int num = Math.Max(1, requiredSlots);
		int num2 = num % 8;
		if (num2 == 0 || num > 2147483639)
		{
			return num;
		}
		return num + 8 - num2;
	}

	private void HideUntouchedBuckets(long frameVersion, AdobeAnimateMultiMeshRdUploadDispatcher.Batch rdBatch)
	{
		List<AdobeAnimateZIndexCrowdBucket> previousTouchedCrowdBuckets = _previousTouchedCrowdBuckets;
		for (int i = 0; i < previousTouchedCrowdBuckets.Count; i++)
		{
			if (previousTouchedCrowdBuckets[i].LastTouchedRenderFrameVersion != frameVersion && !previousTouchedCrowdBuckets[i].TryQueueHide(rdBatch))
			{
				throw new InvalidOperationException("Adobe Animate 未触及 Crowd 桶无法加入 RD 隐藏批次。");
			}
		}
		List<AdobeAnimateOrderedSnapshotFallbackBucket> previousTouchedFallbackBuckets = _previousTouchedFallbackBuckets;
		for (int j = 0; j < previousTouchedFallbackBuckets.Count; j++)
		{
			if (previousTouchedFallbackBuckets[j].LastTouchedRenderFrameVersion != frameVersion)
			{
				previousTouchedFallbackBuckets[j].Hide();
			}
		}
		_previousTouchedCrowdBuckets = _currentTouchedCrowdBuckets;
		_currentTouchedCrowdBuckets = previousTouchedCrowdBuckets;
		_currentTouchedCrowdBuckets.Clear();
		_previousTouchedFallbackBuckets = _currentTouchedFallbackBuckets;
		_currentTouchedFallbackBuckets = previousTouchedFallbackBuckets;
		_currentTouchedFallbackBuckets.Clear();
	}

	private static void HideCrowdBucketAfterPublishedFallback(CrowdFrameGroup group, AdobeAnimateMultiMeshRdUploadDispatcher.Batch rdBatch)
	{
		if (group.HideCrowdBucketAfterFallback)
		{
			if (group.CrowdBucket != null && !group.CrowdBucket.TryQueueHide(rdBatch))
			{
				throw new InvalidOperationException("Adobe Animate 回退接管后无法加入 Crowd RD 隐藏批次。");
			}
			group.HideCrowdBucketAfterFallback = false;
		}
	}

	private bool SubmitImmediateSnapshot(in AdobeAnimateRenderSnapshot snapshot, long frameVersion, bool immediateFlush, bool forceCpuPose)
	{
		if (snapshot.RenderMountParent != _mountParent || !GodotObject.IsInstanceValid(_mountParent))
		{
			return false;
		}
		if (!_immediateSnapshotBuckets.TryGetValue(snapshot.EffectiveZIndex, out var value))
		{
			value = new AdobeAnimateOrderedSnapshotFallbackBucket();
			value.Attach(_mountParent, snapshot.EffectiveZIndex);
			_immediateSnapshotBuckets.Add(snapshot.EffectiveZIndex, value);
		}
		long lifecycleTick = GetLifecycleTick();
		RunGlobalBucketMaintenance(lifecycleTick);
		value.BeginImmediateFrame(frameVersion, lifecycleTick);
		if (!value.TryAppendImmediateSnapshot(in snapshot, forceCpuPose))
		{
			snapshot.Sprite.RestoreExternalVisualNativeFallbacks(frameVersion);
			return false;
		}
		if (!immediateFlush)
		{
			return true;
		}
		if (!value.TryPublish())
		{
			snapshot.Sprite.RestoreExternalVisualNativeFallbacks(frameVersion);
			return false;
		}
		snapshot.Sprite.CommitExternalVisualCrowdFrame(frameVersion);
		return true;
	}

	private bool PublishImmediateSnapshotBuckets()
	{
		bool result = false;
		foreach (AdobeAnimateOrderedSnapshotFallbackBucket value in _immediateSnapshotBuckets.Values)
		{
			if (value.HasImmediateSubmissions && value.TryPublish())
			{
				result = true;
			}
		}
		return result;
	}

	private void HideRuntimeOutputForFailureHandoff()
	{
		foreach (AdobeAnimateZIndexCrowdBucket value in _crowdBuckets.Values)
		{
			value.Hide();
		}
		foreach (AdobeAnimateOrderedSnapshotFallbackBucket value2 in _fallbackBuckets.Values)
		{
			value2.Hide();
		}
	}

	private void HideAllOutput()
	{
		foreach (AdobeAnimateZIndexCrowdBucket value in _crowdBuckets.Values)
		{
			value.Hide();
		}
		foreach (AdobeAnimateOrderedSnapshotFallbackBucket value2 in _fallbackBuckets.Values)
		{
			value2.Hide();
		}
		foreach (AdobeAnimateOrderedSnapshotFallbackBucket value3 in _immediateSnapshotBuckets.Values)
		{
			value3.Hide();
		}
	}

	private void HideAndDisposeOwnedResources()
	{
		HideAllOutput();
		bool queueNodeCleanup = GodotObject.IsInstanceValid(_mountParent) && !_mountParent.IsQueuedForDeletion();
		foreach (AdobeAnimateZIndexCrowdBucket value in _crowdBuckets.Values)
		{
			value.ReleaseForOwnerExit(queueNodeCleanup);
		}
		foreach (AdobeAnimateOrderedSnapshotFallbackBucket value2 in _fallbackBuckets.Values)
		{
			value2.ReleaseForOwnerExit(queueNodeCleanup);
		}
		foreach (AdobeAnimateOrderedSnapshotFallbackBucket value3 in _immediateSnapshotBuckets.Values)
		{
			value3.ReleaseForOwnerExit(queueNodeCleanup);
		}
		_crowdBuckets.Clear();
		_fallbackBuckets.Clear();
		_immediateSnapshotBuckets.Clear();
		while (_crowdBucketPool.Count > 0)
		{
			_crowdBucketPool.Pop().ReleaseForOwnerExit(queueNodeCleanup: true);
		}
		AdobeAnimateMultiMeshBatcher.ClearSharedCrowdStateBinding(_crowdMaterial);
		_crowdWriter = null;
		_crowdStateArena.Dispose();
		_fallbackStateArena.Dispose();
		_crowdMaterial = null;
		_crowdMaterialResourceSignature = default;
		_crowdMaterialStateTextureRid = default;
		_crowdMaterialStateTextureSize = default;
		_crowdMaterialStateTextureLayers = 0;
		_hasCrowdMaterialResourceSignature = false;
		_animationClockMaterial = null;
		_appliedAnimationClockVersion = -9223372036854775808L;
		foreach (CrowdMeshResource value4 in _crowdMeshesByQuadCapacity.Values)
		{
			value4?.Mesh?.Dispose();
		}
		_crowdMeshesByQuadCapacity.Clear();
		_crowdMesh = null;
		_crowdMeshQuadCapacity = 0;
		_compositeDrawCache.Clear();
		_frameGroupsByZ.Clear();
		_frameGroupsInSubmissionOrder.Clear();
		_crowdCapacityWarmupCountsByZ.Clear();
	}

	private void AccumulateFrameStats(ref AggregateAccumulator aggregate)
	{
		if (_hasCurrentOutput)
		{
			aggregate.ActiveMounts++;
		}
		aggregate.ActiveZBuckets += _framePublishedZBuckets;
		aggregate.CrowdRoots += _framePublishedCrowdRoots;
		aggregate.CrowdStateTexels += _frameStateTexels;
		aggregate.CrowdStateCapacityTexels += _crowdStateArena.CapacityTexels;
		aggregate.ResourceSignatures += ((_framePublishedZBuckets > 0 && _hasFinalResourceSignature) ? 1 : 0);
		aggregate.StateTextureUploads += _frameStateUploads;
		aggregate.StateTextureUpdatedLayers += _frameStateUpdatedLayers;
		aggregate.StateTextureUploadedBytes += _frameStateUploadedBytes;
		aggregate.MultiMeshUploads += _frameMultiMeshUploads;
		aggregate.SignatureConflictBuckets += _frameSignatureConflictBuckets;
		aggregate.FallbackRuns += _frameFallbackRuns;
		aggregate.FallbackRoots += _frameFallbackRoots;
		aggregate.ArenaGrowthCount += _crowdStateArena.GrowthCount;
		aggregate.CrowdMeshQuadCapacity = Math.Max(aggregate.CrowdMeshQuadCapacity, _crowdMeshQuadCapacity);
		aggregate.CrowdMeshVersion = Math.Max(aggregate.CrowdMeshVersion, _crowdMeshVersion);
		aggregate.MeshRebinds += _frameMeshRebinds;
		if (TransactionBackend == AdobeAnimateRenderBackend.CpuPose)
		{
			aggregate.CpuRoots += _framePublishedCrowdRoots + _frameFallbackRoots;
			aggregate.CpuFallbackRoots += _frameFallbackRoots;
		}
	}

	public static int ResolveCanvasLayer(Node source)
	{
		ResolveRenderMount(source, out var _, out var canvasLayer);
		return canvasLayer;
	}

	public static void ResolveRenderMount(Node source, out Node mountParent, out int canvasLayer)
	{
		if (!GodotObject.IsInstanceValid(source) || !source.IsInsideTree())
		{
			mountParent = null;
			canvasLayer = 0;
		}
		else
		{
			ResolveValidatedRenderMount(source, out mountParent, out canvasLayer, out var _);
		}
	}

	internal static void ResolveRuntimeRenderMount(AdobeAnimateSprite source, out Node mountParent, out int canvasLayer, out Viewport viewport)
	{
		if (source == null || !source.IsRuntimeInsideTree)
		{
			mountParent = null;
			canvasLayer = 0;
			viewport = null;
		}
		else
		{
			ResolveValidatedRenderMount(source, out mountParent, out canvasLayer, out viewport);
		}
	}

	private static void ResolveValidatedRenderMount(Node source, out Node mountParent, out int canvasLayer, out Viewport viewport)
	{
		mountParent = null;
		canvasLayer = 0;
		viewport = source.GetViewport();
		if (viewport is SubViewport)
		{
			if (TryFindSubViewportClipAncestor(source, viewport, out var clipAncestor))
			{
				mountParent = clipAncestor;
			}
			else
			{
				mountParent = viewport;
			}
			return;
		}
		CanvasLayer canvasLayer2 = FindCanvasLayerAncestor(source);
		if (GodotObject.IsInstanceValid(canvasLayer2))
		{
			mountParent = canvasLayer2;
			canvasLayer = canvasLayer2.Layer;
			return;
		}
		if (GodotObject.IsInstanceValid(viewport))
		{
			mountParent = viewport;
			return;
		}
		SceneTree tree = source.GetTree();
		if (GodotObject.IsInstanceValid(tree))
		{
			mountParent = tree.Root;
		}
	}

	public static void WarmupRenderMount(Node source)
	{
		ResolveRenderMount(source, out var mountParent, out var _);
		TryGetManagerForMount(mountParent, out var _);
	}

	internal static bool HasPreparedRenderMount(Node source)
	{
		if (!GodotObject.IsInstanceValid(source))
		{
			return false;
		}
		ResolveRenderMount(source, out var mountParent, out var _);
		if (!GodotObject.IsInstanceValid(mountParent))
		{
			return false;
		}
		ulong instanceId = mountParent.GetInstanceId();
		if (Instances.TryGetValue(instanceId, out var value) && GodotObject.IsInstanceValid(value) && value._mountId == instanceId && GodotObject.IsInstanceValid(value._mountParent) && value._mountParent == mountParent)
		{
			return value.GetParent() == mountParent;
		}
		return false;
	}

	internal static bool TryBeginDetachedGpuRenderGraphWarmupBatch()
	{
		if (!GpuRenderGraphEnabled)
		{
			return false;
		}
		if (Global.Instance != null && Global.Instance.adobeAnimateRenderBackend != AdobeAnimateRenderBackend.GpuCrowd)
		{
			return false;
		}
		return GpuGraphAtlas.BeginBatch();
	}

	internal static bool CommitDetachedGpuRenderGraphWarmupBatch()
	{
		return GpuGraphAtlas.CommitBatch();
	}

	internal static void CancelDetachedGpuRenderGraphWarmupBatch()
	{
		GpuGraphAtlas.CancelBatch();
	}

	internal static void WarmupDetachedGpuRenderGraphs(Node source)
	{
		if (!GpuRenderGraphEnabled || !GodotObject.IsInstanceValid(source) || (Global.Instance != null && Global.Instance.adobeAnimateRenderBackend != AdobeAnimateRenderBackend.GpuCrowd))
		{
			return;
		}
		PrepareDetachedGpuSprites(source);
		bool flag = GpuGraphAtlas.BeginBatch();
		bool flag2 = false;
		try
		{
			ResolveDetachedGpuRenderRoots(source);
			flag2 = true;
		}
		finally
		{
			if (flag)
			{
				if (flag2)
				{
					GpuGraphAtlas.CommitBatch();
				}
				else
				{
					GpuGraphAtlas.CancelBatch();
				}
			}
		}
	}

	private static void PrepareDetachedGpuSprites(Node node)
	{
		if (GodotObject.IsInstanceValid(node))
		{
			if (node is AdobeAnimateSprite adobeAnimateSprite)
			{
				adobeAnimateSprite.PrepareDetachedGpuGraphWarmup();
			}
			int childCount = node.GetChildCount(includeInternal: true);
			for (int i = 0; i < childCount; i++)
			{
				PrepareDetachedGpuSprites(node.GetChild(i, includeInternal: true));
			}
		}
	}

	private static void ResolveDetachedGpuRenderRoots(Node node)
	{
		if (GodotObject.IsInstanceValid(node))
		{
			if (node is AdobeAnimateSprite adobeAnimateSprite && !adobeAnimateSprite.IsRenderedByParentSpriteForRender())
			{
				TryResolveGpuRenderGraph(adobeAnimateSprite, out var _, out var _, out var _, out var _, out var _);
			}
			int childCount = node.GetChildCount(includeInternal: true);
			for (int i = 0; i < childCount; i++)
			{
				ResolveDetachedGpuRenderRoots(node.GetChild(i, includeInternal: true));
			}
		}
	}

	private static bool TryGetManager(AdobeAnimateRenderSnapshot snapshot, out AdobeAnimateRenderManager manager)
	{
		Node node = snapshot.RenderMountParent;
		if (!GodotObject.IsInstanceValid(node))
		{
			node = ResolveMountParent(snapshot.Sprite);
		}
		return TryGetManagerForMount(node, out manager);
	}

	private static bool TryGetManagerForMount(Node mountParent, out AdobeAnimateRenderManager manager)
	{
		manager = null;
		if (!GodotObject.IsInstanceValid(mountParent))
		{
			return false;
		}
		ulong instanceId = mountParent.GetInstanceId();
		if (Instances.TryGetValue(instanceId, out manager))
		{
			if (GodotObject.IsInstanceValid(manager) && manager._mountId == instanceId && GodotObject.IsInstanceValid(manager._mountParent) && manager._mountParent == mountParent && manager.GetParent() == mountParent)
			{
				return true;
			}
			Instances.Remove(instanceId);
			RemoveManagerFromStaticLists(manager);
			if (GodotObject.IsInstanceValid(manager))
			{
				manager.HideAllOutput();
				manager.QueueFree();
			}
		}
		manager = new AdobeAnimateRenderManager
		{
			Name = "AdobeAnimateRenderManager",
			ProcessMode = ProcessModeEnum.Always,
			ZAsRelative = false,
			ZIndex = 0,
			_mountId = instanceId,
			_mountParent = mountParent
		};
		mountParent.AddChild(manager, forceReadableName: false, InternalMode.Disabled);
		manager.SetProcess(enable: false);
		Instances[instanceId] = manager;
		return manager.GetParent() == mountParent;
	}

	private static void RemoveManagerFromStaticLists(AdobeAnimateRenderManager manager)
	{
		if (manager != null)
		{
			PreviousActiveManagers.Remove(manager);
			CurrentActiveManagers.Remove(manager);
			TransactionManagers.Remove(manager);
			CrowdCapacityWarmupManagers.Remove(manager);
			DirtyImmediateSubmissionManagers.Remove(manager);
		}
	}

	private static void RunGlobalBucketMaintenance(long lifecycleTick)
	{
		if (LastBucketMaintenanceTick == -9223372036854775808L || lifecycleTick < LastBucketMaintenanceTick)
		{
			LastBucketMaintenanceTick = lifecycleTick;
		}
		else
		{
			if (lifecycleTick - LastBucketMaintenanceTick < 120)
			{
				return;
			}
			LastBucketMaintenanceTick = lifecycleTick;
			foreach (AdobeAnimateRenderManager value in Instances.Values)
			{
				if (GodotObject.IsInstanceValid(value))
				{
					value.ReclaimIdleBuckets(lifecycleTick);
				}
			}
		}
	}

	private void ReclaimIdleBuckets(long lifecycleTick)
	{
		_bucketKeysToRemove.Clear();
		foreach (KeyValuePair<int, AdobeAnimateZIndexCrowdBucket> crowdBucket in _crowdBuckets)
		{
			if (!_previousTouchedCrowdBuckets.Contains(crowdBucket.Value))
			{
				long lastActivityFrame = crowdBucket.Value.LastActivityFrame;
				if (lastActivityFrame != -9223372036854775808L && lifecycleTick >= lastActivityFrame && lifecycleTick - lastActivityFrame >= 120)
				{
					_bucketKeysToRemove.Add(crowdBucket.Key);
				}
			}
		}
		for (int i = 0; i < _bucketKeysToRemove.Count; i++)
		{
			int key = _bucketKeysToRemove[i];
			AdobeAnimateZIndexCrowdBucket adobeAnimateZIndexCrowdBucket = _crowdBuckets[key];
			_crowdBuckets.Remove(key);
			_previousTouchedCrowdBuckets.Remove(adobeAnimateZIndexCrowdBucket);
			_currentTouchedCrowdBuckets.Remove(adobeAnimateZIndexCrowdBucket);
			ClearGroupBucketReferences(adobeAnimateZIndexCrowdBucket, null);
			adobeAnimateZIndexCrowdBucket.Hide();
			adobeAnimateZIndexCrowdBucket.DetachForPool();
			if (_crowdBucketPool.Count < 256)
			{
				_crowdBucketPool.Push(adobeAnimateZIndexCrowdBucket);
			}
			else
			{
				adobeAnimateZIndexCrowdBucket.Dispose();
			}
		}
		ReclaimIdleFallbackBuckets(_fallbackBuckets, lifecycleTick, runtimeBuckets: true);
		ReclaimIdleFallbackBuckets(_immediateSnapshotBuckets, lifecycleTick, runtimeBuckets: false);
		_bucketKeysToRemove.Clear();
	}

	private void ReclaimIdleFallbackBuckets(Dictionary<int, AdobeAnimateOrderedSnapshotFallbackBucket> buckets, long lifecycleTick, bool runtimeBuckets)
	{
		_bucketKeysToRemove.Clear();
		foreach (KeyValuePair<int, AdobeAnimateOrderedSnapshotFallbackBucket> bucket in buckets)
		{
			if ((runtimeBuckets || !bucket.Value.HasImmediateSubmissions) && (!runtimeBuckets || !_previousTouchedFallbackBuckets.Contains(bucket.Value)))
			{
				long lastActivityFrame = bucket.Value.LastActivityFrame;
				if (lastActivityFrame != -9223372036854775808L && lifecycleTick >= lastActivityFrame && lifecycleTick - lastActivityFrame >= 120)
				{
					_bucketKeysToRemove.Add(bucket.Key);
				}
			}
		}
		for (int i = 0; i < _bucketKeysToRemove.Count; i++)
		{
			int key = _bucketKeysToRemove[i];
			AdobeAnimateOrderedSnapshotFallbackBucket adobeAnimateOrderedSnapshotFallbackBucket = buckets[key];
			buckets.Remove(key);
			if (runtimeBuckets)
			{
				_previousTouchedFallbackBuckets.Remove(adobeAnimateOrderedSnapshotFallbackBucket);
				_currentTouchedFallbackBuckets.Remove(adobeAnimateOrderedSnapshotFallbackBucket);
				ClearGroupBucketReferences(null, adobeAnimateOrderedSnapshotFallbackBucket);
			}
			adobeAnimateOrderedSnapshotFallbackBucket.ReleaseForOwnerExit(queueNodeCleanup: true);
		}
	}

	private void ClearGroupBucketReferences(AdobeAnimateZIndexCrowdBucket crowdBucket, AdobeAnimateOrderedSnapshotFallbackBucket fallbackBucket)
	{
		foreach (CrowdFrameGroup value in _frameGroupsByZ.Values)
		{
			if (crowdBucket != null && value.CrowdBucket == crowdBucket)
			{
				value.CrowdBucket = null;
			}
			if (fallbackBucket != null && value.FallbackBucket == fallbackBucket)
			{
				value.FallbackBucket = null;
			}
		}
	}

	private static Node ResolveMountParent(Node source)
	{
		ResolveRenderMount(source, out var mountParent, out var _);
		return mountParent;
	}

	internal static Transform2D ToRenderMountLocalTransform(Node renderMountParent, Transform2D globalTransform)
	{
		if (renderMountParent is CanvasItem canvasItem && GodotObject.IsInstanceValid(canvasItem))
		{
			if (RuntimeTransactionInProgress && CachedRenderMountTransformFrame != -9223372036854775808L)
			{
				if (CachedRenderMountTransformItem != canvasItem)
				{
					CachedRenderMountTransformItem = canvasItem;
					CachedRenderMountInverseTransform = canvasItem.GetGlobalTransform().AffineInverse();
				}
				return CachedRenderMountInverseTransform * globalTransform;
			}
			return canvasItem.GetGlobalTransform().AffineInverse() * globalTransform;
		}
		return globalTransform;
	}

	private static void BeginRenderMountTransformFrame(long frameVersion)
	{
		CachedRenderMountTransformFrame = frameVersion;
		CachedRenderMountTransformItem = null;
		CachedRenderMountInverseTransform = Transform2D.Identity;
	}

	private static void EndRenderMountTransformFrame()
	{
		CachedRenderMountTransformFrame = -9223372036854775808L;
		CachedRenderMountTransformItem = null;
		CachedRenderMountInverseTransform = Transform2D.Identity;
	}

	private static bool TryFindSubViewportClipAncestor(Node source, Viewport viewport, out Node clipAncestor)
	{
		clipAncestor = null;
		Node node = source;
		while (GodotObject.IsInstanceValid(node) && node != viewport)
		{
			if (node is Control { ClipContents: not false } control)
			{
				clipAncestor = control;
				return true;
			}
			node = node.GetParent();
		}
		return false;
	}

	private static CanvasLayer FindCanvasLayerAncestor(Node source)
	{
		Node node = source;
		while (GodotObject.IsInstanceValid(node))
		{
			if (node is SubViewport)
			{
				return null;
			}
			if (node is CanvasLayer result)
			{
				return result;
			}
			node = node.GetParent();
		}
		return null;
	}

	private static long GetRenderFrameVersion()
	{
		ulong processFrames = Engine.GetProcessFrames();
		long num = Engine.GetFramesDrawn();
		return (long)(processFrames << 32) ^ num;
	}

	private static long GetLifecycleTick()
	{
		if (!RuntimeTransactionInProgress)
		{
			return (long)Engine.GetProcessFrames();
		}
		return (long)Math.Min(CurrentRenderPhysicsFrame, 9223372036854775807uL);
	}

	private void BeginRuntimeFrame(long frameVersion)
	{
		if (_runtimeFrameVersion != frameVersion)
		{
			PruneCompositeDrawCacheIfNeeded();
			EnsureCrowdWriter();
			for (int i = 0; i < _frameGroupsInSubmissionOrder.Count; i++)
			{
				_frameGroupsInSubmissionOrder[i].ResetForFrame();
			}
			_frameGroupsInSubmissionOrder.Clear();
			_preparedDrawItems.Clear();
			_preparedGpuOwners.Clear();
			_preparedGpuOverrides.Clear();
			_preparedGpuManagedVisuals.Clear();
			_preparedCompositeOwners.Clear();
			_unifiedDrawItems.Clear();
			_frameTextureValidity.Clear();
			_frameTexture2DValidity.Clear();
			_resourceSignatureBuilder.Reset();
			_currentTouchedCrowdBuckets.Clear();
			_currentTouchedFallbackBuckets.Clear();
			_hasFinalResourceSignature = false;
			_hadCurrentOutputAtFrameBegin = _hasCurrentOutput;
			_hasCurrentOutput = false;
			_frameSignatureConflictBuckets = 0;
			_frameMeshRebinds = 0;
			_framePublishedCrowdRoots = 0;
			_framePublishedZBuckets = 0;
			_frameStateTexels = 0;
			_frameStateUploads = 0;
			_frameStateUpdatedLayers = 0;
			_frameStateUploadedBytes = 0;
			_frameMultiMeshUploads = 0;
			_frameFallbackRuns = 0;
			_frameFallbackRoots = 0;
			_fallbackStateArena.BeginFrame(frameVersion);
			_crowdWriter.BeginSharedCrowdFrame(_crowdStateArena, frameVersion);
			_runtimeFrameVersion = frameVersion;
			_lifecycleTick = GetLifecycleTick();
		}
	}

	private void EnsureCrowdWriter()
	{
		if (!GodotObject.IsInstanceValid(_crowdWriter))
		{
			_crowdWriter = new AdobeAnimateMultiMeshBatcher
			{
				Name = "AdobeAnimateCrowdWriter",
				ProcessMode = ProcessModeEnum.Always,
				EncoderOnly = true
			};
			AddChild(_crowdWriter, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private static void TryAccumulateCrowdCapacityWarmup(AdobeAnimateSprite sprite, long frameVersion)
	{
		if (sprite.TryGetCachedOffscreenCrowdWarmupPlacement(out var renderMountParent, out var effectiveZIndex) && TryGetManagerForMount(renderMountParent, out var manager))
		{
			int preparedGpuRenderGraphQuadCount = sprite.GetPreparedGpuRenderGraphQuadCount(GpuGraphPreparationGeneration);
			manager.AccumulateCrowdCapacityWarmup(frameVersion, effectiveZIndex, preparedGpuRenderGraphQuadCount);
		}
	}

	private static bool TryPrepareCachedOffscreenGpuGraphState(AdobeAnimateSprite root, long frameVersion, float animationClockSeconds)
	{
		if (!root.TryGetCachedOffscreenCrowdWarmupPlacement(out var renderMountParent, out var _))
		{
			return false;
		}
		ulong cachedInstanceIdForRender = root.GetCachedInstanceIdForRender();
		if (!GpuGraphRoots.TryGetValue(cachedInstanceIdForRender, out var value) || value.Root != root || value.OffscreenStatePrepared || value.Graph == null || value.Owners.Length != value.Graph.Owners.Length)
		{
			return false;
		}
		AdobeAnimateGpuRenderGraphDefinition graph = value.Graph;
		for (int i = 0; i < value.Owners.Length; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = value.Owners[i];
			if (adobeAnimateSprite == null)
			{
				return false;
			}
			if (!((i == 0) ? adobeAnimateSprite.TryBuildGpuGraphOwnerStateForRender(renderMountParent, enableGpuClock: true, animationClockSeconds, out var state) : adobeAnimateSprite.TryBuildGpuGraphNestedOwnerStateForRender(renderMountParent, enableGpuClock: true, animationClockSeconds, out state)) || state == null || !AreGpuGraphOwnerDefinitionsEquivalent(state.Definition, graph.Owners[i].Definition))
			{
				return false;
			}
			if (state.HasMediaReplace && !GpuDynamicOverrideAtlas.TryGetOrAdd(frameVersion, state, out var _))
			{
				return false;
			}
		}
		for (int j = 0; j < graph.ManagedVisualBindings.Length; j++)
		{
			AdobeAnimateGpuManagedVisualBinding adobeAnimateGpuManagedVisualBinding = graph.ManagedVisualBindings[j];
			if ((uint)adobeAnimateGpuManagedVisualBinding.OwnerIndex >= (uint)value.Owners.Length)
			{
				return false;
			}
			AdobeAnimateSprite adobeAnimateSprite2 = value.Owners[adobeAnimateGpuManagedVisualBinding.OwnerIndex];
			Rid atlasTextureArrayRid = graph.Owners[adobeAnimateGpuManagedVisualBinding.OwnerIndex].Definition.AtlasTextureArrayRid;
			bool flag;
			AdobeAnimateGpuManagedVisualState state2;
			string failureReason;
			switch (adobeAnimateGpuManagedVisualBinding.SourceKind)
			{
			case AdobeAnimateGpuManagedVisualSourceKind.SlotSprite2D:
				flag = adobeAnimateSprite2.TryBuildManagedSlotGpuStateForRender(adobeAnimateGpuManagedVisualBinding.VisualIndex, atlasTextureArrayRid, out state2, out failureReason);
				break;
			case AdobeAnimateGpuManagedVisualSourceKind.ExternalVisual:
				adobeAnimateSprite2.BeginExternalVisualPreparationForRender(frameVersion);
				flag = adobeAnimateSprite2.TryBuildExternalVisualGpuStateForRender(adobeAnimateGpuManagedVisualBinding.VisualIndex, atlasTextureArrayRid, out state2, out failureReason);
				break;
			default:
				flag = false;
				break;
			}
			if (!flag)
			{
				return false;
			}
		}
		root.GetEffectiveTreeOrderPathForRender();
		value.OffscreenStatePrepared = true;
		return true;
	}

	private static bool HasPreparedOffscreenGpuGraphState(AdobeAnimateSprite root)
	{
		ulong cachedInstanceIdForRender = root.GetCachedInstanceIdForRender();
		if (GpuGraphRoots.TryGetValue(cachedInstanceIdForRender, out var value) && value.Root == root)
		{
			return value.OffscreenStatePrepared;
		}
		return false;
	}

	private void AccumulateCrowdCapacityWarmup(long frameVersion, int effectiveZIndex, int quadCount)
	{
		if (_crowdCapacityWarmupFrame != frameVersion)
		{
			_crowdCapacityWarmupFrame = frameVersion;
			_crowdCapacityWarmupCountsByZ.Clear();
			_crowdCapacityWarmupMaxQuadCount = 1;
			CrowdCapacityWarmupManagers.Add(this);
		}
		_crowdCapacityWarmupCountsByZ.TryGetValue(effectiveZIndex, out var value);
		_crowdCapacityWarmupCountsByZ[effectiveZIndex] = value + 1;
		_crowdCapacityWarmupMaxQuadCount = Math.Max(_crowdCapacityWarmupMaxQuadCount, quadCount);
	}

	private void ApplyCrowdCapacityWarmup(long frameVersion)
	{
		if (_crowdCapacityWarmupFrame != frameVersion || _crowdCapacityWarmupCountsByZ.Count == 0 || !GodotObject.IsInstanceValid(_mountParent))
		{
			return;
		}
		EnsureCrowdWriter();
		_crowdStateArena.WarmupTexture();
		_crowdCapacityWarmupMaxQuadCount = Math.Max(_crowdCapacityWarmupMaxQuadCount, GpuGraphMaxRenderSlotCount);
		long lifecycleTick = GetLifecycleTick();
		foreach (KeyValuePair<int, int> item in _crowdCapacityWarmupCountsByZ)
		{
			GetOrCreateCrowdBucket(item.Key)?.WarmupCapacity(item.Value, lifecycleTick);
		}
		WarmupSharedCrowdResources(_crowdCapacityWarmupMaxQuadCount);
	}

	private bool WarmupSharedCrowdResources(int requiredQuadCapacity)
	{
		requiredQuadCapacity = Math.Max(1, requiredQuadCapacity);
		if (!GodotObject.IsInstanceValid(_crowdMaterial))
		{
			_crowdMaterial = AdobeAnimateMultiMeshBatcher.CreateSharedCrowdMaterial();
		}
		if (!GodotObject.IsInstanceValid(_crowdMaterial))
		{
			return false;
		}
		ApplyCrowdClockIfNeeded();
		CrowdMeshResource resource;
		if (!CrowdMeshCapacityClassesEnabled)
		{
			return TryGetOrCreateCrowdMesh(requiredQuadCapacity, out resource);
		}
		for (int i = 8; i < requiredQuadCapacity; i += 8)
		{
			if (!TryGetOrCreateCrowdMesh(i, out resource))
			{
				return false;
			}
			if (i > 2147483639)
			{
				break;
			}
		}
		return TryGetOrCreateCrowdMesh(requiredQuadCapacity, out resource);
	}

	private CrowdFrameGroup GetOrCreateFrameGroup(int effectiveZIndex)
	{
		if (!_frameGroupsByZ.TryGetValue(effectiveZIndex, out var value))
		{
			value = new CrowdFrameGroup();
			_frameGroupsByZ.Add(effectiveZIndex, value);
		}
		if (value.FrameVersion == _runtimeFrameVersion)
		{
			return value;
		}
		value.ResetForFrame();
		value.FrameVersion = _runtimeFrameVersion;
		value.EffectiveZIndex = effectiveZIndex;
		_frameGroupsInSubmissionOrder.Add(value);
		return value;
	}

	private void EncodeRuntimeFrame(long frameVersion, float animationClockSeconds)
	{
		for (int i = 0; i < _frameGroupsInSubmissionOrder.Count; i++)
		{
			CrowdFrameGroup crowdFrameGroup = _frameGroupsInSubmissionOrder[i];
			TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
			long startBytes = TowerDefenseAllocationTelemetry.Begin();
			if (crowdFrameGroup.UseSnapshotFallback || !TryPrepareCrowdGroup(crowdFrameGroup, frameVersion, animationClockSeconds))
			{
				TryPrepareOrderedFallback(crowdFrameGroup, frameVersion, rebuildResourceSignature: true);
				TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.crowd.encode.prepareFallback", in probe, crowdFrameGroup.Submissions.Count);
				TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodePrepare, startBytes);
				continue;
			}
			TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.crowd.encode.prepareGroup", in probe, crowdFrameGroup.Submissions.Count);
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodePrepare, startBytes);
			TowerDefensePerfProfiler.SpikeProbe probe2 = TowerDefensePerfProfiler.BeginSpikeProbe();
			long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
			if (!TryEncodeCrowdGroup(crowdFrameGroup))
			{
				TryPrepareOrderedFallback(crowdFrameGroup, frameVersion, rebuildResourceSignature: true);
				TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.crowd.encode.writeFallback", in probe2, crowdFrameGroup.PreparedRoots.Count);
				TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeWrite, startBytes2);
			}
			else
			{
				TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.crowd.encode.writeGroup", in probe2, crowdFrameGroup.PreparedRoots.Count);
				TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncodeWrite, startBytes2);
			}
		}
	}

	public override void _Ready()
	{
		SetProcess(enable: false);
		CallDeferred("DisableIdleProcess");
	}

	private void DisableIdleProcess()
	{
		SetProcess(enable: false);
	}

	internal static bool Submit(AdobeAnimateRenderSnapshot snapshot, bool immediateFlush = false)
	{
		bool immediateFlush2 = immediateFlush;
		Global instance = Global.Instance;
		return Submit(snapshot, immediateFlush2, instance != null && instance.adobeAnimateRenderBackend == AdobeAnimateRenderBackend.CpuPose);
	}

	internal static bool SubmitCpuPose(AdobeAnimateRenderSnapshot snapshot, bool immediateFlush = false)
	{
		return Submit(snapshot, immediateFlush, forceCpuPose: true);
	}

	public static void ReleaseImmediateSubmission(AdobeAnimateSprite sprite)
	{
		if (sprite == null)
		{
			return;
		}
		for (int num = DeferredImmediateSubmissions.Count - 1; num >= 0; num--)
		{
			AdobeAnimateSprite sprite2 = DeferredImmediateSubmissions[num].Snapshot.Sprite;
			if (!GodotObject.IsInstanceValid(sprite2) || sprite2 == sprite)
			{
				DeferredImmediateSubmissions.RemoveAt(num);
			}
		}
		if (Instances.Count == 0)
		{
			return;
		}
		foreach (AdobeAnimateRenderManager value in Instances.Values)
		{
			if (!GodotObject.IsInstanceValid(value))
			{
				continue;
			}
			bool flag = false;
			foreach (AdobeAnimateOrderedSnapshotFallbackBucket value2 in value._immediateSnapshotBuckets.Values)
			{
				flag |= value2.RemoveImmediateSubmission(sprite);
			}
			if (flag)
			{
				if (RuntimeTransactionInProgress)
				{
					DirtyImmediateSubmissionManagers.Add(value);
				}
				else
				{
					value.PublishImmediateSnapshotBuckets();
				}
			}
		}
	}

	private static bool Submit(AdobeAnimateRenderSnapshot snapshot, bool immediateFlush, bool forceCpuPose)
	{
		if (!GodotObject.IsInstanceValid(snapshot.Sprite) || !snapshot.Sprite.IsInsideTree() || snapshot.Definition == null)
		{
			return false;
		}
		if (RuntimeTransactionInProgress)
		{
			DeferredImmediateSubmissions.Add(new DeferredImmediateSubmission(snapshot, immediateFlush, forceCpuPose));
			return true;
		}
		if (!TryGetManager(snapshot, out var manager))
		{
			return false;
		}
		return manager.SubmitImmediateSnapshot(in snapshot, GetRenderFrameVersion(), immediateFlush, forceCpuPose);
	}

	public static void RenderActive(IReadOnlyList<AdobeAnimateSprite> sprites, bool activeListAlreadyValidated = false, bool renderRootsAlreadyFiltered = false)
	{
		RenderActiveCore(sprites, activeListAlreadyValidated, renderRootsAlreadyFiltered, renderRootsTreeOrdered: false);
	}

	internal static void InjectRenderTransactionFailureForTests(AdobeAnimateRenderTransactionFaultPhase phase)
	{
		throw new InvalidOperationException("Render transaction fault injection is available only in debug builds.");
	}

	[Conditional("DEBUG")]
	private static void ThrowInjectedRenderTransactionFailure(AdobeAnimateRenderTransactionFaultPhase phase)
	{
	}

	internal static void RenderRuntimeTreeOrdered(IReadOnlyList<AdobeAnimateSprite> sprites)
	{
		RenderActiveCore(sprites, activeListAlreadyValidated: true, renderRootsAlreadyFiltered: true, renderRootsTreeOrdered: true);
	}

	private static void RenderActiveCore(IReadOnlyList<AdobeAnimateSprite> sprites, bool activeListAlreadyValidated, bool renderRootsAlreadyFiltered, bool renderRootsTreeOrdered)
	{
		long renderFrameVersion = GetRenderFrameVersion();
		float currentAnimationClockSeconds = CurrentAnimationClockSeconds;
		CurrentRenderPhysicsFrame = Engine.GetPhysicsFrames();
		CurrentRenderPhysicsTicksPerSecond = Math.Max(1, Engine.PhysicsTicksPerSecond);
		CurrentRenderFrameLimit = Engine.MaxFps;
		RuntimeTransactionInProgress = true;
		bool flag = false;
		bool flag2 = false;
		Exception ex = null;
		try
		{
			flag = true;
			AdobeAnimateSprite.BeginViewportWorldRectRenderFrame(renderFrameVersion);
			flag2 = true;
			BeginRenderMountTransformFrame(renderFrameVersion);
			long startBytes = TowerDefenseAllocationTelemetry.Begin();
			long startTicks = TowerDefensePerfProfiler.Begin();
			BeginFrameTransaction(renderFrameVersion, sprites != null && sprites.Count > 0);
			TowerDefensePerfProfiler.End("adobeAnimate.render.transaction.begin", startTicks, TransactionManagers.Count);
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderBegin, startBytes);
			long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
			long startTicks2 = TowerDefensePerfProfiler.Begin();
			PrepareGpuRenderGraphsBeforeAtlasFreeze(sprites, activeListAlreadyValidated, renderRootsAlreadyFiltered, renderFrameVersion, currentAnimationClockSeconds);
			TowerDefensePerfProfiler.End("adobeAnimate.render.transaction.prepareGraphs", startTicks2, sprites?.Count ?? 0);
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphPrepare, startBytes2);
			CollectFrameTransaction(sprites, activeListAlreadyValidated, renderRootsAlreadyFiltered, renderRootsTreeOrdered, renderFrameVersion, TransactionBackend, currentAnimationClockSeconds);
			EncodeFrameTransaction(renderFrameVersion, currentAnimationClockSeconds);
			bool resourcesFrozen = FreezeFrameTransactionResources(renderFrameVersion);
			long startTicks3 = TowerDefensePerfProfiler.Begin();
			PublishFrameTransaction(renderFrameVersion, resourcesFrozen);
			TowerDefensePerfProfiler.End("adobeAnimate.render.transaction.publish", startTicks3, PreviousActiveManagers.Count);
		}
		catch (Exception ex2)
		{
			ex = ex2;
		}
		finally
		{
			if (flag2)
			{
				try
				{
					EndRenderMountTransformFrame();
				}
				catch (Exception ex3)
				{
					if (ex == null)
					{
						ex = ex3;
					}
				}
			}
			if (flag)
			{
				try
				{
					AdobeAnimateSprite.EndViewportWorldRectRenderFrame();
				}
				catch (Exception ex4)
				{
					if (ex == null)
					{
						ex = ex4;
					}
				}
			}
			RuntimeTransactionInProgress = false;
			try
			{
				long startTicks4 = TowerDefensePerfProfiler.Begin();
				int count = DeferredImmediateSubmissions.Count;
				FlushDeferredImmediateSubmissions();
				TowerDefensePerfProfiler.End("adobeAnimate.render.transaction.flushDeferred", startTicks4, count);
			}
			catch (Exception ex5)
			{
				if (ex == null)
				{
					ex = ex5;
				}
			}
			try
			{
				PublishDirtyImmediateSubmissionManagers();
			}
			catch (Exception ex6)
			{
				if (ex == null)
				{
					ex = ex6;
				}
			}
		}
		if (ex == null)
		{
			CompleteSuccessfulFrameTransaction();
			LastCompletedRuntimeTransactionVersion = renderFrameVersion;
			return;
		}
		AbortFrameTransaction(sprites, renderFrameVersion);
		GD.PushError($"Adobe Animate render transaction aborted safely at frame {renderFrameVersion}: {ex}");
		if (RenderTransactionRecoveryInProgress)
		{
			return;
		}
		RenderTransactionRecoveryInProgress = true;
		try
		{
			RenderActiveCore(sprites, activeListAlreadyValidated, renderRootsAlreadyFiltered, renderRootsTreeOrdered);
		}
		finally
		{
			RenderTransactionRecoveryInProgress = false;
			PublishRenderFailureHandoffs(sprites);
		}
	}

	private static void PublishDirtyImmediateSubmissionManagers()
	{
		if (DirtyImmediateSubmissionManagers.Count == 0)
		{
			return;
		}
		foreach (AdobeAnimateRenderManager dirtyImmediateSubmissionManager in DirtyImmediateSubmissionManagers)
		{
			if (GodotObject.IsInstanceValid(dirtyImmediateSubmissionManager))
			{
				dirtyImmediateSubmissionManager.PublishImmediateSnapshotBuckets();
			}
		}
		DirtyImmediateSubmissionManagers.Clear();
	}

	private static void PublishRenderFailureHandoffs(IReadOnlyList<AdobeAnimateSprite> sprites)
	{
		if (sprites == null)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < sprites.Count; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = sprites[i];
			if (!GodotObject.IsInstanceValid(adobeAnimateSprite) || !adobeAnimateSprite.IsInsideTree() || adobeAnimateSprite.IsRenderedByParentSpriteForRender())
			{
				continue;
			}
			num++;
			try
			{
				if (adobeAnimateSprite.TrySubmitRuntimeRenderFailureHandoff())
				{
					num2++;
				}
			}
			catch (Exception value)
			{
				GD.PushError($"Adobe Animate failure handoff build failed for {adobeAnimateSprite.Name}: {value}");
			}
		}
		bool flag = num > 0 && num2 == num;
		foreach (AdobeAnimateRenderManager value3 in Instances.Values)
		{
			if (!GodotObject.IsInstanceValid(value3))
			{
				continue;
			}
			try
			{
				bool flag2 = value3.PublishImmediateSnapshotBuckets();
				if (flag & flag2)
				{
					value3.HideRuntimeOutputForFailureHandoff();
				}
			}
			catch (Exception value2)
			{
				GD.PushError($"Adobe Animate failure handoff publish failed for {value3.Name}: {value2}");
			}
		}
	}

	private static void CompleteSuccessfulFrameTransaction()
	{
		TransactionManagers.Clear();
		CurrentActiveManagers.Clear();
		CrowdCapacityWarmupManagers.Clear();
	}

	private static void AbortFrameTransaction(IReadOnlyList<AdobeAnimateSprite> sprites, long frameVersion)
	{
		PreviousActiveManagers.Clear();
		for (int i = 0; i < TransactionManagers.Count; i++)
		{
			AdobeAnimateRenderManager adobeAnimateRenderManager = TransactionManagers[i];
			if (!GodotObject.IsInstanceValid(adobeAnimateRenderManager))
			{
				continue;
			}
			try
			{
				adobeAnimateRenderManager.AbortRuntimeFrame(frameVersion);
				if (adobeAnimateRenderManager._hasCurrentOutput)
				{
					PreviousActiveManagers.Add(adobeAnimateRenderManager);
				}
			}
			catch (Exception value)
			{
				GD.PushError($"Adobe Animate render-manager abort failed for {adobeAnimateRenderManager.Name}: {value}");
			}
		}
		GpuDynamicOverrideAtlas.AbortFrame(frameVersion);
		CurrentActiveManagers.Clear();
		TransactionManagers.Clear();
		CrowdCapacityWarmupManagers.Clear();
		AggregateRenderStats = default;
		AdobeAnimateRuntimeManager.RecoverFromFailedRenderTransaction(frameVersion);
		if (sprites != null)
		{
			for (int j = 0; j < sprites.Count; j++)
			{
				RestoreSpriteAfterFailedTransaction(sprites[j], frameVersion);
			}
		}
	}

	private static void RestoreSpriteAfterFailedTransaction(AdobeAnimateSprite sprite, long frameVersion)
	{
		if (!GodotObject.IsInstanceValid(sprite))
		{
			return;
		}
		try
		{
			sprite.RestoreRuntimeNativeCanvasLayer();
			sprite.RestoreExternalVisualNativeFallbacks(frameVersion);
			sprite.RequestRuntimeRenderSubmissionRetry();
			AdobeAnimateRuntimeManager.NotifyRenderSubmission(sprite);
		}
		catch (Exception value)
		{
			GD.PushError($"Adobe Animate sprite recovery failed for {sprite.Name}: {value}");
		}
	}

	internal static ulong GetPhysicsFrameForRender()
	{
		if (!RuntimeTransactionInProgress)
		{
			return Engine.GetPhysicsFrames();
		}
		return CurrentRenderPhysicsFrame;
	}

	internal static int GetPhysicsTicksPerSecondForRender()
	{
		if (!RuntimeTransactionInProgress)
		{
			return Math.Max(1, Engine.PhysicsTicksPerSecond);
		}
		return CurrentRenderPhysicsTicksPerSecond;
	}

	internal static int GetFrameLimitForRender()
	{
		if (!RuntimeTransactionInProgress)
		{
			return Engine.MaxFps;
		}
		return CurrentRenderFrameLimit;
	}

	private static void BeginFrameTransaction(long frameVersion, bool hasInputRoots)
	{
		TransactionBackend = ((Global.Instance != null) ? Global.Instance.adobeAnimateRenderBackend : AdobeAnimateRenderBackend.GpuCrowd);
		PruneInvalidGpuGraphRoots();
		MaintainGpuGraphDefinitions();
		TransactionManagers.Clear();
		CurrentActiveManagers.Clear();
		for (int i = 0; i < PreviousActiveManagers.Count; i++)
		{
			AdobeAnimateRenderManager adobeAnimateRenderManager = PreviousActiveManagers[i];
			if (IsLiveRegisteredManager(adobeAnimateRenderManager))
			{
				adobeAnimateRenderManager.BeginRuntimeFrame(frameVersion);
				adobeAnimateRenderManager._transactionFrameMarker = frameVersion;
				TransactionManagers.Add(adobeAnimateRenderManager);
			}
		}
		if (hasInputRoots)
		{
			FlushPendingGpuGraphInvalidations();
		}
	}

	private static void PrepareGpuRenderGraphsBeforeAtlasFreeze(IReadOnlyList<AdobeAnimateSprite> sprites, bool activeListAlreadyValidated, bool renderRootsAlreadyFiltered, long frameVersion, float animationClockSeconds)
	{
		if (TransactionBackend != AdobeAnimateRenderBackend.GpuCrowd && TransactionBackend != AdobeAnimateRenderBackend.CpuPose)
		{
			return;
		}
		bool flag = false;
		try
		{
			CrowdCapacityWarmupManagers.Clear();
			long startBytes = TowerDefenseAllocationTelemetry.Begin();
			GpuDynamicOverrideAtlas.BeginFrame(frameVersion, (long)Math.Min(CurrentRenderPhysicsFrame, 9223372036854775807uL));
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphBeginFrame, startBytes);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
			if (GpuRenderGraphEnabled && sprites != null)
			{
				for (int i = 0; i < sprites.Count; i++)
				{
					AdobeAnimateSprite adobeAnimateSprite = sprites[i];
					if (adobeAnimateSprite == null || (!activeListAlreadyValidated && (!GodotObject.IsInstanceValid(adobeAnimateSprite) || !adobeAnimateSprite.IsInsideTree())) || (!renderRootsAlreadyFiltered && adobeAnimateSprite.IsRenderedByParentSpriteForRender()))
					{
						continue;
					}
					long startBytes3 = TowerDefenseAllocationTelemetry.Begin();
					bool flag2 = adobeAnimateSprite.CanSkipCrowdRenderFromCachedCulling();
					TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphScanCulling, startBytes3);
					long startBytes4 = TowerDefenseAllocationTelemetry.Begin();
					bool flag3 = IsGpuGraphPreparedForTransaction(adobeAnimateSprite);
					if (!flag2 & flag3)
					{
						TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphScanResolve, startBytes4);
						continue;
					}
					if (!flag3)
					{
						if (flag2 && num >= 4)
						{
							num2++;
							TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphScanResolve, startBytes4);
							continue;
						}
						if (!flag)
						{
							if (!GpuGraphAtlas.BeginBatch())
							{
								return;
							}
							flag = true;
						}
						flag3 = TryResolveGpuRenderGraph(adobeAnimateSprite, out var _, out var _, out var _, out var _, out var _, allowTransactionalAtlasAdd: true);
						if (!flag3)
						{
							TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphScanResolve, startBytes4);
							continue;
						}
						if (flag2)
						{
							num++;
						}
					}
					TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphScanResolve, startBytes4);
					long startBytes5 = TowerDefenseAllocationTelemetry.Begin();
					TryAccumulateCrowdCapacityWarmup(adobeAnimateSprite, frameVersion);
					if ((flag3 & flag2) && !HasPreparedOffscreenGpuGraphState(adobeAnimateSprite) && num3 < 8 && TryPrepareCachedOffscreenGpuGraphState(adobeAnimateSprite, frameVersion, animationClockSeconds))
					{
						num3++;
					}
					TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphScanWarmup, startBytes5);
				}
			}
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphScan, startBytes2);
			if (num > 0)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.offscreenGraphBuilds", num);
			}
			if (num2 > 0)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.offscreenGraphBuildsDeferred", num2);
			}
			if (num3 > 0)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.offscreenStateWarmups", num3);
			}
			long startBytes6 = TowerDefenseAllocationTelemetry.Begin();
			for (int j = 0; j < CrowdCapacityWarmupManagers.Count; j++)
			{
				CrowdCapacityWarmupManagers[j].ApplyCrowdCapacityWarmup(frameVersion);
			}
			CrowdCapacityWarmupManagers.Clear();
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphWarmup, startBytes6);
			long startBytes7 = TowerDefenseAllocationTelemetry.Begin();
			if (!flag && GpuGraphAtlas.AllocationCount < GpuGraphDefinitions.Count)
			{
				if (!GpuGraphAtlas.BeginBatch())
				{
					return;
				}
				flag = true;
			}
			if (!flag)
			{
				GpuGraphAtlasPreparedFrame = frameVersion;
				TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphAtlas, startBytes7);
				return;
			}
			PrepareGpuRenderGraphAtlasForFrame(frameVersion);
			int pendingAllocationCount = GpuGraphAtlas.PendingAllocationCount;
			long startTicks = TowerDefensePerfProfiler.Begin();
			bool flag4 = GpuGraphAtlas.CommitBatch();
			flag = false;
			TowerDefensePerfProfiler.End("adobeAnimate.render.graphAtlas.publish", startTicks, pendingAllocationCount);
			if (pendingAllocationCount > 0)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.graphAtlas.newGraphs", pendingAllocationCount);
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.graphAtlas.pages", GpuGraphAtlas.PageCount);
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.graphAtlas.allocations", GpuGraphAtlas.AllocationCount);
			}
			if (!flag4)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.graphAtlas.publishFailed", 1);
			}
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderGraphAtlas, startBytes7);
		}
		finally
		{
			if (flag)
			{
				GpuGraphAtlas.CancelBatch();
			}
		}
	}

	private static void CollectFrameTransaction(IReadOnlyList<AdobeAnimateSprite> sprites, bool activeListAlreadyValidated, bool renderRootsAlreadyFiltered, bool renderRootsTreeOrdered, long frameVersion, AdobeAnimateRenderBackend targetBackend, float animationClockSeconds)
	{
		TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		long startTicks = TowerDefensePerfProfiler.Begin();
		int num = 0;
		int num2 = 0;
		bool flag = renderRootsTreeOrdered && !AdobeAnimateRuntimeManager.HasInvalidatedRenderRootOrder;
		Node node = null;
		AdobeAnimateRenderManager adobeAnimateRenderManager = null;
		if (sprites != null)
		{
			for (int i = 0; i < sprites.Count; i++)
			{
				AdobeAnimateSprite adobeAnimateSprite = sprites[i];
				if (adobeAnimateSprite == null || (!activeListAlreadyValidated && (!GodotObject.IsInstanceValid(adobeAnimateSprite) || !adobeAnimateSprite.IsInsideTree())) || (!renderRootsAlreadyFiltered && adobeAnimateSprite.IsRenderedByParentSpriteForRender()))
				{
					continue;
				}
				if (adobeAnimateSprite.CanSkipCrowdRenderFromCachedCulling())
				{
					adobeAnimateSprite.RestoreRuntimeNativeCanvasLayer();
					num2++;
					continue;
				}
				AdobeAnimateCrowdRenderState state = null;
				AdobeAnimateRasterCompositeRenderState state2 = default;
				long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
				bool flag2 = targetBackend != AdobeAnimateRenderBackend.CpuPose && adobeAnimateSprite.PrefersCachedGpuGraphCrowdRenderState;
				bool flag3 = targetBackend != AdobeAnimateRenderBackend.CpuPose && !flag2 && adobeAnimateSprite.TryBuildRasterCompositeRenderState(out state2);
				AdobeAnimateCrowdRenderStateResult adobeAnimateCrowdRenderStateResult;
				if (flag3)
				{
					Unsafe.SkipInit<AdobeAnimateCrowdRenderState>(out state);
					adobeAnimateCrowdRenderStateResult = AdobeAnimateCrowdRenderStateResult.Submitted;
				}
				else
				{
					state2 = default;
					adobeAnimateCrowdRenderStateResult = ((targetBackend == AdobeAnimateRenderBackend.CpuPose) ? adobeAnimateSprite.TryBuildCpuPoseCrowdRenderState(animationClockSeconds, out state) : adobeAnimateSprite.TryBuildCrowdRenderState(animationClockSeconds, out state));
				}
				TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectState, startBytes2);
				Unsafe.SkipInit<AdobeAnimateRenderSnapshot>(out var value);
				bool flag4 = false;
				Node renderMountParent;
				int effectiveZIndex;
				if (flag3)
				{
					renderMountParent = state2.RenderMountParent;
					effectiveZIndex = state2.EffectiveZIndex;
				}
				else
				{
					switch (adobeAnimateCrowdRenderStateResult)
					{
					case AdobeAnimateCrowdRenderStateResult.Submitted:
						renderMountParent = state.RenderMountParent;
						effectiveZIndex = state.EffectiveZIndex;
						break;
					case AdobeAnimateCrowdRenderStateResult.NotSupported:
					{
						long startBytes3 = TowerDefenseAllocationTelemetry.Begin();
						if (!adobeAnimateSprite.TryBuildRenderSnapshot(out value))
						{
							TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectFallback, startBytes3);
							adobeAnimateSprite.RestoreRuntimeNativeCanvasLayer();
							continue;
						}
						TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectFallback, startBytes3);
						renderMountParent = value.RenderMountParent;
						effectiveZIndex = value.EffectiveZIndex;
						flag4 = true;
						break;
					}
					default:
						adobeAnimateSprite.RestoreRuntimeNativeCanvasLayer();
						continue;
					}
				}
				AdobeAnimateRenderManager manager;
				if (adobeAnimateRenderManager != null && renderMountParent == node)
				{
					manager = adobeAnimateRenderManager;
				}
				else
				{
					if (!TryGetManagerForMount(renderMountParent, out manager))
					{
						adobeAnimateSprite.RestoreRuntimeNativeCanvasLayer();
						continue;
					}
					node = renderMountParent;
					adobeAnimateRenderManager = manager;
				}
				manager.BeginRuntimeFrame(frameVersion);
				long startBytes4 = TowerDefenseAllocationTelemetry.Begin();
				AddTransactionManager(manager, frameVersion);
				CrowdFrameGroup orCreateFrameGroup = manager.GetOrCreateFrameGroup(effectiveZIndex);
				orCreateFrameGroup.ForceCpuPose = targetBackend == AdobeAnimateRenderBackend.CpuPose;
				orCreateFrameGroup.SubmissionsTreeOrdered &= flag;
				int stateIndex = -1;
				int rasterStateIndex = -1;
				int fallbackSnapshotIndex = -1;
				if (flag3)
				{
					rasterStateIndex = orCreateFrameGroup.RasterStates.Count;
					orCreateFrameGroup.RasterStates.Add(state2);
				}
				else if (adobeAnimateCrowdRenderStateResult == AdobeAnimateCrowdRenderStateResult.Submitted)
				{
					stateIndex = orCreateFrameGroup.States.Count;
					orCreateFrameGroup.States.Add(state);
				}
				if (flag4)
				{
					fallbackSnapshotIndex = orCreateFrameGroup.FallbackSnapshots.Count;
					orCreateFrameGroup.FallbackSnapshots.Add(value);
				}
				bool needsRuntimeRenderSubmission = adobeAnimateSprite.NeedsRuntimeRenderSubmission;
				bool forceGpuPrepare = adobeAnimateSprite.ConsumeRuntimeGpuPrepareRequiredForRender();
				orCreateFrameGroup.Submissions.Add(new CrowdSubmission(adobeAnimateSprite, adobeAnimateCrowdRenderStateResult, stateIndex, rasterStateIndex, fallbackSnapshotIndex, flag ? Array.Empty<int>() : adobeAnimateSprite.GetEffectiveTreeOrderPathForRender(), adobeAnimateSprite.GetCachedInstanceIdForRender(), needsRuntimeRenderSubmission, forceGpuPrepare));
				if (adobeAnimateCrowdRenderStateResult != AdobeAnimateCrowdRenderStateResult.Submitted)
				{
					orCreateFrameGroup.UseSnapshotFallback = true;
				}
				TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollectSubmission, startBytes4);
				num++;
			}
		}
		if (num2 > 0)
		{
			TowerDefensePerfProfiler.Sample("adobeAnimate.render.crowdFast.cachedCulled", num2);
		}
		TowerDefensePerfProfiler.End("adobeAnimate.render.crowd.collect", startTicks, num);
		TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.crowd.collect", in probe, num);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderCollect, startBytes);
	}

	private static void EncodeFrameTransaction(long frameVersion, float animationClockSeconds)
	{
		TowerDefensePerfProfiler.SpikeProbe probe = TowerDefensePerfProfiler.BeginSpikeProbe();
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		long startTicks = TowerDefensePerfProfiler.Begin();
		int num = 0;
		for (int i = 0; i < CurrentActiveManagers.Count; i++)
		{
			AdobeAnimateRenderManager adobeAnimateRenderManager = CurrentActiveManagers[i];
			TowerDefensePerfProfiler.SpikeProbe probe2 = TowerDefensePerfProfiler.BeginSpikeProbe();
			adobeAnimateRenderManager.EncodeRuntimeFrame(frameVersion, animationClockSeconds);
			TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.crowd.encode.manager", in probe2, adobeAnimateRenderManager._frameGroupsInSubmissionOrder.Count);
			num += adobeAnimateRenderManager._frameGroupsInSubmissionOrder.Count;
		}
		TowerDefensePerfProfiler.End("adobeAnimate.render.crowd.encode", startTicks, num);
		TowerDefensePerfProfiler.EndSpikeProbe("adobeAnimate.render.crowd.encode", in probe, num);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderEncode, startBytes);
	}

	private static bool FreezeFrameTransactionResources(long frameVersion)
	{
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		long startTicks = TowerDefensePerfProfiler.Begin();
		bool flag = GpuDynamicOverrideAtlas.FrameVersion != frameVersion || GpuDynamicOverrideAtlas.Freeze(frameVersion);
		for (int i = 0; i < CurrentActiveManagers.Count; i++)
		{
			CurrentActiveManagers[i].FinalizeFrameResources(frameVersion, flag);
		}
		TowerDefensePerfProfiler.End("adobeAnimate.render.crowd.freeze", startTicks, CurrentActiveManagers.Count);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderFreeze, startBytes);
		return flag;
	}

	private static void PublishFrameTransaction(long frameVersion, bool resourcesFrozen)
	{
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		AggregateAccumulator aggregate = default;
		PublishGpuTransaction(frameVersion, resourcesFrozen);
		PreviousActiveManagers.Clear();
		CompleteFrameTransaction(ref aggregate);
		AdobeAnimateMultiMeshRdUploadDispatcher.StatisticsSnapshot statistics = AdobeAnimateMultiMeshRdUploadDispatcher.Shared.Statistics;
		aggregate.RdSubmittedBatches = statistics.SubmittedBatchCount;
		aggregate.RdAppliedBatches = statistics.AppliedBatchCount;
		aggregate.RdQueuedUploads = statistics.QueuedUploadCount;
		aggregate.RdAppliedUploads = statistics.AppliedUploadCount;
		aggregate.RdUploadedBytes = statistics.UploadedByteCount;
		aggregate.RdBufferRidRefreshes = statistics.BufferRidRefreshCount;
		aggregate.RdFailedBatches = statistics.FailedBatchCount;
		aggregate.RdBufferUpdateFailures = statistics.BufferUpdateFailureCount;
		aggregate.RdInvalidTargets = statistics.InvalidTargetCount;
		aggregate.RdStaleDrops = statistics.StaleGenerationDropCount;
		aggregate.RdQueueDepth = statistics.CurrentQueueDepth;
		aggregate.RdMaximumQueueDepth = statistics.MaximumQueueDepth;
		aggregate.RdLastQueuedFrameVersion = statistics.LastQueuedFrameVersion;
		aggregate.RdLastAppliedFrameVersion = statistics.LastAppliedFrameVersion;
		AggregateRenderStats = aggregate.ToStats();
		if (TowerDefensePerfProfiler.Enabled)
		{
			TowerDefensePerfProfiler.Sample("adobeAnimate.render.cpuRoots", AggregateRenderStats.CpuRoots);
			TowerDefensePerfProfiler.Sample("adobeAnimate.render.cpuFallbackRoots", AggregateRenderStats.CpuFallbackRoots);
			TowerDefensePerfProfiler.Sample("adobeAnimate.render.cpuValidationFailures", AggregateRenderStats.CpuValidationFailures);
			TowerDefensePerfProfiler.Sample("adobeAnimate.render.cpuMeshRebuilds", AggregateRenderStats.CpuMeshRebuilds);
			TowerDefensePerfProfiler.Sample("adobeAnimate.render.cpuVertexUploads", AggregateRenderStats.CpuVertexUploads);
			TowerDefensePerfProfiler.Sample("adobeAnimate.render.cpuUploadedVertices", AggregateRenderStats.CpuUploadedVertices);
			TowerDefensePerfProfiler.Sample("adobeAnimate.render.cpuCapacityGrowths", AggregateRenderStats.CpuCapacityGrowths);
			TowerDefensePerfProfiler.Sample("adobeAnimate.render.cpuSkippedUploads", AggregateRenderStats.CpuSkippedUploads);
			TowerDefensePerfProfiler.Sample("adobeAnimate.render.rdQueueDepth", (int)Math.Min(2147483647L, AggregateRenderStats.RdQueueDepth));
			TowerDefensePerfProfiler.Sample("adobeAnimate.render.rdFailedBatches", (int)Math.Min(2147483647L, AggregateRenderStats.RdFailedBatches));
		}
		long startTicks = TowerDefensePerfProfiler.Begin();
		RunGlobalBucketMaintenance(GetLifecycleTick());
		TowerDefensePerfProfiler.End("adobeAnimate.render.transaction.maintenance", startTicks, Instances.Count);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderPublish, startBytes);
	}

	private static void PublishGpuTransaction(long frameVersion, bool resourcesFrozen)
	{
		using AdobeAnimateMultiMeshRdUploadDispatcher.Batch batch = AdobeAnimateMultiMeshRdUploadDispatcher.Shared.BeginBatch(frameVersion);
		for (int i = 0; i < TransactionManagers.Count; i++)
		{
			AdobeAnimateRenderManager adobeAnimateRenderManager = TransactionManagers[i];
			if (IsLiveRegisteredManager(adobeAnimateRenderManager))
			{
				adobeAnimateRenderManager.PublishRuntimeFrame(frameVersion, resourcesFrozen, batch);
			}
		}
		if (!batch.EndBatch())
		{
			throw new InvalidOperationException($"Adobe Animate RD MultiMesh 批次提交失败，帧版本: {frameVersion}");
		}
	}

	private static void CompleteFrameTransaction(ref AggregateAccumulator aggregate)
	{
		for (int i = 0; i < TransactionManagers.Count; i++)
		{
			AdobeAnimateRenderManager adobeAnimateRenderManager = TransactionManagers[i];
			if (IsLiveRegisteredManager(adobeAnimateRenderManager))
			{
				adobeAnimateRenderManager.AccumulateFrameStats(ref aggregate);
				if (adobeAnimateRenderManager._hasCurrentOutput)
				{
					PreviousActiveManagers.Add(adobeAnimateRenderManager);
				}
			}
		}
	}

	internal static AdobeAnimateCrowdAggregateStats GetAggregateRenderStats()
	{
		return AggregateRenderStats;
	}

	internal static void SetAnimationClock(double seconds)
	{
		CurrentAnimationClockSeconds = (double.IsFinite(seconds) ? ((float)Math.Max(0.0, seconds)) : 0f);
		CurrentPhysicsInterpolationFraction = Mathf.Clamp((float)Engine.GetPhysicsInterpolationFraction(), 0f, 1f);
		long num = CurrentAnimationClockVersion + 1;
		CurrentAnimationClockVersion = ((num > 0) ? num : 1);
		long startTicks = TowerDefensePerfProfiler.Begin();
		for (int i = 0; i < PreviousActiveManagers.Count; i++)
		{
			PreviousActiveManagers[i]?.ApplyCrowdClockIfNeeded();
		}
		TowerDefensePerfProfiler.End("adobeAnimate.render.clockDispatch", startTicks, PreviousActiveManagers.Count);
	}

	private void ApplyCrowdClockIfNeeded()
	{
		ShaderMaterial crowdMaterial = _crowdMaterial;
		if (_animationClockMaterial != crowdMaterial || _appliedAnimationClockVersion != CurrentAnimationClockVersion)
		{
			long startTicks = TowerDefensePerfProfiler.Begin();
			int num = 0;
			if (GodotObject.IsInstanceValid(crowdMaterial))
			{
				AdobeAnimateMultiMeshBatcher.SetSharedCrowdAnimationClock(crowdMaterial, CurrentAnimationClockSeconds);
				AdobeAnimateMultiMeshBatcher.SetSharedCrowdPhysicsInterpolationFraction(crowdMaterial, CurrentPhysicsInterpolationFraction);
				num++;
			}
			for (int i = 0; i < _previousTouchedFallbackBuckets.Count; i++)
			{
				_previousTouchedFallbackBuckets[i].ApplyAnimationClock(CurrentAnimationClockSeconds, CurrentPhysicsInterpolationFraction);
				num += _previousTouchedFallbackBuckets[i].ActiveRunCount;
			}
			_animationClockMaterial = crowdMaterial;
			_appliedAnimationClockVersion = CurrentAnimationClockVersion;
			TowerDefensePerfProfiler.End("adobeAnimate.render.clockUpload", startTicks, num);
		}
	}

	private static void AddTransactionManager(AdobeAnimateRenderManager manager, long frameVersion)
	{
		if (manager._transactionFrameMarker != frameVersion)
		{
			manager._transactionFrameMarker = frameVersion;
			TransactionManagers.Add(manager);
		}
		if (manager._currentFrameMarker != frameVersion)
		{
			manager._currentFrameMarker = frameVersion;
			CurrentActiveManagers.Add(manager);
		}
	}

	private static bool IsLiveRegisteredManager(AdobeAnimateRenderManager manager)
	{
		if (GodotObject.IsInstanceValid(manager) && manager._mountId != 0L && GodotObject.IsInstanceValid(manager._mountParent) && manager.GetParent() == manager._mountParent && Instances.TryGetValue(manager._mountId, out var value))
		{
			return value == manager;
		}
		return false;
	}

	private static void FlushDeferredImmediateSubmissions()
	{
		for (int i = 0; i < DeferredImmediateSubmissions.Count; i++)
		{
			DeferredImmediateSubmission deferredImmediateSubmission = DeferredImmediateSubmissions[i];
			AdobeAnimateSprite sprite = deferredImmediateSubmission.Snapshot.Sprite;
			if (GodotObject.IsInstanceValid(sprite) && sprite.IsInsideTree() && TryGetManager(deferredImmediateSubmission.Snapshot, out var manager))
			{
				manager.SubmitImmediateSnapshot(deferredImmediateSubmission.Snapshot, GetRenderFrameVersion(), deferredImmediateSubmission.ImmediateFlush, deferredImmediateSubmission.ForceCpuPose);
			}
		}
		DeferredImmediateSubmissions.Clear();
	}

	public override void _ExitTree()
	{
		if (_mountId != 0L && Instances.TryGetValue(_mountId, out var value) && value == this)
		{
			Instances.Remove(_mountId);
		}
		RemoveManagerFromStaticLists(this);
		HideAndDisposeOwnedResources();
		_mountParent = null;
		_mountId = 0uL;
		if (Instances.Count == 0)
		{
			ClearGpuRenderGraphCaches(notifyRuntime: false);
		}
	}

	internal static int CompareTreeOrderForRender(int[] leftPath, ulong leftStableOrder, int[] rightPath, ulong rightStableOrder)
	{
		int val = leftPath?.Length ?? 0;
		int num = rightPath?.Length ?? 0;
		int num2 = Math.Min(val, num);
		for (int i = 0; i < num2; i++)
		{
			int num3 = leftPath[i].CompareTo(rightPath[i]);
			if (num3 != 0)
			{
				return num3;
			}
		}
		int num4 = val.CompareTo(num);
		if (num4 == 0)
		{
			return leftStableOrder.CompareTo(rightStableOrder);
		}
		return num4;
	}

	private static bool AreCrowdSubmissionsSorted(List<CrowdSubmission> submissions)
	{
		Span<CrowdSubmission> span = CollectionsMarshal.AsSpan(submissions);
		for (int i = 1; i < span.Length; i++)
		{
			if (CrowdSubmissionTreeOrderComparer.Instance.Compare(span[i - 1], span[i]) > 0)
			{
				return false;
			}
		}
		return true;
	}

	internal static bool TryCopyLastCrowdSubmissionOrderForTests(AdobeAnimateSprite mountProbe, int effectiveZIndex, Span<AdobeAnimateSprite> destination, out int totalCount, out bool crowdEncoded, out long frameVersion)
	{
		totalCount = 0;
		crowdEncoded = false;
		frameVersion = -9223372036854775808L;
		ResolveRenderMount(mountProbe, out var mountParent, out var _);
		if (!GodotObject.IsInstanceValid(mountParent) || !Instances.TryGetValue(mountParent.GetInstanceId(), out var value) || !value._frameGroupsByZ.TryGetValue(effectiveZIndex, out var value2))
		{
			return false;
		}
		totalCount = value2.Submissions.Count;
		crowdEncoded = value2.CrowdEncoded;
		frameVersion = value2.FrameVersion;
		int num = Math.Min(totalCount, destination.Length);
		for (int i = 0; i < num; i++)
		{
			destination[i] = value2.Submissions[i].Sprite;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(83)
		{
			new MethodInfo(MethodName.IsValidTextureForFrame, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "size", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsValidTextureForFrame, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false)
			}, null),
			new MethodInfo(MethodName.RollbackPreparedScratch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "drawItemMark", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gpuOwnerMark", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gpuOverrideMark", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "gpuManagedVisualMark", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "compositeOwnerMark", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildResourceBuilderFromEncodedGroups, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinalizeFrameResources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "globalFreezeSucceeded", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountEncodedCrowdGroups, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConvertAllCrowdGroupsToFallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PruneCompositeDrawCacheIfNeeded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsSameTextureArray, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "left", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false),
				new PropertyInfo(Variant.Type.Object, "right", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TextureLayered"), exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildPublishedCrowdMaterials, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetPublishedDynamicOverrideBindingCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RebuildPublishedCrowdMaterial, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HaveSameGpuGraphOwners, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "left", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "right", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareGpuRenderGraphAtlasForFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "renderFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsGpuGraphPreparedForTransaction, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsGpuGraphReady, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsGpuGraphStateCurrent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "signature", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InvalidateGpuRenderGraph, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlushPendingGpuGraphInvalidations, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.PruneInvalidGpuGraphRoots, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.MaintainGpuGraphDefinitions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ClearGpuRenderGraphCaches, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "notifyRuntime", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AbortRuntimeFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareSharedCrowdResources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCrowdMaterialMatchesFinalSignature, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindCurrentCrowdBuckets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sharedRequiredQuadCapacity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCrowdMeshCapacityClass, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "requiredQuadCapacity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CapacityForCrowdMeshSlots, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "requiredSlots", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PublishImmediateSnapshotBuckets, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideRuntimeOutputForFailureHandoff, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideAllOutput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HideAndDisposeOwnedResources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveCanvasLayer, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.WarmupRenderMount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasPreparedRenderMount, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.TryBeginDetachedGpuRenderGraphWarmupBatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CommitDetachedGpuRenderGraphWarmupBatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CancelDetachedGpuRenderGraphWarmupBatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.WarmupDetachedGpuRenderGraphs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareDetachedGpuSprites, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveDetachedGpuRenderRoots, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveManagerFromStaticLists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RunGlobalBucketMaintenance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "lifecycleTick", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReclaimIdleBuckets, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "lifecycleTick", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveMountParent, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ToRenderMountLocalTransform, new PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "renderMountParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Transform2D, "globalTransform", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginRenderMountTransformFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EndRenderMountTransformFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.FindCanvasLayerAncestor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("CanvasLayer"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetRenderFrameVersion, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetLifecycleTick, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.BeginRuntimeFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCrowdWriter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryAccumulateCrowdCapacityWarmup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryPrepareCachedOffscreenGpuGraphState, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "animationClockSeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasPreparedOffscreenGpuGraphState, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.AccumulateCrowdCapacityWarmup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "effectiveZIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "quadCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCrowdCapacityWarmup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WarmupSharedCrowdResources, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "requiredQuadCapacity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EncodeRuntimeFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "animationClockSeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisableIdleProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseImmediateSubmission, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.InjectRenderTransactionFailureForTests, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ThrowInjectedRenderTransactionFailure, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "phase", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PublishDirtyImmediateSubmissionManagers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CompleteSuccessfulFrameTransaction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RestoreSpriteAfterFailedTransaction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPhysicsFrameForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetPhysicsTicksPerSecondForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetFrameLimitForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.BeginFrameTransaction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "hasInputRoots", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EncodeFrameTransaction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "animationClockSeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FreezeFrameTransactionResources, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PublishFrameTransaction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "resourcesFrozen", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PublishGpuTransaction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "resourcesFrozen", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAnimationClock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "seconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCrowdClockIfNeeded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddTransactionManager, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsLiveRegisteredManager, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "manager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FlushDeferredImmediateSubmissions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompareTreeOrderForRender, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedInt32Array, "leftPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "leftStableOrder", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.PackedInt32Array, "rightPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rightStableOrder", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsValidTextureForFrame && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidTextureForFrame(VariantUtils.ConvertTo<TextureLayered>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.IsValidTextureForFrame && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsValidTextureForFrame(VariantUtils.ConvertTo<TextureLayered>(in args[0])));
			return true;
		}
		if (method == MethodName.RollbackPreparedScratch && args.Count == 5)
		{
			RollbackPreparedScratch(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildResourceBuilderFromEncodedGroups && args.Count == 1)
		{
			RebuildResourceBuilderFromEncodedGroups(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinalizeFrameResources && args.Count == 2)
		{
			FinalizeFrameResources(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountEncodedCrowdGroups && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountEncodedCrowdGroups());
			return true;
		}
		if (method == MethodName.ConvertAllCrowdGroupsToFallback && args.Count == 1)
		{
			ConvertAllCrowdGroupsToFallback(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PruneCompositeDrawCacheIfNeeded && args.Count == 0)
		{
			PruneCompositeDrawCacheIfNeeded();
			ret = default;
			return true;
		}
		if (method == MethodName.IsSameTextureArray && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSameTextureArray(VariantUtils.ConvertTo<TextureLayered>(in args[0]), VariantUtils.ConvertTo<TextureLayered>(in args[1])));
			return true;
		}
		if (method == MethodName.RebuildPublishedCrowdMaterials && args.Count == 0)
		{
			RebuildPublishedCrowdMaterials();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPublishedDynamicOverrideBindingCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPublishedDynamicOverrideBindingCount());
			return true;
		}
		if (method == MethodName.RebuildPublishedCrowdMaterial && args.Count == 0)
		{
			RebuildPublishedCrowdMaterial();
			ret = default;
			return true;
		}
		if (method == MethodName.HaveSameGpuGraphOwners && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HaveSameGpuGraphOwners(VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[1])));
			return true;
		}
		if (method == MethodName.PrepareGpuRenderGraphAtlasForFrame && args.Count == 1)
		{
			PrepareGpuRenderGraphAtlasForFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsGpuGraphPreparedForTransaction && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsGpuGraphPreparedForTransaction(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.IsGpuGraphReady && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsGpuGraphReady(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.IsGpuGraphStateCurrent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsGpuGraphStateCurrent(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1])));
			return true;
		}
		if (method == MethodName.InvalidateGpuRenderGraph && args.Count == 2)
		{
			InvalidateGpuRenderGraph(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateGpuGraphInvalidationReason>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlushPendingGpuGraphInvalidations && args.Count == 0)
		{
			FlushPendingGpuGraphInvalidations();
			ret = default;
			return true;
		}
		if (method == MethodName.PruneInvalidGpuGraphRoots && args.Count == 0)
		{
			PruneInvalidGpuGraphRoots();
			ret = default;
			return true;
		}
		if (method == MethodName.MaintainGpuGraphDefinitions && args.Count == 0)
		{
			MaintainGpuGraphDefinitions();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearGpuRenderGraphCaches && args.Count == 1)
		{
			ClearGpuRenderGraphCaches(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AbortRuntimeFrame && args.Count == 1)
		{
			AbortRuntimeFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareSharedCrowdResources && args.Count == 1)
		{
			PrepareSharedCrowdResources(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCrowdMaterialMatchesFinalSignature && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureCrowdMaterialMatchesFinalSignature());
			return true;
		}
		if (method == MethodName.BindCurrentCrowdBuckets && args.Count == 2)
		{
			BindCurrentCrowdBuckets(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCrowdMeshCapacityClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetCrowdMeshCapacityClass(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CapacityForCrowdMeshSlots && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CapacityForCrowdMeshSlots(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.PublishImmediateSnapshotBuckets && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(PublishImmediateSnapshotBuckets());
			return true;
		}
		if (method == MethodName.HideRuntimeOutputForFailureHandoff && args.Count == 0)
		{
			HideRuntimeOutputForFailureHandoff();
			ret = default;
			return true;
		}
		if (method == MethodName.HideAllOutput && args.Count == 0)
		{
			HideAllOutput();
			ret = default;
			return true;
		}
		if (method == MethodName.HideAndDisposeOwnedResources && args.Count == 0)
		{
			HideAndDisposeOwnedResources();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveCanvasLayer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveCanvasLayer(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.WarmupRenderMount && args.Count == 1)
		{
			WarmupRenderMount(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasPreparedRenderMount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPreparedRenderMount(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.TryBeginDetachedGpuRenderGraphWarmupBatch && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryBeginDetachedGpuRenderGraphWarmupBatch());
			return true;
		}
		if (method == MethodName.CommitDetachedGpuRenderGraphWarmupBatch && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CommitDetachedGpuRenderGraphWarmupBatch());
			return true;
		}
		if (method == MethodName.CancelDetachedGpuRenderGraphWarmupBatch && args.Count == 0)
		{
			CancelDetachedGpuRenderGraphWarmupBatch();
			ret = default;
			return true;
		}
		if (method == MethodName.WarmupDetachedGpuRenderGraphs && args.Count == 1)
		{
			WarmupDetachedGpuRenderGraphs(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareDetachedGpuSprites && args.Count == 1)
		{
			PrepareDetachedGpuSprites(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveDetachedGpuRenderRoots && args.Count == 1)
		{
			ResolveDetachedGpuRenderRoots(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveManagerFromStaticLists && args.Count == 1)
		{
			RemoveManagerFromStaticLists(VariantUtils.ConvertTo<AdobeAnimateRenderManager>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunGlobalBucketMaintenance && args.Count == 1)
		{
			RunGlobalBucketMaintenance(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReclaimIdleBuckets && args.Count == 1)
		{
			ReclaimIdleBuckets(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveMountParent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(ResolveMountParent(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ToRenderMountLocalTransform && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ToRenderMountLocalTransform(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1])));
			return true;
		}
		if (method == MethodName.BeginRenderMountTransformFrame && args.Count == 1)
		{
			BeginRenderMountTransformFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EndRenderMountTransformFrame && args.Count == 0)
		{
			EndRenderMountTransformFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.FindCanvasLayerAncestor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CanvasLayer>(FindCanvasLayerAncestor(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetRenderFrameVersion && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetRenderFrameVersion());
			return true;
		}
		if (method == MethodName.GetLifecycleTick && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetLifecycleTick());
			return true;
		}
		if (method == MethodName.BeginRuntimeFrame && args.Count == 1)
		{
			BeginRuntimeFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCrowdWriter && args.Count == 0)
		{
			EnsureCrowdWriter();
			ret = default;
			return true;
		}
		if (method == MethodName.TryAccumulateCrowdCapacityWarmup && args.Count == 2)
		{
			TryAccumulateCrowdCapacityWarmup(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryPrepareCachedOffscreenGpuGraphState && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(TryPrepareCachedOffscreenGpuGraphState(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.HasPreparedOffscreenGpuGraphState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPreparedOffscreenGpuGraphState(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.AccumulateCrowdCapacityWarmup && args.Count == 3)
		{
			AccumulateCrowdCapacityWarmup(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCrowdCapacityWarmup && args.Count == 1)
		{
			ApplyCrowdCapacityWarmup(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WarmupSharedCrowdResources && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(WarmupSharedCrowdResources(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.EncodeRuntimeFrame && args.Count == 2)
		{
			EncodeRuntimeFrame(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName.DisableIdleProcess && args.Count == 0)
		{
			DisableIdleProcess();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseImmediateSubmission && args.Count == 1)
		{
			ReleaseImmediateSubmission(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InjectRenderTransactionFailureForTests && args.Count == 1)
		{
			InjectRenderTransactionFailureForTests(VariantUtils.ConvertTo<AdobeAnimateRenderTransactionFaultPhase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ThrowInjectedRenderTransactionFailure && args.Count == 1)
		{
			ret = default;
			return true;
		}
		if (method == MethodName.PublishDirtyImmediateSubmissionManagers && args.Count == 0)
		{
			PublishDirtyImmediateSubmissionManagers();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteSuccessfulFrameTransaction && args.Count == 0)
		{
			CompleteSuccessfulFrameTransaction();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreSpriteAfterFailedTransaction && args.Count == 2)
		{
			RestoreSpriteAfterFailedTransaction(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPhysicsFrameForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetPhysicsFrameForRender());
			return true;
		}
		if (method == MethodName.GetPhysicsTicksPerSecondForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPhysicsTicksPerSecondForRender());
			return true;
		}
		if (method == MethodName.GetFrameLimitForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetFrameLimitForRender());
			return true;
		}
		if (method == MethodName.BeginFrameTransaction && args.Count == 2)
		{
			BeginFrameTransaction(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EncodeFrameTransaction && args.Count == 2)
		{
			EncodeFrameTransaction(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FreezeFrameTransactionResources && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(FreezeFrameTransactionResources(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.PublishFrameTransaction && args.Count == 2)
		{
			PublishFrameTransaction(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PublishGpuTransaction && args.Count == 2)
		{
			PublishGpuTransaction(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAnimationClock && args.Count == 1)
		{
			SetAnimationClock(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCrowdClockIfNeeded && args.Count == 0)
		{
			ApplyCrowdClockIfNeeded();
			ret = default;
			return true;
		}
		if (method == MethodName.AddTransactionManager && args.Count == 2)
		{
			AddTransactionManager(VariantUtils.ConvertTo<AdobeAnimateRenderManager>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsLiveRegisteredManager && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLiveRegisteredManager(VariantUtils.ConvertTo<AdobeAnimateRenderManager>(in args[0])));
			return true;
		}
		if (method == MethodName.FlushDeferredImmediateSubmissions && args.Count == 0)
		{
			FlushDeferredImmediateSubmissions();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.CompareTreeOrderForRender && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<int>(CompareTreeOrderForRender(VariantUtils.ConvertTo<int[]>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]), VariantUtils.ConvertTo<int[]>(in args[2]), VariantUtils.ConvertTo<ulong>(in args[3])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.IsSameTextureArray && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSameTextureArray(VariantUtils.ConvertTo<TextureLayered>(in args[0]), VariantUtils.ConvertTo<TextureLayered>(in args[1])));
			return true;
		}
		if (method == MethodName.RebuildPublishedCrowdMaterials && args.Count == 0)
		{
			RebuildPublishedCrowdMaterials();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPublishedDynamicOverrideBindingCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPublishedDynamicOverrideBindingCount());
			return true;
		}
		if (method == MethodName.HaveSameGpuGraphOwners && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HaveSameGpuGraphOwners(VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertToSystemArrayOfGodotObject<AdobeAnimateSprite>(in args[1])));
			return true;
		}
		if (method == MethodName.PrepareGpuRenderGraphAtlasForFrame && args.Count == 1)
		{
			PrepareGpuRenderGraphAtlasForFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsGpuGraphPreparedForTransaction && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsGpuGraphPreparedForTransaction(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.IsGpuGraphReady && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsGpuGraphReady(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.IsGpuGraphStateCurrent && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsGpuGraphStateCurrent(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1])));
			return true;
		}
		if (method == MethodName.InvalidateGpuRenderGraph && args.Count == 2)
		{
			InvalidateGpuRenderGraph(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<AdobeAnimateGpuGraphInvalidationReason>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlushPendingGpuGraphInvalidations && args.Count == 0)
		{
			FlushPendingGpuGraphInvalidations();
			ret = default;
			return true;
		}
		if (method == MethodName.PruneInvalidGpuGraphRoots && args.Count == 0)
		{
			PruneInvalidGpuGraphRoots();
			ret = default;
			return true;
		}
		if (method == MethodName.MaintainGpuGraphDefinitions && args.Count == 0)
		{
			MaintainGpuGraphDefinitions();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearGpuRenderGraphCaches && args.Count == 1)
		{
			ClearGpuRenderGraphCaches(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCrowdMeshCapacityClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetCrowdMeshCapacityClass(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CapacityForCrowdMeshSlots && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CapacityForCrowdMeshSlots(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveCanvasLayer && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveCanvasLayer(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.WarmupRenderMount && args.Count == 1)
		{
			WarmupRenderMount(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasPreparedRenderMount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPreparedRenderMount(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.TryBeginDetachedGpuRenderGraphWarmupBatch && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryBeginDetachedGpuRenderGraphWarmupBatch());
			return true;
		}
		if (method == MethodName.CommitDetachedGpuRenderGraphWarmupBatch && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CommitDetachedGpuRenderGraphWarmupBatch());
			return true;
		}
		if (method == MethodName.CancelDetachedGpuRenderGraphWarmupBatch && args.Count == 0)
		{
			CancelDetachedGpuRenderGraphWarmupBatch();
			ret = default;
			return true;
		}
		if (method == MethodName.WarmupDetachedGpuRenderGraphs && args.Count == 1)
		{
			WarmupDetachedGpuRenderGraphs(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareDetachedGpuSprites && args.Count == 1)
		{
			PrepareDetachedGpuSprites(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveDetachedGpuRenderRoots && args.Count == 1)
		{
			ResolveDetachedGpuRenderRoots(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveManagerFromStaticLists && args.Count == 1)
		{
			RemoveManagerFromStaticLists(VariantUtils.ConvertTo<AdobeAnimateRenderManager>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunGlobalBucketMaintenance && args.Count == 1)
		{
			RunGlobalBucketMaintenance(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveMountParent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(ResolveMountParent(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ToRenderMountLocalTransform && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(ToRenderMountLocalTransform(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Transform2D>(in args[1])));
			return true;
		}
		if (method == MethodName.BeginRenderMountTransformFrame && args.Count == 1)
		{
			BeginRenderMountTransformFrame(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EndRenderMountTransformFrame && args.Count == 0)
		{
			EndRenderMountTransformFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.FindCanvasLayerAncestor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CanvasLayer>(FindCanvasLayerAncestor(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetRenderFrameVersion && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetRenderFrameVersion());
			return true;
		}
		if (method == MethodName.GetLifecycleTick && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<long>(GetLifecycleTick());
			return true;
		}
		if (method == MethodName.TryAccumulateCrowdCapacityWarmup && args.Count == 2)
		{
			TryAccumulateCrowdCapacityWarmup(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryPrepareCachedOffscreenGpuGraphState && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(TryPrepareCachedOffscreenGpuGraphState(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<float>(in args[2])));
			return true;
		}
		if (method == MethodName.HasPreparedOffscreenGpuGraphState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasPreparedOffscreenGpuGraphState(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.ReleaseImmediateSubmission && args.Count == 1)
		{
			ReleaseImmediateSubmission(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InjectRenderTransactionFailureForTests && args.Count == 1)
		{
			InjectRenderTransactionFailureForTests(VariantUtils.ConvertTo<AdobeAnimateRenderTransactionFaultPhase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ThrowInjectedRenderTransactionFailure && args.Count == 1)
		{
			ret = default;
			return true;
		}
		if (method == MethodName.PublishDirtyImmediateSubmissionManagers && args.Count == 0)
		{
			PublishDirtyImmediateSubmissionManagers();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteSuccessfulFrameTransaction && args.Count == 0)
		{
			CompleteSuccessfulFrameTransaction();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreSpriteAfterFailedTransaction && args.Count == 2)
		{
			RestoreSpriteAfterFailedTransaction(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPhysicsFrameForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ulong>(GetPhysicsFrameForRender());
			return true;
		}
		if (method == MethodName.GetPhysicsTicksPerSecondForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetPhysicsTicksPerSecondForRender());
			return true;
		}
		if (method == MethodName.GetFrameLimitForRender && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetFrameLimitForRender());
			return true;
		}
		if (method == MethodName.BeginFrameTransaction && args.Count == 2)
		{
			BeginFrameTransaction(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EncodeFrameTransaction && args.Count == 2)
		{
			EncodeFrameTransaction(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FreezeFrameTransactionResources && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(FreezeFrameTransactionResources(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.PublishFrameTransaction && args.Count == 2)
		{
			PublishFrameTransaction(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PublishGpuTransaction && args.Count == 2)
		{
			PublishGpuTransaction(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAnimationClock && args.Count == 1)
		{
			SetAnimationClock(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddTransactionManager && args.Count == 2)
		{
			AddTransactionManager(VariantUtils.ConvertTo<AdobeAnimateRenderManager>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsLiveRegisteredManager && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsLiveRegisteredManager(VariantUtils.ConvertTo<AdobeAnimateRenderManager>(in args[0])));
			return true;
		}
		if (method == MethodName.FlushDeferredImmediateSubmissions && args.Count == 0)
		{
			FlushDeferredImmediateSubmissions();
			ret = default;
			return true;
		}
		if (method == MethodName.CompareTreeOrderForRender && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<int>(CompareTreeOrderForRender(VariantUtils.ConvertTo<int[]>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1]), VariantUtils.ConvertTo<int[]>(in args[2]), VariantUtils.ConvertTo<ulong>(in args[3])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.IsValidTextureForFrame)
		{
			return true;
		}
		if (method == MethodName.RollbackPreparedScratch)
		{
			return true;
		}
		if (method == MethodName.RebuildResourceBuilderFromEncodedGroups)
		{
			return true;
		}
		if (method == MethodName.FinalizeFrameResources)
		{
			return true;
		}
		if (method == MethodName.CountEncodedCrowdGroups)
		{
			return true;
		}
		if (method == MethodName.ConvertAllCrowdGroupsToFallback)
		{
			return true;
		}
		if (method == MethodName.PruneCompositeDrawCacheIfNeeded)
		{
			return true;
		}
		if (method == MethodName.IsSameTextureArray)
		{
			return true;
		}
		if (method == MethodName.RebuildPublishedCrowdMaterials)
		{
			return true;
		}
		if (method == MethodName.GetPublishedDynamicOverrideBindingCount)
		{
			return true;
		}
		if (method == MethodName.RebuildPublishedCrowdMaterial)
		{
			return true;
		}
		if (method == MethodName.HaveSameGpuGraphOwners)
		{
			return true;
		}
		if (method == MethodName.PrepareGpuRenderGraphAtlasForFrame)
		{
			return true;
		}
		if (method == MethodName.IsGpuGraphPreparedForTransaction)
		{
			return true;
		}
		if (method == MethodName.IsGpuGraphReady)
		{
			return true;
		}
		if (method == MethodName.IsGpuGraphStateCurrent)
		{
			return true;
		}
		if (method == MethodName.InvalidateGpuRenderGraph)
		{
			return true;
		}
		if (method == MethodName.FlushPendingGpuGraphInvalidations)
		{
			return true;
		}
		if (method == MethodName.PruneInvalidGpuGraphRoots)
		{
			return true;
		}
		if (method == MethodName.MaintainGpuGraphDefinitions)
		{
			return true;
		}
		if (method == MethodName.ClearGpuRenderGraphCaches)
		{
			return true;
		}
		if (method == MethodName.AbortRuntimeFrame)
		{
			return true;
		}
		if (method == MethodName.PrepareSharedCrowdResources)
		{
			return true;
		}
		if (method == MethodName.EnsureCrowdMaterialMatchesFinalSignature)
		{
			return true;
		}
		if (method == MethodName.BindCurrentCrowdBuckets)
		{
			return true;
		}
		if (method == MethodName.GetCrowdMeshCapacityClass)
		{
			return true;
		}
		if (method == MethodName.CapacityForCrowdMeshSlots)
		{
			return true;
		}
		if (method == MethodName.PublishImmediateSnapshotBuckets)
		{
			return true;
		}
		if (method == MethodName.HideRuntimeOutputForFailureHandoff)
		{
			return true;
		}
		if (method == MethodName.HideAllOutput)
		{
			return true;
		}
		if (method == MethodName.HideAndDisposeOwnedResources)
		{
			return true;
		}
		if (method == MethodName.ResolveCanvasLayer)
		{
			return true;
		}
		if (method == MethodName.WarmupRenderMount)
		{
			return true;
		}
		if (method == MethodName.HasPreparedRenderMount)
		{
			return true;
		}
		if (method == MethodName.TryBeginDetachedGpuRenderGraphWarmupBatch)
		{
			return true;
		}
		if (method == MethodName.CommitDetachedGpuRenderGraphWarmupBatch)
		{
			return true;
		}
		if (method == MethodName.CancelDetachedGpuRenderGraphWarmupBatch)
		{
			return true;
		}
		if (method == MethodName.WarmupDetachedGpuRenderGraphs)
		{
			return true;
		}
		if (method == MethodName.PrepareDetachedGpuSprites)
		{
			return true;
		}
		if (method == MethodName.ResolveDetachedGpuRenderRoots)
		{
			return true;
		}
		if (method == MethodName.RemoveManagerFromStaticLists)
		{
			return true;
		}
		if (method == MethodName.RunGlobalBucketMaintenance)
		{
			return true;
		}
		if (method == MethodName.ReclaimIdleBuckets)
		{
			return true;
		}
		if (method == MethodName.ResolveMountParent)
		{
			return true;
		}
		if (method == MethodName.ToRenderMountLocalTransform)
		{
			return true;
		}
		if (method == MethodName.BeginRenderMountTransformFrame)
		{
			return true;
		}
		if (method == MethodName.EndRenderMountTransformFrame)
		{
			return true;
		}
		if (method == MethodName.FindCanvasLayerAncestor)
		{
			return true;
		}
		if (method == MethodName.GetRenderFrameVersion)
		{
			return true;
		}
		if (method == MethodName.GetLifecycleTick)
		{
			return true;
		}
		if (method == MethodName.BeginRuntimeFrame)
		{
			return true;
		}
		if (method == MethodName.EnsureCrowdWriter)
		{
			return true;
		}
		if (method == MethodName.TryAccumulateCrowdCapacityWarmup)
		{
			return true;
		}
		if (method == MethodName.TryPrepareCachedOffscreenGpuGraphState)
		{
			return true;
		}
		if (method == MethodName.HasPreparedOffscreenGpuGraphState)
		{
			return true;
		}
		if (method == MethodName.AccumulateCrowdCapacityWarmup)
		{
			return true;
		}
		if (method == MethodName.ApplyCrowdCapacityWarmup)
		{
			return true;
		}
		if (method == MethodName.WarmupSharedCrowdResources)
		{
			return true;
		}
		if (method == MethodName.EncodeRuntimeFrame)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.DisableIdleProcess)
		{
			return true;
		}
		if (method == MethodName.ReleaseImmediateSubmission)
		{
			return true;
		}
		if (method == MethodName.InjectRenderTransactionFailureForTests)
		{
			return true;
		}
		if (method == MethodName.ThrowInjectedRenderTransactionFailure)
		{
			return true;
		}
		if (method == MethodName.PublishDirtyImmediateSubmissionManagers)
		{
			return true;
		}
		if (method == MethodName.CompleteSuccessfulFrameTransaction)
		{
			return true;
		}
		if (method == MethodName.RestoreSpriteAfterFailedTransaction)
		{
			return true;
		}
		if (method == MethodName.GetPhysicsFrameForRender)
		{
			return true;
		}
		if (method == MethodName.GetPhysicsTicksPerSecondForRender)
		{
			return true;
		}
		if (method == MethodName.GetFrameLimitForRender)
		{
			return true;
		}
		if (method == MethodName.BeginFrameTransaction)
		{
			return true;
		}
		if (method == MethodName.EncodeFrameTransaction)
		{
			return true;
		}
		if (method == MethodName.FreezeFrameTransactionResources)
		{
			return true;
		}
		if (method == MethodName.PublishFrameTransaction)
		{
			return true;
		}
		if (method == MethodName.PublishGpuTransaction)
		{
			return true;
		}
		if (method == MethodName.SetAnimationClock)
		{
			return true;
		}
		if (method == MethodName.ApplyCrowdClockIfNeeded)
		{
			return true;
		}
		if (method == MethodName.AddTransactionManager)
		{
			return true;
		}
		if (method == MethodName.IsLiveRegisteredManager)
		{
			return true;
		}
		if (method == MethodName.FlushDeferredImmediateSubmissions)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.CompareTreeOrderForRender)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._crowdWriter)
		{
			_crowdWriter = VariantUtils.ConvertTo<AdobeAnimateMultiMeshBatcher>(in value);
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
		if (name == PropertyName._crowdMeshQuadCapacity)
		{
			_crowdMeshQuadCapacity = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._crowdMeshVersion)
		{
			_crowdMeshVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._runtimeFrameVersion)
		{
			_runtimeFrameVersion = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._lifecycleTick)
		{
			_lifecycleTick = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._transactionFrameMarker)
		{
			_transactionFrameMarker = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._currentFrameMarker)
		{
			_currentFrameMarker = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._appliedAnimationClockVersion)
		{
			_appliedAnimationClockVersion = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._mountId)
		{
			_mountId = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._mountParent)
		{
			_mountParent = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._animationClockMaterial)
		{
			_animationClockMaterial = VariantUtils.ConvertTo<ShaderMaterial>(in value);
			return true;
		}
		if (name == PropertyName._crowdMaterialStateTextureRid)
		{
			_crowdMaterialStateTextureRid = VariantUtils.ConvertTo<Rid>(in value);
			return true;
		}
		if (name == PropertyName._crowdMaterialStateTextureSize)
		{
			_crowdMaterialStateTextureSize = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._crowdMaterialStateTextureLayers)
		{
			_crowdMaterialStateTextureLayers = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._hasFinalResourceSignature)
		{
			_hasFinalResourceSignature = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasCrowdMaterialResourceSignature)
		{
			_hasCrowdMaterialResourceSignature = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hasCurrentOutput)
		{
			_hasCurrentOutput = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._hadCurrentOutputAtFrameBegin)
		{
			_hadCurrentOutputAtFrameBegin = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._compositeCachePruneCountdown)
		{
			_compositeCachePruneCountdown = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._frameSignatureConflictBuckets)
		{
			_frameSignatureConflictBuckets = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._frameMeshRebinds)
		{
			_frameMeshRebinds = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._framePublishedCrowdRoots)
		{
			_framePublishedCrowdRoots = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._framePublishedZBuckets)
		{
			_framePublishedZBuckets = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._frameStateTexels)
		{
			_frameStateTexels = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._frameStateUploads)
		{
			_frameStateUploads = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._frameStateUpdatedLayers)
		{
			_frameStateUpdatedLayers = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._frameStateUploadedBytes)
		{
			_frameStateUploadedBytes = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._frameMultiMeshUploads)
		{
			_frameMultiMeshUploads = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._frameFallbackRuns)
		{
			_frameFallbackRuns = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._frameFallbackRoots)
		{
			_frameFallbackRoots = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._crowdCapacityWarmupFrame)
		{
			_crowdCapacityWarmupFrame = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._crowdCapacityWarmupMaxQuadCount)
		{
			_crowdCapacityWarmupMaxQuadCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.CrowdMeshVersion)
		{
			value = VariantUtils.CreateFrom<int>(CrowdMeshVersion);
			return true;
		}
		if (name == PropertyName._crowdWriter)
		{
			value = VariantUtils.CreateFrom(in _crowdWriter);
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
		if (name == PropertyName._crowdMeshQuadCapacity)
		{
			value = VariantUtils.CreateFrom(in _crowdMeshQuadCapacity);
			return true;
		}
		if (name == PropertyName._crowdMeshVersion)
		{
			value = VariantUtils.CreateFrom(in _crowdMeshVersion);
			return true;
		}
		if (name == PropertyName._runtimeFrameVersion)
		{
			value = VariantUtils.CreateFrom(in _runtimeFrameVersion);
			return true;
		}
		if (name == PropertyName._lifecycleTick)
		{
			value = VariantUtils.CreateFrom(in _lifecycleTick);
			return true;
		}
		if (name == PropertyName._transactionFrameMarker)
		{
			value = VariantUtils.CreateFrom(in _transactionFrameMarker);
			return true;
		}
		if (name == PropertyName._currentFrameMarker)
		{
			value = VariantUtils.CreateFrom(in _currentFrameMarker);
			return true;
		}
		if (name == PropertyName._appliedAnimationClockVersion)
		{
			value = VariantUtils.CreateFrom(in _appliedAnimationClockVersion);
			return true;
		}
		if (name == PropertyName._mountId)
		{
			value = VariantUtils.CreateFrom(in _mountId);
			return true;
		}
		if (name == PropertyName._mountParent)
		{
			value = VariantUtils.CreateFrom(in _mountParent);
			return true;
		}
		if (name == PropertyName._animationClockMaterial)
		{
			value = VariantUtils.CreateFrom(in _animationClockMaterial);
			return true;
		}
		if (name == PropertyName._crowdMaterialStateTextureRid)
		{
			value = VariantUtils.CreateFrom(in _crowdMaterialStateTextureRid);
			return true;
		}
		if (name == PropertyName._crowdMaterialStateTextureSize)
		{
			value = VariantUtils.CreateFrom(in _crowdMaterialStateTextureSize);
			return true;
		}
		if (name == PropertyName._crowdMaterialStateTextureLayers)
		{
			value = VariantUtils.CreateFrom(in _crowdMaterialStateTextureLayers);
			return true;
		}
		if (name == PropertyName._hasFinalResourceSignature)
		{
			value = VariantUtils.CreateFrom(in _hasFinalResourceSignature);
			return true;
		}
		if (name == PropertyName._hasCrowdMaterialResourceSignature)
		{
			value = VariantUtils.CreateFrom(in _hasCrowdMaterialResourceSignature);
			return true;
		}
		if (name == PropertyName._hasCurrentOutput)
		{
			value = VariantUtils.CreateFrom(in _hasCurrentOutput);
			return true;
		}
		if (name == PropertyName._hadCurrentOutputAtFrameBegin)
		{
			value = VariantUtils.CreateFrom(in _hadCurrentOutputAtFrameBegin);
			return true;
		}
		if (name == PropertyName._compositeCachePruneCountdown)
		{
			value = VariantUtils.CreateFrom(in _compositeCachePruneCountdown);
			return true;
		}
		if (name == PropertyName._frameSignatureConflictBuckets)
		{
			value = VariantUtils.CreateFrom(in _frameSignatureConflictBuckets);
			return true;
		}
		if (name == PropertyName._frameMeshRebinds)
		{
			value = VariantUtils.CreateFrom(in _frameMeshRebinds);
			return true;
		}
		if (name == PropertyName._framePublishedCrowdRoots)
		{
			value = VariantUtils.CreateFrom(in _framePublishedCrowdRoots);
			return true;
		}
		if (name == PropertyName._framePublishedZBuckets)
		{
			value = VariantUtils.CreateFrom(in _framePublishedZBuckets);
			return true;
		}
		if (name == PropertyName._frameStateTexels)
		{
			value = VariantUtils.CreateFrom(in _frameStateTexels);
			return true;
		}
		if (name == PropertyName._frameStateUploads)
		{
			value = VariantUtils.CreateFrom(in _frameStateUploads);
			return true;
		}
		if (name == PropertyName._frameStateUpdatedLayers)
		{
			value = VariantUtils.CreateFrom(in _frameStateUpdatedLayers);
			return true;
		}
		if (name == PropertyName._frameStateUploadedBytes)
		{
			value = VariantUtils.CreateFrom(in _frameStateUploadedBytes);
			return true;
		}
		if (name == PropertyName._frameMultiMeshUploads)
		{
			value = VariantUtils.CreateFrom(in _frameMultiMeshUploads);
			return true;
		}
		if (name == PropertyName._frameFallbackRuns)
		{
			value = VariantUtils.CreateFrom(in _frameFallbackRuns);
			return true;
		}
		if (name == PropertyName._frameFallbackRoots)
		{
			value = VariantUtils.CreateFrom(in _frameFallbackRoots);
			return true;
		}
		if (name == PropertyName._crowdCapacityWarmupFrame)
		{
			value = VariantUtils.CreateFrom(in _crowdCapacityWarmupFrame);
			return true;
		}
		if (name == PropertyName._crowdCapacityWarmupMaxQuadCount)
		{
			value = VariantUtils.CreateFrom(in _crowdCapacityWarmupMaxQuadCount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._crowdWriter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._crowdMaterial, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._crowdMesh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdMeshQuadCapacity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdMeshVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._runtimeFrameVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lifecycleTick, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._transactionFrameMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentFrameMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._appliedAnimationClockVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._mountId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mountParent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationClockMaterial, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rid, PropertyName._crowdMaterialStateTextureRid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._crowdMaterialStateTextureSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdMaterialStateTextureLayers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasFinalResourceSignature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasCrowdMaterialResourceSignature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hasCurrentOutput, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hadCurrentOutputAtFrameBegin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._compositeCachePruneCountdown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameSignatureConflictBuckets, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameMeshRebinds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._framePublishedCrowdRoots, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._framePublishedZBuckets, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameStateTexels, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameStateUploads, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameStateUpdatedLayers, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameStateUploadedBytes, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameMultiMeshUploads, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameFallbackRuns, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._frameFallbackRoots, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdCapacityWarmupFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._crowdCapacityWarmupMaxQuadCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CrowdMeshVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._crowdWriter, Variant.From(in _crowdWriter));
		info.AddProperty(PropertyName._crowdMaterial, Variant.From(in _crowdMaterial));
		info.AddProperty(PropertyName._crowdMesh, Variant.From(in _crowdMesh));
		info.AddProperty(PropertyName._crowdMeshQuadCapacity, Variant.From(in _crowdMeshQuadCapacity));
		info.AddProperty(PropertyName._crowdMeshVersion, Variant.From(in _crowdMeshVersion));
		info.AddProperty(PropertyName._runtimeFrameVersion, Variant.From(in _runtimeFrameVersion));
		info.AddProperty(PropertyName._lifecycleTick, Variant.From(in _lifecycleTick));
		info.AddProperty(PropertyName._transactionFrameMarker, Variant.From(in _transactionFrameMarker));
		info.AddProperty(PropertyName._currentFrameMarker, Variant.From(in _currentFrameMarker));
		info.AddProperty(PropertyName._appliedAnimationClockVersion, Variant.From(in _appliedAnimationClockVersion));
		info.AddProperty(PropertyName._mountId, Variant.From(in _mountId));
		info.AddProperty(PropertyName._mountParent, Variant.From(in _mountParent));
		info.AddProperty(PropertyName._animationClockMaterial, Variant.From(in _animationClockMaterial));
		info.AddProperty(PropertyName._crowdMaterialStateTextureRid, Variant.From(in _crowdMaterialStateTextureRid));
		info.AddProperty(PropertyName._crowdMaterialStateTextureSize, Variant.From(in _crowdMaterialStateTextureSize));
		info.AddProperty(PropertyName._crowdMaterialStateTextureLayers, Variant.From(in _crowdMaterialStateTextureLayers));
		info.AddProperty(PropertyName._hasFinalResourceSignature, Variant.From(in _hasFinalResourceSignature));
		info.AddProperty(PropertyName._hasCrowdMaterialResourceSignature, Variant.From(in _hasCrowdMaterialResourceSignature));
		info.AddProperty(PropertyName._hasCurrentOutput, Variant.From(in _hasCurrentOutput));
		info.AddProperty(PropertyName._hadCurrentOutputAtFrameBegin, Variant.From(in _hadCurrentOutputAtFrameBegin));
		info.AddProperty(PropertyName._compositeCachePruneCountdown, Variant.From(in _compositeCachePruneCountdown));
		info.AddProperty(PropertyName._frameSignatureConflictBuckets, Variant.From(in _frameSignatureConflictBuckets));
		info.AddProperty(PropertyName._frameMeshRebinds, Variant.From(in _frameMeshRebinds));
		info.AddProperty(PropertyName._framePublishedCrowdRoots, Variant.From(in _framePublishedCrowdRoots));
		info.AddProperty(PropertyName._framePublishedZBuckets, Variant.From(in _framePublishedZBuckets));
		info.AddProperty(PropertyName._frameStateTexels, Variant.From(in _frameStateTexels));
		info.AddProperty(PropertyName._frameStateUploads, Variant.From(in _frameStateUploads));
		info.AddProperty(PropertyName._frameStateUpdatedLayers, Variant.From(in _frameStateUpdatedLayers));
		info.AddProperty(PropertyName._frameStateUploadedBytes, Variant.From(in _frameStateUploadedBytes));
		info.AddProperty(PropertyName._frameMultiMeshUploads, Variant.From(in _frameMultiMeshUploads));
		info.AddProperty(PropertyName._frameFallbackRuns, Variant.From(in _frameFallbackRuns));
		info.AddProperty(PropertyName._frameFallbackRoots, Variant.From(in _frameFallbackRoots));
		info.AddProperty(PropertyName._crowdCapacityWarmupFrame, Variant.From(in _crowdCapacityWarmupFrame));
		info.AddProperty(PropertyName._crowdCapacityWarmupMaxQuadCount, Variant.From(in _crowdCapacityWarmupMaxQuadCount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._crowdWriter, out var value))
		{
			_crowdWriter = value.As<AdobeAnimateMultiMeshBatcher>();
		}
		if (info.TryGetProperty(PropertyName._crowdMaterial, out var value2))
		{
			_crowdMaterial = value2.As<ShaderMaterial>();
		}
		if (info.TryGetProperty(PropertyName._crowdMesh, out var value3))
		{
			_crowdMesh = value3.As<ArrayMesh>();
		}
		if (info.TryGetProperty(PropertyName._crowdMeshQuadCapacity, out var value4))
		{
			_crowdMeshQuadCapacity = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._crowdMeshVersion, out var value5))
		{
			_crowdMeshVersion = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._runtimeFrameVersion, out var value6))
		{
			_runtimeFrameVersion = value6.As<long>();
		}
		if (info.TryGetProperty(PropertyName._lifecycleTick, out var value7))
		{
			_lifecycleTick = value7.As<long>();
		}
		if (info.TryGetProperty(PropertyName._transactionFrameMarker, out var value8))
		{
			_transactionFrameMarker = value8.As<long>();
		}
		if (info.TryGetProperty(PropertyName._currentFrameMarker, out var value9))
		{
			_currentFrameMarker = value9.As<long>();
		}
		if (info.TryGetProperty(PropertyName._appliedAnimationClockVersion, out var value10))
		{
			_appliedAnimationClockVersion = value10.As<long>();
		}
		if (info.TryGetProperty(PropertyName._mountId, out var value11))
		{
			_mountId = value11.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._mountParent, out var value12))
		{
			_mountParent = value12.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._animationClockMaterial, out var value13))
		{
			_animationClockMaterial = value13.As<ShaderMaterial>();
		}
		if (info.TryGetProperty(PropertyName._crowdMaterialStateTextureRid, out var value14))
		{
			_crowdMaterialStateTextureRid = value14.As<Rid>();
		}
		if (info.TryGetProperty(PropertyName._crowdMaterialStateTextureSize, out var value15))
		{
			_crowdMaterialStateTextureSize = value15.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._crowdMaterialStateTextureLayers, out var value16))
		{
			_crowdMaterialStateTextureLayers = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName._hasFinalResourceSignature, out var value17))
		{
			_hasFinalResourceSignature = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasCrowdMaterialResourceSignature, out var value18))
		{
			_hasCrowdMaterialResourceSignature = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hasCurrentOutput, out var value19))
		{
			_hasCurrentOutput = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._hadCurrentOutputAtFrameBegin, out var value20))
		{
			_hadCurrentOutputAtFrameBegin = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._compositeCachePruneCountdown, out var value21))
		{
			_compositeCachePruneCountdown = value21.As<int>();
		}
		if (info.TryGetProperty(PropertyName._frameSignatureConflictBuckets, out var value22))
		{
			_frameSignatureConflictBuckets = value22.As<int>();
		}
		if (info.TryGetProperty(PropertyName._frameMeshRebinds, out var value23))
		{
			_frameMeshRebinds = value23.As<int>();
		}
		if (info.TryGetProperty(PropertyName._framePublishedCrowdRoots, out var value24))
		{
			_framePublishedCrowdRoots = value24.As<int>();
		}
		if (info.TryGetProperty(PropertyName._framePublishedZBuckets, out var value25))
		{
			_framePublishedZBuckets = value25.As<int>();
		}
		if (info.TryGetProperty(PropertyName._frameStateTexels, out var value26))
		{
			_frameStateTexels = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName._frameStateUploads, out var value27))
		{
			_frameStateUploads = value27.As<int>();
		}
		if (info.TryGetProperty(PropertyName._frameStateUpdatedLayers, out var value28))
		{
			_frameStateUpdatedLayers = value28.As<int>();
		}
		if (info.TryGetProperty(PropertyName._frameStateUploadedBytes, out var value29))
		{
			_frameStateUploadedBytes = value29.As<int>();
		}
		if (info.TryGetProperty(PropertyName._frameMultiMeshUploads, out var value30))
		{
			_frameMultiMeshUploads = value30.As<int>();
		}
		if (info.TryGetProperty(PropertyName._frameFallbackRuns, out var value31))
		{
			_frameFallbackRuns = value31.As<int>();
		}
		if (info.TryGetProperty(PropertyName._frameFallbackRoots, out var value32))
		{
			_frameFallbackRoots = value32.As<int>();
		}
		if (info.TryGetProperty(PropertyName._crowdCapacityWarmupFrame, out var value33))
		{
			_crowdCapacityWarmupFrame = value33.As<long>();
		}
		if (info.TryGetProperty(PropertyName._crowdCapacityWarmupMaxQuadCount, out var value34))
		{
			_crowdCapacityWarmupMaxQuadCount = value34.As<int>();
		}
	}
}
