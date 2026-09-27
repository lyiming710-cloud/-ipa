using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.Layout;
using PVZHE.ModEditor.OutPutPanel;
using PVZHE.ModEditor.ResourceEditors;
using PVZHE.ModEditor.SceneEditor;
using PVZHE.ModEditor.ScriptEditor;

namespace PVZHE.ModEditor.Core;

[ScriptPath("res://addons/ModEditor/Core/XWEditorInterface.cs")]
public class XWEditorInterface : RefCounted
{
	public new class MethodName : RefCounted.MethodName
	{
		public static readonly StringName Initialize = "Initialize";

		public static readonly StringName SetLayoutManager = "SetLayoutManager";

		public static readonly StringName SetInspector = "SetInspector";

		public static readonly StringName SetFileSystemPanel = "SetFileSystemPanel";

		public static readonly StringName SetBlueprintEditor = "SetBlueprintEditor";

		public static readonly StringName SetScriptEditor = "SetScriptEditor";

		public static readonly StringName SetSceneTreeDock = "SetSceneTreeDock";

		public static readonly StringName Set2DSceneEditor = "Set2DSceneEditor";

		public static readonly StringName SetAnimationEditor = "SetAnimationEditor";

		public static readonly StringName SetResourceEditor = "SetResourceEditor";

		public static readonly StringName SetIssuePanel = "SetIssuePanel";

		public static readonly StringName SetOutputPanel = "SetOutputPanel";

		public static readonly StringName SetFileSystem = "SetFileSystem";

		public static readonly StringName SetEditorPanel = "SetEditorPanel";

		public static readonly StringName SetEditedSceneRoot = "SetEditedSceneRoot";

		public static readonly StringName GetUndoRedoManager = "GetUndoRedoManager";

		public static readonly StringName GetEditorData = "GetEditorData";

		public static readonly StringName GetEditorSettings = "GetEditorSettings";

		public static readonly StringName GetEditorSelection = "GetEditorSelection";

		public static readonly StringName GetSelectionHistory = "GetSelectionHistory";

		public static readonly StringName GetLayoutManager = "GetLayoutManager";

		public static readonly StringName GetInspector = "GetInspector";

		public static readonly StringName GetFileSystemPanel = "GetFileSystemPanel";

		public static readonly StringName GetBlueprintEditor = "GetBlueprintEditor";

		public static readonly StringName GetScriptEditor = "GetScriptEditor";

		public static readonly StringName GetSceneTreeDock = "GetSceneTreeDock";

		public static readonly StringName Get2DSceneEditor = "Get2DSceneEditor";

		public static readonly StringName GetCurrentSceneHistoryId = "GetCurrentSceneHistoryId";

		public static readonly StringName GetAnimationEditor = "GetAnimationEditor";

		public static readonly StringName GetResourceEditor = "GetResourceEditor";

		public static readonly StringName TryGetLoadedResourceEditor = "TryGetLoadedResourceEditor";

		public static readonly StringName EnsureResourceEditor = "EnsureResourceEditor";

		public static readonly StringName GetIssuePanel = "GetIssuePanel";

		public static readonly StringName GetOutputPanel = "GetOutputPanel";

		public static readonly StringName HasAnimationEditor = "HasAnimationEditor";

		public static readonly StringName GetFileSystem = "GetFileSystem";

		public static readonly StringName GetEditorPanel = "GetEditorPanel";

		public static readonly StringName GetEditedSceneRoot = "GetEditedSceneRoot";

		public static readonly StringName GetSceneRoot = "GetSceneRoot";

		public static readonly StringName InspectObject = "InspectObject";

		public static readonly StringName EditResource = "EditResource";

		public static readonly StringName EditNode = "EditNode";

		public static readonly StringName FocusPanel = "FocusPanel";

		public static readonly StringName ShowToast = "ShowToast";

		public static readonly StringName AddOutputMessage = "AddOutputMessage";

		public static readonly StringName LogEditorAction = "LogEditorAction";

		public static readonly StringName LogEditorUndo = "LogEditorUndo";

		public static readonly StringName LogEditorRedo = "LogEditorRedo";

		public static readonly StringName GetSelectedNodes = "GetSelectedNodes";

		public static readonly StringName SelectNode = "SelectNode";

		public static readonly StringName SetSelectedNode = "SetSelectedNode";

		public static readonly StringName ClearSelection = "ClearSelection";

		public static readonly StringName AddInspectorPlugin = "AddInspectorPlugin";

		public static readonly StringName RemoveInspectorPlugin = "RemoveInspectorPlugin";
	}

