using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.ModSystem;

namespace PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/GameplayLogic/XWGameplayResourcePickerWindow.cs")]
public class XWGameplayResourcePickerWindow : Window
{
	public new class MethodName : Window.MethodName
	{
		public static readonly StringName Create = "Create";

		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _Process = "_Process";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName SetResourceLibraryChrome = "SetResourceLibraryChrome";

		public static readonly StringName ConfigureCatalogChrome = "ConfigureCatalogChrome";

		public static readonly StringName TryConfirmIndexedResource = "TryConfirmIndexedResource";

		public static readonly StringName Dismiss = "Dismiss";

		public static readonly StringName HandlePickerVisibilityChanged = "HandlePickerVisibilityChanged";

		public static readonly StringName BindCategoryButton = "BindCategoryButton";

		public static readonly StringName PopupResponsive = "PopupResponsive";

		public static readonly StringName GetAvailableHostSize = "GetAvailableHostSize";

		public static readonly StringName SetActiveCategory = "SetActiveCategory";

		public static readonly StringName RebuildChoiceRows = "RebuildChoiceRows";

		public static readonly StringName SelectChoiceRow = "SelectChoiceRow";

		public static readonly StringName SelectCurrentKey = "SelectCurrentKey";

		public static readonly StringName PreviewSelectedAudio = "PreviewSelectedAudio";

		public static readonly StringName StopSelectedAudio = "StopSelectedAudio";

		public static readonly StringName OnAudioPreviewFinished = "OnAudioPreviewFinished";

		public static readonly StringName StopAudioPreview = "StopAudioPreview";

		public static readonly StringName UpdateProcessState = "UpdateProcessState";

		public static readonly StringName LoadAudioStream = "LoadAudioStream";

		public static readonly StringName ConfirmSelection = "ConfirmSelection";

		public static readonly StringName ClearPreview = "ClearPreview";

		public static readonly StringName FormatAudioTime = "FormatAudioTime";

		public static readonly StringName ScheduleResourceLibraryIndex = "ScheduleResourceLibraryIndex";

		public static readonly StringName TryBeginResourceLibraryIndex = "TryBeginResourceLibraryIndex";

		public static readonly StringName BeginResourceLibraryIndex = "BeginResourceLibraryIndex";

		public static readonly StringName PollResourceLibraryIndex = "PollResourceLibraryIndex";

		public static readonly StringName CancelResourceLibraryIndex = "CancelResourceLibraryIndex";

		public static readonly StringName IsCardResourceLibrary = "IsCardResourceLibrary";

		public static readonly StringName IsCharacterResourceLibrary = "IsCharacterResourceLibrary";

		public static readonly StringName ReadTextResourceReference = "ReadTextResourceReference";

		public static readonly StringName ReadTextResourceReferenceFromFile = "ReadTextResourceReferenceFromFile";

		public static readonly StringName IsEditorResourceExtension = "IsEditorResourceExtension";

		public static readonly StringName ReadResourceTypeMarker = "ReadResourceTypeMarker";

		public static readonly StringName EnsureGlobalResourceClassBases = "EnsureGlobalResourceClassBases";

		public static readonly StringName IsDirectImagePath = "IsDirectImagePath";

		public static readonly StringName IsAudioPath = "IsAudioPath";

		public static readonly StringName NormalizeResourcePath = "NormalizeResourcePath";

		public static readonly StringName LoadRegistryDictionary = "LoadRegistryDictionary";

		public static readonly StringName LoadPreviewTexture = "LoadPreviewTexture";

		public static readonly StringName LoadExternalImagePreview = "LoadExternalImagePreview";

		public static readonly StringName FindPackedScenePreviewTexture = "FindPackedScenePreviewTexture";

		public static readonly StringName GetFallbackIcon = "GetFallbackIcon";

		public static readonly StringName ResolveUidPath = "ResolveUidPath";

		public static readonly StringName ReadString = "ReadString";

		public static readonly StringName KindGlyph = "KindGlyph";

		public static readonly StringName Humanize = "Humanize";
	}

	public new class PropertyName : Window.PropertyName
	{
		public static readonly StringName ResourceLibraryProjectPath = "ResourceLibraryProjectPath";

		public static readonly StringName IsResourceLibraryIndexing = "IsResourceLibraryIndexing";

		public static readonly StringName _titleLabel = "_titleLabel";

		public static readonly StringName _subtitleLabel = "_subtitleLabel";

		public static readonly StringName _categoryTabs = "_categoryTabs";

		public static readonly StringName _searchEdit = "_searchEdit";

		public static readonly StringName _choiceTree = "_choiceTree";

		public static readonly StringName _previewTexture = "_previewTexture";

		public static readonly StringName _displayNameLabel = "_displayNameLabel";

		public static readonly StringName _resourceKeyLabel = "_resourceKeyLabel";

		public static readonly StringName _sourceBadge = "_sourceBadge";

		public static readonly StringName _missingBadge = "_missingBadge";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _confirmButton = "_confirmButton";

		public static readonly StringName _audioPreviewControls = "_audioPreviewControls";

		public static readonly StringName _audioPreviewButton = "_audioPreviewButton";

		public static readonly StringName _audioStopButton = "_audioStopButton";

		public static readonly StringName _audioProgress = "_audioProgress";

		public static readonly StringName _audioTimeLabel = "_audioTimeLabel";

		public static readonly StringName _audioPreviewPlayer = "_audioPreviewPlayer";

		public static readonly StringName _audioPreviewLength = "_audioPreviewLength";

		public static readonly StringName _activeKind = "_activeKind";

		public static readonly StringName _currentKey = "_currentKey";

		public static readonly StringName _resourceLibraryMode = "_resourceLibraryMode";

		public static readonly StringName _resourceLibraryCategory = "_resourceLibraryCategory";

		public static readonly StringName _resourceLibraryDisplayName = "_resourceLibraryDisplayName";

		public static readonly StringName _resourceLibraryProjectPath = "_resourceLibraryProjectPath";

		public static readonly StringName _resourceLibraryIconPath = "_resourceLibraryIconPath";

		public static readonly StringName _resourceLibraryIndexStartFrames = "_resourceLibraryIndexStartFrames";

		public static readonly StringName _resourceLibraryIndexGeneration = "_resourceLibraryIndexGeneration";

		public static readonly StringName _resourceLibraryIndexTaskGeneration = "_resourceLibraryIndexTaskGeneration";
	}

	public new class SignalName : Window.SignalName
	{
	}

	private const string ScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/GameplayLogic/XWGameplayResourcePickerWindow.tscn";

	private const string CharacterRegistryPath = "res://Asset/Config/Character/CharacterResource.json";

	private const string ProjectileRegistryPath = "res://Asset/Config/Projectile/ProjectileResource.json";

	private const string PacketBankRegistryPath = "res://Asset/Config/PacketBank/PacketBankResource.json";

	private const string MapRegistryPath = "res://Asset/Config/Map/MapResource.json";

	private const string BgmRegistryPath = "res://Asset/Config/BGM/BGMResource.json";

	private const string LevelRegistryPath = "res://Asset/Config/Level/LevelResource.json";

	private const string AudioRegistryPath = "res://Asset/Config/Audio/AudioResource.json";

	private const int MaximumVisibleChoices = 120;

	private const int CompactIconSize = 32;

	private const int ResourceLibraryWarmupFrames = 2;

	private static readonly System.Collections.Generic.Dictionary<string, string> GlobalResourceClassBases = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private static bool _globalResourceClassBasesLoaded;

	private readonly System.Collections.Generic.Dictionary<XWGameplayResourceKind, List<XWGameplayResourceChoice>> _choiceCache = new System.Collections.Generic.Dictionary<XWGameplayResourceKind, List<XWGameplayResourceChoice>>();

	private readonly System.Collections.Generic.Dictionary<string, string> _thumbnailPaths = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

	private readonly System.Collections.Generic.Dictionary<string, Texture2D> _loadedThumbnails = new System.Collections.Generic.Dictionary<string, Texture2D>(StringComparer.OrdinalIgnoreCase);

	private readonly HashSet<string> _missingThumbnails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	private readonly System.Collections.Generic.Dictionary<XWGameplayResourceKind, Texture2D> _fallbackIcons = new System.Collections.Generic.Dictionary<XWGameplayResourceKind, Texture2D>();

	private readonly System.Collections.Generic.Dictionary<XWGameplayResourceKind, Button> _categoryButtons = new System.Collections.Generic.Dictionary<XWGameplayResourceKind, Button>();

	private Label _titleLabel;

	private Label _subtitleLabel;

	private Control _categoryTabs;

	private LineEdit _searchEdit;

	private readonly List<XWGameplayResourceChoice> _visibleChoices = new List<XWGameplayResourceChoice>();

	private readonly System.Collections.Generic.Dictionary<string, TreeItem> _visibleItemsByKey = new System.Collections.Generic.Dictionary<string, TreeItem>(StringComparer.OrdinalIgnoreCase);

	private Tree _choiceTree;

	private TextureRect _previewTexture;

	private Label _displayNameLabel;

	private Label _resourceKeyLabel;

	private Label _sourceBadge;

	private Label _missingBadge;

	private Label _statusLabel;

	private Button _confirmButton;

	private Control _audioPreviewControls;

	private Button _audioPreviewButton;

	private Button _audioStopButton;

	private HSlider _audioProgress;

	private Label _audioTimeLabel;

	private AudioStreamPlayer _audioPreviewPlayer;

	private double _audioPreviewLength;

	private XWGameplayResourceKind _activeKind;

	private XWGameplayResourceChoice _selectedChoice;

	private string _currentKey = "";

	private Action<XWGameplayResourceChoice> _onChosen;

	private Predicate<XWGameplayResourceChoice> _choiceFilter;

	private bool _resourceLibraryMode;

	private string _resourceLibraryCategory = "";

	private string _resourceLibraryDisplayName = "";

	private string _resourceLibraryProjectPath = "";

	private string _resourceLibraryIconPath = "";

	private IReadOnlyList<string> _resourceLibraryClassNames = System.Array.Empty<string>();

	private IReadOnlyList<string> _resourceLibraryBuiltInPathMarkers = System.Array.Empty<string>();

	private CancellationTokenSource _resourceLibraryIndexCancellation;

	private Task<XWEditorResourceIndexResult> _resourceLibraryIndexTask;

	private int _resourceLibraryIndexStartFrames;

	private long _resourceLibraryIndexGeneration;

	private long _resourceLibraryIndexTaskGeneration;

	public string ResourceLibraryProjectPath => _resourceLibraryProjectPath;

	public bool IsResourceLibraryIndexing
	{
		get
		{
			if (_resourceLibraryIndexStartFrames <= 0)
			{
				return _resourceLibraryIndexTask != null;
			}
			return true;
		}
	}

	public static XWGameplayResourcePickerWindow Create()
	{
		return ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/GameplayLogic/XWGameplayResourcePickerWindow.tscn", "", ResourceLoader.CacheMode.Ignore)?.Instantiate<XWGameplayResourcePickerWindow>(PackedScene.GenEditState.Disabled);
	}

	public override void _Ready()
	{
		ProcessMode = ProcessModeEnum.Always;
		SetProcess(enable: false);
		_titleLabel = GetNode<Label>("%Title");
		_subtitleLabel = GetNode<Label>("%Subtitle");
		_categoryTabs = GetNode<Control>("%CategoryTabs");
		_searchEdit = GetNode<LineEdit>("%SearchEdit");
		_choiceTree = GetNode<Tree>("%ChoiceTree");
		_previewTexture = GetNode<TextureRect>("%PreviewTexture");
		_displayNameLabel = GetNode<Label>("%DisplayNameLabel");
		_resourceKeyLabel = GetNode<Label>("%ResourceKeyLabel");
		_sourceBadge = GetNode<Label>("%SourceBadge");
		_missingBadge = GetNode<Label>("%MissingBadge");
		_statusLabel = GetNode<Label>("%StatusLabel");
		_confirmButton = GetNode<Button>("%ConfirmButton");
		_audioPreviewControls = GetNode<Control>("%AudioPreviewControls");
		_audioPreviewButton = GetNode<Button>("%AudioPreviewButton");
		_audioStopButton = GetNode<Button>("%AudioStopButton");
		_audioProgress = GetNode<HSlider>("%AudioProgress");
		_audioTimeLabel = GetNode<Label>("%AudioTimeLabel");
		_audioPreviewPlayer = GetNode<AudioStreamPlayer>("%AudioPreviewPlayer");
		_searchEdit.TextChanged += (string _) =>
		{
			RebuildChoiceRows();
		};
		_choiceTree.ItemSelected += SelectChoiceRow;
		_choiceTree.ItemActivated += ConfirmSelection;
		_confirmButton.Pressed += ConfirmSelection;
		_audioPreviewButton.Pressed += PreviewSelectedAudio;
		_audioStopButton.Pressed += StopSelectedAudio;
		_audioPreviewPlayer.Finished += OnAudioPreviewFinished;
		GetNode<Button>("%CancelButton").Pressed += Dismiss;
		CloseRequested += Dismiss;
		VisibilityChanged += HandlePickerVisibilityChanged;
		BindCategoryButton("%ResourceTab", XWGameplayResourceKind.Resource);
		BindCategoryButton("%CharacterTab", XWGameplayResourceKind.Character);
		BindCategoryButton("%CardTab", XWGameplayResourceKind.Card);
		BindCategoryButton("%ProjectileTab", XWGameplayResourceKind.Projectile);
		BindCategoryButton("%PacketBankTab", XWGameplayResourceKind.PacketBank);
		BindCategoryButton("%MapTab", XWGameplayResourceKind.Map);
		BindCategoryButton("%BgmTab", XWGameplayResourceKind.Bgm);
		BindCategoryButton("%LevelTab", XWGameplayResourceKind.Level);
		BindCategoryButton("%AudioTab", XWGameplayResourceKind.Audio);
		ClearPreview();
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		TryBeginResourceLibraryIndex();
		PollResourceLibraryIndex();
		if (GodotObject.IsInstanceValid(_audioPreviewPlayer) && _audioPreviewPlayer.Playing)
		{
			double num = Math.Clamp(_audioPreviewPlayer.GetPlaybackPosition(), 0.0, Math.Max(0.0, _audioPreviewLength));
			_audioProgress.Value = num;
			_audioTimeLabel.Text = FormatAudioTime(num) + " / " + FormatAudioTime(_audioPreviewLength);
		}
		UpdateProcessState();
	}

