using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://addons/godot_state_charts/VisualEditor/StateMachineSimulationPanel.cs")]
public class StateMachineSimulationPanel : PanelContainer
{
	private sealed class RuntimeTraceEntry
	{
		public string Kind { get; init; } = string.Empty;

		public string Summary { get; init; } = string.Empty;

		public string TargetStableId { get; init; } = string.Empty;

		public double ElapsedSeconds { get; init; }

		public StateMachineSnapshot Snapshot { get; init; }
	}

	private sealed class CompletedTransitionTrace
	{
		public string StableId { get; init; } = string.Empty;

		public string Summary { get; init; } = string.Empty;

		public string TargetStableId { get; init; } = string.Empty;

		public double ElapsedSeconds { get; init; }

		public StateMachineSnapshot Snapshot { get; init; }
	}

	private enum ExpressionValueType
	{
		Boolean,
		Integer,
		Number,
		Text
	}

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName BindDefinition = "BindDefinition";

		public static readonly StringName InvalidateRuntimeTarget = "InvalidateRuntimeTarget";

		public static readonly StringName SetWorkbenchActive = "SetWorkbenchActive";

		public static readonly StringName StartSimulation = "StartSimulation";

		public static readonly StringName SendSimulationEvent = "SendSimulationEvent";

		public static readonly StringName SetExpressionProperty = "SetExpressionProperty";

		public static readonly StringName AdvanceSimulation = "AdvanceSimulation";

		public static readonly StringName AdvanceAutoSimulation = "AdvanceAutoSimulation";

		public static readonly StringName PauseSimulation = "PauseSimulation";

		public static readonly StringName ResumeSimulation = "ResumeSimulation";

		public static readonly StringName StepSingleFrame = "StepSingleFrame";

		public static readonly StringName StepBack = "StepBack";

		public static readonly StringName ResetSimulation = "ResetSimulation";

		public static readonly StringName StopSimulation = "StopSimulation";

		public static readonly StringName BuildVisualWorkbench = "BuildVisualWorkbench";

		public static readonly StringName CreateIconButton = "CreateIconButton";

		public static readonly StringName BuildExpressionPropertyWorkbench = "BuildExpressionPropertyWorkbench";

		public static readonly StringName AddExpressionTypeSegment = "AddExpressionTypeSegment";

		public static readonly StringName SelectExpressionValueType = "SelectExpressionValueType";

		public static readonly StringName ApplyExpressionPropertyFromWorkbench = "ApplyExpressionPropertyFromWorkbench";

		public static readonly StringName RefreshExpressionValueEditor = "RefreshExpressionValueEditor";

		public static readonly StringName RefreshExpressionProperties = "RefreshExpressionProperties";

		public static readonly StringName SelectExpressionProperty = "SelectExpressionProperty";

		public static readonly StringName ExpressionTypeGlyph = "ExpressionTypeGlyph";

		public static readonly StringName FormatExpressionValue = "FormatExpressionValue";

		public static readonly StringName SendSelectedEvent = "SendSelectedEvent";

		public static readonly StringName EnsureRunning = "EnsureRunning";

		public static readonly StringName FlushCompletedTransitionTraceDeferred = "FlushCompletedTransitionTraceDeferred";

		public static readonly StringName FlushCompletedTransitionTrace = "FlushCompletedTransitionTrace";

		public static readonly StringName OnRuntimeDiagnostic = "OnRuntimeDiagnostic";

		public static readonly StringName CaptureAndPublish = "CaptureAndPublish";

		public static readonly StringName RefreshPending = "RefreshPending";

		public static readonly StringName ResolvePendingDelay = "ResolvePendingDelay";

		public static readonly StringName ResolvePendingTransitionId = "ResolvePendingTransitionId";

		public static readonly StringName IndexTransitions = "IndexTransitions";

		public static readonly StringName RefreshEventChoices = "RefreshEventChoices";

		public static readonly StringName FormatTransition = "FormatTransition";

		public static readonly StringName ResolveTransitionTarget = "ResolveTransitionTarget";

		public static readonly StringName RefreshHistoryList = "RefreshHistoryList";

		public static readonly StringName NavigateToHistoryItem = "NavigateToHistoryItem";

		public static readonly StringName RestoreRuntimeTrace = "RestoreRuntimeTrace";

		public static readonly StringName CloneSnapshot = "CloneSnapshot";

		public static readonly StringName ClearCompletedTransitionHighlight = "ClearCompletedTransitionHighlight";

		public static readonly StringName ResolvePrimaryActiveState = "ResolvePrimaryActiveState";

		public static readonly StringName ResolveTraceIcon = "ResolveTraceIcon";

		public static readonly StringName RefreshControls = "RefreshControls";

		public static readonly StringName ToggleSimulation = "ToggleSimulation";

		public static readonly StringName SetAutoAdvance = "SetAutoAdvance";

		public static readonly StringName StepOwnedController = "StepOwnedController";

		public static readonly StringName SetStatus = "SetStatus";

		public static readonly StringName UpdateProcessingEligibility = "UpdateProcessingEligibility";

		public static readonly StringName ConnectControllerSignals = "ConnectControllerSignals";

		public static readonly StringName ReleaseController = "ReleaseController";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName IsRunning = "IsRunning";

		public static readonly StringName AutoAdvance = "AutoAdvance";

		public static readonly StringName IsPaused = "IsPaused";

		public static readonly StringName RuntimeAttached = "RuntimeAttached";

		public static readonly StringName UsesBorrowedRuntime = "UsesBorrowedRuntime";

		public static readonly StringName RuntimeSourceName = "RuntimeSourceName";

		public static readonly StringName TransitionHistoryCount = "TransitionHistoryCount";

		public static readonly StringName RuntimeTraceCount = "RuntimeTraceCount";

		public static readonly StringName LastRuntimeTraceKind = "LastRuntimeTraceKind";

		public static readonly StringName CurrentSnapshotRevision = "CurrentSnapshotRevision";

		public static readonly StringName PendingDelayRemaining = "PendingDelayRemaining";

		public static readonly StringName CurrentSnapshot = "CurrentSnapshot";

		public static readonly StringName LastCompletedTransitionStableId = "LastCompletedTransitionStableId";

		public static readonly StringName IsQuiescent = "IsQuiescent";

		public static readonly StringName ExpressionPropertyCount = "ExpressionPropertyCount";

		public static readonly StringName SnapshotPublishCount = "SnapshotPublishCount";

		public static readonly StringName AutoSnapshotPublishCount = "AutoSnapshotPublishCount";

		public static readonly StringName _definition = "_definition";

		public static readonly StringName _lastSnapshot = "_lastSnapshot";

		public static readonly StringName _eventPicker = "_eventPicker";

		public static readonly StringName _sendEventButton = "_sendEventButton";

		public static readonly StringName _startButton = "_startButton";

		public static readonly StringName _resumeButton = "_resumeButton";

		public static readonly StringName _pauseButton = "_pauseButton";

		public static readonly StringName _stepButton = "_stepButton";

		public static readonly StringName _backButton = "_backButton";

		public static readonly StringName _resetButton = "_resetButton";

		public static readonly StringName _autoAdvance = "_autoAdvance";

		public static readonly StringName _status = "_status";

		public static readonly StringName _pendingLabel = "_pendingLabel";

		public static readonly StringName _pendingProgress = "_pendingProgress";

		public static readonly StringName _historyList = "_historyList";

		public static readonly StringName _expressionKey = "_expressionKey";

		public static readonly StringName _expressionType = "_expressionType";

		public static readonly StringName _expressionTypeSegments = "_expressionTypeSegments";

		public static readonly StringName _expressionValue = "_expressionValue";

		public static readonly StringName _expressionBoolean = "_expressionBoolean";

		public static readonly StringName _setExpressionButton = "_setExpressionButton";

		public static readonly StringName _expressionList = "_expressionList";

		public static readonly StringName _running = "_running";

		public static readonly StringName _ownsController = "_ownsController";

		public static readonly StringName _workbenchActive = "_workbenchActive";

		public static readonly StringName _completedTraceFlushQueued = "_completedTraceFlushQueued";

		public static readonly StringName _autoPublishElapsed = "_autoPublishElapsed";

		public static readonly StringName _elapsedSimulationSeconds = "_elapsedSimulationSeconds";

		public static readonly StringName _lastCompletedTransitionStableId = "_lastCompletedTransitionStableId";

		public static readonly StringName _lastCompletedTransitionHighlightRemaining = "_lastCompletedTransitionHighlightRemaining";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private const double AutoPublishIntervalSeconds = 0.1;

	private const double SingleFrameSeconds = 1.0 / 60.0;

	private const double CompletedTransitionHighlightSeconds = 1.2;

	private const int MaxRuntimeTraceEntries = 128;

	private static readonly System.Collections.Generic.Dictionary<string, Texture2D> TraceIcons = new System.Collections.Generic.Dictionary<string, Texture2D>(StringComparer.Ordinal);

	private readonly List<StateMachineSnapshot> _backStack = new List<StateMachineSnapshot>();

	private readonly List<string> _transitionHistory = new List<string>();

	private readonly List<string> _transitionTargets = new List<string>();

	private readonly List<RuntimeTraceEntry> _runtimeTrace = new List<RuntimeTraceEntry>();

	private readonly List<CompletedTransitionTrace> _completedTransitionQueue = new List<CompletedTransitionTrace>();

	private readonly System.Collections.Generic.Dictionary<string, StateMachineTransitionDefinition> _transitions = new System.Collections.Generic.Dictionary<string, StateMachineTransitionDefinition>(StringComparer.Ordinal);

	private StateMachineDefinition _definition;

	private IStateMachineController _controller;

	private StateMachineRuntimeDebugTarget _runtimeTarget;

	private StateMachineSnapshot _lastSnapshot;

	private OptionButton _eventPicker;

	private Button _sendEventButton;

	private Button _startButton;

	private Button _resumeButton;

	private Button _pauseButton;

	private Button _stepButton;

	private Button _backButton;

	private Button _resetButton;

	private CheckButton _autoAdvance;

	private Label _status;

	private Label _pendingLabel;

	private ProgressBar _pendingProgress;

	private ItemList _historyList;

	private LineEdit _expressionKey;

	private OptionButton _expressionType;

