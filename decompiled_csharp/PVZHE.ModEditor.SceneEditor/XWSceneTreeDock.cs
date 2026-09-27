using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.Registry.Class;
using PVZHE.ModEditor.ScriptEditor;

namespace PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://addons/ModEditor/SceneEditor/SceneNodeTree/XWSceneTreeDock.cs")]
public class XWSceneTreeDock : VBoxContainer
{
	private enum ContextMenuOp
	{
		AddNode = 1,
		InstantiateScene,
		Rename,
		Duplicate,
		MoveUp,
		MoveDown,
		Delete,
		CopyNodePath,
		MakeRoot,
		SaveBranchAsScene,
		ExpandAll,
		CollapseAll,
		AddComponentScene
	}

	public new class MethodName : VBoxContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName SetupToolbarIcons = "SetupToolbarIcons";

		public static readonly StringName SetupFileDialogs = "SetupFileDialogs";

		public static readonly StringName SetupAddNodeMenu = "SetupAddNodeMenu";

		public static readonly StringName AddNodeMenuItem = "AddNodeMenuItem";

		public static readonly StringName SetupContextMenu = "SetupContextMenu";

		public static readonly StringName ConnectSignals = "ConnectSignals";

		public static readonly StringName SyncSceneRoot = "SyncSceneRoot";

		public static readonly StringName OnAddNodePressed = "OnAddNodePressed";

		public static readonly StringName OnAddNodeMenuItem = "OnAddNodeMenuItem";

		public static readonly StringName OpenCreateNodeDialog = "OpenCreateNodeDialog";

		public static readonly StringName AddNodeByClass = "AddNodeByClass";

		public static readonly StringName CreateNodeByClass = "CreateNodeByClass";

		public static readonly StringName BuildNodeName = "BuildNodeName";

		public static readonly StringName OnInstantiatePressed = "OnInstantiatePressed";

		public static readonly StringName OnInstantiateFileSelected = "OnInstantiateFileSelected";

		public static readonly StringName InstantiateSceneWithHistory = "InstantiateSceneWithHistory";

		public static readonly StringName ShowComponentSceneDialog = "ShowComponentSceneDialog";

		public static readonly StringName OnComponentSceneFileSelected = "OnComponentSceneFileSelected";

		public static readonly StringName AddComponentSceneWithHistory = "AddComponentSceneWithHistory";

		public static readonly StringName CanAddComponentSceneToSelectedNode = "CanAddComponentSceneToSelectedNode";

		public static readonly StringName IsComponentContainerNode = "IsComponentContainerNode";

		public static readonly StringName FindPreferredComponentSceneDirectory = "FindPreferredComponentSceneDirectory";

		public static readonly StringName OnFilterTextChanged = "OnFilterTextChanged";

		public static readonly StringName UpdateContextMenuState = "UpdateContextMenuState";

		public static readonly StringName SetContextMenuDisabled = "SetContextMenuDisabled";

		public static readonly StringName OnTreeMouseSelected = "OnTreeMouseSelected";

		public static readonly StringName OnContextMenuItem = "OnContextMenuItem";

		public static readonly StringName ShowRenameDialog = "ShowRenameDialog";

		public static readonly StringName OnRenameConfirmed = "OnRenameConfirmed";

		public static readonly StringName CopySelectedNodePath = "CopySelectedNodePath";

		public static readonly StringName BuildNodePath = "BuildNodePath";

		public static readonly StringName ShowSaveBranchDialog = "ShowSaveBranchDialog";

		public static readonly StringName OnSaveBranchFileSelected = "OnSaveBranchFileSelected";

		public static readonly StringName OnNodeSelected = "OnNodeSelected";

		public static readonly StringName OnHistorySelectionChanged = "OnHistorySelectionChanged";

		public static readonly StringName UpdateScriptButtons = "UpdateScriptButtons";

		public static readonly StringName UpdateSceneActionButtons = "UpdateSceneActionButtons";

		public static readonly StringName OnCreateScriptPressed = "OnCreateScriptPressed";

		public static readonly StringName OnDetachScriptPressed = "OnDetachScriptPressed";

		public static readonly StringName DetachSelectedScriptWithHistory = "DetachSelectedScriptWithHistory";

		public static readonly StringName OnExtendScriptPressed = "OnExtendScriptPressed";

		public static readonly StringName OnScriptFileSelected = "OnScriptFileSelected";

		public static readonly StringName CreateAndAttachScriptWithHistory = "CreateAndAttachScriptWithHistory";

		public static readonly StringName SanitizeClassName = "SanitizeClassName";

		public static readonly StringName OnTreeMenuPressed = "OnTreeMenuPressed";

		public static readonly StringName GetSelectedNode = "GetSelectedNode";

		public static readonly StringName SelectedNodeHasScript = "SelectedNodeHasScript";

		public static readonly StringName IsSelectedNodeRoot = "IsSelectedNodeRoot";

		public static readonly StringName ShowInstantiateDialog = "ShowInstantiateDialog";

		public static readonly StringName ShowCreateScriptDialog = "ShowCreateScriptDialog";

		public static readonly StringName DetachSelectedScript = "DetachSelectedScript";

		public static readonly StringName ExtendSelectedScript = "ExtendSelectedScript";

		public static readonly StringName ShowRenameSelectedDialog = "ShowRenameSelectedDialog";

		public static readonly StringName DuplicateSelectedNode = "DuplicateSelectedNode";

		public static readonly StringName MoveSelectedNode = "MoveSelectedNode";

		public static readonly StringName ReparentNodeWithHistory = "ReparentNodeWithHistory";

		public static readonly StringName GetTrackedReparentNodeRecordCount = "GetTrackedReparentNodeRecordCount";

		public static readonly StringName GetLastReparentHistoryId = "GetLastReparentHistoryId";

		public static readonly StringName DidLastReparentPreserveGlobalTransform = "DidLastReparentPreserveGlobalTransform";

		public static readonly StringName GetLastReparentFeedback = "GetLastReparentFeedback";

		public static readonly StringName RemoveSelectedNode = "RemoveSelectedNode";

		public static readonly StringName AddNodeToParentWithHistory = "AddNodeToParentWithHistory";

		public static readonly StringName RemoveNodeWithHistory = "RemoveNodeWithHistory";

		public static readonly StringName MakeSelectedNodeRoot = "MakeSelectedNodeRoot";

		public static readonly StringName ShowSaveBranchDialogForSelected = "ShowSaveBranchDialogForSelected";

		public static readonly StringName HasScript = "HasScript";

		public static readonly StringName PopupAtMouse = "PopupAtMouse";

		public static readonly StringName PopupBelow = "PopupBelow";

		public static readonly StringName SetScene = "SetScene";

		public static readonly StringName ApplySceneRootFromHistory = "ApplySceneRootFromHistory";

		public static readonly StringName RefreshScene = "RefreshScene";

		public static readonly StringName SelectNode = "SelectNode";

		public static readonly StringName PrepareForSceneHistoryBranchChange = "PrepareForSceneHistoryBranchChange";

		public static readonly StringName ReleaseHistoryResources = "ReleaseHistoryResources";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName _addNodeBtn = "_addNodeBtn";

		public static readonly StringName _instantiateBtn = "_instantiateBtn";

		public static readonly StringName _filterLineEdit = "_filterLineEdit";

		public static readonly StringName _createScriptBtn = "_createScriptBtn";

		public static readonly StringName _detachScriptBtn = "_detachScriptBtn";

		public static readonly StringName _extendScriptBtn = "_extendScriptBtn";

		public static readonly StringName _treeMenuBtn = "_treeMenuBtn";

		public static readonly StringName _sceneTree = "_sceneTree";

		public static readonly StringName _addNodeMenu = "_addNodeMenu";

		public static readonly StringName _contextMenu = "_contextMenu";

		public static readonly StringName _renameDialog = "_renameDialog";

		public static readonly StringName _renameLineEdit = "_renameLineEdit";

		public static readonly StringName _instantiateDialog = "_instantiateDialog";

		public static readonly StringName _componentSceneDialog = "_componentSceneDialog";

		public static readonly StringName _scriptDialog = "_scriptDialog";

		public static readonly StringName _saveBranchDialog = "_saveBranchDialog";

		public static readonly StringName _nextAddNodeId = "_nextAddNodeId";

