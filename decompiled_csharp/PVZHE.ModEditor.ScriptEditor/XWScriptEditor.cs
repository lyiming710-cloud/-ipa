using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using Microsoft.CodeAnalysis.CSharp;
using PVZHE.ModEditor.Blueprint;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Debugging;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.OutPutPanel;
using PVZHE.ModEditor.Registry.BP;
using PVZHE.ModEditor.Registry.Class;

namespace PVZHE.ModEditor.ScriptEditor;

[ScriptPath("res://addons/ModEditor/ScriptEditor/GUI/XWScriptEditor.cs")]
public class XWScriptEditor : PanelContainer
{
	private enum MenuFile
	{
		NewScript = 0,
		Sep1 = 1,
		Open = 3,
		Sep2 = 4,
		Save = 5,
		SaveAs = 6,
		SaveAll = 7,
		Sep3 = 8,
		Close = 9,
		CloseAll = 10,
		CloseOther = 11
	}

	private enum DiagnosticsContextMenu
	{
		CopySelected,
		CopyAll
	}

	private enum SearchMenu
	{
		Find,
		Replace,
		Next,
		Previous
	}

	private enum GoToMenu
	{
		Line,
		NextProblem,
		PreviousProblem
	}

	private enum ToolbarOverflowAction
	{
		RenameUndo = 0,
		RenameRedo = 1,
		Format = 3,
		Validate = 4,
		PreviousProblem = 6,
		NextProblem = 7,
		OpenBlueprint = 9
	}

	private readonly struct SearchMatch(int line, int column)
	{
		public int Line { get; } = line;

		public int Column { get; } = column;
	}

	private class ScriptTabInfo
	{
		public string FilePath = "";

		public string Text = "";

		public int CursorLine;

		public int CursorColumn;

		public int ScrollVertical;

		public bool HasUnsavedChanges;

		public string FileExtension = "";

		public ulong ModifiedTime;

		public bool DiskExists;

		public string DiskContentHash = "";

		public long DiskByteLength;

		public bool HasExternalConflict;

		public bool ConflictDiskExists;

		public string ConflictDiskText = "";

		public string ConflictDiskContentHash = "";

		public long ConflictDiskByteLength;

		public ulong ConflictModifiedTime;

		public int[] Breakpoints = Array.Empty<int>();

		public XWBlueprintGeneratedCSharpPolicy.LinkState GeneratedLinkState;

		public string BlueprintSourcePath = "";

		public bool IsBlueprintGenerated => GeneratedLinkState != XWBlueprintGeneratedCSharpPolicy.LinkState.NotGenerated;
	}

	private sealed class DiagnosticListEntry
	{
		public XWCodeErrorChecker.ErrorData Diagnostic;

		public string FilePath = "";
	}

	public enum ScriptSaveStatus
	{
		Unchanged,
		Saved,
		Conflict,
		IoError,
		ReadOnly
	}

	private readonly struct ScriptDiskSnapshot(bool exists, string text, string contentHash, long byteLength, ulong modifiedTime)
	{
		public bool Exists { get; } = exists;

		public string Text { get; } = text ?? "";

		public string ContentHash { get; } = contentHash ?? "";

		public long ByteLength { get; } = byteLength;

		public ulong ModifiedTime { get; } = modifiedTime;
	}

	private enum AtomicWriteFailureKind
	{
		None,
		Conflict,
		ReadOnly,
		IoError
	}

	internal enum IdeRequestKind
	{
		Definition,
		References,
		HoverSignature,
		HoverPopup,
		RenameSymbol,
		RenamePreview
	}

	private readonly struct IdeRequestStamp(long contextRevision, string projectRoot, string filePath, string source, int offset)
	{
		public readonly long ContextRevision = contextRevision;

		public readonly string ProjectRoot = projectRoot;

		public readonly string FilePath = filePath;

		public readonly string Source = source;

		public readonly int Offset = offset;
	}

	private readonly struct IdeSymbolQueryResult(bool isCurrent, IdeRequestStamp stamp, XWCSharpIdeService.SymbolInfoResult info)
	{
		public readonly bool IsCurrent = isCurrent;

		public readonly IdeRequestStamp Stamp = stamp;

		public readonly XWCSharpIdeService.SymbolInfoResult Info = info;
	}

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public static readonly StringName LoadIcons = "LoadIcons";

		public static readonly StringName BuildFileMenu = "BuildFileMenu";

		public static readonly StringName BuildSearchMenu = "BuildSearchMenu";

		public static readonly StringName BuildGoToMenu = "BuildGoToMenu";

		public static readonly StringName BuildToolbarOverflowMenu = "BuildToolbarOverflowMenu";

		public static readonly StringName OnToolbarOverflowItem = "OnToolbarOverflowItem";

		public static readonly StringName OnEditorVisibilityChanged = "OnEditorVisibilityChanged";

		public static readonly StringName SetWorkspaceActive = "SetWorkspaceActive";

		public static readonly StringName ApplyWorkspaceActivity = "ApplyWorkspaceActivity";

		public static readonly StringName OnSearchMenuItem = "OnSearchMenuItem";

		public static readonly StringName OnGoToMenuItem = "OnGoToMenuItem";

		public static readonly StringName BuildDiagnosticsContextMenu = "BuildDiagnosticsContextMenu";

		public static readonly StringName OnDiagnosticsContextMenuItem = "OnDiagnosticsContextMenuItem";

		public static readonly StringName ConnectSignals = "ConnectSignals";

		public new static readonly StringName _UnhandledInput = "_UnhandledInput";

		public static readonly StringName OnFileMenuItem = "OnFileMenuItem";

		public static readonly StringName NewCSharpScript = "NewCSharpScript";

		public static readonly StringName OnNewFileSelected = "OnNewFileSelected";

		public static readonly StringName OpenFileDialog = "OpenFileDialog";

		public static readonly StringName OpenFile = "OpenFile";

		public static readonly StringName TryOpenFile = "TryOpenFile";

		public static readonly StringName OpenFileAt = "OpenFileAt";

		public static readonly StringName OpenGeneratedBlueprintFile = "OpenGeneratedBlueprintFile";

		public static readonly StringName TryOpenFileAt = "TryOpenFileAt";

		public static readonly StringName SaveCurrentTabState = "SaveCurrentTabState";

		public static readonly StringName LoadTabData = "LoadTabData";

		public static readonly StringName OnTabChanged = "OnTabChanged";

		public static readonly StringName OnTabClosePressed = "OnTabClosePressed";

		public static readonly StringName CloseTab = "CloseTab";

		public static readonly StringName ContinueCloseQueue = "ContinueCloseQueue";

		public static readonly StringName OnUnsavedCloseConfirmed = "OnUnsavedCloseConfirmed";

		public static readonly StringName OnUnsavedCloseCanceled = "OnUnsavedCloseCanceled";

		public static readonly StringName OnUnsavedCloseCustomAction = "OnUnsavedCloseCustomAction";

		public static readonly StringName CloseTabImmediately = "CloseTabImmediately";

		public static readonly StringName CloseCurrentTab = "CloseCurrentTab";

		public static readonly StringName CloseAllTabs = "CloseAllTabs";

		public static readonly StringName TrySwitchProjectRoot = "TrySwitchProjectRoot";

		public static readonly StringName RenewBuildLifetime = "RenewBuildLifetime";

		public static readonly StringName NormalizeProjectRoot = "NormalizeProjectRoot";

		public static readonly StringName CloseOtherTabs = "CloseOtherTabs";

		public static readonly StringName ClearEditor = "ClearEditor";

		public static readonly StringName SaveFile = "SaveFile";

		public static readonly StringName SaveTab = "SaveTab";

		public static readonly StringName SaveFileAs = "SaveFileAs";

		public static readonly StringName OnSaveAsFileSelected = "OnSaveAsFileSelected";

		public static readonly StringName SaveAllTabs = "SaveAllTabs";

		public static readonly StringName OnCodeChanged = "OnCodeChanged";

		public static readonly StringName ShowSearchBar = "ShowSearchBar";

		public static readonly StringName CloseSearchBar = "CloseSearchBar";

		public static readonly StringName RefreshSearchMatches = "RefreshSearchMatches";

		public static readonly StringName NavigateSearch = "NavigateSearch";

		public static readonly StringName ReplaceCurrentMatch = "ReplaceCurrentMatch";

		public static readonly StringName ReplaceAllMatches = "ReplaceAllMatches";

		public static readonly StringName FindNearestSearchMatch = "FindNearestSearchMatch";

		public static readonly StringName SelectActiveSearchMatch = "SelectActiveSearchMatch";

		public static readonly StringName UpdateSearchMatchLabel = "UpdateSearchMatchLabel";

		public static readonly StringName IsWholeWordMatch = "IsWholeWordMatch";

		public static readonly StringName IsIdentifierCharacter = "IsIdentifierCharacter";

		public static readonly StringName PositiveModulo = "PositiveModulo";

		public static readonly StringName ShowGoToLineDialog = "ShowGoToLineDialog";

		public static readonly StringName CommitGoToLine = "CommitGoToLine";

		public static readonly StringName OnCodeErrorsChanged = "OnCodeErrorsChanged";

		public static readonly StringName OnCodeDiagnosticsChanged = "OnCodeDiagnosticsChanged";

		public static readonly StringName OnCompileButtonPressed = "OnCompileButtonPressed";

		public static readonly StringName UnloadCallbackPreview = "UnloadCallbackPreview";

		public static readonly StringName ReportCallbackPreviewError = "ReportCallbackPreviewError";

		public static readonly StringName OnDiagnosticListItemSelected = "OnDiagnosticListItemSelected";

		public static readonly StringName OnDiagnosticListItemActivated = "OnDiagnosticListItemActivated";

		public static readonly StringName OnDiagnosticsListGuiInput = "OnDiagnosticsListGuiInput";

		public static readonly StringName CopySelectedDiagnostics = "CopySelectedDiagnostics";

		public static readonly StringName CopyAllDiagnostics = "CopyAllDiagnostics";

		public static readonly StringName JumpToDiagnosticItem = "JumpToDiagnosticItem";

		public static readonly StringName RequestCurrentValidation = "RequestCurrentValidation";

		public static readonly StringName JumpToNextDiagnostic = "JumpToNextDiagnostic";

		public static readonly StringName OpenBlueprintWorkspace = "OpenBlueprintWorkspace";

		public static readonly StringName ResolveOpenFilePath = "ResolveOpenFilePath";

		public static readonly StringName LocalizeProjectPath = "LocalizeProjectPath";

		public static readonly StringName UpdateCompileControls = "UpdateCompileControls";

		public static readonly StringName OnModBuildFlightStateChanged = "OnModBuildFlightStateChanged";

		public static readonly StringName OpenCurrentGeneratedBlueprintSource = "OpenCurrentGeneratedBlueprintSource";

		public static readonly StringName CanEditCurrentDocument = "CanEditCurrentDocument";

		public static readonly StringName FirstNonEmpty = "FirstNonEmpty";

		public static readonly StringName UpdateLineCol = "UpdateLineCol";

		public static readonly StringName UpdateTabTitle = "UpdateTabTitle";

		public static readonly StringName UpdateScriptList = "UpdateScriptList";

		public static readonly StringName UpdateScriptListSelection = "UpdateScriptListSelection";

		public static readonly StringName OnScriptFilterChanged = "OnScriptFilterChanged";

		public static readonly StringName OnScriptListItemSelected = "OnScriptListItemSelected";

		public static readonly StringName HasFileOpen = "HasFileOpen";

		public static readonly StringName GetCurrentFilePath = "GetCurrentFilePath";

		public static readonly StringName IsSupportedScriptPath = "IsSupportedScriptPath";

		public static readonly StringName NormalizeNewCSharpPath = "NormalizeNewCSharpPath";

		public static readonly StringName BuildDefaultCSharpTemplate = "BuildDefaultCSharpTemplate";

		public static readonly StringName RejectUnsupportedScriptPath = "RejectUnsupportedScriptPath";

		public static readonly StringName LoadScript = "LoadScript";

		public static readonly StringName InitializeDebugWorkbench = "InitializeDebugWorkbench";

		public static readonly StringName ShutdownDebugWorkbench = "ShutdownDebugWorkbench";

		public static readonly StringName SetDebugWorkbenchVisible = "SetDebugWorkbenchVisible";

		public static readonly StringName OnDebugWorkbenchStopRequested = "OnDebugWorkbenchStopRequested";

		public static readonly StringName ApplyScriptDebugNavigation = "ApplyScriptDebugNavigation";

		public static readonly StringName InitializeExternalConflictUI = "InitializeExternalConflictUI";

		public static readonly StringName ComputeScriptContentHash = "ComputeScriptContentHash";

		public static readonly StringName ResolveAbsoluteScriptPath = "ResolveAbsoluteScriptPath";

		public static readonly StringName SaveTabWithResult = "SaveTabWithResult";

		public static readonly StringName ReloadCurrentExternalConflict = "ReloadCurrentExternalConflict";

		public static readonly StringName OverwriteCurrentExternalConflict = "OverwriteCurrentExternalConflict";

		public static readonly StringName ShowCurrentExternalConflictDiff = "ShowCurrentExternalConflictDiff";

		public static readonly StringName ScanExternalChangesNowForProbe = "ScanExternalChangesNowForProbe";

		public static readonly StringName SaveCurrentFileAsForProbe = "SaveCurrentFileAsForProbe";

		public static readonly StringName SetExternalConflictWatcherActive = "SetExternalConflictWatcherActive";

		public static readonly StringName ScanExternalChangesFromWatcher = "ScanExternalChangesFromWatcher";

		public static readonly StringName SaveCurrentTabAsAtomic = "SaveCurrentTabAsAtomic";

		public static readonly StringName InitializeIdeFeatures = "InitializeIdeFeatures";

		public static readonly StringName SuspendIdeBackgroundWork = "SuspendIdeBackgroundWork";

		public static readonly StringName ResumeIdeBackgroundWork = "ResumeIdeBackgroundWork";

		public static readonly StringName HandleIdeShortcut = "HandleIdeShortcut";

		public static readonly StringName JumpToReference = "JumpToReference";

		public static readonly StringName OnCodeEditIdeGuiInput = "OnCodeEditIdeGuiInput";

		public static readonly StringName RequestHoverFromTimer = "RequestHoverFromTimer";

		public static readonly StringName RequestRenamePreview = "RequestRenamePreview";

		public static readonly StringName RunDebouncedRenamePreview = "RunDebouncedRenamePreview";

		public static readonly StringName FormatDocumentOrSelection = "FormatDocumentOrSelection";

		public static readonly StringName ApplyQuickFixAtCaret = "ApplyQuickFixAtCaret";

		public static readonly StringName ShowQuickFixMenu = "ShowQuickFixMenu";

		public static readonly StringName ApplyPendingQuickFix = "ApplyPendingQuickFix";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName UpdateDebugControls = "UpdateDebugControls";

		public static readonly StringName RefreshBuildActionState = "RefreshBuildActionState";

		public static readonly StringName BeginRenamePreviewRequest = "BeginRenamePreviewRequest";

		public static readonly StringName InvalidateIdeRequests = "InvalidateIdeRequests";

		public static readonly StringName NormalizeIdePath = "NormalizeIdePath";

		public static readonly StringName GetActiveModProjectRoot = "GetActiveModProjectRoot";

		public static readonly StringName ReloadOpenDocumentsFromDisk = "ReloadOpenDocumentsFromDisk";

		public static readonly StringName IsPathInsideRoot = "IsPathInsideRoot";

		public static readonly StringName SafeRelativePath = "SafeRelativePath";

		public static readonly StringName EscapeBbcode = "EscapeBbcode";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName CallbackPreviewOwnerId = "CallbackPreviewOwnerId";

		public static readonly StringName UnsupportedScriptRequestCount = "UnsupportedScriptRequestCount";

		public static readonly StringName UnsupportedSaveAsRequestCount = "UnsupportedSaveAsRequestCount";

		public static readonly StringName CSharpTemplateCreateCount = "CSharpTemplateCreateCount";

		public static readonly StringName IsHiddenWorkQuiescent = "IsHiddenWorkQuiescent";

		public static readonly StringName IsCurrentDocumentReadOnly = "IsCurrentDocumentReadOnly";

		public static readonly StringName CurrentGeneratedBlueprintSourcePath = "CurrentGeneratedBlueprintSourcePath";

		public static readonly StringName CurrentGeneratedLinkState = "CurrentGeneratedLinkState";

		public static readonly StringName HiddenWorkDiagnostic = "HiddenWorkDiagnostic";

		public static readonly StringName IsDebugWorkbenchBound = "IsDebugWorkbenchBound";

		public static readonly StringName IsDebugWorkbenchVisible = "IsDebugWorkbenchVisible";

		public static readonly StringName IsDebugWorkbenchHiddenProcessSuspended = "IsDebugWorkbenchHiddenProcessSuspended";

		public static readonly StringName DebugWorkbenchCallStackCount = "DebugWorkbenchCallStackCount";

		public static readonly StringName DebugWorkbenchVariableCount = "DebugWorkbenchVariableCount";

		public static readonly StringName LastSaveStatus = "LastSaveStatus";

		public static readonly StringName LastSaveError = "LastSaveError";

		public static readonly StringName ExternalConflictDetectedCount = "ExternalConflictDetectedCount";

		public static readonly StringName ExternalConflictReloadCount = "ExternalConflictReloadCount";

		public static readonly StringName ExternalConflictOverwriteCount = "ExternalConflictOverwriteCount";

		public static readonly StringName CleanExternalReloadCount = "CleanExternalReloadCount";

		public static readonly StringName AtomicSaveCount = "AtomicSaveCount";

		public static readonly StringName SaveAsRollbackCount = "SaveAsRollbackCount";

		public static readonly StringName CompileSaveGateCount = "CompileSaveGateCount";

		public static readonly StringName DebugSaveGateCount = "DebugSaveGateCount";

		public static readonly StringName CompileInvocationCount = "CompileInvocationCount";

		public static readonly StringName DebugStartInvocationCount = "DebugStartInvocationCount";

		public static readonly StringName ExternalConflictWatcherScanCount = "ExternalConflictWatcherScanCount";

		public static readonly StringName IsCurrentDocumentInExternalConflict = "IsCurrentDocumentInExternalConflict";

		public static readonly StringName IsExternalConflictBarVisible = "IsExternalConflictBarVisible";

		public static readonly StringName IsExternalConflictWatcherIdle = "IsExternalConflictWatcherIdle";

		public static readonly StringName CurrentDocumentText = "CurrentDocumentText";

		public static readonly StringName IsDebugSessionRunning = "IsDebugSessionRunning";

		public static readonly StringName HasLiveDebugSession = "HasLiveDebugSession";

		public static readonly StringName DebugEntryInvoked = "DebugEntryInvoked";

		public static readonly StringName DebugLoadedAssemblyCount = "DebugLoadedAssemblyCount";

		public static readonly StringName DebugEntryTaskCompleted = "DebugEntryTaskCompleted";

		public static readonly StringName DebugLoadContextAlive = "DebugLoadContextAlive";

		public static readonly StringName DebugPreviewChildCount = "DebugPreviewChildCount";

		public static readonly StringName DebugMainThreadId = "DebugMainThreadId";

		public static readonly StringName HasPendingIdeBackgroundWork = "HasPendingIdeBackgroundWork";

		public static readonly StringName _fileMenuBtn = "_fileMenuBtn";

		public static readonly StringName _searchMenuBtn = "_searchMenuBtn";

		public static readonly StringName _goToMenuBtn = "_goToMenuBtn";

		public static readonly StringName _overflowMenuBtn = "_overflowMenuBtn";

		public static readonly StringName _scriptNameLabel = "_scriptNameLabel";

		public static readonly StringName _scriptFilter = "_scriptFilter";

		public static readonly StringName _scriptList = "_scriptList";

		public static readonly StringName _scriptTabBar = "_scriptTabBar";

		public static readonly StringName _codeEdit = "_codeEdit";

		public static readonly StringName _lineColLabel = "_lineColLabel";

		public static readonly StringName _compileButton = "_compileButton";

		public static readonly StringName _validateButton = "_validateButton";

		public static readonly StringName _previousProblemButton = "_previousProblemButton";

		public static readonly StringName _nextProblemButton = "_nextProblemButton";

		public static readonly StringName _openBlueprintButton = "_openBlueprintButton";

		public static readonly StringName _generatedBlueprintBar = "_generatedBlueprintBar";

		public static readonly StringName _generatedBlueprintStatusLabel = "_generatedBlueprintStatusLabel";

		public static readonly StringName _generatedBlueprintSourceLabel = "_generatedBlueprintSourceLabel";

		public static readonly StringName _returnToBlueprintButton = "_returnToBlueprintButton";

		public static readonly StringName _diagnosticsLabel = "_diagnosticsLabel";

		public static readonly StringName _diagnosticsPanel = "_diagnosticsPanel";

		public static readonly StringName _diagnosticsList = "_diagnosticsList";

		public static readonly StringName _diagnosticsCopyButton = "_diagnosticsCopyButton";

		public static readonly StringName _diagnosticsContextMenu = "_diagnosticsContextMenu";

		public static readonly StringName _searchBar = "_searchBar";

		public static readonly StringName _searchInput = "_searchInput";

		public static readonly StringName _searchMatchLabel = "_searchMatchLabel";

		public static readonly StringName _searchPreviousButton = "_searchPreviousButton";

		public static readonly StringName _searchNextButton = "_searchNextButton";

		public static readonly StringName _searchCaseButton = "_searchCaseButton";

		public static readonly StringName _searchWholeButton = "_searchWholeButton";

		public static readonly StringName _searchCloseButton = "_searchCloseButton";

		public static readonly StringName _replaceRow = "_replaceRow";

		public static readonly StringName _replaceInput = "_replaceInput";

		public static readonly StringName _replaceNextButton = "_replaceNextButton";

		public static readonly StringName _replaceAllButton = "_replaceAllButton";

		public static readonly StringName _goToLineDialog = "_goToLineDialog";

		public static readonly StringName _goToLineSpin = "_goToLineSpin";

		public static readonly StringName _newFileDialog = "_newFileDialog";

		public static readonly StringName _openFileDialog = "_openFileDialog";

		public static readonly StringName _saveAsDialog = "_saveAsDialog";

		public static readonly StringName _unsavedChangesDialog = "_unsavedChangesDialog";

		public static readonly StringName _discardUnsavedButton = "_discardUnsavedButton";

		public static readonly StringName _currentTabKey = "_currentTabKey";

		public static readonly StringName _isSwitchingTab = "_isSwitchingTab";

		public static readonly StringName _isLoadingTab = "_isLoadingTab";

		public static readonly StringName _isCompiling = "_isCompiling";

		public static readonly StringName _isBulkReplacing = "_isBulkReplacing";

		public static readonly StringName _activeSearchMatch = "_activeSearchMatch";

		public static readonly StringName _pendingCloseTabKey = "_pendingCloseTabKey";

		public static readonly StringName _hiddenWorkSuspended = "_hiddenWorkSuspended";

		public static readonly StringName _workspaceSelected = "_workspaceSelected";

		public static readonly StringName _workspaceActive = "_workspaceActive";

		public static readonly StringName _activeProjectRoot = "_activeProjectRoot";

		public static readonly StringName _iconScript = "_iconScript";

		public static readonly StringName _iconSave = "_iconSave";

		public static readonly StringName _iconNew = "_iconNew";

		public static readonly StringName _iconFolder = "_iconFolder";

		public static readonly StringName _iconClose = "_iconClose";

		public static readonly StringName _iconBuildCSharp = "_iconBuildCSharp";

		public static readonly StringName _iconStatusError = "_iconStatusError";

		public static readonly StringName _iconStatusWarning = "_iconStatusWarning";

		public static readonly StringName _debugWorkbench = "_debugWorkbench";

		public static readonly StringName _debugWorkbenchToggle = "_debugWorkbenchToggle";

		public static readonly StringName _scriptDebugNavigationQueued = "_scriptDebugNavigationQueued";

		public static readonly StringName _externalConflictBar = "_externalConflictBar";

		public static readonly StringName _externalConflictTitleLabel = "_externalConflictTitleLabel";

		public static readonly StringName _externalConflictDetailLabel = "_externalConflictDetailLabel";

		public static readonly StringName _viewExternalDiffButton = "_viewExternalDiffButton";

		public static readonly StringName _reloadExternalButton = "_reloadExternalButton";

		public static readonly StringName _overwriteExternalButton = "_overwriteExternalButton";

		public static readonly StringName _saveConflictCopyButton = "_saveConflictCopyButton";

		public static readonly StringName _externalConflictDiffWindow = "_externalConflictDiffWindow";

		public static readonly StringName _externalConflictEditorText = "_externalConflictEditorText";

		public static readonly StringName _externalConflictDiskText = "_externalConflictDiskText";

		public static readonly StringName _externalConflictWatcher = "_externalConflictWatcher";

		public static readonly StringName _externalConflictWatcherScanning = "_externalConflictWatcherScanning";

		public static readonly StringName _definitionButton = "_definitionButton";

		public static readonly StringName _referencesButton = "_referencesButton";

		public static readonly StringName _renameButton = "_renameButton";

		public static readonly StringName _renameUndoButton = "_renameUndoButton";

		public static readonly StringName _renameRedoButton = "_renameRedoButton";

		public static readonly StringName _formatButton = "_formatButton";

		public static readonly StringName _quickFixButton = "_quickFixButton";

		public static readonly StringName _debugStartButton = "_debugStartButton";

		public static readonly StringName _debugStopButton = "_debugStopButton";

		public static readonly StringName _debugStatusLabel = "_debugStatusLabel";

		public static readonly StringName _referencesWindow = "_referencesWindow";

		public static readonly StringName _referencesHeader = "_referencesHeader";

		public static readonly StringName _referencesList = "_referencesList";

		public static readonly StringName _renameDialog = "_renameDialog";

		public static readonly StringName _renameInput = "_renameInput";

		public static readonly StringName _renamePreviewLabel = "_renamePreviewLabel";

		public static readonly StringName _symbolInfoPopup = "_symbolInfoPopup";

		public static readonly StringName _symbolInfoLabel = "_symbolInfoLabel";

		public static readonly StringName _quickFixPopup = "_quickFixPopup";

		public static readonly StringName _hoverTimer = "_hoverTimer";

		public static readonly StringName _renamePreviewTimer = "_renamePreviewTimer";

		public static readonly StringName _debugHost = "_debugHost";

		public static readonly StringName _debugPreviewRoot = "_debugPreviewRoot";

		public static readonly StringName _pendingRenameGeneration = "_pendingRenameGeneration";

		public static readonly StringName _hoverLocalPosition = "_hoverLocalPosition";

		public static readonly StringName _hoverLine = "_hoverLine";

		public static readonly StringName _hoverColumn = "_hoverColumn";

		public static readonly StringName _hoverGeneration = "_hoverGeneration";

		public static readonly StringName _renameGeneration = "_renameGeneration";

		public static readonly StringName _pendingRenameText = "_pendingRenameText";

		public static readonly StringName _ideContextRevision = "_ideContextRevision";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	public const string SupportedScriptExtension = "cs";

	private MenuButton _fileMenuBtn;

	private MenuButton _searchMenuBtn;

	private MenuButton _goToMenuBtn;

	private MenuButton _overflowMenuBtn;

	private Label _scriptNameLabel;

	private LineEdit _scriptFilter;

	private ItemList _scriptList;

	private TabBar _scriptTabBar;

	private XWCodeEdit _codeEdit;

	private Label _lineColLabel;

	private Button _compileButton;

	private Button _validateButton;

	private Button _previousProblemButton;

	private Button _nextProblemButton;

	private Button _openBlueprintButton;

	private PanelContainer _generatedBlueprintBar;

	private Label _generatedBlueprintStatusLabel;

	private Label _generatedBlueprintSourceLabel;

	private Button _returnToBlueprintButton;

	private Label _diagnosticsLabel;

	private PanelContainer _diagnosticsPanel;

	private ItemList _diagnosticsList;

	private Button _diagnosticsCopyButton;

	private PopupMenu _diagnosticsContextMenu;

	private PanelContainer _searchBar;

	private LineEdit _searchInput;

	private Label _searchMatchLabel;

	private Button _searchPreviousButton;

	private Button _searchNextButton;

	private CheckButton _searchCaseButton;

	private CheckButton _searchWholeButton;

	private Button _searchCloseButton;

	private Container _replaceRow;

	private LineEdit _replaceInput;

	private Button _replaceNextButton;

	private Button _replaceAllButton;

	private ConfirmationDialog _goToLineDialog;

	private SpinBox _goToLineSpin;

	private FileDialog _newFileDialog;

	private FileDialog _openFileDialog;

	private FileDialog _saveAsDialog;

	private ConfirmationDialog _unsavedChangesDialog;

	private Button _discardUnsavedButton;

	private readonly Dictionary<string, ScriptTabInfo> _openTabs = new Dictionary<string, ScriptTabInfo>();

	private readonly List<string> _tabKeys = new List<string>();

	private readonly Queue<string> _pendingCloseKeys = new Queue<string>();

	private readonly List<DiagnosticListEntry> _diagnosticEntries = new List<DiagnosticListEntry>();

	private readonly List<SearchMatch> _searchMatches = new List<SearchMatch>();

	private string _currentTabKey = "";

	private bool _isSwitchingTab;

	private bool _isLoadingTab;

	private bool _isCompiling;

	private bool _isBulkReplacing;

	private int _activeSearchMatch = -1;

	private string _pendingCloseTabKey = "";

	private bool _hiddenWorkSuspended;

	private bool _workspaceSelected;

	private bool _workspaceActive;

	private string _activeProjectRoot = "";

	private SynchronizationContext _scriptEditorSynchronizationContext;

	private CancellationTokenSource _buildLifetimeCts = new CancellationTokenSource();

	private static readonly XWModAssemblyLoader CallbackPreviewLoader = new XWModAssemblyLoader();

	private static string _callbackPreviewOwnerId = "";

	private static string _callbackPreviewAssemblyPath = "";

	private Texture2D _iconScript;

	private Texture2D _iconSave;

	private Texture2D _iconNew;

	private Texture2D _iconFolder;

	private Texture2D _iconClose;

	private Texture2D _iconBuildCSharp;

	private Texture2D _iconStatusError;

	private Texture2D _iconStatusWarning;

	private readonly object _scriptDebugSnapshotLock = new object();

	private XWModDebugWorkbench _debugWorkbench;

	private Button _debugWorkbenchToggle;

	private XWModDebugSnapshot _pendingScriptDebugSnapshot;

	private int _scriptDebugNavigationQueued;

	private static readonly UTF8Encoding ScriptUtf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

	private PanelContainer _externalConflictBar;

	private Label _externalConflictTitleLabel;

	private Label _externalConflictDetailLabel;

	private Button _viewExternalDiffButton;

	private Button _reloadExternalButton;

	private Button _overwriteExternalButton;

	private Button _saveConflictCopyButton;

	private Window _externalConflictDiffWindow;

	private TextEdit _externalConflictEditorText;

	private TextEdit _externalConflictDiskText;

	private Godot.Timer _externalConflictWatcher;

	private bool _externalConflictWatcherScanning;

	private Button _definitionButton;

	private Button _referencesButton;

	private Button _renameButton;

	private Button _renameUndoButton;

	private Button _renameRedoButton;

	private Button _formatButton;

	private Button _quickFixButton;

	private Button _debugStartButton;

	private Button _debugStopButton;

	private Label _debugStatusLabel;

	private Window _referencesWindow;

	private Label _referencesHeader;

	private ItemList _referencesList;

	private ConfirmationDialog _renameDialog;

	private LineEdit _renameInput;

	private RichTextLabel _renamePreviewLabel;

	private PopupPanel _symbolInfoPopup;

	private RichTextLabel _symbolInfoLabel;

	private PopupMenu _quickFixPopup;

	private Godot.Timer _hoverTimer;

	private Godot.Timer _renamePreviewTimer;

	private readonly List<XWCSharpIdeService.LocationResult> _referenceLocations = new List<XWCSharpIdeService.LocationResult>();

	private XWScriptDebugHost _debugHost;

	private Node _debugPreviewRoot;

	private XWScriptDebugSession _debugSession;

	private XWCSharpIdeService.RenamePreview _pendingRename;

	private IdeRequestStamp? _pendingRenameStamp;

	private int _pendingRenameGeneration;

	private XWCodeErrorChecker.ErrorData _pendingQuickFixDiagnostic;

	private Vector2 _hoverLocalPosition;

	private int _hoverLine;

	private int _hoverColumn;

	private int _hoverGeneration;

	private int _renameGeneration;

	private string _pendingRenameText = "";

	private long _ideContextRevision;

	public XWScriptCompiler.CompileResult LastCompileResult { get; private set; }

	public string CallbackPreviewOwnerId => _callbackPreviewOwnerId;

	public int UnsupportedScriptRequestCount { get; private set; }

	public int UnsupportedSaveAsRequestCount { get; private set; }

	public int CSharpTemplateCreateCount { get; private set; }

	public bool IsHiddenWorkQuiescent
	{
		get
		{
			if (!_workspaceActive && _hiddenWorkSuspended)
			{
				return IsExternalConflictWatcherIdle;
			}
			return false;
		}
	}

	public bool IsCurrentDocumentReadOnly
	{
		get
		{
			if (TryGetCurrentTab(out var tab))
			{
				return tab.IsBlueprintGenerated;
			}
			return false;
		}
	}