	public override void _ExitTree()
	{
		CancelResourceLibraryIndex();
		base._ExitTree();
	}

	public void Open(XWGameplayResourceKind kind, string currentKey, Action<XWGameplayResourceChoice> onChosen, Predicate<XWGameplayResourceChoice> choiceFilter = null, bool lockKind = false, string catalogTitle = "")
	{
		CancelResourceLibraryIndex();
		_resourceLibraryMode = false;
		SetResourceLibraryChrome(resourceLibraryMode: false);
		if (kind == XWGameplayResourceKind.Audio)
		{
			_choiceCache.Remove(XWGameplayResourceKind.Audio);
		}
		_activeKind = kind;
		ConfigureCatalogChrome(kind, lockKind, catalogTitle);
		_currentKey = currentKey?.StripEdges() ?? "";
		_onChosen = onChosen;
		_choiceFilter = choiceFilter;
		if (GodotObject.IsInstanceValid(_searchEdit))
		{
			_searchEdit.Text = "";
		}
		ClearPreview();
		SetActiveCategory(kind);
		RebuildChoiceRows();
		SelectCurrentKey();
		if (IsInsideTree())
		{
			PopupResponsive();
		}
	}

	public void OpenChoices(string catalogTitle, string catalogSubtitle, string currentKey, IReadOnlyList<XWGameplayResourceChoice> choices, Action<XWGameplayResourceChoice> onChosen, string confirmText = "选择此项")
	{
		CancelResourceLibraryIndex();
		_resourceLibraryMode = false;
		_activeKind = XWGameplayResourceKind.Resource;
		_currentKey = currentKey?.StripEdges() ?? "";
		_onChosen = onChosen;
		_choiceFilter = null;
		_choiceCache[XWGameplayResourceKind.Resource] = choices?.Where((XWGameplayResourceChoice choice) => choice != null).ToList() ?? new List<XWGameplayResourceChoice>();
		if (GodotObject.IsInstanceValid(_searchEdit))
		{
			_searchEdit.Text = "";
		}
		ClearPreview();
		SetResourceLibraryChrome(resourceLibraryMode: false);
		ConfigureCatalogChrome(XWGameplayResourceKind.Resource, lockKind: true, catalogTitle);
		Title = (string.IsNullOrWhiteSpace(catalogTitle) ? "资源图鉴" : catalogTitle);
		_titleLabel.Text = Title;
		_subtitleLabel.Text = (string.IsNullOrWhiteSpace(catalogSubtitle) ? "从游戏资源与当前 Mod 内容中选择" : catalogSubtitle);
		_confirmButton.Text = (string.IsNullOrWhiteSpace(confirmText) ? "选择此项" : confirmText);
		SetActiveCategory(XWGameplayResourceKind.Resource);
		RebuildChoiceRows();
		SelectCurrentKey();
		if (IsInsideTree())
		{
			PopupResponsive();
		}
	}

	public void OpenResourceLibrary(string category, string displayName, string currentPath, string projectPath, IReadOnlyList<string> resourceClassNames, IReadOnlyList<string> builtInPathMarkers, string iconPath, Action<XWGameplayResourceChoice> onChosen)
	{
		CancelResourceLibraryIndex();
		_resourceLibraryMode = true;
		_resourceLibraryCategory = category?.StripEdges() ?? "";
		_resourceLibraryDisplayName = (string.IsNullOrWhiteSpace(displayName) ? "资源" : displayName.StripEdges());
		_resourceLibraryProjectPath = projectPath?.StripEdges() ?? "";
		_resourceLibraryClassNames = resourceClassNames?.ToArray() ?? System.Array.Empty<string>();
		_resourceLibraryBuiltInPathMarkers = builtInPathMarkers?.ToArray() ?? System.Array.Empty<string>();
		_resourceLibraryIconPath = iconPath?.StripEdges() ?? "";
		_activeKind = XWGameplayResourceKind.Resource;
		_currentKey = NormalizeResourcePath(currentPath);
		_onChosen = onChosen;
		_choiceFilter = null;
		_choiceCache[XWGameplayResourceKind.Resource] = new List<XWGameplayResourceChoice>();
		_fallbackIcons.Remove(XWGameplayResourceKind.Resource);
		if (GodotObject.IsInstanceValid(_searchEdit))
		{
			_searchEdit.Text = "";
		}
		ClearPreview();
		SetResourceLibraryChrome(resourceLibraryMode: true);
		SetActiveCategory(XWGameplayResourceKind.Resource);
		if (IsInsideTree())
		{
			PopupResponsive();
		}
		ScheduleResourceLibraryIndex();
	}

	private void SetResourceLibraryChrome(bool resourceLibraryMode)
	{
		foreach (var (xWGameplayResourceKind2, button2) in _categoryButtons)
		{
			button2.Visible = (resourceLibraryMode ? (xWGameplayResourceKind2 == XWGameplayResourceKind.Resource) : (xWGameplayResourceKind2 != XWGameplayResourceKind.Resource));
		}
		if (GodotObject.IsInstanceValid(_categoryTabs))
		{
			_categoryTabs.Visible = true;
		}
		if (GodotObject.IsInstanceValid(_titleLabel))
		{
			_titleLabel.Text = (resourceLibraryMode ? (_resourceLibraryDisplayName + "图鉴") : "战斗资源图鉴");
		}
		if (GodotObject.IsInstanceValid(_subtitleLabel))
		{
			_subtitleLabel.Text = (resourceLibraryMode ? "从当前 Mod 与游戏内资源中选择，确认后才加载单个资源" : "像选卡一样选择角色、地图、关卡与声音");
		}
		if (GodotObject.IsInstanceValid(_searchEdit))
		{
			_searchEdit.PlaceholderText = (resourceLibraryMode ? "搜索资源名或路径…" : "搜索名称或 Key…");
		}
		if (GodotObject.IsInstanceValid(_confirmButton))
		{
			_confirmButton.Text = (resourceLibraryMode ? "打开此资源" : "选用此资源");
		}
		Title = (resourceLibraryMode ? (_resourceLibraryDisplayName + "图鉴") : "战斗资源图鉴");
	}

	private void ConfigureCatalogChrome(XWGameplayResourceKind kind, bool lockKind, string catalogTitle)
	{
		if (!lockKind)
		{
			return;
		}
		foreach (var (xWGameplayResourceKind2, button2) in _categoryButtons)
		{
			button2.Visible = xWGameplayResourceKind2 == kind;
		}
		string text = (Title = ((!string.IsNullOrWhiteSpace(catalogTitle)) ? catalogTitle : ((kind == XWGameplayResourceKind.Audio) ? "游戏音频图鉴" : (KindGlyph(kind) + "图鉴"))));
		_titleLabel.Text = text;
		_subtitleLabel.Text = ((kind == XWGameplayResourceKind.Audio) ? "从游戏注册音频与当前 Mod 添加的音效、BGM 中选择并试听" : "从游戏与当前 Mod 的资源目录中选择");
		_confirmButton.Text = ((kind == XWGameplayResourceKind.Audio) ? "选用此音频" : "选用此资源");
	}

	public IReadOnlyList<XWGameplayResourceChoice> GetIndexedChoices(XWGameplayResourceKind kind)
	{
		return GetOrBuildChoices(kind);
	}

	public bool TryConfirmIndexedResource(string resourcePath)
	{
		if (string.IsNullOrWhiteSpace(resourcePath) || IsResourceLibraryIndexing)
		{
			return false;
		}
		XWGameplayResourceChoice xWGameplayResourceChoice = GetOrBuildChoices(XWGameplayResourceKind.Resource).FirstOrDefault((XWGameplayResourceChoice item) => string.Equals(item.ResourcePath, resourcePath.Replace('\\', '/').Trim(), StringComparison.OrdinalIgnoreCase));
		if (xWGameplayResourceChoice == null)
		{
			return false;
		}
		SelectChoice(xWGameplayResourceChoice);
		ConfirmSelection();
		return true;
	}

	public void Dismiss()
	{
		CancelResourceLibraryIndex();
		StopAudioPreview(clearStream: true);
		_onChosen = null;
		_choiceFilter = null;
		Hide();
	}

	private void HandlePickerVisibilityChanged()
	{
		if (!Visible)
		{
			CancelResourceLibraryIndex();
		}
	}

	private void BindCategoryButton(NodePath path, XWGameplayResourceKind kind)
	{
		Button node = GetNode<Button>(path);
		node.Icon = GetFallbackIcon(kind);
		node.ExpandIcon = false;
		node.TooltipText = KindGlyph(kind) + "·游戏资源入口";
		_categoryButtons[kind] = node;
		node.Pressed += () =>
		{
			_activeKind = kind;
			SetActiveCategory(kind);
			RebuildChoiceRows();
		};
	}

	private void PopupResponsive()
	{
		Vector2I availableHostSize = GetAvailableHostSize();
		int x = Math.Clamp(availableHostSize.X - 32, 680, 1120);
		int y = Math.Clamp(availableHostSize.Y - 32, 480, 720);
		PopupCentered(new Vector2I(x, y));
	}

	private Vector2I GetAvailableHostSize()
	{
		Node parent = GetParent();
		while (GodotObject.IsInstanceValid(parent))
		{
			if (parent is Window window && window != this && window.Size.X > 0 && window.Size.Y > 0)
			{
				return window.Size;
			}
			parent = parent.GetParent();
		}
		return GetTree()?.Root?.Size ?? new Vector2I(1120, 720);
	}

	private void SetActiveCategory(XWGameplayResourceKind kind)
	{
		foreach (var (xWGameplayResourceKind2, button2) in _categoryButtons)
		{
			button2.ButtonPressed = xWGameplayResourceKind2 == kind;
		}
	}

	private void RebuildChoiceRows()
	{
		if (!GodotObject.IsInstanceValid(_choiceTree))
		{
			return;
		}
		_choiceTree.Clear();
		_visibleChoices.Clear();
		_visibleItemsByKey.Clear();
		TreeItem root = _choiceTree.CreateItem();
		if (_resourceLibraryMode && _activeKind == XWGameplayResourceKind.Resource && (_resourceLibraryIndexTask != null || _resourceLibraryIndexStartFrames > 0))
		{
			ClearPreview("正在后台索引资源…");
			_statusLabel.Text = "正在后台索引资源，窗口可以继续操作…";
			return;
		}
		string query = _searchEdit?.Text?.StripEdges() ?? "";
		List<XWGameplayResourceChoice> list = GetOrBuildChoices(_activeKind).Where((XWGameplayResourceChoice choice) =>
		{
			Predicate<XWGameplayResourceChoice> choiceFilter = _choiceFilter;
			return (choiceFilter == null || choiceFilter(choice)) && (string.IsNullOrWhiteSpace(query) || choice.Key.Contains(query, StringComparison.OrdinalIgnoreCase) || choice.DisplayName.Contains(query, StringComparison.OrdinalIgnoreCase));
		}).ToList();
		int num = Math.Min(list.Count, 120);
		for (int num2 = 0; num2 < num; num2++)
		{
			_visibleChoices.Add(list[num2]);
			TreeItem value = CreateChoiceRow(root, list[num2], _visibleChoices.Count - 1);
			_visibleItemsByKey.TryAdd(list[num2].Key, value);
		}
		_statusLabel.Text = ((list.Count > 120) ? $"找到 {list.Count} 项，当前显示前 {120} 项 · 输入关键词可快速定位" : $"找到 {list.Count} 项 · 列表只读元数据，选中后才加载单个预览");
		if (list.Count == 0)
		{
			ClearPreview("没有符合条件的资源");
		}
	}

	private TreeItem CreateChoiceRow(TreeItem root, XWGameplayResourceChoice choice, int visibleIndex)
	{
		TreeItem treeItem = _choiceTree.CreateItem(root);
		treeItem.SetText(0, choice.DisplayName);
		treeItem.SetText(1, GetChoiceSourceText(choice));
		treeItem.SetTooltipText(0, $"{choice.DisplayName}\n{choice.Key}\n{choice.ResourcePath}");
		treeItem.SetTooltipText(1, choice.ResourcePath);
		treeItem.SetIcon(0, choice.Thumbnail ?? GetFallbackIcon(choice.Kind));
		treeItem.SetIconMaxWidth(0, 32);
		treeItem.SetCustomColor(1, choice.IsModResource ? new Color("75cfff") : new Color("9ccf7a"));
		treeItem.SetMetadata(0, visibleIndex);
		return treeItem;
	}