	public new class PropertyName : RefCounted.PropertyName
	{
		public static readonly StringName _undoRedoManager = "_undoRedoManager";

		public static readonly StringName _editorData = "_editorData";

		public static readonly StringName _editorSettings = "_editorSettings";

		public static readonly StringName _editorSelection = "_editorSelection";

		public static readonly StringName _selectionHistory = "_selectionHistory";

		public static readonly StringName _layoutManager = "_layoutManager";

		public static readonly StringName _inspector = "_inspector";

		public static readonly StringName _fileSystemPanel = "_fileSystemPanel";

		public static readonly StringName _blueprintEditor = "_blueprintEditor";

		public static readonly StringName _scriptEditor = "_scriptEditor";

		public static readonly StringName _sceneTreeDock = "_sceneTreeDock";

		public static readonly StringName _scene2DEditor = "_scene2DEditor";

		public static readonly StringName _animationEditor = "_animationEditor";

		public static readonly StringName _resourceEditor = "_resourceEditor";

		public static readonly StringName _issuePanel = "_issuePanel";

		public static readonly StringName _outputPanel = "_outputPanel";

		public static readonly StringName _fileSystem = "_fileSystem";

		public static readonly StringName _editorPanel = "_editorPanel";

		public static readonly StringName _editedSceneRoot = "_editedSceneRoot";
	}

	public new class SignalName : RefCounted.SignalName
	{
	}

	private XWUndoRedoManager _undoRedoManager;

	private XWEditorData _editorData;

	private XWEditorSettings _editorSettings;

	private XWEditorSelection _editorSelection;

	private XWEditorSelectionHistory _selectionHistory;

	private XWLayoutManager _layoutManager;

	private GodotObject _inspector;

	private Control _fileSystemPanel;

	private Control _blueprintEditor;

	private XWScriptEditor _scriptEditor;

	private XWSceneTreeDock _sceneTreeDock;

	private XW2DSceneEditor _scene2DEditor;

	private Control _animationEditor;

	private Control _resourceEditor;

	private readonly System.Collections.Generic.Dictionary<string, Control> _resourceEditorsByDockKey = new System.Collections.Generic.Dictionary<string, Control>();

	private Func<string, Control> _resourceEditorResolver;

	private Control _issuePanel;

	private XWOutputPanel _outputPanel;

	private RefCounted _fileSystem;

	private Control _editorPanel;

	private Node _editedSceneRoot;

	private Action<string> _showToastCallback;

	public static XWEditorInterface Instance { get; private set; }

	public static void Initialize(XWUndoRedoManager undoRedo, XWEditorData data, XWEditorSettings settings, XWEditorSelection selection, XWEditorSelectionHistory history)
	{
		Instance = new XWEditorInterface
		{
			_undoRedoManager = undoRedo,
			_editorData = data,
			_editorSettings = settings,
			_editorSelection = selection,
			_selectionHistory = history
		};
	}

	public void SetLayoutManager(XWLayoutManager layoutManager)
	{
		_layoutManager = layoutManager;
	}

	public void SetInspector(GodotObject inspector)
	{
		_inspector = inspector;
	}

	public void SetFileSystemPanel(Control fileSystemPanel)
	{
		_fileSystemPanel = fileSystemPanel;
	}

	public void SetBlueprintEditor(Control blueprintEditor)
	{
		_blueprintEditor = blueprintEditor;
	}

	public void SetScriptEditor(XWScriptEditor scriptEditor)
	{
		_scriptEditor = scriptEditor;
	}

	public void SetSceneTreeDock(XWSceneTreeDock sceneTreeDock)
	{
		_sceneTreeDock = sceneTreeDock;
	}

	public void Set2DSceneEditor(XW2DSceneEditor scene2DEditor)
	{
		_scene2DEditor = scene2DEditor;
	}

	public void SetAnimationEditor(Control animationEditor)
	{
		_animationEditor = animationEditor;
	}

	public void SetResourceEditor(Control resourceEditor)
	{
		_resourceEditor = resourceEditor;
	}

	public void SetResourceEditor(string dockKey, Control resourceEditor)
	{
		if (string.IsNullOrWhiteSpace(dockKey))
		{
			SetResourceEditor(resourceEditor);
			return;
		}
		_resourceEditorsByDockKey[dockKey] = resourceEditor;
		if (_resourceEditor == null)
		{
			_resourceEditor = resourceEditor;
		}
	}

	public void SetResourceEditorResolver(Func<string, Control> resolver)
	{
		_resourceEditorResolver = resolver;
	}

	public void SetIssuePanel(Control issuePanel)
	{
		_issuePanel = issuePanel;
	}

	public void SetOutputPanel(XWOutputPanel outputPanel)
	{
		_outputPanel = outputPanel;
	}

	public void SetFileSystem(RefCounted fileSystem)
	{
		_fileSystem = fileSystem;
	}

	public void SetEditorPanel(Control panel)
	{
		_editorPanel = panel;
	}

