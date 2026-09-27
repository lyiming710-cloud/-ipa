using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://Test/Test.cs")]
public class Test : Node2D
{
	private sealed class ExternalVisualStressFixture
	{
		public TowerDefenseCharacter Character;

		public AdobeAnimateSprite Owner;

		public Sprite2D Visual;

		public AdobeAnimateExternalVisualHandle Handle = AdobeAnimateExternalVisualHandle.Invalid;

		public bool OwnsVisual;

		public int StableIndex;
	}

	private struct RenderManagerStats
	{
		public int RenderManagers;

		public int NormalBuckets;

		public int VisibleInstances;

		public int AllocatedInstances;

		public int MaxBucketVisible;

		public int CompactCrowdRoots;

		public int CompositeCrowdRoots;

		public int GpuGraphRoots;

		public int GpuGraphSlots;

		public int GpuGraphStateTexels;

		public int CrowdStateTexels;
	}

	public new class MethodName : Node2D.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OnStressResourceManagerLoadOver = "OnStressResourceManagerLoadOver";

		public static readonly StringName StartStressWorkload = "StartStressWorkload";

		public static readonly StringName InitializeRenderBackendUi = "InitializeRenderBackendUi";

		public static readonly StringName HandleRenderBackendSelected = "HandleRenderBackendSelected";

		public static readonly StringName RestartMeasurementAfterBackendSwitch = "RestartMeasurementAfterBackendSwitch";

		public static readonly StringName UpdateRenderBackendUi = "UpdateRenderBackendUi";

		public static readonly StringName RefreshRenderBackendUi = "RefreshRenderBackendUi";

		public static readonly StringName RunComponentAliveSchedulingRegressionAsync = "RunComponentAliveSchedulingRegressionAsync";

		public static readonly StringName RunStatePhysicsBatchRateRegressionAsync = "RunStatePhysicsBatchRateRegressionAsync";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName CompleteSpawn = "CompleteSpawn";

		public static readonly StringName PrepareAnimationCadenceRegression = "PrepareAnimationCadenceRegression";

		public static readonly StringName FindCadenceSlot = "FindCadenceSlot";

		public static readonly StringName SampleAnimationCadence = "SampleAnimationCadence";

		public static readonly StringName IsAnimationCadencePassed = "IsAnimationCadencePassed";

		public static readonly StringName ForceCpuPoseFallback = "ForceCpuPoseFallback";

		public static readonly StringName PrintScheduledNodeTypeSnapshot = "PrintScheduledNodeTypeSnapshot";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName StartMeasurementTelemetry = "StartMeasurementTelemetry";

		public static readonly StringName HasValidMeasurementTelemetry = "HasValidMeasurementTelemetry";

		public static readonly StringName TrySwitchAnimeFrameRate = "TrySwitchAnimeFrameRate";

		public static readonly StringName TryRunAnimationSwitchStress = "TryRunAnimationSwitchStress";

		public static readonly StringName TryRunBowlingImpactTweenStress = "TryRunBowlingImpactTweenStress";

		public static readonly StringName CountProcessedTweens = "CountProcessedTweens";

		public static readonly StringName CountBowlingImpactMotions = "CountBowlingImpactMotions";

		public static readonly StringName TryApplyRuntimeVisualMutation = "TryApplyRuntimeVisualMutation";

		public static readonly StringName TryApplyViewportEntryMutation = "TryApplyViewportEntryMutation";

		public static readonly StringName CaptureViewportEntryFrame = "CaptureViewportEntryFrame";

		public static readonly StringName PrintViewportEntryPerformance = "PrintViewportEntryPerformance";

		public static readonly StringName TryApplyDamagePartMutation = "TryApplyDamagePartMutation";

		public static readonly StringName ValidateNewDamagePartNode = "ValidateNewDamagePartNode";

		public static readonly StringName CountActiveDamagePartNodes = "CountActiveDamagePartNodes";

		public static readonly StringName PrintNonGpuGraphCrowdModes = "PrintNonGpuGraphCrowdModes";

		public static readonly StringName EnforceFixedAnimationFrame = "EnforceFixedAnimationFrame";

		public static readonly StringName EnforceAdobeFixedAnimationFrame = "EnforceAdobeFixedAnimationFrame";

		public static readonly StringName ApplyFixedAnimationFrame = "ApplyFixedAnimationFrame";

		public static readonly StringName SpawnPendingBatch = "SpawnPendingBatch";

		public static readonly StringName SpawnRange = "SpawnRange";

		public static readonly StringName ConfigureExternalVisualStress = "ConfigureExternalVisualStress";

		public static readonly StringName ShouldEnableExternalVisual = "ShouldEnableExternalVisual";

		public static readonly StringName LoadDifferentZombieScenePool = "LoadDifferentZombieScenePool";

		public static readonly StringName ShouldExcludeZombieScenePath = "ShouldExcludeZombieScenePath";

		public static readonly StringName IsZombieSceneFile = "IsZombieSceneFile";

		public static readonly StringName IsZombieAnimationSceneFile = "IsZombieAnimationSceneFile";

		public static readonly StringName IsZombieCharacterSceneFile = "IsZombieCharacterSceneFile";

		public static readonly StringName RegisterSpawnedNode = "RegisterSpawnedNode";

		public static readonly StringName ConfigureCombatCharacterPreservation = "ConfigureCombatCharacterPreservation";

		public static readonly StringName ShouldFreezeCombatSourcesForInitialSleep = "ShouldFreezeCombatSourcesForInitialSleep";

		public static readonly StringName RestoreCombatSourceProcessModes = "RestoreCombatSourceProcessModes";

		public static readonly StringName PrepareSingleEnemyTargetFixture = "PrepareSingleEnemyTargetFixture";

		public static readonly StringName PrepareManyEnemyIdleTargetFixture = "PrepareManyEnemyIdleTargetFixture";

		public static readonly StringName TryDriveSingleEnemyRoleAttackUtilities = "TryDriveSingleEnemyRoleAttackUtilities";

		public static readonly StringName TryRunSleepCycleStress = "TryRunSleepCycleStress";

		public static readonly StringName BeginSleepCycleStress = "BeginSleepCycleStress";

		public static readonly StringName FindHypnotistSleepEventForStress = "FindHypnotistSleepEventForStress";

		public static readonly StringName CountSleepingStressCharacters = "CountSleepingStressCharacters";

		public static readonly StringName CountWokenStressCharacters = "CountWokenStressCharacters";

		public static readonly StringName FormatSleepCycleMissingBuffScenes = "FormatSleepCycleMissingBuffScenes";

		public static readonly StringName FormatSleepCycleMissingEntryScenes = "FormatSleepCycleMissingEntryScenes";

		public static readonly StringName CompleteSleepCycleStress = "CompleteSleepCycleStress";

		public static readonly StringName RestoreSleepCycleImmunity = "RestoreSleepCycleImmunity";

		public static readonly StringName IsSleepCycleStressPassed = "IsSleepCycleStressPassed";

		public static readonly StringName TryAdvanceSingleEnemyRoleAttackModes = "TryAdvanceSingleEnemyRoleAttackModes";

		public static readonly StringName DetachCombatComponentEngagementHandlers = "DetachCombatComponentEngagementHandlers";

		public static readonly StringName IsCombatComponentEngagementPassed = "IsCombatComponentEngagementPassed";

		public static readonly StringName CountValidManyEnemyIdleTargets = "CountValidManyEnemyIdleTargets";

		public static readonly StringName CountManyEnemyIdleBusyStates = "CountManyEnemyIdleBusyStates";

		public static readonly StringName IsManyEnemyIdleFixturePassed = "IsManyEnemyIdleFixturePassed";

		public static readonly StringName FormatAttackComponentEngagementBreakdown = "FormatAttackComponentEngagementBreakdown";

		public static readonly StringName FormatFireComponentEngagementBreakdown = "FormatFireComponentEngagementBreakdown";

		public static readonly StringName GetCombatTargetDurability = "GetCombatTargetDurability";

		public static readonly StringName PrintExactRoleComponentInventory = "PrintExactRoleComponentInventory";

		public static readonly StringName ResolveSpawnPosition = "ResolveSpawnPosition";

		public static readonly StringName ResolveSingleTargetSourcePosition = "ResolveSingleTargetSourcePosition";

		public static readonly StringName ResolveSingleTargetSourceGridPosition = "ResolveSingleTargetSourceGridPosition";

		public static readonly StringName EnsureZInterleaveFixture = "EnsureZInterleaveFixture";

		public static readonly StringName PrintRuntimeSnapshot = "PrintRuntimeSnapshot";

		public static readonly StringName PrintExternalVisualPositionDiagnostics = "PrintExternalVisualPositionDiagnostics";

		public static readonly StringName FormatVector = "FormatVector";

		public static readonly StringName ReportExternalVisualStressFailures = "ReportExternalVisualStressFailures";

		public static readonly StringName GetAverageMeasuredFps = "GetAverageMeasuredFps";

		public static readonly StringName GetMeasuredSeconds = "GetMeasuredSeconds";

		public static readonly StringName PrintPerformanceResult = "PrintPerformanceResult";

		public static readonly StringName IsAnimationSwitchStressPassed = "IsAnimationSwitchStressPassed";

		public static readonly StringName PrintAnimationSwitchStressResult = "PrintAnimationSwitchStressResult";

		public static readonly StringName IsBowlingImpactTweenStressPassed = "IsBowlingImpactTweenStressPassed";

		public static readonly StringName PrintBowlingImpactTweenStressResult = "PrintBowlingImpactTweenStressResult";

		public static readonly StringName CountValidSpawnedCharacters = "CountValidSpawnedCharacters";

		public static readonly StringName FormatInvalidSpawnedCharacterScenes = "FormatInvalidSpawnedCharacterScenes";

		public static readonly StringName CountStateChartRelatedNodes = "CountStateChartRelatedNodes";

		public static readonly StringName GetAllocationMetric = "GetAllocationMetric";

		public static readonly StringName TryCapturePerformanceScreenshot = "TryCapturePerformanceScreenshot";

		public static readonly StringName HasSettledDamagePartForScreenshot = "HasSettledDamagePartForScreenshot";

		public static readonly StringName PrintScenePoolSnapshot = "PrintScenePoolSnapshot";

		public static readonly StringName PrintCrowdFilterDebugSnapshot = "PrintCrowdFilterDebugSnapshot";

		public static readonly StringName GetWorldViewportRect = "GetWorldViewportRect";

		public static readonly StringName ElapsedMs = "ElapsedMs";

		public static readonly StringName FormatRect = "FormatRect";

		public static readonly StringName NormalizeResDirectory = "NormalizeResDirectory";

		public static readonly StringName GetFileNameWithoutExtension = "GetFileNameWithoutExtension";

		public static readonly StringName ConfigureUnfocusedBenchmarkRun = "ConfigureUnfocusedBenchmarkRun";

		public static readonly StringName ApplyCommandLineOverrides = "ApplyCommandLineOverrides";

		public static readonly StringName GetTestRenderBackendLabel = "GetTestRenderBackendLabel";

		public static readonly StringName ConfigureCombatStateStress = "ConfigureCombatStateStress";

		public static readonly StringName RestoreCombatStateStress = "RestoreCombatStateStress";
	}

	public new class PropertyName : Node2D.PropertyName
	{
		public static readonly StringName TestScene = "TestScene";

		public static readonly StringName ExactScenePath = "ExactScenePath";

		public static readonly StringName ExactAnimationClip = "ExactAnimationClip";

		public static readonly StringName SpawnCount = "SpawnCount";

		public static readonly StringName SpawnOrigin = "SpawnOrigin";

		public static readonly StringName SpawnArea = "SpawnArea";

		public static readonly StringName UseDifferentZombieScenePool = "UseDifferentZombieScenePool";

		public static readonly StringName UseZombieCharacterScenes = "UseZombieCharacterScenes";

		public static readonly StringName ZombieSceneRoot = "ZombieSceneRoot";

		public static readonly StringName CharacterSceneFilePrefix = "CharacterSceneFilePrefix";

		public static readonly StringName ExcludedZombieScenePathSubstring = "ExcludedZombieScenePathSubstring";

		public static readonly StringName MaxDifferentZombieScenes = "MaxDifferentZombieScenes";

		public static readonly StringName SpawnBatchSize = "SpawnBatchSize";

		public static readonly StringName SpawnTimeBudgetMilliseconds = "SpawnTimeBudgetMilliseconds";

		public static readonly StringName UseGridSpawnLayout = "UseGridSpawnLayout";

		public static readonly StringName SpawnGridColumns = "SpawnGridColumns";

		public static readonly StringName RenderZBucketCount = "RenderZBucketCount";

		public static readonly StringName SpawnScale = "SpawnScale";

		public static readonly StringName SpawnRandomSeed = "SpawnRandomSeed";

		public static readonly StringName AutoQuitAfterSeconds = "AutoQuitAfterSeconds";

		public static readonly StringName WarmupSeconds = "WarmupSeconds";

		public static readonly StringName MinimumAverageFps = "MinimumAverageFps";

		public static readonly StringName MaximumFrameP99Milliseconds = "MaximumFrameP99Milliseconds";

		public static readonly StringName MaximumFrameMilliseconds = "MaximumFrameMilliseconds";

		public static readonly StringName MinimumOnePercentLowFps = "MinimumOnePercentLowFps";

		public static readonly StringName BenchmarkMaxFps = "BenchmarkMaxFps";

		public static readonly StringName BenchmarkPhysicsTicksPerSecond = "BenchmarkPhysicsTicksPerSecond";

		public static readonly StringName ScreenshotPath = "ScreenshotPath";

		public static readonly StringName WaitForDamagePartSettledBeforeScreenshot = "WaitForDamagePartSettledBeforeScreenshot";

		public static readonly StringName PrintIntervalSeconds = "PrintIntervalSeconds";

		public static readonly StringName EnableAdobeAnimateProfiler = "EnableAdobeAnimateProfiler";

		public static readonly StringName EnableDetailedAdobeAnimateProfiler = "EnableDetailedAdobeAnimateProfiler";

		public static readonly StringName EnableAllocationTelemetry = "EnableAllocationTelemetry";

		public static readonly StringName ProfilerDumpIntervalFrames = "ProfilerDumpIntervalFrames";

		public static readonly StringName ProfilerMaxMetricsPerDump = "ProfilerMaxMetricsPerDump";

		public static readonly StringName ClearAdobeAnimateRuntimeCachesBeforeRun = "ClearAdobeAnimateRuntimeCachesBeforeRun";

		public static readonly StringName EnableGpuRenderGraph = "EnableGpuRenderGraph";

		public static readonly StringName ForceCpuPoseFallbackForTest = "ForceCpuPoseFallbackForTest";

		public static readonly StringName FixedAnimationFrame = "FixedAnimationFrame";

		public static readonly StringName FixedAnimationSubframe = "FixedAnimationSubframe";

		public static readonly StringName PrintCrowdFilterDebugData = "PrintCrowdFilterDebugData";

		public static readonly StringName CrowdFilterDebugSpriteCount = "CrowdFilterDebugSpriteCount";

		public static readonly StringName CrowdFilterDebugMaxFrameSlices = "CrowdFilterDebugMaxFrameSlices";

		public static readonly StringName PrintCrowdFilterDebugAfterFirstRuntimeFrame = "PrintCrowdFilterDebugAfterFirstRuntimeFrame";

		public static readonly StringName PrintCrowdFilterDebugEveryRuntimeSnapshot = "PrintCrowdFilterDebugEveryRuntimeSnapshot";

		public static readonly StringName EnableZInterleaveFixture = "EnableZInterleaveFixture";

		public static readonly StringName PrintCrowdModeReport = "PrintCrowdModeReport";

		public static readonly StringName InitialAnimeFrameRate = "InitialAnimeFrameRate";

		public static readonly StringName SwitchAnimeFrameRateAfterSeconds = "SwitchAnimeFrameRateAfterSeconds";

		public static readonly StringName SwitchAnimeFrameRateTo = "SwitchAnimeFrameRateTo";

		public static readonly StringName RuntimeVisualMutation = "RuntimeVisualMutation";

		public static readonly StringName RuntimeVisualMutationAfterSeconds = "RuntimeVisualMutationAfterSeconds";

		public static readonly StringName EnableAnimationSwitchStress = "EnableAnimationSwitchStress";

		public static readonly StringName AnimationSwitchClipA = "AnimationSwitchClipA";

		public static readonly StringName AnimationSwitchClipB = "AnimationSwitchClipB";

		public static readonly StringName AnimationSwitchIntervalSeconds = "AnimationSwitchIntervalSeconds";

		public static readonly StringName AnimationSwitchBlendSeconds = "AnimationSwitchBlendSeconds";

		public static readonly StringName AnimationSwitchStartAfterSeconds = "AnimationSwitchStartAfterSeconds";

		public static readonly StringName EnableBowlingImpactTweenStress = "EnableBowlingImpactTweenStress";

		public static readonly StringName BowlingImpactEnsureShield = "BowlingImpactEnsureShield";

		public static readonly StringName BowlingImpactUseProductionEvent = "BowlingImpactUseProductionEvent";

		public static readonly StringName BowlingImpactProcessedTweenLimit = "BowlingImpactProcessedTweenLimit";

		public static readonly StringName BowlingImpactTargetsPerBurst = "BowlingImpactTargetsPerBurst";

		public static readonly StringName BowlingImpactDamage = "BowlingImpactDamage";

		public static readonly StringName BowlingImpactBurstIntervalSeconds = "BowlingImpactBurstIntervalSeconds";

		public static readonly StringName BowlingImpactActiveWindowSeconds = "BowlingImpactActiveWindowSeconds";

		public static readonly StringName BowlingImpactStartAfterSeconds = "BowlingImpactStartAfterSeconds";

		public static readonly StringName RunComponentAliveSchedulingRegression = "RunComponentAliveSchedulingRegression";

		public static readonly StringName RunStatePhysicsBatchRateRegression = "RunStatePhysicsBatchRateRegression";

		public static readonly StringName RunAnimationCadenceRegression = "RunAnimationCadenceRegression";

		public static readonly StringName EnableExternalVisualStress = "EnableExternalVisualStress";

		public static readonly StringName ExternalVisualIceOnly = "ExternalVisualIceOnly";

		public static readonly StringName ExternalVisualRatio = "ExternalVisualRatio";

		public static readonly StringName EnableCombatStateStress = "EnableCombatStateStress";

		public static readonly StringName PreserveCombatCharactersInStress = "PreserveCombatCharactersInStress";

		public static readonly StringName CombatStressIsNight = "CombatStressIsNight";

		public static readonly StringName ShowCharacterHealthInStress = "ShowCharacterHealthInStress";

		public static readonly StringName EnableSingleEnemyTargetFixture = "EnableSingleEnemyTargetFixture";

		public static readonly StringName SingleEnemyTargetScenePath = "SingleEnemyTargetScenePath";

		public static readonly StringName SingleEnemyTargetScale = "SingleEnemyTargetScale";

		public static readonly StringName EnableSleepCycleStress = "EnableSleepCycleStress";

		public static readonly StringName SleepCycleStartAfterSeconds = "SleepCycleStartAfterSeconds";

		public static readonly StringName EnableManyEnemyIdleFixture = "EnableManyEnemyIdleFixture";

		public static readonly StringName ManyEnemyIdleTargetCount = "ManyEnemyIdleTargetCount";

		public static readonly StringName ManyEnemyIdleTargetsOutsideAttackRange = "ManyEnemyIdleTargetsOutsideAttackRange";

		public static readonly StringName WaitForResourceManagerLoad = "WaitForResourceManagerLoad";

		public static readonly StringName KeepRunningWhenUnfocused = "KeepRunningWhenUnfocused";

		public static readonly StringName RuntimeProfile = "RuntimeProfile";

		public static readonly StringName _testRenderBackend = "_testRenderBackend";

		public static readonly StringName _sceneUseCounts = "_sceneUseCounts";

		public static readonly StringName _printTimer = "_printTimer";

		public static readonly StringName _windowDeltaSeconds = "_windowDeltaSeconds";

		public static readonly StringName _sampleStartSeconds = "_sampleStartSeconds";

		public static readonly StringName _windowFrames = "_windowFrames";

		public static readonly StringName _runtimeCrowdFilterDebugPrinted = "_runtimeCrowdFilterDebugPrinted";

		public static readonly StringName _crowdModeReportPrinted = "_crowdModeReportPrinted";

		public static readonly StringName _spawnFailures = "_spawnFailures";

		public static readonly StringName _loadedScenePoolCount = "_loadedScenePoolCount";

		public static readonly StringName _skippedScenePoolCount = "_skippedScenePoolCount";

		public static readonly StringName _excludedScenePoolCount = "_excludedScenePoolCount";

		public static readonly StringName _nextSpawnIndex = "_nextSpawnIndex";

		public static readonly StringName _nextSpawnProgressPrint = "_nextSpawnProgressPrint";

		public static readonly StringName _spawnStartTicks = "_spawnStartTicks";

		public static readonly StringName _loadMs = "_loadMs";

		public static readonly StringName _spawnNoGcRegionActive = "_spawnNoGcRegionActive";

		public static readonly StringName _pendingSpawnFallbackScene = "_pendingSpawnFallbackScene";

		public static readonly StringName _spawnPending = "_spawnPending";

		public static readonly StringName _spawnFrameBudgetPassed = "_spawnFrameBudgetPassed";

		public static readonly StringName _autoQuitPrinted = "_autoQuitPrinted";

		public static readonly StringName _measurementStarted = "_measurementStarted";

		public static readonly StringName _measurementFrames = "_measurementFrames";

		public static readonly StringName _singleEnemyTarget = "_singleEnemyTarget";

		public static readonly StringName _singleEnemyAttackComponentCount = "_singleEnemyAttackComponentCount";

		public static readonly StringName _singleEnemyAttackUtilityComponentCount = "_singleEnemyAttackUtilityComponentCount";

		public static readonly StringName _singleEnemyFireComponentCount = "_singleEnemyFireComponentCount";

		public static readonly StringName _singleEnemyTargetInitialHitpoints = "_singleEnemyTargetInitialHitpoints";

		public static readonly StringName _singleEnemyFixturePrepared = "_singleEnemyFixturePrepared";

		public static readonly StringName _singleEnemyRobotModeAdvanceRequested = "_singleEnemyRobotModeAdvanceRequested";

		public static readonly StringName _singleEnemyRoleUtilityPhaseStarted = "_singleEnemyRoleUtilityPhaseStarted";

		public static readonly StringName _singleEnemyRoleUtilityPhaseCompleted = "_singleEnemyRoleUtilityPhaseCompleted";

		public static readonly StringName _manyEnemyIdleFixturePrepared = "_manyEnemyIdleFixturePrepared";

		public static readonly StringName _nextSingleEnemyRoleUtilityAuditSeconds = "_nextSingleEnemyRoleUtilityAuditSeconds";

		public static readonly StringName _sleepCyclePhase = "_sleepCyclePhase";

		public static readonly StringName _sleepCyclePhaseDeadlineSeconds = "_sleepCyclePhaseDeadlineSeconds";

		public static readonly StringName _sleepCycleCompleted = "_sleepCycleCompleted";

		public static readonly StringName _sleepCyclePassed = "_sleepCyclePassed";

		public static readonly StringName _sleepCycleEligibleCount = "_sleepCycleEligibleCount";

		public static readonly StringName _sleepCycleAppliedCount = "_sleepCycleAppliedCount";

		public static readonly StringName _sleepCycleEnteredCount = "_sleepCycleEnteredCount";

		public static readonly StringName _sleepCycleWokenCount = "_sleepCycleWokenCount";

		public static readonly StringName _sleepCycleNormalizedImmunityCount = "_sleepCycleNormalizedImmunityCount";

		public static readonly StringName _sleepCycleImmunityRestored = "_sleepCycleImmunityRestored";

		public static readonly StringName _sleepCycleHypnotistEventProvider = "_sleepCycleHypnotistEventProvider";

		public static readonly StringName _measurementStartSeconds = "_measurementStartSeconds";

		public static readonly StringName _measurementEndSeconds = "_measurementEndSeconds";

		public static readonly StringName _measurementPreviousFrameTicks = "_measurementPreviousFrameTicks";

		public static readonly StringName _screenshotRequested = "_screenshotRequested";

		public static readonly StringName _screenshotSaved = "_screenshotSaved";

		public static readonly StringName _resultPrinted = "_resultPrinted";

		public static readonly StringName _performanceResultPassed = "_performanceResultPassed";

		public static readonly StringName _animeFrameRateSwitchCompleted = "_animeFrameRateSwitchCompleted";

		public static readonly StringName _animeFrameRateSwitchPassed = "_animeFrameRateSwitchPassed";

		public static readonly StringName _runtimeVisualMutationCompleted = "_runtimeVisualMutationCompleted";

		public static readonly StringName _runtimeVisualMutationPassed = "_runtimeVisualMutationPassed";

		public static readonly StringName _animationSwitchUseSecondClip = "_animationSwitchUseSecondClip";

		public static readonly StringName _nextAnimationSwitchSeconds = "_nextAnimationSwitchSeconds";

		public static readonly StringName _animationSwitchWindowEndSeconds = "_animationSwitchWindowEndSeconds";

		public static readonly StringName _animationSwitchCount = "_animationSwitchCount";

		public static readonly StringName _animationSwitchAppliedCount = "_animationSwitchAppliedCount";

		public static readonly StringName _animationSwitchMissingClipCount = "_animationSwitchMissingClipCount";

		public static readonly StringName _animationSwitchSynchronousTotalMilliseconds = "_animationSwitchSynchronousTotalMilliseconds";

		public static readonly StringName _animationSwitchSynchronousMaxMilliseconds = "_animationSwitchSynchronousMaxMilliseconds";

		public static readonly StringName _nextBowlingImpactSeconds = "_nextBowlingImpactSeconds";

		public static readonly StringName _bowlingImpactWindowEndSeconds = "_bowlingImpactWindowEndSeconds";

		public static readonly StringName _bowlingImpactBurstCount = "_bowlingImpactBurstCount";

		public static readonly StringName _bowlingImpactAppliedCount = "_bowlingImpactAppliedCount";

		public static readonly StringName _bowlingImpactMissingShieldCount = "_bowlingImpactMissingShieldCount";

		public static readonly StringName _bowlingImpactMaxProcessedTweens = "_bowlingImpactMaxProcessedTweens";

		public static readonly StringName _bowlingImpactMaxBatchedMotions = "_bowlingImpactMaxBatchedMotions";

		public static readonly StringName _bowlingImpactMaxBatchedShieldVisuals = "_bowlingImpactMaxBatchedShieldVisuals";

		public static readonly StringName _bowlingImpactSynchronousTotalMilliseconds = "_bowlingImpactSynchronousTotalMilliseconds";

		public static readonly StringName _bowlingImpactSynchronousMaxMilliseconds = "_bowlingImpactSynchronousMaxMilliseconds";

		public static readonly StringName _bowlingImpactThreadAllocatedBytes = "_bowlingImpactThreadAllocatedBytes";

		public static readonly StringName _viewportEntryCaptureFramesRemaining = "_viewportEntryCaptureFramesRemaining";

		public static readonly StringName _viewportEntryCaptureSkipCurrentFrame = "_viewportEntryCaptureSkipCurrentFrame";

		public static readonly StringName _viewportEntryCapturePrinted = "_viewportEntryCapturePrinted";

		public static readonly StringName _measurementThreadAllocatedBefore = "_measurementThreadAllocatedBefore";

		public static readonly StringName _measurementTotalAllocatedBefore = "_measurementTotalAllocatedBefore";

		public static readonly StringName _measurementGen0Before = "_measurementGen0Before";

		public static readonly StringName _measurementGen1Before = "_measurementGen1Before";

		public static readonly StringName _measurementGen2Before = "_measurementGen2Before";

		public static readonly StringName _cadenceSprite = "_cadenceSprite";

		public static readonly StringName _cadencePreviousPhase = "_cadencePreviousPhase";

		public static readonly StringName _cadenceSamples = "_cadenceSamples";

		public static readonly StringName _cadenceAdvances = "_cadenceAdvances";

		public static readonly StringName _cadenceSlot = "_cadenceSlot";

		public static readonly StringName _cadencePreviousSlotVersion = "_cadencePreviousSlotVersion";

		public static readonly StringName _cadenceSlotAdvances = "_cadenceSlotAdvances";

		public static readonly StringName _zInterleaveMarker = "_zInterleaveMarker";

		public static readonly StringName _zInterleaveParticles = "_zInterleaveParticles";

		public static readonly StringName _zInterleaveParticleTexture = "_zInterleaveParticleTexture";

		public static readonly StringName _externalVisualExpectedActive = "_externalVisualExpectedActive";

		public static readonly StringName _externalVisualConfigurationFailures = "_externalVisualConfigurationFailures";

		public static readonly StringName _externalVisualGraphBuildCountAtMeasurementStart = "_externalVisualGraphBuildCountAtMeasurementStart";

		public static readonly StringName _gpuGraphInitialBuildCountAtMeasurementStart = "_gpuGraphInitialBuildCountAtMeasurementStart";

		public static readonly StringName _gpuGraphRebuildCountAtMeasurementStart = "_gpuGraphRebuildCountAtMeasurementStart";

		public static readonly StringName _gpuGraphOwnerExitInvalidationCountAtMeasurementStart = "_gpuGraphOwnerExitInvalidationCountAtMeasurementStart";

		public static readonly StringName _gpuGraphManagedSlotInvalidationCountAtMeasurementStart = "_gpuGraphManagedSlotInvalidationCountAtMeasurementStart";

		public static readonly StringName _gpuGraphExternalVisualInvalidationCountAtMeasurementStart = "_gpuGraphExternalVisualInvalidationCountAtMeasurementStart";

		public static readonly StringName _externalVisualStressTexture = "_externalVisualStressTexture";

		public static readonly StringName _originalMaxFps = "_originalMaxFps";

		public static readonly StringName _originalPhysicsTicksPerSecond = "_originalPhysicsTicksPerSecond";

		public static readonly StringName _invalidTestRenderBackend = "_invalidTestRenderBackend";

		public static readonly StringName _spawnInstantiateMaxMilliseconds = "_spawnInstantiateMaxMilliseconds";

		public static readonly StringName _spawnInstantiateMaxScene = "_spawnInstantiateMaxScene";

		public static readonly StringName _spawnAddChildReadyMaxMilliseconds = "_spawnAddChildReadyMaxMilliseconds";

		public static readonly StringName _spawnAddChildReadyMaxScene = "_spawnAddChildReadyMaxScene";

		public static readonly StringName _adobeFixedAnimationFrame = "_adobeFixedAnimationFrame";

		public static readonly StringName _invalidAdobeFixedAnimationFrame = "_invalidAdobeFixedAnimationFrame";

		public static readonly StringName _renderBackendOption = "_renderBackendOption";

		public static readonly StringName _renderBackendStatus = "_renderBackendStatus";

		public static readonly StringName _renderBackendUiRefreshTimer = "_renderBackendUiRefreshTimer";

		public static readonly StringName _combatStressControl = "_combatStressControl";

		public static readonly StringName _combatStressMapFeature = "_combatStressMapFeature";

		public static readonly StringName _combatStressMapControl = "_combatStressMapControl";

		public static readonly StringName _previousControl = "_previousControl";

		public static readonly StringName _previousGridBegin = "_previousGridBegin";

		public static readonly StringName _previousGridSize = "_previousGridSize";

		public static readonly StringName _previousGridNum = "_previousGridNum";

		public static readonly StringName _previousBackZombie = "_previousBackZombie";

		public static readonly StringName _combatStressConfigured = "_combatStressConfigured";

		public static readonly StringName _resourceManagerLoadGatePending = "_resourceManagerLoadGatePending";

		public static readonly StringName _originalLowProcessorUsageMode = "_originalLowProcessorUsageMode";

		public static readonly StringName _unfocusedRunConfigured = "_unfocusedRunConfigured";
	}

	public new class SignalName : Node2D.SignalName
	{
	}

	private const string DefaultZombieAnimationSceneUid = "uid://d1jj5qhp7tex4";

	private const string DefaultZombieCharacterScenePath = "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn";

	private const string DefaultPlantCombatTargetScenePath = "res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn";

	private const string HypnotistCharacterScenePath = "res://Asset/Anime/Character/Zombie/Chapter5/Hypnotist/Scene/TowerDefenseZombieHypnotist.tscn";

	private const string ExternalVisualStressTexturePath = "res://Asset/Texture/Character/Effect/ButterSplat.png";

	private const double CombatStressDurableHitpoints = 1000000000000.0;

	private const double CombatStressTargetDurableHitpoints = 1000000000000.0;

	private AdobeAnimateRenderBackend _testRenderBackend;

	private readonly List<Node2D> _spawnedNodes = new List<Node2D>();

	private readonly List<TowerDefenseCharacter> _spawnedCharacters = new List<TowerDefenseCharacter>();

	private readonly List<string> _spawnedCharacterScenePaths = new List<string>();

	private readonly List<ProcessModeEnum> _spawnedCharacterProcessModes = new List<ProcessModeEnum>();

	private readonly List<int> _spawnedCharacterSleepBuffFlags = new List<int>();

	private readonly List<AdobeAnimateSpriteBase> _spawnedAnimationSprites = new List<AdobeAnimateSpriteBase>();

	private readonly HashSet<AttackComponent> _engagedAttackComponents = new HashSet<AttackComponent>();

	private readonly HashSet<FireComponent> _engagedFireComponents = new HashSet<FireComponent>();

	private readonly HashSet<AttackComponent> _exercisedAttackUtilityComponents = new HashSet<AttackComponent>();

	private readonly List<(TowerDefenseZombieSoccer Role, AttackComponent Component)> _pendingSoccerAttackUtilityComponents = new List<(TowerDefenseZombieSoccer, AttackComponent)>();

	private readonly List<TowerDefenseCharacter> _manyEnemyIdleTargets = new List<TowerDefenseCharacter>();

	private readonly List<AttackComponent> _manyEnemyIdleAttackComponents = new List<AttackComponent>();

	private readonly List<FireComponent> _manyEnemyIdleFireComponents = new List<FireComponent>();

	private readonly Dictionary<AttackComponent, AttackComponent.AttackEventHandler> _attackEngagementHandlers = new Dictionary<AttackComponent, AttackComponent.AttackEventHandler>();

	private readonly Dictionary<StateHandle, Action> _legacyAttackEngagementHandlers = new Dictionary<StateHandle, Action>();

	private readonly Dictionary<FireComponent, FireComponent.FireVolleyEventHandler> _fireEngagementHandlers = new Dictionary<FireComponent, FireComponent.FireVolleyEventHandler>();

	private readonly List<PackedScene> _scenePool = new List<PackedScene>();

	private readonly List<ExternalVisualStressFixture> _externalVisualStressFixtures = new List<ExternalVisualStressFixture>();

	private readonly List<string> _scenePoolPaths = new List<string>();

	private readonly List<double> _measurementFrameSeconds = new List<double>(131072);

	private readonly List<double> _measurementSimulationDeltaSeconds = new List<double>(131072);

	private readonly List<double> _measurementProcessMilliseconds = new List<double>(131072);

	private readonly List<double> _measurementPhysicsMilliseconds = new List<double>(131072);

	private readonly List<double> _spawnInstantiateMilliseconds = new List<double>(10000);

	private readonly List<double> _spawnAddChildReadyMilliseconds = new List<double>(10000);

	private readonly List<double> _spawnBatchFrameMilliseconds = new List<double>(2048);

	private readonly List<double> _viewportEntryFrameSeconds = new List<double>(128);

	private readonly List<double> _animationSwitchWindowFrameSeconds = new List<double>(4096);

	private readonly List<double> _animationSwitchSteadyFrameSeconds = new List<double>(4096);

	private readonly List<double> _bowlingImpactWindowFrameSeconds = new List<double>(4096);

	private readonly List<double> _bowlingImpactSteadyFrameSeconds = new List<double>(4096);

	private readonly Stopwatch _uptime = new Stopwatch();

	private int[] _sceneUseCounts = Array.Empty<int>();

	private double _printTimer;

	private double _windowDeltaSeconds;

	private double _sampleStartSeconds;

	private int _windowFrames;

	private bool _runtimeCrowdFilterDebugPrinted;

	private bool _crowdModeReportPrinted;

	private int _spawnFailures;

	private int _loadedScenePoolCount;

	private int _skippedScenePoolCount;

	private int _excludedScenePoolCount;

	private int _nextSpawnIndex;

	private int _nextSpawnProgressPrint;

	private long _spawnStartTicks;

	private double _loadMs;

	private bool _spawnNoGcRegionActive;

	private PackedScene _pendingSpawnFallbackScene;

	private bool _spawnPending;

	private bool _spawnFrameBudgetPassed = true;

	private bool _autoQuitPrinted;

	private bool _measurementStarted;

	private int _measurementFrames;

	private TowerDefenseCharacter _singleEnemyTarget;

	private int _singleEnemyAttackComponentCount;

	private int _singleEnemyAttackUtilityComponentCount;

	private int _singleEnemyFireComponentCount;

	private double _singleEnemyTargetInitialHitpoints;

	private bool _singleEnemyFixturePrepared;

	private bool _singleEnemyRobotModeAdvanceRequested;

	private bool _singleEnemyRoleUtilityPhaseStarted;

	private bool _singleEnemyRoleUtilityPhaseCompleted;

	private bool _manyEnemyIdleFixturePrepared;

	private double _nextSingleEnemyRoleUtilityAuditSeconds;

	private int _sleepCyclePhase;

	private double _sleepCyclePhaseDeadlineSeconds;

	private bool _sleepCycleCompleted;

	private bool _sleepCyclePassed;

	private int _sleepCycleEligibleCount;

	private int _sleepCycleAppliedCount;

	private int _sleepCycleEnteredCount;

	private int _sleepCycleWokenCount;

	private int _sleepCycleNormalizedImmunityCount;

	private bool _sleepCycleImmunityRestored;

	private TowerDefenseZombieHypnotist _sleepCycleHypnotistEventProvider;

	private double _measurementStartSeconds;

	private double _measurementEndSeconds = 0.0 / 0.0;

	private long _measurementPreviousFrameTicks;

	private bool _screenshotRequested;

	private bool _screenshotSaved;

	private bool _resultPrinted;

	private bool _performanceResultPassed;

	private bool _animeFrameRateSwitchCompleted;

	private bool _animeFrameRateSwitchPassed = true;

	private bool _runtimeVisualMutationCompleted;

	private bool _runtimeVisualMutationPassed = true;

	private bool _animationSwitchUseSecondClip;

	private double _nextAnimationSwitchSeconds;

	private double _animationSwitchWindowEndSeconds;

	private int _animationSwitchCount;

	private int _animationSwitchAppliedCount;

	private int _animationSwitchMissingClipCount;

	private double _animationSwitchSynchronousTotalMilliseconds;

	private double _animationSwitchSynchronousMaxMilliseconds;

	private double _nextBowlingImpactSeconds;

	private double _bowlingImpactWindowEndSeconds;

	private int _bowlingImpactBurstCount;

	private int _bowlingImpactAppliedCount;

	private int _bowlingImpactMissingShieldCount;

	private int _bowlingImpactMaxProcessedTweens;

	private int _bowlingImpactMaxBatchedMotions;

	private int _bowlingImpactMaxBatchedShieldVisuals;

	private double _bowlingImpactSynchronousTotalMilliseconds;

	private double _bowlingImpactSynchronousMaxMilliseconds;

	private long _bowlingImpactThreadAllocatedBytes;

	private int _viewportEntryCaptureFramesRemaining;

	private bool _viewportEntryCaptureSkipCurrentFrame;

	private bool _viewportEntryCapturePrinted;

	private long _measurementThreadAllocatedBefore;

	private long _measurementTotalAllocatedBefore;

	private int _measurementGen0Before;

	private int _measurementGen1Before;

	private int _measurementGen2Before;

	private AdobeAnimateSpriteBase _cadenceSprite;

	private double _cadencePreviousPhase;

	private int _cadenceSamples;

	private int _cadenceAdvances;

	private AdobeAnimateSlot _cadenceSlot;

	private int _cadencePreviousSlotVersion;

	private int _cadenceSlotAdvances;

	private Polygon2D _zInterleaveMarker;

	private CpuParticles2D _zInterleaveParticles;

	private ImageTexture _zInterleaveParticleTexture;

	private int _externalVisualExpectedActive;

	private int _externalVisualConfigurationFailures;

	private long _externalVisualGraphBuildCountAtMeasurementStart;

	private long _gpuGraphInitialBuildCountAtMeasurementStart;

	private long _gpuGraphRebuildCountAtMeasurementStart;

	private long _gpuGraphOwnerExitInvalidationCountAtMeasurementStart;

	private long _gpuGraphManagedSlotInvalidationCountAtMeasurementStart;

	private long _gpuGraphExternalVisualInvalidationCountAtMeasurementStart;

	private Texture2D _externalVisualStressTexture;

	private int _originalMaxFps;

	private int _originalPhysicsTicksPerSecond;

	private bool _invalidTestRenderBackend;

	private double _spawnInstantiateMaxMilliseconds;

	private string _spawnInstantiateMaxScene = "";

	private double _spawnAddChildReadyMaxMilliseconds;

	private string _spawnAddChildReadyMaxScene = "";

	private int _adobeFixedAnimationFrame = -1;

	private bool _invalidAdobeFixedAnimationFrame;

	private OptionButton _renderBackendOption;

	private Label _renderBackendStatus;

	private double _renderBackendUiRefreshTimer;

	private CharacterStressControlStub _combatStressControl;

	private TowerDefenseBattleFeatureMap _combatStressMapFeature;

	private TowerDefenseMapControl _combatStressMapControl;

	private TowerDefenseControlNew _previousControl;

	private Vector2 _previousGridBegin;

	private Vector2 _previousGridSize;

	private Vector2I _previousGridNum;

	private bool _previousBackZombie;

	private bool _combatStressConfigured;

	private bool _resourceManagerLoadGatePending;

	private bool _originalLowProcessorUsageMode;

	private bool _unfocusedRunConfigured;

	[Export(PropertyHint.None, "")]
	public PackedScene TestScene { get; set; }

	[Export(PropertyHint.None, "")]
	public string ExactScenePath { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public string ExactAnimationClip { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public int SpawnCount { get; set; } = 10000;

	[Export(PropertyHint.None, "")]
	public Vector2 SpawnOrigin { get; set; } = Vector2.Zero;

	[Export(PropertyHint.None, "")]
	public Vector2 SpawnArea { get; set; } = new Vector2(1080f, 600f);

	[Export(PropertyHint.None, "")]
	public bool UseDifferentZombieScenePool { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool UseZombieCharacterScenes { get; set; }

	[Export(PropertyHint.None, "")]
	public string ZombieSceneRoot { get; set; } = "res://Asset/Anime/Character/Zombie";

	[Export(PropertyHint.None, "")]
	public string CharacterSceneFilePrefix { get; set; } = "TowerDefenseZombie";

	[Export(PropertyHint.None, "")]
	public string ExcludedZombieScenePathSubstring { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public int MaxDifferentZombieScenes { get; set; }

	[Export(PropertyHint.None, "")]
	public int SpawnBatchSize { get; set; }

	[Export(PropertyHint.None, "")]
	public double SpawnTimeBudgetMilliseconds { get; set; }

	[Export(PropertyHint.None, "")]
	public bool UseGridSpawnLayout { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public int SpawnGridColumns { get; set; }

	[Export(PropertyHint.None, "")]
	public int RenderZBucketCount { get; set; }

	[Export(PropertyHint.None, "")]
	public float SpawnScale { get; set; } = 1f;

	[Export(PropertyHint.None, "")]
	public ulong SpawnRandomSeed { get; set; } = 10000uL;

	[Export(PropertyHint.None, "")]
	public double AutoQuitAfterSeconds { get; set; }

	[Export(PropertyHint.None, "")]
	public double WarmupSeconds { get; set; } = 2.0;

	[Export(PropertyHint.None, "")]
	public double MinimumAverageFps { get; set; }

	[Export(PropertyHint.None, "")]
	public double MaximumFrameP99Milliseconds { get; set; }

	[Export(PropertyHint.None, "")]
	public double MaximumFrameMilliseconds { get; set; }

	[Export(PropertyHint.None, "")]
	public double MinimumOnePercentLowFps { get; set; }

	[Export(PropertyHint.None, "")]
	public int BenchmarkMaxFps { get; set; } = -1;

	[Export(PropertyHint.None, "")]
	public int BenchmarkPhysicsTicksPerSecond { get; set; }

	[Export(PropertyHint.None, "")]
	public string ScreenshotPath { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public bool WaitForDamagePartSettledBeforeScreenshot { get; set; }

	[Export(PropertyHint.None, "")]
	public float PrintIntervalSeconds { get; set; } = 60f;

	[Export(PropertyHint.None, "")]
	public bool EnableAdobeAnimateProfiler { get; set; }

	[Export(PropertyHint.None, "")]
	public bool EnableDetailedAdobeAnimateProfiler { get; set; }

	[Export(PropertyHint.None, "")]
	public bool EnableAllocationTelemetry { get; set; }

	[Export(PropertyHint.None, "")]
	public int ProfilerDumpIntervalFrames { get; set; } = 120;

	[Export(PropertyHint.None, "")]
	public int ProfilerMaxMetricsPerDump { get; set; } = 96;

	[Export(PropertyHint.None, "")]
	public bool ClearAdobeAnimateRuntimeCachesBeforeRun { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool EnableGpuRenderGraph { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool ForceCpuPoseFallbackForTest { get; set; }

	[Export(PropertyHint.None, "")]
	public int FixedAnimationFrame { get; set; } = -1;

	[Export(PropertyHint.None, "")]
	public double FixedAnimationSubframe { get; set; }

	[Export(PropertyHint.None, "")]
	public bool PrintCrowdFilterDebugData { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public int CrowdFilterDebugSpriteCount { get; set; } = 1;

	[Export(PropertyHint.None, "")]
	public int CrowdFilterDebugMaxFrameSlices { get; set; } = 256;

	[Export(PropertyHint.None, "")]
	public bool PrintCrowdFilterDebugAfterFirstRuntimeFrame { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool PrintCrowdFilterDebugEveryRuntimeSnapshot { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public bool EnableZInterleaveFixture { get; set; }

	[Export(PropertyHint.None, "")]
	public bool PrintCrowdModeReport { get; set; }

	[Export(PropertyHint.None, "")]
	public double InitialAnimeFrameRate { get; set; }

	[Export(PropertyHint.None, "")]
	public double SwitchAnimeFrameRateAfterSeconds { get; set; } = -1.0;

	[Export(PropertyHint.None, "")]
	public double SwitchAnimeFrameRateTo { get; set; }

	[Export(PropertyHint.None, "")]
	public string RuntimeVisualMutation { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public double RuntimeVisualMutationAfterSeconds { get; set; } = -1.0;

	[Export(PropertyHint.None, "")]
	public bool EnableAnimationSwitchStress { get; set; }

	[Export(PropertyHint.None, "")]
	public string AnimationSwitchClipA { get; set; } = "Walk1";

	[Export(PropertyHint.None, "")]
	public string AnimationSwitchClipB { get; set; } = "Eat";

	[Export(PropertyHint.None, "")]
	public double AnimationSwitchIntervalSeconds { get; set; } = 0.75;

	[Export(PropertyHint.None, "")]
	public double AnimationSwitchBlendSeconds { get; set; } = 0.2;

	[Export(PropertyHint.None, "")]
	public double AnimationSwitchStartAfterSeconds { get; set; } = 5.0;

	[Export(PropertyHint.None, "")]
	public bool EnableBowlingImpactTweenStress { get; set; }

	[Export(PropertyHint.None, "")]
	public bool BowlingImpactEnsureShield { get; set; }

	[Export(PropertyHint.None, "")]
	public bool BowlingImpactUseProductionEvent { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public int BowlingImpactProcessedTweenLimit { get; set; } = -1;

	[Export(PropertyHint.None, "")]
	public int BowlingImpactTargetsPerBurst { get; set; }

	[Export(PropertyHint.None, "")]
	public double BowlingImpactDamage { get; set; } = 1.0;

	[Export(PropertyHint.None, "")]
	public double BowlingImpactBurstIntervalSeconds { get; set; } = 0.75;

	[Export(PropertyHint.None, "")]
	public double BowlingImpactActiveWindowSeconds { get; set; } = 0.4;

	[Export(PropertyHint.None, "")]
	public double BowlingImpactStartAfterSeconds { get; set; } = 5.0;

	[Export(PropertyHint.None, "")]
	public bool RunComponentAliveSchedulingRegression { get; set; }

	[Export(PropertyHint.None, "")]
	public bool RunStatePhysicsBatchRateRegression { get; set; }

	[Export(PropertyHint.None, "")]
	public bool RunAnimationCadenceRegression { get; set; }

	[Export(PropertyHint.None, "")]
	public bool EnableExternalVisualStress { get; set; }

	[Export(PropertyHint.None, "")]
	public bool ExternalVisualIceOnly { get; set; }

	[Export(PropertyHint.Range, "0,1,0.01")]
	public double ExternalVisualRatio { get; set; } = 1.0;

	[Export(PropertyHint.None, "")]
	public bool EnableCombatStateStress { get; set; }

	[Export(PropertyHint.None, "")]
	public bool PreserveCombatCharactersInStress { get; set; }

	[Export(PropertyHint.None, "")]
	public bool CombatStressIsNight { get; set; }

	[Export(PropertyHint.None, "")]
	public bool ShowCharacterHealthInStress { get; set; }

	[Export(PropertyHint.None, "")]
	public bool EnableSingleEnemyTargetFixture { get; set; }

	[Export(PropertyHint.None, "")]
	public string SingleEnemyTargetScenePath { get; set; } = "";

	[Export(PropertyHint.None, "")]
	public float SingleEnemyTargetScale { get; set; } = 4f;

	[Export(PropertyHint.None, "")]
	public bool EnableSleepCycleStress { get; set; }

	[Export(PropertyHint.None, "")]
	public double SleepCycleStartAfterSeconds { get; set; } = 2.0;

	[Export(PropertyHint.None, "")]
	public bool EnableManyEnemyIdleFixture { get; set; }

	[Export(PropertyHint.None, "")]
	public int ManyEnemyIdleTargetCount { get; set; } = 1000;

	[Export(PropertyHint.None, "")]
	public bool ManyEnemyIdleTargetsOutsideAttackRange { get; set; }

	[Export(PropertyHint.None, "")]
	public bool WaitForResourceManagerLoad { get; set; }

	[Export(PropertyHint.None, "")]
	public bool KeepRunningWhenUnfocused { get; set; } = true;

	[Export(PropertyHint.None, "")]
	public string RuntimeProfile { get; set; } = "unknown";

	public override void _Ready()
	{
		_originalMaxFps = Engine.MaxFps;
		_originalPhysicsTicksPerSecond = Engine.PhysicsTicksPerSecond;
		_uptime.Restart();
		_spawnedNodes.Clear();
		_spawnedCharacters.Clear();
		_spawnedCharacterScenePaths.Clear();
		_spawnedCharacterProcessModes.Clear();
		_spawnedCharacterSleepBuffFlags.Clear();
		_spawnedAnimationSprites.Clear();
		_manyEnemyIdleTargets.Clear();
		_manyEnemyIdleAttackComponents.Clear();
		_manyEnemyIdleFireComponents.Clear();
		_scenePool.Clear();
		_scenePoolPaths.Clear();
		_externalVisualStressFixtures.Clear();
		_externalVisualStressTexture = null;
		_measurementFrameSeconds.Clear();
		_measurementSimulationDeltaSeconds.Clear();
		_measurementProcessMilliseconds.Clear();
		_measurementPhysicsMilliseconds.Clear();
		_spawnInstantiateMilliseconds.Clear();
		_spawnAddChildReadyMilliseconds.Clear();
		_viewportEntryFrameSeconds.Clear();
		_animationSwitchWindowFrameSeconds.Clear();
		_animationSwitchSteadyFrameSeconds.Clear();
		_bowlingImpactWindowFrameSeconds.Clear();
		_bowlingImpactSteadyFrameSeconds.Clear();
		_sceneUseCounts = Array.Empty<int>();
		_sampleStartSeconds = 0.0;
		_spawnFailures = 0;
		_loadedScenePoolCount = 0;
		_skippedScenePoolCount = 0;
		_excludedScenePoolCount = 0;
		_nextSpawnIndex = 0;
		_nextSpawnProgressPrint = 0;
		_spawnStartTicks = 0L;
		_loadMs = 0.0;
		_pendingSpawnFallbackScene = null;
		_spawnPending = false;
		_spawnFrameBudgetPassed = true;
		_runtimeCrowdFilterDebugPrinted = false;
		_crowdModeReportPrinted = false;
		_autoQuitPrinted = false;
		_measurementStarted = false;
		_measurementFrames = 0;
		_measurementStartSeconds = 0.0;
		_measurementEndSeconds = 0.0 / 0.0;
		_measurementPreviousFrameTicks = 0L;
		_singleEnemyRobotModeAdvanceRequested = false;
		_singleEnemyRoleUtilityPhaseStarted = false;
		_singleEnemyRoleUtilityPhaseCompleted = false;
		_manyEnemyIdleFixturePrepared = false;
		_nextSingleEnemyRoleUtilityAuditSeconds = 0.0;
		_exercisedAttackUtilityComponents.Clear();
		_pendingSoccerAttackUtilityComponents.Clear();
		_screenshotRequested = false;
		_screenshotSaved = false;
		_resultPrinted = false;
		_performanceResultPassed = false;
		_animeFrameRateSwitchCompleted = false;
		_animeFrameRateSwitchPassed = true;
		_runtimeVisualMutationCompleted = false;
		_runtimeVisualMutationPassed = true;
		_animationSwitchUseSecondClip = true;
		_nextAnimationSwitchSeconds = Math.Max(0.0, AnimationSwitchStartAfterSeconds);
		_animationSwitchWindowEndSeconds = -1.0 / 0.0;
		_animationSwitchCount = 0;
		_animationSwitchAppliedCount = 0;
		_animationSwitchMissingClipCount = 0;
		_animationSwitchSynchronousTotalMilliseconds = 0.0;
		_animationSwitchSynchronousMaxMilliseconds = 0.0;
		_nextBowlingImpactSeconds = Math.Max(0.0, BowlingImpactStartAfterSeconds);
		_bowlingImpactWindowEndSeconds = -1.0 / 0.0;
		_bowlingImpactBurstCount = 0;
		_bowlingImpactAppliedCount = 0;
		_bowlingImpactMissingShieldCount = 0;
		_bowlingImpactMaxProcessedTweens = 0;
		_bowlingImpactMaxBatchedMotions = 0;
		_bowlingImpactMaxBatchedShieldVisuals = 0;
		_bowlingImpactSynchronousTotalMilliseconds = 0.0;
		_bowlingImpactSynchronousMaxMilliseconds = 0.0;
		_bowlingImpactThreadAllocatedBytes = 0L;
		_measurementThreadAllocatedBefore = 0L;
		_measurementTotalAllocatedBefore = 0L;
		_measurementGen0Before = 0;
		_measurementGen1Before = 0;
		_measurementGen2Before = 0;
		_cadenceSprite = null;
		_cadencePreviousPhase = 0.0;
		_cadenceSamples = 0;
		_cadenceAdvances = 0;
		_cadenceSlot = null;
		_cadencePreviousSlotVersion = 0;
		_cadenceSlotAdvances = 0;
		_externalVisualExpectedActive = 0;
		_externalVisualConfigurationFailures = 0;
		_externalVisualGraphBuildCountAtMeasurementStart = 0L;
		_gpuGraphInitialBuildCountAtMeasurementStart = 0L;
		_gpuGraphRebuildCountAtMeasurementStart = 0L;
		_gpuGraphOwnerExitInvalidationCountAtMeasurementStart = 0L;
		_gpuGraphManagedSlotInvalidationCountAtMeasurementStart = 0L;
		_gpuGraphExternalVisualInvalidationCountAtMeasurementStart = 0L;
		ApplyCommandLineOverrides();
		ConfigureUnfocusedBenchmarkRun();
		_nextAnimationSwitchSeconds = Math.Max(0.0, AnimationSwitchStartAfterSeconds);
		_nextBowlingImpactSeconds = Math.Max(0.0, BowlingImpactStartAfterSeconds);
		if (_invalidTestRenderBackend || _invalidAdobeFixedAnimationFrame)
		{
			GetTree().Quit(1);
			return;
		}
		Global.Instance.adobeAnimateRenderBackend = _testRenderBackend;
		InitializeRenderBackendUi();
		Viewport viewport = GetViewport();
		if (GodotObject.IsInstanceValid(viewport))
		{
			RenderingServer.ViewportSetMeasureRenderTime(viewport.GetViewportRid(), enable: true);
		}
		TowerDefenseAllocationTelemetry.Enabled = EnableAllocationTelemetry;
		TowerDefenseAllocationTelemetry.Reset();
		if (BenchmarkMaxFps >= 0)
		{
			ProcessPriority = 1000;
			Engine.MaxFps = BenchmarkMaxFps;
		}
		if (BenchmarkPhysicsTicksPerSecond > 0)
		{
			Engine.PhysicsTicksPerSecond = BenchmarkPhysicsTicksPerSecond;
		}
		if (RunComponentAliveSchedulingRegression)
		{
			RunComponentAliveSchedulingRegressionAsync();
		}
		else if (RunStatePhysicsBatchRateRegression)
		{
			RunStatePhysicsBatchRateRegressionAsync();
		}
		else if ((WaitForResourceManagerLoad || EnableSingleEnemyTargetFixture || EnableManyEnemyIdleFixture) && UseZombieCharacterScenes)
		{
			ResourceManager instance = ResourceManager.Instance;
			if (!GodotObject.IsInstanceValid(instance))
			{
				GD.PrintErr("[TestPerf] ResourceManager is unavailable; cannot run a fully initialized character-scene workload.");
				GetTree().Quit(1);
				return;
			}
			_resourceManagerLoadGatePending = true;
			instance.OnLoadOver += OnStressResourceManagerLoadOver;
			GD.Print("[TestPerf] waiting for ResourceManager before loading character scenes.");
			instance.BeginLoad();
		}
		else
		{
			StartStressWorkload();
		}
	}

	private void OnStressResourceManagerLoadOver()
	{
		ResourceManager instance = ResourceManager.Instance;
		if (GodotObject.IsInstanceValid(instance))
		{
			instance.OnLoadOver -= OnStressResourceManagerLoadOver;
		}
		if (_resourceManagerLoadGatePending)
		{
			_resourceManagerLoadGatePending = false;
			if (!GodotObject.IsInstanceValid(instance))
			{
				GD.PrintErr("[TestPerf] ResourceManager became unavailable before the character workload started.");
				GetTree().Quit(1);
				return;
			}
			instance.RequireFullGameplayResourcesReady("OnStressResourceManagerLoadOver");
			GD.Print($"[TestPerf] full gameplay resources ready in {instance.FullGameplayResourceLoadMetrics.FullWallMilliseconds:F3} ms.");
			StartStressWorkload();
		}
	}

	private void StartStressWorkload()
	{
		ConfigureCombatStateStress();
		if (InitialAnimeFrameRate > 0.0 && Global.Instance != null)
		{
			Global.Instance.animeFrameRate = InitialAnimeFrameRate;
		}
		if (EnableZInterleaveFixture)
		{
			SpawnCount = 2;
			SpawnBatchSize = 0;
		}
		AdobeAnimateRenderManager.GpuRenderGraphEnabled = EnableGpuRenderGraph;
		if (ClearAdobeAnimateRuntimeCachesBeforeRun)
		{
			AdobeAnimateDefinitionCache.Clear();
			AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
			GD.Print("[TestPerf] AdobeAnimate runtime caches cleared before load.");
		}
		if (SpawnRandomSeed != 0L)
		{
			GD.Seed(SpawnRandomSeed);
		}
		if (EnableAdobeAnimateProfiler)
		{
			TowerDefensePerfProfiler.Enabled = true;
			TowerDefensePerfProfiler.DetailedHotPathMetrics = EnableDetailedAdobeAnimateProfiler;
			TowerDefensePerfProfiler.DumpIntervalFrames = Math.Max(1, ProfilerDumpIntervalFrames);
			TowerDefensePerfProfiler.MaxMetricsPerDump = Math.Max(1, ProfilerMaxMetricsPerDump);
			TowerDefensePerfProfiler.Reset();
			GD.Print($"[TestPerf] AdobeAnimate profiler enabled detailed={TowerDefensePerfProfiler.DetailedHotPathMetrics} dumpFrames={TowerDefensePerfProfiler.DumpIntervalFrames} maxMetrics={TowerDefensePerfProfiler.MaxMetricsPerDump}");
		}
		long timestamp = Stopwatch.GetTimestamp();
		PackedScene packedScene = null;
		bool flag = !string.IsNullOrWhiteSpace(ExactScenePath);
		if (flag)
		{
			UseDifferentZombieScenePool = false;
			packedScene = GD.Load<PackedScene>(ExactScenePath);
		}
		else if (UseDifferentZombieScenePool)
		{
			LoadDifferentZombieScenePool();
		}
		else
		{
			packedScene = TestScene ?? GD.Load<PackedScene>(UseZombieCharacterScenes ? "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn" : "uid://d1jj5qhp7tex4");
		}
		_loadMs = ElapsedMs(timestamp);
		if (!UseDifferentZombieScenePool && packedScene == null)
		{
			string text;
			if (flag)
			{
				text = ExactScenePath;
			}
			else
			{
				text = (UseZombieCharacterScenes ? "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn" : "uid://d1jj5qhp7tex4");
			}
			GD.PrintErr("[TestPerf] Failed to load test scene: " + text);
			return;
		}
		if (UseDifferentZombieScenePool && _scenePool.Count == 0)
		{
			GD.PrintErr("[TestPerf] Failed to load any zombie " + (UseZombieCharacterScenes ? "character" : "animation") + " scenes under " + ZombieSceneRoot);
			return;
		}
		_spawnedNodes.EnsureCapacity(SpawnCount);
		_spawnedCharacters.EnsureCapacity(SpawnCount);
		_spawnedAnimationSprites.EnsureCapacity(SpawnCount);
		_spawnNoGcRegionActive = TowerDefenseCharacterSpawnBudget.TryBeginSpawnNoGcRegion(SpawnCount);
		_spawnStartTicks = Stopwatch.GetTimestamp();
		if (SpawnCount > 1 && (SpawnTimeBudgetMilliseconds > 0.0 || (SpawnBatchSize > 0 && SpawnCount > SpawnBatchSize)))
		{
			_pendingSpawnFallbackScene = packedScene;
			_spawnPending = true;
			_nextSpawnProgressPrint = Math.Max(Math.Max(1, SpawnBatchSize), Math.Max(1, SpawnCount / 10));
			GD.Print($"[TestPerf] spawn-batch-start count={SpawnCount} batch={SpawnBatchSize} budgetMs={SpawnTimeBudgetMilliseconds:F3} differentScenes={UseDifferentZombieScenePool} scenePool={_scenePool.Count} loadMs={_loadMs:F3}");
		}
		else
		{
			SpawnRange(0, SpawnCount, packedScene);
			CompleteSpawn();
		}
	}

	private void InitializeRenderBackendUi()
	{
		CanvasLayer nodeOrNull = GetNodeOrNull<CanvasLayer>("RenderBackendUi");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			nodeOrNull.Visible = AutoQuitAfterSeconds <= 0.0;
			if (nodeOrNull.Visible)
			{
				_renderBackendOption = GetNode<OptionButton>("%RenderBackendOption");
				_renderBackendStatus = GetNode<Label>("%RenderBackendStatus");
				_renderBackendOption.Clear();
				_renderBackendOption.AddItem(AdobeAnimateRenderBackendPolicy.GetDisplayName(AdobeAnimateRenderBackend.GpuCrowd), 0);
				_renderBackendOption.AddItem(AdobeAnimateRenderBackendPolicy.GetDisplayName(AdobeAnimateRenderBackend.CpuPose), 1);
				_renderBackendOption.Select((int)_testRenderBackend);
				_renderBackendOption.ItemSelected += HandleRenderBackendSelected;
				RefreshRenderBackendUi();
			}
		}
	}

	private void HandleRenderBackendSelected(long selectedIndex)
	{
		if (!GodotObject.IsInstanceValid(_renderBackendOption))
		{
			return;
		}
		int num = (int)selectedIndex;
		if (num < 0 || num >= _renderBackendOption.ItemCount)
		{
			return;
		}
		AdobeAnimateRenderBackend adobeAnimateRenderBackend = AdobeAnimateRenderBackendPolicy.Normalize(_renderBackendOption.GetItemId(num));
		if (_testRenderBackend == adobeAnimateRenderBackend)
		{
			RefreshRenderBackendUi();
			return;
		}
		_testRenderBackend = adobeAnimateRenderBackend;
		if (Global.Instance != null)
		{
			Global.Instance.adobeAnimateRenderBackend = adobeAnimateRenderBackend;
		}
		RestartMeasurementAfterBackendSwitch();
		RefreshRenderBackendUi();
		GD.Print("[TestPerf] render backend switched interactively to " + GetTestRenderBackendLabel() + "; measurement restarted.");
	}

	private void RestartMeasurementAfterBackendSwitch()
	{
		_measurementFrameSeconds.Clear();
		_measurementSimulationDeltaSeconds.Clear();
		_measurementProcessMilliseconds.Clear();
		_measurementPhysicsMilliseconds.Clear();
		_measurementStarted = false;
		_measurementFrames = 0;
		_measurementStartSeconds = 0.0;
		_measurementEndSeconds = 0.0 / 0.0;
		_measurementPreviousFrameTicks = 0L;
		_resultPrinted = false;
		_performanceResultPassed = false;
		_sampleStartSeconds = _uptime.Elapsed.TotalSeconds;
		_windowFrames = 0;
		_windowDeltaSeconds = 0.0;
		_printTimer = 0.0;
		TowerDefenseAllocationTelemetry.Reset();
	}

	private void UpdateRenderBackendUi(double delta)
	{
		if (GodotObject.IsInstanceValid(_renderBackendStatus))
		{
			_renderBackendUiRefreshTimer += delta;
			if (!(_renderBackendUiRefreshTimer < 0.25))
			{
				_renderBackendUiRefreshTimer = 0.0;
				RefreshRenderBackendUi();
			}
		}
	}

	private void RefreshRenderBackendUi()
	{
		if (GodotObject.IsInstanceValid(_renderBackendStatus))
		{
			AdobeAnimateCrowdAggregateStats aggregateRenderStats = AdobeAnimateRenderManager.GetAggregateRenderStats();
			double monitor = Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame);
			int value = ((_testRenderBackend == AdobeAnimateRenderBackend.CpuPose) ? aggregateRenderStats.CpuRoots : (aggregateRenderStats.CrowdRoots + aggregateRenderStats.FallbackRoots));
			_renderBackendStatus.Text = $"当前：{AdobeAnimateRenderBackendPolicy.GetDisplayName(_testRenderBackend)}\nFPS {Engine.GetFramesPerSecond():F1}  Draw Call {monitor:F0}  可见根 {value}";
		}
	}

	private async void RunComponentAliveSchedulingRegressionAsync()
	{
		ComponentAliveSchedulingProbe probe = new ComponentAliveSchedulingProbe();
		AddChild(probe, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		int enabledCount = probe.PhysicsProcessCount;
		bool enabledPassed = enabledCount > 0 && probe.IsPhysicsProcessing();
		probe.alive = false;
		int disabledCount = probe.PhysicsProcessCount;
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		bool disabledPassed = probe.PhysicsProcessCount == disabledCount && !probe.IsPhysicsProcessing();
		probe.alive = true;
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		int physicsProcessCount = probe.PhysicsProcessCount;
		bool flag = physicsProcessCount > disabledCount && probe.IsPhysicsProcessing();
		probe.alive = true;
		bool flag2 = probe.IsPhysicsProcessing();
		probe.alive = false;
		probe.alive = false;
		bool flag3 = !probe.IsPhysicsProcessing();
		bool flag4 = enabledPassed & disabledPassed & flag & flag2 & flag3;
		GD.Print($"[TestComponentAliveScheduling] enabledCount={enabledCount} disabledCount={disabledCount} restoredCount={physicsProcessCount} enabled={enabledPassed} disabled={disabledPassed} restored={flag} repeatEnable={flag2} repeatDisable={flag3} passed={flag4}");
		probe.QueueFree();
		GetTree().Quit((!flag4) ? 2 : 0);
	}

	private async void RunStatePhysicsBatchRateRegressionAsync()
	{
		StateChartState first = new StateChartState
		{
			Name = "BatchRateFirst"
		};
		StateChartState second = new StateChartState
		{
			Name = "BatchRateSecond"
		};
		int firstCount = 0;
		int secondCount = 0;
		double firstDelta = 0.0;
		double secondDelta = 0.0;
		first.OnStatePhysicsProcessing += (double delta) =>
		{
			firstCount++;
			firstDelta += delta;
		};
		second.OnStatePhysicsProcessing += (double delta) =>
		{
			secondCount++;
			secondDelta += delta;
		};
		AddChild(first, forceReadableName: false, InternalMode.Disabled);
		AddChild(second, forceReadableName: false, InternalMode.Disabled);
		first._StateEnter(null);
		second._StateEnter(null);
		for (int i = 0; i < 3; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		firstCount = 0;
		secondCount = 0;
		firstDelta = 0.0;
		secondDelta = 0.0;
		ulong startFrame = Engine.GetPhysicsFrames();
		for (int i = 0; i < 8; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		}
		int num = (int)(Engine.GetPhysicsFrames() - startFrame);
		double num2 = 1.0 / (double)Math.Max(1, Engine.PhysicsTicksPerSecond);
		double num3 = Math.Max(0.0, num - 2) * num2;
		double num4 = (double)(num + 1) * num2;
		bool flag = Math.Abs(firstCount - secondCount) <= 1;
		bool flag2 = firstCount >= num - 1 && firstCount <= num + 1 && secondCount >= num - 1 && secondCount <= num + 1;
		bool flag3 = firstDelta >= num3 && firstDelta <= num4 && secondDelta >= num3 && secondDelta <= num4;
		bool flag4 = flag & flag2 & flag3;
		GD.Print($"[TestStatePhysicsBatchRate] frames={num} counts={firstCount},{secondCount} delta={firstDelta:F6},{secondDelta:F6} balanced={flag} fullRate={flag2} deltaPreserved={flag3} passed={flag4}");
		first._StateExit();
		second._StateExit();
		first.QueueFree();
		second.QueueFree();
		GetTree().Quit((!flag4) ? 2 : 0);
	}

	public override void _ExitTree()
	{
		if (_resourceManagerLoadGatePending && GodotObject.IsInstanceValid(ResourceManager.Instance))
		{
			ResourceManager.Instance.OnLoadOver -= OnStressResourceManagerLoadOver;
		}
		_resourceManagerLoadGatePending = false;
		DetachCombatComponentEngagementHandlers();
		RestoreCombatStateStress();
		Viewport viewport = GetViewport();
		if (GodotObject.IsInstanceValid(viewport))
		{
			RenderingServer.ViewportSetMeasureRenderTime(viewport.GetViewportRid(), enable: false);
		}
		TowerDefenseAllocationTelemetry.Enabled = false;
		if (BenchmarkMaxFps >= 0)
		{
			Engine.MaxFps = _originalMaxFps;
		}
		if (BenchmarkPhysicsTicksPerSecond > 0)
		{
			Engine.PhysicsTicksPerSecond = _originalPhysicsTicksPerSecond;
		}
		if (_unfocusedRunConfigured)
		{
			OS.LowProcessorUsageMode = _originalLowProcessorUsageMode;
		}
		for (int i = 0; i < _externalVisualStressFixtures.Count; i++)
		{
			ExternalVisualStressFixture externalVisualStressFixture = _externalVisualStressFixtures[i];
			if (externalVisualStressFixture.Handle.IsValid && GodotObject.IsInstanceValid(externalVisualStressFixture.Owner))
			{
				externalVisualStressFixture.Owner.UnregisterExternalVisual(externalVisualStressFixture.Handle);
			}
			if (externalVisualStressFixture.OwnsVisual && GodotObject.IsInstanceValid(externalVisualStressFixture.Visual) && !externalVisualStressFixture.Visual.IsQueuedForDeletion())
			{
				externalVisualStressFixture.Visual.QueueFree();
			}
		}
		_externalVisualStressFixtures.Clear();
		_externalVisualStressTexture = null;
		if (GodotObject.IsInstanceValid(_zInterleaveParticles))
		{
			_zInterleaveParticles.Emitting = false;
			_zInterleaveParticles.Texture = null;
			if (!_zInterleaveParticles.IsQueuedForDeletion())
			{
				_zInterleaveParticles.QueueFree();
			}
		}
		if (GodotObject.IsInstanceValid(_zInterleaveMarker) && !_zInterleaveMarker.IsQueuedForDeletion())
		{
			_zInterleaveMarker.QueueFree();
		}
		_zInterleaveParticles = null;
		_zInterleaveMarker = null;
		_zInterleaveParticleTexture?.Dispose();
		_zInterleaveParticleTexture = null;
		if (GodotObject.IsInstanceValid(_sleepCycleHypnotistEventProvider))
		{
			_sleepCycleHypnotistEventProvider.Free();
		}
		_sleepCycleHypnotistEventProvider = null;
	}

	private void CompleteSpawn()
	{
		TowerDefenseCharacterSpawnBudget.EndSpawnNoGcRegion();
		EnsureZInterleaveFixture();
		if (ForceCpuPoseFallbackForTest)
		{
			ForceCpuPoseFallback();
		}
		PrepareAnimationCadenceRegression();
		PrepareSingleEnemyTargetFixture();
		PrepareManyEnemyIdleTargetFixture();
		PrintExactRoleComponentInventory();
		double value = ElapsedMs(_spawnStartTicks);
		GD.Print($"[TestPerf] ready spawn={_spawnedNodes.Count}/{SpawnCount} characters={_spawnedCharacters.Count} animationSprites={_spawnedAnimationSprites.Count} failures={_spawnFailures} differentScenes={UseDifferentZombieScenePool} characterScenes={UseZombieCharacterScenes} scenePool={_scenePool.Count} loaded={_loadedScenePoolCount} skipped={_skippedScenePoolCount} excluded={_excludedScenePoolCount} loadMs={_loadMs:F3} spawnAddChildMs={value:F3} noGc={_spawnNoGcRegionActive} totalMs={_uptime.Elapsed.TotalMilliseconds:F3}");
		PrintScenePoolSnapshot("ready");
		PrintRuntimeSnapshot("ready");
		PrintCrowdFilterDebugSnapshot("ready");
		if (EnableAdobeAnimateProfiler)
		{
			Callable.From(PrintScheduledNodeTypeSnapshot).CallDeferred();
		}
		_sampleStartSeconds = _uptime.Elapsed.TotalSeconds;
	}

	private void PrepareAnimationCadenceRegression()
	{
		if (!RunAnimationCadenceRegression)
		{
			return;
		}
		for (int i = 0; i < _spawnedAnimationSprites.Count; i++)
		{
			AdobeAnimateSpriteBase adobeAnimateSpriteBase = _spawnedAnimationSprites[i];
			if (GodotObject.IsInstanceValid(adobeAnimateSpriteBase))
			{
				_cadenceSprite = adobeAnimateSpriteBase;
				_cadenceSlot = FindCadenceSlot(adobeAnimateSpriteBase);
				if (GodotObject.IsInstanceValid(_cadenceSlot) && !_cadenceSlot.RequiresRuntimeUpdate)
				{
					_cadenceSlot.AddChild(new Node2D
					{
						Name = "CadenceRuntimeDependency"
					}, forceReadableName: false, InternalMode.Disabled);
					_cadenceSlot.RefreshRuntimeUpdateRequirement(refreshVisibilityWatchers: true);
				}
				if (!GodotObject.IsInstanceValid(_cadenceSlot))
				{
					_cadenceSprite.refreshEveryFrame = true;
				}
				_cadencePreviousPhase = (double)_cadenceSprite.frameIndex + _cadenceSprite.elapsedTimer;
				_cadencePreviousSlotVersion = (GodotObject.IsInstanceValid(_cadenceSlot) ? _cadenceSlot.RuntimePositionVersion : 0);
				GD.Print($"[TestAnimationCadence] prepared sprite={_cadenceSprite.Name} slot={_cadenceSlot?.Name.ToString() ?? "<forced-refresh>"} trueFrameRate={_cadenceSprite.trueFrameRate:F2}");
				break;
			}
		}
	}

	private static AdobeAnimateSlot FindCadenceSlot(Node parent)
	{
		foreach (Node child in parent.GetChildren())
		{
			if (child is AdobeAnimateSlot result)
			{
				return result;
			}
			AdobeAnimateSlot adobeAnimateSlot = FindCadenceSlot(child);
			if (GodotObject.IsInstanceValid(adobeAnimateSlot))
			{
				return adobeAnimateSlot;
			}
		}
		return null;
	}

	private void SampleAnimationCadence()
	{
		if (!RunAnimationCadenceRegression || !GodotObject.IsInstanceValid(_cadenceSprite))
		{
			return;
		}
		double num = (double)_cadenceSprite.frameIndex + _cadenceSprite.elapsedTimer;
		_cadenceSamples++;
		if (Math.Abs(num - _cadencePreviousPhase) > 1E-06)
		{
			_cadenceAdvances++;
		}
		_cadencePreviousPhase = num;
		if (GodotObject.IsInstanceValid(_cadenceSlot))
		{
			int runtimePositionVersion = _cadenceSlot.RuntimePositionVersion;
			if (runtimePositionVersion != _cadencePreviousSlotVersion)
			{
				_cadenceSlotAdvances++;
			}
			_cadencePreviousSlotVersion = runtimePositionVersion;
		}
	}

	private bool IsAnimationCadencePassed()
	{
		if (!RunAnimationCadenceRegression)
		{
			return true;
		}
		bool num = _cadenceSamples >= 30 && (double)_cadenceAdvances >= (double)_cadenceSamples * 0.8;
		bool flag = !GodotObject.IsInstanceValid(_cadenceSlot) || (double)_cadenceSlotAdvances >= (double)_cadenceSamples * 0.8;
		return num & flag;
	}

	private void ForceCpuPoseFallback()
	{
		HashSet<AdobeAnimateRuntimeDefinition> hashSet = new HashSet<AdobeAnimateRuntimeDefinition>();
		int num = 0;
		for (int i = 0; i < _spawnedAnimationSprites.Count; i++)
		{
			AdobeAnimateSpriteBase adobeAnimateSpriteBase = _spawnedAnimationSprites[i];
			if (!GodotObject.IsInstanceValid(adobeAnimateSpriteBase) || adobeAnimateSpriteBase.flashAnimeData == null)
			{
				continue;
			}
			AdobeAnimateRuntimeDefinition orBuild = AdobeAnimateDefinitionCache.GetOrBuild(adobeAnimateSpriteBase.flashAnimeData);
			if (orBuild != null && hashSet.Add(orBuild))
			{
				orBuild.GpuPoseTexture = null;
				orBuild.GpuPoseTextureArray = null;
				orBuild.GpuPoseTextureRid = default;
				orBuild.GpuPoseTextureSize = Vector2I.Zero;
				orBuild.UsesGpuPoseTextureArray = false;
				if (AdobeAnimateDefinitionCache.EnsureCpuPoseData(orBuild))
				{
					num += orBuild.Slices.Length;
				}
			}
		}
		AdobeAnimateRenderManager.ClearGpuRenderGraphCaches();
		GD.Print($"[TestCpuPoseFallback] definitions={hashSet.Count} poses={num} enabled={hashSet.Count > 0 && num > 0}");
	}

	private void PrintScheduledNodeTypeSnapshot()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.Ordinal);
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>(StringComparer.Ordinal);
		Dictionary<string, int> dictionary3 = new Dictionary<string, int>(StringComparer.Ordinal);
		Stack<Node> stack = new Stack<Node>();
		stack.Push(this);
		while (stack.Count > 0)
		{
			Node node = stack.Pop();
			string name = node.GetType().Name;
			if (node.IsProcessing())
			{
				dictionary[name] = dictionary.GetValueOrDefault(name) + 1;
			}
			if (node.IsPhysicsProcessing())
			{
				dictionary2[name] = dictionary2.GetValueOrDefault(name) + 1;
			}
			if (node is ComponentBase { IsSharedBatchDispatchActive: not false })
			{
				dictionary3[name] = dictionary3.GetValueOrDefault(name) + 1;
			}
			foreach (Node child in node.GetChildren())
			{
				stack.Push(child);
			}
		}
		PrintScheduledNodeTypeCounts("process", dictionary);
		PrintScheduledNodeTypeCounts("physics", dictionary2);
		PrintScheduledNodeTypeCounts("component-batch", dictionary3);
	}

	private static void PrintScheduledNodeTypeCounts(string kind, Dictionary<string, int> counts)
	{
		List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>(counts);
		list.Sort((KeyValuePair<string, int> left, KeyValuePair<string, int> right) => right.Value.CompareTo(left.Value));
		int num = 0;
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			num += list[num2].Value;
		}
		int num3 = Math.Min(24, list.Count);
		string[] array = new string[num3];
		for (int num4 = 0; num4 < num3; num4++)
		{
			array[num4] = $"{list[num4].Key}:{list[num4].Value}";
		}
		GD.Print($"[TestScheduledNodes] kind={kind} total={num} top={string.Join(',', array)}");
	}

	public override void _Process(double delta)
	{
		if (KeepRunningWhenUnfocused)
		{
			if (OS.LowProcessorUsageMode)
			{
				OS.LowProcessorUsageMode = false;
			}
			SceneTree tree = GetTree();
			if (GodotObject.IsInstanceValid(tree) && tree.Paused)
			{
				tree.Paused = false;
			}
		}
		if (BenchmarkMaxFps >= 0 && Engine.MaxFps != BenchmarkMaxFps)
		{
			Engine.MaxFps = BenchmarkMaxFps;
		}
		if (BenchmarkPhysicsTicksPerSecond > 0 && Engine.PhysicsTicksPerSecond != BenchmarkPhysicsTicksPerSecond)
		{
			Engine.PhysicsTicksPerSecond = BenchmarkPhysicsTicksPerSecond;
		}
		UpdateRenderBackendUi(delta);
		EnforceFixedAnimationFrame();
		if (_resourceManagerLoadGatePending)
		{
			return;
		}
		if (_spawnPending)
		{
			SpawnPendingBatch();
			return;
		}
		_windowFrames++;
		_windowDeltaSeconds += delta;
		_printTimer += delta;
		double num = _uptime.Elapsed.TotalSeconds - _sampleStartSeconds;
		TrySwitchAnimeFrameRate(num);
		TryApplyRuntimeVisualMutation(num);
		TryRunAnimationSwitchStress(num);
		TryRunBowlingImpactTweenStress(num);
		TryAdvanceSingleEnemyRoleAttackModes();
		TryDriveSingleEnemyRoleAttackUtilities();
		TryRunSleepCycleStress(num);
		CaptureViewportEntryFrame(delta);
		if (PrintCrowdModeReport && !_crowdModeReportPrinted && num >= 0.5)
		{
			PrintNonGpuGraphCrowdModes();
			_crowdModeReportPrinted = true;
		}
		if (num >= WarmupSeconds)
		{
			bool flag = false;
			if (!_measurementStarted)
			{
				StartMeasurementTelemetry();
				flag = true;
			}
			if (!flag)
			{
				long timestamp = Stopwatch.GetTimestamp();
				double num2 = (double)(timestamp - _measurementPreviousFrameTicks) / (double)Stopwatch.Frequency;
				_measurementPreviousFrameTicks = timestamp;
				EnforceAdobeFixedAnimationFrame();
				_measurementFrames++;
				_measurementFrameSeconds.Add(Math.Max(0.0, num2));
				_measurementSimulationDeltaSeconds.Add(delta);
				if (EnableAnimationSwitchStress)
				{
					if (num <= _animationSwitchWindowEndSeconds)
					{
						_animationSwitchWindowFrameSeconds.Add(num2);
					}
					else
					{
						_animationSwitchSteadyFrameSeconds.Add(num2);
					}
				}
				if (EnableBowlingImpactTweenStress)
				{
					if (num <= _bowlingImpactWindowEndSeconds)
					{
						_bowlingImpactWindowFrameSeconds.Add(num2);
					}
					else
					{
						_bowlingImpactSteadyFrameSeconds.Add(num2);
					}
				}
				_measurementProcessMilliseconds.Add(Performance.GetMonitor(Performance.Monitor.TimeProcess) * 1000.0);
				_measurementPhysicsMilliseconds.Add(Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess) * 1000.0);
				SampleAnimationCadence();
				TryCapturePerformanceScreenshot();
			}
		}
		if (AutoQuitAfterSeconds > 0.0 && num >= AutoQuitAfterSeconds)
		{
			if (!string.IsNullOrWhiteSpace(ScreenshotPath) && !_screenshotSaved)
			{
				TryCapturePerformanceScreenshot();
				if (!_screenshotSaved)
				{
					return;
				}
			}
			if (_measurementStarted && double.IsNaN(_measurementEndSeconds))
			{
				_measurementEndSeconds = _uptime.Elapsed.TotalSeconds;
			}
			if (!_autoQuitPrinted)
			{
				PrintRuntimeSnapshot("final");
				PrintScenePoolSnapshot("final");
				PrintPerformanceResult();
				_autoQuitPrinted = true;
			}
			bool flag2 = SwitchAnimeFrameRateAfterSeconds >= 0.0 && SwitchAnimeFrameRateTo > 0.0;
			bool flag3 = _performanceResultPassed && _animeFrameRateSwitchPassed && _runtimeVisualMutationPassed && (string.IsNullOrWhiteSpace(RuntimeVisualMutation) || _runtimeVisualMutationCompleted) && (!flag2 || _animeFrameRateSwitchCompleted);
			GetTree().Quit((!flag3) ? 2 : 0);
			return;
		}
		if (PrintCrowdFilterDebugAfterFirstRuntimeFrame && !_runtimeCrowdFilterDebugPrinted)
		{
			PrintCrowdFilterDebugSnapshot("runtime-first-frame");
			_runtimeCrowdFilterDebugPrinted = true;
		}
		if (!(_printTimer < (double)PrintIntervalSeconds))
		{
			PrintRuntimeSnapshot("runtime");
			if (PrintCrowdFilterDebugEveryRuntimeSnapshot)
			{
				PrintCrowdFilterDebugSnapshot("runtime");
			}
			_printTimer = 0.0;
			_windowFrames = 0;
			_windowDeltaSeconds = 0.0;
		}
	}

	private void StartMeasurementTelemetry()
	{
		_measurementStarted = true;
		_measurementStartSeconds = _uptime.Elapsed.TotalSeconds;
		_measurementPreviousFrameTicks = Stopwatch.GetTimestamp();
		_measurementThreadAllocatedBefore = GC.GetAllocatedBytesForCurrentThread();
		_measurementTotalAllocatedBefore = GC.GetTotalAllocatedBytes();
		_measurementGen0Before = GC.CollectionCount(0);
		_measurementGen1Before = GC.CollectionCount(1);
		_measurementGen2Before = GC.CollectionCount(2);
		_externalVisualGraphBuildCountAtMeasurementStart = AdobeAnimateRenderManager.GpuRenderGraphBuildCount;
		_gpuGraphInitialBuildCountAtMeasurementStart = AdobeAnimateRenderManager.GpuRenderGraphInitialBuildCount;
		_gpuGraphRebuildCountAtMeasurementStart = AdobeAnimateRenderManager.GpuRenderGraphRebuildCount;
		_gpuGraphOwnerExitInvalidationCountAtMeasurementStart = AdobeAnimateRenderManager.GpuRenderGraphOwnerExitInvalidationCount;
		_gpuGraphManagedSlotInvalidationCountAtMeasurementStart = AdobeAnimateRenderManager.GpuRenderGraphManagedSlotInvalidationCount;
		_gpuGraphExternalVisualInvalidationCountAtMeasurementStart = AdobeAnimateRenderManager.GpuRenderGraphExternalVisualInvalidationCount;
		TowerDefenseAllocationTelemetry.Reset();
	}

	private bool HasValidMeasurementTelemetry()
	{
		if (_measurementStarted && _measurementFrames > 0 && _measurementFrameSeconds.Count == _measurementFrames && _measurementSimulationDeltaSeconds.Count == _measurementFrames && _measurementProcessMilliseconds.Count == _measurementFrames)
		{
			return _measurementPhysicsMilliseconds.Count == _measurementFrames;
		}
		return false;
	}

	private void TrySwitchAnimeFrameRate(double sampleElapsed)
	{
		if (_animeFrameRateSwitchCompleted || SwitchAnimeFrameRateAfterSeconds < 0.0 || SwitchAnimeFrameRateTo <= 0.0 || sampleElapsed < SwitchAnimeFrameRateAfterSeconds)
		{
			return;
		}
		_animeFrameRateSwitchCompleted = true;
		if (Global.Instance == null)
		{
			_animeFrameRateSwitchPassed = false;
			GD.PrintErr("[TestTrueFrameRate] failed reason=Global.Instance-null");
			return;
		}
		double animeFrameRate = Global.Instance.animeFrameRate;
		double[] array = new double[_spawnedAnimationSprites.Count];
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < _spawnedAnimationSprites.Count; i++)
		{
			AdobeAnimateSpriteBase adobeAnimateSpriteBase = _spawnedAnimationSprites[i];
			if (GodotObject.IsInstanceValid(adobeAnimateSpriteBase))
			{
				array[i] = (double)adobeAnimateSpriteBase.frameIndex + adobeAnimateSpriteBase.elapsedTimer;
				if (adobeAnimateSpriteBase.UsesRuntimeGpuClockInterpolation)
				{
					num++;
				}
				if (adobeAnimateSpriteBase.NeedsRuntimeRenderSubmission)
				{
					num2++;
				}
			}
		}
		Global.Instance.animeFrameRate = SwitchAnimeFrameRateTo;
		double trueAnimeFrameRate = Global.Instance.trueAnimeFrameRate;
		double num3 = 0.0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		for (int j = 0; j < _spawnedAnimationSprites.Count; j++)
		{
			AdobeAnimateSpriteBase adobeAnimateSpriteBase2 = _spawnedAnimationSprites[j];
			if (GodotObject.IsInstanceValid(adobeAnimateSpriteBase2))
			{
				double num7 = (double)adobeAnimateSpriteBase2.frameIndex + adobeAnimateSpriteBase2.elapsedTimer;
				num3 = Math.Max(num3, Math.Abs(num7 - array[j]));
				if (!Mathf.IsEqualApprox((float)adobeAnimateSpriteBase2.trueFrameRate, (float)trueAnimeFrameRate))
				{
					num4++;
				}
				if (adobeAnimateSpriteBase2.UsesRuntimeGpuClockInterpolation)
				{
					num5++;
				}
				if (adobeAnimateSpriteBase2.NeedsRuntimeRenderSubmission)
				{
					num6++;
				}
			}
		}
		_animeFrameRateSwitchPassed = num3 <= 1E-06 && num4 == 0 && num == num5 && num2 == num6 && Engine.MaxFps == Global.Instance.effectiveAnimeFrameRate;
		GD.Print($"[TestTrueFrameRate] old={animeFrameRate:F2} new={Global.Instance.animeFrameRate:F2} true={trueAnimeFrameRate:F2} phaseDelta={num3:F6} wrongTrueRate={num4} gpuClock={num}->{num5} dirty={num2}->{num6} maxFps={Engine.MaxFps} passed={_animeFrameRateSwitchPassed}");
	}

	private void TryRunAnimationSwitchStress(double sampleElapsed)
	{
		if (!EnableAnimationSwitchStress || AnimationSwitchIntervalSeconds <= 0.0 || _sampleStartSeconds <= 0.0 || sampleElapsed < AnimationSwitchStartAfterSeconds || sampleElapsed + 1E-06 < _nextAnimationSwitchSeconds)
		{
			return;
		}
		_nextAnimationSwitchSeconds = sampleElapsed + AnimationSwitchIntervalSeconds;
		string text = (_animationSwitchUseSecondClip ? AnimationSwitchClipB : AnimationSwitchClipA);
		_animationSwitchUseSecondClip = !_animationSwitchUseSecondClip;
		long timestamp = Stopwatch.GetTimestamp();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < _spawnedCharacters.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && GodotObject.IsInstanceValid(towerDefenseCharacter.sprite))
			{
				num++;
				if (!towerDefenseCharacter.sprite.HasClip(text))
				{
					num3++;
					continue;
				}
				towerDefenseCharacter.sprite.SetAnimation(text, loop: true, Math.Max(0.0, AnimationSwitchBlendSeconds));
				num2++;
			}
		}
		double num4 = ElapsedMs(timestamp);
		_animationSwitchCount++;
		_animationSwitchAppliedCount += num2;
		_animationSwitchMissingClipCount += num3;
		_animationSwitchSynchronousTotalMilliseconds += num4;
		_animationSwitchSynchronousMaxMilliseconds = Math.Max(_animationSwitchSynchronousMaxMilliseconds, num4);
		_animationSwitchWindowEndSeconds = Math.Max(_animationSwitchWindowEndSeconds, sampleElapsed + Math.Max(0.05, AnimationSwitchBlendSeconds + 0.05));
		GD.Print($"[TestAnimationSwitch] sequence={_animationSwitchCount} target={text} blend={AnimationSwitchBlendSeconds:F3} candidates={num} applied={num2} missingClip={num3} syncMs={num4:F3}");
	}

	private void TryRunBowlingImpactTweenStress(double sampleElapsed)
	{
		if (!EnableBowlingImpactTweenStress || BowlingImpactBurstIntervalSeconds <= 0.0 || _sampleStartSeconds <= 0.0 || sampleElapsed < BowlingImpactStartAfterSeconds || sampleElapsed + 1E-06 < _nextBowlingImpactSeconds)
		{
			return;
		}
		_nextBowlingImpactSeconds = sampleElapsed + BowlingImpactBurstIntervalSeconds;
		int num = ((BowlingImpactTargetsPerBurst <= 0) ? _spawnedCharacters.Count : Math.Min(BowlingImpactTargetsPerBurst, _spawnedCharacters.Count));
		long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
		long timestamp = Stopwatch.GetTimestamp();
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < num; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter) || !GodotObject.IsInstanceValid(towerDefenseCharacter.instance))
			{
				continue;
			}
			if (BowlingImpactEnsureShield && !towerDefenseCharacter.HasShield())
			{
				num3++;
			}
			Vector2 pos = towerDefenseCharacter.GlobalPosition + new Vector2(-1f, 0f);
			if (BowlingImpactUseProductionEvent)
			{
				TowerDefenseCharacterEventBowlingHurt.Run(pos, towerDefenseCharacter, Math.Max(0.0, BowlingImpactDamage));
			}
			else
			{
				double num4 = Math.Max(0.0, BowlingImpactDamage);
				bool flag = towerDefenseCharacter.HasShield();
				if (flag)
				{
					num4 *= 2.0 / 9.0;
				}
				Vector2 velocity = new Vector2((float)((double)(towerDefenseCharacter.GlobalPosition.X - pos.X) * GD.RandRange(3.0, 6.0)), -200f);
				towerDefenseCharacter.BowlingHurt(num4, playSplatAudio: true, velocity, flag);
			}
			num2++;
		}
		double num5 = ElapsedMs(timestamp);
		long num6 = Math.Max(0L, GC.GetAllocatedBytesForCurrentThread() - allocatedBytesForCurrentThread);
		int num7 = CountProcessedTweens();
		int num8 = CountBowlingImpactMotions();
		int activeCount = TowerDefenseShieldImpactBatch.ActiveCount;
		_bowlingImpactBurstCount++;
		_bowlingImpactAppliedCount += num2;
		_bowlingImpactMissingShieldCount += num3;
		_bowlingImpactMaxProcessedTweens = Math.Max(_bowlingImpactMaxProcessedTweens, num7);
		_bowlingImpactMaxBatchedMotions = Math.Max(_bowlingImpactMaxBatchedMotions, num8);
		_bowlingImpactMaxBatchedShieldVisuals = Math.Max(_bowlingImpactMaxBatchedShieldVisuals, activeCount);
		_bowlingImpactSynchronousTotalMilliseconds += num5;
		_bowlingImpactSynchronousMaxMilliseconds = Math.Max(_bowlingImpactSynchronousMaxMilliseconds, num5);
		_bowlingImpactThreadAllocatedBytes += num6;
		_bowlingImpactWindowEndSeconds = Math.Max(_bowlingImpactWindowEndSeconds, sampleElapsed + Math.Max(0.05, BowlingImpactActiveWindowSeconds));
		GD.Print($"[TestBowlingImpactTween] burst={_bowlingImpactBurstCount} targets={num} applied={num2} missingShield={num3} productionEvent={BowlingImpactUseProductionEvent} damage={BowlingImpactDamage:F3} syncMs={num5:F3} threadAllocatedBytes={num6} processedTweens={num7} batchedMotions={num8} batchedShieldVisuals={activeCount}");
	}

	private int CountProcessedTweens()
	{
		SceneTree tree = GetTree();
		if (!GodotObject.IsInstanceValid(tree))
		{
			return 0;
		}
		return tree.GetProcessedTweens().Count;
	}

	private int CountBowlingImpactMotions()
	{
		int num = 0;
		for (int i = 0; i < _spawnedCharacters.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && towerDefenseCharacter.IsBowlingImpactDisplacementActive)
			{
				num++;
			}
		}
		return num;
	}

	private void TryApplyRuntimeVisualMutation(double sampleElapsed)
	{
		if (_runtimeVisualMutationCompleted || string.IsNullOrWhiteSpace(RuntimeVisualMutation) || RuntimeVisualMutationAfterSeconds < 0.0 || sampleElapsed < RuntimeVisualMutationAfterSeconds || TryApplyDamagePartMutation() || TryApplyViewportEntryMutation())
		{
			return;
		}
		ulong num = 0uL;
		ulong num2 = 0uL;
		int num3 = 0;
		for (int i = 0; i < _spawnedNodes.Count; i++)
		{
			if (_spawnedNodes[i] is ZombieBoss zombieBoss && GodotObject.IsInstanceValid(zombieBoss))
			{
				if (AdobeAnimateRenderManager.TryResolveGpuRenderGraph(zombieBoss, out var graph, out var graphOwners, out var allocation, out var textureArray, out var textureSize))
				{
					num = graph?.Signature ?? 0;
				}
				switch (RuntimeVisualMutation.Trim().ToLowerInvariant())
				{
				case "boss-stage1":
					zombieBoss.DamagePointSet("Stage1");
					break;
				case "boss-stage2":
					zombieBoss.DamagePointSet("Stage2");
					break;
				case "boss-dynamic":
					zombieBoss.DamagePointSet("Stage1");
					zombieBoss.SetHeadAttackBall(isFire: true);
					zombieBoss.SetRVVisible(visible: false);
					break;
				default:
					_runtimeVisualMutationPassed = false;
					_runtimeVisualMutationCompleted = true;
					GD.PrintErr("[TestRuntimeVisualMutation] unknown=" + RuntimeVisualMutation);
					return;
				}
				num3++;
				if (AdobeAnimateRenderManager.TryResolveGpuRenderGraph(zombieBoss, out var graph2, out graphOwners, out allocation, out textureArray, out textureSize))
				{
					num2 = graph2?.Signature ?? 0;
				}
			}
		}
		_runtimeVisualMutationCompleted = true;
		bool flag = num != 0L && num == num2;
		_runtimeVisualMutationPassed = num3 > 0 && (!EnableGpuRenderGraph | flag);
		GD.Print($"[TestRuntimeVisualMutation] mutation={RuntimeVisualMutation} applied={num3} graph=0x{num:X16}->0x{num2:X16} sharedGraphPreserved={flag} passed={_runtimeVisualMutationPassed}");
		if (PrintCrowdModeReport)
		{
			PrintNonGpuGraphCrowdModes();
		}
	}

	private bool TryApplyViewportEntryMutation()
	{
		if (!string.Equals(RuntimeVisualMutation.Trim(), "viewport-entry", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		Vector2 size = GetViewportRect().Size;
		Vector2 vector = new Vector2(Mathf.Max(40f, size.X * 0.08f), Mathf.Max(40f, size.Y * 0.18f));
		Vector2 vector2 = vector - SpawnOrigin;
		int num = 0;
		for (int i = 0; i < _spawnedNodes.Count; i++)
		{
			Node2D node2D = _spawnedNodes[i];
			if (GodotObject.IsInstanceValid(node2D))
			{
				node2D.GlobalPosition += vector2;
				num++;
			}
		}
		_runtimeVisualMutationCompleted = true;
		_runtimeVisualMutationPassed = num > 0;
		_viewportEntryFrameSeconds.Clear();
		_viewportEntryCaptureFramesRemaining = 60;
		_viewportEntryCaptureSkipCurrentFrame = true;
		_viewportEntryCapturePrinted = false;
		GD.Print($"[TestViewportEntry] applied={num} translation={vector2} targetOrigin={vector} passed={_runtimeVisualMutationPassed}");
		return true;
	}

	private void CaptureViewportEntryFrame(double delta)
	{
		if (_viewportEntryCaptureFramesRemaining <= 0)
		{
			return;
		}
		if (_viewportEntryCaptureSkipCurrentFrame)
		{
			_viewportEntryCaptureSkipCurrentFrame = false;
			return;
		}
		_viewportEntryFrameSeconds.Add(delta);
		_viewportEntryCaptureFramesRemaining--;
		if (_viewportEntryCaptureFramesRemaining == 0)
		{
			PrintViewportEntryPerformance();
		}
	}

	private void PrintViewportEntryPerformance()
	{
		if (!_viewportEntryCapturePrinted && _viewportEntryFrameSeconds.Count != 0)
		{
			_viewportEntryCapturePrinted = true;
			double[] array = _viewportEntryFrameSeconds.ToArray();
			double value = BenchmarkStatistics.Percentile(array, array.Length, 95.0) * 1000.0;
			double value2 = BenchmarkStatistics.Maximum(array, array.Length) * 1000.0;
			GD.Print($"[TestViewportEntryPerf] frames={array.Length} frameP95Ms={value:F3} frameMaxMs={value2:F3}");
		}
	}

	private bool TryApplyDamagePartMutation()
	{
		string text = RuntimeVisualMutation.Trim();
		bool flag = text.StartsWith("armor-drop:", StringComparison.OrdinalIgnoreCase);
		if (!flag && !text.StartsWith("damage-part:", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		string text2 = text;
		int length = (flag ? "armor-drop:" : "damage-part:").Length;
		string text3 = text2.Substring(length, text2.Length - length).Trim();
		int num = 0;
		if (!string.IsNullOrEmpty(text3))
		{
			for (int i = 0; i < _spawnedCharacters.Count; i++)
			{
				TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
				if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
				{
					continue;
				}
				int previousCount = CountActiveDamagePartNodes();
				if (flag)
				{
					bool flag2 = GodotObject.IsInstanceValid(towerDefenseCharacter.instance) && towerDefenseCharacter.instance.ArmorHas(text3);
					GD.Print($"[TestArmorDropInitial] armor={text3} present={flag2} currentArmor={towerDefenseCharacter.currentArmor.Contains(text3)}");
					if (!flag2)
					{
						GD.PushError("[TestArmorDropInitial] 角色生成后没有创建逻辑防具：" + text3);
						continue;
					}
					towerDefenseCharacter.instance.ArmorDelete(text3);
					if (towerDefenseCharacter.instance.ArmorHas(text3))
					{
						GD.PushError("[TestArmorDropRemoval] 防具删除后仍存在逻辑实例：" + text3);
						continue;
					}
				}
				else
				{
					towerDefenseCharacter.DamagePartCreate(new StringName(text3), null, new Vector2(-220f, -250f), keepSlotScale: true, default, fromSync: false, null, 0L);
				}
				num += ValidateNewDamagePartNode(towerDefenseCharacter, previousCount);
			}
		}
		_runtimeVisualMutationCompleted = true;
		_runtimeVisualMutationPassed = num > 0;
		GD.Print($"[TestDamagePartMutation] mutation={RuntimeVisualMutation} applied={num} passed={_runtimeVisualMutationPassed}");
		return true;
	}

	private static int ValidateNewDamagePartNode(TowerDefenseCharacter character, int previousCount)
	{
		if (!GodotObject.IsInstanceValid(character))
		{
			return 0;
		}
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		int num = CountActiveDamagePartNodes();
		if (!GodotObject.IsInstanceValid(characterNode) || num <= previousCount)
		{
			return 0;
		}
		int value = (GodotObject.IsInstanceValid(character.sprite) ? character.sprite.GetEffectOnceBatchEffectiveZIndex() : character.ZIndex);
		foreach (Node child in characterNode.GetChildren())
		{
			if (child is DamagePartDrop { over: false } damagePartDrop && GodotObject.IsInstanceValid(damagePartDrop.sprite))
			{
				GD.Print($"[TestDamagePartVisual] characterZ={value} damagePartZ={damagePartDrop.ZIndex} characterGrid={character.gridPos} node={damagePartDrop.GetPath()}");
			}
		}
		return num - previousCount;
	}

	private static int CountActiveDamagePartNodes()
	{
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(characterNode))
		{
			return 0;
		}
		int num = 0;
		foreach (Node child in characterNode.GetChildren())
		{
			if (child is DamagePartDrop { over: false } damagePartDrop && GodotObject.IsInstanceValid(damagePartDrop.sprite))
			{
				num++;
			}
		}
		return num;
	}

	private void PrintNonGpuGraphCrowdModes()
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = 0; i < _spawnedAnimationSprites.Count; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _spawnedAnimationSprites[i];
			if (adobeAnimateSprite == null || !GodotObject.IsInstanceValid(adobeAnimateSprite) || adobeAnimateSprite.IsRenderedByParentSpriteForRender())
			{
				continue;
			}
			num++;
			if (adobeAnimateSprite.TryBuildCrowdRenderState(out var state) != AdobeAnimateCrowdRenderStateResult.Submitted)
			{
				num5++;
				continue;
			}
			if (state.Mode == AdobeAnimateCrowdRenderMode.GpuGraph)
			{
				num2++;
				continue;
			}
			if (state.Mode == AdobeAnimateCrowdRenderMode.Composite)
			{
				num3++;
			}
			else
			{
				num4++;
			}
			string text = adobeAnimateSprite.SceneFilePath;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = state.Definition?.Source?.ResourcePath ?? adobeAnimateSprite.Name.ToString();
			}
			if (hashSet.Add(text))
			{
				GD.Print($"[TestCrowdMode] mode={state.Mode} scene={text}");
				GD.Print(adobeAnimateSprite.BuildCrowdFilterDebugReport($"mode-report:{state.Mode}", 32));
			}
		}
		GD.Print($"[TestCrowdModeSummary] roots={num} gpuGraph={num2} composite={num3} compact={num4} skipped={num5} uniqueNonGraph={hashSet.Count}");
	}

	private void EnforceFixedAnimationFrame()
	{
		ApplyFixedAnimationFrame(FixedAnimationFrame);
	}

	private void EnforceAdobeFixedAnimationFrame()
	{
		ApplyFixedAnimationFrame(_adobeFixedAnimationFrame);
	}

	private void ApplyFixedAnimationFrame(int fixedAnimationFrame)
	{
		if (fixedAnimationFrame < 0)
		{
			return;
		}
		double elapsedTimer = Math.Clamp(FixedAnimationSubframe, 0.0, 0.999999);
		for (int i = 0; i < _spawnedAnimationSprites.Count; i++)
		{
			AdobeAnimateSpriteBase adobeAnimateSpriteBase = _spawnedAnimationSprites[i];
			if (GodotObject.IsInstanceValid(adobeAnimateSpriteBase))
			{
				adobeAnimateSpriteBase.frameIndex = fixedAnimationFrame;
				adobeAnimateSpriteBase.elapsedTimer = elapsedTimer;
				adobeAnimateSpriteBase.pause = true;
			}
		}
		for (int j = 0; j < _spawnedAnimationSprites.Count; j++)
		{
			AdobeAnimateSpriteBase adobeAnimateSpriteBase2 = _spawnedAnimationSprites[j];
			if (GodotObject.IsInstanceValid(adobeAnimateSpriteBase2))
			{
				adobeAnimateSpriteBase2.UpdateChild();
			}
		}
	}

	private void SpawnPendingBatch()
	{
		long timestamp = Stopwatch.GetTimestamp();
		int num = ((SpawnBatchSize > 0) ? SpawnBatchSize : 2147483647);
		int num2 = 0;
		while (_nextSpawnIndex < SpawnCount && num2 < num && (num2 <= 0 || !(SpawnTimeBudgetMilliseconds > 0.0) || !TowerDefenseCharacterSpawnBudget.IsFrameBudgetExhausted(SpawnTimeBudgetMilliseconds)))
		{
			ulong ticksUsec = Time.GetTicksUsec();
			SpawnRange(_nextSpawnIndex, _nextSpawnIndex + 1, _pendingSpawnFallbackScene);
			TowerDefenseCharacterSpawnBudget.RecordFrameWork(ticksUsec);
			_nextSpawnIndex++;
			num2++;
			if (SpawnTimeBudgetMilliseconds > 0.0 && (TowerDefenseCharacterSpawnBudget.IsFrameBudgetExhausted(SpawnTimeBudgetMilliseconds) || ElapsedMs(timestamp) >= TowerDefenseCharacterSpawnBudget.ClampFrameBudget(SpawnTimeBudgetMilliseconds)))
			{
				break;
			}
		}
		double num3 = ElapsedMs(timestamp);
		_spawnBatchFrameMilliseconds.Add(num3);
		if (SpawnTimeBudgetMilliseconds > 0.0 && num3 > TowerDefenseCharacterSpawnBudget.ClampFrameBudget(SpawnTimeBudgetMilliseconds))
		{
			_spawnFrameBudgetPassed = false;
		}
		if (_nextSpawnIndex >= _nextSpawnProgressPrint && _nextSpawnIndex < SpawnCount)
		{
			GD.Print($"[TestPerf] spawn-batch-progress spawned={_nextSpawnIndex}/{SpawnCount} elapsedMs={ElapsedMs(_spawnStartTicks):F3}");
			_nextSpawnProgressPrint += Math.Max(1, SpawnCount / 10);
		}
		if (_nextSpawnIndex >= SpawnCount)
		{
			_spawnPending = false;
			_pendingSpawnFallbackScene = null;
			CompleteSpawn();
		}
	}

	private void SpawnRange(int start, int end, PackedScene fallbackScene)
	{
		for (int i = start; i < end; i++)
		{
			if (!TryInstantiateSpawnNode(i, fallbackScene, out var spawnNode))
			{
				_spawnFailures++;
				continue;
			}
			if (EnableZInterleaveFixture)
			{
				spawnNode.ZAsRelative = false;
				spawnNode.ZIndex = (((i & 1) != 0) ? 2 : 0);
			}
			else if (RenderZBucketCount > 0)
			{
				spawnNode.ZIndex = i % RenderZBucketCount;
			}
			if (SpawnScale > 0f && Math.Abs(SpawnScale - 1f) > 0.0001f)
			{
				spawnNode.Scale = new Vector2(SpawnScale, SpawnScale);
			}
			long timestamp = Stopwatch.GetTimestamp();
			spawnNode.GlobalPosition = ResolveSpawnPosition(i);
			if (EnableCombatStateStress && spawnNode is TowerDefenseCharacter towerDefenseCharacter)
			{
				towerDefenseCharacter.gridPos = ((EnableSingleEnemyTargetFixture || EnableManyEnemyIdleFixture) ? ResolveSingleTargetSourceGridPosition() : new Vector2I(i % Math.Max(1, TowerDefenseManager.Instance.gridNum.X) + 1, i / Math.Max(1, TowerDefenseManager.Instance.gridNum.X) % Math.Max(1, TowerDefenseManager.Instance.gridNum.Y) + 1));
			}
			AddChild(spawnNode, forceReadableName: false, InternalMode.Disabled);
			double num = ElapsedMs(timestamp);
			_spawnAddChildReadyMilliseconds.Add(num);
			if (num > _spawnAddChildReadyMaxMilliseconds)
			{
				_spawnAddChildReadyMaxMilliseconds = num;
				_spawnAddChildReadyMaxScene = spawnNode.SceneFilePath;
			}
			RegisterSpawnedNode(spawnNode);
			if (EnableExternalVisualStress)
			{
				if (spawnNode is TowerDefenseCharacter character)
				{
					ConfigureExternalVisualStress(character, i);
				}
				else if (spawnNode is AdobeAnimateSprite owner)
				{
					ConfigureExternalVisualStress(owner, i);
				}
			}
		}
	}

	private void ConfigureExternalVisualStress(TowerDefenseCharacter character, int stableIndex)
	{
		if (!GodotObject.IsInstanceValid(character) || !ShouldEnableExternalVisual(stableIndex))
		{
			return;
		}
		_externalVisualExpectedActive++;
		if (ExternalVisualIceOnly || (stableIndex & 1) == 0)
		{
			TowerDefenseCharacterBuffFrozen towerDefenseCharacterBuffFrozen = new TowerDefenseCharacterBuffFrozen();
			towerDefenseCharacterBuffFrozen.character = character;
			towerDefenseCharacterBuffFrozen.Enter();
			RegisterExternalVisualStressFixture(character, character.sprite, character.icetrapSprite, AdobeAnimateExternalVisualHandle.Invalid, ownsVisual: false, stableIndex);
			return;
		}
		if (_externalVisualStressTexture == null)
		{
			_externalVisualStressTexture = GD.Load<Texture2D>("res://Asset/Texture/Character/Effect/ButterSplat.png");
		}
		Sprite2D sprite2D = new Sprite2D
		{
			Name = $"ExternalVisualStress_{stableIndex}",
			Texture = _externalVisualStressTexture,
			Rotation = -0.4f
		};
		character.spriteGroup.AddChild(sprite2D, forceReadableName: false, InternalMode.Disabled);
		AdobeAnimateSlot adobeAnimateSlot = (GodotObject.IsInstanceValid(character.headSlot) ? character.headSlot : null);
		if (GodotObject.IsInstanceValid(adobeAnimateSlot))
		{
			adobeAnimateSlot.Update();
			sprite2D.GlobalPosition = adobeAnimateSlot.GlobalPosition + new Vector2(-5f, -10f);
		}
		else
		{
			sprite2D.Position = new Vector2(0f, -30f);
		}
		AdobeAnimateExternalVisualHandle handle = character.RegisterCharacterExternalVisual(sprite2D, GodotObject.IsInstanceValid(adobeAnimateSlot) ? new AdobeAnimateExternalVisualDescriptor(AdobeAnimateExternalVisualAttachmentMode.Slot, AdobeAnimateExternalVisualDrawBand.InFrontOfAnimation, adobeAnimateSlot) : new AdobeAnimateExternalVisualDescriptor(AdobeAnimateExternalVisualAttachmentMode.Root, AdobeAnimateExternalVisualDrawBand.InFrontOfAnimation));
		RegisterExternalVisualStressFixture(character, character.sprite, sprite2D, handle, ownsVisual: true, stableIndex);
	}

	private void ConfigureExternalVisualStress(AdobeAnimateSprite owner, int stableIndex)
	{
		if (GodotObject.IsInstanceValid(owner) && ShouldEnableExternalVisual(stableIndex))
		{
			_externalVisualExpectedActive++;
			if (_externalVisualStressTexture == null)
			{
				_externalVisualStressTexture = GD.Load<Texture2D>("res://Asset/Texture/Character/Effect/ButterSplat.png");
			}
			Sprite2D sprite2D = new Sprite2D
			{
				Name = $"ExternalVisualStress_{stableIndex}",
				Texture = _externalVisualStressTexture,
				Position = new Vector2(0f, -30f),
				Rotation = -0.4f
			};
			owner.AddChild(sprite2D, forceReadableName: false, InternalMode.Disabled);
			AdobeAnimateExternalVisualHandle handle = owner.RegisterExternalVisual(sprite2D, new AdobeAnimateExternalVisualDescriptor(AdobeAnimateExternalVisualAttachmentMode.Root, AdobeAnimateExternalVisualDrawBand.InFrontOfAnimation));
			RegisterExternalVisualStressFixture(null, owner, sprite2D, handle, ownsVisual: true, stableIndex);
		}
	}

	private void RegisterExternalVisualStressFixture(TowerDefenseCharacter character, AdobeAnimateSprite owner, Sprite2D visual, AdobeAnimateExternalVisualHandle handle, bool ownsVisual, int stableIndex)
	{
		_externalVisualStressFixtures.Add(new ExternalVisualStressFixture
		{
			Character = character,
			Owner = owner,
			Visual = visual,
			Handle = handle,
			OwnsVisual = ownsVisual,
			StableIndex = stableIndex
		});
		if (!GodotObject.IsInstanceValid(owner) || !GodotObject.IsInstanceValid(visual) || (ownsVisual && !handle.IsValid))
		{
			_externalVisualConfigurationFailures++;
			GD.PrintErr($"[TestExternalVisual] configure-failed index={stableIndex} reason=invalid-fixture scene={character?.SceneFilePath ?? owner?.SceneFilePath} sprite={owner?.SceneFilePath}");
		}
	}

	private bool ShouldEnableExternalVisual(int stableIndex)
	{
		double num = Math.Clamp(ExternalVisualRatio, 0.0, 1.0);
		if (num <= 0.0)
		{
			return false;
		}
		if (num >= 1.0)
		{
			return true;
		}
		return (double)(uint)(stableIndex * -1640531535 + -2048144777) / 4294967296.0 < num;
	}

	private void LoadDifferentZombieScenePool()
	{
		List<string> list = new List<string>(256);
		CollectZombieScenePaths(NormalizeResDirectory(ZombieSceneRoot), list);
		list.Sort(StringComparer.OrdinalIgnoreCase);
		foreach (string item in list)
		{
			if (MaxDifferentZombieScenes > 0 && _scenePool.Count >= MaxDifferentZombieScenes)
			{
				break;
			}
			if (!IsZombieSceneFile(item))
			{
				_skippedScenePoolCount++;
				continue;
			}
			if (ShouldExcludeZombieScenePath(item))
			{
				_excludedScenePoolCount++;
				continue;
			}
			PackedScene packedScene = GD.Load<PackedScene>(item);
			if (packedScene == null)
			{
				_skippedScenePoolCount++;
				continue;
			}
			_scenePool.Add(packedScene);
			_scenePoolPaths.Add(item);
			_loadedScenePoolCount++;
		}
		_sceneUseCounts = new int[_scenePool.Count];
	}

	private bool ShouldExcludeZombieScenePath(string resPath)
	{
		if (string.IsNullOrWhiteSpace(ExcludedZombieScenePathSubstring))
		{
			return false;
		}
		string text = resPath.Replace('\\', '/');
		string text2 = ExcludedZombieScenePathSubstring.Replace('\\', '/').Trim();
		if (text2.Length > 0)
		{
			return text.Contains(text2, StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private bool IsZombieSceneFile(string resPath)
	{
		if (!UseZombieCharacterScenes)
		{
			return IsZombieAnimationSceneFile(resPath);
		}
		return IsZombieCharacterSceneFile(resPath);
	}

	private static void CollectZombieScenePaths(string directoryPath, List<string> paths)
	{
		using DirAccess dirAccess = DirAccess.Open(directoryPath);
		if (dirAccess == null)
		{
			return;
		}
		dirAccess.ListDirBegin();
		while (true)
		{
			string next = dirAccess.GetNext();
			if (string.IsNullOrEmpty(next))
			{
				break;
			}
			if (!(next == ".") && !(next == ".."))
			{
				string text = directoryPath + "/" + next;
				if (dirAccess.CurrentIsDir())
				{
					CollectZombieScenePaths(text, paths);
				}
				else if (next.EndsWith(".tscn", StringComparison.OrdinalIgnoreCase))
				{
					paths.Add(text);
				}
			}
		}
		dirAccess.ListDirEnd();
	}

	private static bool IsZombieAnimationSceneFile(string resPath)
	{
		string text = resPath.Replace('\\', '/');
		string text2 = text.ToLowerInvariant();
		if (text2.Contains("/scene/") || text2.Contains("/config/") || text2.Contains("/packet/") || text2.Contains("/damagepoint/") || text2.Contains("/effect/") || text2.Contains("/armor/"))
		{
			return false;
		}
		if (!GetFileNameWithoutExtension(text).StartsWith("Zombie", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		using FileAccess fileAccess = FileAccess.Open(resPath, FileAccess.ModeFlags.Read);
		if (fileAccess == null)
		{
			return false;
		}
		string asText = fileAccess.GetAsText();
		return asText.Contains("flashAnimeData", StringComparison.Ordinal) && asText.Contains("AdobeAnimateSpriteBase.cs", StringComparison.Ordinal);
	}

	private bool IsZombieCharacterSceneFile(string resPath)
	{
		string text = resPath.Replace('\\', '/');
		string text2 = text.ToLowerInvariant();
		if (text2.EndsWith("/towerdefensezombiebungispawn.tscn", StringComparison.Ordinal))
		{
			return false;
		}
		if (!text2.Contains("/scene/"))
		{
			return false;
		}
		string fileNameWithoutExtension = GetFileNameWithoutExtension(text);
		string value = (string.IsNullOrWhiteSpace(CharacterSceneFilePrefix) ? "TowerDefenseZombie" : CharacterSceneFilePrefix.Trim());
		if (!fileNameWithoutExtension.StartsWith(value, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		using FileAccess fileAccess = FileAccess.Open(resPath, FileAccess.ModeFlags.Read);
		if (fileAccess == null)
		{
			return false;
		}
		string asText = fileAccess.GetAsText();
		return asText.Contains("Prefab/TowerDefense/Character/TowerDefenseZombie.tscn", StringComparison.Ordinal) || asText.Contains("TowerDefenseZombie : TowerDefenseCharacter", StringComparison.Ordinal) || asText.Contains("script = ExtResource", StringComparison.Ordinal);
	}

	private bool TryInstantiateSpawnNode(int index, PackedScene fallbackScene, out Node2D spawnNode)
	{
		spawnNode = null;
		PackedScene packedScene = fallbackScene;
		int num = -1;
		if (UseDifferentZombieScenePool)
		{
			if (_scenePool.Count == 0)
			{
				return false;
			}
			num = index % _scenePool.Count;
			packedScene = _scenePool[num];
		}
		if (packedScene == null)
		{
			return false;
		}
		long timestamp = Stopwatch.GetTimestamp();
		Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
		double num2 = ElapsedMs(timestamp);
		_spawnInstantiateMilliseconds.Add(num2);
		if (num2 > _spawnInstantiateMaxMilliseconds)
		{
			_spawnInstantiateMaxMilliseconds = num2;
			_spawnInstantiateMaxScene = packedScene.ResourcePath;
		}
		if (!(node is Node2D node2D))
		{
			node?.QueueFree();
			return false;
		}
		if (UseZombieCharacterScenes && !(node2D is TowerDefenseCharacter))
		{
			node2D.QueueFree();
			return false;
		}
		if (!UseZombieCharacterScenes && !(node2D is AdobeAnimateSpriteBase))
		{
			node2D.QueueFree();
			return false;
		}
		if (num >= 0 && num < _sceneUseCounts.Length)
		{
			_sceneUseCounts[num]++;
		}
		spawnNode = node2D;
		return true;
	}

	private void RegisterSpawnedNode(Node2D node)
	{
		_spawnedNodes.Add(node);
		if (node is TowerDefenseCharacter towerDefenseCharacter)
		{
			_spawnedCharacters.Add(towerDefenseCharacter);
			_spawnedCharacterScenePaths.Add(towerDefenseCharacter.SceneFilePath);
			_spawnedCharacterProcessModes.Add(towerDefenseCharacter.ProcessMode);
			if (BowlingImpactEnsureShield && GodotObject.IsInstanceValid(towerDefenseCharacter.instance) && !towerDefenseCharacter.HasShield())
			{
				towerDefenseCharacter.instance.ArmorAdd("Shield");
			}
			ConfigureCombatCharacterPreservation(towerDefenseCharacter);
			towerDefenseCharacter.showHealthComponent?.SetAlive(ShowCharacterHealthInStress);
			if (ShouldFreezeCombatSourcesForInitialSleep())
			{
				towerDefenseCharacter.ProcessMode = ProcessModeEnum.Disabled;
			}
		}
		if (!string.IsNullOrWhiteSpace(ExactAnimationClip))
		{
			if (node is AdobeAnimateSpriteBase adobeAnimateSpriteBase)
			{
				adobeAnimateSpriteBase.SetClip(ExactAnimationClip);
			}
			else if (node is TowerDefenseCharacter towerDefenseCharacter2 && GodotObject.IsInstanceValid(towerDefenseCharacter2.sprite))
			{
				towerDefenseCharacter2.sprite.SetClip(ExactAnimationClip);
			}
		}
		int count = _spawnedAnimationSprites.Count;
		CollectAnimationSprites(node, _spawnedAnimationSprites);
		if (FixedAnimationFrame < 0)
		{
			return;
		}
		for (int i = count; i < _spawnedAnimationSprites.Count; i++)
		{
			AdobeAnimateSpriteBase adobeAnimateSpriteBase2 = _spawnedAnimationSprites[i];
			if (GodotObject.IsInstanceValid(adobeAnimateSpriteBase2))
			{
				adobeAnimateSpriteBase2.frameIndex = FixedAnimationFrame;
				adobeAnimateSpriteBase2.elapsedTimer = Math.Clamp(FixedAnimationSubframe, 0.0, 0.999999);
				adobeAnimateSpriteBase2.pause = true;
			}
		}
	}

	private void ConfigureCombatCharacterPreservation(TowerDefenseCharacter character)
	{
		if (!EnableCombatStateStress || !PreserveCombatCharactersInStress || !GodotObject.IsInstanceValid(character?.instance))
		{
			return;
		}
		TowerDefenseCharacterInstance instance = character.instance;
		instance.keepAlive = true;
		instance.hitpointsNearDeath = 0.0;
		instance.hitpointsSave = 1000000000000.0;
		instance.hitpoints = 1000000000000.0;
		for (int i = 0; i < instance.armorList.Count; i++)
		{
			TowerDefenseArmorInstance towerDefenseArmorInstance = instance.armorList[i];
			if (GodotObject.IsInstanceValid(towerDefenseArmorInstance) && !towerDefenseArmorInstance.isRemove)
			{
				towerDefenseArmorInstance.damagePointBase = 1000000000000.0;
				towerDefenseArmorInstance.hitpointsSave = 1000000000000.0;
				towerDefenseArmorInstance.hitPoints = 1000000000000.0;
			}
		}
	}

	private bool ShouldFreezeCombatSourcesForInitialSleep()
	{
		if (EnableCombatStateStress && EnableSingleEnemyTargetFixture)
		{
			return EnableSleepCycleStress;
		}
		return false;
	}

	private void RestoreCombatSourceProcessModes()
	{
		int num = Math.Min(_spawnedCharacters.Count, _spawnedCharacterProcessModes.Count);
		for (int i = 0; i < num; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				towerDefenseCharacter.ProcessMode = _spawnedCharacterProcessModes[i];
			}
		}
	}

	private bool TryNormalizeCombatStressSourceCamp(out TowerDefenseEnum.CHARACTER_CAMP sourceCamp)
	{
		sourceCamp = _spawnedCharacters[0].camp;
		TowerDefenseEnum.CHARACTER_CAMP cHARACTER_CAMP = sourceCamp;
		if (cHARACTER_CAMP != TowerDefenseEnum.CHARACTER_CAMP.PLANT && cHARACTER_CAMP != TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE)
		{
			GD.PushError($"[TestCombatFixture] Unsupported source camp: {sourceCamp}.");
			return false;
		}
		int num = 0;
		for (int i = 0; i < _spawnedCharacters.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter))
			{
				GD.PushError($"[TestCombatFixture] Source character is invalid at index {i}.");
				return false;
			}
			if (towerDefenseCharacter.camp != sourceCamp)
			{
				towerDefenseCharacter.camp = sourceCamp;
				towerDefenseCharacter.targetRegistrationComponent?.NotifyTargetStateChanged();
				num++;
			}
		}
		if (num > 0)
		{
			GD.Print($"[TestCombatFixture] normalizedSourceCamp={sourceCamp} normalized={num}/{_spawnedCharacters.Count}");
		}
		return true;
	}

	private void PrepareSingleEnemyTargetFixture()
	{
		if (!EnableSingleEnemyTargetFixture)
		{
			return;
		}
		if (!EnableCombatStateStress || !UseZombieCharacterScenes)
		{
			GD.PushError("[TestCombatFixture] The single-enemy fixture requires character scenes and combat-state stress.");
		}
		else if (_spawnedCharacters.Count == 0 || !GodotObject.IsInstanceValid(_spawnedCharacters[0]))
		{
			GD.PushError("[TestCombatFixture] No source character is available for the single enemy.");
		}
		else
		{
			if (!TryNormalizeCombatStressSourceCamp(out var sourceCamp))
			{
				return;
			}
			string text;
			if (string.IsNullOrWhiteSpace(SingleEnemyTargetScenePath))
			{
				text = ((sourceCamp == TowerDefenseEnum.CHARACTER_CAMP.PLANT) ? "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn" : "res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn");
			}
			else
			{
				text = SingleEnemyTargetScenePath.Trim();
			}
			PackedScene packedScene = GD.Load<PackedScene>(text);
			if (packedScene == null)
			{
				GD.PushError("[TestCombatFixture] Failed to load enemy scene: " + text);
				return;
			}
			Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			if (!(node is TowerDefenseCharacter towerDefenseCharacter))
			{
				node?.QueueFree();
				GD.PushError("[TestCombatFixture] Enemy scene is not a TowerDefenseCharacter: " + text);
				return;
			}
			Vector2I gridPos = ResolveSingleTargetSourceGridPosition();
			Vector2 vector = ResolveSingleTargetSourcePosition();
			ulong physicsFrames = Engine.GetPhysicsFrames();
			for (int i = 0; i < _spawnedCharacters.Count; i++)
			{
				TowerDefenseCharacter towerDefenseCharacter2 = _spawnedCharacters[i];
				towerDefenseCharacter2.gridPos = gridPos;
				towerDefenseCharacter2.SetGlobalPositionForPhysicsFrame(vector, physicsFrames);
			}
			int num = ((sourceCamp == TowerDefenseEnum.CHARACTER_CAMP.PLANT) ? 1 : (-1));
			TowerDefenseManager instance = TowerDefenseManager.Instance;
			float num2 = (GodotObject.IsInstanceValid(instance) ? Mathf.Clamp(instance.gridSize.X * 0.4f, 24f, 64f) : 48f);
			towerDefenseCharacter.Name = "SingleEnemyComponentStressTarget";
			towerDefenseCharacter.camp = ((sourceCamp == TowerDefenseEnum.CHARACTER_CAMP.PLANT) ? TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE : TowerDefenseEnum.CHARACTER_CAMP.PLANT);
			towerDefenseCharacter.gridPos = gridPos;
			towerDefenseCharacter.GlobalPosition = vector + new Vector2((float)num * num2, 0f);
			float num3 = Math.Max(0.1f, SingleEnemyTargetScale);
			towerDefenseCharacter.Scale *= num3;
			AddChild(towerDefenseCharacter, forceReadableName: false, InternalMode.Disabled);
			if (!GodotObject.IsInstanceValid(towerDefenseCharacter.instance))
			{
				towerDefenseCharacter.QueueFree();
				GD.PushError("[TestCombatFixture] Enemy target has no runtime instance after Ready.");
				return;
			}
			towerDefenseCharacter.instance.keepAlive = true;
			towerDefenseCharacter.instance.invincible = false;
			towerDefenseCharacter.instance.invincibleHurt = false;
			towerDefenseCharacter.instance.invincibleSmash = false;
			towerDefenseCharacter.instance.canBeCollection = true;
			towerDefenseCharacter.instance.die = false;
			towerDefenseCharacter.instance.nearDie = false;
			towerDefenseCharacter.instance.height = TowerDefenseEnum.CHARACTER_HEIGHT.TALL;
			towerDefenseCharacter.instance.maskFlags = -1;
			towerDefenseCharacter.instance.collisionFlags = -1;
			towerDefenseCharacter.instance.hitpointsNearDeath = 0.0;
			towerDefenseCharacter.instance.hitpointsSave = 1000000000000.0;
			towerDefenseCharacter.instance.hitpoints = 1000000000000.0;
			for (int j = 0; j < towerDefenseCharacter.instance.armorList.Count; j++)
			{
				TowerDefenseArmorInstance towerDefenseArmorInstance = towerDefenseCharacter.instance.armorList[j];
				if (GodotObject.IsInstanceValid(towerDefenseArmorInstance) && !towerDefenseArmorInstance.isRemove)
				{
					towerDefenseArmorInstance.damagePointBase = 1000000000000.0;
					towerDefenseArmorInstance.hitpointsSave = 1000000000000.0;
					towerDefenseArmorInstance.hitPoints = 1000000000000.0;
				}
			}
			towerDefenseCharacter.targetRegistrationComponent?.SetAlive(alive: true);
			if (towerDefenseCharacter.targetRegistrationComponent != null)
			{
				towerDefenseCharacter.targetRegistrationComponent.allLineCheck = true;
				towerDefenseCharacter.targetRegistrationComponent.canProjectileCheck = true;
				towerDefenseCharacter.targetRegistrationComponent.NotifyTargetStateChanged();
			}
			towerDefenseCharacter.showHealthComponent?.SetAlive(alive: false);
			if (GodotObject.IsInstanceValid(towerDefenseCharacter.componentManager))
			{
				IReadOnlyList<CharacterComponentRuntime> resourceComponents = towerDefenseCharacter.componentManager.ResourceComponents;
				for (int k = 0; k < resourceComponents.Count; k++)
				{
					CharacterComponentRuntime characterComponentRuntime = resourceComponents[k];
					if ((!(characterComponentRuntime is TargetRegistrationComponent) && !(characterComponentRuntime is HurtComponent)) || 1 == 0)
					{
						characterComponentRuntime.SetAlive(alive: false);
					}
				}
			}
			_singleEnemyTarget = towerDefenseCharacter;
			_singleEnemyTargetInitialHitpoints = GetCombatTargetDurability(towerDefenseCharacter);
			_singleEnemyAttackComponentCount = 0;
			_singleEnemyAttackUtilityComponentCount = 0;
			_singleEnemyFireComponentCount = 0;
			_engagedAttackComponents.Clear();
			_engagedFireComponents.Clear();
			_exercisedAttackUtilityComponents.Clear();
			_pendingSoccerAttackUtilityComponents.Clear();
			for (int l = 0; l < _spawnedCharacters.Count; l++)
			{
				TowerDefenseCharacter towerDefenseCharacter3 = _spawnedCharacters[l];
				ComponentManager componentManager = towerDefenseCharacter3?.componentManager;
				if (!GodotObject.IsInstanceValid(componentManager))
				{
					continue;
				}
				IReadOnlyList<CharacterComponentRuntime> resourceComponents2 = componentManager.ResourceComponents;
				for (int m = 0; m < resourceComponents2.Count; m++)
				{
					CharacterComponentRuntime characterComponentRuntime2 = resourceComponents2[m];
					AttackComponent attack = characterComponentRuntime2 as AttackComponent;
					if (attack != null)
					{
						if (IsRoleUtilityAttackComponent(towerDefenseCharacter3, attack))
						{
							_singleEnemyAttackUtilityComponentCount++;
							_pendingSoccerAttackUtilityComponents.Add(((TowerDefenseZombieSoccer)towerDefenseCharacter3, attack));
							continue;
						}
						_singleEnemyAttackComponentCount++;
						if (!TryAttachLegacyAttackEngagementHandler(towerDefenseCharacter3, attack))
						{
							AttackComponent.AttackEventHandler handler = null;
							handler = () =>
							{
								_engagedAttackComponents.Add(attack);
								attack.OnAttack -= handler;
								_attackEngagementHandlers.Remove(attack);
							};
							_attackEngagementHandlers[attack] = handler;
							attack.OnAttack += handler;
						}
						continue;
					}
					characterComponentRuntime2 = resourceComponents2[m];
					FireComponent fire = characterComponentRuntime2 as FireComponent;
					if (fire != null)
					{
						_singleEnemyFireComponentCount++;
						FireComponent.FireVolleyEventHandler handler2 = null;
						handler2 = (ulong _) =>
						{
							_engagedFireComponents.Add(fire);
							fire.OnFireVolley -= handler2;
							_fireEngagementHandlers.Remove(fire);
						};
						_fireEngagementHandlers[fire] = handler2;
						fire.OnFireVolley += handler2;
					}
				}
			}
			_singleEnemyFixturePrepared = true;
			if (ShouldFreezeCombatSourcesForInitialSleep())
			{
				BeginSleepCycleStress(0.0);
				RestoreCombatSourceProcessModes();
			}
			GD.Print($"[TestCombatFixture] ready sources={_spawnedCharacters.Count} targets=1 sourceCamp={sourceCamp} targetCamp={towerDefenseCharacter.camp} targetScene={text} attackComponents={_singleEnemyAttackComponentCount} attackUtilityComponents={_singleEnemyAttackUtilityComponentCount} fireComponents={_singleEnemyFireComponentCount} targetScale={num3:F2}");
		}
	}

	private void PrepareManyEnemyIdleTargetFixture()
	{
		if (!EnableManyEnemyIdleFixture)
		{
			return;
		}
		if (EnableSingleEnemyTargetFixture)
		{
			GD.PushError("[TestCombatFixture] Single-enemy attack and many-enemy idle fixtures are mutually exclusive.");
		}
		else if (!EnableCombatStateStress || !UseZombieCharacterScenes)
		{
			GD.PushError("[TestCombatFixture] The many-enemy idle fixture requires character scenes and combat-state stress.");
		}
		else if (_spawnedCharacters.Count == 0 || !GodotObject.IsInstanceValid(_spawnedCharacters[0]))
		{
			GD.PushError("[TestCombatFixture] No source character is available for idle targets.");
		}
		else
		{
			if (!TryNormalizeCombatStressSourceCamp(out var sourceCamp))
			{
				return;
			}
			string text;
			if (string.IsNullOrWhiteSpace(SingleEnemyTargetScenePath))
			{
				text = ((sourceCamp == TowerDefenseEnum.CHARACTER_CAMP.PLANT) ? "res://Asset/Anime/Character/Zombie/Chapter1/Normal/Scene/Base/TowerDefenseZombieNormal.tscn" : "res://Asset/Anime/Character/Plant/Chapter0/Wallnut/Scene/TowerDefensePlantWallnut.tscn");
			}
			else
			{
				text = SingleEnemyTargetScenePath.Trim();
			}
			PackedScene packedScene = GD.Load<PackedScene>(text);
			if (packedScene == null)
			{
				GD.PushError("[TestCombatFixture] Failed to load idle enemy scene: " + text);
				return;
			}
			Vector2I vector2I = ResolveSingleTargetSourceGridPosition();
			Vector2 vector = ResolveSingleTargetSourcePosition();
			ulong physicsFrames = Engine.GetPhysicsFrames();
			for (int i = 0; i < _spawnedCharacters.Count; i++)
			{
				TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
				towerDefenseCharacter.gridPos = vector2I;
				towerDefenseCharacter.SetGlobalPositionForPhysicsFrame(vector, physicsFrames);
			}
			int num = ((sourceCamp == TowerDefenseEnum.CHARACTER_CAMP.PLANT) ? 1 : (-1));
			TowerDefenseManager instance = TowerDefenseManager.Instance;
			float num2 = (GodotObject.IsInstanceValid(instance) ? Mathf.Clamp(instance.gridSize.X * 0.4f, 24f, 64f) : 48f);
			Vector2I gridPos = vector2I;
			Vector2 globalPosition = vector + new Vector2((float)num * num2, 0f);
			if (ManyEnemyIdleTargetsOutsideAttackRange)
			{
				int x = ((sourceCamp == TowerDefenseEnum.CHARACTER_CAMP.PLANT) ? (vector2I.X + 8) : (vector2I.X - 8));
				gridPos = new Vector2I(x, vector2I.Y);
				float num3 = (GodotObject.IsInstanceValid(instance) ? (instance.gridSize.X * 8f) : 800f);
				globalPosition = vector + new Vector2((float)num * num3, 0f);
			}
			float num4 = Math.Max(0.1f, SingleEnemyTargetScale);
			int num5 = Math.Max(1, ManyEnemyIdleTargetCount);
			for (int j = 0; j < num5; j++)
			{
				Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
				if (!(node is TowerDefenseCharacter towerDefenseCharacter2))
				{
					node?.QueueFree();
					GD.PushError("[TestCombatFixture] Idle enemy scene is not a TowerDefenseCharacter: " + text);
					break;
				}
				towerDefenseCharacter2.Name = $"ManyEnemyIdleTarget_{j}";
				towerDefenseCharacter2.camp = ((sourceCamp == TowerDefenseEnum.CHARACTER_CAMP.PLANT) ? TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE : TowerDefenseEnum.CHARACTER_CAMP.PLANT);
				towerDefenseCharacter2.gridPos = gridPos;
				towerDefenseCharacter2.GlobalPosition = globalPosition;
				towerDefenseCharacter2.Scale *= num4;
				towerDefenseCharacter2.Visible = false;
				towerDefenseCharacter2.ProcessMode = ProcessModeEnum.Disabled;
				AddChild(towerDefenseCharacter2, forceReadableName: false, InternalMode.Disabled);
				if (!GodotObject.IsInstanceValid(towerDefenseCharacter2.instance))
				{
					towerDefenseCharacter2.QueueFree();
					GD.PushError($"[TestCombatFixture] Idle enemy target {j} has no runtime instance after Ready.");
					break;
				}
				towerDefenseCharacter2.instance.keepAlive = true;
				towerDefenseCharacter2.instance.invincible = false;
				towerDefenseCharacter2.instance.invincibleHurt = false;
				towerDefenseCharacter2.instance.invincibleSmash = false;
				towerDefenseCharacter2.instance.canBeCollection = true;
				towerDefenseCharacter2.instance.die = false;
				towerDefenseCharacter2.instance.nearDie = false;
				towerDefenseCharacter2.instance.height = TowerDefenseEnum.CHARACTER_HEIGHT.TALL;
				int num6 = (ManyEnemyIdleTargetsOutsideAttackRange ? (-1) : 0);
				towerDefenseCharacter2.instance.maskFlags = num6;
				towerDefenseCharacter2.instance.collisionFlags = num6;
				towerDefenseCharacter2.instance.hitpointsNearDeath = 0.0;
				towerDefenseCharacter2.instance.hitpointsSave = 1000000000000.0;
				towerDefenseCharacter2.instance.hitpoints = 1000000000000.0;
				for (int k = 0; k < towerDefenseCharacter2.instance.armorList.Count; k++)
				{
					TowerDefenseArmorInstance towerDefenseArmorInstance = towerDefenseCharacter2.instance.armorList[k];
					if (GodotObject.IsInstanceValid(towerDefenseArmorInstance) && !towerDefenseArmorInstance.isRemove)
					{
						towerDefenseArmorInstance.damagePointBase = 1000000000000.0;
						towerDefenseArmorInstance.hitpointsSave = 1000000000000.0;
						towerDefenseArmorInstance.hitPoints = 1000000000000.0;
					}
				}
				towerDefenseCharacter2.targetRegistrationComponent?.SetAlive(alive: true);
				if (towerDefenseCharacter2.targetRegistrationComponent != null)
				{
					towerDefenseCharacter2.targetRegistrationComponent.allLineCheck = !ManyEnemyIdleTargetsOutsideAttackRange;
					towerDefenseCharacter2.targetRegistrationComponent.canProjectileCheck = true;
					towerDefenseCharacter2.targetRegistrationComponent.NotifyTargetStateChanged();
				}
				towerDefenseCharacter2.showHealthComponent?.SetAlive(alive: false);
				if (GodotObject.IsInstanceValid(towerDefenseCharacter2.componentManager))
				{
					IReadOnlyList<CharacterComponentRuntime> resourceComponents = towerDefenseCharacter2.componentManager.ResourceComponents;
					for (int l = 0; l < resourceComponents.Count; l++)
					{
						CharacterComponentRuntime characterComponentRuntime = resourceComponents[l];
						if ((!(characterComponentRuntime is TargetRegistrationComponent) && !(characterComponentRuntime is HurtComponent)) || 1 == 0)
						{
							characterComponentRuntime.SetAlive(alive: false);
						}
					}
				}
				_manyEnemyIdleTargets.Add(towerDefenseCharacter2);
			}
			_engagedAttackComponents.Clear();
			_engagedFireComponents.Clear();
			_manyEnemyIdleAttackComponents.Clear();
			_manyEnemyIdleFireComponents.Clear();
			for (int m = 0; m < _spawnedCharacters.Count; m++)
			{
				TowerDefenseCharacter towerDefenseCharacter3 = _spawnedCharacters[m];
				ComponentManager componentManager = towerDefenseCharacter3?.componentManager;
				if (!GodotObject.IsInstanceValid(componentManager))
				{
					continue;
				}
				IReadOnlyList<CharacterComponentRuntime> resourceComponents2 = componentManager.ResourceComponents;
				for (int n = 0; n < resourceComponents2.Count; n++)
				{
					CharacterComponentRuntime characterComponentRuntime2 = resourceComponents2[n];
					AttackComponent attack = characterComponentRuntime2 as AttackComponent;
					if (attack != null)
					{
						_manyEnemyIdleAttackComponents.Add(attack);
						if (!TryAttachLegacyAttackEngagementHandler(towerDefenseCharacter3, attack))
						{
							AttackComponent.AttackEventHandler handler = null;
							handler = () =>
							{
								_engagedAttackComponents.Add(attack);
								attack.OnAttack -= handler;
								_attackEngagementHandlers.Remove(attack);
							};
							_attackEngagementHandlers[attack] = handler;
							attack.OnAttack += handler;
						}
						continue;
					}
					characterComponentRuntime2 = resourceComponents2[n];
					FireComponent fire = characterComponentRuntime2 as FireComponent;
					if (fire != null)
					{
						_manyEnemyIdleFireComponents.Add(fire);
						FireComponent.FireVolleyEventHandler handler2 = null;
						handler2 = (ulong _) =>
						{
							_engagedFireComponents.Add(fire);
							fire.OnFireVolley -= handler2;
							_fireEngagementHandlers.Remove(fire);
						};
						_fireEngagementHandlers[fire] = handler2;
						fire.OnFireVolley += handler2;
					}
				}
			}
			_manyEnemyIdleFixturePrepared = _manyEnemyIdleTargets.Count == num5;
			GD.Print($"[TestCombatIdleFixture] ready sources={_spawnedCharacters.Count} targets={_manyEnemyIdleTargets.Count}/{num5} sourceCamp={sourceCamp} targetScene={text} attackComponents={_manyEnemyIdleAttackComponents.Count} fireComponents={_manyEnemyIdleFireComponents.Count} targetScale={num4:F2} outsideAttackRange={ManyEnemyIdleTargetsOutsideAttackRange}");
		}
	}

	private static bool IsRoleUtilityAttackComponent(TowerDefenseCharacter source, AttackComponent attack)
	{
		if (source is TowerDefenseZombieSoccer)
		{
			return string.Equals(attack.ComponentDefinition?.InstanceId, "character.attack.1", StringComparison.Ordinal);
		}
		return false;
	}

	private void TryDriveSingleEnemyRoleAttackUtilities()
	{
		if (!_singleEnemyFixturePrepared || _singleEnemyAttackUtilityComponentCount == 0 || _singleEnemyRoleUtilityPhaseCompleted || !GodotObject.IsInstanceValid(_singleEnemyTarget))
		{
			return;
		}
		if (!_singleEnemyRoleUtilityPhaseStarted)
		{
			if (_engagedAttackComponents.Count != _singleEnemyAttackComponentCount)
			{
				return;
			}
			_singleEnemyRoleUtilityPhaseStarted = true;
			_singleEnemyTarget.targetRegistrationComponent?.SetAlive(alive: false);
			_singleEnemyTarget.gridPos = Vector2I.One;
			_singleEnemyTarget.SetGlobalPositionForPhysicsFrame(new Vector2(-10000f, -10000f), Engine.GetPhysicsFrames());
			GD.Print($"[TestCombatFixture] role-attack-utility-phase-start role={"TowerDefenseZombieSoccer"} components={_singleEnemyAttackUtilityComponentCount} " + "trigger=primary-attacks-engaged");
		}
		double totalSeconds = _uptime.Elapsed.TotalSeconds;
		if (totalSeconds < _nextSingleEnemyRoleUtilityAuditSeconds)
		{
			return;
		}
		_nextSingleEnemyRoleUtilityAuditSeconds = totalSeconds + 0.1;
		for (int num = _pendingSoccerAttackUtilityComponents.Count - 1; num >= 0; num--)
		{
			var (towerDefenseZombieSoccer, item) = _pendingSoccerAttackUtilityComponents[num];
			if (GodotObject.IsInstanceValid(towerDefenseZombieSoccer) && !towerDefenseZombieSoccer.hasBall)
			{
				_exercisedAttackUtilityComponents.Add(item);
				_pendingSoccerAttackUtilityComponents.RemoveAt(num);
			}
		}
		if (_pendingSoccerAttackUtilityComponents.Count <= 0)
		{
			Vector2I gridPos = ResolveSingleTargetSourceGridPosition();
			Vector2 vector = ResolveSingleTargetSourcePosition();
			ulong physicsFrames = Engine.GetPhysicsFrames();
			for (int i = 0; i < _spawnedCharacters.Count; i++)
			{
				TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
				towerDefenseCharacter.gridPos = gridPos;
				towerDefenseCharacter.SetGlobalPositionForPhysicsFrame(vector, physicsFrames);
			}
			_singleEnemyTarget.gridPos = gridPos;
			_singleEnemyTarget.SetGlobalPositionForPhysicsFrame(vector + new Vector2((_singleEnemyTarget.camp == TowerDefenseEnum.CHARACTER_CAMP.ZOMBIE) ? 48f : (-48f), 0f), physicsFrames);
			_singleEnemyTarget.targetRegistrationComponent?.SetAlive(alive: true);
			_singleEnemyTarget.targetRegistrationComponent?.NotifyTargetStateChanged();
			_singleEnemyRoleUtilityPhaseCompleted = true;
			GD.Print($"[TestCombatFixture] role-attack-utility-phase-complete role={"TowerDefenseZombieSoccer"} components={_exercisedAttackUtilityComponents.Count}/{_singleEnemyAttackUtilityComponentCount} trigger=Put");
		}
	}

	private void TryRunSleepCycleStress(double sampleElapsed)
	{
		if (!EnableSleepCycleStress || _sleepCycleCompleted)
		{
			return;
		}
		if (_sleepCyclePhase == 0)
		{
			if (_singleEnemyFixturePrepared && !(sampleElapsed < Math.Max(0.0, SleepCycleStartAfterSeconds)))
			{
				BeginSleepCycleStress(sampleElapsed);
			}
		}
		else if (_sleepCyclePhase == 1)
		{
			_sleepCycleEnteredCount = CountSleepingStressCharacters();
			if (_sleepCycleEnteredCount < _sleepCycleEligibleCount && sampleElapsed < _sleepCyclePhaseDeadlineSeconds)
			{
				return;
			}
			if (_sleepCycleEligibleCount != SpawnCount || _sleepCycleAppliedCount != _sleepCycleEligibleCount || _sleepCycleEnteredCount != _sleepCycleEligibleCount)
			{
				CompleteSleepCycleStress(passed: false, $"sleep entry mismatch eligible={_sleepCycleEligibleCount} applied={_sleepCycleAppliedCount} entered={_sleepCycleEnteredCount} missingBuffScenes={FormatSleepCycleMissingBuffScenes()} missingEntryScenes={FormatSleepCycleMissingEntryScenes()}");
				return;
			}
			for (int i = 0; i < _spawnedCharacters.Count; i++)
			{
				TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
				if (GodotObject.IsInstanceValid(towerDefenseCharacter))
				{
					towerDefenseCharacter.buff?.DeleteBuff("Sleep");
					towerDefenseCharacter.componentManager?.GetRuntime<SleepComponent>("character.sleep")?.SleepProcessing(0f);
				}
			}
			_sleepCyclePhase = 2;
			_sleepCyclePhaseDeadlineSeconds = sampleElapsed + 3.0;
			GD.Print($"[TestSleepCycle] phase=wake-request entered={_sleepCycleEnteredCount} expected={_sleepCycleEligibleCount}");
		}
		else
		{
			_sleepCycleWokenCount = CountWokenStressCharacters();
			if (_sleepCycleWokenCount >= _sleepCycleEligibleCount || !(sampleElapsed < _sleepCyclePhaseDeadlineSeconds))
			{
				CompleteSleepCycleStress(_sleepCycleWokenCount == _sleepCycleEligibleCount, $"wake result={_sleepCycleWokenCount}/{_sleepCycleEligibleCount}");
			}
		}
	}

	private void BeginSleepCycleStress(double sampleElapsed)
	{
		if (_sleepCyclePhase != 0 || _sleepCycleCompleted)
		{
			return;
		}
		TowerDefenseCharacterEventAddBuff towerDefenseCharacterEventAddBuff = FindHypnotistSleepEventForStress();
		if (!GodotObject.IsInstanceValid(towerDefenseCharacterEventAddBuff))
		{
			CompleteSleepCycleStress(passed: false, "real Hypnotist Sleep event was not found");
			return;
		}
		_sleepCycleEligibleCount = 0;
		_sleepCycleAppliedCount = 0;
		_sleepCycleNormalizedImmunityCount = 0;
		_sleepCycleImmunityRestored = false;
		_spawnedCharacterSleepBuffFlags.Clear();
		int num = 256;
		for (int i = 0; i < _spawnedCharacters.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
			int num2 = (GodotObject.IsInstanceValid(towerDefenseCharacter?.instance) ? towerDefenseCharacter.instance.unUseBuffFlags : 0);
			_spawnedCharacterSleepBuffFlags.Add(num2);
			SleepComponent sleepComponent = towerDefenseCharacter?.componentManager?.GetRuntime<SleepComponent>("character.sleep");
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && sleepComponent != null && !sleepComponent.IsReleased && sleepComponent.Alive)
			{
				_sleepCycleEligibleCount++;
				if ((num2 & num) != 0)
				{
					towerDefenseCharacter.instance.unUseBuffFlags = num2 & ~num;
					_sleepCycleNormalizedImmunityCount++;
				}
				towerDefenseCharacterEventAddBuff.Execute(towerDefenseCharacter.GlobalPosition, towerDefenseCharacter);
				BuffComponent buff = towerDefenseCharacter.buff;
				if (buff != null && buff.BuffHas("Sleep"))
				{
					_sleepCycleAppliedCount++;
				}
			}
		}
		_sleepCyclePhase = 1;
		_sleepCyclePhaseDeadlineSeconds = sampleElapsed + 3.0;
		GD.Print($"[TestSleepCycle] phase=apply eligible={_sleepCycleEligibleCount} applied={_sleepCycleAppliedCount} expected={SpawnCount} normalizedSleepImmunity={_sleepCycleNormalizedImmunityCount} " + "source=Hypnotist.rangeEvent");
	}

	private TowerDefenseCharacterEventAddBuff FindHypnotistSleepEventForStress()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < _spawnedCharacters.Count; i++)
		{
			if (!(_spawnedCharacters[i] is TowerDefenseZombieHypnotist towerDefenseZombieHypnotist))
			{
				continue;
			}
			num++;
			num2 += towerDefenseZombieHypnotist.rangeEvent.Count;
			for (int j = 0; j < towerDefenseZombieHypnotist.rangeEvent.Count; j++)
			{
				if (!(towerDefenseZombieHypnotist.rangeEvent[j] is TowerDefenseCharacterEventAddBuff towerDefenseCharacterEventAddBuff))
				{
					continue;
				}
				num3++;
				for (int k = 0; k < towerDefenseCharacterEventAddBuff.buffList.Count; k++)
				{
					if (towerDefenseCharacterEventAddBuff.buffList[k] is TowerDefenseCharacterBuffSleep)
					{
						num4++;
						return towerDefenseCharacterEventAddBuff;
					}
				}
			}
		}
		_sleepCycleHypnotistEventProvider = ResourceLoader.Load<PackedScene>("res://Asset/Anime/Character/Zombie/Chapter5/Hypnotist/Scene/TowerDefenseZombieHypnotist.tscn", null, ResourceLoader.CacheMode.Ignore)?.Instantiate<TowerDefenseZombieHypnotist>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(_sleepCycleHypnotistEventProvider))
		{
			num++;
			num2 += _sleepCycleHypnotistEventProvider.rangeEvent.Count;
			for (int l = 0; l < _sleepCycleHypnotistEventProvider.rangeEvent.Count; l++)
			{
				if (!(_sleepCycleHypnotistEventProvider.rangeEvent[l] is TowerDefenseCharacterEventAddBuff towerDefenseCharacterEventAddBuff2))
				{
					continue;
				}
				num3++;
				for (int m = 0; m < towerDefenseCharacterEventAddBuff2.buffList.Count; m++)
				{
					if (towerDefenseCharacterEventAddBuff2.buffList[m] is TowerDefenseCharacterBuffSleep)
					{
						num4++;
						GD.Print("[TestSleepCycle] source=authored-hypnotist-provider scene=res://Asset/Anime/Character/Zombie/Chapter5/Hypnotist/Scene/TowerDefenseZombieHypnotist.tscn");
						return towerDefenseCharacterEventAddBuff2;
					}
				}
			}
		}
		GD.PrintErr($"[TestSleepCycle] Hypnotist sleep source missing roles={num} rangeEvents={num2} addBuffEvents={num3} sleepBuffs={num4}");
		return null;
	}

	private int CountSleepingStressCharacters()
	{
		int num = 0;
		for (int i = 0; i < _spawnedCharacters.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && towerDefenseCharacter.IsSleep() && !towerDefenseCharacter.componentAlive)
			{
				BuffComponent buff = towerDefenseCharacter.buff;
				if (buff != null && buff.BuffHas("Sleep"))
				{
					num++;
				}
			}
		}
		return num;
	}

	private int CountWokenStressCharacters()
	{
		int num = 0;
		for (int i = 0; i < _spawnedCharacters.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && !towerDefenseCharacter.IsSleep() && towerDefenseCharacter.componentAlive)
			{
				BuffComponent buff = towerDefenseCharacter.buff;
				if (buff == null || !buff.BuffHas("Sleep"))
				{
					num++;
				}
			}
		}
		return num;
	}

	private string FormatSleepCycleMissingBuffScenes()
	{
		return FormatSleepCycleSceneBreakdown((TowerDefenseCharacter character) =>
		{
			if (GodotObject.IsInstanceValid(character))
			{
				BuffComponent buff = character.buff;
				if (buff == null)
				{
					return true;
				}
				return !buff.BuffHas("Sleep");
			}
			return true;
		});
	}

	private string FormatSleepCycleMissingEntryScenes()
	{
		return FormatSleepCycleSceneBreakdown((TowerDefenseCharacter character) => !GodotObject.IsInstanceValid(character) || !character.IsSleep() || character.componentAlive);
	}

	private string FormatSleepCycleSceneBreakdown(Func<TowerDefenseCharacter, bool> isMissing)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.Ordinal);
		int num = Math.Min(_spawnedCharacters.Count, _spawnedCharacterScenePaths.Count);
		for (int i = 0; i < num; i++)
		{
			if (isMissing(_spawnedCharacters[i]))
			{
				string text = _spawnedCharacterScenePaths[i];
				if (string.IsNullOrWhiteSpace(text))
				{
					text = "<unknown>";
				}
				dictionary[text] = dictionary.GetValueOrDefault(text) + 1;
			}
		}
		if (dictionary.Count == 0)
		{
			return "none";
		}
		List<string> list = new List<string>(dictionary.Keys);
		list.Sort(StringComparer.Ordinal);
		string[] array = new string[list.Count];
		for (int j = 0; j < list.Count; j++)
		{
			string text2 = list[j];
			array[j] = $"{text2}:{dictionary[text2]}";
		}
		return string.Join('|', array);
	}

	private void CompleteSleepCycleStress(bool passed, string reason)
	{
		RestoreSleepCycleImmunity();
		_sleepCycleCompleted = true;
		_sleepCyclePassed = passed;
		GD.Print($"[TestSleepCycle] phase=complete passed={passed} reason={reason} eligible={_sleepCycleEligibleCount} applied={_sleepCycleAppliedCount} entered={_sleepCycleEnteredCount} woken={_sleepCycleWokenCount}");
	}

	private void RestoreSleepCycleImmunity()
	{
		if (_sleepCycleImmunityRestored)
		{
			return;
		}
		int num = Math.Min(_spawnedCharacters.Count, _spawnedCharacterSleepBuffFlags.Count);
		for (int i = 0; i < num; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = _spawnedCharacters[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter?.instance))
			{
				towerDefenseCharacter.instance.unUseBuffFlags = _spawnedCharacterSleepBuffFlags[i];
			}
		}
		_sleepCycleImmunityRestored = true;
	}

	private bool IsSleepCycleStressPassed()
	{
		if (!EnableSleepCycleStress)
		{
			return true;
		}
		if (_sleepCycleCompleted && _sleepCyclePassed && _sleepCycleEligibleCount == SpawnCount && _sleepCycleAppliedCount == SpawnCount && _sleepCycleEnteredCount == SpawnCount)
		{
			return _sleepCycleWokenCount == SpawnCount;
		}
		return false;
	}

	private void TryAdvanceSingleEnemyRoleAttackModes()
	{
		if (!EnableSingleEnemyTargetFixture || !_singleEnemyFixturePrepared || _singleEnemyRobotModeAdvanceRequested || _spawnedCharacters.Count == 0)
		{
			return;
		}
		for (int i = 0; i < _spawnedCharacters.Count; i++)
		{
			if (!(_spawnedCharacters[i] is TowerDefensePlantRobot { componentManager: var componentManager }))
			{
				return;
			}
			FireComponent fireComponent = componentManager?.GetRuntime<FireComponent>("character.fire");
			if (fireComponent == null || !_engagedFireComponents.Contains(fireComponent))
			{
				return;
			}
		}
		_singleEnemyRobotModeAdvanceRequested = true;
		for (int j = 0; j < _spawnedCharacters.Count; j++)
		{
			((TowerDefensePlantRobot)_spawnedCharacters[j]).DoublePressed(Vector2.Zero);
		}
		GD.Print($"[TestCombatFixture] role-attack-mode-advance role={"TowerDefensePlantRobot"} sources={_spawnedCharacters.Count} " + "trigger=DoublePressed");
	}

	private bool TryAttachLegacyAttackEngagementHandler(TowerDefenseCharacter source, AttackComponent attack)
	{
		if (!(source is TowerDefenseZombie towerDefenseZombie) || towerDefenseZombie.attackComponent != attack)
		{
			return false;
		}
		StateHandle attackState = towerDefenseZombie.StateMachine?.GetStateById("zombie.attack");
		StateHandle stateHandle = attackState;
		if (stateHandle == null || !stateHandle.IsValid)
		{
			return false;
		}
		Action handler = null;
		handler = () =>
		{
			_engagedAttackComponents.Add(attack);
			attackState.Entered -= handler;
			_legacyAttackEngagementHandlers.Remove(attackState);
		};
		_legacyAttackEngagementHandlers[attackState] = handler;
		attackState.Entered += handler;
		return true;
	}

	private void DetachCombatComponentEngagementHandlers()
	{
		if (_attackEngagementHandlers.Count > 0)
		{
			foreach (KeyValuePair<AttackComponent, AttackComponent.AttackEventHandler> attackEngagementHandler in _attackEngagementHandlers)
			{
				attackEngagementHandler.Key.OnAttack -= attackEngagementHandler.Value;
			}
			_attackEngagementHandlers.Clear();
		}
		if (_legacyAttackEngagementHandlers.Count > 0)
		{
			foreach (KeyValuePair<StateHandle, Action> legacyAttackEngagementHandler in _legacyAttackEngagementHandlers)
			{
				legacyAttackEngagementHandler.Key.Entered -= legacyAttackEngagementHandler.Value;
			}
			_legacyAttackEngagementHandlers.Clear();
		}
		if (_fireEngagementHandlers.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<FireComponent, FireComponent.FireVolleyEventHandler> fireEngagementHandler in _fireEngagementHandlers)
		{
			fireEngagementHandler.Key.OnFireVolley -= fireEngagementHandler.Value;
		}
		_fireEngagementHandlers.Clear();
	}

	private bool IsCombatComponentEngagementPassed()
	{
		if (!EnableSingleEnemyTargetFixture)
		{
			return true;
		}
		if (_singleEnemyFixturePrepared && GodotObject.IsInstanceValid(_singleEnemyTarget) && _singleEnemyAttackComponentCount + _singleEnemyAttackUtilityComponentCount + _singleEnemyFireComponentCount > 0 && _engagedAttackComponents.Count == _singleEnemyAttackComponentCount && _exercisedAttackUtilityComponents.Count == _singleEnemyAttackUtilityComponentCount)
		{
			return _engagedFireComponents.Count == _singleEnemyFireComponentCount;
		}
		return false;
	}

	private int CountValidManyEnemyIdleTargets()
	{
		int num = 0;
		for (int i = 0; i < _manyEnemyIdleTargets.Count; i++)
		{
			TowerDefenseCharacter towerDefenseCharacter = _manyEnemyIdleTargets[i];
			if (GodotObject.IsInstanceValid(towerDefenseCharacter) && GodotObject.IsInstanceValid(towerDefenseCharacter.instance) && !towerDefenseCharacter.die && !towerDefenseCharacter.nearDie)
			{
				num++;
			}
		}
		return num;
	}

	private int CountManyEnemyIdleBusyStates()
	{
		int num = 0;
		for (int i = 0; i < _manyEnemyIdleAttackComponents.Count; i++)
		{
			StateHandle stateHandle = _manyEnemyIdleAttackComponents[i]?.StateMachine?.CurrentStateHandle;
			if (stateHandle != null && stateHandle.IsValid && stateHandle.StableId != "attack.idle")
			{
				num++;
			}
		}
		for (int j = 0; j < _manyEnemyIdleFireComponents.Count; j++)
		{
			FireComponent fireComponent = _manyEnemyIdleFireComponents[j];
			if (fireComponent != null && fireComponent.IsFireStateBusy())
			{
				num++;
			}
		}
		return num;
	}

	private bool IsManyEnemyIdleFixturePassed()
	{
		if (!EnableManyEnemyIdleFixture)
		{
			return true;
		}
		int num = Math.Max(1, ManyEnemyIdleTargetCount);
		if (_manyEnemyIdleFixturePrepared && _manyEnemyIdleTargets.Count == num && CountValidManyEnemyIdleTargets() == num && _manyEnemyIdleAttackComponents.Count + _manyEnemyIdleFireComponents.Count > 0 && _engagedAttackComponents.Count == 0 && _engagedFireComponents.Count == 0)
		{
			return CountManyEnemyIdleBusyStates() == 0;
		}
		return false;
	}

	private string FormatAttackComponentEngagementBreakdown(bool utilities)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.Ordinal);
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>(StringComparer.Ordinal);
		for (int i = 0; i < _spawnedCharacters.Count; i++)
		{
			ComponentManager componentManager = _spawnedCharacters[i]?.componentManager;
			if (!GodotObject.IsInstanceValid(componentManager))
			{
				continue;
			}
			IReadOnlyList<CharacterComponentRuntime> resourceComponents = componentManager.ResourceComponents;
			for (int j = 0; j < resourceComponents.Count; j++)
			{
				if (resourceComponents[j] is AttackComponent attackComponent && IsRoleUtilityAttackComponent(_spawnedCharacters[i], attackComponent) == utilities)
				{
					string text = attackComponent.ComponentDefinition?.InstanceId;
					if (string.IsNullOrWhiteSpace(text))
					{
						text = "<unknown>";
					}
					string key = text + ":" + attackComponent.attackType;
					dictionary[key] = dictionary.GetValueOrDefault(key) + 1;
					if (utilities ? _exercisedAttackUtilityComponents.Contains(attackComponent) : _engagedAttackComponents.Contains(attackComponent))
					{
						dictionary2[key] = dictionary2.GetValueOrDefault(key) + 1;
					}
				}
			}
		}
		return FormatComponentEngagementBreakdown(dictionary, dictionary2);
	}

	private string FormatFireComponentEngagementBreakdown()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.Ordinal);
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>(StringComparer.Ordinal);
		for (int i = 0; i < _spawnedCharacters.Count; i++)
		{
			ComponentManager componentManager = _spawnedCharacters[i]?.componentManager;
			if (!GodotObject.IsInstanceValid(componentManager))
			{
				continue;
			}
			IReadOnlyList<CharacterComponentRuntime> resourceComponents = componentManager.ResourceComponents;
			for (int j = 0; j < resourceComponents.Count; j++)
			{
				if (resourceComponents[j] is FireComponent { ComponentDefinition: var componentDefinition } fireComponent)
				{
					string text = componentDefinition?.InstanceId;
					if (string.IsNullOrWhiteSpace(text))
					{
						text = "<unknown>";
					}
					dictionary[text] = dictionary.GetValueOrDefault(text) + 1;
					if (_engagedFireComponents.Contains(fireComponent))
					{
						dictionary2[text] = dictionary2.GetValueOrDefault(text) + 1;
					}
				}
			}
		}
		return FormatComponentEngagementBreakdown(dictionary, dictionary2);
	}

	private static string FormatComponentEngagementBreakdown(Dictionary<string, int> totals, Dictionary<string, int> engaged)
	{
		if (totals.Count == 0)
		{
			return "none";
		}
		List<string> list = new List<string>(totals.Keys);
		list.Sort(StringComparer.Ordinal);
		string[] array = new string[list.Count];
		for (int i = 0; i < list.Count; i++)
		{
			string text = list[i];
			array[i] = $"{text}={engaged.GetValueOrDefault(text)}/{totals[text]}";
		}
		return string.Join('|', array);
	}

	private static double GetCombatTargetDurability(TowerDefenseCharacter target)
	{
		if (!GodotObject.IsInstanceValid(target?.instance))
		{
			return 0.0;
		}
		double num = Math.Max(0.0, target.instance.hitpoints);
		for (int i = 0; i < target.instance.armorList.Count; i++)
		{
			TowerDefenseArmorInstance towerDefenseArmorInstance = target.instance.armorList[i];
			if (GodotObject.IsInstanceValid(towerDefenseArmorInstance) && !towerDefenseArmorInstance.isRemove)
			{
				num += Math.Max(0.0, towerDefenseArmorInstance.hitPoints);
			}
		}
		return num;
	}

	private void PrintExactRoleComponentInventory()
	{
		if (!UseZombieCharacterScenes || _spawnedCharacters.Count == 0)
		{
			return;
		}
		ComponentManager componentManager = _spawnedCharacters[0]?.componentManager;
		if (!GodotObject.IsInstanceValid(componentManager))
		{
			return;
		}
		Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.Ordinal);
		IReadOnlyList<CharacterComponentRuntime> resourceComponents = componentManager.ResourceComponents;
		for (int i = 0; i < resourceComponents.Count; i++)
		{
			string key = resourceComponents[i]?.GetType().Name ?? "<null>";
			dictionary[key] = dictionary.GetValueOrDefault(key) + 1;
		}
		for (int j = 0; j < componentManager.componentList.Count; j++)
		{
			ComponentBase componentBase = componentManager.componentList[j];
			if (GodotObject.IsInstanceValid(componentBase))
			{
				string name = componentBase.GetType().Name;
				dictionary[name] = dictionary.GetValueOrDefault(name) + 1;
			}
		}
		List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>(dictionary);
		list.Sort((KeyValuePair<string, int> left, KeyValuePair<string, int> right) =>
		{
			int num3 = right.Value.CompareTo(left.Value);
			return (num3 == 0) ? string.CompareOrdinal(left.Key, right.Key) : num3;
		});
		string[] array = new string[list.Count];
		int num = 0;
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			num += list[num2].Value;
			array[num2] = $"{list[num2].Key}:{list[num2].Value}";
		}
		string value = (string.IsNullOrWhiteSpace(ExactScenePath) ? _spawnedCharacters[0].SceneFilePath : ExactScenePath);
		GD.Print($"[TestComponentInventory] scene={value} runtimeType={_spawnedCharacters[0].GetType().Name} total={num} types={string.Join(',', array)}");
	}

	private static void CollectAnimationSprites(Node node, List<AdobeAnimateSpriteBase> sprites)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateSpriteBase item)
		{
			sprites.Add(item);
		}
		foreach (Node child in node.GetChildren())
		{
			CollectAnimationSprites(child, sprites);
		}
	}

	private Vector2 ResolveSpawnPosition(int index)
	{
		if (EnableZInterleaveFixture)
		{
			Vector2 size = GetViewportRect().Size;
			Vector2 vector = size * 0.5f;
			float num = Mathf.Clamp(size.X * 0.22f, 150f, 230f);
			return vector + new Vector2(((index & 1) == 0) ? (0f - num) : num, 70f);
		}
		if (EnableSingleEnemyTargetFixture || EnableManyEnemyIdleFixture)
		{
			return ResolveSingleTargetSourcePosition();
		}
		if (!UseGridSpawnLayout)
		{
			return new Vector2(SpawnArea.X * GD.Randf(), SpawnArea.Y * GD.Randf());
		}
		int num2 = Math.Max(1, SpawnCount);
		float num3 = Math.Max(1f, SpawnArea.X);
		float num4 = Math.Max(1f, SpawnArea.Y);
		int num5 = ((SpawnGridColumns > 0) ? Math.Min(num2, SpawnGridColumns) : Math.Min(num2, Math.Max(1, Mathf.CeilToInt(Mathf.Sqrt((float)num2 * num3 / num4)))));
		int num6 = Math.Max(1, (num2 + num5 - 1) / num5);
		int num7 = index % num5;
		int num8 = index / num5;
		float x = ((num5 <= 1) ? (num3 * 0.5f) : (num3 * (float)num7 / (float)(num5 - 1)));
		float y = ((num6 <= 1) ? (num4 * 0.5f) : (num4 * (float)num8 / (float)(num6 - 1)));
		return SpawnOrigin + new Vector2(x, y);
	}

	private Vector2 ResolveSingleTargetSourcePosition()
	{
		return SpawnOrigin + SpawnArea * 0.5f;
	}

	private static Vector2I ResolveSingleTargetSourceGridPosition()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		Vector2I vector2I = (GodotObject.IsInstanceValid(instance) ? instance.gridNum : new Vector2I(9, 5));
		return new Vector2I(Math.Max(1, (vector2I.X + 1) / 2), Math.Max(1, (vector2I.Y + 1) / 2));
	}

	private void EnsureZInterleaveFixture()
	{
		if (!EnableZInterleaveFixture || GodotObject.IsInstanceValid(_zInterleaveMarker))
		{
			return;
		}
		if (_spawnedNodes.Count != 2)
		{
			GD.PrintErr($"[TestZInterleave] expected two animation roots but spawned={_spawnedNodes.Count}");
			return;
		}
		Vector2 position = GetViewportRect().Size * 0.5f + new Vector2(0f, 40f);
		_zInterleaveMarker = new Polygon2D
		{
			Name = "ZInterleaveMarker",
			ZAsRelative = false,
			ZIndex = 1,
			Position = position,
			Polygon = new Vector2[6]
			{
				new Vector2(-285f, -82f),
				new Vector2(-70f, -138f),
				new Vector2(276f, -72f),
				new Vector2(238f, 112f),
				new Vector2(28f, 84f),
				new Vector2(-246f, 132f)
			},
			VertexColors = new Color[6]
			{
				new Color(1f, 0f, 0.72f, 0.66f),
				new Color(1f, 0f, 0.72f, 0.66f),
				new Color(0f, 1f, 0.35f, 0.66f),
				new Color(0f, 1f, 0.35f, 0.66f),
				new Color(0f, 1f, 0.35f, 0.66f),
				new Color(1f, 0f, 0.72f, 0.66f)
			}
		};
		AddChild(_zInterleaveMarker, forceReadableName: false, InternalMode.Disabled);
		using Image image = Image.CreateEmpty(8, 8, useMipmaps: false, Image.Format.Rgba8);
		image.Fill(new Color(1f, 1f, 1f));
		_zInterleaveParticleTexture = ImageTexture.CreateFromImage(image);
		_zInterleaveParticles = new CpuParticles2D
		{
			Name = "ZInterleaveParticles",
			ZAsRelative = false,
			ZIndex = 1,
			Position = position,
			Amount = 72,
			Lifetime = 12.0,
			Preprocess = 12.0,
			OneShot = false,
			Randomness = 0f,
			LifetimeRandomness = 0.0,
			UseFixedSeed = true,
			Seed = 20260710u,
			LocalCoords = true,
			EmissionShape = CpuParticles2D.EmissionShapeEnum.Rectangle,
			EmissionRectExtents = new Vector2(270f, 112f),
			Direction = Vector2.Zero,
			Spread = 0f,
			Gravity = Vector2.Zero,
			InitialVelocityMin = 0f,
			InitialVelocityMax = 0f,
			ScaleAmountMin = 2f,
			ScaleAmountMax = 2f,
			Color = new Color(1f, 0.94f, 0.12f),
			Texture = _zInterleaveParticleTexture,
			Emitting = true
		};
		AddChild(_zInterleaveParticles, forceReadableName: false, InternalMode.Disabled);
		GD.Print("[TestZInterleave] animationZ=0,2 externalZ=1 marker=true particles=true");
	}

	private void PrintRuntimeSnapshot(string label)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		Rect2 worldViewportRect = GetWorldViewportRect();
		foreach (Node2D spawnedNode in _spawnedNodes)
		{
			if (GodotObject.IsInstanceValid(spawnedNode))
			{
				num++;
				if (spawnedNode.ProcessMode == ProcessModeEnum.Disabled)
				{
					num9++;
				}
				if (spawnedNode.IsVisibleInTree())
				{
					num2++;
				}
				if (worldViewportRect.HasPoint(spawnedNode.GlobalPosition))
				{
					num3++;
				}
			}
		}
		foreach (TowerDefenseCharacter spawnedCharacter in _spawnedCharacters)
		{
			if (GodotObject.IsInstanceValid(spawnedCharacter))
			{
				num4++;
				if (spawnedCharacter.IsVisibleInTree())
				{
					num5++;
				}
				if (worldViewportRect.HasPoint(spawnedCharacter.GlobalPosition))
				{
					num6++;
				}
			}
		}
		foreach (AdobeAnimateSpriteBase spawnedAnimationSprite in _spawnedAnimationSprites)
		{
			if (GodotObject.IsInstanceValid(spawnedAnimationSprite))
			{
				if (spawnedAnimationSprite.IsRuntimeTickPaused)
				{
					num8++;
				}
				if (spawnedAnimationSprite.IsRuntimeActive)
				{
					num7++;
				}
			}
		}
		double value = ((_windowFrames > 0) ? (_windowDeltaSeconds * 1000.0 / (double)_windowFrames) : 0.0);
		double value2 = Performance.GetMonitor(Performance.Monitor.TimeProcess) * 1000.0;
		double value3 = Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess) * 1000.0;
		RenderManagerStats renderManagerStats = CollectRenderManagerStats();
		AdobeAnimateCrowdAggregateStats aggregateRenderStats = AdobeAnimateRenderManager.GetAggregateRenderStats();
		GetRenderFrameTimes(out var renderCpuMs, out var renderGpuMs);
		double monitor = Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame);
		CollectExternalVisualStats(out var activeVisible, out var nativeFallback, out var expectedVisible);
		long value4 = Math.Max(0L, AdobeAnimateRenderManager.GpuRenderGraphBuildCount - _externalVisualGraphBuildCountAtMeasurementStart);
		TowerDefensePerfProfiler.Sample("adobeAnimate.externalVisual.active", activeVisible);
		TowerDefensePerfProfiler.Sample("adobeAnimate.externalVisual.nativeFallback", nativeFallback);
		GD.Print($"[TestPerf] {label} t={_uptime.Elapsed.TotalSeconds:F1}s fps={Engine.GetFramesPerSecond():F1} maxFps={Engine.MaxFps} godotProcessMs={value2:F3} godotPhysicsMs={value3:F3} windowDeltaMs={value:F3} renderCpuMs={renderCpuMs:F3} renderGpuMs={renderGpuMs:F3} drawCalls={monitor:F0} spawned={_spawnedNodes.Count} valid={num} characters={_spawnedCharacters.Count} validCharacters={num4} animationSprites={_spawnedAnimationSprites.Count} runtimeActive={num7} paused={num8} processDisabled={num9} visibleInTree={num2} pointInViewport={num3} visibleCharacters={num5} pointCharacters={num6} worldViewport={FormatRect(worldViewportRect)} renderManagers={renderManagerStats.RenderManagers} normalBuckets={renderManagerStats.NormalBuckets} visibleInstances={renderManagerStats.VisibleInstances} allocatedInstances={renderManagerStats.AllocatedInstances} maxBucketVisible={renderManagerStats.MaxBucketVisible} compactCrowdRoots={renderManagerStats.CompactCrowdRoots} compositeCrowdRoots={renderManagerStats.CompositeCrowdRoots} gpuGraphRoots={renderManagerStats.GpuGraphRoots} gpuGraphSlots={renderManagerStats.GpuGraphSlots} gpuGraphAtlasPages={AdobeAnimateRenderManager.GpuRenderGraphAtlasPageCount} gpuGraphStateTexels={renderManagerStats.GpuGraphStateTexels} crowdStateTexels={renderManagerStats.CrowdStateTexels} activeMounts={aggregateRenderStats.ActiveMounts} activeZBuckets={aggregateRenderStats.ActiveZBuckets} crowdRoots={aggregateRenderStats.CrowdRoots} sharedCrowdStateTexels={aggregateRenderStats.CrowdStateTexels} resourceSignatures={aggregateRenderStats.ResourceSignatures} stateTextureUploads={aggregateRenderStats.StateTextureUploads} multiMeshUploads={aggregateRenderStats.MultiMeshUploads} rdBatches={aggregateRenderStats.RdAppliedBatches}/{aggregateRenderStats.RdSubmittedBatches} rdUploads={aggregateRenderStats.RdAppliedUploads}/{aggregateRenderStats.RdQueuedUploads} rdUploadedBytes={aggregateRenderStats.RdUploadedBytes} rdRidRefreshes={aggregateRenderStats.RdBufferRidRefreshes} rdFailures={aggregateRenderStats.RdFailedBatches}/{aggregateRenderStats.RdBufferUpdateFailures}/{aggregateRenderStats.RdInvalidTargets} rdStaleDrops={aggregateRenderStats.RdStaleDrops} rdQueueDepth={aggregateRenderStats.RdQueueDepth}/{aggregateRenderStats.RdMaximumQueueDepth} rdFrames={aggregateRenderStats.RdLastAppliedFrameVersion}/{aggregateRenderStats.RdLastQueuedFrameVersion} signatureConflictBuckets={aggregateRenderStats.SignatureConflictBuckets} fallbackRuns={aggregateRenderStats.FallbackRuns} fallbackRoots={aggregateRenderStats.FallbackRoots} arenaGrowthCount={aggregateRenderStats.ArenaGrowthCount} crowdMeshQuadCapacity={aggregateRenderStats.CrowdMeshQuadCapacity} crowdMeshVersion={aggregateRenderStats.CrowdMeshVersion} meshRebinds={aggregateRenderStats.MeshRebinds} renderBackend={GetTestRenderBackendLabel()} cpuRoots={aggregateRenderStats.CpuRoots} cpuFallbackRoots={aggregateRenderStats.CpuFallbackRoots} cpuValidationFailures={aggregateRenderStats.CpuValidationFailures} cpuMeshRebuilds={aggregateRenderStats.CpuMeshRebuilds} cpuVertexUploads={aggregateRenderStats.CpuVertexUploads} cpuUploadedVertices={aggregateRenderStats.CpuUploadedVertices} cpuCapacityGrowths={aggregateRenderStats.CpuCapacityGrowths} cpuSkippedUploads={aggregateRenderStats.CpuSkippedUploads}");
		GD.Print($"[TestExternalVisual] sample={label} enabled={EnableExternalVisualStress} ratio={ExternalVisualRatio:F3} requested={_externalVisualExpectedActive} expected={expectedVisible} externalVisualActive={activeVisible} externalVisualFallback={nativeFallback} externalVisualGraphRebuilds={value4}");
		PrintExternalVisualPositionDiagnostics(label);
		PrintCrowdAggregateStats(label, aggregateRenderStats);
	}

	private void PrintExternalVisualPositionDiagnostics(string label)
	{
		if (!EnableExternalVisualStress || _externalVisualStressFixtures.Count > 16)
		{
			return;
		}
		for (int i = 0; i < _externalVisualStressFixtures.Count; i++)
		{
			ExternalVisualStressFixture externalVisualStressFixture = _externalVisualStressFixtures[i];
			if (GodotObject.IsInstanceValid(externalVisualStressFixture.Visual) && GodotObject.IsInstanceValid(externalVisualStressFixture.Owner))
			{
				Vector2 value = (GodotObject.IsInstanceValid(externalVisualStressFixture.Character?.shadowSprite) ? externalVisualStressFixture.Character.shadowSprite.GlobalPosition : new Vector2(0f / 0f, 0f / 0f));
				Vector2 value2 = (GodotObject.IsInstanceValid(externalVisualStressFixture.Character?.headSlot) ? externalVisualStressFixture.Character.headSlot.GlobalPosition : new Vector2(0f / 0f, 0f / 0f));
				Vector2 value3 = new Vector2(0f / 0f, 0f / 0f);
				AdobeAnimateExternalVisualSnapshot visual2;
				if (externalVisualStressFixture.Handle.IsValid && externalVisualStressFixture.Owner.TryGetExternalVisualForRender(externalVisualStressFixture.Handle, out var visual))
				{
					value3 = visual.Transform.Origin;
				}
				else if (externalVisualStressFixture.Character != null && externalVisualStressFixture.Visual == externalVisualStressFixture.Character.icetrapSprite && externalVisualStressFixture.Character.TryGetIceTrapExternalVisualForDiagnostics(out visual2))
				{
					value3 = visual2.Transform.Origin;
				}
				GD.Print($"[TestExternalVisualPosition] sample={label} index={externalVisualStressFixture.StableIndex} owner={FormatVector(externalVisualStressFixture.Owner.GlobalPosition)} visual={FormatVector(externalVisualStressFixture.Visual.GlobalPosition)} shadow={FormatVector(value)} head={FormatVector(value2)} crowdLocal={FormatVector(value3)}");
			}
		}
	}

	private static string FormatVector(Vector2 value)
	{
		return $"({value.X:F2},{value.Y:F2})";
	}

	private void CollectExternalVisualStats(out int activeVisible, out int nativeFallback, out int expectedVisible)
	{
		activeVisible = 0;
		nativeFallback = 0;
		expectedVisible = 0;
		for (int i = 0; i < _externalVisualStressFixtures.Count; i++)
		{
			Sprite2D visual = _externalVisualStressFixtures[i].Visual;
			if (GodotObject.IsInstanceValid(visual) && AdobeAnimateManagedSprite2D.GetLogicalVisible(visual))
			{
				expectedVisible++;
				activeVisible++;
				if (!AdobeAnimateManagedSprite2D.IsCrowdManaged(visual))
				{
					nativeFallback++;
				}
			}
		}
	}

	private void ReportExternalVisualStressFailures()
	{
		for (int i = 0; i < _externalVisualStressFixtures.Count; i++)
		{
			ExternalVisualStressFixture externalVisualStressFixture = _externalVisualStressFixtures[i];
			if (GodotObject.IsInstanceValid(externalVisualStressFixture.Visual) && !AdobeAnimateManagedSprite2D.IsCrowdManaged(externalVisualStressFixture.Visual))
			{
				GD.PrintErr($"[TestExternalVisual] crowd-fallback index={externalVisualStressFixture.StableIndex} scene={externalVisualStressFixture.Character?.SceneFilePath ?? externalVisualStressFixture.Owner?.SceneFilePath} sprite={externalVisualStressFixture.Owner?.SceneFilePath} texture={externalVisualStressFixture.Visual.Texture?.ResourcePath}");
				if (GodotObject.IsInstanceValid(externalVisualStressFixture.Owner))
				{
					GD.Print(externalVisualStressFixture.Owner.BuildCrowdFilterDebugReport($"external-visual-fallback:{externalVisualStressFixture.StableIndex}", 32));
				}
			}
		}
	}

	private static void PrintCrowdAggregateStats(string label, AdobeAnimateCrowdAggregateStats crowdStats)
	{
		GD.Print($"[TestCrowdStats] activeMounts={crowdStats.ActiveMounts} activeZBuckets={crowdStats.ActiveZBuckets} crowdRoots={crowdStats.CrowdRoots} sharedCrowdStateTexels={crowdStats.CrowdStateTexels} resourceSignatures={crowdStats.ResourceSignatures} stateTextureUploads={crowdStats.StateTextureUploads} multiMeshUploads={crowdStats.MultiMeshUploads} signatureConflictBuckets={crowdStats.SignatureConflictBuckets} fallbackRuns={crowdStats.FallbackRuns} fallbackRoots={crowdStats.FallbackRoots} arenaGrowthCount={crowdStats.ArenaGrowthCount} crowdMeshQuadCapacity={crowdStats.CrowdMeshQuadCapacity} crowdMeshVersion={crowdStats.CrowdMeshVersion} meshRebinds={crowdStats.MeshRebinds} sample={label}");
	}

	private double GetAverageMeasuredFps()
	{
		double measuredSeconds = GetMeasuredSeconds();
		if (!(measuredSeconds > 0.0))
		{
			return 0.0;
		}
		return (double)_measurementFrames / measuredSeconds;
	}

	private bool IsSelectedBackendAccepted(in AdobeAnimateCrowdAggregateStats crowdStats)
	{
		if (_testRenderBackend == AdobeAnimateRenderBackend.CpuPose)
		{
			if (crowdStats.CpuFallbackRoots == 0 && crowdStats.CpuValidationFailures == 0 && crowdStats.FallbackRoots == 0)
			{
				return crowdStats.CpuRoots == crowdStats.CrowdRoots;
			}
			return false;
		}
		return true;
	}

	private void GetRenderFrameTimes(out double renderCpuMs, out double renderGpuMs)
	{
		renderCpuMs = RenderingServer.GetFrameSetupTimeCpu();
		renderGpuMs = 0.0;
		Viewport viewport = GetViewport();
		if (GodotObject.IsInstanceValid(viewport))
		{
			Rid viewportRid = viewport.GetViewportRid();
			renderCpuMs += RenderingServer.ViewportGetMeasuredRenderTimeCpu(viewportRid);
			renderGpuMs = RenderingServer.ViewportGetMeasuredRenderTimeGpu(viewportRid);
		}
	}

	private double GetMeasuredSeconds()
	{
		if (!_measurementStarted)
		{
			return 0.0;
		}
		double num = (double.IsNaN(_measurementEndSeconds) ? _uptime.Elapsed.TotalSeconds : _measurementEndSeconds);
		return Math.Max(0.0, num - _measurementStartSeconds);
	}

	private bool PrintPerformanceResult()
	{
		if (_resultPrinted)
		{
			return _performanceResultPassed;
		}
		_resultPrinted = true;
		double measuredSeconds = GetMeasuredSeconds();
		double averageMeasuredFps = GetAverageMeasuredFps();
		long num = (_measurementStarted ? (GC.GetAllocatedBytesForCurrentThread() - _measurementThreadAllocatedBefore) : 0);
		long num2 = (_measurementStarted ? (GC.GetTotalAllocatedBytes() - _measurementTotalAllocatedBefore) : 0);
		int value = (_measurementStarted ? (GC.CollectionCount(0) - _measurementGen0Before) : 0);
		int value2 = (_measurementStarted ? (GC.CollectionCount(1) - _measurementGen1Before) : 0);
		int value3 = (_measurementStarted ? (GC.CollectionCount(2) - _measurementGen2Before) : 0);
		double[] array = _measurementFrameSeconds.ToArray();
		double[] array2 = _measurementSimulationDeltaSeconds.ToArray();
		double[] array3 = _measurementProcessMilliseconds.ToArray();
		double[] array4 = _measurementPhysicsMilliseconds.ToArray();
		double[] array5 = _spawnInstantiateMilliseconds.ToArray();
		double[] array6 = _spawnAddChildReadyMilliseconds.ToArray();
		double[] array7 = _spawnBatchFrameMilliseconds.ToArray();
		double num3 = BenchmarkStatistics.Percentile(array, array.Length, 50.0) * 1000.0;
		double num4 = BenchmarkStatistics.Percentile(array, array.Length, 95.0) * 1000.0;
		double num5 = BenchmarkStatistics.Percentile(array, array.Length, 99.0) * 1000.0;
		double num6 = BenchmarkStatistics.Maximum(array, array.Length) * 1000.0;
		double value4 = BenchmarkStatistics.Percentile(array2, array2.Length, 99.0) * 1000.0;
		double num7 = BenchmarkStatistics.WorstFractionFps(array, array.Length, 0.01);
		double value5 = BenchmarkStatistics.WorstFractionFps(array, array.Length, 0.001);
		double value6 = BenchmarkStatistics.Percentile(array3, array3.Length, 50.0);
		double value7 = BenchmarkStatistics.Percentile(array3, array3.Length, 95.0);
		double value8 = BenchmarkStatistics.Percentile(array3, array3.Length, 99.0);
		double value9 = BenchmarkStatistics.Maximum(array3, array3.Length);
		double value10 = BenchmarkStatistics.Percentile(array4, array4.Length, 50.0);
		double value11 = BenchmarkStatistics.Percentile(array4, array4.Length, 95.0);
		double value12 = BenchmarkStatistics.Percentile(array4, array4.Length, 99.0);
		double value13 = BenchmarkStatistics.Maximum(array4, array4.Length);
		double value14 = BenchmarkStatistics.Percentile(array5, array5.Length, 50.0);
		double value15 = BenchmarkStatistics.Percentile(array5, array5.Length, 95.0);
		double value16 = BenchmarkStatistics.Maximum(array5, array5.Length);
		double value17 = BenchmarkStatistics.Percentile(array6, array6.Length, 50.0);
		double value18 = BenchmarkStatistics.Percentile(array6, array6.Length, 95.0);
		double value19 = BenchmarkStatistics.Maximum(array6, array6.Length);
		double value20 = BenchmarkStatistics.Percentile(array7, array7.Length, 95.0);
		double num8 = BenchmarkStatistics.Maximum(array7, array7.Length);
		double[] array8 = _viewportEntryFrameSeconds.ToArray();
		double value21 = BenchmarkStatistics.Percentile(array8, array8.Length, 95.0) * 1000.0;
		double value22 = BenchmarkStatistics.Maximum(array8, array8.Length) * 1000.0;
		int num9 = 0;
		for (int i = 0; i < _spawnedNodes.Count; i++)
		{
			Node2D node2D = _spawnedNodes[i];
			if (GodotObject.IsInstanceValid(node2D))
			{
				num9 += CountStateChartRelatedNodes(node2D);
			}
		}
		double monitor = Performance.GetMonitor(Performance.Monitor.MemoryStatic);
		double monitor2 = Performance.GetMonitor(Performance.Monitor.MemoryStaticMax);
		double monitor3 = Performance.GetMonitor(Performance.Monitor.MemoryMessageBufferMax);
		double monitor4 = Performance.GetMonitor(Performance.Monitor.RenderVideoMemUsed);
		double monitor5 = Performance.GetMonitor(Performance.Monitor.RenderTextureMemUsed);
		double monitor6 = Performance.GetMonitor(Performance.Monitor.RenderBufferMemUsed);
		double monitor7 = Performance.GetMonitor(Performance.Monitor.ObjectCount);
		double monitor8 = Performance.GetMonitor(Performance.Monitor.ObjectNodeCount);
		double monitor9 = Performance.GetMonitor(Performance.Monitor.ObjectOrphanNodeCount);
		double monitor10 = Performance.GetMonitor(Performance.Monitor.ObjectResourceCount);
		double monitor11 = Performance.GetMonitor(Performance.Monitor.RenderTotalDrawCallsInFrame);
		GetRenderFrameTimes(out var renderCpuMs, out var renderGpuMs);
		double monitor12 = Performance.GetMonitor(Performance.Monitor.RenderTotalObjectsInFrame);
		double monitor13 = Performance.GetMonitor(Performance.Monitor.RenderTotalPrimitivesInFrame);
		CollectExternalVisualStats(out var activeVisible, out var nativeFallback, out var expectedVisible);
		long value23 = Math.Max(0L, AdobeAnimateRenderManager.GpuRenderGraphBuildCount - _externalVisualGraphBuildCountAtMeasurementStart);
		long value24 = Math.Max(0L, AdobeAnimateRenderManager.GpuRenderGraphInitialBuildCount - _gpuGraphInitialBuildCountAtMeasurementStart);
		long value25 = Math.Max(0L, AdobeAnimateRenderManager.GpuRenderGraphRebuildCount - _gpuGraphRebuildCountAtMeasurementStart);
		long value26 = Math.Max(0L, AdobeAnimateRenderManager.GpuRenderGraphOwnerExitInvalidationCount - _gpuGraphOwnerExitInvalidationCountAtMeasurementStart);
		long value27 = Math.Max(0L, AdobeAnimateRenderManager.GpuRenderGraphManagedSlotInvalidationCount - _gpuGraphManagedSlotInvalidationCountAtMeasurementStart);
		long value28 = Math.Max(0L, AdobeAnimateRenderManager.GpuRenderGraphExternalVisualInvalidationCount - _gpuGraphExternalVisualInvalidationCountAtMeasurementStart);
		AdobeAnimateCrowdAggregateStats crowdStats = AdobeAnimateRenderManager.GetAggregateRenderStats();
		if (EnableExternalVisualStress)
		{
			ReportExternalVisualStressFailures();
		}
		bool flag = IsAnimationCadencePassed();
		bool flag2 = !EnableExternalVisualStress || (activeVisible == expectedVisible && nativeFallback == 0 && _externalVisualConfigurationFailures == 0);
		bool flag3 = _spawnFrameBudgetPassed && (SpawnTimeBudgetMilliseconds <= 0.0 || array7.Length == 0 || num8 <= TowerDefenseCharacterSpawnBudget.ClampFrameBudget(SpawnTimeBudgetMilliseconds));
		int num10 = CountValidSpawnedCharacters();
		int value29 = CountValidManyEnemyIdleTargets();
		int value30 = CountManyEnemyIdleBusyStates();
		bool flag4 = !PreserveCombatCharactersInStress || (_spawnedCharacters.Count == SpawnCount && num10 == SpawnCount);
		bool value31 = GodotObject.IsInstanceValid(ResourceManager.Instance) && ResourceManager.Instance.AreFullGameplayResourcesReady;
		double value32 = (GodotObject.IsInstanceValid(ResourceManager.Instance) ? ResourceManager.Instance.FullGameplayResourceLoadMetrics.FullWallMilliseconds : 0.0);
		bool flag5 = IsAnimationSwitchStressPassed();
		bool flag6 = IsBowlingImpactTweenStressPassed();
		bool flag7 = MaximumFrameP99Milliseconds <= 0.0 || num5 <= MaximumFrameP99Milliseconds;
		bool flag8 = MaximumFrameMilliseconds <= 0.0 || num6 <= MaximumFrameMilliseconds;
		bool flag9 = MinimumOnePercentLowFps <= 0.0 || num7 >= MinimumOnePercentLowFps;
		bool value33 = (_performanceResultPassed = (((((MinimumAverageFps <= 0.0 || averageMeasuredFps >= MinimumAverageFps) && HasValidMeasurementTelemetry() && double.IsFinite(num3) && double.IsFinite(num4) && double.IsFinite(num5) && double.IsFinite(num6)) & flag7 & flag8 & flag9) && num >= 0 && num2 >= 0) & flag & flag2 & flag3 & flag4 & flag5 & flag6) && IsCombatComponentEngagementPassed() && IsSleepCycleStressPassed() && IsManyEnemyIdleFixturePassed() && IsSelectedBackendAccepted(in crowdStats));
		GD.Print($"[TestPerfResult] runtimeProfile={RuntimeProfile} spawn={_spawnedNodes.Count} averageFps={averageMeasuredFps:F2} onePercentLowFps={num7:F2} zeroPointOnePercentLowFps={value5:F2} frameClock=wall physicsHz={Engine.PhysicsTicksPerSecond} renderTargetFps={MinimumAverageFps:F2} frameP50Ms={num3:F3} frameP95Ms={num4:F3} frameP99Ms={num5:F3} frameMaxMs={num6:F3} simulationDeltaP99Ms={value4:F3} targetFrameP99Ms={MaximumFrameP99Milliseconds:F3} targetFrameMaxMs={MaximumFrameMilliseconds:F3} targetOnePercentLowFps={MinimumOnePercentLowFps:F2} frameP99Passed={flag7} frameMaxPassed={flag8} onePercentLowPassed={flag9} processP50Ms={value6:F3} processP95Ms={value7:F3} processP99Ms={value8:F3} processMaxMs={value9:F3} physicsP50Ms={value10:F3} physicsP95Ms={value11:F3} physicsP99Ms={value12:F3} physicsMaxMs={value13:F3} staticMemoryBytes={monitor:F0} staticMemoryPeakBytes={monitor2:F0} messageBufferPeakBytes={monitor3:F0} videoMemoryBytes={monitor4:F0} textureMemoryBytes={monitor5:F0} bufferMemoryBytes={monitor6:F0} objectCount={monitor7:F0} nodeCount={monitor8:F0} orphanNodeCount={monitor9:F0} resourceCount={monitor10:F0} drawCalls={monitor11:F0} renderCpuMs={renderCpuMs:F3} renderGpuMs={renderGpuMs:F3} renderBackend={GetTestRenderBackendLabel()} cpuRoots={crowdStats.CpuRoots} combatStateStress={EnableCombatStateStress} preserveCombatCharacters={PreserveCombatCharactersInStress} singleEnemyTarget={EnableSingleEnemyTargetFixture} singleEnemyTargetValid={GodotObject.IsInstanceValid(_singleEnemyTarget)} singleEnemyTargetDamage={Math.Max(0.0, _singleEnemyTargetInitialHitpoints - GetCombatTargetDurability(_singleEnemyTarget)):F3} attackComponentsEngaged={_engagedAttackComponents.Count}/{_singleEnemyAttackComponentCount} attackUtilityComponentsExercised={_exercisedAttackUtilityComponents.Count}/{_singleEnemyAttackUtilityComponentCount} fireComponentsEngaged={_engagedFireComponents.Count}/{_singleEnemyFireComponentCount} attackComponentEngagementBreakdown={FormatAttackComponentEngagementBreakdown(utilities: false)} attackUtilityComponentBreakdown={FormatAttackComponentEngagementBreakdown(utilities: true)} fireComponentEngagementBreakdown={FormatFireComponentEngagementBreakdown()} combatComponentEngagementPassed={IsCombatComponentEngagementPassed()} sleepCycleStress={EnableSleepCycleStress} sleepCycleEligible={_sleepCycleEligibleCount}/{SpawnCount} sleepCycleApplied={_sleepCycleAppliedCount}/{_sleepCycleEligibleCount} sleepCycleEntered={_sleepCycleEnteredCount}/{_sleepCycleEligibleCount} sleepCycleWoken={_sleepCycleWokenCount}/{_sleepCycleEligibleCount} sleepCycleNormalizedImmunity={_sleepCycleNormalizedImmunityCount} sleepCyclePassed={IsSleepCycleStressPassed()} manyEnemyIdle={EnableManyEnemyIdleFixture} manyEnemyIdleOutsideRange={ManyEnemyIdleTargetsOutsideAttackRange} manyEnemyIdleTargets={value29}/{Math.Max(1, ManyEnemyIdleTargetCount)} manyEnemyIdleUnexpectedAttacks={_engagedAttackComponents.Count + _engagedFireComponents.Count} manyEnemyIdleBusyStates={value30}/{_manyEnemyIdleAttackComponents.Count + _manyEnemyIdleFireComponents.Count} manyEnemyIdlePassed={IsManyEnemyIdleFixturePassed()} validCharactersAtResult={num10} invalidCharacterScenes={FormatInvalidSpawnedCharacterScenes()} preservedCharacterCountPassed={flag4} fullGameplayResourcesReady={value31} fullGameplayLoadMs={value32:F3} showHealthStress={ShowCharacterHealthInStress} healthDrawDisplays={TowerDefenseHealthDisplayBatch.ActiveDisplayCount} healthDrawLayers={TowerDefenseHealthDisplayBatch.ActiveLayerCount} healthDrawRebuilds={TowerDefenseHealthDisplayBatch.DrawRebuildCount} healthTextLayouts={TowerDefenseHealthDisplayBatch.CachedTextLineCount} healthTextLayoutBuilds={TowerDefenseHealthDisplayBatch.TextLayoutBuildCount} cpuFallbackRoots={crowdStats.CpuFallbackRoots} cpuValidationFailures={crowdStats.CpuValidationFailures} cpuMeshRebuilds={crowdStats.CpuMeshRebuilds} cpuVertexUploads={crowdStats.CpuVertexUploads} cpuUploadedVertices={crowdStats.CpuUploadedVertices} cpuCapacityGrowths={crowdStats.CpuCapacityGrowths} cpuSkippedUploads={crowdStats.CpuSkippedUploads} renderObjects={monitor12:F0} renderPrimitives={monitor13:F0} threadAllocatedBytes={num} totalAllocatedBytes={num2} gen0={value} gen1={value2} gen2={value3} stateChartNodeCount={num9} instantiateP50Ms={value14:F3} instantiateP95Ms={value15:F3} instantiateMaxMs={value16:F3} addChildReadyP50Ms={value17:F3} addChildReadyP95Ms={value18:F3} addChildReadyMaxMs={value19:F3} instantiateMaxScene={_spawnInstantiateMaxScene} addChildReadyMaxScene={_spawnAddChildReadyMaxScene} spawnBatchFrames={array7.Length} spawnBatchFrameP95Ms={value20:F3} spawnBatchFrameMaxMs={num8:F3} spawnFrameBudgetPassed={flag3} viewportEntryFrames={array8.Length} viewportEntryFrameP95Ms={value21:F3} viewportEntryFrameMaxMs={value22:F3} externalVisualActive={activeVisible} externalVisualExpected={expectedVisible} externalVisualRequested={_externalVisualExpectedActive} externalVisualFallback={nativeFallback} externalVisualConfigurationFailures={_externalVisualConfigurationFailures} externalVisualGraphRebuilds={value23} gpuGraphInitialBuilds={value24} gpuGraphRebuilds={value25} gpuGraphOwnerExitInvalidations={value26} gpuGraphManagedSlotInvalidations={value27} gpuGraphExternalVisualInvalidations={value28} animationSwitchStress={EnableAnimationSwitchStress} animationSwitchStressPassed={flag5} bowlingImpactTweenStress={EnableBowlingImpactTweenStress} bowlingImpactTweenStressPassed={flag6} targetFps={MinimumAverageFps:F2} passed={value33} warmupSeconds={WarmupSeconds:F2} measuredSeconds={measuredSeconds:F2} frames={_measurementFrames} screenshotSaved={_screenshotSaved}");
		PrintAnimationSwitchStressResult(flag5);
		PrintBowlingImpactTweenStressResult(flag6);
		if (EnableAllocationTelemetry)
		{
			GD.Print($"[TestAllocationBreakdown] runtime={GetAllocationMetric(TowerDefenseAllocationMetric.RuntimeProcess)} animationDisplay={GetAllocationMetric(TowerDefenseAllocationMetric.AnimationDisplay)} animationLogic={GetAllocationMetric(TowerDefenseAllocationMetric.AnimationLogic)} renderDispatch={GetAllocationMetric(TowerDefenseAllocationMetric.RenderDispatch)} collect={GetAllocationMetric(TowerDefenseAllocationMetric.RenderCollect)} encode={GetAllocationMetric(TowerDefenseAllocationMetric.RenderEncode)} freeze={GetAllocationMetric(TowerDefenseAllocationMetric.RenderFreeze)} publish={GetAllocationMetric(TowerDefenseAllocationMetric.RenderPublish)} stateUpload={GetAllocationMetric(TowerDefenseAllocationMetric.StateTextureUpload)} bucketPublish={GetAllocationMetric(TowerDefenseAllocationMetric.BucketPublish)} multiMeshUpload={GetAllocationMetric(TowerDefenseAllocationMetric.MultiMeshUpload)} externalVisualCommit={GetAllocationMetric(TowerDefenseAllocationMetric.ExternalVisualCommit)} componentPhysics={GetAllocationMetric(TowerDefenseAllocationMetric.ComponentPhysics)} characterPhysics={GetAllocationMetric(TowerDefenseAllocationMetric.CharacterPhysics)}");
		}
		if (RunAnimationCadenceRegression)
		{
			double value34 = ((_cadenceSamples > 0) ? ((double)_cadenceAdvances / (double)_cadenceSamples) : 0.0);
			double value35 = ((_cadenceSamples > 0) ? ((double)_cadenceSlotAdvances / (double)_cadenceSamples) : 0.0);
			GD.Print($"[TestAnimationCadenceResult] samples={_cadenceSamples} advances={_cadenceAdvances} ratio={value34:F4} slotAdvances={_cadenceSlotAdvances} slotRatio={value35:F4} trueFrameRate={_cadenceSprite?.trueFrameRate ?? 0.0:F2} passed={flag}");
		}
		return _performanceResultPassed;
	}

	private bool IsAnimationSwitchStressPassed()
	{
		if (EnableAnimationSwitchStress)
		{
			if (_animationSwitchCount > 0 && _animationSwitchAppliedCount > 0)
			{
				return _animationSwitchMissingClipCount == 0;
			}
			return false;
		}
		return true;
	}

	private void PrintAnimationSwitchStressResult(bool passed)
	{
		if (EnableAnimationSwitchStress)
		{
			double[] array = _animationSwitchWindowFrameSeconds.ToArray();
			double[] array2 = _animationSwitchSteadyFrameSeconds.ToArray();
			double value = BenchmarkStatistics.Average(array, array.Length) * 1000.0;
			double value2 = BenchmarkStatistics.Percentile(array, array.Length, 95.0) * 1000.0;
			double value3 = BenchmarkStatistics.Maximum(array, array.Length) * 1000.0;
			double value4 = BenchmarkStatistics.Average(array2, array2.Length) * 1000.0;
			double value5 = BenchmarkStatistics.Percentile(array2, array2.Length, 95.0) * 1000.0;
			double value6 = BenchmarkStatistics.Maximum(array2, array2.Length) * 1000.0;
			double value7 = ((_animationSwitchCount > 0) ? (_animationSwitchSynchronousTotalMilliseconds / (double)_animationSwitchCount) : 0.0);
			GD.Print($"[TestAnimationSwitchResult] switches={_animationSwitchCount} applied={_animationSwitchAppliedCount} missingClip={_animationSwitchMissingClipCount} syncAverageMs={value7:F3} syncMaxMs={_animationSwitchSynchronousMaxMilliseconds:F3} switchFrames={array.Length} switchAverageMs={value:F3} switchP95Ms={value2:F3} switchMaxMs={value3:F3} steadyFrames={array2.Length} steadyAverageMs={value4:F3} steadyP95Ms={value5:F3} steadyMaxMs={value6:F3} blend={AnimationSwitchBlendSeconds:F3} passed={passed}");
		}
	}

	private bool IsBowlingImpactTweenStressPassed()
	{
		if (EnableBowlingImpactTweenStress)
		{
			if (_bowlingImpactBurstCount > 0 && _bowlingImpactAppliedCount > 0 && _bowlingImpactMissingShieldCount == 0 && (!BowlingImpactUseProductionEvent || _bowlingImpactMaxBatchedMotions > 0) && (!BowlingImpactEnsureShield || (_bowlingImpactMaxBatchedShieldVisuals > 0 && TowerDefenseShieldImpactBatch.ActiveCount == 0)))
			{
				if (BowlingImpactProcessedTweenLimit >= 0)
				{
					return _bowlingImpactMaxProcessedTweens <= BowlingImpactProcessedTweenLimit;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	private void PrintBowlingImpactTweenStressResult(bool passed)
	{
		if (EnableBowlingImpactTweenStress)
		{
			double[] array = _bowlingImpactWindowFrameSeconds.ToArray();
			double[] array2 = _bowlingImpactSteadyFrameSeconds.ToArray();
			double value = BenchmarkStatistics.Average(array, array.Length) * 1000.0;
			double value2 = BenchmarkStatistics.Percentile(array, array.Length, 95.0) * 1000.0;
			double value3 = BenchmarkStatistics.Percentile(array, array.Length, 99.0) * 1000.0;
			double value4 = BenchmarkStatistics.Maximum(array, array.Length) * 1000.0;
			double value5 = BenchmarkStatistics.Average(array2, array2.Length) * 1000.0;
			double value6 = BenchmarkStatistics.Percentile(array2, array2.Length, 95.0) * 1000.0;
			double value7 = BenchmarkStatistics.Percentile(array2, array2.Length, 99.0) * 1000.0;
			double value8 = BenchmarkStatistics.Maximum(array2, array2.Length) * 1000.0;
			double value9 = ((_bowlingImpactBurstCount > 0) ? (_bowlingImpactSynchronousTotalMilliseconds / (double)_bowlingImpactBurstCount) : 0.0);
			double value10 = ((_bowlingImpactAppliedCount > 0) ? (_bowlingImpactSynchronousTotalMilliseconds * 1000.0 / (double)_bowlingImpactAppliedCount) : 0.0);
			double value11 = ((_bowlingImpactAppliedCount > 0) ? ((double)_bowlingImpactThreadAllocatedBytes / (double)_bowlingImpactAppliedCount) : 0.0);
			GD.Print($"[TestBowlingImpactTweenResult] bursts={_bowlingImpactBurstCount} applied={_bowlingImpactAppliedCount} missingShield={_bowlingImpactMissingShieldCount} syncAverageMs={value9:F3} syncMaxMs={_bowlingImpactSynchronousMaxMilliseconds:F3} syncPerImpactUs={value10:F3} threadAllocatedBytes={_bowlingImpactThreadAllocatedBytes} allocatedBytesPerImpact={value11:F2} maxProcessedTweens={_bowlingImpactMaxProcessedTweens} processedTweenLimit={BowlingImpactProcessedTweenLimit} maxBatchedMotions={_bowlingImpactMaxBatchedMotions} maxBatchedShieldVisuals={_bowlingImpactMaxBatchedShieldVisuals} remainingBatchedShieldVisuals={TowerDefenseShieldImpactBatch.ActiveCount} productionEvent={BowlingImpactUseProductionEvent} impactFrames={array.Length} impactAverageMs={value:F3} impactP95Ms={value2:F3} impactP99Ms={value3:F3} impactMaxMs={value4:F3} steadyFrames={array2.Length} steadyAverageMs={value5:F3} steadyP95Ms={value6:F3} steadyP99Ms={value7:F3} steadyMaxMs={value8:F3} passed={passed}");
		}
	}

	private int CountValidSpawnedCharacters()
	{
		int num = 0;
		for (int i = 0; i < _spawnedCharacters.Count; i++)
		{
			if (GodotObject.IsInstanceValid(_spawnedCharacters[i]))
			{
				num++;
			}
		}
		return num;
	}

	private string FormatInvalidSpawnedCharacterScenes()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.Ordinal);
		int num = Math.Min(_spawnedCharacters.Count, _spawnedCharacterScenePaths.Count);
		for (int i = 0; i < num; i++)
		{
			if (!GodotObject.IsInstanceValid(_spawnedCharacters[i]))
			{
				string text = _spawnedCharacterScenePaths[i];
				if (string.IsNullOrWhiteSpace(text))
				{
					text = "<unknown>";
				}
				dictionary[text] = dictionary.GetValueOrDefault(text) + 1;
			}
		}
		if (dictionary.Count == 0)
		{
			return "none";
		}
		List<string> list = new List<string>(dictionary.Keys);
		list.Sort(StringComparer.Ordinal);
		string[] array = new string[list.Count];
		for (int j = 0; j < list.Count; j++)
		{
			string text2 = list[j];
			array[j] = $"{text2}:{dictionary[text2]}";
		}
		return string.Join('|', array);
	}

	private static int CountStateChartRelatedNodes(Node node)
	{
		bool flag = ((node is StateChart || node is StateChartState || node is Transition) ? true : false);
		int num = (flag ? 1 : 0);
		foreach (Node child in node.GetChildren())
		{
			num += CountStateChartRelatedNodes(child);
		}
		return num;
	}

	private static string GetAllocationMetric(TowerDefenseAllocationMetric metric)
	{
		return $"{TowerDefenseAllocationTelemetry.GetAllocatedBytes(metric)}/{TowerDefenseAllocationTelemetry.GetSampleCount(metric)}";
	}

	private void TryCapturePerformanceScreenshot()
	{
		if (_screenshotRequested || string.IsNullOrWhiteSpace(ScreenshotPath) || (WaitForDamagePartSettledBeforeScreenshot && !HasSettledDamagePartForScreenshot()))
		{
			return;
		}
		_screenshotRequested = true;
		Viewport viewport = GetViewport();
		if (!GodotObject.IsInstanceValid(viewport) || !GodotObject.IsInstanceValid(viewport.GetTexture()))
		{
			_screenshotRequested = false;
			GD.PrintErr("[TestPerf] screenshot failed: viewport texture is unavailable.");
			return;
		}
		string text = ScreenshotPath.Replace('\\', '/');
		if (!text.Contains("://", StringComparison.Ordinal))
		{
			text = "res://" + text.TrimStart('/');
		}
		string text2 = ProjectSettings.GlobalizePath(text);
		Error error = DirAccess.MakeDirRecursiveAbsolute(text2.GetBaseDir());
		if (error != Error.Ok && error != Error.AlreadyExists)
		{
			_screenshotRequested = false;
			GD.PrintErr($"[TestPerf] screenshot directory failed path={text2.GetBaseDir()} error={error}");
			return;
		}
		Image image = viewport.GetTexture().GetImage();
		Error error2 = image.SavePng(text2);
		_screenshotSaved = error2 == Error.Ok;
		if (_screenshotSaved)
		{
			GD.Print($"[TestPerf] screenshot saved path={text} size={image.GetWidth()}x{image.GetHeight()}");
		}
		else
		{
			_screenshotRequested = false;
			GD.PrintErr($"[TestPerf] screenshot failed path={text} error={error2}");
		}
	}

	private static bool HasSettledDamagePartForScreenshot()
	{
		Node2D characterNode = TowerDefenseManager.GetCharacterNode();
		if (!GodotObject.IsInstanceValid(characterNode))
		{
			return false;
		}
		foreach (Node child in characterNode.GetChildren())
		{
			if (child is DamagePartDrop { over: not false } damagePartDrop && GodotObject.IsInstanceValid(damagePartDrop.sprite) && !damagePartDrop.IsQueuedForDeletion())
			{
				return true;
			}
		}
		return false;
	}

	private void PrintScenePoolSnapshot(string label)
	{
		if (!UseDifferentZombieScenePool)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		string value = "";
		for (int i = 0; i < _sceneUseCounts.Length; i++)
		{
			int num3 = _sceneUseCounts[i];
			if (num3 > 0)
			{
				num++;
				if (num3 > num2)
				{
					num2 = num3;
					value = ((i < _scenePoolPaths.Count) ? _scenePoolPaths[i] : "");
				}
			}
		}
		GD.Print($"[TestPerf] {label} scenePool root={ZombieSceneRoot} excludedPath={ExcludedZombieScenePathSubstring} excluded={_excludedScenePoolCount} characterScenes={UseZombieCharacterScenes} pool={_scenePool.Count} uniqueUsed={num} maxSceneUse={num2} maxScene={value}");
	}

	private RenderManagerStats CollectRenderManagerStats()
	{
		RenderManagerStats stats = default;
		CollectRenderManagerStats(GetTree().Root, ref stats);
		return stats;
	}

	private static void CollectRenderManagerStats(Node node, ref RenderManagerStats stats)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateRenderManager)
		{
			stats.RenderManagers++;
		}
		foreach (Node child in node.GetChildren())
		{
			if (child is AdobeAnimateMultiMeshBatcher adobeAnimateMultiMeshBatcher)
			{
				stats.CompactCrowdRoots += adobeAnimateMultiMeshBatcher.GetCompactCrowdRootCountForTest();
				stats.CompositeCrowdRoots += adobeAnimateMultiMeshBatcher.GetCompositeCrowdRootCountForTest();
				stats.GpuGraphRoots += adobeAnimateMultiMeshBatcher.GetGpuGraphRootCountForTest();
				stats.GpuGraphSlots += adobeAnimateMultiMeshBatcher.GetGpuGraphSlotCountForTest();
				stats.GpuGraphStateTexels += adobeAnimateMultiMeshBatcher.GetGpuGraphStateWrittenTexelsForTest();
				stats.CrowdStateTexels += adobeAnimateMultiMeshBatcher.GetCrowdStateWrittenTexelsForTest();
			}
			if (child is MultiMeshInstance2D { Multimesh: not null } multiMeshInstance2D)
			{
				if (child.Name.ToString().StartsWith("AdobeAnimateBucket", StringComparison.Ordinal))
				{
					stats.NormalBuckets++;
				}
				int num = Math.Max(0, multiMeshInstance2D.Multimesh.VisibleInstanceCount);
				stats.VisibleInstances += num;
				stats.AllocatedInstances += Math.Max(0, multiMeshInstance2D.Multimesh.InstanceCount);
				if (num > stats.MaxBucketVisible)
				{
					stats.MaxBucketVisible = num;
				}
			}
			CollectRenderManagerStats(child, ref stats);
		}
	}

	private void PrintCrowdFilterDebugSnapshot(string label)
	{
		if (!PrintCrowdFilterDebugData)
		{
			return;
		}
		int num = 0;
		int num2 = Math.Max(0, CrowdFilterDebugSpriteCount);
		for (int i = 0; i < _spawnedAnimationSprites.Count; i++)
		{
			if (num >= num2)
			{
				break;
			}
			AdobeAnimateSpriteBase adobeAnimateSpriteBase = _spawnedAnimationSprites[i];
			if (GodotObject.IsInstanceValid(adobeAnimateSpriteBase))
			{
				adobeAnimateSpriteBase.PrintCrowdFilterDebugReport($"Test.{label}.animationSpriteIndex={i}", CrowdFilterDebugMaxFrameSlices);
				num++;
			}
		}
		GD.Print($"[TestCrowdFilterDebug] label={label} printed={num}/{num2} spawned={_spawnedNodes.Count} animationSprites={_spawnedAnimationSprites.Count}");
	}

	private Rect2 GetWorldViewportRect()
	{
		Viewport viewport = GetViewport();
		if (!GodotObject.IsInstanceValid(viewport))
		{
			return new Rect2(Vector2.Zero, SpawnArea);
		}
		Rect2 visibleRect = viewport.GetVisibleRect();
		Transform2D transform2D = viewport.GetCanvasTransform().AffineInverse();
		Vector2 position = transform2D * visibleRect.Position;
		Vector2 to = transform2D * (visibleRect.Position + new Vector2(visibleRect.Size.X, 0f));
		Vector2 to2 = transform2D * (visibleRect.Position + visibleRect.Size);
		Vector2 to3 = transform2D * (visibleRect.Position + new Vector2(0f, visibleRect.Size.Y));
		return new Rect2(position, Vector2.Zero).Expand(to).Expand(to2).Expand(to3)
			.Abs();
	}

	private static double ElapsedMs(long startTicks)
	{
		return (double)(Stopwatch.GetTimestamp() - startTicks) * 1000.0 / (double)Stopwatch.Frequency;
	}

	private static string FormatRect(Rect2 rect)
	{
		return $"({rect.Position.X:F1},{rect.Position.Y:F1},{rect.Size.X:F1},{rect.Size.Y:F1})";
	}

	private static string NormalizeResDirectory(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "res://";
		}
		string text = path.Replace('\\', '/').TrimEnd('/');
		if (text.Length != 0)
		{
			return text;
		}
		return "res://";
	}

	private static string GetFileNameWithoutExtension(string path)
	{
		int num = path.LastIndexOf('/');
		string text = ((num >= 0 && num + 1 < path.Length) ? path.Substring(num + 1) : path);
		int num2 = text.LastIndexOf('.');
		if (num2 <= 0)
		{
			return text;
		}
		return text.Substring(0, num2);
	}

	private void ConfigureUnfocusedBenchmarkRun()
	{
		if (KeepRunningWhenUnfocused)
		{
			_originalLowProcessorUsageMode = OS.LowProcessorUsageMode;
			_unfocusedRunConfigured = true;
			ProcessMode = ProcessModeEnum.Always;
			OS.LowProcessorUsageMode = false;
			SceneTree tree = GetTree();
			if (GodotObject.IsInstanceValid(tree))
			{
				tree.Paused = false;
			}
			GD.Print("[TestPerf] unfocused pause and low-processor throttling disabled.");
		}
	}

	private void ApplyCommandLineOverrides()
	{
		string[] cmdlineUserArgs = OS.GetCmdlineUserArgs();
		foreach (string text in cmdlineUserArgs)
		{
			int value2;
			int value3;
			double value4;
			int value5;
			int value6;
			Vector2 value7;
			Vector2 value8;
			float value9;
			double value10;
			double value11;
			string value12;
			double value13;
			double value14;
			double value15;
			double value16;
			int value17;
			int value18;
			string value19;
			bool value20;
			string value21;
			string value22;
			double value23;
			bool value24;
			bool value25;
			string value26;
			string value27;
			string value28;
			bool value29;
			bool value30;
			bool value31;
			bool value32;
			int value33;
			bool value34;
			bool value35;
			bool value36;
			bool value37;
			int value38;
			double value39;
			bool value40;
			bool value41;
			bool value42;
			double value43;
			double value44;
			double value45;
			string value46;
			double value47;
			bool value48;
			string value49;
			string value50;
			double value51;
			double value52;
			double value53;
			bool value54;
			bool value55;
			bool value56;
			bool value57;
			int value58;
			int value59;
			double value60;
			double value61;
			double value62;
			double value63;
			bool value64;
			bool value65;
			bool value66;
			bool value67;
			bool value68;
			double value69;
			bool value70;
			bool value71;
			bool value72;
			bool value73;
			bool value74;
			bool value75;
			double value76;
			bool value77;
			int value78;
			bool value79;
			string value80;
			float value81;
			string value82;
			if (TryReadIntArg(text, "--test-spawn-count=", out var value))
			{
				SpawnCount = Math.Max(0, value);
			}
			else if (TryReadIntArg(text, "--test-max-scenes=", out value2))
			{
				MaxDifferentZombieScenes = Math.Max(0, value2);
			}
			else if (TryReadIntArg(text, "--test-spawn-batch-size=", out value3))
			{
				SpawnBatchSize = Math.Max(0, value3);
			}
			else if (TryReadDoubleArg(text, "--test-spawn-time-budget-ms=", out value4))
			{
				SpawnTimeBudgetMilliseconds = Math.Max(0.0, value4);
			}
			else if (TryReadIntArg(text, "--test-grid-columns=", out value5))
			{
				SpawnGridColumns = Math.Max(0, value5);
			}
			else if (TryReadIntArg(text, "--test-z-buckets=", out value6))
			{
				RenderZBucketCount = Math.Max(0, value6);
			}
			else if (TryReadVector2Arg(text, "--test-spawn-area=", out value7))
			{
				SpawnArea = new Vector2(Math.Max(1f, value7.X), Math.Max(1f, value7.Y));
			}
			else if (TryReadVector2Arg(text, "--test-spawn-origin=", out value8))
			{
				SpawnOrigin = value8;
			}
			else if (TryReadFloatArg(text, "--test-spawn-scale=", out value9))
			{
				SpawnScale = Math.Max(0f, value9);
			}
			else if (TryReadDoubleArg(text, "--test-auto-quit=", out value10))
			{
				AutoQuitAfterSeconds = Math.Max(0.0, value10);
			}
			else if (TryReadDoubleArg(text, "--test-warmup=", out value11))
			{
				WarmupSeconds = Math.Max(0.0, value11);
			}
			else if (TryReadStringArg(text, "--runtime-profile=", out value12))
			{
				RuntimeProfile = value12;
			}
			else if (TryReadDoubleArg(text, "--test-min-average-fps=", out value13))
			{
				MinimumAverageFps = Math.Max(0.0, value13);
			}
			else if (TryReadDoubleArg(text, "--test-max-frame-p99-ms=", out value14))
			{
				MaximumFrameP99Milliseconds = Math.Max(0.0, value14);
			}
			else if (TryReadDoubleArg(text, "--test-max-frame-ms=", out value15))
			{
				MaximumFrameMilliseconds = Math.Max(0.0, value15);
			}
			else if (TryReadDoubleArg(text, "--test-min-one-percent-low-fps=", out value16))
			{
				MinimumOnePercentLowFps = Math.Max(0.0, value16);
			}
			else if (TryReadIntArg(text, "--test-max-fps=", out value17))
			{
				BenchmarkMaxFps = Math.Max(0, value17);
			}
			else if (TryReadIntArg(text, "--test-physics-ticks=", out value18))
			{
				BenchmarkPhysicsTicksPerSecond = Math.Max(1, value18);
			}
			else if (TryReadStringArg(text, "--test-screenshot=", out value19))
			{
				ScreenshotPath = value19;
			}
			else if (TryReadBoolArg(text, "--test-screenshot-after-damage-part-settled=", out value20))
			{
				WaitForDamagePartSettledBeforeScreenshot = value20;
			}
			else if (TryReadStringArg(text, "--test-scene=", out value21))
			{
				ExactScenePath = value21;
			}
			else if (TryReadStringArg(text, "--test-animation-clip=", out value22))
			{
				ExactAnimationClip = value22;
			}
			else if (TryReadDoubleArg(text, "--test-print-interval=", out value23))
			{
				PrintIntervalSeconds = (float)Math.Max(0.05, value23);
			}
			else if (TryReadBoolArg(text, "--test-different-zombies=", out value24))
			{
				UseDifferentZombieScenePool = value24;
			}
			else if (TryReadBoolArg(text, "--test-character-scenes=", out value25))
			{
				UseZombieCharacterScenes = value25;
			}
			else if (TryReadStringArg(text, "--test-zombie-root=", out value26))
			{
				ZombieSceneRoot = value26;
			}
			else if (TryReadStringArg(text, "--test-character-prefix=", out value27))
			{
				CharacterSceneFilePrefix = (string.IsNullOrWhiteSpace(value27) ? "TowerDefenseZombie" : value27.Trim());
			}
			else if (TryReadStringArg(text, "--test-exclude-zombie-path=", out value28))
			{
				ExcludedZombieScenePathSubstring = value28;
			}
			else if (TryReadBoolArg(text, "--test-wait-resource-load=", out value29))
			{
				WaitForResourceManagerLoad = value29;
			}
			else if (TryReadBoolArg(text, "--test-profiler=", out value30))
			{
				EnableAdobeAnimateProfiler = value30;
			}
			else if (TryReadBoolArg(text, "--test-detailed-profiler=", out value31))
			{
				EnableDetailedAdobeAnimateProfiler = value31;
			}
			else if (TryReadBoolArg(text, "--test-allocation-telemetry=", out value32))
			{
				EnableAllocationTelemetry = value32;
			}
			else if (TryReadIntArg(text, "--test-profiler-dump-frames=", out value33))
			{
				ProfilerDumpIntervalFrames = Math.Max(1, value33);
			}
			else if (TryReadBoolArg(text, "--test-clear-animation-cache=", out value34))
			{
				ClearAdobeAnimateRuntimeCachesBeforeRun = value34;
			}
			else if (TryReadBoolArg(text, "--test-gpu-render-graph=", out value35))
			{
				EnableGpuRenderGraph = value35;
			}
			else if (TryReadBoolArg(text, "--test-crowd-mesh-capacity-classes=", out value36))
			{
				AdobeAnimateRenderManager.CrowdMeshCapacityClassesEnabled = value36;
			}
			else if (TryReadBoolArg(text, "--test-force-cpu-pose-fallback=", out value37))
			{
				ForceCpuPoseFallbackForTest = value37;
			}
			else if (TryReadIntArg(text, "--test-fixed-animation-frame=", out value38))
			{
				FixedAnimationFrame = value38;
			}
			else if (TryReadDoubleArg(text, "--test-fixed-animation-subframe=", out value39))
			{
				FixedAnimationSubframe = Math.Clamp(value39, 0.0, 0.999999);
			}
			else if (text.StartsWith("--test-adobe-fixed-frame=", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = text.Substring("--test-adobe-fixed-frame=".Length);
				if (!int.TryParse(text2, out var result) || result < 0)
				{
					_invalidAdobeFixedAnimationFrame = true;
					GD.PushError("[TestPerf] Invalid --test-adobe-fixed-frame value '" + text2 + "'. Expected a non-negative integer.");
				}
				else
				{
					_adobeFixedAnimationFrame = result;
				}
			}
			else if (TryReadBoolArg(text, "--test-crowd-filter-debug=", out value40))
			{
				PrintCrowdFilterDebugData = value40;
			}
			else if (TryReadBoolArg(text, "--test-z-interleave-fixture=", out value41))
			{
				EnableZInterleaveFixture = value41;
			}
			else if (TryReadBoolArg(text, "--test-crowd-mode-report=", out value42))
			{
				PrintCrowdModeReport = value42;
			}
			else if (TryReadDoubleArg(text, "--test-initial-anime-frame-rate=", out value43))
			{
				InitialAnimeFrameRate = Math.Max(0.0, value43);
			}
			else if (TryReadDoubleArg(text, "--test-switch-anime-frame-rate-after=", out value44))
			{
				SwitchAnimeFrameRateAfterSeconds = Math.Max(0.0, value44);
			}
			else if (TryReadDoubleArg(text, "--test-switch-anime-frame-rate-to=", out value45))
			{
				SwitchAnimeFrameRateTo = Math.Max(0.0, value45);
			}
			else if (TryReadStringArg(text, "--test-runtime-visual-mutation=", out value46))
			{
				RuntimeVisualMutation = value46;
			}
			else if (TryReadDoubleArg(text, "--test-runtime-visual-mutation-after=", out value47))
			{
				RuntimeVisualMutationAfterSeconds = Math.Max(0.0, value47);
			}
			else if (TryReadBoolArg(text, "--test-animation-switch-stress=", out value48))
			{
				EnableAnimationSwitchStress = value48;
			}
			else if (TryReadStringArg(text, "--test-animation-switch-clip-a=", out value49))
			{
				AnimationSwitchClipA = value49.Trim();
			}
			else if (TryReadStringArg(text, "--test-animation-switch-clip-b=", out value50))
			{
				AnimationSwitchClipB = value50.Trim();
			}
			else if (TryReadDoubleArg(text, "--test-animation-switch-interval=", out value51))
			{
				AnimationSwitchIntervalSeconds = Math.Max(0.01, value51);
			}
			else if (TryReadDoubleArg(text, "--test-animation-switch-blend=", out value52))
			{
				AnimationSwitchBlendSeconds = Math.Max(0.0, value52);
			}
			else if (TryReadDoubleArg(text, "--test-animation-switch-start=", out value53))
			{
				AnimationSwitchStartAfterSeconds = Math.Max(0.0, value53);
			}
			else if (TryReadBoolArg(text, "--test-bowling-impact-tween-stress=", out value54))
			{
				EnableBowlingImpactTweenStress = value54;
			}
			else if (TryReadBoolArg(text, "--test-bowling-impact-ensure-shield=", out value55))
			{
				BowlingImpactEnsureShield = value55;
			}
			else if (TryReadBoolArg(text, "--test-bowling-impact-production-event=", out value56))
			{
				BowlingImpactUseProductionEvent = value56;
			}
			else if (TryReadBoolArg(text, "--test-bowling-impact-create-tweens=", out value57))
			{
				BowlingImpactUseProductionEvent = value57;
			}
			else if (TryReadIntArg(text, "--test-bowling-impact-tween-limit=", out value58))
			{
				BowlingImpactProcessedTweenLimit = Math.Max(-1, value58);
			}
			else if (TryReadIntArg(text, "--test-bowling-impact-targets=", out value59))
			{
				BowlingImpactTargetsPerBurst = Math.Max(0, value59);
			}
			else if (TryReadDoubleArg(text, "--test-bowling-impact-damage=", out value60))
			{
				BowlingImpactDamage = Math.Max(0.0, value60);
			}
			else if (TryReadDoubleArg(text, "--test-bowling-impact-interval=", out value61))
			{
				BowlingImpactBurstIntervalSeconds = Math.Max(0.01, value61);
			}
			else if (TryReadDoubleArg(text, "--test-bowling-impact-window=", out value62))
			{
				BowlingImpactActiveWindowSeconds = Math.Max(0.05, value62);
			}
			else if (TryReadDoubleArg(text, "--test-bowling-impact-start=", out value63))
			{
				BowlingImpactStartAfterSeconds = Math.Max(0.0, value63);
			}
			else if (TryReadBoolArg(text, "--test-component-alive-scheduling=", out value64))
			{
				RunComponentAliveSchedulingRegression = value64;
			}
			else if (TryReadBoolArg(text, "--test-state-physics-batch-rate=", out value65))
			{
				RunStatePhysicsBatchRateRegression = value65;
			}
			else if (TryReadBoolArg(text, "--test-animation-cadence=", out value66))
			{
				RunAnimationCadenceRegression = value66;
			}
			else if (TryReadBoolArg(text, "--test-external-visuals=", out value67))
			{
				EnableExternalVisualStress = value67;
			}
			else if (TryReadBoolArg(text, "--test-external-visual-ice-only=", out value68))
			{
				ExternalVisualIceOnly = value68;
			}
			else if (TryReadDoubleArg(text, "--test-external-visual-ratio=", out value69))
			{
				ExternalVisualRatio = Math.Clamp(value69, 0.0, 1.0);
			}
			else if (TryReadBoolArg(text, "--test-combat-state-stress=", out value70))
			{
				EnableCombatStateStress = value70;
			}
			else if (TryReadBoolArg(text, "--test-preserve-combat-characters=", out value71))
			{
				PreserveCombatCharactersInStress = value71;
			}
			else if (TryReadBoolArg(text, "--test-combat-map-night=", out value72))
			{
				CombatStressIsNight = value72;
			}
			else if (TryReadBoolArg(text, "--test-show-health=", out value73))
			{
				ShowCharacterHealthInStress = value73;
			}
			else if (TryReadBoolArg(text, "--test-single-enemy-target=", out value74))
			{
				EnableSingleEnemyTargetFixture = value74;
			}
			else if (TryReadBoolArg(text, "--test-sleep-cycle-stress=", out value75))
			{
				EnableSleepCycleStress = value75;
			}
			else if (TryReadDoubleArg(text, "--test-sleep-cycle-start=", out value76))
			{
				SleepCycleStartAfterSeconds = Math.Max(0.0, value76);
			}
			else if (TryReadBoolArg(text, "--test-many-enemy-idle=", out value77))
			{
				EnableManyEnemyIdleFixture = value77;
			}
			else if (TryReadIntArg(text, "--test-many-enemy-idle-target-count=", out value78))
			{
				ManyEnemyIdleTargetCount = Math.Max(1, value78);
			}
			else if (TryReadBoolArg(text, "--test-many-enemy-idle-outside-range=", out value79))
			{
				ManyEnemyIdleTargetsOutsideAttackRange = value79;
			}
			else if (TryReadStringArg(text, "--test-enemy-scene=", out value80))
			{
				SingleEnemyTargetScenePath = value80.Trim();
			}
			else if (TryReadFloatArg(text, "--test-enemy-scale=", out value81))
			{
				SingleEnemyTargetScale = Math.Max(0.1f, value81);
			}
			else if (TryReadStringArg(text, "--test-adobe-render-backend=", out value82))
			{
				if (string.Equals(value82, "gpu", StringComparison.OrdinalIgnoreCase))
				{
					_testRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
					continue;
				}
				if (string.Equals(value82, "cpu", StringComparison.OrdinalIgnoreCase))
				{
					_testRenderBackend = AdobeAnimateRenderBackend.CpuPose;
					continue;
				}
				_invalidTestRenderBackend = true;
				GD.PushError("[TestPerf] Invalid --test-adobe-render-backend value '" + value82 + "'. Expected gpu or cpu.");
			}
		}
	}

	private string GetTestRenderBackendLabel()
	{
		if (_testRenderBackend != AdobeAnimateRenderBackend.CpuPose)
		{
			return "gpu";
		}
		return "cpu";
	}

	private void ConfigureCombatStateStress()
	{
		if (EnableCombatStateStress && !_combatStressConfigured)
		{
			TowerDefenseManager instance = TowerDefenseManager.Instance;
			if (!GodotObject.IsInstanceValid(instance))
			{
				GD.PushError("[TestPerf] Combat-state stress requires the TowerDefenseManager autoload.");
				return;
			}
			_previousControl = instance.currentControl;
			_previousGridBegin = instance.gridBeginPos;
			_previousGridSize = instance.gridSize;
			_previousGridNum = instance.gridNum;
			_previousBackZombie = instance.backZombie;
			_combatStressControl = new CharacterStressControlStub
			{
				Name = "CharacterStressControl",
				isGameRunning = true,
				isInit = true,
				levelConfig = new TowerDefenseLevelConfig()
			};
			AddChild(_combatStressControl, forceReadableName: false, InternalMode.Disabled);
			_combatStressControl.characterNode = this;
			_combatStressMapControl = new TowerDefenseMapControl
			{
				ProcessMode = ProcessModeEnum.Disabled,
				mapIceCap = new Node2D()
			};
			AddChild(_combatStressMapControl.mapIceCap, forceReadableName: false, InternalMode.Disabled);
			_combatStressMapFeature = new TowerDefenseBattleFeatureMap
			{
				control = _combatStressControl,
				config = new TowerDefenseMapConfig
				{
					isNight = CombatStressIsNight
				},
				mapControl = _combatStressMapControl
			};
			_combatStressMapFeature.iceCapList.Resize(_combatStressMapFeature.config.gridNum.Y + 1);
			_combatStressMapControl.mapFeature = _combatStressMapFeature;
			_combatStressMapFeature.PlantGridInit();
			_combatStressControl.featureDictionary[new StringName("Map")] = _combatStressMapFeature;
			instance.currentControl = _combatStressControl;
			instance.gridBeginPos = SpawnOrigin;
			instance.gridNum = new Vector2I(9, 5);
			instance.gridSize = new Vector2(Math.Max(1f, SpawnArea.X / (float)instance.gridNum.X), Math.Max(1f, SpawnArea.Y / (float)instance.gridNum.Y));
			instance.backZombie = false;
			_combatStressConfigured = true;
			GD.Print($"[TestPerf] Combat-state stress enabled gridBegin={instance.gridBeginPos} gridSize={instance.gridSize} gridNum={instance.gridNum} isNight={CombatStressIsNight}.");
		}
	}

	private void RestoreCombatStateStress()
	{
		if (_combatStressConfigured)
		{
			TowerDefenseManager instance = TowerDefenseManager.Instance;
			if (GodotObject.IsInstanceValid(instance))
			{
				instance.currentControl = _previousControl;
				instance.gridBeginPos = _previousGridBegin;
				instance.gridSize = _previousGridSize;
				instance.gridNum = _previousGridNum;
				instance.backZombie = _previousBackZombie;
			}
			if (GodotObject.IsInstanceValid(_combatStressControl) && !_combatStressControl.IsQueuedForDeletion())
			{
				_combatStressControl.QueueFree();
			}
			_combatStressControl = null;
			_combatStressMapFeature = null;
			if (GodotObject.IsInstanceValid(_combatStressMapControl?.mapIceCap) && !_combatStressMapControl.mapIceCap.IsQueuedForDeletion())
			{
				_combatStressMapControl.mapIceCap.QueueFree();
			}
			if (GodotObject.IsInstanceValid(_combatStressMapControl))
			{
				_combatStressMapControl.Free();
			}
			_combatStressMapControl = null;
			_previousControl = null;
			_combatStressConfigured = false;
		}
	}

	private static bool TryReadIntArg(string arg, string prefix, out int value)
	{
		value = 0;
		if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			return int.TryParse(arg.Substring(prefix.Length), out value);
		}
		return false;
	}

	private static bool TryReadFloatArg(string arg, string prefix, out float value)
	{
		value = 0f;
		if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			return float.TryParse(arg.Substring(prefix.Length), out value);
		}
		return false;
	}

	private static bool TryReadVector2Arg(string arg, string prefix, out Vector2 value)
	{
		value = Vector2.Zero;
		if (!arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		string[] array = arg.Substring(prefix.Length).Split(new char[4] { 'x', 'X', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 2)
		{
			return false;
		}
		if (!float.TryParse(array[0], out var result) || !float.TryParse(array[1], out var result2))
		{
			return false;
		}
		value = new Vector2(result, result2);
		return true;
	}

	private static bool TryReadDoubleArg(string arg, string prefix, out double value)
	{
		value = 0.0;
		if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			return double.TryParse(arg.Substring(prefix.Length), out value);
		}
		return false;
	}

	private static bool TryReadBoolArg(string arg, string prefix, out bool value)
	{
		value = false;
		if (!arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		string text = arg.Substring(prefix.Length);
		if (bool.TryParse(text, out value))
		{
			return true;
		}
		if (text == "1")
		{
			value = true;
			return true;
		}
		if (text == "0")
		{
			value = false;
			return true;
		}
		return false;
	}

	private static bool TryReadStringArg(string arg, string prefix, out string value)
	{
		value = "";
		if (!arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		value = arg.Substring(prefix.Length);
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(106)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnStressResourceManagerLoadOver, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartStressWorkload, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitializeRenderBackendUi, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleRenderBackendSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "selectedIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestartMeasurementAfterBackendSwitch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateRenderBackendUi, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshRenderBackendUi, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunComponentAliveSchedulingRegressionAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunStatePhysicsBatchRateRegressionAsync, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteSpawn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareAnimationCadenceRegression, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindCadenceSlot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SampleAnimationCadence, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsAnimationCadencePassed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ForceCpuPoseFallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrintScheduledNodeTypeSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartMeasurementTelemetry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasValidMeasurementTelemetry, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrySwitchAnimeFrameRate, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "sampleElapsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryRunAnimationSwitchStress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "sampleElapsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryRunBowlingImpactTweenStress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "sampleElapsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountProcessedTweens, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountBowlingImpactMotions, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryApplyRuntimeVisualMutation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "sampleElapsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryApplyViewportEntryMutation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CaptureViewportEntryFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrintViewportEntryPerformance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryApplyDamagePartMutation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ValidateNewDamagePartNode, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "previousCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountActiveDamagePartNodes, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.PrintNonGpuGraphCrowdModes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnforceFixedAnimationFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnforceAdobeFixedAnimationFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyFixedAnimationFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "fixedAnimationFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SpawnPendingBatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "start", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "end", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "fallbackScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureExternalVisualStress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "stableIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldEnableExternalVisual, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "stableIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadDifferentZombieScenePool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldExcludeZombieScenePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsZombieSceneFile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsZombieAnimationSceneFile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsZombieCharacterSceneFile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterSpawnedNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureCombatCharacterPreservation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "character", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldFreezeCombatSourcesForInitialSleep, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreCombatSourceProcessModes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareSingleEnemyTargetFixture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareManyEnemyIdleTargetFixture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryDriveSingleEnemyRoleAttackUtilities, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryRunSleepCycleStress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "sampleElapsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginSleepCycleStress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "sampleElapsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindHypnotistSleepEventForStress, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountSleepingStressCharacters, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountWokenStressCharacters, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FormatSleepCycleMissingBuffScenes, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FormatSleepCycleMissingEntryScenes, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompleteSleepCycleStress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "passed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreSleepCycleImmunity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsSleepCycleStressPassed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryAdvanceSingleEnemyRoleAttackModes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DetachCombatComponentEngagementHandlers, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsCombatComponentEngagementPassed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountValidManyEnemyIdleTargets, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountManyEnemyIdleBusyStates, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsManyEnemyIdleFixturePassed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FormatAttackComponentEngagementBreakdown, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "utilities", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatFireComponentEngagementBreakdown, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCombatTargetDurability, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.PrintExactRoleComponentInventory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveSpawnPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveSingleTargetSourcePosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveSingleTargetSourceGridPosition, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.EnsureZInterleaveFixture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrintRuntimeSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrintExternalVisualPositionDiagnostics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVector, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReportExternalVisualStressFailures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetAverageMeasuredFps, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMeasuredSeconds, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrintPerformanceResult, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsAnimationSwitchStressPassed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrintAnimationSwitchStressResult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "passed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsBowlingImpactTweenStressPassed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrintBowlingImpactTweenStressResult, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "passed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CountValidSpawnedCharacters, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FormatInvalidSpawnedCharacterScenes, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CountStateChartRelatedNodes, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetAllocationMetric, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "metric", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryCapturePerformanceScreenshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasSettledDamagePartForScreenshot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.PrintScenePoolSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrintCrowdFilterDebugSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetWorldViewportRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ElapsedMs, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "startTicks", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatRect, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeResDirectory, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFileNameWithoutExtension, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureUnfocusedBenchmarkRun, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyCommandLineOverrides, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetTestRenderBackendLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureCombatStateStress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreCombatStateStress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.OnStressResourceManagerLoadOver && args.Count == 0)
		{
			OnStressResourceManagerLoadOver();
			ret = default;
			return true;
		}
		if (method == MethodName.StartStressWorkload && args.Count == 0)
		{
			StartStressWorkload();
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeRenderBackendUi && args.Count == 0)
		{
			InitializeRenderBackendUi();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleRenderBackendSelected && args.Count == 1)
		{
			HandleRenderBackendSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestartMeasurementAfterBackendSwitch && args.Count == 0)
		{
			RestartMeasurementAfterBackendSwitch();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateRenderBackendUi && args.Count == 1)
		{
			UpdateRenderBackendUi(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshRenderBackendUi && args.Count == 0)
		{
			RefreshRenderBackendUi();
			ret = default;
			return true;
		}
		if (method == MethodName.RunComponentAliveSchedulingRegressionAsync && args.Count == 0)
		{
			RunComponentAliveSchedulingRegressionAsync();
			ret = default;
			return true;
		}
		if (method == MethodName.RunStatePhysicsBatchRateRegressionAsync && args.Count == 0)
		{
			RunStatePhysicsBatchRateRegressionAsync();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.CompleteSpawn && args.Count == 0)
		{
			CompleteSpawn();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareAnimationCadenceRegression && args.Count == 0)
		{
			PrepareAnimationCadenceRegression();
			ret = default;
			return true;
		}
		if (method == MethodName.FindCadenceSlot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSlot>(FindCadenceSlot(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SampleAnimationCadence && args.Count == 0)
		{
			SampleAnimationCadence();
			ret = default;
			return true;
		}
		if (method == MethodName.IsAnimationCadencePassed && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAnimationCadencePassed());
			return true;
		}
		if (method == MethodName.ForceCpuPoseFallback && args.Count == 0)
		{
			ForceCpuPoseFallback();
			ret = default;
			return true;
		}
		if (method == MethodName.PrintScheduledNodeTypeSnapshot && args.Count == 0)
		{
			PrintScheduledNodeTypeSnapshot();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartMeasurementTelemetry && args.Count == 0)
		{
			StartMeasurementTelemetry();
			ret = default;
			return true;
		}
		if (method == MethodName.HasValidMeasurementTelemetry && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasValidMeasurementTelemetry());
			return true;
		}
		if (method == MethodName.TrySwitchAnimeFrameRate && args.Count == 1)
		{
			TrySwitchAnimeFrameRate(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryRunAnimationSwitchStress && args.Count == 1)
		{
			TryRunAnimationSwitchStress(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryRunBowlingImpactTweenStress && args.Count == 1)
		{
			TryRunBowlingImpactTweenStress(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountProcessedTweens && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountProcessedTweens());
			return true;
		}
		if (method == MethodName.CountBowlingImpactMotions && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountBowlingImpactMotions());
			return true;
		}
		if (method == MethodName.TryApplyRuntimeVisualMutation && args.Count == 1)
		{
			TryApplyRuntimeVisualMutation(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryApplyViewportEntryMutation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryApplyViewportEntryMutation());
			return true;
		}
		if (method == MethodName.CaptureViewportEntryFrame && args.Count == 1)
		{
			CaptureViewportEntryFrame(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrintViewportEntryPerformance && args.Count == 0)
		{
			PrintViewportEntryPerformance();
			ret = default;
			return true;
		}
		if (method == MethodName.TryApplyDamagePartMutation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TryApplyDamagePartMutation());
			return true;
		}
		if (method == MethodName.ValidateNewDamagePartNode && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ValidateNewDamagePartNode(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.CountActiveDamagePartNodes && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountActiveDamagePartNodes());
			return true;
		}
		if (method == MethodName.PrintNonGpuGraphCrowdModes && args.Count == 0)
		{
			PrintNonGpuGraphCrowdModes();
			ret = default;
			return true;
		}
		if (method == MethodName.EnforceFixedAnimationFrame && args.Count == 0)
		{
			EnforceFixedAnimationFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.EnforceAdobeFixedAnimationFrame && args.Count == 0)
		{
			EnforceAdobeFixedAnimationFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyFixedAnimationFrame && args.Count == 1)
		{
			ApplyFixedAnimationFrame(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnPendingBatch && args.Count == 0)
		{
			SpawnPendingBatch();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnRange && args.Count == 3)
		{
			SpawnRange(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<PackedScene>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureExternalVisualStress && args.Count == 2)
		{
			ConfigureExternalVisualStress(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldEnableExternalVisual && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldEnableExternalVisual(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadDifferentZombieScenePool && args.Count == 0)
		{
			LoadDifferentZombieScenePool();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldExcludeZombieScenePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldExcludeZombieScenePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsZombieSceneFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsZombieSceneFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsZombieAnimationSceneFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsZombieAnimationSceneFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsZombieCharacterSceneFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsZombieCharacterSceneFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterSpawnedNode && args.Count == 1)
		{
			RegisterSpawnedNode(VariantUtils.ConvertTo<Node2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureCombatCharacterPreservation && args.Count == 1)
		{
			ConfigureCombatCharacterPreservation(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldFreezeCombatSourcesForInitialSleep && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldFreezeCombatSourcesForInitialSleep());
			return true;
		}
		if (method == MethodName.RestoreCombatSourceProcessModes && args.Count == 0)
		{
			RestoreCombatSourceProcessModes();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareSingleEnemyTargetFixture && args.Count == 0)
		{
			PrepareSingleEnemyTargetFixture();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareManyEnemyIdleTargetFixture && args.Count == 0)
		{
			PrepareManyEnemyIdleTargetFixture();
			ret = default;
			return true;
		}
		if (method == MethodName.TryDriveSingleEnemyRoleAttackUtilities && args.Count == 0)
		{
			TryDriveSingleEnemyRoleAttackUtilities();
			ret = default;
			return true;
		}
		if (method == MethodName.TryRunSleepCycleStress && args.Count == 1)
		{
			TryRunSleepCycleStress(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginSleepCycleStress && args.Count == 1)
		{
			BeginSleepCycleStress(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindHypnotistSleepEventForStress && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacterEventAddBuff>(FindHypnotistSleepEventForStress());
			return true;
		}
		if (method == MethodName.CountSleepingStressCharacters && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountSleepingStressCharacters());
			return true;
		}
		if (method == MethodName.CountWokenStressCharacters && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountWokenStressCharacters());
			return true;
		}
		if (method == MethodName.FormatSleepCycleMissingBuffScenes && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(FormatSleepCycleMissingBuffScenes());
			return true;
		}
		if (method == MethodName.FormatSleepCycleMissingEntryScenes && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(FormatSleepCycleMissingEntryScenes());
			return true;
		}
		if (method == MethodName.CompleteSleepCycleStress && args.Count == 2)
		{
			CompleteSleepCycleStress(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreSleepCycleImmunity && args.Count == 0)
		{
			RestoreSleepCycleImmunity();
			ret = default;
			return true;
		}
		if (method == MethodName.IsSleepCycleStressPassed && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSleepCycleStressPassed());
			return true;
		}
		if (method == MethodName.TryAdvanceSingleEnemyRoleAttackModes && args.Count == 0)
		{
			TryAdvanceSingleEnemyRoleAttackModes();
			ret = default;
			return true;
		}
		if (method == MethodName.DetachCombatComponentEngagementHandlers && args.Count == 0)
		{
			DetachCombatComponentEngagementHandlers();
			ret = default;
			return true;
		}
		if (method == MethodName.IsCombatComponentEngagementPassed && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCombatComponentEngagementPassed());
			return true;
		}
		if (method == MethodName.CountValidManyEnemyIdleTargets && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountValidManyEnemyIdleTargets());
			return true;
		}
		if (method == MethodName.CountManyEnemyIdleBusyStates && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountManyEnemyIdleBusyStates());
			return true;
		}
		if (method == MethodName.IsManyEnemyIdleFixturePassed && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsManyEnemyIdleFixturePassed());
			return true;
		}
		if (method == MethodName.FormatAttackComponentEngagementBreakdown && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatAttackComponentEngagementBreakdown(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatFireComponentEngagementBreakdown && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(FormatFireComponentEngagementBreakdown());
			return true;
		}
		if (method == MethodName.GetCombatTargetDurability && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetCombatTargetDurability(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.PrintExactRoleComponentInventory && args.Count == 0)
		{
			PrintExactRoleComponentInventory();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveSpawnPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ResolveSpawnPosition(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveSingleTargetSourcePosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ResolveSingleTargetSourcePosition());
			return true;
		}
		if (method == MethodName.ResolveSingleTargetSourceGridPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ResolveSingleTargetSourceGridPosition());
			return true;
		}
		if (method == MethodName.EnsureZInterleaveFixture && args.Count == 0)
		{
			EnsureZInterleaveFixture();
			ret = default;
			return true;
		}
		if (method == MethodName.PrintRuntimeSnapshot && args.Count == 1)
		{
			PrintRuntimeSnapshot(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrintExternalVisualPositionDiagnostics && args.Count == 1)
		{
			PrintExternalVisualPositionDiagnostics(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FormatVector && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVector(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.ReportExternalVisualStressFailures && args.Count == 0)
		{
			ReportExternalVisualStressFailures();
			ret = default;
			return true;
		}
		if (method == MethodName.GetAverageMeasuredFps && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetAverageMeasuredFps());
			return true;
		}
		if (method == MethodName.GetMeasuredSeconds && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<double>(GetMeasuredSeconds());
			return true;
		}
		if (method == MethodName.PrintPerformanceResult && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(PrintPerformanceResult());
			return true;
		}
		if (method == MethodName.IsAnimationSwitchStressPassed && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAnimationSwitchStressPassed());
			return true;
		}
		if (method == MethodName.PrintAnimationSwitchStressResult && args.Count == 1)
		{
			PrintAnimationSwitchStressResult(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsBowlingImpactTweenStressPassed && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsBowlingImpactTweenStressPassed());
			return true;
		}
		if (method == MethodName.PrintBowlingImpactTweenStressResult && args.Count == 1)
		{
			PrintBowlingImpactTweenStressResult(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CountValidSpawnedCharacters && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountValidSpawnedCharacters());
			return true;
		}
		if (method == MethodName.FormatInvalidSpawnedCharacterScenes && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(FormatInvalidSpawnedCharacterScenes());
			return true;
		}
		if (method == MethodName.CountStateChartRelatedNodes && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountStateChartRelatedNodes(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAllocationMetric && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetAllocationMetric(VariantUtils.ConvertTo<TowerDefenseAllocationMetric>(in args[0])));
			return true;
		}
		if (method == MethodName.TryCapturePerformanceScreenshot && args.Count == 0)
		{
			TryCapturePerformanceScreenshot();
			ret = default;
			return true;
		}
		if (method == MethodName.HasSettledDamagePartForScreenshot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSettledDamagePartForScreenshot());
			return true;
		}
		if (method == MethodName.PrintScenePoolSnapshot && args.Count == 1)
		{
			PrintScenePoolSnapshot(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrintCrowdFilterDebugSnapshot && args.Count == 1)
		{
			PrintCrowdFilterDebugSnapshot(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetWorldViewportRect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetWorldViewportRect());
			return true;
		}
		if (method == MethodName.ElapsedMs && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ElapsedMs(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatRect(VariantUtils.ConvertTo<Rect2>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeResDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFileNameWithoutExtension && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetFileNameWithoutExtension(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ConfigureUnfocusedBenchmarkRun && args.Count == 0)
		{
			ConfigureUnfocusedBenchmarkRun();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCommandLineOverrides && args.Count == 0)
		{
			ApplyCommandLineOverrides();
			ret = default;
			return true;
		}
		if (method == MethodName.GetTestRenderBackendLabel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetTestRenderBackendLabel());
			return true;
		}
		if (method == MethodName.ConfigureCombatStateStress && args.Count == 0)
		{
			ConfigureCombatStateStress();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreCombatStateStress && args.Count == 0)
		{
			RestoreCombatStateStress();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FindCadenceSlot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateSlot>(FindCadenceSlot(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ValidateNewDamagePartNode && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ValidateNewDamagePartNode(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.CountActiveDamagePartNodes && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountActiveDamagePartNodes());
			return true;
		}
		if (method == MethodName.IsZombieAnimationSceneFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsZombieAnimationSceneFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCombatTargetDurability && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(GetCombatTargetDurability(VariantUtils.ConvertTo<TowerDefenseCharacter>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveSingleTargetSourceGridPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(ResolveSingleTargetSourceGridPosition());
			return true;
		}
		if (method == MethodName.FormatVector && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVector(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.CountStateChartRelatedNodes && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountStateChartRelatedNodes(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAllocationMetric && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetAllocationMetric(VariantUtils.ConvertTo<TowerDefenseAllocationMetric>(in args[0])));
			return true;
		}
		if (method == MethodName.HasSettledDamagePartForScreenshot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasSettledDamagePartForScreenshot());
			return true;
		}
		if (method == MethodName.ElapsedMs && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ElapsedMs(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatRect && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatRect(VariantUtils.ConvertTo<Rect2>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeResDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFileNameWithoutExtension && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetFileNameWithoutExtension(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName.OnStressResourceManagerLoadOver)
		{
			return true;
		}
		if (method == MethodName.StartStressWorkload)
		{
			return true;
		}
		if (method == MethodName.InitializeRenderBackendUi)
		{
			return true;
		}
		if (method == MethodName.HandleRenderBackendSelected)
		{
			return true;
		}
		if (method == MethodName.RestartMeasurementAfterBackendSwitch)
		{
			return true;
		}
		if (method == MethodName.UpdateRenderBackendUi)
		{
			return true;
		}
		if (method == MethodName.RefreshRenderBackendUi)
		{
			return true;
		}
		if (method == MethodName.RunComponentAliveSchedulingRegressionAsync)
		{
			return true;
		}
		if (method == MethodName.RunStatePhysicsBatchRateRegressionAsync)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.CompleteSpawn)
		{
			return true;
		}
		if (method == MethodName.PrepareAnimationCadenceRegression)
		{
			return true;
		}
		if (method == MethodName.FindCadenceSlot)
		{
			return true;
		}
		if (method == MethodName.SampleAnimationCadence)
		{
			return true;
		}
		if (method == MethodName.IsAnimationCadencePassed)
		{
			return true;
		}
		if (method == MethodName.ForceCpuPoseFallback)
		{
			return true;
		}
		if (method == MethodName.PrintScheduledNodeTypeSnapshot)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.StartMeasurementTelemetry)
		{
			return true;
		}
		if (method == MethodName.HasValidMeasurementTelemetry)
		{
			return true;
		}
		if (method == MethodName.TrySwitchAnimeFrameRate)
		{
			return true;
		}
		if (method == MethodName.TryRunAnimationSwitchStress)
		{
			return true;
		}
		if (method == MethodName.TryRunBowlingImpactTweenStress)
		{
			return true;
		}
		if (method == MethodName.CountProcessedTweens)
		{
			return true;
		}
		if (method == MethodName.CountBowlingImpactMotions)
		{
			return true;
		}
		if (method == MethodName.TryApplyRuntimeVisualMutation)
		{
			return true;
		}
		if (method == MethodName.TryApplyViewportEntryMutation)
		{
			return true;
		}
		if (method == MethodName.CaptureViewportEntryFrame)
		{
			return true;
		}
		if (method == MethodName.PrintViewportEntryPerformance)
		{
			return true;
		}
		if (method == MethodName.TryApplyDamagePartMutation)
		{
			return true;
		}
		if (method == MethodName.ValidateNewDamagePartNode)
		{
			return true;
		}
		if (method == MethodName.CountActiveDamagePartNodes)
		{
			return true;
		}
		if (method == MethodName.PrintNonGpuGraphCrowdModes)
		{
			return true;
		}
		if (method == MethodName.EnforceFixedAnimationFrame)
		{
			return true;
		}
		if (method == MethodName.EnforceAdobeFixedAnimationFrame)
		{
			return true;
		}
		if (method == MethodName.ApplyFixedAnimationFrame)
		{
			return true;
		}
		if (method == MethodName.SpawnPendingBatch)
		{
			return true;
		}
		if (method == MethodName.SpawnRange)
		{
			return true;
		}
		if (method == MethodName.ConfigureExternalVisualStress)
		{
			return true;
		}
		if (method == MethodName.ShouldEnableExternalVisual)
		{
			return true;
		}
		if (method == MethodName.LoadDifferentZombieScenePool)
		{
			return true;
		}
		if (method == MethodName.ShouldExcludeZombieScenePath)
		{
			return true;
		}
		if (method == MethodName.IsZombieSceneFile)
		{
			return true;
		}
		if (method == MethodName.IsZombieAnimationSceneFile)
		{
			return true;
		}
		if (method == MethodName.IsZombieCharacterSceneFile)
		{
			return true;
		}
		if (method == MethodName.RegisterSpawnedNode)
		{
			return true;
		}
		if (method == MethodName.ConfigureCombatCharacterPreservation)
		{
			return true;
		}
		if (method == MethodName.ShouldFreezeCombatSourcesForInitialSleep)
		{
			return true;
		}
		if (method == MethodName.RestoreCombatSourceProcessModes)
		{
			return true;
		}
		if (method == MethodName.PrepareSingleEnemyTargetFixture)
		{
			return true;
		}
		if (method == MethodName.PrepareManyEnemyIdleTargetFixture)
		{
			return true;
		}
		if (method == MethodName.TryDriveSingleEnemyRoleAttackUtilities)
		{
			return true;
		}
		if (method == MethodName.TryRunSleepCycleStress)
		{
			return true;
		}
		if (method == MethodName.BeginSleepCycleStress)
		{
			return true;
		}
		if (method == MethodName.FindHypnotistSleepEventForStress)
		{
			return true;
		}
		if (method == MethodName.CountSleepingStressCharacters)
		{
			return true;
		}
		if (method == MethodName.CountWokenStressCharacters)
		{
			return true;
		}
		if (method == MethodName.FormatSleepCycleMissingBuffScenes)
		{
			return true;
		}
		if (method == MethodName.FormatSleepCycleMissingEntryScenes)
		{
			return true;
		}
		if (method == MethodName.CompleteSleepCycleStress)
		{
			return true;
		}
		if (method == MethodName.RestoreSleepCycleImmunity)
		{
			return true;
		}
		if (method == MethodName.IsSleepCycleStressPassed)
		{
			return true;
		}
		if (method == MethodName.TryAdvanceSingleEnemyRoleAttackModes)
		{
			return true;
		}
		if (method == MethodName.DetachCombatComponentEngagementHandlers)
		{
			return true;
		}
		if (method == MethodName.IsCombatComponentEngagementPassed)
		{
			return true;
		}
		if (method == MethodName.CountValidManyEnemyIdleTargets)
		{
			return true;
		}
		if (method == MethodName.CountManyEnemyIdleBusyStates)
		{
			return true;
		}
		if (method == MethodName.IsManyEnemyIdleFixturePassed)
		{
			return true;
		}
		if (method == MethodName.FormatAttackComponentEngagementBreakdown)
		{
			return true;
		}
		if (method == MethodName.FormatFireComponentEngagementBreakdown)
		{
			return true;
		}
		if (method == MethodName.GetCombatTargetDurability)
		{
			return true;
		}
		if (method == MethodName.PrintExactRoleComponentInventory)
		{
			return true;
		}
		if (method == MethodName.ResolveSpawnPosition)
		{
			return true;
		}
		if (method == MethodName.ResolveSingleTargetSourcePosition)
		{
			return true;
		}
		if (method == MethodName.ResolveSingleTargetSourceGridPosition)
		{
			return true;
		}
		if (method == MethodName.EnsureZInterleaveFixture)
		{
			return true;
		}
		if (method == MethodName.PrintRuntimeSnapshot)
		{
			return true;
		}
		if (method == MethodName.PrintExternalVisualPositionDiagnostics)
		{
			return true;
		}
		if (method == MethodName.FormatVector)
		{
			return true;
		}
		if (method == MethodName.ReportExternalVisualStressFailures)
		{
			return true;
		}
		if (method == MethodName.GetAverageMeasuredFps)
		{
			return true;
		}
		if (method == MethodName.GetMeasuredSeconds)
		{
			return true;
		}
		if (method == MethodName.PrintPerformanceResult)
		{
			return true;
		}
		if (method == MethodName.IsAnimationSwitchStressPassed)
		{
			return true;
		}
		if (method == MethodName.PrintAnimationSwitchStressResult)
		{
			return true;
		}
		if (method == MethodName.IsBowlingImpactTweenStressPassed)
		{
			return true;
		}
		if (method == MethodName.PrintBowlingImpactTweenStressResult)
		{
			return true;
		}
		if (method == MethodName.CountValidSpawnedCharacters)
		{
			return true;
		}
		if (method == MethodName.FormatInvalidSpawnedCharacterScenes)
		{
			return true;
		}
		if (method == MethodName.CountStateChartRelatedNodes)
		{
			return true;
		}
		if (method == MethodName.GetAllocationMetric)
		{
			return true;
		}
		if (method == MethodName.TryCapturePerformanceScreenshot)
		{
			return true;
		}
		if (method == MethodName.HasSettledDamagePartForScreenshot)
		{
			return true;
		}
		if (method == MethodName.PrintScenePoolSnapshot)
		{
			return true;
		}
		if (method == MethodName.PrintCrowdFilterDebugSnapshot)
		{
			return true;
		}
		if (method == MethodName.GetWorldViewportRect)
		{
			return true;
		}
		if (method == MethodName.ElapsedMs)
		{
			return true;
		}
		if (method == MethodName.FormatRect)
		{
			return true;
		}
		if (method == MethodName.NormalizeResDirectory)
		{
			return true;
		}
		if (method == MethodName.GetFileNameWithoutExtension)
		{
			return true;
		}
		if (method == MethodName.ConfigureUnfocusedBenchmarkRun)
		{
			return true;
		}
		if (method == MethodName.ApplyCommandLineOverrides)
		{
			return true;
		}
		if (method == MethodName.GetTestRenderBackendLabel)
		{
			return true;
		}
		if (method == MethodName.ConfigureCombatStateStress)
		{
			return true;
		}
		if (method == MethodName.RestoreCombatStateStress)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.TestScene)
		{
			TestScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.ExactScenePath)
		{
			ExactScenePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ExactAnimationClip)
		{
			ExactAnimationClip = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.SpawnCount)
		{
			SpawnCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.SpawnOrigin)
		{
			SpawnOrigin = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.SpawnArea)
		{
			SpawnArea = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.UseDifferentZombieScenePool)
		{
			UseDifferentZombieScenePool = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.UseZombieCharacterScenes)
		{
			UseZombieCharacterScenes = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ZombieSceneRoot)
		{
			ZombieSceneRoot = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.CharacterSceneFilePrefix)
		{
			CharacterSceneFilePrefix = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ExcludedZombieScenePathSubstring)
		{
			ExcludedZombieScenePathSubstring = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.MaxDifferentZombieScenes)
		{
			MaxDifferentZombieScenes = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.SpawnBatchSize)
		{
			SpawnBatchSize = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.SpawnTimeBudgetMilliseconds)
		{
			SpawnTimeBudgetMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.UseGridSpawnLayout)
		{
			UseGridSpawnLayout = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.SpawnGridColumns)
		{
			SpawnGridColumns = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.RenderZBucketCount)
		{
			RenderZBucketCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.SpawnScale)
		{
			SpawnScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.SpawnRandomSeed)
		{
			SpawnRandomSeed = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName.AutoQuitAfterSeconds)
		{
			AutoQuitAfterSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.WarmupSeconds)
		{
			WarmupSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.MinimumAverageFps)
		{
			MinimumAverageFps = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.MaximumFrameP99Milliseconds)
		{
			MaximumFrameP99Milliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.MaximumFrameMilliseconds)
		{
			MaximumFrameMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.MinimumOnePercentLowFps)
		{
			MinimumOnePercentLowFps = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.BenchmarkMaxFps)
		{
			BenchmarkMaxFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.BenchmarkPhysicsTicksPerSecond)
		{
			BenchmarkPhysicsTicksPerSecond = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ScreenshotPath)
		{
			ScreenshotPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.WaitForDamagePartSettledBeforeScreenshot)
		{
			WaitForDamagePartSettledBeforeScreenshot = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.PrintIntervalSeconds)
		{
			PrintIntervalSeconds = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.EnableAdobeAnimateProfiler)
		{
			EnableAdobeAnimateProfiler = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.EnableDetailedAdobeAnimateProfiler)
		{
			EnableDetailedAdobeAnimateProfiler = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.EnableAllocationTelemetry)
		{
			EnableAllocationTelemetry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ProfilerDumpIntervalFrames)
		{
			ProfilerDumpIntervalFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ProfilerMaxMetricsPerDump)
		{
			ProfilerMaxMetricsPerDump = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ClearAdobeAnimateRuntimeCachesBeforeRun)
		{
			ClearAdobeAnimateRuntimeCachesBeforeRun = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.EnableGpuRenderGraph)
		{
			EnableGpuRenderGraph = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ForceCpuPoseFallbackForTest)
		{
			ForceCpuPoseFallbackForTest = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.FixedAnimationFrame)
		{
			FixedAnimationFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.FixedAnimationSubframe)
		{
			FixedAnimationSubframe = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.PrintCrowdFilterDebugData)
		{
			PrintCrowdFilterDebugData = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.CrowdFilterDebugSpriteCount)
		{
			CrowdFilterDebugSpriteCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.CrowdFilterDebugMaxFrameSlices)
		{
			CrowdFilterDebugMaxFrameSlices = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.PrintCrowdFilterDebugAfterFirstRuntimeFrame)
		{
			PrintCrowdFilterDebugAfterFirstRuntimeFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.PrintCrowdFilterDebugEveryRuntimeSnapshot)
		{
			PrintCrowdFilterDebugEveryRuntimeSnapshot = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.EnableZInterleaveFixture)
		{
			EnableZInterleaveFixture = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.PrintCrowdModeReport)
		{
			PrintCrowdModeReport = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.InitialAnimeFrameRate)
		{
			InitialAnimeFrameRate = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.SwitchAnimeFrameRateAfterSeconds)
		{
			SwitchAnimeFrameRateAfterSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.SwitchAnimeFrameRateTo)
		{
			SwitchAnimeFrameRateTo = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeVisualMutation)
		{
			RuntimeVisualMutation = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeVisualMutationAfterSeconds)
		{
			RuntimeVisualMutationAfterSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.EnableAnimationSwitchStress)
		{
			EnableAnimationSwitchStress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.AnimationSwitchClipA)
		{
			AnimationSwitchClipA = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.AnimationSwitchClipB)
		{
			AnimationSwitchClipB = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.AnimationSwitchIntervalSeconds)
		{
			AnimationSwitchIntervalSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.AnimationSwitchBlendSeconds)
		{
			AnimationSwitchBlendSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.AnimationSwitchStartAfterSeconds)
		{
			AnimationSwitchStartAfterSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.EnableBowlingImpactTweenStress)
		{
			EnableBowlingImpactTweenStress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.BowlingImpactEnsureShield)
		{
			BowlingImpactEnsureShield = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.BowlingImpactUseProductionEvent)
		{
			BowlingImpactUseProductionEvent = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.BowlingImpactProcessedTweenLimit)
		{
			BowlingImpactProcessedTweenLimit = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.BowlingImpactTargetsPerBurst)
		{
			BowlingImpactTargetsPerBurst = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.BowlingImpactDamage)
		{
			BowlingImpactDamage = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.BowlingImpactBurstIntervalSeconds)
		{
			BowlingImpactBurstIntervalSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.BowlingImpactActiveWindowSeconds)
		{
			BowlingImpactActiveWindowSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.BowlingImpactStartAfterSeconds)
		{
			BowlingImpactStartAfterSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.RunComponentAliveSchedulingRegression)
		{
			RunComponentAliveSchedulingRegression = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.RunStatePhysicsBatchRateRegression)
		{
			RunStatePhysicsBatchRateRegression = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.RunAnimationCadenceRegression)
		{
			RunAnimationCadenceRegression = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.EnableExternalVisualStress)
		{
			EnableExternalVisualStress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ExternalVisualIceOnly)
		{
			ExternalVisualIceOnly = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ExternalVisualRatio)
		{
			ExternalVisualRatio = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.EnableCombatStateStress)
		{
			EnableCombatStateStress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.PreserveCombatCharactersInStress)
		{
			PreserveCombatCharactersInStress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.CombatStressIsNight)
		{
			CombatStressIsNight = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ShowCharacterHealthInStress)
		{
			ShowCharacterHealthInStress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.EnableSingleEnemyTargetFixture)
		{
			EnableSingleEnemyTargetFixture = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.SingleEnemyTargetScenePath)
		{
			SingleEnemyTargetScenePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.SingleEnemyTargetScale)
		{
			SingleEnemyTargetScale = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.EnableSleepCycleStress)
		{
			EnableSleepCycleStress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.SleepCycleStartAfterSeconds)
		{
			SleepCycleStartAfterSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.EnableManyEnemyIdleFixture)
		{
			EnableManyEnemyIdleFixture = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ManyEnemyIdleTargetCount)
		{
			ManyEnemyIdleTargetCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ManyEnemyIdleTargetsOutsideAttackRange)
		{
			ManyEnemyIdleTargetsOutsideAttackRange = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.WaitForResourceManagerLoad)
		{
			WaitForResourceManagerLoad = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.KeepRunningWhenUnfocused)
		{
			KeepRunningWhenUnfocused = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeProfile)
		{
			RuntimeProfile = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._testRenderBackend)
		{
			_testRenderBackend = VariantUtils.ConvertTo<AdobeAnimateRenderBackend>(in value);
			return true;
		}
		if (name == PropertyName._sceneUseCounts)
		{
			_sceneUseCounts = VariantUtils.ConvertTo<int[]>(in value);
			return true;
		}
		if (name == PropertyName._printTimer)
		{
			_printTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._windowDeltaSeconds)
		{
			_windowDeltaSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._sampleStartSeconds)
		{
			_sampleStartSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._windowFrames)
		{
			_windowFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._runtimeCrowdFilterDebugPrinted)
		{
			_runtimeCrowdFilterDebugPrinted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._crowdModeReportPrinted)
		{
			_crowdModeReportPrinted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._spawnFailures)
		{
			_spawnFailures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._loadedScenePoolCount)
		{
			_loadedScenePoolCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._skippedScenePoolCount)
		{
			_skippedScenePoolCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._excludedScenePoolCount)
		{
			_excludedScenePoolCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._nextSpawnIndex)
		{
			_nextSpawnIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._nextSpawnProgressPrint)
		{
			_nextSpawnProgressPrint = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._spawnStartTicks)
		{
			_spawnStartTicks = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._loadMs)
		{
			_loadMs = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._spawnNoGcRegionActive)
		{
			_spawnNoGcRegionActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingSpawnFallbackScene)
		{
			_pendingSpawnFallbackScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName._spawnPending)
		{
			_spawnPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._spawnFrameBudgetPassed)
		{
			_spawnFrameBudgetPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._autoQuitPrinted)
		{
			_autoQuitPrinted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._measurementStarted)
		{
			_measurementStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._measurementFrames)
		{
			_measurementFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._singleEnemyTarget)
		{
			_singleEnemyTarget = VariantUtils.ConvertTo<TowerDefenseCharacter>(in value);
			return true;
		}
		if (name == PropertyName._singleEnemyAttackComponentCount)
		{
			_singleEnemyAttackComponentCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._singleEnemyAttackUtilityComponentCount)
		{
			_singleEnemyAttackUtilityComponentCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._singleEnemyFireComponentCount)
		{
			_singleEnemyFireComponentCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._singleEnemyTargetInitialHitpoints)
		{
			_singleEnemyTargetInitialHitpoints = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._singleEnemyFixturePrepared)
		{
			_singleEnemyFixturePrepared = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._singleEnemyRobotModeAdvanceRequested)
		{
			_singleEnemyRobotModeAdvanceRequested = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._singleEnemyRoleUtilityPhaseStarted)
		{
			_singleEnemyRoleUtilityPhaseStarted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._singleEnemyRoleUtilityPhaseCompleted)
		{
			_singleEnemyRoleUtilityPhaseCompleted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._manyEnemyIdleFixturePrepared)
		{
			_manyEnemyIdleFixturePrepared = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._nextSingleEnemyRoleUtilityAuditSeconds)
		{
			_nextSingleEnemyRoleUtilityAuditSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._sleepCyclePhase)
		{
			_sleepCyclePhase = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._sleepCyclePhaseDeadlineSeconds)
		{
			_sleepCyclePhaseDeadlineSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._sleepCycleCompleted)
		{
			_sleepCycleCompleted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sleepCyclePassed)
		{
			_sleepCyclePassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sleepCycleEligibleCount)
		{
			_sleepCycleEligibleCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._sleepCycleAppliedCount)
		{
			_sleepCycleAppliedCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._sleepCycleEnteredCount)
		{
			_sleepCycleEnteredCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._sleepCycleWokenCount)
		{
			_sleepCycleWokenCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._sleepCycleNormalizedImmunityCount)
		{
			_sleepCycleNormalizedImmunityCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._sleepCycleImmunityRestored)
		{
			_sleepCycleImmunityRestored = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sleepCycleHypnotistEventProvider)
		{
			_sleepCycleHypnotistEventProvider = VariantUtils.ConvertTo<TowerDefenseZombieHypnotist>(in value);
			return true;
		}
		if (name == PropertyName._measurementStartSeconds)
		{
			_measurementStartSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._measurementEndSeconds)
		{
			_measurementEndSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._measurementPreviousFrameTicks)
		{
			_measurementPreviousFrameTicks = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._screenshotRequested)
		{
			_screenshotRequested = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._screenshotSaved)
		{
			_screenshotSaved = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._resultPrinted)
		{
			_resultPrinted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._performanceResultPassed)
		{
			_performanceResultPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._animeFrameRateSwitchCompleted)
		{
			_animeFrameRateSwitchCompleted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._animeFrameRateSwitchPassed)
		{
			_animeFrameRateSwitchPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeVisualMutationCompleted)
		{
			_runtimeVisualMutationCompleted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeVisualMutationPassed)
		{
			_runtimeVisualMutationPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._animationSwitchUseSecondClip)
		{
			_animationSwitchUseSecondClip = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._nextAnimationSwitchSeconds)
		{
			_nextAnimationSwitchSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._animationSwitchWindowEndSeconds)
		{
			_animationSwitchWindowEndSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._animationSwitchCount)
		{
			_animationSwitchCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationSwitchAppliedCount)
		{
			_animationSwitchAppliedCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationSwitchMissingClipCount)
		{
			_animationSwitchMissingClipCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationSwitchSynchronousTotalMilliseconds)
		{
			_animationSwitchSynchronousTotalMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._animationSwitchSynchronousMaxMilliseconds)
		{
			_animationSwitchSynchronousMaxMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._nextBowlingImpactSeconds)
		{
			_nextBowlingImpactSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactWindowEndSeconds)
		{
			_bowlingImpactWindowEndSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactBurstCount)
		{
			_bowlingImpactBurstCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactAppliedCount)
		{
			_bowlingImpactAppliedCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactMissingShieldCount)
		{
			_bowlingImpactMissingShieldCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactMaxProcessedTweens)
		{
			_bowlingImpactMaxProcessedTweens = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactMaxBatchedMotions)
		{
			_bowlingImpactMaxBatchedMotions = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactMaxBatchedShieldVisuals)
		{
			_bowlingImpactMaxBatchedShieldVisuals = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactSynchronousTotalMilliseconds)
		{
			_bowlingImpactSynchronousTotalMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactSynchronousMaxMilliseconds)
		{
			_bowlingImpactSynchronousMaxMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._bowlingImpactThreadAllocatedBytes)
		{
			_bowlingImpactThreadAllocatedBytes = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._viewportEntryCaptureFramesRemaining)
		{
			_viewportEntryCaptureFramesRemaining = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._viewportEntryCaptureSkipCurrentFrame)
		{
			_viewportEntryCaptureSkipCurrentFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._viewportEntryCapturePrinted)
		{
			_viewportEntryCapturePrinted = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._measurementThreadAllocatedBefore)
		{
			_measurementThreadAllocatedBefore = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._measurementTotalAllocatedBefore)
		{
			_measurementTotalAllocatedBefore = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._measurementGen0Before)
		{
			_measurementGen0Before = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._measurementGen1Before)
		{
			_measurementGen1Before = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._measurementGen2Before)
		{
			_measurementGen2Before = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cadenceSprite)
		{
			_cadenceSprite = VariantUtils.ConvertTo<AdobeAnimateSpriteBase>(in value);
			return true;
		}
		if (name == PropertyName._cadencePreviousPhase)
		{
			_cadencePreviousPhase = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._cadenceSamples)
		{
			_cadenceSamples = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cadenceAdvances)
		{
			_cadenceAdvances = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cadenceSlot)
		{
			_cadenceSlot = VariantUtils.ConvertTo<AdobeAnimateSlot>(in value);
			return true;
		}
		if (name == PropertyName._cadencePreviousSlotVersion)
		{
			_cadencePreviousSlotVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cadenceSlotAdvances)
		{
			_cadenceSlotAdvances = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._zInterleaveMarker)
		{
			_zInterleaveMarker = VariantUtils.ConvertTo<Polygon2D>(in value);
			return true;
		}
		if (name == PropertyName._zInterleaveParticles)
		{
			_zInterleaveParticles = VariantUtils.ConvertTo<CpuParticles2D>(in value);
			return true;
		}
		if (name == PropertyName._zInterleaveParticleTexture)
		{
			_zInterleaveParticleTexture = VariantUtils.ConvertTo<ImageTexture>(in value);
			return true;
		}
		if (name == PropertyName._externalVisualExpectedActive)
		{
			_externalVisualExpectedActive = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._externalVisualConfigurationFailures)
		{
			_externalVisualConfigurationFailures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._externalVisualGraphBuildCountAtMeasurementStart)
		{
			_externalVisualGraphBuildCountAtMeasurementStart = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphInitialBuildCountAtMeasurementStart)
		{
			_gpuGraphInitialBuildCountAtMeasurementStart = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphRebuildCountAtMeasurementStart)
		{
			_gpuGraphRebuildCountAtMeasurementStart = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphOwnerExitInvalidationCountAtMeasurementStart)
		{
			_gpuGraphOwnerExitInvalidationCountAtMeasurementStart = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphManagedSlotInvalidationCountAtMeasurementStart)
		{
			_gpuGraphManagedSlotInvalidationCountAtMeasurementStart = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._gpuGraphExternalVisualInvalidationCountAtMeasurementStart)
		{
			_gpuGraphExternalVisualInvalidationCountAtMeasurementStart = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._externalVisualStressTexture)
		{
			_externalVisualStressTexture = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._originalMaxFps)
		{
			_originalMaxFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._originalPhysicsTicksPerSecond)
		{
			_originalPhysicsTicksPerSecond = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._invalidTestRenderBackend)
		{
			_invalidTestRenderBackend = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._spawnInstantiateMaxMilliseconds)
		{
			_spawnInstantiateMaxMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._spawnInstantiateMaxScene)
		{
			_spawnInstantiateMaxScene = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._spawnAddChildReadyMaxMilliseconds)
		{
			_spawnAddChildReadyMaxMilliseconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._spawnAddChildReadyMaxScene)
		{
			_spawnAddChildReadyMaxScene = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._adobeFixedAnimationFrame)
		{
			_adobeFixedAnimationFrame = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._invalidAdobeFixedAnimationFrame)
		{
			_invalidAdobeFixedAnimationFrame = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderBackendOption)
		{
			_renderBackendOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._renderBackendStatus)
		{
			_renderBackendStatus = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._renderBackendUiRefreshTimer)
		{
			_renderBackendUiRefreshTimer = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._combatStressControl)
		{
			_combatStressControl = VariantUtils.ConvertTo<CharacterStressControlStub>(in value);
			return true;
		}
		if (name == PropertyName._combatStressMapFeature)
		{
			_combatStressMapFeature = VariantUtils.ConvertTo<TowerDefenseBattleFeatureMap>(in value);
			return true;
		}
		if (name == PropertyName._combatStressMapControl)
		{
			_combatStressMapControl = VariantUtils.ConvertTo<TowerDefenseMapControl>(in value);
			return true;
		}
		if (name == PropertyName._previousControl)
		{
			_previousControl = VariantUtils.ConvertTo<TowerDefenseControlNew>(in value);
			return true;
		}
		if (name == PropertyName._previousGridBegin)
		{
			_previousGridBegin = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._previousGridSize)
		{
			_previousGridSize = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._previousGridNum)
		{
			_previousGridNum = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._previousBackZombie)
		{
			_previousBackZombie = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._combatStressConfigured)
		{
			_combatStressConfigured = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._resourceManagerLoadGatePending)
		{
			_resourceManagerLoadGatePending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._originalLowProcessorUsageMode)
		{
			_originalLowProcessorUsageMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._unfocusedRunConfigured)
		{
			_unfocusedRunConfigured = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.TestScene)
		{
			value = VariantUtils.CreateFrom<PackedScene>(TestScene);
			return true;
		}
		string from;
		if (name == PropertyName.ExactScenePath)
		{
			from = ExactScenePath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ExactAnimationClip)
		{
			from = ExactAnimationClip;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.SpawnCount)
		{
			from2 = SpawnCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		Vector2 from3;
		if (name == PropertyName.SpawnOrigin)
		{
			from3 = SpawnOrigin;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.SpawnArea)
		{
			from3 = SpawnArea;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		bool from4;
		if (name == PropertyName.UseDifferentZombieScenePool)
		{
			from4 = UseDifferentZombieScenePool;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.UseZombieCharacterScenes)
		{
			from4 = UseZombieCharacterScenes;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.ZombieSceneRoot)
		{
			from = ZombieSceneRoot;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CharacterSceneFilePrefix)
		{
			from = CharacterSceneFilePrefix;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ExcludedZombieScenePathSubstring)
		{
			from = ExcludedZombieScenePathSubstring;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.MaxDifferentZombieScenes)
		{
			from2 = MaxDifferentZombieScenes;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.SpawnBatchSize)
		{
			from2 = SpawnBatchSize;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		double from5;
		if (name == PropertyName.SpawnTimeBudgetMilliseconds)
		{
			from5 = SpawnTimeBudgetMilliseconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.UseGridSpawnLayout)
		{
			from4 = UseGridSpawnLayout;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.SpawnGridColumns)
		{
			from2 = SpawnGridColumns;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.RenderZBucketCount)
		{
			from2 = RenderZBucketCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		float from6;
		if (name == PropertyName.SpawnScale)
		{
			from6 = SpawnScale;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.SpawnRandomSeed)
		{
			value = VariantUtils.CreateFrom<ulong>(SpawnRandomSeed);
			return true;
		}
		if (name == PropertyName.AutoQuitAfterSeconds)
		{
			from5 = AutoQuitAfterSeconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.WarmupSeconds)
		{
			from5 = WarmupSeconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.MinimumAverageFps)
		{
			from5 = MinimumAverageFps;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.MaximumFrameP99Milliseconds)
		{
			from5 = MaximumFrameP99Milliseconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.MaximumFrameMilliseconds)
		{
			from5 = MaximumFrameMilliseconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.MinimumOnePercentLowFps)
		{
			from5 = MinimumOnePercentLowFps;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.BenchmarkMaxFps)
		{
			from2 = BenchmarkMaxFps;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.BenchmarkPhysicsTicksPerSecond)
		{
			from2 = BenchmarkPhysicsTicksPerSecond;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ScreenshotPath)
		{
			from = ScreenshotPath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.WaitForDamagePartSettledBeforeScreenshot)
		{
			from4 = WaitForDamagePartSettledBeforeScreenshot;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.PrintIntervalSeconds)
		{
			from6 = PrintIntervalSeconds;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.EnableAdobeAnimateProfiler)
		{
			from4 = EnableAdobeAnimateProfiler;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.EnableDetailedAdobeAnimateProfiler)
		{
			from4 = EnableDetailedAdobeAnimateProfiler;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.EnableAllocationTelemetry)
		{
			from4 = EnableAllocationTelemetry;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.ProfilerDumpIntervalFrames)
		{
			from2 = ProfilerDumpIntervalFrames;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ProfilerMaxMetricsPerDump)
		{
			from2 = ProfilerMaxMetricsPerDump;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ClearAdobeAnimateRuntimeCachesBeforeRun)
		{
			from4 = ClearAdobeAnimateRuntimeCachesBeforeRun;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.EnableGpuRenderGraph)
		{
			from4 = EnableGpuRenderGraph;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.ForceCpuPoseFallbackForTest)
		{
			from4 = ForceCpuPoseFallbackForTest;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.FixedAnimationFrame)
		{
			from2 = FixedAnimationFrame;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.FixedAnimationSubframe)
		{
			from5 = FixedAnimationSubframe;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.PrintCrowdFilterDebugData)
		{
			from4 = PrintCrowdFilterDebugData;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.CrowdFilterDebugSpriteCount)
		{
			from2 = CrowdFilterDebugSpriteCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CrowdFilterDebugMaxFrameSlices)
		{
			from2 = CrowdFilterDebugMaxFrameSlices;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PrintCrowdFilterDebugAfterFirstRuntimeFrame)
		{
			from4 = PrintCrowdFilterDebugAfterFirstRuntimeFrame;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.PrintCrowdFilterDebugEveryRuntimeSnapshot)
		{
			from4 = PrintCrowdFilterDebugEveryRuntimeSnapshot;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.EnableZInterleaveFixture)
		{
			from4 = EnableZInterleaveFixture;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.PrintCrowdModeReport)
		{
			from4 = PrintCrowdModeReport;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.InitialAnimeFrameRate)
		{
			from5 = InitialAnimeFrameRate;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.SwitchAnimeFrameRateAfterSeconds)
		{
			from5 = SwitchAnimeFrameRateAfterSeconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.SwitchAnimeFrameRateTo)
		{
			from5 = SwitchAnimeFrameRateTo;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.RuntimeVisualMutation)
		{
			from = RuntimeVisualMutation;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RuntimeVisualMutationAfterSeconds)
		{
			from5 = RuntimeVisualMutationAfterSeconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.EnableAnimationSwitchStress)
		{
			from4 = EnableAnimationSwitchStress;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.AnimationSwitchClipA)
		{
			from = AnimationSwitchClipA;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AnimationSwitchClipB)
		{
			from = AnimationSwitchClipB;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AnimationSwitchIntervalSeconds)
		{
			from5 = AnimationSwitchIntervalSeconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.AnimationSwitchBlendSeconds)
		{
			from5 = AnimationSwitchBlendSeconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.AnimationSwitchStartAfterSeconds)
		{
			from5 = AnimationSwitchStartAfterSeconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.EnableBowlingImpactTweenStress)
		{
			from4 = EnableBowlingImpactTweenStress;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.BowlingImpactEnsureShield)
		{
			from4 = BowlingImpactEnsureShield;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.BowlingImpactUseProductionEvent)
		{
			from4 = BowlingImpactUseProductionEvent;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.BowlingImpactProcessedTweenLimit)
		{
			from2 = BowlingImpactProcessedTweenLimit;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.BowlingImpactTargetsPerBurst)
		{
			from2 = BowlingImpactTargetsPerBurst;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.BowlingImpactDamage)
		{
			from5 = BowlingImpactDamage;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.BowlingImpactBurstIntervalSeconds)
		{
			from5 = BowlingImpactBurstIntervalSeconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.BowlingImpactActiveWindowSeconds)
		{
			from5 = BowlingImpactActiveWindowSeconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.BowlingImpactStartAfterSeconds)
		{
			from5 = BowlingImpactStartAfterSeconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.RunComponentAliveSchedulingRegression)
		{
			from4 = RunComponentAliveSchedulingRegression;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.RunStatePhysicsBatchRateRegression)
		{
			from4 = RunStatePhysicsBatchRateRegression;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.RunAnimationCadenceRegression)
		{
			from4 = RunAnimationCadenceRegression;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.EnableExternalVisualStress)
		{
			from4 = EnableExternalVisualStress;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.ExternalVisualIceOnly)
		{
			from4 = ExternalVisualIceOnly;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.ExternalVisualRatio)
		{
			from5 = ExternalVisualRatio;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.EnableCombatStateStress)
		{
			from4 = EnableCombatStateStress;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.PreserveCombatCharactersInStress)
		{
			from4 = PreserveCombatCharactersInStress;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.CombatStressIsNight)
		{
			from4 = CombatStressIsNight;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.ShowCharacterHealthInStress)
		{
			from4 = ShowCharacterHealthInStress;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.EnableSingleEnemyTargetFixture)
		{
			from4 = EnableSingleEnemyTargetFixture;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.SingleEnemyTargetScenePath)
		{
			from = SingleEnemyTargetScenePath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SingleEnemyTargetScale)
		{
			from6 = SingleEnemyTargetScale;
			value = VariantUtils.CreateFrom(in from6);
			return true;
		}
		if (name == PropertyName.EnableSleepCycleStress)
		{
			from4 = EnableSleepCycleStress;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.SleepCycleStartAfterSeconds)
		{
			from5 = SleepCycleStartAfterSeconds;
			value = VariantUtils.CreateFrom(in from5);
			return true;
		}
		if (name == PropertyName.EnableManyEnemyIdleFixture)
		{
			from4 = EnableManyEnemyIdleFixture;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.ManyEnemyIdleTargetCount)
		{
			from2 = ManyEnemyIdleTargetCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ManyEnemyIdleTargetsOutsideAttackRange)
		{
			from4 = ManyEnemyIdleTargetsOutsideAttackRange;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.WaitForResourceManagerLoad)
		{
			from4 = WaitForResourceManagerLoad;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.KeepRunningWhenUnfocused)
		{
			from4 = KeepRunningWhenUnfocused;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.RuntimeProfile)
		{
			from = RuntimeProfile;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._testRenderBackend)
		{
			value = VariantUtils.CreateFrom(in _testRenderBackend);
			return true;
		}
		if (name == PropertyName._sceneUseCounts)
		{
			value = VariantUtils.CreateFrom(in _sceneUseCounts);
			return true;
		}
		if (name == PropertyName._printTimer)
		{
			value = VariantUtils.CreateFrom(in _printTimer);
			return true;
		}
		if (name == PropertyName._windowDeltaSeconds)
		{
			value = VariantUtils.CreateFrom(in _windowDeltaSeconds);
			return true;
		}
		if (name == PropertyName._sampleStartSeconds)
		{
			value = VariantUtils.CreateFrom(in _sampleStartSeconds);
			return true;
		}
		if (name == PropertyName._windowFrames)
		{
			value = VariantUtils.CreateFrom(in _windowFrames);
			return true;
		}
		if (name == PropertyName._runtimeCrowdFilterDebugPrinted)
		{
			value = VariantUtils.CreateFrom(in _runtimeCrowdFilterDebugPrinted);
			return true;
		}
		if (name == PropertyName._crowdModeReportPrinted)
		{
			value = VariantUtils.CreateFrom(in _crowdModeReportPrinted);
			return true;
		}
		if (name == PropertyName._spawnFailures)
		{
			value = VariantUtils.CreateFrom(in _spawnFailures);
			return true;
		}
		if (name == PropertyName._loadedScenePoolCount)
		{
			value = VariantUtils.CreateFrom(in _loadedScenePoolCount);
			return true;
		}
		if (name == PropertyName._skippedScenePoolCount)
		{
			value = VariantUtils.CreateFrom(in _skippedScenePoolCount);
			return true;
		}
		if (name == PropertyName._excludedScenePoolCount)
		{
			value = VariantUtils.CreateFrom(in _excludedScenePoolCount);
			return true;
		}
		if (name == PropertyName._nextSpawnIndex)
		{
			value = VariantUtils.CreateFrom(in _nextSpawnIndex);
			return true;
		}
		if (name == PropertyName._nextSpawnProgressPrint)
		{
			value = VariantUtils.CreateFrom(in _nextSpawnProgressPrint);
			return true;
		}
		if (name == PropertyName._spawnStartTicks)
		{
			value = VariantUtils.CreateFrom(in _spawnStartTicks);
			return true;
		}
		if (name == PropertyName._loadMs)
		{
			value = VariantUtils.CreateFrom(in _loadMs);
			return true;
		}
		if (name == PropertyName._spawnNoGcRegionActive)
		{
			value = VariantUtils.CreateFrom(in _spawnNoGcRegionActive);
			return true;
		}
		if (name == PropertyName._pendingSpawnFallbackScene)
		{
			value = VariantUtils.CreateFrom(in _pendingSpawnFallbackScene);
			return true;
		}
		if (name == PropertyName._spawnPending)
		{
			value = VariantUtils.CreateFrom(in _spawnPending);
			return true;
		}
		if (name == PropertyName._spawnFrameBudgetPassed)
		{
			value = VariantUtils.CreateFrom(in _spawnFrameBudgetPassed);
			return true;
		}
		if (name == PropertyName._autoQuitPrinted)
		{
			value = VariantUtils.CreateFrom(in _autoQuitPrinted);
			return true;
		}
		if (name == PropertyName._measurementStarted)
		{
			value = VariantUtils.CreateFrom(in _measurementStarted);
			return true;
		}
		if (name == PropertyName._measurementFrames)
		{
			value = VariantUtils.CreateFrom(in _measurementFrames);
			return true;
		}
		if (name == PropertyName._singleEnemyTarget)
		{
			value = VariantUtils.CreateFrom(in _singleEnemyTarget);
			return true;
		}
		if (name == PropertyName._singleEnemyAttackComponentCount)
		{
			value = VariantUtils.CreateFrom(in _singleEnemyAttackComponentCount);
			return true;
		}
		if (name == PropertyName._singleEnemyAttackUtilityComponentCount)
		{
			value = VariantUtils.CreateFrom(in _singleEnemyAttackUtilityComponentCount);
			return true;
		}
		if (name == PropertyName._singleEnemyFireComponentCount)
		{
			value = VariantUtils.CreateFrom(in _singleEnemyFireComponentCount);
			return true;
		}
		if (name == PropertyName._singleEnemyTargetInitialHitpoints)
		{
			value = VariantUtils.CreateFrom(in _singleEnemyTargetInitialHitpoints);
			return true;
		}
		if (name == PropertyName._singleEnemyFixturePrepared)
		{
			value = VariantUtils.CreateFrom(in _singleEnemyFixturePrepared);
			return true;
		}
		if (name == PropertyName._singleEnemyRobotModeAdvanceRequested)
		{
			value = VariantUtils.CreateFrom(in _singleEnemyRobotModeAdvanceRequested);
			return true;
		}
		if (name == PropertyName._singleEnemyRoleUtilityPhaseStarted)
		{
			value = VariantUtils.CreateFrom(in _singleEnemyRoleUtilityPhaseStarted);
			return true;
		}
		if (name == PropertyName._singleEnemyRoleUtilityPhaseCompleted)
		{
			value = VariantUtils.CreateFrom(in _singleEnemyRoleUtilityPhaseCompleted);
			return true;
		}
		if (name == PropertyName._manyEnemyIdleFixturePrepared)
		{
			value = VariantUtils.CreateFrom(in _manyEnemyIdleFixturePrepared);
			return true;
		}
		if (name == PropertyName._nextSingleEnemyRoleUtilityAuditSeconds)
		{
			value = VariantUtils.CreateFrom(in _nextSingleEnemyRoleUtilityAuditSeconds);
			return true;
		}
		if (name == PropertyName._sleepCyclePhase)
		{
			value = VariantUtils.CreateFrom(in _sleepCyclePhase);
			return true;
		}
		if (name == PropertyName._sleepCyclePhaseDeadlineSeconds)
		{
			value = VariantUtils.CreateFrom(in _sleepCyclePhaseDeadlineSeconds);
			return true;
		}
		if (name == PropertyName._sleepCycleCompleted)
		{
			value = VariantUtils.CreateFrom(in _sleepCycleCompleted);
			return true;
		}
		if (name == PropertyName._sleepCyclePassed)
		{
			value = VariantUtils.CreateFrom(in _sleepCyclePassed);
			return true;
		}
		if (name == PropertyName._sleepCycleEligibleCount)
		{
			value = VariantUtils.CreateFrom(in _sleepCycleEligibleCount);
			return true;
		}
		if (name == PropertyName._sleepCycleAppliedCount)
		{
			value = VariantUtils.CreateFrom(in _sleepCycleAppliedCount);
			return true;
		}
		if (name == PropertyName._sleepCycleEnteredCount)
		{
			value = VariantUtils.CreateFrom(in _sleepCycleEnteredCount);
			return true;
		}
		if (name == PropertyName._sleepCycleWokenCount)
		{
			value = VariantUtils.CreateFrom(in _sleepCycleWokenCount);
			return true;
		}
		if (name == PropertyName._sleepCycleNormalizedImmunityCount)
		{
			value = VariantUtils.CreateFrom(in _sleepCycleNormalizedImmunityCount);
			return true;
		}
		if (name == PropertyName._sleepCycleImmunityRestored)
		{
			value = VariantUtils.CreateFrom(in _sleepCycleImmunityRestored);
			return true;
		}
		if (name == PropertyName._sleepCycleHypnotistEventProvider)
		{
			value = VariantUtils.CreateFrom(in _sleepCycleHypnotistEventProvider);
			return true;
		}
		if (name == PropertyName._measurementStartSeconds)
		{
			value = VariantUtils.CreateFrom(in _measurementStartSeconds);
			return true;
		}
		if (name == PropertyName._measurementEndSeconds)
		{
			value = VariantUtils.CreateFrom(in _measurementEndSeconds);
			return true;
		}
		if (name == PropertyName._measurementPreviousFrameTicks)
		{
			value = VariantUtils.CreateFrom(in _measurementPreviousFrameTicks);
			return true;
		}
		if (name == PropertyName._screenshotRequested)
		{
			value = VariantUtils.CreateFrom(in _screenshotRequested);
			return true;
		}
		if (name == PropertyName._screenshotSaved)
		{
			value = VariantUtils.CreateFrom(in _screenshotSaved);
			return true;
		}
		if (name == PropertyName._resultPrinted)
		{
			value = VariantUtils.CreateFrom(in _resultPrinted);
			return true;
		}
		if (name == PropertyName._performanceResultPassed)
		{
			value = VariantUtils.CreateFrom(in _performanceResultPassed);
			return true;
		}
		if (name == PropertyName._animeFrameRateSwitchCompleted)
		{
			value = VariantUtils.CreateFrom(in _animeFrameRateSwitchCompleted);
			return true;
		}
		if (name == PropertyName._animeFrameRateSwitchPassed)
		{
			value = VariantUtils.CreateFrom(in _animeFrameRateSwitchPassed);
			return true;
		}
		if (name == PropertyName._runtimeVisualMutationCompleted)
		{
			value = VariantUtils.CreateFrom(in _runtimeVisualMutationCompleted);
			return true;
		}
		if (name == PropertyName._runtimeVisualMutationPassed)
		{
			value = VariantUtils.CreateFrom(in _runtimeVisualMutationPassed);
			return true;
		}
		if (name == PropertyName._animationSwitchUseSecondClip)
		{
			value = VariantUtils.CreateFrom(in _animationSwitchUseSecondClip);
			return true;
		}
		if (name == PropertyName._nextAnimationSwitchSeconds)
		{
			value = VariantUtils.CreateFrom(in _nextAnimationSwitchSeconds);
			return true;
		}
		if (name == PropertyName._animationSwitchWindowEndSeconds)
		{
			value = VariantUtils.CreateFrom(in _animationSwitchWindowEndSeconds);
			return true;
		}
		if (name == PropertyName._animationSwitchCount)
		{
			value = VariantUtils.CreateFrom(in _animationSwitchCount);
			return true;
		}
		if (name == PropertyName._animationSwitchAppliedCount)
		{
			value = VariantUtils.CreateFrom(in _animationSwitchAppliedCount);
			return true;
		}
		if (name == PropertyName._animationSwitchMissingClipCount)
		{
			value = VariantUtils.CreateFrom(in _animationSwitchMissingClipCount);
			return true;
		}
		if (name == PropertyName._animationSwitchSynchronousTotalMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _animationSwitchSynchronousTotalMilliseconds);
			return true;
		}
		if (name == PropertyName._animationSwitchSynchronousMaxMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _animationSwitchSynchronousMaxMilliseconds);
			return true;
		}
		if (name == PropertyName._nextBowlingImpactSeconds)
		{
			value = VariantUtils.CreateFrom(in _nextBowlingImpactSeconds);
			return true;
		}
		if (name == PropertyName._bowlingImpactWindowEndSeconds)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactWindowEndSeconds);
			return true;
		}
		if (name == PropertyName._bowlingImpactBurstCount)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactBurstCount);
			return true;
		}
		if (name == PropertyName._bowlingImpactAppliedCount)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactAppliedCount);
			return true;
		}
		if (name == PropertyName._bowlingImpactMissingShieldCount)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactMissingShieldCount);
			return true;
		}
		if (name == PropertyName._bowlingImpactMaxProcessedTweens)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactMaxProcessedTweens);
			return true;
		}
		if (name == PropertyName._bowlingImpactMaxBatchedMotions)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactMaxBatchedMotions);
			return true;
		}
		if (name == PropertyName._bowlingImpactMaxBatchedShieldVisuals)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactMaxBatchedShieldVisuals);
			return true;
		}
		if (name == PropertyName._bowlingImpactSynchronousTotalMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactSynchronousTotalMilliseconds);
			return true;
		}
		if (name == PropertyName._bowlingImpactSynchronousMaxMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactSynchronousMaxMilliseconds);
			return true;
		}
		if (name == PropertyName._bowlingImpactThreadAllocatedBytes)
		{
			value = VariantUtils.CreateFrom(in _bowlingImpactThreadAllocatedBytes);
			return true;
		}
		if (name == PropertyName._viewportEntryCaptureFramesRemaining)
		{
			value = VariantUtils.CreateFrom(in _viewportEntryCaptureFramesRemaining);
			return true;
		}
		if (name == PropertyName._viewportEntryCaptureSkipCurrentFrame)
		{
			value = VariantUtils.CreateFrom(in _viewportEntryCaptureSkipCurrentFrame);
			return true;
		}
		if (name == PropertyName._viewportEntryCapturePrinted)
		{
			value = VariantUtils.CreateFrom(in _viewportEntryCapturePrinted);
			return true;
		}
		if (name == PropertyName._measurementThreadAllocatedBefore)
		{
			value = VariantUtils.CreateFrom(in _measurementThreadAllocatedBefore);
			return true;
		}
		if (name == PropertyName._measurementTotalAllocatedBefore)
		{
			value = VariantUtils.CreateFrom(in _measurementTotalAllocatedBefore);
			return true;
		}
		if (name == PropertyName._measurementGen0Before)
		{
			value = VariantUtils.CreateFrom(in _measurementGen0Before);
			return true;
		}
		if (name == PropertyName._measurementGen1Before)
		{
			value = VariantUtils.CreateFrom(in _measurementGen1Before);
			return true;
		}
		if (name == PropertyName._measurementGen2Before)
		{
			value = VariantUtils.CreateFrom(in _measurementGen2Before);
			return true;
		}
		if (name == PropertyName._cadenceSprite)
		{
			value = VariantUtils.CreateFrom(in _cadenceSprite);
			return true;
		}
		if (name == PropertyName._cadencePreviousPhase)
		{
			value = VariantUtils.CreateFrom(in _cadencePreviousPhase);
			return true;
		}
		if (name == PropertyName._cadenceSamples)
		{
			value = VariantUtils.CreateFrom(in _cadenceSamples);
			return true;
		}
		if (name == PropertyName._cadenceAdvances)
		{
			value = VariantUtils.CreateFrom(in _cadenceAdvances);
			return true;
		}
		if (name == PropertyName._cadenceSlot)
		{
			value = VariantUtils.CreateFrom(in _cadenceSlot);
			return true;
		}
		if (name == PropertyName._cadencePreviousSlotVersion)
		{
			value = VariantUtils.CreateFrom(in _cadencePreviousSlotVersion);
			return true;
		}
		if (name == PropertyName._cadenceSlotAdvances)
		{
			value = VariantUtils.CreateFrom(in _cadenceSlotAdvances);
			return true;
		}
		if (name == PropertyName._zInterleaveMarker)
		{
			value = VariantUtils.CreateFrom(in _zInterleaveMarker);
			return true;
		}
		if (name == PropertyName._zInterleaveParticles)
		{
			value = VariantUtils.CreateFrom(in _zInterleaveParticles);
			return true;
		}
		if (name == PropertyName._zInterleaveParticleTexture)
		{
			value = VariantUtils.CreateFrom(in _zInterleaveParticleTexture);
			return true;
		}
		if (name == PropertyName._externalVisualExpectedActive)
		{
			value = VariantUtils.CreateFrom(in _externalVisualExpectedActive);
			return true;
		}
		if (name == PropertyName._externalVisualConfigurationFailures)
		{
			value = VariantUtils.CreateFrom(in _externalVisualConfigurationFailures);
			return true;
		}
		if (name == PropertyName._externalVisualGraphBuildCountAtMeasurementStart)
		{
			value = VariantUtils.CreateFrom(in _externalVisualGraphBuildCountAtMeasurementStart);
			return true;
		}
		if (name == PropertyName._gpuGraphInitialBuildCountAtMeasurementStart)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphInitialBuildCountAtMeasurementStart);
			return true;
		}
		if (name == PropertyName._gpuGraphRebuildCountAtMeasurementStart)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphRebuildCountAtMeasurementStart);
			return true;
		}
		if (name == PropertyName._gpuGraphOwnerExitInvalidationCountAtMeasurementStart)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphOwnerExitInvalidationCountAtMeasurementStart);
			return true;
		}
		if (name == PropertyName._gpuGraphManagedSlotInvalidationCountAtMeasurementStart)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphManagedSlotInvalidationCountAtMeasurementStart);
			return true;
		}
		if (name == PropertyName._gpuGraphExternalVisualInvalidationCountAtMeasurementStart)
		{
			value = VariantUtils.CreateFrom(in _gpuGraphExternalVisualInvalidationCountAtMeasurementStart);
			return true;
		}
		if (name == PropertyName._externalVisualStressTexture)
		{
			value = VariantUtils.CreateFrom(in _externalVisualStressTexture);
			return true;
		}
		if (name == PropertyName._originalMaxFps)
		{
			value = VariantUtils.CreateFrom(in _originalMaxFps);
			return true;
		}
		if (name == PropertyName._originalPhysicsTicksPerSecond)
		{
			value = VariantUtils.CreateFrom(in _originalPhysicsTicksPerSecond);
			return true;
		}
		if (name == PropertyName._invalidTestRenderBackend)
		{
			value = VariantUtils.CreateFrom(in _invalidTestRenderBackend);
			return true;
		}
		if (name == PropertyName._spawnInstantiateMaxMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _spawnInstantiateMaxMilliseconds);
			return true;
		}
		if (name == PropertyName._spawnInstantiateMaxScene)
		{
			value = VariantUtils.CreateFrom(in _spawnInstantiateMaxScene);
			return true;
		}
		if (name == PropertyName._spawnAddChildReadyMaxMilliseconds)
		{
			value = VariantUtils.CreateFrom(in _spawnAddChildReadyMaxMilliseconds);
			return true;
		}
		if (name == PropertyName._spawnAddChildReadyMaxScene)
		{
			value = VariantUtils.CreateFrom(in _spawnAddChildReadyMaxScene);
			return true;
		}
		if (name == PropertyName._adobeFixedAnimationFrame)
		{
			value = VariantUtils.CreateFrom(in _adobeFixedAnimationFrame);
			return true;
		}
		if (name == PropertyName._invalidAdobeFixedAnimationFrame)
		{
			value = VariantUtils.CreateFrom(in _invalidAdobeFixedAnimationFrame);
			return true;
		}
		if (name == PropertyName._renderBackendOption)
		{
			value = VariantUtils.CreateFrom(in _renderBackendOption);
			return true;
		}
		if (name == PropertyName._renderBackendStatus)
		{
			value = VariantUtils.CreateFrom(in _renderBackendStatus);
			return true;
		}
		if (name == PropertyName._renderBackendUiRefreshTimer)
		{
			value = VariantUtils.CreateFrom(in _renderBackendUiRefreshTimer);
			return true;
		}
		if (name == PropertyName._combatStressControl)
		{
			value = VariantUtils.CreateFrom(in _combatStressControl);
			return true;
		}
		if (name == PropertyName._combatStressMapFeature)
		{
			value = VariantUtils.CreateFrom(in _combatStressMapFeature);
			return true;
		}
		if (name == PropertyName._combatStressMapControl)
		{
			value = VariantUtils.CreateFrom(in _combatStressMapControl);
			return true;
		}
		if (name == PropertyName._previousControl)
		{
			value = VariantUtils.CreateFrom(in _previousControl);
			return true;
		}
		if (name == PropertyName._previousGridBegin)
		{
			value = VariantUtils.CreateFrom(in _previousGridBegin);
			return true;
		}
		if (name == PropertyName._previousGridSize)
		{
			value = VariantUtils.CreateFrom(in _previousGridSize);
			return true;
		}
		if (name == PropertyName._previousGridNum)
		{
			value = VariantUtils.CreateFrom(in _previousGridNum);
			return true;
		}
		if (name == PropertyName._previousBackZombie)
		{
			value = VariantUtils.CreateFrom(in _previousBackZombie);
			return true;
		}
		if (name == PropertyName._combatStressConfigured)
		{
			value = VariantUtils.CreateFrom(in _combatStressConfigured);
			return true;
		}
		if (name == PropertyName._resourceManagerLoadGatePending)
		{
			value = VariantUtils.CreateFrom(in _resourceManagerLoadGatePending);
			return true;
		}
		if (name == PropertyName._originalLowProcessorUsageMode)
		{
			value = VariantUtils.CreateFrom(in _originalLowProcessorUsageMode);
			return true;
		}
		if (name == PropertyName._unfocusedRunConfigured)
		{
			value = VariantUtils.CreateFrom(in _unfocusedRunConfigured);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.TestScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ExactScenePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ExactAnimationClip, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.SpawnCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.SpawnOrigin, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.SpawnArea, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UseDifferentZombieScenePool, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UseZombieCharacterScenes, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ZombieSceneRoot, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.CharacterSceneFilePrefix, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ExcludedZombieScenePathSubstring, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.MaxDifferentZombieScenes, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.SpawnBatchSize, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.SpawnTimeBudgetMilliseconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UseGridSpawnLayout, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.SpawnGridColumns, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.RenderZBucketCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.SpawnScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.SpawnRandomSeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.AutoQuitAfterSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.WarmupSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.MinimumAverageFps, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.MaximumFrameP99Milliseconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.MaximumFrameMilliseconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.MinimumOnePercentLowFps, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.BenchmarkMaxFps, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.BenchmarkPhysicsTicksPerSecond, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.ScreenshotPath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.WaitForDamagePartSettledBeforeScreenshot, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.PrintIntervalSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableAdobeAnimateProfiler, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableDetailedAdobeAnimateProfiler, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableAllocationTelemetry, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ProfilerDumpIntervalFrames, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ProfilerMaxMetricsPerDump, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ClearAdobeAnimateRuntimeCachesBeforeRun, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableGpuRenderGraph, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ForceCpuPoseFallbackForTest, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.FixedAnimationFrame, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.FixedAnimationSubframe, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.PrintCrowdFilterDebugData, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CrowdFilterDebugSpriteCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.CrowdFilterDebugMaxFrameSlices, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.PrintCrowdFilterDebugAfterFirstRuntimeFrame, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.PrintCrowdFilterDebugEveryRuntimeSnapshot, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableZInterleaveFixture, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.PrintCrowdModeReport, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.InitialAnimeFrameRate, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.SwitchAnimeFrameRateAfterSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.SwitchAnimeFrameRateTo, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.RuntimeVisualMutation, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.RuntimeVisualMutationAfterSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableAnimationSwitchStress, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.AnimationSwitchClipA, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.AnimationSwitchClipB, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.AnimationSwitchIntervalSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.AnimationSwitchBlendSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.AnimationSwitchStartAfterSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableBowlingImpactTweenStress, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.BowlingImpactEnsureShield, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.BowlingImpactUseProductionEvent, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.BowlingImpactProcessedTweenLimit, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.BowlingImpactTargetsPerBurst, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.BowlingImpactDamage, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.BowlingImpactBurstIntervalSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.BowlingImpactActiveWindowSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.BowlingImpactStartAfterSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RunComponentAliveSchedulingRegression, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RunStatePhysicsBatchRateRegression, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RunAnimationCadenceRegression, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableExternalVisualStress, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ExternalVisualIceOnly, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.ExternalVisualRatio, PropertyHint.Range, "0,1,0.01", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableCombatStateStress, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.PreserveCombatCharactersInStress, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.CombatStressIsNight, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ShowCharacterHealthInStress, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableSingleEnemyTargetFixture, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.SingleEnemyTargetScenePath, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.SingleEnemyTargetScale, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableSleepCycleStress, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.SleepCycleStartAfterSeconds, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.EnableManyEnemyIdleFixture, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName.ManyEnemyIdleTargetCount, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ManyEnemyIdleTargetsOutsideAttackRange, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.WaitForResourceManagerLoad, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.KeepRunningWhenUnfocused, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName.RuntimeProfile, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Int, PropertyName._testRenderBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedInt32Array, PropertyName._sceneUseCounts, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._printTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._windowDeltaSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._sampleStartSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._windowFrames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeCrowdFilterDebugPrinted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._crowdModeReportPrinted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._spawnFailures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._loadedScenePoolCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._skippedScenePoolCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._excludedScenePoolCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextSpawnIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextSpawnProgressPrint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._spawnStartTicks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._loadMs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._spawnNoGcRegionActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pendingSpawnFallbackScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._spawnPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._spawnFrameBudgetPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._autoQuitPrinted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._measurementStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementFrames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._singleEnemyTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._singleEnemyAttackComponentCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._singleEnemyAttackUtilityComponentCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._singleEnemyFireComponentCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._singleEnemyTargetInitialHitpoints, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._singleEnemyFixturePrepared, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._singleEnemyRobotModeAdvanceRequested, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._singleEnemyRoleUtilityPhaseStarted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._singleEnemyRoleUtilityPhaseCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._manyEnemyIdleFixturePrepared, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._nextSingleEnemyRoleUtilityAuditSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._sleepCyclePhase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._sleepCyclePhaseDeadlineSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sleepCycleCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sleepCyclePassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._sleepCycleEligibleCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._sleepCycleAppliedCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._sleepCycleEnteredCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._sleepCycleWokenCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._sleepCycleNormalizedImmunityCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sleepCycleImmunityRestored, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sleepCycleHypnotistEventProvider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._measurementStartSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._measurementEndSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementPreviousFrameTicks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._screenshotRequested, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._screenshotSaved, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._resultPrinted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._performanceResultPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._animeFrameRateSwitchCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._animeFrameRateSwitchPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeVisualMutationCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._runtimeVisualMutationPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._animationSwitchUseSecondClip, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._nextAnimationSwitchSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._animationSwitchWindowEndSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationSwitchCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationSwitchAppliedCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._animationSwitchMissingClipCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._animationSwitchSynchronousTotalMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._animationSwitchSynchronousMaxMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._nextBowlingImpactSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._bowlingImpactWindowEndSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._bowlingImpactBurstCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._bowlingImpactAppliedCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._bowlingImpactMissingShieldCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._bowlingImpactMaxProcessedTweens, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._bowlingImpactMaxBatchedMotions, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._bowlingImpactMaxBatchedShieldVisuals, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._bowlingImpactSynchronousTotalMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._bowlingImpactSynchronousMaxMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._bowlingImpactThreadAllocatedBytes, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._viewportEntryCaptureFramesRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._viewportEntryCaptureSkipCurrentFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._viewportEntryCapturePrinted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementThreadAllocatedBefore, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementTotalAllocatedBefore, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementGen0Before, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementGen1Before, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._measurementGen2Before, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cadenceSprite, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._cadencePreviousPhase, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cadenceSamples, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cadenceAdvances, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cadenceSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cadencePreviousSlotVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._cadenceSlotAdvances, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zInterleaveMarker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zInterleaveParticles, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zInterleaveParticleTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._externalVisualExpectedActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._externalVisualConfigurationFailures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._externalVisualGraphBuildCountAtMeasurementStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphInitialBuildCountAtMeasurementStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphRebuildCountAtMeasurementStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphOwnerExitInvalidationCountAtMeasurementStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphManagedSlotInvalidationCountAtMeasurementStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._gpuGraphExternalVisualInvalidationCountAtMeasurementStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._externalVisualStressTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalMaxFps, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._originalPhysicsTicksPerSecond, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._invalidTestRenderBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._spawnInstantiateMaxMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._spawnInstantiateMaxScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._spawnAddChildReadyMaxMilliseconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._spawnAddChildReadyMaxScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._adobeFixedAnimationFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._invalidAdobeFixedAnimationFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renderBackendOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renderBackendStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._renderBackendUiRefreshTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._combatStressControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._combatStressMapFeature, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._combatStressMapControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previousGridBegin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._previousGridSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._previousGridNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._previousBackZombie, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._combatStressConfigured, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._resourceManagerLoadGatePending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._originalLowProcessorUsageMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._unfocusedRunConfigured, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.TestScene, Variant.From<PackedScene>(TestScene));
		info.AddProperty(PropertyName.ExactScenePath, Variant.From<string>(ExactScenePath));
		info.AddProperty(PropertyName.ExactAnimationClip, Variant.From<string>(ExactAnimationClip));
		info.AddProperty(PropertyName.SpawnCount, Variant.From<int>(SpawnCount));
		info.AddProperty(PropertyName.SpawnOrigin, Variant.From<Vector2>(SpawnOrigin));
		info.AddProperty(PropertyName.SpawnArea, Variant.From<Vector2>(SpawnArea));
		info.AddProperty(PropertyName.UseDifferentZombieScenePool, Variant.From<bool>(UseDifferentZombieScenePool));
		info.AddProperty(PropertyName.UseZombieCharacterScenes, Variant.From<bool>(UseZombieCharacterScenes));
		info.AddProperty(PropertyName.ZombieSceneRoot, Variant.From<string>(ZombieSceneRoot));
		info.AddProperty(PropertyName.CharacterSceneFilePrefix, Variant.From<string>(CharacterSceneFilePrefix));
		info.AddProperty(PropertyName.ExcludedZombieScenePathSubstring, Variant.From<string>(ExcludedZombieScenePathSubstring));
		info.AddProperty(PropertyName.MaxDifferentZombieScenes, Variant.From<int>(MaxDifferentZombieScenes));
		info.AddProperty(PropertyName.SpawnBatchSize, Variant.From<int>(SpawnBatchSize));
		info.AddProperty(PropertyName.SpawnTimeBudgetMilliseconds, Variant.From<double>(SpawnTimeBudgetMilliseconds));
		info.AddProperty(PropertyName.UseGridSpawnLayout, Variant.From<bool>(UseGridSpawnLayout));
		info.AddProperty(PropertyName.SpawnGridColumns, Variant.From<int>(SpawnGridColumns));
		info.AddProperty(PropertyName.RenderZBucketCount, Variant.From<int>(RenderZBucketCount));
		info.AddProperty(PropertyName.SpawnScale, Variant.From<float>(SpawnScale));
		info.AddProperty(PropertyName.SpawnRandomSeed, Variant.From<ulong>(SpawnRandomSeed));
		info.AddProperty(PropertyName.AutoQuitAfterSeconds, Variant.From<double>(AutoQuitAfterSeconds));
		info.AddProperty(PropertyName.WarmupSeconds, Variant.From<double>(WarmupSeconds));
		info.AddProperty(PropertyName.MinimumAverageFps, Variant.From<double>(MinimumAverageFps));
		info.AddProperty(PropertyName.MaximumFrameP99Milliseconds, Variant.From<double>(MaximumFrameP99Milliseconds));
		info.AddProperty(PropertyName.MaximumFrameMilliseconds, Variant.From<double>(MaximumFrameMilliseconds));
		info.AddProperty(PropertyName.MinimumOnePercentLowFps, Variant.From<double>(MinimumOnePercentLowFps));
		info.AddProperty(PropertyName.BenchmarkMaxFps, Variant.From<int>(BenchmarkMaxFps));
		info.AddProperty(PropertyName.BenchmarkPhysicsTicksPerSecond, Variant.From<int>(BenchmarkPhysicsTicksPerSecond));
		info.AddProperty(PropertyName.ScreenshotPath, Variant.From<string>(ScreenshotPath));
		info.AddProperty(PropertyName.WaitForDamagePartSettledBeforeScreenshot, Variant.From<bool>(WaitForDamagePartSettledBeforeScreenshot));
		info.AddProperty(PropertyName.PrintIntervalSeconds, Variant.From<float>(PrintIntervalSeconds));
		info.AddProperty(PropertyName.EnableAdobeAnimateProfiler, Variant.From<bool>(EnableAdobeAnimateProfiler));
		info.AddProperty(PropertyName.EnableDetailedAdobeAnimateProfiler, Variant.From<bool>(EnableDetailedAdobeAnimateProfiler));
		info.AddProperty(PropertyName.EnableAllocationTelemetry, Variant.From<bool>(EnableAllocationTelemetry));
		info.AddProperty(PropertyName.ProfilerDumpIntervalFrames, Variant.From<int>(ProfilerDumpIntervalFrames));
		info.AddProperty(PropertyName.ProfilerMaxMetricsPerDump, Variant.From<int>(ProfilerMaxMetricsPerDump));
		info.AddProperty(PropertyName.ClearAdobeAnimateRuntimeCachesBeforeRun, Variant.From<bool>(ClearAdobeAnimateRuntimeCachesBeforeRun));
		info.AddProperty(PropertyName.EnableGpuRenderGraph, Variant.From<bool>(EnableGpuRenderGraph));
		info.AddProperty(PropertyName.ForceCpuPoseFallbackForTest, Variant.From<bool>(ForceCpuPoseFallbackForTest));
		info.AddProperty(PropertyName.FixedAnimationFrame, Variant.From<int>(FixedAnimationFrame));
		info.AddProperty(PropertyName.FixedAnimationSubframe, Variant.From<double>(FixedAnimationSubframe));
		info.AddProperty(PropertyName.PrintCrowdFilterDebugData, Variant.From<bool>(PrintCrowdFilterDebugData));
		info.AddProperty(PropertyName.CrowdFilterDebugSpriteCount, Variant.From<int>(CrowdFilterDebugSpriteCount));
		info.AddProperty(PropertyName.CrowdFilterDebugMaxFrameSlices, Variant.From<int>(CrowdFilterDebugMaxFrameSlices));
		info.AddProperty(PropertyName.PrintCrowdFilterDebugAfterFirstRuntimeFrame, Variant.From<bool>(PrintCrowdFilterDebugAfterFirstRuntimeFrame));
		info.AddProperty(PropertyName.PrintCrowdFilterDebugEveryRuntimeSnapshot, Variant.From<bool>(PrintCrowdFilterDebugEveryRuntimeSnapshot));
		info.AddProperty(PropertyName.EnableZInterleaveFixture, Variant.From<bool>(EnableZInterleaveFixture));
		info.AddProperty(PropertyName.PrintCrowdModeReport, Variant.From<bool>(PrintCrowdModeReport));
		info.AddProperty(PropertyName.InitialAnimeFrameRate, Variant.From<double>(InitialAnimeFrameRate));
		info.AddProperty(PropertyName.SwitchAnimeFrameRateAfterSeconds, Variant.From<double>(SwitchAnimeFrameRateAfterSeconds));
		info.AddProperty(PropertyName.SwitchAnimeFrameRateTo, Variant.From<double>(SwitchAnimeFrameRateTo));
		info.AddProperty(PropertyName.RuntimeVisualMutation, Variant.From<string>(RuntimeVisualMutation));
		info.AddProperty(PropertyName.RuntimeVisualMutationAfterSeconds, Variant.From<double>(RuntimeVisualMutationAfterSeconds));
		info.AddProperty(PropertyName.EnableAnimationSwitchStress, Variant.From<bool>(EnableAnimationSwitchStress));
		info.AddProperty(PropertyName.AnimationSwitchClipA, Variant.From<string>(AnimationSwitchClipA));
		info.AddProperty(PropertyName.AnimationSwitchClipB, Variant.From<string>(AnimationSwitchClipB));
		info.AddProperty(PropertyName.AnimationSwitchIntervalSeconds, Variant.From<double>(AnimationSwitchIntervalSeconds));
		info.AddProperty(PropertyName.AnimationSwitchBlendSeconds, Variant.From<double>(AnimationSwitchBlendSeconds));
		info.AddProperty(PropertyName.AnimationSwitchStartAfterSeconds, Variant.From<double>(AnimationSwitchStartAfterSeconds));
		info.AddProperty(PropertyName.EnableBowlingImpactTweenStress, Variant.From<bool>(EnableBowlingImpactTweenStress));
		info.AddProperty(PropertyName.BowlingImpactEnsureShield, Variant.From<bool>(BowlingImpactEnsureShield));
		info.AddProperty(PropertyName.BowlingImpactUseProductionEvent, Variant.From<bool>(BowlingImpactUseProductionEvent));
		info.AddProperty(PropertyName.BowlingImpactProcessedTweenLimit, Variant.From<int>(BowlingImpactProcessedTweenLimit));
		info.AddProperty(PropertyName.BowlingImpactTargetsPerBurst, Variant.From<int>(BowlingImpactTargetsPerBurst));
		info.AddProperty(PropertyName.BowlingImpactDamage, Variant.From<double>(BowlingImpactDamage));
		info.AddProperty(PropertyName.BowlingImpactBurstIntervalSeconds, Variant.From<double>(BowlingImpactBurstIntervalSeconds));
		info.AddProperty(PropertyName.BowlingImpactActiveWindowSeconds, Variant.From<double>(BowlingImpactActiveWindowSeconds));
		info.AddProperty(PropertyName.BowlingImpactStartAfterSeconds, Variant.From<double>(BowlingImpactStartAfterSeconds));
		info.AddProperty(PropertyName.RunComponentAliveSchedulingRegression, Variant.From<bool>(RunComponentAliveSchedulingRegression));
		info.AddProperty(PropertyName.RunStatePhysicsBatchRateRegression, Variant.From<bool>(RunStatePhysicsBatchRateRegression));
		info.AddProperty(PropertyName.RunAnimationCadenceRegression, Variant.From<bool>(RunAnimationCadenceRegression));
		info.AddProperty(PropertyName.EnableExternalVisualStress, Variant.From<bool>(EnableExternalVisualStress));
		info.AddProperty(PropertyName.ExternalVisualIceOnly, Variant.From<bool>(ExternalVisualIceOnly));
		info.AddProperty(PropertyName.ExternalVisualRatio, Variant.From<double>(ExternalVisualRatio));
		info.AddProperty(PropertyName.EnableCombatStateStress, Variant.From<bool>(EnableCombatStateStress));
		info.AddProperty(PropertyName.PreserveCombatCharactersInStress, Variant.From<bool>(PreserveCombatCharactersInStress));
		info.AddProperty(PropertyName.CombatStressIsNight, Variant.From<bool>(CombatStressIsNight));
		info.AddProperty(PropertyName.ShowCharacterHealthInStress, Variant.From<bool>(ShowCharacterHealthInStress));
		info.AddProperty(PropertyName.EnableSingleEnemyTargetFixture, Variant.From<bool>(EnableSingleEnemyTargetFixture));
		info.AddProperty(PropertyName.SingleEnemyTargetScenePath, Variant.From<string>(SingleEnemyTargetScenePath));
		info.AddProperty(PropertyName.SingleEnemyTargetScale, Variant.From<float>(SingleEnemyTargetScale));
		info.AddProperty(PropertyName.EnableSleepCycleStress, Variant.From<bool>(EnableSleepCycleStress));
		info.AddProperty(PropertyName.SleepCycleStartAfterSeconds, Variant.From<double>(SleepCycleStartAfterSeconds));
		info.AddProperty(PropertyName.EnableManyEnemyIdleFixture, Variant.From<bool>(EnableManyEnemyIdleFixture));
		info.AddProperty(PropertyName.ManyEnemyIdleTargetCount, Variant.From<int>(ManyEnemyIdleTargetCount));
		info.AddProperty(PropertyName.ManyEnemyIdleTargetsOutsideAttackRange, Variant.From<bool>(ManyEnemyIdleTargetsOutsideAttackRange));
		info.AddProperty(PropertyName.WaitForResourceManagerLoad, Variant.From<bool>(WaitForResourceManagerLoad));
		info.AddProperty(PropertyName.KeepRunningWhenUnfocused, Variant.From<bool>(KeepRunningWhenUnfocused));
		info.AddProperty(PropertyName.RuntimeProfile, Variant.From<string>(RuntimeProfile));
		info.AddProperty(PropertyName._testRenderBackend, Variant.From(in _testRenderBackend));
		info.AddProperty(PropertyName._sceneUseCounts, Variant.From(in _sceneUseCounts));
		info.AddProperty(PropertyName._printTimer, Variant.From(in _printTimer));
		info.AddProperty(PropertyName._windowDeltaSeconds, Variant.From(in _windowDeltaSeconds));
		info.AddProperty(PropertyName._sampleStartSeconds, Variant.From(in _sampleStartSeconds));
		info.AddProperty(PropertyName._windowFrames, Variant.From(in _windowFrames));
		info.AddProperty(PropertyName._runtimeCrowdFilterDebugPrinted, Variant.From(in _runtimeCrowdFilterDebugPrinted));
		info.AddProperty(PropertyName._crowdModeReportPrinted, Variant.From(in _crowdModeReportPrinted));
		info.AddProperty(PropertyName._spawnFailures, Variant.From(in _spawnFailures));
		info.AddProperty(PropertyName._loadedScenePoolCount, Variant.From(in _loadedScenePoolCount));
		info.AddProperty(PropertyName._skippedScenePoolCount, Variant.From(in _skippedScenePoolCount));
		info.AddProperty(PropertyName._excludedScenePoolCount, Variant.From(in _excludedScenePoolCount));
		info.AddProperty(PropertyName._nextSpawnIndex, Variant.From(in _nextSpawnIndex));
		info.AddProperty(PropertyName._nextSpawnProgressPrint, Variant.From(in _nextSpawnProgressPrint));
		info.AddProperty(PropertyName._spawnStartTicks, Variant.From(in _spawnStartTicks));
		info.AddProperty(PropertyName._loadMs, Variant.From(in _loadMs));
		info.AddProperty(PropertyName._spawnNoGcRegionActive, Variant.From(in _spawnNoGcRegionActive));
		info.AddProperty(PropertyName._pendingSpawnFallbackScene, Variant.From(in _pendingSpawnFallbackScene));
		info.AddProperty(PropertyName._spawnPending, Variant.From(in _spawnPending));
		info.AddProperty(PropertyName._spawnFrameBudgetPassed, Variant.From(in _spawnFrameBudgetPassed));
		info.AddProperty(PropertyName._autoQuitPrinted, Variant.From(in _autoQuitPrinted));
		info.AddProperty(PropertyName._measurementStarted, Variant.From(in _measurementStarted));
		info.AddProperty(PropertyName._measurementFrames, Variant.From(in _measurementFrames));
		info.AddProperty(PropertyName._singleEnemyTarget, Variant.From(in _singleEnemyTarget));
		info.AddProperty(PropertyName._singleEnemyAttackComponentCount, Variant.From(in _singleEnemyAttackComponentCount));
		info.AddProperty(PropertyName._singleEnemyAttackUtilityComponentCount, Variant.From(in _singleEnemyAttackUtilityComponentCount));
		info.AddProperty(PropertyName._singleEnemyFireComponentCount, Variant.From(in _singleEnemyFireComponentCount));
		info.AddProperty(PropertyName._singleEnemyTargetInitialHitpoints, Variant.From(in _singleEnemyTargetInitialHitpoints));
		info.AddProperty(PropertyName._singleEnemyFixturePrepared, Variant.From(in _singleEnemyFixturePrepared));
		info.AddProperty(PropertyName._singleEnemyRobotModeAdvanceRequested, Variant.From(in _singleEnemyRobotModeAdvanceRequested));
		info.AddProperty(PropertyName._singleEnemyRoleUtilityPhaseStarted, Variant.From(in _singleEnemyRoleUtilityPhaseStarted));
		info.AddProperty(PropertyName._singleEnemyRoleUtilityPhaseCompleted, Variant.From(in _singleEnemyRoleUtilityPhaseCompleted));
		info.AddProperty(PropertyName._manyEnemyIdleFixturePrepared, Variant.From(in _manyEnemyIdleFixturePrepared));
		info.AddProperty(PropertyName._nextSingleEnemyRoleUtilityAuditSeconds, Variant.From(in _nextSingleEnemyRoleUtilityAuditSeconds));
		info.AddProperty(PropertyName._sleepCyclePhase, Variant.From(in _sleepCyclePhase));
		info.AddProperty(PropertyName._sleepCyclePhaseDeadlineSeconds, Variant.From(in _sleepCyclePhaseDeadlineSeconds));
		info.AddProperty(PropertyName._sleepCycleCompleted, Variant.From(in _sleepCycleCompleted));
		info.AddProperty(PropertyName._sleepCyclePassed, Variant.From(in _sleepCyclePassed));
		info.AddProperty(PropertyName._sleepCycleEligibleCount, Variant.From(in _sleepCycleEligibleCount));
		info.AddProperty(PropertyName._sleepCycleAppliedCount, Variant.From(in _sleepCycleAppliedCount));
		info.AddProperty(PropertyName._sleepCycleEnteredCount, Variant.From(in _sleepCycleEnteredCount));
		info.AddProperty(PropertyName._sleepCycleWokenCount, Variant.From(in _sleepCycleWokenCount));
		info.AddProperty(PropertyName._sleepCycleNormalizedImmunityCount, Variant.From(in _sleepCycleNormalizedImmunityCount));
		info.AddProperty(PropertyName._sleepCycleImmunityRestored, Variant.From(in _sleepCycleImmunityRestored));
		info.AddProperty(PropertyName._sleepCycleHypnotistEventProvider, Variant.From(in _sleepCycleHypnotistEventProvider));
		info.AddProperty(PropertyName._measurementStartSeconds, Variant.From(in _measurementStartSeconds));
		info.AddProperty(PropertyName._measurementEndSeconds, Variant.From(in _measurementEndSeconds));
		info.AddProperty(PropertyName._measurementPreviousFrameTicks, Variant.From(in _measurementPreviousFrameTicks));
		info.AddProperty(PropertyName._screenshotRequested, Variant.From(in _screenshotRequested));
		info.AddProperty(PropertyName._screenshotSaved, Variant.From(in _screenshotSaved));
		info.AddProperty(PropertyName._resultPrinted, Variant.From(in _resultPrinted));
		info.AddProperty(PropertyName._performanceResultPassed, Variant.From(in _performanceResultPassed));
		info.AddProperty(PropertyName._animeFrameRateSwitchCompleted, Variant.From(in _animeFrameRateSwitchCompleted));
		info.AddProperty(PropertyName._animeFrameRateSwitchPassed, Variant.From(in _animeFrameRateSwitchPassed));
		info.AddProperty(PropertyName._runtimeVisualMutationCompleted, Variant.From(in _runtimeVisualMutationCompleted));
		info.AddProperty(PropertyName._runtimeVisualMutationPassed, Variant.From(in _runtimeVisualMutationPassed));
		info.AddProperty(PropertyName._animationSwitchUseSecondClip, Variant.From(in _animationSwitchUseSecondClip));
		info.AddProperty(PropertyName._nextAnimationSwitchSeconds, Variant.From(in _nextAnimationSwitchSeconds));
		info.AddProperty(PropertyName._animationSwitchWindowEndSeconds, Variant.From(in _animationSwitchWindowEndSeconds));
		info.AddProperty(PropertyName._animationSwitchCount, Variant.From(in _animationSwitchCount));
		info.AddProperty(PropertyName._animationSwitchAppliedCount, Variant.From(in _animationSwitchAppliedCount));
		info.AddProperty(PropertyName._animationSwitchMissingClipCount, Variant.From(in _animationSwitchMissingClipCount));
		info.AddProperty(PropertyName._animationSwitchSynchronousTotalMilliseconds, Variant.From(in _animationSwitchSynchronousTotalMilliseconds));
		info.AddProperty(PropertyName._animationSwitchSynchronousMaxMilliseconds, Variant.From(in _animationSwitchSynchronousMaxMilliseconds));
		info.AddProperty(PropertyName._nextBowlingImpactSeconds, Variant.From(in _nextBowlingImpactSeconds));
		info.AddProperty(PropertyName._bowlingImpactWindowEndSeconds, Variant.From(in _bowlingImpactWindowEndSeconds));
		info.AddProperty(PropertyName._bowlingImpactBurstCount, Variant.From(in _bowlingImpactBurstCount));
		info.AddProperty(PropertyName._bowlingImpactAppliedCount, Variant.From(in _bowlingImpactAppliedCount));
		info.AddProperty(PropertyName._bowlingImpactMissingShieldCount, Variant.From(in _bowlingImpactMissingShieldCount));
		info.AddProperty(PropertyName._bowlingImpactMaxProcessedTweens, Variant.From(in _bowlingImpactMaxProcessedTweens));
		info.AddProperty(PropertyName._bowlingImpactMaxBatchedMotions, Variant.From(in _bowlingImpactMaxBatchedMotions));
		info.AddProperty(PropertyName._bowlingImpactMaxBatchedShieldVisuals, Variant.From(in _bowlingImpactMaxBatchedShieldVisuals));
		info.AddProperty(PropertyName._bowlingImpactSynchronousTotalMilliseconds, Variant.From(in _bowlingImpactSynchronousTotalMilliseconds));
		info.AddProperty(PropertyName._bowlingImpactSynchronousMaxMilliseconds, Variant.From(in _bowlingImpactSynchronousMaxMilliseconds));
		info.AddProperty(PropertyName._bowlingImpactThreadAllocatedBytes, Variant.From(in _bowlingImpactThreadAllocatedBytes));
		info.AddProperty(PropertyName._viewportEntryCaptureFramesRemaining, Variant.From(in _viewportEntryCaptureFramesRemaining));
		info.AddProperty(PropertyName._viewportEntryCaptureSkipCurrentFrame, Variant.From(in _viewportEntryCaptureSkipCurrentFrame));
		info.AddProperty(PropertyName._viewportEntryCapturePrinted, Variant.From(in _viewportEntryCapturePrinted));
		info.AddProperty(PropertyName._measurementThreadAllocatedBefore, Variant.From(in _measurementThreadAllocatedBefore));
		info.AddProperty(PropertyName._measurementTotalAllocatedBefore, Variant.From(in _measurementTotalAllocatedBefore));
		info.AddProperty(PropertyName._measurementGen0Before, Variant.From(in _measurementGen0Before));
		info.AddProperty(PropertyName._measurementGen1Before, Variant.From(in _measurementGen1Before));
		info.AddProperty(PropertyName._measurementGen2Before, Variant.From(in _measurementGen2Before));
		info.AddProperty(PropertyName._cadenceSprite, Variant.From(in _cadenceSprite));
		info.AddProperty(PropertyName._cadencePreviousPhase, Variant.From(in _cadencePreviousPhase));
		info.AddProperty(PropertyName._cadenceSamples, Variant.From(in _cadenceSamples));
		info.AddProperty(PropertyName._cadenceAdvances, Variant.From(in _cadenceAdvances));
		info.AddProperty(PropertyName._cadenceSlot, Variant.From(in _cadenceSlot));
		info.AddProperty(PropertyName._cadencePreviousSlotVersion, Variant.From(in _cadencePreviousSlotVersion));
		info.AddProperty(PropertyName._cadenceSlotAdvances, Variant.From(in _cadenceSlotAdvances));
		info.AddProperty(PropertyName._zInterleaveMarker, Variant.From(in _zInterleaveMarker));
		info.AddProperty(PropertyName._zInterleaveParticles, Variant.From(in _zInterleaveParticles));
		info.AddProperty(PropertyName._zInterleaveParticleTexture, Variant.From(in _zInterleaveParticleTexture));
		info.AddProperty(PropertyName._externalVisualExpectedActive, Variant.From(in _externalVisualExpectedActive));
		info.AddProperty(PropertyName._externalVisualConfigurationFailures, Variant.From(in _externalVisualConfigurationFailures));
		info.AddProperty(PropertyName._externalVisualGraphBuildCountAtMeasurementStart, Variant.From(in _externalVisualGraphBuildCountAtMeasurementStart));
		info.AddProperty(PropertyName._gpuGraphInitialBuildCountAtMeasurementStart, Variant.From(in _gpuGraphInitialBuildCountAtMeasurementStart));
		info.AddProperty(PropertyName._gpuGraphRebuildCountAtMeasurementStart, Variant.From(in _gpuGraphRebuildCountAtMeasurementStart));
		info.AddProperty(PropertyName._gpuGraphOwnerExitInvalidationCountAtMeasurementStart, Variant.From(in _gpuGraphOwnerExitInvalidationCountAtMeasurementStart));
		info.AddProperty(PropertyName._gpuGraphManagedSlotInvalidationCountAtMeasurementStart, Variant.From(in _gpuGraphManagedSlotInvalidationCountAtMeasurementStart));
		info.AddProperty(PropertyName._gpuGraphExternalVisualInvalidationCountAtMeasurementStart, Variant.From(in _gpuGraphExternalVisualInvalidationCountAtMeasurementStart));
		info.AddProperty(PropertyName._externalVisualStressTexture, Variant.From(in _externalVisualStressTexture));
		info.AddProperty(PropertyName._originalMaxFps, Variant.From(in _originalMaxFps));
		info.AddProperty(PropertyName._originalPhysicsTicksPerSecond, Variant.From(in _originalPhysicsTicksPerSecond));
		info.AddProperty(PropertyName._invalidTestRenderBackend, Variant.From(in _invalidTestRenderBackend));
		info.AddProperty(PropertyName._spawnInstantiateMaxMilliseconds, Variant.From(in _spawnInstantiateMaxMilliseconds));
		info.AddProperty(PropertyName._spawnInstantiateMaxScene, Variant.From(in _spawnInstantiateMaxScene));
		info.AddProperty(PropertyName._spawnAddChildReadyMaxMilliseconds, Variant.From(in _spawnAddChildReadyMaxMilliseconds));
		info.AddProperty(PropertyName._spawnAddChildReadyMaxScene, Variant.From(in _spawnAddChildReadyMaxScene));
		info.AddProperty(PropertyName._adobeFixedAnimationFrame, Variant.From(in _adobeFixedAnimationFrame));
		info.AddProperty(PropertyName._invalidAdobeFixedAnimationFrame, Variant.From(in _invalidAdobeFixedAnimationFrame));
		info.AddProperty(PropertyName._renderBackendOption, Variant.From(in _renderBackendOption));
		info.AddProperty(PropertyName._renderBackendStatus, Variant.From(in _renderBackendStatus));
		info.AddProperty(PropertyName._renderBackendUiRefreshTimer, Variant.From(in _renderBackendUiRefreshTimer));
		info.AddProperty(PropertyName._combatStressControl, Variant.From(in _combatStressControl));
		info.AddProperty(PropertyName._combatStressMapFeature, Variant.From(in _combatStressMapFeature));
		info.AddProperty(PropertyName._combatStressMapControl, Variant.From(in _combatStressMapControl));
		info.AddProperty(PropertyName._previousControl, Variant.From(in _previousControl));
		info.AddProperty(PropertyName._previousGridBegin, Variant.From(in _previousGridBegin));
		info.AddProperty(PropertyName._previousGridSize, Variant.From(in _previousGridSize));
		info.AddProperty(PropertyName._previousGridNum, Variant.From(in _previousGridNum));
		info.AddProperty(PropertyName._previousBackZombie, Variant.From(in _previousBackZombie));
		info.AddProperty(PropertyName._combatStressConfigured, Variant.From(in _combatStressConfigured));
		info.AddProperty(PropertyName._resourceManagerLoadGatePending, Variant.From(in _resourceManagerLoadGatePending));
		info.AddProperty(PropertyName._originalLowProcessorUsageMode, Variant.From(in _originalLowProcessorUsageMode));
		info.AddProperty(PropertyName._unfocusedRunConfigured, Variant.From(in _unfocusedRunConfigured));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.TestScene, out var value))
		{
			TestScene = value.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.ExactScenePath, out var value2))
		{
			ExactScenePath = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ExactAnimationClip, out var value3))
		{
			ExactAnimationClip = value3.As<string>();
		}
		if (info.TryGetProperty(PropertyName.SpawnCount, out var value4))
		{
			SpawnCount = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.SpawnOrigin, out var value5))
		{
			SpawnOrigin = value5.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.SpawnArea, out var value6))
		{
			SpawnArea = value6.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.UseDifferentZombieScenePool, out var value7))
		{
			UseDifferentZombieScenePool = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.UseZombieCharacterScenes, out var value8))
		{
			UseZombieCharacterScenes = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ZombieSceneRoot, out var value9))
		{
			ZombieSceneRoot = value9.As<string>();
		}
		if (info.TryGetProperty(PropertyName.CharacterSceneFilePrefix, out var value10))
		{
			CharacterSceneFilePrefix = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ExcludedZombieScenePathSubstring, out var value11))
		{
			ExcludedZombieScenePathSubstring = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName.MaxDifferentZombieScenes, out var value12))
		{
			MaxDifferentZombieScenes = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName.SpawnBatchSize, out var value13))
		{
			SpawnBatchSize = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName.SpawnTimeBudgetMilliseconds, out var value14))
		{
			SpawnTimeBudgetMilliseconds = value14.As<double>();
		}
		if (info.TryGetProperty(PropertyName.UseGridSpawnLayout, out var value15))
		{
			UseGridSpawnLayout = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.SpawnGridColumns, out var value16))
		{
			SpawnGridColumns = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName.RenderZBucketCount, out var value17))
		{
			RenderZBucketCount = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName.SpawnScale, out var value18))
		{
			SpawnScale = value18.As<float>();
		}
		if (info.TryGetProperty(PropertyName.SpawnRandomSeed, out var value19))
		{
			SpawnRandomSeed = value19.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName.AutoQuitAfterSeconds, out var value20))
		{
			AutoQuitAfterSeconds = value20.As<double>();
		}
		if (info.TryGetProperty(PropertyName.WarmupSeconds, out var value21))
		{
			WarmupSeconds = value21.As<double>();
		}
		if (info.TryGetProperty(PropertyName.MinimumAverageFps, out var value22))
		{
			MinimumAverageFps = value22.As<double>();
		}
		if (info.TryGetProperty(PropertyName.MaximumFrameP99Milliseconds, out var value23))
		{
			MaximumFrameP99Milliseconds = value23.As<double>();
		}
		if (info.TryGetProperty(PropertyName.MaximumFrameMilliseconds, out var value24))
		{
			MaximumFrameMilliseconds = value24.As<double>();
		}
		if (info.TryGetProperty(PropertyName.MinimumOnePercentLowFps, out var value25))
		{
			MinimumOnePercentLowFps = value25.As<double>();
		}
		if (info.TryGetProperty(PropertyName.BenchmarkMaxFps, out var value26))
		{
			BenchmarkMaxFps = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName.BenchmarkPhysicsTicksPerSecond, out var value27))
		{
			BenchmarkPhysicsTicksPerSecond = value27.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ScreenshotPath, out var value28))
		{
			ScreenshotPath = value28.As<string>();
		}
		if (info.TryGetProperty(PropertyName.WaitForDamagePartSettledBeforeScreenshot, out var value29))
		{
			WaitForDamagePartSettledBeforeScreenshot = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.PrintIntervalSeconds, out var value30))
		{
			PrintIntervalSeconds = value30.As<float>();
		}
		if (info.TryGetProperty(PropertyName.EnableAdobeAnimateProfiler, out var value31))
		{
			EnableAdobeAnimateProfiler = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.EnableDetailedAdobeAnimateProfiler, out var value32))
		{
			EnableDetailedAdobeAnimateProfiler = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.EnableAllocationTelemetry, out var value33))
		{
			EnableAllocationTelemetry = value33.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ProfilerDumpIntervalFrames, out var value34))
		{
			ProfilerDumpIntervalFrames = value34.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ProfilerMaxMetricsPerDump, out var value35))
		{
			ProfilerMaxMetricsPerDump = value35.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ClearAdobeAnimateRuntimeCachesBeforeRun, out var value36))
		{
			ClearAdobeAnimateRuntimeCachesBeforeRun = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.EnableGpuRenderGraph, out var value37))
		{
			EnableGpuRenderGraph = value37.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ForceCpuPoseFallbackForTest, out var value38))
		{
			ForceCpuPoseFallbackForTest = value38.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.FixedAnimationFrame, out var value39))
		{
			FixedAnimationFrame = value39.As<int>();
		}
		if (info.TryGetProperty(PropertyName.FixedAnimationSubframe, out var value40))
		{
			FixedAnimationSubframe = value40.As<double>();
		}
		if (info.TryGetProperty(PropertyName.PrintCrowdFilterDebugData, out var value41))
		{
			PrintCrowdFilterDebugData = value41.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.CrowdFilterDebugSpriteCount, out var value42))
		{
			CrowdFilterDebugSpriteCount = value42.As<int>();
		}
		if (info.TryGetProperty(PropertyName.CrowdFilterDebugMaxFrameSlices, out var value43))
		{
			CrowdFilterDebugMaxFrameSlices = value43.As<int>();
		}
		if (info.TryGetProperty(PropertyName.PrintCrowdFilterDebugAfterFirstRuntimeFrame, out var value44))
		{
			PrintCrowdFilterDebugAfterFirstRuntimeFrame = value44.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.PrintCrowdFilterDebugEveryRuntimeSnapshot, out var value45))
		{
			PrintCrowdFilterDebugEveryRuntimeSnapshot = value45.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.EnableZInterleaveFixture, out var value46))
		{
			EnableZInterleaveFixture = value46.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.PrintCrowdModeReport, out var value47))
		{
			PrintCrowdModeReport = value47.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.InitialAnimeFrameRate, out var value48))
		{
			InitialAnimeFrameRate = value48.As<double>();
		}
		if (info.TryGetProperty(PropertyName.SwitchAnimeFrameRateAfterSeconds, out var value49))
		{
			SwitchAnimeFrameRateAfterSeconds = value49.As<double>();
		}
		if (info.TryGetProperty(PropertyName.SwitchAnimeFrameRateTo, out var value50))
		{
			SwitchAnimeFrameRateTo = value50.As<double>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeVisualMutation, out var value51))
		{
			RuntimeVisualMutation = value51.As<string>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeVisualMutationAfterSeconds, out var value52))
		{
			RuntimeVisualMutationAfterSeconds = value52.As<double>();
		}
		if (info.TryGetProperty(PropertyName.EnableAnimationSwitchStress, out var value53))
		{
			EnableAnimationSwitchStress = value53.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.AnimationSwitchClipA, out var value54))
		{
			AnimationSwitchClipA = value54.As<string>();
		}
		if (info.TryGetProperty(PropertyName.AnimationSwitchClipB, out var value55))
		{
			AnimationSwitchClipB = value55.As<string>();
		}
		if (info.TryGetProperty(PropertyName.AnimationSwitchIntervalSeconds, out var value56))
		{
			AnimationSwitchIntervalSeconds = value56.As<double>();
		}
		if (info.TryGetProperty(PropertyName.AnimationSwitchBlendSeconds, out var value57))
		{
			AnimationSwitchBlendSeconds = value57.As<double>();
		}
		if (info.TryGetProperty(PropertyName.AnimationSwitchStartAfterSeconds, out var value58))
		{
			AnimationSwitchStartAfterSeconds = value58.As<double>();
		}
		if (info.TryGetProperty(PropertyName.EnableBowlingImpactTweenStress, out var value59))
		{
			EnableBowlingImpactTweenStress = value59.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.BowlingImpactEnsureShield, out var value60))
		{
			BowlingImpactEnsureShield = value60.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.BowlingImpactUseProductionEvent, out var value61))
		{
			BowlingImpactUseProductionEvent = value61.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.BowlingImpactProcessedTweenLimit, out var value62))
		{
			BowlingImpactProcessedTweenLimit = value62.As<int>();
		}
		if (info.TryGetProperty(PropertyName.BowlingImpactTargetsPerBurst, out var value63))
		{
			BowlingImpactTargetsPerBurst = value63.As<int>();
		}
		if (info.TryGetProperty(PropertyName.BowlingImpactDamage, out var value64))
		{
			BowlingImpactDamage = value64.As<double>();
		}
		if (info.TryGetProperty(PropertyName.BowlingImpactBurstIntervalSeconds, out var value65))
		{
			BowlingImpactBurstIntervalSeconds = value65.As<double>();
		}
		if (info.TryGetProperty(PropertyName.BowlingImpactActiveWindowSeconds, out var value66))
		{
			BowlingImpactActiveWindowSeconds = value66.As<double>();
		}
		if (info.TryGetProperty(PropertyName.BowlingImpactStartAfterSeconds, out var value67))
		{
			BowlingImpactStartAfterSeconds = value67.As<double>();
		}
		if (info.TryGetProperty(PropertyName.RunComponentAliveSchedulingRegression, out var value68))
		{
			RunComponentAliveSchedulingRegression = value68.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.RunStatePhysicsBatchRateRegression, out var value69))
		{
			RunStatePhysicsBatchRateRegression = value69.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.RunAnimationCadenceRegression, out var value70))
		{
			RunAnimationCadenceRegression = value70.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.EnableExternalVisualStress, out var value71))
		{
			EnableExternalVisualStress = value71.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ExternalVisualIceOnly, out var value72))
		{
			ExternalVisualIceOnly = value72.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ExternalVisualRatio, out var value73))
		{
			ExternalVisualRatio = value73.As<double>();
		}
		if (info.TryGetProperty(PropertyName.EnableCombatStateStress, out var value74))
		{
			EnableCombatStateStress = value74.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.PreserveCombatCharactersInStress, out var value75))
		{
			PreserveCombatCharactersInStress = value75.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.CombatStressIsNight, out var value76))
		{
			CombatStressIsNight = value76.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ShowCharacterHealthInStress, out var value77))
		{
			ShowCharacterHealthInStress = value77.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.EnableSingleEnemyTargetFixture, out var value78))
		{
			EnableSingleEnemyTargetFixture = value78.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.SingleEnemyTargetScenePath, out var value79))
		{
			SingleEnemyTargetScenePath = value79.As<string>();
		}
		if (info.TryGetProperty(PropertyName.SingleEnemyTargetScale, out var value80))
		{
			SingleEnemyTargetScale = value80.As<float>();
		}
		if (info.TryGetProperty(PropertyName.EnableSleepCycleStress, out var value81))
		{
			EnableSleepCycleStress = value81.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.SleepCycleStartAfterSeconds, out var value82))
		{
			SleepCycleStartAfterSeconds = value82.As<double>();
		}
		if (info.TryGetProperty(PropertyName.EnableManyEnemyIdleFixture, out var value83))
		{
			EnableManyEnemyIdleFixture = value83.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ManyEnemyIdleTargetCount, out var value84))
		{
			ManyEnemyIdleTargetCount = value84.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ManyEnemyIdleTargetsOutsideAttackRange, out var value85))
		{
			ManyEnemyIdleTargetsOutsideAttackRange = value85.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.WaitForResourceManagerLoad, out var value86))
		{
			WaitForResourceManagerLoad = value86.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.KeepRunningWhenUnfocused, out var value87))
		{
			KeepRunningWhenUnfocused = value87.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeProfile, out var value88))
		{
			RuntimeProfile = value88.As<string>();
		}
		if (info.TryGetProperty(PropertyName._testRenderBackend, out var value89))
		{
			_testRenderBackend = value89.As<AdobeAnimateRenderBackend>();
		}
		if (info.TryGetProperty(PropertyName._sceneUseCounts, out var value90))
		{
			_sceneUseCounts = value90.As<int[]>();
		}
		if (info.TryGetProperty(PropertyName._printTimer, out var value91))
		{
			_printTimer = value91.As<double>();
		}
		if (info.TryGetProperty(PropertyName._windowDeltaSeconds, out var value92))
		{
			_windowDeltaSeconds = value92.As<double>();
		}
		if (info.TryGetProperty(PropertyName._sampleStartSeconds, out var value93))
		{
			_sampleStartSeconds = value93.As<double>();
		}
		if (info.TryGetProperty(PropertyName._windowFrames, out var value94))
		{
			_windowFrames = value94.As<int>();
		}
		if (info.TryGetProperty(PropertyName._runtimeCrowdFilterDebugPrinted, out var value95))
		{
			_runtimeCrowdFilterDebugPrinted = value95.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._crowdModeReportPrinted, out var value96))
		{
			_crowdModeReportPrinted = value96.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._spawnFailures, out var value97))
		{
			_spawnFailures = value97.As<int>();
		}
		if (info.TryGetProperty(PropertyName._loadedScenePoolCount, out var value98))
		{
			_loadedScenePoolCount = value98.As<int>();
		}
		if (info.TryGetProperty(PropertyName._skippedScenePoolCount, out var value99))
		{
			_skippedScenePoolCount = value99.As<int>();
		}
		if (info.TryGetProperty(PropertyName._excludedScenePoolCount, out var value100))
		{
			_excludedScenePoolCount = value100.As<int>();
		}
		if (info.TryGetProperty(PropertyName._nextSpawnIndex, out var value101))
		{
			_nextSpawnIndex = value101.As<int>();
		}
		if (info.TryGetProperty(PropertyName._nextSpawnProgressPrint, out var value102))
		{
			_nextSpawnProgressPrint = value102.As<int>();
		}
		if (info.TryGetProperty(PropertyName._spawnStartTicks, out var value103))
		{
			_spawnStartTicks = value103.As<long>();
		}
		if (info.TryGetProperty(PropertyName._loadMs, out var value104))
		{
			_loadMs = value104.As<double>();
		}
		if (info.TryGetProperty(PropertyName._spawnNoGcRegionActive, out var value105))
		{
			_spawnNoGcRegionActive = value105.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingSpawnFallbackScene, out var value106))
		{
			_pendingSpawnFallbackScene = value106.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName._spawnPending, out var value107))
		{
			_spawnPending = value107.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._spawnFrameBudgetPassed, out var value108))
		{
			_spawnFrameBudgetPassed = value108.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._autoQuitPrinted, out var value109))
		{
			_autoQuitPrinted = value109.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._measurementStarted, out var value110))
		{
			_measurementStarted = value110.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._measurementFrames, out var value111))
		{
			_measurementFrames = value111.As<int>();
		}
		if (info.TryGetProperty(PropertyName._singleEnemyTarget, out var value112))
		{
			_singleEnemyTarget = value112.As<TowerDefenseCharacter>();
		}
		if (info.TryGetProperty(PropertyName._singleEnemyAttackComponentCount, out var value113))
		{
			_singleEnemyAttackComponentCount = value113.As<int>();
		}
		if (info.TryGetProperty(PropertyName._singleEnemyAttackUtilityComponentCount, out var value114))
		{
			_singleEnemyAttackUtilityComponentCount = value114.As<int>();
		}
		if (info.TryGetProperty(PropertyName._singleEnemyFireComponentCount, out var value115))
		{
			_singleEnemyFireComponentCount = value115.As<int>();
		}
		if (info.TryGetProperty(PropertyName._singleEnemyTargetInitialHitpoints, out var value116))
		{
			_singleEnemyTargetInitialHitpoints = value116.As<double>();
		}
		if (info.TryGetProperty(PropertyName._singleEnemyFixturePrepared, out var value117))
		{
			_singleEnemyFixturePrepared = value117.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._singleEnemyRobotModeAdvanceRequested, out var value118))
		{
			_singleEnemyRobotModeAdvanceRequested = value118.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._singleEnemyRoleUtilityPhaseStarted, out var value119))
		{
			_singleEnemyRoleUtilityPhaseStarted = value119.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._singleEnemyRoleUtilityPhaseCompleted, out var value120))
		{
			_singleEnemyRoleUtilityPhaseCompleted = value120.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._manyEnemyIdleFixturePrepared, out var value121))
		{
			_manyEnemyIdleFixturePrepared = value121.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._nextSingleEnemyRoleUtilityAuditSeconds, out var value122))
		{
			_nextSingleEnemyRoleUtilityAuditSeconds = value122.As<double>();
		}
		if (info.TryGetProperty(PropertyName._sleepCyclePhase, out var value123))
		{
			_sleepCyclePhase = value123.As<int>();
		}
		if (info.TryGetProperty(PropertyName._sleepCyclePhaseDeadlineSeconds, out var value124))
		{
			_sleepCyclePhaseDeadlineSeconds = value124.As<double>();
		}
		if (info.TryGetProperty(PropertyName._sleepCycleCompleted, out var value125))
		{
			_sleepCycleCompleted = value125.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sleepCyclePassed, out var value126))
		{
			_sleepCyclePassed = value126.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sleepCycleEligibleCount, out var value127))
		{
			_sleepCycleEligibleCount = value127.As<int>();
		}
		if (info.TryGetProperty(PropertyName._sleepCycleAppliedCount, out var value128))
		{
			_sleepCycleAppliedCount = value128.As<int>();
		}
		if (info.TryGetProperty(PropertyName._sleepCycleEnteredCount, out var value129))
		{
			_sleepCycleEnteredCount = value129.As<int>();
		}
		if (info.TryGetProperty(PropertyName._sleepCycleWokenCount, out var value130))
		{
			_sleepCycleWokenCount = value130.As<int>();
		}
		if (info.TryGetProperty(PropertyName._sleepCycleNormalizedImmunityCount, out var value131))
		{
			_sleepCycleNormalizedImmunityCount = value131.As<int>();
		}
		if (info.TryGetProperty(PropertyName._sleepCycleImmunityRestored, out var value132))
		{
			_sleepCycleImmunityRestored = value132.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sleepCycleHypnotistEventProvider, out var value133))
		{
			_sleepCycleHypnotistEventProvider = value133.As<TowerDefenseZombieHypnotist>();
		}
		if (info.TryGetProperty(PropertyName._measurementStartSeconds, out var value134))
		{
			_measurementStartSeconds = value134.As<double>();
		}
		if (info.TryGetProperty(PropertyName._measurementEndSeconds, out var value135))
		{
			_measurementEndSeconds = value135.As<double>();
		}
		if (info.TryGetProperty(PropertyName._measurementPreviousFrameTicks, out var value136))
		{
			_measurementPreviousFrameTicks = value136.As<long>();
		}
		if (info.TryGetProperty(PropertyName._screenshotRequested, out var value137))
		{
			_screenshotRequested = value137.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._screenshotSaved, out var value138))
		{
			_screenshotSaved = value138.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._resultPrinted, out var value139))
		{
			_resultPrinted = value139.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._performanceResultPassed, out var value140))
		{
			_performanceResultPassed = value140.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._animeFrameRateSwitchCompleted, out var value141))
		{
			_animeFrameRateSwitchCompleted = value141.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._animeFrameRateSwitchPassed, out var value142))
		{
			_animeFrameRateSwitchPassed = value142.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeVisualMutationCompleted, out var value143))
		{
			_runtimeVisualMutationCompleted = value143.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeVisualMutationPassed, out var value144))
		{
			_runtimeVisualMutationPassed = value144.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._animationSwitchUseSecondClip, out var value145))
		{
			_animationSwitchUseSecondClip = value145.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._nextAnimationSwitchSeconds, out var value146))
		{
			_nextAnimationSwitchSeconds = value146.As<double>();
		}
		if (info.TryGetProperty(PropertyName._animationSwitchWindowEndSeconds, out var value147))
		{
			_animationSwitchWindowEndSeconds = value147.As<double>();
		}
		if (info.TryGetProperty(PropertyName._animationSwitchCount, out var value148))
		{
			_animationSwitchCount = value148.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationSwitchAppliedCount, out var value149))
		{
			_animationSwitchAppliedCount = value149.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationSwitchMissingClipCount, out var value150))
		{
			_animationSwitchMissingClipCount = value150.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationSwitchSynchronousTotalMilliseconds, out var value151))
		{
			_animationSwitchSynchronousTotalMilliseconds = value151.As<double>();
		}
		if (info.TryGetProperty(PropertyName._animationSwitchSynchronousMaxMilliseconds, out var value152))
		{
			_animationSwitchSynchronousMaxMilliseconds = value152.As<double>();
		}
		if (info.TryGetProperty(PropertyName._nextBowlingImpactSeconds, out var value153))
		{
			_nextBowlingImpactSeconds = value153.As<double>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactWindowEndSeconds, out var value154))
		{
			_bowlingImpactWindowEndSeconds = value154.As<double>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactBurstCount, out var value155))
		{
			_bowlingImpactBurstCount = value155.As<int>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactAppliedCount, out var value156))
		{
			_bowlingImpactAppliedCount = value156.As<int>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactMissingShieldCount, out var value157))
		{
			_bowlingImpactMissingShieldCount = value157.As<int>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactMaxProcessedTweens, out var value158))
		{
			_bowlingImpactMaxProcessedTweens = value158.As<int>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactMaxBatchedMotions, out var value159))
		{
			_bowlingImpactMaxBatchedMotions = value159.As<int>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactMaxBatchedShieldVisuals, out var value160))
		{
			_bowlingImpactMaxBatchedShieldVisuals = value160.As<int>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactSynchronousTotalMilliseconds, out var value161))
		{
			_bowlingImpactSynchronousTotalMilliseconds = value161.As<double>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactSynchronousMaxMilliseconds, out var value162))
		{
			_bowlingImpactSynchronousMaxMilliseconds = value162.As<double>();
		}
		if (info.TryGetProperty(PropertyName._bowlingImpactThreadAllocatedBytes, out var value163))
		{
			_bowlingImpactThreadAllocatedBytes = value163.As<long>();
		}
		if (info.TryGetProperty(PropertyName._viewportEntryCaptureFramesRemaining, out var value164))
		{
			_viewportEntryCaptureFramesRemaining = value164.As<int>();
		}
		if (info.TryGetProperty(PropertyName._viewportEntryCaptureSkipCurrentFrame, out var value165))
		{
			_viewportEntryCaptureSkipCurrentFrame = value165.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._viewportEntryCapturePrinted, out var value166))
		{
			_viewportEntryCapturePrinted = value166.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._measurementThreadAllocatedBefore, out var value167))
		{
			_measurementThreadAllocatedBefore = value167.As<long>();
		}
		if (info.TryGetProperty(PropertyName._measurementTotalAllocatedBefore, out var value168))
		{
			_measurementTotalAllocatedBefore = value168.As<long>();
		}
		if (info.TryGetProperty(PropertyName._measurementGen0Before, out var value169))
		{
			_measurementGen0Before = value169.As<int>();
		}
		if (info.TryGetProperty(PropertyName._measurementGen1Before, out var value170))
		{
			_measurementGen1Before = value170.As<int>();
		}
		if (info.TryGetProperty(PropertyName._measurementGen2Before, out var value171))
		{
			_measurementGen2Before = value171.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cadenceSprite, out var value172))
		{
			_cadenceSprite = value172.As<AdobeAnimateSpriteBase>();
		}
		if (info.TryGetProperty(PropertyName._cadencePreviousPhase, out var value173))
		{
			_cadencePreviousPhase = value173.As<double>();
		}
		if (info.TryGetProperty(PropertyName._cadenceSamples, out var value174))
		{
			_cadenceSamples = value174.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cadenceAdvances, out var value175))
		{
			_cadenceAdvances = value175.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cadenceSlot, out var value176))
		{
			_cadenceSlot = value176.As<AdobeAnimateSlot>();
		}
		if (info.TryGetProperty(PropertyName._cadencePreviousSlotVersion, out var value177))
		{
			_cadencePreviousSlotVersion = value177.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cadenceSlotAdvances, out var value178))
		{
			_cadenceSlotAdvances = value178.As<int>();
		}
		if (info.TryGetProperty(PropertyName._zInterleaveMarker, out var value179))
		{
			_zInterleaveMarker = value179.As<Polygon2D>();
		}
		if (info.TryGetProperty(PropertyName._zInterleaveParticles, out var value180))
		{
			_zInterleaveParticles = value180.As<CpuParticles2D>();
		}
		if (info.TryGetProperty(PropertyName._zInterleaveParticleTexture, out var value181))
		{
			_zInterleaveParticleTexture = value181.As<ImageTexture>();
		}
		if (info.TryGetProperty(PropertyName._externalVisualExpectedActive, out var value182))
		{
			_externalVisualExpectedActive = value182.As<int>();
		}
		if (info.TryGetProperty(PropertyName._externalVisualConfigurationFailures, out var value183))
		{
			_externalVisualConfigurationFailures = value183.As<int>();
		}
		if (info.TryGetProperty(PropertyName._externalVisualGraphBuildCountAtMeasurementStart, out var value184))
		{
			_externalVisualGraphBuildCountAtMeasurementStart = value184.As<long>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphInitialBuildCountAtMeasurementStart, out var value185))
		{
			_gpuGraphInitialBuildCountAtMeasurementStart = value185.As<long>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphRebuildCountAtMeasurementStart, out var value186))
		{
			_gpuGraphRebuildCountAtMeasurementStart = value186.As<long>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphOwnerExitInvalidationCountAtMeasurementStart, out var value187))
		{
			_gpuGraphOwnerExitInvalidationCountAtMeasurementStart = value187.As<long>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphManagedSlotInvalidationCountAtMeasurementStart, out var value188))
		{
			_gpuGraphManagedSlotInvalidationCountAtMeasurementStart = value188.As<long>();
		}
		if (info.TryGetProperty(PropertyName._gpuGraphExternalVisualInvalidationCountAtMeasurementStart, out var value189))
		{
			_gpuGraphExternalVisualInvalidationCountAtMeasurementStart = value189.As<long>();
		}
		if (info.TryGetProperty(PropertyName._externalVisualStressTexture, out var value190))
		{
			_externalVisualStressTexture = value190.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._originalMaxFps, out var value191))
		{
			_originalMaxFps = value191.As<int>();
		}
		if (info.TryGetProperty(PropertyName._originalPhysicsTicksPerSecond, out var value192))
		{
			_originalPhysicsTicksPerSecond = value192.As<int>();
		}
		if (info.TryGetProperty(PropertyName._invalidTestRenderBackend, out var value193))
		{
			_invalidTestRenderBackend = value193.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._spawnInstantiateMaxMilliseconds, out var value194))
		{
			_spawnInstantiateMaxMilliseconds = value194.As<double>();
		}
		if (info.TryGetProperty(PropertyName._spawnInstantiateMaxScene, out var value195))
		{
			_spawnInstantiateMaxScene = value195.As<string>();
		}
		if (info.TryGetProperty(PropertyName._spawnAddChildReadyMaxMilliseconds, out var value196))
		{
			_spawnAddChildReadyMaxMilliseconds = value196.As<double>();
		}
		if (info.TryGetProperty(PropertyName._spawnAddChildReadyMaxScene, out var value197))
		{
			_spawnAddChildReadyMaxScene = value197.As<string>();
		}
		if (info.TryGetProperty(PropertyName._adobeFixedAnimationFrame, out var value198))
		{
			_adobeFixedAnimationFrame = value198.As<int>();
		}
		if (info.TryGetProperty(PropertyName._invalidAdobeFixedAnimationFrame, out var value199))
		{
			_invalidAdobeFixedAnimationFrame = value199.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderBackendOption, out var value200))
		{
			_renderBackendOption = value200.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._renderBackendStatus, out var value201))
		{
			_renderBackendStatus = value201.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._renderBackendUiRefreshTimer, out var value202))
		{
			_renderBackendUiRefreshTimer = value202.As<double>();
		}
		if (info.TryGetProperty(PropertyName._combatStressControl, out var value203))
		{
			_combatStressControl = value203.As<CharacterStressControlStub>();
		}
		if (info.TryGetProperty(PropertyName._combatStressMapFeature, out var value204))
		{
			_combatStressMapFeature = value204.As<TowerDefenseBattleFeatureMap>();
		}
		if (info.TryGetProperty(PropertyName._combatStressMapControl, out var value205))
		{
			_combatStressMapControl = value205.As<TowerDefenseMapControl>();
		}
		if (info.TryGetProperty(PropertyName._previousControl, out var value206))
		{
			_previousControl = value206.As<TowerDefenseControlNew>();
		}
		if (info.TryGetProperty(PropertyName._previousGridBegin, out var value207))
		{
			_previousGridBegin = value207.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._previousGridSize, out var value208))
		{
			_previousGridSize = value208.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._previousGridNum, out var value209))
		{
			_previousGridNum = value209.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._previousBackZombie, out var value210))
		{
			_previousBackZombie = value210.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._combatStressConfigured, out var value211))
		{
			_combatStressConfigured = value211.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._resourceManagerLoadGatePending, out var value212))
		{
			_resourceManagerLoadGatePending = value212.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._originalLowProcessorUsageMode, out var value213))
		{
			_originalLowProcessorUsageMode = value213.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._unfocusedRunConfigured, out var value214))
		{
			_unfocusedRunConfigured = value214.As<bool>();
		}
	}
}