	private HBoxContainer _expressionTypeSegments;

	private readonly System.Collections.Generic.Dictionary<ExpressionValueType, Button> _expressionTypeButtons = new System.Collections.Generic.Dictionary<ExpressionValueType, Button>();

	private LineEdit _expressionValue;

	private CheckButton _expressionBoolean;

	private Button _setExpressionButton;

	private ItemList _expressionList;

	private readonly List<StringName> _expressionKeys = new List<StringName>();

	private bool _running;

	private bool _ownsController;

	private bool _workbenchActive;

	private bool _completedTraceFlushQueued;

	private double _autoPublishElapsed;

	private double _elapsedSimulationSeconds;

	private string _lastCompletedTransitionStableId = string.Empty;

	private double _lastCompletedTransitionHighlightRemaining;

	public bool IsRunning
	{
		get
		{
			if (_running)
			{
				return _controller?.IsInitialized ?? false;
			}
			return false;
		}
	}

	public bool AutoAdvance => _autoAdvance?.ButtonPressed ?? false;

	public bool IsPaused
	{
		get
		{
			if (IsRunning)
			{
				return !AutoAdvance;
			}
			return false;
		}
	}

	public bool RuntimeAttached => _controller?.IsInitialized ?? false;

	public bool UsesBorrowedRuntime
	{
		get
		{
			if (RuntimeAttached)
			{
				return !_ownsController;
			}
			return false;
		}
	}

	public IStateMachineController BoundRuntimeController => _controller;

	public string RuntimeSourceName => _runtimeTarget?.DisplayName ?? "隔离编译沙盒";

	public int TransitionHistoryCount => _transitionHistory.Count;

	public int RuntimeTraceCount => _runtimeTrace.Count;

	public string LastRuntimeTraceKind
	{
		get
		{
			if (_runtimeTrace.Count != 0)
			{
				List<RuntimeTraceEntry> runtimeTrace = _runtimeTrace;
				return runtimeTrace[runtimeTrace.Count - 1].Kind;
			}
			return string.Empty;
		}
	}

	public long CurrentSnapshotRevision => _lastSnapshot?.Revision ?? (-1);

	public double PendingDelayRemaining => ResolvePendingDelay(_lastSnapshot);

	public StateMachineSnapshot CurrentSnapshot => _lastSnapshot;

	public string LastCompletedTransitionStableId
	{
		get
		{
			if (!(_lastCompletedTransitionHighlightRemaining > 0.0))
			{
				return string.Empty;
			}
			return _lastCompletedTransitionStableId;
		}
	}

	public bool IsQuiescent
	{
		get
		{
			if (!IsRunning)
			{
				return !IsProcessing();
			}
			return false;
		}
	}

	public int ExpressionPropertyCount => (_lastSnapshot?.ExpressionProperties?.Count).GetValueOrDefault();

	public int SnapshotPublishCount { get; private set; }

	public int AutoSnapshotPublishCount { get; private set; }

	public event Action<StateMachineSnapshot> StateChanged;

	public event Action<bool> RunningChanged;

	public event Action<string> NavigateRequested;

	public override void _Ready()
	{
		BuildVisualWorkbench();
		_workbenchActive = IsVisibleInTree();
		SetProcess(enable: false);
		VisibilityChanged += UpdateProcessingEligibility;
		RefreshEventChoices();
		RefreshControls();
	}

	public override void _ExitTree()
	{
		StopSimulation();
		VisibilityChanged -= UpdateProcessingEligibility;
	}

	public override void _Process(double delta)
	{
		if (!IsRunning || !AutoAdvance || !IsVisibleInTree())
		{
			UpdateProcessingEligibility();
		}
		else
		{
			AdvanceAutoSimulation(Math.Min(Math.Max(delta, 0.0), 0.1));
		}
	}

	public void BindDefinition(StateMachineDefinition definition)
	{
		StopSimulation();
		_definition = definition;
		IndexTransitions();
		RefreshEventChoices();
		Label status = _status;
		string text;
		if (definition == null)
		{
			text = "未载入状态机";
		}
		else
		{
			text = ((_runtimeTarget == null) ? "沙盒待命 · 不会运行角色或关卡逻辑" : ("真实运行预览待命 · " + _runtimeTarget.DisplayName));
		}
		status.Text = text;
		RefreshControls();
	}

	public void BindRuntimeTarget(StateMachineRuntimeDebugTarget target)
	{
		if (_runtimeTarget != target)
		{
			StopSimulation();
			_runtimeTarget = target;
			if (_status != null)
			{
				Label status = _status;
				string text;
				if (target != null)
				{
					text = "真实运行预览待命 · " + target.DisplayName;
				}
				else
				{
					text = ((_definition == null) ? "未载入状态机" : "沙盒待命 · 不会运行角色或关卡逻辑");
				}
				status.Text = text;
			}
			RefreshControls();
		}
	}

	public void InvalidateRuntimeTarget(string reason)
	{
		if (_runtimeTarget != null)
		{
			StopSimulation();
			SetStatus(string.IsNullOrWhiteSpace(reason) ? "状态机配置已变化，重新启动时会重建真实预览。" : reason, error: false);
		}
	}

	public void SetWorkbenchActive(bool active)
	{
		_workbenchActive = active;
		if (!active)
		{
			StopSimulation();
		}
		else
		{
			UpdateProcessingEligibility();
		}
	}

	public bool StartSimulation()
	{
		StopSimulation();
		if (_definition == null)
		{
			SetStatus("没有可模拟的状态机", error: true);
			return false;
		}
		if (_runtimeTarget != null)
		{
			Func<bool> ensureCurrent = _runtimeTarget.EnsureCurrent;
			if (ensureCurrent == null || !ensureCurrent())
			{
				SetStatus("真实组件运行时无法重建，请先检查组件配置。", error: true);
				return false;
			}
			_controller = _runtimeTarget.ResolveController?.Invoke();
			_ownsController = false;
			IStateMachineController controller = _controller;
			if (controller == null || !controller.IsInitialized)
			{
				SetStatus("真实组件没有可调试的状态机控制器。", error: true);
				_controller = null;
				return false;
			}
		}
		else
		{
			StateMachineController stateMachineController = new StateMachineController();
			if (!stateMachineController.Initialize(_definition))
			{
				SetStatus("编译失败：" + stateMachineController.InitializationError, error: true);
				stateMachineController.Dispose();
				return false;
			}
			_controller = stateMachineController;
			_ownsController = true;
		}
		ConnectControllerSignals();
		_running = true;
		_autoPublishElapsed = 0.0;
		_elapsedSimulationSeconds = 0.0;
		_backStack.Clear();
		_transitionHistory.Clear();
		_transitionTargets.Clear();
		_runtimeTrace.Clear();
		_completedTransitionQueue.Clear();
		RefreshHistoryList();
		if (_ownsController && !_controller.EnterInitialState())
		{
			SetStatus("无法进入初始状态，请先检查入口配置", error: true);
			StopSimulation();
			return false;
		}
		CaptureAndPublish((_runtimeTarget == null) ? "模拟运行中" : ("已连接真实运行时：" + _runtimeTarget.DisplayName), "start", (_runtimeTarget == null) ? "进入沙盒初始状态" : "连接真实组件预览", recordTrace: true);
		RunningChanged?.Invoke(obj: true);
		UpdateProcessingEligibility();
		return true;
	}

	public bool SendSimulationEvent(StringName eventName)
	{
		if (!EnsureRunning() || eventName.IsEmpty)
		{
			return false;
		}
		bool flag = ((_runtimeTarget?.SendEvent != null) ? _runtimeTarget.SendEvent(eventName) : _controller.SendEvent(eventName));
		FlushCompletedTransitionTrace();
		CaptureAndPublish(flag ? $"已发送事件：{eventName}" : $"事件未接收：{eventName}", flag ? "event" : "blocked", flag ? $"事件 {eventName}" : $"拒绝事件 {eventName}", recordTrace: true);
		return flag;
	}

	public bool SetExpressionProperty(StringName key, Variant value)
	{
		if (!EnsureRunning() || key.IsEmpty || !StateMachineSnapshot.IsSupportedExpressionVariant(value.VariantType))
		{
			return false;
		}
		_controller.SetExpressionProperty(key, value);
		FlushCompletedTransitionTrace();
		CaptureAndPublish($"表达式属性：{key} = {FormatExpressionValue(value)}", "property", $"{key} = {FormatExpressionValue(value)}", recordTrace: true);
		return true;
	}

	public void AdvanceSimulation(double seconds)
	{
		if (EnsureRunning())
		{
			double num = Math.Max(0.0, seconds);
			SetAutoAdvance(enabled: false);
			if (!((_runtimeTarget?.Step != null) ? _runtimeTarget.Step(num) : StepOwnedController(num)))
			{
				SetStatus("真实组件运行时无法继续单步。", error: true);
				return;
			}
			_elapsedSimulationSeconds += num;
			_lastCompletedTransitionHighlightRemaining = Math.Max(0.0, _lastCompletedTransitionHighlightRemaining - num);
			_autoPublishElapsed = 0.0;
			FlushCompletedTransitionTrace();
			CaptureAndPublish((num > 0.0) ? $"时间推进 {num:0.###} 秒" : "刷新模拟状态", "step", (num > 0.0) ? $"单步 +{num:0.###} 秒" : "刷新状态", recordTrace: true);
		}
	}

	public void AdvanceAutoSimulation(double seconds)
	{
		if (!EnsureRunning())
		{
			return;
		}
		double num = Math.Max(0.0, seconds);
		if (!((_runtimeTarget?.Step != null) ? _runtimeTarget.Step(num) : StepOwnedController(num)))
		{
			PauseSimulation();
			SetStatus("真实组件运行时已经失效，自动计时已暂停。", error: true);
			return;
		}
		_elapsedSimulationSeconds += num;
		_lastCompletedTransitionHighlightRemaining = Math.Max(0.0, _lastCompletedTransitionHighlightRemaining - num);
		_autoPublishElapsed += num;
		if (!(_autoPublishElapsed + 1E-07 < 0.1))
		{
			_autoPublishElapsed = Math.Max(0.0, _autoPublishElapsed - 0.1);
			AutoSnapshotPublishCount++;
			bool recordTrace = _completedTransitionQueue.Count > 0;
			FlushCompletedTransitionTrace();
			CaptureAndPublish((_runtimeTarget == null) ? "模拟运行中" : ("真实运行中 · " + _runtimeTarget.DisplayName), "tick", "自动计时", recordTrace);
		}
	}

