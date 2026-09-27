using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[ScriptPath("res://addons/AdobeAnimateEditor/Runtime/AdobeAnimateRuntimeManager.cs")]
public sealed class AdobeAnimateRuntimeManager : Node
{
	private readonly struct RenderRootOrderEntry(AdobeAnimateSprite sprite, int[] treeOrderPath, ulong stableOrder)
	{
		public AdobeAnimateSprite Sprite { get; } = sprite;

		public int[] TreeOrderPath { get; } = treeOrderPath;

		public ulong StableOrder { get; } = stableOrder;
	}

	private sealed class RenderRootOrderEntryComparer : IComparer<RenderRootOrderEntry>
	{
		public static readonly RenderRootOrderEntryComparer Instance = new RenderRootOrderEntryComparer();

		public int Compare(RenderRootOrderEntry left, RenderRootOrderEntry right)
		{
			return AdobeAnimateRenderManager.CompareTreeOrderForRender(left.TreeOrderPath, left.StableOrder, right.TreeOrderPath, right.StableOrder);
		}
	}

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName InvalidateRenderRoots = "InvalidateRenderRoots";

		public static readonly StringName NotifyRenderRootVisibilityChanged = "NotifyRenderRootVisibilityChanged";

		public static readonly StringName NotifyGpuRenderCachesCleared = "NotifyGpuRenderCachesCleared";

		public static readonly StringName NotifyViewportTransformChanged = "NotifyViewportTransformChanged";

		public static readonly StringName RequestRenderRootRepublish = "RequestRenderRootRepublish";

		public static readonly StringName RecoverFromFailedRenderTransaction = "RecoverFromFailedRenderTransaction";

		public static readonly StringName NotifyRenderAncestorTransformChanged = "NotifyRenderAncestorTransformChanged";

		public static readonly StringName NotifyRenderSubmission = "NotifyRenderSubmission";

		public static readonly StringName NotifyDisplayRenderSubmission = "NotifyDisplayRenderSubmission";

		public static readonly StringName NotifyPauseDispatchInvalidated = "NotifyPauseDispatchInvalidated";

		public static readonly StringName SetRenderStaticArrayAuditRegistration = "SetRenderStaticArrayAuditRegistration";

		public static readonly StringName UpdateRenderStaticArrayAuditRegistration = "UpdateRenderStaticArrayAuditRegistration";

		public static readonly StringName Register = "Register";

		public static readonly StringName Unregister = "Unregister";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName HandleAdobeAnimateRenderBackendChanged = "HandleAdobeAnimateRenderBackendChanged";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName ProcessRenderStaticArrayAudits = "ProcessRenderStaticArrayAudits";

		public static readonly StringName AuditRenderStaticArraySprite = "AuditRenderStaticArraySprite";

		public static readonly StringName ResolveRenderFrameRate = "ResolveRenderFrameRate";

		public static readonly StringName ResolveBattleCharacterCount = "ResolveBattleCharacterCount";

		public static readonly StringName ResolveAdaptiveVisualTicksPerSecond = "ResolveAdaptiveVisualTicksPerSecond";

		public static readonly StringName AdvanceVisualCadence = "AdvanceVisualCadence";

		public static readonly StringName RefreshProcessEligibility = "RefreshProcessEligibility";

		public static readonly StringName HasUsableInstance = "HasUsableInstance";

		public static readonly StringName TryMount = "TryMount";

		public static readonly StringName FindExistingManager = "FindExistingManager";

		public static readonly StringName ProcessDisplayVisualCandidates = "ProcessDisplayVisualCandidates";

		public static readonly StringName ProcessActive = "ProcessActive";

		public static readonly StringName MarkRuntimeRenderSubmissionsConsumed = "MarkRuntimeRenderSubmissionsConsumed";

		public static readonly StringName InvalidateRuntimeCrowdCullingIfCachedOffscreen = "InvalidateRuntimeCrowdCullingIfCachedOffscreen";

		public static readonly StringName RenderActive = "RenderActive";

		public static readonly StringName RefreshRenderRootsIfNeeded = "RefreshRenderRootsIfNeeded";

		public static readonly StringName RefreshPauseDispatchState = "RefreshPauseDispatchState";

		public static readonly StringName ResolvePauseDispatch = "ResolvePauseDispatch";

		public static readonly StringName ShouldDispatchInCurrentPauseState = "ShouldDispatchInCurrentPauseState";

		public static readonly StringName ResolveInheritedProcessMode = "ResolveInheritedProcessMode";

		public static readonly StringName ResolveEffectiveProcessMode = "ResolveEffectiveProcessMode";

		public static readonly StringName FlushPending = "FlushPending";

		public static readonly StringName AddActive = "AddActive";

		public static readonly StringName RemoveActiveAt = "RemoveActiveAt";

		public static readonly StringName SetAnimationClockForBareTest = "SetAnimationClockForBareTest";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName RegisteredSpriteCount = "RegisteredSpriteCount";

		public static readonly StringName CurrentVisualTicksPerSecond = "CurrentVisualTicksPerSecond";

		public static readonly StringName CurrentBattleCharacterCount = "CurrentBattleCharacterCount";

		public static readonly StringName _runtimeTree = "_runtimeTree";

		public static readonly StringName _displayTickVersion = "_displayTickVersion";

		public static readonly StringName _dispatchTreePaused = "_dispatchTreePaused";

		public static readonly StringName _dispatchTreePausedInitialized = "_dispatchTreePausedInitialized";

		public static readonly StringName _pauseDispatchValidationCursor = "_pauseDispatchValidationCursor";

		public static readonly StringName _renderRootsDirty = "_renderRootsDirty";

		public static readonly StringName _staticCrowdRefreshAccumulator = "_staticCrowdRefreshAccumulator";

		public static readonly StringName _animationClockSeconds = "_animationClockSeconds";

		public static readonly StringName _animationLogicAccumulator = "_animationLogicAccumulator";

		public static readonly StringName _visualCadenceAccumulator = "_visualCadenceAccumulator";

		public static readonly StringName _displayVisualDeltaAccumulator = "_displayVisualDeltaAccumulator";

		public static readonly StringName _currentVisualTicksPerSecond = "_currentVisualTicksPerSecond";

		public static readonly StringName _currentBattleCharacterCount = "_currentBattleCharacterCount";

		public static readonly StringName _displayRenderSubmissionPending = "_displayRenderSubmissionPending";

		public static readonly StringName _fixedRenderSubmissionPending = "_fixedRenderSubmissionPending";

		public static readonly StringName _processingDisplayVisualCandidates = "_processingDisplayVisualCandidates";

		public static readonly StringName _lastRenderPhysicsFrame = "_lastRenderPhysicsFrame";

		public static readonly StringName _lastRenderStaticArrayAuditPhysicsFrame = "_lastRenderStaticArrayAuditPhysicsFrame";

		public static readonly StringName _processEligibilityInitialized = "_processEligibilityInitialized";

		public static readonly StringName _processEligibilityEnabled = "_processEligibilityEnabled";

		public static readonly StringName _subscribedGlobal = "_subscribedGlobal";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const double StaticCrowdRefreshIntervalSeconds = 0.2;

	private const int PauseDispatchValidationSlices = 60;

	internal const double AnimationLogicTicksPerSecond = 30.0;

	private const double AnimationLogicTickInterval = 1.0 / 30.0;

	internal const int FullVisualRateCharacterLimitExclusive = 300;

	internal const int HalfVisualRateCharacterLimitExclusive = 500;

	internal const int ThirdVisualRateCharacterLimitExclusive = 800;

	internal const int QuarterVisualRateCharacterLimitExclusive = 1000;

	public static bool UseRuntimeManager = true;

	private static bool _mountRequested;

	private readonly List<AdobeAnimateSprite> _active = new List<AdobeAnimateSprite>();

	private readonly List<AdobeAnimateSprite> _renderRoots = new List<AdobeAnimateSprite>();

	private readonly List<RenderRootOrderEntry> _renderRootOrderEntries = new List<RenderRootOrderEntry>();

	private readonly List<AdobeAnimateSprite> _displayVisualCandidates = new List<AdobeAnimateSprite>();

	private readonly Dictionary<AdobeAnimateSprite, int> _activeIndices = new Dictionary<AdobeAnimateSprite, int>();

	private readonly HashSet<AdobeAnimateSprite> _registered = new HashSet<AdobeAnimateSprite>();

	private readonly HashSet<AdobeAnimateSprite> _renderStaticArrayAuditSprites = new HashSet<AdobeAnimateSprite>();

	private readonly HashSet<AdobeAnimateSprite> _pendingRenderStaticArrayAuditSprites = new HashSet<AdobeAnimateSprite>();

	private readonly List<AdobeAnimateSprite> _staleRenderStaticArrayAuditSprites = new List<AdobeAnimateSprite>();

	private readonly HashSet<AdobeAnimateSprite> _toAdd = new HashSet<AdobeAnimateSprite>();

