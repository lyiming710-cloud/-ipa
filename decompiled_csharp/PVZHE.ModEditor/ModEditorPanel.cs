using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Blueprint;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.GUI;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.Issues.GUI;
using PVZHE.ModEditor.Layout;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.ModSystem.Validation;
using PVZHE.ModEditor.OutPutPanel;
using PVZHE.ModEditor.PVZIntegration;
using PVZHE.ModEditor.ProjectManager;
using PVZHE.ModEditor.ProjectSettingsGUI;
using PVZHE.ModEditor.Registry;
using PVZHE.ModEditor.ResourceEditors;
using PVZHE.ModEditor.ResourceEditors.GUI;
using PVZHE.ModEditor.RunBar;
using PVZHE.ModEditor.SceneEditor;
using PVZHE.ModEditor.ScriptEditor;
using PVZHE.ModEditor.Tools.GUI;

namespace PVZHE.ModEditor;

[ScriptPath("res://addons/ModEditor/ModEditorPanel.cs")]
public class ModEditorPanel : Control
{
	public new class MethodName : Control.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName ApplyResponsiveWorkspaceLayout = "ApplyResponsiveWorkspaceLayout";

		public static readonly StringName BuildMenus = "BuildMenus";

		public static readonly StringName ShowLoadingScreen = "ShowLoadingScreen";

		public static readonly StringName StartAsyncInit = "StartAsyncInit";

		public static readonly StringName LoadProjectManagerPanel = "LoadProjectManagerPanel";

		public static readonly StringName ShowProjectManager = "ShowProjectManager";

		public static readonly StringName OpenProjectFromManager = "OpenProjectFromManager";

		public static readonly StringName OnMainScreenChanged = "OnMainScreenChanged";

		public static readonly StringName PlayWorkspaceTransition = "PlayWorkspaceTransition";

		public static readonly StringName RegisterBuiltinEditorFactories = "RegisterBuiltinEditorFactories";

		public static readonly StringName LoadResourceEditorPanels = "LoadResourceEditorPanels";

		public static readonly StringName EnsureResourceEditor = "EnsureResourceEditor";

		public static readonly StringName ConnectRunBarSignals = "ConnectRunBarSignals";

		public static readonly StringName OnRunMainPressed = "OnRunMainPressed";

		public static readonly StringName OnRunCurrentScenePressed = "OnRunCurrentScenePressed";

		public static readonly StringName OnRunCustomPressed = "OnRunCustomPressed";

		public static readonly StringName OnRunStopPressed = "OnRunStopPressed";

		public static readonly StringName OnRunPauseToggled = "OnRunPauseToggled";

		public static readonly StringName RunCurrent2DScenePreview = "RunCurrent2DScenePreview";

		public static readonly StringName CreatePackedSceneFromCurrent2D = "CreatePackedSceneFromCurrent2D";

		public static readonly StringName FindProjectMainScenePath = "FindProjectMainScenePath";

		public static readonly StringName ShowRunCustomSceneDialog = "ShowRunCustomSceneDialog";

		public static readonly StringName OnRunCustomSceneSelected = "OnRunCustomSceneSelected";

		public static readonly StringName StartRunPreview = "StartRunPreview";

		public static readonly StringName ShowRunPreviewWindow = "ShowRunPreviewWindow";

		public static readonly StringName StopRunPreview = "StopRunPreview";

		public static readonly StringName CacheRunPreviewProcessModes = "CacheRunPreviewProcessModes";

		public static readonly StringName CacheRunPreviewProcessModesRecursive = "CacheRunPreviewProcessModesRecursive";

		public static readonly StringName SetRunPreviewPaused = "SetRunPreviewPaused";

		public static readonly StringName GetCurrent2DSceneDisplayName = "GetCurrent2DSceneDisplayName";

		public static readonly StringName LogRunMessage = "LogRunMessage";

		public static readonly StringName ReportRunError = "ReportRunError";

		public static readonly StringName OnFileMenuItem = "OnFileMenuItem";

		public static readonly StringName OnEditMenuItem = "OnEditMenuItem";

		public static readonly StringName OnModMenuItem = "OnModMenuItem";

		public new static readonly StringName _UnhandledInput = "_UnhandledInput";

		public new static readonly StringName _Notification = "_Notification";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName UpdateDockOverlayZones = "UpdateDockOverlayZones";

		public static readonly StringName ShowCreateProjectDialog = "ShowCreateProjectDialog";

		public static readonly StringName OnCreateProjectConfirmed = "OnCreateProjectConfirmed";

		public static readonly StringName ShowOpenProjectDialog = "ShowOpenProjectDialog";

		public static readonly StringName OnOpenProjectFileSelected = "OnOpenProjectFileSelected";

		public static readonly StringName SaveActiveDocument = "SaveActiveDocument";

		public static readonly StringName SaveAllDocuments = "SaveAllDocuments";

		public static readonly StringName SaveCurrentProject = "SaveCurrentProject";

		public static readonly StringName ShowProjectSettingsDialog = "ShowProjectSettingsDialog";

		public static readonly StringName OnProjectSettingsSaved = "OnProjectSettingsSaved";

		public static readonly StringName UpdateProjectTitle = "UpdateProjectTitle";

		public static readonly StringName ShowExportDialog = "ShowExportDialog";

		public static readonly StringName OnExportDirSelected = "OnExportDirSelected";

		public static readonly StringName NavigateFileSystemToProject = "NavigateFileSystemToProject";

		public static readonly StringName ShowToast = "ShowToast";

		public static readonly StringName OnEditorActionCommitted = "OnEditorActionCommitted";

		public static readonly StringName OnEditorActionUndone = "OnEditorActionUndone";

		public static readonly StringName OnEditorActionRedone = "OnEditorActionRedone";

		public static readonly StringName LogEditorAction = "LogEditorAction";

		public static readonly StringName LogEditorUndo = "LogEditorUndo";

		public static readonly StringName LogEditorRedo = "LogEditorRedo";

		public static readonly StringName GetOutputPanel = "GetOutputPanel";

		public static readonly StringName GetRunBar = "GetRunBar";

		public static readonly StringName GetModToolsPanel = "GetModToolsPanel";

		public static readonly StringName GetSceneTreeDock = "GetSceneTreeDock";

