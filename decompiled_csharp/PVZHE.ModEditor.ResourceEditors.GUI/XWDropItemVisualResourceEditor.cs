using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWDropItemVisualResourceEditor.cs")]
public class XWDropItemVisualResourceEditor : XWGenericVisualResourceEditor
{
	private enum PreviewState
	{
		Idle,
		Dropping,
		Picking
	}

	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public new static readonly StringName _Process = "_Process";

		public static readonly StringName HasCompleteDropItemVisualCoverage = "HasCompleteDropItemVisualCoverage";

		public static readonly StringName BindWorkbench = "BindWorkbench";

		public static readonly StringName BindResourceMetadata = "BindResourceMetadata";

		public static readonly StringName BindConfigSignals = "BindConfigSignals";

		public static readonly StringName MountObjectIdLibrary = "MountObjectIdLibrary";

		public static readonly StringName BuildCategoryCards = "BuildCategoryCards";

		public static readonly StringName BuildHandlerCards = "BuildHandlerCards";

		public static readonly StringName ApplyBuiltInHandler = "ApplyBuiltInHandler";

		public static readonly StringName CreateVisualCard = "CreateVisualCard";

		public static readonly StringName ClearVisualCards = "ClearVisualCards";

		public static readonly StringName SelectVisualCard = "SelectVisualCard";

		public static readonly StringName GetObjectVisualCategory = "GetObjectVisualCategory";

		public static readonly StringName LoadObjectVisualIcon = "LoadObjectVisualIcon";

		public static readonly StringName BindDropItemName = "BindDropItemName";

		public static readonly StringName CommitDropItemName = "CommitDropItemName";

		public static readonly StringName OnScenePicked = "OnScenePicked";

		public static readonly StringName OnHandlerPicked = "OnHandlerPicked";

		public static readonly StringName ShowAudioSelector = "ShowAudioSelector";

		public static readonly StringName EnsureAudioSelector = "EnsureAudioSelector";

		public static readonly StringName SetAudioKey = "SetAudioKey";

		public static readonly StringName RenderConfigWorkbench = "RenderConfigWorkbench";

		public static readonly StringName RenderHandlerWorkbench = "RenderHandlerWorkbench";

		public static readonly StringName PopulateConfigControls = "PopulateConfigControls";

		public static readonly StringName OnDropItemPropertyEdited = "OnDropItemPropertyEdited";

		public static readonly StringName RefreshDropItemVisuals = "RefreshDropItemVisuals";

		public static readonly StringName RefreshDropItemEditorFromHistory = "RefreshDropItemEditorFromHistory";

		public static readonly StringName UpdateConfigPreviewLabels = "UpdateConfigPreviewLabels";

		public static readonly StringName RebuildDropItemScenePreview = "RebuildDropItemScenePreview";

		public static readonly StringName DisablePreviewGameplayProcessing = "DisablePreviewGameplayProcessing";

		public static readonly StringName CollectVisualPreviewNodes = "CollectVisualPreviewNodes";

		public static readonly StringName UpdatePreviewProcessingVisibility = "UpdatePreviewProcessingVisibility";

		public static readonly StringName CenterSceneInstance = "CenterSceneInstance";

		public static readonly StringName StartDropPreview = "StartDropPreview";

		public static readonly StringName StartPickPreview = "StartPickPreview";

		public static readonly StringName UpdateDropPreview = "UpdateDropPreview";

		public static readonly StringName UpdatePickPreview = "UpdatePickPreview";

		public static readonly StringName ResetPreviewTransform = "ResetPreviewTransform";

		public static readonly StringName UpdateHandlerPreview = "UpdateHandlerPreview";

		public static readonly StringName PlayConfiguredAudio = "PlayConfiguredAudio";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName GetBuiltInHandlerId = "GetBuiltInHandlerId";

		public static readonly StringName CreateBuiltInHandler = "CreateBuiltInHandler";

		public static readonly StringName GetHandlerDisplayName = "GetHandlerDisplayName";

		public static readonly StringName GetHandlerDescription = "GetHandlerDescription";

		public static readonly StringName BuildHandlerOutcome = "BuildHandlerOutcome";

		public static readonly StringName GetDropCategoryLabel = "GetDropCategoryLabel";

		public static readonly StringName FormatResourcePath = "FormatResourcePath";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName DropCategoryVisualCardCount = "DropCategoryVisualCardCount";

		public static readonly StringName DropHandlerVisualCardCount = "DropHandlerVisualCardCount";

		public static readonly StringName HasObjectLibrarySearchAndPaging = "HasObjectLibrarySearchAndPaging";

		public static readonly StringName IsDropItemPreviewRendering = "IsDropItemPreviewRendering";

		public static readonly StringName _editingConfig = "_editingConfig";

		public static readonly StringName _editingHandler = "_editingHandler";

		public static readonly StringName _scenePicker = "_scenePicker";

		public static readonly StringName _handlerPicker = "_handlerPicker";

		public static readonly StringName _audioPicker = "_audioPicker";

		public static readonly StringName _audioTargetProperty = "_audioTargetProperty";

		public static readonly StringName _configFields = "_configFields";

		public static readonly StringName _handlerFields = "_handlerFields";

		public static readonly StringName _resourceNameEdit = "_resourceNameEdit";

		public static readonly StringName _localToSceneCheck = "_localToSceneCheck";

		public new static readonly StringName _title = "_title";

		public static readonly StringName _resourceType = "_resourceType";

		public static readonly StringName _status = "_status";

		public static readonly StringName _runtimeState = "_runtimeState";

		public static readonly StringName _sceneStatus = "_sceneStatus";

		public static readonly StringName _valueLabel = "_valueLabel";

		public static readonly StringName _missingScene = "_missingScene";

		public static readonly StringName _handlerTypeLabel = "_handlerTypeLabel";

		public static readonly StringName _handlerEffectLabel = "_handlerEffectLabel";

		public static readonly StringName _handlerOutcome = "_handlerOutcome";

		public static readonly StringName _nameEdit = "_nameEdit";

		public static readonly StringName _valueSpin = "_valueSpin";

		public static readonly StringName _poolSpin = "_poolSpin";

		public static readonly StringName _coinIdSpin = "_coinIdSpin";

		public static readonly StringName _categoryCards = "_categoryCards";

		public static readonly StringName _handlerCards = "_handlerCards";

		public static readonly StringName _objectIdPicker = "_objectIdPicker";

		public static readonly StringName _fallAudioEdit = "_fallAudioEdit";

		public static readonly StringName _pickAudioEdit = "_pickAudioEdit";

		public static readonly StringName _directAudioPlayer = "_directAudioPlayer";

		public static readonly StringName _handlerSimulationCount = "_handlerSimulationCount";

		public static readonly StringName _previewRoot = "_previewRoot";

		public static readonly StringName _sceneHost = "_sceneHost";

		public static readonly StringName _fallbackDrop = "_fallbackDrop";

		public static readonly StringName _previewViewport = "_previewViewport";

		public static readonly StringName _previewState = "_previewState";

		public static readonly StringName _previewElapsed = "_previewElapsed";

		public static readonly StringName _pickStartPosition = "_pickStartPosition";

		public static readonly StringName _updatingControls = "_updatingControls";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string VisualEditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWDropItemVisualEditorLayout.tscn";

	private const string VisualChoiceCardScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWGameVisualChoiceCard.tscn";

	private const string ObjectPoolVisualPickerScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWObjectPoolVisualPicker.tscn";

	private static PackedScene _visualEditorLayoutScene;

	private static PackedScene _visualChoiceCardScene;

	private static PackedScene _objectPoolVisualPickerScene;

	private DropItemConfig _editingConfig;

	private DropItemHandler _editingHandler;

	private XWVisualPropertyBinding _propertyBinding;

	private XWResourcePicker _scenePicker;

	private XWResourcePicker _handlerPicker;

	private XWGameplayResourcePickerWindow _audioPicker;

	private string _audioTargetProperty = "FallAudio";

	private VBoxContainer _configFields;

	private VBoxContainer _handlerFields;

	private LineEdit _resourceNameEdit;

	private CheckButton _localToSceneCheck;

	private Label _title;

	private Label _resourceType;

	private Label _status;

	private Label _runtimeState;

	private Label _sceneStatus;

	private Label _valueLabel;

	private Label _missingScene;

	private Label _handlerTypeLabel;

	private Label _handlerEffectLabel;

	private Label _handlerOutcome;

	private LineEdit _nameEdit;

	private SpinBox _valueSpin;

	private SpinBox _poolSpin;

	private SpinBox _coinIdSpin;

	private HFlowContainer _categoryCards;

	private HFlowContainer _handlerCards;

	private XWObjectPoolVisualPicker _objectIdPicker;

	private LineEdit _fallAudioEdit;

	private LineEdit _pickAudioEdit;

	private AudioStreamPlayer _directAudioPlayer;

	private SpinBox _handlerSimulationCount;

	private Node2D _previewRoot;

	private Node2D _sceneHost;

	private TextureRect _fallbackDrop;

	private SubViewport _previewViewport;

	private readonly List<Node> _visualPreviewNodes = new List<Node>();

	private PreviewState _previewState;

	private double _previewElapsed;

	private Vector2 _pickStartPosition;

	private bool _updatingControls;