	private readonly HashSet<AdobeAnimateSprite> _toRemove = new HashSet<AdobeAnimateSprite>();

	private readonly HashSet<AdobeAnimateSprite> _pauseDispatchDirty = new HashSet<AdobeAnimateSprite>();

	private readonly Dictionary<Node, ProcessModeEnum> _processModeParentCache = new Dictionary<Node, ProcessModeEnum>();

	private SceneTree _runtimeTree;

	private ulong _displayTickVersion;

	private bool _dispatchTreePaused;

	private bool _dispatchTreePausedInitialized;

	private int _pauseDispatchValidationCursor;

	private bool _renderRootsDirty = true;

	private double _staticCrowdRefreshAccumulator;

	private double _animationClockSeconds;

	private double _animationLogicAccumulator;

	private double _visualCadenceAccumulator;

	private double _displayVisualDeltaAccumulator;

	private double _currentVisualTicksPerSecond;

	private int _currentBattleCharacterCount;

	private bool _displayRenderSubmissionPending;

	private bool _fixedRenderSubmissionPending;

	private bool _processingDisplayVisualCandidates;

	private ulong _lastRenderPhysicsFrame = 18446744073709551615uL;

	private ulong _lastRenderStaticArrayAuditPhysicsFrame = 18446744073709551615uL;

	private bool _processEligibilityInitialized;

	private bool _processEligibilityEnabled;

	private Global _subscribedGlobal;

	public static AdobeAnimateRuntimeManager Instance { get; private set; }

	internal static double AnimationClockSeconds
	{
		get
		{
			if (!GodotObject.IsInstanceValid(Instance))
			{
				return 0.0;
			}
			return Instance._animationClockSeconds;
		}
	}

	internal int RegisteredSpriteCount => _registered.Count;

	internal double CurrentVisualTicksPerSecond => _currentVisualTicksPerSecond;

	internal int CurrentBattleCharacterCount => _currentBattleCharacterCount;

	internal static bool HasInvalidatedRenderRootOrder
	{
		get
		{
			if (Instance != null)
			{
				return Instance._renderRootsDirty;
			}
			return true;
		}
	}

	internal static void InvalidateRenderRoots()
	{
		if (GodotObject.IsInstanceValid(Instance))
		{
			Instance._renderRootsDirty = true;
		}
	}

	internal static void NotifyRenderRootVisibilityChanged(AdobeAnimateSprite sprite)
	{
		if (GodotObject.IsInstanceValid(Instance) && sprite != null && Instance._registered.Contains(sprite))
		{
			AdobeAnimateRuntimeManager instance = Instance;
			instance._renderRootsDirty = true;
			instance._fixedRenderSubmissionPending = true;
			instance.RefreshProcessEligibility();
		}
	}

	internal static void NotifyGpuRenderCachesCleared()
	{
		AdobeAnimateSprite.InvalidateStaticAtlasPathLayoutCacheForGpuReset();
		if (!GodotObject.IsInstanceValid(Instance))
		{
			return;
		}
		AdobeAnimateRuntimeManager instance = Instance;
		instance._renderRootsDirty = true;
		instance._fixedRenderSubmissionPending = true;
		instance._staticCrowdRefreshAccumulator = 0.0;
		foreach (AdobeAnimateSprite item in instance._registered)
		{
			if (GodotObject.IsInstanceValid(item) && item.IsInsideTree())
			{
				item.InvalidateRuntimeRenderAfterGpuCacheReset();
			}
		}
		instance.SetProcess(enable: true);
	}

	internal static void NotifyViewportTransformChanged()
	{
		if (!GodotObject.IsInstanceValid(Instance))
		{
			return;
		}
		AdobeAnimateRuntimeManager instance = Instance;
		instance._fixedRenderSubmissionPending = true;
		instance._staticCrowdRefreshAccumulator = 0.0;
		foreach (AdobeAnimateSprite item in instance._registered)
		{
			if (GodotObject.IsInstanceValid(item) && item.IsInsideTree())
			{
				item.InvalidateRuntimeCrowdCullingForRefresh();
			}
		}
		instance.SetProcess(enable: true);
	}

	internal static void RequestRenderRootRepublish()
	{
		if (GodotObject.IsInstanceValid(Instance))
		{
			AdobeAnimateRuntimeManager instance = Instance;
			instance._fixedRenderSubmissionPending = true;
			instance._staticCrowdRefreshAccumulator = 0.0;
			instance.SetProcess(enable: true);
		}
	}

	internal static void RecoverFromFailedRenderTransaction(long frameVersion)
	{
		if (!GodotObject.IsInstanceValid(Instance))
		{
			return;
		}
		AdobeAnimateRuntimeManager instance = Instance;
		instance._fixedRenderSubmissionPending = true;
		instance._staticCrowdRefreshAccumulator = 0.0;
		for (int i = 0; i < instance._active.Count; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = instance._active[i];
			if (GodotObject.IsInstanceValid(adobeAnimateSprite) && adobeAnimateSprite.IsRuntimeInsideTree)
			{
				try
				{
					adobeAnimateSprite.RestoreRuntimeNativeCanvasLayer();
					adobeAnimateSprite.RestoreExternalVisualNativeFallbacks(frameVersion);
					adobeAnimateSprite.RequestRuntimeRenderSubmissionRetry();
				}
				catch (Exception value)
				{
					GD.PushError($"Adobe Animate registered sprite recovery failed for {adobeAnimateSprite.Name}: {value}");
				}
			}
		}
		instance.RefreshProcessEligibility();
	}

	internal static void NotifyRenderAncestorTransformChanged()
	{
		if (!GodotObject.IsInstanceValid(Instance))
		{
			return;
		}
		AdobeAnimateRuntimeManager instance = Instance;
		instance.FlushPending();
		instance.RefreshRenderRootsIfNeeded();
		instance._fixedRenderSubmissionPending = true;
		instance._staticCrowdRefreshAccumulator = 0.0;
		for (int i = 0; i < instance._renderRoots.Count; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = instance._renderRoots[i];
			if (GodotObject.IsInstanceValid(adobeAnimateSprite) && adobeAnimateSprite.IsInsideTree())
			{
				adobeAnimateSprite.NotifyAncestorTransformChangedForRender();
				adobeAnimateSprite.InvalidateRuntimeCrowdCullingForRefresh();
			}
		}
		instance.SetProcess(enable: true);
	}

	internal static void NotifyRenderSubmission(AdobeAnimateSprite sprite)
	{
		if (GodotObject.IsInstanceValid(Instance) && GodotObject.IsInstanceValid(sprite) && Instance._registered.Contains(sprite))
		{
			AdobeAnimateRuntimeManager instance = Instance;
			if (instance._processingDisplayVisualCandidates)
			{
				instance._displayRenderSubmissionPending = true;
			}
			else
			{
				instance._fixedRenderSubmissionPending = true;
			}
			instance.RefreshProcessEligibility();
		}
	}

	internal static void NotifyDisplayRenderSubmission(AdobeAnimateSprite sprite)
	{
		if (GodotObject.IsInstanceValid(Instance) && GodotObject.IsInstanceValid(sprite) && Instance._registered.Contains(sprite))
		{
			AdobeAnimateRuntimeManager instance = Instance;
			instance._displayRenderSubmissionPending = true;
			instance.RefreshProcessEligibility();
		}
	}

	internal static void NotifyPauseDispatchInvalidated(AdobeAnimateSprite sprite)
	{
		if (GodotObject.IsInstanceValid(Instance) && GodotObject.IsInstanceValid(sprite) && Instance._registered.Contains(sprite))
		{
			Instance._pauseDispatchDirty.Add(sprite);
			Instance.SetProcess(enable: true);
		}
	}

	internal static void SetRenderStaticArrayAuditRegistration(AdobeAnimateSprite sprite, bool enabled)
	{
		if (GodotObject.IsInstanceValid(Instance) && GodotObject.IsInstanceValid(sprite))
		{
			Instance.UpdateRenderStaticArrayAuditRegistration(sprite, enabled);
		}
	}

	private void UpdateRenderStaticArrayAuditRegistration(AdobeAnimateSprite sprite, bool enabled)
	{
		if (enabled)
		{
			if (_registered.Contains(sprite) && _renderStaticArrayAuditSprites.Add(sprite))
			{
				_pendingRenderStaticArrayAuditSprites.Add(sprite);
			}
		}
		else
		{
			_renderStaticArrayAuditSprites.Remove(sprite);
			_pendingRenderStaticArrayAuditSprites.Remove(sprite);
		}
	}

