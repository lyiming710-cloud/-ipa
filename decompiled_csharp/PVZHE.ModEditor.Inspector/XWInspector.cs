using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.AdobeAnimateEditor.Inspector;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Registry;
using PVZHE.ModEditor.Registry.Class;
using PVZHE.ModEditor.ScriptEditor;

namespace PVZHE.ModEditor.Inspector;

[ScriptPath("res://addons/ModEditor/Inspector/GUI/XWInspector.cs")]
public class XWInspector : VBoxContainer
{
	public enum PropertyNameStyle
	{
		StyleRaw,
		StyleCapitalized
	}

	[Signal]
	public delegate void ObjectChangedEventHandler();

	[Signal]
	public delegate void ObjectPropertyChangedEventHandler(GodotObject obj, StringName property, StringName field, Variant value);

	[Signal]
	public delegate void PropertyKeyedEventHandler(StringName property, Variant value);

	[Signal]
	public delegate void ResourceSelectedEventHandler(Resource resource);

	[Signal]
	public delegate void NavigatedBackEventHandler();

	[Signal]
	public delegate void NavigatedForwardEventHandler();

	[Signal]
	public delegate void ResourceNewRequestedEventHandler(string resourceType);

	[Signal]
	public delegate void ResourceLoadRequestedEventHandler();

	[Signal]
	public delegate void ResourceSaveRequestedEventHandler();

	[Signal]
	public delegate void ResourceSaveAsRequestedEventHandler();

	[Signal]
	public delegate void ResourceCopyRequestedEventHandler();

	[Signal]
	public delegate void ResourceShowInFilesystemEventHandler();

	[Signal]
	public delegate void ResourceMakeBuiltInEventHandler();

	public new class MethodName : VBoxContainer.MethodName
	{
		public static readonly StringName AddPlugin = "AddPlugin";

		public static readonly StringName RemovePlugin = "RemovePlugin";

		public static readonly StringName Create = "Create";

		public static readonly StringName CreateSubInspector = "CreateSubInspector";

		public new static readonly StringName _EnterTree = "_EnterTree";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName SetupToolbarIcons = "SetupToolbarIcons";

		public static readonly StringName SetButtonIcon = "SetButtonIcon";

		public static readonly StringName SetMenuButtonIcon = "SetMenuButtonIcon";

		public static readonly StringName ConnectIdPressedOnce = "ConnectIdPressedOnce";

		public static readonly StringName BindObjectHeader = "BindObjectHeader";

		public static readonly StringName UpdateObjectHeader = "UpdateObjectHeader";

		public static readonly StringName UpdateObjectPreview = "UpdateObjectPreview";

		public static readonly StringName UpdateMultiNodeHeader = "UpdateMultiNodeHeader";

		public static readonly StringName UpdateToolbarState = "UpdateToolbarState";

		public static readonly StringName ProcessUpdates = "ProcessUpdates";

		public static readonly StringName ShouldRunUpdateTimer = "ShouldRunUpdateTimer";

		public static readonly StringName BindVisibilityHostWindow = "BindVisibilityHostWindow";

		public static readonly StringName DisconnectVisibilityHostWindow = "DisconnectVisibilityHostWindow";

		public static readonly StringName OnVisibilityHostWindowChanged = "OnVisibilityHostWindowChanged";

		public static readonly StringName RefreshUpdateTimerState = "RefreshUpdateTimerState";

		public static readonly StringName ResetPendingInspectorUpdates = "ResetPendingInspectorUpdates";

		public static readonly StringName ClearInvalidCurrentObject = "ClearInvalidCurrentObject";

		public static readonly StringName ProcessPendingPropertyUpdates = "ProcessPendingPropertyUpdates";

		public static readonly StringName PollPropertyChanges = "PollPropertyChanges";

		public static readonly StringName IsTrackedPropertySynchronized = "IsTrackedPropertySynchronized";

		public new static readonly StringName _Notification = "_Notification";

		public static readonly StringName PropertyPathMatches = "PropertyPathMatches";

		public static readonly StringName IsSubsequence = "IsSubsequence";

		public static readonly StringName ResourcePropertiesMatches = "ResourcePropertiesMatches";

		public static readonly StringName Search = "Search";

		public static readonly StringName SetObject = "SetObject";

		public static readonly StringName ShouldSkipSameObjectRefresh = "ShouldSkipSameObjectRefresh";

		public static readonly StringName IsSameInspectableObject = "IsSameInspectableObject";

		public static readonly StringName HasInspectorEditorFocus = "HasInspectorEditorFocus";

		public static readonly StringName IsControlInsideInspector = "IsControlInsideInspector";

		public static readonly StringName SetNodes = "SetNodes";

		public static readonly StringName NavigateBack = "NavigateBack";

		public static readonly StringName NavigateForward = "NavigateForward";

		public static readonly StringName CanNavigateBack = "CanNavigateBack";

		public static readonly StringName CanNavigateForward = "CanNavigateForward";

		public static readonly StringName GetSelectionHistory = "GetSelectionHistory";

		public static readonly StringName UpdateNavigationButtons = "UpdateNavigationButtons";

		public static readonly StringName SaveFoldState = "SaveFoldState";

		public static readonly StringName RestoreFoldState = "RestoreFoldState";

		public static readonly StringName SaveScrollPosition = "SaveScrollPosition";

		public static readonly StringName RestoreScrollPosition = "RestoreScrollPosition";

		public static readonly StringName GetRootInspector = "GetRootInspector";

		public static readonly StringName GetSubInspectorColorLevel = "GetSubInspectorColorLevel";

		public static readonly StringName GetSubInspectorColor = "GetSubInspectorColor";

		public static readonly StringName ToggleFavorite = "ToggleFavorite";

		public static readonly StringName IsFavorite = "IsFavorite";

		public static readonly StringName TogglePin = "TogglePin";

		public static readonly StringName IsPinned = "IsPinned";

		public static readonly StringName PinProperty = "PinProperty";

		public static readonly StringName UnpinProperty = "UnpinProperty";

		public static readonly StringName IsPropertyPinned = "IsPropertyPinned";

		public static readonly StringName GetPinnedProperties = "GetPinnedProperties";

		public static readonly StringName FavoriteProperty = "FavoriteProperty";

		public static readonly StringName UnfavoriteProperty = "UnfavoriteProperty";

		public static readonly StringName IsPropertyFavorited = "IsPropertyFavorited";

		public static readonly StringName GetFavoriteProperties = "GetFavoriteProperties";

		public static readonly StringName ShowMetadataEditor = "ShowMetadataEditor";

		public static readonly StringName SelectEditor = "SelectEditor";

		public static readonly StringName ConnectPropertyEditorSignals = "ConnectPropertyEditorSignals";

		public static readonly StringName OnPropertyEditorValueChanged = "OnPropertyEditorValueChanged";

		public static readonly StringName QueueInspectorRefreshAfterValueChange = "QueueInspectorRefreshAfterValueChange";

		public static readonly StringName ConnectObject = "ConnectObject";

		public static readonly StringName DisconnectObject = "DisconnectObject";

		public static readonly StringName OnPropertyListChanged = "OnPropertyListChanged";

		public static readonly StringName OnPreviewReady = "OnPreviewReady";

		public static readonly StringName UpDateProperties = "UpDateProperties";

		public static readonly StringName IsCurrentObjectInternalResourceEditorTarget = "IsCurrentObjectInternalResourceEditorTarget";

		public static readonly StringName CreateInternalResourceProperty = "CreateInternalResourceProperty";

		public static readonly StringName UpDateMultiNodeProperties = "UpDateMultiNodeProperties";

		public static readonly StringName ShouldShowInspectorProperty = "ShouldShowInspectorProperty";

		public static readonly StringName Clear = "Clear";

		public static readonly StringName GetPropertyKey = "GetPropertyKey";

		public static readonly StringName ReversePropertyListByCategory = "ReversePropertyListByCategory";

		public static readonly StringName CreateFavoritesSection = "CreateFavoritesSection";

		public static readonly StringName LoadFavoritesAndPins = "LoadFavoritesAndPins";

		public static readonly StringName SaveFavoritesAndPins = "SaveFavoritesAndPins";

		public static readonly StringName SetPropertyNameStyle = "SetPropertyNameStyle";

		public static readonly StringName GetPropertyNameStyle = "GetPropertyNameStyle";

		public static readonly StringName SetShowCategories = "SetShowCategories";

		public static readonly StringName Undo = "Undo";

		public static readonly StringName Redo = "Redo";

		public static readonly StringName HasUndo = "HasUndo";

		public static readonly StringName HasRedo = "HasRedo";

		public static readonly StringName GetUndoRedoManager = "GetUndoRedoManager";

		public static readonly StringName ShowPropertyContextMenu = "ShowPropertyContextMenu";

		public static readonly StringName OnPropertyContextMenuItem = "OnPropertyContextMenuItem";

		public static readonly StringName OpenPropertyDocumentation = "OpenPropertyDocumentation";

		public static readonly StringName PopulateNewResourceMenu = "PopulateNewResourceMenu";

		public static readonly StringName OnNewResourceSelected = "OnNewResourceSelected";

		public static readonly StringName PopulateSaveResourceMenu = "PopulateSaveResourceMenu";

		public static readonly StringName OnSaveResourceMenuItem = "OnSaveResourceMenuItem";

		public static readonly StringName PopulateResourceExtraMenu = "PopulateResourceExtraMenu";

		public static readonly StringName OnResourceExtraMenuItem = "OnResourceExtraMenuItem";

		public static readonly StringName EditResourceFromClipboard = "EditResourceFromClipboard";

		public static readonly StringName PopulateHistoryMenu = "PopulateHistoryMenu";

		public static readonly StringName OnHistoryMenuItem = "OnHistoryMenuItem";

		public static readonly StringName PopulateObjectMenu = "PopulateObjectMenu";

		public static readonly StringName OnObjectMenuItem = "OnObjectMenuItem";

		public static readonly StringName CopyObjectProperties = "CopyObjectProperties";

		public static readonly StringName PasteObjectProperties = "PasteObjectProperties";

		public static readonly StringName ExpandAllProperties = "ExpandAllProperties";

		public static readonly StringName CollapseAllProperties = "CollapseAllProperties";

		public static readonly StringName ExpandNonDefaultProperties = "ExpandNonDefaultProperties";

		public static readonly StringName MakeSubResourcesUnique = "MakeSubResourcesUnique";

		public static readonly StringName Edit = "Edit";

		public static readonly StringName EditObject = "EditObject";

		public static readonly StringName CapitalizeName = "CapitalizeName";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName PropertyContainer = "PropertyContainer";

		public static readonly StringName CurrentObject = "CurrentObject";

		public static readonly StringName MultiNodeEdit = "MultiNodeEdit";

		public static readonly StringName UndoRedoManager = "UndoRedoManager";

		public static readonly StringName UpdateTimerCallbackCount = "UpdateTimerCallbackCount";

		public static readonly StringName PropertyPollPassCount = "PropertyPollPassCount";

		public static readonly StringName PropertyPollReadCount = "PropertyPollReadCount";

		public static readonly StringName PropertyEditorCount = "PropertyEditorCount";

		public static readonly StringName IsUpdateTimerRunning = "IsUpdateTimerRunning";

		public static readonly StringName AnimationPreviewEnabled = "AnimationPreviewEnabled";

		public static readonly StringName RootInspector = "RootInspector";

		public static readonly StringName IsSubInspector = "IsSubInspector";

		public static readonly StringName PropertyNameStyleMode = "PropertyNameStyleMode";

		public static readonly StringName ShowStandardCategories = "ShowStandardCategories";

		public static readonly StringName ShowCustomCategories = "ShowCustomCategories";

		public static readonly StringName _searchLineEdit = "_searchLineEdit";

		public static readonly StringName _toolbar = "_toolbar";

		public static readonly StringName _backButton = "_backButton";

		public static readonly StringName _forwardButton = "_forwardButton";

		public static readonly StringName _newResourceButton = "_newResourceButton";

		public static readonly StringName _loadResourceButton = "_loadResourceButton";

		public static readonly StringName _saveResourceButton = "_saveResourceButton";

		public static readonly StringName _objectMenuButton = "_objectMenuButton";

		public static readonly StringName _subResourceBar = "_subResourceBar";

		public static readonly StringName _propertyScrollContainer = "_propertyScrollContainer";

		public static readonly StringName _objectIcon = "_objectIcon";

		public static readonly StringName _objectLabel = "_objectLabel";

		public static readonly StringName _openDocsButton = "_openDocsButton";

		public static readonly StringName _resourceExtraButton = "_resourceExtraButton";

		public static readonly StringName _historyButton = "_historyButton";

		public static readonly StringName _connectedObject = "_connectedObject";

		public static readonly StringName _updateTimer = "_updateTimer";

		public static readonly StringName _visibilityHostWindow = "_visibilityHostWindow";

		public static readonly StringName _pendingUpdates = "_pendingUpdates";

		public static readonly StringName _updateTreePending = "_updateTreePending";

		public static readonly StringName _changing = "_changing";

		public static readonly StringName _updateTimerCallbackCount = "_updateTimerCallbackCount";

		public static readonly StringName _propertyPollPassCount = "_propertyPollPassCount";

		public static readonly StringName _propertyPollReadCount = "_propertyPollReadCount";

		public static readonly StringName _objectHeader = "_objectHeader";

		public static readonly StringName _headerIcon = "_headerIcon";

		public static readonly StringName _headerLabel = "_headerLabel";

		public static readonly StringName _headerClassLabel = "_headerClassLabel";

		public static readonly StringName _headerIdLabel = "_headerIdLabel";

		public static readonly StringName _animationPreview = "_animationPreview";

		public static readonly StringName _animationPreviewEnabled = "_animationPreviewEnabled";

		public static readonly StringName _favorites = "_favorites";

		public static readonly StringName _pinnedProperties = "_pinnedProperties";

		public static readonly StringName _favoriteProperties = "_favoriteProperties";

		public static readonly StringName _pinned_properties = "_pinned_properties";

		public static readonly StringName _metadataEditDialog = "_metadataEditDialog";

		public static readonly StringName _foldStateCache = "_foldStateCache";

		public static readonly StringName _scrollCache = "_scrollCache";

		public static readonly StringName _selectionHistory = "_selectionHistory";

		public static readonly StringName _selectedEditor = "_selectedEditor";

		public static readonly StringName _resourcePreview = "_resourcePreview";

		public static readonly StringName _propertyContextMenu = "_propertyContextMenu";

		public static readonly StringName _newResourceSelectedCallable = "_newResourceSelectedCallable";

		public static readonly StringName _saveResourceMenuCallable = "_saveResourceMenuCallable";

		public static readonly StringName _resourceExtraMenuCallable = "_resourceExtraMenuCallable";

		public static readonly StringName _historyMenuCallable = "_historyMenuCallable";

		public static readonly StringName _objectMenuCallable = "_objectMenuCallable";

		public static readonly StringName _propertyContextMenuCallable = "_propertyContextMenuCallable";

		public static readonly StringName _contextMenuPropertyName = "_contextMenuPropertyName";

		public static readonly StringName _contextMenuEditor = "_contextMenuEditor";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
		public static readonly StringName ObjectChanged = "ObjectChanged";

		public static readonly StringName ObjectPropertyChanged = "ObjectPropertyChanged";

		public static readonly StringName PropertyKeyed = "PropertyKeyed";

		public static readonly StringName ResourceSelected = "ResourceSelected";

		public static readonly StringName NavigatedBack = "NavigatedBack";

		public static readonly StringName NavigatedForward = "NavigatedForward";

		public static readonly StringName ResourceNewRequested = "ResourceNewRequested";

		public static readonly StringName ResourceLoadRequested = "ResourceLoadRequested";

		public static readonly StringName ResourceSaveRequested = "ResourceSaveRequested";

		public static readonly StringName ResourceSaveAsRequested = "ResourceSaveAsRequested";

		public static readonly StringName ResourceCopyRequested = "ResourceCopyRequested";

		public static readonly StringName ResourceShowInFilesystem = "ResourceShowInFilesystem";

		public static readonly StringName ResourceMakeBuiltIn = "ResourceMakeBuiltIn";
	}

	private static readonly string ScenePath = "res://addons/ModEditor/Inspector/GUI/XWInspector.tscn";

	private static readonly Color[] SubInspectorColors = new Color[17]
	{
		new Color(0.063f, 0.369f, 0.533f, 0.12f),
		new Color(0.533f, 0.2f, 0.063f, 0.12f),
		new Color(0.2f, 0.533f, 0.063f, 0.12f),
		new Color(0.533f, 0.063f, 0.369f, 0.12f),
		new Color(0.369f, 0.533f, 0.063f, 0.12f),
		new Color(0.063f, 0.533f, 0.369f, 0.12f),
		new Color(0.533f, 0.369f, 0.063f, 0.12f),
		new Color(0.063f, 0.2f, 0.533f, 0.12f),
		new Color(0.369f, 0.063f, 0.533f, 0.12f),
		new Color(0.2f, 0.063f, 0.533f, 0.12f),
		new Color(0.063f, 0.533f, 0.2f, 0.12f),
		new Color(0.533f, 0.063f, 0.2f, 0.12f),
		new Color(0.063f, 0.533f, 0.533f, 0.12f),
		new Color(0.533f, 0.533f, 0.063f, 0.12f),
		new Color(0.369f, 0.2f, 0.369f, 0.12f),
		new Color(0.2f, 0.369f, 0.2f, 0.12f),
		new Color(0.369f, 0.369f, 0.2f, 0.12f)
	};

	private LineEdit _searchLineEdit;

	private HBoxContainer _toolbar;

	private Button _backButton;

	private Button _forwardButton;

	private MenuButton _newResourceButton;

	private Button _loadResourceButton;

	private MenuButton _saveResourceButton;

	private MenuButton _objectMenuButton;

	private HBoxContainer _subResourceBar;

	private ScrollContainer _propertyScrollContainer;

	private TextureRect _objectIcon;

	private Label _objectLabel;

	private Button _openDocsButton;

	private MenuButton _resourceExtraButton;

	private MenuButton _historyButton;

	private readonly System.Collections.Generic.Dictionary<string, XWInspectorGroupContainer> _groupContainerDictionary = new System.Collections.Generic.Dictionary<string, XWInspectorGroupContainer>();

	private readonly System.Collections.Generic.Dictionary<string, XWInspectorSubgroupContainer> _subgroupContainerDictionary = new System.Collections.Generic.Dictionary<string, XWInspectorSubgroupContainer>();

	private readonly System.Collections.Generic.Dictionary<string, XWInspectorPropertyEditorBase> _propertyEditorDictionary = new System.Collections.Generic.Dictionary<string, XWInspectorPropertyEditorBase>();

	private GodotObject _connectedObject;

	private Timer _updateTimer;

	private Window _visibilityHostWindow;

	private readonly Dictionary _pendingUpdates = new Dictionary();

	private bool _updateTreePending;

	private int _changing;

	private int _updateTimerCallbackCount;

	private int _propertyPollPassCount;

	private int _propertyPollReadCount;

	private HBoxContainer _objectHeader;

	private TextureRect _headerIcon;

	private Label _headerLabel;

	private Label _headerClassLabel;

	private Label _headerIdLabel;

	private AdobeAnimateInspectorPreview _animationPreview;

	private bool _animationPreviewEnabled = true;

	private readonly Dictionary _favorites = new Dictionary();

	private readonly Dictionary _pinnedProperties = new Dictionary();

	private readonly Dictionary _favoriteProperties = new Dictionary();

	private readonly Dictionary _pinned_properties = new Dictionary();

	private ConfirmationDialog _metadataEditDialog;

	private readonly Dictionary _foldStateCache = new Dictionary();

	private readonly Dictionary _scrollCache = new Dictionary();

	private XWEditorSelectionHistory _selectionHistory;

	private XWInspectorPropertyEditorBase _selectedEditor;

	private XWResourcePreview _resourcePreview;

	private PopupMenu _propertyContextMenu;

	private Callable _newResourceSelectedCallable;

	private Callable _saveResourceMenuCallable;

	private Callable _resourceExtraMenuCallable;

	private Callable _historyMenuCallable;

	private Callable _objectMenuCallable;

	private Callable _propertyContextMenuCallable;

	private string _contextMenuPropertyName = "";

	private XWInspectorPropertyEditorBase _contextMenuEditor;

	private readonly List<XWInspectorPlugin> _inspectorPlugins = new List<XWInspectorPlugin>();

	private ObjectChangedEventHandler backing_ObjectChanged;

	private ObjectPropertyChangedEventHandler backing_ObjectPropertyChanged;

	private PropertyKeyedEventHandler backing_PropertyKeyed;

	private ResourceSelectedEventHandler backing_ResourceSelected;

	private NavigatedBackEventHandler backing_NavigatedBack;

	private NavigatedForwardEventHandler backing_NavigatedForward;

	private ResourceNewRequestedEventHandler backing_ResourceNewRequested;

	private ResourceLoadRequestedEventHandler backing_ResourceLoadRequested;

	private ResourceSaveRequestedEventHandler backing_ResourceSaveRequested;

