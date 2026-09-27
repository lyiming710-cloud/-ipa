using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.ModSystem;

namespace PVZHE.ModEditor.Core;

[ScriptPath("res://addons/ModEditor/Core/ModEditorManager.cs")]
public class ModEditorManager : Node
{
	[Signal]
	public delegate void EditorOpenedEventHandler();

	[Signal]
	public delegate void EditorClosedEventHandler();

	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName OpenEditor = "OpenEditor";

		public static readonly StringName SetEditorWindowProjectTitle = "SetEditorWindowProjectTitle";

		public static readonly StringName BuildEditorWindowTitle = "BuildEditorWindowTitle";

		public static readonly StringName GetCurrentScreenUsableRect = "GetCurrentScreenUsableRect";

		public static readonly StringName GetEditorWindowInitialSize = "GetEditorWindowInitialSize";

		public static readonly StringName GetEditorWindowMinSize = "GetEditorWindowMinSize";

		public static readonly StringName CloseEditor = "CloseEditor";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ToggleEditor = "ToggleEditor";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName SetCustomCursorBlockedForEditor = "SetCustomCursorBlockedForEditor";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _editorPanelScene = "_editorPanelScene";

		public static readonly StringName _editorWindow = "_editorWindow";

		public static readonly StringName _editorPanel = "_editorPanel";

		public static readonly StringName _isOpen = "_isOpen";

		public static readonly StringName _customCursorBlockedForEditor = "_customCursorBlockedForEditor";