	public bool PauseSimulation()
	{
		if (!IsRunning)
		{
			return false;
		}
		SetAutoAdvance(enabled: false);
		AddRuntimeTrace("pause", "暂停自动计时", _lastSnapshot);
		SetStatus("已暂停 · " + RuntimeSourceName, error: false);
		RefreshControls();
		UpdateProcessingEligibility();
		return true;
	}

	public bool ResumeSimulation()
	{
		if (!EnsureRunning())
		{
			return false;
		}
		SetAutoAdvance(enabled: true);
		AddRuntimeTrace("resume", "继续自动计时", _lastSnapshot);
		SetStatus("运行中 · " + RuntimeSourceName, error: false);
		RefreshControls();
		UpdateProcessingEligibility();
		return true;
	}

	public void StepSingleFrame()
	{
		AdvanceSimulation(1.0 / 60.0);
	}

	public bool StepBack()
	{
		if (!IsRunning || _backStack.Count == 0)
		{
			return false;
		}
		List<StateMachineSnapshot> backStack = _backStack;
		StateMachineSnapshot snapshot = backStack[backStack.Count - 1];
		if (!_controller.RestoreSnapshot(snapshot, suppressEntryEffects: true))
		{
			SetStatus("无法回退：状态机结构已变化", error: true);
			return false;
		}
		_backStack.RemoveAt(_backStack.Count - 1);
		if (_transitionHistory.Count > 0)
		{
			_transitionHistory.RemoveAt(_transitionHistory.Count - 1);
		}
		if (_transitionTargets.Count > 0)
		{
			_transitionTargets.RemoveAt(_transitionTargets.Count - 1);
		}
		ClearCompletedTransitionHighlight();
		CaptureAndPublish("已回退到上一个转换前", "rewind", "回退到上一个转换前", recordTrace: true);
		return true;
	}

	public bool ResetSimulation()
	{
		if (_runtimeTarget == null)
		{
			return StartSimulation();
		}
		StopSimulation();
		Func<bool> reset = _runtimeTarget.Reset;
		if (reset == null || !reset())
		{
			SetStatus("真实组件预览重建失败。", error: true);
			return false;
		}
		return StartSimulation();
	}

	public void StopSimulation()
	{
		bool num = IsRunning || _running;
		_running = false;
		_autoPublishElapsed = 0.0;
		SetProcess(enable: false);
		ReleaseController();
		_backStack.Clear();
		_transitionHistory.Clear();
		_transitionTargets.Clear();
		_runtimeTrace.Clear();
		_completedTransitionQueue.Clear();
		_completedTraceFlushQueued = false;
		ClearCompletedTransitionHighlight();
		_lastSnapshot = null;
		RefreshHistoryList();
		RefreshPending(null);
		RefreshExpressionProperties(null);
		StateChanged?.Invoke(null);
		if (num)
		{
			RunningChanged?.Invoke(obj: false);
		}
		RefreshControls();
	}

