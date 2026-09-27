using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.AdobeAnimateEditor.Inspector;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.GUI;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.OutPutPanel;
using PVZHE.ModEditor.PVZIntegration;
using PVZHE.ModEditor.Registry.BP;
using PVZHE.ModEditor.Registry.Class;
using PVZHE.ModEditor.ResourceEditors;
using PVZHE.ModEditor.ResourceEditors.GUI;
using PVZHE.ModEditor.ScriptEditor;
using PVZHE.ModEditor.Tools;

namespace PVZHE.ModEditor.FileSystem;

[ScriptPath("res://addons/ModEditor/FileSystem/GUI/XWFileSystemPanel.cs")]
public class XWFileSystemPanel : PanelContainer
{
	[Signal]
	public delegate void ImportDockRequestedEventHandler(string resourcePath);

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName SetupToolbarIcons = "SetupToolbarIcons";

		public static readonly StringName SetupSortMenus = "SetupSortMenus";

		public static readonly StringName SetupSortMenu = "SetupSortMenu";

		public static readonly StringName SetButtonIcon = "SetButtonIcon";

		public static readonly StringName SetMenuButtonIcon = "SetMenuButtonIcon";

		public static readonly StringName InitFileSystem = "InitFileSystem";

		public static readonly StringName ConnectExternalFileDropSignal = "ConnectExternalFileDropSignal";

		public static readonly StringName DisconnectExternalFileDropSignal = "DisconnectExternalFileDropSignal";

		public static readonly StringName OnExternalFilesDropped = "OnExternalFilesDropped";

		public static readonly StringName GetDropTargetDirectoryAtMouse = "GetDropTargetDirectoryAtMouse";

		public static readonly StringName OnScanStarted = "OnScanStarted";

		public static readonly StringName OnScanProgress = "OnScanProgress";

		public static readonly StringName OnScanCompleted = "OnScanCompleted";

		public static readonly StringName OnReload = "OnReload";

		public static readonly StringName OnPathSubmitted = "OnPathSubmitted";

		public static readonly StringName OnSearchChanged = "OnSearchChanged";

		public static readonly StringName OnListSearchChanged = "OnListSearchChanged";

		public static readonly StringName OnFolderActivated = "OnFolderActivated";

		public static readonly StringName OnSortMenuItemPressed = "OnSortMenuItemPressed";

		public static readonly StringName ToggleDisplayMode = "ToggleDisplayMode";

		public static readonly StringName UpdateDisplayMode = "UpdateDisplayMode";

		public static readonly StringName ScheduleSplitClamp = "ScheduleSplitClamp";

		public static readonly StringName OnSplitDragged = "OnSplitDragged";

		public static readonly StringName ToggleFileListDisplayMode = "ToggleFileListDisplayMode";

		public static readonly StringName UpdateFileListDisplayButton = "UpdateFileListDisplayButton";

		public static readonly StringName NavigateHistoryPrevious = "NavigateHistoryPrevious";

		public static readonly StringName NavigateHistoryNext = "NavigateHistoryNext";

		public static readonly StringName PushNavigationHistory = "PushNavigationHistory";

		public static readonly StringName UpdateNavigationButtons = "UpdateNavigationButtons";

		public static readonly StringName NormalizeDirectoryPath = "NormalizeDirectoryPath";

		public static readonly StringName OnFavoriteActivated = "OnFavoriteActivated";

		public static readonly StringName OnListItemSelected = "OnListItemSelected";

		public static readonly StringName OnTreeSelectionChanged = "OnTreeSelectionChanged";

		public static readonly StringName OnTreeMultiSelected = "OnTreeMultiSelected";

		public static readonly StringName UpdateSelectedTreeItem = "UpdateSelectedTreeItem";

		public static readonly StringName NavigateToSelectedTreePath = "NavigateToSelectedTreePath";

		public static readonly StringName TryPreviewSelectedAnimation = "TryPreviewSelectedAnimation";

		public static readonly StringName UpdatePreview = "UpdatePreview";

		public static readonly StringName IsImageFile = "IsImageFile";

		public static readonly StringName OnFileActivated = "OnFileActivated";

		public static readonly StringName OpenCompanionResource = "OpenCompanionResource";

		public static readonly StringName InspectSubResource = "InspectSubResource";

		public static readonly StringName NavigateTo = "NavigateTo";

		public static readonly StringName NavigateToProject = "NavigateToProject";

		public static readonly StringName NavigateToPath = "NavigateToPath";

		public static readonly StringName RevealCreatedPath = "RevealCreatedPath";

		public static readonly StringName OpenCreatedPath = "OpenCreatedPath";

		public static readonly StringName OpenCreatedPathDeferred = "OpenCreatedPathDeferred";

		public static readonly StringName ShowCreateDirDialog = "ShowCreateDirDialog";

		public static readonly StringName ShowRemoveDialog = "ShowRemoveDialog";

		public static readonly StringName ConnectContextMenuSignals = "ConnectContextMenuSignals";

		public static readonly StringName OnTreeItemRmbSelected = "OnTreeItemRmbSelected";

		public static readonly StringName OnListItemClicked = "OnListItemClicked";

		public static readonly StringName ShowEmptyTreePopupMenu = "ShowEmptyTreePopupMenu";

		public static readonly StringName ShowFavoritePopupMenu = "ShowFavoritePopupMenu";

		public static readonly StringName ShowItemPopupMenu = "ShowItemPopupMenu";

		public static readonly StringName AddDirectoryCreateItems = "AddDirectoryCreateItems";

		public static readonly StringName OnToolbarCreateMenuAboutToPopup = "OnToolbarCreateMenuAboutToPopup";

		public static readonly StringName OnToolbarCreateMenuIdPressed = "OnToolbarCreateMenuIdPressed";

		public static readonly StringName AddSubResourceCreateItems = "AddSubResourceCreateItems";

		public static readonly StringName CanCreateSceneInDirectory = "CanCreateSceneInDirectory";

		public static readonly StringName CanCreateAdvancedResourceInDirectory = "CanCreateAdvancedResourceInDirectory";

		public static readonly StringName CanCreateBlueprintInDirectory = "CanCreateBlueprintInDirectory";

		public static readonly StringName CanCreateScriptInDirectory = "CanCreateScriptInDirectory";

		public static readonly StringName GetScriptTemplateIdForDirectory = "GetScriptTemplateIdForDirectory";

		public static readonly StringName GetLogicCreateMenuTitle = "GetLogicCreateMenuTitle";

		public static readonly StringName IsProtectedDirectory = "IsProtectedDirectory";

		public static readonly StringName ClearCreateActionIds = "ClearCreateActionIds";

		public static readonly StringName PopupMenuAt = "PopupMenuAt";

		public static readonly StringName RegisterCreatedFileInManifest = "RegisterCreatedFileInManifest";

		public static readonly StringName GetManifestSectionForTemplate = "GetManifestSectionForTemplate";

		public static readonly StringName GetManifestSectionForPath = "GetManifestSectionForPath";

		public static readonly StringName GetIcon = "GetIcon";

		public static readonly StringName OnTreeContextMenuIdPressed = "OnTreeContextMenuIdPressed";

		public static readonly StringName OnFileListContextMenuIdPressed = "OnFileListContextMenuIdPressed";

		public static readonly StringName HandlePopupMenuId = "HandlePopupMenuId";

		public static readonly StringName ShowAdvancedResourceTypeDialog = "ShowAdvancedResourceTypeDialog";

		public static readonly StringName ShowAdvancedResourceNameDialog = "ShowAdvancedResourceNameDialog";

		public static readonly StringName GetSelectedDir = "GetSelectedDir";

		public static readonly StringName GetContextTargetDir = "GetContextTargetDir";

		public static readonly StringName GetDirectoryForPath = "GetDirectoryForPath";

		public static readonly StringName GetTreeItemPath = "GetTreeItemPath";

		public static readonly StringName IsTreeItemFolder = "IsTreeItemFolder";

		public static readonly StringName ShellOpenInFileManager = "ShellOpenInFileManager";

		public static readonly StringName OpenInTerminal = "OpenInTerminal";

		public static readonly StringName CreateNewScene = "CreateNewScene";

		public static readonly StringName CreateNewBlueprint = "CreateNewBlueprint";

		public static readonly StringName OnBlueprintCreated = "OnBlueprintCreated";

		public static readonly StringName CreateNewTextFile = "CreateNewTextFile";

		public static readonly StringName OnNewTextFileSelected = "OnNewTextFileSelected";

		public static readonly StringName ShowResourceCreateDialog = "ShowResourceCreateDialog";

		public static readonly StringName FinalizeModernComponentCreation = "FinalizeModernComponentCreation";

		public static readonly StringName FirstDiagnosticLine = "FirstDiagnosticLine";

		public static readonly StringName ShowSubResourceCreateDialog = "ShowSubResourceCreateDialog";

		public static readonly StringName ShowAnimationImportDialog = "ShowAnimationImportDialog";

		public static readonly StringName OnAnimationImportFileSelected = "OnAnimationImportFileSelected";

		public static readonly StringName OnAnimationImportFileDialogCanceled = "OnAnimationImportFileDialogCanceled";

		public static readonly StringName StartAnimationImportAsync = "StartAnimationImportAsync";

		public static readonly StringName HasImportedAnimationFullData = "HasImportedAnimationFullData";

		public static readonly StringName GetAnimationImportProgressDialog = "GetAnimationImportProgressDialog";

		public static readonly StringName InstantiateAndSaveResource = "InstantiateAndSaveResource";

		public static readonly StringName DuplicateSelected = "DuplicateSelected";

		public static readonly StringName CopyResource = "CopyResource";

		public static readonly StringName PasteResource = "PasteResource";

		public static readonly StringName MoveTo = "MoveTo";

		public static readonly StringName OnMoveTargetDirectorySelected = "OnMoveTargetDirectorySelected";

		public static readonly StringName InstantiateScene = "InstantiateScene";

		public static readonly StringName ReadResourceUid = "ReadResourceUid";

		public static readonly StringName GetListSelectedItemPath = "GetListSelectedItemPath";

		public static readonly StringName ShowNewCSharpScriptDialog = "ShowNewCSharpScriptDialog";

		public static readonly StringName ShowNewLogicDialog = "ShowNewLogicDialog";

		public static readonly StringName ShowNewScriptDialog = "ShowNewScriptDialog";

		public static readonly StringName BeginTreeInlineRename = "BeginTreeInlineRename";

		public static readonly StringName OnTreeInlineRenameSubmitted = "OnTreeInlineRenameSubmitted";

		public static readonly StringName ShowRenameDialog = "ShowRenameDialog";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName _pathEdit = "_pathEdit";

		public static readonly StringName _searchEdit = "_searchEdit";

		public static readonly StringName _listSearchEdit = "_listSearchEdit";

		public static readonly StringName _histPrevBtn = "_histPrevBtn";

		public static readonly StringName _histNextBtn = "_histNextBtn";

		public static readonly StringName _reloadBtn = "_reloadBtn";

		public static readonly StringName _createButton = "_createButton";

		public static readonly StringName _toggleDisplayModeBtn = "_toggleDisplayModeBtn";

		public static readonly StringName _fileListDisplayModeBtn = "_fileListDisplayModeBtn";

		public static readonly StringName _toolbar2HBox = "_toolbar2HBox";

		public static readonly StringName _treeSortButton = "_treeSortButton";

		public static readonly StringName _fileListSortButton = "_fileListSortButton";

		public static readonly StringName _splitBox = "_splitBox";

		public static readonly StringName _fileListVBox = "_fileListVBox";

		public static readonly StringName _tree = "_tree";

		public static readonly StringName _list = "_list";

		public static readonly StringName _scanningLabel = "_scanningLabel";

		public static readonly StringName _scanningProgress = "_scanningProgress";

		public static readonly StringName _scanningVBox = "_scanningVBox";

		public static readonly StringName _previewTextureRect = "_previewTextureRect";

		public static readonly StringName _previewFileNameLabel = "_previewFileNameLabel";

		public static readonly StringName _previewTextureBackground = "_previewTextureBackground";

		public static readonly StringName _animationFilePreview = "_animationFilePreview";

		public static readonly StringName _treePopupMenu = "_treePopupMenu";

		public static readonly StringName _fileListPopupMenu = "_fileListPopupMenu";

		public static readonly StringName _createDirDialog = "_createDirDialog";

		public static readonly StringName _removeDialog = "_removeDialog";

		public static readonly StringName _currentDirPath = "_currentDirPath";

		public static readonly StringName _rmbSelectedPath = "_rmbSelectedPath";

		public static readonly StringName _rmbSelectedData = "_rmbSelectedData";

		public static readonly StringName _clipboardPath = "_clipboardPath";

		public static readonly StringName _clipboardIsCopy = "_clipboardIsCopy";

		public static readonly StringName _navigationIndex = "_navigationIndex";

		public static readonly StringName _displayMode = "_displayMode";

		public static readonly StringName _fileListDisplayMode = "_fileListDisplayMode";

		public static readonly StringName _splitBoxOffsetV = "_splitBoxOffsetV";

		public static readonly StringName _splitBoxOffsetH = "_splitBoxOffsetH";

		public static readonly StringName _splitDraggedConnected = "_splitDraggedConnected";

		public static readonly StringName _fileSystemSignalsConnected = "_fileSystemSignalsConnected";

		public static readonly StringName _externalDropConnected = "_externalDropConnected";

		public static readonly StringName _externalDropWindow = "_externalDropWindow";

		public static readonly StringName _animationImportInProgress = "_animationImportInProgress";

		public static readonly StringName _animationImportProgressDialog = "_animationImportProgressDialog";

		public static readonly StringName _animationImportFileDialog = "_animationImportFileDialog";

		public static readonly StringName _newTextFileDialog = "_newTextFileDialog";

		public static readonly StringName _moveToDirectoryDialog = "_moveToDirectoryDialog";

		public static readonly StringName _moveSourcePath = "_moveSourcePath";

		public static readonly StringName _animationImportTargetDir = "_animationImportTargetDir";

		public static readonly StringName _splitClampPending = "_splitClampPending";

		public static readonly StringName _toolbarCreateTargetDir = "_toolbarCreateTargetDir";

		public static readonly StringName _nextComponentAssemblyResultId = "_nextComponentAssemblyResultId";

