using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[ScriptPath("res://addons/godot_state_charts/StateChartState.cs")]
public class StateChartState : Node
{
	public delegate void StateEnteredEventHandler();

	public delegate void StateExitedEventHandler();

	public delegate void EventReceivedEventHandler(StringName eventName);

	public delegate void StateProcessingEventHandler(double delta);

	public delegate void StatePhysicsProcessingEventHandler(double delta);

	public delegate void StateSteppedEventHandler();

	public delegate void StateInputEventHandler(InputEvent event_);

	public delegate void StateUnhandledInputEventHandler(InputEvent event_);

	public delegate void TransitionPendingEventHandler(double initialDelay, double remainingDelay);

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName RaiseEventReceived = "RaiseEventReceived";

		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName _FindChart = "_FindChart";

		public static readonly StringName _RunTransition = "_RunTransition";

		public static readonly StringName _StateInit = "_StateInit";

		public static readonly StringName _StateEnter = "_StateEnter";

		public static readonly StringName _StateExit = "_StateExit";

		public static readonly StringName StateSave = "StateSave";

		public static readonly StringName StateRestore = "StateRestore";

		public static readonly StringName _StateSave = "_StateSave";

		public static readonly StringName _StateRestore = "_StateRestore";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName _HandleTransition = "_HandleTransition";

		public new static readonly StringName _PhysicsProcess = "_PhysicsProcess";

		public static readonly StringName BatchPhysicsProcess = "BatchPhysicsProcess";

		public static readonly StringName BatchPhysicsProcessValidated = "BatchPhysicsProcessValidated";

		public static readonly StringName InvokeStatePhysicsProcessing = "InvokeStatePhysicsProcessing";

		public static readonly StringName _StateStep = "_StateStep";

		public new static readonly StringName _Input = "_Input";

		public new static readonly StringName _UnhandledInput = "_UnhandledInput";

		public static readonly StringName _ProcessTransitions = "_ProcessTransitions";

		public static readonly StringName _QueueTransition = "_QueueTransition";

		public new static readonly StringName _GetConfigurationWarnings = "_GetConfigurationWarnings";

		public static readonly StringName _ToggleProcessing = "_ToggleProcessing";

		public new static readonly StringName _ExitTree = "_ExitTree";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName ObservedProcessFlags = "ObservedProcessFlags";

		public static readonly StringName active = "active";

		public static readonly StringName IsPhysicsBatchDispatchActive = "IsPhysicsBatchDispatchActive";

		public static readonly StringName _stateActive = "_stateActive";

		public static readonly StringName _usingPhysicsBatch = "_usingPhysicsBatch";

		public static readonly StringName _statePhysicsCallbackMetricNames = "_statePhysicsCallbackMetricNames";

		public static readonly StringName _pendingTransition = "_pendingTransition";

		public static readonly StringName _pendingTransitionRemainingDelay = "_pendingTransitionRemainingDelay";

		public static readonly StringName _pendingTransitionInitialDelay = "_pendingTransitionInitialDelay";

		public static readonly StringName _chart = "_chart";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private StatePhysicsProcessingEventHandler _onStatePhysicsProcessing;

	public bool _stateActive;

	public static bool UsePhysicsBatch = true;

	private bool _usingPhysicsBatch;

	private StatePhysicsProcessingEventHandler _statePhysicsCallbackCacheSource;

	private Delegate[] _statePhysicsCallbackCache;

	private string[] _statePhysicsCallbackMetricNames;

	private Transition _pendingTransition;

	private double _pendingTransitionRemainingDelay;

	private double _pendingTransitionInitialDelay;

	private List<Transition> _transitions = new List<Transition>();

	public StateChart _chart;