	private static string GetChoiceSourceText(XWGameplayResourceChoice choice)
	{
		if (choice.Kind == XWGameplayResourceKind.Audio)
		{
			if (!choice.IsModResource)
			{
				return "游戏音效";
			}
			return "MOD 音效";
		}
		if (!choice.IsModResource)
		{
			return "游戏资源";
		}
		return "MOD";
	}

	private void SelectChoiceRow()
	{
		TreeItem treeItem = _choiceTree?.GetSelected();
		if (GodotObject.IsInstanceValid(treeItem))
		{
			int num = treeItem.GetMetadata(0).AsInt32();
			if (num >= 0 && num < _visibleChoices.Count)
			{
				SelectChoice(_visibleChoices[num]);
			}
		}
	}

	private void SelectCurrentKey()
	{
		if (string.IsNullOrWhiteSpace(_currentKey))
		{
			return;
		}
		if (_visibleItemsByKey.TryGetValue(_currentKey, out var value) && GodotObject.IsInstanceValid(value))
		{
			value.Select(0);
			_choiceTree.ScrollToItem(value);
			SelectChoiceRow();
			return;
		}
		XWGameplayResourceChoice xWGameplayResourceChoice = GetOrBuildChoices(_activeKind).FirstOrDefault((XWGameplayResourceChoice choice) => string.Equals(choice.Key, _currentKey, StringComparison.OrdinalIgnoreCase));
		if (xWGameplayResourceChoice != null)
		{
			SelectChoice(xWGameplayResourceChoice);
		}
	}

	private void SelectChoice(XWGameplayResourceChoice choice)
	{
		StopAudioPreview(clearStream: true);
		_selectedChoice = EnsureThumbnail(choice);
		bool flag = _selectedChoice.Kind == XWGameplayResourceKind.Audio;
		Texture2D texture2D = _selectedChoice.Thumbnail;
		if (!_resourceLibraryMode && !GodotObject.IsInstanceValid(texture2D))
		{
			texture2D = LoadPreviewTexture(_selectedChoice.ResourcePath);
		}
		_previewTexture.CustomMinimumSize = new Vector2(254f, flag ? 112 : 260);
		_previewTexture.Texture = texture2D ?? GetFallbackIcon(_selectedChoice.Kind);
		_displayNameLabel.Text = _selectedChoice.DisplayName;
		_resourceKeyLabel.Text = (_resourceLibraryMode ? ("路径  " + _selectedChoice.ResourcePath) : ("Key  " + _selectedChoice.Key));
		_sourceBadge.Text = (_selectedChoice.IsModResource ? "MOD 资源" : "游戏内资源");
		_sourceBadge.Modulate = (_selectedChoice.IsModResource ? new Color("75cfff") : new Color("a9db7a"));
		_missingBadge.Visible = !flag && !_resourceLibraryMode && texture2D == null;
		_audioPreviewControls.Visible = flag;
		_audioPreviewButton.Disabled = !flag;
		_audioStopButton.Disabled = true;
		_audioProgress.Value = 0.0;
		_audioTimeLabel.Text = "00:00 / 00:00";
		_confirmButton.Disabled = false;
	}

	private void PreviewSelectedAudio()
	{
		XWGameplayResourceChoice selectedChoice = _selectedChoice;
		if ((object)selectedChoice != null && selectedChoice.Kind == XWGameplayResourceKind.Audio)
		{
			AudioStream audioStream = LoadAudioStream(_selectedChoice);
			if (!GodotObject.IsInstanceValid(audioStream))
			{
				_statusLabel.Text = "无法试听：" + _selectedChoice.DisplayName;
				return;
			}
			_audioPreviewPlayer.Stop();
			_audioPreviewPlayer.Stream = audioStream;
			_audioPreviewLength = Math.Max(0.0, audioStream.GetLength());
			_audioProgress.MaxValue = Math.Max(0.01, _audioPreviewLength);
			_audioProgress.Value = 0.0;
			_audioTimeLabel.Text = "00:00 / " + FormatAudioTime(_audioPreviewLength);
			_audioPreviewPlayer.Play();
			_audioPreviewButton.Disabled = true;
			_audioStopButton.Disabled = false;
			SetProcess(enable: true);
			_statusLabel.Text = "正在试听：" + _selectedChoice.DisplayName;
		}
	}

	private void StopSelectedAudio()
	{
		StopAudioPreview(clearStream: false);
		XWGameplayResourceChoice selectedChoice = _selectedChoice;
		if ((object)selectedChoice != null && selectedChoice.Kind == XWGameplayResourceKind.Audio)
		{
			_statusLabel.Text = "已停止试听：" + _selectedChoice.DisplayName;
		}
	}

	private void OnAudioPreviewFinished()
	{
		StopAudioPreview(clearStream: false);
		XWGameplayResourceChoice selectedChoice = _selectedChoice;
		if ((object)selectedChoice != null && selectedChoice.Kind == XWGameplayResourceKind.Audio)
		{
			_statusLabel.Text = "试听完成：" + _selectedChoice.DisplayName;
		}
	}

	private void StopAudioPreview(bool clearStream)
	{
		if (GodotObject.IsInstanceValid(_audioPreviewPlayer))
		{
			_audioPreviewPlayer.Stop();
			if (clearStream)
			{
				_audioPreviewPlayer.Stream = null;
			}
		}
		_audioPreviewLength = (clearStream ? 0.0 : _audioPreviewLength);
		if (GodotObject.IsInstanceValid(_audioProgress))
		{
			_audioProgress.Value = 0.0;
		}
		if (GodotObject.IsInstanceValid(_audioTimeLabel))
		{
			_audioTimeLabel.Text = "00:00 / " + FormatAudioTime(_audioPreviewLength);
		}
		if (GodotObject.IsInstanceValid(_audioPreviewButton))
		{
			Button audioPreviewButton = _audioPreviewButton;
			XWGameplayResourceChoice selectedChoice = _selectedChoice;
			audioPreviewButton.Disabled = (object)selectedChoice == null || selectedChoice.Kind != XWGameplayResourceKind.Audio;
		}
		if (GodotObject.IsInstanceValid(_audioStopButton))
		{
			_audioStopButton.Disabled = true;
		}
		UpdateProcessState();
	}

	private void UpdateProcessState()
	{
		bool flag = GodotObject.IsInstanceValid(_audioPreviewPlayer) && _audioPreviewPlayer.Playing;
		SetProcess((_resourceLibraryIndexStartFrames > 0 || _resourceLibraryIndexTask != null) | flag);
	}

	public AudioStream LoadAudioStream(string audioKey)
	{
		if (string.IsNullOrWhiteSpace(audioKey))
		{
			return null;
		}
		return LoadAudioStream(GetOrBuildChoices(XWGameplayResourceKind.Audio).FirstOrDefault((XWGameplayResourceChoice item) => string.Equals(item.Key, audioKey, StringComparison.OrdinalIgnoreCase)));
	}

	public static string GetPersistedAudioKey(XWGameplayResourceChoice choice)
	{
		if (choice == null || choice.Kind != XWGameplayResourceKind.Audio)
		{
			return "";
		}
		string result = choice.Key ?? "";
		if (!choice.IsModResource)
		{
			return result;
		}
		string text = choice.ResourcePath?.StripEdges() ?? "";
		string text2 = (string.IsNullOrWhiteSpace(text) ? "" : Path.GetFileNameWithoutExtension(text));
		if (!string.IsNullOrWhiteSpace(text2))
		{
			return text2;
		}
		return result;
	}

	private static AudioStream LoadAudioStream(XWGameplayResourceChoice choice)
	{
		if (choice == null || choice.Kind != XWGameplayResourceKind.Audio)
		{
			return null;
		}
		if (GodotObject.IsInstanceValid(ResourceManager.Instance) && ResourceManager.Instance.AUDIOS.TryGetValue(choice.Key, out var value) && value is AudioStream result)
		{
			return result;
		}
		if (ResourceLoader.Exists(choice.ResourcePath))
		{
			return ResourceLoader.Load<AudioStream>(choice.ResourcePath, "", ResourceLoader.CacheMode.Ignore);
		}
		if (!choice.IsModResource || !File.Exists(choice.ResourcePath))
		{
			return null;
		}
		return Path.GetExtension(choice.ResourcePath).ToLowerInvariant() switch
		{
			".wav" => (AudioStream)AudioStreamWav.LoadFromFile(choice.ResourcePath), 
			".ogg" => AudioStreamOggVorbis.LoadFromFile(choice.ResourcePath), 
			".mp3" => AudioStreamMP3.LoadFromFile(choice.ResourcePath), 
			_ => null, 
		};
	}

	private void ConfirmSelection()
	{
		if (!(_selectedChoice == null))
		{
			Action<XWGameplayResourceChoice> onChosen = _onChosen;
			_onChosen = null;
			_choiceFilter = null;
			CancelResourceLibraryIndex();
			StopAudioPreview(clearStream: true);
			onChosen?.Invoke(_selectedChoice);
			Hide();
		}
	}

	private void ClearPreview(string message = "从左侧选择一个资源")
	{
		StopAudioPreview(clearStream: true);
		_selectedChoice = null;
		if (GodotObject.IsInstanceValid(_previewTexture))
		{
			_previewTexture.Texture = null;
		}
		if (GodotObject.IsInstanceValid(_displayNameLabel))
		{
			_displayNameLabel.Text = message;
		}
		if (GodotObject.IsInstanceValid(_resourceKeyLabel))
		{
			_resourceKeyLabel.Text = "Key  —";
		}
		if (GodotObject.IsInstanceValid(_sourceBadge))
		{
			_sourceBadge.Text = "尚未选择";
		}
		if (GodotObject.IsInstanceValid(_missingBadge))
		{
			_missingBadge.Visible = false;
		}
		if (GodotObject.IsInstanceValid(_confirmButton))
		{
			_confirmButton.Disabled = true;
		}
		if (GodotObject.IsInstanceValid(_audioPreviewControls))
		{
			_audioPreviewControls.Visible = false;
		}
		if (GodotObject.IsInstanceValid(_audioPreviewButton))
		{
			_audioPreviewButton.Disabled = true;
		}
	}

	private static string FormatAudioTime(double seconds)
	{
		if (double.IsNaN(seconds) || double.IsInfinity(seconds) || seconds < 0.0)
		{
			seconds = 0.0;
		}
		int num = Mathf.Max(0, Mathf.FloorToInt(seconds));
		return $"{num / 60:00}:{num % 60:00}";
	}

	private List<XWGameplayResourceChoice> GetOrBuildChoices(XWGameplayResourceKind kind)
	{
		if (_choiceCache.TryGetValue(kind, out var value))
		{
			return value;
		}
		List<XWGameplayResourceChoice> list = new List<XWGameplayResourceChoice>();
		switch (kind)
		{
		case XWGameplayResourceKind.Resource:
			LoadEditorResourceChoices(list);
			break;
		case XWGameplayResourceKind.Character:
		case XWGameplayResourceKind.Card:
			LoadCharacterChoices(list, kind);
			break;
		case XWGameplayResourceKind.Projectile:
			LoadDirectRegistry(list, "res://Asset/Config/Projectile/ProjectileResource.json", kind);
			break;
		case XWGameplayResourceKind.PacketBank:
			LoadPacketBankChoices(list);
			break;
		case XWGameplayResourceKind.Map:
			LoadDirectRegistry(list, "res://Asset/Config/Map/MapResource.json", kind);
			break;
		case XWGameplayResourceKind.Bgm:
			LoadDirectRegistry(list, "res://Asset/Config/BGM/BGMResource.json", kind);
			break;
		case XWGameplayResourceKind.Level:
			LoadLevelChoices(list);
			break;
		case XWGameplayResourceKind.Audio:
			LoadDirectRegistry(list, "res://Asset/Config/Audio/AudioResource.json", kind);
			LoadRuntimeAudioChoices(list);
			LoadModAudioChoices(list);
			break;
		}
		list.Sort((XWGameplayResourceChoice left, XWGameplayResourceChoice right) => string.Compare(left.Key, right.Key, StringComparison.OrdinalIgnoreCase));
		_choiceCache[kind] = list;
		return list;
	}

	private void ScheduleResourceLibraryIndex()
	{
		_resourceLibraryIndexStartFrames = 2;
		_statusLabel.Text = "资源图鉴已打开，索引将在下一帧后台开始…";
		ClearPreview("正在准备资源索引…");
		UpdateProcessState();
	}

	private void TryBeginResourceLibraryIndex()
	{
		if (_resourceLibraryIndexStartFrames <= 0)
		{
			return;
		}
		if (!_resourceLibraryMode || !Visible)
		{
			CancelResourceLibraryIndex();
			return;
		}
		_resourceLibraryIndexStartFrames--;
		if (_resourceLibraryIndexStartFrames <= 0)
		{
			BeginResourceLibraryIndex();
		}
	}