	public int DropCategoryVisualCardCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_categoryCards))
			{
				return 0;
			}
			return _categoryCards.GetChildCount();
		}
	}

	public int DropHandlerVisualCardCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_handlerCards))
			{
				return 0;
			}
			return _handlerCards.GetChildCount();
		}
	}

	public bool HasObjectLibrarySearchAndPaging
	{
		get
		{
			if (GodotObject.IsInstanceValid(_objectIdPicker))
			{
				return _objectIdPicker.HasSearchAndPaging;
			}
			return false;
		}
	}

	public bool IsDropItemPreviewRendering
	{
		get
		{
			if (GodotObject.IsInstanceValid(_previewViewport))
			{
				return _previewViewport.RenderTargetUpdateMode != SubViewport.UpdateMode.Disabled;
			}
			return false;
		}
	}

	public override void _Ready()
	{
		SetProcess(enable: false);
		base._Ready();
	}

	public override void _ExitTree()
	{
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		_audioPicker?.Dismiss();
		_editingConfig = null;
		_editingHandler = null;
		base._ExitTree();
	}

	protected override void OnVisualEditorVisibilityChanged(bool visible)
	{
		base.OnVisualEditorVisibilityChanged(visible);
		if (GodotObject.IsInstanceValid(_previewViewport))
		{
			_previewViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode)(visible ? 4 : 0);
		}
		UpdatePreviewProcessingVisibility();
	}

	public override void _Process(double delta)
	{
		if (!GodotObject.IsInstanceValid(_previewRoot))
		{
			SetProcess(enable: false);
			return;
		}
		_previewElapsed += delta;
		switch (_previewState)
		{
		case PreviewState.Dropping:
			UpdateDropPreview();
			break;
		case PreviewState.Picking:
			UpdatePickPreview();
			break;
		default:
			SetProcess(enable: false);
			break;
		}
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		_editingConfig = null;
		_editingHandler = null;
		if ((!(CurrentResource is DropItemConfig) && !(CurrentResource is DropItemHandler)) || CanvasGrid == null)
		{
			return;
		}
		_editingConfig = CurrentResource as DropItemConfig;
		_editingHandler = CurrentResource as DropItemHandler;
		_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnDropItemPropertyEdited);
		CanvasGrid.Columns = 1;
		if (_visualEditorLayoutScene == null)
		{
			_visualEditorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWDropItemVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		VBoxContainer vBoxContainer = _visualEditorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(vBoxContainer))
		{
			CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
			BindWorkbench(vBoxContainer);
			BindResourceMetadata(CurrentResource);
			if (_editingConfig != null)
			{
				RenderConfigWorkbench();
			}
			else
			{
				RenderHandlerWorkbench();
			}
			AddSummaryRows();
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!HasCompleteDropItemVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool HasCompleteDropItemVisualCoverage(Resource resource)
	{
		Type type = resource?.GetType();
		if (!(type == typeof(DropItemConfig)) && !(type == typeof(DropItemHandler)) && !(type == typeof(SunDropItemHandler)) && !(type == typeof(CoinDropItemHandler)) && !(type == typeof(LuckyBagDropItemHandler)) && !(type == typeof(JalapenoSunDropItemHandler)) && !(type == typeof(BrainSunDropItemHandler)))
		{
			return type == typeof(GoldShardDropItemHandler);
		}
		return true;
	}

	private void BindWorkbench(VBoxContainer root)
	{
		_resourceNameEdit = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToSceneCheck = root.GetNode<CheckButton>("%LocalToSceneCheck");
		_configFields = root.GetNode<VBoxContainer>("%ConfigFields");
		_handlerFields = root.GetNode<VBoxContainer>("%HandlerFields");
		_title = root.GetNode<Label>("%Title");
		_resourceType = root.GetNode<Label>("%ResourceType");
		_status = root.GetNode<Label>("%Status");
		_runtimeState = root.GetNode<Label>("%RuntimeState");
		_sceneStatus = root.GetNode<Label>("%SceneStatus");
		_valueLabel = root.GetNode<Label>("%ValueLabel");
		_missingScene = root.GetNode<Label>("%MissingScene");
		_handlerTypeLabel = root.GetNode<Label>("%HandlerTypeLabel");
		_handlerEffectLabel = root.GetNode<Label>("%HandlerEffectLabel");
		_handlerOutcome = root.GetNode<Label>("%HandlerOutcome");
		_nameEdit = root.GetNode<LineEdit>("%NameEdit");
		_valueSpin = root.GetNode<SpinBox>("%ValueSpin");
		_poolSpin = root.GetNode<SpinBox>("%PoolSpin");
		_coinIdSpin = root.GetNode<SpinBox>("%CoinIdSpin");
		_categoryCards = root.GetNode<HFlowContainer>("%CategoryCards");
		_handlerCards = root.GetNode<HFlowContainer>("%HandlerCards");
		_fallAudioEdit = root.GetNode<LineEdit>("%FallAudioEdit");
		_pickAudioEdit = root.GetNode<LineEdit>("%PickAudioEdit");
		_directAudioPlayer = root.GetNode<AudioStreamPlayer>("%DirectAudioPlayer");
		_handlerSimulationCount = root.GetNode<SpinBox>("%HandlerSimulationCount");
		_previewRoot = root.GetNode<Node2D>("%PreviewRoot");
		_sceneHost = root.GetNode<Node2D>("%SceneHost");
		_fallbackDrop = root.GetNode<TextureRect>("%FallbackDrop");
		_previewViewport = root.GetNode<SubViewport>("%Viewport");
		_previewViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode)(IsVisibleInTree() ? 4 : 0);
		root.GetNode<Button>("%DropButton").Pressed += StartDropPreview;
		root.GetNode<Button>("%PickButton").Pressed += StartPickPreview;
		root.GetNode<Button>("%ReloadButton").Pressed += RebuildDropItemScenePreview;
		root.GetNode<Button>("%PlayFallAudio").Pressed += () =>
		{
			PlayConfiguredAudio(_editingConfig?.FallAudio, "下落");
		};
		root.GetNode<Button>("%PlayPickAudio").Pressed += () =>
		{
			PlayConfiguredAudio(_editingConfig?.PickAudio, "拾取");
		};
		root.GetNode<Button>("%ChooseFallAudioButton").Pressed += () =>
		{
			ShowAudioSelector("FallAudio");
		};
		root.GetNode<Button>("%ChoosePickAudioButton").Pressed += () =>
		{
			ShowAudioSelector("PickAudio");
		};
		root.GetNode<Button>("%ClearFallAudioButton").Pressed += () =>
		{
			SetAudioKey("FallAudio", "", "清除掉落物下落音效");
		};
		root.GetNode<Button>("%ClearPickAudioButton").Pressed += () =>
		{
			SetAudioKey("PickAudio", "", "清除掉落物拾取音效");
		};
		_scenePicker = XWResourcePicker.Create();
		_scenePicker.Setup("PackedScene");
		_scenePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		root.GetNode<HBoxContainer>("%ScenePickerHost").AddChild(_scenePicker, forceReadableName: false, InternalMode.Disabled);
		_scenePicker.ResourceChanged += OnScenePicked;
		_handlerPicker = XWResourcePicker.Create();
		_handlerPicker.Setup("DropItemHandler");
		_handlerPicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		root.GetNode<HBoxContainer>("%HandlerPickerHost").AddChild(_handlerPicker, forceReadableName: false, InternalMode.Disabled);
		_handlerPicker.ResourceChanged += OnHandlerPicked;
		MountObjectIdLibrary(root.GetNode<VBoxContainer>("%ObjectLibraryHost"));
		BuildCategoryCards();
		BindConfigSignals();
		_handlerSimulationCount.ValueChanged += (double _) =>
		{
			if (!_updatingControls)
			{
				UpdateHandlerPreview();
			}
		};
	}

	private void BindResourceMetadata(Resource resource)
	{
		_propertyBinding.BindText(_resourceNameEdit, resource, "resource_name", () =>
		{
			RefreshDropItemVisuals();
		}, this, "RefreshDropItemEditorFromHistory");
		_propertyBinding.BindToggle(_localToSceneCheck, resource, "resource_local_to_scene", () =>
		{
			RefreshDropItemVisuals();
		}, this, "RefreshDropItemEditorFromHistory");
	}

	private void BindConfigSignals()
	{
		DropItemConfig editingConfig = _editingConfig;
		if (editingConfig != null && _propertyBinding != null)
		{
			BindDropItemName(editingConfig);
			_propertyBinding.BindNumber(_valueSpin, editingConfig, "Value", () =>
			{
				RefreshDropItemVisuals();
			}, this, "RefreshDropItemEditorFromHistory");
			_propertyBinding.BindNumber(_poolSpin, editingConfig, "PoolMaxNum", () =>
			{
				RefreshDropItemVisuals();
			}, this, "RefreshDropItemEditorFromHistory");
			_propertyBinding.BindNumber(_coinIdSpin, editingConfig, "CoinObjectId", () =>
			{
				RefreshDropItemVisuals();
			}, this, "RefreshDropItemEditorFromHistory");
			_propertyBinding.BindText(_fallAudioEdit, editingConfig, "FallAudio", () =>
			{
				RefreshDropItemVisuals();
			}, this, "RefreshDropItemEditorFromHistory");
			_propertyBinding.BindText(_pickAudioEdit, editingConfig, "PickAudio", () =>
			{
				RefreshDropItemVisuals();
			}, this, "RefreshDropItemEditorFromHistory");
		}
	}

	private void MountObjectIdLibrary(VBoxContainer host)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return;
		}
		if (_objectPoolVisualPickerScene == null)
		{
			_objectPoolVisualPickerScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWObjectPoolVisualPicker.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		_objectIdPicker = _objectPoolVisualPickerScene?.Instantiate<XWObjectPoolVisualPicker>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(_objectIdPicker))
		{
			return;
		}
		_objectIdPicker.Name = "DropItemObjectLibrary";
		host.AddChild(_objectIdPicker, forceReadableName: false, InternalMode.Disabled);
		List<XWObjectPoolVisualChoice> list = new List<XWObjectPoolVisualChoice>();
		ObjectManagerConfig.OBJECT[] values = Enum.GetValues<ObjectManagerConfig.OBJECT>();
		for (int i = 0; i < values.Length; i++)
		{
			ObjectManagerConfig.OBJECT oBJECT = values[i];
			if (oBJECT != ObjectManagerConfig.OBJECT.MAX)
			{
				string objectVisualCategory = GetObjectVisualCategory(oBJECT);
				list.Add(new XWObjectPoolVisualChoice((int)oBJECT, oBJECT.ToString(), objectVisualCategory, LoadObjectVisualIcon(objectVisualCategory)));
			}
		}
		_objectIdPicker.Configure(list, (int)(_editingConfig?.Id ?? ObjectManagerConfig.OBJECT.NOONE));
		_objectIdPicker.ChoiceSelected += (int id) =>
		{
			if (!_updatingControls && _editingConfig != null && _propertyBinding != null)
			{
				_propertyBinding.SetValue(_editingConfig, "Id", Variant.From<long>((long)id), "更换掉落物对象池 ID", this, "RefreshDropItemEditorFromHistory");
				RefreshDropItemVisuals();
			}
		};
	}

	private void BuildCategoryCards()
	{
		if (!GodotObject.IsInstanceValid(_categoryCards))
		{
			return;
		}
		ClearVisualCards(_categoryCards);
		TowerDefenseEnum.DROP_ITEM_CATEGORY[] values = Enum.GetValues<TowerDefenseEnum.DROP_ITEM_CATEGORY>();
		for (int i = 0; i < values.Length; i++)
		{
			TowerDefenseEnum.DROP_ITEM_CATEGORY dROP_ITEM_CATEGORY = values[i];
			TowerDefenseEnum.DROP_ITEM_CATEGORY selectedCategory = dROP_ITEM_CATEGORY;
			string iconPath = dROP_ITEM_CATEGORY switch
			{
				TowerDefenseEnum.DROP_ITEM_CATEGORY.SUN => "res://addons/ModEditor/Icons/ResourceCollectable.svg", 
				TowerDefenseEnum.DROP_ITEM_CATEGORY.COIN => "res://addons/ModEditor/Icons/Favorites.svg", 
				_ => "res://addons/ModEditor/Icons/ResourceFallingObject.svg", 
			};
			int num = (int)dROP_ITEM_CATEGORY;
			string key = num.ToString();
			string dropCategoryLabel = GetDropCategoryLabel(dROP_ITEM_CATEGORY);
			string detail = dROP_ITEM_CATEGORY.ToString();
			DropItemConfig editingConfig = _editingConfig;
			XWGameVisualChoiceCard xWGameVisualChoiceCard = CreateVisualCard(key, dropCategoryLabel, detail, iconPath, editingConfig != null && editingConfig.Category == dROP_ITEM_CATEGORY, new Vector2(138f, 62f));
			if (!GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
			{
				continue;
			}
			xWGameVisualChoiceCard.Pressed += () =>
			{
				if (!_updatingControls && _editingConfig != null && _propertyBinding != null)
				{
					_propertyBinding.SetValue(_editingConfig, "Category", Variant.From<long>((long)selectedCategory), "更换掉落物分类", this, "RefreshDropItemEditorFromHistory");
					HFlowContainer categoryCards = _categoryCards;
					int num2 = (int)selectedCategory;
					SelectVisualCard(categoryCards, num2.ToString());
					RefreshDropItemVisuals();
				}
			};
			_categoryCards.AddChild(xWGameVisualChoiceCard, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void BuildHandlerCards()
	{
		if (!GodotObject.IsInstanceValid(_handlerCards))
		{
			return;
		}
		ClearVisualCards(_handlerCards);
		(string, string, string)[] array = new (string, string, string)[7]
		{
			("无处理器", "只拾取显示", "res://addons/ModEditor/Icons/Clear.svg"),
			("阳光资源", "增加阳光", "res://addons/ModEditor/Icons/ResourceCollectable.svg"),
			("金币", "增加货币", "res://addons/ModEditor/Icons/Favorites.svg"),
			("福袋里程碑", "累计奖励", "res://addons/ModEditor/Icons/Progress1.svg"),
			("火爆辣椒阳光", "特殊阳光", "res://addons/ModEditor/Icons/StatusError.svg"),
			("僵尸脑子阳光", "阵营阳光", "res://addons/ModEditor/Icons/ResourceCharacter.svg"),
			("金色碎片", "合成卡牌", "res://addons/ModEditor/Icons/ResourceCard.svg")
		};
		int builtInHandlerId = GetBuiltInHandlerId(_editingConfig?.Handler);
		for (int i = 0; i < array.Length; i++)
		{
			int handlerId = i;
			XWGameVisualChoiceCard xWGameVisualChoiceCard = CreateVisualCard(i.ToString(), array[i].Item1, array[i].Item2, array[i].Item3, builtInHandlerId == i, new Vector2(154f, 64f));
			if (GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
			{
				xWGameVisualChoiceCard.Pressed += () =>
				{
					ApplyBuiltInHandler(handlerId);
				};
				_handlerCards.AddChild(xWGameVisualChoiceCard, forceReadableName: false, InternalMode.Disabled);
			}
		}
		if (builtInHandlerId < 0 && _editingConfig?.Handler != null)
		{
			XWGameVisualChoiceCard xWGameVisualChoiceCard2 = CreateVisualCard("100", "自定义处理器", _editingConfig.Handler.GetType().Name, "res://addons/ModEditor/Icons/Script.svg", selected: true, new Vector2(170f, 64f));
			if (GodotObject.IsInstanceValid(xWGameVisualChoiceCard2))
			{
				xWGameVisualChoiceCard2.Disabled = true;
				_handlerCards.AddChild(xWGameVisualChoiceCard2, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void ApplyBuiltInHandler(int handlerId)
	{
		if (!_updatingControls && _editingConfig != null && _propertyBinding != null)
		{
			DropItemHandler from = CreateBuiltInHandler(handlerId);
			_propertyBinding.SetValue(_editingConfig, "Handler", Variant.From(in from), "更换掉落物拾取处理器", this, "RefreshDropItemEditorFromHistory");
			_handlerPicker?.SetEditedResource(from);
			SelectVisualCard(_handlerCards, handlerId.ToString());
			RefreshDropItemVisuals();
		}
	}

	private XWGameVisualChoiceCard CreateVisualCard(string key, string title, string detail, string iconPath, bool selected, Vector2 minimumSize)
	{
		if (_visualChoiceCardScene == null)
		{
			_visualChoiceCardScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWGameVisualChoiceCard.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		XWGameVisualChoiceCard xWGameVisualChoiceCard = _visualChoiceCardScene?.Instantiate<XWGameVisualChoiceCard>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
		{
			return null;
		}
		xWGameVisualChoiceCard.CustomMinimumSize = minimumSize;
		xWGameVisualChoiceCard.Configure(key, title, detail, ResourceLoader.Load<Texture2D>(iconPath, null, ResourceLoader.CacheMode.Reuse), selected);
		return xWGameVisualChoiceCard;
	}

	private static void ClearVisualCards(Control host)
	{
		foreach (Node child in host.GetChildren())
		{
			host.RemoveChild(child);
			child.QueueFree();
		}
	}

	private static void SelectVisualCard(Control host, string selectedKey)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return;
		}
		foreach (Node child in host.GetChildren())
		{
			if (child is XWGameVisualChoiceCard xWGameVisualChoiceCard)
			{
				xWGameVisualChoiceCard.SetSelected(xWGameVisualChoiceCard.ChoiceKey == selectedKey);
			}
		}
	}

	private static string GetObjectVisualCategory(ObjectManagerConfig.OBJECT id)
	{
		string text = id.ToString();
		if (text.Contains("SUN", StringComparison.OrdinalIgnoreCase) || text == "GAMECOLLECT")
		{
			return "阳光";
		}
		if (text.Contains("COIN", StringComparison.OrdinalIgnoreCase))
		{
			return "货币";
		}
		if (text.Contains("PROJECTILE", StringComparison.OrdinalIgnoreCase) || text.Contains("damage", StringComparison.OrdinalIgnoreCase))
		{
			return "战斗";
		}
		if (text.Contains("PARTICLE", StringComparison.OrdinalIgnoreCase))
		{
			return "特效";
		}
		return "对象池";
	}

	private static Texture2D LoadObjectVisualIcon(string category)
	{
		return ResourceLoader.Load<Texture2D>(category switch
		{
			"阳光" => "res://addons/ModEditor/Icons/ResourceCollectable.svg", 
			"货币" => "res://addons/ModEditor/Icons/Favorites.svg", 
			"战斗" => "res://addons/ModEditor/Icons/ResourceProjectile.svg", 
			"特效" => "res://addons/ModEditor/Icons/ResourceAnimation.svg", 
			_ => "res://addons/ModEditor/Icons/ResourceFallingObject.svg", 
		}, null, ResourceLoader.CacheMode.Reuse);
	}

	private void BindDropItemName(DropItemConfig config)
	{
		_nameEdit.Text = config.Name.ToString();
		_nameEdit.FocusEntered += () =>
		{
			_propertyBinding?.BeginEdit(config, "Name");
		};
		_nameEdit.TextChanged += (string text) =>
		{
			if (!_updatingControls && _propertyBinding != null)
			{
				_propertyBinding.PreviewValue(config, "Name", Variant.From<StringName>(new StringName(text)));
				RefreshDropItemVisuals();
			}
		};
		_nameEdit.TextSubmitted += (string text) =>
		{
			CommitDropItemName(config, text);
		};
		_nameEdit.FocusExited += () =>
		{
			CommitDropItemName(config, _nameEdit.Text);
		};
	}

	private void CommitDropItemName(DropItemConfig config, string text)
	{
		if (!_updatingControls && _propertyBinding != null)
		{
			_propertyBinding.CommitEdit(config, "Name", Variant.From<StringName>(new StringName(text ?? "")), "修改掉落物注册名", this, "RefreshDropItemEditorFromHistory");
			RefreshDropItemVisuals();
		}
	}

	private void OnScenePicked(Resource resource)
	{
		if (!_updatingControls && _propertyBinding != null && _editingConfig != null && (resource == null || resource is PackedScene))
		{
			_propertyBinding.SetValue(_editingConfig, "Scene", Variant.From<PackedScene>(resource as PackedScene), "更换掉落物游戏场景", this, "RefreshDropItemEditorFromHistory");
			RefreshDropItemVisuals(rebuildScene: true);
		}
	}

	private void OnHandlerPicked(Resource resource)
	{
		if (!_updatingControls && _propertyBinding != null && _editingConfig != null && (resource == null || resource is DropItemHandler))
		{
			_propertyBinding.SetValue(_editingConfig, "Handler", Variant.From<DropItemHandler>(resource as DropItemHandler), "更换掉落物拾取处理器", this, "RefreshDropItemEditorFromHistory");
			BuildHandlerCards();
			RefreshDropItemVisuals();
		}
	}

	private void ShowAudioSelector(string propertyName)
	{
		if (_editingConfig != null)
		{
			_audioTargetProperty = ((propertyName == "PickAudio") ? "PickAudio" : "FallAudio");
			EnsureAudioSelector();
			string currentKey = ((_audioTargetProperty == "PickAudio") ? _editingConfig.PickAudio : _editingConfig.FallAudio);
			_audioPicker?.Open(XWGameplayResourceKind.Audio, currentKey, ApplyAudioChoice, (XWGameplayResourceChoice choice) => choice.Kind == XWGameplayResourceKind.Audio, lockKind: true, (_audioTargetProperty == "PickAudio") ? "掉落物拾取音效" : "掉落物下落音效");
		}
	}

	private void EnsureAudioSelector()
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
			SetAudioKey(_audioTargetProperty, choice.Key, (_audioTargetProperty == "PickAudio") ? "更换掉落物拾取音效" : "更换掉落物下落音效");
		}
	}

	private void SetAudioKey(string propertyName, string audioKey, string actionName)
	{
		if (_editingConfig != null && _propertyBinding != null)
		{
			propertyName = ((propertyName == "PickAudio") ? "PickAudio" : "FallAudio");
			_propertyBinding.SetValue(_editingConfig, propertyName, audioKey ?? "", actionName, this, "RefreshDropItemEditorFromHistory");
			PopulateConfigControls();
			RefreshDropItemVisuals();
		}
	}

	private void RenderConfigWorkbench()
	{
		_configFields.Visible = true;
		_handlerFields.Visible = false;
		_title.Text = "战场掉落物 · 配置与游戏预览";
		_resourceType.Text = "DropItemConfig";
		PopulateConfigControls();
		UpdateConfigPreviewLabels();
		RebuildDropItemScenePreview();
		ResetPreviewTransform();
	}

	private void RenderHandlerWorkbench()
	{
		_configFields.Visible = false;
		_handlerFields.Visible = true;
		_title.Text = "战场掉落物 · 拾取处理器";
		_resourceType.Text = _editingHandler?.GetType().Name ?? "DropItemHandler";
		_fallbackDrop.Visible = true;
		_missingScene.Visible = false;
		_sceneStatus.Text = "处理器资源不包含显示场景";
		_valueLabel.Text = "拾取";
		UpdateHandlerPreview();
		ResetPreviewTransform();
	}

	private void PopulateConfigControls()
	{
		if (_editingConfig == null || !GodotObject.IsInstanceValid(_nameEdit))
		{
			return;
		}
		_updatingControls = true;
		try
		{
			_nameEdit.Text = _editingConfig.Name.ToString();
			_valueSpin.Value = _editingConfig.Value;
			_poolSpin.Value = Math.Max(1, _editingConfig.PoolMaxNum);
			_coinIdSpin.Value = _editingConfig.CoinObjectId;
			_fallAudioEdit.Text = _editingConfig.FallAudio ?? "";
			_pickAudioEdit.Text = _editingConfig.PickAudio ?? "";
			HFlowContainer categoryCards = _categoryCards;
			int category = (int)_editingConfig.Category;
			SelectVisualCard(categoryCards, category.ToString());
			_objectIdPicker?.SetSelectedId((int)_editingConfig.Id);
			_scenePicker?.SetEditedResource(_editingConfig.Scene);
			_handlerPicker?.SetEditedResource(_editingConfig.Handler);
			BuildHandlerCards();
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void OnDropItemPropertyEdited(bool committed)
	{
		Resource currentResource = CurrentResource;
		if (currentResource != null)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
			}
			else
			{
				MarkCurrentResourceDirty();
				currentResource.EmitChanged();
			}
			RefreshDropItemVisuals();
		}
	}

	private void RefreshDropItemVisuals(bool rebuildScene = false)
	{
		if (_editingConfig != null)
		{
			UpdateConfigPreviewLabels();
			if (rebuildScene)
			{
				RebuildDropItemScenePreview();
			}
		}
		else if (_editingHandler != null)
		{
			UpdateHandlerPreview();
		}
	}

	public void RefreshDropItemEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager == null || (!xWUndoRedoManager.IsUndoing() && !xWUndoRedoManager.IsRedoing()) || CanvasGrid == null)
		{
			return;
		}
		Resource currentResource = CurrentResource;
		if (currentResource == null)
		{
			return;
		}
		MarkCurrentResourceDirty();
		currentResource.EmitChanged();
		_propertyBinding?.Dispose();
		foreach (Node child in CanvasGrid.GetChildren())
		{
			CanvasGrid.RemoveChild(child);
			child.QueueFree();
		}
		RenderCustomVisualPreset(null);
	}

	private void UpdateConfigPreviewLabels()
	{
		if (_editingConfig != null && GodotObject.IsInstanceValid(_valueLabel))
		{
			string value = (string.IsNullOrEmpty(_editingConfig.Name.ToString()) ? "未命名掉落物" : _editingConfig.Name.ToString());
			_valueLabel.Text = ((_editingConfig.Value >= 0) ? $"+{_editingConfig.Value}" : _editingConfig.Value.ToString());
			_sceneStatus.Text = (GodotObject.IsInstanceValid(_editingConfig.Scene) ? FormatResourcePath(_editingConfig.Scene.ResourcePath, "已配置内嵌场景") : "未配置 Scene（可在下方资源选择器选择）");
			_status.Text = $"{value} · {GetDropCategoryLabel(_editingConfig.Category)} · 拾取值 {_editingConfig.Value}";
			_runtimeState.Text = $"{GetHandlerDisplayName(_editingConfig.Handler)} · 池上限 {_editingConfig.PoolMaxNum}";
		}
	}

	private void RebuildDropItemScenePreview()
	{
		if (!GodotObject.IsInstanceValid(_sceneHost))
		{
			return;
		}
		foreach (Node child in _sceneHost.GetChildren())
		{
			child.QueueFree();
		}
		_visualPreviewNodes.Clear();
		bool flag = _editingConfig != null && GodotObject.IsInstanceValid(_editingConfig.Scene);
		_fallbackDrop.Visible = !flag;
		_missingScene.Visible = !flag && _editingConfig != null;
		if (!flag)
		{
			ResetPreviewTransform();
			return;
		}
		try
		{
			Node node = _editingConfig.Scene.Instantiate(PackedScene.GenEditState.Disabled);
			DisablePreviewGameplayProcessing(node);
			_sceneHost.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			CollectVisualPreviewNodes(node);
			UpdatePreviewProcessingVisibility();
			CenterSceneInstance(node);
			_fallbackDrop.Visible = false;
			_missingScene.Visible = false;
			_runtimeState.Text = "已载入游戏场景 · " + GetHandlerDisplayName(_editingConfig.Handler);
		}
		catch (Exception ex)
		{
			_fallbackDrop.Visible = true;
			_missingScene.Visible = true;
			_missingScene.Text = "场景预览失败：" + ex.Message;
			_runtimeState.Text = "场景预览失败";
			GD.PushWarning("Drop-item editor scene preview failed: " + ex.Message);
		}
		ResetPreviewTransform();
	}

	private static void DisablePreviewGameplayProcessing(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		node.ProcessMode = ProcessModeEnum.Disabled;
		node.SetProcess(enable: false);
		node.SetPhysicsProcess(enable: false);
		node.SetProcessInput(enable: false);
		node.SetProcessUnhandledInput(enable: false);
		node.SetProcessUnhandledKeyInput(enable: false);
		foreach (Node child in node.GetChildren())
		{
			DisablePreviewGameplayProcessing(child);
		}
	}

	private void CollectVisualPreviewNodes(Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
		{
			return;
		}
		if (node is AdobeAnimateSprite || node is AnimatedSprite2D || node is AnimationPlayer || node is GpuParticles2D)
		{
			_visualPreviewNodes.Add(node);
		}
		foreach (Node child in node.GetChildren())
		{
			CollectVisualPreviewNodes(child);
		}
	}

	private void UpdatePreviewProcessingVisibility()
	{
		bool flag = IsVisibleInTree();
		foreach (Node visualPreviewNode in _visualPreviewNodes)
		{
			if (!GodotObject.IsInstanceValid(visualPreviewNode))
			{
				continue;
			}
			visualPreviewNode.ProcessMode = (ProcessModeEnum)(flag ? 3 : 4);
			visualPreviewNode.SetProcess(flag);
			if (visualPreviewNode is AnimatedSprite2D animatedSprite2D && GodotObject.IsInstanceValid(animatedSprite2D.SpriteFrames))
			{
				if (flag && !animatedSprite2D.IsPlaying())
				{
					animatedSprite2D.Play();
				}
				else if (!flag)
				{
					animatedSprite2D.Pause();
				}
			}
			if (visualPreviewNode is GpuParticles2D gpuParticles2D)
			{
				gpuParticles2D.Emitting = flag;
			}
		}
		if (!flag)
		{
			SetProcess(enable: false);
		}
		else if (_previewState != PreviewState.Idle)
		{
			SetProcess(enable: true);
		}
	}

	private static void CenterSceneInstance(Node instance)
	{
		if (instance is Node2D node2D)
		{
			node2D.Position = Vector2.Zero;
		}
		else if (instance is Control control)
		{
			control.Position = -control.Size * 0.5f;
		}
	}

	private void StartDropPreview()
	{
		if (GodotObject.IsInstanceValid(_previewRoot))
		{
			_previewState = PreviewState.Dropping;
			_previewElapsed = 0.0;
			_previewRoot.Visible = true;
			_previewRoot.Position = new Vector2(480f, 88f);
			_previewRoot.Scale = Vector2.One;
			_previewRoot.Modulate = Colors.White;
			_status.Text = "掉落物正在进入草坪……";
			_runtimeState.Text = "下落中";
			SetProcess(enable: true);
		}
	}

	private void StartPickPreview()
	{
		if (GodotObject.IsInstanceValid(_previewRoot))
		{
			if (!_previewRoot.Visible)
			{
				ResetPreviewTransform();
			}
			_previewState = PreviewState.Picking;
			_previewElapsed = 0.0;
			_pickStartPosition = ((_previewRoot.Position.Y < 250f) ? new Vector2(480f, 365f) : _previewRoot.Position);
			_previewRoot.Position = _pickStartPosition;
			_previewRoot.Scale = Vector2.One;
			_previewRoot.Modulate = Colors.White;
			_status.Text = "拾取动画：飞向玩家资源栏";
			_runtimeState.Text = "拾取中";
			if (_editingHandler != null)
			{
				UpdateHandlerPreview();
			}
			SetProcess(enable: true);
		}
	}

	private void UpdateDropPreview()
	{
		float num = Mathf.Clamp((float)(_previewElapsed / 0.9), 0f, 1f);
		float weight = 1f - Mathf.Pow(1f - num, 2.3f);
		_previewRoot.Position = new Vector2(480f + Mathf.Sin(num * (float)Math.PI * 2f) * 16f, Mathf.Lerp(88f, 365f, weight));
		_previewRoot.Scale = Vector2.One * (0.84f + Mathf.Sin(num * (float)Math.PI) * 0.18f);
		if (!(num < 1f))
		{
			_previewRoot.Position = new Vector2(480f, 365f);
			_previewRoot.Scale = Vector2.One;
			_previewState = PreviewState.Idle;
			_status.Text = "掉落物已落在草坪，可模拟拾取";
			_runtimeState.Text = "等待拾取";
			SetProcess(enable: false);
		}
	}

	private void UpdatePickPreview()
	{
		float num = Mathf.Clamp((float)(_previewElapsed / 0.58), 0f, 1f);
		float weight = num * num * (3f - 2f * num);
		_previewRoot.Position = _pickStartPosition.Lerp(new Vector2(842f, 57f), weight);
		_previewRoot.Scale = Vector2.One * Mathf.Lerp(1f, 0.18f, weight);
		_previewRoot.Modulate = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0.25f, weight));
		if (!(num < 1f))
		{
			_previewState = PreviewState.Idle;
			_previewRoot.Visible = false;
			_status.Text = ((_editingConfig != null) ? $"已拾取：{_editingConfig.Name}，游戏效果 {GetHandlerDisplayName(_editingConfig.Handler)}" : ("已模拟处理器：" + GetHandlerDisplayName(_editingHandler)));
			_runtimeState.Text = "拾取完成（未修改游戏资源）";
			SetProcess(enable: false);
		}
	}

	private void ResetPreviewTransform()
	{
		_previewState = PreviewState.Idle;
		_previewElapsed = 0.0;
		SetProcess(enable: false);
		if (GodotObject.IsInstanceValid(_previewRoot))
		{
			_previewRoot.Visible = true;
			_previewRoot.Position = new Vector2(480f, 365f);
			_previewRoot.Scale = Vector2.One;
			_previewRoot.Modulate = Colors.White;
		}
	}

	private void UpdateHandlerPreview()
	{
		if (_editingHandler != null && GodotObject.IsInstanceValid(_handlerTypeLabel))
		{
			int num = Math.Max(1, Mathf.RoundToInt(_handlerSimulationCount.Value));
			_handlerTypeLabel.Text = GetHandlerDisplayName(_editingHandler);
			_handlerEffectLabel.Text = GetHandlerDescription(_editingHandler);
			_handlerOutcome.Text = BuildHandlerOutcome(_editingHandler, num);
			_status.Text = $"处理器模拟 · 连续拾取 {num} 次";
			_runtimeState.Text = "仅推演结果，不执行真实奖励";
		}
	}

	private void PlayConfiguredAudio(string key, string kind)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			_runtimeState.Text = "未配置" + kind + "音效键";
			return;
		}
		EnsureAudioSelector();
		AudioStream audioStream = (GodotObject.IsInstanceValid(_audioPicker) ? _audioPicker.LoadAudioStream(key) : null);
		if (GodotObject.IsInstanceValid(audioStream) && GodotObject.IsInstanceValid(_directAudioPlayer))
		{
			_directAudioPlayer.Stop();
			_directAudioPlayer.Stream = audioStream;
			_directAudioPlayer.Play();
			_runtimeState.Text = "正在直接试听" + kind + "音效：" + key;
		}
		else if (!GodotObject.IsInstanceValid(AudioManager.Instance))
		{
			_runtimeState.Text = "无法加载" + kind + "音效：" + key;
		}
		else
		{
			AudioManager.Instance.AudioPlay(key);
			_runtimeState.Text = "正在试听" + kind + "音效：" + key;
		}
	}

	private void AddSummaryRows()
	{
		if (PreviewList != null)
		{
			if (_editingConfig != null)
			{
				PreviewList.AddItem($"注册名 -> {_editingConfig.Name}");
				PreviewList.AddItem("游戏场景 -> " + FormatResourcePath(_editingConfig.Scene?.ResourcePath, "未配置"));
				PreviewList.AddItem("拾取效果 -> " + GetHandlerDisplayName(_editingConfig.Handler));
			}
			else
			{
				PreviewList.AddItem("处理器 -> " + GetHandlerDisplayName(_editingHandler));
				PreviewList.AddItem("模拟不会调用 OnCollect，不会改金币、阳光、卡牌或存档");
			}
		}
	}

	private static int GetBuiltInHandlerId(DropItemHandler handler)
	{
		if (handler != null)
		{
			if (!(handler is SunDropItemHandler))
			{
				if (!(handler is CoinDropItemHandler))
				{
					if (!(handler is LuckyBagDropItemHandler))
					{
						if (!(handler is JalapenoSunDropItemHandler))
						{
							if (!(handler is BrainSunDropItemHandler))
							{
								if (handler is GoldShardDropItemHandler)
								{
									return 6;
								}
								return -1;
							}
							return 5;
						}
						return 4;
					}
					return 3;
				}
				return 2;
			}
			return 1;
		}
		return 0;
	}

	private static DropItemHandler CreateBuiltInHandler(int id)
	{
		return id switch
		{
			1 => (DropItemHandler)new SunDropItemHandler(), 
			2 => new CoinDropItemHandler(), 
			3 => new LuckyBagDropItemHandler(), 
			4 => new JalapenoSunDropItemHandler(), 
			5 => new BrainSunDropItemHandler(), 
			6 => new GoldShardDropItemHandler(), 
			_ => null, 
		};
	}

	private static string GetHandlerDisplayName(DropItemHandler handler)
	{
		if (handler != null)
		{
			if (!(handler is SunDropItemHandler))
			{
				if (!(handler is CoinDropItemHandler))
				{
					if (!(handler is LuckyBagDropItemHandler))
					{
						if (!(handler is JalapenoSunDropItemHandler))
						{
							if (!(handler is BrainSunDropItemHandler))
							{
								if (handler is GoldShardDropItemHandler)
								{
									return "金色碎片合成卡牌";
								}
								return "自定义：" + handler.GetType().Name;
							}
							return "僵尸脑子阳光";
						}
						return "火爆辣椒阳光";
					}
					return "福袋里程碑奖励";
				}
				return "增加金币";
			}
			return "增加阳光";
		}
		return "无拾取效果";
	}

	private static string GetHandlerDescription(DropItemHandler handler)
	{
		if (!(handler is SunDropItemHandler))
		{
			if (!(handler is CoinDropItemHandler))
			{
				if (!(handler is LuckyBagDropItemHandler))
				{
					if (!(handler is JalapenoSunDropItemHandler))
					{
						if (!(handler is BrainSunDropItemHandler))
						{
							if (handler is GoldShardDropItemHandler)
							{
								return "每累计 4 个碎片，从 GeneralPlant/Gold 中生成金卡。";
							}
							return "自定义处理器。编辑器不会直接执行 OnCollect，避免预览污染游戏状态。";
						}
						return "按阵营与账户规则结算僵尸脑子阳光。";
					}
					return "按阳光账户规则结算，并表现为火爆辣椒阳光。";
				}
				return "累计拾取次数，在 10、30、60、100 次触发不同阳光或卡牌奖励。";
			}
			return "拾取后把配置数值加入金币总量。";
		}
		return "拾取后把配置数值加入当前玩家的阳光账户。";
	}

	private static string BuildHandlerOutcome(DropItemHandler handler, int count)
	{
		if (!(handler is LuckyBagDropItemHandler))
		{
			if (!(handler is GoldShardDropItemHandler))
			{
				if (!(handler is CoinDropItemHandler))
				{
					if (!(handler is SunDropItemHandler) && !(handler is JalapenoSunDropItemHandler) && !(handler is BrainSunDropItemHandler))
					{
						return $"将发生 {count} 次自定义拾取回调；预览未执行这些回调。";
					}
					return $"连续拾取 {count} 次会按账户规则结算每次配置的阳光值。";
				}
				return $"连续拾取 {count} 次会把每次配置值累计到金币。";
			}
			return $"{count} 个碎片可组成 {count / 4} 组金卡奖励，剩余 {count % 4} 个。";
		}
		string result;
		if (count < 60)
		{
			if (count < 10)
			{
				result = $"还差 {10 - count} 次触发 100 阳光奖励。";
			}
			else
			{
				result = ((count >= 30) ? "已越过 30 次奖励；下一里程碑为 60 次的金卡奖励。" : "已越过 10 次奖励；下一里程碑为 30 次的白卡奖励。");
			}
		}
		else
		{
			result = ((count >= 100) ? $"达到 {count} 次；100 次后每个整百会按百位数量追加金卡。" : "已越过 60 次奖励；下一里程碑为 100 次的 1000 阳光奖励。");
		}
		return result;
	}

	private static string GetDropCategoryLabel(TowerDefenseEnum.DROP_ITEM_CATEGORY category)
	{
		return category switch
		{
			TowerDefenseEnum.DROP_ITEM_CATEGORY.SUN => "阳光", 
			TowerDefenseEnum.DROP_ITEM_CATEGORY.COIN => "硬币", 
			TowerDefenseEnum.DROP_ITEM_CATEGORY.SPECIAL => "特殊奖励", 
			_ => category.ToString(), 
		};
	}

	private static string FormatResourcePath(string path, string fallback)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return path;
		}
		return fallback;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(51)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName._Process, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "delta", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasCompleteDropItemVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindResourceMetadata, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindConfigSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MountObjectIdLibrary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCategoryCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildHandlerCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyBuiltInHandler, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "handlerId", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateVisualCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Button"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "detail", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "iconPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "minimumSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearVisualCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.SelectVisualCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.String, "selectedKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetObjectVisualCategory, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadObjectVisualIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindDropItemName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CommitDropItemName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnScenePicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnHandlerPicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowAudioSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureAudioSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetAudioKey, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "audioKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RenderConfigWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderHandlerWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateConfigControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnDropItemPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshDropItemVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "rebuildScene", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshDropItemEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateConfigPreviewLabels, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildDropItemScenePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisablePreviewGameplayProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.CollectVisualPreviewNodes, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "node", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdatePreviewProcessingVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CenterSceneInstance, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "instance", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.StartDropPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartPickPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateDropPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePickPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetPreviewTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateHandlerPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PlayConfiguredAudio, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "key", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetBuiltInHandlerId, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "handler", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateBuiltInHandler, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetHandlerDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "handler", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetHandlerDescription, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "handler", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildHandlerOutcome, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "handler", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetDropCategoryLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.OnVisualEditorVisibilityChanged && args.Count == 1)
		{
			OnVisualEditorVisibilityChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName._Process && args.Count == 1)
		{
			_Process(VariantUtils.ConvertTo<double>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.HasCompleteDropItemVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteDropItemVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BindWorkbench && args.Count == 1)
		{
			BindWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindResourceMetadata && args.Count == 1)
		{
			BindResourceMetadata(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindConfigSignals && args.Count == 0)
		{
			BindConfigSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.MountObjectIdLibrary && args.Count == 1)
		{
			MountObjectIdLibrary(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildCategoryCards && args.Count == 0)
		{
			BuildCategoryCards();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildHandlerCards && args.Count == 0)
		{
			BuildHandlerCards();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyBuiltInHandler && args.Count == 1)
		{
			ApplyBuiltInHandler(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateVisualCard && args.Count == 6)
		{
			ret = VariantUtils.CreateFrom<XWGameVisualChoiceCard>(CreateVisualCard(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]), VariantUtils.ConvertTo<bool>(in args[4]), VariantUtils.ConvertTo<Vector2>(in args[5])));
			return true;
		}
		if (method == MethodName.ClearVisualCards && args.Count == 1)
		{
			ClearVisualCards(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectVisualCard && args.Count == 2)
		{
			SelectVisualCard(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetObjectVisualCategory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetObjectVisualCategory(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadObjectVisualIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadObjectVisualIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BindDropItemName && args.Count == 1)
		{
			BindDropItemName(VariantUtils.ConvertTo<DropItemConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitDropItemName && args.Count == 2)
		{
			CommitDropItemName(VariantUtils.ConvertTo<DropItemConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnScenePicked && args.Count == 1)
		{
			OnScenePicked(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnHandlerPicked && args.Count == 1)
		{
			OnHandlerPicked(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowAudioSelector && args.Count == 1)
		{
			ShowAudioSelector(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureAudioSelector && args.Count == 0)
		{
			EnsureAudioSelector();
			ret = default;
			return true;
		}
		if (method == MethodName.SetAudioKey && args.Count == 3)
		{
			SetAudioKey(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderConfigWorkbench && args.Count == 0)
		{
			RenderConfigWorkbench();
			ret = default;
			return true;
		}
		if (method == MethodName.RenderHandlerWorkbench && args.Count == 0)
		{
			RenderHandlerWorkbench();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateConfigControls && args.Count == 0)
		{
			PopulateConfigControls();
			ret = default;
			return true;
		}
		if (method == MethodName.OnDropItemPropertyEdited && args.Count == 1)
		{
			OnDropItemPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDropItemVisuals && args.Count == 1)
		{
			RefreshDropItemVisuals(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDropItemEditorFromHistory && args.Count == 0)
		{
			RefreshDropItemEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateConfigPreviewLabels && args.Count == 0)
		{
			UpdateConfigPreviewLabels();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildDropItemScenePreview && args.Count == 0)
		{
			RebuildDropItemScenePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.DisablePreviewGameplayProcessing && args.Count == 1)
		{
			DisablePreviewGameplayProcessing(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CollectVisualPreviewNodes && args.Count == 1)
		{
			CollectVisualPreviewNodes(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreviewProcessingVisibility && args.Count == 0)
		{
			UpdatePreviewProcessingVisibility();
			ret = default;
			return true;
		}
		if (method == MethodName.CenterSceneInstance && args.Count == 1)
		{
			CenterSceneInstance(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartDropPreview && args.Count == 0)
		{
			StartDropPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.StartPickPreview && args.Count == 0)
		{
			StartPickPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateDropPreview && args.Count == 0)
		{
			UpdateDropPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePickPreview && args.Count == 0)
		{
			UpdatePickPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetPreviewTransform && args.Count == 0)
		{
			ResetPreviewTransform();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateHandlerPreview && args.Count == 0)
		{
			UpdateHandlerPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.PlayConfiguredAudio && args.Count == 2)
		{
			PlayConfiguredAudio(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 0)
		{
			AddSummaryRows();
			ret = default;
			return true;
		}
		if (method == MethodName.GetBuiltInHandlerId && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetBuiltInHandlerId(VariantUtils.ConvertTo<DropItemHandler>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateBuiltInHandler && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemHandler>(CreateBuiltInHandler(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetHandlerDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetHandlerDisplayName(VariantUtils.ConvertTo<DropItemHandler>(in args[0])));
			return true;
		}
		if (method == MethodName.GetHandlerDescription && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetHandlerDescription(VariantUtils.ConvertTo<DropItemHandler>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildHandlerOutcome && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildHandlerOutcome(VariantUtils.ConvertTo<DropItemHandler>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.GetDropCategoryLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetDropCategoryLabel(VariantUtils.ConvertTo<TowerDefenseEnum.DROP_ITEM_CATEGORY>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatResourcePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResourcePath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasCompleteDropItemVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteDropItemVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.ClearVisualCards && args.Count == 1)
		{
			ClearVisualCards(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectVisualCard && args.Count == 2)
		{
			SelectVisualCard(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetObjectVisualCategory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetObjectVisualCategory(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadObjectVisualIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadObjectVisualIcon(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.DisablePreviewGameplayProcessing && args.Count == 1)
		{
			DisablePreviewGameplayProcessing(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CenterSceneInstance && args.Count == 1)
		{
			CenterSceneInstance(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetBuiltInHandlerId && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetBuiltInHandlerId(VariantUtils.ConvertTo<DropItemHandler>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateBuiltInHandler && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<DropItemHandler>(CreateBuiltInHandler(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.GetHandlerDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetHandlerDisplayName(VariantUtils.ConvertTo<DropItemHandler>(in args[0])));
			return true;
		}
		if (method == MethodName.GetHandlerDescription && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetHandlerDescription(VariantUtils.ConvertTo<DropItemHandler>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildHandlerOutcome && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(BuildHandlerOutcome(VariantUtils.ConvertTo<DropItemHandler>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.GetDropCategoryLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetDropCategoryLabel(VariantUtils.ConvertTo<TowerDefenseEnum.DROP_ITEM_CATEGORY>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatResourcePath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResourcePath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
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
		if (method == MethodName.OnVisualEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName._Process)
		{
			return true;
		}
		if (method == MethodName.HasCompleteDropItemVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.BindWorkbench)
		{
			return true;
		}
		if (method == MethodName.BindResourceMetadata)
		{
			return true;
		}
		if (method == MethodName.BindConfigSignals)
		{
			return true;
		}
		if (method == MethodName.MountObjectIdLibrary)
		{
			return true;
		}
		if (method == MethodName.BuildCategoryCards)
		{
			return true;
		}
		if (method == MethodName.BuildHandlerCards)
		{
			return true;
		}
		if (method == MethodName.ApplyBuiltInHandler)
		{
			return true;
		}
		if (method == MethodName.CreateVisualCard)
		{
			return true;
		}
		if (method == MethodName.ClearVisualCards)
		{
			return true;
		}
		if (method == MethodName.SelectVisualCard)
		{
			return true;
		}
		if (method == MethodName.GetObjectVisualCategory)
		{
			return true;
		}
		if (method == MethodName.LoadObjectVisualIcon)
		{
			return true;
		}
		if (method == MethodName.BindDropItemName)
		{
			return true;
		}
		if (method == MethodName.CommitDropItemName)
		{
			return true;
		}
		if (method == MethodName.OnScenePicked)
		{
			return true;
		}
		if (method == MethodName.OnHandlerPicked)
		{
			return true;
		}
		if (method == MethodName.ShowAudioSelector)
		{
			return true;
		}
		if (method == MethodName.EnsureAudioSelector)
		{
			return true;
		}
		if (method == MethodName.SetAudioKey)
		{
			return true;
		}
		if (method == MethodName.RenderConfigWorkbench)
		{
			return true;
		}
		if (method == MethodName.RenderHandlerWorkbench)
		{
			return true;
		}
		if (method == MethodName.PopulateConfigControls)
		{
			return true;
		}
		if (method == MethodName.OnDropItemPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshDropItemVisuals)
		{
			return true;
		}
		if (method == MethodName.RefreshDropItemEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.UpdateConfigPreviewLabels)
		{
			return true;
		}
		if (method == MethodName.RebuildDropItemScenePreview)
		{
			return true;
		}
		if (method == MethodName.DisablePreviewGameplayProcessing)
		{
			return true;
		}
		if (method == MethodName.CollectVisualPreviewNodes)
		{
			return true;
		}
		if (method == MethodName.UpdatePreviewProcessingVisibility)
		{
			return true;
		}
		if (method == MethodName.CenterSceneInstance)
		{
			return true;
		}
		if (method == MethodName.StartDropPreview)
		{
			return true;
		}
		if (method == MethodName.StartPickPreview)
		{
			return true;
		}
		if (method == MethodName.UpdateDropPreview)
		{
			return true;
		}
		if (method == MethodName.UpdatePickPreview)
		{
			return true;
		}
		if (method == MethodName.ResetPreviewTransform)
		{
			return true;
		}
		if (method == MethodName.UpdateHandlerPreview)
		{
			return true;
		}
		if (method == MethodName.PlayConfiguredAudio)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
		{
			return true;
		}
		if (method == MethodName.GetBuiltInHandlerId)
		{
			return true;
		}
		if (method == MethodName.CreateBuiltInHandler)
		{
			return true;
		}
		if (method == MethodName.GetHandlerDisplayName)
		{
			return true;
		}
		if (method == MethodName.GetHandlerDescription)
		{
			return true;
		}
		if (method == MethodName.BuildHandlerOutcome)
		{
			return true;
		}
		if (method == MethodName.GetDropCategoryLabel)
		{
			return true;
		}
		if (method == MethodName.FormatResourcePath)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editingConfig)
		{
			_editingConfig = VariantUtils.ConvertTo<DropItemConfig>(in value);
			return true;
		}
		if (name == PropertyName._editingHandler)
		{
			_editingHandler = VariantUtils.ConvertTo<DropItemHandler>(in value);
			return true;
		}
		if (name == PropertyName._scenePicker)
		{
			_scenePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._handlerPicker)
		{
			_handlerPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._audioPicker)
		{
			_audioPicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._audioTargetProperty)
		{
			_audioTargetProperty = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._configFields)
		{
			_configFields = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._handlerFields)
		{
			_handlerFields = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._resourceNameEdit)
		{
			_resourceNameEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._localToSceneCheck)
		{
			_localToSceneCheck = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._title)
		{
			_title = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._resourceType)
		{
			_resourceType = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._status)
		{
			_status = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._runtimeState)
		{
			_runtimeState = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._sceneStatus)
		{
			_sceneStatus = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._valueLabel)
		{
			_valueLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._missingScene)
		{
			_missingScene = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._handlerTypeLabel)
		{
			_handlerTypeLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._handlerEffectLabel)
		{
			_handlerEffectLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._handlerOutcome)
		{
			_handlerOutcome = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._nameEdit)
		{
			_nameEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._valueSpin)
		{
			_valueSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._poolSpin)
		{
			_poolSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._coinIdSpin)
		{
			_coinIdSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._categoryCards)
		{
			_categoryCards = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._handlerCards)
		{
			_handlerCards = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._objectIdPicker)
		{
			_objectIdPicker = VariantUtils.ConvertTo<XWObjectPoolVisualPicker>(in value);
			return true;
		}
		if (name == PropertyName._fallAudioEdit)
		{
			_fallAudioEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._pickAudioEdit)
		{
			_pickAudioEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._directAudioPlayer)
		{
			_directAudioPlayer = VariantUtils.ConvertTo<AudioStreamPlayer>(in value);
			return true;
		}
		if (name == PropertyName._handlerSimulationCount)
		{
			_handlerSimulationCount = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._previewRoot)
		{
			_previewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._sceneHost)
		{
			_sceneHost = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._fallbackDrop)
		{
			_fallbackDrop = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._previewViewport)
		{
			_previewViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._previewState)
		{
			_previewState = VariantUtils.ConvertTo<PreviewState>(in value);
			return true;
		}
		if (name == PropertyName._previewElapsed)
		{
			_previewElapsed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._pickStartPosition)
		{
			_pickStartPosition = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.DropCategoryVisualCardCount)
		{
			from = DropCategoryVisualCardCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.DropHandlerVisualCardCount)
		{
			from = DropHandlerVisualCardCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.HasObjectLibrarySearchAndPaging)
		{
			from2 = HasObjectLibrarySearchAndPaging;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.IsDropItemPreviewRendering)
		{
			from2 = IsDropItemPreviewRendering;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._editingConfig)
		{
			value = VariantUtils.CreateFrom(in _editingConfig);
			return true;
		}
		if (name == PropertyName._editingHandler)
		{
			value = VariantUtils.CreateFrom(in _editingHandler);
			return true;
		}
		if (name == PropertyName._scenePicker)
		{
			value = VariantUtils.CreateFrom(in _scenePicker);
			return true;
		}
		if (name == PropertyName._handlerPicker)
		{
			value = VariantUtils.CreateFrom(in _handlerPicker);
			return true;
		}
		if (name == PropertyName._audioPicker)
		{
			value = VariantUtils.CreateFrom(in _audioPicker);
			return true;
		}
		if (name == PropertyName._audioTargetProperty)
		{
			value = VariantUtils.CreateFrom(in _audioTargetProperty);
			return true;
		}
		if (name == PropertyName._configFields)
		{
			value = VariantUtils.CreateFrom(in _configFields);
			return true;
		}
		if (name == PropertyName._handlerFields)
		{
			value = VariantUtils.CreateFrom(in _handlerFields);
			return true;
		}
		if (name == PropertyName._resourceNameEdit)
		{
			value = VariantUtils.CreateFrom(in _resourceNameEdit);
			return true;
		}
		if (name == PropertyName._localToSceneCheck)
		{
			value = VariantUtils.CreateFrom(in _localToSceneCheck);
			return true;
		}
		if (name == PropertyName._title)
		{
			value = VariantUtils.CreateFrom(in _title);
			return true;
		}
		if (name == PropertyName._resourceType)
		{
			value = VariantUtils.CreateFrom(in _resourceType);
			return true;
		}
		if (name == PropertyName._status)
		{
			value = VariantUtils.CreateFrom(in _status);
			return true;
		}
		if (name == PropertyName._runtimeState)
		{
			value = VariantUtils.CreateFrom(in _runtimeState);
			return true;
		}
		if (name == PropertyName._sceneStatus)
		{
			value = VariantUtils.CreateFrom(in _sceneStatus);
			return true;
		}
		if (name == PropertyName._valueLabel)
		{
			value = VariantUtils.CreateFrom(in _valueLabel);
			return true;
		}
		if (name == PropertyName._missingScene)
		{
			value = VariantUtils.CreateFrom(in _missingScene);
			return true;
		}
		if (name == PropertyName._handlerTypeLabel)
		{
			value = VariantUtils.CreateFrom(in _handlerTypeLabel);
			return true;
		}
		if (name == PropertyName._handlerEffectLabel)
		{
			value = VariantUtils.CreateFrom(in _handlerEffectLabel);
			return true;
		}
		if (name == PropertyName._handlerOutcome)
		{
			value = VariantUtils.CreateFrom(in _handlerOutcome);
			return true;
		}
		if (name == PropertyName._nameEdit)
		{
			value = VariantUtils.CreateFrom(in _nameEdit);
			return true;
		}
		if (name == PropertyName._valueSpin)
		{
			value = VariantUtils.CreateFrom(in _valueSpin);
			return true;
		}
		if (name == PropertyName._poolSpin)
		{
			value = VariantUtils.CreateFrom(in _poolSpin);
			return true;
		}
		if (name == PropertyName._coinIdSpin)
		{
			value = VariantUtils.CreateFrom(in _coinIdSpin);
			return true;
		}
		if (name == PropertyName._categoryCards)
		{
			value = VariantUtils.CreateFrom(in _categoryCards);
			return true;
		}
		if (name == PropertyName._handlerCards)
		{
			value = VariantUtils.CreateFrom(in _handlerCards);
			return true;
		}
		if (name == PropertyName._objectIdPicker)
		{
			value = VariantUtils.CreateFrom(in _objectIdPicker);
			return true;
		}
		if (name == PropertyName._fallAudioEdit)
		{
			value = VariantUtils.CreateFrom(in _fallAudioEdit);
			return true;
		}
		if (name == PropertyName._pickAudioEdit)
		{
			value = VariantUtils.CreateFrom(in _pickAudioEdit);
			return true;
		}
		if (name == PropertyName._directAudioPlayer)
		{
			value = VariantUtils.CreateFrom(in _directAudioPlayer);
			return true;
		}
		if (name == PropertyName._handlerSimulationCount)
		{
			value = VariantUtils.CreateFrom(in _handlerSimulationCount);
			return true;
		}
		if (name == PropertyName._previewRoot)
		{
			value = VariantUtils.CreateFrom(in _previewRoot);
			return true;
		}
		if (name == PropertyName._sceneHost)
		{
			value = VariantUtils.CreateFrom(in _sceneHost);
			return true;
		}
		if (name == PropertyName._fallbackDrop)
		{
			value = VariantUtils.CreateFrom(in _fallbackDrop);
			return true;
		}
		if (name == PropertyName._previewViewport)
		{
			value = VariantUtils.CreateFrom(in _previewViewport);
			return true;
		}
		if (name == PropertyName._previewState)
		{
			value = VariantUtils.CreateFrom(in _previewState);
			return true;
		}
		if (name == PropertyName._previewElapsed)
		{
			value = VariantUtils.CreateFrom(in _previewElapsed);
			return true;
		}
		if (name == PropertyName._pickStartPosition)
		{
			value = VariantUtils.CreateFrom(in _pickStartPosition);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingConfig, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingHandler, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scenePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._handlerPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._audioPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._audioTargetProperty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._configFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._handlerFields, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceNameEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToSceneCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._title, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._status, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sceneStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._valueLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._missingScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._handlerTypeLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._handlerEffectLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._handlerOutcome, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._nameEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._valueSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._poolSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._coinIdSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._categoryCards, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._handlerCards, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._objectIdPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fallAudioEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pickAudioEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._directAudioPlayer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._handlerSimulationCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._sceneHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fallbackDrop, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._previewState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._previewElapsed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName._pickStartPosition, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DropCategoryVisualCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.DropHandlerVisualCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasObjectLibrarySearchAndPaging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsDropItemPreviewRendering, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingConfig, Variant.From(in _editingConfig));
		info.AddProperty(PropertyName._editingHandler, Variant.From(in _editingHandler));
		info.AddProperty(PropertyName._scenePicker, Variant.From(in _scenePicker));
		info.AddProperty(PropertyName._handlerPicker, Variant.From(in _handlerPicker));
		info.AddProperty(PropertyName._audioPicker, Variant.From(in _audioPicker));
		info.AddProperty(PropertyName._audioTargetProperty, Variant.From(in _audioTargetProperty));
		info.AddProperty(PropertyName._configFields, Variant.From(in _configFields));
		info.AddProperty(PropertyName._handlerFields, Variant.From(in _handlerFields));
		info.AddProperty(PropertyName._resourceNameEdit, Variant.From(in _resourceNameEdit));
		info.AddProperty(PropertyName._localToSceneCheck, Variant.From(in _localToSceneCheck));
		info.AddProperty(PropertyName._title, Variant.From(in _title));
		info.AddProperty(PropertyName._resourceType, Variant.From(in _resourceType));
		info.AddProperty(PropertyName._status, Variant.From(in _status));
		info.AddProperty(PropertyName._runtimeState, Variant.From(in _runtimeState));
		info.AddProperty(PropertyName._sceneStatus, Variant.From(in _sceneStatus));
		info.AddProperty(PropertyName._valueLabel, Variant.From(in _valueLabel));
		info.AddProperty(PropertyName._missingScene, Variant.From(in _missingScene));
		info.AddProperty(PropertyName._handlerTypeLabel, Variant.From(in _handlerTypeLabel));
		info.AddProperty(PropertyName._handlerEffectLabel, Variant.From(in _handlerEffectLabel));
		info.AddProperty(PropertyName._handlerOutcome, Variant.From(in _handlerOutcome));
		info.AddProperty(PropertyName._nameEdit, Variant.From(in _nameEdit));
		info.AddProperty(PropertyName._valueSpin, Variant.From(in _valueSpin));
		info.AddProperty(PropertyName._poolSpin, Variant.From(in _poolSpin));
		info.AddProperty(PropertyName._coinIdSpin, Variant.From(in _coinIdSpin));
		info.AddProperty(PropertyName._categoryCards, Variant.From(in _categoryCards));
		info.AddProperty(PropertyName._handlerCards, Variant.From(in _handlerCards));
		info.AddProperty(PropertyName._objectIdPicker, Variant.From(in _objectIdPicker));
		info.AddProperty(PropertyName._fallAudioEdit, Variant.From(in _fallAudioEdit));
		info.AddProperty(PropertyName._pickAudioEdit, Variant.From(in _pickAudioEdit));
		info.AddProperty(PropertyName._directAudioPlayer, Variant.From(in _directAudioPlayer));
		info.AddProperty(PropertyName._handlerSimulationCount, Variant.From(in _handlerSimulationCount));
		info.AddProperty(PropertyName._previewRoot, Variant.From(in _previewRoot));
		info.AddProperty(PropertyName._sceneHost, Variant.From(in _sceneHost));
		info.AddProperty(PropertyName._fallbackDrop, Variant.From(in _fallbackDrop));
		info.AddProperty(PropertyName._previewViewport, Variant.From(in _previewViewport));
		info.AddProperty(PropertyName._previewState, Variant.From(in _previewState));
		info.AddProperty(PropertyName._previewElapsed, Variant.From(in _previewElapsed));
		info.AddProperty(PropertyName._pickStartPosition, Variant.From(in _pickStartPosition));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingConfig, out var value))
		{
			_editingConfig = value.As<DropItemConfig>();
		}
		if (info.TryGetProperty(PropertyName._editingHandler, out var value2))
		{
			_editingHandler = value2.As<DropItemHandler>();
		}
		if (info.TryGetProperty(PropertyName._scenePicker, out var value3))
		{
			_scenePicker = value3.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._handlerPicker, out var value4))
		{
			_handlerPicker = value4.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._audioPicker, out var value5))
		{
			_audioPicker = value5.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._audioTargetProperty, out var value6))
		{
			_audioTargetProperty = value6.As<string>();
		}
		if (info.TryGetProperty(PropertyName._configFields, out var value7))
		{
			_configFields = value7.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._handlerFields, out var value8))
		{
			_handlerFields = value8.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._resourceNameEdit, out var value9))
		{
			_resourceNameEdit = value9.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToSceneCheck, out var value10))
		{
			_localToSceneCheck = value10.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._title, out var value11))
		{
			_title = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resourceType, out var value12))
		{
			_resourceType = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._status, out var value13))
		{
			_status = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._runtimeState, out var value14))
		{
			_runtimeState = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._sceneStatus, out var value15))
		{
			_sceneStatus = value15.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._valueLabel, out var value16))
		{
			_valueLabel = value16.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._missingScene, out var value17))
		{
			_missingScene = value17.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._handlerTypeLabel, out var value18))
		{
			_handlerTypeLabel = value18.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._handlerEffectLabel, out var value19))
		{
			_handlerEffectLabel = value19.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._handlerOutcome, out var value20))
		{
			_handlerOutcome = value20.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._nameEdit, out var value21))
		{
			_nameEdit = value21.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._valueSpin, out var value22))
		{
			_valueSpin = value22.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._poolSpin, out var value23))
		{
			_poolSpin = value23.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._coinIdSpin, out var value24))
		{
			_coinIdSpin = value24.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._categoryCards, out var value25))
		{
			_categoryCards = value25.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._handlerCards, out var value26))
		{
			_handlerCards = value26.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._objectIdPicker, out var value27))
		{
			_objectIdPicker = value27.As<XWObjectPoolVisualPicker>();
		}
		if (info.TryGetProperty(PropertyName._fallAudioEdit, out var value28))
		{
			_fallAudioEdit = value28.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._pickAudioEdit, out var value29))
		{
			_pickAudioEdit = value29.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._directAudioPlayer, out var value30))
		{
			_directAudioPlayer = value30.As<AudioStreamPlayer>();
		}
		if (info.TryGetProperty(PropertyName._handlerSimulationCount, out var value31))
		{
			_handlerSimulationCount = value31.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._previewRoot, out var value32))
		{
			_previewRoot = value32.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._sceneHost, out var value33))
		{
			_sceneHost = value33.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._fallbackDrop, out var value34))
		{
			_fallbackDrop = value34.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._previewViewport, out var value35))
		{
			_previewViewport = value35.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._previewState, out var value36))
		{
			_previewState = value36.As<PreviewState>();
		}
		if (info.TryGetProperty(PropertyName._previewElapsed, out var value37))
		{
			_previewElapsed = value37.As<double>();
		}
		if (info.TryGetProperty(PropertyName._pickStartPosition, out var value38))
		{
			_pickStartPosition = value38.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value39))
		{
			_updatingControls = value39.As<bool>();
		}
	}
}