	public void SetEditedSceneRoot(Node root)
	{
		_editedSceneRoot = root;
	}

	public void SetShowToastCallback(Action<string> callback)
	{
		_showToastCallback = callback;
	}

	public XWUndoRedoManager GetUndoRedoManager()
	{
		return _undoRedoManager;
	}

	public XWEditorData GetEditorData()
	{
		return _editorData;
	}

	public XWEditorSettings GetEditorSettings()
	{
		return _editorSettings;
	}

	public XWEditorSelection GetEditorSelection()
	{
		return _editorSelection;
	}

	public XWEditorSelectionHistory GetSelectionHistory()
	{
		return _selectionHistory;
	}

	public XWLayoutManager GetLayoutManager()
	{
		return _layoutManager;
	}

	public GodotObject GetInspector()
	{
		return _inspector;
	}

	public Control GetFileSystemPanel()
	{
		return _fileSystemPanel;
	}

	public Control GetBlueprintEditor()
	{
		return _blueprintEditor;
	}

	public XWScriptEditor GetScriptEditor()
	{
		return _scriptEditor;
	}

	public XWSceneTreeDock GetSceneTreeDock()
	{
		return _sceneTreeDock;
	}

	public XW2DSceneEditor Get2DSceneEditor()
	{
		return _scene2DEditor;
	}

	public int GetCurrentSceneHistoryId()
	{
		if (!GodotObject.IsInstanceValid(_scene2DEditor))
		{
			return 1;
		}
		return _scene2DEditor.GetCurrentSceneHistoryId();
	}

	public Control GetAnimationEditor()
	{
		return _animationEditor;
	}

	public Control GetResourceEditor()
	{
		return _resourceEditor;
	}

	public Control GetResourceEditor(string dockKey)
	{
		if (string.IsNullOrWhiteSpace(dockKey))
		{
			return _resourceEditor;
		}
		Control control = TryGetLoadedResourceEditor(dockKey);
		if (!GodotObject.IsInstanceValid(control))
		{
			return EnsureResourceEditor(dockKey);
		}
		return control;
	}

	public Control TryGetLoadedResourceEditor(string dockKey)
	{
		if (!string.IsNullOrWhiteSpace(dockKey) && _resourceEditorsByDockKey.TryGetValue(dockKey, out var value) && GodotObject.IsInstanceValid(value))
		{
			return value;
		}
		return null;
	}

	public Control EnsureResourceEditor(string dockKey)
	{
		Control control = TryGetLoadedResourceEditor(dockKey);
		if (GodotObject.IsInstanceValid(control))
		{
			return control;
		}
		control = _resourceEditorResolver?.Invoke(dockKey);
		if (GodotObject.IsInstanceValid(control))
		{
			SetResourceEditor(dockKey, control);
		}
		return control;
	}

	public Control GetIssuePanel()
	{
		return _issuePanel;
	}

	public XWOutputPanel GetOutputPanel()
	{
		return _outputPanel;
	}

	public bool HasAnimationEditor()
	{
		if (_animationEditor != null)
		{
			return GodotObject.IsInstanceValid(_animationEditor);
		}
		return false;
	}

	public RefCounted GetFileSystem()
	{
		return _fileSystem;
	}

	public Control GetEditorPanel()
	{
		return _editorPanel;
	}

	public Node GetEditedSceneRoot()
	{
		return _editedSceneRoot;
	}

	public Node GetSceneRoot()
	{
		return _editedSceneRoot;
	}

	public void InspectObject(GodotObject obj)
	{
		if (obj is Resource res)
		{
			EditResource(res);
			return;
		}
		_inspector?.Call("EditObject", obj);
	}

	public void EditResource(Resource res)
	{
		EditResource(res, XWResourceEditContext.ForRoot(res, res?.ResourcePath ?? "", "resource_editor"));
	}

	public void EditResource(Resource res, XWResourceEditContext context)
	{
		if (GodotObject.IsInstanceValid(res))
		{
			if (context == null)
			{
				context = XWResourceEditContext.ForRoot(res, res.ResourcePath, "resource_editor");
			}
			if (!XWResourceEditorRegistry.TryOpen(res, context.ResourcePath, context))
			{
				ShowToast("可视化资源编辑器尚未就绪，资源未打开。", 2);
			}
		}
	}

	public void EditNode(Node node)
	{
		_inspector?.Call("EditObject", node);
	}

	public void FocusPanel(string key)
	{
		_layoutManager?.FocusPanel(key);
		XWEditorDock xWEditorDock = _layoutManager?.GetDock(key);
		if (GodotObject.IsInstanceValid(_scriptEditor) && (string.Equals(key, "script_editor", StringComparison.Ordinal) || (xWEditorDock != null && xWEditorDock.IsMainScreen)))
		{
			_scriptEditor.SetWorkspaceActive(string.Equals(key, "script_editor", StringComparison.Ordinal));
		}
	}

