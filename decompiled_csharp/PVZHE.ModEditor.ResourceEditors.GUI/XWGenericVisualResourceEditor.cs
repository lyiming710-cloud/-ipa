using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.Issues.GUI;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.ModSystem.References;
using PVZHE.ModEditor.ModSystem.Validation;
using PVZHE.ModEditor.OutPutPanel;
using PVZHE.ModEditor.PVZIntegration;
using PVZHE.ModEditor.Registry.Class;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;
using PVZHE.ModEditor.ScriptEditor;
using PVZHE.ModEditor.Tools;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/XWGenericVisualResourceEditor.cs")]
public class XWGenericVisualResourceEditor : PanelContainer
{
	private sealed class ResourceTabState
	{
		public Resource Resource;

		public string Path = "";

		public XWVisualEditorDescriptor Descriptor;

		public XWResourceEditContext Context;

		public string Key = "";

		public bool IsDirty;
	}

	private enum PropertyRefreshDomain
	{
		None,
		Scalar,
		Structure
	}

	public new class MethodName : PanelContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName EnableVisibilityGatedProcessing = "EnableVisibilityGatedProcessing";

		public static readonly StringName RequestVisibilityGatedProcessing = "RequestVisibilityGatedProcessing";

		public static readonly StringName RefreshVisibilityGatedProcessing = "RefreshVisibilityGatedProcessing";

		public static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public static readonly StringName HandleVisualEditorVisibilityChanged = "HandleVisualEditorVisibilityChanged";

		public static readonly StringName DisableVisibilityGatedProcessing = "DisableVisibilityGatedProcessing";

		public static readonly StringName LoadResource = "LoadResource";

		public static readonly StringName BindSceneNodes = "BindSceneNodes";

		public static readonly StringName ConnectVisualListNavigation = "ConnectVisualListNavigation";

		public static readonly StringName SelectResourceTab = "SelectResourceTab";

		public static readonly StringName CloseResourceTab = "CloseResourceTab";

		public static readonly StringName RefreshResourceTabs = "RefreshResourceTabs";

		public static readonly StringName UpdateCurrentResourceTabState = "UpdateCurrentResourceTabState";

		public static readonly StringName MarkCurrentResourceDirty = "MarkCurrentResourceDirty";

		public static readonly StringName NotifyCurrentResourceEdited = "NotifyCurrentResourceEdited";

		public static readonly StringName MarkCurrentResourceSaved = "MarkCurrentResourceSaved";

		public static readonly StringName MarkResourceDirty = "MarkResourceDirty";

		public static readonly StringName FindResourceTabIndex = "FindResourceTabIndex";

		public static readonly StringName FindResourceTabIndexByKey = "FindResourceTabIndexByKey";

		public static readonly StringName FindResourceTabIndexByResource = "FindResourceTabIndexByResource";

		public static readonly StringName BuildResourceTabKey = "BuildResourceTabKey";

		public static readonly StringName NormalizeResourceTabPath = "NormalizeResourceTabPath";

		public static readonly StringName Refresh = "Refresh";

		public static readonly StringName BindInlineTextSurface = "BindInlineTextSurface";

		public static readonly StringName OnInlineTextEdited = "OnInlineTextEdited";

		public static readonly StringName BindDirectPropertySurface = "BindDirectPropertySurface";

		public static readonly StringName OnDirectPropertyEdited = "OnDirectPropertyEdited";

		public static readonly StringName BuildContextSummary = "BuildContextSummary";

		public static readonly StringName NormalizeReferenceGraphKey = "NormalizeReferenceGraphKey";

		public static readonly StringName OpenResourceLibraryPicker = "OpenResourceLibraryPicker";

		public static readonly StringName OpenNativeResourceDialogFallback = "OpenNativeResourceDialogFallback";

		public static readonly StringName OnResourceFileSelected = "OnResourceFileSelected";

		public static readonly StringName LoadResourceFromPath = "LoadResourceFromPath";

		public static readonly StringName OnEmbeddedInspectorPropertyChanged = "OnEmbeddedInspectorPropertyChanged";

		public static readonly StringName QueueResourceEditorRefresh = "QueueResourceEditorRefresh";

		public static readonly StringName RefreshAfterDirectPropertyChange = "RefreshAfterDirectPropertyChange";

		public static readonly StringName RefreshPropertyDomain = "RefreshPropertyDomain";

		public static readonly StringName RefreshIncrementalPreviewData = "RefreshIncrementalPreviewData";

		public static readonly StringName DeterminePropertyRefreshDomain = "DeterminePropertyRefreshDomain";

		public static readonly StringName IsStructuralVariant = "IsStructuralVariant";

		public static readonly StringName RememberCurrentHistoryRefreshDomain = "RememberCurrentHistoryRefreshDomain";

		public static readonly StringName TrimHistoryRefreshDomains = "TrimHistoryRefreshDomains";

		public static readonly StringName ResolveHistoryRefreshDomain = "ResolveHistoryRefreshDomain";

		public static readonly StringName HandleToolbarAction = "HandleToolbarAction";

		public static readonly StringName SaveCurrentResource = "SaveCurrentResource";

		public static readonly StringName TrySaveCurrentResource = "TrySaveCurrentResource";

		public static readonly StringName SaveActiveResource = "SaveActiveResource";

		public static readonly StringName SaveAllOpenResources = "SaveAllOpenResources";

		public static readonly StringName SaveCurrentResourceAs = "SaveCurrentResourceAs";

		public static readonly StringName PrepareProjectResourceDialog = "PrepareProjectResourceDialog";

		public static readonly StringName IsInsideProject = "IsInsideProject";

		public static readonly StringName OnSaveResourceAsFileSelected = "OnSaveResourceAsFileSelected";

		public static readonly StringName TrySaveCurrentResourceAs = "TrySaveCurrentResourceAs";

		public static readonly StringName RegisterCurrentResourceInManifest = "RegisterCurrentResourceInManifest";

		public static readonly StringName CopyCurrentResourcePath = "CopyCurrentResourcePath";

		public static readonly StringName ShowCurrentResourceInFileSystem = "ShowCurrentResourceInFileSystem";

		public static readonly StringName MakeCurrentResourceBuiltIn = "MakeCurrentResourceBuiltIn";

		public static readonly StringName UndoLastChange = "UndoLastChange";

		public static readonly StringName RedoLastChange = "RedoLastChange";

		public static readonly StringName ToggleAutosave = "ToggleAutosave";

		public static readonly StringName RunReleaseCheck = "RunReleaseCheck";

		public static readonly StringName SaveResourceDraft = "SaveResourceDraft";

		public static readonly StringName RecoverResourceDraft = "RecoverResourceDraft";

		public static readonly StringName ResolveSaveAsTarget = "ResolveSaveAsTarget";

		public static readonly StringName HasNestedEditContext = "HasNestedEditContext";

		public static readonly StringName ReplaceRecoveredInOwner = "ReplaceRecoveredInOwner";

		public static readonly StringName OnCurrentResourceSaved = "OnCurrentResourceSaved";

		public static readonly StringName OnCurrentResourceDraftRecovered = "OnCurrentResourceDraftRecovered";

		public static readonly StringName RefreshResourceIfOpen = "RefreshResourceIfOpen";

		public static readonly StringName GetCurrentResourcePath = "GetCurrentResourcePath";

		public static readonly StringName GetAutosaveId = "GetAutosaveId";

		public static readonly StringName GetResourcePath = "GetResourcePath";

		public static readonly StringName GetCurrentProjectPath = "GetCurrentProjectPath";

		public static readonly StringName ClearAllSurfaces = "ClearAllSurfaces";

		public static readonly StringName AddMainPanelMessage = "AddMainPanelMessage";

		public static readonly StringName RegisterEmbeddedReferenceTarget = "RegisterEmbeddedReferenceTarget";

		public static readonly StringName ShouldSkipEmbeddedSubResourceProperty = "ShouldSkipEmbeddedSubResourceProperty";

		public static readonly StringName OpenReferencedItem = "OpenReferencedItem";

		public static readonly StringName OpenVisualListItem = "OpenVisualListItem";

		public static readonly StringName TryOpenReferenceRowText = "TryOpenReferenceRowText";

		public static readonly StringName GetReferenceTarget = "GetReferenceTarget";

		public static readonly StringName TryOpenReferenceTarget = "TryOpenReferenceTarget";

		public static readonly StringName TryOpenEmbeddedReferenceTarget = "TryOpenEmbeddedReferenceTarget";

		public static readonly StringName ResolveModRelativeReferenceTarget = "ResolveModRelativeReferenceTarget";

		public static readonly StringName TryOpenScriptTarget = "TryOpenScriptTarget";

		public static readonly StringName TryOpenResourceTarget = "TryOpenResourceTarget";

		public static readonly StringName TryOpenKeyReferenceTarget = "TryOpenKeyReferenceTarget";

		public static readonly StringName NormalizeReferenceTarget = "NormalizeReferenceTarget";

		public static readonly StringName StripReferenceDecoration = "StripReferenceDecoration";

