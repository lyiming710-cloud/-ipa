using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;

namespace PVZHE.ModEditor.OutPutPanel;

[ScriptPath("res://addons/ModEditor/OutPutPanel/XWOutputPanel.cs")]
public class XWOutputPanel : PanelContainer
{
	public enum MessageType
	{
		Std,
		Error,
		Warning,
		Editor
	}

	private class LogMessage
	{
		public MessageType Type;

		public string Text = "";

		public int Count = 1;
	}

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName AddMessage = "AddMessage";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName CanAppendIncrementally = "CanAppendIncrementally";

		public static readonly StringName RequestRefresh = "RequestRefresh";

		public static readonly StringName FlushPendingRefresh = "FlushPendingRefresh";

		public static readonly StringName OnOutputVisibilityChanged = "OnOutputVisibilityChanged";

		public static readonly StringName PassFilter = "PassFilter";

		public static readonly StringName FindStartMessageIndex = "FindStartMessageIndex";

		public static readonly StringName GetInitialSkip = "GetInitialSkip";

		public static readonly StringName OnClearPressed = "OnClearPressed";

		public static readonly StringName OnCollapseToggled = "OnCollapseToggled";

		public static readonly StringName UpdateFilterCounts = "UpdateFilterCounts";

		public static readonly StringName TrimMessages = "TrimMessages";

		public static readonly StringName GetStoredLineCount = "GetStoredLineCount";

		public static readonly StringName GetStoredMessageCount = "GetStoredMessageCount";

		public static readonly StringName StripBbcode = "StripBbcode";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName FullRefreshCount = "FullRefreshCount";

		public static readonly StringName IncrementalAppendCount = "IncrementalAppendCount";

		public static readonly StringName RenderedLineCount = "RenderedLineCount";

		public static readonly StringName StoredLineCount = "StoredLineCount";

		public static readonly StringName _log = "_log";

		public static readonly StringName _searchBox = "_searchBox";

		public static readonly StringName _clearButton = "_clearButton";

		public static readonly StringName _collapseButton = "_collapseButton";

		public static readonly StringName _stdFilter = "_stdFilter";

		public static readonly StringName _errorFilter = "_errorFilter";

		public static readonly StringName _warningFilter = "_warningFilter";

		public static readonly StringName _editorFilter = "_editorFilter";

		public static readonly StringName _filterStd = "_filterStd";

		public static readonly StringName _filterError = "_filterError";

		public static readonly StringName _filterWarning = "_filterWarning";

		public static readonly StringName _filterEditor = "_filterEditor";

		public static readonly StringName _collapse = "_collapse";

		public static readonly StringName _refreshScheduled = "_refreshScheduled";

		public static readonly StringName _refreshWhenVisible = "_refreshWhenVisible";

		public static readonly StringName _renderedLineCount = "_renderedLineCount";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private const int LineLimit = 10000;

	private const int IncrementalRefreshSlack = 256;

	private RichTextLabel _log;

	private LineEdit _searchBox;

	private Button _clearButton;

	private Button _collapseButton;

	private Button _stdFilter;

	private Button _errorFilter;

	private Button _warningFilter;

	private Button _editorFilter;

	private readonly List<LogMessage> _messages = new List<LogMessage>();

	private readonly Dictionary<MessageType, int> _messageCounts = new Dictionary<MessageType, int>
	{
		{
			MessageType.Std,
			0
		},
		{
			MessageType.Error,
			0
		},
		{
			MessageType.Warning,
			0
		},
		{
			MessageType.Editor,
			0
		}
	};

	private bool _filterStd = true;

	private bool _filterError = true;

	private bool _filterWarning = true;

	private bool _filterEditor = true;

	private bool _collapse;

	private bool _refreshScheduled;

	private bool _refreshWhenVisible;

	private int _renderedLineCount;

	public int FullRefreshCount { get; private set; }

	public int IncrementalAppendCount { get; private set; }

	public int RenderedLineCount => _renderedLineCount;

	public int StoredLineCount => GetStoredLineCount();

