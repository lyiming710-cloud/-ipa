using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/RefCounted/Validator/XWBPRealTimeValidator.cs")]
public class XWBPRealTimeValidator : RefCounted
{
	[Signal]
	public delegate void ValidationCompleteEventHandler(Godot.Collections.Array results);

	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName CreateDebounceTimer = "CreateDebounceTimer";

		public static readonly StringName Enable = "Enable";

		public static readonly StringName Disable = "Disable";

		public static readonly StringName ConnectSignals = "ConnectSignals";

		public static readonly StringName DisconnectSignals = "DisconnectSignals";

		public static readonly StringName OnGraphChanged = "OnGraphChanged";

		public static readonly StringName OnBlueprintChanged = "OnBlueprintChanged";

		public static readonly StringName OnDebounceTimerTimeout = "OnDebounceTimerTimeout";

		public static readonly StringName HasErrors = "HasErrors";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName Editor = "Editor";

		public static readonly StringName Validator = "Validator";

		public static readonly StringName DebounceTimer = "DebounceTimer";

		public static readonly StringName IsEnabled = "IsEnabled";

		public static readonly StringName DebounceTime = "DebounceTime";

		public static readonly StringName _subscribedScriptData = "_subscribedScriptData";
	}

	public new class SignalName : RefCounted.SignalName
	{
		public static readonly StringName ValidationComplete = "ValidationComplete";
	}

	private readonly List<XWBPGraphData> _subscribedTargets = new List<XWBPGraphData>();

	private XWBPScriptData _subscribedScriptData;

	private ValidationCompleteEventHandler backing_ValidationComplete;

	public XWBPEditor Editor { get; set; }

	public XWBPValidator Validator { get; set; }

	public Timer DebounceTimer { get; set; }

	public bool IsEnabled { get; set; } = true;

	public float DebounceTime { get; set; } = 1f;

	public event ValidationCompleteEventHandler ValidationComplete
	{
		add
		{
			backing_ValidationComplete = (ValidationCompleteEventHandler)Delegate.Combine(backing_ValidationComplete, value);
		}
		remove
		{
			backing_ValidationComplete = (ValidationCompleteEventHandler)Delegate.Remove(backing_ValidationComplete, value);
		}
	}

	public XWBPRealTimeValidator()
	{
	}

	public XWBPRealTimeValidator(XWBPEditor editor)
	{
		Editor = editor;
		CreateDebounceTimer();
	}

	private void CreateDebounceTimer()
	{
		DebounceTimer = new Timer();
		DebounceTimer.OneShot = true;
		DebounceTimer.WaitTime = DebounceTime;
		DebounceTimer.Timeout += OnDebounceTimerTimeout;
		Editor?.AddChild(DebounceTimer, forceReadableName: false, Node.InternalMode.Disabled);
	}

	public void Enable()
	{
		IsEnabled = true;
		ConnectSignals();
	}

	public void Disable()
	{
		IsEnabled = false;
		DisconnectSignals();
	}

	public void ConnectSignals()
	{
		DisconnectSignals();
		if (!GodotObject.IsInstanceValid(Editor) || Editor.BpScriptData == null)
		{
			return;
		}
		_subscribedScriptData = Editor.BpScriptData;
		_subscribedScriptData.BlueprintChanged += OnBlueprintChanged;
		foreach (XWBPGraphData value in Editor.BpScriptData.Graphs.Values)
		{
			value.GraphChange += OnGraphChanged;
			_subscribedTargets.Add(value);
		}
		foreach (XWBPFunctionData value2 in Editor.BpScriptData.Functions.Values)
		{
			value2.GraphChange += OnGraphChanged;
			_subscribedTargets.Add(value2);
		}
	}

	public void DisconnectSignals()
	{
		foreach (XWBPGraphData subscribedTarget in _subscribedTargets)
		{
			if (subscribedTarget != null && GodotObject.IsInstanceValid(subscribedTarget))
			{
				subscribedTarget.GraphChange -= OnGraphChanged;
			}
		}
		_subscribedTargets.Clear();
		if (GodotObject.IsInstanceValid(_subscribedScriptData))
		{
			_subscribedScriptData.BlueprintChanged -= OnBlueprintChanged;
		}
		_subscribedScriptData = null;
	}

	private void OnGraphChanged()
	{
		if (IsEnabled)
		{
			DebounceTimer?.Start();
		}
	}

	private void OnBlueprintChanged()
	{
		if (IsEnabled)
		{
			ConnectSignals();
			DebounceTimer?.Start();
		}
	}

	private void OnDebounceTimerTimeout()
	{
		if (!GodotObject.IsInstanceValid(Editor) || Editor.BpScriptData == null)
		{
			return;
		}
		Validator = new XWBPValidator(Editor.BpScriptData);
		List<XWBPValidationResult> list = Validator.Validate();
		Godot.Collections.Array array = new Godot.Collections.Array();
		foreach (XWBPValidationResult item in list)
		{
			array.Add(item);
		}
		EmitSignal(SignalName.ValidationComplete, array);
	}

	public List<XWBPValidationResult> GetLastResults()
	{
		if (!GodotObject.IsInstanceValid(Validator))
		{
			return new List<XWBPValidationResult>();
		}
		return Validator.Results;
	}

	public bool HasErrors()
	{
		if (!GodotObject.IsInstanceValid(Validator))
		{
			return false;
		}
		return Validator.HasErrors();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(9)
		{
			new MethodInfo(MethodName.CreateDebounceTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Enable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Disable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGraphChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnBlueprintChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDebounceTimerTimeout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasErrors, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateDebounceTimer && args.Count == 0)
		{
			CreateDebounceTimer();
			ret = default;
			return true;
		}
		if (method == MethodName.Enable && args.Count == 0)
		{
			Enable();
			ret = default;
			return true;
		}
		if (method == MethodName.Disable && args.Count == 0)
		{
			Disable();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectSignals && args.Count == 0)
		{
			ConnectSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectSignals && args.Count == 0)
		{
			DisconnectSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGraphChanged && args.Count == 0)
		{
			OnGraphChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnBlueprintChanged && args.Count == 0)
		{
			OnBlueprintChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDebounceTimerTimeout && args.Count == 0)
		{
			OnDebounceTimerTimeout();
			ret = default;
			return true;
		}
		if (method == MethodName.HasErrors && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasErrors());
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.CreateDebounceTimer)
		{
			return true;
		}
		if (method == MethodName.Enable)
		{
			return true;
		}
		if (method == MethodName.Disable)
		{
			return true;
		}
		if (method == MethodName.ConnectSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectSignals)
		{
			return true;
		}
		if (method == MethodName.OnGraphChanged)
		{
			return true;
		}
		if (method == MethodName.OnBlueprintChanged)
		{
			return true;
		}
		if (method == MethodName.OnDebounceTimerTimeout)
		{
			return true;
		}
		if (method == MethodName.HasErrors)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Editor)
		{
			Editor = VariantUtils.ConvertTo<XWBPEditor>(in value);
			return true;
		}
		if (name == PropertyName.Validator)
		{
			Validator = VariantUtils.ConvertTo<XWBPValidator>(in value);
			return true;
		}
		if (name == PropertyName.DebounceTimer)
		{
			DebounceTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName.IsEnabled)
		{
			IsEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.DebounceTime)
		{
			DebounceTime = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._subscribedScriptData)
		{
			_subscribedScriptData = VariantUtils.ConvertTo<XWBPScriptData>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Editor)
		{
			value = VariantUtils.CreateFrom<XWBPEditor>(Editor);
			return true;
		}
		if (name == PropertyName.Validator)
		{
			value = VariantUtils.CreateFrom<XWBPValidator>(Validator);
			return true;
		}
		if (name == PropertyName.DebounceTimer)
		{
			value = VariantUtils.CreateFrom<Timer>(DebounceTimer);
			return true;
		}
		if (name == PropertyName.IsEnabled)
		{
			value = VariantUtils.CreateFrom<bool>(IsEnabled);
			return true;
		}
		if (name == PropertyName.DebounceTime)
		{
			value = VariantUtils.CreateFrom<float>(DebounceTime);
			return true;
		}
		if (name == PropertyName._subscribedScriptData)
		{
			value = VariantUtils.CreateFrom(in _subscribedScriptData);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.Editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Validator, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.DebounceTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.DebounceTime, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._subscribedScriptData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Editor, Variant.From<XWBPEditor>(Editor));
		info.AddProperty(PropertyName.Validator, Variant.From<XWBPValidator>(Validator));
		info.AddProperty(PropertyName.DebounceTimer, Variant.From<Timer>(DebounceTimer));
		info.AddProperty(PropertyName.IsEnabled, Variant.From<bool>(IsEnabled));
		info.AddProperty(PropertyName.DebounceTime, Variant.From<float>(DebounceTime));
		info.AddProperty(PropertyName._subscribedScriptData, Variant.From(in _subscribedScriptData));
		info.AddSignalEventDelegate(SignalName.ValidationComplete, backing_ValidationComplete);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Editor, out var value))
		{
			Editor = value.As<XWBPEditor>();
		}
		if (info.TryGetProperty(PropertyName.Validator, out var value2))
		{
			Validator = value2.As<XWBPValidator>();
		}
		if (info.TryGetProperty(PropertyName.DebounceTimer, out var value3))
		{
			DebounceTimer = value3.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName.IsEnabled, out var value4))
		{
			IsEnabled = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.DebounceTime, out var value5))
		{
			DebounceTime = value5.As<float>();
		}
		if (info.TryGetProperty(PropertyName._subscribedScriptData, out var value6))
		{
			_subscribedScriptData = value6.As<XWBPScriptData>();
		}
		if (info.TryGetSignalEventDelegate<ValidationCompleteEventHandler>(SignalName.ValidationComplete, out var value7))
		{
			backing_ValidationComplete = value7;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.ValidationComplete, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "results", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalValidationComplete(Godot.Collections.Array results)
	{
		EmitSignal(SignalName.ValidationComplete, new ReadOnlySpan<Variant>((Variant)results));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.ValidationComplete && args.Count == 1)
		{
			backing_ValidationComplete?.Invoke(VariantUtils.ConvertTo<Godot.Collections.Array>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.ValidationComplete)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
