using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.GUI;

[ScriptPath("res://addons/ModEditor/GUI/XWEditorLog.cs")]
public class XWEditorLog : PanelContainer
{
	public enum LogType
	{
		Info,
		Warning,
		Error,
		Script
	}

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName AddEntry = "AddEntry";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName PassFilter = "PassFilter";

		public static readonly StringName AppendColored = "AppendColored";

		public static readonly StringName OnCopyPressed = "OnCopyPressed";

		public static readonly StringName OnClearPressed = "OnClearPressed";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName _infoFilterButton = "_infoFilterButton";

		public static readonly StringName _warningFilterButton = "_warningFilterButton";

		public static readonly StringName _errorFilterButton = "_errorFilterButton";

		public static readonly StringName _scriptFilterButton = "_scriptFilterButton";

		public static readonly StringName _copyButton = "_copyButton";

		public static readonly StringName _clearButton = "_clearButton";

		public static readonly StringName _logLabel = "_logLabel";

		public static readonly StringName _showInfo = "_showInfo";

		public static readonly StringName _showWarning = "_showWarning";

		public static readonly StringName _showError = "_showError";

		public static readonly StringName _showScript = "_showScript";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private Button _infoFilterButton;

	private Button _warningFilterButton;

	private Button _errorFilterButton;

	private Button _scriptFilterButton;

	private Button _copyButton;

	private Button _clearButton;

	private RichTextLabel _logLabel;

	private readonly List<(LogType type, string text)> _entries = new List<(LogType, string)>();

	private bool _showInfo = true;

	private bool _showWarning = true;

	private bool _showError = true;

	private bool _showScript = true;

	public override void _Ready()
	{
		_infoFilterButton = GetNode<Button>("%InfoFilterButton");
		_warningFilterButton = GetNode<Button>("%WarningFilterButton");
		_errorFilterButton = GetNode<Button>("%ErrorFilterButton");
		_scriptFilterButton = GetNode<Button>("%ScriptFilterButton");
		_copyButton = GetNode<Button>("%CopyButton");
		_clearButton = GetNode<Button>("%ClearButton");
		_logLabel = GetNode<RichTextLabel>("%LogLabel");
		_infoFilterButton.Toggled += (bool v) =>
		{
			_showInfo = v;
			Refresh();
		};
		_warningFilterButton.Toggled += (bool v) =>
		{
			_showWarning = v;
			Refresh();
		};
		_errorFilterButton.Toggled += (bool v) =>
		{
			_showError = v;
			Refresh();
		};
		_scriptFilterButton.Toggled += (bool v) =>
		{
			_showScript = v;
			Refresh();
		};
		_copyButton.Pressed += OnCopyPressed;
		_clearButton.Pressed += OnClearPressed;
	}

	public void AddEntry(string text, LogType type = LogType.Info)
	{
		_entries.Add((type, text));
		Refresh();
	}

	private void Refresh()
	{
		_logLabel.Clear();
		foreach (var (type, text) in _entries)
		{
			if (PassFilter(type))
			{
				AppendColored(text, type);
			}
		}
	}

	private bool PassFilter(LogType type)
	{
		return type switch
		{
			LogType.Info => _showInfo, 
			LogType.Warning => _showWarning, 
			LogType.Error => _showError, 
			LogType.Script => _showScript, 
			_ => true, 
		};
	}

	private void AppendColored(string text, LogType type)
	{
		_logLabel.PushColor(new Color(type switch
		{
			LogType.Error => "#ff4545", 
			LogType.Warning => "#ffd400", 
			LogType.Script => "#87ceeb", 
			_ => "#cccccc", 
		}));
		_logLabel.AddText(text + "\n");
		_logLabel.Pop();
	}

	private void OnCopyPressed()
	{
		DisplayServer.ClipboardSet(_logLabel.GetSelectedText());
	}

