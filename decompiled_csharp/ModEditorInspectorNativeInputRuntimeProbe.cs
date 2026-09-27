using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

[ScriptPath("res://Tests/ModEditorInspectorNativeInputRuntimeProbe.cs")]
public class ModEditorInspectorNativeInputRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SendTextCharacter = "SendTextCharacter";

		public static readonly StringName SendKey = "SendKey";

		public static readonly StringName FindPropertyEditor = "FindPropertyEditor";

		public static readonly StringName OnPropertyChanged = "OnPropertyChanged";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName ProbeTitle = "ProbeTitle";

		public static readonly StringName ProbeSpeed = "ProbeSpeed";

		public static readonly StringName ProbeColor = "ProbeColor";

		public static readonly StringName _inspector = "_inspector";

		public static readonly StringName _history = "_history";

		public static readonly StringName _notifications = "_notifications";

		public static readonly StringName _lastNotification = "_lastNotification";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWInspector _inspector;

	private XWUndoRedoManager _history;

	private int _notifications;

	private string _lastNotification = "";

	[Export(PropertyHint.None, "")]
	public string ProbeTitle { get; set; } = "focus-target";

	[Export(PropertyHint.None, "")]
	public double ProbeSpeed { get; set; } = 2.0;

	[Export(PropertyHint.None, "")]
	public Color ProbeColor { get; set; } = new Color(0.18f, 0.31f, 0.47f, 0.44f);

	public override async void _Ready()
	{
		_ = 5;
		try
		{
			ModEditorManager modEditorManager = ModEditorManager.Instance;
			if (!GodotObject.IsInstanceValid(modEditorManager))
			{
				modEditorManager = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Core/ModEditorManager.tscn", null, ResourceLoader.CacheMode.Reuse)?.Instantiate<ModEditorManager>(PackedScene.GenEditState.Disabled);
				if (GodotObject.IsInstanceValid(modEditorManager))
				{
					AddChild(modEditorManager, forceReadableName: false, InternalMode.Disabled);
				}
			}
			Require(GodotObject.IsInstanceValid(modEditorManager), "ModEditorManager could not be instantiated.");
			if (!GodotObject.IsInstanceValid(modEditorManager))
			{
				Finish();
				return;
			}
			await WaitFrames(2);
			SendKey(Key.F3);
			bool flag = await WaitForInspector(900);
			Require(flag, "F3 did not initialize the real Inspector.");
			if (!flag)
			{
				Finish();
				return;
			}
			bool flag2 = await EnterEditorSurface(900);
			Require(flag2, "F3 Inspector surface did not become visible.");
			if (!flag2)
			{
				Finish();
				return;
			}
			bool window = FindAncestorWindow(_inspector) != null;
			_inspector.ObjectPropertyChanged += OnPropertyChanged;
			XWEditorInterface.Instance.InspectObject(this);
			XWEditorInterface.Instance.FocusPanel("inspector");
			await WaitFrames(8);
			var (nativeSpinFocus, singleSpinHistory) = await ProbeNativeSpinInput();
			var (value, value2, value3) = await ProbeNativeColorPopup();
			GD.Print($"[MOD_EDITOR_INSPECTOR_NATIVE_INPUT_PROBE] window={window} nativeSpinFocus={nativeSpinFocus} singleSpinHistory={singleSpinHistory} popupOpened={value} nativeColorPopup={value2} singleColorHistory={value3} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private async Task<(bool focus, bool history)> ProbeNativeSpinInput()
	{
		XWInspectorPropertyEditorBase speedEditor = FindPropertyEditor("ProbeSpeed");
		XWInspectorPropertyEditorBase root = FindPropertyEditor("ProbeTitle");
		LineEdit spinLine = FindDescendant<SpinBox>(speedEditor)?.GetLineEdit();
		LineEdit titleLine = FindDescendant<LineEdit>(root);
		Require(GodotObject.IsInstanceValid(spinLine) && GodotObject.IsInstanceValid(titleLine), "SpinBox internal LineEdit or focus target was not created.");
		if (!GodotObject.IsInstanceValid(spinLine) || !GodotObject.IsInstanceValid(titleLine))
		{
			return (focus: false, history: false);
		}
		double before = ProbeSpeed;
		_history.ClearHistory();
		_notifications = 0;
		_lastNotification = "";
		int beforeVersion = _history.GetVersion();
		spinLine.GrabFocus();
		await WaitFrames(2);
		bool focus = spinLine.HasFocus() && speedEditor.IsContinuousEditActive;
		spinLine.SelectAll();
		string text = "12.25";
		for (int i = 0; i < text.Length; i++)
		{
			SendTextCharacter(text[i]);
			await WaitFrames(1);
		}
		await WaitFrames(2);
		double valueAfterTyping = ProbeSpeed;
		string textAfterTyping = spinLine.Text;
		bool historyAfterTyping = _history.HasUndo();
		int notificationsAfterTyping = _notifications;
		bool preview = textAfterTyping == "12.25" && !historyAfterTyping && notificationsAfterTyping == 0;
		titleLine.GrabFocus();
		await WaitFrames(5);
		bool oneAction = _history.GetVersion() - beforeVersion == 1 && _history.HasUndo() && _notifications == 1 && !speedEditor.IsContinuousEditActive;
		bool undo = oneAction && _history.Undo();
		await WaitFrames(3);
		undo = undo && Math.Abs(ProbeSpeed - before) < 0.001 && !_history.HasUndo();
		bool redo = undo && _history.Redo();
		await WaitFrames(3);
		redo = redo && Math.Abs(ProbeSpeed - 12.25) < 0.001;
		bool flag = preview & oneAction & undo & redo;
		GD.Print($"[MOD_EDITOR_INSPECTOR_NATIVE_INPUT_DETAIL] spin focus={focus} preview={preview} oneAction={oneAction} undo={undo} redo={redo} value={ProbeSpeed} typedValue={valueAfterTyping} typedText='{textAfterTyping}' typedHistory={historyAfterTyping} typedNotifications={notificationsAfterTyping} text='{spinLine.Text}' notifications={_notifications} versionDelta={_history.GetVersion() - beforeVersion} active={speedEditor.IsContinuousEditActive}");
		Require(focus, "Real SpinBox LineEdit focus did not begin a continuous edit session.");
		Require(flag, "Real SpinBox keyboard staging/focus migration did not create exactly one undo action.");
		return (focus: focus, history: flag);
	}

	private async Task<(bool opened, bool popup, bool history)> ProbeNativeColorPopup()
	{
		XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase = FindPropertyEditor("ProbeColor");
		XWInspectorPropertyEditorColor colorEditor = xWInspectorPropertyEditorBase as XWInspectorPropertyEditorColor;
		ColorPickerButton picker = FindDescendant<ColorPickerButton>(colorEditor);
		PopupPanel popup = picker?.GetPopup();
		ColorPicker nativePicker = picker?.GetPicker();
		Require(GodotObject.IsInstanceValid(colorEditor) && GodotObject.IsInstanceValid(picker) && GodotObject.IsInstanceValid(popup) && GodotObject.IsInstanceValid(nativePicker), "Native color picker controls were not created.");
		if (!GodotObject.IsInstanceValid(picker) || !GodotObject.IsInstanceValid(popup) || !GodotObject.IsInstanceValid(nativePicker))
		{
			return (opened: false, popup: false, history: false);
		}
		Color before = ProbeColor;
		_history.ClearHistory();
		_notifications = 0;
		_lastNotification = "";
		int beforeVersion = _history.GetVersion();
		picker.GrabFocus();
		await WaitFrames(1);
		SendKey(Key.Enter);
		await WaitFrames(4);
		bool opened = popup.Visible && colorEditor.IsContinuousEditActive;
		if (!opened)
		{
			picker.EmitSignal(BaseButton.SignalName.Pressed);
			popup.PopupCentered();
			await WaitFrames(3);
			opened = popup.Visible && colorEditor.IsContinuousEditActive;
		}
		bool activeAtOpen = colorEditor.IsContinuousEditActive;
		int versionAtOpen = _history.GetVersion() - beforeVersion;
		int notificationsAtOpen = _notifications;
		string notificationAtOpen = _lastNotification;
		Color color = new Color(0.34f, 0.72f, 0.29f, 0.51f);
		Color final = new Color(0.91f, 0.28f, 0.16f, 0.68f);
		nativePicker.Color = color;
		nativePicker.EmitSignal(ColorPicker.SignalName.ColorChanged, color);
		await WaitFrames(2);
		bool activeAfterStep1 = colorEditor.IsContinuousEditActive;
		int versionAfterStep1 = _history.GetVersion() - beforeVersion;
		int notificationsAfterStep1 = _notifications;
		nativePicker.Color = final;
		nativePicker.EmitSignal(ColorPicker.SignalName.ColorChanged, final);
		await WaitFrames(3);
		bool preview = ProbeColor.IsEqualApprox(final) && !_history.HasUndo() && _notifications == 0;
		popup.Hide();
		await WaitFrames(6);
		bool popupClosed = !popup.Visible && !colorEditor.IsContinuousEditActive;
		bool oneAction = _history.GetVersion() - beforeVersion == 1 && _history.HasUndo() && _notifications == 1;
		bool undo = oneAction && _history.Undo();
		await WaitFrames(3);
		undo = undo && ProbeColor.IsEqualApprox(before) && !_history.HasUndo();
		bool redo = undo && _history.Redo();
		await WaitFrames(3);
		redo = redo && ProbeColor.IsEqualApprox(final);
		bool flag = preview & popupClosed;
		bool flag2 = flag & oneAction & undo & redo;
		GD.Print($"[MOD_EDITOR_INSPECTOR_NATIVE_INPUT_DETAIL] color opened={opened} preview={preview} popupClosed={popupClosed} oneAction={oneAction} undo={undo} redo={redo} value={ProbeColor} notifications={_notifications} versionDelta={_history.GetVersion() - beforeVersion} activeAtOpen={activeAtOpen} openVersion={versionAtOpen} openNotifications={notificationsAtOpen} openNotification='{notificationAtOpen}' lastNotification='{_lastNotification}' step1Active={activeAfterStep1} step1Version={versionAfterStep1} step1Notifications={notificationsAfterStep1} visible={popup.Visible} active={colorEditor.IsContinuousEditActive}");
		Require(opened, "ColorPickerButton popup did not open a continuous edit session.");
		Require(flag, "Native ColorPicker popup did not preview and close through PopupClosed.");
		Require(flag2, "Native ColorPicker popup lifecycle did not create exactly one undo action.");
		return (opened: opened, popup: flag, history: flag2);
	}

	private static void SendTextCharacter(char character)
	{
		Key key = (Key)char.ToUpperInvariant(character);
		Input.ParseInputEvent(new InputEventKey
		{
			Keycode = key,
			PhysicalKeycode = key,
			Unicode = character,
			Pressed = true
		});
		Input.ParseInputEvent(new InputEventKey
		{
			Keycode = key,
			PhysicalKeycode = key,
			Unicode = character,
			Pressed = false
		});
	}

	private static void SendKey(Key key)
	{
		Input.ParseInputEvent(new InputEventKey
		{
			Keycode = key,
			PhysicalKeycode = key,
			Pressed = true
		});
		Input.ParseInputEvent(new InputEventKey
		{
			Keycode = key,
			PhysicalKeycode = key,
			Pressed = false
		});
	}

	private async Task<bool> WaitForInspector(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			if (XWEditorInterface.Instance?.GetInspector() is XWInspector xWInspector && GodotObject.IsInstanceValid(xWInspector))
			{
				_inspector = xWInspector;
				_history = XWEditorInterface.Instance.GetUndoRedoManager();
				return GodotObject.IsInstanceValid(_history);
			}
			await WaitFrames(1);
		}
		return false;
	}

	private async Task<bool> EnterEditorSurface(int maxFrames)
	{
		for (int i = 0; i < maxFrames; i++)
		{
			Control control = XWEditorInterface.Instance?.GetEditorPanel();
			Node instance = control?.FindChild("LoadingOverlay", recursive: true, owned: false);
			Control control2 = control?.FindChild("ProjectManagerPanel", recursive: true, owned: false) as Control;
			if (GodotObject.IsInstanceValid(control) && !GodotObject.IsInstanceValid(instance) && GodotObject.IsInstanceValid(control2))
			{
				control2.Hide();
				XWEditorInterface.Instance.FocusPanel("inspector");
				await WaitFrames(3);
				return _inspector.IsVisibleInTree();
			}
			await WaitFrames(1);
		}
		return false;
	}

	private XWInspectorPropertyEditorBase FindPropertyEditor(string propertyName)
	{
		foreach (Node item in _inspector.FindChildren("*", "", recursive: true, owned: false))
		{
			if (item is XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase && GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase.Property) && xWInspectorPropertyEditorBase.Property.PropName.ToString() == propertyName)
			{
				return xWInspectorPropertyEditorBase;
			}
		}
		return null;
	}

	private static T FindDescendant<T>(Node root) where T : Node
	{
		if (!GodotObject.IsInstanceValid(root))
		{
			return null;
		}
		foreach (Node item in root.FindChildren("*", "", recursive: true, owned: false))
		{
			if (item is T result)
			{
				return result;
			}
		}
		return null;
	}

	private void OnPropertyChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
		_notifications++;
		_lastNotification = $"{property}:{field}={value}";
	}

	private static Window FindAncestorWindow(Node node)
	{
		Node node2 = node;
		while (GodotObject.IsInstanceValid(node2))
		{
			if (node2 is Window result)
			{
				return result;
			}
			node2 = node2.GetParent();
		}
		return null;
	}

	private async Task WaitFrames(int count)
	{
		for (int i = 0; i < count; i++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
			GD.PrintErr("[MOD_EDITOR_INSPECTOR_NATIVE_INPUT_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		foreach (string failure in _failures)
		{
			GD.PrintErr("[MOD_EDITOR_INSPECTOR_NATIVE_INPUT_PROBE_FAILURE] " + failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(8)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SendTextCharacter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "character", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SendKey, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindPropertyEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPropertyChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.FindAncestorWindow, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Window"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.Require, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "condition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.SendTextCharacter && args.Count == 1)
		{
			SendTextCharacter(VariantUtils.ConvertTo<char>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendKey && args.Count == 1)
		{
			SendKey(VariantUtils.ConvertTo<Key>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindPropertyEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(FindPropertyEditor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnPropertyChanged && args.Count == 4)
		{
			OnPropertyChanged(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.Require && args.Count == 2)
		{
			Require(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Finish && args.Count == 0)
		{
			Finish();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.SendTextCharacter && args.Count == 1)
		{
			SendTextCharacter(VariantUtils.ConvertTo<char>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SendKey && args.Count == 1)
		{
			SendKey(VariantUtils.ConvertTo<Key>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindAncestorWindow && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Window>(FindAncestorWindow(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName.SendTextCharacter)
		{
			return true;
		}
		if (method == MethodName.SendKey)
		{
			return true;
		}
		if (method == MethodName.FindPropertyEditor)
		{
			return true;
		}
		if (method == MethodName.OnPropertyChanged)
		{
			return true;
		}
		if (method == MethodName.FindAncestorWindow)
		{
			return true;
		}
		if (method == MethodName.Require)
		{
			return true;
		}
		if (method == MethodName.Finish)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.ProbeTitle)
		{
			ProbeTitle = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ProbeSpeed)
		{
			ProbeSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName.ProbeColor)
		{
			ProbeColor = VariantUtils.ConvertTo<Color>(in value);
			return true;
		}
		if (name == PropertyName._inspector)
		{
			_inspector = VariantUtils.ConvertTo<XWInspector>(in value);
			return true;
		}
		if (name == PropertyName._history)
		{
			_history = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName._notifications)
		{
			_notifications = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._lastNotification)
		{
			_lastNotification = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ProbeTitle)
		{
			value = VariantUtils.CreateFrom<string>(ProbeTitle);
			return true;
		}
		if (name == PropertyName.ProbeSpeed)
		{
			value = VariantUtils.CreateFrom<double>(ProbeSpeed);
			return true;
		}
		if (name == PropertyName.ProbeColor)
		{
			value = VariantUtils.CreateFrom<Color>(ProbeColor);
			return true;
		}
		if (name == PropertyName._inspector)
		{
			value = VariantUtils.CreateFrom(in _inspector);
			return true;
		}
		if (name == PropertyName._history)
		{
			value = VariantUtils.CreateFrom(in _history);
			return true;
		}
		if (name == PropertyName._notifications)
		{
			value = VariantUtils.CreateFrom(in _notifications);
			return true;
		}
		if (name == PropertyName._lastNotification)
		{
			value = VariantUtils.CreateFrom(in _lastNotification);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ProbeTitle, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Float, PropertyName.ProbeSpeed, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Color, PropertyName.ProbeColor, PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._inspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._history, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._notifications, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._lastNotification, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.ProbeTitle, Variant.From<string>(ProbeTitle));
		info.AddProperty(PropertyName.ProbeSpeed, Variant.From<double>(ProbeSpeed));
		info.AddProperty(PropertyName.ProbeColor, Variant.From<Color>(ProbeColor));
		info.AddProperty(PropertyName._inspector, Variant.From(in _inspector));
		info.AddProperty(PropertyName._history, Variant.From(in _history));
		info.AddProperty(PropertyName._notifications, Variant.From(in _notifications));
		info.AddProperty(PropertyName._lastNotification, Variant.From(in _lastNotification));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.ProbeTitle, out var value))
		{
			ProbeTitle = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ProbeSpeed, out var value2))
		{
			ProbeSpeed = value2.As<double>();
		}
		if (info.TryGetProperty(PropertyName.ProbeColor, out var value3))
		{
			ProbeColor = value3.As<Color>();
		}
		if (info.TryGetProperty(PropertyName._inspector, out var value4))
		{
			_inspector = value4.As<XWInspector>();
		}
		if (info.TryGetProperty(PropertyName._history, out var value5))
		{
			_history = value5.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._notifications, out var value6))
		{
			_notifications = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName._lastNotification, out var value7))
		{
			_lastNotification = value7.As<string>();
		}
	}
}
