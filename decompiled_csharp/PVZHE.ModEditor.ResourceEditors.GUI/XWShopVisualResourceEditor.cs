using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.Inspector;
using PVZHE.ModEditor.ModSystem;
using PVZHE.ModEditor.ResourceEditors.GUI.GameplayLogic;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWShopVisualResourceEditor.cs")]
public class XWShopVisualResourceEditor : XWGenericVisualResourceEditor
{
	private enum ShopEditScope
	{
		Shop,
		Page,
		Item,
		Stage
	}

	private sealed class ShopOptionItem
	{
		public string Name = "";

		public string SourceLabel = "";

		public string SourcePath = "";

		public string Description = "";
	}

	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName CreateShopPreviewAdapter = "CreateShopPreviewAdapter";

		public static readonly StringName HasCompleteShopVisualCoverage = "HasCompleteShopVisualCoverage";

		public static readonly StringName RenderShopEditor = "RenderShopEditor";

		public static readonly StringName BindShopLayout = "BindShopLayout";

		public static readonly StringName ConfigureScopeControls = "ConfigureScopeControls";

		public static readonly StringName SetScopeButton = "SetScopeButton";

		public static readonly StringName BindStageFields = "BindStageFields";

		public static readonly StringName CreatePurchaseSimulationPanel = "CreatePurchaseSimulationPanel";

		public static readonly StringName RefreshPurchaseSimulation = "RefreshPurchaseSimulation";

		public static readonly StringName SelectShopPage = "SelectShopPage";

		public static readonly StringName SelectShopItem = "SelectShopItem";

		public static readonly StringName SelectShopStage = "SelectShopStage";

		public static readonly StringName AddShopPage = "AddShopPage";

		public static readonly StringName RemoveSelectedShopPage = "RemoveSelectedShopPage";

		public static readonly StringName MoveSelectedShopPage = "MoveSelectedShopPage";

		public static readonly StringName AddShopItem = "AddShopItem";

		public static readonly StringName RemoveSelectedShopItem = "RemoveSelectedShopItem";

		public static readonly StringName MoveSelectedShopItem = "MoveSelectedShopItem";

		public static readonly StringName AddShopStage = "AddShopStage";

		public static readonly StringName RemoveSelectedShopStage = "RemoveSelectedShopStage";

		public static readonly StringName MoveSelectedShopStage = "MoveSelectedShopStage";

		public static readonly StringName SetItemType = "SetItemType";

		public static readonly StringName BeginItemTypeEdit = "BeginItemTypeEdit";

		public static readonly StringName PreviewItemType = "PreviewItemType";

		public static readonly StringName SetStageField = "SetStageField";

		public static readonly StringName BeginStageFieldEdit = "BeginStageFieldEdit";

		public static readonly StringName PreviewStageField = "PreviewStageField";

		public static readonly StringName SetPacketStageFields = "SetPacketStageFields";

		public static readonly StringName EnsureShopOptionSelectorWindow = "EnsureShopOptionSelectorWindow";

		public static readonly StringName RefreshResourceMetadata = "RefreshResourceMetadata";

		public static readonly StringName OpenJsonSourceDialog = "OpenJsonSourceDialog";

		public static readonly StringName ApplyJsonSource = "ApplyJsonSource";

		public static readonly StringName ConfirmClearJsonSource = "ConfirmClearJsonSource";

		public static readonly StringName ClearJsonSource = "ClearJsonSource";

		public static readonly StringName SetShopDataWithPageSnapshot = "SetShopDataWithPageSnapshot";

		public static readonly StringName RefreshDataStatus = "RefreshDataStatus";

		public static readonly StringName RefreshAll = "RefreshAll";

		public static readonly StringName RefreshPageList = "RefreshPageList";

		public static readonly StringName RefreshItemList = "RefreshItemList";

		public static readonly StringName RefreshStageList = "RefreshStageList";

		public static readonly StringName RefreshStageControls = "RefreshStageControls";

		public static readonly StringName DisposeStageVisualChoices = "DisposeStageVisualChoices";

		public static readonly StringName NormalizeSelection = "NormalizeSelection";

		public static readonly StringName NormalizeIndex = "NormalizeIndex";

		public static readonly StringName GetSelectedPage = "GetSelectedPage";

		public static readonly StringName GetSelectedItem = "GetSelectedItem";

		public static readonly StringName GetSelectedStage = "GetSelectedStage";

		public static readonly StringName CopyPages = "CopyPages";

		public static readonly StringName CopyItems = "CopyItems";

		public static readonly StringName CopyStages = "CopyStages";

		public static readonly StringName OnShopPropertyEdited = "OnShopPropertyEdited";

		public static readonly StringName RefreshLiveEditPreview = "RefreshLiveEditPreview";

		public static readonly StringName CaptureSelectionIdentity = "CaptureSelectionIdentity";

		public static readonly StringName RestoreSelectionIdentity = "RestoreSelectionIdentity";

		public static readonly StringName RefreshShopEditorFromHistory = "RefreshShopEditorFromHistory";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName BuildSummary = "BuildSummary";

		public static readonly StringName ReadSaveKeyFromResourceFile = "ReadSaveKeyFromResourceFile";

		public static readonly StringName CleanResourceTextValue = "CleanResourceTextValue";

		public static readonly StringName RegisterProjectSelectionPath = "RegisterProjectSelectionPath";

		public static readonly StringName GetCurrentProjectPathForShopEditor = "GetCurrentProjectPathForShopEditor";

		public static readonly StringName ToProjectRelativeDisplayPath = "ToProjectRelativeDisplayPath";

		public static readonly StringName SelectOptionByText = "SelectOptionByText";

		public static readonly StringName SetSpinValue = "SetSpinValue";

		public static readonly StringName DisplayIndex = "DisplayIndex";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";

		public static readonly StringName FormatResource = "FormatResource";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingShop = "_editingShop";

		public static readonly StringName _editScope = "_editScope";

		public static readonly StringName _runtimeShopPreview = "_runtimeShopPreview";

		public static readonly StringName _summaryLabel = "_summaryLabel";

		public static readonly StringName _pageSummaryLabel = "_pageSummaryLabel";

		public static readonly StringName _itemSummaryLabel = "_itemSummaryLabel";

		public static readonly StringName _stageSummaryLabel = "_stageSummaryLabel";

		public static readonly StringName _pageList = "_pageList";

		public static readonly StringName _itemList = "_itemList";

		public static readonly StringName _stageList = "_stageList";

		public static readonly StringName _stageTexturePreview = "_stageTexturePreview";

		public static readonly StringName _stagePreviewTitle = "_stagePreviewTitle";

		public static readonly StringName _stagePreviewMeta = "_stagePreviewMeta";

		public static readonly StringName _stagePreviewStock = "_stagePreviewStock";

		public static readonly StringName _purchaseBalanceSpin = "_purchaseBalanceSpin";

		public static readonly StringName _purchaseOwnedSpin = "_purchaseOwnedSpin";

		public static readonly StringName _purchaseQuantitySpin = "_purchaseQuantitySpin";

		public static readonly StringName _purchaseStockProgress = "_purchaseStockProgress";

		public static readonly StringName _purchasePreviewResult = "_purchasePreviewResult";

		public static readonly StringName _itemTypeEdit = "_itemTypeEdit";

		public static readonly StringName _resourceNameEdit = "_resourceNameEdit";

		public static readonly StringName _localToSceneCheck = "_localToSceneCheck";

		public static readonly StringName _jsonSourceButton = "_jsonSourceButton";

		public static readonly StringName _jsonSourceDialog = "_jsonSourceDialog";

		public static readonly StringName _dataStatus = "_dataStatus";

		public static readonly StringName _clearJsonButton = "_clearJsonButton";

		public static readonly StringName _clearJsonConfirmationDialog = "_clearJsonConfirmationDialog";

		public static readonly StringName _stageSaveTypeOption = "_stageSaveTypeOption";

		public static readonly StringName _stageSaveKeyEdit = "_stageSaveKeyEdit";

		public static readonly StringName _stageTexturePicker = "_stageTexturePicker";

		public static readonly StringName _stageDescribeEdit = "_stageDescribeEdit";

		public static readonly StringName _stageNpcTalkEdit = "_stageNpcTalkEdit";

		public static readonly StringName _stageCostTypeOption = "_stageCostTypeOption";

		public static readonly StringName _stageCostSpin = "_stageCostSpin";

		public static readonly StringName _stageOpenMinSpin = "_stageOpenMinSpin";

		public static readonly StringName _stageOpenMaxSpin = "_stageOpenMaxSpin";

		public static readonly StringName _stageAddNumSpin = "_stageAddNumSpin";

		public static readonly StringName _selectedPageIndex = "_selectedPageIndex";

		public static readonly StringName _selectedItemIndex = "_selectedItemIndex";

		public static readonly StringName _selectedStageIndex = "_selectedStageIndex";

		public static readonly StringName _selectedPageIdentity = "_selectedPageIdentity";

		public static readonly StringName _selectedItemIdentity = "_selectedItemIdentity";

		public static readonly StringName _selectedStageIdentity = "_selectedStageIdentity";

		public static readonly StringName _activeItemTypeEditTarget = "_activeItemTypeEditTarget";

		public static readonly StringName _activeStageEditTarget = "_activeStageEditTarget";

		public static readonly StringName _activeStageEditProperty = "_activeStageEditProperty";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _shopOptionSelectorWindow = "_shopOptionSelectorWindow";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string EditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWShopVisualEditorLayout.tscn";

	private const string PurchaseSimulationScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWShopPurchaseSimulation.tscn";

	private static PackedScene _editorLayoutScene;

	private static PackedScene _purchaseSimulationScene;

	private ShopConfig _editingShop;

	private XWVisualPropertyBinding _shopPropertyBinding;

	private ShopEditScope _editScope;

	private Shop _runtimeShopPreview;

	private Label _summaryLabel;

	private Label _pageSummaryLabel;

	private Label _itemSummaryLabel;

	private Label _stageSummaryLabel;

	private ItemList _pageList;

	private ItemList _itemList;

	private ItemList _stageList;

	private TextureRect _stageTexturePreview;

	private Label _stagePreviewTitle;

	private Label _stagePreviewMeta;

	private Label _stagePreviewStock;

	private SpinBox _purchaseBalanceSpin;

	private SpinBox _purchaseOwnedSpin;

	private SpinBox _purchaseQuantitySpin;

	private ProgressBar _purchaseStockProgress;

	private Label _purchasePreviewResult;

	private LineEdit _itemTypeEdit;

	private LineEdit _resourceNameEdit;

	private CheckButton _localToSceneCheck;

	private Button _jsonSourceButton;

	private FileDialog _jsonSourceDialog;

	private Label _dataStatus;

	private Button _clearJsonButton;

	private ConfirmationDialog _clearJsonConfirmationDialog;

	private OptionButton _stageSaveTypeOption;

	private XWVisualSegmentedOption _stageSaveTypeVisualChoices;

	private LineEdit _stageSaveKeyEdit;

	private XWResourcePicker _stageTexturePicker;

	private LineEdit _stageDescribeEdit;

	private LineEdit _stageNpcTalkEdit;

	private OptionButton _stageCostTypeOption;

	private XWVisualSegmentedOption _stageCostTypeVisualChoices;

	private SpinBox _stageCostSpin;

	private SpinBox _stageOpenMinSpin;

	private SpinBox _stageOpenMaxSpin;

	private SpinBox _stageAddNumSpin;

	private int _selectedPageIndex = -1;

	private int _selectedItemIndex = -1;

	private int _selectedStageIndex = -1;

	private ShopPageConfig _selectedPageIdentity;

	private ShopItemConfig _selectedItemIdentity;

	private ShopItemStageConfig _selectedStageIdentity;

	private ShopItemConfig _activeItemTypeEditTarget;

	private ShopItemStageConfig _activeStageEditTarget;

	private string _activeStageEditProperty = "";

	private bool _updatingControls;

	private XWGameplayResourcePickerWindow _shopOptionSelectorWindow;

	private readonly List<ShopOptionItem> _shopOptionSelectorOptions = new List<ShopOptionItem>();

	private Action<ShopOptionItem> _shopOptionSelectorConfirmed;

	private const string CharacterResourceRegistryPath = "res://Asset/Config/Character/CharacterResource.json";

	private const string TalkResourceRegistryPath = "res://Asset/Config/Npc/TalkResource.json";