		public static readonly StringName _pendingSceneRoot = "_pendingSceneRoot";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
	}

	private const long MoreNodeId = 9999L;

	private const string ModComponentDirectory = "Battle/Components";

	private Button _addNodeBtn;

	private Button _instantiateBtn;

	private LineEdit _filterLineEdit;

	private Button _createScriptBtn;

	private Button _detachScriptBtn;

	private Button _extendScriptBtn;

	private Button _treeMenuBtn;

	private XWSceneNodeTree _sceneTree;

	private PopupMenu _addNodeMenu;

	private PopupMenu _contextMenu;

	private ConfirmationDialog _renameDialog;

	private LineEdit _renameLineEdit;

	private FileDialog _instantiateDialog;

	private FileDialog _componentSceneDialog;

	private FileDialog _scriptDialog;

	private FileDialog _saveBranchDialog;

	private readonly Dictionary<long, string> _addNodeClasses = new Dictionary<long, string>();

	private long _nextAddNodeId = 1L;

	private Node _pendingSceneRoot;

	private Vector2? _pendingAddNodeCanvasPosition;

	public override void _Ready()
	{
		_addNodeBtn = GetNode<Button>("%AddNode");
		_instantiateBtn = GetNode<Button>("%Instantiate");
		_filterLineEdit = GetNode<LineEdit>("%FilterLineEdit");
		_createScriptBtn = GetNode<Button>("%CreateScript");
		_detachScriptBtn = GetNode<Button>("%DetachScript");
		_extendScriptBtn = GetNode<Button>("%ExtendScript");
		_treeMenuBtn = GetNode<Button>("%TreeMenu");
		_sceneTree = GetNode<XWSceneNodeTree>("%SceneTree");
		_addNodeMenu = GetNode<PopupMenu>("%AddNodeMenu");
		_contextMenu = GetNode<PopupMenu>("%ContextMenu");
		_renameDialog = GetNode<ConfirmationDialog>("%RenameDialog");
		_renameLineEdit = GetNode<LineEdit>("%RenameLineEdit");
		_instantiateDialog = GetNode<FileDialog>("%InstantiateDialog");
		_componentSceneDialog = GetNode<FileDialog>("%ComponentSceneDialog");
		_scriptDialog = GetNode<FileDialog>("%ScriptDialog");
		_saveBranchDialog = GetNode<FileDialog>("%SaveBranchDialog");
		SetupToolbarIcons();
		SetupFileDialogs();
		SetupAddNodeMenu();
		SetupContextMenu();
		ConnectSignals();
		SyncSceneRoot();
		UpdateScriptButtons(_sceneTree.GetSelectedNode());
	}

	private void SetupToolbarIcons()
	{
		XWClassRegistry instance = XWClassRegistry.Instance;
		_addNodeBtn.Icon = instance.GetUIIcon("Add");
		_instantiateBtn.Icon = instance.GetUIIcon("Instance");
		_createScriptBtn.Icon = instance.GetUIIcon("ScriptCreate");
		_detachScriptBtn.Icon = instance.GetUIIcon("ScriptRemove");
		_extendScriptBtn.Icon = instance.GetUIIcon("ScriptExtend");
		_treeMenuBtn.Icon = instance.GetUIIcon("GuiTabMenuHl");
		Texture2D uIIcon = instance.GetUIIcon("Search");
		if (GodotObject.IsInstanceValid(uIIcon))
		{
			_filterLineEdit.RightIcon = uIIcon;
		}
	}

	private void SetupFileDialogs()
	{
		_instantiateDialog.Title = "实例化场景";
		_instantiateDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		_instantiateDialog.Access = FileDialog.AccessEnum.Resources;
		_instantiateDialog.ClearFilters();
		_instantiateDialog.AddFilter("*.tscn", "场景文件");
		_componentSceneDialog.Title = "选择 Component 场景";
		_componentSceneDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		_componentSceneDialog.Access = FileDialog.AccessEnum.Filesystem;
		_componentSceneDialog.ClearFilters();
		_componentSceneDialog.AddFilter("*.tscn", "Component 场景");
		_scriptDialog.Title = "创建脚本";
		_scriptDialog.FileMode = FileDialog.FileModeEnum.SaveFile;
		_scriptDialog.Access = FileDialog.AccessEnum.Resources;
		_scriptDialog.ClearFilters();
		_scriptDialog.AddFilter("*.cs", "C# 脚本");
		_saveBranchDialog.Title = "保存分支为场景";
		_saveBranchDialog.FileMode = FileDialog.FileModeEnum.SaveFile;
		_saveBranchDialog.Access = FileDialog.AccessEnum.Resources;
		_saveBranchDialog.ClearFilters();
		_saveBranchDialog.AddFilter("*.tscn", "场景文件");
	}

	private void SetupAddNodeMenu()
	{
		_addNodeMenu.Clear();
		_addNodeClasses.Clear();
		_nextAddNodeId = 1L;
		AddNodeMenuItem("Node");
		AddNodeMenuItem("Node2D");
		AddNodeMenuItem("CanvasLayer");
		_addNodeMenu.AddSeparator();
		AddNodeMenuItem("Sprite2D");
		AddNodeMenuItem("AnimatedSprite2D");
		AddNodeMenuItem("Camera2D");
		AddNodeMenuItem("Marker2D");
		AddNodeMenuItem("Path2D");
		AddNodeMenuItem("Line2D");
		AddNodeMenuItem("Polygon2D");
		_addNodeMenu.AddSeparator();
		AddNodeMenuItem("Area2D");
		AddNodeMenuItem("StaticBody2D");
		AddNodeMenuItem("CharacterBody2D");
		AddNodeMenuItem("CollisionShape2D");
		AddNodeMenuItem("CollisionPolygon2D");
		AddNodeMenuItem("RayCast2D");
		_addNodeMenu.AddSeparator();
		AddNodeMenuItem("Control");
		AddNodeMenuItem("Label");
		AddNodeMenuItem("Button");
		AddNodeMenuItem("TextureRect");
		AddNodeMenuItem("ColorRect");
		_addNodeMenu.AddSeparator();
		AddNodeMenuItem("Timer");
		AddNodeMenuItem("AudioStreamPlayer2D");
		AddNodeMenuItem("AnimationPlayer", "动画播放器");
		AddNodeMenuItem("AnimationTree", "动画树");
		AddNodeMenuItem("PointLight2D");
		AddNodeMenuItem("DirectionalLight2D");
		_addNodeMenu.AddSeparator();
		_addNodeMenu.AddIconItem(XWClassRegistry.Instance.GetUIIcon("Add"), "更多...", 9999, Key.None);
	}

	private void AddNodeMenuItem(string className, string displayName = null)
	{
		long num = _nextAddNodeId++;
		_addNodeClasses[num] = className;
		_addNodeMenu.AddIconItem(XWClassRegistry.Instance.GetClassIcon(className), string.IsNullOrWhiteSpace(displayName) ? className : displayName, (int)num, Key.None);
	}

	private void SetupContextMenu()
	{
		_contextMenu.Clear();
		XWClassRegistry instance = XWClassRegistry.Instance;
		_contextMenu.AddIconItem(instance.GetUIIcon("Add"), "添加子节点", 1, Key.None);
		_contextMenu.AddIconItem(instance.GetUIIcon("Instance"), "实例化场景...", 2, Key.None);
		Texture2D classIcon = instance.GetClassIcon("ComponentBase");
		_contextMenu.AddIconItem(GodotObject.IsInstanceValid(classIcon) ? classIcon : instance.GetUIIcon("Instance"), "添加 Component 节点...", 13, Key.None);
		_contextMenu.AddSeparator();
		_contextMenu.AddIconItem(instance.GetUIIcon("Rename"), "重命名", 3, Key.None);
		_contextMenu.AddIconItem(instance.GetUIIcon("Duplicate"), "复制", 4, Key.None);
		_contextMenu.AddSeparator();
		_contextMenu.AddIconItem(instance.GetUIIcon("MoveUp"), "上移", 5, Key.None);
		_contextMenu.AddIconItem(instance.GetUIIcon("MoveDown"), "下移", 6, Key.None);
		_contextMenu.AddSeparator();
		_contextMenu.AddIconItem(instance.GetUIIcon("Remove"), "删除", 7, Key.None);
		_contextMenu.AddSeparator();
		_contextMenu.AddItem("复制节点路径", 8, Key.None);
		_contextMenu.AddItem("设为场景根节点", 9, Key.None);
		_contextMenu.AddIconItem(instance.GetUIIcon("PackedScene"), "保存分支为场景...", 10, Key.None);
		_contextMenu.AddSeparator();
		_contextMenu.AddIconItem(instance.GetUIIcon("ExpandTree"), "展开全部", 11, Key.None);
		_contextMenu.AddIconItem(instance.GetUIIcon("CollapseTree"), "折叠全部", 12, Key.None);
	}

	private void ConnectSignals()
	{
		_addNodeBtn.Pressed += OnAddNodePressed;
		_instantiateBtn.Pressed += OnInstantiatePressed;
		_filterLineEdit.TextChanged += OnFilterTextChanged;
		_createScriptBtn.Pressed += OnCreateScriptPressed;
		_detachScriptBtn.Pressed += OnDetachScriptPressed;
		_extendScriptBtn.Pressed += OnExtendScriptPressed;
		_treeMenuBtn.Pressed += OnTreeMenuPressed;
		_addNodeMenu.IdPressed += OnAddNodeMenuItem;
		_contextMenu.IdPressed += OnContextMenuItem;
		_renameDialog.Confirmed += OnRenameConfirmed;
		_renameDialog.RegisterTextEnter(_renameLineEdit);
		_instantiateDialog.FileSelected += OnInstantiateFileSelected;
		_componentSceneDialog.FileSelected += OnComponentSceneFileSelected;
		_scriptDialog.FileSelected += OnScriptFileSelected;
		_saveBranchDialog.FileSelected += OnSaveBranchFileSelected;
		_sceneTree.ItemMouseSelected += OnTreeMouseSelected;
		_sceneTree.NodeSelected += OnNodeSelected;
		_sceneTree.HistorySelectionChanged += OnHistorySelectionChanged;
	}

	private void SyncSceneRoot()
	{
		Node node = _pendingSceneRoot;
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			node = XWEditorInterface.Instance?.GetEditedSceneRoot();
		}
		if (node != null && GodotObject.IsInstanceValid(node))
		{
			XWEditorInterface.Instance?.SetEditedSceneRoot(node);
		}
		else
		{
			node = null;
			XWEditorInterface.Instance?.SetEditedSceneRoot(null);
		}
		_sceneTree.SetSceneRoot(node);
		UpdateSceneActionButtons(node);
	}

	private void OnAddNodePressed()
	{
		_pendingAddNodeCanvasPosition = null;
		PopupBelow(_addNodeBtn, _addNodeMenu);
	}

	private void OnAddNodeMenuItem(long id)
	{
		string value;
		if (id == 9999)
		{
			OpenCreateNodeDialog();
		}
		else if (_addNodeClasses.TryGetValue(id, out value))
		{
			AddNodeByClass(value);
		}
	}

	private void OpenCreateNodeDialog()
	{
		XWWindowNodeSelector xWWindowNodeSelector = XWWindowNodeSelector.Create();
		if (!GodotObject.IsInstanceValid(xWWindowNodeSelector))
		{
			XWEditorInterface.Instance?.ShowToast("节点选择器未就绪");
			return;
		}
		xWWindowNodeSelector.NodeTypeSelected += AddNodeByClass;
		AddChild(xWWindowNodeSelector, forceReadableName: false, InternalMode.Disabled);
		xWWindowNodeSelector.PopupCenteredClamped(new Vector2I(900, 700), 0.9f);
	}

	private void AddNodeByClass(string className)
	{
		AddNodeByClassWithHistory(className, _pendingAddNodeCanvasPosition);
	}

	public Node AddNodeByClassWithHistory(string className, Vector2? canvasPosition = null)
	{
		_pendingAddNodeCanvasPosition = null;
		Node node = _sceneTree?.GetEditedSceneRoot();
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			XWEditorInterface.Instance?.ShowToast("请先打开或新建 2D 场景");
			return null;
		}
		Node node2 = CreateNodeByClass(className);
		if (node2 == null)
		{
			XWEditorInterface.Instance?.ShowToast("无法创建节点类型: " + className);
			return null;
		}
		node2.Name = BuildNodeName(className, node2);
		Node node3 = _sceneTree?.AddChildNodeWithHistory(node2);
		if (node3 != null)
		{
			ApplyInitialCanvasPosition(node3, canvasPosition);
			XWEditorInterface.Instance?.ShowToast($"已添加节点: {node3.Name}");
			XWEditorInterface.Instance?.Get2DSceneEditor()?.QueueViewportRedraw();
		}
		else if (GodotObject.IsInstanceValid(node2) && node2.GetParent() == null)
		{
			node2.Free();
		}
		return node3;
	}

	private static void ApplyInitialCanvasPosition(Node node, Vector2? canvasPosition)
	{
		if (canvasPosition.HasValue && node != null && GodotObject.IsInstanceValid(node))
		{
			if (node is Node2D node2D)
			{
				node2D.GlobalPosition = canvasPosition.Value;
			}
			else if (node is Control control)
			{
				control.GlobalPosition = canvasPosition.Value;
			}
		}
	}

	private static Node CreateNodeByClass(string className)
	{
		if (string.IsNullOrWhiteSpace(className))
		{
			return null;
		}
		try
		{
			XWClassRegistry instance = XWClassRegistry.Instance;
			if (instance.HasClass(className) && instance.CanInstantiate(className) && instance.Instantiate(className).AsGodotObject() is Node result)
			{
				return result;
			}
			if (ClassDB.CanInstantiate(className) && ClassDB.Instantiate(className).AsGodotObject() is Node result2)
			{
				return result2;
			}
		}
		catch (Exception ex)
		{
			GD.PushWarning("创建节点失败: " + className + "\n" + ex.Message);
		}
		return null;
	}

	private static string BuildNodeName(string className, Node node)
	{
		string text = (string.IsNullOrWhiteSpace(className) ? node.GetClass() : className);
		int num = text.LastIndexOf('.');
		if (num >= 0 && num < text.Length - 1)
		{
			string text2 = text;
			int num2 = num + 1;
			text = text2.Substring(num2, text2.Length - num2);
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return node.GetClass();
	}

	private void OnInstantiatePressed()
	{
		Node node = _sceneTree?.GetEditedSceneRoot();
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			XWEditorInterface.Instance?.ShowToast("请先打开或新建 2D 场景");
		}
		else
		{
			_instantiateDialog.PopupCenteredClamped(new Vector2I(700, 500), 0.9f);
		}
	}

	private void OnInstantiateFileSelected(string path)
	{
		InstantiateSceneWithHistory(path);
	}

	public Node InstantiateSceneWithHistory(string path)
	{
		if (!ResourceLoader.Exists(path))
		{
			XWEditorInterface.Instance?.ShowToast("场景不存在: " + path);
			return null;
		}
		Node node = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Reuse)?.Instantiate(PackedScene.GenEditState.Disabled);
		if (node != null)
		{
			Node node2 = node;
			node2.Name = path.GetFile().GetBaseName();
			Node node3 = _sceneTree?.AddChildNodeWithHistory(node2, "实例化 2D 场景");
			if (node3 != null)
			{
				XWEditorInterface instance = XWEditorInterface.Instance;
				if (instance != null)
				{
					instance.ShowToast("已实例化: " + path.GetFile());
					return node3;
				}
				return node3;
			}
			if (GodotObject.IsInstanceValid(node2) && node2.GetParent() == null)
			{
				node2.Free();
			}
			return node3;
		}
		XWEditorInterface.Instance?.ShowToast("无法实例化该场景");
		return null;
	}

	private void ShowComponentSceneDialog()
	{
		if (!CanAddComponentSceneToSelectedNode())
		{
			XWEditorInterface.Instance?.ShowToast("请选择 ComponentManager 节点后再添加组件");
			return;
		}
		string text = FindPreferredComponentSceneDirectory();
		_componentSceneDialog.Access = (FileDialog.AccessEnum)(text.StartsWith("res://", StringComparison.OrdinalIgnoreCase) ? 0 : 2);
		_componentSceneDialog.CurrentDir = text;
		_componentSceneDialog.PopupCenteredClamped(new Vector2I(700, 500), 0.9f);
	}

	private void OnComponentSceneFileSelected(string path)
	{
		AddComponentSceneWithHistory(path);
	}

	public Node AddComponentSceneWithHistory(string path)
	{
		Node node = _sceneTree?.GetSelectedNode();
		if (!IsComponentContainerNode(node))
		{
			XWEditorInterface.Instance?.ShowToast("当前节点不能添加 Component 场景");
			return null;
		}
		if (!ResourceLoader.Exists(path) && !Godot.FileAccess.FileExists(path))
		{
			XWEditorInterface.Instance?.ShowToast("Component 场景不存在: " + path);
			return null;
		}
		Node node2 = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Reuse)?.Instantiate(PackedScene.GenEditState.Disabled);
		if (node2 != null)
		{
			Node node3 = node2;
			if (!(node3 is ComponentBase))
			{
				node3.QueueFree();
				XWEditorInterface.Instance?.ShowToast("选择的场景根节点不是 ComponentBase");
				return null;
			}
			if (string.IsNullOrWhiteSpace(node3.Name))
			{
				node3.Name = path.GetFile().GetBaseName();
			}
			Node node4 = _sceneTree?.AddChildNodeToWithHistory(node, node3, "添加 2D Component");
			if (node4 != null)
			{
				XWEditorInterface.Instance?.ShowToast($"已添加 Component 节点: {node4.Name}");
				XWEditorInterface.Instance?.Get2DSceneEditor()?.QueueViewportRedraw();
			}
			else if (GodotObject.IsInstanceValid(node3) && node3.GetParent() == null)
			{
				node3.Free();
			}
			return node4;
		}
		XWEditorInterface.Instance?.ShowToast("无法实例化 Component 场景");
		return null;
	}

	public bool CanAddComponentSceneToSelectedNode()
	{
		return IsComponentContainerNode(_sceneTree?.GetSelectedNode());
	}

	private static bool IsComponentContainerNode(Node node)
	{
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			return false;
		}
		if (node is TowerDefenseCharacter || node is LegacyComponentManagerNode)
		{
			return true;
		}
		return node.Name.ToString().Equals("ComponentManager", StringComparison.OrdinalIgnoreCase);
	}

	private static string FindPreferredComponentSceneDirectory()
	{
		string text = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
		if (!string.IsNullOrWhiteSpace(text))
		{
			string text2 = Path.Combine(text, "Battle/Components".Replace('/', Path.DirectorySeparatorChar));
			if (Directory.Exists(text2))
			{
				return text2.Replace('\\', '/');
			}
		}
		return "res://Script/Component";
	}

	private void OnFilterTextChanged(string filter)
	{
		_sceneTree.ApplyFilter(filter);
	}

	private void UpdateContextMenuState()
	{
		SetContextMenuDisabled(13, !CanAddComponentSceneToSelectedNode());
	}

	private void SetContextMenuDisabled(int itemId, bool disabled)
	{
		int itemIndex = _contextMenu.GetItemIndex(itemId);
		if (itemIndex >= 0)
		{
			_contextMenu.SetItemDisabled(itemIndex, disabled);
		}
	}

	private void OnTreeMouseSelected(Vector2 position, long mouseButtonIndex)
	{
		if (mouseButtonIndex == 2)
		{
			UpdateContextMenuState();
			PopupAtMouse(_contextMenu);
		}
	}

	private void OnContextMenuItem(long id)
	{
		switch ((ContextMenuOp)id)
		{
		case ContextMenuOp.AddNode:
			PopupAddNodeMenuAtMouse();
			break;
		case ContextMenuOp.InstantiateScene:
			ShowInstantiateDialog();
			break;
		case ContextMenuOp.AddComponentScene:
			ShowComponentSceneDialog();
			break;
		case ContextMenuOp.Rename:
			ShowRenameSelectedDialog();
			break;
		case ContextMenuOp.Duplicate:
			DuplicateSelectedNode();
			break;
		case ContextMenuOp.MoveUp:
			MoveSelectedNode(-1);
			break;
		case ContextMenuOp.MoveDown:
			MoveSelectedNode(1);
			break;
		case ContextMenuOp.Delete:
			RemoveSelectedNode();
			break;
		case ContextMenuOp.CopyNodePath:
			CopySelectedNodePath();
			break;
		case ContextMenuOp.MakeRoot:
			if (MakeSelectedNodeRoot())
			{
				XWEditorInterface.Instance?.ShowToast("已设置场景根节点");
			}
			break;
		case ContextMenuOp.SaveBranchAsScene:
			ShowSaveBranchDialogForSelected();
			break;
		case ContextMenuOp.ExpandAll:
			_sceneTree.GetRoot()?.SetCollapsedRecursive(enable: false);
			break;
		case ContextMenuOp.CollapseAll:
			_sceneTree.GetRoot()?.SetCollapsedRecursive(enable: true);
			break;
		}
	}

	private void ShowRenameDialog()
	{
		Node selectedNode = _sceneTree.GetSelectedNode();
		if (selectedNode != null)
		{
			_renameLineEdit.Text = selectedNode.Name;
			_renameDialog.PopupCentered(new Vector2I(360, 110));
			_renameLineEdit.GrabFocus();
			_renameLineEdit.SelectAll();
		}
	}

	private void OnRenameConfirmed()
	{
		if (!_sceneTree.RenameSelectedNode(_renameLineEdit.Text))
		{
			XWEditorInterface.Instance?.ShowToast("无法重命名该节点");
		}
	}

	public void CopySelectedNodePath()
	{
		Node selectedNode = _sceneTree.GetSelectedNode();
		if (selectedNode != null)
		{
			string text = BuildNodePath(selectedNode);
			DisplayServer.ClipboardSet(text);
			XWEditorInterface.Instance?.ShowToast("已复制节点路径: " + text);
		}
	}

	private static string BuildNodePath(Node node)
	{
		List<string> list = new List<string>();
		for (Node node2 = node; node2 != null; node2 = node2.GetParent())
		{
			list.Add(node2.Name);
		}
		list.Reverse();
		return string.Join("/", list);
	}

	private void ShowSaveBranchDialog()
	{
		Node selectedNode = _sceneTree.GetSelectedNode();
		if (selectedNode != null)
		{
			_saveBranchDialog.CurrentFile = $"{selectedNode.Name}.tscn";
			_saveBranchDialog.PopupCenteredClamped(new Vector2I(700, 500), 0.9f);
		}
	}

	private void OnSaveBranchFileSelected(string path)
	{
		Node selectedNode = _sceneTree.GetSelectedNode();
		if (selectedNode != null)
		{
			Error error = PackBranchPreservingOwners(selectedNode, out var packedScene);
			if (error == Error.Ok)
			{
				error = ResourceSaver.Save(packedScene, path, ResourceSaver.SaverFlags.None);
			}
			XWEditorInterface.Instance?.ShowToast((error == Error.Ok) ? ("已保存场景: " + path) : $"保存场景失败: {error}");
		}
	}

	public static Error PackBranchPreservingOwners(Node branchRoot, out PackedScene packedScene)
	{
		packedScene = new PackedScene();
		if (branchRoot == null || !GodotObject.IsInstanceValid(branchRoot))
		{
			return Error.InvalidParameter;
		}
		HashSet<Node> hashSet = new HashSet<Node>();
		Dictionary<Node, Node> dictionary = new Dictionary<Node, Node>();
		CollectBranchNodes(branchRoot, hashSet, dictionary);
		try
		{
			branchRoot.Owner = null;
			foreach (Node item in hashSet)
			{
				if (item != branchRoot)
				{
					Node node = dictionary[item];
					if (node == null || !hashSet.Contains(node))
					{
						item.Owner = branchRoot;
					}
				}
			}
			return packedScene.Pack(branchRoot);
		}
		finally
		{
			foreach (var (node4, node5) in dictionary)
			{
				if (GodotObject.IsInstanceValid(node4))
				{
					node4.Owner = (GodotObject.IsInstanceValid(node5) ? node5 : null);
				}
			}
		}
	}

	private static void CollectBranchNodes(Node node, HashSet<Node> subtree, Dictionary<Node, Node> ownerSnapshot)
	{
		subtree.Add(node);
		ownerSnapshot[node] = node.Owner;
		foreach (Node child in node.GetChildren())
		{
			CollectBranchNodes(child, subtree, ownerSnapshot);
		}
	}

	private void OnNodeSelected(Node node)
	{
		UpdateScriptButtons(node);
		XWEditorInterface.Instance?.SetSelectedNode(node);
		XWEditorInterface.Instance?.Get2DSceneEditor()?.ShowDirectNodeProperties(node, focusTab: false);
	}

	private void OnHistorySelectionChanged(Node node)
	{
		UpdateScriptButtons(node);
	}

	private void UpdateScriptButtons(Node node)
	{
		bool flag = node != null && GodotObject.IsInstanceValid(node);
		bool flag2 = flag && HasScript(node);
		_createScriptBtn.Visible = flag && !flag2;
		_detachScriptBtn.Visible = flag2;
		_extendScriptBtn.Visible = flag2;
		_createScriptBtn.Disabled = !flag;
		_detachScriptBtn.Disabled = !flag2;
		_extendScriptBtn.Disabled = !flag2;
	}

	private void UpdateSceneActionButtons(Node sceneRoot)
	{
		bool flag = sceneRoot != null && GodotObject.IsInstanceValid(sceneRoot);
		_addNodeBtn.Disabled = !flag;
		_instantiateBtn.Disabled = !flag;
		_treeMenuBtn.Disabled = !flag;
	}

	private void OnCreateScriptPressed()
	{
		Node selectedNode = _sceneTree.GetSelectedNode();
		if (selectedNode != null)
		{
			_scriptDialog.Title = "创建脚本";
			_scriptDialog.CurrentFile = $"{selectedNode.Name}.cs";
			_scriptDialog.PopupCenteredClamped(new Vector2I(700, 500), 0.9f);
		}
	}

	private void OnDetachScriptPressed()
	{
		DetachSelectedScriptWithHistory();
	}

	public bool DetachSelectedScriptWithHistory()
	{
		Node node = _sceneTree?.GetSelectedNode();
		if (node == null || !HasScript(node))
		{
			return false;
		}
		bool flag = _sceneTree.SetNodeScriptWithHistory(node, null, "分离 2D 节点脚本");
		if (flag)
		{
			XWEditorInterface instance = XWEditorInterface.Instance;
			if (instance == null)
			{
				return flag;
			}
			instance.ShowToast("已分离脚本");
		}
		return flag;
	}

	private void OnExtendScriptPressed()
	{
		OnCreateScriptPressed();
	}

	private void OnScriptFileSelected(string path)
	{
		CreateAndAttachScriptWithHistory(path);
	}

	public Script CreateAndAttachScriptWithHistory(string path)
	{
		Node node = _sceneTree?.GetSelectedNode();
		if (node == null)
		{
			return null;
		}
		string text = path.Replace('\\', '/');
		if (string.IsNullOrWhiteSpace(text.GetExtension()))
		{
			text += ".cs";
		}
		if (!XWScriptEditor.IsSupportedScriptPath(text))
		{
			XWEditorInterface.Instance?.ShowToast("2D 节点脚本仅支持 C#。", 2);
			return null;
		}
		string baseDir = text.GetBaseDir();
		if (!string.IsNullOrEmpty(baseDir) && !DirAccess.DirExistsAbsolute(baseDir))
		{
			Error error = DirAccess.MakeDirRecursiveAbsolute(baseDir);
			if (error != Error.Ok)
			{
				XWEditorInterface.Instance?.ShowToast($"无法创建脚本目录: {error}");
				return null;
			}
		}
		string value = SanitizeClassName(text.GetFile().GetBaseName());
		string text2 = $"using Godot;\n\npublic partial class {value} : {node.GetClass()}\n{{\n    public override void _Ready()\n    {{\n    }}\n}}\n";
		using Godot.FileAccess fileAccess = Godot.FileAccess.Open(text, Godot.FileAccess.ModeFlags.Write);
		if (fileAccess == null)
		{
			XWEditorInterface.Instance?.ShowToast($"无法创建脚本: {Godot.FileAccess.GetOpenError()}");
			return null;
		}
		fileAccess.StoreString(text2);
		fileAccess.Close();
		Script script = ResourceLoader.Load<Script>(text, null, ResourceLoader.CacheMode.Reuse);
		if (GodotObject.IsInstanceValid(script))
		{
			if (!_sceneTree.SetNodeScriptWithHistory(node, script, "挂载 2D 节点脚本"))
			{
				return null;
			}
			XWEditorInterface.Instance?.ShowToast("已创建并挂载脚本: " + text);
			return script;
		}
		XWEditorInterface.Instance?.ShowToast("脚本已创建，编译后可挂载: " + text);
		return null;
	}

	private static string SanitizeClassName(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return "NewNodeScript";
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in value)
		{
			if (char.IsLetterOrDigit(c) || c == '_')
			{
				stringBuilder.Append(c);
			}
		}
		if (stringBuilder.Length == 0)
		{
			stringBuilder.Append("NewNodeScript");
		}
		if (!char.IsLetter(stringBuilder[0]) && stringBuilder[0] != '_')
		{
			stringBuilder.Insert(0, '_');
		}
		return stringBuilder.ToString();
	}

	private void OnTreeMenuPressed()
	{
		UpdateContextMenuState();
		PopupBelow(_treeMenuBtn, _contextMenu);
	}

	public Node GetSelectedNode()
	{
		return _sceneTree?.GetSelectedNode();
	}

	public bool SelectedNodeHasScript()
	{
		Node selectedNode = GetSelectedNode();
		if (selectedNode != null)
		{
			return HasScript(selectedNode);
		}
		return false;
	}

	public bool IsSelectedNodeRoot()
	{
		Node selectedNode = GetSelectedNode();
		Node node = _sceneTree?.GetEditedSceneRoot();
		if (selectedNode != null)
		{
			return selectedNode == node;
		}
		return false;
	}

	public void PopupAddNodeMenuAtMouse(Vector2? initialCanvasPosition = null)
	{
		_pendingAddNodeCanvasPosition = initialCanvasPosition;
		PopupAtMouse(_addNodeMenu);
	}

	public void ShowInstantiateDialog()
	{
		OnInstantiatePressed();
	}

	public void ShowCreateScriptDialog()
	{
		OnCreateScriptPressed();
	}

	public void DetachSelectedScript()
	{
		OnDetachScriptPressed();
	}

	public void ExtendSelectedScript()
	{
		OnExtendScriptPressed();
	}

	public void ShowRenameSelectedDialog()
	{
		ShowRenameDialog();
	}

	public Node DuplicateSelectedNode()
	{
		Node node = _sceneTree?.DuplicateSelectedNode();
		if (node != null)
		{
			XWEditorInterface instance = XWEditorInterface.Instance;
			if (instance == null)
			{
				return node;
			}
			XW2DSceneEditor xW2DSceneEditor = instance.Get2DSceneEditor();
			if (xW2DSceneEditor == null)
			{
				return node;
			}
			xW2DSceneEditor.QueueViewportRedraw();
		}
		return node;
	}

	public bool MoveSelectedNode(int offset)
	{
		XWSceneNodeTree sceneTree = _sceneTree;
		int num;
		if (sceneTree == null)
		{
			num = 0;
		}
		else
		{
			num = (sceneTree.MoveSelectedNode(offset) ? 1 : 0);
			if (num != 0)
			{
				XWEditorInterface instance = XWEditorInterface.Instance;
				if (instance == null)
				{
					return (byte)num != 0;
				}
				XW2DSceneEditor xW2DSceneEditor = instance.Get2DSceneEditor();
				if (xW2DSceneEditor == null)
				{
					return (byte)num != 0;
				}
				xW2DSceneEditor.QueueViewportRedraw();
			}
		}
		return (byte)num != 0;
	}

	public bool CanReparentNode(Node node, Node newParent, out string reason)
	{
		if (_sceneTree == null)
		{
			reason = "2D 场景树尚未初始化。";
			return false;
		}
		return _sceneTree.CanReparentNode(node, newParent, out reason);
	}

	public bool ReparentNodeWithHistory(Node node, Node newParent, string actionName = "拖拽重挂 2D 节点")
	{
		if (_sceneTree == null)
		{
			return false;
		}
		bool flag = _sceneTree.ReparentNodeWithHistory(node, newParent, actionName);
		XWEditorInterface.Instance?.ShowToast(_sceneTree.LastReparentFeedback, (!flag) ? 2 : 0);
		if (flag)
		{
			XWEditorInterface.Instance?.Get2DSceneEditor()?.QueueViewportRedraw();
		}
		return flag;
	}

	public int GetTrackedReparentNodeRecordCount(int historyId = -1)
	{
		return _sceneTree?.GetTrackedReparentNodeRecordCount(historyId) ?? 0;
	}

	public int GetLastReparentHistoryId()
	{
		return _sceneTree?.LastReparentHistoryId ?? (-1);
	}

	public bool DidLastReparentPreserveGlobalTransform()
	{
		return _sceneTree?.LastReparentPreservedGlobalTransform ?? false;
	}

	public string GetLastReparentFeedback()
	{
		return _sceneTree?.LastReparentFeedback ?? "";
	}

	public bool RemoveSelectedNode()
	{
		XWSceneNodeTree sceneTree = _sceneTree;
		int num;
		if (sceneTree == null)
		{
			num = 0;
		}
		else
		{
			num = (sceneTree.RemoveSelectedNode() ? 1 : 0);
			if (num != 0)
			{
				XWEditorInterface instance = XWEditorInterface.Instance;
				if (instance == null)
				{
					return (byte)num != 0;
				}
				XW2DSceneEditor xW2DSceneEditor = instance.Get2DSceneEditor();
				if (xW2DSceneEditor == null)
				{
					return (byte)num != 0;
				}
				xW2DSceneEditor.QueueViewportRedraw();
			}
		}
		return (byte)num != 0;
	}

	public Node AddNodeToParentWithHistory(Node parent, Node node, string actionName)
	{
		Node node2 = _sceneTree?.AddChildNodeToWithHistory(parent, node, actionName);
		if (node2 != null)
		{
			XWEditorInterface instance = XWEditorInterface.Instance;
			if (instance == null)
			{
				return node2;
			}
			XW2DSceneEditor xW2DSceneEditor = instance.Get2DSceneEditor();
			if (xW2DSceneEditor == null)
			{
				return node2;
			}
			xW2DSceneEditor.QueueViewportRedraw();
		}
		return node2;
	}

	public bool RemoveNodeWithHistory(Node node, string actionName)
	{
		XWSceneNodeTree sceneTree = _sceneTree;
		int num;
		if (sceneTree == null)
		{
			num = 0;
		}
		else
		{
			num = (sceneTree.RemoveNodeWithHistory(node, actionName) ? 1 : 0);
			if (num != 0)
			{
				XWEditorInterface instance = XWEditorInterface.Instance;
				if (instance == null)
				{
					return (byte)num != 0;
				}
				XW2DSceneEditor xW2DSceneEditor = instance.Get2DSceneEditor();
				if (xW2DSceneEditor == null)
				{
					return (byte)num != 0;
				}
				xW2DSceneEditor.QueueViewportRedraw();
			}
		}
		return (byte)num != 0;
	}

	public bool MakeSelectedNodeRoot()
	{
		XWSceneNodeTree sceneTree = _sceneTree;
		int num;
		if (sceneTree == null)
		{
			num = 0;
		}
		else
		{
			num = (sceneTree.MakeSelectedNodeRoot() ? 1 : 0);
			if (num != 0)
			{
				XWEditorInterface instance = XWEditorInterface.Instance;
				if (instance == null)
				{
					return (byte)num != 0;
				}
				XW2DSceneEditor xW2DSceneEditor = instance.Get2DSceneEditor();
				if (xW2DSceneEditor == null)
				{
					return (byte)num != 0;
				}
				xW2DSceneEditor.QueueViewportRedraw();
			}
		}
		return (byte)num != 0;
	}

	public void ShowSaveBranchDialogForSelected()
	{
		ShowSaveBranchDialog();
	}

	private static bool HasScript(Node node)
	{
		return node.GetScript().VariantType != Variant.Type.Nil;
	}

	private static void PopupAtMouse(PopupMenu popup)
	{
		popup.Position = DisplayServer.MouseGetPosition();
		popup.Popup();
	}

	private static void PopupBelow(Control control, PopupMenu popup)
	{
		Rect2 globalRect = control.GetGlobalRect();
		popup.Position = new Vector2I((int)globalRect.Position.X, (int)(globalRect.Position.Y + globalRect.Size.Y));
		popup.Popup();
	}

	public void SetScene(Node scene)
	{
		_pendingSceneRoot = scene;
		if (_sceneTree != null)
		{
			if (scene != null && GodotObject.IsInstanceValid(scene))
			{
				XWEditorInterface.Instance?.SetEditedSceneRoot(scene);
				_sceneTree.SetSceneRoot(scene);
				UpdateSceneActionButtons(scene);
			}
			else
			{
				XWEditorInterface.Instance?.SetEditedSceneRoot(null);
				_sceneTree.SetSceneRoot(null);
				UpdateSceneActionButtons(null);
				UpdateScriptButtons(null);
			}
		}
	}

	public void ApplySceneRootFromHistory(Node scene)
	{
		_pendingSceneRoot = scene;
		if (_sceneTree != null)
		{
			_sceneTree.SetSceneRootFromHistory(scene);
			UpdateSceneActionButtons(scene);
			UpdateScriptButtons(scene);
		}
	}

	public void RefreshScene()
	{
		_sceneTree?.Refresh();
	}

	public void SelectNode(Node node, bool emitSignal = true)
	{
		_sceneTree?.SelectNode(node, emitSignal);
	}

	public void PrepareForSceneHistoryBranchChange()
	{
		_sceneTree?.PrepareForSceneHistoryBranchChange();
	}

	public void ReleaseHistoryResources(int historyId)
	{
		_sceneTree?.ReleaseHistoryResources(historyId);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(75)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupToolbarIcons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupFileDialogs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupAddNodeMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddNodeMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "displayName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetupContextMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncSceneRoot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAddNodePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAddNodeMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenCreateNodeDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddNodeByClass, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateNodeByClass, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildNodeName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnInstantiatePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnInstantiateFileSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InstantiateSceneWithHistory, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowComponentSceneDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnComponentSceneFileSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddComponentSceneWithHistory, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanAddComponentSceneToSelectedNode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsComponentContainerNode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindPreferredComponentSceneDirectory, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.OnFilterTextChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateContextMenuState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetContextMenuDisabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "itemId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "disabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTreeMouseSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "mouseButtonIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnContextMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowRenameDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRenameConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CopySelectedNodePath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildNodePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowSaveBranchDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSaveBranchFileSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnNodeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnHistorySelectionChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateScriptButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateSceneActionButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sceneRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnCreateScriptPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDetachScriptPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DetachSelectedScriptWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnExtendScriptPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnScriptFileSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateAndAttachScriptWithHistory, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Script"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SanitizeClassName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTreeMenuPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectedNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectedNodeHasScript, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsSelectedNodeRoot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowInstantiateDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowCreateScriptDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DetachSelectedScript, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExtendSelectedScript, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowRenameSelectedDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DuplicateSelectedNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedNode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReparentNodeWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "newParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTrackedReparentNodeRecordCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetLastReparentHistoryId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DidLastReparentPreserveGlobalTransform, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetLastReparentFeedback, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedNode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddNodeToParentWithHistory, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveNodeWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeSelectedNodeRoot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowSaveBranchDialogForSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasScript, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.PopupAtMouse, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "popup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false)
			}, null),
			new MethodInfo(MethodName.PopupBelow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "popup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySceneRootFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "emitSignal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareForSceneHistoryBranchChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseHistoryResources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SetupToolbarIcons && args.Count == 0)
		{
			SetupToolbarIcons();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupFileDialogs && args.Count == 0)
		{
			SetupFileDialogs();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupAddNodeMenu && args.Count == 0)
		{
			SetupAddNodeMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.AddNodeMenuItem && args.Count == 2)
		{
			AddNodeMenuItem(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetupContextMenu && args.Count == 0)
		{
			SetupContextMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectSignals && args.Count == 0)
		{
			ConnectSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncSceneRoot && args.Count == 0)
		{
			SyncSceneRoot();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAddNodePressed && args.Count == 0)
		{
			OnAddNodePressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAddNodeMenuItem && args.Count == 1)
		{
			OnAddNodeMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenCreateNodeDialog && args.Count == 0)
		{
			OpenCreateNodeDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.AddNodeByClass && args.Count == 1)
		{
			AddNodeByClass(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateNodeByClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(CreateNodeByClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildNodeName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildNodeName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.OnInstantiatePressed && args.Count == 0)
		{
			OnInstantiatePressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnInstantiateFileSelected && args.Count == 1)
		{
			OnInstantiateFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InstantiateSceneWithHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(InstantiateSceneWithHistory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShowComponentSceneDialog && args.Count == 0)
		{
			ShowComponentSceneDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.OnComponentSceneFileSelected && args.Count == 1)
		{
			OnComponentSceneFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddComponentSceneWithHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(AddComponentSceneWithHistory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanAddComponentSceneToSelectedNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanAddComponentSceneToSelectedNode());
			return true;
		}
		if (method == MethodName.IsComponentContainerNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsComponentContainerNode(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindPreferredComponentSceneDirectory && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(FindPreferredComponentSceneDirectory());
			return true;
		}
		if (method == MethodName.OnFilterTextChanged && args.Count == 1)
		{
			OnFilterTextChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateContextMenuState && args.Count == 0)
		{
			UpdateContextMenuState();
			ret = default;
			return true;
		}
		if (method == MethodName.SetContextMenuDisabled && args.Count == 2)
		{
			SetContextMenuDisabled(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTreeMouseSelected && args.Count == 2)
		{
			OnTreeMouseSelected(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnContextMenuItem && args.Count == 1)
		{
			OnContextMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowRenameDialog && args.Count == 0)
		{
			ShowRenameDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRenameConfirmed && args.Count == 0)
		{
			OnRenameConfirmed();
			ret = default;
			return true;
		}
		if (method == MethodName.CopySelectedNodePath && args.Count == 0)
		{
			CopySelectedNodePath();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildNodePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildNodePath(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ShowSaveBranchDialog && args.Count == 0)
		{
			ShowSaveBranchDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSaveBranchFileSelected && args.Count == 1)
		{
			OnSaveBranchFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnNodeSelected && args.Count == 1)
		{
			OnNodeSelected(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnHistorySelectionChanged && args.Count == 1)
		{
			OnHistorySelectionChanged(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateScriptButtons && args.Count == 1)
		{
			UpdateScriptButtons(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSceneActionButtons && args.Count == 1)
		{
			UpdateSceneActionButtons(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCreateScriptPressed && args.Count == 0)
		{
			OnCreateScriptPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDetachScriptPressed && args.Count == 0)
		{
			OnDetachScriptPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.DetachSelectedScriptWithHistory && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(DetachSelectedScriptWithHistory());
			return true;
		}
		if (method == MethodName.OnExtendScriptPressed && args.Count == 0)
		{
			OnExtendScriptPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnScriptFileSelected && args.Count == 1)
		{
			OnScriptFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateAndAttachScriptWithHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Script>(CreateAndAttachScriptWithHistory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SanitizeClassName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(SanitizeClassName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnTreeMenuPressed && args.Count == 0)
		{
			OnTreeMenuPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(GetSelectedNode());
			return true;
		}
		if (method == MethodName.SelectedNodeHasScript && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(SelectedNodeHasScript());
			return true;
		}
		if (method == MethodName.IsSelectedNodeRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSelectedNodeRoot());
			return true;
		}
		if (method == MethodName.ShowInstantiateDialog && args.Count == 0)
		{
			ShowInstantiateDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCreateScriptDialog && args.Count == 0)
		{
			ShowCreateScriptDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.DetachSelectedScript && args.Count == 0)
		{
			DetachSelectedScript();
			ret = default;
			return true;
		}
		if (method == MethodName.ExtendSelectedScript && args.Count == 0)
		{
			ExtendSelectedScript();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowRenameSelectedDialog && args.Count == 0)
		{
			ShowRenameSelectedDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.DuplicateSelectedNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(DuplicateSelectedNode());
			return true;
		}
		if (method == MethodName.MoveSelectedNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MoveSelectedNode(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ReparentNodeWithHistory && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ReparentNodeWithHistory(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.GetTrackedReparentNodeRecordCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetTrackedReparentNodeRecordCount(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLastReparentHistoryId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetLastReparentHistoryId());
			return true;
		}
		if (method == MethodName.DidLastReparentPreserveGlobalTransform && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(DidLastReparentPreserveGlobalTransform());
			return true;
		}
		if (method == MethodName.GetLastReparentFeedback && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetLastReparentFeedback());
			return true;
		}
		if (method == MethodName.RemoveSelectedNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveSelectedNode());
			return true;
		}
		if (method == MethodName.AddNodeToParentWithHistory && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Node>(AddNodeToParentWithHistory(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.RemoveNodeWithHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveNodeWithHistory(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.MakeSelectedNodeRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(MakeSelectedNodeRoot());
			return true;
		}
		if (method == MethodName.ShowSaveBranchDialogForSelected && args.Count == 0)
		{
			ShowSaveBranchDialogForSelected();
			ret = default;
			return true;
		}
		if (method == MethodName.HasScript && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasScript(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.PopupAtMouse && args.Count == 1)
		{
			PopupAtMouse(VariantUtils.ConvertTo<PopupMenu>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopupBelow && args.Count == 2)
		{
			PopupBelow(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<PopupMenu>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetScene && args.Count == 1)
		{
			SetScene(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySceneRootFromHistory && args.Count == 1)
		{
			ApplySceneRootFromHistory(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshScene && args.Count == 0)
		{
			RefreshScene();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectNode && args.Count == 2)
		{
			SelectNode(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareForSceneHistoryBranchChange && args.Count == 0)
		{
			PrepareForSceneHistoryBranchChange();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseHistoryResources && args.Count == 1)
		{
			ReleaseHistoryResources(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CreateNodeByClass && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(CreateNodeByClass(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildNodeName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildNodeName(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.IsComponentContainerNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsComponentContainerNode(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.FindPreferredComponentSceneDirectory && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(FindPreferredComponentSceneDirectory());
			return true;
		}
		if (method == MethodName.BuildNodePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildNodePath(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SanitizeClassName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(SanitizeClassName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasScript && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasScript(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.PopupAtMouse && args.Count == 1)
		{
			PopupAtMouse(VariantUtils.ConvertTo<PopupMenu>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopupBelow && args.Count == 2)
		{
			PopupBelow(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<PopupMenu>(in args[1]));
			ret = default;
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
		if (method == MethodName.SetupToolbarIcons)
		{
			return true;
		}
		if (method == MethodName.SetupFileDialogs)
		{
			return true;
		}
		if (method == MethodName.SetupAddNodeMenu)
		{
			return true;
		}
		if (method == MethodName.AddNodeMenuItem)
		{
			return true;
		}
		if (method == MethodName.SetupContextMenu)
		{
			return true;
		}
		if (method == MethodName.ConnectSignals)
		{
			return true;
		}
		if (method == MethodName.SyncSceneRoot)
		{
			return true;
		}
		if (method == MethodName.OnAddNodePressed)
		{
			return true;
		}
		if (method == MethodName.OnAddNodeMenuItem)
		{
			return true;
		}
		if (method == MethodName.OpenCreateNodeDialog)
		{
			return true;
		}
		if (method == MethodName.AddNodeByClass)
		{
			return true;
		}
		if (method == MethodName.CreateNodeByClass)
		{
			return true;
		}
		if (method == MethodName.BuildNodeName)
		{
			return true;
		}
		if (method == MethodName.OnInstantiatePressed)
		{
			return true;
		}
		if (method == MethodName.OnInstantiateFileSelected)
		{
			return true;
		}
		if (method == MethodName.InstantiateSceneWithHistory)
		{
			return true;
		}
		if (method == MethodName.ShowComponentSceneDialog)
		{
			return true;
		}
		if (method == MethodName.OnComponentSceneFileSelected)
		{
			return true;
		}
		if (method == MethodName.AddComponentSceneWithHistory)
		{
			return true;
		}
		if (method == MethodName.CanAddComponentSceneToSelectedNode)
		{
			return true;
		}
		if (method == MethodName.IsComponentContainerNode)
		{
			return true;
		}
		if (method == MethodName.FindPreferredComponentSceneDirectory)
		{
			return true;
		}
		if (method == MethodName.OnFilterTextChanged)
		{
			return true;
		}
		if (method == MethodName.UpdateContextMenuState)
		{
			return true;
		}
		if (method == MethodName.SetContextMenuDisabled)
		{
			return true;
		}
		if (method == MethodName.OnTreeMouseSelected)
		{
			return true;
		}
		if (method == MethodName.OnContextMenuItem)
		{
			return true;
		}
		if (method == MethodName.ShowRenameDialog)
		{
			return true;
		}
		if (method == MethodName.OnRenameConfirmed)
		{
			return true;
		}
		if (method == MethodName.CopySelectedNodePath)
		{
			return true;
		}
		if (method == MethodName.BuildNodePath)
		{
			return true;
		}
		if (method == MethodName.ShowSaveBranchDialog)
		{
			return true;
		}
		if (method == MethodName.OnSaveBranchFileSelected)
		{
			return true;
		}
		if (method == MethodName.OnNodeSelected)
		{
			return true;
		}
		if (method == MethodName.OnHistorySelectionChanged)
		{
			return true;
		}
		if (method == MethodName.UpdateScriptButtons)
		{
			return true;
		}
		if (method == MethodName.UpdateSceneActionButtons)
		{
			return true;
		}
		if (method == MethodName.OnCreateScriptPressed)
		{
			return true;
		}
		if (method == MethodName.OnDetachScriptPressed)
		{
			return true;
		}
		if (method == MethodName.DetachSelectedScriptWithHistory)
		{
			return true;
		}
		if (method == MethodName.OnExtendScriptPressed)
		{
			return true;
		}
		if (method == MethodName.OnScriptFileSelected)
		{
			return true;
		}
		if (method == MethodName.CreateAndAttachScriptWithHistory)
		{
			return true;
		}
		if (method == MethodName.SanitizeClassName)
		{
			return true;
		}
		if (method == MethodName.OnTreeMenuPressed)
		{
			return true;
		}
		if (method == MethodName.GetSelectedNode)
		{
			return true;
		}
		if (method == MethodName.SelectedNodeHasScript)
		{
			return true;
		}
		if (method == MethodName.IsSelectedNodeRoot)
		{
			return true;
		}
		if (method == MethodName.ShowInstantiateDialog)
		{
			return true;
		}
		if (method == MethodName.ShowCreateScriptDialog)
		{
			return true;
		}
		if (method == MethodName.DetachSelectedScript)
		{
			return true;
		}
		if (method == MethodName.ExtendSelectedScript)
		{
			return true;
		}
		if (method == MethodName.ShowRenameSelectedDialog)
		{
			return true;
		}
		if (method == MethodName.DuplicateSelectedNode)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedNode)
		{
			return true;
		}
		if (method == MethodName.ReparentNodeWithHistory)
		{
			return true;
		}
		if (method == MethodName.GetTrackedReparentNodeRecordCount)
		{
			return true;
		}
		if (method == MethodName.GetLastReparentHistoryId)
		{
			return true;
		}
		if (method == MethodName.DidLastReparentPreserveGlobalTransform)
		{
			return true;
		}
		if (method == MethodName.GetLastReparentFeedback)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedNode)
		{
			return true;
		}
		if (method == MethodName.AddNodeToParentWithHistory)
		{
			return true;
		}
		if (method == MethodName.RemoveNodeWithHistory)
		{
			return true;
		}
		if (method == MethodName.MakeSelectedNodeRoot)
		{
			return true;
		}
		if (method == MethodName.ShowSaveBranchDialogForSelected)
		{
			return true;
		}
		if (method == MethodName.HasScript)
		{
			return true;
		}
		if (method == MethodName.PopupAtMouse)
		{
			return true;
		}
		if (method == MethodName.PopupBelow)
		{
			return true;
		}
		if (method == MethodName.SetScene)
		{
			return true;
		}
		if (method == MethodName.ApplySceneRootFromHistory)
		{
			return true;
		}
		if (method == MethodName.RefreshScene)
		{
			return true;
		}
		if (method == MethodName.SelectNode)
		{
			return true;
		}
		if (method == MethodName.PrepareForSceneHistoryBranchChange)
		{
			return true;
		}
		if (method == MethodName.ReleaseHistoryResources)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._addNodeBtn)
		{
			_addNodeBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._instantiateBtn)
		{
			_instantiateBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._filterLineEdit)
		{
			_filterLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._createScriptBtn)
		{
			_createScriptBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._detachScriptBtn)
		{
			_detachScriptBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._extendScriptBtn)
		{
			_extendScriptBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._treeMenuBtn)
		{
			_treeMenuBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._sceneTree)
		{
			_sceneTree = VariantUtils.ConvertTo<XWSceneNodeTree>(in value);
			return true;
		}
		if (name == PropertyName._addNodeMenu)
		{
			_addNodeMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._contextMenu)
		{
			_contextMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._renameDialog)
		{
			_renameDialog = VariantUtils.ConvertTo<ConfirmationDialog>(in value);
			return true;
		}
		if (name == PropertyName._renameLineEdit)
		{
			_renameLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._instantiateDialog)
		{
			_instantiateDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._componentSceneDialog)
		{
			_componentSceneDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._scriptDialog)
		{
			_scriptDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._saveBranchDialog)
		{
			_saveBranchDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._nextAddNodeId)
		{
			_nextAddNodeId = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._pendingSceneRoot)
		{
			_pendingSceneRoot = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._addNodeBtn)
		{
			value = VariantUtils.CreateFrom(in _addNodeBtn);
			return true;
		}
		if (name == PropertyName._instantiateBtn)
		{
			value = VariantUtils.CreateFrom(in _instantiateBtn);
			return true;
		}
		if (name == PropertyName._filterLineEdit)
		{
			value = VariantUtils.CreateFrom(in _filterLineEdit);
			return true;
		}
		if (name == PropertyName._createScriptBtn)
		{
			value = VariantUtils.CreateFrom(in _createScriptBtn);
			return true;
		}
		if (name == PropertyName._detachScriptBtn)
		{
			value = VariantUtils.CreateFrom(in _detachScriptBtn);
			return true;
		}
		if (name == PropertyName._extendScriptBtn)
		{
			value = VariantUtils.CreateFrom(in _extendScriptBtn);
			return true;
		}
		if (name == PropertyName._treeMenuBtn)
		{
			value = VariantUtils.CreateFrom(in _treeMenuBtn);
			return true;
		}
		if (name == PropertyName._sceneTree)
		{
			value = VariantUtils.CreateFrom(in _sceneTree);
			return true;
		}
		if (name == PropertyName._addNodeMenu)
		{
			value = VariantUtils.CreateFrom(in _addNodeMenu);
			return true;
		}
		if (name == PropertyName._contextMenu)
		{
			value = VariantUtils.CreateFrom(in _contextMenu);
			return true;
		}
		if (name == PropertyName._renameDialog)
		{
			value = VariantUtils.CreateFrom(in _renameDialog);
			return true;
		}
		if (name == PropertyName._renameLineEdit)
		{
			value = VariantUtils.CreateFrom(in _renameLineEdit);
			return true;
		}
		if (name == PropertyName._instantiateDialog)
		{
			value = VariantUtils.CreateFrom(in _instantiateDialog);
			return true;
		}
		if (name == PropertyName._componentSceneDialog)
		{
			value = VariantUtils.CreateFrom(in _componentSceneDialog);
			return true;
		}
		if (name == PropertyName._scriptDialog)
		{
			value = VariantUtils.CreateFrom(in _scriptDialog);
			return true;
		}
		if (name == PropertyName._saveBranchDialog)
		{
			value = VariantUtils.CreateFrom(in _saveBranchDialog);
			return true;
		}
		if (name == PropertyName._nextAddNodeId)
		{
			value = VariantUtils.CreateFrom(in _nextAddNodeId);
			return true;
		}
		if (name == PropertyName._pendingSceneRoot)
		{
			value = VariantUtils.CreateFrom(in _pendingSceneRoot);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._addNodeBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._instantiateBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._filterLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._createScriptBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._detachScriptBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._extendScriptBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._treeMenuBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sceneTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addNodeMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._contextMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renameDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renameLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._instantiateDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._componentSceneDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scriptDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveBranchDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextAddNodeId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pendingSceneRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._addNodeBtn, Variant.From(in _addNodeBtn));
		info.AddProperty(PropertyName._instantiateBtn, Variant.From(in _instantiateBtn));
		info.AddProperty(PropertyName._filterLineEdit, Variant.From(in _filterLineEdit));
		info.AddProperty(PropertyName._createScriptBtn, Variant.From(in _createScriptBtn));
		info.AddProperty(PropertyName._detachScriptBtn, Variant.From(in _detachScriptBtn));
		info.AddProperty(PropertyName._extendScriptBtn, Variant.From(in _extendScriptBtn));
		info.AddProperty(PropertyName._treeMenuBtn, Variant.From(in _treeMenuBtn));
		info.AddProperty(PropertyName._sceneTree, Variant.From(in _sceneTree));
		info.AddProperty(PropertyName._addNodeMenu, Variant.From(in _addNodeMenu));
		info.AddProperty(PropertyName._contextMenu, Variant.From(in _contextMenu));
		info.AddProperty(PropertyName._renameDialog, Variant.From(in _renameDialog));
		info.AddProperty(PropertyName._renameLineEdit, Variant.From(in _renameLineEdit));
		info.AddProperty(PropertyName._instantiateDialog, Variant.From(in _instantiateDialog));
		info.AddProperty(PropertyName._componentSceneDialog, Variant.From(in _componentSceneDialog));
		info.AddProperty(PropertyName._scriptDialog, Variant.From(in _scriptDialog));
		info.AddProperty(PropertyName._saveBranchDialog, Variant.From(in _saveBranchDialog));
		info.AddProperty(PropertyName._nextAddNodeId, Variant.From(in _nextAddNodeId));
		info.AddProperty(PropertyName._pendingSceneRoot, Variant.From(in _pendingSceneRoot));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._addNodeBtn, out var value))
		{
			_addNodeBtn = value.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._instantiateBtn, out var value2))
		{
			_instantiateBtn = value2.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._filterLineEdit, out var value3))
		{
			_filterLineEdit = value3.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._createScriptBtn, out var value4))
		{
			_createScriptBtn = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._detachScriptBtn, out var value5))
		{
			_detachScriptBtn = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._extendScriptBtn, out var value6))
		{
			_extendScriptBtn = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._treeMenuBtn, out var value7))
		{
			_treeMenuBtn = value7.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._sceneTree, out var value8))
		{
			_sceneTree = value8.As<XWSceneNodeTree>();
		}
		if (info.TryGetProperty(PropertyName._addNodeMenu, out var value9))
		{
			_addNodeMenu = value9.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._contextMenu, out var value10))
		{
			_contextMenu = value10.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._renameDialog, out var value11))
		{
			_renameDialog = value11.As<ConfirmationDialog>();
		}
		if (info.TryGetProperty(PropertyName._renameLineEdit, out var value12))
		{
			_renameLineEdit = value12.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._instantiateDialog, out var value13))
		{
			_instantiateDialog = value13.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._componentSceneDialog, out var value14))
		{
			_componentSceneDialog = value14.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._scriptDialog, out var value15))
		{
			_scriptDialog = value15.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._saveBranchDialog, out var value16))
		{
			_saveBranchDialog = value16.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._nextAddNodeId, out var value17))
		{
			_nextAddNodeId = value17.As<long>();
		}
		if (info.TryGetProperty(PropertyName._pendingSceneRoot, out var value18))
		{
			_pendingSceneRoot = value18.As<Node>();
		}
	}
}