	public void ShowToast(string message, int severity = 0)
	{
		_showToastCallback?.Invoke(message);
	}

	public void AddOutputMessage(string message, XWOutputPanel.MessageType type = XWOutputPanel.MessageType.Std)
	{
		if (GodotObject.IsInstanceValid(_outputPanel))
		{
			_outputPanel.AddMessage(message, type);
		}
	}

	public void LogEditorAction(string actionName)
	{
		if (!string.IsNullOrWhiteSpace(actionName))
		{
			AddOutputMessage("动作: " + actionName, XWOutputPanel.MessageType.Editor);
		}
	}

	public void LogEditorUndo(string actionName)
	{
		if (!string.IsNullOrWhiteSpace(actionName))
		{
			AddOutputMessage("撤销: " + actionName, XWOutputPanel.MessageType.Editor);
		}
	}

	public void LogEditorRedo(string actionName)
	{
		if (!string.IsNullOrWhiteSpace(actionName))
		{
			AddOutputMessage("重做: " + actionName, XWOutputPanel.MessageType.Editor);
		}
	}

	public Array<Node> GetSelectedNodes()
	{
		List<Node> list = _editorSelection?.GetSelectedNodes();
		if (list == null)
		{
			return new Array<Node>();
		}
		Array<Node> array = new Array<Node>();
		foreach (Node item in list)
		{
			array.Add(item);
		}
		return array;
	}

	public void SelectNode(Node node)
	{
		_editorSelection?.SelectNode(node);
	}

	public void SetSelectedNode(Node node)
	{
		_editorSelection?.Clear();
		_editorSelection?.SelectNode(node);
	}

	public void ClearSelection()
	{
		_editorSelection?.Clear();
	}

	public void AddInspectorPlugin(XWInspectorPlugin plugin)
	{
		if (_inspector is XWInspector xWInspector)
		{
			xWInspector.AddPlugin(plugin);
		}
	}

