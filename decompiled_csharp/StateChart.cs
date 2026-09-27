using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[Icon("res://addons/godot_state_charts/state_chart.svg")]
[Tool]
[ScriptPath("res://addons/godot_state_charts/StateChart.cs")]
public class StateChart : Node
{
	[Flags]
	public enum TriggerType
	{
		None = 0,
		Event = 1,
		StateEnter = 2,
		PropertyChange = 4,
		StateChange = 8
	}

	public delegate void EventReceivedEventHandler(StringName eventName);

	private struct PendingTransition
	{
		public Transition transition;

		public StateChartState source;
	}

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName EmitEventReceived = "EmitEventReceived";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName InitializeLegacy = "InitializeLegacy";

		public static readonly StringName InitializeExpressionProperties = "InitializeExpressionProperties";

		public static readonly StringName InitializeResourceRuntime = "InitializeResourceRuntime";

		public static readonly StringName CopyExpressionPropertiesToResourceRuntime = "CopyExpressionPropertiesToResourceRuntime";

		public static readonly StringName DisableLegacyStateNodes = "DisableLegacyStateNodes";

		public static readonly StringName OnResourceDiagnostic = "OnResourceDiagnostic";

		public static readonly StringName ReportInitializationError = "ReportInitializationError";

		public static readonly StringName RegisterResourceRuntimeBatch = "RegisterResourceRuntimeBatch";

		public static readonly StringName SuspendResourceRuntimeBatch = "SuspendResourceRuntimeBatch";

		public static readonly StringName _EnterInitialState = "_EnterInitialState";

		public static readonly StringName SendEvent = "SendEvent";

		public static readonly StringName WarnUnknownEvent = "WarnUnknownEvent";

		public static readonly StringName SetExpressionProperty = "SetExpressionProperty";

		public static readonly StringName GetExpressionProperty = "GetExpressionProperty";

		public static readonly StringName CaptureResourceSnapshot = "CaptureResourceSnapshot";

		public static readonly StringName RestoreResourceSnapshot = "RestoreResourceSnapshot";

		public static readonly StringName ApplyRemoteSnapshot = "ApplyRemoteSnapshot";

		public static readonly StringName _RunChanges = "_RunChanges";

		public static readonly StringName _RunTransition = "_RunTransition";

		public static readonly StringName _RunQueuedTransitions = "_RunQueuedTransitions";

		public static readonly StringName _DoRunTransition = "_DoRunTransition";

		public static readonly StringName _WarnNotActive = "_WarnNotActive";

		public static readonly StringName Step = "Step";

		public new static readonly StringName _GetConfigurationWarnings = "_GetConfigurationWarnings";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName ReleaseResourceRuntime = "ReleaseResourceRuntime";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName trackInEditor = "trackInEditor";

		public static readonly StringName warnOnSendingUnknownEvents = "warnOnSendingUnknownEvents";

		public static readonly StringName initialExpressionProperties = "initialExpressionProperties";

		public static readonly StringName RuntimeMode = "RuntimeMode";

		public static readonly StringName Definition = "Definition";

		public static readonly StringName IsResourceMode = "IsResourceMode";

		public static readonly StringName ResourceInitializationSucceeded = "ResourceInitializationSucceeded";

		public static readonly StringName InitializationError = "InitializationError";

		public static readonly StringName StateRevision = "StateRevision";

		public static readonly StringName InitializationDisabled = "InitializationDisabled";

		public static readonly StringName CurrentState = "CurrentState";

		public static readonly StringName _trackInEditor = "_trackInEditor";

		public static readonly StringName _warnOnSendingUnknownEvents = "_warnOnSendingUnknownEvents";

		public static readonly StringName _initialExpressionProperties = "_initialExpressionProperties";

		public static readonly StringName _state = "_state";

		public static readonly StringName _expressionProperties = "_expressionProperties";

		public static readonly StringName _propertyChangePending = "_propertyChangePending";

		public static readonly StringName _stateChangePending = "_stateChangePending";

		public static readonly StringName _lockedDown = "_lockedDown";

		public static readonly StringName _transitionsProcessingActive = "_transitionsProcessingActive";

		public static readonly StringName _resourceBatchRegistered = "_resourceBatchRegistered";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private bool _trackInEditor;

	private bool _warnOnSendingUnknownEvents = true;