	public StateMachineProcessFlags ObservedProcessFlags
	{
		get
		{
			StateMachineProcessFlags stateMachineProcessFlags = StateMachineProcessFlags.None;
			if (OnStateProcessing != null)
			{
				stateMachineProcessFlags |= StateMachineProcessFlags.Process;
			}
			if (_onStatePhysicsProcessing != null)
			{
				stateMachineProcessFlags |= StateMachineProcessFlags.PhysicsProcess;
			}
			if (OnStateInput != null)
			{
				stateMachineProcessFlags |= StateMachineProcessFlags.Input;
			}
			if (OnStateUnhandledInput != null)
			{
				stateMachineProcessFlags |= StateMachineProcessFlags.UnhandledInput;
			}
			return stateMachineProcessFlags;
		}
	}

	public bool active => _stateActive;

	internal bool IsPhysicsBatchDispatchActive
	{
		get
		{
			if (_usingPhysicsBatch)
			{
				return _stateActive;
			}
			return false;
		}
	}

	internal StatePhysicsProcessingEventHandler PhysicsBatchCallback => _onStatePhysicsProcessing;

	public event StateEnteredEventHandler OnStateEntered;

	public event StateExitedEventHandler OnStateExited;

	public event EventReceivedEventHandler OnEventReceived;

	public event StateProcessingEventHandler OnStateProcessing;

	public event StatePhysicsProcessingEventHandler OnStatePhysicsProcessing
	{
		add
		{
			_onStatePhysicsProcessing = (StatePhysicsProcessingEventHandler)Delegate.Combine(_onStatePhysicsProcessing, value);
			StateChartStatePhysicsBatch.UpdateCallback(this, _onStatePhysicsProcessing);
		}
		remove
		{
			_onStatePhysicsProcessing = (StatePhysicsProcessingEventHandler)Delegate.Remove(_onStatePhysicsProcessing, value);
			StateChartStatePhysicsBatch.UpdateCallback(this, _onStatePhysicsProcessing);
		}
	}

	public event StateSteppedEventHandler OnStateStepped;

	public event StateInputEventHandler OnStateInput;

	public event StateUnhandledInputEventHandler OnStateUnhandledInput;

	public event TransitionPendingEventHandler OnTransitionPending;

	protected void RaiseEventReceived(StringName event_)
	{
		OnEventReceived?.Invoke(event_);
	}

	public override void _Ready()
	{
		if (!Engine.IsEditorHint())
		{
			_chart = _FindChart(GetParent());
		}
	}

	private StateChart _FindChart(Node parent)
	{
		while (parent != null)
		{
			if (parent is StateChart result)
			{
				return result;
			}
			parent = parent.GetParent();
		}
		return null;
	}

	public void _RunTransition(Transition transition, bool immediately = false)
	{
		double num = transition.EvaluateDelay();
		if (!immediately && num > 0.0)
		{
			_QueueTransition(transition, num);
		}
		else
		{
			_chart._RunTransition(transition, this);
		}
	}

	public virtual void _StateInit()
	{
		ProcessMode = ProcessModeEnum.Disabled;
		_stateActive = false;
		_ToggleProcessing(isActive: false);
		_transitions.Clear();
		foreach (Node child in GetChildren())
		{
			if (child is Transition item)
			{
				_transitions.Add(item);
			}
		}
	}

	public virtual void _StateEnter(StateChartState transitionTarget)
	{
		_stateActive = true;
		ProcessMode = ProcessModeEnum.Inherit;
		_ToggleProcessing(isActive: true);
		OnStateEntered?.Invoke();
		_ProcessTransitions(StateChart.TriggerType.StateEnter);
	}

	public virtual void _StateExit()
	{
		_pendingTransition = null;
		_pendingTransitionRemainingDelay = 0.0;
		_pendingTransitionInitialDelay = 0.0;
		_stateActive = false;
		ProcessMode = ProcessModeEnum.Disabled;
		_ToggleProcessing(isActive: false);
		OnStateExited?.Invoke();
	}

	public void StateSave(SavedState savedState, int childLevels = -1)
	{
		_StateSave(savedState, childLevels);
	}

	public void StateRestore(SavedState savedState, int childLevels = -1)
	{
		_StateRestore(savedState, childLevels);
	}

