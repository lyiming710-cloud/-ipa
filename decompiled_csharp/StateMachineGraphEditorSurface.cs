using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;

[Tool]
[ScriptPath("res://addons/godot_state_charts/VisualEditor/StateMachineGraphEditorSurface.cs")]
public class StateMachineGraphEditorSurface : GraphEdit
{
	private enum DetailsContextKind
	{
		Definition,
		State,
		Transition
	}

	private enum ToolbarAction
	{
		Validate = 1,
		Compile,
		ToggleDetails,
		ToggleDiagnostics
	}

	public new class MethodName : GraphEdit.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName ConnectSurfaceSignals = "ConnectSurfaceSignals";

		public static readonly StringName DisconnectSurfaceSignals = "DisconnectSurfaceSignals";

		public static readonly StringName SetWorkbenchActive = "SetWorkbenchActive";

		public static readonly StringName FindStateNode = "FindStateNode";

		public static readonly StringName FindTransitionChip = "FindTransitionChip";

		public static readonly StringName HasVisualConnection = "HasVisualConnection";

		public static readonly StringName IsStateVisible = "IsStateVisible";

		public static readonly StringName IsStateCollapsed = "IsStateCollapsed";

		public static readonly StringName RefreshLayoutOnly = "RefreshLayoutOnly";

		public static readonly StringName LoadDefinition = "LoadDefinition";

		public static readonly StringName RefreshGraph = "RefreshGraph";

		public static readonly StringName EnsureDetailsContextVisible = "EnsureDetailsContextVisible";

		public static readonly StringName AddTransitionChipStateIndex = "AddTransitionChipStateIndex";

		public static readonly StringName SynchronizeTransitionChipVisibility = "SynchronizeTransitionChipVisibility";

		public static readonly StringName UpdateAllTransitionChipPositions = "UpdateAllTransitionChipPositions";

		public static readonly StringName QueueTransitionChipLayoutAfterGraphSettles = "QueueTransitionChipLayoutAfterGraphSettles";

		public static readonly StringName UpdateTransitionChipLayoutAfterGraphSettles = "UpdateTransitionChipLayoutAfterGraphSettles";

		public static readonly StringName UpdateMovingTransitionChipPositions = "UpdateMovingTransitionChipPositions";

		public static readonly StringName OnTransitionChipInspectRequested = "OnTransitionChipInspectRequested";

		public static readonly StringName ResolveStateDisplayName = "ResolveStateDisplayName";

		public static readonly StringName CompareTransitionChips = "CompareTransitionChips";

		public static readonly StringName ToLocalPosition = "ToLocalPosition";

		public static readonly StringName GetSlotEdgePosition = "GetSlotEdgePosition";

		public static readonly StringName CubicBezierTangent = "CubicBezierTangent";

		public static readonly StringName RemoveVisualConnectionsForNode = "RemoveVisualConnectionsForNode";

		public static readonly StringName NavigateToStableId = "NavigateToStableId";

		public static readonly StringName ApplyPickedResource = "ApplyPickedResource";

		public static readonly StringName GetSelectedStateNodes = "GetSelectedStateNodes";

		public static readonly StringName GetSelectedNodes = "GetSelectedNodes";

		public static readonly StringName BuildGameToolbar = "BuildGameToolbar";

		public static readonly StringName RefreshStateAuthorTypes = "RefreshStateAuthorTypes";

		public static readonly StringName OnAuthorTypesChanged = "OnAuthorTypesChanged";

		public static readonly StringName GetSelectedStateAuthorTypeId = "GetSelectedStateAuthorTypeId";

		public static readonly StringName RefreshCompactStateAuthorTypes = "RefreshCompactStateAuthorTypes";

		public static readonly StringName SelectStateAuthorTypeIndex = "SelectStateAuthorTypeIndex";

		public static readonly StringName ShowCompositionFailureDiagnostics = "ShowCompositionFailureDiagnostics";

		public static readonly StringName AddStateKindMenuItem = "AddStateKindMenuItem";

		public static readonly StringName BindZoomButtons = "BindZoomButtons";

		public static readonly StringName ApplyResponsiveLayout = "ApplyResponsiveLayout";

		public static readonly StringName ToggleDetailsPanel = "ToggleDetailsPanel";

		public static readonly StringName OpenDetailsForSelection = "OpenDetailsForSelection";

		public static readonly StringName BuildLegacyToolbar = "BuildLegacyToolbar";

		public static readonly StringName BuildOverlayPanels = "BuildOverlayPanels";

		public static readonly StringName ConfigureEmptyState = "ConfigureEmptyState";

		public static readonly StringName InitializeEmptyDefinition = "InitializeEmptyDefinition";

		public static readonly StringName DisconnectOverlaySignals = "DisconnectOverlaySignals";

		public static readonly StringName OnDetailsResourcePickerRequested = "OnDetailsResourcePickerRequested";

		public static readonly StringName OnDetailsGuardEditRequested = "OnDetailsGuardEditRequested";

		public static readonly StringName OnAliasAddRequested = "OnAliasAddRequested";

		public static readonly StringName OnAliasRemoveRequested = "OnAliasRemoveRequested";

		public static readonly StringName OnTransitionCreateRequested = "OnTransitionCreateRequested";

		public static readonly StringName OnTransitionRemoveRequested = "OnTransitionRemoveRequested";

		public static readonly StringName OnStateOverrideCreateRequested = "OnStateOverrideCreateRequested";

		public static readonly StringName OnStateOverrideRestoreRequested = "OnStateOverrideRestoreRequested";

		public static readonly StringName OnTransitionOverrideCreateRequested = "OnTransitionOverrideCreateRequested";

		public static readonly StringName OnTransitionOverrideRestoreRequested = "OnTransitionOverrideRestoreRequested";

		public static readonly StringName BuildCanvasContextMenu = "BuildCanvasContextMenu";

		public static readonly StringName OnSurfaceVisibilityChanged = "OnSurfaceVisibilityChanged";

		public static readonly StringName OnToolbarActionPressed = "OnToolbarActionPressed";

		public static readonly StringName ApplySimulationSnapshot = "ApplySimulationSnapshot";

		public static readonly StringName ApplyTransitionChipRuntimeState = "ApplyTransitionChipRuntimeState";

		public static readonly StringName ApplyTransitionChipSelection = "ApplyTransitionChipSelection";

		public static readonly StringName ResolveVisibleHierarchyProjection = "ResolveVisibleHierarchyProjection";

		public static readonly StringName ResolvePosition = "ResolvePosition";

		public static readonly StringName AddStateAtViewportCenter = "AddStateAtViewportCenter";

		public static readonly StringName AddStateAtGraphPosition = "AddStateAtGraphPosition";

		public static readonly StringName ResolveParentForNewState = "ResolveParentForNewState";

		public static readonly StringName FindVisibleState = "FindVisibleState";

		public static readonly StringName CanContainStateKind = "CanContainStateKind";

		public static readonly StringName OnConnectionRequest = "OnConnectionRequest";

		public static readonly StringName OnDisconnectionRequest = "OnDisconnectionRequest";

		public static readonly StringName OnDeleteNodesRequest = "OnDeleteNodesRequest";

		public static readonly StringName OnCopyNodesRequest = "OnCopyNodesRequest";

		public static readonly StringName OnCutNodesRequest = "OnCutNodesRequest";

		public static readonly StringName OnPasteNodesRequest = "OnPasteNodesRequest";

		public static readonly StringName OnDuplicateNodesRequest = "OnDuplicateNodesRequest";

		public static readonly StringName OnPopupRequest = "OnPopupRequest";

		public static readonly StringName ConfigureCanvasContextMenu = "ConfigureCanvasContextMenu";

		public static readonly StringName OnCanvasContextAction = "OnCanvasContextAction";

		public static readonly StringName ToGraphPosition = "ToGraphPosition";

		public static readonly StringName OnBeginNodeMove = "OnBeginNodeMove";

		public static readonly StringName OnEndNodeMove = "OnEndNodeMove";

		public static readonly StringName OnScrollOffsetChanged = "OnScrollOffsetChanged";

		public static readonly StringName OnGraphGuiInput = "OnGraphGuiInput";

		public static readonly StringName TrySelectConnectionAt = "TrySelectConnectionAt";

		public static readonly StringName DistanceToSegment = "DistanceToSegment";

		public static readonly StringName DistanceToConnectionCurve = "DistanceToConnectionCurve";

		public static readonly StringName QueueViewportCapture = "QueueViewportCapture";

		public static readonly StringName CaptureViewportAfterInput = "CaptureViewportAfterInput";

		public static readonly StringName QueueViewportPersistence = "QueueViewportPersistence";

		public static readonly StringName FlushViewportState = "FlushViewportState";

		public static readonly StringName OnNodeSelected = "OnNodeSelected";

		public static readonly StringName OnNodeCollapseRequested = "OnNodeCollapseRequested";

		public static readonly StringName ShowStateDetails = "ShowStateDetails";

		public static readonly StringName ShowTransitionDetails = "ShowTransitionDetails";

		public static readonly StringName ShowDefinitionOverview = "ShowDefinitionOverview";

		public static readonly StringName SetAsInitialState = "SetAsInitialState";

		public static readonly StringName ApplyFieldChange = "ApplyFieldChange";

		public static readonly StringName RunDiagnostics = "RunDiagnostics";

		public static readonly StringName CompilePreview = "CompilePreview";

