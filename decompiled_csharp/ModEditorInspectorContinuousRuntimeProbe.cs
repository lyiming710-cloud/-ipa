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

[ScriptPath("res://Tests/ModEditorInspectorContinuousRuntimeProbe.cs")]
public class ModEditorInspectorContinuousRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName FindPropertyEditor = "FindPropertyEditor";

		public static readonly StringName OnObjectPropertyChanged = "OnObjectPropertyChanged";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _inspector = "_inspector";

		public static readonly StringName _history = "_history";

		public static readonly StringName _changeNotifications = "_changeNotifications";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWInspector _inspector;

	private XWUndoRedoManager _history;

	private int _changeNotifications;

	public override async void _Ready()
	{
		_ = 7;
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
			Input.ParseInputEvent(new InputEventKey
			{
				Keycode = Key.F3,
				PhysicalKeycode = Key.F3,
				Pressed = true
			});
			Input.ParseInputEvent(new InputEventKey
			{
				Keycode = Key.F3,
				PhysicalKeycode = Key.F3,
				Pressed = false
			});
			bool flag = await WaitForInspector(900);
			Require(flag, "F3 did not initialize the real Inspector within 900 frames.");
			if (!flag)
			{
				Finish();
				return;
			}
			bool window = FindAncestorWindow(_inspector) != null;
			Require(window, "Inspector is not mounted under the F3 ModEditor window.");
			_inspector.ObjectPropertyChanged += OnObjectPropertyChanged;
			InspectorContinuousProbeResource resource = new InspectorContinuousProbeResource();
			await Inspect(resource);
			bool textSingleCommit = await ProbeText(resource);
			bool numberSingleCommit = await ProbeNumber(resource);
			bool indexedSingleCommit = await ProbeIndexedVector(resource);
			bool switchCancel = await ProbeObjectSwitchCancel(resource);
			bool value = await ProbeRefreshSuppression();
			GD.Print($"[MOD_EDITOR_INSPECTOR_CONTINUOUS_PROBE] window={window} textSingleCommit={textSingleCommit} numberSingleCommit={numberSingleCommit} indexedSingleCommit={indexedSingleCommit} switchCancel={switchCancel} refreshSuppressed={value} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish();
	}

	private async Task<bool> ProbeText(InspectorContinuousProbeResource resource)
	{
		XWInspectorPropertyEditorBase editor = FindPropertyEditor("ProbeTitle");
		LineEdit lineEdit = FindDescendant<LineEdit>(editor);
		Require(GodotObject.IsInstanceValid(editor) && GodotObject.IsInstanceValid(lineEdit), "ProbeTitle LineEdit was not created.");
		if (!GodotObject.IsInstanceValid(lineEdit))
		{
			return false;
		}
		_history.ClearHistory();
		_changeNotifications = 0;
		int beforeVersion = _history.GetVersion();
		lineEdit.EmitSignal(Control.SignalName.FocusEntered);
		bool sessionBegan = editor.IsContinuousEditActive;
		string[] array = new string[3] { "title-a", "title-ab", "title-final" };
		for (int i = 0; i < array.Length; i++)
		{
			string text = (lineEdit.Text = array[i]);
			lineEdit.EmitSignal(LineEdit.SignalName.TextChanged, text);
		}
		await WaitFrames(2);
		bool previewOnly = ((resource.ProbeTitle == "title-final") & sessionBegan) && _history.GetVersion() == beforeVersion && !_history.HasUndo() && _changeNotifications == 0;
		lineEdit.EmitSignal(LineEdit.SignalName.TextSubmitted, "title-final");
		await WaitFrames(3);
		bool committedOnce = _history.GetVersion() - beforeVersion == 1 && _history.HasUndo() && _changeNotifications == 1;
		bool undoCalled = _history.Undo();
		await WaitFrames(2);
		bool undone = undoCalled && resource.ProbeTitle == "before-title";
		bool redoCalled = _history.Redo();
		await WaitFrames(2);
		bool flag = redoCalled && resource.ProbeTitle == "title-final";
		bool flag2 = previewOnly & committedOnce & undone & flag;
		GD.Print($"[MOD_EDITOR_INSPECTOR_CONTINUOUS_DETAIL] text root={editor.ContinuousBindingRootDescription} controls={editor.ContinuousInputControlCount} sessionBegan={sessionBegan} previewOnly={previewOnly} committedOnce={committedOnce} undone={undone} redone={flag} value={resource.ProbeTitle} notifications={_changeNotifications} versionDelta={_history.GetVersion() - beforeVersion}");
		Require(flag2, "Continuous LineEdit input did not preview silently and commit one undo action/notification.");
		return flag2;
	}

	private async Task<bool> ProbeNumber(InspectorContinuousProbeResource resource)
	{
		XWInspectorPropertyEditorBase editor = FindPropertyEditor("ProbeSpeed");
		SpinBox spinBox = FindDescendant<SpinBox>(editor);
		Require(GodotObject.IsInstanceValid(editor) && GodotObject.IsInstanceValid(spinBox), "ProbeSpeed SpinBox was not created.");
		if (!GodotObject.IsInstanceValid(spinBox))
		{
			return false;
		}
		_history.ClearHistory();
		_changeNotifications = 0;
		int beforeVersion = _history.GetVersion();
		spinBox.EmitSignal(Control.SignalName.FocusEntered);
		bool sessionBegan = editor.IsContinuousEditActive;
		double[] array = new double[3] { 4.0, 7.5, 12.25 };
		foreach (double num in array)
		{
			spinBox.SetValueNoSignal(num);
			spinBox.EmitSignal(Godot.Range.SignalName.ValueChanged, num);
		}
		await WaitFrames(2);
		bool previewOnly = ((Math.Abs(resource.ProbeSpeed - 12.25) < 0.0001) & sessionBegan) && _history.GetVersion() == beforeVersion && !_history.HasUndo() && _changeNotifications == 0;
		spinBox.EmitSignal(Control.SignalName.FocusExited);
		await WaitFrames(3);
		bool committedOnce = _history.GetVersion() - beforeVersion == 1 && _history.HasUndo() && _changeNotifications == 1;
		bool undoCalled = _history.Undo();
		await WaitFrames(2);
		bool undone = undoCalled && Math.Abs(resource.ProbeSpeed - 2.0) < 0.0001;
		bool redoCalled = _history.Redo();
		await WaitFrames(2);
		bool flag = redoCalled && Math.Abs(resource.ProbeSpeed - 12.25) < 0.0001;
		bool flag2 = previewOnly & committedOnce & undone & flag;
		GD.Print($"[MOD_EDITOR_INSPECTOR_CONTINUOUS_DETAIL] number root={editor.ContinuousBindingRootDescription} controls={editor.ContinuousInputControlCount} sessionBegan={sessionBegan} previewOnly={previewOnly} committedOnce={committedOnce} undone={undone} redone={flag} value={resource.ProbeSpeed} notifications={_changeNotifications} versionDelta={_history.GetVersion() - beforeVersion}");
		Require(flag2, "Continuous SpinBox input did not preview silently and commit one undo action/notification.");
		return flag2;
	}

	private async Task<bool> ProbeObjectSwitchCancel(InspectorContinuousProbeResource resource)
	{
		await Inspect(resource);
		LineEdit lineEdit = FindDescendant<LineEdit>(FindPropertyEditor("ProbeTitle"));
		Require(GodotObject.IsInstanceValid(lineEdit), "ProbeTitle LineEdit is missing before switch-cancel test.");
		if (!GodotObject.IsInstanceValid(lineEdit))
		{
			return false;
		}
		string before = resource.ProbeTitle;
		_history.ClearHistory();
		_changeNotifications = 0;
		lineEdit.EmitSignal(Control.SignalName.FocusEntered);
		lineEdit.Text = "uncommitted-switch-value";
		lineEdit.EmitSignal(LineEdit.SignalName.TextChanged, "uncommitted-switch-value");
		await WaitFrames(1);
		bool previewApplied = resource.ProbeTitle == "uncommitted-switch-value";
		InspectorContinuousProbeResource obj = new InspectorContinuousProbeResource
		{
			ProbeTitle = "replacement"
		};
		_inspector.EditObject(obj);
		await WaitFrames(5);
		bool flag = previewApplied && resource.ProbeTitle == before && !_history.HasUndo() && _changeNotifications == 0;
		Require(flag, "Switching inspected objects did not safely cancel and restore an unfinished preview.");
		return flag;
	}

	private async Task<bool> ProbeIndexedVector(InspectorContinuousProbeResource resource)
	{
		XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase = FindPropertyEditor("ProbeOffset");
		SpinBox xSpin = null;
		if (GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase))
		{
			foreach (Node item in xWInspectorPropertyEditorBase.FindChildren("*", "", recursive: true, owned: false))
			{
				if (item is SpinBox spinBox)
				{
					xSpin = spinBox;
					break;
				}
			}
		}
		Require(GodotObject.IsInstanceValid(xSpin), "ProbeOffset X SpinBox was not created.");
		if (!GodotObject.IsInstanceValid(xSpin))
		{
			return false;
		}
		_history.ClearHistory();
		_changeNotifications = 0;
		int beforeVersion = _history.GetVersion();
		Vector2 before = resource.ProbeOffset;
		xSpin.EmitSignal(Control.SignalName.FocusEntered);
		bool sessionBegan = xWInspectorPropertyEditorBase.IsContinuousEditActive;
		double[] array = new double[3] { 8.0, 15.0, 27.0 };
		foreach (double num in array)
		{
			xSpin.SetValueNoSignal(num);
			xSpin.EmitSignal(Godot.Range.SignalName.ValueChanged, num);
		}
		await WaitFrames(2);
		bool previewOnly = ((Math.Abs(resource.ProbeOffset.X - 27f) < 0.001f) & sessionBegan) && _history.GetVersion() == beforeVersion && _changeNotifications == 0;
		xSpin.EmitSignal(Control.SignalName.FocusExited);
		await WaitFrames(3);
		bool committedOnce = _history.GetVersion() - beforeVersion == 1 && _history.HasUndo() && _changeNotifications == 1;
		bool undoCalled = _history.Undo();
		await WaitFrames(2);
		bool undone = undoCalled && resource.ProbeOffset.IsEqualApprox(before);
		bool redoCalled = _history.Redo();
		await WaitFrames(2);
		bool flag = redoCalled && Math.Abs(resource.ProbeOffset.X - 27f) < 0.001f && Math.Abs(resource.ProbeOffset.Y - before.Y) < 0.001f;
		bool flag2 = previewOnly & committedOnce & undone & flag;
		GD.Print($"[MOD_EDITOR_INSPECTOR_CONTINUOUS_DETAIL] indexed sessionBegan={sessionBegan} previewOnly={previewOnly} committedOnce={committedOnce} undone={undone} redone={flag} value={resource.ProbeOffset} notifications={_changeNotifications} versionDelta={_history.GetVersion() - beforeVersion}");
		Require(flag2, "Indexed Vector2 X input did not single-commit and round-trip through UndoRedo.");
		return flag2;
	}

	private async Task<bool> ProbeRefreshSuppression()
	{
		XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase = FindPropertyEditor("ProbeTitle");
		if (!GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase))
		{
			Require(condition: false, "Replacement ProbeTitle editor is missing for refresh suppression test.");
			return false;
		}
		_history.ClearHistory();
		_changeNotifications = 0;
		int beforeVersion = _history.GetVersion();
		xWInspectorPropertyEditorBase.UpdateValueSafely();
		await WaitFrames(2);
		bool flag = _history.GetVersion() == beforeVersion && !_history.HasUndo() && _changeNotifications == 0;
		Require(flag, "Programmatic inspector refresh created history or a save-triggering notification.");
		return flag;
	}

	private async Task Inspect(GodotObject obj)
	{
		_inspector.EditObject(obj);
		XWEditorInterface.Instance.FocusPanel("inspector");
		await WaitFrames(5);
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

	private void OnObjectPropertyChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
		_changeNotifications++;
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
			GD.PrintErr("[MOD_EDITOR_INSPECTOR_CONTINUOUS_PROBE_FAILURE] " + message);
		}
	}

	private void Finish()
	{
		if (_failures.Count > 0)
		{
			foreach (string failure in _failures)
			{
				GD.PrintErr("[MOD_EDITOR_INSPECTOR_CONTINUOUS_PROBE_FAILURE] " + failure);
			}
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(6)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindPropertyEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnObjectPropertyChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
		if (method == MethodName.FindPropertyEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWInspectorPropertyEditorBase>(FindPropertyEditor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnObjectPropertyChanged && args.Count == 4)
		{
			OnObjectPropertyChanged(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
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
		if (method == MethodName.FindPropertyEditor)
		{
			return true;
		}
		if (method == MethodName.OnObjectPropertyChanged)
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
		if (name == PropertyName._changeNotifications)
		{
			_changeNotifications = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
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
		if (name == PropertyName._changeNotifications)
		{
			value = VariantUtils.CreateFrom(in _changeNotifications);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._inspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._history, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._changeNotifications, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._inspector, Variant.From(in _inspector));
		info.AddProperty(PropertyName._history, Variant.From(in _history));
		info.AddProperty(PropertyName._changeNotifications, Variant.From(in _changeNotifications));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._inspector, out var value))
		{
			_inspector = value.As<XWInspector>();
		}
		if (info.TryGetProperty(PropertyName._history, out var value2))
		{
			_history = value2.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._changeNotifications, out var value3))
		{
			_changeNotifications = value3.As<int>();
		}
	}
}