	public override void _ExitTree()
	{
		DisposeStageVisualChoices();
		_shopPropertyBinding?.Dispose();
		_shopPropertyBinding = null;
		if (GodotObject.IsInstanceValid(_shopOptionSelectorWindow))
		{
			_shopOptionSelectorWindow.QueueFree();
		}
		_shopOptionSelectorWindow = null;
		if (GodotObject.IsInstanceValid(_clearJsonConfirmationDialog))
		{
			_clearJsonConfirmationDialog.QueueFree();
		}
		_clearJsonConfirmationDialog = null;
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeStageVisualChoices();
		_shopPropertyBinding?.Dispose();
		_shopPropertyBinding = null;
		_editingShop = null;
		_runtimeShopPreview = null;
		_selectedPageIdentity = null;
		_selectedItemIdentity = null;
		_selectedStageIdentity = null;
		_activeItemTypeEditTarget = null;
		_activeStageEditTarget = null;
		_activeStageEditProperty = "";
		_shopPropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnShopPropertyEdited);
		ShopConfig shopConfig = CreateShopPreviewAdapter(CurrentResource);
		if (!GodotObject.IsInstanceValid(shopConfig))
		{
			_shopPropertyBinding.Dispose();
			_shopPropertyBinding = null;
		}
		else
		{
			_editingShop = shopConfig;
			RenderShopEditor(shopConfig);
		}
	}

	private ShopConfig CreateShopPreviewAdapter(Resource resource)
	{
		if (!(resource is ShopConfig result))
		{
			if (!(resource is ShopPageConfig item))
			{
				if (!(resource is ShopItemConfig item2))
				{
					if (resource is ShopItemStageConfig item3)
					{
						_editScope = ShopEditScope.Stage;
						ShopItemConfig shopItemConfig = new ShopItemConfig
						{
							type = "Item"
						};
						shopItemConfig.stageList.Add(item3);
						ShopPageConfig shopPageConfig = new ShopPageConfig();
						shopPageConfig.itemList.Add(shopItemConfig);
						return new ShopConfig
						{
							pageList = { shopPageConfig }
						};
					}
					return null;
				}
				_editScope = ShopEditScope.Item;
				ShopPageConfig shopPageConfig2 = new ShopPageConfig();
				shopPageConfig2.itemList.Add(item2);
				return new ShopConfig
				{
					pageList = { shopPageConfig2 }
				};
			}
			_editScope = ShopEditScope.Page;
			return new ShopConfig
			{
				pageList = { item }
			};
		}
		_editScope = ShopEditScope.Shop;
		return result;
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!HasCompleteShopVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool HasCompleteShopVisualCoverage(Resource resource)
	{
		Type type = resource?.GetType();
		if (!(type == typeof(ShopConfig)) && !(type == typeof(ShopPageConfig)) && !(type == typeof(ShopItemConfig)))
		{
			return type == typeof(ShopItemStageConfig);
		}
		return true;
	}

	private void RenderShopEditor(ShopConfig shop)
	{
		if (CanvasGrid != null)
		{
			CanvasGrid.Columns = 1;
			if (_editorLayoutScene == null)
			{
				_editorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWShopVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _editorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindShopLayout(vBoxContainer, shop);
				NormalizeSelection();
				RefreshAll();
				AddSummaryRows(shop);
			}
		}
	}

	private void BindShopLayout(VBoxContainer root, ShopConfig shop)
	{
		_resourceNameEdit = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToSceneCheck = root.GetNode<CheckButton>("%LocalToSceneCheck");
		_shopPropertyBinding.BindText(_resourceNameEdit, CurrentResource, "resource_name", RefreshResourceMetadata, this, "RefreshShopEditorFromHistory");
		_shopPropertyBinding.BindToggle(_localToSceneCheck, CurrentResource, "resource_local_to_scene", RefreshResourceMetadata, this, "RefreshShopEditorFromHistory");
		_jsonSourceButton = root.GetNode<Button>("%JsonSourceButton");
		_jsonSourceDialog = root.GetNode<FileDialog>("%JsonSourceDialog");
		_dataStatus = root.GetNode<Label>("%DataStatus");
		_clearJsonButton = root.GetNode<Button>("%ClearJsonButton");
		_jsonSourceButton.Pressed += OpenJsonSourceDialog;
		_jsonSourceDialog.FileSelected += ApplyJsonSource;
		_clearJsonButton.Pressed += ConfirmClearJsonSource;
		_clearJsonConfirmationDialog = new ConfirmationDialog
		{
			Title = "清空商店页面",
			DialogText = "确认清除 JSON 来源并清空全部商店页面吗？此操作可以撤销。",
			OkButtonText = "清空页面"
		};
		_clearJsonConfirmationDialog.Confirmed += ClearJsonSource;
		root.AddChild(_clearJsonConfirmationDialog, forceReadableName: false, InternalMode.Disabled);
		_runtimeShopPreview = root.GetNode<Shop>("%RuntimeShop");
		_runtimeShopPreview.EditPreviewConfig(shop);
		_summaryLabel = root.GetNode<Label>("%ShopSummaryLabel");
		_summaryLabel.Text = BuildSummary(shop);
		root.GetNode<Button>("%AddPageTopButton").Pressed += AddShopPage;
		root.GetNode<Button>("%SaveButton").Pressed += SaveCurrentResource;
		_pageSummaryLabel = root.GetNode<Label>("%PageSummaryLabel");
		root.GetNode<Button>("%PageAddButton").Pressed += AddShopPage;
		root.GetNode<Button>("%PageRemoveButton").Pressed += RemoveSelectedShopPage;
		root.GetNode<Button>("%PageUpButton").Pressed += () =>
		{
			MoveSelectedShopPage(-1);
		};
		root.GetNode<Button>("%PageDownButton").Pressed += () =>
		{
			MoveSelectedShopPage(1);
		};
		_pageList = root.GetNode<ItemList>("%ShopPageList");
		_pageList.ItemSelected += (long index) =>
		{
			SelectShopPage((int)index);
		};
		_itemSummaryLabel = root.GetNode<Label>("%ItemSummaryLabel");
		_itemTypeEdit = root.GetNode<LineEdit>("%ShopItemTypeLineEdit");
		_itemTypeEdit.FocusEntered += BeginItemTypeEdit;
		_itemTypeEdit.TextChanged += PreviewItemType;
		_itemTypeEdit.TextSubmitted += (string text) =>
		{
			if (!_updatingControls)
			{
				SetItemType(text);
			}
		};
		_itemTypeEdit.FocusExited += () =>
		{
			if (!_updatingControls)
			{
				SetItemType(_itemTypeEdit.Text);
			}
		};
		root.GetNode<Button>("%ItemTypeButton").Pressed += () =>
		{
			SetItemType("Item");
		};
		root.GetNode<Button>("%PacketTypeButton").Pressed += () =>
		{
			SetItemType("Packet");
		};
		root.GetNode<Button>("%FeatureTypeButton").Pressed += () =>
		{
			SetItemType("Feature");
		};
		root.GetNode<Button>("%ItemAddButton").Pressed += AddShopItem;
		root.GetNode<Button>("%ItemRemoveButton").Pressed += RemoveSelectedShopItem;
		root.GetNode<Button>("%ItemUpButton").Pressed += () =>
		{
			MoveSelectedShopItem(-1);
		};
		root.GetNode<Button>("%ItemDownButton").Pressed += () =>
		{
			MoveSelectedShopItem(1);
		};
		_itemList = root.GetNode<ItemList>("%ShopItemList");
		_itemList.ItemSelected += (long index) =>
		{
			SelectShopItem((int)index);
		};
		_stageSummaryLabel = root.GetNode<Label>("%StageSummaryLabel");
		root.GetNode<Button>("%StageAddButton").Pressed += AddShopStage;
		root.GetNode<Button>("%StageRemoveButton").Pressed += RemoveSelectedShopStage;
		root.GetNode<Button>("%StageUpButton").Pressed += () =>
		{
			MoveSelectedShopStage(-1);
		};
		root.GetNode<Button>("%StageDownButton").Pressed += () =>
		{
			MoveSelectedShopStage(1);
		};
		_stageList = root.GetNode<ItemList>("%ShopStageList");
		_stageList.ItemSelected += (long index) =>
		{
			SelectShopStage((int)index);
		};
		BindStageFields(root);
		_stageTexturePreview = root.GetNode<TextureRect>("%StageTexturePreview");
		_stagePreviewTitle = root.GetNode<Label>("%StagePreviewTitle");
		_stagePreviewMeta = root.GetNode<Label>("%StagePreviewMeta");
		_stagePreviewStock = root.GetNode<Label>("%StagePreviewStock");
		root.GetNode<VBoxContainer>("%PurchaseSimulationHost").AddChild(CreatePurchaseSimulationPanel(), forceReadableName: false, InternalMode.Disabled);
		ConfigureScopeControls(root);
	}

	private void ConfigureScopeControls(VBoxContainer root)
	{
		bool enabled = _editScope == ShopEditScope.Shop;
		bool flag = _editScope == ShopEditScope.Shop;
		ShopEditScope editScope = _editScope;
		bool flag2 = (uint)editScope <= 1u;
		bool enabled2 = flag2;
		bool flag3 = _editScope != ShopEditScope.Stage;
		bool enabled3 = _editScope != ShopEditScope.Stage;
		SetScopeButton(root, "%AddPageTopButton", enabled, "该资源本身不是完整商店，不能增删预览包装页");
		string[] array = new string[4] { "%PageAddButton", "%PageRemoveButton", "%PageUpButton", "%PageDownButton" };
		foreach (string nodePath in array)
		{
			SetScopeButton(root, nodePath, enabled, "当前只编辑这一页，不能改动预览包装层");
		}
		array = new string[4] { "%ItemAddButton", "%ItemRemoveButton", "%ItemUpButton", "%ItemDownButton" };
		foreach (string nodePath2 in array)
		{
			SetScopeButton(root, nodePath2, enabled2, "当前资源边界不允许增删或移动商品");
		}
		array = new string[4] { "%StageAddButton", "%StageRemoveButton", "%StageUpButton", "%StageDownButton" };
		foreach (string nodePath3 in array)
		{
			SetScopeButton(root, nodePath3, enabled3, "当前只编辑这一商品阶段，不能改动预览包装层");
		}
		array = new string[3] { "%ItemTypeButton", "%PacketTypeButton", "%FeatureTypeButton" };
		foreach (string nodePath4 in array)
		{
			SetScopeButton(root, nodePath4, flag3, "商品类型属于上级商品资源");
		}
		if (GodotObject.IsInstanceValid(_itemTypeEdit))
		{
			_itemTypeEdit.Editable = flag3;
			_itemTypeEdit.TooltipText = (flag3 ? "编辑商品类型" : "商品类型属于上级商品资源");
		}
		if (GodotObject.IsInstanceValid(_jsonSourceButton))
		{
			_jsonSourceButton.Disabled = !flag;
			_jsonSourceButton.TooltipText = (flag ? "选择 ShopConfig JSON 来源" : "仅完整 ShopConfig 可设置 JSON 来源；当前是嵌套资源编辑范围。");
		}
		if (GodotObject.IsInstanceValid(_clearJsonButton))
		{
			_clearJsonButton.Disabled = !flag;
			_clearJsonButton.TooltipText = (flag ? "清除 JSON 来源并清空页面" : "仅完整 ShopConfig 可清除 JSON 来源；当前是嵌套资源编辑范围。");
		}
		RefreshDataStatus();
	}

	private static void SetScopeButton(Node root, string nodePath, bool enabled, string disabledReason)
	{
		Button nodeOrNull = root.GetNodeOrNull<Button>(nodePath);
		if (GodotObject.IsInstanceValid(nodeOrNull))
		{
			nodeOrNull.Disabled = !enabled;
			nodeOrNull.TooltipText = (enabled ? "" : disabledReason);
		}
	}

	private void BindStageFields(VBoxContainer root)
	{
		_stageSaveTypeOption = root.GetNode<OptionButton>("%StageSaveTypeOption");
		string[] array = new string[2] { "Feature", "TowerDefensePacket" };
		foreach (string label in array)
		{
			_stageSaveTypeOption.AddItem(label);
		}
		_stageSaveTypeVisualChoices = new XWVisualSegmentedOption(_stageSaveTypeOption, root.GetNode<HFlowContainer>("%StageSaveTypeVisualChoices"));
		_stageSaveTypeVisualChoices.Rebuild();
		_stageSaveTypeOption.ItemSelected += (long index) =>
		{
			if (!_updatingControls)
			{
				SetStageField("saveType", Variant.From<string>(_stageSaveTypeOption.GetItemText((int)index)));
			}
		};
		_stageSaveKeyEdit = root.GetNode<LineEdit>("%StageSaveKeyLineEdit");
		BindContinuousStageText(_stageSaveKeyEdit, "saveKey", (Variant value) =>
		{
			SetStageField("saveKey", value);
		});
		root.GetNode<Button>("%ChoosePacketButton").Pressed += () =>
		{
			ShowShopOptionSelector("选择卡片", BuildPacketKeySelectionOptions(), (ShopOptionItem item) =>
			{
				SetPacketStageFields("TowerDefensePacket", item.Name);
			});
		};
		_stageTexturePicker = XWResourcePicker.Create();
		_stageTexturePicker.Name = "StageTexturePicker";
		_stageTexturePicker.Setup("Texture2D");
		_stageTexturePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		_stageTexturePicker.ResourceChanged += (Resource resource) =>
		{
			if (!_updatingControls)
			{
				SetStageField("texture", Variant.From(in resource));
			}
		};
		root.GetNode<HBoxContainer>("%TexturePickerHost").AddChild(_stageTexturePicker, forceReadableName: false, InternalMode.Disabled);
		_stageDescribeEdit = root.GetNode<LineEdit>("%StageDescribeLineEdit");
		BindContinuousStageText(_stageDescribeEdit, "describe", (Variant value) =>
		{
			SetStageField("describe", value);
		});
		_stageNpcTalkEdit = root.GetNode<LineEdit>("%StageNpcTalkLineEdit");
		BindContinuousStageText(_stageNpcTalkEdit, "npcTalk", (Variant value) =>
		{
			SetStageField("npcTalk", value);
		});
		root.GetNode<Button>("%ChooseTalkButton").Pressed += () =>
		{
			ShowShopOptionSelector("选择对话", BuildNpcTalkSelectionOptions(), (ShopOptionItem item) =>
			{
				SetStageField("npcTalk", Variant.From(in item.Name));
			});
		};
		_stageCostTypeOption = root.GetNode<OptionButton>("%StageCostTypeOption");
		_stageCostTypeOption.AddItem("Coin");
		_stageCostTypeVisualChoices = new XWVisualSegmentedOption(_stageCostTypeOption, root.GetNode<HFlowContainer>("%StageCostTypeVisualChoices"));
		_stageCostTypeVisualChoices.Rebuild();
		_stageCostTypeOption.ItemSelected += (long index) =>
		{
			if (!_updatingControls)
			{
				SetStageField("type", Variant.From<string>(_stageCostTypeOption.GetItemText((int)index)));
			}
		};
		_stageCostSpin = root.GetNode<SpinBox>("%StageCostSpin");
		_stageOpenMinSpin = root.GetNode<SpinBox>("%StageOpenMinSpin");
		_stageOpenMaxSpin = root.GetNode<SpinBox>("%StageOpenMaxSpin");
		_stageAddNumSpin = root.GetNode<SpinBox>("%StageAddNumSpin");
		BindContinuousStageNumber(_stageCostSpin, "cost", (Variant value) =>
		{
			SetStageField("cost", value);
		});
		BindContinuousStageNumber(_stageOpenMinSpin, "openMinNum", (Variant value) =>
		{
			SetStageField("openMinNum", value);
		});
		BindContinuousStageNumber(_stageOpenMaxSpin, "openMaxNum", (Variant value) =>
		{
			SetStageField("openMaxNum", value);
		});
		BindContinuousStageNumber(_stageAddNumSpin, "addNum", (Variant value) =>
		{
			SetStageField("addNum", value);
		});
	}

	private void BindContinuousStageText(LineEdit control, string propertyName, Action<Variant> commit)
	{
		control.FocusEntered += () =>
		{
			BeginStageFieldEdit(propertyName);
		};
		control.TextChanged += (string text) =>
		{
			if (!_updatingControls)
			{
				PreviewStageField(propertyName, Variant.From(in text));
			}
		};
		control.TextSubmitted += (string text) =>
		{
			if (!_updatingControls)
			{
				commit(Variant.From(in text));
			}
		};
		control.FocusExited += () =>
		{
			if (!_updatingControls)
			{
				commit(Variant.From<string>(control.Text));
			}
		};
	}

	private void BindContinuousStageNumber(SpinBox control, string propertyName, Action<Variant> commit)
	{
		control.FocusEntered += () =>
		{
			BeginStageFieldEdit(propertyName);
		};
		control.ValueChanged += (double value) =>
		{
			if (!_updatingControls)
			{
				PreviewStageField(propertyName, Variant.From<int>((int)Math.Round(value)));
			}
		};
		control.FocusExited += () =>
		{
			if (!_updatingControls)
			{
				commit(Variant.From<int>((int)Math.Round(control.Value)));
			}
		};
	}

	private Control CreatePurchaseSimulationPanel()
	{
		if (_purchaseSimulationScene == null)
		{
			_purchaseSimulationScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWShopPurchaseSimulation.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		if (!GodotObject.IsInstanceValid(_purchaseSimulationScene))
		{
			return new Label
			{
				Text = "商店购买模拟场景加载失败"
			};
		}
		Control control = _purchaseSimulationScene.Instantiate<Control>(PackedScene.GenEditState.Disabled);
		_purchaseBalanceSpin = control.GetNode<SpinBox>("Layout/Inputs/Balance");
		_purchaseOwnedSpin = control.GetNode<SpinBox>("Layout/Inputs/Owned");
		_purchaseQuantitySpin = control.GetNode<SpinBox>("Layout/Inputs/Quantity");
		_purchaseStockProgress = control.GetNode<ProgressBar>("Layout/StockProgress");
		_purchasePreviewResult = control.GetNode<Label>("Layout/Result");
		_purchaseBalanceSpin.ValueChanged += (double _) =>
		{
			RefreshPurchaseSimulation();
		};
		_purchaseOwnedSpin.ValueChanged += (double _) =>
		{
			RefreshPurchaseSimulation();
		};
		_purchaseQuantitySpin.ValueChanged += (double _) =>
		{
			RefreshPurchaseSimulation();
		};
		RefreshPurchaseSimulation();
		return control;
	}

	private void RefreshPurchaseSimulation()
	{
		if (!GodotObject.IsInstanceValid(_purchasePreviewResult))
		{
			return;
		}
		ShopItemStageConfig selectedStage = GetSelectedStage();
		if (!GodotObject.IsInstanceValid(selectedStage))
		{
			_purchasePreviewResult.Text = "选择商品阶段后可模拟购买。";
			_purchasePreviewResult.Modulate = new Color(0.72f, 0.75f, 0.8f);
			return;
		}
		long num = (long)(_purchaseBalanceSpin?.Value ?? 0.0);
		long num2 = (long)(_purchaseOwnedSpin?.Value ?? 0.0);
		long num3 = Math.Max(1L, (long)(_purchaseQuantitySpin?.Value ?? 1.0));
		long num4 = Math.Max(0L, selectedStage.cost) * num3;
		long num5 = num2 + selectedStage.addNum * num3;
		bool flag = num2 >= selectedStage.openMinNum;
		bool flag2 = selectedStage.openMaxNum < 0 || num5 <= selectedStage.openMaxNum;
		bool flag3 = num >= num4;
		bool flag4 = flag & flag2 & flag3;
		if (GodotObject.IsInstanceValid(_purchaseStockProgress))
		{
			_purchaseStockProgress.MinValue = 0.0;
			_purchaseStockProgress.MaxValue = Math.Max(1, selectedStage.openMaxNum);
			_purchaseStockProgress.Value = Math.Clamp(num5, 0L, Math.Max(1, selectedStage.openMaxNum));
		}
		string text;
		if (flag)
		{
			if (flag2)
			{
				text = ((!flag3) ? $"余额不足，还差 {num4 - num} {selectedStage.type}" : $"将花费 {num4} {selectedStage.type}，数量 {num2} → {num5}，余额 {num - num4}");
			}
			else
			{
				text = $"购买后数量 {num5} 超过上限 {selectedStage.openMaxNum}";
			}
		}
		else
		{
			text = $"尚未达到开放数量 {selectedStage.openMinNum}";
		}
		_purchasePreviewResult.Text = (flag4 ? ("✓ 可以购买 · " + text) : ("✗ 无法购买 · " + text));
		_purchasePreviewResult.Modulate = (flag4 ? new Color(0.47f, 0.92f, 0.56f) : new Color(1f, 0.54f, 0.45f));
	}

	private void SelectShopPage(int index)
	{
		_selectedPageIndex = index;
		_selectedItemIndex = 0;
		_selectedStageIndex = 0;
		NormalizeSelection();
		CaptureSelectionIdentity();
		RefreshAll();
	}

	private void SelectShopItem(int index)
	{
		_selectedItemIndex = index;
		_selectedStageIndex = 0;
		NormalizeSelection();
		CaptureSelectionIdentity();
		RefreshAll();
	}

	private void SelectShopStage(int index)
	{
		_selectedStageIndex = index;
		NormalizeSelection();
		CaptureSelectionIdentity();
		RefreshAll();
	}

	private void AddShopPage()
	{
		if (_editingShop != null && _editScope == ShopEditScope.Shop && _shopPropertyBinding != null)
		{
			ShopPageConfig shopPageConfig = new ShopPageConfig();
			Array<ShopPageConfig> array = CopyPages(_editingShop.pageList);
			array.Add(shopPageConfig);
			_selectedPageIndex = array.Count - 1;
			_selectedItemIndex = -1;
			_selectedStageIndex = -1;
			_selectedPageIdentity = shopPageConfig;
			_selectedItemIdentity = null;
			_selectedStageIdentity = null;
			_shopPropertyBinding.SetValue(_editingShop, "pageList", array, "添加商店页面", this, "RefreshShopEditorFromHistory");
		}
	}

	private void RemoveSelectedShopPage()
	{
		if (_editScope == ShopEditScope.Shop && _editingShop?.pageList != null && _selectedPageIndex >= 0 && _selectedPageIndex < _editingShop.pageList.Count)
		{
			Array<ShopPageConfig> array = CopyPages(_editingShop.pageList);
			array.RemoveAt(_selectedPageIndex);
			_selectedPageIndex = Math.Min(_selectedPageIndex, array.Count - 1);
			_selectedItemIndex = 0;
			_selectedStageIndex = 0;
			_shopPropertyBinding.SetValue(_editingShop, "pageList", array, "删除商店页面", this, "RefreshShopEditorFromHistory");
		}
	}

	private void MoveSelectedShopPage(int direction)
	{
		if (_editScope == ShopEditScope.Shop && _editingShop?.pageList != null && _selectedPageIndex >= 0 && direction != 0)
		{
			int num = Math.Clamp(_selectedPageIndex + direction, 0, _editingShop.pageList.Count - 1);
			if (num != _selectedPageIndex)
			{
				Array<ShopPageConfig> array = CopyPages(_editingShop.pageList);
				ShopPageConfig item = array[_selectedPageIndex];
				array.RemoveAt(_selectedPageIndex);
				array.Insert(num, item);
				_selectedPageIndex = num;
				_shopPropertyBinding.SetValue(_editingShop, "pageList", array, "调整商店页面顺序", this, "RefreshShopEditorFromHistory");
			}
		}
	}

	private void AddShopItem()
	{
		ShopEditScope editScope = _editScope;
		if ((uint)(editScope - 2) > 1u)
		{
			ShopPageConfig selectedPage = GetSelectedPage();
			if (!GodotObject.IsInstanceValid(selectedPage))
			{
				AddShopPage();
				selectedPage = GetSelectedPage();
			}
			if (GodotObject.IsInstanceValid(selectedPage))
			{
				ShopItemConfig shopItemConfig = new ShopItemConfig
				{
					type = "Item"
				};
				Array<ShopItemConfig> array = CopyItems(selectedPage.itemList);
				array.Add(shopItemConfig);
				_selectedItemIndex = array.Count - 1;
				_selectedStageIndex = -1;
				_selectedItemIdentity = shopItemConfig;
				_selectedStageIdentity = null;
				_shopPropertyBinding.SetValue(selectedPage, "itemList", array, "添加商店商品", this, "RefreshShopEditorFromHistory");
			}
		}
	}

	private void RemoveSelectedShopItem()
	{
		ShopEditScope editScope = _editScope;
		if ((uint)(editScope - 2) > 1u)
		{
			ShopPageConfig selectedPage = GetSelectedPage();
			if (GodotObject.IsInstanceValid(selectedPage) && selectedPage.itemList != null && _selectedItemIndex >= 0 && _selectedItemIndex < selectedPage.itemList.Count)
			{
				Array<ShopItemConfig> array = CopyItems(selectedPage.itemList);
				array.RemoveAt(_selectedItemIndex);
				_selectedItemIndex = Math.Min(_selectedItemIndex, array.Count - 1);
				_selectedStageIndex = 0;
				_shopPropertyBinding.SetValue(selectedPage, "itemList", array, "删除商店商品", this, "RefreshShopEditorFromHistory");
			}
		}
	}

	private void MoveSelectedShopItem(int direction)
	{
		ShopEditScope editScope = _editScope;
		if ((uint)(editScope - 2) <= 1u)
		{
			return;
		}
		ShopPageConfig selectedPage = GetSelectedPage();
		if (GodotObject.IsInstanceValid(selectedPage) && selectedPage.itemList != null && _selectedItemIndex >= 0 && direction != 0)
		{
			int num = Math.Clamp(_selectedItemIndex + direction, 0, selectedPage.itemList.Count - 1);
			if (num != _selectedItemIndex)
			{
				Array<ShopItemConfig> array = CopyItems(selectedPage.itemList);
				ShopItemConfig item = array[_selectedItemIndex];
				array.RemoveAt(_selectedItemIndex);
				array.Insert(num, item);
				_selectedItemIndex = num;
				_shopPropertyBinding.SetValue(selectedPage, "itemList", array, "调整商店商品顺序", this, "RefreshShopEditorFromHistory");
			}
		}
	}

	private void AddShopStage()
	{
		if (_editScope != ShopEditScope.Stage)
		{
			ShopItemConfig selectedItem = GetSelectedItem();
			if (!GodotObject.IsInstanceValid(selectedItem))
			{
				AddShopItem();
				selectedItem = GetSelectedItem();
			}
			if (GodotObject.IsInstanceValid(selectedItem))
			{
				ShopItemStageConfig shopItemStageConfig = new ShopItemStageConfig
				{
					saveType = "Feature",
					type = "Coin",
					openMaxNum = 1,
					addNum = 1
				};
				Array<ShopItemStageConfig> array = CopyStages(selectedItem.stageList);
				array.Add(shopItemStageConfig);
				_selectedStageIndex = array.Count - 1;
				_selectedStageIdentity = shopItemStageConfig;
				_shopPropertyBinding.SetValue(selectedItem, "stageList", array, "添加商店阶段", this, "RefreshShopEditorFromHistory");
			}
		}
	}

	private void RemoveSelectedShopStage()
	{
		if (_editScope != ShopEditScope.Stage)
		{
			ShopItemConfig selectedItem = GetSelectedItem();
			if (GodotObject.IsInstanceValid(selectedItem) && selectedItem.stageList != null && _selectedStageIndex >= 0 && _selectedStageIndex < selectedItem.stageList.Count)
			{
				Array<ShopItemStageConfig> array = CopyStages(selectedItem.stageList);
				array.RemoveAt(_selectedStageIndex);
				_selectedStageIndex = Math.Min(_selectedStageIndex, array.Count - 1);
				_shopPropertyBinding.SetValue(selectedItem, "stageList", array, "删除商店阶段", this, "RefreshShopEditorFromHistory");
			}
		}
	}

	private void MoveSelectedShopStage(int direction)
	{
		if (_editScope == ShopEditScope.Stage)
		{
			return;
		}
		ShopItemConfig selectedItem = GetSelectedItem();
		if (GodotObject.IsInstanceValid(selectedItem) && selectedItem.stageList != null && _selectedStageIndex >= 0 && direction != 0)
		{
			int num = Math.Clamp(_selectedStageIndex + direction, 0, selectedItem.stageList.Count - 1);
			if (num != _selectedStageIndex)
			{
				Array<ShopItemStageConfig> array = CopyStages(selectedItem.stageList);
				ShopItemStageConfig item = array[_selectedStageIndex];
				array.RemoveAt(_selectedStageIndex);
				array.Insert(num, item);
				_selectedStageIndex = num;
				_shopPropertyBinding.SetValue(selectedItem, "stageList", array, "调整商店阶段顺序", this, "RefreshShopEditorFromHistory");
			}
		}
	}

	private void SetItemType(string value)
	{
		if (_editScope != ShopEditScope.Stage && _shopPropertyBinding != null)
		{
			ShopItemConfig shopItemConfig = (GodotObject.IsInstanceValid(_activeItemTypeEditTarget) ? _activeItemTypeEditTarget : GetSelectedItem());
			if (GodotObject.IsInstanceValid(shopItemConfig))
			{
				_shopPropertyBinding.SetValue(shopItemConfig, "type", value.StripEdges(), "修改商品类型", this, "RefreshShopEditorFromHistory");
				_activeItemTypeEditTarget = null;
			}
		}
	}

	private void BeginItemTypeEdit()
	{
		if (!_updatingControls && _editScope != ShopEditScope.Stage)
		{
			ShopItemConfig selectedItem = GetSelectedItem();
			if (GodotObject.IsInstanceValid(selectedItem))
			{
				_activeItemTypeEditTarget = selectedItem;
				_shopPropertyBinding?.BeginEdit(selectedItem, "type");
			}
		}
	}

	private void PreviewItemType(string value)
	{
		if (!_updatingControls && _editScope != ShopEditScope.Stage)
		{
			ShopItemConfig shopItemConfig = (GodotObject.IsInstanceValid(_activeItemTypeEditTarget) ? _activeItemTypeEditTarget : GetSelectedItem());
			if (GodotObject.IsInstanceValid(shopItemConfig))
			{
				_shopPropertyBinding?.PreviewValue(shopItemConfig, "type", value.StripEdges());
			}
		}
	}

	private void SetStageField(string propertyName, Variant value)
	{
		ShopItemStageConfig shopItemStageConfig = ((string.Equals(_activeStageEditProperty, propertyName, StringComparison.Ordinal) && GodotObject.IsInstanceValid(_activeStageEditTarget)) ? _activeStageEditTarget : GetSelectedStage());
		if (GodotObject.IsInstanceValid(shopItemStageConfig) && !string.IsNullOrWhiteSpace(propertyName) && _shopPropertyBinding != null)
		{
			_shopPropertyBinding.SetValue(shopItemStageConfig, propertyName, value, "修改商品阶段 " + propertyName, this, "RefreshShopEditorFromHistory");
			_activeStageEditTarget = null;
			_activeStageEditProperty = "";
		}
	}

	private void BeginStageFieldEdit(string propertyName)
	{
		if (!_updatingControls)
		{
			ShopItemStageConfig selectedStage = GetSelectedStage();
			if (GodotObject.IsInstanceValid(selectedStage))
			{
				_activeStageEditTarget = selectedStage;
				_activeStageEditProperty = propertyName;
				_shopPropertyBinding?.BeginEdit(selectedStage, propertyName);
			}
		}
	}

	private void PreviewStageField(string propertyName, Variant value)
	{
		if (!_updatingControls)
		{
			ShopItemStageConfig shopItemStageConfig = ((string.Equals(_activeStageEditProperty, propertyName, StringComparison.Ordinal) && GodotObject.IsInstanceValid(_activeStageEditTarget)) ? _activeStageEditTarget : GetSelectedStage());
			if (GodotObject.IsInstanceValid(shopItemStageConfig))
			{
				_shopPropertyBinding?.PreviewValue(shopItemStageConfig, propertyName, value);
			}
		}
	}

	private void SetPacketStageFields(string saveType, string saveKey)
	{
		ShopItemStageConfig selectedStage = GetSelectedStage();
		if (GodotObject.IsInstanceValid(selectedStage) && (!string.Equals(selectedStage.saveType, saveType, StringComparison.Ordinal) || !string.Equals(selectedStage.saveKey, saveKey, StringComparison.Ordinal)))
		{
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			if (xWUndoRedoManager == null)
			{
				selectedStage.Set("saveType", saveType);
				selectedStage.Set("saveKey", saveKey);
			}
			else
			{
				Variant value = selectedStage.Get("saveType");
				Variant value2 = selectedStage.Get("saveKey");
				xWUndoRedoManager.CreateAction("选择商店卡片");
				xWUndoRedoManager.AddDoProperty(selectedStage, "saveType", saveType);
				xWUndoRedoManager.AddDoProperty(selectedStage, "saveKey", saveKey);
				xWUndoRedoManager.AddUndoProperty(selectedStage, "saveType", value);
				xWUndoRedoManager.AddUndoProperty(selectedStage, "saveKey", value2);
				xWUndoRedoManager.AddDoMethod(this, "RefreshShopEditorFromHistory");
				xWUndoRedoManager.AddUndoMethod(this, "RefreshShopEditorFromHistory");
				xWUndoRedoManager.CommitAction();
			}
			OnShopPropertyEdited(committed: true);
		}
	}

	private void ShowShopOptionSelector(string title, List<ShopOptionItem> options, Action<ShopOptionItem> confirmed)
	{
		EnsureShopOptionSelectorWindow();
		if (GodotObject.IsInstanceValid(_shopOptionSelectorWindow))
		{
			_shopOptionSelectorWindow.Title = title;
			_shopOptionSelectorOptions.Clear();
			_shopOptionSelectorOptions.AddRange(options ?? new List<ShopOptionItem>());
			_shopOptionSelectorConfirmed = confirmed;
			_shopOptionSelectorWindow.OpenChoices(title, "从游戏内容与当前 Mod 资源中选择，单击查看来源", "", BuildShopOptionChoices(), SelectShopOptionChoice, "选用此项");
		}
	}

	private void EnsureShopOptionSelectorWindow()
	{
		if (!GodotObject.IsInstanceValid(_shopOptionSelectorWindow))
		{
			_shopOptionSelectorWindow = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_shopOptionSelectorWindow))
			{
				AddChild(_shopOptionSelectorWindow, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private List<XWGameplayResourceChoice> BuildShopOptionChoices()
	{
		List<XWGameplayResourceChoice> list = new List<XWGameplayResourceChoice>();
		foreach (ShopOptionItem shopOptionSelectorOption in _shopOptionSelectorOptions)
		{
			if (shopOptionSelectorOption != null && !string.IsNullOrWhiteSpace(shopOptionSelectorOption.Name))
			{
				list.Add(new XWGameplayResourceChoice(shopOptionSelectorOption.Name, shopOptionSelectorOption.Name, shopOptionSelectorOption.SourcePath ?? "", null, XWGameplayResourceKind.Resource, (shopOptionSelectorOption.SourceLabel ?? "").Contains("Mod", StringComparison.OrdinalIgnoreCase)));
			}
		}
		return list;
	}

	private void SelectShopOptionChoice(XWGameplayResourceChoice choice)
	{
		if (!(choice == null))
		{
			ShopOptionItem shopOptionItem = _shopOptionSelectorOptions.Find((ShopOptionItem option) => option != null && string.Equals(option.Name, choice.Key, StringComparison.OrdinalIgnoreCase) && string.Equals(option.SourcePath ?? "", choice.ResourcePath ?? "", StringComparison.OrdinalIgnoreCase)) ?? _shopOptionSelectorOptions.Find((ShopOptionItem option) => option != null && string.Equals(option.Name, choice.Key, StringComparison.OrdinalIgnoreCase));
			if (shopOptionItem != null)
			{
				RegisterProjectSelectionPath(shopOptionItem.SourcePath);
				_shopOptionSelectorConfirmed?.Invoke(shopOptionItem);
			}
		}
	}

	private List<ShopOptionItem> BuildPacketKeySelectionOptions()
	{
		List<ShopOptionItem> options = new List<ShopOptionItem>();
		LoadRegisteredKeysFromJson(options, "res://Asset/Config/Character/CharacterResource.json", "游戏内卡片", "内置 Character/Packet");
		CollectProjectResourceKeyOptions(options, "Resources/Cards", "Mod 卡片");
		CollectProjectResourceKeyOptions(options, "Resources/Characters", "Mod 角色");
		return SortShopOptions(options);
	}

	private List<ShopOptionItem> BuildNpcTalkSelectionOptions()
	{
		List<ShopOptionItem> options = new List<ShopOptionItem>();
		LoadRegisteredKeysFromJson(options, "res://Asset/Config/Npc/TalkResource.json", "游戏内对话", "内置 NPC Talk");
		CollectProjectResourceKeyOptions(options, "Resources/NpcTalks", "Mod 对话");
		return SortShopOptions(options);
	}

	private static void LoadRegisteredKeysFromJson(List<ShopOptionItem> options, string registryPath, string sourceLabel, string description)
	{
		if (options == null || string.IsNullOrWhiteSpace(registryPath) || !ResourceLoader.Exists(registryPath))
		{
			return;
		}
		try
		{
			Json json = ResourceLoader.Load<Json>(registryPath, null, ResourceLoader.CacheMode.Reuse);
			if (!GodotObject.IsInstanceValid(json) || json.Data.VariantType != Variant.Type.Dictionary)
			{
				return;
			}
			Dictionary dictionary = json.Data.AsGodotDictionary();
			foreach (Variant key in dictionary.Keys)
			{
				AddShopOption(options, key.AsString(), sourceLabel, registryPath, description);
				if (dictionary[key].VariantType != Variant.Type.Dictionary)
				{
					continue;
				}
				Variant valueOrDefault = dictionary[key].AsGodotDictionary().GetValueOrDefault("Packet", new Dictionary());
				if (valueOrDefault.VariantType != Variant.Type.Dictionary)
				{
					continue;
				}
				foreach (Variant key2 in valueOrDefault.AsGodotDictionary().Keys)
				{
					AddShopOption(options, key2.AsString(), sourceLabel, registryPath, "Packet of " + key.AsString());
				}
			}
		}
		catch (Exception ex)
		{
			GD.PushWarning("Shop selector registry load failed: " + registryPath + " " + ex.Message);
		}
	}

	private static void CollectProjectResourceKeyOptions(List<ShopOptionItem> options, string relativeFolder, string sourceLabel)
	{
		string currentProjectPathForShopEditor = GetCurrentProjectPathForShopEditor();
		if (options == null || string.IsNullOrWhiteSpace(currentProjectPathForShopEditor) || string.IsNullOrWhiteSpace(relativeFolder))
		{
			return;
		}
		string path = Path.Combine(currentProjectPathForShopEditor, relativeFolder.Replace('/', Path.DirectorySeparatorChar));
		if (!Directory.Exists(path))
		{
			return;
		}
		foreach (string item in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
		{
			switch (Path.GetExtension(item).ToLowerInvariant())
			{
			case ".tres":
			case ".res":
			case ".json":
			case ".csv":
			{
				string text = ReadSaveKeyFromResourceFile(item);
				if (string.IsNullOrWhiteSpace(text))
				{
					text = Path.GetFileNameWithoutExtension(item);
				}
				AddShopOption(options, text, sourceLabel, item, ToProjectRelativeDisplayPath(item, currentProjectPathForShopEditor));
				break;
			}
			}
		}
	}

	private void RefreshResourceMetadata()
	{
		if (GodotObject.IsInstanceValid(_summaryLabel) && GodotObject.IsInstanceValid(_editingShop))
		{
			_summaryLabel.Text = BuildSummary(_editingShop);
		}
	}

	private void OpenJsonSourceDialog()
	{
		if (_editScope == ShopEditScope.Shop && GodotObject.IsInstanceValid(_jsonSourceDialog))
		{
			_jsonSourceDialog.PopupCenteredClamped(new Vector2I(900, 620), 0.9f);
		}
	}

	private void ApplyJsonSource(string path)
	{
		if (_editScope != ShopEditScope.Shop || !(CurrentResource is ShopConfig shop) || string.IsNullOrWhiteSpace(path))
		{
			return;
		}
		Json json = ResourceLoader.Load<Json>(path, "", ResourceLoader.CacheMode.Ignore);
		if (!TryValidateShopJson(json, out var reason))
		{
			if (GodotObject.IsInstanceValid(_dataStatus))
			{
				_dataStatus.Text = reason;
			}
			XWEditorInterface.Instance?.ShowToast(reason, 1);
			return;
		}
		ShopConfig shopConfig;
		try
		{
			shopConfig = new ShopConfig
			{
				data = json
			};
		}
		catch (Exception ex)
		{
			string text = "商店 JSON 解析失败：" + ex.Message;
			if (GodotObject.IsInstanceValid(_dataStatus))
			{
				_dataStatus.Text = text;
			}
			XWEditorInterface.Instance?.ShowToast(text, 1);
			return;
		}
		SetShopDataWithPageSnapshot(shop, json, CopyPages(shopConfig.pageList), "更换商店 JSON 来源");
	}

	private static bool TryValidateShopJson(Json source, out string reason)
	{
		if (!GodotObject.IsInstanceValid(source))
		{
			reason = "无法加载商店 JSON。";
			return false;
		}
		if (source.Data.VariantType != Variant.Type.Dictionary)
		{
			reason = "商店 JSON 根节点必须是 Dictionary。";
			return false;
		}
		Variant valueOrDefault = source.Data.AsGodotDictionary().GetValueOrDefault("Page");
		if (valueOrDefault.VariantType != Variant.Type.Array)
		{
			reason = "商店 JSON 的 Page 必须是 Array。";
			return false;
		}
		Godot.Collections.Array array = valueOrDefault.AsGodotArray();
		for (int i = 0; i < array.Count; i++)
		{
			Variant variant = array[i];
			if (variant.VariantType != Variant.Type.Dictionary)
			{
				reason = $"商店 JSON 的 Page[{i}] 必须是 Dictionary。";
				return false;
			}
			Variant valueOrDefault2 = variant.AsGodotDictionary().GetValueOrDefault("Item");
			if (valueOrDefault2.VariantType != Variant.Type.Array)
			{
				reason = $"商店 JSON 的 Page[{i}].Item 必须是 Array。";
				return false;
			}
			Godot.Collections.Array array2 = valueOrDefault2.AsGodotArray();
			for (int j = 0; j < array2.Count; j++)
			{
				Variant variant2 = array2[j];
				if (variant2.VariantType != Variant.Type.Dictionary)
				{
					reason = $"商店 JSON 的 Page[{i}].Item[{j}] 必须是 Dictionary。";
					return false;
				}
				Variant valueOrDefault3 = variant2.AsGodotDictionary().GetValueOrDefault("Stage");
				if (valueOrDefault3.VariantType == Variant.Type.Nil)
				{
					continue;
				}
				if (valueOrDefault3.VariantType != Variant.Type.Array)
				{
					reason = $"商店 JSON 的 Page[{i}].Item[{j}].Stage 必须是 Array。";
					return false;
				}
				Godot.Collections.Array array3 = valueOrDefault3.AsGodotArray();
				for (int k = 0; k < array3.Count; k++)
				{
					if (array3[k].VariantType != Variant.Type.Dictionary)
					{
						reason = $"商店 JSON 的 Page[{i}].Item[{j}].Stage[{k}] 必须是 Dictionary。";
						return false;
					}
				}
			}
		}
		reason = "";
		return true;
	}

	private void ConfirmClearJsonSource()
	{
		if (_editScope == ShopEditScope.Shop && GodotObject.IsInstanceValid(_clearJsonConfirmationDialog))
		{
			_clearJsonConfirmationDialog.PopupCentered(new Vector2I(520, 220));
		}
	}

	private void ClearJsonSource()
	{
		if (_editScope != ShopEditScope.Shop || !(CurrentResource is ShopConfig shopConfig))
		{
			return;
		}
		if (!GodotObject.IsInstanceValid(shopConfig.data))
		{
			Array<ShopPageConfig> pageList = shopConfig.pageList;
			if (pageList == null || pageList.Count == 0)
			{
				return;
			}
		}
		SetShopDataWithPageSnapshot(shopConfig, null, new Array<ShopPageConfig>(), "清空商店页面");
	}

	private void SetShopDataWithPageSnapshot(ShopConfig shop, Json nextData, Array<ShopPageConfig> nextPages, string actionName)
	{
		if (GodotObject.IsInstanceValid(shop))
		{
			Json from = shop.data;
			Array<ShopPageConfig> array = CopyPages(shop.pageList);
			XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
			if (xWUndoRedoManager == null)
			{
				shop.Set("pageList", new Array<ShopPageConfig>());
				shop.Set("data", nextData);
				shop.Set("pageList", nextPages);
			}
			else
			{
				Array<ShopPageConfig> array2 = new Array<ShopPageConfig>();
				Array<ShopPageConfig> array3 = new Array<ShopPageConfig>();
				xWUndoRedoManager.CreateAction(actionName);
				xWUndoRedoManager.AddDoProperty(shop, "pageList", array2);
				xWUndoRedoManager.AddDoProperty(shop, "data", Variant.From(in nextData));
				xWUndoRedoManager.AddDoProperty(shop, "pageList", nextPages);
				xWUndoRedoManager.AddUndoProperty(shop, "pageList", array3);
				xWUndoRedoManager.AddUndoProperty(shop, "data", Variant.From(in from));
				xWUndoRedoManager.AddUndoProperty(shop, "pageList", array);
				xWUndoRedoManager.AddDoMethod(this, "RefreshShopEditorFromHistory");
				xWUndoRedoManager.AddUndoMethod(this, "RefreshShopEditorFromHistory");
				xWUndoRedoManager.CommitAction();
			}
			_selectedPageIndex = ((nextPages.Count <= 0) ? (-1) : 0);
			_selectedItemIndex = 0;
			_selectedStageIndex = 0;
			OnShopPropertyEdited(committed: true);
		}
	}

	private void RefreshDataStatus()
	{
		if (GodotObject.IsInstanceValid(_dataStatus))
		{
			if (_editScope != ShopEditScope.Shop)
			{
				_dataStatus.Text = "嵌套资源范围：JSON 来源只可在完整 ShopConfig 中编辑。";
			}
			else
			{
				_dataStatus.Text = (GodotObject.IsInstanceValid(_editingShop?.data) ? ("JSON 来源：" + _editingShop.data.ResourcePath.GetFile()) : "无 JSON 来源；当前页面为手工配置。");
			}
		}
	}

	private void RefreshAll()
	{
		if (_editingShop == null)
		{
			return;
		}
		_updatingControls = true;
		try
		{
			NormalizeSelection();
			if (GodotObject.IsInstanceValid(_summaryLabel))
			{
				_summaryLabel.Text = BuildSummary(_editingShop);
			}
			RefreshPageList();
			RefreshItemList();
			RefreshStageList();
			RefreshStageControls();
			RefreshDataStatus();
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void RefreshPageList()
	{
		if (GodotObject.IsInstanceValid(_pageList))
		{
			_pageList.Clear();
			int valueOrDefault = (_editingShop?.pageList?.Count).GetValueOrDefault();
			if (valueOrDefault == 0)
			{
				_pageList.AddItem("没有页面");
			}
			for (int i = 0; i < valueOrDefault; i++)
			{
				int valueOrDefault2 = (_editingShop.pageList[i]?.itemList?.Count).GetValueOrDefault();
				_pageList.AddItem($"页面 {i + 1}  商品 {valueOrDefault2}");
			}
			if (_selectedPageIndex >= 0 && _selectedPageIndex < valueOrDefault)
			{
				_pageList.Select(_selectedPageIndex);
			}
			if (GodotObject.IsInstanceValid(_pageSummaryLabel))
			{
				_pageSummaryLabel.Text = $"页面数: {valueOrDefault}";
			}
		}
	}

	private void RefreshItemList()
	{
		if (GodotObject.IsInstanceValid(_itemList))
		{
			_itemList.Clear();
			ShopPageConfig selectedPage = GetSelectedPage();
			int valueOrDefault = (selectedPage?.itemList?.Count).GetValueOrDefault();
			if (valueOrDefault == 0)
			{
				_itemList.AddItem("没有商品");
			}
			for (int i = 0; i < valueOrDefault; i++)
			{
				ShopItemConfig shopItemConfig = selectedPage.itemList[i];
				_itemList.AddItem($"{i + 1}. {EmptyToPlaceholder(shopItemConfig?.type)}  阶段 {(shopItemConfig?.stageList?.Count).GetValueOrDefault()}");
			}
			if (_selectedItemIndex >= 0 && _selectedItemIndex < valueOrDefault)
			{
				_itemList.Select(_selectedItemIndex);
			}
			if (GodotObject.IsInstanceValid(_itemSummaryLabel))
			{
				_itemSummaryLabel.Text = $"当前页面: {DisplayIndex(_selectedPageIndex)}  商品数: {valueOrDefault}";
			}
			if (GodotObject.IsInstanceValid(_itemTypeEdit))
			{
				_itemTypeEdit.Text = GetSelectedItem()?.type ?? "";
			}
		}
	}

	private void RefreshStageList()
	{
		if (GodotObject.IsInstanceValid(_stageList))
		{
			_stageList.Clear();
			ShopItemConfig selectedItem = GetSelectedItem();
			int valueOrDefault = (selectedItem?.stageList?.Count).GetValueOrDefault();
			if (valueOrDefault == 0)
			{
				_stageList.AddItem("没有阶段");
			}
			for (int i = 0; i < valueOrDefault; i++)
			{
				ShopItemStageConfig shopItemStageConfig = selectedItem.stageList[i];
				_stageList.AddItem($"{i + 1}. {EmptyToPlaceholder(shopItemStageConfig?.saveKey)}  {shopItemStageConfig?.cost ?? 0} {EmptyToPlaceholder(shopItemStageConfig?.type)}");
			}
			if (_selectedStageIndex >= 0 && _selectedStageIndex < valueOrDefault)
			{
				_stageList.Select(_selectedStageIndex);
			}
			if (GodotObject.IsInstanceValid(_stageSummaryLabel))
			{
				_stageSummaryLabel.Text = $"当前商品: {DisplayIndex(_selectedItemIndex)}  阶段数: {valueOrDefault}";
			}
		}
	}

	private void RefreshStageControls()
	{
		ShopItemStageConfig selectedStage = GetSelectedStage();
		bool flag = GodotObject.IsInstanceValid(selectedStage);
		if (GodotObject.IsInstanceValid(_stageSaveTypeOption))
		{
			SelectOptionByText(_stageSaveTypeOption, flag ? selectedStage.saveType : "Feature");
		}
		_stageSaveTypeVisualChoices?.RefreshSelection();
		if (GodotObject.IsInstanceValid(_stageSaveKeyEdit))
		{
			_stageSaveKeyEdit.Text = (flag ? selectedStage.saveKey : "");
		}
		if (GodotObject.IsInstanceValid(_stageTexturePicker))
		{
			_stageTexturePicker.SetEditedResource(flag ? selectedStage.texture : null);
		}
		if (GodotObject.IsInstanceValid(_stageDescribeEdit))
		{
			_stageDescribeEdit.Text = (flag ? selectedStage.describe : "");
		}
		if (GodotObject.IsInstanceValid(_stageNpcTalkEdit))
		{
			_stageNpcTalkEdit.Text = (flag ? selectedStage.npcTalk : "");
		}
		if (GodotObject.IsInstanceValid(_stageCostTypeOption))
		{
			SelectOptionByText(_stageCostTypeOption, flag ? selectedStage.type : "Coin");
		}
		_stageCostTypeVisualChoices?.RefreshSelection();
		SetSpinValue(_stageCostSpin, flag ? selectedStage.cost : 0);
		SetSpinValue(_stageOpenMinSpin, flag ? selectedStage.openMinNum : 0);
		SetSpinValue(_stageOpenMaxSpin, (!flag) ? 1 : selectedStage.openMaxNum);
		SetSpinValue(_stageAddNumSpin, (!flag) ? 1 : selectedStage.addNum);
		if (GodotObject.IsInstanceValid(_stageTexturePreview))
		{
			_stageTexturePreview.Texture = (flag ? selectedStage.texture : null);
		}
		if (GodotObject.IsInstanceValid(_stagePreviewTitle))
		{
			_stagePreviewTitle.Text = (flag ? ("商品: " + EmptyToPlaceholder(selectedStage.saveKey)) : "商品: 未选择");
		}
		if (GodotObject.IsInstanceValid(_stagePreviewMeta))
		{
			_stagePreviewMeta.Text = (flag ? $"价格: {selectedStage.cost} {EmptyToPlaceholder(selectedStage.type)}" : "价格: -");
		}
		if (GodotObject.IsInstanceValid(_stagePreviewStock))
		{
			_stagePreviewStock.Text = (flag ? $"库存: {selectedStage.openMinNum}-{selectedStage.openMaxNum} +{selectedStage.addNum}" : "库存: -");
		}
		RefreshPurchaseSimulation();
	}

	private void DisposeStageVisualChoices()
	{
		_stageSaveTypeVisualChoices?.Dispose();
		_stageSaveTypeVisualChoices = null;
		_stageCostTypeVisualChoices?.Dispose();
		_stageCostTypeVisualChoices = null;
	}

	private void NormalizeSelection()
	{
		int valueOrDefault = (_editingShop?.pageList?.Count).GetValueOrDefault();
		_selectedPageIndex = NormalizeIndex(_selectedPageIndex, valueOrDefault);
		int valueOrDefault2 = (GetSelectedPage()?.itemList?.Count).GetValueOrDefault();
		_selectedItemIndex = NormalizeIndex(_selectedItemIndex, valueOrDefault2);
		int valueOrDefault3 = (GetSelectedItem()?.stageList?.Count).GetValueOrDefault();
		_selectedStageIndex = NormalizeIndex(_selectedStageIndex, valueOrDefault3);
	}

	private static int NormalizeIndex(int index, int count)
	{
		if (count <= 0)
		{
			return -1;
		}
		if (index < 0)
		{
			return 0;
		}
		return Math.Min(index, count - 1);
	}

	private ShopPageConfig GetSelectedPage()
	{
		if (_editingShop?.pageList == null || _selectedPageIndex < 0 || _selectedPageIndex >= _editingShop.pageList.Count)
		{
			return null;
		}
		return _editingShop.pageList[_selectedPageIndex];
	}

	private ShopItemConfig GetSelectedItem()
	{
		ShopPageConfig selectedPage = GetSelectedPage();
		if (!GodotObject.IsInstanceValid(selectedPage) || selectedPage.itemList == null || _selectedItemIndex < 0 || _selectedItemIndex >= selectedPage.itemList.Count)
		{
			return null;
		}
		return selectedPage.itemList[_selectedItemIndex];
	}

	private ShopItemStageConfig GetSelectedStage()
	{
		ShopItemConfig selectedItem = GetSelectedItem();
		if (!GodotObject.IsInstanceValid(selectedItem) || selectedItem.stageList == null || _selectedStageIndex < 0 || _selectedStageIndex >= selectedItem.stageList.Count)
		{
			return null;
		}
		return selectedItem.stageList[_selectedStageIndex];
	}

	private static Array<ShopPageConfig> CopyPages(Array<ShopPageConfig> source)
	{
		if (source != null)
		{
			return new Array<ShopPageConfig>(source);
		}
		return new Array<ShopPageConfig>();
	}

	private static Array<ShopItemConfig> CopyItems(Array<ShopItemConfig> source)
	{
		if (source != null)
		{
			return new Array<ShopItemConfig>(source);
		}
		return new Array<ShopItemConfig>();
	}

	private static Array<ShopItemStageConfig> CopyStages(Array<ShopItemStageConfig> source)
	{
		if (source != null)
		{
			return new Array<ShopItemStageConfig>(source);
		}
		return new Array<ShopItemStageConfig>();
	}

	private void OnShopPropertyEdited(bool committed)
	{
		if (_editingShop == null || !GodotObject.IsInstanceValid(CurrentResource))
		{
			return;
		}
		if (committed)
		{
			NotifyCurrentResourceEdited();
			if (CurrentResource != _editingShop)
			{
				_editingShop.EmitChanged();
			}
			if (GodotObject.IsInstanceValid(_runtimeShopPreview))
			{
				_runtimeShopPreview.EditPreviewConfig(_editingShop);
			}
			RefreshAll();
		}
		else
		{
			CurrentResource.EmitChanged();
			if (CurrentResource != _editingShop)
			{
				_editingShop.EmitChanged();
			}
			MarkCurrentResourceDirty();
			RefreshLiveEditPreview();
		}
		CaptureSelectionIdentity();
	}

	private void RefreshLiveEditPreview()
	{
		ShopItemConfig selectedItem = GetSelectedItem();
		if (GodotObject.IsInstanceValid(_itemList) && GodotObject.IsInstanceValid(selectedItem) && _selectedItemIndex >= 0 && _selectedItemIndex < _itemList.ItemCount)
		{
			_itemList.SetItemText(_selectedItemIndex, $"{_selectedItemIndex + 1}. {EmptyToPlaceholder(selectedItem.type)}  阶段 {selectedItem.stageList?.Count ?? 0}");
		}
		ShopItemStageConfig selectedStage = GetSelectedStage();
		if (GodotObject.IsInstanceValid(selectedStage))
		{
			if (GodotObject.IsInstanceValid(_stageList) && _selectedStageIndex >= 0 && _selectedStageIndex < _stageList.ItemCount)
			{
				_stageList.SetItemText(_selectedStageIndex, $"{_selectedStageIndex + 1}. {EmptyToPlaceholder(selectedStage.saveKey)}  {selectedStage.cost} {EmptyToPlaceholder(selectedStage.type)}");
			}
			if (GodotObject.IsInstanceValid(_stagePreviewTitle))
			{
				_stagePreviewTitle.Text = "商品: " + EmptyToPlaceholder(selectedStage.saveKey);
			}
			if (GodotObject.IsInstanceValid(_stagePreviewMeta))
			{
				_stagePreviewMeta.Text = $"价格: {selectedStage.cost} {EmptyToPlaceholder(selectedStage.type)}";
			}
			if (GodotObject.IsInstanceValid(_stagePreviewStock))
			{
				_stagePreviewStock.Text = $"库存: {selectedStage.openMinNum}-{selectedStage.openMaxNum} +{selectedStage.addNum}";
			}
			RefreshPurchaseSimulation();
		}
	}

	private void CaptureSelectionIdentity()
	{
		_selectedPageIdentity = GetSelectedPage();
		_selectedItemIdentity = GetSelectedItem();
		_selectedStageIdentity = GetSelectedStage();
	}

	private void RestoreSelectionIdentity()
	{
		int num = IndexOfIdentity(_editingShop?.pageList, _selectedPageIdentity);
		if (num >= 0)
		{
			_selectedPageIndex = num;
		}
		int num2 = IndexOfIdentity(GetSelectedPage()?.itemList, _selectedItemIdentity);
		if (num2 >= 0)
		{
			_selectedItemIndex = num2;
		}
		int num3 = IndexOfIdentity(GetSelectedItem()?.stageList, _selectedStageIdentity);
		if (num3 >= 0)
		{
			_selectedStageIndex = num3;
		}
		NormalizeSelection();
	}

	private static int IndexOfIdentity<[MustBeVariant] T>(Array<T> values, T target) where T : GodotObject
	{
		if (values == null || !GodotObject.IsInstanceValid(target))
		{
			return -1;
		}
		for (int i = 0; i < values.Count; i++)
		{
			if (values[i] == target)
			{
				return i;
			}
		}
		return -1;
	}

	public void RefreshShopEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager == null || (!xWUndoRedoManager.IsUndoing() && !xWUndoRedoManager.IsRedoing()) || CanvasGrid == null)
		{
			return;
		}
		_shopPropertyBinding?.Dispose();
		_shopPropertyBinding = null;
		foreach (Node child in CanvasGrid.GetChildren())
		{
			CanvasGrid.RemoveChild(child);
			child.QueueFree();
		}
		RenderCustomVisualPreset(null);
		RestoreSelectionIdentity();
		RefreshAll();
		CaptureSelectionIdentity();
	}

	private void AddSummaryRows(ShopConfig shop)
	{
		AddItemIfMissing(PreviewList, BuildSummary(shop));
		AddItemIfMissing(TimelineList, $"pageList -> {(shop?.pageList?.Count).GetValueOrDefault()}");
		AddItemIfMissing(GraphList, "商店 -> pageList -> itemList -> stageList -> saveKey/texture/npcTalk");
		AddItemIfMissing(ReferenceList, "商店资源 -> " + CurrentResourcePath);
	}

	private static string BuildSummary(ShopConfig shop)
	{
		int valueOrDefault = (shop?.pageList?.Count).GetValueOrDefault();
		int num = 0;
		int num2 = 0;
		if (shop?.pageList != null)
		{
			foreach (ShopPageConfig page in shop.pageList)
			{
				num += (page?.itemList?.Count).GetValueOrDefault();
				if (page?.itemList == null)
				{
					continue;
				}
				foreach (ShopItemConfig item in page.itemList)
				{
					num2 += (item?.stageList?.Count).GetValueOrDefault();
				}
			}
		}
		return $"商店: 页面 {valueOrDefault}, 商品 {num}, 阶段 {num2}";
	}

	private static void AddShopOption(List<ShopOptionItem> options, string name, string sourceLabel, string sourcePath, string description)
	{
		if (options == null || string.IsNullOrWhiteSpace(name))
		{
			return;
		}
		string text = name.StripEdges();
		foreach (ShopOptionItem option in options)
		{
			if (string.Equals(option.Name, text, StringComparison.OrdinalIgnoreCase))
			{
				MergeText(ref option.SourceLabel, sourceLabel);
				if (!string.IsNullOrWhiteSpace(sourcePath))
				{
					option.SourcePath = sourcePath;
				}
				MergeText(ref option.Description, description);
				return;
			}
		}
		options.Add(new ShopOptionItem
		{
			Name = text,
			SourceLabel = (sourceLabel ?? ""),
			SourcePath = (sourcePath ?? ""),
			Description = (description ?? "")
		});
	}

	private static List<ShopOptionItem> SortShopOptions(List<ShopOptionItem> options)
	{
		options.Sort((ShopOptionItem a, ShopOptionItem b) => string.Compare(a?.Name, b?.Name, StringComparison.OrdinalIgnoreCase));
		return options;
	}

	private static void MergeText(ref string target, string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			if (string.IsNullOrWhiteSpace(target))
			{
				target = value;
			}
			else if (!target.Contains(value, StringComparison.OrdinalIgnoreCase))
			{
				target = target + "+" + value;
			}
		}
	}

	private static string ReadSaveKeyFromResourceFile(string filePath)
	{
		if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
		{
			return "";
		}
		try
		{
			foreach (string item in File.ReadLines(filePath))
			{
				string text = item.Trim();
				if (text.StartsWith("saveKey", StringComparison.Ordinal))
				{
					int num = text.IndexOf('=');
					if (num >= 0)
					{
						string text2 = text;
						int num2 = num + 1;
						return CleanResourceTextValue(text2.Substring(num2, text2.Length - num2));
					}
				}
			}
		}
		catch (Exception ex)
		{
			GD.PushWarning("Shop editor key scan failed: " + filePath + " " + ex.Message);
		}
		return "";
	}

	private static string CleanResourceTextValue(string value)
	{
		string text = (value ?? "").Trim();
		if (text.StartsWith("&", StringComparison.Ordinal))
		{
			string text2 = text;
			text = text2.Substring(1, text2.Length - 1).TrimStart();
		}
		if (text.Length >= 2 && text[0] == '"')
		{
			string text3 = text;
			if (text3[text3.Length - 1] == '"')
			{
				string text2 = text;
				text = text2.Substring(1, text2.Length - 1 - 1);
			}
		}
		return text.Replace("\\\"", "\"").StripEdges();
	}

	private static void RegisterProjectSelectionPath(string sourcePath)
	{
		string currentProjectPathForShopEditor = GetCurrentProjectPathForShopEditor();
		if (!string.IsNullOrWhiteSpace(currentProjectPathForShopEditor) && !string.IsNullOrWhiteSpace(sourcePath))
		{
			XWModManifestSyncService.RegisterPath(currentProjectPathForShopEditor, sourcePath);
		}
	}

	private static string GetCurrentProjectPathForShopEditor()
	{
		string text = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		if (XWEditorInterface.Instance?.GetEditorPanel() is ModEditorPanel modEditorPanel)
		{
			return modEditorPanel.GetCurrentProject()?.ProjectPath ?? "";
		}
		return "";
	}

	private static string ToProjectRelativeDisplayPath(string filePath, string projectPath)
	{
		if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(projectPath))
		{
			return filePath ?? "";
		}
		try
		{
			return Path.GetRelativePath(projectPath, filePath).Replace('\\', '/');
		}
		catch
		{
			return filePath;
		}
	}

	private static void SelectOptionByText(OptionButton option, string text)
	{
		if (!GodotObject.IsInstanceValid(option))
		{
			return;
		}
		for (int i = 0; i < option.ItemCount; i++)
		{
			if (!(option.GetItemText(i) != text))
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

	private static void SetSpinValue(SpinBox spin, double value)
	{
		if (GodotObject.IsInstanceValid(spin))
		{
			spin.SetValueNoSignal(value);
		}
	}

	private static string DisplayIndex(int index)
	{
		if (index >= 0)
		{
			return (index + 1).ToString();
		}
		return "未选择";
	}

	private static string EmptyToPlaceholder(string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return "未配置";
	}

	private static string FormatResource(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "未配置";
		}
		if (!string.IsNullOrWhiteSpace(resource.ResourcePath))
		{
			return resource.ResourcePath.GetFile();
		}
		return resource.ResourceName;
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
		return new List<MethodInfo>(69)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateShopPreviewAdapter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.HasCompleteShopVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderShopEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shop", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindShopLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "shop", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ConfigureScopeControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetScopeButton, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false),
				new PropertyInfo(Variant.Type.String, "nodePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "disabledReason", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindStageFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePurchaseSimulationPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPurchaseSimulation, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SelectShopPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectShopItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectShopStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddShopPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedShopPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedShopPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddShopItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedShopItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedShopItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddShopStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedShopStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedShopStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetItemType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginItemTypeEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PreviewItemType, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetStageField, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginStageFieldEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PreviewStageField, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPacketStageFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "saveType", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "saveKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureShopOptionSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshResourceMetadata, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenJsonSourceDialog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyJsonSource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ConfirmClearJsonSource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearJsonSource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetShopDataWithPageSnapshot, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shop", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "nextData", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("JSON"), exported: false),
				new PropertyInfo(Variant.Type.Array, "nextPages", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshDataStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPageList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshItemList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshStageList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshStageControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeStageVisualChoices, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizeSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizeIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "count", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetSelectedPage, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectedItem, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectedStage, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CopyPages, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyItems, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CopyStages, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnShopPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshLiveEditPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CaptureSelectionIdentity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RestoreSelectionIdentity, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshShopEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shop", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "shop", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadSaveKeyFromResourceFile, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CleanResourceTextValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterProjectSelectionPath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentProjectPathForShopEditor, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ToProjectRelativeDisplayPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "projectPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectOptionByText, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSpinValue, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spin", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Float, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisplayIndex, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EmptyToPlaceholder, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatResource, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
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
		if (method == MethodName.CreateShopPreviewAdapter && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<ShopConfig>(CreateShopPreviewAdapter(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.HasCompleteShopVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteShopVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.RenderShopEditor && args.Count == 1)
		{
			RenderShopEditor(VariantUtils.ConvertTo<ShopConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindShopLayout && args.Count == 2)
		{
			BindShopLayout(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<ShopConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigureScopeControls && args.Count == 1)
		{
			ConfigureScopeControls(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetScopeButton && args.Count == 4)
		{
			SetScopeButton(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindStageFields && args.Count == 1)
		{
			BindStageFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePurchaseSimulationPanel && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Control>(CreatePurchaseSimulationPanel());
			return true;
		}
		if (method == MethodName.RefreshPurchaseSimulation && args.Count == 0)
		{
			RefreshPurchaseSimulation();
			ret = default;
			return true;
		}
		if (method == MethodName.SelectShopPage && args.Count == 1)
		{
			SelectShopPage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectShopItem && args.Count == 1)
		{
			SelectShopItem(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectShopStage && args.Count == 1)
		{
			SelectShopStage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddShopPage && args.Count == 0)
		{
			AddShopPage();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedShopPage && args.Count == 0)
		{
			RemoveSelectedShopPage();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedShopPage && args.Count == 1)
		{
			MoveSelectedShopPage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddShopItem && args.Count == 0)
		{
			AddShopItem();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedShopItem && args.Count == 0)
		{
			RemoveSelectedShopItem();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedShopItem && args.Count == 1)
		{
			MoveSelectedShopItem(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddShopStage && args.Count == 0)
		{
			AddShopStage();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedShopStage && args.Count == 0)
		{
			RemoveSelectedShopStage();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedShopStage && args.Count == 1)
		{
			MoveSelectedShopStage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetItemType && args.Count == 1)
		{
			SetItemType(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginItemTypeEdit && args.Count == 0)
		{
			BeginItemTypeEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewItemType && args.Count == 1)
		{
			PreviewItemType(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetStageField && args.Count == 2)
		{
			SetStageField(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginStageFieldEdit && args.Count == 1)
		{
			BeginStageFieldEdit(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewStageField && args.Count == 2)
		{
			PreviewStageField(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPacketStageFields && args.Count == 2)
		{
			SetPacketStageFields(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureShopOptionSelectorWindow && args.Count == 0)
		{
			EnsureShopOptionSelectorWindow();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshResourceMetadata && args.Count == 0)
		{
			RefreshResourceMetadata();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenJsonSourceDialog && args.Count == 0)
		{
			OpenJsonSourceDialog();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyJsonSource && args.Count == 1)
		{
			ApplyJsonSource(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ConfirmClearJsonSource && args.Count == 0)
		{
			ConfirmClearJsonSource();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearJsonSource && args.Count == 0)
		{
			ClearJsonSource();
			ret = default;
			return true;
		}
		if (method == MethodName.SetShopDataWithPageSnapshot && args.Count == 4)
		{
			SetShopDataWithPageSnapshot(VariantUtils.ConvertTo<ShopConfig>(in args[0]), VariantUtils.ConvertTo<Json>(in args[1]), VariantUtils.ConvertToArray<ShopPageConfig>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshDataStatus && args.Count == 0)
		{
			RefreshDataStatus();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAll && args.Count == 0)
		{
			RefreshAll();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPageList && args.Count == 0)
		{
			RefreshPageList();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshItemList && args.Count == 0)
		{
			RefreshItemList();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshStageList && args.Count == 0)
		{
			RefreshStageList();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshStageControls && args.Count == 0)
		{
			RefreshStageControls();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeStageVisualChoices && args.Count == 0)
		{
			DisposeStageVisualChoices();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeSelection && args.Count == 0)
		{
			NormalizeSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeIndex && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(NormalizeIndex(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.GetSelectedPage && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ShopPageConfig>(GetSelectedPage());
			return true;
		}
		if (method == MethodName.GetSelectedItem && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ShopItemConfig>(GetSelectedItem());
			return true;
		}
		if (method == MethodName.GetSelectedStage && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ShopItemStageConfig>(GetSelectedStage());
			return true;
		}
		if (method == MethodName.CopyPages && args.Count == 1)
		{
			Array<ShopPageConfig> array = CopyPages(VariantUtils.ConvertToArray<ShopPageConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CopyItems && args.Count == 1)
		{
			Array<ShopItemConfig> array2 = CopyItems(VariantUtils.ConvertToArray<ShopItemConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.CopyStages && args.Count == 1)
		{
			Array<ShopItemStageConfig> array3 = CopyStages(VariantUtils.ConvertToArray<ShopItemStageConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
			return true;
		}
		if (method == MethodName.OnShopPropertyEdited && args.Count == 1)
		{
			OnShopPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshLiveEditPreview && args.Count == 0)
		{
			RefreshLiveEditPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.CaptureSelectionIdentity && args.Count == 0)
		{
			CaptureSelectionIdentity();
			ret = default;
			return true;
		}
		if (method == MethodName.RestoreSelectionIdentity && args.Count == 0)
		{
			RestoreSelectionIdentity();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshShopEditorFromHistory && args.Count == 0)
		{
			RefreshShopEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 1)
		{
			AddSummaryRows(VariantUtils.ConvertTo<ShopConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSummary(VariantUtils.ConvertTo<ShopConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadSaveKeyFromResourceFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadSaveKeyFromResourceFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CleanResourceTextValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CleanResourceTextValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterProjectSelectionPath && args.Count == 1)
		{
			RegisterProjectSelectionPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentProjectPathForShopEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrentProjectPathForShopEditor());
			return true;
		}
		if (method == MethodName.ToProjectRelativeDisplayPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ToProjectRelativeDisplayPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SelectOptionByText && args.Count == 2)
		{
			SelectOptionByText(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSpinValue && args.Count == 2)
		{
			SetSpinValue(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisplayIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DisplayIndex(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.EmptyToPlaceholder && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyToPlaceholder(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
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
		if (method == MethodName.HasCompleteShopVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteShopVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.SetScopeButton && args.Count == 4)
		{
			SetScopeButton(VariantUtils.ConvertTo<Node>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<string>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizeIndex && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<int>(NormalizeIndex(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1])));
			return true;
		}
		if (method == MethodName.CopyPages && args.Count == 1)
		{
			Array<ShopPageConfig> array = CopyPages(VariantUtils.ConvertToArray<ShopPageConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.CopyItems && args.Count == 1)
		{
			Array<ShopItemConfig> array2 = CopyItems(VariantUtils.ConvertToArray<ShopItemConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.CopyStages && args.Count == 1)
		{
			Array<ShopItemStageConfig> array3 = CopyStages(VariantUtils.ConvertToArray<ShopItemStageConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array3);
			return true;
		}
		if (method == MethodName.BuildSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSummary(VariantUtils.ConvertTo<ShopConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadSaveKeyFromResourceFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadSaveKeyFromResourceFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CleanResourceTextValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CleanResourceTextValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterProjectSelectionPath && args.Count == 1)
		{
			RegisterProjectSelectionPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentProjectPathForShopEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrentProjectPathForShopEditor());
			return true;
		}
		if (method == MethodName.ToProjectRelativeDisplayPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ToProjectRelativeDisplayPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.SelectOptionByText && args.Count == 2)
		{
			SelectOptionByText(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSpinValue && args.Count == 2)
		{
			SetSpinValue(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisplayIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(DisplayIndex(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.EmptyToPlaceholder && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyToPlaceholder(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
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
		if (method == MethodName.CreateShopPreviewAdapter)
		{
			return true;
		}
		if (method == MethodName.HasCompleteShopVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.RenderShopEditor)
		{
			return true;
		}
		if (method == MethodName.BindShopLayout)
		{
			return true;
		}
		if (method == MethodName.ConfigureScopeControls)
		{
			return true;
		}
		if (method == MethodName.SetScopeButton)
		{
			return true;
		}
		if (method == MethodName.BindStageFields)
		{
			return true;
		}
		if (method == MethodName.CreatePurchaseSimulationPanel)
		{
			return true;
		}
		if (method == MethodName.RefreshPurchaseSimulation)
		{
			return true;
		}
		if (method == MethodName.SelectShopPage)
		{
			return true;
		}
		if (method == MethodName.SelectShopItem)
		{
			return true;
		}
		if (method == MethodName.SelectShopStage)
		{
			return true;
		}
		if (method == MethodName.AddShopPage)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedShopPage)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedShopPage)
		{
			return true;
		}
		if (method == MethodName.AddShopItem)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedShopItem)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedShopItem)
		{
			return true;
		}
		if (method == MethodName.AddShopStage)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedShopStage)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedShopStage)
		{
			return true;
		}
		if (method == MethodName.SetItemType)
		{
			return true;
		}
		if (method == MethodName.BeginItemTypeEdit)
		{
			return true;
		}
		if (method == MethodName.PreviewItemType)
		{
			return true;
		}
		if (method == MethodName.SetStageField)
		{
			return true;
		}
		if (method == MethodName.BeginStageFieldEdit)
		{
			return true;
		}
		if (method == MethodName.PreviewStageField)
		{
			return true;
		}
		if (method == MethodName.SetPacketStageFields)
		{
			return true;
		}
		if (method == MethodName.EnsureShopOptionSelectorWindow)
		{
			return true;
		}
		if (method == MethodName.RefreshResourceMetadata)
		{
			return true;
		}
		if (method == MethodName.OpenJsonSourceDialog)
		{
			return true;
		}
		if (method == MethodName.ApplyJsonSource)
		{
			return true;
		}
		if (method == MethodName.ConfirmClearJsonSource)
		{
			return true;
		}
		if (method == MethodName.ClearJsonSource)
		{
			return true;
		}
		if (method == MethodName.SetShopDataWithPageSnapshot)
		{
			return true;
		}
		if (method == MethodName.RefreshDataStatus)
		{
			return true;
		}
		if (method == MethodName.RefreshAll)
		{
			return true;
		}
		if (method == MethodName.RefreshPageList)
		{
			return true;
		}
		if (method == MethodName.RefreshItemList)
		{
			return true;
		}
		if (method == MethodName.RefreshStageList)
		{
			return true;
		}
		if (method == MethodName.RefreshStageControls)
		{
			return true;
		}
		if (method == MethodName.DisposeStageVisualChoices)
		{
			return true;
		}
		if (method == MethodName.NormalizeSelection)
		{
			return true;
		}
		if (method == MethodName.NormalizeIndex)
		{
			return true;
		}
		if (method == MethodName.GetSelectedPage)
		{
			return true;
		}
		if (method == MethodName.GetSelectedItem)
		{
			return true;
		}
		if (method == MethodName.GetSelectedStage)
		{
			return true;
		}
		if (method == MethodName.CopyPages)
		{
			return true;
		}
		if (method == MethodName.CopyItems)
		{
			return true;
		}
		if (method == MethodName.CopyStages)
		{
			return true;
		}
		if (method == MethodName.OnShopPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshLiveEditPreview)
		{
			return true;
		}
		if (method == MethodName.CaptureSelectionIdentity)
		{
			return true;
		}
		if (method == MethodName.RestoreSelectionIdentity)
		{
			return true;
		}
		if (method == MethodName.RefreshShopEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
		{
			return true;
		}
		if (method == MethodName.BuildSummary)
		{
			return true;
		}
		if (method == MethodName.ReadSaveKeyFromResourceFile)
		{
			return true;
		}
		if (method == MethodName.CleanResourceTextValue)
		{
			return true;
		}
		if (method == MethodName.RegisterProjectSelectionPath)
		{
			return true;
		}
		if (method == MethodName.GetCurrentProjectPathForShopEditor)
		{
			return true;
		}
		if (method == MethodName.ToProjectRelativeDisplayPath)
		{
			return true;
		}
		if (method == MethodName.SelectOptionByText)
		{
			return true;
		}
		if (method == MethodName.SetSpinValue)
		{
			return true;
		}
		if (method == MethodName.DisplayIndex)
		{
			return true;
		}
		if (method == MethodName.EmptyToPlaceholder)
		{
			return true;
		}
		if (method == MethodName.FormatResource)
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
		if (name == PropertyName._editingShop)
		{
			_editingShop = VariantUtils.ConvertTo<ShopConfig>(in value);
			return true;
		}
		if (name == PropertyName._editScope)
		{
			_editScope = VariantUtils.ConvertTo<ShopEditScope>(in value);
			return true;
		}
		if (name == PropertyName._runtimeShopPreview)
		{
			_runtimeShopPreview = VariantUtils.ConvertTo<Shop>(in value);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			_summaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pageSummaryLabel)
		{
			_pageSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._itemSummaryLabel)
		{
			_itemSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._stageSummaryLabel)
		{
			_stageSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pageList)
		{
			_pageList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._itemList)
		{
			_itemList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._stageList)
		{
			_stageList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._stageTexturePreview)
		{
			_stageTexturePreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._stagePreviewTitle)
		{
			_stagePreviewTitle = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._stagePreviewMeta)
		{
			_stagePreviewMeta = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._stagePreviewStock)
		{
			_stagePreviewStock = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._purchaseBalanceSpin)
		{
			_purchaseBalanceSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._purchaseOwnedSpin)
		{
			_purchaseOwnedSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._purchaseQuantitySpin)
		{
			_purchaseQuantitySpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._purchaseStockProgress)
		{
			_purchaseStockProgress = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._purchasePreviewResult)
		{
			_purchasePreviewResult = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._itemTypeEdit)
		{
			_itemTypeEdit = VariantUtils.ConvertTo<LineEdit>(in value);
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
		if (name == PropertyName._jsonSourceButton)
		{
			_jsonSourceButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._jsonSourceDialog)
		{
			_jsonSourceDialog = VariantUtils.ConvertTo<FileDialog>(in value);
			return true;
		}
		if (name == PropertyName._dataStatus)
		{
			_dataStatus = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._clearJsonButton)
		{
			_clearJsonButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._clearJsonConfirmationDialog)
		{
			_clearJsonConfirmationDialog = VariantUtils.ConvertTo<ConfirmationDialog>(in value);
			return true;
		}
		if (name == PropertyName._stageSaveTypeOption)
		{
			_stageSaveTypeOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._stageSaveKeyEdit)
		{
			_stageSaveKeyEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._stageTexturePicker)
		{
			_stageTexturePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._stageDescribeEdit)
		{
			_stageDescribeEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._stageNpcTalkEdit)
		{
			_stageNpcTalkEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._stageCostTypeOption)
		{
			_stageCostTypeOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._stageCostSpin)
		{
			_stageCostSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._stageOpenMinSpin)
		{
			_stageOpenMinSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._stageOpenMaxSpin)
		{
			_stageOpenMaxSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._stageAddNumSpin)
		{
			_stageAddNumSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._selectedPageIndex)
		{
			_selectedPageIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedItemIndex)
		{
			_selectedItemIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedStageIndex)
		{
			_selectedStageIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedPageIdentity)
		{
			_selectedPageIdentity = VariantUtils.ConvertTo<ShopPageConfig>(in value);
			return true;
		}
		if (name == PropertyName._selectedItemIdentity)
		{
			_selectedItemIdentity = VariantUtils.ConvertTo<ShopItemConfig>(in value);
			return true;
		}
		if (name == PropertyName._selectedStageIdentity)
		{
			_selectedStageIdentity = VariantUtils.ConvertTo<ShopItemStageConfig>(in value);
			return true;
		}
		if (name == PropertyName._activeItemTypeEditTarget)
		{
			_activeItemTypeEditTarget = VariantUtils.ConvertTo<ShopItemConfig>(in value);
			return true;
		}
		if (name == PropertyName._activeStageEditTarget)
		{
			_activeStageEditTarget = VariantUtils.ConvertTo<ShopItemStageConfig>(in value);
			return true;
		}
		if (name == PropertyName._activeStageEditProperty)
		{
			_activeStageEditProperty = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._shopOptionSelectorWindow)
		{
			_shopOptionSelectorWindow = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editingShop)
		{
			value = VariantUtils.CreateFrom(in _editingShop);
			return true;
		}
		if (name == PropertyName._editScope)
		{
			value = VariantUtils.CreateFrom(in _editScope);
			return true;
		}
		if (name == PropertyName._runtimeShopPreview)
		{
			value = VariantUtils.CreateFrom(in _runtimeShopPreview);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			value = VariantUtils.CreateFrom(in _summaryLabel);
			return true;
		}
		if (name == PropertyName._pageSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _pageSummaryLabel);
			return true;
		}
		if (name == PropertyName._itemSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _itemSummaryLabel);
			return true;
		}
		if (name == PropertyName._stageSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _stageSummaryLabel);
			return true;
		}
		if (name == PropertyName._pageList)
		{
			value = VariantUtils.CreateFrom(in _pageList);
			return true;
		}
		if (name == PropertyName._itemList)
		{
			value = VariantUtils.CreateFrom(in _itemList);
			return true;
		}
		if (name == PropertyName._stageList)
		{
			value = VariantUtils.CreateFrom(in _stageList);
			return true;
		}
		if (name == PropertyName._stageTexturePreview)
		{
			value = VariantUtils.CreateFrom(in _stageTexturePreview);
			return true;
		}
		if (name == PropertyName._stagePreviewTitle)
		{
			value = VariantUtils.CreateFrom(in _stagePreviewTitle);
			return true;
		}
		if (name == PropertyName._stagePreviewMeta)
		{
			value = VariantUtils.CreateFrom(in _stagePreviewMeta);
			return true;
		}
		if (name == PropertyName._stagePreviewStock)
		{
			value = VariantUtils.CreateFrom(in _stagePreviewStock);
			return true;
		}
		if (name == PropertyName._purchaseBalanceSpin)
		{
			value = VariantUtils.CreateFrom(in _purchaseBalanceSpin);
			return true;
		}
		if (name == PropertyName._purchaseOwnedSpin)
		{
			value = VariantUtils.CreateFrom(in _purchaseOwnedSpin);
			return true;
		}
		if (name == PropertyName._purchaseQuantitySpin)
		{
			value = VariantUtils.CreateFrom(in _purchaseQuantitySpin);
			return true;
		}
		if (name == PropertyName._purchaseStockProgress)
		{
			value = VariantUtils.CreateFrom(in _purchaseStockProgress);
			return true;
		}
		if (name == PropertyName._purchasePreviewResult)
		{
			value = VariantUtils.CreateFrom(in _purchasePreviewResult);
			return true;
		}
		if (name == PropertyName._itemTypeEdit)
		{
			value = VariantUtils.CreateFrom(in _itemTypeEdit);
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
		if (name == PropertyName._jsonSourceButton)
		{
			value = VariantUtils.CreateFrom(in _jsonSourceButton);
			return true;
		}
		if (name == PropertyName._jsonSourceDialog)
		{
			value = VariantUtils.CreateFrom(in _jsonSourceDialog);
			return true;
		}
		if (name == PropertyName._dataStatus)
		{
			value = VariantUtils.CreateFrom(in _dataStatus);
			return true;
		}
		if (name == PropertyName._clearJsonButton)
		{
			value = VariantUtils.CreateFrom(in _clearJsonButton);
			return true;
		}
		if (name == PropertyName._clearJsonConfirmationDialog)
		{
			value = VariantUtils.CreateFrom(in _clearJsonConfirmationDialog);
			return true;
		}
		if (name == PropertyName._stageSaveTypeOption)
		{
			value = VariantUtils.CreateFrom(in _stageSaveTypeOption);
			return true;
		}
		if (name == PropertyName._stageSaveKeyEdit)
		{
			value = VariantUtils.CreateFrom(in _stageSaveKeyEdit);
			return true;
		}
		if (name == PropertyName._stageTexturePicker)
		{
			value = VariantUtils.CreateFrom(in _stageTexturePicker);
			return true;
		}
		if (name == PropertyName._stageDescribeEdit)
		{
			value = VariantUtils.CreateFrom(in _stageDescribeEdit);
			return true;
		}
		if (name == PropertyName._stageNpcTalkEdit)
		{
			value = VariantUtils.CreateFrom(in _stageNpcTalkEdit);
			return true;
		}
		if (name == PropertyName._stageCostTypeOption)
		{
			value = VariantUtils.CreateFrom(in _stageCostTypeOption);
			return true;
		}
		if (name == PropertyName._stageCostSpin)
		{
			value = VariantUtils.CreateFrom(in _stageCostSpin);
			return true;
		}
		if (name == PropertyName._stageOpenMinSpin)
		{
			value = VariantUtils.CreateFrom(in _stageOpenMinSpin);
			return true;
		}
		if (name == PropertyName._stageOpenMaxSpin)
		{
			value = VariantUtils.CreateFrom(in _stageOpenMaxSpin);
			return true;
		}
		if (name == PropertyName._stageAddNumSpin)
		{
			value = VariantUtils.CreateFrom(in _stageAddNumSpin);
			return true;
		}
		if (name == PropertyName._selectedPageIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedPageIndex);
			return true;
		}
		if (name == PropertyName._selectedItemIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedItemIndex);
			return true;
		}
		if (name == PropertyName._selectedStageIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedStageIndex);
			return true;
		}
		if (name == PropertyName._selectedPageIdentity)
		{
			value = VariantUtils.CreateFrom(in _selectedPageIdentity);
			return true;
		}
		if (name == PropertyName._selectedItemIdentity)
		{
			value = VariantUtils.CreateFrom(in _selectedItemIdentity);
			return true;
		}
		if (name == PropertyName._selectedStageIdentity)
		{
			value = VariantUtils.CreateFrom(in _selectedStageIdentity);
			return true;
		}
		if (name == PropertyName._activeItemTypeEditTarget)
		{
			value = VariantUtils.CreateFrom(in _activeItemTypeEditTarget);
			return true;
		}
		if (name == PropertyName._activeStageEditTarget)
		{
			value = VariantUtils.CreateFrom(in _activeStageEditTarget);
			return true;
		}
		if (name == PropertyName._activeStageEditProperty)
		{
			value = VariantUtils.CreateFrom(in _activeStageEditProperty);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._shopOptionSelectorWindow)
		{
			value = VariantUtils.CreateFrom(in _shopOptionSelectorWindow);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingShop, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._editScope, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimeShopPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._summaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pageSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._itemSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pageList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._itemList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageTexturePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stagePreviewTitle, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stagePreviewMeta, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stagePreviewStock, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._purchaseBalanceSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._purchaseOwnedSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._purchaseQuantitySpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._purchaseStockProgress, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._purchasePreviewResult, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._itemTypeEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceNameEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToSceneCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._jsonSourceButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._jsonSourceDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._dataStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._clearJsonButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._clearJsonConfirmationDialog, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageSaveTypeOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageSaveKeyEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageTexturePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageDescribeEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageNpcTalkEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageCostTypeOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageCostSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageOpenMinSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageOpenMaxSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageAddNumSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedPageIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedItemIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedStageIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectedPageIdentity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectedItemIdentity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectedStageIdentity, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._activeItemTypeEditTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._activeStageEditTarget, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._activeStageEditProperty, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shopOptionSelectorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingShop, Variant.From(in _editingShop));
		info.AddProperty(PropertyName._editScope, Variant.From(in _editScope));
		info.AddProperty(PropertyName._runtimeShopPreview, Variant.From(in _runtimeShopPreview));
		info.AddProperty(PropertyName._summaryLabel, Variant.From(in _summaryLabel));
		info.AddProperty(PropertyName._pageSummaryLabel, Variant.From(in _pageSummaryLabel));
		info.AddProperty(PropertyName._itemSummaryLabel, Variant.From(in _itemSummaryLabel));
		info.AddProperty(PropertyName._stageSummaryLabel, Variant.From(in _stageSummaryLabel));
		info.AddProperty(PropertyName._pageList, Variant.From(in _pageList));
		info.AddProperty(PropertyName._itemList, Variant.From(in _itemList));
		info.AddProperty(PropertyName._stageList, Variant.From(in _stageList));
		info.AddProperty(PropertyName._stageTexturePreview, Variant.From(in _stageTexturePreview));
		info.AddProperty(PropertyName._stagePreviewTitle, Variant.From(in _stagePreviewTitle));
		info.AddProperty(PropertyName._stagePreviewMeta, Variant.From(in _stagePreviewMeta));
		info.AddProperty(PropertyName._stagePreviewStock, Variant.From(in _stagePreviewStock));
		info.AddProperty(PropertyName._purchaseBalanceSpin, Variant.From(in _purchaseBalanceSpin));
		info.AddProperty(PropertyName._purchaseOwnedSpin, Variant.From(in _purchaseOwnedSpin));
		info.AddProperty(PropertyName._purchaseQuantitySpin, Variant.From(in _purchaseQuantitySpin));
		info.AddProperty(PropertyName._purchaseStockProgress, Variant.From(in _purchaseStockProgress));
		info.AddProperty(PropertyName._purchasePreviewResult, Variant.From(in _purchasePreviewResult));
		info.AddProperty(PropertyName._itemTypeEdit, Variant.From(in _itemTypeEdit));
		info.AddProperty(PropertyName._resourceNameEdit, Variant.From(in _resourceNameEdit));
		info.AddProperty(PropertyName._localToSceneCheck, Variant.From(in _localToSceneCheck));
		info.AddProperty(PropertyName._jsonSourceButton, Variant.From(in _jsonSourceButton));
		info.AddProperty(PropertyName._jsonSourceDialog, Variant.From(in _jsonSourceDialog));
		info.AddProperty(PropertyName._dataStatus, Variant.From(in _dataStatus));
		info.AddProperty(PropertyName._clearJsonButton, Variant.From(in _clearJsonButton));
		info.AddProperty(PropertyName._clearJsonConfirmationDialog, Variant.From(in _clearJsonConfirmationDialog));
		info.AddProperty(PropertyName._stageSaveTypeOption, Variant.From(in _stageSaveTypeOption));
		info.AddProperty(PropertyName._stageSaveKeyEdit, Variant.From(in _stageSaveKeyEdit));
		info.AddProperty(PropertyName._stageTexturePicker, Variant.From(in _stageTexturePicker));
		info.AddProperty(PropertyName._stageDescribeEdit, Variant.From(in _stageDescribeEdit));
		info.AddProperty(PropertyName._stageNpcTalkEdit, Variant.From(in _stageNpcTalkEdit));
		info.AddProperty(PropertyName._stageCostTypeOption, Variant.From(in _stageCostTypeOption));
		info.AddProperty(PropertyName._stageCostSpin, Variant.From(in _stageCostSpin));
		info.AddProperty(PropertyName._stageOpenMinSpin, Variant.From(in _stageOpenMinSpin));
		info.AddProperty(PropertyName._stageOpenMaxSpin, Variant.From(in _stageOpenMaxSpin));
		info.AddProperty(PropertyName._stageAddNumSpin, Variant.From(in _stageAddNumSpin));
		info.AddProperty(PropertyName._selectedPageIndex, Variant.From(in _selectedPageIndex));
		info.AddProperty(PropertyName._selectedItemIndex, Variant.From(in _selectedItemIndex));
		info.AddProperty(PropertyName._selectedStageIndex, Variant.From(in _selectedStageIndex));
		info.AddProperty(PropertyName._selectedPageIdentity, Variant.From(in _selectedPageIdentity));
		info.AddProperty(PropertyName._selectedItemIdentity, Variant.From(in _selectedItemIdentity));
		info.AddProperty(PropertyName._selectedStageIdentity, Variant.From(in _selectedStageIdentity));
		info.AddProperty(PropertyName._activeItemTypeEditTarget, Variant.From(in _activeItemTypeEditTarget));
		info.AddProperty(PropertyName._activeStageEditTarget, Variant.From(in _activeStageEditTarget));
		info.AddProperty(PropertyName._activeStageEditProperty, Variant.From(in _activeStageEditProperty));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._shopOptionSelectorWindow, Variant.From(in _shopOptionSelectorWindow));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingShop, out var value))
		{
			_editingShop = value.As<ShopConfig>();
		}
		if (info.TryGetProperty(PropertyName._editScope, out var value2))
		{
			_editScope = value2.As<ShopEditScope>();
		}
		if (info.TryGetProperty(PropertyName._runtimeShopPreview, out var value3))
		{
			_runtimeShopPreview = value3.As<Shop>();
		}
		if (info.TryGetProperty(PropertyName._summaryLabel, out var value4))
		{
			_summaryLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pageSummaryLabel, out var value5))
		{
			_pageSummaryLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._itemSummaryLabel, out var value6))
		{
			_itemSummaryLabel = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._stageSummaryLabel, out var value7))
		{
			_stageSummaryLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pageList, out var value8))
		{
			_pageList = value8.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._itemList, out var value9))
		{
			_itemList = value9.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._stageList, out var value10))
		{
			_stageList = value10.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._stageTexturePreview, out var value11))
		{
			_stageTexturePreview = value11.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._stagePreviewTitle, out var value12))
		{
			_stagePreviewTitle = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._stagePreviewMeta, out var value13))
		{
			_stagePreviewMeta = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._stagePreviewStock, out var value14))
		{
			_stagePreviewStock = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._purchaseBalanceSpin, out var value15))
		{
			_purchaseBalanceSpin = value15.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._purchaseOwnedSpin, out var value16))
		{
			_purchaseOwnedSpin = value16.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._purchaseQuantitySpin, out var value17))
		{
			_purchaseQuantitySpin = value17.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._purchaseStockProgress, out var value18))
		{
			_purchaseStockProgress = value18.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._purchasePreviewResult, out var value19))
		{
			_purchasePreviewResult = value19.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._itemTypeEdit, out var value20))
		{
			_itemTypeEdit = value20.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._resourceNameEdit, out var value21))
		{
			_resourceNameEdit = value21.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToSceneCheck, out var value22))
		{
			_localToSceneCheck = value22.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._jsonSourceButton, out var value23))
		{
			_jsonSourceButton = value23.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._jsonSourceDialog, out var value24))
		{
			_jsonSourceDialog = value24.As<FileDialog>();
		}
		if (info.TryGetProperty(PropertyName._dataStatus, out var value25))
		{
			_dataStatus = value25.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._clearJsonButton, out var value26))
		{
			_clearJsonButton = value26.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._clearJsonConfirmationDialog, out var value27))
		{
			_clearJsonConfirmationDialog = value27.As<ConfirmationDialog>();
		}
		if (info.TryGetProperty(PropertyName._stageSaveTypeOption, out var value28))
		{
			_stageSaveTypeOption = value28.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._stageSaveKeyEdit, out var value29))
		{
			_stageSaveKeyEdit = value29.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._stageTexturePicker, out var value30))
		{
			_stageTexturePicker = value30.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._stageDescribeEdit, out var value31))
		{
			_stageDescribeEdit = value31.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._stageNpcTalkEdit, out var value32))
		{
			_stageNpcTalkEdit = value32.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._stageCostTypeOption, out var value33))
		{
			_stageCostTypeOption = value33.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._stageCostSpin, out var value34))
		{
			_stageCostSpin = value34.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._stageOpenMinSpin, out var value35))
		{
			_stageOpenMinSpin = value35.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._stageOpenMaxSpin, out var value36))
		{
			_stageOpenMaxSpin = value36.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._stageAddNumSpin, out var value37))
		{
			_stageAddNumSpin = value37.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._selectedPageIndex, out var value38))
		{
			_selectedPageIndex = value38.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedItemIndex, out var value39))
		{
			_selectedItemIndex = value39.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedStageIndex, out var value40))
		{
			_selectedStageIndex = value40.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedPageIdentity, out var value41))
		{
			_selectedPageIdentity = value41.As<ShopPageConfig>();
		}
		if (info.TryGetProperty(PropertyName._selectedItemIdentity, out var value42))
		{
			_selectedItemIdentity = value42.As<ShopItemConfig>();
		}
		if (info.TryGetProperty(PropertyName._selectedStageIdentity, out var value43))
		{
			_selectedStageIdentity = value43.As<ShopItemStageConfig>();
		}
		if (info.TryGetProperty(PropertyName._activeItemTypeEditTarget, out var value44))
		{
			_activeItemTypeEditTarget = value44.As<ShopItemConfig>();
		}
		if (info.TryGetProperty(PropertyName._activeStageEditTarget, out var value45))
		{
			_activeStageEditTarget = value45.As<ShopItemStageConfig>();
		}
		if (info.TryGetProperty(PropertyName._activeStageEditProperty, out var value46))
		{
			_activeStageEditProperty = value46.As<string>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value47))
		{
			_updatingControls = value47.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._shopOptionSelectorWindow, out var value48))
		{
			_shopOptionSelectorWindow = value48.As<XWGameplayResourcePickerWindow>();
		}
	}
}
