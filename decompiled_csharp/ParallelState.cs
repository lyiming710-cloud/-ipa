using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[Icon("res://addons/godot_state_charts/parallel_state.svg")]
[ScriptPath("res://addons/godot_state_charts/ParallelState.cs")]
public class ParallelState : StateChartState
{
	public new class MethodName : StateChartState.MethodName
	{
		public new static readonly StringName _StateInit = "_StateInit";

		public new static readonly StringName _HandleTransition = "_HandleTransition";

		public new static readonly StringName _StateEnter = "_StateEnter";

		public new static readonly StringName _StateExit = "_StateExit";

		public new static readonly StringName _StateStep = "_StateStep";

		public new static readonly StringName _ProcessTransitions = "_ProcessTransitions";

		public new static readonly StringName _GetConfigurationWarnings = "_GetConfigurationWarnings";
	}

	public new class PropertyName : StateChartState.PropertyName
	{
	}

	public new class SignalName : StateChartState.SignalName
	{
	}

	private List<StateChartState> _subStates = new List<StateChartState>();

	public override void _StateInit()
	{
		base._StateInit();
		foreach (Node child in GetChildren())
		{
			if (child is StateChartState stateChartState)
			{
				_subStates.Add(stateChartState);
				stateChartState._StateInit();
			}
		}
	}

	public override void _HandleTransition(Transition transition, StateChartState source)
	{
		StateChartState stateChartState = transition.ResolveTarget();
		if (stateChartState == null)
		{
			GD.PushError(string.Concat(new string[5]
			{
				"The target state '",
				transition.to.ToString(),
				"' of the transition from '",
				source.Name,
				"' is not a state."
			}));
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
			return;
		}
		if (IsAncestorOf(stateChartState))
		{
			foreach (Node child2 in GetChildren())
			{
				if (child2 is StateChartState stateChartState2 && stateChartState2.IsAncestorOf(stateChartState))
				{
					stateChartState2._HandleTransition(transition, source);
					break;
				}
			}
			return;
		}
		((StateChartState)GetParent())._HandleTransition(transition, source);
	}

	public override void _StateEnter(StateChartState transitionTarget)
	{
		base._StateEnter(transitionTarget);
		foreach (StateChartState subState in _subStates)
		{
			subState._StateEnter(transitionTarget);
		}
	}

	public override void _StateExit()
	{
		foreach (StateChartState subState in _subStates)
		{
			subState._StateExit();
		}
		base._StateExit();
	}

	public override void _StateStep()
	{
		base._StateStep();
		foreach (StateChartState subState in _subStates)
		{
			subState._StateStep();
		}
	}

	public override bool _ProcessTransitions(StateChart.TriggerType triggerType, StringName event_ = null)
	{
		if (!active)
		{
			return false;
		}
		bool flag = false;
		foreach (StateChartState subState in _subStates)
		{
			bool flag2 = subState._ProcessTransitions(triggerType, event_);
			flag |= flag2;
		}
		if (flag)
		{
			if (triggerType == StateChart.TriggerType.Event)
			{
				RaiseEventReceived(event_);
			}
			return true;
		}
		return base._ProcessTransitions(triggerType, event_);
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
		if (num < 2)
		{
			list.Add("Parallel states should have at least two child states.");
		}
		return list.ToArray();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._StateInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._HandleTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._StateEnter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transitionTarget", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._StateExit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._StateStep, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ProcessTransitions, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "triggerType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "event_", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GetConfigurationWarnings, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._StateInit && args.Count == 0)
		{
			_StateInit();
			ret = default;
			return true;
		}
		if (method == MethodName._HandleTransition && args.Count == 2)
		{
			_HandleTransition(VariantUtils.ConvertTo<Transition>(in args[0]), VariantUtils.ConvertTo<StateChartState>(in args[1]));
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
		if (method == MethodName._StateStep && args.Count == 0)
		{
			_StateStep();
			ret = default;
			return true;
		}
		if (method == MethodName._ProcessTransitions && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_ProcessTransitions(VariantUtils.ConvertTo<StateChart.TriggerType>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
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
		if (method == MethodName._StateInit)
		{
			return true;
		}
		if (method == MethodName._HandleTransition)
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
		if (method == MethodName._StateStep)
		{
			return true;
		}
		if (method == MethodName._ProcessTransitions)
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
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
	}
}
