using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Tool]
[Icon("res://addons/godot_state_charts/animation_tree_state.svg")]
[ScriptPath("res://addons/godot_state_charts/AnimationTreeState.cs")]
public class AnimationTreeState : AtomicState
{
	public new class MethodName : AtomicState.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _StateEnter = "_StateEnter";

		public new static readonly StringName _GetConfigurationWarnings = "_GetConfigurationWarnings";
	}

	public new class PropertyName : AtomicState.PropertyName
	{
		public static readonly StringName animationTree = "animationTree";

		public static readonly StringName _animationTreePath = "_animationTreePath";

		public static readonly StringName stateName = "stateName";

		public static readonly StringName _animationTreeStateMachine = "_animationTreeStateMachine";
	}

	public new class SignalName : AtomicState.SignalName
	{
	}

	private NodePath _animationTreePath = new NodePath();

	[Export(PropertyHint.None, "")]
	public StringName stateName = "";

	private AnimationNodeStateMachinePlayback _animationTreeStateMachine;

	[Export(PropertyHint.None, "")]
	public NodePath animationTree
	{
		get
		{
			return _animationTreePath;
		}
		set
		{
			_animationTreePath = value;
			UpdateConfigurationWarnings();
		}
	}

	public override void _Ready()
	{
		if (Engine.IsEditorHint())
		{
			return;
		}
		base._Ready();
		_animationTreeStateMachine = null;
		AnimationTree nodeOrNull = GetNodeOrNull<AnimationTree>(_animationTreePath);
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			Variant variant = nodeOrNull.Get("parameters/playback");
			if (variant.VariantType == Variant.Type.Nil)
			{
				GD.PushError("The animation tree does not have a state machine as root node. This node will not work.");
				return;
			}
			_animationTreeStateMachine = variant.As<AnimationNodeStateMachinePlayback>();
			if (_animationTreeStateMachine == null)
			{
				GD.PushError("The animation tree does not have a state machine as root node. This node will not work.");
			}
		}
		else
		{
			GD.PushError("The animation tree is invalid. This node will not work.");
		}
	}

	public override void _StateEnter(StateChartState transitionTarget)
	{
		base._StateEnter(transitionTarget);
		if (GodotObject.IsInstanceValid(_animationTreeStateMachine))
		{
			StringName name = stateName;
			if (name == (StringName)"")
			{
				name = Name;
			}
			_animationTreeStateMachine.Travel(name);
		}
	}

	public override string[] _GetConfigurationWarnings()
	{
		List<string> list = new List<string>(base._GetConfigurationWarnings());
		list.Add("This node is deprecated and will be removed in a future version.");
		if (_animationTreePath == null || _animationTreePath == (NodePath)"")
		{
			list.Add("No animation tree is set.");
		}
		else if (GetNodeOrNull(_animationTreePath) == null)
		{
			list.Add("The animation tree path is invalid.");
		}
		return list.ToArray();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._StateEnter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "transitionTarget", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName._StateEnter && args.Count == 1)
		{
			_StateEnter(VariantUtils.ConvertTo<StateChartState>(in args[0]));
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
		if (method == MethodName._StateEnter)
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
		if (name == PropertyName.animationTree)
		{
			animationTree = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName._animationTreePath)
		{
			_animationTreePath = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.stateName)
		{
			stateName = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._animationTreeStateMachine)
		{
			_animationTreeStateMachine = VariantUtils.ConvertTo<AnimationNodeStateMachinePlayback>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.animationTree)
		{
			value = VariantUtils.CreateFrom<NodePath>(animationTree);
			return true;
		}
		if (name == PropertyName._animationTreePath)
		{
			value = VariantUtils.CreateFrom(in _animationTreePath);
			return true;
		}
		if (name == PropertyName.stateName)
		{
			value = VariantUtils.CreateFrom(in stateName);
			return true;
		}
		if (name == PropertyName._animationTreeStateMachine)
		{
			value = VariantUtils.CreateFrom(in _animationTreeStateMachine);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.NodePath, PropertyName.animationTree, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName._animationTreePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.stateName, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationTreeStateMachine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.animationTree, Variant.From<NodePath>(animationTree));
		info.AddProperty(PropertyName._animationTreePath, Variant.From(in _animationTreePath));
		info.AddProperty(PropertyName.stateName, Variant.From(in stateName));
		info.AddProperty(PropertyName._animationTreeStateMachine, Variant.From(in _animationTreeStateMachine));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.animationTree, out var value))
		{
			animationTree = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName._animationTreePath, out var value2))
		{
			_animationTreePath = value2.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.stateName, out var value3))
		{
			stateName = value3.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._animationTreeStateMachine, out var value4))
		{
			_animationTreeStateMachine = value4.As<AnimationNodeStateMachinePlayback>();
		}
	}
}
