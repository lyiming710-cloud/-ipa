using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Blueprint;
using PVZHE.ModEditor.Core;

[ScriptPath("res://Tests/ModEditorBlueprintDocumentIsolationRuntimeProbe.cs")]
public class ModEditorBlueprintDocumentIsolationRuntimeProbe : Node
{
	public new class MethodName : Node.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName CreateBlueprint = "CreateBlueprint";

		public static readonly StringName FindProbePort = "FindProbePort";

		public static readonly StringName FirstGraph = "FirstGraph";

		public static readonly StringName FindAncestorWindow = "FindAncestorWindow";

		public static readonly StringName Require = "Require";

		public static readonly StringName Finish = "Finish";
	}

	public new class PropertyName : Node.PropertyName
	{
		public static readonly StringName _editor = "_editor";

		public static readonly StringName _history = "_history";
	}

	public new class SignalName : Node.SignalName
	{
	}

	private const string BlueprintAPath = "user://mod_editor_blueprint_document_a.tres";

	private const string BlueprintBPath = "user://mod_editor_blueprint_document_b.tres";

	private const int ProbeNodeId = 41;

	private const string EditablePortName = "条件";

	private readonly List<string> _failures = new List<string>();

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private XWBPEditor _editor;

	private XWUndoRedoManager _history;

	public override async void _Ready()
	{
		bool window = false;
		bool distinctHistory = false;
		bool restoredHistory = false;
		bool inlineCommitted = false;
		bool switchFlushA = false;
		bool undoOnlyB = false;
		bool redoB = false;
		bool switchFlushB = false;
		bool exitFlush = false;
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
				Finish(window: false, distinctHistory: false, restoredHistory: false, inlineCommitted: false, switchFlushA: false, undoOnlyB: false, redoB: false, switchFlushB: false, exitFlush: false);
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
			bool flag = await WaitForEditor(900);
			Require(flag, "F3 did not initialize the real Blueprint editor within 900 frames.");
			if (!flag)
			{
				Finish(window: false, distinctHistory: false, restoredHistory: false, inlineCommitted: false, switchFlushA: false, undoOnlyB: false, redoB: false, switchFlushB: false, exitFlush: false);
				return;
			}
			window = FindAncestorWindow(_editor) != null;
			Require(window, "Blueprint editor is not mounted under the F3 ModEditor window.");
			XWBPScript blueprintA = CreateBlueprint("user://mod_editor_blueprint_document_a.tres", "DocumentA");
			XWBPScript blueprintB = CreateBlueprint("user://mod_editor_blueprint_document_b.tres", "DocumentB");
			Require(GodotObject.IsInstanceValid(blueprintA) && GodotObject.IsInstanceValid(blueprintB), "Could not create the isolated Blueprint documents.");
			if (!GodotObject.IsInstanceValid(blueprintA) || !GodotObject.IsInstanceValid(blueprintB))
			{
				Finish(window, distinctHistory: false, restoredHistory: false, inlineCommitted: false, switchFlushA: false, undoOnlyB: false, redoB: false, switchFlushB: false, exitFlush: false);
				return;
			}
			_editor.Init(blueprintA);
			await WaitFrames(4);
			int historyA = _editor.CurrentBlueprintHistoryId;
			XWBPScriptData bpScriptData = _editor.BpScriptData;
			XWBPNodePortData portA = FindProbePort(bpScriptData);
			inlineCommitted = await ToggleVisiblePort(value: true);
			Require(inlineCommitted && (portA?.Value.AsBool() ?? false), "Blueprint A inline port editor did not commit through the graph surface.");
			_editor.Init(blueprintB);
			await WaitFrames(4);
			int currentBlueprintHistoryId = _editor.CurrentBlueprintHistoryId;
			distinctHistory = historyA > 0 && currentBlueprintHistoryId > 0 && historyA != currentBlueprintHistoryId && _history.GetCurrentHistoryType() == currentBlueprintHistoryId;
			Require(distinctHistory, "Blueprint A and B did not receive independent active history scopes.");
			switchFlushA = ReadDiskPortValue("user://mod_editor_blueprint_document_a.tres") == true;
			Require(switchFlushA, "Blueprint A inline edit was lost when switching immediately to Blueprint B.");
			XWBPScriptData bpScriptData2 = _editor.BpScriptData;
			XWBPNodePortData portB = FindProbePort(bpScriptData2);
			Require(await ToggleVisiblePort(value: true) && (portB?.Value.AsBool() ?? false), "Blueprint B inline port edit did not commit.");
			bool undoCalled = _history.Undo();
			await WaitFrames(3);
			int num;
			if (undoCalled)
			{
				if (portB != null && !portB.Value.AsBool())
				{
					num = ((portA?.Value.AsBool() ?? false) ? 1 : 0);
					goto IL_064c;
				}
			}
			num = 0;
			goto IL_064c;
			IL_064c:
			undoOnlyB = (byte)num != 0;
			Require(undoOnlyB, "Undo in Blueprint B changed the wrong Blueprint document or failed to restore its port value.");
			bool redoCalled = _history.Redo();
			await WaitFrames(3);
			redoB = redoCalled && (portB?.Value.AsBool() ?? false) && (portA?.Value.AsBool() ?? false);
			Require(redoB, "Redo in Blueprint B did not restore only Blueprint B's inline value.");
			_editor.Init(blueprintA);
			await WaitFrames(4);
			int currentBlueprintHistoryId2 = _editor.CurrentBlueprintHistoryId;
			restoredHistory = historyA == currentBlueprintHistoryId2 && _history.GetCurrentHistoryType() == historyA;
			Require(restoredHistory, "Returning to Blueprint A did not restore its original history session.");
			switchFlushB = ReadDiskPortValue("user://mod_editor_blueprint_document_b.tres") == true;
			Require(switchFlushB, "Blueprint B redo state was lost when switching immediately back to Blueprint A.");
			Require(await ToggleVisiblePort(value: false), "Blueprint A exit-flush edit did not reach the inline port.");
			_editor.QueueFree();
			await WaitFrames(4);
			exitFlush = ReadDiskPortValue("user://mod_editor_blueprint_document_a.tres") == false;
			Require(exitFlush, "Blueprint editor exit discarded the pending inline port edit.");
		}
		catch (Exception ex)
		{
			_failures.Add(ex.ToString());
		}
		Finish(window, distinctHistory, restoredHistory, inlineCommitted, switchFlushA, undoOnlyB, redoB, switchFlushB, exitFlush);
	}

	private XWBPScript CreateBlueprint(string path, string resourceName)
	{
		XWBPScript xWBPScript = XWBPScript.Create();
		xWBPScript.ResourceName = resourceName;
		XWBPScriptData data = xWBPScript.Deserialize();
		XWBPGraphData xWBPGraphData = FirstGraph(data);
		if (xWBPGraphData == null)
		{
			return null;
		}
		XWBPNodeData xWBPNodeData = new XWBPNodeBranch().CreateNodeData();
		xWBPNodeData.Id = 41;
		xWBPNodeData.Position = new Vector2(96f, 72f);
		xWBPGraphData.AddNodePreserveId(xWBPNodeData);
		xWBPScript.Serialize(data);
		Error error = ResourceSaver.Save(xWBPScript, path, ResourceSaver.SaverFlags.None);
		Require(error == Error.Ok, $"Could not save {resourceName}: {error}.");
		if (error != Error.Ok)
		{
			return null;
		}
		return ResourceLoader.Load<XWBPScript>(path, "", ResourceLoader.CacheMode.Replace);
	}

	private async Task<bool> ToggleVisiblePort(bool value)
	{
		XWBPGraphNode xWBPGraphNode = (_editor?.GetGraphEditor())?.GetGraphNode(41);
		if (!GodotObject.IsInstanceValid(xWBPGraphNode) || !xWBPGraphNode.PortWidgets.TryGetValue("条件", out var value2) || !GodotObject.IsInstanceValid(value2))
		{
			Require(condition: false, "The Branch condition inline editor is unavailable in the real Blueprint graph.");
			return false;
		}
		CheckBox checkBox = value2.FindChild("CheckBox", recursive: true, owned: false) as CheckBox;
		Require(GodotObject.IsInstanceValid(checkBox), "The Branch condition CheckBox is unavailable.");
		if (!GodotObject.IsInstanceValid(checkBox))
		{
			return false;
		}
		int versionBefore = _history.GetVersion();
		checkBox.ButtonPressed = value;
		await WaitFrames(3);
		XWBPNodePortData xWBPNodePortData = FindProbePort(_editor.BpScriptData);
		return xWBPNodePortData != null && xWBPNodePortData.Value.AsBool() == value && _history.GetVersion() == versionBefore + 1;
	}

	private static XWBPNodePortData FindProbePort(XWBPScriptData data)
	{
		XWBPNodeData xWBPNodeData = FirstGraph(data)?.GetNode(41);
		if (xWBPNodeData == null)
		{
			return null;
		}
		foreach (XWBPNodePortData inputPort in xWBPNodeData.InputPorts)
		{
			if (inputPort != null && inputPort.Name == "条件")
			{
				return inputPort;
			}
		}
		return null;
	}

	private static bool? ReadDiskPortValue(string path)
	{
		XWBPScript xWBPScript = ResourceLoader.Load<XWBPScript>(path, "", ResourceLoader.CacheMode.Ignore);
		return (GodotObject.IsInstanceValid(xWBPScript) ? FindProbePort(xWBPScript.Deserialize()) : null)?.Value.AsBool();
	}

	private static XWBPGraphData FirstGraph(XWBPScriptData data)
	{
		if (data == null)
		{
			return null;
		}
		using (Dictionary<int, XWBPGraphData>.ValueCollection.Enumerator enumerator = data.Graphs.Values.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
		}
		return null;
	}

	private async Task<bool> WaitForEditor(int maxFrames)
	{
		for (int frame = 0; frame < maxFrames; frame++)
		{
			if (XWEditorInterface.Instance?.GetBlueprintEditor() is XWBPEditor xWBPEditor && GodotObject.IsInstanceValid(xWBPEditor) && xWBPEditor.IsInsideTree())
			{
				_editor = xWBPEditor;
				_history = XWEditorInterface.Instance.GetUndoRedoManager();
				return GodotObject.IsInstanceValid(_history);
			}
			await WaitFrames(1);
		}
		return false;
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
		for (int index = 0; index < count; index++)
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}

	private void Require(bool condition, string message)
	{
		if (!condition)
		{
			_failures.Add(message);
		}
	}

	private void Finish(bool window, bool distinctHistory, bool restoredHistory, bool inlineCommitted, bool switchFlushA, bool undoOnlyB, bool redoB, bool switchFlushB, bool exitFlush)
	{
		GD.Print($"[MOD_EDITOR_BLUEPRINT_DOCUMENT_ISOLATION_PROBE] window={window} distinctHistory={distinctHistory} restoredHistory={restoredHistory} inlineCommitted={inlineCommitted} switchFlushA={switchFlushA} undoOnlyB={undoOnlyB} redoB={redoB} switchFlushB={switchFlushB} exitFlush={exitFlush} failures={_failures.Count} elapsedMs={_stopwatch.ElapsedMilliseconds}");
		foreach (string failure in _failures)
		{
			GD.PrintErr(failure);
		}
		GetTree().Quit((_failures.Count != 0) ? 1 : 0);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(7)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateBlueprint, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "resourceName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindProbePort, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.FirstGraph, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
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
			new MethodInfo(MethodName.Finish, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "window", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "distinctHistory", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "restoredHistory", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "inlineCommitted", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "switchFlushA", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "undoOnlyB", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "redoB", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "switchFlushB", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "exitFlush", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.CreateBlueprint && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWBPScript>(CreateBlueprint(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindProbePort && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodePortData>(FindProbePort(VariantUtils.ConvertTo<XWBPScriptData>(in args[0])));
			return true;
		}
		if (method == MethodName.FirstGraph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(FirstGraph(VariantUtils.ConvertTo<XWBPScriptData>(in args[0])));
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
		if (method == MethodName.Finish && args.Count == 9)
		{
			Finish(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<bool>(in args[5]), VariantUtils.ConvertTo<bool>(in args[6]), VariantUtils.ConvertTo<bool>(in args[7]), VariantUtils.ConvertTo<bool>(in args[8]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.FindProbePort && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodePortData>(FindProbePort(VariantUtils.ConvertTo<XWBPScriptData>(in args[0])));
			return true;
		}
		if (method == MethodName.FirstGraph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphData>(FirstGraph(VariantUtils.ConvertTo<XWBPScriptData>(in args[0])));
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
		if (method == MethodName.CreateBlueprint)
		{
			return true;
		}
		if (method == MethodName.FindProbePort)
		{
			return true;
		}
		if (method == MethodName.FirstGraph)
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
		if (name == PropertyName._editor)
		{
			_editor = VariantUtils.ConvertTo<XWBPEditor>(in value);
			return true;
		}
		if (name == PropertyName._history)
		{
			_history = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
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
		if (name == PropertyName._history)
		{
			value = VariantUtils.CreateFrom(in _history);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._history, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editor, Variant.From(in _editor));
		info.AddProperty(PropertyName._history, Variant.From(in _history));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editor, out var value))
		{
			_editor = value.As<XWBPEditor>();
		}
		if (info.TryGetProperty(PropertyName._history, out var value2))
		{
			_history = value2.As<XWUndoRedoManager>();
		}
	}
}