	private Dictionary _initialExpressionProperties = new Dictionary();

	private StateChartState _state;

	public Dictionary _expressionProperties = new Dictionary();

	private List<StringName> _queuedEvents = new List<StringName>();

	private bool _propertyChangePending;

	private bool _stateChangePending;

	private bool _lockedDown;

	private bool _transitionsProcessingActive;

	private readonly List<PendingTransition> _pendingTransitions = new List<PendingTransition>();

	private HashSet<StringName> _validEventNames = new HashSet<StringName>();

	private StateMachineController _resourceController;

	private bool _resourceBatchRegistered;

	[Export(PropertyHint.None, "")]
	public bool trackInEditor
	{
		get
		{
			return _trackInEditor;
		}
		set
		{
			_trackInEditor = value;
		}
	}

	[Export(PropertyHint.None, "")]
	public bool warnOnSendingUnknownEvents
	{
		get
		{
			return _warnOnSendingUnknownEvents;
		}
		set
		{
			_warnOnSendingUnknownEvents = value;
		}
	}

	[Export(PropertyHint.None, "")]
	public Dictionary initialExpressionProperties
	{
		get
		{
			return _initialExpressionProperties;
		}
		set
		{
			_initialExpressionProperties = value;
		}
	}

	[Export(PropertyHint.None, "")]
	public StateMachineRuntimeMode RuntimeMode { get; set; }

	[Export(PropertyHint.None, "")]
	public StateMachineDefinition Definition { get; set; }

	public bool IsResourceMode { get; private set; }

	public bool ResourceInitializationSucceeded
	{
		get
		{
			if (IsResourceMode)
			{
				return _resourceController?.IsInitialized ?? false;
			}
			return false;
		}
	}

	public string InitializationError { get; private set; } = string.Empty;

	public StateHandle CurrentStateHandle
	{
		get
		{
			if (!IsResourceMode)
			{
				return null;
			}
			return _resourceController?.CurrentStateHandle;
		}
	}

	public long StateRevision
	{
		get
		{
			if (!IsResourceMode)
			{
				return 0L;
			}
			return _resourceController?.Revision ?? 0;
		}
	}

	internal StateMachineRuntime ResourceRuntime => _resourceController?.Runtime;

	public bool InitializationDisabled { get; set; }

	public StateChartState CurrentState
	{
		get
		{
			if (IsResourceMode)
			{
				return null;
			}
			if (_state == null || !GodotObject.IsInstanceValid(_state))
			{
				return null;
			}
			return _state;
		}
	}

	public event EventReceivedEventHandler OnEventReceived;

	public void EmitEventReceived(StringName eventName)
	{
		OnEventReceived?.Invoke(eventName);
	}

	public override void _EnterTree()
	{
		if (_resourceController?.Runtime != null && !_resourceBatchRegistered && !RegisterResourceRuntimeBatch())
		{
			ReportInitializationError("Unable to resume compact StateChart runtime batching.");
		}
	}

	public override void _Ready()
	{
		if (Engine.IsEditorHint() || InitializationDisabled)
		{
			return;
		}
		InitializeExpressionProperties();
		if (RuntimeMode == StateMachineRuntimeMode.Legacy)
		{
			InitializeLegacy();
		}
		else if (RuntimeMode == StateMachineRuntimeMode.Resource)
		{
			if (Definition == null)
			{
				IsResourceMode = true;
				DisableLegacyStateNodes(this);
				ReportInitializationError("Resource StateChart requires a StateMachineDefinition.");
			}
			else
			{
				InitializeResourceRuntime();
			}
		}
		else if (Definition != null)
		{
			InitializeResourceRuntime();
		}
		else
		{
			InitializeLegacy();
		}
	}

	private void InitializeLegacy()
	{
		IsResourceMode = false;
		InitializationError = string.Empty;
		if (GetChildCount() != 1)
		{
			GD.PushError("StateChart must have exactly one child");
			return;
		}
		Node child = GetChild(0);
		if (!(child is StateChartState))
		{
			GD.PushError("StateMachine's child must be a State");
			return;
		}
		if (OS.IsDebugBuild())
		{
			_validEventNames = new HashSet<StringName>(StateChartUtil.EventsOf(this));
		}
		_state = child as StateChartState;
		_state._StateInit();
		Callable.From(_EnterInitialState).CallDeferred();
	}

