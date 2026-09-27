using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/GUI/ValidationPanel/XWBPValidationPanel.cs")]
public class XWBPValidationPanel : PanelContainer
{
	[Signal]
	public delegate void ValidationResultClickedEventHandler(XWBPValidationResult result);

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetEditor = "SetEditor";

		public static readonly StringName OnValidatePressed = "OnValidatePressed";

		public static readonly StringName OnCleanPressed = "OnCleanPressed";

		public static readonly StringName OnTogglePressed = "OnTogglePressed";

		public static readonly StringName OnResultMetaClicked = "OnResultMetaClicked";

		public static readonly StringName EscapeBbcode = "EscapeBbcode";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName _editor = "_editor";

		public static readonly StringName _resultList = "_resultList";

		public static readonly StringName _errorCountLabel = "_errorCountLabel";

		public static readonly StringName _warningCountLabel = "_warningCountLabel";

		public static readonly StringName _toggleButton = "_toggleButton";

		public static readonly StringName _validateButton = "_validateButton";

		public static readonly StringName _cleanButton = "_cleanButton";
	}

	public new class SignalName : PanelContainer.SignalName
	{
		public static readonly StringName ValidationResultClicked = "ValidationResultClicked";
	}

	private XWBPEditor _editor;

	private readonly List<XWBPValidationResult> _results = new List<XWBPValidationResult>();

	private RichTextLabel _resultList;

	private Label _errorCountLabel;

	private Label _warningCountLabel;

	private Button _toggleButton;

	private Button _validateButton;

	private Button _cleanButton;

	private ValidationResultClickedEventHandler backing_ValidationResultClicked;

	public event ValidationResultClickedEventHandler ValidationResultClicked
	{
		add
		{
			backing_ValidationResultClicked = (ValidationResultClickedEventHandler)Delegate.Combine(backing_ValidationResultClicked, value);
		}
		remove
		{
			backing_ValidationResultClicked = (ValidationResultClickedEventHandler)Delegate.Remove(backing_ValidationResultClicked, value);
		}
	}

	public override void _Ready()
	{
		_resultList = GetNode<RichTextLabel>("%ResultList");
		_errorCountLabel = GetNode<Label>("%ErrorCountLabel");
		_warningCountLabel = GetNode<Label>("%WarningCountLabel");
		_toggleButton = GetNode<Button>("%ToggleButton");
		_validateButton = GetNode<Button>("%ValidateButton");
		_cleanButton = GetNode<Button>("%CleanButton");
		_validateButton.Pressed += OnValidatePressed;
		_cleanButton.Pressed += OnCleanPressed;
		_toggleButton.Pressed += OnTogglePressed;
		_resultList.MetaClicked += OnResultMetaClicked;
	}

	public void SetEditor(XWBPEditor editor)
	{
		_editor = editor;
	}

	public void SetResults(List<XWBPValidationResult> results)
	{
		_results.Clear();
		_resultList.Clear();
		int num = 0;
		int num2 = 0;
		foreach (XWBPValidationResult result in results)
		{
			_results.Add(result);
			string severityText = result.GetSeverityText();
			string value;
			if (result.ResultSeverity != XWBPValidationResult.Severity.Error)
			{
				value = ((result.ResultSeverity == XWBPValidationResult.Severity.Warning) ? "yellow" : "white");
			}
			else
			{
				value = "red";
			}
			string text = ((result.GraphData != null) ? result.GraphData.Name : "");
			string text2 = $"[url={_results.Count - 1}][color={value}][{severityText}] {EscapeBbcode(result.GetErrorTypeText())}";
			if (!string.IsNullOrEmpty(text))
			{
				text2 = text2 + " / " + EscapeBbcode(text);
			}
			if (result.NodeId >= 0)
			{
				text2 += $" (节点 {result.NodeId})";
			}
			text2 = text2 + ": " + EscapeBbcode(result.Message) + "[/color][/url]\n";
			_resultList.AppendText(text2);
			if (result.ResultSeverity == XWBPValidationResult.Severity.Error)
			{
				num++;
			}
			else if (result.ResultSeverity == XWBPValidationResult.Severity.Warning)
			{
				num2++;
			}
		}
		_errorCountLabel.Text = num.ToString();
		_warningCountLabel.Text = num2.ToString();
	}

	private void OnValidatePressed()
	{
		if (_editor?.RealTimeValidator?.DebounceTimer != null)
		{
			_editor.RealTimeValidator.DebounceTimer.Start();
		}
	}

	private void OnCleanPressed()
	{
		_results.Clear();
		_resultList.Clear();
		_errorCountLabel.Text = "0";
		_warningCountLabel.Text = "0";
	}

	private void OnTogglePressed()
	{
		bool visible = _resultList.Visible;
		_resultList.Visible = !visible;
		_toggleButton.Text = (visible ? "▼" : "▲");
	}

	private void OnResultMetaClicked(Variant meta)
	{
		int num = meta.AsInt32();
		if (num >= 0 && num < _results.Count)
		{
			XWBPValidationResult xWBPValidationResult = _results[num];
			EmitSignal(SignalName.ValidationResultClicked, xWBPValidationResult);
			_editor?.FocusValidationResult(xWBPValidationResult);
		}
	}

	private static string EscapeBbcode(string text)
	{
		return (text ?? "").Replace("[", "\\[").Replace("]", "\\]");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnValidatePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCleanPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTogglePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnResultMetaClicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "meta", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.EscapeBbcode, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
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
		if (method == MethodName.SetEditor && args.Count == 1)
		{
			SetEditor(VariantUtils.ConvertTo<XWBPEditor>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnValidatePressed && args.Count == 0)
		{
			OnValidatePressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCleanPressed && args.Count == 0)
		{
			OnCleanPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTogglePressed && args.Count == 0)
		{
			OnTogglePressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnResultMetaClicked && args.Count == 1)
		{
			OnResultMetaClicked(VariantUtils.ConvertTo<Variant>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EscapeBbcode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EscapeBbcode(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.EscapeBbcode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EscapeBbcode(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.SetEditor)
		{
			return true;
		}
		if (method == MethodName.OnValidatePressed)
		{
			return true;
		}
		if (method == MethodName.OnCleanPressed)
		{
			return true;
		}
		if (method == MethodName.OnTogglePressed)
		{
			return true;
		}
		if (method == MethodName.OnResultMetaClicked)
		{
			return true;
		}
		if (method == MethodName.EscapeBbcode)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editor)
		{
			_editor = VariantUtils.ConvertTo<XWBPEditor>(in value);
			return true;
		}
		if (name == PropertyName._resultList)
		{
			_resultList = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName._errorCountLabel)
		{
			_errorCountLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._warningCountLabel)
		{
			_warningCountLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._toggleButton)
		{
			_toggleButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._validateButton)
		{
			_validateButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._cleanButton)
		{
			_cleanButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editor)
		{
			value = VariantUtils.CreateFrom(in _editor);
			return true;
		}
		if (name == PropertyName._resultList)
		{
			value = VariantUtils.CreateFrom(in _resultList);
			return true;
		}
		if (name == PropertyName._errorCountLabel)
		{
			value = VariantUtils.CreateFrom(in _errorCountLabel);
			return true;
		}
		if (name == PropertyName._warningCountLabel)
		{
			value = VariantUtils.CreateFrom(in _warningCountLabel);
			return true;
		}
		if (name == PropertyName._toggleButton)
		{
			value = VariantUtils.CreateFrom(in _toggleButton);
			return true;
		}
		if (name == PropertyName._validateButton)
		{
			value = VariantUtils.CreateFrom(in _validateButton);
			return true;
		}
		if (name == PropertyName._cleanButton)
		{
			value = VariantUtils.CreateFrom(in _cleanButton);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._errorCountLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._warningCountLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._toggleButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._validateButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cleanButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
		info.AddProperty(PropertyName._resultList, Variant.From(in _resultList));
		info.AddProperty(PropertyName._errorCountLabel, Variant.From(in _errorCountLabel));
		info.AddProperty(PropertyName._warningCountLabel, Variant.From(in _warningCountLabel));
		info.AddProperty(PropertyName._toggleButton, Variant.From(in _toggleButton));
		info.AddProperty(PropertyName._validateButton, Variant.From(in _validateButton));
		info.AddProperty(PropertyName._cleanButton, Variant.From(in _cleanButton));
		info.AddSignalEventDelegate(SignalName.ValidationResultClicked, backing_ValidationResultClicked);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XWBPEditor>();
		}
		if (info.TryGetProperty(PropertyName._resultList, out var value2))
		{
			_resultList = value2.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName._errorCountLabel, out var value3))
		{
			_errorCountLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._warningCountLabel, out var value4))
		{
			_warningCountLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._toggleButton, out var value5))
		{
			_toggleButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._validateButton, out var value6))
		{
			_validateButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._cleanButton, out var value7))
		{
			_cleanButton = value7.As<Button>();
		}
		if (info.TryGetSignalEventDelegate<ValidationResultClickedEventHandler>(SignalName.ValidationResultClicked, out var value8))
		{
			backing_ValidationResultClicked = value8;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.ValidationResultClicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "result", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null)
		};
	}

	protected void EmitSignalValidationResultClicked(XWBPValidationResult result)
	{
		EmitSignal(SignalName.ValidationResultClicked, new ReadOnlySpan<Variant>((Variant)result));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.ValidationResultClicked && args.Count == 1)
		{
			backing_ValidationResultClicked?.Invoke(VariantUtils.ConvertTo<XWBPValidationResult>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.ValidationResultClicked)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