	private ResourceSaveAsRequestedEventHandler backing_ResourceSaveAsRequested;

	private ResourceCopyRequestedEventHandler backing_ResourceCopyRequested;

	private ResourceShowInFilesystemEventHandler backing_ResourceShowInFilesystem;

	private ResourceMakeBuiltInEventHandler backing_ResourceMakeBuiltIn;

	public VBoxContainer PropertyContainer { get; private set; }

	public GodotObject CurrentObject { get; private set; }

	public XWMultiNodeEdit MultiNodeEdit { get; private set; }

	public XWUndoRedoManager UndoRedoManager { get; set; }

	public int UpdateTimerCallbackCount => _updateTimerCallbackCount;

	public int PropertyPollPassCount => _propertyPollPassCount;

	public int PropertyPollReadCount => _propertyPollReadCount;

	public int PropertyEditorCount => _propertyEditorDictionary.Count;

	public bool IsUpdateTimerRunning
	{
		get
		{
			if (GodotObject.IsInstanceValid(_updateTimer))
			{
				return !_updateTimer.IsStopped();
			}
			return false;
		}
	}

	public bool AnimationPreviewEnabled
	{
		get
		{
			return _animationPreviewEnabled;
		}
		set
		{
			if (_animationPreviewEnabled != value)
			{
				_animationPreviewEnabled = value;
				if (IsNodeReady())
				{
					UpdateObjectPreview();
				}
			}
		}
	}

	public XWInspector RootInspector { get; private set; }

	public bool IsSubInspector { get; set; }

	public PropertyNameStyle PropertyNameStyleMode { get; set; } = PropertyNameStyle.StyleCapitalized;

	public bool ShowStandardCategories { get; set; } = true;

	public bool ShowCustomCategories { get; set; } = true;

	public Func<XWInspectorProperty, bool> PropertyFilter { get; set; }

	public event ObjectChangedEventHandler ObjectChanged
	{
		add
		{
			backing_ObjectChanged = (ObjectChangedEventHandler)Delegate.Combine(backing_ObjectChanged, value);
		}
		remove
		{
			backing_ObjectChanged = (ObjectChangedEventHandler)Delegate.Remove(backing_ObjectChanged, value);
		}
	}

	public event ObjectPropertyChangedEventHandler ObjectPropertyChanged
	{
		add
		{
			backing_ObjectPropertyChanged = (ObjectPropertyChangedEventHandler)Delegate.Combine(backing_ObjectPropertyChanged, value);
		}
		remove
		{
			backing_ObjectPropertyChanged = (ObjectPropertyChangedEventHandler)Delegate.Remove(backing_ObjectPropertyChanged, value);
		}
	}

	public event PropertyKeyedEventHandler PropertyKeyed
	{
		add
		{
			backing_PropertyKeyed = (PropertyKeyedEventHandler)Delegate.Combine(backing_PropertyKeyed, value);
		}
		remove
		{
			backing_PropertyKeyed = (PropertyKeyedEventHandler)Delegate.Remove(backing_PropertyKeyed, value);
		}
	}

	public event ResourceSelectedEventHandler ResourceSelected
	{
		add
		{
			backing_ResourceSelected = (ResourceSelectedEventHandler)Delegate.Combine(backing_ResourceSelected, value);
		}
		remove
		{
			backing_ResourceSelected = (ResourceSelectedEventHandler)Delegate.Remove(backing_ResourceSelected, value);
		}
	}

	public event NavigatedBackEventHandler NavigatedBack
	{
		add
		{
			backing_NavigatedBack = (NavigatedBackEventHandler)Delegate.Combine(backing_NavigatedBack, value);
		}
		remove
		{
			backing_NavigatedBack = (NavigatedBackEventHandler)Delegate.Remove(backing_NavigatedBack, value);
		}
	}

	public event NavigatedForwardEventHandler NavigatedForward
	{
		add
		{
			backing_NavigatedForward = (NavigatedForwardEventHandler)Delegate.Combine(backing_NavigatedForward, value);
		}
		remove
		{
			backing_NavigatedForward = (NavigatedForwardEventHandler)Delegate.Remove(backing_NavigatedForward, value);
		}
	}

	public event ResourceNewRequestedEventHandler ResourceNewRequested
	{
		add
		{
			backing_ResourceNewRequested = (ResourceNewRequestedEventHandler)Delegate.Combine(backing_ResourceNewRequested, value);
		}
		remove
		{
			backing_ResourceNewRequested = (ResourceNewRequestedEventHandler)Delegate.Remove(backing_ResourceNewRequested, value);
		}
	}

	public event ResourceLoadRequestedEventHandler ResourceLoadRequested
	{
		add
		{
			backing_ResourceLoadRequested = (ResourceLoadRequestedEventHandler)Delegate.Combine(backing_ResourceLoadRequested, value);
		}
		remove
		{
			backing_ResourceLoadRequested = (ResourceLoadRequestedEventHandler)Delegate.Remove(backing_ResourceLoadRequested, value);
		}
	}

	public event ResourceSaveRequestedEventHandler ResourceSaveRequested
	{
		add
		{
			backing_ResourceSaveRequested = (ResourceSaveRequestedEventHandler)Delegate.Combine(backing_ResourceSaveRequested, value);
		}
		remove
		{
			backing_ResourceSaveRequested = (ResourceSaveRequestedEventHandler)Delegate.Remove(backing_ResourceSaveRequested, value);
		}
	}

	public event ResourceSaveAsRequestedEventHandler ResourceSaveAsRequested
	{
		add
		{
			backing_ResourceSaveAsRequested = (ResourceSaveAsRequestedEventHandler)Delegate.Combine(backing_ResourceSaveAsRequested, value);
		}
		remove
		{
			backing_ResourceSaveAsRequested = (ResourceSaveAsRequestedEventHandler)Delegate.Remove(backing_ResourceSaveAsRequested, value);
		}
	}

	public event ResourceCopyRequestedEventHandler ResourceCopyRequested
	{
		add
		{
			backing_ResourceCopyRequested = (ResourceCopyRequestedEventHandler)Delegate.Combine(backing_ResourceCopyRequested, value);
		}
		remove
		{
			backing_ResourceCopyRequested = (ResourceCopyRequestedEventHandler)Delegate.Remove(backing_ResourceCopyRequested, value);
		}
	}

	public event ResourceShowInFilesystemEventHandler ResourceShowInFilesystem
	{
		add
		{
			backing_ResourceShowInFilesystem = (ResourceShowInFilesystemEventHandler)Delegate.Combine(backing_ResourceShowInFilesystem, value);
		}
		remove
		{
			backing_ResourceShowInFilesystem = (ResourceShowInFilesystemEventHandler)Delegate.Remove(backing_ResourceShowInFilesystem, value);
		}
	}

	public event ResourceMakeBuiltInEventHandler ResourceMakeBuiltIn
	{
		add
		{
			backing_ResourceMakeBuiltIn = (ResourceMakeBuiltInEventHandler)Delegate.Combine(backing_ResourceMakeBuiltIn, value);
		}
		remove
		{
			backing_ResourceMakeBuiltIn = (ResourceMakeBuiltInEventHandler)Delegate.Remove(backing_ResourceMakeBuiltIn, value);
		}
	}

	public void AddPlugin(XWInspectorPlugin plugin)
	{
		if (plugin != null)
		{
			_inspectorPlugins.Add(plugin);
			plugin.SetInspector(this);
		}
	}

	public void RemovePlugin(XWInspectorPlugin plugin)
	{
		if (plugin != null)
		{
			_inspectorPlugins.Remove(plugin);
		}
	}

	public IReadOnlyList<XWInspectorPlugin> GetPlugins()
	{
		return _inspectorPlugins;
	}

	public static XWInspector Create()
	{
		return ResourceLoader.Load<PackedScene>(ScenePath, null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspector>(PackedScene.GenEditState.Disabled);
	}

	public static XWInspector CreateSubInspector()
	{
		XWInspector xWInspector = ResourceLoader.Load<PackedScene>(ScenePath, null, ResourceLoader.CacheMode.Reuse).Instantiate<XWInspector>(PackedScene.GenEditState.Disabled);
		xWInspector.IsSubInspector = true;
		return xWInspector;
	}

	public override void _EnterTree()
	{
		BindVisibilityHostWindow();
		RefreshUpdateTimerState();
	}

	public override void _Ready()
	{
		PropertyContainer = GetNode<VBoxContainer>("%PropertyContainer");
		_searchLineEdit = GetNode<LineEdit>("%SearchLineEdit");
		_toolbar = GetNode<HBoxContainer>("%Toolbar");
		_backButton = GetNode<Button>("%BackButton");
		_forwardButton = GetNode<Button>("%ForwardButton");
		_newResourceButton = GetNode<MenuButton>("%NewResourceButton");
		_loadResourceButton = GetNode<Button>("%LoadResourceButton");
		_saveResourceButton = GetNode<MenuButton>("%SaveResourceButton");
		_objectMenuButton = GetNode<MenuButton>("%ObjectMenuButton");
		_subResourceBar = GetNode<HBoxContainer>("%SubResourceBar");
		_propertyScrollContainer = GetNode<ScrollContainer>("ScrollContainer");
		_objectIcon = GetNode<TextureRect>("%ObjectIcon");
		_objectLabel = GetNode<Label>("%ObjectLabel");
		_openDocsButton = GetNode<Button>("%OpenDocsButton");
		_resourceExtraButton = GetNode<MenuButton>("%ResourceExtraButton");
		_historyButton = GetNode<MenuButton>("%HistoryButton");
		_animationPreview = GetNode<AdobeAnimateInspectorPreview>("%ObjectAnimationPreview");
		if (!GodotObject.IsInstanceValid(UndoRedoManager))
		{
			UndoRedoManager = new XWUndoRedoManager();
		}
		_selectionHistory = new XWEditorSelectionHistory();
		_newResourceSelectedCallable = Callable.From<long>(OnNewResourceSelected);
		_saveResourceMenuCallable = Callable.From<long>(OnSaveResourceMenuItem);
		_resourceExtraMenuCallable = Callable.From<long>(OnResourceExtraMenuItem);
		_historyMenuCallable = Callable.From<long>(OnHistoryMenuItem);
		_objectMenuCallable = Callable.From<long>(OnObjectMenuItem);
		_propertyContextMenuCallable = Callable.From<long>(OnPropertyContextMenuItem);
		if (IsSubInspector)
		{
			_toolbar.Visible = false;
		}
		else
		{
			_searchLineEdit.TextChanged += Search;
			_backButton.Pressed += () =>
			{
				NavigateBack();
				EmitSignal(SignalName.NavigatedBack);
			};
			_forwardButton.Pressed += () =>
			{
				NavigateForward();
				EmitSignal(SignalName.NavigatedForward);
			};
			_loadResourceButton.Pressed += () =>
			{
				EmitSignal(SignalName.ResourceLoadRequested);
			};
			SetupToolbarIcons();
			PopulateSaveResourceMenu();
			PopulateNewResourceMenu();
			PopulateObjectMenu();
			PopulateResourceExtraMenu();
			PopulateHistoryMenu();
			UpdateToolbarState();
		}
		_updateTimer = new Timer
		{
			WaitTime = 0.2,
			Autostart = false
		};
		_updateTimer.Timeout += ProcessUpdates;
		AddChild(_updateTimer, forceReadableName: false, InternalMode.Disabled);
		BindVisibilityHostWindow();
		RefreshUpdateTimerState();
		_resourcePreview = new XWResourcePreview();
		_resourcePreview.PreviewReady += OnPreviewReady;
		AddChild(_resourcePreview, forceReadableName: false, InternalMode.Disabled);
		BindObjectHeader();
		LoadFavoritesAndPins();
		_propertyContextMenu = GetNode<PopupMenu>("%PropertyContextMenu");
		ConnectIdPressedOnce(_propertyContextMenu, _propertyContextMenuCallable);
	}

	public override void _ExitTree()
	{
		if (GodotObject.IsInstanceValid(_updateTimer))
		{
			_updateTimer.Stop();
		}
		DisconnectVisibilityHostWindow();
	}

	private void SetupToolbarIcons()
	{
		XWClassRegistry instance = XWClassRegistry.Instance;
		SetMenuButtonIcon(_newResourceButton, instance.GetUIIcon("Add"));
		SetButtonIcon(_loadResourceButton, instance.GetUIIcon("Load"));
		SetMenuButtonIcon(_saveResourceButton, instance.GetUIIcon("Save"));
		SetMenuButtonIcon(_resourceExtraButton, instance.GetUIIcon("GuiTabMenuHl"));
		SetMenuButtonIcon(_objectMenuButton, instance.GetUIIcon("Tools"));
		SetButtonIcon(_backButton, instance.GetUIIcon("Back"));
		SetButtonIcon(_forwardButton, instance.GetUIIcon("Forward"));
		SetMenuButtonIcon(_historyButton, instance.GetUIIcon("History"));
		SetButtonIcon(_openDocsButton, instance.GetUIIcon("Help"));
		_newResourceButton.Text = "";
		_loadResourceButton.Text = "";
		_saveResourceButton.Text = "";
		_resourceExtraButton.Text = "";
		_objectMenuButton.Text = "";
		_backButton.Text = "";
		_forwardButton.Text = "";
		_historyButton.Text = "";
		_openDocsButton.Text = "";
	}

	private static void SetButtonIcon(Button button, Texture2D icon)
	{
		if (button != null && GodotObject.IsInstanceValid(icon))
		{
			button.Icon = icon;
		}
	}

	private static void SetMenuButtonIcon(MenuButton button, Texture2D icon)
	{
		if (button != null && GodotObject.IsInstanceValid(icon))
		{
			button.Icon = icon;
		}
	}

	private static void ConnectIdPressedOnce(PopupMenu popup, Callable callable)
	{
		if (popup != null && !popup.IsConnected(PopupMenu.SignalName.IdPressed, callable))
		{
			popup.Connect(PopupMenu.SignalName.IdPressed, callable);
		}
	}

	private void BindObjectHeader()
	{
		_objectHeader = GetNode<HBoxContainer>("%ObjectHeader");
		_headerIcon = GetNode<TextureRect>("%HeaderIcon");
		_headerLabel = GetNode<Label>("%HeaderLabel");
		_headerClassLabel = GetNode<Label>("%HeaderClassLabel");
		_headerIdLabel = GetNode<Label>("%HeaderIdLabel");
		if (IsSubInspector)
		{
			MoveChild(_objectHeader, 0);
		}
	}

	private void UpdateObjectHeader()
	{
		UpdateObjectPreview();
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			if (GodotObject.IsInstanceValid(_objectHeader))
			{
				_objectHeader.Visible = false;
			}
			if (GodotObject.IsInstanceValid(_objectLabel))
			{
				_objectLabel.Text = "无选中对象";
			}
			if (GodotObject.IsInstanceValid(_objectIcon))
			{
				_objectIcon.Texture = null;
			}
			UpdateToolbarState();
			return;
		}
		_objectHeader.Visible = true;
		if (MultiNodeEdit != null)
		{
			UpdateMultiNodeHeader();
			return;
		}
		string text = CurrentObject.GetClass();
		string text2 = text;
		Script script = CurrentObject.GetScript().As<Script>();
		if (GodotObject.IsInstanceValid(script))
		{
			string text3 = script.GetGlobalName();
			if (text3 != "")
			{
				text2 = text3;
			}
		}
		if (CurrentObject is Node node)
		{
			text2 = node.Name;
		}
		_headerLabel.Text = text2;
		_headerClassLabel.Text = text;
		_headerIdLabel.Text = $"ID: {CurrentObject.GetInstanceId()}";
		if (GodotObject.IsInstanceValid(_objectLabel))
		{
			_objectLabel.Text = text2;
		}
		Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(text);
		if (GodotObject.IsInstanceValid(classIcon))
		{
			_headerIcon.Texture = classIcon;
			_headerIcon.Visible = true;
			if (GodotObject.IsInstanceValid(_objectIcon))
			{
				_objectIcon.Texture = classIcon;
			}
		}
		else
		{
			_headerIcon.Visible = false;
			if (GodotObject.IsInstanceValid(_objectIcon))
			{
				_objectIcon.Texture = null;
			}
		}
		UpdateToolbarState();
	}

	private void UpdateObjectPreview()
	{
		if (GodotObject.IsInstanceValid(_animationPreview))
		{
			if (!_animationPreviewEnabled)
			{
				_animationPreview.Visible = false;
				_animationPreview.ClearPreview();
			}
			else if (MultiNodeEdit == null && CurrentObject is AdobeAnimateData animation)
			{
				_animationPreview.Visible = true;
				_animationPreview.EditAnimation(animation);
			}
			else
			{
				_animationPreview.Visible = false;
				_animationPreview.ClearPreview();
			}
		}
	}

	private void UpdateMultiNodeHeader()
	{
		if (MultiNodeEdit == null)
		{
			return;
		}
		_objectHeader.Visible = true;
		int nodeCount = MultiNodeEdit.GetNodeCount();
		string className = MultiNodeEdit.GetClassName();
		_headerLabel.Text = $"{nodeCount} 个节点";
		_headerClassLabel.Text = className;
		_headerIdLabel.Text = "多选编辑";
		Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(className);
		if (GodotObject.IsInstanceValid(classIcon))
		{
			_headerIcon.Texture = classIcon;
			_headerIcon.Visible = true;
			if (GodotObject.IsInstanceValid(_objectIcon))
			{
				_objectIcon.Texture = classIcon;
			}
		}
		else
		{
			_headerIcon.Visible = false;
			if (GodotObject.IsInstanceValid(_objectIcon))
			{
				_objectIcon.Texture = null;
			}
		}
		if (GodotObject.IsInstanceValid(_objectLabel))
		{
			_objectLabel.Text = $"{nodeCount} 个节点";
		}
		UpdateToolbarState();
	}

	private void UpdateToolbarState()
	{
		if (!IsSubInspector)
		{
			bool flag = GodotObject.IsInstanceValid(CurrentObject);
			bool flag2 = CurrentObject is Resource;
			if (GodotObject.IsInstanceValid(_saveResourceButton))
			{
				_saveResourceButton.Disabled = !flag2;
			}
			if (GodotObject.IsInstanceValid(_resourceExtraButton))
			{
				_resourceExtraButton.Disabled = !flag2;
			}
			if (GodotObject.IsInstanceValid(_objectMenuButton))
			{
				_objectMenuButton.Disabled = !flag;
			}
			if (GodotObject.IsInstanceValid(_openDocsButton))
			{
				_openDocsButton.Disabled = !flag;
			}
			if (GodotObject.IsInstanceValid(_historyButton))
			{
				_historyButton.Disabled = _selectionHistory == null || _selectionHistory.GetHistory().Count == 0;
			}
		}
	}