		public static readonly StringName FindNodeByGraphName = "FindNodeByGraphName";
	}

	public new class PropertyName : GraphEdit.PropertyName
	{
		public static readonly StringName Definition = "Definition";

		public static readonly StringName Layout = "Layout";

		public static readonly StringName SimulationPanel = "SimulationPanel";

		public static readonly StringName DetailsPanel = "DetailsPanel";

		public static readonly StringName IsWorkbenchActive = "IsWorkbenchActive";

		public static readonly StringName IsHiddenWorkQuiescent = "IsHiddenWorkQuiescent";

		public static readonly StringName ActiveDetailsKind = "ActiveDetailsKind";

		public static readonly StringName ActiveDetailsStableId = "ActiveDetailsStableId";

		public static readonly StringName LastPointerGraphPosition = "LastPointerGraphPosition";

		public static readonly StringName FullGraphRebuildCount = "FullGraphRebuildCount";

		public static readonly StringName IncrementalGraphRefreshCount = "IncrementalGraphRefreshCount";

		public static readonly StringName GraphNodeCreateCount = "GraphNodeCreateCount";

		public static readonly StringName LastConnectionSelectionGroupSize = "LastConnectionSelectionGroupSize";

		public static readonly StringName VisibleStateNodeCount = "VisibleStateNodeCount";

		public static readonly StringName VisibleConnectionCount = "VisibleConnectionCount";

		public static readonly StringName TransitionChipCount = "TransitionChipCount";

		public static readonly StringName VisibleTransitionChipCount = "VisibleTransitionChipCount";

		public static readonly StringName TransitionChipCreateCount = "TransitionChipCreateCount";

		public static readonly StringName TransitionChipSynchronizeCount = "TransitionChipSynchronizeCount";

		public static readonly StringName TransitionChipPositionUpdateCount = "TransitionChipPositionUpdateCount";

		public static readonly StringName LastTransitionChipRelayoutCount = "LastTransitionChipRelayoutCount";

		public static readonly StringName LastTransitionChipRelayoutReason = "LastTransitionChipRelayoutReason";

		public static readonly StringName ActiveViewportDebounceTimerCount = "ActiveViewportDebounceTimerCount";

		public static readonly StringName _details = "_details";

		public static readonly StringName _diagnostics = "_diagnostics";

		public static readonly StringName _simulation = "_simulation";

		public static readonly StringName _newStateKind = "_newStateKind";

		public static readonly StringName _newStateType = "_newStateType";

		public static readonly StringName _compactStateTypeMenu = "_compactStateTypeMenu";

		public static readonly StringName _addStateButton = "_addStateButton";

		public static readonly StringName _narrowAddStateMenu = "_narrowAddStateMenu";

		public static readonly StringName _detailsToggleButton = "_detailsToggleButton";

		public static readonly StringName _simulationToggle = "_simulationToggle";

		public static readonly StringName _emptyStatePanel = "_emptyStatePanel";

		public static readonly StringName _emptyStateHint = "_emptyStateHint";

		public static readonly StringName _createDefinitionButton = "_createDefinitionButton";

		public static readonly StringName _openDefinitionButton = "_openDefinitionButton";

		public static readonly StringName _initializeDefinitionButton = "_initializeDefinitionButton";

		public static readonly StringName _compositionFailureBanner = "_compositionFailureBanner";

		public static readonly StringName _compactToolbar = "_compactToolbar";

		public static readonly StringName _detailsRequestedVisible = "_detailsRequestedVisible";

		public static readonly StringName _narrowDetailsVisible = "_narrowDetailsVisible";

		public static readonly StringName _refreshing = "_refreshing";

		public static readonly StringName _renderedDefinition = "_renderedDefinition";

		public static readonly StringName _detailsContext = "_detailsContext";

		public static readonly StringName _detailsStableId = "_detailsStableId";

		public static readonly StringName _viewportDirty = "_viewportDirty";

		public static readonly StringName _viewportCaptureQueued = "_viewportCaptureQueued";

		public static readonly StringName _pendingScrollOffset = "_pendingScrollOffset";

		public static readonly StringName _pendingZoom = "_pendingZoom";

		public static readonly StringName _viewportDebounceTimer = "_viewportDebounceTimer";

		public static readonly StringName _canvasContextMenu = "_canvasContextMenu";

		public static readonly StringName _lastPointerGraphPosition = "_lastPointerGraphPosition";

		public static readonly StringName _surfaceSignalsConnected = "_surfaceSignalsConnected";

		public static readonly StringName _trackingNodeMove = "_trackingNodeMove";

		public static readonly StringName _transitionChipLayoutQueued = "_transitionChipLayoutQueued";

		public static readonly StringName _queuedTransitionChipLayoutReason = "_queuedTransitionChipLayoutReason";

		public static readonly StringName _lastCompletedTransitionChipId = "_lastCompletedTransitionChipId";

		public static readonly StringName _selectedTransitionChipId = "_selectedTransitionChipId";
	}

	public new class SignalName : GraphEdit.SignalName
	{
	}

	private const float NarrowLayoutWidth = 620f;

	public const long CanvasMenuAddAtomicId = 100L;

	public const long CanvasMenuAddCompoundId = 101L;

	public const long CanvasMenuAddParallelId = 102L;

	public const long CanvasMenuAddHistoryId = 103L;

	public const long CanvasMenuCopyId = 200L;

	public const long CanvasMenuCutId = 201L;

	public const long CanvasMenuDuplicateId = 202L;

	public const long CanvasMenuPasteId = 203L;

	public const long CanvasMenuDeleteId = 204L;

	private readonly System.Collections.Generic.Dictionary<string, StateMachineGraphNode> _nodes = new System.Collections.Generic.Dictionary<string, StateMachineGraphNode>(StringComparer.Ordinal);

	private readonly System.Collections.Generic.Dictionary<string, Vector2> _moveOrigins = new System.Collections.Generic.Dictionary<string, Vector2>(StringComparer.Ordinal);

	private readonly System.Collections.Generic.Dictionary<string, string> _parentStateIds = new System.Collections.Generic.Dictionary<string, string>(StringComparer.Ordinal);

	private readonly HashSet<(string Source, string Target)> _visualConnections = new HashSet<(string, string)>();

	private readonly HashSet<string> _visibleStateIds = new HashSet<string>(StringComparer.Ordinal);

	private readonly System.Collections.Generic.Dictionary<string, StateMachineTransitionChip> _transitionChips = new System.Collections.Generic.Dictionary<string, StateMachineTransitionChip>(StringComparer.Ordinal);

	private readonly System.Collections.Generic.Dictionary<(string Source, string Target), List<StateMachineTransitionChip>> _transitionChipGroups = new System.Collections.Generic.Dictionary<(string, string), List<StateMachineTransitionChip>>();

	private readonly System.Collections.Generic.Dictionary<string, List<StateMachineTransitionChip>> _transitionChipsByState = new System.Collections.Generic.Dictionary<string, List<StateMachineTransitionChip>>(StringComparer.Ordinal);

	private readonly HashSet<(string Source, string Target)> _transitionGroupsNeedingLayout = new HashSet<(string, string)>();

	private readonly HashSet<string> _activeStateProjectionIds = new HashSet<string>(StringComparer.Ordinal);

	private readonly HashSet<string> _pendingStateProjectionIds = new HashSet<string>(StringComparer.Ordinal);

	private readonly HashSet<string> _activePendingTransitionChipIds = new HashSet<string>(StringComparer.Ordinal);

	private readonly HashSet<string> _nextPendingTransitionChipIds = new HashSet<string>(StringComparer.Ordinal);

	private readonly HashSet<string> _layoutMutationMovedStateIds = new HashSet<string>(StringComparer.Ordinal);

	private readonly List<string> _movingStateIds = new List<string>();

	private readonly System.Collections.Generic.Dictionary<string, Vector2> _movingStatePositions = new System.Collections.Generic.Dictionary<string, Vector2>(StringComparer.Ordinal);

	private StateMachineDetailsPanel _details;

	private StateMachineDiagnosticsPanel _diagnostics;

	private StateMachineSimulationPanel _simulation;

	private OptionButton _newStateKind;

	private OptionButton _newStateType;

	private MenuButton _compactStateTypeMenu;

	private Button _addStateButton;

	private MenuButton _narrowAddStateMenu;

	private Button _detailsToggleButton;

	private Button _simulationToggle;

	private PanelContainer _emptyStatePanel;

	private Label _emptyStateHint;

	private Button _createDefinitionButton;

	private Button _openDefinitionButton;

	private Button _initializeDefinitionButton;

	private PanelContainer _compositionFailureBanner;

	private HBoxContainer _compactToolbar;

	private bool _detailsRequestedVisible = true;

	private bool _narrowDetailsVisible;

	private bool _refreshing;

	private StateMachineDefinition _renderedDefinition;

	private DetailsContextKind _detailsContext;

	private string _detailsStableId = string.Empty;

	private bool _viewportDirty;

	private bool _viewportCaptureQueued;

	private Vector2 _pendingScrollOffset;

	private float _pendingZoom = 1f;

	private Timer _viewportDebounceTimer;

	private PopupMenu _canvasContextMenu;

	private Vector2 _lastPointerGraphPosition;

	private bool _surfaceSignalsConnected;

	private bool _trackingNodeMove;

	private bool _transitionChipLayoutQueued;

	private string _queuedTransitionChipLayoutReason = string.Empty;

	private string _lastCompletedTransitionChipId = string.Empty;

	private string _selectedTransitionChipId = string.Empty;

	public StateMachineGraphController GraphController { get; private set; }

	public IStateMachineUndoAdapter UndoAdapter { get; private set; }

	public StateMachineDefinition Definition => GraphController?.Definition;

	public StateMachineLayout Layout => GraphController?.Layout;

	public StateMachineSimulationPanel SimulationPanel => _simulation;

	public StateMachineDetailsPanel DetailsPanel => _details;

	public bool IsWorkbenchActive { get; private set; }

	public bool IsHiddenWorkQuiescent
	{
		get
		{
			if (!IsWorkbenchActive && !IsProcessing() && !_transitionChipLayoutQueued && (_simulation == null || _simulation.IsQuiescent))
			{
				if (_details != null)
				{
					return _details.ActiveNumericDebounceTimerCount == 0;
				}
				return true;
			}
			return false;
		}
	}

	public string ActiveDetailsKind => _detailsContext.ToString();

	public string ActiveDetailsStableId => _detailsStableId;

	public Vector2 LastPointerGraphPosition => _lastPointerGraphPosition;

	public int FullGraphRebuildCount { get; private set; }

	public int IncrementalGraphRefreshCount { get; private set; }

	public int GraphNodeCreateCount { get; private set; }

	public int LastConnectionSelectionGroupSize { get; private set; }

	public int VisibleStateNodeCount => _visibleStateIds.Count;

	public int VisibleConnectionCount => _visualConnections.Count;

	public int TransitionChipCount => _transitionChips.Count;

	public int VisibleTransitionChipCount
	{
		get
		{
			int num = 0;
			foreach (StateMachineTransitionChip value in _transitionChips.Values)
			{
				if (GodotObject.IsInstanceValid(value) && value.Visible)
				{
					num++;
				}
			}
			return num;
		}
	}

	public int TransitionChipCreateCount { get; private set; }

	public int TransitionChipSynchronizeCount { get; private set; }

	public int TransitionChipPositionUpdateCount { get; private set; }

	public int LastTransitionChipRelayoutCount { get; private set; }

	public string LastTransitionChipRelayoutReason { get; private set; } = string.Empty;

	public int ActiveViewportDebounceTimerCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_viewportDebounceTimer) || _viewportDebounceTimer.IsStopped())
			{
				return 0;
			}
			return 1;
		}
	}

	public event Action<Resource, string> ResourcePickerRequested;

	public event Action<Resource, Resource, string, int> GuardEditRequested;

	public event Action<StateMachineCallbackCatalogEntry, StateMachineCallbackPhase> CallbackSourceOpenRequested;

	public event Action DefinitionChanged;

	public event Action LayoutChanged;

	public event Action CreateDefinitionRequested;

	public event Action OpenDefinitionRequested;

	public override void _Ready()
	{
		StateMachineAuthorTypeRegistry.Changed += OnAuthorTypesChanged;
		ConnectSurfaceSignals();
		SetProcess(enable: false);
		_viewportDebounceTimer = new Timer
		{
			Name = "ViewportDebounceTimer",
			OneShot = true,
			WaitTime = 0.18,
			ProcessMode = ProcessModeEnum.Always
		};
		_viewportDebounceTimer.Timeout += FlushViewportState;
		AddChild(_viewportDebounceTimer, forceReadableName: false, InternalMode.Disabled);
		BuildGameToolbar();
		BindZoomButtons();
		BuildOverlayPanels();
		BuildCanvasContextMenu();
		CallDeferred("ApplyResponsiveLayout");
		SetWorkbenchActive(IsVisibleInTree());
	}

	public override void _ExitTree()
	{
		StateMachineAuthorTypeRegistry.Changed -= OnAuthorTypesChanged;
		FlushViewportState();
		DisconnectSurfaceSignals();
		if (GodotObject.IsInstanceValid(_viewportDebounceTimer))
		{
			_viewportDebounceTimer.Stop();
			_viewportDebounceTimer.Timeout -= FlushViewportState;
		}
		DisconnectOverlaySignals();
		if (GraphController != null)
		{
			GraphController.MutationApplied -= OnControllerMutationApplied;
		}
		SetWorkbenchActive(active: false);
	}

	public override void _Process(double delta)
	{
		if (!_trackingNodeMove || !IsWorkbenchActive || !IsVisibleInTree())
		{
			SetProcess(enable: false);
		}
		else
		{
			UpdateMovingTransitionChipPositions();
		}
	}

	private void ConnectSurfaceSignals()
	{
		if (!_surfaceSignalsConnected)
		{
			ConnectionRequest += OnConnectionRequest;
			DisconnectionRequest += OnDisconnectionRequest;
			DeleteNodesRequest += OnDeleteNodesRequest;
			CopyNodesRequest += OnCopyNodesRequest;
			CutNodesRequest += OnCutNodesRequest;
			PasteNodesRequest += OnPasteNodesRequest;
			DuplicateNodesRequest += OnDuplicateNodesRequest;
			PopupRequest += OnPopupRequest;
			BeginNodeMove += OnBeginNodeMove;
			EndNodeMove += OnEndNodeMove;
			NodeSelected += OnNodeSelected;
			ScrollOffsetChanged += OnScrollOffsetChanged;
			GuiInput += OnGraphGuiInput;
			VisibilityChanged += OnSurfaceVisibilityChanged;
			Resized += ApplyResponsiveLayout;
			_surfaceSignalsConnected = true;
		}
	}

	private void DisconnectSurfaceSignals()
	{
		if (_surfaceSignalsConnected)
		{
			ConnectionRequest -= OnConnectionRequest;
			DisconnectionRequest -= OnDisconnectionRequest;
			DeleteNodesRequest -= OnDeleteNodesRequest;
			CopyNodesRequest -= OnCopyNodesRequest;
			CutNodesRequest -= OnCutNodesRequest;
			PasteNodesRequest -= OnPasteNodesRequest;
			DuplicateNodesRequest -= OnDuplicateNodesRequest;
			PopupRequest -= OnPopupRequest;
			BeginNodeMove -= OnBeginNodeMove;
			EndNodeMove -= OnEndNodeMove;
			NodeSelected -= OnNodeSelected;
			ScrollOffsetChanged -= OnScrollOffsetChanged;
			GuiInput -= OnGraphGuiInput;
			VisibilityChanged -= OnSurfaceVisibilityChanged;
			Resized -= ApplyResponsiveLayout;
			_surfaceSignalsConnected = false;
		}
	}

	public void SetWorkbenchActive(bool active)
	{
		if (!active)
		{
			_details?.FlushPendingNumericCommit();
			FlushViewportState();
		}
		IsWorkbenchActive = active;
		_simulation?.SetWorkbenchActive(IsWorkbenchActive);
		_details?.SetEditingActive(IsWorkbenchActive);
		if (IsWorkbenchActive)
		{
			QueueTransitionChipLayoutAfterGraphSettles("SetWorkbenchActive");
			SetProcess(_trackingNodeMove);
			return;
		}
		_trackingNodeMove = false;
		_movingStateIds.Clear();
		_movingStatePositions.Clear();
		SetProcess(enable: false);
		if (_simulationToggle != null)
		{
			_simulationToggle.ButtonPressed = false;
		}
		if (_simulation != null)
		{
			_simulation.Visible = false;
		}
	}

	public StateMachineGraphNode FindStateNode(string stableId)
	{
		if (!_nodes.TryGetValue(stableId ?? string.Empty, out var value))
		{
			return null;
		}
		return value;
	}

	public StateMachineTransitionChip FindTransitionChip(string stableId)
	{
		if (!_transitionChips.TryGetValue(stableId ?? string.Empty, out var value))
		{
			return null;
		}
		return value;
	}

	public bool HasVisualConnection(string sourceStableId, string targetStableId)
	{
		string text = sourceStableId ?? string.Empty;
		string text2 = targetStableId ?? string.Empty;
		if (_visualConnections.Contains((text, text2)) && _visibleStateIds.Contains(text) && _visibleStateIds.Contains(text2) && _nodes.TryGetValue(text, out var value) && _nodes.TryGetValue(targetStableId ?? string.Empty, out var value2))
		{
			return IsNodeConnected(value.Name, 0, value2.Name, 0);
		}
		return false;
	}

	public bool IsStateVisible(string stableId)
	{
		return _visibleStateIds.Contains(stableId ?? string.Empty);
	}

	public bool IsStateCollapsed(string stableId)
	{
		StateMachineLayout stateMachineLayout = GraphController?.Layout;
		bool value = default;
		return (stateMachineLayout?.Collapsed != null && stateMachineLayout.Collapsed.TryGetValue(stableId ?? string.Empty, out value)) & value;
	}

	public void Bind(StateMachineGraphController controller, IStateMachineUndoAdapter undoAdapter)
	{
		if (GraphController != null)
		{
			GraphController.MutationApplied -= OnControllerMutationApplied;
		}
		GraphController = controller;
		UndoAdapter = undoAdapter;
		GraphController?.BindUndoAdapter(undoAdapter);
		if (GraphController != null)
		{
			GraphController.MutationApplied += OnControllerMutationApplied;
		}
		RefreshGraph(preserveContext: false);
	}

	private void OnControllerMutationApplied(StateMachineGraphChange change)
	{
		if (change != null)
		{
			if ((change.Kind & StateMachineGraphChangeKind.Definition) != 0)
			{
				RefreshGraph();
			}
			else if ((change.Kind & StateMachineGraphChangeKind.Layout) != 0)
			{
				RefreshLayoutOnly();
			}
			if ((change.Kind & StateMachineGraphChangeKind.Definition) != 0)
			{
				DefinitionChanged?.Invoke();
			}
			if ((change.Kind & StateMachineGraphChangeKind.Layout) != 0)
			{
				LayoutChanged?.Invoke();
			}
		}
	}

	private void RefreshLayoutOnly()
	{
		if (!IsInsideTree() || GraphController?.ViewModel == null)
		{
			return;
		}
		_refreshing = true;
		try
		{
			IncrementalGraphRefreshCount++;
			for (int i = 0; i < GraphController.ViewModel.Nodes.Count; i++)
			{
				StateMachineNodeViewModel stateMachineNodeViewModel = GraphController.ViewModel.Nodes[i];
				if (stateMachineNodeViewModel != null && _nodes.TryGetValue(stateMachineNodeViewModel.StableId, out var value))
				{
					Vector2 vector = ResolvePosition(stateMachineNodeViewModel.StableId, i);
					if (!value.PositionOffset.IsEqualApprox(vector))
					{
						value.PositionOffset = vector;
					}
				}
			}
			SynchronizeHierarchyPresentation(GraphController.ViewModel);
			SynchronizeGraphConnections(GraphController.ViewModel);
			SynchronizeTransitionChipVisibility();
			if (_layoutMutationMovedStateIds.Count > 0)
			{
				UpdateTransitionChipPositionsForStates(_layoutMutationMovedStateIds, "RefreshLayoutOnly");
			}
			else
			{
				UpdateAllTransitionChipPositions("RefreshLayoutOnly");
			}
			ApplyTransitionChipRuntimeState(_simulation?.CurrentSnapshot);
		}
		finally
		{
			_refreshing = false;
		}
	}

	public void LoadDefinition(StateMachineDefinition definition, StateMachineLayout layout, bool readOnly = false)
	{
		_details?.FlushPendingNumericCommit();
		FlushViewportState();
		GraphController?.SetReadOnly(readOnly);
		GraphController?.LoadDefinition(definition, layout);
		RefreshStateAuthorTypes();
		_viewportDirty = false;
		_detailsContext = DetailsContextKind.Definition;
		_detailsStableId = string.Empty;
		RefreshGraph(preserveContext: false);
	}

	public void RefreshGraph()
	{
		RefreshGraph(preserveContext: true);
	}

	private void RefreshGraph(bool preserveContext)
	{
		if (!IsInsideTree())
		{
			return;
		}
		StateMachineGraphViewModel stateMachineGraphViewModel = GraphController?.ViewModel;
		StateMachineDefinition stateMachineDefinition = stateMachineGraphViewModel?.StateMachineDefinition;
		HashSet<string> selectedStableIds = CaptureSelectedStableIds();
		bool flag = preserveContext && stateMachineDefinition != null && _renderedDefinition == stateMachineDefinition;
		Vector2 scrollOffset = (flag ? ScrollOffset : (stateMachineGraphViewModel?.StateMachineLayout?.ScrollOffset ?? Vector2.Zero));
		float value = (flag ? Zoom : (stateMachineGraphViewModel?.StateMachineLayout?.Zoom ?? 1f));
		_refreshing = true;
		try
		{
			ConfigureMutationControls(stateMachineGraphViewModel);
			if (stateMachineGraphViewModel?.StateMachineDefinition == null)
			{
				RebuildGraphNodes(null);
				SynchronizeTransitionChips(null);
				_renderedDefinition = null;
				_detailsContext = DetailsContextKind.Definition;
				_detailsStableId = string.Empty;
				ApplyTransitionChipSelection(string.Empty);
				ConfigureEmptyState(missingDefinition: true, emptyDefinition: false, readOnly: true);
				_details?.ShowDefinition(null, isReadOnly: true);
				_simulation?.BindDefinition(null);
				return;
			}
			ConfigureEmptyState(missingDefinition: false, stateMachineGraphViewModel.Nodes.Count == 0, !stateMachineGraphViewModel.CanMutate);
			_renderedDefinition = stateMachineGraphViewModel.StateMachineDefinition;
			if (flag)
			{
				IncrementalGraphRefreshCount++;
				SynchronizeGraphNodes(stateMachineGraphViewModel);
			}
			else
			{
				RebuildGraphNodes(stateMachineGraphViewModel);
			}
			SynchronizeHierarchyPresentation(stateMachineGraphViewModel);
			SynchronizeTransitionChips(stateMachineGraphViewModel);
			SynchronizeGraphConnections(stateMachineGraphViewModel);
			Zoom = Mathf.Clamp(value, 0.25f, 2f);
			ScrollOffset = scrollOffset;
			_pendingScrollOffset = ScrollOffset;
			_pendingZoom = Zoom;
			QueueTransitionChipLayoutAfterGraphSettles("RefreshGraph");
			_details?.SetStateChoices(stateMachineGraphViewModel.Nodes);
			RestoreDetailsContext(stateMachineGraphViewModel);
			RestoreSelectedStableIds(selectedStableIds);
			_diagnostics?.ShowDiagnostics(stateMachineGraphViewModel.Diagnostics);
			if (stateMachineGraphViewModel.HasCompositionFailure && _diagnostics != null)
			{
				_diagnostics.Visible = true;
			}
			_simulation?.BindDefinition(stateMachineGraphViewModel.StateMachineDefinition);
		}
		finally
		{
			_refreshing = false;
		}
	}

	private HashSet<string> CaptureSelectedStableIds()
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		foreach (StateMachineGraphNode value in _nodes.Values)
		{
			if (value.Selected && !string.IsNullOrWhiteSpace(value.StableId))
			{
				hashSet.Add(value.StableId);
			}
		}
		return hashSet;
	}

	private void RestoreSelectedStableIds(HashSet<string> selectedStableIds)
	{
		if (selectedStableIds == null || selectedStableIds.Count == 0)
		{
			return;
		}
		foreach (string selectedStableId in selectedStableIds)
		{
			if (_visibleStateIds.Contains(selectedStableId) && _nodes.TryGetValue(selectedStableId, out var value))
			{
				value.Selected = true;
			}
		}
	}

	private void RebuildGraphNodes(StateMachineGraphViewModel viewModel)
	{
		FullGraphRebuildCount++;
		ClearConnections();
		_visualConnections.Clear();
		_visibleStateIds.Clear();
		foreach (StateMachineGraphNode value in _nodes.Values)
		{
			RemoveChild(value);
			value.QueueFree();
		}
		_nodes.Clear();
		if (viewModel == null)
		{
			return;
		}
		for (int i = 0; i < viewModel.Nodes.Count; i++)
		{
			StateMachineNodeViewModel stateMachineNodeViewModel = viewModel.Nodes[i];
			if (stateMachineNodeViewModel?.State != null && !string.IsNullOrWhiteSpace(stateMachineNodeViewModel.StableId))
			{
				StateMachineGraphNode stateMachineGraphNode = CreateGraphNode(stateMachineNodeViewModel);
				stateMachineGraphNode.PositionOffset = ResolvePosition(stateMachineNodeViewModel.StableId, i);
				_nodes[stateMachineNodeViewModel.StableId] = stateMachineGraphNode;
			}
		}
	}

	private void SynchronizeGraphNodes(StateMachineGraphViewModel viewModel)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		for (int i = 0; i < viewModel.Nodes.Count; i++)
		{
			StateMachineNodeViewModel stateMachineNodeViewModel = viewModel.Nodes[i];
			if (stateMachineNodeViewModel?.State != null && !string.IsNullOrWhiteSpace(stateMachineNodeViewModel.StableId))
			{
				hashSet.Add(stateMachineNodeViewModel.StableId);
				if (!_nodes.TryGetValue(stateMachineNodeViewModel.StableId, out var value))
				{
					value = CreateGraphNode(stateMachineNodeViewModel);
					_nodes[stateMachineNodeViewModel.StableId] = value;
				}
				else
				{
					value.Bind(stateMachineNodeViewModel.State, stateMachineNodeViewModel.IsInherited, stateMachineNodeViewModel.IsReadOnly);
				}
				Vector2 vector = ResolvePosition(stateMachineNodeViewModel.StableId, i);
				if (!value.PositionOffset.IsEqualApprox(vector))
				{
					value.PositionOffset = vector;
				}
			}
		}
		List<string> list = new List<string>();
		foreach (string key in _nodes.Keys)
		{
			if (!hashSet.Contains(key))
			{
				list.Add(key);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			string text = list[j];
			RemoveVisualConnectionsForNode(text);
			StateMachineGraphNode stateMachineGraphNode = _nodes[text];
			_nodes.Remove(text);
			RemoveChild(stateMachineGraphNode);
			stateMachineGraphNode.QueueFree();
		}
	}

	private StateMachineGraphNode CreateGraphNode(StateMachineNodeViewModel item)
	{
		StateMachineGraphNode stateMachineGraphNode = new StateMachineGraphNode();
		AddChild(stateMachineGraphNode, forceReadableName: false, InternalMode.Disabled);
		stateMachineGraphNode.Bind(item.State, item.IsInherited, item.IsReadOnly);
		stateMachineGraphNode.InspectRequested += (StateMachineGraphNode selected) =>
		{
			ShowStateDetails(selected.StableId);
		};
		stateMachineGraphNode.InitialStateRequested += (StateMachineGraphNode selected) =>
		{
			SetAsInitialState(selected.State);
		};
		stateMachineGraphNode.CollapseRequested += OnNodeCollapseRequested;
		GraphNodeCreateCount++;
		return stateMachineGraphNode;
	}

	private void SynchronizeHierarchyPresentation(StateMachineGraphViewModel viewModel)
	{
		_visibleStateIds.Clear();
		_parentStateIds.Clear();
		if (viewModel == null)
		{
			return;
		}
		System.Collections.Generic.Dictionary<string, StateMachineStateDefinition> statesById = new System.Collections.Generic.Dictionary<string, StateMachineStateDefinition>(StringComparer.Ordinal);
		System.Collections.Generic.Dictionary<string, StateMachineNodeViewModel> dictionary = new System.Collections.Generic.Dictionary<string, StateMachineNodeViewModel>(StringComparer.Ordinal);
		System.Collections.Generic.Dictionary<string, int> dictionary2 = new System.Collections.Generic.Dictionary<string, int>(StringComparer.Ordinal);
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		for (int i = 0; i < viewModel.Nodes.Count; i++)
		{
			StateMachineNodeViewModel stateMachineNodeViewModel = viewModel.Nodes[i];
			StateMachineStateDefinition stateMachineStateDefinition = stateMachineNodeViewModel?.State;
			if (stateMachineStateDefinition != null && !string.IsNullOrWhiteSpace(stateMachineStateDefinition.StableId))
			{
				statesById[stateMachineStateDefinition.StableId] = stateMachineStateDefinition;
				dictionary[stateMachineStateDefinition.StableId] = stateMachineNodeViewModel;
				_parentStateIds[stateMachineStateDefinition.StableId] = stateMachineStateDefinition.ParentId ?? string.Empty;
				if (!string.IsNullOrWhiteSpace(stateMachineStateDefinition.ParentId))
				{
					dictionary2.TryGetValue(stateMachineStateDefinition.ParentId, out var value);
					dictionary2[stateMachineStateDefinition.ParentId] = value + 1;
				}
				if (!string.IsNullOrWhiteSpace(stateMachineStateDefinition.InitialChildId))
				{
					hashSet.Add(stateMachineStateDefinition.InitialChildId);
				}
			}
		}
		HashSet<string> collapsedIds = new HashSet<string>(StringComparer.Ordinal);
		StateMachineLayout stateMachineLayout = viewModel.StateMachineLayout;
		if (stateMachineLayout?.Collapsed != null)
		{
			StateMachineStateDefinition value2 = default;
			foreach (KeyValuePair<string, bool> item in stateMachineLayout.Collapsed)
			{
				bool flag = !item.Value || !statesById.TryGetValue(item.Key, out value2) || !dictionary2.ContainsKey(item.Key);
				if (!flag)
				{
					StateMachineStateKind kind = value2.Kind;
					bool flag2 = (uint)(kind - 1) <= 1u;
					flag = !flag2;
				}
				if (!flag)
				{
					collapsedIds.Add(item.Key);
				}
			}
		}
		System.Collections.Generic.Dictionary<string, bool> visibility = new System.Collections.Generic.Dictionary<string, bool>(StringComparer.Ordinal);
		HashSet<string> resolving = new HashSet<string>(StringComparer.Ordinal);
		for (int j = 0; j < viewModel.Nodes.Count; j++)
		{
			StateMachineNodeViewModel stateMachineNodeViewModel2 = viewModel.Nodes[j];
			if (stateMachineNodeViewModel2?.State != null && !string.IsNullOrWhiteSpace(stateMachineNodeViewModel2.StableId) && _nodes.TryGetValue(stateMachineNodeViewModel2.StableId, out var value3))
			{
				StateMachineStateDefinition state = stateMachineNodeViewModel2.State;
				dictionary2.TryGetValue(stateMachineNodeViewModel2.StableId, out var value4);
				string parentDisplayName = (statesById.TryGetValue(state.ParentId ?? string.Empty, out var value5) ? value5.DisplayName.ToString() : string.Empty);
				bool canSetAsInitial = value5 != null && value5.Kind == StateMachineStateKind.Compound && !stateMachineNodeViewModel2.IsReadOnly && dictionary.TryGetValue(state.ParentId ?? string.Empty, out var value6) && !value6.IsReadOnly;
				value3.BindHierarchy(state.ParentId, parentDisplayName, value4, hashSet.Contains(stateMachineNodeViewModel2.StableId), string.Equals(stateMachineNodeViewModel2.StableId, viewModel.EffectiveDefinition?.RootStateId ?? viewModel.StateMachineDefinition?.RootStateId, StringComparison.Ordinal), canSetAsInitial, collapsedIds.Contains(stateMachineNodeViewModel2.StableId));
				bool flag3 = ResolveVisible(stateMachineNodeViewModel2.StableId);
				if (!flag3)
				{
					value3.Selected = false;
				}
				value3.Visible = flag3;
				if (flag3)
				{
					_visibleStateIds.Add(stateMachineNodeViewModel2.StableId);
				}
			}
		}
		EnsureDetailsContextVisible();
		ApplySimulationSnapshot(_simulation?.CurrentSnapshot);
		bool ResolveVisible(string stableId)
		{
			if (visibility.TryGetValue(stableId, out var value7))
			{
				return value7;
			}
			if (!statesById.TryGetValue(stableId, out var value8))
			{
				return true;
			}
			if (!resolving.Add(stableId))
			{
				return true;
			}
			string text = value8.ParentId ?? string.Empty;
			bool flag4 = string.IsNullOrWhiteSpace(text) || !statesById.ContainsKey(text) || (!collapsedIds.Contains(text) && ResolveVisible(text));
			resolving.Remove(stableId);
			visibility[stableId] = flag4;
			return flag4;
		}
	}

	private void EnsureDetailsContextVisible()
	{
		if (_detailsContext == DetailsContextKind.State && !_visibleStateIds.Contains(_detailsStableId))
		{
			string text = ResolveVisibleHierarchyProjection(_detailsStableId);
			if (!string.IsNullOrWhiteSpace(text) && !string.Equals(text, _detailsStableId, StringComparison.Ordinal))
			{
				ShowStateDetails(text, selectNode: true);
			}
		}
		else
		{
			if (_detailsContext != DetailsContextKind.Transition)
			{
				return;
			}
			StateMachineGraphTransitionViewModel stateMachineGraphTransitionViewModel = FindTransition(_detailsStableId);
			string text2 = stateMachineGraphTransitionViewModel?.Transition?.SourceStateId ?? string.Empty;
			string text3 = stateMachineGraphTransitionViewModel?.Transition?.TargetStateId ?? string.Empty;
			if (!_visibleStateIds.Contains(text2) || !_visibleStateIds.Contains(text3))
			{
				string text4 = ResolveVisibleHierarchyProjection(text2);
				if (!_visibleStateIds.Contains(text4))
				{
					text4 = ResolveVisibleHierarchyProjection(text3);
				}
				if (_visibleStateIds.Contains(text4))
				{
					ShowStateDetails(text4, selectNode: true);
				}
			}
		}
	}

	private void SynchronizeGraphConnections(StateMachineGraphViewModel viewModel)
	{
		HashSet<(string, string)> hashSet = new HashSet<(string, string)>();
		foreach (StateMachineGraphTransitionViewModel transition in viewModel.Transitions)
		{
			StateMachineTransitionDefinition stateMachineTransitionDefinition = transition?.Transition;
			string text = stateMachineTransitionDefinition?.SourceStateId ?? string.Empty;
			string text2 = stateMachineTransitionDefinition?.TargetStateId ?? string.Empty;
			if (_visibleStateIds.Contains(text) && _visibleStateIds.Contains(text2) && _nodes.ContainsKey(text) && _nodes.ContainsKey(text2))
			{
				hashSet.Add((text, text2));
			}
		}
		List<(string, string)> list = new List<(string, string)>();
		foreach (var visualConnection in _visualConnections)
		{
			if (!hashSet.Contains(visualConnection))
			{
				list.Add(visualConnection);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			RemoveVisualConnection(list[i]);
		}
		foreach (var item in hashSet)
		{
			StateMachineGraphNode stateMachineGraphNode = _nodes[item.Item1];
			StateMachineGraphNode stateMachineGraphNode2 = _nodes[item.Item2];
			if (!IsNodeConnected(stateMachineGraphNode.Name, 0, stateMachineGraphNode2.Name, 0))
			{
				ConnectNode(stateMachineGraphNode.Name, 0, stateMachineGraphNode2.Name, 0);
			}
			_visualConnections.Add(item);
		}
	}

	private void SynchronizeTransitionChips(StateMachineGraphViewModel viewModel)
	{
		TransitionChipSynchronizeCount++;
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		System.Collections.Generic.Dictionary<(string, string), int> dictionary = new System.Collections.Generic.Dictionary<(string, string), int>();
		if (viewModel != null)
		{
			foreach (StateMachineGraphTransitionViewModel transition in viewModel.Transitions)
			{
				StateMachineTransitionDefinition stateMachineTransitionDefinition = transition?.Transition;
				if (stateMachineTransitionDefinition != null && !string.IsNullOrWhiteSpace(stateMachineTransitionDefinition.StableId))
				{
					(string, string) key = (stateMachineTransitionDefinition.SourceStateId ?? string.Empty, stateMachineTransitionDefinition.TargetStateId ?? string.Empty);
					dictionary.TryGetValue(key, out var value);
					dictionary[key] = value + 1;
				}
			}
		}
		_transitionChipGroups.Clear();
		_transitionChipsByState.Clear();
		if (viewModel != null)
		{
			foreach (StateMachineGraphTransitionViewModel transition2 in viewModel.Transitions)
			{
				StateMachineTransitionDefinition stateMachineTransitionDefinition2 = transition2?.Transition;
				if (stateMachineTransitionDefinition2 != null && !string.IsNullOrWhiteSpace(stateMachineTransitionDefinition2.StableId))
				{
					hashSet.Add(stateMachineTransitionDefinition2.StableId);
					if (!_transitionChips.TryGetValue(stateMachineTransitionDefinition2.StableId, out var value2))
					{
						value2 = new StateMachineTransitionChip();
						value2.Bind(transition2, ResolveStateDisplayName(stateMachineTransitionDefinition2.SourceStateId), ResolveStateDisplayName(stateMachineTransitionDefinition2.TargetStateId), dictionary[(stateMachineTransitionDefinition2.SourceStateId ?? string.Empty, stateMachineTransitionDefinition2.TargetStateId ?? string.Empty)] > 1);
						value2.InspectRequested += OnTransitionChipInspectRequested;
						AddChild(value2, forceReadableName: false, InternalMode.Disabled);
						_transitionChips[stateMachineTransitionDefinition2.StableId] = value2;
						TransitionChipCreateCount++;
					}
					else
					{
						value2.Bind(transition2, ResolveStateDisplayName(stateMachineTransitionDefinition2.SourceStateId), ResolveStateDisplayName(stateMachineTransitionDefinition2.TargetStateId), dictionary[(stateMachineTransitionDefinition2.SourceStateId ?? string.Empty, stateMachineTransitionDefinition2.TargetStateId ?? string.Empty)] > 1);
					}
					(string, string) key2 = (value2.SourceStateId, value2.TargetStateId);
					if (!_transitionChipGroups.TryGetValue(key2, out var value3))
					{
						value3 = new List<StateMachineTransitionChip>();
						_transitionChipGroups[key2] = value3;
					}
					value3.Add(value2);
					AddTransitionChipStateIndex(value2.SourceStateId, value2);
					if (!string.Equals(value2.SourceStateId, value2.TargetStateId, StringComparison.Ordinal))
					{
						AddTransitionChipStateIndex(value2.TargetStateId, value2);
					}
				}
			}
		}
		List<string> list = new List<string>();
		foreach (string key3 in _transitionChips.Keys)
		{
			if (!hashSet.Contains(key3))
			{
				list.Add(key3);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			string text = list[i];
			StateMachineTransitionChip stateMachineTransitionChip = _transitionChips[text];
			_transitionChips.Remove(text);
			_activePendingTransitionChipIds.Remove(text);
			_nextPendingTransitionChipIds.Remove(text);
			if (string.Equals(_lastCompletedTransitionChipId, text, StringComparison.Ordinal))
			{
				_lastCompletedTransitionChipId = string.Empty;
			}
			stateMachineTransitionChip.InspectRequested -= OnTransitionChipInspectRequested;
			RemoveChild(stateMachineTransitionChip);
			stateMachineTransitionChip.QueueFree();
		}
		foreach (List<StateMachineTransitionChip> value4 in _transitionChipGroups.Values)
		{
			value4.Sort(CompareTransitionChips);
		}
		SynchronizeTransitionChipVisibility();
		ApplyTransitionChipSelection(_detailsStableId);
		ApplyTransitionChipRuntimeState(_simulation?.CurrentSnapshot);
		QueueTransitionChipLayoutAfterGraphSettles("SynchronizeTransitionChips");
	}

	private void AddTransitionChipStateIndex(string stableId, StateMachineTransitionChip chip)
	{
		if (!string.IsNullOrWhiteSpace(stableId))
		{
			if (!_transitionChipsByState.TryGetValue(stableId, out var value))
			{
				value = new List<StateMachineTransitionChip>();
				_transitionChipsByState[stableId] = value;
			}
			value.Add(chip);
		}
	}

	private void SynchronizeTransitionChipVisibility()
	{
		foreach (StateMachineTransitionChip value in _transitionChips.Values)
		{
			value.Visible = _visibleStateIds.Contains(value.SourceStateId) && _visibleStateIds.Contains(value.TargetStateId) && _nodes.ContainsKey(value.SourceStateId) && _nodes.ContainsKey(value.TargetStateId);
		}
	}

	private void UpdateAllTransitionChipPositions([CallerMemberName] string reason = "")
	{
		LastTransitionChipRelayoutReason = reason ?? string.Empty;
		LastTransitionChipRelayoutCount = 0;
		if (!IsWorkbenchActive)
		{
			LastTransitionChipRelayoutReason += ":hidden";
			return;
		}
		foreach (KeyValuePair<(string, string), List<StateMachineTransitionChip>> transitionChipGroup in _transitionChipGroups)
		{
			UpdateTransitionChipGroupPosition(transitionChipGroup.Key, transitionChipGroup.Value);
		}
		TransitionChipPositionUpdateCount += LastTransitionChipRelayoutCount;
	}

	private void QueueTransitionChipLayoutAfterGraphSettles([CallerMemberName] string reason = "")
	{
		_queuedTransitionChipLayoutReason = reason ?? string.Empty;
		if (!_transitionChipLayoutQueued && IsInsideTree())
		{
			_transitionChipLayoutQueued = true;
			CallDeferred("UpdateTransitionChipLayoutAfterGraphSettles");
		}
	}

	private void UpdateTransitionChipLayoutAfterGraphSettles()
	{
		_transitionChipLayoutQueued = false;
		if (GodotObject.IsInstanceValid(this) && IsInsideTree())
		{
			if (!IsWorkbenchActive)
			{
				_queuedTransitionChipLayoutReason = string.Empty;
				return;
			}
			SynchronizeTransitionChipVisibility();
			UpdateAllTransitionChipPositions(_queuedTransitionChipLayoutReason + ":deferred");
			_queuedTransitionChipLayoutReason = string.Empty;
		}
	}

	private void UpdateTransitionChipPositionsForStates(IEnumerable<string> stableIds, [CallerMemberName] string reason = "")
	{
		LastTransitionChipRelayoutReason = (reason ?? string.Empty) + ":adjacent";
		LastTransitionChipRelayoutCount = 0;
		if (!IsWorkbenchActive)
		{
			LastTransitionChipRelayoutReason += ":hidden";
			return;
		}
		_transitionGroupsNeedingLayout.Clear();
		foreach (string stableId in stableIds)
		{
			if (_transitionChipsByState.TryGetValue(stableId ?? string.Empty, out var value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					StateMachineTransitionChip stateMachineTransitionChip = value[i];
					_transitionGroupsNeedingLayout.Add((stateMachineTransitionChip.SourceStateId, stateMachineTransitionChip.TargetStateId));
				}
			}
		}
		foreach (var item in _transitionGroupsNeedingLayout)
		{
			if (_transitionChipGroups.TryGetValue(item, out var value2))
			{
				UpdateTransitionChipGroupPosition(item, value2);
			}
		}
		TransitionChipPositionUpdateCount += LastTransitionChipRelayoutCount;
	}

	private void UpdateTransitionChipGroupPosition((string Source, string Target) pair, List<StateMachineTransitionChip> group)
	{
		if (group == null || group.Count == 0)
		{
			return;
		}
		if (!_visibleStateIds.Contains(pair.Source) || !_visibleStateIds.Contains(pair.Target) || !_nodes.TryGetValue(pair.Source, out var value) || !_nodes.TryGetValue(pair.Target, out var value2))
		{
			for (int i = 0; i < group.Count; i++)
			{
				group[i].Visible = false;
			}
			return;
		}
		Vector2 graphPosition;
		Vector2 vector;
		if (string.Equals(pair.Source, pair.Target, StringComparison.Ordinal))
		{
			graphPosition = value.PositionOffset + new Vector2(value.Size.X + 82f, value.Size.Y * 0.18f);
			vector = Vector2.Right;
		}
		else
		{
			GetConnectionEndpoints(value, value2, out var from, out var to);
			GetConnectionCurveControls(from, to, ConnectionLinesCurvature, out var controlFrom, out var controlTo);
			graphPosition = from.BezierInterpolate(controlFrom, controlTo, to, 0.52f);
			vector = CubicBezierTangent(from, controlFrom, controlTo, to, 0.52f);
		}
		Vector2 vector2 = ToLocalPosition(graphPosition);
		Vector2 vector3 = vector * Mathf.Max(Zoom, 0.001f);
		Vector2 vector4 = ((vector3.LengthSquared() > 0.001f) ? new Vector2(0f - vector3.Y, vector3.X).Normalized() : Vector2.Up);
		float num = ((string.CompareOrdinal(pair.Source, pair.Target) <= 0) ? 1f : (-1f));
		Vector2 vector5 = (string.Equals(pair.Source, pair.Target, StringComparison.Ordinal) ? Vector2.Zero : (vector4 * 17f * num));
		float num2 = (float)(group.Count - 1) * 0.5f;
		for (int j = 0; j < group.Count; j++)
		{
			StateMachineTransitionChip stateMachineTransitionChip = group[j];
			stateMachineTransitionChip.Visible = true;
			Vector2 vector6 = stateMachineTransitionChip.Size;
			if (vector6.X < 1f || vector6.Y < 1f)
			{
				vector6 = stateMachineTransitionChip.CustomMinimumSize;
			}
			Vector2 vector7 = vector4 * (((float)j - num2) * 32f);
			stateMachineTransitionChip.Position = (vector2 + vector5 + vector7 - vector6 * 0.5f).Round();
			LastTransitionChipRelayoutCount++;
		}
	}

	private void UpdateMovingTransitionChipPositions()
	{
		bool flag = false;
		for (int i = 0; i < _movingStateIds.Count; i++)
		{
			string key = _movingStateIds[i];
			if (_nodes.TryGetValue(key, out var value) && (!_movingStatePositions.TryGetValue(key, out var value2) || !value2.IsEqualApprox(value.PositionOffset)))
			{
				_movingStatePositions[key] = value.PositionOffset;
				flag = true;
			}
		}
		if (flag)
		{
			UpdateTransitionChipPositionsForStates(_movingStateIds, "UpdateMovingTransitionChipPositions");
		}
	}

	private void OnTransitionChipInspectRequested(string stableId)
	{
		ShowTransitionDetails(stableId, openDetails: true);
	}

	private string ResolveStateDisplayName(string stableId)
	{
		if (_nodes.TryGetValue(stableId ?? string.Empty, out var value) && value.State != null && !value.State.DisplayName.IsEmpty)
		{
			return value.State.DisplayName.ToString();
		}
		return stableId ?? string.Empty;
	}

	private static int CompareTransitionChips(StateMachineTransitionChip left, StateMachineTransitionChip right)
	{
		int num = right.Priority.CompareTo(left.Priority);
		if (num != 0)
		{
			return num;
		}
		int num2 = left.DeclarationOrder.CompareTo(right.DeclarationOrder);
		if (num2 == 0)
		{
			return string.CompareOrdinal(left.StableId, right.StableId);
		}
		return num2;
	}

	private Vector2 ToLocalPosition(Vector2 graphPosition)
	{
		return (graphPosition - ScrollOffset) * Mathf.Max(Zoom, 0.001f);
	}

	private static void GetConnectionEndpoints(StateMachineGraphNode source, StateMachineGraphNode target, out Vector2 from, out Vector2 to)
	{
		from = source.PositionOffset + ((source.GetOutputPortCount() > 0) ? source.GetOutputPortPosition(0) : GetSlotEdgePosition(source, output: true));
		to = target.PositionOffset + ((target.GetInputPortCount() > 0) ? target.GetInputPortPosition(0) : GetSlotEdgePosition(target, output: false));
	}

	private static Vector2 GetSlotEdgePosition(StateMachineGraphNode node, bool output)
	{
		Control control = node?.GetNodeOrNull<Control>("PuzzleTile");
		float y = ((GodotObject.IsInstanceValid(control) && control.Size.Y > 0f) ? (control.Position.Y + control.Size.Y * 0.5f) : (node.Size.Y * 0.5f));
		return new Vector2(output ? node.Size.X : 0f, y);
	}

	private static void GetConnectionCurveControls(Vector2 from, Vector2 to, float curvature, out Vector2 controlFrom, out Vector2 controlTo)
	{
		float num = Mathf.Abs(to.X - from.X);
		float num2 = Mathf.Max(24f, num * Mathf.Clamp(curvature, 0f, 1f));
		if (to.X < from.X)
		{
			num2 = Mathf.Max(num2, from.DistanceTo(to) * 0.5f);
		}
		controlFrom = from + Vector2.Right * num2;
		controlTo = to - Vector2.Right * num2;
	}

	private static Vector2 CubicBezierTangent(Vector2 from, Vector2 controlFrom, Vector2 controlTo, Vector2 to, float amount)
	{
		float num = 1f - amount;
		return 3f * num * num * (controlFrom - from) + 6f * num * amount * (controlTo - controlFrom) + 3f * amount * amount * (to - controlTo);
	}

	private void RemoveVisualConnectionsForNode(string stableId)
	{
		List<(string, string)> list = new List<(string, string)>();
		foreach (var visualConnection in _visualConnections)
		{
			if (string.Equals(visualConnection.Source, stableId, StringComparison.Ordinal) || string.Equals(visualConnection.Target, stableId, StringComparison.Ordinal))
			{
				list.Add(visualConnection);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			RemoveVisualConnection(list[i]);
		}
	}

	private void RemoveVisualConnection((string Source, string Target) pair)
	{
		if (_nodes.TryGetValue(pair.Source, out var value) && _nodes.TryGetValue(pair.Target, out var value2) && IsNodeConnected(value.Name, 0, value2.Name, 0))
		{
			DisconnectNode(value.Name, 0, value2.Name, 0);
		}
		_visualConnections.Remove(pair);
	}

	public void NavigateToStableId(string stableId)
	{
		if (_nodes.TryGetValue(stableId ?? string.Empty, out var value))
		{
			ShowStateDetails(stableId, selectNode: true);
			ScrollOffset = value.PositionOffset - Size * 0.5f / Mathf.Max(Zoom, 0.001f) + value.Size * 0.5f;
			return;
		}
		StateMachineGraphTransitionViewModel stateMachineGraphTransitionViewModel = FindTransition(stableId);
		if (stateMachineGraphTransitionViewModel != null)
		{
			ShowTransitionDetails(stateMachineGraphTransitionViewModel.StableId, openDetails: true);
		}
	}

	public void ApplyPickedResource(Resource owner, string propertyName, Resource pickedResource)
	{
		ApplyFieldChange(owner, new StringName(propertyName ?? string.Empty), Variant.From(in pickedResource));
	}

	public Array<StringName> GetSelectedStateNodes()
	{
		Array<StringName> selectedNodes = GetSelectedNodes();
		Array<StringName> array = new Array<StringName>();
		foreach (StringName item in selectedNodes)
		{
			StateMachineGraphNode stateMachineGraphNode = FindNodeByGraphName(item);
			if (stateMachineGraphNode != null)
			{
				array.Add(stateMachineGraphNode.StableId);
			}
		}
		return array;
	}

	public Array<StringName> GetSelectedNodes()
	{
		Array<StringName> array = new Array<StringName>();
		foreach (StateMachineGraphNode value in _nodes.Values)
		{
			if (value.Visible && value.Selected)
			{
				array.Add(value.Name);
			}
		}
		return array;
	}

	private void BuildGameToolbar()
	{
		HBoxContainer hBoxContainer = Call("get_menu_hbox").As<HBoxContainer>();
		if (hBoxContainer == null)
		{
			return;
		}
		_compactToolbar = new HBoxContainer
		{
			Name = "StateMachineCompactToolbar",
			SizeFlagsHorizontal = SizeFlags.ShrinkBegin
		};
		hBoxContainer.AddChild(_compactToolbar, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.MoveChild(_compactToolbar, 0);
		_compactToolbar.AddChild(new Label
		{
			Name = "StateMachineToolbarTitle",
			Text = "  状态机"
		}, forceReadableName: false, InternalMode.Disabled);
		_newStateKind = new OptionButton
		{
			Name = "NewStateKind",
			TooltipText = "选择新拼图种类",
			CustomMinimumSize = new Vector2(92f, 0f)
		};
		_newStateKind.AddItem("◆ 原子", 0);
		_newStateKind.AddItem("▣ 复合", 1);
		_newStateKind.AddItem("▦ 并行", 2);
		_newStateKind.AddItem("◉ 历史", 3);
		_compactToolbar.AddChild(_newStateKind, forceReadableName: false, InternalMode.Disabled);
		_newStateType = new OptionButton
		{
			Name = "NewStateTypeOption",
			TooltipText = "选择状态拼图的 C# Resource 类型",
			CustomMinimumSize = new Vector2(136f, 0f)
		};
		_newStateType.ItemSelected += (long _) =>
		{
			RefreshCompactStateAuthorTypes();
		};
		_compactToolbar.AddChild(_newStateType, forceReadableName: false, InternalMode.Disabled);
		RefreshStateAuthorTypes();
		_compactStateTypeMenu = new MenuButton
		{
			Name = "CompactStateTypeMenu",
			Text = "♯",
			TooltipText = "选择状态拼图的 C# Resource 类型",
			Visible = false
		};
		_compactStateTypeMenu.GetPopup().IdPressed += (long id) =>
		{
			SelectStateAuthorTypeIndex((int)id);
		};
		_compactToolbar.AddChild(_compactStateTypeMenu, forceReadableName: false, InternalMode.Disabled);
		RefreshCompactStateAuthorTypes();
		_addStateButton = new Button
		{
			Name = "AddStatePuzzleButton",
			Text = "+",
			TooltipText = "在画布中心放置状态拼图"
		};
		_addStateButton.Pressed += AddStateAtViewportCenter;
		_compactToolbar.AddChild(_addStateButton, forceReadableName: false, InternalMode.Disabled);
		_narrowAddStateMenu = new MenuButton
		{
			Name = "NarrowAddStateMenu",
			Text = "＋◆",
			TooltipText = "选择并新增状态拼图",
			Visible = false
		};
		PopupMenu popup = _narrowAddStateMenu.GetPopup();
		AddStateKindMenuItem(popup, StateMachineStateKind.Atomic, "新增原子状态", "res://addons/godot_state_charts/atomic_state.svg");
		AddStateKindMenuItem(popup, StateMachineStateKind.Compound, "新增复合状态", "res://addons/godot_state_charts/compound_state.svg");
		AddStateKindMenuItem(popup, StateMachineStateKind.Parallel, "新增并行状态", "res://addons/godot_state_charts/parallel_state.svg");
		AddStateKindMenuItem(popup, StateMachineStateKind.History, "新增历史状态", "res://addons/godot_state_charts/history_state.svg");
		popup.IdPressed += (long id) =>
		{
			AddStateAtViewportCenter((StateMachineStateKind)id);
		};
		_compactToolbar.AddChild(_narrowAddStateMenu, forceReadableName: false, InternalMode.Disabled);
		Button button = new Button
		{
			Name = "DefinitionOverviewButton",
			Text = "◎",
			TooltipText = "返回状态机总览与继承配置"
		};
		button.Pressed += ShowDefinitionOverview;
		_compactToolbar.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		_detailsToggleButton = new Button
		{
			Name = "ToggleStateDetailsButton",
			Text = "◇",
			TooltipText = "显示或隐藏当前配置拼图"
		};
		_detailsToggleButton.Pressed += ToggleDetailsPanel;
		_compactToolbar.AddChild(_detailsToggleButton, forceReadableName: false, InternalMode.Disabled);
		Button button2 = new Button
		{
			Name = "UndoStateMachineButton",
			Text = "↶",
			TooltipText = "撤销"
		};
		button2.Pressed += () =>
		{
			GraphController?.Undo();
		};
		_compactToolbar.AddChild(button2, forceReadableName: false, InternalMode.Disabled);
		Button button3 = new Button
		{
			Name = "RedoStateMachineButton",
			Text = "↷",
			TooltipText = "重做"
		};
		button3.Pressed += () =>
		{
			GraphController?.Redo();
		};
		_compactToolbar.AddChild(button3, forceReadableName: false, InternalMode.Disabled);
		_simulationToggle = new Button
		{
			Name = "SimulationToggle",
			Text = "▶",
			TooltipText = "打开无副作用状态机沙盒",
			ToggleMode = true,
			ButtonPressed = false
		};
		_simulationToggle.Toggled += (bool visible) =>
		{
			if (_simulation != null)
			{
				_simulation.Visible = visible && IsWorkbenchActive;
			}
		};
		_compactToolbar.AddChild(_simulationToggle, forceReadableName: false, InternalMode.Disabled);
		MenuButton menuButton = new MenuButton
		{
			Name = "StateMachineMoreMenu",
			Text = "⋯",
			TooltipText = "诊断、预览和面板"
		};
		PopupMenu popup2 = menuButton.GetPopup();
		popup2.AddItem("✓ 运行诊断", 1, Key.None);
		popup2.AddItem("▶ 编译预览", 2, Key.None);
		popup2.AddSeparator();
		popup2.AddItem("显示 / 隐藏配置拼图", 3, Key.None);
		popup2.AddItem("显示 / 隐藏诊断", 4, Key.None);
		popup2.IdPressed += OnToolbarActionPressed;
		_compactToolbar.AddChild(menuButton, forceReadableName: false, InternalMode.Disabled);
	}

	private void RefreshStateAuthorTypes()
	{
		if (!GodotObject.IsInstanceValid(_newStateType))
		{
			return;
		}
		string selectedStateAuthorTypeId = GetSelectedStateAuthorTypeId();
		_newStateType.Clear();
		Texture2D texture2D = ResourceLoader.Load<Texture2D>("res://addons/godot_state_charts/atomic_state.svg", null, ResourceLoader.CacheMode.Reuse);
		_newStateType.AddIconItem(texture2D, "基础状态");
		_newStateType.SetItemMetadata(0, string.Empty);
		int idx = 0;
		IReadOnlyList<StateMachineAuthorTypeDescriptor> stateTypes = StateMachineAuthorTypeRegistry.GetStateTypes();
		for (int i = 0; i < stateTypes.Count; i++)
		{
			StateMachineAuthorTypeDescriptor stateMachineAuthorTypeDescriptor = stateTypes[i];
			Texture2D texture = ((!string.IsNullOrWhiteSpace(stateMachineAuthorTypeDescriptor.IconPath)) ? ResourceLoader.Load<Texture2D>(stateMachineAuthorTypeDescriptor.IconPath, null, ResourceLoader.CacheMode.Reuse) : texture2D);
			_newStateType.AddIconItem(texture, stateMachineAuthorTypeDescriptor.DisplayName);
			int num = _newStateType.ItemCount - 1;
			_newStateType.SetItemMetadata(num, stateMachineAuthorTypeDescriptor.TypeId);
			_newStateType.GetPopup().SetItemTooltip(num, string.IsNullOrWhiteSpace(stateMachineAuthorTypeDescriptor.Description) ? stateMachineAuthorTypeDescriptor.TypeId : stateMachineAuthorTypeDescriptor.Description);
			if (string.Equals(stateMachineAuthorTypeDescriptor.TypeId, selectedStateAuthorTypeId, StringComparison.Ordinal))
			{
				idx = num;
			}
		}
		_newStateType.Select(idx);
		RefreshCompactStateAuthorTypes();
	}

	private void OnAuthorTypesChanged()
	{
		RefreshStateAuthorTypes();
		ApplyResponsiveLayout();
		if (IsInsideTree() && GraphController?.ViewModel?.StateMachineDefinition != null)
		{
			RefreshGraph();
		}
	}

	private string GetSelectedStateAuthorTypeId()
	{
		if (!GodotObject.IsInstanceValid(_newStateType) || _newStateType.Selected < 0 || _newStateType.Selected >= _newStateType.ItemCount)
		{
			return string.Empty;
		}
		return _newStateType.GetItemMetadata(_newStateType.Selected).AsString();
	}

	private void RefreshCompactStateAuthorTypes()
	{
		if (GodotObject.IsInstanceValid(_compactStateTypeMenu) && GodotObject.IsInstanceValid(_newStateType))
		{
			PopupMenu popup = _compactStateTypeMenu.GetPopup();
			popup.Clear();
			for (int i = 0; i < _newStateType.ItemCount; i++)
			{
				popup.AddIconRadioCheckItem(_newStateType.GetItemIcon(i), _newStateType.GetItemText(i), i, Key.None);
				popup.SetItemChecked(i, i == _newStateType.Selected);
				popup.SetItemMetadata(i, _newStateType.GetItemMetadata(i));
				popup.SetItemTooltip(i, _newStateType.GetPopup().GetItemTooltip(i));
			}
			int num = Mathf.Max(0, _newStateType.Selected);
			_compactStateTypeMenu.TooltipText = ((num < _newStateType.ItemCount) ? ("C# 状态类型：" + _newStateType.GetItemText(num)) : "选择状态拼图的 C# Resource 类型");
		}
	}

	private void SelectStateAuthorTypeIndex(int index)
	{
		if (GodotObject.IsInstanceValid(_newStateType) && index >= 0 && index < _newStateType.ItemCount)
		{
			_newStateType.Select(index);
			_newStateType.EmitSignal(OptionButton.SignalName.ItemSelected, index);
			RefreshCompactStateAuthorTypes();
		}
	}

	private void ConfigureMutationControls(StateMachineGraphViewModel viewModel)
	{
		bool flag = viewModel?.CanMutate ?? false;
		if (GodotObject.IsInstanceValid(_newStateKind))
		{
			_newStateKind.Disabled = !flag;
		}
		if (GodotObject.IsInstanceValid(_newStateType))
		{
			_newStateType.Disabled = !flag;
		}
		if (GodotObject.IsInstanceValid(_compactStateTypeMenu))
		{
			_compactStateTypeMenu.Disabled = !flag;
		}
		if (GodotObject.IsInstanceValid(_addStateButton))
		{
			_addStateButton.Disabled = !flag;
		}
		if (GodotObject.IsInstanceValid(_narrowAddStateMenu))
		{
			_narrowAddStateMenu.Disabled = !flag;
		}
		if (GodotObject.IsInstanceValid(_simulationToggle))
		{
			_simulationToggle.Disabled = viewModel?.HasCompositionFailure ?? false;
			if (_simulationToggle.Disabled)
			{
				_simulationToggle.ButtonPressed = false;
			}
		}
		if (GodotObject.IsInstanceValid(_compositionFailureBanner))
		{
			_compositionFailureBanner.Visible = viewModel?.HasCompositionFailure ?? false;
		}
	}

	private void ShowCompositionFailureDiagnostics()
	{
		if (GodotObject.IsInstanceValid(_diagnostics))
		{
			_diagnostics.ShowDiagnostics(GraphController?.Diagnostics);
			_diagnostics.Visible = true;
		}
	}

	private static void AddStateKindMenuItem(PopupMenu popup, StateMachineStateKind kind, string label, string iconPath)
	{
		Texture2D texture = ResourceLoader.Load<Texture2D>(iconPath, null, ResourceLoader.CacheMode.Reuse);
		popup.AddIconItem(texture, label, (int)kind, Key.None);
	}

	private void BindZoomButtons()
	{
		HBoxContainer zoomHBox = GetZoomHBox();
		if (zoomHBox == null)
		{
			return;
		}
		foreach (Node child in zoomHBox.GetChildren())
		{
			if (child is BaseButton baseButton)
			{
				baseButton.Pressed += QueueViewportCapture;
			}
		}
	}

	private void ApplyResponsiveLayout()
	{
		bool flag = Size.X <= 620f;
		if (_newStateKind != null)
		{
			_newStateKind.Visible = !flag;
		}
		bool flag2 = !flag && Size.X > 920f;
		if (_newStateType != null)
		{
			_newStateType.Visible = flag2;
			_newStateType.CustomMinimumSize = new Vector2(136f, 0f);
		}
		if (_compactStateTypeMenu != null)
		{
			_compactStateTypeMenu.Visible = !flag2 && _newStateType != null && _newStateType.ItemCount > 1;
		}
		if (_addStateButton != null)
		{
			_addStateButton.Visible = !flag;
		}
		if (_narrowAddStateMenu != null)
		{
			_narrowAddStateMenu.Visible = flag;
		}
		Label label = _compactToolbar?.GetNodeOrNull<Label>("StateMachineToolbarTitle");
		if (label != null)
		{
			label.Visible = !flag;
		}
		if (_details != null)
		{
			if (flag)
			{
				_details.AnchorLeft = 0f;
				_details.AnchorTop = 0.38f;
				_details.AnchorRight = 1f;
				_details.AnchorBottom = 1f;
				_details.OffsetLeft = 12f;
				_details.OffsetTop = 0f;
				_details.OffsetRight = -12f;
				_details.OffsetBottom = -12f;
				_details.Visible = _narrowDetailsVisible;
			}
			else
			{
				_details.AnchorLeft = 1f;
				_details.AnchorTop = 0f;
				_details.AnchorRight = 1f;
				_details.AnchorBottom = 0.72f;
				_details.OffsetLeft = -310f;
				_details.OffsetTop = 48f;
				_details.OffsetRight = -10f;
				_details.OffsetBottom = -8f;
				_details.Visible = _detailsRequestedVisible;
			}
		}
		if (_diagnostics != null)
		{
			_diagnostics.OffsetRight = (flag ? (-12) : (-322));
		}
		QueueTransitionChipLayoutAfterGraphSettles("ApplyResponsiveLayout");
	}

	private void ToggleDetailsPanel()
	{
		if (Size.X <= 620f)
		{
			_narrowDetailsVisible = !_narrowDetailsVisible;
		}
		else
		{
			_detailsRequestedVisible = !_detailsRequestedVisible;
		}
		ApplyResponsiveLayout();
	}

	private void OpenDetailsForSelection()
	{
		if (!(Size.X > 620f) && !_narrowDetailsVisible)
		{
			_narrowDetailsVisible = true;
			ApplyResponsiveLayout();
		}
	}

	private void BuildLegacyToolbar()
	{
		HBoxContainer hBoxContainer = Call("get_menu_hbox").As<HBoxContainer>();
		if (hBoxContainer != null)
		{
			Label node = new Label
			{
				Text = "  状态拼图库"
			};
			hBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			_newStateKind = new OptionButton
			{
				TooltipText = "选择新拼图种类"
			};
			_newStateKind.AddItem("◆ 原子", 0);
			_newStateKind.AddItem("▣ 复合", 1);
			_newStateKind.AddItem("▥ 并行", 2);
			_newStateKind.AddItem("◴ 历史", 3);
			hBoxContainer.AddChild(_newStateKind, forceReadableName: false, InternalMode.Disabled);
			Button button = new Button
			{
				Text = "+ 放置状态"
			};
			button.Pressed += AddStateAtViewportCenter;
			hBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
			Button button2 = new Button
			{
				Text = "↶",
				TooltipText = "撤销状态机操作"
			};
			button2.Pressed += () =>
			{
				GraphController?.Undo();
			};
			hBoxContainer.AddChild(button2, forceReadableName: false, InternalMode.Disabled);
			Button button3 = new Button
			{
				Text = "↷",
				TooltipText = "重做状态机操作"
			};
			button3.Pressed += () =>
			{
				GraphController?.Redo();
			};
			hBoxContainer.AddChild(button3, forceReadableName: false, InternalMode.Disabled);
			Button button4 = new Button
			{
				Text = "✓ 运行诊断"
			};
			button4.Pressed += RunDiagnostics;
			hBoxContainer.AddChild(button4, forceReadableName: false, InternalMode.Disabled);
			Button button5 = new Button
			{
				Text = "▶ 编译预览",
				TooltipText = "不启动关卡，先编译并检查状态机"
			};
			button5.Pressed += CompilePreview;
			hBoxContainer.AddChild(button5, forceReadableName: false, InternalMode.Disabled);
			Button button6 = new Button
			{
				Text = "▶ 沙盒",
				TooltipText = "可视模拟事件、自动与延迟转换；不启动角色或关卡",
				ToggleMode = true,
				ButtonPressed = true
			};
			button6.Toggled += (bool visible) =>
			{
				_simulation.Visible = visible;
			};
			hBoxContainer.AddChild(button6, forceReadableName: false, InternalMode.Disabled);
			Button button7 = new Button
			{
				Text = "配置",
				ToggleMode = true,
				ButtonPressed = true
			};
			button7.Toggled += (bool visible) =>
			{
				_details.Visible = visible;
			};
			hBoxContainer.AddChild(button7, forceReadableName: false, InternalMode.Disabled);
			Button button8 = new Button
			{
				Text = "诊断",
				ToggleMode = true,
				ButtonPressed = true
			};
			button8.Toggled += (bool visible) =>
			{
				_diagnostics.Visible = visible;
			};
			hBoxContainer.AddChild(button8, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void BuildOverlayPanels()
	{
		_compositionFailureBanner = new PanelContainer
		{
			Name = "CompositionFailureBanner",
			LayoutMode = 1,
			AnchorLeft = 0f,
			AnchorTop = 0f,
			AnchorRight = 1f,
			AnchorBottom = 0f,
			OffsetLeft = 12f,
			OffsetTop = 44f,
			OffsetRight = -322f,
			OffsetBottom = 76f,
			MouseFilter = MouseFilterEnum.Ignore,
			ZIndex = 20,
			Visible = false
		};
		_compositionFailureBanner.AddChild(new Label
		{
			Text = "⚠ 继承合成失败：图结构和剪贴板已锁定，请在右侧更换或清除基定义。",
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
			ThemeTypeVariation = "HeaderSmall",
			Modulate = new Color(1f, 0.42f, 0.34f)
		}, forceReadableName: false, InternalMode.Disabled);
		AddChild(_compositionFailureBanner, forceReadableName: false, InternalMode.Disabled);
		_emptyStatePanel = new PanelContainer
		{
			Name = "StateMachineEmptyStatePanel",
			LayoutMode = 1,
			AnchorLeft = 0.5f,
			AnchorTop = 0.5f,
			AnchorRight = 0.5f,
			AnchorBottom = 0.5f,
			OffsetLeft = -190f,
			OffsetTop = -92f,
			OffsetRight = 190f,
			OffsetBottom = 92f,
			MouseFilter = MouseFilterEnum.Stop,
			ZIndex = 40,
			Visible = false
		};
		MarginContainer marginContainer = new MarginContainer();
		marginContainer.AddThemeConstantOverride("margin_left", 20);
		marginContainer.AddThemeConstantOverride("margin_top", 16);
		marginContainer.AddThemeConstantOverride("margin_right", 20);
		marginContainer.AddThemeConstantOverride("margin_bottom", 16);
		_emptyStatePanel.AddChild(marginContainer, forceReadableName: false, InternalMode.Disabled);
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			Alignment = BoxContainer.AlignmentMode.Center
		};
		marginContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(new Label
		{
			Text = "状态机拼图工作台",
			HorizontalAlignment = HorizontalAlignment.Center,
			ThemeTypeVariation = "HeaderSmall"
		}, forceReadableName: false, InternalMode.Disabled);
		_emptyStateHint = new Label
		{
			Text = "新建一个状态机，或打开已有 .tres / .res 资源",
			HorizontalAlignment = HorizontalAlignment.Center
		};
		vBoxContainer.AddChild(_emptyStateHint, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			Alignment = BoxContainer.AlignmentMode.Center
		};
		vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		_createDefinitionButton = new Button
		{
			Name = "CreateDefinitionButton",
			Text = "+ 新建状态机"
		};
		_createDefinitionButton.Pressed += () =>
		{
			CreateDefinitionRequested?.Invoke();
		};
		hBoxContainer.AddChild(_createDefinitionButton, forceReadableName: false, InternalMode.Disabled);
		_openDefinitionButton = new Button
		{
			Name = "OpenDefinitionButton",
			Text = "打开状态机"
		};
		_openDefinitionButton.Pressed += () =>
		{
			OpenDefinitionRequested?.Invoke();
		};
		hBoxContainer.AddChild(_openDefinitionButton, forceReadableName: false, InternalMode.Disabled);
		_initializeDefinitionButton = new Button
		{
			Name = "InitializeDefinitionButton",
			Text = "✦ 初始化入口与初始状态",
			Visible = false
		};
		_initializeDefinitionButton.Pressed += InitializeEmptyDefinition;
		hBoxContainer.AddChild(_initializeDefinitionButton, forceReadableName: false, InternalMode.Disabled);
		AddChild(_emptyStatePanel, forceReadableName: false, InternalMode.Disabled);
		_details = new StateMachineDetailsPanel
		{
			Name = "DetailsPanel",
			LayoutMode = 1,
			AnchorsPreset = 11,
			AnchorLeft = 1f,
			AnchorTop = 0f,
			AnchorRight = 1f,
			AnchorBottom = 0.72f,
			OffsetLeft = -310f,
			OffsetTop = 48f,
			OffsetRight = -10f,
			OffsetBottom = -8f,
			MouseFilter = MouseFilterEnum.Stop,
			ZIndex = 40
		};
		AddChild(_details, forceReadableName: false, InternalMode.Disabled);
		_details.FieldChanged += ApplyFieldChange;
		_details.ResourcePickerRequested += OnDetailsResourcePickerRequested;
		_details.GuardEditRequested += OnDetailsGuardEditRequested;
		_details.CallbackSourceOpenRequested += OnDetailsCallbackSourceOpenRequested;
		_details.AliasAddRequested += OnAliasAddRequested;
		_details.AliasRemoveRequested += OnAliasRemoveRequested;
		_details.NavigateRequested += NavigateToStableId;
		_details.TransitionCreateRequested += OnTransitionCreateRequested;
		_details.TransitionRemoveRequested += OnTransitionRemoveRequested;
		_details.StateOverrideCreateRequested += OnStateOverrideCreateRequested;
		_details.StateOverrideRestoreRequested += OnStateOverrideRestoreRequested;
		_details.TransitionOverrideCreateRequested += OnTransitionOverrideCreateRequested;
		_details.TransitionOverrideRestoreRequested += OnTransitionOverrideRestoreRequested;
		_diagnostics = new StateMachineDiagnosticsPanel
		{
			Name = "DiagnosticsPanel",
			LayoutMode = 1,
			AnchorLeft = 0f,
			AnchorTop = 1f,
			AnchorRight = 1f,
			AnchorBottom = 1f,
			OffsetLeft = 12f,
			OffsetTop = -166f,
			OffsetRight = -322f,
			OffsetBottom = -12f,
			MouseFilter = MouseFilterEnum.Stop,
			ZIndex = 40,
			Visible = false
		};
		AddChild(_diagnostics, forceReadableName: false, InternalMode.Disabled);
		_diagnostics.NavigateRequested += NavigateToStableId;
		_simulation = new StateMachineSimulationPanel
		{
			Name = "SimulationPanel",
			LayoutMode = 1,
			AnchorLeft = 0f,
			AnchorTop = 0f,
			AnchorRight = 0f,
			AnchorBottom = 0f,
			OffsetLeft = 12f,
			OffsetTop = 48f,
			OffsetRight = 372f,
			OffsetBottom = 304f,
			MouseFilter = MouseFilterEnum.Stop,
			ZIndex = 40,
			Visible = false
		};
		AddChild(_simulation, forceReadableName: false, InternalMode.Disabled);
		_simulation.StateChanged += ApplySimulationSnapshot;
		_simulation.NavigateRequested += NavigateToStableId;
	}

	private void ConfigureEmptyState(bool missingDefinition, bool emptyDefinition, bool readOnly)
	{
		if (!GodotObject.IsInstanceValid(_emptyStatePanel))
		{
			return;
		}
		bool flag = missingDefinition | emptyDefinition;
		_emptyStatePanel.Visible = flag;
		if (flag)
		{
			if (GodotObject.IsInstanceValid(_emptyStateHint))
			{
				Label emptyStateHint = _emptyStateHint;
				string text;
				if (missingDefinition)
				{
					text = "新建一个状态机，或打开已有 .tres / .res 资源";
				}
				else
				{
					text = (readOnly ? "这个只读状态机还没有可显示的状态" : "这个状态机还没有入口；点击初始化即可开始拼图");
				}
				emptyStateHint.Text = text;
			}
			if (GodotObject.IsInstanceValid(_createDefinitionButton))
			{
				_createDefinitionButton.Visible = missingDefinition;
			}
			if (GodotObject.IsInstanceValid(_openDefinitionButton))
			{
				_openDefinitionButton.Visible = missingDefinition;
			}
			if (GodotObject.IsInstanceValid(_initializeDefinitionButton))
			{
				_initializeDefinitionButton.Visible = emptyDefinition;
				_initializeDefinitionButton.Disabled = readOnly;
			}
		}
	}

	private void InitializeEmptyDefinition()
	{
		StateMachineGraphViewModel stateMachineGraphViewModel = GraphController?.ViewModel;
		if (stateMachineGraphViewModel?.StateMachineDefinition != null && stateMachineGraphViewModel.CanMutate && stateMachineGraphViewModel.Nodes.Count <= 0)
		{
			string addedStableId = string.Empty;
			Vector2 position = ScrollOffset + Size * 0.5f / Mathf.Max(Zoom, 0.001f);
			ExecuteMutation(() =>
			{
				addedStableId = GraphController.AddStateHierarchySafeAtPosition("状态机入口", StateMachineStateKind.Compound, position, string.Empty);
			});
			if (!string.IsNullOrWhiteSpace(addedStableId))
			{
				ShowStateDetails(addedStableId, selectNode: true);
			}
		}
	}

	private void DisconnectOverlaySignals()
	{
		if (GodotObject.IsInstanceValid(_details))
		{
			_details.FieldChanged -= ApplyFieldChange;
			_details.ResourcePickerRequested -= OnDetailsResourcePickerRequested;
			_details.GuardEditRequested -= OnDetailsGuardEditRequested;
			_details.CallbackSourceOpenRequested -= OnDetailsCallbackSourceOpenRequested;
			_details.AliasAddRequested -= OnAliasAddRequested;
			_details.AliasRemoveRequested -= OnAliasRemoveRequested;
			_details.NavigateRequested -= NavigateToStableId;
			_details.TransitionCreateRequested -= OnTransitionCreateRequested;
			_details.TransitionRemoveRequested -= OnTransitionRemoveRequested;
			_details.StateOverrideCreateRequested -= OnStateOverrideCreateRequested;
			_details.StateOverrideRestoreRequested -= OnStateOverrideRestoreRequested;
			_details.TransitionOverrideCreateRequested -= OnTransitionOverrideCreateRequested;
			_details.TransitionOverrideRestoreRequested -= OnTransitionOverrideRestoreRequested;
		}
		if (GodotObject.IsInstanceValid(_diagnostics))
		{
			_diagnostics.NavigateRequested -= NavigateToStableId;
		}
		if (GodotObject.IsInstanceValid(_simulation))
		{
			_simulation.StateChanged -= ApplySimulationSnapshot;
			_simulation.NavigateRequested -= NavigateToStableId;
		}
		if (GodotObject.IsInstanceValid(_canvasContextMenu))
		{
			_canvasContextMenu.IdPressed -= OnCanvasContextAction;
		}
	}

	private void OnDetailsResourcePickerRequested(Resource resource, string property)
	{
		ResourcePickerRequested?.Invoke(resource, property);
	}

	private void OnDetailsGuardEditRequested(Resource guard, Resource owner, string property, int index)
	{
		GuardEditRequested?.Invoke(guard, owner, property, index);
	}

	private void OnDetailsCallbackSourceOpenRequested(StateMachineCallbackCatalogEntry entry, StateMachineCallbackPhase phase)
	{
		CallbackSourceOpenRequested?.Invoke(entry, phase);
	}

	private void OnAliasAddRequested(string alias, string target)
	{
		ExecuteMutation(() =>
		{
			GraphController.AddAlias(alias, target);
		});
	}

	private void OnAliasRemoveRequested(string alias)
	{
		ExecuteMutation(() =>
		{
			GraphController.RemoveAlias(alias);
		});
	}

	private void OnTransitionCreateRequested(string sourceStateId, string targetStateId, StateMachineTriggerKind triggerKind, string eventName, double delaySeconds, int priority, string authorTypeId)
	{
		StateMachineGraphController graphController = GraphController;
		if (graphController != null && graphController.ViewModel?.CanMutate == true)
		{
			string addedTransitionId = string.Empty;
			ExecuteMutation(() =>
			{
				addedTransitionId = GraphController.AddTransition(sourceStateId, targetStateId, triggerKind, eventName, delaySeconds, priority, authorTypeId);
			});
			if (!string.IsNullOrWhiteSpace(addedTransitionId))
			{
				ShowTransitionDetails(addedTransitionId, openDetails: true);
			}
		}
	}

	private void OnTransitionRemoveRequested(string stableId)
	{
		ExecuteMutation(() =>
		{
			GraphController.RemoveTransition(stableId);
		});
	}

	private void OnStateOverrideCreateRequested(string stableId)
	{
		ExecuteMutation(() =>
		{
			GraphController.CreateStateOverride(stableId);
		});
	}

	private void OnStateOverrideRestoreRequested(string stableId)
	{
		ExecuteMutation(() =>
		{
			GraphController.RestoreInheritedState(stableId);
		});
	}

	private void OnTransitionOverrideCreateRequested(string stableId)
	{
		ExecuteMutation(() =>
		{
			GraphController.CreateTransitionOverride(stableId);
		});
	}

	private void OnTransitionOverrideRestoreRequested(string stableId)
	{
		ExecuteMutation(() =>
		{
			GraphController.RestoreInheritedTransition(stableId);
		});
	}

	private void BuildCanvasContextMenu()
	{
		_canvasContextMenu = new PopupMenu
		{
			Name = "StateMachineCanvasContextMenu"
		};
		AddChild(_canvasContextMenu, forceReadableName: false, InternalMode.Disabled);
		Texture2D texture = ResourceLoader.Load<Texture2D>("res://addons/godot_state_charts/atomic_state.svg", null, ResourceLoader.CacheMode.Reuse);
		Texture2D texture2 = ResourceLoader.Load<Texture2D>("res://addons/godot_state_charts/compound_state.svg", null, ResourceLoader.CacheMode.Reuse);
		Texture2D texture3 = ResourceLoader.Load<Texture2D>("res://addons/godot_state_charts/parallel_state.svg", null, ResourceLoader.CacheMode.Reuse);
		Texture2D texture4 = ResourceLoader.Load<Texture2D>("res://addons/godot_state_charts/history_state.svg", null, ResourceLoader.CacheMode.Reuse);
		Texture2D texture5 = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ActionCopy.svg", null, ResourceLoader.CacheMode.Reuse);
		Texture2D texture6 = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ActionCut.svg", null, ResourceLoader.CacheMode.Reuse);
		Texture2D texture7 = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Duplicate.svg", null, ResourceLoader.CacheMode.Reuse);
		Texture2D texture8 = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ActionPaste.svg", null, ResourceLoader.CacheMode.Reuse);
		Texture2D texture9 = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/Remove.svg", null, ResourceLoader.CacheMode.Reuse);
		_canvasContextMenu.AddIconItem(texture, "新增原子状态", 100, Key.None);
		_canvasContextMenu.AddIconItem(texture2, "新增复合状态", 101, Key.None);
		_canvasContextMenu.AddIconItem(texture3, "新增并行状态", 102, Key.None);
		_canvasContextMenu.AddIconItem(texture4, "新增历史状态", 103, Key.None);
		_canvasContextMenu.AddSeparator();
		_canvasContextMenu.AddIconItem(texture5, "复制", 200, Key.None);
		_canvasContextMenu.AddIconItem(texture6, "剪切", 201, Key.None);
		_canvasContextMenu.AddIconItem(texture7, "重复", 202, Key.None);
		_canvasContextMenu.AddIconItem(texture8, "粘贴到这里", 203, Key.None);
		_canvasContextMenu.AddIconItem(texture9, "删除", 204, Key.None);
		_canvasContextMenu.IdPressed += OnCanvasContextAction;
	}

	private void OnSurfaceVisibilityChanged()
	{
		SetWorkbenchActive(IsVisibleInTree());
	}

	private void OnToolbarActionPressed(long id)
	{
		switch ((ToolbarAction)id)
		{
		case ToolbarAction.Validate:
			RunDiagnostics();
			_diagnostics.Visible = true;
			break;
		case ToolbarAction.Compile:
			CompilePreview();
			_diagnostics.Visible = true;
			break;
		case ToolbarAction.ToggleDetails:
			ToggleDetailsPanel();
			break;
		case ToolbarAction.ToggleDiagnostics:
			_diagnostics.Visible = !_diagnostics.Visible;
			break;
		}
	}

	private void ApplySimulationSnapshot(StateMachineSnapshot snapshot)
	{
		_activeStateProjectionIds.Clear();
		_pendingStateProjectionIds.Clear();
		if (snapshot != null)
		{
			foreach (string activeStateId in snapshot.ActiveStateIds)
			{
				_activeStateProjectionIds.Add(ResolveVisibleHierarchyProjection(activeStateId));
			}
			foreach (string pendingTransitionId in snapshot.PendingTransitionIds)
			{
				StateMachineGraphTransitionViewModel stateMachineGraphTransitionViewModel = FindTransition(pendingTransitionId);
				if (stateMachineGraphTransitionViewModel?.Transition != null)
				{
					_pendingStateProjectionIds.Add(ResolveVisibleHierarchyProjection(stateMachineGraphTransitionViewModel.Transition.SourceStateId ?? string.Empty));
				}
			}
		}
		foreach (StateMachineGraphNode value in _nodes.Values)
		{
			value.SetSimulationState(_activeStateProjectionIds.Contains(value.StableId), _pendingStateProjectionIds.Contains(value.StableId));
		}
		ApplyTransitionChipRuntimeState(snapshot);
	}

	private void ApplyTransitionChipRuntimeState(StateMachineSnapshot snapshot)
	{
		_nextPendingTransitionChipIds.Clear();
		if (snapshot?.PendingTransitionIds != null)
		{
			foreach (string pendingTransitionId in snapshot.PendingTransitionIds)
			{
				if (!string.IsNullOrWhiteSpace(pendingTransitionId))
				{
					_nextPendingTransitionChipIds.Add(pendingTransitionId);
				}
			}
		}
		string text = _simulation?.LastCompletedTransitionStableId ?? string.Empty;
		foreach (string activePendingTransitionChipId in _activePendingTransitionChipIds)
		{
			if (!_nextPendingTransitionChipIds.Contains(activePendingTransitionChipId) && _transitionChips.TryGetValue(activePendingTransitionChipId, out var value))
			{
				value.SetRuntimeState(pending: false, 0.0, string.Equals(activePendingTransitionChipId, text, StringComparison.Ordinal));
			}
		}
		foreach (string nextPendingTransitionChipId in _nextPendingTransitionChipIds)
		{
			if (_transitionChips.TryGetValue(nextPendingTransitionChipId, out var value2))
			{
				double value3 = 0.0;
				snapshot?.PendingDelayRemaining?.TryGetValue(nextPendingTransitionChipId, out value3);
				value2.SetRuntimeState(pending: true, value3, recentlyCompleted: false);
			}
		}
		if (!string.Equals(_lastCompletedTransitionChipId, text, StringComparison.Ordinal))
		{
			if (!string.IsNullOrWhiteSpace(_lastCompletedTransitionChipId) && !_nextPendingTransitionChipIds.Contains(_lastCompletedTransitionChipId) && _transitionChips.TryGetValue(_lastCompletedTransitionChipId, out var value4))
			{
				value4.SetRuntimeState(pending: false, 0.0, recentlyCompleted: false);
			}
			if (!string.IsNullOrWhiteSpace(text) && !_nextPendingTransitionChipIds.Contains(text) && _transitionChips.TryGetValue(text, out var value5))
			{
				value5.SetRuntimeState(pending: false, 0.0, recentlyCompleted: true);
			}
			_lastCompletedTransitionChipId = text;
		}
		_activePendingTransitionChipIds.Clear();
		foreach (string nextPendingTransitionChipId2 in _nextPendingTransitionChipIds)
		{
			_activePendingTransitionChipIds.Add(nextPendingTransitionChipId2);
		}
	}

	private void ApplyTransitionChipSelection(string stableId)
	{
		string text = ((_detailsContext == DetailsContextKind.Transition) ? (stableId ?? string.Empty) : string.Empty);
		if (!string.Equals(_selectedTransitionChipId, text, StringComparison.Ordinal))
		{
			if (!string.IsNullOrWhiteSpace(_selectedTransitionChipId) && _transitionChips.TryGetValue(_selectedTransitionChipId, out var value))
			{
				value.SetSelectedTransition(selected: false);
			}
			_selectedTransitionChipId = text;
			if (!string.IsNullOrWhiteSpace(_selectedTransitionChipId) && _transitionChips.TryGetValue(_selectedTransitionChipId, out var value2))
			{
				value2.SetSelectedTransition(selected: true);
			}
		}
	}

	private string ResolveVisibleHierarchyProjection(string stableId)
	{
		string value = stableId ?? string.Empty;
		int num = _parentStateIds.Count + 1;
		while (!string.IsNullOrWhiteSpace(value) && num-- > 0)
		{
			if (_visibleStateIds.Contains(value))
			{
				return value;
			}
			if (!_parentStateIds.TryGetValue(value, out value))
			{
				break;
			}
		}
		return stableId ?? string.Empty;
	}

	private Vector2 ResolvePosition(string stableId, int index)
	{
		if (Layout != null && Layout.Positions.TryGetValue(stableId, out var value))
		{
			return value;
		}
		return new Vector2(80 + index % 4 * 292, 96 + index / 4 * 184);
	}

	private void AddStateAtViewportCenter()
	{
		StateMachineStateKind kind = ((_newStateKind != null) ? ((StateMachineStateKind)_newStateKind.GetSelectedId()) : StateMachineStateKind.Atomic);
		AddStateAtViewportCenter(kind);
	}

	private void AddStateAtViewportCenter(StateMachineStateKind kind)
	{
		if (GraphController?.ViewModel != null && GraphController.ViewModel.StateMachineDefinition != null && GraphController.ViewModel.CanMutate)
		{
			Vector2 position = ScrollOffset + Size * 0.5f / Mathf.Max(Zoom, 0.001f);
			AddStateAtGraphPosition(kind, position);
		}
	}

	private void AddStateAtGraphPosition(StateMachineStateKind kind, Vector2 position)
	{
		if (GraphController?.ViewModel != null && GraphController.ViewModel.StateMachineDefinition != null && GraphController.ViewModel.CanMutate)
		{
			string displayName = kind switch
			{
				StateMachineStateKind.Compound => "新复合状态", 
				StateMachineStateKind.Parallel => "新并行状态", 
				StateMachineStateKind.History => "新历史状态", 
				_ => "新原子状态", 
			};
			string addedStableId = string.Empty;
			string parentId = ResolveParentForNewState(kind);
			ExecuteMutation(() =>
			{
				addedStableId = GraphController.AddStateHierarchySafeAtPosition(displayName, kind, position, parentId, GetSelectedStateAuthorTypeId());
			});
			if (!string.IsNullOrWhiteSpace(addedStableId))
			{
				ShowStateDetails(addedStableId, selectNode: true);
			}
		}
	}

	private string ResolveParentForNewState(StateMachineStateKind kind)
	{
		StateMachineGraphViewModel stateMachineGraphViewModel = GraphController?.ViewModel;
		if (stateMachineGraphViewModel?.StateMachineDefinition == null)
		{
			return string.Empty;
		}
		foreach (StringName selectedStateNode in GetSelectedStateNodes())
		{
			_nodes.TryGetValue(selectedStateNode.ToString(), out var value);
			if (CanContainStateKind(value?.State, kind))
			{
				return value.StableId;
			}
			StateMachineStateDefinition stateMachineStateDefinition = FindVisibleState(value?.State?.ParentId);
			if (CanContainStateKind(stateMachineStateDefinition, kind))
			{
				return stateMachineStateDefinition.StableId;
			}
		}
		StateMachineStateDefinition stateMachineStateDefinition2 = FindVisibleState(stateMachineGraphViewModel.StateMachineDefinition.RootStateId);
		if (CanContainStateKind(stateMachineStateDefinition2, kind) || (stateMachineStateDefinition2 != null && stateMachineStateDefinition2.Kind == StateMachineStateKind.Atomic && kind != StateMachineStateKind.History))
		{
			return stateMachineStateDefinition2.StableId;
		}
		return string.Empty;
	}

	private StateMachineStateDefinition FindVisibleState(string stableId)
	{
		if (string.IsNullOrWhiteSpace(stableId) || GraphController?.ViewModel == null)
		{
			return null;
		}
		foreach (StateMachineNodeViewModel node in GraphController.ViewModel.Nodes)
		{
			if (node?.State != null && string.Equals(node.StableId, stableId, StringComparison.Ordinal))
			{
				return node.State;
			}
		}
		return null;
	}

	private static bool CanContainStateKind(StateMachineStateDefinition state, StateMachineStateKind childKind)
	{
		if (state != null)
		{
			if (state.Kind != StateMachineStateKind.Compound)
			{
				if (state.Kind == StateMachineStateKind.Parallel)
				{
					return childKind != StateMachineStateKind.History;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private void OnConnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
	{
		if (_refreshing || GraphController == null)
		{
			return;
		}
		StateMachineGraphNode source = FindNodeByGraphName(fromNode);
		StateMachineGraphNode target = FindNodeByGraphName(toNode);
		if (source != null && target != null && GraphController.ViewModel.CanMutate)
		{
			string addedTransitionId = string.Empty;
			ExecuteMutation(() =>
			{
				addedTransitionId = GraphController.AddTransition(source.StableId, target.StableId, StateMachineTriggerKind.Event, "", 0.0, 0, _details?.SelectedTransitionAuthorTypeId ?? string.Empty);
			});
			if (!string.IsNullOrWhiteSpace(addedTransitionId))
			{
				ShowTransitionDetails(addedTransitionId, openDetails: true);
			}
		}
	}

	private void OnDisconnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
	{
		if (_refreshing || GraphController == null)
		{
			return;
		}
		StateMachineGraphNode stateMachineGraphNode = FindNodeByGraphName(fromNode);
		StateMachineGraphNode stateMachineGraphNode2 = FindNodeByGraphName(toNode);
		if (stateMachineGraphNode == null || stateMachineGraphNode2 == null)
		{
			return;
		}
		List<StateMachineGraphTransitionViewModel> list = new List<StateMachineGraphTransitionViewModel>();
		foreach (StateMachineGraphTransitionViewModel transition2 in GraphController.ViewModel.Transitions)
		{
			StateMachineTransitionDefinition transition = transition2.Transition;
			if (!transition2.IsReadOnly && transition.SourceStateId == stateMachineGraphNode.StableId && transition.TargetStateId == stateMachineGraphNode2.StableId)
			{
				list.Add(transition2);
			}
		}
		if (list.Count == 1)
		{
			string stableId = list[0].StableId;
			ExecuteMutation(() =>
			{
				GraphController.RemoveTransition(stableId);
			});
		}
		else
		{
			if (list.Count <= 1)
			{
				return;
			}
			StateMachineGraphTransitionViewModel stateMachineGraphTransitionViewModel = list.Find((StateMachineGraphTransitionViewModel item) => _detailsContext == DetailsContextKind.Transition && item.StableId == _detailsStableId);
			if (stateMachineGraphTransitionViewModel != null)
			{
				string stableId2 = stateMachineGraphTransitionViewModel.StableId;
				ExecuteMutation(() =>
				{
					GraphController.RemoveTransition(stableId2);
				});
			}
			else
			{
				LastConnectionSelectionGroupSize = list.Count;
				ShowTransitionDetails(list[0].StableId, openDetails: true);
			}
		}
	}

	private void OnDeleteNodesRequest(Array<StringName> requested)
	{
		if (GraphController == null)
		{
			return;
		}
		List<string> stableIds = new List<string>();
		foreach (StringName item in requested)
		{
			StateMachineGraphNode stateMachineGraphNode = FindNodeByGraphName(item);
			if (stateMachineGraphNode != null && !stateMachineGraphNode.IsReadOnly && stateMachineGraphNode.StableId != GraphController.Definition?.RootStateId)
			{
				stableIds.Add(stateMachineGraphNode.StableId);
			}
		}
		if (stableIds.Count > 0)
		{
			ExecuteMutation(() =>
			{
				GraphController.RemoveStates(stableIds);
			});
		}
	}

	private void OnCopyNodesRequest()
	{
		if (GraphController == null || !GraphController.CanCopySubgraph)
		{
			return;
		}
		List<string> list = new List<string>();
		System.Collections.Generic.Dictionary<string, Vector2> dictionary = new System.Collections.Generic.Dictionary<string, Vector2>(StringComparer.Ordinal);
		foreach (StringName selectedStateNode in GetSelectedStateNodes())
		{
			string text = selectedStateNode.ToString();
			list.Add(text);
			if (_nodes.TryGetValue(text, out var value))
			{
				dictionary[text] = value.PositionOffset;
			}
		}
		GraphController.CopySubgraph(list, dictionary);
	}

	private void OnCutNodesRequest()
	{
		OnCopyNodesRequest();
		OnDeleteNodesRequest(GetSelectedNodes());
	}

	private void OnPasteNodesRequest()
	{
		if (GraphController == null || !GraphController.CanPasteSubgraph)
		{
			return;
		}
		IReadOnlyDictionary<string, string> pasted = null;
		ExecuteMutation(() =>
		{
			pasted = GraphController.PasteSubgraphAt(_lastPointerGraphPosition);
		});
		if (pasted == null || pasted.Count == 0)
		{
			return;
		}
		foreach (StateMachineGraphNode value2 in _nodes.Values)
		{
			value2.Selected = false;
		}
		string text = string.Empty;
		foreach (string value3 in pasted.Values)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				text = value3;
			}
			if (_nodes.TryGetValue(value3, out var value))
			{
				value.Selected = true;
			}
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			ShowStateDetails(text);
		}
	}

	private void OnDuplicateNodesRequest()
	{
		OnCopyNodesRequest();
		OnPasteNodesRequest();
	}

	private void OnPopupRequest(Vector2 atPosition)
	{
		if (_canvasContextMenu != null)
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
		bool flag = GraphController?.ViewModel?.StateMachineDefinition == null || !GraphController.ViewModel.CanMutate;
		bool flag2 = GetSelectedStateNodes().Count > 0;
		long[] array = new long[4] { 100L, 101L, 102L, 103L };
		foreach (long num in array)
		{
			_canvasContextMenu.SetItemDisabled(_canvasContextMenu.GetItemIndex((int)num), flag);
		}
		PopupMenu canvasContextMenu = _canvasContextMenu;
		int itemIndex = _canvasContextMenu.GetItemIndex(200);
		StateMachineGraphController graphController = GraphController;
		canvasContextMenu.SetItemDisabled(itemIndex, graphController == null || !graphController.CanCopySubgraph || !flag2);
		_canvasContextMenu.SetItemDisabled(_canvasContextMenu.GetItemIndex(201), flag || !flag2);
		_canvasContextMenu.SetItemDisabled(_canvasContextMenu.GetItemIndex(202), flag || !flag2);
		PopupMenu canvasContextMenu2 = _canvasContextMenu;
		int itemIndex2 = _canvasContextMenu.GetItemIndex(203);
		int disabled;
		if (!flag)
		{
			StateMachineGraphController graphController2 = GraphController;
			disabled = ((graphController2 == null || !graphController2.CanPasteSubgraph) ? 1 : 0);
		}
		else
		{
			disabled = 1;
		}
		canvasContextMenu2.SetItemDisabled(itemIndex2, (byte)disabled != 0);
		_canvasContextMenu.SetItemDisabled(_canvasContextMenu.GetItemIndex(204), flag || !flag2);
	}

	private void OnCanvasContextAction(long id)
	{
		long num = id - 100;
		if ((ulong)num <= 3uL)
		{
			switch ((int)num)
			{
			case 0:
				AddStateAtGraphPosition(StateMachineStateKind.Atomic, _lastPointerGraphPosition);
				return;
			case 1:
				AddStateAtGraphPosition(StateMachineStateKind.Compound, _lastPointerGraphPosition);
				return;
			case 2:
				AddStateAtGraphPosition(StateMachineStateKind.Parallel, _lastPointerGraphPosition);
				return;
			case 3:
				AddStateAtGraphPosition(StateMachineStateKind.History, _lastPointerGraphPosition);
				return;
			}
		}
		long num2 = id - 200;
		if ((ulong)num2 <= 4uL)
		{
			switch ((int)num2)
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
				OnDeleteNodesRequest(GetSelectedNodes());
				break;
			}
		}
	}

	private Vector2 ToGraphPosition(Vector2 localPosition)
	{
		return ScrollOffset + localPosition / Mathf.Max(Zoom, 0.001f);
	}

	private void OnBeginNodeMove()
	{
		_moveOrigins.Clear();
		_movingStateIds.Clear();
		_movingStatePositions.Clear();
		foreach (StringName selectedStateNode in GetSelectedStateNodes())
		{
			if (_nodes.TryGetValue(selectedStateNode.ToString(), out var value))
			{
				_moveOrigins[value.StableId] = value.PositionOffset;
				_movingStateIds.Add(value.StableId);
				_movingStatePositions[value.StableId] = value.PositionOffset;
			}
		}
		_trackingNodeMove = _movingStateIds.Count > 0;
		SetProcess(_trackingNodeMove && IsWorkbenchActive);
	}

	private void OnEndNodeMove()
	{
		UpdateMovingTransitionChipPositions();
		_trackingNodeMove = false;
		SetProcess(enable: false);
		if (_refreshing || GraphController == null)
		{
			_moveOrigins.Clear();
			_movingStateIds.Clear();
			_movingStatePositions.Clear();
			return;
		}
		System.Collections.Generic.Dictionary<string, Vector2> dictionary = new System.Collections.Generic.Dictionary<string, Vector2>(StringComparer.Ordinal);
		foreach (KeyValuePair<string, Vector2> moveOrigin in _moveOrigins)
		{
			if (_nodes.TryGetValue(moveOrigin.Key, out var value))
			{
				if (value.IsReadOnly)
				{
					value.PositionOffset = moveOrigin.Value;
				}
				else if (!value.PositionOffset.IsEqualApprox(moveOrigin.Value))
				{
					dictionary[moveOrigin.Key] = value.PositionOffset;
				}
			}
		}
		UpdateTransitionChipPositionsForStates(_movingStateIds, "OnEndNodeMove");
		_moveOrigins.Clear();
		_movingStateIds.Clear();
		_movingStatePositions.Clear();
		if (dictionary.Count == 0)
		{
			return;
		}
		_layoutMutationMovedStateIds.Clear();
		foreach (string key in dictionary.Keys)
		{
			_layoutMutationMovedStateIds.Add(key);
		}
		try
		{
			GraphController.MoveGraphNodes(dictionary);
		}
		finally
		{
			_layoutMutationMovedStateIds.Clear();
		}
	}

	private void OnScrollOffsetChanged(Vector2 scrollOffset)
	{
		if (!_refreshing)
		{
			QueueTransitionChipLayoutAfterGraphSettles("OnScrollOffsetChanged");
			QueueViewportPersistence(scrollOffset, Zoom);
		}
	}

	private void OnGraphGuiInput(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseMotion inputEventMouseMotion)
		{
			_lastPointerGraphPosition = ToGraphPosition(inputEventMouseMotion.Position);
		}
		else if (inputEvent is InputEventMouseButton inputEventMouseButton)
		{
			_lastPointerGraphPosition = ToGraphPosition(inputEventMouseButton.Position);
		}
		if (inputEvent is InputEventMouseButton { Pressed: not false } inputEventMouseButton2 && inputEventMouseButton2.ButtonIndex == MouseButton.Left && TrySelectConnectionAt(inputEventMouseButton2.Position))
		{
			AcceptEvent();
			return;
		}
		if (inputEvent is InputEventKey { Pressed: not false, Echo: false, CtrlPressed: not false } inputEventKey)
		{
			bool num = inputEventKey.Keycode == Key.Z && !inputEventKey.ShiftPressed;
			bool flag = inputEventKey.Keycode == Key.Y || (inputEventKey.Keycode == Key.Z && inputEventKey.ShiftPressed);
			if (num)
			{
				IStateMachineUndoAdapter undoAdapter = UndoAdapter;
				if (undoAdapter != null && undoAdapter.CanUndo)
				{
					GraphController.Undo();
					AcceptEvent();
					return;
				}
			}
			if (flag)
			{
				IStateMachineUndoAdapter undoAdapter2 = UndoAdapter;
				if (undoAdapter2 != null && undoAdapter2.CanRedo)
				{
					GraphController.Redo();
					AcceptEvent();
					return;
				}
			}
		}
		InputEventMouseButton inputEventMouseButton3 = inputEvent as InputEventMouseButton;
		bool flag2 = inputEventMouseButton3?.Pressed ?? false;
		if (flag2)
		{
			MouseButton buttonIndex = inputEventMouseButton3.ButtonIndex;
			bool flag3 = (((ulong)(buttonIndex - 4) <= 1uL) ? true : false);
			flag2 = flag3;
		}
		if (flag2)
		{
			QueueViewportCapture();
		}
		else if (inputEvent is InputEventMagnifyGesture)
		{
			QueueViewportCapture();
		}
	}

	private bool TrySelectConnectionAt(Vector2 localPosition)
	{
		if (GraphController?.ViewModel == null)
		{
			return false;
		}
		Vector2 point = ToGraphPosition(localPosition);
		float num = 18f / Mathf.Max(Zoom, 0.001f);
		foreach (var visualConnection in _visualConnections)
		{
			if (!_nodes.TryGetValue(visualConnection.Source, out var value) || !_nodes.TryGetValue(visualConnection.Target, out var value2))
			{
				continue;
			}
			GetConnectionEndpoints(value, value2, out var from, out var to);
			if (DistanceToConnectionCurve(point, from, to, ConnectionLinesCurvature) > num)
			{
				continue;
			}
			List<StateMachineGraphTransitionViewModel> list = new List<StateMachineGraphTransitionViewModel>();
			foreach (StateMachineGraphTransitionViewModel transition in GraphController.ViewModel.Transitions)
			{
				if (transition?.Transition?.SourceStateId == visualConnection.Source && transition.Transition.TargetStateId == visualConnection.Target)
				{
					list.Add(transition);
				}
			}
			if (list.Count != 0)
			{
				int num2 = list.FindIndex((StateMachineGraphTransitionViewModel item) => _detailsContext == DetailsContextKind.Transition && item.StableId == _detailsStableId);
				StateMachineGraphTransitionViewModel stateMachineGraphTransitionViewModel = list[(num2 + 1) % list.Count];
				LastConnectionSelectionGroupSize = list.Count;
				return ShowTransitionDetails(stateMachineGraphTransitionViewModel.StableId, openDetails: true);
			}
		}
		LastConnectionSelectionGroupSize = 0;
		return false;
	}

	private static float DistanceToSegment(Vector2 point, Vector2 from, Vector2 to)
	{
		Vector2 vector = to - from;
		float num = vector.LengthSquared();
		if (num <= 0.001f)
		{
			return point.DistanceTo(from);
		}
		float num2 = Mathf.Clamp((point - from).Dot(vector) / num, 0f, 1f);
		return point.DistanceTo(from + vector * num2);
	}

	private static float DistanceToConnectionCurve(Vector2 point, Vector2 from, Vector2 to, float curvature)
	{
		GetConnectionCurveControls(from, to, curvature, out var controlFrom, out var controlTo);
		float num = 3.4028235E+38f;
		Vector2 vector = from;
		for (int i = 1; i <= 24; i++)
		{
			float t = (float)i / 24f;
			Vector2 vector2 = from.BezierInterpolate(controlFrom, controlTo, to, t);
			num = Mathf.Min(num, DistanceToSegment(point, vector, vector2));
			vector = vector2;
		}
		return num;
	}

	private void QueueViewportCapture()
	{
		if (!_viewportCaptureQueued)
		{
			_viewportCaptureQueued = true;
			CallDeferred("CaptureViewportAfterInput");
		}
	}

	private void CaptureViewportAfterInput()
	{
		_viewportCaptureQueued = false;
		if (GodotObject.IsInstanceValid(this) && !_refreshing)
		{
			QueueTransitionChipLayoutAfterGraphSettles("CaptureViewportAfterInput");
			QueueViewportPersistence(ScrollOffset, Zoom);
		}
	}

	private void QueueViewportPersistence(Vector2 scrollOffset, float zoom)
	{
		if (!_refreshing && GraphController?.Layout != null)
		{
			_pendingScrollOffset = scrollOffset;
			_pendingZoom = Mathf.Clamp(zoom, 0.25f, 2f);
			_viewportDirty = true;
			_viewportDebounceTimer?.Start();
		}
	}

	public void FlushViewportState()
	{
		if (_viewportDirty && GraphController?.Layout != null)
		{
			_viewportDebounceTimer?.Stop();
			_viewportDirty = false;
			if (GraphController.UpdateViewportState(_pendingScrollOffset, _pendingZoom))
			{
				LayoutChanged?.Invoke();
			}
		}
	}

	private void OnNodeSelected(Node selected)
	{
		if (!_refreshing && selected is StateMachineGraphNode stateMachineGraphNode)
		{
			ShowStateDetails(stateMachineGraphNode.StableId);
		}
	}

	private void OnNodeCollapseRequested(StateMachineGraphNode node, bool collapsed)
	{
		if (!_refreshing && GraphController?.Layout != null && node != null && node.HierarchyChildCount > 0 && node.IsHierarchyCollapsed != collapsed)
		{
			ShowStateDetails(node.StableId, selectNode: true);
			ExecuteMutation(() =>
			{
				GraphController.SetCollapsed(node.StableId, collapsed);
			});
		}
	}

	private bool ShowStateDetails(string stableId, bool selectNode = false)
	{
		_details?.FlushPendingNumericCommit();
		if (GraphController == null)
		{
			return false;
		}
		foreach (StateMachineNodeViewModel node in GraphController.ViewModel.Nodes)
		{
			if (!(node.StableId == stableId))
			{
				continue;
			}
			_detailsContext = DetailsContextKind.State;
			_detailsStableId = stableId;
			ApplyTransitionChipSelection(string.Empty);
			if (selectNode && _visibleStateIds.Contains(stableId) && _nodes.TryGetValue(stableId, out var value))
			{
				foreach (StateMachineGraphNode value2 in _nodes.Values)
				{
					value2.Selected = false;
				}
				value.Selected = true;
			}
			_details.ShowState(node.State, node.IsInherited, node.IsLocalOverride, GraphController.ViewModel.IsReadOnly || GraphController.ViewModel.HasCompositionFailure);
			OpenDetailsForSelection();
			return true;
		}
		return false;
	}

	private bool ShowTransitionDetails(string stableId, bool openDetails = false)
	{
		_details?.FlushPendingNumericCommit();
		StateMachineGraphTransitionViewModel stateMachineGraphTransitionViewModel = FindTransition(stableId);
		if (stateMachineGraphTransitionViewModel == null)
		{
			return false;
		}
		_detailsContext = DetailsContextKind.Transition;
		_detailsStableId = stateMachineGraphTransitionViewModel.StableId;
		ApplyTransitionChipSelection(stateMachineGraphTransitionViewModel.StableId);
		_details?.ShowTransition(stateMachineGraphTransitionViewModel.Transition, stateMachineGraphTransitionViewModel.IsInherited, stateMachineGraphTransitionViewModel.IsLocalOverride, GraphController.ViewModel.IsReadOnly || GraphController.ViewModel.HasCompositionFailure);
		if (openDetails)
		{
			OpenDetailsForSelection();
		}
		return true;
	}

	public void ShowDefinitionOverview()
	{
		_details?.FlushPendingNumericCommit();
		if (GraphController?.ViewModel?.StateMachineDefinition == null)
		{
			return;
		}
		foreach (StateMachineGraphNode value in _nodes.Values)
		{
			value.Selected = false;
		}
		_detailsContext = DetailsContextKind.Definition;
		_detailsStableId = string.Empty;
		ApplyTransitionChipSelection(string.Empty);
		_details?.ShowDefinition(GraphController.ViewModel.StateMachineDefinition, GraphController.ViewModel.IsReadOnly, GraphController.ViewModel.HasCompositionFailure);
		OpenDetailsForSelection();
	}

	private void RestoreDetailsContext(StateMachineGraphViewModel viewModel)
	{
		if ((_detailsContext != DetailsContextKind.State || !ShowStateDetails(_detailsStableId, selectNode: true)) && (_detailsContext != DetailsContextKind.Transition || !ShowTransitionDetails(_detailsStableId)))
		{
			_detailsContext = DetailsContextKind.Definition;
			_detailsStableId = string.Empty;
			ApplyTransitionChipSelection(string.Empty);
			_details?.ShowDefinition(viewModel?.StateMachineDefinition, viewModel?.IsReadOnly ?? true, viewModel?.HasCompositionFailure ?? false);
		}
	}

	private StateMachineGraphTransitionViewModel FindTransition(string stableId)
	{
		if (GraphController == null)
		{
			return null;
		}
		foreach (StateMachineGraphTransitionViewModel transition in GraphController.ViewModel.Transitions)
		{
			if (transition.StableId == stableId)
			{
				return transition;
			}
		}
		return null;
	}

	private void SetAsInitialState(StateMachineStateDefinition child)
	{
		if (child == null || string.IsNullOrWhiteSpace(child.ParentId))
		{
			return;
		}
		StateMachineNodeViewModel stateMachineNodeViewModel = null;
		StateMachineNodeViewModel stateMachineNodeViewModel2 = null;
		foreach (StateMachineNodeViewModel item in GraphController?.ViewModel?.Nodes ?? System.Array.Empty<StateMachineNodeViewModel>())
		{
			if (item?.StableId == child.StableId)
			{
				stateMachineNodeViewModel = item;
			}
			if (item?.StableId == child.ParentId)
			{
				stateMachineNodeViewModel2 = item;
			}
		}
		if (stateMachineNodeViewModel == null || stateMachineNodeViewModel.IsReadOnly || stateMachineNodeViewModel2 == null || stateMachineNodeViewModel2.IsReadOnly)
		{
			return;
		}
		StateMachineStateDefinition state = stateMachineNodeViewModel2.State;
		if (state != null && state.Kind == StateMachineStateKind.Compound)
		{
			ExecuteMutation(() =>
			{
				GraphController.SetInitialState(child.ParentId, child.StableId);
			});
		}
	}

	private void ApplyFieldChange(GodotObject owner, StringName propertyName, Variant value)
	{
		if (GraphController == null || owner == null)
		{
			return;
		}
		string text = propertyName.ToString();
		if (owner is StateMachineDefinition)
		{
			if (text == "RootStateId")
			{
				ExecuteMutation(() =>
				{
					GraphController.SetRootState(value.AsString());
				});
			}
			else if (text == "BaseDefinition")
			{
				StateMachineDefinition candidate = ((value.VariantType == Variant.Type.Nil) ? null : (value.AsGodotObject() as StateMachineDefinition));
				bool accepted = false;
				ExecuteMutation(() =>
				{
					accepted = GraphController.SetBaseDefinition(candidate);
				});
				if (!accepted)
				{
					ShowCompositionFailureDiagnostics();
				}
			}
			else
			{
				ExecuteMutation(() =>
				{
					GraphController.SetDefinitionProperty(propertyName, value);
				});
			}
			return;
		}
		StateMachineStateDefinition state = owner as StateMachineStateDefinition;
		if (state != null)
		{
			switch (text)
			{
			case "DisplayName":
				ExecuteMutation(() =>
				{
					GraphController.RenameState(state.StableId, value.AsString());
				});
				break;
			case "Kind":
				ExecuteMutation(() =>
				{
					GraphController.SetStateKind(state.StableId, (StateMachineStateKind)value.AsInt32());
				});
				break;
			case "ParentId":
				ExecuteMutation(() =>
				{
					GraphController.SetStateParent(state.StableId, value.AsString());
				});
				break;
			case "InitialChildId":
				if (string.IsNullOrWhiteSpace(value.AsString()))
				{
					ExecuteMutation(() =>
					{
						GraphController.SetStateProperty(state.StableId, propertyName, value);
					});
				}
				else
				{
					ExecuteMutation(() =>
					{
						GraphController.SetInitialState(state.StableId, value.AsString());
					});
				}
				break;
			case "ProcessFlags":
				ExecuteMutation(() =>
				{
					GraphController.SetStateProcessFlags(state.StableId, (StateMachineProcessFlags)value.AsInt32());
				});
				break;
			case "CallbackKey":
				ExecuteMutation(() =>
				{
					GraphController.SetStateCallbackKey(state.StableId, value.AsString());
				});
				break;
			case "EnterCallbackKey":
				ExecuteMutation(() =>
				{
					GraphController.SetStateLifecycleCallbackKey(state.StableId, StateMachineCallbackPhase.Enter, value.AsString());
				});
				break;
			case "ExitCallbackKey":
				ExecuteMutation(() =>
				{
					GraphController.SetStateLifecycleCallbackKey(state.StableId, StateMachineCallbackPhase.Exit, value.AsString());
				});
				break;
			case "ProcessCallbackKey":
				ExecuteMutation(() =>
				{
					GraphController.SetStateLifecycleCallbackKey(state.StableId, StateMachineCallbackPhase.Process, value.AsString());
				});
				break;
			case "PhysicsProcessCallbackKey":
				ExecuteMutation(() =>
				{
					GraphController.SetStateLifecycleCallbackKey(state.StableId, StateMachineCallbackPhase.PhysicsProcess, value.AsString());
				});
				break;
			default:
				ExecuteMutation(() =>
				{
					GraphController.SetStateProperty(state.StableId, propertyName, value);
				});
				break;
			}
			return;
		}
		StateMachineTransitionDefinition transition = owner as StateMachineTransitionDefinition;
		if (transition == null)
		{
			return;
		}
		if (text == "GuardDefinition")
		{
			ExecuteMutation(() =>
			{
				GraphController.SetTransitionGuard(transition.StableId, value.As<Resource>());
			});
		}
		else
		{
			ExecuteMutation(() =>
			{
				GraphController.SetTransitionProperty(transition.StableId, propertyName, value);
			});
		}
	}

	private void RunDiagnostics()
	{
		StateMachineValidationResult result = GraphController?.Validate();
		_diagnostics.ShowDiagnostics(result);
	}

	private void CompilePreview()
	{
		if (GraphController != null)
		{
			GraphController.CompilePreview(out var _, out var validation);
			_diagnostics.ShowDiagnostics(validation);
		}
	}

	private void ExecuteMutation(Action mutation)
	{
		if (GraphController == null || mutation == null)
		{
			return;
		}
		try
		{
			mutation();
		}
		catch (Exception ex)
		{
			GD.PushWarning("状态机操作未完成：" + ex.Message);
			RunDiagnostics();
		}
	}

	private StateMachineGraphNode FindNodeByGraphName(StringName graphName)
	{
		foreach (StateMachineGraphNode value in _nodes.Values)
		{
			if (value.Name == graphName)
			{
				return value;
			}
		}
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(106)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectSurfaceSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectSurfaceSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetWorkbenchActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindStateNode, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindTransitionChip, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasVisualConnection, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourceStableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "targetStableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsStateVisible, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsStateCollapsed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshLayoutOnly, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadDefinition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "definition", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "layout", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "readOnly", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshGraph, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshGraph, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "preserveContext", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureDetailsContextVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddTransitionChipStateIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "chip", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false)
			}, null),
			new MethodInfo(MethodName.SynchronizeTransitionChipVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateAllTransitionChipPositions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueTransitionChipLayoutAfterGraphSettles, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "reason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateTransitionChipLayoutAfterGraphSettles, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateMovingTransitionChipPositions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnTransitionChipInspectRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveStateDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CompareTransitionChips, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "left", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false),
				new PropertyInfo(Variant.Type.Object, "right", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false)
			}, null),
			new MethodInfo(MethodName.ToLocalPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "graphPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSlotEdgePosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "output", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CubicBezierTangent, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "controlFrom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "controlTo", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "amount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveVisualConnectionsForNode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NavigateToStableId, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPickedResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "pickedResource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedStateNodes, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectedNodes, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildGameToolbar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshStateAuthorTypes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAuthorTypesChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectedStateAuthorTypeId, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCompactStateAuthorTypes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectStateAuthorTypeIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowCompositionFailureDiagnostics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddStateKindMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "popup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false),
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "iconPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindZoomButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyResponsiveLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToggleDetailsPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenDetailsForSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildLegacyToolbar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildOverlayPanels, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureEmptyState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "missingDefinition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "emptyDefinition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "readOnly", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeEmptyDefinition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectOverlaySignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDetailsResourcePickerRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDetailsGuardEditRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "guard", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAliasAddRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "alias", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnAliasRemoveRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "alias", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTransitionCreateRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourceStateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "targetStateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "triggerKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "eventName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "delaySeconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "priority", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "authorTypeId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTransitionRemoveRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnStateOverrideCreateRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnStateOverrideRestoreRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTransitionOverrideCreateRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTransitionOverrideRestoreRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCanvasContextMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSurfaceVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnToolbarActionPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySimulationSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyTransitionChipRuntimeState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "snapshot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyTransitionChipSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveVisibleHierarchyProjection, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolvePosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddStateAtViewportCenter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddStateAtViewportCenter, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddStateAtGraphPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveParentForNewState, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindVisibleState, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CanContainStateKind, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "state", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "childKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.OnDeleteNodesRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "requested", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCopyNodesRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCutNodesRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPasteNodesRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDuplicateNodesRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPopupRequest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "atPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureCanvasContextMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCanvasContextAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToGraphPosition, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "localPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnBeginNodeMove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnEndNodeMove, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnScrollOffsetChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "scrollOffset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnGraphGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.TrySelectConnectionAt, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "localPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DistanceToSegment, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "point", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DistanceToConnectionCurve, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "point", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "from", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "to", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "curvature", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueViewportCapture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CaptureViewportAfterInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueViewportPersistence, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "scrollOffset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "zoom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FlushViewportState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnNodeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnNodeCollapseRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "collapsed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowStateDetails, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "selectNode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowTransitionDetails, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "stableId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "openDetails", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowDefinitionOverview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetAsInitialState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "child", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyFieldChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "owner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.RunDiagnostics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CompilePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindNodeByGraphName, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("GraphNode"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "graphName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectSurfaceSignals && args.Count == 0)
		{
			ConnectSurfaceSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectSurfaceSignals && args.Count == 0)
		{
			DisconnectSurfaceSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.SetWorkbenchActive && args.Count == 1)
		{
			SetWorkbenchActive(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindStateNode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineGraphNode>(FindStateNode(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindTransitionChip && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineTransitionChip>(FindTransitionChip(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasVisualConnection && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasVisualConnection(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.IsStateVisible && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsStateVisible(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsStateCollapsed && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsStateCollapsed(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RefreshLayoutOnly && args.Count == 0)
		{
			RefreshLayoutOnly();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadDefinition && args.Count == 3)
		{
			LoadDefinition(VariantUtils.ConvertTo<StateMachineDefinition>(in args[0]), VariantUtils.ConvertTo<StateMachineLayout>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshGraph && args.Count == 0)
		{
			RefreshGraph();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshGraph && args.Count == 1)
		{
			RefreshGraph(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureDetailsContextVisible && args.Count == 0)
		{
			EnsureDetailsContextVisible();
			ret = default;
			return true;
		}
		if (method == MethodName.AddTransitionChipStateIndex && args.Count == 2)
		{
			AddTransitionChipStateIndex(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<StateMachineTransitionChip>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SynchronizeTransitionChipVisibility && args.Count == 0)
		{
			SynchronizeTransitionChipVisibility();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateAllTransitionChipPositions && args.Count == 1)
		{
			UpdateAllTransitionChipPositions(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.QueueTransitionChipLayoutAfterGraphSettles && args.Count == 1)
		{
			QueueTransitionChipLayoutAfterGraphSettles(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateTransitionChipLayoutAfterGraphSettles && args.Count == 0)
		{
			UpdateTransitionChipLayoutAfterGraphSettles();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMovingTransitionChipPositions && args.Count == 0)
		{
			UpdateMovingTransitionChipPositions();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTransitionChipInspectRequested && args.Count == 1)
		{
			OnTransitionChipInspectRequested(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveStateDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveStateDisplayName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CompareTransitionChips && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CompareTransitionChips(VariantUtils.ConvertTo<StateMachineTransitionChip>(in args[0]), VariantUtils.ConvertTo<StateMachineTransitionChip>(in args[1])));
			return true;
		}
		if (method == MethodName.ToLocalPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ToLocalPosition(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSlotEdgePosition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetSlotEdgePosition(VariantUtils.ConvertTo<StateMachineGraphNode>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.CubicBezierTangent && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<Vector2>(CubicBezierTangent(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<float>(in args[4])));
			return true;
		}
		if (method == MethodName.RemoveVisualConnectionsForNode && args.Count == 1)
		{
			RemoveVisualConnectionsForNode(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateToStableId && args.Count == 1)
		{
			NavigateToStableId(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPickedResource && args.Count == 3)
		{
			ApplyPickedResource(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Resource>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedStateNodes && args.Count == 0)
		{
			Array<StringName> selectedStateNodes = GetSelectedStateNodes();
			ret = VariantUtils.CreateFromArray(selectedStateNodes);
			return true;
		}
		if (method == MethodName.GetSelectedNodes && args.Count == 0)
		{
			Array<StringName> selectedNodes = GetSelectedNodes();
			ret = VariantUtils.CreateFromArray(selectedNodes);
			return true;
		}
		if (method == MethodName.BuildGameToolbar && args.Count == 0)
		{
			BuildGameToolbar();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshStateAuthorTypes && args.Count == 0)
		{
			RefreshStateAuthorTypes();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAuthorTypesChanged && args.Count == 0)
		{
			OnAuthorTypesChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedStateAuthorTypeId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetSelectedStateAuthorTypeId());
			return true;
		}
		if (method == MethodName.RefreshCompactStateAuthorTypes && args.Count == 0)
		{
			RefreshCompactStateAuthorTypes();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectStateAuthorTypeIndex && args.Count == 1)
		{
			SelectStateAuthorTypeIndex(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCompositionFailureDiagnostics && args.Count == 0)
		{
			ShowCompositionFailureDiagnostics();
			ret = default;
			return true;
		}
		if (method == MethodName.AddStateKindMenuItem && args.Count == 4)
		{
			AddStateKindMenuItem(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<StateMachineStateKind>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindZoomButtons && args.Count == 0)
		{
			BindZoomButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyResponsiveLayout && args.Count == 0)
		{
			ApplyResponsiveLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleDetailsPanel && args.Count == 0)
		{
			ToggleDetailsPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenDetailsForSelection && args.Count == 0)
		{
			OpenDetailsForSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildLegacyToolbar && args.Count == 0)
		{
			BuildLegacyToolbar();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildOverlayPanels && args.Count == 0)
		{
			BuildOverlayPanels();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureEmptyState && args.Count == 3)
		{
			ConfigureEmptyState(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeEmptyDefinition && args.Count == 0)
		{
			InitializeEmptyDefinition();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectOverlaySignals && args.Count == 0)
		{
			DisconnectOverlaySignals();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDetailsResourcePickerRequested && args.Count == 2)
		{
			OnDetailsResourcePickerRequested(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDetailsGuardEditRequested && args.Count == 4)
		{
			OnDetailsGuardEditRequested(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAliasAddRequested && args.Count == 2)
		{
			OnAliasAddRequested(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAliasRemoveRequested && args.Count == 1)
		{
			OnAliasRemoveRequested(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTransitionCreateRequested && args.Count == 7)
		{
			OnTransitionCreateRequested(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<StateMachineTriggerKind>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<double>(in args[4]), VariantUtils.ConvertTo<int>(in args[5]), VariantUtils.ConvertTo<string>(in args[6]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTransitionRemoveRequested && args.Count == 1)
		{
			OnTransitionRemoveRequested(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnStateOverrideCreateRequested && args.Count == 1)
		{
			OnStateOverrideCreateRequested(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnStateOverrideRestoreRequested && args.Count == 1)
		{
			OnStateOverrideRestoreRequested(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTransitionOverrideCreateRequested && args.Count == 1)
		{
			OnTransitionOverrideCreateRequested(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTransitionOverrideRestoreRequested && args.Count == 1)
		{
			OnTransitionOverrideRestoreRequested(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildCanvasContextMenu && args.Count == 0)
		{
			BuildCanvasContextMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSurfaceVisibilityChanged && args.Count == 0)
		{
			OnSurfaceVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnToolbarActionPressed && args.Count == 1)
		{
			OnToolbarActionPressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySimulationSnapshot && args.Count == 1)
		{
			ApplySimulationSnapshot(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyTransitionChipRuntimeState && args.Count == 1)
		{
			ApplyTransitionChipRuntimeState(VariantUtils.ConvertTo<StateMachineSnapshot>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyTransitionChipSelection && args.Count == 1)
		{
			ApplyTransitionChipSelection(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveVisibleHierarchyProjection && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveVisibleHierarchyProjection(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolvePosition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ResolvePosition(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.AddStateAtViewportCenter && args.Count == 0)
		{
			AddStateAtViewportCenter();
			ret = default;
			return true;
		}
		if (method == MethodName.AddStateAtViewportCenter && args.Count == 1)
		{
			AddStateAtViewportCenter(VariantUtils.ConvertTo<StateMachineStateKind>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddStateAtGraphPosition && args.Count == 2)
		{
			AddStateAtGraphPosition(VariantUtils.ConvertTo<StateMachineStateKind>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveParentForNewState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveParentForNewState(VariantUtils.ConvertTo<StateMachineStateKind>(in args[0])));
			return true;
		}
		if (method == MethodName.FindVisibleState && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineStateDefinition>(FindVisibleState(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanContainStateKind && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanContainStateKind(VariantUtils.ConvertTo<StateMachineStateDefinition>(in args[0]), VariantUtils.ConvertTo<StateMachineStateKind>(in args[1])));
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
		if (method == MethodName.OnDeleteNodesRequest && args.Count == 1)
		{
			OnDeleteNodesRequest(VariantUtils.ConvertToArray<StringName>(in args[0]));
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
		if (method == MethodName.OnCanvasContextAction && args.Count == 1)
		{
			OnCanvasContextAction(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToGraphPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(ToGraphPosition(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.OnBeginNodeMove && args.Count == 0)
		{
			OnBeginNodeMove();
			ret = default;
			return true;
		}
		if (method == MethodName.OnEndNodeMove && args.Count == 0)
		{
			OnEndNodeMove();
			ret = default;
			return true;
		}
		if (method == MethodName.OnScrollOffsetChanged && args.Count == 1)
		{
			OnScrollOffsetChanged(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnGraphGuiInput && args.Count == 1)
		{
			OnGraphGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TrySelectConnectionAt && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TrySelectConnectionAt(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.DistanceToSegment && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<float>(DistanceToSegment(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.DistanceToConnectionCurve && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<float>(DistanceToConnectionCurve(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<float>(in args[3])));
			return true;
		}
		if (method == MethodName.QueueViewportCapture && args.Count == 0)
		{
			QueueViewportCapture();
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureViewportAfterInput && args.Count == 0)
		{
			CaptureViewportAfterInput();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueViewportPersistence && args.Count == 2)
		{
			QueueViewportPersistence(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlushViewportState && args.Count == 0)
		{
			FlushViewportState();
			ret = default;
			return true;
		}
		if (method == MethodName.OnNodeSelected && args.Count == 1)
		{
			OnNodeSelected(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnNodeCollapseRequested && args.Count == 2)
		{
			OnNodeCollapseRequested(VariantUtils.ConvertTo<StateMachineGraphNode>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowStateDetails && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ShowStateDetails(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ShowTransitionDetails && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ShowTransitionDetails(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ShowDefinitionOverview && args.Count == 0)
		{
			ShowDefinitionOverview();
			ret = default;
			return true;
		}
		if (method == MethodName.SetAsInitialState && args.Count == 1)
		{
			SetAsInitialState(VariantUtils.ConvertTo<StateMachineStateDefinition>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyFieldChange && args.Count == 3)
		{
			ApplyFieldChange(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunDiagnostics && args.Count == 0)
		{
			RunDiagnostics();
			ret = default;
			return true;
		}
		if (method == MethodName.CompilePreview && args.Count == 0)
		{
			CompilePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.FindNodeByGraphName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<StateMachineGraphNode>(FindNodeByGraphName(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.CompareTransitionChips && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CompareTransitionChips(VariantUtils.ConvertTo<StateMachineTransitionChip>(in args[0]), VariantUtils.ConvertTo<StateMachineTransitionChip>(in args[1])));
			return true;
		}
		if (method == MethodName.GetSlotEdgePosition && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetSlotEdgePosition(VariantUtils.ConvertTo<StateMachineGraphNode>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.CubicBezierTangent && args.Count == 5)
		{
			ret = VariantUtils.CreateFrom<Vector2>(CubicBezierTangent(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3]), VariantUtils.ConvertTo<float>(in args[4])));
			return true;
		}
		if (method == MethodName.AddStateKindMenuItem && args.Count == 4)
		{
			AddStateKindMenuItem(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<StateMachineStateKind>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanContainStateKind && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CanContainStateKind(VariantUtils.ConvertTo<StateMachineStateDefinition>(in args[0]), VariantUtils.ConvertTo<StateMachineStateKind>(in args[1])));
			return true;
		}
		if (method == MethodName.DistanceToSegment && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<float>(DistanceToSegment(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2])));
			return true;
		}
		if (method == MethodName.DistanceToConnectionCurve && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<float>(DistanceToConnectionCurve(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<float>(in args[3])));
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
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.ConnectSurfaceSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectSurfaceSignals)
		{
			return true;
		}
		if (method == MethodName.SetWorkbenchActive)
		{
			return true;
		}
		if (method == MethodName.FindStateNode)
		{
			return true;
		}
		if (method == MethodName.FindTransitionChip)
		{
			return true;
		}
		if (method == MethodName.HasVisualConnection)
		{
			return true;
		}
		if (method == MethodName.IsStateVisible)
		{
			return true;
		}
		if (method == MethodName.IsStateCollapsed)
		{
			return true;
		}
		if (method == MethodName.RefreshLayoutOnly)
		{
			return true;
		}
		if (method == MethodName.LoadDefinition)
		{
			return true;
		}
		if (method == MethodName.RefreshGraph)
		{
			return true;
		}
		if (method == MethodName.EnsureDetailsContextVisible)
		{
			return true;
		}
		if (method == MethodName.AddTransitionChipStateIndex)
		{
			return true;
		}
		if (method == MethodName.SynchronizeTransitionChipVisibility)
		{
			return true;
		}
		if (method == MethodName.UpdateAllTransitionChipPositions)
		{
			return true;
		}
		if (method == MethodName.QueueTransitionChipLayoutAfterGraphSettles)
		{
			return true;
		}
		if (method == MethodName.UpdateTransitionChipLayoutAfterGraphSettles)
		{
			return true;
		}
		if (method == MethodName.UpdateMovingTransitionChipPositions)
		{
			return true;
		}
		if (method == MethodName.OnTransitionChipInspectRequested)
		{
			return true;
		}
		if (method == MethodName.ResolveStateDisplayName)
		{
			return true;
		}
		if (method == MethodName.CompareTransitionChips)
		{
			return true;
		}
		if (method == MethodName.ToLocalPosition)
		{
			return true;
		}
		if (method == MethodName.GetSlotEdgePosition)
		{
			return true;
		}
		if (method == MethodName.CubicBezierTangent)
		{
			return true;
		}
		if (method == MethodName.RemoveVisualConnectionsForNode)
		{
			return true;
		}
		if (method == MethodName.NavigateToStableId)
		{
			return true;
		}
		if (method == MethodName.ApplyPickedResource)
		{
			return true;
		}
		if (method == MethodName.GetSelectedStateNodes)
		{
			return true;
		}
		if (method == MethodName.GetSelectedNodes)
		{
			return true;
		}
		if (method == MethodName.BuildGameToolbar)
		{
			return true;
		}
		if (method == MethodName.RefreshStateAuthorTypes)
		{
			return true;
		}
		if (method == MethodName.OnAuthorTypesChanged)
		{
			return true;
		}
		if (method == MethodName.GetSelectedStateAuthorTypeId)
		{
			return true;
		}
		if (method == MethodName.RefreshCompactStateAuthorTypes)
		{
			return true;
		}
		if (method == MethodName.SelectStateAuthorTypeIndex)
		{
			return true;
		}
		if (method == MethodName.ShowCompositionFailureDiagnostics)
		{
			return true;
		}
		if (method == MethodName.AddStateKindMenuItem)
		{
			return true;
		}
		if (method == MethodName.BindZoomButtons)
		{
			return true;
		}
		if (method == MethodName.ApplyResponsiveLayout)
		{
			return true;
		}
		if (method == MethodName.ToggleDetailsPanel)
		{
			return true;
		}
		if (method == MethodName.OpenDetailsForSelection)
		{
			return true;
		}
		if (method == MethodName.BuildLegacyToolbar)
		{
			return true;
		}
		if (method == MethodName.BuildOverlayPanels)
		{
			return true;
		}
		if (method == MethodName.ConfigureEmptyState)
		{
			return true;
		}
		if (method == MethodName.InitializeEmptyDefinition)
		{
			return true;
		}
		if (method == MethodName.DisconnectOverlaySignals)
		{
			return true;
		}
		if (method == MethodName.OnDetailsResourcePickerRequested)
		{
			return true;
		}
		if (method == MethodName.OnDetailsGuardEditRequested)
		{
			return true;
		}
		if (method == MethodName.OnAliasAddRequested)
		{
			return true;
		}
		if (method == MethodName.OnAliasRemoveRequested)
		{
			return true;
		}
		if (method == MethodName.OnTransitionCreateRequested)
		{
			return true;
		}
		if (method == MethodName.OnTransitionRemoveRequested)
		{
			return true;
		}
		if (method == MethodName.OnStateOverrideCreateRequested)
		{
			return true;
		}
		if (method == MethodName.OnStateOverrideRestoreRequested)
		{
			return true;
		}
		if (method == MethodName.OnTransitionOverrideCreateRequested)
		{
			return true;
		}
		if (method == MethodName.OnTransitionOverrideRestoreRequested)
		{
			return true;
		}
		if (method == MethodName.BuildCanvasContextMenu)
		{
			return true;
		}
		if (method == MethodName.OnSurfaceVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.OnToolbarActionPressed)
		{
			return true;
		}
		if (method == MethodName.ApplySimulationSnapshot)
		{
			return true;
		}
		if (method == MethodName.ApplyTransitionChipRuntimeState)
		{
			return true;
		}
		if (method == MethodName.ApplyTransitionChipSelection)
		{
			return true;
		}
		if (method == MethodName.ResolveVisibleHierarchyProjection)
		{
			return true;
		}
		if (method == MethodName.ResolvePosition)
		{
			return true;
		}
		if (method == MethodName.AddStateAtViewportCenter)
		{
			return true;
		}
		if (method == MethodName.AddStateAtGraphPosition)
		{
			return true;
		}
		if (method == MethodName.ResolveParentForNewState)
		{
			return true;
		}
		if (method == MethodName.FindVisibleState)
		{
			return true;
		}
		if (method == MethodName.CanContainStateKind)
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
		if (method == MethodName.OnDeleteNodesRequest)
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
		if (method == MethodName.OnPopupRequest)
		{
			return true;
		}
		if (method == MethodName.ConfigureCanvasContextMenu)
		{
			return true;
		}
		if (method == MethodName.OnCanvasContextAction)
		{
			return true;
		}
		if (method == MethodName.ToGraphPosition)
		{
			return true;
		}
		if (method == MethodName.OnBeginNodeMove)
		{
			return true;
		}
		if (method == MethodName.OnEndNodeMove)
		{
			return true;
		}
		if (method == MethodName.OnScrollOffsetChanged)
		{
			return true;
		}
		if (method == MethodName.OnGraphGuiInput)
		{
			return true;
		}
		if (method == MethodName.TrySelectConnectionAt)
		{
			return true;
		}
		if (method == MethodName.DistanceToSegment)
		{
			return true;
		}
		if (method == MethodName.DistanceToConnectionCurve)
		{
			return true;
		}
		if (method == MethodName.QueueViewportCapture)
		{
			return true;
		}
		if (method == MethodName.CaptureViewportAfterInput)
		{
			return true;
		}
		if (method == MethodName.QueueViewportPersistence)
		{
			return true;
		}
		if (method == MethodName.FlushViewportState)
		{
			return true;
		}
		if (method == MethodName.OnNodeSelected)
		{
			return true;
		}
		if (method == MethodName.OnNodeCollapseRequested)
		{
			return true;
		}
		if (method == MethodName.ShowStateDetails)
		{
			return true;
		}
		if (method == MethodName.ShowTransitionDetails)
		{
			return true;
		}
		if (method == MethodName.ShowDefinitionOverview)
		{
			return true;
		}
		if (method == MethodName.SetAsInitialState)
		{
			return true;
		}
		if (method == MethodName.ApplyFieldChange)
		{
			return true;
		}
		if (method == MethodName.RunDiagnostics)
		{
			return true;
		}
		if (method == MethodName.CompilePreview)
		{
			return true;
		}
		if (method == MethodName.FindNodeByGraphName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.IsWorkbenchActive)
		{
			IsWorkbenchActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.FullGraphRebuildCount)
		{
			FullGraphRebuildCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.IncrementalGraphRefreshCount)
		{
			IncrementalGraphRefreshCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.GraphNodeCreateCount)
		{
			GraphNodeCreateCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.LastConnectionSelectionGroupSize)
		{
			LastConnectionSelectionGroupSize = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.TransitionChipCreateCount)
		{
			TransitionChipCreateCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.TransitionChipSynchronizeCount)
		{
			TransitionChipSynchronizeCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.TransitionChipPositionUpdateCount)
		{
			TransitionChipPositionUpdateCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.LastTransitionChipRelayoutCount)
		{
			LastTransitionChipRelayoutCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.LastTransitionChipRelayoutReason)
		{
			LastTransitionChipRelayoutReason = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._details)
		{
			_details = VariantUtils.ConvertTo<StateMachineDetailsPanel>(in value);
			return true;
		}
		if (name == PropertyName._diagnostics)
		{
			_diagnostics = VariantUtils.ConvertTo<StateMachineDiagnosticsPanel>(in value);
			return true;
		}
		if (name == PropertyName._simulation)
		{
			_simulation = VariantUtils.ConvertTo<StateMachineSimulationPanel>(in value);
			return true;
		}
		if (name == PropertyName._newStateKind)
		{
			_newStateKind = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._newStateType)
		{
			_newStateType = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._compactStateTypeMenu)
		{
			_compactStateTypeMenu = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._addStateButton)
		{
			_addStateButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._narrowAddStateMenu)
		{
			_narrowAddStateMenu = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._detailsToggleButton)
		{
			_detailsToggleButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._simulationToggle)
		{
			_simulationToggle = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._emptyStatePanel)
		{
			_emptyStatePanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._emptyStateHint)
		{
			_emptyStateHint = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._createDefinitionButton)
		{
			_createDefinitionButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._openDefinitionButton)
		{
			_openDefinitionButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._initializeDefinitionButton)
		{
			_initializeDefinitionButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._compositionFailureBanner)
		{
			_compositionFailureBanner = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._compactToolbar)
		{
			_compactToolbar = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._detailsRequestedVisible)
		{
			_detailsRequestedVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._narrowDetailsVisible)
		{
			_narrowDetailsVisible = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._refreshing)
		{
			_refreshing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._renderedDefinition)
		{
			_renderedDefinition = VariantUtils.ConvertTo<StateMachineDefinition>(in value);
			return true;
		}
		if (name == PropertyName._detailsContext)
		{
			_detailsContext = VariantUtils.ConvertTo<DetailsContextKind>(in value);
			return true;
		}
		if (name == PropertyName._detailsStableId)
		{
			_detailsStableId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._viewportDirty)
		{
			_viewportDirty = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._viewportCaptureQueued)
		{
			_viewportCaptureQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingScrollOffset)
		{
			_pendingScrollOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._pendingZoom)
		{
			_pendingZoom = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName._viewportDebounceTimer)
		{
			_viewportDebounceTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._canvasContextMenu)
		{
			_canvasContextMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._lastPointerGraphPosition)
		{
			_lastPointerGraphPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._surfaceSignalsConnected)
		{
			_surfaceSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._trackingNodeMove)
		{
			_trackingNodeMove = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._transitionChipLayoutQueued)
		{
			_transitionChipLayoutQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._queuedTransitionChipLayoutReason)
		{
			_queuedTransitionChipLayoutReason = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._lastCompletedTransitionChipId)
		{
			_lastCompletedTransitionChipId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._selectedTransitionChipId)
		{
			_selectedTransitionChipId = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.Definition)
		{
			value = VariantUtils.CreateFrom<StateMachineDefinition>(Definition);
			return true;
		}
		if (name == PropertyName.Layout)
		{
			value = VariantUtils.CreateFrom<StateMachineLayout>(Layout);
			return true;
		}
		if (name == PropertyName.SimulationPanel)
		{
			value = VariantUtils.CreateFrom<StateMachineSimulationPanel>(SimulationPanel);
			return true;
		}
		if (name == PropertyName.DetailsPanel)
		{
			value = VariantUtils.CreateFrom<StateMachineDetailsPanel>(DetailsPanel);
			return true;
		}
		bool from;
		if (name == PropertyName.IsWorkbenchActive)
		{
			from = IsWorkbenchActive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsHiddenWorkQuiescent)
		{
			from = IsHiddenWorkQuiescent;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		string from2;
		if (name == PropertyName.ActiveDetailsKind)
		{
			from2 = ActiveDetailsKind;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ActiveDetailsStableId)
		{
			from2 = ActiveDetailsStableId;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.LastPointerGraphPosition)
		{
			value = VariantUtils.CreateFrom<Vector2>(LastPointerGraphPosition);
			return true;
		}
		int from3;
		if (name == PropertyName.FullGraphRebuildCount)
		{
			from3 = FullGraphRebuildCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.IncrementalGraphRefreshCount)
		{
			from3 = IncrementalGraphRefreshCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.GraphNodeCreateCount)
		{
			from3 = GraphNodeCreateCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.LastConnectionSelectionGroupSize)
		{
			from3 = LastConnectionSelectionGroupSize;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.VisibleStateNodeCount)
		{
			from3 = VisibleStateNodeCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.VisibleConnectionCount)
		{
			from3 = VisibleConnectionCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.TransitionChipCount)
		{
			from3 = TransitionChipCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.VisibleTransitionChipCount)
		{
			from3 = VisibleTransitionChipCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.TransitionChipCreateCount)
		{
			from3 = TransitionChipCreateCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.TransitionChipSynchronizeCount)
		{
			from3 = TransitionChipSynchronizeCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.TransitionChipPositionUpdateCount)
		{
			from3 = TransitionChipPositionUpdateCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.LastTransitionChipRelayoutCount)
		{
			from3 = LastTransitionChipRelayoutCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.LastTransitionChipRelayoutReason)
		{
			from2 = LastTransitionChipRelayoutReason;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ActiveViewportDebounceTimerCount)
		{
			from3 = ActiveViewportDebounceTimerCount;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName._details)
		{
			value = VariantUtils.CreateFrom(in _details);
			return true;
		}
		if (name == PropertyName._diagnostics)
		{
			value = VariantUtils.CreateFrom(in _diagnostics);
			return true;
		}
		if (name == PropertyName._simulation)
		{
			value = VariantUtils.CreateFrom(in _simulation);
			return true;
		}
		if (name == PropertyName._newStateKind)
		{
			value = VariantUtils.CreateFrom(in _newStateKind);
			return true;
		}
		if (name == PropertyName._newStateType)
		{
			value = VariantUtils.CreateFrom(in _newStateType);
			return true;
		}
		if (name == PropertyName._compactStateTypeMenu)
		{
			value = VariantUtils.CreateFrom(in _compactStateTypeMenu);
			return true;
		}
		if (name == PropertyName._addStateButton)
		{
			value = VariantUtils.CreateFrom(in _addStateButton);
			return true;
		}
		if (name == PropertyName._narrowAddStateMenu)
		{
			value = VariantUtils.CreateFrom(in _narrowAddStateMenu);
			return true;
		}
		if (name == PropertyName._detailsToggleButton)
		{
			value = VariantUtils.CreateFrom(in _detailsToggleButton);
			return true;
		}
		if (name == PropertyName._simulationToggle)
		{
			value = VariantUtils.CreateFrom(in _simulationToggle);
			return true;
		}
		if (name == PropertyName._emptyStatePanel)
		{
			value = VariantUtils.CreateFrom(in _emptyStatePanel);
			return true;
		}
		if (name == PropertyName._emptyStateHint)
		{
			value = VariantUtils.CreateFrom(in _emptyStateHint);
			return true;
		}
		if (name == PropertyName._createDefinitionButton)
		{
			value = VariantUtils.CreateFrom(in _createDefinitionButton);
			return true;
		}
		if (name == PropertyName._openDefinitionButton)
		{
			value = VariantUtils.CreateFrom(in _openDefinitionButton);
			return true;
		}
		if (name == PropertyName._initializeDefinitionButton)
		{
			value = VariantUtils.CreateFrom(in _initializeDefinitionButton);
			return true;
		}
		if (name == PropertyName._compositionFailureBanner)
		{
			value = VariantUtils.CreateFrom(in _compositionFailureBanner);
			return true;
		}
		if (name == PropertyName._compactToolbar)
		{
			value = VariantUtils.CreateFrom(in _compactToolbar);
			return true;
		}
		if (name == PropertyName._detailsRequestedVisible)
		{
			value = VariantUtils.CreateFrom(in _detailsRequestedVisible);
			return true;
		}
		if (name == PropertyName._narrowDetailsVisible)
		{
			value = VariantUtils.CreateFrom(in _narrowDetailsVisible);
			return true;
		}
		if (name == PropertyName._refreshing)
		{
			value = VariantUtils.CreateFrom(in _refreshing);
			return true;
		}
		if (name == PropertyName._renderedDefinition)
		{
			value = VariantUtils.CreateFrom(in _renderedDefinition);
			return true;
		}
		if (name == PropertyName._detailsContext)
		{
			value = VariantUtils.CreateFrom(in _detailsContext);
			return true;
		}
		if (name == PropertyName._detailsStableId)
		{
			value = VariantUtils.CreateFrom(in _detailsStableId);
			return true;
		}
		if (name == PropertyName._viewportDirty)
		{
			value = VariantUtils.CreateFrom(in _viewportDirty);
			return true;
		}
		if (name == PropertyName._viewportCaptureQueued)
		{
			value = VariantUtils.CreateFrom(in _viewportCaptureQueued);
			return true;
		}
		if (name == PropertyName._pendingScrollOffset)
		{
			value = VariantUtils.CreateFrom(in _pendingScrollOffset);
			return true;
		}
		if (name == PropertyName._pendingZoom)
		{
			value = VariantUtils.CreateFrom(in _pendingZoom);
			return true;
		}
		if (name == PropertyName._viewportDebounceTimer)
		{
			value = VariantUtils.CreateFrom(in _viewportDebounceTimer);
			return true;
		}
		if (name == PropertyName._canvasContextMenu)
		{
			value = VariantUtils.CreateFrom(in _canvasContextMenu);
			return true;
		}
		if (name == PropertyName._lastPointerGraphPosition)
		{
			value = VariantUtils.CreateFrom(in _lastPointerGraphPosition);
			return true;
		}
		if (name == PropertyName._surfaceSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _surfaceSignalsConnected);
			return true;
		}
		if (name == PropertyName._trackingNodeMove)
		{
			value = VariantUtils.CreateFrom(in _trackingNodeMove);
			return true;
		}
		if (name == PropertyName._transitionChipLayoutQueued)
		{
			value = VariantUtils.CreateFrom(in _transitionChipLayoutQueued);
			return true;
		}
		if (name == PropertyName._queuedTransitionChipLayoutReason)
		{
			value = VariantUtils.CreateFrom(in _queuedTransitionChipLayoutReason);
			return true;
		}
		if (name == PropertyName._lastCompletedTransitionChipId)
		{
			value = VariantUtils.CreateFrom(in _lastCompletedTransitionChipId);
			return true;
		}
		if (name == PropertyName._selectedTransitionChipId)
		{
			value = VariantUtils.CreateFrom(in _selectedTransitionChipId);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.Definition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.Layout, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.SimulationPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.DetailsPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsWorkbenchActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsHiddenWorkQuiescent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ActiveDetailsKind, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ActiveDetailsStableId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.LastPointerGraphPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.FullGraphRebuildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.IncrementalGraphRefreshCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.GraphNodeCreateCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LastConnectionSelectionGroupSize, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisibleStateNodeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisibleConnectionCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TransitionChipCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.VisibleTransitionChipCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TransitionChipCreateCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TransitionChipSynchronizeCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.TransitionChipPositionUpdateCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LastTransitionChipRelayoutCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.LastTransitionChipRelayoutReason, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ActiveViewportDebounceTimerCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._details, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._diagnostics, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._newStateKind, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._newStateType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._compactStateTypeMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addStateButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._narrowAddStateMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._detailsToggleButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulationToggle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyStatePanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyStateHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._createDefinitionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openDefinitionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._initializeDefinitionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._compositionFailureBanner, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._compactToolbar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._detailsRequestedVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._narrowDetailsVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._refreshing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renderedDefinition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._detailsContext, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._detailsStableId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._viewportDirty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._viewportCaptureQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._pendingScrollOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._pendingZoom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._viewportDebounceTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._canvasContextMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._lastPointerGraphPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._surfaceSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._trackingNodeMove, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._transitionChipLayoutQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._queuedTransitionChipLayoutReason, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._lastCompletedTransitionChipId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._selectedTransitionChipId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.IsWorkbenchActive, Variant.From<bool>(IsWorkbenchActive));
		info.AddProperty(PropertyName.FullGraphRebuildCount, Variant.From<int>(FullGraphRebuildCount));
		info.AddProperty(PropertyName.IncrementalGraphRefreshCount, Variant.From<int>(IncrementalGraphRefreshCount));
		info.AddProperty(PropertyName.GraphNodeCreateCount, Variant.From<int>(GraphNodeCreateCount));
		info.AddProperty(PropertyName.LastConnectionSelectionGroupSize, Variant.From<int>(LastConnectionSelectionGroupSize));
		info.AddProperty(PropertyName.TransitionChipCreateCount, Variant.From<int>(TransitionChipCreateCount));
		info.AddProperty(PropertyName.TransitionChipSynchronizeCount, Variant.From<int>(TransitionChipSynchronizeCount));
		info.AddProperty(PropertyName.TransitionChipPositionUpdateCount, Variant.From<int>(TransitionChipPositionUpdateCount));
		info.AddProperty(PropertyName.LastTransitionChipRelayoutCount, Variant.From<int>(LastTransitionChipRelayoutCount));
		info.AddProperty(PropertyName.LastTransitionChipRelayoutReason, Variant.From<string>(LastTransitionChipRelayoutReason));
		info.AddProperty(PropertyName._details, Variant.From(in _details));
		info.AddProperty(PropertyName._diagnostics, Variant.From(in _diagnostics));
		info.AddProperty(PropertyName._simulation, Variant.From(in _simulation));
		info.AddProperty(PropertyName._newStateKind, Variant.From(in _newStateKind));
		info.AddProperty(PropertyName._newStateType, Variant.From(in _newStateType));
		info.AddProperty(PropertyName._compactStateTypeMenu, Variant.From(in _compactStateTypeMenu));
		info.AddProperty(PropertyName._addStateButton, Variant.From(in _addStateButton));
		info.AddProperty(PropertyName._narrowAddStateMenu, Variant.From(in _narrowAddStateMenu));
		info.AddProperty(PropertyName._detailsToggleButton, Variant.From(in _detailsToggleButton));
		info.AddProperty(PropertyName._simulationToggle, Variant.From(in _simulationToggle));
		info.AddProperty(PropertyName._emptyStatePanel, Variant.From(in _emptyStatePanel));
		info.AddProperty(PropertyName._emptyStateHint, Variant.From(in _emptyStateHint));
		info.AddProperty(PropertyName._createDefinitionButton, Variant.From(in _createDefinitionButton));
		info.AddProperty(PropertyName._openDefinitionButton, Variant.From(in _openDefinitionButton));
		info.AddProperty(PropertyName._initializeDefinitionButton, Variant.From(in _initializeDefinitionButton));
		info.AddProperty(PropertyName._compositionFailureBanner, Variant.From(in _compositionFailureBanner));
		info.AddProperty(PropertyName._compactToolbar, Variant.From(in _compactToolbar));
		info.AddProperty(PropertyName._detailsRequestedVisible, Variant.From(in _detailsRequestedVisible));
		info.AddProperty(PropertyName._narrowDetailsVisible, Variant.From(in _narrowDetailsVisible));
		info.AddProperty(PropertyName._refreshing, Variant.From(in _refreshing));
		info.AddProperty(PropertyName._renderedDefinition, Variant.From(in _renderedDefinition));
		info.AddProperty(PropertyName._detailsContext, Variant.From(in _detailsContext));
		info.AddProperty(PropertyName._detailsStableId, Variant.From(in _detailsStableId));
		info.AddProperty(PropertyName._viewportDirty, Variant.From(in _viewportDirty));
		info.AddProperty(PropertyName._viewportCaptureQueued, Variant.From(in _viewportCaptureQueued));
		info.AddProperty(PropertyName._pendingScrollOffset, Variant.From(in _pendingScrollOffset));
		info.AddProperty(PropertyName._pendingZoom, Variant.From(in _pendingZoom));
		info.AddProperty(PropertyName._viewportDebounceTimer, Variant.From(in _viewportDebounceTimer));
		info.AddProperty(PropertyName._canvasContextMenu, Variant.From(in _canvasContextMenu));
		info.AddProperty(PropertyName._lastPointerGraphPosition, Variant.From(in _lastPointerGraphPosition));
		info.AddProperty(PropertyName._surfaceSignalsConnected, Variant.From(in _surfaceSignalsConnected));
		info.AddProperty(PropertyName._trackingNodeMove, Variant.From(in _trackingNodeMove));
		info.AddProperty(PropertyName._transitionChipLayoutQueued, Variant.From(in _transitionChipLayoutQueued));
		info.AddProperty(PropertyName._queuedTransitionChipLayoutReason, Variant.From(in _queuedTransitionChipLayoutReason));
		info.AddProperty(PropertyName._lastCompletedTransitionChipId, Variant.From(in _lastCompletedTransitionChipId));
		info.AddProperty(PropertyName._selectedTransitionChipId, Variant.From(in _selectedTransitionChipId));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.IsWorkbenchActive, out var value))
		{
			IsWorkbenchActive = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.FullGraphRebuildCount, out var value2))
		{
			FullGraphRebuildCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.IncrementalGraphRefreshCount, out var value3))
		{
			IncrementalGraphRefreshCount = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.GraphNodeCreateCount, out var value4))
		{
			GraphNodeCreateCount = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.LastConnectionSelectionGroupSize, out var value5))
		{
			LastConnectionSelectionGroupSize = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName.TransitionChipCreateCount, out var value6))
		{
			TransitionChipCreateCount = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.TransitionChipSynchronizeCount, out var value7))
		{
			TransitionChipSynchronizeCount = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.TransitionChipPositionUpdateCount, out var value8))
		{
			TransitionChipPositionUpdateCount = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.LastTransitionChipRelayoutCount, out var value9))
		{
			LastTransitionChipRelayoutCount = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.LastTransitionChipRelayoutReason, out var value10))
		{
			LastTransitionChipRelayoutReason = value10.As<string>();
		}
		if (info.TryGetProperty(PropertyName._details, out var value11))
		{
			_details = value11.As<StateMachineDetailsPanel>();
		}
		if (info.TryGetProperty(PropertyName._diagnostics, out var value12))
		{
			_diagnostics = value12.As<StateMachineDiagnosticsPanel>();
		}
		if (info.TryGetProperty(PropertyName._simulation, out var value13))
		{
			_simulation = value13.As<StateMachineSimulationPanel>();
		}
		if (info.TryGetProperty(PropertyName._newStateKind, out var value14))
		{
			_newStateKind = value14.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._newStateType, out var value15))
		{
			_newStateType = value15.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._compactStateTypeMenu, out var value16))
		{
			_compactStateTypeMenu = value16.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._addStateButton, out var value17))
		{
			_addStateButton = value17.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._narrowAddStateMenu, out var value18))
		{
			_narrowAddStateMenu = value18.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._detailsToggleButton, out var value19))
		{
			_detailsToggleButton = value19.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._simulationToggle, out var value20))
		{
			_simulationToggle = value20.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._emptyStatePanel, out var value21))
		{
			_emptyStatePanel = value21.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._emptyStateHint, out var value22))
		{
			_emptyStateHint = value22.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._createDefinitionButton, out var value23))
		{
			_createDefinitionButton = value23.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._openDefinitionButton, out var value24))
		{
			_openDefinitionButton = value24.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._initializeDefinitionButton, out var value25))
		{
			_initializeDefinitionButton = value25.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._compositionFailureBanner, out var value26))
		{
			_compositionFailureBanner = value26.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._compactToolbar, out var value27))
		{
			_compactToolbar = value27.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._detailsRequestedVisible, out var value28))
		{
			_detailsRequestedVisible = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._narrowDetailsVisible, out var value29))
		{
			_narrowDetailsVisible = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._refreshing, out var value30))
		{
			_refreshing = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._renderedDefinition, out var value31))
		{
			_renderedDefinition = value31.As<StateMachineDefinition>();
		}
		if (info.TryGetProperty(PropertyName._detailsContext, out var value32))
		{
			_detailsContext = value32.As<DetailsContextKind>();
		}
		if (info.TryGetProperty(PropertyName._detailsStableId, out var value33))
		{
			_detailsStableId = value33.As<string>();
		}
		if (info.TryGetProperty(PropertyName._viewportDirty, out var value34))
		{
			_viewportDirty = value34.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._viewportCaptureQueued, out var value35))
		{
			_viewportCaptureQueued = value35.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingScrollOffset, out var value36))
		{
			_pendingScrollOffset = value36.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._pendingZoom, out var value37))
		{
			_pendingZoom = value37.As<float>();
		}
		if (info.TryGetProperty(PropertyName._viewportDebounceTimer, out var value38))
		{
			_viewportDebounceTimer = value38.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._canvasContextMenu, out var value39))
		{
			_canvasContextMenu = value39.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._lastPointerGraphPosition, out var value40))
		{
			_lastPointerGraphPosition = value40.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._surfaceSignalsConnected, out var value41))
		{
			_surfaceSignalsConnected = value41.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._trackingNodeMove, out var value42))
		{
			_trackingNodeMove = value42.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._transitionChipLayoutQueued, out var value43))
		{
			_transitionChipLayoutQueued = value43.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._queuedTransitionChipLayoutReason, out var value44))
		{
			_queuedTransitionChipLayoutReason = value44.As<string>();
		}
		if (info.TryGetProperty(PropertyName._lastCompletedTransitionChipId, out var value45))
		{
			_lastCompletedTransitionChipId = value45.As<string>();
		}
		if (info.TryGetProperty(PropertyName._selectedTransitionChipId, out var value46))
		{
			_selectedTransitionChipId = value46.As<string>();
		}
	}
}
