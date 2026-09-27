using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

[GlobalClass]
[Icon("res://addons/godot_state_charts/transition.svg")]
[Tool]
[ScriptPath("res://addons/godot_state_charts/Transition.cs")]
public class Transition : Node
{
	public delegate void TakenEventHandler();

	public new class MethodName : Node.MethodName
	{
		public static readonly StringName EmitTaken = "EmitTaken";

		public static readonly StringName IsTriggeredBy = "IsTriggeredBy";

		public static readonly StringName Take = "Take";

		public static readonly StringName EvaluateGuard = "EvaluateGuard";

		public static readonly StringName EvaluateDelay = "EvaluateDelay";

		public static readonly StringName ResolveTarget = "ResolveTarget";

		public new static readonly StringName _GetConfigurationWarnings = "_GetConfigurationWarnings";

		public static readonly StringName _RefreshCaches = "_RefreshCaches";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName to = "to";

		public static readonly StringName @event = "event";

		public static readonly StringName guard = "guard";

		public static readonly StringName delaySeconds = "delaySeconds";

		public static readonly StringName delayInSeconds = "delayInSeconds";

		public static readonly StringName hasEvent = "hasEvent";

		public static readonly StringName _dirty = "_dirty";

		public static readonly StringName _target = "_target";

		public static readonly StringName _supportedTriggerTypes = "_supportedTriggerTypes";

		public static readonly StringName _to = "_to";

		public static readonly StringName _event = "_event";

		public static readonly StringName _guard = "_guard";

		public static readonly StringName _delayInSeconds = "_delayInSeconds";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private bool _dirty = true;

	private StateChartState _target;

	private int _supportedTriggerTypes;

	private NodePath _to = new NodePath();

	private StringName _event = new StringName();

	private Guard _guard;

	private string _delayInSeconds = "0.0";

	[Export(PropertyHint.None, "")]
	public NodePath to
	{
		get
		{
			return _to;
		}
		set
		{
			_to = value;
			_dirty = true;
			UpdateConfigurationWarnings();
		}
	}

	[Export(PropertyHint.None, "")]
	public StringName @event
	{
		get
		{
			return _event;
		}
		set
		{
			_event = value;
			_dirty = true;
			UpdateConfigurationWarnings();
		}
	}

	[Export(PropertyHint.None, "")]
	public Guard guard
	{
		get
		{
			return _guard;
		}
		set
		{
			_guard = value;
			_dirty = true;
			UpdateConfigurationWarnings();
		}
	}

	public float delaySeconds
	{
		get
		{
			if (float.TryParse(delayInSeconds, out var result))
			{
				return result;
			}
			return 0f;
		}
		set
		{
			delayInSeconds = value.ToString();
		}
	}

	[Export(PropertyHint.Expression, "")]
	public string delayInSeconds
	{
		get
		{
			return _delayInSeconds;
		}
		set
		{
			_delayInSeconds = value;
			UpdateConfigurationWarnings();
		}
	}

	public bool hasEvent
	{
		get
		{
			if (_event != null)
			{
				return _event != (StringName)"";
			}
			return false;
		}
	}

	public event TakenEventHandler OnTaken;

	public void EmitTaken()
	{
		OnTaken?.Invoke();
	}

	public bool IsTriggeredBy(StateChart.TriggerType triggerType)
	{
		if (_dirty)
		{
			_RefreshCaches();
		}
		return ((uint)_supportedTriggerTypes & (uint)triggerType) != 0;
	}

	public void Take(bool immediately = true)
	{
		if (!(GetParent() is StateChartState stateChartState))
		{
			GD.PushError("Transitions must be children of states.");
		}
		else
		{
			stateChartState._RunTransition(this, immediately);
		}
	}

	public bool EvaluateGuard()
	{
		if (_guard == null)
		{
			return true;
		}
		if (!(GetParent() is StateChartState))
		{
			GD.PushError("Transitions must be children of states.");
			return false;
		}
		return _guard.IsSatisfied(this, GetParent() as StateChartState);
	}

	public float EvaluateDelay()
	{
		if (float.TryParse(_delayInSeconds, out var result))
		{
			return result;
		}
		if (!(GetParent() is StateChartState stateChartState))
		{
			GD.PushError("Transitions must be children of states.");
			return 0f;
		}
		Variant variant = ExpressionUtil.EvaluateExpression("delay of " + DebugUtil.PathOf(this), stateChartState._chart, _delayInSeconds, 0.0);
		if (variant.VariantType != Variant.Type.Float)
		{
			GD.PushError("Expression: ", _delayInSeconds, " result: ", variant, " is not a float. Returning 0.0.");
			return 0f;
		}
		return variant.As<float>();
	}

	public StateChartState ResolveTarget()
	{
		if (_dirty)
		{
			_RefreshCaches();
		}
		return _target;
	}

	public override string[] _GetConfigurationWarnings()
	{
		List<string> list = new List<string>();
		if (GetChildCount() > 0)
		{
			list.Add("Transitions should not have children");
		}
		if (_to == null || _to == (NodePath)"")
		{
			list.Add("The target state is not set");
		}
		else if (ResolveTarget() == null)
		{
			list.Add("The target state " + _to.ToString() + " could not be found");
		}
		if (!(GetParent() is StateChartState))
		{
			list.Add("Transitions must be children of states.");
		}
		if (string.IsNullOrWhiteSpace(_delayInSeconds))
		{
			list.Add("Delay must be a valid expression. Use 0.0 if you want no delay.");
		}
		return list.ToArray();
	}

	private void _RefreshCaches()
	{
		_dirty = false;
		bool num = _event == null || _event == (StringName)"";
		if (_to != null && _to != (NodePath)"" && GetNodeOrNull(_to) is StateChartState target)
		{
			_target = target;
		}
		_supportedTriggerTypes = 0;
		if (!num)
		{
			_supportedTriggerTypes |= 1;
			return;
		}
		_supportedTriggerTypes |= 2;
		if (_guard != null)
		{
			_supportedTriggerTypes |= _guard.GetSupportedTriggerTypes();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName.EmitTaken, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsTriggeredBy, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "triggerType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Take, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "immediately", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EvaluateGuard, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EvaluateDelay, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveTarget, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._GetConfigurationWarnings, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._RefreshCaches, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EmitTaken && args.Count == 0)
		{
			EmitTaken();
			ret = default;
			return true;
		}
		if (method == MethodName.IsTriggeredBy && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTriggeredBy(VariantUtils.ConvertTo<StateChart.TriggerType>(in args[0])));
			return true;
		}
		if (method == MethodName.Take && args.Count == 1)
		{
			Take(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EvaluateGuard && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(EvaluateGuard());
			return true;
		}
		if (method == MethodName.EvaluateDelay && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<float>(EvaluateDelay());
			return true;
		}
		if (method == MethodName.ResolveTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StateChartState>(ResolveTarget());
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(_GetConfigurationWarnings());
			return true;
		}
		if (method == MethodName._RefreshCaches && args.Count == 0)
		{
			_RefreshCaches();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.EmitTaken)
		{
			return true;
		}
		if (method == MethodName.IsTriggeredBy)
		{
			return true;
		}
		if (method == MethodName.Take)
		{
			return true;
		}
		if (method == MethodName.EvaluateGuard)
		{
			return true;
		}
		if (method == MethodName.EvaluateDelay)
		{
			return true;
		}
		if (method == MethodName.ResolveTarget)
		{
			return true;
		}
		if (method == MethodName._GetConfigurationWarnings)
		{
			return true;
		}
		if (method == MethodName._RefreshCaches)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.to)
		{
			to = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName.@event)
		{
			@event = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName.guard)
		{
			guard = VariantUtils.ConvertTo<Guard>(in value);
			return true;
		}
		if (name == PropertyName.delaySeconds)
		{
			delaySeconds = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.delayInSeconds)
		{
			delayInSeconds = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._dirty)
		{
			_dirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._target)
		{
			_target = VariantUtils.ConvertTo<StateChartState>(in value);
			return true;
		}
		if (name == PropertyName._supportedTriggerTypes)
		{
			_supportedTriggerTypes = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._to)
		{
			_to = VariantUtils.ConvertTo<NodePath>(in value);
			return true;
		}
		if (name == PropertyName._event)
		{
			_event = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._guard)
		{
			_guard = VariantUtils.ConvertTo<Guard>(in value);
			return true;
		}
		if (name == PropertyName._delayInSeconds)
		{
			_delayInSeconds = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.to)
		{
			value = VariantUtils.CreateFrom<NodePath>(to);
			return true;
		}
		if (name == PropertyName.@event)
		{
			value = VariantUtils.CreateFrom<StringName>(@event);
			return true;
		}
		if (name == PropertyName.guard)
		{
			value = VariantUtils.CreateFrom<Guard>(guard);
			return true;
		}
		if (name == PropertyName.delaySeconds)
		{
			value = VariantUtils.CreateFrom<float>(delaySeconds);
			return true;
		}
		if (name == PropertyName.delayInSeconds)
		{
			value = VariantUtils.CreateFrom<string>(delayInSeconds);
			return true;
		}
		if (name == PropertyName.hasEvent)
		{
			value = VariantUtils.CreateFrom<bool>(hasEvent);
			return true;
		}
		if (name == PropertyName._dirty)
		{
			value = VariantUtils.CreateFrom(in _dirty);
			return true;
		}
		if (name == PropertyName._target)
		{
			value = VariantUtils.CreateFrom(in _target);
			return true;
		}
		if (name == PropertyName._supportedTriggerTypes)
		{
			value = VariantUtils.CreateFrom(in _supportedTriggerTypes);
			return true;
		}
		if (name == PropertyName._to)
		{
			value = VariantUtils.CreateFrom(in _to);
			return true;
		}
		if (name == PropertyName._event)
		{
			value = VariantUtils.CreateFrom(in _event);
			return true;
		}
		if (name == PropertyName._guard)
		{
			value = VariantUtils.CreateFrom(in _guard);
			return true;
		}
		if (name == PropertyName._delayInSeconds)
		{
			value = VariantUtils.CreateFrom(in _delayInSeconds);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Bool, PropertyName._dirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._target, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._supportedTriggerTypes, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.NodePath, PropertyName.to, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.NodePath, PropertyName._to, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.@event, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.StringName, PropertyName._event, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.guard, PropertyHint.ResourceType, "Guard", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._guard, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.delaySeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.delayInSeconds, PropertyHint.Expression, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.String, PropertyName._delayInSeconds, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.hasEvent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.to, Variant.From<NodePath>(to));
		info.AddProperty(PropertyName.@event, Variant.From<StringName>(@event));
		info.AddProperty(PropertyName.guard, Variant.From<Guard>(guard));
		info.AddProperty(PropertyName.delaySeconds, Variant.From<float>(delaySeconds));
		info.AddProperty(PropertyName.delayInSeconds, Variant.From<string>(delayInSeconds));
		info.AddProperty(PropertyName._dirty, Variant.From(in _dirty));
		info.AddProperty(PropertyName._target, Variant.From(in _target));
		info.AddProperty(PropertyName._supportedTriggerTypes, Variant.From(in _supportedTriggerTypes));
		info.AddProperty(PropertyName._to, Variant.From(in _to));
		info.AddProperty(PropertyName._event, Variant.From(in _event));
		info.AddProperty(PropertyName._guard, Variant.From(in _guard));
		info.AddProperty(PropertyName._delayInSeconds, Variant.From(in _delayInSeconds));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.to, out var value))
		{
			to = value.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName.@event, out var value2))
		{
			@event = value2.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName.guard, out var value3))
		{
			guard = value3.As<Guard>();
		}
		if (info.TryGetProperty(PropertyName.delaySeconds, out var value4))
		{
			delaySeconds = value4.As<float>();
		}
		if (info.TryGetProperty(PropertyName.delayInSeconds, out var value5))
		{
			delayInSeconds = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName._dirty, out var value6))
		{
			_dirty = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._target, out var value7))
		{
			_target = value7.As<StateChartState>();
		}
		if (info.TryGetProperty(PropertyName._supportedTriggerTypes, out var value8))
		{
			_supportedTriggerTypes = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName._to, out var value9))
		{
			_to = value9.As<NodePath>();
		}
		if (info.TryGetProperty(PropertyName._event, out var value10))
		{
			_event = value10.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._guard, out var value11))
		{
			_guard = value11.As<Guard>();
		}
		if (info.TryGetProperty(PropertyName._delayInSeconds, out var value12))
		{
			_delayInSeconds = value12.As<string>();
		}
	}
}