	private void InitializeExpressionProperties()
	{
		if (_initialExpressionProperties == null)
		{
			return;
		}
		foreach (Variant key in _initialExpressionProperties.Keys)
		{
			if (key.VariantType != Variant.Type.String && key.VariantType != Variant.Type.StringName)
			{
				GD.PushError("Expression property names must be strings. Ignoring initial expression property with key ", key);
			}
			else
			{
				_expressionProperties[key] = _initialExpressionProperties[key];
			}
		}
	}

	private void InitializeResourceRuntime()
	{
		IsResourceMode = true;
		InitializationError = string.Empty;
		DisableLegacyStateNodes(this);
		try
		{
			_resourceController = new StateMachineController();
			_resourceController.EventReceived += EmitEventReceived;
			_resourceController.Diagnostic += OnResourceDiagnostic;
			if (!_resourceController.Initialize(Definition, this))
			{
				throw new InvalidOperationException(_resourceController.InitializationError);
			}
			CopyExpressionPropertiesToResourceRuntime();
			BuildResourceEventNames(_resourceController.Runtime.Program);
			if (!RegisterResourceRuntimeBatch())
			{
				throw new InvalidOperationException("Unable to register compact StateChart runtime batching.");
			}
			Callable.From(_EnterInitialState).CallDeferred();
		}
		catch (Exception ex)
		{
			ReleaseResourceRuntime();
			ReportInitializationError(ex.Message);
		}
	}

	private void CopyExpressionPropertiesToResourceRuntime()
	{
		foreach (Variant key in _expressionProperties.Keys)
		{
			_resourceController.SetExpressionProperty(key.AsStringName(), _expressionProperties[key]);
		}
	}

	private void DisableLegacyStateNodes(Node parent)
	{
		foreach (Node child in parent.GetChildren())
		{
			if (child is StateChartState stateChartState)
			{
				stateChartState.ProcessMode = ProcessModeEnum.Disabled;
				stateChartState.SetProcess(enable: false);
				stateChartState.SetPhysicsProcess(enable: false);
				stateChartState.SetProcessInput(enable: false);
				stateChartState.SetProcessUnhandledInput(enable: false);
			}
			DisableLegacyStateNodes(child);
		}
	}

	private void BuildResourceEventNames(StateMachineProgram program)
	{
		_validEventNames.Clear();
		ReadOnlySpan<CompiledStateMachineTransition> transitions = program.Transitions;
		for (int i = 0; i < transitions.Length; i++)
		{
			StringName eventName = transitions[i].EventName;
			if (eventName != null && eventName != (StringName)"")
			{
				_validEventNames.Add(eventName);
			}
		}
	}

	private void OnResourceDiagnostic(string code)
	{
		GD.PushError("StateChart compact runtime diagnostic: ", code);
	}

	private void ReportInitializationError(string message)
	{
		InitializationError = message ?? "StateChart Resource initialization failed.";
		GD.PushError(InitializationError);
	}

	private bool RegisterResourceRuntimeBatch()
	{
		if (_resourceController?.Runtime == null)
		{
			return false;
		}
		if (_resourceBatchRegistered)
		{
			return true;
		}
		_resourceBatchRegistered = StateMachineRuntimeBatch.Register(this, _resourceController.Runtime, _resourceController.HasProcessWork, _resourceController.HasPhysicsWork);
		return _resourceBatchRegistered;
	}

	private void SuspendResourceRuntimeBatch()
	{
		if (_resourceBatchRegistered)
		{
			StateMachineRuntimeBatch.Unregister(this);
			_resourceBatchRegistered = false;
		}
	}

	private void _EnterInitialState()
	{
		if (IsResourceMode)
		{
			_resourceController?.EnterInitialState();
			return;
		}
		_transitionsProcessingActive = true;
		_lockedDown = true;
		_state._StateEnter(null);
		_RunQueuedTransitions();
		_RunChanges();
	}

	public void SendEvent(StringName eventName)
	{
		if (!IsNodeReady())
		{
			GD.PushError("State chart is not yet ready. If you call `SendEvent` in _Ready, please call it deferred.");
			return;
		}
		if (IsResourceMode)
		{
			if (_resourceController?.Runtime == null)
			{
				GD.PushError("State chart has no compact runtime. Ignoring call to `SendEvent`.");
				return;
			}
			WarnUnknownEvent(eventName);
			_resourceController.SendEvent(eventName);
			return;
		}
		if (!GodotObject.IsInstanceValid(_state))
		{
			GD.PushError("State chart has no root state. Ignoring call to `SendEvent`.");
			return;
		}
		WarnUnknownEvent(eventName);
		_queuedEvents.Add(eventName);
		if (!_lockedDown)
		{
			_RunChanges();
		}
	}