	private void BeginResourceLibraryIndex()
	{
		List<XWEditorResourceIndexRoot> roots = BuildEditorResourceIndexRoots();
		List<XWEditorBuiltInResourceSeed> builtInSeeds = BuildBuiltInResourceSeeds();
		_resourceLibraryIndexCancellation = new CancellationTokenSource();
		CancellationToken cancellationToken = _resourceLibraryIndexCancellation.Token;
		bool readResourceType = _resourceLibraryClassNames.Count > 0;
		_resourceLibraryIndexTaskGeneration = _resourceLibraryIndexGeneration;
		_resourceLibraryIndexTask = Task.Run(() => IndexResourceLibrary(roots, readResourceType, builtInSeeds, cancellationToken), cancellationToken);
		_statusLabel.Text = "正在后台索引资源，窗口可以继续操作…";
		ClearPreview("正在后台索引资源…");
		UpdateProcessState();
	}

	private void PollResourceLibraryIndex()
	{
		if (_resourceLibraryIndexTask == null || !_resourceLibraryIndexTask.IsCompleted)
		{
			return;
		}
		Task<XWEditorResourceIndexResult> resourceLibraryIndexTask = _resourceLibraryIndexTask;
		long resourceLibraryIndexTaskGeneration = _resourceLibraryIndexTaskGeneration;
		_resourceLibraryIndexTask = null;
		_resourceLibraryIndexTaskGeneration = 0L;
		_resourceLibraryIndexCancellation?.Dispose();
		_resourceLibraryIndexCancellation = null;
		if (resourceLibraryIndexTask.IsCanceled)
		{
			UpdateProcessState();
			return;
		}
		if (resourceLibraryIndexTaskGeneration != _resourceLibraryIndexGeneration || !_resourceLibraryMode || !Visible)
		{
			UpdateProcessState();
			return;
		}
		if (resourceLibraryIndexTask.IsFaulted)
		{
			ClearPreview("资源索引失败");
			_statusLabel.Text = "资源索引失败：" + resourceLibraryIndexTask.Exception?.GetBaseException().Message;
			UpdateProcessState();
			return;
		}
		if (!_choiceCache.TryGetValue(XWGameplayResourceKind.Resource, out var value))
		{
			value = new List<XWGameplayResourceChoice>();
			_choiceCache[XWGameplayResourceKind.Resource] = value;
		}
		XWEditorResourceIndexResult result = resourceLibraryIndexTask.GetAwaiter().GetResult();
		ApplyBuiltInResourceCandidates(value, result.BuiltInResources);
		HashSet<string> hashSet = new HashSet<string>(value.Select((XWGameplayResourceChoice choice) => choice.Key), StringComparer.OrdinalIgnoreCase);
		foreach (XWEditorResourceFile file in result.Files)
		{
			if (!MatchesEditorResourceFile(file))
			{
				continue;
			}
			XWGameplayResourceChoice xWGameplayResourceChoice = CreateEditorResourceChoice(file.FilePath, file.RootPath, file.IsModResource);
			if (!(xWGameplayResourceChoice == null) && hashSet.Add(xWGameplayResourceChoice.Key))
			{
				value.Add(xWGameplayResourceChoice);
				if (IsDirectImagePath(file.FilePath))
				{
					_thumbnailPaths[ChoiceId(xWGameplayResourceChoice)] = xWGameplayResourceChoice.ResourcePath;
				}
			}
		}
		value.Sort((XWGameplayResourceChoice left, XWGameplayResourceChoice right) => string.Compare(left.Key, right.Key, StringComparison.OrdinalIgnoreCase));
		RebuildChoiceRows();
		SelectCurrentKey();
		if (_selectedChoice == null && value.Count > 0)
		{
			ClearPreview();
		}
		UpdateProcessState();
	}

	private void ApplyBuiltInResourceCandidates(List<XWGameplayResourceChoice> choices, IReadOnlyList<XWEditorBuiltInResourceCandidate> candidates)
	{
		HashSet<string> hashSet = new HashSet<string>(choices.Select((XWGameplayResourceChoice xWGameplayResourceChoice) => xWGameplayResourceChoice.ResourcePath), StringComparer.OrdinalIgnoreCase);
		foreach (XWEditorBuiltInResourceCandidate candidate in candidates)
		{
			string text = ResolveUidPath(candidate.ResourcePath);
			string text2 = NormalizeResourcePath(string.IsNullOrWhiteSpace(text) ? candidate.ResourcePath : text);
			if (!string.IsNullOrWhiteSpace(text2) && hashSet.Add(text2))
			{
				XWGameplayResourceChoice choice = new XWGameplayResourceChoice(text2, candidate.DisplayName, text2, null, XWGameplayResourceKind.Resource, IsModResource: false);
				AddChoice(choices, choice, candidate.ThumbnailPath);
			}
		}
	}

	private void CancelResourceLibraryIndex()
	{
		_resourceLibraryIndexGeneration++;
		_resourceLibraryIndexStartFrames = 0;
		_resourceLibraryIndexTaskGeneration = 0L;
		if (_resourceLibraryIndexCancellation != null)
		{
			_resourceLibraryIndexCancellation.Cancel();
			_resourceLibraryIndexCancellation.Dispose();
		}
		_resourceLibraryIndexCancellation = null;
		_resourceLibraryIndexTask = null;
		UpdateProcessState();
	}

	private List<XWEditorResourceIndexRoot> BuildEditorResourceIndexRoots()
	{
		List<XWEditorResourceIndexRoot> list = new List<XWEditorResourceIndexRoot>();
		HashSet<string> indexedRoots = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		string text = XWModProjectLayout.GetResourceFolder(_resourceLibraryCategory);
		if (string.Equals(_resourceLibraryCategory, "Audio", StringComparison.OrdinalIgnoreCase))
		{
			text = "Assets/Audio";
		}
		if (!string.IsNullOrWhiteSpace(_resourceLibraryProjectPath))
		{
			AddEditorResourceIndexRoot(list, indexedRoots, Path.Combine(_resourceLibraryProjectPath, text.Replace('/', Path.DirectorySeparatorChar)), isModResource: true);
		}
		foreach (string item in _resourceLibraryBuiltInPathMarkers ?? System.Array.Empty<string>())
		{
			string text2 = (item ?? "").Replace('\\', '/').Trim('/');
			if (!string.IsNullOrWhiteSpace(text2) && !Path.IsPathRooted(text2))
			{
				AddEditorResourceIndexRoot(list, indexedRoots, ProjectSettings.GlobalizePath("res://" + text2), isModResource: false);
			}
		}
		return list;
	}

	private static void AddEditorResourceIndexRoot(List<XWEditorResourceIndexRoot> roots, HashSet<string> indexedRoots, string rootPath, bool isModResource)
	{
		if (!string.IsNullOrWhiteSpace(rootPath) && Directory.Exists(rootPath))
		{
			string fullPath = Path.GetFullPath(rootPath);
			if (indexedRoots.Add(fullPath))
			{
				roots.Add(new XWEditorResourceIndexRoot(fullPath, isModResource));
			}
		}
	}

