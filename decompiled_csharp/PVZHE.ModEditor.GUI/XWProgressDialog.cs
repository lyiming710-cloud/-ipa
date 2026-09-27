using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/GUI/XWProgressDialog.cs")]
public class XWProgressDialog : AcceptDialog
{
	[Signal]
	public delegate void ProgressCanceledEventHandler();

	public new class MethodName : AcceptDialog.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetStatus = "SetStatus";

		public static readonly StringName SetProgress = "SetProgress";

		public static readonly StringName SetProgressValue = "SetProgressValue";

		public static readonly StringName SetMaxValue = "SetMaxValue";

		public static readonly StringName Cancel = "Cancel";
	}

	public new class PropertyName : AcceptDialog.PropertyName
	{
		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _progressBar = "_progressBar";

		public static readonly StringName _cancelButton = "_cancelButton";
	}

	public new class SignalName : AcceptDialog.SignalName
	{
		public static readonly StringName ProgressCanceled = "ProgressCanceled";
	}

	private Label _statusLabel;

	private ProgressBar _progressBar;

	private Button _cancelButton;

	private ProgressCanceledEventHandler backing_ProgressCanceled;

	public event ProgressCanceledEventHandler ProgressCanceled
	{
		add
		{
			backing_ProgressCanceled = (ProgressCanceledEventHandler)Delegate.Combine(backing_ProgressCanceled, value);
		}
		remove
		{
			backing_ProgressCanceled = (ProgressCanceledEventHandler)Delegate.Remove(backing_ProgressCanceled, value);
		}
	}

	public override void _Ready()
	{
		_statusLabel = GetNode<Label>("%StatusLabel");
		_progressBar = GetNode<ProgressBar>("%ProgressBar");
		_cancelButton = GetNode<Button>("%CancelButton");
		CloseRequested += Cancel;
		_cancelButton.Pressed += Cancel;
	}

	public void SetStatus(string text)
	{
		_statusLabel.Text = text;
	}

	public void SetProgress(float ratio)
	{
		_progressBar.Value = ratio * 100f;
	}

	public void SetProgressValue(double value)
	{
		_progressBar.Value = value;
	}

	public void SetMaxValue(double max)
	{
		_progressBar.MaxValue = max;
	}

	private void Cancel()
	{
		EmitSignal(SignalName.ProgressCanceled);
		Hide();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgress, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "ratio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetProgressValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMaxValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "max", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Cancel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SetStatus && args.Count == 1)
		{
			SetStatus(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgress && args.Count == 1)
		{
			SetProgress(VariantUtils.ConvertTo<float>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetProgressValue && args.Count == 1)
		{
			SetProgressValue(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMaxValue && args.Count == 1)
		{
			SetMaxValue(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Cancel && args.Count == 0)
		{
			Cancel();
			ret = default;
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
		if (method == MethodName.SetStatus)
		{
			return true;
		}
		if (method == MethodName.SetProgress)
		{
			return true;
		}
		if (method == MethodName.SetProgressValue)
		{
			return true;
		}
		if (method == MethodName.SetMaxValue)
		{
			return true;
		}
		if (method == MethodName.Cancel)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._progressBar)
		{
			_progressBar = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._cancelButton)
		{
			_cancelButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._progressBar)
		{
			value = VariantUtils.CreateFrom(in _progressBar);
			return true;
		}
		if (name == PropertyName._cancelButton)
		{
			value = VariantUtils.CreateFrom(in _cancelButton);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._progressBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cancelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._progressBar, Variant.From(in _progressBar));
		info.AddProperty(PropertyName._cancelButton, Variant.From(in _cancelButton));
		info.AddSignalEventDelegate(SignalName.ProgressCanceled, backing_ProgressCanceled);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._statusLabel, out var value))
		{
			_statusLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._progressBar, out var value2))
		{
			_progressBar = value2.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._cancelButton, out var value3))
		{
			_cancelButton = value3.As<Button>();
		}
		if (info.TryGetSignalEventDelegate<ProgressCanceledEventHandler>(SignalName.ProgressCanceled, out var value4))
		{
			backing_ProgressCanceled = value4;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.ProgressCanceled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalProgressCanceled()
	{
		EmitSignal(SignalName.ProgressCanceled, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.ProgressCanceled && args.Count == 0)
		{
			backing_ProgressCanceled?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.ProgressCanceled)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
