using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.GUI;
using PVZHE.ModEditor.Layout;
using PVZHE.ModEditor.Registry.BP;
using PVZHE.ModEditor.Registry.Class;
using PVZHE.ModEditor.ScriptEditor;

namespace PVZHE.ModEditor.Blueprint;

[ScriptPath("res://addons/ModEditor/Blueprint/GUI/GraphEdit/XWBPGraphEdit.cs")]
public class XWBPGraphEdit : GraphEdit
{
	public new class MethodName : GraphEdit.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName HasGraph = "HasGraph";

		public static readonly StringName HasUndoRedoManager = "HasUndoRedoManager";

		public static readonly StringName CreateExtendsClassUI = "CreateExtendsClassUI";

		public static readonly StringName CreateNavigationUI = "CreateNavigationUI";

		public static readonly StringName BuildCanvasContextMenu = "BuildCanvasContextMenu";

		public static readonly StringName GetCanvasContextMenu = "GetCanvasContextMenu";

		public static readonly StringName OnMinimapToggled = "OnMinimapToggled";

		public static readonly StringName ResetZoom = "ResetZoom";

		public static readonly StringName FocusSelectedNode = "FocusSelectedNode";

		public static readonly StringName RefreshNavigationOverview = "RefreshNavigationOverview";

		public static readonly StringName OnGraphNodeSelected = "OnGraphNodeSelected";

		public static readonly StringName OnGraphNodeDeselected = "OnGraphNodeDeselected";

		public static readonly StringName UpdateExtendsClassLabel = "UpdateExtendsClassLabel";

		public static readonly StringName ExtendsClassButtonPressed = "ExtendsClassButtonPressed";

		public static readonly StringName OnExtendsClassSelected = "OnExtendsClassSelected";

		public static readonly StringName GenerateScriptButtonPressed = "GenerateScriptButtonPressed";

		public static readonly StringName RunPreviewButtonPressed = "RunPreviewButtonPressed";

		public static readonly StringName GetRunPreviewButton = "GetRunPreviewButton";

		public static readonly StringName GetPreviewStatusLabel = "GetPreviewStatusLabel";

		public static readonly StringName SetRuntimePreviewState = "SetRuntimePreviewState";

		public static readonly StringName HighlightRuntimeNode = "HighlightRuntimeNode";

		public static readonly StringName ClearRuntimeHighlight = "ClearRuntimeHighlight";

		public static readonly StringName Init = "Init";

		public static readonly StringName InitNodes = "InitNodes";

		public static readonly StringName InitConnections = "InitConnections";

		public static readonly StringName IsConnectionDrawable = "IsConnectionDrawable";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName GetGraphNode = "GetGraphNode";

		public static readonly StringName RefreshNodeTransform = "RefreshNodeTransform";

		public static readonly StringName RefreshPortEditor = "RefreshPortEditor";

		public static readonly StringName GetGraphNodeId = "GetGraphNodeId";

		public static readonly StringName GetGraphNodeByName = "GetGraphNodeByName";

		public static readonly StringName ReserveNodeId = "ReserveNodeId";

		public static readonly StringName HasConnection = "HasConnection";

		public static readonly StringName CloneConnectionData = "CloneConnectionData";

		public new static readonly StringName _IsNodeHoverValid = "_IsNodeHoverValid";

		public static readonly StringName OnDeleteNodesRequest = "OnDeleteNodesRequest";

		public static readonly StringName DoRemoveNodes = "DoRemoveNodes";

		public static readonly StringName UndoRemoveNodes = "UndoRemoveNodes";

		public static readonly StringName AddConnection = "AddConnection";

		public static readonly StringName DoAddConnection = "DoAddConnection";

		public static readonly StringName RemoveConnection = "RemoveConnection";

		public static readonly StringName UndoRemoveConnection = "UndoRemoveConnection";

		public static readonly StringName AddNode = "AddNode";

		public static readonly StringName CreateNodeWithUndo = "CreateNodeWithUndo";

		public static readonly StringName DoCreateNode = "DoCreateNode";

		public static readonly StringName UndoCreateNode = "UndoCreateNode";

		public static readonly StringName RemoveGraphNodeWithName = "RemoveGraphNodeWithName";

		public static readonly StringName RemoveGraphNode = "RemoveGraphNode";

		public static readonly StringName GetNodeConnections = "GetNodeConnections";

		public static readonly StringName DoRemoveNode = "DoRemoveNode";

		public static readonly StringName UndoRemoveNode = "UndoRemoveNode";

		public new static readonly StringName _GuiInput = "_GuiInput";

		public static readonly StringName OnPopupRequest = "OnPopupRequest";

		public static readonly StringName ConfigureCanvasContextMenu = "ConfigureCanvasContextMenu";

		public static readonly StringName SetCanvasMenuItemDisabled = "SetCanvasMenuItemDisabled";

		public static readonly StringName OnCanvasContextMenuIdPressed = "OnCanvasContextMenuIdPressed";

		public static readonly StringName GetSelectedGraphNodeNames = "GetSelectedGraphNodeNames";

		public static readonly StringName ShowNodeSelector = "ShowNodeSelector";

		public static readonly StringName ToGraphPosition = "ToGraphPosition";

		public static readonly StringName OnConnectionRequest = "OnConnectionRequest";

		public static readonly StringName OnDisconnectionRequest = "OnDisconnectionRequest";

		public static readonly StringName ResetRecentlyDisconnected = "ResetRecentlyDisconnected";

		public static readonly StringName OnConnectionDragStarted = "OnConnectionDragStarted";

		public static readonly StringName OnConnectionDragEnded = "OnConnectionDragEnded";

		public static readonly StringName ResetConnectionDragState = "ResetConnectionDragState";

		public static readonly StringName ShowFilteredNodeSelector = "ShowFilteredNodeSelector";

		public static readonly StringName OnNodeSelectedFromPortDrag = "OnNodeSelectedFromPortDrag";

		public static readonly StringName DoCreateAndConnectNode = "DoCreateAndConnectNode";

		public static readonly StringName UndoCreateAndConnectNode = "UndoCreateAndConnectNode";

		public static readonly StringName FindCompatibleInputPort = "FindCompatibleInputPort";

		public static readonly StringName FindCompatibleOutputPort = "FindCompatibleOutputPort";

		public static readonly StringName ScorePortMatch = "ScorePortMatch";

		public new static readonly StringName _CanDropData = "_CanDropData";

		public new static readonly StringName _DropData = "_DropData";

		public static readonly StringName IsResourceDropData = "IsResourceDropData";

		public static readonly StringName DropResourceData = "DropResourceData";

		public static readonly StringName TryLoadDroppedResource = "TryLoadDroppedResource";

		public static readonly StringName DropFunctionData = "DropFunctionData";

		public static readonly StringName DropVariableData = "DropVariableData";

		public static readonly StringName OnVariablePopupMenuIdPressed = "OnVariablePopupMenuIdPressed";

		public static readonly StringName CreateGetNode = "CreateGetNode";

		public static readonly StringName CreateSetNode = "CreateSetNode";

		public static readonly StringName DropSignalData = "DropSignalData";

		public static readonly StringName OnSignalPopupMenuIdPressed = "OnSignalPopupMenuIdPressed";

		public static readonly StringName CreateSignalEventNode = "CreateSignalEventNode";

		public static readonly StringName CreateEmitSignalNode = "CreateEmitSignalNode";

		public static readonly StringName PopupAtLocalPosition = "PopupAtLocalPosition";

		public static readonly StringName OnCopyNodesRequest = "OnCopyNodesRequest";

		public static readonly StringName OnCutNodesRequest = "OnCutNodesRequest";

		public static readonly StringName OnPasteNodesRequest = "OnPasteNodesRequest";

		public static readonly StringName OnDuplicateNodesRequest = "OnDuplicateNodesRequest";

		public static readonly StringName CopySelectedNodes = "CopySelectedNodes";

		public static readonly StringName CutSelectedNodes = "CutSelectedNodes";

		public static readonly StringName PasteNodes = "PasteNodes";

		public static readonly StringName DoPasteNodes = "DoPasteNodes";

		public static readonly StringName UndoPasteNodes = "UndoPasteNodes";

		public static readonly StringName DuplicateSelectedNodes = "DuplicateSelectedNodes";

		public static readonly StringName DeselectAllGraphNodes = "DeselectAllGraphNodes";

		public static readonly StringName CloneNodeData = "CloneNodeData";

		public static readonly StringName AddNodeFromData = "AddNodeFromData";

