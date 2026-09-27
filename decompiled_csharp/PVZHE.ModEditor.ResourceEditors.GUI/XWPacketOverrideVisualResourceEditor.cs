using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketOverrideVisualResourceEditor.cs")]
public class XWPacketOverrideVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName HasCompletePacketOverrideVisualCoverage = "HasCompletePacketOverrideVisualCoverage";

		public static readonly StringName DisposePacketOverrideBinding = "DisposePacketOverrideBinding";

		public static readonly StringName BindWorkbench = "BindWorkbench";

		public static readonly StringName BindDirectEditControls = "BindDirectEditControls";

		public static readonly StringName OnOverrideTypeSelected = "OnOverrideTypeSelected";

		public static readonly StringName OnPacketOverridePropertyEdited = "OnPacketOverridePropertyEdited";

		public static readonly StringName RefreshDirectEditPreview = "RefreshDirectEditPreview";

		public static readonly StringName RefreshPacketOverrideFromHistory = "RefreshPacketOverrideFromHistory";

		public static readonly StringName PopulateOverrideFields = "PopulateOverrideFields";

		public static readonly StringName AddPlantCover = "AddPlantCover";

		public static readonly StringName RemovePlantCover = "RemovePlantCover";

		public static readonly StringName CloneStringArray = "CloneStringArray";

		public static readonly StringName RefreshLists = "RefreshLists";

		public static readonly StringName PopulateEventList = "PopulateEventList";

		public static readonly StringName ApplyOverridePreview = "ApplyOverridePreview";

		public static readonly StringName UpdateSimulationPreview = "UpdateSimulationPreview";

		public static readonly StringName CalculateEffectiveCost = "CalculateEffectiveCost";

		public static readonly StringName BuildOverrideBadge = "BuildOverrideBadge";

		public static readonly StringName BuildFlagSuffix = "BuildFlagSuffix";

		public static readonly StringName ConfigurePacketCard = "ConfigurePacketCard";

		public static readonly StringName LoadPacketTexture = "LoadPacketTexture";

		public static readonly StringName GetPacketTexturePath = "GetPacketTexturePath";

		public static readonly StringName GetPacketTypeDisplayName = "GetPacketTypeDisplayName";

		public static readonly StringName SelectOptionById = "SelectOptionById";

		public static readonly StringName OpenCharacterOverride = "OpenCharacterOverride";

		public static readonly StringName OnCharacterOverridePicked = "OnCharacterOverridePicked";

		public static readonly StringName ShowPacketEventSelector = "ShowPacketEventSelector";

		public static readonly StringName EnsurePacketEventSelectorWindow = "EnsurePacketEventSelectorWindow";

		public static readonly StringName RemoveSelectedPacketEvent = "RemoveSelectedPacketEvent";

		public static readonly StringName MoveSelectedPacketEvent = "MoveSelectedPacketEvent";

		public static readonly StringName OpenPacketEventResource = "OpenPacketEventResource";

		public static readonly StringName GetPacketEventArray = "GetPacketEventArray";

		public static readonly StringName ClonePacketEventArray = "ClonePacketEventArray";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingOverride = "_editingOverride";

		public static readonly StringName _basePacket = "_basePacket";

		public static readonly StringName _resultPacket = "_resultPacket";

		public static readonly StringName _resultCardRoot = "_resultCardRoot";

		public static readonly StringName _baseSummary = "_baseSummary";

		public static readonly StringName _resultSummary = "_resultSummary";

		public static readonly StringName _overrideState = "_overrideState";

		public static readonly StringName _overrideBadge = "_overrideBadge";

		public static readonly StringName _characterOverrideStatus = "_characterOverrideStatus";

		public static readonly StringName _baseType = "_baseType";

		public static readonly StringName _baseCost = "_baseCost";

		public static readonly StringName _baseCooldown = "_baseCooldown";

		public static readonly StringName _baseStartingCooldown = "_baseStartingCooldown";

		public static readonly StringName _baseWeight = "_baseWeight";

		public static readonly StringName _baseWavePointCost = "_baseWavePointCost";

		public static readonly StringName _overrideType = "_overrideType";

		public static readonly StringName _costRise = "_costRise";

		public static readonly StringName _overrideCost = "_overrideCost";

		public static readonly StringName _costMultiple = "_costMultiple";

		public static readonly StringName _packetCooldown = "_packetCooldown";

		public static readonly StringName _startingCooldown = "_startingCooldown";

		public static readonly StringName _weight = "_weight";

		public static readonly StringName _wavePointCost = "_wavePointCost";

		public static readonly StringName _isLimitGridNum = "_isLimitGridNum";

		public static readonly StringName _coverCanDirectPlant = "_coverCanDirectPlant";

		public static readonly StringName _hypnoses = "_hypnoses";

		public static readonly StringName _plantCoverList = "_plantCoverList";

		public static readonly StringName _plantCoverInput = "_plantCoverInput";

		public static readonly StringName _pressedActionsList = "_pressedActionsList";

		public static readonly StringName _useSucceededActionsList = "_useSucceededActionsList";

		public static readonly StringName _resourceName = "_resourceName";

		public static readonly StringName _localToScene = "_localToScene";

		public static readonly StringName _characterOverridePicker = "_characterOverridePicker";

		public static readonly StringName _packetEventSelectorWindow = "_packetEventSelectorWindow";

		public static readonly StringName _packetEventTargetProperty = "_packetEventTargetProperty";

		public static readonly StringName _previewTween = "_previewTween";

		public static readonly StringName _updatingControls = "_updatingControls";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string VisualEditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketOverrideVisualEditorLayout.tscn";

	private const string PacketEventSelectorScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketEventSelectorWindow.tscn";

	private static PackedScene _visualEditorLayoutScene;

	private static PackedScene _packetEventSelectorScene;

	private static readonly System.Collections.Generic.Dictionary<string, Texture2D> PacketTextureCache = new System.Collections.Generic.Dictionary<string, Texture2D>(StringComparer.Ordinal);

	private static readonly TowerDefenseEnum.PACKET_TYPE[] PacketTypes = new TowerDefenseEnum.PACKET_TYPE[9]
	{
		TowerDefenseEnum.PACKET_TYPE.WHITE,
		TowerDefenseEnum.PACKET_TYPE.GOLD,
		TowerDefenseEnum.PACKET_TYPE.DIAMOND,
		TowerDefenseEnum.PACKET_TYPE.COLOUR,
		TowerDefenseEnum.PACKET_TYPE.STAR,
		TowerDefenseEnum.PACKET_TYPE.ORIGINAL,
		TowerDefenseEnum.PACKET_TYPE.ZOMBIE,
		TowerDefenseEnum.PACKET_TYPE.COVER,
		TowerDefenseEnum.PACKET_TYPE.GRAY
	};

	private TowerDefensePacketOverride _editingOverride;

	private TowerDefenseInGamePacketShow _basePacket;

	private TowerDefenseInGamePacketShow _resultPacket;

	private Control _resultCardRoot;

	private Label _baseSummary;

	private Label _resultSummary;

	private Label _overrideState;

	private Label _overrideBadge;

	private Label _characterOverrideStatus;

	private OptionButton _baseType;

	private XWVisualOptionGallery _baseTypeVisualGallery;

	private SpinBox _baseCost;

	private SpinBox _baseCooldown;

	private SpinBox _baseStartingCooldown;

	private SpinBox _baseWeight;

	private SpinBox _baseWavePointCost;

	private OptionButton _overrideType;

	private XWVisualOptionGallery _overrideTypeVisualGallery;

	private SpinBox _costRise;

	private SpinBox _overrideCost;

	private SpinBox _costMultiple;

	private SpinBox _packetCooldown;

	private SpinBox _startingCooldown;

	private SpinBox _weight;

	private SpinBox _wavePointCost;

	private CheckBox _isLimitGridNum;

	private CheckBox _coverCanDirectPlant;

	private CheckBox _hypnoses;

	private ItemList _plantCoverList;

	private LineEdit _plantCoverInput;

	private ItemList _pressedActionsList;

	private ItemList _useSucceededActionsList;

	private LineEdit _resourceName;

	private CheckButton _localToScene;

	private XWResourcePicker _characterOverridePicker;

	private XWVisualPropertyBinding _propertyBinding;

	private XWPacketEventSelectorWindow _packetEventSelectorWindow;

	private string _packetEventTargetProperty = "pressedActions";

	private Tween _previewTween;

	private bool _updatingControls;

	public override void _ExitTree()
	{
		DisposePacketOverrideBinding();
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposePacketOverrideBinding();
		if (CurrentResource is TowerDefensePacketOverride towerDefensePacketOverride && CanvasGrid != null)
		{
			_editingOverride = towerDefensePacketOverride;
			CanvasGrid.Columns = 1;
			if (_visualEditorLayoutScene == null)
			{
				_visualEditorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketOverrideVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _visualEditorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindWorkbench(vBoxContainer, towerDefensePacketOverride);
				PopulateOverrideFields();
				UpdateSimulationPreview(applied: true);
				AddSummaryRows();
			}
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!HasCompletePacketOverrideVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool HasCompletePacketOverrideVisualCoverage(Resource resource)
	{
		return resource?.GetType() == typeof(TowerDefensePacketOverride);
	}

	private void DisposePacketOverrideBinding()
	{
		_baseTypeVisualGallery?.Dispose();
		_baseTypeVisualGallery = null;
		_overrideTypeVisualGallery?.Dispose();
		_overrideTypeVisualGallery = null;
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		_editingOverride = null;
	}

	private void BindWorkbench(VBoxContainer root, TowerDefensePacketOverride packetOverride)
	{
		_basePacket = root.GetNode<TowerDefenseInGamePacketShow>("%BasePacket");
		_resultPacket = root.GetNode<TowerDefenseInGamePacketShow>("%ResultPacket");
		_resultCardRoot = root.GetNode<Control>("%ResultCardRoot");
		_baseSummary = root.GetNode<Label>("%BaseSummary");
		_resultSummary = root.GetNode<Label>("%ResultSummary");
		_overrideState = root.GetNode<Label>("%OverrideState");
		_overrideBadge = root.GetNode<Label>("%OverrideBadge");
		_characterOverrideStatus = root.GetNode<Label>("%CharacterOverrideStatus");
		_baseType = root.GetNode<OptionButton>("%BaseType");
		_baseCost = root.GetNode<SpinBox>("%BaseCost");
		_baseCooldown = root.GetNode<SpinBox>("%BaseCooldown");
		_baseStartingCooldown = root.GetNode<SpinBox>("%BaseStartingCooldown");
		_baseWeight = root.GetNode<SpinBox>("%BaseWeight");
		_baseWavePointCost = root.GetNode<SpinBox>("%BaseWavePointCost");
		_overrideType = root.GetNode<OptionButton>("%OverrideType");
		_costRise = root.GetNode<SpinBox>("%CostRise");
		_overrideCost = root.GetNode<SpinBox>("%OverrideCost");
		_costMultiple = root.GetNode<SpinBox>("%CostMultiple");
		_packetCooldown = root.GetNode<SpinBox>("%PacketCooldown");
		_startingCooldown = root.GetNode<SpinBox>("%StartingCooldown");
		_weight = root.GetNode<SpinBox>("%Weight");
		_wavePointCost = root.GetNode<SpinBox>("%WavePointCost");
		_isLimitGridNum = root.GetNode<CheckBox>("%IsLimitGridNum");
		_coverCanDirectPlant = root.GetNode<CheckBox>("%CoverCanDirectPlant");
		_hypnoses = root.GetNode<CheckBox>("%Hypnoses");
		_plantCoverList = root.GetNode<ItemList>("%PlantCoverList");
		_plantCoverInput = root.GetNode<LineEdit>("%PlantCoverInput");
		_pressedActionsList = root.GetNode<ItemList>("%EventPressList");
		_useSucceededActionsList = root.GetNode<ItemList>("%EventPlantList");
		_resourceName = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToScene = root.GetNode<CheckButton>("%LocalToSceneCheck");
		XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
		xWResourcePicker.Setup("TowerDefenseCharacterOverride");
		xWResourcePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		root.GetNode<VBoxContainer>("%CharacterOverridePickerHost").AddChild(xWResourcePicker, forceReadableName: false, InternalMode.Disabled);
		_characterOverridePicker = xWResourcePicker;
		xWResourcePicker.ResourceChanged += OnCharacterOverridePicked;
		BindDirectEditControls(packetOverride);
		_overrideType.AddItem("继承原卡框");
		TowerDefenseEnum.PACKET_TYPE[] packetTypes = PacketTypes;
		foreach (TowerDefenseEnum.PACKET_TYPE pACKET_TYPE in packetTypes)
		{
			_baseType.AddItem(GetPacketTypeDisplayName(pACKET_TYPE), (int)pACKET_TYPE);
			_overrideType.AddItem(GetPacketTypeDisplayName(pACKET_TYPE), (int)pACKET_TYPE);
		}
		_baseTypeVisualGallery = new XWVisualOptionGallery(_baseType, root.GetNode<HFlowContainer>("%BaseTypeVisualChoices"), "BasePacketTypeVisualCatalog", (int index) => LoadPacketTexture((TowerDefenseEnum.PACKET_TYPE)_baseType.GetItemId(index)));
		_baseTypeVisualGallery.Rebuild();
		_overrideTypeVisualGallery = new XWVisualOptionGallery(_overrideType, root.GetNode<HFlowContainer>("%OverrideTypeVisualChoices"), "OverridePacketTypeVisualCatalog", (int index) => (_overrideType.GetItemId(index) != -1) ? LoadPacketTexture((TowerDefenseEnum.PACKET_TYPE)_overrideType.GetItemId(index)) : ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ResourceCard.svg", null, ResourceLoader.CacheMode.Reuse));
		_overrideTypeVisualGallery.Rebuild();
		ConfigurePacketCard(_basePacket, 100, TowerDefenseEnum.PACKET_TYPE.WHITE);
		ConfigurePacketCard(_resultPacket, 100, TowerDefenseEnum.PACKET_TYPE.WHITE);
		root.GetNode<Button>("%ApplyButton").Pressed += ApplyOverridePreview;
		root.GetNode<Button>("%ResetButton").Pressed += () =>
		{
			UpdateSimulationPreview(applied: false);
		};
		root.GetNode<Button>("%AddPlantCoverButton").Pressed += AddPlantCover;
		root.GetNode<Button>("%RemovePlantCoverButton").Pressed += RemovePlantCover;
		root.GetNode<Button>("%CharacterOverrideButton").Pressed += OpenCharacterOverride;
		root.GetNode<Button>("%AddPressEventButton").Pressed += () =>
		{
			ShowPacketEventSelector("pressedActions");
		};
		root.GetNode<Button>("%AddPlantEventButton").Pressed += () =>
		{
			ShowPacketEventSelector("useSucceededActions");
		};
		root.GetNode<Button>("%MovePressEventUpButton").Pressed += () =>
		{
			MoveSelectedPacketEvent("pressedActions", -1);
		};
		root.GetNode<Button>("%MovePressEventDownButton").Pressed += () =>
		{
			MoveSelectedPacketEvent("pressedActions", 1);
		};
		root.GetNode<Button>("%MovePlantEventUpButton").Pressed += () =>
		{
			MoveSelectedPacketEvent("useSucceededActions", -1);
		};
		root.GetNode<Button>("%MovePlantEventDownButton").Pressed += () =>
		{
			MoveSelectedPacketEvent("useSucceededActions", 1);
		};
		root.GetNode<Button>("%RemovePressEventButton").Pressed += () =>
		{
			RemoveSelectedPacketEvent("pressedActions");
		};
		root.GetNode<Button>("%RemovePlantEventButton").Pressed += () =>
		{
			RemoveSelectedPacketEvent("useSucceededActions");
		};
		_pressedActionsList.ItemActivated += (long index) =>
		{
			OpenPacketEventResource("pressedActions", (int)index);
		};
		_useSucceededActionsList.ItemActivated += (long index) =>
		{
			OpenPacketEventResource("useSucceededActions", (int)index);
		};
		_baseType.ItemSelected += (long _) =>
		{
			UpdateSimulationPreview(applied: true);
		};
		_baseCost.ValueChanged += (double _) =>
		{
			UpdateSimulationPreview(applied: true);
		};
		_baseCooldown.ValueChanged += (double _) =>
		{
			UpdateSimulationPreview(applied: true);
		};
		_baseStartingCooldown.ValueChanged += (double _) =>
		{
			UpdateSimulationPreview(applied: true);
		};
		_baseWeight.ValueChanged += (double _) =>
		{
			UpdateSimulationPreview(applied: true);
		};
		_baseWavePointCost.ValueChanged += (double _) =>
		{
			UpdateSimulationPreview(applied: true);
		};
		_overrideType.ItemSelected += OnOverrideTypeSelected;
	}

	private void BindDirectEditControls(TowerDefensePacketOverride packetOverride)
	{
		_propertyBinding?.Dispose();
		_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnPacketOverridePropertyEdited);
		_propertyBinding.BindText(_resourceName, packetOverride, "resource_name", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
		_propertyBinding.BindToggle(_localToScene, packetOverride, "resource_local_to_scene", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
		_propertyBinding.BindNumber(_costRise, packetOverride, "costRise", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
		_propertyBinding.BindNumber(_overrideCost, packetOverride, "cost", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
		_propertyBinding.BindNumber(_costMultiple, packetOverride, "costMultiple", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
		_propertyBinding.BindNumber(_packetCooldown, packetOverride, "packetCooldown", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
		_propertyBinding.BindNumber(_startingCooldown, packetOverride, "startingCooldown", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
		_propertyBinding.BindNumber(_weight, packetOverride, "weight", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
		_propertyBinding.BindNumber(_wavePointCost, packetOverride, "wavePointCost", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
		_propertyBinding.BindToggle(_isLimitGridNum, packetOverride, "islimitGridNum", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
		_propertyBinding.BindToggle(_coverCanDirectPlant, packetOverride, "coverCanDirectPlant", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
		_propertyBinding.BindToggle(_hypnoses, packetOverride, "hypnoses", RefreshDirectEditPreview, this, "RefreshPacketOverrideFromHistory");
	}

	private void OnOverrideTypeSelected(long index)
	{
		if (!_updatingControls && _propertyBinding != null && GodotObject.IsInstanceValid(_editingOverride) && index >= 0 && index < _overrideType.ItemCount)
		{
			_propertyBinding.SetValue(_editingOverride, "type", Variant.From<long>((long)_overrideType.GetItemId((int)index)), "修改覆盖卡框", this, "RefreshPacketOverrideFromHistory");
			UpdateSimulationPreview(applied: true);
		}
	}

	private void OnPacketOverridePropertyEdited(bool committed)
	{
		if (CurrentResource is TowerDefensePacketOverride towerDefensePacketOverride && towerDefensePacketOverride == _editingOverride)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
			}
			else
			{
				MarkCurrentResourceDirty();
				towerDefensePacketOverride.EmitChanged();
			}
			RefreshDirectEditPreview();
		}
	}

	private void RefreshDirectEditPreview()
	{
		UpdateSimulationPreview(applied: true);
		RefreshLists();
	}

	public void RefreshPacketOverrideFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()) && CurrentResource is TowerDefensePacketOverride towerDefensePacketOverride && towerDefensePacketOverride == _editingOverride)
		{
			BindDirectEditControls(towerDefensePacketOverride);
			PopulateOverrideFields();
			UpdateSimulationPreview(applied: true);
		}
	}

	private void PopulateOverrideFields()
	{
		if (!GodotObject.IsInstanceValid(_editingOverride) || !GodotObject.IsInstanceValid(_overrideType))
		{
			return;
		}
		_updatingControls = true;
		try
		{
			SelectOptionById(_overrideType, (int)_editingOverride.type);
			_overrideTypeVisualGallery?.RefreshSelection();
			_costRise.Value = _editingOverride.costRise;
			_overrideCost.Value = _editingOverride.cost;
			_costMultiple.Value = _editingOverride.costMultiple;
			_packetCooldown.Value = _editingOverride.packetCooldown;
			_startingCooldown.Value = _editingOverride.startingCooldown;
			_weight.Value = _editingOverride.weight;
			_wavePointCost.Value = _editingOverride.wavePointCost;
			_isLimitGridNum.ButtonPressed = _editingOverride.islimitGridNum;
			_coverCanDirectPlant.ButtonPressed = _editingOverride.coverCanDirectPlant;
			_hypnoses.ButtonPressed = _editingOverride.hypnoses;
			_characterOverridePicker?.SetEditedResource(_editingOverride.characterOverride);
			_characterOverrideStatus.Text = (GodotObject.IsInstanceValid(_editingOverride.characterOverride) ? "角色覆盖：已配置" : "角色覆盖：默认");
			RefreshLists();
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void AddPlantCover()
	{
		if (!GodotObject.IsInstanceValid(_editingOverride) || _propertyBinding == null)
		{
			return;
		}
		string text = _plantCoverInput.Text.StripEdges();
		if (!string.IsNullOrWhiteSpace(text))
		{
			Array<string> array = CloneStringArray(_editingOverride.plantCover);
			if (!array.Contains(text))
			{
				array.Add(text);
				_propertyBinding.SetValue(_editingOverride, "plantCover", array, "添加可覆盖植物", this, "RefreshPacketOverrideFromHistory");
				_plantCoverInput.Text = "";
				RefreshDirectEditPreview();
			}
		}
	}

	private void RemovePlantCover()
	{
		if (GodotObject.IsInstanceValid(_editingOverride) && _propertyBinding != null && _editingOverride.plantCover != null)
		{
			int[] selectedItems = _plantCoverList.GetSelectedItems();
			if (selectedItems.Length != 0 && selectedItems[0] >= 0 && selectedItems[0] < _editingOverride.plantCover.Count)
			{
				Array<string> array = CloneStringArray(_editingOverride.plantCover);
				array.RemoveAt(selectedItems[0]);
				_propertyBinding.SetValue(_editingOverride, "plantCover", array, "删除可覆盖植物", this, "RefreshPacketOverrideFromHistory");
				RefreshDirectEditPreview();
			}
		}
	}

	private static Array<string> CloneStringArray(Array<string> source)
	{
		Array<string> array = new Array<string>();
		if (source != null)
		{
			foreach (string item in source)
			{
				array.Add(item);
			}
		}
		return array;
	}

	private void RefreshLists()
	{
		if (!GodotObject.IsInstanceValid(_editingOverride))
		{
			return;
		}
		_plantCoverList?.Clear();
		if (_editingOverride.plantCover != null && GodotObject.IsInstanceValid(_plantCoverList))
		{
			foreach (string item in _editingOverride.plantCover)
			{
				_plantCoverList.AddItem(item);
			}
		}
		PopulateEventList(_pressedActionsList, _editingOverride.pressedActions);
		PopulateEventList(_useSucceededActionsList, _editingOverride.useSucceededActions);
	}

	private static void PopulateEventList(ItemList list, Array<CardActionBehaviorDefinition> events)
	{
		if (!GodotObject.IsInstanceValid(list))
		{
			return;
		}
		list.Clear();
		if (events != null)
		{
			for (int i = 0; i < events.Count; i++)
			{
				CardActionBehaviorDefinition cardActionBehaviorDefinition = events[i];
				list.AddItem(GodotObject.IsInstanceValid(cardActionBehaviorDefinition) ? $"{i + 1}. {cardActionBehaviorDefinition.GetType().Name}" : $"{i + 1}. <空事件>");
			}
		}
	}

	private void ApplyOverridePreview()
	{
		UpdateSimulationPreview(applied: true);
		if (GodotObject.IsInstanceValid(_resultCardRoot))
		{
			if (GodotObject.IsInstanceValid(_previewTween))
			{
				_previewTween.Kill();
			}
			_resultCardRoot.Scale = Vector2.One * 0.78f;
			_previewTween = CreateTween();
			_previewTween.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
			_previewTween.TweenProperty(_resultCardRoot, "scale", Vector2.One * 1.08f, 0.22);
			_previewTween.TweenProperty(_resultCardRoot, "scale", Vector2.One, 0.14);
		}
	}

	private void UpdateSimulationPreview(bool applied)
	{
		if (GodotObject.IsInstanceValid(_editingOverride) && GodotObject.IsInstanceValid(_basePacket))
		{
			if (GodotObject.IsInstanceValid(_previewTween))
			{
				_previewTween.Kill();
				_previewTween = null;
			}
			TowerDefenseEnum.PACKET_TYPE selectedId = (TowerDefenseEnum.PACKET_TYPE)_baseType.GetSelectedId();
			int num = Mathf.RoundToInt(_baseCost.Value);
			double value = _baseCooldown.Value;
			double value2 = _baseStartingCooldown.Value;
			int num2 = Mathf.RoundToInt(_baseWeight.Value);
			int num3 = Mathf.RoundToInt(_baseWavePointCost.Value);
			TowerDefenseEnum.PACKET_TYPE packetType = ((applied && _editingOverride.type != TowerDefenseEnum.PACKET_TYPE.NOONE) ? _editingOverride.type : selectedId);
			int num4 = (applied ? CalculateEffectiveCost(num, _editingOverride) : num);
			double value3 = ((applied && _editingOverride.packetCooldown != -1.0) ? _editingOverride.packetCooldown : value);
			double value4 = ((applied && _editingOverride.startingCooldown != -1.0) ? _editingOverride.startingCooldown : value2);
			int value5 = ((applied && _editingOverride.weight != -1) ? _editingOverride.weight : num2);
			int value6 = ((applied && _editingOverride.wavePointCost != -1) ? _editingOverride.wavePointCost : num3);
			ConfigurePacketCard(_basePacket, num, selectedId);
			ConfigurePacketCard(_resultPacket, num4, packetType);
			_baseSummary.Text = $"费用 {num} · 冷却 {value:0.##} 秒 · 开局 {value2:0.##} 秒";
			_resultSummary.Text = (applied ? $"费用 {num4} · 冷却 {value3:0.##} 秒 · 开局 {value4:0.##} 秒" : _baseSummary.Text);
			_resultCardRoot.Modulate = ((applied && _editingOverride.hypnoses) ? new Color(0.83f, 0.68f, 1f) : Colors.White);
			_overrideBadge.Text = BuildOverrideBadge();
			_overrideState.Text = (applied ? $"{GetPacketTypeDisplayName(selectedId)} → {GetPacketTypeDisplayName(packetType)} · 僵尸权重 {num2}→{value5} · 波次点数 {num3}→{value6}{BuildFlagSuffix()}" : "当前显示继承值；点击应用覆盖规则查看游戏最终结果。");
			_resultCardRoot.Scale = Vector2.One;
		}
	}

	private static int CalculateEffectiveCost(int baseCost, TowerDefensePacketOverride packetOverride)
	{
		if (packetOverride == null || packetOverride.cost == -1)
		{
			return baseCost;
		}
		return packetOverride.cost;
	}

	private string BuildOverrideBadge()
	{
		int num = 0;
		if (_editingOverride.type != TowerDefenseEnum.PACKET_TYPE.NOONE)
		{
			num++;
		}
		if (_editingOverride.costRise != -1)
		{
			num++;
		}
		if (_editingOverride.cost != -1)
		{
			num++;
		}
		if (_editingOverride.costMultiple != -1.0)
		{
			num++;
		}
		if (_editingOverride.packetCooldown != -1.0)
		{
			num++;
		}
		if (_editingOverride.startingCooldown != -1.0)
		{
			num++;
		}
		if (_editingOverride.weight != -1)
		{
			num++;
		}
		if (_editingOverride.wavePointCost != -1)
		{
			num++;
		}
		if (num != 0)
		{
			return $"已覆盖 {num} 项运行值";
		}
		return "继承原卡牌";
	}

	private string BuildFlagSuffix()
	{
		return (_editingOverride.hypnoses ? " · 催眠" : "") + (_editingOverride.coverCanDirectPlant ? " · 可直接覆盖种植" : "") + ((!_editingOverride.islimitGridNum) ? " · 不限制同格数量" : "") + $" · 按下事件 {_editingOverride.pressedActions?.Count ?? 0} · 种植事件 {_editingOverride.useSucceededActions?.Count ?? 0}";
	}

	private static void ConfigurePacketCard(TowerDefenseInGamePacketShow packet, int cost, TowerDefenseEnum.PACKET_TYPE packetType)
	{
		if (GodotObject.IsInstanceValid(packet))
		{
			packet.ProcessMode = ProcessModeEnum.Disabled;
			packet.onlyDraw = true;
			packet.useCost = false;
			packet.baseItemCost = cost;
			packet.itemCost = cost;
			packet.alive = true;
			packet.@lock = false;
			packet.openShadow = false;
			packet.select = false;
			packet.coldDownOpen = false;
			if (GodotObject.IsInstanceValid(packet.coldDownProgressBar))
			{
				packet.coldDownProgressBar.Visible = false;
			}
			if (GodotObject.IsInstanceValid(packet.backgroundTexture))
			{
				packet.backgroundTexture.Texture = LoadPacketTexture(packetType);
			}
			packet.ColorSet();
		}
	}

	private static Texture2D LoadPacketTexture(TowerDefenseEnum.PACKET_TYPE packetType)
	{
		string packetTexturePath = GetPacketTexturePath(packetType);
		if (!PacketTextureCache.TryGetValue(packetTexturePath, out var value) || !GodotObject.IsInstanceValid(value))
		{
			value = ResourceLoader.Load<Texture2D>(packetTexturePath, null, ResourceLoader.CacheMode.Reuse);
			PacketTextureCache[packetTexturePath] = value;
		}
		return value;
	}

	private static string GetPacketTexturePath(TowerDefenseEnum.PACKET_TYPE packetType)
	{
		return "res://Asset/Texture/TowerDefense/Packet/PC/" + packetType switch
		{
			TowerDefenseEnum.PACKET_TYPE.GOLD => "PacketGold.png", 
			TowerDefenseEnum.PACKET_TYPE.DIAMOND => "PacketDiamond.png", 
			TowerDefenseEnum.PACKET_TYPE.COLOUR => "PacketColour.png", 
			TowerDefenseEnum.PACKET_TYPE.STAR => "PacketStar.png", 
			TowerDefenseEnum.PACKET_TYPE.ZOMBIE => "PacketZombie.png", 
			TowerDefenseEnum.PACKET_TYPE.COVER => "PacketCover.png", 
			TowerDefenseEnum.PACKET_TYPE.GRAY => "PacketGray.png", 
			_ => "PacketNormal.png", 
		};
	}

	private static string GetPacketTypeDisplayName(TowerDefenseEnum.PACKET_TYPE packetType)
	{
		return packetType switch
		{
			TowerDefenseEnum.PACKET_TYPE.NOONE => "继承原卡框", 
			TowerDefenseEnum.PACKET_TYPE.WHITE => "普通白卡", 
			TowerDefenseEnum.PACKET_TYPE.GOLD => "金卡", 
			TowerDefenseEnum.PACKET_TYPE.DIAMOND => "钻石卡", 
			TowerDefenseEnum.PACKET_TYPE.COLOUR => "彩卡", 
			TowerDefenseEnum.PACKET_TYPE.STAR => "星卡", 
			TowerDefenseEnum.PACKET_TYPE.ORIGINAL => "原版卡", 
			TowerDefenseEnum.PACKET_TYPE.ZOMBIE => "僵尸卡", 
			TowerDefenseEnum.PACKET_TYPE.COVER => "覆盖卡", 
			TowerDefenseEnum.PACKET_TYPE.GRAY => "灰卡", 
			_ => packetType.ToString(), 
		};
	}

	private static void SelectOptionById(OptionButton option, int id)
	{
		for (int i = 0; i < option.ItemCount; i++)
		{
			if (option.GetItemId(i) == id)
			{
				option.Select(i);
				return;
			}
		}
		option.Select(0);
	}

	private void OpenCharacterOverride()
	{
		if (GodotObject.IsInstanceValid(_editingOverride) && _propertyBinding != null)
		{
			if (!GodotObject.IsInstanceValid(_editingOverride.characterOverride))
			{
				TowerDefenseCharacterOverride from = new TowerDefenseCharacterOverride();
				_propertyBinding.SetValue(_editingOverride, "characterOverride", Variant.From(in from), "创建角色覆盖", this, "RefreshPacketOverrideFromHistory");
				_characterOverridePicker?.SetEditedResource(from);
				RefreshDirectEditPreview();
			}
			XWResourceEditContext context = XWResourceEditContext.ForProperty(_editingOverride.characterOverride, _editingOverride, _editingOverride.characterOverride.ResourcePath, CurrentResourcePath, "characterOverride", -1, "packet_override_editor", CurrentEditContext?.IsBuiltInSource ?? false);
			XWEditorInterface.Instance?.EditResource(_editingOverride.characterOverride, context);
		}
	}

	private void OnCharacterOverridePicked(Resource resource)
	{
		if (!_updatingControls && _propertyBinding != null && GodotObject.IsInstanceValid(_editingOverride) && (resource == null || resource is TowerDefenseCharacterOverride))
		{
			_propertyBinding.SetValue(_editingOverride, "characterOverride", Variant.From(in resource), "更换角色覆盖", this, "RefreshPacketOverrideFromHistory");
			RefreshDirectEditPreview();
			_characterOverrideStatus.Text = (GodotObject.IsInstanceValid(resource) ? "角色覆盖：已配置" : "角色覆盖：默认");
		}
	}

	private void ShowPacketEventSelector(string targetProperty)
	{
		_packetEventTargetProperty = ((targetProperty == "useSucceededActions") ? "useSucceededActions" : "pressedActions");
		EnsurePacketEventSelectorWindow();
		if (GodotObject.IsInstanceValid(_packetEventSelectorWindow))
		{
			string title = ((_packetEventTargetProperty == "useSucceededActions") ? "覆盖种植事件拼图" : "覆盖按下事件拼图");
			string subtitle = ((_packetEventTargetProperty == "useSucceededActions") ? "为覆盖卡牌配置完成种植后的事件链" : "为覆盖卡牌配置拿起或按下时的事件链");
			_packetEventSelectorWindow.ShowFor(title, subtitle, OnPacketEventSelected);
		}
	}

	private void EnsurePacketEventSelectorWindow()
	{
		if (!GodotObject.IsInstanceValid(_packetEventSelectorWindow))
		{
			if (_packetEventSelectorScene == null)
			{
				_packetEventSelectorScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketEventSelectorWindow.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			_packetEventSelectorWindow = _packetEventSelectorScene?.Instantiate<XWPacketEventSelectorWindow>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(_packetEventSelectorWindow))
			{
				AddChild(_packetEventSelectorWindow, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void OnPacketEventSelected(Type runtimeType)
	{
		CardActionBehaviorDefinition cardActionBehaviorDefinition = CreatePacketEventInstance(runtimeType);
		if (GodotObject.IsInstanceValid(cardActionBehaviorDefinition) && _propertyBinding != null && GodotObject.IsInstanceValid(_editingOverride))
		{
			Array<CardActionBehaviorDefinition> array = ClonePacketEventArray(GetPacketEventArray(_packetEventTargetProperty));
			array.Add(cardActionBehaviorDefinition);
			_propertyBinding.SetValue(_editingOverride, _packetEventTargetProperty, array, "添加卡牌覆盖事件", this, "RefreshPacketOverrideFromHistory");
			RefreshDirectEditPreview();
			OpenPacketEventResource(_packetEventTargetProperty, array.Count - 1);
		}
	}

	private void RemoveSelectedPacketEvent(string propertyName)
	{
		Array<CardActionBehaviorDefinition> packetEventArray = GetPacketEventArray(propertyName);
		int[] selectedItems = ((propertyName == "useSucceededActions") ? _useSucceededActionsList : _pressedActionsList).GetSelectedItems();
		if (_propertyBinding != null && packetEventArray != null && selectedItems.Length != 0 && selectedItems[0] >= 0 && selectedItems[0] < packetEventArray.Count)
		{
			Array<CardActionBehaviorDefinition> array = ClonePacketEventArray(packetEventArray);
			array.RemoveAt(selectedItems[0]);
			_propertyBinding.SetValue(_editingOverride, propertyName, array, "删除卡牌覆盖事件", this, "RefreshPacketOverrideFromHistory");
			RefreshDirectEditPreview();
		}
	}

	private void MoveSelectedPacketEvent(string propertyName, int direction)
	{
		Array<CardActionBehaviorDefinition> packetEventArray = GetPacketEventArray(propertyName);
		ItemList itemList = ((propertyName == "useSucceededActions") ? _useSucceededActionsList : _pressedActionsList);
		int[] selectedItems = itemList.GetSelectedItems();
		if (_propertyBinding != null && packetEventArray != null && selectedItems.Length != 0)
		{
			int num = selectedItems[0];
			int num2 = num + Math.Sign(direction);
			if (num >= 0 && num < packetEventArray.Count && num2 >= 0 && num2 < packetEventArray.Count)
			{
				Array<CardActionBehaviorDefinition> array = ClonePacketEventArray(packetEventArray);
				CardActionBehaviorDefinition value = array[num];
				array[num] = array[num2];
				array[num2] = value;
				_propertyBinding.SetValue(_editingOverride, propertyName, array, "调整卡牌覆盖事件顺序", this, "RefreshPacketOverrideFromHistory");
				RefreshDirectEditPreview();
				itemList.Select(num2);
			}
		}
	}

	private void OpenPacketEventResource(string propertyName, int index)
	{
		Array<CardActionBehaviorDefinition> packetEventArray = GetPacketEventArray(propertyName);
		if (packetEventArray != null && index >= 0 && index < packetEventArray.Count)
		{
			CardActionBehaviorDefinition cardActionBehaviorDefinition = packetEventArray[index];
			if (GodotObject.IsInstanceValid(cardActionBehaviorDefinition))
			{
				XWResourceEditContext context = XWResourceEditContext.ForProperty(cardActionBehaviorDefinition, _editingOverride, cardActionBehaviorDefinition.ResourcePath, CurrentResourcePath, propertyName, index, "packet_override_editor", CurrentEditContext?.IsBuiltInSource ?? false);
				XWEditorInterface.Instance?.EditResource(cardActionBehaviorDefinition, context);
			}
		}
	}

	private Array<CardActionBehaviorDefinition> GetPacketEventArray(string propertyName)
	{
		if (!GodotObject.IsInstanceValid(_editingOverride))
		{
			return null;
		}
		if (!(propertyName == "useSucceededActions"))
		{
			return _editingOverride.pressedActions;
		}
		return _editingOverride.useSucceededActions;
	}

	private static Array<CardActionBehaviorDefinition> ClonePacketEventArray(Array<CardActionBehaviorDefinition> source)
	{
		Array<CardActionBehaviorDefinition> array = new Array<CardActionBehaviorDefinition>();
		if (source != null)
		{
			foreach (CardActionBehaviorDefinition item in source)
			{
				array.Add(item);
			}
		}
		return array;
	}

	[UnconditionalSuppressMessage("Trimming", "IL2067", Justification = "Editor-only packet event instances are created from discovered runtime types.")]
	private static CardActionBehaviorDefinition CreatePacketEventInstance(Type type)
	{
		try
		{
			return Activator.CreateInstance(type) as CardActionBehaviorDefinition;
		}
		catch (Exception ex)
		{
			GD.PushWarning("Packet override event create failed: " + type?.FullName + " " + ex.Message);
			return null;
		}
	}

	private void AddSummaryRows()
	{
		AddItemIfMissing(PreviewList, "真实种子栏卡牌：继承值 → 覆盖结果");
		AddItemIfMissing(TimelineList, "卡框 / 费用 / 冷却 / 僵尸参数 → 安全演算");
		AddItemIfMissing(GraphList, "卡牌覆盖规则 → 事件 / 植物覆盖 / 角色覆盖（不执行游戏事件）");
	}

	private static void AddItemIfMissing(ItemList list, string text)
	{
		if (!GodotObject.IsInstanceValid(list) || string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		for (int i = 0; i < list.ItemCount; i++)
		{
			if (list.GetItemText(i) == text)
			{
				return;
			}
		}
		list.AddItem(text);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(36)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCompletePacketOverrideVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposePacketOverrideBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packetOverride", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindDirectEditControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetOverride", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnOverrideTypeSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPacketOverridePropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshDirectEditPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPacketOverrideFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateOverrideFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddPlantCover, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemovePlantCover, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CloneStringArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshLists, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateEventList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.Array, "events", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyOverridePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateSimulationPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "applied", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CalculateEffectiveCost, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "baseCost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "packetOverride", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildOverrideBadge, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildFlagSuffix, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigurePacketCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Int, "cost", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.LoadPacketTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketTexturePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketTypeDisplayName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "packetType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectOptionById, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Int, "id", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenCharacterOverride, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCharacterOverridePicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowPacketEventSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "targetProperty", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsurePacketEventSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedPacketEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveSelectedPacketEvent, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenPacketEventResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetPacketEventArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClonePacketEventArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddItemIfMissing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool InvokeGodotClassMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName._ExitTree && args.Count == 0)
		{
			_ExitTree();
			ret = default;
			return true;
		}
		if (method == MethodName.HasCompletePacketOverrideVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompletePacketOverrideVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.DisposePacketOverrideBinding && args.Count == 0)
		{
			DisposePacketOverrideBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.BindWorkbench && args.Count == 2)
		{
			BindWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketOverride>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindDirectEditControls && args.Count == 1)
		{
			BindDirectEditControls(VariantUtils.ConvertTo<TowerDefensePacketOverride>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnOverrideTypeSelected && args.Count == 1)
		{
			OnOverrideTypeSelected(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPacketOverridePropertyEdited && args.Count == 1)
		{
			OnPacketOverridePropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDirectEditPreview && args.Count == 0)
		{
			RefreshDirectEditPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketOverrideFromHistory && args.Count == 0)
		{
			RefreshPacketOverrideFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateOverrideFields && args.Count == 0)
		{
			PopulateOverrideFields();
			ret = default;
			return true;
		}
		if (method == MethodName.AddPlantCover && args.Count == 0)
		{
			AddPlantCover();
			ret = default;
			return true;
		}
		if (method == MethodName.RemovePlantCover && args.Count == 0)
		{
			RemovePlantCover();
			ret = default;
			return true;
		}
		if (method == MethodName.CloneStringArray && args.Count == 1)
		{
			Array<string> array = CloneStringArray(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.RefreshLists && args.Count == 0)
		{
			RefreshLists();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateEventList && args.Count == 2)
		{
			PopulateEventList(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertToArray<CardActionBehaviorDefinition>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyOverridePreview && args.Count == 0)
		{
			ApplyOverridePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateSimulationPreview && args.Count == 1)
		{
			UpdateSimulationPreview(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CalculateEffectiveCost && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CalculateEffectiveCost(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketOverride>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildOverrideBadge && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildOverrideBadge());
			return true;
		}
		if (method == MethodName.BuildFlagSuffix && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildFlagSuffix());
			return true;
		}
		if (method == MethodName.ConfigurePacketCard && args.Count == 3)
		{
			ConfigurePacketCard(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadPacketTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadPacketTexture(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketTexturePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPacketTexturePath(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketTypeDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPacketTypeDisplayName(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectOptionById && args.Count == 2)
		{
			SelectOptionById(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenCharacterOverride && args.Count == 0)
		{
			OpenCharacterOverride();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCharacterOverridePicked && args.Count == 1)
		{
			OnCharacterOverridePicked(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowPacketEventSelector && args.Count == 1)
		{
			ShowPacketEventSelector(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsurePacketEventSelectorWindow && args.Count == 0)
		{
			EnsurePacketEventSelectorWindow();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedPacketEvent && args.Count == 1)
		{
			RemoveSelectedPacketEvent(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedPacketEvent && args.Count == 2)
		{
			MoveSelectedPacketEvent(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenPacketEventResource && args.Count == 2)
		{
			OpenPacketEventResource(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetPacketEventArray && args.Count == 1)
		{
			Array<CardActionBehaviorDefinition> packetEventArray = GetPacketEventArray(VariantUtils.ConvertTo<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(packetEventArray);
			return true;
		}
		if (method == MethodName.ClonePacketEventArray && args.Count == 1)
		{
			Array<CardActionBehaviorDefinition> array2 = ClonePacketEventArray(VariantUtils.ConvertToArray<CardActionBehaviorDefinition>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 0)
		{
			AddSummaryRows();
			ret = default;
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasCompletePacketOverrideVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompletePacketOverrideVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.CloneStringArray && args.Count == 1)
		{
			Array<string> array = CloneStringArray(VariantUtils.ConvertToArray<string>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.PopulateEventList && args.Count == 2)
		{
			PopulateEventList(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertToArray<CardActionBehaviorDefinition>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CalculateEffectiveCost && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(CalculateEffectiveCost(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketOverride>(in args[1])));
			return true;
		}
		if (method == MethodName.ConfigurePacketCard && args.Count == 3)
		{
			ConfigurePacketCard(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadPacketTexture && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadPacketTexture(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketTexturePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPacketTexturePath(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.GetPacketTypeDisplayName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetPacketTypeDisplayName(VariantUtils.ConvertTo<TowerDefenseEnum.PACKET_TYPE>(in args[0])));
			return true;
		}
		if (method == MethodName.SelectOptionById && args.Count == 2)
		{
			SelectOptionById(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClonePacketEventArray && args.Count == 1)
		{
			Array<CardActionBehaviorDefinition> array2 = ClonePacketEventArray(VariantUtils.ConvertToArray<CardActionBehaviorDefinition>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		ret = default;
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool HasGodotClassMethod(in godot_string_name method)
	{
		if (method == MethodName._ExitTree)
		{
			return true;
		}
		if (method == MethodName.HasCompletePacketOverrideVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.DisposePacketOverrideBinding)
		{
			return true;
		}
		if (method == MethodName.BindWorkbench)
		{
			return true;
		}
		if (method == MethodName.BindDirectEditControls)
		{
			return true;
		}
		if (method == MethodName.OnOverrideTypeSelected)
		{
			return true;
		}
		if (method == MethodName.OnPacketOverridePropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshDirectEditPreview)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketOverrideFromHistory)
		{
			return true;
		}
		if (method == MethodName.PopulateOverrideFields)
		{
			return true;
		}
		if (method == MethodName.AddPlantCover)
		{
			return true;
		}
		if (method == MethodName.RemovePlantCover)
		{
			return true;
		}
		if (method == MethodName.CloneStringArray)
		{
			return true;
		}
		if (method == MethodName.RefreshLists)
		{
			return true;
		}
		if (method == MethodName.PopulateEventList)
		{
			return true;
		}
		if (method == MethodName.ApplyOverridePreview)
		{
			return true;
		}
		if (method == MethodName.UpdateSimulationPreview)
		{
			return true;
		}
		if (method == MethodName.CalculateEffectiveCost)
		{
			return true;
		}
		if (method == MethodName.BuildOverrideBadge)
		{
			return true;
		}
		if (method == MethodName.BuildFlagSuffix)
		{
			return true;
		}
		if (method == MethodName.ConfigurePacketCard)
		{
			return true;
		}
		if (method == MethodName.LoadPacketTexture)
		{
			return true;
		}
		if (method == MethodName.GetPacketTexturePath)
		{
			return true;
		}
		if (method == MethodName.GetPacketTypeDisplayName)
		{
			return true;
		}
		if (method == MethodName.SelectOptionById)
		{
			return true;
		}
		if (method == MethodName.OpenCharacterOverride)
		{
			return true;
		}
		if (method == MethodName.OnCharacterOverridePicked)
		{
			return true;
		}
		if (method == MethodName.ShowPacketEventSelector)
		{
			return true;
		}
		if (method == MethodName.EnsurePacketEventSelectorWindow)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedPacketEvent)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedPacketEvent)
		{
			return true;
		}
		if (method == MethodName.OpenPacketEventResource)
		{
			return true;
		}
		if (method == MethodName.GetPacketEventArray)
		{
			return true;
		}
		if (method == MethodName.ClonePacketEventArray)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
		{
			return true;
		}
		if (method == MethodName.AddItemIfMissing)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editingOverride)
		{
			_editingOverride = VariantUtils.ConvertTo<TowerDefensePacketOverride>(in value);
			return true;
		}
		if (name == PropertyName._basePacket)
		{
			_basePacket = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName._resultPacket)
		{
			_resultPacket = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		if (name == PropertyName._resultCardRoot)
		{
			_resultCardRoot = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._baseSummary)
		{
			_baseSummary = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._resultSummary)
		{
			_resultSummary = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._overrideState)
		{
			_overrideState = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._overrideBadge)
		{
			_overrideBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._characterOverrideStatus)
		{
			_characterOverrideStatus = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._baseType)
		{
			_baseType = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._baseCost)
		{
			_baseCost = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._baseCooldown)
		{
			_baseCooldown = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._baseStartingCooldown)
		{
			_baseStartingCooldown = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._baseWeight)
		{
			_baseWeight = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._baseWavePointCost)
		{
			_baseWavePointCost = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._overrideType)
		{
			_overrideType = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._costRise)
		{
			_costRise = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._overrideCost)
		{
			_overrideCost = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._costMultiple)
		{
			_costMultiple = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._packetCooldown)
		{
			_packetCooldown = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._startingCooldown)
		{
			_startingCooldown = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._weight)
		{
			_weight = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._wavePointCost)
		{
			_wavePointCost = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._isLimitGridNum)
		{
			_isLimitGridNum = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._coverCanDirectPlant)
		{
			_coverCanDirectPlant = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._hypnoses)
		{
			_hypnoses = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._plantCoverList)
		{
			_plantCoverList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._plantCoverInput)
		{
			_plantCoverInput = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._pressedActionsList)
		{
			_pressedActionsList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._useSucceededActionsList)
		{
			_useSucceededActionsList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._resourceName)
		{
			_resourceName = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._localToScene)
		{
			_localToScene = VariantUtils.ConvertTo<CheckButton>(in value);
			return true;
		}
		if (name == PropertyName._characterOverridePicker)
		{
			_characterOverridePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._packetEventSelectorWindow)
		{
			_packetEventSelectorWindow = VariantUtils.ConvertTo<XWPacketEventSelectorWindow>(in value);
			return true;
		}
		if (name == PropertyName._packetEventTargetProperty)
		{
			_packetEventTargetProperty = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._previewTween)
		{
			_previewTween = VariantUtils.ConvertTo<Tween>(in value);
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
		if (name == PropertyName._editingOverride)
		{
			value = VariantUtils.CreateFrom(in _editingOverride);
			return true;
		}
		if (name == PropertyName._basePacket)
		{
			value = VariantUtils.CreateFrom(in _basePacket);
			return true;
		}
		if (name == PropertyName._resultPacket)
		{
			value = VariantUtils.CreateFrom(in _resultPacket);
			return true;
		}
		if (name == PropertyName._resultCardRoot)
		{
			value = VariantUtils.CreateFrom(in _resultCardRoot);
			return true;
		}
		if (name == PropertyName._baseSummary)
		{
			value = VariantUtils.CreateFrom(in _baseSummary);
			return true;
		}
		if (name == PropertyName._resultSummary)
		{
			value = VariantUtils.CreateFrom(in _resultSummary);
			return true;
		}
		if (name == PropertyName._overrideState)
		{
			value = VariantUtils.CreateFrom(in _overrideState);
			return true;
		}
		if (name == PropertyName._overrideBadge)
		{
			value = VariantUtils.CreateFrom(in _overrideBadge);
			return true;
		}
		if (name == PropertyName._characterOverrideStatus)
		{
			value = VariantUtils.CreateFrom(in _characterOverrideStatus);
			return true;
		}
		if (name == PropertyName._baseType)
		{
			value = VariantUtils.CreateFrom(in _baseType);
			return true;
		}
		if (name == PropertyName._baseCost)
		{
			value = VariantUtils.CreateFrom(in _baseCost);
			return true;
		}
		if (name == PropertyName._baseCooldown)
		{
			value = VariantUtils.CreateFrom(in _baseCooldown);
			return true;
		}
		if (name == PropertyName._baseStartingCooldown)
		{
			value = VariantUtils.CreateFrom(in _baseStartingCooldown);
			return true;
		}
		if (name == PropertyName._baseWeight)
		{
			value = VariantUtils.CreateFrom(in _baseWeight);
			return true;
		}
		if (name == PropertyName._baseWavePointCost)
		{
			value = VariantUtils.CreateFrom(in _baseWavePointCost);
			return true;
		}
		if (name == PropertyName._overrideType)
		{
			value = VariantUtils.CreateFrom(in _overrideType);
			return true;
		}
		if (name == PropertyName._costRise)
		{
			value = VariantUtils.CreateFrom(in _costRise);
			return true;
		}
		if (name == PropertyName._overrideCost)
		{
			value = VariantUtils.CreateFrom(in _overrideCost);
			return true;
		}
		if (name == PropertyName._costMultiple)
		{
			value = VariantUtils.CreateFrom(in _costMultiple);
			return true;
		}
		if (name == PropertyName._packetCooldown)
		{
			value = VariantUtils.CreateFrom(in _packetCooldown);
			return true;
		}
		if (name == PropertyName._startingCooldown)
		{
			value = VariantUtils.CreateFrom(in _startingCooldown);
			return true;
		}
		if (name == PropertyName._weight)
		{
			value = VariantUtils.CreateFrom(in _weight);
			return true;
		}
		if (name == PropertyName._wavePointCost)
		{
			value = VariantUtils.CreateFrom(in _wavePointCost);
			return true;
		}
		if (name == PropertyName._isLimitGridNum)
		{
			value = VariantUtils.CreateFrom(in _isLimitGridNum);
			return true;
		}
		if (name == PropertyName._coverCanDirectPlant)
		{
			value = VariantUtils.CreateFrom(in _coverCanDirectPlant);
			return true;
		}
		if (name == PropertyName._hypnoses)
		{
			value = VariantUtils.CreateFrom(in _hypnoses);
			return true;
		}
		if (name == PropertyName._plantCoverList)
		{
			value = VariantUtils.CreateFrom(in _plantCoverList);
			return true;
		}
		if (name == PropertyName._plantCoverInput)
		{
			value = VariantUtils.CreateFrom(in _plantCoverInput);
			return true;
		}
		if (name == PropertyName._pressedActionsList)
		{
			value = VariantUtils.CreateFrom(in _pressedActionsList);
			return true;
		}
		if (name == PropertyName._useSucceededActionsList)
		{
			value = VariantUtils.CreateFrom(in _useSucceededActionsList);
			return true;
		}
		if (name == PropertyName._resourceName)
		{
			value = VariantUtils.CreateFrom(in _resourceName);
			return true;
		}
		if (name == PropertyName._localToScene)
		{
			value = VariantUtils.CreateFrom(in _localToScene);
			return true;
		}
		if (name == PropertyName._characterOverridePicker)
		{
			value = VariantUtils.CreateFrom(in _characterOverridePicker);
			return true;
		}
		if (name == PropertyName._packetEventSelectorWindow)
		{
			value = VariantUtils.CreateFrom(in _packetEventSelectorWindow);
			return true;
		}
		if (name == PropertyName._packetEventTargetProperty)
		{
			value = VariantUtils.CreateFrom(in _packetEventTargetProperty);
			return true;
		}
		if (name == PropertyName._previewTween)
		{
			value = VariantUtils.CreateFrom(in _previewTween);
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
			new PropertyInfo(Variant.Type.Object, PropertyName._editingOverride, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._basePacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultPacket, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultCardRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._baseSummary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resultSummary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideState, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterOverrideStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._baseType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._baseCost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._baseCooldown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._baseStartingCooldown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._baseWeight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._baseWavePointCost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._costRise, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideCost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._costMultiple, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetCooldown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._startingCooldown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weight, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._wavePointCost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._isLimitGridNum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._coverCanDirectPlant, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._hypnoses, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._plantCoverList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._plantCoverInput, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pressedActionsList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._useSucceededActionsList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._characterOverridePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetEventSelectorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._packetEventTargetProperty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingOverride, Variant.From(in _editingOverride));
		info.AddProperty(PropertyName._basePacket, Variant.From(in _basePacket));
		info.AddProperty(PropertyName._resultPacket, Variant.From(in _resultPacket));
		info.AddProperty(PropertyName._resultCardRoot, Variant.From(in _resultCardRoot));
		info.AddProperty(PropertyName._baseSummary, Variant.From(in _baseSummary));
		info.AddProperty(PropertyName._resultSummary, Variant.From(in _resultSummary));
		info.AddProperty(PropertyName._overrideState, Variant.From(in _overrideState));
		info.AddProperty(PropertyName._overrideBadge, Variant.From(in _overrideBadge));
		info.AddProperty(PropertyName._characterOverrideStatus, Variant.From(in _characterOverrideStatus));
		info.AddProperty(PropertyName._baseType, Variant.From(in _baseType));
		info.AddProperty(PropertyName._baseCost, Variant.From(in _baseCost));
		info.AddProperty(PropertyName._baseCooldown, Variant.From(in _baseCooldown));
		info.AddProperty(PropertyName._baseStartingCooldown, Variant.From(in _baseStartingCooldown));
		info.AddProperty(PropertyName._baseWeight, Variant.From(in _baseWeight));
		info.AddProperty(PropertyName._baseWavePointCost, Variant.From(in _baseWavePointCost));
		info.AddProperty(PropertyName._overrideType, Variant.From(in _overrideType));
		info.AddProperty(PropertyName._costRise, Variant.From(in _costRise));
		info.AddProperty(PropertyName._overrideCost, Variant.From(in _overrideCost));
		info.AddProperty(PropertyName._costMultiple, Variant.From(in _costMultiple));
		info.AddProperty(PropertyName._packetCooldown, Variant.From(in _packetCooldown));
		info.AddProperty(PropertyName._startingCooldown, Variant.From(in _startingCooldown));
		info.AddProperty(PropertyName._weight, Variant.From(in _weight));
		info.AddProperty(PropertyName._wavePointCost, Variant.From(in _wavePointCost));
		info.AddProperty(PropertyName._isLimitGridNum, Variant.From(in _isLimitGridNum));
		info.AddProperty(PropertyName._coverCanDirectPlant, Variant.From(in _coverCanDirectPlant));
		info.AddProperty(PropertyName._hypnoses, Variant.From(in _hypnoses));
		info.AddProperty(PropertyName._plantCoverList, Variant.From(in _plantCoverList));
		info.AddProperty(PropertyName._plantCoverInput, Variant.From(in _plantCoverInput));
		info.AddProperty(PropertyName._pressedActionsList, Variant.From(in _pressedActionsList));
		info.AddProperty(PropertyName._useSucceededActionsList, Variant.From(in _useSucceededActionsList));
		info.AddProperty(PropertyName._resourceName, Variant.From(in _resourceName));
		info.AddProperty(PropertyName._localToScene, Variant.From(in _localToScene));
		info.AddProperty(PropertyName._characterOverridePicker, Variant.From(in _characterOverridePicker));
		info.AddProperty(PropertyName._packetEventSelectorWindow, Variant.From(in _packetEventSelectorWindow));
		info.AddProperty(PropertyName._packetEventTargetProperty, Variant.From(in _packetEventTargetProperty));
		info.AddProperty(PropertyName._previewTween, Variant.From(in _previewTween));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingOverride, out var value))
		{
			_editingOverride = value.As<TowerDefensePacketOverride>();
		}
		if (info.TryGetProperty(PropertyName._basePacket, out var value2))
		{
			_basePacket = value2.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._resultPacket, out var value3))
		{
			_resultPacket = value3.As<TowerDefenseInGamePacketShow>();
		}
		if (info.TryGetProperty(PropertyName._resultCardRoot, out var value4))
		{
			_resultCardRoot = value4.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._baseSummary, out var value5))
		{
			_baseSummary = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resultSummary, out var value6))
		{
			_resultSummary = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._overrideState, out var value7))
		{
			_overrideState = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._overrideBadge, out var value8))
		{
			_overrideBadge = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._characterOverrideStatus, out var value9))
		{
			_characterOverrideStatus = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._baseType, out var value10))
		{
			_baseType = value10.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._baseCost, out var value11))
		{
			_baseCost = value11.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._baseCooldown, out var value12))
		{
			_baseCooldown = value12.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._baseStartingCooldown, out var value13))
		{
			_baseStartingCooldown = value13.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._baseWeight, out var value14))
		{
			_baseWeight = value14.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._baseWavePointCost, out var value15))
		{
			_baseWavePointCost = value15.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._overrideType, out var value16))
		{
			_overrideType = value16.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._costRise, out var value17))
		{
			_costRise = value17.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._overrideCost, out var value18))
		{
			_overrideCost = value18.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._costMultiple, out var value19))
		{
			_costMultiple = value19.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._packetCooldown, out var value20))
		{
			_packetCooldown = value20.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._startingCooldown, out var value21))
		{
			_startingCooldown = value21.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._weight, out var value22))
		{
			_weight = value22.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._wavePointCost, out var value23))
		{
			_wavePointCost = value23.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._isLimitGridNum, out var value24))
		{
			_isLimitGridNum = value24.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._coverCanDirectPlant, out var value25))
		{
			_coverCanDirectPlant = value25.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._hypnoses, out var value26))
		{
			_hypnoses = value26.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._plantCoverList, out var value27))
		{
			_plantCoverList = value27.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._plantCoverInput, out var value28))
		{
			_plantCoverInput = value28.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._pressedActionsList, out var value29))
		{
			_pressedActionsList = value29.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._useSucceededActionsList, out var value30))
		{
			_useSucceededActionsList = value30.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._resourceName, out var value31))
		{
			_resourceName = value31.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToScene, out var value32))
		{
			_localToScene = value32.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._characterOverridePicker, out var value33))
		{
			_characterOverridePicker = value33.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._packetEventSelectorWindow, out var value34))
		{
			_packetEventSelectorWindow = value34.As<XWPacketEventSelectorWindow>();
		}
		if (info.TryGetProperty(PropertyName._packetEventTargetProperty, out var value35))
		{
			_packetEventTargetProperty = value35.As<string>();
		}
		if (info.TryGetProperty(PropertyName._previewTween, out var value36))
		{
			_previewTween = value36.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value37))
		{
			_updatingControls = value37.As<bool>();
		}
	}
}