	private void ProcessUpdates()
	{
		_updateTimerCallbackCount++;
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			ClearInvalidCurrentObject();
			RefreshUpdateTimerState();
			return;
		}
		if (!ShouldRunUpdateTimer())
		{
			RefreshUpdateTimerState();
			return;
		}
		bool flag = _updateTreePending && HasInspectorEditorFocus();
		_changing++;
		try
		{
			if (_updateTreePending && !flag)
			{
				_updateTreePending = false;
				UpDateProperties();
			}
			else
			{
				ProcessPendingPropertyUpdates();
			}
			_pendingUpdates.Clear();
			PollPropertyChanges();
			_propertyPollPassCount++;
		}
		finally
		{
			_changing--;
		}
	}

	private bool ShouldRunUpdateTimer()
	{
		if (!IsInsideTree() || !IsVisibleInTree() || !GodotObject.IsInstanceValid(CurrentObject))
		{
			return false;
		}
		Window window = GetWindow();
		if (GodotObject.IsInstanceValid(window))
		{
			return window.Visible;
		}
		return true;
	}

	private void BindVisibilityHostWindow()
	{
		Window window = GetWindow();
		if (_visibilityHostWindow != window)
		{
			DisconnectVisibilityHostWindow();
			if (GodotObject.IsInstanceValid(window))
			{
				_visibilityHostWindow = window;
				_visibilityHostWindow.VisibilityChanged += OnVisibilityHostWindowChanged;
			}
		}
	}

	private void DisconnectVisibilityHostWindow()
	{
		if (GodotObject.IsInstanceValid(_visibilityHostWindow))
		{
			_visibilityHostWindow.VisibilityChanged -= OnVisibilityHostWindowChanged;
		}
		_visibilityHostWindow = null;
	}

	private void OnVisibilityHostWindowChanged()
	{
		RefreshUpdateTimerState();
	}

	private void RefreshUpdateTimerState()
	{
		if (CurrentObject != null && !GodotObject.IsInstanceValid(CurrentObject))
		{
			ClearInvalidCurrentObject();
		}
		if (!GodotObject.IsInstanceValid(_updateTimer))
		{
			return;
		}
		if (ShouldRunUpdateTimer())
		{
			if (_updateTimer.IsStopped())
			{
				_updateTimer.Start();
			}
		}
		else if (!_updateTimer.IsStopped())
		{
			_updateTimer.Stop();
		}
	}

	private void ResetPendingInspectorUpdates()
	{
		_pendingUpdates.Clear();
		_updateTreePending = false;
	}

	private void ClearInvalidCurrentObject()
	{
		if (CurrentObject != null)
		{
			DisconnectObject();
			CurrentObject = null;
			MultiNodeEdit = null;
			ResetPendingInspectorUpdates();
			if (IsNodeReady())
			{
				UpDateProperties();
				UpdateToolbarState();
				EmitSignal(SignalName.ObjectChanged);
			}
		}
	}

	private void ProcessPendingPropertyUpdates()
	{
		foreach (Variant key in _pendingUpdates.Keys)
		{
			string text = (string)key;
			if (_propertyEditorDictionary.TryGetValue(text, out var value) && GodotObject.IsInstanceValid(value))
			{
				value.UpdateValueSafely();
				continue;
			}
			foreach (KeyValuePair<string, XWInspectorPropertyEditorBase> item in _propertyEditorDictionary)
			{
				if ((item.Key.EndsWith(":" + text) || item.Key == text) && GodotObject.IsInstanceValid(item.Value))
				{
					item.Value.UpdateValueSafely();
				}
			}
		}
	}

	private void PollPropertyChanges()
	{
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			return;
		}
		Rect2 rect = (GodotObject.IsInstanceValid(_propertyScrollContainer) ? _propertyScrollContainer.GetGlobalRect() : GetGlobalRect());
		foreach (KeyValuePair<string, XWInspectorPropertyEditorBase> item in _propertyEditorDictionary)
		{
			XWInspectorPropertyEditorBase value = item.Value;
			if (!GodotObject.IsInstanceValid(value) || !GodotObject.IsInstanceValid(value.Property) || !GodotObject.IsInstanceValid(value.Property.Object) || value.IsInner || value.Property.PropName == (StringName)"")
			{
				continue;
			}
			Control control = value;
			if (control == null || (control.IsVisibleInTree() && rect.Intersects(control.GetGlobalRect())))
			{
				_propertyPollReadCount++;
				Variant lastValue = XWInspectorPropertyEditorBase.ReadObjectProperty(value.Property.Object, value.Property.PropName);
				if (!lastValue.Equals(value.LastValue))
				{
					value.LastValue = lastValue;
					value.UpdateValueSafely();
				}
			}
		}
	}

	public bool IsTrackedPropertySynchronized(StringName property)
	{
		bool result = false;
		foreach (XWInspectorPropertyEditorBase value in _propertyEditorDictionary.Values)
		{
			if (GodotObject.IsInstanceValid(value) && GodotObject.IsInstanceValid(value.Property) && GodotObject.IsInstanceValid(value.Property.Object) && !(value.Property.PropName != property))
			{
				result = true;
				if (!XWInspectorPropertyEditorBase.ReadObjectProperty(value.Property.Object, value.Property.PropName).Equals(value.LastValue))
				{
					return false;
				}
			}
		}
		return result;
	}

	public override void _Notification(int what)
	{
		if ((long)what == 31)
		{
			BindVisibilityHostWindow();
			RefreshUpdateTimerState();
		}
		else if ((long)what == 1)
		{
			DisconnectVisibilityHostWindow();
			DisconnectObject();
		}
		else if ((long)what == 1006)
		{
			SaveFavoritesAndPins();
		}
	}

	private bool PropertyPathMatches(string propertyPath, string filter)
	{
		if (string.IsNullOrEmpty(filter))
		{
			return true;
		}
		string text = propertyPath.ToLower();
		string text2 = filter.ToLower();
		if (text.Contains(text2))
		{
			return true;
		}
		string[] array = text.Split('/');
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(':');
			foreach (string text3 in array2)
			{
				if (IsSubsequence(text2, text3))
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool IsSubsequence(string filter, string text)
	{
		int num = 0;
		for (int i = 0; i < text.Length; i++)
		{
			if (num >= filter.Length)
			{
				break;
			}
			if (text[i] == filter[num])
			{
				num++;
			}
		}
		return num == filter.Length;
	}

	private bool ResourcePropertiesMatches(Resource res, string filter)
	{
		if (!GodotObject.IsInstanceValid(res))
		{
			return false;
		}
		foreach (Dictionary property in res.GetPropertyList())
		{
			if ((property["usage"].AsInt64() & 4) == 0L)
			{
				continue;
			}
			string text = property["name"].AsString();
			if (PropertyPathMatches(text, filter))
			{
				return true;
			}
			if (property["hint"].AsInt32() == 17)
			{
				Resource resource = res.Get(text).As<Resource>();
				if (GodotObject.IsInstanceValid(resource) && ResourcePropertiesMatches(resource, filter))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void Search(string query)
	{
		if (string.IsNullOrEmpty(query))
		{
			foreach (XWInspectorGroupContainer value3 in _groupContainerDictionary.Values)
			{
				if (GodotObject.IsInstanceValid(value3))
				{
					value3.Visible = true;
				}
			}
			foreach (XWInspectorSubgroupContainer value4 in _subgroupContainerDictionary.Values)
			{
				if (GodotObject.IsInstanceValid(value4))
				{
					value4.Visible = true;
				}
			}
			{
				foreach (XWInspectorPropertyEditorBase value5 in _propertyEditorDictionary.Values)
				{
					if (GodotObject.IsInstanceValid(value5))
					{
						value5.Visible = true;
					}
				}
				return;
			}
		}
		HashSet<string> hashSet = new HashSet<string>();
		HashSet<string> hashSet2 = new HashSet<string>();
		foreach (string key in _groupContainerDictionary.Keys)
		{
			if (PropertyPathMatches(key, query))
			{
				hashSet.Add(key);
				_groupContainerDictionary[key].Visible = true;
			}
			else
			{
				_groupContainerDictionary[key].Visible = false;
			}
		}
		foreach (string key2 in _subgroupContainerDictionary.Keys)
		{
			if (PropertyPathMatches(key2, query))
			{
				hashSet2.Add(key2);
				Node node = _subgroupContainerDictionary[key2];
				while (node != PropertyContainer && GodotObject.IsInstanceValid(node))
				{
					if (node is CanvasItem canvasItem)
					{
						canvasItem.Visible = true;
					}
					if (node is XWInspectorGroupContainer xWInspectorGroupContainer)
					{
						hashSet.Add(xWInspectorGroupContainer.Group.GroupName.ToString());
					}
					node = node.GetParent();
				}
			}
			else
			{
				_subgroupContainerDictionary[key2].Visible = false;
			}
		}
		foreach (string key3 in _propertyEditorDictionary.Keys)
		{
			string propertyPath = (key3.Contains(":") ? key3.Split(":")[^1] : key3);
			bool flag = PropertyPathMatches(propertyPath, query);
			if (!flag)
			{
				XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase = _propertyEditorDictionary[key3];
				if (GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase) && GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase.Property))
				{
					Variant variant = ((xWInspectorPropertyEditorBase.Property.PropName != (StringName)"" && GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase.Property.Object)) ? XWInspectorPropertyEditorBase.ReadObjectProperty(xWInspectorPropertyEditorBase.Property.Object, xWInspectorPropertyEditorBase.Property.PropName) : default(Variant));
					if (variant.VariantType == Variant.Type.Object && variant.As<GodotObject>() is Resource res && ResourcePropertiesMatches(res, query))
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				Node node2 = _propertyEditorDictionary[key3];
				while (node2 != PropertyContainer && GodotObject.IsInstanceValid(node2))
				{
					if (node2 is CanvasItem canvasItem2)
					{
						canvasItem2.Visible = true;
					}
					if (node2 is XWInspectorGroupContainer xWInspectorGroupContainer2)
					{
						hashSet.Add(xWInspectorGroupContainer2.Group.GroupName.ToString());
					}
					else if (node2 is XWInspectorSubgroupContainer xWInspectorSubgroupContainer)
					{
						hashSet2.Add(xWInspectorSubgroupContainer.Subgroup.SubgroupName.ToString());
					}
					node2 = node2.GetParent();
				}
			}
			else
			{
				_propertyEditorDictionary[key3].Visible = false;
			}
		}
		foreach (string item in hashSet)
		{
			if (_groupContainerDictionary.TryGetValue(item, out var value))
			{
				value.Visible = true;
			}
		}
		foreach (string item2 in hashSet2)
		{
			if (_subgroupContainerDictionary.TryGetValue(item2, out var value2))
			{
				value2.Visible = true;
			}
		}
	}

	public void SetObject(GodotObject obj, bool forceRefresh = false)
	{
		if (ShouldSkipSameObjectRefresh(obj, forceRefresh))
		{
			RefreshUpdateTimerState();
			return;
		}
		if (GodotObject.IsInstanceValid(CurrentObject))
		{
			SaveFoldState();
			SaveScrollPosition();
		}
		DisconnectObject();
		ResetPendingInspectorUpdates();
		MultiNodeEdit = null;
		if (GodotObject.IsInstanceValid(obj))
		{
			_selectionHistory.AddObject(obj.GetInstanceId());
		}
		CurrentObject = obj;
		if (GodotObject.IsInstanceValid(_searchLineEdit))
		{
			_searchLineEdit.Text = "";
		}
		UpDateProperties();
		ConnectObject();
		UpdateObjectHeader();
		RestoreFoldState();
		RestoreScrollPosition();
		EmitSignal(SignalName.ObjectChanged);
		UpdateNavigationButtons(CanNavigateBack(), CanNavigateForward());
		if (!IsSubInspector)
		{
			PopulateHistoryMenu();
		}
		RefreshUpdateTimerState();
	}

	private bool ShouldSkipSameObjectRefresh(GodotObject obj, bool forceRefresh = false)
	{
		if (forceRefresh || MultiNodeEdit != null)
		{
			return false;
		}
		return IsSameInspectableObject(obj);
	}

	private bool IsSameInspectableObject(GodotObject obj)
	{
		if (!GodotObject.IsInstanceValid(CurrentObject) || !GodotObject.IsInstanceValid(obj))
		{
			return false;
		}
		return CurrentObject.GetInstanceId() == obj.GetInstanceId();
	}

	private bool HasInspectorEditorFocus()
	{
		Viewport viewport = GetViewport();
		if (!GodotObject.IsInstanceValid(viewport))
		{
			return false;
		}
		return IsControlInsideInspector(viewport.GuiGetFocusOwner());
	}

	private bool IsControlInsideInspector(Control control)
	{
		Node node = control;
		while (GodotObject.IsInstanceValid(node))
		{
			if (node == this || node == PropertyContainer)
			{
				return true;
			}
			node = node.GetParent();
		}
		return false;
	}

	public void SetNodes(Node[] nodes)
	{
		if (nodes == null || nodes.Length == 0)
		{
			SetObject(null);
			return;
		}
		if (nodes.Length == 1)
		{
			SetObject(nodes[0]);
			return;
		}
		if (GodotObject.IsInstanceValid(CurrentObject))
		{
			SaveFoldState();
			SaveScrollPosition();
		}
		DisconnectObject();
		ResetPendingInspectorUpdates();
		MultiNodeEdit = XWMultiNodeEdit.Create(nodes);
		CurrentObject = nodes[0];
		UpdateObjectPreview();
		if (GodotObject.IsInstanceValid(_searchLineEdit))
		{
			_searchLineEdit.Text = "";
		}
		UpDateProperties();
		ConnectObject();
		UpdateMultiNodeHeader();
		RestoreFoldState();
		EmitSignal(SignalName.ObjectChanged);
		UpdateNavigationButtons(CanNavigateBack(), CanNavigateForward());
		RefreshUpdateTimerState();
	}

	public void NavigateBack()
	{
		if (!_selectionHistory.Previous())
		{
			return;
		}
		ulong current = _selectionHistory.GetCurrent();
		if (current != 0L)
		{
			GodotObject godotObject = GodotObject.InstanceFromId(current);
			if (GodotObject.IsInstanceValid(godotObject))
			{
				DisconnectObject();
				ResetPendingInspectorUpdates();
				CurrentObject = godotObject;
				UpDateProperties();
				ConnectObject();
				UpdateObjectHeader();
				EmitSignal(SignalName.ObjectChanged);
			}
			UpdateNavigationButtons(CanNavigateBack(), CanNavigateForward());
			RefreshUpdateTimerState();
		}
	}

	public void NavigateForward()
	{
		if (!_selectionHistory.Next())
		{
			return;
		}
		ulong current = _selectionHistory.GetCurrent();
		if (current != 0L)
		{
			GodotObject godotObject = GodotObject.InstanceFromId(current);
			if (GodotObject.IsInstanceValid(godotObject))
			{
				DisconnectObject();
				ResetPendingInspectorUpdates();
				CurrentObject = godotObject;
				UpDateProperties();
				ConnectObject();
				UpdateObjectHeader();
				EmitSignal(SignalName.ObjectChanged);
			}
			UpdateNavigationButtons(CanNavigateBack(), CanNavigateForward());
			RefreshUpdateTimerState();
		}
	}

	public bool CanNavigateBack()
	{
		return !_selectionHistory.IsAtBeginning();
	}

	public bool CanNavigateForward()
	{
		return !_selectionHistory.IsAtEnd();
	}

	public XWEditorSelectionHistory GetSelectionHistory()
	{
		return _selectionHistory;
	}

	private void UpdateNavigationButtons(bool canBack, bool canForward)
	{
		if (GodotObject.IsInstanceValid(_backButton))
		{
			_backButton.Disabled = !canBack;
		}
		if (GodotObject.IsInstanceValid(_forwardButton))
		{
			_forwardButton.Disabled = !canForward;
		}
	}

	private void SaveFoldState()
	{
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			return;
		}
		ulong instanceId = CurrentObject.GetInstanceId();
		Dictionary dictionary = new Dictionary();
		foreach (string key in _groupContainerDictionary.Keys)
		{
			XWInspectorGroupContainer xWInspectorGroupContainer = _groupContainerDictionary[key];
			if (GodotObject.IsInstanceValid(xWInspectorGroupContainer))
			{
				dictionary["g:" + key] = xWInspectorGroupContainer.IsFolded();
			}
		}
		foreach (string key2 in _subgroupContainerDictionary.Keys)
		{
			XWInspectorSubgroupContainer xWInspectorSubgroupContainer = _subgroupContainerDictionary[key2];
			if (GodotObject.IsInstanceValid(xWInspectorSubgroupContainer))
			{
				dictionary["s:" + key2] = xWInspectorSubgroupContainer.IsFolded();
			}
		}
		_foldStateCache[instanceId] = dictionary;
	}

	private void RestoreFoldState()
	{
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			return;
		}
		ulong instanceId = CurrentObject.GetInstanceId();
		if (!_foldStateCache.ContainsKey(instanceId))
		{
			return;
		}
		Dictionary dictionary = _foldStateCache[instanceId].As<Dictionary>();
		foreach (Variant key3 in dictionary.Keys)
		{
			string text = (string)key3;
			bool folded = (bool)dictionary[key3];
			if (text.StartsWith("g:"))
			{
				string key = text.Substring(2);
				if (_groupContainerDictionary.TryGetValue(key, out var value))
				{
					value.SetFolded(folded);
				}
			}
			else if (text.StartsWith("s:"))
			{
				string key2 = text.Substring(2);
				if (_subgroupContainerDictionary.TryGetValue(key2, out var value2))
				{
					value2.SetFolded(folded);
				}
			}
		}
	}

	private void SaveScrollPosition()
	{
		if (GodotObject.IsInstanceValid(CurrentObject))
		{
			ScrollContainer scrollContainer = PropertyContainer.GetParent() as ScrollContainer;
			if (GodotObject.IsInstanceValid(scrollContainer))
			{
				_scrollCache[CurrentObject.GetInstanceId()] = scrollContainer.ScrollVertical;
			}
		}
	}

	private void RestoreScrollPosition()
	{
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			return;
		}
		ulong instanceId = CurrentObject.GetInstanceId();
		if (_scrollCache.ContainsKey(instanceId))
		{
			ScrollContainer scrollContainer = PropertyContainer.GetParent() as ScrollContainer;
			if (GodotObject.IsInstanceValid(scrollContainer))
			{
				scrollContainer.ScrollVertical = (int)_scrollCache[instanceId].AsInt64();
			}
		}
	}

	public XWInspector GetRootInspector()
	{
		if (GodotObject.IsInstanceValid(RootInspector))
		{
			return RootInspector;
		}
		return this;
	}

	public int GetSubInspectorColorLevel()
	{
		int num = 0;
		for (Node node = this; node != null; node = node.GetParent())
		{
			if (node is XWInspectorPropertyEditorBase { IsResourceEditor: not false })
			{
				num++;
				if (num >= SubInspectorColors.Length)
				{
					break;
				}
			}
		}
		return num;
	}

	public Color GetSubInspectorColor()
	{
		int subInspectorColorLevel = GetSubInspectorColorLevel();
		return SubInspectorColors[subInspectorColorLevel % SubInspectorColors.Length];
	}

	public void ToggleFavorite(string propertyName)
	{
		if (_favorites.ContainsKey(propertyName))
		{
			_favorites.Remove(propertyName);
		}
		else
		{
			_favorites[propertyName] = true;
		}
		SaveFavoritesAndPins();
	}

	public bool IsFavorite(string propertyName)
	{
		return _favorites.ContainsKey(propertyName);
	}

	public void TogglePin(string propertyName)
	{
		if (_pinnedProperties.ContainsKey(propertyName))
		{
			_pinnedProperties.Remove(propertyName);
		}
		else
		{
			_pinnedProperties[propertyName] = true;
		}
		SaveFavoritesAndPins();
	}

	public bool IsPinned(string propertyName)
	{
		return _pinnedProperties.ContainsKey(propertyName);
	}

	public void PinProperty(string propertyName)
	{
		_pinned_properties[propertyName] = true;
		if (!_pinnedProperties.ContainsKey(propertyName))
		{
			_pinnedProperties[propertyName] = true;
		}
	}

	public void UnpinProperty(string propertyName)
	{
		_pinned_properties.Remove(propertyName);
		if (_pinnedProperties.ContainsKey(propertyName))
		{
			_pinnedProperties.Remove(propertyName);
		}
	}

	public bool IsPropertyPinned(string propertyName)
	{
		if (!_pinned_properties.ContainsKey(propertyName))
		{
			return _pinnedProperties.ContainsKey(propertyName);
		}
		return true;
	}

	public string[] GetPinnedProperties()
	{
		List<string> list = new List<string>();
		foreach (Variant key in _pinned_properties.Keys)
		{
			string item = (string)key;
			list.Add(item);
		}
		foreach (Variant key2 in _pinnedProperties.Keys)
		{
			string item2 = (string)key2;
			if (!list.Contains(item2))
			{
				list.Add(item2);
			}
		}
		return list.ToArray();
	}

	public void FavoriteProperty(string propertyName)
	{
		_favoriteProperties[propertyName] = true;
		if (!_favorites.ContainsKey(propertyName))
		{
			_favorites[propertyName] = true;
		}
	}

	public void UnfavoriteProperty(string propertyName)
	{
		_favoriteProperties.Remove(propertyName);
		if (_favorites.ContainsKey(propertyName))
		{
			_favorites.Remove(propertyName);
		}
	}

	public bool IsPropertyFavorited(string propertyName)
	{
		if (!_favoriteProperties.ContainsKey(propertyName))
		{
			return _favorites.ContainsKey(propertyName);
		}
		return true;
	}

	public string[] GetFavoriteProperties()
	{
		List<string> list = new List<string>();
		foreach (Variant key in _favoriteProperties.Keys)
		{
			string item = (string)key;
			list.Add(item);
		}
		foreach (Variant key2 in _favorites.Keys)
		{
			string item2 = (string)key2;
			if (!list.Contains(item2))
			{
				list.Add(item2);
			}
		}
		return list.ToArray();
	}

	public void ShowMetadataEditor(GodotObject obj)
	{
	}

	public void SelectEditor(XWInspectorPropertyEditorBase editor)
	{
		if (GodotObject.IsInstanceValid(_selectedEditor) && _selectedEditor != editor)
		{
			_selectedEditor.Deselect();
		}
		_selectedEditor = editor;
		if (GodotObject.IsInstanceValid(editor))
		{
			editor.DoSelect();
		}
	}

	private void ConnectPropertyEditorSignals(XWInspectorPropertyEditorBase editor)
	{
		if (GodotObject.IsInstanceValid(editor))
		{
			editor.ValueChanged -= OnPropertyEditorValueChanged;
			editor.ValueChanged += OnPropertyEditorValueChanged;
		}
	}

	private void OnPropertyEditorValueChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
		if (_changing <= 0)
		{
			if (_animationPreviewEnabled && obj == CurrentObject && obj is AdobeAnimateData)
			{
				_animationPreview?.RefreshAnimation();
			}
			QueueInspectorRefreshAfterValueChange(property, field);
			EmitSignal(SignalName.ObjectPropertyChanged, obj, property, field, value);
			EmitSignal(SignalName.ObjectChanged);
		}
	}

	private void QueueInspectorRefreshAfterValueChange(StringName property, StringName field)
	{
		if (!(property == null) && !(property == (StringName)""))
		{
			_pendingUpdates[property.ToString()] = true;
			if (field != null && field != (StringName)"")
			{
				_pendingUpdates[$"{property}:{field}"] = true;
			}
		}
	}

	private void ConnectObject()
	{
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			return;
		}
		_connectedObject = CurrentObject;
		if (CurrentObject.HasSignal("property_list_changed"))
		{
			Callable callable = new Callable(this, MethodName.OnPropertyListChanged);
			if (!CurrentObject.IsConnected("property_list_changed", callable))
			{
				CurrentObject.Connect("property_list_changed", callable);
			}
		}
	}

	private void DisconnectObject()
	{
		if (GodotObject.IsInstanceValid(_connectedObject) && _connectedObject.HasSignal("property_list_changed"))
		{
			Callable callable = new Callable(this, MethodName.OnPropertyListChanged);
			if (_connectedObject.IsConnected("property_list_changed", callable))
			{
				_connectedObject.Disconnect("property_list_changed", callable);
			}
		}
		_connectedObject = null;
	}

	private void OnPropertyListChanged()
	{
		_updateTreePending = true;
	}

	private void OnPreviewReady(string path, Texture2D previewTexture, Texture2D previewSmall)
	{
		foreach (KeyValuePair<string, XWInspectorPropertyEditorBase> item in _propertyEditorDictionary)
		{
			if (GodotObject.IsInstanceValid(item.Value) && item.Value is IXWInspectorPropertyEditorPreviewable iXWInspectorPropertyEditorPreviewable)
			{
				iXWInspectorPropertyEditorPreviewable.OnPreviewReady(path, previewTexture, previewSmall);
			}
		}
	}

	public void UpDateProperties()
	{
		Clear();
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			UpdateObjectHeader();
			return;
		}
		if (MultiNodeEdit != null)
		{
			UpDateMultiNodeProperties();
			return;
		}
		List<XWInspectorProperty> list = new List<XWInspectorProperty>();
		if (CurrentObject.HasMethod("GetProperties"))
		{
			Variant variant = CurrentObject.Call("GetProperties");
			if (variant.VariantType == Variant.Type.Array)
			{
				foreach (Variant item in variant.As<Godot.Collections.Array>())
				{
					XWInspectorProperty xWInspectorProperty = item.As<XWInspectorProperty>();
					if (GodotObject.IsInstanceValid(xWInspectorProperty))
					{
						list.Add(xWInspectorProperty);
					}
				}
			}
		}
		else
		{
			list = GetObjectProperties(CurrentObject);
		}
		list = FilterInspectorProperties(list);
		List<XWInspectorPlugin> list2 = new List<XWInspectorPlugin>();
		foreach (XWInspectorPlugin inspectorPlugin in _inspectorPlugins)
		{
			if (GodotObject.IsInstanceValid(inspectorPlugin) && inspectorPlugin.CanHandle(CurrentObject))
			{
				inspectorPlugin.SetInspector(this);
				list2.Add(inspectorPlugin);
				Control control = inspectorPlugin.ParseBegin(CurrentObject);
				if (GodotObject.IsInstanceValid(control))
				{
					PropertyContainer.AddChild(control, forceReadableName: false, InternalMode.Disabled);
				}
			}
		}
		XWInspectorPropertyEditorBase xWInspectorPropertyEditorBase = CreateCurrentObjectInternalResourceEditor(CurrentObject, out var property);
		if (GodotObject.IsInstanceValid(xWInspectorPropertyEditorBase))
		{
			xWInspectorPropertyEditorBase.UndoRedoManager = UndoRedoManager;
			PropertyContainer.AddChild(xWInspectorPropertyEditorBase, forceReadableName: false, InternalMode.Disabled);
			xWInspectorPropertyEditorBase.SetEditProperty(property);
			ConnectPropertyEditorSignals(xWInspectorPropertyEditorBase);
			_propertyEditorDictionary["__internal_resource_editor"] = xWInspectorPropertyEditorBase;
		}
		StringName stringName = "";
		XWInspectorGroupContainer xWInspectorGroupContainer = null;
		XWInspectorSubgroupContainer xWInspectorSubgroupContainer = null;
		StringName stringName2 = "";
		StringName stringName3 = "";
		foreach (XWInspectorProperty item2 in list)
		{
			if (item2.CategoryName != stringName)
			{
				stringName = item2.CategoryName;
				xWInspectorGroupContainer = null;
				xWInspectorSubgroupContainer = null;
				stringName2 = "";
				stringName3 = "";
				if (stringName != (StringName)"")
				{
					XWInspectorCategory xWInspectorCategory = XWInspectorCategory.Create();
					Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(stringName.ToString());
					string text = stringName.ToString();
					if (PropertyNameStyleMode == PropertyNameStyle.StyleCapitalized)
					{
						text = CapitalizeName(text);
					}
					PropertyContainer.AddChild(xWInspectorCategory, forceReadableName: false, InternalMode.Disabled);
					xWInspectorCategory.Setup(text, classIcon);
					foreach (XWInspectorPlugin item3 in list2)
					{
						item3.ParseCategory(CurrentObject, stringName.ToString());
					}
				}
			}
			if (item2.GroupName != (StringName)"")
			{
				if (!GodotObject.IsInstanceValid(xWInspectorGroupContainer) || stringName2 != item2.GroupName)
				{
					XWInspectorGroupContainer xWInspectorGroupContainer2 = XWInspectorGroupContainer.Create();
					PropertyContainer.AddChild(xWInspectorGroupContainer2, forceReadableName: false, InternalMode.Disabled);
					string text2 = item2.GroupName.ToString();
					if (PropertyNameStyleMode == PropertyNameStyle.StyleCapitalized)
					{
						text2 = CapitalizeName(text2);
					}
					xWInspectorGroupContainer2.Init(new XWInspectorGroup(text2));
					xWInspectorGroupContainer = xWInspectorGroupContainer2;
					xWInspectorSubgroupContainer = null;
					stringName2 = item2.GroupName;
					stringName3 = "";
					_groupContainerDictionary[item2.GroupName.ToString()] = xWInspectorGroupContainer2;
					foreach (XWInspectorPlugin item4 in list2)
					{
						item4.ParseGroup(CurrentObject, item2.GroupName.ToString());
					}
				}
			}
			else
			{
				xWInspectorGroupContainer = null;
				xWInspectorSubgroupContainer = null;
				stringName2 = "";
				stringName3 = "";
			}
			if (item2.SubgroupName != (StringName)"")
			{
				if (!GodotObject.IsInstanceValid(xWInspectorSubgroupContainer) || stringName3 != item2.SubgroupName)
				{
					XWInspectorSubgroupContainer xWInspectorSubgroupContainer2 = XWInspectorSubgroupContainer.Create();
					PropertyContainer.AddChild(xWInspectorSubgroupContainer2, forceReadableName: false, InternalMode.Disabled);
					string text3 = item2.SubgroupName.ToString();
					if (PropertyNameStyleMode == PropertyNameStyle.StyleCapitalized)
					{
						text3 = CapitalizeName(text3);
					}
					xWInspectorSubgroupContainer2.Init(new XWInspectorSubgroup(text3));
					xWInspectorSubgroupContainer2.SetIndentDepth(item2.SectionDepth + 1);
					xWInspectorSubgroupContainer = xWInspectorSubgroupContainer2;
					stringName3 = item2.SubgroupName;
					_subgroupContainerDictionary[item2.SubgroupName.ToString()] = xWInspectorSubgroupContainer2;
				}
			}
			else
			{
				xWInspectorSubgroupContainer = null;
				stringName3 = "";
			}
			bool flag = false;
			if (list2.Count > 0)
			{
				Variant.Type type = Variant.Type.Nil;
				if (GodotObject.IsInstanceValid(item2.Object))
				{
					type = item2.Object.Get(item2.PropName).VariantType;
				}
				foreach (XWInspectorPlugin item5 in list2)
				{
					if (item5.ParseProperty(item2.Object, type, item2.PropName.ToString(), item2.Hint, item2.HintString, item2.PropertyUsage, wide: false))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				continue;
			}
			XWInspectorPropertyEditorBase editorFromObjectProperty = XWTypeRegistry.Instance.GetEditorFromObjectProperty(item2.Object, item2.PropName, (int)item2.Hint, item2.HintString);
			if (!GodotObject.IsInstanceValid(editorFromObjectProperty))
			{
				continue;
			}
			editorFromObjectProperty.UndoRedoManager = UndoRedoManager;
			if (item2.GroupName != (StringName)"" && GodotObject.IsInstanceValid(xWInspectorGroupContainer))
			{
				if (item2.SubgroupName != (StringName)"" && GodotObject.IsInstanceValid(xWInspectorSubgroupContainer))
				{
					xWInspectorSubgroupContainer.AddEditor(editorFromObjectProperty);
				}
				else
				{
					xWInspectorGroupContainer.EditorContainer.AddChild(editorFromObjectProperty, forceReadableName: false, InternalMode.Disabled);
				}
			}
			else
			{
				PropertyContainer.AddChild(editorFromObjectProperty, forceReadableName: false, InternalMode.Disabled);
			}
			editorFromObjectProperty.SetEditProperty(item2);
			ConnectPropertyEditorSignals(editorFromObjectProperty);
			string propertyKey = GetPropertyKey(item2);
			_propertyEditorDictionary[propertyKey] = editorFromObjectProperty;
		}
		foreach (XWInspectorPlugin item6 in list2)
		{
			Control control2 = item6.ParseEnd(CurrentObject);
			if (GodotObject.IsInstanceValid(control2))
			{
				PropertyContainer.AddChild(control2, forceReadableName: false, InternalMode.Disabled);
			}
		}
		if (GodotObject.IsInstanceValid(_searchLineEdit))
		{
			string text4 = _searchLineEdit.Text;
			if (!string.IsNullOrEmpty(text4))
			{
				Search(text4);
			}
		}
		CreateFavoritesSection();
	}

	private XWInspectorPropertyEditorBase CreateCurrentObjectInternalResourceEditor(GodotObject currentObject, out XWInspectorProperty property)
	{
		property = null;
		if (IsCurrentObjectInternalResourceEditorTarget(currentObject))
		{
			PackedScene editor = XWInspectorPropertyRegistry.GetEditor(currentObject);
			if (editor != null)
			{
				property = CreateInternalResourceProperty(currentObject);
				return editor.Instantiate<XWInspectorPropertyEditorBase>(PackedScene.GenEditState.Disabled);
			}
		}
		if (currentObject is Texture2D texture && XWTextureSafety.CanPreview(texture))
		{
			property = CreateInternalResourceProperty(currentObject);
			return XWInspectorPropertyEditorShowTexture2D.Create();
		}
		return null;
	}

	private static bool IsCurrentObjectInternalResourceEditorTarget(GodotObject currentObject)
	{
		if (!(currentObject is Gradient) && !(currentObject is Curve) && !(currentObject is AudioStream) && !(currentObject is GradientTexture1D) && !(currentObject is GradientTexture2D) && !(currentObject is CurveTexture) && !(currentObject is CurveXyzTexture) && !(currentObject is StyleBox) && !(currentObject is Material))
		{
			return currentObject is Theme;
		}
		return true;
	}

	private static XWInspectorProperty CreateInternalResourceProperty(GodotObject currentObject)
	{
		return new XWInspectorProperty(currentObject, "", default, null, null, PropertyHint.None)
		{
			LabelOverride = currentObject.GetClass(),
			PropertyUsage = 4L
		};
	}

	private List<XWInspectorProperty> GetObjectProperties(GodotObject obj)
	{
		List<XWInspectorProperty> list = new List<XWInspectorProperty>();
		if (!GodotObject.IsInstanceValid(obj))
		{
			return list;
		}
		StringName stringName = "";
		StringName stringName2 = "";
		StringName stringName3 = "";
		StringName stringName4 = "";
		StringName stringName5 = "";
		int result = 0;
		Array<Dictionary> propertyList = obj.GetPropertyList();
		propertyList = ReversePropertyListByCategory(propertyList);
		for (int i = 0; i < propertyList.Count; i++)
		{
			Dictionary dictionary = propertyList[i];
			StringName stringName6 = dictionary["name"].AsStringName();
			long num = dictionary["usage"].AsInt64();
			string text = (dictionary.ContainsKey("hint_string") ? dictionary["hint_string"].AsString() : "");
			if ((num & 0x80) != 0L)
			{
				stringName2 = "";
				stringName3 = "";
				stringName4 = "";
				stringName5 = "";
				result = 0;
				bool flag = string.IsNullOrEmpty(text);
				int j = i + 1;
				bool flag2 = true;
				for (; j < propertyList.Count; j++)
				{
					Dictionary dictionary2 = propertyList[j];
					long num2 = dictionary2["usage"].AsInt64();
					if (!dictionary2["name"].AsString().StartsWith("metadata/_") && (num2 & 4) != 0L)
					{
						break;
					}
					if ((num2 & 0x80) != 0L)
					{
						string value = (dictionary2.ContainsKey("hint_string") ? dictionary2["hint_string"].AsString() : "");
						if (flag || !string.IsNullOrEmpty(value))
						{
							flag2 = false;
							break;
						}
					}
				}
				if (!flag2)
				{
					continue;
				}
				if (!flag || ShowCustomCategories)
				{
					if (flag)
					{
						stringName = stringName6;
						continue;
					}
					_ = ShowStandardCategories;
				}
				stringName = stringName6;
			}
			else if ((num & 0x40) != 0L)
			{
				stringName2 = stringName6;
				stringName3 = "";
				stringName5 = "";
				stringName4 = "";
				result = 0;
				if (text != "")
				{
					stringName4 = text.Split(",")[0];
					if (text.Split(",").Length > 1)
					{
						int.TryParse(text.Split(",")[1], out result);
					}
				}
			}
			else if ((num & 0x100) != 0L)
			{
				stringName3 = stringName6;
				stringName5 = "";
				if (text != "")
				{
					stringName5 = text.Split(",")[0];
					if (text.Split(",").Length > 1)
					{
						int.TryParse(text.Split(",")[1], out result);
					}
				}
			}
			else if ((num & 4) != 0L && !(stringName6 == (StringName)"script") && !stringName6.ToString().StartsWith("metadata/_") && (!obj.HasMethod("_hide_metadata_from_inspector") || !stringName6.ToString().StartsWith("metadata/") || !obj.Call("_hide_metadata_from_inspector").AsBool()))
			{
				StringName stringName7 = stringName;
				StringName groupName = stringName2;
				StringName stringName8 = stringName3;
				StringName stringName9 = stringName4;
				StringName stringName10 = stringName5;
				int sectionDepth = result;
				if (stringName7 == (StringName)"Object" || stringName7 == (StringName)"")
				{
					stringName7 = "";
				}
				string text2 = stringName6.ToString();
				if (stringName10 != (StringName)"" && !text2.StartsWith(stringName10.ToString()) && !stringName10.ToString().StartsWith(text2))
				{
					stringName3 = "";
					stringName5 = "";
					stringName8 = "";
					stringName10 = "";
				}
				if (stringName9 != (StringName)"" && stringName8 == (StringName)"" && !text2.StartsWith(stringName9.ToString()) && !stringName9.ToString().StartsWith(text2))
				{
					stringName2 = "";
					stringName4 = "";
					stringName3 = "";
					stringName5 = "";
					groupName = "";
					stringName9 = "";
					stringName8 = "";
					stringName10 = "";
				}
				int num3 = (dictionary.ContainsKey("hint") ? dictionary["hint"].AsInt32() : 0);
				string description = (dictionary.ContainsKey("description") ? dictionary["description"].AsString() : "");
				XWInspectorProperty xWInspectorProperty = new XWInspectorProperty(obj, stringName6, default, groupName, stringName8, (PropertyHint)num3, text)
				{
					CategoryName = stringName7,
					GroupBase = stringName9,
					SubgroupBase = stringName10,
					SectionDepth = sectionDepth,
					PropertyUsage = num,
					Description = description
				};
				if ((num & 0x10000000) != 0L)
				{
					xWInspectorProperty.ReadOnly = true;
				}
				string text3 = text2;
				if (stringName10 != (StringName)"" && text2.StartsWith(stringName10.ToString()))
				{
					text3 = text2.Substring(stringName10.ToString().Length);
				}
				else if (stringName9 != (StringName)"" && stringName8 == (StringName)"" && text2.StartsWith(stringName9.ToString()))
				{
					text3 = text2.Substring(stringName9.ToString().Length);
				}
				if (string.IsNullOrEmpty(text3))
				{
					text3 = text2;
				}
				xWInspectorProperty.LabelOverride = text3;
				list.Add(xWInspectorProperty);
			}
		}
		return list;
	}

	private void UpDateMultiNodeProperties()
	{
		if (MultiNodeEdit == null)
		{
			return;
		}
		Array<Dictionary> commonProperties = MultiNodeEdit.GetCommonProperties();
		if (commonProperties.Count == 0)
		{
			return;
		}
		StringName stringName = "";
		XWInspectorGroupContainer xWInspectorGroupContainer = null;
		XWInspectorSubgroupContainer xWInspectorSubgroupContainer = null;
		int result = 0;
		foreach (Dictionary item in commonProperties)
		{
			StringName stringName2 = item["name"].AsStringName();
			long num = item["usage"].AsInt64();
			int num2 = (item.ContainsKey("hint") ? item["hint"].AsInt32() : 0);
			string text = (item.ContainsKey("hint_string") ? item["hint_string"].AsString() : "");
			if ((num & 0x80) != 0L)
			{
				stringName = stringName2;
				xWInspectorGroupContainer = null;
				xWInspectorSubgroupContainer = null;
				result = 0;
				XWInspectorCategory xWInspectorCategory = XWInspectorCategory.Create();
				Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(stringName.ToString());
				string text2 = stringName.ToString();
				if (PropertyNameStyleMode == PropertyNameStyle.StyleCapitalized)
				{
					text2 = CapitalizeName(text2);
				}
				PropertyContainer.AddChild(xWInspectorCategory, forceReadableName: false, InternalMode.Disabled);
				xWInspectorCategory.Setup(text2, classIcon);
			}
			else if ((num & 0x40) != 0L)
			{
				XWInspectorGroupContainer xWInspectorGroupContainer2 = XWInspectorGroupContainer.Create();
				PropertyContainer.AddChild(xWInspectorGroupContainer2, forceReadableName: false, InternalMode.Disabled);
				string text3 = stringName2.ToString();
				if (PropertyNameStyleMode == PropertyNameStyle.StyleCapitalized)
				{
					text3 = CapitalizeName(text3);
				}
				xWInspectorGroupContainer2.Init(new XWInspectorGroup(text3));
				xWInspectorGroupContainer = xWInspectorGroupContainer2;
				xWInspectorSubgroupContainer = null;
				result = 0;
				if (text != "" && text.Split(",").Length > 1)
				{
					int.TryParse(text.Split(",")[1], out result);
				}
				_groupContainerDictionary[stringName2.ToString()] = xWInspectorGroupContainer2;
			}
			else if ((num & 0x100) != 0L)
			{
				XWInspectorSubgroupContainer xWInspectorSubgroupContainer2 = XWInspectorSubgroupContainer.Create();
				PropertyContainer.AddChild(xWInspectorSubgroupContainer2, forceReadableName: false, InternalMode.Disabled);
				string text4 = stringName2.ToString();
				if (PropertyNameStyleMode == PropertyNameStyle.StyleCapitalized)
				{
					text4 = CapitalizeName(text4);
				}
				xWInspectorSubgroupContainer2.Init(new XWInspectorSubgroup(text4));
				int result2 = result + 1;
				if (text != "" && text.Split(",").Length > 1)
				{
					int.TryParse(text.Split(",")[1], out result2);
				}
				xWInspectorSubgroupContainer2.SetIndentDepth(result2 + 1);
				xWInspectorSubgroupContainer = xWInspectorSubgroupContainer2;
				_subgroupContainerDictionary[stringName2.ToString()] = xWInspectorSubgroupContainer2;
			}
			else
			{
				if ((num & 4) == 0L || stringName2 == (StringName)"script")
				{
					continue;
				}
				bool flag = MultiNodeEdit.IsPropertySameForAll(stringName2);
				XWInspectorProperty xWInspectorProperty = new XWInspectorProperty(MultiNodeEdit.GetFirstNode(), stringName2, default, "", "", (PropertyHint)num2, text)
				{
					CategoryName = stringName
				};
				if (!ShouldShowInspectorProperty(xWInspectorProperty))
				{
					continue;
				}
				XWInspectorPropertyEditorBase editorFromObjectProperty = XWTypeRegistry.Instance.GetEditorFromObjectProperty(MultiNodeEdit.GetFirstNode(), stringName2, num2, text);
				if (!GodotObject.IsInstanceValid(editorFromObjectProperty))
				{
					continue;
				}
				editorFromObjectProperty.UndoRedoManager = UndoRedoManager;
				if (GodotObject.IsInstanceValid(xWInspectorGroupContainer))
				{
					if (GodotObject.IsInstanceValid(xWInspectorSubgroupContainer))
					{
						xWInspectorSubgroupContainer.AddEditor(editorFromObjectProperty);
					}
					else
					{
						xWInspectorGroupContainer.EditorContainer.AddChild(editorFromObjectProperty, forceReadableName: false, InternalMode.Disabled);
					}
				}
				else
				{
					PropertyContainer.AddChild(editorFromObjectProperty, forceReadableName: false, InternalMode.Disabled);
				}
				editorFromObjectProperty.SetEditProperty(xWInspectorProperty);
				ConnectPropertyEditorSignals(editorFromObjectProperty);
				if (!flag)
				{
					editorFromObjectProperty.SetMixedValue();
				}
				string propertyKey = GetPropertyKey(xWInspectorProperty);
				_propertyEditorDictionary[propertyKey] = editorFromObjectProperty;
			}
		}
	}

	private List<XWInspectorProperty> FilterInspectorProperties(List<XWInspectorProperty> properties)
	{
		if (properties == null)
		{
			return new List<XWInspectorProperty>();
		}
		if (PropertyFilter == null)
		{
			return properties;
		}
		List<XWInspectorProperty> list = new List<XWInspectorProperty>();
		foreach (XWInspectorProperty property in properties)
		{
			if (ShouldShowInspectorProperty(property))
			{
				list.Add(property);
			}
		}
		return list;
	}

	private bool ShouldShowInspectorProperty(XWInspectorProperty property)
	{
		if (property != null)
		{
			if (PropertyFilter != null)
			{
				return PropertyFilter(property);
			}
			return true;
		}
		return false;
	}

	public void Clear()
	{
		foreach (Node child in PropertyContainer.GetChildren())
		{
			if (child != null)
			{
				Node node = child;
				node.QueueFree();
			}
		}
		_groupContainerDictionary.Clear();
		_subgroupContainerDictionary.Clear();
		_propertyEditorDictionary.Clear();
		_selectedEditor = null;
	}

	private string GetPropertyKey(XWInspectorProperty prop)
	{
		if (prop.GroupName != (StringName)"")
		{
			return $"{prop.GroupName}:{prop.PropName}";
		}
		return prop.PropName.ToString();
	}

	private Array<Dictionary> ReversePropertyListByCategory(Array<Dictionary> propertyList)
	{
		Array<Dictionary> array = new Array<Dictionary>();
		List<Array<Dictionary>> list = new List<Array<Dictionary>>();
		Array<Dictionary> array2 = new Array<Dictionary>();
		foreach (Dictionary property in propertyList)
		{
			if ((property["usage"].AsInt64() & 0x80) != 0L)
			{
				if (array2.Count > 0)
				{
					list.Add(array2);
				}
				array2 = new Array<Dictionary> { property };
			}
			else if (array2.Count == 0)
			{
				array.Add(property);
			}
			else
			{
				array2.Add(property);
			}
		}
		if (array2.Count > 0)
		{
			list.Add(array2);
		}
		list.Reverse();
		Array<Dictionary> array3 = new Array<Dictionary>();
		foreach (Dictionary item in array)
		{
			array3.Add(item);
		}
		foreach (Array<Dictionary> item2 in list)
		{
			foreach (Dictionary item3 in item2)
			{
				array3.Add(item3);
			}
		}
		return array3;
	}

	private void CreateFavoritesSection()
	{
		string[] favoriteProperties = GetFavoriteProperties();
		if (favoriteProperties.Length == 0)
		{
			return;
		}
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			Name = "FavoritesSection"
		};
		Label label = new Label
		{
			Text = "收藏"
		};
		label.AddThemeColorOverride("font_color", new Color(0.45f, 0.5f, 0.62f));
		vBoxContainer.AddChild(label, forceReadableName: false, InternalMode.Disabled);
		string[] array = favoriteProperties;
		foreach (string text in array)
		{
			if (_propertyEditorDictionary.TryGetValue(text, out var value) && GodotObject.IsInstanceValid(value))
			{
				Label node = new Label
				{
					Text = text
				};
				vBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			}
		}
		PropertyContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		PropertyContainer.MoveChild(vBoxContainer, 0);
	}

	private void LoadFavoritesAndPins()
	{
		XWEditorSettings xWEditorSettings = XWEditorInterface.Instance?.GetEditorSettings();
		if (!GodotObject.IsInstanceValid(xWEditorSettings))
		{
			return;
		}
		Variant setting = xWEditorSettings.GetSetting("inspector/favorites", (Variant)new Dictionary());
		if (setting.VariantType == Variant.Type.Dictionary)
		{
			foreach (Variant key in setting.As<Dictionary>().Keys)
			{
				string text = (string)key;
				_favoriteProperties[text] = true;
				_favorites[text] = true;
			}
		}
		Variant setting2 = xWEditorSettings.GetSetting("inspector/pinned", (Variant)new Dictionary());
		if (setting2.VariantType != Variant.Type.Dictionary)
		{
			return;
		}
		foreach (Variant key2 in setting2.As<Dictionary>().Keys)
		{
			string text2 = (string)key2;
			_pinned_properties[text2] = true;
			_pinnedProperties[text2] = true;
		}
	}

	private void SaveFavoritesAndPins()
	{
		XWEditorSettings xWEditorSettings = XWEditorInterface.Instance?.GetEditorSettings();
		if (!GodotObject.IsInstanceValid(xWEditorSettings))
		{
			return;
		}
		Dictionary dictionary = new Dictionary();
		foreach (Variant key in _favoriteProperties.Keys)
		{
			string text = (string)key;
			dictionary[text] = true;
		}
		foreach (Variant key2 in _favorites.Keys)
		{
			string text2 = (string)key2;
			dictionary[text2] = true;
		}
		xWEditorSettings.SetSetting("inspector/favorites", dictionary);
		Dictionary dictionary2 = new Dictionary();
		foreach (Variant key3 in _pinned_properties.Keys)
		{
			string text3 = (string)key3;
			dictionary2[text3] = true;
		}
		foreach (Variant key4 in _pinnedProperties.Keys)
		{
			string text4 = (string)key4;
			dictionary2[text4] = true;
		}
		xWEditorSettings.SetSetting("inspector/pinned", dictionary2);
		xWEditorSettings.Save();
	}

	public void SetPropertyNameStyle(PropertyNameStyle style)
	{
		PropertyNameStyleMode = style;
		UpDateProperties();
	}

	public PropertyNameStyle GetPropertyNameStyle()
	{
		return PropertyNameStyleMode;
	}

	public void SetShowCategories(bool showStandard, bool showCustom)
	{
		ShowStandardCategories = showStandard;
		ShowCustomCategories = showCustom;
		UpDateProperties();
	}

	public bool Undo()
	{
		if (GodotObject.IsInstanceValid(UndoRedoManager))
		{
			return UndoRedoManager.Undo();
		}
		return false;
	}

	public bool Redo()
	{
		if (GodotObject.IsInstanceValid(UndoRedoManager))
		{
			return UndoRedoManager.Redo();
		}
		return false;
	}

	public bool HasUndo()
	{
		if (GodotObject.IsInstanceValid(UndoRedoManager))
		{
			return UndoRedoManager.HasUndo();
		}
		return false;
	}

	public bool HasRedo()
	{
		if (GodotObject.IsInstanceValid(UndoRedoManager))
		{
			return UndoRedoManager.HasRedo();
		}
		return false;
	}

	public XWUndoRedoManager GetUndoRedoManager()
	{
		return UndoRedoManager;
	}

	public void ShowPropertyContextMenu(string propertyName, Vector2 menuPos, XWInspectorPropertyEditorBase editor)
	{
		_contextMenuPropertyName = propertyName;
		_contextMenuEditor = editor;
		_propertyContextMenu.Clear();
		_propertyContextMenu.AddItem("复制属性", 0, Key.None);
		_propertyContextMenu.AddItem("复制值", 1, Key.None);
		_propertyContextMenu.AddItem("粘贴值", 2, Key.None);
		_propertyContextMenu.AddSeparator();
		if (GodotObject.IsInstanceValid(editor) && GodotObject.IsInstanceValid(editor.ResetButton) && editor.ResetButton.Visible)
		{
			_propertyContextMenu.AddItem("重置为默认值", 3, Key.None);
			_propertyContextMenu.AddSeparator();
		}
		if (IsPinned(propertyName))
		{
			_propertyContextMenu.AddItem("取消置顶", 4, Key.None);
		}
		else
		{
			_propertyContextMenu.AddItem("置顶属性", 4, Key.None);
		}
		if (IsFavorite(propertyName))
		{
			_propertyContextMenu.AddItem("取消收藏", 5, Key.None);
		}
		else
		{
			_propertyContextMenu.AddItem("收藏属性", 5, Key.None);
		}
		_propertyContextMenu.AddSeparator();
		_propertyContextMenu.AddItem("复制属性路径", 6, Key.None);
		_propertyContextMenu.AddItem("打开文档", 7, Key.None);
		_propertyContextMenu.AddItem("删除属性", 8, Key.None);
		_propertyContextMenu.ResetSize();
		_propertyContextMenu.Position = new Vector2I((int)menuPos.X, (int)menuPos.Y);
		_propertyContextMenu.Popup();
	}

	private void OnPropertyContextMenuItem(long id)
	{
		if ((ulong)id <= 8uL)
		{
			switch ((int)id)
			{
			case 0:
				DisplayServer.ClipboardSet(_contextMenuPropertyName);
				break;
			case 1:
				_contextMenuEditor?.Call("_copy_value");
				break;
			case 2:
				_contextMenuEditor?.Call("_paste_value");
				break;
			case 3:
				_contextMenuEditor?.Call("reset_button_pressed");
				break;
			case 4:
				TogglePin(_contextMenuPropertyName);
				break;
			case 5:
				ToggleFavorite(_contextMenuPropertyName);
				break;
			case 6:
				_contextMenuEditor?.Call("_copy_property_path");
				break;
			case 7:
				OpenPropertyDocumentation();
				break;
			case 8:
				_contextMenuEditor?.Call("_delete_property");
				break;
			}
		}
	}

	private void OpenPropertyDocumentation()
	{
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			return;
		}
		Script script = CurrentObject.GetScript().As<Script>();
		if (GodotObject.IsInstanceValid(script) && !string.IsNullOrWhiteSpace(script.ResourcePath))
		{
			if (!XWScriptEditor.IsSupportedScriptPath(script.ResourcePath))
			{
				XWEditorInterface.Instance?.ShowToast("属性定义跳转仅支持 C# 脚本。", 1);
				return;
			}
			XWScriptEditor xWScriptEditor = XWEditorInterface.Instance?.GetScriptEditor();
			if (GodotObject.IsInstanceValid(xWScriptEditor) && xWScriptEditor.TryOpenFileAt(script.ResourcePath, 0, 0))
			{
				XWEditorInterface.Instance?.FocusPanel("script_editor");
				XWEditorInterface.Instance?.ShowToast("已打开属性定义: " + _contextMenuPropertyName);
			}
			else
			{
				XWEditorInterface.Instance?.ShowToast("无法打开属性定义: " + _contextMenuPropertyName, 1);
			}
			return;
		}
		string text = CurrentObject.GetClass();
		if (string.IsNullOrWhiteSpace(text) || !ClassDB.ClassExists(text))
		{
			XWEditorInterface.Instance?.ShowToast("未找到 " + _contextMenuPropertyName + " 的可用文档。", 1);
			return;
		}
		string text2 = text.ToLowerInvariant();
		string text3 = (_contextMenuPropertyName ?? "").Trim().ToLowerInvariant().Replace('_', '-');
		string text4 = "https://docs.godotengine.org/en/stable/classes/class_" + text2 + ".html";
		if (!string.IsNullOrWhiteSpace(text3))
		{
			text4 = text4 + "#class-" + text2 + "-property-" + text3;
		}
		Error error = OS.ShellOpen(text4);
		if (error != Error.Ok)
		{
			XWEditorInterface.Instance?.ShowToast($"无法打开文档: {error}", 1);
		}
	}

	private void PopulateNewResourceMenu()
	{
		PopupMenu popup = _newResourceButton.GetPopup();
		popup.Clear();
		int num = 0;
		foreach (string commonNewResourceType in GetCommonNewResourceTypes())
		{
			Texture2D classIcon = XWClassRegistry.Instance.GetClassIcon(commonNewResourceType);
			if (GodotObject.IsInstanceValid(classIcon))
			{
				popup.AddIconItem(classIcon, commonNewResourceType, num, Key.None);
			}
			else
			{
				popup.AddItem(commonNewResourceType, num, Key.None);
			}
			num++;
		}
		ConnectIdPressedOnce(popup, _newResourceSelectedCallable);
	}

	private static IEnumerable<string> GetCommonNewResourceTypes()
	{
		string[] array = new string[10] { "Resource", "PackedScene", "Animation", "Theme", "Gradient", "Curve", "StyleBoxFlat", "ShaderMaterial", "CanvasItemMaterial", "AudioStreamRandomizer" };
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (ClassDB.CanInstantiate(text))
			{
				yield return text;
			}
		}
	}

	private void OnNewResourceSelected(long id)
	{
		string itemText = _newResourceButton.GetPopup().GetItemText((int)id);
		EmitSignal(SignalName.ResourceNewRequested, itemText);
	}

	private void PopulateSaveResourceMenu()
	{
		PopupMenu popup = _saveResourceButton.GetPopup();
		popup.Clear();
		popup.AddItem("保存", 0, Key.None);
		popup.AddItem("另存为...", 1, Key.None);
		ConnectIdPressedOnce(popup, _saveResourceMenuCallable);
	}

	private void OnSaveResourceMenuItem(long id)
	{
		switch (id)
		{
		case 0L:
			EmitSignal(SignalName.ResourceSaveRequested);
			break;
		case 1L:
			EmitSignal(SignalName.ResourceSaveAsRequested);
			break;
		}
	}

	private void PopulateResourceExtraMenu()
	{
		PopupMenu popup = _resourceExtraButton.GetPopup();
		popup.Clear();
		popup.AddItem("从剪贴板编辑资源", 0, Key.None);
		popup.AddItem("复制资源", 1, Key.None);
		popup.AddSeparator();
		popup.AddItem("在文件系统中显示", 2, Key.None);
		popup.AddItem("将资源设为内置", 3, Key.None);
		ConnectIdPressedOnce(popup, _resourceExtraMenuCallable);
	}

	private void OnResourceExtraMenuItem(long id)
	{
		if ((ulong)id <= 3uL)
		{
			switch ((int)id)
			{
			case 0:
				EditResourceFromClipboard();
				break;
			case 1:
				EmitSignal(SignalName.ResourceCopyRequested);
				break;
			case 2:
				EmitSignal(SignalName.ResourceShowInFilesystem);
				break;
			case 3:
				EmitSignal(SignalName.ResourceMakeBuiltIn);
				break;
			}
		}
	}

	private void EditResourceFromClipboard()
	{
		string text = DisplayServer.ClipboardGet();
		if (string.IsNullOrEmpty(text))
		{
			XWEditorInterface.Instance?.ShowToast("剪贴板为空", 1);
			return;
		}
		string text2 = text.StripEdges();
		if (text2.StartsWith("res://") && (text2.EndsWith(".tres") || text2.EndsWith(".res")) && ResourceLoader.Exists(text2))
		{
			Resource resource = ResourceLoader.Load<Resource>(text2, null, ResourceLoader.CacheMode.Reuse);
			if (GodotObject.IsInstanceValid(resource))
			{
				SetObject(resource);
				XWEditorInterface.Instance?.ShowToast("已从剪贴板加载资源: " + text2.GetFile());
				return;
			}
		}
		XWEditorInterface.Instance?.ShowToast("无法从剪贴板解析资源", 1);
	}

	private void PopulateHistoryMenu()
	{
		PopupMenu popup = _historyButton.GetPopup();
		popup.Clear();
		if (_selectionHistory == null)
		{
			return;
		}
		Godot.Collections.Array history = _selectionHistory.GetHistory();
		if (history.Count == 0)
		{
			popup.AddItem("(无历史记录)", 0, Key.None);
			popup.SetItemDisabled(0, disabled: true);
			return;
		}
		for (int i = 0; i < history.Count; i++)
		{
			ulong num = (ulong)(long)history[i];
			GodotObject godotObject = GodotObject.InstanceFromId(num);
			if (GodotObject.IsInstanceValid(godotObject))
			{
				string label = godotObject.GetClass();
				if (godotObject is Node node)
				{
					label = node.Name;
				}
				else if (godotObject is Resource { ResourcePath: var resourcePath } && !string.IsNullOrEmpty(resourcePath))
				{
					label = resourcePath.GetFile().GetBaseName();
				}
				popup.AddItem(label, i, Key.None);
				if (num == _selectionHistory.GetCurrent())
				{
					popup.SetItemDisabled(popup.ItemCount - 1, disabled: true);
				}
			}
		}
		ConnectIdPressedOnce(popup, _historyMenuCallable);
	}

	private void OnHistoryMenuItem(long id)
	{
		if (_selectionHistory == null)
		{
			return;
		}
		Godot.Collections.Array history = _selectionHistory.GetHistory();
		if (id >= 0 && id < history.Count)
		{
			GodotObject godotObject = GodotObject.InstanceFromId((ulong)(long)history[(int)id]);
			if (GodotObject.IsInstanceValid(godotObject))
			{
				DisconnectObject();
				ResetPendingInspectorUpdates();
				CurrentObject = godotObject;
				UpDateProperties();
				ConnectObject();
				UpdateObjectHeader();
				EmitSignal(SignalName.ObjectChanged);
			}
			UpdateNavigationButtons(CanNavigateBack(), CanNavigateForward());
			RefreshUpdateTimerState();
		}
	}

	private void PopulateObjectMenu()
	{
		PopupMenu popup = _objectMenuButton.GetPopup();
		popup.Clear();
		Texture2D uIIcon = XWClassRegistry.Instance.GetUIIcon("ExpandTree");
		Texture2D uIIcon2 = XWClassRegistry.Instance.GetUIIcon("CollapseTree");
		if (GodotObject.IsInstanceValid(uIIcon))
		{
			popup.AddIconItem(uIIcon, "展开全部", 3, Key.None);
		}
		else
		{
			popup.AddItem("展开全部", 3, Key.None);
		}
		if (GodotObject.IsInstanceValid(uIIcon2))
		{
			popup.AddIconItem(uIIcon2, "折叠全部", 4, Key.None);
		}
		else
		{
			popup.AddItem("折叠全部", 4, Key.None);
		}
		popup.AddItem("展开非默认项", 5, Key.None);
		popup.AddSeparator();
		popup.AddItem("原始属性名", 10, Key.None);
		popup.SetItemChecked(popup.ItemCount - 1, PropertyNameStyleMode == PropertyNameStyle.StyleRaw);
		popup.AddItem("大写化属性名", 11, Key.None);
		popup.SetItemChecked(popup.ItemCount - 1, PropertyNameStyleMode == PropertyNameStyle.StyleCapitalized);
		popup.AddSeparator();
		popup.AddItem("复制属性", 0, Key.None);
		popup.AddItem("粘贴属性", 1, Key.None);
		popup.AddSeparator();
		popup.AddItem("使子资源唯一", 6, Key.None);
		ConnectIdPressedOnce(popup, _objectMenuCallable);
	}

	private void OnObjectMenuItem(long id)
	{
		if ((ulong)id <= 11uL)
		{
			switch ((int)id)
			{
			case 0:
				CopyObjectProperties();
				break;
			case 1:
				PasteObjectProperties();
				break;
			case 3:
				ExpandAllProperties();
				break;
			case 4:
				CollapseAllProperties();
				break;
			case 5:
				ExpandNonDefaultProperties();
				break;
			case 6:
				MakeSubResourcesUnique();
				break;
			case 10:
				SetPropertyNameStyle(PropertyNameStyle.StyleRaw);
				PopulateObjectMenu();
				break;
			case 11:
				SetPropertyNameStyle(PropertyNameStyle.StyleCapitalized);
				PopulateObjectMenu();
				break;
			case 2:
			case 7:
			case 8:
			case 9:
				break;
			}
		}
	}

	private void CopyObjectProperties()
	{
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			return;
		}
		Dictionary dictionary = new Dictionary();
		foreach (Dictionary property in CurrentObject.GetPropertyList())
		{
			if ((property["usage"].AsInt64() & 4) != 0L)
			{
				string text = property["name"].AsString();
				dictionary[text] = CurrentObject.Get(text);
			}
		}
		DisplayServer.ClipboardSet(Json.Stringify(dictionary));
	}

	private void PasteObjectProperties()
	{
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			return;
		}
		string text = DisplayServer.ClipboardGet();
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		Json json = new Json();
		if (json.Parse(text) != Error.Ok)
		{
			return;
		}
		Variant data = json.Data;
		if (data.VariantType != Variant.Type.Dictionary)
		{
			return;
		}
		Dictionary dictionary = data.As<Dictionary>();
		_changing++;
		Array<Dictionary> propertyList = CurrentObject.GetPropertyList();
		HashSet<string> hashSet = new HashSet<string>();
		foreach (Dictionary item in propertyList)
		{
			if ((item["usage"].AsInt64() & 4) != 0L)
			{
				hashSet.Add(item["name"].AsString());
			}
		}
		foreach (Variant key in dictionary.Keys)
		{
			string text2 = (string)key;
			if (hashSet.Contains(text2))
			{
				CurrentObject.Set(text2, dictionary[text2]);
			}
		}
		_changing--;
		UpDateProperties();
	}

	private void ExpandAllProperties()
	{
		foreach (XWInspectorGroupContainer value in _groupContainerDictionary.Values)
		{
			if (GodotObject.IsInstanceValid(value))
			{
				value.SetFolded(folded: false);
			}
		}
		foreach (XWInspectorSubgroupContainer value2 in _subgroupContainerDictionary.Values)
		{
			if (GodotObject.IsInstanceValid(value2))
			{
				value2.SetFolded(folded: false);
			}
		}
	}

	private void CollapseAllProperties()
	{
		foreach (XWInspectorGroupContainer value in _groupContainerDictionary.Values)
		{
			if (GodotObject.IsInstanceValid(value))
			{
				value.SetFolded(folded: true);
			}
		}
		foreach (XWInspectorSubgroupContainer value2 in _subgroupContainerDictionary.Values)
		{
			if (GodotObject.IsInstanceValid(value2))
			{
				value2.SetFolded(folded: true);
			}
		}
	}

	private void ExpandNonDefaultProperties()
	{
		foreach (XWInspectorGroupContainer value in _groupContainerDictionary.Values)
		{
			if (GodotObject.IsInstanceValid(value))
			{
				value.SetFolded(folded: false);
			}
		}
	}

	private void MakeSubResourcesUnique()
	{
		if (!GodotObject.IsInstanceValid(CurrentObject))
		{
			return;
		}
		_changing++;
		foreach (Dictionary property in CurrentObject.GetPropertyList())
		{
			long num = property["usage"].AsInt64();
			int num2 = (property.ContainsKey("hint") ? property["hint"].AsInt32() : 0);
			if ((num & 4) != 0L && num2 == 17)
			{
				string text = property["name"].AsString();
				Resource resource = CurrentObject.Get(text).As<Resource>();
				if (GodotObject.IsInstanceValid(resource))
				{
					CurrentObject.Set(text, resource.Duplicate());
				}
			}
		}
		_changing--;
		UpDateProperties();
	}

	public void Edit(GodotObject obj)
	{
		SetObject(obj);
	}

	public void EditObject(GodotObject obj)
	{
		SetObject(obj);
	}

	private static string CapitalizeName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return name;
		}
		if (name.Length == 1)
		{
			return name.ToUpper();
		}
		bool flag = false;
		string text2;
		for (int i = 0; i < name.Length; i++)
		{
			if (name[i] == '_')
			{
				string text = name.Substring(0, i);
				text2 = name;
				int num = i + 1;
				name = text + " " + text2.Substring(num, text2.Length - num);
				flag = true;
			}
		}
		if (!flag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < name.Length; j++)
			{
				if (j > 0 && char.IsUpper(name[j]) && !char.IsUpper(name[j - 1]))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(name[j]);
			}
			name = stringBuilder.ToString();
		}
		ReadOnlySpan<char> readOnlySpan = new ReadOnlySpan<char>(char.ToUpper(name[0]));
		text2 = name;
		return string.Concat(readOnlySpan, text2.Substring(1, text2.Length - 1));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(114)
		{
			new MethodInfo(MethodName.AddPlugin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plugin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemovePlugin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "plugin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.CreateSubInspector, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._EnterTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetupToolbarIcons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetButtonIcon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false),
				new PropertyInfo(Variant.Type.Object, "icon", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetMenuButtonIcon, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "button", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("MenuButton"), exported: false),
				new PropertyInfo(Variant.Type.Object, "icon", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectIdPressedOnce, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "popup", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PopupMenu"), exported: false),
				new PropertyInfo(Variant.Type.Callable, "callable", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindObjectHeader, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateObjectHeader, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateObjectPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateMultiNodeHeader, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateToolbarState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessUpdates, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldRunUpdateTimer, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindVisibilityHostWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectVisibilityHostWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisibilityHostWindowChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshUpdateTimerState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetPendingInspectorUpdates, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearInvalidCurrentObject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ProcessPendingPropertyUpdates, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PollPropertyChanges, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsTrackedPropertySynchronized, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Notification, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "what", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PropertyPathMatches, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "filter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSubsequence, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResourcePropertiesMatches, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "res", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "filter", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Search, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "query", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetObject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "forceRefresh", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShouldSkipSameObjectRefresh, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "forceRefresh", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsSameInspectableObject, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasInspectorEditorFocus, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsControlInsideInspector, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "control", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "nodes", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NavigateBack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NavigateForward, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanNavigateBack, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CanNavigateForward, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectionHistory, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateNavigationButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "canBack", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "canForward", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SaveFoldState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreFoldState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveScrollPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreScrollPosition, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetRootInspector, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSubInspectorColorLevel, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSubInspectorColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ToggleFavorite, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsFavorite, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TogglePin, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPinned, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PinProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UnpinProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPropertyPinned, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPinnedProperties, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FavoriteProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UnfavoriteProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPropertyFavorited, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetFavoriteProperties, new PropertyInfo(Variant.Type.PackedStringArray, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowMetadataEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.SelectEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectPropertyEditorSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnPropertyEditorValueChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.QueueInspectorRefreshAfterValueChange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectObject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectObject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPropertyListChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPreviewReady, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "previewTexture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false),
				new PropertyInfo(Variant.Type.Object, "previewSmall", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpDateProperties, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsCurrentObjectInternalResourceEditorTarget, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "currentObject", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateInternalResourceProperty, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "currentObject", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpDateMultiNodeProperties, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShouldShowInspectorProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "property", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.Clear, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetPropertyKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "prop", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReversePropertyListByCategory, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "propertyList", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateFavoritesSection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadFavoritesAndPins, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveFavoritesAndPins, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPropertyNameStyle, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "style", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPropertyNameStyle, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetShowCategories, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "showStandard", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "showCustom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Undo, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Redo, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasUndo, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasRedo, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetUndoRedoManager, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("RefCounted"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowPropertyContextMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "menuPos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PanelContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnPropertyContextMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenPropertyDocumentation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateNewResourceMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnNewResourceSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateSaveResourceMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnSaveResourceMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateResourceExtraMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnResourceExtraMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EditResourceFromClipboard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateHistoryMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnHistoryMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateObjectMenu, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnObjectMenuItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyObjectProperties, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PasteObjectProperties, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExpandAllProperties, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CollapseAllProperties, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ExpandNonDefaultProperties, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MakeSubResourcesUnique, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.Edit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.EditObject, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false)
			}, null),
			new MethodInfo(MethodName.CapitalizeName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.AddPlugin && args.Count == 1)
		{
			AddPlugin(VariantUtils.ConvertTo<XWInspectorPlugin>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemovePlugin && args.Count == 1)
		{
			RemovePlugin(VariantUtils.ConvertTo<XWInspectorPlugin>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspector>(Create());
			return true;
		}
		if (method == MethodName.CreateSubInspector && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspector>(CreateSubInspector());
			return true;
		}
		if (method == MethodName._EnterTree && args.Count == 0)
		{
			_EnterTree();
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
		if (method == MethodName.SetupToolbarIcons && args.Count == 0)
		{
			SetupToolbarIcons();
			ret = default;
			return true;
		}
		if (method == MethodName.SetButtonIcon && args.Count == 2)
		{
			SetButtonIcon(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMenuButtonIcon && args.Count == 2)
		{
			SetMenuButtonIcon(VariantUtils.ConvertTo<MenuButton>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectIdPressedOnce && args.Count == 2)
		{
			ConnectIdPressedOnce(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<Callable>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindObjectHeader && args.Count == 0)
		{
			BindObjectHeader();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateObjectHeader && args.Count == 0)
		{
			UpdateObjectHeader();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateObjectPreview && args.Count == 0)
		{
			UpdateObjectPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMultiNodeHeader && args.Count == 0)
		{
			UpdateMultiNodeHeader();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateToolbarState && args.Count == 0)
		{
			UpdateToolbarState();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessUpdates && args.Count == 0)
		{
			ProcessUpdates();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldRunUpdateTimer && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldRunUpdateTimer());
			return true;
		}
		if (method == MethodName.BindVisibilityHostWindow && args.Count == 0)
		{
			BindVisibilityHostWindow();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectVisibilityHostWindow && args.Count == 0)
		{
			DisconnectVisibilityHostWindow();
			ret = default;
			return true;
		}
		if (method == MethodName.OnVisibilityHostWindowChanged && args.Count == 0)
		{
			OnVisibilityHostWindowChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshUpdateTimerState && args.Count == 0)
		{
			RefreshUpdateTimerState();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetPendingInspectorUpdates && args.Count == 0)
		{
			ResetPendingInspectorUpdates();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearInvalidCurrentObject && args.Count == 0)
		{
			ClearInvalidCurrentObject();
			ret = default;
			return true;
		}
		if (method == MethodName.ProcessPendingPropertyUpdates && args.Count == 0)
		{
			ProcessPendingPropertyUpdates();
			ret = default;
			return true;
		}
		if (method == MethodName.PollPropertyChanges && args.Count == 0)
		{
			PollPropertyChanges();
			ret = default;
			return true;
		}
		if (method == MethodName.IsTrackedPropertySynchronized && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsTrackedPropertySynchronized(VariantUtils.ConvertTo<StringName>(in args[0])));
			return true;
		}
		if (method == MethodName._Notification && args.Count == 1)
		{
			_Notification(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PropertyPathMatches && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(PropertyPathMatches(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.IsSubsequence && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSubsequence(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ResourcePropertiesMatches && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ResourcePropertiesMatches(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.Search && args.Count == 1)
		{
			Search(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetObject && args.Count == 2)
		{
			SetObject(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldSkipSameObjectRefresh && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldSkipSameObjectRefresh(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1])));
			return true;
		}
		if (method == MethodName.IsSameInspectableObject && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSameInspectableObject(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.HasInspectorEditorFocus && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasInspectorEditorFocus());
			return true;
		}
		if (method == MethodName.IsControlInsideInspector && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsControlInsideInspector(VariantUtils.ConvertTo<Control>(in args[0])));
			return true;
		}
		if (method == MethodName.SetNodes && args.Count == 1)
		{
			SetNodes(VariantUtils.ConvertToSystemArrayOfGodotObject<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateBack && args.Count == 0)
		{
			NavigateBack();
			ret = default;
			return true;
		}
		if (method == MethodName.NavigateForward && args.Count == 0)
		{
			NavigateForward();
			ret = default;
			return true;
		}
		if (method == MethodName.CanNavigateBack && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanNavigateBack());
			return true;
		}
		if (method == MethodName.CanNavigateForward && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(CanNavigateForward());
			return true;
		}
		if (method == MethodName.GetSelectionHistory && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWEditorSelectionHistory>(GetSelectionHistory());
			return true;
		}
		if (method == MethodName.UpdateNavigationButtons && args.Count == 2)
		{
			UpdateNavigationButtons(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFoldState && args.Count == 0)
		{
			SaveFoldState();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreFoldState && args.Count == 0)
		{
			RestoreFoldState();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveScrollPosition && args.Count == 0)
		{
			SaveScrollPosition();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreScrollPosition && args.Count == 0)
		{
			RestoreScrollPosition();
			ret = default;
			return true;
		}
		if (method == MethodName.GetRootInspector && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspector>(GetRootInspector());
			return true;
		}
		if (method == MethodName.GetSubInspectorColorLevel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(GetSubInspectorColorLevel());
			return true;
		}
		if (method == MethodName.GetSubInspectorColor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Color>(GetSubInspectorColor());
			return true;
		}
		if (method == MethodName.ToggleFavorite && args.Count == 1)
		{
			ToggleFavorite(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsFavorite && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsFavorite(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.TogglePin && args.Count == 1)
		{
			TogglePin(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsPinned && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPinned(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.PinProperty && args.Count == 1)
		{
			PinProperty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnpinProperty && args.Count == 1)
		{
			UnpinProperty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsPropertyPinned && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPropertyPinned(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPinnedProperties && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetPinnedProperties());
			return true;
		}
		if (method == MethodName.FavoriteProperty && args.Count == 1)
		{
			FavoriteProperty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UnfavoriteProperty && args.Count == 1)
		{
			UnfavoriteProperty(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsPropertyFavorited && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPropertyFavorited(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFavoriteProperties && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string[]>(GetFavoriteProperties());
			return true;
		}
		if (method == MethodName.ShowMetadataEditor && args.Count == 1)
		{
			ShowMetadataEditor(VariantUtils.ConvertTo<GodotObject>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectEditor && args.Count == 1)
		{
			SelectEditor(VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectPropertyEditorSignals && args.Count == 1)
		{
			ConnectPropertyEditorSignals(VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPropertyEditorValueChanged && args.Count == 4)
		{
			OnPropertyEditorValueChanged(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.QueueInspectorRefreshAfterValueChange && args.Count == 2)
		{
			QueueInspectorRefreshAfterValueChange(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectObject && args.Count == 0)
		{
			ConnectObject();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectObject && args.Count == 0)
		{
			DisconnectObject();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPropertyListChanged && args.Count == 0)
		{
			OnPropertyListChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnPreviewReady && args.Count == 3)
		{
			OnPreviewReady(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1]), VariantUtils.ConvertTo<Texture2D>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpDateProperties && args.Count == 0)
		{
			UpDateProperties();
			ret = default;
			return true;
		}
		if (method == MethodName.IsCurrentObjectInternalResourceEditorTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCurrentObjectInternalResourceEditorTarget(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateInternalResourceProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWInspectorProperty>(CreateInternalResourceProperty(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.UpDateMultiNodeProperties && args.Count == 0)
		{
			UpDateMultiNodeProperties();
			ret = default;
			return true;
		}
		if (method == MethodName.ShouldShowInspectorProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(ShouldShowInspectorProperty(VariantUtils.ConvertTo<XWInspectorProperty>(in args[0])));
			return true;
		}
		if (method == MethodName.Clear && args.Count == 0)
		{
			Clear();
			ret = default;
			return true;
		}
		if (method == MethodName.GetPropertyKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPropertyKey(VariantUtils.ConvertTo<XWInspectorProperty>(in args[0])));
			return true;
		}
		if (method == MethodName.ReversePropertyListByCategory && args.Count == 1)
		{
			Array<Dictionary> array = ReversePropertyListByCategory(VariantUtils.ConvertToArray<Dictionary>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CreateFavoritesSection && args.Count == 0)
		{
			CreateFavoritesSection();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadFavoritesAndPins && args.Count == 0)
		{
			LoadFavoritesAndPins();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFavoritesAndPins && args.Count == 0)
		{
			SaveFavoritesAndPins();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPropertyNameStyle && args.Count == 1)
		{
			SetPropertyNameStyle(VariantUtils.ConvertTo<PropertyNameStyle>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPropertyNameStyle && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PropertyNameStyle>(GetPropertyNameStyle());
			return true;
		}
		if (method == MethodName.SetShowCategories && args.Count == 2)
		{
			SetShowCategories(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.Undo && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Undo());
			return true;
		}
		if (method == MethodName.Redo && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(Redo());
			return true;
		}
		if (method == MethodName.HasUndo && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasUndo());
			return true;
		}
		if (method == MethodName.HasRedo && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasRedo());
			return true;
		}
		if (method == MethodName.GetUndoRedoManager && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWUndoRedoManager>(GetUndoRedoManager());
			return true;
		}
		if (method == MethodName.ShowPropertyContextMenu && args.Count == 3)
		{
			ShowPropertyContextMenu(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPropertyContextMenuItem && args.Count == 1)
		{
			OnPropertyContextMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenPropertyDocumentation && args.Count == 0)
		{
			OpenPropertyDocumentation();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateNewResourceMenu && args.Count == 0)
		{
			PopulateNewResourceMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.OnNewResourceSelected && args.Count == 1)
		{
			OnNewResourceSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateSaveResourceMenu && args.Count == 0)
		{
			PopulateSaveResourceMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.OnSaveResourceMenuItem && args.Count == 1)
		{
			OnSaveResourceMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateResourceExtraMenu && args.Count == 0)
		{
			PopulateResourceExtraMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.OnResourceExtraMenuItem && args.Count == 1)
		{
			OnResourceExtraMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EditResourceFromClipboard && args.Count == 0)
		{
			EditResourceFromClipboard();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateHistoryMenu && args.Count == 0)
		{
			PopulateHistoryMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.OnHistoryMenuItem && args.Count == 1)
		{
			OnHistoryMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateObjectMenu && args.Count == 0)
		{
			PopulateObjectMenu();
			ret = default;
			return true;
		}
		if (method == MethodName.OnObjectMenuItem && args.Count == 1)
		{
			OnObjectMenuItem(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CopyObjectProperties && args.Count == 0)
		{
			CopyObjectProperties();
			ret = default;
			return true;
		}
		if (method == MethodName.PasteObjectProperties && args.Count == 0)
		{
			PasteObjectProperties();
			ret = default;
			return true;
		}
		if (method == MethodName.ExpandAllProperties && args.Count == 0)
		{
			ExpandAllProperties();
			ret = default;
			return true;
		}
		if (method == MethodName.CollapseAllProperties && args.Count == 0)
		{
			CollapseAllProperties();
			ret = default;
			return true;
		}
		if (method == MethodName.ExpandNonDefaultProperties && args.Count == 0)
		{
			ExpandNonDefaultProperties();
			ret = default;
			return true;
		}
		if (method == MethodName.MakeSubResourcesUnique && args.Count == 0)
		{
			MakeSubResourcesUnique();
			ret = default;
			return true;
		}
		if (method == MethodName.Edit && args.Count == 1)
		{
			Edit(VariantUtils.ConvertTo<GodotObject>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EditObject && args.Count == 1)
		{
			EditObject(VariantUtils.ConvertTo<GodotObject>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CapitalizeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CapitalizeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspector>(Create());
			return true;
		}
		if (method == MethodName.CreateSubInspector && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWInspector>(CreateSubInspector());
			return true;
		}
		if (method == MethodName.SetButtonIcon && args.Count == 2)
		{
			SetButtonIcon(VariantUtils.ConvertTo<Button>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMenuButtonIcon && args.Count == 2)
		{
			SetMenuButtonIcon(VariantUtils.ConvertTo<MenuButton>(in args[0]), VariantUtils.ConvertTo<Texture2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectIdPressedOnce && args.Count == 2)
		{
			ConnectIdPressedOnce(VariantUtils.ConvertTo<PopupMenu>(in args[0]), VariantUtils.ConvertTo<Callable>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.IsSubsequence && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(IsSubsequence(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.IsCurrentObjectInternalResourceEditorTarget && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCurrentObjectInternalResourceEditorTarget(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateInternalResourceProperty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<XWInspectorProperty>(CreateInternalResourceProperty(VariantUtils.ConvertTo<GodotObject>(in args[0])));
			return true;
		}
		if (method == MethodName.CapitalizeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CapitalizeName(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.AddPlugin)
		{
			return true;
		}
		if (method == MethodName.RemovePlugin)
		{
			return true;
		}
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName.CreateSubInspector)
		{
			return true;
		}
		if (method == MethodName._EnterTree)
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
		if (method == MethodName.SetupToolbarIcons)
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
		if (method == MethodName.ConnectIdPressedOnce)
		{
			return true;
		}
		if (method == MethodName.BindObjectHeader)
		{
			return true;
		}
		if (method == MethodName.UpdateObjectHeader)
		{
			return true;
		}
		if (method == MethodName.UpdateObjectPreview)
		{
			return true;
		}
		if (method == MethodName.UpdateMultiNodeHeader)
		{
			return true;
		}
		if (method == MethodName.UpdateToolbarState)
		{
			return true;
		}
		if (method == MethodName.ProcessUpdates)
		{
			return true;
		}
		if (method == MethodName.ShouldRunUpdateTimer)
		{
			return true;
		}
		if (method == MethodName.BindVisibilityHostWindow)
		{
			return true;
		}
		if (method == MethodName.DisconnectVisibilityHostWindow)
		{
			return true;
		}
		if (method == MethodName.OnVisibilityHostWindowChanged)
		{
			return true;
		}
		if (method == MethodName.RefreshUpdateTimerState)
		{
			return true;
		}
		if (method == MethodName.ResetPendingInspectorUpdates)
		{
			return true;
		}
		if (method == MethodName.ClearInvalidCurrentObject)
		{
			return true;
		}
		if (method == MethodName.ProcessPendingPropertyUpdates)
		{
			return true;
		}
		if (method == MethodName.PollPropertyChanges)
		{
			return true;
		}
		if (method == MethodName.IsTrackedPropertySynchronized)
		{
			return true;
		}
		if (method == MethodName._Notification)
		{
			return true;
		}
		if (method == MethodName.PropertyPathMatches)
		{
			return true;
		}
		if (method == MethodName.IsSubsequence)
		{
			return true;
		}
		if (method == MethodName.ResourcePropertiesMatches)
		{
			return true;
		}
		if (method == MethodName.Search)
		{
			return true;
		}
		if (method == MethodName.SetObject)
		{
			return true;
		}
		if (method == MethodName.ShouldSkipSameObjectRefresh)
		{
			return true;
		}
		if (method == MethodName.IsSameInspectableObject)
		{
			return true;
		}
		if (method == MethodName.HasInspectorEditorFocus)
		{
			return true;
		}
		if (method == MethodName.IsControlInsideInspector)
		{
			return true;
		}
		if (method == MethodName.SetNodes)
		{
			return true;
		}
		if (method == MethodName.NavigateBack)
		{
			return true;
		}
		if (method == MethodName.NavigateForward)
		{
			return true;
		}
		if (method == MethodName.CanNavigateBack)
		{
			return true;
		}
		if (method == MethodName.CanNavigateForward)
		{
			return true;
		}
		if (method == MethodName.GetSelectionHistory)
		{
			return true;
		}
		if (method == MethodName.UpdateNavigationButtons)
		{
			return true;
		}
		if (method == MethodName.SaveFoldState)
		{
			return true;
		}
		if (method == MethodName.RestoreFoldState)
		{
			return true;
		}
		if (method == MethodName.SaveScrollPosition)
		{
			return true;
		}
		if (method == MethodName.RestoreScrollPosition)
		{
			return true;
		}
		if (method == MethodName.GetRootInspector)
		{
			return true;
		}
		if (method == MethodName.GetSubInspectorColorLevel)
		{
			return true;
		}
		if (method == MethodName.GetSubInspectorColor)
		{
			return true;
		}
		if (method == MethodName.ToggleFavorite)
		{
			return true;
		}
		if (method == MethodName.IsFavorite)
		{
			return true;
		}
		if (method == MethodName.TogglePin)
		{
			return true;
		}
		if (method == MethodName.IsPinned)
		{
			return true;
		}
		if (method == MethodName.PinProperty)
		{
			return true;
		}
		if (method == MethodName.UnpinProperty)
		{
			return true;
		}
		if (method == MethodName.IsPropertyPinned)
		{
			return true;
		}
		if (method == MethodName.GetPinnedProperties)
		{
			return true;
		}
		if (method == MethodName.FavoriteProperty)
		{
			return true;
		}
		if (method == MethodName.UnfavoriteProperty)
		{
			return true;
		}
		if (method == MethodName.IsPropertyFavorited)
		{
			return true;
		}
		if (method == MethodName.GetFavoriteProperties)
		{
			return true;
		}
		if (method == MethodName.ShowMetadataEditor)
		{
			return true;
		}
		if (method == MethodName.SelectEditor)
		{
			return true;
		}
		if (method == MethodName.ConnectPropertyEditorSignals)
		{
			return true;
		}
		if (method == MethodName.OnPropertyEditorValueChanged)
		{
			return true;
		}
		if (method == MethodName.QueueInspectorRefreshAfterValueChange)
		{
			return true;
		}
		if (method == MethodName.ConnectObject)
		{
			return true;
		}
		if (method == MethodName.DisconnectObject)
		{
			return true;
		}
		if (method == MethodName.OnPropertyListChanged)
		{
			return true;
		}
		if (method == MethodName.OnPreviewReady)
		{
			return true;
		}
		if (method == MethodName.UpDateProperties)
		{
			return true;
		}
		if (method == MethodName.IsCurrentObjectInternalResourceEditorTarget)
		{
			return true;
		}
		if (method == MethodName.CreateInternalResourceProperty)
		{
			return true;
		}
		if (method == MethodName.UpDateMultiNodeProperties)
		{
			return true;
		}
		if (method == MethodName.ShouldShowInspectorProperty)
		{
			return true;
		}
		if (method == MethodName.Clear)
		{
			return true;
		}
		if (method == MethodName.GetPropertyKey)
		{
			return true;
		}
		if (method == MethodName.ReversePropertyListByCategory)
		{
			return true;
		}
		if (method == MethodName.CreateFavoritesSection)
		{
			return true;
		}
		if (method == MethodName.LoadFavoritesAndPins)
		{
			return true;
		}
		if (method == MethodName.SaveFavoritesAndPins)
		{
			return true;
		}
		if (method == MethodName.SetPropertyNameStyle)
		{
			return true;
		}
		if (method == MethodName.GetPropertyNameStyle)
		{
			return true;
		}
		if (method == MethodName.SetShowCategories)
		{
			return true;
		}
		if (method == MethodName.Undo)
		{
			return true;
		}
		if (method == MethodName.Redo)
		{
			return true;
		}
		if (method == MethodName.HasUndo)
		{
			return true;
		}
		if (method == MethodName.HasRedo)
		{
			return true;
		}
		if (method == MethodName.GetUndoRedoManager)
		{
			return true;
		}
		if (method == MethodName.ShowPropertyContextMenu)
		{
			return true;
		}
		if (method == MethodName.OnPropertyContextMenuItem)
		{
			return true;
		}
		if (method == MethodName.OpenPropertyDocumentation)
		{
			return true;
		}
		if (method == MethodName.PopulateNewResourceMenu)
		{
			return true;
		}
		if (method == MethodName.OnNewResourceSelected)
		{
			return true;
		}
		if (method == MethodName.PopulateSaveResourceMenu)
		{
			return true;
		}
		if (method == MethodName.OnSaveResourceMenuItem)
		{
			return true;
		}
		if (method == MethodName.PopulateResourceExtraMenu)
		{
			return true;
		}
		if (method == MethodName.OnResourceExtraMenuItem)
		{
			return true;
		}
		if (method == MethodName.EditResourceFromClipboard)
		{
			return true;
		}
		if (method == MethodName.PopulateHistoryMenu)
		{
			return true;
		}
		if (method == MethodName.OnHistoryMenuItem)
		{
			return true;
		}
		if (method == MethodName.PopulateObjectMenu)
		{
			return true;
		}
		if (method == MethodName.OnObjectMenuItem)
		{
			return true;
		}
		if (method == MethodName.CopyObjectProperties)
		{
			return true;
		}
		if (method == MethodName.PasteObjectProperties)
		{
			return true;
		}
		if (method == MethodName.ExpandAllProperties)
		{
			return true;
		}
		if (method == MethodName.CollapseAllProperties)
		{
			return true;
		}
		if (method == MethodName.ExpandNonDefaultProperties)
		{
			return true;
		}
		if (method == MethodName.MakeSubResourcesUnique)
		{
			return true;
		}
		if (method == MethodName.Edit)
		{
			return true;
		}
		if (method == MethodName.EditObject)
		{
			return true;
		}
		if (method == MethodName.CapitalizeName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.PropertyContainer)
		{
			PropertyContainer = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName.CurrentObject)
		{
			CurrentObject = VariantUtils.ConvertTo<GodotObject>(in value);
			return true;
		}
		if (name == PropertyName.MultiNodeEdit)
		{
			MultiNodeEdit = VariantUtils.ConvertTo<XWMultiNodeEdit>(in value);
			return true;
		}
		if (name == PropertyName.UndoRedoManager)
		{
			UndoRedoManager = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName.AnimationPreviewEnabled)
		{
			AnimationPreviewEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.RootInspector)
		{
			RootInspector = VariantUtils.ConvertTo<XWInspector>(in value);
			return true;
		}
		if (name == PropertyName.IsSubInspector)
		{
			IsSubInspector = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.PropertyNameStyleMode)
		{
			PropertyNameStyleMode = VariantUtils.ConvertTo<PropertyNameStyle>(in value);
			return true;
		}
		if (name == PropertyName.ShowStandardCategories)
		{
			ShowStandardCategories = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.ShowCustomCategories)
		{
			ShowCustomCategories = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._searchLineEdit)
		{
			_searchLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._toolbar)
		{
			_toolbar = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._backButton)
		{
			_backButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._forwardButton)
		{
			_forwardButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._newResourceButton)
		{
			_newResourceButton = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._loadResourceButton)
		{
			_loadResourceButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._saveResourceButton)
		{
			_saveResourceButton = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._objectMenuButton)
		{
			_objectMenuButton = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._subResourceBar)
		{
			_subResourceBar = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._propertyScrollContainer)
		{
			_propertyScrollContainer = VariantUtils.ConvertTo<ScrollContainer>(in value);
			return true;
		}
		if (name == PropertyName._objectIcon)
		{
			_objectIcon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._objectLabel)
		{
			_objectLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._openDocsButton)
		{
			_openDocsButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._resourceExtraButton)
		{
			_resourceExtraButton = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._historyButton)
		{
			_historyButton = VariantUtils.ConvertTo<MenuButton>(in value);
			return true;
		}
		if (name == PropertyName._connectedObject)
		{
			_connectedObject = VariantUtils.ConvertTo<GodotObject>(in value);
			return true;
		}
		if (name == PropertyName._updateTimer)
		{
			_updateTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._visibilityHostWindow)
		{
			_visibilityHostWindow = VariantUtils.ConvertTo<Window>(in value);
			return true;
		}
		if (name == PropertyName._updateTreePending)
		{
			_updateTreePending = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._changing)
		{
			_changing = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._updateTimerCallbackCount)
		{
			_updateTimerCallbackCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._propertyPollPassCount)
		{
			_propertyPollPassCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._propertyPollReadCount)
		{
			_propertyPollReadCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._objectHeader)
		{
			_objectHeader = VariantUtils.ConvertTo<HBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._headerIcon)
		{
			_headerIcon = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._headerLabel)
		{
			_headerLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._headerClassLabel)
		{
			_headerClassLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._headerIdLabel)
		{
			_headerIdLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._animationPreview)
		{
			_animationPreview = VariantUtils.ConvertTo<AdobeAnimateInspectorPreview>(in value);
			return true;
		}
		if (name == PropertyName._animationPreviewEnabled)
		{
			_animationPreviewEnabled = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._metadataEditDialog)
		{
			_metadataEditDialog = VariantUtils.ConvertTo<ConfirmationDialog>(in value);
			return true;
		}
		if (name == PropertyName._selectionHistory)
		{
			_selectionHistory = VariantUtils.ConvertTo<XWEditorSelectionHistory>(in value);
			return true;
		}
		if (name == PropertyName._selectedEditor)
		{
			_selectedEditor = VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in value);
			return true;
		}
		if (name == PropertyName._resourcePreview)
		{
			_resourcePreview = VariantUtils.ConvertTo<XWResourcePreview>(in value);
			return true;
		}
		if (name == PropertyName._propertyContextMenu)
		{
			_propertyContextMenu = VariantUtils.ConvertTo<PopupMenu>(in value);
			return true;
		}
		if (name == PropertyName._newResourceSelectedCallable)
		{
			_newResourceSelectedCallable = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName._saveResourceMenuCallable)
		{
			_saveResourceMenuCallable = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName._resourceExtraMenuCallable)
		{
			_resourceExtraMenuCallable = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName._historyMenuCallable)
		{
			_historyMenuCallable = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName._objectMenuCallable)
		{
			_objectMenuCallable = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName._propertyContextMenuCallable)
		{
			_propertyContextMenuCallable = VariantUtils.ConvertTo<Callable>(in value);
			return true;
		}
		if (name == PropertyName._contextMenuPropertyName)
		{
			_contextMenuPropertyName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._contextMenuEditor)
		{
			_contextMenuEditor = VariantUtils.ConvertTo<XWInspectorPropertyEditorBase>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.PropertyContainer)
		{
			value = VariantUtils.CreateFrom<VBoxContainer>(PropertyContainer);
			return true;
		}
		if (name == PropertyName.CurrentObject)
		{
			value = VariantUtils.CreateFrom<GodotObject>(CurrentObject);
			return true;
		}
		if (name == PropertyName.MultiNodeEdit)
		{
			value = VariantUtils.CreateFrom<XWMultiNodeEdit>(MultiNodeEdit);
			return true;
		}
		if (name == PropertyName.UndoRedoManager)
		{
			value = VariantUtils.CreateFrom<XWUndoRedoManager>(UndoRedoManager);
			return true;
		}
		int from;
		if (name == PropertyName.UpdateTimerCallbackCount)
		{
			from = UpdateTimerCallbackCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PropertyPollPassCount)
		{
			from = PropertyPollPassCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PropertyPollReadCount)
		{
			from = PropertyPollReadCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.PropertyEditorCount)
		{
			from = PropertyEditorCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.IsUpdateTimerRunning)
		{
			from2 = IsUpdateTimerRunning;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.AnimationPreviewEnabled)
		{
			from2 = AnimationPreviewEnabled;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.RootInspector)
		{
			value = VariantUtils.CreateFrom<XWInspector>(RootInspector);
			return true;
		}
		if (name == PropertyName.IsSubInspector)
		{
			from2 = IsSubInspector;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PropertyNameStyleMode)
		{
			value = VariantUtils.CreateFrom<PropertyNameStyle>(PropertyNameStyleMode);
			return true;
		}
		if (name == PropertyName.ShowStandardCategories)
		{
			from2 = ShowStandardCategories;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ShowCustomCategories)
		{
			from2 = ShowCustomCategories;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._searchLineEdit)
		{
			value = VariantUtils.CreateFrom(in _searchLineEdit);
			return true;
		}
		if (name == PropertyName._toolbar)
		{
			value = VariantUtils.CreateFrom(in _toolbar);
			return true;
		}
		if (name == PropertyName._backButton)
		{
			value = VariantUtils.CreateFrom(in _backButton);
			return true;
		}
		if (name == PropertyName._forwardButton)
		{
			value = VariantUtils.CreateFrom(in _forwardButton);
			return true;
		}
		if (name == PropertyName._newResourceButton)
		{
			value = VariantUtils.CreateFrom(in _newResourceButton);
			return true;
		}
		if (name == PropertyName._loadResourceButton)
		{
			value = VariantUtils.CreateFrom(in _loadResourceButton);
			return true;
		}
		if (name == PropertyName._saveResourceButton)
		{
			value = VariantUtils.CreateFrom(in _saveResourceButton);
			return true;
		}
		if (name == PropertyName._objectMenuButton)
		{
			value = VariantUtils.CreateFrom(in _objectMenuButton);
			return true;
		}
		if (name == PropertyName._subResourceBar)
		{
			value = VariantUtils.CreateFrom(in _subResourceBar);
			return true;
		}
		if (name == PropertyName._propertyScrollContainer)
		{
			value = VariantUtils.CreateFrom(in _propertyScrollContainer);
			return true;
		}
		if (name == PropertyName._objectIcon)
		{
			value = VariantUtils.CreateFrom(in _objectIcon);
			return true;
		}
		if (name == PropertyName._objectLabel)
		{
			value = VariantUtils.CreateFrom(in _objectLabel);
			return true;
		}
		if (name == PropertyName._openDocsButton)
		{
			value = VariantUtils.CreateFrom(in _openDocsButton);
			return true;
		}
		if (name == PropertyName._resourceExtraButton)
		{
			value = VariantUtils.CreateFrom(in _resourceExtraButton);
			return true;
		}
		if (name == PropertyName._historyButton)
		{
			value = VariantUtils.CreateFrom(in _historyButton);
			return true;
		}
		if (name == PropertyName._connectedObject)
		{
			value = VariantUtils.CreateFrom(in _connectedObject);
			return true;
		}
		if (name == PropertyName._updateTimer)
		{
			value = VariantUtils.CreateFrom(in _updateTimer);
			return true;
		}
		if (name == PropertyName._visibilityHostWindow)
		{
			value = VariantUtils.CreateFrom(in _visibilityHostWindow);
			return true;
		}
		if (name == PropertyName._pendingUpdates)
		{
			value = VariantUtils.CreateFrom(in _pendingUpdates);
			return true;
		}
		if (name == PropertyName._updateTreePending)
		{
			value = VariantUtils.CreateFrom(in _updateTreePending);
			return true;
		}
		if (name == PropertyName._changing)
		{
			value = VariantUtils.CreateFrom(in _changing);
			return true;
		}
		if (name == PropertyName._updateTimerCallbackCount)
		{
			value = VariantUtils.CreateFrom(in _updateTimerCallbackCount);
			return true;
		}
		if (name == PropertyName._propertyPollPassCount)
		{
			value = VariantUtils.CreateFrom(in _propertyPollPassCount);
			return true;
		}
		if (name == PropertyName._propertyPollReadCount)
		{
			value = VariantUtils.CreateFrom(in _propertyPollReadCount);
			return true;
		}
		if (name == PropertyName._objectHeader)
		{
			value = VariantUtils.CreateFrom(in _objectHeader);
			return true;
		}
		if (name == PropertyName._headerIcon)
		{
			value = VariantUtils.CreateFrom(in _headerIcon);
			return true;
		}
		if (name == PropertyName._headerLabel)
		{
			value = VariantUtils.CreateFrom(in _headerLabel);
			return true;
		}
		if (name == PropertyName._headerClassLabel)
		{
			value = VariantUtils.CreateFrom(in _headerClassLabel);
			return true;
		}
		if (name == PropertyName._headerIdLabel)
		{
			value = VariantUtils.CreateFrom(in _headerIdLabel);
			return true;
		}
		if (name == PropertyName._animationPreview)
		{
			value = VariantUtils.CreateFrom(in _animationPreview);
			return true;
		}
		if (name == PropertyName._animationPreviewEnabled)
		{
			value = VariantUtils.CreateFrom(in _animationPreviewEnabled);
			return true;
		}
		if (name == PropertyName._favorites)
		{
			value = VariantUtils.CreateFrom(in _favorites);
			return true;
		}
		if (name == PropertyName._pinnedProperties)
		{
			value = VariantUtils.CreateFrom(in _pinnedProperties);
			return true;
		}
		if (name == PropertyName._favoriteProperties)
		{
			value = VariantUtils.CreateFrom(in _favoriteProperties);
			return true;
		}
		if (name == PropertyName._pinned_properties)
		{
			value = VariantUtils.CreateFrom(in _pinned_properties);
			return true;
		}
		if (name == PropertyName._metadataEditDialog)
		{
			value = VariantUtils.CreateFrom(in _metadataEditDialog);
			return true;
		}
		if (name == PropertyName._foldStateCache)
		{
			value = VariantUtils.CreateFrom(in _foldStateCache);
			return true;
		}
		if (name == PropertyName._scrollCache)
		{
			value = VariantUtils.CreateFrom(in _scrollCache);
			return true;
		}
		if (name == PropertyName._selectionHistory)
		{
			value = VariantUtils.CreateFrom(in _selectionHistory);
			return true;
		}
		if (name == PropertyName._selectedEditor)
		{
			value = VariantUtils.CreateFrom(in _selectedEditor);
			return true;
		}
		if (name == PropertyName._resourcePreview)
		{
			value = VariantUtils.CreateFrom(in _resourcePreview);
			return true;
		}
		if (name == PropertyName._propertyContextMenu)
		{
			value = VariantUtils.CreateFrom(in _propertyContextMenu);
			return true;
		}
		if (name == PropertyName._newResourceSelectedCallable)
		{
			value = VariantUtils.CreateFrom(in _newResourceSelectedCallable);
			return true;
		}
		if (name == PropertyName._saveResourceMenuCallable)
		{
			value = VariantUtils.CreateFrom(in _saveResourceMenuCallable);
			return true;
		}
		if (name == PropertyName._resourceExtraMenuCallable)
		{
			value = VariantUtils.CreateFrom(in _resourceExtraMenuCallable);
			return true;
		}
		if (name == PropertyName._historyMenuCallable)
		{
			value = VariantUtils.CreateFrom(in _historyMenuCallable);
			return true;
		}
		if (name == PropertyName._objectMenuCallable)
		{
			value = VariantUtils.CreateFrom(in _objectMenuCallable);
			return true;
		}
		if (name == PropertyName._propertyContextMenuCallable)
		{
			value = VariantUtils.CreateFrom(in _propertyContextMenuCallable);
			return true;
		}
		if (name == PropertyName._contextMenuPropertyName)
		{
			value = VariantUtils.CreateFrom(in _contextMenuPropertyName);
			return true;
		}
		if (name == PropertyName._contextMenuEditor)
		{
			value = VariantUtils.CreateFrom(in _contextMenuEditor);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName.PropertyContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._toolbar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._backButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._forwardButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._newResourceButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._loadResourceButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveResourceButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._objectMenuButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._subResourceBar, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._propertyScrollContainer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._objectIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._objectLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openDocsButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceExtraButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._historyButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.CurrentObject, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.MultiNodeEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.UndoRedoManager, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._connectedObject, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._updateTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._visibilityHostWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._pendingUpdates, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updateTreePending, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._changing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._updateTimerCallbackCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._propertyPollPassCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._propertyPollReadCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.UpdateTimerCallbackCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PropertyPollPassCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PropertyPollReadCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PropertyEditorCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsUpdateTimerRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._objectHeader, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._headerIcon, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._headerLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._headerClassLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._headerIdLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animationPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._animationPreviewEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.AnimationPreviewEnabled, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.RootInspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsSubInspector, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._favorites, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._pinnedProperties, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._favoriteProperties, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._pinned_properties, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._metadataEditDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._foldStateCache, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Dictionary, PropertyName._scrollCache, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectionHistory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectedEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourcePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._propertyContextMenu, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName._newResourceSelectedCallable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName._saveResourceMenuCallable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName._resourceExtraMenuCallable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName._historyMenuCallable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName._objectMenuCallable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Callable, PropertyName._propertyContextMenuCallable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._contextMenuPropertyName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._contextMenuEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PropertyNameStyleMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ShowStandardCategories, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.ShowCustomCategories, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.PropertyContainer, Variant.From<VBoxContainer>(PropertyContainer));
		info.AddProperty(PropertyName.CurrentObject, Variant.From<GodotObject>(CurrentObject));
		info.AddProperty(PropertyName.MultiNodeEdit, Variant.From<XWMultiNodeEdit>(MultiNodeEdit));
		info.AddProperty(PropertyName.UndoRedoManager, Variant.From<XWUndoRedoManager>(UndoRedoManager));
		info.AddProperty(PropertyName.AnimationPreviewEnabled, Variant.From<bool>(AnimationPreviewEnabled));
		info.AddProperty(PropertyName.RootInspector, Variant.From<XWInspector>(RootInspector));
		info.AddProperty(PropertyName.IsSubInspector, Variant.From<bool>(IsSubInspector));
		info.AddProperty(PropertyName.PropertyNameStyleMode, Variant.From<PropertyNameStyle>(PropertyNameStyleMode));
		info.AddProperty(PropertyName.ShowStandardCategories, Variant.From<bool>(ShowStandardCategories));
		info.AddProperty(PropertyName.ShowCustomCategories, Variant.From<bool>(ShowCustomCategories));
		info.AddProperty(PropertyName._searchLineEdit, Variant.From(in _searchLineEdit));
		info.AddProperty(PropertyName._toolbar, Variant.From(in _toolbar));
		info.AddProperty(PropertyName._backButton, Variant.From(in _backButton));
		info.AddProperty(PropertyName._forwardButton, Variant.From(in _forwardButton));
		info.AddProperty(PropertyName._newResourceButton, Variant.From(in _newResourceButton));
		info.AddProperty(PropertyName._loadResourceButton, Variant.From(in _loadResourceButton));
		info.AddProperty(PropertyName._saveResourceButton, Variant.From(in _saveResourceButton));
		info.AddProperty(PropertyName._objectMenuButton, Variant.From(in _objectMenuButton));
		info.AddProperty(PropertyName._subResourceBar, Variant.From(in _subResourceBar));
		info.AddProperty(PropertyName._propertyScrollContainer, Variant.From(in _propertyScrollContainer));
		info.AddProperty(PropertyName._objectIcon, Variant.From(in _objectIcon));
		info.AddProperty(PropertyName._objectLabel, Variant.From(in _objectLabel));
		info.AddProperty(PropertyName._openDocsButton, Variant.From(in _openDocsButton));
		info.AddProperty(PropertyName._resourceExtraButton, Variant.From(in _resourceExtraButton));
		info.AddProperty(PropertyName._historyButton, Variant.From(in _historyButton));
		info.AddProperty(PropertyName._connectedObject, Variant.From(in _connectedObject));
		info.AddProperty(PropertyName._updateTimer, Variant.From(in _updateTimer));
		info.AddProperty(PropertyName._visibilityHostWindow, Variant.From(in _visibilityHostWindow));
		info.AddProperty(PropertyName._updateTreePending, Variant.From(in _updateTreePending));
		info.AddProperty(PropertyName._changing, Variant.From(in _changing));
		info.AddProperty(PropertyName._updateTimerCallbackCount, Variant.From(in _updateTimerCallbackCount));
		info.AddProperty(PropertyName._propertyPollPassCount, Variant.From(in _propertyPollPassCount));
		info.AddProperty(PropertyName._propertyPollReadCount, Variant.From(in _propertyPollReadCount));
		info.AddProperty(PropertyName._objectHeader, Variant.From(in _objectHeader));
		info.AddProperty(PropertyName._headerIcon, Variant.From(in _headerIcon));
		info.AddProperty(PropertyName._headerLabel, Variant.From(in _headerLabel));
		info.AddProperty(PropertyName._headerClassLabel, Variant.From(in _headerClassLabel));
		info.AddProperty(PropertyName._headerIdLabel, Variant.From(in _headerIdLabel));
		info.AddProperty(PropertyName._animationPreview, Variant.From(in _animationPreview));
		info.AddProperty(PropertyName._animationPreviewEnabled, Variant.From(in _animationPreviewEnabled));
		info.AddProperty(PropertyName._metadataEditDialog, Variant.From(in _metadataEditDialog));
		info.AddProperty(PropertyName._selectionHistory, Variant.From(in _selectionHistory));
		info.AddProperty(PropertyName._selectedEditor, Variant.From(in _selectedEditor));
		info.AddProperty(PropertyName._resourcePreview, Variant.From(in _resourcePreview));
		info.AddProperty(PropertyName._propertyContextMenu, Variant.From(in _propertyContextMenu));
		info.AddProperty(PropertyName._newResourceSelectedCallable, Variant.From(in _newResourceSelectedCallable));
		info.AddProperty(PropertyName._saveResourceMenuCallable, Variant.From(in _saveResourceMenuCallable));
		info.AddProperty(PropertyName._resourceExtraMenuCallable, Variant.From(in _resourceExtraMenuCallable));
		info.AddProperty(PropertyName._historyMenuCallable, Variant.From(in _historyMenuCallable));
		info.AddProperty(PropertyName._objectMenuCallable, Variant.From(in _objectMenuCallable));
		info.AddProperty(PropertyName._propertyContextMenuCallable, Variant.From(in _propertyContextMenuCallable));
		info.AddProperty(PropertyName._contextMenuPropertyName, Variant.From(in _contextMenuPropertyName));
		info.AddProperty(PropertyName._contextMenuEditor, Variant.From(in _contextMenuEditor));
		info.AddSignalEventDelegate(SignalName.ObjectChanged, backing_ObjectChanged);
		info.AddSignalEventDelegate(SignalName.ObjectPropertyChanged, backing_ObjectPropertyChanged);
		info.AddSignalEventDelegate(SignalName.PropertyKeyed, backing_PropertyKeyed);
		info.AddSignalEventDelegate(SignalName.ResourceSelected, backing_ResourceSelected);
		info.AddSignalEventDelegate(SignalName.NavigatedBack, backing_NavigatedBack);
		info.AddSignalEventDelegate(SignalName.NavigatedForward, backing_NavigatedForward);
		info.AddSignalEventDelegate(SignalName.ResourceNewRequested, backing_ResourceNewRequested);
		info.AddSignalEventDelegate(SignalName.ResourceLoadRequested, backing_ResourceLoadRequested);
		info.AddSignalEventDelegate(SignalName.ResourceSaveRequested, backing_ResourceSaveRequested);
		info.AddSignalEventDelegate(SignalName.ResourceSaveAsRequested, backing_ResourceSaveAsRequested);
		info.AddSignalEventDelegate(SignalName.ResourceCopyRequested, backing_ResourceCopyRequested);
		info.AddSignalEventDelegate(SignalName.ResourceShowInFilesystem, backing_ResourceShowInFilesystem);
		info.AddSignalEventDelegate(SignalName.ResourceMakeBuiltIn, backing_ResourceMakeBuiltIn);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.PropertyContainer, out var value))
		{
			PropertyContainer = value.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName.CurrentObject, out var value2))
		{
			CurrentObject = value2.As<GodotObject>();
		}
		if (info.TryGetProperty(PropertyName.MultiNodeEdit, out var value3))
		{
			MultiNodeEdit = value3.As<XWMultiNodeEdit>();
		}
		if (info.TryGetProperty(PropertyName.UndoRedoManager, out var value4))
		{
			UndoRedoManager = value4.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName.AnimationPreviewEnabled, out var value5))
		{
			AnimationPreviewEnabled = value5.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.RootInspector, out var value6))
		{
			RootInspector = value6.As<XWInspector>();
		}
		if (info.TryGetProperty(PropertyName.IsSubInspector, out var value7))
		{
			IsSubInspector = value7.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.PropertyNameStyleMode, out var value8))
		{
			PropertyNameStyleMode = value8.As<PropertyNameStyle>();
		}
		if (info.TryGetProperty(PropertyName.ShowStandardCategories, out var value9))
		{
			ShowStandardCategories = value9.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.ShowCustomCategories, out var value10))
		{
			ShowCustomCategories = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._searchLineEdit, out var value11))
		{
			_searchLineEdit = value11.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._toolbar, out var value12))
		{
			_toolbar = value12.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._backButton, out var value13))
		{
			_backButton = value13.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._forwardButton, out var value14))
		{
			_forwardButton = value14.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._newResourceButton, out var value15))
		{
			_newResourceButton = value15.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._loadResourceButton, out var value16))
		{
			_loadResourceButton = value16.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._saveResourceButton, out var value17))
		{
			_saveResourceButton = value17.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._objectMenuButton, out var value18))
		{
			_objectMenuButton = value18.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._subResourceBar, out var value19))
		{
			_subResourceBar = value19.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._propertyScrollContainer, out var value20))
		{
			_propertyScrollContainer = value20.As<ScrollContainer>();
		}
		if (info.TryGetProperty(PropertyName._objectIcon, out var value21))
		{
			_objectIcon = value21.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._objectLabel, out var value22))
		{
			_objectLabel = value22.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._openDocsButton, out var value23))
		{
			_openDocsButton = value23.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._resourceExtraButton, out var value24))
		{
			_resourceExtraButton = value24.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._historyButton, out var value25))
		{
			_historyButton = value25.As<MenuButton>();
		}
		if (info.TryGetProperty(PropertyName._connectedObject, out var value26))
		{
			_connectedObject = value26.As<GodotObject>();
		}
		if (info.TryGetProperty(PropertyName._updateTimer, out var value27))
		{
			_updateTimer = value27.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._visibilityHostWindow, out var value28))
		{
			_visibilityHostWindow = value28.As<Window>();
		}
		if (info.TryGetProperty(PropertyName._updateTreePending, out var value29))
		{
			_updateTreePending = value29.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._changing, out var value30))
		{
			_changing = value30.As<int>();
		}
		if (info.TryGetProperty(PropertyName._updateTimerCallbackCount, out var value31))
		{
			_updateTimerCallbackCount = value31.As<int>();
		}
		if (info.TryGetProperty(PropertyName._propertyPollPassCount, out var value32))
		{
			_propertyPollPassCount = value32.As<int>();
		}
		if (info.TryGetProperty(PropertyName._propertyPollReadCount, out var value33))
		{
			_propertyPollReadCount = value33.As<int>();
		}
		if (info.TryGetProperty(PropertyName._objectHeader, out var value34))
		{
			_objectHeader = value34.As<HBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._headerIcon, out var value35))
		{
			_headerIcon = value35.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._headerLabel, out var value36))
		{
			_headerLabel = value36.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._headerClassLabel, out var value37))
		{
			_headerClassLabel = value37.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._headerIdLabel, out var value38))
		{
			_headerIdLabel = value38.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._animationPreview, out var value39))
		{
			_animationPreview = value39.As<AdobeAnimateInspectorPreview>();
		}
		if (info.TryGetProperty(PropertyName._animationPreviewEnabled, out var value40))
		{
			_animationPreviewEnabled = value40.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._metadataEditDialog, out var value41))
		{
			_metadataEditDialog = value41.As<ConfirmationDialog>();
		}
		if (info.TryGetProperty(PropertyName._selectionHistory, out var value42))
		{
			_selectionHistory = value42.As<XWEditorSelectionHistory>();
		}
		if (info.TryGetProperty(PropertyName._selectedEditor, out var value43))
		{
			_selectedEditor = value43.As<XWInspectorPropertyEditorBase>();
		}
		if (info.TryGetProperty(PropertyName._resourcePreview, out var value44))
		{
			_resourcePreview = value44.As<XWResourcePreview>();
		}
		if (info.TryGetProperty(PropertyName._propertyContextMenu, out var value45))
		{
			_propertyContextMenu = value45.As<PopupMenu>();
		}
		if (info.TryGetProperty(PropertyName._newResourceSelectedCallable, out var value46))
		{
			_newResourceSelectedCallable = value46.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName._saveResourceMenuCallable, out var value47))
		{
			_saveResourceMenuCallable = value47.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName._resourceExtraMenuCallable, out var value48))
		{
			_resourceExtraMenuCallable = value48.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName._historyMenuCallable, out var value49))
		{
			_historyMenuCallable = value49.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName._objectMenuCallable, out var value50))
		{
			_objectMenuCallable = value50.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName._propertyContextMenuCallable, out var value51))
		{
			_propertyContextMenuCallable = value51.As<Callable>();
		}
		if (info.TryGetProperty(PropertyName._contextMenuPropertyName, out var value52))
		{
			_contextMenuPropertyName = value52.As<string>();
		}
		if (info.TryGetProperty(PropertyName._contextMenuEditor, out var value53))
		{
			_contextMenuEditor = value53.As<XWInspectorPropertyEditorBase>();
		}
		if (info.TryGetSignalEventDelegate<ObjectChangedEventHandler>(SignalName.ObjectChanged, out var value54))
		{
			backing_ObjectChanged = value54;
		}
		if (info.TryGetSignalEventDelegate<ObjectPropertyChangedEventHandler>(SignalName.ObjectPropertyChanged, out var value55))
		{
			backing_ObjectPropertyChanged = value55;
		}
		if (info.TryGetSignalEventDelegate<PropertyKeyedEventHandler>(SignalName.PropertyKeyed, out var value56))
		{
			backing_PropertyKeyed = value56;
		}
		if (info.TryGetSignalEventDelegate<ResourceSelectedEventHandler>(SignalName.ResourceSelected, out var value57))
		{
			backing_ResourceSelected = value57;
		}
		if (info.TryGetSignalEventDelegate<NavigatedBackEventHandler>(SignalName.NavigatedBack, out var value58))
		{
			backing_NavigatedBack = value58;
		}
		if (info.TryGetSignalEventDelegate<NavigatedForwardEventHandler>(SignalName.NavigatedForward, out var value59))
		{
			backing_NavigatedForward = value59;
		}
		if (info.TryGetSignalEventDelegate<ResourceNewRequestedEventHandler>(SignalName.ResourceNewRequested, out var value60))
		{
			backing_ResourceNewRequested = value60;
		}
		if (info.TryGetSignalEventDelegate<ResourceLoadRequestedEventHandler>(SignalName.ResourceLoadRequested, out var value61))
		{
			backing_ResourceLoadRequested = value61;
		}
		if (info.TryGetSignalEventDelegate<ResourceSaveRequestedEventHandler>(SignalName.ResourceSaveRequested, out var value62))
		{
			backing_ResourceSaveRequested = value62;
		}
		if (info.TryGetSignalEventDelegate<ResourceSaveAsRequestedEventHandler>(SignalName.ResourceSaveAsRequested, out var value63))
		{
			backing_ResourceSaveAsRequested = value63;
		}
		if (info.TryGetSignalEventDelegate<ResourceCopyRequestedEventHandler>(SignalName.ResourceCopyRequested, out var value64))
		{
			backing_ResourceCopyRequested = value64;
		}
		if (info.TryGetSignalEventDelegate<ResourceShowInFilesystemEventHandler>(SignalName.ResourceShowInFilesystem, out var value65))
		{
			backing_ResourceShowInFilesystem = value65;
		}
		if (info.TryGetSignalEventDelegate<ResourceMakeBuiltInEventHandler>(SignalName.ResourceMakeBuiltIn, out var value66))
		{
			backing_ResourceMakeBuiltIn = value66;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(13)
		{
			new MethodInfo(SignalName.ObjectChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ObjectPropertyChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(SignalName.PropertyKeyed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(SignalName.ResourceSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(SignalName.NavigatedBack, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.NavigatedForward, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ResourceNewRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourceType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(SignalName.ResourceLoadRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ResourceSaveRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ResourceSaveAsRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ResourceCopyRequested, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ResourceShowInFilesystem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(SignalName.ResourceMakeBuiltIn, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalObjectChanged()
	{
		EmitSignal(SignalName.ObjectChanged, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalObjectPropertyChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
		StringName objectPropertyChanged = SignalName.ObjectPropertyChanged;
		_003C_003Ey__InlineArray4<Variant> buffer = default;
		buffer[0] = obj;
		buffer[1] = property;
		buffer[2] = field;
		buffer[3] = value;
		EmitSignal(objectPropertyChanged, buffer);
	}

	protected void EmitSignalPropertyKeyed(StringName property, Variant value)
	{
		StringName propertyKeyed = SignalName.PropertyKeyed;
		_003C_003Ey__InlineArray2<Variant> buffer = default;
		buffer[0] = property;
		buffer[1] = value;
		EmitSignal(propertyKeyed, buffer);
	}

	protected void EmitSignalResourceSelected(Resource resource)
	{
		EmitSignal(SignalName.ResourceSelected, new ReadOnlySpan<Variant>((Variant)resource));
	}

	protected void EmitSignalNavigatedBack()
	{
		EmitSignal(SignalName.NavigatedBack, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalNavigatedForward()
	{
		EmitSignal(SignalName.NavigatedForward, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalResourceNewRequested(string resourceType)
	{
		EmitSignal(SignalName.ResourceNewRequested, new ReadOnlySpan<Variant>((Variant)resourceType));
	}

	protected void EmitSignalResourceLoadRequested()
	{
		EmitSignal(SignalName.ResourceLoadRequested, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalResourceSaveRequested()
	{
		EmitSignal(SignalName.ResourceSaveRequested, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalResourceSaveAsRequested()
	{
		EmitSignal(SignalName.ResourceSaveAsRequested, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalResourceCopyRequested()
	{
		EmitSignal(SignalName.ResourceCopyRequested, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalResourceShowInFilesystem()
	{
		EmitSignal(SignalName.ResourceShowInFilesystem, default(ReadOnlySpan<Variant>));
	}

	protected void EmitSignalResourceMakeBuiltIn()
	{
		EmitSignal(SignalName.ResourceMakeBuiltIn, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.ObjectChanged && args.Count == 0)
		{
			backing_ObjectChanged?.Invoke();
		}
		else if (signal == SignalName.ObjectPropertyChanged && args.Count == 4)
		{
			backing_ObjectPropertyChanged?.Invoke(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
		}
		else if (signal == SignalName.PropertyKeyed && args.Count == 2)
		{
			backing_PropertyKeyed?.Invoke(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
		}
		else if (signal == SignalName.ResourceSelected && args.Count == 1)
		{
			backing_ResourceSelected?.Invoke(VariantUtils.ConvertTo<Resource>(in args[0]));
		}
		else if (signal == SignalName.NavigatedBack && args.Count == 0)
		{
			backing_NavigatedBack?.Invoke();
		}
		else if (signal == SignalName.NavigatedForward && args.Count == 0)
		{
			backing_NavigatedForward?.Invoke();
		}
		else if (signal == SignalName.ResourceNewRequested && args.Count == 1)
		{
			backing_ResourceNewRequested?.Invoke(VariantUtils.ConvertTo<string>(in args[0]));
		}
		else if (signal == SignalName.ResourceLoadRequested && args.Count == 0)
		{
			backing_ResourceLoadRequested?.Invoke();
		}
		else if (signal == SignalName.ResourceSaveRequested && args.Count == 0)
		{
			backing_ResourceSaveRequested?.Invoke();
		}
		else if (signal == SignalName.ResourceSaveAsRequested && args.Count == 0)
		{
			backing_ResourceSaveAsRequested?.Invoke();
		}
		else if (signal == SignalName.ResourceCopyRequested && args.Count == 0)
		{
			backing_ResourceCopyRequested?.Invoke();
		}
		else if (signal == SignalName.ResourceShowInFilesystem && args.Count == 0)
		{
			backing_ResourceShowInFilesystem?.Invoke();
		}
		else if (signal == SignalName.ResourceMakeBuiltIn && args.Count == 0)
		{
			backing_ResourceMakeBuiltIn?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.ObjectChanged)
		{
			return true;
		}
		if (signal == SignalName.ObjectPropertyChanged)
		{
			return true;
		}
		if (signal == SignalName.PropertyKeyed)
		{
			return true;
		}
		if (signal == SignalName.ResourceSelected)
		{
			return true;
		}
		if (signal == SignalName.NavigatedBack)
		{
			return true;
		}
		if (signal == SignalName.NavigatedForward)
		{
			return true;
		}
		if (signal == SignalName.ResourceNewRequested)
		{
			return true;
		}
		if (signal == SignalName.ResourceLoadRequested)
		{
			return true;
		}
		if (signal == SignalName.ResourceSaveRequested)
		{
			return true;
		}
		if (signal == SignalName.ResourceSaveAsRequested)
		{
			return true;
		}
		if (signal == SignalName.ResourceCopyRequested)
		{
			return true;
		}
		if (signal == SignalName.ResourceShowInFilesystem)
		{
			return true;
		}
		if (signal == SignalName.ResourceMakeBuiltIn)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