		public static readonly StringName FocusNode = "FocusNode";
	}

	public new class PropertyName : GraphEdit.PropertyName
	{
		public static readonly StringName Editor = "Editor";

		public static readonly StringName GraphData = "GraphData";

		public static readonly StringName ClipboardNodeCount = "ClipboardNodeCount";

		public static readonly StringName ClipboardConnectionCount = "ClipboardConnectionCount";

		public static readonly StringName LastPointerGraphPosition = "LastPointerGraphPosition";

		public static readonly StringName _registry = "_registry";

		public static readonly StringName _extendsClassButton = "_extendsClassButton";

		public static readonly StringName _generateScriptButton = "_generateScriptButton";

		public static readonly StringName _runPreviewButton = "_runPreviewButton";

		public static readonly StringName _navigationTools = "_navigationTools";

		public static readonly StringName _previewStatusPanel = "_previewStatusPanel";

		public static readonly StringName _previewStatusLabel = "_previewStatusLabel";

		public static readonly StringName _minimapToggleButton = "_minimapToggleButton";

		public static readonly StringName _resetZoomButton = "_resetZoomButton";

		public static readonly StringName _focusSelectionButton = "_focusSelectionButton";

		public static readonly StringName _nodeCountLabel = "_nodeCountLabel";

		public static readonly StringName _connectionCountLabel = "_connectionCountLabel";

		public static readonly StringName _nodeCountIcon = "_nodeCountIcon";

		public static readonly StringName _connectionCountIcon = "_connectionCountIcon";

		public static readonly StringName _variablePopupMenu = "_variablePopupMenu";

		public static readonly StringName _signalPopupMenu = "_signalPopupMenu";

		public static readonly StringName _canvasContextMenu = "_canvasContextMenu";

		public static readonly StringName _pendingVariableData = "_pendingVariableData";

		public static readonly StringName _pendingSignalData = "_pendingSignalData";

		public static readonly StringName _pendingDropPosition = "_pendingDropPosition";

		public static readonly StringName _lastPointerGraphPosition = "_lastPointerGraphPosition";

		public static readonly StringName _isDraggingConnection = "_isDraggingConnection";

		public static readonly StringName _dragFromNode = "_dragFromNode";

		public static readonly StringName _dragFromPort = "_dragFromPort";

		public static readonly StringName _dragIsOutput = "_dragIsOutput";

		public static readonly StringName _connectionMadeDuringDrag = "_connectionMadeDuringDrag";

		public static readonly StringName _disconnectionDuringDrag = "_disconnectionDuringDrag";

		public static readonly StringName _recentlyDisconnected = "_recentlyDisconnected";

		public static readonly StringName _runtimeHighlightedNode = "_runtimeHighlightedNode";
	}

	public new class SignalName : GraphEdit.SignalName
	{
	}

	public const long CanvasMenuAddNodeId = 100L;

	public const long CanvasMenuCopyId = 200L;

	public const long CanvasMenuCutId = 201L;

	public const long CanvasMenuDuplicateId = 202L;

	public const long CanvasMenuPasteId = 203L;

	public const long CanvasMenuDeleteId = 204L;

	private XWBPNodeRegistry _registry;

	private Button _extendsClassButton;

	private Button _generateScriptButton;

	private Button _runPreviewButton;

	private HBoxContainer _navigationTools;

	private PanelContainer _previewStatusPanel;

	private Label _previewStatusLabel;

	private Button _minimapToggleButton;

	private Button _resetZoomButton;

	private Button _focusSelectionButton;

	private Label _nodeCountLabel;

	private Label _connectionCountLabel;

	private TextureRect _nodeCountIcon;

	private TextureRect _connectionCountIcon;

	private PopupMenu _variablePopupMenu;

	private PopupMenu _signalPopupMenu;

	private PopupMenu _canvasContextMenu;

	private XWBPVariableData _pendingVariableData;

	private XWBPSignalData _pendingSignalData;

	private Vector2 _pendingDropPosition = Vector2.Zero;

	private Vector2 _lastPointerGraphPosition = Vector2.Zero;

	private bool _isDraggingConnection;

	private StringName _dragFromNode = "";

	private int _dragFromPort = -1;

	private bool _dragIsOutput = true;

	private bool _connectionMadeDuringDrag;

	private bool _disconnectionDuringDrag;

	private bool _recentlyDisconnected;

	private XWBPGraphNode _runtimeHighlightedNode;

	private readonly List<XWBPNodeData> _clipboardNodes = new List<XWBPNodeData>();

	private readonly List<(int fromLocalIdx, int fromPort, int toLocalIdx, int toPort)> _clipboardConnections = new List<(int, int, int, int)>();

	public XWBPEditor Editor { get; set; }

	public XWBPGraphData GraphData { get; private set; }

	public System.Collections.Generic.Dictionary<int, XWBPGraphNode> GraphNodeDictionary { get; } = new System.Collections.Generic.Dictionary<int, XWBPGraphNode>();

	public int ClipboardNodeCount => _clipboardNodes.Count;

	public int ClipboardConnectionCount => _clipboardConnections.Count;

	public Vector2 LastPointerGraphPosition => _lastPointerGraphPosition;

	public XWBPGraphEdit()
	{
		ConnectionLinesThickness = 4f;
		ConnectionLinesCurvature = 0.5f;
	}

	public override void _Ready()
	{
		_registry = XWBPNodeRegistry.Instance;
		ConnectionRequest += OnConnectionRequest;
		DisconnectionRequest += OnDisconnectionRequest;
		ConnectionDragStarted += OnConnectionDragStarted;
		ConnectionDragEnded += OnConnectionDragEnded;
		NodeSelected += OnGraphNodeSelected;
		NodeDeselected += OnGraphNodeDeselected;
		DeleteNodesRequest += OnDeleteNodesRequest;
		CopyNodesRequest += OnCopyNodesRequest;
		CutNodesRequest += OnCutNodesRequest;
		PasteNodesRequest += OnPasteNodesRequest;
		DuplicateNodesRequest += OnDuplicateNodesRequest;
		PopupRequest += OnPopupRequest;
		_extendsClassButton = GetNodeOrNull<Button>("%ExtendsClassButton");
		_generateScriptButton = GetNodeOrNull<Button>("%GenerateScriptButton");
		_runPreviewButton = GetNodeOrNull<Button>("%RunPreviewButton");
		_navigationTools = GetNodeOrNull<HBoxContainer>("%BlueprintNavigationTools");
		_previewStatusPanel = GetNodeOrNull<PanelContainer>("%PreviewStatusPanel");
		_previewStatusLabel = GetNodeOrNull<Label>("%PreviewStatusLabel");
		_minimapToggleButton = GetNodeOrNull<Button>("%MinimapToggleButton");
		_resetZoomButton = GetNodeOrNull<Button>("%ResetZoomButton");
		_focusSelectionButton = GetNodeOrNull<Button>("%FocusSelectionButton");
		_nodeCountLabel = GetNodeOrNull<Label>("%NodeCountLabel");
		_connectionCountLabel = GetNodeOrNull<Label>("%ConnectionCountLabel");
		_nodeCountIcon = GetNodeOrNull<TextureRect>("%NodeCountIcon");
		_connectionCountIcon = GetNodeOrNull<TextureRect>("%ConnectionCountIcon");
		_variablePopupMenu = GetNodeOrNull<PopupMenu>("%VariablePopupMenu");
		_signalPopupMenu = GetNodeOrNull<PopupMenu>("%SignalPopupMenu");
		CreateExtendsClassUI();
		CreateNavigationUI();
		BuildCanvasContextMenu();
		UpdateExtendsClassLabel();
		if (_extendsClassButton != null)
		{
			_extendsClassButton.Pressed += ExtendsClassButtonPressed;
		}
		if (_generateScriptButton != null)
		{
			_generateScriptButton.Pressed += GenerateScriptButtonPressed;
		}
		if (_runPreviewButton != null)
		{
			_runPreviewButton.Pressed += RunPreviewButtonPressed;
		}
		if (_variablePopupMenu != null)
		{
			_variablePopupMenu.IdPressed += OnVariablePopupMenuIdPressed;
		}
		if (_signalPopupMenu != null)
		{
			_signalPopupMenu.IdPressed += OnSignalPopupMenuIdPressed;
		}
		RefreshNavigationOverview();
	}

	public bool HasGraph()
	{
		return GodotObject.IsInstanceValid(GraphData);
	}

	private bool HasUndoRedoManager()
	{
		if (GodotObject.IsInstanceValid(Editor))
		{
			return GodotObject.IsInstanceValid(Editor.UndoRedoManager);
		}
		return false;
	}

	private void CreateExtendsClassUI()
	{
		HBoxContainer hBoxContainer = Call("get_menu_hbox").As<HBoxContainer>();
		if (GodotObject.IsInstanceValid(hBoxContainer))
		{
			if (GodotObject.IsInstanceValid(_extendsClassButton) && _extendsClassButton.GetParent() != hBoxContainer)
			{
				_extendsClassButton.Reparent(hBoxContainer);
			}
			if (GodotObject.IsInstanceValid(_generateScriptButton) && _generateScriptButton.GetParent() != hBoxContainer)
			{
				_generateScriptButton.Reparent(hBoxContainer);
			}
			if (GodotObject.IsInstanceValid(_runPreviewButton) && _runPreviewButton.GetParent() != hBoxContainer)
			{
				_runPreviewButton.Reparent(hBoxContainer);
			}
			if (GodotObject.IsInstanceValid(_generateScriptButton))
			{
				_generateScriptButton.Icon = XWClassRegistry.Instance.GetUIIcon("TransitionSyncAuto");
			}
			if (GodotObject.IsInstanceValid(_runPreviewButton))
			{
				_runPreviewButton.Icon = XWClassRegistry.Instance.GetUIIcon("MainPlay");
			}
		}
	}

	private void CreateNavigationUI()
	{
		HBoxContainer hBoxContainer = Call("get_menu_hbox").As<HBoxContainer>();
		if (!GodotObject.IsInstanceValid(hBoxContainer))
		{
			return;
		}
		ShowMinimapButton = false;
		ShowZoomButtons = true;
		ShowZoomLabel = true;
		MinimapEnabled = true;
		MinimapSize = new Vector2(220f, 140f);
		if (GodotObject.IsInstanceValid(_navigationTools) && _navigationTools.GetParent() != hBoxContainer)
		{
			_navigationTools.Reparent(hBoxContainer);
		}
		SetRuntimePreviewState(running: false, "● 预览待机", new Color("7f93a8"), "在受限纯数据环境中运行当前拼图；不会载入或执行生成的 Mod 程序集");
		XWClassRegistry instance = XWClassRegistry.Instance;
		if (GodotObject.IsInstanceValid(_minimapToggleButton))
		{
			_minimapToggleButton.Icon = instance.GetUIIcon("GridMinimap");
			_minimapToggleButton.ButtonPressed = MinimapEnabled;
			_minimapToggleButton.Toggled += OnMinimapToggled;
		}
		if (GodotObject.IsInstanceValid(_resetZoomButton))
		{
			_resetZoomButton.Icon = instance.GetUIIcon("ZoomReset");
			_resetZoomButton.Pressed += ResetZoom;
		}
		if (GodotObject.IsInstanceValid(_focusSelectionButton))
		{
			_focusSelectionButton.Icon = instance.GetUIIcon("CenterView");
			_focusSelectionButton.Pressed += () =>
			{
				FocusSelectedNode();
			};
		}
		if (GodotObject.IsInstanceValid(_nodeCountIcon))
		{
			_nodeCountIcon.Texture = instance.GetUIIcon("GraphNode");
		}
		if (GodotObject.IsInstanceValid(_connectionCountIcon))
		{
			_connectionCountIcon.Texture = instance.GetUIIcon("GuiGraphNodePort");
		}
	}

	private void BuildCanvasContextMenu()
	{
		if (!GodotObject.IsInstanceValid(_canvasContextMenu))
		{
			_canvasContextMenu = new PopupMenu
			{
				Name = "BlueprintCanvasContextMenu"
			};
			AddChild(_canvasContextMenu, forceReadableName: false, InternalMode.Disabled);
			Texture2D texture = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Add.svg", null, ResourceLoader.CacheMode.Reuse);
			Texture2D texture2 = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ActionCopy.svg", null, ResourceLoader.CacheMode.Reuse);
			Texture2D texture3 = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ActionCut.svg", null, ResourceLoader.CacheMode.Reuse);
			Texture2D texture4 = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Duplicate.svg", null, ResourceLoader.CacheMode.Reuse);
			Texture2D texture5 = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ActionPaste.svg", null, ResourceLoader.CacheMode.Reuse);
			Texture2D texture6 = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Remove.svg", null, ResourceLoader.CacheMode.Reuse);
			_canvasContextMenu.AddIconItem(texture, "添加拼图", 100, Key.None);
			_canvasContextMenu.AddSeparator();
			_canvasContextMenu.AddIconItem(texture2, "复制", 200, Key.None);
			_canvasContextMenu.AddIconItem(texture3, "剪切", 201, Key.None);
			_canvasContextMenu.AddIconItem(texture4, "重复", 202, Key.None);
			_canvasContextMenu.AddIconItem(texture5, "粘贴到这里", 203, Key.None);
			_canvasContextMenu.AddIconItem(texture6, "删除", 204, Key.None);
			_canvasContextMenu.IdPressed += OnCanvasContextMenuIdPressed;
		}
	}

	public PopupMenu GetCanvasContextMenu()
	{
		return _canvasContextMenu;
	}

	private void OnMinimapToggled(bool enabled)
	{
		MinimapEnabled = enabled;
		if (GodotObject.IsInstanceValid(_minimapToggleButton))
		{
			_minimapToggleButton.TooltipText = (enabled ? "隐藏蓝图小地图" : "显示蓝图小地图");
		}
	}

	public void ResetZoom()
	{
		Zoom = 1f;
	}

	public bool FocusSelectedNode()
	{
		foreach (KeyValuePair<int, XWBPGraphNode> item in GraphNodeDictionary)
		{
			if (GodotObject.IsInstanceValid(item.Value) && item.Value.Selected)
			{
				FocusNode(item.Key);
				return true;
			}
		}
		return false;
	}

	public void RefreshNavigationOverview()
	{
		int num = (HasGraph() ? GraphNodeDictionary.Count : 0);
		int num2 = (HasGraph() ? GraphData.Connections.Count : 0);
		if (GodotObject.IsInstanceValid(_nodeCountLabel))
		{
			_nodeCountLabel.Text = num.ToString();
		}
		if (GodotObject.IsInstanceValid(_connectionCountLabel))
		{
			_connectionCountLabel.Text = num2.ToString();
		}
		bool flag = false;
		foreach (XWBPGraphNode value in GraphNodeDictionary.Values)
		{
			if (GodotObject.IsInstanceValid(value) && value.Selected)
			{
				flag = true;
				break;
			}
		}
		if (GodotObject.IsInstanceValid(_focusSelectionButton))
		{
			_focusSelectionButton.Disabled = !flag;
		}
	}

	private void OnGraphNodeSelected(Node node)
	{
		Editor?.OnGraphNodeSelected(node as XWBPGraphNode);
		RefreshNavigationOverview();
	}

	private void OnGraphNodeDeselected(Node _)
	{
		Editor?.OnGraphNodeDeselected();
		RefreshNavigationOverview();
	}

	public void UpdateExtendsClassLabel()
	{
		if (!GodotObject.IsInstanceValid(_extendsClassButton))
		{
			return;
		}
		if (!GodotObject.IsInstanceValid(Editor) || !GodotObject.IsInstanceValid(Editor.BpScriptData))
		{
			_extendsClassButton.Text = "无继承";
			_extendsClassButton.Icon = null;
			return;
		}
		string text = Editor.BpScriptData.ExtendsClass.ToString();
		if (string.IsNullOrEmpty(text))
		{
			_extendsClassButton.Text = "无继承";
			_extendsClassButton.Icon = null;
		}
		else
		{
			_extendsClassButton.Text = text;
			_extendsClassButton.Icon = XWClassRegistry.Instance.GetClassIcon(text);
		}
	}

	private void ExtendsClassButtonPressed()
	{
		if (GodotObject.IsInstanceValid(Editor) && Editor.HasEditScript())
		{
			XWWindowExtendsClassSelector xWWindowExtendsClassSelector = XWWindowExtendsClassSelector.Create();
			xWWindowExtendsClassSelector.ContextEditor = Editor;
			xWWindowExtendsClassSelector.ClassSelected += OnExtendsClassSelected;
			AddChild(xWWindowExtendsClassSelector, forceReadableName: false, InternalMode.Disabled);
			xWWindowExtendsClassSelector.PopupCentered();
		}
	}

	private void OnExtendsClassSelected(StringName className)
	{
		if (GodotObject.IsInstanceValid(Editor) && GodotObject.IsInstanceValid(Editor.BpScriptData))
		{
			Editor.BpScriptData.ExtendsClass = className;
			UpdateExtendsClassLabel();
		}
	}

	private void GenerateScriptButtonPressed()
	{
		Editor?.GenerateScriptButtonPressed();
	}

	private async void RunPreviewButtonPressed()
	{
		if (Editor != null)
		{
			await Editor.RunSafePreviewAsync();
		}
	}

	public Button GetRunPreviewButton()
	{
		return _runPreviewButton;
	}

	public Label GetPreviewStatusLabel()
	{
		return _previewStatusLabel;
	}

	public void SetRuntimePreviewState(bool running, string text, Color accent, string tooltip = "")
	{
		if (GodotObject.IsInstanceValid(_runPreviewButton))
		{
			_runPreviewButton.Disabled = running || !HasGraph();
		}
		if (GodotObject.IsInstanceValid(_previewStatusLabel))
		{
			_previewStatusLabel.Text = text ?? string.Empty;
			_previewStatusLabel.Modulate = accent;
		}
		if (GodotObject.IsInstanceValid(_previewStatusPanel))
		{
			_previewStatusPanel.TooltipText = tooltip ?? string.Empty;
			StyleBoxFlat stylebox = new StyleBoxFlat
			{
				BgColor = new Color(accent, 0.12f),
				BorderColor = new Color(accent, 0.58f),
				CornerRadiusTopLeft = 7,
				CornerRadiusTopRight = 7,
				CornerRadiusBottomLeft = 7,
				CornerRadiusBottomRight = 7,
				BorderWidthLeft = 1,
				BorderWidthTop = 1,
				BorderWidthRight = 1,
				BorderWidthBottom = 1,
				ContentMarginLeft = 8f,
				ContentMarginRight = 8f,
				ContentMarginTop = 4f,
				ContentMarginBottom = 4f
			};
			_previewStatusPanel.AddThemeStyleboxOverride("panel", stylebox);
		}
	}

	public void HighlightRuntimeNode(int nodeId)
	{
		ClearRuntimeHighlight();
		if (GraphNodeDictionary.TryGetValue(nodeId, out var value) && GodotObject.IsInstanceValid(value))
		{
			_runtimeHighlightedNode = value;
			value.Modulate = new Color("ffd261");
			FocusNode(nodeId);
		}
	}

	public void ClearRuntimeHighlight()
	{
		if (GodotObject.IsInstanceValid(_runtimeHighlightedNode))
		{
			_runtimeHighlightedNode.Modulate = Colors.White;
		}
		_runtimeHighlightedNode = null;
	}

	public void Init(XWBPGraphData graphData)
	{
		if (GraphData == graphData)
		{
			return;
		}
		ClearRuntimeHighlight();
		Clear();
		GraphData = graphData;
		if (!HasGraph())
		{
			if (GodotObject.IsInstanceValid(_runPreviewButton))
			{
				_runPreviewButton.Disabled = true;
			}
			return;
		}
		InitNodes();
		InitConnections();
		RefreshNavigationOverview();
		if (GodotObject.IsInstanceValid(_runPreviewButton))
		{
			_runPreviewButton.Disabled = false;
		}
	}

	private void InitNodes()
	{
		if (!HasGraph())
		{
			return;
		}
		foreach (KeyValuePair<int, XWBPNodeData> node in GraphData.Nodes)
		{
			XWBPNodeData value = node.Value;
			if (value != null)
			{
				XWBPNodeType nodeType = _registry.GetNodeType(value.TypeId.ToString());
				if (nodeType == null)
				{
					GD.PrintErr($"[XWBPGraphEdit] 未知节点类型: {value.TypeId}");
					continue;
				}
				nodeType.LoadNodeData(value);
				XWBPGraphNode xWBPGraphNode = XWBPGraphNode.Create();
				xWBPGraphNode.Editor = Editor;
				AddChild(xWBPGraphNode, forceReadableName: false, InternalMode.Disabled);
				xWBPGraphNode.Init(value, nodeType);
				GraphNodeDictionary[value.Id] = xWBPGraphNode;
			}
		}
	}

	private void InitConnections()
	{
		if (!HasGraph())
		{
			return;
		}
		foreach (XWBPNodeConnectionData connection in GraphData.Connections)
		{
			if (IsConnectionDrawable(connection))
			{
				XWBPGraphNode graphNode = GetGraphNode(connection.FromNodeId);
				XWBPGraphNode graphNode2 = GetGraphNode(connection.ToNodeId);
				if (graphNode != null && graphNode2 != null && !IsNodeConnected(graphNode.Name, connection.FromPortIndex, graphNode2.Name, connection.ToPortIndex))
				{
					ConnectNode(graphNode.Name, connection.FromPortIndex, graphNode2.Name, connection.ToPortIndex);
				}
			}
		}
	}

	private bool IsConnectionDrawable(XWBPNodeConnectionData connection)
	{
		if (!HasGraph() || connection == null)
		{
			return false;
		}
		if (!GraphData.IsConnectionStructurallyValid(connection, ignoreOccupiedPorts: true))
		{
			return false;
		}
		if (GetGraphNode(connection.FromNodeId) != null)
		{
			return GetGraphNode(connection.ToNodeId) != null;
		}
		return false;
	}

	public void Clear()
	{
		ClearConnections();
		foreach (Node child in GetChildren())
		{
			if (child is XWBPGraphNode xWBPGraphNode)
			{
				xWBPGraphNode.QueueFree();
			}
		}
		GraphNodeDictionary.Clear();
		GraphData = null;
		RefreshNavigationOverview();
	}

	public XWBPGraphNode GetGraphNode(int nodeId)
	{
		if (!GraphNodeDictionary.TryGetValue(nodeId, out var value))
		{
			return null;
		}
		return value;
	}

	public void RefreshNodeTransform(int nodeId)
	{
		XWBPGraphNode graphNode = GetGraphNode(nodeId);
		XWBPNodeData xWBPNodeData = GraphData?.GetNode(nodeId);
		if (GodotObject.IsInstanceValid(graphNode) && xWBPNodeData != null)
		{
			graphNode.PositionOffset = xWBPNodeData.Position;
			if (xWBPNodeData.Size != Vector2.Zero)
			{
				graphNode.Size = xWBPNodeData.Size;
			}
			QueueRedraw();
		}
	}

	public void RefreshPortEditor(XWBPNodePortData port)
	{
		if (!HasGraph() || !GodotObject.IsInstanceValid(port))
		{
			return;
		}
		foreach (var (nodeId, xWBPNodeData2) in GraphData.Nodes)
		{
			if (xWBPNodeData2 != null && xWBPNodeData2.InputPorts.Contains(port))
			{
				XWBPGraphNode graphNode = GetGraphNode(nodeId);
				if (GodotObject.IsInstanceValid(graphNode))
				{
					graphNode.PortChange();
				}
				break;
			}
		}
	}

	public int GetGraphNodeId(StringName nodeName)
	{
		foreach (KeyValuePair<int, XWBPGraphNode> item in GraphNodeDictionary)
		{
			if (item.Value.Name == (StringName)nodeName.ToString())
			{
				return item.Key;
			}
		}
		return -1;
	}

	public XWBPGraphNode GetGraphNodeByName(StringName nodeName)
	{
		int graphNodeId = GetGraphNodeId(nodeName);
		if (graphNodeId < 0)
		{
			return null;
		}
		return GetGraphNode(graphNodeId);
	}

	private void ReserveNodeId(XWBPNodeData nodeData)
	{
		if (HasGraph() && nodeData != null && nodeData.Id <= 0)
		{
			if (GraphData.NextNodeId < 1)
			{
				GraphData.NextNodeId = 1;
			}
			while (GraphData.Nodes.ContainsKey(GraphData.NextNodeId))
			{
				GraphData.NextNodeId++;
			}
			nodeData.Id = GraphData.NextNodeId;
			GraphData.NextNodeId++;
		}
	}

	private bool HasConnection(int fromNodeId, int fromPort, int toNodeId, int toPort)
	{
		if (!HasGraph())
		{
			return false;
		}
		foreach (XWBPNodeConnectionData connection in GraphData.Connections)
		{
			if (connection != null && connection.FromNodeId == fromNodeId && connection.FromPortIndex == fromPort && connection.ToNodeId == toNodeId && connection.ToPortIndex == toPort)
			{
				return true;
			}
		}
		return false;
	}

	private static XWBPNodeConnectionData CloneConnectionData(XWBPNodeConnectionData connection)
	{
		if (connection != null)
		{
			return new XWBPNodeConnectionData(connection.FromNodeId, connection.FromPortIndex, connection.ToNodeId, connection.ToPortIndex);
		}
		return null;
	}

	public override bool _IsNodeHoverValid(StringName fromNode, int fromPort, StringName toNode, int toPort)
	{
		if (fromNode == toNode || !HasGraph())
		{
			return false;
		}
		int graphNodeId = GetGraphNodeId(fromNode);
		int graphNodeId2 = GetGraphNodeId(toNode);
		if (graphNodeId < 0 || graphNodeId2 < 0)
		{
			return false;
		}
		return GraphData.CanConnect(graphNodeId, fromPort, graphNodeId2, toPort);
	}

	private void OnDeleteNodesRequest(Array<StringName> nodes)
	{
		if (!HasGraph())
		{
			return;
		}
		List<int> list = new List<int>();
		foreach (StringName node2 in nodes)
		{
			int graphNodeId = GetGraphNodeId(node2);
			if (graphNodeId >= 0)
			{
				XWBPNodeData node = GraphData.GetNode(graphNodeId);
				if (node != null && !node.Lock)
				{
					list.Add(graphNodeId);
				}
			}
		}
		RemoveNodesWithUndo(list, "删除节点");
	}

	private void RemoveNodesWithUndo(List<int> nodeIds, string actionName)
	{
		if (!HasGraph() || nodeIds == null || nodeIds.Count == 0)
		{
			return;
		}
		HashSet<int> hashSet = new HashSet<int>();
		Array from = new Array();
		foreach (int nodeId in nodeIds)
		{
			XWBPNodeData from2 = GraphData.GetNode(nodeId);
			if (from2 != null && !from2.Lock && hashSet.Add(nodeId))
			{
				from.Add(Variant.From(in from2));
			}
		}
		if (from.Count == 0)
		{
			return;
		}
		Array from3 = new Array();
		foreach (XWBPNodeConnectionData connection in GraphData.Connections)
		{
			if (connection != null && (hashSet.Contains(connection.FromNodeId) || hashSet.Contains(connection.ToNodeId)))
			{
				from3.Add(Variant.From<XWBPNodeConnectionData>(CloneConnectionData(connection)));
			}
		}
		if (!HasUndoRedoManager())
		{
			DoRemoveNodes(from);
			return;
		}
		Editor.CreateBlueprintAction(actionName);
		Editor.UndoRedoManager.AddDoMethod(this, "DoRemoveNodes", Variant.From(in from));
		Editor.UndoRedoManager.AddUndoMethod(this, "UndoRemoveNodes", Variant.From(in from), Variant.From(in from3));
		Editor.UndoRedoManager.CommitAction();
	}

	public void DoRemoveNodes(Array nodes)
	{
		if (!HasGraph() || nodes == null)
		{
			return;
		}
		foreach (Variant node in nodes)
		{
			XWBPNodeData xWBPNodeData = node.As<XWBPNodeData>();
			if (xWBPNodeData != null)
			{
				DoRemoveNode(xWBPNodeData.Id);
			}
		}
	}

	public void UndoRemoveNodes(Array nodes, Array connections)
	{
		if (!HasGraph() || nodes == null)
		{
			return;
		}
		DeselectAllGraphNodes();
		foreach (Variant node in nodes)
		{
			XWBPNodeData xWBPNodeData = node.As<XWBPNodeData>();
			if (xWBPNodeData != null)
			{
				DoCreateNode(xWBPNodeData);
				XWBPGraphNode graphNode = GetGraphNode(xWBPNodeData.Id);
				if (GodotObject.IsInstanceValid(graphNode))
				{
					graphNode.Selected = true;
				}
			}
		}
		if (connections != null)
		{
			foreach (Variant connection in connections)
			{
				XWBPNodeConnectionData xWBPNodeConnectionData = connection.As<XWBPNodeConnectionData>();
				if (xWBPNodeConnectionData != null)
				{
					DoAddConnection(xWBPNodeConnectionData.FromNodeId, xWBPNodeConnectionData.FromPortIndex, xWBPNodeConnectionData.ToNodeId, xWBPNodeConnectionData.ToPortIndex);
				}
			}
		}
		RefreshNavigationOverview();
	}

	public void AddConnection(StringName fromNode, int fromPort, StringName toNode, int toPort)
	{
		if (!HasGraph())
		{
			return;
		}
		int from = GetGraphNodeId(fromNode);
		int from2 = GetGraphNodeId(toNode);
		if (from >= 0 && from2 >= 0 && GraphData.CanConnect(from, fromPort, from2, toPort))
		{
			if (HasUndoRedoManager())
			{
				Editor.CreateBlueprintAction("连接节点");
				Editor.UndoRedoManager.AddDoMethod(this, "DoAddConnection", Variant.From(in from), Variant.From(in fromPort), Variant.From(in from2), Variant.From(in toPort));
				Editor.UndoRedoManager.AddUndoMethod(this, "UndoRemoveConnection", Variant.From(in from), Variant.From(in fromPort), Variant.From(in from2), Variant.From(in toPort));
				Editor.UndoRedoManager.CommitAction();
			}
			else
			{
				DoAddConnection(from, fromPort, from2, toPort);
			}
		}
	}

	public void DoAddConnection(int fromNodeId, int fromPort, int toNodeId, int toPort)
	{
		if (!HasGraph())
		{
			return;
		}
		if (!GraphData.AddConnection(fromNodeId, fromPort, toNodeId, toPort))
		{
			XWBPNodeConnectionData connection = new XWBPNodeConnectionData(fromNodeId, fromPort, toNodeId, toPort);
			if (!IsConnectionDrawable(connection))
			{
				return;
			}
		}
		XWBPGraphNode graphNode = GetGraphNode(fromNodeId);
		XWBPGraphNode graphNode2 = GetGraphNode(toNodeId);
		if (graphNode != null && graphNode2 != null)
		{
			if (!IsNodeConnected(graphNode.Name, fromPort, graphNode2.Name, toPort))
			{
				ConnectNode(graphNode.Name, fromPort, graphNode2.Name, toPort);
			}
		}
		else
		{
			GraphData.RemoveConnection(fromNodeId, fromPort, toNodeId, toPort);
		}
		RefreshNavigationOverview();
	}

	public void RemoveConnection(StringName fromNode, int fromPort, StringName toNode, int toPort)
	{
		if (!HasGraph())
		{
			return;
		}
		int from = GetGraphNodeId(fromNode);
		int from2 = GetGraphNodeId(toNode);
		if (from >= 0 && from2 >= 0 && HasConnection(from, fromPort, from2, toPort))
		{
			if (HasUndoRedoManager())
			{
				Editor.CreateBlueprintAction("断开连接");
				Editor.UndoRedoManager.AddDoMethod(this, "UndoRemoveConnection", Variant.From(in from), Variant.From(in fromPort), Variant.From(in from2), Variant.From(in toPort));
				Editor.UndoRedoManager.AddUndoMethod(this, "DoAddConnection", Variant.From(in from), Variant.From(in fromPort), Variant.From(in from2), Variant.From(in toPort));
				Editor.UndoRedoManager.CommitAction();
			}
			else
			{
				UndoRemoveConnection(from, fromPort, from2, toPort);
			}
		}
	}

	public void UndoRemoveConnection(int fromNodeId, int fromPort, int toNodeId, int toPort)
	{
		if (HasGraph())
		{
			XWBPGraphNode graphNode = GetGraphNode(fromNodeId);
			XWBPGraphNode graphNode2 = GetGraphNode(toNodeId);
			if (graphNode != null && graphNode2 != null && IsNodeConnected(graphNode.Name, fromPort, graphNode2.Name, toPort))
			{
				DisconnectNode(graphNode.Name, fromPort, graphNode2.Name, toPort);
			}
			GraphData.RemoveConnection(fromNodeId, fromPort, toNodeId, toPort);
			RefreshNavigationOverview();
		}
	}

	public XWBPGraphNode AddNode(string typeId, Vector2 position)
	{
		if (!HasGraph())
		{
			return null;
		}
		XWBPNodeType nodeType = _registry.GetNodeType(typeId);
		if (nodeType == null)
		{
			GD.PrintErr("[XWBPGraphEdit] 未知节点类型: " + typeId);
			return null;
		}
		XWBPNodeData xWBPNodeData = nodeType.CreateNodeData();
		xWBPNodeData.Position = position;
		return CreateNodeWithUndo(xWBPNodeData);
	}

	public XWBPGraphNode AddNode(XWBPNodeType nodeType, Vector2 position)
	{
		if (!HasGraph() || nodeType == null)
		{
			return null;
		}
		XWBPNodeData xWBPNodeData = nodeType.CreateNodeData();
		xWBPNodeData.NodeType = nodeType;
		xWBPNodeData.Position = position;
		return CreateNodeWithUndo(xWBPNodeData);
	}

	private XWBPGraphNode CreateNodeWithUndo(XWBPNodeData nodeData, string actionName = "创建节点")
	{
		if (!HasGraph() || nodeData == null)
		{
			return null;
		}
		ReserveNodeId(nodeData);
		if (HasUndoRedoManager())
		{
			if (actionName == "创建节点")
			{
				Editor.CreateBlueprintAction("创建节点");
			}
			else
			{
				Editor.CreateBlueprintAction(actionName);
			}
			Editor.UndoRedoManager.AddDoMethod(this, "DoCreateNode", Variant.From(in nodeData));
			Editor.UndoRedoManager.AddUndoMethod(this, "UndoCreateNode", Variant.From(in nodeData));
			Editor.UndoRedoManager.CommitAction();
			return GetGraphNode(nodeData.Id);
		}
		DoCreateNode(nodeData);
		return GetGraphNode(nodeData.Id);
	}

	public void DoCreateNode(XWBPNodeData nodeData)
	{
		if (HasGraph() && nodeData != null && !GraphNodeDictionary.ContainsKey(nodeData.Id))
		{
			XWBPNodeType xWBPNodeType = nodeData.NodeType ?? _registry.GetNodeType(nodeData.TypeId.ToString());
			if (xWBPNodeType == null)
			{
				GD.PrintErr($"[XWBPGraphEdit] 未知节点类型: {nodeData.TypeId}");
				return;
			}
			xWBPNodeType.LoadNodeData(nodeData);
			nodeData.NodeType = xWBPNodeType;
			GraphData.AddNodePreserveId(nodeData);
			int id = nodeData.Id;
			XWBPGraphNode xWBPGraphNode = XWBPGraphNode.Create();
			xWBPGraphNode.Editor = Editor;
			AddChild(xWBPGraphNode, forceReadableName: false, InternalMode.Disabled);
			xWBPGraphNode.Init(nodeData, xWBPNodeType);
			GraphNodeDictionary[id] = xWBPGraphNode;
			RefreshNavigationOverview();
		}
	}

	public void UndoCreateNode(XWBPNodeData nodeData)
	{
		if (nodeData != null)
		{
			DoRemoveNode(nodeData.Id);
		}
	}

	public void RemoveGraphNodeWithName(string nodeName)
	{
		int graphNodeId = GetGraphNodeId(nodeName);
		if (graphNodeId >= 0)
		{
			RemoveGraphNode(graphNodeId);
		}
	}

	public void RemoveGraphNode(int nodeId)
	{
		if (!HasGraph())
		{
			return;
		}
		XWBPNodeData from = GraphData.GetNode(nodeId);
		if (from != null && !from.Lock)
		{
			Array from2 = GetNodeConnections(nodeId);
			if (HasUndoRedoManager())
			{
				Editor.CreateBlueprintAction("删除节点");
				Editor.UndoRedoManager.AddDoMethod(this, "DoRemoveNode", Variant.From(in nodeId));
				Editor.UndoRedoManager.AddUndoMethod(this, "UndoRemoveNode", Variant.From(in from), Variant.From(in from2));
				Editor.UndoRedoManager.CommitAction();
			}
			else
			{
				DoRemoveNode(nodeId);
			}
		}
	}

	public Array GetNodeConnections(int nodeId)
	{
		Array array = new Array();
		if (!HasGraph())
		{
			return array;
		}
		foreach (XWBPNodeConnectionData connection in GraphData.Connections)
		{
			if (connection != null && (connection.FromNodeId == nodeId || connection.ToNodeId == nodeId))
			{
				array.Add(Variant.From<XWBPNodeConnectionData>(CloneConnectionData(connection)));
			}
		}
		return array;
	}

	public void DoRemoveNode(int nodeId)
	{
		if (!HasGraph() || !GraphNodeDictionary.TryGetValue(nodeId, out var value))
		{
			return;
		}
		foreach (Variant nodeConnection in GetNodeConnections(nodeId))
		{
			XWBPNodeConnectionData xWBPNodeConnectionData = nodeConnection.As<XWBPNodeConnectionData>();
			if (xWBPNodeConnectionData != null)
			{
				UndoRemoveConnection(xWBPNodeConnectionData.FromNodeId, xWBPNodeConnectionData.FromPortIndex, xWBPNodeConnectionData.ToNodeId, xWBPNodeConnectionData.ToPortIndex);
			}
		}
		GraphData.RemoveNode(nodeId);
		value.QueueFree();
		GraphNodeDictionary.Remove(nodeId);
		RefreshNavigationOverview();
	}

	public void UndoRemoveNode(XWBPNodeData nodeData, Array connections)
	{
		if (!HasGraph() || nodeData == null)
		{
			return;
		}
		DoCreateNode(nodeData);
		foreach (Variant connection in connections)
		{
			XWBPNodeConnectionData xWBPNodeConnectionData = connection.As<XWBPNodeConnectionData>();
			if (xWBPNodeConnectionData != null)
			{
				DoAddConnection(xWBPNodeConnectionData.FromNodeId, xWBPNodeConnectionData.FromPortIndex, xWBPNodeConnectionData.ToNodeId, xWBPNodeConnectionData.ToPortIndex);
			}
		}
	}

	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion inputEventMouseMotion)
		{
			_lastPointerGraphPosition = ToGraphPosition(inputEventMouseMotion.Position);
		}
		else if (@event is InputEventMouseButton inputEventMouseButton)
		{
			_lastPointerGraphPosition = ToGraphPosition(inputEventMouseButton.Position);
		}
		base._GuiInput(@event);
	}

	private void OnPopupRequest(Vector2 atPosition)
	{
		if (GodotObject.IsInstanceValid(_canvasContextMenu) && HasGraph())
		{
			_lastPointerGraphPosition = ToGraphPosition(atPosition);
			ConfigureCanvasContextMenu();
			Vector2 vector = GetScreenPosition() + atPosition;
			_canvasContextMenu.Position = new Vector2I(Mathf.RoundToInt(vector.X), Mathf.RoundToInt(vector.Y));
			_canvasContextMenu.Popup();
		}
	}

	private void ConfigureCanvasContextMenu()
	{
		if (GodotObject.IsInstanceValid(_canvasContextMenu))
		{
			bool flag = GetEditableSelectedNodeIds().Count > 0;
			SetCanvasMenuItemDisabled(100L, !HasGraph());
			SetCanvasMenuItemDisabled(200L, !flag);
			SetCanvasMenuItemDisabled(201L, !flag);
			SetCanvasMenuItemDisabled(202L, !flag);
			SetCanvasMenuItemDisabled(203L, _clipboardNodes.Count == 0);
			SetCanvasMenuItemDisabled(204L, !flag);
		}
	}

	private void SetCanvasMenuItemDisabled(long id, bool disabled)
	{
		int num = _canvasContextMenu?.GetItemIndex((int)id) ?? (-1);
		if (num >= 0)
		{
			_canvasContextMenu.SetItemDisabled(num, disabled);
		}
	}

	private void OnCanvasContextMenuIdPressed(long id)
	{
		if (id != 100)
		{
			long num = id - 200;
			if ((ulong)num <= 4uL)
			{
				switch ((int)num)
				{
				case 0:
					OnCopyNodesRequest();
					break;
				case 1:
					OnCutNodesRequest();
					break;
				case 2:
					OnDuplicateNodesRequest();
					break;
				case 3:
					OnPasteNodesRequest();
					break;
				case 4:
					OnDeleteNodesRequest(GetSelectedGraphNodeNames());
					break;
				}
			}
		}
		else
		{
			ShowNodeSelector(_lastPointerGraphPosition);
		}
	}

	private Array<StringName> GetSelectedGraphNodeNames()
	{
		Array<StringName> array = new Array<StringName>();
		foreach (XWBPGraphNode value in GraphNodeDictionary.Values)
		{
			if (GodotObject.IsInstanceValid(value) && value.Selected)
			{
				array.Add(value.Name);
			}
		}
		return array;
	}

	private void ShowNodeSelector(Vector2 position)
	{
		if (HasGraph())
		{
			XWWindowBPNodeSelector xWWindowBPNodeSelector = XWWindowBPNodeSelector.Create();
			xWWindowBPNodeSelector.Editor = Editor;
			xWWindowBPNodeSelector.CurrentGraph = GraphData;
			AddChild(xWWindowBPNodeSelector, forceReadableName: false, InternalMode.Disabled);
			xWWindowBPNodeSelector.NodeTypeSelected += (XWBPNodeType nodeType) =>
			{
				AddNode(nodeType, position);
			};
			xWWindowBPNodeSelector.PopupCentered();
		}
	}

	private Vector2 ToGraphPosition(Vector2 localPosition)
	{
		return ScrollOffset + localPosition / Mathf.Max(Zoom, 0.001f);
	}

	private void OnConnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
	{
		_connectionMadeDuringDrag = true;
		AddConnection(fromNode, (int)fromPort, toNode, (int)toPort);
	}

	private void OnDisconnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
	{
		if (_isDraggingConnection)
		{
			_disconnectionDuringDrag = true;
		}
		else
		{
			_recentlyDisconnected = true;
			CallDeferred("ResetRecentlyDisconnected");
		}
		RemoveConnection(fromNode, (int)fromPort, toNode, (int)toPort);
	}

	public void ResetRecentlyDisconnected()
	{
		_recentlyDisconnected = false;
	}

	private void OnConnectionDragStarted(StringName fromNode, long fromPort, bool isOutput)
	{
		_isDraggingConnection = true;
		_dragFromNode = fromNode;
		_dragFromPort = (int)fromPort;
		_dragIsOutput = isOutput;
		_connectionMadeDuringDrag = false;
		_disconnectionDuringDrag = _recentlyDisconnected;
		_recentlyDisconnected = false;
	}

	private void OnConnectionDragEnded()
	{
		if (!_isDraggingConnection)
		{
			return;
		}
		_isDraggingConnection = false;
		if (_connectionMadeDuringDrag || _disconnectionDuringDrag)
		{
			ResetConnectionDragState();
			return;
		}
		if (!HasGraph())
		{
			ResetConnectionDragState();
			return;
		}
		int graphNodeId = GetGraphNodeId(_dragFromNode);
		XWBPNodeData node = GraphData.GetNode(graphNodeId);
		if (!GodotObject.IsInstanceValid(node))
		{
			ResetConnectionDragState();
			return;
		}
		XWBPNodePortData xWBPNodePortData = (_dragIsOutput ? node.GetOutputPort(_dragFromPort) : node.GetInputPort(_dragFromPort));
		if (!GodotObject.IsInstanceValid(xWBPNodePortData))
		{
			ResetConnectionDragState();
			return;
		}
		ShowFilteredNodeSelector(graphNodeId, _dragFromPort, _dragIsOutput, xWBPNodePortData);
		ResetConnectionDragState();
	}

	private void ResetConnectionDragState()
	{
		_connectionMadeDuringDrag = false;
		_disconnectionDuringDrag = false;
		_recentlyDisconnected = false;
		_dragFromNode = "";
		_dragFromPort = -1;
	}

	private void ShowFilteredNodeSelector(int sourceNodeId, int sourcePortIndex, bool sourceIsOutput, XWBPNodePortData portData)
	{
		if (HasGraph())
		{
			XWWindowBPNodeSelector xWWindowBPNodeSelector = XWWindowBPNodeSelector.Create();
			xWWindowBPNodeSelector.Editor = Editor;
			xWWindowBPNodeSelector.CurrentGraph = GraphData;
			xWWindowBPNodeSelector.SetPortFilter(portData, sourceIsOutput);
			AddChild(xWWindowBPNodeSelector, forceReadableName: false, InternalMode.Disabled);
			xWWindowBPNodeSelector.NodeTypeSelected += (XWBPNodeType nodeType) =>
			{
				OnNodeSelectedFromPortDrag(nodeType, sourceNodeId, sourcePortIndex, sourceIsOutput);
			};
			xWWindowBPNodeSelector.PopupCentered();
		}
	}

	private void OnNodeSelectedFromPortDrag(XWBPNodeType nodeType, int sourceNodeId, int sourcePortIndex, bool sourceIsOutput)
	{
		if (!HasGraph() || nodeType == null)
		{
			return;
		}
		XWBPNodeData node = GraphData.GetNode(sourceNodeId);
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		XWBPNodePortData xWBPNodePortData = (sourceIsOutput ? node.GetOutputPort(sourcePortIndex) : node.GetInputPort(sourcePortIndex));
		if (!GodotObject.IsInstanceValid(xWBPNodePortData))
		{
			return;
		}
		int from = (sourceIsOutput ? FindCompatibleInputPort(nodeType, xWBPNodePortData) : FindCompatibleOutputPort(nodeType, xWBPNodePortData));
		XWBPNodeData from2 = nodeType.CreateNodeData();
		from2.NodeType = nodeType;
		from2.Position = ToGraphPosition(GetLocalMousePosition());
		if (from < 0)
		{
			CreateNodeWithUndo(from2);
			return;
		}
		ReserveNodeId(from2);
		if (HasUndoRedoManager())
		{
			Editor.CreateBlueprintAction("创建并连接节点");
			Editor.UndoRedoManager.AddDoMethod(this, "DoCreateAndConnectNode", Variant.From(in from2), Variant.From(in sourceNodeId), Variant.From(in sourcePortIndex), Variant.From(in sourceIsOutput), Variant.From(in from));
			Editor.UndoRedoManager.AddUndoMethod(this, "UndoCreateAndConnectNode", Variant.From(in from2), Variant.From(in sourceNodeId), Variant.From(in sourcePortIndex), Variant.From(in sourceIsOutput), Variant.From(in from));
			Editor.UndoRedoManager.CommitAction();
		}
		else
		{
			DoCreateAndConnectNode(from2, sourceNodeId, sourcePortIndex, sourceIsOutput, from);
		}
	}

	public void DoCreateAndConnectNode(XWBPNodeData nodeData, int sourceNodeId, int sourcePortIndex, bool sourceIsOutput, int targetPortIndex)
	{
		if (HasGraph() && nodeData != null)
		{
			DoCreateNode(nodeData);
			if (sourceIsOutput)
			{
				DoAddConnection(sourceNodeId, sourcePortIndex, nodeData.Id, targetPortIndex);
			}
			else
			{
				DoAddConnection(nodeData.Id, targetPortIndex, sourceNodeId, sourcePortIndex);
			}
		}
	}

	public void UndoCreateAndConnectNode(XWBPNodeData nodeData, int sourceNodeId, int sourcePortIndex, bool sourceIsOutput, int targetPortIndex)
	{
		if (nodeData != null)
		{
			if (sourceIsOutput)
			{
				UndoRemoveConnection(sourceNodeId, sourcePortIndex, nodeData.Id, targetPortIndex);
			}
			else
			{
				UndoRemoveConnection(nodeData.Id, targetPortIndex, sourceNodeId, sourcePortIndex);
			}
			UndoCreateNode(nodeData);
		}
	}

	private static int FindCompatibleInputPort(XWBPNodeType nodeType, XWBPNodePortData sourcePortData)
	{
		int result = -1;
		int num = 0;
		for (int i = 0; i < nodeType.InputPorts.Count; i++)
		{
			XWBPNodePortData xWBPNodePortData = nodeType.InputPorts[i];
			if (xWBPNodePortData != null && sourcePortData.CanConnectTo(xWBPNodePortData))
			{
				int num2 = ScorePortMatch(sourcePortData, xWBPNodePortData);
				if (num2 > num)
				{
					num = num2;
					result = i;
				}
			}
		}
		return result;
	}

	private static int FindCompatibleOutputPort(XWBPNodeType nodeType, XWBPNodePortData sourcePortData)
	{
		int result = -1;
		int num = 0;
		for (int i = 0; i < nodeType.OutputPorts.Count; i++)
		{
			XWBPNodePortData xWBPNodePortData = nodeType.OutputPorts[i];
			if (xWBPNodePortData != null && xWBPNodePortData.CanConnectTo(sourcePortData))
			{
				int num2 = ScorePortMatch(xWBPNodePortData, sourcePortData);
				if (num2 > num)
				{
					num = num2;
					result = i;
				}
			}
		}
		return result;
	}

	private static int ScorePortMatch(XWBPNodePortData fromPort, XWBPNodePortData toPort)
	{
		if (fromPort.PortTypeValue == toPort.PortTypeValue)
		{
			if (XWBPNodePortData.IsObjectPortType(fromPort.PortTypeValue) && fromPort.ClassName == toPort.ClassName)
			{
				return 4;
			}
			return 3;
		}
		if (fromPort.PortTypeValue != XWBPNodePortData.PortType.Any && toPort.PortTypeValue != XWBPNodePortData.PortType.Any)
		{
			return 2;
		}
		return 1;
	}

	public override bool _CanDropData(Vector2 atPosition, Variant data)
	{
		if (!HasGraph())
		{
			return false;
		}
		if (IsResourceDropData(data))
		{
			return true;
		}
		if (!XWDragData.IsValidDragData(data))
		{
			return false;
		}
		if (data.Obj is XWDragData xWDragData)
		{
			if (!xWDragData.IsType(XWDragData.Type.BpFunction) && !xWDragData.IsType(XWDragData.Type.BpVariable))
			{
				return xWDragData.IsType(XWDragData.Type.BpSignal);
			}
			return true;
		}
		return false;
	}

	public override void _DropData(Vector2 atPosition, Variant data)
	{
		if (!HasGraph())
		{
			return;
		}
		Vector2 graphPosition = ToGraphPosition(atPosition);
		if (IsResourceDropData(data))
		{
			DropResourceData(graphPosition, data);
		}
		else if (XWDragData.IsValidDragData(data) && data.Obj is XWDragData xWDragData)
		{
			if (xWDragData.IsType(XWDragData.Type.BpFunction))
			{
				DropFunctionData(graphPosition, xWDragData);
			}
			else if (xWDragData.IsType(XWDragData.Type.BpVariable))
			{
				DropVariableData(atPosition, graphPosition, xWDragData);
			}
			else if (xWDragData.IsType(XWDragData.Type.BpSignal))
			{
				DropSignalData(atPosition, graphPosition, xWDragData);
			}
		}
	}

	private static bool IsResourceDropData(Variant data)
	{
		foreach (string item in XWFileSystemDropHelper.ExtractDropPaths(data))
		{
			if (TryLoadDroppedResource(item) != null)
			{
				return true;
			}
		}
		return false;
	}

	private void DropResourceData(Vector2 graphPosition, Variant data)
	{
		int num = 0;
		foreach (string item in XWFileSystemDropHelper.ExtractDropPaths(data))
		{
			Resource resource = TryLoadDroppedResource(item);
			if (GodotObject.IsInstanceValid(resource))
			{
				XWBPNodeLoadResource xWBPNodeLoadResource = XWBPNodeLoadResource.CreateForPath(item, resource);
				if (xWBPNodeLoadResource != null)
				{
					AddNode(xWBPNodeLoadResource, graphPosition + new Vector2(32f * (float)num, 96f * (float)num));
					num++;
				}
			}
		}
	}

	private static Resource TryLoadDroppedResource(string path)
	{
		string text = XWBPNodeLoadResource.NormalizeResourcePath(path);
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		if (XWCSharpOnlyPolicy.IsUnsupportedScriptSourcePath(text))
		{
			return null;
		}
		if (DirAccess.DirExistsAbsolute(text))
		{
			return null;
		}
		if (!ResourceLoader.Exists(text))
		{
			return null;
		}
		Resource resource = ResourceLoader.Load<Resource>(text, null, ResourceLoader.CacheMode.Reuse);
		if (!XWCSharpOnlyPolicy.IsSupportedModScriptResource(resource))
		{
			return null;
		}
		return resource;
	}

	private void DropFunctionData(Vector2 graphPosition, XWDragData dragData)
	{
		if (dragData.Data.Obj is XWBPFunctionData xWBPFunctionData)
		{
			XWBPNodeCallMethod xWBPNodeCallMethod = new XWBPNodeCallMethod
			{
				MethodType = XWBPNodeCallMethod.Type.Bp,
				FunctionId = xWBPFunctionData.Id
			};
			xWBPNodeCallMethod.BuildFunction(xWBPFunctionData);
			AddNode(xWBPNodeCallMethod, graphPosition);
		}
	}

	private void DropVariableData(Vector2 localPosition, Vector2 graphPosition, XWDragData dragData)
	{
		if (dragData.Data.Obj is XWBPVariableData pendingVariableData)
		{
			_pendingVariableData = pendingVariableData;
			_pendingDropPosition = graphPosition;
			PopupAtLocalPosition(_variablePopupMenu, localPosition);
		}
	}

	private void OnVariablePopupMenuIdPressed(long id)
	{
		if (GodotObject.IsInstanceValid(_pendingVariableData))
		{
			switch (id)
			{
			case 0L:
				CreateGetNode(_pendingVariableData, _pendingDropPosition);
				break;
			case 1L:
				CreateSetNode(_pendingVariableData, _pendingDropPosition);
				break;
			}
			_pendingVariableData = null;
			_pendingDropPosition = Vector2.Zero;
		}
	}

	private void CreateGetNode(XWBPVariableData variableData, Vector2 graphPosition)
	{
		XWBPNodeGetProperty xWBPNodeGetProperty = new XWBPNodeGetProperty
		{
			MethodType = XWBPNodeGetProperty.Type.Bp,
			VariableId = variableData.Id
		};
		xWBPNodeGetProperty.BuildVariable(variableData);
		AddNode(xWBPNodeGetProperty, graphPosition);
	}

	private void CreateSetNode(XWBPVariableData variableData, Vector2 graphPosition)
	{
		XWBPNodeSetProperty xWBPNodeSetProperty = new XWBPNodeSetProperty
		{
			MethodType = XWBPNodeSetProperty.Type.Bp,
			VariableId = variableData.Id
		};
		xWBPNodeSetProperty.BuildVariable(variableData);
		AddNode(xWBPNodeSetProperty, graphPosition);
	}

	private void DropSignalData(Vector2 localPosition, Vector2 graphPosition, XWDragData dragData)
	{
		if (dragData.Data.Obj is XWBPSignalData pendingSignalData)
		{
			_pendingSignalData = pendingSignalData;
			_pendingDropPosition = graphPosition;
			PopupAtLocalPosition(_signalPopupMenu, localPosition);
		}
	}

	private void OnSignalPopupMenuIdPressed(long id)
	{
		if (GodotObject.IsInstanceValid(_pendingSignalData))
		{
			switch (id)
			{
			case 0L:
				CreateSignalEventNode(_pendingSignalData, _pendingDropPosition);
				break;
			case 1L:
				CreateEmitSignalNode(_pendingSignalData, _pendingDropPosition);
				break;
			}
			_pendingSignalData = null;
			_pendingDropPosition = Vector2.Zero;
		}
	}

	private void CreateSignalEventNode(XWBPSignalData signalData, Vector2 graphPosition)
	{
		XWBPNodeSignalEvent xWBPNodeSignalEvent = new XWBPNodeSignalEvent
		{
			SignalId = signalData.Id
		};
		xWBPNodeSignalEvent.BuildSignal(signalData);
		AddNode(xWBPNodeSignalEvent, graphPosition);
	}

	private void CreateEmitSignalNode(XWBPSignalData signalData, Vector2 graphPosition)
	{
		XWBPNodeEmitSignal xWBPNodeEmitSignal = new XWBPNodeEmitSignal
		{
			SignalId = signalData.Id
		};
		xWBPNodeEmitSignal.BuildSignal(signalData);
		AddNode(xWBPNodeEmitSignal, graphPosition);
	}

	private void PopupAtLocalPosition(PopupMenu popupMenu, Vector2 localPosition)
	{
		if (GodotObject.IsInstanceValid(popupMenu))
		{
			popupMenu.Position = new Vector2I((int)((float)GetWindow().Position.X + GlobalPosition.X + localPosition.X), (int)((float)GetWindow().Position.Y + GlobalPosition.Y + localPosition.Y));
			popupMenu.Popup();
		}
	}

	private List<int> GetEditableSelectedNodeIds()
	{
		List<int> list = new List<int>();
		if (!HasGraph())
		{
			return list;
		}
		foreach (KeyValuePair<int, XWBPGraphNode> item in GraphNodeDictionary)
		{
			if (GodotObject.IsInstanceValid(item.Value) && item.Value.Selected)
			{
				XWBPNodeData node = GraphData.GetNode(item.Key);
				if (node != null && !node.Lock)
				{
					list.Add(item.Key);
				}
			}
		}
		list.Sort();
		return list;
	}

	private void OnCopyNodesRequest()
	{
		CopySelectedNodes();
	}

	private void OnCutNodesRequest()
	{
		CutSelectedNodes();
	}

	private void OnPasteNodesRequest()
	{
		PasteNodes(_lastPointerGraphPosition);
	}

	private void OnDuplicateNodesRequest()
	{
		DuplicateSelectedNodes();
	}

	public void CopySelectedNodes()
	{
		List<int> editableSelectedNodeIds = GetEditableSelectedNodeIds();
		if (editableSelectedNodeIds.Count != 0)
		{
			CopyNodeIdsToClipboard(editableSelectedNodeIds);
		}
	}

	private void CopyNodeIdsToClipboard(List<int> selectedIds)
	{
		_clipboardNodes.Clear();
		_clipboardConnections.Clear();
		System.Collections.Generic.Dictionary<int, int> dictionary = new System.Collections.Generic.Dictionary<int, int>();
		for (int i = 0; i < selectedIds.Count; i++)
		{
			XWBPNodeData node = GraphData.GetNode(selectedIds[i]);
			if (node != null && !node.Lock)
			{
				XWBPNodeData xWBPNodeData = CloneNodeData(node);
				if (xWBPNodeData != null)
				{
					dictionary[selectedIds[i]] = _clipboardNodes.Count;
					_clipboardNodes.Add(xWBPNodeData);
				}
			}
		}
		foreach (XWBPNodeConnectionData connection in GraphData.Connections)
		{
			if (connection != null && dictionary.TryGetValue(connection.FromNodeId, out var value) && dictionary.TryGetValue(connection.ToNodeId, out var value2))
			{
				_clipboardConnections.Add((value, connection.FromPortIndex, value2, connection.ToPortIndex));
			}
		}
	}

	public void CutSelectedNodes()
	{
		List<int> editableSelectedNodeIds = GetEditableSelectedNodeIds();
		if (editableSelectedNodeIds.Count != 0)
		{
			CopyNodeIdsToClipboard(editableSelectedNodeIds);
			RemoveNodesWithUndo(editableSelectedNodeIds, "剪切节点");
		}
	}

	public void PasteNodes(Vector2 position, string actionName = "粘贴节点")
	{
		if (_clipboardNodes.Count == 0 || !HasGraph())
		{
			return;
		}
		Vector2 position2 = _clipboardNodes[0].Position;
		foreach (XWBPNodeData clipboardNode in _clipboardNodes)
		{
			if (clipboardNode.Position.X < position2.X)
			{
				position2.X = clipboardNode.Position.X;
			}
			if (clipboardNode.Position.Y < position2.Y)
			{
				position2.Y = clipboardNode.Position.Y;
			}
		}
		Vector2 vector = position - position2;
		System.Collections.Generic.Dictionary<int, int> dictionary = new System.Collections.Generic.Dictionary<int, int>();
		Array from = new Array();
		for (int i = 0; i < _clipboardNodes.Count; i++)
		{
			XWBPNodeData from2 = CloneNodeData(_clipboardNodes[i]);
			if (from2 != null)
			{
				from2.Position = _clipboardNodes[i].Position + vector;
				ReserveNodeId(from2);
				dictionary[i] = from2.Id;
				from.Add(Variant.From(in from2));
			}
		}
		Array from3 = new Array();
		foreach (var clipboardConnection in _clipboardConnections)
		{
			if (dictionary.TryGetValue(clipboardConnection.fromLocalIdx, out var value) && dictionary.TryGetValue(clipboardConnection.toLocalIdx, out var value2))
			{
				from3.Add(Variant.From<XWBPNodeConnectionData>(new XWBPNodeConnectionData(value, clipboardConnection.fromPort, value2, clipboardConnection.toPort)));
			}
		}
		if (from.Count != 0)
		{
			if (HasUndoRedoManager())
			{
				Editor.CreateBlueprintAction(actionName);
				Editor.UndoRedoManager.AddDoMethod(this, "DoPasteNodes", Variant.From(in from), Variant.From(in from3));
				Editor.UndoRedoManager.AddUndoMethod(this, "UndoPasteNodes", Variant.From(in from), Variant.From(in from3));
				Editor.UndoRedoManager.CommitAction();
			}
			else
			{
				DoPasteNodes(from, from3);
			}
		}
	}

	public void DoPasteNodes(Array nodes, Array connections)
	{
		if (!HasGraph())
		{
			return;
		}
		DeselectAllGraphNodes();
		foreach (Variant node in nodes)
		{
			XWBPNodeData xWBPNodeData = node.As<XWBPNodeData>();
			if (xWBPNodeData != null)
			{
				DoCreateNode(xWBPNodeData);
				XWBPGraphNode graphNode = GetGraphNode(xWBPNodeData.Id);
				if (graphNode != null)
				{
					graphNode.Selected = true;
				}
			}
		}
		foreach (Variant connection in connections)
		{
			XWBPNodeConnectionData xWBPNodeConnectionData = connection.As<XWBPNodeConnectionData>();
			if (xWBPNodeConnectionData != null)
			{
				DoAddConnection(xWBPNodeConnectionData.FromNodeId, xWBPNodeConnectionData.FromPortIndex, xWBPNodeConnectionData.ToNodeId, xWBPNodeConnectionData.ToPortIndex);
			}
		}
		RefreshNavigationOverview();
	}

	public void UndoPasteNodes(Array nodes, Array connections)
	{
		if (!HasGraph())
		{
			return;
		}
		for (int num = nodes.Count - 1; num >= 0; num--)
		{
			XWBPNodeData xWBPNodeData = nodes[num].As<XWBPNodeData>();
			if (xWBPNodeData != null)
			{
				DoRemoveNode(xWBPNodeData.Id);
			}
		}
	}

	public void DuplicateSelectedNodes()
	{
		List<int> editableSelectedNodeIds = GetEditableSelectedNodeIds();
		if (editableSelectedNodeIds.Count == 0)
		{
			return;
		}
		CopyNodeIdsToClipboard(editableSelectedNodeIds);
		if (_clipboardNodes.Count == 0)
		{
			return;
		}
		Vector2 position = _clipboardNodes[0].Position;
		foreach (XWBPNodeData clipboardNode in _clipboardNodes)
		{
			if (clipboardNode.Position.X < position.X)
			{
				position.X = clipboardNode.Position.X;
			}
			if (clipboardNode.Position.Y < position.Y)
			{
				position.Y = clipboardNode.Position.Y;
			}
		}
		PasteNodes(position + new Vector2(40f, 40f), "重复节点");
	}

	private void DeselectAllGraphNodes()
	{
		foreach (XWBPGraphNode value in GraphNodeDictionary.Values)
		{
			if (GodotObject.IsInstanceValid(value))
			{
				value.Selected = false;
			}
		}
	}

	private static XWBPNodeData CloneNodeData(XWBPNodeData source)
	{
		if (source == null)
		{
			return null;
		}
		XWBPNodeData xWBPNodeData = new XWBPNodeData
		{
			Id = 0,
			Lock = source.Lock,
			TypeId = source.TypeId,
			Position = source.Position,
			Size = source.Size,
			MetaData = new Dictionary()
		};
		if (source.MetaData != null)
		{
			foreach (KeyValuePair<Variant, Variant> item in source.MetaData)
			{
				xWBPNodeData.MetaData[item.Key] = item.Value;
			}
		}
		foreach (XWBPNodePortData inputPort in source.InputPorts)
		{
			if (inputPort != null)
			{
				xWBPNodeData.InputPorts.Add(new XWBPNodePortData(inputPort.Name, inputPort.PortDirection, inputPort.PortTypeValue, inputPort.ClassName, inputPort.DefaultValue));
			}
		}
		foreach (XWBPNodePortData outputPort in source.OutputPorts)
		{
			if (outputPort != null)
			{
				xWBPNodeData.OutputPorts.Add(new XWBPNodePortData(outputPort.Name, outputPort.PortDirection, outputPort.PortTypeValue, outputPort.ClassName, outputPort.DefaultValue));
			}
		}
		xWBPNodeData.RebuildPortMap();
		return xWBPNodeData;
	}

	private XWBPGraphNode AddNodeFromData(XWBPNodeData nodeData)
	{
		if (!HasGraph() || nodeData == null)
		{
			return null;
		}
		return CreateNodeWithUndo(nodeData);
	}

	public void FocusNode(int nodeId)
	{
		if (GraphNodeDictionary.TryGetValue(nodeId, out var value))
		{
			value.Selected = true;
			Vector2 positionOffset = value.PositionOffset;
			Vector2 size = Size;
			ScrollOffset = positionOffset - size * 0.5f + value.Size * 0.5f;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(103)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasGraph, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasUndoRedoManager, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateExtendsClassUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateNavigationUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildCanvasContextMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCanvasContextMenu, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMinimapToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetZoom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FocusSelectedNode, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshNavigationOverview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGraphNodeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnGraphNodeDeselected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "_", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateExtendsClassLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExtendsClassButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnExtendsClassSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "className", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GenerateScriptButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunPreviewButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetRunPreviewButton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPreviewStatusLabel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetRuntimePreviewState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "running", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "accent", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HighlightRuntimeNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearRuntimeHighlight, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Init, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "graphData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.InitNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitConnections, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsConnectionDrawable, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "connection", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetGraphNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshNodeTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshPortEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "port", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetGraphNodeId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetGraphNodeByName, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReserveNodeId, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasConnection, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "fromNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneConnectionData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "connection", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName._IsNodeHoverValid, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "fromNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "toNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDeleteNodesRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "nodes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoRemoveNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "nodes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UndoRemoveNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "nodes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "connections", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddConnection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "fromNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "toNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoAddConnection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "fromNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveConnection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "fromNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "toNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UndoRemoveConnection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "fromNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "typeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateNodeWithUndo, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoCreateNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.UndoCreateNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveGraphNodeWithName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveGraphNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetNodeConnections, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoRemoveNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UndoRemoveNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Array, "connections", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._GuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnPopupRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureCanvasContextMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetCanvasMenuItemDisabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "disabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCanvasContextMenuIdPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedGraphNodeNames, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowNodeSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToGraphPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "localPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnConnectionRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "fromNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "toNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDisconnectionRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "fromNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "toNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "toPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetRecentlyDisconnected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnConnectionDragStarted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "fromNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "fromPort", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isOutput", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnConnectionDragEnded, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetConnectionDragState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowFilteredNodeSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "sourceNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sourcePortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "sourceIsOutput", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "portData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnNodeSelectedFromPortDrag, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeType", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "sourceNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sourcePortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "sourceIsOutput", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoCreateAndConnectNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "sourceNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sourcePortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "sourceIsOutput", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "targetPortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UndoCreateAndConnectNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Int, "sourceNodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "sourcePortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "sourceIsOutput", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "targetPortIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindCompatibleInputPort, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeType", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "sourcePortData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindCompatibleOutputPort, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeType", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "sourcePortData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.ScorePortMatch, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fromPort", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "toPort", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
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
			new MethodInfo(MethodName.IsResourceDropData, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.DropResourceData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "graphPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "data", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.TryLoadDroppedResource, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DropFunctionData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "graphPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "dragData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.DropVariableData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "localPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "graphPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "dragData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnVariablePopupMenuIdPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateGetNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "variableData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "graphPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSetNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "variableData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "graphPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DropSignalData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "localPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "graphPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "dragData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnSignalPopupMenuIdPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSignalEventNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "signalData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "graphPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateEmitSignalNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "signalData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "graphPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopupAtLocalPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "popupMenu", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "localPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCopyNodesRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCutNodesRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPasteNodesRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDuplicateNodesRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CopySelectedNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CutSelectedNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PasteNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DoPasteNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "nodes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "connections", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UndoPasteNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "nodes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Array, "connections", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DuplicateSelectedNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DeselectAllGraphNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloneNodeData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddNodeFromData, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "nodeData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.FocusNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "nodeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.HasGraph && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasGraph());
			return true;
		}
		if (method == MethodName.HasUndoRedoManager && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasUndoRedoManager());
			return true;
		}
		if (method == MethodName.CreateExtendsClassUI && args.Count == 0)
		{
			CreateExtendsClassUI();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateNavigationUI && args.Count == 0)
		{
			CreateNavigationUI();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildCanvasContextMenu && args.Count == 0)
		{
			BuildCanvasContextMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCanvasContextMenu && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PopupMenu>(GetCanvasContextMenu());
			return true;
		}
		if (method == MethodName.OnMinimapToggled && args.Count == 1)
		{
			OnMinimapToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetZoom && args.Count == 0)
		{
			ResetZoom();
			ret = default;
			return true;
		}
		if (method == MethodName.FocusSelectedNode && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(FocusSelectedNode());
			return true;
		}
		if (method == MethodName.RefreshNavigationOverview && args.Count == 0)
		{
			RefreshNavigationOverview();
			ret = default;
			return true;
		}
		if (method == MethodName.OnGraphNodeSelected && args.Count == 1)
		{
			OnGraphNodeSelected(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnGraphNodeDeselected && args.Count == 1)
		{
			OnGraphNodeDeselected(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateExtendsClassLabel && args.Count == 0)
		{
			UpdateExtendsClassLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.ExtendsClassButtonPressed && args.Count == 0)
		{
			ExtendsClassButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnExtendsClassSelected && args.Count == 1)
		{
			OnExtendsClassSelected(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GenerateScriptButtonPressed && args.Count == 0)
		{
			GenerateScriptButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.RunPreviewButtonPressed && args.Count == 0)
		{
			RunPreviewButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.GetRunPreviewButton && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Button>(GetRunPreviewButton());
			return true;
		}
		if (method == MethodName.GetPreviewStatusLabel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Label>(GetPreviewStatusLabel());
			return true;
		}
		if (method == MethodName.SetRuntimePreviewState && args.Count == 4)
		{
			SetRuntimePreviewState(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Color>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.HighlightRuntimeNode && args.Count == 1)
		{
			HighlightRuntimeNode(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearRuntimeHighlight && args.Count == 0)
		{
			ClearRuntimeHighlight();
			ret = default;
			return true;
		}
		if (method == MethodName.Init && args.Count == 1)
		{
			Init(VariantUtils.ConvertTo<XWBPGraphData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitNodes && args.Count == 0)
		{
			InitNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.InitConnections && args.Count == 0)
		{
			InitConnections();
			ret = default;
			return true;
		}
		if (method == MethodName.IsConnectionDrawable && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsConnectionDrawable(VariantUtils.ConvertTo<XWBPNodeConnectionData>(in args[0])));
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.GetGraphNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphNode>(GetGraphNode(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshNodeTransform && args.Count == 1)
		{
			RefreshNodeTransform(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPortEditor && args.Count == 1)
		{
			RefreshPortEditor(VariantUtils.ConvertTo<XWBPNodePortData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetGraphNodeId && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetGraphNodeId(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.GetGraphNodeByName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphNode>(GetGraphNodeByName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName.ReserveNodeId && args.Count == 1)
		{
			ReserveNodeId(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasConnection && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(HasConnection(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		if (method == MethodName.CloneConnectionData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeConnectionData>(CloneConnectionData(VariantUtils.ConvertTo<XWBPNodeConnectionData>(in args[0])));
			return true;
		}
		if (method == MethodName._IsNodeHoverValid && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<bool>(_IsNodeHoverValid(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		if (method == MethodName.OnDeleteNodesRequest && args.Count == 1)
		{
			OnDeleteNodesRequest(VariantUtils.ConvertToArray<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DoRemoveNodes && args.Count == 1)
		{
			DoRemoveNodes(VariantUtils.ConvertTo<Array>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UndoRemoveNodes && args.Count == 2)
		{
			UndoRemoveNodes(VariantUtils.ConvertTo<Array>(in args[0]), VariantUtils.ConvertTo<Array>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddConnection && args.Count == 4)
		{
			AddConnection(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.DoAddConnection && args.Count == 4)
		{
			DoAddConnection(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveConnection && args.Count == 4)
		{
			RemoveConnection(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.UndoRemoveConnection && args.Count == 4)
		{
			UndoRemoveConnection(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddNode && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphNode>(AddNode(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateNodeWithUndo && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphNode>(CreateNodeWithUndo(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.DoCreateNode && args.Count == 1)
		{
			DoCreateNode(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UndoCreateNode && args.Count == 1)
		{
			UndoCreateNode(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveGraphNodeWithName && args.Count == 1)
		{
			RemoveGraphNodeWithName(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveGraphNode && args.Count == 1)
		{
			RemoveGraphNode(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetNodeConnections && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Array>(GetNodeConnections(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.DoRemoveNode && args.Count == 1)
		{
			DoRemoveNode(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UndoRemoveNode && args.Count == 2)
		{
			UndoRemoveNode(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]), VariantUtils.ConvertTo<Array>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName._GuiInput && args.Count == 1)
		{
			_GuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPopupRequest && args.Count == 1)
		{
			OnPopupRequest(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureCanvasContextMenu && args.Count == 0)
		{
			ConfigureCanvasContextMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.SetCanvasMenuItemDisabled && args.Count == 2)
		{
			SetCanvasMenuItemDisabled(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCanvasContextMenuIdPressed && args.Count == 1)
		{
			OnCanvasContextMenuIdPressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedGraphNodeNames && args.Count == 0)
		{
			Array<StringName> selectedGraphNodeNames = GetSelectedGraphNodeNames();
			ret = VariantUtils.CreateFromArray(selectedGraphNodeNames);
			return true;
		}
		if (method == MethodName.ShowNodeSelector && args.Count == 1)
		{
			ShowNodeSelector(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToGraphPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ToGraphPosition(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.OnConnectionRequest && args.Count == 4)
		{
			OnConnectionRequest(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<long>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDisconnectionRequest && args.Count == 4)
		{
			OnDisconnectionRequest(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<long>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetRecentlyDisconnected && args.Count == 0)
		{
			ResetRecentlyDisconnected();
			ret = default;
			return true;
		}
		if (method == MethodName.OnConnectionDragStarted && args.Count == 3)
		{
			OnConnectionDragStarted(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnConnectionDragEnded && args.Count == 0)
		{
			OnConnectionDragEnded();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetConnectionDragState && args.Count == 0)
		{
			ResetConnectionDragState();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowFilteredNodeSelector && args.Count == 4)
		{
			ShowFilteredNodeSelector(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<XWBPNodePortData>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnNodeSelectedFromPortDrag && args.Count == 4)
		{
			OnNodeSelectedFromPortDrag(VariantUtils.ConvertTo<XWBPNodeType>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.DoCreateAndConnectNode && args.Count == 5)
		{
			DoCreateAndConnectNode(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.UndoCreateAndConnectNode && args.Count == 5)
		{
			UndoCreateAndConnectNode(VariantUtils.ConvertTo<XWBPNodeData>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]), VariantUtils.ConvertTo<int>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindCompatibleInputPort && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindCompatibleInputPort(VariantUtils.ConvertTo<XWBPNodeType>(in args[0]), VariantUtils.ConvertTo<XWBPNodePortData>(in args[1])));
			return true;
		}
		if (method == MethodName.FindCompatibleOutputPort && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindCompatibleOutputPort(VariantUtils.ConvertTo<XWBPNodeType>(in args[0]), VariantUtils.ConvertTo<XWBPNodePortData>(in args[1])));
			return true;
		}
		if (method == MethodName.ScorePortMatch && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ScorePortMatch(VariantUtils.ConvertTo<XWBPNodePortData>(in args[0]), VariantUtils.ConvertTo<XWBPNodePortData>(in args[1])));
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
		if (method == MethodName.IsResourceDropData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsResourceDropData(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.DropResourceData && args.Count == 2)
		{
			DropResourceData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryLoadDroppedResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(TryLoadDroppedResource(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DropFunctionData && args.Count == 2)
		{
			DropFunctionData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<XWDragData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DropVariableData && args.Count == 3)
		{
			DropVariableData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<XWDragData>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnVariablePopupMenuIdPressed && args.Count == 1)
		{
			OnVariablePopupMenuIdPressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateGetNode && args.Count == 2)
		{
			CreateGetNode(VariantUtils.ConvertTo<XWBPVariableData>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSetNode && args.Count == 2)
		{
			CreateSetNode(VariantUtils.ConvertTo<XWBPVariableData>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DropSignalData && args.Count == 3)
		{
			DropSignalData(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<XWDragData>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSignalPopupMenuIdPressed && args.Count == 1)
		{
			OnSignalPopupMenuIdPressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSignalEventNode && args.Count == 2)
		{
			CreateSignalEventNode(VariantUtils.ConvertTo<XWBPSignalData>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateEmitSignalNode && args.Count == 2)
		{
			CreateEmitSignalNode(VariantUtils.ConvertTo<XWBPSignalData>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopupAtLocalPosition && args.Count == 2)
		{
			PopupAtLocalPosition(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCopyNodesRequest && args.Count == 0)
		{
			OnCopyNodesRequest();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCutNodesRequest && args.Count == 0)
		{
			OnCutNodesRequest();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPasteNodesRequest && args.Count == 0)
		{
			OnPasteNodesRequest();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDuplicateNodesRequest && args.Count == 0)
		{
			OnDuplicateNodesRequest();
			ret = default;
			return true;
		}
		if (method == MethodName.CopySelectedNodes && args.Count == 0)
		{
			CopySelectedNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.CutSelectedNodes && args.Count == 0)
		{
			CutSelectedNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.PasteNodes && args.Count == 2)
		{
			PasteNodes(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DoPasteNodes && args.Count == 2)
		{
			DoPasteNodes(VariantUtils.ConvertTo<Array>(in args[0]), VariantUtils.ConvertTo<Array>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UndoPasteNodes && args.Count == 2)
		{
			UndoPasteNodes(VariantUtils.ConvertTo<Array>(in args[0]), VariantUtils.ConvertTo<Array>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DuplicateSelectedNodes && args.Count == 0)
		{
			DuplicateSelectedNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.DeselectAllGraphNodes && args.Count == 0)
		{
			DeselectAllGraphNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.CloneNodeData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(CloneNodeData(VariantUtils.ConvertTo<XWBPNodeData>(in args[0])));
			return true;
		}
		if (method == MethodName.AddNodeFromData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPGraphNode>(AddNodeFromData(VariantUtils.ConvertTo<XWBPNodeData>(in args[0])));
			return true;
		}
		if (method == MethodName.FocusNode && args.Count == 1)
		{
			FocusNode(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CloneConnectionData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeConnectionData>(CloneConnectionData(VariantUtils.ConvertTo<XWBPNodeConnectionData>(in args[0])));
			return true;
		}
		if (method == MethodName.FindCompatibleInputPort && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindCompatibleInputPort(VariantUtils.ConvertTo<XWBPNodeType>(in args[0]), VariantUtils.ConvertTo<XWBPNodePortData>(in args[1])));
			return true;
		}
		if (method == MethodName.FindCompatibleOutputPort && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindCompatibleOutputPort(VariantUtils.ConvertTo<XWBPNodeType>(in args[0]), VariantUtils.ConvertTo<XWBPNodePortData>(in args[1])));
			return true;
		}
		if (method == MethodName.ScorePortMatch && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(ScorePortMatch(VariantUtils.ConvertTo<XWBPNodePortData>(in args[0]), VariantUtils.ConvertTo<XWBPNodePortData>(in args[1])));
			return true;
		}
		if (method == MethodName.IsResourceDropData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsResourceDropData(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.TryLoadDroppedResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(TryLoadDroppedResource(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CloneNodeData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWBPNodeData>(CloneNodeData(VariantUtils.ConvertTo<XWBPNodeData>(in args[0])));
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
		if (method == MethodName.HasGraph)
		{
			return true;
		}
		if (method == MethodName.HasUndoRedoManager)
		{
			return true;
		}
		if (method == MethodName.CreateExtendsClassUI)
		{
			return true;
		}
		if (method == MethodName.CreateNavigationUI)
		{
			return true;
		}
		if (method == MethodName.BuildCanvasContextMenu)
		{
			return true;
		}
		if (method == MethodName.GetCanvasContextMenu)
		{
			return true;
		}
		if (method == MethodName.OnMinimapToggled)
		{
			return true;
		}
		if (method == MethodName.ResetZoom)
		{
			return true;
		}
		if (method == MethodName.FocusSelectedNode)
		{
			return true;
		}
		if (method == MethodName.RefreshNavigationOverview)
		{
			return true;
		}
		if (method == MethodName.OnGraphNodeSelected)
		{
			return true;
		}
		if (method == MethodName.OnGraphNodeDeselected)
		{
			return true;
		}
		if (method == MethodName.UpdateExtendsClassLabel)
		{
			return true;
		}
		if (method == MethodName.ExtendsClassButtonPressed)
		{
			return true;
		}
		if (method == MethodName.OnExtendsClassSelected)
		{
			return true;
		}
		if (method == MethodName.GenerateScriptButtonPressed)
		{
			return true;
		}
		if (method == MethodName.RunPreviewButtonPressed)
		{
			return true;
		}
		if (method == MethodName.GetRunPreviewButton)
		{
			return true;
		}
		if (method == MethodName.GetPreviewStatusLabel)
		{
			return true;
		}
		if (method == MethodName.SetRuntimePreviewState)
		{
			return true;
		}
		if (method == MethodName.HighlightRuntimeNode)
		{
			return true;
		}
		if (method == MethodName.ClearRuntimeHighlight)
		{
			return true;
		}
		if (method == MethodName.Init)
		{
			return true;
		}
		if (method == MethodName.InitNodes)
		{
			return true;
		}
		if (method == MethodName.InitConnections)
		{
			return true;
		}
		if (method == MethodName.IsConnectionDrawable)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.GetGraphNode)
		{
			return true;
		}
		if (method == MethodName.RefreshNodeTransform)
		{
			return true;
		}
		if (method == MethodName.RefreshPortEditor)
		{
			return true;
		}
		if (method == MethodName.GetGraphNodeId)
		{
			return true;
		}
		if (method == MethodName.GetGraphNodeByName)
		{
			return true;
		}
		if (method == MethodName.ReserveNodeId)
		{
			return true;
		}
		if (method == MethodName.HasConnection)
		{
			return true;
		}
		if (method == MethodName.CloneConnectionData)
		{
			return true;
		}
		if (method == MethodName._IsNodeHoverValid)
		{
			return true;
		}
		if (method == MethodName.OnDeleteNodesRequest)
		{
			return true;
		}
		if (method == MethodName.DoRemoveNodes)
		{
			return true;
		}
		if (method == MethodName.UndoRemoveNodes)
		{
			return true;
		}
		if (method == MethodName.AddConnection)
		{
			return true;
		}
		if (method == MethodName.DoAddConnection)
		{
			return true;
		}
		if (method == MethodName.RemoveConnection)
		{
			return true;
		}
		if (method == MethodName.UndoRemoveConnection)
		{
			return true;
		}
		if (method == MethodName.AddNode)
		{
			return true;
		}
		if (method == MethodName.CreateNodeWithUndo)
		{
			return true;
		}
		if (method == MethodName.DoCreateNode)
		{
			return true;
		}
		if (method == MethodName.UndoCreateNode)
		{
			return true;
		}
		if (method == MethodName.RemoveGraphNodeWithName)
		{
			return true;
		}
		if (method == MethodName.RemoveGraphNode)
		{
			return true;
		}
		if (method == MethodName.GetNodeConnections)
		{
			return true;
		}
		if (method == MethodName.DoRemoveNode)
		{
			return true;
		}
		if (method == MethodName.UndoRemoveNode)
		{
			return true;
		}
		if (method == MethodName._GuiInput)
		{
			return true;
		}
		if (method == MethodName.OnPopupRequest)
		{
			return true;
		}
		if (method == MethodName.ConfigureCanvasContextMenu)
		{
			return true;
		}
		if (method == MethodName.SetCanvasMenuItemDisabled)
		{
			return true;
		}
		if (method == MethodName.OnCanvasContextMenuIdPressed)
		{
			return true;
		}
		if (method == MethodName.GetSelectedGraphNodeNames)
		{
			return true;
		}
		if (method == MethodName.ShowNodeSelector)
		{
			return true;
		}
		if (method == MethodName.ToGraphPosition)
		{
			return true;
		}
		if (method == MethodName.OnConnectionRequest)
		{
			return true;
		}
		if (method == MethodName.OnDisconnectionRequest)
		{
			return true;
		}
		if (method == MethodName.ResetRecentlyDisconnected)
		{
			return true;
		}
		if (method == MethodName.OnConnectionDragStarted)
		{
			return true;
		}
		if (method == MethodName.OnConnectionDragEnded)
		{
			return true;
		}
		if (method == MethodName.ResetConnectionDragState)
		{
			return true;
		}
		if (method == MethodName.ShowFilteredNodeSelector)
		{
			return true;
		}
		if (method == MethodName.OnNodeSelectedFromPortDrag)
		{
			return true;
		}
		if (method == MethodName.DoCreateAndConnectNode)
		{
			return true;
		}
		if (method == MethodName.UndoCreateAndConnectNode)
		{
			return true;
		}
		if (method == MethodName.FindCompatibleInputPort)
		{
			return true;
		}
		if (method == MethodName.FindCompatibleOutputPort)
		{
			return true;
		}
		if (method == MethodName.ScorePortMatch)
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
		if (method == MethodName.IsResourceDropData)
		{
			return true;
		}
		if (method == MethodName.DropResourceData)
		{
			return true;
		}
		if (method == MethodName.TryLoadDroppedResource)
		{
			return true;
		}
		if (method == MethodName.DropFunctionData)
		{
			return true;
		}
		if (method == MethodName.DropVariableData)
		{
			return true;
		}
		if (method == MethodName.OnVariablePopupMenuIdPressed)
		{
			return true;
		}
		if (method == MethodName.CreateGetNode)
		{
			return true;
		}
		if (method == MethodName.CreateSetNode)
		{
			return true;
		}
		if (method == MethodName.DropSignalData)
		{
			return true;
		}
		if (method == MethodName.OnSignalPopupMenuIdPressed)
		{
			return true;
		}
		if (method == MethodName.CreateSignalEventNode)
		{
			return true;
		}
		if (method == MethodName.CreateEmitSignalNode)
		{
			return true;
		}
		if (method == MethodName.PopupAtLocalPosition)
		{
			return true;
		}
		if (method == MethodName.OnCopyNodesRequest)
		{
			return true;
		}
		if (method == MethodName.OnCutNodesRequest)
		{
			return true;
		}
		if (method == MethodName.OnPasteNodesRequest)
		{
			return true;
		}
		if (method == MethodName.OnDuplicateNodesRequest)
		{
			return true;
		}
		if (method == MethodName.CopySelectedNodes)
		{
			return true;
		}
		if (method == MethodName.CutSelectedNodes)
		{
			return true;
		}
		if (method == MethodName.PasteNodes)
		{
			return true;
		}
		if (method == MethodName.DoPasteNodes)
		{
			return true;
		}
		if (method == MethodName.UndoPasteNodes)
		{
			return true;
		}
		if (method == MethodName.DuplicateSelectedNodes)
		{
			return true;
		}
		if (method == MethodName.DeselectAllGraphNodes)
		{
			return true;
		}
		if (method == MethodName.CloneNodeData)
		{
			return true;
		}
		if (method == MethodName.AddNodeFromData)
		{
			return true;
		}
		if (method == MethodName.FocusNode)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.Editor)
		{
			Editor = VariantUtils.ConvertTo<XWBPEditor>(in value);
			return true;
		}
		if (name == PropertyName.GraphData)
		{
			GraphData = VariantUtils.ConvertTo<XWBPGraphData>(in value);
			return true;
		}
		if (name == PropertyName._registry)
		{
			_registry = VariantUtils.ConvertTo<XWBPNodeRegistry>(in value);
			return true;
		}
		if (name == PropertyName._extendsClassButton)
		{
			_extendsClassButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._generateScriptButton)
		{
			_generateScriptButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._runPreviewButton)
		{
			_runPreviewButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._navigationTools)
		{
			_navigationTools = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._previewStatusPanel)
		{
			_previewStatusPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._previewStatusLabel)
		{
			_previewStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._minimapToggleButton)
		{
			_minimapToggleButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._resetZoomButton)
		{
			_resetZoomButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._focusSelectionButton)
		{
			_focusSelectionButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._nodeCountLabel)
		{
			_nodeCountLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._connectionCountLabel)
		{
			_connectionCountLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._nodeCountIcon)
		{
			_nodeCountIcon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._connectionCountIcon)
		{
			_connectionCountIcon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._variablePopupMenu)
		{
			_variablePopupMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._signalPopupMenu)
		{
			_signalPopupMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._canvasContextMenu)
		{
			_canvasContextMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._pendingVariableData)
		{
			_pendingVariableData = VariantUtils.ConvertTo<XWBPVariableData>(in value);
			return true;
		}
		if (name == PropertyName._pendingSignalData)
		{
			_pendingSignalData = VariantUtils.ConvertTo<XWBPSignalData>(in value);
			return true;
		}
		if (name == PropertyName._pendingDropPosition)
		{
			_pendingDropPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._lastPointerGraphPosition)
		{
			_lastPointerGraphPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._isDraggingConnection)
		{
			_isDraggingConnection = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragFromNode)
		{
			_dragFromNode = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._dragFromPort)
		{
			_dragFromPort = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._dragIsOutput)
		{
			_dragIsOutput = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._connectionMadeDuringDrag)
		{
			_connectionMadeDuringDrag = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._disconnectionDuringDrag)
		{
			_disconnectionDuringDrag = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._recentlyDisconnected)
		{
			_recentlyDisconnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._runtimeHighlightedNode)
		{
			_runtimeHighlightedNode = VariantUtils.ConvertTo<XWBPGraphNode>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Editor)
		{
			value = VariantUtils.CreateFrom<XWBPEditor>(Editor);
			return true;
		}
		if (name == PropertyName.GraphData)
		{
			value = VariantUtils.CreateFrom<XWBPGraphData>(GraphData);
			return true;
		}
		int from;
		if (name == PropertyName.ClipboardNodeCount)
		{
			from = ClipboardNodeCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ClipboardConnectionCount)
		{
			from = ClipboardConnectionCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.LastPointerGraphPosition)
		{
			value = VariantUtils.CreateFrom<Vector2>(LastPointerGraphPosition);
			return true;
		}
		if (name == PropertyName._registry)
		{
			value = VariantUtils.CreateFrom(in _registry);
			return true;
		}
		if (name == PropertyName._extendsClassButton)
		{
			value = VariantUtils.CreateFrom(in _extendsClassButton);
			return true;
		}
		if (name == PropertyName._generateScriptButton)
		{
			value = VariantUtils.CreateFrom(in _generateScriptButton);
			return true;
		}
		if (name == PropertyName._runPreviewButton)
		{
			value = VariantUtils.CreateFrom(in _runPreviewButton);
			return true;
		}
		if (name == PropertyName._navigationTools)
		{
			value = VariantUtils.CreateFrom(in _navigationTools);
			return true;
		}
		if (name == PropertyName._previewStatusPanel)
		{
			value = VariantUtils.CreateFrom(in _previewStatusPanel);
			return true;
		}
		if (name == PropertyName._previewStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _previewStatusLabel);
			return true;
		}
		if (name == PropertyName._minimapToggleButton)
		{
			value = VariantUtils.CreateFrom(in _minimapToggleButton);
			return true;
		}
		if (name == PropertyName._resetZoomButton)
		{
			value = VariantUtils.CreateFrom(in _resetZoomButton);
			return true;
		}
		if (name == PropertyName._focusSelectionButton)
		{
			value = VariantUtils.CreateFrom(in _focusSelectionButton);
			return true;
		}
		if (name == PropertyName._nodeCountLabel)
		{
			value = VariantUtils.CreateFrom(in _nodeCountLabel);
			return true;
		}
		if (name == PropertyName._connectionCountLabel)
		{
			value = VariantUtils.CreateFrom(in _connectionCountLabel);
			return true;
		}
		if (name == PropertyName._nodeCountIcon)
		{
			value = VariantUtils.CreateFrom(in _nodeCountIcon);
			return true;
		}
		if (name == PropertyName._connectionCountIcon)
		{
			value = VariantUtils.CreateFrom(in _connectionCountIcon);
			return true;
		}
		if (name == PropertyName._variablePopupMenu)
		{
			value = VariantUtils.CreateFrom(in _variablePopupMenu);
			return true;
		}
		if (name == PropertyName._signalPopupMenu)
		{
			value = VariantUtils.CreateFrom(in _signalPopupMenu);
			return true;
		}
		if (name == PropertyName._canvasContextMenu)
		{
			value = VariantUtils.CreateFrom(in _canvasContextMenu);
			return true;
		}
		if (name == PropertyName._pendingVariableData)
		{
			value = VariantUtils.CreateFrom(in _pendingVariableData);
			return true;
		}
		if (name == PropertyName._pendingSignalData)
		{
			value = VariantUtils.CreateFrom(in _pendingSignalData);
			return true;
		}
		if (name == PropertyName._pendingDropPosition)
		{
			value = VariantUtils.CreateFrom(in _pendingDropPosition);
			return true;
		}
		if (name == PropertyName._lastPointerGraphPosition)
		{
			value = VariantUtils.CreateFrom(in _lastPointerGraphPosition);
			return true;
		}
		if (name == PropertyName._isDraggingConnection)
		{
			value = VariantUtils.CreateFrom(in _isDraggingConnection);
			return true;
		}
		if (name == PropertyName._dragFromNode)
		{
			value = VariantUtils.CreateFrom(in _dragFromNode);
			return true;
		}
		if (name == PropertyName._dragFromPort)
		{
			value = VariantUtils.CreateFrom(in _dragFromPort);
			return true;
		}
		if (name == PropertyName._dragIsOutput)
		{
			value = VariantUtils.CreateFrom(in _dragIsOutput);
			return true;
		}
		if (name == PropertyName._connectionMadeDuringDrag)
		{
			value = VariantUtils.CreateFrom(in _connectionMadeDuringDrag);
			return true;
		}
		if (name == PropertyName._disconnectionDuringDrag)
		{
			value = VariantUtils.CreateFrom(in _disconnectionDuringDrag);
			return true;
		}
		if (name == PropertyName._recentlyDisconnected)
		{
			value = VariantUtils.CreateFrom(in _recentlyDisconnected);
			return true;
		}
		if (name == PropertyName._runtimeHighlightedNode)
		{
			value = VariantUtils.CreateFrom(in _runtimeHighlightedNode);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.Editor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.GraphData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ClipboardNodeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ClipboardConnectionCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.LastPointerGraphPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._registry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._extendsClassButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._generateScriptButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runPreviewButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._navigationTools, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewStatusPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._minimapToggleButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resetZoomButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._focusSelectionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nodeCountLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._connectionCountLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nodeCountIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._connectionCountIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._variablePopupMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._signalPopupMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._canvasContextMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pendingVariableData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pendingSignalData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._pendingDropPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._lastPointerGraphPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isDraggingConnection, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName._dragFromNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._dragFromPort, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._dragIsOutput, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._connectionMadeDuringDrag, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._disconnectionDuringDrag, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._recentlyDisconnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeHighlightedNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.Editor, Variant.From<XWBPEditor>(Editor));
		info.AddProperty(PropertyName.GraphData, Variant.From<XWBPGraphData>(GraphData));
		info.AddProperty(PropertyName._registry, Variant.From(in _registry));
		info.AddProperty(PropertyName._extendsClassButton, Variant.From(in _extendsClassButton));
		info.AddProperty(PropertyName._generateScriptButton, Variant.From(in _generateScriptButton));
		info.AddProperty(PropertyName._runPreviewButton, Variant.From(in _runPreviewButton));
		info.AddProperty(PropertyName._navigationTools, Variant.From(in _navigationTools));
		info.AddProperty(PropertyName._previewStatusPanel, Variant.From(in _previewStatusPanel));
		info.AddProperty(PropertyName._previewStatusLabel, Variant.From(in _previewStatusLabel));
		info.AddProperty(PropertyName._minimapToggleButton, Variant.From(in _minimapToggleButton));
		info.AddProperty(PropertyName._resetZoomButton, Variant.From(in _resetZoomButton));
		info.AddProperty(PropertyName._focusSelectionButton, Variant.From(in _focusSelectionButton));
		info.AddProperty(PropertyName._nodeCountLabel, Variant.From(in _nodeCountLabel));
		info.AddProperty(PropertyName._connectionCountLabel, Variant.From(in _connectionCountLabel));
		info.AddProperty(PropertyName._nodeCountIcon, Variant.From(in _nodeCountIcon));
		info.AddProperty(PropertyName._connectionCountIcon, Variant.From(in _connectionCountIcon));
		info.AddProperty(PropertyName._variablePopupMenu, Variant.From(in _variablePopupMenu));
		info.AddProperty(PropertyName._signalPopupMenu, Variant.From(in _signalPopupMenu));
		info.AddProperty(PropertyName._canvasContextMenu, Variant.From(in _canvasContextMenu));
		info.AddProperty(PropertyName._pendingVariableData, Variant.From(in _pendingVariableData));
		info.AddProperty(PropertyName._pendingSignalData, Variant.From(in _pendingSignalData));
		info.AddProperty(PropertyName._pendingDropPosition, Variant.From(in _pendingDropPosition));
		info.AddProperty(PropertyName._lastPointerGraphPosition, Variant.From(in _lastPointerGraphPosition));
		info.AddProperty(PropertyName._isDraggingConnection, Variant.From(in _isDraggingConnection));
		info.AddProperty(PropertyName._dragFromNode, Variant.From(in _dragFromNode));
		info.AddProperty(PropertyName._dragFromPort, Variant.From(in _dragFromPort));
		info.AddProperty(PropertyName._dragIsOutput, Variant.From(in _dragIsOutput));
		info.AddProperty(PropertyName._connectionMadeDuringDrag, Variant.From(in _connectionMadeDuringDrag));
		info.AddProperty(PropertyName._disconnectionDuringDrag, Variant.From(in _disconnectionDuringDrag));
		info.AddProperty(PropertyName._recentlyDisconnected, Variant.From(in _recentlyDisconnected));
		info.AddProperty(PropertyName._runtimeHighlightedNode, Variant.From(in _runtimeHighlightedNode));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.Editor, out var value))
		{
			Editor = value.As<XWBPEditor>();
		}
		if (info.TryGetProperty(PropertyName.GraphData, out var value2))
		{
			GraphData = value2.As<XWBPGraphData>();
		}
		if (info.TryGetProperty(PropertyName._registry, out var value3))
		{
			_registry = value3.As<XWBPNodeRegistry>();
		}
		if (info.TryGetProperty(PropertyName._extendsClassButton, out var value4))
		{
			_extendsClassButton = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._generateScriptButton, out var value5))
		{
			_generateScriptButton = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._runPreviewButton, out var value6))
		{
			_runPreviewButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._navigationTools, out var value7))
		{
			_navigationTools = value7.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._previewStatusPanel, out var value8))
		{
			_previewStatusPanel = value8.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._previewStatusLabel, out var value9))
		{
			_previewStatusLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._minimapToggleButton, out var value10))
		{
			_minimapToggleButton = value10.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._resetZoomButton, out var value11))
		{
			_resetZoomButton = value11.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._focusSelectionButton, out var value12))
		{
			_focusSelectionButton = value12.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nodeCountLabel, out var value13))
		{
			_nodeCountLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._connectionCountLabel, out var value14))
		{
			_connectionCountLabel = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._nodeCountIcon, out var value15))
		{
			_nodeCountIcon = value15.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._connectionCountIcon, out var value16))
		{
			_connectionCountIcon = value16.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._variablePopupMenu, out var value17))
		{
			_variablePopupMenu = value17.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._signalPopupMenu, out var value18))
		{
			_signalPopupMenu = value18.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._canvasContextMenu, out var value19))
		{
			_canvasContextMenu = value19.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._pendingVariableData, out var value20))
		{
			_pendingVariableData = value20.As<XWBPVariableData>();
		}
		if (info.TryGetProperty(PropertyName._pendingSignalData, out var value21))
		{
			_pendingSignalData = value21.As<XWBPSignalData>();
		}
		if (info.TryGetProperty(PropertyName._pendingDropPosition, out var value22))
		{
			_pendingDropPosition = value22.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._lastPointerGraphPosition, out var value23))
		{
			_lastPointerGraphPosition = value23.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._isDraggingConnection, out var value24))
		{
			_isDraggingConnection = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragFromNode, out var value25))
		{
			_dragFromNode = value25.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._dragFromPort, out var value26))
		{
			_dragFromPort = value26.As<int>();
		}
		if (info.TryGetProperty(PropertyName._dragIsOutput, out var value27))
		{
			_dragIsOutput = value27.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._connectionMadeDuringDrag, out var value28))
		{
			_connectionMadeDuringDrag = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._disconnectionDuringDrag, out var value29))
		{
			_disconnectionDuringDrag = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._recentlyDisconnected, out var value30))
		{
			_recentlyDisconnected = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._runtimeHighlightedNode, out var value31))
		{
			_runtimeHighlightedNode = value31.As<XWBPGraphNode>();
		}
	}
}
