using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[Icon("res://addons/godot_state_charts/compound_state.svg")]
[ScriptPath("res://addons/godot_state_charts/CompoundState.cs")]
public class CompoundState : StateChartState
{
	public delegate void ChildStateEnteredEventHandler();

	public delegate void ChildStateExitedEventHandler();

	public new class MethodName : StateChartState.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _StateInit = "_StateInit";

		public new static readonly StringName _StateEnter = "_StateEnter";

		public new static readonly StringName _StateStep = "_StateStep";

		public new static readonly StringName _StateSave = "_StateSave";

		public new static readonly StringName _StateRestore = "_StateRestore";

		public new static readonly StringName _StateExit = "_StateExit";

		public new static readonly StringName _ProcessTransitions = "_ProcessTransitions";

		public new static readonly StringName _HandleTransition = "_HandleTransition";

		public static readonly StringName _RestoreHistoryState = "_RestoreHistoryState";

		public new static readonly StringName _GetConfigurationWarnings = "_GetConfigurationWarnings";
	}

	public new class PropertyName : StateChartState.PropertyName
	{
		public static readonly StringName initialState = "initialState";

		public static readonly StringName activeState = "activeState";

		public static readonly StringName _initialStatePath = "_initialStatePath";

		public static readonly StringName _activeState = "_activeState";

		public static readonly StringName _initialState = "_initialState";

		public static readonly StringName _needsDeepHistory = "_needsDeepHistory";
	}

	public new class SignalName : StateChartState.SignalName
	{
	}

	private NodePath _initialStatePath = new NodePath();

	public StateChartState _activeState;

	private StateChartState _initialState;

	private List<HistoryState> _historyStates = new List<HistoryState>();

	private bool _needsDeepHistory;

	[Export(PropertyHint.None, "")]
	public NodePath initialState
	{
		get
		{
			return _initialStatePath;
		}
		set
		{
			_initialStatePath = value;
			UpdateConfigurationWarnings();
		}
	}

	public StateChartState activeState
	{
		get
		{
			if (_activeState == null || !GodotObject.IsInstanceValid(_activeState))
			{
				return null;
			}
			return _activeState;
		}
	}

	public event ChildStateEnteredEventHandler OnChildStateEntered;

	public event ChildStateExitedEventHandler OnChildStateExited;

	public override void _Ready()
	{
		base._Ready();
		if (Engine.IsEditorHint())
		{
			ChildEnteredTree += (Node child) =>
			{
				if (child is StateChartState && (_initialStatePath == null || _initialStatePath == (NodePath)""))
				{
					Callable.From(() =>
					{
						initialState = GetPathTo(child);
					}).CallDeferred();
				}
			};
		}
		_initialState = GetNodeOrNull<StateChartState>(_initialStatePath);
	}

	public override void _StateInit()
	{
		base._StateInit();
		foreach (Node child in GetChildren())
		{
			if (child is HistoryState historyState)
			{
				_historyStates.Add(historyState);
				_needsDeepHistory = _needsDeepHistory || historyState.deep;
			}
		}
		foreach (Node child2 in GetChildren())
		{
			if (child2 is StateChartState stateChartState)
			{
				stateChartState._StateInit();
				stateChartState.OnStateEntered += () =>
				{
					OnChildStateEntered?.Invoke();
				};
				stateChartState.OnStateExited += () =>
				{
					OnChildStateExited?.Invoke();
				};
			}
		}
	}

	public override void _StateEnter(StateChartState transitionTarget)
	{
		base._StateEnter(transitionTarget);
		bool flag = false;
		if (transitionTarget != null && IsAncestorOf(transitionTarget))
		{
			flag = true;
		}
		if (flag || GodotObject.IsInstanceValid(_activeState) || !_stateActive)
		{
			return;
		}
		if (_initialState != null)
		{
			if (_initialState is HistoryState)
			{
				_RestoreHistoryState((HistoryState)_initialState);
				return;
			}
			_activeState = _initialState;
			_activeState._StateEnter(null);
		}
		else
		{
			GD.PushError(string.Concat("No initial state set for state '", Name, "'."));
		}
	}

	public override void _StateStep()
	{
		base._StateStep();
		if (_activeState != null)
		{
			_activeState._StateStep();
		}
	}

	public override void _StateSave(SavedState savedState, int childLevels = -1)
	{
		base._StateSave(savedState, childLevels);
		SavedState substateOrNull = savedState.GetSubstateOrNull(this);
		if (substateOrNull == null)
		{
			GD.PushError(string.Concat("Probably a bug: The state of '", Name, "' was not saved."));
			return;
		}
		foreach (HistoryState historyState in _historyStates)
		{
			historyState._StateSave(substateOrNull, childLevels);
		}
	}

	public override void _StateRestore(SavedState savedState, int childLevels = -1)
	{
		base._StateRestore(savedState, childLevels);
		if (!active)
		{
			return;
		}
		foreach (Node child in GetChildren())
		{
			if (child is StateChartState { active: not false } stateChartState)
			{
				_activeState = stateChartState;
				break;
			}
		}
	}

	public override void _StateExit()
	{
		if (_historyStates.Count > 0)
		{
			SavedState savedState = new SavedState();
			_StateSave(savedState, (!_needsDeepHistory) ? 1 : (-1));
			foreach (HistoryState historyState in _historyStates)
			{
				historyState.history = savedState;
			}
		}
		if (_activeState != null)
		{
			_activeState._StateExit();
			_activeState = null;
		}
		base._StateExit();
	}

	public override bool _ProcessTransitions(StateChart.TriggerType triggerType, StringName event_ = null)
	{
		if (!active)
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(_activeState) && _activeState._ProcessTransitions(triggerType, event_))
		{
			if (triggerType == StateChart.TriggerType.Event)
			{
				RaiseEventReceived(event_);
			}
			return true;
		}
		return base._ProcessTransitions(triggerType, event_);
	}

	public override void _HandleTransition(Transition transition, StateChartState source)
	{
		StateChartState stateChartState = transition.ResolveTarget();
		if (stateChartState == null)
		{
			GD.PushError($"[CompoundState] 目标状态 '{transition.to}' 不是 StateChartState, target={stateChartState?.GetType().Name ?? "null"}");
			return;
		}
		if (stateChartState == this)
		{
			_StateExit();
			_StateEnter(stateChartState);
			return;
		}
		bool flag = false;
		foreach (Node child in GetChildren())
		{
			if (child == stateChartState)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (GodotObject.IsInstanceValid(_activeState))
			{
				_activeState._StateExit();
			}
			if (stateChartState is HistoryState)
			{
				_RestoreHistoryState((HistoryState)stateChartState);
				return;
			}
			_activeState = stateChartState;
			_activeState._StateEnter(stateChartState);
			return;
		}
		if (IsAncestorOf(stateChartState))
		{
			foreach (Node child2 in GetChildren())
			{
				if (child2 is StateChartState stateChartState2 && stateChartState2.IsAncestorOf(stateChartState))
				{
					if (_activeState != child2)
					{
						if (GodotObject.IsInstanceValid(_activeState))
						{
							_activeState._StateExit();
						}
						_activeState = stateChartState2;
						_activeState._StateEnter(stateChartState);
					}
					stateChartState2._HandleTransition(transition, source);
					break;
				}
			}
			return;
		}
		((StateChartState)GetParent())._HandleTransition(transition, source);
	}

	public void _RestoreHistoryState(HistoryState target)
	{
		SavedState history = target.history;
		if (history != null)
		{
			_StateRestore(history, (!target.deep) ? 1 : (-1));
			return;
		}
		StateChartState nodeOrNull = target.GetNodeOrNull<StateChartState>(target.defaultState);
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			_activeState = nodeOrNull;
			_activeState._StateEnter(null);
			return;
		}
		GD.PushError(string.Concat(new string[5]
		{
			"The default state '",
			target.defaultState.ToString(),
			"' of the history state '",
			target.Name,
			"' cannot be found."
		}));
	}

	public override string[] _GetConfigurationWarnings()
	{
		List<string> list = new List<string>(base._GetConfigurationWarnings());
		int num = 0;
		foreach (Node child in GetChildren())
		{
			if (child is StateChartState)
			{
				num++;
			}
		}
		if (num < 1)
		{
			list.Add("Compound states should have at least one child state.");
		}
		else if (num < 2)
		{
			list.Add("Compound states with only one child state are not very useful. Consider adding more child states or removing this compound state.");
		}
		StateChartState nodeOrNull = GetNodeOrNull<StateChartState>(_initialStatePath);
		if (!GodotObject.IsInstanceValid(nodeOrNull))
		{
			list.Add("Initial state could not be resolved, is the path correct?");
		}
		else if (nodeOrNull.GetParent() != this)
		{
			list.Add("Initial state must be a direct child of this compound state.");
		}
		return list.ToArray();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(11)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._StateInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._StateEnter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transitionTarget", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._StateStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
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
			new MethodInfo(MethodName._StateExit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ProcessTransitions, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "triggerType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "event_", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._HandleTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._RestoreHistoryState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._GetConfigurationWarnings, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._StateStep && args.Count == 0)
		{
			_StateStep();
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
		if (method == MethodName._StateExit && args.Count == 0)
		{
			_StateExit();
			ret = default;
			return true;
		}
		if (method == MethodName._ProcessTransitions && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_ProcessTransitions(VariantUtils.ConvertTo<StateChart.TriggerType>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName._HandleTransition && args.Count == 2)
		{
			_HandleTransition(VariantUtils.ConvertTo<Transition>(in args[0]), VariantUtils.ConvertTo<StateChartState>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._RestoreHistoryState && args.Count == 1)
		{
			_RestoreHistoryState(VariantUtils.ConvertTo<HistoryState>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(_GetConfigurationWarnings());
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
		if (method == MethodName._StateInit)
		{
			return true;
		}
		if (method == MethodName._StateEnter)
		{
			return true;
		}
		if (method == MethodName._StateStep)
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
		if (method == MethodName._StateExit)
		{
			return true;
		}
		if (method == MethodName._ProcessTransitions)
		{
			return true;
		}
		if (method == MethodName._HandleTransition)
		{
			return true;
		}
		if (method == MethodName._RestoreHistoryState)
		{
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.initialState)
		{
			initialState = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName._initialStatePath)
		{
			_initialStatePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName._activeState)
		{
			_activeState = VariantUtils.ConvertTo<StateChartState>(in value);
			return true;
		}
		if (name == PropertyName._initialState)
		{
			_initialState = VariantUtils.ConvertTo<StateChartState>(in value);
			return true;
		}
		if (name == PropertyName._needsDeepHistory)
		{
			_needsDeepHistory = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.initialState)
		{
			value = VariantUtils.CreateFrom<NodePath>(initialState);
			return true;
		}
		if (name == PropertyName.activeState)
		{
			value = VariantUtils.CreateFrom<StateChartState>(activeState);
			return true;
		}
		if (name == PropertyName._initialStatePath)
		{
			value = VariantUtils.CreateFrom(in _initialStatePath);
			return true;
		}
		if (name == PropertyName._activeState)
		{
			value = VariantUtils.CreateFrom(in _activeState);
			return true;
		}
		if (name == PropertyName._initialState)
		{
			value = VariantUtils.CreateFrom(in _initialState);
			return true;
		}
		if (name == PropertyName._needsDeepHistory)
		{
			value = VariantUtils.CreateFrom(in _needsDeepHistory);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.NodePath, PropertyName.initialState, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName._initialStatePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._activeState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.activeState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._initialState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._needsDeepHistory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.initialState, Variant.From<NodePath>(initialState));
		info.AddProperty(PropertyName._initialStatePath, Variant.From(in _initialStatePath));
		info.AddProperty(PropertyName._activeState, Variant.From(in _activeState));
		info.AddProperty(PropertyName._initialState, Variant.From(in _initialState));
		info.AddProperty(PropertyName._needsDeepHistory, Variant.From(in _needsDeepHistory));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.initialState, out var value))
		{
			initialState = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName._initialStatePath, out var value2))
		{
			_initialStatePath = value2.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName._activeState, out var value3))
		{
			_activeState = value3.As<StateChartState>();
		}
		if (info.TryGetProperty(PropertyName._initialState, out var value4))
		{
			_initialState = value4.As<StateChartState>();
		}
		if (info.TryGetProperty(PropertyName._needsDeepHistory, out var value5))
		{
			_needsDeepHistory = value5.As<bool>();
		}
	}
}