	private void WarnUnknownEvent(StringName eventName)
	{
		if (_warnOnSendingUnknownEvents && eventName != null && eventName != (StringName)"" && OS.IsDebugBuild() && !_validEventNames.Contains(eventName))
		{
			GD.PushWarning("State chart does not have an event '", eventName, "' defined. Sending this event will do nothing.");
		}
	}

	public void SetExpressionProperty(StringName name, Variant value)
	{
		if (!IsNodeReady())
		{
			GD.PushError("State chart is not yet ready. If you call `SetExpressionProperty` in `_Ready`, please call it deferred.");
			return;
		}
		if (IsResourceMode)
		{
			if (_resourceController?.Runtime == null)
			{
				GD.PushError("State chart has no compact runtime. Ignoring call to `SetExpressionProperty`.");
			}
			else
			{
				_resourceController.SetExpressionProperty(name, value);
			}
			return;
		}
		if (!GodotObject.IsInstanceValid(_state))
		{
			GD.PushError("State chart has no root state. Ignoring call to `SetExpressionProperty`.");
			return;
		}
		_expressionProperties[name] = value;
		_propertyChangePending = true;
		if (!_lockedDown)
		{
			_RunChanges();
		}
	}

	public Variant GetExpressionProperty(StringName name, Variant defaultValue = default(Variant))
	{
		if (IsResourceMode)
		{
			return _resourceController?.GetExpressionProperty(name, defaultValue) ?? defaultValue;
		}
		if (_expressionProperties.ContainsKey(name))
		{
			return _expressionProperties[name];
		}
		return defaultValue;
	}

	public StateHandle GetStateById(string stableId)
	{
		if (!IsResourceMode)
		{
			return null;
		}
		return _resourceController?.GetStateById(stableId);
	}

	public StateHandle GetState(string stableId)
	{
		return GetStateById(stableId);
	}

	public StateMachineSnapshot CaptureResourceSnapshot()
	{
		if (!IsResourceMode)
		{
			return null;
		}
		return _resourceController?.CaptureSnapshot();
	}

	public bool RestoreResourceSnapshot(StateMachineSnapshot snapshot, bool suppressEntryEffects = false)
	{
		if (IsResourceMode && _resourceController?.Runtime != null)
		{
			return _resourceController.RestoreSnapshot(snapshot, suppressEntryEffects);
		}
		return false;
	}

	public bool ApplyRemoteSnapshot(StateMachineSnapshot snapshot, bool suppressEntryEffects = true)
	{
		if (IsResourceMode && _resourceController?.Runtime != null)
		{
			return _resourceController.ApplyRemoteSnapshot(snapshot, suppressEntryEffects);
		}
		return false;
	}

	internal void _RunChanges()
	{
		_lockedDown = true;
		while (_queuedEvents.Count > 0 || _propertyChangePending || _stateChangePending)
		{
			if (_stateChangePending)
			{
				_stateChangePending = false;
				_state._ProcessTransitions(TriggerType.StateChange);
			}
			if (_propertyChangePending)
			{
				_propertyChangePending = false;
				_state._ProcessTransitions(TriggerType.PropertyChange);
			}
			if (_queuedEvents.Count > 0)
			{
				StringName stringName = _queuedEvents[0];
				_queuedEvents.RemoveAt(0);
				OnEventReceived?.Invoke(stringName);
				_state._ProcessTransitions(TriggerType.Event, stringName);
			}
		}
		_lockedDown = false;
	}

	public void _RunTransition(Transition transition, StateChartState source)
	{
		_pendingTransitions.Add(new PendingTransition
		{
			transition = transition,
			source = source
		});
		if (!_transitionsProcessingActive)
		{
			_RunQueuedTransitions();
		}
	}

