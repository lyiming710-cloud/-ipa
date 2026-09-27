using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[GlobalClass]
[ScriptPath("res://Core/CommandConsole/CommandConsole.cs")]
public class CommandConsole : CanvasLayer
{
	public delegate void ConsoleOpenedEventHandler();

	public delegate void ConsoleClosedEventHandler();

	public new class MethodName : CanvasLayer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName Open = "Open";

		public static readonly StringName Close = "Close";

		public static readonly StringName IsOpen = "IsOpen";

		public static readonly StringName GetHistory = "GetHistory";

		public static readonly StringName PrintLine = "PrintLine";

		public static readonly StringName PrintInfo = "PrintInfo";

		public static readonly StringName PrintSuccess = "PrintSuccess";

		public static readonly StringName PrintWarning = "PrintWarning";

		public static readonly StringName PrintError = "PrintError";

		public static readonly StringName ClearLog = "ClearLog";

		public static readonly StringName _ShowWelcome = "_ShowWelcome";

		public static readonly StringName _OnInputSubmitted = "_OnInputSubmitted";

		public static readonly StringName _OnInputChanged = "_OnInputChanged";

		public static readonly StringName _HandleAutocomplete = "_HandleAutocomplete";
	}

	public new class PropertyName : CanvasLayer.PropertyName
	{
		public static readonly StringName _history = "_history";

		public static readonly StringName _historyIndex = "_historyIndex";

		public static readonly StringName _isOpen = "_isOpen";

		public static readonly StringName _autocompleteIndex = "_autocompleteIndex";

		public static readonly StringName _autocompleteList = "_autocompleteList";

		public static readonly StringName _welcomeShown = "_welcomeShown";

		public static readonly StringName _consolePanel = "_consolePanel";

		public static readonly StringName _outputLog = "_outputLog";

		public static readonly StringName _inputLine = "_inputLine";
	}

	public new class SignalName : CanvasLayer.SignalName
	{
	}

	private Array<string> _history = new Array<string>();

	private int _historyIndex = -1;

	private bool _isOpen;

	private int _autocompleteIndex = -1;

	private Array<string> _autocompleteList = new Array<string>();

	private bool _welcomeShown;

	private PanelContainer _consolePanel;

	private RichTextLabel _outputLog;

	private LineEdit _inputLine;

	public static CommandConsole Instance;

	public event ConsoleOpenedEventHandler OnConsoleOpened;

	public event ConsoleClosedEventHandler OnConsoleClosed;

	public override void _Ready()
	{
		Instance = this;
		Layer = 200;
		_consolePanel = GetNode<PanelContainer>("%ConsolePanel");
		_outputLog = GetNode<RichTextLabel>("%OutputLog");
		_inputLine = GetNode<LineEdit>("%InputLine");
		_consolePanel.Visible = false;
		_outputLog.BbcodeEnabled = true;
		_outputLog.ScrollFollowing = true;
		_inputLine.TextSubmitted += _OnInputSubmitted;
		_inputLine.TextChanged += _OnInputChanged;
	}

	public override void _Input(InputEvent _event)
	{
		if (!(_event is InputEventKey { Pressed: not false } inputEventKey) || !_isOpen)
		{
			return;
		}
		switch (inputEventKey.Keycode)
		{
		case Key.Escape:
			Close();
			GetViewport().SetInputAsHandled();
			break;
		case Key.Up:
			if (_inputLine.HasFocus() && _history.Count > 0)
			{
				_historyIndex = Mathf.Min(_historyIndex + 1, _history.Count - 1);
				_inputLine.Text = _history[_historyIndex];
				_inputLine.CaretColumn = _inputLine.Text.Length;
				GetViewport().SetInputAsHandled();
			}
			break;
		case Key.Down:
			if (_inputLine.HasFocus() && _history.Count > 0)
			{
				_historyIndex = Mathf.Max(_historyIndex - 1, -1);
				if (_historyIndex == -1)
				{
					_inputLine.Text = "";
				}
				else
				{
					_inputLine.Text = _history[_historyIndex];
				}
				_inputLine.CaretColumn = _inputLine.Text.Length;
				GetViewport().SetInputAsHandled();
			}
			break;
		case Key.Tab:
			if (_inputLine.HasFocus())
			{
				_HandleAutocomplete();
				GetViewport().SetInputAsHandled();
			}
			break;
		}
	}

	public void Open()
	{
		if (!_isOpen)
		{
			_isOpen = true;
			_consolePanel.Visible = true;
			_inputLine.GrabFocus();
			_autocompleteIndex = -1;
			_autocompleteList.Clear();
			if (!_welcomeShown)
			{
				_ShowWelcome();
				_welcomeShown = true;
			}
			OnConsoleOpened?.Invoke();
		}
	}

	public void Close()
	{
		if (_isOpen)
		{
			_isOpen = false;
			_consolePanel.Visible = false;
			_inputLine.Text = "";
			_inputLine.ReleaseFocus();
			_historyIndex = -1;
			_autocompleteIndex = -1;
			_autocompleteList.Clear();
			OnConsoleClosed?.Invoke();
		}
	}

	public bool IsOpen()
	{
		return _isOpen;
	}

	public Array<string> GetHistory()
	{
		return _history;
	}

	public void PrintLine(string text)
	{
		_outputLog.AppendText(text + "\n");
	}

	public void PrintInfo(string text)
	{
		_outputLog.AppendText("[color=gray]" + text + "[/color]\n");
	}

	public void PrintSuccess(string text)
	{
		_outputLog.AppendText("[color=green]" + text + "[/color]\n");
	}

	public void PrintWarning(string text)
	{
		_outputLog.AppendText("[color=yellow]" + text + "[/color]\n");
	}

	public void PrintError(string text)
	{
		_outputLog.AppendText("[color=red]" + text + "[/color]\n");
	}

	public void ClearLog()
	{
		_outputLog.Clear();
	}

	private void _ShowWelcome()
	{
		PrintLine("[color=cyan]═══════════════════════════════════════[/color]");
		PrintLine("[color=cyan]  指令控制台 v1.0[/color]");
		PrintLine("[color=gray]  输入 /help 查看所有指令[/color]");
		PrintLine("[color=gray]  按 Esc 关闭控制台[/color]");
		PrintLine("[color=cyan]═══════════════════════════════════════[/color]");
	}

	private void _OnInputSubmitted(string text)
	{
		_inputLine.Text = "";
		if (!(text.StripEdges() == ""))
		{
			_history.Insert(0, text);
			if (_history.Count > 50)
			{
				_history.RemoveAt(_history.Count - 1);
			}
			_historyIndex = -1;
			_autocompleteIndex = -1;
			_autocompleteList.Clear();
			PrintLine("[color=white]> " + text + "[/color]");
			CommandRegistry.ExecuteCommand(text, new Callable(this, MethodName.PrintError));
			_inputLine.GrabFocus();
		}
	}

	private void _OnInputChanged(string newText)
	{
		_autocompleteIndex = -1;
		_autocompleteList.Clear();
	}

	private void _HandleAutocomplete()
	{
		string text = _inputLine.Text.StripEdges();
		if (text == "")
		{
			return;
		}
		if (_autocompleteIndex == -1)
		{
			_autocompleteList.Clear();
			Array<string> array = CommandRegistry._ParseCommand(text.StartsWith("/") ? text.Substring(1) : text);
			if (array.Count <= 1)
			{
				string text2 = text;
				if (text2.StartsWith("/"))
				{
					text2 = text2.Substring(1);
				}
				text2 = text2.ToLower();
				foreach (KeyValuePair<string, CommandConfig> allCommand in CommandRegistry.GetAllCommands())
				{
					string key = allCommand.Key;
					if (key.StartsWith(text2))
					{
						_autocompleteList.Add(key);
					}
				}
			}
			else
			{
				CommandConfig command = CommandRegistry.GetCommand(array[0].ToLower());
				if (command != null && array.Count - 2 < command.ArgsInfo.Count)
				{
					CommandArg commandArg = command.ArgsInfo[array.Count - 2];
					if (commandArg.Suggestions.Target != null || (object)commandArg.Suggestions.Delegate != null)
					{
						string value = array[array.Count - 1].ToLower();
						Variant variant = commandArg.Suggestions.Call();
						if (variant.VariantType == Variant.Type.Array)
						{
							foreach (Variant item in variant.AsGodotArray())
							{
								if (item.AsString().ToLower().StartsWith(value))
								{
									_autocompleteList.Add(item.AsString());
								}
							}
						}
					}
				}
			}
			if (_autocompleteList.Count == 0)
			{
				return;
			}
			_autocompleteIndex = 0;
		}
		else
		{
			_autocompleteIndex = (_autocompleteIndex + 1) % _autocompleteList.Count;
		}
		if (_autocompleteList.Count > 0)
		{
			Array<string> array2 = CommandRegistry._ParseCommand(text.StartsWith("/") ? text.Substring(1) : text);
			string text3 = _autocompleteList[_autocompleteIndex];
			if (array2.Count <= 1)
			{
				_inputLine.Text = "/" + text3 + " ";
			}
			else
			{
				array2[array2.Count - 1] = text3;
				_inputLine.Text = "/" + string.Join(" ", array2) + " ";
			}
			_inputLine.CaretColumn = _inputLine.Text.Length;
		}
	}

	public CommandConsole()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/CommandConsole");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(16)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.Open, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Close, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsOpen, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetHistory, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrintLine, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrintInfo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrintSuccess, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrintWarning, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrintError, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearLog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ShowWelcome, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._OnInputSubmitted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._OnInputChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._HandleAutocomplete, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Open && args.Count == 0)
		{
			Open();
			ret = default;
			return true;
		}
		if (method == MethodName.Close && args.Count == 0)
		{
			Close();
			ret = default;
			return true;
		}
		if (method == MethodName.IsOpen && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsOpen());
			return true;
		}
		if (method == MethodName.GetHistory && args.Count == 0)
		{
			Array<string> history = GetHistory();
			ret = VariantUtils.CreateFromArray(history);
			return true;
		}
		if (method == MethodName.PrintLine && args.Count == 1)
		{
			PrintLine(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrintInfo && args.Count == 1)
		{
			PrintInfo(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrintSuccess && args.Count == 1)
		{
			PrintSuccess(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrintWarning && args.Count == 1)
		{
			PrintWarning(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrintError && args.Count == 1)
		{
			PrintError(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearLog && args.Count == 0)
		{
			ClearLog();
			ret = default;
			return true;
		}
		if (method == MethodName._ShowWelcome && args.Count == 0)
		{
			_ShowWelcome();
			ret = default;
			return true;
		}
		if (method == MethodName._OnInputSubmitted && args.Count == 1)
		{
			_OnInputSubmitted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._OnInputChanged && args.Count == 1)
		{
			_OnInputChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._HandleAutocomplete && args.Count == 0)
		{
			_HandleAutocomplete();
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
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName.Open)
		{
			return true;
		}
		if (method == MethodName.Close)
		{
			return true;
		}
		if (method == MethodName.IsOpen)
		{
			return true;
		}
		if (method == MethodName.GetHistory)
		{
			return true;
		}
		if (method == MethodName.PrintLine)
		{
			return true;
		}
		if (method == MethodName.PrintInfo)
		{
			return true;
		}
		if (method == MethodName.PrintSuccess)
		{
			return true;
		}
		if (method == MethodName.PrintWarning)
		{
			return true;
		}
		if (method == MethodName.PrintError)
		{
			return true;
		}
		if (method == MethodName.ClearLog)
		{
			return true;
		}
		if (method == MethodName._ShowWelcome)
		{
			return true;
		}
		if (method == MethodName._OnInputSubmitted)
		{
			return true;
		}
		if (method == MethodName._OnInputChanged)
		{
			return true;
		}
		if (method == MethodName._HandleAutocomplete)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._history)
		{
			_history = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName._historyIndex)
		{
			_historyIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._isOpen)
		{
			_isOpen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._autocompleteIndex)
		{
			_autocompleteIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._autocompleteList)
		{
			_autocompleteList = VariantUtils.ConvertToArray<string>(in value);
			return true;
		}
		if (name == PropertyName._welcomeShown)
		{
			_welcomeShown = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._consolePanel)
		{
			_consolePanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._outputLog)
		{
			_outputLog = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName._inputLine)
		{
			_inputLine = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._history)
		{
			value = VariantUtils.CreateFromArray(_history);
			return true;
		}
		if (name == PropertyName._historyIndex)
		{
			value = VariantUtils.CreateFrom(in _historyIndex);
			return true;
		}
		if (name == PropertyName._isOpen)
		{
			value = VariantUtils.CreateFrom(in _isOpen);
			return true;
		}
		if (name == PropertyName._autocompleteIndex)
		{
			value = VariantUtils.CreateFrom(in _autocompleteIndex);
			return true;
		}
		if (name == PropertyName._autocompleteList)
		{
			value = VariantUtils.CreateFromArray(_autocompleteList);
			return true;
		}
		if (name == PropertyName._welcomeShown)
		{
			value = VariantUtils.CreateFrom(in _welcomeShown);
			return true;
		}
		if (name == PropertyName._consolePanel)
		{
			value = VariantUtils.CreateFrom(in _consolePanel);
			return true;
		}
		if (name == PropertyName._outputLog)
		{
			value = VariantUtils.CreateFrom(in _outputLog);
			return true;
		}
		if (name == PropertyName._inputLine)
		{
			value = VariantUtils.CreateFrom(in _inputLine);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Array, PropertyName._history, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._historyIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isOpen, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._autocompleteIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._autocompleteList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._welcomeShown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._consolePanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._outputLog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inputLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._history, Variant.CreateFrom(_history));
		info.AddProperty(PropertyName._historyIndex, Variant.From(in _historyIndex));
		info.AddProperty(PropertyName._isOpen, Variant.From(in _isOpen));
		info.AddProperty(PropertyName._autocompleteIndex, Variant.From(in _autocompleteIndex));
		info.AddProperty(PropertyName._autocompleteList, Variant.CreateFrom(_autocompleteList));
		info.AddProperty(PropertyName._welcomeShown, Variant.From(in _welcomeShown));
		info.AddProperty(PropertyName._consolePanel, Variant.From(in _consolePanel));
		info.AddProperty(PropertyName._outputLog, Variant.From(in _outputLog));
		info.AddProperty(PropertyName._inputLine, Variant.From(in _inputLine));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._history, out var value))
		{
			_history = value.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName._historyIndex, out var value2))
		{
			_historyIndex = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName._isOpen, out var value3))
		{
			_isOpen = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._autocompleteIndex, out var value4))
		{
			_autocompleteIndex = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._autocompleteList, out var value5))
		{
			_autocompleteList = value5.AsGodotArray<string>();
		}
		if (info.TryGetProperty(PropertyName._welcomeShown, out var value6))
		{
			_welcomeShown = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._consolePanel, out var value7))
		{
			_consolePanel = value7.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._outputLog, out var value8))
		{
			_outputLog = value8.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName._inputLine, out var value9))
		{
			_inputLine = value9.As<LineEdit>();
		}
	}
}