	public virtual void _StateSave(SavedState savedState, int childLevels = -1)
	{
		if (!active)
		{
			GD.PushError("_StateSave should only be called if the state is active.");
			return;
		}
		SavedState savedState2 = new SavedState();
		savedState2.pendingTransitionName = ((_pendingTransition != null) ? _pendingTransition.GetPath() : new NodePath());
		savedState2.pendingTransitionRemainingDelay = (float)_pendingTransitionRemainingDelay;
		savedState2.pendingTransitionInitialDelay = (float)_pendingTransitionInitialDelay;
		savedState.AddSubstate(this, savedState2);
		int num;
		switch (childLevels)
		{
		case 0:
			return;
		default:
			num = childLevels - 1;
			break;
		case -1:
			num = -1;
			break;
		}
		int childLevels2 = num;
		foreach (Node child in GetChildren())
		{
			if (child is StateChartState { active: not false } stateChartState)
			{
				stateChartState._StateSave(savedState2, childLevels2);
			}
		}
	}

	public virtual void _StateRestore(SavedState savedState, int childLevels = -1)
	{
		SavedState substateOrNull = savedState.GetSubstateOrNull(this);
		if (substateOrNull == null)
		{
			if (active)
			{
				_StateExit();
			}
			return;
		}
		if (!active)
		{
			_StateEnter(null);
		}
		_pendingTransition = GetNodeOrNull<Transition>(substateOrNull.pendingTransitionName);
		_pendingTransitionRemainingDelay = substateOrNull.pendingTransitionRemainingDelay;
		_pendingTransitionInitialDelay = substateOrNull.pendingTransitionInitialDelay;
		int num;
		switch (childLevels)
		{
		case 0:
			return;
		default:
			num = childLevels - 1;
			break;
		case -1:
			num = -1;
			break;
		}
		int childLevels2 = num;
		foreach (Node child in GetChildren())
		{
			if (child is StateChartState stateChartState)
			{
				stateChartState._StateRestore(substateOrNull, childLevels2);
			}
		}
	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		OnStateProcessing?.Invoke(delta);
		if (_pendingTransition != null)
		{
			_pendingTransitionRemainingDelay -= delta;
			OnTransitionPending?.Invoke(_pendingTransition.delaySeconds, Mathf.Max(0.0, _pendingTransitionRemainingDelay));
			if (_pendingTransitionRemainingDelay <= 0.0)
			{
				Transition pendingTransition = _pendingTransition;
				_pendingTransition = null;
				_pendingTransitionRemainingDelay = 0.0;
				_chart._RunTransition(pendingTransition, this);
			}
		}
	}