	public static bool Register(AdobeAnimateSprite sprite)
	{
		if (!UseRuntimeManager || Engine.IsEditorHint() || !GodotObject.IsInstanceValid(sprite) || !sprite.IsInsideTree())
		{
			return false;
		}
		if (!HasUsableInstance())
		{
			TryMount(sprite);
		}
		if (!HasUsableInstance())
		{
			return false;
		}
		AdobeAnimateRuntimeManager instance = Instance;
		instance._toRemove.Remove(sprite);
		if (instance._registered.Contains(sprite))
		{
			if (sprite.RequiresRenderStaticArrayAuditForRuntimeManager())
			{
				instance.UpdateRenderStaticArrayAuditRegistration(sprite, enabled: true);
			}
			else
			{
				instance.UpdateRenderStaticArrayAuditRegistration(sprite, enabled: false);
			}
			return true;
		}
		instance._registered.Add(sprite);
		if (sprite.RequiresRenderStaticArrayAuditForRuntimeManager())
		{
			instance.UpdateRenderStaticArrayAuditRegistration(sprite, enabled: true);
		}
		instance._toAdd.Add(sprite);
		instance._pauseDispatchDirty.Add(sprite);
		instance.RefreshProcessEligibility();
		return true;
	}

	public static void Unregister(AdobeAnimateSprite sprite)
	{
		if (GodotObject.IsInstanceValid(Instance))
		{
			AdobeAnimateRuntimeManager instance = Instance;
			if (instance._toAdd.Remove(sprite))
			{
				instance._registered.Remove(sprite);
				instance.UpdateRenderStaticArrayAuditRegistration(sprite, enabled: false);
				instance._pauseDispatchDirty.Remove(sprite);
				instance.RefreshProcessEligibility();
			}
			else if (instance._registered.Contains(sprite))
			{
				instance.UpdateRenderStaticArrayAuditRegistration(sprite, enabled: false);
				instance._pauseDispatchDirty.Remove(sprite);
				instance._toRemove.Add(sprite);
				instance.RefreshProcessEligibility();
			}
		}
	}

	public override void _EnterTree()
	{
		_runtimeTree = GetTree();
	}

	public override void _Ready()
	{
		if (GodotObject.IsInstanceValid(Instance) && Instance != this)
		{
			SetProcess(enable: false);
			SetPhysicsProcess(enable: false);
			QueueFree();
			return;
		}
		Instance = this;
		_mountRequested = false;
		_animationClockSeconds = 0.0;
		_animationLogicAccumulator = 0.0;
		_visualCadenceAccumulator = 0.0;
		_displayVisualDeltaAccumulator = 0.0;
		_currentVisualTicksPerSecond = 0.0;
		_currentBattleCharacterCount = 0;
		_lastRenderPhysicsFrame = 18446744073709551615uL;
		_lastRenderStaticArrayAuditPhysicsFrame = 18446744073709551615uL;
		ProcessMode = ProcessModeEnum.Always;
		_subscribedGlobal = Global.Instance;
		if (_subscribedGlobal != null)
		{
			_subscribedGlobal.OnAdobeAnimateRenderBackendChanged += HandleAdobeAnimateRenderBackendChanged;
		}
		RefreshProcessEligibility();
	}

	public override void _ExitTree()
	{
		_runtimeTree = null;
		if (_subscribedGlobal != null)
		{
			_subscribedGlobal.OnAdobeAnimateRenderBackendChanged -= HandleAdobeAnimateRenderBackendChanged;
		}
		_subscribedGlobal = null;
		if (Instance == this)
		{
			Instance = null;
			_mountRequested = false;
		}
		_active.Clear();
		_renderRoots.Clear();
		_renderRootOrderEntries.Clear();
		_displayVisualCandidates.Clear();
		_activeIndices.Clear();
		_registered.Clear();
		_renderStaticArrayAuditSprites.Clear();
		_pendingRenderStaticArrayAuditSprites.Clear();
		_staleRenderStaticArrayAuditSprites.Clear();
		_toAdd.Clear();
		_toRemove.Clear();
		_pauseDispatchDirty.Clear();
		_processModeParentCache.Clear();
		_displayTickVersion = 0uL;
		_dispatchTreePaused = false;
		_dispatchTreePausedInitialized = false;
		_pauseDispatchValidationCursor = 0;
		_renderRootsDirty = true;
		_staticCrowdRefreshAccumulator = 0.0;
		_animationClockSeconds = 0.0;
		_animationLogicAccumulator = 0.0;
		_visualCadenceAccumulator = 0.0;
		_displayVisualDeltaAccumulator = 0.0;
		_currentVisualTicksPerSecond = 0.0;
		_currentBattleCharacterCount = 0;
		_displayRenderSubmissionPending = false;
		_fixedRenderSubmissionPending = false;
		_processingDisplayVisualCandidates = false;
		_lastRenderPhysicsFrame = 18446744073709551615uL;
		_lastRenderStaticArrayAuditPhysicsFrame = 18446744073709551615uL;
		_processEligibilityInitialized = false;
		_processEligibilityEnabled = false;
	}

	private void HandleAdobeAnimateRenderBackendChanged(AdobeAnimateRenderBackend _)
	{
		_renderRootsDirty = true;
		foreach (AdobeAnimateSprite item in _registered)
		{
			if (GodotObject.IsInstanceValid(item))
			{
				item.InvalidateRuntimeCrowdCullingForRefresh();
			}
		}
		if (_registered.Count != 0)
		{
			_fixedRenderSubmissionPending = true;
			SetProcess(enable: true);
		}
	}