	public override void _Ready()
	{
		_log = GetNode<RichTextLabel>("%Log");
		_searchBox = GetNode<LineEdit>("%SearchBox");
		_clearButton = GetNode<Button>("%ClearButton");
		_collapseButton = GetNode<Button>("%CollapseButton");
		_stdFilter = GetNode<Button>("%StdFilter");
		_errorFilter = GetNode<Button>("%ErrorFilter");
		_warningFilter = GetNode<Button>("%WarningFilter");
		_editorFilter = GetNode<Button>("%EditorFilter");
		_clearButton.Pressed += OnClearPressed;
		_collapseButton.Toggled += OnCollapseToggled;
		_stdFilter.Toggled += (bool _) =>
		{
			_filterStd = _stdFilter.ButtonPressed;
			RequestRefresh();
		};
		_errorFilter.Toggled += (bool _) =>
		{
			_filterError = _errorFilter.ButtonPressed;
			RequestRefresh();
		};
		_warningFilter.Toggled += (bool _) =>
		{
			_filterWarning = _warningFilter.ButtonPressed;
			RequestRefresh();
		};
		_editorFilter.Toggled += (bool _) =>
		{
			_filterEditor = _editorFilter.ButtonPressed;
			RequestRefresh();
		};
		_searchBox.TextChanged += (string _) =>
		{
			RequestRefresh();
		};
		VisibilityChanged += OnOutputVisibilityChanged;
		UpdateFilterCounts();
		Refresh();
	}

