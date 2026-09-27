using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[ScriptPath("res://Test/EffectSpriteOnceNodeBatcherOptimizationRuntimeTest.cs")]
public sealed class EffectSpriteOnceNodeBatcherOptimizationRuntimeTest : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName Run = "Run";

		public static readonly StringName PrepareProductionNodes = "PrepareProductionNodes";

		public static readonly StringName ValidateRuntimeRendererIdentity = "ValidateRuntimeRendererIdentity";

		public static readonly StringName PrepareSteadyStateAfterDeferredUpdates = "PrepareSteadyStateAfterDeferredUpdates";

		public static readonly StringName MeasureRestart = "MeasureRestart";

		public static readonly StringName MeasureUnregisterRegister = "MeasureUnregisterRegister";

		public static readonly StringName MeasureSimpleClipRestart = "MeasureSimpleClipRestart";

		public static readonly StringName RegisterAllWithClip = "RegisterAllWithClip";

		public static readonly StringName MeasureHeterogeneousClipRestart = "MeasureHeterogeneousClipRestart";

		public static readonly StringName IsFullPublicationVisible = "IsFullPublicationVisible";

		public static readonly StringName MeasureSteadyDraw = "MeasureSteadyDraw";

		public static readonly StringName RestartAll = "RestartAll";

		public static readonly StringName UnregisterRegisterAll = "UnregisterRegisterAll";

		public static readonly StringName MeasureAuthoritativeNodeCompletion = "MeasureAuthoritativeNodeCompletion";

		public static readonly StringName PrepareAuthoritativeNodeCompletion = "PrepareAuthoritativeNodeCompletion";

		public static readonly StringName CountAuthoritativeNodeCompletions = "CountAuthoritativeNodeCompletions";

		public static readonly StringName ValidateClipChoiceAndFallbacks = "ValidateClipChoiceAndFallbacks";

		public static readonly StringName ValidateStableDrawOrder = "ValidateStableDrawOrder";

		public static readonly StringName ValidateModAndUnsupportedFallbacks = "ValidateModAndUnsupportedFallbacks";

		public static readonly StringName ValidateEligibilityInvalidation = "ValidateEligibilityInvalidation";

		public static readonly StringName ValidatePlaybackDescriptorInvalidation = "ValidatePlaybackDescriptorInvalidation";

		public static readonly StringName ValidateDynamicDataFailClosed = "ValidateDynamicDataFailClosed";

		public static readonly StringName ValidateCompletionReentry = "ValidateCompletionReentry";

		public static readonly StringName ValidateSelfRecycle = "ValidateSelfRecycle";

		public static readonly StringName RecycleProductionEffects = "RecycleProductionEffects";

		public static readonly StringName ValidateEntryReuseAndBoundedCache = "ValidateEntryReuseAndBoundedCache";

		public static readonly StringName ValidateRealNodeFallback = "ValidateRealNodeFallback";

		public static readonly StringName ValidateBattleLifetimeClear = "ValidateBattleLifetimeClear";

		public static readonly StringName BeginRenderFrameCadence = "BeginRenderFrameCadence";

		public static readonly StringName PaceNextBenchmarkFrame = "PaceNextBenchmarkFrame";

		public static readonly StringName ValidateAndPrintRenderFrameCadence = "ValidateAndPrintRenderFrameCadence";

		public static readonly StringName GetActiveEntryCount = "GetActiveEntryCount";

		public static readonly StringName GetEntryDictionaryCount = "GetEntryDictionaryCount";

		public static readonly StringName GetEntryClip = "GetEntryClip";

		public static readonly StringName GetReverseEffectAt = "GetReverseEffectAt";

		public static readonly StringName GetPlaybackDataStampVersion = "GetPlaybackDataStampVersion";

		public static readonly StringName GetPlaybackDescriptorInt = "GetPlaybackDescriptorInt";

		public static readonly StringName GetPlaybackDescriptorUInt = "GetPlaybackDescriptorUInt";

		public static readonly StringName CheckRegistration = "CheckRegistration";

		public static readonly StringName Check = "Check";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _effects = "_effects";

		public static readonly StringName _sprites = "_sprites";

		public static readonly StringName _productionScenes = "_productionScenes";

		public static readonly StringName _instanceClips = "_instanceClips";

		public static readonly StringName _baselineMode = "_baselineMode";

		public static readonly StringName _manager = "_manager";

		public static readonly StringName _previousControl = "_previousControl";

		public static readonly StringName _testControl = "_testControl";

		public static readonly StringName _previousBackend = "_previousBackend";

		public static readonly StringName _effectMount = "_effectMount";

		public static readonly StringName _fireSplatScene = "_fireSplatScene";

		public static readonly StringName _data = "_data";

		public static readonly StringName _batcher = "_batcher";

		public static readonly StringName _completionDelta = "_completionDelta";

		public static readonly StringName _checks = "_checks";

		public static readonly StringName _failures = "_failures";

		public static readonly StringName _realNodesPassed = "_realNodesPassed";

		public static readonly StringName _clipChoicePassed = "_clipChoicePassed";

		public static readonly StringName _randomChoicePassed = "_randomChoicePassed";

		public static readonly StringName _invalidClipFallbackPassed = "_invalidClipFallbackPassed";

		public static readonly StringName _modFallbackPassed = "_modFallbackPassed";

		public static readonly StringName _unsupportedFallbackPassed = "_unsupportedFallbackPassed";

		public static readonly StringName _eligibilityInvalidationPassed = "_eligibilityInvalidationPassed";

		public static readonly StringName _activeFailClosedPassed = "_activeFailClosedPassed";

		public static readonly StringName _descriptorInvalidationPassed = "_descriptorInvalidationPassed";

		public static readonly StringName _dynamicDataFailClosedPassed = "_dynamicDataFailClosedPassed";

		public static readonly StringName _drawOrderPassed = "_drawOrderPassed";

		public static readonly StringName _transformedMountPassed = "_transformedMountPassed";

		public static readonly StringName _nodePlaybackStatePassed = "_nodePlaybackStatePassed";

		public static readonly StringName _completeReentryPassed = "_completeReentryPassed";

		public static readonly StringName _selfRecyclePassed = "_selfRecyclePassed";

		public static readonly StringName _dictionaryConsistencyPassed = "_dictionaryConsistencyPassed";

		public static readonly StringName _realNodeFallbackPassed = "_realNodeFallbackPassed";

		public static readonly StringName _entryReusePassed = "_entryReusePassed";

		public static readonly StringName _cacheBoundPassed = "_cacheBoundPassed";

		public static readonly StringName _cacheClearPassed = "_cacheClearPassed";

		public static readonly StringName _renderingIdentityPassed = "_renderingIdentityPassed";

		public static readonly StringName _runtimeRendererIdentity = "_runtimeRendererIdentity";

		public static readonly StringName _previousMaxFps = "_previousMaxFps";

		public static readonly StringName _maxFpsOverridden = "_maxFpsOverridden";

		public static readonly StringName _cadenceStartTicks = "_cadenceStartTicks";

		public static readonly StringName _cadenceLastTicks = "_cadenceLastTicks";

		public static readonly StringName _cadenceFrameCount = "_cadenceFrameCount";

		public static readonly StringName _cadenceDeltaSeconds = "_cadenceDeltaSeconds";

		public static readonly StringName _cadenceMinimumDeltaSeconds = "_cadenceMinimumDeltaSeconds";

		public static readonly StringName _cadenceMaximumDeltaSeconds = "_cadenceMaximumDeltaSeconds";

		public static readonly StringName _cadenceNextDeadlineTicks = "_cadenceNextDeadlineTicks";

		public static readonly StringName _cadenceMissedDeadlines = "_cadenceMissedDeadlines";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const int InstanceCount = 1000;

	private const int ProductionMaximumRowCount = 7;

	private const int ProductionMaximumBucketCount = 49;

	private const int CadenceTargetFps = 250;

	private const int WarmupSamples = 240;

	private const int MeasuredSamples = 1200;

	private const int CrossFrameMeasuredSamples = 600;

	private const int ExpectedCadenceFrameCount = 3720;

	private const string FireSplatScenePath = "res://Prefab/Particles/Splats/FireSplats/FireSplats.tscn";

	private const string MixedProductionSceneIdentity = "mixed-production-splat-scenes-7";

	private const string CompositeClip = "Done";

	private static readonly string[] ProductionScenePaths = new string[7] { "res://Prefab/Particles/Splats/FireSplats/FireSplats.tscn", "res://Prefab/Particles/Splats/IceFireSplats/IceFireSplats.tscn", "res://Prefab/Particles/Splats/WhiteFireSplats/WhiteFireSplats.tscn", "res://Prefab/Particles/Splats/MegaFireSplats/MegaFireSplats.tscn", "res://Prefab/Particles/Splats/PeaBombSplats/PeaBombSplats.tscn", "res://Prefab/Particles/Splats/FirePeaBombSplats/FirePeaBombSplats.tscn", "res://Prefab/Particles/Splats/FireZombiePeaSplats/FireZombiePeaSplats.tscn" };

	private static readonly string[] ProductionSceneClips = new string[7] { "Done", "Done", "Done", "Done", "Idle", "Idle", "Done" };

	private readonly TowerDefenseEffectSpriteOnce[] _effects = new TowerDefenseEffectSpriteOnce[1000];

	private readonly AdobeAnimateSprite[] _sprites = new AdobeAnimateSprite[1000];

	private readonly PackedScene[] _productionScenes = new PackedScene[ProductionScenePaths.Length];

	private readonly string[] _instanceClips = new string[1000];

	private bool _baselineMode;

	private TowerDefenseManager _manager;

	private TowerDefenseControlNew _previousControl;

	private TowerDefenseControlNew _testControl;

	private AdobeAnimateRenderBackend _previousBackend;

	private Node2D _effectMount;

	private PackedScene _fireSplatScene;

	private AdobeAnimateData _data;

	private TowerDefenseEffectSpriteOnceBatcher _batcher;

	private double _completionDelta;

	private int _checks;

	private int _failures;

	private bool _realNodesPassed;

	private bool _clipChoicePassed;

	private bool _randomChoicePassed;

	private bool _invalidClipFallbackPassed;

	private bool _modFallbackPassed;

	private bool _unsupportedFallbackPassed;

	private bool _eligibilityInvalidationPassed;

	private bool _activeFailClosedPassed;

	private bool _descriptorInvalidationPassed;

	private bool _dynamicDataFailClosedPassed;

	private bool _drawOrderPassed;

	private bool _transformedMountPassed;

	private bool _nodePlaybackStatePassed;

	private bool _completeReentryPassed;

	private bool _selfRecyclePassed;

	private bool _dictionaryConsistencyPassed;

	private bool _realNodeFallbackPassed;

	private bool _entryReusePassed;

	private bool _cacheBoundPassed;

	private bool _cacheClearPassed;

	private bool _renderingIdentityPassed;

	private string _runtimeRendererIdentity = "unverified_renderer";

	private int _previousMaxFps;

	private bool _maxFpsOverridden;

	private long _cadenceStartTicks;

	private long _cadenceLastTicks;

	private int _cadenceFrameCount;

	private double _cadenceDeltaSeconds;

	private double _cadenceMinimumDeltaSeconds = 1.0 / 0.0;

	private double _cadenceMaximumDeltaSeconds;

	private long _cadenceNextDeadlineTicks;

	private int _cadenceMissedDeadlines;

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		OS.LowProcessorUsageMode = false;
		_baselineMode = string.Equals(System.Environment.GetEnvironmentVariable("PVZHE_EFFECT_ONCE_BATCHER_BASELINE"), "1", StringComparison.Ordinal);
		Callable.From(Run).CallDeferred();
	}

	private async void Run()
	{
		try
		{
			_ = 5;
			try
			{
				PrepareProductionNodes();
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				PrepareSteadyStateAfterDeferredUpdates();
				MeasureRestart();
				MeasureSimpleClipRestart();
				MeasureHeterogeneousClipRestart();
				BeginRenderFrameCadence();
				await MeasureRestartPublication();
				await MeasureHeterogeneousRestartPublication();
				MeasureUnregisterRegister();
				MeasureAuthoritativeNodeCompletion();
				MeasureSteadyDraw();
				await MeasureCrossFrameSteadyDraw();
				ValidateAndPrintRenderFrameCadence();
				ValidateClipChoiceAndFallbacks();
				ValidateStableDrawOrder();
				ValidateModAndUnsupportedFallbacks();
				ValidateEligibilityInvalidation();
				await ValidateActivePlaybackFailClosed();
				ValidatePlaybackDescriptorInvalidation();
				ValidateDynamicDataFailClosed();
				ValidateCompletionReentry();
				ValidateSelfRecycle();
				ValidateEntryReuseAndBoundedCache();
				ValidateRealNodeFallback();
				ValidateBattleLifetimeClear();
			}
			catch (Exception ex)
			{
				_failures++;
				GD.PushError(ex.ToString());
			}
		}
		finally
		{
			await Cleanup();
			bool flag = _failures == 0;
			GD.Print($"EFFECT_SPRITE_ONCE_NODE_BATCHER_RESULT passed={flag} baselineMode={_baselineMode} checks={_checks} failures={_failures} realNodes={_realNodesPassed} clipChoice={_clipChoicePassed} randomChoice={_randomChoicePassed} invalidClipFallback={_invalidClipFallbackPassed} modFallback={_modFallbackPassed} unsupportedFallback={_unsupportedFallbackPassed} eligibilityInvalidation={_eligibilityInvalidationPassed} activeFailClosed={_activeFailClosedPassed} descriptorInvalidation={_descriptorInvalidationPassed} dynamicDataFailClosed={_dynamicDataFailClosedPassed} drawOrder={_drawOrderPassed} transformedMount={_transformedMountPassed} nodePlaybackState={_nodePlaybackStatePassed} completeReentry={_completeReentryPassed} selfRecycle={_selfRecyclePassed} dictionaryConsistency={_dictionaryConsistencyPassed} realNodeFallback={_realNodeFallbackPassed} entryReuse={_entryReusePassed} cacheBound={_cacheBoundPassed} cacheClear={_cacheClearPassed} renderingIdentity={_renderingIdentityPassed}");
			GetTree().Quit((!flag) ? 2 : 0);
		}
	}

	private void PrepareProductionNodes()
	{
		ValidateRuntimeRendererIdentity();
		_manager = TowerDefenseManager.Instance;
		Check(GodotObject.IsInstanceValid(_manager), "TowerDefenseManager autoload must be available.");
		if (!GodotObject.IsInstanceValid(_manager))
		{
			throw new InvalidOperationException("TowerDefenseManager autoload is unavailable.");
		}
		_previousControl = _manager.currentControl;
		_previousBackend = Global.Instance.adobeAnimateRenderBackend;
		Global.Instance.adobeAnimateRenderBackend = AdobeAnimateRenderBackend.GpuCrowd;
		_effectMount = new Node2D
		{
			Name = "EffectSpriteOnceNodeBatcherMount",
			Position = new Vector2(137f, 83f),
			Rotation = 0.17f,
			Scale = new Vector2(1.15f, 0.9f)
		};
		AddChild(_effectMount, forceReadableName: false, InternalMode.Disabled);
		_testControl = new TowerDefenseControlNew
		{
			characterNode = _effectMount
		};
		_manager.currentControl = _testControl;
		for (int i = 0; i < ProductionScenePaths.Length; i++)
		{
			_productionScenes[i] = ResourceLoader.Load<PackedScene>(ProductionScenePaths[i], null, ResourceLoader.CacheMode.Reuse);
			Check(GodotObject.IsInstanceValid(_productionScenes[i]), "Production splat scene must load: " + ProductionScenePaths[i]);
			if (!GodotObject.IsInstanceValid(_productionScenes[i]))
			{
				throw new InvalidOperationException("Production splat scene could not be loaded: " + ProductionScenePaths[i]);
			}
		}
		_fireSplatScene = _productionScenes[0];
		for (int j = 0; j < 1000; j++)
		{
			int num = j % _productionScenes.Length;
			AdobeAnimateSprite adobeAnimateSprite = _productionScenes[num].Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
			string text = ProductionSceneClips[num];
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = _manager.CreateEffectSpriteSceneOnce(adobeAnimateSprite, new Vector2I(j / 7 % 11, 1 + j % 7), text);
			towerDefenseEffectSpriteOnce.Name = $"RealEffect{j}";
			_effectMount.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
			_effects[j] = towerDefenseEffectSpriteOnce;
			_sprites[j] = adobeAnimateSprite;
			_instanceClips[j] = text;
		}
		_batcher = _effectMount.GetNodeOrNull<TowerDefenseEffectSpriteOnceBatcher>("TowerDefenseEffectSpriteOnceBatcher");
		Check(GodotObject.IsInstanceValid(_batcher), "The real Effect Nodes must register with the production batcher.");
		if (!GodotObject.IsInstanceValid(_batcher))
		{
			throw new InvalidOperationException("Production one-shot batcher was not created.");
		}
		_batcher.SetProcess(enable: false);
		_data = _sprites[0].flashAnimeData;
		int num2 = 0;
		double num3 = 1.7976931348623157E+308;
		for (int k = 0; k < ProductionScenePaths.Length; k++)
		{
			AdobeAnimateData flashAnimeData = _sprites[k].flashAnimeData;
			Vector2I clip = flashAnimeData.GetClip(_instanceClips[k]);
			num2 = Math.Max(num2, clip.Y - clip.X);
			num3 = Math.Min(num3, flashAnimeData.frameRate);
		}
		_completionDelta = ((double)num2 + 1.0) / Math.Max(1.0, num3);
		_realNodesPassed = _effectMount.GetChildCount() == 1001 && GetActiveEntryCount() == 1000;
		for (int l = 0; l < 1000; l++)
		{
			_realNodesPassed &= GodotObject.IsInstanceValid(_effects[l]) && GodotObject.IsInstanceValid(_sprites[l]) && _effects[l].GetParent() == _effectMount && _sprites[l].GetParent() == _effects[l] && _effects[l].IsInsideTree() && _sprites[l].IsInsideTree();
		}
		Check(_realNodesPassed, "Exactly 1000 real TowerDefenseEffectSpriteOnce and AdobeAnimateSprite Nodes must be active.");
	}

	private void ValidateRuntimeRendererIdentity()
	{
		string text = RenderingServer.GetCurrentRenderingMethod().ToString();
		string text2 = RenderingServer.GetCurrentRenderingDriverName().ToString();
		_renderingIdentityPassed = string.Equals(text, "mobile", StringComparison.OrdinalIgnoreCase) && text2.IndexOf("vulkan", StringComparison.OrdinalIgnoreCase) >= 0;
		_runtimeRendererIdentity = (text2 + "_" + text).Replace(' ', '_').ToLowerInvariant();
		Check(_renderingIdentityPassed, "The strict effect benchmark must run on Vulkan Mobile; " + $"driver={text2}, method={text}.");
		GD.Print("EFFECT_SPRITE_ONCE_NODE_RENDERER " + $"passed={_renderingIdentityPassed} " + "driver=" + text2 + " method=" + text);
	}

	private void PrepareSteadyStateAfterDeferredUpdates()
	{
		bool flag = true;
		for (int i = 0; i < 1000; i++)
		{
			flag &= !_sprites[i].needMediaReplaceUpdate;
		}
		Check(flag, "Deferred Adobe media updates must finish before measurement.");
		RestartAll();
		_batcher.SetProcess(enable: false);
	}

	private void MeasureRestart()
	{
		OptimizationBatchSampler.PrepareForWarmup();
		for (int i = 0; i < 240; i++)
		{
			long startTicks = OptimizationBatchSampler.BeginSample();
			RestartAll();
			OptimizationBatchSampler.EndWarmupSample(startTicks);
		}
		OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
		optimizationBatchSampler.BeginMeasurement();
		for (int j = 0; j < 1200; j++)
		{
			long startTicks2 = OptimizationBatchSampler.BeginSample();
			RestartAll();
			optimizationBatchSampler.EndSample(startTicks2);
		}
		PrintPerformanceResult("effect-sprite-once-node-restart", "production-effect-refresh", optimizationBatchSampler.Complete(), GetActiveEntryCount() == 1000, GetActiveEntryCount(), GetActiveEntryCount());
	}

	private void MeasureUnregisterRegister()
	{
		OptimizationBatchSampler.PrepareForWarmup();
		for (int i = 0; i < 240; i++)
		{
			long startTicks = OptimizationBatchSampler.BeginSample();
			UnregisterRegisterAll();
			OptimizationBatchSampler.EndWarmupSample(startTicks);
		}
		OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
		optimizationBatchSampler.BeginMeasurement();
		for (int j = 0; j < 1200; j++)
		{
			long startTicks2 = OptimizationBatchSampler.BeginSample();
			UnregisterRegisterAll();
			optimizationBatchSampler.EndSample(startTicks2);
		}
		PrintPerformanceResult("effect-sprite-once-node-unregister-register", "unregister-register", optimizationBatchSampler.Complete(), GetActiveEntryCount() == 1000, GetActiveEntryCount(), GetEntryDictionaryCount());
	}

	private void MeasureSimpleClipRestart()
	{
		RegisterAllWithClip("Done");
		OptimizationBatchSampler.PrepareForWarmup();
		for (int i = 0; i < 240; i++)
		{
			long startTicks = OptimizationBatchSampler.BeginSample();
			RegisterAllWithClip("Done");
			OptimizationBatchSampler.EndWarmupSample(startTicks);
		}
		OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
		optimizationBatchSampler.BeginMeasurement();
		for (int j = 0; j < 1200; j++)
		{
			long startTicks2 = OptimizationBatchSampler.BeginSample();
			RegisterAllWithClip("Done");
			optimizationBatchSampler.EndSample(startTicks2);
		}
		PrintPerformanceResult("effect-sprite-once-node-simple-restart", "simple-register-or-restart", optimizationBatchSampler.Complete(), GetActiveEntryCount() == 1000, GetActiveEntryCount(), GetEntryDictionaryCount());
		RestartAll();
	}

	private void RegisterAllWithClip(string clip)
	{
		for (int i = 0; i < 1000; i++)
		{
			string clipName = (_sprites[i].flashAnimeData.HasClip(clip) ? clip : _instanceClips[i]);
			_sprites[i].SetAnimation(clipName, loop: false);
			if (!_batcher.RegisterOrRestart(_effects[i], _sprites[i], _sprites[i].clip))
			{
				throw new InvalidOperationException($"Register failed at index {i}.");
			}
		}
	}

	private void MeasureHeterogeneousClipRestart()
	{
		RegisterAllWithClip("Done");
		RegisterAllWithClip("Flame");
		OptimizationBatchSampler.PrepareForWarmup();
		for (int i = 0; i < 240; i++)
		{
			string clip = (((i & 1) == 0) ? "Done" : "Flame");
			long startTicks = OptimizationBatchSampler.BeginSample();
			RegisterAllWithClip(clip);
			OptimizationBatchSampler.EndWarmupSample(startTicks);
		}
		OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
		optimizationBatchSampler.BeginMeasurement();
		for (int j = 0; j < 1200; j++)
		{
			string clip2 = (((j & 1) == 0) ? "Done" : "Flame");
			long startTicks2 = OptimizationBatchSampler.BeginSample();
			RegisterAllWithClip(clip2);
			optimizationBatchSampler.EndSample(startTicks2);
		}
		PrintPerformanceResult("effect-sprite-once-node-heterogeneous-restart", "heterogeneous-single-clip-restart", optimizationBatchSampler.Complete(), GetActiveEntryCount() == 1000, GetActiveEntryCount(), GetEntryDictionaryCount());
		RestartAll();
	}

	private async Task MeasureRestartPublication()
	{
		_batcher.SetProcess(enable: false);
		OptimizationBatchSampler.PrepareForWarmup();
		for (int sample = 0; sample < 240; sample++)
		{
			await AwaitBenchmarkRenderFrame();
			_batcher.SetProcess(enable: false);
			long startTicks = OptimizationBatchSampler.BeginSample();
			RestartAll();
			_batcher._Process(0.0);
			OptimizationBatchSampler.EndWarmupSample(startTicks);
		}
		OptimizationBatchSampler sampler = new OptimizationBatchSampler(1200);
		long timedAllocatedBytes = 0L;
		int timedGen0 = 0;
		int timedGen1 = 0;
		int timedGen2 = 0;
		sampler.BeginMeasurement();
		for (int sample = 0; sample < 1200; sample++)
		{
			await AwaitBenchmarkRenderFrame();
			_batcher.SetProcess(enable: false);
			long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
			int gen0Before = GC.CollectionCount(0);
			int gen1Before = GC.CollectionCount(1);
			int gen2Before = GC.CollectionCount(2);
			long startTicks2 = OptimizationBatchSampler.BeginSample();
			RestartAll();
			_batcher._Process(0.0);
			sampler.EndSample(startTicks2);
			AccumulateTimedRuntimeCounters(allocatedBytesForCurrentThread, gen0Before, gen1Before, gen2Before, ref timedAllocatedBytes, ref timedGen0, ref timedGen1, ref timedGen2);
		}
		PrintPerformanceResult("effect-sprite-once-node-restart-publication", "register-or-restart-and-publish", CompleteWithTimedRuntimeCounters(sampler, timedAllocatedBytes, timedGen0, timedGen1, timedGen2), IsFullPublicationVisible(), GetActiveEntryCount(), GetPrivateInt(_batcher, "_lastDrawSubmissionCount"), OptimizationScheduleKind.RenderFrame);
	}

	private async Task MeasureHeterogeneousRestartPublication()
	{
		RegisterAllWithClip("Done");
		_batcher._Process(0.0);
		RegisterAllWithClip("Flame");
		_batcher._Process(0.0);
		OptimizationBatchSampler.PrepareForWarmup();
		for (int sample = 0; sample < 240; sample++)
		{
			string clip = (((sample & 1) == 0) ? "Done" : "Flame");
			await AwaitBenchmarkRenderFrame();
			_batcher.SetProcess(enable: false);
			long startTicks = OptimizationBatchSampler.BeginSample();
			RegisterAllWithClip(clip);
			_batcher._Process(0.0);
			OptimizationBatchSampler.EndWarmupSample(startTicks);
		}
		OptimizationBatchSampler sampler = new OptimizationBatchSampler(1200);
		long timedAllocatedBytes = 0L;
		int timedGen0 = 0;
		int timedGen1 = 0;
		int timedGen2 = 0;
		sampler.BeginMeasurement();
		for (int sample = 0; sample < 1200; sample++)
		{
			string clip = (((sample & 1) == 0) ? "Done" : "Flame");
			await AwaitBenchmarkRenderFrame();
			_batcher.SetProcess(enable: false);
			long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
			int gen0Before = GC.CollectionCount(0);
			int gen1Before = GC.CollectionCount(1);
			int gen2Before = GC.CollectionCount(2);
			long startTicks2 = OptimizationBatchSampler.BeginSample();
			RegisterAllWithClip(clip);
			_batcher._Process(0.0);
			sampler.EndSample(startTicks2);
			AccumulateTimedRuntimeCounters(allocatedBytesForCurrentThread, gen0Before, gen1Before, gen2Before, ref timedAllocatedBytes, ref timedGen0, ref timedGen1, ref timedGen2);
		}
		PrintPerformanceResult("effect-sprite-once-node-heterogeneous-restart-publication", "heterogeneous-single-clip-restart-and-publish", CompleteWithTimedRuntimeCounters(sampler, timedAllocatedBytes, timedGen0, timedGen1, timedGen2), IsFullPublicationVisible(), GetActiveEntryCount(), GetPrivateInt(_batcher, "_lastDrawSubmissionCount"), OptimizationScheduleKind.RenderFrame);
		RestartAll();
	}

	private bool IsFullPublicationVisible()
	{
		AnimateMultiMeshRenderer animateMultiMeshRenderer = GetPrivateObject(_batcher, "_renderer") as AnimateMultiMeshRenderer;
		if (GetActiveEntryCount() == 1000 && GetPrivateInt(_batcher, "_lastDrawSubmissionCount") == 1000 && GodotObject.IsInstanceValid(animateMultiMeshRenderer))
		{
			return animateMultiMeshRenderer.GetVisibleInstanceCountForTest() == 1000;
		}
		return false;
	}

	private void MeasureSteadyDraw()
	{
		OptimizationBatchSampler.PrepareForWarmup();
		for (int i = 0; i < 240; i++)
		{
			long startTicks = OptimizationBatchSampler.BeginSample();
			_batcher._Process(0.0);
			OptimizationBatchSampler.EndWarmupSample(startTicks);
		}
		OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
		optimizationBatchSampler.BeginMeasurement();
		for (int j = 0; j < 1200; j++)
		{
			long startTicks2 = OptimizationBatchSampler.BeginSample();
			_batcher._Process(0.0);
			optimizationBatchSampler.EndSample(startTicks2);
		}
		OptimizationBatchResult result = optimizationBatchSampler.Complete();
		AnimateMultiMeshRenderer animateMultiMeshRenderer = GetPrivateObject(_batcher, "_renderer") as AnimateMultiMeshRenderer;
		bool functionalPassed = GetActiveEntryCount() == 1000 && GetPrivateInt(_batcher, "_lastDrawSubmissionCount") == 1000 && GodotObject.IsInstanceValid(animateMultiMeshRenderer) && animateMultiMeshRenderer.GetVisibleInstanceCountForTest() == 1000;
		PrintPerformanceResult("effect-sprite-once-node-steady-draw", "steady-draw", in result, functionalPassed, GetActiveEntryCount(), GetPrivateInt(_batcher, "_lastDrawSubmissionCount"));
	}

	private async Task MeasureCrossFrameSteadyDraw()
	{
		RestartAll();
		_batcher.SetProcess(enable: false);
		OptimizationBatchSampler.PrepareForWarmup();
		for (int sample = 0; sample < 240; sample++)
		{
			if ((sample & 7) == 0)
			{
				RestartAll();
				_batcher.SetProcess(enable: false);
				_batcher._Process(0.0);
				_batcher.SetProcess(enable: false);
			}
			double delta = await AwaitBenchmarkRenderFrame();
			_batcher.SetProcess(enable: false);
			long startTicks = OptimizationBatchSampler.BeginSample();
			_batcher._Process(delta);
			OptimizationBatchSampler.EndWarmupSample(startTicks);
		}
		OptimizationBatchSampler sampler = new OptimizationBatchSampler(600);
		long timedAllocatedBytes = 0L;
		int timedGen0 = 0;
		int timedGen1 = 0;
		int timedGen2 = 0;
		sampler.BeginMeasurement();
		for (int sample = 0; sample < 600; sample++)
		{
			if ((sample & 7) == 0)
			{
				RestartAll();
				_batcher.SetProcess(enable: false);
				_batcher._Process(0.0);
				_batcher.SetProcess(enable: false);
			}
			double delta2 = await AwaitBenchmarkRenderFrame();
			_batcher.SetProcess(enable: false);
			long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
			int num = GC.CollectionCount(0);
			int num2 = GC.CollectionCount(1);
			int num3 = GC.CollectionCount(2);
			long startTicks2 = OptimizationBatchSampler.BeginSample();
			_batcher._Process(delta2);
			sampler.EndSample(startTicks2);
			timedAllocatedBytes += Math.Max(0L, GC.GetAllocatedBytesForCurrentThread() - allocatedBytesForCurrentThread);
			timedGen0 += GC.CollectionCount(0) - num;
			timedGen1 += GC.CollectionCount(1) - num2;
			timedGen2 += GC.CollectionCount(2) - num3;
		}
		OptimizationBatchResult optimizationBatchResult = sampler.Complete();
		OptimizationBatchResult result = new OptimizationBatchResult(optimizationBatchResult.MeanMilliseconds, optimizationBatchResult.P50Milliseconds, optimizationBatchResult.P95Milliseconds, optimizationBatchResult.P99Milliseconds, optimizationBatchResult.MaximumMilliseconds, timedAllocatedBytes, optimizationBatchResult.SampleCount, optimizationBatchResult.OverBudgetSamples, optimizationBatchResult.MaximumSampleIndex, timedGen0, timedGen1, timedGen2);
		AnimateMultiMeshRenderer animateMultiMeshRenderer = GetPrivateObject(_batcher, "_renderer") as AnimateMultiMeshRenderer;
		float num4 = (float)((double)(_sprites[0].frameIndex - _sprites[0].clipRange.X) + _sprites[0].elapsedTimer);
		int activeEntryCount = GetActiveEntryCount();
		int privateInt = GetPrivateInt(_batcher, "_lastDrawSubmissionCount");
		int num5 = animateMultiMeshRenderer?.GetUnifiedBucketCountForTest() ?? 0;
		Dictionary dictionary = _sprites[0].ExportSpriteSave();
		_nodePlaybackStatePassed = dictionary["frameIndex"].AsInt32() == _sprites[0].frameIndex && Math.Abs(dictionary["elapsedTimer"].AsDouble() - _sprites[0].elapsedTimer) <= 1E-06 && dictionary["clip"].AsString() == _sprites[0].clip && !dictionary["clipOver"].AsBool();
		Check(_nodePlaybackStatePassed, "GPU-batched drawing must advance the authoritative Adobe Node frame and save/export state.");
		bool functionalPassed = num4 > 0f && _nodePlaybackStatePassed && activeEntryCount == 1000 && privateInt == 1000 && GodotObject.IsInstanceValid(animateMultiMeshRenderer) && animateMultiMeshRenderer.GetVisibleInstanceCountForTest() == 1000 && num5 > 0 && num5 <= 49;
		GD.Print($"EFFECT_SPRITE_ONCE_NODE_LAYOUT rows={7} resources={ProductionScenePaths.Length} rendererBuckets={num5} instances={1000} active={activeEntryCount} dispatched={privateInt}");
		PrintPerformanceResult("effect-sprite-once-node-cross-frame-draw", "cross-frame-steady-draw", in result, functionalPassed, activeEntryCount, privateInt, OptimizationScheduleKind.RenderFrame);
		RestartAll();
		_batcher.SetProcess(enable: false);
	}

	private void RestartAll()
	{
		for (int i = 0; i < 1000; i++)
		{
			_effects[i].currentIndex = 0;
			_effects[i].Refresh();
		}
	}

	private void UnregisterRegisterAll()
	{
		for (int i = 0; i < 1000; i++)
		{
			_batcher.Unregister(_effects[i]);
		}
		for (int j = 0; j < 1000; j++)
		{
			if (!_batcher.RegisterOrRestart(_effects[j], _sprites[j], _sprites[j].clip))
			{
				throw new InvalidOperationException($"Register failed at index {j}.");
			}
		}
	}

	private void MeasureAuthoritativeNodeCompletion()
	{
		for (int i = 0; i < 1000; i++)
		{
			_sprites[i].OnAnimeCompleted -= _effects[i].AnimeCompleted;
		}
		try
		{
			OptimizationBatchSampler.PrepareForWarmup();
			for (int j = 0; j < 240; j++)
			{
				PrepareAuthoritativeNodeCompletion(1.0 / 240.0);
				long startTicks = OptimizationBatchSampler.BeginSample();
				_batcher._Process(1.0 / 240.0);
				OptimizationBatchSampler.EndWarmupSample(startTicks);
			}
			OptimizationBatchSampler optimizationBatchSampler = new OptimizationBatchSampler(1200);
			long allocatedBytes = 0L;
			int gen = 0;
			int gen2 = 0;
			int gen3 = 0;
			int num = 0;
			int num2 = 0;
			optimizationBatchSampler.BeginMeasurement();
			for (int k = 0; k < 1200; k++)
			{
				num = PrepareAuthoritativeNodeCompletion(1.0 / 240.0);
				long allocatedBytesForCurrentThread = GC.GetAllocatedBytesForCurrentThread();
				int gen0Before = GC.CollectionCount(0);
				int gen1Before = GC.CollectionCount(1);
				int gen2Before = GC.CollectionCount(2);
				long startTicks2 = OptimizationBatchSampler.BeginSample();
				_batcher._Process(1.0 / 240.0);
				optimizationBatchSampler.EndSample(startTicks2);
				AccumulateTimedRuntimeCounters(allocatedBytesForCurrentThread, gen0Before, gen1Before, gen2Before, ref allocatedBytes, ref gen, ref gen2, ref gen3);
			}
			num2 = CountAuthoritativeNodeCompletions();
			OptimizationBatchResult result = CompleteWithTimedRuntimeCounters(optimizationBatchSampler, allocatedBytes, gen, gen2, gen3);
			bool functionalPassed = num == 1000 && num2 == 1000 && GetActiveEntryCount() == 0 && GetEntryDictionaryCount() == 0;
			PrintPerformanceResult("effect-sprite-once-node-authoritative-completion", "adobe-node-batcher-completion-only", in result, functionalPassed, num, num2);
		}
		finally
		{
			for (int l = 0; l < 1000; l++)
			{
				_sprites[l].OnAnimeCompleted += _effects[l].AnimeCompleted;
			}
			RestartAll();
			_batcher.SetProcess(enable: false);
		}
	}

	private int PrepareAuthoritativeNodeCompletion(double delta)
	{
		int num = 0;
		for (int i = 0; i < 1000; i++)
		{
			TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = _effects[i];
			AdobeAnimateSprite adobeAnimateSprite = _sprites[i];
			towerDefenseEffectSpriteOnce.currentIndex = 0;
			towerDefenseEffectSpriteOnce.Refresh();
			Vector2I clip = adobeAnimateSprite.flashAnimeData.GetClip(adobeAnimateSprite.clip);
			int num2 = clip.Y - clip.X;
			float targetElapsedFrame = Math.Max(0f, (float)num2 - (float)(delta * adobeAnimateSprite.frameRate) + 0.0001f);
			if (adobeAnimateSprite.TryImportEffectOnceGpuPlaybackPositionWithoutEvents(adobeAnimateSprite.clip, targetElapsedFrame) && _batcher.RegisterOrRestart(towerDefenseEffectSpriteOnce, adobeAnimateSprite, adobeAnimateSprite.clip))
			{
				num++;
			}
		}
		_batcher.SetProcess(enable: false);
		return num;
	}

	private int CountAuthoritativeNodeCompletions()
	{
		int num = 0;
		for (int i = 0; i < 1000; i++)
		{
			if (_sprites[i].clipOver && GetEntryObject(_effects[i]) == null)
			{
				num++;
			}
		}
		return num;
	}

	private void ValidateClipChoiceAndFallbacks()
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		for (int i = 0; i < 256; i++)
		{
			_sprites[0].SetAnimation("Done&&Flame&", loop: false);
			bool registered = _batcher.RegisterOrRestart(_effects[0], _sprites[0], _sprites[0].clip);
			CheckRegistration(registered, "random composite clip");
			hashSet.Add(_sprites[0].clip);
		}
		_clipChoicePassed = hashSet.Count == 2 && hashSet.Contains("Done") && hashSet.Contains("Flame");
		_randomChoicePassed = _clipChoicePassed;
		Check(_clipChoicePassed, "RemoveEmptyEntries and random multi-choice semantics changed.");
		_sprites[0].SetAnimation("&Done&&", loop: false);
		bool flag = _batcher.RegisterOrRestart(_effects[0], _sprites[0], _sprites[0].clip);
		string entryClip = GetEntryClip(_effects[0]);
		object entryObject = GetEntryObject(_effects[0]);
		bool flag2 = _batcher.RegisterOrRestart(_effects[0], _sprites[0], "&&");
		_invalidClipFallbackPassed = flag && entryClip == "Done" && !flag2 && entryObject == GetEntryObject(_effects[0]) && GetEntryClip(_effects[0]) == "Done";
		Check(_invalidClipFallbackPassed, "Empty composite parts must fall back to the original invalid clip without unregistering an existing entry.");
	}

	private void ValidateStableDrawOrder()
	{
		UnregisterRegisterAll();
		TowerDefenseEffectSpriteOnce reverseEffectAt = GetReverseEffectAt(0);
		TowerDefenseEffectSpriteOnce reverseEffectAt2 = GetReverseEffectAt(1);
		_batcher.RegisterOrRestart(_effects[123], _sprites[123], _sprites[123].clip);
		_drawOrderPassed = reverseEffectAt == _effects[999] && reverseEffectAt2 == _effects[998] && GetReverseEffectAt(0) == _effects[123] && GetReverseEffectAt(1) == reverseEffectAt;
		AnimateMultiMeshRenderer animateMultiMeshRenderer = GetPrivateObject(_batcher, "_renderer") as AnimateMultiMeshRenderer;
		_transformedMountPassed = GodotObject.IsInstanceValid(animateMultiMeshRenderer) && animateMultiMeshRenderer.TopLevel && animateMultiMeshRenderer.GlobalTransform.IsEqualApprox(Transform2D.Identity) && !_effectMount.Transform.IsEqualApprox(Transform2D.Identity) && !_sprites[123].GlobalTransform.IsEqualApprox(_sprites[123].Transform);
		Check(_drawOrderPassed, "Restart must move the entry to the tail while reverse draw order remains stable.");
		Check(_transformedMountPassed, "The dedicated batch renderer must remain top-level at global identity while real sprite Nodes inherit the transformed mount.");
	}

	private void ValidateModAndUnsupportedFallbacks()
	{
		EffectSpriteOnceModProbe effectSpriteOnceModProbe = new EffectSpriteOnceModProbe
		{
			Name = "EffectSpriteOnceModProbe",
			flashAnimeData = _data,
			clip = "Done",
			onlyDraw = true
		};
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = new TowerDefenseEffectSpriteOnce
		{
			Name = "EffectSpriteOnceModEffect"
		};
		towerDefenseEffectSpriteOnce.InitScene(effectSpriteOnceModProbe, "Done");
		_effectMount.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
		bool flag = GetEntryObject(towerDefenseEffectSpriteOnce) != null;
		_batcher.Unregister(towerDefenseEffectSpriteOnce);
		_modFallbackPassed = !flag && effectSpriteOnceModProbe.GetType() == typeof(EffectSpriteOnceModProbe) && GetEntryObject(towerDefenseEffectSpriteOnce) == null && !GetPrivateBool(towerDefenseEffectSpriteOnce, "_gpuBatchActive") && effectSpriteOnceModProbe.Visible && !effectSpriteOnceModProbe.pause;
		Check(_modFallbackPassed, "A C# Mod AdobeAnimateSprite subclass must stay on its real CPU Node path without a one-frame GPU registration.");
		towerDefenseEffectSpriteOnce.QueueFree();
		UnregisterRegisterAll();
		object entryObject = GetEntryObject(_effects[0]);
		Node node = new Node
		{
			Name = "UnsupportedDynamicChild"
		};
		_sprites[0].AddChild(node, forceReadableName: false, InternalMode.Disabled);
		bool flag2 = _batcher.RegisterOrRestart(_effects[0], _sprites[0], "Done");
		node.Free();
		_sprites[0].playBack = true;
		bool flag3 = _batcher.RegisterOrRestart(_effects[0], _sprites[0], "Done");
		_sprites[0].playBack = false;
		_unsupportedFallbackPassed = !flag2 && !flag3 && entryObject == GetEntryObject(_effects[0]);
		Check(_unsupportedFallbackPassed, "Unsupported sprite features must fail closed without dropping the existing CPU-fallback-compatible Node state.");
	}

	private void ValidateEligibilityInvalidation()
	{
		object entryObject = GetEntryObject(_effects[0]);
		bool flag = false;
		bool flag2 = false;
		if (_data.layerDictionary.Count > 0)
		{
			StringName layerName = null;
			using (IEnumerator<Variant> enumerator = _data.layerDictionary.Keys.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					layerName = enumerator.Current.AsStringName();
				}
			}
			bool fliter = _sprites[0].GetFliter(layerName);
			_sprites[0].SetFliter(layerName, open: false);
			flag = !_batcher.RegisterOrRestart(_effects[0], _sprites[0], "Done");
			_sprites[0].SetFliter(layerName, fliter);
			flag2 = _batcher.RegisterOrRestart(_effects[0], _sprites[0], "Done");
		}
		bool flag3 = false;
		bool flag4 = false;
		if (_sprites[0].mediaReplaceUse.Count > 0)
		{
			bool value = _sprites[0].mediaReplaceUse[0];
			_sprites[0].mediaReplaceUse[0] = true;
			_sprites[0].QueueUpdateMediaReplace();
			flag3 = !_batcher.RegisterOrRestart(_effects[0], _sprites[0], "Done");
			_sprites[0].mediaReplaceUse[0] = value;
			_sprites[0].UpdateMediaReplace();
			flag4 = _batcher.RegisterOrRestart(_effects[0], _sprites[0], "Done");
		}
		_eligibilityInvalidationPassed = (flag & flag2 & flag3 & flag4) && entryObject == GetEntryObject(_effects[0]) && GetPrivateCollectionCount(_batcher, "_eligibilityBySprite") == 1000;
		Check(_eligibilityInvalidationPassed, "Layer/media mutation versions must invalidate exact built-in eligibility snapshots and recache only after restoration.");
	}

	private async Task ValidateActivePlaybackFailClosed()
	{
		bool passed = true;
		bool flag;
		if (_sprites[0].layerVisible.Count > 0)
		{
			bool previous = _sprites[0].layerVisible[0];
			flag = passed;
			passed = flag & await VerifyActiveMutationFallback(() =>
			{
				_sprites[0].layerVisible[0] = false;
			}, () =>
			{
				_sprites[0].layerVisible[0] = previous;
			});
		}
		if (_sprites[0].mediaReplaceUse.Count > 0)
		{
			bool previous2 = _sprites[0].mediaReplaceUse[0];
			flag = passed;
			passed = flag & await VerifyActiveMutationFallback(() =>
			{
				_sprites[0].mediaReplaceUse[0] = true;
			}, () =>
			{
				_sprites[0].mediaReplaceUse[0] = previous2;
			});
		}
		flag = passed;
		passed = flag & await VerifyActiveMutationFallback(() =>
		{
			_sprites[0].Modulate = new Color(0.5f, 1f, 1f);
		}, () =>
		{
			_sprites[0].Modulate = Colors.White;
		});
		flag = passed;
		passed = flag & await VerifyActiveMutationFallback(() =>
		{
			_effects[0].Visible = false;
		}, () =>
		{
			_effects[0].Visible = true;
		});
		flag = passed;
		passed = flag & await VerifyActiveMutationFallback(() =>
		{
			_effects[0].SelfModulate = new Color(1f, 0.5f, 1f);
		}, () =>
		{
			_effects[0].SelfModulate = Colors.White;
		});
		Node unsupportedChild = new Node
		{
			Name = "ActiveUnsupportedChild"
		};
		flag = passed;
		passed = flag & await VerifyActiveMutationFallback(() =>
		{
			_sprites[0].AddChild(unsupportedChild, forceReadableName: false, InternalMode.Disabled);
		}, () =>
		{
			if (GodotObject.IsInstanceValid(unsupportedChild))
			{
				unsupportedChild.Free();
			}
		});
		RestartAll();
		_batcher.SetProcess(enable: false);
		TowerDefenseEffectSpriteOnce effect = _effects[0];
		AdobeAnimateSprite sprite = _sprites[0];
		effect.RemoveChild(sprite);
		_effectMount.AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		bool flag2 = GetEntryObject(effect) == null && sprite.Visible && !sprite.pause && !GetPrivateBool(effect, "_gpuBatchActive");
		_effectMount.RemoveChild(sprite);
		effect.AddChild(sprite, forceReadableName: false, InternalMode.Disabled);
		effect.Refresh();
		_batcher.SetProcess(enable: false);
		passed &= flag2 && GetEntryObject(effect) != null;
		_activeFailClosedPassed = passed;
		Check(_activeFailClosedPassed, "Active GPU effects must detect direct layer/media, sprite/effect visibility/modulate, child, and parent mutations on the next real process frame and resume the real CPU Nodes.");
		RestartAll();
		_batcher.SetProcess(enable: false);
	}

	private async Task<bool> VerifyActiveMutationFallback(Action mutate, Action restore)
	{
		RestartAll();
		_batcher.SetProcess(enable: false);
		TowerDefenseEffectSpriteOnce effect = _effects[0];
		AdobeAnimateSprite sprite = _sprites[0];
		mutate();
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		_batcher.SetProcess(enable: false);
		_batcher._Process(0.0);
		bool num = GetEntryObject(effect) == null && sprite.Visible && !sprite.pause && !GetPrivateBool(effect, "_gpuBatchActive");
		restore();
		effect.Refresh();
		_batcher.SetProcess(enable: false);
		return num && GetEntryObject(effect) != null;
	}

	private void ValidatePlaybackDescriptorInvalidation()
	{
		uint playbackDataStampVersion = GetPlaybackDataStampVersion(_data);
		_data.EmitChanged();
		uint playbackDataStampVersion2 = GetPlaybackDataStampVersion(_data);
		bool flag = _batcher.RegisterOrRestart(_effects[0], _sprites[0], "Done");
		uint playbackDescriptorUInt = GetPlaybackDescriptorUInt(_data, "Done", "DataVersion");
		int cacheVersion = AdobeAnimateGlobalAtlasCache.CacheVersion;
		AdobeAnimateGlobalAtlasCache.Invalidate(_data);
		int cacheVersion2 = AdobeAnimateGlobalAtlasCache.CacheVersion;
		bool flag2 = _batcher.RegisterOrRestart(_effects[0], _sprites[0], "Done");
		int playbackDescriptorInt = GetPlaybackDescriptorInt(_data, "Done", "AtlasCacheVersion");
		int privateInt = GetPrivateInt(_batcher, "_rendererGeneration");
		(GetPrivateObject(_batcher, "_renderer") as Node)?.Free();
		bool flag3 = _batcher.RegisterOrRestart(_effects[0], _sprites[0], "Done");
		int privateInt2 = GetPrivateInt(_batcher, "_rendererGeneration");
		int playbackDescriptorInt2 = GetPlaybackDescriptorInt(_data, "Done", "RendererGeneration");
		_descriptorInvalidationPassed = ((((((playbackDataStampVersion != 0 && playbackDataStampVersion2 != playbackDataStampVersion) & flag) && playbackDescriptorUInt == playbackDataStampVersion2 && cacheVersion2 != cacheVersion) & flag2) && playbackDescriptorInt == AdobeAnimateGlobalAtlasCache.CacheVersion) & flag3) && privateInt2 > privateInt && playbackDescriptorInt2 == privateInt2;
		Check(_descriptorInvalidationPassed, "Playback descriptors must rebuild after Resource.Changed, global atlas invalidation, and renderer replacement. " + $"stamp={playbackDataStampVersion}->{playbackDataStampVersion2}/" + $"{playbackDescriptorUInt} changed={flag} " + $"atlas={cacheVersion}->{cacheVersion2}/" + $"{playbackDescriptorInt} registered={flag2} " + $"renderer={privateInt}->" + $"{privateInt2}/" + $"{playbackDescriptorInt2} " + $"registered={flag3}");
		RestartAll();
	}

	private void ValidateDynamicDataFailClosed()
	{
		int privateCollectionCount = GetPrivateCollectionCount(_batcher, "_playbackDescriptors");
		int privateCollectionCount2 = GetPrivateCollectionCount(_batcher, "_playbackDataStamps");
		int privateCollectionCount3 = GetPrivateCollectionCount(_batcher, "_eligibilityBySprite");
		AdobeAnimateData adobeAnimateData = _data.Duplicate(deep: true) as AdobeAnimateData;
		AdobeAnimateSpriteBase adobeAnimateSpriteBase = new AdobeAnimateSpriteBase
		{
			Name = "DynamicEffectSprite",
			flashAnimeData = adobeAnimateData,
			clip = "Done",
			onlyDraw = true
		};
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = new TowerDefenseEffectSpriteOnce
		{
			Name = "DynamicEffect"
		};
		towerDefenseEffectSpriteOnce.InitScene(adobeAnimateSpriteBase, "Done");
		_effectMount.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
		object entryObject = GetEntryObject(towerDefenseEffectSpriteOnce);
		Dictionary dictionary = adobeAnimateData?.clips?.Duplicate(deep: true);
		adobeAnimateData?.clips?.Clear();
		bool flag = !_batcher.RegisterOrRestart(towerDefenseEffectSpriteOnce, adobeAnimateSpriteBase, "Done");
		if (adobeAnimateData != null && dictionary != null)
		{
			adobeAnimateData.clips = dictionary;
		}
		bool flag2 = !_batcher.RegisterOrRestart(towerDefenseEffectSpriteOnce, adobeAnimateSpriteBase, "Done");
		_dynamicDataFailClosedPassed = ((adobeAnimateData != null && string.IsNullOrEmpty(adobeAnimateData.ResourcePath) && entryObject == null) & flag & flag2) && GetEntryObject(towerDefenseEffectSpriteOnce) == null && !GetPrivateBool(towerDefenseEffectSpriteOnce, "_gpuBatchActive") && adobeAnimateSpriteBase.Visible && !adobeAnimateSpriteBase.pause && GetPrivateCollectionCount(_batcher, "_playbackDescriptors") == privateCollectionCount && GetPrivateCollectionCount(_batcher, "_playbackDataStamps") == privateCollectionCount2 && GetPrivateCollectionCount(_batcher, "_eligibilityBySprite") == privateCollectionCount3;
		Check(_dynamicDataFailClosedPassed, "Runtime/Mod animation data must remain on the real CPU Node path and never pollute trusted descriptor or eligibility caches.");
		_batcher.Unregister(towerDefenseEffectSpriteOnce);
		towerDefenseEffectSpriteOnce.Free();
	}

	private void ValidateCompletionReentry()
	{
		RecycleProductionEffects();
		AdobeAnimateSprite adobeAnimateSprite = _fireSplatScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		TowerDefenseEffectSpriteOnce reentryEffect = _manager.CreateEffectSpriteSceneOnce(adobeAnimateSprite, new Vector2I(9, 8), "Done");
		reentryEffect.Name = "CompletionReentryEffect";
		_effectMount.AddChild(reentryEffect, forceReadableName: false, InternalMode.Disabled);
		adobeAnimateSprite.OnAnimeCompleted -= reentryEffect.AnimeCompleted;
		int completionCount = 0;
		AdobeAnimateSprite.AnimeCompletedEventHandler value = (string completedClip) =>
		{
			completionCount++;
			reentryEffect.Refresh();
		};
		adobeAnimateSprite.OnAnimeCompleted += value;
		reentryEffect.Refresh();
		_batcher.SetProcess(enable: false);
		_batcher._Process(_completionDelta);
		_completeReentryPassed = completionCount == 1 && !reentryEffect.IsQueuedForDeletion() && GetEntryObject(reentryEffect) != null && GetActiveEntryCount() == 1 && !adobeAnimateSprite.clipOver && GetPrivateBool(reentryEffect, "_gpuBatchActive");
		_dictionaryConsistencyPassed = GetEntryDictionaryCount() == 1;
		Check(_completeReentryPassed, "A real Adobe completion callback that Refreshes the same Effect must keep the newly registered Entry generation.");
		Check(_dictionaryConsistencyPassed, "Entry order and effect dictionary counts diverged after same-Effect completion callback re-entry.");
		adobeAnimateSprite.OnAnimeCompleted -= value;
		reentryEffect.Recycle();
		reentryEffect.Free();
		RestartAll();
		_batcher.SetProcess(enable: false);
	}

	private void ValidateSelfRecycle()
	{
		RecycleProductionEffects();
		AdobeAnimateSprite scene = _fireSplatScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = _manager.CreateEffectSpriteSceneOnce(scene, new Vector2I(9, 9), "Done");
		towerDefenseEffectSpriteOnce.Name = "SelfRecycleEffect";
		_effectMount.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
		bool flag = GetEntryObject(towerDefenseEffectSpriteOnce) != null && GetActiveEntryCount() == 1;
		_batcher.SetProcess(enable: false);
		_batcher._Process(_completionDelta);
		_selfRecyclePassed = flag && towerDefenseEffectSpriteOnce.IsQueuedForDeletion() && GetEntryObject(towerDefenseEffectSpriteOnce) == null && GetActiveEntryCount() == 0 && GetEntryDictionaryCount() == 0;
		_dictionaryConsistencyPassed &= GetEntryDictionaryCount() == GetActiveEntryCount();
		Check(_selfRecyclePassed, "A real single-clip Effect must QueueFree through its normal Adobe completion callback without a synthetic next clip.");
		towerDefenseEffectSpriteOnce.Free();
		RestartAll();
		_batcher.SetProcess(enable: false);
	}

	private void RecycleProductionEffects()
	{
		for (int i = 0; i < 1000; i++)
		{
			_effects[i].Recycle();
		}
		_batcher.SetProcess(enable: false);
		Check(GetActiveEntryCount() == 0 && GetEntryDictionaryCount() == 0, "Production Effects must leave the batch through their real Recycle lifecycle before isolated completion checks.");
	}

	private void ValidateEntryReuseAndBoundedCache()
	{
		int privateInt = GetPrivateInt(_batcher, "_createdEntryCount");
		for (int i = 0; i < 8; i++)
		{
			UnregisterRegisterAll();
		}
		int privateInt2 = GetPrivateInt(_batcher, "_createdEntryCount");
		_entryReusePassed = privateInt >= 1000 && privateInt2 == privateInt && GetPrivateInt(_batcher, "_freeEntryCount") == privateInt2 - GetActiveEntryCount();
		Check(_entryReusePassed, "Steady unregister/register must reuse Entry bookkeeping objects without growing the pool.");
		int privateStaticInt = GetPrivateStaticInt(typeof(TowerDefenseEffectSpriteOnceBatcher), "MaxClipChoiceCacheEntries");
		for (int j = 0; j < privateStaticInt + 64; j++)
		{
			_batcher.RegisterOrRestart(_effects[0], _sprites[0], $"MissingA{j}&MissingB{j}");
		}
		int privateCollectionCount = GetPrivateCollectionCount(_batcher, "_clipChoiceCache");
		int privateStaticInt2 = GetPrivateStaticInt(typeof(TowerDefenseEffectSpriteOnceBatcher), "MaxPlaybackDescriptorCacheEntries");
		int privateStaticInt3 = GetPrivateStaticInt(typeof(TowerDefenseEffectSpriteOnceBatcher), "MaxPlaybackDataStampEntries");
		int privateStaticInt4 = GetPrivateStaticInt(typeof(TowerDefenseEffectSpriteOnceBatcher), "MaxEligibilityCacheEntries");
		_cacheBoundPassed = privateStaticInt > 0 && privateCollectionCount <= privateStaticInt && GetPrivateCollectionCount(_batcher, "_playbackDescriptors") <= privateStaticInt2 && GetPrivateCollectionCount(_batcher, "_playbackDataStamps") <= privateStaticInt3 && GetPrivateCollectionCount(_batcher, "_eligibilityBySprite") <= privateStaticInt4;
		Check(_cacheBoundPassed, "Clip, playback descriptor, Resource subscription, and eligibility caches must remain battle-lifetime bounded.");
	}

	private void ValidateRealNodeFallback()
	{
		RecycleProductionEffects();
		int childCount = _effectMount.GetChildCount();
		AdobeAnimateSprite adobeAnimateSprite = _fireSplatScene.Instantiate<AdobeAnimateSprite>(PackedScene.GenEditState.Disabled);
		TowerDefenseEffectSpriteOnce towerDefenseEffectSpriteOnce = TowerDefenseManager.Instance.CreateEffectSpriteSceneOnce(adobeAnimateSprite, new Vector2I(4, 3), "Done");
		_effectMount.AddChild(towerDefenseEffectSpriteOnce, forceReadableName: false, InternalMode.Disabled);
		bool flag = GodotObject.IsInstanceValid(towerDefenseEffectSpriteOnce) && GodotObject.IsInstanceValid(adobeAnimateSprite) && towerDefenseEffectSpriteOnce.GetParent() == _effectMount && adobeAnimateSprite.GetParent() == towerDefenseEffectSpriteOnce && _effectMount.GetChildCount() == childCount + 1 && GetActiveEntryCount() == 1;
		_batcher._Process(_completionDelta);
		bool flag2 = towerDefenseEffectSpriteOnce.IsQueuedForDeletion() && GetActiveEntryCount() == 0;
		_realNodeFallbackPassed = flag & flag2;
		Check(_realNodeFallbackPassed, "Bullet splat fallback must create a real Effect/Adobe Node pair and retire it through the normal one-shot lifecycle.");
	}

	private void ValidateBattleLifetimeClear()
	{
		object playbackDataStampObject = GetPlaybackDataStampObject(_data);
		uint publicUInt = GetPublicUInt(playbackDataStampObject, "Version");
		_effectMount.RemoveChild(_batcher);
		_data.EmitChanged();
		_cacheClearPassed = GetPrivateCollectionCount(_batcher, "_clipChoiceCache") == 0 && GetPrivateCollectionCount(_batcher, "_playbackDescriptors") == 0 && GetPrivateCollectionCount(_batcher, "_playbackDataStamps") == 0 && GetPrivateCollectionCount(_batcher, "_eligibilityBySprite") == 0 && GetActiveEntryCount() == 0 && GetEntryDictionaryCount() == 0 && GetPrivateInt(_batcher, "_freeEntryCount") == 0 && GetPublicUInt(playbackDataStampObject, "Version") == publicUInt;
		Check(_cacheClearPassed, "Battle exit must unsubscribe Resource.Changed and clear clip, descriptor, eligibility, active-entry, dictionary, and free-list ownership.");
		_batcher.Free();
		_batcher = null;
	}

	private void BeginRenderFrameCadence()
	{
		if (!_maxFpsOverridden)
		{
			_previousMaxFps = Engine.MaxFps;
			_maxFpsOverridden = true;
		}
		Engine.MaxFps = 0;
		_cadenceStartTicks = 0L;
		_cadenceLastTicks = 0L;
		_cadenceFrameCount = 0;
		_cadenceDeltaSeconds = 0.0;
		_cadenceMinimumDeltaSeconds = 1.0 / 0.0;
		_cadenceMaximumDeltaSeconds = 0.0;
		_cadenceNextDeadlineTicks = 0L;
		_cadenceMissedDeadlines = 0;
	}

	private async Task<double> AwaitBenchmarkRenderFrame()
	{
		PaceNextBenchmarkFrame();
		if (_cadenceStartTicks == 0L)
		{
			_cadenceStartTicks = Stopwatch.GetTimestamp();
		}
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		_cadenceLastTicks = Stopwatch.GetTimestamp();
		double processDeltaTime = GetProcessDeltaTime();
		_cadenceFrameCount++;
		_cadenceDeltaSeconds += processDeltaTime;
		_cadenceMinimumDeltaSeconds = Math.Min(_cadenceMinimumDeltaSeconds, processDeltaTime);
		_cadenceMaximumDeltaSeconds = Math.Max(_cadenceMaximumDeltaSeconds, processDeltaTime);
		return processDeltaTime;
	}

	private void PaceNextBenchmarkFrame()
	{
		long frequency = Stopwatch.Frequency;
		long num = Math.Max(1L, frequency / 250);
		long timestamp = Stopwatch.GetTimestamp();
		if (_cadenceNextDeadlineTicks == 0L)
		{
			_cadenceNextDeadlineTicks = timestamp;
		}
		else if (timestamp > _cadenceNextDeadlineTicks)
		{
			_cadenceMissedDeadlines++;
			_cadenceNextDeadlineTicks = timestamp;
		}
		while (timestamp < _cadenceNextDeadlineTicks)
		{
			if ((double)(_cadenceNextDeadlineTicks - timestamp) * 1000000.0 / (double)frequency > 3000.0)
			{
				Thread.Yield();
			}
			else
			{
				Thread.SpinWait(256);
			}
			timestamp = Stopwatch.GetTimestamp();
		}
		_cadenceNextDeadlineTicks += num;
	}

	private void ValidateAndPrintRenderFrameCadence()
	{
		double num = (double)(_cadenceLastTicks - _cadenceStartTicks) * 1000.0 / (double)Stopwatch.Frequency;
		double num2 = ((num > 0.0) ? ((double)_cadenceFrameCount * 1000.0 / num) : (1.0 / 0.0));
		double value = ((_cadenceFrameCount > 0) ? (_cadenceDeltaSeconds * 1000.0 / (double)_cadenceFrameCount) : 0.0);
		bool flag = Engine.MaxFps == 0 && _cadenceFrameCount >= 3720 && num2 <= 275.0 && num2 >= 240.0;
		GD.Print($"EFFECT_SPRITE_ONCE_NODE_CADENCE passed={flag} frames={_cadenceFrameCount} wallMs={num:F3} actualFps={num2:F3} meanDeltaMs={value:F6} minDeltaMs={_cadenceMinimumDeltaSeconds * 1000.0:F6} maxDeltaMs={_cadenceMaximumDeltaSeconds * 1000.0:F6} maxFps={Engine.MaxFps} targetFps={250} missedDeadline={_cadenceMissedDeadlines}");
		Check(flag, "The render-frame publication benchmark must meet the real 250 FPS deadline pacer (at least 240 FPS wall throughput) without fixed-fps or per-sample RenderingServer sync.");
	}

	private static void AccumulateTimedRuntimeCounters(long allocatedBefore, int gen0Before, int gen1Before, int gen2Before, ref long allocatedBytes, ref int gen0, ref int gen1, ref int gen2)
	{
		allocatedBytes += Math.Max(0L, GC.GetAllocatedBytesForCurrentThread() - allocatedBefore);
		gen0 += GC.CollectionCount(0) - gen0Before;
		gen1 += GC.CollectionCount(1) - gen1Before;
		gen2 += GC.CollectionCount(2) - gen2Before;
	}

	private static OptimizationBatchResult CompleteWithTimedRuntimeCounters(OptimizationBatchSampler sampler, long allocatedBytes, int gen0, int gen1, int gen2)
	{
		OptimizationBatchResult optimizationBatchResult = sampler.Complete();
		return new OptimizationBatchResult(optimizationBatchResult.MeanMilliseconds, optimizationBatchResult.P50Milliseconds, optimizationBatchResult.P95Milliseconds, optimizationBatchResult.P99Milliseconds, optimizationBatchResult.MaximumMilliseconds, allocatedBytes, optimizationBatchResult.SampleCount, optimizationBatchResult.OverBudgetSamples, optimizationBatchResult.MaximumSampleIndex, gen0, gen1, gen2);
	}

	private void PrintPerformanceResult(string task, string phase, in OptimizationBatchResult result, bool functionalPassed, int actualActiveInstances, int actualDispatchedInstances, OptimizationScheduleKind schedule = OptimizationScheduleKind.BackToBack, int warmupSamples = 240)
	{
		OptimizationResultIdentity identity = new OptimizationResultIdentity(task, OptimizationWorkloadKind.BareComponent, "effect_sprite_once", "mixed-production-splat-scenes-7", "TowerDefenseEffectSpriteOnceBatcher", "production", "exact1000", "none", "active", phase, schedule, _runtimeRendererIdentity, Engine.PhysicsTicksPerSecond, 250);
		bool flag = OptimizationPerformanceGate.IsBareResultPassed(in result, functionalPassed, 1000, warmupSamples, actualActiveInstances, actualDispatchedInstances, in identity) && result.Gen0Collections == 0 && result.Gen1Collections == 0 && result.Gen2Collections == 0;
		bool passed = !_baselineMode & flag;
		GD.Print(OptimizationPerformanceGate.FormatBareResult(in identity, in result, functionalPassed, passed, 1000, warmupSamples, actualActiveInstances, actualDispatchedInstances));
		Check(_baselineMode ? functionalPassed : flag, task + " must process exactly 1000 real Node entries below 0.2 ms P99 with zero steady allocation and GC.");
	}

	private int GetActiveEntryCount()
	{
		if (!GodotObject.IsInstanceValid(_batcher))
		{
			return 0;
		}
		FieldInfo field = _batcher.GetType().GetField("_entryCount", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field != null)
		{
			return (int)field.GetValue(_batcher);
		}
		if (!(_batcher.GetType().GetField("_entries", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(_batcher) is ICollection collection))
		{
			return 0;
		}
		return collection.Count;
	}

	private int GetEntryDictionaryCount()
	{
		return GetPrivateCollectionCount(_batcher, "_entryByEffect");
	}

	private object GetEntryObject(TowerDefenseEffectSpriteOnce effect)
	{
		if (!GodotObject.IsInstanceValid(_batcher))
		{
			return null;
		}
		if (!(_batcher.GetType().GetField("_entryByEffect", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(_batcher) is IDictionary dictionary))
		{
			return null;
		}
		if (!dictionary.Contains(effect))
		{
			return null;
		}
		return dictionary[effect];
	}

	private string GetEntryClip(TowerDefenseEffectSpriteOnce effect)
	{
		object entryObject = GetEntryObject(effect);
		return entryObject?.GetType().GetField("Clip", BindingFlags.Instance | BindingFlags.Public)?.GetValue(entryObject) as string;
	}

	private TowerDefenseEffectSpriteOnce GetReverseEffectAt(int reverseIndex)
	{
		FieldInfo field = _batcher.GetType().GetField("_tail", BindingFlags.Instance | BindingFlags.NonPublic);
		if (field != null)
		{
			object obj = field.GetValue(_batcher);
			int num = 0;
			while (obj != null && num < reverseIndex)
			{
				obj = obj.GetType().GetField("Previous", BindingFlags.Instance | BindingFlags.Public)?.GetValue(obj);
				num++;
			}
			return obj?.GetType().GetField("Effect", BindingFlags.Instance | BindingFlags.Public)?.GetValue(obj) as TowerDefenseEffectSpriteOnce;
		}
		if (!(_batcher.GetType().GetField("_entries", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(_batcher) is IList list))
		{
			return null;
		}
		int num2 = list.Count - reverseIndex - 1;
		if ((uint)num2 >= (uint)list.Count)
		{
			return null;
		}
		object obj2 = list[num2];
		return obj2?.GetType().GetField("Effect", BindingFlags.Instance | BindingFlags.Public)?.GetValue(obj2) as TowerDefenseEffectSpriteOnce;
	}

	private static int GetPrivateInt(object owner, string fieldName)
	{
		object obj = (owner?.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic))?.GetValue(owner);
		if (obj is int)
		{
			return (int)obj;
		}
		return -1;
	}

	private static int GetPrivateStaticInt(Type ownerType, string fieldName)
	{
		object obj = ownerType.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic)?.GetValue(null);
		if (obj is int)
		{
			return (int)obj;
		}
		return -1;
	}

	private static int GetPrivateCollectionCount(object owner, string fieldName)
	{
		if (!((owner?.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic))?.GetValue(owner) is ICollection collection))
		{
			return -1;
		}
		return collection.Count;
	}

	private static object GetPrivateObject(object owner, string fieldName)
	{
		return (owner?.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic))?.GetValue(owner);
	}

	private object GetPlaybackDataStampObject(AdobeAnimateData data)
	{
		if (!(GetPrivateObject(_batcher, "_playbackDataStamps") is IDictionary dictionary) || !dictionary.Contains(data))
		{
			return null;
		}
		return dictionary[data];
	}

	private uint GetPlaybackDataStampVersion(AdobeAnimateData data)
	{
		return GetPublicUInt(GetPlaybackDataStampObject(data), "Version");
	}

	private object GetPlaybackDescriptor(AdobeAnimateData data, string clip)
	{
		if (!(GetPrivateObject(_batcher, "_playbackDescriptors") is IDictionary dictionary))
		{
			return null;
		}
		foreach (DictionaryEntry item in dictionary)
		{
			object key = item.Key;
			FieldInfo? fieldInfo = key?.GetType().GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo fieldInfo2 = key?.GetType().GetField("_clip", BindingFlags.Instance | BindingFlags.NonPublic);
			if (fieldInfo?.GetValue(key) == data && string.Equals(fieldInfo2?.GetValue(key) as string, clip, StringComparison.Ordinal))
			{
				return item.Value;
			}
		}
		return null;
	}

	private int GetPlaybackDescriptorInt(AdobeAnimateData data, string clip, string fieldName)
	{
		object playbackDescriptor = GetPlaybackDescriptor(data, clip);
		object obj = (playbackDescriptor?.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public))?.GetValue(playbackDescriptor);
		if (obj is int)
		{
			return (int)obj;
		}
		return -1;
	}

	private uint GetPlaybackDescriptorUInt(AdobeAnimateData data, string clip, string fieldName)
	{
		return GetPublicUInt(GetPlaybackDescriptor(data, clip), fieldName);
	}

	private static uint GetPublicUInt(object owner, string fieldName)
	{
		object obj = (owner?.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public))?.GetValue(owner);
		if (obj is uint)
		{
			return (uint)obj;
		}
		return 0u;
	}

	private static bool GetPrivateBool(object owner, string fieldName)
	{
		if (owner == null)
		{
			return false;
		}
		object obj = owner.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(owner);
		bool flag = default;
		int num;
		if (obj is bool)
		{
			flag = (bool)obj;
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)((uint)num & (flag ? 1u : 0u)) != 0;
	}

	private void CheckRegistration(bool registered, string operation)
	{
		if (!registered)
		{
			throw new InvalidOperationException("Production effect registration failed during " + operation + ".");
		}
	}

	private void Check(bool condition, string message)
	{
		_checks++;
		if (!condition)
		{
			_failures++;
			GD.PushError(message);
		}
	}

	private async Task Cleanup()
	{
		if (_maxFpsOverridden)
		{
			Engine.MaxFps = _previousMaxFps;
			_maxFpsOverridden = false;
		}
		if (GodotObject.IsInstanceValid(_manager))
		{
			_manager.currentControl = _previousControl;
		}
		if (Global.Instance != null)
		{
			Global.Instance.adobeAnimateRenderBackend = _previousBackend;
		}
		if (GodotObject.IsInstanceValid(_effectMount))
		{
			_effectMount.QueueFree();
		}
		if (GodotObject.IsInstanceValid(_testControl))
		{
			_testControl.Free();
		}
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(41)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.Run, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.PrepareProductionNodes, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateRuntimeRendererIdentity, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.PrepareSteadyStateAfterDeferredUpdates, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.MeasureRestart, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.MeasureUnregisterRegister, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.MeasureSimpleClipRestart, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterAllWithClip, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.MeasureHeterogeneousClipRestart, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.IsFullPublicationVisible, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.MeasureSteadyDraw, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RestartAll, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.UnregisterRegisterAll, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.MeasureAuthoritativeNodeCompletion, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.PrepareAuthoritativeNodeCompletion, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CountAuthoritativeNodeCompletions, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateClipChoiceAndFallbacks, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateStableDrawOrder, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateModAndUnsupportedFallbacks, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateEligibilityInvalidation, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidatePlaybackDescriptorInvalidation, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateDynamicDataFailClosed, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateCompletionReentry, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateSelfRecycle, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.RecycleProductionEffects, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateEntryReuseAndBoundedCache, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateRealNodeFallback, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateBattleLifetimeClear, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.BeginRenderFrameCadence, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.PaceNextBenchmarkFrame, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ValidateAndPrintRenderFrameCadence, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.GetActiveEntryCount, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.GetEntryDictionaryCount, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.GetEntryClip, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "effect", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetReverseEffectAt, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "reverseIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetPlaybackDataStampVersion, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetPlaybackDescriptorInt, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "fieldName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetPlaybackDescriptorUInt, new Godot.Bridge.PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "clip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "fieldName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CheckRegistration, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "registered", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "operation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.Check, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
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
		if (method == MethodName.Run && args.Count == 0)
		{
			Run();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareProductionNodes && args.Count == 0)
		{
			PrepareProductionNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateRuntimeRendererIdentity && args.Count == 0)
		{
			ValidateRuntimeRendererIdentity();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareSteadyStateAfterDeferredUpdates && args.Count == 0)
		{
			PrepareSteadyStateAfterDeferredUpdates();
			ret = default;
			return true;
		}
		if (method == MethodName.MeasureRestart && args.Count == 0)
		{
			MeasureRestart();
			ret = default;
			return true;
		}
		if (method == MethodName.MeasureUnregisterRegister && args.Count == 0)
		{
			MeasureUnregisterRegister();
			ret = default;
			return true;
		}
		if (method == MethodName.MeasureSimpleClipRestart && args.Count == 0)
		{
			MeasureSimpleClipRestart();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterAllWithClip && args.Count == 1)
		{
			RegisterAllWithClip(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MeasureHeterogeneousClipRestart && args.Count == 0)
		{
			MeasureHeterogeneousClipRestart();
			ret = default;
			return true;
		}
		if (method == MethodName.IsFullPublicationVisible && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFullPublicationVisible());
			return true;
		}
		if (method == MethodName.MeasureSteadyDraw && args.Count == 0)
		{
			MeasureSteadyDraw();
			ret = default;
			return true;
		}
		if (method == MethodName.RestartAll && args.Count == 0)
		{
			RestartAll();
			ret = default;
			return true;
		}
		if (method == MethodName.UnregisterRegisterAll && args.Count == 0)
		{
			UnregisterRegisterAll();
			ret = default;
			return true;
		}
		if (method == MethodName.MeasureAuthoritativeNodeCompletion && args.Count == 0)
		{
			MeasureAuthoritativeNodeCompletion();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareAuthoritativeNodeCompletion && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(PrepareAuthoritativeNodeCompletion(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.CountAuthoritativeNodeCompletions && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CountAuthoritativeNodeCompletions());
			return true;
		}
		if (method == MethodName.ValidateClipChoiceAndFallbacks && args.Count == 0)
		{
			ValidateClipChoiceAndFallbacks();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateStableDrawOrder && args.Count == 0)
		{
			ValidateStableDrawOrder();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateModAndUnsupportedFallbacks && args.Count == 0)
		{
			ValidateModAndUnsupportedFallbacks();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateEligibilityInvalidation && args.Count == 0)
		{
			ValidateEligibilityInvalidation();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidatePlaybackDescriptorInvalidation && args.Count == 0)
		{
			ValidatePlaybackDescriptorInvalidation();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateDynamicDataFailClosed && args.Count == 0)
		{
			ValidateDynamicDataFailClosed();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateCompletionReentry && args.Count == 0)
		{
			ValidateCompletionReentry();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateSelfRecycle && args.Count == 0)
		{
			ValidateSelfRecycle();
			ret = default;
			return true;
		}
		if (method == MethodName.RecycleProductionEffects && args.Count == 0)
		{
			RecycleProductionEffects();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateEntryReuseAndBoundedCache && args.Count == 0)
		{
			ValidateEntryReuseAndBoundedCache();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateRealNodeFallback && args.Count == 0)
		{
			ValidateRealNodeFallback();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateBattleLifetimeClear && args.Count == 0)
		{
			ValidateBattleLifetimeClear();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginRenderFrameCadence && args.Count == 0)
		{
			BeginRenderFrameCadence();
			ret = default;
			return true;
		}
		if (method == MethodName.PaceNextBenchmarkFrame && args.Count == 0)
		{
			PaceNextBenchmarkFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.ValidateAndPrintRenderFrameCadence && args.Count == 0)
		{
			ValidateAndPrintRenderFrameCadence();
			ret = default;
			return true;
		}
		if (method == MethodName.GetActiveEntryCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetActiveEntryCount());
			return true;
		}
		if (method == MethodName.GetEntryDictionaryCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetEntryDictionaryCount());
			return true;
		}
		if (method == MethodName.GetEntryClip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetEntryClip(VariantUtils.ConvertTo<TowerDefenseEffectSpriteOnce>(in args[0])));
			return true;
		}
		if (method == MethodName.GetReverseEffectAt && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseEffectSpriteOnce>(GetReverseEffectAt(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPlaybackDataStampVersion && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<uint>(GetPlaybackDataStampVersion(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPlaybackDescriptorInt && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<int>(GetPlaybackDescriptorInt(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.GetPlaybackDescriptorUInt && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<uint>(GetPlaybackDescriptorUInt(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.CheckRegistration && args.Count == 2)
		{
			CheckRegistration(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Check && args.Count == 2)
		{
			Check(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.Run)
		{
			return true;
		}
		if (method == MethodName.PrepareProductionNodes)
		{
			return true;
		}
		if (method == MethodName.ValidateRuntimeRendererIdentity)
		{
			return true;
		}
		if (method == MethodName.PrepareSteadyStateAfterDeferredUpdates)
		{
			return true;
		}
		if (method == MethodName.MeasureRestart)
		{
			return true;
		}
		if (method == MethodName.MeasureUnregisterRegister)
		{
			return true;
		}
		if (method == MethodName.MeasureSimpleClipRestart)
		{
			return true;
		}
		if (method == MethodName.RegisterAllWithClip)
		{
			return true;
		}
		if (method == MethodName.MeasureHeterogeneousClipRestart)
		{
			return true;
		}
		if (method == MethodName.IsFullPublicationVisible)
		{
			return true;
		}
		if (method == MethodName.MeasureSteadyDraw)
		{
			return true;
		}
		if (method == MethodName.RestartAll)
		{
			return true;
		}
		if (method == MethodName.UnregisterRegisterAll)
		{
			return true;
		}
		if (method == MethodName.MeasureAuthoritativeNodeCompletion)
		{
			return true;
		}
		if (method == MethodName.PrepareAuthoritativeNodeCompletion)
		{
			return true;
		}
		if (method == MethodName.CountAuthoritativeNodeCompletions)
		{
			return true;
		}
		if (method == MethodName.ValidateClipChoiceAndFallbacks)
		{
			return true;
		}
		if (method == MethodName.ValidateStableDrawOrder)
		{
			return true;
		}
		if (method == MethodName.ValidateModAndUnsupportedFallbacks)
		{
			return true;
		}
		if (method == MethodName.ValidateEligibilityInvalidation)
		{
			return true;
		}
		if (method == MethodName.ValidatePlaybackDescriptorInvalidation)
		{
			return true;
		}
		if (method == MethodName.ValidateDynamicDataFailClosed)
		{
			return true;
		}
		if (method == MethodName.ValidateCompletionReentry)
		{
			return true;
		}
		if (method == MethodName.ValidateSelfRecycle)
		{
			return true;
		}
		if (method == MethodName.RecycleProductionEffects)
		{
			return true;
		}
		if (method == MethodName.ValidateEntryReuseAndBoundedCache)
		{
			return true;
		}
		if (method == MethodName.ValidateRealNodeFallback)
		{
			return true;
		}
		if (method == MethodName.ValidateBattleLifetimeClear)
		{
			return true;
		}
		if (method == MethodName.BeginRenderFrameCadence)
		{
			return true;
		}
		if (method == MethodName.PaceNextBenchmarkFrame)
		{
			return true;
		}
		if (method == MethodName.ValidateAndPrintRenderFrameCadence)
		{
			return true;
		}
		if (method == MethodName.GetActiveEntryCount)
		{
			return true;
		}
		if (method == MethodName.GetEntryDictionaryCount)
		{
			return true;
		}
		if (method == MethodName.GetEntryClip)
		{
			return true;
		}
		if (method == MethodName.GetReverseEffectAt)
		{
			return true;
		}
		if (method == MethodName.GetPlaybackDataStampVersion)
		{
			return true;
		}
		if (method == MethodName.GetPlaybackDescriptorInt)
		{
			return true;
		}
		if (method == MethodName.GetPlaybackDescriptorUInt)
		{
			return true;
		}
		if (method == MethodName.CheckRegistration)
		{
			return true;
		}
		if (method == MethodName.Check)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._baselineMode)
		{
			_baselineMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._manager)
		{
			_manager = VariantUtils.ConvertTo<TowerDefenseManager>(in value);
			return true;
		}
		if (name == PropertyName._previousControl)
		{
			_previousControl = VariantUtils.ConvertTo<TowerDefenseControlNew>(in value);
			return true;
		}
		if (name == PropertyName._testControl)
		{
			_testControl = VariantUtils.ConvertTo<TowerDefenseControlNew>(in value);
			return true;
		}
		if (name == PropertyName._previousBackend)
		{
			_previousBackend = VariantUtils.ConvertTo<AdobeAnimateRenderBackend>(in value);
			return true;
		}
		if (name == PropertyName._effectMount)
		{
			_effectMount = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._fireSplatScene)
		{
			_fireSplatScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName._data)
		{
			_data = VariantUtils.ConvertTo<AdobeAnimateData>(in value);
			return true;
		}
		if (name == PropertyName._batcher)
		{
			_batcher = VariantUtils.ConvertTo<TowerDefenseEffectSpriteOnceBatcher>(in value);
			return true;
		}
		if (name == PropertyName._completionDelta)
		{
			_completionDelta = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._checks)
		{
			_checks = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._failures)
		{
			_failures = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._realNodesPassed)
		{
			_realNodesPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._clipChoicePassed)
		{
			_clipChoicePassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._randomChoicePassed)
		{
			_randomChoicePassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._invalidClipFallbackPassed)
		{
			_invalidClipFallbackPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._modFallbackPassed)
		{
			_modFallbackPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._unsupportedFallbackPassed)
		{
			_unsupportedFallbackPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._eligibilityInvalidationPassed)
		{
			_eligibilityInvalidationPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._activeFailClosedPassed)
		{
			_activeFailClosedPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._descriptorInvalidationPassed)
		{
			_descriptorInvalidationPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dynamicDataFailClosedPassed)
		{
			_dynamicDataFailClosedPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._drawOrderPassed)
		{
			_drawOrderPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._transformedMountPassed)
		{
			_transformedMountPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._nodePlaybackStatePassed)
		{
			_nodePlaybackStatePassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._completeReentryPassed)
		{
			_completeReentryPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._selfRecyclePassed)
		{
			_selfRecyclePassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dictionaryConsistencyPassed)
		{
			_dictionaryConsistencyPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._realNodeFallbackPassed)
		{
			_realNodeFallbackPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._entryReusePassed)
		{
			_entryReusePassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cacheBoundPassed)
		{
			_cacheBoundPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cacheClearPassed)
		{
			_cacheClearPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderingIdentityPassed)
		{
			_renderingIdentityPassed = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeRendererIdentity)
		{
			_runtimeRendererIdentity = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._previousMaxFps)
		{
			_previousMaxFps = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._maxFpsOverridden)
		{
			_maxFpsOverridden = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._cadenceStartTicks)
		{
			_cadenceStartTicks = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._cadenceLastTicks)
		{
			_cadenceLastTicks = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._cadenceFrameCount)
		{
			_cadenceFrameCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._cadenceDeltaSeconds)
		{
			_cadenceDeltaSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._cadenceMinimumDeltaSeconds)
		{
			_cadenceMinimumDeltaSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._cadenceMaximumDeltaSeconds)
		{
			_cadenceMaximumDeltaSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._cadenceNextDeadlineTicks)
		{
			_cadenceNextDeadlineTicks = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._cadenceMissedDeadlines)
		{
			_cadenceMissedDeadlines = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._effects)
		{
			GodotObject[] effects = _effects;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(effects);
			return true;
		}
		if (name == PropertyName._sprites)
		{
			GodotObject[] effects = _sprites;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(effects);
			return true;
		}
		if (name == PropertyName._productionScenes)
		{
			GodotObject[] effects = _productionScenes;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(effects);
			return true;
		}
		if (name == PropertyName._instanceClips)
		{
			value = VariantUtils.CreateFrom(in _instanceClips);
			return true;
		}
		if (name == PropertyName._baselineMode)
		{
			value = VariantUtils.CreateFrom(in _baselineMode);
			return true;
		}
		if (name == PropertyName._manager)
		{
			value = VariantUtils.CreateFrom(in _manager);
			return true;
		}
		if (name == PropertyName._previousControl)
		{
			value = VariantUtils.CreateFrom(in _previousControl);
			return true;
		}
		if (name == PropertyName._testControl)
		{
			value = VariantUtils.CreateFrom(in _testControl);
			return true;
		}
		if (name == PropertyName._previousBackend)
		{
			value = VariantUtils.CreateFrom(in _previousBackend);
			return true;
		}
		if (name == PropertyName._effectMount)
		{
			value = VariantUtils.CreateFrom(in _effectMount);
			return true;
		}
		if (name == PropertyName._fireSplatScene)
		{
			value = VariantUtils.CreateFrom(in _fireSplatScene);
			return true;
		}
		if (name == PropertyName._data)
		{
			value = VariantUtils.CreateFrom(in _data);
			return true;
		}
		if (name == PropertyName._batcher)
		{
			value = VariantUtils.CreateFrom(in _batcher);
			return true;
		}
		if (name == PropertyName._completionDelta)
		{
			value = VariantUtils.CreateFrom(in _completionDelta);
			return true;
		}
		if (name == PropertyName._checks)
		{
			value = VariantUtils.CreateFrom(in _checks);
			return true;
		}
		if (name == PropertyName._failures)
		{
			value = VariantUtils.CreateFrom(in _failures);
			return true;
		}
		if (name == PropertyName._realNodesPassed)
		{
			value = VariantUtils.CreateFrom(in _realNodesPassed);
			return true;
		}
		if (name == PropertyName._clipChoicePassed)
		{
			value = VariantUtils.CreateFrom(in _clipChoicePassed);
			return true;
		}
		if (name == PropertyName._randomChoicePassed)
		{
			value = VariantUtils.CreateFrom(in _randomChoicePassed);
			return true;
		}
		if (name == PropertyName._invalidClipFallbackPassed)
		{
			value = VariantUtils.CreateFrom(in _invalidClipFallbackPassed);
			return true;
		}
		if (name == PropertyName._modFallbackPassed)
		{
			value = VariantUtils.CreateFrom(in _modFallbackPassed);
			return true;
		}
		if (name == PropertyName._unsupportedFallbackPassed)
		{
			value = VariantUtils.CreateFrom(in _unsupportedFallbackPassed);
			return true;
		}
		if (name == PropertyName._eligibilityInvalidationPassed)
		{
			value = VariantUtils.CreateFrom(in _eligibilityInvalidationPassed);
			return true;
		}
		if (name == PropertyName._activeFailClosedPassed)
		{
			value = VariantUtils.CreateFrom(in _activeFailClosedPassed);
			return true;
		}
		if (name == PropertyName._descriptorInvalidationPassed)
		{
			value = VariantUtils.CreateFrom(in _descriptorInvalidationPassed);
			return true;
		}
		if (name == PropertyName._dynamicDataFailClosedPassed)
		{
			value = VariantUtils.CreateFrom(in _dynamicDataFailClosedPassed);
			return true;
		}
		if (name == PropertyName._drawOrderPassed)
		{
			value = VariantUtils.CreateFrom(in _drawOrderPassed);
			return true;
		}
		if (name == PropertyName._transformedMountPassed)
		{
			value = VariantUtils.CreateFrom(in _transformedMountPassed);
			return true;
		}
		if (name == PropertyName._nodePlaybackStatePassed)
		{
			value = VariantUtils.CreateFrom(in _nodePlaybackStatePassed);
			return true;
		}
		if (name == PropertyName._completeReentryPassed)
		{
			value = VariantUtils.CreateFrom(in _completeReentryPassed);
			return true;
		}
		if (name == PropertyName._selfRecyclePassed)
		{
			value = VariantUtils.CreateFrom(in _selfRecyclePassed);
			return true;
		}
		if (name == PropertyName._dictionaryConsistencyPassed)
		{
			value = VariantUtils.CreateFrom(in _dictionaryConsistencyPassed);
			return true;
		}
		if (name == PropertyName._realNodeFallbackPassed)
		{
			value = VariantUtils.CreateFrom(in _realNodeFallbackPassed);
			return true;
		}
		if (name == PropertyName._entryReusePassed)
		{
			value = VariantUtils.CreateFrom(in _entryReusePassed);
			return true;
		}
		if (name == PropertyName._cacheBoundPassed)
		{
			value = VariantUtils.CreateFrom(in _cacheBoundPassed);
			return true;
		}
		if (name == PropertyName._cacheClearPassed)
		{
			value = VariantUtils.CreateFrom(in _cacheClearPassed);
			return true;
		}
		if (name == PropertyName._renderingIdentityPassed)
		{
			value = VariantUtils.CreateFrom(in _renderingIdentityPassed);
			return true;
		}
		if (name == PropertyName._runtimeRendererIdentity)
		{
			value = VariantUtils.CreateFrom(in _runtimeRendererIdentity);
			return true;
		}
		if (name == PropertyName._previousMaxFps)
		{
			value = VariantUtils.CreateFrom(in _previousMaxFps);
			return true;
		}
		if (name == PropertyName._maxFpsOverridden)
		{
			value = VariantUtils.CreateFrom(in _maxFpsOverridden);
			return true;
		}
		if (name == PropertyName._cadenceStartTicks)
		{
			value = VariantUtils.CreateFrom(in _cadenceStartTicks);
			return true;
		}
		if (name == PropertyName._cadenceLastTicks)
		{
			value = VariantUtils.CreateFrom(in _cadenceLastTicks);
			return true;
		}
		if (name == PropertyName._cadenceFrameCount)
		{
			value = VariantUtils.CreateFrom(in _cadenceFrameCount);
			return true;
		}
		if (name == PropertyName._cadenceDeltaSeconds)
		{
			value = VariantUtils.CreateFrom(in _cadenceDeltaSeconds);
			return true;
		}
		if (name == PropertyName._cadenceMinimumDeltaSeconds)
		{
			value = VariantUtils.CreateFrom(in _cadenceMinimumDeltaSeconds);
			return true;
		}
		if (name == PropertyName._cadenceMaximumDeltaSeconds)
		{
			value = VariantUtils.CreateFrom(in _cadenceMaximumDeltaSeconds);
			return true;
		}
		if (name == PropertyName._cadenceNextDeadlineTicks)
		{
			value = VariantUtils.CreateFrom(in _cadenceNextDeadlineTicks);
			return true;
		}
		if (name == PropertyName._cadenceMissedDeadlines)
		{
			value = VariantUtils.CreateFrom(in _cadenceMissedDeadlines);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Array, PropertyName._effects, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Array, PropertyName._sprites, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Array, PropertyName._productionScenes, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.PackedStringArray, PropertyName._instanceClips, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._baselineMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._manager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._previousControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._testControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._previousBackend, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._effectMount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._fireSplatScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._data, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._batcher, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Float, PropertyName._completionDelta, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._checks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._failures, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._realNodesPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._clipChoicePassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._randomChoicePassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._invalidClipFallbackPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._modFallbackPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._unsupportedFallbackPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._eligibilityInvalidationPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._activeFailClosedPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._descriptorInvalidationPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._dynamicDataFailClosedPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._drawOrderPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._transformedMountPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._nodePlaybackStatePassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._completeReentryPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._selfRecyclePassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._dictionaryConsistencyPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._realNodeFallbackPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._entryReusePassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._cacheBoundPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._cacheClearPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._renderingIdentityPassed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName._runtimeRendererIdentity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._previousMaxFps, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._maxFpsOverridden, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._cadenceStartTicks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._cadenceLastTicks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._cadenceFrameCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Float, PropertyName._cadenceDeltaSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Float, PropertyName._cadenceMinimumDeltaSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Float, PropertyName._cadenceMaximumDeltaSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._cadenceNextDeadlineTicks, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._cadenceMissedDeadlines, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._baselineMode, Variant.From(in _baselineMode));
		info.AddProperty(PropertyName._manager, Variant.From(in _manager));
		info.AddProperty(PropertyName._previousControl, Variant.From(in _previousControl));
		info.AddProperty(PropertyName._testControl, Variant.From(in _testControl));
		info.AddProperty(PropertyName._previousBackend, Variant.From(in _previousBackend));
		info.AddProperty(PropertyName._effectMount, Variant.From(in _effectMount));
		info.AddProperty(PropertyName._fireSplatScene, Variant.From(in _fireSplatScene));
		info.AddProperty(PropertyName._data, Variant.From(in _data));
		info.AddProperty(PropertyName._batcher, Variant.From(in _batcher));
		info.AddProperty(PropertyName._completionDelta, Variant.From(in _completionDelta));
		info.AddProperty(PropertyName._checks, Variant.From(in _checks));
		info.AddProperty(PropertyName._failures, Variant.From(in _failures));
		info.AddProperty(PropertyName._realNodesPassed, Variant.From(in _realNodesPassed));
		info.AddProperty(PropertyName._clipChoicePassed, Variant.From(in _clipChoicePassed));
		info.AddProperty(PropertyName._randomChoicePassed, Variant.From(in _randomChoicePassed));
		info.AddProperty(PropertyName._invalidClipFallbackPassed, Variant.From(in _invalidClipFallbackPassed));
		info.AddProperty(PropertyName._modFallbackPassed, Variant.From(in _modFallbackPassed));
		info.AddProperty(PropertyName._unsupportedFallbackPassed, Variant.From(in _unsupportedFallbackPassed));
		info.AddProperty(PropertyName._eligibilityInvalidationPassed, Variant.From(in _eligibilityInvalidationPassed));
		info.AddProperty(PropertyName._activeFailClosedPassed, Variant.From(in _activeFailClosedPassed));
		info.AddProperty(PropertyName._descriptorInvalidationPassed, Variant.From(in _descriptorInvalidationPassed));
		info.AddProperty(PropertyName._dynamicDataFailClosedPassed, Variant.From(in _dynamicDataFailClosedPassed));
		info.AddProperty(PropertyName._drawOrderPassed, Variant.From(in _drawOrderPassed));
		info.AddProperty(PropertyName._transformedMountPassed, Variant.From(in _transformedMountPassed));
		info.AddProperty(PropertyName._nodePlaybackStatePassed, Variant.From(in _nodePlaybackStatePassed));
		info.AddProperty(PropertyName._completeReentryPassed, Variant.From(in _completeReentryPassed));
		info.AddProperty(PropertyName._selfRecyclePassed, Variant.From(in _selfRecyclePassed));
		info.AddProperty(PropertyName._dictionaryConsistencyPassed, Variant.From(in _dictionaryConsistencyPassed));
		info.AddProperty(PropertyName._realNodeFallbackPassed, Variant.From(in _realNodeFallbackPassed));
		info.AddProperty(PropertyName._entryReusePassed, Variant.From(in _entryReusePassed));
		info.AddProperty(PropertyName._cacheBoundPassed, Variant.From(in _cacheBoundPassed));
		info.AddProperty(PropertyName._cacheClearPassed, Variant.From(in _cacheClearPassed));
		info.AddProperty(PropertyName._renderingIdentityPassed, Variant.From(in _renderingIdentityPassed));
		info.AddProperty(PropertyName._runtimeRendererIdentity, Variant.From(in _runtimeRendererIdentity));
		info.AddProperty(PropertyName._previousMaxFps, Variant.From(in _previousMaxFps));
		info.AddProperty(PropertyName._maxFpsOverridden, Variant.From(in _maxFpsOverridden));
		info.AddProperty(PropertyName._cadenceStartTicks, Variant.From(in _cadenceStartTicks));
		info.AddProperty(PropertyName._cadenceLastTicks, Variant.From(in _cadenceLastTicks));
		info.AddProperty(PropertyName._cadenceFrameCount, Variant.From(in _cadenceFrameCount));
		info.AddProperty(PropertyName._cadenceDeltaSeconds, Variant.From(in _cadenceDeltaSeconds));
		info.AddProperty(PropertyName._cadenceMinimumDeltaSeconds, Variant.From(in _cadenceMinimumDeltaSeconds));
		info.AddProperty(PropertyName._cadenceMaximumDeltaSeconds, Variant.From(in _cadenceMaximumDeltaSeconds));
		info.AddProperty(PropertyName._cadenceNextDeadlineTicks, Variant.From(in _cadenceNextDeadlineTicks));
		info.AddProperty(PropertyName._cadenceMissedDeadlines, Variant.From(in _cadenceMissedDeadlines));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._baselineMode, out var value))
		{
			_baselineMode = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._manager, out var value2))
		{
			_manager = value2.As<TowerDefenseManager>();
		}
		if (info.TryGetProperty(PropertyName._previousControl, out var value3))
		{
			_previousControl = value3.As<TowerDefenseControlNew>();
		}
		if (info.TryGetProperty(PropertyName._testControl, out var value4))
		{
			_testControl = value4.As<TowerDefenseControlNew>();
		}
		if (info.TryGetProperty(PropertyName._previousBackend, out var value5))
		{
			_previousBackend = value5.As<AdobeAnimateRenderBackend>();
		}
		if (info.TryGetProperty(PropertyName._effectMount, out var value6))
		{
			_effectMount = value6.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._fireSplatScene, out var value7))
		{
			_fireSplatScene = value7.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName._data, out var value8))
		{
			_data = value8.As<AdobeAnimateData>();
		}
		if (info.TryGetProperty(PropertyName._batcher, out var value9))
		{
			_batcher = value9.As<TowerDefenseEffectSpriteOnceBatcher>();
		}
		if (info.TryGetProperty(PropertyName._completionDelta, out var value10))
		{
			_completionDelta = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName._checks, out var value11))
		{
			_checks = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName._failures, out var value12))
		{
			_failures = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName._realNodesPassed, out var value13))
		{
			_realNodesPassed = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._clipChoicePassed, out var value14))
		{
			_clipChoicePassed = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._randomChoicePassed, out var value15))
		{
			_randomChoicePassed = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._invalidClipFallbackPassed, out var value16))
		{
			_invalidClipFallbackPassed = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._modFallbackPassed, out var value17))
		{
			_modFallbackPassed = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._unsupportedFallbackPassed, out var value18))
		{
			_unsupportedFallbackPassed = value18.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._eligibilityInvalidationPassed, out var value19))
		{
			_eligibilityInvalidationPassed = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._activeFailClosedPassed, out var value20))
		{
			_activeFailClosedPassed = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._descriptorInvalidationPassed, out var value21))
		{
			_descriptorInvalidationPassed = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dynamicDataFailClosedPassed, out var value22))
		{
			_dynamicDataFailClosedPassed = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._drawOrderPassed, out var value23))
		{
			_drawOrderPassed = value23.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._transformedMountPassed, out var value24))
		{
			_transformedMountPassed = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._nodePlaybackStatePassed, out var value25))
		{
			_nodePlaybackStatePassed = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._completeReentryPassed, out var value26))
		{
			_completeReentryPassed = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._selfRecyclePassed, out var value27))
		{
			_selfRecyclePassed = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dictionaryConsistencyPassed, out var value28))
		{
			_dictionaryConsistencyPassed = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._realNodeFallbackPassed, out var value29))
		{
			_realNodeFallbackPassed = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._entryReusePassed, out var value30))
		{
			_entryReusePassed = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cacheBoundPassed, out var value31))
		{
			_cacheBoundPassed = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cacheClearPassed, out var value32))
		{
			_cacheClearPassed = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderingIdentityPassed, out var value33))
		{
			_renderingIdentityPassed = value33.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeRendererIdentity, out var value34))
		{
			_runtimeRendererIdentity = value34.As<string>();
		}
		if (info.TryGetProperty(PropertyName._previousMaxFps, out var value35))
		{
			_previousMaxFps = value35.As<int>();
		}
		if (info.TryGetProperty(PropertyName._maxFpsOverridden, out var value36))
		{
			_maxFpsOverridden = value36.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._cadenceStartTicks, out var value37))
		{
			_cadenceStartTicks = value37.As<long>();
		}
		if (info.TryGetProperty(PropertyName._cadenceLastTicks, out var value38))
		{
			_cadenceLastTicks = value38.As<long>();
		}
		if (info.TryGetProperty(PropertyName._cadenceFrameCount, out var value39))
		{
			_cadenceFrameCount = value39.As<int>();
		}
		if (info.TryGetProperty(PropertyName._cadenceDeltaSeconds, out var value40))
		{
			_cadenceDeltaSeconds = value40.As<double>();
		}
		if (info.TryGetProperty(PropertyName._cadenceMinimumDeltaSeconds, out var value41))
		{
			_cadenceMinimumDeltaSeconds = value41.As<double>();
		}
		if (info.TryGetProperty(PropertyName._cadenceMaximumDeltaSeconds, out var value42))
		{
			_cadenceMaximumDeltaSeconds = value42.As<double>();
		}
		if (info.TryGetProperty(PropertyName._cadenceNextDeadlineTicks, out var value43))
		{
			_cadenceNextDeadlineTicks = value43.As<long>();
		}
		if (info.TryGetProperty(PropertyName._cadenceMissedDeadlines, out var value44))
		{
			_cadenceMissedDeadlines = value44.As<int>();
		}
	}
}