		public static readonly StringName LogResourceEditorAction = "LogResourceEditorAction";
	}

	public new class PropertyName : PanelContainer.PropertyName
	{
		public static readonly StringName CurrentResource = "CurrentResource";

		public static readonly StringName CurrentResourcePath = "CurrentResourcePath";

		public static readonly StringName ActiveResource = "ActiveResource";

		public static readonly StringName ActiveResourcePath = "ActiveResourcePath";

		public static readonly StringName MainPanel = "MainPanel";

		public static readonly StringName CanvasGrid = "CanvasGrid";

		public static readonly StringName TimelineList = "TimelineList";

		public static readonly StringName GraphList = "GraphList";

		public static readonly StringName PreviewList = "PreviewList";

		public static readonly StringName ReferenceList = "ReferenceList";

		public static readonly StringName DirectEditablePropertyCount = "DirectEditablePropertyCount";

		public static readonly StringName DirectMountedPropertyCount = "DirectMountedPropertyCount";

		public static readonly StringName DirectMissingPropertyCount = "DirectMissingPropertyCount";

		public static readonly StringName InlineSemanticTextCount = "InlineSemanticTextCount";

		public static readonly StringName InlineCoveredTextCount = "InlineCoveredTextCount";

		public static readonly StringName InlineTechnicalTextCount = "InlineTechnicalTextCount";

		public static readonly StringName SupportsIncrementalPropertyRefresh = "SupportsIncrementalPropertyRefresh";

		public static readonly StringName _resource = "_resource";

		public static readonly StringName _path = "_path";

		public static readonly StringName _autosaveEnabled = "_autosaveEnabled";

		public static readonly StringName _title = "_title";

		public static readonly StringName _summary = "_summary";

		public static readonly StringName _toolbar = "_toolbar";

		public static readonly StringName _resourceTabBar = "_resourceTabBar";

		public static readonly StringName _mainPanel = "_mainPanel";

		public static readonly StringName _canvasGrid = "_canvasGrid";

		public static readonly StringName _inlineTextSurface = "_inlineTextSurface";

		public static readonly StringName _directPropertiesHost = "_directPropertiesHost";

		public static readonly StringName _directPropertySurface = "_directPropertySurface";

		public static readonly StringName _universalResourcePreview = "_universalResourcePreview";

		public static readonly StringName _timelineList = "_timelineList";

		public static readonly StringName _graphList = "_graphList";

		public static readonly StringName _previewList = "_previewList";

		public static readonly StringName _referenceList = "_referenceList";

		public static readonly StringName _loadResourceDialog = "_loadResourceDialog";

		public static readonly StringName _saveResourceAsDialog = "_saveResourceAsDialog";

		public static readonly StringName _resourceLibraryPicker = "_resourceLibraryPicker";

		public static readonly StringName _updatingResourceTabs = "_updatingResourceTabs";

		public static readonly StringName _resourceEditorRefreshQueued = "_resourceEditorRefreshQueued";

		public static readonly StringName _queuedPropertyRefreshDomain = "_queuedPropertyRefreshDomain";

		public static readonly StringName _visibilityGatedProcessingEnabled = "_visibilityGatedProcessingEnabled";

		public static readonly StringName _visibilityGatedProcessingRequested = "_visibilityGatedProcessingRequested";
	}

	public new class SignalName : PanelContainer.SignalName
	{
	}

	private const string UniversalResourcePreviewScenePath = "res://addons/ModEditor/ResourceEditors/GUI/XWUniversalResourcePreview.tscn";

	private const string UnsavedResourceTabPrefix = "(*)";

	private const string ResourceEditorSaveOutputLabel = "资源编辑器: 保存";

	private const string ResourceEditorJumpOutputLabel = "资源编辑器: 跳转引用";

	private const string ResourceEditorSubResourceOutputLabel = "资源编辑器: 打开子资源";

	private static PackedScene _universalResourcePreviewScene;

	private readonly XWAutosaveManager _autosaveManager = new XWAutosaveManager();

	private readonly List<ResourceTabState> _openResourceTabs = new List<ResourceTabState>();

	private readonly System.Collections.Generic.Dictionary<string, Resource> _embeddedReferenceTargets = new System.Collections.Generic.Dictionary<string, Resource>();

	private readonly System.Collections.Generic.Dictionary<long, PropertyRefreshDomain> _historyRefreshDomains = new System.Collections.Generic.Dictionary<long, PropertyRefreshDomain>();

	private Resource _resource;

	private string _path = "";

	private XWVisualEditorDescriptor _descriptor;

	private XWResourceEditContext _editContext;

	private bool _autosaveEnabled;

	private Label _title;

	private Label _summary;

	private HBoxContainer _toolbar;

	private TabBar _resourceTabBar;

	private Control _mainPanel;

	private GridContainer _canvasGrid;

	private XWInlineTextSurface _inlineTextSurface;

	private VBoxContainer _directPropertiesHost;

	private XWDirectPropertySurface _directPropertySurface;

	private XWUniversalResourcePreview _universalResourcePreview;

	private ItemList _timelineList;

	private ItemList _graphList;

	private ItemList _previewList;

	private ItemList _referenceList;

	private FileDialog _loadResourceDialog;

	private FileDialog _saveResourceAsDialog;

	private XWGameplayResourcePickerWindow _resourceLibraryPicker;

	private bool _updatingResourceTabs;

	private bool _resourceEditorRefreshQueued;

	private PropertyRefreshDomain _queuedPropertyRefreshDomain;

	private bool _visibilityGatedProcessingEnabled;

	private bool _visibilityGatedProcessingRequested;

	protected Resource CurrentResource => _resource;

	protected string CurrentResourcePath => GetCurrentResourcePath();

	public Resource ActiveResource => _resource;

	public string ActiveResourcePath => GetCurrentResourcePath();

	protected XWVisualEditorDescriptor CurrentDescriptor => _descriptor;

	protected XWResourceEditContext CurrentEditContext => _editContext;

	protected Control MainPanel => _mainPanel;

	protected GridContainer CanvasGrid => _canvasGrid;

	protected ItemList TimelineList => _timelineList;

	protected ItemList GraphList => _graphList;

	protected ItemList PreviewList => _previewList;

	protected ItemList ReferenceList => _referenceList;

	public int DirectEditablePropertyCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_directPropertySurface))
			{
				return 0;
			}
			return _directPropertySurface.EditablePropertyCount;
		}
	}

	public int DirectMountedPropertyCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_directPropertySurface))
			{
				return 0;
			}
			return _directPropertySurface.MountedPropertyCount;
		}
	}

	public int DirectMissingPropertyCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_directPropertySurface))
			{
				return 0;
			}
			return _directPropertySurface.MissingPropertyCount;
		}
	}

	public int InlineSemanticTextCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_inlineTextSurface))
			{
				return 0;
			}
			return _inlineTextSurface.SemanticTextCount;
		}
	}

	public int InlineCoveredTextCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_inlineTextSurface))
			{
				return 0;
			}
			return _inlineTextSurface.InlineCoveredCount;
		}
	}

	public int InlineTechnicalTextCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_inlineTextSurface))
			{
				return 0;
			}
			return _inlineTextSurface.TechnicalTextCount;
		}
	}

	protected virtual bool SupportsIncrementalPropertyRefresh => GetType() == typeof(XWGenericVisualResourceEditor);

	public override void _Ready()
	{
		EnableVisibilityGatedProcessing();
		BindSceneNodes();
		RefreshResourceTabs(FindResourceTabIndex(_resource, _path));
		Refresh();
	}

	public override void _ExitTree()
	{
		DisableVisibilityGatedProcessing();
		base._ExitTree();
	}

	protected void EnableVisibilityGatedProcessing()
	{
		if (!_visibilityGatedProcessingEnabled)
		{
			_visibilityGatedProcessingEnabled = true;
			VisibilityChanged += HandleVisualEditorVisibilityChanged;
			RefreshVisibilityGatedProcessing();
		}
	}

	protected void RequestVisibilityGatedProcessing(bool requested)
	{
		if (_visibilityGatedProcessingRequested != requested)
		{
			_visibilityGatedProcessingRequested = requested;
			RefreshVisibilityGatedProcessing();
		}
	}

	protected void RefreshVisibilityGatedProcessing()
	{
		if (_visibilityGatedProcessingEnabled)
		{
			bool flag = IsVisibleInTree();
			ProcessMode = (ProcessModeEnum)(flag ? 0 : 4);
			SetProcess(_visibilityGatedProcessingRequested & flag);
			OnVisualEditorVisibilityChanged(flag);
		}
	}

	protected virtual void OnVisualEditorVisibilityChanged(bool visible)
	{
	}

	private void HandleVisualEditorVisibilityChanged()
	{
		RefreshVisibilityGatedProcessing();
	}

	private void DisableVisibilityGatedProcessing()
	{
		if (_visibilityGatedProcessingEnabled)
		{
			VisibilityChanged -= HandleVisualEditorVisibilityChanged;
			_visibilityGatedProcessingEnabled = false;
			_visibilityGatedProcessingRequested = false;
			SetProcess(enable: false);
			ProcessMode = ProcessModeEnum.Disabled;
			OnVisualEditorVisibilityChanged(visible: false);
		}
	}

	public void LoadResource(string path)
	{
		Resource resource = (ResourceLoader.Exists(path) ? ResourceLoader.Load<Resource>(path, null, ResourceLoader.CacheMode.Reuse) : null);
		if (XWResourceEditorRegistry.TryGetEditor(resource, path, out var descriptor))
		{
			LoadResource(resource, path, descriptor);
		}
	}

	public void LoadResource(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		XWResourceEditContext context = XWResourceEditContext.ForRoot(resource, path, descriptor?.DockKey ?? "resource_editor");
		LoadResource(resource, path, descriptor, context);
	}

	public void LoadResource(Resource resource, string path, XWVisualEditorDescriptor descriptor, XWResourceEditContext context)
	{
		LoadResourceInternal(resource, path, descriptor, context, updateTabs: true);
	}

	private void LoadResourceInternal(Resource resource, string path, XWVisualEditorDescriptor descriptor, XWResourceEditContext context, bool updateTabs)
	{
		_resource = resource;
		_path = path ?? "";
		_descriptor = descriptor ?? _descriptor;
		_editContext = context ?? XWResourceEditContext.ForRoot(resource, _path, _descriptor?.DockKey ?? "resource_editor");
		if (updateTabs)
		{
			if (GodotObject.IsInstanceValid(_resource))
			{
				UpsertResourceTab(_resource, _path, _descriptor, _editContext);
			}
			else if (_openResourceTabs.Count == 0)
			{
				RefreshResourceTabs(-1);
			}
		}
		Refresh();
	}

	private void BindSceneNodes()
	{
		_title = GetNodeOrNull<Label>("%Title");
		_summary = GetNodeOrNull<Label>("%Summary");
		_toolbar = GetNodeOrNull<HBoxContainer>("%Toolbar");
		_resourceTabBar = GetNodeOrNull<TabBar>("%ResourceTabBar");
		_mainPanel = GetNodeOrNull<Control>("%MainPanel");
		_canvasGrid = GetNodeOrNull<GridContainer>("%CanvasGrid");
		_directPropertiesHost = GetNodeOrNull<VBoxContainer>("%DirectPropertiesHost");
		_timelineList = GetNodeOrNull<ItemList>("%TimelineList");
		_graphList = GetNodeOrNull<ItemList>("%GraphList");
		_previewList = GetNodeOrNull<ItemList>("%PreviewList");
		_referenceList = GetNodeOrNull<ItemList>("%ReferenceList");
		_loadResourceDialog = GetNode<FileDialog>("%LoadResourceDialog");
		_saveResourceAsDialog = GetNode<FileDialog>("%SaveResourceAsDialog");
		_loadResourceDialog.FileSelected += OnResourceFileSelected;
		_saveResourceAsDialog.FileSelected += OnSaveResourceAsFileSelected;
		_resourceLibraryPicker = XWGameplayResourcePickerWindow.Create();
		if (GodotObject.IsInstanceValid(_resourceLibraryPicker))
		{
			AddChild(_resourceLibraryPicker, forceReadableName: false, InternalMode.Disabled);
		}
		if (_resourceTabBar != null)
		{
			_resourceTabBar.TabChanged += SelectResourceTab;
			_resourceTabBar.TabClosePressed += CloseResourceTab;
		}
		if (_referenceList != null)
		{
			_referenceList.ItemActivated += OpenReferencedItem;
		}
		ConnectVisualListNavigation();
	}

	private void ConnectVisualListNavigation()
	{
		if (_timelineList != null)
		{
			_timelineList.ItemActivated += (long index) =>
			{
				OpenVisualListItem(_timelineList, index);
			};
		}
		if (_graphList != null)
		{
			_graphList.ItemActivated += (long index) =>
			{
				OpenVisualListItem(_graphList, index);
			};
		}
		if (_previewList != null)
		{
			_previewList.ItemActivated += (long index) =>
			{
				OpenVisualListItem(_previewList, index);
			};
		}
	}

	private void UpsertResourceTab(Resource resource, string path, XWVisualEditorDescriptor descriptor, XWResourceEditContext context)
	{
		if (GodotObject.IsInstanceValid(resource))
		{
			string text = NormalizeResourceTabPath(path);
			if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(resource.ResourcePath))
			{
				text = NormalizeResourceTabPath(resource.ResourcePath);
			}
			string key = BuildResourceTabKey(resource, text);
			int num = FindResourceTabIndexByKey(key);
			if (num < 0)
			{
				num = FindResourceTabIndexByResource(resource);
			}
			if (num < 0)
			{
				_openResourceTabs.Add(new ResourceTabState
				{
					Resource = resource,
					Path = text,
					Descriptor = descriptor,
					Context = context,
					Key = key,
					IsDirty = string.IsNullOrWhiteSpace(text)
				});
				num = _openResourceTabs.Count - 1;
			}
			else
			{
				_openResourceTabs[num].Resource = resource;
				_openResourceTabs[num].Path = text;
				_openResourceTabs[num].Descriptor = descriptor;
				_openResourceTabs[num].Context = context;
				_openResourceTabs[num].Key = key;
			}
			RefreshResourceTabs(num);
		}
	}

	private void SelectResourceTab(long tabIdx)
	{
		if (!_updatingResourceTabs && tabIdx >= 0 && tabIdx < _openResourceTabs.Count)
		{
			ResourceTabState resourceTabState = _openResourceTabs[(int)tabIdx];
			if (!GodotObject.IsInstanceValid(resourceTabState.Resource))
			{
				CloseResourceTab(tabIdx);
				return;
			}
			LoadResourceInternal(resourceTabState.Resource, resourceTabState.Path, resourceTabState.Descriptor, resourceTabState.Context, updateTabs: false);
			RefreshResourceTabs((int)tabIdx);
		}
	}

	private void CloseResourceTab(long tabIdx)
	{
		if (tabIdx < 0 || tabIdx >= _openResourceTabs.Count)
		{
			return;
		}
		bool flag = IsCurrentResourceTab(_openResourceTabs[(int)tabIdx]);
		_openResourceTabs.RemoveAt((int)tabIdx);
		if (_openResourceTabs.Count == 0)
		{
			_resource = null;
			_path = "";
			_editContext = null;
			RefreshResourceTabs(-1);
			Refresh();
			return;
		}
		int num = Mathf.Min((int)tabIdx, _openResourceTabs.Count - 1);
		RefreshResourceTabs(flag ? num : FindResourceTabIndex(_resource, _path));
		if (flag)
		{
			ResourceTabState resourceTabState = _openResourceTabs[num];
			LoadResourceInternal(resourceTabState.Resource, resourceTabState.Path, resourceTabState.Descriptor, resourceTabState.Context, updateTabs: false);
			RefreshResourceTabs(num);
		}
	}

	protected int CloseResourceTabs(Predicate<Resource> shouldClose)
	{
		if (shouldClose == null)
		{
			return 0;
		}
		bool flag = false;
		int num = 0;
		for (int num2 = _openResourceTabs.Count - 1; num2 >= 0; num2--)
		{
			ResourceTabState resourceTabState = _openResourceTabs[num2];
			if (!GodotObject.IsInstanceValid(resourceTabState.Resource) || shouldClose(resourceTabState.Resource))
			{
				flag |= IsCurrentResourceTab(resourceTabState);
				_openResourceTabs.RemoveAt(num2);
				num++;
			}
		}
		if (num == 0)
		{
			return 0;
		}
		if (_openResourceTabs.Count == 0)
		{
			_resource = null;
			_path = "";
			_editContext = null;
			RefreshResourceTabs(-1);
			Refresh();
			return num;
		}
		if (flag || !GodotObject.IsInstanceValid(_resource))
		{
			List<ResourceTabState> openResourceTabs = _openResourceTabs;
			ResourceTabState resourceTabState2 = openResourceTabs[openResourceTabs.Count - 1];
			LoadResourceInternal(resourceTabState2.Resource, resourceTabState2.Path, resourceTabState2.Descriptor, resourceTabState2.Context, updateTabs: false);
			RefreshResourceTabs(_openResourceTabs.Count - 1);
		}
		else
		{
			RefreshResourceTabs(FindResourceTabIndex(_resource, _path));
		}
		return num;
	}

	private void RefreshResourceTabs(int selectedIndex)
	{
		if (_resourceTabBar != null)
		{
			_updatingResourceTabs = true;
			_resourceTabBar.ClearTabs();
			_resourceTabBar.Visible = _openResourceTabs.Count > 0;
			for (int i = 0; i < _openResourceTabs.Count; i++)
			{
				ResourceTabState tab = _openResourceTabs[i];
				_resourceTabBar.AddTab(GetResourceTabTitle(tab), GetResourceTabIcon(tab));
				_resourceTabBar.SetTabTooltip(i, GetResourceTabTooltip(tab));
			}
			if (_openResourceTabs.Count > 0)
			{
				int num = Mathf.Clamp(selectedIndex, 0, _openResourceTabs.Count - 1);
				_resourceTabBar.CurrentTab = num;
				_resourceTabBar.EnsureTabVisible(num);
			}
			_updatingResourceTabs = false;
		}
	}

	private void UpdateCurrentResourceTabState()
	{
		if (GodotObject.IsInstanceValid(_resource))
		{
			int num = FindResourceTabIndexByResource(_resource);
			if (num >= 0)
			{
				string currentResourcePath = GetCurrentResourcePath();
				_openResourceTabs[num].Path = NormalizeResourceTabPath(currentResourcePath);
				_openResourceTabs[num].Descriptor = _descriptor;
				_openResourceTabs[num].Key = BuildResourceTabKey(_resource, _openResourceTabs[num].Path);
				RefreshResourceTabs(num);
			}
		}
	}

	protected void MarkCurrentResourceDirty()
	{
		MarkResourceDirty(_resource, GetCurrentResourcePath(), isDirty: true);
	}

	protected void NotifyCurrentResourceEdited(bool refreshVisuals = false)
	{
		if (GodotObject.IsInstanceValid(_resource))
		{
			MarkCurrentResourceDirty();
			_resource.EmitChanged();
			SaveResourceDraft();
			if (refreshVisuals)
			{
				QueueResourceEditorRefresh(PropertyRefreshDomain.Structure);
			}
		}
	}

	protected void MarkCurrentResourceSaved()
	{
		MarkResourceDirty(_resource, GetCurrentResourcePath(), isDirty: false);
	}

	protected void MarkResourceDirty(Resource resource, string path, bool isDirty)
	{
		if (GodotObject.IsInstanceValid(resource))
		{
			int num = FindResourceTabIndex(resource, path);
			if (num >= 0)
			{
				string path2 = NormalizeResourceTabPath(path);
				_openResourceTabs[num].Resource = resource;
				_openResourceTabs[num].Path = path2;
				_openResourceTabs[num].Descriptor = _descriptor;
				_openResourceTabs[num].Key = BuildResourceTabKey(resource, path2);
				_openResourceTabs[num].IsDirty = isDirty;
				RefreshResourceTabs(num);
			}
		}
	}

	private bool IsCurrentResourceTab(ResourceTabState tab)
	{
		if (tab == null || !GodotObject.IsInstanceValid(_resource))
		{
			return false;
		}
		if (tab.Resource != _resource)
		{
			return tab.Key == BuildResourceTabKey(_resource, _path);
		}
		return true;
	}

	private int FindResourceTabIndex(Resource resource, string path)
	{
		string key = BuildResourceTabKey(resource, NormalizeResourceTabPath(path));
		int num = FindResourceTabIndexByKey(key);
		if (num >= 0)
		{
			return num;
		}
		return FindResourceTabIndexByResource(resource);
	}

	private int FindResourceTabIndexByKey(string key)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			return -1;
		}
		for (int i = 0; i < _openResourceTabs.Count; i++)
		{
			if (_openResourceTabs[i].Key == key)
			{
				return i;
			}
		}
		return -1;
	}

	private int FindResourceTabIndexByResource(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return -1;
		}
		for (int i = 0; i < _openResourceTabs.Count; i++)
		{
			if (_openResourceTabs[i].Resource == resource)
			{
				return i;
			}
		}
		return -1;
	}

	private static string BuildResourceTabKey(Resource resource, string path)
	{
		string text = NormalizeResourceTabPath(path);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return "path:" + text;
		}
		if (GodotObject.IsInstanceValid(resource))
		{
			return $"instance:{resource.GetInstanceId()}";
		}
		return "";
	}

	private static string NormalizeResourceTabPath(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return path.Trim().Replace('\\', '/');
		}
		return "";
	}

	private static string GetResourceTabTitle(ResourceTabState tab)
	{
		string resourceTabBaseTitle = GetResourceTabBaseTitle(tab);
		if (!tab.IsDirty)
		{
			return resourceTabBaseTitle;
		}
		return "(*)" + resourceTabBaseTitle;
	}

	private static string GetResourceTabBaseTitle(ResourceTabState tab)
	{
		if (!string.IsNullOrWhiteSpace(tab.Path))
		{
			string file = tab.Path.GetFile();
			string baseName = file.GetBaseName();
			if (!string.IsNullOrWhiteSpace(baseName))
			{
				return baseName;
			}
			if (!string.IsNullOrWhiteSpace(file))
			{
				return file;
			}
		}
		if (GodotObject.IsInstanceValid(tab.Resource))
		{
			if (!string.IsNullOrWhiteSpace(tab.Resource.ResourceName))
			{
				return tab.Resource.ResourceName;
			}
			return tab.Resource.GetType().Name;
		}
		if (!string.IsNullOrWhiteSpace(tab.Descriptor?.DisplayName))
		{
			return tab.Descriptor.DisplayName;
		}
		return "Resource";
	}

	private static string GetResourceTabTooltip(ResourceTabState tab)
	{
		if (!string.IsNullOrWhiteSpace(tab.Path))
		{
			return tab.Path;
		}
		if (GodotObject.IsInstanceValid(tab.Resource))
		{
			return tab.Resource.GetType().Name;
		}
		return tab.Descriptor?.DisplayName ?? "";
	}

	private static Texture2D GetResourceTabIcon(ResourceTabState tab)
	{
		string className = (GodotObject.IsInstanceValid(tab.Resource) ? tab.Resource.GetType().Name : "Resource");
		return XWClassRegistry.Instance.GetClassIcon(className);
	}

	private void Refresh()
	{
		if (_title == null || _canvasGrid == null)
		{
			return;
		}
		ClearAllSurfaces();
		if (_descriptor == null)
		{
			BindInlineTextSurface(null);
			BindDirectPropertySurface(null);
			_title.Text = "资源编辑器";
			if (_summary != null)
			{
				_summary.Text = "打开关卡、地图、角色、卡片、子弹、动画、音频或 UI 资源后会显示对应的可视化编辑界面。";
			}
			AddMainPanelMessage("请选择一个支持的 Mod 资源。");
		}
		else
		{
			RenderPreset(XWVisualEditorPreset.CreateFor(_descriptor, _resource, _path));
		}
	}

	private void RenderPreset(XWVisualEditorPreset preset)
	{
		_title.Text = preset.Title;
		if (_summary != null)
		{
			_summary.Text = BuildContextSummary(preset.Summary);
		}
		RenderToolbar(preset);
		if (ShouldRenderPresetPlaceholders(preset))
		{
			RenderCanvas(preset);
			RenderList(_timelineList, preset.TimelineItems);
			RenderList(_graphList, preset.GraphItems);
			RenderList(_previewList, preset.PreviewItems);
			List<string> list = new List<string>(preset.ReferenceItems);
			AppendProjectReferenceGraphItems(list);
			AppendEmbeddedSubResourceItems(list);
			RenderList(_referenceList, list);
		}
		RenderCustomVisualPreset(preset);
		BindInlineTextSurface(ShouldBindInlineTextSurface(_resource, _path, _descriptor) ? _resource : null);
		BindDirectPropertySurface(ShouldBindDirectPropertySurface(_resource, _path, _descriptor) ? _resource : null);
	}

	protected virtual bool ShouldBindInlineTextSurface(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return false;
		}
		if (descriptor?.Category != "Animation" || resource.GetType().Name != "AdobeAnimateData")
		{
			return true;
		}
		foreach (XWInlineTextProperty item in XWInlineTextCoverageContract.Inspect(resource, descriptor.Category))
		{
			if (item.IsSemantic)
			{
				return true;
			}
		}
		return false;
	}

	private void BindInlineTextSurface(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(_canvasGrid) || !GodotObject.IsInstanceValid(resource))
		{
			_inlineTextSurface = null;
			return;
		}
		_inlineTextSurface = new XWInlineTextSurface
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		_inlineTextSurface.TextEdited += OnInlineTextEdited;
		_inlineTextSurface.BindingPickerRequested += OnInlineTextBindingPickerRequested;
		_canvasGrid.AddChild(_inlineTextSurface, forceReadableName: false, InternalMode.Disabled);
		XWUndoRedoManager undoRedo = XWEditorInterface.Instance?.GetUndoRedoManager() ?? new XWUndoRedoManager();
		_inlineTextSurface.BindResource(resource, _descriptor?.Category ?? "General", undoRedo);
		if (!_inlineTextSurface.Visible)
		{
			_inlineTextSurface.QueueFree();
			_inlineTextSurface = null;
		}
	}

	private void OnInlineTextEdited(bool committed)
	{
		if (GodotObject.IsInstanceValid(_resource))
		{
			MarkCurrentResourceDirty();
			_resource.EmitChanged();
			if (committed)
			{
				SaveResourceDraft();
				RememberCurrentHistoryRefreshDomain(PropertyRefreshDomain.Scalar);
				QueueResourceEditorRefresh(PropertyRefreshDomain.Scalar);
			}
		}
	}

	private void OnInlineTextBindingPickerRequested(XWInlineTextProperty property)
	{
		if (property == null || !GodotObject.IsInstanceValid(_resourceLibraryPicker) || !XWInlineTextCoverageContract.TryResolveBindingBaseType(property, out var baseType))
		{
			return;
		}
		XWResourcePickerLibraryProfile xWResourcePickerLibraryProfile = XWResourcePickerLibraryProfileResolver.Resolve(baseType);
		_resourceLibraryPicker.OpenResourceLibrary(xWResourcePickerLibraryProfile.Category, property.Label + " · " + xWResourcePickerLibraryProfile.DisplayName, property.Owner.Get(property.Property).AsString(), GetCurrentProjectPath(), xWResourcePickerLibraryProfile.ClassNames, xWResourcePickerLibraryProfile.BuiltInPathMarkers, xWResourcePickerLibraryProfile.IconPath, (XWGameplayResourceChoice choice) =>
		{
			if (GodotObject.IsInstanceValid(_inlineTextSurface))
			{
				string value = (XWInlineTextCoverageContract.ShouldStorePickedResourcePath(property) ? choice.ResourcePath : choice.ResourcePath.GetFile().GetBaseName());
				_inlineTextSurface.ApplyPickedBinding(property, value);
			}
		});
	}

	protected virtual bool ShouldBindDirectPropertySurface(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		return GodotObject.IsInstanceValid(resource);
	}

	private void BindDirectPropertySurface(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(_directPropertiesHost))
		{
			return;
		}
		if (!GodotObject.IsInstanceValid(_directPropertySurface))
		{
			_directPropertySurface = new XWDirectPropertySurface
			{
				Name = "DirectPropertySurface",
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};
			_directPropertySurface.PropertyEdited += OnDirectPropertyEdited;
			_directPropertiesHost.AddChild(_directPropertySurface, forceReadableName: false, InternalMode.Disabled);
		}
		XWUndoRedoManager undoRedo = XWEditorInterface.Instance?.GetUndoRedoManager() ?? new XWUndoRedoManager();
		List<string> list = new List<string>();
		if (GodotObject.IsInstanceValid(_inlineTextSurface))
		{
			foreach (XWInlineTextProperty property in _inlineTextSurface.Properties)
			{
				list.Add(property.Property.ToString());
			}
		}
		_directPropertySurface.BindResource(resource, undoRedo, list);
	}

	private void OnDirectPropertyEdited(GodotObject obj, StringName property, StringName field, Variant value)
	{
		if (GodotObject.IsInstanceValid(_resource))
		{
			OnEmbeddedInspectorPropertyChanged(obj, property, field, value);
			PropertyRefreshDomain domain = DeterminePropertyRefreshDomain(obj, property, value);
			RememberCurrentHistoryRefreshDomain(domain);
			MarkCurrentResourceDirty();
			_resource.EmitChanged();
			SaveResourceDraft();
			QueueResourceEditorRefresh(domain);
		}
	}

	private string BuildContextSummary(string summary)
	{
		if (_editContext == null || string.IsNullOrWhiteSpace(_editContext.PropertyPath))
		{
			return summary ?? "";
		}
		string value = ((!string.IsNullOrWhiteSpace(_editContext.OwnerPath)) ? _editContext.OwnerPath : (_editContext.OwnerResource?.ResourceName ?? "内嵌资源"));
		string value2 = ((_editContext.ArrayIndex >= 0) ? $"[{_editContext.ArrayIndex}]" : "");
		return $"{summary}\n拥有者: {value} · 属性: {_editContext.PropertyPath}{value2}";
	}

	protected virtual bool ShouldRenderPresetPlaceholders(XWVisualEditorPreset preset)
	{
		return false;
	}

	protected virtual void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		if (_canvasGrid != null)
		{
			_canvasGrid.Columns = 1;
			if (_universalResourcePreviewScene == null)
			{
				_universalResourcePreviewScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/XWUniversalResourcePreview.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			_universalResourcePreview = _universalResourcePreviewScene?.Instantiate<XWUniversalResourcePreview>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(_universalResourcePreview))
			{
				_canvasGrid.AddChild(_universalResourcePreview, forceReadableName: false, InternalMode.Disabled);
				_universalResourcePreview.ShowResource(_resource, GetCurrentResourcePath());
			}
		}
	}

	private void AppendProjectReferenceGraphItems(List<string> referenceItems)
	{
		if (referenceItems == null)
		{
			return;
		}
		string currentProjectPath = GetCurrentProjectPath();
		string currentResourcePath = GetCurrentResourcePath();
		if (!string.IsNullOrWhiteSpace(currentProjectPath) && !string.IsNullOrWhiteSpace(currentResourcePath))
		{
			HashSet<string> hashSet = BuildReferenceKeyCandidates(currentResourcePath, currentProjectPath);
			if (hashSet.Count != 0)
			{
				XWReferenceGraphService.ReferenceGraph referenceGraph = new XWReferenceGraphService().BuildForProject(currentProjectPath);
				AppendMatchingGraphRows(referenceItems, referenceGraph.References, hashSet, "引用 -> ");
				AppendMatchingGraphRows(referenceItems, referenceGraph.ReverseReferences, hashSet, "被引用于 -> ");
				AppendMatchingGraphRows(referenceItems, referenceGraph.MissingReferences, hashSet, "缺失引用 -> ");
			}
		}
	}

	private static void AppendMatchingGraphRows(List<string> target, System.Collections.Generic.Dictionary<string, List<string>> graphRows, HashSet<string> keyCandidates, string rowPrefix)
	{
		foreach (KeyValuePair<string, List<string>> graphRow in graphRows)
		{
			if (!keyCandidates.Contains(NormalizeReferenceGraphKey(graphRow.Key)))
			{
				continue;
			}
			foreach (string item in graphRow.Value)
			{
				AddUniqueRow(target, rowPrefix + item);
			}
		}
	}

	private static HashSet<string> BuildReferenceKeyCandidates(string currentPath, string projectPath)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		AddReferenceKeyCandidate(hashSet, currentPath);
		string value = XWImportWizard.ToModRelativePath(currentPath, projectPath);
		AddReferenceKeyCandidate(hashSet, value);
		try
		{
			if (!currentPath.StartsWith("res://", StringComparison.OrdinalIgnoreCase) && !currentPath.StartsWith("uid://", StringComparison.OrdinalIgnoreCase))
			{
				AddReferenceKeyCandidate(hashSet, Path.GetFullPath(currentPath));
			}
		}
		catch
		{
		}
		return hashSet;
	}

	private static void AddReferenceKeyCandidate(HashSet<string> target, string value)
	{
		string text = NormalizeReferenceGraphKey(value);
		if (!string.IsNullOrWhiteSpace(text))
		{
			target.Add(text);
		}
	}

	private static string NormalizeReferenceGraphKey(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return "";
		}
		string text = value.Trim().TrimEnd(',', ';').Replace('\\', '/');
		if (text.StartsWith("res://", StringComparison.OrdinalIgnoreCase) || text.StartsWith("uid://", StringComparison.OrdinalIgnoreCase))
		{
			return text;
		}
		try
		{
			if (Path.IsPathRooted(text))
			{
				return Path.GetFullPath(text).Replace('\\', '/').TrimEnd('/');
			}
		}
		catch
		{
			return text.Trim('/');
		}
		return text.Trim('/');
	}

	private static void AddUniqueRow(List<string> target, string row)
	{
		if (!string.IsNullOrWhiteSpace(row) && !target.Contains(row))
		{
			target.Add(row);
		}
	}

	protected virtual bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		return false;
	}

	protected virtual bool ShouldShowEmbeddedAnimationPreview(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		return false;
	}

	protected virtual HashSet<string> GetEmbeddedInspectorAllowedProperties(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		return null;
	}

	private void OpenResourceLibraryPicker()
	{
		if (!GodotObject.IsInstanceValid(_resourceLibraryPicker))
		{
			OpenNativeResourceDialogFallback();
			return;
		}
		_resourceLibraryPicker.OpenResourceLibrary(_descriptor?.Category ?? "General", _descriptor?.DisplayName ?? "资源", GetCurrentResourcePath(), GetCurrentProjectPath(), _descriptor?.ResourceClassNames ?? new List<string>(), _descriptor?.PathMarkers ?? new List<string>(), _descriptor?.IconPath ?? "", (XWGameplayResourceChoice choice) =>
		{
			OnResourceFileSelected(choice.ResourcePath);
		});
	}

	private void OpenNativeResourceDialogFallback()
	{
		PrepareProjectResourceDialog(_loadResourceDialog, forSave: false);
		_loadResourceDialog.PopupCentered();
	}

	private void OnResourceFileSelected(string path)
	{
		if (TresExtensionMethod.TryOpenAdobeAnimateResourceWithoutLoading(path))
		{
			return;
		}
		Resource resource = LoadResourceFromPath(path);
		if (GodotObject.IsInstanceValid(resource))
		{
			if (XWResourceEditorRegistry.TryGetEditor(resource, path, out var descriptor))
			{
				LoadResource(resource, path, descriptor);
			}
			else
			{
				LoadResource(resource, path, _descriptor);
			}
		}
	}

	private static Resource LoadResourceFromPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path) || !ResourceLoader.Exists(path))
		{
			XWEditorInterface.Instance?.ShowToast("无法加载资源: " + path);
			return null;
		}
		Resource resource = ResourceLoader.Load<Resource>(path, null, ResourceLoader.CacheMode.Reuse);
		if (!GodotObject.IsInstanceValid(resource))
		{
			XWEditorInterface instance = XWEditorInterface.Instance;
			if (instance == null)
			{
				return resource;
			}
			instance.ShowToast("无法加载资源: " + path);
		}
		return resource;
	}

	protected virtual void OnEmbeddedInspectorPropertyChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
	}

	private void QueueResourceEditorRefresh(PropertyRefreshDomain domain)
	{
		if (!SupportsIncrementalPropertyRefresh)
		{
			domain = PropertyRefreshDomain.Structure;
		}
		if (domain > _queuedPropertyRefreshDomain)
		{
			_queuedPropertyRefreshDomain = domain;
		}
		if (!_resourceEditorRefreshQueued)
		{
			_resourceEditorRefreshQueued = true;
			CallDeferred("RefreshAfterDirectPropertyChange");
		}
	}

	private void RefreshAfterDirectPropertyChange()
	{
		_resourceEditorRefreshQueued = false;
		PropertyRefreshDomain queuedPropertyRefreshDomain = _queuedPropertyRefreshDomain;
		_queuedPropertyRefreshDomain = PropertyRefreshDomain.None;
		if (GodotObject.IsInstanceValid(_resource))
		{
			UpdateCurrentResourceTabState();
			RefreshPropertyDomain(queuedPropertyRefreshDomain);
		}
	}

	private void RefreshPropertyDomain(PropertyRefreshDomain domain)
	{
		if (domain != PropertyRefreshDomain.Scalar || !SupportsIncrementalPropertyRefresh)
		{
			Refresh();
			return;
		}
		XWVisualEditorPreset xWVisualEditorPreset = XWVisualEditorPreset.CreateFor(_descriptor, _resource, _path);
		if (GodotObject.IsInstanceValid(_title))
		{
			_title.Text = xWVisualEditorPreset.Title;
		}
		if (GodotObject.IsInstanceValid(_summary))
		{
			_summary.Text = BuildContextSummary(xWVisualEditorPreset.Summary);
		}
		_inlineTextSurface?.RefreshValues();
		_directPropertySurface?.RefreshValues();
		RefreshIncrementalPreviewData();
	}

	protected virtual void RefreshIncrementalPreviewData()
	{
		if (GodotObject.IsInstanceValid(_universalResourcePreview))
		{
			_universalResourcePreview.ShowResource(_resource, GetCurrentResourcePath());
		}
	}

	private static PropertyRefreshDomain DeterminePropertyRefreshDomain(GodotObject obj, StringName property, Variant editedValue)
	{
		if (!IsStructuralVariant((GodotObject.IsInstanceValid(obj) ? obj.Get(property) : editedValue).VariantType) && !IsStructuralVariant(editedValue.VariantType))
		{
			return PropertyRefreshDomain.Scalar;
		}
		return PropertyRefreshDomain.Structure;
	}

	private static bool IsStructuralVariant(Variant.Type type)
	{
		if (type == Variant.Type.Object || (ulong)(type - 27) <= 11uL)
		{
			return true;
		}
		return false;
	}

	private void RememberCurrentHistoryRefreshDomain(PropertyRefreshDomain domain)
	{
		long num = (XWEditorInterface.Instance?.GetUndoRedoManager())?.GetCurrentActionStateToken() ?? 0;
		if (num > 0)
		{
			_historyRefreshDomains[num] = (SupportsIncrementalPropertyRefresh ? domain : PropertyRefreshDomain.Structure);
			TrimHistoryRefreshDomains();
		}
	}

	private void TrimHistoryRefreshDomains()
	{
		if (_historyRefreshDomains.Count > 4096)
		{
			List<long> list = new List<long>(_historyRefreshDomains.Keys);
			list.Sort();
			for (int i = 0; i < list.Count - 2048; i++)
			{
				_historyRefreshDomains.Remove(list[i]);
			}
		}
	}

	private PropertyRefreshDomain ResolveHistoryRefreshDomain(long actionToken)
	{
		if (!SupportsIncrementalPropertyRefresh)
		{
			return PropertyRefreshDomain.Structure;
		}
		if (actionToken <= 0 || !_historyRefreshDomains.TryGetValue(actionToken, out var value))
		{
			return PropertyRefreshDomain.Structure;
		}
		return value;
	}

	private void RenderToolbar(XWVisualEditorPreset preset)
	{
		if (_toolbar == null)
		{
			return;
		}
		foreach (Node child in _toolbar.GetChildren())
		{
			child.QueueFree();
		}
		foreach (string toolbarAction in preset.ToolbarActions)
		{
			string actionName = toolbarAction;
			Button button = new Button
			{
				Text = toolbarAction,
				FocusMode = FocusModeEnum.None,
				ToggleMode = (toolbarAction == "自动保存"),
				ButtonPressed = (toolbarAction == "自动保存" && _autosaveEnabled),
				Disabled = ((toolbarAction == "保存" && _resource == null) || (toolbarAction == "恢复草稿" && !_autosaveManager.HasResourceDraft(GetAutosaveId())))
			};
			button.Pressed += () =>
			{
				HandleToolbarAction(actionName);
			};
			_toolbar.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void HandleToolbarAction(string action)
	{
		switch (action)
		{
		case "打开":
			OpenResourceLibraryPicker();
			break;
		case "保存":
			SaveCurrentResource();
			break;
		case "撤销":
			UndoLastChange();
			break;
		case "重做":
			RedoLastChange();
			break;
		case "自动保存":
			ToggleAutosave();
			break;
		case "恢复草稿":
			RecoverResourceDraft();
			break;
		case "发布检查":
			RunReleaseCheck();
			break;
		default:
			XWEditorInterface.Instance?.ShowToast(action + " 暂未接入。");
			break;
		}
	}

	protected void SaveCurrentResource()
	{
		TrySaveCurrentResource();
	}

	private bool TrySaveCurrentResource()
	{
		if (!GodotObject.IsInstanceValid(_resource))
		{
			XWEditorInterface.Instance?.ShowToast("没有可保存的资源。");
			return false;
		}
		if (!CanPersistResource(_resource, out var failureReason))
		{
			XWEditorInterface.Instance?.ShowToast(failureReason);
			return false;
		}
		Resource resource = ResolveSaveTarget(out var savePath);
		if (string.IsNullOrWhiteSpace(savePath))
		{
			SaveCurrentResourceAs();
			return false;
		}
		XWResourceEditContext editContext = _editContext;
		if (editContext != null && editContext.IsBuiltInSource && XWResourceEditContext.IsBuiltInPath(savePath))
		{
			XWEditorInterface.Instance?.ShowToast("内置资源不可直接覆盖，请先复制到当前 Mod。");
			return false;
		}
		if (!CanWriteSaveTarget(resource, savePath, out var failureReason2))
		{
			XWEditorInterface.Instance?.ShowToast(failureReason2);
			LogResourceEditorAction("资源编辑器 · 已阻止危险保存: " + failureReason2);
			return false;
		}
		Error error = ResourceSaver.Save(resource, savePath, ResourceSaver.SaverFlags.None);
		if (error != Error.Ok)
		{
			XWEditorInterface.Instance?.ShowToast($"保存失败: {error}");
			return false;
		}
		XWModProjectLayout.MakeSavedTextResourceReferencesPortable(savePath, GetCurrentProjectPath());
		_autosaveManager.ClearDraft(GetAutosaveId());
		RegisterCurrentResourceInManifest(savePath);
		MarkCurrentResourceSaved();
		UpdateCurrentResourceTabState();
		OnCurrentResourceSaved();
		XWEditorInterface.Instance?.ShowToast("已保存: " + savePath);
		LogResourceEditorAction("资源编辑器: 保存", savePath);
		Refresh();
		return true;
	}

	public bool SaveActiveResource()
	{
		return TrySaveCurrentResource();
	}

	public bool SaveAllOpenResources(bool showToast = true)
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		foreach (ResourceTabState openResourceTab in _openResourceTabs)
		{
			if (openResourceTab.IsDirty && GodotObject.IsInstanceValid(openResourceTab.Resource))
			{
				if (!TrySaveResourceTab(openResourceTab, out var savePath, out var failureReason))
				{
					num2++;
					LogResourceEditorAction("资源批量保存失败", failureReason);
				}
				else
				{
					num++;
					flag |= IsCurrentResourceTab(openResourceTab);
					LogResourceEditorAction("资源编辑器: 保存", savePath);
				}
			}
		}
		RefreshResourceTabs(FindResourceTabIndex(_resource, _path));
		if (flag)
		{
			OnCurrentResourceSaved();
			Refresh();
		}
		if (num == 0 && num2 == 0)
		{
			return true;
		}
		string message = $"资源批量保存完成：成功 {num}，失败 {num2}。";
		LogResourceEditorAction(message);
		if (showToast)
		{
			XWEditorInterface.Instance?.ShowToast(message);
		}
		return num2 == 0;
	}

	private bool TrySaveResourceTab(ResourceTabState tab, out string savePath, out string failureReason)
	{
		savePath = "";
		failureReason = "";
		string resourceTabBaseTitle = GetResourceTabBaseTitle(tab);
		if (!CanPersistResource(tab.Resource, out var failureReason2))
		{
			failureReason = resourceTabBaseTitle + "：" + failureReason2;
			return false;
		}
		Resource resource = ResolveSaveTarget(tab.Resource, tab.Path, tab.Context, out savePath);
		if (string.IsNullOrWhiteSpace(savePath))
		{
			failureReason = resourceTabBaseTitle + "：没有保存路径，需要另存为。";
			return false;
		}
		XWResourceEditContext context = tab.Context;
		if (context != null && context.IsBuiltInSource && XWResourceEditContext.IsBuiltInPath(savePath))
		{
			failureReason = resourceTabBaseTitle + "：内置资源不可直接覆盖。";
			return false;
		}
		if (!CanWriteSaveTarget(resource, savePath, out var failureReason3))
		{
			failureReason = resourceTabBaseTitle + "：" + failureReason3;
			return false;
		}
		Error error = ResourceSaver.Save(resource, savePath, ResourceSaver.SaverFlags.None);
		if (error != Error.Ok)
		{
			failureReason = $"{resourceTabBaseTitle}：{error}";
			return false;
		}
		XWModProjectLayout.MakeSavedTextResourceReferencesPortable(savePath, GetCurrentProjectPath());
		_autosaveManager.ClearDraft(GetAutosaveId(tab.Resource, tab.Path, tab.Descriptor));
		RegisterCurrentResourceInManifest(savePath);
		tab.Path = NormalizeResourceTabPath(GetResourcePath(tab.Resource, tab.Path));
		tab.Key = BuildResourceTabKey(tab.Resource, tab.Path);
		tab.IsDirty = false;
		return true;
	}

	private void SaveCurrentResourceAs()
	{
		if (_resource == null)
		{
			XWEditorInterface.Instance?.ShowToast("没有可保存的资源。");
			return;
		}
		_saveResourceAsDialog.CurrentFile = GetCurrentResourcePath().GetFile();
		PrepareProjectResourceDialog(_saveResourceAsDialog, forSave: true);
		_saveResourceAsDialog.PopupCentered();
	}

	private void PrepareProjectResourceDialog(FileDialog dialog, bool forSave)
	{
		if (!GodotObject.IsInstanceValid(dialog))
		{
			return;
		}
		string currentProjectPath = GetCurrentProjectPath();
		if (string.IsNullOrWhiteSpace(currentProjectPath) || !Directory.Exists(currentProjectPath))
		{
			return;
		}
		string text = "";
		string currentResourcePath = GetCurrentResourcePath();
		if (!string.IsNullOrWhiteSpace(currentResourcePath) && Path.IsPathRooted(currentResourcePath))
		{
			string text2 = Path.GetDirectoryName(currentResourcePath) ?? "";
			if (Directory.Exists(text2) && IsInsideProject(text2, currentProjectPath))
			{
				text = text2;
			}
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			string resourceFolder = XWModProjectLayout.GetResourceFolder(_descriptor?.Category ?? "");
			string text3 = Path.Combine(currentProjectPath, resourceFolder.Replace('/', Path.DirectorySeparatorChar));
			text = (Directory.Exists(text3) ? text3 : currentProjectPath);
		}
		dialog.CurrentDir = text;
		if (!forSave)
		{
			dialog.CurrentFile = "";
		}
	}

	private static bool IsInsideProject(string path, string projectPath)
	{
		try
		{
			string relativePath = Path.GetRelativePath(projectPath, path);
			return !Path.IsPathRooted(relativePath) && !string.Equals(relativePath, "..", StringComparison.Ordinal) && !relativePath.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal) && !relativePath.StartsWith(".." + Path.AltDirectorySeparatorChar, StringComparison.Ordinal);
		}
		catch
		{
			return false;
		}
	}

	private void OnSaveResourceAsFileSelected(string savePath)
	{
		TrySaveCurrentResourceAs(savePath);
	}

	private bool TrySaveCurrentResourceAs(string savePath)
	{
		if (string.IsNullOrWhiteSpace(savePath) || _resource == null)
		{
			return false;
		}
		Resource resource = ResolveSaveAsTarget();
		if (!CanPersistResource(resource, out var failureReason))
		{
			XWEditorInterface.Instance?.ShowToast(failureReason);
			return false;
		}
		string autosaveId = GetAutosaveId();
		if (!CanWriteSaveTarget(resource, savePath, out var failureReason2))
		{
			XWEditorInterface.Instance?.ShowToast(failureReason2);
			LogResourceEditorAction("资源编辑器 · 已阻止危险另存为: " + failureReason2);
			return false;
		}
		Error error = ResourceSaver.Save(resource, savePath, ResourceSaver.SaverFlags.None);
		if (error != Error.Ok)
		{
			XWEditorInterface.Instance?.ShowToast($"保存失败: {error}");
			return false;
		}
		XWModProjectLayout.MakeSavedTextResourceReferencesPortable(savePath, GetCurrentProjectPath());
		bool flag = resource != _resource;
		resource.ResourcePath = savePath;
		if (!flag)
		{
			_path = savePath;
			_resource.ResourcePath = savePath;
		}
		_autosaveManager.ClearDraft(autosaveId);
		_autosaveManager.ClearDraft(GetAutosaveId());
		RegisterCurrentResourceInManifest(savePath);
		UpdateCurrentResourceTabState();
		MarkCurrentResourceSaved();
		OnCurrentResourceSaved();
		XWEditorInterface.Instance?.ShowToast("已保存 " + savePath);
		LogResourceEditorAction("资源编辑器: 保存", savePath);
		Refresh();
		return true;
	}

	private static void RegisterCurrentResourceInManifest(string resourcePath)
	{
		if (!string.IsNullOrWhiteSpace(resourcePath))
		{
			string currentProjectPath = GetCurrentProjectPath();
			if (!string.IsNullOrWhiteSpace(currentProjectPath))
			{
				XWModManifestSyncService.RegisterPath(currentProjectPath, resourcePath);
			}
		}
	}

	private void CopyCurrentResourcePath()
	{
		string currentResourcePath = GetCurrentResourcePath();
		if (string.IsNullOrWhiteSpace(currentResourcePath))
		{
			XWEditorInterface.Instance?.ShowToast("当前资源没有文件路径。");
			return;
		}
		DisplayServer.ClipboardSet(currentResourcePath);
		XWEditorInterface.Instance?.ShowToast("已复制资源路径: " + currentResourcePath);
		LogResourceEditorAction("复制路径: " + currentResourcePath);
	}

	private void ShowCurrentResourceInFileSystem()
	{
		string currentResourcePath = GetCurrentResourcePath();
		if (string.IsNullOrWhiteSpace(currentResourcePath))
		{
			XWEditorInterface.Instance?.ShowToast("当前资源没有文件系统位置。");
			return;
		}
		Control control = XWEditorInterface.Instance?.GetFileSystemPanel();
		if (GodotObject.IsInstanceValid(control) && control.HasMethod("NavigateToPath"))
		{
			control.Call("NavigateToPath", currentResourcePath);
			XWEditorInterface.Instance?.FocusPanel("file_system");
			LogResourceEditorAction("定位文件: " + currentResourcePath);
		}
	}

	private void MakeCurrentResourceBuiltIn()
	{
		if (_resource != null)
		{
			_resource.ResourcePath = "";
			_path = "";
			UpdateCurrentResourceTabState();
			MarkCurrentResourceDirty();
			XWEditorInterface.Instance?.ShowToast("已将资源设为内置。");
			LogResourceEditorAction("设为内置资源");
			Refresh();
		}
	}

	protected virtual void UndoLastChange()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager == null || !xWUndoRedoManager.HasUndo())
		{
			XWEditorInterface.Instance?.ShowToast("没有可撤销的修改。");
			return;
		}
		PropertyRefreshDomain domain = ResolveHistoryRefreshDomain(xWUndoRedoManager.GetCurrentActionStateToken());
		xWUndoRedoManager.Undo();
		MarkCurrentResourceDirty();
		SaveResourceDraft();
		RefreshPropertyDomain(domain);
	}

	protected virtual void RedoLastChange()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager == null || !xWUndoRedoManager.HasRedo())
		{
			XWEditorInterface.Instance?.ShowToast("没有可重做的修改。");
			return;
		}
		PropertyRefreshDomain domain = ResolveHistoryRefreshDomain(xWUndoRedoManager.GetNextRedoActionStateToken());
		xWUndoRedoManager.Redo();
		MarkCurrentResourceDirty();
		SaveResourceDraft();
		RefreshPropertyDomain(domain);
	}

	private void ToggleAutosave()
	{
		_autosaveEnabled = !_autosaveEnabled;
		SaveResourceDraft();
		XWEditorInterface.Instance?.ShowToast(_autosaveEnabled ? "自动保存已启用。" : "自动保存已关闭。");
		LogResourceEditorAction(_autosaveEnabled ? "启用自动保存" : "关闭自动保存");
		RenderToolbar(XWVisualEditorPreset.CreateFor(_descriptor, _resource, _path));
	}

	private async void RunReleaseCheck()
	{
		string currentProjectPath = GetCurrentProjectPath();
		if (string.IsNullOrWhiteSpace(currentProjectPath))
		{
			XWEditorInterface.Instance?.ShowToast("请先打开 Mod 工程。");
			return;
		}
		List<XWValidationIssue> list = await new XWModValidationService().ValidateProjectAsync(currentProjectPath);
		LogResourceEditorAction($"发布检查: {list.Count} 个问题");
		if (XWEditorInterface.Instance?.GetIssuePanel() is XWIssuePanel xWIssuePanel)
		{
			xWIssuePanel.LoadIssues(list);
			XWEditorInterface.Instance.FocusPanel("issues");
		}
		XWEditorInterface.Instance?.ShowToast((list.Count == 0) ? "发布检查未发现问题。" : $"发布检查发现 {list.Count} 个问题。");
	}

	protected virtual void SaveResourceDraft()
	{
		if (_autosaveEnabled && _resource != null && CanPersistResource(_resource, out var _))
		{
			string autosaveId = GetAutosaveId();
			Error error = _autosaveManager.SaveResourceDraft(autosaveId, _resource);
			if (error != Error.Ok)
			{
				XWEditorInterface.Instance?.ShowToast($"自动保存草稿失败: {error}");
				LogResourceEditorAction($"自动保存失败: {error}");
			}
			else
			{
				_autosaveManager.SaveDraft(autosaveId, new XWResourceAutosaveDraft
				{
					Path = GetCurrentResourcePath(),
					Category = (_descriptor?.Category ?? ""),
					ResourceType = _resource.GetType().Name,
					SavedAt = DateTimeOffset.Now.ToString("O")
				}, XWModJsonContext.Default.XWResourceAutosaveDraft);
				RenderToolbar(XWVisualEditorPreset.CreateFor(_descriptor, _resource, _path));
			}
		}
	}

	protected virtual void RecoverResourceDraft()
	{
		string autosaveId = GetAutosaveId();
		Resource resource = _autosaveManager.RecoverResourceDraft(autosaveId);
		if (!GodotObject.IsInstanceValid(resource))
		{
			XWEditorInterface.Instance?.ShowToast("当前资源没有可恢复的自动保存草稿。");
			return;
		}
		string text = (resource.ResourcePath = GetCurrentResourcePath());
		XWResourceEditContext context;
		if (HasNestedEditContext())
		{
			ReplaceRecoveredInOwner(resource);
			context = XWResourceEditContext.ForProperty(resource, _editContext.OwnerResource, text, _editContext.OwnerPath, _editContext.PropertyPath, _editContext.ArrayIndex, _editContext.SourcePanelKey, _editContext.IsBuiltInSource, _editContext.PreviewKey, _editContext.PersistenceRootResource, _editContext.PersistenceRootPath);
		}
		else
		{
			context = XWResourceEditContext.ForRoot(resource, text, _descriptor?.DockKey ?? "resource_editor");
		}
		LoadResourceInternal(resource, text, _descriptor, context, updateTabs: true);
		MarkCurrentResourceDirty();
		OnCurrentResourceDraftRecovered();
		XWEditorInterface.Instance?.ShowToast("已恢复自动保存草稿，请确认后手动保存。");
		LogResourceEditorAction("恢复自动保存草稿", text);
	}

	private Resource ResolveSaveTarget(out string savePath)
	{
		return ResolveSaveTarget(_resource, GetCurrentResourcePath(), _editContext, out savePath);
	}

	private static Resource ResolveSaveTarget(Resource resource, string path, XWResourceEditContext context, out string savePath)
	{
		savePath = GetResourcePath(resource, path);
		if (!HasNestedPersistenceContext(resource, context))
		{
			return resource;
		}
		string text = resource?.ResourcePath ?? "";
		if (!string.IsNullOrWhiteSpace(text) && !text.Contains("::", StringComparison.Ordinal))
		{
			return resource;
		}
		Resource persistenceRootResource = context.PersistenceRootResource;
		string persistenceRootPath = GetPersistenceRootPath(context);
		if (!GodotObject.IsInstanceValid(persistenceRootResource) || string.IsNullOrWhiteSpace(persistenceRootPath))
		{
			return resource;
		}
		savePath = persistenceRootPath;
		return persistenceRootResource;
	}

	private static bool HasNestedPersistenceContext(Resource resource, XWResourceEditContext context)
	{
		if (context != null && GodotObject.IsInstanceValid(context.PersistenceRootResource) && context.PersistenceRootResource != resource)
		{
			return !string.IsNullOrWhiteSpace(GetPersistenceRootPath(context));
		}
		return false;
	}

	private Resource ResolveSaveAsTarget()
	{
		string text = _resource?.ResourcePath ?? "";
		if ((string.IsNullOrWhiteSpace(text) || text.Contains("::", StringComparison.Ordinal)) && _editContext != null && GodotObject.IsInstanceValid(_editContext.PersistenceRootResource) && _editContext.PersistenceRootResource != _resource)
		{
			return _editContext.PersistenceRootResource;
		}
		return _resource;
	}

	private static string GetPersistenceRootPath(XWResourceEditContext context)
	{
		if (context == null)
		{
			return "";
		}
		if (!string.IsNullOrWhiteSpace(context.PersistenceRootPath))
		{
			return context.PersistenceRootPath;
		}
		return context.PersistenceRootResource?.ResourcePath ?? "";
	}

	private static bool CanWriteSaveTarget(Resource saveTarget, string savePath, out string failureReason)
	{
		failureReason = "";
		if (!GodotObject.IsInstanceValid(saveTarget))
		{
			failureReason = "保存目标已经失效，请返回上一级资源后重试。";
			return false;
		}
		if (string.IsNullOrWhiteSpace(savePath) || !ResourceLoader.Exists(savePath))
		{
			return true;
		}
		Resource resource = ResourceLoader.Load<Resource>(savePath, "", ResourceLoader.CacheMode.Ignore);
		if (!GodotObject.IsInstanceValid(resource))
		{
			return true;
		}
		Type type = resource.GetType();
		Type type2 = saveTarget.GetType();
		if (type == type2 || type.IsAssignableFrom(type2) || type2.IsAssignableFrom(type))
		{
			return true;
		}
		failureReason = $"已阻止覆盖：文件内是 {type.Name}，当前保存目标却是 {type2.Name}。请返回根资源后保存。";
		return false;
	}

	private bool HasNestedEditContext()
	{
		return HasNestedEditContext(_resource, _editContext);
	}

	private static bool HasNestedEditContext(Resource resource, XWResourceEditContext context)
	{
		if (context != null && GodotObject.IsInstanceValid(context.OwnerResource) && context.OwnerResource != resource)
		{
			return !string.IsNullOrWhiteSpace(context.PropertyPath);
		}
		return false;
	}

	private void ReplaceRecoveredInOwner(Resource recovered)
	{
		if (!HasNestedEditContext() || !GodotObject.IsInstanceValid(recovered))
		{
			return;
		}
		if (_editContext.ArrayIndex < 0)
		{
			_editContext.OwnerResource.Set(_editContext.PropertyPath, recovered);
		}
		else
		{
			Variant variant = _editContext.OwnerResource.Get(_editContext.PropertyPath);
			if (variant.VariantType != Variant.Type.Array)
			{
				return;
			}
			Godot.Collections.Array array = variant.AsGodotArray().Duplicate(deep: true);
			if (_editContext.ArrayIndex < 0 || _editContext.ArrayIndex >= array.Count)
			{
				return;
			}
			array[_editContext.ArrayIndex] = Variant.From(in recovered);
			_editContext.OwnerResource.Set(_editContext.PropertyPath, array);
		}
		_editContext.OwnerResource.EmitChanged();
	}

	protected virtual void OnCurrentResourceSaved()
	{
	}

	protected virtual void OnCurrentResourceDraftRecovered()
	{
	}

	public bool RefreshResourceIfOpen(Resource resource, bool markSaved = false)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return false;
		}
		int num = FindResourceTabIndexByResource(resource);
		if (num < 0)
		{
			return false;
		}
		_openResourceTabs[num].Resource = resource;
		if (markSaved)
		{
			_openResourceTabs[num].IsDirty = false;
		}
		if (_resource == resource)
		{
			Refresh();
		}
		RefreshResourceTabs(FindResourceTabIndex(_resource, _path));
		return true;
	}

	private string GetCurrentResourcePath()
	{
		if (!string.IsNullOrWhiteSpace(_path))
		{
			return _path;
		}
		return _resource?.ResourcePath ?? "";
	}

	private string GetAutosaveId()
	{
		return GetAutosaveId(_resource, GetCurrentResourcePath(), _descriptor);
	}

	private static string GetAutosaveId(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		path = GetResourcePath(resource, path);
		if (!string.IsNullOrWhiteSpace(path))
		{
			return path;
		}
		return $"{descriptor?.Category ?? "Resource"}:{resource?.GetInstanceId() ?? 0}";
	}

	private static string GetResourcePath(Resource resource, string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return path;
		}
		return resource?.ResourcePath ?? "";
	}

	private static string GetCurrentProjectPath()
	{
		if (XWEditorInterface.Instance?.GetEditorPanel() is ModEditorPanel modEditorPanel)
		{
			return modEditorPanel.GetCurrentProject()?.ProjectPath ?? "";
		}
		return "";
	}

	private void RenderCanvas(XWVisualEditorPreset preset)
	{
		if (_canvasGrid == null)
		{
			return;
		}
		foreach (Node child in _canvasGrid.GetChildren())
		{
			child.QueueFree();
		}
		foreach (string canvasItem in preset.CanvasItems)
		{
			Button node = new Button
			{
				Text = canvasItem,
				CustomMinimumSize = new Vector2(92f, 52f),
				SizeFlagsHorizontal = SizeFlags.ExpandFill,
				SizeFlagsVertical = SizeFlags.ExpandFill,
				FocusMode = FocusModeEnum.None
			};
			_canvasGrid.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private static void RenderList(ItemList list, IEnumerable<string> items)
	{
		if (list == null)
		{
			return;
		}
		list.Clear();
		foreach (string item in items)
		{
			list.AddItem(item);
		}
	}

	private void ClearAllSurfaces()
	{
		_embeddedReferenceTargets.Clear();
		_inlineTextSurface = null;
		_universalResourcePreview = null;
		if (_canvasGrid != null)
		{
			foreach (Node child in _canvasGrid.GetChildren())
			{
				if (!child.IsInGroup("mod_editor_static_surface"))
				{
					_canvasGrid.RemoveChild(child);
					child.QueueFree();
				}
			}
		}
		_timelineList?.Clear();
		_graphList?.Clear();
		_previewList?.Clear();
		_referenceList?.Clear();
	}

	private void AddMainPanelMessage(string text)
	{
		if (_canvasGrid != null)
		{
			_canvasGrid.Columns = 1;
			_canvasGrid.AddChild(new Label
			{
				Text = text,
				AutowrapMode = TextServer.AutowrapMode.WordSmart,
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			}, forceReadableName: false, InternalMode.Disabled);
		}
	}

	protected virtual bool CanPersistResource(Resource resource, out string failureReason)
	{
		failureReason = "";
		return GodotObject.IsInstanceValid(resource);
	}

	private void AppendEmbeddedSubResourceItems(List<string> referenceItems)
	{
		if (referenceItems != null && GodotObject.IsInstanceValid(_resource))
		{
			HashSet<Resource> visited = new HashSet<Resource> { _resource };
			int count = 0;
			CollectEmbeddedSubResources(referenceItems, _resource, "", visited, ref count, 64);
		}
	}

	private void CollectEmbeddedSubResources(List<string> referenceItems, Resource resource, string parentPath, HashSet<Resource> visited, ref int count, int maxCount)
	{
		if (!GodotObject.IsInstanceValid(resource) || count >= maxCount)
		{
			return;
		}
		foreach (Dictionary property in resource.GetPropertyList())
		{
			if (count >= maxCount)
			{
				break;
			}
			if (!property.ContainsKey("name"))
			{
				continue;
			}
			string text = property["name"].AsString();
			if (!ShouldSkipEmbeddedSubResourceProperty(text))
			{
				Variant value;
				try
				{
					value = resource.Get(text);
				}
				catch
				{
					continue;
				}
				string propertyPath = (string.IsNullOrWhiteSpace(parentPath) ? text : (parentPath + "." + text));
				CollectEmbeddedSubResources(referenceItems, value, propertyPath, visited, ref count, maxCount);
			}
		}
	}

	private void CollectEmbeddedSubResources(List<string> referenceItems, Variant value, string propertyPath, HashSet<Resource> visited, ref int count, int maxCount)
	{
		if (count >= maxCount)
		{
			return;
		}
		Variant.Type variantType = value.VariantType;
		Variant.Type num = variantType - 24;
		if ((ulong)num > 4uL)
		{
			return;
		}
		switch ((int)num)
		{
		case 0:
			if (value.AsGodotObject() is Resource resource && GodotObject.IsInstanceValid(resource) && !visited.Contains(resource) && string.IsNullOrWhiteSpace(resource.ResourcePath))
			{
				visited.Add(resource);
				string value2 = RegisterEmbeddedReferenceTarget(resource);
				referenceItems.Add($"子资源 {propertyPath} {resource.GetType().Name} -> {value2}");
				count++;
				CollectEmbeddedSubResources(referenceItems, resource, propertyPath, visited, ref count, maxCount);
			}
			break;
		case 4:
		{
			Godot.Collections.Array array = value.AsGodotArray();
			for (int i = 0; i < array.Count; i++)
			{
				if (count >= maxCount)
				{
					break;
				}
				CollectEmbeddedSubResources(referenceItems, array[i], $"{propertyPath}[{i}]", visited, ref count, maxCount);
			}
			break;
		}
		case 3:
		{
			Dictionary dictionary = value.AsGodotDictionary();
			{
				foreach (Variant key in dictionary.Keys)
				{
					if (count >= maxCount)
					{
						break;
					}
					CollectEmbeddedSubResources(referenceItems, dictionary[key], propertyPath + "." + key.AsString(), visited, ref count, maxCount);
				}
				break;
			}
		}
		case 1:
		case 2:
			break;
		}
	}

	private string RegisterEmbeddedReferenceTarget(Resource subResource)
	{
		string text = $"@sub:{_embeddedReferenceTargets.Count}";
		_embeddedReferenceTargets[text] = subResource;
		return text;
	}

	private static bool ShouldSkipEmbeddedSubResourceProperty(string propertyName)
	{
		if (!string.IsNullOrWhiteSpace(propertyName))
		{
			switch (propertyName)
			{
			default:
				return propertyName.StartsWith("_", StringComparison.Ordinal);
			case "script":
			case "resource_name":
			case "resource_path":
			case "resource_local_to_scene":
				break;
			}
		}
		return true;
	}

	private void OpenReferencedItem(long index)
	{
		if (_referenceList != null && index >= 0 && index < _referenceList.ItemCount)
		{
			string itemText = _referenceList.GetItemText((int)index);
			if (!TryOpenReferenceRowText(itemText))
			{
				XWEditorInterface.Instance?.ShowToast("这一行没有可跳转的引用。");
			}
		}
	}

	private void OpenVisualListItem(ItemList list, long index)
	{
		if (list != null && index >= 0 && index < list.ItemCount)
		{
			string itemText = list.GetItemText((int)index);
			if (!TryOpenReferenceRowText(itemText))
			{
				XWEditorInterface.Instance?.ShowToast("这一行没有可跳转的资源。");
			}
		}
	}

	private bool TryOpenReferenceRowText(string rowText)
	{
		string referenceTarget = GetReferenceTarget(rowText);
		int num;
		if (!string.IsNullOrWhiteSpace(referenceTarget))
		{
			num = (TryOpenReferenceTarget(referenceTarget, GetCurrentProjectPath()) ? 1 : 0);
			if (num != 0)
			{
				LogResourceEditorAction("资源编辑器: 跳转引用", referenceTarget);
			}
		}
		else
		{
			num = 0;
		}
		return (byte)num != 0;
	}

	private static string GetReferenceTarget(string rowText)
	{
		if (string.IsNullOrWhiteSpace(rowText))
		{
			return "";
		}
		int num = rowText.IndexOf("->", StringComparison.Ordinal);
		string target;
		if (num < 0)
		{
			target = rowText;
		}
		else
		{
			int num2 = num + 2;
			target = rowText.Substring(num2, rowText.Length - num2);
		}
		return StripReferenceDecoration(target);
	}

	private bool TryOpenReferenceTarget(string target, string projectPath)
	{
		if (string.IsNullOrWhiteSpace(target))
		{
			return false;
		}
		if (TryOpenEmbeddedReferenceTarget(target))
		{
			return true;
		}
		foreach (string item in BuildReferenceTargetCandidates(target, projectPath))
		{
			if (TryOpenScriptTarget(item) || TryOpenResourceTarget(item))
			{
				return true;
			}
		}
		return TryOpenKeyReferenceTarget(target, projectPath);
	}

	private bool TryOpenEmbeddedReferenceTarget(string target)
	{
		string text = StripReferenceDecoration(target);
		if (string.IsNullOrWhiteSpace(text) || !_embeddedReferenceTargets.TryGetValue(text, out var value) || !GodotObject.IsInstanceValid(value))
		{
			return false;
		}
		XWVisualEditorDescriptor descriptor = _descriptor;
		if (XWResourceEditorRegistry.TryGetEditor(value, "", out var descriptor2))
		{
			descriptor = descriptor2;
		}
		LoadResource(value, "", descriptor);
		LogResourceEditorAction("资源编辑器: 打开子资源", value.GetType().Name);
		return true;
	}

	private static List<string> BuildReferenceTargetCandidates(string target, string projectPath)
	{
		List<string> list = new List<string>();
		string text = StripReferenceDecoration(target);
		AddReferenceTargetCandidate(list, NormalizeReferenceTarget(text));
		AddReferenceTargetCandidate(list, ResolveModRelativeReferenceTarget(text, projectPath));
		return list;
	}

	private static string ResolveModRelativeReferenceTarget(string target, string projectPath)
	{
		if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(projectPath) || target.StartsWith("res://") || target.StartsWith("uid://"))
		{
			return "";
		}
		string text = target.Replace('\\', '/').TrimStart('/');
		try
		{
			if (Path.IsPathRooted(text))
			{
				return NormalizeReferenceTarget(text);
			}
			return NormalizeReferenceTarget(Path.Combine(projectPath, text));
		}
		catch
		{
			return "";
		}
	}

	private static bool TryOpenScriptTarget(string target)
	{
		if (string.IsNullOrWhiteSpace(target))
		{
			return false;
		}
		string text = NormalizeReferenceTarget(target);
		if (Path.GetExtension(text).ToLowerInvariant() != ".cs")
		{
			return false;
		}
		XWScriptEditor xWScriptEditor = XWEditorInterface.Instance?.GetScriptEditor();
		if (xWScriptEditor == null)
		{
			return false;
		}
		if (!xWScriptEditor.TryOpenFileAt(text, 0, 0))
		{
			return false;
		}
		XWEditorInterface.Instance?.FocusPanel("script_editor");
		return true;
	}

	private static bool TryOpenResourceTarget(string target)
	{
		if (string.IsNullOrWhiteSpace(target))
		{
			return false;
		}
		string path = NormalizeReferenceTarget(target);
		if (XWResourceEditorRegistry.TryOpenPath(path))
		{
			return true;
		}
		if (!ResourceLoader.Exists(path))
		{
			return false;
		}
		return XWResourceEditorRegistry.TryOpen(ResourceLoader.Load<Resource>(path, null, ResourceLoader.CacheMode.Reuse), path);
	}

	private static bool TryOpenKeyReferenceTarget(string target, string projectPath)
	{
		string text = StripReferenceDecoration(target);
		if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(projectPath) || text.Contains('/') || text.Contains('\\') || text.Contains("://") || Path.HasExtension(text) || !Directory.Exists(projectPath))
		{
			return false;
		}
		bool flag;
		try
		{
			foreach (string item in Directory.EnumerateFiles(projectPath, "*.*", SearchOption.AllDirectories))
			{
				switch (Path.GetExtension(item).ToLowerInvariant())
				{
				case ".tscn":
				case ".tres":
				case ".json":
				case ".csv":
				case ".res":
				case ".scn":
				case ".cs":
					flag = true;
					break;
				default:
					flag = false;
					break;
				}
				if (!flag || !string.Equals(Path.GetFileNameWithoutExtension(item), text, StringComparison.OrdinalIgnoreCase) || (!TryOpenScriptTarget(item) && !TryOpenResourceTarget(item)))
				{
					continue;
				}
				flag = true;
				goto IL_0192;
			}
		}
		catch
		{
			flag = false;
			goto IL_0192;
		}
		return false;
		IL_0192:
		return flag;
	}

	private static void AddReferenceTargetCandidate(List<string> target, string candidate)
	{
		if (string.IsNullOrWhiteSpace(candidate))
		{
			return;
		}
		foreach (string item in target)
		{
			if (string.Equals(item, candidate, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
		}
		target.Add(candidate);
	}

	private static string NormalizeReferenceTarget(string path)
	{
		if (string.IsNullOrWhiteSpace(path) || path.StartsWith("res://") || path.StartsWith("uid://"))
		{
			return path;
		}
		string text = ProjectSettings.GlobalizePath("res://").TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		string fullPath;
		try
		{
			fullPath = Path.GetFullPath(path);
		}
		catch
		{
			return path;
		}
		if (!fullPath.StartsWith(text, StringComparison.OrdinalIgnoreCase))
		{
			return path;
		}
		string text2 = fullPath.Substring(text.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		return "res://" + text2.Replace('\\', '/');
	}

	private static string StripReferenceDecoration(string target)
	{
		if (string.IsNullOrWhiteSpace(target))
		{
			return "";
		}
		string text = target.Trim().TrimEnd(',', ';');
		int num = text.IndexOf(" (", StringComparison.Ordinal);
		if (num > 0)
		{
			text = text.Substring(0, num).Trim();
		}
		return text;
	}

	private static void LogResourceEditorAction(string message)
	{
		if (!string.IsNullOrWhiteSpace(message))
		{
			XWEditorInterface.Instance?.AddOutputMessage("资源编辑器: " + message, XWOutputPanel.MessageType.Editor);
		}
	}

	private static void LogResourceEditorAction(string label, string detail)
	{
		if (!string.IsNullOrWhiteSpace(label))
		{
			string message = (string.IsNullOrWhiteSpace(detail) ? label : (label + ": " + detail));
			XWEditorInterface.Instance?.AddOutputMessage(message, XWOutputPanel.MessageType.Editor);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(93)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnableVisibilityGatedProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RequestVisibilityGatedProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "requested", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshVisibilityGatedProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HandleVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisableVisibilityGatedProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindSceneNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectVisualListNavigation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectResourceTab, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "tabIdx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloseResourceTab, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "tabIdx", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshResourceTabs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "selectedIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCurrentResourceTabState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkCurrentResourceDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NotifyCurrentResourceEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "refreshVisuals", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MarkCurrentResourceSaved, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MarkResourceDirty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "isDirty", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindResourceTabIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindResourceTabIndexByKey, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindResourceTabIndexByResource, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildResourceTabKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeResourceTabPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Refresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindInlineTextSurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnInlineTextEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindDirectPropertySurface, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnDirectPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildContextSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "summary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeReferenceGraphKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenResourceLibraryPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenNativeResourceDialogFallback, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnResourceFileSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadResourceFromPath, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnEmbeddedInspectorPropertyChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueResourceEditorRefresh, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "domain", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAfterDirectPropertyChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPropertyDomain, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "domain", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshIncrementalPreviewData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DeterminePropertyRefreshDomain, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "editedValue", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.IsStructuralVariant, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RememberCurrentHistoryRefreshDomain, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "domain", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TrimHistoryRefreshDomains, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveHistoryRefreshDomain, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "actionToken", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HandleToolbarAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "action", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveCurrentResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TrySaveCurrentResource, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveActiveResource, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveAllOpenResources, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "showToast", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveCurrentResourceAs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PrepareProjectResourceDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "dialog", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("FileDialog"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "forSave", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsInsideProject, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "projectPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnSaveResourceAsFileSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "savePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TrySaveCurrentResourceAs, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "savePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterCurrentResourceInManifest, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyCurrentResourcePath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowCurrentResourceInFileSystem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MakeCurrentResourceBuiltIn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UndoLastChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RedoLastChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToggleAutosave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunReleaseCheck, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveResourceDraft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RecoverResourceDraft, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveSaveAsTarget, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasNestedEditContext, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReplaceRecoveredInOwner, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "recovered", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnCurrentResourceSaved, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCurrentResourceDraftRecovered, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshResourceIfOpen, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "markSaved", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetAutosaveId, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentProjectPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ClearAllSurfaces, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddMainPanelMessage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterEmbeddedReferenceTarget, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "subResource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldSkipEmbeddedSubResourceProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenReferencedItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenVisualListItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryOpenReferenceRowText, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "rowText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetReferenceTarget, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "rowText", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryOpenReferenceTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "projectPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryOpenEmbeddedReferenceTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveModRelativeReferenceTarget, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "projectPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryOpenScriptTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryOpenResourceTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryOpenKeyReferenceTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "projectPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeReferenceTarget, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StripReferenceDecoration, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "target", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LogResourceEditorAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LogResourceEditorAction, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "detail", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.EnableVisibilityGatedProcessing && args.Count == 0)
		{
			EnableVisibilityGatedProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.RequestVisibilityGatedProcessing && args.Count == 1)
		{
			RequestVisibilityGatedProcessing(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshVisibilityGatedProcessing && args.Count == 0)
		{
			RefreshVisibilityGatedProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged && args.Count == 1)
		{
			OnVisualEditorVisibilityChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HandleVisualEditorVisibilityChanged && args.Count == 0)
		{
			HandleVisualEditorVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.DisableVisibilityGatedProcessing && args.Count == 0)
		{
			DisableVisibilityGatedProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadResource && args.Count == 1)
		{
			LoadResource(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindSceneNodes && args.Count == 0)
		{
			BindSceneNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectVisualListNavigation && args.Count == 0)
		{
			ConnectVisualListNavigation();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectResourceTab && args.Count == 1)
		{
			SelectResourceTab(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CloseResourceTab && args.Count == 1)
		{
			CloseResourceTab(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshResourceTabs && args.Count == 1)
		{
			RefreshResourceTabs(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCurrentResourceTabState && args.Count == 0)
		{
			UpdateCurrentResourceTabState();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkCurrentResourceDirty && args.Count == 0)
		{
			MarkCurrentResourceDirty();
			ret = default;
			return true;
		}
		if (method == MethodName.NotifyCurrentResourceEdited && args.Count == 1)
		{
			NotifyCurrentResourceEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MarkCurrentResourceSaved && args.Count == 0)
		{
			MarkCurrentResourceSaved();
			ret = default;
			return true;
		}
		if (method == MethodName.MarkResourceDirty && args.Count == 3)
		{
			MarkResourceDirty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.FindResourceTabIndex && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(FindResourceTabIndex(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.FindResourceTabIndexByKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindResourceTabIndexByKey(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindResourceTabIndexByResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindResourceTabIndexByResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildResourceTabKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildResourceTabKey(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeResourceTabPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResourceTabPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Refresh && args.Count == 0)
		{
			Refresh();
			ret = default;
			return true;
		}
		if (method == MethodName.BindInlineTextSurface && args.Count == 1)
		{
			BindInlineTextSurface(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnInlineTextEdited && args.Count == 1)
		{
			OnInlineTextEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindDirectPropertySurface && args.Count == 1)
		{
			BindDirectPropertySurface(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnDirectPropertyEdited && args.Count == 4)
		{
			OnDirectPropertyEdited(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildContextSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildContextSummary(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeReferenceGraphKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeReferenceGraphKey(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OpenResourceLibraryPicker && args.Count == 0)
		{
			OpenResourceLibraryPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenNativeResourceDialogFallback && args.Count == 0)
		{
			OpenNativeResourceDialogFallback();
			ret = default;
			return true;
		}
		if (method == MethodName.OnResourceFileSelected && args.Count == 1)
		{
			OnResourceFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadResourceFromPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(LoadResourceFromPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OnEmbeddedInspectorPropertyChanged && args.Count == 4)
		{
			OnEmbeddedInspectorPropertyChanged(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.QueueResourceEditorRefresh && args.Count == 1)
		{
			QueueResourceEditorRefresh(VariantUtils.ConvertTo<PropertyRefreshDomain>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAfterDirectPropertyChange && args.Count == 0)
		{
			RefreshAfterDirectPropertyChange();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPropertyDomain && args.Count == 1)
		{
			RefreshPropertyDomain(VariantUtils.ConvertTo<PropertyRefreshDomain>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshIncrementalPreviewData && args.Count == 0)
		{
			RefreshIncrementalPreviewData();
			ret = default;
			return true;
		}
		if (method == MethodName.DeterminePropertyRefreshDomain && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<PropertyRefreshDomain>(DeterminePropertyRefreshDomain(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2])));
			return true;
		}
		if (method == MethodName.IsStructuralVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsStructuralVariant(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.RememberCurrentHistoryRefreshDomain && args.Count == 1)
		{
			RememberCurrentHistoryRefreshDomain(VariantUtils.ConvertTo<PropertyRefreshDomain>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TrimHistoryRefreshDomains && args.Count == 0)
		{
			TrimHistoryRefreshDomains();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveHistoryRefreshDomain && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PropertyRefreshDomain>(ResolveHistoryRefreshDomain(VariantUtils.ConvertTo<long>(in args[0])));
			return true;
		}
		if (method == MethodName.HandleToolbarAction && args.Count == 1)
		{
			HandleToolbarAction(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveCurrentResource && args.Count == 0)
		{
			SaveCurrentResource();
			ret = default;
			return true;
		}
		if (method == MethodName.TrySaveCurrentResource && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(TrySaveCurrentResource());
			return true;
		}
		if (method == MethodName.SaveActiveResource && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(SaveActiveResource());
			return true;
		}
		if (method == MethodName.SaveAllOpenResources && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SaveAllOpenResources(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.SaveCurrentResourceAs && args.Count == 0)
		{
			SaveCurrentResourceAs();
			ret = default;
			return true;
		}
		if (method == MethodName.PrepareProjectResourceDialog && args.Count == 2)
		{
			PrepareProjectResourceDialog(VariantUtils.ConvertTo<FileDialog>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsInsideProject && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsInsideProject(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.OnSaveResourceAsFileSelected && args.Count == 1)
		{
			OnSaveResourceAsFileSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TrySaveCurrentResourceAs && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TrySaveCurrentResourceAs(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterCurrentResourceInManifest && args.Count == 1)
		{
			RegisterCurrentResourceInManifest(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CopyCurrentResourcePath && args.Count == 0)
		{
			CopyCurrentResourcePath();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCurrentResourceInFileSystem && args.Count == 0)
		{
			ShowCurrentResourceInFileSystem();
			ret = default;
			return true;
		}
		if (method == MethodName.MakeCurrentResourceBuiltIn && args.Count == 0)
		{
			MakeCurrentResourceBuiltIn();
			ret = default;
			return true;
		}
		if (method == MethodName.UndoLastChange && args.Count == 0)
		{
			UndoLastChange();
			ret = default;
			return true;
		}
		if (method == MethodName.RedoLastChange && args.Count == 0)
		{
			RedoLastChange();
			ret = default;
			return true;
		}
		if (method == MethodName.ToggleAutosave && args.Count == 0)
		{
			ToggleAutosave();
			ret = default;
			return true;
		}
		if (method == MethodName.RunReleaseCheck && args.Count == 0)
		{
			RunReleaseCheck();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveResourceDraft && args.Count == 0)
		{
			SaveResourceDraft();
			ret = default;
			return true;
		}
		if (method == MethodName.RecoverResourceDraft && args.Count == 0)
		{
			RecoverResourceDraft();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveSaveAsTarget && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Resource>(ResolveSaveAsTarget());
			return true;
		}
		if (method == MethodName.HasNestedEditContext && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasNestedEditContext());
			return true;
		}
		if (method == MethodName.ReplaceRecoveredInOwner && args.Count == 1)
		{
			ReplaceRecoveredInOwner(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCurrentResourceSaved && args.Count == 0)
		{
			OnCurrentResourceSaved();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCurrentResourceDraftRecovered && args.Count == 0)
		{
			OnCurrentResourceDraftRecovered();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshResourceIfOpen && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(RefreshResourceIfOpen(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCurrentResourcePath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrentResourcePath());
			return true;
		}
		if (method == MethodName.GetAutosaveId && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetAutosaveId());
			return true;
		}
		if (method == MethodName.GetResourcePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourcePath(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCurrentProjectPath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrentProjectPath());
			return true;
		}
		if (method == MethodName.ClearAllSurfaces && args.Count == 0)
		{
			ClearAllSurfaces();
			ret = default;
			return true;
		}
		if (method == MethodName.AddMainPanelMessage && args.Count == 1)
		{
			AddMainPanelMessage(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RegisterEmbeddedReferenceTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(RegisterEmbeddedReferenceTarget(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.ShouldSkipEmbeddedSubResourceProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipEmbeddedSubResourceProperty(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.OpenReferencedItem && args.Count == 1)
		{
			OpenReferencedItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenVisualListItem && args.Count == 2)
		{
			OpenVisualListItem(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<long>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryOpenReferenceRowText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryOpenReferenceRowText(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetReferenceTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetReferenceTarget(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.TryOpenReferenceTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryOpenReferenceTarget(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.TryOpenEmbeddedReferenceTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryOpenEmbeddedReferenceTarget(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveModRelativeReferenceTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveModRelativeReferenceTarget(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.TryOpenScriptTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryOpenScriptTarget(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.TryOpenResourceTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryOpenResourceTarget(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.TryOpenKeyReferenceTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryOpenKeyReferenceTarget(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeReferenceTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeReferenceTarget(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.StripReferenceDecoration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(StripReferenceDecoration(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LogResourceEditorAction && args.Count == 1)
		{
			LogResourceEditorAction(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LogResourceEditorAction && args.Count == 2)
		{
			LogResourceEditorAction(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.BuildResourceTabKey && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildResourceTabKey(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeResourceTabPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResourceTabPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeReferenceGraphKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeReferenceGraphKey(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadResourceFromPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(LoadResourceFromPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DeterminePropertyRefreshDomain && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<PropertyRefreshDomain>(DeterminePropertyRefreshDomain(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<Variant>(in args[2])));
			return true;
		}
		if (method == MethodName.IsStructuralVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsStructuralVariant(VariantUtils.ConvertTo<Variant.Type>(in args[0])));
			return true;
		}
		if (method == MethodName.IsInsideProject && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsInsideProject(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.RegisterCurrentResourceInManifest && args.Count == 1)
		{
			RegisterCurrentResourceInManifest(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetResourcePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(GetResourcePath(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCurrentProjectPath && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrentProjectPath());
			return true;
		}
		if (method == MethodName.ShouldSkipEmbeddedSubResourceProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipEmbeddedSubResourceProperty(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetReferenceTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetReferenceTarget(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveModRelativeReferenceTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveModRelativeReferenceTarget(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.TryOpenScriptTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryOpenScriptTarget(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.TryOpenResourceTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryOpenResourceTarget(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.TryOpenKeyReferenceTarget && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(TryOpenKeyReferenceTarget(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.NormalizeReferenceTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeReferenceTarget(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.StripReferenceDecoration && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(StripReferenceDecoration(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LogResourceEditorAction && args.Count == 1)
		{
			LogResourceEditorAction(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LogResourceEditorAction && args.Count == 2)
		{
			LogResourceEditorAction(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
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
		if (method == MethodName.EnableVisibilityGatedProcessing)
		{
			return true;
		}
		if (method == MethodName.RequestVisibilityGatedProcessing)
		{
			return true;
		}
		if (method == MethodName.RefreshVisibilityGatedProcessing)
		{
			return true;
		}
		if (method == MethodName.OnVisualEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.HandleVisualEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.DisableVisibilityGatedProcessing)
		{
			return true;
		}
		if (method == MethodName.LoadResource)
		{
			return true;
		}
		if (method == MethodName.BindSceneNodes)
		{
			return true;
		}
		if (method == MethodName.ConnectVisualListNavigation)
		{
			return true;
		}
		if (method == MethodName.SelectResourceTab)
		{
			return true;
		}
		if (method == MethodName.CloseResourceTab)
		{
			return true;
		}
		if (method == MethodName.RefreshResourceTabs)
		{
			return true;
		}
		if (method == MethodName.UpdateCurrentResourceTabState)
		{
			return true;
		}
		if (method == MethodName.MarkCurrentResourceDirty)
		{
			return true;
		}
		if (method == MethodName.NotifyCurrentResourceEdited)
		{
			return true;
		}
		if (method == MethodName.MarkCurrentResourceSaved)
		{
			return true;
		}
		if (method == MethodName.MarkResourceDirty)
		{
			return true;
		}
		if (method == MethodName.FindResourceTabIndex)
		{
			return true;
		}
		if (method == MethodName.FindResourceTabIndexByKey)
		{
			return true;
		}
		if (method == MethodName.FindResourceTabIndexByResource)
		{
			return true;
		}
		if (method == MethodName.BuildResourceTabKey)
		{
			return true;
		}
		if (method == MethodName.NormalizeResourceTabPath)
		{
			return true;
		}
		if (method == MethodName.Refresh)
		{
			return true;
		}
		if (method == MethodName.BindInlineTextSurface)
		{
			return true;
		}
		if (method == MethodName.OnInlineTextEdited)
		{
			return true;
		}
		if (method == MethodName.BindDirectPropertySurface)
		{
			return true;
		}
		if (method == MethodName.OnDirectPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.BuildContextSummary)
		{
			return true;
		}
		if (method == MethodName.NormalizeReferenceGraphKey)
		{
			return true;
		}
		if (method == MethodName.OpenResourceLibraryPicker)
		{
			return true;
		}
		if (method == MethodName.OpenNativeResourceDialogFallback)
		{
			return true;
		}
		if (method == MethodName.OnResourceFileSelected)
		{
			return true;
		}
		if (method == MethodName.LoadResourceFromPath)
		{
			return true;
		}
		if (method == MethodName.OnEmbeddedInspectorPropertyChanged)
		{
			return true;
		}
		if (method == MethodName.QueueResourceEditorRefresh)
		{
			return true;
		}
		if (method == MethodName.RefreshAfterDirectPropertyChange)
		{
			return true;
		}
		if (method == MethodName.RefreshPropertyDomain)
		{
			return true;
		}
		if (method == MethodName.RefreshIncrementalPreviewData)
		{
			return true;
		}
		if (method == MethodName.DeterminePropertyRefreshDomain)
		{
			return true;
		}
		if (method == MethodName.IsStructuralVariant)
		{
			return true;
		}
		if (method == MethodName.RememberCurrentHistoryRefreshDomain)
		{
			return true;
		}
		if (method == MethodName.TrimHistoryRefreshDomains)
		{
			return true;
		}
		if (method == MethodName.ResolveHistoryRefreshDomain)
		{
			return true;
		}
		if (method == MethodName.HandleToolbarAction)
		{
			return true;
		}
		if (method == MethodName.SaveCurrentResource)
		{
			return true;
		}
		if (method == MethodName.TrySaveCurrentResource)
		{
			return true;
		}
		if (method == MethodName.SaveActiveResource)
		{
			return true;
		}
		if (method == MethodName.SaveAllOpenResources)
		{
			return true;
		}
		if (method == MethodName.SaveCurrentResourceAs)
		{
			return true;
		}
		if (method == MethodName.PrepareProjectResourceDialog)
		{
			return true;
		}
		if (method == MethodName.IsInsideProject)
		{
			return true;
		}
		if (method == MethodName.OnSaveResourceAsFileSelected)
		{
			return true;
		}
		if (method == MethodName.TrySaveCurrentResourceAs)
		{
			return true;
		}
		if (method == MethodName.RegisterCurrentResourceInManifest)
		{
			return true;
		}
		if (method == MethodName.CopyCurrentResourcePath)
		{
			return true;
		}
		if (method == MethodName.ShowCurrentResourceInFileSystem)
		{
			return true;
		}
		if (method == MethodName.MakeCurrentResourceBuiltIn)
		{
			return true;
		}
		if (method == MethodName.UndoLastChange)
		{
			return true;
		}
		if (method == MethodName.RedoLastChange)
		{
			return true;
		}
		if (method == MethodName.ToggleAutosave)
		{
			return true;
		}
		if (method == MethodName.RunReleaseCheck)
		{
			return true;
		}
		if (method == MethodName.SaveResourceDraft)
		{
			return true;
		}
		if (method == MethodName.RecoverResourceDraft)
		{
			return true;
		}
		if (method == MethodName.ResolveSaveAsTarget)
		{
			return true;
		}
		if (method == MethodName.HasNestedEditContext)
		{
			return true;
		}
		if (method == MethodName.ReplaceRecoveredInOwner)
		{
			return true;
		}
		if (method == MethodName.OnCurrentResourceSaved)
		{
			return true;
		}
		if (method == MethodName.OnCurrentResourceDraftRecovered)
		{
			return true;
		}
		if (method == MethodName.RefreshResourceIfOpen)
		{
			return true;
		}
		if (method == MethodName.GetCurrentResourcePath)
		{
			return true;
		}
		if (method == MethodName.GetAutosaveId)
		{
			return true;
		}
		if (method == MethodName.GetResourcePath)
		{
			return true;
		}
		if (method == MethodName.GetCurrentProjectPath)
		{
			return true;
		}
		if (method == MethodName.ClearAllSurfaces)
		{
			return true;
		}
		if (method == MethodName.AddMainPanelMessage)
		{
			return true;
		}
		if (method == MethodName.RegisterEmbeddedReferenceTarget)
		{
			return true;
		}
		if (method == MethodName.ShouldSkipEmbeddedSubResourceProperty)
		{
			return true;
		}
		if (method == MethodName.OpenReferencedItem)
		{
			return true;
		}
		if (method == MethodName.OpenVisualListItem)
		{
			return true;
		}
		if (method == MethodName.TryOpenReferenceRowText)
		{
			return true;
		}
		if (method == MethodName.GetReferenceTarget)
		{
			return true;
		}
		if (method == MethodName.TryOpenReferenceTarget)
		{
			return true;
		}
		if (method == MethodName.TryOpenEmbeddedReferenceTarget)
		{
			return true;
		}
		if (method == MethodName.ResolveModRelativeReferenceTarget)
		{
			return true;
		}
		if (method == MethodName.TryOpenScriptTarget)
		{
			return true;
		}
		if (method == MethodName.TryOpenResourceTarget)
		{
			return true;
		}
		if (method == MethodName.TryOpenKeyReferenceTarget)
		{
			return true;
		}
		if (method == MethodName.NormalizeReferenceTarget)
		{
			return true;
		}
		if (method == MethodName.StripReferenceDecoration)
		{
			return true;
		}
		if (method == MethodName.LogResourceEditorAction)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._resource)
		{
			_resource = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._path)
		{
			_path = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._autosaveEnabled)
		{
			_autosaveEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._title)
		{
			_title = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._summary)
		{
			_summary = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._toolbar)
		{
			_toolbar = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._resourceTabBar)
		{
			_resourceTabBar = VariantUtils.ConvertTo<TabBar>(in value);
			return true;
		}
		if (name == PropertyName._mainPanel)
		{
			_mainPanel = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._canvasGrid)
		{
			_canvasGrid = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName._inlineTextSurface)
		{
			_inlineTextSurface = VariantUtils.ConvertTo<XWInlineTextSurface>(in value);
			return true;
		}
		if (name == PropertyName._directPropertiesHost)
		{
			_directPropertiesHost = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._directPropertySurface)
		{
			_directPropertySurface = VariantUtils.ConvertTo<XWDirectPropertySurface>(in value);
			return true;
		}
		if (name == PropertyName._universalResourcePreview)
		{
			_universalResourcePreview = VariantUtils.ConvertTo<XWUniversalResourcePreview>(in value);
			return true;
		}
		if (name == PropertyName._timelineList)
		{
			_timelineList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._graphList)
		{
			_graphList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._previewList)
		{
			_previewList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._referenceList)
		{
			_referenceList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._loadResourceDialog)
		{
			_loadResourceDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._saveResourceAsDialog)
		{
			_saveResourceAsDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._resourceLibraryPicker)
		{
			_resourceLibraryPicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._updatingResourceTabs)
		{
			_updatingResourceTabs = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._resourceEditorRefreshQueued)
		{
			_resourceEditorRefreshQueued = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._queuedPropertyRefreshDomain)
		{
			_queuedPropertyRefreshDomain = VariantUtils.ConvertTo<PropertyRefreshDomain>(in value);
			return true;
		}
		if (name == PropertyName._visibilityGatedProcessingEnabled)
		{
			_visibilityGatedProcessingEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._visibilityGatedProcessingRequested)
		{
			_visibilityGatedProcessingRequested = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		Resource from;
		if (name == PropertyName.CurrentResource)
		{
			from = CurrentResource;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		string from2;
		if (name == PropertyName.CurrentResourcePath)
		{
			from2 = CurrentResourcePath;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ActiveResource)
		{
			from = ActiveResource;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ActiveResourcePath)
		{
			from2 = ActiveResourcePath;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.MainPanel)
		{
			value = VariantUtils.CreateFrom<Control>(MainPanel);
			return true;
		}
		if (name == PropertyName.CanvasGrid)
		{
			value = VariantUtils.CreateFrom<GridContainer>(CanvasGrid);
			return true;
		}
		ItemList from3;
		if (name == PropertyName.TimelineList)
		{
			from3 = TimelineList;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.GraphList)
		{
			from3 = GraphList;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.PreviewList)
		{
			from3 = PreviewList;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		if (name == PropertyName.ReferenceList)
		{
			from3 = ReferenceList;
			value = VariantUtils.CreateFrom(in from3);
			return true;
		}
		int from4;
		if (name == PropertyName.DirectEditablePropertyCount)
		{
			from4 = DirectEditablePropertyCount;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.DirectMountedPropertyCount)
		{
			from4 = DirectMountedPropertyCount;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.DirectMissingPropertyCount)
		{
			from4 = DirectMissingPropertyCount;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.InlineSemanticTextCount)
		{
			from4 = InlineSemanticTextCount;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.InlineCoveredTextCount)
		{
			from4 = InlineCoveredTextCount;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.InlineTechnicalTextCount)
		{
			from4 = InlineTechnicalTextCount;
			value = VariantUtils.CreateFrom(in from4);
			return true;
		}
		if (name == PropertyName.SupportsIncrementalPropertyRefresh)
		{
			value = VariantUtils.CreateFrom<bool>(SupportsIncrementalPropertyRefresh);
			return true;
		}
		if (name == PropertyName._resource)
		{
			value = VariantUtils.CreateFrom(in _resource);
			return true;
		}
		if (name == PropertyName._path)
		{
			value = VariantUtils.CreateFrom(in _path);
			return true;
		}
		if (name == PropertyName._autosaveEnabled)
		{
			value = VariantUtils.CreateFrom(in _autosaveEnabled);
			return true;
		}
		if (name == PropertyName._title)
		{
			value = VariantUtils.CreateFrom(in _title);
			return true;
		}
		if (name == PropertyName._summary)
		{
			value = VariantUtils.CreateFrom(in _summary);
			return true;
		}
		if (name == PropertyName._toolbar)
		{
			value = VariantUtils.CreateFrom(in _toolbar);
			return true;
		}
		if (name == PropertyName._resourceTabBar)
		{
			value = VariantUtils.CreateFrom(in _resourceTabBar);
			return true;
		}
		if (name == PropertyName._mainPanel)
		{
			value = VariantUtils.CreateFrom(in _mainPanel);
			return true;
		}
		if (name == PropertyName._canvasGrid)
		{
			value = VariantUtils.CreateFrom(in _canvasGrid);
			return true;
		}
		if (name == PropertyName._inlineTextSurface)
		{
			value = VariantUtils.CreateFrom(in _inlineTextSurface);
			return true;
		}
		if (name == PropertyName._directPropertiesHost)
		{
			value = VariantUtils.CreateFrom(in _directPropertiesHost);
			return true;
		}
		if (name == PropertyName._directPropertySurface)
		{
			value = VariantUtils.CreateFrom(in _directPropertySurface);
			return true;
		}
		if (name == PropertyName._universalResourcePreview)
		{
			value = VariantUtils.CreateFrom(in _universalResourcePreview);
			return true;
		}
		if (name == PropertyName._timelineList)
		{
			value = VariantUtils.CreateFrom(in _timelineList);
			return true;
		}
		if (name == PropertyName._graphList)
		{
			value = VariantUtils.CreateFrom(in _graphList);
			return true;
		}
		if (name == PropertyName._previewList)
		{
			value = VariantUtils.CreateFrom(in _previewList);
			return true;
		}
		if (name == PropertyName._referenceList)
		{
			value = VariantUtils.CreateFrom(in _referenceList);
			return true;
		}
		if (name == PropertyName._loadResourceDialog)
		{
			value = VariantUtils.CreateFrom(in _loadResourceDialog);
			return true;
		}
		if (name == PropertyName._saveResourceAsDialog)
		{
			value = VariantUtils.CreateFrom(in _saveResourceAsDialog);
			return true;
		}
		if (name == PropertyName._resourceLibraryPicker)
		{
			value = VariantUtils.CreateFrom(in _resourceLibraryPicker);
			return true;
		}
		if (name == PropertyName._updatingResourceTabs)
		{
			value = VariantUtils.CreateFrom(in _updatingResourceTabs);
			return true;
		}
		if (name == PropertyName._resourceEditorRefreshQueued)
		{
			value = VariantUtils.CreateFrom(in _resourceEditorRefreshQueued);
			return true;
		}
		if (name == PropertyName._queuedPropertyRefreshDomain)
		{
			value = VariantUtils.CreateFrom(in _queuedPropertyRefreshDomain);
			return true;
		}
		if (name == PropertyName._visibilityGatedProcessingEnabled)
		{
			value = VariantUtils.CreateFrom(in _visibilityGatedProcessingEnabled);
			return true;
		}
		if (name == PropertyName._visibilityGatedProcessingRequested)
		{
			value = VariantUtils.CreateFrom(in _visibilityGatedProcessingRequested);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._resource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._path, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._autosaveEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._title, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._summary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._toolbar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceTabBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mainPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._canvasGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._inlineTextSurface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._directPropertiesHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._directPropertySurface, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._universalResourcePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._timelineList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._graphList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._referenceList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadResourceDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveResourceAsDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceLibraryPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingResourceTabs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._resourceEditorRefreshQueued, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._queuedPropertyRefreshDomain, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._visibilityGatedProcessingEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._visibilityGatedProcessingRequested, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CurrentResource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.CurrentResourcePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ActiveResource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName.ActiveResourcePath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.MainPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CanvasGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.TimelineList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.GraphList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.PreviewList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ReferenceList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DirectEditablePropertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DirectMountedPropertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DirectMissingPropertyCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.InlineSemanticTextCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.InlineCoveredTextCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.InlineTechnicalTextCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.SupportsIncrementalPropertyRefresh, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._resource, Variant.From(in _resource));
		info.AddProperty(PropertyName._path, Variant.From(in _path));
		info.AddProperty(PropertyName._autosaveEnabled, Variant.From(in _autosaveEnabled));
		info.AddProperty(PropertyName._title, Variant.From(in _title));
		info.AddProperty(PropertyName._summary, Variant.From(in _summary));
		info.AddProperty(PropertyName._toolbar, Variant.From(in _toolbar));
		info.AddProperty(PropertyName._resourceTabBar, Variant.From(in _resourceTabBar));
		info.AddProperty(PropertyName._mainPanel, Variant.From(in _mainPanel));
		info.AddProperty(PropertyName._canvasGrid, Variant.From(in _canvasGrid));
		info.AddProperty(PropertyName._inlineTextSurface, Variant.From(in _inlineTextSurface));
		info.AddProperty(PropertyName._directPropertiesHost, Variant.From(in _directPropertiesHost));
		info.AddProperty(PropertyName._directPropertySurface, Variant.From(in _directPropertySurface));
		info.AddProperty(PropertyName._universalResourcePreview, Variant.From(in _universalResourcePreview));
		info.AddProperty(PropertyName._timelineList, Variant.From(in _timelineList));
		info.AddProperty(PropertyName._graphList, Variant.From(in _graphList));
		info.AddProperty(PropertyName._previewList, Variant.From(in _previewList));
		info.AddProperty(PropertyName._referenceList, Variant.From(in _referenceList));
		info.AddProperty(PropertyName._loadResourceDialog, Variant.From(in _loadResourceDialog));
		info.AddProperty(PropertyName._saveResourceAsDialog, Variant.From(in _saveResourceAsDialog));
		info.AddProperty(PropertyName._resourceLibraryPicker, Variant.From(in _resourceLibraryPicker));
		info.AddProperty(PropertyName._updatingResourceTabs, Variant.From(in _updatingResourceTabs));
		info.AddProperty(PropertyName._resourceEditorRefreshQueued, Variant.From(in _resourceEditorRefreshQueued));
		info.AddProperty(PropertyName._queuedPropertyRefreshDomain, Variant.From(in _queuedPropertyRefreshDomain));
		info.AddProperty(PropertyName._visibilityGatedProcessingEnabled, Variant.From(in _visibilityGatedProcessingEnabled));
		info.AddProperty(PropertyName._visibilityGatedProcessingRequested, Variant.From(in _visibilityGatedProcessingRequested));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._resource, out var value))
		{
			_resource = value.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._path, out var value2))
		{
			_path = value2.As<string>();
		}
		if (info.TryGetProperty(PropertyName._autosaveEnabled, out var value3))
		{
			_autosaveEnabled = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._title, out var value4))
		{
			_title = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._summary, out var value5))
		{
			_summary = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._toolbar, out var value6))
		{
			_toolbar = value6.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._resourceTabBar, out var value7))
		{
			_resourceTabBar = value7.As<TabBar>();
		}
		if (info.TryGetProperty(PropertyName._mainPanel, out var value8))
		{
			_mainPanel = value8.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._canvasGrid, out var value9))
		{
			_canvasGrid = value9.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName._inlineTextSurface, out var value10))
		{
			_inlineTextSurface = value10.As<XWInlineTextSurface>();
		}
		if (info.TryGetProperty(PropertyName._directPropertiesHost, out var value11))
		{
			_directPropertiesHost = value11.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._directPropertySurface, out var value12))
		{
			_directPropertySurface = value12.As<XWDirectPropertySurface>();
		}
		if (info.TryGetProperty(PropertyName._universalResourcePreview, out var value13))
		{
			_universalResourcePreview = value13.As<XWUniversalResourcePreview>();
		}
		if (info.TryGetProperty(PropertyName._timelineList, out var value14))
		{
			_timelineList = value14.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._graphList, out var value15))
		{
			_graphList = value15.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._previewList, out var value16))
		{
			_previewList = value16.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._referenceList, out var value17))
		{
			_referenceList = value17.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._loadResourceDialog, out var value18))
		{
			_loadResourceDialog = value18.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._saveResourceAsDialog, out var value19))
		{
			_saveResourceAsDialog = value19.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._resourceLibraryPicker, out var value20))
		{
			_resourceLibraryPicker = value20.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._updatingResourceTabs, out var value21))
		{
			_updatingResourceTabs = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._resourceEditorRefreshQueued, out var value22))
		{
			_resourceEditorRefreshQueued = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._queuedPropertyRefreshDomain, out var value23))
		{
			_queuedPropertyRefreshDomain = value23.As<PropertyRefreshDomain>();
		}
		if (info.TryGetProperty(PropertyName._visibilityGatedProcessingEnabled, out var value24))
		{
			_visibilityGatedProcessingEnabled = value24.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._visibilityGatedProcessingRequested, out var value25))
		{
			_visibilityGatedProcessingRequested = value25.As<bool>();
		}
	}
}