	public void RemoveInspectorPlugin(XWInspectorPlugin plugin)
	{
		if (_inspector is XWInspector xWInspector)
		{
			xWInspector.RemovePlugin(plugin);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(56)
		{
			new MethodInfo(MethodName.Initialize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "undoRedo", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "settings", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "selection", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "history", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetLayoutManager, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "layoutManager", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetInspector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inspector", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetFileSystemPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fileSystemPanel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetBlueprintEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "blueprintEditor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetScriptEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scriptEditor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetSceneTreeDock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sceneTreeDock", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.Set2DSceneEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene2DEditor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetAnimationEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "animationEditor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetResourceEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resourceEditor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetResourceEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dockKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resourceEditor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetIssuePanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "issuePanel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetOutputPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "outputPanel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetFileSystem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fileSystem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetEditorPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "panel", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetEditedSceneRoot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetUndoRedoManager, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEditorData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEditorSettings, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEditorSelection, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectionHistory, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetLayoutManager, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetInspector, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFileSystemPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetBlueprintEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetScriptEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSceneTreeDock, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Get2DSceneEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrentSceneHistoryId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetAnimationEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetResourceEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetResourceEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dockKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryGetLoadedResourceEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dockKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureResourceEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dockKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetIssuePanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetOutputPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasAnimationEditor, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetFileSystem, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEditorPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEditedSceneRoot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSceneRoot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InspectObject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.EditResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "res", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.EditNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FocusPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowToast, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "severity", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddOutputMessage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LogEditorAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LogEditorUndo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LogEditorRedo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedNodes, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetSelectedNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddInspectorPlugin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plugin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveInspectorPlugin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plugin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Initialize && args.Count == 5)
		{
			Initialize(VariantUtils.ConvertTo<XWUndoRedoManager>(in args[0]), VariantUtils.ConvertTo<XWEditorData>(in args[1]), VariantUtils.ConvertTo<XWEditorSettings>(in args[2]), VariantUtils.ConvertTo<XWEditorSelection>(in args[3]), VariantUtils.ConvertTo<XWEditorSelectionHistory>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetLayoutManager && args.Count == 1)
		{
			SetLayoutManager(VariantUtils.ConvertTo<XWLayoutManager>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetInspector && args.Count == 1)
		{
			SetInspector(VariantUtils.ConvertTo<GodotObject>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetFileSystemPanel && args.Count == 1)
		{
			SetFileSystemPanel(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetBlueprintEditor && args.Count == 1)
		{
			SetBlueprintEditor(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetScriptEditor && args.Count == 1)
		{
			SetScriptEditor(VariantUtils.ConvertTo<XWScriptEditor>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSceneTreeDock && args.Count == 1)
		{
			SetSceneTreeDock(VariantUtils.ConvertTo<XWSceneTreeDock>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Set2DSceneEditor && args.Count == 1)
		{
			Set2DSceneEditor(VariantUtils.ConvertTo<XW2DSceneEditor>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAnimationEditor && args.Count == 1)
		{
			SetAnimationEditor(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetResourceEditor && args.Count == 1)
		{
			SetResourceEditor(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetResourceEditor && args.Count == 2)
		{
			SetResourceEditor(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetIssuePanel && args.Count == 1)
		{
			SetIssuePanel(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetOutputPanel && args.Count == 1)
		{
			SetOutputPanel(VariantUtils.ConvertTo<XWOutputPanel>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetFileSystem && args.Count == 1)
		{
			SetFileSystem(VariantUtils.ConvertTo<RefCounted>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetEditorPanel && args.Count == 1)
		{
			SetEditorPanel(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetEditedSceneRoot && args.Count == 1)
		{
			SetEditedSceneRoot(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetUndoRedoManager && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWUndoRedoManager>(GetUndoRedoManager());
			return true;
		}
		if (method == MethodName.GetEditorData && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWEditorData>(GetEditorData());
			return true;
		}
		if (method == MethodName.GetEditorSettings && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWEditorSettings>(GetEditorSettings());
			return true;
		}
		if (method == MethodName.GetEditorSelection && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWEditorSelection>(GetEditorSelection());
			return true;
		}
		if (method == MethodName.GetSelectionHistory && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWEditorSelectionHistory>(GetSelectionHistory());
			return true;
		}
		if (method == MethodName.GetLayoutManager && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWLayoutManager>(GetLayoutManager());
			return true;
		}
		if (method == MethodName.GetInspector && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<GodotObject>(GetInspector());
			return true;
		}
		if (method == MethodName.GetFileSystemPanel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(GetFileSystemPanel());
			return true;
		}
		if (method == MethodName.GetBlueprintEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(GetBlueprintEditor());
			return true;
		}
		if (method == MethodName.GetScriptEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWScriptEditor>(GetScriptEditor());
			return true;
		}
		if (method == MethodName.GetSceneTreeDock && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWSceneTreeDock>(GetSceneTreeDock());
			return true;
		}
		if (method == MethodName.Get2DSceneEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XW2DSceneEditor>(Get2DSceneEditor());
			return true;
		}
		if (method == MethodName.GetCurrentSceneHistoryId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCurrentSceneHistoryId());
			return true;
		}
		if (method == MethodName.GetAnimationEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(GetAnimationEditor());
			return true;
		}
		if (method == MethodName.GetResourceEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(GetResourceEditor());
			return true;
		}
		if (method == MethodName.GetResourceEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(GetResourceEditor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.TryGetLoadedResourceEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(TryGetLoadedResourceEditor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.EnsureResourceEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(EnsureResourceEditor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetIssuePanel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(GetIssuePanel());
			return true;
		}
		if (method == MethodName.GetOutputPanel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWOutputPanel>(GetOutputPanel());
			return true;
		}
		if (method == MethodName.HasAnimationEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasAnimationEditor());
			return true;
		}
		if (method == MethodName.GetFileSystem && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<RefCounted>(GetFileSystem());
			return true;
		}
		if (method == MethodName.GetEditorPanel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(GetEditorPanel());
			return true;
		}
		if (method == MethodName.GetEditedSceneRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(GetEditedSceneRoot());
			return true;
		}
		if (method == MethodName.GetSceneRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(GetSceneRoot());
			return true;
		}
		if (method == MethodName.InspectObject && args.Count == 1)
		{
			InspectObject(VariantUtils.ConvertTo<GodotObject>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EditResource && args.Count == 1)
		{
			EditResource(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EditNode && args.Count == 1)
		{
			EditNode(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FocusPanel && args.Count == 1)
		{
			FocusPanel(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowToast && args.Count == 2)
		{
			ShowToast(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddOutputMessage && args.Count == 2)
		{
			AddOutputMessage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<XWOutputPanel.MessageType>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LogEditorAction && args.Count == 1)
		{
			LogEditorAction(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LogEditorUndo && args.Count == 1)
		{
			LogEditorUndo(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LogEditorRedo && args.Count == 1)
		{
			LogEditorRedo(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedNodes && args.Count == 0)
		{
			Array<Node> selectedNodes = GetSelectedNodes();
			ret = VariantUtils.CreateFromArray(selectedNodes);
			return true;
		}
		if (method == MethodName.SelectNode && args.Count == 1)
		{
			SelectNode(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSelectedNode && args.Count == 1)
		{
			SetSelectedNode(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearSelection && args.Count == 0)
		{
			ClearSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.AddInspectorPlugin && args.Count == 1)
		{
			AddInspectorPlugin(VariantUtils.ConvertTo<XWInspectorPlugin>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveInspectorPlugin && args.Count == 1)
		{
			RemoveInspectorPlugin(VariantUtils.ConvertTo<XWInspectorPlugin>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Initialize && args.Count == 5)
		{
			Initialize(VariantUtils.ConvertTo<XWUndoRedoManager>(in args[0]), VariantUtils.ConvertTo<XWEditorData>(in args[1]), VariantUtils.ConvertTo<XWEditorSettings>(in args[2]), VariantUtils.ConvertTo<XWEditorSelection>(in args[3]), VariantUtils.ConvertTo<XWEditorSelectionHistory>(in args[4]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Initialize)
		{
			return true;
		}
		if (method == MethodName.SetLayoutManager)
		{
			return true;
		}
		if (method == MethodName.SetInspector)
		{
			return true;
		}
		if (method == MethodName.SetFileSystemPanel)
		{
			return true;
		}
		if (method == MethodName.SetBlueprintEditor)
		{
			return true;
		}
		if (method == MethodName.SetScriptEditor)
		{
			return true;
		}
		if (method == MethodName.SetSceneTreeDock)
		{
			return true;
		}
		if (method == MethodName.Set2DSceneEditor)
		{
			return true;
		}
		if (method == MethodName.SetAnimationEditor)
		{
			return true;
		}
		if (method == MethodName.SetResourceEditor)
		{
			return true;
		}
		if (method == MethodName.SetIssuePanel)
		{
			return true;
		}
		if (method == MethodName.SetOutputPanel)
		{
			return true;
		}
		if (method == MethodName.SetFileSystem)
		{
			return true;
		}
		if (method == MethodName.SetEditorPanel)
		{
			return true;
		}
		if (method == MethodName.SetEditedSceneRoot)
		{
			return true;
		}
		if (method == MethodName.GetUndoRedoManager)
		{
			return true;
		}
		if (method == MethodName.GetEditorData)
		{
			return true;
		}
		if (method == MethodName.GetEditorSettings)
		{
			return true;
		}
		if (method == MethodName.GetEditorSelection)
		{
			return true;
		}
		if (method == MethodName.GetSelectionHistory)
		{
			return true;
		}
		if (method == MethodName.GetLayoutManager)
		{
			return true;
		}
		if (method == MethodName.GetInspector)
		{
			return true;
		}
		if (method == MethodName.GetFileSystemPanel)
		{
			return true;
		}
		if (method == MethodName.GetBlueprintEditor)
		{
			return true;
		}
		if (method == MethodName.GetScriptEditor)
		{
			return true;
		}
		if (method == MethodName.GetSceneTreeDock)
		{
			return true;
		}
		if (method == MethodName.Get2DSceneEditor)
		{
			return true;
		}
		if (method == MethodName.GetCurrentSceneHistoryId)
		{
			return true;
		}
		if (method == MethodName.GetAnimationEditor)
		{
			return true;
		}
		if (method == MethodName.GetResourceEditor)
		{
			return true;
		}
		if (method == MethodName.TryGetLoadedResourceEditor)
		{
			return true;
		}
		if (method == MethodName.EnsureResourceEditor)
		{
			return true;
		}
		if (method == MethodName.GetIssuePanel)
		{
			return true;
		}
		if (method == MethodName.GetOutputPanel)
		{
			return true;
		}
		if (method == MethodName.HasAnimationEditor)
		{
			return true;
		}
		if (method == MethodName.GetFileSystem)
		{
			return true;
		}
		if (method == MethodName.GetEditorPanel)
		{
			return true;
		}
		if (method == MethodName.GetEditedSceneRoot)
		{
			return true;
		}
		if (method == MethodName.GetSceneRoot)
		{
			return true;
		}
		if (method == MethodName.InspectObject)
		{
			return true;
		}
		if (method == MethodName.EditResource)
		{
			return true;
		}
		if (method == MethodName.EditNode)
		{
			return true;
		}
		if (method == MethodName.FocusPanel)
		{
			return true;
		}
		if (method == MethodName.ShowToast)
		{
			return true;
		}
		if (method == MethodName.AddOutputMessage)
		{
			return true;
		}
		if (method == MethodName.LogEditorAction)
		{
			return true;
		}
		if (method == MethodName.LogEditorUndo)
		{
			return true;
		}
		if (method == MethodName.LogEditorRedo)
		{
			return true;
		}
		if (method == MethodName.GetSelectedNodes)
		{
			return true;
		}
		if (method == MethodName.SelectNode)
		{
			return true;
		}
		if (method == MethodName.SetSelectedNode)
		{
			return true;
		}
		if (method == MethodName.ClearSelection)
		{
			return true;
		}
		if (method == MethodName.AddInspectorPlugin)
		{
			return true;
		}
		if (method == MethodName.RemoveInspectorPlugin)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._undoRedoManager)
		{
			_undoRedoManager = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName._editorData)
		{
			_editorData = VariantUtils.ConvertTo<XWEditorData>(in value);
			return true;
		}
		if (name == PropertyName._editorSettings)
		{
			_editorSettings = VariantUtils.ConvertTo<XWEditorSettings>(in value);
			return true;
		}
		if (name == PropertyName._editorSelection)
		{
			_editorSelection = VariantUtils.ConvertTo<XWEditorSelection>(in value);
			return true;
		}
		if (name == PropertyName._selectionHistory)
		{
			_selectionHistory = VariantUtils.ConvertTo<XWEditorSelectionHistory>(in value);
			return true;
		}
		if (name == PropertyName._layoutManager)
		{
			_layoutManager = VariantUtils.ConvertTo<XWLayoutManager>(in value);
			return true;
		}
		if (name == PropertyName._inspector)
		{
			_inspector = VariantUtils.ConvertTo<GodotObject>(in value);
			return true;
		}
		if (name == PropertyName._fileSystemPanel)
		{
			_fileSystemPanel = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._blueprintEditor)
		{
			_blueprintEditor = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._scriptEditor)
		{
			_scriptEditor = VariantUtils.ConvertTo<XWScriptEditor>(in value);
			return true;
		}
		if (name == PropertyName._sceneTreeDock)
		{
			_sceneTreeDock = VariantUtils.ConvertTo<XWSceneTreeDock>(in value);
			return true;
		}
		if (name == PropertyName._scene2DEditor)
		{
			_scene2DEditor = VariantUtils.ConvertTo<XW2DSceneEditor>(in value);
			return true;
		}
		if (name == PropertyName._animationEditor)
		{
			_animationEditor = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._resourceEditor)
		{
			_resourceEditor = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._issuePanel)
		{
			_issuePanel = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._outputPanel)
		{
			_outputPanel = VariantUtils.ConvertTo<XWOutputPanel>(in value);
			return true;
		}
		if (name == PropertyName._fileSystem)
		{
			_fileSystem = VariantUtils.ConvertTo<RefCounted>(in value);
			return true;
		}
		if (name == PropertyName._editorPanel)
		{
			_editorPanel = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._editedSceneRoot)
		{
			_editedSceneRoot = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._undoRedoManager)
		{
			value = VariantUtils.CreateFrom(in _undoRedoManager);
			return true;
		}
		if (name == PropertyName._editorData)
		{
			value = VariantUtils.CreateFrom(in _editorData);
			return true;
		}
		if (name == PropertyName._editorSettings)
		{
			value = VariantUtils.CreateFrom(in _editorSettings);
			return true;
		}
		if (name == PropertyName._editorSelection)
		{
			value = VariantUtils.CreateFrom(in _editorSelection);
			return true;
		}
		if (name == PropertyName._selectionHistory)
		{
			value = VariantUtils.CreateFrom(in _selectionHistory);
			return true;
		}
		if (name == PropertyName._layoutManager)
		{
			value = VariantUtils.CreateFrom(in _layoutManager);
			return true;
		}
		if (name == PropertyName._inspector)
		{
			value = VariantUtils.CreateFrom(in _inspector);
			return true;
		}
		if (name == PropertyName._fileSystemPanel)
		{
			value = VariantUtils.CreateFrom(in _fileSystemPanel);
			return true;
		}
		if (name == PropertyName._blueprintEditor)
		{
			value = VariantUtils.CreateFrom(in _blueprintEditor);
			return true;
		}
		if (name == PropertyName._scriptEditor)
		{
			value = VariantUtils.CreateFrom(in _scriptEditor);
			return true;
		}
		if (name == PropertyName._sceneTreeDock)
		{
			value = VariantUtils.CreateFrom(in _sceneTreeDock);
			return true;
		}
		if (name == PropertyName._scene2DEditor)
		{
			value = VariantUtils.CreateFrom(in _scene2DEditor);
			return true;
		}
		if (name == PropertyName._animationEditor)
		{
			value = VariantUtils.CreateFrom(in _animationEditor);
			return true;
		}
		if (name == PropertyName._resourceEditor)
		{
			value = VariantUtils.CreateFrom(in _resourceEditor);
			return true;
		}
		if (name == PropertyName._issuePanel)
		{
			value = VariantUtils.CreateFrom(in _issuePanel);
			return true;
		}
		if (name == PropertyName._outputPanel)
		{
			value = VariantUtils.CreateFrom(in _outputPanel);
			return true;
		}
		if (name == PropertyName._fileSystem)
		{
			value = VariantUtils.CreateFrom(in _fileSystem);
			return true;
		}
		if (name == PropertyName._editorPanel)
		{
			value = VariantUtils.CreateFrom(in _editorPanel);
			return true;
		}
		if (name == PropertyName._editedSceneRoot)
		{
			value = VariantUtils.CreateFrom(in _editedSceneRoot);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._undoRedoManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorSettings, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorSelection, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectionHistory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._layoutManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fileSystemPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._blueprintEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scriptEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sceneTreeDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scene2DEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._issuePanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._outputPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fileSystem, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editorPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editedSceneRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._undoRedoManager, Variant.From(in _undoRedoManager));
		info.AddProperty(PropertyName._editorData, Variant.From(in _editorData));
		info.AddProperty(PropertyName._editorSettings, Variant.From(in _editorSettings));
		info.AddProperty(PropertyName._editorSelection, Variant.From(in _editorSelection));
		info.AddProperty(PropertyName._selectionHistory, Variant.From(in _selectionHistory));
		info.AddProperty(PropertyName._layoutManager, Variant.From(in _layoutManager));
		info.AddProperty(PropertyName._inspector, Variant.From(in _inspector));
		info.AddProperty(PropertyName._fileSystemPanel, Variant.From(in _fileSystemPanel));
		info.AddProperty(PropertyName._blueprintEditor, Variant.From(in _blueprintEditor));
		info.AddProperty(PropertyName._scriptEditor, Variant.From(in _scriptEditor));
		info.AddProperty(PropertyName._sceneTreeDock, Variant.From(in _sceneTreeDock));
		info.AddProperty(PropertyName._scene2DEditor, Variant.From(in _scene2DEditor));
		info.AddProperty(PropertyName._animationEditor, Variant.From(in _animationEditor));
		info.AddProperty(PropertyName._resourceEditor, Variant.From(in _resourceEditor));
		info.AddProperty(PropertyName._issuePanel, Variant.From(in _issuePanel));
		info.AddProperty(PropertyName._outputPanel, Variant.From(in _outputPanel));
		info.AddProperty(PropertyName._fileSystem, Variant.From(in _fileSystem));
		info.AddProperty(PropertyName._editorPanel, Variant.From(in _editorPanel));
		info.AddProperty(PropertyName._editedSceneRoot, Variant.From(in _editedSceneRoot));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._undoRedoManager, out var value))
		{
			_undoRedoManager = value.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._editorData, out var value2))
		{
			_editorData = value2.As<XWEditorData>();
		}
		if (info.TryGetProperty(PropertyName._editorSettings, out var value3))
		{
			_editorSettings = value3.As<XWEditorSettings>();
		}
		if (info.TryGetProperty(PropertyName._editorSelection, out var value4))
		{
			_editorSelection = value4.As<XWEditorSelection>();
		}
		if (info.TryGetProperty(PropertyName._selectionHistory, out var value5))
		{
			_selectionHistory = value5.As<XWEditorSelectionHistory>();
		}
		if (info.TryGetProperty(PropertyName._layoutManager, out var value6))
		{
			_layoutManager = value6.As<XWLayoutManager>();
		}
		if (info.TryGetProperty(PropertyName._inspector, out var value7))
		{
			_inspector = value7.As<GodotObject>();
		}
		if (info.TryGetProperty(PropertyName._fileSystemPanel, out var value8))
		{
			_fileSystemPanel = value8.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._blueprintEditor, out var value9))
		{
			_blueprintEditor = value9.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._scriptEditor, out var value10))
		{
			_scriptEditor = value10.As<XWScriptEditor>();
		}
		if (info.TryGetProperty(PropertyName._sceneTreeDock, out var value11))
		{
			_sceneTreeDock = value11.As<XWSceneTreeDock>();
		}
		if (info.TryGetProperty(PropertyName._scene2DEditor, out var value12))
		{
			_scene2DEditor = value12.As<XW2DSceneEditor>();
		}
		if (info.TryGetProperty(PropertyName._animationEditor, out var value13))
		{
			_animationEditor = value13.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._resourceEditor, out var value14))
		{
			_resourceEditor = value14.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._issuePanel, out var value15))
		{
			_issuePanel = value15.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._outputPanel, out var value16))
		{
			_outputPanel = value16.As<XWOutputPanel>();
		}
		if (info.TryGetProperty(PropertyName._fileSystem, out var value17))
		{
			_fileSystem = value17.As<RefCounted>();
		}
		if (info.TryGetProperty(PropertyName._editorPanel, out var value18))
		{
			_editorPanel = value18.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._editedSceneRoot, out var value19))
		{
			_editedSceneRoot = value19.As<Node>();
		}
	}
}
