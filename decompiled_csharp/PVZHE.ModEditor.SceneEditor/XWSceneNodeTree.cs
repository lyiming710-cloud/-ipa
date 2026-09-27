using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://addons/ModEditor/SceneEditor/SceneNodeTree/Tree/XWSceneNodeTree.cs")]
public class XWSceneNodeTree : Tree
{
	[Signal]
	public delegate void NodeSelectedEventHandler(Node node);

	[Signal]
	public delegate void NodeRenamedEventHandler(Node node, string newName);

	[Signal]
	public delegate void HistorySelectionChangedEventHandler(Node node);

	private sealed class StructuralNodeRecord
	{
		public int HistoryId;

		public Node Parent;

		public Node Node;

		public Node AttachedSelection;

		public Node DetachedSelection;

		public int Index;

		public bool Applied;

		public bool InRedoBranch;
	}

	private sealed class SceneRootRecord
	{
		public int HistoryId;

		public Node OldRoot;

		public Node NewRoot;

		public Node OldParent;

		public int OldIndex;

		public readonly List<Node> RemappedOwnerNodes = new List<Node>();

		public bool NewRootActive;

		public bool InRedoBranch;
	}

	private sealed class ReparentNodeRecord
	{
		public int HistoryId;

		public Node Node;

		public Node OldParent;

		public Node NewParent;

		public int OldIndex;

		public int NewIndex;

		public StringName OldName;

		public StringName NewName;

		public readonly System.Collections.Generic.Dictionary<Node, Node> OldOwners = new System.Collections.Generic.Dictionary<Node, Node>();

		public readonly System.Collections.Generic.Dictionary<Node, Node> NewOwners = new System.Collections.Generic.Dictionary<Node, Node>();

		public CanvasTransformSnapshot CanvasTransform;

		public bool NewOwnersCaptured;

		public bool AppliedToNewParent;

		public bool InRedoBranch;
	}

	private readonly struct CanvasTransformSnapshot
	{
		public readonly Node2D Node2D;

		public readonly Control Control;

		public readonly Transform2D GlobalTransform;

		public readonly Vector2 ControlSize;

		public readonly Vector2 ControlGlobalPosition;

		public bool HasCanvasTransform
		{
			get
			{
				if (!GodotObject.IsInstanceValid(Node2D))
				{
					return GodotObject.IsInstanceValid(Control);
				}
				return true;
			}
		}

		public CanvasTransformSnapshot(Node2D node)
		{
			Node2D = node;
			Control = null;
			GlobalTransform = node.GlobalTransform;
			ControlSize = Vector2.Zero;
			ControlGlobalPosition = Vector2.Zero;
		}

		public CanvasTransformSnapshot(Control control)
		{
			Node2D = null;
			Control = control;
			GlobalTransform = control.GetGlobalTransform();
			ControlSize = control.Size;
			ControlGlobalPosition = control.GlobalPosition;
		}
	}

	public new class MethodName : Tree.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName GetEditedSceneRoot = "GetEditedSceneRoot";

		public static readonly StringName SetSceneRoot = "SetSceneRoot";

		public static readonly StringName SetSceneRootFromHistory = "SetSceneRootFromHistory";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName BuildTree = "BuildTree";

		public static readonly StringName SetupItem = "SetupItem";

		public static readonly StringName ApplyFilter = "ApplyFilter";

		public static readonly StringName BuildFilterTerms = "BuildFilterTerms";

		public static readonly StringName FilterRecursive = "FilterRecursive";

		public static readonly StringName MatchesFilter = "MatchesFilter";

		public static readonly StringName NodeHasGroup = "NodeHasGroup";

		public static readonly StringName SetAllVisible = "SetAllVisible";

		public static readonly StringName OnItemSelected = "OnItemSelected";

		public static readonly StringName OnItemEdited = "OnItemEdited";

		public static readonly StringName OnButtonClicked = "OnButtonClicked";

		public new static readonly StringName _GetDragData = "_GetDragData";

		public new static readonly StringName _CanDropData = "_CanDropData";

		public new static readonly StringName _DropData = "_DropData";

		public static readonly StringName ToggleNodeVisibilityWithHistory = "ToggleNodeVisibilityWithHistory";

		public static readonly StringName SetNodeVisibilityWithHistory = "SetNodeVisibilityWithHistory";

		public static readonly StringName ApplyNodeVisibility = "ApplyNodeVisibility";

		public static readonly StringName ToggleNodeLockWithHistory = "ToggleNodeLockWithHistory";

		public static readonly StringName SetNodeLockWithHistory = "SetNodeLockWithHistory";

		public static readonly StringName ApplyNodeLock = "ApplyNodeLock";

		public static readonly StringName ApplyNodeLockStorage = "ApplyNodeLockStorage";

		public static readonly StringName SetNodeScriptWithHistory = "SetNodeScriptWithHistory";

		public static readonly StringName ApplyNodeScript = "ApplyNodeScript";

		public static readonly StringName GetSelectedNode = "GetSelectedNode";

		public static readonly StringName GetSelectedItem = "GetSelectedItem";

		public static readonly StringName IsNodeLocked = "IsNodeLocked";

		public static readonly StringName SelectNode = "SelectNode";

		public static readonly StringName SelectRoot = "SelectRoot";

		public static readonly StringName AddChildNode = "AddChildNode";

		public static readonly StringName AddChildNodeTo = "AddChildNodeTo";

		public static readonly StringName AddChildNodeWithHistory = "AddChildNodeWithHistory";

		public static readonly StringName AddChildNodeToWithHistory = "AddChildNodeToWithHistory";

		public static readonly StringName RemoveSelectedNode = "RemoveSelectedNode";

		public static readonly StringName RemoveNodeWithHistory = "RemoveNodeWithHistory";

		public static readonly StringName DuplicateSelectedNode = "DuplicateSelectedNode";

		public static readonly StringName RenameSelectedNode = "RenameSelectedNode";

		public static readonly StringName MoveSelectedNode = "MoveSelectedNode";

		public static readonly StringName ReparentSelectedNodeWithHistory = "ReparentSelectedNodeWithHistory";

		public static readonly StringName ReparentNodeWithHistory = "ReparentNodeWithHistory";

		public static readonly StringName ApplyReparentNodeRecord = "ApplyReparentNodeRecord";

		public static readonly StringName RenameNode = "RenameNode";

		public static readonly StringName ApplyNodeName = "ApplyNodeName";

		public static readonly StringName ApplyNodeIndex = "ApplyNodeIndex";

		public static readonly StringName AttachStructuralNode = "AttachStructuralNode";

		public static readonly StringName DetachStructuralNode = "DetachStructuralNode";

		public static readonly StringName PrepareForSceneHistoryBranchChange = "PrepareForSceneHistoryBranchChange";

		public static readonly StringName ReleaseHistoryResources = "ReleaseHistoryResources";

		public static readonly StringName GetTrackedStructuralNodeRecordCount = "GetTrackedStructuralNodeRecordCount";

		public static readonly StringName GetTrackedSceneRootRecordCount = "GetTrackedSceneRootRecordCount";

		public static readonly StringName GetTrackedReparentNodeRecordCount = "GetTrackedReparentNodeRecordCount";

		public static readonly StringName MakeSelectedNodeRoot = "MakeSelectedNodeRoot";

		public static readonly StringName ApplySceneRootRecord = "ApplySceneRootRecord";

		public static readonly StringName FindItemForNode = "FindItemForNode";

		public static readonly StringName PrepareSceneHistoryMutation = "PrepareSceneHistoryMutation";

		public static readonly StringName CurrentSceneHistoryId = "CurrentSceneHistoryId";

		public static readonly StringName SyncHistorySelection = "SyncHistorySelection";

		public static readonly StringName ReleaseStructuralNodeRecordsIfHistoryWasCleared = "ReleaseStructuralNodeRecordsIfHistoryWasCleared";

		public static readonly StringName ReleaseSceneRootRecordsIfHistoryWasCleared = "ReleaseSceneRootRecordsIfHistoryWasCleared";

		public static readonly StringName ReleaseReparentNodeRecordsIfHistoryWasCleared = "ReleaseReparentNodeRecordsIfHistoryWasCleared";

		public static readonly StringName ReleaseStructuralNodeRecords = "ReleaseStructuralNodeRecords";

		public static readonly StringName ReleaseSceneRootRecords = "ReleaseSceneRootRecords";

		public static readonly StringName ReleaseReparentNodeRecords = "ReleaseReparentNodeRecords";

		public static readonly StringName IsNodeInEditedScene = "IsNodeInEditedScene";

		public static readonly StringName RepairOwnersAfterReparent = "RepairOwnersAfterReparent";

		public static readonly StringName RepairOwnerRecursive = "RepairOwnerRecursive";

		public static readonly StringName IsSiblingNameAvailable = "IsSiblingNameAvailable";

		public static readonly StringName BuildUniqueSiblingName = "BuildUniqueSiblingName";

		public static readonly StringName HasChildNamed = "HasChildNamed";

		public static readonly StringName RefreshAndReselect = "RefreshAndReselect";

		public static readonly StringName AssignOwnerForAddedSubtree = "AssignOwnerForAddedSubtree";

		public static readonly StringName PruneInvalidLocks = "PruneInvalidLocks";