	public string CurrentGeneratedBlueprintSourcePath
	{
		get
		{
			if (!TryGetCurrentTab(out var tab))
			{
				return "";
			}
			return tab.BlueprintSourcePath;
		}
	}

	public XWBlueprintGeneratedCSharpPolicy.LinkState CurrentGeneratedLinkState
	{
		get
		{
			if (!TryGetCurrentTab(out var tab))
			{
				return XWBlueprintGeneratedCSharpPolicy.LinkState.NotGenerated;
			}
			return tab.GeneratedLinkState;
		}
	}

	public string HiddenWorkDiagnostic => $"visible={IsVisibleInTree()} process={IsProcessing()} physics={IsPhysicsProcessing()} codePending={_codeEdit?.HasPendingBackgroundWork ?? false} idePending={HasPendingIdeBackgroundWork} externalWatcherIdle={IsExternalConflictWatcherIdle}";

	public bool IsDebugWorkbenchBound => _debugWorkbench?.IsControllerBound ?? false;

	public bool IsDebugWorkbenchVisible => _debugWorkbench?.Visible ?? false;

	public bool IsDebugWorkbenchHiddenProcessSuspended => _debugWorkbench?.IsHiddenProcessSuspended ?? true;

	public int DebugWorkbenchCallStackCount => _debugWorkbench?.CallStackItemCount ?? 0;

	public int DebugWorkbenchVariableCount => _debugWorkbench?.VariableItemCount ?? 0;

	public ScriptSaveStatus LastSaveStatus { get; private set; }

	public string LastSaveError { get; private set; } = "";

	public int ExternalConflictDetectedCount { get; private set; }

	public int ExternalConflictReloadCount { get; private set; }

	public int ExternalConflictOverwriteCount { get; private set; }

	public int CleanExternalReloadCount { get; private set; }

	public int AtomicSaveCount { get; private set; }

	public int SaveAsRollbackCount { get; private set; }

	public int CompileSaveGateCount { get; private set; }

	public int DebugSaveGateCount { get; private set; }

	public int CompileInvocationCount { get; private set; }

	public int DebugStartInvocationCount { get; private set; }

	public int ExternalConflictWatcherScanCount { get; private set; }

	public bool IsCurrentDocumentInExternalConflict
	{
		get
		{
			if (TryGetCurrentTab(out var tab))
			{
				return tab.HasExternalConflict;
			}
			return false;
		}
	}

	public bool IsExternalConflictBarVisible
	{
		get
		{
			if (GodotObject.IsInstanceValid(_externalConflictBar))
			{
				return _externalConflictBar.Visible;
			}
			return false;
		}
	}

	public bool IsExternalConflictWatcherIdle
	{
		get
		{
			if (GodotObject.IsInstanceValid(_externalConflictWatcher))
			{
				if (_externalConflictWatcher.IsStopped() && _externalConflictWatcher.ProcessMode == ProcessModeEnum.Disabled)
				{
					return !_externalConflictWatcherScanning;
				}
				return false;
			}
			return true;
		}
	}

	public string CurrentDocumentText => _codeEdit?.Text ?? "";

	internal XWCSharpIdeService.SymbolInfoResult LastSymbolInfo { get; private set; }

	internal XWCSharpIdeService.RenamePreview LastRenamePreview => _pendingRename;

	internal Func<IdeRequestKind, string, Task> IdeResultGateForProbeAsync { get; set; }

	public XWScriptDebugSession.StartResult LastDebugStartResult { get; private set; }

	public bool IsDebugSessionRunning => _debugSession?.IsRunning ?? false;

	public bool HasLiveDebugSession => _debugSession?.HasLiveSession ?? false;

	public bool DebugEntryInvoked => _debugSession?.EntryPointInvoked ?? false;

	public int DebugLoadedAssemblyCount => _debugSession?.LoadedAssemblyCount ?? 0;

	public bool DebugEntryTaskCompleted => _debugSession?.EntryTaskCompleted ?? true;

	public bool DebugLoadContextAlive
	{
		get
		{
			XWScriptDebugSession debugSession = _debugSession;
			if (debugSession == null)
			{
				return false;
			}
			return debugSession.LastLoadContextWeakReference?.IsAlive == true;
		}
	}

