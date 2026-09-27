using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[Icon("res://addons/godot_state_charts/atomic_state.svg")]
[ScriptPath("res://addons/godot_state_charts/AtomicState.cs")]
public class AtomicState : StateChartState
{
	public new class MethodName : StateChartState.MethodName
	{
		public new static readonly StringName _HandleTransition = "_HandleTransition";

		public new static readonly StringName _GetConfigurationWarnings = "_GetConfigurationWarnings";
	}

	public new class PropertyName : StateChartState.PropertyName
	{
	}

	public new class SignalName : StateChartState.SignalName
	{
	}

	public override void _HandleTransition(Transition transition, StateChartState source)
	{
		if (transition.ResolveTarget() == null)
		{
			GD.PushError(string.Concat(new string[5]
			{
				"The target state '",
				transition.to.ToString(),
				"' of the transition from '",
				source.Name,
				"' is not a state."
			}));
		}
		else
		{
			((StateChartState)GetParent())._HandleTransition(transition, source);
		}
	}

	public override string[] _GetConfigurationWarnings()
	{
		List<string> list = new List<string>(base._GetConfigurationWarnings());
		foreach (Node child in GetChildren())
		{
			if (child is StateChartState)
			{
				list.Add("Atomic states cannot have child states. These will be ignored.");
				break;
			}
		}
		return list.ToArray();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(MethodName._HandleTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName._GetConfigurationWarnings, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._HandleTransition && args.Count == 2)
		{
			_HandleTransition(VariantUtils.ConvertTo<Transition>(in args[0]), VariantUtils.ConvertTo<StateChartState>(in args[1]));
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
		if (method == MethodName._HandleTransition)
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