		public static readonly StringName Get2DSceneEditor = "Get2DSceneEditor";
	}

	public new class PropertyName : Control.PropertyName
	{
		public static readonly StringName LoadedResourceEditorCount = "LoadedResourceEditorCount";

		public static readonly StringName _undoRedoManager = "_undoRedoManager";

		public static readonly StringName _editorData = "_editorData";

		public static readonly StringName _editorSettings = "_editorSettings";

		public static readonly StringName _editorSelection = "_editorSelection";

		public static readonly StringName _selectionHistory = "_selectionHistory";

		public static readonly StringName _layoutManager = "_layoutManager";

		public static readonly StringName _panelRegistry = "_panelRegistry";

		public static readonly StringName _titleBar = "_titleBar";

		public static readonly StringName _workspaceSplit = "_workspaceSplit";

		public static readonly StringName _leftDock = "_leftDock";

		public static readonly StringName _leftBottomDock = "_leftBottomDock";

		public static readonly StringName _leftTopRightDock = "_leftTopRightDock";

		public static readonly StringName _leftBottomRightDock = "_leftBottomRightDock";

		public static readonly StringName _centerDock = "_centerDock";

		public static readonly StringName _centerWorkspaceMotion = "_centerWorkspaceMotion";

		public static readonly StringName _bottomPanel = "_bottomPanel";

		public static readonly StringName _bottomDock = "_bottomDock";

		public static readonly StringName _rightDock = "_rightDock";

		public static readonly StringName _rightBottomDock = "_rightBottomDock";

		public static readonly StringName _rightTopRightDock = "_rightTopRightDock";

		public static readonly StringName _rightBottomRightDock = "_rightBottomRightDock";

		public static readonly StringName _rightPanelTabs = "_rightPanelTabs";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _fileSystemPanel = "_fileSystemPanel";

		public static readonly StringName _sceneTreeDock = "_sceneTreeDock";

		public static readonly StringName _scene2DEditor = "_scene2DEditor";

		public static readonly StringName _inspector = "_inspector";

		public static readonly StringName _bpEditor = "_bpEditor";

		public static readonly StringName _scriptEditor = "_scriptEditor";

		public static readonly StringName _issuePanel = "_issuePanel";

		public static readonly StringName _modToolsPanel = "_modToolsPanel";

		public static readonly StringName _projectManagerPanel = "_projectManagerPanel";

		public static readonly StringName _runBar = "_runBar";

		public static readonly StringName _outputPanel = "_outputPanel";

		public static readonly StringName _toaster = "_toaster";

		public static readonly StringName _runPreviewWindow = "_runPreviewWindow";

		public static readonly StringName _runPreviewViewport = "_runPreviewViewport";

		public static readonly StringName _runPreviewRoot = "_runPreviewRoot";

		public static readonly StringName _runCustomSceneDialog = "_runCustomSceneDialog";

		public static readonly StringName _loadingOverlay = "_loadingOverlay";

		public static readonly StringName _loadingBar = "_loadingBar";

		public static readonly StringName _loadingLabel = "_loadingLabel";

		public static readonly StringName _dockOverlay = "_dockOverlay";

		public static readonly StringName _isDockDragging = "_isDockDragging";

		public static readonly StringName _dragPanelKey = "_dragPanelKey";

		public static readonly StringName _createProjectDialog = "_createProjectDialog";

		public static readonly StringName _projectSettingsDialog = "_projectSettingsDialog";

		public static readonly StringName _openProjectFileDialog = "_openProjectFileDialog";

		public static readonly StringName _exportDirDialog = "_exportDirDialog";

		public static readonly StringName _isExporting = "_isExporting";
	}

	public new class SignalName : Control.SignalName
	{
	}

	private const string CreateProjectDialogScenePath = "res://addons/ModEditor/ModSystem/ModProjectCreateDialog.tscn";

	private const string ExportDirectoryDialogScenePath = "res://addons/ModEditor/GUI/XWExportDirectoryDialog.tscn";

	private const string OpenProjectDialogScenePath = "res://addons/ModEditor/GUI/XWOpenProjectDialog.tscn";

	private const string RunCustomSceneDialogScenePath = "res://addons/ModEditor/GUI/XWRunCustomSceneDialog.tscn";

	private const string RunPreviewWindowScenePath = "res://addons/ModEditor/GUI/XWRunPreviewWindow.tscn";

	private static PackedScene _createProjectDialogScene;

	private static PackedScene _exportDirectoryDialogScene;

	private static PackedScene _openProjectDialogScene;

	private static PackedScene _runCustomSceneDialogScene;

	private static PackedScene _runPreviewWindowScene;

	private XWUndoRedoManager _undoRedoManager;

	private XWEditorData _editorData;

	private XWEditorSettings _editorSettings;

	private XWEditorSelection _editorSelection;

	private XWEditorSelectionHistory _selectionHistory;

	private XWLayoutManager _layoutManager;

	private XWPanelRegistry _panelRegistry;

	private XWEditorTitleBar _titleBar;

	private HSplitContainer _workspaceSplit;

	private TabContainer _leftDock;

	private TabContainer _leftBottomDock;

	private TabContainer _leftTopRightDock;

	private TabContainer _leftBottomRightDock;

	private TabContainer _centerDock;

	private XWUiMotion _centerWorkspaceMotion;

	private XWBottomPanel _bottomPanel;

	private TabContainer _bottomDock;

	private TabContainer _rightDock;

	private TabContainer _rightBottomDock;

	private TabContainer _rightTopRightDock;

	private TabContainer _rightBottomRightDock;

	private TabContainer _rightPanelTabs;

	private Label _statusLabel;

	private XWFileSystemPanel _fileSystemPanel;

	private XWSceneTreeDock _sceneTreeDock;

	private XW2DSceneEditor _scene2DEditor;

	private XWInspector _inspector;

	private XWBPEditor _bpEditor;

	private XWScriptEditor _scriptEditor;

	private readonly Dictionary<string, XWGenericVisualResourceEditor> _resourceEditors = new Dictionary<string, XWGenericVisualResourceEditor>();

	private readonly Dictionary<string, XWVisualEditorDescriptor> _resourceEditorDescriptors = new Dictionary<string, XWVisualEditorDescriptor>();

	private XWIssuePanel _issuePanel;

	private XWModToolsPanel _modToolsPanel;

	private XWModProjectManagerPanel _projectManagerPanel;

	private XWEditorRunBar _runBar;

	private XWOutputPanel _outputPanel;

	private XWEditorToaster _toaster;

	private Window _runPreviewWindow;

	private SubViewport _runPreviewViewport;

	private Node _runPreviewRoot;

	private FileDialog _runCustomSceneDialog;

	private readonly Dictionary<Node, ProcessModeEnum> _runPreviewProcessModes = new Dictionary<Node, ProcessModeEnum>();

	private Control _loadingOverlay;

	private ProgressBar _loadingBar;

	private Label _loadingLabel;

	private XWDockOverlay _dockOverlay;

	private bool _isDockDragging;

	private string _dragPanelKey = "";

	private ModProject _currentProject;

	private ModProjectCreateDialog _createProjectDialog;

	private XWProjectSettingsDialog _projectSettingsDialog;

	private FileDialog _openProjectFileDialog;

	private FileDialog _exportDirDialog;

	private CancellationTokenSource _exportLifetimeCts = new CancellationTokenSource();

	private bool _isExporting;

	public int LoadedResourceEditorCount => _resourceEditors.Count;

	public override void _Ready()
	{
		SetProcess(enable: false);
		ModEditorTheme.ApplyTo(this);
		ColorRect nodeOrNull = GetNodeOrNull<ColorRect>("Background");
		if (nodeOrNull != null)
		{
			nodeOrNull.Color = ModEditorTheme.BackgroundColor;
		}
		_titleBar = GetNode<XWEditorTitleBar>("%TitleBar");
		_workspaceSplit = GetNode<HSplitContainer>("VBoxContainer/HSplitContainer");
		_leftDock = GetNode<TabContainer>("%LeftTopDock");
		_leftBottomDock = GetNodeOrNull<TabContainer>("%LeftBottomDock");
		_leftTopRightDock = GetNodeOrNull<TabContainer>("%LeftTopRightDock");
		_leftBottomRightDock = GetNodeOrNull<TabContainer>("%LeftBottomRightDock");
		_centerDock = GetNode<TabContainer>("%CenterDock");
		_centerWorkspaceMotion = XWUiMotion.BindPanel(_centerDock);
		_bottomPanel = GetNode<XWBottomPanel>("%BottomPanel");
		_bottomDock = _bottomPanel.ContentContainer;
		_rightDock = GetNode<TabContainer>("%RightTopDock");
		_rightBottomDock = GetNodeOrNull<TabContainer>("%RightBottomDock");
		_rightTopRightDock = GetNodeOrNull<TabContainer>("%RightTopRightDock");
		_rightBottomRightDock = GetNodeOrNull<TabContainer>("%RightBottomRightDock");
		_rightPanelTabs = GetNodeOrNull<TabContainer>("%RightPanel");
		if (_rightPanelTabs != null)
		{
			_rightPanelTabs.SetTabTitle(0, "主要");
			_rightPanelTabs.SetTabTitle(1, "扩展");
		}
		_statusLabel = GetNodeOrNull<Label>("%StatusLabel");
		_panelRegistry = new XWPanelRegistry();
		_panelRegistry.RegisterBuiltInPanels();
		_layoutManager = new XWLayoutManager();
		_layoutManager.InitializeExisting(this, _panelRegistry);
		_layoutManager.AdoptContainer(4, _leftDock);
		if (_leftBottomDock != null)
		{
			_layoutManager.AdoptContainer(5, _leftBottomDock);
		}
		if (_leftTopRightDock != null)
		{
			_layoutManager.AdoptContainer(6, _leftTopRightDock);
		}
		if (_leftBottomRightDock != null)
		{
			_layoutManager.AdoptContainer(7, _leftBottomRightDock);
		}
		_layoutManager.AdoptContainer(2, _centerDock);
		_layoutManager.AdoptContainer(8, _rightDock);
		if (_rightBottomDock != null)
		{
			_layoutManager.AdoptContainer(9, _rightBottomDock);
		}
		if (_rightTopRightDock != null)
		{
			_layoutManager.AdoptContainer(10, _rightTopRightDock);
		}
		if (_rightBottomRightDock != null)
		{
			_layoutManager.AdoptContainer(11, _rightBottomRightDock);
		}
		_layoutManager.AdoptContainer(3, _bottomDock);
		Resized += ApplyResponsiveWorkspaceLayout;
		ApplyResponsiveWorkspaceLayout();
		_dockOverlay = new XWDockOverlay
		{
			Name = "DockOverlay"
		};
		AddChild(_dockOverlay, forceReadableName: false, InternalMode.Disabled);
		BuildMenus();
		UpdateProjectTitle();
		_titleBar.MainScreenChanged += OnMainScreenChanged;
		ShowLoadingScreen();
		CallDeferred("StartAsyncInit");
	}

	public override void _ExitTree()
	{
		Resized -= ApplyResponsiveWorkspaceLayout;
		CancellationTokenSource exportLifetimeCts = _exportLifetimeCts;
		_exportLifetimeCts = null;
		if (exportLifetimeCts != null)
		{
			try
			{
				exportLifetimeCts.Cancel();
			}
			catch (ObjectDisposedException)
			{
			}
			exportLifetimeCts.Dispose();
		}
		base._ExitTree();
	}

	private void ApplyResponsiveWorkspaceLayout()
	{
		if (GodotObject.IsInstanceValid(_workspaceSplit))
		{
			float num = Mathf.Max(Size.X, GetViewportRect().Size.X);
			if (!(num <= 0f))
			{
				int num2 = Mathf.Clamp(Mathf.FloorToInt((num - 760f) * 0.5f), 180, 300);
				_workspaceSplit.SplitOffsets = new int[2]
				{
					num2,
					-num2
				};
			}
		}
	}

	private void BuildMenus()
	{
		MenuBar menuBar = _titleBar.MenuBar;
		menuBar.GetNode<PopupMenu>("文件").IdPressed += OnFileMenuItem;
		menuBar.GetNode<PopupMenu>("编辑").IdPressed += OnEditMenuItem;
		PopupMenu node = menuBar.GetNode<PopupMenu>("模组");
		int itemIndex = node.GetItemIndex(20);
		if (itemIndex >= 0)
		{
			node.SetItemText(itemIndex, "应用已安装 Mod 到当前游戏");
		}
		int itemIndex2 = node.GetItemIndex(21);
		if (itemIndex2 >= 0)
		{
			node.SetItemText(itemIndex2, "打开 Mod 安装目录");
		}
		node.IdPressed += OnModMenuItem;
	}

	private void ShowLoadingScreen()
	{
		_loadingOverlay = GetNode<Control>("%LoadingOverlay");
		_loadingBar = GetNode<ProgressBar>("%LoadingBar");
		_loadingLabel = GetNode<Label>("%LoadingLabel");
		GetNode<ColorRect>("%LoadingBackground").Color = ModEditorTheme.BackgroundColor;
		GetNode<Label>("%LoadingTitle").AddThemeColorOverride("font_color", ModEditorTheme.TextColor);
		_loadingLabel.AddThemeColorOverride("font_color", ModEditorTheme.DimTextColor);
		_loadingOverlay.Show();
	}

	private async void StartAsyncInit()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		List<(string, Action)> steps = new List<(string, Action)>
		{
			("初始化核心系统...", () =>
			{
				_undoRedoManager = new XWUndoRedoManager();
				_undoRedoManager.ActionCommitted = Callable.From<string>(OnEditorActionCommitted);
				_undoRedoManager.ActionUndone = Callable.From<string>(OnEditorActionUndone);
				_undoRedoManager.ActionRedone = Callable.From<string>(OnEditorActionRedone);
				_editorData = new XWEditorData();
				_editorSettings = new XWEditorSettings();
				_editorSelection = new XWEditorSelection();
				_selectionHistory = new XWEditorSelectionHistory();
				_editorData.Load();
				_editorSettings.Load();
				_editorSettings.InitializeDefaultSettings();
				_titleBar.ConfigureMotion(_editorSettings.GetSetting("interface/reduced_motion", defaultValue: false), _editorSettings.GetSetting("interface/low_performance_mode", defaultValue: false));
			}),
			("初始化编辑器接口...", () =>
			{
				XWEditorInterface.Initialize(_undoRedoManager, _editorData, _editorSettings, _editorSelection, _selectionHistory);
				XWEditorInterface.Instance.SetEditorPanel(this);
				XWEditorInterface.Instance.SetLayoutManager(_layoutManager);
				XWEditorInterface.Instance.SetResourceEditorResolver(EnsureResourceEditor);
				XWEditorInterface.Instance.SetShowToastCallback(ShowToast);
			}),
			("注册属性编辑器...", RegisterBuiltinEditorFactories),
			("加载运行栏与输出面板...", () =>
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/RunBar/XWEditorRunBar.tscn", null, ResourceLoader.CacheMode.Reuse);
				_runBar = packedScene.Instantiate<XWEditorRunBar>(PackedScene.GenEditState.Disabled);
				_runBar.Name = "RunBar";
				_titleBar.RunBarContainer.AddChild(_runBar, forceReadableName: false, InternalMode.Disabled);
				ConnectRunBarSignals();
				PackedScene packedScene2 = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/OutPutPanel/XWOutputPanel.tscn", null, ResourceLoader.CacheMode.Reuse);
				_outputPanel = packedScene2.Instantiate<XWOutputPanel>(PackedScene.GenEditState.Disabled);
				_outputPanel.Name = "Output";
				_layoutManager.AddPanel(_outputPanel, "output", "输出", 3);
				XWEditorInterface.Instance.SetOutputPanel(_outputPanel);
				PackedScene packedScene3 = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/GUI/XWEditorToaster.tscn", null, ResourceLoader.CacheMode.Reuse);
				_toaster = packedScene3.Instantiate<XWEditorToaster>(PackedScene.GenEditState.Disabled);
				_toaster.Name = "Toaster";
				_toaster.SetAnchorsPreset(LayoutPreset.FullRect);
				_toaster.MouseFilter = MouseFilterEnum.Ignore;
				_toaster.ZIndex = 500;
				AddChild(_toaster, forceReadableName: false, InternalMode.Disabled);
			}),
			("加载场景树面板...", () =>
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/SceneEditor/SceneNodeTree/XWSceneTreeDock.tscn", null, ResourceLoader.CacheMode.Reuse);
				_sceneTreeDock = packedScene.Instantiate<XWSceneTreeDock>(PackedScene.GenEditState.Disabled);
				_sceneTreeDock.Name = "SceneTreeDock";
				_layoutManager.AddPanel(_sceneTreeDock, "scene_tree", "场景", 6);
				XWEditorInterface.Instance.SetSceneTreeDock(_sceneTreeDock);
			}),
			("加载文件系统面板...", () =>
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/FileSystem/GUI/XWFileSystemPanel.tscn", null, ResourceLoader.CacheMode.Reuse);
				_fileSystemPanel = packedScene.Instantiate<XWFileSystemPanel>(PackedScene.GenEditState.Disabled);
				_fileSystemPanel.Name = "FileSystemPanel";
				_layoutManager.AddPanel(_fileSystemPanel, "file_system", "文件系统", 7);
				XWEditorInterface.Instance.SetFileSystemPanel(_fileSystemPanel);
			}),
			("加载检查器面板...", () =>
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Inspector/GUI/XWInspector.tscn", null, ResourceLoader.CacheMode.Reuse);
				_inspector = packedScene.Instantiate<XWInspector>(PackedScene.GenEditState.Disabled);
				_inspector.Name = "Inspector";
				_inspector.UndoRedoManager = _undoRedoManager;
				_layoutManager.AddPanel(_inspector, "inspector", "检查器", 8);
				XWEditorInterface.Instance.SetInspector(_inspector);
			}),
			("加载二维场景编辑器...", () =>
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/SceneEditor/2D/GUI/XW2DSceneEditor.tscn", null, ResourceLoader.CacheMode.Reuse);
				_scene2DEditor = packedScene.Instantiate<XW2DSceneEditor>(PackedScene.GenEditState.Disabled);
				_scene2DEditor.Name = "Scene2DEditor";
				_layoutManager.AddPanel(_scene2DEditor, "2d_editor", "二维场景", 2);
				XWEditorInterface.Instance.Set2DSceneEditor(_scene2DEditor);
			}),
			("加载蓝图编辑器...", () =>
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Blueprint/GUI/XWBPEditor.tscn", null, ResourceLoader.CacheMode.Reuse);
				_bpEditor = packedScene.Instantiate<XWBPEditor>(PackedScene.GenEditState.Disabled);
				_bpEditor.Name = "BPEditor";
				_layoutManager.AddPanel(_bpEditor, "bp_editor", "蓝图", 2);
				XWEditorInterface.Instance.SetBlueprintEditor(_bpEditor);
			}),
			("加载脚本编辑器...", () =>
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ScriptEditor/GUI/XWScriptEditor.tscn", null, ResourceLoader.CacheMode.Reuse);
				_scriptEditor = packedScene.Instantiate<XWScriptEditor>(PackedScene.GenEditState.Disabled);
				_scriptEditor.Name = "ScriptEditor";
				_layoutManager.AddPanel(_scriptEditor, "script_editor", "脚本", 2);
				XWEditorInterface.Instance.SetScriptEditor(_scriptEditor);
			}),
			("加载资源编辑器...", () =>
			{
				LoadResourceEditorPanels();
			}),
			("加载问题面板...", () =>
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Issues/GUI/XWIssuePanel.tscn", null, ResourceLoader.CacheMode.Reuse);
				_issuePanel = packedScene.Instantiate<XWIssuePanel>(PackedScene.GenEditState.Disabled);
				_issuePanel.Name = "IssuePanel";
				_layoutManager.AddPanel(_issuePanel, "issues", "问题", 3);
				XWEditorInterface.Instance.SetIssuePanel(_issuePanel);
			}),
			("加载 Mod 工具面板...", () =>
			{
				PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/Tools/GUI/XWModToolsPanel.tscn", null, ResourceLoader.CacheMode.Reuse);
				_modToolsPanel = packedScene.Instantiate<XWModToolsPanel>(PackedScene.GenEditState.Disabled);
				_modToolsPanel.Name = "ModToolsPanel";
				_layoutManager.AddPanel(_modToolsPanel, "mod_tools", "Mod 工具", 9);
			}),
			("初始化 PVZ 集成...", () =>
			{
				PVZIntegrationModule.Initialize();
			})
		};
		int total = steps.Count;
		for (int i = 0; i < total; i++)
		{
			_loadingLabel.Text = steps[i].Item1;
			_loadingBar.Value = (float)i / (float)total * 100f;
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			steps[i].Item2();
			_loadingBar.Value = (float)(i + 1) / (float)total * 100f;
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		LoadProjectManagerPanel();
		_layoutManager.ApplySavedLayout();
		_layoutManager.AutoSaveLayout();
		XWEditorInterface.Instance?.FocusPanel("bp_editor");
		_loadingLabel.Text = "加载完成";
		_loadingBar.Value = 100.0;
		await ToSignal(GetTree().CreateTimer(0.3), SceneTreeTimer.SignalName.Timeout);
		_loadingOverlay.QueueFree();
		_loadingOverlay = null;
		ShowProjectManager();
		ShowToast("Mod 编辑器已加载");
	}

	private void LoadProjectManagerPanel()
	{
		PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ProjectManager/GUI/XWModProjectManagerPanel.tscn", null, ResourceLoader.CacheMode.Reuse);
		_projectManagerPanel = packedScene.Instantiate<XWModProjectManagerPanel>(PackedScene.GenEditState.Disabled);
		_projectManagerPanel.Name = "ProjectManagerPanel";
		_projectManagerPanel.SetAnchorsPreset(LayoutPreset.FullRect);
		_projectManagerPanel.ZIndex = 900;
		_projectManagerPanel.NewProjectRequested += ShowCreateProjectDialog;
		_projectManagerPanel.OpenProjectRequested += ShowOpenProjectDialog;
		_projectManagerPanel.ProjectOpenRequested += OpenProjectFromManager;
		AddChild(_projectManagerPanel, forceReadableName: false, InternalMode.Disabled);
	}

	private void ShowProjectManager()
	{
		if (_projectManagerPanel != null && GodotObject.IsInstanceValid(_projectManagerPanel))
		{
			_scriptEditor?.SetWorkspaceActive(active: false);
			_projectManagerPanel.ShowStartScreen();
			ModEditorManager.Instance?.SetEditorWindowProjectTitle(_currentProject?.Name);
		}
	}

	private void OpenProjectFromManager(string projectFilePath)
	{
		ModProject modProject = ModProject.Load(projectFilePath);
		if (modProject == null)
		{
			ShowToast("打开工程失败");
		}
		else if (EnterProject(modProject))
		{
			ShowToast($"已打开工程: {modProject.Name} ({modProject.ProjectPath})");
		}
	}

	private bool EnterProject(ModProject project)
	{
		if (project == null)
		{
			return false;
		}
		if (GodotObject.IsInstanceValid(_scriptEditor) && !_scriptEditor.TrySwitchProjectRoot(project.ProjectPath))
		{
			ShowToast("脚本页仍有未保存修改、外部冲突、构建或调试任务；请处理完成后再切换 Mod。");
			return false;
		}
		if (GodotObject.IsInstanceValid(_bpEditor) && !_bpEditor.TrySwitchProjectRoot(project.ProjectPath))
		{
			ShowToast("蓝图保存失败；已取消切换 Mod。");
			return false;
		}
		_currentProject = project;
		_scriptEditor?.RefreshBuildActionState();
		_modToolsPanel?.UpdateScriptBuildActionState();
		UpdateProjectTitle();
		_projectManagerPanel?.AddRecentProject(project);
		if (_projectManagerPanel != null && GodotObject.IsInstanceValid(_projectManagerPanel))
		{
			_projectManagerPanel.Visible = false;
		}
		NavigateFileSystemToProject(project.ProjectPath);
		XWEditorInterface.Instance?.FocusPanel("bp_editor");
		return true;
	}

	private void OnMainScreenChanged(string screenKey)
	{
		string key;
		if (_resourceEditorDescriptors.ContainsKey(screenKey))
		{
			key = (GodotObject.IsInstanceValid(EnsureResourceEditor(screenKey)) ? screenKey : "bp_editor");
		}
		else
		{
			key = screenKey switch
			{
				"blueprint" => "bp_editor", 
				"script" => "script_editor", 
				"2d" => "2d_editor", 
				_ => "bp_editor", 
			};
		}
		XWEditorInterface.Instance?.FocusPanel(key);
		CallDeferred("PlayWorkspaceTransition");
	}

	private void PlayWorkspaceTransition()
	{
		_centerWorkspaceMotion?.PlayPanelReveal(9f);
	}

	private static void RegisterBuiltinEditorFactories()
	{
		_ = XWTypeRegistry.Instance;
	}

	private void LoadResourceEditorPanels()
	{
		_resourceEditors.Clear();
		_resourceEditorDescriptors.Clear();
		foreach (XWVisualEditorDescriptor allEditor in XWResourceEditorRegistry.GetAllEditors())
		{
			_resourceEditorDescriptors[allEditor.DockKey] = allEditor;
			_titleBar.AddResourceEditorOption(allEditor.DockKey, GetMainScreenButtonText(allEditor), GetResourceEditorIcon(allEditor));
		}
	}

	public Control EnsureResourceEditor(string dockKey)
	{
		if (string.IsNullOrWhiteSpace(dockKey))
		{
			return null;
		}
		if (_resourceEditors.TryGetValue(dockKey, out var value) && GodotObject.IsInstanceValid(value))
		{
			return value;
		}
		if (!TryResolveResourceEditorDescriptor(dockKey, out var descriptor))
		{
			return null;
		}
		PackedScene packedScene = LoadResourceEditorScene(descriptor);
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			return null;
		}
		XWGenericVisualResourceEditor xWGenericVisualResourceEditor = packedScene.Instantiate<XWGenericVisualResourceEditor>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(xWGenericVisualResourceEditor))
		{
			return null;
		}
		xWGenericVisualResourceEditor.Name = descriptor.Category + "Editor";
		xWGenericVisualResourceEditor.LoadResource(null, "", descriptor);
		_resourceEditors[dockKey] = xWGenericVisualResourceEditor;
		_layoutManager.AddPanel(xWGenericVisualResourceEditor, dockKey, descriptor.DisplayName, 2);
		XWEditorInterface.Instance?.SetResourceEditor(dockKey, xWGenericVisualResourceEditor);
		return xWGenericVisualResourceEditor;
	}

	private bool TryResolveResourceEditorDescriptor(string dockKey, out XWVisualEditorDescriptor descriptor)
	{
		if (_resourceEditorDescriptors.TryGetValue(dockKey, out descriptor))
		{
			return true;
		}
		foreach (XWVisualEditorDescriptor allEditor in XWResourceEditorRegistry.GetAllEditors())
		{
			if (string.Equals(allEditor.DockKey, dockKey, StringComparison.Ordinal))
			{
				descriptor = allEditor;
				_resourceEditorDescriptors[dockKey] = allEditor;
				return true;
			}
		}
		descriptor = null;
		return false;
	}

	private static string GetMainScreenButtonText(XWVisualEditorDescriptor descriptor)
	{
		string text = descriptor?.DisplayName ?? "";
		if (string.IsNullOrWhiteSpace(text))
		{
			text = descriptor?.Category ?? "";
		}
		return text.Replace("编辑器", "").Trim();
	}

	private static Texture2D GetResourceEditorIcon(XWVisualEditorDescriptor descriptor)
	{
		string text = ((descriptor == null) ? "" : descriptor.IconPath);
		if (!string.IsNullOrWhiteSpace(text) && ResourceLoader.Exists(text))
		{
			return XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>(text, null, ResourceLoader.CacheMode.Reuse));
		}
		if (!ResourceLoader.Exists("res://addons/ModEditor/Icons/ClassIcon/ResourcePreloader.svg"))
		{
			return null;
		}
		return XWTextureSafety.SafeIcon(ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ClassIcon/ResourcePreloader.svg", null, ResourceLoader.CacheMode.Reuse));
	}

	private static PackedScene LoadResourceEditorScene(XWVisualEditorDescriptor descriptor)
	{
		if (descriptor == null)
		{
			return null;
		}
		string text = descriptor.ScenePath;
		if (string.IsNullOrWhiteSpace(text) || !ResourceLoader.Exists(text))
		{
			text = "res://addons/ModEditor/ResourceEditors/GUI/XWGenericVisualResourceEditor.tscn";
		}
		if (!ResourceLoader.Exists(text))
		{
			return null;
		}
		return ResourceLoader.Load<PackedScene>(text, null, ResourceLoader.CacheMode.Reuse);
	}

	private void ConnectRunBarSignals()
	{
		if (_runBar != null)
		{
			_runBar.PlayPressed += OnRunMainPressed;
			_runBar.PlayScenePressed += OnRunCurrentScenePressed;
			_runBar.PlayCustomPressed += OnRunCustomPressed;
			_runBar.StopPressed += OnRunStopPressed;
			_runBar.PauseToggled += OnRunPauseToggled;
		}
	}

	private void OnRunMainPressed()
	{
		LogRunMessage("运行主场景");
		PackedScene packedScene = TryLoadProjectMainScene(out var displayName);
		if (packedScene != null)
		{
			StartRunPreview(packedScene, displayName, XWEditorRunBar.RunMode.RunMain);
			return;
		}
		LogRunMessage("未找到工程主场景，改为运行当前 2D 场景。", XWOutputPanel.MessageType.Warning);
		RunCurrent2DScenePreview(XWEditorRunBar.RunMode.RunMain, "当前 2D 场景");
	}

	private void OnRunCurrentScenePressed()
	{
		RunCurrent2DScenePreview(XWEditorRunBar.RunMode.RunCurrent, "当前 2D 场景");
	}

	private void OnRunCustomPressed()
	{
		LogRunMessage("选择自定义运行场景");
		_runBar?.SetRunning(running: false);
		ShowRunCustomSceneDialog();
	}

	private void OnRunStopPressed()
	{
		StopRunPreview(updateRunBar: true, "停止运行");
	}

	private void OnRunPauseToggled(bool paused)
	{
		if (_runPreviewRoot == null || !GodotObject.IsInstanceValid(_runPreviewRoot))
		{
			_runBar?.SetPaused(paused: false);
			ReportRunError("没有正在运行的场景。");
			return;
		}
		SetRunPreviewPaused(paused);
		_runBar?.SetPaused(paused);
		LogRunMessage(paused ? "暂停运行" : "继续运行");
		ShowToast(paused ? "已暂停运行预览" : "已继续运行预览");
	}

	private void RunCurrent2DScenePreview(XWEditorRunBar.RunMode mode, string label)
	{
		PackedScene packedScene = CreatePackedSceneFromCurrent2D();
		if (packedScene == null)
		{
			_runBar?.SetRunning(running: false);
		}
		else
		{
			StartRunPreview(packedScene, GetCurrent2DSceneDisplayName(label), mode);
		}
	}

	private PackedScene CreatePackedSceneFromCurrent2D()
	{
		Node node = _scene2DEditor?.CurrentSceneInstance;
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			ReportRunError("没有打开的 2D 场景。");
			return null;
		}
		PackedScene packedScene = new PackedScene();
		Error error = packedScene.Pack(node);
		if (error != Error.Ok)
		{
			ReportRunError($"打包当前 2D 场景失败: {error}");
			return null;
		}
		return packedScene;
	}

	private PackedScene TryLoadProjectMainScene(out string displayName)
	{
		displayName = "";
		string text = FindProjectMainScenePath();
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		string text2 = text.Replace('\\', '/');
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(text2, null, ResourceLoader.CacheMode.Reuse);
		if (packedScene == null)
		{
			LogRunMessage("无法加载工程主场景: " + text2, XWOutputPanel.MessageType.Warning);
			return null;
		}
		displayName = Path.GetFileName(text);
		return packedScene;
	}

	private string FindProjectMainScenePath()
	{
		if (_currentProject == null || string.IsNullOrWhiteSpace(_currentProject.ProjectPath))
		{
			return "";
		}
		string text = Path.Combine(_currentProject.ProjectPath, "Scenes");
		if (!Directory.Exists(text))
		{
			return "";
		}
		string[] array = new string[4] { "Main.tscn", "main.tscn", "Test.tscn", "test.tscn" };
		foreach (string path in array)
		{
			string text2 = Path.Combine(text, path);
			if (File.Exists(text2))
			{
				return text2;
			}
		}
		List<string> list = new List<string>(Directory.EnumerateFiles(text, "*.tscn", SearchOption.AllDirectories));
		list.Sort(StringComparer.OrdinalIgnoreCase);
		if (list.Count <= 0)
		{
			return "";
		}
		return list[0];
	}

	private void ShowRunCustomSceneDialog()
	{
		if (_runCustomSceneDialog == null || !GodotObject.IsInstanceValid(_runCustomSceneDialog))
		{
			if (_runCustomSceneDialogScene == null)
			{
				_runCustomSceneDialogScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/GUI/XWRunCustomSceneDialog.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			_runCustomSceneDialog = _runCustomSceneDialogScene?.Instantiate<FileDialog>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(_runCustomSceneDialog))
			{
				ReportRunError("无法加载自定义场景选择对话框。");
				return;
			}
			_runCustomSceneDialog.FileSelected += OnRunCustomSceneSelected;
			AddChild(_runCustomSceneDialog, forceReadableName: false, InternalMode.Disabled);
		}
		if (_currentProject != null && !string.IsNullOrWhiteSpace(_currentProject.ProjectPath))
		{
			string text = Path.Combine(_currentProject.ProjectPath, "Scenes");
			if (Directory.Exists(text))
			{
				_runCustomSceneDialog.CurrentDir = text;
			}
		}
		_runCustomSceneDialog.PopupCenteredClamped(new Vector2I(700, 500), 0.9f);
	}

	private void OnRunCustomSceneSelected(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			_runBar?.SetRunning(running: false);
			return;
		}
		string text = path.Replace('\\', '/');
		PackedScene packedScene = ResourceLoader.Load<PackedScene>(text, null, ResourceLoader.CacheMode.Reuse);
		if (packedScene == null)
		{
			_runBar?.SetRunning(running: false);
			ReportRunError("无法加载场景: " + text);
		}
		else
		{
			StartRunPreview(packedScene, Path.GetFileName(path), XWEditorRunBar.RunMode.RunCustom);
		}
	}

	private void StartRunPreview(PackedScene packedScene, string displayName, XWEditorRunBar.RunMode runMode)
	{
		if (packedScene == null)
		{
			_runBar?.SetRunning(running: false);
			return;
		}
		Node node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
		if (node != null)
		{
			Node sceneRoot = node;
			StopRunPreview(updateRunBar: false);
			if (!ShowRunPreviewWindow(sceneRoot, displayName))
			{
				_runBar?.SetRunning(running: false);
				return;
			}
			_runBar?.SetRunMode(runMode);
			_runBar?.SetPaused(paused: false);
			LogRunMessage("开始运行: " + displayName);
			ShowToast("正在运行: " + displayName);
		}
		else
		{
			_runBar?.SetRunning(running: false);
			ReportRunError("场景没有有效根节点。");
		}
	}

	private bool ShowRunPreviewWindow(Node sceneRoot, string displayName)
	{
		Vector2I value = new Vector2I(960, 540);
		if (_runPreviewWindowScene == null)
		{
			_runPreviewWindowScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/GUI/XWRunPreviewWindow.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		_runPreviewWindow = _runPreviewWindowScene?.Instantiate<Window>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(_runPreviewWindow))
		{
			if (GodotObject.IsInstanceValid(sceneRoot))
			{
				sceneRoot.QueueFree();
			}
			ReportRunError("无法加载运行预览窗口场景。");
			return false;
		}
		_runPreviewWindow.Title = "运行预览 - " + displayName;
		_runPreviewWindow.CloseRequested += () =>
		{
			StopRunPreview(updateRunBar: true, "关闭运行预览");
		};
		AddChild(_runPreviewWindow, forceReadableName: false, InternalMode.Disabled);
		_runPreviewViewport = _runPreviewWindow.GetNode<SubViewport>("RunPreviewContainer/RunPreviewViewport");
		_runPreviewRoot = sceneRoot;
		_runPreviewViewport.AddChild(_runPreviewRoot, forceReadableName: false, InternalMode.Disabled);
		if (_runPreviewRoot is Control control)
		{
			control.SetAnchorsPreset(LayoutPreset.FullRect);
		}
		CacheRunPreviewProcessModes(_runPreviewRoot);
		_runPreviewWindow.PopupCenteredClamped(value, 0.9f);
		return true;
	}

	private void StopRunPreview(bool updateRunBar, string message = "")
	{
		if (_runPreviewWindow != null && GodotObject.IsInstanceValid(_runPreviewWindow))
		{
			_runPreviewWindow.QueueFree();
		}
		_runPreviewWindow = null;
		_runPreviewViewport = null;
		_runPreviewRoot = null;
		_runPreviewProcessModes.Clear();
		if (updateRunBar)
		{
			_runBar?.SetRunning(running: false);
			if (!string.IsNullOrWhiteSpace(message))
			{
				LogRunMessage(message);
				ShowToast(message);
			}
		}
	}

	private void CacheRunPreviewProcessModes(Node node)
	{
		_runPreviewProcessModes.Clear();
		CacheRunPreviewProcessModesRecursive(node);
	}

	private void CacheRunPreviewProcessModesRecursive(Node node)
	{
		if (node == null || !GodotObject.IsInstanceValid(node))
		{
			return;
		}
		_runPreviewProcessModes[node] = node.ProcessMode;
		foreach (Node child in node.GetChildren())
		{
			if (child != null)
			{
				Node node2 = child;
				CacheRunPreviewProcessModesRecursive(node2);
			}
		}
	}

	private void SetRunPreviewPaused(bool paused)
	{
		foreach (KeyValuePair<Node, ProcessModeEnum> runPreviewProcessMode in _runPreviewProcessModes)
		{
			if (GodotObject.IsInstanceValid(runPreviewProcessMode.Key))
			{
				runPreviewProcessMode.Key.ProcessMode = (paused ? ProcessModeEnum.Disabled : runPreviewProcessMode.Value);
			}
		}
	}

	private string GetCurrent2DSceneDisplayName(string fallback)
	{
		string text = _scene2DEditor?.CurrentPackedScene?.ResourcePath ?? "";
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text.GetFile();
		}
		string text2 = _scene2DEditor?.CurrentSceneInstance?.Name ?? ((StringName)"");
		if (!string.IsNullOrWhiteSpace(text2))
		{
			return text2;
		}
		return fallback;
	}

	private void LogRunMessage(string message, XWOutputPanel.MessageType type = XWOutputPanel.MessageType.Editor)
	{
		if (!string.IsNullOrWhiteSpace(message))
		{
			_outputPanel?.AddMessage(message, type);
		}
	}

	private void ReportRunError(string message)
	{
		LogRunMessage(message, XWOutputPanel.MessageType.Error);
		ShowToast(message);
	}

	private void OnFileMenuItem(long id)
	{
		if ((ulong)id <= 6uL)
		{
			switch ((int)id)
			{
			case 0:
				ShowCreateProjectDialog();
				return;
			case 1:
				ShowOpenProjectDialog();
				return;
			case 2:
				SaveActiveDocument();
				return;
			case 6:
				SaveAllDocuments();
				return;
			case 3:
				ShowExportDialog();
				return;
			case 4:
				ModEditorManager.Instance.CloseEditor();
				return;
			case 5:
				ShowProjectManager();
				return;
			}
		}
		ShowToast($"文件菜单项 {id}（待实现）");
	}

	private void OnEditMenuItem(long id)
	{
		_scene2DEditor?.CommitPendingAuthoringInput();
		switch (id)
		{
		case 10L:
			_undoRedoManager.Undo();
			break;
		case 11L:
			_undoRedoManager.Redo();
			break;
		}
	}

	private async void OnModMenuItem(long id)
	{
		long num = id - 20;
		if ((ulong)num > 3uL)
		{
			return;
		}
		switch ((int)num)
		{
		case 0:
		{
			if (OperatingSystem.IsAndroid())
			{
				ShowToast("Android 上的 Mod 状态修改将在重启游戏后生效");
				break;
			}
			int num2 = ModLoader.LoadAll();
			ModEditorPanel modEditorPanel = this;
			string message;
			if (num2 < 0)
			{
				message = "Mod 重载被仍在使用的运行实例阻止";
			}
			else
			{
				message = ((num2 > 0) ? $"已重新应用 {num2} 个已启用 Mod" : "没有已启用且可安全应用的 Mod；请查看输出诊断");
			}
			modEditorPanel.ShowToast(message);
			break;
		}
		case 1:
		{
			string text = ProjectSettings.GlobalizePath("user://Mods/");
			Directory.CreateDirectory(text);
			OS.ShellOpen(text);
			ShowToast("已打开 Mod 安装目录");
			break;
		}
		case 2:
			if (_currentProject == null)
			{
				ShowToast("没有打开的工程，请先新建或打开工程");
			}
			else
			{
				await ExportCurrentProjectAsync("");
			}
			break;
		case 3:
			ShowProjectSettingsDialog();
			break;
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventKey { Pressed: not false, Echo: false } inputEventKey)
		{
			if (inputEventKey.Keycode == Key.S && inputEventKey.CtrlPressed && !inputEventKey.ShiftPressed && !inputEventKey.AltPressed)
			{
				SaveActiveDocument();
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.Z && inputEventKey.CtrlPressed)
			{
				_scene2DEditor?.CommitPendingAuthoringInput();
				_undoRedoManager.Undo();
				GetViewport().SetInputAsHandled();
			}
			else if (inputEventKey.Keycode == Key.Y && inputEventKey.CtrlPressed)
			{
				_scene2DEditor?.CommitPendingAuthoringInput();
				_undoRedoManager.Redo();
				GetViewport().SetInputAsHandled();
			}
		}
	}

	public override void _Notification(int what)
	{
		base._Notification(what);
		if ((long)what == 21)
		{
			if (GetViewport().GuiGetDragData().Obj is XWDragData xWDragData && xWDragData.IsType(XWDragData.Type.DockPanel))
			{
				_isDockDragging = true;
				SetProcess(enable: true);
				_dragPanelKey = xWDragData.GetPanelKey();
				_dockOverlay.CenterZoneEnabled = _layoutManager?.CanPanelMoveToDock(_dragPanelKey, 2) ?? false;
				UpdateDockOverlayZones();
				_dockOverlay.Visible = true;
				_dockOverlay.Modulate = new Color(1f, 1f, 1f, 0f);
				_dockOverlay.CreateTween().TweenProperty(_dockOverlay, "modulate:a", 1f, 0.2);
			}
		}
		else
		{
			if ((long)what != 22 || !_isDockDragging)
			{
				return;
			}
			_isDockDragging = false;
			SetProcess(enable: false);
			if (_dockOverlay != null)
			{
				int highlightZone = _dockOverlay.GetHighlightZone();
				_dockOverlay.CenterZoneEnabled = true;
				_dockOverlay.ClearHighlight();
				Tween tween = _dockOverlay.CreateTween();
				tween.TweenProperty(_dockOverlay, "modulate:a", 0f, 0.15);
				tween.TweenCallback(Callable.From(() =>
				{
					if (_dockOverlay != null)
					{
						_dockOverlay.Visible = false;
					}
				}));
				if (!string.IsNullOrEmpty(_dragPanelKey))
				{
					int panelCurrentDock = _layoutManager.GetPanelCurrentDock(_dragPanelKey);
					if (highlightZone >= 0 && highlightZone != panelCurrentDock)
					{
						if (_layoutManager.CanPanelMoveToDock(_dragPanelKey, highlightZone))
						{
							_layoutManager.MovePanelToDock(_dragPanelKey, highlightZone);
						}
					}
					else if (highlightZone >= 0 && highlightZone == panelCurrentDock)
					{
						_layoutManager.GetDockContainer(highlightZone)?.ReorderTab(_dragPanelKey, GetGlobalMousePosition());
					}
				}
			}
			_dragPanelKey = "";
		}
	}

	public override void _Process(double delta)
	{
		if (_isDockDragging && _dockOverlay != null)
		{
			Vector2 globalMousePosition = GetGlobalMousePosition();
			_dockOverlay.UpdateHighlight(globalMousePosition);
		}
	}

	private void UpdateDockOverlayZones()
	{
		if (_dockOverlay != null)
		{
			Rect2 rect = default;
			Rect2 value = default;
			Rect2 value2 = default;
			Rect2 value3 = default;
			Rect2 rect2 = default;
			Rect2 value4 = default;
			Rect2 value5 = default;
			Rect2 value6 = default;
			Rect2 rect3 = default;
			Rect2 rect4 = default;
			if (_leftDock != null && _leftDock.Visible)
			{
				rect = new Rect2(_leftDock.GlobalPosition - GlobalPosition, _leftDock.Size);
			}
			if (_leftBottomDock != null && _leftBottomDock.Visible)
			{
				value = new Rect2(_leftBottomDock.GlobalPosition - GlobalPosition, _leftBottomDock.Size);
			}
			if (_leftTopRightDock != null && _leftTopRightDock.Visible)
			{
				value2 = new Rect2(_leftTopRightDock.GlobalPosition - GlobalPosition, _leftTopRightDock.Size);
			}
			if (_leftBottomRightDock != null && _leftBottomRightDock.Visible)
			{
				value3 = new Rect2(_leftBottomRightDock.GlobalPosition - GlobalPosition, _leftBottomRightDock.Size);
			}
			if (_rightDock != null && _rightDock.Visible)
			{
				rect2 = new Rect2(_rightDock.GlobalPosition - GlobalPosition, _rightDock.Size);
			}
			if (_rightBottomDock != null && _rightBottomDock.Visible)
			{
				value4 = new Rect2(_rightBottomDock.GlobalPosition - GlobalPosition, _rightBottomDock.Size);
			}
			if (_rightTopRightDock != null && _rightTopRightDock.Visible)
			{
				value5 = new Rect2(_rightTopRightDock.GlobalPosition - GlobalPosition, _rightTopRightDock.Size);
			}
			if (_rightBottomRightDock != null && _rightBottomRightDock.Visible)
			{
				value6 = new Rect2(_rightBottomRightDock.GlobalPosition - GlobalPosition, _rightBottomRightDock.Size);
			}
			if (_centerDock != null && _centerDock.Visible)
			{
				rect3 = new Rect2(_centerDock.GlobalPosition - GlobalPosition, _centerDock.Size);
			}
			if (_bottomPanel != null && _bottomPanel.Visible)
			{
				rect4 = new Rect2(_bottomPanel.GlobalPosition - GlobalPosition, _bottomPanel.Size);
			}
			Dictionary<int, Rect2> subZones = new Dictionary<int, Rect2>
			{
				{ 4, rect },
				{ 5, value },
				{ 6, value2 },
				{ 7, value3 },
				{ 2, rect3 },
				{ 8, rect2 },
				{ 9, value4 },
				{ 10, value5 },
				{ 11, value6 },
				{ 3, rect4 }
			};
			_dockOverlay.UpdateZones(rect, rect2, rect3, rect4, subZones);
		}
	}

	private void ShowCreateProjectDialog()
	{
		if (_createProjectDialog == null || !GodotObject.IsInstanceValid(_createProjectDialog))
		{
			if (_createProjectDialogScene == null)
			{
				_createProjectDialogScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ModSystem/ModProjectCreateDialog.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			_createProjectDialog = _createProjectDialogScene?.Instantiate<ModProjectCreateDialog>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(_createProjectDialog))
			{
				ShowToast("无法加载新建 Mod 工程对话框");
				return;
			}
			AddChild(_createProjectDialog, forceReadableName: false, InternalMode.Disabled);
			_createProjectDialog.Confirmed += OnCreateProjectConfirmed;
		}
		_createProjectDialog.PopupCentered();
	}

	private void OnCreateProjectConfirmed()
	{
		string modName = _createProjectDialog.ModName;
		string selectedParentDir = _createProjectDialog.SelectedParentDir;
		if (string.IsNullOrWhiteSpace(modName))
		{
			ShowToast("工程名称不能为空");
			_createProjectDialog.PopupCentered();
			return;
		}
		if (string.IsNullOrWhiteSpace(selectedParentDir))
		{
			ShowToast("请输入或选择工程保存位置");
			_createProjectDialog.PopupCentered();
			return;
		}
		if (!DirAccess.DirExistsAbsolute(selectedParentDir))
		{
			DirAccess.MakeDirRecursiveAbsolute(selectedParentDir);
			if (!DirAccess.DirExistsAbsolute(selectedParentDir))
			{
				ShowToast("目录无效或无法创建: " + selectedParentDir);
				_createProjectDialog.PopupCentered();
				return;
			}
		}
		ModProject modProject = ModProject.Create(selectedParentDir, modName, _createProjectDialog.ModVersion, _createProjectDialog.ModAuthor, _createProjectDialog.ModDescription);
		if (modProject == null)
		{
			ShowToast("创建工程失败，请检查名称和路径");
			_createProjectDialog.PopupCentered();
		}
		else if (EnterProject(modProject))
		{
			ShowToast($"已创建工程: {modProject.Name} ({modProject.ProjectPath})");
		}
	}

	private void ShowOpenProjectDialog()
	{
		if (_openProjectFileDialog == null || !GodotObject.IsInstanceValid(_openProjectFileDialog))
		{
			if (_openProjectDialogScene == null)
			{
				_openProjectDialogScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/GUI/XWOpenProjectDialog.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			_openProjectFileDialog = _openProjectDialogScene?.Instantiate<FileDialog>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(_openProjectFileDialog))
			{
				ShowToast("无法加载打开工程对话框");
				return;
			}
			_openProjectFileDialog.FileSelected += OnOpenProjectFileSelected;
			AddChild(_openProjectFileDialog, forceReadableName: false, InternalMode.Disabled);
		}
		_openProjectFileDialog.PopupCentered();
	}

	private void OnOpenProjectFileSelected(string path)
	{
		OpenProjectFromManager(path);
	}

	public bool SaveActiveDocument()
	{
		bool flag = false;
		bool result = false;
		string text = _layoutManager?.ActiveMainPanelKey ?? "";
		bool num = text == "script_editor" || (string.IsNullOrEmpty(text) && GodotObject.IsInstanceValid(_scriptEditor) && _scriptEditor.IsVisibleInTree());
		bool flag2 = text == "2d_editor" || (string.IsNullOrEmpty(text) && GodotObject.IsInstanceValid(_scene2DEditor) && _scene2DEditor.IsVisibleInTree());
		bool flag3 = text == "bp_editor" || (string.IsNullOrEmpty(text) && GodotObject.IsInstanceValid(_bpEditor) && _bpEditor.IsVisibleInTree());
		XWGenericVisualResourceEditor value;
		if (num && GodotObject.IsInstanceValid(_scriptEditor) && _scriptEditor.HasFileOpen())
		{
			flag = true;
			result = _scriptEditor.SaveFile();
		}
		else if (flag2 && GodotObject.IsInstanceValid(_scene2DEditor) && GodotObject.IsInstanceValid(_scene2DEditor.CurrentSceneInstance))
		{
			flag = true;
			result = _scene2DEditor.SaveCurrentScene();
		}
		else if (flag3 && GodotObject.IsInstanceValid(_bpEditor) && _bpEditor.HasEditScript())
		{
			flag = true;
			result = _bpEditor.FlushBlueprintPersistence();
		}
		else if (_resourceEditors.TryGetValue(text, out value) && GodotObject.IsInstanceValid(value) && GodotObject.IsInstanceValid(value.ActiveResource))
		{
			flag = true;
			result = value.SaveActiveResource();
		}
		bool result2 = SaveCurrentProject(!flag);
		if (!flag)
		{
			return result2;
		}
		return result;
	}

	public bool SaveAllDocuments()
	{
		bool flag = true;
		if (GodotObject.IsInstanceValid(_scriptEditor))
		{
			flag &= _scriptEditor.SaveAllTabs() >= 0;
		}
		if (GodotObject.IsInstanceValid(_scene2DEditor))
		{
			flag &= _scene2DEditor.SaveAllScenes(showToast: false) >= 0;
		}
		if (GodotObject.IsInstanceValid(_bpEditor))
		{
			flag &= _bpEditor.SaveAllBlueprints(showToast: false) >= 0;
		}
		foreach (XWGenericVisualResourceEditor value in _resourceEditors.Values)
		{
			if (GodotObject.IsInstanceValid(value))
			{
				flag &= value.SaveAllOpenResources(showToast: false);
			}
		}
		flag &= SaveCurrentProject(showToast: false);
		ShowToast(flag ? "已保存全部文档与 Mod 项目。" : "保存全部已完成，但有文档保存失败，请查看输出面板。");
		return flag;
	}

	private bool SaveCurrentProject(bool showToast = true)
	{
		if (_currentProject == null)
		{
			if (showToast)
			{
				ShowToast("没有打开的工程，请先新建或打开工程");
			}
			return false;
		}
		_currentProject.Save();
		UpdateProjectTitle();
		if (showToast)
		{
			ShowToast("已保存工程: " + _currentProject.Name);
		}
		return true;
	}

	private void ShowProjectSettingsDialog()
	{
		if (_projectSettingsDialog == null || !GodotObject.IsInstanceValid(_projectSettingsDialog))
		{
			PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ProjectSettings/GUI/XWProjectSettingsDialog.tscn", null, ResourceLoader.CacheMode.Reuse);
			_projectSettingsDialog = packedScene.Instantiate<XWProjectSettingsDialog>(PackedScene.GenEditState.Disabled);
			_projectSettingsDialog.ProjectSaved += OnProjectSettingsSaved;
			AddChild(_projectSettingsDialog, forceReadableName: false, InternalMode.Disabled);
		}
		_projectSettingsDialog.EditProject(_currentProject, _editorSettings);
		_projectSettingsDialog.PopupCenteredClamped(new Vector2I(720, 560), 0.9f);
	}

	private void OnProjectSettingsSaved()
	{
		UpdateProjectTitle();
		if (_currentProject != null)
		{
			NavigateFileSystemToProject(_currentProject.ProjectPath);
		}
	}

	private void UpdateProjectTitle()
	{
		string editorWindowProjectTitle = _currentProject?.Name;
		_titleBar?.SetProjectTitle("");
		ModEditorManager.Instance?.SetEditorWindowProjectTitle(editorWindowProjectTitle);
	}

	private void ShowExportDialog()
	{
		if (_currentProject == null)
		{
			ShowToast("没有打开的工程，请先新建或打开工程");
			return;
		}
		if (_isExporting)
		{
			ShowToast("Mod 正在后台编译并导出，请稍候");
			return;
		}
		if (_exportDirDialog == null || !GodotObject.IsInstanceValid(_exportDirDialog))
		{
			if (_exportDirectoryDialogScene == null)
			{
				_exportDirectoryDialogScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/GUI/XWExportDirectoryDialog.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			_exportDirDialog = _exportDirectoryDialogScene?.Instantiate<FileDialog>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(_exportDirDialog))
			{
				ShowToast("无法加载导出目录对话框");
				return;
			}
			_exportDirDialog.DirSelected += OnExportDirSelected;
			AddChild(_exportDirDialog, forceReadableName: false, InternalMode.Disabled);
		}
		_exportDirDialog.CurrentDir = (string.IsNullOrWhiteSpace(_currentProject.ExportDirectory) ? ProjectSettings.GlobalizePath("user://Mods/") : _currentProject.ExportDirectory);
		_exportDirDialog.PopupCentered();
	}

	private async void OnExportDirSelected(string dir)
	{
		if (_currentProject == null)
		{
			ShowToast("没有打开的工程");
		}
		else
		{
			await ExportCurrentProjectAsync(dir);
		}
	}

	private async Task ExportCurrentProjectAsync(string outputDirectory)
	{
		if (_isExporting)
		{
			ShowToast("Mod 正在后台编译并导出，请稍候");
			return;
		}
		ModProject currentProject = _currentProject;
		if (currentProject == null)
		{
			ShowToast("没有打开的工程");
			return;
		}
		if (!SaveAllDocuments())
		{
			string message = "导出已停止：有文档保存失败，请先修复保存问题。";
			LogRunMessage(message, XWOutputPanel.MessageType.Error);
			ShowToast(message);
			return;
		}
		CancellationTokenSource exportLifetimeCts = _exportLifetimeCts;
		if (exportLifetimeCts == null || exportLifetimeCts.IsCancellationRequested)
		{
			return;
		}
		_isExporting = true;
		ShowToast("正在后台编译并导出 Mod…");
		ModProject.ExportResult exportResult;
		try
		{
			exportResult = await currentProject.ExportAsync(outputDirectory, exportLifetimeCts.Token);
		}
		catch (Exception ex)
		{
			exportResult = new ModProject.ExportResult
			{
				Success = false,
				ErrorMessage = "Mod 导出出现未处理错误：" + ex.Message
			};
		}
		finally
		{
			_isExporting = false;
		}
		if (!GodotObject.IsInstanceValid(this))
		{
			return;
		}
		if (exportResult.Issues.Count > 0)
		{
			if (XWEditorInterface.Instance?.GetIssuePanel() is XWIssuePanel xWIssuePanel)
			{
				xWIssuePanel.LoadIssues(exportResult.Issues);
				XWEditorInterface.Instance.FocusPanel("issues");
			}
			foreach (XWValidationIssue issue in exportResult.Issues)
			{
				LogRunMessage(issue.Message, XWOutputPanel.MessageType.Std);
			}
		}
		if (exportResult.Success)
		{
			string text = (exportResult.ContainsRuntimeAssembly ? "已导出 Mod（包含已编译的 C# 运行程序集）" : "已导出纯资源 Mod（无需 C# 运行程序集）");
			LogRunMessage(text + ": " + exportResult.OutputPath, XWOutputPanel.MessageType.Std);
			ShowToast(text + ": " + exportResult.OutputPath);
		}
		else
		{
			string message2 = (string.IsNullOrWhiteSpace(exportResult.ErrorMessage) ? "Mod 导出失败，请查看输出面板。" : exportResult.ErrorMessage);
			LogRunMessage(message2, XWOutputPanel.MessageType.Error);
			ShowToast(message2);
		}
	}

	public ModProject GetCurrentProject()
	{
		return _currentProject;
	}

	private void NavigateFileSystemToProject(string projectDir)
	{
		if (_fileSystemPanel != null && GodotObject.IsInstanceValid(_fileSystemPanel))
		{
			_fileSystemPanel.NavigateToProject(projectDir);
		}
	}

	public void ShowToast(string message)
	{
		if (_statusLabel != null)
		{
			_statusLabel.Text = message;
		}
		_toaster?.ShowToast(message);
	}

	private void OnEditorActionCommitted(string actionName)
	{
		LogEditorAction(actionName);
	}

	private void OnEditorActionUndone(string actionName)
	{
		LogEditorUndo(actionName);
	}

	private void OnEditorActionRedone(string actionName)
	{
		LogEditorRedo(actionName);
	}

	public void LogEditorAction(string actionName)
	{
		if (!string.IsNullOrWhiteSpace(actionName))
		{
			_outputPanel?.AddMessage("动作: " + actionName, XWOutputPanel.MessageType.Editor);
		}
	}

	public void LogEditorUndo(string actionName)
	{
		if (!string.IsNullOrWhiteSpace(actionName))
		{
			_outputPanel?.AddMessage("撤销: " + actionName, XWOutputPanel.MessageType.Editor);
		}
	}

	public void LogEditorRedo(string actionName)
	{
		if (!string.IsNullOrWhiteSpace(actionName))
		{
			_outputPanel?.AddMessage("重做: " + actionName, XWOutputPanel.MessageType.Editor);
		}
	}

	public XWOutputPanel GetOutputPanel()
	{
		return _outputPanel;
	}

	public XWEditorRunBar GetRunBar()
	{
		return _runBar;
	}

	public XWModToolsPanel GetModToolsPanel()
	{
		return _modToolsPanel;
	}

	public XWSceneTreeDock GetSceneTreeDock()
	{
		return _sceneTreeDock;
	}

	public XW2DSceneEditor Get2DSceneEditor()
	{
		return _scene2DEditor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(66)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyResponsiveWorkspaceLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildMenus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowLoadingScreen, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartAsyncInit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadProjectManagerPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowProjectManager, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenProjectFromManager, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "projectFilePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnMainScreenChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "screenKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PlayWorkspaceTransition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RegisterBuiltinEditorFactories, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.LoadResourceEditorPanels, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureResourceEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dockKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectRunBarSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRunMainPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRunCurrentScenePressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRunCustomPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRunStopPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRunPauseToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "paused", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RunCurrent2DScenePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "mode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePackedSceneFromCurrent2D, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindProjectMainScenePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowRunCustomSceneDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnRunCustomSceneSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartRunPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packedScene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.String, "displayName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "runMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowRunPreviewWindow, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "sceneRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "displayName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StopRunPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "updateRunBar", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CacheRunPreviewProcessModes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CacheRunPreviewProcessModesRecursive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetRunPreviewPaused, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "paused", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrent2DSceneDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LogRunMessage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReportRunError, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnFileMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnEditMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnModMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._UnhandledInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "event", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateDockOverlayZones, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowCreateProjectDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCreateProjectConfirmed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowOpenProjectDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnOpenProjectFileSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveActiveDocument, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveAllDocuments, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveCurrentProject, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "showToast", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowProjectSettingsDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnProjectSettingsSaved, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateProjectTitle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowExportDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnExportDirSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "dir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NavigateFileSystemToProject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "projectDir", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowToast, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnEditorActionCommitted, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnEditorActionUndone, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnEditorActionRedone, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
			new MethodInfo(MethodName.GetOutputPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetRunBar, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetModToolsPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSceneTreeDock, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Get2DSceneEditor, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.ApplyResponsiveWorkspaceLayout && args.Count == 0)
		{
			ApplyResponsiveWorkspaceLayout();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildMenus && args.Count == 0)
		{
			BuildMenus();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowLoadingScreen && args.Count == 0)
		{
			ShowLoadingScreen();
			ret = default;
			return true;
		}
		if (method == MethodName.StartAsyncInit && args.Count == 0)
		{
			StartAsyncInit();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadProjectManagerPanel && args.Count == 0)
		{
			LoadProjectManagerPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowProjectManager && args.Count == 0)
		{
			ShowProjectManager();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenProjectFromManager && args.Count == 1)
		{
			OpenProjectFromManager(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnMainScreenChanged && args.Count == 1)
		{
			OnMainScreenChanged(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PlayWorkspaceTransition && args.Count == 0)
		{
			PlayWorkspaceTransition();
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterBuiltinEditorFactories && args.Count == 0)
		{
			RegisterBuiltinEditorFactories();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadResourceEditorPanels && args.Count == 0)
		{
			LoadResourceEditorPanels();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureResourceEditor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(EnsureResourceEditor(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ConnectRunBarSignals && args.Count == 0)
		{
			ConnectRunBarSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRunMainPressed && args.Count == 0)
		{
			OnRunMainPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRunCurrentScenePressed && args.Count == 0)
		{
			OnRunCurrentScenePressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRunCustomPressed && args.Count == 0)
		{
			OnRunCustomPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRunStopPressed && args.Count == 0)
		{
			OnRunStopPressed();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRunPauseToggled && args.Count == 1)
		{
			OnRunPauseToggled(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunCurrent2DScenePreview && args.Count == 2)
		{
			RunCurrent2DScenePreview(VariantUtils.ConvertTo<XWEditorRunBar.RunMode>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePackedSceneFromCurrent2D && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(CreatePackedSceneFromCurrent2D());
			return true;
		}
		if (method == MethodName.FindProjectMainScenePath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(FindProjectMainScenePath());
			return true;
		}
		if (method == MethodName.ShowRunCustomSceneDialog && args.Count == 0)
		{
			ShowRunCustomSceneDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.OnRunCustomSceneSelected && args.Count == 1)
		{
			OnRunCustomSceneSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartRunPreview && args.Count == 3)
		{
			StartRunPreview(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<XWEditorRunBar.RunMode>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowRunPreviewWindow && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ShowRunPreviewWindow(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.StopRunPreview && args.Count == 2)
		{
			StopRunPreview(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CacheRunPreviewProcessModes && args.Count == 1)
		{
			CacheRunPreviewProcessModes(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CacheRunPreviewProcessModesRecursive && args.Count == 1)
		{
			CacheRunPreviewProcessModesRecursive(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetRunPreviewPaused && args.Count == 1)
		{
			SetRunPreviewPaused(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrent2DSceneDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrent2DSceneDisplayName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LogRunMessage && args.Count == 2)
		{
			LogRunMessage(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<XWOutputPanel.MessageType>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReportRunError && args.Count == 1)
		{
			ReportRunError(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnFileMenuItem && args.Count == 1)
		{
			OnFileMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnEditMenuItem && args.Count == 1)
		{
			OnEditMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnModMenuItem && args.Count == 1)
		{
			OnModMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._UnhandledInput && args.Count == 1)
		{
			_UnhandledInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDockOverlayZones && args.Count == 0)
		{
			UpdateDockOverlayZones();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCreateProjectDialog && args.Count == 0)
		{
			ShowCreateProjectDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCreateProjectConfirmed && args.Count == 0)
		{
			OnCreateProjectConfirmed();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowOpenProjectDialog && args.Count == 0)
		{
			ShowOpenProjectDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.OnOpenProjectFileSelected && args.Count == 1)
		{
			OnOpenProjectFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveActiveDocument && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(SaveActiveDocument());
			return true;
		}
		if (method == MethodName.SaveAllDocuments && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(SaveAllDocuments());
			return true;
		}
		if (method == MethodName.SaveCurrentProject && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SaveCurrentProject(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.ShowProjectSettingsDialog && args.Count == 0)
		{
			ShowProjectSettingsDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.OnProjectSettingsSaved && args.Count == 0)
		{
			OnProjectSettingsSaved();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateProjectTitle && args.Count == 0)
		{
			UpdateProjectTitle();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowExportDialog && args.Count == 0)
		{
			ShowExportDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.OnExportDirSelected && args.Count == 1)
		{
			OnExportDirSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateFileSystemToProject && args.Count == 1)
		{
			NavigateFileSystemToProject(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowToast && args.Count == 1)
		{
			ShowToast(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnEditorActionCommitted && args.Count == 1)
		{
			OnEditorActionCommitted(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnEditorActionUndone && args.Count == 1)
		{
			OnEditorActionUndone(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnEditorActionRedone && args.Count == 1)
		{
			OnEditorActionRedone(VariantUtils.ConvertTo<string>(in args[0]));
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
		if (method == MethodName.GetOutputPanel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWOutputPanel>(GetOutputPanel());
			return true;
		}
		if (method == MethodName.GetRunBar && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWEditorRunBar>(GetRunBar());
			return true;
		}
		if (method == MethodName.GetModToolsPanel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWModToolsPanel>(GetModToolsPanel());
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.RegisterBuiltinEditorFactories && args.Count == 0)
		{
			RegisterBuiltinEditorFactories();
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
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.ApplyResponsiveWorkspaceLayout)
		{
			return true;
		}
		if (method == MethodName.BuildMenus)
		{
			return true;
		}
		if (method == MethodName.ShowLoadingScreen)
		{
			return true;
		}
		if (method == MethodName.StartAsyncInit)
		{
			return true;
		}
		if (method == MethodName.LoadProjectManagerPanel)
		{
			return true;
		}
		if (method == MethodName.ShowProjectManager)
		{
			return true;
		}
		if (method == MethodName.OpenProjectFromManager)
		{
			return true;
		}
		if (method == MethodName.OnMainScreenChanged)
		{
			return true;
		}
		if (method == MethodName.PlayWorkspaceTransition)
		{
			return true;
		}
		if (method == MethodName.RegisterBuiltinEditorFactories)
		{
			return true;
		}
		if (method == MethodName.LoadResourceEditorPanels)
		{
			return true;
		}
		if (method == MethodName.EnsureResourceEditor)
		{
			return true;
		}
		if (method == MethodName.ConnectRunBarSignals)
		{
			return true;
		}
		if (method == MethodName.OnRunMainPressed)
		{
			return true;
		}
		if (method == MethodName.OnRunCurrentScenePressed)
		{
			return true;
		}
		if (method == MethodName.OnRunCustomPressed)
		{
			return true;
		}
		if (method == MethodName.OnRunStopPressed)
		{
			return true;
		}
		if (method == MethodName.OnRunPauseToggled)
		{
			return true;
		}
		if (method == MethodName.RunCurrent2DScenePreview)
		{
			return true;
		}
		if (method == MethodName.CreatePackedSceneFromCurrent2D)
		{
			return true;
		}
		if (method == MethodName.FindProjectMainScenePath)
		{
			return true;
		}
		if (method == MethodName.ShowRunCustomSceneDialog)
		{
			return true;
		}
		if (method == MethodName.OnRunCustomSceneSelected)
		{
			return true;
		}
		if (method == MethodName.StartRunPreview)
		{
			return true;
		}
		if (method == MethodName.ShowRunPreviewWindow)
		{
			return true;
		}
		if (method == MethodName.StopRunPreview)
		{
			return true;
		}
		if (method == MethodName.CacheRunPreviewProcessModes)
		{
			return true;
		}
		if (method == MethodName.CacheRunPreviewProcessModesRecursive)
		{
			return true;
		}
		if (method == MethodName.SetRunPreviewPaused)
		{
			return true;
		}
		if (method == MethodName.GetCurrent2DSceneDisplayName)
		{
			return true;
		}
		if (method == MethodName.LogRunMessage)
		{
			return true;
		}
		if (method == MethodName.ReportRunError)
		{
			return true;
		}
		if (method == MethodName.OnFileMenuItem)
		{
			return true;
		}
		if (method == MethodName.OnEditMenuItem)
		{
			return true;
		}
		if (method == MethodName.OnModMenuItem)
		{
			return true;
		}
		if (method == MethodName._UnhandledInput)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.UpdateDockOverlayZones)
		{
			return true;
		}
		if (method == MethodName.ShowCreateProjectDialog)
		{
			return true;
		}
		if (method == MethodName.OnCreateProjectConfirmed)
		{
			return true;
		}
		if (method == MethodName.ShowOpenProjectDialog)
		{
			return true;
		}
		if (method == MethodName.OnOpenProjectFileSelected)
		{
			return true;
		}
		if (method == MethodName.SaveActiveDocument)
		{
			return true;
		}
		if (method == MethodName.SaveAllDocuments)
		{
			return true;
		}
		if (method == MethodName.SaveCurrentProject)
		{
			return true;
		}
		if (method == MethodName.ShowProjectSettingsDialog)
		{
			return true;
		}
		if (method == MethodName.OnProjectSettingsSaved)
		{
			return true;
		}
		if (method == MethodName.UpdateProjectTitle)
		{
			return true;
		}
		if (method == MethodName.ShowExportDialog)
		{
			return true;
		}
		if (method == MethodName.OnExportDirSelected)
		{
			return true;
		}
		if (method == MethodName.NavigateFileSystemToProject)
		{
			return true;
		}
		if (method == MethodName.ShowToast)
		{
			return true;
		}
		if (method == MethodName.OnEditorActionCommitted)
		{
			return true;
		}
		if (method == MethodName.OnEditorActionUndone)
		{
			return true;
		}
		if (method == MethodName.OnEditorActionRedone)
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
		if (method == MethodName.GetOutputPanel)
		{
			return true;
		}
		if (method == MethodName.GetRunBar)
		{
			return true;
		}
		if (method == MethodName.GetModToolsPanel)
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
		if (name == PropertyName._panelRegistry)
		{
			_panelRegistry = VariantUtils.ConvertTo<XWPanelRegistry>(in value);
			return true;
		}
		if (name == PropertyName._titleBar)
		{
			_titleBar = VariantUtils.ConvertTo<XWEditorTitleBar>(in value);
			return true;
		}
		if (name == PropertyName._workspaceSplit)
		{
			_workspaceSplit = VariantUtils.ConvertTo<HSplitContainer>(in value);
			return true;
		}
		if (name == PropertyName._leftDock)
		{
			_leftDock = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._leftBottomDock)
		{
			_leftBottomDock = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._leftTopRightDock)
		{
			_leftTopRightDock = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._leftBottomRightDock)
		{
			_leftBottomRightDock = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._centerDock)
		{
			_centerDock = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._centerWorkspaceMotion)
		{
			_centerWorkspaceMotion = VariantUtils.ConvertTo<XWUiMotion>(in value);
			return true;
		}
		if (name == PropertyName._bottomPanel)
		{
			_bottomPanel = VariantUtils.ConvertTo<XWBottomPanel>(in value);
			return true;
		}
		if (name == PropertyName._bottomDock)
		{
			_bottomDock = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._rightDock)
		{
			_rightDock = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._rightBottomDock)
		{
			_rightBottomDock = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._rightTopRightDock)
		{
			_rightTopRightDock = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._rightBottomRightDock)
		{
			_rightBottomRightDock = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._rightPanelTabs)
		{
			_rightPanelTabs = VariantUtils.ConvertTo<TabContainer>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._fileSystemPanel)
		{
			_fileSystemPanel = VariantUtils.ConvertTo<XWFileSystemPanel>(in value);
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
		if (name == PropertyName._inspector)
		{
			_inspector = VariantUtils.ConvertTo<XWInspector>(in value);
			return true;
		}
		if (name == PropertyName._bpEditor)
		{
			_bpEditor = VariantUtils.ConvertTo<XWBPEditor>(in value);
			return true;
		}
		if (name == PropertyName._scriptEditor)
		{
			_scriptEditor = VariantUtils.ConvertTo<XWScriptEditor>(in value);
			return true;
		}
		if (name == PropertyName._issuePanel)
		{
			_issuePanel = VariantUtils.ConvertTo<XWIssuePanel>(in value);
			return true;
		}
		if (name == PropertyName._modToolsPanel)
		{
			_modToolsPanel = VariantUtils.ConvertTo<XWModToolsPanel>(in value);
			return true;
		}
		if (name == PropertyName._projectManagerPanel)
		{
			_projectManagerPanel = VariantUtils.ConvertTo<XWModProjectManagerPanel>(in value);
			return true;
		}
		if (name == PropertyName._runBar)
		{
			_runBar = VariantUtils.ConvertTo<XWEditorRunBar>(in value);
			return true;
		}
		if (name == PropertyName._outputPanel)
		{
			_outputPanel = VariantUtils.ConvertTo<XWOutputPanel>(in value);
			return true;
		}
		if (name == PropertyName._toaster)
		{
			_toaster = VariantUtils.ConvertTo<XWEditorToaster>(in value);
			return true;
		}
		if (name == PropertyName._runPreviewWindow)
		{
			_runPreviewWindow = VariantUtils.ConvertTo<Window>(in value);
			return true;
		}
		if (name == PropertyName._runPreviewViewport)
		{
			_runPreviewViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._runPreviewRoot)
		{
			_runPreviewRoot = VariantUtils.ConvertTo<Node>(in value);
			return true;
		}
		if (name == PropertyName._runCustomSceneDialog)
		{
			_runCustomSceneDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._loadingOverlay)
		{
			_loadingOverlay = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._loadingBar)
		{
			_loadingBar = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._loadingLabel)
		{
			_loadingLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._dockOverlay)
		{
			_dockOverlay = VariantUtils.ConvertTo<XWDockOverlay>(in value);
			return true;
		}
		if (name == PropertyName._isDockDragging)
		{
			_isDockDragging = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._dragPanelKey)
		{
			_dragPanelKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._createProjectDialog)
		{
			_createProjectDialog = VariantUtils.ConvertTo<ModProjectCreateDialog>(in value);
			return true;
		}
		if (name == PropertyName._projectSettingsDialog)
		{
			_projectSettingsDialog = VariantUtils.ConvertTo<XWProjectSettingsDialog>(in value);
			return true;
		}
		if (name == PropertyName._openProjectFileDialog)
		{
			_openProjectFileDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._exportDirDialog)
		{
			_exportDirDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._isExporting)
		{
			_isExporting = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.LoadedResourceEditorCount)
		{
			value = VariantUtils.CreateFrom<int>(LoadedResourceEditorCount);
			return true;
		}
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
		if (name == PropertyName._panelRegistry)
		{
			value = VariantUtils.CreateFrom(in _panelRegistry);
			return true;
		}
		if (name == PropertyName._titleBar)
		{
			value = VariantUtils.CreateFrom(in _titleBar);
			return true;
		}
		if (name == PropertyName._workspaceSplit)
		{
			value = VariantUtils.CreateFrom(in _workspaceSplit);
			return true;
		}
		if (name == PropertyName._leftDock)
		{
			value = VariantUtils.CreateFrom(in _leftDock);
			return true;
		}
		if (name == PropertyName._leftBottomDock)
		{
			value = VariantUtils.CreateFrom(in _leftBottomDock);
			return true;
		}
		if (name == PropertyName._leftTopRightDock)
		{
			value = VariantUtils.CreateFrom(in _leftTopRightDock);
			return true;
		}
		if (name == PropertyName._leftBottomRightDock)
		{
			value = VariantUtils.CreateFrom(in _leftBottomRightDock);
			return true;
		}
		if (name == PropertyName._centerDock)
		{
			value = VariantUtils.CreateFrom(in _centerDock);
			return true;
		}
		if (name == PropertyName._centerWorkspaceMotion)
		{
			value = VariantUtils.CreateFrom(in _centerWorkspaceMotion);
			return true;
		}
		if (name == PropertyName._bottomPanel)
		{
			value = VariantUtils.CreateFrom(in _bottomPanel);
			return true;
		}
		if (name == PropertyName._bottomDock)
		{
			value = VariantUtils.CreateFrom(in _bottomDock);
			return true;
		}
		if (name == PropertyName._rightDock)
		{
			value = VariantUtils.CreateFrom(in _rightDock);
			return true;
		}
		if (name == PropertyName._rightBottomDock)
		{
			value = VariantUtils.CreateFrom(in _rightBottomDock);
			return true;
		}
		if (name == PropertyName._rightTopRightDock)
		{
			value = VariantUtils.CreateFrom(in _rightTopRightDock);
			return true;
		}
		if (name == PropertyName._rightBottomRightDock)
		{
			value = VariantUtils.CreateFrom(in _rightBottomRightDock);
			return true;
		}
		if (name == PropertyName._rightPanelTabs)
		{
			value = VariantUtils.CreateFrom(in _rightPanelTabs);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._fileSystemPanel)
		{
			value = VariantUtils.CreateFrom(in _fileSystemPanel);
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
		if (name == PropertyName._inspector)
		{
			value = VariantUtils.CreateFrom(in _inspector);
			return true;
		}
		if (name == PropertyName._bpEditor)
		{
			value = VariantUtils.CreateFrom(in _bpEditor);
			return true;
		}
		if (name == PropertyName._scriptEditor)
		{
			value = VariantUtils.CreateFrom(in _scriptEditor);
			return true;
		}
		if (name == PropertyName._issuePanel)
		{
			value = VariantUtils.CreateFrom(in _issuePanel);
			return true;
		}
		if (name == PropertyName._modToolsPanel)
		{
			value = VariantUtils.CreateFrom(in _modToolsPanel);
			return true;
		}
		if (name == PropertyName._projectManagerPanel)
		{
			value = VariantUtils.CreateFrom(in _projectManagerPanel);
			return true;
		}
		if (name == PropertyName._runBar)
		{
			value = VariantUtils.CreateFrom(in _runBar);
			return true;
		}
		if (name == PropertyName._outputPanel)
		{
			value = VariantUtils.CreateFrom(in _outputPanel);
			return true;
		}
		if (name == PropertyName._toaster)
		{
			value = VariantUtils.CreateFrom(in _toaster);
			return true;
		}
		if (name == PropertyName._runPreviewWindow)
		{
			value = VariantUtils.CreateFrom(in _runPreviewWindow);
			return true;
		}
		if (name == PropertyName._runPreviewViewport)
		{
			value = VariantUtils.CreateFrom(in _runPreviewViewport);
			return true;
		}
		if (name == PropertyName._runPreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _runPreviewRoot);
			return true;
		}
		if (name == PropertyName._runCustomSceneDialog)
		{
			value = VariantUtils.CreateFrom(in _runCustomSceneDialog);
			return true;
		}
		if (name == PropertyName._loadingOverlay)
		{
			value = VariantUtils.CreateFrom(in _loadingOverlay);
			return true;
		}
		if (name == PropertyName._loadingBar)
		{
			value = VariantUtils.CreateFrom(in _loadingBar);
			return true;
		}
		if (name == PropertyName._loadingLabel)
		{
			value = VariantUtils.CreateFrom(in _loadingLabel);
			return true;
		}
		if (name == PropertyName._dockOverlay)
		{
			value = VariantUtils.CreateFrom(in _dockOverlay);
			return true;
		}
		if (name == PropertyName._isDockDragging)
		{
			value = VariantUtils.CreateFrom(in _isDockDragging);
			return true;
		}
		if (name == PropertyName._dragPanelKey)
		{
			value = VariantUtils.CreateFrom(in _dragPanelKey);
			return true;
		}
		if (name == PropertyName._createProjectDialog)
		{
			value = VariantUtils.CreateFrom(in _createProjectDialog);
			return true;
		}
		if (name == PropertyName._projectSettingsDialog)
		{
			value = VariantUtils.CreateFrom(in _projectSettingsDialog);
			return true;
		}
		if (name == PropertyName._openProjectFileDialog)
		{
			value = VariantUtils.CreateFrom(in _openProjectFileDialog);
			return true;
		}
		if (name == PropertyName._exportDirDialog)
		{
			value = VariantUtils.CreateFrom(in _exportDirDialog);
			return true;
		}
		if (name == PropertyName._isExporting)
		{
			value = VariantUtils.CreateFrom(in _isExporting);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._panelRegistry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._titleBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._workspaceSplit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._leftDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._leftBottomDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._leftTopRightDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._leftBottomRightDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._centerDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._centerWorkspaceMotion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bottomPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bottomDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rightDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rightBottomDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rightTopRightDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rightBottomRightDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rightPanelTabs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fileSystemPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sceneTreeDock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scene2DEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bpEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scriptEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._issuePanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._modToolsPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectManagerPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._outputPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._toaster, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runPreviewWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runPreviewViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runPreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runCustomSceneDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.LoadedResourceEditorCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadingOverlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadingBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadingLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._dockOverlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isDockDragging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._dragPanelKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._createProjectDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._projectSettingsDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openProjectFileDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._exportDirDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isExporting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
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
		info.AddProperty(PropertyName._panelRegistry, Variant.From(in _panelRegistry));
		info.AddProperty(PropertyName._titleBar, Variant.From(in _titleBar));
		info.AddProperty(PropertyName._workspaceSplit, Variant.From(in _workspaceSplit));
		info.AddProperty(PropertyName._leftDock, Variant.From(in _leftDock));
		info.AddProperty(PropertyName._leftBottomDock, Variant.From(in _leftBottomDock));
		info.AddProperty(PropertyName._leftTopRightDock, Variant.From(in _leftTopRightDock));
		info.AddProperty(PropertyName._leftBottomRightDock, Variant.From(in _leftBottomRightDock));
		info.AddProperty(PropertyName._centerDock, Variant.From(in _centerDock));
		info.AddProperty(PropertyName._centerWorkspaceMotion, Variant.From(in _centerWorkspaceMotion));
		info.AddProperty(PropertyName._bottomPanel, Variant.From(in _bottomPanel));
		info.AddProperty(PropertyName._bottomDock, Variant.From(in _bottomDock));
		info.AddProperty(PropertyName._rightDock, Variant.From(in _rightDock));
		info.AddProperty(PropertyName._rightBottomDock, Variant.From(in _rightBottomDock));
		info.AddProperty(PropertyName._rightTopRightDock, Variant.From(in _rightTopRightDock));
		info.AddProperty(PropertyName._rightBottomRightDock, Variant.From(in _rightBottomRightDock));
		info.AddProperty(PropertyName._rightPanelTabs, Variant.From(in _rightPanelTabs));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._fileSystemPanel, Variant.From(in _fileSystemPanel));
		info.AddProperty(PropertyName._sceneTreeDock, Variant.From(in _sceneTreeDock));
		info.AddProperty(PropertyName._scene2DEditor, Variant.From(in _scene2DEditor));
		info.AddProperty(PropertyName._inspector, Variant.From(in _inspector));
		info.AddProperty(PropertyName._bpEditor, Variant.From(in _bpEditor));
		info.AddProperty(PropertyName._scriptEditor, Variant.From(in _scriptEditor));
		info.AddProperty(PropertyName._issuePanel, Variant.From(in _issuePanel));
		info.AddProperty(PropertyName._modToolsPanel, Variant.From(in _modToolsPanel));
		info.AddProperty(PropertyName._projectManagerPanel, Variant.From(in _projectManagerPanel));
		info.AddProperty(PropertyName._runBar, Variant.From(in _runBar));
		info.AddProperty(PropertyName._outputPanel, Variant.From(in _outputPanel));
		info.AddProperty(PropertyName._toaster, Variant.From(in _toaster));
		info.AddProperty(PropertyName._runPreviewWindow, Variant.From(in _runPreviewWindow));
		info.AddProperty(PropertyName._runPreviewViewport, Variant.From(in _runPreviewViewport));
		info.AddProperty(PropertyName._runPreviewRoot, Variant.From(in _runPreviewRoot));
		info.AddProperty(PropertyName._runCustomSceneDialog, Variant.From(in _runCustomSceneDialog));
		info.AddProperty(PropertyName._loadingOverlay, Variant.From(in _loadingOverlay));
		info.AddProperty(PropertyName._loadingBar, Variant.From(in _loadingBar));
		info.AddProperty(PropertyName._loadingLabel, Variant.From(in _loadingLabel));
		info.AddProperty(PropertyName._dockOverlay, Variant.From(in _dockOverlay));
		info.AddProperty(PropertyName._isDockDragging, Variant.From(in _isDockDragging));
		info.AddProperty(PropertyName._dragPanelKey, Variant.From(in _dragPanelKey));
		info.AddProperty(PropertyName._createProjectDialog, Variant.From(in _createProjectDialog));
		info.AddProperty(PropertyName._projectSettingsDialog, Variant.From(in _projectSettingsDialog));
		info.AddProperty(PropertyName._openProjectFileDialog, Variant.From(in _openProjectFileDialog));
		info.AddProperty(PropertyName._exportDirDialog, Variant.From(in _exportDirDialog));
		info.AddProperty(PropertyName._isExporting, Variant.From(in _isExporting));
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
		if (info.TryGetProperty(PropertyName._panelRegistry, out var value7))
		{
			_panelRegistry = value7.As<XWPanelRegistry>();
		}
		if (info.TryGetProperty(PropertyName._titleBar, out var value8))
		{
			_titleBar = value8.As<XWEditorTitleBar>();
		}
		if (info.TryGetProperty(PropertyName._workspaceSplit, out var value9))
		{
			_workspaceSplit = value9.As<HSplitContainer>();
		}
		if (info.TryGetProperty(PropertyName._leftDock, out var value10))
		{
			_leftDock = value10.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._leftBottomDock, out var value11))
		{
			_leftBottomDock = value11.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._leftTopRightDock, out var value12))
		{
			_leftTopRightDock = value12.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._leftBottomRightDock, out var value13))
		{
			_leftBottomRightDock = value13.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._centerDock, out var value14))
		{
			_centerDock = value14.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._centerWorkspaceMotion, out var value15))
		{
			_centerWorkspaceMotion = value15.As<XWUiMotion>();
		}
		if (info.TryGetProperty(PropertyName._bottomPanel, out var value16))
		{
			_bottomPanel = value16.As<XWBottomPanel>();
		}
		if (info.TryGetProperty(PropertyName._bottomDock, out var value17))
		{
			_bottomDock = value17.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._rightDock, out var value18))
		{
			_rightDock = value18.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._rightBottomDock, out var value19))
		{
			_rightBottomDock = value19.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._rightTopRightDock, out var value20))
		{
			_rightTopRightDock = value20.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._rightBottomRightDock, out var value21))
		{
			_rightBottomRightDock = value21.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._rightPanelTabs, out var value22))
		{
			_rightPanelTabs = value22.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value23))
		{
			_statusLabel = value23.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._fileSystemPanel, out var value24))
		{
			_fileSystemPanel = value24.As<XWFileSystemPanel>();
		}
		if (info.TryGetProperty(PropertyName._sceneTreeDock, out var value25))
		{
			_sceneTreeDock = value25.As<XWSceneTreeDock>();
		}
		if (info.TryGetProperty(PropertyName._scene2DEditor, out var value26))
		{
			_scene2DEditor = value26.As<XW2DSceneEditor>();
		}
		if (info.TryGetProperty(PropertyName._inspector, out var value27))
		{
			_inspector = value27.As<XWInspector>();
		}
		if (info.TryGetProperty(PropertyName._bpEditor, out var value28))
		{
			_bpEditor = value28.As<XWBPEditor>();
		}
		if (info.TryGetProperty(PropertyName._scriptEditor, out var value29))
		{
			_scriptEditor = value29.As<XWScriptEditor>();
		}
		if (info.TryGetProperty(PropertyName._issuePanel, out var value30))
		{
			_issuePanel = value30.As<XWIssuePanel>();
		}
		if (info.TryGetProperty(PropertyName._modToolsPanel, out var value31))
		{
			_modToolsPanel = value31.As<XWModToolsPanel>();
		}
		if (info.TryGetProperty(PropertyName._projectManagerPanel, out var value32))
		{
			_projectManagerPanel = value32.As<XWModProjectManagerPanel>();
		}
		if (info.TryGetProperty(PropertyName._runBar, out var value33))
		{
			_runBar = value33.As<XWEditorRunBar>();
		}
		if (info.TryGetProperty(PropertyName._outputPanel, out var value34))
		{
			_outputPanel = value34.As<XWOutputPanel>();
		}
		if (info.TryGetProperty(PropertyName._toaster, out var value35))
		{
			_toaster = value35.As<XWEditorToaster>();
		}
		if (info.TryGetProperty(PropertyName._runPreviewWindow, out var value36))
		{
			_runPreviewWindow = value36.As<Window>();
		}
		if (info.TryGetProperty(PropertyName._runPreviewViewport, out var value37))
		{
			_runPreviewViewport = value37.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._runPreviewRoot, out var value38))
		{
			_runPreviewRoot = value38.As<Node>();
		}
		if (info.TryGetProperty(PropertyName._runCustomSceneDialog, out var value39))
		{
			_runCustomSceneDialog = value39.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._loadingOverlay, out var value40))
		{
			_loadingOverlay = value40.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._loadingBar, out var value41))
		{
			_loadingBar = value41.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._loadingLabel, out var value42))
		{
			_loadingLabel = value42.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._dockOverlay, out var value43))
		{
			_dockOverlay = value43.As<XWDockOverlay>();
		}
		if (info.TryGetProperty(PropertyName._isDockDragging, out var value44))
		{
			_isDockDragging = value44.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._dragPanelKey, out var value45))
		{
			_dragPanelKey = value45.As<string>();
		}
		if (info.TryGetProperty(PropertyName._createProjectDialog, out var value46))
		{
			_createProjectDialog = value46.As<ModProjectCreateDialog>();
		}
		if (info.TryGetProperty(PropertyName._projectSettingsDialog, out var value47))
		{
			_projectSettingsDialog = value47.As<XWProjectSettingsDialog>();
		}
		if (info.TryGetProperty(PropertyName._openProjectFileDialog, out var value48))
		{
			_openProjectFileDialog = value48.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._exportDirDialog, out var value49))
		{
			_exportDirDialog = value49.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._isExporting, out var value50))
		{
			_isExporting = value50.As<bool>();
		}
	}
}
