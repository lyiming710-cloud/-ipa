using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWNpcTalkInlineEditor.cs")]
public class XWNpcTalkInlineEditor : VBoxContainer
{
	[Signal]
	public delegate void TalkEditedEventHandler();

	public new class MethodName : VBoxContainer.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName EditConfig = "EditConfig";

		public static readonly StringName BindSceneNodes = "BindSceneNodes";

		public static readonly StringName PopulateFixedOptions = "PopulateFixedOptions";

		public static readonly StringName BuildTypeVisualCards = "BuildTypeVisualCards";

		public static readonly StringName RebuildEntryVisualCards = "RebuildEntryVisualCards";

		public static readonly StringName CreateVisualChoiceCard = "CreateVisualChoiceCard";

		public static readonly StringName ConfigureEntryCard = "ConfigureEntryCard";

		public static readonly StringName RefreshEntryVisualCard = "RefreshEntryVisualCard";

		public static readonly StringName RefreshEntryVisualSelection = "RefreshEntryVisualSelection";

		public static readonly StringName OpenEntryVisualPicker = "OpenEntryVisualPicker";

		public static readonly StringName OpenAddTypeVisualPicker = "OpenAddTypeVisualPicker";

		public static readonly StringName SelectAddTypeVisual = "SelectAddTypeVisual";

		public static readonly StringName SetCurrentText = "SetCurrentText";

		public static readonly StringName GetEntryTypeIcon = "GetEntryTypeIcon";

		public static readonly StringName EntryTypeIndex = "EntryTypeIndex";

		public static readonly StringName BuildEntryVisualDetail = "BuildEntryVisualDetail";

		public static readonly StringName ConnectSceneSignals = "ConnectSceneSignals";

		public static readonly StringName BindRootFields = "BindRootFields";

		public static readonly StringName RefreshRootHeader = "RefreshRootHeader";

		public static readonly StringName RefreshFromConfig = "RefreshFromConfig";

		public static readonly StringName ShowCurrent = "ShowCurrent";

		public static readonly StringName BindCurrentEntry = "BindCurrentEntry";

		public static readonly StringName RefreshEntryControlValues = "RefreshEntryControlValues";

		public static readonly StringName RefreshEntryVisuals = "RefreshEntryVisuals";

		public static readonly StringName RefreshNpcEditorFromHistory = "RefreshNpcEditorFromHistory";

		public static readonly StringName SelectEntry = "SelectEntry";

		public static readonly StringName ShowPrevious = "ShowPrevious";

		public static readonly StringName ShowNext = "ShowNext";

		public static readonly StringName AddTalkEntry = "AddTalkEntry";

		public static readonly StringName DuplicateTalkEntry = "DuplicateTalkEntry";

		public static readonly StringName RemoveTalkEntry = "RemoveTalkEntry";

		public static readonly StringName MoveTalkEntry = "MoveTalkEntry";

		public static readonly StringName ReplaceTalkList = "ReplaceTalkList";

		public static readonly StringName ChooseJsonSource = "ChooseJsonSource";

		public static readonly StringName ApplyJsonSource = "ApplyJsonSource";

		public static readonly StringName SelectKnownNpc = "SelectKnownNpc";

		public static readonly StringName SelectKnownAnimation = "SelectKnownAnimation";

		public static readonly StringName SetEntryString = "SetEntryString";

		public static readonly StringName ShowAudioPicker = "ShowAudioPicker";

		public static readonly StringName EnsureAudioPicker = "EnsureAudioPicker";

		public static readonly StringName PlayCurrentAudio = "PlayCurrentAudio";

		public static readonly StringName LoadSelectedAudioStream = "LoadSelectedAudioStream";

		public static readonly StringName ChoosePropScene = "ChoosePropScene";

		public static readonly StringName ApplyPropScene = "ApplyPropScene";

		public static readonly StringName ChooseTutorialResource = "ChooseTutorialResource";

		public static readonly StringName ApplyTutorialResource = "ApplyTutorialResource";

		public static readonly StringName SetEntryResource = "SetEntryResource";

		public static readonly StringName EnsureNpc = "EnsureNpc";

		public static readonly StringName AttachBubbleEditor = "AttachBubbleEditor";

		public static readonly StringName ApplyTalk = "ApplyTalk";

		public static readonly StringName RebuildNpcOptions = "RebuildNpcOptions";

		public static readonly StringName RebuildAnimationOptions = "RebuildAnimationOptions";

		public static readonly StringName SelectAnimationOption = "SelectAnimationOption";

		public static readonly StringName SelectOptionByMetadata = "SelectOptionByMetadata";

		public static readonly StringName SetEntryPanelsVisible = "SetEntryPanelsVisible";

		public static readonly StringName UpdateToolbarButtons = "UpdateToolbarButtons";

		public static readonly StringName GetCurrentTalk = "GetCurrentTalk";

		public static readonly StringName HasCurrentTalk = "HasCurrentTalk";

		public static readonly StringName EmitEdited = "EmitEdited";

		public static readonly StringName ClearNpc = "ClearNpc";

		public static readonly StringName BuildEntryLabel = "BuildEntryLabel";

		public static readonly StringName BuildStatus = "BuildStatus";

		public static readonly StringName EntryTypeName = "EntryTypeName";

		public static readonly StringName ResourceLabel = "ResourceLabel";

		public static readonly StringName DisplayNpc = "DisplayNpc";