		public static readonly StringName HasScript = "HasScript";
	}

	public new class PropertyName : Tree.PropertyName
	{
		public static readonly StringName LastReparentFeedback = "LastReparentFeedback";

		public static readonly StringName LastReparentHistoryId = "LastReparentHistoryId";

		public static readonly StringName LastReparentPreservedGlobalTransform = "LastReparentPreservedGlobalTransform";

		public static readonly StringName _editedSceneRoot = "_editedSceneRoot";

		public static readonly StringName _rootItem = "_rootItem";

		public static readonly StringName _suppressItemSelected = "_suppressItemSelected";

		public static readonly StringName _nextStructuralNodeRecordId = "_nextStructuralNodeRecordId";

		public static readonly StringName _nextSceneRootRecordId = "_nextSceneRootRecordId";

		public static readonly StringName _nextReparentNodeRecordId = "_nextReparentNodeRecordId";

		public static readonly StringName _filterText = "_filterText";

		public static readonly StringName _visibleIcon = "_visibleIcon";

		public static readonly StringName _hiddenIcon = "_hiddenIcon";

		public static readonly StringName _lockIcon = "_lockIcon";

		public static readonly StringName _unlockIcon = "_unlockIcon";

		public static readonly StringName _scriptIcon = "_scriptIcon";
	}

	public new class SignalName : Tree.SignalName
	{
		public static readonly StringName NodeSelected = "NodeSelected";

		public static readonly StringName NodeRenamed = "NodeRenamed";

		public static readonly StringName HistorySelectionChanged = "HistorySelectionChanged";
	}

	private const int ColumnName = 0;

	private const int ColumnVisibility = 1;

	private const int ColumnLock = 2;

	private const int ColumnScript = 3;

	private const int ColumnCount = 4;

	private const int ButtonVisibility = 1;

	private const int ButtonLock = 2;

	private const int ButtonScript = 3;

	private const string EditLockMeta = "_edit_lock_";

	private const string SceneNodeDragType = "xw_2d_scene_node";

	private const double TransformEpsilon = 0.001;

	private Node _editedSceneRoot;

	private TreeItem _rootItem;

	private readonly System.Collections.Generic.Dictionary<TreeItem, Node> _itemToNode = new System.Collections.Generic.Dictionary<TreeItem, Node>();

	private readonly System.Collections.Generic.Dictionary<Node, TreeItem> _nodeToItem = new System.Collections.Generic.Dictionary<Node, TreeItem>();

	private bool _suppressItemSelected;

	private readonly HashSet<Node> _lockedNodes = new HashSet<Node>();

	private readonly System.Collections.Generic.Dictionary<long, StructuralNodeRecord> _structuralNodeRecords = new System.Collections.Generic.Dictionary<long, StructuralNodeRecord>();

	private readonly System.Collections.Generic.Dictionary<long, SceneRootRecord> _sceneRootRecords = new System.Collections.Generic.Dictionary<long, SceneRootRecord>();

	private readonly System.Collections.Generic.Dictionary<long, ReparentNodeRecord> _reparentNodeRecords = new System.Collections.Generic.Dictionary<long, ReparentNodeRecord>();

	private long _nextStructuralNodeRecordId = 1L;

	private long _nextSceneRootRecordId = 1L;

	private long _nextReparentNodeRecordId = 1L;

	private string _filterText = "";

	private Texture2D _visibleIcon;

	private Texture2D _hiddenIcon;

	private Texture2D _lockIcon;

	private Texture2D _unlockIcon;

	private Texture2D _scriptIcon;

	private NodeSelectedEventHandler backing_NodeSelected;

	private NodeRenamedEventHandler backing_NodeRenamed;

	private HistorySelectionChangedEventHandler backing_HistorySelectionChanged;

	public string LastReparentFeedback { get; private set; } = "";

	public int LastReparentHistoryId { get; private set; } = -1;

	public bool LastReparentPreservedGlobalTransform { get; private set; }

	public event NodeSelectedEventHandler NodeSelected
	{
		add
		{
			backing_NodeSelected = (NodeSelectedEventHandler)Delegate.Combine(backing_NodeSelected, value);
		}
		remove
		{
			backing_NodeSelected = (NodeSelectedEventHandler)Delegate.Remove(backing_NodeSelected, value);
		}
	}

	public event NodeRenamedEventHandler NodeRenamed
	{
		add
		{
			backing_NodeRenamed = (NodeRenamedEventHandler)Delegate.Combine(backing_NodeRenamed, value);
		}
		remove
		{
			backing_NodeRenamed = (NodeRenamedEventHandler)Delegate.Remove(backing_NodeRenamed, value);
		}
	}

	public event HistorySelectionChangedEventHandler HistorySelectionChanged
	{
		add
		{
			backing_HistorySelectionChanged = (HistorySelectionChangedEventHandler)Delegate.Combine(backing_HistorySelectionChanged, value);
		}
		remove
		{
			backing_HistorySelectionChanged = (HistorySelectionChangedEventHandler)Delegate.Remove(backing_HistorySelectionChanged, value);
		}
	}

	public override void _Ready()
	{
		SetColumns(4);
		SetColumnExpand(0, expand: true);
		SetColumnExpand(1, expand: false);
		SetColumnExpand(2, expand: false);
		SetColumnExpand(3, expand: false);
		SetColumnCustomMinimumWidth(1, 24);
		SetColumnCustomMinimumWidth(2, 24);
		SetColumnCustomMinimumWidth(3, 24);
		SetColumnClipContent(0, enable: true);
		SetAllowRmbSelect(allow: true);
		SetAllowReselect(allow: true);
		SetHideRoot(enable: false);
		HideFolding = false;
		DropModeFlags = 0;
		XWClassRegistry instance = XWClassRegistry.Instance;
		_visibleIcon = instance.GetUIIcon("GuiVisibilityVisible");
		_hiddenIcon = instance.GetUIIcon("GuiVisibilityHidden");
		_lockIcon = instance.GetUIIcon("Lock");
		_unlockIcon = instance.GetUIIcon("Unlock");
		_scriptIcon = instance.GetUIIcon("Script");
		ItemSelected += OnItemSelected;
		ItemEdited += OnItemEdited;
		ButtonClicked += OnButtonClicked;
	}

	public override void _ExitTree()
	{
		ReleaseStructuralNodeRecords(releaseAll: true);
		ReleaseSceneRootRecords(releaseAll: true);
		ReleaseReparentNodeRecords(releaseAll: true);
	}

	public Node GetEditedSceneRoot()
	{
		return _editedSceneRoot;
	}

	public void SetSceneRoot(Node root)
	{
		_editedSceneRoot = root;
		Refresh();
		SelectNode(root);
	}

	public void SetSceneRootFromHistory(Node root)
	{
		_editedSceneRoot = root;
		Refresh();
	}

	public void Refresh()
	{
		Clear();
		_itemToNode.Clear();
		_nodeToItem.Clear();
		PruneInvalidLocks();
		if (_editedSceneRoot != null && GodotObject.IsInstanceValid(_editedSceneRoot))
		{
			_rootItem = CreateItem();
			SetupItem(_rootItem, _editedSceneRoot);
			BuildTree(_rootItem, _editedSceneRoot);
			ApplyFilter(_filterText);
		}
	}

	private void BuildTree(TreeItem parentItem, Node parentNode)
	{
		foreach (Node child in parentNode.GetChildren())
		{
			if (child != null)
			{
				Node node = child;
				TreeItem treeItem = CreateItem(parentItem);
				SetupItem(treeItem, node);
				BuildTree(treeItem, node);
			}
		}
	}

	private void SetupItem(TreeItem item, Node node)
	{
		_itemToNode[item] = node;
		_nodeToItem[node] = item;
		item.SetText(0, node.Name);
		item.SetEditable(0, enabled: true);
		item.SetMetadata(0, node.GetInstanceId());
		item.SetTooltipText(0, $"{node.Name} ({node.GetClass()})");
		Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(node.GetClass());
		if (GodotObject.IsInstanceValid(classIcon))
		{
			item.SetIcon(0, classIcon);
		}
		if (node is CanvasItem canvasItem)
		{
			item.AddButton(1, canvasItem.Visible ? _visibleIcon : _hiddenIcon, 1, disabled: false, canvasItem.Visible ? "隐藏节点" : "显示节点");
		}
		bool flag = IsNodeLocked(node);
		item.AddButton(2, flag ? _lockIcon : _unlockIcon, 2, disabled: false, flag ? "解锁节点" : "锁定节点");
		if (HasScript(node))
		{
			item.AddButton(3, _scriptIcon, 3, disabled: false, "检查脚本");
		}
	}

	public void ApplyFilter(string filterText)
	{
		_filterText = filterText ?? "";
		if (_rootItem != null)
		{
			if (string.IsNullOrWhiteSpace(_filterText))
			{
				SetAllVisible(_rootItem, visible: true);
				return;
			}
			SetAllVisible(_rootItem, visible: false);
			FilterRecursive(_rootItem, BuildFilterTerms(_filterText));
		}
	}

	private static string[] BuildFilterTerms(string filterText)
	{
		return filterText.Split(new char[4] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
	}

	private bool FilterRecursive(TreeItem item, string[] terms)
	{
		bool flag = false;
		for (TreeItem treeItem = item.GetFirstChild(); treeItem != null; treeItem = treeItem.GetNext())
		{
			bool flag2 = FilterRecursive(treeItem, terms);
			flag |= flag2;
		}
		bool flag3 = (item.Visible = (_itemToNode.TryGetValue(item, out var value) && MatchesFilter(value, terms)) | flag);
		if (flag3 & flag)
		{
			item.Collapsed = false;
		}
		return flag3;
	}

	private static bool MatchesFilter(Node node, string[] terms)
	{
		if (terms.Length == 0)
		{
			return true;
		}
		string text = node.Name.ToString();
		string text2 = node.GetClass();
		for (int i = 0; i < terms.Length; i++)
		{
			string text3 = terms[i].Trim();
			if (text3.Length == 0)
			{
				continue;
			}
			if (text3.StartsWith("type:", StringComparison.OrdinalIgnoreCase))
			{
				string text4 = text3;
				if (!text2.Contains(text4.Substring(5, text4.Length - 5), StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
			}
			else if (text3.StartsWith("t:", StringComparison.OrdinalIgnoreCase))
			{
				string text4 = text3;
				if (!text2.Contains(text4.Substring(2, text4.Length - 2), StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
			}
			else if (text3.StartsWith("group:", StringComparison.OrdinalIgnoreCase))
			{
				string text4 = text3;
				if (!NodeHasGroup(node, text4.Substring(6, text4.Length - 6)))
				{
					return false;
				}
			}
			else if (text3.StartsWith("g:", StringComparison.OrdinalIgnoreCase))
			{
				string text4 = text3;
				if (!NodeHasGroup(node, text4.Substring(2, text4.Length - 2)))
				{
					return false;
				}
			}
			else if (!text.Contains(text3, StringComparison.OrdinalIgnoreCase) && !text2.Contains(text3, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
		}
		return true;
	}

	private static bool NodeHasGroup(Node node, string groupFilter)
	{
		foreach (StringName group in node.GetGroups())
		{
			if (group.ToString().Contains(groupFilter, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	private void SetAllVisible(TreeItem item, bool visible)
	{
		if (item != null)
		{
			item.Visible = visible;
			for (TreeItem treeItem = item.GetFirstChild(); treeItem != null; treeItem = treeItem.GetNext())
			{
				SetAllVisible(treeItem, visible);
			}
		}
	}

	private void OnItemSelected()
	{
		ReleaseStructuralNodeRecordsIfHistoryWasCleared();
		if (!_suppressItemSelected)
		{
			TreeItem selected = GetSelected();
			if (selected != null && _itemToNode.TryGetValue(selected, out var value))
			{
				EmitSignal(SignalName.NodeSelected, value);
			}
		}
	}

	private void OnItemEdited()
	{
		TreeItem edited = GetEdited();
		if (edited != null && _itemToNode.TryGetValue(edited, out var value))
		{
			string newName = edited.GetText(0).ToString();
			if (!RenameNode(value, newName))
			{
				edited.SetText(0, value.Name);
			}
		}
	}

	private void OnButtonClicked(TreeItem item, long column, long id, long mouseButtonIndex)
	{
		if (item == null || !_itemToNode.TryGetValue(item, out var value))
		{
			return;
		}
		long num = id - 1;
		if ((ulong)num <= 2uL)
		{
			switch ((int)num)
			{
			case 0:
				ToggleNodeVisibilityWithHistory(value);
				break;
			case 1:
				ToggleNodeLockWithHistory(value);
				break;
			case 2:
				XWEditorInterface.Instance?.Get2DSceneEditor()?.ShowDirectNodeProperties(value);
				XWEditorInterface.Instance?.ShowToast("已在 2D 主工作区打开脚本节点属性");
				break;
			}
		}
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		TreeItem itemAtPosition = GetItemAtPosition(atPosition);
		if (itemAtPosition == null || !_itemToNode.TryGetValue(itemAtPosition, out var from))
		{
			return default;
		}
		if (!CanBeginNodeDrag(from, out var reason))
		{
			LastReparentFeedback = reason;
			return default;
		}
		PanelContainer panelContainer = new PanelContainer
		{
			MouseFilter = MouseFilterEnum.Ignore
		};
		panelContainer.AddChild(new Label
		{
			Text = $"↳ 将“{from.Name}”重挂到目标节点",
			MouseFilter = MouseFilterEnum.Ignore
		}, forceReadableName: false, InternalMode.Disabled);
		SetDragPreview(panelContainer);
		Dictionary from2 = new Dictionary
		{
			["type"] = "xw_2d_scene_node",
			["node"] = Variant.From(in from),
			["scene_root_id"] = (long)_editedSceneRoot.GetInstanceId()
		};
		LastReparentFeedback = $"正在拖动节点“{from.Name}”。";
		return Variant.From(in from2);
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		DropModeFlags = 0;
		if (!TryResolveNodeDrop(atPosition, data, out var node, out var newParent, out var reason))
		{
			LastReparentFeedback = reason;
			return false;
		}
		bool flag = CanReparentNode(node, newParent, out reason);
		LastReparentFeedback = reason;
		if (flag)
		{
			DropModeFlags = 1;
		}
		return flag;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		DropModeFlags = 0;
		if (!TryResolveNodeDrop(atPosition, data, out var node, out var newParent, out var reason) || !CanReparentNode(node, newParent, out reason))
		{
			LastReparentFeedback = reason;
			XWEditorInterface.Instance?.ShowToast(reason, 2);
			return;
		}
		if (!ReparentNodeWithHistory(node, newParent))
		{
			XWEditorInterface.Instance?.ShowToast(LastReparentFeedback, 2);
			return;
		}
		if (_nodeToItem.TryGetValue(newParent, out var value))
		{
			value.Collapsed = false;
		}
		XWEditorInterface.Instance?.ShowToast(LastReparentFeedback);
	}

	private bool TryResolveNodeDrop(Vector2 atPosition, Variant data, out Node node, out Node newParent, out string reason)
	{
		node = null;
		newParent = null;
		reason = "这里只接受当前 2D 场景树中的节点。";
		if (data.VariantType != Variant.Type.Dictionary)
		{
			return false;
		}
		Dictionary dictionary = data.AsGodotDictionary();
		if (!dictionary.TryGetValue("type", out var value) || !string.Equals(value.AsString(), "xw_2d_scene_node", StringComparison.Ordinal) || !dictionary.TryGetValue("node", out var value2))
		{
			return false;
		}
		node = value2.As<Node>();
		TreeItem itemAtPosition = GetItemAtPosition(atPosition);
		if (itemAtPosition == null || !_itemToNode.TryGetValue(itemAtPosition, out newParent))
		{
			reason = "请把节点拖到另一个场景节点上。";
			return false;
		}
		if (!dictionary.TryGetValue("scene_root_id", out var value3) || !GodotObject.IsInstanceValid(_editedSceneRoot) || Math.Max(0L, value3.AsInt64()) != (long)_editedSceneRoot.GetInstanceId())
		{
			reason = "不能把其他场景标签中的节点拖入当前场景。";
			return false;
		}
		return true;
	}

	private bool CanBeginNodeDrag(Node node, out string reason)
	{
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			reason = "拖动节点已经失效。";
			return false;
		}
		if (node == _editedSceneRoot)
		{
			reason = "场景根节点不能通过拖拽重挂。";
			return false;
		}
		if (!IsNodeInEditedScene(node))
		{
			reason = "只能拖动当前编辑场景中的节点。";
			return false;
		}
		if (IsNodeLocked(node))
		{
			reason = $"节点“{node.Name}”已锁定，不能重挂。";
			return false;
		}
		reason = $"可以拖动节点“{node.Name}”。";
		return true;
	}

	public bool ToggleNodeVisibilityWithHistory(Node node)
	{
		if (!(node is CanvasItem canvasItem))
		{
			return false;
		}
		return SetNodeVisibilityWithHistory(node, !canvasItem.Visible);
	}

	public bool SetNodeVisibilityWithHistory(Node node, bool visible)
	{
		if (!(node is CanvasItem canvasItem) || canvasItem.Visible == visible)
		{
			return false;
		}
		PrepareSceneHistoryMutation();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			xWUndoRedoManager.CreateAction(visible ? "显示 2D 节点" : "隐藏 2D 节点", mergeMode: false, CurrentSceneHistoryId());
			xWUndoRedoManager.AddDoMethod(this, "ApplyNodeVisibility", Variant.From(in node), Variant.From(in visible));
			xWUndoRedoManager.AddUndoMethod(this, "ApplyNodeVisibility", Variant.From(in node), Variant.From<bool>(!visible));
			xWUndoRedoManager.CommitAction();
		}
		else
		{
			ApplyNodeVisibility(node, visible);
		}
		return true;
	}

	public void ApplyNodeVisibility(Node node, bool visible)
	{
		if (node is CanvasItem canvasItem && GodotObject.IsInstanceValid(node))
		{
			canvasItem.Visible = visible;
			SyncHistorySelection(node);
		}
	}

	public bool ToggleNodeLockWithHistory(Node node)
	{
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			return false;
		}
		return SetNodeLockWithHistory(node, !IsNodeLocked(node));
	}

	public bool SetNodeLockWithHistory(Node node, bool locked)
	{
		if (node == null || !GodotObject.IsInstanceValid(node) || IsNodeLocked(node) == locked)
		{
			return false;
		}
		PrepareSceneHistoryMutation();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			xWUndoRedoManager.CreateAction(locked ? "锁定 2D 节点" : "解锁 2D 节点", mergeMode: false, CurrentSceneHistoryId());
			xWUndoRedoManager.AddDoMethod(this, "ApplyNodeLock", Variant.From(in node), Variant.From(in locked));
			xWUndoRedoManager.AddUndoMethod(this, "ApplyNodeLock", Variant.From(in node), Variant.From<bool>(!locked));
			xWUndoRedoManager.CommitAction();
		}
		else
		{
			ApplyNodeLock(node, locked);
		}
		return true;
	}

	public void ApplyNodeLock(Node node, bool locked)
	{
		if (node != null && GodotObject.IsInstanceValid(node))
		{
			ApplyNodeLockStorage(node, locked);
			SyncHistorySelection(node);
		}
	}

	public void ApplyNodeLockStorage(Node node, bool locked)
	{
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (locked)
		{
			node.SetMeta("_edit_lock_", true);
			_lockedNodes.Add(node);
			return;
		}
		if (node.HasMeta("_edit_lock_"))
		{
			node.RemoveMeta("_edit_lock_");
		}
		_lockedNodes.Remove(node);
	}

	public bool SetNodeScriptWithHistory(Node node, Script script, string actionName)
	{
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			return false;
		}
		Variant script2 = node.GetScript();
		Variant variant = (GodotObject.IsInstanceValid(script) ? Variant.From(in script) : default(Variant));
		GodotObject godotObject = script2.AsGodotObject();
		if ((script2.VariantType == Variant.Type.Nil && variant.VariantType == Variant.Type.Nil) || (GodotObject.IsInstanceValid(godotObject) && godotObject == script))
		{
			return false;
		}
		PrepareSceneHistoryMutation();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			string name;
			if (string.IsNullOrWhiteSpace(actionName))
			{
				name = (GodotObject.IsInstanceValid(script) ? "挂载 2D 节点脚本" : "分离 2D 节点脚本");
			}
			else
			{
				name = actionName;
			}
			xWUndoRedoManager.CreateAction(name, mergeMode: false, CurrentSceneHistoryId());
			xWUndoRedoManager.AddDoMethod(this, "ApplyNodeScript", Variant.From(in node), variant);
			xWUndoRedoManager.AddUndoMethod(this, "ApplyNodeScript", Variant.From(in node), script2);
			xWUndoRedoManager.CommitAction();
		}
		else
		{
			ApplyNodeScript(node, variant);
		}
		return true;
	}

	public void ApplyNodeScript(Node node, Variant script)
	{
		if (node != null && GodotObject.IsInstanceValid(node))
		{
			node.SetScript(script);
			SyncHistorySelection(node);
		}
	}

	public Node GetSelectedNode()
	{
		TreeItem selected = GetSelected();
		if (selected == null || !_itemToNode.TryGetValue(selected, out var value))
		{
			return null;
		}
		return value;
	}

	public TreeItem GetSelectedItem()
	{
		return GetSelected();
	}

	public bool IsNodeLocked(Node node)
	{
		if (node != null && GodotObject.IsInstanceValid(node))
		{
			if (!node.HasMeta("_edit_lock_"))
			{
				return _lockedNodes.Contains(node);
			}
			return true;
		}
		return false;
	}

	public void SelectNode(Node node, bool emitSignal = true)
	{
		if (node != null && _nodeToItem.TryGetValue(node, out var value))
		{
			_suppressItemSelected = true;
			try
			{
				value.Select(0);
			}
			finally
			{
				_suppressItemSelected = false;
			}
			if (emitSignal)
			{
				EmitSignal(SignalName.NodeSelected, node);
			}
		}
	}

	public void SelectRoot()
	{
		SelectNode(_editedSceneRoot);
	}

	public Node AddChildNode(Node newNode)
	{
		Node parent = GetSelectedNode() ?? _editedSceneRoot;
		return AddChildNodeTo(parent, newNode);
	}

	public Node AddChildNodeTo(Node parent, Node newNode)
	{
		if (parent == null || newNode == null)
		{
			return null;
		}
		parent.AddChild(newNode, forceReadableName: false, InternalMode.Disabled);
		AssignOwnerForAddedSubtree(newNode);
		RefreshAndReselect(newNode);
		return newNode;
	}

	public Node AddChildNodeWithHistory(Node newNode, string actionName = "添加 2D 节点")
	{
		Node parent = GetSelectedNode() ?? _editedSceneRoot;
		return AddChildNodeToWithHistory(parent, newNode, actionName);
	}

	public Node AddChildNodeToWithHistory(Node parent, Node newNode, string actionName = "添加 2D 节点")
	{
		if (parent == null || !GodotObject.IsInstanceValid(parent) || newNode == null || !GodotObject.IsInstanceValid(newNode))
		{
			return null;
		}
		PrepareSceneHistoryMutation();
		int num = CurrentSceneHistoryId();
		long from = _nextStructuralNodeRecordId++;
		_structuralNodeRecords[from] = new StructuralNodeRecord
		{
			HistoryId = num,
			Parent = parent,
			Node = newNode,
			AttachedSelection = newNode,
			DetachedSelection = parent,
			Index = parent.GetChildCount(),
			Applied = false
		};
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			string name = (string.IsNullOrWhiteSpace(actionName) ? "添加 2D 节点" : actionName);
			xWUndoRedoManager.CreateAction(name, mergeMode: false, num);
			xWUndoRedoManager.AddDoMethod(this, "AttachStructuralNode", Variant.From(in from));
			xWUndoRedoManager.AddUndoMethod(this, "DetachStructuralNode", Variant.From(in from));
			xWUndoRedoManager.CommitAction();
		}
		else
		{
			AttachStructuralNode(from);
		}
		return newNode;
	}

	public bool RemoveSelectedNode()
	{
		return RemoveNodeWithHistory(GetSelectedNode());
	}

	public bool RemoveNodeWithHistory(Node node, string actionName = "删除 2D 节点")
	{
		if (node == null || node == _editedSceneRoot || IsNodeLocked(node))
		{
			return false;
		}
		Node parent = node.GetParent();
		if (parent == null || !GodotObject.IsInstanceValid(parent))
		{
			return false;
		}
		PrepareSceneHistoryMutation();
		int num = CurrentSceneHistoryId();
		long from = _nextStructuralNodeRecordId++;
		_structuralNodeRecords[from] = new StructuralNodeRecord
		{
			HistoryId = num,
			Parent = parent,
			Node = node,
			AttachedSelection = node,
			DetachedSelection = parent,
			Index = node.GetIndex(),
			Applied = true
		};
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			string name = (string.IsNullOrWhiteSpace(actionName) ? "删除 2D 节点" : actionName);
			xWUndoRedoManager.CreateAction(name, mergeMode: false, num);
			xWUndoRedoManager.AddDoMethod(this, "DetachStructuralNode", Variant.From(in from));
			xWUndoRedoManager.AddUndoMethod(this, "AttachStructuralNode", Variant.From(in from));
			xWUndoRedoManager.CommitAction();
		}
		else
		{
			DetachStructuralNode(from);
		}
		return true;
	}

	public Node DuplicateSelectedNode()
	{
		Node selectedNode = GetSelectedNode();
		if (selectedNode == null || selectedNode == _editedSceneRoot || IsNodeLocked(selectedNode))
		{
			return null;
		}
		Node node = selectedNode.Duplicate();
		if (node == null)
		{
			return null;
		}
		Node parent = selectedNode.GetParent();
		if (parent == null || !GodotObject.IsInstanceValid(parent))
		{
			node.Free();
			return null;
		}
		node.Name = BuildUniqueSiblingName(parent, string.Concat(selectedNode.Name, "2"));
		PrepareSceneHistoryMutation();
		int num = CurrentSceneHistoryId();
		long from = _nextStructuralNodeRecordId++;
		_structuralNodeRecords[from] = new StructuralNodeRecord
		{
			HistoryId = num,
			Parent = parent,
			Node = node,
			AttachedSelection = node,
			DetachedSelection = selectedNode,
			Index = selectedNode.GetIndex() + 1,
			Applied = false
		};
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			xWUndoRedoManager.CreateAction("复制 2D 节点", mergeMode: false, num);
			xWUndoRedoManager.AddDoMethod(this, "AttachStructuralNode", Variant.From(in from));
			xWUndoRedoManager.AddUndoMethod(this, "DetachStructuralNode", Variant.From(in from));
			xWUndoRedoManager.CommitAction();
		}
		else
		{
			AttachStructuralNode(from);
		}
		return node;
	}

	public bool RenameSelectedNode(string newName)
	{
		return RenameNode(GetSelectedNode(), newName);
	}

	public bool MoveSelectedNode(int offset)
	{
		Node from = GetSelectedNode();
		Node node = from?.GetParent();
		if (from == null || node == null || from == _editedSceneRoot || IsNodeLocked(from))
		{
			return false;
		}
		int from2 = from.GetIndex();
		int from3 = Mathf.Clamp(from2 + offset, 0, node.GetChildCount() - 1);
		if (from3 == from2)
		{
			return false;
		}
		PrepareSceneHistoryMutation();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			xWUndoRedoManager.CreateAction("调整 2D 节点顺序", mergeMode: false, CurrentSceneHistoryId());
			xWUndoRedoManager.AddDoMethod(this, "ApplyNodeIndex", Variant.From(in from), Variant.From(in from3));
			xWUndoRedoManager.AddUndoMethod(this, "ApplyNodeIndex", Variant.From(in from), Variant.From(in from2));
			xWUndoRedoManager.CommitAction();
		}
		else
		{
			ApplyNodeIndex(from, from3);
		}
		return true;
	}

	public bool CanReparentNode(Node node, Node newParent, out string reason)
	{
		if (!CanBeginNodeDrag(node, out reason))
		{
			return false;
		}
		if (newParent == null || !GodotObject.IsInstanceValid(newParent))
		{
			reason = "重挂目标节点已经失效。";
			return false;
		}
		if (!IsNodeInEditedScene(newParent))
		{
			reason = "不能把节点重挂到其他场景标签。";
			return false;
		}
		if (node == newParent)
		{
			reason = "节点不能成为自己的子节点。";
			return false;
		}
		if (node.IsAncestorOf(newParent))
		{
			reason = "不能把节点重挂到自己的后代中，这会形成循环。";
			return false;
		}
		if (node.GetParent() == newParent)
		{
			reason = $"节点“{node.Name}”已经是“{newParent.Name}”的子节点。";
			return false;
		}
		if (IsNodeLocked(newParent))
		{
			reason = $"目标节点“{newParent.Name}”已锁定，不能接收子节点。";
			return false;
		}
		reason = $"可将“{node.Name}”重挂为“{newParent.Name}”的子节点。";
		return true;
	}

	public bool ReparentSelectedNodeWithHistory(Node newParent)
	{
		return ReparentNodeWithHistory(GetSelectedNode(), newParent);
	}

	public bool ReparentNodeWithHistory(Node node, Node newParent, string actionName = "拖拽重挂 2D 节点")
	{
		if (!CanReparentNode(node, newParent, out var reason))
		{
			LastReparentFeedback = reason;
			return false;
		}
		PrepareSceneHistoryMutation();
		int num = CurrentSceneHistoryId();
		long from = _nextReparentNodeRecordId++;
		ReparentNodeRecord reparentNodeRecord = new ReparentNodeRecord
		{
			HistoryId = num,
			Node = node,
			OldParent = node.GetParent(),
			NewParent = newParent,
			OldIndex = node.GetIndex(),
			NewIndex = newParent.GetChildCount(),
			OldName = node.Name,
			NewName = (HasChildNamed(newParent, node.Name) ? BuildUniqueSiblingName(newParent, node.Name) : node.Name),
			CanvasTransform = CaptureCanvasTransform(node)
		};
		CaptureOwnerSnapshot(node, reparentNodeRecord.OldOwners);
		_reparentNodeRecords[from] = reparentNodeRecord;
		LastReparentHistoryId = num;
		LastReparentPreservedGlobalTransform = true;
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			string name = (string.IsNullOrWhiteSpace(actionName) ? "拖拽重挂 2D 节点" : actionName);
			xWUndoRedoManager.CreateAction(name, mergeMode: false, num);
			xWUndoRedoManager.AddDoMethod(this, "ApplyReparentNodeRecord", Variant.From(in from), Variant.From<bool>(true));
			xWUndoRedoManager.AddUndoMethod(this, "ApplyReparentNodeRecord", Variant.From(in from), Variant.From<bool>(false));
			xWUndoRedoManager.CommitAction();
		}
		else
		{
			ApplyReparentNodeRecord(from, useNewParent: true);
		}
		bool flag = node.GetParent() == newParent;
		LastReparentFeedback = (flag ? $"已将“{node.Name}”重挂为“{newParent.Name}”的子节点，可撤销。" : $"节点“{node.Name}”重挂失败。");
		return flag;
	}

	public void ApplyReparentNodeRecord(long recordId, bool useNewParent)
	{
		if (!_reparentNodeRecords.TryGetValue(recordId, out var value) || !GodotObject.IsInstanceValid(value.Node) || !GodotObject.IsInstanceValid(value.OldParent) || !GodotObject.IsInstanceValid(value.NewParent))
		{
			return;
		}
		Node node = (useNewParent ? value.NewParent : value.OldParent);
		int value2 = (useNewParent ? value.NewIndex : value.OldIndex);
		StringName name = (useNewParent ? value.NewName : value.OldName);
		if (value.Node == node || value.Node.IsAncestorOf(node))
		{
			return;
		}
		value.Node.Name = name;
		value.Node.Reparent(node);
		node.MoveChild(value.Node, Mathf.Clamp(value2, 0, node.GetChildCount() - 1));
		RestoreCanvasTransform(value.CanvasTransform);
		value.Node.ResetPhysicsInterpolation();
		if (useNewParent)
		{
			if (!value.NewOwnersCaptured)
			{
				RepairOwnersAfterReparent(value.Node);
				CaptureOwnerSnapshot(value.Node, value.NewOwners);
				value.NewOwnersCaptured = true;
			}
			else
			{
				RestoreOwnerSnapshot(value.NewOwners);
			}
		}
		else
		{
			RestoreOwnerSnapshot(value.OldOwners);
		}
		value.AppliedToNewParent = useNewParent;
		UpdateRecordRedoBranch(value);
		LastReparentHistoryId = value.HistoryId;
		LastReparentPreservedGlobalTransform = IsCanvasTransformPreserved(value.CanvasTransform);
		LastReparentFeedback = (useNewParent ? $"已将“{value.Node.Name}”重挂到“{value.NewParent.Name}”。" : $"已撤销重挂，“{value.Node.Name}”已回到“{value.OldParent.Name}”。");
		SyncHistorySelection(value.Node);
	}

	private bool RenameNode(Node node, string newName)
	{
		string from = newName?.Trim() ?? "";
		if (node == null || !GodotObject.IsInstanceValid(node) || IsNodeLocked(node) || string.IsNullOrWhiteSpace(from))
		{
			return false;
		}
		string from2 = node.Name.ToString();
		if (from2 == from)
		{
			return true;
		}
		if (!IsSiblingNameAvailable(node, from))
		{
			return false;
		}
		PrepareSceneHistoryMutation();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			xWUndoRedoManager.CreateAction("重命名 2D 节点", mergeMode: false, CurrentSceneHistoryId());
			xWUndoRedoManager.AddDoMethod(this, "ApplyNodeName", Variant.From(in node), Variant.From(in from));
			xWUndoRedoManager.AddUndoMethod(this, "ApplyNodeName", Variant.From(in node), Variant.From(in from2));
			xWUndoRedoManager.CommitAction();
		}
		else
		{
			ApplyNodeName(node, from);
		}
		return true;
	}

	public void ApplyNodeName(Node node, string name)
	{
		if (node != null && GodotObject.IsInstanceValid(node) && !string.IsNullOrWhiteSpace(name))
		{
			node.Name = name;
			SyncHistorySelection(node);
			EmitSignal(SignalName.NodeRenamed, node, node.Name.ToString());
		}
	}

	public void ApplyNodeIndex(Node node, int index)
	{
		Node node2 = node?.GetParent();
		if (node != null && GodotObject.IsInstanceValid(node) && node2 != null && GodotObject.IsInstanceValid(node2))
		{
			node2.MoveChild(node, Mathf.Clamp(index, 0, node2.GetChildCount() - 1));
			SyncHistorySelection(node);
		}
	}

	public void AttachStructuralNode(long recordId)
	{
		if (_structuralNodeRecords.TryGetValue(recordId, out var value) && value.Parent != null && GodotObject.IsInstanceValid(value.Parent) && value.Node != null && GodotObject.IsInstanceValid(value.Node))
		{
			value.Node.GetParent()?.RemoveChild(value.Node);
			value.Parent.AddChild(value.Node, forceReadableName: true, InternalMode.Disabled);
			value.Parent.MoveChild(value.Node, Mathf.Clamp(value.Index, 0, value.Parent.GetChildCount() - 1));
			AssignOwnerForAddedSubtree(value.Node);
			value.Applied = true;
			UpdateRecordRedoBranch(value);
			SyncHistorySelection(value.AttachedSelection);
		}
	}

	public void DetachStructuralNode(long recordId)
	{
		if (_structuralNodeRecords.TryGetValue(recordId, out var value) && value.Node != null && GodotObject.IsInstanceValid(value.Node))
		{
			value.Node.GetParent()?.RemoveChild(value.Node);
			value.Applied = false;
			UpdateRecordRedoBranch(value);
			SyncHistorySelection(value.DetachedSelection);
		}
	}

	public void PrepareForSceneHistoryBranchChange()
	{
		ReleaseStructuralNodeRecordsIfHistoryWasCleared();
		ReleaseSceneRootRecordsIfHistoryWasCleared();
		ReleaseReparentNodeRecordsIfHistoryWasCleared();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		int num = CurrentSceneHistoryId();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager) && xWUndoRedoManager.HasRedo(num))
		{
			ReleaseStructuralNodeRecords(releaseAll: false, num);
			ReleaseSceneRootRecords(releaseAll: false, num);
			ReleaseReparentNodeRecords(releaseAll: false, num);
		}
	}

	public void ReleaseHistoryResources(int historyId)
	{
		ReleaseStructuralNodeRecords(releaseAll: true, historyId);
		ReleaseSceneRootRecords(releaseAll: true, historyId);
		ReleaseReparentNodeRecords(releaseAll: true, historyId);
	}

	public int GetTrackedStructuralNodeRecordCount(int historyId = -1)
	{
		if (historyId < 0)
		{
			return _structuralNodeRecords.Count;
		}
		int num = 0;
		foreach (StructuralNodeRecord value in _structuralNodeRecords.Values)
		{
			if (value.HistoryId == historyId)
			{
				num++;
			}
		}
		return num;
	}

	public int GetTrackedSceneRootRecordCount(int historyId = -1)
	{
		if (historyId < 0)
		{
			return _sceneRootRecords.Count;
		}
		int num = 0;
		foreach (SceneRootRecord value in _sceneRootRecords.Values)
		{
			if (value.HistoryId == historyId)
			{
				num++;
			}
		}
		return num;
	}

	public int GetTrackedReparentNodeRecordCount(int historyId = -1)
	{
		if (historyId < 0)
		{
			return _reparentNodeRecords.Count;
		}
		int num = 0;
		foreach (ReparentNodeRecord value in _reparentNodeRecords.Values)
		{
			if (value.HistoryId == historyId)
			{
				num++;
			}
		}
		return num;
	}

	public bool MakeSelectedNodeRoot()
	{
		Node selectedNode = GetSelectedNode();
		if (selectedNode == null || selectedNode == _editedSceneRoot || IsNodeLocked(selectedNode))
		{
			return false;
		}
		Node parent = selectedNode.GetParent();
		if (parent == null || !GodotObject.IsInstanceValid(parent))
		{
			return false;
		}
		PrepareSceneHistoryMutation();
		int num = CurrentSceneHistoryId();
		long from = _nextSceneRootRecordId++;
		SceneRootRecord sceneRootRecord = new SceneRootRecord
		{
			HistoryId = num,
			OldRoot = _editedSceneRoot,
			NewRoot = selectedNode,
			OldParent = parent,
			OldIndex = selectedNode.GetIndex(),
			NewRootActive = false
		};
		CollectNodesOwnedBy(selectedNode, _editedSceneRoot, sceneRootRecord.RemappedOwnerNodes);
		_sceneRootRecords[from] = sceneRootRecord;
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			xWUndoRedoManager.CreateAction("设置 2D 场景根节点", mergeMode: false, num);
			xWUndoRedoManager.AddDoMethod(this, "ApplySceneRootRecord", Variant.From(in from), Variant.From<bool>(true));
			xWUndoRedoManager.AddUndoMethod(this, "ApplySceneRootRecord", Variant.From(in from), Variant.From<bool>(false));
			xWUndoRedoManager.CommitAction();
		}
		else
		{
			ApplySceneRootRecord(from, useNewRoot: true);
		}
		return true;
	}

	public void ApplySceneRootRecord(long recordId, bool useNewRoot)
	{
		if (_sceneRootRecords.TryGetValue(recordId, out var value) && value.OldRoot != null && GodotObject.IsInstanceValid(value.OldRoot) && value.NewRoot != null && GodotObject.IsInstanceValid(value.NewRoot) && value.OldParent != null && GodotObject.IsInstanceValid(value.OldParent))
		{
			XW2DSceneEditor xW2DSceneEditor = XWEditorInterface.Instance?.Get2DSceneEditor();
			xW2DSceneEditor?.DetachCurrentSceneRootForHistory();
			if (useNewRoot)
			{
				value.NewRoot.GetParent()?.RemoveChild(value.NewRoot);
				value.NewRoot.Owner = null;
				ApplyOwnerMapping(value.RemappedOwnerNodes, value.NewRoot);
				_editedSceneRoot = value.NewRoot;
			}
			else
			{
				value.NewRoot.GetParent()?.RemoveChild(value.NewRoot);
				value.OldParent.AddChild(value.NewRoot, forceReadableName: true, InternalMode.Disabled);
				value.OldParent.MoveChild(value.NewRoot, Mathf.Clamp(value.OldIndex, 0, value.OldParent.GetChildCount() - 1));
				_editedSceneRoot = value.OldRoot;
				value.NewRoot.Owner = value.OldRoot;
				ApplyOwnerMapping(value.RemappedOwnerNodes, value.OldRoot);
				value.OldRoot.Owner = null;
			}
			value.NewRootActive = useNewRoot;
			UpdateRecordRedoBranch(value);
			if (GodotObject.IsInstanceValid(xW2DSceneEditor))
			{
				xW2DSceneEditor.ApplySceneRootFromHistory(_editedSceneRoot);
			}
			else
			{
				XWEditorInterface.Instance?.SetEditedSceneRoot(_editedSceneRoot);
			}
			SyncHistorySelection(_editedSceneRoot);
		}
	}

	public TreeItem FindItemForNode(Node node)
	{
		if (!_nodeToItem.TryGetValue(node, out var value))
		{
			return null;
		}
		return value;
	}

	private void PrepareSceneHistoryMutation()
	{
		PrepareForSceneHistoryBranchChange();
		XWEditorInterface.Instance?.Get2DSceneEditor()?.PrepareForSceneHistoryBranchChange();
	}

	private static int CurrentSceneHistoryId()
	{
		return XWEditorInterface.Instance?.GetCurrentSceneHistoryId() ?? 1;
	}

	private void SyncHistorySelection(Node node)
	{
		Refresh();
		if (node != null && GodotObject.IsInstanceValid(node))
		{
			SelectNode(node, emitSignal: false);
			XWEditorInterface.Instance?.SetSelectedNode(node);
		}
		else
		{
			XWEditorInterface.Instance?.ClearSelection();
		}
		EmitSignal(SignalName.HistorySelectionChanged, node);
		XW2DSceneEditor xW2DSceneEditor = XWEditorInterface.Instance?.Get2DSceneEditor();
		xW2DSceneEditor?.RefreshDirectTransformSurface();
		xW2DSceneEditor?.QueueViewportRedraw();
	}

	private static void UpdateRecordRedoBranch(StructuralNodeRecord record)
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		record.InRedoBranch = GodotObject.IsInstanceValid(xWUndoRedoManager) && xWUndoRedoManager.IsUndoing();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager) && xWUndoRedoManager.IsRedoing())
		{
			record.InRedoBranch = false;
		}
	}

	private static void UpdateRecordRedoBranch(SceneRootRecord record)
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		record.InRedoBranch = GodotObject.IsInstanceValid(xWUndoRedoManager) && xWUndoRedoManager.IsUndoing();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager) && xWUndoRedoManager.IsRedoing())
		{
			record.InRedoBranch = false;
		}
	}

	private static void UpdateRecordRedoBranch(ReparentNodeRecord record)
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		record.InRedoBranch = GodotObject.IsInstanceValid(xWUndoRedoManager) && xWUndoRedoManager.IsUndoing();
		if (GodotObject.IsInstanceValid(xWUndoRedoManager) && xWUndoRedoManager.IsRedoing())
		{
			record.InRedoBranch = false;
		}
	}

	private void ReleaseStructuralNodeRecordsIfHistoryWasCleared()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (!GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			return;
		}
		HashSet<int> hashSet = new HashSet<int>();
		foreach (StructuralNodeRecord value in _structuralNodeRecords.Values)
		{
			if (!xWUndoRedoManager.HasHistory(value.HistoryId))
			{
				hashSet.Add(value.HistoryId);
			}
		}
		foreach (int item in hashSet)
		{
			ReleaseStructuralNodeRecords(releaseAll: true, item);
		}
	}

	private void ReleaseSceneRootRecordsIfHistoryWasCleared()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (!GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			return;
		}
		HashSet<int> hashSet = new HashSet<int>();
		foreach (SceneRootRecord value in _sceneRootRecords.Values)
		{
			if (!xWUndoRedoManager.HasHistory(value.HistoryId))
			{
				hashSet.Add(value.HistoryId);
			}
		}
		foreach (int item in hashSet)
		{
			ReleaseSceneRootRecords(releaseAll: true, item);
		}
	}

	private void ReleaseReparentNodeRecordsIfHistoryWasCleared()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (!GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			return;
		}
		HashSet<int> hashSet = new HashSet<int>();
		foreach (ReparentNodeRecord value in _reparentNodeRecords.Values)
		{
			if (!xWUndoRedoManager.HasHistory(value.HistoryId))
			{
				hashSet.Add(value.HistoryId);
			}
		}
		foreach (int item in hashSet)
		{
			ReleaseReparentNodeRecords(releaseAll: true, item);
		}
	}

	private void ReleaseStructuralNodeRecords(bool releaseAll, int historyId = -1)
	{
		List<long> list = new List<long>();
		foreach (var (item, structuralNodeRecord2) in _structuralNodeRecords)
		{
			if ((historyId < 0 || structuralNodeRecord2.HistoryId == historyId) && (releaseAll || structuralNodeRecord2.InRedoBranch))
			{
				if (structuralNodeRecord2.Node != null && GodotObject.IsInstanceValid(structuralNodeRecord2.Node) && structuralNodeRecord2.Node.GetParent() == null)
				{
					structuralNodeRecord2.Node.Free();
				}
				list.Add(item);
			}
		}
		foreach (long item2 in list)
		{
			_structuralNodeRecords.Remove(item2);
		}
	}

	private void ReleaseSceneRootRecords(bool releaseAll, int historyId = -1)
	{
		List<long> list = new List<long>();
		foreach (var (item, sceneRootRecord2) in _sceneRootRecords)
		{
			if ((historyId < 0 || sceneRootRecord2.HistoryId == historyId) && (releaseAll || sceneRootRecord2.InRedoBranch))
			{
				if (sceneRootRecord2.NewRootActive && sceneRootRecord2.OldRoot != null && GodotObject.IsInstanceValid(sceneRootRecord2.OldRoot) && sceneRootRecord2.OldRoot.GetParent() == null && sceneRootRecord2.OldRoot != _editedSceneRoot)
				{
					sceneRootRecord2.OldRoot.Free();
				}
				list.Add(item);
			}
		}
		foreach (long item2 in list)
		{
			_sceneRootRecords.Remove(item2);
		}
	}

	private void ReleaseReparentNodeRecords(bool releaseAll, int historyId = -1)
	{
		List<long> list = new List<long>();
		foreach (var (item, reparentNodeRecord2) in _reparentNodeRecords)
		{
			if ((historyId < 0 || reparentNodeRecord2.HistoryId == historyId) && (releaseAll || reparentNodeRecord2.InRedoBranch))
			{
				list.Add(item);
			}
		}
		foreach (long item2 in list)
		{
			_reparentNodeRecords.Remove(item2);
		}
	}

	private bool IsNodeInEditedScene(Node node)
	{
		if (node == null || !GodotObject.IsInstanceValid(node) || _editedSceneRoot == null || !GodotObject.IsInstanceValid(_editedSceneRoot))
		{
			return false;
		}
		Node node2 = node;
		while (GodotObject.IsInstanceValid(node2))
		{
			if (node2 == _editedSceneRoot)
			{
				return true;
			}
			node2 = node2.GetParent();
		}
		return false;
	}

	private static CanvasTransformSnapshot CaptureCanvasTransform(Node node)
	{
		if (!(node is Node2D node2))
		{
			if (node is Control control)
			{
				return new CanvasTransformSnapshot(control);
			}
			return default;
		}
		return new CanvasTransformSnapshot(node2);
	}

	private static void RestoreCanvasTransform(CanvasTransformSnapshot snapshot)
	{
		if (GodotObject.IsInstanceValid(snapshot.Node2D))
		{
			snapshot.Node2D.GlobalTransform = snapshot.GlobalTransform;
		}
		else if (GodotObject.IsInstanceValid(snapshot.Control))
		{
			Control control = snapshot.Control;
			Transform2D transform2D = Transform2D.Identity;
			if (!control.TopLevel && control.GetParent() is CanvasItem canvasItem)
			{
				transform2D = canvasItem.GetGlobalTransform();
			}
			Transform2D transform2D2 = transform2D.AffineInverse() * snapshot.GlobalTransform;
			control.Size = snapshot.ControlSize;
			control.Rotation = transform2D2.Rotation;
			control.Scale = transform2D2.Scale;
			control.GlobalPosition = snapshot.ControlGlobalPosition;
		}
	}

	private static bool IsCanvasTransformPreserved(CanvasTransformSnapshot snapshot)
	{
		if (!snapshot.HasCanvasTransform)
		{
			return true;
		}
		Transform2D transform2D = (GodotObject.IsInstanceValid(snapshot.Node2D) ? snapshot.Node2D.GlobalTransform : snapshot.Control.GetGlobalTransform());
		if ((double)transform2D.Origin.DistanceTo(snapshot.GlobalTransform.Origin) <= 0.001 && (double)transform2D.X.DistanceTo(snapshot.GlobalTransform.X) <= 0.001)
		{
			return (double)transform2D.Y.DistanceTo(snapshot.GlobalTransform.Y) <= 0.001;
		}
		return false;
	}

	private static void CaptureOwnerSnapshot(Node node, System.Collections.Generic.Dictionary<Node, Node> snapshot)
	{
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			return;
		}
		snapshot[node] = node.Owner;
		foreach (Node child in node.GetChildren())
		{
			CaptureOwnerSnapshot(child, snapshot);
		}
	}

	private void RepairOwnersAfterReparent(Node node)
	{
		RepairOwnerRecursive(node, isMovedRoot: true);
	}

	private void RepairOwnerRecursive(Node node, bool isMovedRoot)
	{
		if (node == null || !GodotObject.IsInstanceValid(node) || node == _editedSceneRoot)
		{
			return;
		}
		Node owner = node.Owner;
		if (!GodotObject.IsInstanceValid(owner) || !owner.IsAncestorOf(node))
		{
			node.Owner = _editedSceneRoot;
		}
		if (!isMovedRoot && !string.IsNullOrWhiteSpace(node.SceneFilePath))
		{
			return;
		}
		foreach (Node child in node.GetChildren())
		{
			RepairOwnerRecursive(child, isMovedRoot: false);
		}
	}

	private static void RestoreOwnerSnapshot(System.Collections.Generic.Dictionary<Node, Node> snapshot)
	{
		foreach (var (node3, node4) in snapshot)
		{
			if (GodotObject.IsInstanceValid(node3))
			{
				node3.Owner = ((GodotObject.IsInstanceValid(node4) && node4.IsAncestorOf(node3)) ? node4 : null);
			}
		}
	}

	private static bool IsSiblingNameAvailable(Node node, string requestedName)
	{
		Node parent = node.GetParent();
		if (parent == null || !GodotObject.IsInstanceValid(parent))
		{
			return true;
		}
		foreach (Node child in parent.GetChildren())
		{
			if (child != node && child.Name.ToString().Equals(requestedName, StringComparison.Ordinal))
			{
				return false;
			}
		}
		return true;
	}

	private static StringName BuildUniqueSiblingName(Node parent, StringName preferredName)
	{
		string name = preferredName.ToString();
		int num = 2;
		while (HasChildNamed(parent, name))
		{
			name = $"{preferredName}{num++}";
		}
		return new StringName(name);
	}

	private static bool HasChildNamed(Node parent, string name)
	{
		foreach (Node child in parent.GetChildren())
		{
			if (child.Name.ToString().Equals(name, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	private void RefreshAndReselect(Node node)
	{
		Refresh();
		SelectNode(node);
	}

	private void AssignOwnerForAddedSubtree(Node node)
	{
		if (_editedSceneRoot != null && node != _editedSceneRoot && node.Owner == null)
		{
			node.Owner = _editedSceneRoot;
		}
		if (!string.IsNullOrWhiteSpace(node.SceneFilePath))
		{
			return;
		}
		foreach (Node child in node.GetChildren())
		{
			if (child != null)
			{
				Node node2 = child;
				if (node2.Owner == null)
				{
					AssignOwnerForAddedSubtree(node2);
				}
			}
		}
	}

	private static void CollectNodesOwnedBy(Node node, Node owner, List<Node> result)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child.Owner == owner)
			{
				result.Add(child);
			}
			CollectNodesOwnedBy(child, owner, result);
		}
	}

	private static void ApplyOwnerMapping(List<Node> nodes, Node owner)
	{
		foreach (Node node in nodes)
		{
			if (GodotObject.IsInstanceValid(node))
			{
				node.Owner = owner;
			}
		}
	}

	private void PruneInvalidLocks()
	{
		_lockedNodes.RemoveWhere((Node node) => node == null || !GodotObject.IsInstanceValid(node));
	}

	private static bool HasScript(Node node)
	{
		if (node != null)
		{
			return node.GetScript().VariantType != Variant.Type.Nil;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(78)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetEditedSceneRoot, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetSceneRoot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetSceneRootFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parentItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Object, "parentNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetupItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyFilter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filterText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildFilterTerms, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filterText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FilterRecursive, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "terms", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MatchesFilter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "terms", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NodeHasGroup, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "groupFilter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAllVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnItemEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnButtonClicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "mouseButtonIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GetDragData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._CanDropData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName._DropData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.ToggleNodeVisibilityWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetNodeVisibilityWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyNodeVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToggleNodeLockWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetNodeLockWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "locked", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyNodeLock, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "locked", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyNodeLockStorage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "locked", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetNodeScriptWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "script", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Script"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyNodeScript, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Nil, "script", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectedItem, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsNodeLocked, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SelectNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "emitSignal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectRoot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddChildNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "newNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddChildNodeTo, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "newNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddChildNodeWithHistory, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "newNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddChildNodeToWithHistory, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "newNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelectedNode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveNodeWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DuplicateSelectedNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenameSelectedNode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "newName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveSelectedNode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReparentSelectedNodeWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "newParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReparentNodeWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "newParent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyReparentNodeRecord, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "recordId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "useNewParent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenameNode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "newName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyNodeName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyNodeIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttachStructuralNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "recordId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DetachStructuralNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "recordId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareForSceneHistoryBranchChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseHistoryResources, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTrackedStructuralNodeRecordCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTrackedSceneRootRecordCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetTrackedReparentNodeRecordCount, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeSelectedNodeRoot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplySceneRootRecord, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "recordId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "useNewRoot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindItemForNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareSceneHistoryMutation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CurrentSceneHistoryId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.SyncHistorySelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseStructuralNodeRecordsIfHistoryWasCleared, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseSceneRootRecordsIfHistoryWasCleared, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseReparentNodeRecordsIfHistoryWasCleared, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReleaseStructuralNodeRecords, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "releaseAll", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "historyId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseSceneRootRecords, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "releaseAll", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "historyId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleaseReparentNodeRecords, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "releaseAll", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "historyId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsNodeInEditedScene, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RepairOwnersAfterReparent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RepairOwnerRecursive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "isMovedRoot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSiblingNameAvailable, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "requestedName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildUniqueSiblingName, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "preferredName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasChildNamed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAndReselect, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.AssignOwnerForAddedSubtree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.PruneInvalidLocks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasScript, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
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
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.GetEditedSceneRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(GetEditedSceneRoot());
			return true;
		}
		if (method == MethodName.SetSceneRoot && args.Count == 1)
		{
			SetSceneRoot(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSceneRootFromHistory && args.Count == 1)
		{
			SetSceneRootFromHistory(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildTree && args.Count == 2)
		{
			BuildTree(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetupItem && args.Count == 2)
		{
			SetupItem(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyFilter && args.Count == 1)
		{
			ApplyFilter(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildFilterTerms && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(BuildFilterTerms(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FilterRecursive && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(FilterRecursive(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.MatchesFilter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(MatchesFilter(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.NodeHasGroup && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(NodeHasGroup(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SetAllVisible && args.Count == 2)
		{
			SetAllVisible(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnItemSelected && args.Count == 0)
		{
			OnItemSelected();
			ret = default;
			return true;
		}
		if (method == MethodName.OnItemEdited && args.Count == 0)
		{
			OnItemEdited();
			ret = default;
			return true;
		}
		if (method == MethodName.OnButtonClicked && args.Count == 4)
		{
			OnButtonClicked(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<long>(in args[2]), VariantUtils.ConvertTo<long>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName._GetDragData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Variant>(_GetDragData(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName._CanDropData && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(_CanDropData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1])));
			return true;
		}
		if (method == MethodName._DropData && args.Count == 2)
		{
			_DropData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleNodeVisibilityWithHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ToggleNodeVisibilityWithHistory(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SetNodeVisibilityWithHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetNodeVisibilityWithHistory(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyNodeVisibility && args.Count == 2)
		{
			ApplyNodeVisibility(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleNodeLockWithHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ToggleNodeLockWithHistory(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SetNodeLockWithHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetNodeLockWithHistory(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyNodeLock && args.Count == 2)
		{
			ApplyNodeLock(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyNodeLockStorage && args.Count == 2)
		{
			ApplyNodeLockStorage(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetNodeScriptWithHistory && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(SetNodeScriptWithHistory(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Script>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.ApplyNodeScript && args.Count == 2)
		{
			ApplyNodeScript(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(GetSelectedNode());
			return true;
		}
		if (method == MethodName.GetSelectedItem && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<TreeItem>(GetSelectedItem());
			return true;
		}
		if (method == MethodName.IsNodeLocked && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNodeLocked(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectNode && args.Count == 2)
		{
			SelectNode(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectRoot && args.Count == 0)
		{
			SelectRoot();
			ret = default;
			return true;
		}
		if (method == MethodName.AddChildNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Node>(AddChildNode(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.AddChildNodeTo && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Node>(AddChildNodeTo(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.AddChildNodeWithHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Node>(AddChildNodeWithHistory(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AddChildNodeToWithHistory && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Node>(AddChildNodeToWithHistory(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.RemoveSelectedNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveSelectedNode());
			return true;
		}
		if (method == MethodName.RemoveNodeWithHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveNodeWithHistory(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.DuplicateSelectedNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(DuplicateSelectedNode());
			return true;
		}
		if (method == MethodName.RenameSelectedNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(RenameSelectedNode(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MoveSelectedNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(MoveSelectedNode(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ReparentSelectedNodeWithHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ReparentSelectedNodeWithHistory(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ReparentNodeWithHistory && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(ReparentNodeWithHistory(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.ApplyReparentNodeRecord && args.Count == 2)
		{
			ApplyReparentNodeRecord(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenameNode && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(RenameNode(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyNodeName && args.Count == 2)
		{
			ApplyNodeName(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyNodeIndex && args.Count == 2)
		{
			ApplyNodeIndex(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AttachStructuralNode && args.Count == 1)
		{
			AttachStructuralNode(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DetachStructuralNode && args.Count == 1)
		{
			DetachStructuralNode(VariantUtils.ConvertTo<long>(in args[0]));
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
		if (method == MethodName.GetTrackedStructuralNodeRecordCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetTrackedStructuralNodeRecordCount(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTrackedSceneRootRecordCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetTrackedSceneRootRecordCount(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTrackedReparentNodeRecordCount && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetTrackedReparentNodeRecordCount(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.MakeSelectedNodeRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(MakeSelectedNodeRoot());
			return true;
		}
		if (method == MethodName.ApplySceneRootRecord && args.Count == 2)
		{
			ApplySceneRootRecord(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindItemForNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<TreeItem>(FindItemForNode(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.PrepareSceneHistoryMutation && args.Count == 0)
		{
			PrepareSceneHistoryMutation();
			ret = default;
			return true;
		}
		if (method == MethodName.CurrentSceneHistoryId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CurrentSceneHistoryId());
			return true;
		}
		if (method == MethodName.SyncHistorySelection && args.Count == 1)
		{
			SyncHistorySelection(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseStructuralNodeRecordsIfHistoryWasCleared && args.Count == 0)
		{
			ReleaseStructuralNodeRecordsIfHistoryWasCleared();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseSceneRootRecordsIfHistoryWasCleared && args.Count == 0)
		{
			ReleaseSceneRootRecordsIfHistoryWasCleared();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseReparentNodeRecordsIfHistoryWasCleared && args.Count == 0)
		{
			ReleaseReparentNodeRecordsIfHistoryWasCleared();
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseStructuralNodeRecords && args.Count == 2)
		{
			ReleaseStructuralNodeRecords(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseSceneRootRecords && args.Count == 2)
		{
			ReleaseSceneRootRecords(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleaseReparentNodeRecords && args.Count == 2)
		{
			ReleaseReparentNodeRecords(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsNodeInEditedScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNodeInEditedScene(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.RepairOwnersAfterReparent && args.Count == 1)
		{
			RepairOwnersAfterReparent(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RepairOwnerRecursive && args.Count == 2)
		{
			RepairOwnerRecursive(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSiblingNameAvailable && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSiblingNameAvailable(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildUniqueSiblingName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StringName>(BuildUniqueSiblingName(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.HasChildNamed && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasChildNamed(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.RefreshAndReselect && args.Count == 1)
		{
			RefreshAndReselect(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AssignOwnerForAddedSubtree && args.Count == 1)
		{
			AssignOwnerForAddedSubtree(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PruneInvalidLocks && args.Count == 0)
		{
			PruneInvalidLocks();
			ret = default;
			return true;
		}
		if (method == MethodName.HasScript && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasScript(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BuildFilterTerms && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string[]>(BuildFilterTerms(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.MatchesFilter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(MatchesFilter(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.NodeHasGroup && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(NodeHasGroup(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.CurrentSceneHistoryId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(CurrentSceneHistoryId());
			return true;
		}
		if (method == MethodName.IsSiblingNameAvailable && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSiblingNameAvailable(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildUniqueSiblingName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StringName>(BuildUniqueSiblingName(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1])));
			return true;
		}
		if (method == MethodName.HasChildNamed && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasChildNamed(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.HasScript && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasScript(VariantUtils.ConvertTo<Node>(in args[0])));
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.GetEditedSceneRoot)
		{
			return true;
		}
		if (method == MethodName.SetSceneRoot)
		{
			return true;
		}
		if (method == MethodName.SetSceneRootFromHistory)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.BuildTree)
		{
			return true;
		}
		if (method == MethodName.SetupItem)
		{
			return true;
		}
		if (method == MethodName.ApplyFilter)
		{
			return true;
		}
		if (method == MethodName.BuildFilterTerms)
		{
			return true;
		}
		if (method == MethodName.FilterRecursive)
		{
			return true;
		}
		if (method == MethodName.MatchesFilter)
		{
			return true;
		}
		if (method == MethodName.NodeHasGroup)
		{
			return true;
		}
		if (method == MethodName.SetAllVisible)
		{
			return true;
		}
		if (method == MethodName.OnItemSelected)
		{
			return true;
		}
		if (method == MethodName.OnItemEdited)
		{
			return true;
		}
		if (method == MethodName.OnButtonClicked)
		{
			return true;
		}
		if (method == MethodName._GetDragData)
		{
			return true;
		}
		if (method == MethodName._CanDropData)
		{
			return true;
		}
		if (method == MethodName._DropData)
		{
			return true;
		}
		if (method == MethodName.ToggleNodeVisibilityWithHistory)
		{
			return true;
		}
		if (method == MethodName.SetNodeVisibilityWithHistory)
		{
			return true;
		}
		if (method == MethodName.ApplyNodeVisibility)
		{
			return true;
		}
		if (method == MethodName.ToggleNodeLockWithHistory)
		{
			return true;
		}
		if (method == MethodName.SetNodeLockWithHistory)
		{
			return true;
		}
		if (method == MethodName.ApplyNodeLock)
		{
			return true;
		}
		if (method == MethodName.ApplyNodeLockStorage)
		{
			return true;
		}
		if (method == MethodName.SetNodeScriptWithHistory)
		{
			return true;
		}
		if (method == MethodName.ApplyNodeScript)
		{
			return true;
		}
		if (method == MethodName.GetSelectedNode)
		{
			return true;
		}
		if (method == MethodName.GetSelectedItem)
		{
			return true;
		}
		if (method == MethodName.IsNodeLocked)
		{
			return true;
		}
		if (method == MethodName.SelectNode)
		{
			return true;
		}
		if (method == MethodName.SelectRoot)
		{
			return true;
		}
		if (method == MethodName.AddChildNode)
		{
			return true;
		}
		if (method == MethodName.AddChildNodeTo)
		{
			return true;
		}
		if (method == MethodName.AddChildNodeWithHistory)
		{
			return true;
		}
		if (method == MethodName.AddChildNodeToWithHistory)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedNode)
		{
			return true;
		}
		if (method == MethodName.RemoveNodeWithHistory)
		{
			return true;
		}
		if (method == MethodName.DuplicateSelectedNode)
		{
			return true;
		}
		if (method == MethodName.RenameSelectedNode)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedNode)
		{
			return true;
		}
		if (method == MethodName.ReparentSelectedNodeWithHistory)
		{
			return true;
		}
		if (method == MethodName.ReparentNodeWithHistory)
		{
			return true;
		}
		if (method == MethodName.ApplyReparentNodeRecord)
		{
			return true;
		}
		if (method == MethodName.RenameNode)
		{
			return true;
		}
		if (method == MethodName.ApplyNodeName)
		{
			return true;
		}
		if (method == MethodName.ApplyNodeIndex)
		{
			return true;
		}
		if (method == MethodName.AttachStructuralNode)
		{
			return true;
		}
		if (method == MethodName.DetachStructuralNode)
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
		if (method == MethodName.GetTrackedStructuralNodeRecordCount)
		{
			return true;
		}
		if (method == MethodName.GetTrackedSceneRootRecordCount)
		{
			return true;
		}
		if (method == MethodName.GetTrackedReparentNodeRecordCount)
		{
			return true;
		}
		if (method == MethodName.MakeSelectedNodeRoot)
		{
			return true;
		}
		if (method == MethodName.ApplySceneRootRecord)
		{
			return true;
		}
		if (method == MethodName.FindItemForNode)
		{
			return true;
		}
		if (method == MethodName.PrepareSceneHistoryMutation)
		{
			return true;
		}
		if (method == MethodName.CurrentSceneHistoryId)
		{
			return true;
		}
		if (method == MethodName.SyncHistorySelection)
		{
			return true;
		}
		if (method == MethodName.ReleaseStructuralNodeRecordsIfHistoryWasCleared)
		{
			return true;
		}
		if (method == MethodName.ReleaseSceneRootRecordsIfHistoryWasCleared)
		{
			return true;
		}
		if (method == MethodName.ReleaseReparentNodeRecordsIfHistoryWasCleared)
		{
			return true;
		}
		if (method == MethodName.ReleaseStructuralNodeRecords)
		{
			return true;
		}
		if (method == MethodName.ReleaseSceneRootRecords)
		{
			return true;
		}
		if (method == MethodName.ReleaseReparentNodeRecords)
		{
			return true;
		}
		if (method == MethodName.IsNodeInEditedScene)
		{
			return true;
		}
		if (method == MethodName.RepairOwnersAfterReparent)
		{
			return true;
		}
		if (method == MethodName.RepairOwnerRecursive)
		{
			return true;
		}
		if (method == MethodName.IsSiblingNameAvailable)
		{
			return true;
		}
		if (method == MethodName.BuildUniqueSiblingName)
		{
			return true;
		}
		if (method == MethodName.HasChildNamed)
		{
			return true;
		}
		if (method == MethodName.RefreshAndReselect)
		{
			return true;
		}
		if (method == MethodName.AssignOwnerForAddedSubtree)
		{
			return true;
		}
		if (method == MethodName.PruneInvalidLocks)
		{
			return true;
		}
		if (method == MethodName.HasScript)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.LastReparentFeedback)
		{
			LastReparentFeedback = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.LastReparentHistoryId)
		{
			LastReparentHistoryId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.LastReparentPreservedGlobalTransform)
		{
			LastReparentPreservedGlobalTransform = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._editedSceneRoot)
		{
			_editedSceneRoot = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._rootItem)
		{
			_rootItem = VariantUtils.ConvertTo<TreeItem>(in value);
			return true;
		}
		if (name == PropertyName._suppressItemSelected)
		{
			_suppressItemSelected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._nextStructuralNodeRecordId)
		{
			_nextStructuralNodeRecordId = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._nextSceneRootRecordId)
		{
			_nextSceneRootRecordId = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._nextReparentNodeRecordId)
		{
			_nextReparentNodeRecordId = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._filterText)
		{
			_filterText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._visibleIcon)
		{
			_visibleIcon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._hiddenIcon)
		{
			_hiddenIcon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._lockIcon)
		{
			_lockIcon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._unlockIcon)
		{
			_unlockIcon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._scriptIcon)
		{
			_scriptIcon = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.LastReparentFeedback)
		{
			value = VariantUtils.CreateFrom<string>(LastReparentFeedback);
			return true;
		}
		if (name == PropertyName.LastReparentHistoryId)
		{
			value = VariantUtils.CreateFrom<int>(LastReparentHistoryId);
			return true;
		}
		if (name == PropertyName.LastReparentPreservedGlobalTransform)
		{
			value = VariantUtils.CreateFrom<bool>(LastReparentPreservedGlobalTransform);
			return true;
		}
		if (name == PropertyName._editedSceneRoot)
		{
			value = VariantUtils.CreateFrom(in _editedSceneRoot);
			return true;
		}
		if (name == PropertyName._rootItem)
		{
			value = VariantUtils.CreateFrom(in _rootItem);
			return true;
		}
		if (name == PropertyName._suppressItemSelected)
		{
			value = VariantUtils.CreateFrom(in _suppressItemSelected);
			return true;
		}
		if (name == PropertyName._nextStructuralNodeRecordId)
		{
			value = VariantUtils.CreateFrom(in _nextStructuralNodeRecordId);
			return true;
		}
		if (name == PropertyName._nextSceneRootRecordId)
		{
			value = VariantUtils.CreateFrom(in _nextSceneRootRecordId);
			return true;
		}
		if (name == PropertyName._nextReparentNodeRecordId)
		{
			value = VariantUtils.CreateFrom(in _nextReparentNodeRecordId);
			return true;
		}
		if (name == PropertyName._filterText)
		{
			value = VariantUtils.CreateFrom(in _filterText);
			return true;
		}
		if (name == PropertyName._visibleIcon)
		{
			value = VariantUtils.CreateFrom(in _visibleIcon);
			return true;
		}
		if (name == PropertyName._hiddenIcon)
		{
			value = VariantUtils.CreateFrom(in _hiddenIcon);
			return true;
		}
		if (name == PropertyName._lockIcon)
		{
			value = VariantUtils.CreateFrom(in _lockIcon);
			return true;
		}
		if (name == PropertyName._unlockIcon)
		{
			value = VariantUtils.CreateFrom(in _unlockIcon);
			return true;
		}
		if (name == PropertyName._scriptIcon)
		{
			value = VariantUtils.CreateFrom(in _scriptIcon);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editedSceneRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rootItem, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._suppressItemSelected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextStructuralNodeRecordId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextSceneRootRecordId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._nextReparentNodeRecordId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._filterText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.LastReparentFeedback, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LastReparentHistoryId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.LastReparentPreservedGlobalTransform, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._visibleIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hiddenIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lockIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlockIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scriptIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.LastReparentFeedback, Variant.From<string>(LastReparentFeedback));
		info.AddProperty(PropertyName.LastReparentHistoryId, Variant.From<int>(LastReparentHistoryId));
		info.AddProperty(PropertyName.LastReparentPreservedGlobalTransform, Variant.From<bool>(LastReparentPreservedGlobalTransform));
		info.AddProperty(PropertyName._editedSceneRoot, Variant.From(in _editedSceneRoot));
		info.AddProperty(PropertyName._rootItem, Variant.From(in _rootItem));
		info.AddProperty(PropertyName._suppressItemSelected, Variant.From(in _suppressItemSelected));
		info.AddProperty(PropertyName._nextStructuralNodeRecordId, Variant.From(in _nextStructuralNodeRecordId));
		info.AddProperty(PropertyName._nextSceneRootRecordId, Variant.From(in _nextSceneRootRecordId));
		info.AddProperty(PropertyName._nextReparentNodeRecordId, Variant.From(in _nextReparentNodeRecordId));
		info.AddProperty(PropertyName._filterText, Variant.From(in _filterText));
		info.AddProperty(PropertyName._visibleIcon, Variant.From(in _visibleIcon));
		info.AddProperty(PropertyName._hiddenIcon, Variant.From(in _hiddenIcon));
		info.AddProperty(PropertyName._lockIcon, Variant.From(in _lockIcon));
		info.AddProperty(PropertyName._unlockIcon, Variant.From(in _unlockIcon));
		info.AddProperty(PropertyName._scriptIcon, Variant.From(in _scriptIcon));
		info.AddSignalEventDelegate(SignalName.NodeSelected, backing_NodeSelected);
		info.AddSignalEventDelegate(SignalName.NodeRenamed, backing_NodeRenamed);
		info.AddSignalEventDelegate(SignalName.HistorySelectionChanged, backing_HistorySelectionChanged);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.LastReparentFeedback, out var value))
		{
			LastReparentFeedback = value.As<string>();
		}
		if (info.TryGetProperty(PropertyName.LastReparentHistoryId, out var value2))
		{
			LastReparentHistoryId = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.LastReparentPreservedGlobalTransform, out var value3))
		{
			LastReparentPreservedGlobalTransform = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._editedSceneRoot, out var value4))
		{
			_editedSceneRoot = value4.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._rootItem, out var value5))
		{
			_rootItem = value5.As<TreeItem>();
		}
		if (info.TryGetProperty(PropertyName._suppressItemSelected, out var value6))
		{
			_suppressItemSelected = value6.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._nextStructuralNodeRecordId, out var value7))
		{
			_nextStructuralNodeRecordId = value7.As<long>();
		}
		if (info.TryGetProperty(PropertyName._nextSceneRootRecordId, out var value8))
		{
			_nextSceneRootRecordId = value8.As<long>();
		}
		if (info.TryGetProperty(PropertyName._nextReparentNodeRecordId, out var value9))
		{
			_nextReparentNodeRecordId = value9.As<long>();
		}
		if (info.TryGetProperty(PropertyName._filterText, out var value10))
		{
			_filterText = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName._visibleIcon, out var value11))
		{
			_visibleIcon = value11.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._hiddenIcon, out var value12))
		{
			_hiddenIcon = value12.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._lockIcon, out var value13))
		{
			_lockIcon = value13.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._unlockIcon, out var value14))
		{
			_unlockIcon = value14.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._scriptIcon, out var value15))
		{
			_scriptIcon = value15.As<Texture2D>();
		}
		if (info.TryGetSignalEventDelegate<NodeSelectedEventHandler>(SignalName.NodeSelected, out var value16))
		{
			backing_NodeSelected = value16;
		}
		if (info.TryGetSignalEventDelegate<NodeRenamedEventHandler>(SignalName.NodeRenamed, out var value17))
		{
			backing_NodeRenamed = value17;
		}
		if (info.TryGetSignalEventDelegate<HistorySelectionChangedEventHandler>(SignalName.HistorySelectionChanged, out var value18))
		{
			backing_HistorySelectionChanged = value18;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(3)
		{
			new MethodInfo(SignalName.NodeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(SignalName.NodeRenamed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "newName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.HistorySelectionChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null)
		};
	}

	protected void EmitSignalNodeSelected(Node node)
	{
		EmitSignal(SignalName.NodeSelected, new ReadOnlySpan<Variant>((Variant)node));
	}

	protected void EmitSignalNodeRenamed(Node node, string newName)
	{
		StringName nodeRenamed = SignalName.NodeRenamed;
		_003C_003Ey__InlineArray2<Variant> buffer = default;
		buffer[0] = node;
		buffer[1] = newName;
		EmitSignal(nodeRenamed, buffer);
	}

	protected void EmitSignalHistorySelectionChanged(Node node)
	{
		EmitSignal(SignalName.HistorySelectionChanged, new ReadOnlySpan<Variant>((Variant)node));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.NodeSelected && args.Count == 1)
		{
			backing_NodeSelected?.Invoke(VariantUtils.ConvertTo<Node>(in args[0]));
		}
		else if (signal == SignalName.NodeRenamed && args.Count == 2)
		{
			backing_NodeRenamed?.Invoke(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
		}
		else if (signal == SignalName.HistorySelectionChanged && args.Count == 1)
		{
			backing_HistorySelectionChanged?.Invoke(VariantUtils.ConvertTo<Node>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.NodeSelected)
		{
			return true;
		}
		if (signal == SignalName.NodeRenamed)
		{
			return true;
		}
		if (signal == SignalName.HistorySelectionChanged)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