		public static readonly StringName _editorWindowProjectTitle = "_editorWindowProjectTitle";
	}

	public new class SignalName : Node.SignalName
	{
		public static readonly StringName EditorOpened = "EditorOpened";

		public static readonly StringName EditorClosed = "EditorClosed";
	}

	private const string EditorWindowScenePath = "res://addons/ModEditor/GUI/XWModEditorWindow.tscn";

	private static PackedScene _editorWindowScene;

	[Export(PropertyHint.None, "")]
	private PackedScene _editorPanelScene;

	private Window _editorWindow;

	private Control _editorPanel;

	private bool _isOpen;

	private bool _customCursorBlockedForEditor;

	private string _editorWindowProjectTitle = "";

	private const string EditorWindowBaseTitle = "PVZ Mod 编辑器";

	private static readonly Vector2I PreferredEditorWindowSize = new Vector2I(1600, 900);

	private static readonly Vector2I MinimumEditorWindowSize = new Vector2I(960, 540);

	private EditorOpenedEventHandler backing_EditorOpened;

	private EditorClosedEventHandler backing_EditorClosed;

	public static ModEditorManager Instance { get; private set; }

	public event EditorOpenedEventHandler EditorOpened
	{
		add
		{
			backing_EditorOpened = (EditorOpenedEventHandler)Delegate.Combine(backing_EditorOpened, value);
		}
		remove
		{
			backing_EditorOpened = (EditorOpenedEventHandler)Delegate.Remove(backing_EditorOpened, value);
		}
	}

	public event EditorClosedEventHandler EditorClosed
	{
		add
		{
			backing_EditorClosed = (EditorClosedEventHandler)Delegate.Combine(backing_EditorClosed, value);
		}
		remove
		{
			backing_EditorClosed = (EditorClosedEventHandler)Delegate.Remove(backing_EditorClosed, value);
		}
	}

	public override void _Ready()
	{
		Instance = this;
		ProcessMode = ProcessModeEnum.Always;
	}

	public void OpenEditor()
	{
		if (Global.IsMobile)
		{
			return;
		}
		if (_editorWindow == null)
		{
			if (_editorPanelScene == null)
			{
				GD.PrintErr("[ModEditor] _editorPanelScene 未绑定，无法打开编辑器。");
				return;
			}
			Rect2I currentScreenUsableRect = GetCurrentScreenUsableRect();
			Vector2I editorWindowInitialSize = GetEditorWindowInitialSize(currentScreenUsableRect.Size);
			if (_editorWindowScene == null)
			{
				_editorWindowScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/GUI/XWModEditorWindow.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			_editorWindow = _editorWindowScene?.Instantiate<Window>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(_editorWindow))
			{
				GD.PrintErr("[ModEditor] 无法加载编辑器宿主窗口场景。");
				_editorWindow = null;
				return;
			}
			_editorWindow.Title = BuildEditorWindowTitle(_editorWindowProjectTitle);
			_editorWindow.Size = editorWindowInitialSize;
			_editorWindow.MinSize = GetEditorWindowMinSize(editorWindowInitialSize);
			_editorWindow.Position = new Vector2I(currentScreenUsableRect.Position.X + Mathf.Max(0, (currentScreenUsableRect.Size.X - editorWindowInitialSize.X) / 2), currentScreenUsableRect.Position.Y + Mathf.Max(0, (currentScreenUsableRect.Size.Y - editorWindowInitialSize.Y) / 2));
			AddChild(_editorWindow, forceReadableName: false, InternalMode.Disabled);
			_editorWindow.CloseRequested += CloseEditor;
			_editorPanel = _editorPanelScene.Instantiate<Control>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(_editorPanel))
			{
				GD.PrintErr("[ModEditor] 无法实例化编辑器主面板。");
				_editorWindow.QueueFree();
				_editorWindow = null;
				return;
			}
			_editorPanel.SetAnchorsPreset(Control.LayoutPreset.FullRect);
			_editorPanel.OffsetLeft = 0f;
			_editorPanel.OffsetTop = 0f;
			_editorPanel.OffsetRight = 0f;
			_editorPanel.OffsetBottom = 0f;
			_editorWindow.GetNode<Control>("PanelHost").AddChild(_editorPanel, forceReadableName: false, InternalMode.Disabled);
		}
		_editorWindow.Mode = Window.ModeEnum.Maximized;
		_editorWindow.Show();
		_editorWindow.GrabFocus();
		SetCustomCursorBlockedForEditor(blocked: true);
		_isOpen = true;
		EmitSignal(SignalName.EditorOpened);
	}

	public void SetEditorWindowProjectTitle(string projectName)
	{
		_editorWindowProjectTitle = (projectName ?? "").Trim();
		if (_editorWindow != null)
		{
			_editorWindow.Title = BuildEditorWindowTitle(_editorWindowProjectTitle);
		}
	}

	private static string BuildEditorWindowTitle(string projectName)
	{
		if (!string.IsNullOrWhiteSpace(projectName))
		{
			return "PVZ Mod 编辑器 - " + projectName;
		}
		return "PVZ Mod 编辑器";
	}

	private static Rect2I GetCurrentScreenUsableRect()
	{
		int windowAtScreenPosition = DisplayServer.GetWindowAtScreenPosition(DisplayServer.MouseGetPosition());
		Rect2I result = DisplayServer.ScreenGetUsableRect(windowAtScreenPosition);
		if (result.Size != Vector2I.Zero)
		{
			return result;
		}
		Vector2I vector2I = DisplayServer.ScreenGetSize(windowAtScreenPosition);
		if (vector2I == Vector2I.Zero)
		{
			vector2I = DisplayServer.ScreenGetSize();
		}
		return new Rect2I(Vector2I.Zero, vector2I);
	}

	private static Vector2I GetEditorWindowInitialSize(Vector2I usableSize)
	{
		if (usableSize == Vector2I.Zero)
		{
			return PreferredEditorWindowSize;
		}
		int b = Mathf.Max(640, usableSize.X - 48);
		int b2 = Mathf.Max(360, usableSize.Y - 48);
		return new Vector2I(Mathf.Min(PreferredEditorWindowSize.X, b), Mathf.Min(PreferredEditorWindowSize.Y, b2));
	}

	private static Vector2I GetEditorWindowMinSize(Vector2I initialSize)
	{
		return new Vector2I(Mathf.Min(MinimumEditorWindowSize.X, initialSize.X), Mathf.Min(MinimumEditorWindowSize.Y, initialSize.Y));
	}

	public void CloseEditor()
	{
		if (_editorWindow != null)
		{
			_editorWindow.Hide();
		}
		SetCustomCursorBlockedForEditor(blocked: false);
		_isOpen = false;
		EmitSignal(SignalName.EditorClosed);
	}

	public override void _ExitTree()
	{
		SetCustomCursorBlockedForEditor(blocked: false);
		ModLoader.UnloadAll();
		Instance = null;
		base._ExitTree();
	}

	public void ToggleEditor()
	{
		if (_isOpen)
		{
			CloseEditor();
		}
		else
		{
			OpenEditor();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey { Pressed: not false, Echo: false } inputEventKey && inputEventKey.Keycode == Key.F3)
		{
			ToggleEditor();
			GetViewport().SetInputAsHandled();
		}
	}

	private void SetCustomCursorBlockedForEditor(bool blocked)
	{
		if (_customCursorBlockedForEditor != blocked)
		{
			Node nodeOrNull = GetNodeOrNull<Node>("/root/Cursor");
			if (nodeOrNull != null && nodeOrNull.HasMethod("set_mod_editor_cursor_blocked"))
			{
				nodeOrNull.Call("set_mod_editor_cursor_blocked", blocked);
				_customCursorBlockedForEditor = blocked;
			}
		}
	}

	public ModEditorManager()
	{
		StartupLoadDiagnostics.Mark("autoload.constructed/ModEditorManager");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(12)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetEditorWindowProjectTitle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "projectName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildEditorWindowTitle, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "projectName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentScreenUsableRect, new PropertyInfo(Variant.Type.Rect2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.GetEditorWindowInitialSize, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "usableSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetEditorWindowMinSize, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "initialSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToggleEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetCustomCursorBlockedForEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "blocked", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.OpenEditor && args.Count == 0)
		{
			OpenEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.SetEditorWindowProjectTitle && args.Count == 1)
		{
			SetEditorWindowProjectTitle(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildEditorWindowTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildEditorWindowTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurrentScreenUsableRect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Rect2I>(GetCurrentScreenUsableRect());
			return true;
		}
		if (method == MethodName.GetEditorWindowInitialSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetEditorWindowInitialSize(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.GetEditorWindowMinSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetEditorWindowMinSize(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.CloseEditor && args.Count == 0)
		{
			CloseEditor();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleEditor && args.Count == 0)
		{
			ToggleEditor();
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCustomCursorBlockedForEditor && args.Count == 1)
		{
			SetCustomCursorBlockedForEditor(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BuildEditorWindowTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildEditorWindowTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurrentScreenUsableRect && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Rect2I>(GetCurrentScreenUsableRect());
			return true;
		}
		if (method == MethodName.GetEditorWindowInitialSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetEditorWindowInitialSize(VariantUtils.ConvertTo<Vector2I>(in args[0])));
			return true;
		}
		if (method == MethodName.GetEditorWindowMinSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetEditorWindowMinSize(VariantUtils.ConvertTo<Vector2I>(in args[0])));
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
		if (method == MethodName.OpenEditor)
		{
			return true;
		}
		if (method == MethodName.SetEditorWindowProjectTitle)
		{
			return true;
		}
		if (method == MethodName.BuildEditorWindowTitle)
		{
			return true;
		}
		if (method == MethodName.GetCurrentScreenUsableRect)
		{
			return true;
		}
		if (method == MethodName.GetEditorWindowInitialSize)
		{
			return true;
		}
		if (method == MethodName.GetEditorWindowMinSize)
		{
			return true;
		}
		if (method == MethodName.CloseEditor)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ToggleEditor)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName.SetCustomCursorBlockedForEditor)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editorPanelScene)
		{
			_editorPanelScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName._editorWindow)
		{
			_editorWindow = VariantUtils.ConvertTo<Window>(in value);
			return true;
		}
		if (name == PropertyName._editorPanel)
		{
			_editorPanel = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._isOpen)
		{
			_isOpen = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._customCursorBlockedForEditor)
		{
			_customCursorBlockedForEditor = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._editorWindowProjectTitle)
		{
			_editorWindowProjectTitle = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editorPanelScene)
		{
			value = VariantUtils.CreateFrom(in _editorPanelScene);
			return true;
		}
		if (name == PropertyName._editorWindow)
		{
			value = VariantUtils.CreateFrom(in _editorWindow);
			return true;
		}
		if (name == PropertyName._editorPanel)
		{
			value = VariantUtils.CreateFrom(in _editorPanel);
			return true;
		}
		if (name == PropertyName._isOpen)
		{
			value = VariantUtils.CreateFrom(in _isOpen);
			return true;
		}
		if (name == PropertyName._customCursorBlockedForEditor)
		{
			value = VariantUtils.CreateFrom(in _customCursorBlockedForEditor);
			return true;
		}
		if (name == PropertyName._editorWindowProjectTitle)
		{
			value = VariantUtils.CreateFrom(in _editorWindowProjectTitle);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editorPanelScene, PropertyHint.ResourceType, "PackedScene", PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable, exported: true),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isOpen, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._customCursorBlockedForEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._editorWindowProjectTitle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editorPanelScene, Variant.From(in _editorPanelScene));
		info.AddProperty(PropertyName._editorWindow, Variant.From(in _editorWindow));
		info.AddProperty(PropertyName._editorPanel, Variant.From(in _editorPanel));
		info.AddProperty(PropertyName._isOpen, Variant.From(in _isOpen));
		info.AddProperty(PropertyName._customCursorBlockedForEditor, Variant.From(in _customCursorBlockedForEditor));
		info.AddProperty(PropertyName._editorWindowProjectTitle, Variant.From(in _editorWindowProjectTitle));
		info.AddSignalEventDelegate(SignalName.EditorOpened, backing_EditorOpened);
		info.AddSignalEventDelegate(SignalName.EditorClosed, backing_EditorClosed);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editorPanelScene, out var value))
		{
			_editorPanelScene = value.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName._editorWindow, out var value2))
		{
			_editorWindow = value2.As<Window>();
		}
		if (info.TryGetProperty(PropertyName._editorPanel, out var value3))
		{
			_editorPanel = value3.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._isOpen, out var value4))
		{
			_isOpen = value4.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._customCursorBlockedForEditor, out var value5))
		{
			_customCursorBlockedForEditor = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._editorWindowProjectTitle, out var value6))
		{
			_editorWindowProjectTitle = value6.As<string>();
		}
		if (info.TryGetSignalEventDelegate<EditorOpenedEventHandler>(SignalName.EditorOpened, out var value7))
		{
			backing_EditorOpened = value7;
		}
		if (info.TryGetSignalEventDelegate<EditorClosedEventHandler>(SignalName.EditorClosed, out var value8))
		{
			backing_EditorClosed = value8;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(2)
		{
			new MethodInfo(SignalName.EditorOpened, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.EditorClosed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalEditorOpened()
	{
		EmitSignal(SignalName.EditorOpened, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalEditorClosed()
	{
		EmitSignal(SignalName.EditorClosed, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.EditorOpened && args.Count == 0)
		{
			backing_EditorOpened?.Invoke();
		}
		else if (signal == SignalName.EditorClosed && args.Count == 0)
		{
			backing_EditorClosed?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.EditorOpened)
		{
			return true;
		}
		if (signal == SignalName.EditorClosed)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