		public static readonly StringName DisplayValue = "DisplayValue";
	}

	public new class PropertyName : VBoxContainer.PropertyName
	{
		public static readonly StringName EntryCount = "EntryCount";

		public static readonly StringName CurrentEntryIndex = "CurrentEntryIndex";

		public static readonly StringName EntryVisualCardCount = "EntryVisualCardCount";

		public static readonly StringName EntryTypeCardCount = "EntryTypeCardCount";

		public static readonly StringName SelectedAddType = "SelectedAddType";

		public static readonly StringName ResponsiveStage = "ResponsiveStage";

		public static readonly StringName _config = "_config";

		public static readonly StringName _editingRoot = "_editingRoot";

		public static readonly StringName _standaloneEntry = "_standaloneEntry";

		public static readonly StringName _index = "_index";

		public static readonly StringName _runtimeControl = "_runtimeControl";

		public static readonly StringName _npc = "_npc";

		public static readonly StringName _npcSceneKey = "_npcSceneKey";

		public static readonly StringName _bubbleEditor = "_bubbleEditor";

		public static readonly StringName _undoRedo = "_undoRedo";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _pendingPropProperty = "_pendingPropProperty";

		public static readonly StringName _selectedAddType = "_selectedAddType";

		public static readonly StringName _saveKeyLineEdit = "_saveKeyLineEdit";

		public static readonly StringName _jsonSourceButton = "_jsonSourceButton";

		public static readonly StringName _entryVisualButton = "_entryVisualButton";

		public static readonly StringName _addTypeVisualButton = "_addTypeVisualButton";

		public static readonly StringName _entryPickerPopup = "_entryPickerPopup";

		public static readonly StringName _addTypePopup = "_addTypePopup";

		public static readonly StringName _entryPickerGrid = "_entryPickerGrid";

		public static readonly StringName _addTypeGrid = "_addTypeGrid";

		public static readonly StringName _stageFrame = "_stageFrame";

		public static readonly StringName _previousButton = "_previousButton";

		public static readonly StringName _nextButton = "_nextButton";

		public static readonly StringName _addButton = "_addButton";

		public static readonly StringName _duplicateButton = "_duplicateButton";

		public static readonly StringName _removeButton = "_removeButton";

		public static readonly StringName _moveUpButton = "_moveUpButton";

		public static readonly StringName _moveDownButton = "_moveDownButton";

		public static readonly StringName _emptyLabel = "_emptyLabel";

		public static readonly StringName _statusLabel = "_statusLabel";

		public static readonly StringName _npcOption = "_npcOption";

		public static readonly StringName _animeOption = "_animeOption";

		public static readonly StringName _audioSelectionButton = "_audioSelectionButton";

		public static readonly StringName _playAudioButton = "_playAudioButton";

		public static readonly StringName _audioPicker = "_audioPicker";

		public static readonly StringName _handPropsPanel = "_handPropsPanel";

		public static readonly StringName _tutorialPanel = "_tutorialPanel";

		public static readonly StringName _tutorialResourceButton = "_tutorialResourceButton";

		public static readonly StringName _jsonSourceDialog = "_jsonSourceDialog";

		public static readonly StringName _propSceneDialog = "_propSceneDialog";

		public static readonly StringName _tutorialResourceDialog = "_tutorialResourceDialog";

		public static readonly StringName _directAudioPlayer = "_directAudioPlayer";
	}

	public new class SignalName : VBoxContainer.SignalName
	{
		public static readonly StringName TalkEdited = "TalkEdited";
	}

	private const string CrazyDaveScenePath = "res://Prefab/Npc/CrazyDave/NpcCrazyDave.tscn";

	private const string WeiWeiMiScenePath = "res://Prefab/Npc/WeiWeiMi/WeiWeiMi.tscn";

	private const string BubbleTextEditorScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWNpcTalkBubbleTextEditor.tscn";

	private const string VisualChoiceCardScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWNpcTalkVisualChoiceCard.tscn";

	private static readonly string[] KnownNpcKeys = new string[2] { "CrazyDave", "WeiWeiMi" };

	private static readonly string[] HandSceneProperties = new string[4] { "handScene", "shoulderScene", "shoulder2Scene", "headScene" };

	private static readonly string[] EntryTypeTitles = new string[4] { "普通气泡", "道具表演", "教程衔接", "购买对话" };

	private static readonly string[] EntryTypeDetails = new string[4] { "NPC、动画、音频和气泡", "额外装配手部、肩部和头部场景", "对话结束后进入教程资源", "商店和购买流程使用的对白" };

	private static readonly string[] EntryTypeIconPaths = new string[4] { "res://addons/ModEditor/Icons/ClassIcon/RichTextLabel.svg", "res://addons/ModEditor/Icons/ClassIcon/PackedScene.svg", "res://addons/ModEditor/Icons/ClassIcon/GraphEdit.svg", "res://addons/ModEditor/Icons/AssetLib.svg" };

	private static PackedScene _bubbleTextEditorScene;

	private static PackedScene _visualChoiceCardScene;

	private NpcTalkConfig _config;

	private Resource _editingRoot;

	private bool _standaloneEntry;

	private int _index;

	private NpcTalkControl _runtimeControl;

	private NpcBase _npc;

	private string _npcSceneKey = "";

	private TextEdit _bubbleEditor;

	private XWVisualPropertyBinding _rootBinding;

	private XWVisualPropertyBinding _entryBinding;

	private XWUndoRedoManager _undoRedo;

	private bool _updatingControls;

	private string _pendingPropProperty = "";

	private int _selectedAddType;

	private LineEdit _saveKeyLineEdit;

	private Button _jsonSourceButton;

	private Button _entryVisualButton;

	private Button _addTypeVisualButton;

	private PopupPanel _entryPickerPopup;

	private PopupPanel _addTypePopup;

	private HFlowContainer _entryPickerGrid;

	private HFlowContainer _addTypeGrid;

	private XWAspectScaledPreviewHost _stageFrame;

	private readonly List<XWNpcTalkVisualChoiceCard> _entryCards = new List<XWNpcTalkVisualChoiceCard>();

	private readonly List<XWNpcTalkVisualChoiceCard> _typeCards = new List<XWNpcTalkVisualChoiceCard>();

	private Button _previousButton;

	private Button _nextButton;

	private Button _addButton;

	private Button _duplicateButton;

	private Button _removeButton;

	private Button _moveUpButton;

	private Button _moveDownButton;

	private Label _emptyLabel;

	private Label _statusLabel;

	private OptionButton _npcOption;

	private XWVisualOptionGallery _npcVisualGallery;

	private OptionButton _animeOption;

	private XWVisualOptionGallery _animeVisualGallery;

	private Button _audioSelectionButton;

	private Button _playAudioButton;

	private XWGameplayResourcePickerWindow _audioPicker;

	private PanelContainer _handPropsPanel;

	private PanelContainer _tutorialPanel;

	private Button _tutorialResourceButton;

	private readonly System.Collections.Generic.Dictionary<string, Button> _propSceneButtons = new System.Collections.Generic.Dictionary<string, Button>();

	private FileDialog _jsonSourceDialog;

	private FileDialog _propSceneDialog;

	private FileDialog _tutorialResourceDialog;

	private AudioStreamPlayer _directAudioPlayer;

	private TalkEditedEventHandler backing_TalkEdited;

	public int EntryCount => (_config?.talkList?.Count).GetValueOrDefault();

	public int CurrentEntryIndex => _index;

	public int EntryVisualCardCount => _entryCards.Count;

	public int EntryTypeCardCount => _typeCards.Count;

	public int SelectedAddType => _selectedAddType;

	public XWAspectScaledPreviewHost ResponsiveStage => _stageFrame;

	public event TalkEditedEventHandler TalkEdited
	{
		add
		{
			backing_TalkEdited = (TalkEditedEventHandler)Delegate.Combine(backing_TalkEdited, value);
		}
		remove
		{
			backing_TalkEdited = (TalkEditedEventHandler)Delegate.Remove(backing_TalkEdited, value);
		}
	}

	public override void _Ready()
	{
		_undoRedo = XWEditorInterface.Instance?.GetUndoRedoManager();
		BindSceneNodes();
		PopulateFixedOptions();
		ConnectSceneSignals();
		RefreshFromConfig();
	}

	public override void _ExitTree()
	{
		_npcVisualGallery?.Dispose();
		_npcVisualGallery = null;
		_animeVisualGallery?.Dispose();
		_animeVisualGallery = null;
		_rootBinding?.Dispose();
		_entryBinding?.Dispose();
		_rootBinding = null;
		_entryBinding = null;
		_directAudioPlayer?.Stop();
		ClearNpc();
		base._ExitTree();
	}

	public void EditConfig(NpcTalkConfig config, Resource editingRoot)
	{
		_config = config;
		_editingRoot = editingRoot;
		_standaloneEntry = editingRoot is NpcTalkBaseConfig;
		_index = 0;
		if (IsNodeReady())
		{
			RefreshFromConfig();
		}
	}

	private void BindSceneNodes()
	{
		_runtimeControl = GetNode<NpcTalkControl>("%RuntimeNpcTalkControl");
		_saveKeyLineEdit = GetNode<LineEdit>("%SaveKeyLineEdit");
		_jsonSourceButton = GetNode<Button>("%JsonSourceButton");
		_entryVisualButton = GetNode<Button>("%EntryVisualButton");
		_addTypeVisualButton = GetNode<Button>("%AddTypeVisualButton");
		_entryPickerPopup = GetNode<PopupPanel>("%EntryPickerPopup");
		_addTypePopup = GetNode<PopupPanel>("%AddTypePopup");
		_entryPickerGrid = GetNode<HFlowContainer>("%EntryPickerGrid");
		_addTypeGrid = GetNode<HFlowContainer>("%AddTypeGrid");
		_stageFrame = GetNode<XWAspectScaledPreviewHost>("%StageFrame");
		_previousButton = GetNode<Button>("%PreviousButton");
		_nextButton = GetNode<Button>("%NextButton");
		_addButton = GetNode<Button>("%AddEntryButton");
		_duplicateButton = GetNode<Button>("%DuplicateEntryButton");
		_removeButton = GetNode<Button>("%RemoveEntryButton");
		_moveUpButton = GetNode<Button>("%MoveEntryUpButton");
		_moveDownButton = GetNode<Button>("%MoveEntryDownButton");
		_emptyLabel = GetNode<Label>("%EmptyLabel");
		_statusLabel = GetNode<Label>("%Status");
		_npcOption = GetNode<OptionButton>("%NpcOption");
		_animeOption = GetNode<OptionButton>("%AnimeOption");
		_npcVisualGallery = new XWVisualOptionGallery(_npcOption, GetNode<HFlowContainer>("%NpcVisualChoices"), "NpcVisualCatalog", (int _) => ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ResourceCharacter.svg", null, ResourceLoader.CacheMode.Reuse));
		_animeVisualGallery = new XWVisualOptionGallery(_animeOption, GetNode<HFlowContainer>("%AnimeVisualChoices"), "NpcAnimationVisualCatalog", (int _) => ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/MainMovieWrite.svg", null, ResourceLoader.CacheMode.Reuse));
		_audioSelectionButton = GetNode<Button>("%AudioSelectionButton");
		_playAudioButton = GetNode<Button>("%PlayAudioButton");
		_handPropsPanel = GetNode<PanelContainer>("%HandPropsPanel");
		_tutorialPanel = GetNode<PanelContainer>("%TutorialPanel");
		_tutorialResourceButton = GetNode<Button>("%TutorialResourceButton");
		_jsonSourceDialog = GetNode<FileDialog>("%JsonSourceDialog");
		_propSceneDialog = GetNode<FileDialog>("%PropSceneDialog");
		_tutorialResourceDialog = GetNode<FileDialog>("%TutorialResourceDialog");
		_directAudioPlayer = GetNode<AudioStreamPlayer>("%DirectAudioPlayer");
		_propSceneButtons["handScene"] = GetNode<Button>("%HandSceneButton");
		_propSceneButtons["shoulderScene"] = GetNode<Button>("%ShoulderSceneButton");
		_propSceneButtons["shoulder2Scene"] = GetNode<Button>("%Shoulder2SceneButton");
		_propSceneButtons["headScene"] = GetNode<Button>("%HeadSceneButton");
	}

	private void PopulateFixedOptions()
	{
		BuildTypeVisualCards();
		SelectAddTypeVisual(0);
		RebuildNpcOptions("");
	}

	private void BuildTypeVisualCards()
	{
		ClearVisualCards(_addTypeGrid, _typeCards);
		for (int i = 0; i < EntryTypeTitles.Length; i++)
		{
			int capturedIndex = i;
			XWNpcTalkVisualChoiceCard xWNpcTalkVisualChoiceCard = CreateVisualChoiceCard();
			if (GodotObject.IsInstanceValid(xWNpcTalkVisualChoiceCard))
			{
				xWNpcTalkVisualChoiceCard.Configure(i, EntryTypeTitles[i], EntryTypeDetails[i], GetEntryTypeIcon(i), i == _selectedAddType);
				xWNpcTalkVisualChoiceCard.Pressed += () =>
				{
					SelectAddTypeVisual(capturedIndex);
				};
				_typeCards.Add(xWNpcTalkVisualChoiceCard);
				_addTypeGrid.AddChild(xWNpcTalkVisualChoiceCard, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void RebuildEntryVisualCards()
	{
		ClearVisualCards(_entryPickerGrid, _entryCards);
		int valueOrDefault = (_config?.talkList?.Count).GetValueOrDefault();
		for (int i = 0; i < valueOrDefault; i++)
		{
			int capturedIndex = i;
			XWNpcTalkVisualChoiceCard xWNpcTalkVisualChoiceCard = CreateVisualChoiceCard();
			if (GodotObject.IsInstanceValid(xWNpcTalkVisualChoiceCard))
			{
				ConfigureEntryCard(xWNpcTalkVisualChoiceCard, i, _config.talkList[i]);
				xWNpcTalkVisualChoiceCard.Pressed += () =>
				{
					SelectEntry(capturedIndex);
					_entryPickerPopup.Hide();
				};
				_entryCards.Add(xWNpcTalkVisualChoiceCard);
				_entryPickerGrid.AddChild(xWNpcTalkVisualChoiceCard, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private XWNpcTalkVisualChoiceCard CreateVisualChoiceCard()
	{
		if (_visualChoiceCardScene == null)
		{
			_visualChoiceCardScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWNpcTalkVisualChoiceCard.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		return _visualChoiceCardScene?.Instantiate<XWNpcTalkVisualChoiceCard>(PackedScene.GenEditState.Disabled);
	}

	private static void ClearVisualCards(Control host, List<XWNpcTalkVisualChoiceCard> cards)
	{
		foreach (XWNpcTalkVisualChoiceCard card in cards)
		{
			if (GodotObject.IsInstanceValid(card))
			{
				if (card.GetParent() == host)
				{
					host.RemoveChild(card);
				}
				card.QueueFree();
			}
		}
		cards.Clear();
	}

	private void ConfigureEntryCard(XWNpcTalkVisualChoiceCard card, int index, NpcTalkBaseConfig talk)
	{
		int typeIndex = EntryTypeIndex(talk);
		card.Configure(index, $"#{index + 1}  {EntryTypeName(talk)}", BuildEntryVisualDetail(talk), GetEntryTypeIcon(typeIndex), index == _index);
	}

	private void RefreshEntryVisualCard(int index)
	{
		int valueOrDefault = (_config?.talkList?.Count).GetValueOrDefault();
		if (index >= 0 && index < valueOrDefault && index < _entryCards.Count)
		{
			ConfigureEntryCard(_entryCards[index], index, _config.talkList[index]);
		}
	}

	private void RefreshEntryVisualSelection()
	{
		NpcTalkBaseConfig currentTalk = GetCurrentTalk();
		if (GodotObject.IsInstanceValid(currentTalk))
		{
			_entryVisualButton.Text = BuildEntryLabel(_index, currentTalk);
			_entryVisualButton.Icon = GetEntryTypeIcon(EntryTypeIndex(currentTalk));
			for (int i = 0; i < _entryCards.Count; i++)
			{
				_entryCards[i].SetSelected(i == _index);
			}
		}
	}

	private void OpenEntryVisualPicker()
	{
		if (EntryCount > 0)
		{
			_entryPickerPopup.PopupCenteredClamped(new Vector2I(720, 480), 0.88f);
		}
	}

	private void OpenAddTypeVisualPicker()
	{
		if (!_standaloneEntry)
		{
			_addTypePopup.PopupCenteredClamped(new Vector2I(650, 300), 0.88f);
		}
	}

	public void SelectAddTypeVisual(int typeIndex)
	{
		_selectedAddType = Mathf.Clamp(typeIndex, 0, EntryTypeTitles.Length - 1);
		_addTypeVisualButton.Text = "新增类型：" + EntryTypeTitles[_selectedAddType];
		_addTypeVisualButton.Icon = GetEntryTypeIcon(_selectedAddType);
		for (int i = 0; i < _typeCards.Count; i++)
		{
			_typeCards[i].SetSelected(_typeCards[i].ChoiceIndex == _selectedAddType);
		}
		if (GodotObject.IsInstanceValid(_addTypePopup) && _addTypePopup.Visible)
		{
			_addTypePopup.Hide();
		}
	}

	public bool SetCurrentText(string text)
	{
		NpcTalkBaseConfig currentTalk = GetCurrentTalk();
		if (!GodotObject.IsInstanceValid(currentTalk) || _entryBinding == null)
		{
			return false;
		}
		_entryBinding.SetValue(currentTalk, "text", text ?? "", "修改 NPC 气泡文本", this, "RefreshNpcEditorFromHistory");
		ShowCurrent();
		return currentTalk.text == (text ?? "");
	}

	private static Texture2D GetEntryTypeIcon(int typeIndex)
	{
		int num = Mathf.Clamp(typeIndex, 0, EntryTypeIconPaths.Length - 1);
		return ResourceLoader.Load<Texture2D>(EntryTypeIconPaths[num], null, ResourceLoader.CacheMode.Reuse);
	}

	private static int EntryTypeIndex(NpcTalkBaseConfig talk)
	{
		if (!(talk is NpcTalkHandConfig))
		{
			if (!(talk is NpcTalkTutorialConfig))
			{
				if (talk is NpcTalkBuyConfig)
				{
					return 3;
				}
				return 0;
			}
			return 2;
		}
		return 1;
	}

	private static string BuildEntryVisualDetail(NpcTalkBaseConfig talk)
	{
		if (!GodotObject.IsInstanceValid(talk))
		{
			return "空条目";
		}
		string text = (talk.text ?? "").Replace('\n', ' ').Replace('\r', ' ').StripEdges();
		if (text.Length > 32)
		{
			text = text.Substring(0, 32) + "…";
		}
		return DisplayNpc(talk.npc) + " · " + text;
	}

	private void ConnectSceneSignals()
	{
		_previousButton.Pressed += ShowPrevious;
		_nextButton.Pressed += ShowNext;
		_entryVisualButton.Pressed += OpenEntryVisualPicker;
		_addTypeVisualButton.Pressed += OpenAddTypeVisualPicker;
		_addButton.Pressed += () =>
		{
			AddTalkEntry();
		};
		_duplicateButton.Pressed += () =>
		{
			DuplicateTalkEntry();
		};
		_removeButton.Pressed += () =>
		{
			RemoveTalkEntry();
		};
		_moveUpButton.Pressed += () =>
		{
			MoveTalkEntry(-1);
		};
		_moveDownButton.Pressed += () =>
		{
			MoveTalkEntry(1);
		};
		_jsonSourceButton.Pressed += ChooseJsonSource;
		_jsonSourceDialog.FileSelected += ApplyJsonSource;
		_npcOption.ItemSelected += SelectKnownNpc;
		_animeOption.ItemSelected += SelectKnownAnimation;
		_audioSelectionButton.Pressed += ShowAudioPicker;
		_playAudioButton.Pressed += PlayCurrentAudio;
		GetNode<Button>("%ClearAudioButton").Pressed += () =>
		{
			SetEntryString("audio", "", "清除 NPC 对话音效");
		};
		string[] handSceneProperties = HandSceneProperties;
		foreach (string text in handSceneProperties)
		{
			string captured = text;
			_propSceneButtons[text].Pressed += () =>
			{
				ChoosePropScene(captured);
			};
		}
		GetNode<Button>("%ClearHandSceneButton").Pressed += () =>
		{
			SetEntryResource("handScene", null, "清除手部道具");
		};
		GetNode<Button>("%ClearShoulderSceneButton").Pressed += () =>
		{
			SetEntryResource("shoulderScene", null, "清除肩部道具 1");
		};
		GetNode<Button>("%ClearShoulder2SceneButton").Pressed += () =>
		{
			SetEntryResource("shoulder2Scene", null, "清除肩部道具 2");
		};
		GetNode<Button>("%ClearHeadSceneButton").Pressed += () =>
		{
			SetEntryResource("headScene", null, "清除头部道具");
		};
		_propSceneDialog.FileSelected += ApplyPropScene;
		_tutorialResourceButton.Pressed += ChooseTutorialResource;
		GetNode<Button>("%ClearTutorialButton").Pressed += () =>
		{
			SetEntryResource("tutorial", null, "清除教程引用");
		};
		_tutorialResourceDialog.FileSelected += ApplyTutorialResource;
	}

	private void BindRootFields()
	{
		_rootBinding?.Dispose();
		_rootBinding = null;
		bool flag = GodotObject.IsInstanceValid(_config) && !_standaloneEntry;
		_saveKeyLineEdit.Editable = flag;
		_jsonSourceButton.Disabled = !flag;
		_addTypeVisualButton.Disabled = !flag;
		_addButton.Disabled = !flag;
		if (!flag)
		{
			_saveKeyLineEdit.Text = "独立对话条目";
			_jsonSourceButton.Text = "独立条目 · 无 JSON 根来源";
			return;
		}
		_rootBinding = new XWVisualPropertyBinding(_undoRedo, (bool _) =>
		{
			EmitEdited();
		});
		_rootBinding.BindText(_saveKeyLineEdit, _config, "saveKey", RefreshRootHeader, this, "RefreshNpcEditorFromHistory");
		RefreshRootHeader();
	}

	private void RefreshRootHeader()
	{
		if (GodotObject.IsInstanceValid(_config) && !_standaloneEntry)
		{
			string text = (GodotObject.IsInstanceValid(_config.data) ? _config.data.ResourcePath.GetFile() : "未设置");
			_jsonSourceButton.Text = "JSON 来源：" + text;
			_jsonSourceButton.TooltipText = (GodotObject.IsInstanceValid(_config.data) ? _config.data.ResourcePath : "选择 JSON 会按游戏加载规则重新生成整段对话");
		}
	}

	private void RefreshFromConfig()
	{
		if (IsNodeReady())
		{
			_entryBinding?.Dispose();
			_entryBinding = null;
			BindRootFields();
			int valueOrDefault = (_config?.talkList?.Count).GetValueOrDefault();
			RebuildEntryVisualCards();
			_emptyLabel.Visible = valueOrDefault == 0;
			_entryVisualButton.Disabled = valueOrDefault == 0;
			_previousButton.Disabled = valueOrDefault == 0;
			_nextButton.Disabled = valueOrDefault == 0;
			_duplicateButton.Disabled = _standaloneEntry || valueOrDefault == 0;
			_removeButton.Disabled = _standaloneEntry || valueOrDefault == 0;
			_moveUpButton.Disabled = _standaloneEntry || valueOrDefault < 2;
			_moveDownButton.Disabled = _standaloneEntry || valueOrDefault < 2;
			if (valueOrDefault == 0)
			{
				_index = 0;
				_entryVisualButton.Text = "当前没有可编辑的句子";
				_entryVisualButton.Icon = GetEntryTypeIcon(0);
				ClearNpc();
				SetEntryPanelsVisible(null);
				_statusLabel.Text = "当前没有对话条目；请在上方选择表现类型并点击“新增”。";
			}
			else
			{
				_index = Mathf.Clamp(_index, 0, valueOrDefault - 1);
				ShowCurrent();
			}
		}
	}

	private void ShowCurrent()
	{
		_entryBinding?.Dispose();
		_entryBinding = null;
		int valueOrDefault = (_config?.talkList?.Count).GetValueOrDefault();
		if (valueOrDefault != 0 && _index >= 0 && _index < valueOrDefault)
		{
			NpcTalkBaseConfig npcTalkBaseConfig = _config.talkList[_index];
			RefreshEntryVisualSelection();
			UpdateToolbarButtons(valueOrDefault);
			if (!GodotObject.IsInstanceValid(npcTalkBaseConfig))
			{
				ClearNpc();
				SetEntryPanelsVisible(null);
				_statusLabel.Text = $"{_index + 1} / {valueOrDefault} · 空对话条目";
			}
			else
			{
				ClearNpc();
				EnsureNpc(npcTalkBaseConfig.npc);
				ApplyTalk(npcTalkBaseConfig, applyProps: true);
				BindCurrentEntry(npcTalkBaseConfig);
				SetEntryPanelsVisible(npcTalkBaseConfig);
				RefreshEntryControlValues(npcTalkBaseConfig);
				_statusLabel.Text = BuildStatus(npcTalkBaseConfig, valueOrDefault);
			}
		}
	}

	private void BindCurrentEntry(NpcTalkBaseConfig talk)
	{
		_entryBinding = new XWVisualPropertyBinding(_undoRedo, (bool _) =>
		{
			EmitEdited();
		});
		if (GodotObject.IsInstanceValid(_bubbleEditor))
		{
			_entryBinding.BindText(_bubbleEditor, talk, "text", RefreshEntryVisuals, this, "RefreshNpcEditorFromHistory");
		}
	}

	private void RefreshEntryControlValues(NpcTalkBaseConfig talk)
	{
		_updatingControls = true;
		RebuildNpcOptions(talk.npc);
		RebuildAnimationOptions(talk.anime);
		string text = talk.audio?.StripEdges() ?? "";
		_audioSelectionButton.Text = (string.IsNullOrWhiteSpace(text) ? "选择游戏 / Mod 音效" : ("音效：" + text));
		_playAudioButton.Disabled = string.IsNullOrWhiteSpace(text);
		if (talk is NpcTalkHandConfig npcTalkHandConfig)
		{
			string[] handSceneProperties = HandSceneProperties;
			foreach (string text2 in handSceneProperties)
			{
				_propSceneButtons[text2].Text = ResourceLabel(npcTalkHandConfig.Get(text2).As<Resource>());
			}
		}
		if (talk is NpcTalkTutorialConfig npcTalkTutorialConfig)
		{
			_tutorialResourceButton.Text = "教程：" + ResourceLabel(npcTalkTutorialConfig.tutorial);
		}
		_updatingControls = false;
	}

	private void RefreshEntryVisuals()
	{
		NpcTalkBaseConfig currentTalk = GetCurrentTalk();
		if (GodotObject.IsInstanceValid(currentTalk))
		{
			bool flag = EnsureNpc(currentTalk.npc);
			ApplyTalk(currentTalk, flag);
			if (flag)
			{
				CallDeferred("ShowCurrent");
				return;
			}
			_updatingControls = true;
			RebuildNpcOptions(currentTalk.npc);
			SelectAnimationOption(currentTalk.anime);
			_updatingControls = false;
			RefreshEntryVisualCard(_index);
			RefreshEntryVisualSelection();
			_statusLabel.Text = BuildStatus(currentTalk, _config.talkList.Count);
		}
	}

	public void RefreshNpcEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()))
		{
			RefreshFromConfig();
			EmitEdited();
		}
	}

	public void SelectEntry(long index)
	{
		if (!_updatingControls)
		{
			int valueOrDefault = (_config?.talkList?.Count).GetValueOrDefault();
			if (valueOrDefault != 0)
			{
				_index = Mathf.Clamp((int)index, 0, valueOrDefault - 1);
				ShowCurrent();
			}
		}
	}

	public void ShowPrevious()
	{
		if (_index > 0)
		{
			SelectEntry(_index - 1);
		}
	}

	public void ShowNext()
	{
		int valueOrDefault = (_config?.talkList?.Count).GetValueOrDefault();
		if (_index + 1 < valueOrDefault)
		{
			SelectEntry(_index + 1);
		}
	}

	public bool AddTalkEntry()
	{
		if (_standaloneEntry || !GodotObject.IsInstanceValid(_config) || _rootBinding == null)
		{
			return false;
		}
		int count = _config.talkList.Count;
		NpcTalkBaseConfig npcTalkBaseConfig = _selectedAddType switch
		{
			1 => new NpcTalkHandConfig(), 
			2 => new NpcTalkTutorialConfig(), 
			3 => new NpcTalkBuyConfig(), 
			_ => new NpcTalkBaseConfig(), 
		};
		npcTalkBaseConfig.ResourceLocalToScene = true;
		npcTalkBaseConfig.ResourceName = $"Talk_{_config.talkList.Count + 1}";
		npcTalkBaseConfig.npc = "CrazyDave";
		npcTalkBaseConfig.text = "点击这里编辑新的对话气泡";
		Array<NpcTalkBaseConfig> array = new Array<NpcTalkBaseConfig>(_config.talkList);
		array.Add(npcTalkBaseConfig);
		_index = array.Count - 1;
		ReplaceTalkList(array, "新增 NPC 对话条目");
		return _config.talkList.Count == count + 1;
	}

	public bool DuplicateTalkEntry()
	{
		NpcTalkBaseConfig currentTalk = GetCurrentTalk();
		if (_standaloneEntry || !GodotObject.IsInstanceValid(currentTalk))
		{
			return false;
		}
		int count = _config.talkList.Count;
		NpcTalkBaseConfig npcTalkBaseConfig = currentTalk.Duplicate(deep: true) as NpcTalkBaseConfig;
		if (!GodotObject.IsInstanceValid(npcTalkBaseConfig))
		{
			return false;
		}
		npcTalkBaseConfig.ResourceLocalToScene = true;
		npcTalkBaseConfig.ResourceName = (string.IsNullOrWhiteSpace(currentTalk.ResourceName) ? $"Talk_{_index + 2}" : (currentTalk.ResourceName + "_Copy"));
		Array<NpcTalkBaseConfig> array = new Array<NpcTalkBaseConfig>(_config.talkList);
		_index = Mathf.Clamp(_index + 1, 0, array.Count);
		array.Insert(_index, npcTalkBaseConfig);
		ReplaceTalkList(array, "复制 NPC 对话条目");
		return _config.talkList.Count == count + 1;
	}

	public bool RemoveTalkEntry()
	{
		if (_standaloneEntry || !HasCurrentTalk())
		{
			return false;
		}
		int count = _config.talkList.Count;
		Array<NpcTalkBaseConfig> array = new Array<NpcTalkBaseConfig>(_config.talkList);
		array.RemoveAt(_index);
		_index = ((array.Count != 0) ? Math.Min(_index, array.Count - 1) : 0);
		ReplaceTalkList(array, "移除 NPC 对话条目");
		return _config.talkList.Count == count - 1;
	}

	private void MoveTalkEntry(int direction)
	{
		if (!_standaloneEntry && HasCurrentTalk())
		{
			int num = _index + Math.Sign(direction);
			if (num >= 0 && num < _config.talkList.Count)
			{
				Array<NpcTalkBaseConfig> array = new Array<NpcTalkBaseConfig>(_config.talkList);
				NpcTalkBaseConfig value = array[_index];
				array[_index] = array[num];
				array[num] = value;
				_index = num;
				ReplaceTalkList(array, (direction < 0) ? "上移 NPC 对话条目" : "下移 NPC 对话条目");
			}
		}
	}

	private void ReplaceTalkList(Array<NpcTalkBaseConfig> next, string actionName)
	{
		if (!_standaloneEntry && GodotObject.IsInstanceValid(_config) && _rootBinding != null)
		{
			_rootBinding.SetValue(_config, "talkList", next, actionName, this, "RefreshNpcEditorFromHistory");
			RefreshFromConfig();
		}
	}

	private void ChooseJsonSource()
	{
		if (!_standaloneEntry)
		{
			_jsonSourceDialog.PopupCenteredClamped(new Vector2I(900, 620), 0.9f);
		}
	}

	private void ApplyJsonSource(string path)
	{
		if (!_standaloneEntry && _rootBinding != null && !string.IsNullOrWhiteSpace(path))
		{
			Json json = ResourceLoader.Load<Json>(path, "", ResourceLoader.CacheMode.Ignore);
			if (!GodotObject.IsInstanceValid(json))
			{
				XWEditorInterface.Instance?.ShowToast("无法加载 NPC 对话 JSON：" + path);
				return;
			}
			_index = 0;
			_rootBinding.SetValue(_config, "data", json, "更换 NPC 对话 JSON 来源", this, "RefreshNpcEditorFromHistory");
			RefreshFromConfig();
		}
	}

	private void SelectKnownNpc(long index)
	{
		if (!_updatingControls && index >= 0 && index < _npcOption.ItemCount)
		{
			SetEntryString("npc", _npcOption.GetItemMetadata((int)index).AsString(), "更换 NPC 演员");
		}
	}

	private void SelectKnownAnimation(long index)
	{
		if (!_updatingControls && index >= 0 && index < _animeOption.ItemCount)
		{
			SetEntryString("anime", _animeOption.GetItemMetadata((int)index).AsString(), "更换 NPC 动画");
		}
	}

	private void SetEntryString(StringName property, string value, string actionName)
	{
		NpcTalkBaseConfig currentTalk = GetCurrentTalk();
		if (GodotObject.IsInstanceValid(currentTalk) && _entryBinding != null)
		{
			_entryBinding.SetValue(currentTalk, property, value ?? "", actionName, this, "RefreshNpcEditorFromHistory");
			ShowCurrent();
		}
	}

	private void ShowAudioPicker()
	{
		NpcTalkBaseConfig currentTalk = GetCurrentTalk();
		if (GodotObject.IsInstanceValid(currentTalk))
		{
			EnsureAudioPicker();
			_audioPicker?.Open(XWGameplayResourceKind.Audio, currentTalk.audio, ApplyAudioChoice, (XWGameplayResourceChoice choice) => choice.Kind == XWGameplayResourceKind.Audio);
		}
	}

	private void EnsureAudioPicker()
	{
		if (!GodotObject.IsInstanceValid(_audioPicker))
		{
			_audioPicker = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_audioPicker))
			{
				AddChild(_audioPicker, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void ApplyAudioChoice(XWGameplayResourceChoice choice)
	{
		if (!(choice == null) && choice.Kind == XWGameplayResourceKind.Audio)
		{
			SetEntryString("audio", choice.Key, "更换 NPC 对话音效");
		}
	}

	private void PlayCurrentAudio()
	{
		NpcTalkBaseConfig currentTalk = GetCurrentTalk();
		if (!GodotObject.IsInstanceValid(currentTalk))
		{
			return;
		}
		_directAudioPlayer.Stop();
		string text = currentTalk.audio?.StripEdges() ?? "";
		if (string.IsNullOrWhiteSpace(text))
		{
			_statusLabel.Text = "当前条目没有配置音频。";
			return;
		}
		if (ResourceLoader.Exists(text))
		{
			_directAudioPlayer.Stream = ResourceLoader.Load<AudioStream>(text, null, ResourceLoader.CacheMode.Reuse);
			if (GodotObject.IsInstanceValid(_directAudioPlayer.Stream))
			{
				_directAudioPlayer.Play();
				_statusLabel.Text = "正在试听：" + text.GetFile();
				return;
			}
		}
		AudioStream audioStream = LoadSelectedAudioStream(text);
		if (GodotObject.IsInstanceValid(audioStream))
		{
			_directAudioPlayer.Stream = audioStream;
			_directAudioPlayer.Play();
			_statusLabel.Text = "正在试听：" + text;
		}
		else if (GodotObject.IsInstanceValid(AudioManager.Instance))
		{
			AudioManager.Instance.AudioPlay(text, AudioManagerEnum.TYPE.SFX, 0.0, once: true, pauseAlive: true);
			_statusLabel.Text = "正在通过游戏音频表试听：" + text;
		}
		else
		{
			_statusLabel.Text = "无法试听音频：" + text;
		}
	}

	private AudioStream LoadSelectedAudioStream(string audioKey)
	{
		EnsureAudioPicker();
		if (!GodotObject.IsInstanceValid(_audioPicker))
		{
			return null;
		}
		return _audioPicker.LoadAudioStream(audioKey);
	}

	private void ChoosePropScene(string property)
	{
		if (GetCurrentTalk() is NpcTalkHandConfig && System.Array.IndexOf(HandSceneProperties, property) >= 0)
		{
			_pendingPropProperty = property;
			_propSceneDialog.PopupCenteredClamped(new Vector2I(900, 620), 0.9f);
		}
	}

	private void ApplyPropScene(string path)
	{
		if (!string.IsNullOrWhiteSpace(_pendingPropProperty) && !string.IsNullOrWhiteSpace(path))
		{
			PackedScene packedScene = ResourceLoader.Load<PackedScene>(path, "", ResourceLoader.CacheMode.Ignore);
			if (!GodotObject.IsInstanceValid(packedScene))
			{
				XWEditorInterface.Instance?.ShowToast("无法加载 NPC 道具场景：" + path);
				return;
			}
			string pendingPropProperty = _pendingPropProperty;
			_pendingPropProperty = "";
			SetEntryResource(pendingPropProperty, packedScene, "更换 NPC " + pendingPropProperty);
		}
	}

	private void ChooseTutorialResource()
	{
		if (GetCurrentTalk() is NpcTalkTutorialConfig)
		{
			_tutorialResourceDialog.PopupCenteredClamped(new Vector2I(900, 620), 0.9f);
		}
	}

	private void ApplyTutorialResource(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			TutorialConfig tutorialConfig = ResourceLoader.Load<TutorialConfig>(path, "", ResourceLoader.CacheMode.Ignore);
			if (!GodotObject.IsInstanceValid(tutorialConfig))
			{
				XWEditorInterface.Instance?.ShowToast("无法加载教程资源：" + path);
			}
			else
			{
				SetEntryResource("tutorial", tutorialConfig, "更换 NPC 对话教程引用");
			}
		}
	}

	private void SetEntryResource(StringName property, Resource resource, string actionName)
	{
		NpcTalkBaseConfig currentTalk = GetCurrentTalk();
		if (GodotObject.IsInstanceValid(currentTalk) && _entryBinding != null)
		{
			_entryBinding.SetValue(currentTalk, property, resource, actionName, this, "RefreshNpcEditorFromHistory");
			ClearNpc();
			ShowCurrent();
		}
	}

	private bool EnsureNpc(string npcKey)
	{
		string text = (string.Equals(npcKey, "WeiWeiMi", StringComparison.OrdinalIgnoreCase) ? "WeiWeiMi" : "CrazyDave");
		if (GodotObject.IsInstanceValid(_npc) && _npcSceneKey == text)
		{
			return false;
		}
		ClearNpc();
		_npc = ResourceLoader.Load<PackedScene>((text == "WeiWeiMi") ? "res://Prefab/Npc/WeiWeiMi/WeiWeiMi.tscn" : "res://Prefab/Npc/CrazyDave/NpcCrazyDave.tscn", null, ResourceLoader.CacheMode.Reuse)?.Instantiate<NpcBase>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(_npc))
		{
			return false;
		}
		_npcSceneKey = text;
		_npc.Name = "EditableNpc";
		_npc.Position = Vector2.Zero;
		_npc.Scale = Vector2.One;
		ColorRect nodeOrNull = _npc.GetNodeOrNull<ColorRect>("CanvasLayer/ColorRect");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			nodeOrNull.MouseFilter = MouseFilterEnum.Ignore;
		}
		_runtimeControl.AddNpc(_npc);
		AttachBubbleEditor();
		return true;
	}

	private void AttachBubbleEditor()
	{
		_bubbleEditor = null;
		if (!GodotObject.IsInstanceValid(_npc))
		{
			return;
		}
		TextureRect nodeOrNull = _npc.GetNodeOrNull<TextureRect>("%TalkBubble");
		Label nodeOrNull2 = _npc.GetNodeOrNull<Label>("%TalkLabel");
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			if (GodotObject.IsInstanceValid(nodeOrNull2))
			{
				nodeOrNull2.Visible = false;
			}
			if (_bubbleTextEditorScene == null)
			{
				_bubbleTextEditorScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWNpcTalkBubbleTextEditor.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			_bubbleEditor = _bubbleTextEditorScene?.Instantiate<TextEdit>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(_bubbleEditor))
			{
				nodeOrNull.AddChild(_bubbleEditor, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void ApplyTalk(NpcTalkBaseConfig talk, bool applyProps)
	{
		if (GodotObject.IsInstanceValid(_npc))
		{
			TextureRect nodeOrNull = _npc.GetNodeOrNull<TextureRect>("%TalkBubble");
			if (GodotObject.IsInstanceValid(nodeOrNull))
			{
				nodeOrNull.Visible = true;
				nodeOrNull.Scale = Vector2.One;
			}
			if (GodotObject.IsInstanceValid(_bubbleEditor) && _bubbleEditor.Text != (talk.text ?? ""))
			{
				_bubbleEditor.Text = talk.text ?? "";
			}
			if (GodotObject.IsInstanceValid(_npc.sprite) && !string.IsNullOrWhiteSpace(talk.anime))
			{
				_npc.sprite.SetAnimation(talk.anime);
			}
			if (applyProps && talk is NpcTalkHandConfig hand)
			{
				_npc.Hand(hand);
			}
		}
	}

	private void RebuildNpcOptions(string current)
	{
		_npcOption.Clear();
		string[] knownNpcKeys = KnownNpcKeys;
		foreach (string text in knownNpcKeys)
		{
			_npcOption.AddItem(text);
			_npcOption.SetItemMetadata(_npcOption.ItemCount - 1, text);
		}
		if (!string.IsNullOrWhiteSpace(current) && System.Array.IndexOf(KnownNpcKeys, current) < 0)
		{
			_npcOption.AddItem("自定义：" + current);
			_npcOption.SetItemMetadata(_npcOption.ItemCount - 1, current);
		}
		SelectOptionByMetadata(_npcOption, string.IsNullOrWhiteSpace(current) ? "CrazyDave" : current);
		_npcVisualGallery?.Rebuild();
	}

	private void RebuildAnimationOptions(string current)
	{
		_animeOption.Clear();
		_animeOption.AddItem("未设置");
		_animeOption.SetItemMetadata(0, "");
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		if (GodotObject.IsInstanceValid(_npc?.sprite?.flashAnimeData) && _npc.sprite.flashAnimeData.clips != null)
		{
			foreach (Variant key in _npc.sprite.flashAnimeData.clips.Keys)
			{
				string text = key.AsString();
				if (!string.IsNullOrWhiteSpace(text) && hashSet.Add(text))
				{
					_animeOption.AddItem(text);
					_animeOption.SetItemMetadata(_animeOption.ItemCount - 1, text);
				}
			}
		}
		if (!string.IsNullOrWhiteSpace(current) && hashSet.Add(current))
		{
			_animeOption.AddItem("自定义：" + current);
			_animeOption.SetItemMetadata(_animeOption.ItemCount - 1, current);
		}
		SelectAnimationOption(current);
		_animeVisualGallery?.Rebuild();
	}

	private void SelectAnimationOption(string current)
	{
		SelectOptionByMetadata(_animeOption, current ?? "");
		_animeVisualGallery?.RefreshSelection();
	}

	private static void SelectOptionByMetadata(OptionButton option, string value)
	{
		for (int i = 0; i < option.ItemCount; i++)
		{
			if (option.GetItemMetadata(i).AsString() == value)
			{
				option.Select(i);
				return;
			}
		}
		if (option.ItemCount > 0)
		{
			option.Select(0);
		}
	}

	private void SetEntryPanelsVisible(NpcTalkBaseConfig talk)
	{
		_handPropsPanel.Visible = talk is NpcTalkHandConfig;
		_tutorialPanel.Visible = talk is NpcTalkTutorialConfig;
	}

	private void UpdateToolbarButtons(int count)
	{
		_previousButton.Disabled = _index <= 0;
		_nextButton.Disabled = _index >= count - 1;
		_duplicateButton.Disabled = _standaloneEntry;
		_removeButton.Disabled = _standaloneEntry;
		_moveUpButton.Disabled = _standaloneEntry || _index <= 0;
		_moveDownButton.Disabled = _standaloneEntry || _index >= count - 1;
	}

	private NpcTalkBaseConfig GetCurrentTalk()
	{
		int valueOrDefault = (_config?.talkList?.Count).GetValueOrDefault();
		if (_index < 0 || _index >= valueOrDefault)
		{
			return null;
		}
		return _config.talkList[_index];
	}

	private bool HasCurrentTalk()
	{
		return GodotObject.IsInstanceValid(GetCurrentTalk());
	}

	private void EmitEdited()
	{
		GetCurrentTalk()?.EmitChanged();
		_config?.EmitChanged();
		EmitSignal(SignalName.TalkEdited);
	}

	private void ClearNpc()
	{
		_bubbleEditor = null;
		if (GodotObject.IsInstanceValid(_npc))
		{
			Node parent = _npc.GetParent();
			if (GodotObject.IsInstanceValid(parent))
			{
				parent.RemoveChild(_npc);
			}
			_npc.QueueFree();
		}
		_npc = null;
		_npcSceneKey = "";
	}

	private static string BuildEntryLabel(int index, NpcTalkBaseConfig talk)
	{
		if (GodotObject.IsInstanceValid(talk))
		{
			string text = (talk.text ?? "").Replace('\n', ' ').Replace('\r', ' ').StripEdges();
			if (text.Length > 18)
			{
				text = text.Substring(0, 18) + "…";
			}
			return $"{index + 1}. {EntryTypeName(talk)} · {DisplayNpc(talk.npc)} · {text}";
		}
		return $"{index + 1}. 空条目";
	}

	private static string BuildStatus(NpcTalkBaseConfig talk, int count)
	{
		return $"{EntryTypeName(talk)} · {DisplayNpc(talk.npc)} · 点击气泡改文本 · 动画 {DisplayValue(talk.anime)} · 音频 {DisplayValue(talk.audio)} · {count} 条";
	}

	private static string EntryTypeName(NpcTalkBaseConfig talk)
	{
		if (!(talk is NpcTalkHandConfig))
		{
			if (!(talk is NpcTalkTutorialConfig))
			{
				if (talk is NpcTalkBuyConfig)
				{
					return "购买对话";
				}
				return "普通气泡";
			}
			return "教程衔接";
		}
		return "道具表演";
	}

	private static string ResourceLabel(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "未设置";
		}
		if (!string.IsNullOrWhiteSpace(resource.ResourcePath))
		{
			return resource.ResourcePath.GetFile();
		}
		if (!string.IsNullOrWhiteSpace(resource.ResourceName))
		{
			return resource.ResourceName;
		}
		return resource.GetType().Name;
	}

	private static string DisplayNpc(string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return "CrazyDave";
	}

	private static string DisplayValue(string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return "未设置";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(68)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EditConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "editingRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindSceneNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateFixedOptions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildTypeVisualCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildEntryVisualCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateVisualChoiceCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigureEntryCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "card", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshEntryVisualCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshEntryVisualSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenEntryVisualPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenAddTypeVisualPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectAddTypeVisual, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "typeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCurrentText, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetEntryTypeIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "typeIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EntryTypeIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildEntryVisualDetail, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConnectSceneSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindRootFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshRootHeader, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshFromConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowCurrent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindCurrentEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshEntryControlValues, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshEntryVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshNpcEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowPrevious, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ShowNext, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddTalkEntry, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DuplicateTalkEntry, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveTalkEntry, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveTalkEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReplaceTalkList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChooseJsonSource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyJsonSource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectKnownNpc, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectKnownAnimation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetEntryString, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowAudioPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureAudioPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayCurrentAudio, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.LoadSelectedAudioStream, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("AudioStream"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "audioKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChoosePropScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyPropScene, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ChooseTutorialResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyTutorialResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetEntryResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureNpc, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "npcKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AttachBubbleEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyTalk, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Bool, "applyProps", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildNpcOptions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "current", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildAnimationOptions, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "current", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectAnimationOption, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "current", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectOptionByMetadata, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetEntryPanelsVisible, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateToolbarButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentTalk, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCurrentTalk, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EmitEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearNpc, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildEntryLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildStatus, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EntryTypeName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "talk", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResourceLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisplayNpc, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisplayValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.EditConfig && args.Count == 2)
		{
			EditConfig(VariantUtils.ConvertTo<NpcTalkConfig>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindSceneNodes && args.Count == 0)
		{
			BindSceneNodes();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateFixedOptions && args.Count == 0)
		{
			PopulateFixedOptions();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildTypeVisualCards && args.Count == 0)
		{
			BuildTypeVisualCards();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildEntryVisualCards && args.Count == 0)
		{
			RebuildEntryVisualCards();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateVisualChoiceCard && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<XWNpcTalkVisualChoiceCard>(CreateVisualChoiceCard());
			return true;
		}
		if (method == MethodName.ConfigureEntryCard && args.Count == 3)
		{
			ConfigureEntryCard(VariantUtils.ConvertTo<XWNpcTalkVisualChoiceCard>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEntryVisualCard && args.Count == 1)
		{
			RefreshEntryVisualCard(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEntryVisualSelection && args.Count == 0)
		{
			RefreshEntryVisualSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenEntryVisualPicker && args.Count == 0)
		{
			OpenEntryVisualPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenAddTypeVisualPicker && args.Count == 0)
		{
			OpenAddTypeVisualPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectAddTypeVisual && args.Count == 1)
		{
			SelectAddTypeVisual(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCurrentText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(SetCurrentText(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.GetEntryTypeIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetEntryTypeIcon(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.EntryTypeIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(EntryTypeIndex(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildEntryVisualDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildEntryVisualDetail(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ConnectSceneSignals && args.Count == 0)
		{
			ConnectSceneSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.BindRootFields && args.Count == 0)
		{
			BindRootFields();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshRootHeader && args.Count == 0)
		{
			RefreshRootHeader();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshFromConfig && args.Count == 0)
		{
			RefreshFromConfig();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowCurrent && args.Count == 0)
		{
			ShowCurrent();
			ret = default;
			return true;
		}
		if (method == MethodName.BindCurrentEntry && args.Count == 1)
		{
			BindCurrentEntry(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEntryControlValues && args.Count == 1)
		{
			RefreshEntryControlValues(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEntryVisuals && args.Count == 0)
		{
			RefreshEntryVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshNpcEditorFromHistory && args.Count == 0)
		{
			RefreshNpcEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectEntry && args.Count == 1)
		{
			SelectEntry(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowPrevious && args.Count == 0)
		{
			ShowPrevious();
			ret = default;
			return true;
		}
		if (method == MethodName.ShowNext && args.Count == 0)
		{
			ShowNext();
			ret = default;
			return true;
		}
		if (method == MethodName.AddTalkEntry && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(AddTalkEntry());
			return true;
		}
		if (method == MethodName.DuplicateTalkEntry && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(DuplicateTalkEntry());
			return true;
		}
		if (method == MethodName.RemoveTalkEntry && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(RemoveTalkEntry());
			return true;
		}
		if (method == MethodName.MoveTalkEntry && args.Count == 1)
		{
			MoveTalkEntry(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceTalkList && args.Count == 2)
		{
			ReplaceTalkList(VariantUtils.ConvertToArray<NpcTalkBaseConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChooseJsonSource && args.Count == 0)
		{
			ChooseJsonSource();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyJsonSource && args.Count == 1)
		{
			ApplyJsonSource(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectKnownNpc && args.Count == 1)
		{
			SelectKnownNpc(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectKnownAnimation && args.Count == 1)
		{
			SelectKnownAnimation(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetEntryString && args.Count == 3)
		{
			SetEntryString(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowAudioPicker && args.Count == 0)
		{
			ShowAudioPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureAudioPicker && args.Count == 0)
		{
			EnsureAudioPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayCurrentAudio && args.Count == 0)
		{
			PlayCurrentAudio();
			ret = default;
			return true;
		}
		if (method == MethodName.LoadSelectedAudioStream && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AudioStream>(LoadSelectedAudioStream(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ChoosePropScene && args.Count == 1)
		{
			ChoosePropScene(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyPropScene && args.Count == 1)
		{
			ApplyPropScene(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ChooseTutorialResource && args.Count == 0)
		{
			ChooseTutorialResource();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyTutorialResource && args.Count == 1)
		{
			ApplyTutorialResource(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetEntryResource && args.Count == 3)
		{
			SetEntryResource(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureNpc && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureNpc(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AttachBubbleEditor && args.Count == 0)
		{
			AttachBubbleEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyTalk && args.Count == 2)
		{
			ApplyTalk(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildNpcOptions && args.Count == 1)
		{
			RebuildNpcOptions(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildAnimationOptions && args.Count == 1)
		{
			RebuildAnimationOptions(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectAnimationOption && args.Count == 1)
		{
			SelectAnimationOption(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectOptionByMetadata && args.Count == 2)
		{
			SelectOptionByMetadata(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetEntryPanelsVisible && args.Count == 1)
		{
			SetEntryPanelsVisible(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateToolbarButtons && args.Count == 1)
		{
			UpdateToolbarButtons(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentTalk && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<NpcTalkBaseConfig>(GetCurrentTalk());
			return true;
		}
		if (method == MethodName.HasCurrentTalk && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCurrentTalk());
			return true;
		}
		if (method == MethodName.EmitEdited && args.Count == 0)
		{
			EmitEdited();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearNpc && args.Count == 0)
		{
			ClearNpc();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildEntryLabel && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildEntryLabel(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildStatus && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildStatus(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.EntryTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EntryTypeName(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResourceLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResourceLabel(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.DisplayNpc && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DisplayNpc(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DisplayValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DisplayValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.GetEntryTypeIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(GetEntryTypeIcon(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.EntryTypeIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(EntryTypeIndex(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildEntryVisualDetail && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildEntryVisualDetail(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectOptionByMetadata && args.Count == 2)
		{
			SelectOptionByMetadata(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildEntryLabel && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildEntryLabel(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildStatus && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildStatus(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.EntryTypeName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EntryTypeName(VariantUtils.ConvertTo<NpcTalkBaseConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResourceLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ResourceLabel(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.DisplayNpc && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DisplayNpc(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DisplayValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DisplayValue(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.EditConfig)
		{
			return true;
		}
		if (method == MethodName.BindSceneNodes)
		{
			return true;
		}
		if (method == MethodName.PopulateFixedOptions)
		{
			return true;
		}
		if (method == MethodName.BuildTypeVisualCards)
		{
			return true;
		}
		if (method == MethodName.RebuildEntryVisualCards)
		{
			return true;
		}
		if (method == MethodName.CreateVisualChoiceCard)
		{
			return true;
		}
		if (method == MethodName.ConfigureEntryCard)
		{
			return true;
		}
		if (method == MethodName.RefreshEntryVisualCard)
		{
			return true;
		}
		if (method == MethodName.RefreshEntryVisualSelection)
		{
			return true;
		}
		if (method == MethodName.OpenEntryVisualPicker)
		{
			return true;
		}
		if (method == MethodName.OpenAddTypeVisualPicker)
		{
			return true;
		}
		if (method == MethodName.SelectAddTypeVisual)
		{
			return true;
		}
		if (method == MethodName.SetCurrentText)
		{
			return true;
		}
		if (method == MethodName.GetEntryTypeIcon)
		{
			return true;
		}
		if (method == MethodName.EntryTypeIndex)
		{
			return true;
		}
		if (method == MethodName.BuildEntryVisualDetail)
		{
			return true;
		}
		if (method == MethodName.ConnectSceneSignals)
		{
			return true;
		}
		if (method == MethodName.BindRootFields)
		{
			return true;
		}
		if (method == MethodName.RefreshRootHeader)
		{
			return true;
		}
		if (method == MethodName.RefreshFromConfig)
		{
			return true;
		}
		if (method == MethodName.ShowCurrent)
		{
			return true;
		}
		if (method == MethodName.BindCurrentEntry)
		{
			return true;
		}
		if (method == MethodName.RefreshEntryControlValues)
		{
			return true;
		}
		if (method == MethodName.RefreshEntryVisuals)
		{
			return true;
		}
		if (method == MethodName.RefreshNpcEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.SelectEntry)
		{
			return true;
		}
		if (method == MethodName.ShowPrevious)
		{
			return true;
		}
		if (method == MethodName.ShowNext)
		{
			return true;
		}
		if (method == MethodName.AddTalkEntry)
		{
			return true;
		}
		if (method == MethodName.DuplicateTalkEntry)
		{
			return true;
		}
		if (method == MethodName.RemoveTalkEntry)
		{
			return true;
		}
		if (method == MethodName.MoveTalkEntry)
		{
			return true;
		}
		if (method == MethodName.ReplaceTalkList)
		{
			return true;
		}
		if (method == MethodName.ChooseJsonSource)
		{
			return true;
		}
		if (method == MethodName.ApplyJsonSource)
		{
			return true;
		}
		if (method == MethodName.SelectKnownNpc)
		{
			return true;
		}
		if (method == MethodName.SelectKnownAnimation)
		{
			return true;
		}
		if (method == MethodName.SetEntryString)
		{
			return true;
		}
		if (method == MethodName.ShowAudioPicker)
		{
			return true;
		}
		if (method == MethodName.EnsureAudioPicker)
		{
			return true;
		}
		if (method == MethodName.PlayCurrentAudio)
		{
			return true;
		}
		if (method == MethodName.LoadSelectedAudioStream)
		{
			return true;
		}
		if (method == MethodName.ChoosePropScene)
		{
			return true;
		}
		if (method == MethodName.ApplyPropScene)
		{
			return true;
		}
		if (method == MethodName.ChooseTutorialResource)
		{
			return true;
		}
		if (method == MethodName.ApplyTutorialResource)
		{
			return true;
		}
		if (method == MethodName.SetEntryResource)
		{
			return true;
		}
		if (method == MethodName.EnsureNpc)
		{
			return true;
		}
		if (method == MethodName.AttachBubbleEditor)
		{
			return true;
		}
		if (method == MethodName.ApplyTalk)
		{
			return true;
		}
		if (method == MethodName.RebuildNpcOptions)
		{
			return true;
		}
		if (method == MethodName.RebuildAnimationOptions)
		{
			return true;
		}
		if (method == MethodName.SelectAnimationOption)
		{
			return true;
		}
		if (method == MethodName.SelectOptionByMetadata)
		{
			return true;
		}
		if (method == MethodName.SetEntryPanelsVisible)
		{
			return true;
		}
		if (method == MethodName.UpdateToolbarButtons)
		{
			return true;
		}
		if (method == MethodName.GetCurrentTalk)
		{
			return true;
		}
		if (method == MethodName.HasCurrentTalk)
		{
			return true;
		}
		if (method == MethodName.EmitEdited)
		{
			return true;
		}
		if (method == MethodName.ClearNpc)
		{
			return true;
		}
		if (method == MethodName.BuildEntryLabel)
		{
			return true;
		}
		if (method == MethodName.BuildStatus)
		{
			return true;
		}
		if (method == MethodName.EntryTypeName)
		{
			return true;
		}
		if (method == MethodName.ResourceLabel)
		{
			return true;
		}
		if (method == MethodName.DisplayNpc)
		{
			return true;
		}
		if (method == MethodName.DisplayValue)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._config)
		{
			_config = VariantUtils.ConvertTo<NpcTalkConfig>(in value);
			return true;
		}
		if (name == PropertyName._editingRoot)
		{
			_editingRoot = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._standaloneEntry)
		{
			_standaloneEntry = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._index)
		{
			_index = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._runtimeControl)
		{
			_runtimeControl = VariantUtils.ConvertTo<NpcTalkControl>(in value);
			return true;
		}
		if (name == PropertyName._npc)
		{
			_npc = VariantUtils.ConvertTo<NpcBase>(in value);
			return true;
		}
		if (name == PropertyName._npcSceneKey)
		{
			_npcSceneKey = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._bubbleEditor)
		{
			_bubbleEditor = VariantUtils.ConvertTo<TextEdit>(in value);
			return true;
		}
		if (name == PropertyName._undoRedo)
		{
			_undoRedo = VariantUtils.ConvertTo<XWUndoRedoManager>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._pendingPropProperty)
		{
			_pendingPropProperty = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._selectedAddType)
		{
			_selectedAddType = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._saveKeyLineEdit)
		{
			_saveKeyLineEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._jsonSourceButton)
		{
			_jsonSourceButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._entryVisualButton)
		{
			_entryVisualButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._addTypeVisualButton)
		{
			_addTypeVisualButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._entryPickerPopup)
		{
			_entryPickerPopup = VariantUtils.ConvertTo<PopupPanel>(in value);
			return true;
		}
		if (name == PropertyName._addTypePopup)
		{
			_addTypePopup = VariantUtils.ConvertTo<PopupPanel>(in value);
			return true;
		}
		if (name == PropertyName._entryPickerGrid)
		{
			_entryPickerGrid = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._addTypeGrid)
		{
			_addTypeGrid = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._stageFrame)
		{
			_stageFrame = VariantUtils.ConvertTo<XWAspectScaledPreviewHost>(in value);
			return true;
		}
		if (name == PropertyName._previousButton)
		{
			_previousButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._nextButton)
		{
			_nextButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._addButton)
		{
			_addButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._duplicateButton)
		{
			_duplicateButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._removeButton)
		{
			_removeButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveUpButton)
		{
			_moveUpButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._moveDownButton)
		{
			_moveDownButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._emptyLabel)
		{
			_emptyLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			_statusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._npcOption)
		{
			_npcOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._animeOption)
		{
			_animeOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._audioSelectionButton)
		{
			_audioSelectionButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._playAudioButton)
		{
			_playAudioButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._audioPicker)
		{
			_audioPicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._handPropsPanel)
		{
			_handPropsPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._tutorialPanel)
		{
			_tutorialPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._tutorialResourceButton)
		{
			_tutorialResourceButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._jsonSourceDialog)
		{
			_jsonSourceDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._propSceneDialog)
		{
			_propSceneDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._tutorialResourceDialog)
		{
			_tutorialResourceDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._directAudioPlayer)
		{
			_directAudioPlayer = VariantUtils.ConvertTo<AudioStreamPlayer>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.EntryCount)
		{
			from = EntryCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.CurrentEntryIndex)
		{
			from = CurrentEntryIndex;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.EntryVisualCardCount)
		{
			from = EntryVisualCardCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.EntryTypeCardCount)
		{
			from = EntryTypeCardCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.SelectedAddType)
		{
			from = SelectedAddType;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.ResponsiveStage)
		{
			value = VariantUtils.CreateFrom<XWAspectScaledPreviewHost>(ResponsiveStage);
			return true;
		}
		if (name == PropertyName._config)
		{
			value = VariantUtils.CreateFrom(in _config);
			return true;
		}
		if (name == PropertyName._editingRoot)
		{
			value = VariantUtils.CreateFrom(in _editingRoot);
			return true;
		}
		if (name == PropertyName._standaloneEntry)
		{
			value = VariantUtils.CreateFrom(in _standaloneEntry);
			return true;
		}
		if (name == PropertyName._index)
		{
			value = VariantUtils.CreateFrom(in _index);
			return true;
		}
		if (name == PropertyName._runtimeControl)
		{
			value = VariantUtils.CreateFrom(in _runtimeControl);
			return true;
		}
		if (name == PropertyName._npc)
		{
			value = VariantUtils.CreateFrom(in _npc);
			return true;
		}
		if (name == PropertyName._npcSceneKey)
		{
			value = VariantUtils.CreateFrom(in _npcSceneKey);
			return true;
		}
		if (name == PropertyName._bubbleEditor)
		{
			value = VariantUtils.CreateFrom(in _bubbleEditor);
			return true;
		}
		if (name == PropertyName._undoRedo)
		{
			value = VariantUtils.CreateFrom(in _undoRedo);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._pendingPropProperty)
		{
			value = VariantUtils.CreateFrom(in _pendingPropProperty);
			return true;
		}
		if (name == PropertyName._selectedAddType)
		{
			value = VariantUtils.CreateFrom(in _selectedAddType);
			return true;
		}
		if (name == PropertyName._saveKeyLineEdit)
		{
			value = VariantUtils.CreateFrom(in _saveKeyLineEdit);
			return true;
		}
		if (name == PropertyName._jsonSourceButton)
		{
			value = VariantUtils.CreateFrom(in _jsonSourceButton);
			return true;
		}
		if (name == PropertyName._entryVisualButton)
		{
			value = VariantUtils.CreateFrom(in _entryVisualButton);
			return true;
		}
		if (name == PropertyName._addTypeVisualButton)
		{
			value = VariantUtils.CreateFrom(in _addTypeVisualButton);
			return true;
		}
		if (name == PropertyName._entryPickerPopup)
		{
			value = VariantUtils.CreateFrom(in _entryPickerPopup);
			return true;
		}
		if (name == PropertyName._addTypePopup)
		{
			value = VariantUtils.CreateFrom(in _addTypePopup);
			return true;
		}
		if (name == PropertyName._entryPickerGrid)
		{
			value = VariantUtils.CreateFrom(in _entryPickerGrid);
			return true;
		}
		if (name == PropertyName._addTypeGrid)
		{
			value = VariantUtils.CreateFrom(in _addTypeGrid);
			return true;
		}
		if (name == PropertyName._stageFrame)
		{
			value = VariantUtils.CreateFrom(in _stageFrame);
			return true;
		}
		if (name == PropertyName._previousButton)
		{
			value = VariantUtils.CreateFrom(in _previousButton);
			return true;
		}
		if (name == PropertyName._nextButton)
		{
			value = VariantUtils.CreateFrom(in _nextButton);
			return true;
		}
		if (name == PropertyName._addButton)
		{
			value = VariantUtils.CreateFrom(in _addButton);
			return true;
		}
		if (name == PropertyName._duplicateButton)
		{
			value = VariantUtils.CreateFrom(in _duplicateButton);
			return true;
		}
		if (name == PropertyName._removeButton)
		{
			value = VariantUtils.CreateFrom(in _removeButton);
			return true;
		}
		if (name == PropertyName._moveUpButton)
		{
			value = VariantUtils.CreateFrom(in _moveUpButton);
			return true;
		}
		if (name == PropertyName._moveDownButton)
		{
			value = VariantUtils.CreateFrom(in _moveDownButton);
			return true;
		}
		if (name == PropertyName._emptyLabel)
		{
			value = VariantUtils.CreateFrom(in _emptyLabel);
			return true;
		}
		if (name == PropertyName._statusLabel)
		{
			value = VariantUtils.CreateFrom(in _statusLabel);
			return true;
		}
		if (name == PropertyName._npcOption)
		{
			value = VariantUtils.CreateFrom(in _npcOption);
			return true;
		}
		if (name == PropertyName._animeOption)
		{
			value = VariantUtils.CreateFrom(in _animeOption);
			return true;
		}
		if (name == PropertyName._audioSelectionButton)
		{
			value = VariantUtils.CreateFrom(in _audioSelectionButton);
			return true;
		}
		if (name == PropertyName._playAudioButton)
		{
			value = VariantUtils.CreateFrom(in _playAudioButton);
			return true;
		}
		if (name == PropertyName._audioPicker)
		{
			value = VariantUtils.CreateFrom(in _audioPicker);
			return true;
		}
		if (name == PropertyName._handPropsPanel)
		{
			value = VariantUtils.CreateFrom(in _handPropsPanel);
			return true;
		}
		if (name == PropertyName._tutorialPanel)
		{
			value = VariantUtils.CreateFrom(in _tutorialPanel);
			return true;
		}
		if (name == PropertyName._tutorialResourceButton)
		{
			value = VariantUtils.CreateFrom(in _tutorialResourceButton);
			return true;
		}
		if (name == PropertyName._jsonSourceDialog)
		{
			value = VariantUtils.CreateFrom(in _jsonSourceDialog);
			return true;
		}
		if (name == PropertyName._propSceneDialog)
		{
			value = VariantUtils.CreateFrom(in _propSceneDialog);
			return true;
		}
		if (name == PropertyName._tutorialResourceDialog)
		{
			value = VariantUtils.CreateFrom(in _tutorialResourceDialog);
			return true;
		}
		if (name == PropertyName._directAudioPlayer)
		{
			value = VariantUtils.CreateFrom(in _directAudioPlayer);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._config, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._standaloneEntry, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._index, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._npc, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._npcSceneKey, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._bubbleEditor, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._undoRedo, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingPropProperty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedAddType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._saveKeyLineEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._jsonSourceButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._entryVisualButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addTypeVisualButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._entryPickerPopup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addTypePopup, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._entryPickerGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addTypeGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageFrame, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previousButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nextButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._addButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._duplicateButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._removeButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveUpButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._moveDownButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._statusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._npcOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._animeOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioSelectionButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._playAudioButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._handPropsPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._tutorialPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._tutorialResourceButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._jsonSourceDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._propSceneDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._tutorialResourceDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._directAudioPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.EntryCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CurrentEntryIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.EntryVisualCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.EntryTypeCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.SelectedAddType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.ResponsiveStage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._config, Variant.From(in _config));
		info.AddProperty(PropertyName._editingRoot, Variant.From(in _editingRoot));
		info.AddProperty(PropertyName._standaloneEntry, Variant.From(in _standaloneEntry));
		info.AddProperty(PropertyName._index, Variant.From(in _index));
		info.AddProperty(PropertyName._runtimeControl, Variant.From(in _runtimeControl));
		info.AddProperty(PropertyName._npc, Variant.From(in _npc));
		info.AddProperty(PropertyName._npcSceneKey, Variant.From(in _npcSceneKey));
		info.AddProperty(PropertyName._bubbleEditor, Variant.From(in _bubbleEditor));
		info.AddProperty(PropertyName._undoRedo, Variant.From(in _undoRedo));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._pendingPropProperty, Variant.From(in _pendingPropProperty));
		info.AddProperty(PropertyName._selectedAddType, Variant.From(in _selectedAddType));
		info.AddProperty(PropertyName._saveKeyLineEdit, Variant.From(in _saveKeyLineEdit));
		info.AddProperty(PropertyName._jsonSourceButton, Variant.From(in _jsonSourceButton));
		info.AddProperty(PropertyName._entryVisualButton, Variant.From(in _entryVisualButton));
		info.AddProperty(PropertyName._addTypeVisualButton, Variant.From(in _addTypeVisualButton));
		info.AddProperty(PropertyName._entryPickerPopup, Variant.From(in _entryPickerPopup));
		info.AddProperty(PropertyName._addTypePopup, Variant.From(in _addTypePopup));
		info.AddProperty(PropertyName._entryPickerGrid, Variant.From(in _entryPickerGrid));
		info.AddProperty(PropertyName._addTypeGrid, Variant.From(in _addTypeGrid));
		info.AddProperty(PropertyName._stageFrame, Variant.From(in _stageFrame));
		info.AddProperty(PropertyName._previousButton, Variant.From(in _previousButton));
		info.AddProperty(PropertyName._nextButton, Variant.From(in _nextButton));
		info.AddProperty(PropertyName._addButton, Variant.From(in _addButton));
		info.AddProperty(PropertyName._duplicateButton, Variant.From(in _duplicateButton));
		info.AddProperty(PropertyName._removeButton, Variant.From(in _removeButton));
		info.AddProperty(PropertyName._moveUpButton, Variant.From(in _moveUpButton));
		info.AddProperty(PropertyName._moveDownButton, Variant.From(in _moveDownButton));
		info.AddProperty(PropertyName._emptyLabel, Variant.From(in _emptyLabel));
		info.AddProperty(PropertyName._statusLabel, Variant.From(in _statusLabel));
		info.AddProperty(PropertyName._npcOption, Variant.From(in _npcOption));
		info.AddProperty(PropertyName._animeOption, Variant.From(in _animeOption));
		info.AddProperty(PropertyName._audioSelectionButton, Variant.From(in _audioSelectionButton));
		info.AddProperty(PropertyName._playAudioButton, Variant.From(in _playAudioButton));
		info.AddProperty(PropertyName._audioPicker, Variant.From(in _audioPicker));
		info.AddProperty(PropertyName._handPropsPanel, Variant.From(in _handPropsPanel));
		info.AddProperty(PropertyName._tutorialPanel, Variant.From(in _tutorialPanel));
		info.AddProperty(PropertyName._tutorialResourceButton, Variant.From(in _tutorialResourceButton));
		info.AddProperty(PropertyName._jsonSourceDialog, Variant.From(in _jsonSourceDialog));
		info.AddProperty(PropertyName._propSceneDialog, Variant.From(in _propSceneDialog));
		info.AddProperty(PropertyName._tutorialResourceDialog, Variant.From(in _tutorialResourceDialog));
		info.AddProperty(PropertyName._directAudioPlayer, Variant.From(in _directAudioPlayer));
		info.AddSignalEventDelegate(SignalName.TalkEdited, backing_TalkEdited);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._config, out var value))
		{
			_config = value.As<NpcTalkConfig>();
		}
		if (info.TryGetProperty(PropertyName._editingRoot, out var value2))
		{
			_editingRoot = value2.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._standaloneEntry, out var value3))
		{
			_standaloneEntry = value3.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._index, out var value4))
		{
			_index = value4.As<int>();
		}
		if (info.TryGetProperty(PropertyName._runtimeControl, out var value5))
		{
			_runtimeControl = value5.As<NpcTalkControl>();
		}
		if (info.TryGetProperty(PropertyName._npc, out var value6))
		{
			_npc = value6.As<NpcBase>();
		}
		if (info.TryGetProperty(PropertyName._npcSceneKey, out var value7))
		{
			_npcSceneKey = value7.As<string>();
		}
		if (info.TryGetProperty(PropertyName._bubbleEditor, out var value8))
		{
			_bubbleEditor = value8.As<TextEdit>();
		}
		if (info.TryGetProperty(PropertyName._undoRedo, out var value9))
		{
			_undoRedo = value9.As<XWUndoRedoManager>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value10))
		{
			_updatingControls = value10.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._pendingPropProperty, out var value11))
		{
			_pendingPropProperty = value11.As<string>();
		}
		if (info.TryGetProperty(PropertyName._selectedAddType, out var value12))
		{
			_selectedAddType = value12.As<int>();
		}
		if (info.TryGetProperty(PropertyName._saveKeyLineEdit, out var value13))
		{
			_saveKeyLineEdit = value13.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._jsonSourceButton, out var value14))
		{
			_jsonSourceButton = value14.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._entryVisualButton, out var value15))
		{
			_entryVisualButton = value15.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._addTypeVisualButton, out var value16))
		{
			_addTypeVisualButton = value16.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._entryPickerPopup, out var value17))
		{
			_entryPickerPopup = value17.As<PopupPanel>();
		}
		if (info.TryGetProperty(PropertyName._addTypePopup, out var value18))
		{
			_addTypePopup = value18.As<PopupPanel>();
		}
		if (info.TryGetProperty(PropertyName._entryPickerGrid, out var value19))
		{
			_entryPickerGrid = value19.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._addTypeGrid, out var value20))
		{
			_addTypeGrid = value20.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._stageFrame, out var value21))
		{
			_stageFrame = value21.As<XWAspectScaledPreviewHost>();
		}
		if (info.TryGetProperty(PropertyName._previousButton, out var value22))
		{
			_previousButton = value22.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._nextButton, out var value23))
		{
			_nextButton = value23.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._addButton, out var value24))
		{
			_addButton = value24.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._duplicateButton, out var value25))
		{
			_duplicateButton = value25.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._removeButton, out var value26))
		{
			_removeButton = value26.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveUpButton, out var value27))
		{
			_moveUpButton = value27.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._moveDownButton, out var value28))
		{
			_moveDownButton = value28.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._emptyLabel, out var value29))
		{
			_emptyLabel = value29.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._statusLabel, out var value30))
		{
			_statusLabel = value30.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._npcOption, out var value31))
		{
			_npcOption = value31.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._animeOption, out var value32))
		{
			_animeOption = value32.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._audioSelectionButton, out var value33))
		{
			_audioSelectionButton = value33.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._playAudioButton, out var value34))
		{
			_playAudioButton = value34.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._audioPicker, out var value35))
		{
			_audioPicker = value35.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._handPropsPanel, out var value36))
		{
			_handPropsPanel = value36.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._tutorialPanel, out var value37))
		{
			_tutorialPanel = value37.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._tutorialResourceButton, out var value38))
		{
			_tutorialResourceButton = value38.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._jsonSourceDialog, out var value39))
		{
			_jsonSourceDialog = value39.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._propSceneDialog, out var value40))
		{
			_propSceneDialog = value40.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._tutorialResourceDialog, out var value41))
		{
			_tutorialResourceDialog = value41.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._directAudioPlayer, out var value42))
		{
			_directAudioPlayer = value42.As<AudioStreamPlayer>();
		}
		if (info.TryGetSignalEventDelegate<TalkEditedEventHandler>(SignalName.TalkEdited, out var value43))
		{
			backing_TalkEdited = value43;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static List<MethodInfo> GetGodotSignalList()
	{
		return new List<MethodInfo>(1)
		{
			new MethodInfo(SignalName.TalkEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
		};
	}

	protected void EmitSignalTalkEdited()
	{
		EmitSignal(SignalName.TalkEdited, default(ReadOnlySpan<Variant>));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RaiseGodotClassSignalCallbacks(in godot_string_name signal, NativeVariantPtrArgs args)
	{
		if (signal == SignalName.TalkEdited && args.Count == 0)
		{
			backing_TalkEdited?.Invoke();
		}
		else
		{
			base.RaiseGodotClassSignalCallbacks(in signal, args);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassSignal(in godot_string_name signal)
	{
		if (signal == SignalName.TalkEdited)
		{
			return true;
		}
		return base.HasGodotClassSignal(in signal);
	}
}