	private void _RunQueuedTransitions()
	{
		_transitionsProcessingActive = true;
		int num = 1;
		while (_pendingTransitions.Count > 0)
		{
			PendingTransition pendingTransition = _pendingTransitions[0];
			_pendingTransitions.RemoveAt(0);
			_DoRunTransition(pendingTransition.transition, pendingTransition.source);
			num++;
			if (num > 100)
			{
				GD.PushError("Infinite loop detected in transitions. Aborting. The state chart is now in an invalid state and no longer usable.");
				break;
			}
		}
		_transitionsProcessingActive = false;
		if (!_lockedDown)
		{
			_RunChanges();
		}
	}

	private void _DoRunTransition(Transition transition, StateChartState source)
	{
		if (source.active)
		{
			transition.EmitTaken();
			source._HandleTransition(transition, source);
			_stateChangePending = true;
		}
		else
		{
			_WarnNotActive(transition, source);
		}
	}

	private void _WarnNotActive(Transition transition, StateChartState source)
	{
		GD.PushWarning("Ignoring request for transitioning from ", source.Name, " to ", transition.to, " as the source state is no longer active. Check whether your trigger multiple state changes within a single frame.");
	}

	public void Step()
	{
		if (!IsNodeReady())
		{
			GD.PushError("State chart is not yet ready. If you call `Step` in `_Ready`, please call it deferred.");
		}
		else if (IsResourceMode)
		{
			GD.PushWarning("Compact StateChart does not expose Legacy Step callbacks.");
		}
		else if (!GodotObject.IsInstanceValid(_state))
		{
			GD.PushError("State chart has no root state. Ignoring call to `Step`.");
		}
		else
		{
			_state._StateStep();
		}
	}

	public override string[] _GetConfigurationWarnings()
	{
		List<string> list = new List<string>();
		if (RuntimeMode == StateMachineRuntimeMode.Resource || (RuntimeMode == StateMachineRuntimeMode.Auto && Definition != null))
		{
			if (Definition == null)
			{
				list.Add("Resource StateChart requires a StateMachineDefinition.");
			}
			else
			{
				StateMachineValidationResult stateMachineValidationResult = StateMachineValidator.Validate(Definition);
				for (int i = 0; i < stateMachineValidationResult.Diagnostics.Count; i++)
				{
					StateMachineDiagnostic stateMachineDiagnostic = stateMachineValidationResult.Diagnostics[i];
					list.Add(stateMachineDiagnostic.Code + ": " + stateMachineDiagnostic.Message);
				}
			}
			return list.ToArray();
		}
		if (GetChildCount() != 1)
		{
			list.Add("StateChart must have exactly one child");
		}
		else if (!(GetChild(0) is StateChartState))
		{
			list.Add("StateChart's child must be a State");
		}
		return list.ToArray();
	}

	public override void _ExitTree()
	{
		SuspendResourceRuntimeBatch();
	}

	public override void _Notification(int what)
	{
		if ((long)what == 1)
		{
			ReleaseResourceRuntime();
		}
	}