	public virtual void _HandleTransition(Transition transition, StateChartState source)
	{
		GD.PushError(string.Concat("State ", Name, " cannot handle transitions."));
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!UsePhysicsBatch || !_usingPhysicsBatch)
		{
			BatchPhysicsProcess(delta);
		}
	}

	public void BatchPhysicsProcess(double delta)
	{
		if (!Engine.IsEditorHint() && active)
		{
			BatchPhysicsProcessValidated(delta);
		}
	}

	internal void BatchPhysicsProcessValidated(double delta)
	{
		InvokeStatePhysicsProcessing(delta);
	}

	private void InvokeStatePhysicsProcessing(double delta)
	{
		StatePhysicsProcessingEventHandler onStatePhysicsProcessing = _onStatePhysicsProcessing;
		if (onStatePhysicsProcessing == null)
		{
			return;
		}
		if (!TowerDefensePerfProfiler.Enabled || !TowerDefensePerfProfiler.DetailedHotPathMetrics)
		{
			onStatePhysicsProcessing(delta);
			return;
		}
		EnsureStatePhysicsCallbackCache(onStatePhysicsProcessing);
		for (int i = 0; i < _statePhysicsCallbackCache.Length; i++)
		{
			StatePhysicsProcessingEventHandler statePhysicsProcessingEventHandler = (StatePhysicsProcessingEventHandler)_statePhysicsCallbackCache[i];
			string name = _statePhysicsCallbackMetricNames[i];
			long startTicks = TowerDefensePerfProfiler.Begin();
			try
			{
				statePhysicsProcessingEventHandler(delta);
			}
			finally
			{
				TowerDefensePerfProfiler.End(name, startTicks);
			}
		}
	}

	private void EnsureStatePhysicsCallbackCache(StatePhysicsProcessingEventHandler handler)
	{
		if (_statePhysicsCallbackCacheSource != handler || _statePhysicsCallbackCache == null || _statePhysicsCallbackMetricNames == null)
		{
			_statePhysicsCallbackCacheSource = handler;
			_statePhysicsCallbackCache = handler.GetInvocationList();
			_statePhysicsCallbackMetricNames = new string[_statePhysicsCallbackCache.Length];
			for (int i = 0; i < _statePhysicsCallbackCache.Length; i++)
			{
				_statePhysicsCallbackMetricNames[i] = TowerDefensePerfProfiler.GetCallbackMetricName("statePhysics", _statePhysicsCallbackCache[i]);
			}
		}
	}

	public virtual void _StateStep()
	{
		OnStateStepped?.Invoke();
	}

	public override void _Input(InputEvent event_)
	{
		OnStateInput?.Invoke(event_);
	}

	public override void _UnhandledInput(InputEvent event_)
	{
		OnStateUnhandledInput?.Invoke(event_);
	}

	public virtual bool _ProcessTransitions(StateChart.TriggerType triggerType, StringName event_ = null)
	{
		if (!active)
		{
			return false;
		}
		if (triggerType == StateChart.TriggerType.Event)
		{
			RaiseEventReceived(event_);
		}
		foreach (Transition transition in _transitions)
		{
			if (transition.IsTriggeredBy(triggerType) && (event_ == null || event_ == (StringName)"" || transition.@event == event_) && transition.EvaluateGuard())
			{
				if (transition != _pendingTransition)
				{
					_RunTransition(transition);
				}
				return true;
			}
		}
		return false;
	}

	private void _QueueTransition(Transition transition, double initialDelay)
	{
		_pendingTransition = transition;
		_pendingTransitionInitialDelay = initialDelay;
		_pendingTransitionRemainingDelay = initialDelay;
		SetProcess(enable: true);
	}

	public override string[] _GetConfigurationWarnings()
	{
		List<string> list = new List<string>();
		Node parent = GetParent();
		bool flag = false;
		while (GodotObject.IsInstanceValid(parent))
		{
			if (parent is StateChart)
			{
				flag = true;
				break;
			}
			parent = parent.GetParent();
		}
		if (!flag)
		{
			list.Add("State is not a child of a StateChart. This will not work.");
		}
		return list.ToArray();
	}

	protected void _ToggleProcessing(bool isActive)
	{
		SetProcess(isActive && OnStateProcessing != null);
		bool flag = isActive && _onStatePhysicsProcessing != null;
		if (UsePhysicsBatch & flag)
		{
			_usingPhysicsBatch = StateChartStatePhysicsBatch.Register(this);
			SetPhysicsProcess(!_usingPhysicsBatch);
		}
		else
		{
			if (_usingPhysicsBatch)
			{
				StateChartStatePhysicsBatch.Unregister(this);
				_usingPhysicsBatch = false;
			}
			SetPhysicsProcess(flag);
		}
		SetProcessInput(isActive && OnStateInput != null);
		SetProcessUnhandledInput(isActive && OnStateUnhandledInput != null);
	}

	public override void _ExitTree()
	{
		if (_usingPhysicsBatch)
		{
			StateChartStatePhysicsBatch.Unregister(this);
			_usingPhysicsBatch = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(25)
		{
			new MethodInfo(MethodName.RaiseEventReceived, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "event_", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._FindChart, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._RunTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "immediately", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._StateInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._StateEnter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transitionTarget", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._StateExit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StateSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "savedState", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "childLevels", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StateRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "savedState", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "childLevels", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._StateSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "savedState", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "childLevels", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._StateRestore, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "savedState", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "childLevels", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._HandleTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._PhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchPhysicsProcess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BatchPhysicsProcessValidated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InvokeStatePhysicsProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._StateStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._UnhandledInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._ProcessTransitions, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "triggerType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "event_", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._QueueTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Float, "initialDelay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GetConfigurationWarnings, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ToggleProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "isActive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RaiseEventReceived && args.Count == 1)
		{
			RaiseEventReceived(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._FindChart && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateChart>(_FindChart(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName._RunTransition && args.Count == 2)
		{
			_RunTransition(VariantUtils.ConvertTo<Transition>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._StateInit && args.Count == 0)
		{
			_StateInit();
			ret = default;
			return true;
		}
		if (method == MethodName._StateEnter && args.Count == 1)
		{
			_StateEnter(VariantUtils.ConvertTo<StateChartState>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._StateExit && args.Count == 0)
		{
			_StateExit();
			ret = default;
			return true;
		}
		if (method == MethodName.StateSave && args.Count == 2)
		{
			StateSave(VariantUtils.ConvertTo<SavedState>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.StateRestore && args.Count == 2)
		{
			StateRestore(VariantUtils.ConvertTo<SavedState>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._StateSave && args.Count == 2)
		{
			_StateSave(VariantUtils.ConvertTo<SavedState>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._StateRestore && args.Count == 2)
		{
			_StateRestore(VariantUtils.ConvertTo<SavedState>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._HandleTransition && args.Count == 2)
		{
			_HandleTransition(VariantUtils.ConvertTo<Transition>(in args[0]), VariantUtils.ConvertTo<StateChartState>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._PhysicsProcess && args.Count == 1)
		{
			_PhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchPhysicsProcess && args.Count == 1)
		{
			BatchPhysicsProcess(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BatchPhysicsProcessValidated && args.Count == 1)
		{
			BatchPhysicsProcessValidated(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InvokeStatePhysicsProcessing && args.Count == 1)
		{
			InvokeStatePhysicsProcessing(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._StateStep && args.Count == 0)
		{
			_StateStep();
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._UnhandledInput && args.Count == 1)
		{
			_UnhandledInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ProcessTransitions && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_ProcessTransitions(VariantUtils.ConvertTo<StateChart.TriggerType>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName._QueueTransition && args.Count == 2)
		{
			_QueueTransition(VariantUtils.ConvertTo<Transition>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(_GetConfigurationWarnings());
			return true;
		}
		if (method == MethodName._ToggleProcessing && args.Count == 1)
		{
			_ToggleProcessing(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.RaiseEventReceived)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._FindChart)
		{
			return true;
		}
		if (method == MethodName._RunTransition)
		{
			return true;
		}
		if (method == MethodName._StateInit)
		{
			return true;
		}
		if (method == MethodName._StateEnter)
		{
			return true;
		}
		if (method == MethodName._StateExit)
		{
			return true;
		}
		if (method == MethodName.StateSave)
		{
			return true;
		}
		if (method == MethodName.StateRestore)
		{
			return true;
		}
		if (method == MethodName._StateSave)
		{
			return true;
		}
		if (method == MethodName._StateRestore)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._HandleTransition)
		{
			return true;
		}
		if (method == MethodName._PhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.BatchPhysicsProcess)
		{
			return true;
		}
		if (method == MethodName.BatchPhysicsProcessValidated)
		{
			return true;
		}
		if (method == MethodName.InvokeStatePhysicsProcessing)
		{
			return true;
		}
		if (method == MethodName._StateStep)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName._UnhandledInput)
		{
			return true;
		}
		if (method == MethodName._ProcessTransitions)
		{
			return true;
		}
		if (method == MethodName._QueueTransition)
		{
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings)
		{
			return true;
		}
		if (method == MethodName._ToggleProcessing)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._stateActive)
		{
			_stateActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._usingPhysicsBatch)
		{
			_usingPhysicsBatch = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._statePhysicsCallbackMetricNames)
		{
			_statePhysicsCallbackMetricNames = VariantUtils.ConvertTo<string[]>(in value);
			return true;
		}
		if (name == PropertyName._pendingTransition)
		{
			_pendingTransition = VariantUtils.ConvertTo<Transition>(in value);
			return true;
		}
		if (name == PropertyName._pendingTransitionRemainingDelay)
		{
			_pendingTransitionRemainingDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._pendingTransitionInitialDelay)
		{
			_pendingTransitionInitialDelay = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._chart)
		{
			_chart = VariantUtils.ConvertTo<StateChart>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ObservedProcessFlags)
		{
			value = VariantUtils.CreateFrom<StateMachineProcessFlags>(ObservedProcessFlags);
			return true;
		}
		bool from;
		if (name == PropertyName.active)
		{
			from = active;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsPhysicsBatchDispatchActive)
		{
			from = IsPhysicsBatchDispatchActive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._stateActive)
		{
			value = VariantUtils.CreateFrom(in _stateActive);
			return true;
		}
		if (name == PropertyName._usingPhysicsBatch)
		{
			value = VariantUtils.CreateFrom(in _usingPhysicsBatch);
			return true;
		}
		if (name == PropertyName._statePhysicsCallbackMetricNames)
		{
			value = VariantUtils.CreateFrom(in _statePhysicsCallbackMetricNames);
			return true;
		}
		if (name == PropertyName._pendingTransition)
		{
			value = VariantUtils.CreateFrom(in _pendingTransition);
			return true;
		}
		if (name == PropertyName._pendingTransitionRemainingDelay)
		{
			value = VariantUtils.CreateFrom(in _pendingTransitionRemainingDelay);
			return true;
		}
		if (name == PropertyName._pendingTransitionInitialDelay)
		{
			value = VariantUtils.CreateFrom(in _pendingTransitionInitialDelay);
			return true;
		}
		if (name == PropertyName._chart)
		{
			value = VariantUtils.CreateFrom(in _chart);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Int, PropertyName.ObservedProcessFlags, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.active, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._stateActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._usingPhysicsBatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsPhysicsBatchDispatchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.PackedStringArray, PropertyName._statePhysicsCallbackMetricNames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pendingTransition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingTransitionRemainingDelay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingTransitionInitialDelay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._chart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._stateActive, Variant.From(in _stateActive));
		info.AddProperty(PropertyName._usingPhysicsBatch, Variant.From(in _usingPhysicsBatch));
		info.AddProperty(PropertyName._statePhysicsCallbackMetricNames, Variant.From(in _statePhysicsCallbackMetricNames));
		info.AddProperty(PropertyName._pendingTransition, Variant.From(in _pendingTransition));
		info.AddProperty(PropertyName._pendingTransitionRemainingDelay, Variant.From(in _pendingTransitionRemainingDelay));
		info.AddProperty(PropertyName._pendingTransitionInitialDelay, Variant.From(in _pendingTransitionInitialDelay));
		info.AddProperty(PropertyName._chart, Variant.From(in _chart));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._stateActive, out var value))
		{
			_stateActive = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._usingPhysicsBatch, out var value2))
		{
			_usingPhysicsBatch = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._statePhysicsCallbackMetricNames, out var value3))
		{
			_statePhysicsCallbackMetricNames = value3.As<string[]>();
		}
		if (info.TryGetProperty(PropertyName._pendingTransition, out var value4))
		{
			_pendingTransition = value4.As<Transition>();
		}
		if (info.TryGetProperty(PropertyName._pendingTransitionRemainingDelay, out var value5))
		{
			_pendingTransitionRemainingDelay = value5.As<double>();
		}
		if (info.TryGetProperty(PropertyName._pendingTransitionInitialDelay, out var value6))
		{
			_pendingTransitionInitialDelay = value6.As<double>();
		}
		if (info.TryGetProperty(PropertyName._chart, out var value7))
		{
			_chart = value7.As<StateChart>();
		}
	}
}