	private void OnClearPressed()
	{
		_entries.Clear();
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PassFilter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AppendColored, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCopyPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnClearPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.AddEntry && args.Count == 2)
		{
			AddEntry(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<LogType>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.PassFilter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(PassFilter(VariantUtils.ConvertTo<LogType>(in args[0])));
			return true;
		}
		if (method == MethodName.AppendColored && args.Count == 2)
		{
			AppendColored(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<LogType>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCopyPressed && args.Count == 0)
		{
			OnCopyPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnClearPressed && args.Count == 0)
		{
			OnClearPressed();
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
		if (method == MethodName.AddEntry)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.PassFilter)
		{
			return true;
		}
		if (method == MethodName.AppendColored)
		{
			return true;
		}
		if (method == MethodName.OnCopyPressed)
		{
			return true;
		}
		if (method == MethodName.OnClearPressed)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._infoFilterButton)
		{
			_infoFilterButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._warningFilterButton)
		{
			_warningFilterButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._errorFilterButton)
		{
			_errorFilterButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._scriptFilterButton)
		{
			_scriptFilterButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._copyButton)
		{
			_copyButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._clearButton)
		{
			_clearButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._logLabel)
		{
			_logLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName._showInfo)
		{
			_showInfo = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._showWarning)
		{
			_showWarning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._showError)
		{
			_showError = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._showScript)
		{
			_showScript = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._infoFilterButton)
		{
			value = VariantUtils.CreateFrom(in _infoFilterButton);
			return true;
		}
		if (name == PropertyName._warningFilterButton)
		{
			value = VariantUtils.CreateFrom(in _warningFilterButton);
			return true;
		}
		if (name == PropertyName._errorFilterButton)
		{
			value = VariantUtils.CreateFrom(in _errorFilterButton);
			return true;
		}
		if (name == PropertyName._scriptFilterButton)
		{
			value = VariantUtils.CreateFrom(in _scriptFilterButton);
			return true;
		}
		if (name == PropertyName._copyButton)
		{
			value = VariantUtils.CreateFrom(in _copyButton);
			return true;
		}
		if (name == PropertyName._clearButton)
		{
			value = VariantUtils.CreateFrom(in _clearButton);
			return true;
		}
		if (name == PropertyName._logLabel)
		{
			value = VariantUtils.CreateFrom(in _logLabel);
			return true;
		}
		if (name == PropertyName._showInfo)
		{
			value = VariantUtils.CreateFrom(in _showInfo);
			return true;
		}
		if (name == PropertyName._showWarning)
		{
			value = VariantUtils.CreateFrom(in _showWarning);
			return true;
		}
		if (name == PropertyName._showError)
		{
			value = VariantUtils.CreateFrom(in _showError);
			return true;
		}
		if (name == PropertyName._showScript)
		{
			value = VariantUtils.CreateFrom(in _showScript);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._infoFilterButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._warningFilterButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._errorFilterButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scriptFilterButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._copyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._clearButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._logLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._showInfo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._showWarning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._showError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._showScript, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._infoFilterButton, Variant.From(in _infoFilterButton));
		info.AddProperty(PropertyName._warningFilterButton, Variant.From(in _warningFilterButton));
		info.AddProperty(PropertyName._errorFilterButton, Variant.From(in _errorFilterButton));
		info.AddProperty(PropertyName._scriptFilterButton, Variant.From(in _scriptFilterButton));
		info.AddProperty(PropertyName._copyButton, Variant.From(in _copyButton));
		info.AddProperty(PropertyName._clearButton, Variant.From(in _clearButton));
		info.AddProperty(PropertyName._logLabel, Variant.From(in _logLabel));
		info.AddProperty(PropertyName._showInfo, Variant.From(in _showInfo));
		info.AddProperty(PropertyName._showWarning, Variant.From(in _showWarning));
		info.AddProperty(PropertyName._showError, Variant.From(in _showError));
		info.AddProperty(PropertyName._showScript, Variant.From(in _showScript));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._infoFilterButton, out var value))
		{
			_infoFilterButton = value.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._warningFilterButton, out var value2))
		{
			_warningFilterButton = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._errorFilterButton, out var value3))
		{
			_errorFilterButton = value3.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._scriptFilterButton, out var value4))
		{
			_scriptFilterButton = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._copyButton, out var value5))
		{
			_copyButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._clearButton, out var value6))
		{
			_clearButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._logLabel, out var value7))
		{
			_logLabel = value7.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName._showInfo, out var value8))
		{
			_showInfo = value8.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._showWarning, out var value9))
		{
			_showWarning = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._showError, out var value10))
		{
			_showError = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._showScript, out var value11))
		{
			_showScript = value11.As<bool>();
		}
	}
}