	private void ReleaseResourceRuntime()
	{
		SuspendResourceRuntimeBatch();
		if (_resourceController != null)
		{
			_resourceController.EventReceived -= EmitEventReceived;
			_resourceController.Diagnostic -= OnResourceDiagnostic;
			_resourceController.Dispose();
			_resourceController = null;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(30)
		{
			new MethodInfo(MethodName.EmitEventReceived, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitializeLegacy, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitializeExpressionProperties, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitializeResourceRuntime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CopyExpressionPropertiesToResourceRuntime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisableLegacyStateNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnResourceDiagnostic, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "code", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReportInitializationError, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterResourceRuntimeBatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SuspendResourceRuntimeBatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._EnterInitialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WarnUnknownEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetExpressionProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetExpressionProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "defaultValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.CaptureResourceSnapshot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreResourceSnapshot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressEntryEffects", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyRemoteSnapshot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "suppressEntryEffects", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._RunChanges, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._RunTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._RunQueuedTransitions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._DoRunTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._WarnNotActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.Step, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetConfigurationWarnings, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseResourceRuntime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EmitEventReceived && args.Count == 1)
		{
			EmitEventReceived(VariantUtils.ConvertTo<StringName>(in args[0]));
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
		if (method == MethodName.InitializeLegacy && args.Count == 0)
		{
			InitializeLegacy();
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeExpressionProperties && args.Count == 0)
		{
			InitializeExpressionProperties();
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeResourceRuntime && args.Count == 0)
		{
			InitializeResourceRuntime();
			ret = default;
			return true;
		}
		if (method == MethodName.CopyExpressionPropertiesToResourceRuntime && args.Count == 0)
		{
			CopyExpressionPropertiesToResourceRuntime();
			ret = default;
			return true;
		}
		if (method == MethodName.DisableLegacyStateNodes && args.Count == 1)
		{
			DisableLegacyStateNodes(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnResourceDiagnostic && args.Count == 1)
		{
			OnResourceDiagnostic(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReportInitializationError && args.Count == 1)
		{
			ReportInitializationError(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterResourceRuntimeBatch && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RegisterResourceRuntimeBatch());
			return true;
		}
		if (method == MethodName.SuspendResourceRuntimeBatch && args.Count == 0)
		{
			SuspendResourceRuntimeBatch();
			ret = default;
			return true;
		}
		if (method == MethodName._EnterInitialState && args.Count == 0)
		{
			_EnterInitialState();
			ret = default;
			return true;
		}
		if (method == MethodName.SendEvent && args.Count == 1)
		{
			SendEvent(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.WarnUnknownEvent && args.Count == 1)
		{
			WarnUnknownEvent(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetExpressionProperty && args.Count == 2)
		{
			SetExpressionProperty(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetExpressionProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(GetExpressionProperty(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName.CaptureResourceSnapshot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateMachineSnapshot>(CaptureResourceSnapshot());
			return true;
		}
		if (method == MethodName.RestoreResourceSnapshot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(RestoreResourceSnapshot(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyRemoteSnapshot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ApplyRemoteSnapshot(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName._RunChanges && args.Count == 0)
		{
			_RunChanges();
			ret = default;
			return true;
		}
		if (method == MethodName._RunTransition && args.Count == 2)
		{
			_RunTransition(VariantUtils.ConvertTo<Transition>(in args[0]), VariantUtils.ConvertTo<StateChartState>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._RunQueuedTransitions && args.Count == 0)
		{
			_RunQueuedTransitions();
			ret = default;
			return true;
		}
		if (method == MethodName._DoRunTransition && args.Count == 2)
		{
			_DoRunTransition(VariantUtils.ConvertTo<Transition>(in args[0]), VariantUtils.ConvertTo<StateChartState>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._WarnNotActive && args.Count == 2)
		{
			_WarnNotActive(VariantUtils.ConvertTo<Transition>(in args[0]), VariantUtils.ConvertTo<StateChartState>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Step && args.Count == 0)
		{
			Step();
			ret = default;
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(_GetConfigurationWarnings());
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
		if (method == MethodName.ReleaseResourceRuntime && args.Count == 0)
		{
			ReleaseResourceRuntime();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EmitEventReceived)
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
		if (method == MethodName.InitializeLegacy)
		{
			return true;
		}
		if (method == MethodName.InitializeExpressionProperties)
		{
			return true;
		}
		if (method == MethodName.InitializeResourceRuntime)
		{
			return true;
		}
		if (method == MethodName.CopyExpressionPropertiesToResourceRuntime)
		{
			return true;
		}
		if (method == MethodName.DisableLegacyStateNodes)
		{
			return true;
		}
		if (method == MethodName.OnResourceDiagnostic)
		{
			return true;
		}
		if (method == MethodName.ReportInitializationError)
		{
			return true;
		}
		if (method == MethodName.RegisterResourceRuntimeBatch)
		{
			return true;
		}
		if (method == MethodName.SuspendResourceRuntimeBatch)
		{
			return true;
		}
		if (method == MethodName._EnterInitialState)
		{
			return true;
		}
		if (method == MethodName.SendEvent)
		{
			return true;
		}
		if (method == MethodName.WarnUnknownEvent)
		{
			return true;
		}
		if (method == MethodName.SetExpressionProperty)
		{
			return true;
		}
		if (method == MethodName.GetExpressionProperty)
		{
			return true;
		}
		if (method == MethodName.CaptureResourceSnapshot)
		{
			return true;
		}
		if (method == MethodName.RestoreResourceSnapshot)
		{
			return true;
		}
		if (method == MethodName.ApplyRemoteSnapshot)
		{
			return true;
		}
		if (method == MethodName._RunChanges)
		{
			return true;
		}
		if (method == MethodName._RunTransition)
		{
			return true;
		}
		if (method == MethodName._RunQueuedTransitions)
		{
			return true;
		}
		if (method == MethodName._DoRunTransition)
		{
			return true;
		}
		if (method == MethodName._WarnNotActive)
		{
			return true;
		}
		if (method == MethodName.Step)
		{
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings)
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
		if (method == MethodName.ReleaseResourceRuntime)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.trackInEditor)
		{
			trackInEditor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.warnOnSendingUnknownEvents)
		{
			warnOnSendingUnknownEvents = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.initialExpressionProperties)
		{
			initialExpressionProperties = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName.RuntimeMode)
		{
			RuntimeMode = VariantUtils.ConvertTo<StateMachineRuntimeMode>(in value);
			return true;
		}
		if (name == PropertyName.Definition)
		{
			Definition = VariantUtils.ConvertTo<StateMachineDefinition>(in value);
			return true;
		}
		if (name == PropertyName.IsResourceMode)
		{
			IsResourceMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.InitializationError)
		{
			InitializationError = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.InitializationDisabled)
		{
			InitializationDisabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._trackInEditor)
		{
			_trackInEditor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._warnOnSendingUnknownEvents)
		{
			_warnOnSendingUnknownEvents = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._initialExpressionProperties)
		{
			_initialExpressionProperties = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._state)
		{
			_state = VariantUtils.ConvertTo<StateChartState>(in value);
			return true;
		}
		if (name == PropertyName._expressionProperties)
		{
			_expressionProperties = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._propertyChangePending)
		{
			_propertyChangePending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._stateChangePending)
		{
			_stateChangePending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._lockedDown)
		{
			_lockedDown = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._transitionsProcessingActive)
		{
			_transitionsProcessingActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._resourceBatchRegistered)
		{
			_resourceBatchRegistered = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		bool from;
		if (name == PropertyName.trackInEditor)
		{
			from = trackInEditor;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.warnOnSendingUnknownEvents)
		{
			from = warnOnSendingUnknownEvents;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.initialExpressionProperties)
		{
			value = VariantUtils.CreateFrom<Dictionary>(initialExpressionProperties);
			return true;
		}
		if (name == PropertyName.RuntimeMode)
		{
			value = VariantUtils.CreateFrom<StateMachineRuntimeMode>(RuntimeMode);
			return true;
		}
		if (name == PropertyName.Definition)
		{
			value = VariantUtils.CreateFrom<StateMachineDefinition>(Definition);
			return true;
		}
		if (name == PropertyName.IsResourceMode)
		{
			from = IsResourceMode;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ResourceInitializationSucceeded)
		{
			from = ResourceInitializationSucceeded;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.InitializationError)
		{
			value = VariantUtils.CreateFrom<string>(InitializationError);
			return true;
		}
		if (name == PropertyName.StateRevision)
		{
			value = VariantUtils.CreateFrom<long>(StateRevision);
			return true;
		}
		if (name == PropertyName.InitializationDisabled)
		{
			from = InitializationDisabled;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CurrentState)
		{
			value = VariantUtils.CreateFrom<StateChartState>(CurrentState);
			return true;
		}
		if (name == PropertyName._trackInEditor)
		{
			value = VariantUtils.CreateFrom(in _trackInEditor);
			return true;
		}
		if (name == PropertyName._warnOnSendingUnknownEvents)
		{
			value = VariantUtils.CreateFrom(in _warnOnSendingUnknownEvents);
			return true;
		}
		if (name == PropertyName._initialExpressionProperties)
		{
			value = VariantUtils.CreateFrom(in _initialExpressionProperties);
			return true;
		}
		if (name == PropertyName._state)
		{
			value = VariantUtils.CreateFrom(in _state);
			return true;
		}
		if (name == PropertyName._expressionProperties)
		{
			value = VariantUtils.CreateFrom(in _expressionProperties);
			return true;
		}
		if (name == PropertyName._propertyChangePending)
		{
			value = VariantUtils.CreateFrom(in _propertyChangePending);
			return true;
		}
		if (name == PropertyName._stateChangePending)
		{
			value = VariantUtils.CreateFrom(in _stateChangePending);
			return true;
		}
		if (name == PropertyName._lockedDown)
		{
			value = VariantUtils.CreateFrom(in _lockedDown);
			return true;
		}
		if (name == PropertyName._transitionsProcessingActive)
		{
			value = VariantUtils.CreateFrom(in _transitionsProcessingActive);
			return true;
		}
		if (name == PropertyName._resourceBatchRegistered)
		{
			value = VariantUtils.CreateFrom(in _resourceBatchRegistered);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName.trackInEditor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._trackInEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.warnOnSendingUnknownEvents, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName._warnOnSendingUnknownEvents, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName.initialExpressionProperties, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._initialExpressionProperties, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._state, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._expressionProperties, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._propertyChangePending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateChangePending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._lockedDown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._transitionsProcessingActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._resourceBatchRegistered, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RuntimeMode, PropertyHint.Enum, "Auto,Resource,Legacy", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName.Definition, PropertyHint.ResourceType, "StateMachineDefinition", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsResourceMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ResourceInitializationSucceeded, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.InitializationError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.StateRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.InitializationDisabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CurrentState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.trackInEditor, Variant.From<bool>(trackInEditor));
		info.AddProperty(PropertyName.warnOnSendingUnknownEvents, Variant.From<bool>(warnOnSendingUnknownEvents));
		info.AddProperty(PropertyName.initialExpressionProperties, Variant.From<Dictionary>(initialExpressionProperties));
		info.AddProperty(PropertyName.RuntimeMode, Variant.From<StateMachineRuntimeMode>(RuntimeMode));
		info.AddProperty(PropertyName.Definition, Variant.From<StateMachineDefinition>(Definition));
		info.AddProperty(PropertyName.IsResourceMode, Variant.From<bool>(IsResourceMode));
		info.AddProperty(PropertyName.InitializationError, Variant.From<string>(InitializationError));
		info.AddProperty(PropertyName.InitializationDisabled, Variant.From<bool>(InitializationDisabled));
		info.AddProperty(PropertyName._trackInEditor, Variant.From(in _trackInEditor));
		info.AddProperty(PropertyName._warnOnSendingUnknownEvents, Variant.From(in _warnOnSendingUnknownEvents));
		info.AddProperty(PropertyName._initialExpressionProperties, Variant.From(in _initialExpressionProperties));
		info.AddProperty(PropertyName._state, Variant.From(in _state));
		info.AddProperty(PropertyName._expressionProperties, Variant.From(in _expressionProperties));
		info.AddProperty(PropertyName._propertyChangePending, Variant.From(in _propertyChangePending));
		info.AddProperty(PropertyName._stateChangePending, Variant.From(in _stateChangePending));
		info.AddProperty(PropertyName._lockedDown, Variant.From(in _lockedDown));
		info.AddProperty(PropertyName._transitionsProcessingActive, Variant.From(in _transitionsProcessingActive));
		info.AddProperty(PropertyName._resourceBatchRegistered, Variant.From(in _resourceBatchRegistered));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.trackInEditor, out var value))
		{
			trackInEditor = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.warnOnSendingUnknownEvents, out var value2))
		{
			warnOnSendingUnknownEvents = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.initialExpressionProperties, out var value3))
		{
			initialExpressionProperties = value3.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName.RuntimeMode, out var value4))
		{
			RuntimeMode = value4.As<StateMachineRuntimeMode>();
		}
		if (info.TryGetProperty(PropertyName.Definition, out var value5))
		{
			Definition = value5.As<StateMachineDefinition>();
		}
		if (info.TryGetProperty(PropertyName.IsResourceMode, out var value6))
		{
			IsResourceMode = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.InitializationError, out var value7))
		{
			InitializationError = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName.InitializationDisabled, out var value8))
		{
			InitializationDisabled = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._trackInEditor, out var value9))
		{
			_trackInEditor = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._warnOnSendingUnknownEvents, out var value10))
		{
			_warnOnSendingUnknownEvents = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._initialExpressionProperties, out var value11))
		{
			_initialExpressionProperties = value11.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._state, out var value12))
		{
			_state = value12.As<StateChartState>();
		}
		if (info.TryGetProperty(PropertyName._expressionProperties, out var value13))
		{
			_expressionProperties = value13.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._propertyChangePending, out var value14))
		{
			_propertyChangePending = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._stateChangePending, out var value15))
		{
			_stateChangePending = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._lockedDown, out var value16))
		{
			_lockedDown = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._transitionsProcessingActive, out var value17))
		{
			_transitionsProcessingActive = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._resourceBatchRegistered, out var value18))
		{
			_resourceBatchRegistered = value18.As<bool>();
		}
	}
}