	public int DebugPreviewChildCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_debugPreviewRoot))
			{
				return 0;
			}
			return _debugPreviewRoot.GetChildCount();
		}
	}

	public int DebugMainThreadId => _debugHost?.MainThreadId ?? 0;

	private bool HasPendingIdeBackgroundWork
	{
		get
		{
			Godot.Timer hoverTimer = _hoverTimer;
			if (hoverTimer == null || hoverTimer.ProcessMode != ProcessModeEnum.Disabled)
			{
				Godot.Timer hoverTimer2 = _hoverTimer;
				if (hoverTimer2 != null && !hoverTimer2.IsStopped())
				{
					return true;
				}
			}
			Godot.Timer renamePreviewTimer = _renamePreviewTimer;
			if (renamePreviewTimer == null || renamePreviewTimer.ProcessMode != ProcessModeEnum.Disabled)
			{
				Godot.Timer renamePreviewTimer2 = _renamePreviewTimer;
				if (renamePreviewTimer2 == null)
				{
					return false;
				}
				return !renamePreviewTimer2.IsStopped();
			}
			return false;
		}
	}

	public override void _Ready()
	{
		_scriptEditorSynchronizationContext = SynchronizationContext.Current;
		XWModBuildSingleFlight.FlightStateChanged += OnModBuildFlightStateChanged;
		_fileMenuBtn = GetNode<MenuButton>("%FileMenuBtn");
		_searchMenuBtn = GetNode<MenuButton>("%SearchMenuBtn");
		_goToMenuBtn = GetNode<MenuButton>("%GoToMenuBtn");
		_overflowMenuBtn = GetNode<MenuButton>("%OverflowMenuBtn");
		_scriptNameLabel = GetNode<Label>("%ScriptNameLabel");
		_scriptFilter = GetNode<LineEdit>("%ScriptFilter");
		_scriptList = GetNode<ItemList>("%ScriptList");
		_scriptTabBar = GetNode<TabBar>("%ScriptTabBar");
		_codeEdit = GetNode<XWCodeEdit>("%XWCodeEdit");
		_lineColLabel = GetNode<Label>("%LineColLabel");
		_compileButton = GetNode<Button>("%CompileButton");
		_validateButton = GetNode<Button>("%ValidateButton");
		_previousProblemButton = GetNode<Button>("%PreviousProblemButton");
		_nextProblemButton = GetNode<Button>("%NextProblemButton");
		_openBlueprintButton = GetNode<Button>("%OpenBlueprintButton");
		_generatedBlueprintBar = GetNode<PanelContainer>("%GeneratedBlueprintBar");
		_generatedBlueprintStatusLabel = GetNode<Label>("%GeneratedBlueprintStatusLabel");
		_generatedBlueprintSourceLabel = GetNode<Label>("%GeneratedBlueprintSourceLabel");
		_returnToBlueprintButton = GetNode<Button>("%ReturnToBlueprintButton");
		_diagnosticsLabel = GetNode<Label>("%DiagnosticsLabel");
		_diagnosticsPanel = GetNode<PanelContainer>("%DiagnosticsPanel");
		_diagnosticsList = GetNode<ItemList>("%DiagnosticsList");
		_diagnosticsCopyButton = GetNodeOrNull<Button>("%DiagnosticsCopyButton");
		_diagnosticsContextMenu = GetNode<PopupMenu>("%DiagnosticsContextMenu");
		_searchBar = GetNode<PanelContainer>("%SearchBar");
		_searchInput = GetNode<LineEdit>("%SearchInput");
		_searchMatchLabel = GetNode<Label>("%SearchMatchLabel");
		_searchPreviousButton = GetNode<Button>("%SearchPreviousButton");
		_searchNextButton = GetNode<Button>("%SearchNextButton");
		_searchCaseButton = GetNode<CheckButton>("%SearchCaseButton");
		_searchWholeButton = GetNode<CheckButton>("%SearchWholeButton");
		_searchCloseButton = GetNode<Button>("%SearchCloseButton");
		_replaceRow = GetNode<Container>("%ReplaceRow");
		_replaceInput = GetNode<LineEdit>("%ReplaceInput");
		_replaceNextButton = GetNode<Button>("%ReplaceNextButton");
		_replaceAllButton = GetNode<Button>("%ReplaceAllButton");
		_goToLineDialog = GetNode<ConfirmationDialog>("%GoToLineDialog");
		_goToLineSpin = GetNode<SpinBox>("%GoToLineSpin");
		_newFileDialog = GetNode<FileDialog>("%NewFileDialog");
		_openFileDialog = GetNode<FileDialog>("%OpenFileDialog");
		_saveAsDialog = GetNode<FileDialog>("%SaveAsDialog");
		_unsavedChangesDialog = GetNode<ConfirmationDialog>("%UnsavedChangesDialog");
		_discardUnsavedButton = _unsavedChangesDialog.AddButton("不保存", right: true);
		_discardUnsavedButton.Name = "DiscardUnsavedButton";
		_discardUnsavedButton.UniqueNameInOwner = true;
		_discardUnsavedButton.Text = "不保存";
		InitializeExternalConflictUI();
		LoadIcons();
		BuildFileMenu();
		BuildSearchMenu();
		BuildGoToMenu();
		BuildToolbarOverflowMenu();
		BuildDiagnosticsContextMenu();
		ConnectSignals();
		UpdateDiagnosticList(new List<XWCodeErrorChecker.ErrorData>(), "", includeFileName: false);
		_searchBar.Hide();
		_searchMatches.Clear();
		_activeSearchMatch = -1;
		_searchMatchLabel.Text = "0 / 0";
		UpdateCompileControls();
		InitializeIdeFeatures();
		SetProcess(enable: false);
		SetPhysicsProcess(enable: false);
		VisibilityChanged += OnEditorVisibilityChanged;
		OnEditorVisibilityChanged();
	}

	private void LoadIcons()
	{
		XWClassRegistry instance = XWClassRegistry.Instance;
		_iconScript = instance.GetUIIcon("Script");
		_iconSave = instance.GetUIIcon("Save");
		_iconNew = instance.GetUIIcon("Add");
		_iconFolder = instance.GetUIIcon("Folder");
		_iconClose = instance.GetUIIcon("Close");
		_iconBuildCSharp = instance.GetUIIcon("BuildCSharp");
		_iconStatusError = instance.GetUIIcon("StatusError");
		_iconStatusWarning = instance.GetUIIcon("StatusWarning");
		_compileButton.Icon = _iconBuildCSharp;
		_validateButton.Icon = instance.GetUIIcon("Search");
		_previousProblemButton.Icon = instance.GetUIIcon("ArrowUp");
		_nextProblemButton.Icon = instance.GetUIIcon("ArrowDown");
		_openBlueprintButton.Icon = instance.GetUIIcon("GraphEdit");
		_overflowMenuBtn.Icon = instance.GetUIIcon("GuiTabMenuHl");
		Button nodeOrNull = GetNodeOrNull<Button>("%HistoryBackBtn");
		if (nodeOrNull != null)
		{
			nodeOrNull.Icon = instance.GetUIIcon("Back");
		}
		Button nodeOrNull2 = GetNodeOrNull<Button>("%HistoryForwardBtn");
		if (nodeOrNull2 != null)
		{
			nodeOrNull2.Icon = instance.GetUIIcon("Forward");
		}
		LineEdit nodeOrNull3 = GetNodeOrNull<LineEdit>("%ScriptFilter");
		if (nodeOrNull3 != null)
		{
			nodeOrNull3.RightIcon = instance.GetUIIcon("Search");
		}
	}

	private void BuildFileMenu()
	{
		PopupMenu popup = _fileMenuBtn.GetPopup();
		popup.Clear();
		popup.AddIconItem(_iconNew, "新建 C# 脚本", 0, Key.None);
		popup.AddSeparator();
		popup.AddIconItem(_iconFolder, "打开...", 3, Key.None);
		popup.AddSeparator();
		popup.AddIconItem(_iconSave, "保存", 5, Key.None);
		popup.AddIconItem(_iconSave, "另存为...", 6, Key.None);
		popup.AddIconItem(_iconSave, "全部保存", 7, Key.None);
		popup.AddSeparator();
		popup.AddIconItem(_iconClose, "关闭", 9, Key.None);
		popup.AddItem("关闭全部", 10, Key.None);
		popup.AddItem("关闭其他", 11, Key.None);
		popup.IdPressed += OnFileMenuItem;
	}

	private void BuildSearchMenu()
	{
		PopupMenu popup = _searchMenuBtn.GetPopup();
		popup.Clear();
		popup.AddItem("查找（Ctrl+F）", 0, Key.None);
		popup.AddItem("查找并替换（Ctrl+H）", 1, Key.None);
		popup.AddSeparator();
		popup.AddItem("下一个匹配（Enter）", 2, Key.None);
		popup.AddItem("上一个匹配（Shift+Enter）", 3, Key.None);
		popup.IdPressed += OnSearchMenuItem;
	}

	private void BuildGoToMenu()
	{
		PopupMenu popup = _goToMenuBtn.GetPopup();
		popup.Clear();
		popup.AddItem("跳转到行（Ctrl+G）", 0, Key.None);
		popup.AddSeparator();
		popup.AddItem("下一个问题（F8）", 1, Key.None);
		popup.AddItem("上一个问题（Shift+F8）", 2, Key.None);
		popup.IdPressed += OnGoToMenuItem;
	}

	private void BuildToolbarOverflowMenu()
	{
		PopupMenu popup = _overflowMenuBtn.GetPopup();
		popup.Clear();
		XWClassRegistry instance = XWClassRegistry.Instance;
		popup.AddIconItem(instance.GetUIIcon("Back"), "撤销项目重命名", 0, Key.None);
		popup.AddIconItem(instance.GetUIIcon("Forward"), "重做项目重命名", 1, Key.None);
		popup.AddSeparator();
		popup.AddIconItem(instance.GetUIIcon("CombineLines"), "格式化文档或选区", 3, Key.None);
		popup.AddIconItem(instance.GetUIIcon("Search"), "检查当前脚本", 4, Key.None);
		popup.AddSeparator();
		popup.AddIconItem(instance.GetUIIcon("ArrowUp"), "上一个问题", 6, Key.None);
		popup.AddIconItem(instance.GetUIIcon("ArrowDown"), "下一个问题", 7, Key.None);
		popup.AddSeparator();
		popup.AddIconItem(instance.GetUIIcon("GraphEdit"), "打开 API 拼图", 9, Key.None);
		popup.IdPressed += OnToolbarOverflowItem;
	}

	private void OnToolbarOverflowItem(long id)
	{
		switch ((ToolbarOverflowAction)id)
		{
		case ToolbarOverflowAction.RenameUndo:
			UndoLastProjectRenameAsync();
			break;
		case ToolbarOverflowAction.RenameRedo:
			RedoLastProjectRenameAsync();
			break;
		case ToolbarOverflowAction.Format:
			FormatDocumentOrSelection();
			break;
		case ToolbarOverflowAction.Validate:
			RequestCurrentValidation();
			break;
		case ToolbarOverflowAction.PreviousProblem:
			JumpToNextDiagnostic(-1);
			break;
		case ToolbarOverflowAction.NextProblem:
			JumpToNextDiagnostic(1);
			break;
		case ToolbarOverflowAction.OpenBlueprint:
			OpenBlueprintWorkspace();
			break;
		case (ToolbarOverflowAction)2:
		case (ToolbarOverflowAction)5:
		case (ToolbarOverflowAction)8:
			break;
		}
	}

	private void OnEditorVisibilityChanged()
	{
		SetProcess(enable: false);
		SetPhysicsProcess(enable: false);
		ApplyWorkspaceActivity();
	}

	public void SetWorkspaceActive(bool active)
	{
		_workspaceSelected = active;
		ApplyWorkspaceActivity();
	}

	private void ApplyWorkspaceActivity()
	{
		if (_workspaceActive = _workspaceSelected && IsVisibleInTree())
		{
			_hiddenWorkSuspended = false;
			_codeEdit?.ResumeBackgroundWork();
			ResumeIdeBackgroundWork();
			SetExternalConflictWatcherActive(active: true);
		}
		else
		{
			SetExternalConflictWatcherActive(active: false);
			_codeEdit?.SuspendBackgroundWork();
			SuspendIdeBackgroundWork();
			_hiddenWorkSuspended = true;
		}
	}

	private void OnSearchMenuItem(long id)
	{
		switch ((SearchMenu)id)
		{
		case SearchMenu.Find:
			ShowSearchBar(showReplace: false);
			break;
		case SearchMenu.Replace:
			ShowSearchBar(showReplace: true);
			break;
		case SearchMenu.Next:
			NavigateSearch(1);
			break;
		case SearchMenu.Previous:
			NavigateSearch(-1);
			break;
		}
	}

	private void OnGoToMenuItem(long id)
	{
		switch ((GoToMenu)id)
		{
		case GoToMenu.Line:
			ShowGoToLineDialog();
			break;
		case GoToMenu.NextProblem:
			JumpToNextDiagnostic(1);
			break;
		case GoToMenu.PreviousProblem:
			JumpToNextDiagnostic(-1);
			break;
		}
	}

	private void BuildDiagnosticsContextMenu()
	{
		_diagnosticsContextMenu.Clear();
		_diagnosticsContextMenu.AddItem("复制所选", 0, Key.None);
		_diagnosticsContextMenu.AddItem("复制全部", 1, Key.None);
		_diagnosticsContextMenu.IdPressed += OnDiagnosticsContextMenuItem;
	}

	private void OnDiagnosticsContextMenuItem(long id)
	{
		switch ((DiagnosticsContextMenu)id)
		{
		case DiagnosticsContextMenu.CopySelected:
			CopySelectedDiagnostics();
			break;
		case DiagnosticsContextMenu.CopyAll:
			CopyAllDiagnostics();
			break;
		}
	}

	private void ConnectSignals()
	{
		_codeEdit.TextChanged += OnCodeChanged;
		_codeEdit.CaretChanged += UpdateLineCol;
		_scriptFilter.TextChanged += OnScriptFilterChanged;
		_scriptList.ItemSelected += OnScriptListItemSelected;
		_scriptTabBar.TabChanged += OnTabChanged;
		_scriptTabBar.TabClosePressed += OnTabClosePressed;
		_compileButton.Pressed += OnCompileButtonPressed;
		_validateButton.Pressed += RequestCurrentValidation;
		_previousProblemButton.Pressed += () =>
		{
			JumpToNextDiagnostic(-1);
		};
		_nextProblemButton.Pressed += () =>
		{
			JumpToNextDiagnostic(1);
		};
		_openBlueprintButton.Pressed += OpenBlueprintWorkspace;
		_returnToBlueprintButton.Pressed += () =>
		{
			OpenCurrentGeneratedBlueprintSource();
		};
		_codeEdit.ErrorsChanged += OnCodeErrorsChanged;
		_codeEdit.DiagnosticsChanged += OnCodeDiagnosticsChanged;
		_diagnosticsList.ItemSelected += OnDiagnosticListItemSelected;
		_diagnosticsList.ItemActivated += OnDiagnosticListItemActivated;
		_diagnosticsList.GuiInput += OnDiagnosticsListGuiInput;
		if (_diagnosticsCopyButton != null)
		{
			_diagnosticsCopyButton.Pressed += CopySelectedDiagnostics;
		}
		_newFileDialog.FileSelected += OnNewFileSelected;
		_openFileDialog.FileSelected += OpenFile;
		_saveAsDialog.FileSelected += OnSaveAsFileSelected;
		_unsavedChangesDialog.Confirmed += OnUnsavedCloseConfirmed;
		_unsavedChangesDialog.Canceled += OnUnsavedCloseCanceled;
		_discardUnsavedButton.Pressed += () =>
		{
			OnUnsavedCloseCustomAction("discard");
		};
		_searchInput.TextChanged += (string _) =>
		{
			RefreshSearchMatches(selectNearest: true);
		};
		_searchInput.TextSubmitted += (string _) =>
		{
			NavigateSearch(1);
		};
		_searchPreviousButton.Pressed += () =>
		{
			NavigateSearch(-1);
		};
		_searchNextButton.Pressed += () =>
		{
			NavigateSearch(1);
		};
		_searchCaseButton.Toggled += (bool _) =>
		{
			RefreshSearchMatches(selectNearest: true);
		};
		_searchWholeButton.Toggled += (bool _) =>
		{
			RefreshSearchMatches(selectNearest: true);
		};
		_searchCloseButton.Pressed += CloseSearchBar;
		_replaceInput.TextSubmitted += (string _) =>
		{
			ReplaceCurrentMatch();
		};
		_replaceNextButton.Pressed += ReplaceCurrentMatch;
		_replaceAllButton.Pressed += () =>
		{
			ReplaceAllMatches();
		};
		_goToLineDialog.Confirmed += CommitGoToLine;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!_workspaceActive || !(@event is InputEventKey { Pressed: not false, Echo: false } inputEventKey))
		{
			return;
		}
		if (inputEventKey.Keycode == Key.B && inputEventKey.CtrlPressed && !inputEventKey.ShiftPressed)
		{
			Button compileButton = _compileButton;
			if (compileButton == null || !compileButton.Disabled)
			{
				OnCompileButtonPressed();
			}
			GetViewport().SetInputAsHandled();
		}
		else if (inputEventKey.Keycode == Key.F5 && inputEventKey.ShiftPressed)
		{
			Button debugStopButton = _debugStopButton;
			if (debugStopButton == null || !debugStopButton.Disabled)
			{
				StopDebugSessionAsync();
			}
			GetViewport().SetInputAsHandled();
		}
		else if (inputEventKey.Keycode == Key.F5)
		{
			Button debugStartButton = _debugStartButton;
			if (debugStartButton == null || !debugStartButton.Disabled)
			{
				StartDebugSessionAsync();
			}
			GetViewport().SetInputAsHandled();
		}
		else if (inputEventKey.Keycode == Key.O && inputEventKey.CtrlPressed && !inputEventKey.ShiftPressed)
		{
			if (!string.IsNullOrWhiteSpace(GetActiveModProjectRoot()))
			{
				OpenFileDialog();
			}
			GetViewport().SetInputAsHandled();
		}
		else if (HasFileOpen())
		{
			if (HandleIdeShortcut(inputEventKey))
			{
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.F && inputEventKey.CtrlPressed)
			{
				ShowSearchBar(showReplace: false);
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.H && inputEventKey.CtrlPressed)
			{
				ShowSearchBar(showReplace: true);
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.G && inputEventKey.CtrlPressed)
			{
				ShowGoToLineDialog();
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.Escape && GodotObject.IsInstanceValid(_searchBar) && _searchBar.Visible)
			{
				CloseSearchBar();
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.S && inputEventKey.CtrlPressed && !inputEventKey.ShiftPressed)
			{
				SaveFile();
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.S && inputEventKey.CtrlPressed && inputEventKey.ShiftPressed)
			{
				SaveFileAs();
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.W && inputEventKey.CtrlPressed)
			{
				CloseCurrentTab();
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.F8 && inputEventKey.ShiftPressed)
			{
				JumpToNextDiagnostic(-1);
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.F8)
			{
				JumpToNextDiagnostic(1);
				GetViewport().SetInputAsHandled();
			}
		}
	}

	private void OnFileMenuItem(long id)
	{
		switch ((MenuFile)id)
		{
		case MenuFile.NewScript:
			NewCSharpScript();
			break;
		case MenuFile.Open:
			OpenFileDialog();
			break;
		case MenuFile.Save:
			SaveFile();
			break;
		case MenuFile.SaveAs:
			SaveFileAs();
			break;
		case MenuFile.SaveAll:
			SaveAllTabs(showToast: true);
			break;
		case MenuFile.Close:
			CloseCurrentTab();
			break;
		case MenuFile.CloseAll:
			CloseAllTabs();
			break;
		case MenuFile.CloseOther:
			CloseOtherTabs();
			break;
		case MenuFile.Sep1:
		case (MenuFile)2:
		case MenuFile.Sep2:
		case MenuFile.Sep3:
			break;
		}
	}

	private void NewCSharpScript()
	{
		if (XWEditorInterface.Instance?.GetFileSystemPanel() is XWFileSystemPanel xWFileSystemPanel && GodotObject.IsInstanceValid(xWFileSystemPanel))
		{
			XWEditorInterface.Instance.FocusPanel("filesystem");
			xWFileSystemPanel.ShowNewCSharpScriptDialog();
		}
		else
		{
			_newFileDialog.CurrentFile = "NewModScript.cs";
			_newFileDialog.PopupCentered();
		}
	}

	private void OnNewFileSelected(string path)
	{
		path = NormalizeNewCSharpPath(path);
		if (!IsSupportedScriptPath(path))
		{
			RejectUnsupportedScriptPath(path, "新建");
			return;
		}
		using Godot.FileAccess fileAccess = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Write);
		if (fileAccess == null)
		{
			XWEditorInterface.Instance?.ShowToast("无法创建 C# 脚本: " + path, 2);
			return;
		}
		fileAccess.StoreString(BuildDefaultCSharpTemplate(path));
		CSharpTemplateCreateCount++;
		XWBPCSharpMemberRegistry.Instance.NotifyScriptSaved(path);
		OpenFile(path);
	}

	private void OpenFileDialog()
	{
		_openFileDialog.PopupCentered();
	}

	public void OpenFile(string filePath)
	{
		TryOpenFile(filePath);
	}

	public bool TryOpenFile(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath))
		{
			return false;
		}
		filePath = ResolveOpenFilePath(filePath);
		if (!IsSupportedScriptPath(filePath))
		{
			RejectUnsupportedScriptPath(filePath, "打开");
			return false;
		}
		if (_openTabs.TryGetValue(filePath, out var value))
		{
			if (value.IsBlueprintGenerated)
			{
				ReloadTabFromDisk(value);
			}
			else
			{
				RefreshTabExternalState(value, reloadVisible: false);
			}
			int num = _tabKeys.IndexOf(filePath);
			if (num >= 0 && _scriptTabBar.CurrentTab != num)
			{
				_isSwitchingTab = true;
				_scriptTabBar.CurrentTab = num;
				_isSwitchingTab = false;
			}
			LoadTabData(filePath);
			_codeEdit.GrabFocus();
			return true;
		}
		SaveCurrentTabState();
		ScriptTabInfo scriptTabInfo = new ScriptTabInfo
		{
			FilePath = filePath,
			FileExtension = filePath.GetExtension()
		};
		if (TryReadDiskSnapshot(filePath, out var snapshot, out var error))
		{
			scriptTabInfo.Text = snapshot.Text;
			ApplyDiskBaseline(scriptTabInfo, snapshot);
		}
		else
		{
			LastSaveError = error;
		}
		RefreshGeneratedMetadata(scriptTabInfo);
		_openTabs[filePath] = scriptTabInfo;
		_tabKeys.Add(filePath);
		string baseName = filePath.GetFile().GetBaseName();
		_isSwitchingTab = true;
		_scriptTabBar.AddTab(baseName, _iconScript);
		_scriptTabBar.CurrentTab = _tabKeys.Count - 1;
		_isSwitchingTab = false;
		LoadTabData(filePath);
		UpdateScriptList();
		_codeEdit.GrabFocus();
		return true;
	}

	public void OpenFileAt(string filePath, int line, int column)
	{
		TryOpenFileAt(filePath, line, column);
	}

	public bool OpenGeneratedBlueprintFile(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath))
		{
			return false;
		}
		string text = ResolveOpenFilePath(filePath);
		if (_openTabs.TryGetValue(text, out var value))
		{
			ReloadTabFromDisk(value);
			LoadTabData(text);
			UpdateScriptList();
			return value.IsBlueprintGenerated;
		}
		if (!TryOpenFile(text))
		{
			return false;
		}
		return IsCurrentDocumentReadOnly;
	}

	public bool TryOpenFileAt(string filePath, int line, int column)
	{
		if (!TryOpenFile(filePath))
		{
			return false;
		}
		if (_codeEdit == null)
		{
			return false;
		}
		if (line >= 0)
		{
			_codeEdit.SetCaretLine(line);
		}
		if (column >= 0)
		{
			_codeEdit.SetCaretColumn(column);
		}
		_codeEdit.GrabFocus();
		return true;
	}

	private void SaveCurrentTabState()
	{
		if (!string.IsNullOrEmpty(_currentTabKey) && _openTabs.TryGetValue(_currentTabKey, out var value))
		{
			value.Text = _codeEdit.Text;
			value.CursorLine = _codeEdit.GetCaretLine();
			value.CursorColumn = _codeEdit.GetCaretColumn();
			value.ScrollVertical = (int)_codeEdit.ScrollVertical;
			value.Breakpoints = _codeEdit.GetBreakpointLines();
		}
	}

	private void LoadTabData(string tabKey)
	{
		if (_openTabs.TryGetValue(tabKey, out var value))
		{
			_scriptNameLabel.Text = value.FilePath.GetFile();
			_isLoadingTab = true;
			try
			{
				_codeEdit.SetDocumentPath(value.FilePath);
				_codeEdit.SetProjectRoot(GetActiveModProjectRoot());
				_codeEdit.LoadDocumentText(value.Text);
				_codeEdit.SetFileExtension(value.FileExtension);
				_codeEdit.RestoreBreakpointLines(value.Breakpoints);
				_currentTabKey = tabKey;
				_codeEdit.Editable = !value.IsBlueprintGenerated;
				_codeEdit.SetCaretLine(value.CursorLine);
				_codeEdit.SetCaretColumn(value.CursorColumn);
				_codeEdit.ScrollVertical = value.ScrollVertical;
			}
			finally
			{
				_isLoadingTab = false;
			}
			if (_workspaceActive)
			{
				_codeEdit.ResumeBackgroundWork();
			}
			if (GodotObject.IsInstanceValid(_searchBar) && _searchBar.Visible)
			{
				RefreshSearchMatches(selectNearest: true);
			}
			UpdateTabTitle();
			UpdateLineCol();
			UpdateGeneratedBlueprintBar(value);
			UpdateExternalConflictSurface(value);
			_codeEdit.RequestValidationNow();
			UpdateCompileControls();
		}
	}

	private void OnTabChanged(long tabIdx)
	{
		if (_isSwitchingTab || tabIdx < 0 || tabIdx >= _tabKeys.Count)
		{
			return;
		}
		string text = _tabKeys[(int)tabIdx];
		if (!(text == _currentTabKey))
		{
			SaveCurrentTabState();
			if (_openTabs.TryGetValue(text, out var value) && !value.IsBlueprintGenerated)
			{
				RefreshTabExternalState(value, reloadVisible: false);
			}
			LoadTabData(text);
			UpdateScriptListSelection();
		}
	}

	private void OnTabClosePressed(long tabIdx)
	{
		if (tabIdx >= 0 && tabIdx < _tabKeys.Count)
		{
			CloseTab(_tabKeys[(int)tabIdx]);
		}
	}

	public void CloseTab(string tabKey)
	{
		BeginCloseTabs(new string[1] { tabKey });
	}

	private void BeginCloseTabs(IEnumerable<string> tabKeys)
	{
		_pendingCloseKeys.Clear();
		_pendingCloseTabKey = "";
		foreach (string tabKey in tabKeys)
		{
			if (_openTabs.ContainsKey(tabKey))
			{
				_pendingCloseKeys.Enqueue(tabKey);
			}
		}
		ContinueCloseQueue();
	}

	private void ContinueCloseQueue()
	{
		while (_pendingCloseKeys.Count > 0)
		{
			string text = _pendingCloseKeys.Dequeue();
			if (_openTabs.TryGetValue(text, out var value))
			{
				if (value.HasUnsavedChanges || value.HasExternalConflict)
				{
					_pendingCloseTabKey = text;
					_unsavedChangesDialog.Title = "未保存的修改";
					_unsavedChangesDialog.DialogText = (value.HasExternalConflict ? (value.FilePath.GetFile() + " 同时存在编辑器缓冲区和外部磁盘版本。\n保存会保持冲突并停止关闭；请先在编辑器中选择重新载入或覆盖磁盘。") : (value.FilePath.GetFile() + " 仍有未保存的修改。\n请选择保存、不保存或取消关闭。"));
					_unsavedChangesDialog.PopupCentered();
					return;
				}
				CloseTabImmediately(text);
			}
		}
		_pendingCloseTabKey = "";
	}

	private void OnUnsavedCloseConfirmed()
	{
		string pendingCloseTabKey = _pendingCloseTabKey;
		_pendingCloseTabKey = "";
		if (string.IsNullOrEmpty(pendingCloseTabKey) || !_openTabs.ContainsKey(pendingCloseTabKey))
		{
			ContinueCloseQueue();
			return;
		}
		if (!SaveTab(pendingCloseTabKey))
		{
			_pendingCloseKeys.Clear();
			return;
		}
		CloseTabImmediately(pendingCloseTabKey);
		CallDeferred("ContinueCloseQueue");
	}

	private void OnUnsavedCloseCanceled()
	{
		_pendingCloseTabKey = "";
		_pendingCloseKeys.Clear();
	}

	private void OnUnsavedCloseCustomAction(StringName action)
	{
		if (!(action != (StringName)"discard"))
		{
			string pendingCloseTabKey = _pendingCloseTabKey;
			_pendingCloseTabKey = "";
			_unsavedChangesDialog.Hide();
			if (!string.IsNullOrEmpty(pendingCloseTabKey) && _openTabs.TryGetValue(pendingCloseTabKey, out var value))
			{
				value.HasUnsavedChanges = false;
				CloseTabImmediately(pendingCloseTabKey);
			}
			CallDeferred("ContinueCloseQueue");
		}
	}

	private void CloseTabImmediately(string tabKey)
	{
		if (!_openTabs.ContainsKey(tabKey))
		{
			return;
		}
		bool flag = tabKey == _currentTabKey;
		int num = _tabKeys.IndexOf(tabKey);
		_openTabs.Remove(tabKey);
		_tabKeys.RemoveAt(num);
		_scriptTabBar.RemoveTab(num);
		if (_tabKeys.Count == 0)
		{
			ClearEditor();
			return;
		}
		if (flag)
		{
			int num2 = Mathf.Min(num, _tabKeys.Count - 1);
			_isSwitchingTab = true;
			_scriptTabBar.CurrentTab = num2;
			_isSwitchingTab = false;
			LoadTabData(_tabKeys[num2]);
		}
		UpdateScriptList();
	}

	public void CloseCurrentTab()
	{
		if (!string.IsNullOrEmpty(_currentTabKey))
		{
			CloseTab(_currentTabKey);
		}
	}

	public void CloseAllTabs()
	{
		BeginCloseTabs(new List<string>(_tabKeys));
	}

	public bool TrySwitchProjectRoot(string projectRoot)
	{
		string text = NormalizeProjectRoot(projectRoot);
		if (string.Equals(_activeProjectRoot, text, StringComparison.OrdinalIgnoreCase) && string.Equals(NormalizeProjectRoot(_codeEdit?.GetProjectRoot()), text, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (XWScriptCompiler.IsModBuildBusy(NormalizeProjectRoot(_activeProjectRoot)))
		{
			RenewBuildLifetime(cancelCurrentBuild: true);
			return false;
		}
		XWScriptDebugSession debugSession = _debugSession;
		if (debugSession != null && debugSession.HasLiveSession)
		{
			return false;
		}
		SaveCurrentTabState();
		foreach (ScriptTabInfo value in _openTabs.Values)
		{
			if (value.HasUnsavedChanges || value.HasExternalConflict)
			{
				return false;
			}
		}
		if (!UnloadCallbackPreview(reportFailure: true))
		{
			return false;
		}
		SetWorkspaceActive(active: false);
		_pendingCloseKeys.Clear();
		_pendingCloseTabKey = "";
		_openTabs.Clear();
		_tabKeys.Clear();
		ClearEditor();
		_codeEdit.SetDocumentPath("");
		_codeEdit.SetProjectRoot(text);
		_activeProjectRoot = text;
		RenewBuildLifetime(cancelCurrentBuild: true);
		UpdateCompileControls();
		UpdateDebugControls();
		return true;
	}

	private void RenewBuildLifetime(bool cancelCurrentBuild)
	{
		CancellationTokenSource buildLifetimeCts = _buildLifetimeCts;
		_buildLifetimeCts = new CancellationTokenSource();
		if (buildLifetimeCts == null)
		{
			return;
		}
		if (cancelCurrentBuild)
		{
			try
			{
				buildLifetimeCts.Cancel();
			}
			catch (ObjectDisposedException)
			{
			}
		}
		buildLifetimeCts.Dispose();
	}

	private static string NormalizeProjectRoot(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			path = ProjectSettings.GlobalizePath(path);
		}
		try
		{
			return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}
		catch
		{
			return path.TrimEnd('/', '\\');
		}
	}

	public void CloseOtherTabs()
	{
		string currentTabKey = _currentTabKey;
		List<string> list = new List<string>();
		foreach (string tabKey in _tabKeys)
		{
			if (tabKey != currentTabKey)
			{
				list.Add(tabKey);
			}
		}
		BeginCloseTabs(list);
	}

	private void ClearEditor()
	{
		_codeEdit.LoadDocumentText("");
		_codeEdit.Editable = true;
		_codeEdit.ClearAllErrors();
		_currentTabKey = "";
		_scriptNameLabel.Text = "未打开文件";
		_scriptTabBar.ClearTabs();
		_scriptList.Clear();
		_diagnosticsLabel.Text = "0 错误，0 警告";
		UpdateDiagnosticList(new List<XWCodeErrorChecker.ErrorData>(), "", includeFileName: false);
		_searchBar.Hide();
		_searchMatches.Clear();
		_activeSearchMatch = -1;
		_searchMatchLabel.Text = "0 / 0";
		if (GodotObject.IsInstanceValid(_generatedBlueprintBar))
		{
			_generatedBlueprintBar.Hide();
		}
		if (GodotObject.IsInstanceValid(_externalConflictBar))
		{
			_externalConflictBar.Hide();
		}
		if (GodotObject.IsInstanceValid(_externalConflictDiffWindow))
		{
			_externalConflictDiffWindow.Hide();
		}
		UpdateCompileControls();
	}

	public bool SaveFile()
	{
		if (string.IsNullOrEmpty(_currentTabKey))
		{
			return false;
		}
		return SaveTab(_currentTabKey);
	}

	private bool SaveTab(string tabKey)
	{
		if (_openTabs.TryGetValue(tabKey, out var value) && value.IsBlueprintGenerated)
		{
			SaveTabWithResult(tabKey, overwriteExternalConflict: false);
			return false;
		}
		ScriptSaveStatus scriptSaveStatus = SaveTabWithResult(tabKey, overwriteExternalConflict: false);
		if ((uint)scriptSaveStatus <= 1u)
		{
			return true;
		}
		return false;
	}

	public void SaveFileAs()
	{
		if (!string.IsNullOrEmpty(_currentTabKey) && CanEditCurrentDocument(showToast: true))
		{
			_saveAsDialog.CurrentPath = _currentTabKey;
			_saveAsDialog.PopupCentered();
		}
	}

	private void OnSaveAsFileSelected(string path)
	{
		path = NormalizeNewCSharpPath(path);
		if (!IsSupportedScriptPath(path))
		{
			UnsupportedSaveAsRequestCount++;
			RejectUnsupportedScriptPath(path, "另存为", countRequest: false);
		}
		else
		{
			SaveCurrentTabAsAtomic(path);
		}
	}

	public int SaveAllTabs(bool showToast = false)
	{
		SaveCurrentTabState();
		int num = 0;
		int num2 = 0;
		foreach (string tabKey in _tabKeys)
		{
			if (_openTabs.TryGetValue(tabKey, out var value) && !value.IsBlueprintGenerated)
			{
				ScriptSaveStatus scriptSaveStatus = RefreshTabExternalState(value, tabKey == _currentTabKey);
				if ((uint)(scriptSaveStatus - 2) <= 2u)
				{
					num2++;
				}
			}
		}
		if (num2 > 0)
		{
			if (showToast)
			{
				XWEditorInterface.Instance?.ShowToast($"C# 脚本保存已停止：{num2} 个冲突或读取失败。", 1);
			}
			return -num2;
		}
		foreach (string tabKey2 in _tabKeys)
		{
			if (_openTabs.TryGetValue(tabKey2, out var value2) && !value2.IsBlueprintGenerated)
			{
				bool flag;
				switch (value2.HasUnsavedChanges ? SaveTabWithResult(tabKey2, overwriteExternalConflict: false) : ScriptSaveStatus.Unchanged)
				{
				case ScriptSaveStatus.Saved:
					num++;
					continue;
				case ScriptSaveStatus.Conflict:
				case ScriptSaveStatus.IoError:
				case ScriptSaveStatus.ReadOnly:
					flag = true;
					break;
				default:
					flag = false;
					break;
				}
				if (flag)
				{
					num2++;
				}
			}
		}
		if (showToast)
		{
			XWEditorInterface.Instance?.ShowToast((num2 == 0) ? $"已保存全部 C# 脚本（{num}）" : $"C# 脚本保存完成：成功 {num}，失败 {num2}");
		}
		if (num2 != 0)
		{
			return -num2;
		}
		return num;
	}

	private void OnCodeChanged()
	{
		if (!_isLoadingTab)
		{
			if (!string.IsNullOrEmpty(_currentTabKey) && _openTabs.TryGetValue(_currentTabKey, out var value) && !value.IsBlueprintGenerated)
			{
				value.HasUnsavedChanges = true;
				value.Text = _codeEdit.Text;
			}
			UpdateTabTitle();
			UpdateLineCol();
			if (!_isBulkReplacing && GodotObject.IsInstanceValid(_searchBar) && _searchBar.Visible)
			{
				RefreshSearchMatches(selectNearest: true);
			}
		}
	}

	public void ShowSearchBar(bool showReplace)
	{
		if (HasFileOpen())
		{
			if (showReplace && !CanEditCurrentDocument(showToast: true))
			{
				showReplace = false;
			}
			_searchBar.Show();
			_replaceRow.Visible = showReplace;
			_searchBar.CustomMinimumSize = new Vector2(0f, showReplace ? 68 : 38);
			RefreshSearchMatches(selectNearest: true);
			_searchInput.GrabFocus();
			_searchInput.SelectAll();
		}
	}

	public void CloseSearchBar()
	{
		_searchBar.Hide();
		_activeSearchMatch = -1;
		_searchMatches.Clear();
		_searchMatchLabel.Text = "0 / 0";
		_codeEdit.GrabFocus();
	}

	private void RefreshSearchMatches(bool selectNearest)
	{
		_searchMatches.Clear();
		_activeSearchMatch = -1;
		string text = _searchInput.Text;
		if (string.IsNullOrEmpty(text) || !HasFileOpen())
		{
			UpdateSearchMatchLabel();
			return;
		}
		StringComparison comparisonType = (_searchCaseButton.ButtonPressed ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
		for (int i = 0; i < _codeEdit.GetLineCount(); i++)
		{
			string line = _codeEdit.GetLine(i);
			int num = 0;
			while (num <= line.Length - text.Length)
			{
				int num2 = line.IndexOf(text, num, comparisonType);
				if (num2 < 0)
				{
					break;
				}
				if (!_searchWholeButton.ButtonPressed || IsWholeWordMatch(line, num2, text.Length))
				{
					_searchMatches.Add(new SearchMatch(i, num2));
				}
				num = num2 + Math.Max(1, text.Length);
			}
		}
		if ((_searchMatches.Count > 0) & selectNearest)
		{
			_activeSearchMatch = FindNearestSearchMatch();
			SelectActiveSearchMatch();
		}
		UpdateSearchMatchLabel();
	}

	public void NavigateSearch(int direction)
	{
		if (!_searchBar.Visible)
		{
			ShowSearchBar(showReplace: false);
		}
		if (string.IsNullOrEmpty(_searchInput.Text))
		{
			_searchInput.GrabFocus();
			return;
		}
		if (_searchMatches.Count == 0)
		{
			RefreshSearchMatches(selectNearest: false);
		}
		if (_searchMatches.Count != 0)
		{
			if (_activeSearchMatch < 0)
			{
				_activeSearchMatch = FindNearestSearchMatch();
			}
			else
			{
				_activeSearchMatch = PositiveModulo(_activeSearchMatch + Math.Sign(direction), _searchMatches.Count);
			}
			SelectActiveSearchMatch();
			UpdateSearchMatchLabel();
		}
	}

	public void ReplaceCurrentMatch()
	{
		if (!CanEditCurrentDocument(showToast: true))
		{
			return;
		}
		if (_searchMatches.Count == 0)
		{
			RefreshSearchMatches(selectNearest: true);
		}
		if (_searchMatches.Count != 0)
		{
			if (_activeSearchMatch < 0)
			{
				_activeSearchMatch = FindNearestSearchMatch();
			}
			SearchMatch searchMatch = _searchMatches[_activeSearchMatch];
			_codeEdit.Select(searchMatch.Line, searchMatch.Column, searchMatch.Line, searchMatch.Column + _searchInput.Text.Length);
			_codeEdit.InsertAtCursor(_replaceInput.Text);
			RefreshSearchMatches(selectNearest: true);
		}
	}

	public int ReplaceAllMatches()
	{
		if (!CanEditCurrentDocument(showToast: true))
		{
			return 0;
		}
		RefreshSearchMatches(selectNearest: false);
		if (_searchMatches.Count == 0)
		{
			return 0;
		}
		List<SearchMatch> list = new List<SearchMatch>(_searchMatches);
		_isBulkReplacing = true;
		_codeEdit.BeginComplexOperation();
		try
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				SearchMatch searchMatch = list[num];
				_codeEdit.Select(searchMatch.Line, searchMatch.Column, searchMatch.Line, searchMatch.Column + _searchInput.Text.Length);
				_codeEdit.InsertAtCursor(_replaceInput.Text);
			}
		}
		finally
		{
			_codeEdit.EndComplexOperation();
			_isBulkReplacing = false;
		}
		RefreshSearchMatches(selectNearest: true);
		XWEditorInterface.Instance?.ShowToast($"已替换 {list.Count} 处匹配");
		return list.Count;
	}

	private int FindNearestSearchMatch()
	{
		int caretLine = _codeEdit.GetCaretLine();
		int caretColumn = _codeEdit.GetCaretColumn();
		for (int i = 0; i < _searchMatches.Count; i++)
		{
			SearchMatch searchMatch = _searchMatches[i];
			if (searchMatch.Line > caretLine || (searchMatch.Line == caretLine && searchMatch.Column >= caretColumn))
			{
				return i;
			}
		}
		return 0;
	}

	private void SelectActiveSearchMatch()
	{
		if (_activeSearchMatch >= 0 && _activeSearchMatch < _searchMatches.Count)
		{
			SearchMatch searchMatch = _searchMatches[_activeSearchMatch];
			_codeEdit.Select(searchMatch.Line, searchMatch.Column, searchMatch.Line, searchMatch.Column + _searchInput.Text.Length);
			_codeEdit.SetCaretLine(searchMatch.Line);
			_codeEdit.SetCaretColumn(searchMatch.Column + _searchInput.Text.Length);
			_codeEdit.CenterViewportToCaret();
		}
	}

	private void UpdateSearchMatchLabel()
	{
		int value = ((_activeSearchMatch >= 0) ? (_activeSearchMatch + 1) : 0);
		_searchMatchLabel.Text = $"{value} / {_searchMatches.Count}";
		bool flag = _searchMatches.Count > 0;
		_searchPreviousButton.Disabled = !flag;
		_searchNextButton.Disabled = !flag;
		_replaceNextButton.Disabled = !flag;
		_replaceAllButton.Disabled = !flag;
	}

	private static bool IsWholeWordMatch(string line, int column, int length)
	{
		bool num = column == 0 || !IsIdentifierCharacter(line[column - 1]);
		int num2 = column + length;
		bool flag = num2 >= line.Length || !IsIdentifierCharacter(line[num2]);
		return num & flag;
	}

	private static bool IsIdentifierCharacter(char character)
	{
		if (!char.IsLetterOrDigit(character))
		{
			return character == '_';
		}
		return true;
	}

	private static int PositiveModulo(int value, int divisor)
	{
		int num = value % divisor;
		if (num >= 0)
		{
			return num;
		}
		return num + divisor;
	}

	public void ShowGoToLineDialog()
	{
		if (HasFileOpen())
		{
			_goToLineSpin.MaxValue = Math.Max(1, _codeEdit.GetLineCount());
			_goToLineSpin.Value = Mathf.Clamp(_codeEdit.GetCaretLine() + 1, 1, _codeEdit.GetLineCount());
			_goToLineDialog.PopupCentered();
			_goToLineSpin.GetLineEdit().GrabFocus();
			_goToLineSpin.GetLineEdit().SelectAll();
		}
	}

	public void CommitGoToLine()
	{
		int line = Mathf.Clamp((int)_goToLineSpin.Value - 1, 0, Math.Max(0, _codeEdit.GetLineCount() - 1));
		_codeEdit.SetCaretLine(line);
		_codeEdit.SetCaretColumn(0);
		_codeEdit.CenterViewportToCaret();
		_codeEdit.GrabFocus();
	}

	private void OnCodeErrorsChanged(int errorCount, int warningCount)
	{
		if (!_isCompiling)
		{
			_diagnosticsLabel.Text = $"实时检测：{errorCount} 错误，{warningCount} 警告";
		}
	}

	private void OnCodeDiagnosticsChanged()
	{
		if (!_isCompiling)
		{
			UpdateDiagnosticList(_codeEdit.GetAllErrors(), _currentTabKey, includeFileName: false);
		}
	}

	private async void OnCompileButtonPressed()
	{
		if (!(_compileButton?.Disabled ?? false))
		{
			await CompileScriptsAsync();
		}
	}

	public async Task<XWScriptCompiler.CompileResult> CompileScriptsAsync()
	{
		if (_isCompiling)
		{
			return LastCompileResult;
		}
		string root = GetActiveModProjectRoot();
		if (string.IsNullOrWhiteSpace(root))
		{
			LastCompileResult = null;
			_diagnosticsLabel.Text = "编译未启动 · 请先打开 Mod 工程";
			XWEditorInterface.Instance?.ShowToast("编译未启动：请先打开一个 Mod 工程。", 1);
			UpdateCompileControls();
			return null;
		}
		if ((_debugSession?.IsRunning ?? false) || (_debugSession?.AssemblyLoaded ?? false))
		{
			LastCompileResult = null;
			_diagnosticsLabel.Text = "编译未启动 · 请先停止调试";
			XWEditorInterface.Instance?.ShowToast("编译未启动：请先停止当前调试会话。", 1);
			UpdateCompileControls();
			return null;
		}
		SaveCurrentTabState();
		if (SaveAllTabs() < 0)
		{
			CompileSaveGateCount++;
			LastCompileResult = null;
			_diagnosticsLabel.Text = "编译已停止 · 请先处理外部修改或保存失败";
			XWEditorInterface.Instance?.ShowToast("编译未启动：存在 C# 外部修改冲突或保存失败。", 1);
			return null;
		}
		_isCompiling = true;
		UpdateCompileControls();
		_diagnosticsLabel.Text = "正在后台编译当前 Mod 脚本…";
		try
		{
			CompileInvocationCount++;
			CancellationToken cancellationToken = _buildLifetimeCts?.Token ?? CancellationToken.None;
			XWScriptCompiler.CompileResult result = (LastCompileResult = await XWScriptCompiler.CompileModProjectAsync(root, cancellationToken));
			ApplyCompileResult(result);
			if (result?.Success ?? false)
			{
				await RefreshCallbackPreviewAsync(root, result.OutputAssemblyPath, cancellationToken);
			}
			return result;
		}
		catch (Exception ex)
		{
			_diagnosticsLabel.Text = "编译启动失败";
			XWEditorInterface.Instance?.AddOutputMessage("脚本编译启动失败: " + ex.Message, XWOutputPanel.MessageType.Error);
			return null;
		}
		finally
		{
			_isCompiling = false;
			UpdateCompileControls();
		}
	}

	private async Task<bool> RefreshCallbackPreviewAsync(string projectRoot, string assemblyPath, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!TryResolveCallbackPreviewOwner(projectRoot, out var ownerId, out var error))
		{
			ReportCallbackPreviewError(error);
			return false;
		}
		if (string.IsNullOrWhiteSpace(assemblyPath) || !File.Exists(assemblyPath))
		{
			ReportCallbackPreviewError("状态机动作目录没有找到本次编译的 Mod 程序集。");
			return false;
		}
		cancellationToken.ThrowIfCancellationRequested();
		if (!string.IsNullOrWhiteSpace(_callbackPreviewOwnerId))
		{
			if (!(await Task.Run(() => CallbackPreviewLoader.UnloadMod(_callbackPreviewOwnerId), cancellationToken)))
			{
				ReportCallbackPreviewError("旧的状态机动作仍被预览或运行时使用。请停止状态机模拟后重新编译。");
				return false;
			}
			_callbackPreviewOwnerId = "";
			_callbackPreviewAssemblyPath = "";
		}
		try
		{
			XWModAssemblyLoader.LoadedModAssembly loadedModAssembly = await Task.Run(() => CallbackPreviewLoader.LoadModAssembly(ownerId, assemblyPath, new string[1] { Path.GetDirectoryName(assemblyPath) ?? string.Empty }, registerCharacterComponentRuntimes: false), cancellationToken);
			_callbackPreviewOwnerId = loadedModAssembly.ModId;
			_callbackPreviewAssemblyPath = loadedModAssembly.AssemblyPath;
			XWEditorInterface.Instance?.AddOutputMessage($"状态机动作目录已刷新：{loadedModAssembly.ModId} · {loadedModAssembly.StateMachineCallbackCount} 个动作");
			return true;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			ReportCallbackPreviewError("状态机动作目录加载失败：" + ex2.GetBaseException().Message);
			return false;
		}
	}

	private bool UnloadCallbackPreview(bool reportFailure)
	{
		if (string.IsNullOrWhiteSpace(_callbackPreviewOwnerId))
		{
			return true;
		}
		if (!CallbackPreviewLoader.UnloadMod(_callbackPreviewOwnerId))
		{
			if (reportFailure)
			{
				ReportCallbackPreviewError("状态机动作仍有活动运行实例，暂时不能卸载。请停止状态机模拟或游戏预览后重试。");
			}
			return false;
		}
		_callbackPreviewOwnerId = "";
		_callbackPreviewAssemblyPath = "";
		return true;
	}

	private static bool TryResolveCallbackPreviewOwner(string projectRoot, out string ownerId, out string error)
	{
		ownerId = "";
		error = "";
		try
		{
			XWModManifest xWModManifest = XWModManifest.Load(Path.Combine(projectRoot ?? string.Empty, "mod.json"));
			if (xWModManifest == null)
			{
				error = "找不到 mod.json，无法确定状态机动作的真实 Mod ID。";
				return false;
			}
			if (!StateMachineCallbackKey.TryNormalizeOwnerId(xWModManifest.Id, out ownerId) || string.Equals(ownerId, "builtin", StringComparison.Ordinal))
			{
				error = "mod.json 的 id 无效，无法加载状态机动作目录。";
				ownerId = "";
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			error = "读取 Mod ID 失败：" + ex.Message;
			return false;
		}
	}

	private static void ReportCallbackPreviewError(string message)
	{
		XWEditorInterface.Instance?.AddOutputMessage(message, XWOutputPanel.MessageType.Error);
		XWEditorInterface.Instance?.ShowToast(message, 2);
	}

	private void ApplyCompileResult(XWScriptCompiler.CompileResult result)
	{
		if (result == null)
		{
			_diagnosticsLabel.Text = "编译没有返回结果";
			return;
		}
		int num = 0;
		int num2 = 0;
		foreach (XWCodeErrorChecker.ErrorData diagnostic in result.Diagnostics)
		{
			if (diagnostic.SeverityLevel == XWCodeErrorChecker.Severity.Error)
			{
				num++;
			}
			else if (diagnostic.SeverityLevel == XWCodeErrorChecker.Severity.Warning)
			{
				num2++;
			}
		}
		if (!string.IsNullOrEmpty(_currentTabKey) && _currentTabKey.GetExtension().ToLowerInvariant() == "cs")
		{
			List<XWCodeErrorChecker.ErrorData> list = XWScriptCompiler.FilterDiagnosticsForFile(result.Diagnostics, _currentTabKey);
			if (list.Count > 0)
			{
				_codeEdit.SetExternalDiagnostics(list);
			}
			else
			{
				_codeEdit.RequestValidationNow();
			}
		}
		string value = (result.IsModProject ? ("Mod " + FirstNonEmpty(result.TargetName, "脚本")) : "游戏工程");
		if (result.Cancelled)
		{
			_diagnosticsLabel.Text = $"{value} 编译已取消 · {result.ElapsedSeconds:0.0}s";
		}
		else if (result.TimedOut)
		{
			_diagnosticsLabel.Text = $"{value} 编译超时 · {result.ElapsedSeconds:0.0}s";
		}
		else if (result.Success)
		{
			_diagnosticsLabel.Text = $"{value} 编译完成 · {result.ScriptCount} 个脚本 · {num2} 个警告 · {result.ElapsedSeconds:0.0}s";
		}
		else if (num > 0 || num2 > 0)
		{
			_diagnosticsLabel.Text = $"{value} 编译失败 · {num} 个错误 · {num2} 个警告 · {result.ElapsedSeconds:0.0}s";
		}
		else
		{
			_diagnosticsLabel.Text = $"{value} 编译失败 · 退出码 {result.ExitCode} · {result.ElapsedSeconds:0.0}s";
		}
		if (!string.IsNullOrWhiteSpace(result.Output))
		{
			XWEditorInterface.Instance?.AddOutputMessage(result.Output);
		}
		UpdateDiagnosticList(result.Diagnostics, "", includeFileName: true);
	}

	private void UpdateDiagnosticList(IEnumerable<XWCodeErrorChecker.ErrorData> diagnostics, string defaultFilePath, bool includeFileName)
	{
		_diagnosticEntries.Clear();
		_diagnosticsList.Clear();
		if (diagnostics != null)
		{
			foreach (XWCodeErrorChecker.ErrorData diagnostic in diagnostics)
			{
				if (diagnostic != null)
				{
					_diagnosticEntries.Add(new DiagnosticListEntry
					{
						Diagnostic = diagnostic,
						FilePath = ResolveDiagnosticFilePath(diagnostic, defaultFilePath)
					});
				}
			}
		}
		_diagnosticEntries.Sort(CompareDiagnosticEntries);
		for (int i = 0; i < _diagnosticEntries.Count; i++)
		{
			DiagnosticListEntry diagnosticListEntry = _diagnosticEntries[i];
			int idx = _diagnosticsList.AddItem(BuildDiagnosticText(diagnosticListEntry, includeFileName), GetDiagnosticIcon(diagnosticListEntry.Diagnostic));
			_diagnosticsList.SetItemMetadata(idx, i);
		}
		_diagnosticsPanel.Visible = _diagnosticEntries.Count > 0;
		if (GodotObject.IsInstanceValid(_diagnosticsCopyButton))
		{
			_diagnosticsCopyButton.Disabled = _diagnosticEntries.Count == 0;
		}
	}

	private void OnDiagnosticListItemSelected(long index)
	{
		JumpToDiagnosticItem((int)index);
	}

	private void OnDiagnosticListItemActivated(long index)
	{
		JumpToDiagnosticItem((int)index);
	}

	private void OnDiagnosticsListGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton { Pressed: not false } inputEventMouseButton && inputEventMouseButton.ButtonIndex == MouseButton.Right)
		{
			int itemAtPosition = _diagnosticsList.GetItemAtPosition(inputEventMouseButton.Position, exact: true);
			if (itemAtPosition >= 0 && itemAtPosition < _diagnosticsList.ItemCount)
			{
				_diagnosticsList.Select(itemAtPosition);
			}
			if (GodotObject.IsInstanceValid(_diagnosticsContextMenu))
			{
				_diagnosticsContextMenu.Position = (Vector2I)(_diagnosticsList.GetScreenPosition() + inputEventMouseButton.Position);
				_diagnosticsContextMenu.Popup();
			}
			_diagnosticsList.AcceptEvent();
		}
		else if (@event is InputEventKey { Pressed: not false, Echo: false } inputEventKey && inputEventKey.Keycode == Key.C && (inputEventKey.CtrlPressed || inputEventKey.MetaPressed))
		{
			CopySelectedDiagnostics();
			_diagnosticsList.AcceptEvent();
		}
	}

	private void CopySelectedDiagnostics()
	{
		List<DiagnosticListEntry> list = GetSelectedDiagnosticEntries();
		if (list.Count == 0)
		{
			list = new List<DiagnosticListEntry>(_diagnosticEntries);
		}
		CopyDiagnosticEntriesToClipboard(list);
	}

	private void CopyAllDiagnostics()
	{
		CopyDiagnosticEntriesToClipboard(_diagnosticEntries);
	}

	private void CopyDiagnosticEntriesToClipboard(IEnumerable<DiagnosticListEntry> entries)
	{
		List<string> list = new List<string>();
		if (entries != null)
		{
			foreach (DiagnosticListEntry entry in entries)
			{
				string text = BuildDiagnosticClipboardText(entry);
				if (!string.IsNullOrWhiteSpace(text))
				{
					list.Add(text);
				}
			}
		}
		if (list.Count != 0)
		{
			DisplayServer.ClipboardSet(string.Join("\n", list));
			XWEditorInterface.Instance?.AddOutputMessage($"脚本诊断: 已复制 {list.Count} 条警告/错误", XWOutputPanel.MessageType.Editor);
		}
	}

	private List<DiagnosticListEntry> GetSelectedDiagnosticEntries()
	{
		List<DiagnosticListEntry> list = new List<DiagnosticListEntry>();
		if (!GodotObject.IsInstanceValid(_diagnosticsList))
		{
			return list;
		}
		int[] selectedItems = _diagnosticsList.GetSelectedItems();
		foreach (int itemIndex in selectedItems)
		{
			DiagnosticListEntry diagnosticEntryFromItem = GetDiagnosticEntryFromItem(itemIndex);
			if (diagnosticEntryFromItem != null)
			{
				list.Add(diagnosticEntryFromItem);
			}
		}
		return list;
	}

	private DiagnosticListEntry GetDiagnosticEntryFromItem(int itemIndex)
	{
		if (itemIndex < 0 || itemIndex >= _diagnosticsList.ItemCount)
		{
			return null;
		}
		Variant itemMetadata = _diagnosticsList.GetItemMetadata(itemIndex);
		if (itemMetadata.VariantType == Variant.Type.Nil)
		{
			return null;
		}
		int num = itemMetadata.AsInt32();
		if (num < 0 || num >= _diagnosticEntries.Count)
		{
			return null;
		}
		return _diagnosticEntries[num];
	}

	private string BuildDiagnosticClipboardText(DiagnosticListEntry entry)
	{
		if (entry?.Diagnostic == null)
		{
			return "";
		}
		XWCodeErrorChecker.ErrorData diagnostic = entry.Diagnostic;
		string value = ((diagnostic.SeverityLevel == XWCodeErrorChecker.Severity.Warning) ? "警告" : "错误");
		string text = (string.IsNullOrWhiteSpace(entry.FilePath) ? _currentTabKey : entry.FilePath);
		string value2 = (string.IsNullOrWhiteSpace(text) ? $"{Mathf.Max(0, diagnostic.Line) + 1}:{Mathf.Max(0, diagnostic.Column) + 1}" : $"{text.Replace('\\', '/')}:{Mathf.Max(0, diagnostic.Line) + 1}:{Mathf.Max(0, diagnostic.Column) + 1}");
		string value3 = (string.IsNullOrEmpty(diagnostic.Code) ? "" : (diagnostic.Code + " "));
		return $"{value} {value2} {value3}{diagnostic.Message}";
	}

	private void JumpToDiagnosticItem(int itemIndex)
	{
		if (itemIndex >= 0 && itemIndex < _diagnosticsList.ItemCount)
		{
			JumpToDiagnostic(GetDiagnosticEntryFromItem(itemIndex));
		}
	}

	private void JumpToDiagnostic(DiagnosticListEntry entry)
	{
		if (entry?.Diagnostic == null)
		{
			return;
		}
		if (!string.IsNullOrEmpty(entry.FilePath))
		{
			string text = ResolveOpenFilePath(entry.FilePath);
			if (!string.IsNullOrEmpty(text) && text != _currentTabKey && Godot.FileAccess.FileExists(text))
			{
				OpenFile(text);
			}
		}
		_codeEdit.JumpToError(entry.Diagnostic);
		_codeEdit.GrabFocus();
	}

	private void RequestCurrentValidation()
	{
		if (HasFileOpen())
		{
			_diagnosticsLabel.Text = "正在检查当前脚本...";
			_codeEdit.RequestValidationNow();
		}
	}

	private void JumpToNextDiagnostic(int direction)
	{
		if (_diagnosticEntries.Count == 0)
		{
			RequestCurrentValidation();
			return;
		}
		int caretLine = _codeEdit.GetCaretLine();
		int num = -1;
		if (direction >= 0)
		{
			for (int i = 0; i < _diagnosticEntries.Count; i++)
			{
				DiagnosticListEntry diagnosticListEntry = _diagnosticEntries[i];
				if (diagnosticListEntry.FilePath != _currentTabKey || diagnosticListEntry.Diagnostic.Line > caretLine)
				{
					num = i;
					break;
				}
			}
			if (num < 0)
			{
				num = 0;
			}
		}
		else
		{
			for (int num2 = _diagnosticEntries.Count - 1; num2 >= 0; num2--)
			{
				DiagnosticListEntry diagnosticListEntry2 = _diagnosticEntries[num2];
				if (diagnosticListEntry2.FilePath != _currentTabKey || diagnosticListEntry2.Diagnostic.Line < caretLine)
				{
					num = num2;
					break;
				}
			}
			if (num < 0)
			{
				num = _diagnosticEntries.Count - 1;
			}
		}
		_diagnosticsList.Select(num);
		_diagnosticsList.EnsureCurrentIsVisible();
		JumpToDiagnostic(_diagnosticEntries[num]);
	}

	private void OpenBlueprintWorkspace()
	{
		XWEditorInterface.Instance?.FocusPanel("bp_editor");
	}

	private string ResolveOpenFilePath(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath))
		{
			return "";
		}
		string text = XWScriptCompiler.NormalizeFilePath(filePath);
		foreach (string tabKey in _tabKeys)
		{
			if (XWScriptCompiler.NormalizeFilePath(tabKey) == text)
			{
				return tabKey;
			}
		}
		return LocalizeProjectPath(filePath);
	}

	private static string ResolveDiagnosticFilePath(XWCodeErrorChecker.ErrorData diagnostic, string defaultFilePath)
	{
		string text = (string.IsNullOrWhiteSpace(diagnostic.FilePath) ? defaultFilePath : diagnostic.FilePath);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return LocalizeProjectPath(text);
		}
		return "";
	}

	private static string LocalizeProjectPath(string filePath)
	{
		string text = filePath.Replace('\\', '/');
		if (text.StartsWith("res://"))
		{
			return text;
		}
		string text2 = ProjectSettings.GlobalizePath("res://").Replace('\\', '/');
		if (!text2.EndsWith("/"))
		{
			text2 += "/";
		}
		if (!string.IsNullOrEmpty(text2) && text.StartsWith(text2, StringComparison.OrdinalIgnoreCase))
		{
			return "res://" + text.Substring(text2.Length);
		}
		return filePath;
	}

	private static int CompareDiagnosticEntries(DiagnosticListEntry a, DiagnosticListEntry b)
	{
		int num = GetSeverityRank(a.Diagnostic).CompareTo(GetSeverityRank(b.Diagnostic));
		if (num != 0)
		{
			return num;
		}
		int num2 = string.Compare(a.FilePath, b.FilePath, StringComparison.OrdinalIgnoreCase);
		if (num2 != 0)
		{
			return num2;
		}
		int num3 = a.Diagnostic.Line.CompareTo(b.Diagnostic.Line);
		if (num3 != 0)
		{
			return num3;
		}
		int num4 = a.Diagnostic.Column.CompareTo(b.Diagnostic.Column);
		if (num4 != 0)
		{
			return num4;
		}
		return string.Compare(a.Diagnostic.Code, b.Diagnostic.Code, StringComparison.Ordinal);
	}

	private static int GetSeverityRank(XWCodeErrorChecker.ErrorData diagnostic)
	{
		return diagnostic.SeverityLevel switch
		{
			XWCodeErrorChecker.Severity.Error => 0, 
			XWCodeErrorChecker.Severity.Warning => 1, 
			_ => 2, 
		};
	}

	private string BuildDiagnosticText(DiagnosticListEntry entry, bool includeFileName)
	{
		XWCodeErrorChecker.ErrorData diagnostic = entry.Diagnostic;
		string value = ((diagnostic.SeverityLevel == XWCodeErrorChecker.Severity.Warning) ? "警告" : "错误");
		string value2 = ((includeFileName && !string.IsNullOrEmpty(entry.FilePath)) ? (entry.FilePath.Replace('\\', '/').GetFile() + " ") : "");
		string value3 = $"{Mathf.Max(0, diagnostic.Line) + 1}:{Mathf.Max(0, diagnostic.Column) + 1}";
		string value4 = (string.IsNullOrEmpty(diagnostic.Code) ? "" : (diagnostic.Code + " "));
		return $"{value} {value2}{value3} {value4}{diagnostic.Message}";
	}

	private Texture2D GetDiagnosticIcon(XWCodeErrorChecker.ErrorData diagnostic)
	{
		if (diagnostic.SeverityLevel != XWCodeErrorChecker.Severity.Warning)
		{
			return _iconStatusError;
		}
		return _iconStatusWarning;
	}

	private void UpdateCompileControls()
	{
		if (_compileButton != null)
		{
			string activeModProjectRoot = GetActiveModProjectRoot();
			bool flag = !string.IsNullOrWhiteSpace(activeModProjectRoot);
			bool flag2 = flag && XWScriptCompiler.IsModBuildBusy(activeModProjectRoot);
			bool flag3 = _debugSession?.HasLiveSession ?? false;
			bool flag4 = flag2 | flag3;
			_compileButton.Disabled = (!flag || _isCompiling) | flag2 | flag3;
			Button compileButton = _compileButton;
			string text;
			if (_isCompiling | flag2)
			{
				text = "编译中…";
			}
			else
			{
				text = (flag3 ? "调试中" : "编译 Mod");
			}
			compileButton.Text = text;
			_compileButton.TooltipText = "后台编译当前 Mod 工程中的 C# 与蓝图生成脚本，不会执行脚本";
			bool flag5 = !HasFileOpen();
			bool isCurrentDocumentReadOnly = IsCurrentDocumentReadOnly;
			if (GodotObject.IsInstanceValid(_searchMenuBtn))
			{
				_searchMenuBtn.Disabled = flag5;
			}
			if (GodotObject.IsInstanceValid(_goToMenuBtn))
			{
				_goToMenuBtn.Disabled = flag5;
			}
			if (GodotObject.IsInstanceValid(_definitionButton))
			{
				_definitionButton.Disabled = flag5;
			}
			if (GodotObject.IsInstanceValid(_referencesButton))
			{
				_referencesButton.Disabled = flag5;
			}
			if (GodotObject.IsInstanceValid(_renameButton))
			{
				_renameButton.Disabled = flag5 | isCurrentDocumentReadOnly | flag4;
			}
			if (GodotObject.IsInstanceValid(_formatButton))
			{
				_formatButton.Disabled = flag5 | isCurrentDocumentReadOnly;
			}
			if (GodotObject.IsInstanceValid(_quickFixButton))
			{
				_quickFixButton.Disabled = flag5 | isCurrentDocumentReadOnly;
			}
			XWCSharpIdeService.RenameHistoryAvailability renameHistoryAvailability = XWCSharpIdeService.GetRenameHistoryAvailability(activeModProjectRoot);
			if (GodotObject.IsInstanceValid(_renameUndoButton))
			{
				_renameUndoButton.Disabled = flag4 || !renameHistoryAvailability.CanUndo;
			}
			if (GodotObject.IsInstanceValid(_renameRedoButton))
			{
				_renameRedoButton.Disabled = flag4 || !renameHistoryAvailability.CanRedo;
			}
		}
	}

	private void OnModBuildFlightStateChanged(string projectRoot, bool _)
	{
		if (!string.Equals(NormalizeProjectRoot(GetActiveModProjectRoot()), NormalizeProjectRoot(projectRoot), StringComparison.OrdinalIgnoreCase))
		{
			return;
		}
		_scriptEditorSynchronizationContext?.Post((object? obj) =>
		{
			if (GodotObject.IsInstanceValid(this) && IsInsideTree())
			{
				UpdateCompileControls();
				UpdateDebugControls();
			}
		}, null);
	}

	public bool OpenCurrentGeneratedBlueprintSource()
	{
		if (!TryGetCurrentTab(out var tab) || !tab.IsBlueprintGenerated)
		{
			return false;
		}
		XWBlueprintGeneratedCSharpPolicy.LinkState linkState = (tab.GeneratedLinkState = XWBlueprintGeneratedCSharpPolicy.GetLinkState(tab.FilePath, tab.Text, GetActiveModProjectRoot(), out var blueprintSourcePath));
		tab.BlueprintSourcePath = ((linkState == XWBlueprintGeneratedCSharpPolicy.LinkState.Linked) ? blueprintSourcePath : "");
		UpdateGeneratedBlueprintBar(tab);
		if (linkState != XWBlueprintGeneratedCSharpPolicy.LinkState.Linked)
		{
			XWEditorInterface.Instance?.ShowToast("源蓝图不存在或不属于当前 Mod，生成代码继续保持只读。", 2);
			return false;
		}
		XWBPScript xWBPScript = ResourceLoader.Load<XWBPScript>(blueprintSourcePath, "", ResourceLoader.CacheMode.Replace);
		if (!GodotObject.IsInstanceValid(xWBPScript) || !(XWEditorInterface.Instance?.GetBlueprintEditor() is XWBPEditor xWBPEditor))
		{
			XWEditorInterface.Instance?.ShowToast("无法打开源蓝图。", 2);
			return false;
		}
		xWBPEditor.Init(xWBPScript);
		XWEditorInterface.Instance.FocusPanel("bp_editor");
		return true;
	}

	private bool CanEditCurrentDocument(bool showToast)
	{
		bool num = TryGetCurrentTab(out var tab) && !tab.IsBlueprintGenerated;
		if ((!num & showToast) && HasFileOpen())
		{
			XWEditorInterface instance = XWEditorInterface.Instance;
			if (instance == null)
			{
				return num;
			}
			instance.ShowToast("该文件由蓝图生成；请返回源蓝图修改。", 1);
		}
		return num;
	}

	private bool TryGetCurrentTab(out ScriptTabInfo tab)
	{
		if (!string.IsNullOrWhiteSpace(_currentTabKey) && _openTabs.TryGetValue(_currentTabKey, out tab))
		{
			return true;
		}
		tab = null;
		return false;
	}

	private void RefreshGeneratedMetadata(ScriptTabInfo tab)
	{
		if (tab != null)
		{
			tab.GeneratedLinkState = XWBlueprintGeneratedCSharpPolicy.GetLinkState(tab.FilePath, tab.Text, GetActiveModProjectRoot(), out var blueprintSourcePath);
			tab.BlueprintSourcePath = ((tab.GeneratedLinkState == XWBlueprintGeneratedCSharpPolicy.LinkState.Linked) ? blueprintSourcePath : "");
		}
	}

	private bool ReloadTabFromDisk(ScriptTabInfo tab)
	{
		string error = "";
		if (tab == null || !TryReadDiskSnapshot(tab.FilePath, out var snapshot, out error) || !snapshot.Exists)
		{
			LastSaveError = error;
			return false;
		}
		ApplyDiskSnapshotToTab(tab, snapshot);
		return true;
	}

	private void UpdateGeneratedBlueprintBar(ScriptTabInfo tab)
	{
		if (GodotObject.IsInstanceValid(_generatedBlueprintBar))
		{
			bool flag = tab?.IsBlueprintGenerated ?? false;
			_generatedBlueprintBar.Visible = flag;
			if (flag)
			{
				bool flag2 = tab.GeneratedLinkState == XWBlueprintGeneratedCSharpPolicy.LinkState.Linked;
				_generatedBlueprintStatusLabel.Text = (flag2 ? "蓝图生成代码 · 只读" : "孤立的蓝图生成代码 · 只读保护");
				_generatedBlueprintSourceLabel.Text = (flag2 ? ("来源：" + tab.BlueprintSourcePath) : "来源蓝图缺失、已移动或不属于当前 Mod；不会退化成可写脚本。");
				_returnToBlueprintButton.Disabled = !flag2;
				_returnToBlueprintButton.TooltipText = (flag2 ? "打开生成此 C# 文件的源蓝图" : "源蓝图链接未通过当前 Mod 安全校验");
			}
		}
	}

	private static string FirstNonEmpty(params string[] values)
	{
		foreach (string text in values)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text.Trim();
			}
		}
		return "";
	}

	private void UpdateLineCol()
	{
		int value = _codeEdit.GetCaretLine() + 1;
		int value2 = _codeEdit.GetCaretColumn() + 1;
		_lineColLabel.Text = $"行 {value}, 列 {value2}";
	}

	private void UpdateTabTitle()
	{
		if (string.IsNullOrEmpty(_currentTabKey))
		{
			return;
		}
		int num = _tabKeys.IndexOf(_currentTabKey);
		if (num < 0)
		{
			return;
		}
		string text = _currentTabKey.GetFile().GetBaseName();
		if (_openTabs.TryGetValue(_currentTabKey, out var value))
		{
			if (value.HasUnsavedChanges)
			{
				text = "(*)" + text;
			}
			if (value.HasExternalConflict)
			{
				text = "(⚠)" + text;
			}
		}
		_scriptTabBar.SetTabTitle(num, text);
	}

	private void UpdateScriptList()
	{
		_scriptList.Clear();
		foreach (string tabKey in _tabKeys)
		{
			string text = tabKey.GetFile();
			if (_openTabs.TryGetValue(tabKey, out var value) && value.HasExternalConflict)
			{
				text = "⚠ " + text;
			}
			Texture2D iconScript = _iconScript;
			int idx = _scriptList.AddItem(text, iconScript);
			_scriptList.SetItemMetadata(idx, tabKey);
			if (tabKey == _currentTabKey)
			{
				_scriptList.Select(idx);
				_scriptList.EnsureCurrentIsVisible();
			}
		}
	}

	private void UpdateScriptListSelection()
	{
		for (int i = 0; i < _scriptList.ItemCount; i++)
		{
			Variant itemMetadata = _scriptList.GetItemMetadata(i);
			if (itemMetadata.VariantType != Variant.Type.Nil && itemMetadata.As<string>() == _currentTabKey)
			{
				_scriptList.Select(i);
				_scriptList.EnsureCurrentIsVisible();
				return;
			}
		}
		_scriptList.DeselectAll();
	}

	private void OnScriptFilterChanged(string filterText)
	{
		string value = filterText.ToLowerInvariant();
		_scriptList.Clear();
		foreach (string tabKey in _tabKeys)
		{
			string text = tabKey.GetFile();
			if (string.IsNullOrEmpty(value) || text.ToLowerInvariant().Contains(value) || tabKey.ToLowerInvariant().Contains(value))
			{
				if (_openTabs.TryGetValue(tabKey, out var value2) && value2.HasExternalConflict)
				{
					text = "⚠ " + text;
				}
				Texture2D iconScript = _iconScript;
				int idx = _scriptList.AddItem(text, iconScript);
				_scriptList.SetItemMetadata(idx, tabKey);
				if (tabKey == _currentTabKey)
				{
					_scriptList.Select(idx);
				}
			}
		}
	}

	private void OnScriptListItemSelected(long idx)
	{
		Variant itemMetadata = _scriptList.GetItemMetadata((int)idx);
		if (itemMetadata.VariantType == Variant.Type.Nil)
		{
			return;
		}
		string text = itemMetadata.As<string>();
		if (!(text == _currentTabKey))
		{
			SaveCurrentTabState();
			int num = _tabKeys.IndexOf(text);
			if (num >= 0)
			{
				_isSwitchingTab = true;
				_scriptTabBar.CurrentTab = num;
				_isSwitchingTab = false;
			}
			LoadTabData(text);
		}
	}

	public bool HasFileOpen()
	{
		return !string.IsNullOrEmpty(_currentTabKey);
	}

	public string GetCurrentFilePath()
	{
		return _currentTabKey;
	}

	public static bool IsSupportedScriptPath(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return string.Equals(path.GetExtension(), "cs", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public static string NormalizeNewCSharpPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path) || !string.IsNullOrWhiteSpace(path.GetExtension()))
		{
			return path ?? string.Empty;
		}
		return path + ".cs";
	}

	public static string BuildDefaultCSharpTemplate(string path)
	{
		string baseName = (path ?? string.Empty).GetFile().GetBaseName();
		StringBuilder stringBuilder = new StringBuilder();
		string text = baseName;
		foreach (char c in text)
		{
			stringBuilder.Append((char.IsLetterOrDigit(c) || c == '_') ? c : '_');
		}
		string text2 = ((stringBuilder.Length == 0) ? "NewModScript" : stringBuilder.ToString());
		if (!char.IsLetter(text2[0]) && text2[0] != '_')
		{
			text2 = "Mod_" + text2;
		}
		if (SyntaxFacts.GetKeywordKind(text2) != SyntaxKind.None)
		{
			text2 = "Mod_" + text2;
		}
		return "using Godot;\n\npublic partial class " + text2 + " : Node\n{\n    public override void _Ready()\n    {\n    }\n}\n";
	}

	private void RejectUnsupportedScriptPath(string path, string action, bool countRequest = true)
	{
		if (countRequest)
		{
			UnsupportedScriptRequestCount++;
		}
		string extension = (path ?? string.Empty).GetExtension();
		string text = (string.IsNullOrWhiteSpace(extension) ? "无扩展名文件" : ("." + extension + " 文件"));
		XWEditorInterface.Instance?.ShowToast("脚本编辑器仅支持 C#，不能" + action + text + "。", 2);
	}

	public void LoadScript(string path)
	{
		OpenFile(path);
	}

	public XWModDebugController GetDebugController()
	{
		return _debugSession?.DebugController;
	}

	private void InitializeDebugWorkbench()
	{
		_debugWorkbench = GetNode<XWModDebugWorkbench>("%DebugWorkbench");
		_debugWorkbenchToggle = GetNode<Button>("%DebugWorkbenchToggle");
		_debugWorkbench.BindController(_debugSession?.DebugController);
		_debugWorkbench.StopRequested += OnDebugWorkbenchStopRequested;
		_debugWorkbenchToggle.Toggled += SetDebugWorkbenchVisible;
		if (_debugSession?.DebugController != null)
		{
			_debugSession.DebugController.SnapshotChanged += OnScriptDebugSnapshotChanged;
		}
		SetDebugWorkbenchVisible(visible: false);
	}

	private void ShutdownDebugWorkbench()
	{
		if (_debugSession?.DebugController != null)
		{
			_debugSession.DebugController.SnapshotChanged -= OnScriptDebugSnapshotChanged;
		}
		if (GodotObject.IsInstanceValid(_debugWorkbench))
		{
			_debugWorkbench.StopRequested -= OnDebugWorkbenchStopRequested;
			_debugWorkbench.UnbindController();
		}
		if (GodotObject.IsInstanceValid(_debugWorkbenchToggle))
		{
			_debugWorkbenchToggle.Toggled -= SetDebugWorkbenchVisible;
		}
		lock (_scriptDebugSnapshotLock)
		{
			_pendingScriptDebugSnapshot = null;
		}
		Interlocked.Exchange(ref _scriptDebugNavigationQueued, 0);
	}

	private void SetDebugWorkbenchVisible(bool visible)
	{
		if (GodotObject.IsInstanceValid(_debugWorkbench))
		{
			_debugWorkbench.Visible = visible;
		}
		if (GodotObject.IsInstanceValid(_debugWorkbenchToggle) && _debugWorkbenchToggle.ButtonPressed != visible)
		{
			_debugWorkbenchToggle.SetPressedNoSignal(visible);
		}
	}

	private async void OnDebugWorkbenchStopRequested()
	{
		await StopDebugSessionAsync();
	}

	private void OnScriptDebugSnapshotChanged(object sender, XWModDebugSnapshot snapshot)
	{
		if (snapshot == null || snapshot.State != XWModDebugState.Paused)
		{
			return;
		}
		XWModDebugCheckpoint currentCheckpoint = snapshot.CurrentCheckpoint;
		if (currentCheckpoint != null && currentCheckpoint.Kind == XWModDebugCheckpointKind.CSharpLine)
		{
			lock (_scriptDebugSnapshotLock)
			{
				_pendingScriptDebugSnapshot = snapshot;
			}
			if (Interlocked.Exchange(ref _scriptDebugNavigationQueued, 1) == 0)
			{
				CallDeferred("ApplyScriptDebugNavigation");
			}
		}
	}

	private void ApplyScriptDebugNavigation()
	{
		Interlocked.Exchange(ref _scriptDebugNavigationQueued, 0);
		XWModDebugSnapshot pendingScriptDebugSnapshot;
		lock (_scriptDebugSnapshotLock)
		{
			pendingScriptDebugSnapshot = _pendingScriptDebugSnapshot;
			_pendingScriptDebugSnapshot = null;
		}
		XWModDebugCheckpoint xWModDebugCheckpoint = pendingScriptDebugSnapshot?.CurrentCheckpoint;
		if (xWModDebugCheckpoint != null && xWModDebugCheckpoint.Kind == XWModDebugCheckpointKind.CSharpLine && IsPathInsideRoot(xWModDebugCheckpoint.SourcePath, GetActiveModProjectRoot()) && File.Exists(xWModDebugCheckpoint.SourcePath))
		{
			SetDebugWorkbenchVisible(visible: true);
			OpenFile(xWModDebugCheckpoint.SourcePath);
			_codeEdit?.SetCaretLine(Math.Max(0, xWModDebugCheckpoint.Line - 1));
			_codeEdit?.SetCaretColumn(0);
			_codeEdit?.CenterViewportToCaret();
		}
	}

	private void InitializeExternalConflictUI()
	{
		_externalConflictBar = GetNode<PanelContainer>("%ExternalConflictBar");
		_externalConflictTitleLabel = GetNode<Label>("%ExternalConflictTitleLabel");
		_externalConflictDetailLabel = GetNode<Label>("%ExternalConflictDetailLabel");
		_viewExternalDiffButton = GetNode<Button>("%ViewExternalDiffButton");
		_reloadExternalButton = GetNode<Button>("%ReloadExternalButton");
		_overwriteExternalButton = GetNode<Button>("%OverwriteExternalButton");
		_saveConflictCopyButton = GetNode<Button>("%SaveConflictCopyButton");
		_externalConflictDiffWindow = GetNode<Window>("%ExternalConflictDiffWindow");
		_externalConflictEditorText = GetNode<TextEdit>("%ExternalConflictEditorText");
		_externalConflictDiskText = GetNode<TextEdit>("%ExternalConflictDiskText");
		_viewExternalDiffButton.Pressed += ShowCurrentExternalConflictDiff;
		_reloadExternalButton.Pressed += () =>
		{
			ReloadCurrentExternalConflict();
		};
		_overwriteExternalButton.Pressed += () =>
		{
			OverwriteCurrentExternalConflict();
		};
		_saveConflictCopyButton.Pressed += SaveFileAs;
		_externalConflictDiffWindow.CloseRequested += _externalConflictDiffWindow.Hide;
		_externalConflictBar.Hide();
		_externalConflictWatcher = new Godot.Timer
		{
			Name = "ExternalConflictWatcher",
			OneShot = false,
			WaitTime = 0.85,
			Autostart = false,
			ProcessMode = ProcessModeEnum.Disabled
		};
		_externalConflictWatcher.Timeout += ScanExternalChangesFromWatcher;
		AddChild(_externalConflictWatcher, forceReadableName: false, InternalMode.Disabled);
	}

	private static string ComputeScriptContentHash(string text)
	{
		return Convert.ToHexString(SHA256.HashData(ScriptUtf8.GetBytes(text ?? "")));
	}

	private static string ResolveAbsoluteScriptPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		return Path.GetFullPath((path.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("user://", StringComparison.OrdinalIgnoreCase)) ? ProjectSettings.GlobalizePath(path) : path);
	}

	private static bool TryReadDiskSnapshot(string path, out ScriptDiskSnapshot snapshot, out string error)
	{
		snapshot = default;
		error = "";
		try
		{
			string text = ResolveAbsoluteScriptPath(path);
			if (string.IsNullOrWhiteSpace(text))
			{
				error = "脚本路径为空。";
				return false;
			}
			if (!File.Exists(text))
			{
				snapshot = new ScriptDiskSnapshot(exists: false, "", "", 0L, 0uL);
				return true;
			}
			string text2 = File.ReadAllText(text, ScriptUtf8);
			FileInfo fileInfo = new FileInfo(text);
			snapshot = new ScriptDiskSnapshot(exists: true, text2, ComputeScriptContentHash(text2), fileInfo.Length, Godot.FileAccess.GetModifiedTime(path));
			return true;
		}
		catch (Exception ex)
		{
			error = ex.Message;
			return false;
		}
	}

	private static bool DiskSnapshotMatchesBaseline(ScriptTabInfo tab, ScriptDiskSnapshot snapshot)
	{
		if (tab == null || tab.DiskExists != snapshot.Exists)
		{
			return false;
		}
		if (!snapshot.Exists)
		{
			return true;
		}
		return string.Equals(tab.DiskContentHash, snapshot.ContentHash, StringComparison.Ordinal);
	}

	private static ScriptDiskSnapshot BuildSavedSnapshot(string path, string text)
	{
		if (text == null)
		{
			text = "";
		}
		return new ScriptDiskSnapshot(exists: true, text, ComputeScriptContentHash(text), ScriptUtf8.GetByteCount(text), Godot.FileAccess.GetModifiedTime(path));
	}

	private static void ApplyDiskBaseline(ScriptTabInfo tab, ScriptDiskSnapshot snapshot)
	{
		if (tab != null)
		{
			tab.DiskExists = snapshot.Exists;
			tab.DiskContentHash = snapshot.ContentHash;
			tab.DiskByteLength = snapshot.ByteLength;
			tab.ModifiedTime = snapshot.ModifiedTime;
		}
	}

	private void ApplyDiskSnapshotToTab(ScriptTabInfo tab, ScriptDiskSnapshot snapshot)
	{
		tab.Text = snapshot.Text;
		tab.HasUnsavedChanges = false;
		ApplyDiskBaseline(tab, snapshot);
		ClearExternalConflict(tab, updateSurface: false);
		RefreshGeneratedMetadata(tab);
	}

	private void MarkExternalConflict(ScriptTabInfo tab, ScriptDiskSnapshot snapshot)
	{
		if (tab != null)
		{
			bool num = !tab.HasExternalConflict || tab.ConflictDiskExists != snapshot.Exists || !string.Equals(tab.ConflictDiskContentHash, snapshot.ContentHash, StringComparison.Ordinal);
			tab.HasExternalConflict = true;
			tab.ConflictDiskExists = snapshot.Exists;
			tab.ConflictDiskText = snapshot.Text;
			tab.ConflictDiskContentHash = snapshot.ContentHash;
			tab.ConflictDiskByteLength = snapshot.ByteLength;
			tab.ConflictModifiedTime = snapshot.ModifiedTime;
			if (num)
			{
				ExternalConflictDetectedCount++;
			}
			if (tab.FilePath == _currentTabKey)
			{
				UpdateExternalConflictSurface(tab);
			}
			UpdateTabTitle();
			UpdateScriptList();
		}
	}

	private void ClearExternalConflict(ScriptTabInfo tab, bool updateSurface = true)
	{
		if (tab != null)
		{
			tab.HasExternalConflict = false;
			tab.ConflictDiskExists = false;
			tab.ConflictDiskText = "";
			tab.ConflictDiskContentHash = "";
			tab.ConflictDiskByteLength = 0L;
			tab.ConflictModifiedTime = 0uL;
			if (updateSurface && tab.FilePath == _currentTabKey)
			{
				UpdateExternalConflictSurface(tab);
			}
		}
	}

	private void UpdateExternalConflictSurface(ScriptTabInfo tab)
	{
		if (!GodotObject.IsInstanceValid(_externalConflictBar))
		{
			return;
		}
		bool flag = tab != null && tab.HasExternalConflict && !tab.IsBlueprintGenerated;
		_externalConflictBar.Visible = flag;
		if (!flag)
		{
			if (GodotObject.IsInstanceValid(_externalConflictDiffWindow))
			{
				_externalConflictDiffWindow.Hide();
			}
			return;
		}
		_externalConflictTitleLabel.Text = (tab.ConflictDiskExists ? "检测到外部修改 · 尚未写入磁盘" : "磁盘文件已被外部删除 · 尚未重新创建");
		string value = ((tab.ConflictModifiedTime != 0) ? DateTimeOffset.FromUnixTimeSeconds((long)tab.ConflictModifiedTime).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : "未知时间");
		_externalConflictDetailLabel.Text = (tab.ConflictDiskExists ? $"{tab.FilePath.GetFile()} · {tab.ConflictDiskByteLength} 字节 · {value} · 两个版本均已保留" : (tab.FilePath.GetFile() + " · 编辑器缓冲区仍保留；重新载入不可用"));
		_reloadExternalButton.Disabled = !tab.ConflictDiskExists;
	}

	private ScriptSaveStatus RefreshTabExternalState(ScriptTabInfo tab, bool reloadVisible)
	{
		if (tab == null || tab.IsBlueprintGenerated)
		{
			return ScriptSaveStatus.Unchanged;
		}
		if (!TryReadDiskSnapshot(tab.FilePath, out var snapshot, out var error))
		{
			LastSaveError = error;
			return ScriptSaveStatus.IoError;
		}
		if (DiskSnapshotMatchesBaseline(tab, snapshot))
		{
			if (tab.HasExternalConflict)
			{
				ClearExternalConflict(tab);
			}
			return ScriptSaveStatus.Unchanged;
		}
		if (tab.HasUnsavedChanges || !snapshot.Exists)
		{
			MarkExternalConflict(tab, snapshot);
			return ScriptSaveStatus.Conflict;
		}
		ApplyDiskSnapshotToTab(tab, snapshot);
		CleanExternalReloadCount++;
		if (reloadVisible && tab.FilePath == _currentTabKey)
		{
			LoadTabData(tab.FilePath);
		}
		UpdateScriptList();
		XWEditorInterface.Instance?.ShowToast("已载入外部修改：" + tab.FilePath.GetFile());
		return ScriptSaveStatus.Unchanged;
	}

	private ScriptSaveStatus SaveTabWithResult(string tabKey, bool overwriteExternalConflict)
	{
		LastSaveError = "";
		if (!_openTabs.TryGetValue(tabKey, out var value))
		{
			return LastSaveStatus = ScriptSaveStatus.IoError;
		}
		if (value.IsBlueprintGenerated)
		{
			XWEditorInterface.Instance?.ShowToast("蓝图生成代码是只读构建产物；请返回源蓝图修改。", 1);
			return LastSaveStatus = ScriptSaveStatus.ReadOnly;
		}
		string text = ((tabKey == _currentTabKey) ? _codeEdit.Text : value.Text);
		if (!TryReadDiskSnapshot(value.FilePath, out var snapshot, out var error))
		{
			LastSaveError = error;
			XWEditorInterface.Instance?.ShowToast("读取磁盘版本失败：" + value.FilePath.GetFile(), 2);
			return LastSaveStatus = ScriptSaveStatus.IoError;
		}
		if (!overwriteExternalConflict && !DiskSnapshotMatchesBaseline(value, snapshot))
		{
			if (!value.HasUnsavedChanges && snapshot.Exists)
			{
				ApplyDiskSnapshotToTab(value, snapshot);
				CleanExternalReloadCount++;
				if (tabKey == _currentTabKey)
				{
					LoadTabData(tabKey);
				}
				UpdateScriptList();
				return LastSaveStatus = ScriptSaveStatus.Unchanged;
			}
			MarkExternalConflict(value, snapshot);
			return LastSaveStatus = ScriptSaveStatus.Conflict;
		}
		if (!TryAtomicWriteScript(value.FilePath, text, snapshot, out var error2, out var failureKind))
		{
			LastSaveError = error2;
			if (failureKind == AtomicWriteFailureKind.Conflict)
			{
				if (TryReadDiskSnapshot(value.FilePath, out var snapshot2, out var _))
				{
					MarkExternalConflict(value, snapshot2);
				}
				XWEditorInterface.Instance?.ShowToast("保存已停止：" + value.FilePath.GetFile() + " 在写入前再次发生外部修改。", 1);
				return LastSaveStatus = ScriptSaveStatus.Conflict;
			}
			XWEditorInterface.Instance?.ShowToast("保存失败：" + value.FilePath.GetFile() + " · " + error2, 2);
			return LastSaveStatus = ((failureKind == AtomicWriteFailureKind.ReadOnly) ? ScriptSaveStatus.ReadOnly : ScriptSaveStatus.IoError);
		}
		ScriptDiskSnapshot snapshot3 = BuildSavedSnapshot(value.FilePath, text);
		AtomicSaveCount++;
		if (overwriteExternalConflict)
		{
			ExternalConflictOverwriteCount++;
		}
		value.Text = text;
		value.HasUnsavedChanges = false;
		ApplyDiskBaseline(value, snapshot3);
		ClearExternalConflict(value);
		RefreshGeneratedMetadata(value);
		XWCSharpCodeModel.RequestSourceUpdate(value.FilePath, text);
		XWBPCSharpMemberRegistry.Instance.NotifyScriptSaved(value.FilePath);
		if (tabKey == _currentTabKey)
		{
			UpdateTabTitle();
		}
		UpdateScriptList();
		return LastSaveStatus = ScriptSaveStatus.Saved;
	}

	private static bool TryAtomicWriteScript(string path, string text, ScriptDiskSnapshot expectedDisk, out string error, out AtomicWriteFailureKind failureKind)
	{
		error = "";
		failureKind = AtomicWriteFailureKind.None;
		string text2 = "";
		try
		{
			string text3 = ResolveAbsoluteScriptPath(path);
			string text4 = Path.GetDirectoryName(text3) ?? "";
			if (string.IsNullOrWhiteSpace(text4) || !Directory.Exists(text4))
			{
				error = "目标目录不存在";
				failureKind = AtomicWriteFailureKind.IoError;
				return false;
			}
			if (File.Exists(text3) && (File.GetAttributes(text3) & FileAttributes.ReadOnly) != 0)
			{
				error = "目标文件为只读";
				failureKind = AtomicWriteFailureKind.ReadOnly;
				return false;
			}
			text2 = Path.Combine(text4, $".{Path.GetFileName(text3)}.{Guid.NewGuid():N}.saving");
			using (FileStream fileStream = new FileStream(text2, FileMode.CreateNew, System.IO.FileAccess.Write, FileShare.None, 4096, FileOptions.WriteThrough))
			{
				using StreamWriter streamWriter = new StreamWriter(fileStream, ScriptUtf8, 4096, leaveOpen: true);
				streamWriter.Write(text ?? "");
				streamWriter.Flush();
				fileStream.Flush(flushToDisk: true);
			}
			if (expectedDisk.Exists)
			{
				if (!File.Exists(text3))
				{
					error = "目标文件在写入前已被删除";
					failureKind = AtomicWriteFailureKind.Conflict;
					return false;
				}
				using FileStream fileStream2 = new FileStream(text3, FileMode.Open, System.IO.FileAccess.Read, FileShare.Read | FileShare.Delete);
				using StreamReader streamReader = new StreamReader(fileStream2, ScriptUtf8, detectEncodingFromByteOrderMarks: true, 4096, leaveOpen: true);
				string text5 = streamReader.ReadToEnd();
				if (fileStream2.Length != expectedDisk.ByteLength || !string.Equals(ComputeScriptContentHash(text5), expectedDisk.ContentHash, StringComparison.Ordinal))
				{
					error = "目标文件在写入前再次发生外部修改";
					failureKind = AtomicWriteFailureKind.Conflict;
					return false;
				}
				File.Replace(text2, text3, null, ignoreMetadataErrors: true);
			}
			else
			{
				if (File.Exists(text3))
				{
					error = "目标路径在写入前已被其他程序创建";
					failureKind = AtomicWriteFailureKind.Conflict;
					return false;
				}
				File.Move(text2, text3);
			}
			text2 = "";
			return true;
		}
		catch (Exception ex)
		{
			error = ex.Message;
			if (failureKind == AtomicWriteFailureKind.None)
			{
				failureKind = ((File.Exists(ResolveAbsoluteScriptPath(path)) != expectedDisk.Exists) ? AtomicWriteFailureKind.Conflict : AtomicWriteFailureKind.IoError);
			}
			return false;
		}
		finally
		{
			if (!string.IsNullOrWhiteSpace(text2))
			{
				try
				{
					if (File.Exists(text2))
					{
						File.Delete(text2);
					}
				}
				catch
				{
				}
			}
		}
	}

	public bool ReloadCurrentExternalConflict()
	{
		if (!TryGetCurrentTab(out var tab) || !tab.HasExternalConflict)
		{
			return false;
		}
		if (!TryReadDiskSnapshot(tab.FilePath, out var snapshot, out var error) || !snapshot.Exists)
		{
			LastSaveError = (string.IsNullOrWhiteSpace(error) ? "磁盘文件不存在" : error);
			XWEditorInterface.Instance?.ShowToast("无法重新载入：磁盘文件不存在或不可读。", 2);
			return false;
		}
		ApplyDiskSnapshotToTab(tab, snapshot);
		ExternalConflictReloadCount++;
		LoadTabData(tab.FilePath);
		UpdateScriptList();
		XWEditorInterface.Instance?.ShowToast("已载入磁盘版本；编辑器旧缓冲区未写入磁盘。");
		return true;
	}

	public bool OverwriteCurrentExternalConflict()
	{
		if (!TryGetCurrentTab(out var tab) || !tab.HasExternalConflict)
		{
			return false;
		}
		SaveCurrentTabState();
		return SaveTabWithResult(tab.FilePath, overwriteExternalConflict: true) == ScriptSaveStatus.Saved;
	}

	public void ShowCurrentExternalConflictDiff()
	{
		if (TryGetCurrentTab(out var tab) && tab.HasExternalConflict)
		{
			SaveCurrentTabState();
			_externalConflictEditorText.Text = tab.Text ?? "";
			_externalConflictDiskText.Text = (tab.ConflictDiskExists ? (tab.ConflictDiskText ?? "") : "（磁盘文件已被删除）");
			_externalConflictDiffWindow.Title = "C# 外部修改差异 · " + tab.FilePath.GetFile();
			_externalConflictDiffWindow.PopupCentered();
		}
	}

	public int ScanExternalChangesNowForProbe()
	{
		SaveCurrentTabState();
		int num = 0;
		foreach (ScriptTabInfo value in _openTabs.Values)
		{
			if (!value.IsBlueprintGenerated)
			{
				ExternalConflictWatcherScanCount++;
				RefreshTabExternalState(value, value.FilePath == _currentTabKey);
				if (value.HasExternalConflict)
				{
					num++;
				}
			}
		}
		return num;
	}

	public ScriptSaveStatus SaveCurrentFileAsForProbe(string path)
	{
		path = NormalizeNewCSharpPath(path);
		if (!IsSupportedScriptPath(path))
		{
			return LastSaveStatus = ScriptSaveStatus.IoError;
		}
		return SaveCurrentTabAsAtomic(path);
	}

	private void SetExternalConflictWatcherActive(bool active)
	{
		if (!GodotObject.IsInstanceValid(_externalConflictWatcher))
		{
			return;
		}
		if (active)
		{
			_externalConflictWatcher.ProcessMode = ProcessModeEnum.Inherit;
			if (_externalConflictWatcher.IsStopped())
			{
				_externalConflictWatcher.Start();
			}
		}
		else
		{
			_externalConflictWatcher.Stop();
			_externalConflictWatcher.ProcessMode = ProcessModeEnum.Disabled;
		}
	}

	private void ScanExternalChangesFromWatcher()
	{
		if (!_workspaceActive || _externalConflictWatcherScanning)
		{
			return;
		}
		_externalConflictWatcherScanning = true;
		try
		{
			SaveCurrentTabState();
			foreach (ScriptTabInfo value in _openTabs.Values)
			{
				if (!value.IsBlueprintGenerated && DiskMetadataMayHaveChanged(value))
				{
					ExternalConflictWatcherScanCount++;
					RefreshTabExternalState(value, value.FilePath == _currentTabKey);
				}
			}
		}
		finally
		{
			_externalConflictWatcherScanning = false;
		}
	}

	private static bool DiskMetadataMayHaveChanged(ScriptTabInfo tab)
	{
		try
		{
			string text = ResolveAbsoluteScriptPath(tab.FilePath);
			bool flag = File.Exists(text);
			bool flag2 = (tab.HasExternalConflict ? tab.ConflictDiskExists : tab.DiskExists);
			if (flag != flag2)
			{
				return true;
			}
			if (!flag)
			{
				return false;
			}
			FileInfo fileInfo = new FileInfo(text);
			long num = (tab.HasExternalConflict ? tab.ConflictDiskByteLength : tab.DiskByteLength);
			ulong num2 = (tab.HasExternalConflict ? tab.ConflictModifiedTime : tab.ModifiedTime);
			return fileInfo.Length != num || Godot.FileAccess.GetModifiedTime(tab.FilePath) != num2;
		}
		catch
		{
			return true;
		}
	}

	private ScriptSaveStatus SaveCurrentTabAsAtomic(string path)
	{
		LastSaveError = "";
		if (!TryGetCurrentTab(out var tab) || tab.IsBlueprintGenerated)
		{
			return LastSaveStatus = ScriptSaveStatus.ReadOnly;
		}
		if (_openTabs.ContainsKey(path) && !string.Equals(path, _currentTabKey, StringComparison.OrdinalIgnoreCase))
		{
			LastSaveError = "目标文件已在另一个标签中打开";
			SaveAsRollbackCount++;
			return LastSaveStatus = ScriptSaveStatus.IoError;
		}
		SaveCurrentTabState();
		string currentTabKey = _currentTabKey;
		string text = tab.Text;
		if (!TryReadDiskSnapshot(path, out var snapshot, out var error))
		{
			LastSaveError = error;
			SaveAsRollbackCount++;
			return LastSaveStatus = ScriptSaveStatus.IoError;
		}
		if (!TryAtomicWriteScript(path, text, snapshot, out var error2, out var failureKind))
		{
			LastSaveError = error2;
			SaveAsRollbackCount++;
			XWEditorInterface.Instance?.ShowToast("另存为失败；原标签保持不变：" + LastSaveError, 2);
			return LastSaveStatus = failureKind switch
			{
				AtomicWriteFailureKind.Conflict => ScriptSaveStatus.Conflict, 
				AtomicWriteFailureKind.ReadOnly => ScriptSaveStatus.ReadOnly, 
				_ => ScriptSaveStatus.IoError, 
			};
		}
		ScriptDiskSnapshot snapshot2 = BuildSavedSnapshot(path, text);
		int num = _tabKeys.IndexOf(currentTabKey);
		_openTabs.Remove(currentTabKey);
		_openTabs[path] = tab;
		if (num >= 0)
		{
			_tabKeys[num] = path;
		}
		tab.FilePath = path;
		tab.FileExtension = path.GetExtension();
		tab.Text = text;
		tab.HasUnsavedChanges = false;
		ApplyDiskBaseline(tab, snapshot2);
		ClearExternalConflict(tab, updateSurface: false);
		_currentTabKey = path;
		AtomicSaveCount++;
		XWCSharpCodeModel.RequestSourceUpdate(path, text);
		XWBPCSharpMemberRegistry.Instance.NotifyScriptSaved(path);
		LoadTabData(path);
		UpdateScriptList();
		return LastSaveStatus = ScriptSaveStatus.Saved;
	}

	private void InitializeIdeFeatures()
	{
		_debugHost = XWScriptDebugHost.GetOrCreate(this);
		if (!GodotObject.IsInstanceValid(_debugHost))
		{
			throw new InvalidOperationException("Could not create the ModEditor script debug host.");
		}
		_debugPreviewRoot = _debugHost.CreatePreviewRoot();
		_debugSession = new XWScriptDebugSession(_debugHost, _debugPreviewRoot);
		InitializeDebugWorkbench();
		_definitionButton = GetNode<Button>("%DefinitionButton");
		_referencesButton = GetNode<Button>("%ReferencesButton");
		_renameButton = GetNode<Button>("%RenameButton");
		_renameUndoButton = GetNodeOrNull<Button>("%RenameUndoButton");
		_renameRedoButton = GetNodeOrNull<Button>("%RenameRedoButton");
		_formatButton = GetNode<Button>("%FormatButton");
		_quickFixButton = GetNode<Button>("%QuickFixButton");
		_debugStartButton = GetNode<Button>("%DebugStartButton");
		_debugStopButton = GetNode<Button>("%DebugStopButton");
		_debugStatusLabel = GetNode<Label>("%DebugStatusLabel");
		_referencesWindow = GetNode<Window>("%ReferencesWindow");
		_referencesHeader = GetNode<Label>("%ReferencesHeader");
		_referencesList = GetNode<ItemList>("%ReferencesList");
		_renameDialog = GetNode<ConfirmationDialog>("%RenameDialog");
		_renameInput = GetNode<LineEdit>("%RenameInput");
		_renamePreviewLabel = GetNode<RichTextLabel>("%RenamePreviewLabel");
		_symbolInfoPopup = GetNode<PopupPanel>("%SymbolInfoPopup");
		_symbolInfoLabel = GetNode<RichTextLabel>("%SymbolInfoLabel");
		_quickFixPopup = GetNode<PopupMenu>("%QuickFixPopup");
		XWClassRegistry instance = XWClassRegistry.Instance;
		_definitionButton.Icon = instance.GetUIIcon("MemberMethod");
		_referencesButton.Icon = instance.GetUIIcon("Search");
		_renameButton.Icon = instance.GetUIIcon("Rename");
		if (_renameUndoButton != null)
		{
			_renameUndoButton.Icon = instance.GetUIIcon("Back");
		}
		if (_renameRedoButton != null)
		{
			_renameRedoButton.Icon = instance.GetUIIcon("Forward");
		}
		_formatButton.Icon = instance.GetUIIcon("CombineLines");
		_quickFixButton.Icon = instance.GetUIIcon("StatusWarning");
		_debugStartButton.Icon = instance.GetUIIcon("Debug");
		_debugStopButton.Icon = instance.GetUIIcon("Stop");
		_hoverTimer = new Godot.Timer
		{
			OneShot = true,
			WaitTime = 0.45
		};
		_hoverTimer.Timeout += RequestHoverFromTimer;
		AddChild(_hoverTimer, forceReadableName: false, InternalMode.Disabled);
		_renamePreviewTimer = new Godot.Timer
		{
			OneShot = true,
			WaitTime = 0.28
		};
		_renamePreviewTimer.Timeout += RunDebouncedRenamePreview;
		AddChild(_renamePreviewTimer, forceReadableName: false, InternalMode.Disabled);
		_definitionButton.Pressed += () =>
		{
			NavigateToDefinitionAtCaretAsync();
		};
		_referencesButton.Pressed += () =>
		{
			ShowReferencesAtCaretAsync();
		};
		_renameButton.Pressed += () =>
		{
			ShowRenameDialogAtCaretAsync();
		};
		if (_renameUndoButton != null)
		{
			_renameUndoButton.Pressed += () =>
			{
				UndoLastProjectRenameAsync();
			};
		}
		if (_renameRedoButton != null)
		{
			_renameRedoButton.Pressed += () =>
			{
				RedoLastProjectRenameAsync();
			};
		}
		_formatButton.Pressed += () =>
		{
			FormatDocumentOrSelection();
		};
		_quickFixButton.Pressed += ShowQuickFixMenu;
		_debugStartButton.Pressed += () =>
		{
			StartDebugSessionAsync();
		};
		_debugStopButton.Pressed += async () =>
		{
			await StopDebugSessionAsync();
		};
		_referencesList.ItemActivated += JumpToReference;
		_referencesWindow.CloseRequested += _referencesWindow.Hide;
		_renameInput.TextChanged += (string _) =>
		{
			RequestRenamePreview();
		};
		_renameDialog.Confirmed += () =>
		{
			ApplyPendingRenameAsync();
		};
		_quickFixPopup.IdPressed += (long _) =>
		{
			ApplyPendingQuickFix();
		};
		_codeEdit.GuiInput += OnCodeEditIdeGuiInput;
		_codeEdit.TextChanged += InvalidateIdeRequests;
		_scriptTabBar.TabChanged += (long _) =>
		{
			InvalidateIdeRequests();
		};
		UpdateDebugControls();
	}

	private void SuspendIdeBackgroundWork()
	{
		InvalidateIdeRequests();
		_hoverTimer?.Stop();
		_renamePreviewTimer?.Stop();
		if (_hoverTimer != null)
		{
			_hoverTimer.ProcessMode = ProcessModeEnum.Disabled;
		}
		if (_renamePreviewTimer != null)
		{
			_renamePreviewTimer.ProcessMode = ProcessModeEnum.Disabled;
		}
		if (GodotObject.IsInstanceValid(_symbolInfoPopup))
		{
			_symbolInfoPopup.Hide();
		}
	}

	private void ResumeIdeBackgroundWork()
	{
		if (_hoverTimer != null)
		{
			_hoverTimer.ProcessMode = ProcessModeEnum.Inherit;
		}
		if (_renamePreviewTimer != null)
		{
			_renamePreviewTimer.ProcessMode = ProcessModeEnum.Inherit;
		}
	}

	private bool HandleIdeShortcut(InputEventKey key)
	{
		if (key.Keycode == Key.F12 && key.ShiftPressed)
		{
			Button referencesButton = _referencesButton;
			if (referencesButton != null && referencesButton.Disabled)
			{
				return false;
			}
			ShowReferencesAtCaretAsync();
			return true;
		}
		if (key.Keycode == Key.F12)
		{
			Button definitionButton = _definitionButton;
			if (definitionButton != null && definitionButton.Disabled)
			{
				return false;
			}
			NavigateToDefinitionAtCaretAsync();
			return true;
		}
		if (key.Keycode == Key.F2)
		{
			Button renameButton = _renameButton;
			if (renameButton != null && renameButton.Disabled)
			{
				return false;
			}
			ShowRenameDialogAtCaretAsync();
			return true;
		}
		if (key.Keycode == Key.F && key.AltPressed && key.ShiftPressed)
		{
			FormatDocumentOrSelection();
			return true;
		}
		if (key.Keycode == Key.Period && key.CtrlPressed)
		{
			ShowQuickFixMenu();
			return true;
		}
		if (key.Keycode == Key.F9)
		{
			_codeEdit.ToggleBreakpoint(_codeEdit.GetCaretLine());
			return true;
		}
		if (key.Keycode == Key.F5 && key.ShiftPressed)
		{
			Button debugStopButton = _debugStopButton;
			if (debugStopButton != null && debugStopButton.Disabled)
			{
				return false;
			}
			StopDebugSessionAsync();
			return true;
		}
		if (key.Keycode == Key.F5)
		{
			Button debugStartButton = _debugStartButton;
			if (debugStartButton != null && debugStartButton.Disabled)
			{
				return false;
			}
			StartDebugSessionAsync();
			return true;
		}
		if (key.Keycode == Key.Z && key.CtrlPressed && key.AltPressed)
		{
			UndoLastProjectRenameAsync();
			return true;
		}
		if (key.Keycode == Key.Y && key.CtrlPressed && key.AltPressed)
		{
			RedoLastProjectRenameAsync();
			return true;
		}
		return false;
	}

	public async Task<bool> NavigateToDefinitionAtCaretAsync()
	{
		IdeSymbolQueryResult ideSymbolQueryResult = await QuerySymbolAtCaretAsync(includeReferences: false, IdeRequestKind.Definition);
		if (!ideSymbolQueryResult.IsCurrent)
		{
			return false;
		}
		XWCSharpIdeService.SymbolInfoResult info = ideSymbolQueryResult.Info;
		if (info?.Definition == null)
		{
			XWEditorInterface.Instance?.ShowToast("当前符号没有可跳转的 Mod 源码定义");
			return false;
		}
		OpenFileAt(info.Definition.FilePath, info.Definition.Line, info.Definition.Column);
		return true;
	}

	public async Task<int> ShowReferencesAtCaretAsync()
	{
		IdeSymbolQueryResult ideSymbolQueryResult = await QuerySymbolAtCaretAsync(includeReferences: true, IdeRequestKind.References);
		if (!ideSymbolQueryResult.IsCurrent)
		{
			return 0;
		}
		XWCSharpIdeService.SymbolInfoResult info = ideSymbolQueryResult.Info;
		_referenceLocations.Clear();
		_referencesList.Clear();
		if (info == null)
		{
			XWEditorInterface.Instance?.ShowToast("光标处没有可解析的 C# 符号");
			return 0;
		}
		_referenceLocations.AddRange(info.References);
		_referencesHeader.Text = $"{info.Display} · {_referenceLocations.Count} 处（仅当前 Mod）";
		string projectRoot = ideSymbolQueryResult.Stamp.ProjectRoot;
		foreach (XWCSharpIdeService.LocationResult referenceLocation in _referenceLocations)
		{
			string value = SafeRelativePath(projectRoot, referenceLocation.FilePath);
			_referencesList.AddItem($"{value}:{referenceLocation.Line + 1}:{referenceLocation.Column + 1}   {referenceLocation.Preview}");
		}
		_referencesWindow.PopupCentered();
		return _referenceLocations.Count;
	}

	private void JumpToReference(long index)
	{
		if (index >= 0 && index < _referenceLocations.Count)
		{
			XWCSharpIdeService.LocationResult locationResult = _referenceLocations[(int)index];
			OpenFileAt(locationResult.FilePath, locationResult.Line, locationResult.Column);
			_referencesWindow.Hide();
		}
	}

	internal async Task<XWCSharpIdeService.SymbolInfoResult> GetHoverAndSignatureAtCaretAsync()
	{
		IdeSymbolQueryResult ideSymbolQueryResult = await QuerySymbolAtCaretAsync(includeReferences: false, IdeRequestKind.HoverSignature);
		return ideSymbolQueryResult.IsCurrent ? ideSymbolQueryResult.Info : null;
	}

	private async Task<IdeSymbolQueryResult> QuerySymbolAtCaretAsync(bool includeReferences, IdeRequestKind requestKind)
	{
		if (!TryCaptureIdeRequest(out var stamp, out var documents))
		{
			return default;
		}
		try
		{
			XWCSharpIdeService.SymbolInfoResult symbolInfoResult = ((!includeReferences) ? (await XWCSharpIdeService.GetSymbolInfoAsync(stamp.ProjectRoot, stamp.FilePath, stamp.Source, stamp.Offset, documents)) : (await XWCSharpIdeService.FindReferencesAsync(stamp.ProjectRoot, stamp.FilePath, stamp.Source, stamp.Offset, documents)));
			XWCSharpIdeService.SymbolInfoResult info = symbolInfoResult;
			await WaitForIdeResultGateForProbeAsync(requestKind, stamp.FilePath);
			if (!IsIdeRequestCurrent(stamp, requireCaretOffset: true))
			{
				return default;
			}
			LastSymbolInfo = info;
			return new IdeSymbolQueryResult(isCurrent: true, stamp, info);
		}
		catch (Exception ex)
		{
			if (IsIdeRequestCurrent(stamp, requireCaretOffset: true))
			{
				XWEditorInterface.Instance?.ShowToast(ex.GetBaseException().Message, 1);
				return new IdeSymbolQueryResult(isCurrent: true, stamp, null);
			}
			return default;
		}
	}

	private void OnCodeEditIdeGuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion inputEventMouseMotion && HasFileOpen())
		{
			Vector2I lineColumnAtPos = _codeEdit.GetLineColumnAtPos(new Vector2I(Mathf.RoundToInt(inputEventMouseMotion.Position.X), Mathf.RoundToInt(inputEventMouseMotion.Position.Y)), allowOutOfBounds: true);
			_hoverLocalPosition = inputEventMouseMotion.Position;
			_hoverLine = Mathf.Clamp(lineColumnAtPos.X, 0, Math.Max(0, _codeEdit.GetLineCount() - 1));
			_hoverColumn = Mathf.Clamp(lineColumnAtPos.Y, 0, _codeEdit.GetLine(_hoverLine).Length);
			_hoverGeneration++;
			_hoverTimer.Stop();
			_hoverTimer.Start();
			if (_symbolInfoPopup.Visible)
			{
				_symbolInfoPopup.Hide();
			}
		}
	}

	private void RequestHoverFromTimer()
	{
		RequestHoverAsync(_hoverGeneration);
	}

	private async Task<bool> RequestHoverAsync(int generation)
	{
		if (!TryGetIdeContext(out var _, out var _, out var source, out var _))
		{
			return false;
		}
		int textOffset = XWCSharpIdeService.GetTextOffset(source, _hoverLine, _hoverColumn);
		if (!TryCaptureIdeRequest(out var stamp, out var documents, textOffset))
		{
			return false;
		}
		Vector2 popupPosition = _hoverLocalPosition;
		XWCSharpIdeService.SymbolInfoResult info;
		try
		{
			info = await XWCSharpIdeService.GetSymbolInfoAsync(stamp.ProjectRoot, stamp.FilePath, stamp.Source, stamp.Offset, documents);
			await WaitForIdeResultGateForProbeAsync(IdeRequestKind.HoverPopup, stamp.FilePath);
		}
		catch
		{
			return false;
		}
		if (generation != _hoverGeneration || info == null || !IsIdeRequestCurrent(stamp, requireCaretOffset: false))
		{
			return false;
		}
		LastSymbolInfo = info;
		_symbolInfoLabel.Text = BuildSymbolInfoText(info);
		Vector2 vector = _codeEdit.GetScreenPosition() + popupPosition + new Vector2(14f, 20f);
		_symbolInfoPopup.Popup(new Rect2I((Vector2I)vector, new Vector2I(520, 160)));
		return true;
	}

	internal Task<bool> RequestHoverForProbeAsync(int line, int column)
	{
		if (_codeEdit == null)
		{
			return Task.FromResult(result: false);
		}
		_hoverLine = Mathf.Clamp(line, 0, Math.Max(0, _codeEdit.GetLineCount() - 1));
		_hoverColumn = Mathf.Clamp(column, 0, _codeEdit.GetLine(_hoverLine).Length);
		_hoverGeneration++;
		return RequestHoverAsync(_hoverGeneration);
	}

	internal async Task<XWCSharpIdeService.RenamePreview> PreviewRenameAtCaretAsync(string newName)
	{
		int generation = BeginRenamePreviewRequest();
		return await PreviewRenameAtCaretAsync(newName, generation);
	}

	private async Task<XWCSharpIdeService.RenamePreview> PreviewRenameAtCaretAsync(string newName, int generation)
	{
		if (!TryCaptureIdeRequest(out var stamp, out var documents))
		{
			return null;
		}
		try
		{
			XWCSharpIdeService.RenamePreview preview = await XWCSharpIdeService.PreviewRenameAsync(stamp.ProjectRoot, stamp.FilePath, stamp.Source, stamp.Offset, newName, documents);
			await WaitForIdeResultGateForProbeAsync(IdeRequestKind.RenamePreview, newName);
			if (generation != _renameGeneration || !IsIdeRequestCurrent(stamp, requireCaretOffset: true))
			{
				return null;
			}
			_pendingRename = preview;
			_pendingRenameStamp = stamp;
			_pendingRenameGeneration = generation;
			return preview;
		}
		catch (Exception ex)
		{
			if (generation == _renameGeneration && IsIdeRequestCurrent(stamp, requireCaretOffset: true))
			{
				XWEditorInterface.Instance?.ShowToast(ex.GetBaseException().Message, 1);
			}
			return null;
		}
	}

	private async Task ShowRenameDialogAtCaretAsync()
	{
		if (!CanEditCurrentDocument(showToast: true))
		{
			return;
		}
		IdeSymbolQueryResult ideSymbolQueryResult = await QuerySymbolAtCaretAsync(includeReferences: false, IdeRequestKind.RenameSymbol);
		if (ideSymbolQueryResult.IsCurrent)
		{
			XWCSharpIdeService.SymbolInfoResult info = ideSymbolQueryResult.Info;
			if (info == null || info.Definition == null)
			{
				XWEditorInterface.Instance?.ShowToast("只能重命名当前 Mod 工程中定义的符号");
				return;
			}
			_pendingRename = null;
			_pendingRenameStamp = null;
			_renameInput.Text = info.Name;
			_renameInput.SelectAll();
			_renamePreviewLabel.Text = "[b]" + EscapeBbcode(info.Display) + "[/b]\n输入新名称后显示受影响文件与引用。";
			_renameDialog.GetOkButton().Disabled = true;
			_renameDialog.PopupCentered();
			_renameInput.GrabFocus();
		}
	}

	private void RequestRenamePreview()
	{
		if (_renameDialog.Visible)
		{
			_pendingRenameText = _renameInput.Text;
			BeginRenamePreviewRequest();
			_renamePreviewTimer.Stop();
			_renamePreviewTimer.Start();
		}
	}

	private void RunDebouncedRenamePreview()
	{
		if (_renameDialog.Visible)
		{
			int renameGeneration = _renameGeneration;
			UpdateRenamePreviewAsync(renameGeneration, _pendingRenameText);
		}
	}

	private async Task UpdateRenamePreviewAsync(int generation, string newName)
	{
		XWCSharpIdeService.RenamePreview preview = await PreviewRenameAtCaretAsync(newName, generation);
		if (!GodotObject.IsInstanceValid(this) || generation != _renameGeneration)
		{
			return;
		}
		XWCSharpIdeService.RenamePreview renamePreview = preview;
		if (renamePreview != null && renamePreview.ReadOnlyGeneratedDocuments.Count > 0)
		{
			_renameDialog.GetOkButton().Disabled = true;
			_renamePreviewLabel.Text = "[color=#ff8f70][b]重命名涉及蓝图生成代码，操作已中止[/b][/color]\n请先返回对应蓝图修改名称并重新生成：\n" + string.Join("\n", preview.ReadOnlyGeneratedDocuments.Select((string path) => "• " + EscapeBbcode(SafeRelativePath(preview.ProjectRoot, path))));
			return;
		}
		bool flag = (preview?.IsValid ?? false) && !string.Equals(preview.OldName, preview.NewName, StringComparison.Ordinal);
		List<string> list = (flag ? GetDirtyAffectedDocuments(preview) : new List<string>());
		if (list.Count > 0)
		{
			flag = false;
		}
		_renameDialog.GetOkButton().Disabled = !flag;
		if (list.Count > 0)
		{
			_renamePreviewLabel.Text = "[color=#ff8f70][b]受影响的标签仍有未保存修改[/b][/color]\n请先保存这些文件，再重新预览重命名：\n" + string.Join("\n", list.Select((string path) => "• " + EscapeBbcode(SafeRelativePath(GetActiveModProjectRoot(), path))));
			return;
		}
		if (!flag)
		{
			_renamePreviewLabel.Text = "请输入有效且不同的 C# 标识符。";
			return;
		}
		_renamePreviewLabel.Text = $"[b]{EscapeBbcode(preview.SymbolDisplay)}[/b]\n[color=#e9c46a]{preview.OldName} → {preview.NewName}[/color]\n{preview.UpdatedDocuments.Count} 个文件，{preview.Occurrences.Count} 处引用\n\n" + string.Join("\n", preview.UpdatedDocuments.Keys.Select((string path) => "• " + EscapeBbcode(SafeRelativePath(preview.ProjectRoot, path))));
	}

	public async Task<bool> ApplyPendingRenameAsync()
	{
		if (!CanEditCurrentDocument(showToast: true))
		{
			return false;
		}
		if (XWScriptCompiler.IsModBuildBusy(GetActiveModProjectRoot()) || (_debugSession?.HasLiveSession ?? false))
		{
			XWEditorInterface.Instance?.ShowToast("构建或调试期间不能应用项目重命名。", 1);
			UpdateCompileControls();
			return false;
		}
		XWCSharpIdeService.RenamePreview preview = _pendingRename;
		IdeRequestStamp? pendingRenameStamp = _pendingRenameStamp;
		if (preview == null || !preview.IsValid || !pendingRenameStamp.HasValue || _pendingRenameGeneration != _renameGeneration || !IsIdeRequestCurrent(pendingRenameStamp.GetValueOrDefault(), requireCaretOffset: true) || (_renameDialog.Visible && !string.Equals(_renameInput.Text, preview.NewName, StringComparison.Ordinal)))
		{
			_pendingRename = null;
			_pendingRenameStamp = null;
			if (GodotObject.IsInstanceValid(_renameDialog))
			{
				_renameDialog.GetOkButton().Disabled = true;
			}
			XWEditorInterface.Instance?.ShowToast("重命名预览已过期，请重新预览。", 1);
			return false;
		}
		if (GetDirtyAffectedDocuments(preview).Count > 0)
		{
			XWEditorInterface.Instance?.ShowToast("受影响脚本仍有未保存修改；请先保存后重新预览。", 1);
			return false;
		}
		bool flag = await XWCSharpIdeService.ApplyRenameAsync(preview);
		if (!GodotObject.IsInstanceValid(this))
		{
			return false;
		}
		if (flag)
		{
			ReloadOpenDocumentsFromDisk();
			UpdateCompileControls();
			XWEditorInterface.Instance?.ShowToast($"已重命名 {preview.OldName} → {preview.NewName}；Ctrl+Alt+Z 可恢复");
		}
		else
		{
			XWEditorInterface.Instance?.ShowToast(XWCSharpIdeService.LastRenameError, 2);
		}
		return flag;
	}

	public async Task<bool> UndoLastProjectRenameAsync()
	{
		if (!CanEditCurrentDocument(showToast: true))
		{
			return false;
		}
		string activeModProjectRoot = GetActiveModProjectRoot();
		if (XWScriptCompiler.IsModBuildBusy(activeModProjectRoot) || (_debugSession?.HasLiveSession ?? false))
		{
			XWEditorInterface.Instance?.ShowToast("构建或调试期间不能撤销项目重命名。", 1);
			UpdateCompileControls();
			return false;
		}
		bool flag = await XWCSharpIdeService.UndoLastRenameAsync(activeModProjectRoot);
		if (flag && GodotObject.IsInstanceValid(this))
		{
			ReloadOpenDocumentsFromDisk();
			UpdateCompileControls();
			XWEditorInterface.Instance?.ShowToast("已撤销项目重命名；Ctrl+Alt+Y 可重做");
		}
		return flag;
	}

	public async Task<bool> RedoLastProjectRenameAsync()
	{
		if (!CanEditCurrentDocument(showToast: true))
		{
			return false;
		}
		string activeModProjectRoot = GetActiveModProjectRoot();
		if (XWScriptCompiler.IsModBuildBusy(activeModProjectRoot) || (_debugSession?.HasLiveSession ?? false))
		{
			XWEditorInterface.Instance?.ShowToast("构建或调试期间不能重做项目重命名。", 1);
			UpdateCompileControls();
			return false;
		}
		bool flag = await XWCSharpIdeService.RedoLastRenameAsync(activeModProjectRoot);
		if (flag && GodotObject.IsInstanceValid(this))
		{
			ReloadOpenDocumentsFromDisk();
			UpdateCompileControls();
			XWEditorInterface.Instance?.ShowToast("已重做项目重命名");
		}
		return flag;
	}

	public bool FormatDocumentOrSelection()
	{
		if (!HasFileOpen() || !_currentTabKey.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) || !CanEditCurrentDocument(showToast: true))
		{
			return false;
		}
		string text = (_codeEdit.HasSelection() ? _codeEdit.GetSelectedText() : _codeEdit.Text);
		string text2;
		try
		{
			text2 = XWCSharpIdeService.FormatCSharp(text);
		}
		catch
		{
			return false;
		}
		if (string.Equals(text, text2, StringComparison.Ordinal))
		{
			return true;
		}
		_codeEdit.BeginComplexOperation();
		try
		{
			if (!_codeEdit.HasSelection())
			{
				_codeEdit.SelectAll();
			}
			_codeEdit.InsertAtCursor(text2);
		}
		finally
		{
			_codeEdit.EndComplexOperation();
		}
		XWEditorInterface.Instance?.ShowToast("已格式化；一次撤销可恢复");
		return true;
	}

	public bool ApplyQuickFixAtCaret()
	{
		XWCodeErrorChecker.ErrorData diagnostic = FindDiagnosticAtCaret();
		return ApplyQuickFix(diagnostic);
	}

	private void ShowQuickFixMenu()
	{
		if (CanEditCurrentDocument(showToast: true))
		{
			_pendingQuickFixDiagnostic = FindDiagnosticAtCaret();
			if (XWCSharpIdeService.TryBuildQuickFix(_codeEdit.Text, _pendingQuickFixDiagnostic, out var title) == null)
			{
				XWEditorInterface.Instance?.ShowToast("当前诊断没有安全的自动修复");
				return;
			}
			_quickFixPopup.Clear();
			_quickFixPopup.AddItem("⚡ " + title, 0, Key.None);
			Vector2 vector = _quickFixButton.GetScreenPosition() + new Vector2(0f, _quickFixButton.Size.Y);
			_quickFixPopup.Position = (Vector2I)vector;
			_quickFixPopup.Popup();
		}
	}

	private void ApplyPendingQuickFix()
	{
		ApplyQuickFix(_pendingQuickFixDiagnostic);
	}

	private bool ApplyQuickFix(XWCodeErrorChecker.ErrorData diagnostic)
	{
		if (!CanEditCurrentDocument(showToast: true))
		{
			return false;
		}
		string text = XWCSharpIdeService.TryBuildQuickFix(_codeEdit.Text, diagnostic, out var title);
		if (text == null)
		{
			return false;
		}
		_codeEdit.BeginComplexOperation();
		try
		{
			_codeEdit.SelectAll();
			_codeEdit.InsertAtCursor(text);
		}
		finally
		{
			_codeEdit.EndComplexOperation();
		}
		XWEditorInterface.Instance?.ShowToast("已应用：" + title + "；一次撤销可恢复");
		return true;
	}

	private XWCodeErrorChecker.ErrorData FindDiagnosticAtCaret()
	{
		List<XWCodeErrorChecker.ErrorData> allErrors = _codeEdit.GetAllErrors();
		int line = _codeEdit.GetCaretLine();
		return allErrors.FirstOrDefault((XWCodeErrorChecker.ErrorData item) => item.Line == line) ?? allErrors.FirstOrDefault();
	}

	public IReadOnlyList<XWScriptDebugSession.Breakpoint> GetProjectBreakpoints()
	{
		SaveCurrentTabState();
		string activeModProjectRoot = GetActiveModProjectRoot();
		List<XWScriptDebugSession.Breakpoint> list = new List<XWScriptDebugSession.Breakpoint>();
		foreach (ScriptTabInfo value in _openTabs.Values)
		{
			if (IsPathInsideRoot(value.FilePath, activeModProjectRoot))
			{
				int[] array = value.Breakpoints ?? Array.Empty<int>();
				foreach (int line in array)
				{
					list.Add(new XWScriptDebugSession.Breakpoint
					{
						FilePath = value.FilePath,
						Line = line
					});
				}
			}
		}
		return list;
	}

	public async Task<XWScriptDebugSession.StartResult> StartDebugSessionAsync()
	{
		string activeModProjectRoot = GetActiveModProjectRoot();
		if (string.IsNullOrWhiteSpace(activeModProjectRoot))
		{
			LastDebugStartResult = new XWScriptDebugSession.StartResult
			{
				Message = "请先打开一个 Mod 工程。"
			};
			XWEditorInterface.Instance?.ShowToast(LastDebugStartResult.Message, 1);
			return LastDebugStartResult;
		}
		if (SaveAllTabs() < 0)
		{
			DebugSaveGateCount++;
			LastDebugStartResult = new XWScriptDebugSession.StartResult
			{
				Message = "调试未启动：请先处理 C# 外部修改冲突或保存失败。"
			};
			_debugStatusLabel.Text = "● 保存冲突";
			XWEditorInterface.Instance?.ShowToast(LastDebugStartResult.Message, 1);
			return LastDebugStartResult;
		}
		_debugStartButton.Disabled = true;
		_debugStatusLabel.Text = "◌ 正在后台编译…";
		if (_debugSession == null)
		{
			LastDebugStartResult = new XWScriptDebugSession.StartResult
			{
				Message = "脚本调试宿主不可用。"
			};
			UpdateDebugControls();
			XWEditorInterface.Instance?.ShowToast(LastDebugStartResult.Message, 2);
			return LastDebugStartResult;
		}
		DebugStartInvocationCount++;
		CancellationToken cancellationToken = _buildLifetimeCts?.Token ?? CancellationToken.None;
		if (!UnloadCallbackPreview(reportFailure: true))
		{
			LastDebugStartResult = new XWScriptDebugSession.StartResult
			{
				Message = "调试未启动：状态机动作仍被预览或运行时使用。"
			};
			UpdateDebugControls();
			return LastDebugStartResult;
		}
		LastDebugStartResult = await _debugSession.StartAsync(activeModProjectRoot, GetProjectBreakpoints(), cancellationToken);
		if (!GodotObject.IsInstanceValid(this))
		{
			return LastDebugStartResult;
		}
		if (LastDebugStartResult.CompileResult != null && !LastDebugStartResult.CompileResult.Success)
		{
			ApplyCompileResult(LastDebugStartResult.CompileResult);
		}
		if (LastDebugStartResult.Success)
		{
			SetDebugWorkbenchVisible(visible: true);
		}
		UpdateDebugControls();
		XWEditorInterface.Instance?.ShowToast(LastDebugStartResult.Message, (!LastDebugStartResult.Success) ? 2 : 0);
		return LastDebugStartResult;
	}

	public async Task<bool> StopDebugSessionAsync(int timeoutMilliseconds = 2500)
	{
		if (_debugSession == null)
		{
			return true;
		}
		_debugStopButton.Disabled = true;
		_debugStatusLabel.Text = "◌ 正在取消入口并卸载…";
		bool stopped = await _debugSession.StopAsync(timeoutMilliseconds);
		if (!GodotObject.IsInstanceValid(this))
		{
			return stopped;
		}
		if (stopped)
		{
			XWScriptDebugSession.StartResult lastDebugStartResult = LastDebugStartResult;
			if (lastDebugStartResult != null && lastDebugStartResult.CompileResult?.Success == true)
			{
				await RefreshCallbackPreviewAsync(GetActiveModProjectRoot(), LastDebugStartResult.CompileResult.OutputAssemblyPath, _buildLifetimeCts?.Token ?? CancellationToken.None);
			}
		}
		UpdateDebugControls();
		if (stopped)
		{
			XWEditorInterface.Instance?.ShowToast("调试程序集已卸载");
		}
		else
		{
			XWEditorInterface.Instance?.ShowToast(_debugSession.StatusText, 2);
		}
		return stopped;
	}

	public override void _ExitTree()
	{
		CancellationTokenSource buildLifetimeCts = _buildLifetimeCts;
		_buildLifetimeCts = null;
		if (buildLifetimeCts != null)
		{
			try
			{
				buildLifetimeCts.Cancel();
			}
			catch (ObjectDisposedException)
			{
			}
			buildLifetimeCts.Dispose();
		}
		XWModBuildSingleFlight.FlightStateChanged -= OnModBuildFlightStateChanged;
		_scriptEditorSynchronizationContext = null;
		UnloadCallbackPreview(reportFailure: false);
		VisibilityChanged -= OnEditorVisibilityChanged;
		SetExternalConflictWatcherActive(active: false);
		SuspendIdeBackgroundWork();
		ShutdownDebugWorkbench();
		if (GodotObject.IsInstanceValid(_debugHost))
		{
			_debugHost.OwnSessionShutdown(_debugSession, _debugPreviewRoot);
		}
		_debugSession = null;
		_debugPreviewRoot = null;
	}

	private void UpdateDebugControls()
	{
		if (GodotObject.IsInstanceValid(_debugStartButton))
		{
			string activeModProjectRoot = GetActiveModProjectRoot();
			bool flag = !string.IsNullOrWhiteSpace(activeModProjectRoot);
			bool flag2 = flag && XWScriptCompiler.IsModBuildBusy(activeModProjectRoot);
			bool flag3 = _debugSession?.HasLiveSession ?? false;
			_debugStartButton.Disabled = !flag | flag3 | flag2;
			_debugStopButton.Disabled = !flag3;
			Label debugStatusLabel = _debugStatusLabel;
			object text;
			if (!flag2 || flag3)
			{
				XWScriptDebugSession debugSession = _debugSession;
				if (debugSession != null && debugSession.IsRunning)
				{
					text = (_debugSession.EntryPointInvoked ? "● 入口运行中" : "● 程序集已加载");
				}
				else
				{
					text = "● " + (_debugSession?.StatusText ?? "未启动");
				}
			}
			else
			{
				text = "◌ Mod 编译中";
			}
			debugStatusLabel.Text = (string)text;
			Label debugStatusLabel2 = _debugStatusLabel;
			XWScriptDebugSession debugSession2 = _debugSession;
			debugStatusLabel2.Modulate = ((debugSession2 != null && debugSession2.IsRunning) ? new Color(0.35f, 1f, 0.55f) : Colors.White);
			UpdateCompileControls();
			if (XWEditorInterface.Instance?.GetEditorPanel() is ModEditorPanel modEditorPanel)
			{
				modEditorPanel.GetModToolsPanel()?.UpdateScriptBuildActionState();
			}
		}
	}

	public void RefreshBuildActionState()
	{
		UpdateDebugControls();
	}

	private int BeginRenamePreviewRequest()
	{
		_renameGeneration++;
		_pendingRename = null;
		_pendingRenameStamp = null;
		_pendingRenameGeneration = 0;
		if (GodotObject.IsInstanceValid(_renameDialog))
		{
			_renameDialog.GetOkButton().Disabled = true;
		}
		return _renameGeneration;
	}

	private void InvalidateIdeRequests()
	{
		Interlocked.Increment(ref _ideContextRevision);
		LastSymbolInfo = null;
		_hoverGeneration++;
		_hoverTimer?.Stop();
		if (GodotObject.IsInstanceValid(_symbolInfoPopup))
		{
			_symbolInfoPopup.Hide();
		}
		BeginRenamePreviewRequest();
		_renamePreviewTimer?.Stop();
		if (GodotObject.IsInstanceValid(_referencesWindow))
		{
			_referencesWindow.Hide();
		}
		_referenceLocations.Clear();
		_referencesList?.Clear();
	}

	private bool TryCaptureIdeRequest(out IdeRequestStamp stamp, out IReadOnlyDictionary<string, string> documents, int? offsetOverride = null)
	{
		stamp = default;
		documents = null;
		if (!TryGetIdeContext(out var root, out var path, out var source, out var offset))
		{
			return false;
		}
		documents = CaptureOpenDocuments();
		stamp = new IdeRequestStamp(Volatile.Read(in _ideContextRevision), NormalizeIdePath(root), NormalizeIdePath(path), source, offsetOverride ?? offset);
		return true;
	}

	private bool IsIdeRequestCurrent(IdeRequestStamp stamp, bool requireCaretOffset)
	{
		if (!GodotObject.IsInstanceValid(this) || !_workspaceActive || stamp.ContextRevision != Volatile.Read(in _ideContextRevision) || !string.Equals(stamp.ProjectRoot, NormalizeIdePath(GetActiveModProjectRoot()), StringComparison.OrdinalIgnoreCase) || !string.Equals(stamp.FilePath, NormalizeIdePath(_currentTabKey), StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		string text = _codeEdit?.Text ?? "";
		if (!string.Equals(stamp.Source, text, StringComparison.Ordinal))
		{
			return false;
		}
		if (!requireCaretOffset || _codeEdit == null)
		{
			return true;
		}
		return XWCSharpIdeService.GetTextOffset(text, _codeEdit.GetCaretLine(), _codeEdit.GetCaretColumn()) == stamp.Offset;
	}

	private async Task WaitForIdeResultGateForProbeAsync(IdeRequestKind requestKind, string discriminator)
	{
		Func<IdeRequestKind, string, Task> ideResultGateForProbeAsync = IdeResultGateForProbeAsync;
		if (ideResultGateForProbeAsync != null)
		{
			await ideResultGateForProbeAsync(requestKind, discriminator ?? "");
		}
	}

	private static string NormalizeIdePath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || path.StartsWith("user://", StringComparison.OrdinalIgnoreCase))
		{
			path = ProjectSettings.GlobalizePath(path);
		}
		try
		{
			return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}
		catch
		{
			return path.TrimEnd('/', '\\');
		}
	}

	private string GetActiveModProjectRoot()
	{
		if (XWEditorInterface.Instance?.GetEditorPanel() is ModEditorPanel modEditorPanel)
		{
			string text = modEditorPanel.GetCurrentProject()?.ProjectPath ?? "";
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
		}
		return _activeProjectRoot;
	}

	private bool TryGetIdeContext(out string root, out string path, out string source, out int offset)
	{
		root = GetActiveModProjectRoot();
		path = _currentTabKey;
		source = _codeEdit?.Text ?? "";
		offset = ((_codeEdit != null) ? XWCSharpIdeService.GetTextOffset(source, _codeEdit.GetCaretLine(), _codeEdit.GetCaretColumn()) : 0);
		if (!string.IsNullOrWhiteSpace(root) && !string.IsNullOrWhiteSpace(path) && path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
		{
			return IsPathInsideRoot(path, root);
		}
		return false;
	}

	private IReadOnlyDictionary<string, string> CaptureOpenDocuments()
	{
		SaveCurrentTabState();
		string activeModProjectRoot = GetActiveModProjectRoot();
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		foreach (ScriptTabInfo value in _openTabs.Values)
		{
			if (value.FilePath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) && IsPathInsideRoot(value.FilePath, activeModProjectRoot))
			{
				dictionary[value.FilePath] = value.Text ?? "";
			}
		}
		return dictionary;
	}

	private List<string> GetDirtyAffectedDocuments(XWCSharpIdeService.RenamePreview preview)
	{
		List<string> list = new List<string>();
		if (preview == null)
		{
			return list;
		}
		SaveCurrentTabState();
		foreach (string key in preview.UpdatedDocuments.Keys)
		{
			if (_openTabs.TryGetValue(key, out var value) && value.HasUnsavedChanges)
			{
				list.Add(key);
			}
		}
		return list;
	}

	private void ReloadOpenDocumentsFromDisk()
	{
		string currentTabKey = _currentTabKey;
		foreach (ScriptTabInfo value in _openTabs.Values)
		{
			ReloadTabFromDisk(value);
		}
		if (!string.IsNullOrWhiteSpace(currentTabKey) && _openTabs.ContainsKey(currentTabKey))
		{
			LoadTabData(currentTabKey);
		}
		UpdateScriptList();
	}

	private static bool IsPathInsideRoot(string path, string root)
	{
		if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(root))
		{
			return false;
		}
		try
		{
			string value = Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
			return Path.GetFullPath(path).StartsWith(value, StringComparison.OrdinalIgnoreCase);
		}
		catch
		{
			return false;
		}
	}

	private static string SafeRelativePath(string root, string path)
	{
		try
		{
			return Path.GetRelativePath(root, path).Replace('\\', '/');
		}
		catch
		{
			return Path.GetFileName(path);
		}
	}

	private static string BuildSymbolInfoText(XWCSharpIdeService.SymbolInfoResult info)
	{
		string text = (string.IsNullOrWhiteSpace(info.Signature) ? info.Display : info.Signature);
		string text2 = (string.IsNullOrWhiteSpace(info.Documentation) ? "没有 XML 文档说明" : info.Documentation);
		return $"[color=#e9c46a][b]{EscapeBbcode(text)}[/b][/color]\n{EscapeBbcode(info.Kind)} · F12 定义 · Shift+F12 引用\n\n{EscapeBbcode(text2)}";
	}

	private static string EscapeBbcode(string text)
	{
		return (text ?? "").Replace("[", "［").Replace("]", "］");
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(137)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadIcons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildFileMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildSearchMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildGoToMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildToolbarOverflowMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnToolbarOverflowItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetWorkspaceActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyWorkspaceActivity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSearchMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnGoToMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildDiagnosticsContextMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDiagnosticsContextMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._UnhandledInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnFileMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NewCSharpScript, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnNewFileSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenFileDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenFile, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryOpenFile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenFileAt, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenGeneratedBlueprintFile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryOpenFileAt, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveCurrentTabState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadTabData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
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
			new MethodInfo(MethodName.CloseTab, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tabKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ContinueCloseQueue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnUnsavedCloseConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnUnsavedCloseCanceled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnUnsavedCloseCustomAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "action", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseTabImmediately, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tabKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseCurrentTab, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloseAllTabs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrySwitchProjectRoot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "projectRoot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenewBuildLifetime, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "cancelCurrentBuild", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeProjectRoot, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseOtherTabs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveFile, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveTab, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tabKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveFileAs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSaveAsFileSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveAllTabs, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "showToast", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCodeChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowSearchBar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "showReplace", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseSearchBar, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshSearchMatches, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "selectNearest", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NavigateSearch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceCurrentMatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReplaceAllMatches, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindNearestSearchMatch, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectActiveSearchMatch, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSearchMatchLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsWholeWordMatch, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "length", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsIdentifierCharacter, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "character", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PositiveModulo, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "divisor", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowGoToLineDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CommitGoToLine, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCodeErrorsChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "errorCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "warningCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCodeDiagnosticsChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCompileButtonPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UnloadCallbackPreview, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "reportFailure", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReportCallbackPreviewError, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDiagnosticListItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDiagnosticListItemActivated, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDiagnosticsListGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.CopySelectedDiagnostics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CopyAllDiagnostics, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpToDiagnosticItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "itemIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RequestCurrentValidation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.JumpToNextDiagnostic, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenBlueprintWorkspace, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveOpenFilePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LocalizeProjectPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCompileControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnModBuildFlightStateChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "projectRoot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "_", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenCurrentGeneratedBlueprintSource, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanEditCurrentDocument, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "showToast", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FirstNonEmpty, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.PackedStringArray, "values", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateLineCol, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateTabTitle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateScriptList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateScriptListSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnScriptFilterChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filterText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnScriptListItemSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "idx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasFileOpen, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCurrentFilePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsSupportedScriptPath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeNewCSharpPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildDefaultCSharpTemplate, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RejectUnsupportedScriptPath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "action", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "countRequest", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadScript, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeDebugWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShutdownDebugWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetDebugWorkbenchVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnDebugWorkbenchStopRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyScriptDebugNavigation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InitializeExternalConflictUI, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ComputeScriptContentHash, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveAbsoluteScriptPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveTabWithResult, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "tabKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "overwriteExternalConflict", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReloadCurrentExternalConflict, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OverwriteCurrentExternalConflict, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowCurrentExternalConflictDiff, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ScanExternalChangesNowForProbe, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveCurrentFileAsForProbe, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetExternalConflictWatcherActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScanExternalChangesFromWatcher, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveCurrentTabAsAtomic, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InitializeIdeFeatures, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SuspendIdeBackgroundWork, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResumeIdeBackgroundWork, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandleIdeShortcut, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "key", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEventKey"), exported: false)
			}, null),
			new MethodInfo(MethodName.JumpToReference, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnCodeEditIdeGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.RequestHoverFromTimer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestRenamePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunDebouncedRenamePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FormatDocumentOrSelection, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyQuickFixAtCaret, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowQuickFixMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyPendingQuickFix, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateDebugControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshBuildActionState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginRenamePreviewRequest, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.InvalidateIdeRequests, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizeIdePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetActiveModProjectRoot, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReloadOpenDocumentsFromDisk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsPathInsideRoot, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "root", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SafeRelativePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "root", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EscapeBbcode, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.LoadIcons && args.Count == 0)
		{
			LoadIcons();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildFileMenu && args.Count == 0)
		{
			BuildFileMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSearchMenu && args.Count == 0)
		{
			BuildSearchMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildGoToMenu && args.Count == 0)
		{
			BuildGoToMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildToolbarOverflowMenu && args.Count == 0)
		{
			BuildToolbarOverflowMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.OnToolbarOverflowItem && args.Count == 1)
		{
			OnToolbarOverflowItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnEditorVisibilityChanged && args.Count == 0)
		{
			OnEditorVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.SetWorkspaceActive && args.Count == 1)
		{
			SetWorkspaceActive(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyWorkspaceActivity && args.Count == 0)
		{
			ApplyWorkspaceActivity();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSearchMenuItem && args.Count == 1)
		{
			OnSearchMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnGoToMenuItem && args.Count == 1)
		{
			OnGoToMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildDiagnosticsContextMenu && args.Count == 0)
		{
			BuildDiagnosticsContextMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDiagnosticsContextMenuItem && args.Count == 1)
		{
			OnDiagnosticsContextMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectSignals && args.Count == 0)
		{
			ConnectSignals();
			ret = default;
			return true;
		}
		if (method == MethodName._UnhandledInput && args.Count == 1)
		{
			_UnhandledInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnFileMenuItem && args.Count == 1)
		{
			OnFileMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NewCSharpScript && args.Count == 0)
		{
			NewCSharpScript();
			ret = default;
			return true;
		}
		if (method == MethodName.OnNewFileSelected && args.Count == 1)
		{
			OnNewFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenFileDialog && args.Count == 0)
		{
			OpenFileDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenFile && args.Count == 1)
		{
			OpenFile(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryOpenFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryOpenFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OpenFileAt && args.Count == 3)
		{
			OpenFileAt(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenGeneratedBlueprintFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(OpenGeneratedBlueprintFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.TryOpenFileAt && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(TryOpenFileAt(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
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
		if (method == MethodName.CloseTab && args.Count == 1)
		{
			CloseTab(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ContinueCloseQueue && args.Count == 0)
		{
			ContinueCloseQueue();
			ret = default;
			return true;
		}
		if (method == MethodName.OnUnsavedCloseConfirmed && args.Count == 0)
		{
			OnUnsavedCloseConfirmed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnUnsavedCloseCanceled && args.Count == 0)
		{
			OnUnsavedCloseCanceled();
			ret = default;
			return true;
		}
		if (method == MethodName.OnUnsavedCloseCustomAction && args.Count == 1)
		{
			OnUnsavedCloseCustomAction(VariantUtils.ConvertTo<StringName>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloseTabImmediately && args.Count == 1)
		{
			CloseTabImmediately(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloseCurrentTab && args.Count == 0)
		{
			CloseCurrentTab();
			ret = default;
			return true;
		}
		if (method == MethodName.CloseAllTabs && args.Count == 0)
		{
			CloseAllTabs();
			ret = default;
			return true;
		}
		if (method == MethodName.TrySwitchProjectRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TrySwitchProjectRoot(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RenewBuildLifetime && args.Count == 1)
		{
			RenewBuildLifetime(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeProjectRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeProjectRoot(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CloseOtherTabs && args.Count == 0)
		{
			CloseOtherTabs();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearEditor && args.Count == 0)
		{
			ClearEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFile && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(SaveFile());
			return true;
		}
		if (method == MethodName.SaveTab && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SaveTab(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SaveFileAs && args.Count == 0)
		{
			SaveFileAs();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSaveAsFileSelected && args.Count == 1)
		{
			OnSaveAsFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveAllTabs && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(SaveAllTabs(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.OnCodeChanged && args.Count == 0)
		{
			OnCodeChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowSearchBar && args.Count == 1)
		{
			ShowSearchBar(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloseSearchBar && args.Count == 0)
		{
			CloseSearchBar();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSearchMatches && args.Count == 1)
		{
			RefreshSearchMatches(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateSearch && args.Count == 1)
		{
			NavigateSearch(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceCurrentMatch && args.Count == 0)
		{
			ReplaceCurrentMatch();
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceAllMatches && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ReplaceAllMatches());
			return true;
		}
		if (method == MethodName.FindNearestSearchMatch && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(FindNearestSearchMatch());
			return true;
		}
		if (method == MethodName.SelectActiveSearchMatch && args.Count == 0)
		{
			SelectActiveSearchMatch();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSearchMatchLabel && args.Count == 0)
		{
			UpdateSearchMatchLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.IsWholeWordMatch && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsWholeWordMatch(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.IsIdentifierCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIdentifierCharacter(VariantUtils.ConvertTo<char>(in args[0])));
			return true;
		}
		if (method == MethodName.PositiveModulo && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(PositiveModulo(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ShowGoToLineDialog && args.Count == 0)
		{
			ShowGoToLineDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.CommitGoToLine && args.Count == 0)
		{
			CommitGoToLine();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCodeErrorsChanged && args.Count == 2)
		{
			OnCodeErrorsChanged(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCodeDiagnosticsChanged && args.Count == 0)
		{
			OnCodeDiagnosticsChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCompileButtonPressed && args.Count == 0)
		{
			OnCompileButtonPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.UnloadCallbackPreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(UnloadCallbackPreview(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.ReportCallbackPreviewError && args.Count == 1)
		{
			ReportCallbackPreviewError(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDiagnosticListItemSelected && args.Count == 1)
		{
			OnDiagnosticListItemSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDiagnosticListItemActivated && args.Count == 1)
		{
			OnDiagnosticListItemActivated(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDiagnosticsListGuiInput && args.Count == 1)
		{
			OnDiagnosticsListGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CopySelectedDiagnostics && args.Count == 0)
		{
			CopySelectedDiagnostics();
			ret = default;
			return true;
		}
		if (method == MethodName.CopyAllDiagnostics && args.Count == 0)
		{
			CopyAllDiagnostics();
			ret = default;
			return true;
		}
		if (method == MethodName.JumpToDiagnosticItem && args.Count == 1)
		{
			JumpToDiagnosticItem(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RequestCurrentValidation && args.Count == 0)
		{
			RequestCurrentValidation();
			ret = default;
			return true;
		}
		if (method == MethodName.JumpToNextDiagnostic && args.Count == 1)
		{
			JumpToNextDiagnostic(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenBlueprintWorkspace && args.Count == 0)
		{
			OpenBlueprintWorkspace();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveOpenFilePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveOpenFilePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LocalizeProjectPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(LocalizeProjectPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateCompileControls && args.Count == 0)
		{
			UpdateCompileControls();
			ret = default;
			return true;
		}
		if (method == MethodName.OnModBuildFlightStateChanged && args.Count == 2)
		{
			OnModBuildFlightStateChanged(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenCurrentGeneratedBlueprintSource && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(OpenCurrentGeneratedBlueprintSource());
			return true;
		}
		if (method == MethodName.CanEditCurrentDocument && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanEditCurrentDocument(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.FirstNonEmpty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FirstNonEmpty(VariantUtils.ConvertTo<string[]>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateLineCol && args.Count == 0)
		{
			UpdateLineCol();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateTabTitle && args.Count == 0)
		{
			UpdateTabTitle();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateScriptList && args.Count == 0)
		{
			UpdateScriptList();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateScriptListSelection && args.Count == 0)
		{
			UpdateScriptListSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.OnScriptFilterChanged && args.Count == 1)
		{
			OnScriptFilterChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnScriptListItemSelected && args.Count == 1)
		{
			OnScriptListItemSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasFileOpen && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasFileOpen());
			return true;
		}
		if (method == MethodName.GetCurrentFilePath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrentFilePath());
			return true;
		}
		if (method == MethodName.IsSupportedScriptPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSupportedScriptPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeNewCSharpPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeNewCSharpPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildDefaultCSharpTemplate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildDefaultCSharpTemplate(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RejectUnsupportedScriptPath && args.Count == 3)
		{
			RejectUnsupportedScriptPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadScript && args.Count == 1)
		{
			LoadScript(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeDebugWorkbench && args.Count == 0)
		{
			InitializeDebugWorkbench();
			ret = default;
			return true;
		}
		if (method == MethodName.ShutdownDebugWorkbench && args.Count == 0)
		{
			ShutdownDebugWorkbench();
			ret = default;
			return true;
		}
		if (method == MethodName.SetDebugWorkbenchVisible && args.Count == 1)
		{
			SetDebugWorkbenchVisible(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDebugWorkbenchStopRequested && args.Count == 0)
		{
			OnDebugWorkbenchStopRequested();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyScriptDebugNavigation && args.Count == 0)
		{
			ApplyScriptDebugNavigation();
			ret = default;
			return true;
		}
		if (method == MethodName.InitializeExternalConflictUI && args.Count == 0)
		{
			InitializeExternalConflictUI();
			ret = default;
			return true;
		}
		if (method == MethodName.ComputeScriptContentHash && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ComputeScriptContentHash(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveAbsoluteScriptPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveAbsoluteScriptPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SaveTabWithResult && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<ScriptSaveStatus>(SaveTabWithResult(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.ReloadCurrentExternalConflict && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ReloadCurrentExternalConflict());
			return true;
		}
		if (method == MethodName.OverwriteCurrentExternalConflict && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(OverwriteCurrentExternalConflict());
			return true;
		}
		if (method == MethodName.ShowCurrentExternalConflictDiff && args.Count == 0)
		{
			ShowCurrentExternalConflictDiff();
			ret = default;
			return true;
		}
		if (method == MethodName.ScanExternalChangesNowForProbe && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(ScanExternalChangesNowForProbe());
			return true;
		}
		if (method == MethodName.SaveCurrentFileAsForProbe && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ScriptSaveStatus>(SaveCurrentFileAsForProbe(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.SetExternalConflictWatcherActive && args.Count == 1)
		{
			SetExternalConflictWatcherActive(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ScanExternalChangesFromWatcher && args.Count == 0)
		{
			ScanExternalChangesFromWatcher();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveCurrentTabAsAtomic && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ScriptSaveStatus>(SaveCurrentTabAsAtomic(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.InitializeIdeFeatures && args.Count == 0)
		{
			InitializeIdeFeatures();
			ret = default;
			return true;
		}
		if (method == MethodName.SuspendIdeBackgroundWork && args.Count == 0)
		{
			SuspendIdeBackgroundWork();
			ret = default;
			return true;
		}
		if (method == MethodName.ResumeIdeBackgroundWork && args.Count == 0)
		{
			ResumeIdeBackgroundWork();
			ret = default;
			return true;
		}
		if (method == MethodName.HandleIdeShortcut && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HandleIdeShortcut(VariantUtils.ConvertTo<InputEventKey>(in args[0])));
			return true;
		}
		if (method == MethodName.JumpToReference && args.Count == 1)
		{
			JumpToReference(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCodeEditIdeGuiInput && args.Count == 1)
		{
			OnCodeEditIdeGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RequestHoverFromTimer && args.Count == 0)
		{
			RequestHoverFromTimer();
			ret = default;
			return true;
		}
		if (method == MethodName.RequestRenamePreview && args.Count == 0)
		{
			RequestRenamePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RunDebouncedRenamePreview && args.Count == 0)
		{
			RunDebouncedRenamePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.FormatDocumentOrSelection && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(FormatDocumentOrSelection());
			return true;
		}
		if (method == MethodName.ApplyQuickFixAtCaret && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ApplyQuickFixAtCaret());
			return true;
		}
		if (method == MethodName.ShowQuickFixMenu && args.Count == 0)
		{
			ShowQuickFixMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPendingQuickFix && args.Count == 0)
		{
			ApplyPendingQuickFix();
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDebugControls && args.Count == 0)
		{
			UpdateDebugControls();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshBuildActionState && args.Count == 0)
		{
			RefreshBuildActionState();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginRenamePreviewRequest && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(BeginRenamePreviewRequest());
			return true;
		}
		if (method == MethodName.InvalidateIdeRequests && args.Count == 0)
		{
			InvalidateIdeRequests();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeIdePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeIdePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetActiveModProjectRoot && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetActiveModProjectRoot());
			return true;
		}
		if (method == MethodName.ReloadOpenDocumentsFromDisk && args.Count == 0)
		{
			ReloadOpenDocumentsFromDisk();
			ret = default;
			return true;
		}
		if (method == MethodName.IsPathInsideRoot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPathInsideRoot(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SafeRelativePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(SafeRelativePath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.EscapeBbcode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EscapeBbcode(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.NormalizeProjectRoot && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeProjectRoot(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsWholeWordMatch && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<bool>(IsWholeWordMatch(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.IsIdentifierCharacter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsIdentifierCharacter(VariantUtils.ConvertTo<char>(in args[0])));
			return true;
		}
		if (method == MethodName.PositiveModulo && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(PositiveModulo(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.ReportCallbackPreviewError && args.Count == 1)
		{
			ReportCallbackPreviewError(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LocalizeProjectPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(LocalizeProjectPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FirstNonEmpty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FirstNonEmpty(VariantUtils.ConvertTo<string[]>(in args[0])));
			return true;
		}
		if (method == MethodName.IsSupportedScriptPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSupportedScriptPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeNewCSharpPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeNewCSharpPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildDefaultCSharpTemplate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildDefaultCSharpTemplate(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ComputeScriptContentHash && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ComputeScriptContentHash(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveAbsoluteScriptPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveAbsoluteScriptPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeIdePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeIdePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsPathInsideRoot && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPathInsideRoot(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SafeRelativePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(SafeRelativePath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.EscapeBbcode && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EscapeBbcode(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.LoadIcons)
		{
			return true;
		}
		if (method == MethodName.BuildFileMenu)
		{
			return true;
		}
		if (method == MethodName.BuildSearchMenu)
		{
			return true;
		}
		if (method == MethodName.BuildGoToMenu)
		{
			return true;
		}
		if (method == MethodName.BuildToolbarOverflowMenu)
		{
			return true;
		}
		if (method == MethodName.OnToolbarOverflowItem)
		{
			return true;
		}
		if (method == MethodName.OnEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.SetWorkspaceActive)
		{
			return true;
		}
		if (method == MethodName.ApplyWorkspaceActivity)
		{
			return true;
		}
		if (method == MethodName.OnSearchMenuItem)
		{
			return true;
		}
		if (method == MethodName.OnGoToMenuItem)
		{
			return true;
		}
		if (method == MethodName.BuildDiagnosticsContextMenu)
		{
			return true;
		}
		if (method == MethodName.OnDiagnosticsContextMenuItem)
		{
			return true;
		}
		if (method == MethodName.ConnectSignals)
		{
			return true;
		}
		if (method == MethodName._UnhandledInput)
		{
			return true;
		}
		if (method == MethodName.OnFileMenuItem)
		{
			return true;
		}
		if (method == MethodName.NewCSharpScript)
		{
			return true;
		}
		if (method == MethodName.OnNewFileSelected)
		{
			return true;
		}
		if (method == MethodName.OpenFileDialog)
		{
			return true;
		}
		if (method == MethodName.OpenFile)
		{
			return true;
		}
		if (method == MethodName.TryOpenFile)
		{
			return true;
		}
		if (method == MethodName.OpenFileAt)
		{
			return true;
		}
		if (method == MethodName.OpenGeneratedBlueprintFile)
		{
			return true;
		}
		if (method == MethodName.TryOpenFileAt)
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
		if (method == MethodName.OnTabChanged)
		{
			return true;
		}
		if (method == MethodName.OnTabClosePressed)
		{
			return true;
		}
		if (method == MethodName.CloseTab)
		{
			return true;
		}
		if (method == MethodName.ContinueCloseQueue)
		{
			return true;
		}
		if (method == MethodName.OnUnsavedCloseConfirmed)
		{
			return true;
		}
		if (method == MethodName.OnUnsavedCloseCanceled)
		{
			return true;
		}
		if (method == MethodName.OnUnsavedCloseCustomAction)
		{
			return true;
		}
		if (method == MethodName.CloseTabImmediately)
		{
			return true;
		}
		if (method == MethodName.CloseCurrentTab)
		{
			return true;
		}
		if (method == MethodName.CloseAllTabs)
		{
			return true;
		}
		if (method == MethodName.TrySwitchProjectRoot)
		{
			return true;
		}
		if (method == MethodName.RenewBuildLifetime)
		{
			return true;
		}
		if (method == MethodName.NormalizeProjectRoot)
		{
			return true;
		}
		if (method == MethodName.CloseOtherTabs)
		{
			return true;
		}
		if (method == MethodName.ClearEditor)
		{
			return true;
		}
		if (method == MethodName.SaveFile)
		{
			return true;
		}
		if (method == MethodName.SaveTab)
		{
			return true;
		}
		if (method == MethodName.SaveFileAs)
		{
			return true;
		}
		if (method == MethodName.OnSaveAsFileSelected)
		{
			return true;
		}
		if (method == MethodName.SaveAllTabs)
		{
			return true;
		}
		if (method == MethodName.OnCodeChanged)
		{
			return true;
		}
		if (method == MethodName.ShowSearchBar)
		{
			return true;
		}
		if (method == MethodName.CloseSearchBar)
		{
			return true;
		}
		if (method == MethodName.RefreshSearchMatches)
		{
			return true;
		}
		if (method == MethodName.NavigateSearch)
		{
			return true;
		}
		if (method == MethodName.ReplaceCurrentMatch)
		{
			return true;
		}
		if (method == MethodName.ReplaceAllMatches)
		{
			return true;
		}
		if (method == MethodName.FindNearestSearchMatch)
		{
			return true;
		}
		if (method == MethodName.SelectActiveSearchMatch)
		{
			return true;
		}
		if (method == MethodName.UpdateSearchMatchLabel)
		{
			return true;
		}
		if (method == MethodName.IsWholeWordMatch)
		{
			return true;
		}
		if (method == MethodName.IsIdentifierCharacter)
		{
			return true;
		}
		if (method == MethodName.PositiveModulo)
		{
			return true;
		}
		if (method == MethodName.ShowGoToLineDialog)
		{
			return true;
		}
		if (method == MethodName.CommitGoToLine)
		{
			return true;
		}
		if (method == MethodName.OnCodeErrorsChanged)
		{
			return true;
		}
		if (method == MethodName.OnCodeDiagnosticsChanged)
		{
			return true;
		}
		if (method == MethodName.OnCompileButtonPressed)
		{
			return true;
		}
		if (method == MethodName.UnloadCallbackPreview)
		{
			return true;
		}
		if (method == MethodName.ReportCallbackPreviewError)
		{
			return true;
		}
		if (method == MethodName.OnDiagnosticListItemSelected)
		{
			return true;
		}
		if (method == MethodName.OnDiagnosticListItemActivated)
		{
			return true;
		}
		if (method == MethodName.OnDiagnosticsListGuiInput)
		{
			return true;
		}
		if (method == MethodName.CopySelectedDiagnostics)
		{
			return true;
		}
		if (method == MethodName.CopyAllDiagnostics)
		{
			return true;
		}
		if (method == MethodName.JumpToDiagnosticItem)
		{
			return true;
		}
		if (method == MethodName.RequestCurrentValidation)
		{
			return true;
		}
		if (method == MethodName.JumpToNextDiagnostic)
		{
			return true;
		}
		if (method == MethodName.OpenBlueprintWorkspace)
		{
			return true;
		}
		if (method == MethodName.ResolveOpenFilePath)
		{
			return true;
		}
		if (method == MethodName.LocalizeProjectPath)
		{
			return true;
		}
		if (method == MethodName.UpdateCompileControls)
		{
			return true;
		}
		if (method == MethodName.OnModBuildFlightStateChanged)
		{
			return true;
		}
		if (method == MethodName.OpenCurrentGeneratedBlueprintSource)
		{
			return true;
		}
		if (method == MethodName.CanEditCurrentDocument)
		{
			return true;
		}
		if (method == MethodName.FirstNonEmpty)
		{
			return true;
		}
		if (method == MethodName.UpdateLineCol)
		{
			return true;
		}
		if (method == MethodName.UpdateTabTitle)
		{
			return true;
		}
		if (method == MethodName.UpdateScriptList)
		{
			return true;
		}
		if (method == MethodName.UpdateScriptListSelection)
		{
			return true;
		}
		if (method == MethodName.OnScriptFilterChanged)
		{
			return true;
		}
		if (method == MethodName.OnScriptListItemSelected)
		{
			return true;
		}
		if (method == MethodName.HasFileOpen)
		{
			return true;
		}
		if (method == MethodName.GetCurrentFilePath)
		{
			return true;
		}
		if (method == MethodName.IsSupportedScriptPath)
		{
			return true;
		}
		if (method == MethodName.NormalizeNewCSharpPath)
		{
			return true;
		}
		if (method == MethodName.BuildDefaultCSharpTemplate)
		{
			return true;
		}
		if (method == MethodName.RejectUnsupportedScriptPath)
		{
			return true;
		}
		if (method == MethodName.LoadScript)
		{
			return true;
		}
		if (method == MethodName.InitializeDebugWorkbench)
		{
			return true;
		}
		if (method == MethodName.ShutdownDebugWorkbench)
		{
			return true;
		}
		if (method == MethodName.SetDebugWorkbenchVisible)
		{
			return true;
		}
		if (method == MethodName.OnDebugWorkbenchStopRequested)
		{
			return true;
		}
		if (method == MethodName.ApplyScriptDebugNavigation)
		{
			return true;
		}
		if (method == MethodName.InitializeExternalConflictUI)
		{
			return true;
		}
		if (method == MethodName.ComputeScriptContentHash)
		{
			return true;
		}
		if (method == MethodName.ResolveAbsoluteScriptPath)
		{
			return true;
		}
		if (method == MethodName.SaveTabWithResult)
		{
			return true;
		}
		if (method == MethodName.ReloadCurrentExternalConflict)
		{
			return true;
		}
		if (method == MethodName.OverwriteCurrentExternalConflict)
		{
			return true;
		}
		if (method == MethodName.ShowCurrentExternalConflictDiff)
		{
			return true;
		}
		if (method == MethodName.ScanExternalChangesNowForProbe)
		{
			return true;
		}
		if (method == MethodName.SaveCurrentFileAsForProbe)
		{
			return true;
		}
		if (method == MethodName.SetExternalConflictWatcherActive)
		{
			return true;
		}
		if (method == MethodName.ScanExternalChangesFromWatcher)
		{
			return true;
		}
		if (method == MethodName.SaveCurrentTabAsAtomic)
		{
			return true;
		}
		if (method == MethodName.InitializeIdeFeatures)
		{
			return true;
		}
		if (method == MethodName.SuspendIdeBackgroundWork)
		{
			return true;
		}
		if (method == MethodName.ResumeIdeBackgroundWork)
		{
			return true;
		}
		if (method == MethodName.HandleIdeShortcut)
		{
			return true;
		}
		if (method == MethodName.JumpToReference)
		{
			return true;
		}
		if (method == MethodName.OnCodeEditIdeGuiInput)
		{
			return true;
		}
		if (method == MethodName.RequestHoverFromTimer)
		{
			return true;
		}
		if (method == MethodName.RequestRenamePreview)
		{
			return true;
		}
		if (method == MethodName.RunDebouncedRenamePreview)
		{
			return true;
		}
		if (method == MethodName.FormatDocumentOrSelection)
		{
			return true;
		}
		if (method == MethodName.ApplyQuickFixAtCaret)
		{
			return true;
		}
		if (method == MethodName.ShowQuickFixMenu)
		{
			return true;
		}
		if (method == MethodName.ApplyPendingQuickFix)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.UpdateDebugControls)
		{
			return true;
		}
		if (method == MethodName.RefreshBuildActionState)
		{
			return true;
		}
		if (method == MethodName.BeginRenamePreviewRequest)
		{
			return true;
		}
		if (method == MethodName.InvalidateIdeRequests)
		{
			return true;
		}
		if (method == MethodName.NormalizeIdePath)
		{
			return true;
		}
		if (method == MethodName.GetActiveModProjectRoot)
		{
			return true;
		}
		if (method == MethodName.ReloadOpenDocumentsFromDisk)
		{
			return true;
		}
		if (method == MethodName.IsPathInsideRoot)
		{
			return true;
		}
		if (method == MethodName.SafeRelativePath)
		{
			return true;
		}
		if (method == MethodName.EscapeBbcode)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.UnsupportedScriptRequestCount)
		{
			UnsupportedScriptRequestCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.UnsupportedSaveAsRequestCount)
		{
			UnsupportedSaveAsRequestCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.CSharpTemplateCreateCount)
		{
			CSharpTemplateCreateCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.LastSaveStatus)
		{
			LastSaveStatus = VariantUtils.ConvertTo<ScriptSaveStatus>(in value);
			return true;
		}
		if (name == PropertyName.LastSaveError)
		{
			LastSaveError = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName.ExternalConflictDetectedCount)
		{
			ExternalConflictDetectedCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ExternalConflictReloadCount)
		{
			ExternalConflictReloadCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ExternalConflictOverwriteCount)
		{
			ExternalConflictOverwriteCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.CleanExternalReloadCount)
		{
			CleanExternalReloadCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.AtomicSaveCount)
		{
			AtomicSaveCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.SaveAsRollbackCount)
		{
			SaveAsRollbackCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.CompileSaveGateCount)
		{
			CompileSaveGateCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.DebugSaveGateCount)
		{
			DebugSaveGateCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.CompileInvocationCount)
		{
			CompileInvocationCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.DebugStartInvocationCount)
		{
			DebugStartInvocationCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName.ExternalConflictWatcherScanCount)
		{
			ExternalConflictWatcherScanCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._fileMenuBtn)
		{
			_fileMenuBtn = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._searchMenuBtn)
		{
			_searchMenuBtn = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._goToMenuBtn)
		{
			_goToMenuBtn = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._overflowMenuBtn)
		{
			_overflowMenuBtn = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._scriptNameLabel)
		{
			_scriptNameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._scriptFilter)
		{
			_scriptFilter = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._scriptList)
		{
			_scriptList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._scriptTabBar)
		{
			_scriptTabBar = VariantUtils.ConvertTo<TabBar>(in value);
			return true;
		}
		if (name == PropertyName._codeEdit)
		{
			_codeEdit = VariantUtils.ConvertTo<XWCodeEdit>(in value);
			return true;
		}
		if (name == PropertyName._lineColLabel)
		{
			_lineColLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._compileButton)
		{
			_compileButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._validateButton)
		{
			_validateButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._previousProblemButton)
		{
			_previousProblemButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._nextProblemButton)
		{
			_nextProblemButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._openBlueprintButton)
		{
			_openBlueprintButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._generatedBlueprintBar)
		{
			_generatedBlueprintBar = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._generatedBlueprintStatusLabel)
		{
			_generatedBlueprintStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._generatedBlueprintSourceLabel)
		{
			_generatedBlueprintSourceLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._returnToBlueprintButton)
		{
			_returnToBlueprintButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._diagnosticsLabel)
		{
			_diagnosticsLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._diagnosticsPanel)
		{
			_diagnosticsPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._diagnosticsList)
		{
			_diagnosticsList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._diagnosticsCopyButton)
		{
			_diagnosticsCopyButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._diagnosticsContextMenu)
		{
			_diagnosticsContextMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._searchBar)
		{
			_searchBar = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._searchInput)
		{
			_searchInput = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._searchMatchLabel)
		{
			_searchMatchLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._searchPreviousButton)
		{
			_searchPreviousButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._searchNextButton)
		{
			_searchNextButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._searchCaseButton)
		{
			_searchCaseButton = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._searchWholeButton)
		{
			_searchWholeButton = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._searchCloseButton)
		{
			_searchCloseButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._replaceRow)
		{
			_replaceRow = VariantUtils.ConvertTo<Container>(in value);
			return true;
		}
		if (name == PropertyName._replaceInput)
		{
			_replaceInput = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._replaceNextButton)
		{
			_replaceNextButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._replaceAllButton)
		{
			_replaceAllButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._goToLineDialog)
		{
			_goToLineDialog = VariantUtils.ConvertTo<ConfirmationDialog>(in value);
			return true;
		}
		if (name == PropertyName._goToLineSpin)
		{
			_goToLineSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._newFileDialog)
		{
			_newFileDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._openFileDialog)
		{
			_openFileDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._saveAsDialog)
		{
			_saveAsDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._unsavedChangesDialog)
		{
			_unsavedChangesDialog = VariantUtils.ConvertTo<ConfirmationDialog>(in value);
			return true;
		}
		if (name == PropertyName._discardUnsavedButton)
		{
			_discardUnsavedButton = VariantUtils.ConvertTo<Button>(in value);
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
		if (name == PropertyName._isLoadingTab)
		{
			_isLoadingTab = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isCompiling)
		{
			_isCompiling = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._isBulkReplacing)
		{
			_isBulkReplacing = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._activeSearchMatch)
		{
			_activeSearchMatch = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._pendingCloseTabKey)
		{
			_pendingCloseTabKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._hiddenWorkSuspended)
		{
			_hiddenWorkSuspended = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._workspaceSelected)
		{
			_workspaceSelected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._workspaceActive)
		{
			_workspaceActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._activeProjectRoot)
		{
			_activeProjectRoot = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._iconScript)
		{
			_iconScript = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._iconSave)
		{
			_iconSave = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._iconNew)
		{
			_iconNew = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._iconFolder)
		{
			_iconFolder = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._iconClose)
		{
			_iconClose = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._iconBuildCSharp)
		{
			_iconBuildCSharp = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._iconStatusError)
		{
			_iconStatusError = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._iconStatusWarning)
		{
			_iconStatusWarning = VariantUtils.ConvertTo<Texture2D>(in value);
			return true;
		}
		if (name == PropertyName._debugWorkbench)
		{
			_debugWorkbench = VariantUtils.ConvertTo<XWModDebugWorkbench>(in value);
			return true;
		}
		if (name == PropertyName._debugWorkbenchToggle)
		{
			_debugWorkbenchToggle = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._scriptDebugNavigationQueued)
		{
			_scriptDebugNavigationQueued = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._externalConflictBar)
		{
			_externalConflictBar = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._externalConflictTitleLabel)
		{
			_externalConflictTitleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._externalConflictDetailLabel)
		{
			_externalConflictDetailLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._viewExternalDiffButton)
		{
			_viewExternalDiffButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._reloadExternalButton)
		{
			_reloadExternalButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._overwriteExternalButton)
		{
			_overwriteExternalButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._saveConflictCopyButton)
		{
			_saveConflictCopyButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._externalConflictDiffWindow)
		{
			_externalConflictDiffWindow = VariantUtils.ConvertTo<Window>(in value);
			return true;
		}
		if (name == PropertyName._externalConflictEditorText)
		{
			_externalConflictEditorText = VariantUtils.ConvertTo<TextEdit>(in value);
			return true;
		}
		if (name == PropertyName._externalConflictDiskText)
		{
			_externalConflictDiskText = VariantUtils.ConvertTo<TextEdit>(in value);
			return true;
		}
		if (name == PropertyName._externalConflictWatcher)
		{
			_externalConflictWatcher = VariantUtils.ConvertTo<Godot.Timer>(in value);
			return true;
		}
		if (name == PropertyName._externalConflictWatcherScanning)
		{
			_externalConflictWatcherScanning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._definitionButton)
		{
			_definitionButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._referencesButton)
		{
			_referencesButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._renameButton)
		{
			_renameButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._renameUndoButton)
		{
			_renameUndoButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._renameRedoButton)
		{
			_renameRedoButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._formatButton)
		{
			_formatButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._quickFixButton)
		{
			_quickFixButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._debugStartButton)
		{
			_debugStartButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._debugStopButton)
		{
			_debugStopButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._debugStatusLabel)
		{
			_debugStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._referencesWindow)
		{
			_referencesWindow = VariantUtils.ConvertTo<Window>(in value);
			return true;
		}
		if (name == PropertyName._referencesHeader)
		{
			_referencesHeader = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._referencesList)
		{
			_referencesList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._renameDialog)
		{
			_renameDialog = VariantUtils.ConvertTo<ConfirmationDialog>(in value);
			return true;
		}
		if (name == PropertyName._renameInput)
		{
			_renameInput = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._renamePreviewLabel)
		{
			_renamePreviewLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName._symbolInfoPopup)
		{
			_symbolInfoPopup = VariantUtils.ConvertTo<PopupPanel>(in value);
			return true;
		}
		if (name == PropertyName._symbolInfoLabel)
		{
			_symbolInfoLabel = VariantUtils.ConvertTo<RichTextLabel>(in value);
			return true;
		}
		if (name == PropertyName._quickFixPopup)
		{
			_quickFixPopup = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._hoverTimer)
		{
			_hoverTimer = VariantUtils.ConvertTo<Godot.Timer>(in value);
			return true;
		}
		if (name == PropertyName._renamePreviewTimer)
		{
			_renamePreviewTimer = VariantUtils.ConvertTo<Godot.Timer>(in value);
			return true;
		}
		if (name == PropertyName._debugHost)
		{
			_debugHost = VariantUtils.ConvertTo<XWScriptDebugHost>(in value);
			return true;
		}
		if (name == PropertyName._debugPreviewRoot)
		{
			_debugPreviewRoot = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._pendingRenameGeneration)
		{
			_pendingRenameGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._hoverLocalPosition)
		{
			_hoverLocalPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._hoverLine)
		{
			_hoverLine = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._hoverColumn)
		{
			_hoverColumn = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._hoverGeneration)
		{
			_hoverGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._renameGeneration)
		{
			_renameGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._pendingRenameText)
		{
			_pendingRenameText = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._ideContextRevision)
		{
			_ideContextRevision = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		string from;
		if (name == PropertyName.CallbackPreviewOwnerId)
		{
			from = CallbackPreviewOwnerId;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.UnsupportedScriptRequestCount)
		{
			from2 = UnsupportedScriptRequestCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.UnsupportedSaveAsRequestCount)
		{
			from2 = UnsupportedSaveAsRequestCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CSharpTemplateCreateCount)
		{
			from2 = CSharpTemplateCreateCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		bool from3;
		if (name == PropertyName.IsHiddenWorkQuiescent)
		{
			from3 = IsHiddenWorkQuiescent;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.IsCurrentDocumentReadOnly)
		{
			from3 = IsCurrentDocumentReadOnly;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CurrentGeneratedBlueprintSourcePath)
		{
			from = CurrentGeneratedBlueprintSourcePath;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CurrentGeneratedLinkState)
		{
			value = VariantUtils.CreateFrom<XWBlueprintGeneratedCSharpPolicy.LinkState>(CurrentGeneratedLinkState);
			return true;
		}
		if (name == PropertyName.HiddenWorkDiagnostic)
		{
			from = HiddenWorkDiagnostic;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsDebugWorkbenchBound)
		{
			from3 = IsDebugWorkbenchBound;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.IsDebugWorkbenchVisible)
		{
			from3 = IsDebugWorkbenchVisible;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.IsDebugWorkbenchHiddenProcessSuspended)
		{
			from3 = IsDebugWorkbenchHiddenProcessSuspended;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.DebugWorkbenchCallStackCount)
		{
			from2 = DebugWorkbenchCallStackCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.DebugWorkbenchVariableCount)
		{
			from2 = DebugWorkbenchVariableCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.LastSaveStatus)
		{
			value = VariantUtils.CreateFrom<ScriptSaveStatus>(LastSaveStatus);
			return true;
		}
		if (name == PropertyName.LastSaveError)
		{
			from = LastSaveError;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ExternalConflictDetectedCount)
		{
			from2 = ExternalConflictDetectedCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ExternalConflictReloadCount)
		{
			from2 = ExternalConflictReloadCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ExternalConflictOverwriteCount)
		{
			from2 = ExternalConflictOverwriteCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CleanExternalReloadCount)
		{
			from2 = CleanExternalReloadCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.AtomicSaveCount)
		{
			from2 = AtomicSaveCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.SaveAsRollbackCount)
		{
			from2 = SaveAsRollbackCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CompileSaveGateCount)
		{
			from2 = CompileSaveGateCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.DebugSaveGateCount)
		{
			from2 = DebugSaveGateCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.CompileInvocationCount)
		{
			from2 = CompileInvocationCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.DebugStartInvocationCount)
		{
			from2 = DebugStartInvocationCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ExternalConflictWatcherScanCount)
		{
			from2 = ExternalConflictWatcherScanCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsCurrentDocumentInExternalConflict)
		{
			from3 = IsCurrentDocumentInExternalConflict;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.IsExternalConflictBarVisible)
		{
			from3 = IsExternalConflictBarVisible;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.IsExternalConflictWatcherIdle)
		{
			from3 = IsExternalConflictWatcherIdle;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.CurrentDocumentText)
		{
			from = CurrentDocumentText;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsDebugSessionRunning)
		{
			from3 = IsDebugSessionRunning;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.HasLiveDebugSession)
		{
			from3 = HasLiveDebugSession;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.DebugEntryInvoked)
		{
			from3 = DebugEntryInvoked;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.DebugLoadedAssemblyCount)
		{
			from2 = DebugLoadedAssemblyCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.DebugEntryTaskCompleted)
		{
			from3 = DebugEntryTaskCompleted;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.DebugLoadContextAlive)
		{
			from3 = DebugLoadContextAlive;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.DebugPreviewChildCount)
		{
			from2 = DebugPreviewChildCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.DebugMainThreadId)
		{
			from2 = DebugMainThreadId;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.HasPendingIdeBackgroundWork)
		{
			from3 = HasPendingIdeBackgroundWork;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName._fileMenuBtn)
		{
			value = VariantUtils.CreateFrom(in _fileMenuBtn);
			return true;
		}
		if (name == PropertyName._searchMenuBtn)
		{
			value = VariantUtils.CreateFrom(in _searchMenuBtn);
			return true;
		}
		if (name == PropertyName._goToMenuBtn)
		{
			value = VariantUtils.CreateFrom(in _goToMenuBtn);
			return true;
		}
		if (name == PropertyName._overflowMenuBtn)
		{
			value = VariantUtils.CreateFrom(in _overflowMenuBtn);
			return true;
		}
		if (name == PropertyName._scriptNameLabel)
		{
			value = VariantUtils.CreateFrom(in _scriptNameLabel);
			return true;
		}
		if (name == PropertyName._scriptFilter)
		{
			value = VariantUtils.CreateFrom(in _scriptFilter);
			return true;
		}
		if (name == PropertyName._scriptList)
		{
			value = VariantUtils.CreateFrom(in _scriptList);
			return true;
		}
		if (name == PropertyName._scriptTabBar)
		{
			value = VariantUtils.CreateFrom(in _scriptTabBar);
			return true;
		}
		if (name == PropertyName._codeEdit)
		{
			value = VariantUtils.CreateFrom(in _codeEdit);
			return true;
		}
		if (name == PropertyName._lineColLabel)
		{
			value = VariantUtils.CreateFrom(in _lineColLabel);
			return true;
		}
		if (name == PropertyName._compileButton)
		{
			value = VariantUtils.CreateFrom(in _compileButton);
			return true;
		}
		if (name == PropertyName._validateButton)
		{
			value = VariantUtils.CreateFrom(in _validateButton);
			return true;
		}
		if (name == PropertyName._previousProblemButton)
		{
			value = VariantUtils.CreateFrom(in _previousProblemButton);
			return true;
		}
		if (name == PropertyName._nextProblemButton)
		{
			value = VariantUtils.CreateFrom(in _nextProblemButton);
			return true;
		}
		if (name == PropertyName._openBlueprintButton)
		{
			value = VariantUtils.CreateFrom(in _openBlueprintButton);
			return true;
		}
		if (name == PropertyName._generatedBlueprintBar)
		{
			value = VariantUtils.CreateFrom(in _generatedBlueprintBar);
			return true;
		}
		if (name == PropertyName._generatedBlueprintStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _generatedBlueprintStatusLabel);
			return true;
		}
		if (name == PropertyName._generatedBlueprintSourceLabel)
		{
			value = VariantUtils.CreateFrom(in _generatedBlueprintSourceLabel);
			return true;
		}
		if (name == PropertyName._returnToBlueprintButton)
		{
			value = VariantUtils.CreateFrom(in _returnToBlueprintButton);
			return true;
		}
		if (name == PropertyName._diagnosticsLabel)
		{
			value = VariantUtils.CreateFrom(in _diagnosticsLabel);
			return true;
		}
		if (name == PropertyName._diagnosticsPanel)
		{
			value = VariantUtils.CreateFrom(in _diagnosticsPanel);
			return true;
		}
		if (name == PropertyName._diagnosticsList)
		{
			value = VariantUtils.CreateFrom(in _diagnosticsList);
			return true;
		}
		if (name == PropertyName._diagnosticsCopyButton)
		{
			value = VariantUtils.CreateFrom(in _diagnosticsCopyButton);
			return true;
		}
		if (name == PropertyName._diagnosticsContextMenu)
		{
			value = VariantUtils.CreateFrom(in _diagnosticsContextMenu);
			return true;
		}
		if (name == PropertyName._searchBar)
		{
			value = VariantUtils.CreateFrom(in _searchBar);
			return true;
		}
		if (name == PropertyName._searchInput)
		{
			value = VariantUtils.CreateFrom(in _searchInput);
			return true;
		}
		if (name == PropertyName._searchMatchLabel)
		{
			value = VariantUtils.CreateFrom(in _searchMatchLabel);
			return true;
		}
		if (name == PropertyName._searchPreviousButton)
		{
			value = VariantUtils.CreateFrom(in _searchPreviousButton);
			return true;
		}
		if (name == PropertyName._searchNextButton)
		{
			value = VariantUtils.CreateFrom(in _searchNextButton);
			return true;
		}
		if (name == PropertyName._searchCaseButton)
		{
			value = VariantUtils.CreateFrom(in _searchCaseButton);
			return true;
		}
		if (name == PropertyName._searchWholeButton)
		{
			value = VariantUtils.CreateFrom(in _searchWholeButton);
			return true;
		}
		if (name == PropertyName._searchCloseButton)
		{
			value = VariantUtils.CreateFrom(in _searchCloseButton);
			return true;
		}
		if (name == PropertyName._replaceRow)
		{
			value = VariantUtils.CreateFrom(in _replaceRow);
			return true;
		}
		if (name == PropertyName._replaceInput)
		{
			value = VariantUtils.CreateFrom(in _replaceInput);
			return true;
		}
		if (name == PropertyName._replaceNextButton)
		{
			value = VariantUtils.CreateFrom(in _replaceNextButton);
			return true;
		}
		if (name == PropertyName._replaceAllButton)
		{
			value = VariantUtils.CreateFrom(in _replaceAllButton);
			return true;
		}
		if (name == PropertyName._goToLineDialog)
		{
			value = VariantUtils.CreateFrom(in _goToLineDialog);
			return true;
		}
		if (name == PropertyName._goToLineSpin)
		{
			value = VariantUtils.CreateFrom(in _goToLineSpin);
			return true;
		}
		if (name == PropertyName._newFileDialog)
		{
			value = VariantUtils.CreateFrom(in _newFileDialog);
			return true;
		}
		if (name == PropertyName._openFileDialog)
		{
			value = VariantUtils.CreateFrom(in _openFileDialog);
			return true;
		}
		if (name == PropertyName._saveAsDialog)
		{
			value = VariantUtils.CreateFrom(in _saveAsDialog);
			return true;
		}
		if (name == PropertyName._unsavedChangesDialog)
		{
			value = VariantUtils.CreateFrom(in _unsavedChangesDialog);
			return true;
		}
		if (name == PropertyName._discardUnsavedButton)
		{
			value = VariantUtils.CreateFrom(in _discardUnsavedButton);
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
		if (name == PropertyName._isLoadingTab)
		{
			value = VariantUtils.CreateFrom(in _isLoadingTab);
			return true;
		}
		if (name == PropertyName._isCompiling)
		{
			value = VariantUtils.CreateFrom(in _isCompiling);
			return true;
		}
		if (name == PropertyName._isBulkReplacing)
		{
			value = VariantUtils.CreateFrom(in _isBulkReplacing);
			return true;
		}
		if (name == PropertyName._activeSearchMatch)
		{
			value = VariantUtils.CreateFrom(in _activeSearchMatch);
			return true;
		}
		if (name == PropertyName._pendingCloseTabKey)
		{
			value = VariantUtils.CreateFrom(in _pendingCloseTabKey);
			return true;
		}
		if (name == PropertyName._hiddenWorkSuspended)
		{
			value = VariantUtils.CreateFrom(in _hiddenWorkSuspended);
			return true;
		}
		if (name == PropertyName._workspaceSelected)
		{
			value = VariantUtils.CreateFrom(in _workspaceSelected);
			return true;
		}
		if (name == PropertyName._workspaceActive)
		{
			value = VariantUtils.CreateFrom(in _workspaceActive);
			return true;
		}
		if (name == PropertyName._activeProjectRoot)
		{
			value = VariantUtils.CreateFrom(in _activeProjectRoot);
			return true;
		}
		if (name == PropertyName._iconScript)
		{
			value = VariantUtils.CreateFrom(in _iconScript);
			return true;
		}
		if (name == PropertyName._iconSave)
		{
			value = VariantUtils.CreateFrom(in _iconSave);
			return true;
		}
		if (name == PropertyName._iconNew)
		{
			value = VariantUtils.CreateFrom(in _iconNew);
			return true;
		}
		if (name == PropertyName._iconFolder)
		{
			value = VariantUtils.CreateFrom(in _iconFolder);
			return true;
		}
		if (name == PropertyName._iconClose)
		{
			value = VariantUtils.CreateFrom(in _iconClose);
			return true;
		}
		if (name == PropertyName._iconBuildCSharp)
		{
			value = VariantUtils.CreateFrom(in _iconBuildCSharp);
			return true;
		}
		if (name == PropertyName._iconStatusError)
		{
			value = VariantUtils.CreateFrom(in _iconStatusError);
			return true;
		}
		if (name == PropertyName._iconStatusWarning)
		{
			value = VariantUtils.CreateFrom(in _iconStatusWarning);
			return true;
		}
		if (name == PropertyName._debugWorkbench)
		{
			value = VariantUtils.CreateFrom(in _debugWorkbench);
			return true;
		}
		if (name == PropertyName._debugWorkbenchToggle)
		{
			value = VariantUtils.CreateFrom(in _debugWorkbenchToggle);
			return true;
		}
		if (name == PropertyName._scriptDebugNavigationQueued)
		{
			value = VariantUtils.CreateFrom(in _scriptDebugNavigationQueued);
			return true;
		}
		if (name == PropertyName._externalConflictBar)
		{
			value = VariantUtils.CreateFrom(in _externalConflictBar);
			return true;
		}
		if (name == PropertyName._externalConflictTitleLabel)
		{
			value = VariantUtils.CreateFrom(in _externalConflictTitleLabel);
			return true;
		}
		if (name == PropertyName._externalConflictDetailLabel)
		{
			value = VariantUtils.CreateFrom(in _externalConflictDetailLabel);
			return true;
		}
		if (name == PropertyName._viewExternalDiffButton)
		{
			value = VariantUtils.CreateFrom(in _viewExternalDiffButton);
			return true;
		}
		if (name == PropertyName._reloadExternalButton)
		{
			value = VariantUtils.CreateFrom(in _reloadExternalButton);
			return true;
		}
		if (name == PropertyName._overwriteExternalButton)
		{
			value = VariantUtils.CreateFrom(in _overwriteExternalButton);
			return true;
		}
		if (name == PropertyName._saveConflictCopyButton)
		{
			value = VariantUtils.CreateFrom(in _saveConflictCopyButton);
			return true;
		}
		if (name == PropertyName._externalConflictDiffWindow)
		{
			value = VariantUtils.CreateFrom(in _externalConflictDiffWindow);
			return true;
		}
		if (name == PropertyName._externalConflictEditorText)
		{
			value = VariantUtils.CreateFrom(in _externalConflictEditorText);
			return true;
		}
		if (name == PropertyName._externalConflictDiskText)
		{
			value = VariantUtils.CreateFrom(in _externalConflictDiskText);
			return true;
		}
		if (name == PropertyName._externalConflictWatcher)
		{
			value = VariantUtils.CreateFrom(in _externalConflictWatcher);
			return true;
		}
		if (name == PropertyName._externalConflictWatcherScanning)
		{
			value = VariantUtils.CreateFrom(in _externalConflictWatcherScanning);
			return true;
		}
		if (name == PropertyName._definitionButton)
		{
			value = VariantUtils.CreateFrom(in _definitionButton);
			return true;
		}
		if (name == PropertyName._referencesButton)
		{
			value = VariantUtils.CreateFrom(in _referencesButton);
			return true;
		}
		if (name == PropertyName._renameButton)
		{
			value = VariantUtils.CreateFrom(in _renameButton);
			return true;
		}
		if (name == PropertyName._renameUndoButton)
		{
			value = VariantUtils.CreateFrom(in _renameUndoButton);
			return true;
		}
		if (name == PropertyName._renameRedoButton)
		{
			value = VariantUtils.CreateFrom(in _renameRedoButton);
			return true;
		}
		if (name == PropertyName._formatButton)
		{
			value = VariantUtils.CreateFrom(in _formatButton);
			return true;
		}
		if (name == PropertyName._quickFixButton)
		{
			value = VariantUtils.CreateFrom(in _quickFixButton);
			return true;
		}
		if (name == PropertyName._debugStartButton)
		{
			value = VariantUtils.CreateFrom(in _debugStartButton);
			return true;
		}
		if (name == PropertyName._debugStopButton)
		{
			value = VariantUtils.CreateFrom(in _debugStopButton);
			return true;
		}
		if (name == PropertyName._debugStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _debugStatusLabel);
			return true;
		}
		if (name == PropertyName._referencesWindow)
		{
			value = VariantUtils.CreateFrom(in _referencesWindow);
			return true;
		}
		if (name == PropertyName._referencesHeader)
		{
			value = VariantUtils.CreateFrom(in _referencesHeader);
			return true;
		}
		if (name == PropertyName._referencesList)
		{
			value = VariantUtils.CreateFrom(in _referencesList);
			return true;
		}
		if (name == PropertyName._renameDialog)
		{
			value = VariantUtils.CreateFrom(in _renameDialog);
			return true;
		}
		if (name == PropertyName._renameInput)
		{
			value = VariantUtils.CreateFrom(in _renameInput);
			return true;
		}
		if (name == PropertyName._renamePreviewLabel)
		{
			value = VariantUtils.CreateFrom(in _renamePreviewLabel);
			return true;
		}
		if (name == PropertyName._symbolInfoPopup)
		{
			value = VariantUtils.CreateFrom(in _symbolInfoPopup);
			return true;
		}
		if (name == PropertyName._symbolInfoLabel)
		{
			value = VariantUtils.CreateFrom(in _symbolInfoLabel);
			return true;
		}
		if (name == PropertyName._quickFixPopup)
		{
			value = VariantUtils.CreateFrom(in _quickFixPopup);
			return true;
		}
		if (name == PropertyName._hoverTimer)
		{
			value = VariantUtils.CreateFrom(in _hoverTimer);
			return true;
		}
		if (name == PropertyName._renamePreviewTimer)
		{
			value = VariantUtils.CreateFrom(in _renamePreviewTimer);
			return true;
		}
		if (name == PropertyName._debugHost)
		{
			value = VariantUtils.CreateFrom(in _debugHost);
			return true;
		}
		if (name == PropertyName._debugPreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _debugPreviewRoot);
			return true;
		}
		if (name == PropertyName._pendingRenameGeneration)
		{
			value = VariantUtils.CreateFrom(in _pendingRenameGeneration);
			return true;
		}
		if (name == PropertyName._hoverLocalPosition)
		{
			value = VariantUtils.CreateFrom(in _hoverLocalPosition);
			return true;
		}
		if (name == PropertyName._hoverLine)
		{
			value = VariantUtils.CreateFrom(in _hoverLine);
			return true;
		}
		if (name == PropertyName._hoverColumn)
		{
			value = VariantUtils.CreateFrom(in _hoverColumn);
			return true;
		}
		if (name == PropertyName._hoverGeneration)
		{
			value = VariantUtils.CreateFrom(in _hoverGeneration);
			return true;
		}
		if (name == PropertyName._renameGeneration)
		{
			value = VariantUtils.CreateFrom(in _renameGeneration);
			return true;
		}
		if (name == PropertyName._pendingRenameText)
		{
			value = VariantUtils.CreateFrom(in _pendingRenameText);
			return true;
		}
		if (name == PropertyName._ideContextRevision)
		{
			value = VariantUtils.CreateFrom(in _ideContextRevision);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._fileMenuBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchMenuBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._goToMenuBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overflowMenuBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scriptNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scriptFilter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scriptList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scriptTabBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._codeEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lineColLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._compileButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._validateButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousProblemButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nextProblemButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openBlueprintButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._generatedBlueprintBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._generatedBlueprintStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._generatedBlueprintSourceLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._returnToBlueprintButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._diagnosticsLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._diagnosticsPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._diagnosticsList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._diagnosticsCopyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._diagnosticsContextMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchInput, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchMatchLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchPreviousButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchNextButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchCaseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchWholeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchCloseButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._replaceRow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._replaceInput, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._replaceNextButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._replaceAllButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._goToLineDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._goToLineSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._newFileDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openFileDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveAsDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unsavedChangesDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._discardUnsavedButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._currentTabKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isSwitchingTab, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isLoadingTab, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isCompiling, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isBulkReplacing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._activeSearchMatch, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingCloseTabKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._hiddenWorkSuspended, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._workspaceSelected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._workspaceActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._activeProjectRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.CallbackPreviewOwnerId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.UnsupportedScriptRequestCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.UnsupportedSaveAsRequestCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CSharpTemplateCreateCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsHiddenWorkQuiescent, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsCurrentDocumentReadOnly, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.CurrentGeneratedBlueprintSourcePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CurrentGeneratedLinkState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.HiddenWorkDiagnostic, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iconScript, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iconSave, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iconNew, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iconFolder, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iconClose, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iconBuildCSharp, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iconStatusError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._iconStatusWarning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._debugWorkbench, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._debugWorkbenchToggle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._scriptDebugNavigationQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsDebugWorkbenchBound, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsDebugWorkbenchVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsDebugWorkbenchHiddenProcessSuspended, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DebugWorkbenchCallStackCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DebugWorkbenchVariableCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._externalConflictBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._externalConflictTitleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._externalConflictDetailLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._viewExternalDiffButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._reloadExternalButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overwriteExternalButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveConflictCopyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._externalConflictDiffWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._externalConflictEditorText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._externalConflictDiskText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._externalConflictWatcher, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._externalConflictWatcherScanning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LastSaveStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.LastSaveError, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ExternalConflictDetectedCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ExternalConflictReloadCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ExternalConflictOverwriteCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CleanExternalReloadCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.AtomicSaveCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SaveAsRollbackCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CompileSaveGateCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DebugSaveGateCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CompileInvocationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DebugStartInvocationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ExternalConflictWatcherScanCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsCurrentDocumentInExternalConflict, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsExternalConflictBarVisible, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsExternalConflictWatcherIdle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.CurrentDocumentText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._definitionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._referencesButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renameButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renameUndoButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renameRedoButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._formatButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._quickFixButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._debugStartButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._debugStopButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._debugStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._referencesWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._referencesHeader, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._referencesList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renameDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renameInput, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renamePreviewLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._symbolInfoPopup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._symbolInfoLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._quickFixPopup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hoverTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._renamePreviewTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._debugHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._debugPreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._pendingRenameGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._hoverLocalPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._hoverLine, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._hoverColumn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._hoverGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._renameGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingRenameText, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._ideContextRevision, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsDebugSessionRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasLiveDebugSession, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.DebugEntryInvoked, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DebugLoadedAssemblyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.DebugEntryTaskCompleted, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.DebugLoadContextAlive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DebugPreviewChildCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DebugMainThreadId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasPendingIdeBackgroundWork, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.UnsupportedScriptRequestCount, Variant.From<int>(UnsupportedScriptRequestCount));
		info.AddProperty(PropertyName.UnsupportedSaveAsRequestCount, Variant.From<int>(UnsupportedSaveAsRequestCount));
		info.AddProperty(PropertyName.CSharpTemplateCreateCount, Variant.From<int>(CSharpTemplateCreateCount));
		info.AddProperty(PropertyName.LastSaveStatus, Variant.From<ScriptSaveStatus>(LastSaveStatus));
		info.AddProperty(PropertyName.LastSaveError, Variant.From<string>(LastSaveError));
		info.AddProperty(PropertyName.ExternalConflictDetectedCount, Variant.From<int>(ExternalConflictDetectedCount));
		info.AddProperty(PropertyName.ExternalConflictReloadCount, Variant.From<int>(ExternalConflictReloadCount));
		info.AddProperty(PropertyName.ExternalConflictOverwriteCount, Variant.From<int>(ExternalConflictOverwriteCount));
		info.AddProperty(PropertyName.CleanExternalReloadCount, Variant.From<int>(CleanExternalReloadCount));
		info.AddProperty(PropertyName.AtomicSaveCount, Variant.From<int>(AtomicSaveCount));
		info.AddProperty(PropertyName.SaveAsRollbackCount, Variant.From<int>(SaveAsRollbackCount));
		info.AddProperty(PropertyName.CompileSaveGateCount, Variant.From<int>(CompileSaveGateCount));
		info.AddProperty(PropertyName.DebugSaveGateCount, Variant.From<int>(DebugSaveGateCount));
		info.AddProperty(PropertyName.CompileInvocationCount, Variant.From<int>(CompileInvocationCount));
		info.AddProperty(PropertyName.DebugStartInvocationCount, Variant.From<int>(DebugStartInvocationCount));
		info.AddProperty(PropertyName.ExternalConflictWatcherScanCount, Variant.From<int>(ExternalConflictWatcherScanCount));
		info.AddProperty(PropertyName._fileMenuBtn, Variant.From(in _fileMenuBtn));
		info.AddProperty(PropertyName._searchMenuBtn, Variant.From(in _searchMenuBtn));
		info.AddProperty(PropertyName._goToMenuBtn, Variant.From(in _goToMenuBtn));
		info.AddProperty(PropertyName._overflowMenuBtn, Variant.From(in _overflowMenuBtn));
		info.AddProperty(PropertyName._scriptNameLabel, Variant.From(in _scriptNameLabel));
		info.AddProperty(PropertyName._scriptFilter, Variant.From(in _scriptFilter));
		info.AddProperty(PropertyName._scriptList, Variant.From(in _scriptList));
		info.AddProperty(PropertyName._scriptTabBar, Variant.From(in _scriptTabBar));
		info.AddProperty(PropertyName._codeEdit, Variant.From(in _codeEdit));
		info.AddProperty(PropertyName._lineColLabel, Variant.From(in _lineColLabel));
		info.AddProperty(PropertyName._compileButton, Variant.From(in _compileButton));
		info.AddProperty(PropertyName._validateButton, Variant.From(in _validateButton));
		info.AddProperty(PropertyName._previousProblemButton, Variant.From(in _previousProblemButton));
		info.AddProperty(PropertyName._nextProblemButton, Variant.From(in _nextProblemButton));
		info.AddProperty(PropertyName._openBlueprintButton, Variant.From(in _openBlueprintButton));
		info.AddProperty(PropertyName._generatedBlueprintBar, Variant.From(in _generatedBlueprintBar));
		info.AddProperty(PropertyName._generatedBlueprintStatusLabel, Variant.From(in _generatedBlueprintStatusLabel));
		info.AddProperty(PropertyName._generatedBlueprintSourceLabel, Variant.From(in _generatedBlueprintSourceLabel));
		info.AddProperty(PropertyName._returnToBlueprintButton, Variant.From(in _returnToBlueprintButton));
		info.AddProperty(PropertyName._diagnosticsLabel, Variant.From(in _diagnosticsLabel));
		info.AddProperty(PropertyName._diagnosticsPanel, Variant.From(in _diagnosticsPanel));
		info.AddProperty(PropertyName._diagnosticsList, Variant.From(in _diagnosticsList));
		info.AddProperty(PropertyName._diagnosticsCopyButton, Variant.From(in _diagnosticsCopyButton));
		info.AddProperty(PropertyName._diagnosticsContextMenu, Variant.From(in _diagnosticsContextMenu));
		info.AddProperty(PropertyName._searchBar, Variant.From(in _searchBar));
		info.AddProperty(PropertyName._searchInput, Variant.From(in _searchInput));
		info.AddProperty(PropertyName._searchMatchLabel, Variant.From(in _searchMatchLabel));
		info.AddProperty(PropertyName._searchPreviousButton, Variant.From(in _searchPreviousButton));
		info.AddProperty(PropertyName._searchNextButton, Variant.From(in _searchNextButton));
		info.AddProperty(PropertyName._searchCaseButton, Variant.From(in _searchCaseButton));
		info.AddProperty(PropertyName._searchWholeButton, Variant.From(in _searchWholeButton));
		info.AddProperty(PropertyName._searchCloseButton, Variant.From(in _searchCloseButton));
		info.AddProperty(PropertyName._replaceRow, Variant.From(in _replaceRow));
		info.AddProperty(PropertyName._replaceInput, Variant.From(in _replaceInput));
		info.AddProperty(PropertyName._replaceNextButton, Variant.From(in _replaceNextButton));
		info.AddProperty(PropertyName._replaceAllButton, Variant.From(in _replaceAllButton));
		info.AddProperty(PropertyName._goToLineDialog, Variant.From(in _goToLineDialog));
		info.AddProperty(PropertyName._goToLineSpin, Variant.From(in _goToLineSpin));
		info.AddProperty(PropertyName._newFileDialog, Variant.From(in _newFileDialog));
		info.AddProperty(PropertyName._openFileDialog, Variant.From(in _openFileDialog));
		info.AddProperty(PropertyName._saveAsDialog, Variant.From(in _saveAsDialog));
		info.AddProperty(PropertyName._unsavedChangesDialog, Variant.From(in _unsavedChangesDialog));
		info.AddProperty(PropertyName._discardUnsavedButton, Variant.From(in _discardUnsavedButton));
		info.AddProperty(PropertyName._currentTabKey, Variant.From(in _currentTabKey));
		info.AddProperty(PropertyName._isSwitchingTab, Variant.From(in _isSwitchingTab));
		info.AddProperty(PropertyName._isLoadingTab, Variant.From(in _isLoadingTab));
		info.AddProperty(PropertyName._isCompiling, Variant.From(in _isCompiling));
		info.AddProperty(PropertyName._isBulkReplacing, Variant.From(in _isBulkReplacing));
		info.AddProperty(PropertyName._activeSearchMatch, Variant.From(in _activeSearchMatch));
		info.AddProperty(PropertyName._pendingCloseTabKey, Variant.From(in _pendingCloseTabKey));
		info.AddProperty(PropertyName._hiddenWorkSuspended, Variant.From(in _hiddenWorkSuspended));
		info.AddProperty(PropertyName._workspaceSelected, Variant.From(in _workspaceSelected));
		info.AddProperty(PropertyName._workspaceActive, Variant.From(in _workspaceActive));
		info.AddProperty(PropertyName._activeProjectRoot, Variant.From(in _activeProjectRoot));
		info.AddProperty(PropertyName._iconScript, Variant.From(in _iconScript));
		info.AddProperty(PropertyName._iconSave, Variant.From(in _iconSave));
		info.AddProperty(PropertyName._iconNew, Variant.From(in _iconNew));
		info.AddProperty(PropertyName._iconFolder, Variant.From(in _iconFolder));
		info.AddProperty(PropertyName._iconClose, Variant.From(in _iconClose));
		info.AddProperty(PropertyName._iconBuildCSharp, Variant.From(in _iconBuildCSharp));
		info.AddProperty(PropertyName._iconStatusError, Variant.From(in _iconStatusError));
		info.AddProperty(PropertyName._iconStatusWarning, Variant.From(in _iconStatusWarning));
		info.AddProperty(PropertyName._debugWorkbench, Variant.From(in _debugWorkbench));
		info.AddProperty(PropertyName._debugWorkbenchToggle, Variant.From(in _debugWorkbenchToggle));
		info.AddProperty(PropertyName._scriptDebugNavigationQueued, Variant.From(in _scriptDebugNavigationQueued));
		info.AddProperty(PropertyName._externalConflictBar, Variant.From(in _externalConflictBar));
		info.AddProperty(PropertyName._externalConflictTitleLabel, Variant.From(in _externalConflictTitleLabel));
		info.AddProperty(PropertyName._externalConflictDetailLabel, Variant.From(in _externalConflictDetailLabel));
		info.AddProperty(PropertyName._viewExternalDiffButton, Variant.From(in _viewExternalDiffButton));
		info.AddProperty(PropertyName._reloadExternalButton, Variant.From(in _reloadExternalButton));
		info.AddProperty(PropertyName._overwriteExternalButton, Variant.From(in _overwriteExternalButton));
		info.AddProperty(PropertyName._saveConflictCopyButton, Variant.From(in _saveConflictCopyButton));
		info.AddProperty(PropertyName._externalConflictDiffWindow, Variant.From(in _externalConflictDiffWindow));
		info.AddProperty(PropertyName._externalConflictEditorText, Variant.From(in _externalConflictEditorText));
		info.AddProperty(PropertyName._externalConflictDiskText, Variant.From(in _externalConflictDiskText));
		info.AddProperty(PropertyName._externalConflictWatcher, Variant.From(in _externalConflictWatcher));
		info.AddProperty(PropertyName._externalConflictWatcherScanning, Variant.From(in _externalConflictWatcherScanning));
		info.AddProperty(PropertyName._definitionButton, Variant.From(in _definitionButton));
		info.AddProperty(PropertyName._referencesButton, Variant.From(in _referencesButton));
		info.AddProperty(PropertyName._renameButton, Variant.From(in _renameButton));
		info.AddProperty(PropertyName._renameUndoButton, Variant.From(in _renameUndoButton));
		info.AddProperty(PropertyName._renameRedoButton, Variant.From(in _renameRedoButton));
		info.AddProperty(PropertyName._formatButton, Variant.From(in _formatButton));
		info.AddProperty(PropertyName._quickFixButton, Variant.From(in _quickFixButton));
		info.AddProperty(PropertyName._debugStartButton, Variant.From(in _debugStartButton));
		info.AddProperty(PropertyName._debugStopButton, Variant.From(in _debugStopButton));
		info.AddProperty(PropertyName._debugStatusLabel, Variant.From(in _debugStatusLabel));
		info.AddProperty(PropertyName._referencesWindow, Variant.From(in _referencesWindow));
		info.AddProperty(PropertyName._referencesHeader, Variant.From(in _referencesHeader));
		info.AddProperty(PropertyName._referencesList, Variant.From(in _referencesList));
		info.AddProperty(PropertyName._renameDialog, Variant.From(in _renameDialog));
		info.AddProperty(PropertyName._renameInput, Variant.From(in _renameInput));
		info.AddProperty(PropertyName._renamePreviewLabel, Variant.From(in _renamePreviewLabel));
		info.AddProperty(PropertyName._symbolInfoPopup, Variant.From(in _symbolInfoPopup));
		info.AddProperty(PropertyName._symbolInfoLabel, Variant.From(in _symbolInfoLabel));
		info.AddProperty(PropertyName._quickFixPopup, Variant.From(in _quickFixPopup));
		info.AddProperty(PropertyName._hoverTimer, Variant.From(in _hoverTimer));
		info.AddProperty(PropertyName._renamePreviewTimer, Variant.From(in _renamePreviewTimer));
		info.AddProperty(PropertyName._debugHost, Variant.From(in _debugHost));
		info.AddProperty(PropertyName._debugPreviewRoot, Variant.From(in _debugPreviewRoot));
		info.AddProperty(PropertyName._pendingRenameGeneration, Variant.From(in _pendingRenameGeneration));
		info.AddProperty(PropertyName._hoverLocalPosition, Variant.From(in _hoverLocalPosition));
		info.AddProperty(PropertyName._hoverLine, Variant.From(in _hoverLine));
		info.AddProperty(PropertyName._hoverColumn, Variant.From(in _hoverColumn));
		info.AddProperty(PropertyName._hoverGeneration, Variant.From(in _hoverGeneration));
		info.AddProperty(PropertyName._renameGeneration, Variant.From(in _renameGeneration));
		info.AddProperty(PropertyName._pendingRenameText, Variant.From(in _pendingRenameText));
		info.AddProperty(PropertyName._ideContextRevision, Variant.From(in _ideContextRevision));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.UnsupportedScriptRequestCount, out var value))
		{
			UnsupportedScriptRequestCount = value.As<int>();
		}
		if (info.TryGetProperty(PropertyName.UnsupportedSaveAsRequestCount, out var value2))
		{
			UnsupportedSaveAsRequestCount = value2.As<int>();
		}
		if (info.TryGetProperty(PropertyName.CSharpTemplateCreateCount, out var value3))
		{
			CSharpTemplateCreateCount = value3.As<int>();
		}
		if (info.TryGetProperty(PropertyName.LastSaveStatus, out var value4))
		{
			LastSaveStatus = value4.As<ScriptSaveStatus>();
		}
		if (info.TryGetProperty(PropertyName.LastSaveError, out var value5))
		{
			LastSaveError = value5.As<string>();
		}
		if (info.TryGetProperty(PropertyName.ExternalConflictDetectedCount, out var value6))
		{
			ExternalConflictDetectedCount = value6.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ExternalConflictReloadCount, out var value7))
		{
			ExternalConflictReloadCount = value7.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ExternalConflictOverwriteCount, out var value8))
		{
			ExternalConflictOverwriteCount = value8.As<int>();
		}
		if (info.TryGetProperty(PropertyName.CleanExternalReloadCount, out var value9))
		{
			CleanExternalReloadCount = value9.As<int>();
		}
		if (info.TryGetProperty(PropertyName.AtomicSaveCount, out var value10))
		{
			AtomicSaveCount = value10.As<int>();
		}
		if (info.TryGetProperty(PropertyName.SaveAsRollbackCount, out var value11))
		{
			SaveAsRollbackCount = value11.As<int>();
		}
		if (info.TryGetProperty(PropertyName.CompileSaveGateCount, out var value12))
		{
			CompileSaveGateCount = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName.DebugSaveGateCount, out var value13))
		{
			DebugSaveGateCount = value13.As<int>();
		}
		if (info.TryGetProperty(PropertyName.CompileInvocationCount, out var value14))
		{
			CompileInvocationCount = value14.As<int>();
		}
		if (info.TryGetProperty(PropertyName.DebugStartInvocationCount, out var value15))
		{
			DebugStartInvocationCount = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName.ExternalConflictWatcherScanCount, out var value16))
		{
			ExternalConflictWatcherScanCount = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName._fileMenuBtn, out var value17))
		{
			_fileMenuBtn = value17.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._searchMenuBtn, out var value18))
		{
			_searchMenuBtn = value18.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._goToMenuBtn, out var value19))
		{
			_goToMenuBtn = value19.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._overflowMenuBtn, out var value20))
		{
			_overflowMenuBtn = value20.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._scriptNameLabel, out var value21))
		{
			_scriptNameLabel = value21.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._scriptFilter, out var value22))
		{
			_scriptFilter = value22.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._scriptList, out var value23))
		{
			_scriptList = value23.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._scriptTabBar, out var value24))
		{
			_scriptTabBar = value24.As<TabBar>();
		}
		if (info.TryGetProperty(PropertyName._codeEdit, out var value25))
		{
			_codeEdit = value25.As<XWCodeEdit>();
		}
		if (info.TryGetProperty(PropertyName._lineColLabel, out var value26))
		{
			_lineColLabel = value26.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._compileButton, out var value27))
		{
			_compileButton = value27.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._validateButton, out var value28))
		{
			_validateButton = value28.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._previousProblemButton, out var value29))
		{
			_previousProblemButton = value29.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nextProblemButton, out var value30))
		{
			_nextProblemButton = value30.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._openBlueprintButton, out var value31))
		{
			_openBlueprintButton = value31.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._generatedBlueprintBar, out var value32))
		{
			_generatedBlueprintBar = value32.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._generatedBlueprintStatusLabel, out var value33))
		{
			_generatedBlueprintStatusLabel = value33.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._generatedBlueprintSourceLabel, out var value34))
		{
			_generatedBlueprintSourceLabel = value34.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._returnToBlueprintButton, out var value35))
		{
			_returnToBlueprintButton = value35.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._diagnosticsLabel, out var value36))
		{
			_diagnosticsLabel = value36.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._diagnosticsPanel, out var value37))
		{
			_diagnosticsPanel = value37.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._diagnosticsList, out var value38))
		{
			_diagnosticsList = value38.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._diagnosticsCopyButton, out var value39))
		{
			_diagnosticsCopyButton = value39.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._diagnosticsContextMenu, out var value40))
		{
			_diagnosticsContextMenu = value40.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._searchBar, out var value41))
		{
			_searchBar = value41.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._searchInput, out var value42))
		{
			_searchInput = value42.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._searchMatchLabel, out var value43))
		{
			_searchMatchLabel = value43.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._searchPreviousButton, out var value44))
		{
			_searchPreviousButton = value44.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._searchNextButton, out var value45))
		{
			_searchNextButton = value45.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._searchCaseButton, out var value46))
		{
			_searchCaseButton = value46.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._searchWholeButton, out var value47))
		{
			_searchWholeButton = value47.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._searchCloseButton, out var value48))
		{
			_searchCloseButton = value48.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._replaceRow, out var value49))
		{
			_replaceRow = value49.As<Container>();
		}
		if (info.TryGetProperty(PropertyName._replaceInput, out var value50))
		{
			_replaceInput = value50.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._replaceNextButton, out var value51))
		{
			_replaceNextButton = value51.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._replaceAllButton, out var value52))
		{
			_replaceAllButton = value52.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._goToLineDialog, out var value53))
		{
			_goToLineDialog = value53.As<ConfirmationDialog>();
		}
		if (info.TryGetProperty(PropertyName._goToLineSpin, out var value54))
		{
			_goToLineSpin = value54.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._newFileDialog, out var value55))
		{
			_newFileDialog = value55.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._openFileDialog, out var value56))
		{
			_openFileDialog = value56.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._saveAsDialog, out var value57))
		{
			_saveAsDialog = value57.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._unsavedChangesDialog, out var value58))
		{
			_unsavedChangesDialog = value58.As<ConfirmationDialog>();
		}
		if (info.TryGetProperty(PropertyName._discardUnsavedButton, out var value59))
		{
			_discardUnsavedButton = value59.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._currentTabKey, out var value60))
		{
			_currentTabKey = value60.As<string>();
		}
		if (info.TryGetProperty(PropertyName._isSwitchingTab, out var value61))
		{
			_isSwitchingTab = value61.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isLoadingTab, out var value62))
		{
			_isLoadingTab = value62.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isCompiling, out var value63))
		{
			_isCompiling = value63.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._isBulkReplacing, out var value64))
		{
			_isBulkReplacing = value64.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._activeSearchMatch, out var value65))
		{
			_activeSearchMatch = value65.As<int>();
		}
		if (info.TryGetProperty(PropertyName._pendingCloseTabKey, out var value66))
		{
			_pendingCloseTabKey = value66.As<string>();
		}
		if (info.TryGetProperty(PropertyName._hiddenWorkSuspended, out var value67))
		{
			_hiddenWorkSuspended = value67.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._workspaceSelected, out var value68))
		{
			_workspaceSelected = value68.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._workspaceActive, out var value69))
		{
			_workspaceActive = value69.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._activeProjectRoot, out var value70))
		{
			_activeProjectRoot = value70.As<string>();
		}
		if (info.TryGetProperty(PropertyName._iconScript, out var value71))
		{
			_iconScript = value71.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._iconSave, out var value72))
		{
			_iconSave = value72.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._iconNew, out var value73))
		{
			_iconNew = value73.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._iconFolder, out var value74))
		{
			_iconFolder = value74.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._iconClose, out var value75))
		{
			_iconClose = value75.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._iconBuildCSharp, out var value76))
		{
			_iconBuildCSharp = value76.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._iconStatusError, out var value77))
		{
			_iconStatusError = value77.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._iconStatusWarning, out var value78))
		{
			_iconStatusWarning = value78.As<Texture2D>();
		}
		if (info.TryGetProperty(PropertyName._debugWorkbench, out var value79))
		{
			_debugWorkbench = value79.As<XWModDebugWorkbench>();
		}
		if (info.TryGetProperty(PropertyName._debugWorkbenchToggle, out var value80))
		{
			_debugWorkbenchToggle = value80.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._scriptDebugNavigationQueued, out var value81))
		{
			_scriptDebugNavigationQueued = value81.As<int>();
		}
		if (info.TryGetProperty(PropertyName._externalConflictBar, out var value82))
		{
			_externalConflictBar = value82.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._externalConflictTitleLabel, out var value83))
		{
			_externalConflictTitleLabel = value83.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._externalConflictDetailLabel, out var value84))
		{
			_externalConflictDetailLabel = value84.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._viewExternalDiffButton, out var value85))
		{
			_viewExternalDiffButton = value85.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._reloadExternalButton, out var value86))
		{
			_reloadExternalButton = value86.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._overwriteExternalButton, out var value87))
		{
			_overwriteExternalButton = value87.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._saveConflictCopyButton, out var value88))
		{
			_saveConflictCopyButton = value88.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._externalConflictDiffWindow, out var value89))
		{
			_externalConflictDiffWindow = value89.As<Window>();
		}
		if (info.TryGetProperty(PropertyName._externalConflictEditorText, out var value90))
		{
			_externalConflictEditorText = value90.As<TextEdit>();
		}
		if (info.TryGetProperty(PropertyName._externalConflictDiskText, out var value91))
		{
			_externalConflictDiskText = value91.As<TextEdit>();
		}
		if (info.TryGetProperty(PropertyName._externalConflictWatcher, out var value92))
		{
			_externalConflictWatcher = value92.As<Godot.Timer>();
		}
		if (info.TryGetProperty(PropertyName._externalConflictWatcherScanning, out var value93))
		{
			_externalConflictWatcherScanning = value93.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._definitionButton, out var value94))
		{
			_definitionButton = value94.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._referencesButton, out var value95))
		{
			_referencesButton = value95.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._renameButton, out var value96))
		{
			_renameButton = value96.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._renameUndoButton, out var value97))
		{
			_renameUndoButton = value97.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._renameRedoButton, out var value98))
		{
			_renameRedoButton = value98.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._formatButton, out var value99))
		{
			_formatButton = value99.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._quickFixButton, out var value100))
		{
			_quickFixButton = value100.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._debugStartButton, out var value101))
		{
			_debugStartButton = value101.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._debugStopButton, out var value102))
		{
			_debugStopButton = value102.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._debugStatusLabel, out var value103))
		{
			_debugStatusLabel = value103.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._referencesWindow, out var value104))
		{
			_referencesWindow = value104.As<Window>();
		}
		if (info.TryGetProperty(PropertyName._referencesHeader, out var value105))
		{
			_referencesHeader = value105.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._referencesList, out var value106))
		{
			_referencesList = value106.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._renameDialog, out var value107))
		{
			_renameDialog = value107.As<ConfirmationDialog>();
		}
		if (info.TryGetProperty(PropertyName._renameInput, out var value108))
		{
			_renameInput = value108.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._renamePreviewLabel, out var value109))
		{
			_renamePreviewLabel = value109.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName._symbolInfoPopup, out var value110))
		{
			_symbolInfoPopup = value110.As<PopupPanel>();
		}
		if (info.TryGetProperty(PropertyName._symbolInfoLabel, out var value111))
		{
			_symbolInfoLabel = value111.As<RichTextLabel>();
		}
		if (info.TryGetProperty(PropertyName._quickFixPopup, out var value112))
		{
			_quickFixPopup = value112.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._hoverTimer, out var value113))
		{
			_hoverTimer = value113.As<Godot.Timer>();
		}
		if (info.TryGetProperty(PropertyName._renamePreviewTimer, out var value114))
		{
			_renamePreviewTimer = value114.As<Godot.Timer>();
		}
		if (info.TryGetProperty(PropertyName._debugHost, out var value115))
		{
			_debugHost = value115.As<XWScriptDebugHost>();
		}
		if (info.TryGetProperty(PropertyName._debugPreviewRoot, out var value116))
		{
			_debugPreviewRoot = value116.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._pendingRenameGeneration, out var value117))
		{
			_pendingRenameGeneration = value117.As<int>();
		}
		if (info.TryGetProperty(PropertyName._hoverLocalPosition, out var value118))
		{
			_hoverLocalPosition = value118.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._hoverLine, out var value119))
		{
			_hoverLine = value119.As<int>();
		}
		if (info.TryGetProperty(PropertyName._hoverColumn, out var value120))
		{
			_hoverColumn = value120.As<int>();
		}
		if (info.TryGetProperty(PropertyName._hoverGeneration, out var value121))
		{
			_hoverGeneration = value121.As<int>();
		}
		if (info.TryGetProperty(PropertyName._renameGeneration, out var value122))
		{
			_renameGeneration = value122.As<int>();
		}
		if (info.TryGetProperty(PropertyName._pendingRenameText, out var value123))
		{
			_pendingRenameText = value123.As<string>();
		}
		if (info.TryGetProperty(PropertyName._ideContextRevision, out var value124))
		{
			_ideContextRevision = value124.As<long>();
		}
	}
}
