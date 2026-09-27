using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.GUI;
using PVZHE.ModEditor.Registry.Class;
using PVZHE.ModEditor.ResourceEditors.GUI;

namespace PVZHE.ModEditor.SceneEditor;

[ScriptPath("res://addons/ModEditor/SceneEditor/2D/GUI/XW2DSceneEditor.cs")]
public class XW2DSceneEditor : PanelContainer
{
	private enum SnapMenuId
	{
		SmartSnap = 1,
		GridSnap,
		ShowGrid,
		ShowGridWhenSnapping,
		HideGrid,
		OpenVisualSettings
	}

	private enum ViewMenuId
	{
		CenterSelection = 1,
		FrameSelection = 2,
		ZoomIn = 3,
		ZoomOut = 4,
		ZoomReset = 5,
		ShowOrigin = 10,
		ShowViewport = 11,
		ShowRulers = 12,
		ShowGuides = 13,
		ShowHelpers = 14
	}

	private enum SkeletonMenuId
	{
		MakeBones = 1,
		ClearBones
	}

	private enum ViewportContextMenuId
	{
		AddNode = 1,
		InstantiateScene = 2,
		SaveScene = 3,
		Rename = 10,
		Duplicate = 11,
		MoveUp = 12,
		MoveDown = 13,
		Delete = 14,
		LockSelection = 15,
		UnlockSelection = 16,
		GroupSelection = 17,
		UngroupSelection = 18,
		CopyNodePath = 19,
		MakeRoot = 20,
		SaveBranchAsScene = 21,
		CreateScript = 30,
		DetachScript = 31,
		ExtendScript = 32,
		CenterSelection = 40,
		FrameSelection = 41,
		ZoomIn = 42,
		ZoomOut = 43,
		ZoomReset = 44,
		ShowOrigin = 50,
		ShowViewport = 51,
		ShowRulers = 52,
		ShowGuides = 53,
		ShowHelpers = 54
	}

	private class SceneTabInfo
	{
		public string Key = "";

		public string DisplayName = "";

		public string SourcePath = "";

		public PackedScene PackedScene;

		public Node SceneInstance;

		public bool ScriptlessPreview;

		public int HistoryId = 1;

		public Dictionary ViewportState = new Dictionary();

		public Dictionary AnimationTimelineState = new Dictionary();
	}

	private readonly struct DirectTransformState(Vector2 position, Vector2 scale, float rotation, Vector2 size, Vector2 pivot, bool isControl)
	{
		public readonly Vector2 Position = position;

		public readonly Vector2 Scale = scale;

		public readonly float Rotation = rotation;

		public readonly Vector2 Size = size;

		public readonly Vector2 Pivot = pivot;

		public readonly bool IsControl = isControl;
	}

	private readonly struct ControlLayoutState(Control control)
	{
		public readonly float AnchorLeft = control.AnchorLeft;

		public readonly float AnchorTop = control.AnchorTop;

		public readonly float AnchorRight = control.AnchorRight;

		public readonly float AnchorBottom = control.AnchorBottom;

		public readonly float OffsetLeft = control.OffsetLeft;

		public readonly float OffsetTop = control.OffsetTop;

		public readonly float OffsetRight = control.OffsetRight;

		public readonly float OffsetBottom = control.OffsetBottom;

		public readonly GrowDirection GrowHorizontal = control.GrowHorizontal;

		public readonly GrowDirection GrowVertical = control.GrowVertical;
	}

	public enum SelectionLayoutOperation
	{
		AlignLeft,
		AlignHorizontalCenter,
		AlignRight,
		AlignTop,
		AlignVerticalCenter,
		AlignBottom,
		DistributeHorizontal,
		DistributeVertical
	}

	private readonly struct SelectionLayoutEntry(Node node, DirectTransformState state, Rect2 worldBounds)
	{
		public readonly Node Node = node;

		public readonly DirectTransformState State = state;

		public readonly Rect2 WorldBounds = worldBounds;
	}

	private readonly struct SpriteAtlasState
	{
		public readonly int Columns;

		public readonly int Rows;

		public readonly int Frame;

		public readonly bool RegionEnabled;

		public readonly Rect2 RegionRect;

		public SpriteAtlasState(Sprite2D sprite)
		{
			Columns = Math.Max(1, sprite.Hframes);
			Rows = Math.Max(1, sprite.Vframes);
			Frame = Math.Clamp(sprite.Frame, 0, Columns * Rows - 1);
			RegionEnabled = sprite.RegionEnabled;
			RegionRect = sprite.RegionRect;
		}

		public SpriteAtlasState(int columns, int rows, int frame, bool regionEnabled, Rect2 regionRect)
		{
			Columns = Math.Clamp(columns, 1, 256);
			Rows = Math.Clamp(rows, 1, 256);
			Frame = Math.Clamp(frame, 0, Columns * Rows - 1);
			RegionEnabled = regionEnabled;
			RegionRect = regionRect;
		}
	}