	private List<XWEditorBuiltInResourceSeed> BuildBuiltInResourceSeeds()
	{
		List<XWEditorBuiltInResourceSeed> list = new List<XWEditorBuiltInResourceSeed>();
		bool flag = IsCardResourceLibrary();
		bool flag2 = IsCharacterResourceLibrary();
		if (!flag && !flag2)
		{
			return list;
		}
		Dictionary dictionary = LoadRegistryDictionary("res://Asset/Config/Character/CharacterResource.json");
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		foreach (Variant key in dictionary.Keys)
		{
			if (dictionary[key].VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary2 = dictionary[key].AsGodotDictionary();
			string thumbnailPath = ReadString(dictionary2, "Sprite");
			Variant valueOrDefault = dictionary2.GetValueOrDefault("Packet");
			if (valueOrDefault.VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary3 = valueOrDefault.AsGodotDictionary();
			foreach (Variant key2 in dictionary3.Keys)
			{
				string value = key2.AsString();
				string text = dictionary3[key2].AsString();
				string text2 = ResolveUidPath(text);
				string text3 = NormalizeResourcePath(string.IsNullOrWhiteSpace(text2) ? text : text2);
				if (string.IsNullOrWhiteSpace(text3))
				{
					continue;
				}
				if (flag)
				{
					if (hashSet.Add(text3))
					{
						list.Add(new XWEditorBuiltInResourceSeed(Humanize(value), text3, "", "", thumbnailPath));
					}
					continue;
				}
				string text4 = (text3.StartsWith("res://", StringComparison.OrdinalIgnoreCase) ? ProjectSettings.GlobalizePath(text3) : text3);
				if (hashSet.Add(text4))
				{
					list.Add(new XWEditorBuiltInResourceSeed(Humanize(value), "", text4, "characterConfig", thumbnailPath));
				}
			}
		}
		return list;
	}

	private static XWEditorResourceIndexResult IndexResourceLibrary(IReadOnlyList<XWEditorResourceIndexRoot> roots, bool readResourceType, IReadOnlyList<XWEditorBuiltInResourceSeed> builtInSeeds, CancellationToken cancellationToken)
	{
		List<XWEditorBuiltInResourceCandidate> list = new List<XWEditorBuiltInResourceCandidate>();
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		foreach (XWEditorBuiltInResourceSeed builtInSeed in builtInSeeds)
		{
			cancellationToken.ThrowIfCancellationRequested();
			string path = (string.IsNullOrWhiteSpace(builtInSeed.SourceFilePath) ? builtInSeed.ResourcePath : ReadTextResourceReferenceFromFile(builtInSeed.SourceFilePath, builtInSeed.ReferenceProperty));
			path = NormalizeResourcePath(path);
			if (!string.IsNullOrWhiteSpace(path) && hashSet.Add(path))
			{
				list.Add(new XWEditorBuiltInResourceCandidate(builtInSeed.DisplayName, path, builtInSeed.ThumbnailPath));
			}
		}
		return new XWEditorResourceIndexResult(IndexEditorResourceRoots(roots, readResourceType, cancellationToken), list);
	}

	private static List<XWEditorResourceFile> IndexEditorResourceRoots(IReadOnlyList<XWEditorResourceIndexRoot> roots, bool readResourceType, CancellationToken cancellationToken)
	{
		List<XWEditorResourceFile> list = new List<XWEditorResourceFile>();
		foreach (XWEditorResourceIndexRoot root in roots)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				foreach (string item in Directory.EnumerateFiles(root.RootPath, "*.*", SearchOption.AllDirectories))
				{
					cancellationToken.ThrowIfCancellationRequested();
					if (IsEditorResourceExtension(item))
					{
						list.Add(new XWEditorResourceFile(item, root.RootPath, root.IsModResource, readResourceType ? ReadResourceTypeMarker(item) : ""));
					}
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}
		return list;
	}

	private bool MatchesEditorResourceFile(XWEditorResourceFile indexedFile)
	{
		if (_resourceLibraryClassNames == null || _resourceLibraryClassNames.Count == 0)
		{
			return true;
		}
		if (!(Path.GetExtension(indexedFile.FilePath).ToLowerInvariant() == ".res") || !string.IsNullOrWhiteSpace(indexedFile.ResourceType))
		{
			return MatchesResourceClass(indexedFile.ResourceType, _resourceLibraryClassNames);
		}
		return true;
	}

	private void LoadEditorResourceChoices(List<XWGameplayResourceChoice> choices)
	{
		HashSet<string> indexedRoots = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		string resourceLibraryCategory = _resourceLibraryCategory;
		string text = XWModProjectLayout.GetResourceFolder(resourceLibraryCategory);
		if (string.Equals(resourceLibraryCategory, "Audio", StringComparison.OrdinalIgnoreCase))
		{
			text = "Assets/Audio";
		}
		if (!string.IsNullOrWhiteSpace(_resourceLibraryProjectPath))
		{
			string rootPath = Path.Combine(_resourceLibraryProjectPath, text.Replace('/', Path.DirectorySeparatorChar));
			IndexEditorResourceRoot(choices, rootPath, isModResource: true, indexedRoots);
		}
		if (IsCardResourceLibrary())
		{
			LoadBuiltInCardEditorResourceChoices(choices);
		}
		else if (IsCharacterResourceLibrary())
		{
			LoadBuiltInCharacterEditorResourceChoices(choices);
		}
		foreach (string item in _resourceLibraryBuiltInPathMarkers ?? System.Array.Empty<string>())
		{
			string text2 = (item ?? "").Replace('\\', '/').Trim('/');
			if (!string.IsNullOrWhiteSpace(text2) && !Path.IsPathRooted(text2))
			{
				string rootPath2 = ProjectSettings.GlobalizePath("res://" + text2);
				IndexEditorResourceRoot(choices, rootPath2, isModResource: false, indexedRoots);
			}
		}
	}

	private bool IsCardResourceLibrary()
	{
		return _resourceLibraryClassNames.Any((string className) => string.Equals(className, "TowerDefensePacketConfig", StringComparison.OrdinalIgnoreCase));
	}

	private bool IsCharacterResourceLibrary()
	{
		return _resourceLibraryClassNames.Any((string className) => string.Equals(className, "TowerDefenseCharacterConfig", StringComparison.OrdinalIgnoreCase));
	}

	private void LoadBuiltInCardEditorResourceChoices(List<XWGameplayResourceChoice> choices)
	{
		Dictionary dictionary = LoadRegistryDictionary("res://Asset/Config/Character/CharacterResource.json");
		foreach (Variant key in dictionary.Keys)
		{
			if (dictionary[key].VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary2 = dictionary[key].AsGodotDictionary();
			string thumbnailPath = ReadString(dictionary2, "Sprite");
			Variant valueOrDefault = dictionary2.GetValueOrDefault("Packet");
			if (valueOrDefault.VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary3 = valueOrDefault.AsGodotDictionary();
			foreach (Variant key2 in dictionary3.Keys)
			{
				string value = key2.AsString();
				string text = dictionary3[key2].AsString();
				string text2 = ResolveUidPath(text);
				string resourcePath = NormalizeResourcePath(string.IsNullOrWhiteSpace(text2) ? text : text2);
				if (!string.IsNullOrWhiteSpace(resourcePath) && !choices.Any((XWGameplayResourceChoice existing) => string.Equals(existing.ResourcePath, resourcePath, StringComparison.OrdinalIgnoreCase)))
				{
					XWGameplayResourceChoice choice = new XWGameplayResourceChoice(resourcePath, Humanize(value), resourcePath, null, XWGameplayResourceKind.Resource, IsModResource: false);
					AddChoice(choices, choice, thumbnailPath);
				}
			}
		}
	}

	private void LoadBuiltInCharacterEditorResourceChoices(List<XWGameplayResourceChoice> choices)
	{
		Dictionary dictionary = LoadRegistryDictionary("res://Asset/Config/Character/CharacterResource.json");
		foreach (Variant key in dictionary.Keys)
		{
			if (dictionary[key].VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary2 = dictionary[key].AsGodotDictionary();
			string thumbnailPath = ReadString(dictionary2, "Sprite");
			Variant valueOrDefault = dictionary2.GetValueOrDefault("Packet");
			if (valueOrDefault.VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary3 = valueOrDefault.AsGodotDictionary();
			foreach (Variant key2 in dictionary3.Keys)
			{
				string value = key2.AsString();
				string resourcePath = ResolveUidPath(dictionary3[key2].AsString());
				string characterPath = NormalizeResourcePath(ReadTextResourceReference(resourcePath, "characterConfig"));
				if (!string.IsNullOrWhiteSpace(characterPath) && !choices.Any((XWGameplayResourceChoice existing) => string.Equals(existing.ResourcePath, characterPath, StringComparison.OrdinalIgnoreCase)))
				{
					string displayName = Humanize(value);
					XWGameplayResourceChoice choice = new XWGameplayResourceChoice(characterPath, displayName, characterPath, null, XWGameplayResourceKind.Resource, IsModResource: false);
					AddChoice(choices, choice, thumbnailPath);
				}
			}
		}
	}

	private static string ReadTextResourceReference(string resourcePath, string propertyName)
	{
		resourcePath = ResolveUidPath(resourcePath);
		if (string.IsNullOrWhiteSpace(resourcePath) || string.IsNullOrWhiteSpace(propertyName))
		{
			return "";
		}
		return ReadTextResourceReferenceFromFile(resourcePath.StartsWith("res://", StringComparison.OrdinalIgnoreCase) ? ProjectSettings.GlobalizePath(resourcePath) : resourcePath, propertyName);
	}

	private static string ReadTextResourceReferenceFromFile(string filePath, string propertyName)
	{
		if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(propertyName) || !string.Equals(Path.GetExtension(filePath), ".tres", StringComparison.OrdinalIgnoreCase) || !File.Exists(filePath))
		{
			return "";
		}
		try
		{
			string text = propertyName + " = ExtResource(\"";
			string text2 = "";
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>(StringComparer.Ordinal);
			foreach (string item in File.ReadLines(filePath))
			{
				string text3 = item.Trim();
				if (text3.StartsWith(text, StringComparison.Ordinal))
				{
					int length = text.Length;
					int num = text3.IndexOf("\")", length, StringComparison.Ordinal);
					if (num > length)
					{
						int num2 = length;
						text2 = text3.Substring(num2, num - num2);
					}
				}
				else
				{
					if (!text3.StartsWith("[ext_resource ", StringComparison.Ordinal))
					{
						continue;
					}
					int num3 = text3.IndexOf("path=\"", StringComparison.Ordinal);
					int num4 = text3.IndexOf(" id=\"", StringComparison.Ordinal);
					if (num3 >= 0 && num4 >= 0)
					{
						num3 += "path=\"".Length;
						num4 += " id=\"".Length;
						int num5 = text3.IndexOf('"', num3);
						int num6 = text3.IndexOf('"', num4);
						if (num5 > num3 && num6 > num4)
						{
							int num2 = num4;
							string key = text3.Substring(num2, num6 - num2);
							num2 = num3;
							dictionary[key] = text3.Substring(num2, num5 - num2);
						}
					}
				}
			}
			string value;
			return (!string.IsNullOrWhiteSpace(text2) && dictionary.TryGetValue(text2, out value)) ? value : "";
		}
		catch (IOException)
		{
			return "";
		}
		catch (UnauthorizedAccessException)
		{
			return "";
		}
	}

	private void IndexEditorResourceRoot(List<XWGameplayResourceChoice> choices, string rootPath, bool isModResource, HashSet<string> indexedRoots)
	{
		if (string.IsNullOrWhiteSpace(rootPath) || !Directory.Exists(rootPath))
		{
			return;
		}
		string fullPath = Path.GetFullPath(rootPath);
		if (!indexedRoots.Add(fullPath))
		{
			return;
		}
		try
		{
			foreach (string item in Directory.EnumerateFiles(rootPath, "*.*", SearchOption.AllDirectories))
			{
				if (!IsEditorResourceFile(item, _resourceLibraryClassNames))
				{
					continue;
				}
				XWGameplayResourceChoice choice = CreateEditorResourceChoice(item, fullPath, isModResource);
				if (choice != null && !choices.Any((XWGameplayResourceChoice existing) => string.Equals(existing.Key, choice.Key, StringComparison.OrdinalIgnoreCase)))
				{
					choices.Add(choice);
					if (IsDirectImagePath(item))
					{
						_thumbnailPaths[ChoiceId(choice)] = choice.ResourcePath;
					}
				}
			}
		}
		catch (IOException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
	}

	private static bool IsEditorResourceFile(string filePath, IReadOnlyList<string> resourceClassNames)
	{
		if (!IsEditorResourceExtension(filePath))
		{
			return false;
		}
		string text = Path.GetExtension(filePath).ToLowerInvariant();
		if (resourceClassNames == null || resourceClassNames.Count == 0)
		{
			return true;
		}
		string text2 = ReadResourceTypeMarker(filePath);
		if (!(text == ".res") || !string.IsNullOrWhiteSpace(text2))
		{
			return MatchesResourceClass(text2, resourceClassNames);
		}
		return true;
	}

	private static bool IsEditorResourceExtension(string filePath)
	{
		switch (Path.GetExtension(filePath).ToLowerInvariant())
		{
		case ".tres":
		case ".tscn":
		case ".flac":
		case ".jpeg":
		case ".webp":
		case ".woff":
		case ".res":
		case ".scn":
		case ".svg":
		case ".wav":
		case ".ogg":
		case ".otf":
		case ".mp3":
		case ".png":
		case ".jpg":
		case ".bmp":
		case ".tga":
		case ".ttf":
		case ".woff2":
		case ".gdshader":
			return true;
		default:
			return false;
		}
	}

	private static string ReadResourceTypeMarker(string filePath)
	{
		string text = Path.GetExtension(filePath).ToLowerInvariant();
		if (text != null)
		{
			int length = text.Length;
			if (length != 4)
			{
				if (length == 5)
				{
					char c = text[1];
					if (c != 'j')
					{
						if (c == 'w' && text == ".webp")
						{
							goto IL_00d2;
						}
					}
					else if (text == ".jpeg")
					{
						goto IL_00d2;
					}
				}
			}
			else
			{
				switch (text[1])
				{
				case 'p':
					break;
				case 'j':
					goto IL_007a;
				case 's':
					goto IL_0089;
				case 'b':
					goto IL_0098;
				case 't':
					goto IL_00a7;
				default:
					goto IL_00d6;
				}
				if (text == ".png")
				{
					goto IL_00d2;
				}
			}
		}
		goto IL_00d6;
		IL_00d6:
		bool flag = false;
		goto IL_00d8;
		IL_00d2:
		flag = true;
		goto IL_00d8;
		IL_00d8:
		if (flag)
		{
			return "Texture2D";
		}
		switch (text)
		{
		case ".wav":
			return "AudioStreamWav";
		case ".ogg":
			return "AudioStreamOggVorbis";
		case ".mp3":
			return "AudioStreamMP3";
		case ".flac":
			return "AudioStreamWav";
		case ".ttf":
		case ".otf":
		case ".woff":
		case ".woff2":
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			return "FontFile";
		}
		switch (text)
		{
		case ".gdshader":
			return "Shader";
		case ".res":
		case ".scn":
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			return "";
		}
		try
		{
			string text2 = File.ReadLines(filePath).FirstOrDefault() ?? "";
			int num = text2.IndexOf("script_class=\"", StringComparison.Ordinal);
			if (num >= 0)
			{
				num += "script_class=\"".Length;
				int num2 = text2.IndexOf('"', num);
				if (num2 > num)
				{
					int length = num;
					return text2.Substring(length, num2 - length);
				}
			}
			int num3 = text2.IndexOf("type=\"", StringComparison.Ordinal);
			if (num3 < 0)
			{
				return "";
			}
			num3 += "type=\"".Length;
			int num4 = text2.IndexOf('"', num3);
			string result;
			if (num4 <= num3)
			{
				result = "";
			}
			else
			{
				int length = num3;
				result = text2.Substring(length, num4 - length);
			}
			return result;
		}
		catch (IOException)
		{
			return "";
		}
		catch (UnauthorizedAccessException)
		{
			return "";
		}
		IL_007a:
		if (text == ".jpg")
		{
			goto IL_00d2;
		}
		goto IL_00d6;
		IL_0089:
		if (text == ".svg")
		{
			goto IL_00d2;
		}
		goto IL_00d6;
		IL_0098:
		if (text == ".bmp")
		{
			goto IL_00d2;
		}
		goto IL_00d6;
		IL_00a7:
		if (text == ".tga")
		{
			goto IL_00d2;
		}
		goto IL_00d6;
	}

	private static bool MatchesResourceClass(string resourceType, IReadOnlyList<string> resourceClassNames)
	{
		if (string.IsNullOrWhiteSpace(resourceType))
		{
			return false;
		}
		EnsureGlobalResourceClassBases();
		foreach (string resourceClassName in resourceClassNames)
		{
			if (string.Equals(resourceType, resourceClassName, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (ClassDB.ClassExists(resourceType) && ClassDB.ClassExists(resourceClassName) && ClassDB.IsParentClass(resourceType, resourceClassName))
			{
				return true;
			}
			string text = resourceType;
			for (int i = 0; i < 64; i++)
			{
				if (!GlobalResourceClassBases.TryGetValue(text, out var value))
				{
					break;
				}
				if (string.IsNullOrWhiteSpace(value))
				{
					break;
				}
				if (string.Equals(value, text, StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
				if (string.Equals(value, resourceClassName, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				text = value;
			}
		}
		return false;
	}

	private static void EnsureGlobalResourceClassBases()
	{
		if (_globalResourceClassBasesLoaded)
		{
			return;
		}
		_globalResourceClassBasesLoaded = true;
		foreach (Dictionary globalClass in ProjectSettings.GetGlobalClassList())
		{
			if (globalClass.ContainsKey("class") && globalClass.ContainsKey("base"))
			{
				string text = (string)globalClass["class"];
				string text2 = (string)globalClass["base"];
				if (!string.IsNullOrWhiteSpace(text))
				{
					GlobalResourceClassBases[text] = text2 ?? "";
				}
			}
		}
	}

	private static XWGameplayResourceChoice CreateEditorResourceChoice(string filePath, string rootPath, bool isModResource)
	{
		if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(rootPath))
		{
			return null;
		}
		string text = (isModResource ? NormalizeResourcePath(Path.GetFullPath(filePath)) : NormalizeResourcePath(ProjectSettings.LocalizePath(filePath)));
		string displayName = Path.ChangeExtension(Path.GetRelativePath(rootPath, filePath), null)?.Replace('\\', '/') ?? Path.GetFileNameWithoutExtension(filePath);
		XWGameplayResourceKind kind = (IsAudioPath(filePath) ? XWGameplayResourceKind.Audio : XWGameplayResourceKind.Resource);
		return new XWGameplayResourceChoice(text, displayName, text, null, kind, isModResource);
	}

	private static bool IsDirectImagePath(string path)
	{
		string text = Path.GetExtension(path).ToLowerInvariant();
		if (text != null)
		{
			int length = text.Length;
			if (length != 4)
			{
				if (length == 5)
				{
					char c = text[1];
					if (c != 'j')
					{
						if (c == 'w' && text == ".webp")
						{
							goto IL_00d2;
						}
					}
					else if (text == ".jpeg")
					{
						goto IL_00d2;
					}
				}
			}
			else
			{
				switch (text[1])
				{
				case 'p':
					break;
				case 'j':
					goto IL_007a;
				case 's':
					goto IL_0089;
				case 'b':
					goto IL_0098;
				case 't':
					goto IL_00a7;
				default:
					goto IL_00d6;
				}
				if (text == ".png")
				{
					goto IL_00d2;
				}
			}
		}
		goto IL_00d6;
		IL_00a7:
		if (text == ".tga")
		{
			goto IL_00d2;
		}
		goto IL_00d6;
		IL_00d2:
		return true;
		IL_0098:
		if (text == ".bmp")
		{
			goto IL_00d2;
		}
		goto IL_00d6;
		IL_007a:
		if (text == ".jpg")
		{
			goto IL_00d2;
		}
		goto IL_00d6;
		IL_00d6:
		return false;
		IL_0089:
		if (text == ".svg")
		{
			goto IL_00d2;
		}
		goto IL_00d6;
	}

	private static bool IsAudioPath(string path)
	{
		switch (Path.GetExtension(path).ToLowerInvariant())
		{
		case ".wav":
		case ".ogg":
		case ".mp3":
		case ".flac":
			return true;
		default:
			return false;
		}
	}

	private static string NormalizeResourcePath(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return path.Replace('\\', '/').Trim();
		}
		return "";
	}

	private void LoadCharacterChoices(List<XWGameplayResourceChoice> choices, XWGameplayResourceKind kind)
	{
		Dictionary dictionary = LoadRegistryDictionary("res://Asset/Config/Character/CharacterResource.json");
		foreach (Variant key in dictionary.Keys)
		{
			string text = key.AsString();
			if (dictionary[key].VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary2 = dictionary[key].AsGodotDictionary();
			string thumbnailPath = ReadString(dictionary2, "Sprite");
			if (kind == XWGameplayResourceKind.Character)
			{
				string resourcePath = ReadString(dictionary2, "Scene");
				AddChoice(choices, new XWGameplayResourceChoice(text, Humanize(text), resourcePath, null, kind, IsModResource: false), thumbnailPath);
				continue;
			}
			Variant valueOrDefault = dictionary2.GetValueOrDefault("Packet");
			if (valueOrDefault.VariantType != Variant.Type.Dictionary)
			{
				continue;
			}
			Dictionary dictionary3 = valueOrDefault.AsGodotDictionary();
			foreach (Variant key2 in dictionary3.Keys)
			{
				string text2 = key2.AsString();
				AddChoice(choices, new XWGameplayResourceChoice(text2, Humanize(text2), dictionary3[key2].AsString(), null, kind, IsModResource: false), thumbnailPath);
			}
		}
	}

	private void LoadPacketBankChoices(List<XWGameplayResourceChoice> choices)
	{
		foreach (Variant key in LoadRegistryDictionary("res://Asset/Config/PacketBank/PacketBankResource.json").Keys)
		{
			string text = key.AsString();
			AddChoice(choices, new XWGameplayResourceChoice(text, Humanize(text), "res://Asset/Config/PacketBank/PacketBankResource.json", null, XWGameplayResourceKind.PacketBank, IsModResource: false));
		}
	}

	private void LoadDirectRegistry(List<XWGameplayResourceChoice> choices, string registryPath, XWGameplayResourceKind kind)
	{
		Dictionary dictionary = LoadRegistryDictionary(registryPath);
		foreach (Variant key in dictionary.Keys)
		{
			string text = key.AsString();
			string resourcePath = ((dictionary[key].VariantType == Variant.Type.String) ? dictionary[key].AsString() : registryPath);
			AddChoice(choices, new XWGameplayResourceChoice(text, Humanize(text), resourcePath, null, kind, IsModResource: false));
		}
	}

	private void LoadModAudioChoices(List<XWGameplayResourceChoice> choices)
	{
		string text = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		string text2 = Path.Combine(text, "Assets", "Audio");
		if (!Directory.Exists(text2))
		{
			return;
		}
		foreach (string item in Directory.EnumerateFiles(text2, "*.*", SearchOption.AllDirectories))
		{
			bool flag;
			switch (Path.GetExtension(item).ToLowerInvariant())
			{
			case ".wav":
			case ".ogg":
			case ".mp3":
			case ".flac":
				flag = true;
				break;
			default:
				flag = false;
				break;
			}
			if (flag)
			{
				string key = Path.GetFileNameWithoutExtension(item);
				string path = Path.GetRelativePath(text2, item).Replace('\\', '/');
				XWGameplayResourceChoice xWGameplayResourceChoice = new XWGameplayResourceChoice(key, Path.ChangeExtension(path, null)?.Replace('\\', '/') ?? key, item.Replace('\\', '/'), null, XWGameplayResourceKind.Audio, IsModResource: true);
				int num = choices.FindIndex((XWGameplayResourceChoice existing) => string.Equals(existing.Key, key, StringComparison.OrdinalIgnoreCase));
				if (num >= 0)
				{
					choices[num] = xWGameplayResourceChoice;
				}
				else
				{
					AddChoice(choices, xWGameplayResourceChoice);
				}
			}
		}
	}

	private static void LoadRuntimeAudioChoices(List<XWGameplayResourceChoice> choices)
	{
		if (!GodotObject.IsInstanceValid(ResourceManager.Instance))
		{
			return;
		}
		foreach (KeyValuePair<string, Resource> aUDIO in ResourceManager.Instance.AUDIOS)
		{
			var (key, resource2) = aUDIO;
			if (!string.IsNullOrWhiteSpace(key) && resource2 is AudioStream)
			{
				string resourcePath = resource2.ResourcePath;
				XWGameplayResourceChoice xWGameplayResourceChoice = new XWGameplayResourceChoice(key, Humanize(key), resourcePath, null, XWGameplayResourceKind.Audio, IsModResource: false);
				int num = choices.FindIndex((XWGameplayResourceChoice existing) => string.Equals(existing.Key, key, StringComparison.OrdinalIgnoreCase));
				if (num >= 0)
				{
					choices[num] = xWGameplayResourceChoice;
				}
				else
				{
					choices.Add(xWGameplayResourceChoice);
				}
			}
		}
	}

	private void LoadLevelChoices(List<XWGameplayResourceChoice> choices)
	{
		Dictionary dictionary = LoadRegistryDictionary("res://Asset/Config/Level/LevelResource.json");
		CollectLevelChoices(dictionary, choices, "");
	}

	private void CollectLevelChoices(Variant node, List<XWGameplayResourceChoice> choices, string groupName)
	{
		if (node.VariantType == Variant.Type.Array)
		{
			foreach (Variant item in node.AsGodotArray())
			{
				CollectLevelChoices(item, choices, groupName);
			}
			return;
		}
		if (node.VariantType != Variant.Type.Dictionary)
		{
			return;
		}
		Dictionary dictionary = node.AsGodotDictionary();
		string text = ReadString(dictionary, "SaveKey");
		Variant valueOrDefault = dictionary.GetValueOrDefault("Level");
		if (!string.IsNullOrWhiteSpace(text) && valueOrDefault.VariantType == Variant.Type.Dictionary)
		{
			Dictionary dictionary2 = valueOrDefault.AsGodotDictionary();
			foreach (Variant key2 in dictionary2.Keys)
			{
				string text2 = dictionary2[key2].AsString();
				if (!string.IsNullOrWhiteSpace(text2))
				{
					string text3 = key2.AsString();
					string key = (string.Equals(text3, "Normal", StringComparison.OrdinalIgnoreCase) ? text : (text + ":" + text3));
					string displayName = (string.IsNullOrWhiteSpace(groupName) ? (text + " · " + text3) : $"{groupName} / {text} · {text3}");
					AddChoice(choices, new XWGameplayResourceChoice(key, displayName, text2, null, XWGameplayResourceKind.Level, IsModResource: false));
				}
			}
		}
		string text4 = ReadString(dictionary, "Name");
		if (string.IsNullOrWhiteSpace(text4))
		{
			text4 = groupName;
		}
		bool flag = !string.IsNullOrWhiteSpace(text) && valueOrDefault.VariantType == Variant.Type.Dictionary;
		foreach (Variant key3 in dictionary.Keys)
		{
			string text5 = key3.AsString();
			if (!(text5 == "SaveKey") && !((text5 == "Level") & flag))
			{
				CollectLevelChoices(dictionary[key3], choices, text4);
			}
		}
	}

	private static Dictionary LoadRegistryDictionary(string registryPath)
	{
		if (!ResourceLoader.Exists(registryPath))
		{
			return new Dictionary();
		}
		Json json = ResourceLoader.Load<Json>(registryPath, "", ResourceLoader.CacheMode.Ignore);
		if (!GodotObject.IsInstanceValid(json) || json.Data.VariantType != Variant.Type.Dictionary)
		{
			return new Dictionary();
		}
		return json.Data.AsGodotDictionary();
	}

	private void AddChoice(List<XWGameplayResourceChoice> choices, XWGameplayResourceChoice choice, string thumbnailPath = "")
	{
		if (!(choice == null) && !string.IsNullOrWhiteSpace(choice.Key))
		{
			choices.Add(choice);
			if (!string.IsNullOrWhiteSpace(thumbnailPath))
			{
				_thumbnailPaths[ChoiceId(choice)] = thumbnailPath;
			}
		}
	}

	private XWGameplayResourceChoice EnsureThumbnail(XWGameplayResourceChoice choice)
	{
		if (choice == null || GodotObject.IsInstanceValid(choice.Thumbnail))
		{
			return choice;
		}
		string text = ChoiceId(choice);
		if (_loadedThumbnails.TryGetValue(text, out var value) && GodotObject.IsInstanceValid(value))
		{
			return choice with
			{
				Thumbnail = value
			};
		}
		if (_missingThumbnails.Contains(text))
		{
			return choice;
		}
		_thumbnailPaths.TryGetValue(text, out var value2);
		if (string.IsNullOrWhiteSpace(value2) && choice.Kind == XWGameplayResourceKind.Projectile)
		{
			value2 = choice.ResourcePath;
		}
		Texture2D texture2D = LoadPreviewTexture(value2);
		if (!GodotObject.IsInstanceValid(texture2D))
		{
			_missingThumbnails.Add(text);
			return choice;
		}
		_loadedThumbnails[text] = texture2D;
		return choice with
		{
			Thumbnail = texture2D
		};
	}

	private static Texture2D LoadPreviewTexture(string path)
	{
		path = ResolveUidPath(path);
		if (string.IsNullOrWhiteSpace(path))
		{
			return null;
		}
		if (!ResourceLoader.Exists(path))
		{
			return LoadExternalImagePreview(path);
		}
		Resource resource = ResourceLoader.Load<Resource>(path, "", ResourceLoader.CacheMode.Ignore);
		if (resource is Texture2D result)
		{
			return result;
		}
		if (resource is TowerDefenseProjectileData towerDefenseProjectileData)
		{
			Texture2D texture2D = FindPackedScenePreviewTexture(towerDefenseProjectileData.projectileScene);
			if (GodotObject.IsInstanceValid(texture2D))
			{
				return texture2D;
			}
		}
		if (resource is TowerDefenseProjectileConfig towerDefenseProjectileConfig)
		{
			Texture2D texture2D2 = FindPackedScenePreviewTexture(towerDefenseProjectileConfig.projectileScene);
			if (GodotObject.IsInstanceValid(texture2D2))
			{
				return texture2D2;
			}
		}
		string[] array = new string[6] { "previewTexture", "texture", "mapTexture", "icon", "image", "sprite" };
		foreach (string text in array)
		{
			Variant variant = resource?.Get(text) ?? default(Variant);
			if (variant.VariantType == Variant.Type.Object && variant.AsGodotObject() is Texture2D result2)
			{
				return result2;
			}
		}
		return null;
	}

	private static Texture2D LoadExternalImagePreview(string path)
	{
		if (!IsDirectImagePath(path))
		{
			return null;
		}
		string path2 = (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase) ? ProjectSettings.GlobalizePath(path) : path);
		if (!File.Exists(path2))
		{
			return null;
		}
		Image image = new Image();
		Error error;
		try
		{
			error = (string.Equals(Path.GetExtension(path2), ".svg", StringComparison.OrdinalIgnoreCase) ? image.LoadSvgFromBuffer(File.ReadAllBytes(path2)) : image.Load(path2));
		}
		catch (IOException)
		{
			return null;
		}
		catch (UnauthorizedAccessException)
		{
			return null;
		}
		if (error != Error.Ok || image.GetWidth() <= 0 || image.GetHeight() <= 0)
		{
			return null;
		}
		return ImageTexture.CreateFromImage(image);
	}

	private static Texture2D FindPackedScenePreviewTexture(PackedScene scene)
	{
		if (!GodotObject.IsInstanceValid(scene))
		{
			return null;
		}
		SceneState state = scene.GetState();
		if (!GodotObject.IsInstanceValid(state))
		{
			return null;
		}
		for (int i = 0; i < state.GetNodeCount(); i++)
		{
			for (int j = 0; j < state.GetNodePropertyCount(i); j++)
			{
				Variant nodePropertyValue = state.GetNodePropertyValue(i, j);
				if (nodePropertyValue.VariantType == Variant.Type.Object && nodePropertyValue.AsGodotObject() is Texture2D texture2D && XWTextureSafety.CanPreview(texture2D))
				{
					return texture2D;
				}
			}
		}
		return null;
	}

	private Texture2D GetFallbackIcon(XWGameplayResourceKind kind)
	{
		if (_fallbackIcons.TryGetValue(kind, out var value) && GodotObject.IsInstanceValid(value))
		{
			return value;
		}
		if (kind == XWGameplayResourceKind.Resource && !string.IsNullOrWhiteSpace(_resourceLibraryIconPath) && ResourceLoader.Exists(_resourceLibraryIconPath))
		{
			Texture2D texture2D = ResourceLoader.Load<Texture2D>(_resourceLibraryIconPath, "", ResourceLoader.CacheMode.Reuse);
			_fallbackIcons[kind] = texture2D;
			return texture2D;
		}
		Texture2D texture2D2 = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/" + kind switch
		{
			XWGameplayResourceKind.Resource => "ResourceGUI.svg", 
			XWGameplayResourceKind.Character => "ResourceCharacter.svg", 
			XWGameplayResourceKind.Card => "ResourceCard.svg", 
			XWGameplayResourceKind.Projectile => "ResourceProjectile.svg", 
			XWGameplayResourceKind.PacketBank => "ResourcePacketBank.svg", 
			XWGameplayResourceKind.Map => "ResourceMap.svg", 
			XWGameplayResourceKind.Bgm => "ResourceBGM.svg", 
			XWGameplayResourceKind.Level => "ResourceLevel.svg", 
			_ => "ResourceAudio.svg", 
		}, "", ResourceLoader.CacheMode.Reuse);
		_fallbackIcons[kind] = texture2D2;
		return texture2D2;
	}

	private static string ResolveUidPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path) || !path.StartsWith("uid://", StringComparison.Ordinal))
		{
			return path;
		}
		long num = ResourceUid.TextToId(path);
		if (num == -1 || !ResourceUid.HasId(num))
		{
			return "";
		}
		return ResourceUid.GetIdPath(num);
	}

	private static string ReadString(Dictionary dictionary, string key)
	{
		Variant variant = dictionary?.GetValueOrDefault(key) ?? default(Variant);
		if (variant.VariantType != Variant.Type.String && variant.VariantType != Variant.Type.StringName)
		{
			return "";
		}
		return variant.AsString();
	}

	private static string ChoiceId(XWGameplayResourceChoice choice)
	{
		return $"{choice.Kind}:{choice.Key}";
	}

	private static string KindGlyph(XWGameplayResourceKind kind)
	{
		return kind switch
		{
			XWGameplayResourceKind.Resource => "资源", 
			XWGameplayResourceKind.Character => "角色", 
			XWGameplayResourceKind.Card => "卡片", 
			XWGameplayResourceKind.Projectile => "子弹", 
			XWGameplayResourceKind.PacketBank => "卡组", 
			XWGameplayResourceKind.Map => "地图", 
			XWGameplayResourceKind.Bgm => "音乐", 
			XWGameplayResourceKind.Level => "关卡", 
			_ => "音效", 
		};
	}

	private static string Humanize(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return "未命名资源";
		}
		StringBuilder stringBuilder = new StringBuilder(value.Length + 8);
		for (int i = 0; i < value.Length; i++)
		{
			char c = value[i];
			if (i > 0 && char.IsUpper(c) && !char.IsUpper(value[i - 1]))
			{
				stringBuilder.Append(' ');
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(49)
		{
			new MethodInfo(MethodName.Create, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Window"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetResourceLibraryChrome, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "resourceLibraryMode", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureCatalogChrome, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "lockKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "catalogTitle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryConfirmIndexedResource, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Dismiss, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HandlePickerVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindCategoryButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.NodePath, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PopupResponsive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetAvailableHostSize, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetActiveCategory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildChoiceRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectChoiceRow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectCurrentKey, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviewSelectedAudio, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopSelectedAudio, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnAudioPreviewFinished, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StopAudioPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "clearStream", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateProcessState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadAudioStream, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AudioStream"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "audioKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfirmSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatAudioTime, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "seconds", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScheduleResourceLibraryIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.TryBeginResourceLibraryIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginResourceLibraryIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PollResourceLibraryIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CancelResourceLibraryIndex, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsCardResourceLibrary, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.IsCharacterResourceLibrary, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadTextResourceReference, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "resourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadTextResourceReferenceFromFile, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsEditorResourceExtension, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadResourceTypeMarker, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureGlobalResourceClassBases, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.IsDirectImagePath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsAudioPath, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadRegistryDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "registryPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadPreviewTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadExternalImagePreview, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FindPackedScenePreviewTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetFallbackIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveUidPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dictionary", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.KindGlyph, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.Humanize, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWGameplayResourcePickerWindow>(Create());
			return true;
		}
		if (method == MethodName._Ready && args.Count == 0)
		{
			_Ready();
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.SetResourceLibraryChrome && args.Count == 1)
		{
			SetResourceLibraryChrome(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureCatalogChrome && args.Count == 3)
		{
			ConfigureCatalogChrome(VariantUtils.ConvertTo<XWGameplayResourceKind>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryConfirmIndexedResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(TryConfirmIndexedResource(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.Dismiss && args.Count == 0)
		{
			Dismiss();
			ret = default;
			return true;
		}
		if (method == MethodName.HandlePickerVisibilityChanged && args.Count == 0)
		{
			HandlePickerVisibilityChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.BindCategoryButton && args.Count == 2)
		{
			BindCategoryButton(VariantUtils.ConvertTo<NodePath>(in args[0]), VariantUtils.ConvertTo<XWGameplayResourceKind>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopupResponsive && args.Count == 0)
		{
			PopupResponsive();
			ret = default;
			return true;
		}
		if (method == MethodName.GetAvailableHostSize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetAvailableHostSize());
			return true;
		}
		if (method == MethodName.SetActiveCategory && args.Count == 1)
		{
			SetActiveCategory(VariantUtils.ConvertTo<XWGameplayResourceKind>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildChoiceRows && args.Count == 0)
		{
			RebuildChoiceRows();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectChoiceRow && args.Count == 0)
		{
			SelectChoiceRow();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectCurrentKey && args.Count == 0)
		{
			SelectCurrentKey();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewSelectedAudio && args.Count == 0)
		{
			PreviewSelectedAudio();
			ret = default;
			return true;
		}
		if (method == MethodName.StopSelectedAudio && args.Count == 0)
		{
			StopSelectedAudio();
			ret = default;
			return true;
		}
		if (method == MethodName.OnAudioPreviewFinished && args.Count == 0)
		{
			OnAudioPreviewFinished();
			ret = default;
			return true;
		}
		if (method == MethodName.StopAudioPreview && args.Count == 1)
		{
			StopAudioPreview(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateProcessState && args.Count == 0)
		{
			UpdateProcessState();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadAudioStream && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AudioStream>(LoadAudioStream(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ConfirmSelection && args.Count == 0)
		{
			ConfirmSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearPreview && args.Count == 1)
		{
			ClearPreview(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FormatAudioTime && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatAudioTime(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.ScheduleResourceLibraryIndex && args.Count == 0)
		{
			ScheduleResourceLibraryIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.TryBeginResourceLibraryIndex && args.Count == 0)
		{
			TryBeginResourceLibraryIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginResourceLibraryIndex && args.Count == 0)
		{
			BeginResourceLibraryIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.PollResourceLibraryIndex && args.Count == 0)
		{
			PollResourceLibraryIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.CancelResourceLibraryIndex && args.Count == 0)
		{
			CancelResourceLibraryIndex();
			ret = default;
			return true;
		}
		if (method == MethodName.IsCardResourceLibrary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCardResourceLibrary());
			return true;
		}
		if (method == MethodName.IsCharacterResourceLibrary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(IsCharacterResourceLibrary());
			return true;
		}
		if (method == MethodName.ReadTextResourceReference && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadTextResourceReference(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadTextResourceReferenceFromFile && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadTextResourceReferenceFromFile(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.IsEditorResourceExtension && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEditorResourceExtension(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadResourceTypeMarker && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadResourceTypeMarker(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.EnsureGlobalResourceClassBases && args.Count == 0)
		{
			EnsureGlobalResourceClassBases();
			ret = default;
			return true;
		}
		if (method == MethodName.IsDirectImagePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDirectImagePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAudioPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAudioPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadRegistryDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(LoadRegistryDictionary(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadPreviewTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadPreviewTexture(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadExternalImagePreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadExternalImagePreview(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindPackedScenePreviewTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(FindPackedScenePreviewTexture(VariantUtils.ConvertTo<PackedScene>(in args[0])));
			return true;
		}
		if (method == MethodName.GetFallbackIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetFallbackIcon(VariantUtils.ConvertTo<XWGameplayResourceKind>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveUidPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveUidPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadString && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadString(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.KindGlyph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(KindGlyph(VariantUtils.ConvertTo<XWGameplayResourceKind>(in args[0])));
			return true;
		}
		if (method == MethodName.Humanize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(Humanize(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.Create && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWGameplayResourcePickerWindow>(Create());
			return true;
		}
		if (method == MethodName.FormatAudioTime && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatAudioTime(VariantUtils.ConvertTo<double>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadTextResourceReference && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadTextResourceReference(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadTextResourceReferenceFromFile && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadTextResourceReferenceFromFile(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.IsEditorResourceExtension && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsEditorResourceExtension(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadResourceTypeMarker && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadResourceTypeMarker(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.EnsureGlobalResourceClassBases && args.Count == 0)
		{
			EnsureGlobalResourceClassBases();
			ret = default;
			return true;
		}
		if (method == MethodName.IsDirectImagePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsDirectImagePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsAudioPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsAudioPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.NormalizeResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(NormalizeResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadRegistryDictionary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(LoadRegistryDictionary(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadPreviewTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadPreviewTexture(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadExternalImagePreview && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadExternalImagePreview(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FindPackedScenePreviewTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(FindPackedScenePreviewTexture(VariantUtils.ConvertTo<PackedScene>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveUidPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResolveUidPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadString && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadString(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.KindGlyph && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(KindGlyph(VariantUtils.ConvertTo<XWGameplayResourceKind>(in args[0])));
			return true;
		}
		if (method == MethodName.Humanize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(Humanize(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName.Create)
		{
			return true;
		}
		if (method == MethodName._Ready)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.SetResourceLibraryChrome)
		{
			return true;
		}
		if (method == MethodName.ConfigureCatalogChrome)
		{
			return true;
		}
		if (method == MethodName.TryConfirmIndexedResource)
		{
			return true;
		}
		if (method == MethodName.Dismiss)
		{
			return true;
		}
		if (method == MethodName.HandlePickerVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.BindCategoryButton)
		{
			return true;
		}
		if (method == MethodName.PopupResponsive)
		{
			return true;
		}
		if (method == MethodName.GetAvailableHostSize)
		{
			return true;
		}
		if (method == MethodName.SetActiveCategory)
		{
			return true;
		}
		if (method == MethodName.RebuildChoiceRows)
		{
			return true;
		}
		if (method == MethodName.SelectChoiceRow)
		{
			return true;
		}
		if (method == MethodName.SelectCurrentKey)
		{
			return true;
		}
		if (method == MethodName.PreviewSelectedAudio)
		{
			return true;
		}
		if (method == MethodName.StopSelectedAudio)
		{
			return true;
		}
		if (method == MethodName.OnAudioPreviewFinished)
		{
			return true;
		}
		if (method == MethodName.StopAudioPreview)
		{
			return true;
		}
		if (method == MethodName.UpdateProcessState)
		{
			return true;
		}
		if (method == MethodName.LoadAudioStream)
		{
			return true;
		}
		if (method == MethodName.ConfirmSelection)
		{
			return true;
		}
		if (method == MethodName.ClearPreview)
		{
			return true;
		}
		if (method == MethodName.FormatAudioTime)
		{
			return true;
		}
		if (method == MethodName.ScheduleResourceLibraryIndex)
		{
			return true;
		}
		if (method == MethodName.TryBeginResourceLibraryIndex)
		{
			return true;
		}
		if (method == MethodName.BeginResourceLibraryIndex)
		{
			return true;
		}
		if (method == MethodName.PollResourceLibraryIndex)
		{
			return true;
		}
		if (method == MethodName.CancelResourceLibraryIndex)
		{
			return true;
		}
		if (method == MethodName.IsCardResourceLibrary)
		{
			return true;
		}
		if (method == MethodName.IsCharacterResourceLibrary)
		{
			return true;
		}
		if (method == MethodName.ReadTextResourceReference)
		{
			return true;
		}
		if (method == MethodName.ReadTextResourceReferenceFromFile)
		{
			return true;
		}
		if (method == MethodName.IsEditorResourceExtension)
		{
			return true;
		}
		if (method == MethodName.ReadResourceTypeMarker)
		{
			return true;
		}
		if (method == MethodName.EnsureGlobalResourceClassBases)
		{
			return true;
		}
		if (method == MethodName.IsDirectImagePath)
		{
			return true;
		}
		if (method == MethodName.IsAudioPath)
		{
			return true;
		}
		if (method == MethodName.NormalizeResourcePath)
		{
			return true;
		}
		if (method == MethodName.LoadRegistryDictionary)
		{
			return true;
		}
		if (method == MethodName.LoadPreviewTexture)
		{
			return true;
		}
		if (method == MethodName.LoadExternalImagePreview)
		{
			return true;
		}
		if (method == MethodName.FindPackedScenePreviewTexture)
		{
			return true;
		}
		if (method == MethodName.GetFallbackIcon)
		{
			return true;
		}
		if (method == MethodName.ResolveUidPath)
		{
			return true;
		}
		if (method == MethodName.ReadString)
		{
			return true;
		}
		if (method == MethodName.KindGlyph)
		{
			return true;
		}
		if (method == MethodName.Humanize)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._titleLabel)
		{
			_titleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._subtitleLabel)
		{
			_subtitleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._categoryTabs)
		{
			_categoryTabs = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._searchEdit)
		{
			_searchEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._choiceTree)
		{
			_choiceTree = VariantUtils.ConvertTo<Tree>(in value);
			return true;
		}
		if (name == PropertyName._previewTexture)
		{
			_previewTexture = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._displayNameLabel)
		{
			_displayNameLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._resourceKeyLabel)
		{
			_resourceKeyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._sourceBadge)
		{
			_sourceBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._missingBadge)
		{
			_missingBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._confirmButton)
		{
			_confirmButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._audioPreviewControls)
		{
			_audioPreviewControls = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._audioPreviewButton)
		{
			_audioPreviewButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._audioStopButton)
		{
			_audioStopButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._audioProgress)
		{
			_audioProgress = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._audioTimeLabel)
		{
			_audioTimeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._audioPreviewPlayer)
		{
			_audioPreviewPlayer = VariantUtils.ConvertTo<AudioStreamPlayer>(in value);
			return true;
		}
		if (name == PropertyName._audioPreviewLength)
		{
			_audioPreviewLength = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._activeKind)
		{
			_activeKind = VariantUtils.ConvertTo<XWGameplayResourceKind>(in value);
			return true;
		}
		if (name == PropertyName._currentKey)
		{
			_currentKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._resourceLibraryMode)
		{
			_resourceLibraryMode = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._resourceLibraryCategory)
		{
			_resourceLibraryCategory = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._resourceLibraryDisplayName)
		{
			_resourceLibraryDisplayName = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._resourceLibraryProjectPath)
		{
			_resourceLibraryProjectPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._resourceLibraryIconPath)
		{
			_resourceLibraryIconPath = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._resourceLibraryIndexStartFrames)
		{
			_resourceLibraryIndexStartFrames = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._resourceLibraryIndexGeneration)
		{
			_resourceLibraryIndexGeneration = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		if (name == PropertyName._resourceLibraryIndexTaskGeneration)
		{
			_resourceLibraryIndexTaskGeneration = VariantUtils.ConvertTo<long>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.ResourceLibraryProjectPath)
		{
			value = VariantUtils.CreateFrom<string>(ResourceLibraryProjectPath);
			return true;
		}
		if (name == PropertyName.IsResourceLibraryIndexing)
		{
			value = VariantUtils.CreateFrom<bool>(IsResourceLibraryIndexing);
			return true;
		}
		if (name == PropertyName._titleLabel)
		{
			value = VariantUtils.CreateFrom(in _titleLabel);
			return true;
		}
		if (name == PropertyName._subtitleLabel)
		{
			value = VariantUtils.CreateFrom(in _subtitleLabel);
			return true;
		}
		if (name == PropertyName._categoryTabs)
		{
			value = VariantUtils.CreateFrom(in _categoryTabs);
			return true;
		}
		if (name == PropertyName._searchEdit)
		{
			value = VariantUtils.CreateFrom(in _searchEdit);
			return true;
		}
		if (name == PropertyName._choiceTree)
		{
			value = VariantUtils.CreateFrom(in _choiceTree);
			return true;
		}
		if (name == PropertyName._previewTexture)
		{
			value = VariantUtils.CreateFrom(in _previewTexture);
			return true;
		}
		if (name == PropertyName._displayNameLabel)
		{
			value = VariantUtils.CreateFrom(in _displayNameLabel);
			return true;
		}
		if (name == PropertyName._resourceKeyLabel)
		{
			value = VariantUtils.CreateFrom(in _resourceKeyLabel);
			return true;
		}
		if (name == PropertyName._sourceBadge)
		{
			value = VariantUtils.CreateFrom(in _sourceBadge);
			return true;
		}
		if (name == PropertyName._missingBadge)
		{
			value = VariantUtils.CreateFrom(in _missingBadge);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._confirmButton)
		{
			value = VariantUtils.CreateFrom(in _confirmButton);
			return true;
		}
		if (name == PropertyName._audioPreviewControls)
		{
			value = VariantUtils.CreateFrom(in _audioPreviewControls);
			return true;
		}
		if (name == PropertyName._audioPreviewButton)
		{
			value = VariantUtils.CreateFrom(in _audioPreviewButton);
			return true;
		}
		if (name == PropertyName._audioStopButton)
		{
			value = VariantUtils.CreateFrom(in _audioStopButton);
			return true;
		}
		if (name == PropertyName._audioProgress)
		{
			value = VariantUtils.CreateFrom(in _audioProgress);
			return true;
		}
		if (name == PropertyName._audioTimeLabel)
		{
			value = VariantUtils.CreateFrom(in _audioTimeLabel);
			return true;
		}
		if (name == PropertyName._audioPreviewPlayer)
		{
			value = VariantUtils.CreateFrom(in _audioPreviewPlayer);
			return true;
		}
		if (name == PropertyName._audioPreviewLength)
		{
			value = VariantUtils.CreateFrom(in _audioPreviewLength);
			return true;
		}
		if (name == PropertyName._activeKind)
		{
			value = VariantUtils.CreateFrom(in _activeKind);
			return true;
		}
		if (name == PropertyName._currentKey)
		{
			value = VariantUtils.CreateFrom(in _currentKey);
			return true;
		}
		if (name == PropertyName._resourceLibraryMode)
		{
			value = VariantUtils.CreateFrom(in _resourceLibraryMode);
			return true;
		}
		if (name == PropertyName._resourceLibraryCategory)
		{
			value = VariantUtils.CreateFrom(in _resourceLibraryCategory);
			return true;
		}
		if (name == PropertyName._resourceLibraryDisplayName)
		{
			value = VariantUtils.CreateFrom(in _resourceLibraryDisplayName);
			return true;
		}
		if (name == PropertyName._resourceLibraryProjectPath)
		{
			value = VariantUtils.CreateFrom(in _resourceLibraryProjectPath);
			return true;
		}
		if (name == PropertyName._resourceLibraryIconPath)
		{
			value = VariantUtils.CreateFrom(in _resourceLibraryIconPath);
			return true;
		}
		if (name == PropertyName._resourceLibraryIndexStartFrames)
		{
			value = VariantUtils.CreateFrom(in _resourceLibraryIndexStartFrames);
			return true;
		}
		if (name == PropertyName._resourceLibraryIndexGeneration)
		{
			value = VariantUtils.CreateFrom(in _resourceLibraryIndexGeneration);
			return true;
		}
		if (name == PropertyName._resourceLibraryIndexTaskGeneration)
		{
			value = VariantUtils.CreateFrom(in _resourceLibraryIndexTaskGeneration);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.String, PropertyName.ResourceLibraryProjectPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsResourceLibraryIndexing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._titleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._subtitleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._categoryTabs, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._searchEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._choiceTree, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewTexture, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._displayNameLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceKeyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sourceBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._missingBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._confirmButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioPreviewControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioPreviewButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioStopButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioTimeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioPreviewPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._audioPreviewLength, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._activeKind, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._currentKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._resourceLibraryMode, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._resourceLibraryCategory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._resourceLibraryDisplayName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._resourceLibraryProjectPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._resourceLibraryIconPath, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._resourceLibraryIndexStartFrames, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._resourceLibraryIndexGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._resourceLibraryIndexTaskGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._titleLabel, Variant.From(in _titleLabel));
		info.AddProperty(PropertyName._subtitleLabel, Variant.From(in _subtitleLabel));
		info.AddProperty(PropertyName._categoryTabs, Variant.From(in _categoryTabs));
		info.AddProperty(PropertyName._searchEdit, Variant.From(in _searchEdit));
		info.AddProperty(PropertyName._choiceTree, Variant.From(in _choiceTree));
		info.AddProperty(PropertyName._previewTexture, Variant.From(in _previewTexture));
		info.AddProperty(PropertyName._displayNameLabel, Variant.From(in _displayNameLabel));
		info.AddProperty(PropertyName._resourceKeyLabel, Variant.From(in _resourceKeyLabel));
		info.AddProperty(PropertyName._sourceBadge, Variant.From(in _sourceBadge));
		info.AddProperty(PropertyName._missingBadge, Variant.From(in _missingBadge));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._confirmButton, Variant.From(in _confirmButton));
		info.AddProperty(PropertyName._audioPreviewControls, Variant.From(in _audioPreviewControls));
		info.AddProperty(PropertyName._audioPreviewButton, Variant.From(in _audioPreviewButton));
		info.AddProperty(PropertyName._audioStopButton, Variant.From(in _audioStopButton));
		info.AddProperty(PropertyName._audioProgress, Variant.From(in _audioProgress));
		info.AddProperty(PropertyName._audioTimeLabel, Variant.From(in _audioTimeLabel));
		info.AddProperty(PropertyName._audioPreviewPlayer, Variant.From(in _audioPreviewPlayer));
		info.AddProperty(PropertyName._audioPreviewLength, Variant.From(in _audioPreviewLength));
		info.AddProperty(PropertyName._activeKind, Variant.From(in _activeKind));
		info.AddProperty(PropertyName._currentKey, Variant.From(in _currentKey));
		info.AddProperty(PropertyName._resourceLibraryMode, Variant.From(in _resourceLibraryMode));
		info.AddProperty(PropertyName._resourceLibraryCategory, Variant.From(in _resourceLibraryCategory));
		info.AddProperty(PropertyName._resourceLibraryDisplayName, Variant.From(in _resourceLibraryDisplayName));
		info.AddProperty(PropertyName._resourceLibraryProjectPath, Variant.From(in _resourceLibraryProjectPath));
		info.AddProperty(PropertyName._resourceLibraryIconPath, Variant.From(in _resourceLibraryIconPath));
		info.AddProperty(PropertyName._resourceLibraryIndexStartFrames, Variant.From(in _resourceLibraryIndexStartFrames));
		info.AddProperty(PropertyName._resourceLibraryIndexGeneration, Variant.From(in _resourceLibraryIndexGeneration));
		info.AddProperty(PropertyName._resourceLibraryIndexTaskGeneration, Variant.From(in _resourceLibraryIndexTaskGeneration));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._titleLabel, out var value))
		{
			_titleLabel = value.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._subtitleLabel, out var value2))
		{
			_subtitleLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._categoryTabs, out var value3))
		{
			_categoryTabs = value3.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._searchEdit, out var value4))
		{
			_searchEdit = value4.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._choiceTree, out var value5))
		{
			_choiceTree = value5.As<Tree>();
		}
		if (info.TryGetProperty(PropertyName._previewTexture, out var value6))
		{
			_previewTexture = value6.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._displayNameLabel, out var value7))
		{
			_displayNameLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resourceKeyLabel, out var value8))
		{
			_resourceKeyLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._sourceBadge, out var value9))
		{
			_sourceBadge = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._missingBadge, out var value10))
		{
			_missingBadge = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value11))
		{
			_statusLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._confirmButton, out var value12))
		{
			_confirmButton = value12.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._audioPreviewControls, out var value13))
		{
			_audioPreviewControls = value13.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._audioPreviewButton, out var value14))
		{
			_audioPreviewButton = value14.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._audioStopButton, out var value15))
		{
			_audioStopButton = value15.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._audioProgress, out var value16))
		{
			_audioProgress = value16.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._audioTimeLabel, out var value17))
		{
			_audioTimeLabel = value17.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._audioPreviewPlayer, out var value18))
		{
			_audioPreviewPlayer = value18.As<AudioStreamPlayer>();
		}
		if (info.TryGetProperty(PropertyName._audioPreviewLength, out var value19))
		{
			_audioPreviewLength = value19.As<double>();
		}
		if (info.TryGetProperty(PropertyName._activeKind, out var value20))
		{
			_activeKind = value20.As<XWGameplayResourceKind>();
		}
		if (info.TryGetProperty(PropertyName._currentKey, out var value21))
		{
			_currentKey = value21.As<string>();
		}
		if (info.TryGetProperty(PropertyName._resourceLibraryMode, out var value22))
		{
			_resourceLibraryMode = value22.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._resourceLibraryCategory, out var value23))
		{
			_resourceLibraryCategory = value23.As<string>();
		}
		if (info.TryGetProperty(PropertyName._resourceLibraryDisplayName, out var value24))
		{
			_resourceLibraryDisplayName = value24.As<string>();
		}
		if (info.TryGetProperty(PropertyName._resourceLibraryProjectPath, out var value25))
		{
			_resourceLibraryProjectPath = value25.As<string>();
		}
		if (info.TryGetProperty(PropertyName._resourceLibraryIconPath, out var value26))
		{
			_resourceLibraryIconPath = value26.As<string>();
		}
		if (info.TryGetProperty(PropertyName._resourceLibraryIndexStartFrames, out var value27))
		{
			_resourceLibraryIndexStartFrames = value27.As<int>();
		}
		if (info.TryGetProperty(PropertyName._resourceLibraryIndexGeneration, out var value28))
		{
			_resourceLibraryIndexGeneration = value28.As<long>();
		}
		if (info.TryGetProperty(PropertyName._resourceLibraryIndexTaskGeneration, out var value29))
		{
			_resourceLibraryIndexTaskGeneration = value29.As<long>();
		}
	}
}