		public static readonly StringName _animationInspectRequestVersion = "_animationInspectRequestVersion";
	}

	public new class SignalName : PanelContainer.SignalName
	{
		public static readonly StringName ImportDockRequested = "ImportDockRequested";
	}

	private static readonly string[] SceneExtensions = new string[2] { "tscn", "scn" };

	private const string NameInputDialogScenePath = "res://addons/ModEditor/FileSystem/GUI/Dialog/XWNameInputDialog.tscn";

	private const string ScriptCreateDialogScenePath = "res://addons/ModEditor/FileSystem/GUI/Dialog/XWScriptCreateDialog.tscn";

	private const string ResourceTypeCreateDialogScenePath = "res://addons/ModEditor/GUI/XWCreateDialog.tscn";

	private static PackedScene _nameInputDialogScene;

	private static PackedScene _scriptCreateDialogScene;

	private static PackedScene _resourceTypeCreateDialogScene;

	private LineEdit _pathEdit;

	private LineEdit _searchEdit;

	private LineEdit _listSearchEdit;

	private Button _histPrevBtn;

	private Button _histNextBtn;

	private Button _reloadBtn;

	private MenuButton _createButton;

	private Button _toggleDisplayModeBtn;

	private Button _fileListDisplayModeBtn;

	private HBoxContainer _toolbar2HBox;

	private MenuButton _treeSortButton;

	private MenuButton _fileListSortButton;

	private SplitContainer _splitBox;

	private VBoxContainer _fileListVBox;

	private XWFileSystemTree _tree;

	private XWFileSystemList _list;

	private Label _scanningLabel;

	private ProgressBar _scanningProgress;

	private VBoxContainer _scanningVBox;

	private TextureRect _previewTextureRect;

	private Label _previewFileNameLabel;

	private PanelContainer _previewTextureBackground;

	private AdobeAnimateInspectorPreview _animationFilePreview;

	private PopupMenu _treePopupMenu;

	private PopupMenu _fileListPopupMenu;

	private XWDirectoryCreateDialog _createDirDialog;

	private XWRemoveConfirmDialog _removeDialog;

	private string _currentDirPath = "";

	private string _rmbSelectedPath = "";

	private XWFileSystemTreeItemData _rmbSelectedData;

	private string _clipboardPath = "";

	private bool _clipboardIsCopy;

	private readonly List<string> _navigationHistory = new List<string>();

	private int _navigationIndex = -1;

	private XWFileSystemEnum.DisplayMode _displayMode;

	private XWFileSystemEnum.FileListDisplayMode _fileListDisplayMode = XWFileSystemEnum.FileListDisplayMode.List;

	private int _splitBoxOffsetV = 120;

	private int _splitBoxOffsetH = 180;

	private bool _splitDraggedConnected;

	private bool _fileSystemSignalsConnected;

	private bool _externalDropConnected;

	private Window _externalDropWindow;

	private bool _animationImportInProgress;

	private CancellationTokenSource _animationImportCancellation;

	private XWProgressDialog _animationImportProgressDialog;

	private FileDialog _animationImportFileDialog;

	private FileDialog _newTextFileDialog;

	private FileDialog _moveToDirectoryDialog;

	private string _moveSourcePath = "";

	private string _animationImportTargetDir = "";

	private bool _splitClampPending;

	private readonly Dictionary<int, string> _resourceCreateActionIds = new Dictionary<int, string>();

	private readonly Dictionary<int, string> _subResourceCreateActionIds = new Dictionary<int, string>();

	private const int ResourceCreateActionBaseId = 10000;

	private const int SubResourceCreateActionBaseId = 11000;

	private const int AdvancedResourceCreateId = 12000;

	private string _toolbarCreateTargetDir = "";

	private readonly XWModComponentAssemblyCatalog _componentAssemblyCatalog = new XWModComponentAssemblyCatalog();

	private readonly CancellationTokenSource _componentAssemblyRefreshCancellation = new CancellationTokenSource();

	private readonly object _componentAssemblyResultGate = new object();

	private readonly Dictionary<long, (XWTemplateLibrary.TemplateCreateResult Create, XWModComponentAssemblyCatalog.RefreshResult Refresh)> _componentAssemblyResults = new Dictionary<long, (XWTemplateLibrary.TemplateCreateResult, XWModComponentAssemblyCatalog.RefreshResult)>();

	private long _nextComponentAssemblyResultId;

	private int _animationInspectRequestVersion;

	private ImportDockRequestedEventHandler backing_ImportDockRequested;

	public static XWFileSystemPanel Instance { get; private set; }

	public event ImportDockRequestedEventHandler ImportDockRequested
	{
		add
		{
			backing_ImportDockRequested = (ImportDockRequestedEventHandler)Delegate.Combine(backing_ImportDockRequested, value);
		}
		remove
		{
			backing_ImportDockRequested = (ImportDockRequestedEventHandler)Delegate.Remove(backing_ImportDockRequested, value);
		}
	}

	public override void _Ready()
	{
		Instance = this;
		_pathEdit = GetNode<LineEdit>("%CurrentPathLineEdit");
		_searchEdit = GetNode<LineEdit>("%TreeSearchBox");
		_listSearchEdit = GetNodeOrNull<LineEdit>("%FileListSearchBox");
		_histPrevBtn = GetNode<Button>("%ButtonHistPrev");
		_histNextBtn = GetNode<Button>("%ButtonHistNext");
		_reloadBtn = GetNode<Button>("%ButtonReload");
		_createButton = GetNode<MenuButton>("%ButtonCreate");
		_toggleDisplayModeBtn = GetNode<Button>("%ButtonToggleDisplayMode");
		_fileListDisplayModeBtn = GetNode<Button>("%ButtonFileListDisplayMode");
		_toolbar2HBox = GetNode<HBoxContainer>("%Toolbar2HBox");
		_treeSortButton = GetNode<MenuButton>("%TreeButtonSort");
		_fileListSortButton = GetNode<MenuButton>("%FileListButtonSort");
		_splitBox = GetNode<SplitContainer>("%SplitBox");
		_fileListVBox = GetNode<VBoxContainer>("%FileListVBox");
		_tree = GetNode<XWFileSystemTree>("%FileTree");
		_list = GetNode<XWFileSystemList>("%FileList");
		_scanningVBox = GetNode<VBoxContainer>("%ScanningVBox");
		_scanningLabel = GetNode<Label>("%ScanningLabel");
		_scanningProgress = GetNode<ProgressBar>("%ScanningProgress");
		_previewTextureRect = GetNode<TextureRect>("%PreviewTextureRect");
		_previewFileNameLabel = GetNode<Label>("%PreviewFileNameLabel");
		_previewTextureBackground = GetNode<PanelContainer>("%PreviewTextureBackground");
		_animationFilePreview = GetNode<AdobeAnimateInspectorPreview>("%AnimationFilePreview");
		_treePopupMenu = GetNode<PopupMenu>("%TreePopupMenu");
		_fileListPopupMenu = GetNode<PopupMenu>("%FileListPopupMenu");
		_animationImportFileDialog = GetNode<FileDialog>("%AnimationImportFileDialog");
		_newTextFileDialog = GetNode<FileDialog>("%NewTextFileDialog");
		_moveToDirectoryDialog = GetNode<FileDialog>("%MoveToDirectoryDialog");
		_reloadBtn.Pressed += OnReload;
		_createButton.GetPopup().AboutToPopup += OnToolbarCreateMenuAboutToPopup;
		_createButton.GetPopup().IdPressed += OnToolbarCreateMenuIdPressed;
		_histPrevBtn.Pressed += NavigateHistoryPrevious;
		_histNextBtn.Pressed += NavigateHistoryNext;
		_toggleDisplayModeBtn.Pressed += ToggleDisplayMode;
		_fileListDisplayModeBtn.Pressed += ToggleFileListDisplayMode;
		_pathEdit.TextSubmitted += OnPathSubmitted;
		_searchEdit.TextChanged += OnSearchChanged;
		if (_listSearchEdit != null)
		{
			_listSearchEdit.TextChanged += OnListSearchChanged;
		}
		_tree.ItemSelected += OnTreeSelectionChanged;
		_tree.MultiSelected += OnTreeMultiSelected;
		_tree.ItemActivatedFile += OnFileActivated;
		_tree.ItemActivatedFolder += OnFolderActivated;
		_tree.FavoriteActivated += OnFavoriteActivated;
		_tree.InlineRenameSubmitted += OnTreeInlineRenameSubmitted;
		_list.ItemSelected += OnListItemSelected;
		_list.ItemActivatedFile += OnFileActivated;
		_list.ItemActivatedFolder += OnFolderActivated;
		_animationImportFileDialog.FileSelected += OnAnimationImportFileSelected;
		_animationImportFileDialog.Canceled += OnAnimationImportFileDialogCanceled;
		_newTextFileDialog.FileSelected += OnNewTextFileSelected;
		_moveToDirectoryDialog.DirSelected += OnMoveTargetDirectorySelected;
		_moveToDirectoryDialog.Canceled += () =>
		{
			_moveSourcePath = "";
		};
		if (!_splitDraggedConnected)
		{
			_splitBox.Dragged += OnSplitDragged;
			_splitDraggedConnected = true;
		}
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/FileSystem/GUI/Dialog/XWDirectoryCreateDialog.tscn", null, ResourceLoader.CacheMode.Reuse);
		_createDirDialog = packedScene.Instantiate<XWDirectoryCreateDialog>(PackedScene.GenEditState.Disabled);
		_createDirDialog.Name = "CreateDirDialog";
		AddChild(_createDirDialog, forceReadableName: false, InternalMode.Disabled);
		PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/FileSystem/GUI/Dialog/XWRemoveConfirmDialog.tscn", null, ResourceLoader.CacheMode.Reuse);
		_removeDialog = packedScene2.Instantiate<XWRemoveConfirmDialog>(PackedScene.GenEditState.Disabled);
		_removeDialog.Name = "RemoveDialog";
		AddChild(_removeDialog, forceReadableName: false, InternalMode.Disabled);
		SetupToolbarIcons();
		SetupSortMenus();
		UpdateDisplayMode();
		UpdateFileListDisplayButton();
		UpdateNavigationButtons();
		ConnectContextMenuSignals();
		InitFileSystem();
		ConnectExternalFileDropSignal();
	}

	public override void _ExitTree()
	{
		_componentAssemblyRefreshCancellation.Cancel();
		lock (_componentAssemblyResultGate)
		{
			_componentAssemblyResults.Clear();
		}
		Assembly activeAssembly = _componentAssemblyCatalog.ActiveAssembly;
		if ((object)activeAssembly != null && XWEditorInterface.Instance?.TryGetLoadedResourceEditor("character_component_editor") is XWCharacterComponentVisualResourceEditor xWCharacterComponentVisualResourceEditor)
		{
			xWCharacterComponentVisualResourceEditor.ReleaseComponentAssemblyReferences(activeAssembly);
		}
		_componentAssemblyCatalog.UnloadAsync();
		DisconnectExternalFileDropSignal();
		if (_fileSystemSignalsConnected && XWFileSystem.Instance != null)
		{
			XWFileSystem.Instance.ScanStarted -= OnScanStarted;
			XWFileSystem.Instance.ScanCompleted -= OnScanCompleted;
			XWFileSystem.Instance.ScanProgress -= OnScanProgress;
			_fileSystemSignalsConnected = false;
		}
		base._ExitTree();
	}

	private void SetupToolbarIcons()
	{
		XWClassRegistry instance = XWClassRegistry.Instance;
		Texture2D uIIcon = instance.GetUIIcon("Search");
		Texture2D uIIcon2 = instance.GetUIIcon("Sort");
		SetButtonIcon("%ButtonHistPrev", instance.GetUIIcon("Back"));
		SetButtonIcon("%ButtonHistNext", instance.GetUIIcon("Forward"));
		SetButtonIcon("%ButtonReload", instance.GetUIIcon("Reload"));
		SetMenuButtonIcon("%ButtonCreate", instance.GetUIIcon("Add"));
		SetButtonIcon("%ButtonFileListDisplayMode", instance.GetUIIcon("FileThumbnail"));
		SetButtonIcon("%PreviewHeader", instance.GetUIIcon("FileThumbnail"));
		SetMenuButtonIcon("%TreeButtonSort", uIIcon2);
		SetMenuButtonIcon("%FileListButtonSort", uIIcon2);
		LineEdit nodeOrNull = GetNodeOrNull<LineEdit>("%TreeSearchBox");
		if (nodeOrNull != null && GodotObject.IsInstanceValid(uIIcon))
		{
			nodeOrNull.RightIcon = uIIcon;
		}
	}

	private void SetupSortMenus()
	{
		SetupSortMenu(_treeSortButton);
		SetupSortMenu(_fileListSortButton);
	}

	private void SetupSortMenu(MenuButton button)
	{
		if (button != null)
		{
			PopupMenu popup = button.GetPopup();
			popup.Clear();
			popup.AddItem("名称升序", 0, Key.None);
			popup.AddItem("名称降序", 1, Key.None);
			popup.AddSeparator();
			popup.AddItem("类型升序", 2, Key.None);
			popup.AddItem("类型降序", 3, Key.None);
			popup.AddSeparator();
			popup.AddItem("修改时间升序", 4, Key.None);
			popup.AddItem("修改时间降序", 5, Key.None);
			popup.IdPressed += OnSortMenuItemPressed;
		}
	}

	private void SetButtonIcon(string path, Texture2D icon)
	{
		Button nodeOrNull = GetNodeOrNull<Button>(path);
		if (nodeOrNull != null && GodotObject.IsInstanceValid(icon))
		{
			nodeOrNull.Icon = icon;
		}
	}

	private void SetMenuButtonIcon(string path, Texture2D icon)
	{
		MenuButton nodeOrNull = GetNodeOrNull<MenuButton>(path);
		if (nodeOrNull != null && GodotObject.IsInstanceValid(icon))
		{
			nodeOrNull.Icon = icon;
		}
	}

	private void InitFileSystem()
	{
		XWFileSystem singleton = XWFileSystem.GetSingleton();
		if (!_fileSystemSignalsConnected)
		{
			singleton.ScanStarted += OnScanStarted;
			singleton.ScanCompleted += OnScanCompleted;
			singleton.ScanProgress += OnScanProgress;
			_fileSystemSignalsConnected = true;
		}
	}

	private void ConnectExternalFileDropSignal()
	{
		Window window = GetWindow();
		if (window == null)
		{
			window = GetTree()?.Root;
		}
		if (window != null && (!_externalDropConnected || _externalDropWindow != window))
		{
			DisconnectExternalFileDropSignal();
			window.FilesDropped += OnExternalFilesDropped;
			_externalDropWindow = window;
			_externalDropConnected = true;
		}
	}

	private void DisconnectExternalFileDropSignal()
	{
		if (_externalDropConnected)
		{
			if (_externalDropWindow != null && GodotObject.IsInstanceValid(_externalDropWindow))
			{
				_externalDropWindow.FilesDropped -= OnExternalFilesDropped;
			}
			_externalDropWindow = null;
			_externalDropConnected = false;
		}
	}

	private void OnExternalFilesDropped(string[] files)
	{
		if (files != null && files.Length != 0 && IsVisibleInTree())
		{
			string text = GetDropTargetDirectoryAtMouse();
			if (string.IsNullOrWhiteSpace(text))
			{
				text = GetSelectedDir();
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				text = XWFileSystem.GetSingleton().ProjectFolderPath;
			}
			text = NormalizeDirectoryPath(text);
			List<string> list = XWFileSystemDropHelper.ImportExternalFilesToBestDirectory(files, text);
			if (list.Count == 0)
			{
				XWEditorInterface.Instance?.ShowToast("没有可导入到当前目录的外部资源。");
				return;
			}
			XWFileSystem.GetSingleton().ScanChanges();
			string path = NormalizeDirectoryPath(list[0].GetBaseDir());
			NavigateTo(path, addHistory: false);
			_list.SelectPath(list[0]);
			XWEditorInterface.Instance?.ShowToast($"已导入 {list.Count} 个外部资源。");
		}
	}

	public string GetDropTargetDirectoryAtMouse()
	{
		Vector2 globalMousePosition = GetGlobalMousePosition();
		if (_list != null && _list.Visible && _list.GetGlobalRect().HasPoint(globalMousePosition))
		{
			return _list.GetDropTargetDirectoryAtGlobalPosition(globalMousePosition);
		}
		if (_tree != null && _tree.Visible && _tree.GetGlobalRect().HasPoint(globalMousePosition))
		{
			return _tree.GetDropTargetDirectoryAtGlobalPosition(globalMousePosition);
		}
		return NormalizeDirectoryPath(_currentDirPath);
	}

	private void OnScanStarted()
	{
		_scanningVBox.Visible = true;
		_scanningLabel.Text = "正在扫描文件，请稍候...";
	}

	private void OnScanProgress(int cur, int total)
	{
		_scanningLabel.Text = ((total > 0) ? $"扫描中... {cur}/{total}" : $"扫描中... 已发现 {cur} 项");
		if (total > 0)
		{
			_scanningProgress.MaxValue = total;
			_scanningProgress.Value = cur;
		}
	}

	private void OnScanCompleted()
	{
		_scanningVBox.Visible = false;
		NavigateTo(_currentDirPath, addHistory: false);
		if (_navigationHistory.Count == 0 && !string.IsNullOrEmpty(_currentDirPath))
		{
			PushNavigationHistory(_currentDirPath);
		}
	}

	private void OnReload()
	{
		XWFileSystem.GetSingleton().ScanChanges();
	}

	private void OnPathSubmitted(string path)
	{
		NavigateTo(path);
	}

	private void OnSearchChanged(string query)
	{
		_tree.Search(query);
		_list.Search(query);
	}

	private void OnListSearchChanged(string query)
	{
		_list.Search(query);
	}

	private void OnFolderActivated(string path)
	{
		NavigateTo(path);
	}

	private void OnSortMenuItemPressed(long id)
	{
		XWFileSystem.GetSingleton().SetSortMode((XWFileSystemEnum.SortMode)id);
		NavigateTo(_currentDirPath, addHistory: false);
	}

	private void ToggleDisplayMode()
	{
		_displayMode = _displayMode switch
		{
			XWFileSystemEnum.DisplayMode.VSplit => XWFileSystemEnum.DisplayMode.HSplit, 
			XWFileSystemEnum.DisplayMode.HSplit => XWFileSystemEnum.DisplayMode.TreeOnly, 
			_ => XWFileSystemEnum.DisplayMode.VSplit, 
		};
		UpdateDisplayMode();
	}

	private void UpdateDisplayMode()
	{
		if (_splitBox != null && _fileListVBox != null)
		{
			bool flag = _displayMode != XWFileSystemEnum.DisplayMode.TreeOnly;
			_splitBox.Vertical = _displayMode != XWFileSystemEnum.DisplayMode.HSplit;
			_splitBox.Collapsed = false;
			_splitBox.DraggingEnabled = flag;
			_splitBox.DraggerVisibility = (SplitContainer.DraggerVisibilityEnum)(flag ? 0 : 2);
			_tree.Visible = true;
			_tree.SizeFlagsVertical = SizeFlags.ExpandFill;
			if (_toolbar2HBox != null)
			{
				_toolbar2HBox.Visible = !flag;
			}
			_fileListVBox.Visible = flag;
			if (flag)
			{
				_splitBox.SplitOffsets = new int[1] { _splitBox.Vertical ? _splitBoxOffsetV : _splitBoxOffsetH };
				ScheduleSplitClamp();
			}
			Button toggleDisplayModeBtn = _toggleDisplayModeBtn;
			toggleDisplayModeBtn.Icon = _displayMode switch
			{
				XWFileSystemEnum.DisplayMode.TreeOnly => XWClassRegistry.Instance.GetUIIcon("Panels1"), 
				XWFileSystemEnum.DisplayMode.HSplit => XWClassRegistry.Instance.GetUIIcon("Panels2Alt"), 
				_ => XWClassRegistry.Instance.GetUIIcon("Panels2"), 
			};
			toggleDisplayModeBtn = _toggleDisplayModeBtn;
			toggleDisplayModeBtn.TooltipText = _displayMode switch
			{
				XWFileSystemEnum.DisplayMode.VSplit => "当前为上下分割，点击切换为左右分割", 
				XWFileSystemEnum.DisplayMode.HSplit => "当前为左右分割，点击切换为仅目录树", 
				_ => "当前仅显示目录树，点击显示文件列表", 
			};
		}
	}

	private async void ScheduleSplitClamp()
	{
		if (!_splitClampPending && _splitBox != null && GodotObject.IsInstanceValid(_splitBox))
		{
			_splitClampPending = true;
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			_splitClampPending = false;
			if (_splitBox != null && GodotObject.IsInstanceValid(_splitBox) && _displayMode != XWFileSystemEnum.DisplayMode.TreeOnly && _splitBox.GetChildCount() >= 2)
			{
				_splitBox.ClampSplitOffset();
			}
		}
	}

	private void OnSplitDragged(long offset)
	{
		if (_splitBox != null)
		{
			if (_splitBox.Vertical)
			{
				_splitBoxOffsetV = (int)offset;
			}
			else
			{
				_splitBoxOffsetH = (int)offset;
			}
		}
	}

	private void ToggleFileListDisplayMode()
	{
		_fileListDisplayMode = ((_fileListDisplayMode != XWFileSystemEnum.FileListDisplayMode.List) ? XWFileSystemEnum.FileListDisplayMode.List : XWFileSystemEnum.FileListDisplayMode.Thumbnails);
		_list.SetDisplayMode(_fileListDisplayMode);
		UpdateFileListDisplayButton();
	}

	private void UpdateFileListDisplayButton()
	{
		_fileListDisplayModeBtn.TooltipText = ((_fileListDisplayMode == XWFileSystemEnum.FileListDisplayMode.List) ? "当前为列表视图，点击切换为缩略图" : "当前为缩略图，点击切换为列表视图");
	}

	private void NavigateHistoryPrevious()
	{
		if (_navigationIndex > 0)
		{
			_navigationIndex--;
			NavigateTo(_navigationHistory[_navigationIndex], addHistory: false);
			UpdateNavigationButtons();
		}
	}

	private void NavigateHistoryNext()
	{
		if (_navigationIndex >= 0 && _navigationIndex < _navigationHistory.Count - 1)
		{
			_navigationIndex++;
			NavigateTo(_navigationHistory[_navigationIndex], addHistory: false);
			UpdateNavigationButtons();
		}
	}

	private void PushNavigationHistory(string path)
	{
		if (_navigationIndex >= 0 && _navigationIndex < _navigationHistory.Count && _navigationHistory[_navigationIndex] == path)
		{
			UpdateNavigationButtons();
			return;
		}
		if (_navigationIndex < _navigationHistory.Count - 1)
		{
			_navigationHistory.RemoveRange(_navigationIndex + 1, _navigationHistory.Count - _navigationIndex - 1);
		}
		_navigationHistory.Add(path);
		_navigationIndex = _navigationHistory.Count - 1;
		UpdateNavigationButtons();
	}

	private void UpdateNavigationButtons()
	{
		if (_histPrevBtn != null)
		{
			_histPrevBtn.Disabled = _navigationIndex <= 0;
		}
		if (_histNextBtn != null)
		{
			_histNextBtn.Disabled = _navigationIndex < 0 || _navigationIndex >= _navigationHistory.Count - 1;
		}
	}

	private static string NormalizeDirectoryPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		path = path.Replace('\\', '/');
		if (!path.EndsWith("/"))
		{
			return path + "/";
		}
		return path;
	}

	private void OnFavoriteActivated(string path)
	{
		if (DirAccess.DirExistsAbsolute(path))
		{
			NavigateTo(path);
		}
		else
		{
			OnFileActivated(new XWFileSystemTreeItemData(path, Variant.From(in path), pIsFolder: false));
		}
	}

	private void OnListItemSelected(long index)
	{
		XWFileSystemTreeItemData data = _list.GetItemMetadata((int)index).As<XWFileSystemTreeItemData>();
		UpdatePreview(data);
		TryPreviewSelectedAnimation(data);
	}

	private void OnTreeSelectionChanged()
	{
		UpdateSelectedTreeItem(_tree.GetSelected());
	}

	private void OnTreeMultiSelected(TreeItem item, long column, bool selected)
	{
		if (selected)
		{
			UpdateSelectedTreeItem(item);
		}
		else if (_tree.GetNextSelected(null) == null)
		{
			UpdatePreview(null);
		}
	}

	private void UpdateSelectedTreeItem(TreeItem item)
	{
		if (item == null)
		{
			UpdatePreview(null);
			return;
		}
		if (_tree.IsFavoritesItem(item))
		{
			string from = _tree.GetFavoritePath(item);
			NavigateToSelectedTreePath(from);
			UpdatePreview(new XWFileSystemTreeItemData(from, Variant.From(in from), DirAccess.DirExistsAbsolute(from)));
			return;
		}
		Variant metadata = item.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Nil)
		{
			UpdatePreview(null);
			return;
		}
		XWFileSystemTreeItemData xWFileSystemTreeItemData = metadata.As<XWFileSystemTreeItemData>();
		if (xWFileSystemTreeItemData != null && !xWFileSystemTreeItemData.IsSubResource)
		{
			NavigateToSelectedTreePath(xWFileSystemTreeItemData.Path);
		}
		UpdatePreview(xWFileSystemTreeItemData);
		TryPreviewSelectedAnimation(xWFileSystemTreeItemData);
	}

	private void NavigateToSelectedTreePath(string path)
	{
		string directoryForPath = GetDirectoryForPath(path);
		if (!string.IsNullOrWhiteSpace(directoryForPath) && DirAccess.DirExistsAbsolute(directoryForPath) && !string.Equals(directoryForPath, NormalizeDirectoryPath(_currentDirPath), StringComparison.Ordinal))
		{
			NavigateTo(directoryForPath);
		}
	}

	private void TryPreviewSelectedAnimation(XWFileSystemTreeItemData data)
	{
		int requestVersion = ++_animationInspectRequestVersion;
		if (data == null || data.IsFolder || data.IsSubResource || data.IsCompanionResource || !XWFileSystemCompanionResourcePolicy.IsAdobeAnimateResource(data.Path))
		{
			return;
		}
		string resourcePath = data.Path;
		SceneTreeTimer sceneTreeTimer = GetTree()?.CreateTimer(0.45, processAlways: true, processInPhysics: false, ignoreTimeScale: true);
		if (!GodotObject.IsInstanceValid(sceneTreeTimer))
		{
			return;
		}
		sceneTreeTimer.Timeout += () =>
		{
			if (GodotObject.IsInstanceValid(this) && requestVersion == _animationInspectRequestVersion && GodotObject.IsInstanceValid(_animationFilePreview) && TresExtensionMethod.TryCreateAdobeAnimatePreview(resourcePath, out var animation))
			{
				_previewTextureBackground?.Hide();
				_animationFilePreview.Show();
				_animationFilePreview.EditAnimation(animation);
			}
		};
	}

	private void UpdatePreview(XWFileSystemTreeItemData data)
	{
		if (_previewTextureRect == null || _previewFileNameLabel == null)
		{
			return;
		}
		_animationFilePreview?.Hide();
		_previewTextureBackground?.Show();
		if (data == null || string.IsNullOrEmpty(data.Path))
		{
			_previewTextureRect.Texture = null;
			_previewFileNameLabel.Text = "未选择文件";
			return;
		}
		Label previewFileNameLabel = _previewFileNameLabel;
		string text;
		if (data.IsSubResource || data.IsCompanionResource)
		{
			text = data.GetDisplayName();
		}
		else
		{
			text = (data.IsFolder ? data.Path.TrimSuffix("/").GetFile() : data.Path.GetFile());
		}
		previewFileNameLabel.Text = text;
		Texture2D texture2D = null;
		if (!data.IsSubResource && !data.IsFolder && IsImageFile(data.Path))
		{
			texture2D = XWFileSystemExtensionRegistry.GetIcon(data, 128);
		}
		if (!XWTextureSafety.CanPreview(texture2D))
		{
			texture2D = null;
		}
		if (texture2D == null)
		{
			if (data.IsSubResource)
			{
				texture2D = GetIcon("ResourcePreloader");
			}
			else
			{
				texture2D = (data.IsFolder ? GetIcon("Folder") : XWFileSystemExtensionRegistry.GetIcon(data));
			}
		}
		_previewTextureRect.Texture = texture2D;
	}

	private static bool IsImageFile(string path)
	{
		switch (path.GetExtension().ToLowerInvariant())
		{
		case "png":
		case "jpg":
		case "jpeg":
		case "svg":
		case "webp":
			return true;
		default:
			return false;
		}
	}

	private void OnFileActivated(XWFileSystemTreeItemData data)
	{
		if (data == null)
		{
			return;
		}
		_animationInspectRequestVersion++;
		UpdatePreview(data);
		if (data.IsSubResource)
		{
			InspectSubResource(data);
			return;
		}
		if (data.IsCompanionResource)
		{
			OpenCompanionResource(data);
			return;
		}
		EmitSignal(SignalName.ImportDockRequested, data.Path);
		if (XWFileSystemExtensionRegistry.GetMethod(data) != null)
		{
			XWFileSystemExtensionRegistry.Execute(data);
		}
		else if (ResourceLoader.Exists(data.Path))
		{
			Resource resource = ResourceLoader.Load<Resource>(data.Path, null, ResourceLoader.CacheMode.Reuse);
			if (resource != null)
			{
				XWEditorInterface.Instance?.EditResource(resource, XWResourceEditContext.ForRoot(resource, data.Path, "resource_editor"));
			}
		}
	}

	private void OpenCompanionResource(XWFileSystemTreeItemData data)
	{
		if (data == null || string.IsNullOrWhiteSpace(data.Path))
		{
			return;
		}
		EmitSignal(SignalName.ImportDockRequested, data.Path);
		if (XWFileSystemExtensionRegistry.GetMethod(data) != null)
		{
			XWFileSystemExtensionRegistry.Execute(data);
		}
		else if (ResourceLoader.Exists(data.Path))
		{
			Resource resource = ResourceLoader.Load<Resource>(data.Path, null, ResourceLoader.CacheMode.Reuse);
			if (resource != null)
			{
				XWEditorInterface.Instance?.EditResource(resource, XWResourceEditContext.ForRoot(resource, data.Path, "resource_editor"));
			}
		}
	}

	private void InspectSubResource(XWFileSystemTreeItemData data)
	{
		if (data != null && data.IsSubResource && data.Data.VariantType == Variant.Type.Object && data.Data.As<GodotObject>() is Resource res)
		{
			XWEditorInterface.Instance?.EditResource(res);
		}
	}

	private void NavigateTo(string path, bool addHistory = true)
	{
		path = NormalizeDirectoryPath(path);
		if (!string.IsNullOrEmpty(path))
		{
			_currentDirPath = path;
			_pathEdit.Text = path;
			XWFileSystemDirectory filesystemPath = XWFileSystem.GetSingleton().GetFilesystemPath(path);
			_list.DisplayDirectory(filesystemPath);
			if (addHistory)
			{
				PushNavigationHistory(path);
			}
			UpdatePreview(null);
		}
	}

	public void NavigateToProject(string projectDir)
	{
		if (!string.IsNullOrEmpty(projectDir))
		{
			projectDir = projectDir.Replace('\\', '/');
			if (!projectDir.EndsWith("/"))
			{
				projectDir += "/";
			}
			_currentDirPath = projectDir;
			_navigationHistory.Clear();
			_navigationIndex = -1;
			UpdateNavigationButtons();
			_tree.Init(projectDir);
		}
	}

	public void NavigateToPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return;
		}
		path = path.Replace('\\', '/');
		bool flag = path.EndsWith("/") || DirAccess.DirExistsAbsolute(path);
		string text = (flag ? path : path.GetBaseDir());
		if (!string.IsNullOrWhiteSpace(text))
		{
			NavigateTo(text);
			if (!flag)
			{
				_list.SelectPath(path);
			}
		}
	}

	private void RevealCreatedPath(string path)
	{
		path = path?.Replace('\\', '/') ?? "";
		if (!string.IsNullOrWhiteSpace(path))
		{
			bool flag = path.EndsWith("/") || DirAccess.DirExistsAbsolute(path);
			string text = (flag ? path : path.GetBaseDir());
			if (!string.IsNullOrWhiteSpace(text))
			{
				NavigateTo(text, addHistory: false);
			}
			XWFileSystemTreeItemData data = new XWFileSystemTreeItemData(path, Variant.From(in path), flag);
			if (!flag)
			{
				_list?.SelectPath(path);
			}
			UpdatePreview(data);
		}
	}

	private void OpenCreatedPath(string path)
	{
		path = path?.Replace('\\', '/') ?? "";
		if (!string.IsNullOrWhiteSpace(path))
		{
			RevealCreatedPath(path);
			if (!path.EndsWith("/") && !DirAccess.DirExistsAbsolute(path))
			{
				CallDeferred("OpenCreatedPathDeferred", path);
			}
		}
	}

	public void CompleteTemplateCreationFromTool(XWTemplateLibrary.TemplateCreateResult result)
	{
		if (result != null && result.Success)
		{
			if (string.Equals(result.TemplateId, "character-component", StringComparison.OrdinalIgnoreCase))
			{
				XWEditorInterface.Instance?.ShowToast("角色组件已创建，正在后台编译并刷新图鉴…");
				CompleteModernComponentCreationAsync(result);
			}
			else
			{
				OpenCreatedPath(result.CreatedPath);
			}
		}
	}

	private void OpenCreatedPathDeferred(string path)
	{
		if (IsInsideTree() && !string.IsNullOrWhiteSpace(path))
		{
			OnFileActivated(new XWFileSystemTreeItemData(path, Variant.From(in path), pIsFolder: false));
		}
	}

	public void ShowCreateDirDialog(string basePath)
	{
		_createDirDialog.PopupAt(basePath);
	}

	public void ShowRemoveDialog(string path)
	{
		_removeDialog.PopupFor(path);
	}

	private void ConnectContextMenuSignals()
	{
		_tree.ItemMouseSelected += OnTreeItemRmbSelected;
		_list.ItemClicked += OnListItemClicked;
		_treePopupMenu.IdPressed += OnTreeContextMenuIdPressed;
		_fileListPopupMenu.IdPressed += OnFileListContextMenuIdPressed;
	}

	private void OnTreeItemRmbSelected(Vector2 position, long mouseButtonIndex)
	{
		if (mouseButtonIndex != 2)
		{
			return;
		}
		_list.DeselectAll();
		TreeItem itemAtPosition = _tree.GetItemAtPosition(position);
		if (itemAtPosition == null)
		{
			_rmbSelectedPath = "";
			_rmbSelectedData = null;
			ShowEmptyTreePopupMenu();
			return;
		}
		if (_tree.IsFavoritesRoot(itemAtPosition))
		{
			_rmbSelectedPath = "";
			_rmbSelectedData = null;
			return;
		}
		if (_tree.IsFavoritesItem(itemAtPosition))
		{
			string favPath = (_rmbSelectedPath = _tree.GetFavoritePath(itemAtPosition));
			_rmbSelectedData = null;
			ShowFavoritePopupMenu(favPath);
			return;
		}
		XWFileSystemTreeItemData xWFileSystemTreeItemData = itemAtPosition.GetMetadata(0).As<XWFileSystemTreeItemData>();
		if (xWFileSystemTreeItemData == null)
		{
			_rmbSelectedPath = "";
			_rmbSelectedData = null;
			ShowEmptyTreePopupMenu();
		}
		else
		{
			_rmbSelectedData = xWFileSystemTreeItemData;
			_rmbSelectedPath = (xWFileSystemTreeItemData.IsSubResource ? "" : xWFileSystemTreeItemData.Path);
			ShowItemPopupMenu(xWFileSystemTreeItemData, _treePopupMenu);
		}
	}

	private void OnListItemClicked(long index, Vector2 position, long mouseButtonIndex)
	{
		if (mouseButtonIndex == 2)
		{
			_tree.DeselectAll();
			_list.Select((int)index, single: false);
			XWFileSystemTreeItemData xWFileSystemTreeItemData = _list.GetItemMetadata((int)index).As<XWFileSystemTreeItemData>();
			if (xWFileSystemTreeItemData != null)
			{
				_rmbSelectedPath = xWFileSystemTreeItemData.Path;
				_rmbSelectedData = xWFileSystemTreeItemData;
				UpdatePreview(xWFileSystemTreeItemData);
				ShowItemPopupMenu(xWFileSystemTreeItemData, _fileListPopupMenu);
			}
		}
	}

	private void ShowEmptyTreePopupMenu()
	{
		PopupMenu treePopupMenu = _treePopupMenu;
		treePopupMenu.Clear();
		ClearCreateActionIds();
		treePopupMenu.AddIconItem(GetIcon("Folder"), "新建文件夹...", 9, Key.None);
		AddDirectoryCreateItems(treePopupMenu, GetContextTargetDir());
		treePopupMenu.AddIconItem(GetIcon("TextFile"), "新建文本文件...", 20, Key.None);
		treePopupMenu.AddSeparator();
		treePopupMenu.AddIconItem(GetIcon("Terminal"), "在终端中打开", 21, Key.None);
		treePopupMenu.AddIconItem(GetIcon("Filesystem"), "在文件管理器中显示", 16, Key.None);
		treePopupMenu.AddSeparator();
		treePopupMenu.AddItem("刷新", 17, Key.None);
		PopupMenuAt(treePopupMenu);
	}

	private void ShowFavoritePopupMenu(string favPath)
	{
		PopupMenu treePopupMenu = _treePopupMenu;
		treePopupMenu.Clear();
		treePopupMenu.AddItem("打开", 0, Key.None);
		treePopupMenu.AddIconItem(GetIcon("NonFavorite"), "从收藏中移除", 4, Key.None);
		treePopupMenu.AddSeparator();
		treePopupMenu.AddIconItem(GetIcon("Filesystem"), "在文件管理器中显示", 16, Key.None);
		treePopupMenu.AddItem("复制路径", 13, Key.None);
		PopupMenuAt(treePopupMenu);
	}

	private void ShowItemPopupMenu(XWFileSystemTreeItemData data, PopupMenu popup)
	{
		popup.Clear();
		ClearCreateActionIds();
		if (data.IsSubResource)
		{
			popup.AddIconItem(GetIcon("ResourcePreloader"), "打开子资源", 0, Key.None);
			popup.AddIconItem(GetIcon("ActionCopy"), "复制子资源路径", 15, Key.None);
			PopupMenuAt(popup);
			return;
		}
		List<string> selectedPaths = GetSelectedPaths();
		bool flag = selectedPaths.Count > 0;
		bool flag2 = selectedPaths.Count > 0;
		foreach (string item in selectedPaths)
		{
			if (XWFileSystem.GetSingleton().IsFavorite(item))
			{
				flag2 = false;
			}
			else
			{
				flag = false;
			}
		}
		if (data.IsFolder)
		{
			AddDirectoryCreateItems(popup, data.Path);
			popup.AddIconItem(GetIcon("Folder"), "新建文件夹...", 9, Key.None);
			popup.AddIconItem(GetIcon("TextFile"), "新建文本文件...", 20, Key.None);
			popup.AddSeparator();
			if (!flag)
			{
				popup.AddIconItem(GetIcon("Favorites"), "添加到收藏", 3, Key.None);
			}
			if (!flag2)
			{
				popup.AddIconItem(GetIcon("NonFavorite"), "从收藏中移除", 4, Key.None);
			}
			popup.AddSeparator();
			popup.AddIconItem(GetIcon("Filesystem"), "在文件管理器中显示", 16, Key.None);
			popup.AddIconItem(GetIcon("Terminal"), "在终端中打开", 21, Key.None);
			popup.AddItem("复制路径", 13, Key.None);
			if (!IsProtectedDirectory(data.Path))
			{
				popup.AddSeparator();
				popup.AddIconItem(GetIcon("Rename"), "重命名...", 6, Key.None);
				popup.AddIconItem(GetIcon("Remove"), "删除", 7, Key.None);
			}
		}
		else
		{
			string value = data.Path.GetExtension().ToLower();
			if (Array.IndexOf(SceneExtensions, value) >= 0)
			{
				popup.AddIconItem(GetIcon("Load"), "打开场景", 22, Key.None);
				popup.AddIconItem(GetIcon("Instance"), "实例化", 25, Key.None);
				popup.AddSeparator();
			}
			AddSubResourceCreateItems(popup, data.Path);
			popup.AddIconItem(GetIcon("Load"), "打开", 0, Key.None);
			popup.AddSeparator();
			if (!flag)
			{
				popup.AddIconItem(GetIcon("Favorites"), "添加到收藏", 3, Key.None);
			}
			if (!flag2)
			{
				popup.AddIconItem(GetIcon("NonFavorite"), "从收藏中移除", 4, Key.None);
			}
			popup.AddSeparator();
			popup.AddIconItem(GetIcon("Filesystem"), "在文件管理器中显示", 16, Key.None);
			popup.AddIconItem(GetIcon("ActionCopy"), "复制路径", 13, Key.None);
			popup.AddIconItem(GetIcon("ActionCopy"), "复制绝对路径", 14, Key.None);
			popup.AddIconItem(GetIcon("Instance"), "复制资源路径", 15, Key.None);
			popup.AddSeparator();
			popup.AddIconItem(GetIcon("Rename"), "重命名...", 6, Key.None);
			popup.AddIconItem(GetIcon("MoveUp"), "移动到...", 5, Key.None);
			popup.AddIconItem(GetIcon("Remove"), "删除", 7, Key.None);
			popup.AddSeparator();
			popup.AddIconItem(GetIcon("ActionCopy"), "复制", 18, Key.None);
			popup.AddIconItem(GetIcon("ActionCopy"), "粘贴", 19, Key.None);
			popup.AddIconItem(GetIcon("Duplicate"), "重复", 8, Key.None);
		}
		PopupMenuAt(popup);
	}

	private void AddDirectoryCreateItems(PopupMenu popup, string directoryPath)
	{
		bool flag = false;
		if (CanCreateSceneInDirectory(directoryPath))
		{
			popup.AddIconItem(GetIcon("PackedScene"), "新建场景...", 11, Key.None);
			flag = true;
		}
		if (CanCreateScriptInDirectory(directoryPath))
		{
			popup.AddIconItem(GetIcon("ScriptCreate"), GetLogicCreateMenuTitle(directoryPath), 10, Key.None);
			flag = true;
		}
		foreach (XWResourceCreateRoute.CreateAction item in XWResourceCreateRoute.GetActionsForDirectory(directoryPath, XWFileSystem.GetSingleton().ProjectFolderPath))
		{
			int num = 10000 + _resourceCreateActionIds.Count;
			_resourceCreateActionIds[num] = item.Id;
			popup.AddIconItem(GetIcon(item.IconName), item.DisplayName + "...", num, Key.None);
			flag = true;
		}
		if (CanCreateAdvancedResourceInDirectory(directoryPath))
		{
			popup.AddIconItem(GetIcon("ResourcePreloader"), "新建其他资源...", 12000, Key.None);
			flag = true;
		}
		if (flag)
		{
			popup.AddSeparator();
		}
	}

	private void OnToolbarCreateMenuAboutToPopup()
	{
		PopupMenu popupMenu = _createButton?.GetPopup();
		if (GodotObject.IsInstanceValid(popupMenu))
		{
			_toolbarCreateTargetDir = NormalizeDirectoryPath(_currentDirPath);
			if (string.IsNullOrWhiteSpace(_toolbarCreateTargetDir))
			{
				_toolbarCreateTargetDir = NormalizeDirectoryPath(GetSelectedDir());
			}
			if (string.IsNullOrWhiteSpace(_toolbarCreateTargetDir))
			{
				_toolbarCreateTargetDir = NormalizeDirectoryPath(XWFileSystem.GetSingleton().ProjectFolderPath);
			}
			popupMenu.Clear();
			ClearCreateActionIds();
			AddDirectoryCreateItems(popupMenu, _toolbarCreateTargetDir);
			popupMenu.AddIconItem(GetIcon("Folder"), "新建文件夹...", 9, Key.None);
			popupMenu.AddIconItem(GetIcon("TextFile"), "新建文本文件...", 20, Key.None);
		}
	}

	private void OnToolbarCreateMenuIdPressed(long id)
	{
		HandlePopupMenuId(id, _toolbarCreateTargetDir);
	}

	private void AddSubResourceCreateItems(PopupMenu popup, string ownerPath)
	{
		IReadOnlyList<XWResourceCreateRoute.CreateAction> subResourceActionsForOwner = XWResourceCreateRoute.GetSubResourceActionsForOwner(ownerPath, XWFileSystem.GetSingleton().ProjectFolderPath);
		if (subResourceActionsForOwner.Count == 0)
		{
			return;
		}
		foreach (XWResourceCreateRoute.CreateAction item in subResourceActionsForOwner)
		{
			int num = 11000 + _subResourceCreateActionIds.Count;
			_subResourceCreateActionIds[num] = item.Id;
			popup.AddIconItem(GetIcon(item.IconName), item.DisplayName + "...", num, Key.None);
		}
		popup.AddSeparator();
	}

	private static bool CanCreateSceneInDirectory(string directoryPath)
	{
		return XWModProjectLayout.IsInsideDirectory(directoryPath, XWFileSystem.GetSingleton().ProjectFolderPath, "Scenes");
	}

	private static bool CanCreateAdvancedResourceInDirectory(string directoryPath)
	{
		string text = XWModProjectLayout.ToProjectRelativePath(directoryPath, XWFileSystem.GetSingleton().ProjectFolderPath);
		if (!(text == "Resources"))
		{
			return text.StartsWith("Resources/", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	private static bool CanCreateBlueprintInDirectory(string directoryPath)
	{
		return CanCreateScriptInDirectory(directoryPath);
	}

	private static bool CanCreateScriptInDirectory(string directoryPath)
	{
		string text = XWModProjectLayout.ToProjectRelativePath(directoryPath, XWFileSystem.GetSingleton().ProjectFolderPath);
		switch (text)
		{
		default:
			return text == "Battle/Components";
		case "Scripts":
		case "Battle/Features":
		case "Battle/Processes":
			return true;
		}
	}

	private static string GetScriptTemplateIdForDirectory(string directoryPath)
	{
		string projectFolderPath = XWFileSystem.GetSingleton().ProjectFolderPath;
		if (XWModProjectLayout.IsInsideDirectory(directoryPath, projectFolderPath, "Battle/Features"))
		{
			return "feature-csharp";
		}
		if (XWModProjectLayout.IsInsideDirectory(directoryPath, projectFolderPath, "Battle/Processes"))
		{
			return "process-csharp";
		}
		if (XWModProjectLayout.IsInsideDirectory(directoryPath, projectFolderPath, "Battle/Components"))
		{
			return "character-component";
		}
		return "shared-csharp";
	}

	private static string GetLogicCreateMenuTitle(string directoryPath)
	{
		string projectFolderPath = XWFileSystem.GetSingleton().ProjectFolderPath;
		if (XWModProjectLayout.IsInsideDirectory(directoryPath, projectFolderPath, "Battle/Components"))
		{
			return "新建组件逻辑...";
		}
		return "新建 C# / 蓝图逻辑...";
	}

	private static List<(string TemplateId, string DisplayName)> BuildScriptTemplateOptionsForDirectory(string directoryPath, XWTemplateLibrary templateLibrary)
	{
		List<(string TemplateId, string DisplayName)> options = new List<(string, string)>();
		string projectFolderPath = XWFileSystem.GetSingleton().ProjectFolderPath;
		if (XWModProjectLayout.IsInsideDirectory(directoryPath, projectFolderPath, "Battle/Features"))
		{
			Add("feature-csharp");
		}
		else if (XWModProjectLayout.IsInsideDirectory(directoryPath, projectFolderPath, "Battle/Processes"))
		{
			Add("process-csharp");
		}
		else if (XWModProjectLayout.IsInsideDirectory(directoryPath, projectFolderPath, "Battle/Components"))
		{
			Add("character-component");
		}
		else
		{
			Add("shared-csharp");
			Add("debug-entry-csharp");
			Add("character-event-csharp");
			Add("shovel-event-csharp");
			Add("card-event-csharp");
			Add("character-csharp");
			Add("projectile-csharp");
		}
		if (options.Count == 0)
		{
			Add(GetScriptTemplateIdForDirectory(directoryPath));
		}
		return options;
		void Add(string templateId)
		{
			XWTemplateLibrary.TemplateInfo templateInfo = templateLibrary.FindTemplate(templateId);
			if (templateInfo != null)
			{
				options.Add((templateId, XWTemplatePresentation.Resolve(templateInfo).DisplayNameLabel));
			}
		}
	}

	private static bool IsProtectedDirectory(string path)
	{
		return XWModProjectLayout.IsProtectedDirectory(path, XWFileSystem.GetSingleton().ProjectFolderPath);
	}

	private void ClearCreateActionIds()
	{
		_resourceCreateActionIds.Clear();
		_subResourceCreateActionIds.Clear();
	}

	private void PopupMenuAt(PopupMenu menu)
	{
		menu.Position = DisplayServer.MouseGetPosition();
		menu.ResetSize();
		menu.Popup();
	}

	private static void RegisterCreatedFileInManifest(string filePath, string section)
	{
		if (!string.IsNullOrWhiteSpace(filePath))
		{
			string text = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
			if (!string.IsNullOrWhiteSpace(text))
			{
				XWModManifestSyncService.RegisterPath(text, filePath);
			}
		}
	}

	private static void RegisterCreatedFilesInManifest(XWTemplateLibrary.TemplateCreateResult result, string defaultSection)
	{
		if (result == null)
		{
			return;
		}
		if (result.CreatedPaths != null && result.CreatedPaths.Count > 0)
		{
			string text = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
			if (!string.IsNullOrWhiteSpace(text))
			{
				XWModManifestSyncService.RegisterPaths(text, result.CreatedPaths);
			}
		}
		else
		{
			RegisterCreatedFileInManifest(result.CreatedPath, GetManifestSectionForPath(result.CreatedPath, defaultSection));
		}
	}

	private static string GetManifestSectionForTemplate(string templateId)
	{
		switch (templateId)
		{
		case "blueprint":
			return "Blueprints";
		case "feature-csharp":
		case "process-csharp":
		case "character-component":
		case "shovel-event-csharp":
		case "card-event-csharp":
		case "projectile-csharp":
		case "character-csharp":
		case "shared-csharp":
		case "debug-entry-csharp":
		case "character-event-csharp":
			return "Scripts";
		default:
			return "Resources";
		}
	}

	private static string GetManifestSectionForPath(string path, string defaultSection)
	{
		if (path.GetExtension().ToLowerInvariant() == "cs")
		{
			return "Scripts";
		}
		return defaultSection;
	}

	private Texture2D GetIcon(string iconName)
	{
		return XWClassRegistry.Instance.GetUIIcon(iconName);
	}

	private void OnTreeContextMenuIdPressed(long id)
	{
		HandlePopupMenuId(id);
	}

	private void OnFileListContextMenuIdPressed(long id)
	{
		HandlePopupMenuId(id);
	}

	private void HandlePopupMenuId(long id, string targetDirectoryOverride = "")
	{
		if (id == 12000)
		{
			ShowAdvancedResourceTypeDialog(ResolveTargetDirectory());
			return;
		}
		if (_resourceCreateActionIds.TryGetValue((int)id, out var value))
		{
			string text = ResolveTargetDirectory();
			if (value == "new-animation")
			{
				XWAdobeAnimateTrace.Write("UI context action selected: action=" + value + ", target=" + text);
			}
			ShowResourceCreateDialog(value, text);
			return;
		}
		if (_subResourceCreateActionIds.TryGetValue((int)id, out var value2))
		{
			ShowSubResourceCreateDialog(value2, _rmbSelectedPath);
			return;
		}
		List<string> selectedPaths = GetSelectedPaths();
		string from = ((selectedPaths.Count > 0) ? selectedPaths[0] : "");
		if (string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(_rmbSelectedPath))
		{
			from = _rmbSelectedPath;
		}
		switch ((XWFileSystemEnum.FileMenu)id)
		{
		case XWFileSystemEnum.FileMenu.Open:
			if (_rmbSelectedData != null && _rmbSelectedData.IsSubResource)
			{
				OnFileActivated(_rmbSelectedData);
			}
			else if (!string.IsNullOrEmpty(from))
			{
				if (DirAccess.DirExistsAbsolute(from))
				{
					NavigateTo(from.EndsWith("/") ? from : (from + "/"));
				}
				else
				{
					OnFileActivated(new XWFileSystemTreeItemData(from, Variant.From(in from), pIsFolder: false));
				}
			}
			break;
		case XWFileSystemEnum.FileMenu.NewFolder:
			ShowCreateDirDialog(ResolveTargetDirectory());
			break;
		case XWFileSystemEnum.FileMenu.NewScript:
			ShowNewScriptDialog(ResolveTargetDirectory());
			break;
		case XWFileSystemEnum.FileMenu.NewScene:
			CreateNewScene(ResolveTargetDirectory());
			break;
		case XWFileSystemEnum.FileMenu.NewBlueprint:
			CreateNewBlueprint(ResolveTargetDirectory());
			break;
		case XWFileSystemEnum.FileMenu.NewTextfile:
			CreateNewTextFile(ResolveTargetDirectory());
			break;
		case XWFileSystemEnum.FileMenu.CopyPath:
			if (!string.IsNullOrEmpty(from))
			{
				DisplayServer.ClipboardSet(from);
				XWEditorInterface.Instance?.ShowToast("已复制路径: " + from);
			}
			break;
		case XWFileSystemEnum.FileMenu.CopyAbsolutePath:
			if (!string.IsNullOrEmpty(from))
			{
				DisplayServer.ClipboardSet(ProjectSettings.GlobalizePath(from));
				XWEditorInterface.Instance?.ShowToast("已复制绝对路径");
			}
			break;
		case XWFileSystemEnum.FileMenu.CopyUid:
			if (_rmbSelectedData != null && _rmbSelectedData.IsSubResource)
			{
				DisplayServer.ClipboardSet(_rmbSelectedData.SubResourcePropertyPath);
			}
			else if (!string.IsNullOrEmpty(from))
			{
				string text2 = ReadResourceUid(from);
				DisplayServer.ClipboardSet(string.IsNullOrEmpty(text2) ? from : text2);
				XWEditorInterface.Instance?.ShowToast("已复制资源路径");
			}
			break;
		case XWFileSystemEnum.FileMenu.Rename:
			if (!string.IsNullOrEmpty(from))
			{
				BeginTreeInlineRename(from);
			}
			break;
		case XWFileSystemEnum.FileMenu.Move:
			MoveTo();
			break;
		case XWFileSystemEnum.FileMenu.Duplicate:
			DuplicateSelected();
			break;
		case XWFileSystemEnum.FileMenu.AddFavorite:
			AddFavoriteOp(selectedPaths);
			break;
		case XWFileSystemEnum.FileMenu.RemoveFavorite:
			RemoveFavoriteOp(selectedPaths);
			break;
		case XWFileSystemEnum.FileMenu.Remove:
			DeleteSelected(selectedPaths);
			break;
		case XWFileSystemEnum.FileMenu.ShowInExplorer:
			if (!string.IsNullOrEmpty(from))
			{
				ShellOpenInFileManager(from);
			}
			else if (!string.IsNullOrEmpty(_currentDirPath))
			{
				ShellOpenInFileManager(_currentDirPath);
			}
			break;
		case XWFileSystemEnum.FileMenu.OpenInTerminal:
			OpenInTerminal(GetContextTargetDir());
			break;
		case XWFileSystemEnum.FileMenu.Reimport:
			XWFileSystem.GetSingleton().ScanChanges();
			break;
		case XWFileSystemEnum.FileMenu.Copy:
			CopyResource(from);
			break;
		case XWFileSystemEnum.FileMenu.Paste:
			PasteResource();
			break;
		case XWFileSystemEnum.FileMenu.OpenScene:
			if (!string.IsNullOrEmpty(from))
			{
				OnFileActivated(new XWFileSystemTreeItemData(from, Variant.From(in from), pIsFolder: false));
			}
			break;
		case XWFileSystemEnum.FileMenu.InstantiateScene:
			if (!string.IsNullOrEmpty(from))
			{
				InstantiateScene(from);
			}
			break;
		case XWFileSystemEnum.FileMenu.Inherit:
		case XWFileSystemEnum.FileMenu.Instance:
		case XWFileSystemEnum.FileMenu.InheritScene:
		case XWFileSystemEnum.FileMenu.SetMainScene:
			break;
		}
		string ResolveTargetDirectory()
		{
			if (!string.IsNullOrWhiteSpace(targetDirectoryOverride))
			{
				return NormalizeDirectoryPath(targetDirectoryOverride);
			}
			return GetContextTargetDir();
		}
	}

	private void ShowAdvancedResourceTypeDialog(string baseDir)
	{
		baseDir = NormalizeDirectoryPath(baseDir);
		if (!CanCreateAdvancedResourceInDirectory(baseDir))
		{
			XWEditorInterface.Instance?.ShowToast("高级资源只能创建在 Resources 目录中。");
			return;
		}
		if (_resourceTypeCreateDialogScene == null)
		{
			_resourceTypeCreateDialogScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/GUI/XWCreateDialog.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		XWCreateDialog dialog = _resourceTypeCreateDialogScene?.Instantiate<XWCreateDialog>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(dialog))
		{
			XWEditorInterface.Instance?.ShowToast("无法加载资源类型浏览器。");
			return;
		}
		AddChild(dialog, forceReadableName: false, InternalMode.Disabled);
		dialog.ConfigureForResources();
		dialog.Created += (string typeName) =>
		{
			dialog.QueueFree();
			ShowAdvancedResourceNameDialog(typeName, baseDir);
		};
		dialog.Canceled += dialog.QueueFree;
		dialog.PopupCenteredClamped(new Vector2I(920, 650), 0.9f);
	}

	private void ShowAdvancedResourceNameDialog(string typeName, string baseDir)
	{
		if (string.IsNullOrWhiteSpace(typeName))
		{
			return;
		}
		ConfirmationDialog dialog = CreateNameInputDialog("新建 " + typeName, "输入资源文件名", "New" + typeName, out var nameEdit);
		bool creating;
		if (GodotObject.IsInstanceValid(dialog))
		{
			creating = false;
			dialog.Confirmed += Create;
			dialog.Canceled += Cleanup;
			nameEdit.TextSubmitted += (string _) =>
			{
				Create();
			};
			dialog.PopupCentered();
			nameEdit.GrabFocus();
			nameEdit.SelectAll();
		}
		void Cleanup()
		{
			dialog.QueueFree();
		}
		void Create()
		{
			if (!creating)
			{
				creating = true;
				string text = XWTemplateLibrary.SanitizeName(nameEdit.Text);
				string createdPath;
				string error;
				if (string.IsNullOrWhiteSpace(text))
				{
					Cleanup();
				}
				else if (!TryCreateAdvancedResource(typeName, baseDir, text, out createdPath, out error))
				{
					XWEditorInterface.Instance?.ShowToast("创建失败: " + error);
					creating = false;
				}
				else
				{
					RegisterCreatedFileInManifest(createdPath, "Resources");
					XWFileSystem.GetSingleton().ScanChanges();
					XWEditorInterface.Instance?.ShowToast("已创建 " + typeName + ": " + createdPath);
					OpenCreatedPath(createdPath);
					Cleanup();
				}
			}
		}
	}

	private static bool TryCreateAdvancedResource(string typeName, string baseDir, string safeName, out string createdPath, out string error)
	{
		createdPath = "";
		error = "";
		XWClassRegistry instance = XWClassRegistry.Instance;
		if (!instance.HasClass(typeName) || !instance.IsParentClass(typeName, "Resource"))
		{
			error = typeName + " 不是可创建的 Resource 类型";
			return false;
		}
		XWClassData classData = instance.GetClassData(typeName);
		if (classData == null || !classData.IsGlobalClass || !GodotObject.IsInstanceValid(classData.ScriptFile))
		{
			error = typeName + " 没有可写入的全局类脚本";
			return false;
		}
		try
		{
			Variant variant = instance.Instantiate(typeName);
			if (variant.VariantType != Variant.Type.Object || !(variant.AsGodotObject() is Resource resource))
			{
				error = typeName + " 无法实例化，可能是抽象资源类型";
				return false;
			}
			resource.Free();
			string resourcePath = classData.ScriptFile.ResourcePath;
			if (string.IsNullOrWhiteSpace(resourcePath))
			{
				error = typeName + " 的脚本路径为空";
				return false;
			}
			createdPath = Path.Combine(baseDir, safeName + ".tres").Replace('\\', '/');
			if (File.Exists(createdPath))
			{
				error = "同名资源已经存在";
				return false;
			}
			Directory.CreateDirectory(baseDir);
			string value = safeName.Replace("\\", "\\\\").Replace("\"", "\\\"");
			string value2 = resourcePath.Replace("\\", "/").Replace("\"", "\\\"");
			string contents = $"[gd_resource type=\"Resource\" script_class=\"{typeName}\" format=3]\n\n[ext_resource type=\"Script\" path=\"{value2}\" id=\"1_script\"]\n\n" + "[resource]\nscript = ExtResource(\"1_script\")\n" + $"resource_name = \"{value}\"\nmetadata/mod_display_name = \"{value}\"\n";
			File.WriteAllText(createdPath, contents, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			return true;
		}
		catch (Exception ex)
		{
			error = ex.Message;
			return false;
		}
	}

	private List<string> GetSelectedPaths()
	{
		List<string> list = new List<string>();
		if (_list != null && _list.Visible)
		{
			int[] selectedItems = _list.GetSelectedItems();
			foreach (int idx in selectedItems)
			{
				XWFileSystemTreeItemData xWFileSystemTreeItemData = _list.GetItemMetadata(idx).As<XWFileSystemTreeItemData>();
				if (xWFileSystemTreeItemData != null && !xWFileSystemTreeItemData.IsSubResource)
				{
					list.Add(xWFileSystemTreeItemData.Path);
				}
			}
			return list;
		}
		for (TreeItem nextSelected = _tree.GetNextSelected(null); nextSelected != null; nextSelected = _tree.GetNextSelected(nextSelected))
		{
			if (_tree.IsFavoritesItem(nextSelected))
			{
				string favoritePath = _tree.GetFavoritePath(nextSelected);
				if (!string.IsNullOrEmpty(favoritePath))
				{
					list.Add(favoritePath);
				}
			}
			else if (!_tree.IsFavoritesRoot(nextSelected))
			{
				XWFileSystemTreeItemData xWFileSystemTreeItemData2 = nextSelected.GetMetadata(0).As<XWFileSystemTreeItemData>();
				if (xWFileSystemTreeItemData2 != null && !xWFileSystemTreeItemData2.IsSubResource)
				{
					list.Add(xWFileSystemTreeItemData2.Path);
				}
			}
		}
		return list;
	}

	private string GetSelectedDir()
	{
		List<string> selectedPaths = GetSelectedPaths();
		if (selectedPaths.Count == 0)
		{
			if (!string.IsNullOrEmpty(_currentDirPath))
			{
				return _currentDirPath;
			}
			return XWFileSystem.GetSingleton().ProjectFolderPath;
		}
		string text = selectedPaths[0];
		if (DirAccess.DirExistsAbsolute(text))
		{
			if (!text.EndsWith("/"))
			{
				return text + "/";
			}
			return text;
		}
		return text.GetBaseDir() + "/";
	}

	private string GetContextTargetDir()
	{
		if (!string.IsNullOrEmpty(_rmbSelectedPath))
		{
			return GetDirectoryForPath(_rmbSelectedPath);
		}
		return NormalizeDirectoryPath(string.IsNullOrEmpty(_currentDirPath) ? XWFileSystem.GetSingleton().ProjectFolderPath : _currentDirPath);
	}

	private static string GetDirectoryForPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		path = path.Replace('\\', '/');
		if (path.EndsWith("/") || DirAccess.DirExistsAbsolute(path))
		{
			return NormalizeDirectoryPath(path);
		}
		return NormalizeDirectoryPath(path.GetBaseDir());
	}

	private static string GetTreeItemPath(TreeItem item)
	{
		if (item == null)
		{
			return "";
		}
		Variant metadata = item.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Nil)
		{
			return "";
		}
		XWFileSystemTreeItemData xWFileSystemTreeItemData = metadata.As<XWFileSystemTreeItemData>();
		if (xWFileSystemTreeItemData == null)
		{
			return metadata.AsString();
		}
		return xWFileSystemTreeItemData.Path;
	}

	private static bool IsTreeItemFolder(TreeItem item)
	{
		if (item == null)
		{
			return false;
		}
		Variant metadata = item.GetMetadata(0);
		if (metadata.VariantType == Variant.Type.Nil)
		{
			return false;
		}
		return metadata.As<XWFileSystemTreeItemData>() == null;
	}

	private void ShellOpenInFileManager(string path)
	{
		string text = ProjectSettings.GlobalizePath(path);
		if (DirAccess.DirExistsAbsolute(path))
		{
			OS.ShellOpen(text);
		}
		else
		{
			OS.ShellOpen(text.GetBaseDir());
		}
	}

	private void OpenInTerminal(string selectedPath = "")
	{
		if (string.IsNullOrEmpty(selectedPath))
		{
			selectedPath = GetSelectedDir();
		}
		if (string.IsNullOrEmpty(selectedPath))
		{
			selectedPath = XWFileSystem.GetSingleton().ProjectFolderPath;
		}
		selectedPath = NormalizeDirectoryPath(selectedPath);
		OS.ShellOpen(ProjectSettings.GlobalizePath(selectedPath));
	}

	private void CreateNewScene(string baseDir = "")
	{
		if (string.IsNullOrEmpty(baseDir))
		{
			baseDir = GetSelectedDir();
		}
		baseDir = NormalizeDirectoryPath(baseDir);
		XWFileSystem singleton = XWFileSystem.GetSingleton();
		string text = singleton.GenerateUniqueFileName(baseDir, "NewScene.tscn");
		string text2 = baseDir + text;
		string text3 = "[gd_scene format=3]\n\n[node name=\"Node2D\" type=\"Node2D\"]\n";
		Godot.FileAccess fileAccess = Godot.FileAccess.Open(text2, Godot.FileAccess.ModeFlags.Write);
		if (fileAccess == null)
		{
			XWEditorInterface.Instance?.ShowToast("无法创建场景文件: " + text2);
			return;
		}
		fileAccess.StoreString(text3);
		fileAccess.Close();
		RegisterCreatedFileInManifest(text2, "Resources");
		singleton.ScanChanges();
		XWEditorInterface.Instance?.ShowToast("已创建: " + text2);
		OpenCreatedPath(text2);
	}

	private void CreateNewBlueprint(string baseDir)
	{
		if (string.IsNullOrEmpty(baseDir))
		{
			baseDir = XWFileSystem.GetSingleton().ProjectFolderPath;
		}
		if (string.IsNullOrEmpty(baseDir))
		{
			XWEditorInterface.Instance?.ShowToast("请先打开或创建 Mod 工程");
			return;
		}
		if (!CanCreateBlueprintInDirectory(baseDir))
		{
			XWEditorInterface.Instance?.ShowToast("蓝图只能创建在 Scripts 子目录中。");
			return;
		}
		baseDir = baseDir.Replace('\\', '/');
		if (!baseDir.EndsWith("/"))
		{
			baseDir += "/";
		}
		string text = XWFileSystem.GetSingleton().GenerateUniqueFileName(baseDir, "new_blueprint.tres");
		string path = baseDir + text;
		XWBlueprintCreateDialog xWBlueprintCreateDialog = XWBlueprintCreateDialog.Create();
		xWBlueprintCreateDialog.Config("Node", path);
		xWBlueprintCreateDialog.BlueprintCreated += OnBlueprintCreated;
		AddChild(xWBlueprintCreateDialog, forceReadableName: false, InternalMode.Disabled);
		xWBlueprintCreateDialog.PopupCentered();
	}

	private void OnBlueprintCreated(string filePath)
	{
		RegisterCreatedFileInManifest(filePath, "Blueprints");
		XWFileSystem.GetSingleton().ScanChanges();
		XWEditorInterface.Instance?.ShowToast("已创建蓝图: " + filePath);
		OpenCreatedPath(filePath);
	}

	private void CreateNewTextFile(string baseDir = "")
	{
		if (string.IsNullOrEmpty(baseDir))
		{
			baseDir = GetSelectedDir();
		}
		if (string.IsNullOrEmpty(baseDir))
		{
			baseDir = XWFileSystem.GetSingleton().ProjectFolderPath;
		}
		baseDir = NormalizeDirectoryPath(baseDir);
		_newTextFileDialog.CurrentDir = baseDir;
		_newTextFileDialog.CurrentFile = "new_file.txt";
		_newTextFileDialog.PopupCentered();
	}

	private void OnNewTextFileSelected(string path)
	{
		using Godot.FileAccess fileAccess = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Write);
		if (fileAccess == null)
		{
			XWEditorInterface.Instance?.ShowToast("无法创建文本文件: " + path);
			return;
		}
		fileAccess.StoreString("");
		fileAccess.Close();
		XWFileSystem.GetSingleton().ScanChanges();
		XWEditorInterface.Instance?.ShowToast("已创建文本文件: " + path);
		OpenCreatedPath(path);
	}

	private void ShowResourceCreateDialog(string actionId, string baseDir)
	{
		baseDir = NormalizeDirectoryPath(baseDir);
		IReadOnlyList<XWResourceCreateRoute.CreateAction> actionsForDirectory = XWResourceCreateRoute.GetActionsForDirectory(baseDir, XWFileSystem.GetSingleton().ProjectFolderPath);
		XWResourceCreateRoute.CreateAction createAction = null;
		foreach (XWResourceCreateRoute.CreateAction item in actionsForDirectory)
		{
			if (item.Id == actionId)
			{
				createAction = item;
				break;
			}
		}
		if (createAction == null)
		{
			XWEditorInterface.Instance?.ShowToast("当前目录不能创建该资源。");
			return;
		}
		if (actionId == "new-animation")
		{
			XWAdobeAnimateTrace.Write("UI resource create action: action=" + actionId + ", baseDir=" + baseDir);
			ShowAnimationImportDialog(baseDir);
			return;
		}
		ConfirmationDialog dialog = CreateNameInputDialog(createAction.DisplayName, "输入资源名称", createAction.DefaultName, out var nameEdit);
		bool creating;
		if (GodotObject.IsInstanceValid(dialog))
		{
			creating = false;
			dialog.Confirmed += DoCreate;
			dialog.Canceled += Cleanup;
			nameEdit.TextSubmitted += (string _) =>
			{
				DoCreate();
			};
			dialog.PopupCentered();
			nameEdit.GrabFocus();
			nameEdit.SelectAll();
		}
		void Cleanup()
		{
			dialog.QueueFree();
		}
		void DoCreate()
		{
			if (!creating)
			{
				creating = true;
				string text = nameEdit.Text.Trim();
				if (string.IsNullOrEmpty(text))
				{
					Cleanup();
				}
				else
				{
					XWTemplateLibrary.TemplateCreateResult templateCreateResult = XWResourceCreateRoute.CreateFromAction(actionId, baseDir, text);
					if (templateCreateResult.Success)
					{
						RegisterCreatedFilesInManifest(templateCreateResult, "Resources");
						XWFileSystem.GetSingleton().ScanChanges();
						if (!string.IsNullOrWhiteSpace(templateCreateResult.Warning))
						{
							XWEditorInterface.Instance?.ShowToast("创建完成，但旧备份需要手动清理：" + FirstDiagnosticLine(templateCreateResult.Warning), 1);
						}
						if (actionId == "new-character-component-definition")
						{
							XWEditorInterface.Instance?.ShowToast("角色组件已创建，正在后台编译并刷新图鉴…");
							CompleteModernComponentCreationAsync(templateCreateResult);
						}
						else
						{
							XWEditorInterface.Instance?.ShowToast("已创建 " + templateCreateResult.CreatedPath);
							OpenCreatedPath(templateCreateResult.CreatedPath);
						}
					}
					else
					{
						XWEditorInterface.Instance?.ShowToast("创建失败: " + templateCreateResult.Error);
					}
					Cleanup();
				}
			}
		}
	}

	private async Task CompleteModernComponentCreationAsync(XWTemplateLibrary.TemplateCreateResult create)
	{
		string projectRoot = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
		XWModComponentAssemblyCatalog.RefreshResult item = await _componentAssemblyCatalog.RefreshAsync(projectRoot, _componentAssemblyRefreshCancellation.Token);
		long num = Interlocked.Increment(ref _nextComponentAssemblyResultId);
		lock (_componentAssemblyResultGate)
		{
			_componentAssemblyResults[num] = (create, item);
		}
		CallDeferred("FinalizeModernComponentCreation", num);
	}

	private void FinalizeModernComponentCreation(long resultId)
	{
		(XWTemplateLibrary.TemplateCreateResult, XWModComponentAssemblyCatalog.RefreshResult) value;
		lock (_componentAssemblyResultGate)
		{
			if (!_componentAssemblyResults.Remove(resultId, out value))
			{
				return;
			}
		}
		if (!IsInsideTree())
		{
			return;
		}
		XWModComponentAssemblyCatalog.RefreshResult item = value.Item2;
		if (item == null || !item.Success)
		{
			string message = value.Item2?.Message ?? "未知编译错误";
			XWEditorInterface.Instance?.ShowToast("角色组件编译失败：" + FirstDiagnosticLine(message), 2);
			{
				foreach (string createdPath in value.Item1.CreatedPaths)
				{
					if (createdPath.EndsWith("Definition.cs", StringComparison.OrdinalIgnoreCase))
					{
						OpenCreatedPath(createdPath);
						break;
					}
				}
				return;
			}
		}
		if (XWEditorInterface.Instance?.TryGetLoadedResourceEditor("character_component_editor") is XWCharacterComponentVisualResourceEditor xWCharacterComponentVisualResourceEditor)
		{
			if (value.Item2.ReplacedAssembly != null)
			{
				xWCharacterComponentVisualResourceEditor.ReleaseComponentAssemblyReferences(value.Item2.ReplacedAssembly);
			}
			xWCharacterComponentVisualResourceEditor.SetModComponentDefinitionTypes(value.Item2.LoadedAssembly?.Assembly, value.Item2.DefinitionTypes);
		}
		XWFileSystem.GetSingleton()?.ScanChanges();
		ModCharacterComponentDefinition modCharacterComponentDefinition = ResourceLoader.Load<ModCharacterComponentDefinition>(value.Item1.CreatedPath, "", ResourceLoader.CacheMode.Replace);
		if (GodotObject.IsInstanceValid(modCharacterComponentDefinition) && !TrySynchronizeCreatedComponentDefinition(modCharacterComponentDefinition, value.Item1.CreatedPath, value.Item2.DefinitionTypes, out var error))
		{
			XWEditorInterface.Instance?.ShowToast("角色组件配置初始化失败：" + error, 2);
			OpenCreatedDefinitionScript(value.Item1);
		}
		else if (GodotObject.IsInstanceValid(modCharacterComponentDefinition) && XWResourceEditorRegistry.TryOpen(modCharacterComponentDefinition, value.Item1.CreatedPath))
		{
			XWEditorInterface.Instance?.ShowToast("角色组件已编译并进入可视化编辑器");
		}
		else
		{
			XWEditorInterface.Instance?.ShowToast("角色组件已编译，但 Definition 资源无法打开", 2);
			OpenCreatedPath(value.Item1.CreatedPath);
		}
	}

	[UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Only catalog-validated public Mod component definitions are instantiated for schema synchronization.")]
	private static bool TrySynchronizeCreatedComponentDefinition(ModCharacterComponentDefinition resource, string resourcePath, IReadOnlyList<Type> definitionTypes, out string error)
	{
		error = "";
		Type type = definitionTypes?.FirstOrDefault((Type type2) => string.Equals(type2.FullName, resource.DefinitionTypeName, StringComparison.Ordinal) || string.Equals(type2.Name, resource.DefinitionTypeName, StringComparison.Ordinal));
		if (type == null)
		{
			error = "未找到 " + resource.DefinitionTypeName + " 的活动 Definition";
			return false;
		}
		CharacterComponentDefinition characterComponentDefinition = null;
		try
		{
			characterComponentDefinition = Activator.CreateInstance(type) as CharacterComponentDefinition;
			if (characterComponentDefinition == null)
			{
				error = "无法实例化 " + type.FullName;
				return false;
			}
			if (!resource.SynchronizeConfiguration(characterComponentDefinition))
			{
				return true;
			}
			Error error2 = ResourceSaver.Save(resource, resourcePath, ResourceSaver.SaverFlags.None);
			if (error2 == Error.Ok)
			{
				return true;
			}
			error = $"保存配置元数据失败：{error2}";
			return false;
		}
		catch (Exception ex)
		{
			error = ex.GetBaseException().Message;
			return false;
		}
		finally
		{
			characterComponentDefinition?.Dispose();
		}
	}

	private void OpenCreatedDefinitionScript(XWTemplateLibrary.TemplateCreateResult create)
	{
		foreach (string createdPath in create.CreatedPaths)
		{
			if (createdPath.EndsWith("Definition.cs", StringComparison.OrdinalIgnoreCase))
			{
				OpenCreatedPath(createdPath);
				break;
			}
		}
	}

	private static string FirstDiagnosticLine(string message)
	{
		if (string.IsNullOrWhiteSpace(message))
		{
			return "未知错误";
		}
		using StringReader stringReader = new StringReader(message);
		return stringReader.ReadLine()?.Trim() ?? message.Trim();
	}

	private void ShowSubResourceCreateDialog(string actionId, string ownerPath)
	{
		ownerPath = ownerPath?.Replace('\\', '/') ?? "";
		IReadOnlyList<XWResourceCreateRoute.CreateAction> subResourceActionsForOwner = XWResourceCreateRoute.GetSubResourceActionsForOwner(ownerPath, XWFileSystem.GetSingleton().ProjectFolderPath);
		XWResourceCreateRoute.CreateAction createAction = null;
		foreach (XWResourceCreateRoute.CreateAction item in subResourceActionsForOwner)
		{
			if (item.Id == actionId)
			{
				createAction = item;
				break;
			}
		}
		if (createAction == null)
		{
			XWEditorInterface.Instance?.ShowToast("当前资源不能创建该子资源。");
			return;
		}
		ConfirmationDialog dialog = CreateNameInputDialog(createAction.DisplayName, "输入子资源名称", createAction.DefaultName, out var nameEdit);
		bool creating;
		if (GodotObject.IsInstanceValid(dialog))
		{
			creating = false;
			dialog.Confirmed += DoCreate;
			dialog.Canceled += Cleanup;
			nameEdit.TextSubmitted += (string _) =>
			{
				DoCreate();
			};
			dialog.PopupCentered();
			nameEdit.GrabFocus();
			nameEdit.SelectAll();
		}
		void Cleanup()
		{
			dialog.QueueFree();
		}
		void DoCreate()
		{
			if (!creating)
			{
				creating = true;
				string text = nameEdit.Text.Trim();
				if (string.IsNullOrEmpty(text))
				{
					Cleanup();
				}
				else
				{
					XWTemplateLibrary.TemplateCreateResult templateCreateResult = XWResourceCreateRoute.CreateSubResourceFromAction(actionId, ownerPath, text);
					if (templateCreateResult.Success)
					{
						RegisterCreatedFilesInManifest(templateCreateResult, GetManifestSectionForTemplate(templateCreateResult.TemplateId));
						XWFileSystem.GetSingleton().ScanChanges();
						XWEditorInterface.Instance?.ShowToast("已创建子资源 " + templateCreateResult.CreatedPath);
						OpenCreatedPath(templateCreateResult.CreatedPath);
					}
					else
					{
						XWEditorInterface.Instance?.ShowToast("创建子资源失败: " + templateCreateResult.Error);
					}
					Cleanup();
				}
			}
		}
	}

	private void ShowAnimationImportDialog(string baseDir)
	{
		baseDir = NormalizeDirectoryPath(baseDir);
		XWAdobeAnimateTrace.Write("UI file dialog open: baseDir=" + baseDir);
		XWEditorInterface.Instance?.AddOutputMessage("Adobe Animate import dialog: " + baseDir, XWOutputPanel.MessageType.Editor);
		if (string.IsNullOrEmpty(baseDir))
		{
			XWEditorInterface.Instance?.ShowToast("请先打开或创建 Mod 工程。");
			return;
		}
		_animationImportTargetDir = baseDir;
		_animationImportFileDialog.PopupCentered();
	}

	private void OnAnimationImportFileSelected(string path)
	{
		XWAdobeAnimateTrace.Write("UI file dialog result: ok=True, count=1, first=" + path);
		XWEditorInterface.Instance?.AddOutputMessage("Adobe Animate file dialog result: ok=True, count=1", XWOutputPanel.MessageType.Editor);
		StartAnimationImportAsync(path, _animationImportTargetDir);
	}

	private void OnAnimationImportFileDialogCanceled()
	{
		XWAdobeAnimateTrace.Write("UI file dialog result: ok=False, count=0, first=");
		XWEditorInterface.Instance?.AddOutputMessage("Adobe Animate file dialog result: ok=False, count=0", XWOutputPanel.MessageType.Editor);
	}

	private async void StartAnimationImportAsync(string sourcePath, string baseDir)
	{
		if (_animationImportInProgress)
		{
			XWEditorInterface.Instance?.ShowToast("已有动画导入任务正在运行");
			return;
		}
		XWAdobeAnimateTrace.ResetLog();
		XWAdobeAnimateTrace.Write("UI Adobe Animate import start: source=" + sourcePath + ", baseDir=" + baseDir);
		XWEditorInterface.Instance?.AddOutputMessage("Adobe Animate import start: " + sourcePath, XWOutputPanel.MessageType.Editor);
		XWEditorInterface.Instance?.AddOutputMessage("Adobe Animate trace log: " + XWAdobeAnimateTrace.LogPath, XWOutputPanel.MessageType.Editor);
		_animationImportInProgress = true;
		_animationImportCancellation?.Dispose();
		_animationImportCancellation = new CancellationTokenSource();
		XWProgressDialog progressDialog = GetAnimationImportProgressDialog();
		progressDialog.Title = "导入 Adobe Animate 动画";
		progressDialog.SetMaxValue(100.0);
		progressDialog.SetProgressValue(5.0);
		progressDialog.SetStatus("正在导入: " + sourcePath.GetFile());
		progressDialog.PopupCentered();
		try
		{
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			progressDialog.SetProgressValue(25.0);
			progressDialog.SetStatus("正在导出 DAT: " + sourcePath.GetFile());
			progressDialog.SetProgressValue(55.0);
			XWAdobeAnimateImportResult xWAdobeAnimateImportResult = await XWAdobeAnimateImportService.ImportAdobeAnimateFileAsync(sourcePath, baseDir, "", _animationImportCancellation.Token);
			XWAdobeAnimateTrace.Write($"UI Adobe Animate import result: success={xWAdobeAnimateImportResult.Success}, resource={xWAdobeAnimateImportResult.ResourcePath}, dat={xWAdobeAnimateImportResult.DatPath}, error={xWAdobeAnimateImportResult.Error}");
			if (!xWAdobeAnimateImportResult.Success)
			{
				XWEditorInterface.Instance?.ShowToast("动画导入失败: " + xWAdobeAnimateImportResult.Error);
				return;
			}
			progressDialog.SetStatus("正在读取动画数据: " + xWAdobeAnimateImportResult.ResourcePath.GetFile());
			progressDialog.SetProgressValue(75.0);
			if (HydrateImportedAnimationResource(xWAdobeAnimateImportResult, progressDialog))
			{
				progressDialog.SetProgressValue(100.0);
				XWEditorInterface.Instance?.ShowToast("已导入动画: " + xWAdobeAnimateImportResult.ResourcePath);
				if (GodotObject.IsInstanceValid(progressDialog))
				{
					progressDialog.Hide();
				}
				XWAdobeAnimateTrace.Write("UI Adobe Animate import resource ready: " + xWAdobeAnimateImportResult.ResourcePath);
				OpenCreatedPath(xWAdobeAnimateImportResult.ResourcePath);
			}
		}
		catch (Exception ex)
		{
			XWEditorInterface.Instance?.ShowToast("动画导入失败: " + ex.Message);
		}
		finally
		{
			_animationImportInProgress = false;
			_animationImportCancellation?.Dispose();
			_animationImportCancellation = null;
			if (GodotObject.IsInstanceValid(progressDialog))
			{
				progressDialog.Hide();
			}
		}
	}

	private bool HydrateImportedAnimationResource(XWAdobeAnimateImportResult result, XWProgressDialog progressDialog)
	{
		if (result == null || string.IsNullOrWhiteSpace(result.ResourcePath) || string.IsNullOrWhiteSpace(result.DatPath))
		{
			XWEditorInterface.Instance?.ShowToast("动画导入失败: 缺少动画资源或 DAT 路径。");
			return false;
		}
		XWAdobeAnimateTrace.Write("UI Adobe Animate import full data load start: resource=" + result.ResourcePath + ", dat=" + result.DatPath);
		AdobeAnimateData adobeAnimateData = ResourceLoader.Load<AdobeAnimateData>(result.ResourcePath, "", ResourceLoader.CacheMode.Ignore);
		if (adobeAnimateData == null)
		{
			adobeAnimateData = new AdobeAnimateData
			{
				ResourceName = result.ResourcePath.GetFile().GetBaseName(),
				ResourcePath = result.ResourcePath
			};
		}
		adobeAnimateData.ResourcePath = result.ResourcePath;
		adobeAnimateData.SetAnimeFileForModImport(result.DatPath.GetFile());
		adobeAnimateData.InitForModImport(result.DatPath);
		XWAdobeAnimateTrace.Write($"UI Adobe Animate import runtime data state: frames={adobeAnimateData.frameMax}, atlasSource={adobeAnimateData.HasEmbeddedAtlasSource()}, media={adobeAnimateData.mediaDictionary?.Count ?? 0}, layers={adobeAnimateData.layerDictionary?.Count ?? 0}, packedFrames={adobeAnimateData.frameOffsets?.Length ?? 0}, packedSlices={adobeAnimateData.sliceKeys?.Length ?? 0}, mediaRects={adobeAnimateData.mediaRects?.Length ?? 0}");
		if (!HasImportedAnimationFullData(adobeAnimateData))
		{
			XWAdobeAnimateTrace.WriteError("UI Adobe Animate import full data missing after load: resource=" + result.ResourcePath + ", dat=" + result.DatPath);
			XWEditorInterface.Instance?.ShowToast("动画导入失败: DAT 已生成，但完整动画数据读取失败。");
			return false;
		}
		Error error = ResourceSaver.Save(adobeAnimateData, result.ResourcePath, ResourceSaver.SaverFlags.None);
		XWAdobeAnimateTrace.Write($"UI Adobe Animate import runtime data saved: resource={result.ResourcePath}, result={error}, frames={adobeAnimateData.frameMax}, layers={adobeAnimateData.layerDictionary?.Count ?? 0}, packedFrames={adobeAnimateData.frameOffsets?.Length ?? 0}, packedSlices={adobeAnimateData.sliceKeys?.Length ?? 0}");
		if (error != Error.Ok)
		{
			XWEditorInterface.Instance?.ShowToast($"动画导入失败: 完整动画资源保存失败 {error}");
			return false;
		}
		progressDialog?.SetProgressValue(90.0);
		return true;
	}

	private static bool HasImportedAnimationFullData(AdobeAnimateData animation)
	{
		if (animation != null && animation.frameMax > 0 && animation.HasEmbeddedAtlasSource() && animation.mediaDictionary != null && animation.mediaDictionary.Count > 0 && animation.layerDictionary != null && animation.layerDictionary.Count > 0 && animation.frameOffsets != null && animation.frameOffsets.Length == animation.frameMax && animation.frameCounts != null && animation.frameCounts.Length == animation.frameMax && animation.mediaRects != null && animation.mediaRects.Length != 0 && animation.sliceKeys != null)
		{
			return animation.sliceKeys.Length != 0;
		}
		return false;
	}

	private XWProgressDialog GetAnimationImportProgressDialog()
	{
		if (_animationImportProgressDialog != null && GodotObject.IsInstanceValid(_animationImportProgressDialog))
		{
			return _animationImportProgressDialog;
		}
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/GUI/XWProgressDialog.tscn", null, ResourceLoader.CacheMode.Reuse);
		_animationImportProgressDialog = packedScene.Instantiate<XWProgressDialog>(PackedScene.GenEditState.Disabled);
		_animationImportProgressDialog.ProgressCanceled += () =>
		{
			if (_animationImportCancellation != null)
			{
				_animationImportCancellation.Cancel();
				XWEditorInterface.Instance?.ShowToast("正在取消动画导入");
			}
		};
		AddChild(_animationImportProgressDialog, forceReadableName: false, InternalMode.Disabled);
		return _animationImportProgressDialog;
	}

	private void InstantiateAndSaveResource(string typeName, string baseDir)
	{
		string baseName = typeName + ".tres";
		XWFileSystem singleton = XWFileSystem.GetSingleton();
		baseName = singleton.GenerateUniqueFileName(baseDir, baseName);
		string text = baseDir + baseName;
		string text2 = "[gd_resource type=\"" + typeName + "\" format=3]\n\n[resource]\n";
		Godot.FileAccess fileAccess = Godot.FileAccess.Open(text, Godot.FileAccess.ModeFlags.Write);
		if (fileAccess == null)
		{
			XWEditorInterface.Instance?.ShowToast("无法创建资源: " + text);
			return;
		}
		fileAccess.StoreString(text2);
		fileAccess.Close();
		singleton.ScanChanges();
		XWEditorInterface.Instance?.ShowToast("已创建: " + text);
	}

	private void AddFavoriteOp(List<string> paths)
	{
		XWFileSystem singleton = XWFileSystem.GetSingleton();
		List<string> list;
		if (paths.Count > 0)
		{
			list = paths;
		}
		else
		{
			list = (string.IsNullOrEmpty(_rmbSelectedPath) ? new List<string>() : new List<string> { _rmbSelectedPath });
		}
		foreach (string item in list)
		{
			if (!string.IsNullOrEmpty(item))
			{
				singleton.AddFavorite(item);
			}
		}
		if (_tree.FavoritesItem != null)
		{
			_tree.FavoritesItem.SetCollapsed(enable: false);
		}
	}

	private void RemoveFavoriteOp(List<string> paths)
	{
		XWFileSystem singleton = XWFileSystem.GetSingleton();
		List<string> list;
		if (paths.Count > 0)
		{
			list = paths;
		}
		else
		{
			list = (string.IsNullOrEmpty(_rmbSelectedPath) ? new List<string>() : new List<string> { _rmbSelectedPath });
		}
		foreach (string item in list)
		{
			if (!string.IsNullOrEmpty(item))
			{
				singleton.RemoveFavorite(item);
			}
		}
	}

	private void DeleteSelected(List<string> paths)
	{
		if (paths.Count == 0 && !string.IsNullOrEmpty(_rmbSelectedPath))
		{
			paths = new List<string> { _rmbSelectedPath };
		}
		if (paths.Count != 0)
		{
			if (IsProtectedDirectory(paths[0]))
			{
				XWEditorInterface.Instance?.ShowToast("固定目录不能删除。");
			}
			else
			{
				_removeDialog.PopupFor(paths[0]);
			}
		}
	}

	private void DuplicateSelected()
	{
		List<string> list = GetSelectedPaths();
		if (list.Count == 0 && !string.IsNullOrEmpty(_rmbSelectedPath))
		{
			list = new List<string> { _rmbSelectedPath };
		}
		foreach (string item in list)
		{
			if (string.IsNullOrEmpty(item))
			{
				continue;
			}
			string text = item.GetBaseDir() + "/";
			string file = item.GetFile();
			XWFileSystem singleton = XWFileSystem.GetSingleton();
			string text2 = text + singleton.GenerateUniqueFileName(text, file, file);
			if (Godot.FileAccess.FileExists(item))
			{
				if (DirAccess.CopyAbsolute(item, text2) == Error.Ok)
				{
					singleton.ScanChanges();
				}
			}
			else if (DirAccess.DirExistsAbsolute(item))
			{
				singleton.CopyDirectory(item.EndsWith("/") ? item : (item + "/"), text2.EndsWith("/") ? text2 : (text2 + "/"));
			}
		}
	}

	private void CopyResource(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			_clipboardPath = path;
			_clipboardIsCopy = true;
		}
	}

	private void PasteResource()
	{
		if (string.IsNullOrEmpty(_clipboardPath))
		{
			return;
		}
		string selectedDir = GetSelectedDir();
		if (!DirAccess.DirExistsAbsolute(selectedDir))
		{
			return;
		}
		string file = _clipboardPath.GetFile();
		string text = selectedDir + file;
		XWFileSystem singleton = XWFileSystem.GetSingleton();
		if (_clipboardPath == text)
		{
			text = selectedDir + singleton.GenerateUniqueFileName(selectedDir, file);
		}
		if (Godot.FileAccess.FileExists(_clipboardPath))
		{
			if (DirAccess.CopyAbsolute(_clipboardPath, text) == Error.Ok)
			{
				singleton.ScanChanges();
			}
		}
		else if (DirAccess.DirExistsAbsolute(_clipboardPath))
		{
			singleton.CopyDirectory(_clipboardPath, text);
		}
	}

	private void MoveTo()
	{
		List<string> list = GetSelectedPaths();
		if (list.Count == 0 && !string.IsNullOrEmpty(_rmbSelectedPath))
		{
			list = new List<string> { _rmbSelectedPath };
		}
		if (list.Count != 0)
		{
			_moveSourcePath = list[0];
			string selectedDir = GetSelectedDir();
			_moveToDirectoryDialog.Title = "移动 \"" + _moveSourcePath.GetFile() + "\" 到...";
			_moveToDirectoryDialog.CurrentDir = selectedDir;
			_moveToDirectoryDialog.PopupCentered();
		}
	}

	private void OnMoveTargetDirectorySelected(string targetDir)
	{
		string moveSourcePath = _moveSourcePath;
		_moveSourcePath = "";
		if (string.IsNullOrWhiteSpace(moveSourcePath))
		{
			return;
		}
		string text = targetDir.TrimSuffix("/") + "/" + moveSourcePath.GetFile();
		if (!(moveSourcePath == text))
		{
			Error error = XWFileSystem.GetSingleton().MoveFile(moveSourcePath, text);
			if (error == Error.Ok)
			{
				XWFileSystem.GetSingleton().ScanChanges();
				XWEditorInterface.Instance?.ShowToast("已移动到: " + text);
			}
			else
			{
				XWEditorInterface.Instance?.ShowToast($"移动失败: {error}");
			}
		}
	}

	private void InstantiateScene(string path)
	{
		if (!ResourceLoader.Exists(path))
		{
			return;
		}
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, null, ResourceLoader.CacheMode.Reuse);
		if (packedScene == null)
		{
			return;
		}
		Node editedSceneRoot = GetTree().EditedSceneRoot;
		if (editedSceneRoot != null)
		{
			Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			if (node != null)
			{
				editedSceneRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
				node.Owner = editedSceneRoot;
			}
		}
	}

	private static string ReadResourceUid(string resPath)
	{
		string path = resPath + ".uid";
		if (!Godot.FileAccess.FileExists(path))
		{
			return "";
		}
		Godot.FileAccess fileAccess = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
		if (fileAccess == null)
		{
			return "";
		}
		string text = fileAccess.GetAsText().Trim();
		fileAccess.Close();
		if (!text.StartsWith("uid://"))
		{
			return "";
		}
		return text;
	}

	private string GetListSelectedItemPath()
	{
		int[] selectedItems = _list.GetSelectedItems();
		if (selectedItems.Length == 0)
		{
			return "";
		}
		Variant itemMetadata = _list.GetItemMetadata(selectedItems[0]);
		if (itemMetadata.VariantType == Variant.Type.Nil)
		{
			return "";
		}
		return itemMetadata.As<XWFileSystemTreeItemData>()?.Path ?? "";
	}

	private ConfirmationDialog CreateNameInputDialog(string title, string prompt, string defaultName, out LineEdit nameEdit)
	{
		nameEdit = null;
		if (_nameInputDialogScene == null)
		{
			_nameInputDialogScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/FileSystem/GUI/Dialog/XWNameInputDialog.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		ConfirmationDialog confirmationDialog = _nameInputDialogScene?.Instantiate<ConfirmationDialog>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(confirmationDialog))
		{
			XWEditorInterface.Instance?.ShowToast("无法加载名称输入对话框");
			return null;
		}
		Label node = confirmationDialog.GetNode<Label>("%PromptLabel");
		nameEdit = confirmationDialog.GetNode<LineEdit>("%NameEdit");
		confirmationDialog.Title = title ?? "";
		node.Text = prompt ?? "";
		nameEdit.Text = defaultName ?? "";
		AddChild(confirmationDialog, forceReadableName: false, InternalMode.Disabled);
		return confirmationDialog;
	}

	public void ShowNewCSharpScriptDialog()
	{
		ShowNewScriptDialog(GetSelectedDir(), cSharpOnly: true);
	}

	public void ShowNewLogicDialog()
	{
		ShowNewScriptDialog(GetSelectedDir());
	}

	private void ShowNewScriptDialog(string baseDir, bool cSharpOnly = false)
	{
		baseDir = NormalizeDirectoryPath(baseDir);
		XWTemplateLibrary templateLibrary = new XWTemplateLibrary();
		List<(string TemplateId, string DisplayName)> templateOptions = BuildScriptTemplateOptionsForDirectory(baseDir, templateLibrary);
		string templateId = ((templateOptions.Count > 0) ? templateOptions[0].TemplateId : GetScriptTemplateIdForDirectory(baseDir));
		string displayNameLabel = XWTemplatePresentation.Resolve(templateLibrary.FindTemplate(templateId)).DisplayNameLabel;
		if (_scriptCreateDialogScene == null)
		{
			_scriptCreateDialogScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/FileSystem/GUI/Dialog/XWScriptCreateDialog.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		ConfirmationDialog dialog = _scriptCreateDialogScene?.Instantiate<ConfirmationDialog>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(dialog))
		{
			XWEditorInterface.Instance?.ShowToast("无法加载新建脚本对话框");
			return;
		}
		Label node = dialog.GetNode<Label>("%DirectoryLabel");
		VBoxContainer node2 = dialog.GetNode<VBoxContainer>("%AuthoringModeSection");
		Button cSharpModeCard = dialog.GetNode<Button>("%CSharpModeCard");
		Button blueprintModeCard = dialog.GetNode<Button>("%BlueprintModeCard");
		Label authoringModeHint = dialog.GetNode<Label>("%AuthoringModeHint");
		Label templateTitle = dialog.GetNode<Label>("%TemplateTitle");
		HBoxContainer node3 = dialog.GetNode<HBoxContainer>("%TemplateRow");
		OptionButton templateOption = dialog.GetNode<OptionButton>("%TemplateOption");
		HFlowContainer node4 = dialog.GetNode<HFlowContainer>("%TemplateCards");
		ScrollContainer templateCardScroll = dialog.GetNode<ScrollContainer>("%TemplateCardScroll");
		LineEdit nameEdit = dialog.GetNode<LineEdit>("%NameEdit");
		HBoxContainer previewHeader = dialog.GetNode<HBoxContainer>("%PreviewHeader");
		TextEdit templatePreview = dialog.GetNode<TextEdit>("%TemplatePreview");
		Label hint = dialog.GetNode<Label>("%TemplateHint");
		dialog.Title = (cSharpOnly ? ("新建" + displayNameLabel) : "新建 Mod 逻辑");
		node.Text = "目录：" + baseDir;
		hint.Text = "模板：" + displayNameLabel;
		templateTitle.Text = "选择 C# 模板";
		node3.Visible = templateOptions.Count > 1;
		node3.Hide();
		for (int i = 0; i < templateOptions.Count; i++)
		{
			templateOption.AddItem(templateOptions[i].DisplayName, i);
			Texture2D texture2D = ResourceLoader.Load<Texture2D>(ResolveScriptTemplateIconPath(templateLibrary.FindTemplate(templateOptions[i].TemplateId)), null, ResourceLoader.CacheMode.Reuse);
			if (GodotObject.IsInstanceValid(texture2D))
			{
				templateOption.SetItemIcon(i, texture2D);
			}
		}
		if (templateOptions.Count > 0)
		{
			templateOption.Select(0);
		}
		int selectedTemplateIndex = 0;
		bool blueprintMode = false;
		List<Button> cardButtons = new List<Button>();
		ButtonGroup buttonGroup = new ButtonGroup
		{
			AllowUnpress = false
		};
		ButtonGroup buttonGroup2 = new ButtonGroup
		{
			AllowUnpress = false
		};
		cSharpModeCard.ButtonGroup = buttonGroup2;
		blueprintModeCard.ButtonGroup = buttonGroup2;
		for (int j = 0; j < templateOptions.Count; j++)
		{
			int cardIndex = j;
			XWTemplateLibrary.TemplateInfo template = templateLibrary.FindTemplate(templateOptions[j].TemplateId);
			XWTemplatePresentation xWTemplatePresentation = XWTemplatePresentation.Resolve(template);
			Button button = new Button
			{
				Name = $"ScriptTemplateCard_{j}",
				Text = xWTemplatePresentation.CategoryLabel + "\n" + xWTemplatePresentation.DisplayNameLabel,
				TooltipText = xWTemplatePresentation.PreviewTextLabel,
				ToggleMode = true,
				ButtonGroup = buttonGroup,
				CustomMinimumSize = new Vector2(160f, 72f),
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				Icon = ResourceLoader.Load<Texture2D>(ResolveScriptTemplateIconPath(template), null, ResourceLoader.CacheMode.Reuse),
				ExpandIcon = false
			};
			button.Pressed += () =>
			{
				SelectTemplateCard(cardIndex);
			};
			node4.AddChild(button, forceReadableName: false, InternalMode.Disabled);
			cardButtons.Add(button);
		}
		templateOption.ItemSelected += (long index) =>
		{
			SelectTemplateCard((int)index);
		};
		nameEdit.TextChanged += (string _) =>
		{
			SelectTemplateCard(selectedTemplateIndex);
		};
		cSharpModeCard.Pressed += () =>
		{
			SelectAuthoringMode(useBlueprint: false);
		};
		blueprintModeCard.Pressed += () =>
		{
			SelectAuthoringMode(useBlueprint: true);
		};
		SelectTemplateCard(0);
		node2.Visible = !cSharpOnly;
		blueprintModeCard.Visible = !cSharpOnly;
		SelectAuthoringMode(useBlueprint: false);
		AddChild(dialog, forceReadableName: false, InternalMode.Disabled);
		bool creating = false;
		dialog.Confirmed += DoCreate;
		dialog.Canceled += Cleanup;
		nameEdit.TextSubmitted += (string _) =>
		{
			DoCreate();
		};
		dialog.PopupCenteredClamped(new Vector2I(720, 540), 0.9f);
		nameEdit.GrabFocus();
		void Cleanup()
		{
			dialog.QueueFree();
		}
		void DoCreate()
		{
			if (!creating)
			{
				creating = true;
				if (blueprintMode)
				{
					dialog.Hide();
					Cleanup();
					CreateNewBlueprint(baseDir);
				}
				else
				{
					string text = nameEdit.Text.Trim();
					selectedTemplateIndex = Mathf.Clamp(selectedTemplateIndex, 0, Mathf.Max(0, templateOptions.Count - 1));
					string templateId2 = ((templateOptions.Count > 0) ? templateOptions[selectedTemplateIndex].TemplateId : templateId);
					if (string.IsNullOrWhiteSpace(text))
					{
						text = XWTemplatePresentation.Resolve(templateLibrary.FindTemplate(templateId2)).SuggestedName;
					}
					XWTemplateLibrary.TemplateCreateResult templateCreateResult = templateLibrary.CreateFromTemplateInDirectory(templateId2, baseDir, text);
					if (templateCreateResult.Success)
					{
						RegisterCreatedFilesInManifest(templateCreateResult, "Scripts");
						foreach (string createdPath in templateCreateResult.CreatedPaths)
						{
							if (createdPath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
							{
								XWBPCSharpMemberRegistry.Instance.NotifyScriptSaved(createdPath);
							}
						}
						XWFileSystem.GetSingleton().ScanChanges();
						XWEditorInterface.Instance?.ShowToast("已创建 " + templateCreateResult.CreatedPath);
						CompleteTemplateCreationFromTool(templateCreateResult);
					}
					else
					{
						XWEditorInterface.Instance?.ShowToast("创建失败: " + templateCreateResult.Error);
					}
					Cleanup();
				}
			}
		}
		void SelectAuthoringMode(bool useBlueprint)
		{
			blueprintMode = useBlueprint && !cSharpOnly;
			cSharpModeCard.SetPressedNoSignal(!blueprintMode);
			blueprintModeCard.SetPressedNoSignal(blueprintMode);
			templateTitle.Visible = !blueprintMode;
			templateCardScroll.Visible = !blueprintMode;
			nameEdit.Visible = !blueprintMode;
			previewHeader.Visible = !blueprintMode;
			templatePreview.Visible = !blueprintMode;
			hint.Visible = !blueprintMode;
			dialog.GetOkButton().Text = (blueprintMode ? "下一步：配置蓝图" : "创建 C#");
			authoringModeHint.Text = (blueprintMode ? "蓝图是唯一可视化脚本系统；下一步选择父类和保存位置。" : "C# 是唯一文本脚本语言；选择模板后会直接进入脚本编辑器。");
			if (!blueprintMode && nameEdit.IsInsideTree())
			{
				nameEdit.GrabFocus();
			}
		}
		void SelectTemplateCard(int index)
		{
			if (templateOptions.Count != 0)
			{
				selectedTemplateIndex = Mathf.Clamp(index, 0, templateOptions.Count - 1);
				templateOption.Select(selectedTemplateIndex);
				(string, string) tuple = templateOptions[selectedTemplateIndex];
				XWTemplateLibrary.TemplateInfo templateInfo = templateLibrary.FindTemplate(tuple.Item1);
				XWTemplatePresentation xWTemplatePresentation2 = XWTemplatePresentation.Resolve(templateInfo);
				hint.Text = ((templateInfo == null) ? ("模板：" + tuple.Item2) : $"{xWTemplatePresentation2.CategoryLabel} · {xWTemplatePresentation2.DisplayNameLabel} · {xWTemplatePresentation2.DefaultFolderLabel}");
				nameEdit.PlaceholderText = "例如：" + xWTemplatePresentation2.SuggestedName;
				templatePreview.Text = BuildScriptTemplatePreview(templateInfo, nameEdit.Text);
				for (int k = 0; k < cardButtons.Count; k++)
				{
					cardButtons[k].SetPressedNoSignal(k == selectedTemplateIndex);
				}
			}
		}
	}

	private static string BuildScriptTemplatePreview(XWTemplateLibrary.TemplateInfo template, string resourceName)
	{
		if (template == null)
		{
			return "// 请选择脚本模板";
		}
		string text = XWTemplateLibrary.SanitizeName(string.IsNullOrWhiteSpace(resourceName) ? "NewScript" : resourceName);
		return ((string.IsNullOrWhiteSpace(template.Content) ? template.PreviewText : template.Content) ?? "").Replace("${Name}", text).Replace("${DisplayName}", string.IsNullOrWhiteSpace(resourceName) ? text : resourceName.Trim());
	}

	private static string ResolveScriptTemplateIconPath(XWTemplateLibrary.TemplateInfo template)
	{
		switch (template?.Id)
		{
		case "feature-csharp":
			return "res://addons/ModEditor/Icons/ClassIcon/AnimationTrackGroup.svg";
		case "process-csharp":
			return "res://addons/ModEditor/Icons/ClassIcon/AnimationPlayer.svg";
		case "character-component":
			return "res://addons/ModEditor/Icons/ClassIcon/Node.svg";
		case "projectile-csharp":
			return "res://addons/ModEditor/Icons/ResourceProjectile.svg";
		case "character-csharp":
		case "character-event-csharp":
			return "res://addons/ModEditor/Icons/ResourceCharacter.svg";
		default:
			return "res://addons/ModEditor/Icons/ScriptCreate.svg";
		}
	}

	private void BeginTreeInlineRename(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			if (IsProtectedDirectory(path))
			{
				XWEditorInterface.Instance?.ShowToast("固定目录不能重命名。");
			}
			else if (!_tree.BeginInlineRenamePath(path))
			{
				XWEditorInterface.Instance?.ShowToast("无法在文件树中重命名该项目。");
			}
		}
	}

	private void OnTreeInlineRenameSubmitted(string oldPath, string newName)
	{
		oldPath = oldPath?.Replace('\\', '/') ?? "";
		newName = newName?.Trim() ?? "";
		if (string.IsNullOrWhiteSpace(oldPath))
		{
			return;
		}
		string file = oldPath.TrimEnd('/').GetFile();
		if (string.IsNullOrWhiteSpace(newName) || newName == file)
		{
			return;
		}
		if (newName.Contains('/') || newName.Contains('\\'))
		{
			XWEditorInterface.Instance?.ShowToast("名称不能包含路径分隔符。");
			return;
		}
		if (IsProtectedDirectory(oldPath))
		{
			XWEditorInterface.Instance?.ShowToast("固定目录不能重命名。");
			return;
		}
		bool flag = DirAccess.DirExistsAbsolute(oldPath);
		string text = oldPath.TrimEnd('/').GetBaseDir().PathJoin(newName);
		if (flag)
		{
			text += "/";
		}
		if (Godot.FileAccess.FileExists(text) || DirAccess.DirExistsAbsolute(text))
		{
			XWEditorInterface.Instance?.ShowToast("目标已存在: " + newName);
			return;
		}
		Error error = XWFileSystem.GetSingleton().MoveFile(oldPath, text);
		if (error == Error.Ok)
		{
			XWEditorInterface.Instance?.ShowToast("已重命名: " + file + " → " + newName);
			return;
		}
		XWEditorInterface.Instance?.ShowToast($"重命名失败: {error}");
		XWFileSystem.GetSingleton().ScanChanges();
	}

	private void ShowRenameDialog(string path)
	{
		string oldName = path.GetFile();
		ConfirmationDialog dialog = CreateNameInputDialog("重命名", "原名称: " + oldName, oldName, out var nameEdit);
		if (GodotObject.IsInstanceValid(dialog))
		{
			dialog.Confirmed += DoRename;
			dialog.Canceled += Cleanup;
			nameEdit.TextSubmitted += (string _) =>
			{
				DoRename();
			};
			dialog.PopupCentered();
			nameEdit.GrabFocus();
			nameEdit.SelectAll();
		}
		void Cleanup()
		{
			dialog.QueueFree();
		}
		void DoRename()
		{
			string text = nameEdit.Text.Trim();
			if (string.IsNullOrEmpty(text) || text == oldName)
			{
				Cleanup();
			}
			else
			{
				string to = path.GetBaseDir().PathJoin(text);
				Error error = XWFileSystem.GetSingleton().MoveFile(path, to);
				if (error == Error.Ok)
				{
					XWFileSystem.GetSingleton().ScanChanges();
					XWEditorInterface.Instance?.ShowToast("已重命名: " + oldName + " → " + text);
				}
				else
				{
					XWEditorInterface.Instance?.ShowToast($"重命名失败: {error}");
				}
				Cleanup();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotMethodList()
	{
		return new List<Godot.Bridge.MethodInfo>(117)
		{
			new Godot.Bridge.MethodInfo(MethodName._Ready, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName._ExitTree, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.SetupToolbarIcons, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.SetupSortMenus, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.SetupSortMenu, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MenuButton"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.SetButtonIcon, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "icon", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.SetMenuButtonIcon, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "icon", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.InitFileSystem, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ConnectExternalFileDropSignal, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.DisconnectExternalFileDropSignal, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnExternalFilesDropped, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.PackedStringArray, "files", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetDropTargetDirectoryAtMouse, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnScanStarted, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnScanProgress, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "cur", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "total", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnScanCompleted, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnReload, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnPathSubmitted, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnSearchChanged, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnListSearchChanged, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnFolderActivated, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnSortMenuItemPressed, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ToggleDisplayMode, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.UpdateDisplayMode, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ScheduleSplitClamp, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnSplitDragged, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ToggleFileListDisplayMode, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.UpdateFileListDisplayButton, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.NavigateHistoryPrevious, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.NavigateHistoryNext, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.PushNavigationHistory, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.UpdateNavigationButtons, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.NormalizeDirectoryPath, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnFavoriteActivated, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnListItemSelected, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnTreeSelectionChanged, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnTreeMultiSelected, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "column", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.UpdateSelectedTreeItem, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.NavigateToSelectedTreePath, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.TryPreviewSelectedAnimation, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.UpdatePreview, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.IsImageFile, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnFileActivated, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OpenCompanionResource, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.InspectSubResource, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.NavigateTo, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "addHistory", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.NavigateToProject, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "projectDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.NavigateToPath, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RevealCreatedPath, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OpenCreatedPath, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OpenCreatedPathDeferred, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowCreateDirDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "basePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowRemoveDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ConnectContextMenuSignals, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnTreeItemRmbSelected, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "mouseButtonIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnListItemClicked, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "mouseButtonIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowEmptyTreePopupMenu, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowFavoritePopupMenu, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "favPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowItemPopupMenu, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "data", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "popup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.AddDirectoryCreateItems, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "popup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "directoryPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnToolbarCreateMenuAboutToPopup, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnToolbarCreateMenuIdPressed, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.AddSubResourceCreateItems, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "popup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "ownerPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CanCreateSceneInDirectory, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "directoryPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CanCreateAdvancedResourceInDirectory, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "directoryPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CanCreateBlueprintInDirectory, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "directoryPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CanCreateScriptInDirectory, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "directoryPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetScriptTemplateIdForDirectory, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "directoryPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetLogicCreateMenuTitle, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "directoryPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.IsProtectedDirectory, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ClearCreateActionIds, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.PopupMenuAt, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "menu", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.RegisterCreatedFileInManifest, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "section", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetManifestSectionForTemplate, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "templateId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetManifestSectionForPath, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "defaultSection", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetIcon, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "iconName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnTreeContextMenuIdPressed, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnFileListContextMenuIdPressed, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.HandlePopupMenuId, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "targetDirectoryOverride", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowAdvancedResourceTypeDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "baseDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowAdvancedResourceNameDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "typeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "baseDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetSelectedDir, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.GetContextTargetDir, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.GetDirectoryForPath, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetTreeItemPath, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.IsTreeItemFolder, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("TreeItem"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShellOpenInFileManager, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OpenInTerminal, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "selectedPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateNewScene, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "baseDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateNewBlueprint, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "baseDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnBlueprintCreated, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.CreateNewTextFile, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "baseDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnNewTextFileSelected, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowResourceCreateDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "actionId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "baseDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.FinalizeModernComponentCreation, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Int, "resultId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.FirstDiagnosticLine, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowSubResourceCreateDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "actionId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "ownerPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowAnimationImportDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "baseDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnAnimationImportFileSelected, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnAnimationImportFileDialogCanceled, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.StartAnimationImportAsync, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "sourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "baseDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.HasImportedAnimationFullData, new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.Object, "animation", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetAnimationImportProgressDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AcceptDialog"), exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.InstantiateAndSaveResource, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "typeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "baseDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.DuplicateSelected, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.CopyResource, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.PasteResource, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.MoveTo, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.OnMoveTargetDirectorySelected, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "targetDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.InstantiateScene, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ReadResourceUid, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "resPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.GetListSelectedItemPath, new Godot.Bridge.PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowNewCSharpScriptDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowNewLogicDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowNewScriptDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "baseDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.Bool, "cSharpOnly", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.BeginTreeInlineRename, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.OnTreeInlineRenameSubmitted, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "oldPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "newName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new Godot.Bridge.MethodInfo(MethodName.ShowRenameDialog, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.SetupToolbarIcons && args.Count == 0)
		{
			SetupToolbarIcons();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupSortMenus && args.Count == 0)
		{
			SetupSortMenus();
			ret = default;
			return true;
		}
		if (method == MethodName.SetupSortMenu && args.Count == 1)
		{
			SetupSortMenu(VariantUtils.ConvertTo<MenuButton>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetButtonIcon && args.Count == 2)
		{
			SetButtonIcon(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMenuButtonIcon && args.Count == 2)
		{
			SetMenuButtonIcon(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.InitFileSystem && args.Count == 0)
		{
			InitFileSystem();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectExternalFileDropSignal && args.Count == 0)
		{
			ConnectExternalFileDropSignal();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectExternalFileDropSignal && args.Count == 0)
		{
			DisconnectExternalFileDropSignal();
			ret = default;
			return true;
		}
		if (method == MethodName.OnExternalFilesDropped && args.Count == 1)
		{
			OnExternalFilesDropped(VariantUtils.ConvertTo<string[]>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetDropTargetDirectoryAtMouse && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetDropTargetDirectoryAtMouse());
			return true;
		}
		if (method == MethodName.OnScanStarted && args.Count == 0)
		{
			OnScanStarted();
			ret = default;
			return true;
		}
		if (method == MethodName.OnScanProgress && args.Count == 2)
		{
			OnScanProgress(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnScanCompleted && args.Count == 0)
		{
			OnScanCompleted();
			ret = default;
			return true;
		}
		if (method == MethodName.OnReload && args.Count == 0)
		{
			OnReload();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPathSubmitted && args.Count == 1)
		{
			OnPathSubmitted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSearchChanged && args.Count == 1)
		{
			OnSearchChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnListSearchChanged && args.Count == 1)
		{
			OnListSearchChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnFolderActivated && args.Count == 1)
		{
			OnFolderActivated(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnSortMenuItemPressed && args.Count == 1)
		{
			OnSortMenuItemPressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleDisplayMode && args.Count == 0)
		{
			ToggleDisplayMode();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDisplayMode && args.Count == 0)
		{
			UpdateDisplayMode();
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleSplitClamp && args.Count == 0)
		{
			ScheduleSplitClamp();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSplitDragged && args.Count == 1)
		{
			OnSplitDragged(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleFileListDisplayMode && args.Count == 0)
		{
			ToggleFileListDisplayMode();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateFileListDisplayButton && args.Count == 0)
		{
			UpdateFileListDisplayButton();
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateHistoryPrevious && args.Count == 0)
		{
			NavigateHistoryPrevious();
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateHistoryNext && args.Count == 0)
		{
			NavigateHistoryNext();
			ret = default;
			return true;
		}
		if (method == MethodName.PushNavigationHistory && args.Count == 1)
		{
			PushNavigationHistory(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateNavigationButtons && args.Count == 0)
		{
			UpdateNavigationButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeDirectoryPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeDirectoryPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnFavoriteActivated && args.Count == 1)
		{
			OnFavoriteActivated(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnListItemSelected && args.Count == 1)
		{
			OnListItemSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTreeSelectionChanged && args.Count == 0)
		{
			OnTreeSelectionChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTreeMultiSelected && args.Count == 3)
		{
			OnTreeMultiSelected(VariantUtils.ConvertTo<TreeItem>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSelectedTreeItem && args.Count == 1)
		{
			UpdateSelectedTreeItem(VariantUtils.ConvertTo<TreeItem>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateToSelectedTreePath && args.Count == 1)
		{
			NavigateToSelectedTreePath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryPreviewSelectedAnimation && args.Count == 1)
		{
			TryPreviewSelectedAnimation(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreview && args.Count == 1)
		{
			UpdatePreview(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsImageFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsImageFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnFileActivated && args.Count == 1)
		{
			OnFileActivated(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenCompanionResource && args.Count == 1)
		{
			OpenCompanionResource(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InspectSubResource && args.Count == 1)
		{
			InspectSubResource(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateTo && args.Count == 2)
		{
			NavigateTo(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateToProject && args.Count == 1)
		{
			NavigateToProject(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateToPath && args.Count == 1)
		{
			NavigateToPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RevealCreatedPath && args.Count == 1)
		{
			RevealCreatedPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenCreatedPath && args.Count == 1)
		{
			OpenCreatedPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenCreatedPathDeferred && args.Count == 1)
		{
			OpenCreatedPathDeferred(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCreateDirDialog && args.Count == 1)
		{
			ShowCreateDirDialog(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowRemoveDialog && args.Count == 1)
		{
			ShowRemoveDialog(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectContextMenuSignals && args.Count == 0)
		{
			ConnectContextMenuSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.OnTreeItemRmbSelected && args.Count == 2)
		{
			OnTreeItemRmbSelected(VariantUtils.ConvertTo<Vector2>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnListItemClicked && args.Count == 3)
		{
			OnListItemClicked(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<long>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowEmptyTreePopupMenu && args.Count == 0)
		{
			ShowEmptyTreePopupMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowFavoritePopupMenu && args.Count == 1)
		{
			ShowFavoritePopupMenu(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowItemPopupMenu && args.Count == 2)
		{
			ShowItemPopupMenu(VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in args[0]), VariantUtils.ConvertTo<PopupMenu>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddDirectoryCreateItems && args.Count == 2)
		{
			AddDirectoryCreateItems(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnToolbarCreateMenuAboutToPopup && args.Count == 0)
		{
			OnToolbarCreateMenuAboutToPopup();
			ret = default;
			return true;
		}
		if (method == MethodName.OnToolbarCreateMenuIdPressed && args.Count == 1)
		{
			OnToolbarCreateMenuIdPressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddSubResourceCreateItems && args.Count == 2)
		{
			AddSubResourceCreateItems(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CanCreateSceneInDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCreateSceneInDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanCreateAdvancedResourceInDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCreateAdvancedResourceInDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanCreateBlueprintInDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCreateBlueprintInDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanCreateScriptInDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCreateScriptInDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetScriptTemplateIdForDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetScriptTemplateIdForDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLogicCreateMenuTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetLogicCreateMenuTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsProtectedDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProtectedDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearCreateActionIds && args.Count == 0)
		{
			ClearCreateActionIds();
			ret = default;
			return true;
		}
		if (method == MethodName.PopupMenuAt && args.Count == 1)
		{
			PopupMenuAt(VariantUtils.ConvertTo<PopupMenu>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterCreatedFileInManifest && args.Count == 2)
		{
			RegisterCreatedFileInManifest(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetManifestSectionForTemplate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetManifestSectionForTemplate(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetManifestSectionForPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetManifestSectionForPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnTreeContextMenuIdPressed && args.Count == 1)
		{
			OnTreeContextMenuIdPressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnFileListContextMenuIdPressed && args.Count == 1)
		{
			OnFileListContextMenuIdPressed(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandlePopupMenuId && args.Count == 2)
		{
			HandlePopupMenuId(VariantUtils.ConvertTo<long>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowAdvancedResourceTypeDialog && args.Count == 1)
		{
			ShowAdvancedResourceTypeDialog(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowAdvancedResourceNameDialog && args.Count == 2)
		{
			ShowAdvancedResourceNameDialog(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetSelectedDir && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetSelectedDir());
			return true;
		}
		if (method == MethodName.GetContextTargetDir && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetContextTargetDir());
			return true;
		}
		if (method == MethodName.GetDirectoryForPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetDirectoryForPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTreeItemPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetTreeItemPath(VariantUtils.ConvertTo<TreeItem>(in args[0])));
			return true;
		}
		if (method == MethodName.IsTreeItemFolder && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTreeItemFolder(VariantUtils.ConvertTo<TreeItem>(in args[0])));
			return true;
		}
		if (method == MethodName.ShellOpenInFileManager && args.Count == 1)
		{
			ShellOpenInFileManager(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenInTerminal && args.Count == 1)
		{
			OpenInTerminal(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateNewScene && args.Count == 1)
		{
			CreateNewScene(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateNewBlueprint && args.Count == 1)
		{
			CreateNewBlueprint(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnBlueprintCreated && args.Count == 1)
		{
			OnBlueprintCreated(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateNewTextFile && args.Count == 1)
		{
			CreateNewTextFile(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnNewTextFileSelected && args.Count == 1)
		{
			OnNewTextFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowResourceCreateDialog && args.Count == 2)
		{
			ShowResourceCreateDialog(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinalizeModernComponentCreation && args.Count == 1)
		{
			FinalizeModernComponentCreation(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FirstDiagnosticLine && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FirstDiagnosticLine(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ShowSubResourceCreateDialog && args.Count == 2)
		{
			ShowSubResourceCreateDialog(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowAnimationImportDialog && args.Count == 1)
		{
			ShowAnimationImportDialog(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationImportFileSelected && args.Count == 1)
		{
			OnAnimationImportFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAnimationImportFileDialogCanceled && args.Count == 0)
		{
			OnAnimationImportFileDialogCanceled();
			ret = default;
			return true;
		}
		if (method == MethodName.StartAnimationImportAsync && args.Count == 2)
		{
			StartAnimationImportAsync(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasImportedAnimationFullData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasImportedAnimationFullData(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.GetAnimationImportProgressDialog && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWProgressDialog>(GetAnimationImportProgressDialog());
			return true;
		}
		if (method == MethodName.InstantiateAndSaveResource && args.Count == 2)
		{
			InstantiateAndSaveResource(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DuplicateSelected && args.Count == 0)
		{
			DuplicateSelected();
			ret = default;
			return true;
		}
		if (method == MethodName.CopyResource && args.Count == 1)
		{
			CopyResource(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PasteResource && args.Count == 0)
		{
			PasteResource();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveTo && args.Count == 0)
		{
			MoveTo();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMoveTargetDirectorySelected && args.Count == 1)
		{
			OnMoveTargetDirectorySelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InstantiateScene && args.Count == 1)
		{
			InstantiateScene(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadResourceUid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadResourceUid(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetListSelectedItemPath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetListSelectedItemPath());
			return true;
		}
		if (method == MethodName.ShowNewCSharpScriptDialog && args.Count == 0)
		{
			ShowNewCSharpScriptDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowNewLogicDialog && args.Count == 0)
		{
			ShowNewLogicDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowNewScriptDialog && args.Count == 2)
		{
			ShowNewScriptDialog(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginTreeInlineRename && args.Count == 1)
		{
			BeginTreeInlineRename(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnTreeInlineRenameSubmitted && args.Count == 2)
		{
			OnTreeInlineRenameSubmitted(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowRenameDialog && args.Count == 1)
		{
			ShowRenameDialog(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.NormalizeDirectoryPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeDirectoryPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsImageFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsImageFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanCreateSceneInDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCreateSceneInDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanCreateAdvancedResourceInDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCreateAdvancedResourceInDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanCreateBlueprintInDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCreateBlueprintInDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CanCreateScriptInDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(CanCreateScriptInDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetScriptTemplateIdForDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetScriptTemplateIdForDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetLogicCreateMenuTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetLogicCreateMenuTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsProtectedDirectory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsProtectedDirectory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterCreatedFileInManifest && args.Count == 2)
		{
			RegisterCreatedFileInManifest(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetManifestSectionForTemplate && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetManifestSectionForTemplate(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetManifestSectionForPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetManifestSectionForPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetDirectoryForPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetDirectoryForPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTreeItemPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetTreeItemPath(VariantUtils.ConvertTo<TreeItem>(in args[0])));
			return true;
		}
		if (method == MethodName.IsTreeItemFolder && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTreeItemFolder(VariantUtils.ConvertTo<TreeItem>(in args[0])));
			return true;
		}
		if (method == MethodName.FirstDiagnosticLine && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FirstDiagnosticLine(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.HasImportedAnimationFullData && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasImportedAnimationFullData(VariantUtils.ConvertTo<AdobeAnimateData>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadResourceUid && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadResourceUid(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.SetupToolbarIcons)
		{
			return true;
		}
		if (method == MethodName.SetupSortMenus)
		{
			return true;
		}
		if (method == MethodName.SetupSortMenu)
		{
			return true;
		}
		if (method == MethodName.SetButtonIcon)
		{
			return true;
		}
		if (method == MethodName.SetMenuButtonIcon)
		{
			return true;
		}
		if (method == MethodName.InitFileSystem)
		{
			return true;
		}
		if (method == MethodName.ConnectExternalFileDropSignal)
		{
			return true;
		}
		if (method == MethodName.DisconnectExternalFileDropSignal)
		{
			return true;
		}
		if (method == MethodName.OnExternalFilesDropped)
		{
			return true;
		}
		if (method == MethodName.GetDropTargetDirectoryAtMouse)
		{
			return true;
		}
		if (method == MethodName.OnScanStarted)
		{
			return true;
		}
		if (method == MethodName.OnScanProgress)
		{
			return true;
		}
		if (method == MethodName.OnScanCompleted)
		{
			return true;
		}
		if (method == MethodName.OnReload)
		{
			return true;
		}
		if (method == MethodName.OnPathSubmitted)
		{
			return true;
		}
		if (method == MethodName.OnSearchChanged)
		{
			return true;
		}
		if (method == MethodName.OnListSearchChanged)
		{
			return true;
		}
		if (method == MethodName.OnFolderActivated)
		{
			return true;
		}
		if (method == MethodName.OnSortMenuItemPressed)
		{
			return true;
		}
		if (method == MethodName.ToggleDisplayMode)
		{
			return true;
		}
		if (method == MethodName.UpdateDisplayMode)
		{
			return true;
		}
		if (method == MethodName.ScheduleSplitClamp)
		{
			return true;
		}
		if (method == MethodName.OnSplitDragged)
		{
			return true;
		}
		if (method == MethodName.ToggleFileListDisplayMode)
		{
			return true;
		}
		if (method == MethodName.UpdateFileListDisplayButton)
		{
			return true;
		}
		if (method == MethodName.NavigateHistoryPrevious)
		{
			return true;
		}
		if (method == MethodName.NavigateHistoryNext)
		{
			return true;
		}
		if (method == MethodName.PushNavigationHistory)
		{
			return true;
		}
		if (method == MethodName.UpdateNavigationButtons)
		{
			return true;
		}
		if (method == MethodName.NormalizeDirectoryPath)
		{
			return true;
		}
		if (method == MethodName.OnFavoriteActivated)
		{
			return true;
		}
		if (method == MethodName.OnListItemSelected)
		{
			return true;
		}
		if (method == MethodName.OnTreeSelectionChanged)
		{
			return true;
		}
		if (method == MethodName.OnTreeMultiSelected)
		{
			return true;
		}
		if (method == MethodName.UpdateSelectedTreeItem)
		{
			return true;
		}
		if (method == MethodName.NavigateToSelectedTreePath)
		{
			return true;
		}
		if (method == MethodName.TryPreviewSelectedAnimation)
		{
			return true;
		}
		if (method == MethodName.UpdatePreview)
		{
			return true;
		}
		if (method == MethodName.IsImageFile)
		{
			return true;
		}
		if (method == MethodName.OnFileActivated)
		{
			return true;
		}
		if (method == MethodName.OpenCompanionResource)
		{
			return true;
		}
		if (method == MethodName.InspectSubResource)
		{
			return true;
		}
		if (method == MethodName.NavigateTo)
		{
			return true;
		}
		if (method == MethodName.NavigateToProject)
		{
			return true;
		}
		if (method == MethodName.NavigateToPath)
		{
			return true;
		}
		if (method == MethodName.RevealCreatedPath)
		{
			return true;
		}
		if (method == MethodName.OpenCreatedPath)
		{
			return true;
		}
		if (method == MethodName.OpenCreatedPathDeferred)
		{
			return true;
		}
		if (method == MethodName.ShowCreateDirDialog)
		{
			return true;
		}
		if (method == MethodName.ShowRemoveDialog)
		{
			return true;
		}
		if (method == MethodName.ConnectContextMenuSignals)
		{
			return true;
		}
		if (method == MethodName.OnTreeItemRmbSelected)
		{
			return true;
		}
		if (method == MethodName.OnListItemClicked)
		{
			return true;
		}
		if (method == MethodName.ShowEmptyTreePopupMenu)
		{
			return true;
		}
		if (method == MethodName.ShowFavoritePopupMenu)
		{
			return true;
		}
		if (method == MethodName.ShowItemPopupMenu)
		{
			return true;
		}
		if (method == MethodName.AddDirectoryCreateItems)
		{
			return true;
		}
		if (method == MethodName.OnToolbarCreateMenuAboutToPopup)
		{
			return true;
		}
		if (method == MethodName.OnToolbarCreateMenuIdPressed)
		{
			return true;
		}
		if (method == MethodName.AddSubResourceCreateItems)
		{
			return true;
		}
		if (method == MethodName.CanCreateSceneInDirectory)
		{
			return true;
		}
		if (method == MethodName.CanCreateAdvancedResourceInDirectory)
		{
			return true;
		}
		if (method == MethodName.CanCreateBlueprintInDirectory)
		{
			return true;
		}
		if (method == MethodName.CanCreateScriptInDirectory)
		{
			return true;
		}
		if (method == MethodName.GetScriptTemplateIdForDirectory)
		{
			return true;
		}
		if (method == MethodName.GetLogicCreateMenuTitle)
		{
			return true;
		}
		if (method == MethodName.IsProtectedDirectory)
		{
			return true;
		}
		if (method == MethodName.ClearCreateActionIds)
		{
			return true;
		}
		if (method == MethodName.PopupMenuAt)
		{
			return true;
		}
		if (method == MethodName.RegisterCreatedFileInManifest)
		{
			return true;
		}
		if (method == MethodName.GetManifestSectionForTemplate)
		{
			return true;
		}
		if (method == MethodName.GetManifestSectionForPath)
		{
			return true;
		}
		if (method == MethodName.GetIcon)
		{
			return true;
		}
		if (method == MethodName.OnTreeContextMenuIdPressed)
		{
			return true;
		}
		if (method == MethodName.OnFileListContextMenuIdPressed)
		{
			return true;
		}
		if (method == MethodName.HandlePopupMenuId)
		{
			return true;
		}
		if (method == MethodName.ShowAdvancedResourceTypeDialog)
		{
			return true;
		}
		if (method == MethodName.ShowAdvancedResourceNameDialog)
		{
			return true;
		}
		if (method == MethodName.GetSelectedDir)
		{
			return true;
		}
		if (method == MethodName.GetContextTargetDir)
		{
			return true;
		}
		if (method == MethodName.GetDirectoryForPath)
		{
			return true;
		}
		if (method == MethodName.GetTreeItemPath)
		{
			return true;
		}
		if (method == MethodName.IsTreeItemFolder)
		{
			return true;
		}
		if (method == MethodName.ShellOpenInFileManager)
		{
			return true;
		}
		if (method == MethodName.OpenInTerminal)
		{
			return true;
		}
		if (method == MethodName.CreateNewScene)
		{
			return true;
		}
		if (method == MethodName.CreateNewBlueprint)
		{
			return true;
		}
		if (method == MethodName.OnBlueprintCreated)
		{
			return true;
		}
		if (method == MethodName.CreateNewTextFile)
		{
			return true;
		}
		if (method == MethodName.OnNewTextFileSelected)
		{
			return true;
		}
		if (method == MethodName.ShowResourceCreateDialog)
		{
			return true;
		}
		if (method == MethodName.FinalizeModernComponentCreation)
		{
			return true;
		}
		if (method == MethodName.FirstDiagnosticLine)
		{
			return true;
		}
		if (method == MethodName.ShowSubResourceCreateDialog)
		{
			return true;
		}
		if (method == MethodName.ShowAnimationImportDialog)
		{
			return true;
		}
		if (method == MethodName.OnAnimationImportFileSelected)
		{
			return true;
		}
		if (method == MethodName.OnAnimationImportFileDialogCanceled)
		{
			return true;
		}
		if (method == MethodName.StartAnimationImportAsync)
		{
			return true;
		}
		if (method == MethodName.HasImportedAnimationFullData)
		{
			return true;
		}
		if (method == MethodName.GetAnimationImportProgressDialog)
		{
			return true;
		}
		if (method == MethodName.InstantiateAndSaveResource)
		{
			return true;
		}
		if (method == MethodName.DuplicateSelected)
		{
			return true;
		}
		if (method == MethodName.CopyResource)
		{
			return true;
		}
		if (method == MethodName.PasteResource)
		{
			return true;
		}
		if (method == MethodName.MoveTo)
		{
			return true;
		}
		if (method == MethodName.OnMoveTargetDirectorySelected)
		{
			return true;
		}
		if (method == MethodName.InstantiateScene)
		{
			return true;
		}
		if (method == MethodName.ReadResourceUid)
		{
			return true;
		}
		if (method == MethodName.GetListSelectedItemPath)
		{
			return true;
		}
		if (method == MethodName.ShowNewCSharpScriptDialog)
		{
			return true;
		}
		if (method == MethodName.ShowNewLogicDialog)
		{
			return true;
		}
		if (method == MethodName.ShowNewScriptDialog)
		{
			return true;
		}
		if (method == MethodName.BeginTreeInlineRename)
		{
			return true;
		}
		if (method == MethodName.OnTreeInlineRenameSubmitted)
		{
			return true;
		}
		if (method == MethodName.ShowRenameDialog)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._pathEdit)
		{
			_pathEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._searchEdit)
		{
			_searchEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._listSearchEdit)
		{
			_listSearchEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._histPrevBtn)
		{
			_histPrevBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._histNextBtn)
		{
			_histNextBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._reloadBtn)
		{
			_reloadBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._createButton)
		{
			_createButton = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._toggleDisplayModeBtn)
		{
			_toggleDisplayModeBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._fileListDisplayModeBtn)
		{
			_fileListDisplayModeBtn = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._toolbar2HBox)
		{
			_toolbar2HBox = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._treeSortButton)
		{
			_treeSortButton = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._fileListSortButton)
		{
			_fileListSortButton = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._splitBox)
		{
			_splitBox = VariantUtils.ConvertTo<SplitContainer>(in value);
			return true;
		}
		if (name == PropertyName._fileListVBox)
		{
			_fileListVBox = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._tree)
		{
			_tree = VariantUtils.ConvertTo<XWFileSystemTree>(in value);
			return true;
		}
		if (name == PropertyName._list)
		{
			_list = VariantUtils.ConvertTo<XWFileSystemList>(in value);
			return true;
		}
		if (name == PropertyName._scanningLabel)
		{
			_scanningLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._scanningProgress)
		{
			_scanningProgress = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._scanningVBox)
		{
			_scanningVBox = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._previewTextureRect)
		{
			_previewTextureRect = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._previewFileNameLabel)
		{
			_previewFileNameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewTextureBackground)
		{
			_previewTextureBackground = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._animationFilePreview)
		{
			_animationFilePreview = VariantUtils.ConvertTo<AdobeAnimateInspectorPreview>(in value);
			return true;
		}
		if (name == PropertyName._treePopupMenu)
		{
			_treePopupMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._fileListPopupMenu)
		{
			_fileListPopupMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._createDirDialog)
		{
			_createDirDialog = VariantUtils.ConvertTo<XWDirectoryCreateDialog>(in value);
			return true;
		}
		if (name == PropertyName._removeDialog)
		{
			_removeDialog = VariantUtils.ConvertTo<XWRemoveConfirmDialog>(in value);
			return true;
		}
		if (name == PropertyName._currentDirPath)
		{
			_currentDirPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._rmbSelectedPath)
		{
			_rmbSelectedPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._rmbSelectedData)
		{
			_rmbSelectedData = VariantUtils.ConvertTo<XWFileSystemTreeItemData>(in value);
			return true;
		}
		if (name == PropertyName._clipboardPath)
		{
			_clipboardPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._clipboardIsCopy)
		{
			_clipboardIsCopy = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._navigationIndex)
		{
			_navigationIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._displayMode)
		{
			_displayMode = VariantUtils.ConvertTo<XWFileSystemEnum.DisplayMode>(in value);
			return true;
		}
		if (name == PropertyName._fileListDisplayMode)
		{
			_fileListDisplayMode = VariantUtils.ConvertTo<XWFileSystemEnum.FileListDisplayMode>(in value);
			return true;
		}
		if (name == PropertyName._splitBoxOffsetV)
		{
			_splitBoxOffsetV = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._splitBoxOffsetH)
		{
			_splitBoxOffsetH = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._splitDraggedConnected)
		{
			_splitDraggedConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fileSystemSignalsConnected)
		{
			_fileSystemSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._externalDropConnected)
		{
			_externalDropConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._externalDropWindow)
		{
			_externalDropWindow = VariantUtils.ConvertTo<Window>(in value);
			return true;
		}
		if (name == PropertyName._animationImportInProgress)
		{
			_animationImportInProgress = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._animationImportProgressDialog)
		{
			_animationImportProgressDialog = VariantUtils.ConvertTo<XWProgressDialog>(in value);
			return true;
		}
		if (name == PropertyName._animationImportFileDialog)
		{
			_animationImportFileDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._newTextFileDialog)
		{
			_newTextFileDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._moveToDirectoryDialog)
		{
			_moveToDirectoryDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._moveSourcePath)
		{
			_moveSourcePath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._animationImportTargetDir)
		{
			_animationImportTargetDir = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._splitClampPending)
		{
			_splitClampPending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._toolbarCreateTargetDir)
		{
			_toolbarCreateTargetDir = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._nextComponentAssemblyResultId)
		{
			_nextComponentAssemblyResultId = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._animationInspectRequestVersion)
		{
			_animationInspectRequestVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._pathEdit)
		{
			value = VariantUtils.CreateFrom(in _pathEdit);
			return true;
		}
		if (name == PropertyName._searchEdit)
		{
			value = VariantUtils.CreateFrom(in _searchEdit);
			return true;
		}
		if (name == PropertyName._listSearchEdit)
		{
			value = VariantUtils.CreateFrom(in _listSearchEdit);
			return true;
		}
		if (name == PropertyName._histPrevBtn)
		{
			value = VariantUtils.CreateFrom(in _histPrevBtn);
			return true;
		}
		if (name == PropertyName._histNextBtn)
		{
			value = VariantUtils.CreateFrom(in _histNextBtn);
			return true;
		}
		if (name == PropertyName._reloadBtn)
		{
			value = VariantUtils.CreateFrom(in _reloadBtn);
			return true;
		}
		if (name == PropertyName._createButton)
		{
			value = VariantUtils.CreateFrom(in _createButton);
			return true;
		}
		if (name == PropertyName._toggleDisplayModeBtn)
		{
			value = VariantUtils.CreateFrom(in _toggleDisplayModeBtn);
			return true;
		}
		if (name == PropertyName._fileListDisplayModeBtn)
		{
			value = VariantUtils.CreateFrom(in _fileListDisplayModeBtn);
			return true;
		}
		if (name == PropertyName._toolbar2HBox)
		{
			value = VariantUtils.CreateFrom(in _toolbar2HBox);
			return true;
		}
		if (name == PropertyName._treeSortButton)
		{
			value = VariantUtils.CreateFrom(in _treeSortButton);
			return true;
		}
		if (name == PropertyName._fileListSortButton)
		{
			value = VariantUtils.CreateFrom(in _fileListSortButton);
			return true;
		}
		if (name == PropertyName._splitBox)
		{
			value = VariantUtils.CreateFrom(in _splitBox);
			return true;
		}
		if (name == PropertyName._fileListVBox)
		{
			value = VariantUtils.CreateFrom(in _fileListVBox);
			return true;
		}
		if (name == PropertyName._tree)
		{
			value = VariantUtils.CreateFrom(in _tree);
			return true;
		}
		if (name == PropertyName._list)
		{
			value = VariantUtils.CreateFrom(in _list);
			return true;
		}
		if (name == PropertyName._scanningLabel)
		{
			value = VariantUtils.CreateFrom(in _scanningLabel);
			return true;
		}
		if (name == PropertyName._scanningProgress)
		{
			value = VariantUtils.CreateFrom(in _scanningProgress);
			return true;
		}
		if (name == PropertyName._scanningVBox)
		{
			value = VariantUtils.CreateFrom(in _scanningVBox);
			return true;
		}
		if (name == PropertyName._previewTextureRect)
		{
			value = VariantUtils.CreateFrom(in _previewTextureRect);
			return true;
		}
		if (name == PropertyName._previewFileNameLabel)
		{
			value = VariantUtils.CreateFrom(in _previewFileNameLabel);
			return true;
		}
		if (name == PropertyName._previewTextureBackground)
		{
			value = VariantUtils.CreateFrom(in _previewTextureBackground);
			return true;
		}
		if (name == PropertyName._animationFilePreview)
		{
			value = VariantUtils.CreateFrom(in _animationFilePreview);
			return true;
		}
		if (name == PropertyName._treePopupMenu)
		{
			value = VariantUtils.CreateFrom(in _treePopupMenu);
			return true;
		}
		if (name == PropertyName._fileListPopupMenu)
		{
			value = VariantUtils.CreateFrom(in _fileListPopupMenu);
			return true;
		}
		if (name == PropertyName._createDirDialog)
		{
			value = VariantUtils.CreateFrom(in _createDirDialog);
			return true;
		}
		if (name == PropertyName._removeDialog)
		{
			value = VariantUtils.CreateFrom(in _removeDialog);
			return true;
		}
		if (name == PropertyName._currentDirPath)
		{
			value = VariantUtils.CreateFrom(in _currentDirPath);
			return true;
		}
		if (name == PropertyName._rmbSelectedPath)
		{
			value = VariantUtils.CreateFrom(in _rmbSelectedPath);
			return true;
		}
		if (name == PropertyName._rmbSelectedData)
		{
			value = VariantUtils.CreateFrom(in _rmbSelectedData);
			return true;
		}
		if (name == PropertyName._clipboardPath)
		{
			value = VariantUtils.CreateFrom(in _clipboardPath);
			return true;
		}
		if (name == PropertyName._clipboardIsCopy)
		{
			value = VariantUtils.CreateFrom(in _clipboardIsCopy);
			return true;
		}
		if (name == PropertyName._navigationIndex)
		{
			value = VariantUtils.CreateFrom(in _navigationIndex);
			return true;
		}
		if (name == PropertyName._displayMode)
		{
			value = VariantUtils.CreateFrom(in _displayMode);
			return true;
		}
		if (name == PropertyName._fileListDisplayMode)
		{
			value = VariantUtils.CreateFrom(in _fileListDisplayMode);
			return true;
		}
		if (name == PropertyName._splitBoxOffsetV)
		{
			value = VariantUtils.CreateFrom(in _splitBoxOffsetV);
			return true;
		}
		if (name == PropertyName._splitBoxOffsetH)
		{
			value = VariantUtils.CreateFrom(in _splitBoxOffsetH);
			return true;
		}
		if (name == PropertyName._splitDraggedConnected)
		{
			value = VariantUtils.CreateFrom(in _splitDraggedConnected);
			return true;
		}
		if (name == PropertyName._fileSystemSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _fileSystemSignalsConnected);
			return true;
		}
		if (name == PropertyName._externalDropConnected)
		{
			value = VariantUtils.CreateFrom(in _externalDropConnected);
			return true;
		}
		if (name == PropertyName._externalDropWindow)
		{
			value = VariantUtils.CreateFrom(in _externalDropWindow);
			return true;
		}
		if (name == PropertyName._animationImportInProgress)
		{
			value = VariantUtils.CreateFrom(in _animationImportInProgress);
			return true;
		}
		if (name == PropertyName._animationImportProgressDialog)
		{
			value = VariantUtils.CreateFrom(in _animationImportProgressDialog);
			return true;
		}
		if (name == PropertyName._animationImportFileDialog)
		{
			value = VariantUtils.CreateFrom(in _animationImportFileDialog);
			return true;
		}
		if (name == PropertyName._newTextFileDialog)
		{
			value = VariantUtils.CreateFrom(in _newTextFileDialog);
			return true;
		}
		if (name == PropertyName._moveToDirectoryDialog)
		{
			value = VariantUtils.CreateFrom(in _moveToDirectoryDialog);
			return true;
		}
		if (name == PropertyName._moveSourcePath)
		{
			value = VariantUtils.CreateFrom(in _moveSourcePath);
			return true;
		}
		if (name == PropertyName._animationImportTargetDir)
		{
			value = VariantUtils.CreateFrom(in _animationImportTargetDir);
			return true;
		}
		if (name == PropertyName._splitClampPending)
		{
			value = VariantUtils.CreateFrom(in _splitClampPending);
			return true;
		}
		if (name == PropertyName._toolbarCreateTargetDir)
		{
			value = VariantUtils.CreateFrom(in _toolbarCreateTargetDir);
			return true;
		}
		if (name == PropertyName._nextComponentAssemblyResultId)
		{
			value = VariantUtils.CreateFrom(in _nextComponentAssemblyResultId);
			return true;
		}
		if (name == PropertyName._animationInspectRequestVersion)
		{
			value = VariantUtils.CreateFrom(in _animationInspectRequestVersion);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.PropertyInfo> GetGodotPropertyList()
	{
		return new List<Godot.Bridge.PropertyInfo>
		{
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._pathEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._searchEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._listSearchEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._histPrevBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._histNextBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._reloadBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._createButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._toggleDisplayModeBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._fileListDisplayModeBtn, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._toolbar2HBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._treeSortButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._fileListSortButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._splitBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._fileListVBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._tree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._list, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._scanningLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._scanningProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._scanningVBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._previewTextureRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._previewFileNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._previewTextureBackground, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._animationFilePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._treePopupMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._fileListPopupMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._createDirDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._removeDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName._currentDirPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName._rmbSelectedPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._rmbSelectedData, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName._clipboardPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._clipboardIsCopy, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._navigationIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._displayMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._fileListDisplayMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._splitBoxOffsetV, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._splitBoxOffsetH, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._splitDraggedConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._fileSystemSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._externalDropConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._externalDropWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._animationImportInProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._animationImportProgressDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._animationImportFileDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._newTextFileDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Object, PropertyName._moveToDirectoryDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName._moveSourcePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName._animationImportTargetDir, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Bool, PropertyName._splitClampPending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.String, PropertyName._toolbarCreateTargetDir, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._nextComponentAssemblyResultId, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new Godot.Bridge.PropertyInfo(Variant.Type.Int, PropertyName._animationInspectRequestVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._pathEdit, Variant.From(in _pathEdit));
		info.AddProperty(PropertyName._searchEdit, Variant.From(in _searchEdit));
		info.AddProperty(PropertyName._listSearchEdit, Variant.From(in _listSearchEdit));
		info.AddProperty(PropertyName._histPrevBtn, Variant.From(in _histPrevBtn));
		info.AddProperty(PropertyName._histNextBtn, Variant.From(in _histNextBtn));
		info.AddProperty(PropertyName._reloadBtn, Variant.From(in _reloadBtn));
		info.AddProperty(PropertyName._createButton, Variant.From(in _createButton));
		info.AddProperty(PropertyName._toggleDisplayModeBtn, Variant.From(in _toggleDisplayModeBtn));
		info.AddProperty(PropertyName._fileListDisplayModeBtn, Variant.From(in _fileListDisplayModeBtn));
		info.AddProperty(PropertyName._toolbar2HBox, Variant.From(in _toolbar2HBox));
		info.AddProperty(PropertyName._treeSortButton, Variant.From(in _treeSortButton));
		info.AddProperty(PropertyName._fileListSortButton, Variant.From(in _fileListSortButton));
		info.AddProperty(PropertyName._splitBox, Variant.From(in _splitBox));
		info.AddProperty(PropertyName._fileListVBox, Variant.From(in _fileListVBox));
		info.AddProperty(PropertyName._tree, Variant.From(in _tree));
		info.AddProperty(PropertyName._list, Variant.From(in _list));
		info.AddProperty(PropertyName._scanningLabel, Variant.From(in _scanningLabel));
		info.AddProperty(PropertyName._scanningProgress, Variant.From(in _scanningProgress));
		info.AddProperty(PropertyName._scanningVBox, Variant.From(in _scanningVBox));
		info.AddProperty(PropertyName._previewTextureRect, Variant.From(in _previewTextureRect));
		info.AddProperty(PropertyName._previewFileNameLabel, Variant.From(in _previewFileNameLabel));
		info.AddProperty(PropertyName._previewTextureBackground, Variant.From(in _previewTextureBackground));
		info.AddProperty(PropertyName._animationFilePreview, Variant.From(in _animationFilePreview));
		info.AddProperty(PropertyName._treePopupMenu, Variant.From(in _treePopupMenu));
		info.AddProperty(PropertyName._fileListPopupMenu, Variant.From(in _fileListPopupMenu));
		info.AddProperty(PropertyName._createDirDialog, Variant.From(in _createDirDialog));
		info.AddProperty(PropertyName._removeDialog, Variant.From(in _removeDialog));
		info.AddProperty(PropertyName._currentDirPath, Variant.From(in _currentDirPath));
		info.AddProperty(PropertyName._rmbSelectedPath, Variant.From(in _rmbSelectedPath));
		info.AddProperty(PropertyName._rmbSelectedData, Variant.From(in _rmbSelectedData));
		info.AddProperty(PropertyName._clipboardPath, Variant.From(in _clipboardPath));
		info.AddProperty(PropertyName._clipboardIsCopy, Variant.From(in _clipboardIsCopy));
		info.AddProperty(PropertyName._navigationIndex, Variant.From(in _navigationIndex));
		info.AddProperty(PropertyName._displayMode, Variant.From(in _displayMode));
		info.AddProperty(PropertyName._fileListDisplayMode, Variant.From(in _fileListDisplayMode));
		info.AddProperty(PropertyName._splitBoxOffsetV, Variant.From(in _splitBoxOffsetV));
		info.AddProperty(PropertyName._splitBoxOffsetH, Variant.From(in _splitBoxOffsetH));
		info.AddProperty(PropertyName._splitDraggedConnected, Variant.From(in _splitDraggedConnected));
		info.AddProperty(PropertyName._fileSystemSignalsConnected, Variant.From(in _fileSystemSignalsConnected));
		info.AddProperty(PropertyName._externalDropConnected, Variant.From(in _externalDropConnected));
		info.AddProperty(PropertyName._externalDropWindow, Variant.From(in _externalDropWindow));
		info.AddProperty(PropertyName._animationImportInProgress, Variant.From(in _animationImportInProgress));
		info.AddProperty(PropertyName._animationImportProgressDialog, Variant.From(in _animationImportProgressDialog));
		info.AddProperty(PropertyName._animationImportFileDialog, Variant.From(in _animationImportFileDialog));
		info.AddProperty(PropertyName._newTextFileDialog, Variant.From(in _newTextFileDialog));
		info.AddProperty(PropertyName._moveToDirectoryDialog, Variant.From(in _moveToDirectoryDialog));
		info.AddProperty(PropertyName._moveSourcePath, Variant.From(in _moveSourcePath));
		info.AddProperty(PropertyName._animationImportTargetDir, Variant.From(in _animationImportTargetDir));
		info.AddProperty(PropertyName._splitClampPending, Variant.From(in _splitClampPending));
		info.AddProperty(PropertyName._toolbarCreateTargetDir, Variant.From(in _toolbarCreateTargetDir));
		info.AddProperty(PropertyName._nextComponentAssemblyResultId, Variant.From(in _nextComponentAssemblyResultId));
		info.AddProperty(PropertyName._animationInspectRequestVersion, Variant.From(in _animationInspectRequestVersion));
		info.AddSignalEventDelegate(SignalName.ImportDockRequested, backing_ImportDockRequested);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._pathEdit, out var value))
		{
			_pathEdit = value.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._searchEdit, out var value2))
		{
			_searchEdit = value2.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._listSearchEdit, out var value3))
		{
			_listSearchEdit = value3.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._histPrevBtn, out var value4))
		{
			_histPrevBtn = value4.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._histNextBtn, out var value5))
		{
			_histNextBtn = value5.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._reloadBtn, out var value6))
		{
			_reloadBtn = value6.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._createButton, out var value7))
		{
			_createButton = value7.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._toggleDisplayModeBtn, out var value8))
		{
			_toggleDisplayModeBtn = value8.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._fileListDisplayModeBtn, out var value9))
		{
			_fileListDisplayModeBtn = value9.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._toolbar2HBox, out var value10))
		{
			_toolbar2HBox = value10.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._treeSortButton, out var value11))
		{
			_treeSortButton = value11.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._fileListSortButton, out var value12))
		{
			_fileListSortButton = value12.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._splitBox, out var value13))
		{
			_splitBox = value13.As<SplitContainer>();
		}
		if (info.TryGetProperty(PropertyName._fileListVBox, out var value14))
		{
			_fileListVBox = value14.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._tree, out var value15))
		{
			_tree = value15.As<XWFileSystemTree>();
		}
		if (info.TryGetProperty(PropertyName._list, out var value16))
		{
			_list = value16.As<XWFileSystemList>();
		}
		if (info.TryGetProperty(PropertyName._scanningLabel, out var value17))
		{
			_scanningLabel = value17.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._scanningProgress, out var value18))
		{
			_scanningProgress = value18.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._scanningVBox, out var value19))
		{
			_scanningVBox = value19.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._previewTextureRect, out var value20))
		{
			_previewTextureRect = value20.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._previewFileNameLabel, out var value21))
		{
			_previewFileNameLabel = value21.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewTextureBackground, out var value22))
		{
			_previewTextureBackground = value22.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._animationFilePreview, out var value23))
		{
			_animationFilePreview = value23.As<AdobeAnimateInspectorPreview>();
		}
		if (info.TryGetProperty(PropertyName._treePopupMenu, out var value24))
		{
			_treePopupMenu = value24.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._fileListPopupMenu, out var value25))
		{
			_fileListPopupMenu = value25.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._createDirDialog, out var value26))
		{
			_createDirDialog = value26.As<XWDirectoryCreateDialog>();
		}
		if (info.TryGetProperty(PropertyName._removeDialog, out var value27))
		{
			_removeDialog = value27.As<XWRemoveConfirmDialog>();
		}
		if (info.TryGetProperty(PropertyName._currentDirPath, out var value28))
		{
			_currentDirPath = value28.As<string>();
		}
		if (info.TryGetProperty(PropertyName._rmbSelectedPath, out var value29))
		{
			_rmbSelectedPath = value29.As<string>();
		}
		if (info.TryGetProperty(PropertyName._rmbSelectedData, out var value30))
		{
			_rmbSelectedData = value30.As<XWFileSystemTreeItemData>();
		}
		if (info.TryGetProperty(PropertyName._clipboardPath, out var value31))
		{
			_clipboardPath = value31.As<string>();
		}
		if (info.TryGetProperty(PropertyName._clipboardIsCopy, out var value32))
		{
			_clipboardIsCopy = value32.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._navigationIndex, out var value33))
		{
			_navigationIndex = value33.As<int>();
		}
		if (info.TryGetProperty(PropertyName._displayMode, out var value34))
		{
			_displayMode = value34.As<XWFileSystemEnum.DisplayMode>();
		}
		if (info.TryGetProperty(PropertyName._fileListDisplayMode, out var value35))
		{
			_fileListDisplayMode = value35.As<XWFileSystemEnum.FileListDisplayMode>();
		}
		if (info.TryGetProperty(PropertyName._splitBoxOffsetV, out var value36))
		{
			_splitBoxOffsetV = value36.As<int>();
		}
		if (info.TryGetProperty(PropertyName._splitBoxOffsetH, out var value37))
		{
			_splitBoxOffsetH = value37.As<int>();
		}
		if (info.TryGetProperty(PropertyName._splitDraggedConnected, out var value38))
		{
			_splitDraggedConnected = value38.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fileSystemSignalsConnected, out var value39))
		{
			_fileSystemSignalsConnected = value39.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._externalDropConnected, out var value40))
		{
			_externalDropConnected = value40.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._externalDropWindow, out var value41))
		{
			_externalDropWindow = value41.As<Window>();
		}
		if (info.TryGetProperty(PropertyName._animationImportInProgress, out var value42))
		{
			_animationImportInProgress = value42.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._animationImportProgressDialog, out var value43))
		{
			_animationImportProgressDialog = value43.As<XWProgressDialog>();
		}
		if (info.TryGetProperty(PropertyName._animationImportFileDialog, out var value44))
		{
			_animationImportFileDialog = value44.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._newTextFileDialog, out var value45))
		{
			_newTextFileDialog = value45.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._moveToDirectoryDialog, out var value46))
		{
			_moveToDirectoryDialog = value46.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._moveSourcePath, out var value47))
		{
			_moveSourcePath = value47.As<string>();
		}
		if (info.TryGetProperty(PropertyName._animationImportTargetDir, out var value48))
		{
			_animationImportTargetDir = value48.As<string>();
		}
		if (info.TryGetProperty(PropertyName._splitClampPending, out var value49))
		{
			_splitClampPending = value49.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._toolbarCreateTargetDir, out var value50))
		{
			_toolbarCreateTargetDir = value50.As<string>();
		}
		if (info.TryGetProperty(PropertyName._nextComponentAssemblyResultId, out var value51))
		{
			_nextComponentAssemblyResultId = value51.As<long>();
		}
		if (info.TryGetProperty(PropertyName._animationInspectRequestVersion, out var value52))
		{
			_animationInspectRequestVersion = value52.As<int>();
		}
		if (info.TryGetSignalEventDelegate<ImportDockRequestedEventHandler>(SignalName.ImportDockRequested, out var value53))
		{
			backing_ImportDockRequested = value53;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<Godot.Bridge.MethodInfo> GetGodotSignalList()
	{
		return new List<Godot.Bridge.MethodInfo>(1)
		{
			new Godot.Bridge.MethodInfo(SignalName.ImportDockRequested, new Godot.Bridge.PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<Godot.Bridge.PropertyInfo>
			{
				new Godot.Bridge.PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	protected void EmitSignalImportDockRequested(string resourcePath)
	{
		EmitSignal(SignalName.ImportDockRequested, new ReadOnlySpan<Variant>((Variant)resourcePath));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.ImportDockRequested && args.Count == 1)
		{
			backing_ImportDockRequested?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.ImportDockRequested)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