	public new class MethodName : PanelContainer.MethodName
	{
		public static readonly StringName InitializeSceneAnimationSurface = "InitializeSceneAnimationSurface";

		public static readonly StringName ShutdownSceneAnimationSurface = "ShutdownSceneAnimationSurface";

		public static readonly StringName SetSceneAnimationTimelineVisible = "SetSceneAnimationTimelineVisible";

		public static readonly StringName OnSceneAnimationSelectionChanged = "OnSceneAnimationSelectionChanged";

		public static readonly StringName RebindSceneAnimationRoot = "RebindSceneAnimationRoot";

		public static readonly StringName SuspendSceneAnimationPreviewForSave = "SuspendSceneAnimationPreviewForSave";

		public static readonly StringName ResumeSceneAnimationPreviewAfterSave = "ResumeSceneAnimationPreviewAfterSave";

		public static readonly StringName RefreshSceneAnimationAfterHistory = "RefreshSceneAnimationAfterHistory";

		public static readonly StringName RefreshSceneAnimationAfterHistoryIfVisible = "RefreshSceneAnimationAfterHistoryIfVisible";

		public static readonly StringName NotifySceneAnimationHistoryApplied = "NotifySceneAnimationHistoryApplied";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName LoadPackedScene = "LoadPackedScene";

		public static readonly StringName LoadPackedSceneFromPath = "LoadPackedSceneFromPath";

		public static readonly StringName SaveCurrentScene = "SaveCurrentScene";

		public static readonly StringName SaveAllScenes = "SaveAllScenes";

		public static readonly StringName TryInstantiateFullScene = "TryInstantiateFullScene";

		public static readonly StringName AreAllExternalCSharpScriptsRegistered = "AreAllExternalCSharpScriptsRegistered";

		public static readonly StringName LoadScriptlessPreviewPackedScene = "LoadScriptlessPreviewPackedScene";

		public static readonly StringName NormalizeScriptResourcePath = "NormalizeScriptResourcePath";

		public static readonly StringName NormalizeComparablePath = "NormalizeComparablePath";

		public static readonly StringName IsModCharacterSceneText = "IsModCharacterSceneText";

		public static readonly StringName IsCharacterBaseSceneResourceLine = "IsCharacterBaseSceneResourceLine";

		public static readonly StringName IsRuntimeCharacterRootAssignment = "IsRuntimeCharacterRootAssignment";

		public static readonly StringName IsRuntimeCharacterComponentPreviewLine = "IsRuntimeCharacterComponentPreviewLine";

		public static readonly StringName IsExternalCSharpScriptPath = "IsExternalCSharpScriptPath";

		public static readonly StringName ReadSceneResourceAttribute = "ReadSceneResourceAttribute";

		public static readonly StringName RefreshSceneTree = "RefreshSceneTree";

		public static readonly StringName BuildMenus = "BuildMenus";

		public static readonly StringName BuildViewportContextMenu = "BuildViewportContextMenu";

		public static readonly StringName ConnectSignals = "ConnectSignals";

		public static readonly StringName ConnectDirectTransformSurface = "ConnectDirectTransformSurface";

		public static readonly StringName GetDirectTransformSpins = "GetDirectTransformSpins";

		public static readonly StringName UpdateDirectTransformSurface = "UpdateDirectTransformSurface";

		public static readonly StringName ShowDirectNodeProperties = "ShowDirectNodeProperties";

		public static readonly StringName RefreshDirectNodePropertySurface = "RefreshDirectNodePropertySurface";

		public static readonly StringName ResolveDirectPropertyTarget = "ResolveDirectPropertyTarget";

		public static readonly StringName BindDirectNodePropertyTarget = "BindDirectNodePropertyTarget";

		public static readonly StringName OnDirectNodePropertyEdited = "OnDirectNodePropertyEdited";

		public static readonly StringName OnWorkspaceTabChanged = "OnWorkspaceTabChanged";

		public static readonly StringName BeginDirectTransformEditSession = "BeginDirectTransformEditSession";

		public static readonly StringName OnDirectTransformValueChanged = "OnDirectTransformValueChanged";

		public static readonly StringName CommitDirectTransformEditSession = "CommitDirectTransformEditSession";

		public static readonly StringName NotifyDirectTransformHistoryApplied = "NotifyDirectTransformHistoryApplied";

		public static readonly StringName RefreshDirectTransformSurface = "RefreshDirectTransformSurface";

		public static readonly StringName DetachCurrentSceneRootForHistory = "DetachCurrentSceneRootForHistory";

		public static readonly StringName ApplySceneRootFromHistory = "ApplySceneRootFromHistory";

		public static readonly StringName SaveCurrentTabState = "SaveCurrentTabState";

		public static readonly StringName LoadTabData = "LoadTabData";

		public static readonly StringName SwitchToTab = "SwitchToTab";

		public static readonly StringName OnTabChanged = "OnTabChanged";

		public static readonly StringName OnTabClosePressed = "OnTabClosePressed";

		public static readonly StringName GetCurrentSceneHistoryId = "GetCurrentSceneHistoryId";

		public static readonly StringName GetSceneHistoryIdAt = "GetSceneHistoryIdAt";

		public static readonly StringName IsSceneTabDirty = "IsSceneTabDirty";

		public static readonly StringName RequestCloseSceneTab = "RequestCloseSceneTab";

		public static readonly StringName OnUnsavedSceneCloseConfirmed = "OnUnsavedSceneCloseConfirmed";

		public static readonly StringName OnUnsavedSceneCloseCanceled = "OnUnsavedSceneCloseCanceled";

		public static readonly StringName OnDiscardUnsavedScenePressed = "OnDiscardUnsavedScenePressed";

		public static readonly StringName CloseSceneTabImmediately = "CloseSceneTabImmediately";

		public static readonly StringName OnSceneHistoryChanged = "OnSceneHistoryChanged";

		public static readonly StringName ClearEditor = "ClearEditor";

		public static readonly StringName OpenViewportContextMenu = "OpenViewportContextMenu";

		public static readonly StringName QueueViewportRedraw = "QueueViewportRedraw";

		public static readonly StringName PrepareForSceneHistoryBranchChange = "PrepareForSceneHistoryBranchChange";

		public static readonly StringName SetToolMode = "SetToolMode";

		public static readonly StringName OnSnapMenuItem = "OnSnapMenuItem";

		public static readonly StringName OnViewMenuItem = "OnViewMenuItem";

		public static readonly StringName OnViewportContextMenuItem = "OnViewportContextMenuItem";

		public static readonly StringName OnSkeletonMenuItem = "OnSkeletonMenuItem";

		public static readonly StringName GenerateSkeletonFromSelectionWithHistory = "GenerateSkeletonFromSelectionWithHistory";

		public static readonly StringName ClearGeneratedSkeletonWithHistory = "ClearGeneratedSkeletonWithHistory";

		public static readonly StringName GetBoneRelativeTransform = "GetBoneRelativeTransform";

		public static readonly StringName UpdateGeneratedBoneGizmo = "UpdateGeneratedBoneGizmo";

		public static readonly StringName FindSelectedGeneratedSkeleton = "FindSelectedGeneratedSkeleton";

		public static readonly StringName FindGeneratedSkeleton = "FindGeneratedSkeleton";

		public static readonly StringName IsGeneratedSkeleton = "IsGeneratedSkeleton";

		public static readonly StringName GetNodeDepth = "GetNodeDepth";

		public static readonly StringName BuildUniqueChildName = "BuildUniqueChildName";

		public static readonly StringName SetSelectionMetaWithHistory = "SetSelectionMetaWithHistory";

		public static readonly StringName UpdateSelectionActionButtons = "UpdateSelectionActionButtons";

		public static readonly StringName UpdateSnapButtons = "UpdateSnapButtons";

		public static readonly StringName SyncToolbarFromViewportState = "SyncToolbarFromViewportState";

		public static readonly StringName UpdateSnapMenuChecks = "UpdateSnapMenuChecks";

		public static readonly StringName UpdateViewMenuChecks = "UpdateViewMenuChecks";

		public static readonly StringName UpdateViewportContextMenuState = "UpdateViewportContextMenuState";

		public static readonly StringName UpdateZoomLabel = "UpdateZoomLabel";

		public static readonly StringName UpdateMousePositionLabel = "UpdateMousePositionLabel";

		public static readonly StringName GetSceneKey = "GetSceneKey";

		public static readonly StringName GetSceneDisplayName = "GetSceneDisplayName";

		public static readonly StringName SetToolButtonPressed = "SetToolButtonPressed";

		public static readonly StringName SetMenuChecked = "SetMenuChecked";

		public static readonly StringName SetMenuDisabled = "SetMenuDisabled";

		public static readonly StringName LoadIcon = "LoadIcon";

		public static readonly StringName FreeSceneInstance = "FreeSceneInstance";

		public static readonly StringName InitializeSceneGridSettings = "InitializeSceneGridSettings";

		public static readonly StringName ShutdownSceneGridSettings = "ShutdownSceneGridSettings";

		public static readonly StringName OpenGridSettingsPanel = "OpenGridSettingsPanel";

		public static readonly StringName SyncGridSettingsPanelFromViewport = "SyncGridSettingsPanelFromViewport";

		public static readonly StringName BuildGridPanelState = "BuildGridPanelState";

		public static readonly StringName OnGridSettingsChanged = "OnGridSettingsChanged";

		public static readonly StringName LoadPersistedGridSettings = "LoadPersistedGridSettings";

		public static readonly StringName PersistGridSettings = "PersistGridSettings";

		public static readonly StringName SetGridSettingIfChanged = "SetGridSettingIfChanged";

		public static readonly StringName FlushGridSettingsSave = "FlushGridSettingsSave";

		public static readonly StringName RegisterGridSettingDefaults = "RegisterGridSettingDefaults";

		public static readonly StringName InitializeInlineTextEditing = "InitializeInlineTextEditing";

		public static readonly StringName ShutdownInlineTextEditing = "ShutdownInlineTextEditing";

		public new static readonly StringName _Input = "_Input";

		public static readonly StringName CanInlineEditText = "CanInlineEditText";

		public static readonly StringName ClearInlineTextCapabilityCache = "ClearInlineTextCapabilityCache";

		public static readonly StringName BeginInlineTextEdit = "BeginInlineTextEdit";

		public static readonly StringName SetInlineTextDraft = "SetInlineTextDraft";

		public static readonly StringName CommitPendingAuthoringInput = "CommitPendingAuthoringInput";

		public static readonly StringName CommitInlineTextEdit = "CommitInlineTextEdit";

		public static readonly StringName CancelInlineTextEdit = "CancelInlineTextEdit";

		public static readonly StringName RefreshInlineTextEditorGeometry = "RefreshInlineTextEditorGeometry";

		public static readonly StringName NotifyInlineTextHistoryApplied = "NotifyInlineTextHistoryApplied";

		public static readonly StringName CommitInlineTextEditForScene = "CommitInlineTextEditForScene";

		public static readonly StringName OnInlineTextLineChanged = "OnInlineTextLineChanged";

		public static readonly StringName OnInlineTextMultilineChanged = "OnInlineTextMultilineChanged";

		public static readonly StringName ApplyInlineTextPreview = "ApplyInlineTextPreview";

		public static readonly StringName ApplyInlineTextValue = "ApplyInlineTextValue";

		public static readonly StringName MakeInlineTextVariant = "MakeInlineTextVariant";

		public static readonly StringName QueueInlineTextFocusExitCommit = "QueueInlineTextFocusExitCommit";

		public static readonly StringName OnInlineTextEditorGuiInput = "OnInlineTextEditorGuiInput";

		public static readonly StringName OnInlineTextHostVisibilityChanged = "OnInlineTextHostVisibilityChanged";

		public static readonly StringName OnInlineTextViewportVisibilityChanged = "OnInlineTextViewportVisibilityChanged";

		public static readonly StringName QueueInlineTextGeometryRefresh = "QueueInlineTextGeometryRefresh";

		public static readonly StringName ConfigureInlineTextInputForTarget = "ConfigureInlineTextInputForTarget";

		public static readonly StringName ApplyPendingInlineTextIme = "ApplyPendingInlineTextIme";

		public static readonly StringName CancelPendingInlineTextIme = "CancelPendingInlineTextIme";

		public static readonly StringName GrabInlineTextEditorFocus = "GrabInlineTextEditorFocus";

		public static readonly StringName QueueInlineTextEditorFocus = "QueueInlineTextEditorFocus";

		public static readonly StringName HideInlineTextEditor = "HideInlineTextEditor";

		public static readonly StringName BuildInlineTextModeLabel = "BuildInlineTextModeLabel";

		public static readonly StringName GetInlineTextControlRectInViewport = "GetInlineTextControlRectInViewport";

		public static readonly StringName CreateInlineTextHeaderButton = "CreateInlineTextHeaderButton";

		public static readonly StringName LoadInlineTextIcon = "LoadInlineTextIcon";

		public static readonly StringName CreateInlineTextOverlayStyle = "CreateInlineTextOverlayStyle";

		public static readonly StringName InitializeControlLayoutSurface = "InitializeControlLayoutSurface";

		public static readonly StringName AddGrowItems = "AddGrowItems";

		public static readonly StringName GrowDirectionFromVisualIndex = "GrowDirectionFromVisualIndex";

		public static readonly StringName GetGrowDirectionVisualIndex = "GetGrowDirectionVisualIndex";

		public static readonly StringName BindGrowDirectionButtons = "BindGrowDirectionButtons";

		public static readonly StringName GetControlLayoutSpins = "GetControlLayoutSpins";

		public static readonly StringName ClearDirectSpinMixedMarker = "ClearDirectSpinMixedMarker";

		public static readonly StringName ApplyDirectSpinValue = "ApplyDirectSpinValue";

		public static readonly StringName IsDirectTransformFieldMixed = "IsDirectTransformFieldMixed";

		public static readonly StringName ApplySelectedTransformFieldWithHistory = "ApplySelectedTransformFieldWithHistory";

		public static readonly StringName FindDirectTransformSpin = "FindDirectTransformSpin";

		public static readonly StringName UpdateControlLayoutSurface = "UpdateControlLayoutSurface";

		public static readonly StringName SelectGrowDirectionButton = "SelectGrowDirectionButton";

		public static readonly StringName BeginControlLayoutSession = "BeginControlLayoutSession";

		public static readonly StringName OnControlLayoutSpinChanged = "OnControlLayoutSpinChanged";

		public static readonly StringName CommitControlLayoutSession = "CommitControlLayoutSession";

		public static readonly StringName BeginViewportControlLayoutSession = "BeginViewportControlLayoutSession";

		public static readonly StringName CommitViewportControlLayoutSession = "CommitViewportControlLayoutSession";

		public static readonly StringName CancelViewportControlLayoutSession = "CancelViewportControlLayoutSession";

		public static readonly StringName EditSelectedControlLayoutFieldWithHistory = "EditSelectedControlLayoutFieldWithHistory";

		public static readonly StringName ApplySelectedControlLayoutPresetWithHistory = "ApplySelectedControlLayoutPresetWithHistory";

		public static readonly StringName ApplyControlLayoutPresetWithHistory = "ApplyControlLayoutPresetWithHistory";

		public static readonly StringName SetControlGrowWithHistory = "SetControlGrowWithHistory";

		public static readonly StringName AddHistoryProperty = "AddHistoryProperty";

		public static readonly StringName NotifyControlLayoutHistoryApplied = "NotifyControlLayoutHistoryApplied";

		public static readonly StringName SetSelectionLockWithHistory = "SetSelectionLockWithHistory";

		public static readonly StringName SetSelectionGroupWithHistory = "SetSelectionGroupWithHistory";

		public static readonly StringName CommitSelectionMetaHistory = "CommitSelectionMetaHistory";

		public static readonly StringName ApplyNodeEditMeta = "ApplyNodeEditMeta";

		public static readonly StringName NotifySelectionMetaHistoryApplied = "NotifySelectionMetaHistoryApplied";

		public static readonly StringName PrepareSceneSurfaceHistoryMutation = "PrepareSceneSurfaceHistoryMutation";

		public static readonly StringName MarkCurrentSceneHistorySurfaceDirty = "MarkCurrentSceneHistorySurfaceDirty";

		public static readonly StringName UpdateCurrentSceneDirtyMarker = "UpdateCurrentSceneDirtyMarker";

		public static readonly StringName UpdateSceneDirtyMarker = "UpdateSceneDirtyMarker";

		public static readonly StringName InitializeSelectionLayoutSurface = "InitializeSelectionLayoutSurface";

		public static readonly StringName UpdateSelectionLayoutSurface = "UpdateSelectionLayoutSurface";

		public static readonly StringName CollectSelectionLayoutNodes = "CollectSelectionLayoutNodes";

		public static readonly StringName ApplySelectionLayoutWithHistory = "ApplySelectionLayoutWithHistory";

		public static readonly StringName GetAxisStart = "GetAxisStart";

		public static readonly StringName GetAxisSize = "GetAxisSize";

		public static readonly StringName GetAxisEnd = "GetAxisEnd";

		public static readonly StringName GetSelectionLayoutHistoryName = "GetSelectionLayoutHistoryName";

		public static readonly StringName InitializeSpriteAtlasSurface = "InitializeSpriteAtlasSurface";

		public static readonly StringName ShutdownSpriteAtlasSurface = "ShutdownSpriteAtlasSurface";

		public static readonly StringName OnSpriteAtlasSelectionChanged = "OnSpriteAtlasSelectionChanged";

		public static readonly StringName ResolveSingleSelectedSprite = "ResolveSingleSelectedSprite";

		public static readonly StringName OpenSelectedSpriteAtlas = "OpenSelectedSpriteAtlas";

		public static readonly StringName OpenSpriteAtlasForSelection = "OpenSpriteAtlasForSelection";

		public static readonly StringName CommitSpriteAtlasStateWithHistory = "CommitSpriteAtlasStateWithHistory";

		public static readonly StringName ApplySpriteAtlasState = "ApplySpriteAtlasState";

		public static readonly StringName NotifySpriteAtlasHistoryApplied = "NotifySpriteAtlasHistoryApplied";

		public static readonly StringName IsNodeInCurrentScene = "IsNodeInCurrentScene";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName CurrentPackedScene = "CurrentPackedScene";

		public static readonly StringName CurrentSceneInstance = "CurrentSceneInstance";

		public static readonly StringName InlineTextEditorVisible = "InlineTextEditorVisible";

		public static readonly StringName InlineTextTarget = "InlineTextTarget";

		public static readonly StringName InlineTextProperty = "InlineTextProperty";

		public static readonly StringName InlineTextValueType = "InlineTextValueType";

		public static readonly StringName InlineTextHint = "InlineTextHint";

		public static readonly StringName InlineTextDraft = "InlineTextDraft";

		public static readonly StringName InlineTextCommitCount = "InlineTextCommitCount";

		public static readonly StringName InlineTextCancelCount = "InlineTextCancelCount";

		public static readonly StringName InlineTextPreviewCount = "InlineTextPreviewCount";

		public static readonly StringName InlineTextOverlayRect = "InlineTextOverlayRect";

		public static readonly StringName InlineTextInputRect = "InlineTextInputRect";

		public static readonly StringName SpriteAtlasPanel = "SpriteAtlasPanel";

		public static readonly StringName _animationTimelineButton = "_animationTimelineButton";

		public static readonly StringName _canvasTimelineSplit = "_canvasTimelineSplit";

		public static readonly StringName _sceneAnimationTimelinePanel = "_sceneAnimationTimelinePanel";

		public static readonly StringName _sceneAnimationSelectionConnected = "_sceneAnimationSelectionConnected";

		public static readonly StringName _sceneTabBar = "_sceneTabBar";

		public static readonly StringName _viewport = "_viewport";

		public static readonly StringName _mousePositionLabel = "_mousePositionLabel";

		public static readonly StringName _zoomLabel = "_zoomLabel";

		public static readonly StringName _selectButton = "_selectButton";

		public static readonly StringName _moveButton = "_moveButton";

		public static readonly StringName _rotateButton = "_rotateButton";

		public static readonly StringName _scaleButton = "_scaleButton";

		public static readonly StringName _listSelectButton = "_listSelectButton";

		public static readonly StringName _pivotButton = "_pivotButton";

		public static readonly StringName _panButton = "_panButton";

		public static readonly StringName _rulerButton = "_rulerButton";

		public static readonly StringName _pathButton = "_pathButton";

		public static readonly StringName _polygonButton = "_polygonButton";

		public static readonly StringName _localSpaceButton = "_localSpaceButton";

		public static readonly StringName _smartSnapButton = "_smartSnapButton";

		public static readonly StringName _gridSnapButton = "_gridSnapButton";

		public static readonly StringName _lockButton = "_lockButton";

		public static readonly StringName _unlockButton = "_unlockButton";

		public static readonly StringName _groupButton = "_groupButton";

		public static readonly StringName _ungroupButton = "_ungroupButton";

		public static readonly StringName _centerViewButton = "_centerViewButton";

		public static readonly StringName _zoomOutButton = "_zoomOutButton";

		public static readonly StringName _zoomResetButton = "_zoomResetButton";

		public static readonly StringName _zoomInButton = "_zoomInButton";

		public static readonly StringName _snapConfigMenu = "_snapConfigMenu";

		public static readonly StringName _skeletonMenu = "_skeletonMenu";

		public static readonly StringName _viewMenu = "_viewMenu";

		public static readonly StringName _viewportContextMenu = "_viewportContextMenu";

		public static readonly StringName _workspaceTabs = "_workspaceTabs";

		public static readonly StringName _allPropertiesButton = "_allPropertiesButton";

		public static readonly StringName _nodeDirectPropertySurface = "_nodeDirectPropertySurface";

		public static readonly StringName _transformDirectPanel = "_transformDirectPanel";

		public static readonly StringName _transformSelectionLabel = "_transformSelectionLabel";

		public static readonly StringName _controlFields = "_controlFields";

		public static readonly StringName _positionXSpin = "_positionXSpin";

		public static readonly StringName _positionYSpin = "_positionYSpin";

		public static readonly StringName _rotationSpin = "_rotationSpin";

		public static readonly StringName _scaleXSpin = "_scaleXSpin";

		public static readonly StringName _scaleYSpin = "_scaleYSpin";

		public static readonly StringName _sizeXSpin = "_sizeXSpin";

		public static readonly StringName _sizeYSpin = "_sizeYSpin";

		public static readonly StringName _pivotXSpin = "_pivotXSpin";

		public static readonly StringName _pivotYSpin = "_pivotYSpin";

		public static readonly StringName _currentTabKey = "_currentTabKey";

		public static readonly StringName _isSwitchingTab = "_isSwitchingTab";

		public static readonly StringName _contextMenuWorldPosition = "_contextMenuWorldPosition";

		public static readonly StringName _directTransformNode = "_directTransformNode";

		public static readonly StringName _directTransformSessionActive = "_directTransformSessionActive";

		public static readonly StringName _updatingDirectTransformSurface = "_updatingDirectTransformSurface";

		public static readonly StringName _sceneRootHistoryViewportState = "_sceneRootHistoryViewportState";

		public static readonly StringName _sceneHistoryManager = "_sceneHistoryManager";

		public static readonly StringName _unsavedSceneCloseDialog = "_unsavedSceneCloseDialog";

		public static readonly StringName _discardUnsavedSceneButton = "_discardUnsavedSceneButton";

		public static readonly StringName _pendingCloseTabKey = "_pendingCloseTabKey";

		public static readonly StringName _gridSettingsPopup = "_gridSettingsPopup";

		public static readonly StringName _gridSettingsPanel = "_gridSettingsPanel";

		public static readonly StringName _gridSettingsSaveTimer = "_gridSettingsSaveTimer";

		public static readonly StringName _inlineTextOverlay = "_inlineTextOverlay";

		public static readonly StringName _inlineTextModeLabel = "_inlineTextModeLabel";

		public static readonly StringName _inlineTextLineEdit = "_inlineTextLineEdit";

		public static readonly StringName _inlineTextMultilineEdit = "_inlineTextMultilineEdit";

		public static readonly StringName _inlineTextCommitButton = "_inlineTextCommitButton";

		public static readonly StringName _inlineTextCancelButton = "_inlineTextCancelButton";

		public static readonly StringName _inlineTextOverlayMotion = "_inlineTextOverlayMotion";

		public static readonly StringName _inlineTextTarget = "_inlineTextTarget";

		public static readonly StringName _inlineTextProperty = "_inlineTextProperty";

		public static readonly StringName _inlineTextValueType = "_inlineTextValueType";

		public static readonly StringName _inlineTextHint = "_inlineTextHint";

		public static readonly StringName _inlineTextBefore = "_inlineTextBefore";

		public static readonly StringName _inlineTextHistoryId = "_inlineTextHistoryId";

		public static readonly StringName _inlineTextSessionToken = "_inlineTextSessionToken";

		public static readonly StringName _inlineTextMultiline = "_inlineTextMultiline";

		public static readonly StringName _inlineTextApplyingPreview = "_inlineTextApplyingPreview";

		public static readonly StringName _inlineTextGeometryRefreshQueued = "_inlineTextGeometryRefreshQueued";

		public static readonly StringName _controlLayoutPanel = "_controlLayoutPanel";

		public static readonly StringName _anchorLeftSpin = "_anchorLeftSpin";

		public static readonly StringName _anchorTopSpin = "_anchorTopSpin";

		public static readonly StringName _anchorRightSpin = "_anchorRightSpin";

		public static readonly StringName _anchorBottomSpin = "_anchorBottomSpin";

		public static readonly StringName _offsetLeftSpin = "_offsetLeftSpin";

		public static readonly StringName _offsetTopSpin = "_offsetTopSpin";

		public static readonly StringName _offsetRightSpin = "_offsetRightSpin";

		public static readonly StringName _offsetBottomSpin = "_offsetBottomSpin";

		public static readonly StringName _growHorizontalOption = "_growHorizontalOption";

		public static readonly StringName _growVerticalOption = "_growVerticalOption";

		public static readonly StringName _growHorizontalButtons = "_growHorizontalButtons";

		public static readonly StringName _growVerticalButtons = "_growVerticalButtons";

		public static readonly StringName _presetTopLeftButton = "_presetTopLeftButton";

		public static readonly StringName _presetCenterButton = "_presetCenterButton";

		public static readonly StringName _presetFullRectButton = "_presetFullRectButton";

		public static readonly StringName _presetHCenterWideButton = "_presetHCenterWideButton";

		public static readonly StringName _presetVCenterWideButton = "_presetVCenterWideButton";

		public static readonly StringName _layoutControl = "_layoutControl";

		public static readonly StringName _layoutSessionActive = "_layoutSessionActive";

		public static readonly StringName _updatingLayoutSurface = "_updatingLayoutSurface";

		public static readonly StringName _selectionLayoutPanel = "_selectionLayoutPanel";

		public static readonly StringName _selectionLayoutLabel = "_selectionLayoutLabel";

		public static readonly StringName _selectionAlignButtons = "_selectionAlignButtons";

		public static readonly StringName _selectionDistributeButtons = "_selectionDistributeButtons";

		public static readonly StringName _spriteAtlasPanel = "_spriteAtlasPanel";

		public static readonly StringName _spriteAtlasButton = "_spriteAtlasButton";

		public static readonly StringName _spriteAtlasTabIndex = "_spriteAtlasTabIndex";

		public static readonly StringName _spriteAtlasSelectionConnected = "_spriteAtlasSelectionConnected";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private Button _animationTimelineButton;

	private VSplitContainer _canvasTimelineSplit;

	private XW2DAnimationTimelinePanel _sceneAnimationTimelinePanel;

	private bool _sceneAnimationSelectionConnected;

	private const string EditLockMeta = "_edit_lock_";

	private const string EditGroupMeta = "_edit_group_";

	private const string GeneratedSkeletonMeta = "_xw_generated_skeleton_2d";

	private const string BoneSourcePathMeta = "_xw_bone_source_path";

	private const string RuntimeCharacterComponentManagerName = "ComponentManager";

	private TabBar _sceneTabBar;

	private XW2DViewport _viewport;

	private Label _mousePositionLabel;

	private Label _zoomLabel;

	private Button _selectButton;

	private Button _moveButton;

	private Button _rotateButton;

	private Button _scaleButton;

	private Button _listSelectButton;

	private Button _pivotButton;

	private Button _panButton;

	private Button _rulerButton;

	private Button _pathButton;

	private Button _polygonButton;

	private Button _localSpaceButton;

	private Button _smartSnapButton;

	private Button _gridSnapButton;

	private Button _lockButton;

	private Button _unlockButton;

	private Button _groupButton;

	private Button _ungroupButton;

	private Button _centerViewButton;

	private Button _zoomOutButton;

	private Button _zoomResetButton;

	private Button _zoomInButton;

	private MenuButton _snapConfigMenu;

	private MenuButton _skeletonMenu;

	private MenuButton _viewMenu;

	private PopupMenu _viewportContextMenu;

	private TabContainer _workspaceTabs;

	private Button _allPropertiesButton;

	private XWDirectPropertySurface _nodeDirectPropertySurface;

	private PanelContainer _transformDirectPanel;

	private Label _transformSelectionLabel;

	private HBoxContainer _controlFields;

	private SpinBox _positionXSpin;

	private SpinBox _positionYSpin;

	private SpinBox _rotationSpin;

	private SpinBox _scaleXSpin;

	private SpinBox _scaleYSpin;

	private SpinBox _sizeXSpin;

	private SpinBox _sizeYSpin;

	private SpinBox _pivotXSpin;

	private SpinBox _pivotYSpin;

	private readonly System.Collections.Generic.Dictionary<string, SceneTabInfo> _openTabs = new System.Collections.Generic.Dictionary<string, SceneTabInfo>();

	private readonly List<string> _tabKeys = new List<string>();

	private string _currentTabKey = "";

	private bool _isSwitchingTab;

	private Vector2 _contextMenuWorldPosition;

	private Node _directTransformNode;

	private bool _directTransformSessionActive;

	private bool _updatingDirectTransformSurface;

	private Dictionary _sceneRootHistoryViewportState = new Dictionary();

	private XWUndoRedoManager _sceneHistoryManager;

	private ConfirmationDialog _unsavedSceneCloseDialog;

	private Button _discardUnsavedSceneButton;

	private string _pendingCloseTabKey = "";

	private const string GridSettingPrefix = "2d_editor_grid/";

	private PopupPanel _gridSettingsPopup;

	private XW2DGridSettingsPanel _gridSettingsPanel;

	private Timer _gridSettingsSaveTimer;

	private static readonly StringName InlineTextPropertyFallback = new StringName("text");

	private PanelContainer _inlineTextOverlay;

	private Label _inlineTextModeLabel;

	private LineEdit _inlineTextLineEdit;

	private TextEdit _inlineTextMultilineEdit;

	private Button _inlineTextCommitButton;

	private Button _inlineTextCancelButton;

	private XWUiMotion _inlineTextOverlayMotion;

	private Node _inlineTextTarget;

	private StringName _inlineTextProperty = InlineTextPropertyFallback;

	private Variant.Type _inlineTextValueType = Variant.Type.String;

	private PropertyHint _inlineTextHint;

	private string _inlineTextBefore = "";

	private int _inlineTextHistoryId = 1;

	private int _inlineTextSessionToken;

	private bool _inlineTextMultiline;

	private bool _inlineTextApplyingPreview;

	private bool _inlineTextGeometryRefreshQueued;

	private readonly System.Collections.Generic.Dictionary<ulong, bool> _inlineTextCapabilityCache = new System.Collections.Generic.Dictionary<ulong, bool>();

	private readonly List<Node> _directTransformNodes = new List<Node>();

	private readonly System.Collections.Generic.Dictionary<Node, DirectTransformState> _directTransformStarts = new System.Collections.Generic.Dictionary<Node, DirectTransformState>();

	private readonly HashSet<SpinBox> _directTransformTouchedSpins = new HashSet<SpinBox>();

	private readonly System.Collections.Generic.Dictionary<SpinBox, string> _directSpinBaseSuffixes = new System.Collections.Generic.Dictionary<SpinBox, string>();

	private readonly HashSet<SpinBox> _directMixedSpins = new HashSet<SpinBox>();

	private PanelContainer _controlLayoutPanel;

	private SpinBox _anchorLeftSpin;

	private SpinBox _anchorTopSpin;

	private SpinBox _anchorRightSpin;

	private SpinBox _anchorBottomSpin;

	private SpinBox _offsetLeftSpin;

	private SpinBox _offsetTopSpin;

	private SpinBox _offsetRightSpin;

	private SpinBox _offsetBottomSpin;

	private OptionButton _growHorizontalOption;

	private OptionButton _growVerticalOption;

	private Button[] _growHorizontalButtons;

	private Button[] _growVerticalButtons;

	private Button _presetTopLeftButton;

	private Button _presetCenterButton;

	private Button _presetFullRectButton;

	private Button _presetHCenterWideButton;

	private Button _presetVCenterWideButton;

	private Control _layoutControl;

	private ControlLayoutState _layoutStart;

	private bool _layoutSessionActive;

	private bool _updatingLayoutSurface;

	private readonly List<Node> _selectionLayoutNodes = new List<Node>();

	private PanelContainer _selectionLayoutPanel;

	private Label _selectionLayoutLabel;

	private Button[] _selectionAlignButtons;

	private Button[] _selectionDistributeButtons;

	private XW2DSpriteAtlasPanel _spriteAtlasPanel;

	private Button _spriteAtlasButton;

	private int _spriteAtlasTabIndex = -1;

	private bool _spriteAtlasSelectionConnected;

	public PackedScene CurrentPackedScene { get; private set; }

	public Node CurrentSceneInstance { get; private set; }

	public bool InlineTextEditorVisible
	{
		get
		{
			if (GodotObject.IsInstanceValid(_inlineTextOverlay))
			{
				return _inlineTextOverlay.Visible;
			}
			return false;
		}
	}

	public Node InlineTextTarget
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_inlineTextTarget))
			{
				return null;
			}
			return _inlineTextTarget;
		}
	}

	public StringName InlineTextProperty => _inlineTextProperty;

	public Variant.Type InlineTextValueType => _inlineTextValueType;

	public PropertyHint InlineTextHint => _inlineTextHint;

	public string InlineTextDraft
	{
		get
		{
			object obj;
			if (_inlineTextMultiline)
			{
				obj = _inlineTextMultilineEdit?.Text ?? "";
			}
			else
			{
				obj = _inlineTextLineEdit?.Text;
				if (obj == null)
				{
					return "";
				}
			}
			return (string)obj;
		}
	}

	public int InlineTextCommitCount { get; private set; }

	public int InlineTextCancelCount { get; private set; }

	public int InlineTextPreviewCount { get; private set; }

	public Rect2 InlineTextOverlayRect
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_inlineTextOverlay))
			{
				return default;
			}
			return new Rect2(_inlineTextOverlay.Position, _inlineTextOverlay.Size);
		}
	}

	public Rect2 InlineTextInputRect
	{
		get
		{
			Control control = (_inlineTextMultiline ? ((Control)_inlineTextMultilineEdit) : ((Control)_inlineTextLineEdit));
			return GetInlineTextControlRectInViewport(control);
		}
	}

	public XW2DSpriteAtlasPanel SpriteAtlasPanel => _spriteAtlasPanel;

	private void InitializeSceneAnimationSurface()
	{
		_animationTimelineButton = GetNodeOrNull<Button>("%AnimationTimelineButton") ?? (FindChild("AnimationTimelineButton", recursive: true, owned: false) as Button);
		_canvasTimelineSplit = GetNodeOrNull<VSplitContainer>("%CanvasTimelineSplit") ?? (FindChild("CanvasTimelineSplit", recursive: true, owned: false) as VSplitContainer);
		_sceneAnimationTimelinePanel = GetNodeOrNull<XW2DAnimationTimelinePanel>("%SceneAnimationTimelinePanel") ?? (FindChild("SceneAnimationTimelinePanel", recursive: true, owned: false) as XW2DAnimationTimelinePanel);
		if (!GodotObject.IsInstanceValid(_animationTimelineButton) || !GodotObject.IsInstanceValid(_canvasTimelineSplit) || !GodotObject.IsInstanceValid(_sceneAnimationTimelinePanel))
		{
			GD.PushError("2D 动画时间轴界面缺少按钮、分割面板或动画工作台节点。");
			return;
		}
		_sceneAnimationTimelinePanel.BindEditor(this);
		_animationTimelineButton.Toggled += SetSceneAnimationTimelineVisible;
		XWEditorSelection xWEditorSelection = XWEditorInterface.Instance?.GetEditorSelection();
		if (xWEditorSelection != null)
		{
			xWEditorSelection.SelectionChanged += OnSceneAnimationSelectionChanged;
			_sceneAnimationSelectionConnected = true;
		}
		SetSceneAnimationTimelineVisible(visible: false);
	}

	private void ShutdownSceneAnimationSurface()
	{
		if (_sceneAnimationSelectionConnected)
		{
			XWEditorSelection xWEditorSelection = XWEditorInterface.Instance?.GetEditorSelection();
			if (xWEditorSelection != null)
			{
				xWEditorSelection.SelectionChanged -= OnSceneAnimationSelectionChanged;
			}
			_sceneAnimationSelectionConnected = false;
		}
		_sceneAnimationTimelinePanel?.ClearSurface();
	}

	private void SetSceneAnimationTimelineVisible(bool visible)
	{
		if (!GodotObject.IsInstanceValid(_sceneAnimationTimelinePanel))
		{
			return;
		}
		_animationTimelineButton?.SetPressedNoSignal(visible);
		_sceneAnimationTimelinePanel.Visible = visible;
		if (GodotObject.IsInstanceValid(_canvasTimelineSplit))
		{
			_canvasTimelineSplit.Collapsed = !visible;
			if (visible)
			{
				int num = Mathf.Clamp(Mathf.RoundToInt(Size.Y * 0.42f), 260, 520);
				_canvasTimelineSplit.SplitOffsets = new int[1] { -num };
			}
		}
		if (visible)
		{
			_sceneAnimationTimelinePanel.RefreshFromSceneHistory();
			_sceneAnimationTimelinePanel.RefreshSelection();
		}
	}

	private void OnSceneAnimationSelectionChanged()
	{
		if (!GodotObject.IsInstanceValid(_sceneAnimationTimelinePanel))
		{
			return;
		}
		_sceneAnimationTimelinePanel.RefreshSelection();
		foreach (Node item in XWEditorInterface.Instance?.GetEditorSelection()?.GetSelectedNodes() ?? new List<Node>())
		{
			if (item is AnimationPlayer || item is AnimationTree)
			{
				SetSceneAnimationTimelineVisible(visible: true);
				break;
			}
		}
	}

	private void SaveSceneAnimationTabState(SceneTabInfo tabInfo)
	{
		if (tabInfo != null && GodotObject.IsInstanceValid(_sceneAnimationTimelinePanel))
		{
			tabInfo.AnimationTimelineState = _sceneAnimationTimelinePanel.CaptureState();
			tabInfo.AnimationTimelineState["expanded"] = GodotObject.IsInstanceValid(_sceneAnimationTimelinePanel) && _sceneAnimationTimelinePanel.Visible;
		}
	}

	private void LoadSceneAnimationTab(SceneTabInfo tabInfo)
	{
		if (GodotObject.IsInstanceValid(_sceneAnimationTimelinePanel))
		{
			_sceneAnimationTimelinePanel.BindScene(tabInfo?.SceneInstance, tabInfo?.AnimationTimelineState);
			bool sceneAnimationTimelineVisible = tabInfo != null && tabInfo.AnimationTimelineState != null && tabInfo.AnimationTimelineState.GetValueOrDefault("expanded", false).AsBool();
			SetSceneAnimationTimelineVisible(sceneAnimationTimelineVisible);
		}
	}

	private void RebindSceneAnimationRoot(Node sceneRoot)
	{
		if (GodotObject.IsInstanceValid(_sceneAnimationTimelinePanel))
		{
			_sceneAnimationTimelinePanel.BindScene(sceneRoot, null);
		}
	}

	private bool SuspendSceneAnimationPreviewForSave()
	{
		if (!GodotObject.IsInstanceValid(_sceneAnimationTimelinePanel))
		{
			return false;
		}
		_sceneAnimationTimelinePanel.SuspendForSave();
		return true;
	}

	private void ResumeSceneAnimationPreviewAfterSave(bool resume)
	{
		if (resume && GodotObject.IsInstanceValid(_sceneAnimationTimelinePanel))
		{
			_sceneAnimationTimelinePanel.ResumeAfterSave();
		}
	}

	private void RefreshSceneAnimationAfterHistory()
	{
		_sceneAnimationTimelinePanel?.RefreshFromSceneHistory();
	}

	private void RefreshSceneAnimationAfterHistoryIfVisible()
	{
		if (GodotObject.IsInstanceValid(_sceneAnimationTimelinePanel) && _sceneAnimationTimelinePanel.Visible && _sceneAnimationTimelinePanel.IsVisibleInTree())
		{
			_sceneAnimationTimelinePanel.RefreshFromSceneHistory();
		}
	}

	public void NotifySceneAnimationHistoryApplied()
	{
		CallDeferred("RefreshSceneAnimationAfterHistory");
		CallDeferred("UpdateCurrentSceneDirtyMarker");
	}

	public override void _Ready()
	{
		_sceneTabBar = GetNode<TabBar>("%SceneTabBar");
		_viewport = GetNode<XW2DViewport>("%Viewport2D");
		_mousePositionLabel = GetNode<Label>("%MousePositionLabel");
		_zoomLabel = GetNode<Label>("%ZoomLabel");
		_selectButton = GetNode<Button>("%SelectButton");
		_moveButton = GetNode<Button>("%MoveButton");
		_rotateButton = GetNode<Button>("%RotateButton");
		_scaleButton = GetNode<Button>("%ScaleButton");
		_listSelectButton = GetNode<Button>("%ListSelectButton");
		_pivotButton = GetNode<Button>("%PivotButton");
		_panButton = GetNode<Button>("%PanButton");
		_rulerButton = GetNode<Button>("%RulerButton");
		_pathButton = GetNode<Button>("%PathButton");
		_polygonButton = GetNode<Button>("%PolygonButton");
		_localSpaceButton = GetNode<Button>("%LocalSpaceButton");
		_smartSnapButton = GetNode<Button>("%SmartSnapButton");
		_gridSnapButton = GetNode<Button>("%GridSnapButton");
		_lockButton = GetNode<Button>("%LockButton");
		_unlockButton = GetNode<Button>("%UnlockButton");
		_groupButton = GetNode<Button>("%GroupButton");
		_ungroupButton = GetNode<Button>("%UngroupButton");
		_centerViewButton = GetNode<Button>("%CenterViewButton");
		_zoomOutButton = GetNode<Button>("%ZoomOutButton");
		_zoomResetButton = GetNode<Button>("%ZoomResetButton");
		_zoomInButton = GetNode<Button>("%ZoomInButton");
		_snapConfigMenu = GetNode<MenuButton>("%SnapConfigMenu");
		_skeletonMenu = GetNode<MenuButton>("%SkeletonMenu");
		_viewMenu = GetNode<MenuButton>("%ViewMenu");
		_viewportContextMenu = GetNode<PopupMenu>("%ViewportContextMenu");
		_workspaceTabs = GetNode<TabContainer>("%WorkspaceTabs");
		_allPropertiesButton = GetNode<Button>("%AllPropertiesButton");
		_nodeDirectPropertySurface = GetNode<XWDirectPropertySurface>("%NodeDirectPropertySurface");
		_unsavedSceneCloseDialog = GetNode<ConfirmationDialog>("%UnsavedSceneCloseDialog");
		_discardUnsavedSceneButton = _unsavedSceneCloseDialog.AddButton("不保存", right: true);
		_discardUnsavedSceneButton.Name = "DiscardUnsavedSceneButton";
		_discardUnsavedSceneButton.UniqueNameInOwner = true;
		_transformDirectPanel = GetNode<PanelContainer>("%TransformDirectPanel");
		_transformSelectionLabel = GetNode<Label>("%TransformSelectionLabel");
		_controlFields = GetNode<HBoxContainer>("%ControlFields");
		_positionXSpin = GetNode<SpinBox>("%PositionXSpin");
		_positionYSpin = GetNode<SpinBox>("%PositionYSpin");
		_rotationSpin = GetNode<SpinBox>("%RotationSpin");
		_scaleXSpin = GetNode<SpinBox>("%ScaleXSpin");
		_scaleYSpin = GetNode<SpinBox>("%ScaleYSpin");
		_sizeXSpin = GetNode<SpinBox>("%SizeXSpin");
		_sizeYSpin = GetNode<SpinBox>("%SizeYSpin");
		_pivotXSpin = GetNode<SpinBox>("%PivotXSpin");
		_pivotYSpin = GetNode<SpinBox>("%PivotYSpin");
		_workspaceTabs.SetTabTitle(0, "画布");
		_workspaceTabs.SetTabTitle(1, "节点属性");
		_workspaceTabs.SetTabIcon(1, XWClassRegistry.Instance.GetUIIcon("Object"));
		_nodeDirectPropertySurface.PropertyEdited += OnDirectNodePropertyEdited;
		InitializeControlLayoutSurface();
		InitializeSelectionLayoutSurface();
		InitializeSceneAnimationSurface();
		InitializeSceneGridSettings();
		InitializeSpriteAtlasSurface();
		_viewport.SetEditor(this);
		InitializeInlineTextEditing();
		_sceneHistoryManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (GodotObject.IsInstanceValid(_sceneHistoryManager))
		{
			_sceneHistoryManager.HistoryChanged += OnSceneHistoryChanged;
		}
		BuildMenus();
		ConnectSignals();
		SetToolMode(XW2DViewport.ToolMode.Select);
		UpdateSnapButtons();
		UpdateZoomLabel();
		UpdateMousePositionLabel(Vector2.Zero);
		UpdateSelectionActionButtons();
		UpdateDirectTransformSurface();
	}

	public override void _ExitTree()
	{
		ShutdownInlineTextEditing();
		ShutdownSpriteAtlasSurface();
		ShutdownSceneGridSettings();
		ShutdownSceneAnimationSurface();
		if (GodotObject.IsInstanceValid(_sceneHistoryManager))
		{
			_sceneHistoryManager.HistoryChanged -= OnSceneHistoryChanged;
		}
		_viewport?.ClearSceneInstance();
		XWSceneTreeDock xWSceneTreeDock = XWEditorInterface.Instance?.GetSceneTreeDock();
		foreach (SceneTabInfo value in _openTabs.Values)
		{
			_viewport?.ReleaseHistoryResources(value.HistoryId);
			xWSceneTreeDock?.ReleaseHistoryResources(value.HistoryId);
			if (GodotObject.IsInstanceValid(_sceneHistoryManager))
			{
				_sceneHistoryManager.ReleaseHistory(value.HistoryId);
			}
			FreeSceneInstance(value.SceneInstance);
		}
		_openTabs.Clear();
		_tabKeys.Clear();
		base._ExitTree();
	}

	public void LoadPackedScene(PackedScene packedScene, string sourcePath = "")
	{
		if (packedScene == null || !GodotObject.IsInstanceValid(packedScene))
		{
			XWEditorInterface.Instance?.ShowToast("无法加载场景资源");
			return;
		}
		string sceneKey = GetSceneKey(packedScene);
		if (_openTabs.ContainsKey(sceneKey))
		{
			TryRecoverScriptlessPreviewTab(_openTabs[sceneKey], showToast: true);
			SwitchToTab(sceneKey);
			return;
		}
		SaveCurrentTabState();
		Node node = InstantiateSceneForEditing(packedScene, sourcePath, out var scriptlessPreview);
		if (node != null)
		{
			Node sceneInstance = node;
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			int num = ((!GodotObject.IsInstanceValid(xWUndoRedoManager)) ? 1 : xWUndoRedoManager.CreateHistoryScope());
			SceneTabInfo sceneTabInfo = new SceneTabInfo
			{
				Key = sceneKey,
				DisplayName = GetSceneDisplayName(packedScene),
				SourcePath = (string.IsNullOrWhiteSpace(sourcePath) ? packedScene.ResourcePath : sourcePath),
				PackedScene = packedScene,
				SceneInstance = sceneInstance,
				ScriptlessPreview = scriptlessPreview,
				HistoryId = num
			};
			SeedSceneGridTabState(sceneTabInfo);
			_openTabs[sceneKey] = sceneTabInfo;
			_tabKeys.Add(sceneKey);
			_isSwitchingTab = true;
			_sceneTabBar.AddTab(sceneTabInfo.DisplayName, LoadIcon("res://addons/ModEditor/Icons/PackedScene.svg"));
			_sceneTabBar.CurrentTab = _tabKeys.Count - 1;
			_isSwitchingTab = false;
			if (GodotObject.IsInstanceValid(xWUndoRedoManager))
			{
				xWUndoRedoManager.SetHistoryAsSaved(num);
			}
			LoadTabData(sceneKey);
			XWEditorInterface.Instance?.FocusPanel("2d_editor");
			XWEditorInterface.Instance?.ShowToast("已打开场景: " + sceneTabInfo.DisplayName);
		}
		else
		{
			XWEditorInterface.Instance?.ShowToast("该场景没有有效根节点");
		}
	}

	public void LoadPackedSceneFromPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path) || !ResourceLoader.Exists(path))
		{
			XWEditorInterface.Instance?.ShowToast("场景不存在: " + path);
			return;
		}
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, "", ResourceLoader.CacheMode.Ignore);
		if (packedScene == null)
		{
			XWEditorInterface.Instance?.ShowToast("无法加载场景: " + path);
		}
		else
		{
			LoadPackedScene(packedScene, path);
		}
	}

	public bool SaveCurrentScene()
	{
		if (string.IsNullOrEmpty(_currentTabKey) || !_openTabs.TryGetValue(_currentTabKey, out var value))
		{
			XWEditorInterface.Instance?.ShowToast("没有打开的 2D 场景");
			return false;
		}
		return SaveSceneTab(value, showToast: true);
	}

	public int SaveAllScenes(bool showToast = true)
	{
		int num = 0;
		int num2 = 0;
		foreach (string tabKey in _tabKeys)
		{
			if (_openTabs.TryGetValue(tabKey, out var value) && GodotObject.IsInstanceValid(value.SceneInstance) && (!GodotObject.IsInstanceValid(_sceneHistoryManager) || _sceneHistoryManager.IsHistoryUnsaved(value.HistoryId)))
			{
				if (SaveSceneTab(value, showToast: false))
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		if (showToast)
		{
			XWEditorInterface.Instance?.ShowToast((num2 == 0) ? $"已保存全部 2D 场景（{num}）" : $"2D 场景保存完成：成功 {num}，失败 {num2}");
		}
		if (num2 != 0)
		{
			return -num2;
		}
		return num;
	}

	private bool SaveSceneTab(SceneTabInfo tabInfo, bool showToast)
	{
		if (tabInfo == null || !GodotObject.IsInstanceValid(tabInfo.SceneInstance))
		{
			return false;
		}
		CommitInlineTextEditForScene(tabInfo.SceneInstance);
		if (tabInfo.ScriptlessPreview && !TryRecoverScriptlessPreviewTab(tabInfo, showToast: false))
		{
			if (showToast)
			{
				XWEditorInterface.Instance?.ShowToast("当前场景仍是无脚本预览；请先成功编译 Mod C# 脚本再保存。");
			}
			return false;
		}
		string text = ((!string.IsNullOrWhiteSpace(tabInfo.SourcePath)) ? tabInfo.SourcePath : (tabInfo.PackedScene?.ResourcePath ?? ""));
		if (string.IsNullOrWhiteSpace(text))
		{
			if (showToast)
			{
				XWEditorInterface.Instance?.ShowToast("当前场景没有保存路径");
			}
			return false;
		}
		bool resume = SuspendSceneAnimationPreviewForSave();
		PackedScene packedScene = new PackedScene();
		Error error;
		try
		{
			error = packedScene.Pack(tabInfo.SceneInstance);
			if (error == Error.Ok)
			{
				packedScene.TakeOverPath(text);
				error = ResourceSaver.Save(packedScene, text, ResourceSaver.SaverFlags.None);
			}
		}
		finally
		{
			ResumeSceneAnimationPreviewAfterSave(resume);
		}
		if (error == Error.Ok)
		{
			tabInfo.PackedScene = packedScene;
			if (tabInfo.Key == _currentTabKey)
			{
				CurrentPackedScene = packedScene;
			}
			XWEditorInterface.Instance?.GetUndoRedoManager()?.SetHistoryAsSaved(tabInfo.HistoryId);
			UpdateSceneDirtyMarker(tabInfo.Key);
		}
		if (showToast)
		{
			XWEditorInterface.Instance?.ShowToast((error == Error.Ok) ? ("已保存场景: " + text.GetFile()) : $"保存场景失败: {error}");
		}
		return error == Error.Ok;
	}

	private Node InstantiateSceneForEditing(PackedScene packedScene, string sourcePath, out bool scriptlessPreview)
	{
		scriptlessPreview = false;
		Node node = TryInstantiateFullScene(packedScene, sourcePath);
		if (GodotObject.IsInstanceValid(node))
		{
			return node;
		}
		PackedScene packedScene2 = LoadScriptlessPreviewPackedScene(sourcePath);
		if (GodotObject.IsInstanceValid(packedScene2))
		{
			Node node2 = packedScene2.Instantiate(PackedScene.GenEditState.Disabled);
			if (node2 != null)
			{
				Node result = node2;
				scriptlessPreview = true;
				return result;
			}
		}
		return null;
	}

	private static Node TryInstantiateFullScene(PackedScene packedScene, string sourcePath)
	{
		if (!AreAllExternalCSharpScriptsRegistered(sourcePath))
		{
			return null;
		}
		Node node;
		try
		{
			node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
		}
		catch (Exception ex)
		{
			GD.PushWarning("完整场景实例化失败，改用无脚本预览: " + ex.GetBaseException().Message);
			return null;
		}
		if (!GodotObject.IsInstanceValid(node))
		{
			return null;
		}
		return node;
	}

	private static bool AreAllExternalCSharpScriptsRegistered(string sourcePath)
	{
		if (string.IsNullOrWhiteSpace(sourcePath) || !Godot.FileAccess.FileExists(sourcePath))
		{
			return true;
		}
		string fileAsString = Godot.FileAccess.GetFileAsString(sourcePath);
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		string[] array = fileAsString.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
		for (int i = 0; i < array.Length; i++)
		{
			if (TryReadExternalScriptResource(array[i], out var _, out var scriptPath))
			{
				if (!TryResolveExternalScriptPath(sourcePath, scriptPath, out var resolvedPath))
				{
					return false;
				}
				hashSet.Add(NormalizeComparablePath(resolvedPath));
			}
		}
		if (hashSet.Count == 0)
		{
			return true;
		}
		HashSet<string> hashSet2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		foreach (Dictionary globalClass in ProjectSettings.GetGlobalClassList())
		{
			if (globalClass.ContainsKey("path"))
			{
				string text = NormalizeScriptResourcePath(sourcePath, globalClass["path"].AsString());
				if (!string.IsNullOrWhiteSpace(text))
				{
					hashSet2.Add(text);
				}
			}
		}
		return hashSet.IsSubsetOf(hashSet2);
	}

	private static PackedScene LoadScriptlessPreviewPackedScene(string sourcePath)
	{
		if (string.IsNullOrWhiteSpace(sourcePath) || !Godot.FileAccess.FileExists(sourcePath))
		{
			return null;
		}
		if (sourcePath.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || sourcePath.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			return null;
		}
		string sceneText = StripUncompiledCSharpScriptResources(Godot.FileAccess.GetFileAsString(sourcePath), out var changed);
		sceneText = StripRuntimeCharacterScenePreviewBase(sceneText, out var changed2);
		if (!(changed | changed2))
		{
			return null;
		}
		string path = sourcePath.GetBaseDir().PathJoin($".modeditor_scene_scriptless_preview_{Guid.NewGuid():N}.tscn");
		using Godot.FileAccess fileAccess = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Write);
		if (fileAccess == null)
		{
			return null;
		}
		fileAccess.StoreString(sceneText);
		fileAccess.Close();
		PackedScene result = ResourceLoader.Load<PackedScene>(path, "", ResourceLoader.CacheMode.Ignore);
		DirAccess.RemoveAbsolute(path);
		return result;
	}

	private bool TryRecoverScriptlessPreviewTab(SceneTabInfo tabInfo, bool showToast)
	{
		if (tabInfo == null || !tabInfo.ScriptlessPreview)
		{
			return true;
		}
		string text = ((!string.IsNullOrWhiteSpace(tabInfo.SourcePath)) ? tabInfo.SourcePath : (tabInfo.PackedScene?.ResourcePath ?? ""));
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(text, "", ResourceLoader.CacheMode.Ignore);
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			return false;
		}
		Node node = TryInstantiateFullScene(packedScene, text);
		if (!GodotObject.IsInstanceValid(node))
		{
			return false;
		}
		bool flag = tabInfo.Key == _currentTabKey;
		if (flag)
		{
			tabInfo.ViewportState = _viewport.GetState();
			_viewport.ClearSceneInstance();
		}
		_viewport.ReleaseHistoryResources(tabInfo.HistoryId);
		XWEditorInterface.Instance?.GetSceneTreeDock()?.ReleaseHistoryResources(tabInfo.HistoryId);
		_sceneHistoryManager?.ReleaseHistory(tabInfo.HistoryId);
		FreeSceneInstance(tabInfo.SceneInstance);
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		int num = ((!GodotObject.IsInstanceValid(xWUndoRedoManager)) ? 1 : xWUndoRedoManager.CreateHistoryScope());
		tabInfo.PackedScene = packedScene;
		tabInfo.SceneInstance = node;
		tabInfo.ScriptlessPreview = false;
		tabInfo.HistoryId = num;
		xWUndoRedoManager?.SetHistoryAsSaved(num);
		if (flag)
		{
			LoadTabData(tabInfo.Key);
		}
		UpdateSceneDirtyMarker(tabInfo.Key);
		if (showToast)
		{
			XWEditorInterface.Instance?.ShowToast("Mod C# 脚本已可用，场景已恢复为完整可编辑模式。");
		}
		return true;
	}

	private static string NormalizeScriptResourcePath(string sourcePath, string scriptResourcePath)
	{
		if (string.IsNullOrWhiteSpace(scriptResourcePath))
		{
			return "";
		}
		if (IsExternalCSharpScriptPath(scriptResourcePath) && TryResolveExternalScriptPath(sourcePath, scriptResourcePath, out var resolvedPath))
		{
			return NormalizeComparablePath(resolvedPath);
		}
		try
		{
			return NormalizeComparablePath(scriptResourcePath.StartsWith("res://", StringComparison.OrdinalIgnoreCase) ? ProjectSettings.GlobalizePath(scriptResourcePath) : scriptResourcePath);
		}
		catch
		{
			return "";
		}
	}

	private static bool TryResolveExternalScriptPath(string sourcePath, string scriptPath, out string resolvedPath)
	{
		resolvedPath = "";
		if (string.IsNullOrWhiteSpace(sourcePath) || !IsExternalCSharpScriptPath(scriptPath))
		{
			return false;
		}
		try
		{
			string directoryName = Path.GetDirectoryName(Path.GetFullPath(sourcePath));
			if (string.IsNullOrWhiteSpace(directoryName))
			{
				return false;
			}
			resolvedPath = Path.GetFullPath(Path.Combine(directoryName, scriptPath.Replace('/', Path.DirectorySeparatorChar))).Replace('\\', '/');
			return File.Exists(resolvedPath);
		}
		catch
		{
			resolvedPath = "";
			return false;
		}
	}

	private static string NormalizeComparablePath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		try
		{
			return Path.GetFullPath(path).Replace('\\', '/').TrimEnd('/');
		}
		catch
		{
			return path.Replace('\\', '/').TrimEnd('/');
		}
	}

	private static string StripUncompiledCSharpScriptResources(string sceneText, out bool changed)
	{
		changed = false;
		if (string.IsNullOrWhiteSpace(sceneText))
		{
			return sceneText ?? "";
		}
		HashSet<string> hashSet = new HashSet<string>();
		List<string> list = new List<string>();
		string[] array = sceneText.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
		foreach (string text in array)
		{
			if (TryReadExternalScriptResource(text, out var scriptId))
			{
				hashSet.Add(scriptId);
				changed = true;
			}
			else
			{
				list.Add(text);
			}
		}
		if (hashSet.Count == 0)
		{
			return sceneText;
		}
		List<string> list2 = new List<string>();
		foreach (string item in list)
		{
			if (IsScriptAssignmentToStrippedResource(item, hashSet))
			{
				changed = true;
			}
			else
			{
				list2.Add(item);
			}
		}
		return string.Join("\n", list2);
	}

	private static string StripRuntimeCharacterScenePreviewBase(string sceneText, out bool changed)
	{
		changed = false;
		if (!IsModCharacterSceneText(sceneText) || !TryReadCharacterBaseSceneResourceId(sceneText, out var baseSceneId))
		{
			return sceneText ?? "";
		}
		List<string> list = new List<string>();
		string[] array = sceneText.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (IsCharacterBaseSceneResourceLine(text, baseSceneId))
			{
				changed = true;
				continue;
			}
			if (!flag && TryReplaceCharacterRootNodeForPreview(text, baseSceneId, out var replacementLine))
			{
				list.Add(replacementLine);
				list.Add("metadata/modeditor_scriptless_preview = true");
				flag = true;
				flag2 = true;
				changed = true;
				continue;
			}
			if (flag && text.StartsWith("[node "))
			{
				if (!flag3)
				{
					AddMinimalCharacterSpritePreviewHierarchy(list);
					flag3 = true;
					changed = true;
				}
				flag2 = false;
			}
			if (flag2 && IsRuntimeCharacterRootAssignment(text))
			{
				changed = true;
			}
			else if (IsRuntimeCharacterComponentPreviewLine(text))
			{
				changed = true;
			}
			else
			{
				list.Add(text);
			}
		}
		if (flag && !flag3)
		{
			AddMinimalCharacterSpritePreviewHierarchy(list);
			changed = true;
		}
		if (!changed)
		{
			return sceneText;
		}
		return string.Join("\n", list);
	}

	private static bool IsModCharacterSceneText(string sceneText)
	{
		if (!string.IsNullOrWhiteSpace(sceneText) && sceneText.Contains("metadata/mod_resource_kind = \"Character\"", StringComparison.OrdinalIgnoreCase))
		{
			return sceneText.Contains("metadata/mod_character_sprite_scene", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static bool TryReadCharacterBaseSceneResourceId(string sceneText, out string baseSceneId)
	{
		baseSceneId = "";
		if (string.IsNullOrWhiteSpace(sceneText))
		{
			return false;
		}
		string[] array = sceneText.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
		foreach (string text in array)
		{
			if (text.StartsWith("[ext_resource") && text.Contains("type=\"PackedScene\"") && text.Contains("res://Prefab/TowerDefense/Character/TowerDefense"))
			{
				baseSceneId = ReadSceneResourceAttribute(text, "id");
				return !string.IsNullOrWhiteSpace(baseSceneId);
			}
		}
		return false;
	}

	private static bool IsCharacterBaseSceneResourceLine(string line, string baseSceneId)
	{
		if (!string.IsNullOrWhiteSpace(baseSceneId) && !string.IsNullOrWhiteSpace(line) && line.StartsWith("[ext_resource"))
		{
			return line.Contains("id=\"" + baseSceneId + "\"");
		}
		return false;
	}

	private static bool TryReplaceCharacterRootNodeForPreview(string line, string baseSceneId, out string replacementLine)
	{
		replacementLine = "";
		if (string.IsNullOrWhiteSpace(line) || string.IsNullOrWhiteSpace(baseSceneId) || !line.StartsWith("[node ") || !line.Contains("instance=ExtResource(\"" + baseSceneId + "\")"))
		{
			return false;
		}
		string text = ReadSceneResourceAttribute(line, "name");
		if (string.IsNullOrWhiteSpace(text))
		{
			text = "CharacterPreview";
		}
		replacementLine = "[node name=\"" + text + "\" type=\"Node2D\"]";
		return true;
	}

	private static bool IsRuntimeCharacterRootAssignment(string line)
	{
		string text = line?.Trim() ?? "";
		if (!text.StartsWith("script = ExtResource") && !text.StartsWith("config = ") && !text.StartsWith("sprite = "))
		{
			return text.StartsWith("node_paths=");
		}
		return true;
	}

	private static bool IsRuntimeCharacterComponentPreviewLine(string line)
	{
		if (string.IsNullOrWhiteSpace(line))
		{
			return false;
		}
		if (!line.Contains("name=\"ComponentManager\"") && !line.Contains("parent=\"ComponentManager\"") && !line.Contains("parent=\"ComponentManager/"))
		{
			return line.Contains("parent=\"./ComponentManager");
		}
		return true;
	}

	private static void AddMinimalCharacterSpritePreviewHierarchy(List<string> lines)
	{
		lines.Add("");
		lines.Add("[node name=\"SpriteGroup\" type=\"Node2D\" parent=\".\"]");
		lines.Add("");
		lines.Add("[node name=\"TransformPoint\" type=\"Marker2D\" parent=\"SpriteGroup\"]");
		lines.Add("");
	}

	private static bool TryReadExternalScriptResource(string line, out string scriptId)
	{
		string scriptPath;
		return TryReadExternalScriptResource(line, out scriptId, out scriptPath);
	}

	private static bool TryReadExternalScriptResource(string line, out string scriptId, out string scriptPath)
	{
		scriptId = "";
		scriptPath = "";
		if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("[ext_resource") || !line.Contains("type=\"Script\""))
		{
			return false;
		}
		scriptPath = ReadSceneResourceAttribute(line, "path");
		if (!IsExternalCSharpScriptPath(scriptPath))
		{
			return false;
		}
		scriptId = ReadSceneResourceAttribute(line, "id");
		return !string.IsNullOrWhiteSpace(scriptId);
	}

	private static bool IsScriptAssignmentToStrippedResource(string line, HashSet<string> scriptIds)
	{
		if (scriptIds == null || scriptIds.Count == 0 || string.IsNullOrWhiteSpace(line))
		{
			return false;
		}
		string text = line.Trim();
		if (!text.StartsWith("script = ExtResource"))
		{
			return false;
		}
		foreach (string scriptId in scriptIds)
		{
			if (text == "script = ExtResource(\"" + scriptId + "\")")
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsExternalCSharpScriptPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return false;
		}
		string text = path.Replace('\\', '/');
		if (text.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) && !text.StartsWith("res://", StringComparison.OrdinalIgnoreCase))
		{
			return !text.StartsWith("uid://", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	private static string ReadSceneResourceAttribute(string line, string attributeName)
	{
		Match match = Regex.Match(line ?? "", "\\b" + Regex.Escape(attributeName) + "=\"([^\"]*)\"");
		if (!match.Success)
		{
			return "";
		}
		return match.Groups[1].Value;
	}

	public void RefreshSceneTree()
	{
		XWEditorInterface.Instance?.GetSceneTreeDock()?.RefreshScene();
	}

	private void BuildMenus()
	{
		PopupMenu popup = _snapConfigMenu.GetPopup();
		popup.Clear();
		popup.AddCheckItem("智能吸附", 1, Key.None);
		popup.AddCheckItem("网格吸附", 2, Key.None);
		popup.AddSeparator();
		popup.AddRadioCheckItem("显示网格", 3, Key.None);
		popup.AddRadioCheckItem("吸附时显示网格", 4, Key.None);
		popup.AddRadioCheckItem("隐藏网格", 5, Key.None);
		popup.AddSeparator();
		popup.AddItem("打开可视化网格与吸附面板…", 6, Key.None);
		popup.IdPressed += OnSnapMenuItem;
		PopupMenu popup2 = _skeletonMenu.GetPopup();
		popup2.Clear();
		popup2.AddItem("从节点生成骨骼", 1, Key.None);
		popup2.AddItem("清除骨骼", 2, Key.None);
		popup2.IdPressed += OnSkeletonMenuItem;
		PopupMenu popup3 = _viewMenu.GetPopup();
		popup3.Clear();
		popup3.AddItem("居中所选项", 1, Key.None);
		popup3.AddItem("聚焦所选项", 2, Key.None);
		popup3.AddSeparator();
		popup3.AddItem("放大", 3, Key.None);
		popup3.AddItem("缩小", 4, Key.None);
		popup3.AddItem("重置缩放", 5, Key.None);
		popup3.AddSeparator();
		popup3.AddCheckItem("显示原点", 10, Key.None);
		popup3.AddCheckItem("显示视口边框", 11, Key.None);
		popup3.AddCheckItem("显示标尺", 12, Key.None);
		popup3.AddCheckItem("显示参考线", 13, Key.None);
		popup3.AddCheckItem("显示辅助线", 14, Key.None);
		popup3.IdPressed += OnViewMenuItem;
		BuildViewportContextMenu();
		UpdateSnapMenuChecks();
		UpdateViewMenuChecks();
	}

	private void BuildViewportContextMenu()
	{
		_viewportContextMenu.Clear();
		XWClassRegistry instance = XWClassRegistry.Instance;
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("Add"), "添加子节点", 1, Key.None);
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("Instance"), "实例化场景...", 2, Key.None);
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("Save"), "保存场景", 3, Key.None);
		_viewportContextMenu.AddSeparator();
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("Rename"), "重命名", 10, Key.None);
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("Duplicate"), "复制", 11, Key.None);
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("MoveUp"), "上移", 12, Key.None);
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("MoveDown"), "下移", 13, Key.None);
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("Remove"), "删除", 14, Key.None);
		_viewportContextMenu.AddSeparator();
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("Lock"), "锁定所选项", 15, Key.None);
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("Unlock"), "解锁所选项", 16, Key.None);
		_viewportContextMenu.AddIconItem(LoadIcon("res://addons/ModEditor/Icons/ClassIcon/Group.svg"), "分组所选项", 17, Key.None);
		_viewportContextMenu.AddIconItem(LoadIcon("res://addons/ModEditor/Icons/ClassIcon/Ungroup.svg"), "取消所选分组", 18, Key.None);
		_viewportContextMenu.AddSeparator();
		_viewportContextMenu.AddItem("复制节点路径", 19, Key.None);
		_viewportContextMenu.AddItem("设为场景根节点", 20, Key.None);
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("PackedScene"), "保存分支为场景...", 21, Key.None);
		_viewportContextMenu.AddSeparator();
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("ScriptCreate"), "创建脚本", 30, Key.None);
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("ScriptRemove"), "分离脚本", 31, Key.None);
		_viewportContextMenu.AddIconItem(instance.GetUIIcon("ScriptExtend"), "扩展脚本", 32, Key.None);
		_viewportContextMenu.AddSeparator();
		_viewportContextMenu.AddIconItem(LoadIcon("res://addons/ModEditor/Icons/ClassIcon/CenterView.svg"), "居中所选项", 40, Key.None);
		_viewportContextMenu.AddItem("聚焦所选项", 41, Key.None);
		_viewportContextMenu.AddItem("放大", 42, Key.None);
		_viewportContextMenu.AddItem("缩小", 43, Key.None);
		_viewportContextMenu.AddItem("重置缩放", 44, Key.None);
		_viewportContextMenu.AddSeparator();
		_viewportContextMenu.AddCheckItem("显示原点", 50, Key.None);
		_viewportContextMenu.AddCheckItem("显示视口边框", 51, Key.None);
		_viewportContextMenu.AddCheckItem("显示标尺", 52, Key.None);
		_viewportContextMenu.AddCheckItem("显示参考线", 53, Key.None);
		_viewportContextMenu.AddCheckItem("显示辅助线", 54, Key.None);
		_viewportContextMenu.IdPressed += OnViewportContextMenuItem;
	}

	private void ConnectSignals()
	{
		_sceneTabBar.TabChanged += OnTabChanged;
		_sceneTabBar.TabClosePressed += OnTabClosePressed;
		_unsavedSceneCloseDialog.Confirmed += OnUnsavedSceneCloseConfirmed;
		_unsavedSceneCloseDialog.Canceled += OnUnsavedSceneCloseCanceled;
		_discardUnsavedSceneButton.Pressed += OnDiscardUnsavedScenePressed;
		_selectButton.Pressed += () =>
		{
			SetToolMode(XW2DViewport.ToolMode.Select);
		};
		_moveButton.Pressed += () =>
		{
			SetToolMode(XW2DViewport.ToolMode.Move);
		};
		_rotateButton.Pressed += () =>
		{
			SetToolMode(XW2DViewport.ToolMode.Rotate);
		};
		_scaleButton.Pressed += () =>
		{
			SetToolMode(XW2DViewport.ToolMode.Scale);
		};
		_listSelectButton.Pressed += () =>
		{
			SetToolMode(XW2DViewport.ToolMode.ListSelect);
		};
		_pivotButton.Pressed += () =>
		{
			SetToolMode(XW2DViewport.ToolMode.Pivot);
		};
		_panButton.Pressed += () =>
		{
			SetToolMode(XW2DViewport.ToolMode.Pan);
		};
		_rulerButton.Pressed += () =>
		{
			SetToolMode(XW2DViewport.ToolMode.Ruler);
		};
		_pathButton.Pressed += () =>
		{
			SetToolMode(XW2DViewport.ToolMode.Path);
		};
		_polygonButton.Pressed += () =>
		{
			SetToolMode(XW2DViewport.ToolMode.Polygon);
		};
		_localSpaceButton.Toggled += (bool pressed) =>
		{
			_viewport.UseLocalSpace = pressed;
		};
		_smartSnapButton.Toggled += (bool pressed) =>
		{
			_viewport.SmartSnapActive = pressed;
			UpdateSnapMenuChecks();
			_viewport.QueueRedrawAll();
		};
		_gridSnapButton.Toggled += (bool pressed) =>
		{
			_viewport.GridSnapActive = pressed;
			UpdateSnapMenuChecks();
			_viewport.QueueRedrawAll();
		};
		_lockButton.Pressed += () =>
		{
			SetSelectionMetaWithHistory("_edit_lock_", enabled: true);
		};
		_unlockButton.Pressed += () =>
		{
			SetSelectionMetaWithHistory("_edit_lock_", enabled: false);
		};
		_groupButton.Pressed += () =>
		{
			SetSelectionMetaWithHistory("_edit_group_", enabled: true);
		};
		_ungroupButton.Pressed += () =>
		{
			SetSelectionMetaWithHistory("_edit_group_", enabled: false);
		};
		_centerViewButton.Pressed += () =>
		{
			_viewport.CenterSelection();
		};
		_zoomOutButton.Pressed += () =>
		{
			_viewport.SetZoom(_viewport.Zoom / 1.1f);
		};
		_zoomResetButton.Pressed += () =>
		{
			_viewport.SetZoom(1f);
		};
		_zoomInButton.Pressed += () =>
		{
			_viewport.SetZoom(_viewport.Zoom * 1.1f);
		};
		_viewport.ZoomChanged += UpdateZoomLabel;
		_viewport.SelectionChanged += UpdateSelectionActionButtons;
		_viewport.MousePositionChanged += UpdateMousePositionLabel;
		XWEditorSelection xWEditorSelection = XWEditorInterface.Instance?.GetEditorSelection();
		if (xWEditorSelection != null)
		{
			xWEditorSelection.SelectionChanged += UpdateSelectionActionButtons;
			xWEditorSelection.SelectionChanged += UpdateDirectTransformSurface;
		}
		ConnectDirectTransformSurface();
		_allPropertiesButton.Pressed += () =>
		{
			ShowDirectNodeProperties();
		};
		_workspaceTabs.TabChanged += OnWorkspaceTabChanged;
	}

	private void ConnectDirectTransformSurface()
	{
		SpinBox[] directTransformSpins = GetDirectTransformSpins();
		foreach (SpinBox spinBox in directTransformSpins)
		{
			SpinBox capturedSpin = spinBox;
			spinBox.ValueChanged += (double _) =>
			{
				OnDirectTransformValueChanged(capturedSpin);
			};
			LineEdit lineEdit = spinBox.GetLineEdit();
			lineEdit.FocusEntered += BeginDirectTransformEditSession;
			lineEdit.FocusExited += CommitDirectTransformEditSession;
		}
	}

	private SpinBox[] GetDirectTransformSpins()
	{
		return new SpinBox[9] { _positionXSpin, _positionYSpin, _rotationSpin, _scaleXSpin, _scaleYSpin, _sizeXSpin, _sizeYSpin, _pivotXSpin, _pivotYSpin };
	}

	private void UpdateDirectTransformSurface()
	{
		if (_directTransformSessionActive)
		{
			CommitDirectTransformEditSession();
		}
		List<Node> obj = XWEditorInterface.Instance?.GetEditorSelection()?.GetSelectedNodes() ?? new List<Node>();
		_directTransformNodes.Clear();
		foreach (Node item in obj)
		{
			if ((item is Node2D || item is Control) ? true : false)
			{
				_directTransformNodes.Add(item);
			}
		}
		Node node = (_directTransformNode = ((_directTransformNodes.Count > 0) ? _directTransformNodes[0] : null));
		bool flag = _directTransformNodes.Count > 0;
		bool flag2 = flag;
		foreach (Node directTransformNode in _directTransformNodes)
		{
			flag2 &= directTransformNode is Control;
		}
		Label transformSelectionLabel = _transformSelectionLabel;
		string text;
		if (flag)
		{
			text = ((_directTransformNodes.Count == 1) ? $"{node.Name}  ·  {node.GetType().Name}" : $"{_directTransformNodes.Count} 个 2D 节点（批量）");
		}
		else
		{
			text = "未选择 2D 节点";
		}
		transformSelectionLabel.Text = text;
		_transformDirectPanel.TooltipText = (flag ? "直接编辑所选节点；混合值以 • 标识，修改一个字段会批量写回该字段" : "在画布或场景树中选择 Node2D / Control");
		SpinBox[] directTransformSpins = GetDirectTransformSpins();
		for (int i = 0; i < directTransformSpins.Length; i++)
		{
			directTransformSpins[i].Editable = flag;
		}
		_controlFields.Visible = flag2;
		_updatingDirectTransformSurface = true;
		List<DirectTransformState> list = new List<DirectTransformState>();
		foreach (Node directTransformNode2 in _directTransformNodes)
		{
			if (TryCaptureDirectTransformState(directTransformNode2, out var state))
			{
				list.Add(state);
			}
		}
		if (list.Count > 0)
		{
			SetDirectSpinValue(_positionXSpin, list, (DirectTransformState directTransformState) => directTransformState.Position.X);
			SetDirectSpinValue(_positionYSpin, list, (DirectTransformState directTransformState) => directTransformState.Position.Y);
			SetDirectSpinValue(_rotationSpin, list, (DirectTransformState directTransformState) => Mathf.RadToDeg(directTransformState.Rotation));
			SetDirectSpinValue(_scaleXSpin, list, (DirectTransformState directTransformState) => directTransformState.Scale.X);
			SetDirectSpinValue(_scaleYSpin, list, (DirectTransformState directTransformState) => directTransformState.Scale.Y);
			SetDirectSpinValue(_sizeXSpin, list, (DirectTransformState directTransformState) => directTransformState.Size.X, flag2);
			SetDirectSpinValue(_sizeYSpin, list, (DirectTransformState directTransformState) => directTransformState.Size.Y, flag2);
			SetDirectSpinValue(_pivotXSpin, list, (DirectTransformState directTransformState) => directTransformState.Pivot.X, flag2);
			SetDirectSpinValue(_pivotYSpin, list, (DirectTransformState directTransformState) => directTransformState.Pivot.Y, flag2);
		}
		else
		{
			directTransformSpins = GetDirectTransformSpins();
			foreach (SpinBox spinBox in directTransformSpins)
			{
				spinBox.SetValueNoSignal(0.0);
				ClearDirectSpinMixedMarker(spinBox);
			}
		}
		_updatingDirectTransformSurface = false;
		UpdateSelectionLayoutSurface();
		UpdateControlLayoutSurface();
		RefreshDirectNodePropertySurface();
	}

	public void ShowDirectNodeProperties(Node node = null, bool focusTab = true)
	{
		Node target = (GodotObject.IsInstanceValid(node) ? node : ResolveDirectPropertyTarget());
		BindDirectNodePropertyTarget(target);
		if (focusTab && GodotObject.IsInstanceValid(_workspaceTabs))
		{
			_workspaceTabs.CurrentTab = 1;
		}
	}

	public void RefreshDirectNodePropertySurface()
	{
		BindDirectNodePropertyTarget(ResolveDirectPropertyTarget());
	}

	private Node ResolveDirectPropertyTarget()
	{
		foreach (Node item in XWEditorInterface.Instance?.GetEditorSelection()?.GetSelectedNodes() ?? new List<Node>())
		{
			if (GodotObject.IsInstanceValid(item))
			{
				return item;
			}
		}
		if (!GodotObject.IsInstanceValid(CurrentSceneInstance))
		{
			return null;
		}
		return CurrentSceneInstance;
	}

	private void BindDirectNodePropertyTarget(Node target)
	{
		if (GodotObject.IsInstanceValid(_nodeDirectPropertySurface))
		{
			XWUndoRedoManager undoRedo = XWEditorInterface.Instance?.GetUndoRedoManager();
			_nodeDirectPropertySurface.BindObject(target, undoRedo, GetSpecialized2DPropertyNames(target), GetCurrentSceneHistoryId(), includeScript: true);
			bool flag = GodotObject.IsInstanceValid(target);
			if (GodotObject.IsInstanceValid(_allPropertiesButton))
			{
				_allPropertiesButton.Disabled = !flag;
				_allPropertiesButton.TooltipText = (flag ? $"在 2D 主工作区打开 {target.Name} 的全部属性拼图" : "请先打开场景或选择节点");
			}
		}
	}

	private static IReadOnlyCollection<string> GetSpecialized2DPropertyNames(Node target)
	{
		HashSet<string> hashSet = new HashSet<string> { "position", "rotation", "rotation_degrees", "scale" };
		if (target is Control)
		{
			hashSet.UnionWith(new string[12]
			{
				"size", "pivot_offset", "anchor_left", "anchor_top", "anchor_right", "anchor_bottom", "offset_left", "offset_top", "offset_right", "offset_bottom",
				"grow_horizontal", "grow_vertical"
			});
		}
		return hashSet;
	}

	private void OnDirectNodePropertyEdited(GodotObject target, StringName property, StringName field, Variant value)
	{
		MarkCurrentSceneHistorySurfaceDirty();
		_nodeDirectPropertySurface?.RefreshValues();
		_viewport?.NotifySceneStructureChanged();
		_viewport?.QueueRedrawAll();
		UpdateCurrentSceneDirtyMarker();
	}

	private void OnWorkspaceTabChanged(long tab)
	{
		if (tab == 1)
		{
			XWEditorInterface.Instance?.GetUndoRedoManager()?.SetCurrentHistoryType(GetCurrentSceneHistoryId());
			RefreshDirectNodePropertySurface();
		}
	}

	private void BeginDirectTransformEditSession()
	{
		if (_updatingDirectTransformSurface || _directTransformSessionActive || _directTransformNodes.Count == 0)
		{
			return;
		}
		_directTransformStarts.Clear();
		foreach (Node directTransformNode in _directTransformNodes)
		{
			if (TryCaptureDirectTransformState(directTransformNode, out var state))
			{
				_directTransformStarts[directTransformNode] = state;
			}
		}
		if (_directTransformStarts.Count != 0)
		{
			_directTransformTouchedSpins.Clear();
			_directTransformSessionActive = true;
		}
	}

	private void OnDirectTransformValueChanged(SpinBox changedSpin)
	{
		if (_updatingDirectTransformSurface || _directTransformNodes.Count == 0)
		{
			return;
		}
		if (!_directTransformSessionActive)
		{
			BeginDirectTransformEditSession();
		}
		if (!_directTransformSessionActive)
		{
			return;
		}
		_directTransformTouchedSpins.Add(changedSpin);
		ClearDirectSpinMixedMarker(changedSpin);
		foreach (Node directTransformNode in _directTransformNodes)
		{
			ApplyDirectSpinValue(directTransformNode, changedSpin);
		}
		_viewport.QueueRedrawAll();
	}

	private void CommitDirectTransformEditSession()
	{
		if (!_directTransformSessionActive)
		{
			return;
		}
		_directTransformSessionActive = false;
		List<(Node, DirectTransformState, DirectTransformState)> list = new List<(Node, DirectTransformState, DirectTransformState)>();
		foreach (var (node2, directTransformState2) in _directTransformStarts)
		{
			if (TryCaptureDirectTransformState(node2, out var state) && HasDirectTransformChanged(directTransformState2, state))
			{
				list.Add((node2, directTransformState2, state));
			}
		}
		_directTransformStarts.Clear();
		_directTransformTouchedSpins.Clear();
		if (list.Count == 0)
		{
			return;
		}
		PrepareSceneSurfaceHistoryMutation();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (!GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			return;
		}
		xWUndoRedoManager.CreateAction((list.Count == 1) ? "直接编辑 2D 变换" : "批量编辑 2D 变换", mergeMode: false, GetCurrentSceneHistoryId());
		foreach (var item in list)
		{
			AddDirectTransformHistoryProperties(xWUndoRedoManager, item.Item1, item.Item2, item.Item3);
		}
		xWUndoRedoManager.AddDoMethod(this, "NotifyDirectTransformHistoryApplied");
		xWUndoRedoManager.AddUndoMethod(this, "NotifyDirectTransformHistoryApplied");
		xWUndoRedoManager.CommitAction();
		_viewport.ActivateSceneTransformHistoryDeferred();
	}

	private static void AddDirectTransformHistoryProperties(XWUndoRedoManager undoRedo, Node target, DirectTransformState before, DirectTransformState after)
	{
		undoRedo.AddDoProperty(target, "position", Variant.From(in after.Position));
		undoRedo.AddUndoProperty(target, "position", Variant.From(in before.Position));
		undoRedo.AddDoProperty(target, "rotation", Variant.From(in after.Rotation));
		undoRedo.AddUndoProperty(target, "rotation", Variant.From(in before.Rotation));
		undoRedo.AddDoProperty(target, "scale", Variant.From(in after.Scale));
		undoRedo.AddUndoProperty(target, "scale", Variant.From(in before.Scale));
		if (target is Control)
		{
			undoRedo.AddDoProperty(target, "size", Variant.From(in after.Size));
			undoRedo.AddUndoProperty(target, "size", Variant.From(in before.Size));
			undoRedo.AddDoProperty(target, "pivot_offset", Variant.From(in after.Pivot));
			undoRedo.AddUndoProperty(target, "pivot_offset", Variant.From(in before.Pivot));
		}
	}

	private static bool TryCaptureDirectTransformState(Node node, out DirectTransformState state)
	{
		state = default;
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			return false;
		}
		if (node is Control control)
		{
			state = new DirectTransformState(control.Position, control.Scale, control.Rotation, control.Size, control.PivotOffset, isControl: true);
			return true;
		}
		if (node is Node2D node2D)
		{
			state = new DirectTransformState(node2D.Position, node2D.Scale, node2D.Rotation, Vector2.Zero, Vector2.Zero, isControl: false);
			return true;
		}
		return false;
	}

	private static bool HasDirectTransformChanged(DirectTransformState before, DirectTransformState after)
	{
		if (before.IsControl == after.IsControl && before.Position.IsEqualApprox(after.Position) && before.Scale.IsEqualApprox(after.Scale) && Mathf.IsEqualApprox(before.Rotation, after.Rotation))
		{
			if (before.IsControl)
			{
				if (before.Size.IsEqualApprox(after.Size))
				{
					return !before.Pivot.IsEqualApprox(after.Pivot);
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public void NotifyDirectTransformHistoryApplied()
	{
		MarkCurrentSceneHistorySurfaceDirty();
		UpdateDirectTransformSurface();
		_viewport.QueueRedrawAll();
	}

	public void RefreshDirectTransformSurface()
	{
		UpdateDirectTransformSurface();
	}

	public void DetachCurrentSceneRootForHistory()
	{
		_sceneRootHistoryViewportState = _viewport?.GetState() ?? new Dictionary();
		_viewport?.DetachSceneInstanceForHistory();
	}

	public void ApplySceneRootFromHistory(Node sceneRoot)
	{
		if (sceneRoot != null && GodotObject.IsInstanceValid(sceneRoot))
		{
			CancelInlineTextEdit();
			if (_openTabs.TryGetValue(_currentTabKey, out var value))
			{
				value.SceneInstance = sceneRoot;
			}
			CurrentSceneInstance = sceneRoot;
			XWEditorInterface.Instance?.ClearSelection();
			_viewport?.SetSceneInstanceFromHistory(sceneRoot);
			XWEditorInterface.Instance?.SetEditedSceneRoot(sceneRoot);
			if (_sceneRootHistoryViewportState.Count > 0)
			{
				_viewport?.SetState(_sceneRootHistoryViewportState);
			}
			XWEditorInterface.Instance?.GetSceneTreeDock()?.ApplySceneRootFromHistory(sceneRoot);
			RebindSceneAnimationRoot(sceneRoot);
			UpdateSelectionActionButtons();
			UpdateDirectTransformSurface();
			QueueViewportRedraw();
		}
	}

	private void SaveCurrentTabState()
	{
		if (!string.IsNullOrEmpty(_currentTabKey) && _openTabs.TryGetValue(_currentTabKey, out var value))
		{
			value.ViewportState = _viewport.GetState();
			SaveSceneAnimationTabState(value);
		}
	}

	private void LoadTabData(string tabKey)
	{
		if (_openTabs.TryGetValue(tabKey, out var value))
		{
			CommitInlineTextEdit();
			CurrentPackedScene = value.PackedScene;
			CurrentSceneInstance = value.SceneInstance;
			_currentTabKey = tabKey;
			XWEditorInterface.Instance?.GetUndoRedoManager()?.SetCurrentHistoryType(value.HistoryId);
			XWEditorInterface.Instance?.ClearSelection();
			XWEditorInterface.Instance?.SetEditedSceneRoot(CurrentSceneInstance);
			_viewport.SetSceneInstance(CurrentSceneInstance);
			_viewport.SetState(value.ViewportState);
			SyncGridSettingsPanelFromViewport();
			SyncToolbarFromViewportState();
			XWEditorInterface.Instance?.GetSceneTreeDock()?.SetScene(CurrentSceneInstance);
			LoadSceneAnimationTab(value);
			ShowDirectNodeProperties(CurrentSceneInstance, focusTab: false);
			UpdateZoomLabel();
			UpdateSelectionActionButtons();
			UpdateCurrentSceneDirtyMarker();
		}
	}

	private void SwitchToTab(string tabKey)
	{
		int num = _tabKeys.IndexOf(tabKey);
		if (num >= 0)
		{
			SaveCurrentTabState();
			_isSwitchingTab = true;
			_sceneTabBar.CurrentTab = num;
			_isSwitchingTab = false;
			LoadTabData(tabKey);
			XWEditorInterface.Instance?.FocusPanel("2d_editor");
		}
	}

	private void OnTabChanged(long tabIdx)
	{
		if (!_isSwitchingTab && tabIdx >= 0 && tabIdx < _tabKeys.Count)
		{
			string text = _tabKeys[(int)tabIdx];
			if (!(text == _currentTabKey))
			{
				SaveCurrentTabState();
				LoadTabData(text);
			}
		}
	}

	private void OnTabClosePressed(long tabIdx)
	{
		if (tabIdx >= 0 && tabIdx < _tabKeys.Count)
		{
			RequestCloseSceneTab(_tabKeys[(int)tabIdx]);
		}
	}

	public int GetCurrentSceneHistoryId()
	{
		if (!_openTabs.TryGetValue(_currentTabKey, out var value))
		{
			return 1;
		}
		return value.HistoryId;
	}

	public int GetSceneHistoryIdAt(long tabIndex)
	{
		if (tabIndex < 0 || tabIndex >= _tabKeys.Count || !_openTabs.TryGetValue(_tabKeys[(int)tabIndex], out var value))
		{
			return -1;
		}
		return value.HistoryId;
	}

	public bool IsSceneTabDirty(long tabIndex)
	{
		int sceneHistoryIdAt = GetSceneHistoryIdAt(tabIndex);
		if (sceneHistoryIdAt >= 0 && GodotObject.IsInstanceValid(_sceneHistoryManager))
		{
			return _sceneHistoryManager.IsHistoryUnsaved(sceneHistoryIdAt);
		}
		return false;
	}

	private void RequestCloseSceneTab(string tabKey)
	{
		if (_openTabs.TryGetValue(tabKey, out var value))
		{
			if (tabKey == _currentTabKey)
			{
				CommitInlineTextEdit();
			}
			if (GodotObject.IsInstanceValid(_sceneHistoryManager) && _sceneHistoryManager.IsHistoryUnsaved(value.HistoryId))
			{
				_pendingCloseTabKey = tabKey;
				_unsavedSceneCloseDialog.DialogText = "场景“" + value.DisplayName + "”仍有未保存的修改。";
				_unsavedSceneCloseDialog.PopupCentered();
			}
			else
			{
				CloseSceneTabImmediately(tabKey);
			}
		}
	}

	private void OnUnsavedSceneCloseConfirmed()
	{
		string pendingCloseTabKey = _pendingCloseTabKey;
		if (string.IsNullOrEmpty(pendingCloseTabKey) || !_openTabs.ContainsKey(pendingCloseTabKey))
		{
			_pendingCloseTabKey = "";
			return;
		}
		if (pendingCloseTabKey != _currentTabKey)
		{
			SwitchToTab(pendingCloseTabKey);
		}
		if (SaveCurrentScene())
		{
			_pendingCloseTabKey = "";
			CloseSceneTabImmediately(pendingCloseTabKey);
		}
	}

	private void OnUnsavedSceneCloseCanceled()
	{
		_pendingCloseTabKey = "";
	}

	private void OnDiscardUnsavedScenePressed()
	{
		string pendingCloseTabKey = _pendingCloseTabKey;
		_pendingCloseTabKey = "";
		_unsavedSceneCloseDialog.Hide();
		if (!string.IsNullOrEmpty(pendingCloseTabKey))
		{
			CloseSceneTabImmediately(pendingCloseTabKey);
		}
	}

	private void CloseSceneTabImmediately(string tabKey)
	{
		int num = _tabKeys.IndexOf(tabKey);
		if (num < 0 || !_openTabs.TryGetValue(tabKey, out var value))
		{
			return;
		}
		bool flag = tabKey == _currentTabKey;
		if (flag)
		{
			_viewport.ClearSceneInstance();
		}
		_viewport.ReleaseHistoryResources(value.HistoryId);
		XWEditorInterface.Instance?.GetSceneTreeDock()?.ReleaseHistoryResources(value.HistoryId);
		_sceneHistoryManager?.ReleaseHistory(value.HistoryId);
		FreeSceneInstance(value.SceneInstance);
		_openTabs.Remove(tabKey);
		_tabKeys.RemoveAt(num);
		_sceneTabBar.RemoveTab(num);
		if (_tabKeys.Count == 0)
		{
			ClearEditor();
			return;
		}
		int a = (flag ? Mathf.Min(num, _tabKeys.Count - 1) : _tabKeys.IndexOf(_currentTabKey));
		_isSwitchingTab = true;
		_sceneTabBar.CurrentTab = Mathf.Max(a, 0);
		_isSwitchingTab = false;
		if (flag)
		{
			LoadTabData(_tabKeys[Mathf.Max(a, 0)]);
		}
	}

	private void OnSceneHistoryChanged(int historyId)
	{
		foreach (var (text2, sceneTabInfo2) in _openTabs)
		{
			if (sceneTabInfo2.HistoryId == historyId)
			{
				UpdateSceneDirtyMarker(text2);
				if (text2 == _currentTabKey)
				{
					RefreshSceneAnimationAfterHistoryIfVisible();
				}
				break;
			}
		}
	}

	private void ClearEditor()
	{
		CancelInlineTextEdit();
		_currentTabKey = "";
		CurrentPackedScene = null;
		CurrentSceneInstance = null;
		_viewport.ClearSceneInstance();
		XWEditorInterface.Instance?.GetUndoRedoManager()?.SetCurrentHistoryType(0);
		XWEditorInterface.Instance?.ClearSelection();
		XWEditorInterface.Instance?.GetSceneTreeDock()?.SetScene(null);
		_sceneAnimationTimelinePanel?.ClearSurface();
		SetSceneAnimationTimelineVisible(visible: false);
		BindDirectNodePropertyTarget(null);
		if (GodotObject.IsInstanceValid(_workspaceTabs))
		{
			_workspaceTabs.CurrentTab = 0;
		}
		UpdateSelectionActionButtons();
	}

	public void OpenViewportContextMenu(Vector2 viewPosition, Vector2 worldPosition, Node hitNode)
	{
		if (hitNode != null && GodotObject.IsInstanceValid(hitNode))
		{
			XWEditorInterface.Instance?.SetSelectedNode(hitNode);
			XWEditorInterface.Instance?.GetSceneTreeDock()?.SelectNode(hitNode);
			ShowDirectNodeProperties(hitNode, focusTab: false);
			UpdateSelectionActionButtons();
		}
		_contextMenuWorldPosition = worldPosition;
		UpdateViewportContextMenuState();
		_viewportContextMenu.Position = (Vector2I)(_viewport.GetScreenPosition() + viewPosition);
		_viewportContextMenu.Popup();
	}

	public void QueueViewportRedraw()
	{
		_viewport?.NotifySceneStructureChanged();
	}

	public void PrepareForSceneHistoryBranchChange()
	{
		_viewport?.PrepareForSceneHistoryBranchChange();
	}

	private void SetToolMode(XW2DViewport.ToolMode mode)
	{
		_viewport.SetToolMode(mode);
		SetToolButtonPressed(_selectButton, mode == XW2DViewport.ToolMode.Select);
		SetToolButtonPressed(_moveButton, mode == XW2DViewport.ToolMode.Move);
		SetToolButtonPressed(_rotateButton, mode == XW2DViewport.ToolMode.Rotate);
		SetToolButtonPressed(_scaleButton, mode == XW2DViewport.ToolMode.Scale);
		SetToolButtonPressed(_listSelectButton, mode == XW2DViewport.ToolMode.ListSelect);
		SetToolButtonPressed(_pivotButton, mode == XW2DViewport.ToolMode.Pivot);
		SetToolButtonPressed(_panButton, mode == XW2DViewport.ToolMode.Pan);
		SetToolButtonPressed(_rulerButton, mode == XW2DViewport.ToolMode.Ruler);
		SetToolButtonPressed(_pathButton, mode == XW2DViewport.ToolMode.Path);
		SetToolButtonPressed(_polygonButton, mode == XW2DViewport.ToolMode.Polygon);
	}

	private void OnSnapMenuItem(long id)
	{
		switch ((SnapMenuId)id)
		{
		case SnapMenuId.SmartSnap:
			_viewport.SmartSnapActive = !_viewport.SmartSnapActive;
			break;
		case SnapMenuId.GridSnap:
			_viewport.GridSnapActive = !_viewport.GridSnapActive;
			break;
		case SnapMenuId.ShowGrid:
			_viewport.SetGridVisibility(XW2DViewport.GridVisibility.Show);
			break;
		case SnapMenuId.ShowGridWhenSnapping:
			_viewport.SetGridVisibility(XW2DViewport.GridVisibility.ShowWhenSnapping);
			break;
		case SnapMenuId.HideGrid:
			_viewport.SetGridVisibility(XW2DViewport.GridVisibility.Hide);
			break;
		case SnapMenuId.OpenVisualSettings:
			OpenGridSettingsPanel();
			break;
		}
		UpdateSnapButtons();
		UpdateSnapMenuChecks();
		_viewport.QueueRedrawAll();
	}

	private void OnViewMenuItem(long id)
	{
		switch ((ViewMenuId)id)
		{
		case ViewMenuId.CenterSelection:
			_viewport.CenterSelection();
			break;
		case ViewMenuId.FrameSelection:
			_viewport.FrameSelection();
			break;
		case ViewMenuId.ZoomIn:
			_viewport.SetZoom(_viewport.Zoom * 1.1f);
			break;
		case ViewMenuId.ZoomOut:
			_viewport.SetZoom(_viewport.Zoom / 1.1f);
			break;
		case ViewMenuId.ZoomReset:
			_viewport.SetZoom(1f);
			break;
		case ViewMenuId.ShowOrigin:
			_viewport.ShowOrigin = !_viewport.ShowOrigin;
			break;
		case ViewMenuId.ShowViewport:
			_viewport.ShowViewportRect = !_viewport.ShowViewportRect;
			break;
		case ViewMenuId.ShowRulers:
			_viewport.ShowRulers = !_viewport.ShowRulers;
			break;
		case ViewMenuId.ShowGuides:
			_viewport.ShowGuides = !_viewport.ShowGuides;
			break;
		case ViewMenuId.ShowHelpers:
			_viewport.ShowHelpers = !_viewport.ShowHelpers;
			break;
		}
		UpdateViewMenuChecks();
		_viewport.QueueRedrawAll();
	}

	private void OnViewportContextMenuItem(long id)
	{
		XWSceneTreeDock xWSceneTreeDock = XWEditorInterface.Instance?.GetSceneTreeDock();
		switch ((ViewportContextMenuId)id)
		{
		case ViewportContextMenuId.AddNode:
			xWSceneTreeDock?.PopupAddNodeMenuAtMouse(_contextMenuWorldPosition);
			break;
		case ViewportContextMenuId.InstantiateScene:
			xWSceneTreeDock?.ShowInstantiateDialog();
			break;
		case ViewportContextMenuId.SaveScene:
			SaveCurrentScene();
			break;
		case ViewportContextMenuId.Rename:
			xWSceneTreeDock?.ShowRenameSelectedDialog();
			break;
		case ViewportContextMenuId.Duplicate:
			xWSceneTreeDock?.DuplicateSelectedNode();
			break;
		case ViewportContextMenuId.MoveUp:
			xWSceneTreeDock?.MoveSelectedNode(-1);
			break;
		case ViewportContextMenuId.MoveDown:
			xWSceneTreeDock?.MoveSelectedNode(1);
			break;
		case ViewportContextMenuId.Delete:
			xWSceneTreeDock?.RemoveSelectedNode();
			break;
		case ViewportContextMenuId.LockSelection:
			SetSelectionMetaWithHistory("_edit_lock_", enabled: true);
			break;
		case ViewportContextMenuId.UnlockSelection:
			SetSelectionMetaWithHistory("_edit_lock_", enabled: false);
			break;
		case ViewportContextMenuId.GroupSelection:
			SetSelectionMetaWithHistory("_edit_group_", enabled: true);
			break;
		case ViewportContextMenuId.UngroupSelection:
			SetSelectionMetaWithHistory("_edit_group_", enabled: false);
			break;
		case ViewportContextMenuId.CopyNodePath:
			xWSceneTreeDock?.CopySelectedNodePath();
			break;
		case ViewportContextMenuId.MakeRoot:
			if (xWSceneTreeDock != null && xWSceneTreeDock.MakeSelectedNodeRoot())
			{
				XWEditorInterface.Instance?.ShowToast("已设置场景根节点");
			}
			break;
		case ViewportContextMenuId.SaveBranchAsScene:
			xWSceneTreeDock?.ShowSaveBranchDialogForSelected();
			break;
		case ViewportContextMenuId.CreateScript:
			xWSceneTreeDock?.ShowCreateScriptDialog();
			break;
		case ViewportContextMenuId.DetachScript:
			xWSceneTreeDock?.DetachSelectedScript();
			break;
		case ViewportContextMenuId.ExtendScript:
			xWSceneTreeDock?.ExtendSelectedScript();
			break;
		case ViewportContextMenuId.CenterSelection:
			_viewport.CenterSelection();
			break;
		case ViewportContextMenuId.FrameSelection:
			_viewport.FrameSelection();
			break;
		case ViewportContextMenuId.ZoomIn:
			_viewport.SetZoom(_viewport.Zoom * 1.1f);
			break;
		case ViewportContextMenuId.ZoomOut:
			_viewport.SetZoom(_viewport.Zoom / 1.1f);
			break;
		case ViewportContextMenuId.ZoomReset:
			_viewport.SetZoom(1f);
			break;
		case ViewportContextMenuId.ShowOrigin:
			_viewport.ShowOrigin = !_viewport.ShowOrigin;
			break;
		case ViewportContextMenuId.ShowViewport:
			_viewport.ShowViewportRect = !_viewport.ShowViewportRect;
			break;
		case ViewportContextMenuId.ShowRulers:
			_viewport.ShowRulers = !_viewport.ShowRulers;
			break;
		case ViewportContextMenuId.ShowGuides:
			_viewport.ShowGuides = !_viewport.ShowGuides;
			break;
		case ViewportContextMenuId.ShowHelpers:
			_viewport.ShowHelpers = !_viewport.ShowHelpers;
			break;
		}
		UpdateSelectionActionButtons();
		UpdateViewMenuChecks();
		UpdateViewportContextMenuState();
		_viewport.QueueRedrawAll();
	}

	private void OnSkeletonMenuItem(long id)
	{
		switch ((SkeletonMenuId)id)
		{
		case SkeletonMenuId.MakeBones:
			GenerateSkeletonFromSelectionWithHistory();
			break;
		case SkeletonMenuId.ClearBones:
			ClearGeneratedSkeletonWithHistory();
			break;
		}
	}

	public Skeleton2D GenerateSkeletonFromSelectionWithHistory()
	{
		Node currentSceneInstance = CurrentSceneInstance;
		if (currentSceneInstance == null || !GodotObject.IsInstanceValid(currentSceneInstance))
		{
			XWEditorInterface.Instance?.ShowToast("请先打开 2D 场景");
			return null;
		}
		Skeleton2D skeleton2D = FindGeneratedSkeleton(currentSceneInstance);
		if (GodotObject.IsInstanceValid(skeleton2D))
		{
			XWEditorInterface.Instance?.ShowToast("当前场景已有生成骨骼，请先清除");
			return skeleton2D;
		}
		List<Node2D> list = CollectBoneSourceNodes(currentSceneInstance);
		if (list.Count == 0)
		{
			XWEditorInterface.Instance?.ShowToast("请选择至少一个 Node2D 节点生成骨骼");
			return null;
		}
		list.Sort((Node2D left, Node2D right) => GetNodeDepth(left).CompareTo(GetNodeDepth(right)));
		Skeleton2D skeleton2D2 = new Skeleton2D
		{
			Name = BuildUniqueChildName(currentSceneInstance, "GeneratedSkeleton2D")
		};
		skeleton2D2.SetMeta("_xw_generated_skeleton_2d", true);
		System.Collections.Generic.Dictionary<Node2D, Bone2D> dictionary = new System.Collections.Generic.Dictionary<Node2D, Bone2D>();
		foreach (Node2D item in list)
		{
			Node2D node2D = FindNearestBoneSourceAncestor(item, dictionary);
			Node node = ((node2D != null) ? ((Node2D)dictionary[node2D]) : ((Node2D)skeleton2D2));
			Transform2D boneRelativeTransform = GetBoneRelativeTransform(currentSceneInstance, item, node2D);
			Bone2D bone2D = new Bone2D
			{
				Name = BuildUniqueChildName(node, $"{item.Name}Bone"),
				Transform = boneRelativeTransform
			};
			bone2D.Rest = bone2D.Transform;
			bone2D.SetAutocalculateLengthAndAngle(autoCalculate: false);
			bone2D.SetLength(32f);
			bone2D.SetBoneAngle(0f);
			bone2D.SetMeta("_xw_bone_source_path", currentSceneInstance.GetPathTo(item).ToString());
			node.AddChild(bone2D, forceReadableName: false, InternalMode.Disabled);
			dictionary[item] = bone2D;
		}
		foreach (Bone2D value in dictionary.Values)
		{
			UpdateGeneratedBoneGizmo(value);
		}
		if ((XWEditorInterface.Instance?.GetSceneTreeDock())?.AddNodeToParentWithHistory(currentSceneInstance, skeleton2D2, "从节点生成 2D 骨骼") != skeleton2D2)
		{
			if (GodotObject.IsInstanceValid(skeleton2D2) && skeleton2D2.GetParent() == null)
			{
				skeleton2D2.Free();
			}
			XWEditorInterface.Instance?.ShowToast("无法把生成骨骼加入当前场景");
			return null;
		}
		XWEditorInterface.Instance?.ShowToast($"已生成 {dictionary.Count} 根 2D 骨骼");
		QueueViewportRedraw();
		return skeleton2D2;
	}

	public bool ClearGeneratedSkeletonWithHistory()
	{
		Node currentSceneInstance = CurrentSceneInstance;
		if (currentSceneInstance == null || !GodotObject.IsInstanceValid(currentSceneInstance))
		{
			return false;
		}
		Skeleton2D skeleton2D = FindSelectedGeneratedSkeleton() ?? FindGeneratedSkeleton(currentSceneInstance);
		if (!GodotObject.IsInstanceValid(skeleton2D))
		{
			XWEditorInterface.Instance?.ShowToast("当前场景没有可清除的生成骨骼");
			return false;
		}
		XWEditorInterface instance = XWEditorInterface.Instance;
		int num;
		if (instance == null)
		{
			num = 0;
		}
		else
		{
			num = ((instance.GetSceneTreeDock()?.RemoveNodeWithHistory(skeleton2D, "清除 2D 骨骼") == true) ? 1 : 0);
			if (num != 0)
			{
				XWEditorInterface.Instance?.ShowToast("已清除生成的 2D 骨骼");
				QueueViewportRedraw();
			}
		}
		return (byte)num != 0;
	}

	private List<Node2D> CollectBoneSourceNodes(Node sceneRoot)
	{
		List<Node2D> list = new List<Node2D>();
		HashSet<Node2D> seen = new HashSet<Node2D>();
		foreach (Node item in XWEditorInterface.Instance?.GetEditorSelection()?.GetSelectedNodes() ?? new List<Node>())
		{
			AddBoneSourceNode(sceneRoot, item, list, seen);
		}
		if (list.Count == 0)
		{
			AddBoneSourceNode(sceneRoot, XWEditorInterface.Instance?.GetSceneTreeDock()?.GetSelectedNode(), list, seen);
		}
		return list;
	}

	private static void AddBoneSourceNode(Node sceneRoot, Node candidate, List<Node2D> result, HashSet<Node2D> seen)
	{
		Node2D node2D = candidate as Node2D;
		bool flag = node2D == null;
		if (!flag)
		{
			bool flag2 = ((candidate is Bone2D || candidate is Skeleton2D) ? true : false);
			flag = flag2;
		}
		if (!flag && GodotObject.IsInstanceValid(candidate) && candidate != sceneRoot && (sceneRoot.IsAncestorOf(candidate) || sceneRoot == candidate) && seen.Add(node2D))
		{
			result.Add(node2D);
		}
	}

	private static Node2D FindNearestBoneSourceAncestor(Node2D source, System.Collections.Generic.Dictionary<Node2D, Bone2D> sourceToBone)
	{
		for (Node parent = source.GetParent(); parent != null; parent = parent.GetParent())
		{
			if (parent is Node2D node2D && sourceToBone.ContainsKey(node2D))
			{
				return node2D;
			}
		}
		return null;
	}

	private static Transform2D GetBoneRelativeTransform(Node sceneRoot, Node2D source, Node2D parentSource)
	{
		if (GodotObject.IsInstanceValid(parentSource))
		{
			return parentSource.GlobalTransform.AffineInverse() * source.GlobalTransform;
		}
		if (sceneRoot is CanvasItem canvasItem)
		{
			return canvasItem.GetGlobalTransform().AffineInverse() * source.GlobalTransform;
		}
		return source.Transform;
	}

	private static void UpdateGeneratedBoneGizmo(Bone2D bone)
	{
		Bone2D bone2D = null;
		foreach (Node child in bone.GetChildren())
		{
			if (child is Bone2D bone2D2)
			{
				bone2D = bone2D2;
				break;
			}
		}
		Vector2 vector = bone2D?.Position ?? new Vector2(32f, 0f);
		float length = Mathf.Max(vector.Length(), 16f);
		bone.SetLength(length);
		bone.SetBoneAngle((vector.LengthSquared() > 0.001f) ? vector.Angle() : 0f);
		bone.Rest = bone.Transform;
	}

	private Skeleton2D FindSelectedGeneratedSkeleton()
	{
		foreach (Node item in XWEditorInterface.Instance?.GetEditorSelection()?.GetSelectedNodes() ?? new List<Node>())
		{
			for (Node node = item; node != null; node = node.GetParent())
			{
				if (node is Skeleton2D skeleton2D && IsGeneratedSkeleton(skeleton2D))
				{
					return skeleton2D;
				}
			}
		}
		return null;
	}

	private static Skeleton2D FindGeneratedSkeleton(Node node)
	{
		if (node is Skeleton2D skeleton2D && IsGeneratedSkeleton(skeleton2D))
		{
			return skeleton2D;
		}
		foreach (Node child in node.GetChildren())
		{
			Skeleton2D skeleton2D2 = FindGeneratedSkeleton(child);
			if (GodotObject.IsInstanceValid(skeleton2D2))
			{
				return skeleton2D2;
			}
		}
		return null;
	}

	private static bool IsGeneratedSkeleton(Skeleton2D skeleton)
	{
		if (GodotObject.IsInstanceValid(skeleton) && skeleton.HasMeta("_xw_generated_skeleton_2d"))
		{
			return skeleton.GetMeta("_xw_generated_skeleton_2d").AsBool();
		}
		return false;
	}

	private static int GetNodeDepth(Node node)
	{
		int num = 0;
		for (Node node2 = node?.GetParent(); node2 != null; node2 = node2.GetParent())
		{
			num++;
		}
		return num;
	}

	private static StringName BuildUniqueChildName(Node parent, string preferredName)
	{
		string text = preferredName;
		int num = 2;
		while (parent.HasNode(new NodePath(text)))
		{
			text = $"{preferredName}{num++}";
		}
		return new StringName(text);
	}

	private bool SetSelectionMetaWithHistory(string metaName, bool enabled)
	{
		return CommitSelectionMetaHistory(metaName, enabled);
	}

	private void UpdateSelectionActionButtons()
	{
		int valueOrDefault = (XWEditorInterface.Instance?.GetEditorSelection()?.GetSelectedCount()).GetValueOrDefault();
		_lockButton.Disabled = valueOrDefault == 0;
		_unlockButton.Disabled = valueOrDefault == 0;
		_groupButton.Disabled = valueOrDefault == 0;
		_ungroupButton.Disabled = valueOrDefault == 0;
		_centerViewButton.Disabled = valueOrDefault == 0;
	}

	private void UpdateSnapButtons()
	{
		_smartSnapButton.SetPressedNoSignal(_viewport.SmartSnapActive);
		_gridSnapButton.SetPressedNoSignal(_viewport.GridSnapActive);
	}

	private void SyncToolbarFromViewportState()
	{
		_localSpaceButton?.SetPressedNoSignal(_viewport.UseLocalSpace);
		UpdateSnapButtons();
		UpdateSnapMenuChecks();
		UpdateViewMenuChecks();
		UpdateZoomLabel();
	}

	private void UpdateSnapMenuChecks()
	{
		PopupMenu popup = _snapConfigMenu.GetPopup();
		SetMenuChecked(popup, 1, _viewport.SmartSnapActive);
		SetMenuChecked(popup, 2, _viewport.GridSnapActive);
		SetMenuChecked(popup, 3, _viewport.GridMode == XW2DViewport.GridVisibility.Show);
		SetMenuChecked(popup, 4, _viewport.GridMode == XW2DViewport.GridVisibility.ShowWhenSnapping);
		SetMenuChecked(popup, 5, _viewport.GridMode == XW2DViewport.GridVisibility.Hide);
	}

	private void UpdateViewMenuChecks()
	{
		PopupMenu popup = _viewMenu.GetPopup();
		SetMenuChecked(popup, 10, _viewport.ShowOrigin);
		SetMenuChecked(popup, 11, _viewport.ShowViewportRect);
		SetMenuChecked(popup, 12, _viewport.ShowRulers);
		SetMenuChecked(popup, 13, _viewport.ShowGuides);
		SetMenuChecked(popup, 14, _viewport.ShowHelpers);
	}

	private void UpdateViewportContextMenuState()
	{
		XWSceneTreeDock xWSceneTreeDock = XWEditorInterface.Instance?.GetSceneTreeDock();
		Node node = xWSceneTreeDock?.GetSelectedNode();
		bool flag = CurrentSceneInstance != null && GodotObject.IsInstanceValid(CurrentSceneInstance);
		bool flag2 = node != null && GodotObject.IsInstanceValid(node);
		bool flag3 = xWSceneTreeDock?.IsSelectedNodeRoot() ?? false;
		XWEditorInterface instance = XWEditorInterface.Instance;
		bool flag4 = instance != null && instance.GetEditorSelection()?.GetSelectedCount() > 0;
		bool flag5 = xWSceneTreeDock?.SelectedNodeHasScript() ?? false;
		SetMenuDisabled(_viewportContextMenu, 1, !flag);
		SetMenuDisabled(_viewportContextMenu, 2, !flag);
		SetMenuDisabled(_viewportContextMenu, 3, !flag);
		SetMenuDisabled(_viewportContextMenu, 10, !flag2);
		SetMenuDisabled(_viewportContextMenu, 11, !flag2 | flag3);
		SetMenuDisabled(_viewportContextMenu, 12, !flag2 | flag3);
		SetMenuDisabled(_viewportContextMenu, 13, !flag2 | flag3);
		SetMenuDisabled(_viewportContextMenu, 14, !flag2 | flag3);
		SetMenuDisabled(_viewportContextMenu, 15, !flag4);
		SetMenuDisabled(_viewportContextMenu, 16, !flag4);
		SetMenuDisabled(_viewportContextMenu, 17, !flag4);
		SetMenuDisabled(_viewportContextMenu, 18, !flag4);
		SetMenuDisabled(_viewportContextMenu, 19, !flag2);
		SetMenuDisabled(_viewportContextMenu, 20, !flag2 | flag3);
		SetMenuDisabled(_viewportContextMenu, 21, !flag2);
		SetMenuDisabled(_viewportContextMenu, 30, !flag2 | flag5);
		SetMenuDisabled(_viewportContextMenu, 31, !flag2 || !flag5);
		SetMenuDisabled(_viewportContextMenu, 32, !flag2 || !flag5);
		SetMenuDisabled(_viewportContextMenu, 40, !flag4);
		SetMenuDisabled(_viewportContextMenu, 41, !flag4);
		SetMenuChecked(_viewportContextMenu, 50, _viewport.ShowOrigin);
		SetMenuChecked(_viewportContextMenu, 51, _viewport.ShowViewportRect);
		SetMenuChecked(_viewportContextMenu, 52, _viewport.ShowRulers);
		SetMenuChecked(_viewportContextMenu, 53, _viewport.ShowGuides);
		SetMenuChecked(_viewportContextMenu, 54, _viewport.ShowHelpers);
	}

	private void UpdateZoomLabel()
	{
		_zoomLabel.Text = $"{Mathf.RoundToInt(_viewport.Zoom * 100f)}%";
	}

	private void UpdateMousePositionLabel(Vector2 position)
	{
		_mousePositionLabel.Text = $"{Mathf.RoundToInt(position.X)}, {Mathf.RoundToInt(position.Y)}";
	}

	private static string GetSceneKey(PackedScene scene)
	{
		if (string.IsNullOrWhiteSpace(scene.ResourcePath))
		{
			return scene.GetInstanceId().ToString();
		}
		return scene.ResourcePath;
	}

	private static string GetSceneDisplayName(PackedScene scene)
	{
		if (!string.IsNullOrWhiteSpace(scene.ResourcePath))
		{
			return scene.ResourcePath.GetFile().GetBaseName();
		}
		if (!string.IsNullOrWhiteSpace(scene.ResourceName))
		{
			return scene.ResourceName;
		}
		return "未命名";
	}

	private static void SetToolButtonPressed(Button button, bool pressed)
	{
		button?.SetPressedNoSignal(pressed);
	}

	private static void SetMenuChecked(PopupMenu popup, int itemId, bool checkedState)
	{
		int itemIndex = popup.GetItemIndex(itemId);
		if (itemIndex >= 0)
		{
			popup.SetItemChecked(itemIndex, checkedState);
		}
	}

	private static void SetMenuDisabled(PopupMenu popup, int itemId, bool disabled)
	{
		int itemIndex = popup.GetItemIndex(itemId);
		if (itemIndex >= 0)
		{
			popup.SetItemDisabled(itemIndex, disabled);
		}
	}

	private static Texture2D LoadIcon(string path)
	{
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		return XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse));
	}

	private static void FreeSceneInstance(Node sceneInstance)
	{
		if (sceneInstance != null && GodotObject.IsInstanceValid(sceneInstance))
		{
			sceneInstance.GetParent()?.RemoveChild(sceneInstance);
			sceneInstance.QueueFree();
		}
	}

	private void InitializeSceneGridSettings()
	{
		_gridSettingsPopup = GetNodeOrNull<PopupPanel>("%GridSettingsPopup") ?? (FindChild("GridSettingsPopup", recursive: true, owned: false) as PopupPanel);
		_gridSettingsPanel = GetNodeOrNull<XW2DGridSettingsPanel>("%GridSettingsPanel") ?? (FindChild("GridSettingsPanel", recursive: true, owned: false) as XW2DGridSettingsPanel);
		if (!GodotObject.IsInstanceValid(_gridSettingsPopup) || !GodotObject.IsInstanceValid(_gridSettingsPanel))
		{
			GD.PushError("2D 编辑器缺少可视化网格与吸附配置面板。");
			return;
		}
		LoadPersistedGridSettings();
		_gridSettingsSaveTimer = new Timer
		{
			Name = "GridSettingsSaveDebounce",
			OneShot = true,
			WaitTime = 0.35,
			ProcessCallback = Timer.TimerProcessCallback.Idle
		};
		AddChild(_gridSettingsSaveTimer, forceReadableName: false, InternalMode.Disabled);
		_gridSettingsSaveTimer.Timeout += FlushGridSettingsSave;
		_gridSettingsPanel.SettingsChanged += OnGridSettingsChanged;
		SyncGridSettingsPanelFromViewport();
	}

	private void ShutdownSceneGridSettings()
	{
		if (GodotObject.IsInstanceValid(_gridSettingsPanel))
		{
			_gridSettingsPanel.SettingsChanged -= OnGridSettingsChanged;
		}
		if (GodotObject.IsInstanceValid(_gridSettingsSaveTimer))
		{
			_gridSettingsSaveTimer.Timeout -= FlushGridSettingsSave;
			if (!_gridSettingsSaveTimer.IsStopped())
			{
				FlushGridSettingsSave();
			}
		}
	}

	private void OpenGridSettingsPanel()
	{
		if (GodotObject.IsInstanceValid(_gridSettingsPopup) && GodotObject.IsInstanceValid(_gridSettingsPanel))
		{
			SyncGridSettingsPanelFromViewport();
			_gridSettingsPopup.PopupCentered(new Vector2I(900, 720));
		}
	}

	private void SyncGridSettingsPanelFromViewport()
	{
		if (GodotObject.IsInstanceValid(_gridSettingsPanel) && GodotObject.IsInstanceValid(_viewport))
		{
			_gridSettingsPanel.BindState(BuildGridPanelState());
		}
	}

	private Dictionary BuildGridPanelState()
	{
		return new Dictionary
		{
			["grid_offset"] = _viewport?.GridOffset ?? Vector2.Zero,
			["grid_step"] = _viewport?.GridStep ?? new Vector2(8f, 8f),
			["primary_grid_step"] = _viewport?.PrimaryGridStep ?? new Vector2I(8, 8),
			["move_snap_step"] = _viewport?.MoveSnapStep ?? new Vector2(8f, 8f),
			["rotate_snap_step"] = _viewport?.RotationSnapStepDegrees ?? 15f,
			["scale_snap_step"] = _viewport?.ScaleSnapStep ?? 0.1f,
			["smart_snap_threshold"] = _viewport?.SmartSnapThreshold ?? 8f
		};
	}

	private void OnGridSettingsChanged()
	{
		if (GodotObject.IsInstanceValid(_gridSettingsPanel) && GodotObject.IsInstanceValid(_viewport))
		{
			_viewport.GridOffset = _gridSettingsPanel.GridOffset;
			_viewport.GridStep = _gridSettingsPanel.GridStep;
			_viewport.PrimaryGridStep = _gridSettingsPanel.PrimaryGridStep;
			_viewport.MoveSnapStep = _gridSettingsPanel.MoveSnapStep;
			_viewport.RotationSnapStepDegrees = _gridSettingsPanel.RotateSnapStep;
			_viewport.ScaleSnapStep = _gridSettingsPanel.ScaleSnapStep;
			_viewport.SmartSnapThreshold = _gridSettingsPanel.SmartSnapThreshold;
			_viewport.QueueRedrawAll();
			PersistGridSettings();
			SaveCurrentTabState();
		}
	}

	private void LoadPersistedGridSettings()
	{
		XWEditorSettings xWEditorSettings = XWEditorInterface.Instance?.GetEditorSettings();
		if (xWEditorSettings != null && GodotObject.IsInstanceValid(_viewport))
		{
			RegisterGridSettingDefaults(xWEditorSettings);
			_viewport.GridOffset = xWEditorSettings.GetSetting("2d_editor_grid/offset", Vector2.Zero);
			_viewport.GridStep = xWEditorSettings.GetSetting("2d_editor_grid/grid_step", new Vector2(8f, 8f));
			_viewport.PrimaryGridStep = xWEditorSettings.GetSetting("2d_editor_grid/primary_step", new Vector2I(8, 8));
			_viewport.MoveSnapStep = xWEditorSettings.GetSetting("2d_editor_grid/move_step", new Vector2(8f, 8f));
			_viewport.RotationSnapStepDegrees = xWEditorSettings.GetSetting("2d_editor_grid/rotation_degrees", 15f);
			_viewport.ScaleSnapStep = xWEditorSettings.GetSetting("2d_editor_grid/scale_step", 0.1f);
			_viewport.SmartSnapThreshold = xWEditorSettings.GetSetting("2d_editor_grid/smart_threshold", 8f);
		}
	}

	private void PersistGridSettings()
	{
		XWEditorSettings xWEditorSettings = XWEditorInterface.Instance?.GetEditorSettings();
		if (xWEditorSettings != null && GodotObject.IsInstanceValid(_viewport))
		{
			SetGridSettingIfChanged(xWEditorSettings, "2d_editor_grid/offset", _viewport.GridOffset);
			SetGridSettingIfChanged(xWEditorSettings, "2d_editor_grid/grid_step", _viewport.GridStep);
			SetGridSettingIfChanged(xWEditorSettings, "2d_editor_grid/primary_step", _viewport.PrimaryGridStep);
			SetGridSettingIfChanged(xWEditorSettings, "2d_editor_grid/move_step", _viewport.MoveSnapStep);
			SetGridSettingIfChanged(xWEditorSettings, "2d_editor_grid/rotation_degrees", _viewport.RotationSnapStepDegrees);
			SetGridSettingIfChanged(xWEditorSettings, "2d_editor_grid/scale_step", _viewport.ScaleSnapStep);
			SetGridSettingIfChanged(xWEditorSettings, "2d_editor_grid/smart_threshold", _viewport.SmartSnapThreshold);
			if (GodotObject.IsInstanceValid(_gridSettingsSaveTimer))
			{
				_gridSettingsSaveTimer.Start();
			}
			else
			{
				xWEditorSettings.Save();
			}
		}
	}

	private static void SetGridSettingIfChanged(XWEditorSettings settings, string key, Variant value)
	{
		if (!settings.HasSetting(key) || !settings.GetSetting(key).Equals(value))
		{
			settings.SetSetting(key, value);
		}
	}

	private void FlushGridSettingsSave()
	{
		XWEditorInterface.Instance?.GetEditorSettings()?.Save();
		_gridSettingsSaveTimer?.Stop();
	}

	private static void RegisterGridSettingDefaults(XWEditorSettings settings)
	{
		settings.RegisterSetting("2d_editor_grid/offset", Vector2.Zero);
		settings.RegisterSetting("2d_editor_grid/grid_step", new Vector2(8f, 8f));
		settings.RegisterSetting("2d_editor_grid/primary_step", new Vector2I(8, 8));
		settings.RegisterSetting("2d_editor_grid/move_step", new Vector2(8f, 8f));
		settings.RegisterSetting("2d_editor_grid/rotation_degrees", 15f);
		settings.RegisterSetting("2d_editor_grid/scale_step", 0.1f);
		settings.RegisterSetting("2d_editor_grid/smart_threshold", 8f);
	}

	private void SeedSceneGridTabState(SceneTabInfo tabInfo)
	{
		if (tabInfo != null)
		{
			Dictionary dictionary = BuildGridPanelState();
			tabInfo.ViewportState = new Dictionary
			{
				["zoom"] = 1f,
				["ofs"] = Vector2.Zero,
				["grid_offset"] = dictionary["grid_offset"],
				["grid_step"] = dictionary["grid_step"],
				["primary_grid_step"] = dictionary["primary_grid_step"],
				["move_snap_step"] = dictionary["move_snap_step"],
				["rotation_snap_step_degrees"] = dictionary["rotate_snap_step"],
				["scale_snap_step"] = dictionary["scale_snap_step"],
				["smart_snap_threshold"] = dictionary["smart_snap_threshold"],
				["smart_snap_active"] = false,
				["grid_snap_active"] = false,
				["grid_visibility"] = 1,
				["show_origin"] = true,
				["show_viewport"] = true,
				["show_rulers"] = true,
				["show_guides"] = true,
				["show_helpers"] = false,
				["use_local_space"] = false,
				["vertical_guides"] = new Godot.Collections.Array(),
				["horizontal_guides"] = new Godot.Collections.Array()
			};
		}
	}

	private void InitializeInlineTextEditing()
	{
		if (GodotObject.IsInstanceValid(_viewport) && !GodotObject.IsInstanceValid(_inlineTextOverlay))
		{
			_inlineTextOverlay = new PanelContainer
			{
				Name = "InlineTextEditorOverlay",
				Visible = false,
				ZIndex = 500,
				MouseFilter = MouseFilterEnum.Stop,
				TooltipText = "直接覆盖在游戏文字位置；输入时即时预览，提交后进入当前场景的撤销历史。"
			};
			_inlineTextOverlay.AddThemeStyleboxOverride("panel", CreateInlineTextOverlayStyle());
			_viewport.AddChild(_inlineTextOverlay, forceReadableName: false, InternalMode.Disabled);
			MarginContainer marginContainer = new MarginContainer
			{
				Name = "InlineTextEditorMargin",
				MouseFilter = MouseFilterEnum.Pass
			};
			marginContainer.AddThemeConstantOverride("margin_left", 8);
			marginContainer.AddThemeConstantOverride("margin_top", 6);
			marginContainer.AddThemeConstantOverride("margin_right", 8);
			marginContainer.AddThemeConstantOverride("margin_bottom", 7);
			_inlineTextOverlay.AddChild(marginContainer, forceReadableName: false, InternalMode.Disabled);
			VBoxContainer vBoxContainer = new VBoxContainer
			{
				Name = "InlineTextEditorStack",
				MouseFilter = MouseFilterEnum.Pass
			};
			vBoxContainer.AddThemeConstantOverride("separation", 5);
			marginContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
			HBoxContainer hBoxContainer = new HBoxContainer
			{
				Name = "InlineTextEditorHeader",
				MouseFilter = MouseFilterEnum.Pass
			};
			hBoxContainer.AddThemeConstantOverride("separation", 5);
			vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
			TextureRect node = new TextureRect
			{
				Name = "InlineTextEditorIcon",
				Texture = LoadInlineTextIcon("Edit"),
				CustomMinimumSize = new Vector2(18f, 18f),
				ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
				StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
				MouseFilter = MouseFilterEnum.Ignore,
				Modulate = ModEditorTheme.SunGoldColor
			};
			hBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			_inlineTextModeLabel = new Label
			{
				Name = "InlineTextEditorMode",
				Text = "画面文字 · 即时预览",
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				VerticalAlignment = VerticalAlignment.Center,
				ClipText = true,
				TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis,
				MouseFilter = MouseFilterEnum.Ignore
			};
			_inlineTextModeLabel.AddThemeColorOverride("font_color", ModEditorTheme.SunGoldColor);
			_inlineTextModeLabel.AddThemeFontOverride("font", ModEditorTheme.BoldFont);
			_inlineTextModeLabel.AddThemeFontSizeOverride("font_size", 12);
			hBoxContainer.AddChild(_inlineTextModeLabel, forceReadableName: false, InternalMode.Disabled);
			_inlineTextCancelButton = CreateInlineTextHeaderButton("InlineTextCancelButton", "Close", "取消本次文字修改（Esc）");
			_inlineTextCommitButton = CreateInlineTextHeaderButton("InlineTextCommitButton", "Save", "完成并写入一次撤销历史（Enter）");
			hBoxContainer.AddChild(_inlineTextCancelButton, forceReadableName: false, InternalMode.Disabled);
			hBoxContainer.AddChild(_inlineTextCommitButton, forceReadableName: false, InternalMode.Disabled);
			_inlineTextLineEdit = new LineEdit
			{
				Name = "InlineTextLineEdit",
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				CustomMinimumSize = new Vector2(0f, 34f),
				SelectAllOnFocus = false,
				PlaceholderText = "在画面中输入文字"
			};
			vBoxContainer.AddChild(_inlineTextLineEdit, forceReadableName: false, InternalMode.Disabled);
			_inlineTextMultilineEdit = new TextEdit
			{
				Name = "InlineTextMultilineEdit",
				Visible = false,
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				SizeFlagsVertical = SizeFlags.ExpandFill,
				CustomMinimumSize = new Vector2(0f, 76f),
				PlaceholderText = "在画面中输入多行文字\nCtrl+Enter 完成"
			};
			vBoxContainer.AddChild(_inlineTextMultilineEdit, forceReadableName: false, InternalMode.Disabled);
			_inlineTextLineEdit.TextChanged += OnInlineTextLineChanged;
			_inlineTextLineEdit.TextSubmitted += (string _) =>
			{
				CommitInlineTextEdit();
			};
			_inlineTextLineEdit.FocusExited += QueueInlineTextFocusExitCommit;
			_inlineTextLineEdit.GuiInput += OnInlineTextEditorGuiInput;
			_inlineTextMultilineEdit.TextChanged += OnInlineTextMultilineChanged;
			_inlineTextMultilineEdit.FocusExited += QueueInlineTextFocusExitCommit;
			_inlineTextMultilineEdit.GuiInput += OnInlineTextEditorGuiInput;
			_inlineTextCancelButton.Pressed += CancelInlineTextEdit;
			_inlineTextCommitButton.Pressed += () =>
			{
				CommitInlineTextEdit();
			};
			VisibilityChanged += OnInlineTextHostVisibilityChanged;
			_viewport.VisibilityChanged += OnInlineTextViewportVisibilityChanged;
			_inlineTextOverlayMotion = XWUiMotion.BindPanel(_inlineTextOverlay);
			XWUiMotion.BindButton(_inlineTextCancelButton);
			XWUiMotion.BindButton(_inlineTextCommitButton);
		}
	}

	private void ShutdownInlineTextEditing()
	{
		CancelInlineTextEdit();
		VisibilityChanged -= OnInlineTextHostVisibilityChanged;
		if (GodotObject.IsInstanceValid(_viewport))
		{
			_viewport.VisibilityChanged -= OnInlineTextViewportVisibilityChanged;
		}
		if (GodotObject.IsInstanceValid(_inlineTextOverlay))
		{
			_inlineTextOverlay.QueueFree();
		}
		_inlineTextOverlay = null;
		_inlineTextModeLabel = null;
		_inlineTextLineEdit = null;
		_inlineTextMultilineEdit = null;
		_inlineTextCommitButton = null;
		_inlineTextCancelButton = null;
		_inlineTextOverlayMotion = null;
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (!InlineTextEditorVisible || !(inputEvent is InputEventKey { Pressed: not false, Echo: false } inputEventKey))
		{
			return;
		}
		Control control = GetViewport()?.GuiGetFocusOwner();
		Control control2 = (_inlineTextMultiline ? ((Control)_inlineTextMultilineEdit) : ((Control)_inlineTextLineEdit));
		if (control != control2)
		{
			return;
		}
		if (inputEventKey.Keycode == Key.Escape)
		{
			if (!CancelPendingInlineTextIme())
			{
				CancelInlineTextEdit();
			}
			GetViewport()?.SetInputAsHandled();
			return;
		}
		Key keycode = inputEventKey.Keycode;
		bool flag = (((ulong)(keycode - 4194309) <= 1uL) ? true : false);
		if (flag && (!_inlineTextMultiline || inputEventKey.CtrlPressed || inputEventKey.MetaPressed))
		{
			CommitInlineTextEdit();
			GetViewport()?.SetInputAsHandled();
		}
	}

	public bool CanInlineEditText(Node candidate)
	{
		if (!GodotObject.IsInstanceValid(candidate))
		{
			return false;
		}
		ulong instanceId = candidate.GetInstanceId();
		if (_inlineTextCapabilityCache.TryGetValue(instanceId, out var value))
		{
			return value;
		}
		bool flag = TryResolveInlineTextTarget(candidate, out var _, out var _, out var _, out var _, out var _, out var _);
		_inlineTextCapabilityCache[instanceId] = flag;
		return flag;
	}

	public void ClearInlineTextCapabilityCache()
	{
		_inlineTextCapabilityCache.Clear();
	}

	public bool BeginInlineTextEdit(Node candidate)
	{
		if (_openTabs.TryGetValue(_currentTabKey, out var value) && value.ScriptlessPreview)
		{
			XWEditorInterface.Instance?.ShowToast("当前是外部 C# 场景的无脚本预览；请先完成后台编译，再直接编辑画面文字。");
			return false;
		}
		if (!TryResolveInlineTextTarget(candidate, out var target, out var property, out var valueType, out var hint, out var text, out var multiline))
		{
			return false;
		}
		if (InlineTextEditorVisible && _inlineTextTarget == target && _inlineTextProperty == property)
		{
			RefreshInlineTextEditorGeometry();
			QueueInlineTextEditorFocus(selectAll: false);
			return true;
		}
		CommitInlineTextEdit();
		_inlineTextTarget = target;
		_inlineTextProperty = property;
		_inlineTextValueType = valueType;
		_inlineTextHint = hint;
		_inlineTextBefore = text;
		_inlineTextHistoryId = GetCurrentSceneHistoryId();
		_inlineTextMultiline = multiline;
		_inlineTextSessionToken++;
		_inlineTextApplyingPreview = true;
		_inlineTextLineEdit.Text = text;
		_inlineTextMultilineEdit.Text = text;
		_inlineTextApplyingPreview = false;
		_inlineTextLineEdit.Visible = !multiline;
		_inlineTextMultilineEdit.Visible = multiline;
		ConfigureInlineTextInputForTarget(target, property, hint);
		_inlineTextModeLabel.Text = BuildInlineTextModeLabel(target, property, multiline);
		_inlineTextOverlay.Visible = true;
		if (_inlineTextOverlay.GetThemeStylebox("panel") is StyleBoxFlat styleBoxFlat)
		{
			styleBoxFlat.ShadowSize = ((!XWUiMotion.LowPerformanceMode) ? 8 : 0);
		}
		XWEditorInterface.Instance?.SetSelectedNode(target);
		XWEditorInterface.Instance?.GetSceneTreeDock()?.SelectNode(target, emitSignal: false);
		RefreshInlineTextEditorGeometry();
		QueueInlineTextEditorFocus(selectAll: true);
		_inlineTextOverlayMotion?.PlayPanelReveal(0f);
		_viewport?.QueueEditorOverlayRedraw();
		return true;
	}

	public void SetInlineTextDraft(string text)
	{
		if (InlineTextEditorVisible)
		{
			string text2 = text ?? "";
			_inlineTextApplyingPreview = true;
			if (_inlineTextMultiline)
			{
				_inlineTextMultilineEdit.Text = text2;
			}
			else
			{
				_inlineTextLineEdit.Text = text2;
			}
			_inlineTextApplyingPreview = false;
			ApplyInlineTextPreview(text2);
		}
	}

	public bool CommitPendingAuthoringInput()
	{
		return CommitInlineTextEdit();
	}

	public bool CommitInlineTextEdit()
	{
		if (!InlineTextEditorVisible || !GodotObject.IsInstanceValid(_inlineTextTarget))
		{
			HideInlineTextEditor();
			return false;
		}
		ApplyPendingInlineTextIme();
		Node from = _inlineTextTarget;
		StringName inlineTextProperty = _inlineTextProperty;
		Variant.Type inlineTextValueType = _inlineTextValueType;
		string inlineTextBefore = _inlineTextBefore;
		string inlineTextDraft = InlineTextDraft;
		int inlineTextHistoryId = _inlineTextHistoryId;
		ApplyInlineTextValue(from, inlineTextProperty, inlineTextValueType, inlineTextDraft);
		HideInlineTextEditor();
		if (string.Equals(inlineTextBefore, inlineTextDraft, StringComparison.Ordinal))
		{
			NotifyInlineTextHistoryApplied(from);
			return false;
		}
		PrepareSceneSurfaceHistoryMutation();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (!GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			InlineTextCommitCount++;
			NotifyInlineTextHistoryApplied(from);
			return true;
		}
		xWUndoRedoManager.CreateAction($"编辑画面文字 · {from.Name}", mergeMode: false, inlineTextHistoryId);
		xWUndoRedoManager.AddUndoMethod(this, "NotifyInlineTextHistoryApplied", Variant.From(in from));
		xWUndoRedoManager.AddDoProperty(from, inlineTextProperty, MakeInlineTextVariant(inlineTextValueType, inlineTextDraft));
		xWUndoRedoManager.AddUndoProperty(from, inlineTextProperty, MakeInlineTextVariant(inlineTextValueType, inlineTextBefore));
		xWUndoRedoManager.AddDoMethod(this, "NotifyInlineTextHistoryApplied", Variant.From(in from));
		xWUndoRedoManager.CommitAction();
		InlineTextCommitCount++;
		_viewport?.ActivateSceneTransformHistoryDeferred();
		return true;
	}

	public void CancelInlineTextEdit()
	{
		if (!InlineTextEditorVisible)
		{
			HideInlineTextEditor();
			return;
		}
		Node inlineTextTarget = _inlineTextTarget;
		if (GodotObject.IsInstanceValid(inlineTextTarget))
		{
			ApplyInlineTextValue(inlineTextTarget, _inlineTextProperty, _inlineTextValueType, _inlineTextBefore);
		}
		InlineTextCancelCount++;
		HideInlineTextEditor();
		if (GodotObject.IsInstanceValid(inlineTextTarget))
		{
			NotifyInlineTextHistoryApplied(inlineTextTarget);
		}
	}

	public void RefreshInlineTextEditorGeometry()
	{
		_inlineTextGeometryRefreshQueued = false;
		if (InlineTextEditorVisible && GodotObject.IsInstanceValid(_viewport) && _inlineTextTarget is CanvasItem canvasItem && GodotObject.IsInstanceValid(canvasItem))
		{
			Rect2 canvasItemViewportBounds = _viewport.GetCanvasItemViewportBounds(canvasItem);
			Vector2 size = _viewport.Size;
			float num = (_inlineTextMultiline ? Mathf.Max(82f, canvasItemViewportBounds.Size.Y) : Mathf.Max(36f, canvasItemViewportBounds.Size.Y));
			Vector2 vector = new Vector2(Mathf.Clamp(Mathf.Max(190f, canvasItemViewportBounds.Size.X + 16f), 190f, Mathf.Max(190f, size.X - 12f)), Mathf.Clamp(num + 38f, 70f, Mathf.Max(70f, size.Y - 12f)));
			Vector2 vector2 = canvasItemViewportBounds.Position + new Vector2(-8f, -36f);
			vector2.X = Mathf.Clamp(vector2.X, 6f, Mathf.Max(6f, size.X - vector.X - 6f));
			vector2.Y = Mathf.Clamp(vector2.Y, 6f, Mathf.Max(6f, size.Y - vector.Y - 6f));
			_inlineTextOverlay.Position = vector2.Round();
			_inlineTextOverlay.Size = vector.Round();
		}
	}

	public void NotifyInlineTextHistoryApplied(Node target)
	{
		if (GodotObject.IsInstanceValid(target))
		{
			XWEditorInterface.Instance?.SetSelectedNode(target);
			XWEditorInterface.Instance?.GetSceneTreeDock()?.SelectNode(target, emitSignal: false);
		}
		MarkCurrentSceneHistorySurfaceDirty();
		_nodeDirectPropertySurface?.RefreshValues();
		if (target is CanvasItem canvasItem)
		{
			canvasItem.QueueRedraw();
		}
		_viewport?.QueueEditorOverlayRedraw();
	}

	private bool CommitInlineTextEditForScene(Node sceneRoot)
	{
		if (!InlineTextEditorVisible || !GodotObject.IsInstanceValid(_inlineTextTarget))
		{
			return false;
		}
		if (!GodotObject.IsInstanceValid(sceneRoot) || (sceneRoot != _inlineTextTarget && !sceneRoot.IsAncestorOf(_inlineTextTarget)))
		{
			return false;
		}
		return CommitInlineTextEdit();
	}

	private void OnInlineTextLineChanged(string text)
	{
		if (!_inlineTextApplyingPreview)
		{
			ApplyInlineTextPreview(text);
		}
	}

	private void OnInlineTextMultilineChanged()
	{
		if (!_inlineTextApplyingPreview)
		{
			ApplyInlineTextPreview(_inlineTextMultilineEdit.Text);
		}
	}

	private void ApplyInlineTextPreview(string text)
	{
		if (InlineTextEditorVisible && GodotObject.IsInstanceValid(_inlineTextTarget))
		{
			ApplyInlineTextValue(_inlineTextTarget, _inlineTextProperty, _inlineTextValueType, text ?? "");
			InlineTextPreviewCount++;
			QueueInlineTextGeometryRefresh();
		}
	}

	private static void ApplyInlineTextValue(Node target, StringName property, Variant.Type valueType, string text)
	{
		if (GodotObject.IsInstanceValid(target))
		{
			target.Set(property, MakeInlineTextVariant(valueType, text));
			if (target is CanvasItem canvasItem)
			{
				canvasItem.QueueRedraw();
			}
		}
	}

	private static Variant MakeInlineTextVariant(Variant.Type valueType, string text)
	{
		string from = text ?? "";
		if (valueType != Variant.Type.StringName)
		{
			return Variant.From(in from);
		}
		return Variant.From<StringName>(new StringName(from));
	}

	private void QueueInlineTextFocusExitCommit()
	{
		int token = _inlineTextSessionToken;
		Callable.From(() =>
		{
			if (token == _inlineTextSessionToken && InlineTextEditorVisible)
			{
				CommitInlineTextEdit();
			}
		}).CallDeferred();
	}

	private void OnInlineTextEditorGuiInput(InputEvent inputEvent)
	{
		if (!(inputEvent is InputEventKey { Pressed: not false, Echo: false } inputEventKey))
		{
			return;
		}
		if (inputEventKey.Keycode == Key.Escape)
		{
			if (CancelPendingInlineTextIme())
			{
				GetViewport()?.SetInputAsHandled();
				return;
			}
			CancelInlineTextEdit();
			GetViewport()?.SetInputAsHandled();
			return;
		}
		bool flag = !_inlineTextMultiline;
		if (flag)
		{
			Key keycode = inputEventKey.Keycode;
			bool flag2 = (((ulong)(keycode - 4194309) <= 1uL) ? true : false);
			flag = flag2;
		}
		if (flag)
		{
			CommitInlineTextEdit();
			GetViewport()?.SetInputAsHandled();
			return;
		}
		flag = _inlineTextMultiline;
		if (flag)
		{
			Key keycode = inputEventKey.Keycode;
			bool flag2 = (((ulong)(keycode - 4194309) <= 1uL) ? true : false);
			flag = flag2;
		}
		if (flag && (inputEventKey.CtrlPressed || inputEventKey.MetaPressed))
		{
			CommitInlineTextEdit();
			GetViewport()?.SetInputAsHandled();
		}
	}

	private void OnInlineTextHostVisibilityChanged()
	{
		if (!IsVisibleInTree() && InlineTextEditorVisible)
		{
			CommitInlineTextEdit();
		}
	}

	private void OnInlineTextViewportVisibilityChanged()
	{
		if (GodotObject.IsInstanceValid(_viewport) && !_viewport.IsVisibleInTree() && InlineTextEditorVisible)
		{
			CommitInlineTextEdit();
		}
	}

	private void QueueInlineTextGeometryRefresh()
	{
		if (!_inlineTextGeometryRefreshQueued && InlineTextEditorVisible)
		{
			_inlineTextGeometryRefreshQueued = true;
			Callable.From(() =>
			{
				_inlineTextGeometryRefreshQueued = false;
				RefreshInlineTextEditorGeometry();
			}).CallDeferred();
		}
	}

	private void ConfigureInlineTextInputForTarget(Node target, StringName property, PropertyHint hint)
	{
		LineEdit lineEdit = target as LineEdit;
		bool flag = hint == PropertyHint.Password || (GodotObject.IsInstanceValid(lineEdit) && lineEdit.Secret && string.Equals(property.ToString(), "text", StringComparison.OrdinalIgnoreCase));
		_inlineTextLineEdit.Secret = flag;
		_inlineTextLineEdit.SecretCharacter = "•";
		if (flag && GodotObject.IsInstanceValid(lineEdit))
		{
			_inlineTextLineEdit.SecretCharacter = lineEdit.SecretCharacter;
		}
		Control control = (_inlineTextMultiline ? ((Control)_inlineTextMultilineEdit) : ((Control)_inlineTextLineEdit));
		if (GodotObject.IsInstanceValid(control) && target is Control control2)
		{
			control.RemoveThemeFontOverride("font");
			control.RemoveThemeFontSizeOverride("font_size");
			string text = ((target is RichTextLabel) ? "normal_font" : "font");
			string text2 = ((target is RichTextLabel) ? "normal_font_size" : "font_size");
			Font themeFont = control2.GetThemeFont(text);
			if (GodotObject.IsInstanceValid(themeFont))
			{
				control.AddThemeFontOverride("font", themeFont);
			}
			int fontSize = Mathf.Clamp(Mathf.RoundToInt((float)control2.GetThemeFontSize(text2) * Mathf.Max(_viewport?.Zoom ?? 1f, 0.1f)), 14, 28);
			control.AddThemeFontSizeOverride("font_size", fontSize);
			if (!_inlineTextMultiline)
			{
				LineEdit inlineTextLineEdit = _inlineTextLineEdit;
				HorizontalAlignment alignment;
				if (target is Label label)
				{
					alignment = label.HorizontalAlignment;
				}
				else if (target is Button button)
				{
					alignment = button.Alignment;
				}
				else
				{
					alignment = ((!(target is LineEdit lineEdit2)) ? HorizontalAlignment.Left : lineEdit2.Alignment);
				}
				inlineTextLineEdit.Alignment = alignment;
			}
		}
	}

	private void ApplyPendingInlineTextIme()
	{
		if (_inlineTextMultiline)
		{
			if (GodotObject.IsInstanceValid(_inlineTextMultilineEdit) && _inlineTextMultilineEdit.HasImeText())
			{
				_inlineTextMultilineEdit.ApplyIme();
			}
		}
		else if (GodotObject.IsInstanceValid(_inlineTextLineEdit) && _inlineTextLineEdit.HasImeText())
		{
			_inlineTextLineEdit.ApplyIme();
		}
	}

	private bool CancelPendingInlineTextIme()
	{
		if (_inlineTextMultiline)
		{
			if (!GodotObject.IsInstanceValid(_inlineTextMultilineEdit) || !_inlineTextMultilineEdit.HasImeText())
			{
				return false;
			}
			_inlineTextMultilineEdit.CancelIme();
			return true;
		}
		if (!GodotObject.IsInstanceValid(_inlineTextLineEdit) || !_inlineTextLineEdit.HasImeText())
		{
			return false;
		}
		_inlineTextLineEdit.CancelIme();
		return true;
	}

	private void GrabInlineTextEditorFocus(bool selectAll)
	{
		Control control = (_inlineTextMultiline ? ((Control)_inlineTextMultilineEdit) : ((Control)_inlineTextLineEdit));
		if (!GodotObject.IsInstanceValid(control))
		{
			return;
		}
		control.GrabFocus();
		if (selectAll)
		{
			if (control is LineEdit lineEdit)
			{
				lineEdit.SelectAll();
			}
			else if (control is TextEdit textEdit)
			{
				textEdit.SelectAll();
			}
		}
	}

	private void QueueInlineTextEditorFocus(bool selectAll)
	{
		int token = _inlineTextSessionToken;
		Callable.From(() =>
		{
			if (token == _inlineTextSessionToken && InlineTextEditorVisible)
			{
				GrabInlineTextEditorFocus(selectAll);
			}
		}).CallDeferred();
	}

	private void HideInlineTextEditor()
	{
		_inlineTextSessionToken++;
		if (GodotObject.IsInstanceValid(_inlineTextOverlay))
		{
			_inlineTextOverlay.Visible = false;
		}
		_inlineTextTarget = null;
		_inlineTextProperty = InlineTextPropertyFallback;
		_inlineTextValueType = Variant.Type.String;
		_inlineTextHint = PropertyHint.None;
		_inlineTextBefore = "";
		_inlineTextMultiline = false;
		_inlineTextApplyingPreview = false;
		_inlineTextGeometryRefreshQueued = false;
		if (GodotObject.IsInstanceValid(_viewport) && _viewport.IsInsideTree())
		{
			_viewport.GrabFocus();
		}
		_viewport?.QueueEditorOverlayRedraw();
	}

	private bool TryResolveInlineTextTarget(Node candidate, out Node target, out StringName property, out Variant.Type valueType, out PropertyHint hint, out string text, out bool multiline)
	{
		target = null;
		property = InlineTextPropertyFallback;
		valueType = Variant.Type.String;
		hint = PropertyHint.None;
		text = "";
		multiline = false;
		if (!GodotObject.IsInstanceValid(candidate) || !GodotObject.IsInstanceValid(CurrentSceneInstance))
		{
			return false;
		}
		Node node = candidate;
		while (GodotObject.IsInstanceValid(node))
		{
			if (node is CanvasItem && TryFindInlineTextProperty(node, out property, out valueType, out hint, out text))
			{
				target = node;
				bool flag = node is TextEdit || node is RichTextLabel || ((!(node is Label) && !(node is Button) && !(node is LinkButton) && !(node is LineEdit)) ? (hint == PropertyHint.MultilineText || text.Contains('\n')) : text.Contains('\n'));
				multiline = flag;
				return true;
			}
			if (node == CurrentSceneInstance)
			{
				break;
			}
			node = node.GetParent();
		}
		return false;
	}

	private static bool TryFindInlineTextProperty(Node node, out StringName property, out Variant.Type valueType, out PropertyHint hint, out string text)
	{
		property = InlineTextPropertyFallback;
		valueType = Variant.Type.String;
		hint = PropertyHint.None;
		text = "";
		if (node.HasMeta("_xw_inline_text_property"))
		{
			string requestedName = node.GetMeta("_xw_inline_text_property").AsString();
			if (TryReadInlineTextProperty(node, requestedName, out property, out valueType, out hint, out text))
			{
				return true;
			}
		}
		if (node is LineEdit lineEdit && string.IsNullOrEmpty(lineEdit.Text) && !string.IsNullOrEmpty(lineEdit.PlaceholderText) && TryReadInlineTextProperty(node, "placeholder_text", out property, out valueType, out hint, out text))
		{
			return true;
		}
		if (node is TextEdit textEdit && string.IsNullOrEmpty(textEdit.Text) && !string.IsNullOrEmpty(textEdit.PlaceholderText) && TryReadInlineTextProperty(node, "placeholder_text", out property, out valueType, out hint, out text))
		{
			return true;
		}
		string[] array = new string[10] { "text", "caption", "title", "content", "display_text", "button_text", "label_text", "dialogue", "description", "placeholder_text" };
		foreach (string requestedName2 in array)
		{
			if (TryReadInlineTextProperty(node, requestedName2, out property, out valueType, out hint, out text))
			{
				return true;
			}
		}
		return false;
	}

	private static bool TryReadInlineTextProperty(Node node, string requestedName, out StringName property, out Variant.Type valueType, out PropertyHint hint, out string text)
	{
		property = InlineTextPropertyFallback;
		valueType = Variant.Type.String;
		hint = PropertyHint.None;
		text = "";
		if (string.IsNullOrWhiteSpace(requestedName) || string.Equals(requestedName, "name", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		bool flag;
		foreach (Dictionary property2 in node.GetPropertyList())
		{
			if (!property2.ContainsKey("name") || !property2.ContainsKey("type"))
			{
				continue;
			}
			string text2 = property2["name"].AsString();
			if (!string.Equals(text2, requestedName, StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			Variant.Type type = (Variant.Type)property2["type"].AsInt32();
			flag = ((type == Variant.Type.String || type == Variant.Type.StringName) ? true : false);
			if (!flag)
			{
				flag = false;
			}
			else
			{
				PropertyUsageFlags propertyUsageFlags = (PropertyUsageFlags)(property2.ContainsKey("usage") ? property2["usage"].AsInt64() : 6);
				if ((propertyUsageFlags & PropertyUsageFlags.Editor) == PropertyUsageFlags.None || (propertyUsageFlags & PropertyUsageFlags.ReadOnly) != PropertyUsageFlags.None)
				{
					flag = false;
				}
				else
				{
					property = new StringName(text2);
					valueType = type;
					hint = (PropertyHint)(property2.ContainsKey("hint") ? property2["hint"].AsInt32() : 0);
					text = node.Get(property).AsString();
					flag = true;
				}
			}
			goto IL_018b;
		}
		return false;
		IL_018b:
		return flag;
	}

	private static string BuildInlineTextModeLabel(Node target, StringName property, bool multiline)
	{
		if (string.Equals(property.ToString(), "placeholder_text", StringComparison.OrdinalIgnoreCase))
		{
			return "输入占位文字 · 即时预览";
		}
		string text;
		if (target is RichTextLabel)
		{
			text = "富文本";
		}
		else if (target is TextEdit)
		{
			text = "多行输入";
		}
		else if (!(target is Button) && !(target is LinkButton))
		{
			if (target is LineEdit)
			{
				text = "输入框文字";
			}
			else if (!(target is Label))
			{
				text = (multiline ? "自定义多行文字" : "自定义画面文字");
			}
			else
			{
				text = (multiline ? "多行标签" : "标签文字");
			}
		}
		else
		{
			text = "按钮文字";
		}
		return text + " · 即时预览";
	}

	private Rect2 GetInlineTextControlRectInViewport(Control control)
	{
		if (!GodotObject.IsInstanceValid(control) || !GodotObject.IsInstanceValid(_viewport))
		{
			return default;
		}
		Transform2D globalTransformWithCanvas = _viewport.GetGlobalTransformWithCanvas();
		Transform2D globalTransformWithCanvas2 = control.GetGlobalTransformWithCanvas();
		if (Mathf.IsZeroApprox(globalTransformWithCanvas.Determinant()) || Mathf.IsZeroApprox(globalTransformWithCanvas2.Determinant()))
		{
			return default;
		}
		Transform2D transform2D = globalTransformWithCanvas.AffineInverse() * globalTransformWithCanvas2;
		Vector2 position = transform2D * Vector2.Zero;
		Vector2 to = transform2D * new Vector2(control.Size.X, 0f);
		Vector2 to2 = transform2D * control.Size;
		Vector2 to3 = transform2D * new Vector2(0f, control.Size.Y);
		return new Rect2(position, Vector2.Zero).Expand(to).Expand(to2).Expand(to3);
	}

	private static Button CreateInlineTextHeaderButton(string name, string iconName, string tooltip)
	{
		return new Button
		{
			Name = name,
			Icon = LoadInlineTextIcon(iconName),
			Flat = true,
			FocusMode = FocusModeEnum.None,
			CustomMinimumSize = new Vector2(27f, 27f),
			TooltipText = tooltip
		};
	}

	private static Texture2D LoadInlineTextIcon(string iconName)
	{
		string path = "res://addons/ModEditor/Icons/" + iconName + ".svg";
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		return ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse);
	}

	private static StyleBoxFlat CreateInlineTextOverlayStyle()
	{
		return new StyleBoxFlat
		{
			BgColor = new Color(0.035f, 0.09f, 0.052f, 0.98f),
			BorderColor = ModEditorTheme.SunGoldColor,
			BorderWidthLeft = 2,
			BorderWidthTop = 2,
			BorderWidthRight = 2,
			BorderWidthBottom = 2,
			CornerRadiusTopLeft = 10,
			CornerRadiusTopRight = 10,
			CornerRadiusBottomRight = 10,
			CornerRadiusBottomLeft = 10,
			ShadowColor = new Color(0f, 0f, 0f, 0.48f),
			ShadowSize = 8
		};
	}

	private void InitializeControlLayoutSurface()
	{
		_controlLayoutPanel = GetNode<PanelContainer>("%ControlLayoutPanel");
		_anchorLeftSpin = GetNode<SpinBox>("%AnchorLeftSpin");
		_anchorTopSpin = GetNode<SpinBox>("%AnchorTopSpin");
		_anchorRightSpin = GetNode<SpinBox>("%AnchorRightSpin");
		_anchorBottomSpin = GetNode<SpinBox>("%AnchorBottomSpin");
		_offsetLeftSpin = GetNode<SpinBox>("%OffsetLeftSpin");
		_offsetTopSpin = GetNode<SpinBox>("%OffsetTopSpin");
		_offsetRightSpin = GetNode<SpinBox>("%OffsetRightSpin");
		_offsetBottomSpin = GetNode<SpinBox>("%OffsetBottomSpin");
		_growHorizontalOption = GetNode<OptionButton>("%GrowHorizontalOption");
		_growVerticalOption = GetNode<OptionButton>("%GrowVerticalOption");
		_growHorizontalButtons = new Button[3]
		{
			GetNode<Button>("%GrowHorizontalBegin"),
			GetNode<Button>("%GrowHorizontalBoth"),
			GetNode<Button>("%GrowHorizontalEnd")
		};
		_growVerticalButtons = new Button[3]
		{
			GetNode<Button>("%GrowVerticalBegin"),
			GetNode<Button>("%GrowVerticalBoth"),
			GetNode<Button>("%GrowVerticalEnd")
		};
		_presetTopLeftButton = GetNode<Button>("%PresetTopLeftButton");
		_presetCenterButton = GetNode<Button>("%PresetCenterButton");
		_presetFullRectButton = GetNode<Button>("%PresetFullRectButton");
		_presetHCenterWideButton = GetNode<Button>("%PresetHCenterWideButton");
		_presetVCenterWideButton = GetNode<Button>("%PresetVCenterWideButton");
		AddGrowItems(_growHorizontalOption);
		AddGrowItems(_growVerticalOption);
		BindGrowDirectionButtons(_growHorizontalButtons, horizontal: true);
		BindGrowDirectionButtons(_growVerticalButtons, horizontal: false);
		SpinBox[] controlLayoutSpins = GetControlLayoutSpins();
		foreach (SpinBox spinBox in controlLayoutSpins)
		{
			SpinBox captured = spinBox;
			spinBox.ValueChanged += (double _) =>
			{
				OnControlLayoutSpinChanged(captured);
			};
			spinBox.GetLineEdit().FocusEntered += BeginControlLayoutSession;
			spinBox.GetLineEdit().FocusExited += CommitControlLayoutSession;
		}
		_growHorizontalOption.ItemSelected += (long index) =>
		{
			if (!_updatingLayoutSurface && GodotObject.IsInstanceValid(_layoutControl))
			{
				SetControlGrowWithHistory(_layoutControl, horizontal: true, GrowDirectionFromVisualIndex((int)index));
			}
		};
		_growVerticalOption.ItemSelected += (long index) =>
		{
			if (!_updatingLayoutSurface && GodotObject.IsInstanceValid(_layoutControl))
			{
				SetControlGrowWithHistory(_layoutControl, horizontal: false, GrowDirectionFromVisualIndex((int)index));
			}
		};
		_presetTopLeftButton.Pressed += () =>
		{
			ApplySelectedControlLayoutPresetWithHistory(LayoutPreset.TopLeft);
		};
		_presetCenterButton.Pressed += () =>
		{
			ApplySelectedControlLayoutPresetWithHistory(LayoutPreset.Center);
		};
		_presetFullRectButton.Pressed += () =>
		{
			ApplySelectedControlLayoutPresetWithHistory(LayoutPreset.FullRect);
		};
		_presetHCenterWideButton.Pressed += () =>
		{
			ApplySelectedControlLayoutPresetWithHistory(LayoutPreset.HcenterWide);
		};
		_presetVCenterWideButton.Pressed += () =>
		{
			ApplySelectedControlLayoutPresetWithHistory(LayoutPreset.VcenterWide);
		};
	}

	private static void AddGrowItems(OptionButton option)
	{
		option.Clear();
		option.AddItem("起点", 0);
		option.AddItem("两侧", 2);
		option.AddItem("终点", 1);
	}

	private static GrowDirection GrowDirectionFromVisualIndex(int index)
	{
		return index switch
		{
			1 => GrowDirection.Both, 
			2 => GrowDirection.End, 
			_ => GrowDirection.Begin, 
		};
	}

	private static int GetGrowDirectionVisualIndex(GrowDirection direction)
	{
		return direction switch
		{
			GrowDirection.Both => 1, 
			GrowDirection.End => 2, 
			_ => 0, 
		};
	}

	private void BindGrowDirectionButtons(Button[] buttons, bool horizontal)
	{
		ButtonGroup buttonGroup = new ButtonGroup
		{
			AllowUnpress = false
		};
		for (int i = 0; i < buttons.Length; i++)
		{
			Button obj = buttons[i];
			int directionIndex = i;
			obj.ButtonGroup = buttonGroup;
			obj.Pressed += () =>
			{
				if (!_updatingLayoutSurface && GodotObject.IsInstanceValid(_layoutControl))
				{
					GrowDirection direction = GrowDirectionFromVisualIndex(directionIndex);
					if (horizontal)
					{
						_growHorizontalOption.Select(directionIndex);
					}
					else
					{
						_growVerticalOption.Select(directionIndex);
					}
					SetControlGrowWithHistory(_layoutControl, horizontal, direction);
				}
			};
		}
	}

	private SpinBox[] GetControlLayoutSpins()
	{
		return new SpinBox[8] { _anchorLeftSpin, _anchorTopSpin, _anchorRightSpin, _anchorBottomSpin, _offsetLeftSpin, _offsetTopSpin, _offsetRightSpin, _offsetBottomSpin };
	}

	private void SetDirectSpinValue(SpinBox spin, List<DirectTransformState> states, Func<DirectTransformState, float> getter, bool available = true)
	{
		if (!_directSpinBaseSuffixes.ContainsKey(spin))
		{
			_directSpinBaseSuffixes[spin] = spin.Suffix;
		}
		if (!available || states.Count == 0)
		{
			spin.SetValueNoSignal(0.0);
			ClearDirectSpinMixedMarker(spin);
			return;
		}
		float num = getter(states[0]);
		bool flag = false;
		for (int i = 1; i < states.Count; i++)
		{
			if (!Mathf.IsEqualApprox(num, getter(states[i])))
			{
				flag = true;
				break;
			}
		}
		spin.SetValueNoSignal(num);
		if (flag)
		{
			_directMixedSpins.Add(spin);
			spin.Suffix = _directSpinBaseSuffixes[spin] + " •";
			spin.TooltipText = "所选节点包含混合值；输入后只批量修改此字段";
		}
		else
		{
			ClearDirectSpinMixedMarker(spin);
		}
	}

	private void ClearDirectSpinMixedMarker(SpinBox spin)
	{
		_directMixedSpins.Remove(spin);
		if (_directSpinBaseSuffixes.TryGetValue(spin, out var value))
		{
			spin.Suffix = value;
		}
		spin.TooltipText = "";
	}

	private void ApplyDirectSpinValue(Node node, SpinBox spin)
	{
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is Control control)
		{
			if (spin == _positionXSpin)
			{
				control.Position = new Vector2((float)spin.Value, control.Position.Y);
			}
			else if (spin == _positionYSpin)
			{
				control.Position = new Vector2(control.Position.X, (float)spin.Value);
			}
			else if (spin == _rotationSpin)
			{
				control.Rotation = Mathf.DegToRad((float)spin.Value);
			}
			else if (spin == _scaleXSpin)
			{
				control.Scale = new Vector2((float)spin.Value, control.Scale.Y);
			}
			else if (spin == _scaleYSpin)
			{
				control.Scale = new Vector2(control.Scale.X, (float)spin.Value);
			}
			else if (spin == _sizeXSpin)
			{
				control.Size = new Vector2((float)spin.Value, control.Size.Y);
			}
			else if (spin == _sizeYSpin)
			{
				control.Size = new Vector2(control.Size.X, (float)spin.Value);
			}
			else if (spin == _pivotXSpin)
			{
				control.PivotOffset = new Vector2((float)spin.Value, control.PivotOffset.Y);
			}
			else if (spin == _pivotYSpin)
			{
				control.PivotOffset = new Vector2(control.PivotOffset.X, (float)spin.Value);
			}
		}
		else if (node is Node2D node2D)
		{
			if (spin == _positionXSpin)
			{
				node2D.Position = new Vector2((float)spin.Value, node2D.Position.Y);
			}
			else if (spin == _positionYSpin)
			{
				node2D.Position = new Vector2(node2D.Position.X, (float)spin.Value);
			}
			else if (spin == _rotationSpin)
			{
				node2D.Rotation = Mathf.DegToRad((float)spin.Value);
			}
			else if (spin == _scaleXSpin)
			{
				node2D.Scale = new Vector2((float)spin.Value, node2D.Scale.Y);
			}
			else if (spin == _scaleYSpin)
			{
				node2D.Scale = new Vector2(node2D.Scale.X, (float)spin.Value);
			}
		}
	}

	public bool IsDirectTransformFieldMixed(string fieldName)
	{
		SpinBox spinBox = FindDirectTransformSpin(fieldName);
		if (spinBox != null)
		{
			return _directMixedSpins.Contains(spinBox);
		}
		return false;
	}

	public bool ApplySelectedTransformFieldWithHistory(string fieldName, double value)
	{
		UpdateDirectTransformSurface();
		SpinBox spinBox = FindDirectTransformSpin(fieldName);
		if (spinBox == null || !spinBox.Editable || _directTransformNodes.Count == 0)
		{
			return false;
		}
		BeginDirectTransformEditSession();
		spinBox.Value = value;
		CommitDirectTransformEditSession();
		return true;
	}

	private SpinBox FindDirectTransformSpin(string fieldName)
	{
		return fieldName?.ToLowerInvariant() switch
		{
			"position_x" => _positionXSpin, 
			"position_y" => _positionYSpin, 
			"rotation" => _rotationSpin, 
			"scale_x" => _scaleXSpin, 
			"scale_y" => _scaleYSpin, 
			"size_x" => _sizeXSpin, 
			"size_y" => _sizeYSpin, 
			"pivot_x" => _pivotXSpin, 
			"pivot_y" => _pivotYSpin, 
			_ => null, 
		};
	}

	private void UpdateControlLayoutSurface()
	{
		if (_layoutSessionActive)
		{
			CommitControlLayoutSession();
		}
		_layoutControl = ((_directTransformNodes.Count == 1) ? (_directTransformNodes[0] as Control) : null);
		bool flag = GodotObject.IsInstanceValid(_layoutControl);
		_controlLayoutPanel.Visible = flag;
		if (flag)
		{
			ControlLayoutState controlLayoutState = new ControlLayoutState(_layoutControl);
			_updatingLayoutSurface = true;
			_anchorLeftSpin.SetValueNoSignal(controlLayoutState.AnchorLeft);
			_anchorTopSpin.SetValueNoSignal(controlLayoutState.AnchorTop);
			_anchorRightSpin.SetValueNoSignal(controlLayoutState.AnchorRight);
			_anchorBottomSpin.SetValueNoSignal(controlLayoutState.AnchorBottom);
			_offsetLeftSpin.SetValueNoSignal(controlLayoutState.OffsetLeft);
			_offsetTopSpin.SetValueNoSignal(controlLayoutState.OffsetTop);
			_offsetRightSpin.SetValueNoSignal(controlLayoutState.OffsetRight);
			_offsetBottomSpin.SetValueNoSignal(controlLayoutState.OffsetBottom);
			_growHorizontalOption.Select(GetGrowDirectionVisualIndex(controlLayoutState.GrowHorizontal));
			_growVerticalOption.Select(GetGrowDirectionVisualIndex(controlLayoutState.GrowVertical));
			SelectGrowDirectionButton(_growHorizontalButtons, controlLayoutState.GrowHorizontal);
			SelectGrowDirectionButton(_growVerticalButtons, controlLayoutState.GrowVertical);
			_updatingLayoutSurface = false;
		}
	}

	private static void SelectGrowDirectionButton(Button[] buttons, GrowDirection direction)
	{
		int num = Mathf.Clamp(GetGrowDirectionVisualIndex(direction), 0, buttons.Length - 1);
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].SetPressedNoSignal(i == num);
		}
	}

	private void BeginControlLayoutSession()
	{
		if (!_updatingLayoutSurface && !_layoutSessionActive && GodotObject.IsInstanceValid(_layoutControl))
		{
			_layoutStart = new ControlLayoutState(_layoutControl);
			_layoutSessionActive = true;
		}
	}

	private void OnControlLayoutSpinChanged(SpinBox spin)
	{
		if (_updatingLayoutSurface || !GodotObject.IsInstanceValid(_layoutControl))
		{
			return;
		}
		if (!_layoutSessionActive)
		{
			BeginControlLayoutSession();
		}
		if (_layoutSessionActive)
		{
			if (spin == _anchorLeftSpin)
			{
				_layoutControl.AnchorLeft = (float)spin.Value;
			}
			else if (spin == _anchorTopSpin)
			{
				_layoutControl.AnchorTop = (float)spin.Value;
			}
			else if (spin == _anchorRightSpin)
			{
				_layoutControl.AnchorRight = (float)spin.Value;
			}
			else if (spin == _anchorBottomSpin)
			{
				_layoutControl.AnchorBottom = (float)spin.Value;
			}
			else if (spin == _offsetLeftSpin)
			{
				_layoutControl.OffsetLeft = (float)spin.Value;
			}
			else if (spin == _offsetTopSpin)
			{
				_layoutControl.OffsetTop = (float)spin.Value;
			}
			else if (spin == _offsetRightSpin)
			{
				_layoutControl.OffsetRight = (float)spin.Value;
			}
			else if (spin == _offsetBottomSpin)
			{
				_layoutControl.OffsetBottom = (float)spin.Value;
			}
			_viewport.QueueRedrawAll();
		}
	}

	private void CommitControlLayoutSession()
	{
		if (_layoutSessionActive)
		{
			_layoutSessionActive = false;
			if (GodotObject.IsInstanceValid(_layoutControl))
			{
				CommitControlLayoutHistory(after: new ControlLayoutState(_layoutControl), control: _layoutControl, before: _layoutStart, actionName: "编辑 Control 布局");
			}
		}
	}

	public bool BeginViewportControlLayoutSession(Control control)
	{
		if (!GodotObject.IsInstanceValid(control) || _layoutSessionActive)
		{
			return false;
		}
		_layoutControl = control;
		_layoutStart = new ControlLayoutState(control);
		_layoutSessionActive = true;
		return true;
	}

	public bool CommitViewportControlLayoutSession(string actionName)
	{
		if (!_layoutSessionActive || !GodotObject.IsInstanceValid(_layoutControl))
		{
			return false;
		}
		Control layoutControl = _layoutControl;
		ControlLayoutState layoutStart = _layoutStart;
		ControlLayoutState after = new ControlLayoutState(layoutControl);
		_layoutSessionActive = false;
		return CommitControlLayoutHistory(layoutControl, layoutStart, after, string.IsNullOrWhiteSpace(actionName) ? "拖动 Control 布局手柄" : actionName);
	}

	public void CancelViewportControlLayoutSession()
	{
		if (_layoutSessionActive)
		{
			_layoutSessionActive = false;
			if (GodotObject.IsInstanceValid(_layoutControl))
			{
				ApplyControlLayoutState(_layoutControl, _layoutStart);
				NotifyControlLayoutHistoryApplied(_layoutControl);
			}
		}
	}

	public bool EditSelectedControlLayoutFieldWithHistory(string fieldName, double value)
	{
		UpdateControlLayoutSurface();
		SpinBox spinBox = fieldName?.ToLowerInvariant() switch
		{
			"anchor_left" => _anchorLeftSpin, 
			"anchor_top" => _anchorTopSpin, 
			"anchor_right" => _anchorRightSpin, 
			"anchor_bottom" => _anchorBottomSpin, 
			"offset_left" => _offsetLeftSpin, 
			"offset_top" => _offsetTopSpin, 
			"offset_right" => _offsetRightSpin, 
			"offset_bottom" => _offsetBottomSpin, 
			_ => null, 
		};
		if (spinBox == null || !GodotObject.IsInstanceValid(_layoutControl))
		{
			return false;
		}
		BeginControlLayoutSession();
		spinBox.Value = value;
		CommitControlLayoutSession();
		return true;
	}

	public bool ApplySelectedControlLayoutPresetWithHistory(LayoutPreset preset)
	{
		UpdateControlLayoutSurface();
		return ApplyControlLayoutPresetWithHistory(_layoutControl, preset);
	}

	public bool ApplyControlLayoutPresetWithHistory(Control control, LayoutPreset preset)
	{
		if (!GodotObject.IsInstanceValid(control))
		{
			return false;
		}
		ControlLayoutState controlLayoutState = new ControlLayoutState(control);
		control.SetAnchorsAndOffsetsPreset(preset, LayoutPresetMode.Minsize);
		ControlLayoutState after = new ControlLayoutState(control);
		ApplyControlLayoutState(control, controlLayoutState);
		return CommitControlLayoutHistory(control, controlLayoutState, after, $"应用布局预设 {preset}");
	}

	public bool SetControlGrowWithHistory(Control control, bool horizontal, GrowDirection direction)
	{
		if (!GodotObject.IsInstanceValid(control))
		{
			return false;
		}
		ControlLayoutState controlLayoutState = new ControlLayoutState(control);
		if (horizontal)
		{
			control.GrowHorizontal = direction;
		}
		else
		{
			control.GrowVertical = direction;
		}
		ControlLayoutState after = new ControlLayoutState(control);
		ApplyControlLayoutState(control, controlLayoutState);
		return CommitControlLayoutHistory(control, controlLayoutState, after, horizontal ? "设置水平增长方向" : "设置垂直增长方向");
	}

	private bool CommitControlLayoutHistory(Control control, ControlLayoutState before, ControlLayoutState after, string actionName)
	{
		if (!HasControlLayoutChanged(before, after))
		{
			return false;
		}
		PrepareSceneSurfaceHistoryMutation();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (!GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			ApplyControlLayoutState(control, after);
			return true;
		}
		xWUndoRedoManager.CreateAction(actionName, mergeMode: false, GetCurrentSceneHistoryId());
		AddControlLayoutProperties(xWUndoRedoManager, control, before, after);
		xWUndoRedoManager.AddDoMethod(this, "NotifyControlLayoutHistoryApplied", Variant.From(in control));
		xWUndoRedoManager.AddUndoMethod(this, "NotifyControlLayoutHistoryApplied", Variant.From(in control));
		xWUndoRedoManager.CommitAction();
		_viewport.ActivateSceneTransformHistoryDeferred();
		return true;
	}

	private static void AddControlLayoutProperties(XWUndoRedoManager undoRedo, Control control, ControlLayoutState before, ControlLayoutState after)
	{
		AddHistoryProperty(undoRedo, control, "anchor_left", before.AnchorLeft, after.AnchorLeft);
		AddHistoryProperty(undoRedo, control, "anchor_top", before.AnchorTop, after.AnchorTop);
		AddHistoryProperty(undoRedo, control, "anchor_right", before.AnchorRight, after.AnchorRight);
		AddHistoryProperty(undoRedo, control, "anchor_bottom", before.AnchorBottom, after.AnchorBottom);
		AddHistoryProperty(undoRedo, control, "offset_left", before.OffsetLeft, after.OffsetLeft);
		AddHistoryProperty(undoRedo, control, "offset_top", before.OffsetTop, after.OffsetTop);
		AddHistoryProperty(undoRedo, control, "offset_right", before.OffsetRight, after.OffsetRight);
		AddHistoryProperty(undoRedo, control, "offset_bottom", before.OffsetBottom, after.OffsetBottom);
		undoRedo.AddDoProperty(control, "grow_horizontal", Variant.From<int>((int)after.GrowHorizontal));
		undoRedo.AddUndoProperty(control, "grow_horizontal", Variant.From<int>((int)before.GrowHorizontal));
		undoRedo.AddDoProperty(control, "grow_vertical", Variant.From<int>((int)after.GrowVertical));
		undoRedo.AddUndoProperty(control, "grow_vertical", Variant.From<int>((int)before.GrowVertical));
	}

	private static void AddHistoryProperty(XWUndoRedoManager undoRedo, GodotObject target, StringName property, float before, float after)
	{
		undoRedo.AddDoProperty(target, property, Variant.From(in after));
		undoRedo.AddUndoProperty(target, property, Variant.From(in before));
	}

	private static void ApplyControlLayoutState(Control control, ControlLayoutState state)
	{
		control.AnchorLeft = state.AnchorLeft;
		control.AnchorTop = state.AnchorTop;
		control.AnchorRight = state.AnchorRight;
		control.AnchorBottom = state.AnchorBottom;
		control.OffsetLeft = state.OffsetLeft;
		control.OffsetTop = state.OffsetTop;
		control.OffsetRight = state.OffsetRight;
		control.OffsetBottom = state.OffsetBottom;
		control.GrowHorizontal = state.GrowHorizontal;
		control.GrowVertical = state.GrowVertical;
	}

	private static bool HasControlLayoutChanged(ControlLayoutState before, ControlLayoutState after)
	{
		if (Mathf.IsEqualApprox(before.AnchorLeft, after.AnchorLeft) && Mathf.IsEqualApprox(before.AnchorTop, after.AnchorTop) && Mathf.IsEqualApprox(before.AnchorRight, after.AnchorRight) && Mathf.IsEqualApprox(before.AnchorBottom, after.AnchorBottom) && Mathf.IsEqualApprox(before.OffsetLeft, after.OffsetLeft) && Mathf.IsEqualApprox(before.OffsetTop, after.OffsetTop) && Mathf.IsEqualApprox(before.OffsetRight, after.OffsetRight) && Mathf.IsEqualApprox(before.OffsetBottom, after.OffsetBottom) && before.GrowHorizontal == after.GrowHorizontal)
		{
			return before.GrowVertical != after.GrowVertical;
		}
		return true;
	}

	public void NotifyControlLayoutHistoryApplied(Control control)
	{
		if (GodotObject.IsInstanceValid(control))
		{
			XWEditorInterface.Instance?.GetSceneTreeDock()?.SelectNode(control, emitSignal: false);
		}
		MarkCurrentSceneHistorySurfaceDirty();
		UpdateDirectTransformSurface();
		_viewport.QueueRedrawAll();
	}

	public bool SetSelectionLockWithHistory(bool enabled)
	{
		return CommitSelectionMetaHistory("_edit_lock_", enabled);
	}

	public bool SetSelectionGroupWithHistory(bool enabled)
	{
		return CommitSelectionMetaHistory("_edit_group_", enabled);
	}

	private bool CommitSelectionMetaHistory(string metaName, bool enabled)
	{
		List<Node> list = new List<Node>();
		foreach (Node item in XWEditorInterface.Instance?.GetEditorSelection()?.GetSelectedNodes() ?? new List<Node>())
		{
			if (item is CanvasItem && item.HasMeta(metaName) != enabled)
			{
				list.Add(item);
			}
		}
		if (list.Count == 0)
		{
			return false;
		}
		PrepareSceneSurfaceHistoryMutation();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (!GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			foreach (Node item2 in list)
			{
				ApplyNodeEditMeta(item2, metaName, enabled);
			}
			NotifySelectionMetaHistoryApplied(list[list.Count - 1]);
			return true;
		}
		string text = ((metaName == "_edit_lock_") ? "锁定" : "分组");
		string name = (enabled ? ("批量" + text + " 2D 节点") : ("批量取消" + text + " 2D 节点"));
		xWUndoRedoManager.CreateAction(name, mergeMode: false, GetCurrentSceneHistoryId());
		Variant[] array = new Variant[1];
		array[0] = Variant.From<Node>(list[list.Count - 1]);
		xWUndoRedoManager.AddUndoMethod(this, "NotifySelectionMetaHistoryApplied", array);
		foreach (Node item3 in list)
		{
			Node from = item3;
			xWUndoRedoManager.AddDoMethod(this, "ApplyNodeEditMeta", Variant.From(in from), Variant.From(in metaName), Variant.From(in enabled));
			xWUndoRedoManager.AddUndoMethod(this, "ApplyNodeEditMeta", Variant.From(in from), Variant.From(in metaName), Variant.From<bool>(!enabled));
		}
		Variant[] array2 = new Variant[1];
		array2[0] = Variant.From<Node>(list[list.Count - 1]);
		xWUndoRedoManager.AddDoMethod(this, "NotifySelectionMetaHistoryApplied", array2);
		xWUndoRedoManager.CommitAction();
		return true;
	}

	public void ApplyNodeEditMeta(Node node, string metaName, bool enabled)
	{
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (metaName == "_edit_lock_")
		{
			XWSceneNodeTree xWSceneNodeTree = XWEditorInterface.Instance?.GetSceneTreeDock()?.GetNodeOrNull<XWSceneNodeTree>("%SceneTree");
			if (GodotObject.IsInstanceValid(xWSceneNodeTree))
			{
				xWSceneNodeTree.ApplyNodeLockStorage(node, enabled);
				return;
			}
		}
		if (enabled)
		{
			node.SetMeta(metaName, true);
		}
		else if (node.HasMeta(metaName))
		{
			node.RemoveMeta(metaName);
		}
	}

	public void NotifySelectionMetaHistoryApplied(Node focus)
	{
		RefreshSceneTree();
		if (focus != null && GodotObject.IsInstanceValid(focus))
		{
			XWEditorInterface.Instance?.GetSceneTreeDock()?.SelectNode(focus, emitSignal: false);
		}
		MarkCurrentSceneHistorySurfaceDirty();
		UpdateSelectionActionButtons();
		UpdateDirectTransformSurface();
		_viewport.QueueRedrawAll();
	}

	private void PrepareSceneSurfaceHistoryMutation()
	{
		XWEditorInterface.Instance?.GetSceneTreeDock()?.PrepareForSceneHistoryBranchChange();
		PrepareForSceneHistoryBranchChange();
	}

	private void MarkCurrentSceneHistorySurfaceDirty()
	{
		CallDeferred("UpdateCurrentSceneDirtyMarker");
	}

	public void UpdateCurrentSceneDirtyMarker()
	{
		UpdateSceneDirtyMarker(_currentTabKey);
	}

	private void UpdateSceneDirtyMarker(string tabKey)
	{
		int num = _tabKeys.IndexOf(tabKey);
		if (num >= 0 && _openTabs.TryGetValue(tabKey, out var value))
		{
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			bool flag = GodotObject.IsInstanceValid(xWUndoRedoManager) && xWUndoRedoManager.IsHistoryUnsaved(value.HistoryId);
			_sceneTabBar.SetTabTitle(num, flag ? ("(*) " + value.DisplayName) : value.DisplayName);
		}
	}

	private void InitializeSelectionLayoutSurface()
	{
		_selectionLayoutPanel = GetNode<PanelContainer>("%SelectionLayoutPanel");
		_selectionLayoutLabel = GetNode<Label>("%SelectionLayoutLabel");
		Button node = GetNode<Button>("%AlignSelectionLeftButton");
		Button node2 = GetNode<Button>("%AlignSelectionHorizontalCenterButton");
		Button node3 = GetNode<Button>("%AlignSelectionRightButton");
		Button node4 = GetNode<Button>("%AlignSelectionTopButton");
		Button node5 = GetNode<Button>("%AlignSelectionVerticalCenterButton");
		Button node6 = GetNode<Button>("%AlignSelectionBottomButton");
		Button node7 = GetNode<Button>("%DistributeSelectionHorizontalButton");
		Button node8 = GetNode<Button>("%DistributeSelectionVerticalButton");
		_selectionAlignButtons = new Button[6] { node, node2, node3, node4, node5, node6 };
		_selectionDistributeButtons = new Button[2] { node7, node8 };
		node.Pressed += () =>
		{
			ApplySelectionLayoutWithHistory(SelectionLayoutOperation.AlignLeft);
		};
		node2.Pressed += () =>
		{
			ApplySelectionLayoutWithHistory(SelectionLayoutOperation.AlignHorizontalCenter);
		};
		node3.Pressed += () =>
		{
			ApplySelectionLayoutWithHistory(SelectionLayoutOperation.AlignRight);
		};
		node4.Pressed += () =>
		{
			ApplySelectionLayoutWithHistory(SelectionLayoutOperation.AlignTop);
		};
		node5.Pressed += () =>
		{
			ApplySelectionLayoutWithHistory(SelectionLayoutOperation.AlignVerticalCenter);
		};
		node6.Pressed += () =>
		{
			ApplySelectionLayoutWithHistory(SelectionLayoutOperation.AlignBottom);
		};
		node7.Pressed += () =>
		{
			ApplySelectionLayoutWithHistory(SelectionLayoutOperation.DistributeHorizontal);
		};
		node8.Pressed += () =>
		{
			ApplySelectionLayoutWithHistory(SelectionLayoutOperation.DistributeVertical);
		};
	}

	private void UpdateSelectionLayoutSurface()
	{
		CollectSelectionLayoutNodes();
		int count = _selectionLayoutNodes.Count;
		if (GodotObject.IsInstanceValid(_selectionLayoutPanel))
		{
			_selectionLayoutPanel.Visible = count >= 2;
		}
		if (GodotObject.IsInstanceValid(_selectionLayoutLabel))
		{
			_selectionLayoutLabel.Text = ((count >= 2) ? $"多选排版 · {count} 项" : "多选排版");
		}
		Button[] array = _selectionAlignButtons ?? System.Array.Empty<Button>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Disabled = count < 2;
		}
		array = _selectionDistributeButtons ?? System.Array.Empty<Button>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Disabled = count < 3;
		}
	}

	private void CollectSelectionLayoutNodes()
	{
		_selectionLayoutNodes.Clear();
		HashSet<Node> hashSet = new HashSet<Node>();
		foreach (Node directTransformNode in _directTransformNodes)
		{
			if (directTransformNode != null && GodotObject.IsInstanceValid(directTransformNode) && !directTransformNode.HasMeta("_edit_lock_") && directTransformNode is CanvasItem)
			{
				hashSet.Add(directTransformNode);
			}
		}
		foreach (Node directTransformNode2 in _directTransformNodes)
		{
			if (hashSet.Contains(directTransformNode2) && !HasSelectedTransformAncestor(directTransformNode2, hashSet))
			{
				_selectionLayoutNodes.Add(directTransformNode2);
			}
		}
	}

	private static bool HasSelectedTransformAncestor(Node node, HashSet<Node> selectedNodes)
	{
		Node node2 = node?.GetParent();
		while (node2 != null && GodotObject.IsInstanceValid(node2))
		{
			if (selectedNodes.Contains(node2))
			{
				return true;
			}
			node2 = node2.GetParent();
		}
		return false;
	}

	public bool ApplySelectionLayoutWithHistory(SelectionLayoutOperation operation)
	{
		if (!GodotObject.IsInstanceValid(_viewport))
		{
			return false;
		}
		CollectSelectionLayoutNodes();
		bool flag = (uint)(operation - 6) <= 1u;
		int num = (flag ? 3 : 2);
		if (_selectionLayoutNodes.Count < num)
		{
			return false;
		}
		List<SelectionLayoutEntry> list = new List<SelectionLayoutEntry>();
		foreach (Node selectionLayoutNode in _selectionLayoutNodes)
		{
			if (selectionLayoutNode is CanvasItem item && TryCaptureDirectTransformState(selectionLayoutNode, out var state))
			{
				list.Add(new SelectionLayoutEntry(selectionLayoutNode, state, _viewport.GetCanvasItemWorldBounds(item)));
			}
		}
		if (list.Count < num)
		{
			return false;
		}
		System.Collections.Generic.Dictionary<Node, Vector2> dictionary = BuildSelectionLayoutWorldDeltas(list, operation);
		List<(Node, DirectTransformState, DirectTransformState)> list2 = new List<(Node, DirectTransformState, DirectTransformState)>();
		foreach (SelectionLayoutEntry item2 in list)
		{
			if (dictionary.TryGetValue(item2.Node, out var value) && !value.IsEqualApprox(Vector2.Zero) && _viewport.TryResolveLocalPositionForWorldDelta(item2.Node, item2.State.Position, value, out var localTarget) && !localTarget.IsEqualApprox(item2.State.Position))
			{
				list2.Add(new ValueTuple<Node, DirectTransformState, DirectTransformState>(item3: new DirectTransformState(localTarget, item2.State.Scale, item2.State.Rotation, item2.State.Size, item2.State.Pivot, item2.State.IsControl), item1: item2.Node, item2: item2.State));
			}
		}
		if (list2.Count == 0)
		{
			return false;
		}
		PrepareSceneSurfaceHistoryMutation();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (!GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			return false;
		}
		xWUndoRedoManager.CreateAction(GetSelectionLayoutHistoryName(operation), mergeMode: false, GetCurrentSceneHistoryId());
		foreach (var item3 in list2)
		{
			AddDirectTransformHistoryProperties(xWUndoRedoManager, item3.Item1, item3.Item2, item3.Item3);
		}
		xWUndoRedoManager.AddDoMethod(this, "NotifyDirectTransformHistoryApplied");
		xWUndoRedoManager.AddUndoMethod(this, "NotifyDirectTransformHistoryApplied");
		xWUndoRedoManager.CommitAction();
		_viewport.ActivateSceneTransformHistoryDeferred();
		return true;
	}

	private static System.Collections.Generic.Dictionary<Node, Vector2> BuildSelectionLayoutWorldDeltas(List<SelectionLayoutEntry> entries, SelectionLayoutOperation operation)
	{
		System.Collections.Generic.Dictionary<Node, Vector2> dictionary = new System.Collections.Generic.Dictionary<Node, Vector2>();
		Rect2 rect = entries[0].WorldBounds;
		for (int i = 1; i < entries.Count; i++)
		{
			rect = rect.Merge(entries[i].WorldBounds);
		}
		if ((uint)(operation - 6) <= 1u)
		{
			bool horizontal = operation == SelectionLayoutOperation.DistributeHorizontal;
			entries.Sort((SelectionLayoutEntry left, SelectionLayoutEntry right) => GetAxisStart(left.WorldBounds, horizontal).CompareTo(GetAxisStart(right.WorldBounds, horizontal)));
			float num = 0f;
			foreach (SelectionLayoutEntry entry in entries)
			{
				num += GetAxisSize(entry.WorldBounds, horizontal);
			}
			float axisStart = GetAxisStart(entries[0].WorldBounds, horizontal);
			float num2 = (GetAxisEnd(entries[entries.Count - 1].WorldBounds, horizontal) - axisStart - num) / (float)(entries.Count - 1);
			float num3 = axisStart;
			for (int num4 = 0; num4 < entries.Count; num4++)
			{
				SelectionLayoutEntry selectionLayoutEntry = entries[num4];
				float num5 = ((num4 == 0 || num4 == entries.Count - 1) ? 0f : (num3 - GetAxisStart(selectionLayoutEntry.WorldBounds, horizontal)));
				dictionary[selectionLayoutEntry.Node] = (horizontal ? new Vector2(num5, 0f) : new Vector2(0f, num5));
				num3 += GetAxisSize(selectionLayoutEntry.WorldBounds, horizontal) + num2;
			}
			return dictionary;
		}
		float num6 = operation switch
		{
			SelectionLayoutOperation.AlignLeft => rect.Position.X, 
			SelectionLayoutOperation.AlignHorizontalCenter => rect.Position.X + rect.Size.X * 0.5f, 
			SelectionLayoutOperation.AlignRight => rect.Position.X + rect.Size.X, 
			SelectionLayoutOperation.AlignTop => rect.Position.Y, 
			SelectionLayoutOperation.AlignVerticalCenter => rect.Position.Y + rect.Size.Y * 0.5f, 
			SelectionLayoutOperation.AlignBottom => rect.Position.Y + rect.Size.Y, 
			_ => 0f, 
		};
		foreach (SelectionLayoutEntry entry2 in entries)
		{
			Rect2 worldBounds = entry2.WorldBounds;
			Vector2 value = operation switch
			{
				SelectionLayoutOperation.AlignLeft => new Vector2(num6 - worldBounds.Position.X, 0f), 
				SelectionLayoutOperation.AlignHorizontalCenter => new Vector2(num6 - worldBounds.Position.X - worldBounds.Size.X * 0.5f, 0f), 
				SelectionLayoutOperation.AlignRight => new Vector2(num6 - worldBounds.Position.X - worldBounds.Size.X, 0f), 
				SelectionLayoutOperation.AlignTop => new Vector2(0f, num6 - worldBounds.Position.Y), 
				SelectionLayoutOperation.AlignVerticalCenter => new Vector2(0f, num6 - worldBounds.Position.Y - worldBounds.Size.Y * 0.5f), 
				SelectionLayoutOperation.AlignBottom => new Vector2(0f, num6 - worldBounds.Position.Y - worldBounds.Size.Y), 
				_ => Vector2.Zero, 
			};
			dictionary[entry2.Node] = value;
		}
		return dictionary;
	}

	private static float GetAxisStart(Rect2 rect, bool horizontal)
	{
		if (!horizontal)
		{
			return rect.Position.Y;
		}
		return rect.Position.X;
	}

	private static float GetAxisSize(Rect2 rect, bool horizontal)
	{
		if (!horizontal)
		{
			return rect.Size.Y;
		}
		return rect.Size.X;
	}

	private static float GetAxisEnd(Rect2 rect, bool horizontal)
	{
		return GetAxisStart(rect, horizontal) + GetAxisSize(rect, horizontal);
	}

	private static string GetSelectionLayoutHistoryName(SelectionLayoutOperation operation)
	{
		return operation switch
		{
			SelectionLayoutOperation.AlignLeft => "左对齐 2D 节点", 
			SelectionLayoutOperation.AlignHorizontalCenter => "水平居中对齐 2D 节点", 
			SelectionLayoutOperation.AlignRight => "右对齐 2D 节点", 
			SelectionLayoutOperation.AlignTop => "顶部对齐 2D 节点", 
			SelectionLayoutOperation.AlignVerticalCenter => "垂直居中对齐 2D 节点", 
			SelectionLayoutOperation.AlignBottom => "底部对齐 2D 节点", 
			SelectionLayoutOperation.DistributeHorizontal => "水平等距分布 2D 节点", 
			SelectionLayoutOperation.DistributeVertical => "垂直等距分布 2D 节点", 
			_ => "排版 2D 节点", 
		};
	}

	private void InitializeSpriteAtlasSurface()
	{
		_spriteAtlasPanel = new XW2DSpriteAtlasPanel
		{
			Name = "SpriteAtlasWorkbench",
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		_workspaceTabs.AddChild(_spriteAtlasPanel, forceReadableName: false, InternalMode.Disabled);
		_spriteAtlasTabIndex = _workspaceTabs.GetTabCount() - 1;
		_workspaceTabs.SetTabTitle(_spriteAtlasTabIndex, "图集切片");
		_workspaceTabs.SetTabIcon(_spriteAtlasTabIndex, XWClassRegistry.Instance.GetUIIcon("AtlasTexture"));
		_spriteAtlasPanel.BindEditor(this);
		_spriteAtlasButton = new Button
		{
			Name = "SpriteAtlasButton",
			Text = "图集",
			TooltipText = "打开 Sprite2D 可视化图集切片台",
			Flat = true,
			Visible = false,
			CustomMinimumSize = new Vector2(56f, 28f),
			Icon = XWClassRegistry.Instance.GetUIIcon("AtlasTexture")
		};
		Control control = _animationTimelineButton?.GetParent() as Control;
		control?.AddChild(_spriteAtlasButton, forceReadableName: false, InternalMode.Disabled);
		if (GodotObject.IsInstanceValid(control))
		{
			int index = _animationTimelineButton.GetIndex();
			control.MoveChild(_spriteAtlasButton, index + 1);
		}
		_spriteAtlasButton.Pressed += OpenSelectedSpriteAtlas;
		XWEditorSelection xWEditorSelection = XWEditorInterface.Instance?.GetEditorSelection();
		if (xWEditorSelection != null)
		{
			xWEditorSelection.SelectionChanged += OnSpriteAtlasSelectionChanged;
			_spriteAtlasSelectionConnected = true;
		}
		OnSpriteAtlasSelectionChanged();
	}

	private void ShutdownSpriteAtlasSurface()
	{
		if (_spriteAtlasSelectionConnected)
		{
			XWEditorSelection xWEditorSelection = XWEditorInterface.Instance?.GetEditorSelection();
			if (xWEditorSelection != null)
			{
				xWEditorSelection.SelectionChanged -= OnSpriteAtlasSelectionChanged;
			}
			_spriteAtlasSelectionConnected = false;
		}
		if (GodotObject.IsInstanceValid(_spriteAtlasButton))
		{
			_spriteAtlasButton.Pressed -= OpenSelectedSpriteAtlas;
		}
		_spriteAtlasPanel?.BindSprite(null);
	}

	private void OnSpriteAtlasSelectionChanged()
	{
		Sprite2D sprite2D = ResolveSingleSelectedSprite();
		_spriteAtlasPanel?.BindSprite(sprite2D);
		if (GodotObject.IsInstanceValid(_spriteAtlasButton))
		{
			_spriteAtlasButton.Visible = GodotObject.IsInstanceValid(sprite2D);
		}
	}

	private Sprite2D ResolveSingleSelectedSprite()
	{
		Sprite2D sprite2D = null;
		foreach (Node item in XWEditorInterface.Instance?.GetEditorSelection()?.GetSelectedNodes() ?? new List<Node>())
		{
			if (item is Sprite2D sprite2D2)
			{
				if (GodotObject.IsInstanceValid(sprite2D))
				{
					return null;
				}
				sprite2D = sprite2D2;
			}
		}
		return sprite2D;
	}

	private void OpenSelectedSpriteAtlas()
	{
		if (_spriteAtlasTabIndex >= 0 && GodotObject.IsInstanceValid(_spriteAtlasPanel))
		{
			_workspaceTabs.CurrentTab = _spriteAtlasTabIndex;
			_spriteAtlasPanel.RefreshFromSprite();
		}
	}

	public bool OpenSpriteAtlasForSelection()
	{
		Sprite2D sprite2D = ResolveSingleSelectedSprite();
		if (!GodotObject.IsInstanceValid(sprite2D))
		{
			return false;
		}
		_spriteAtlasPanel.BindSprite(sprite2D);
		OpenSelectedSpriteAtlas();
		return _workspaceTabs.CurrentTab == _spriteAtlasTabIndex;
	}

	public bool CommitSpriteAtlasStateWithHistory(Sprite2D sprite, int columns, int rows, int frame, bool regionEnabled, Rect2 regionRect, string actionName)
	{
		if (!GodotObject.IsInstanceValid(sprite) || !IsNodeInCurrentScene(sprite))
		{
			return false;
		}
		SpriteAtlasState left = new SpriteAtlasState(sprite);
		SpriteAtlasState right = new SpriteAtlasState(columns, rows, frame, regionEnabled, regionRect);
		if (SpriteAtlasStatesEqual(left, right))
		{
			return true;
		}
		PrepareSceneSurfaceHistoryMutation();
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (!GodotObject.IsInstanceValid(xWUndoRedoManager))
		{
			ApplySpriteAtlasState(sprite, right.Columns, right.Rows, right.Frame, right.RegionEnabled, right.RegionRect);
			NotifySpriteAtlasHistoryApplied(sprite);
			return true;
		}
		string text = (string.IsNullOrWhiteSpace(actionName) ? "编辑精灵图集" : actionName);
		bool mergeMode = text == "选择精灵图集帧";
		xWUndoRedoManager.CreateAction(text, mergeMode, GetCurrentSceneHistoryId());
		xWUndoRedoManager.AddDoMethod(this, "ApplySpriteAtlasState", Variant.From(in sprite), Variant.From(in right.Columns), Variant.From(in right.Rows), Variant.From(in right.Frame), Variant.From(in right.RegionEnabled), Variant.From(in right.RegionRect));
		xWUndoRedoManager.AddUndoMethod(this, "NotifySpriteAtlasHistoryApplied", Variant.From(in sprite));
		xWUndoRedoManager.AddUndoMethod(this, "ApplySpriteAtlasState", Variant.From(in sprite), Variant.From(in left.Columns), Variant.From(in left.Rows), Variant.From(in left.Frame), Variant.From(in left.RegionEnabled), Variant.From(in left.RegionRect));
		xWUndoRedoManager.AddDoMethod(this, "NotifySpriteAtlasHistoryApplied", Variant.From(in sprite));
		xWUndoRedoManager.CommitAction();
		return true;
	}

	public void ApplySpriteAtlasState(Sprite2D sprite, int columns, int rows, int frame, bool regionEnabled, Rect2 regionRect)
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			int num = Math.Clamp(columns, 1, 256);
			int num2 = Math.Clamp(rows, 1, 256);
			sprite.Frame = 0;
			sprite.Hframes = num;
			sprite.Vframes = num2;
			sprite.Frame = Math.Clamp(frame, 0, num * num2 - 1);
			sprite.RegionRect = regionRect;
			sprite.RegionEnabled = regionEnabled;
		}
	}

	public void NotifySpriteAtlasHistoryApplied(Sprite2D sprite)
	{
		if (GodotObject.IsInstanceValid(sprite))
		{
			XWSceneTreeDock xWSceneTreeDock = XWEditorInterface.Instance?.GetSceneTreeDock();
			if (xWSceneTreeDock?.GetSelectedNode() != sprite)
			{
				xWSceneTreeDock?.SelectNode(sprite, emitSignal: false);
			}
			XWEditorSelection xWEditorSelection = XWEditorInterface.Instance?.GetEditorSelection();
			if (xWEditorSelection != null && !xWEditorSelection.IsSelected(sprite))
			{
				XWEditorInterface.Instance?.SetSelectedNode(sprite);
			}
			_spriteAtlasPanel?.BindSprite(sprite);
		}
		QueueViewportRedraw();
	}

	private bool IsNodeInCurrentScene(Node node)
	{
		if (!GodotObject.IsInstanceValid(node) || !GodotObject.IsInstanceValid(CurrentSceneInstance))
		{
			return false;
		}
		Node node2 = node;
		while (GodotObject.IsInstanceValid(node2))
		{
			if (node2 == CurrentSceneInstance)
			{
				return true;
			}
			node2 = node2.GetParent();
		}
		return false;
	}

	private static bool SpriteAtlasStatesEqual(SpriteAtlasState left, SpriteAtlasState right)
	{
		if (left.Columns == right.Columns && left.Rows == right.Rows && left.Frame == right.Frame && left.RegionEnabled == right.RegionEnabled)
		{
			return left.RegionRect.IsEqualApprox(right.RegionRect);
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(192)
		{
			new MethodInfo(MethodName.InitializeSceneAnimationSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShutdownSceneAnimationSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetSceneAnimationTimelineVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSceneAnimationSelectionChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebindSceneAnimationRoot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sceneRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SuspendSceneAnimationPreviewForSave, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResumeSceneAnimationPreviewAfterSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "resume", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshSceneAnimationAfterHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshSceneAnimationAfterHistoryIfVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifySceneAnimationHistoryApplied, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadPackedScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packedScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.String, "sourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadPackedSceneFromPath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveCurrentScene, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveAllScenes, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "showToast", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryInstantiateFullScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packedScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.String, "sourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AreAllExternalCSharpScriptsRegistered, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadScriptlessPreviewPackedScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeScriptResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "scriptResourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeComparablePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsModCharacterSceneText, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sceneText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsCharacterBaseSceneResourceLine, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "baseSceneId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRuntimeCharacterRootAssignment, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsRuntimeCharacterComponentPreviewLine, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsExternalCSharpScriptPath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadSceneResourceAttribute, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "attributeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshSceneTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildMenus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildViewportContextMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectDirectTransformSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetDirectTransformSpins, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateDirectTransformSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowDirectNodeProperties, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "focusTab", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshDirectNodePropertySurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveDirectPropertyTarget, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindDirectNodePropertyTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnDirectNodePropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.OnWorkspaceTabChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "tab", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginDirectTransformEditSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDirectTransformValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "changedSpin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.CommitDirectTransformEditSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifyDirectTransformHistoryApplied, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshDirectTransformSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DetachCurrentSceneRootForHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplySceneRootFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sceneRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SaveCurrentTabState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadTabData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tabKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SwitchToTab, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tabKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTabChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "tabIdx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnTabClosePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "tabIdx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentSceneHistoryId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSceneHistoryIdAt, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "tabIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSceneTabDirty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "tabIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RequestCloseSceneTab, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tabKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnUnsavedSceneCloseConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnUnsavedSceneCloseCanceled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDiscardUnsavedScenePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloseSceneTabImmediately, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tabKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSceneHistoryChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "historyId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenViewportContextMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "viewPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "worldPosition", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "hitNode", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.QueueViewportRedraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareForSceneHistoryBranchChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetToolMode, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSnapMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnViewMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnViewportContextMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSkeletonMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GenerateSkeletonFromSelectionWithHistory, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Skeleton2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearGeneratedSkeletonWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetBoneRelativeTransform, new PropertyInfo(Variant.Type.Transform2D, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sceneRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "source", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "parentSource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateGeneratedBoneGizmo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "bone", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Bone2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.FindSelectedGeneratedSkeleton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Skeleton2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindGeneratedSkeleton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Skeleton2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsGeneratedSkeleton, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "skeleton", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Skeleton2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetNodeDepth, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildUniqueChildName, new PropertyInfo(Variant.Type.StringName, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "preferredName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSelectionMetaWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "metaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateSelectionActionButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSnapButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncToolbarFromViewportState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSnapMenuChecks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateViewMenuChecks, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateViewportContextMenuState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateZoomLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateMousePositionLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSceneKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetSceneDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetToolButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMenuChecked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "popup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false),
				new PropertyInfo(Variant.Type.Int, "itemId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "checkedState", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMenuDisabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "popup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false),
				new PropertyInfo(Variant.Type.Int, "itemId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "disabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FreeSceneInstance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sceneInstance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeSceneGridSettings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShutdownSceneGridSettings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenGridSettingsPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SyncGridSettingsPanelFromViewport, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildGridPanelState, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnGridSettingsChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadPersistedGridSettings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PersistGridSettings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetGridSettingIfChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "settings", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.FlushGridSettingsSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterGridSettingDefaults, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "settings", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeInlineTextEditing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShutdownInlineTextEditing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Input, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.CanInlineEditText, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "candidate", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearInlineTextCapabilityCache, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginInlineTextEdit, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "candidate", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetInlineTextDraft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitPendingAuthoringInput, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitInlineTextEdit, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelInlineTextEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshInlineTextEditorGeometry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifyInlineTextHistoryApplied, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CommitInlineTextEditForScene, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sceneRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnInlineTextLineChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnInlineTextMultilineChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyInlineTextPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyInlineTextValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "valueType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MakeInlineTextVariant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "valueType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueInlineTextFocusExitCommit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnInlineTextEditorGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnInlineTextHostVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnInlineTextViewportVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueInlineTextGeometryRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureInlineTextInputForTarget, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "hint", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPendingInlineTextIme, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelPendingInlineTextIme, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GrabInlineTextEditorFocus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "selectAll", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueInlineTextEditorFocus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "selectAll", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HideInlineTextEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildInlineTextModeLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "multiline", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetInlineTextControlRectInViewport, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateInlineTextHeaderButton, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "iconName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadInlineTextIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "iconName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateInlineTextOverlayStyle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBoxFlat"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.InitializeControlLayoutSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddGrowItems, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false)
			}, null),
			new MethodInfo(MethodName.GrowDirectionFromVisualIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetGrowDirectionVisualIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindGrowDirectionButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "buttons", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "horizontal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetControlLayoutSpins, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearDirectSpinMixedMarker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyDirectSpinValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.Object, "spin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsDirectTransformFieldMixed, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fieldName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySelectedTransformFieldWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fieldName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindDirectTransformSpin, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fieldName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateControlLayoutSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectGrowDirectionButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "buttons", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginControlLayoutSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnControlLayoutSpinChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.CommitControlLayoutSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginViewportControlLayoutSession, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.CommitViewportControlLayoutSession, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CancelViewportControlLayoutSession, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EditSelectedControlLayoutFieldWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fieldName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySelectedControlLayoutPresetWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "preset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyControlLayoutPresetWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Int, "preset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetControlGrowWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "horizontal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddHistoryProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "undoRedo", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new PropertyInfo(Variant.Type.Object, "target", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "before", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "after", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NotifyControlLayoutHistoryApplied, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetSelectionLockWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSelectionGroupWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitSelectionMetaHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "metaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyNodeEditMeta, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "metaName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NotifySelectionMetaHistoryApplied, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "focus", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.PrepareSceneSurfaceHistoryMutation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkCurrentSceneHistorySurfaceDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCurrentSceneDirtyMarker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSceneDirtyMarker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tabKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeSelectionLayoutSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSelectionLayoutSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CollectSelectionLayoutNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplySelectionLayoutWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "operation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetAxisStart, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "horizontal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetAxisSize, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "horizontal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetAxisEnd, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "horizontal", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectionLayoutHistoryName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "operation", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeSpriteAtlasSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShutdownSpriteAtlasSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSpriteAtlasSelectionChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveSingleSelectedSprite, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Sprite2D"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenSelectedSpriteAtlas, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenSpriteAtlasForSelection, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitSpriteAtlasStateWithHistory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Sprite2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "columns", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rows", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "regionEnabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "regionRect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplySpriteAtlasState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Sprite2D"), exported: false),
				new PropertyInfo(Variant.Type.Int, "columns", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "rows", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "regionEnabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2, "regionRect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NotifySpriteAtlasHistoryApplied, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sprite", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Sprite2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.IsNodeInCurrentScene, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.InitializeSceneAnimationSurface && args.Count == 0)
		{
			InitializeSceneAnimationSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.ShutdownSceneAnimationSurface && args.Count == 0)
		{
			ShutdownSceneAnimationSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.SetSceneAnimationTimelineVisible && args.Count == 1)
		{
			SetSceneAnimationTimelineVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSceneAnimationSelectionChanged && args.Count == 0)
		{
			OnSceneAnimationSelectionChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.RebindSceneAnimationRoot && args.Count == 1)
		{
			RebindSceneAnimationRoot(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SuspendSceneAnimationPreviewForSave && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(SuspendSceneAnimationPreviewForSave());
			return true;
		}
		if (method == MethodName.ResumeSceneAnimationPreviewAfterSave && args.Count == 1)
		{
			ResumeSceneAnimationPreviewAfterSave(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSceneAnimationAfterHistory && args.Count == 0)
		{
			RefreshSceneAnimationAfterHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSceneAnimationAfterHistoryIfVisible && args.Count == 0)
		{
			RefreshSceneAnimationAfterHistoryIfVisible();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifySceneAnimationHistoryApplied && args.Count == 0)
		{
			NotifySceneAnimationHistoryApplied();
			ret = default;
			return true;
		}
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
		if (method == MethodName.LoadPackedScene && args.Count == 2)
		{
			LoadPackedScene(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadPackedSceneFromPath && args.Count == 1)
		{
			LoadPackedSceneFromPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveCurrentScene && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(SaveCurrentScene());
			return true;
		}
		if (method == MethodName.SaveAllScenes && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(SaveAllScenes(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.TryInstantiateFullScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Node>(TryInstantiateFullScene(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AreAllExternalCSharpScriptsRegistered && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(AreAllExternalCSharpScriptsRegistered(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadScriptlessPreviewPackedScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(LoadScriptlessPreviewPackedScene(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeScriptResourcePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeScriptResourcePath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeComparablePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeComparablePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsModCharacterSceneText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsModCharacterSceneText(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCharacterBaseSceneResourceLine && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCharacterBaseSceneResourceLine(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.IsRuntimeCharacterRootAssignment && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRuntimeCharacterRootAssignment(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsRuntimeCharacterComponentPreviewLine && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRuntimeCharacterComponentPreviewLine(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsExternalCSharpScriptPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsExternalCSharpScriptPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadSceneResourceAttribute && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadSceneResourceAttribute(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.RefreshSceneTree && args.Count == 0)
		{
			RefreshSceneTree();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildMenus && args.Count == 0)
		{
			BuildMenus();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildViewportContextMenu && args.Count == 0)
		{
			BuildViewportContextMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectSignals && args.Count == 0)
		{
			ConnectSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectDirectTransformSurface && args.Count == 0)
		{
			ConnectDirectTransformSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.GetDirectTransformSpins && args.Count == 0)
		{
			SpinBox[] directTransformSpins = GetDirectTransformSpins();
			GodotObject[] array = directTransformSpins;
			ret = VariantUtils.CreateFromSystemArrayOfGodotObject(array);
			return true;
		}
		if (method == MethodName.UpdateDirectTransformSurface && args.Count == 0)
		{
			UpdateDirectTransformSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowDirectNodeProperties && args.Count == 2)
		{
			ShowDirectNodeProperties(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDirectNodePropertySurface && args.Count == 0)
		{
			RefreshDirectNodePropertySurface();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveDirectPropertyTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Node>(ResolveDirectPropertyTarget());
			return true;
		}
		if (method == MethodName.BindDirectNodePropertyTarget && args.Count == 1)
		{
			BindDirectNodePropertyTarget(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDirectNodePropertyEdited && args.Count == 4)
		{
			OnDirectNodePropertyEdited(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnWorkspaceTabChanged && args.Count == 1)
		{
			OnWorkspaceTabChanged(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginDirectTransformEditSession && args.Count == 0)
		{
			BeginDirectTransformEditSession();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDirectTransformValueChanged && args.Count == 1)
		{
			OnDirectTransformValueChanged(VariantUtils.ConvertTo<SpinBox>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitDirectTransformEditSession && args.Count == 0)
		{
			CommitDirectTransformEditSession();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyDirectTransformHistoryApplied && args.Count == 0)
		{
			NotifyDirectTransformHistoryApplied();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDirectTransformSurface && args.Count == 0)
		{
			RefreshDirectTransformSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.DetachCurrentSceneRootForHistory && args.Count == 0)
		{
			DetachCurrentSceneRootForHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySceneRootFromHistory && args.Count == 1)
		{
			ApplySceneRootFromHistory(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveCurrentTabState && args.Count == 0)
		{
			SaveCurrentTabState();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadTabData && args.Count == 1)
		{
			LoadTabData(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SwitchToTab && args.Count == 1)
		{
			SwitchToTab(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTabChanged && args.Count == 1)
		{
			OnTabChanged(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTabClosePressed && args.Count == 1)
		{
			OnTabClosePressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentSceneHistoryId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetCurrentSceneHistoryId());
			return true;
		}
		if (method == MethodName.GetSceneHistoryIdAt && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetSceneHistoryIdAt(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSceneTabDirty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSceneTabDirty(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.RequestCloseSceneTab && args.Count == 1)
		{
			RequestCloseSceneTab(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnUnsavedSceneCloseConfirmed && args.Count == 0)
		{
			OnUnsavedSceneCloseConfirmed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnUnsavedSceneCloseCanceled && args.Count == 0)
		{
			OnUnsavedSceneCloseCanceled();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDiscardUnsavedScenePressed && args.Count == 0)
		{
			OnDiscardUnsavedScenePressed();
			ret = default;
			return true;
		}
		if (method == MethodName.CloseSceneTabImmediately && args.Count == 1)
		{
			CloseSceneTabImmediately(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSceneHistoryChanged && args.Count == 1)
		{
			OnSceneHistoryChanged(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearEditor && args.Count == 0)
		{
			ClearEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenViewportContextMenu && args.Count == 3)
		{
			OpenViewportContextMenu(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Node>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.QueueViewportRedraw && args.Count == 0)
		{
			QueueViewportRedraw();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareForSceneHistoryBranchChange && args.Count == 0)
		{
			PrepareForSceneHistoryBranchChange();
			ret = default;
			return true;
		}
		if (method == MethodName.SetToolMode && args.Count == 1)
		{
			SetToolMode(VariantUtils.ConvertTo<XW2DViewport.ToolMode>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSnapMenuItem && args.Count == 1)
		{
			OnSnapMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnViewMenuItem && args.Count == 1)
		{
			OnViewMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnViewportContextMenuItem && args.Count == 1)
		{
			OnViewportContextMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSkeletonMenuItem && args.Count == 1)
		{
			OnSkeletonMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GenerateSkeletonFromSelectionWithHistory && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Skeleton2D>(GenerateSkeletonFromSelectionWithHistory());
			return true;
		}
		if (method == MethodName.ClearGeneratedSkeletonWithHistory && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ClearGeneratedSkeletonWithHistory());
			return true;
		}
		if (method == MethodName.GetBoneRelativeTransform && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(GetBoneRelativeTransform(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node2D>(in args[1]), VariantUtils.ConvertTo<Node2D>(in args[2])));
			return true;
		}
		if (method == MethodName.UpdateGeneratedBoneGizmo && args.Count == 1)
		{
			UpdateGeneratedBoneGizmo(VariantUtils.ConvertTo<Bone2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindSelectedGeneratedSkeleton && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Skeleton2D>(FindSelectedGeneratedSkeleton());
			return true;
		}
		if (method == MethodName.FindGeneratedSkeleton && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Skeleton2D>(FindGeneratedSkeleton(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.IsGeneratedSkeleton && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsGeneratedSkeleton(VariantUtils.ConvertTo<Skeleton2D>(in args[0])));
			return true;
		}
		if (method == MethodName.GetNodeDepth && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetNodeDepth(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildUniqueChildName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StringName>(BuildUniqueChildName(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SetSelectionMetaWithHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(SetSelectionMetaWithHistory(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.UpdateSelectionActionButtons && args.Count == 0)
		{
			UpdateSelectionActionButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSnapButtons && args.Count == 0)
		{
			UpdateSnapButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncToolbarFromViewportState && args.Count == 0)
		{
			SyncToolbarFromViewportState();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSnapMenuChecks && args.Count == 0)
		{
			UpdateSnapMenuChecks();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateViewMenuChecks && args.Count == 0)
		{
			UpdateViewMenuChecks();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateViewportContextMenuState && args.Count == 0)
		{
			UpdateViewportContextMenuState();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateZoomLabel && args.Count == 0)
		{
			UpdateZoomLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMousePositionLabel && args.Count == 1)
		{
			UpdateMousePositionLabel(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSceneKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetSceneKey(VariantUtils.ConvertTo<PackedScene>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSceneDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetSceneDisplayName(VariantUtils.ConvertTo<PackedScene>(in args[0])));
			return true;
		}
		if (method == MethodName.SetToolButtonPressed && args.Count == 2)
		{
			SetToolButtonPressed(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMenuChecked && args.Count == 3)
		{
			SetMenuChecked(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMenuDisabled && args.Count == 3)
		{
			SetMenuDisabled(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FreeSceneInstance && args.Count == 1)
		{
			FreeSceneInstance(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeSceneGridSettings && args.Count == 0)
		{
			InitializeSceneGridSettings();
			ret = default;
			return true;
		}
		if (method == MethodName.ShutdownSceneGridSettings && args.Count == 0)
		{
			ShutdownSceneGridSettings();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenGridSettingsPanel && args.Count == 0)
		{
			OpenGridSettingsPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.SyncGridSettingsPanelFromViewport && args.Count == 0)
		{
			SyncGridSettingsPanelFromViewport();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildGridPanelState && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(BuildGridPanelState());
			return true;
		}
		if (method == MethodName.OnGridSettingsChanged && args.Count == 0)
		{
			OnGridSettingsChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadPersistedGridSettings && args.Count == 0)
		{
			LoadPersistedGridSettings();
			ret = default;
			return true;
		}
		if (method == MethodName.PersistGridSettings && args.Count == 0)
		{
			PersistGridSettings();
			ret = default;
			return true;
		}
		if (method == MethodName.SetGridSettingIfChanged && args.Count == 3)
		{
			SetGridSettingIfChanged(VariantUtils.ConvertTo<XWEditorSettings>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FlushGridSettingsSave && args.Count == 0)
		{
			FlushGridSettingsSave();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterGridSettingDefaults && args.Count == 1)
		{
			RegisterGridSettingDefaults(VariantUtils.ConvertTo<XWEditorSettings>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeInlineTextEditing && args.Count == 0)
		{
			InitializeInlineTextEditing();
			ret = default;
			return true;
		}
		if (method == MethodName.ShutdownInlineTextEditing && args.Count == 0)
		{
			ShutdownInlineTextEditing();
			ret = default;
			return true;
		}
		if (method == MethodName._Input && args.Count == 1)
		{
			_Input(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanInlineEditText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanInlineEditText(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearInlineTextCapabilityCache && args.Count == 0)
		{
			ClearInlineTextCapabilityCache();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginInlineTextEdit && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(BeginInlineTextEdit(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.SetInlineTextDraft && args.Count == 1)
		{
			SetInlineTextDraft(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitPendingAuthoringInput && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CommitPendingAuthoringInput());
			return true;
		}
		if (method == MethodName.CommitInlineTextEdit && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CommitInlineTextEdit());
			return true;
		}
		if (method == MethodName.CancelInlineTextEdit && args.Count == 0)
		{
			CancelInlineTextEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshInlineTextEditorGeometry && args.Count == 0)
		{
			RefreshInlineTextEditorGeometry();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyInlineTextHistoryApplied && args.Count == 1)
		{
			NotifyInlineTextHistoryApplied(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitInlineTextEditForScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CommitInlineTextEditForScene(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.OnInlineTextLineChanged && args.Count == 1)
		{
			OnInlineTextLineChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnInlineTextMultilineChanged && args.Count == 0)
		{
			OnInlineTextMultilineChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyInlineTextPreview && args.Count == 1)
		{
			ApplyInlineTextPreview(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyInlineTextValue && args.Count == 4)
		{
			ApplyInlineTextValue(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant.Type>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.MakeInlineTextVariant && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(MakeInlineTextVariant(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.QueueInlineTextFocusExitCommit && args.Count == 0)
		{
			QueueInlineTextFocusExitCommit();
			ret = default;
			return true;
		}
		if (method == MethodName.OnInlineTextEditorGuiInput && args.Count == 1)
		{
			OnInlineTextEditorGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnInlineTextHostVisibilityChanged && args.Count == 0)
		{
			OnInlineTextHostVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnInlineTextViewportVisibilityChanged && args.Count == 0)
		{
			OnInlineTextViewportVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueInlineTextGeometryRefresh && args.Count == 0)
		{
			QueueInlineTextGeometryRefresh();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureInlineTextInputForTarget && args.Count == 3)
		{
			ConfigureInlineTextInputForTarget(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<PropertyHint>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingInlineTextIme && args.Count == 0)
		{
			ApplyPendingInlineTextIme();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelPendingInlineTextIme && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CancelPendingInlineTextIme());
			return true;
		}
		if (method == MethodName.GrabInlineTextEditorFocus && args.Count == 1)
		{
			GrabInlineTextEditorFocus(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.QueueInlineTextEditorFocus && args.Count == 1)
		{
			QueueInlineTextEditorFocus(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HideInlineTextEditor && args.Count == 0)
		{
			HideInlineTextEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildInlineTextModeLabel && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(BuildInlineTextModeLabel(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.GetInlineTextControlRectInViewport && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetInlineTextControlRectInViewport(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateInlineTextHeaderButton && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Button>(CreateInlineTextHeaderButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.LoadInlineTextIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadInlineTextIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateInlineTextOverlayStyle && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreateInlineTextOverlayStyle());
			return true;
		}
		if (method == MethodName.InitializeControlLayoutSurface && args.Count == 0)
		{
			InitializeControlLayoutSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.AddGrowItems && args.Count == 1)
		{
			AddGrowItems(VariantUtils.ConvertTo<OptionButton>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GrowDirectionFromVisualIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<GrowDirection>(GrowDirectionFromVisualIndex(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetGrowDirectionVisualIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetGrowDirectionVisualIndex(VariantUtils.ConvertTo<GrowDirection>(in args[0])));
			return true;
		}
		if (method == MethodName.BindGrowDirectionButtons && args.Count == 2)
		{
			BindGrowDirectionButtons(VariantUtils.ConvertToSystemArrayOfGodotObject<Button>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetControlLayoutSpins && args.Count == 0)
		{
			SpinBox[] controlLayoutSpins = GetControlLayoutSpins();
			GodotObject[] array = controlLayoutSpins;
			ret = VariantUtils.CreateFromSystemArrayOfGodotObject(array);
			return true;
		}
		if (method == MethodName.ClearDirectSpinMixedMarker && args.Count == 1)
		{
			ClearDirectSpinMixedMarker(VariantUtils.ConvertTo<SpinBox>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyDirectSpinValue && args.Count == 2)
		{
			ApplyDirectSpinValue(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsDirectTransformFieldMixed && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDirectTransformFieldMixed(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplySelectedTransformFieldWithHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ApplySelectedTransformFieldWithHistory(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.FindDirectTransformSpin && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<SpinBox>(FindDirectTransformSpin(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateControlLayoutSurface && args.Count == 0)
		{
			UpdateControlLayoutSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectGrowDirectionButton && args.Count == 2)
		{
			SelectGrowDirectionButton(VariantUtils.ConvertToSystemArrayOfGodotObject<Button>(in args[0]), VariantUtils.ConvertTo<GrowDirection>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginControlLayoutSession && args.Count == 0)
		{
			BeginControlLayoutSession();
			ret = default;
			return true;
		}
		if (method == MethodName.OnControlLayoutSpinChanged && args.Count == 1)
		{
			OnControlLayoutSpinChanged(VariantUtils.ConvertTo<SpinBox>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitControlLayoutSession && args.Count == 0)
		{
			CommitControlLayoutSession();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginViewportControlLayoutSession && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(BeginViewportControlLayoutSession(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.CommitViewportControlLayoutSession && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CommitViewportControlLayoutSession(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CancelViewportControlLayoutSession && args.Count == 0)
		{
			CancelViewportControlLayoutSession();
			ret = default;
			return true;
		}
		if (method == MethodName.EditSelectedControlLayoutFieldWithHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(EditSelectedControlLayoutFieldWithHistory(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<double>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplySelectedControlLayoutPresetWithHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ApplySelectedControlLayoutPresetWithHistory(VariantUtils.ConvertTo<LayoutPreset>(in args[0])));
			return true;
		}
		if (method == MethodName.ApplyControlLayoutPresetWithHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ApplyControlLayoutPresetWithHistory(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<LayoutPreset>(in args[1])));
			return true;
		}
		if (method == MethodName.SetControlGrowWithHistory && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(SetControlGrowWithHistory(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<GrowDirection>(in args[2])));
			return true;
		}
		if (method == MethodName.AddHistoryProperty && args.Count == 5)
		{
			AddHistoryProperty(VariantUtils.ConvertTo<XWUndoRedoManager>(in args[0]), VariantUtils.ConvertTo<GodotObject>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyControlLayoutHistoryApplied && args.Count == 1)
		{
			NotifyControlLayoutHistoryApplied(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSelectionLockWithHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetSelectionLockWithHistory(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.SetSelectionGroupWithHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetSelectionGroupWithHistory(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.CommitSelectionMetaHistory && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(CommitSelectionMetaHistory(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ApplyNodeEditMeta && args.Count == 3)
		{
			ApplyNodeEditMeta(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifySelectionMetaHistoryApplied && args.Count == 1)
		{
			NotifySelectionMetaHistoryApplied(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareSceneSurfaceHistoryMutation && args.Count == 0)
		{
			PrepareSceneSurfaceHistoryMutation();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkCurrentSceneHistorySurfaceDirty && args.Count == 0)
		{
			MarkCurrentSceneHistorySurfaceDirty();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCurrentSceneDirtyMarker && args.Count == 0)
		{
			UpdateCurrentSceneDirtyMarker();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSceneDirtyMarker && args.Count == 1)
		{
			UpdateSceneDirtyMarker(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeSelectionLayoutSurface && args.Count == 0)
		{
			InitializeSelectionLayoutSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSelectionLayoutSurface && args.Count == 0)
		{
			UpdateSelectionLayoutSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.CollectSelectionLayoutNodes && args.Count == 0)
		{
			CollectSelectionLayoutNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplySelectionLayoutWithHistory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ApplySelectionLayoutWithHistory(VariantUtils.ConvertTo<SelectionLayoutOperation>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAxisStart && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(GetAxisStart(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetAxisSize && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(GetAxisSize(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetAxisEnd && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(GetAxisEnd(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetSelectionLayoutHistoryName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetSelectionLayoutHistoryName(VariantUtils.ConvertTo<SelectionLayoutOperation>(in args[0])));
			return true;
		}
		if (method == MethodName.InitializeSpriteAtlasSurface && args.Count == 0)
		{
			InitializeSpriteAtlasSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.ShutdownSpriteAtlasSurface && args.Count == 0)
		{
			ShutdownSpriteAtlasSurface();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSpriteAtlasSelectionChanged && args.Count == 0)
		{
			OnSpriteAtlasSelectionChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveSingleSelectedSprite && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Sprite2D>(ResolveSingleSelectedSprite());
			return true;
		}
		if (method == MethodName.OpenSelectedSpriteAtlas && args.Count == 0)
		{
			OpenSelectedSpriteAtlas();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenSpriteAtlasForSelection && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(OpenSpriteAtlasForSelection());
			return true;
		}
		if (method == MethodName.CommitSpriteAtlasStateWithHistory && args.Count == 7)
		{
			ret = VariantUtils.CreateFrom<bool>(CommitSpriteAtlasStateWithHistory(VariantUtils.ConvertTo<Sprite2D>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<Rect2>(in args[5]), VariantUtils.ConvertTo<string>(in args[6])));
			return true;
		}
		if (method == MethodName.ApplySpriteAtlasState && args.Count == 6)
		{
			ApplySpriteAtlasState(VariantUtils.ConvertTo<Sprite2D>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]), VariantUtils.ConvertTo<int>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<Rect2>(in args[5]));
			ret = default;
			return true;
		}
		if (method == MethodName.NotifySpriteAtlasHistoryApplied && args.Count == 1)
		{
			NotifySpriteAtlasHistoryApplied(VariantUtils.ConvertTo<Sprite2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsNodeInCurrentScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsNodeInCurrentScene(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.TryInstantiateFullScene && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Node>(TryInstantiateFullScene(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.AreAllExternalCSharpScriptsRegistered && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(AreAllExternalCSharpScriptsRegistered(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadScriptlessPreviewPackedScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(LoadScriptlessPreviewPackedScene(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeScriptResourcePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeScriptResourcePath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeComparablePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeComparablePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsModCharacterSceneText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsModCharacterSceneText(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsCharacterBaseSceneResourceLine && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCharacterBaseSceneResourceLine(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.IsRuntimeCharacterRootAssignment && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRuntimeCharacterRootAssignment(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsRuntimeCharacterComponentPreviewLine && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsRuntimeCharacterComponentPreviewLine(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsExternalCSharpScriptPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsExternalCSharpScriptPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadSceneResourceAttribute && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadSceneResourceAttribute(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetBoneRelativeTransform && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Transform2D>(GetBoneRelativeTransform(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<Node2D>(in args[1]), VariantUtils.ConvertTo<Node2D>(in args[2])));
			return true;
		}
		if (method == MethodName.UpdateGeneratedBoneGizmo && args.Count == 1)
		{
			UpdateGeneratedBoneGizmo(VariantUtils.ConvertTo<Bone2D>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindGeneratedSkeleton && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Skeleton2D>(FindGeneratedSkeleton(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.IsGeneratedSkeleton && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsGeneratedSkeleton(VariantUtils.ConvertTo<Skeleton2D>(in args[0])));
			return true;
		}
		if (method == MethodName.GetNodeDepth && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetNodeDepth(VariantUtils.ConvertTo<Node>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildUniqueChildName && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<StringName>(BuildUniqueChildName(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetSceneKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetSceneKey(VariantUtils.ConvertTo<PackedScene>(in args[0])));
			return true;
		}
		if (method == MethodName.GetSceneDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetSceneDisplayName(VariantUtils.ConvertTo<PackedScene>(in args[0])));
			return true;
		}
		if (method == MethodName.SetToolButtonPressed && args.Count == 2)
		{
			SetToolButtonPressed(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMenuChecked && args.Count == 3)
		{
			SetMenuChecked(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMenuDisabled && args.Count == 3)
		{
			SetMenuDisabled(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FreeSceneInstance && args.Count == 1)
		{
			FreeSceneInstance(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetGridSettingIfChanged && args.Count == 3)
		{
			SetGridSettingIfChanged(VariantUtils.ConvertTo<XWEditorSettings>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterGridSettingDefaults && args.Count == 1)
		{
			RegisterGridSettingDefaults(VariantUtils.ConvertTo<XWEditorSettings>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyInlineTextValue && args.Count == 4)
		{
			ApplyInlineTextValue(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant.Type>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.MakeInlineTextVariant && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(MakeInlineTextVariant(VariantUtils.ConvertTo<Variant.Type>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildInlineTextModeLabel && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<string>(BuildInlineTextModeLabel(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.CreateInlineTextHeaderButton && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Button>(CreateInlineTextHeaderButton(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2])));
			return true;
		}
		if (method == MethodName.LoadInlineTextIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadInlineTextIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateInlineTextOverlayStyle && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreateInlineTextOverlayStyle());
			return true;
		}
		if (method == MethodName.AddGrowItems && args.Count == 1)
		{
			AddGrowItems(VariantUtils.ConvertTo<OptionButton>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GrowDirectionFromVisualIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<GrowDirection>(GrowDirectionFromVisualIndex(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetGrowDirectionVisualIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetGrowDirectionVisualIndex(VariantUtils.ConvertTo<GrowDirection>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectGrowDirectionButton && args.Count == 2)
		{
			SelectGrowDirectionButton(VariantUtils.ConvertToSystemArrayOfGodotObject<Button>(in args[0]), VariantUtils.ConvertTo<GrowDirection>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddHistoryProperty && args.Count == 5)
		{
			AddHistoryProperty(VariantUtils.ConvertTo<XWUndoRedoManager>(in args[0]), VariantUtils.ConvertTo<GodotObject>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]), VariantUtils.ConvertTo<float>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetAxisStart && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(GetAxisStart(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetAxisSize && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(GetAxisSize(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetAxisEnd && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<float>(GetAxisEnd(VariantUtils.ConvertTo<Rect2>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetSelectionLayoutHistoryName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetSelectionLayoutHistoryName(VariantUtils.ConvertTo<SelectionLayoutOperation>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.InitializeSceneAnimationSurface)
		{
			return true;
		}
		if (method == MethodName.ShutdownSceneAnimationSurface)
		{
			return true;
		}
		if (method == MethodName.SetSceneAnimationTimelineVisible)
		{
			return true;
		}
		if (method == MethodName.OnSceneAnimationSelectionChanged)
		{
			return true;
		}
		if (method == MethodName.RebindSceneAnimationRoot)
		{
			return true;
		}
		if (method == MethodName.SuspendSceneAnimationPreviewForSave)
		{
			return true;
		}
		if (method == MethodName.ResumeSceneAnimationPreviewAfterSave)
		{
			return true;
		}
		if (method == MethodName.RefreshSceneAnimationAfterHistory)
		{
			return true;
		}
		if (method == MethodName.RefreshSceneAnimationAfterHistoryIfVisible)
		{
			return true;
		}
		if (method == MethodName.NotifySceneAnimationHistoryApplied)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.LoadPackedScene)
		{
			return true;
		}
		if (method == MethodName.LoadPackedSceneFromPath)
		{
			return true;
		}
		if (method == MethodName.SaveCurrentScene)
		{
			return true;
		}
		if (method == MethodName.SaveAllScenes)
		{
			return true;
		}
		if (method == MethodName.TryInstantiateFullScene)
		{
			return true;
		}
		if (method == MethodName.AreAllExternalCSharpScriptsRegistered)
		{
			return true;
		}
		if (method == MethodName.LoadScriptlessPreviewPackedScene)
		{
			return true;
		}
		if (method == MethodName.NormalizeScriptResourcePath)
		{
			return true;
		}
		if (method == MethodName.NormalizeComparablePath)
		{
			return true;
		}
		if (method == MethodName.IsModCharacterSceneText)
		{
			return true;
		}
		if (method == MethodName.IsCharacterBaseSceneResourceLine)
		{
			return true;
		}
		if (method == MethodName.IsRuntimeCharacterRootAssignment)
		{
			return true;
		}
		if (method == MethodName.IsRuntimeCharacterComponentPreviewLine)
		{
			return true;
		}
		if (method == MethodName.IsExternalCSharpScriptPath)
		{
			return true;
		}
		if (method == MethodName.ReadSceneResourceAttribute)
		{
			return true;
		}
		if (method == MethodName.RefreshSceneTree)
		{
			return true;
		}
		if (method == MethodName.BuildMenus)
		{
			return true;
		}
		if (method == MethodName.BuildViewportContextMenu)
		{
			return true;
		}
		if (method == MethodName.ConnectSignals)
		{
			return true;
		}
		if (method == MethodName.ConnectDirectTransformSurface)
		{
			return true;
		}
		if (method == MethodName.GetDirectTransformSpins)
		{
			return true;
		}
		if (method == MethodName.UpdateDirectTransformSurface)
		{
			return true;
		}
		if (method == MethodName.ShowDirectNodeProperties)
		{
			return true;
		}
		if (method == MethodName.RefreshDirectNodePropertySurface)
		{
			return true;
		}
		if (method == MethodName.ResolveDirectPropertyTarget)
		{
			return true;
		}
		if (method == MethodName.BindDirectNodePropertyTarget)
		{
			return true;
		}
		if (method == MethodName.OnDirectNodePropertyEdited)
		{
			return true;
		}
		if (method == MethodName.OnWorkspaceTabChanged)
		{
			return true;
		}
		if (method == MethodName.BeginDirectTransformEditSession)
		{
			return true;
		}
		if (method == MethodName.OnDirectTransformValueChanged)
		{
			return true;
		}
		if (method == MethodName.CommitDirectTransformEditSession)
		{
			return true;
		}
		if (method == MethodName.NotifyDirectTransformHistoryApplied)
		{
			return true;
		}
		if (method == MethodName.RefreshDirectTransformSurface)
		{
			return true;
		}
		if (method == MethodName.DetachCurrentSceneRootForHistory)
		{
			return true;
		}
		if (method == MethodName.ApplySceneRootFromHistory)
		{
			return true;
		}
		if (method == MethodName.SaveCurrentTabState)
		{
			return true;
		}
		if (method == MethodName.LoadTabData)
		{
			return true;
		}
		if (method == MethodName.SwitchToTab)
		{
			return true;
		}
		if (method == MethodName.OnTabChanged)
		{
			return true;
		}
		if (method == MethodName.OnTabClosePressed)
		{
			return true;
		}
		if (method == MethodName.GetCurrentSceneHistoryId)
		{
			return true;
		}
		if (method == MethodName.GetSceneHistoryIdAt)
		{
			return true;
		}
		if (method == MethodName.IsSceneTabDirty)
		{
			return true;
		}
		if (method == MethodName.RequestCloseSceneTab)
		{
			return true;
		}
		if (method == MethodName.OnUnsavedSceneCloseConfirmed)
		{
			return true;
		}
		if (method == MethodName.OnUnsavedSceneCloseCanceled)
		{
			return true;
		}
		if (method == MethodName.OnDiscardUnsavedScenePressed)
		{
			return true;
		}
		if (method == MethodName.CloseSceneTabImmediately)
		{
			return true;
		}
		if (method == MethodName.OnSceneHistoryChanged)
		{
			return true;
		}
		if (method == MethodName.ClearEditor)
		{
			return true;
		}
		if (method == MethodName.OpenViewportContextMenu)
		{
			return true;
		}
		if (method == MethodName.QueueViewportRedraw)
		{
			return true;
		}
		if (method == MethodName.PrepareForSceneHistoryBranchChange)
		{
			return true;
		}
		if (method == MethodName.SetToolMode)
		{
			return true;
		}
		if (method == MethodName.OnSnapMenuItem)
		{
			return true;
		}
		if (method == MethodName.OnViewMenuItem)
		{
			return true;
		}
		if (method == MethodName.OnViewportContextMenuItem)
		{
			return true;
		}
		if (method == MethodName.OnSkeletonMenuItem)
		{
			return true;
		}
		if (method == MethodName.GenerateSkeletonFromSelectionWithHistory)
		{
			return true;
		}
		if (method == MethodName.ClearGeneratedSkeletonWithHistory)
		{
			return true;
		}
		if (method == MethodName.GetBoneRelativeTransform)
		{
			return true;
		}
		if (method == MethodName.UpdateGeneratedBoneGizmo)
		{
			return true;
		}
		if (method == MethodName.FindSelectedGeneratedSkeleton)
		{
			return true;
		}
		if (method == MethodName.FindGeneratedSkeleton)
		{
			return true;
		}
		if (method == MethodName.IsGeneratedSkeleton)
		{
			return true;
		}
		if (method == MethodName.GetNodeDepth)
		{
			return true;
		}
		if (method == MethodName.BuildUniqueChildName)
		{
			return true;
		}
		if (method == MethodName.SetSelectionMetaWithHistory)
		{
			return true;
		}
		if (method == MethodName.UpdateSelectionActionButtons)
		{
			return true;
		}
		if (method == MethodName.UpdateSnapButtons)
		{
			return true;
		}
		if (method == MethodName.SyncToolbarFromViewportState)
		{
			return true;
		}
		if (method == MethodName.UpdateSnapMenuChecks)
		{
			return true;
		}
		if (method == MethodName.UpdateViewMenuChecks)
		{
			return true;
		}
		if (method == MethodName.UpdateViewportContextMenuState)
		{
			return true;
		}
		if (method == MethodName.UpdateZoomLabel)
		{
			return true;
		}
		if (method == MethodName.UpdateMousePositionLabel)
		{
			return true;
		}
		if (method == MethodName.GetSceneKey)
		{
			return true;
		}
		if (method == MethodName.GetSceneDisplayName)
		{
			return true;
		}
		if (method == MethodName.SetToolButtonPressed)
		{
			return true;
		}
		if (method == MethodName.SetMenuChecked)
		{
			return true;
		}
		if (method == MethodName.SetMenuDisabled)
		{
			return true;
		}
		if (method == MethodName.LoadIcon)
		{
			return true;
		}
		if (method == MethodName.FreeSceneInstance)
		{
			return true;
		}
		if (method == MethodName.InitializeSceneGridSettings)
		{
			return true;
		}
		if (method == MethodName.ShutdownSceneGridSettings)
		{
			return true;
		}
		if (method == MethodName.OpenGridSettingsPanel)
		{
			return true;
		}
		if (method == MethodName.SyncGridSettingsPanelFromViewport)
		{
			return true;
		}
		if (method == MethodName.BuildGridPanelState)
		{
			return true;
		}
		if (method == MethodName.OnGridSettingsChanged)
		{
			return true;
		}
		if (method == MethodName.LoadPersistedGridSettings)
		{
			return true;
		}
		if (method == MethodName.PersistGridSettings)
		{
			return true;
		}
		if (method == MethodName.SetGridSettingIfChanged)
		{
			return true;
		}
		if (method == MethodName.FlushGridSettingsSave)
		{
			return true;
		}
		if (method == MethodName.RegisterGridSettingDefaults)
		{
			return true;
		}
		if (method == MethodName.InitializeInlineTextEditing)
		{
			return true;
		}
		if (method == MethodName.ShutdownInlineTextEditing)
		{
			return true;
		}
		if (method == MethodName._Input)
		{
			return true;
		}
		if (method == MethodName.CanInlineEditText)
		{
			return true;
		}
		if (method == MethodName.ClearInlineTextCapabilityCache)
		{
			return true;
		}
		if (method == MethodName.BeginInlineTextEdit)
		{
			return true;
		}
		if (method == MethodName.SetInlineTextDraft)
		{
			return true;
		}
		if (method == MethodName.CommitPendingAuthoringInput)
		{
			return true;
		}
		if (method == MethodName.CommitInlineTextEdit)
		{
			return true;
		}
		if (method == MethodName.CancelInlineTextEdit)
		{
			return true;
		}
		if (method == MethodName.RefreshInlineTextEditorGeometry)
		{
			return true;
		}
		if (method == MethodName.NotifyInlineTextHistoryApplied)
		{
			return true;
		}
		if (method == MethodName.CommitInlineTextEditForScene)
		{
			return true;
		}
		if (method == MethodName.OnInlineTextLineChanged)
		{
			return true;
		}
		if (method == MethodName.OnInlineTextMultilineChanged)
		{
			return true;
		}
		if (method == MethodName.ApplyInlineTextPreview)
		{
			return true;
		}
		if (method == MethodName.ApplyInlineTextValue)
		{
			return true;
		}
		if (method == MethodName.MakeInlineTextVariant)
		{
			return true;
		}
		if (method == MethodName.QueueInlineTextFocusExitCommit)
		{
			return true;
		}
		if (method == MethodName.OnInlineTextEditorGuiInput)
		{
			return true;
		}
		if (method == MethodName.OnInlineTextHostVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.OnInlineTextViewportVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.QueueInlineTextGeometryRefresh)
		{
			return true;
		}
		if (method == MethodName.ConfigureInlineTextInputForTarget)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingInlineTextIme)
		{
			return true;
		}
		if (method == MethodName.CancelPendingInlineTextIme)
		{
			return true;
		}
		if (method == MethodName.GrabInlineTextEditorFocus)
		{
			return true;
		}
		if (method == MethodName.QueueInlineTextEditorFocus)
		{
			return true;
		}
		if (method == MethodName.HideInlineTextEditor)
		{
			return true;
		}
		if (method == MethodName.BuildInlineTextModeLabel)
		{
			return true;
		}
		if (method == MethodName.GetInlineTextControlRectInViewport)
		{
			return true;
		}
		if (method == MethodName.CreateInlineTextHeaderButton)
		{
			return true;
		}
		if (method == MethodName.LoadInlineTextIcon)
		{
			return true;
		}
		if (method == MethodName.CreateInlineTextOverlayStyle)
		{
			return true;
		}
		if (method == MethodName.InitializeControlLayoutSurface)
		{
			return true;
		}
		if (method == MethodName.AddGrowItems)
		{
			return true;
		}
		if (method == MethodName.GrowDirectionFromVisualIndex)
		{
			return true;
		}
		if (method == MethodName.GetGrowDirectionVisualIndex)
		{
			return true;
		}
		if (method == MethodName.BindGrowDirectionButtons)
		{
			return true;
		}
		if (method == MethodName.GetControlLayoutSpins)
		{
			return true;
		}
		if (method == MethodName.ClearDirectSpinMixedMarker)
		{
			return true;
		}
		if (method == MethodName.ApplyDirectSpinValue)
		{
			return true;
		}
		if (method == MethodName.IsDirectTransformFieldMixed)
		{
			return true;
		}
		if (method == MethodName.ApplySelectedTransformFieldWithHistory)
		{
			return true;
		}
		if (method == MethodName.FindDirectTransformSpin)
		{
			return true;
		}
		if (method == MethodName.UpdateControlLayoutSurface)
		{
			return true;
		}
		if (method == MethodName.SelectGrowDirectionButton)
		{
			return true;
		}
		if (method == MethodName.BeginControlLayoutSession)
		{
			return true;
		}
		if (method == MethodName.OnControlLayoutSpinChanged)
		{
			return true;
		}
		if (method == MethodName.CommitControlLayoutSession)
		{
			return true;
		}
		if (method == MethodName.BeginViewportControlLayoutSession)
		{
			return true;
		}
		if (method == MethodName.CommitViewportControlLayoutSession)
		{
			return true;
		}
		if (method == MethodName.CancelViewportControlLayoutSession)
		{
			return true;
		}
		if (method == MethodName.EditSelectedControlLayoutFieldWithHistory)
		{
			return true;
		}
		if (method == MethodName.ApplySelectedControlLayoutPresetWithHistory)
		{
			return true;
		}
		if (method == MethodName.ApplyControlLayoutPresetWithHistory)
		{
			return true;
		}
		if (method == MethodName.SetControlGrowWithHistory)
		{
			return true;
		}
		if (method == MethodName.AddHistoryProperty)
		{
			return true;
		}
		if (method == MethodName.NotifyControlLayoutHistoryApplied)
		{
			return true;
		}
		if (method == MethodName.SetSelectionLockWithHistory)
		{
			return true;
		}
		if (method == MethodName.SetSelectionGroupWithHistory)
		{
			return true;
		}
		if (method == MethodName.CommitSelectionMetaHistory)
		{
			return true;
		}
		if (method == MethodName.ApplyNodeEditMeta)
		{
			return true;
		}
		if (method == MethodName.NotifySelectionMetaHistoryApplied)
		{
			return true;
		}
		if (method == MethodName.PrepareSceneSurfaceHistoryMutation)
		{
			return true;
		}
		if (method == MethodName.MarkCurrentSceneHistorySurfaceDirty)
		{
			return true;
		}
		if (method == MethodName.UpdateCurrentSceneDirtyMarker)
		{
			return true;
		}
		if (method == MethodName.UpdateSceneDirtyMarker)
		{
			return true;
		}
		if (method == MethodName.InitializeSelectionLayoutSurface)
		{
			return true;
		}
		if (method == MethodName.UpdateSelectionLayoutSurface)
		{
			return true;
		}
		if (method == MethodName.CollectSelectionLayoutNodes)
		{
			return true;
		}
		if (method == MethodName.ApplySelectionLayoutWithHistory)
		{
			return true;
		}
		if (method == MethodName.GetAxisStart)
		{
			return true;
		}
		if (method == MethodName.GetAxisSize)
		{
			return true;
		}
		if (method == MethodName.GetAxisEnd)
		{
			return true;
		}
		if (method == MethodName.GetSelectionLayoutHistoryName)
		{
			return true;
		}
		if (method == MethodName.InitializeSpriteAtlasSurface)
		{
			return true;
		}
		if (method == MethodName.ShutdownSpriteAtlasSurface)
		{
			return true;
		}
		if (method == MethodName.OnSpriteAtlasSelectionChanged)
		{
			return true;
		}
		if (method == MethodName.ResolveSingleSelectedSprite)
		{
			return true;
		}
		if (method == MethodName.OpenSelectedSpriteAtlas)
		{
			return true;
		}
		if (method == MethodName.OpenSpriteAtlasForSelection)
		{
			return true;
		}
		if (method == MethodName.CommitSpriteAtlasStateWithHistory)
		{
			return true;
		}
		if (method == MethodName.ApplySpriteAtlasState)
		{
			return true;
		}
		if (method == MethodName.NotifySpriteAtlasHistoryApplied)
		{
			return true;
		}
		if (method == MethodName.IsNodeInCurrentScene)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.CurrentPackedScene)
		{
			CurrentPackedScene = VariantUtils.ConvertTo<PackedScene>(in value);
			return true;
		}
		if (name == PropertyName.CurrentSceneInstance)
		{
			CurrentSceneInstance = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName.InlineTextCommitCount)
		{
			InlineTextCommitCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.InlineTextCancelCount)
		{
			InlineTextCancelCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.InlineTextPreviewCount)
		{
			InlineTextPreviewCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._animationTimelineButton)
		{
			_animationTimelineButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._canvasTimelineSplit)
		{
			_canvasTimelineSplit = VariantUtils.ConvertTo<VSplitContainer>(in value);
			return true;
		}
		if (name == PropertyName._sceneAnimationTimelinePanel)
		{
			_sceneAnimationTimelinePanel = VariantUtils.ConvertTo<XW2DAnimationTimelinePanel>(in value);
			return true;
		}
		if (name == PropertyName._sceneAnimationSelectionConnected)
		{
			_sceneAnimationSelectionConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sceneTabBar)
		{
			_sceneTabBar = VariantUtils.ConvertTo<TabBar>(in value);
			return true;
		}
		if (name == PropertyName._viewport)
		{
			_viewport = VariantUtils.ConvertTo<XW2DViewport>(in value);
			return true;
		}
		if (name == PropertyName._mousePositionLabel)
		{
			_mousePositionLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._zoomLabel)
		{
			_zoomLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._selectButton)
		{
			_selectButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveButton)
		{
			_moveButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._rotateButton)
		{
			_rotateButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._scaleButton)
		{
			_scaleButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._listSelectButton)
		{
			_listSelectButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._pivotButton)
		{
			_pivotButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._panButton)
		{
			_panButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._rulerButton)
		{
			_rulerButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._pathButton)
		{
			_pathButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._polygonButton)
		{
			_polygonButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._localSpaceButton)
		{
			_localSpaceButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._smartSnapButton)
		{
			_smartSnapButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._gridSnapButton)
		{
			_gridSnapButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._lockButton)
		{
			_lockButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._unlockButton)
		{
			_unlockButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._groupButton)
		{
			_groupButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._ungroupButton)
		{
			_ungroupButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._centerViewButton)
		{
			_centerViewButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._zoomOutButton)
		{
			_zoomOutButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._zoomResetButton)
		{
			_zoomResetButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._zoomInButton)
		{
			_zoomInButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._snapConfigMenu)
		{
			_snapConfigMenu = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._skeletonMenu)
		{
			_skeletonMenu = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._viewMenu)
		{
			_viewMenu = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._viewportContextMenu)
		{
			_viewportContextMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._workspaceTabs)
		{
			_workspaceTabs = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._allPropertiesButton)
		{
			_allPropertiesButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._nodeDirectPropertySurface)
		{
			_nodeDirectPropertySurface = VariantUtils.ConvertTo<XWDirectPropertySurface>(in value);
			return true;
		}
		if (name == PropertyName._transformDirectPanel)
		{
			_transformDirectPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._transformSelectionLabel)
		{
			_transformSelectionLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._controlFields)
		{
			_controlFields = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._positionXSpin)
		{
			_positionXSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._positionYSpin)
		{
			_positionYSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._rotationSpin)
		{
			_rotationSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._scaleXSpin)
		{
			_scaleXSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._scaleYSpin)
		{
			_scaleYSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._sizeXSpin)
		{
			_sizeXSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._sizeYSpin)
		{
			_sizeYSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._pivotXSpin)
		{
			_pivotXSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._pivotYSpin)
		{
			_pivotYSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._currentTabKey)
		{
			_currentTabKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._isSwitchingTab)
		{
			_isSwitchingTab = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._contextMenuWorldPosition)
		{
			_contextMenuWorldPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._directTransformNode)
		{
			_directTransformNode = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._directTransformSessionActive)
		{
			_directTransformSessionActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._updatingDirectTransformSurface)
		{
			_updatingDirectTransformSurface = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._sceneRootHistoryViewportState)
		{
			_sceneRootHistoryViewportState = VariantUtils.ConvertTo<Dictionary>(in value);
			return true;
		}
		if (name == PropertyName._sceneHistoryManager)
		{
			_sceneHistoryManager = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName._unsavedSceneCloseDialog)
		{
			_unsavedSceneCloseDialog = VariantUtils.ConvertTo<ConfirmationDialog>(in value);
			return true;
		}
		if (name == PropertyName._discardUnsavedSceneButton)
		{
			_discardUnsavedSceneButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._pendingCloseTabKey)
		{
			_pendingCloseTabKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._gridSettingsPopup)
		{
			_gridSettingsPopup = VariantUtils.ConvertTo<PopupPanel>(in value);
			return true;
		}
		if (name == PropertyName._gridSettingsPanel)
		{
			_gridSettingsPanel = VariantUtils.ConvertTo<XW2DGridSettingsPanel>(in value);
			return true;
		}
		if (name == PropertyName._gridSettingsSaveTimer)
		{
			_gridSettingsSaveTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextOverlay)
		{
			_inlineTextOverlay = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextModeLabel)
		{
			_inlineTextModeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextLineEdit)
		{
			_inlineTextLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextMultilineEdit)
		{
			_inlineTextMultilineEdit = VariantUtils.ConvertTo<TextEdit>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextCommitButton)
		{
			_inlineTextCommitButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextCancelButton)
		{
			_inlineTextCancelButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextOverlayMotion)
		{
			_inlineTextOverlayMotion = VariantUtils.ConvertTo<XWUiMotion>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextTarget)
		{
			_inlineTextTarget = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextProperty)
		{
			_inlineTextProperty = VariantUtils.ConvertTo<StringName>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextValueType)
		{
			_inlineTextValueType = VariantUtils.ConvertTo<Variant.Type>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextHint)
		{
			_inlineTextHint = VariantUtils.ConvertTo<PropertyHint>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextBefore)
		{
			_inlineTextBefore = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextHistoryId)
		{
			_inlineTextHistoryId = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextSessionToken)
		{
			_inlineTextSessionToken = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextMultiline)
		{
			_inlineTextMultiline = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextApplyingPreview)
		{
			_inlineTextApplyingPreview = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextGeometryRefreshQueued)
		{
			_inlineTextGeometryRefreshQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._controlLayoutPanel)
		{
			_controlLayoutPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._anchorLeftSpin)
		{
			_anchorLeftSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._anchorTopSpin)
		{
			_anchorTopSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._anchorRightSpin)
		{
			_anchorRightSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._anchorBottomSpin)
		{
			_anchorBottomSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._offsetLeftSpin)
		{
			_offsetLeftSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._offsetTopSpin)
		{
			_offsetTopSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._offsetRightSpin)
		{
			_offsetRightSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._offsetBottomSpin)
		{
			_offsetBottomSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._growHorizontalOption)
		{
			_growHorizontalOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._growVerticalOption)
		{
			_growVerticalOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._growHorizontalButtons)
		{
			_growHorizontalButtons = VariantUtils.ConvertToSystemArrayOfGodotObject<Button>(in value);
			return true;
		}
		if (name == PropertyName._growVerticalButtons)
		{
			_growVerticalButtons = VariantUtils.ConvertToSystemArrayOfGodotObject<Button>(in value);
			return true;
		}
		if (name == PropertyName._presetTopLeftButton)
		{
			_presetTopLeftButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._presetCenterButton)
		{
			_presetCenterButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._presetFullRectButton)
		{
			_presetFullRectButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._presetHCenterWideButton)
		{
			_presetHCenterWideButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._presetVCenterWideButton)
		{
			_presetVCenterWideButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._layoutControl)
		{
			_layoutControl = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._layoutSessionActive)
		{
			_layoutSessionActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._updatingLayoutSurface)
		{
			_updatingLayoutSurface = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._selectionLayoutPanel)
		{
			_selectionLayoutPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._selectionLayoutLabel)
		{
			_selectionLayoutLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._selectionAlignButtons)
		{
			_selectionAlignButtons = VariantUtils.ConvertToSystemArrayOfGodotObject<Button>(in value);
			return true;
		}
		if (name == PropertyName._selectionDistributeButtons)
		{
			_selectionDistributeButtons = VariantUtils.ConvertToSystemArrayOfGodotObject<Button>(in value);
			return true;
		}
		if (name == PropertyName._spriteAtlasPanel)
		{
			_spriteAtlasPanel = VariantUtils.ConvertTo<XW2DSpriteAtlasPanel>(in value);
			return true;
		}
		if (name == PropertyName._spriteAtlasButton)
		{
			_spriteAtlasButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._spriteAtlasTabIndex)
		{
			_spriteAtlasTabIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._spriteAtlasSelectionConnected)
		{
			_spriteAtlasSelectionConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.CurrentPackedScene)
		{
			value = VariantUtils.CreateFrom<PackedScene>(CurrentPackedScene);
			return true;
		}
		Node from;
		if (name == PropertyName.CurrentSceneInstance)
		{
			from = CurrentSceneInstance;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.InlineTextEditorVisible)
		{
			value = VariantUtils.CreateFrom<bool>(InlineTextEditorVisible);
			return true;
		}
		if (name == PropertyName.InlineTextTarget)
		{
			from = InlineTextTarget;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.InlineTextProperty)
		{
			value = VariantUtils.CreateFrom<StringName>(InlineTextProperty);
			return true;
		}
		if (name == PropertyName.InlineTextValueType)
		{
			value = VariantUtils.CreateFrom<Variant.Type>(InlineTextValueType);
			return true;
		}
		if (name == PropertyName.InlineTextHint)
		{
			value = VariantUtils.CreateFrom<PropertyHint>(InlineTextHint);
			return true;
		}
		if (name == PropertyName.InlineTextDraft)
		{
			value = VariantUtils.CreateFrom<string>(InlineTextDraft);
			return true;
		}
		int from2;
		if (name == PropertyName.InlineTextCommitCount)
		{
			from2 = InlineTextCommitCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.InlineTextCancelCount)
		{
			from2 = InlineTextCancelCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.InlineTextPreviewCount)
		{
			from2 = InlineTextPreviewCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		Rect2 from3;
		if (name == PropertyName.InlineTextOverlayRect)
		{
			from3 = InlineTextOverlayRect;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.InlineTextInputRect)
		{
			from3 = InlineTextInputRect;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.SpriteAtlasPanel)
		{
			value = VariantUtils.CreateFrom<XW2DSpriteAtlasPanel>(SpriteAtlasPanel);
			return true;
		}
		if (name == PropertyName._animationTimelineButton)
		{
			value = VariantUtils.CreateFrom(in _animationTimelineButton);
			return true;
		}
		if (name == PropertyName._canvasTimelineSplit)
		{
			value = VariantUtils.CreateFrom(in _canvasTimelineSplit);
			return true;
		}
		if (name == PropertyName._sceneAnimationTimelinePanel)
		{
			value = VariantUtils.CreateFrom(in _sceneAnimationTimelinePanel);
			return true;
		}
		if (name == PropertyName._sceneAnimationSelectionConnected)
		{
			value = VariantUtils.CreateFrom(in _sceneAnimationSelectionConnected);
			return true;
		}
		if (name == PropertyName._sceneTabBar)
		{
			value = VariantUtils.CreateFrom(in _sceneTabBar);
			return true;
		}
		if (name == PropertyName._viewport)
		{
			value = VariantUtils.CreateFrom(in _viewport);
			return true;
		}
		if (name == PropertyName._mousePositionLabel)
		{
			value = VariantUtils.CreateFrom(in _mousePositionLabel);
			return true;
		}
		if (name == PropertyName._zoomLabel)
		{
			value = VariantUtils.CreateFrom(in _zoomLabel);
			return true;
		}
		if (name == PropertyName._selectButton)
		{
			value = VariantUtils.CreateFrom(in _selectButton);
			return true;
		}
		if (name == PropertyName._moveButton)
		{
			value = VariantUtils.CreateFrom(in _moveButton);
			return true;
		}
		if (name == PropertyName._rotateButton)
		{
			value = VariantUtils.CreateFrom(in _rotateButton);
			return true;
		}
		if (name == PropertyName._scaleButton)
		{
			value = VariantUtils.CreateFrom(in _scaleButton);
			return true;
		}
		if (name == PropertyName._listSelectButton)
		{
			value = VariantUtils.CreateFrom(in _listSelectButton);
			return true;
		}
		if (name == PropertyName._pivotButton)
		{
			value = VariantUtils.CreateFrom(in _pivotButton);
			return true;
		}
		if (name == PropertyName._panButton)
		{
			value = VariantUtils.CreateFrom(in _panButton);
			return true;
		}
		if (name == PropertyName._rulerButton)
		{
			value = VariantUtils.CreateFrom(in _rulerButton);
			return true;
		}
		if (name == PropertyName._pathButton)
		{
			value = VariantUtils.CreateFrom(in _pathButton);
			return true;
		}
		if (name == PropertyName._polygonButton)
		{
			value = VariantUtils.CreateFrom(in _polygonButton);
			return true;
		}
		if (name == PropertyName._localSpaceButton)
		{
			value = VariantUtils.CreateFrom(in _localSpaceButton);
			return true;
		}
		if (name == PropertyName._smartSnapButton)
		{
			value = VariantUtils.CreateFrom(in _smartSnapButton);
			return true;
		}
		if (name == PropertyName._gridSnapButton)
		{
			value = VariantUtils.CreateFrom(in _gridSnapButton);
			return true;
		}
		if (name == PropertyName._lockButton)
		{
			value = VariantUtils.CreateFrom(in _lockButton);
			return true;
		}
		if (name == PropertyName._unlockButton)
		{
			value = VariantUtils.CreateFrom(in _unlockButton);
			return true;
		}
		if (name == PropertyName._groupButton)
		{
			value = VariantUtils.CreateFrom(in _groupButton);
			return true;
		}
		if (name == PropertyName._ungroupButton)
		{
			value = VariantUtils.CreateFrom(in _ungroupButton);
			return true;
		}
		if (name == PropertyName._centerViewButton)
		{
			value = VariantUtils.CreateFrom(in _centerViewButton);
			return true;
		}
		if (name == PropertyName._zoomOutButton)
		{
			value = VariantUtils.CreateFrom(in _zoomOutButton);
			return true;
		}
		if (name == PropertyName._zoomResetButton)
		{
			value = VariantUtils.CreateFrom(in _zoomResetButton);
			return true;
		}
		if (name == PropertyName._zoomInButton)
		{
			value = VariantUtils.CreateFrom(in _zoomInButton);
			return true;
		}
		if (name == PropertyName._snapConfigMenu)
		{
			value = VariantUtils.CreateFrom(in _snapConfigMenu);
			return true;
		}
		if (name == PropertyName._skeletonMenu)
		{
			value = VariantUtils.CreateFrom(in _skeletonMenu);
			return true;
		}
		if (name == PropertyName._viewMenu)
		{
			value = VariantUtils.CreateFrom(in _viewMenu);
			return true;
		}
		if (name == PropertyName._viewportContextMenu)
		{
			value = VariantUtils.CreateFrom(in _viewportContextMenu);
			return true;
		}
		if (name == PropertyName._workspaceTabs)
		{
			value = VariantUtils.CreateFrom(in _workspaceTabs);
			return true;
		}
		if (name == PropertyName._allPropertiesButton)
		{
			value = VariantUtils.CreateFrom(in _allPropertiesButton);
			return true;
		}
		if (name == PropertyName._nodeDirectPropertySurface)
		{
			value = VariantUtils.CreateFrom(in _nodeDirectPropertySurface);
			return true;
		}
		if (name == PropertyName._transformDirectPanel)
		{
			value = VariantUtils.CreateFrom(in _transformDirectPanel);
			return true;
		}
		if (name == PropertyName._transformSelectionLabel)
		{
			value = VariantUtils.CreateFrom(in _transformSelectionLabel);
			return true;
		}
		if (name == PropertyName._controlFields)
		{
			value = VariantUtils.CreateFrom(in _controlFields);
			return true;
		}
		if (name == PropertyName._positionXSpin)
		{
			value = VariantUtils.CreateFrom(in _positionXSpin);
			return true;
		}
		if (name == PropertyName._positionYSpin)
		{
			value = VariantUtils.CreateFrom(in _positionYSpin);
			return true;
		}
		if (name == PropertyName._rotationSpin)
		{
			value = VariantUtils.CreateFrom(in _rotationSpin);
			return true;
		}
		if (name == PropertyName._scaleXSpin)
		{
			value = VariantUtils.CreateFrom(in _scaleXSpin);
			return true;
		}
		if (name == PropertyName._scaleYSpin)
		{
			value = VariantUtils.CreateFrom(in _scaleYSpin);
			return true;
		}
		if (name == PropertyName._sizeXSpin)
		{
			value = VariantUtils.CreateFrom(in _sizeXSpin);
			return true;
		}
		if (name == PropertyName._sizeYSpin)
		{
			value = VariantUtils.CreateFrom(in _sizeYSpin);
			return true;
		}
		if (name == PropertyName._pivotXSpin)
		{
			value = VariantUtils.CreateFrom(in _pivotXSpin);
			return true;
		}
		if (name == PropertyName._pivotYSpin)
		{
			value = VariantUtils.CreateFrom(in _pivotYSpin);
			return true;
		}
		if (name == PropertyName._currentTabKey)
		{
			value = VariantUtils.CreateFrom(in _currentTabKey);
			return true;
		}
		if (name == PropertyName._isSwitchingTab)
		{
			value = VariantUtils.CreateFrom(in _isSwitchingTab);
			return true;
		}
		if (name == PropertyName._contextMenuWorldPosition)
		{
			value = VariantUtils.CreateFrom(in _contextMenuWorldPosition);
			return true;
		}
		if (name == PropertyName._directTransformNode)
		{
			value = VariantUtils.CreateFrom(in _directTransformNode);
			return true;
		}
		if (name == PropertyName._directTransformSessionActive)
		{
			value = VariantUtils.CreateFrom(in _directTransformSessionActive);
			return true;
		}
		if (name == PropertyName._updatingDirectTransformSurface)
		{
			value = VariantUtils.CreateFrom(in _updatingDirectTransformSurface);
			return true;
		}
		if (name == PropertyName._sceneRootHistoryViewportState)
		{
			value = VariantUtils.CreateFrom(in _sceneRootHistoryViewportState);
			return true;
		}
		if (name == PropertyName._sceneHistoryManager)
		{
			value = VariantUtils.CreateFrom(in _sceneHistoryManager);
			return true;
		}
		if (name == PropertyName._unsavedSceneCloseDialog)
		{
			value = VariantUtils.CreateFrom(in _unsavedSceneCloseDialog);
			return true;
		}
		if (name == PropertyName._discardUnsavedSceneButton)
		{
			value = VariantUtils.CreateFrom(in _discardUnsavedSceneButton);
			return true;
		}
		if (name == PropertyName._pendingCloseTabKey)
		{
			value = VariantUtils.CreateFrom(in _pendingCloseTabKey);
			return true;
		}
		if (name == PropertyName._gridSettingsPopup)
		{
			value = VariantUtils.CreateFrom(in _gridSettingsPopup);
			return true;
		}
		if (name == PropertyName._gridSettingsPanel)
		{
			value = VariantUtils.CreateFrom(in _gridSettingsPanel);
			return true;
		}
		if (name == PropertyName._gridSettingsSaveTimer)
		{
			value = VariantUtils.CreateFrom(in _gridSettingsSaveTimer);
			return true;
		}
		if (name == PropertyName._inlineTextOverlay)
		{
			value = VariantUtils.CreateFrom(in _inlineTextOverlay);
			return true;
		}
		if (name == PropertyName._inlineTextModeLabel)
		{
			value = VariantUtils.CreateFrom(in _inlineTextModeLabel);
			return true;
		}
		if (name == PropertyName._inlineTextLineEdit)
		{
			value = VariantUtils.CreateFrom(in _inlineTextLineEdit);
			return true;
		}
		if (name == PropertyName._inlineTextMultilineEdit)
		{
			value = VariantUtils.CreateFrom(in _inlineTextMultilineEdit);
			return true;
		}
		if (name == PropertyName._inlineTextCommitButton)
		{
			value = VariantUtils.CreateFrom(in _inlineTextCommitButton);
			return true;
		}
		if (name == PropertyName._inlineTextCancelButton)
		{
			value = VariantUtils.CreateFrom(in _inlineTextCancelButton);
			return true;
		}
		if (name == PropertyName._inlineTextOverlayMotion)
		{
			value = VariantUtils.CreateFrom(in _inlineTextOverlayMotion);
			return true;
		}
		if (name == PropertyName._inlineTextTarget)
		{
			value = VariantUtils.CreateFrom(in _inlineTextTarget);
			return true;
		}
		if (name == PropertyName._inlineTextProperty)
		{
			value = VariantUtils.CreateFrom(in _inlineTextProperty);
			return true;
		}
		if (name == PropertyName._inlineTextValueType)
		{
			value = VariantUtils.CreateFrom(in _inlineTextValueType);
			return true;
		}
		if (name == PropertyName._inlineTextHint)
		{
			value = VariantUtils.CreateFrom(in _inlineTextHint);
			return true;
		}
		if (name == PropertyName._inlineTextBefore)
		{
			value = VariantUtils.CreateFrom(in _inlineTextBefore);
			return true;
		}
		if (name == PropertyName._inlineTextHistoryId)
		{
			value = VariantUtils.CreateFrom(in _inlineTextHistoryId);
			return true;
		}
		if (name == PropertyName._inlineTextSessionToken)
		{
			value = VariantUtils.CreateFrom(in _inlineTextSessionToken);
			return true;
		}
		if (name == PropertyName._inlineTextMultiline)
		{
			value = VariantUtils.CreateFrom(in _inlineTextMultiline);
			return true;
		}
		if (name == PropertyName._inlineTextApplyingPreview)
		{
			value = VariantUtils.CreateFrom(in _inlineTextApplyingPreview);
			return true;
		}
		if (name == PropertyName._inlineTextGeometryRefreshQueued)
		{
			value = VariantUtils.CreateFrom(in _inlineTextGeometryRefreshQueued);
			return true;
		}
		if (name == PropertyName._controlLayoutPanel)
		{
			value = VariantUtils.CreateFrom(in _controlLayoutPanel);
			return true;
		}
		if (name == PropertyName._anchorLeftSpin)
		{
			value = VariantUtils.CreateFrom(in _anchorLeftSpin);
			return true;
		}
		if (name == PropertyName._anchorTopSpin)
		{
			value = VariantUtils.CreateFrom(in _anchorTopSpin);
			return true;
		}
		if (name == PropertyName._anchorRightSpin)
		{
			value = VariantUtils.CreateFrom(in _anchorRightSpin);
			return true;
		}
		if (name == PropertyName._anchorBottomSpin)
		{
			value = VariantUtils.CreateFrom(in _anchorBottomSpin);
			return true;
		}
		if (name == PropertyName._offsetLeftSpin)
		{
			value = VariantUtils.CreateFrom(in _offsetLeftSpin);
			return true;
		}
		if (name == PropertyName._offsetTopSpin)
		{
			value = VariantUtils.CreateFrom(in _offsetTopSpin);
			return true;
		}
		if (name == PropertyName._offsetRightSpin)
		{
			value = VariantUtils.CreateFrom(in _offsetRightSpin);
			return true;
		}
		if (name == PropertyName._offsetBottomSpin)
		{
			value = VariantUtils.CreateFrom(in _offsetBottomSpin);
			return true;
		}
		if (name == PropertyName._growHorizontalOption)
		{
			value = VariantUtils.CreateFrom(in _growHorizontalOption);
			return true;
		}
		if (name == PropertyName._growVerticalOption)
		{
			value = VariantUtils.CreateFrom(in _growVerticalOption);
			return true;
		}
		if (name == PropertyName._growHorizontalButtons)
		{
			GodotObject[] growHorizontalButtons = _growHorizontalButtons;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(growHorizontalButtons);
			return true;
		}
		if (name == PropertyName._growVerticalButtons)
		{
			GodotObject[] growHorizontalButtons = _growVerticalButtons;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(growHorizontalButtons);
			return true;
		}
		if (name == PropertyName._presetTopLeftButton)
		{
			value = VariantUtils.CreateFrom(in _presetTopLeftButton);
			return true;
		}
		if (name == PropertyName._presetCenterButton)
		{
			value = VariantUtils.CreateFrom(in _presetCenterButton);
			return true;
		}
		if (name == PropertyName._presetFullRectButton)
		{
			value = VariantUtils.CreateFrom(in _presetFullRectButton);
			return true;
		}
		if (name == PropertyName._presetHCenterWideButton)
		{
			value = VariantUtils.CreateFrom(in _presetHCenterWideButton);
			return true;
		}
		if (name == PropertyName._presetVCenterWideButton)
		{
			value = VariantUtils.CreateFrom(in _presetVCenterWideButton);
			return true;
		}
		if (name == PropertyName._layoutControl)
		{
			value = VariantUtils.CreateFrom(in _layoutControl);
			return true;
		}
		if (name == PropertyName._layoutSessionActive)
		{
			value = VariantUtils.CreateFrom(in _layoutSessionActive);
			return true;
		}
		if (name == PropertyName._updatingLayoutSurface)
		{
			value = VariantUtils.CreateFrom(in _updatingLayoutSurface);
			return true;
		}
		if (name == PropertyName._selectionLayoutPanel)
		{
			value = VariantUtils.CreateFrom(in _selectionLayoutPanel);
			return true;
		}
		if (name == PropertyName._selectionLayoutLabel)
		{
			value = VariantUtils.CreateFrom(in _selectionLayoutLabel);
			return true;
		}
		if (name == PropertyName._selectionAlignButtons)
		{
			GodotObject[] growHorizontalButtons = _selectionAlignButtons;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(growHorizontalButtons);
			return true;
		}
		if (name == PropertyName._selectionDistributeButtons)
		{
			GodotObject[] growHorizontalButtons = _selectionDistributeButtons;
			value = VariantUtils.CreateFromSystemArrayOfGodotObject(growHorizontalButtons);
			return true;
		}
		if (name == PropertyName._spriteAtlasPanel)
		{
			value = VariantUtils.CreateFrom(in _spriteAtlasPanel);
			return true;
		}
		if (name == PropertyName._spriteAtlasButton)
		{
			value = VariantUtils.CreateFrom(in _spriteAtlasButton);
			return true;
		}
		if (name == PropertyName._spriteAtlasTabIndex)
		{
			value = VariantUtils.CreateFrom(in _spriteAtlasTabIndex);
			return true;
		}
		if (name == PropertyName._spriteAtlasSelectionConnected)
		{
			value = VariantUtils.CreateFrom(in _spriteAtlasSelectionConnected);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._animationTimelineButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._canvasTimelineSplit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sceneAnimationTimelinePanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._sceneAnimationSelectionConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sceneTabBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._viewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mousePositionLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zoomLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rotateButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scaleButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._listSelectButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pivotButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._panButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rulerButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pathButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._polygonButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localSpaceButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._smartSnapButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gridSnapButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lockButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlockButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._groupButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._ungroupButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._centerViewButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zoomOutButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zoomResetButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._zoomInButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._snapConfigMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._skeletonMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._viewMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._viewportContextMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._workspaceTabs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._allPropertiesButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nodeDirectPropertySurface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._transformDirectPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._transformSelectionLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._controlFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._positionXSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._positionYSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rotationSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scaleXSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scaleYSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sizeXSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sizeYSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pivotXSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pivotYSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._currentTabKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isSwitchingTab, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._contextMenuWorldPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._directTransformNode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._directTransformSessionActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingDirectTransformSurface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._sceneRootHistoryViewportState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sceneHistoryManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unsavedSceneCloseDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._discardUnsavedSceneButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingCloseTabKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CurrentPackedScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CurrentSceneInstance, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gridSettingsPopup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gridSettingsPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gridSettingsSaveTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineTextOverlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineTextModeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineTextLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineTextMultilineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineTextCommitButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineTextCancelButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineTextOverlayMotion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineTextTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName._inlineTextProperty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._inlineTextValueType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._inlineTextHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._inlineTextBefore, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._inlineTextHistoryId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._inlineTextSessionToken, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._inlineTextMultiline, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._inlineTextApplyingPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._inlineTextGeometryRefreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.InlineTextEditorVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.InlineTextTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.StringName, PropertyName.InlineTextProperty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.InlineTextValueType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.InlineTextHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.InlineTextDraft, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.InlineTextCommitCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.InlineTextCancelCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.InlineTextPreviewCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.InlineTextOverlayRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2, PropertyName.InlineTextInputRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._controlLayoutPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._anchorLeftSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._anchorTopSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._anchorRightSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._anchorBottomSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._offsetLeftSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._offsetTopSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._offsetRightSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._offsetBottomSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._growHorizontalOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._growVerticalOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._growHorizontalButtons, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._growVerticalButtons, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._presetTopLeftButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._presetCenterButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._presetFullRectButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._presetHCenterWideButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._presetVCenterWideButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._layoutControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._layoutSessionActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingLayoutSurface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectionLayoutPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectionLayoutLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._selectionAlignButtons, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Array, PropertyName._selectionDistributeButtons, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spriteAtlasPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spriteAtlasButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._spriteAtlasTabIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._spriteAtlasSelectionConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.SpriteAtlasPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.CurrentPackedScene, Variant.From<PackedScene>(CurrentPackedScene));
		info.AddProperty(PropertyName.CurrentSceneInstance, Variant.From<Node>(CurrentSceneInstance));
		info.AddProperty(PropertyName.InlineTextCommitCount, Variant.From<int>(InlineTextCommitCount));
		info.AddProperty(PropertyName.InlineTextCancelCount, Variant.From<int>(InlineTextCancelCount));
		info.AddProperty(PropertyName.InlineTextPreviewCount, Variant.From<int>(InlineTextPreviewCount));
		info.AddProperty(PropertyName._animationTimelineButton, Variant.From(in _animationTimelineButton));
		info.AddProperty(PropertyName._canvasTimelineSplit, Variant.From(in _canvasTimelineSplit));
		info.AddProperty(PropertyName._sceneAnimationTimelinePanel, Variant.From(in _sceneAnimationTimelinePanel));
		info.AddProperty(PropertyName._sceneAnimationSelectionConnected, Variant.From(in _sceneAnimationSelectionConnected));
		info.AddProperty(PropertyName._sceneTabBar, Variant.From(in _sceneTabBar));
		info.AddProperty(PropertyName._viewport, Variant.From(in _viewport));
		info.AddProperty(PropertyName._mousePositionLabel, Variant.From(in _mousePositionLabel));
		info.AddProperty(PropertyName._zoomLabel, Variant.From(in _zoomLabel));
		info.AddProperty(PropertyName._selectButton, Variant.From(in _selectButton));
		info.AddProperty(PropertyName._moveButton, Variant.From(in _moveButton));
		info.AddProperty(PropertyName._rotateButton, Variant.From(in _rotateButton));
		info.AddProperty(PropertyName._scaleButton, Variant.From(in _scaleButton));
		info.AddProperty(PropertyName._listSelectButton, Variant.From(in _listSelectButton));
		info.AddProperty(PropertyName._pivotButton, Variant.From(in _pivotButton));
		info.AddProperty(PropertyName._panButton, Variant.From(in _panButton));
		info.AddProperty(PropertyName._rulerButton, Variant.From(in _rulerButton));
		info.AddProperty(PropertyName._pathButton, Variant.From(in _pathButton));
		info.AddProperty(PropertyName._polygonButton, Variant.From(in _polygonButton));
		info.AddProperty(PropertyName._localSpaceButton, Variant.From(in _localSpaceButton));
		info.AddProperty(PropertyName._smartSnapButton, Variant.From(in _smartSnapButton));
		info.AddProperty(PropertyName._gridSnapButton, Variant.From(in _gridSnapButton));
		info.AddProperty(PropertyName._lockButton, Variant.From(in _lockButton));
		info.AddProperty(PropertyName._unlockButton, Variant.From(in _unlockButton));
		info.AddProperty(PropertyName._groupButton, Variant.From(in _groupButton));
		info.AddProperty(PropertyName._ungroupButton, Variant.From(in _ungroupButton));
		info.AddProperty(PropertyName._centerViewButton, Variant.From(in _centerViewButton));
		info.AddProperty(PropertyName._zoomOutButton, Variant.From(in _zoomOutButton));
		info.AddProperty(PropertyName._zoomResetButton, Variant.From(in _zoomResetButton));
		info.AddProperty(PropertyName._zoomInButton, Variant.From(in _zoomInButton));
		info.AddProperty(PropertyName._snapConfigMenu, Variant.From(in _snapConfigMenu));
		info.AddProperty(PropertyName._skeletonMenu, Variant.From(in _skeletonMenu));
		info.AddProperty(PropertyName._viewMenu, Variant.From(in _viewMenu));
		info.AddProperty(PropertyName._viewportContextMenu, Variant.From(in _viewportContextMenu));
		info.AddProperty(PropertyName._workspaceTabs, Variant.From(in _workspaceTabs));
		info.AddProperty(PropertyName._allPropertiesButton, Variant.From(in _allPropertiesButton));
		info.AddProperty(PropertyName._nodeDirectPropertySurface, Variant.From(in _nodeDirectPropertySurface));
		info.AddProperty(PropertyName._transformDirectPanel, Variant.From(in _transformDirectPanel));
		info.AddProperty(PropertyName._transformSelectionLabel, Variant.From(in _transformSelectionLabel));
		info.AddProperty(PropertyName._controlFields, Variant.From(in _controlFields));
		info.AddProperty(PropertyName._positionXSpin, Variant.From(in _positionXSpin));
		info.AddProperty(PropertyName._positionYSpin, Variant.From(in _positionYSpin));
		info.AddProperty(PropertyName._rotationSpin, Variant.From(in _rotationSpin));
		info.AddProperty(PropertyName._scaleXSpin, Variant.From(in _scaleXSpin));
		info.AddProperty(PropertyName._scaleYSpin, Variant.From(in _scaleYSpin));
		info.AddProperty(PropertyName._sizeXSpin, Variant.From(in _sizeXSpin));
		info.AddProperty(PropertyName._sizeYSpin, Variant.From(in _sizeYSpin));
		info.AddProperty(PropertyName._pivotXSpin, Variant.From(in _pivotXSpin));
		info.AddProperty(PropertyName._pivotYSpin, Variant.From(in _pivotYSpin));
		info.AddProperty(PropertyName._currentTabKey, Variant.From(in _currentTabKey));
		info.AddProperty(PropertyName._isSwitchingTab, Variant.From(in _isSwitchingTab));
		info.AddProperty(PropertyName._contextMenuWorldPosition, Variant.From(in _contextMenuWorldPosition));
		info.AddProperty(PropertyName._directTransformNode, Variant.From(in _directTransformNode));
		info.AddProperty(PropertyName._directTransformSessionActive, Variant.From(in _directTransformSessionActive));
		info.AddProperty(PropertyName._updatingDirectTransformSurface, Variant.From(in _updatingDirectTransformSurface));
		info.AddProperty(PropertyName._sceneRootHistoryViewportState, Variant.From(in _sceneRootHistoryViewportState));
		info.AddProperty(PropertyName._sceneHistoryManager, Variant.From(in _sceneHistoryManager));
		info.AddProperty(PropertyName._unsavedSceneCloseDialog, Variant.From(in _unsavedSceneCloseDialog));
		info.AddProperty(PropertyName._discardUnsavedSceneButton, Variant.From(in _discardUnsavedSceneButton));
		info.AddProperty(PropertyName._pendingCloseTabKey, Variant.From(in _pendingCloseTabKey));
		info.AddProperty(PropertyName._gridSettingsPopup, Variant.From(in _gridSettingsPopup));
		info.AddProperty(PropertyName._gridSettingsPanel, Variant.From(in _gridSettingsPanel));
		info.AddProperty(PropertyName._gridSettingsSaveTimer, Variant.From(in _gridSettingsSaveTimer));
		info.AddProperty(PropertyName._inlineTextOverlay, Variant.From(in _inlineTextOverlay));
		info.AddProperty(PropertyName._inlineTextModeLabel, Variant.From(in _inlineTextModeLabel));
		info.AddProperty(PropertyName._inlineTextLineEdit, Variant.From(in _inlineTextLineEdit));
		info.AddProperty(PropertyName._inlineTextMultilineEdit, Variant.From(in _inlineTextMultilineEdit));
		info.AddProperty(PropertyName._inlineTextCommitButton, Variant.From(in _inlineTextCommitButton));
		info.AddProperty(PropertyName._inlineTextCancelButton, Variant.From(in _inlineTextCancelButton));
		info.AddProperty(PropertyName._inlineTextOverlayMotion, Variant.From(in _inlineTextOverlayMotion));
		info.AddProperty(PropertyName._inlineTextTarget, Variant.From(in _inlineTextTarget));
		info.AddProperty(PropertyName._inlineTextProperty, Variant.From(in _inlineTextProperty));
		info.AddProperty(PropertyName._inlineTextValueType, Variant.From(in _inlineTextValueType));
		info.AddProperty(PropertyName._inlineTextHint, Variant.From(in _inlineTextHint));
		info.AddProperty(PropertyName._inlineTextBefore, Variant.From(in _inlineTextBefore));
		info.AddProperty(PropertyName._inlineTextHistoryId, Variant.From(in _inlineTextHistoryId));
		info.AddProperty(PropertyName._inlineTextSessionToken, Variant.From(in _inlineTextSessionToken));
		info.AddProperty(PropertyName._inlineTextMultiline, Variant.From(in _inlineTextMultiline));
		info.AddProperty(PropertyName._inlineTextApplyingPreview, Variant.From(in _inlineTextApplyingPreview));
		info.AddProperty(PropertyName._inlineTextGeometryRefreshQueued, Variant.From(in _inlineTextGeometryRefreshQueued));
		info.AddProperty(PropertyName._controlLayoutPanel, Variant.From(in _controlLayoutPanel));
		info.AddProperty(PropertyName._anchorLeftSpin, Variant.From(in _anchorLeftSpin));
		info.AddProperty(PropertyName._anchorTopSpin, Variant.From(in _anchorTopSpin));
		info.AddProperty(PropertyName._anchorRightSpin, Variant.From(in _anchorRightSpin));
		info.AddProperty(PropertyName._anchorBottomSpin, Variant.From(in _anchorBottomSpin));
		info.AddProperty(PropertyName._offsetLeftSpin, Variant.From(in _offsetLeftSpin));
		info.AddProperty(PropertyName._offsetTopSpin, Variant.From(in _offsetTopSpin));
		info.AddProperty(PropertyName._offsetRightSpin, Variant.From(in _offsetRightSpin));
		info.AddProperty(PropertyName._offsetBottomSpin, Variant.From(in _offsetBottomSpin));
		info.AddProperty(PropertyName._growHorizontalOption, Variant.From(in _growHorizontalOption));
		info.AddProperty(PropertyName._growVerticalOption, Variant.From(in _growVerticalOption));
		StringName growHorizontalButtons = PropertyName._growHorizontalButtons;
		GodotObject[] growHorizontalButtons2 = _growHorizontalButtons;
		info.AddProperty(growHorizontalButtons, Variant.CreateFrom(growHorizontalButtons2));
		StringName growVerticalButtons = PropertyName._growVerticalButtons;
		growHorizontalButtons2 = _growVerticalButtons;
		info.AddProperty(growVerticalButtons, Variant.CreateFrom(growHorizontalButtons2));
		info.AddProperty(PropertyName._presetTopLeftButton, Variant.From(in _presetTopLeftButton));
		info.AddProperty(PropertyName._presetCenterButton, Variant.From(in _presetCenterButton));
		info.AddProperty(PropertyName._presetFullRectButton, Variant.From(in _presetFullRectButton));
		info.AddProperty(PropertyName._presetHCenterWideButton, Variant.From(in _presetHCenterWideButton));
		info.AddProperty(PropertyName._presetVCenterWideButton, Variant.From(in _presetVCenterWideButton));
		info.AddProperty(PropertyName._layoutControl, Variant.From(in _layoutControl));
		info.AddProperty(PropertyName._layoutSessionActive, Variant.From(in _layoutSessionActive));
		info.AddProperty(PropertyName._updatingLayoutSurface, Variant.From(in _updatingLayoutSurface));
		info.AddProperty(PropertyName._selectionLayoutPanel, Variant.From(in _selectionLayoutPanel));
		info.AddProperty(PropertyName._selectionLayoutLabel, Variant.From(in _selectionLayoutLabel));
		StringName selectionAlignButtons = PropertyName._selectionAlignButtons;
		growHorizontalButtons2 = _selectionAlignButtons;
		info.AddProperty(selectionAlignButtons, Variant.CreateFrom(growHorizontalButtons2));
		StringName selectionDistributeButtons = PropertyName._selectionDistributeButtons;
		growHorizontalButtons2 = _selectionDistributeButtons;
		info.AddProperty(selectionDistributeButtons, Variant.CreateFrom(growHorizontalButtons2));
		info.AddProperty(PropertyName._spriteAtlasPanel, Variant.From(in _spriteAtlasPanel));
		info.AddProperty(PropertyName._spriteAtlasButton, Variant.From(in _spriteAtlasButton));
		info.AddProperty(PropertyName._spriteAtlasTabIndex, Variant.From(in _spriteAtlasTabIndex));
		info.AddProperty(PropertyName._spriteAtlasSelectionConnected, Variant.From(in _spriteAtlasSelectionConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.CurrentPackedScene, out var value))
		{
			CurrentPackedScene = value.As<PackedScene>();
		}
		if (info.TryGetProperty(PropertyName.CurrentSceneInstance, out var value2))
		{
			CurrentSceneInstance = value2.As<Node>();
		}
		if (info.TryGetProperty(PropertyName.InlineTextCommitCount, out var value3))
		{
			InlineTextCommitCount = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.InlineTextCancelCount, out var value4))
		{
			InlineTextCancelCount = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName.InlineTextPreviewCount, out var value5))
		{
			InlineTextPreviewCount = value5.As<int>();
		}
		if (info.TryGetProperty(PropertyName._animationTimelineButton, out var value6))
		{
			_animationTimelineButton = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._canvasTimelineSplit, out var value7))
		{
			_canvasTimelineSplit = value7.As<VSplitContainer>();
		}
		if (info.TryGetProperty(PropertyName._sceneAnimationTimelinePanel, out var value8))
		{
			_sceneAnimationTimelinePanel = value8.As<XW2DAnimationTimelinePanel>();
		}
		if (info.TryGetProperty(PropertyName._sceneAnimationSelectionConnected, out var value9))
		{
			_sceneAnimationSelectionConnected = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sceneTabBar, out var value10))
		{
			_sceneTabBar = value10.As<TabBar>();
		}
		if (info.TryGetProperty(PropertyName._viewport, out var value11))
		{
			_viewport = value11.As<XW2DViewport>();
		}
		if (info.TryGetProperty(PropertyName._mousePositionLabel, out var value12))
		{
			_mousePositionLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._zoomLabel, out var value13))
		{
			_zoomLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._selectButton, out var value14))
		{
			_selectButton = value14.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveButton, out var value15))
		{
			_moveButton = value15.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._rotateButton, out var value16))
		{
			_rotateButton = value16.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._scaleButton, out var value17))
		{
			_scaleButton = value17.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._listSelectButton, out var value18))
		{
			_listSelectButton = value18.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pivotButton, out var value19))
		{
			_pivotButton = value19.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._panButton, out var value20))
		{
			_panButton = value20.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._rulerButton, out var value21))
		{
			_rulerButton = value21.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pathButton, out var value22))
		{
			_pathButton = value22.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._polygonButton, out var value23))
		{
			_polygonButton = value23.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._localSpaceButton, out var value24))
		{
			_localSpaceButton = value24.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._smartSnapButton, out var value25))
		{
			_smartSnapButton = value25.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._gridSnapButton, out var value26))
		{
			_gridSnapButton = value26.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._lockButton, out var value27))
		{
			_lockButton = value27.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._unlockButton, out var value28))
		{
			_unlockButton = value28.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._groupButton, out var value29))
		{
			_groupButton = value29.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._ungroupButton, out var value30))
		{
			_ungroupButton = value30.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._centerViewButton, out var value31))
		{
			_centerViewButton = value31.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._zoomOutButton, out var value32))
		{
			_zoomOutButton = value32.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._zoomResetButton, out var value33))
		{
			_zoomResetButton = value33.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._zoomInButton, out var value34))
		{
			_zoomInButton = value34.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._snapConfigMenu, out var value35))
		{
			_snapConfigMenu = value35.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._skeletonMenu, out var value36))
		{
			_skeletonMenu = value36.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._viewMenu, out var value37))
		{
			_viewMenu = value37.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._viewportContextMenu, out var value38))
		{
			_viewportContextMenu = value38.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._workspaceTabs, out var value39))
		{
			_workspaceTabs = value39.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._allPropertiesButton, out var value40))
		{
			_allPropertiesButton = value40.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nodeDirectPropertySurface, out var value41))
		{
			_nodeDirectPropertySurface = value41.As<XWDirectPropertySurface>();
		}
		if (info.TryGetProperty(PropertyName._transformDirectPanel, out var value42))
		{
			_transformDirectPanel = value42.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._transformSelectionLabel, out var value43))
		{
			_transformSelectionLabel = value43.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._controlFields, out var value44))
		{
			_controlFields = value44.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._positionXSpin, out var value45))
		{
			_positionXSpin = value45.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._positionYSpin, out var value46))
		{
			_positionYSpin = value46.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._rotationSpin, out var value47))
		{
			_rotationSpin = value47.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._scaleXSpin, out var value48))
		{
			_scaleXSpin = value48.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._scaleYSpin, out var value49))
		{
			_scaleYSpin = value49.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._sizeXSpin, out var value50))
		{
			_sizeXSpin = value50.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._sizeYSpin, out var value51))
		{
			_sizeYSpin = value51.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._pivotXSpin, out var value52))
		{
			_pivotXSpin = value52.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._pivotYSpin, out var value53))
		{
			_pivotYSpin = value53.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._currentTabKey, out var value54))
		{
			_currentTabKey = value54.As<string>();
		}
		if (info.TryGetProperty(PropertyName._isSwitchingTab, out var value55))
		{
			_isSwitchingTab = value55.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._contextMenuWorldPosition, out var value56))
		{
			_contextMenuWorldPosition = value56.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._directTransformNode, out var value57))
		{
			_directTransformNode = value57.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._directTransformSessionActive, out var value58))
		{
			_directTransformSessionActive = value58.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._updatingDirectTransformSurface, out var value59))
		{
			_updatingDirectTransformSurface = value59.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._sceneRootHistoryViewportState, out var value60))
		{
			_sceneRootHistoryViewportState = value60.As<Dictionary>();
		}
		if (info.TryGetProperty(PropertyName._sceneHistoryManager, out var value61))
		{
			_sceneHistoryManager = value61.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._unsavedSceneCloseDialog, out var value62))
		{
			_unsavedSceneCloseDialog = value62.As<ConfirmationDialog>();
		}
		if (info.TryGetProperty(PropertyName._discardUnsavedSceneButton, out var value63))
		{
			_discardUnsavedSceneButton = value63.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._pendingCloseTabKey, out var value64))
		{
			_pendingCloseTabKey = value64.As<string>();
		}
		if (info.TryGetProperty(PropertyName._gridSettingsPopup, out var value65))
		{
			_gridSettingsPopup = value65.As<PopupPanel>();
		}
		if (info.TryGetProperty(PropertyName._gridSettingsPanel, out var value66))
		{
			_gridSettingsPanel = value66.As<XW2DGridSettingsPanel>();
		}
		if (info.TryGetProperty(PropertyName._gridSettingsSaveTimer, out var value67))
		{
			_gridSettingsSaveTimer = value67.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextOverlay, out var value68))
		{
			_inlineTextOverlay = value68.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextModeLabel, out var value69))
		{
			_inlineTextModeLabel = value69.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextLineEdit, out var value70))
		{
			_inlineTextLineEdit = value70.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextMultilineEdit, out var value71))
		{
			_inlineTextMultilineEdit = value71.As<TextEdit>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextCommitButton, out var value72))
		{
			_inlineTextCommitButton = value72.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextCancelButton, out var value73))
		{
			_inlineTextCancelButton = value73.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextOverlayMotion, out var value74))
		{
			_inlineTextOverlayMotion = value74.As<XWUiMotion>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextTarget, out var value75))
		{
			_inlineTextTarget = value75.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextProperty, out var value76))
		{
			_inlineTextProperty = value76.As<StringName>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextValueType, out var value77))
		{
			_inlineTextValueType = value77.As<Variant.Type>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextHint, out var value78))
		{
			_inlineTextHint = value78.As<PropertyHint>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextBefore, out var value79))
		{
			_inlineTextBefore = value79.As<string>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextHistoryId, out var value80))
		{
			_inlineTextHistoryId = value80.As<int>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextSessionToken, out var value81))
		{
			_inlineTextSessionToken = value81.As<int>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextMultiline, out var value82))
		{
			_inlineTextMultiline = value82.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextApplyingPreview, out var value83))
		{
			_inlineTextApplyingPreview = value83.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextGeometryRefreshQueued, out var value84))
		{
			_inlineTextGeometryRefreshQueued = value84.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._controlLayoutPanel, out var value85))
		{
			_controlLayoutPanel = value85.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._anchorLeftSpin, out var value86))
		{
			_anchorLeftSpin = value86.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._anchorTopSpin, out var value87))
		{
			_anchorTopSpin = value87.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._anchorRightSpin, out var value88))
		{
			_anchorRightSpin = value88.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._anchorBottomSpin, out var value89))
		{
			_anchorBottomSpin = value89.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._offsetLeftSpin, out var value90))
		{
			_offsetLeftSpin = value90.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._offsetTopSpin, out var value91))
		{
			_offsetTopSpin = value91.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._offsetRightSpin, out var value92))
		{
			_offsetRightSpin = value92.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._offsetBottomSpin, out var value93))
		{
			_offsetBottomSpin = value93.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._growHorizontalOption, out var value94))
		{
			_growHorizontalOption = value94.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._growVerticalOption, out var value95))
		{
			_growVerticalOption = value95.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._growHorizontalButtons, out var value96))
		{
			_growHorizontalButtons = value96.AsGodotObjectArray<Button>();
		}
		if (info.TryGetProperty(PropertyName._growVerticalButtons, out var value97))
		{
			_growVerticalButtons = value97.AsGodotObjectArray<Button>();
		}
		if (info.TryGetProperty(PropertyName._presetTopLeftButton, out var value98))
		{
			_presetTopLeftButton = value98.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._presetCenterButton, out var value99))
		{
			_presetCenterButton = value99.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._presetFullRectButton, out var value100))
		{
			_presetFullRectButton = value100.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._presetHCenterWideButton, out var value101))
		{
			_presetHCenterWideButton = value101.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._presetVCenterWideButton, out var value102))
		{
			_presetVCenterWideButton = value102.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._layoutControl, out var value103))
		{
			_layoutControl = value103.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._layoutSessionActive, out var value104))
		{
			_layoutSessionActive = value104.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._updatingLayoutSurface, out var value105))
		{
			_updatingLayoutSurface = value105.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._selectionLayoutPanel, out var value106))
		{
			_selectionLayoutPanel = value106.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._selectionLayoutLabel, out var value107))
		{
			_selectionLayoutLabel = value107.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._selectionAlignButtons, out var value108))
		{
			_selectionAlignButtons = value108.AsGodotObjectArray<Button>();
		}
		if (info.TryGetProperty(PropertyName._selectionDistributeButtons, out var value109))
		{
			_selectionDistributeButtons = value109.AsGodotObjectArray<Button>();
		}
		if (info.TryGetProperty(PropertyName._spriteAtlasPanel, out var value110))
		{
			_spriteAtlasPanel = value110.As<XW2DSpriteAtlasPanel>();
		}
		if (info.TryGetProperty(PropertyName._spriteAtlasButton, out var value111))
		{
			_spriteAtlasButton = value111.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._spriteAtlasTabIndex, out var value112))
		{
			_spriteAtlasTabIndex = value112.As<int>();
		}
		if (info.TryGetProperty(PropertyName._spriteAtlasSelectionConnected, out var value113))
		{
			_spriteAtlasSelectionConnected = value113.As<bool>();
		}
	}
}