	private void BuildVisualWorkbench()
	{
		Name = "SimulationPanel";
		CustomMinimumSize = new Vector2(340f, 344f);
		MouseFilter = MouseFilterEnum.Stop;
		MarginContainer marginContainer = new MarginContainer();
		marginContainer.AddThemeConstantOverride("margin_left", 12);
		marginContainer.AddThemeConstantOverride("margin_top", 10);
		marginContainer.AddThemeConstantOverride("margin_right", 12);
		marginContainer.AddThemeConstantOverride("margin_bottom", 10);
		AddChild(marginContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.AddThemeConstantOverride("separation", 8);
		marginContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer = new HBoxContainer();
		vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(new Label
		{
			Text = "状态机调试台",
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			TooltipText = "独立资源使用安全沙盒；组件工作台连接真实预览运行时"
		}, forceReadableName: false, InternalMode.Disabled);
		_startButton = CreateIconButton("SimulationStartButton", "res://addons/ModEditor/Icons/ClassIcon/PlayScene.svg", "启动调试运行时");
		_startButton.Pressed += ToggleSimulation;
		hBoxContainer.AddChild(_startButton, forceReadableName: false, InternalMode.Disabled);
		_resumeButton = CreateIconButton("SimulationResumeButton", "res://addons/ModEditor/Icons/ClassIcon/DebugContinue.svg", "继续自动计时");
		_resumeButton.Pressed += () =>
		{
			ResumeSimulation();
		};
		hBoxContainer.AddChild(_resumeButton, forceReadableName: false, InternalMode.Disabled);
		_pauseButton = CreateIconButton("SimulationPauseButton", "res://addons/ModEditor/Icons/ClassIcon/Pause.svg", "暂停并保留当前运行状态");
		_pauseButton.Pressed += () =>
		{
			PauseSimulation();
		};
		hBoxContainer.AddChild(_pauseButton, forceReadableName: false, InternalMode.Disabled);
		_stepButton = CreateIconButton("SimulationStepButton", "res://addons/ModEditor/Icons/ClassIcon/DebugStep.svg", "暂停状态下单步推进一帧");
		_stepButton.Pressed += StepSingleFrame;
		hBoxContainer.AddChild(_stepButton, forceReadableName: false, InternalMode.Disabled);
		_resetButton = CreateIconButton("SimulationResetButton", "res://addons/ModEditor/Icons/ClassIcon/ZoomReset.svg", "重建真实预览或重新进入沙盒初始状态");
		_resetButton.Pressed += () =>
		{
			ResetSimulation();
		};
		hBoxContainer.AddChild(_resetButton, forceReadableName: false, InternalMode.Disabled);
		_backButton = CreateIconButton("SimulationBackButton", "res://addons/ModEditor/Icons/ClassIcon/PlayBackwards.svg", "回到最近一次转换之前");
		_backButton.Pressed += () =>
		{
			StepBack();
		};
		hBoxContainer.AddChild(_backButton, forceReadableName: false, InternalMode.Disabled);
		HFlowContainer hFlowContainer = new HFlowContainer();
		vBoxContainer.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
		hFlowContainer.AddChild(new Label
		{
			Text = "事件"
		}, forceReadableName: false, InternalMode.Disabled);
		_eventPicker = new OptionButton
		{
			Name = "SimulationEventPicker",
			CustomMinimumSize = new Vector2(156f, 0f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			TooltipText = "来自事件转换的可用事件"
		};
		hFlowContainer.AddChild(_eventPicker, forceReadableName: false, InternalMode.Disabled);
		_sendEventButton = CreateIconButton("SimulationSendEventButton", "res://addons/ModEditor/Icons/ClassIcon/DebugNext.svg", "发送选中的状态事件");
		_sendEventButton.Pressed += SendSelectedEvent;
		hFlowContainer.AddChild(_sendEventButton, forceReadableName: false, InternalMode.Disabled);
		_autoAdvance = new CheckButton
		{
			Name = "SimulationAutoAdvanceSource",
			Text = "自动计时",
			ButtonPressed = false,
			Visible = false
		};
		_autoAdvance.Toggled += (bool _) =>
		{
			UpdateProcessingEligibility();
		};
		hFlowContainer.AddChild(_autoAdvance, forceReadableName: false, InternalMode.Disabled);
		BuildExpressionPropertyWorkbench(vBoxContainer);
		_status = new Label
		{
			Text = "沙盒待命",
			AutowrapMode = TextServer.AutowrapMode.WordSmart
		};
		vBoxContainer.AddChild(_status, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer2 = new HBoxContainer();
		vBoxContainer.AddChild(hBoxContainer2, forceReadableName: false, InternalMode.Disabled);
		_pendingLabel = new Label
		{
			Text = "延迟：无",
			CustomMinimumSize = new Vector2(146f, 0f)
		};
		hBoxContainer2.AddChild(_pendingLabel, forceReadableName: false, InternalMode.Disabled);
		_pendingProgress = new ProgressBar
		{
			MinValue = 0.0,
			MaxValue = 1.0,
			Value = 0.0,
			ShowPercentage = false,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hBoxContainer2.AddChild(_pendingProgress, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(new Label
		{
			Text = "运行时间线",
			TooltipText = "单击定位状态，双击把运行时恢复到该时刻；最多保留 128 步"
		}, forceReadableName: false, InternalMode.Disabled);
		_historyList = new ItemList
		{
			Name = "RuntimeTraceList",
			CustomMinimumSize = new Vector2(0f, 76f),
			SizeFlagsVertical = SizeFlags.ExpandFill,
			AllowReselect = true
		};
		_historyList.ItemSelected += (long index) =>
		{
			NavigateToHistoryItem((int)index);
		};
		_historyList.ItemActivated += (long index) =>
		{
			RestoreRuntimeTrace((int)index);
		};
		vBoxContainer.AddChild(_historyList, forceReadableName: false, InternalMode.Disabled);
	}

	private static Button CreateIconButton(string name, string iconPath, string tooltip)
	{
		return new Button
		{
			Name = name,
			Icon = ResourceLoader.Load<Texture2D>(iconPath, null, ResourceLoader.CacheMode.Reuse),
			TooltipText = tooltip,
			CustomMinimumSize = new Vector2(34f, 30f)
		};
	}

	private void BuildExpressionPropertyWorkbench(VBoxContainer stack)
	{
		stack.AddChild(new Label
		{
			Text = "守卫变量拼图 · 只影响当前沙盒",
			TooltipText = "设置 expressionProperties，立即重新评估自动转换与事件守卫"
		}, forceReadableName: false, InternalMode.Disabled);
		HFlowContainer hFlowContainer = new HFlowContainer
		{
			Name = "ExpressionPropertyWorkbench"
		};
		stack.AddChild(hFlowContainer, forceReadableName: false, InternalMode.Disabled);
		_expressionKey = new LineEdit
		{
			Name = "ExpressionPropertyKey",
			PlaceholderText = "属性键，例如 health",
			CustomMinimumSize = new Vector2(120f, 0f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hFlowContainer.AddChild(_expressionKey, forceReadableName: false, InternalMode.Disabled);
		_expressionType = new OptionButton
		{
			Name = "ExpressionPropertyType",
			TooltipText = "选择守卫值类型",
			CustomMinimumSize = new Vector2(92f, 0f),
			Visible = false
		};
		_expressionType.AddItem("◉ 开关", 0);
		_expressionType.AddItem("# 整数", 1);
		_expressionType.AddItem("0.0 数值", 2);
		_expressionType.AddItem("T 文本", 3);
		_expressionType.ItemSelected += (long _) =>
		{
			RefreshExpressionValueEditor();
		};
		hFlowContainer.AddChild(_expressionType, forceReadableName: false, InternalMode.Disabled);
		_expressionTypeSegments = new HBoxContainer
		{
			Name = "ExpressionPropertyTypeSegments"
		};
		hFlowContainer.AddChild(_expressionTypeSegments, forceReadableName: false, InternalMode.Disabled);
		AddExpressionTypeSegment(ExpressionValueType.Boolean, "ExpressionPropertyTypeBooleanButton", "◉", "开关");
		AddExpressionTypeSegment(ExpressionValueType.Integer, "ExpressionPropertyTypeIntegerButton", "#", "整数");
		AddExpressionTypeSegment(ExpressionValueType.Number, "ExpressionPropertyTypeNumberButton", "0.0", "数值");
		AddExpressionTypeSegment(ExpressionValueType.Text, "ExpressionPropertyTypeTextButton", "T", "文本");
		_expressionValue = new LineEdit
		{
			Name = "ExpressionPropertyValue",
			PlaceholderText = "值",
			CustomMinimumSize = new Vector2(86f, 0f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_expressionValue.TextSubmitted += (string _) =>
		{
			ApplyExpressionPropertyFromWorkbench();
		};
		hFlowContainer.AddChild(_expressionValue, forceReadableName: false, InternalMode.Disabled);
		_expressionBoolean = new CheckButton
		{
			Name = "ExpressionPropertyBoolean",
			Text = "开启",
			ButtonPressed = false
		};
		hFlowContainer.AddChild(_expressionBoolean, forceReadableName: false, InternalMode.Disabled);
		_setExpressionButton = new Button
		{
			Name = "SetExpressionPropertyButton",
			Text = "应用到沙盒",
			TooltipText = "调用 StateMachineController.SetExpressionProperty"
		};
		_setExpressionButton.Pressed += ApplyExpressionPropertyFromWorkbench;
		hFlowContainer.AddChild(_setExpressionButton, forceReadableName: false, InternalMode.Disabled);
		_expressionList = new ItemList
		{
			Name = "ExpressionPropertyList",
			CustomMinimumSize = new Vector2(0f, 54f),
			SelectMode = ItemList.SelectModeEnum.Single
		};
		_expressionList.ItemSelected += (long index) =>
		{
			SelectExpressionProperty((int)index);
		};
		stack.AddChild(_expressionList, forceReadableName: false, InternalMode.Disabled);
		RefreshExpressionValueEditor();
		RefreshExpressionProperties(null);
	}

	private void AddExpressionTypeSegment(ExpressionValueType valueType, string nodeName, string glyph, string tooltip)
	{
		Button button = new Button
		{
			Name = nodeName,
			Text = glyph,
			TooltipText = tooltip,
			ToggleMode = true,
			CustomMinimumSize = new Vector2(42f, 0f)
		};
		button.Pressed += () =>
		{
			SelectExpressionValueType(valueType);
		};
		_expressionTypeButtons[valueType] = button;
		_expressionTypeSegments.AddChild(button, forceReadableName: false, InternalMode.Disabled);
	}

	private void SelectExpressionValueType(ExpressionValueType valueType)
	{
		int num = _expressionType?.GetItemIndex((int)valueType) ?? (-1);
		if (num >= 0)
		{
			_expressionType.Select(num);
		}
		RefreshExpressionValueEditor();
	}

	private void ApplyExpressionPropertyFromWorkbench()
	{
		string text = _expressionKey?.Text?.StripEdges() ?? string.Empty;
		if (string.IsNullOrWhiteSpace(text))
		{
			SetStatus("表达式属性键不能为空", error: true);
			return;
		}
		ExpressionValueType selectedId = (ExpressionValueType)_expressionType.GetSelectedId();
		string from = _expressionValue?.Text ?? string.Empty;
		Variant value;
		switch (selectedId)
		{
		case ExpressionValueType.Boolean:
			value = Variant.From<bool>(_expressionBoolean.ButtonPressed);
			break;
		case ExpressionValueType.Integer:
		{
			if (!long.TryParse(from, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result2))
			{
				SetStatus("整数值格式不正确", error: true);
				return;
			}
			value = Variant.From(in result2);
			break;
		}
		case ExpressionValueType.Number:
		{
			if (!double.TryParse(from, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
			{
				SetStatus("数值格式不正确", error: true);
				return;
			}
			value = Variant.From(in result);
			break;
		}
		default:
			value = Variant.From(in from);
			break;
		}
		SetExpressionProperty(new StringName(text), value);
	}

	private void RefreshExpressionValueEditor()
	{
		if (_expressionType == null || _expressionValue == null || _expressionBoolean == null)
		{
			return;
		}
		bool flag = _expressionType.GetSelectedId() == 0;
		_expressionValue.Visible = !flag;
		_expressionBoolean.Visible = flag;
		ExpressionValueType selectedId = (ExpressionValueType)_expressionType.GetSelectedId();
		foreach (KeyValuePair<ExpressionValueType, Button> expressionTypeButton in _expressionTypeButtons)
		{
			expressionTypeButton.Value.ButtonPressed = expressionTypeButton.Key == selectedId;
		}
	}

	private void RefreshExpressionProperties(StateMachineSnapshot snapshot)
	{
		if (_expressionList == null)
		{
			return;
		}
		string text = ((_expressionList.GetSelectedItems().Length != 0 && _expressionList.GetSelectedItems()[0] < _expressionKeys.Count) ? _expressionKeys[_expressionList.GetSelectedItems()[0]].ToString() : (_expressionKey?.Text ?? string.Empty));
		_expressionList.Clear();
		_expressionKeys.Clear();
		List<StringName> list = new List<StringName>();
		if (snapshot?.ExpressionProperties != null)
		{
			foreach (StringName key in snapshot.ExpressionProperties.Keys)
			{
				list.Add(key);
			}
		}
		list.Sort((StringName left, StringName right) => string.Compare(left.ToString(), right.ToString(), StringComparison.Ordinal));
		int num = -1;
		foreach (StringName item in list)
		{
			Variant value = snapshot.ExpressionProperties[item];
			_expressionKeys.Add(item);
			_expressionList.AddItem($"{item}  ·  {ExpressionTypeGlyph(value)}  {FormatExpressionValue(value)}");
			if (item.ToString() == text)
			{
				num = _expressionKeys.Count - 1;
			}
		}
		if (_expressionKeys.Count == 0)
		{
			_expressionList.AddItem("尚未设置守卫变量");
			_expressionList.SetItemDisabled(0, disabled: true);
		}
		else if (num >= 0)
		{
			_expressionList.Select(num);
		}
	}

	private void SelectExpressionProperty(int index)
	{
		if (_lastSnapshot?.ExpressionProperties == null || (uint)index >= (uint)_expressionKeys.Count)
		{
			return;
		}
		StringName stringName = _expressionKeys[index];
		Variant value = _lastSnapshot.ExpressionProperties[stringName];
		_expressionKey.Text = stringName.ToString();
		Variant.Type variantType = value.VariantType;
		Variant.Type num = variantType - 1;
		if ((ulong)num > 2uL)
		{
			goto IL_0089;
		}
		switch ((int)num)
		{
		case 0:
			break;
		case 1:
			goto IL_0081;
		case 2:
			goto IL_0085;
		default:
			goto IL_0089;
		}
		ExpressionValueType expressionValueType = ExpressionValueType.Boolean;
		goto IL_008b;
		IL_008b:
		ExpressionValueType id = expressionValueType;
		_expressionType.Select(Math.Max(0, _expressionType.GetItemIndex((int)id)));
		_expressionBoolean.ButtonPressed = value.VariantType == Variant.Type.Bool && value.AsBool();
		_expressionValue.Text = FormatExpressionValue(value);
		RefreshExpressionValueEditor();
		return;
		IL_0089:
		expressionValueType = ExpressionValueType.Text;
		goto IL_008b;
		IL_0085:
		expressionValueType = ExpressionValueType.Number;
		goto IL_008b;
		IL_0081:
		expressionValueType = ExpressionValueType.Integer;
		goto IL_008b;
	}

	private static string ExpressionTypeGlyph(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		Variant.Type num = variantType - 1;
		if ((ulong)num <= 2uL)
		{
			switch ((int)num)
			{
			case 0:
				return "◉";
			case 1:
				return "#";
			case 2:
				return "0.0";
			}
		}
		return "T";
	}

	private static string FormatExpressionValue(Variant value)
	{
		Variant.Type variantType = value.VariantType;
		if ((ulong)variantType <= 3uL)
		{
			switch ((int)variantType)
			{
			case 1:
				return value.AsBool() ? "true" : "false";
			case 2:
				return value.AsInt64().ToString(CultureInfo.InvariantCulture);
			case 3:
				return value.AsDouble().ToString("0.###", CultureInfo.InvariantCulture);
			case 0:
				return "null";
			}
		}
		return value.AsString();
	}

	private void SendSelectedEvent()
	{
		if (_eventPicker != null && _eventPicker.ItemCount != 0)
		{
			SendSimulationEvent(new StringName(_eventPicker.GetItemText(_eventPicker.Selected)));
		}
	}

	private bool EnsureRunning()
	{
		if (!IsRunning)
		{
			return StartSimulation();
		}
		return true;
	}

	private void OnTransitionTaken(CompiledStateMachineTransition transition)
	{
		StateMachineSnapshot stateMachineSnapshot = _controller?.CaptureSnapshot();
		if (stateMachineSnapshot != null)
		{
			_backStack.Add(stateMachineSnapshot);
		}
		string item = FormatTransition(transition.StableId);
		_transitionHistory.Add(item);
		_transitionTargets.Add(ResolveTransitionTarget(transition.StableId));
	}

	private void OnTransitionCompleted(CompiledStateMachineTransition transition)
	{
		StateMachineSnapshot stateMachineSnapshot = _controller?.CaptureSnapshot();
		if (stateMachineSnapshot != null)
		{
			_completedTransitionQueue.Add(new CompletedTransitionTrace
			{
				StableId = (transition.StableId ?? string.Empty),
				Summary = FormatTransition(transition.StableId),
				TargetStableId = ResolveTransitionTarget(transition.StableId),
				ElapsedSeconds = _elapsedSimulationSeconds,
				Snapshot = CloneSnapshot(stateMachineSnapshot)
			});
			if (!_completedTraceFlushQueued)
			{
				_completedTraceFlushQueued = true;
				CallDeferred("FlushCompletedTransitionTraceDeferred");
			}
		}
	}

	private void FlushCompletedTransitionTraceDeferred()
	{
		_completedTraceFlushQueued = false;
		if (!IsRunning)
		{
			_completedTransitionQueue.Clear();
			return;
		}
		FlushCompletedTransitionTrace();
		StateMachineSnapshot stateMachineSnapshot = _controller?.CaptureSnapshot();
		if (stateMachineSnapshot != null)
		{
			_lastSnapshot = stateMachineSnapshot;
			RefreshPending(stateMachineSnapshot);
			RefreshExpressionProperties(stateMachineSnapshot);
			RefreshControls();
			StateChanged?.Invoke(stateMachineSnapshot);
		}
	}

	private void FlushCompletedTransitionTrace()
	{
		if (_completedTransitionQueue.Count == 0)
		{
			return;
		}
		foreach (CompletedTransitionTrace item in _completedTransitionQueue)
		{
			_lastCompletedTransitionStableId = item.StableId;
			_lastCompletedTransitionHighlightRemaining = 1.2;
			AddRuntimeTrace("transition", item.Summary, item.Snapshot, item.TargetStableId, item.ElapsedSeconds);
		}
		_completedTransitionQueue.Clear();
	}

	private void OnRuntimeDiagnostic(string code)
	{
		if (!string.IsNullOrWhiteSpace(code))
		{
			SetStatus("运行诊断：" + code, error: true);
		}
	}

	private void CaptureAndPublish(string status, string traceKind, string traceSummary, bool recordTrace)
	{
		SnapshotPublishCount++;
		_lastSnapshot = _controller?.CaptureSnapshot();
		if (recordTrace)
		{
			AddRuntimeTrace(traceKind, traceSummary, _lastSnapshot);
		}
		SetStatus(status, error: false);
		RefreshPending(_lastSnapshot);
		RefreshExpressionProperties(_lastSnapshot);
		RefreshControls();
		StateChanged?.Invoke(_lastSnapshot);
	}

	private void RefreshPending(StateMachineSnapshot snapshot)
	{
		double num = ResolvePendingDelay(snapshot);
		string text = ResolvePendingTransitionId(snapshot, num);
		if (num <= 0.0 || string.IsNullOrWhiteSpace(text))
		{
			_pendingLabel.Text = "延迟：无";
			_pendingProgress.MaxValue = 1.0;
			_pendingProgress.Value = 0.0;
			return;
		}
		double num2 = (_transitions.TryGetValue(text, out var value) ? Math.Max(num, value.DelaySeconds) : num);
		_pendingLabel.Text = $"延迟：{num:0.###} 秒";
		_pendingProgress.MaxValue = Math.Max(num2, 0.001);
		_pendingProgress.Value = Math.Clamp(num2 - num, 0.0, num2);
	}

	private static double ResolvePendingDelay(StateMachineSnapshot snapshot)
	{
		if (snapshot?.PendingDelayRemaining == null || snapshot.PendingDelayRemaining.Count == 0)
		{
			return 0.0;
		}
		double num = 1.0 / 0.0;
		foreach (string key in snapshot.PendingDelayRemaining.Keys)
		{
			num = Math.Min(num, snapshot.PendingDelayRemaining[key]);
		}
		if (!double.IsPositiveInfinity(num))
		{
			return Math.Max(0.0, num);
		}
		return 0.0;
	}

	private static string ResolvePendingTransitionId(StateMachineSnapshot snapshot, double remaining)
	{
		if (snapshot?.PendingDelayRemaining == null)
		{
			return string.Empty;
		}
		foreach (string key in snapshot.PendingDelayRemaining.Keys)
		{
			if (Math.Abs(snapshot.PendingDelayRemaining[key] - remaining) < 1E-06)
			{
				return key;
			}
		}
		return string.Empty;
	}

	private void IndexTransitions()
	{
		_transitions.Clear();
		IndexTransitionsRecursive(_definition, new HashSet<ulong>());
	}

	private void IndexTransitionsRecursive(StateMachineDefinition definition, HashSet<ulong> visited)
	{
		if (definition == null || !visited.Add(definition.GetInstanceId()))
		{
			return;
		}
		IndexTransitionsRecursive(definition.BaseDefinition, visited);
		foreach (StateMachineTransitionDefinition transition in definition.Transitions)
		{
			if (transition != null && !string.IsNullOrWhiteSpace(transition.StableId))
			{
				_transitions[transition.StableId] = transition;
			}
		}
	}

	private void RefreshEventChoices()
	{
		if (_eventPicker == null)
		{
			return;
		}
		string text = ((_eventPicker.ItemCount > 0) ? _eventPicker.GetItemText(_eventPicker.Selected) : string.Empty);
		_eventPicker.Clear();
		SortedSet<string> sortedSet = new SortedSet<string>(StringComparer.Ordinal);
		foreach (StateMachineTransitionDefinition value in _transitions.Values)
		{
			if (value.TriggerKind == StateMachineTriggerKind.Event && !value.EventName.IsEmpty)
			{
				sortedSet.Add(value.EventName.ToString());
			}
		}
		int idx = 0;
		foreach (string item in sortedSet)
		{
			_eventPicker.AddItem(item);
			if (item == text)
			{
				idx = _eventPicker.ItemCount - 1;
			}
		}
		if (_eventPicker.ItemCount > 0)
		{
			_eventPicker.Select(idx);
		}
		RefreshControls();
	}

	private string FormatTransition(string stableId)
	{
		if (!_transitions.TryGetValue(stableId ?? string.Empty, out var value))
		{
			return stableId ?? "转换";
		}
		string value2 = value.TriggerKind switch
		{
			StateMachineTriggerKind.Event => value.EventName.IsEmpty ? "事件" : value.EventName.ToString(), 
			StateMachineTriggerKind.Automatic => "自动", 
			StateMachineTriggerKind.Delay => $"延迟 {value.DelaySeconds:0.###}s", 
			_ => value.TriggerKind.ToString(), 
		};
		return $"{value.SourceStateId}  →  {value.TargetStateId}  [{value2}]";
	}

	private string ResolveTransitionTarget(string stableId)
	{
		if (!_transitions.TryGetValue(stableId ?? string.Empty, out var value))
		{
			return string.Empty;
		}
		return value.TargetStateId ?? string.Empty;
	}

	private void RefreshHistoryList()
	{
		if (_historyList == null)
		{
			return;
		}
		_historyList.Clear();
		if (_runtimeTrace.Count == 0)
		{
			_historyList.AddItem("尚无运行轨迹");
			_historyList.SetItemDisabled(0, disabled: true);
			return;
		}
		for (int i = 0; i < _runtimeTrace.Count; i++)
		{
			AppendRuntimeTraceItem(_runtimeTrace[i], i, select: false);
		}
		_historyList.Select(_runtimeTrace.Count - 1);
		_historyList.EnsureCurrentIsVisible();
	}

	private void AppendRuntimeTraceItem(RuntimeTraceEntry entry, int index, bool select)
	{
		if (_historyList != null && entry != null)
		{
			string text = ResolvePrimaryActiveState(entry.Snapshot);
			string value = (string.IsNullOrWhiteSpace(text) ? string.Empty : (" · " + text));
			_historyList.AddItem($"{index + 1:00}  {entry.ElapsedSeconds,6:0.000}s  {entry.Summary}{value}", ResolveTraceIcon(entry.Kind));
			int idx = _historyList.ItemCount - 1;
			_historyList.SetItemTooltip(idx, $"类型：{entry.Kind}\n快照版本：{entry.Snapshot?.Revision ?? (-1)}\n双击恢复到这一时刻");
			if (select)
			{
				_historyList.Select(idx);
				_historyList.EnsureCurrentIsVisible();
			}
		}
	}

	private void NavigateToHistoryItem(int index)
	{
		if ((uint)index < (uint)_runtimeTrace.Count)
		{
			string text = _runtimeTrace[index].TargetStableId;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = ResolvePrimaryActiveState(_runtimeTrace[index].Snapshot);
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				NavigateRequested?.Invoke(text);
			}
		}
	}

	public bool RestoreRuntimeTrace(int index)
	{
		if (!IsRunning || (uint)index >= (uint)_runtimeTrace.Count)
		{
			return false;
		}
		RuntimeTraceEntry runtimeTraceEntry = _runtimeTrace[index];
		if (runtimeTraceEntry.Snapshot == null || !_controller.RestoreSnapshot(runtimeTraceEntry.Snapshot, suppressEntryEffects: true))
		{
			SetStatus("无法恢复该轨迹：运行时结构已经变化。", error: true);
			return false;
		}
		SetAutoAdvance(enabled: false);
		_elapsedSimulationSeconds = runtimeTraceEntry.ElapsedSeconds;
		_backStack.Clear();
		_transitionHistory.Clear();
		_transitionTargets.Clear();
		_completedTransitionQueue.Clear();
		ClearCompletedTransitionHighlight();
		CaptureAndPublish($"已恢复到轨迹 {index + 1}", "rewind", $"恢复轨迹 {index + 1}", recordTrace: true);
		return true;
	}

	private void AddRuntimeTrace(string kind, string summary, StateMachineSnapshot snapshot, string targetStableId = null, double? elapsedSeconds = null)
	{
		if (snapshot == null)
		{
			return;
		}
		RuntimeTraceEntry runtimeTraceEntry = new RuntimeTraceEntry
		{
			Kind = (kind ?? string.Empty),
			Summary = (summary ?? string.Empty),
			TargetStableId = (string.IsNullOrWhiteSpace(targetStableId) ? ResolvePrimaryActiveState(snapshot) : targetStableId),
			ElapsedSeconds = (elapsedSeconds ?? _elapsedSimulationSeconds),
			Snapshot = CloneSnapshot(snapshot)
		};
		_runtimeTrace.Add(runtimeTraceEntry);
		bool flag = false;
		if (_runtimeTrace.Count > 128)
		{
			_runtimeTrace.RemoveAt(0);
			flag = true;
		}
		if (flag)
		{
			RefreshHistoryList();
			return;
		}
		if (_runtimeTrace.Count == 1)
		{
			_historyList?.Clear();
		}
		AppendRuntimeTraceItem(runtimeTraceEntry, _runtimeTrace.Count - 1, select: true);
	}

	private static StateMachineSnapshot CloneSnapshot(StateMachineSnapshot snapshot)
	{
		return (snapshot?.Duplicate(deep: true) as StateMachineSnapshot) ?? snapshot;
	}

	private void ClearCompletedTransitionHighlight()
	{
		_lastCompletedTransitionStableId = string.Empty;
		_lastCompletedTransitionHighlightRemaining = 0.0;
	}

	private static string ResolvePrimaryActiveState(StateMachineSnapshot snapshot)
	{
		if (snapshot?.ActiveStateIds == null || snapshot.ActiveStateIds.Count == 0)
		{
			return string.Empty;
		}
		Array<string> activeStateIds = snapshot.ActiveStateIds;
		return activeStateIds[activeStateIds.Count - 1] ?? string.Empty;
	}

	private static Texture2D ResolveTraceIcon(string kind)
	{
		string text = kind switch
		{
			"start" => "PlayScene", 
			"transition" => "DebugNext", 
			"event" => "KeyNext", 
			"blocked" => "DebugSkipBreakpointsOn", 
			"property" => "Edit", 
			"pause" => "Pause", 
			"resume" => "DebugContinue", 
			"step" => "DebugStep", 
			"rewind" => "PlayBackwards", 
			_ => "Time", 
		};
		if (!TraceIcons.TryGetValue(text, out var value) || !GodotObject.IsInstanceValid(value))
		{
			value = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ClassIcon/" + text + ".svg", null, ResourceLoader.CacheMode.Reuse);
			TraceIcons[text] = value;
		}
		return value;
	}

	private void RefreshControls()
	{
		if (_startButton == null)
		{
			return;
		}
		_startButton.Icon = ResourceLoader.Load<Texture2D>(IsRunning ? "res://addons/ModEditor/Icons/ClassIcon/Stop.svg" : "res://addons/ModEditor/Icons/ClassIcon/PlayScene.svg", null, ResourceLoader.CacheMode.Reuse);
		_startButton.TooltipText = (IsRunning ? "结束当前调试运行时" : "启动调试运行时");
		_startButton.Disabled = _definition == null;
		_resetButton.Disabled = _definition == null;
		_resumeButton.Disabled = !IsRunning || AutoAdvance;
		_pauseButton.Disabled = !IsRunning || !AutoAdvance;
		_stepButton.Disabled = !IsRunning;
		_backButton.Disabled = !IsRunning || _backStack.Count == 0;
		_sendEventButton.Disabled = _eventPicker.ItemCount == 0 || _definition == null;
		bool flag = !IsRunning;
		if (_expressionKey != null)
		{
			_expressionKey.Editable = !flag;
		}
		if (_expressionType != null)
		{
			_expressionType.Disabled = flag;
		}
		foreach (Button value in _expressionTypeButtons.Values)
		{
			value.Disabled = flag;
		}
		if (_expressionValue != null)
		{
			_expressionValue.Editable = !flag;
		}
		if (_expressionBoolean != null)
		{
			_expressionBoolean.Disabled = flag;
		}
		if (_setExpressionButton != null)
		{
			_setExpressionButton.Disabled = flag;
		}
	}

	private void ToggleSimulation()
	{
		if (IsRunning)
		{
			StopSimulation();
		}
		else
		{
			StartSimulation();
		}
	}

	private void SetAutoAdvance(bool enabled)
	{
		if (_autoAdvance != null)
		{
			_autoAdvance.SetPressedNoSignal(enabled);
			UpdateProcessingEligibility();
		}
	}

	private bool StepOwnedController(double seconds)
	{
		IStateMachineController controller = _controller;
		if (controller == null || !controller.IsInitialized)
		{
			return false;
		}
		_controller.TickProcess(seconds);
		_controller.TickPhysics(seconds);
		return true;
	}

	private void SetStatus(string text, bool error)
	{
		if (_status != null)
		{
			_status.Text = text ?? string.Empty;
			_status.Modulate = (error ? new Color(1f, 0.46f, 0.4f) : new Color(0.72f, 0.9f, 0.78f));
		}
	}

	private void UpdateProcessingEligibility()
	{
		SetProcess(_workbenchActive && IsRunning && AutoAdvance && IsVisibleInTree());
	}

	private void ConnectControllerSignals()
	{
		if (_controller != null)
		{
			_controller.TransitionTaken += OnTransitionTaken;
			_controller.TransitionCompleted += OnTransitionCompleted;
			_controller.Diagnostic += OnRuntimeDiagnostic;
		}
	}

	private void ReleaseController()
	{
		if (_controller != null)
		{
			_controller.TransitionTaken -= OnTransitionTaken;
			_controller.TransitionCompleted -= OnTransitionCompleted;
			_controller.Diagnostic -= OnRuntimeDiagnostic;
			if (_ownsController)
			{
				_controller.Dispose();
			}
			_controller = null;
			_ownsController = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(56)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindDefinition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.InvalidateRuntimeTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetWorkbenchActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartSimulation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendSimulationEvent, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetExpressionProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.AdvanceSimulation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "seconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AdvanceAutoSimulation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "seconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PauseSimulation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResumeSimulation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StepSingleFrame, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StepBack, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetSimulation, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopSimulation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildVisualWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateIconButton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "iconPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildExpressionPropertyWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "stack", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddExpressionTypeSegment, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "valueType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "glyph", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectExpressionValueType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "valueType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyExpressionPropertyFromWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshExpressionValueEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshExpressionProperties, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SelectExpressionProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ExpressionTypeGlyph, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatExpressionValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.SendSelectedEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureRunning, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlushCompletedTransitionTraceDeferred, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlushCompletedTransitionTrace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRuntimeDiagnostic, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "code", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureAndPublish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "status", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "traceKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "traceSummary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "recordTrace", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPending, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolvePendingDelay, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolvePendingTransitionId, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "remaining", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IndexTransitions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshEventChoices, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FormatTransition, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveTransitionTarget, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshHistoryList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NavigateToHistoryItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RestoreRuntimeTrace, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneSnapshot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearCompletedTransitionHighlight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolvePrimaryActiveState, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveTraceIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToggleSimulation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetAutoAdvance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StepOwnedController, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "seconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "error", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateProcessingEligibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectControllerSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseController, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindDefinition && args.Count == 1)
		{
			BindDefinition(VariantUtils.ConvertTo<StateMachineDefinition>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvalidateRuntimeTarget && args.Count == 1)
		{
			InvalidateRuntimeTarget(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetWorkbenchActive && args.Count == 1)
		{
			SetWorkbenchActive(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartSimulation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(StartSimulation());
			return true;
		}
		if (method == MethodName.SendSimulationEvent && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SendSimulationEvent(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.SetExpressionProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetExpressionProperty(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.AdvanceSimulation && args.Count == 1)
		{
			AdvanceSimulation(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AdvanceAutoSimulation && args.Count == 1)
		{
			AdvanceAutoSimulation(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PauseSimulation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(PauseSimulation());
			return true;
		}
		if (method == MethodName.ResumeSimulation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ResumeSimulation());
			return true;
		}
		if (method == MethodName.StepSingleFrame && args.Count == 0)
		{
			StepSingleFrame();
			ret = default;
			return true;
		}
		if (method == MethodName.StepBack && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(StepBack());
			return true;
		}
		if (method == MethodName.ResetSimulation && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ResetSimulation());
			return true;
		}
		if (method == MethodName.StopSimulation && args.Count == 0)
		{
			StopSimulation();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildVisualWorkbench && args.Count == 0)
		{
			BuildVisualWorkbench();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateIconButton && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Button>(CreateIconButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.BuildExpressionPropertyWorkbench && args.Count == 1)
		{
			BuildExpressionPropertyWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddExpressionTypeSegment && args.Count == 4)
		{
			AddExpressionTypeSegment(VariantUtils.ConvertTo<ExpressionValueType>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectExpressionValueType && args.Count == 1)
		{
			SelectExpressionValueType(VariantUtils.ConvertTo<ExpressionValueType>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyExpressionPropertyFromWorkbench && args.Count == 0)
		{
			ApplyExpressionPropertyFromWorkbench();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshExpressionValueEditor && args.Count == 0)
		{
			RefreshExpressionValueEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshExpressionProperties && args.Count == 1)
		{
			RefreshExpressionProperties(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectExpressionProperty && args.Count == 1)
		{
			SelectExpressionProperty(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ExpressionTypeGlyph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ExpressionTypeGlyph(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatExpressionValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatExpressionValue(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.SendSelectedEvent && args.Count == 0)
		{
			SendSelectedEvent();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureRunning && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureRunning());
			return true;
		}
		if (method == MethodName.FlushCompletedTransitionTraceDeferred && args.Count == 0)
		{
			FlushCompletedTransitionTraceDeferred();
			ret = default;
			return true;
		}
		if (method == MethodName.FlushCompletedTransitionTrace && args.Count == 0)
		{
			FlushCompletedTransitionTrace();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRuntimeDiagnostic && args.Count == 1)
		{
			OnRuntimeDiagnostic(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureAndPublish && args.Count == 4)
		{
			CaptureAndPublish(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPending && args.Count == 1)
		{
			RefreshPending(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePendingDelay && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ResolvePendingDelay(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolvePendingTransitionId && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ResolvePendingTransitionId(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.IndexTransitions && args.Count == 0)
		{
			IndexTransitions();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEventChoices && args.Count == 0)
		{
			RefreshEventChoices();
			ret = default;
			return true;
		}
		if (method == MethodName.FormatTransition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatTransition(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveTransitionTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveTransitionTarget(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshHistoryList && args.Count == 0)
		{
			RefreshHistoryList();
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateToHistoryItem && args.Count == 1)
		{
			NavigateToHistoryItem(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreRuntimeTrace && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RestoreRuntimeTrace(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.CloneSnapshot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineSnapshot>(CloneSnapshot(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearCompletedTransitionHighlight && args.Count == 0)
		{
			ClearCompletedTransitionHighlight();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolvePrimaryActiveState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolvePrimaryActiveState(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveTraceIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(ResolveTraceIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshControls && args.Count == 0)
		{
			RefreshControls();
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleSimulation && args.Count == 0)
		{
			ToggleSimulation();
			ret = default;
			return true;
		}
		if (method == MethodName.SetAutoAdvance && args.Count == 1)
		{
			SetAutoAdvance(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StepOwnedController && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(StepOwnedController(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.SetStatus && args.Count == 2)
		{
			SetStatus(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateProcessingEligibility && args.Count == 0)
		{
			UpdateProcessingEligibility();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectControllerSignals && args.Count == 0)
		{
			ConnectControllerSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseController && args.Count == 0)
		{
			ReleaseController();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateIconButton && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Button>(CreateIconButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.ExpressionTypeGlyph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ExpressionTypeGlyph(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatExpressionValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatExpressionValue(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolvePendingDelay && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<double>(ResolvePendingDelay(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolvePendingTransitionId && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ResolvePendingTransitionId(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.CloneSnapshot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineSnapshot>(CloneSnapshot(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolvePrimaryActiveState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolvePrimaryActiveState(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveTraceIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(ResolveTraceIcon(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.BindDefinition)
		{
			return true;
		}
		if (method == MethodName.InvalidateRuntimeTarget)
		{
			return true;
		}
		if (method == MethodName.SetWorkbenchActive)
		{
			return true;
		}
		if (method == MethodName.StartSimulation)
		{
			return true;
		}
		if (method == MethodName.SendSimulationEvent)
		{
			return true;
		}
		if (method == MethodName.SetExpressionProperty)
		{
			return true;
		}
		if (method == MethodName.AdvanceSimulation)
		{
			return true;
		}
		if (method == MethodName.AdvanceAutoSimulation)
		{
			return true;
		}
		if (method == MethodName.PauseSimulation)
		{
			return true;
		}
		if (method == MethodName.ResumeSimulation)
		{
			return true;
		}
		if (method == MethodName.StepSingleFrame)
		{
			return true;
		}
		if (method == MethodName.StepBack)
		{
			return true;
		}
		if (method == MethodName.ResetSimulation)
		{
			return true;
		}
		if (method == MethodName.StopSimulation)
		{
			return true;
		}
		if (method == MethodName.BuildVisualWorkbench)
		{
			return true;
		}
		if (method == MethodName.CreateIconButton)
		{
			return true;
		}
		if (method == MethodName.BuildExpressionPropertyWorkbench)
		{
			return true;
		}
		if (method == MethodName.AddExpressionTypeSegment)
		{
			return true;
		}
		if (method == MethodName.SelectExpressionValueType)
		{
			return true;
		}
		if (method == MethodName.ApplyExpressionPropertyFromWorkbench)
		{
			return true;
		}
		if (method == MethodName.RefreshExpressionValueEditor)
		{
			return true;
		}
		if (method == MethodName.RefreshExpressionProperties)
		{
			return true;
		}
		if (method == MethodName.SelectExpressionProperty)
		{
			return true;
		}
		if (method == MethodName.ExpressionTypeGlyph)
		{
			return true;
		}
		if (method == MethodName.FormatExpressionValue)
		{
			return true;
		}
		if (method == MethodName.SendSelectedEvent)
		{
			return true;
		}
		if (method == MethodName.EnsureRunning)
		{
			return true;
		}
		if (method == MethodName.FlushCompletedTransitionTraceDeferred)
		{
			return true;
		}
		if (method == MethodName.FlushCompletedTransitionTrace)
		{
			return true;
		}
		if (method == MethodName.OnRuntimeDiagnostic)
		{
			return true;
		}
		if (method == MethodName.CaptureAndPublish)
		{
			return true;
		}
		if (method == MethodName.RefreshPending)
		{
			return true;
		}
		if (method == MethodName.ResolvePendingDelay)
		{
			return true;
		}
		if (method == MethodName.ResolvePendingTransitionId)
		{
			return true;
		}
		if (method == MethodName.IndexTransitions)
		{
			return true;
		}
		if (method == MethodName.RefreshEventChoices)
		{
			return true;
		}
		if (method == MethodName.FormatTransition)
		{
			return true;
		}
		if (method == MethodName.ResolveTransitionTarget)
		{
			return true;
		}
		if (method == MethodName.RefreshHistoryList)
		{
			return true;
		}
		if (method == MethodName.NavigateToHistoryItem)
		{
			return true;
		}
		if (method == MethodName.RestoreRuntimeTrace)
		{
			return true;
		}
		if (method == MethodName.CloneSnapshot)
		{
			return true;
		}
		if (method == MethodName.ClearCompletedTransitionHighlight)
		{
			return true;
		}
		if (method == MethodName.ResolvePrimaryActiveState)
		{
			return true;
		}
		if (method == MethodName.ResolveTraceIcon)
		{
			return true;
		}
		if (method == MethodName.RefreshControls)
		{
			return true;
		}
		if (method == MethodName.ToggleSimulation)
		{
			return true;
		}
		if (method == MethodName.SetAutoAdvance)
		{
			return true;
		}
		if (method == MethodName.StepOwnedController)
		{
			return true;
		}
		if (method == MethodName.SetStatus)
		{
			return true;
		}
		if (method == MethodName.UpdateProcessingEligibility)
		{
			return true;
		}
		if (method == MethodName.ConnectControllerSignals)
		{
			return true;
		}
		if (method == MethodName.ReleaseController)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.SnapshotPublishCount)
		{
			SnapshotPublishCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.AutoSnapshotPublishCount)
		{
			AutoSnapshotPublishCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._definition)
		{
			_definition = VariantUtils.ConvertTo<StateMachineDefinition>(in value);
			return true;
		}
		if (name == PropertyName._lastSnapshot)
		{
			_lastSnapshot = VariantUtils.ConvertTo<StateMachineSnapshot>(in value);
			return true;
		}
		if (name == PropertyName._eventPicker)
		{
			_eventPicker = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._sendEventButton)
		{
			_sendEventButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._startButton)
		{
			_startButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._resumeButton)
		{
			_resumeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._pauseButton)
		{
			_pauseButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._stepButton)
		{
			_stepButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._backButton)
		{
			_backButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._resetButton)
		{
			_resetButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._autoAdvance)
		{
			_autoAdvance = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._status)
		{
			_status = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pendingLabel)
		{
			_pendingLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pendingProgress)
		{
			_pendingProgress = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._historyList)
		{
			_historyList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._expressionKey)
		{
			_expressionKey = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._expressionType)
		{
			_expressionType = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._expressionTypeSegments)
		{
			_expressionTypeSegments = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._expressionValue)
		{
			_expressionValue = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._expressionBoolean)
		{
			_expressionBoolean = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._setExpressionButton)
		{
			_setExpressionButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._expressionList)
		{
			_expressionList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._running)
		{
			_running = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._ownsController)
		{
			_ownsController = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._workbenchActive)
		{
			_workbenchActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._completedTraceFlushQueued)
		{
			_completedTraceFlushQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._autoPublishElapsed)
		{
			_autoPublishElapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._elapsedSimulationSeconds)
		{
			_elapsedSimulationSeconds = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._lastCompletedTransitionStableId)
		{
			_lastCompletedTransitionStableId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._lastCompletedTransitionHighlightRemaining)
		{
			_lastCompletedTransitionHighlightRemaining = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.IsRunning)
		{
			from = IsRunning;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.AutoAdvance)
		{
			from = AutoAdvance;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsPaused)
		{
			from = IsPaused;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RuntimeAttached)
		{
			from = RuntimeAttached;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.UsesBorrowedRuntime)
		{
			from = UsesBorrowedRuntime;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		string from2;
		if (name == PropertyName.RuntimeSourceName)
		{
			from2 = RuntimeSourceName;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		int from3;
		if (name == PropertyName.TransitionHistoryCount)
		{
			from3 = TransitionHistoryCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.RuntimeTraceCount)
		{
			from3 = RuntimeTraceCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.LastRuntimeTraceKind)
		{
			from2 = LastRuntimeTraceKind;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CurrentSnapshotRevision)
		{
			value = VariantUtils.CreateFrom<long>(CurrentSnapshotRevision);
			return true;
		}
		if (name == PropertyName.PendingDelayRemaining)
		{
			value = VariantUtils.CreateFrom<double>(PendingDelayRemaining);
			return true;
		}
		if (name == PropertyName.CurrentSnapshot)
		{
			value = VariantUtils.CreateFrom<StateMachineSnapshot>(CurrentSnapshot);
			return true;
		}
		if (name == PropertyName.LastCompletedTransitionStableId)
		{
			from2 = LastCompletedTransitionStableId;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsQuiescent)
		{
			from = IsQuiescent;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ExpressionPropertyCount)
		{
			from3 = ExpressionPropertyCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.SnapshotPublishCount)
		{
			from3 = SnapshotPublishCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.AutoSnapshotPublishCount)
		{
			from3 = AutoSnapshotPublishCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName._definition)
		{
			value = VariantUtils.CreateFrom(in _definition);
			return true;
		}
		if (name == PropertyName._lastSnapshot)
		{
			value = VariantUtils.CreateFrom(in _lastSnapshot);
			return true;
		}
		if (name == PropertyName._eventPicker)
		{
			value = VariantUtils.CreateFrom(in _eventPicker);
			return true;
		}
		if (name == PropertyName._sendEventButton)
		{
			value = VariantUtils.CreateFrom(in _sendEventButton);
			return true;
		}
		if (name == PropertyName._startButton)
		{
			value = VariantUtils.CreateFrom(in _startButton);
			return true;
		}
		if (name == PropertyName._resumeButton)
		{
			value = VariantUtils.CreateFrom(in _resumeButton);
			return true;
		}
		if (name == PropertyName._pauseButton)
		{
			value = VariantUtils.CreateFrom(in _pauseButton);
			return true;
		}
		if (name == PropertyName._stepButton)
		{
			value = VariantUtils.CreateFrom(in _stepButton);
			return true;
		}
		if (name == PropertyName._backButton)
		{
			value = VariantUtils.CreateFrom(in _backButton);
			return true;
		}
		if (name == PropertyName._resetButton)
		{
			value = VariantUtils.CreateFrom(in _resetButton);
			return true;
		}
		if (name == PropertyName._autoAdvance)
		{
			value = VariantUtils.CreateFrom(in _autoAdvance);
			return true;
		}
		if (name == PropertyName._status)
		{
			value = VariantUtils.CreateFrom(in _status);
			return true;
		}
		if (name == PropertyName._pendingLabel)
		{
			value = VariantUtils.CreateFrom(in _pendingLabel);
			return true;
		}
		if (name == PropertyName._pendingProgress)
		{
			value = VariantUtils.CreateFrom(in _pendingProgress);
			return true;
		}
		if (name == PropertyName._historyList)
		{
			value = VariantUtils.CreateFrom(in _historyList);
			return true;
		}
		if (name == PropertyName._expressionKey)
		{
			value = VariantUtils.CreateFrom(in _expressionKey);
			return true;
		}
		if (name == PropertyName._expressionType)
		{
			value = VariantUtils.CreateFrom(in _expressionType);
			return true;
		}
		if (name == PropertyName._expressionTypeSegments)
		{
			value = VariantUtils.CreateFrom(in _expressionTypeSegments);
			return true;
		}
		if (name == PropertyName._expressionValue)
		{
			value = VariantUtils.CreateFrom(in _expressionValue);
			return true;
		}
		if (name == PropertyName._expressionBoolean)
		{
			value = VariantUtils.CreateFrom(in _expressionBoolean);
			return true;
		}
		if (name == PropertyName._setExpressionButton)
		{
			value = VariantUtils.CreateFrom(in _setExpressionButton);
			return true;
		}
		if (name == PropertyName._expressionList)
		{
			value = VariantUtils.CreateFrom(in _expressionList);
			return true;
		}
		if (name == PropertyName._running)
		{
			value = VariantUtils.CreateFrom(in _running);
			return true;
		}
		if (name == PropertyName._ownsController)
		{
			value = VariantUtils.CreateFrom(in _ownsController);
			return true;
		}
		if (name == PropertyName._workbenchActive)
		{
			value = VariantUtils.CreateFrom(in _workbenchActive);
			return true;
		}
		if (name == PropertyName._completedTraceFlushQueued)
		{
			value = VariantUtils.CreateFrom(in _completedTraceFlushQueued);
			return true;
		}
		if (name == PropertyName._autoPublishElapsed)
		{
			value = VariantUtils.CreateFrom(in _autoPublishElapsed);
			return true;
		}
		if (name == PropertyName._elapsedSimulationSeconds)
		{
			value = VariantUtils.CreateFrom(in _elapsedSimulationSeconds);
			return true;
		}
		if (name == PropertyName._lastCompletedTransitionStableId)
		{
			value = VariantUtils.CreateFrom(in _lastCompletedTransitionStableId);
			return true;
		}
		if (name == PropertyName._lastCompletedTransitionHighlightRemaining)
		{
			value = VariantUtils.CreateFrom(in _lastCompletedTransitionHighlightRemaining);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AutoAdvance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsPaused, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.RuntimeAttached, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UsesBorrowedRuntime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.RuntimeSourceName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TransitionHistoryCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RuntimeTraceCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.LastRuntimeTraceKind, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CurrentSnapshotRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.PendingDelayRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CurrentSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.LastCompletedTransitionStableId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsQuiescent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ExpressionPropertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SnapshotPublishCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AutoSnapshotPublishCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._definition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lastSnapshot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sendEventButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._startButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resumeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pauseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stepButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._backButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resetButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._autoAdvance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._status, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pendingLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pendingProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._historyList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._expressionKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._expressionType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._expressionTypeSegments, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._expressionValue, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._expressionBoolean, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._setExpressionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._expressionList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._running, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._ownsController, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._workbenchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._completedTraceFlushQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._autoPublishElapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._elapsedSimulationSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._lastCompletedTransitionStableId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._lastCompletedTransitionHighlightRemaining, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.SnapshotPublishCount, Variant.From<int>(SnapshotPublishCount));
		info.AddProperty(PropertyName.AutoSnapshotPublishCount, Variant.From<int>(AutoSnapshotPublishCount));
		info.AddProperty(PropertyName._definition, Variant.From(in _definition));
		info.AddProperty(PropertyName._lastSnapshot, Variant.From(in _lastSnapshot));
		info.AddProperty(PropertyName._eventPicker, Variant.From(in _eventPicker));
		info.AddProperty(PropertyName._sendEventButton, Variant.From(in _sendEventButton));
		info.AddProperty(PropertyName._startButton, Variant.From(in _startButton));
		info.AddProperty(PropertyName._resumeButton, Variant.From(in _resumeButton));
		info.AddProperty(PropertyName._pauseButton, Variant.From(in _pauseButton));
		info.AddProperty(PropertyName._stepButton, Variant.From(in _stepButton));
		info.AddProperty(PropertyName._backButton, Variant.From(in _backButton));
		info.AddProperty(PropertyName._resetButton, Variant.From(in _resetButton));
		info.AddProperty(PropertyName._autoAdvance, Variant.From(in _autoAdvance));
		info.AddProperty(PropertyName._status, Variant.From(in _status));
		info.AddProperty(PropertyName._pendingLabel, Variant.From(in _pendingLabel));
		info.AddProperty(PropertyName._pendingProgress, Variant.From(in _pendingProgress));
		info.AddProperty(PropertyName._historyList, Variant.From(in _historyList));
		info.AddProperty(PropertyName._expressionKey, Variant.From(in _expressionKey));
		info.AddProperty(PropertyName._expressionType, Variant.From(in _expressionType));
		info.AddProperty(PropertyName._expressionTypeSegments, Variant.From(in _expressionTypeSegments));
		info.AddProperty(PropertyName._expressionValue, Variant.From(in _expressionValue));
		info.AddProperty(PropertyName._expressionBoolean, Variant.From(in _expressionBoolean));
		info.AddProperty(PropertyName._setExpressionButton, Variant.From(in _setExpressionButton));
		info.AddProperty(PropertyName._expressionList, Variant.From(in _expressionList));
		info.AddProperty(PropertyName._running, Variant.From(in _running));
		info.AddProperty(PropertyName._ownsController, Variant.From(in _ownsController));
		info.AddProperty(PropertyName._workbenchActive, Variant.From(in _workbenchActive));
		info.AddProperty(PropertyName._completedTraceFlushQueued, Variant.From(in _completedTraceFlushQueued));
		info.AddProperty(PropertyName._autoPublishElapsed, Variant.From(in _autoPublishElapsed));
		info.AddProperty(PropertyName._elapsedSimulationSeconds, Variant.From(in _elapsedSimulationSeconds));
		info.AddProperty(PropertyName._lastCompletedTransitionStableId, Variant.From(in _lastCompletedTransitionStableId));
		info.AddProperty(PropertyName._lastCompletedTransitionHighlightRemaining, Variant.From(in _lastCompletedTransitionHighlightRemaining));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.SnapshotPublishCount, out var value))
		{
			SnapshotPublishCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.AutoSnapshotPublishCount, out var value2))
		{
			AutoSnapshotPublishCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._definition, out var value3))
		{
			_definition = value3.As<StateMachineDefinition>();
		}
		if (info.TryGetProperty(PropertyName._lastSnapshot, out var value4))
		{
			_lastSnapshot = value4.As<StateMachineSnapshot>();
		}
		if (info.TryGetProperty(PropertyName._eventPicker, out var value5))
		{
			_eventPicker = value5.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._sendEventButton, out var value6))
		{
			_sendEventButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._startButton, out var value7))
		{
			_startButton = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._resumeButton, out var value8))
		{
			_resumeButton = value8.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pauseButton, out var value9))
		{
			_pauseButton = value9.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._stepButton, out var value10))
		{
			_stepButton = value10.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._backButton, out var value11))
		{
			_backButton = value11.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._resetButton, out var value12))
		{
			_resetButton = value12.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._autoAdvance, out var value13))
		{
			_autoAdvance = value13.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._status, out var value14))
		{
			_status = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pendingLabel, out var value15))
		{
			_pendingLabel = value15.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pendingProgress, out var value16))
		{
			_pendingProgress = value16.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._historyList, out var value17))
		{
			_historyList = value17.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._expressionKey, out var value18))
		{
			_expressionKey = value18.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._expressionType, out var value19))
		{
			_expressionType = value19.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._expressionTypeSegments, out var value20))
		{
			_expressionTypeSegments = value20.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._expressionValue, out var value21))
		{
			_expressionValue = value21.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._expressionBoolean, out var value22))
		{
			_expressionBoolean = value22.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._setExpressionButton, out var value23))
		{
			_setExpressionButton = value23.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._expressionList, out var value24))
		{
			_expressionList = value24.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._running, out var value25))
		{
			_running = value25.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._ownsController, out var value26))
		{
			_ownsController = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._workbenchActive, out var value27))
		{
			_workbenchActive = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._completedTraceFlushQueued, out var value28))
		{
			_completedTraceFlushQueued = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._autoPublishElapsed, out var value29))
		{
			_autoPublishElapsed = value29.As<double>();
		}
		if (info.TryGetProperty(PropertyName._elapsedSimulationSeconds, out var value30))
		{
			_elapsedSimulationSeconds = value30.As<double>();
		}
		if (info.TryGetProperty(PropertyName._lastCompletedTransitionStableId, out var value31))
		{
			_lastCompletedTransitionStableId = value31.As<string>();
		}
		if (info.TryGetProperty(PropertyName._lastCompletedTransitionHighlightRemaining, out var value32))
		{
			_lastCompletedTransitionHighlightRemaining = value32.As<double>();
		}
	}
}