	public void AddMessage(string text, MessageType type = MessageType.Std)
	{
		if (text == null)
		{
			text = "";
		}
		string[] array = text.Replace("\r\n", "\n").Split('\n');
		List<LogMessage> list = new List<LogMessage>(array.Length);
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			list.Add(ProcessMessageLine(text2, type));
		}
		TrimMessages();
		UpdateFilterCounts();
		if (CanAppendIncrementally())
		{
			foreach (LogMessage item in list)
			{
				if (PassFilter(item.Type))
				{
					AppendLogLine(item);
					_renderedLineCount++;
					IncrementalAppendCount++;
				}
			}
			if (_renderedLineCount >= 10256)
			{
				RequestRefresh();
			}
		}
		else
		{
			RequestRefresh();
		}
	}

	public void Clear()
	{
		OnClearPressed();
	}

	private LogMessage ProcessMessageLine(string text, MessageType type)
	{
		if (_messages.Count > 0)
		{
			List<LogMessage> messages = _messages;
			LogMessage logMessage = messages[messages.Count - 1];
			if (logMessage.Type == type && logMessage.Text == text)
			{
				logMessage.Count++;
				_messageCounts[type]++;
				return logMessage;
			}
		}
		LogMessage logMessage2 = new LogMessage
		{
			Type = type,
			Text = text
		};
		_messages.Add(logMessage2);
		_messageCounts[type]++;
		return logMessage2;
	}

	private void Refresh()
	{
		if (!GodotObject.IsInstanceValid(_log) || !GodotObject.IsInstanceValid(_searchBox))
		{
			return;
		}
		FullRefreshCount++;
		_refreshScheduled = false;
		_refreshWhenVisible = false;
		_log.Clear();
		string text = _searchBox.Text;
		int num = 0;
		int num2 = FindStartMessageIndex(text);
		int num3 = GetInitialSkip(num2, text);
		for (int i = num2; i < _messages.Count; i++)
		{
			LogMessage logMessage = _messages[i];
			if (!PassFilter(logMessage.Type) || !PassSearch(logMessage, text))
			{
				continue;
			}
			int num4 = (_collapse ? 1 : logMessage.Count);
			for (int j = num3; j < num4; j++)
			{
				num3 = 0;
				AppendLogLine(logMessage);
				num++;
				if (num >= 10000)
				{
					_renderedLineCount = num;
					return;
				}
			}
		}
		_renderedLineCount = num;
	}

	private bool CanAppendIncrementally()
	{
		if (IsVisibleInTree() && !_refreshScheduled && !_collapse && GodotObject.IsInstanceValid(_searchBox) && string.IsNullOrEmpty(_searchBox.Text))
		{
			return _renderedLineCount < 10256;
		}
		return false;
	}

	private void RequestRefresh()
	{
		if (!IsVisibleInTree())
		{
			_refreshWhenVisible = true;
		}
		else if (!_refreshScheduled)
		{
			_refreshScheduled = true;
			CallDeferred("FlushPendingRefresh");
		}
	}

	private void FlushPendingRefresh()
	{
		if (_refreshScheduled)
		{
			if (!IsVisibleInTree())
			{
				_refreshScheduled = false;
				_refreshWhenVisible = true;
			}
			else
			{
				Refresh();
			}
		}
	}

	private void OnOutputVisibilityChanged()
	{
		if (IsVisibleInTree() && _refreshWhenVisible)
		{
			RequestRefresh();
		}
	}

	private bool PassFilter(MessageType type)
	{
		return type switch
		{
			MessageType.Std => _filterStd, 
			MessageType.Error => _filterError, 
			MessageType.Warning => _filterWarning, 
			MessageType.Editor => _filterEditor, 
			_ => true, 
		};
	}

	private static bool PassSearch(LogMessage message, string filter)
	{
		if (!string.IsNullOrEmpty(filter) && !message.Text.Contains(filter, StringComparison.OrdinalIgnoreCase))
		{
			return StripBbcode(message.Text).Contains(filter, StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	private int FindStartMessageIndex(string filter)
	{
		int num = 0;
		for (int num2 = _messages.Count - 1; num2 >= 0; num2--)
		{
			LogMessage logMessage = _messages[num2];
			if (PassFilter(logMessage.Type) && PassSearch(logMessage, filter))
			{
				num += (_collapse ? 1 : logMessage.Count);
				if (num >= 10000)
				{
					return num2;
				}
			}
		}
		return 0;
	}

	private int GetInitialSkip(int startIndex, string filter)
	{
		if (_collapse || startIndex <= 0 || startIndex >= _messages.Count)
		{
			return 0;
		}
		int num = 0;
		for (int i = startIndex; i < _messages.Count; i++)
		{
			LogMessage logMessage = _messages[i];
			if (PassFilter(logMessage.Type) && PassSearch(logMessage, filter))
			{
				num += logMessage.Count;
			}
		}
		return Mathf.Max(0, num - 10000);
	}

	private void AppendLogLine(LogMessage message)
	{
		bool flag = false;
		if (message.Type == MessageType.Error)
		{
			_log.PushColor(new Color("#ff5f5f"));
			_log.PushBold();
			_log.AddText("错误: ");
			_log.Pop();
			flag = true;
		}
		else if (message.Type == MessageType.Warning)
		{
			_log.PushColor(new Color("#ffd166"));
			_log.PushBold();
			_log.AddText("警告: ");
			_log.Pop();
			flag = true;
		}
		else if (message.Type == MessageType.Editor)
		{
			_log.PushColor(new Color(1f, 1f, 1f, 0.62f));
			flag = true;
		}
		if (_collapse && message.Count > 1)
		{
			_log.PushBold();
			_log.AddText($"({message.Count}) ");
			_log.Pop();
		}
		if (message.Type == MessageType.Error || message.Type == MessageType.Warning)
		{
			_log.AppendText(message.Text);
		}
		else
		{
			_log.AddText(message.Text);
		}
		if (flag)
		{
			_log.Pop();
		}
		_log.Newline();
	}

	private void OnClearPressed()
	{
		_messages.Clear();
		_messageCounts[MessageType.Std] = 0;
		_messageCounts[MessageType.Error] = 0;
		_messageCounts[MessageType.Warning] = 0;
		_messageCounts[MessageType.Editor] = 0;
		_refreshScheduled = false;
		_refreshWhenVisible = false;
		UpdateFilterCounts();
		Refresh();
	}

	private void OnCollapseToggled(bool toggled)
	{
		_collapse = toggled;
		RequestRefresh();
	}

	private void UpdateFilterCounts()
	{
		_stdFilter.Text = _messageCounts[MessageType.Std].ToString();
		_errorFilter.Text = _messageCounts[MessageType.Error].ToString();
		_warningFilter.Text = _messageCounts[MessageType.Warning].ToString();
		_editorFilter.Text = _messageCounts[MessageType.Editor].ToString();
	}

	private void TrimMessages()
	{
		int num = GetStoredLineCount();
		while (num > 10000 && _messages.Count > 0)
		{
			LogMessage logMessage = _messages[0];
			int num2 = num - 10000;
			if (logMessage.Count > num2)
			{
				logMessage.Count -= num2;
				_messageCounts[logMessage.Type] -= num2;
				num -= num2;
				break;
			}
			num -= logMessage.Count;
			_messageCounts[logMessage.Type] -= logMessage.Count;
			_messages.RemoveAt(0);
		}
	}

	private int GetStoredLineCount()
	{
		int num = 0;
		foreach (LogMessage message in _messages)
		{
			num += message.Count;
		}
		return num;
	}

	public int GetStoredMessageCount(MessageType type)
	{
		return _messageCounts.GetValueOrDefault(type);
	}

	private static string StripBbcode(string text)
	{
		if (string.IsNullOrEmpty(text) || !text.Contains('['))
		{
			return text ?? "";
		}
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder(text.Length);
		foreach (char c in text)
		{
			if (c == '[')
			{
				flag = true;
			}
			else if ((c == ']') & flag)
			{
				flag = false;
			}
			else if (!flag)
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(18)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddMessage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanAppendIncrementally, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FlushPendingRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnOutputVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PassFilter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindStartMessageIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetInitialSkip, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "startIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "filter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnClearPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCollapseToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "toggled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateFilterCounts, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrimMessages, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetStoredLineCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetStoredMessageCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StripBbcode, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
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
		if (method == MethodName.AddMessage && args.Count == 2)
		{
			AddMessage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<MessageType>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.CanAppendIncrementally && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanAppendIncrementally());
			return true;
		}
		if (method == MethodName.RequestRefresh && args.Count == 0)
		{
			RequestRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.FlushPendingRefresh && args.Count == 0)
		{
			FlushPendingRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.OnOutputVisibilityChanged && args.Count == 0)
		{
			OnOutputVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.PassFilter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(PassFilter(VariantUtils.ConvertTo<MessageType>(in args[0])));
			return true;
		}
		if (method == MethodName.FindStartMessageIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindStartMessageIndex(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetInitialSkip && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(GetInitialSkip(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.OnClearPressed && args.Count == 0)
		{
			OnClearPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCollapseToggled && args.Count == 1)
		{
			OnCollapseToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateFilterCounts && args.Count == 0)
		{
			UpdateFilterCounts();
			ret = default;
			return true;
		}
		if (method == MethodName.TrimMessages && args.Count == 0)
		{
			TrimMessages();
			ret = default;
			return true;
		}
		if (method == MethodName.GetStoredLineCount && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetStoredLineCount());
			return true;
		}
		if (method == MethodName.GetStoredMessageCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetStoredMessageCount(VariantUtils.ConvertTo<MessageType>(in args[0])));
			return true;
		}
		if (method == MethodName.StripBbcode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(StripBbcode(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.StripBbcode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(StripBbcode(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.AddMessage)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.CanAppendIncrementally)
		{
			return true;
		}
		if (method == MethodName.RequestRefresh)
		{
			return true;
		}
		if (method == MethodName.FlushPendingRefresh)
		{
			return true;
		}
		if (method == MethodName.OnOutputVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.PassFilter)
		{
			return true;
		}
		if (method == MethodName.FindStartMessageIndex)
		{
			return true;
		}
		if (method == MethodName.GetInitialSkip)
		{
			return true;
		}
		if (method == MethodName.OnClearPressed)
		{
			return true;
		}
		if (method == MethodName.OnCollapseToggled)
		{
			return true;
		}
		if (method == MethodName.UpdateFilterCounts)
		{
			return true;
		}
		if (method == MethodName.TrimMessages)
		{
			return true;
		}
		if (method == MethodName.GetStoredLineCount)
		{
			return true;
		}
		if (method == MethodName.GetStoredMessageCount)
		{
			return true;
		}
		if (method == MethodName.StripBbcode)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.FullRefreshCount)
		{
			FullRefreshCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.IncrementalAppendCount)
		{
			IncrementalAppendCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._log)
		{
			_log = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName._searchBox)
		{
			_searchBox = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._clearButton)
		{
			_clearButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._collapseButton)
		{
			_collapseButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._stdFilter)
		{
			_stdFilter = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._errorFilter)
		{
			_errorFilter = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._warningFilter)
		{
			_warningFilter = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._editorFilter)
		{
			_editorFilter = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._filterStd)
		{
			_filterStd = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._filterError)
		{
			_filterError = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._filterWarning)
		{
			_filterWarning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._filterEditor)
		{
			_filterEditor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._collapse)
		{
			_collapse = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._refreshScheduled)
		{
			_refreshScheduled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._refreshWhenVisible)
		{
			_refreshWhenVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderedLineCount)
		{
			_renderedLineCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.FullRefreshCount)
		{
			from = FullRefreshCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IncrementalAppendCount)
		{
			from = IncrementalAppendCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.RenderedLineCount)
		{
			from = RenderedLineCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.StoredLineCount)
		{
			from = StoredLineCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName._log)
		{
			value = VariantUtils.CreateFrom(in _log);
			return true;
		}
		if (name == PropertyName._searchBox)
		{
			value = VariantUtils.CreateFrom(in _searchBox);
			return true;
		}
		if (name == PropertyName._clearButton)
		{
			value = VariantUtils.CreateFrom(in _clearButton);
			return true;
		}
		if (name == PropertyName._collapseButton)
		{
			value = VariantUtils.CreateFrom(in _collapseButton);
			return true;
		}
		if (name == PropertyName._stdFilter)
		{
			value = VariantUtils.CreateFrom(in _stdFilter);
			return true;
		}
		if (name == PropertyName._errorFilter)
		{
			value = VariantUtils.CreateFrom(in _errorFilter);
			return true;
		}
		if (name == PropertyName._warningFilter)
		{
			value = VariantUtils.CreateFrom(in _warningFilter);
			return true;
		}
		if (name == PropertyName._editorFilter)
		{
			value = VariantUtils.CreateFrom(in _editorFilter);
			return true;
		}
		if (name == PropertyName._filterStd)
		{
			value = VariantUtils.CreateFrom(in _filterStd);
			return true;
		}
		if (name == PropertyName._filterError)
		{
			value = VariantUtils.CreateFrom(in _filterError);
			return true;
		}
		if (name == PropertyName._filterWarning)
		{
			value = VariantUtils.CreateFrom(in _filterWarning);
			return true;
		}
		if (name == PropertyName._filterEditor)
		{
			value = VariantUtils.CreateFrom(in _filterEditor);
			return true;
		}
		if (name == PropertyName._collapse)
		{
			value = VariantUtils.CreateFrom(in _collapse);
			return true;
		}
		if (name == PropertyName._refreshScheduled)
		{
			value = VariantUtils.CreateFrom(in _refreshScheduled);
			return true;
		}
		if (name == PropertyName._refreshWhenVisible)
		{
			value = VariantUtils.CreateFrom(in _refreshWhenVisible);
			return true;
		}
		if (name == PropertyName._renderedLineCount)
		{
			value = VariantUtils.CreateFrom(in _renderedLineCount);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._log, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._clearButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._collapseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stdFilter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._errorFilter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._warningFilter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorFilter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._filterStd, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._filterError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._filterWarning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._filterEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._collapse, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._refreshScheduled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._refreshWhenVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renderedLineCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.FullRefreshCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.IncrementalAppendCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.RenderedLineCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.StoredLineCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.FullRefreshCount, Variant.From<int>(FullRefreshCount));
		info.AddProperty(PropertyName.IncrementalAppendCount, Variant.From<int>(IncrementalAppendCount));
		info.AddProperty(PropertyName._log, Variant.From(in _log));
		info.AddProperty(PropertyName._searchBox, Variant.From(in _searchBox));
		info.AddProperty(PropertyName._clearButton, Variant.From(in _clearButton));
		info.AddProperty(PropertyName._collapseButton, Variant.From(in _collapseButton));
		info.AddProperty(PropertyName._stdFilter, Variant.From(in _stdFilter));
		info.AddProperty(PropertyName._errorFilter, Variant.From(in _errorFilter));
		info.AddProperty(PropertyName._warningFilter, Variant.From(in _warningFilter));
		info.AddProperty(PropertyName._editorFilter, Variant.From(in _editorFilter));
		info.AddProperty(PropertyName._filterStd, Variant.From(in _filterStd));
		info.AddProperty(PropertyName._filterError, Variant.From(in _filterError));
		info.AddProperty(PropertyName._filterWarning, Variant.From(in _filterWarning));
		info.AddProperty(PropertyName._filterEditor, Variant.From(in _filterEditor));
		info.AddProperty(PropertyName._collapse, Variant.From(in _collapse));
		info.AddProperty(PropertyName._refreshScheduled, Variant.From(in _refreshScheduled));
		info.AddProperty(PropertyName._refreshWhenVisible, Variant.From(in _refreshWhenVisible));
		info.AddProperty(PropertyName._renderedLineCount, Variant.From(in _renderedLineCount));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.FullRefreshCount, out var value))
		{
			FullRefreshCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.IncrementalAppendCount, out var value2))
		{
			IncrementalAppendCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._log, out var value3))
		{
			_log = value3.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName._searchBox, out var value4))
		{
			_searchBox = value4.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._clearButton, out var value5))
		{
			_clearButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._collapseButton, out var value6))
		{
			_collapseButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._stdFilter, out var value7))
		{
			_stdFilter = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._errorFilter, out var value8))
		{
			_errorFilter = value8.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._warningFilter, out var value9))
		{
			_warningFilter = value9.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._editorFilter, out var value10))
		{
			_editorFilter = value10.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._filterStd, out var value11))
		{
			_filterStd = value11.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._filterError, out var value12))
		{
			_filterError = value12.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._filterWarning, out var value13))
		{
			_filterWarning = value13.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._filterEditor, out var value14))
		{
			_filterEditor = value14.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._collapse, out var value15))
		{
			_collapse = value15.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._refreshScheduled, out var value16))
		{
			_refreshScheduled = value16.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._refreshWhenVisible, out var value17))
		{
			_refreshWhenVisible = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderedLineCount, out var value18))
		{
			_renderedLineCount = value18.As<int>();
		}
	}
}