	public override void _Process(double delta)
	{
		if (GodotObject.IsInstanceValid(Instance) && Instance != this)
		{
			SetProcess(enable: false);
			SetPhysicsProcess(enable: false);
			QueueFree();
			return;
		}
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		long startBytes2 = TowerDefenseAllocationTelemetry.Begin();
		_animationClockSeconds += delta;
		_animationLogicAccumulator += delta;
		_displayVisualDeltaAccumulator += delta;
		_currentBattleCharacterCount = ResolveBattleCharacterCount();
		int renderFrameRate = ResolveRenderFrameRate();
		double num = ResolveAdaptiveVisualTicksPerSecond(renderFrameRate, _currentBattleCharacterCount);
		bool flag = AdvanceVisualCadence(delta, num, renderFrameRate);
		AdobeAnimateRenderManager.SetAnimationClock(_animationClockSeconds);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RuntimeClock, startBytes2);
		_displayTickVersion++;
		bool flag2 = false;
		long startBytes3 = TowerDefenseAllocationTelemetry.Begin();
		if (flag)
		{
			double displayVisualDeltaAccumulator = _displayVisualDeltaAccumulator;
			_displayVisualDeltaAccumulator = 0.0;
			_processingDisplayVisualCandidates = true;
			try
			{
				flag2 = ProcessDisplayVisualCandidates(displayVisualDeltaAccumulator);
			}
			finally
			{
				_processingDisplayVisualCandidates = false;
			}
		}
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RuntimeDisplay, startBytes3);
		_displayRenderSubmissionPending |= flag2;
		long startBytes4 = TowerDefenseAllocationTelemetry.Begin();
		long startTicks = TowerDefensePerfProfiler.Begin();
		bool flag3 = ProcessRenderStaticArrayAudits();
		TowerDefensePerfProfiler.End("adobeAnimate.process.staticArrayAudit", startTicks, _renderStaticArrayAuditSprites.Count);
		if (flag3)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.reason.staticArrayAudit", 1);
		}
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RuntimeStaticAudit, startBytes4);
		bool flag4 = (_displayRenderSubmissionPending || _fixedRenderSubmissionPending) | flag3;
		if (_animationLogicAccumulator >= 1.0 / 30.0)
		{
			long startBytes5 = TowerDefenseAllocationTelemetry.Begin();
			double num2 = (double)Math.Max(1, (int)Math.Floor(_animationLogicAccumulator / (1.0 / 30.0))) * (1.0 / 30.0);
			_animationLogicAccumulator = Math.Max(0.0, _animationLogicAccumulator - num2);
			_processingDisplayVisualCandidates = true;
			bool flag5;
			try
			{
				flag5 = ProcessActive(num2);
			}
			finally
			{
				_processingDisplayVisualCandidates = false;
			}
			flag4 |= flag5;
			TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RuntimeLogic, startBytes5);
		}
		_staticCrowdRefreshAccumulator += Math.Max(0.0, delta);
		bool renderRootsDirty = _renderRootsDirty;
		bool flag6 = _staticCrowdRefreshAccumulator >= 0.2;
		bool flag7 = false;
		if (flag6)
		{
			flag7 = InvalidateRuntimeCrowdCullingIfCachedOffscreen();
			_staticCrowdRefreshAccumulator %= 0.2;
		}
		bool flag8 = renderRootsDirty | flag4 | flag7;
		ulong physicsFrames = Engine.GetPhysicsFrames();
		bool flag9 = _lastRenderPhysicsFrame != physicsFrames;
		bool num3 = ((renderRootsDirty | flag7) || _fixedRenderSubmissionPending) & flag9;
		bool flag10 = _displayRenderSubmissionPending & flag;
		bool flag11 = num3 | flag10;
		SceneTree runtimeTree = _runtimeTree;
		if (!flag11 && GodotObject.IsInstanceValid(runtimeTree) && runtimeTree.Paused)
		{
			flag11 = true;
		}
		if (TowerDefensePerfProfiler.DetailedHotPathMetrics)
		{
			if (flag4)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.reason.invalidated", 1);
			}
			if (renderRootsDirty)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.reason.topology", 1);
			}
			if (flag7)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.reason.periodic", 1);
			}
			if (flag10)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.render.reason.displayCadence", 1);
			}
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.visual.characterCount", _currentBattleCharacterCount);
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.visual.targetFps", (int)Math.Round(num));
		}
		if (flag8 & flag11)
		{
			_displayRenderSubmissionPending = false;
			_fixedRenderSubmissionPending = false;
			RenderActive();
			_fixedRenderSubmissionPending |= MarkRuntimeRenderSubmissionsConsumed();
			_lastRenderPhysicsFrame = physicsFrames;
			_staticCrowdRefreshAccumulator = 0.0;
		}
		else if (!flag8)
		{
			TowerDefensePerfProfiler.DumpIfNeeded();
		}
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RuntimeProcess, startBytes);
		RefreshProcessEligibility();
	}

	private bool ProcessRenderStaticArrayAudits()
	{
		ulong physicsFrames = Engine.GetPhysicsFrames();
		bool flag = _lastRenderStaticArrayAuditPhysicsFrame != physicsFrames;
		if (!flag && _pendingRenderStaticArrayAuditSprites.Count == 0)
		{
			return false;
		}
		if (_renderStaticArrayAuditSprites.Count == 0)
		{
			_pendingRenderStaticArrayAuditSprites.Clear();
			return false;
		}
		bool flag2 = false;
		_staleRenderStaticArrayAuditSprites.Clear();
		if (flag)
		{
			_lastRenderStaticArrayAuditPhysicsFrame = physicsFrames;
			_pendingRenderStaticArrayAuditSprites.Clear();
			foreach (AdobeAnimateSprite renderStaticArrayAuditSprite in _renderStaticArrayAuditSprites)
			{
				flag2 |= AuditRenderStaticArraySprite(renderStaticArrayAuditSprite, physicsFrames);
			}
		}
		else
		{
			foreach (AdobeAnimateSprite pendingRenderStaticArrayAuditSprite in _pendingRenderStaticArrayAuditSprites)
			{
				flag2 |= AuditRenderStaticArraySprite(pendingRenderStaticArrayAuditSprite, physicsFrames);
			}
			_pendingRenderStaticArrayAuditSprites.Clear();
		}
		for (int i = 0; i < _staleRenderStaticArrayAuditSprites.Count; i++)
		{
			UpdateRenderStaticArrayAuditRegistration(_staleRenderStaticArrayAuditSprites[i], enabled: false);
		}
		_staleRenderStaticArrayAuditSprites.Clear();
		return flag2;
	}

	private bool AuditRenderStaticArraySprite(AdobeAnimateSprite sprite, ulong physicsFrame)
	{
		if (!GodotObject.IsInstanceValid(sprite) || !sprite.IsInsideTree())
		{
			_staleRenderStaticArrayAuditSprites.Add(sprite);
			return false;
		}
		return sprite.QueueRenderStaticArrayAuditIfDue(physicsFrame);
	}

	private static int ResolveRenderFrameRate()
	{
		Global instance = Global.Instance;
		if (instance != null && instance.effectiveAnimeFrameRate > 0)
		{
			return instance.effectiveAnimeFrameRate;
		}
		int maxFps = Engine.MaxFps;
		if (maxFps <= 0)
		{
			return 60;
		}
		return maxFps;
	}

	private static int ResolveBattleCharacterCount()
	{
		TowerDefenseManager instance = TowerDefenseManager.Instance;
		if (!GodotObject.IsInstanceValid(instance))
		{
			return 0;
		}
		TowerDefenseBattleCharacterRegistry towerDefenseBattleCharacterRegistry = instance?.characterRegistry;
		if (!GodotObject.IsInstanceValid(towerDefenseBattleCharacterRegistry))
		{
			return 0;
		}
		return Math.Max(0, towerDefenseBattleCharacterRegistry.ActiveCharacterCount);
	}

	internal static double ResolveAdaptiveVisualTicksPerSecond(int renderFrameRate, int characterCount)
	{
		int num = Math.Max(1, renderFrameRate);
		int num2 = Math.Max(0, characterCount);
		double val;
		if (num2 < 300)
		{
			val = 180.0;
		}
		else if (num2 < 500)
		{
			val = 90.0;
		}
		else if (num2 < 800)
		{
			val = 60.0;
		}
		else
		{
			val = ((num2 < 1000) ? 45.0 : 30.0);
		}
		return Math.Min(num, val);
	}

	private bool AdvanceVisualCadence(double delta, double visualTicksPerSecond, int renderFrameRate)
	{
		bool flag = !Mathf.IsEqualApprox((float)_currentVisualTicksPerSecond, (float)visualTicksPerSecond);
		_currentVisualTicksPerSecond = visualTicksPerSecond;
		if (flag)
		{
			_visualCadenceAccumulator = 0.0;
		}
		if (visualTicksPerSecond >= (double)renderFrameRate - 0.001)
		{
			_visualCadenceAccumulator = 0.0;
			return true;
		}
		double num = 1.0 / Math.Max(1.0, visualTicksPerSecond);
		_visualCadenceAccumulator += Math.Max(0.0, delta);
		if (!flag && _visualCadenceAccumulator + 1E-06 < num)
		{
			return false;
		}
		_visualCadenceAccumulator %= num;
		return true;
	}

	private void RefreshProcessEligibility()
	{
		bool flag = _registered.Count > 0 || _toAdd.Count > 0 || _toRemove.Count > 0;
		if (!_processEligibilityInitialized || _processEligibilityEnabled != flag)
		{
			_processEligibilityInitialized = true;
			_processEligibilityEnabled = flag;
			SetProcess(flag);
		}
	}

	private static bool HasUsableInstance()
	{
		return GodotObject.IsInstanceValid(Instance);
	}

	private static void TryMount(AdobeAnimateSprite sprite)
	{
		if (_mountRequested)
		{
			return;
		}
		SceneTree tree = sprite.GetTree();
		if (GodotObject.IsInstanceValid(tree) && GodotObject.IsInstanceValid(tree.Root))
		{
			AdobeAnimateRuntimeManager instance = FindExistingManager(tree.Root);
			if (GodotObject.IsInstanceValid(instance))
			{
				Instance = instance;
				return;
			}
			AdobeAnimateRuntimeManager adobeAnimateRuntimeManager = (Instance = new AdobeAnimateRuntimeManager
			{
				Name = "AdobeAnimateRuntimeManager"
			});
			_mountRequested = true;
			tree.Root.CallDeferred(Node.MethodName.AddChild, adobeAnimateRuntimeManager);
		}
	}

	private static AdobeAnimateRuntimeManager FindExistingManager(Node root)
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		int childCount = root.GetChildCount();
		for (int i = 0; i < childCount; i++)
		{
			if (root.GetChild(i) is AdobeAnimateRuntimeManager adobeAnimateRuntimeManager && GodotObject.IsInstanceValid(adobeAnimateRuntimeManager))
			{
				return adobeAnimateRuntimeManager;
			}
		}
		return null;
	}

	private bool ProcessDisplayVisualCandidates(double delta)
	{
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		long startTicks = TowerDefensePerfProfiler.Begin();
		int num = 0;
		bool flag = false;
		int num2 = 0;
		int num3 = 0;
		long startTicks2 = TowerDefensePerfProfiler.Begin();
		FlushPending();
		TowerDefensePerfProfiler.End("adobeAnimate.process.flushPending", startTicks2);
		RefreshPauseDispatchState(validatePeriodicSlice: false);
		for (int i = 0; i < _displayVisualCandidates.Count; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _displayVisualCandidates[i];
			if (adobeAnimateSprite == null || !adobeAnimateSprite.IsRuntimeInsideTree)
			{
				_toRemove.Add(adobeAnimateSprite);
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.process.detachedSprite");
				continue;
			}
			flag |= adobeAnimateSprite.NeedsRuntimeRenderSubmission;
			if (!adobeAnimateSprite.IsRuntimeDisplayTickActive)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.process.inactive");
				continue;
			}
			if (!adobeAnimateSprite.RuntimeManagerDispatchActive)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.process.pauseModeSkip");
				continue;
			}
			adobeAnimateSprite.MarkRuntimeDisplayTicked(_displayTickVersion);
			long startTicks3 = TowerDefensePerfProfiler.BeginHotPath();
			adobeAnimateSprite.RunDisplayFrameVisualUpdate(delta);
			bool needsRuntimeRenderSubmission = adobeAnimateSprite.NeedsRuntimeRenderSubmission;
			flag |= needsRuntimeRenderSubmission;
			if (TowerDefensePerfProfiler.DetailedHotPathMetrics)
			{
				if (adobeAnimateSprite.UsesRuntimeGpuClockInterpolation)
				{
					num2++;
				}
				if (needsRuntimeRenderSubmission)
				{
					num3++;
				}
			}
			TowerDefensePerfProfiler.End("adobeAnimate.process.sprite", startTicks3, 1);
			num++;
		}
		if (_toRemove.Count > 0)
		{
			startTicks2 = TowerDefensePerfProfiler.Begin();
			FlushPending();
			TowerDefensePerfProfiler.End("adobeAnimate.process.flushRemoved", startTicks2);
		}
		if (TowerDefensePerfProfiler.DetailedHotPathMetrics)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.process.gpuClockActive", num2);
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.process.dirtyAfter", num3);
		}
		TowerDefensePerfProfiler.End("batch.adobeAnimate.displayVisual", startTicks, num);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.AnimationDisplay, startBytes);
		return flag;
	}

	private bool ProcessActive(double delta)
	{
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		long startTicks = TowerDefensePerfProfiler.Begin();
		int num = 0;
		bool flag = false;
		int num2 = 0;
		int num3 = 0;
		long startTicks2 = TowerDefensePerfProfiler.Begin();
		FlushPending();
		TowerDefensePerfProfiler.End("adobeAnimate.process.flushPending", startTicks2);
		RefreshPauseDispatchState(validatePeriodicSlice: true);
		_displayVisualCandidates.Clear();
		for (int i = 0; i < _active.Count; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _active[i];
			if (adobeAnimateSprite == null || !adobeAnimateSprite.IsRuntimeInsideTree)
			{
				_toRemove.Add(adobeAnimateSprite);
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.process.detachedSprite");
				continue;
			}
			flag |= adobeAnimateSprite.NeedsRuntimeRenderSubmission;
			if (!adobeAnimateSprite.IsRuntimeActive)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.process.inactive");
			}
			else if (adobeAnimateSprite.IsRuntimeTickPaused)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.process.paused");
			}
			else if (!adobeAnimateSprite.RuntimeManagerDispatchActive)
			{
				TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.process.pauseModeSkip");
			}
			else if (adobeAnimateSprite.RequiresDisplayFrameVisualTick())
			{
				_displayVisualCandidates.Add(adobeAnimateSprite);
			}
			else
			{
				if (adobeAnimateSprite.WasRuntimeDisplayTicked(_displayTickVersion))
				{
					continue;
				}
				long startTicks3 = TowerDefensePerfProfiler.BeginHotPath();
				adobeAnimateSprite.RunDefaultBatchedProcessUpdate(delta);
				bool needsRuntimeRenderSubmission = adobeAnimateSprite.NeedsRuntimeRenderSubmission;
				flag |= needsRuntimeRenderSubmission;
				if (TowerDefensePerfProfiler.DetailedHotPathMetrics)
				{
					if (adobeAnimateSprite.UsesRuntimeGpuClockInterpolation)
					{
						num2++;
					}
					if (needsRuntimeRenderSubmission)
					{
						num3++;
					}
				}
				TowerDefensePerfProfiler.End("adobeAnimate.process.sprite", startTicks3, 1);
				num++;
			}
		}
		if (_toRemove.Count > 0)
		{
			startTicks2 = TowerDefensePerfProfiler.Begin();
			FlushPending();
			TowerDefensePerfProfiler.End("adobeAnimate.process.flushRemoved", startTicks2);
		}
		if (TowerDefensePerfProfiler.DetailedHotPathMetrics)
		{
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.process.gpuClockActive", num2);
			TowerDefensePerfProfiler.SampleHotPath("adobeAnimate.process.dirtyAfter", num3);
		}
		TowerDefensePerfProfiler.End("batch.adobeAnimate.process", startTicks, num);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.AnimationLogic, startBytes);
		return flag;
	}

	private bool MarkRuntimeRenderSubmissionsConsumed()
	{
		bool flag = false;
		long lastCompletedRuntimeTransactionVersion = AdobeAnimateRenderManager.LastCompletedRuntimeTransactionVersion;
		for (int i = 0; i < _active.Count; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _active[i];
			if (adobeAnimateSprite != null && adobeAnimateSprite.IsRuntimeInsideTree)
			{
				if (lastCompletedRuntimeTransactionVersion != -9223372036854775808L)
				{
					flag |= adobeAnimateSprite.RecoverRuntimeNativeCanvasAfterMissedTransaction(lastCompletedRuntimeTransactionVersion);
				}
				flag |= adobeAnimateSprite.MarkRuntimeRenderSubmissionConsumed();
			}
		}
		return flag;
	}

	private bool InvalidateRuntimeCrowdCullingIfCachedOffscreen()
	{
		RefreshRenderRootsIfNeeded();
		bool flag = false;
		for (int i = 0; i < _renderRoots.Count; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _renderRoots[i];
			if (adobeAnimateSprite != null && adobeAnimateSprite.IsRuntimeInsideTree)
			{
				flag |= adobeAnimateSprite.InvalidateRuntimeCrowdCullingIfCachedOffscreen();
			}
		}
		return flag;
	}

	private void RenderActive()
	{
		RefreshRenderRootsIfNeeded();
		long startBytes = TowerDefenseAllocationTelemetry.Begin();
		long startTicks = TowerDefensePerfProfiler.Begin();
		AdobeAnimateRenderManager.RenderRuntimeTreeOrdered(_renderRoots);
		TowerDefensePerfProfiler.End("adobeAnimate.render.dispatch", startTicks, _renderRoots.Count);
		TowerDefenseAllocationTelemetry.End(TowerDefenseAllocationMetric.RenderDispatch, startBytes);
		TowerDefensePerfProfiler.DumpIfNeeded();
	}

	private void RefreshRenderRootsIfNeeded()
	{
		if (!_renderRootsDirty)
		{
			return;
		}
		_renderRoots.Clear();
		_renderRootOrderEntries.Clear();
		for (int i = 0; i < _active.Count; i++)
		{
			AdobeAnimateSprite adobeAnimateSprite = _active[i];
			if (adobeAnimateSprite != null && adobeAnimateSprite.IsRuntimeInsideTree && adobeAnimateSprite.IsRuntimeVisibleInTree && !adobeAnimateSprite.IsRenderedByParentSpriteForRender())
			{
				adobeAnimateSprite.InvalidateRuntimeTreeOrderForTopologyChange();
				_renderRoots.Add(adobeAnimateSprite);
				_renderRootOrderEntries.Add(new RenderRootOrderEntry(adobeAnimateSprite, adobeAnimateSprite.GetEffectiveTreeOrderPathForRender(), adobeAnimateSprite.GetCachedInstanceIdForRender()));
			}
		}
		if (_renderRootOrderEntries.Count > 1)
		{
			_renderRootOrderEntries.Sort(RenderRootOrderEntryComparer.Instance);
		}
		for (int j = 0; j < _renderRootOrderEntries.Count; j++)
		{
			_renderRoots[j] = _renderRootOrderEntries[j].Sprite;
		}
		_renderRootsDirty = false;
	}

	private void RefreshPauseDispatchState(bool validatePeriodicSlice)
	{
		SceneTree runtimeTree = _runtimeTree;
		bool flag = GodotObject.IsInstanceValid(runtimeTree) && runtimeTree.Paused;
		bool num = !_dispatchTreePausedInitialized || flag != _dispatchTreePaused;
		_dispatchTreePaused = flag;
		_dispatchTreePausedInitialized = true;
		_processModeParentCache.Clear();
		if (num)
		{
			for (int i = 0; i < _active.Count; i++)
			{
				ResolvePauseDispatch(_active[i]);
			}
			_pauseDispatchDirty.Clear();
			_pauseDispatchValidationCursor = 0;
			return;
		}
		if (_pauseDispatchDirty.Count > 0)
		{
			foreach (AdobeAnimateSprite item in _pauseDispatchDirty)
			{
				if (_registered.Contains(item))
				{
					ResolvePauseDispatch(item);
				}
			}
			_pauseDispatchDirty.Clear();
		}
		if (!validatePeriodicSlice || _active.Count == 0)
		{
			return;
		}
		int num2 = Math.Max(1, (_active.Count + 60 - 1) / 60);
		for (int j = 0; j < num2; j++)
		{
			if (_pauseDispatchValidationCursor >= _active.Count)
			{
				_pauseDispatchValidationCursor = 0;
			}
			ResolvePauseDispatch(_active[_pauseDispatchValidationCursor]);
			_pauseDispatchValidationCursor++;
		}
	}

	private void ResolvePauseDispatch(AdobeAnimateSprite sprite)
	{
		if (sprite == null || !sprite.IsRuntimeInsideTree)
		{
			_toRemove.Add(sprite);
		}
		else
		{
			sprite.SetRuntimeManagerDispatchActive(ShouldDispatchInCurrentPauseState(sprite));
		}
	}

	private bool ShouldDispatchInCurrentPauseState(Node node)
	{
		bool dispatchTreePaused = _dispatchTreePaused;
		ProcessModeEnum processMode = node.ProcessMode;
		switch (processMode)
		{
		case ProcessModeEnum.Disabled:
			return false;
		default:
			if (!dispatchTreePaused)
			{
				return processMode != ProcessModeEnum.WhenPaused;
			}
			if (processMode != ProcessModeEnum.Always)
			{
				return processMode == ProcessModeEnum.WhenPaused;
			}
			return true;
		case ProcessModeEnum.Inherit:
		{
			ProcessModeEnum processModeEnum = ResolveInheritedProcessMode(node);
			if (processModeEnum == ProcessModeEnum.Disabled)
			{
				return false;
			}
			ProcessModeEnum processModeEnum2 = processModeEnum;
			if (!dispatchTreePaused)
			{
				return processModeEnum2 != ProcessModeEnum.WhenPaused;
			}
			if (processModeEnum2 != ProcessModeEnum.Always)
			{
				return processModeEnum2 == ProcessModeEnum.WhenPaused;
			}
			return true;
		}
		}
	}

	private ProcessModeEnum ResolveInheritedProcessMode(Node node)
	{
		Node parent = node.GetParent();
		if (!GodotObject.IsInstanceValid(parent))
		{
			return ProcessModeEnum.Pausable;
		}
		if (_processModeParentCache.TryGetValue(parent, out var value))
		{
			return value;
		}
		ProcessModeEnum processModeEnum = ResolveEffectiveProcessMode(parent);
		_processModeParentCache[parent] = processModeEnum;
		return processModeEnum;
	}

	private static ProcessModeEnum ResolveEffectiveProcessMode(Node node)
	{
		Node node2 = node;
		while (GodotObject.IsInstanceValid(node2))
		{
			ProcessModeEnum processMode = node2.ProcessMode;
			if (processMode != ProcessModeEnum.Inherit)
			{
				return processMode;
			}
			node2 = node2.GetParent();
		}
		return ProcessModeEnum.Pausable;
	}

	private void FlushPending()
	{
		if (_toRemove.Count > 0)
		{
			foreach (AdobeAnimateSprite item in _toRemove)
			{
				if (_activeIndices.TryGetValue(item, out var value))
				{
					RemoveActiveAt(value);
				}
				_registered.Remove(item);
				_pauseDispatchDirty.Remove(item);
			}
			_toRemove.Clear();
		}
		if (_toAdd.Count <= 0)
		{
			return;
		}
		foreach (AdobeAnimateSprite item2 in _toAdd)
		{
			if (GodotObject.IsInstanceValid(item2))
			{
				AddActive(item2);
			}
			else
			{
				_registered.Remove(item2);
			}
		}
		_toAdd.Clear();
	}

	private void AddActive(AdobeAnimateSprite sprite)
	{
		if (!_activeIndices.ContainsKey(sprite))
		{
			_activeIndices[sprite] = _active.Count;
			_active.Add(sprite);
			_pauseDispatchDirty.Add(sprite);
			_renderRootsDirty = true;
		}
	}

	private void RemoveActiveAt(int index)
	{
		int num = _active.Count - 1;
		if ((uint)index <= (uint)num)
		{
			AdobeAnimateSprite adobeAnimateSprite = _active[index];
			if (index != num)
			{
				AdobeAnimateSprite adobeAnimateSprite2 = _active[num];
				_active[index] = adobeAnimateSprite2;
				_activeIndices[adobeAnimateSprite2] = index;
			}
			_active.RemoveAt(num);
			_activeIndices.Remove(adobeAnimateSprite);
			_pauseDispatchDirty.Remove(adobeAnimateSprite);
			if (_pauseDispatchValidationCursor > _active.Count)
			{
				_pauseDispatchValidationCursor = 0;
			}
			_renderRootsDirty = true;
		}
	}

	internal void SetAnimationClockForBareTest(double seconds)
	{
		_animationClockSeconds = seconds;
	}

	internal static bool TryCopyRenderRootOrderForTests(Span<AdobeAnimateSprite> destination, out int totalCount, out bool dirty)
	{
		totalCount = 0;
		dirty = true;
		AdobeAnimateRuntimeManager instance = Instance;
		if (instance == null)
		{
			return false;
		}
		totalCount = instance._renderRoots.Count;
		dirty = instance._renderRootsDirty;
		int num = Math.Min(totalCount, destination.Length);
		for (int i = 0; i < num; i++)
		{
			destination[i] = instance._renderRoots[i];
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(44)
		{
			new MethodInfo(MethodName.InvalidateRenderRoots, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.NotifyRenderRootVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyGpuRenderCachesCleared, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.NotifyViewportTransformChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RequestRenderRootRepublish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.RecoverFromFailedRenderTransaction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "frameVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyRenderAncestorTransformChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.NotifyRenderSubmission, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyDisplayRenderSubmission, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyPauseDispatchInvalidated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetRenderStaticArrayAuditRegistration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateRenderStaticArrayAuditRegistration, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Register, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.Unregister, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleAdobeAnimateRenderBackendChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessRenderStaticArrayAudits, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AuditRenderStaticArraySprite, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "physicsFrame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveRenderFrameRate, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ResolveBattleCharacterCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ResolveAdaptiveVisualTicksPerSecond, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "renderFrameRate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "characterCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AdvanceVisualCadence, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "visualTicksPerSecond", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "renderFrameRate", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshProcessEligibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasUsableInstance, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.TryMount, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindExistingManager, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessDisplayVisualCandidates, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ProcessActive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MarkRuntimeRenderSubmissionsConsumed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateRuntimeCrowdCullingIfCachedOffscreen, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshRenderRootsIfNeeded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPauseDispatchState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "validatePeriodicSlice", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolvePauseDispatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldDispatchInCurrentPauseState, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveInheritedProcessMode, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveEffectiveProcessMode, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FlushPending, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveActiveAt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAnimationClockForBareTest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "seconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.InvalidateRenderRoots && args.Count == 0)
		{
			InvalidateRenderRoots();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyRenderRootVisibilityChanged && args.Count == 1)
		{
			NotifyRenderRootVisibilityChanged(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyGpuRenderCachesCleared && args.Count == 0)
		{
			NotifyGpuRenderCachesCleared();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyViewportTransformChanged && args.Count == 0)
		{
			NotifyViewportTransformChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.RequestRenderRootRepublish && args.Count == 0)
		{
			RequestRenderRootRepublish();
			ret = default;
			return true;
		}
		if (method == MethodName.RecoverFromFailedRenderTransaction && args.Count == 1)
		{
			RecoverFromFailedRenderTransaction(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyRenderAncestorTransformChanged && args.Count == 0)
		{
			NotifyRenderAncestorTransformChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyRenderSubmission && args.Count == 1)
		{
			NotifyRenderSubmission(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyDisplayRenderSubmission && args.Count == 1)
		{
			NotifyDisplayRenderSubmission(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyPauseDispatchInvalidated && args.Count == 1)
		{
			NotifyPauseDispatchInvalidated(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRenderStaticArrayAuditRegistration && args.Count == 2)
		{
			SetRenderStaticArrayAuditRegistration(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateRenderStaticArrayAuditRegistration && args.Count == 2)
		{
			UpdateRenderStaticArrayAuditRegistration(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Register && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Register(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.Unregister && args.Count == 1)
		{
			Unregister(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
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
		if (method == MethodName.HandleAdobeAnimateRenderBackendChanged && args.Count == 1)
		{
			HandleAdobeAnimateRenderBackendChanged(VariantUtils.ConvertTo<AdobeAnimateRenderBackend>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessRenderStaticArrayAudits && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ProcessRenderStaticArrayAudits());
			return true;
		}
		if (method == MethodName.AuditRenderStaticArraySprite && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(AuditRenderStaticArraySprite(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<ulong>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolveRenderFrameRate && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveRenderFrameRate());
			return true;
		}
		if (method == MethodName.ResolveBattleCharacterCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveBattleCharacterCount());
			return true;
		}
		if (method == MethodName.ResolveAdaptiveVisualTicksPerSecond && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(ResolveAdaptiveVisualTicksPerSecond(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.AdvanceVisualCadence && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(AdvanceVisualCadence(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.RefreshProcessEligibility && args.Count == 0)
		{
			RefreshProcessEligibility();
			ret = default;
			return true;
		}
		if (method == MethodName.HasUsableInstance && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasUsableInstance());
			return true;
		}
		if (method == MethodName.TryMount && args.Count == 1)
		{
			TryMount(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindExistingManager && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateRuntimeManager>(FindExistingManager(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ProcessDisplayVisualCandidates && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ProcessDisplayVisualCandidates(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.ProcessActive && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ProcessActive(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.MarkRuntimeRenderSubmissionsConsumed && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(MarkRuntimeRenderSubmissionsConsumed());
			return true;
		}
		if (method == MethodName.InvalidateRuntimeCrowdCullingIfCachedOffscreen && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(InvalidateRuntimeCrowdCullingIfCachedOffscreen());
			return true;
		}
		if (method == MethodName.RenderActive && args.Count == 0)
		{
			RenderActive();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshRenderRootsIfNeeded && args.Count == 0)
		{
			RefreshRenderRootsIfNeeded();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPauseDispatchState && args.Count == 1)
		{
			RefreshPauseDispatchState(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePauseDispatch && args.Count == 1)
		{
			ResolvePauseDispatch(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldDispatchInCurrentPauseState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldDispatchInCurrentPauseState(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveInheritedProcessMode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ProcessModeEnum>(ResolveInheritedProcessMode(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveEffectiveProcessMode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ProcessModeEnum>(ResolveEffectiveProcessMode(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FlushPending && args.Count == 0)
		{
			FlushPending();
			ret = default;
			return true;
		}
		if (method == MethodName.AddActive && args.Count == 1)
		{
			AddActive(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveActiveAt && args.Count == 1)
		{
			RemoveActiveAt(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAnimationClockForBareTest && args.Count == 1)
		{
			SetAnimationClockForBareTest(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.InvalidateRenderRoots && args.Count == 0)
		{
			InvalidateRenderRoots();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyRenderRootVisibilityChanged && args.Count == 1)
		{
			NotifyRenderRootVisibilityChanged(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyGpuRenderCachesCleared && args.Count == 0)
		{
			NotifyGpuRenderCachesCleared();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyViewportTransformChanged && args.Count == 0)
		{
			NotifyViewportTransformChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.RequestRenderRootRepublish && args.Count == 0)
		{
			RequestRenderRootRepublish();
			ret = default;
			return true;
		}
		if (method == MethodName.RecoverFromFailedRenderTransaction && args.Count == 1)
		{
			RecoverFromFailedRenderTransaction(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyRenderAncestorTransformChanged && args.Count == 0)
		{
			NotifyRenderAncestorTransformChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyRenderSubmission && args.Count == 1)
		{
			NotifyRenderSubmission(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyDisplayRenderSubmission && args.Count == 1)
		{
			NotifyDisplayRenderSubmission(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyPauseDispatchInvalidated && args.Count == 1)
		{
			NotifyPauseDispatchInvalidated(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRenderStaticArrayAuditRegistration && args.Count == 2)
		{
			SetRenderStaticArrayAuditRegistration(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Register && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(Register(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0])));
			return true;
		}
		if (method == MethodName.Unregister && args.Count == 1)
		{
			Unregister(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveRenderFrameRate && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveRenderFrameRate());
			return true;
		}
		if (method == MethodName.ResolveBattleCharacterCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ResolveBattleCharacterCount());
			return true;
		}
		if (method == MethodName.ResolveAdaptiveVisualTicksPerSecond && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<double>(ResolveAdaptiveVisualTicksPerSecond(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.HasUsableInstance && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasUsableInstance());
			return true;
		}
		if (method == MethodName.TryMount && args.Count == 1)
		{
			TryMount(VariantUtils.ConvertTo<AdobeAnimateSprite>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindExistingManager && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AdobeAnimateRuntimeManager>(FindExistingManager(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveEffectiveProcessMode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ProcessModeEnum>(ResolveEffectiveProcessMode(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.InvalidateRenderRoots)
		{
			return true;
		}
		if (method == MethodName.NotifyRenderRootVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.NotifyGpuRenderCachesCleared)
		{
			return true;
		}
		if (method == MethodName.NotifyViewportTransformChanged)
		{
			return true;
		}
		if (method == MethodName.RequestRenderRootRepublish)
		{
			return true;
		}
		if (method == MethodName.RecoverFromFailedRenderTransaction)
		{
			return true;
		}
		if (method == MethodName.NotifyRenderAncestorTransformChanged)
		{
			return true;
		}
		if (method == MethodName.NotifyRenderSubmission)
		{
			return true;
		}
		if (method == MethodName.NotifyDisplayRenderSubmission)
		{
			return true;
		}
		if (method == MethodName.NotifyPauseDispatchInvalidated)
		{
			return true;
		}
		if (method == MethodName.SetRenderStaticArrayAuditRegistration)
		{
			return true;
		}
		if (method == MethodName.UpdateRenderStaticArrayAuditRegistration)
		{
			return true;
		}
		if (method == MethodName.Register)
		{
			return true;
		}
		if (method == MethodName.Unregister)
		{
			return true;
		}
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
		if (method == MethodName.HandleAdobeAnimateRenderBackendChanged)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.ProcessRenderStaticArrayAudits)
		{
			return true;
		}
		if (method == MethodName.AuditRenderStaticArraySprite)
		{
			return true;
		}
		if (method == MethodName.ResolveRenderFrameRate)
		{
			return true;
		}
		if (method == MethodName.ResolveBattleCharacterCount)
		{
			return true;
		}
		if (method == MethodName.ResolveAdaptiveVisualTicksPerSecond)
		{
			return true;
		}
		if (method == MethodName.AdvanceVisualCadence)
		{
			return true;
		}
		if (method == MethodName.RefreshProcessEligibility)
		{
			return true;
		}
		if (method == MethodName.HasUsableInstance)
		{
			return true;
		}
		if (method == MethodName.TryMount)
		{
			return true;
		}
		if (method == MethodName.FindExistingManager)
		{
			return true;
		}
		if (method == MethodName.ProcessDisplayVisualCandidates)
		{
			return true;
		}
		if (method == MethodName.ProcessActive)
		{
			return true;
		}
		if (method == MethodName.MarkRuntimeRenderSubmissionsConsumed)
		{
			return true;
		}
		if (method == MethodName.InvalidateRuntimeCrowdCullingIfCachedOffscreen)
		{
			return true;
		}
		if (method == MethodName.RenderActive)
		{
			return true;
		}
		if (method == MethodName.RefreshRenderRootsIfNeeded)
		{
			return true;
		}
		if (method == MethodName.RefreshPauseDispatchState)
		{
			return true;
		}
		if (method == MethodName.ResolvePauseDispatch)
		{
			return true;
		}
		if (method == MethodName.ShouldDispatchInCurrentPauseState)
		{
			return true;
		}
		if (method == MethodName.ResolveInheritedProcessMode)
		{
			return true;
		}
		if (method == MethodName.ResolveEffectiveProcessMode)
		{
			return true;
		}
		if (method == MethodName.FlushPending)
		{
			return true;
		}
		if (method == MethodName.AddActive)
		{
			return true;
		}
		if (method == MethodName.RemoveActiveAt)
		{
			return true;
		}
		if (method == MethodName.SetAnimationClockForBareTest)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._runtimeTree)
		{
			_runtimeTree = VariantUtils.ConvertTo<SceneTree>(in value);
			return true;
		}
		if (name == PropertyName._displayTickVersion)
		{
			_displayTickVersion = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._dispatchTreePaused)
		{
			_dispatchTreePaused = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dispatchTreePausedInitialized)
		{
			_dispatchTreePausedInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pauseDispatchValidationCursor)
		{
			_pauseDispatchValidationCursor = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._renderRootsDirty)
		{
			_renderRootsDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._staticCrowdRefreshAccumulator)
		{
			_staticCrowdRefreshAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._animationClockSeconds)
		{
			_animationClockSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._animationLogicAccumulator)
		{
			_animationLogicAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._visualCadenceAccumulator)
		{
			_visualCadenceAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._displayVisualDeltaAccumulator)
		{
			_displayVisualDeltaAccumulator = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._currentVisualTicksPerSecond)
		{
			_currentVisualTicksPerSecond = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._currentBattleCharacterCount)
		{
			_currentBattleCharacterCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._displayRenderSubmissionPending)
		{
			_displayRenderSubmissionPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fixedRenderSubmissionPending)
		{
			_fixedRenderSubmissionPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._processingDisplayVisualCandidates)
		{
			_processingDisplayVisualCandidates = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._lastRenderPhysicsFrame)
		{
			_lastRenderPhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._lastRenderStaticArrayAuditPhysicsFrame)
		{
			_lastRenderStaticArrayAuditPhysicsFrame = VariantUtils.ConvertTo<ulong>(in value);
			return true;
		}
		if (name == PropertyName._processEligibilityInitialized)
		{
			_processEligibilityInitialized = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._processEligibilityEnabled)
		{
			_processEligibilityEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._subscribedGlobal)
		{
			_subscribedGlobal = VariantUtils.ConvertTo<Global>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.RegisteredSpriteCount)
		{
			from = RegisteredSpriteCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CurrentVisualTicksPerSecond)
		{
			value = VariantUtils.CreateFrom<double>(CurrentVisualTicksPerSecond);
			return true;
		}
		if (name == PropertyName.CurrentBattleCharacterCount)
		{
			from = CurrentBattleCharacterCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._runtimeTree)
		{
			value = VariantUtils.CreateFrom(in _runtimeTree);
			return true;
		}
		if (name == PropertyName._displayTickVersion)
		{
			value = VariantUtils.CreateFrom(in _displayTickVersion);
			return true;
		}
		if (name == PropertyName._dispatchTreePaused)
		{
			value = VariantUtils.CreateFrom(in _dispatchTreePaused);
			return true;
		}
		if (name == PropertyName._dispatchTreePausedInitialized)
		{
			value = VariantUtils.CreateFrom(in _dispatchTreePausedInitialized);
			return true;
		}
		if (name == PropertyName._pauseDispatchValidationCursor)
		{
			value = VariantUtils.CreateFrom(in _pauseDispatchValidationCursor);
			return true;
		}
		if (name == PropertyName._renderRootsDirty)
		{
			value = VariantUtils.CreateFrom(in _renderRootsDirty);
			return true;
		}
		if (name == PropertyName._staticCrowdRefreshAccumulator)
		{
			value = VariantUtils.CreateFrom(in _staticCrowdRefreshAccumulator);
			return true;
		}
		if (name == PropertyName._animationClockSeconds)
		{
			value = VariantUtils.CreateFrom(in _animationClockSeconds);
			return true;
		}
		if (name == PropertyName._animationLogicAccumulator)
		{
			value = VariantUtils.CreateFrom(in _animationLogicAccumulator);
			return true;
		}
		if (name == PropertyName._visualCadenceAccumulator)
		{
			value = VariantUtils.CreateFrom(in _visualCadenceAccumulator);
			return true;
		}
		if (name == PropertyName._displayVisualDeltaAccumulator)
		{
			value = VariantUtils.CreateFrom(in _displayVisualDeltaAccumulator);
			return true;
		}
		if (name == PropertyName._currentVisualTicksPerSecond)
		{
			value = VariantUtils.CreateFrom(in _currentVisualTicksPerSecond);
			return true;
		}
		if (name == PropertyName._currentBattleCharacterCount)
		{
			value = VariantUtils.CreateFrom(in _currentBattleCharacterCount);
			return true;
		}
		if (name == PropertyName._displayRenderSubmissionPending)
		{
			value = VariantUtils.CreateFrom(in _displayRenderSubmissionPending);
			return true;
		}
		if (name == PropertyName._fixedRenderSubmissionPending)
		{
			value = VariantUtils.CreateFrom(in _fixedRenderSubmissionPending);
			return true;
		}
		if (name == PropertyName._processingDisplayVisualCandidates)
		{
			value = VariantUtils.CreateFrom(in _processingDisplayVisualCandidates);
			return true;
		}
		if (name == PropertyName._lastRenderPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _lastRenderPhysicsFrame);
			return true;
		}
		if (name == PropertyName._lastRenderStaticArrayAuditPhysicsFrame)
		{
			value = VariantUtils.CreateFrom(in _lastRenderStaticArrayAuditPhysicsFrame);
			return true;
		}
		if (name == PropertyName._processEligibilityInitialized)
		{
			value = VariantUtils.CreateFrom(in _processEligibilityInitialized);
			return true;
		}
		if (name == PropertyName._processEligibilityEnabled)
		{
			value = VariantUtils.CreateFrom(in _processEligibilityEnabled);
			return true;
		}
		if (name == PropertyName._subscribedGlobal)
		{
			value = VariantUtils.CreateFrom(in _subscribedGlobal);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.RegisteredSpriteCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.CurrentVisualTicksPerSecond, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CurrentBattleCharacterCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._displayTickVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dispatchTreePaused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dispatchTreePausedInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pauseDispatchValidationCursor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._renderRootsDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._staticCrowdRefreshAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._animationClockSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._animationLogicAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._visualCadenceAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._displayVisualDeltaAccumulator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._currentVisualTicksPerSecond, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._currentBattleCharacterCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._displayRenderSubmissionPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._fixedRenderSubmissionPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._processingDisplayVisualCandidates, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastRenderPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._lastRenderStaticArrayAuditPhysicsFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._processEligibilityInitialized, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._processEligibilityEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._subscribedGlobal, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._runtimeTree, Variant.From(in _runtimeTree));
		info.AddProperty(PropertyName._displayTickVersion, Variant.From(in _displayTickVersion));
		info.AddProperty(PropertyName._dispatchTreePaused, Variant.From(in _dispatchTreePaused));
		info.AddProperty(PropertyName._dispatchTreePausedInitialized, Variant.From(in _dispatchTreePausedInitialized));
		info.AddProperty(PropertyName._pauseDispatchValidationCursor, Variant.From(in _pauseDispatchValidationCursor));
		info.AddProperty(PropertyName._renderRootsDirty, Variant.From(in _renderRootsDirty));
		info.AddProperty(PropertyName._staticCrowdRefreshAccumulator, Variant.From(in _staticCrowdRefreshAccumulator));
		info.AddProperty(PropertyName._animationClockSeconds, Variant.From(in _animationClockSeconds));
		info.AddProperty(PropertyName._animationLogicAccumulator, Variant.From(in _animationLogicAccumulator));
		info.AddProperty(PropertyName._visualCadenceAccumulator, Variant.From(in _visualCadenceAccumulator));
		info.AddProperty(PropertyName._displayVisualDeltaAccumulator, Variant.From(in _displayVisualDeltaAccumulator));
		info.AddProperty(PropertyName._currentVisualTicksPerSecond, Variant.From(in _currentVisualTicksPerSecond));
		info.AddProperty(PropertyName._currentBattleCharacterCount, Variant.From(in _currentBattleCharacterCount));
		info.AddProperty(PropertyName._displayRenderSubmissionPending, Variant.From(in _displayRenderSubmissionPending));
		info.AddProperty(PropertyName._fixedRenderSubmissionPending, Variant.From(in _fixedRenderSubmissionPending));
		info.AddProperty(PropertyName._processingDisplayVisualCandidates, Variant.From(in _processingDisplayVisualCandidates));
		info.AddProperty(PropertyName._lastRenderPhysicsFrame, Variant.From(in _lastRenderPhysicsFrame));
		info.AddProperty(PropertyName._lastRenderStaticArrayAuditPhysicsFrame, Variant.From(in _lastRenderStaticArrayAuditPhysicsFrame));
		info.AddProperty(PropertyName._processEligibilityInitialized, Variant.From(in _processEligibilityInitialized));
		info.AddProperty(PropertyName._processEligibilityEnabled, Variant.From(in _processEligibilityEnabled));
		info.AddProperty(PropertyName._subscribedGlobal, Variant.From(in _subscribedGlobal));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._runtimeTree, out var value))
		{
			_runtimeTree = value.As<SceneTree>();
		}
		if (info.TryGetProperty(PropertyName._displayTickVersion, out var value2))
		{
			_displayTickVersion = value2.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._dispatchTreePaused, out var value3))
		{
			_dispatchTreePaused = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dispatchTreePausedInitialized, out var value4))
		{
			_dispatchTreePausedInitialized = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pauseDispatchValidationCursor, out var value5))
		{
			_pauseDispatchValidationCursor = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._renderRootsDirty, out var value6))
		{
			_renderRootsDirty = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._staticCrowdRefreshAccumulator, out var value7))
		{
			_staticCrowdRefreshAccumulator = value7.As<double>();
		}
		if (info.TryGetProperty(PropertyName._animationClockSeconds, out var value8))
		{
			_animationClockSeconds = value8.As<double>();
		}
		if (info.TryGetProperty(PropertyName._animationLogicAccumulator, out var value9))
		{
			_animationLogicAccumulator = value9.As<double>();
		}
		if (info.TryGetProperty(PropertyName._visualCadenceAccumulator, out var value10))
		{
			_visualCadenceAccumulator = value10.As<double>();
		}
		if (info.TryGetProperty(PropertyName._displayVisualDeltaAccumulator, out var value11))
		{
			_displayVisualDeltaAccumulator = value11.As<double>();
		}
		if (info.TryGetProperty(PropertyName._currentVisualTicksPerSecond, out var value12))
		{
			_currentVisualTicksPerSecond = value12.As<double>();
		}
		if (info.TryGetProperty(PropertyName._currentBattleCharacterCount, out var value13))
		{
			_currentBattleCharacterCount = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName._displayRenderSubmissionPending, out var value14))
		{
			_displayRenderSubmissionPending = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fixedRenderSubmissionPending, out var value15))
		{
			_fixedRenderSubmissionPending = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._processingDisplayVisualCandidates, out var value16))
		{
			_processingDisplayVisualCandidates = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._lastRenderPhysicsFrame, out var value17))
		{
			_lastRenderPhysicsFrame = value17.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._lastRenderStaticArrayAuditPhysicsFrame, out var value18))
		{
			_lastRenderStaticArrayAuditPhysicsFrame = value18.As<ulong>();
		}
		if (info.TryGetProperty(PropertyName._processEligibilityInitialized, out var value19))
		{
			_processEligibilityInitialized = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._processEligibilityEnabled, out var value20))
		{
			_processEligibilityEnabled = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._subscribedGlobal, out var value21))
		{
			_subscribedGlobal = value21.As<Global>();
		}
	}
}
