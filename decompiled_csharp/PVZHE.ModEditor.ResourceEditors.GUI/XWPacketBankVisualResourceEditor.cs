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

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketBankVisualResourceEditor.cs")]
public class XWPacketBankVisualResourceEditor : XWGenericVisualResourceEditor
{
	private sealed class PacketSelectionOption
	{
		public string Key = "";

		public string SourceLabel = "";

		public string SourcePath = "";

		public string Description = "";

		public string DetailText => $"{Key}\n来源: {SourceLabel}\n路径: {SourcePath}\n说明: {Description}";

		public bool Matches(string query)
		{
			if (string.IsNullOrWhiteSpace(query))
			{
				return true;
			}
			if (!(Key ?? "").Contains(query, StringComparison.OrdinalIgnoreCase) && !(SourceLabel ?? "").Contains(query, StringComparison.OrdinalIgnoreCase) && !(SourcePath ?? "").Contains(query, StringComparison.OrdinalIgnoreCase))
			{
				return (Description ?? "").Contains(query, StringComparison.OrdinalIgnoreCase);
			}
			return true;
		}
	}

	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName HasCompletePacketBankVisualCoverage = "HasCompletePacketBankVisualCoverage";

		public static readonly StringName DisposePacketBankPropertyBinding = "DisposePacketBankPropertyBinding";

		public static readonly StringName RenderPacketBankEditor = "RenderPacketBankEditor";

		public static readonly StringName BindPacketBankLayout = "BindPacketBankLayout";

		public static readonly StringName BindDirectEditControls = "BindDirectEditControls";

		public static readonly StringName OnPacketBankPropertyEdited = "OnPacketBankPropertyEdited";

		public static readonly StringName RefreshResourceFieldsPreview = "RefreshResourceFieldsPreview";

		public static readonly StringName RefreshPacketBankFromHistory = "RefreshPacketBankFromHistory";

		public static readonly StringName DrawRandomPacketFromPool = "DrawRandomPacketFromPool";

		public static readonly StringName FindCategoryIndex = "FindCategoryIndex";

		public static readonly StringName IsPlantPacketCategory = "IsPlantPacketCategory";

		public static readonly StringName AddCategoryEntry = "AddCategoryEntry";

		public static readonly StringName RemoveSelectedCategoryEntry = "RemoveSelectedCategoryEntry";

		public static readonly StringName AddPacketToSelectedCategory = "AddPacketToSelectedCategory";

		public static readonly StringName RemoveSelectedPacketEntry = "RemoveSelectedPacketEntry";

		public static readonly StringName MoveSelectedPacketEntry = "MoveSelectedPacketEntry";

		public static readonly StringName SetCategoryDictionary = "SetCategoryDictionary";

		public static readonly StringName SelectCategory = "SelectCategory";

		public static readonly StringName SelectPacket = "SelectPacket";

		public static readonly StringName OnPacketResourcePicked = "OnPacketResourcePicked";

		public static readonly StringName ShowPacketSelector = "ShowPacketSelector";

		public static readonly StringName EnsurePacketSelectorWindow = "EnsurePacketSelectorWindow";

		public static readonly StringName FilterPacketSelector = "FilterPacketSelector";

		public static readonly StringName ConfirmPacketSelectorSelection = "ConfirmPacketSelectorSelection";

		public static readonly StringName ConfigurePacketSelectorPacketShow = "ConfigurePacketSelectorPacketShow";

		public static readonly StringName ReadPacketKeyFromResourceFile = "ReadPacketKeyFromResourceFile";

		public static readonly StringName CleanResourceTextValue = "CleanResourceTextValue";

		public static readonly StringName RegisterPacketSelectionPath = "RegisterPacketSelectionPath";

		public static readonly StringName GetCurrentProjectPathForPacketBankEditor = "GetCurrentProjectPathForPacketBankEditor";

		public static readonly StringName ToProjectRelativeDisplayPath = "ToProjectRelativeDisplayPath";

		public static readonly StringName RefreshAll = "RefreshAll";

		public static readonly StringName RefreshCategories = "RefreshCategories";

		public static readonly StringName RefreshPacketList = "RefreshPacketList";

		public static readonly StringName RefreshPacketGrid = "RefreshPacketGrid";

		public static readonly StringName CreatePacketCard = "CreatePacketCard";

		public static readonly StringName DuplicateCategoryDictionary = "DuplicateCategoryDictionary";

		public static readonly StringName GetSelectedPacketArray = "GetSelectedPacketArray";

		public static readonly StringName GetCategoryArray = "GetCategoryArray";

		public static readonly StringName EnsureCategorySelected = "EnsureCategorySelected";

		public static readonly StringName GetCategoryAt = "GetCategoryAt";

		public static readonly StringName ReadPacketKey = "ReadPacketKey";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName BuildSummary = "BuildSummary";

		public static readonly StringName CountPackets = "CountPackets";

		public static readonly StringName CreatePanelStyle = "CreatePanelStyle";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";

		public static readonly StringName FormatVariant = "FormatVariant";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingPacketBank = "_editingPacketBank";

		public static readonly StringName _runtimePacketBankPreview = "_runtimePacketBankPreview";

		public static readonly StringName _summaryLabel = "_summaryLabel";

		public static readonly StringName _categorySummaryLabel = "_categorySummaryLabel";

		public static readonly StringName _packetSummaryLabel = "_packetSummaryLabel";

		public static readonly StringName _poolPreviewOption = "_poolPreviewOption";

		public static readonly StringName _poolPreviewResultLabel = "_poolPreviewResultLabel";

		public static readonly StringName _categoryList = "_categoryList";

		public static readonly StringName _packetList = "_packetList";

		public static readonly StringName _packetGrid = "_packetGrid";

		public static readonly StringName _categoryNameEdit = "_categoryNameEdit";

		public static readonly StringName _packetKeyEdit = "_packetKeyEdit";

		public static readonly StringName _resourceName = "_resourceName";

		public static readonly StringName _localToScene = "_localToScene";

		public static readonly StringName _packetPicker = "_packetPicker";

		public static readonly StringName _selectedCategory = "_selectedCategory";

		public static readonly StringName _selectedCategoryIndex = "_selectedCategoryIndex";

		public static readonly StringName _selectedPacketIndex = "_selectedPacketIndex";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _packetSelectorWindow = "_packetSelectorWindow";

		public static readonly StringName _packetSelectorFilter = "_packetSelectorFilter";

		public static readonly StringName _packetSelectorGrid = "_packetSelectorGrid";

		public static readonly StringName _packetSelectorDescription = "_packetSelectorDescription";

		public static readonly StringName _poolPreviewRandom = "_poolPreviewRandom";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string EditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketBankVisualEditorLayout.tscn";

	private const string CompactPacketSelectorRowScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCompactPacketSelectorRow.tscn";

	private const string PacketSelectorScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketBankPacketSelectorWindow.tscn";

	private const string PoolPreviewScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketBankPoolPreview.tscn";

	private static PackedScene _editorLayoutScene;

	private static PackedScene _compactPacketSelectorRowScene;

	private static PackedScene _packetSelectorScene;

	private static PackedScene _poolPreviewScene;

	private static readonly string[] DefaultCategories = new string[9] { "White", "Gold", "Diamond", "Colour", "Star", "Original", "Item", "GraveStone", "Zombie" };

	private TowerDefensePacketBankData _editingPacketBank;

	private LevelEditorPacketBank _runtimePacketBankPreview;

	private Label _summaryLabel;

	private Label _categorySummaryLabel;

	private Label _packetSummaryLabel;

	private OptionButton _poolPreviewOption;

	private XWVisualSegmentedOption _poolPreviewVisualChoices;

	private Label _poolPreviewResultLabel;

	private ItemList _categoryList;

	private ItemList _packetList;

	private GridContainer _packetGrid;

	private LineEdit _categoryNameEdit;

	private LineEdit _packetKeyEdit;

	private LineEdit _resourceName;

	private CheckButton _localToScene;

	private XWResourcePicker _packetPicker;

	private XWVisualPropertyBinding _propertyBinding;

	private string _selectedCategory = "";

	private int _selectedCategoryIndex = -1;

	private int _selectedPacketIndex = -1;

	private bool _updatingControls;

	private Window _packetSelectorWindow;

	private LineEdit _packetSelectorFilter;

	private GridContainer _packetSelectorGrid;

	private Label _packetSelectorDescription;

	private readonly List<PacketSelectionOption> _packetSelectionOptions = new List<PacketSelectionOption>();

	private readonly List<PacketSelectionOption> _filteredPacketSelectionOptions = new List<PacketSelectionOption>();

	private readonly RandomNumberGenerator _poolPreviewRandom = new RandomNumberGenerator();

	private PacketSelectionOption _selectedPacketSelectionOption;

	private const string CharacterResourceRegistryPath = "res://Asset/Config/Character/CharacterResource.json";

	private const string ModCardResourceFolder = "Resources/Cards";

	public override void _ExitTree()
	{
		DisposePacketBankPropertyBinding();
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposePacketBankPropertyBinding();
		if (CurrentResource is TowerDefensePacketBankData towerDefensePacketBankData)
		{
			_editingPacketBank = towerDefensePacketBankData;
			RenderPacketBankEditor(towerDefensePacketBankData);
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!HasCompletePacketBankVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool HasCompletePacketBankVisualCoverage(Resource resource)
	{
		return resource?.GetType() == typeof(TowerDefensePacketBankData);
	}

	private void DisposePacketBankPropertyBinding()
	{
		_poolPreviewVisualChoices?.Dispose();
		_poolPreviewVisualChoices = null;
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		_editingPacketBank = null;
		_runtimePacketBankPreview = null;
		_packetPicker = null;
		_selectedPacketSelectionOption = null;
	}

	private void RenderPacketBankEditor(TowerDefensePacketBankData packetBank)
	{
		if (CanvasGrid != null)
		{
			CanvasGrid.Columns = 1;
			if (_editorLayoutScene == null)
			{
				_editorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketBankVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _editorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindPacketBankLayout(vBoxContainer, packetBank);
				RefreshAll();
				AddSummaryRows(packetBank);
			}
		}
	}

	private void BindPacketBankLayout(VBoxContainer root, TowerDefensePacketBankData packetBank)
	{
		_resourceName = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToScene = root.GetNode<CheckButton>("%LocalToSceneCheck");
		BindDirectEditControls(packetBank);
		_runtimePacketBankPreview = root.GetNode<LevelEditorPacketBank>("%RuntimePacketBank");
		_runtimePacketBankPreview.Init(packetBank);
		_summaryLabel = root.GetNode<Label>("%SummaryLabel");
		_summaryLabel.Text = BuildSummary(packetBank);
		string[] defaultCategories = DefaultCategories;
		foreach (string categoryName in defaultCategories)
		{
			root.GetNode<Button>("%" + categoryName + "Button").Pressed += () =>
			{
				AddCategoryEntry(categoryName);
			};
		}
		if (_poolPreviewScene == null)
		{
			_poolPreviewScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketBankPoolPreview.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		if (GodotObject.IsInstanceValid(_poolPreviewScene))
		{
			Control control = _poolPreviewScene.Instantiate<Control>(PackedScene.GenEditState.Disabled);
			_poolPreviewOption = control.GetNode<OptionButton>("PoolOption");
			_poolPreviewVisualChoices = new XWVisualSegmentedOption(_poolPreviewOption, control.GetNode<HFlowContainer>("%PoolVisualChoices"));
			_poolPreviewVisualChoices.Rebuild();
			_poolPreviewResultLabel = control.GetNode<Label>("Result");
			control.GetNode<Button>("DrawButton").Pressed += DrawRandomPacketFromPool;
			root.GetNode<VBoxContainer>("%PoolPreviewHost").AddChild(control, forceReadableName: false, InternalMode.Disabled);
		}
		_poolPreviewRandom.Randomize();
		_categorySummaryLabel = root.GetNode<Label>("%CategorySummaryLabel");
		_categoryNameEdit = root.GetNode<LineEdit>("%CategoryNameLineEdit");
		root.GetNode<Button>("%AddCategoryButton").Pressed += AddCategoryEntry;
		root.GetNode<Button>("%RemoveCategoryButton").Pressed += RemoveSelectedCategoryEntry;
		_categoryList = root.GetNode<ItemList>("%CategoryList");
		_categoryList.ItemSelected += (long index) =>
		{
			SelectCategory((int)index);
		};
		_packetSummaryLabel = root.GetNode<Label>("%PacketSummaryLabel");
		_packetKeyEdit = root.GetNode<LineEdit>("%PacketKeyLineEdit");
		_packetPicker = XWResourcePicker.Create();
		_packetPicker.Name = "PacketConfigPicker";
		_packetPicker.Setup("TowerDefensePacketConfig");
		_packetPicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		_packetPicker.ResourceChanged += OnPacketResourcePicked;
		root.GetNode<HBoxContainer>("%PacketPickerHost").AddChild(_packetPicker, forceReadableName: false, InternalMode.Disabled);
		root.GetNode<Button>("%ChoosePacketButton").Pressed += ShowPacketSelector;
		root.GetNode<Button>("%AddPacketButton").Pressed += AddPacketToSelectedCategory;
		root.GetNode<Button>("%RemovePacketButton").Pressed += RemoveSelectedPacketEntry;
		root.GetNode<Button>("%PacketUpButton").Pressed += () =>
		{
			MoveSelectedPacketEntry(-1);
		};
		root.GetNode<Button>("%PacketDownButton").Pressed += () =>
		{
			MoveSelectedPacketEntry(1);
		};
		_packetList = root.GetNode<ItemList>("%PacketList");
		_packetList.ItemSelected += (long index) =>
		{
			SelectPacket((int)index);
		};
		_packetGrid = root.GetNode<GridContainer>("%PacketGrid");
	}

	private void BindDirectEditControls(TowerDefensePacketBankData packetBank)
	{
		_propertyBinding?.Dispose();
		_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnPacketBankPropertyEdited);
		_propertyBinding.BindText(_resourceName, packetBank, "resource_name", RefreshResourceFieldsPreview, this, "RefreshPacketBankFromHistory");
		_propertyBinding.BindToggle(_localToScene, packetBank, "resource_local_to_scene", RefreshResourceFieldsPreview, this, "RefreshPacketBankFromHistory");
	}

	private void OnPacketBankPropertyEdited(bool committed)
	{
		if (CurrentResource is TowerDefensePacketBankData towerDefensePacketBankData && towerDefensePacketBankData == _editingPacketBank)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
			}
			else
			{
				MarkCurrentResourceDirty();
				towerDefensePacketBankData.EmitChanged();
			}
			if (GodotObject.IsInstanceValid(_runtimePacketBankPreview))
			{
				_runtimePacketBankPreview.Init(_editingPacketBank);
			}
			RefreshAll();
		}
	}

	private void RefreshResourceFieldsPreview()
	{
		if (GodotObject.IsInstanceValid(_summaryLabel) && GodotObject.IsInstanceValid(_editingPacketBank))
		{
			_summaryLabel.Text = BuildSummary(_editingPacketBank);
		}
	}

	public void RefreshPacketBankFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()) && CurrentResource is TowerDefensePacketBankData towerDefensePacketBankData && towerDefensePacketBankData == _editingPacketBank)
		{
			BindDirectEditControls(towerDefensePacketBankData);
			if (GodotObject.IsInstanceValid(_runtimePacketBankPreview))
			{
				_runtimePacketBankPreview.Init(towerDefensePacketBankData);
			}
			RefreshAll();
		}
	}

	private void DrawRandomPacketFromPool()
	{
		if (_editingPacketBank?.category == null)
		{
			return;
		}
		int num = (GodotObject.IsInstanceValid(_poolPreviewOption) ? _poolPreviewOption.Selected : 0);
		List<(string, int, string)> list = new List<(string, int, string)>();
		foreach (Variant key in _editingPacketBank.category.Keys)
		{
			string text = key.AsString();
			if ((num == 1 && !IsPlantPacketCategory(text)) || (num == 2 && text != "Zombie") || (num == 3 && text != _selectedCategory))
			{
				continue;
			}
			Godot.Collections.Array categoryArray = GetCategoryArray(_editingPacketBank.category, text, create: false);
			for (int i = 0; i < categoryArray.Count; i++)
			{
				string text2 = FormatVariant(categoryArray[i]);
				bool flag = num == 1;
				if (flag)
				{
					bool flag2 = ((text2 == "PlantPresentBox" || text2 == "PlantLuckyBlover") ? true : false);
					flag = flag2;
				}
				if (!flag && !string.IsNullOrWhiteSpace(text2))
				{
					list.Add((text, i, text2));
				}
			}
		}
		if (list.Count == 0)
		{
			if (GodotObject.IsInstanceValid(_poolPreviewResultLabel))
			{
				_poolPreviewResultLabel.Text = "当前卡池没有可抽取卡片";
			}
			return;
		}
		(string, int, string) tuple = list[_poolPreviewRandom.RandiRange(0, list.Count - 1)];
		_selectedCategory = tuple.Item1;
		_selectedCategoryIndex = FindCategoryIndex(tuple.Item1);
		_selectedPacketIndex = tuple.Item2;
		RefreshAll();
		if (GodotObject.IsInstanceValid(_poolPreviewResultLabel))
		{
			_poolPreviewResultLabel.Text = $"抽中: {tuple.Item3} · {tuple.Item1} #{tuple.Item2 + 1}";
		}
	}

	private int FindCategoryIndex(string categoryName)
	{
		if (_editingPacketBank?.category == null)
		{
			return -1;
		}
		int num = 0;
		foreach (Variant key in _editingPacketBank.category.Keys)
		{
			if (key.AsString() == categoryName)
			{
				return num;
			}
			num++;
		}
		return -1;
	}

	private static bool IsPlantPacketCategory(string category)
	{
		switch (category)
		{
		case "White":
		case "Gold":
		case "Diamond":
		case "Colour":
		case "Star":
		case "Original":
			return true;
		default:
			return false;
		}
	}

	private void AddCategoryEntry()
	{
		AddCategoryEntry(_categoryNameEdit?.Text ?? "");
	}

	private void AddCategoryEntry(string categoryName)
	{
		if (_editingPacketBank == null)
		{
			return;
		}
		string text = categoryName.StripEdges();
		if (!string.IsNullOrWhiteSpace(text))
		{
			Dictionary dictionary = DuplicateCategoryDictionary();
			if (!dictionary.ContainsKey(text))
			{
				dictionary[text] = new Godot.Collections.Array();
			}
			_selectedCategory = text;
			SetCategoryDictionary(dictionary, "添加卡牌库分类 " + text);
		}
	}

	private void RemoveSelectedCategoryEntry()
	{
		if (_editingPacketBank != null && !string.IsNullOrWhiteSpace(_selectedCategory))
		{
			string selectedCategory = _selectedCategory;
			Dictionary dictionary = DuplicateCategoryDictionary();
			dictionary.Remove(selectedCategory);
			_selectedCategory = "";
			_selectedCategoryIndex = -1;
			_selectedPacketIndex = -1;
			SetCategoryDictionary(dictionary, "删除卡牌库分类 " + selectedCategory);
		}
	}

	private void AddPacketToSelectedCategory()
	{
		if (_editingPacketBank == null)
		{
			return;
		}
		EnsureCategorySelected();
		if (!string.IsNullOrWhiteSpace(_selectedCategory))
		{
			string text = _packetKeyEdit?.Text.StripEdges() ?? "";
			if (string.IsNullOrWhiteSpace(text) && GodotObject.IsInstanceValid(_packetPicker?.GetEditedResource()))
			{
				text = ReadPacketKey(_packetPicker.GetEditedResource());
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				Dictionary dictionary = DuplicateCategoryDictionary();
				Godot.Collections.Array categoryArray = GetCategoryArray(dictionary, _selectedCategory, create: true);
				categoryArray.Add(text);
				dictionary[_selectedCategory] = categoryArray;
				_selectedPacketIndex = categoryArray.Count - 1;
				SetCategoryDictionary(dictionary, "向 " + _selectedCategory + " 添加卡牌 " + text);
			}
		}
	}

	private void RemoveSelectedPacketEntry()
	{
		if (_editingPacketBank != null && !string.IsNullOrWhiteSpace(_selectedCategory) && _selectedPacketIndex >= 0)
		{
			Dictionary dictionary = DuplicateCategoryDictionary();
			Godot.Collections.Array categoryArray = GetCategoryArray(dictionary, _selectedCategory, create: false);
			if (_selectedPacketIndex < categoryArray.Count)
			{
				categoryArray.RemoveAt(_selectedPacketIndex);
				dictionary[_selectedCategory] = categoryArray;
				_selectedPacketIndex = Math.Min(_selectedPacketIndex, categoryArray.Count - 1);
				SetCategoryDictionary(dictionary, "从 " + _selectedCategory + " 移除卡牌");
			}
		}
	}

	private void MoveSelectedPacketEntry(int direction)
	{
		if (_editingPacketBank != null && !string.IsNullOrWhiteSpace(_selectedCategory) && _selectedPacketIndex >= 0 && direction != 0)
		{
			Dictionary dictionary = DuplicateCategoryDictionary();
			Godot.Collections.Array categoryArray = GetCategoryArray(dictionary, _selectedCategory, create: false);
			int num = Math.Clamp(_selectedPacketIndex + direction, 0, categoryArray.Count - 1);
			if (num != _selectedPacketIndex)
			{
				Variant item = categoryArray[_selectedPacketIndex];
				categoryArray.RemoveAt(_selectedPacketIndex);
				categoryArray.Insert(num, item);
				dictionary[_selectedCategory] = categoryArray;
				_selectedPacketIndex = num;
				SetCategoryDictionary(dictionary, "调整 " + _selectedCategory + " 卡牌顺序");
			}
		}
	}

	private void SetCategoryDictionary(Dictionary category, string actionName)
	{
		if (_editingPacketBank != null && _propertyBinding != null)
		{
			_propertyBinding.SetValue(_editingPacketBank, "category", category ?? new Dictionary(), actionName, this, "RefreshPacketBankFromHistory");
		}
	}

	private void SelectCategory(int index)
	{
		_selectedCategoryIndex = index;
		_selectedCategory = GetCategoryAt(index);
		_selectedPacketIndex = -1;
		_updatingControls = true;
		try
		{
			if (GodotObject.IsInstanceValid(_categoryNameEdit))
			{
				_categoryNameEdit.Text = _selectedCategory;
			}
		}
		finally
		{
			_updatingControls = false;
		}
		RefreshPacketList();
		RefreshPacketGrid();
		if (GodotObject.IsInstanceValid(_runtimePacketBankPreview) && !string.IsNullOrWhiteSpace(_selectedCategory))
		{
			_runtimePacketBankPreview.CategoryChoose(_selectedCategory, reFresh: true);
		}
	}

	private void SelectPacket(int index)
	{
		_selectedPacketIndex = index;
		Godot.Collections.Array selectedPacketArray = GetSelectedPacketArray();
		if (index < 0 || index >= selectedPacketArray.Count)
		{
			return;
		}
		_updatingControls = true;
		try
		{
			if (GodotObject.IsInstanceValid(_packetKeyEdit))
			{
				_packetKeyEdit.Text = FormatVariant(selectedPacketArray[index]);
			}
		}
		finally
		{
			_updatingControls = false;
		}
		RefreshPacketGrid();
	}

	private void OnPacketResourcePicked(Resource resource)
	{
		if (!_updatingControls && GodotObject.IsInstanceValid(resource))
		{
			string text = ReadPacketKey(resource);
			if (GodotObject.IsInstanceValid(_packetKeyEdit))
			{
				_packetKeyEdit.Text = text;
			}
		}
	}

	private void ShowPacketSelector()
	{
		EnsurePacketSelectorWindow();
		if (GodotObject.IsInstanceValid(_packetSelectorWindow))
		{
			_packetSelectionOptions.Clear();
			_packetSelectionOptions.AddRange(CollectPacketSelectionOptions());
			if (GodotObject.IsInstanceValid(_packetSelectorFilter))
			{
				_packetSelectorFilter.Text = "";
			}
			FilterPacketSelector();
			_packetSelectorWindow.PopupCenteredClamped(new Vector2I(760, 520), 0.9f);
		}
	}

	private void EnsurePacketSelectorWindow()
	{
		if (GodotObject.IsInstanceValid(_packetSelectorWindow))
		{
			return;
		}
		if (_packetSelectorScene == null)
		{
			_packetSelectorScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketBankPacketSelectorWindow.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		_packetSelectorWindow = _packetSelectorScene?.Instantiate<Window>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(_packetSelectorWindow))
		{
			_packetSelectorWindow.CloseRequested += () =>
			{
				_packetSelectorWindow.Hide();
			};
			AddChild(_packetSelectorWindow, forceReadableName: false, InternalMode.Disabled);
			_packetSelectorFilter = _packetSelectorWindow.GetNode<LineEdit>("Layout/Filter");
			_packetSelectorFilter.TextChanged += (string _) =>
			{
				FilterPacketSelector();
			};
			_packetSelectorGrid = _packetSelectorWindow.GetNode<GridContainer>("Layout/PacketScroll/PacketGrid");
			_packetSelectorDescription = _packetSelectorWindow.GetNode<Label>("Layout/Description");
			_packetSelectorWindow.GetNode<Button>("Layout/Controls/AddButton").Pressed += ConfirmPacketSelectorSelection;
			_packetSelectorWindow.GetNode<Button>("Layout/Controls/CancelButton").Pressed += () =>
			{
				_packetSelectorWindow.Hide();
			};
		}
	}

	private void FilterPacketSelector()
	{
		if (!GodotObject.IsInstanceValid(_packetSelectorGrid))
		{
			return;
		}
		foreach (Node child in _packetSelectorGrid.GetChildren())
		{
			child.QueueFree();
		}
		_filteredPacketSelectionOptions.Clear();
		_selectedPacketSelectionOption = null;
		string query = _packetSelectorFilter?.Text?.StripEdges() ?? "";
		foreach (PacketSelectionOption packetSelectionOption in _packetSelectionOptions)
		{
			if (packetSelectionOption.Matches(query))
			{
				_filteredPacketSelectionOptions.Add(packetSelectionOption);
				_packetSelectorGrid.AddChild(CreatePacketSelectorItem(packetSelectionOption), forceReadableName: false, InternalMode.Disabled);
			}
		}
		if (_filteredPacketSelectionOptions.Count == 0)
		{
			_packetSelectorGrid.AddChild(new Label
			{
				Text = "没有可用卡片",
				CustomMinimumSize = new Vector2(240f, 64f)
			}, forceReadableName: false, InternalMode.Disabled);
			UpdatePacketSelectorDescription(null);
		}
		else
		{
			SelectPacketSelectorOption(_filteredPacketSelectionOptions[0]);
		}
	}

	private void UpdatePacketSelectorDescription(PacketSelectionOption option)
	{
		if (GodotObject.IsInstanceValid(_packetSelectorDescription))
		{
			_packetSelectorDescription.Text = ((option != null) ? option.DetailText : "没有选择卡片");
		}
	}

	private void ConfirmPacketSelectorSelection()
	{
		ConfirmPacketSelectorSelection(_selectedPacketSelectionOption ?? ((_filteredPacketSelectionOptions.Count > 0) ? _filteredPacketSelectionOptions[0] : null));
	}

	private void ConfirmPacketSelectorSelection(PacketSelectionOption option)
	{
		if (option != null)
		{
			AddSelectedPacketOption(option);
			if (GodotObject.IsInstanceValid(_packetSelectorWindow))
			{
				_packetSelectorWindow.Hide();
			}
		}
	}

	private Control CreatePacketSelectorItem(PacketSelectionOption option)
	{
		if (_compactPacketSelectorRowScene == null)
		{
			_compactPacketSelectorRowScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCompactPacketSelectorRow.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		Control control = _compactPacketSelectorRowScene?.Instantiate<Control>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(control))
		{
			throw new InvalidOperationException("Cannot instantiate selector row: res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCompactPacketSelectorRow.tscn");
		}
		control.Name = "PacketSelectorItem";
		Button node = control.GetNode<Button>("%SelectSurface");
		node.ToggleMode = false;
		node.Pressed += () =>
		{
			ConfirmPacketSelectorSelection(option);
		};
		node.MouseEntered += () =>
		{
			SelectPacketSelectorOption(option);
		};
		control.GetNode<Label>("%PacketName").Text = option.Key;
		control.GetNode<Label>("%PacketCost").Text = option.SourceLabel;
		Label node2 = control.GetNode<Label>("%LoadingBadge");
		node2.Text = "卡";
		node2.Visible = true;
		Control node3 = control.GetNode<Control>("%PreviewHost");
		TowerDefensePacketConfig towerDefensePacketConfig = LoadPacketConfigForSelector(option);
		if (GodotObject.IsInstanceValid(towerDefensePacketConfig))
		{
			TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = null;
			try
			{
				towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
				if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
				{
					towerDefenseInGamePacketShow.Name = "PacketSelectorPacketShow";
					towerDefenseInGamePacketShow.Position = new Vector2(16f, 18f);
					towerDefenseInGamePacketShow.Scale = new Vector2(0.23f, 0.23f);
					towerDefenseInGamePacketShow.MouseFilter = MouseFilterEnum.Ignore;
					node3.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
					ConfigurePacketSelectorPacketShow(towerDefenseInGamePacketShow, towerDefensePacketConfig);
					node2.Visible = false;
				}
			}
			catch (Exception ex)
			{
				if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
				{
					towerDefenseInGamePacketShow.QueueFree();
				}
				GD.PushWarning("PacketBank selector PacketShow failed: " + option.Key + " " + ex.Message);
			}
		}
		return control;
	}

	private static Label CreatePacketSelectorFallbackLabel(PacketSelectionOption option)
	{
		Label label = new Label();
		label.Text = option?.Key ?? "Packet";
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.VerticalAlignment = VerticalAlignment.Center;
		label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
		label.MouseFilter = MouseFilterEnum.Ignore;
		label.SetAnchorsPreset(LayoutPreset.FullRect);
		return label;
	}

	private void SelectPacketSelectorOption(PacketSelectionOption option)
	{
		_selectedPacketSelectionOption = option;
		UpdatePacketSelectorDescription(option);
	}

	private static TowerDefensePacketConfig LoadPacketConfigForSelector(PacketSelectionOption option)
	{
		if (option == null || string.IsNullOrWhiteSpace(option.Key))
		{
			return null;
		}
		if (!string.IsNullOrWhiteSpace(option.SourcePath))
		{
			string text = Path.GetExtension(option.SourcePath).ToLowerInvariant();
			if ((text == ".tres" || text == ".res") ? true : false)
			{
				try
				{
					TowerDefensePacketConfig towerDefensePacketConfig = ResourceLoader.Load<TowerDefensePacketConfig>(option.SourcePath, null, ResourceLoader.CacheMode.Reuse);
					if (GodotObject.IsInstanceValid(towerDefensePacketConfig))
					{
						return towerDefensePacketConfig;
					}
				}
				catch (Exception ex)
				{
					GD.PushWarning("PacketBank selector resource load failed: " + option.SourcePath + " " + ex.Message);
				}
			}
		}
		try
		{
			TowerDefensePacketConfig packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(option.Key);
			if (GodotObject.IsInstanceValid(packetConfigReadOnly))
			{
				return packetConfigReadOnly;
			}
		}
		catch
		{
		}
		try
		{
			TowerDefensePacketConfig packetConfigReadOnlyByCharacterName = TowerDefenseManager.GetPacketConfigReadOnlyByCharacterName(option.Key);
			if (GodotObject.IsInstanceValid(packetConfigReadOnlyByCharacterName))
			{
				return packetConfigReadOnlyByCharacterName;
			}
		}
		catch
		{
		}
		return null;
	}

	private static void ConfigurePacketSelectorPacketShow(TowerDefenseInGamePacketShow packetShow, TowerDefensePacketConfig packet)
	{
		if (GodotObject.IsInstanceValid(packetShow) && GodotObject.IsInstanceValid(packet))
		{
			packetShow.onlyDraw = true;
			packetShow.setPcLayout = true;
			packetShow.showLove = false;
			packetShow.select = false;
			packetShow.alive = true;
			packetShow.@lock = false;
			packetShow.MouseFilter = MouseFilterEnum.Ignore;
			packetShow.Init(packet);
			packetShow.onlyDraw = true;
			packetShow.RefreshPreview();
		}
	}

	private void AddSelectedPacketOption(PacketSelectionOption option)
	{
		if (_editingPacketBank != null && option != null && !string.IsNullOrWhiteSpace(option.Key))
		{
			EnsureCategorySelected();
			if (string.IsNullOrWhiteSpace(_selectedCategory))
			{
				_selectedCategory = ((DefaultCategories.Length != 0) ? DefaultCategories[0] : "Default");
			}
			if (GodotObject.IsInstanceValid(_packetKeyEdit))
			{
				_packetKeyEdit.Text = option.Key;
			}
			Dictionary dictionary = DuplicateCategoryDictionary();
			Godot.Collections.Array categoryArray = GetCategoryArray(dictionary, _selectedCategory, create: true);
			categoryArray.Add(option.Key);
			dictionary[_selectedCategory] = categoryArray;
			_selectedPacketIndex = categoryArray.Count - 1;
			RegisterPacketSelectionPath(option.SourcePath);
			SetCategoryDictionary(dictionary, "向 " + _selectedCategory + " 添加卡牌 " + option.Key);
			XWEditorInterface.Instance?.AddOutputMessage("卡牌库: 添加卡片 " + option.Key);
		}
	}

	private List<PacketSelectionOption> CollectPacketSelectionOptions()
	{
		List<PacketSelectionOption> list = new List<PacketSelectionOption>();
		LoadRegisteredPacketKeys(list);
		CollectModCardOptions(list);
		list.Sort((PacketSelectionOption a, PacketSelectionOption b) => string.Compare(a?.Key, b?.Key, StringComparison.OrdinalIgnoreCase));
		return list;
	}

	private static void LoadRegisteredPacketKeys(List<PacketSelectionOption> options)
	{
		if (options == null || !ResourceLoader.Exists("res://Asset/Config/Character/CharacterResource.json"))
		{
			return;
		}
		try
		{
			Json json = ResourceLoader.Load<Json>("res://Asset/Config/Character/CharacterResource.json", null, ResourceLoader.CacheMode.Reuse);
			if (!GodotObject.IsInstanceValid(json) || json.Data.VariantType != Variant.Type.Dictionary)
			{
				return;
			}
			Dictionary dictionary = json.Data.AsGodotDictionary();
			foreach (Variant key in dictionary.Keys)
			{
				AddPacketSelectionOption(options, key.AsString(), "游戏内卡片", "res://Asset/Config/Character/CharacterResource.json", "内置 Character 注册表");
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
					AddPacketSelectionOption(options, key2.AsString(), "游戏内卡片", "res://Asset/Config/Character/CharacterResource.json", "Packet of " + key.AsString());
				}
			}
		}
		catch (Exception ex)
		{
			GD.PushWarning("PacketBank selector registry load failed: res://Asset/Config/Character/CharacterResource.json " + ex.Message);
		}
	}

	private static void CollectModCardOptions(List<PacketSelectionOption> options)
	{
		string currentProjectPathForPacketBankEditor = GetCurrentProjectPathForPacketBankEditor();
		if (options == null || string.IsNullOrWhiteSpace(currentProjectPathForPacketBankEditor))
		{
			return;
		}
		string path = Path.Combine(currentProjectPathForPacketBankEditor, "Resources/Cards".Replace('/', Path.DirectorySeparatorChar));
		if (!Directory.Exists(path))
		{
			return;
		}
		foreach (string item in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
		{
			string text = Path.GetExtension(item).ToLowerInvariant();
			if (text == ".tres" || text == ".res")
			{
				string text2 = ReadPacketKeyFromResourceFile(item);
				if (string.IsNullOrWhiteSpace(text2))
				{
					text2 = Path.GetFileNameWithoutExtension(item);
				}
				AddPacketSelectionOption(options, text2, "Mod 卡片", item, ToProjectRelativeDisplayPath(item, currentProjectPathForPacketBankEditor));
			}
		}
	}

	private static string ReadPacketKeyFromResourceFile(string filePath)
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
			GD.PushWarning("PacketBank selector card scan failed: " + filePath + " " + ex.Message);
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

	private static void AddPacketSelectionOption(List<PacketSelectionOption> options, string key, string sourceLabel, string sourcePath, string description)
	{
		if (options == null || string.IsNullOrWhiteSpace(key))
		{
			return;
		}
		string text = key.StripEdges();
		foreach (PacketSelectionOption option in options)
		{
			if (string.Equals(option.Key, text, StringComparison.OrdinalIgnoreCase))
			{
				MergePacketSelectionText(ref option.SourceLabel, sourceLabel);
				if (!string.IsNullOrWhiteSpace(sourcePath))
				{
					option.SourcePath = sourcePath;
				}
				MergePacketSelectionText(ref option.Description, description);
				return;
			}
		}
		options.Add(new PacketSelectionOption
		{
			Key = text,
			SourceLabel = (sourceLabel ?? ""),
			SourcePath = (sourcePath ?? ""),
			Description = (description ?? "")
		});
	}

	private static void MergePacketSelectionText(ref string target, string value)
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

	private static void RegisterPacketSelectionPath(string sourcePath)
	{
		string currentProjectPathForPacketBankEditor = GetCurrentProjectPathForPacketBankEditor();
		if (!string.IsNullOrWhiteSpace(currentProjectPathForPacketBankEditor) && !string.IsNullOrWhiteSpace(sourcePath))
		{
			XWModManifestSyncService.RegisterPath(currentProjectPathForPacketBankEditor, sourcePath);
		}
	}

	private static string GetCurrentProjectPathForPacketBankEditor()
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

	private void RefreshAll()
	{
		if (_editingPacketBank == null)
		{
			return;
		}
		_updatingControls = true;
		try
		{
			if (GodotObject.IsInstanceValid(_summaryLabel))
			{
				_summaryLabel.Text = BuildSummary(_editingPacketBank);
			}
			RefreshCategories();
			RefreshPacketList();
			RefreshPacketGrid();
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void RefreshCategories()
	{
		if (!GodotObject.IsInstanceValid(_categoryList) || _editingPacketBank == null)
		{
			return;
		}
		_categoryList.Clear();
		Dictionary dictionary = _editingPacketBank.category ?? new Dictionary();
		int num = 0;
		int num2 = -1;
		foreach (Variant key in dictionary.Keys)
		{
			string text = key.AsString();
			Godot.Collections.Array categoryArray = GetCategoryArray(dictionary, text, create: false);
			_categoryList.AddItem($"{text} ({categoryArray.Count})");
			if (text == _selectedCategory)
			{
				num2 = num;
			}
			num++;
		}
		if (dictionary.Count == 0)
		{
			_categoryList.AddItem("未配置分类");
			_selectedCategory = "";
			_selectedCategoryIndex = -1;
		}
		else
		{
			if (num2 < 0)
			{
				SelectCategory(0);
				return;
			}
			_categoryList.Select(num2);
			_selectedCategoryIndex = num2;
		}
		if (GodotObject.IsInstanceValid(_categorySummaryLabel))
		{
			_categorySummaryLabel.Text = $"分类数量: {dictionary.Count}";
		}
	}

	private void RefreshPacketList()
	{
		if (!GodotObject.IsInstanceValid(_packetList))
		{
			return;
		}
		_packetList.Clear();
		Godot.Collections.Array selectedPacketArray = GetSelectedPacketArray();
		if (selectedPacketArray.Count == 0)
		{
			_packetList.AddItem(string.IsNullOrWhiteSpace(_selectedCategory) ? "未选择分类" : "当前分类没有卡片");
		}
		else
		{
			for (int i = 0; i < selectedPacketArray.Count; i++)
			{
				_packetList.AddItem($"{i + 1}. {FormatVariant(selectedPacketArray[i])}");
			}
			if (_selectedPacketIndex >= 0 && _selectedPacketIndex < selectedPacketArray.Count)
			{
				_packetList.Select(_selectedPacketIndex);
			}
		}
		if (GodotObject.IsInstanceValid(_packetSummaryLabel))
		{
			_packetSummaryLabel.Text = $"当前分类: {EmptyToPlaceholder(_selectedCategory)}  卡片数量: {selectedPacketArray.Count}";
		}
	}

	private void RefreshPacketGrid()
	{
		if (!GodotObject.IsInstanceValid(_packetGrid))
		{
			return;
		}
		foreach (Node child in _packetGrid.GetChildren())
		{
			_packetGrid.RemoveChild(child);
			child.QueueFree();
		}
		Godot.Collections.Array selectedPacketArray = GetSelectedPacketArray();
		if (selectedPacketArray.Count == 0)
		{
			_packetGrid.AddChild(CreatePacketCard("未配置", "选择分类后添加卡片 saveKey", selected: false, -1), forceReadableName: false, InternalMode.Disabled);
			return;
		}
		for (int i = 0; i < selectedPacketArray.Count; i++)
		{
			_packetGrid.AddChild(CreatePacketCard(FormatVariant(selectedPacketArray[i]), $"#{i + 1}", i == _selectedPacketIndex, i), forceReadableName: false, InternalMode.Disabled);
		}
	}

	private Control CreatePacketCard(string packetKey, string subtitle, bool selected, int packetIndex)
	{
		Button button = new Button
		{
			Name = "PacketPreviewCard",
			Text = "",
			CustomMinimumSize = new Vector2(148f, 188f),
			ClipContents = true,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			FocusMode = FocusModeEnum.None
		};
		button.AddThemeStyleboxOverride("normal", CreatePanelStyle(selected ? new Color(0.18f, 0.3f, 0.46f) : new Color(0.13f, 0.135f, 0.14f), (!selected) ? 1 : 2, 4));
		button.AddThemeStyleboxOverride("hover", CreatePanelStyle(new Color(0.2f, 0.32f, 0.48f), 2, 4));
		button.AddThemeStyleboxOverride("pressed", CreatePanelStyle(new Color(0.14f, 0.25f, 0.39f), 2, 4));
		if (packetIndex >= 0)
		{
			button.Pressed += () =>
			{
				SelectPacket(packetIndex);
			};
		}
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			MouseFilter = MouseFilterEnum.Ignore
		};
		vBoxContainer.SetAnchorsPreset(LayoutPreset.FullRect);
		vBoxContainer.OffsetLeft = 6f;
		vBoxContainer.OffsetTop = 6f;
		vBoxContainer.OffsetRight = -6f;
		vBoxContainer.OffsetBottom = -6f;
		vBoxContainer.AddThemeConstantOverride("separation", 4);
		button.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		Control control = new Control
		{
			Name = "PacketBankCardGamePreview",
			CustomMinimumSize = new Vector2(130f, 140f),
			ClipContents = true,
			MouseFilter = MouseFilterEnum.Ignore
		};
		vBoxContainer.AddChild(control, forceReadableName: false, InternalMode.Disabled);
		TowerDefensePacketConfig towerDefensePacketConfig = LoadPacketConfigForSelector(new PacketSelectionOption
		{
			Key = packetKey
		});
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = null;
		if (GodotObject.IsInstanceValid(towerDefensePacketConfig))
		{
			try
			{
				towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShow();
				if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
				{
					towerDefenseInGamePacketShow.Name = "PacketBankCardPacketShow";
					towerDefenseInGamePacketShow.Position = new Vector2(65f, 73f);
					towerDefenseInGamePacketShow.Scale = new Vector2(0.88f, 0.88f);
					towerDefenseInGamePacketShow.MouseFilter = MouseFilterEnum.Ignore;
					control.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
					ConfigurePacketSelectorPacketShow(towerDefenseInGamePacketShow, towerDefensePacketConfig);
				}
			}
			catch (Exception ex)
			{
				if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
				{
					towerDefenseInGamePacketShow.QueueFree();
				}
				GD.PushWarning("PacketBank card preview failed: " + packetKey + " " + ex.Message);
			}
		}
		if (!GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
		{
			control.AddChild(CreatePacketSelectorFallbackLabel(new PacketSelectionOption
			{
				Key = packetKey
			}), forceReadableName: false, InternalMode.Disabled);
		}
		vBoxContainer.AddChild(new Label
		{
			Text = packetKey,
			HorizontalAlignment = HorizontalAlignment.Center,
			ClipText = true,
			MouseFilter = MouseFilterEnum.Ignore,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(new Label
		{
			Text = (selected ? (subtitle + " · 当前选择") : subtitle),
			HorizontalAlignment = HorizontalAlignment.Center,
			ClipText = true,
			MouseFilter = MouseFilterEnum.Ignore,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		return button;
	}

	private Dictionary DuplicateCategoryDictionary()
	{
		if (_editingPacketBank?.category == null)
		{
			return new Dictionary();
		}
		return _editingPacketBank.category.Duplicate(deep: true);
	}

	private Godot.Collections.Array GetSelectedPacketArray()
	{
		if (_editingPacketBank == null || string.IsNullOrWhiteSpace(_selectedCategory))
		{
			return new Godot.Collections.Array();
		}
		return GetCategoryArray(_editingPacketBank.category, _selectedCategory, create: false);
	}

	private static Godot.Collections.Array GetCategoryArray(Dictionary dict, string categoryName, bool create)
	{
		if (dict == null || string.IsNullOrWhiteSpace(categoryName))
		{
			return new Godot.Collections.Array();
		}
		if (!dict.ContainsKey(categoryName))
		{
			if (!create)
			{
				return new Godot.Collections.Array();
			}
			dict[categoryName] = new Godot.Collections.Array();
		}
		Variant variant = dict[categoryName];
		if (variant.VariantType == Variant.Type.Array)
		{
			return variant.AsGodotArray();
		}
		Godot.Collections.Array array = new Godot.Collections.Array();
		if (create)
		{
			dict[categoryName] = array;
		}
		return array;
	}

	private void EnsureCategorySelected()
	{
		if (string.IsNullOrWhiteSpace(_selectedCategory))
		{
			Dictionary dictionary = _editingPacketBank?.category;
			if (dictionary != null && dictionary.Count != 0)
			{
				_selectedCategory = GetCategoryAt(0);
				_selectedCategoryIndex = (string.IsNullOrWhiteSpace(_selectedCategory) ? (-1) : 0);
			}
		}
	}

	private string GetCategoryAt(int index)
	{
		if (_editingPacketBank?.category == null || index < 0)
		{
			return "";
		}
		int num = 0;
		foreach (Variant key in _editingPacketBank.category.Keys)
		{
			if (num++ == index)
			{
				return key.AsString();
			}
		}
		return "";
	}

	private static string ReadPacketKey(Resource resource)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return "";
		}
		if (resource is TowerDefensePacketConfig towerDefensePacketConfig)
		{
			return towerDefensePacketConfig.saveKey;
		}
		try
		{
			Variant variant = resource.Get("saveKey");
			if (variant.VariantType != Variant.Type.Nil)
			{
				return variant.AsString();
			}
		}
		catch
		{
		}
		if (!string.IsNullOrWhiteSpace(resource.ResourcePath))
		{
			return resource.ResourcePath.GetFile().GetBaseName();
		}
		return resource.ResourceName;
	}

	private void AddSummaryRows(TowerDefensePacketBankData packetBank)
	{
		AddItemIfMissing(PreviewList, BuildSummary(packetBank));
		foreach (Variant key in packetBank.category.Keys)
		{
			string text = key.AsString();
			AddItemIfMissing(PreviewList, $"分类 {text}: {GetCategoryArray(packetBank.category, text, create: false).Count}");
		}
		AddItemIfMissing(TimelineList, $"category -> {packetBank.category.Count}");
		AddItemIfMissing(TimelineList, $"packet total -> {CountPackets(packetBank)}");
		AddItemIfMissing(GraphList, "卡牌库 -> category -> packet saveKey -> 卡片资源");
		AddItemIfMissing(ReferenceList, "卡牌库资源 -> " + CurrentResourcePath);
	}

	private static string BuildSummary(TowerDefensePacketBankData packetBank)
	{
		string value = (string.IsNullOrWhiteSpace(packetBank?.ResourceName) ? "未命名卡牌库" : packetBank.ResourceName);
		string value2 = ((packetBank != null && packetBank.ResourceLocalToScene) ? " · 仅当前场景" : "");
		return $"{value}{value2} · 分类 {(packetBank?.category?.Count).GetValueOrDefault()}, 卡片 {CountPackets(packetBank)}";
	}

	private static int CountPackets(TowerDefensePacketBankData packetBank)
	{
		if (packetBank?.category == null)
		{
			return 0;
		}
		int num = 0;
		foreach (Variant key in packetBank.category.Keys)
		{
			num += GetCategoryArray(packetBank.category, key.AsString(), create: false).Count;
		}
		return num;
	}

	private static StyleBoxFlat CreatePanelStyle(Color color, int borderWidth, int cornerRadius)
	{
		return new StyleBoxFlat
		{
			BgColor = color,
			CornerRadiusTopLeft = cornerRadius,
			CornerRadiusTopRight = cornerRadius,
			CornerRadiusBottomLeft = cornerRadius,
			CornerRadiusBottomRight = cornerRadius,
			ContentMarginLeft = 10f,
			ContentMarginRight = 10f,
			ContentMarginTop = 10f,
			ContentMarginBottom = 10f,
			BorderColor = color.Lightened(0.22f),
			BorderWidthLeft = borderWidth,
			BorderWidthRight = borderWidth,
			BorderWidthTop = borderWidth,
			BorderWidthBottom = borderWidth
		};
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

	private static string FormatVariant(Variant value)
	{
		switch (value.VariantType)
		{
		case Variant.Type.Nil:
			return "空";
		case Variant.Type.String:
		case Variant.Type.StringName:
			return value.AsString();
		case Variant.Type.Object:
			return (value.AsGodotObject() is Resource resource) ? ReadPacketKey(resource) : (value.AsGodotObject()?.GetType().Name ?? "空");
		default:
			return value.ToString();
		}
	}

	private static string EmptyToPlaceholder(string value)
	{
		if (!string.IsNullOrWhiteSpace(value))
		{
			return value;
		}
		return "未配置";
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(50)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCompletePacketBankVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposePacketBankPropertyBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderPacketBankEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetBank", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindPacketBankLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packetBank", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindDirectEditControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetBank", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnPacketBankPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshResourceFieldsPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPacketBankFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawRandomPacketFromPool, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FindCategoryIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "categoryName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.IsPlantPacketCategory, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddCategoryEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddCategoryEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "categoryName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelectedCategoryEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddPacketToSelectedCategory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedPacketEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedPacketEntry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCategoryDictionary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "category", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectCategory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectPacket, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnPacketResourcePicked, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ShowPacketSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsurePacketSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.FilterPacketSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfirmPacketSelectorSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfigurePacketSelectorPacketShow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetShow", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "packet", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadPacketKeyFromResourceFile, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CleanResourceTextValue, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RegisterPacketSelectionPath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "sourcePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurrentProjectPathForPacketBankEditor, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.ToProjectRelativeDisplayPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "filePath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "projectPath", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCategories, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPacketList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPacketGrid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreatePacketCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "subtitle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "packetIndex", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DuplicateCategoryDictionary, new PropertyInfo(Variant.Type.Dictionary, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectedPacketArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCategoryArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Dictionary, "dict", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "categoryName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "create", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCategorySelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetCategoryAt, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadPacketKey, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetBank", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetBank", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CountPackets, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "packetBank", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreatePanelStyle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("StyleBoxFlat"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "borderWidth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "cornerRadius", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddItemIfMissing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false),
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVariant, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.EmptyToPlaceholder, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.HasCompletePacketBankVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompletePacketBankVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.DisposePacketBankPropertyBinding && args.Count == 0)
		{
			DisposePacketBankPropertyBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.RenderPacketBankEditor && args.Count == 1)
		{
			RenderPacketBankEditor(VariantUtils.ConvertTo<TowerDefensePacketBankData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindPacketBankLayout && args.Count == 2)
		{
			BindPacketBankLayout(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketBankData>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindDirectEditControls && args.Count == 1)
		{
			BindDirectEditControls(VariantUtils.ConvertTo<TowerDefensePacketBankData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPacketBankPropertyEdited && args.Count == 1)
		{
			OnPacketBankPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshResourceFieldsPreview && args.Count == 0)
		{
			RefreshResourceFieldsPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketBankFromHistory && args.Count == 0)
		{
			RefreshPacketBankFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawRandomPacketFromPool && args.Count == 0)
		{
			DrawRandomPacketFromPool();
			ret = default;
			return true;
		}
		if (method == MethodName.FindCategoryIndex && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(FindCategoryIndex(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.IsPlantPacketCategory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPlantPacketCategory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.AddCategoryEntry && args.Count == 0)
		{
			AddCategoryEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.AddCategoryEntry && args.Count == 1)
		{
			AddCategoryEntry(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedCategoryEntry && args.Count == 0)
		{
			RemoveSelectedCategoryEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.AddPacketToSelectedCategory && args.Count == 0)
		{
			AddPacketToSelectedCategory();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedPacketEntry && args.Count == 0)
		{
			RemoveSelectedPacketEntry();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedPacketEntry && args.Count == 1)
		{
			MoveSelectedPacketEntry(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCategoryDictionary && args.Count == 2)
		{
			SetCategoryDictionary(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectCategory && args.Count == 1)
		{
			SelectCategory(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SelectPacket && args.Count == 1)
		{
			SelectPacket(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnPacketResourcePicked && args.Count == 1)
		{
			OnPacketResourcePicked(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ShowPacketSelector && args.Count == 0)
		{
			ShowPacketSelector();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsurePacketSelectorWindow && args.Count == 0)
		{
			EnsurePacketSelectorWindow();
			ret = default;
			return true;
		}
		if (method == MethodName.FilterPacketSelector && args.Count == 0)
		{
			FilterPacketSelector();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfirmPacketSelectorSelection && args.Count == 0)
		{
			ConfirmPacketSelectorSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfigurePacketSelectorPacketShow && args.Count == 2)
		{
			ConfigurePacketSelectorPacketShow(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadPacketKeyFromResourceFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadPacketKeyFromResourceFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CleanResourceTextValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CleanResourceTextValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterPacketSelectionPath && args.Count == 1)
		{
			RegisterPacketSelectionPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentProjectPathForPacketBankEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrentProjectPathForPacketBankEditor());
			return true;
		}
		if (method == MethodName.ToProjectRelativeDisplayPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ToProjectRelativeDisplayPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.RefreshAll && args.Count == 0)
		{
			RefreshAll();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCategories && args.Count == 0)
		{
			RefreshCategories();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketList && args.Count == 0)
		{
			RefreshPacketList();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketGrid && args.Count == 0)
		{
			RefreshPacketGrid();
			ret = default;
			return true;
		}
		if (method == MethodName.CreatePacketCard && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Control>(CreatePacketCard(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		if (method == MethodName.DuplicateCategoryDictionary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Dictionary>(DuplicateCategoryDictionary());
			return true;
		}
		if (method == MethodName.GetSelectedPacketArray && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetSelectedPacketArray());
			return true;
		}
		if (method == MethodName.GetCategoryArray && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetCategoryArray(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.EnsureCategorySelected && args.Count == 0)
		{
			EnsureCategorySelected();
			ret = default;
			return true;
		}
		if (method == MethodName.GetCategoryAt && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCategoryAt(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadPacketKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadPacketKey(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 1)
		{
			AddSummaryRows(VariantUtils.ConvertTo<TowerDefensePacketBankData>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSummary(VariantUtils.ConvertTo<TowerDefensePacketBankData>(in args[0])));
			return true;
		}
		if (method == MethodName.CountPackets && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountPackets(VariantUtils.ConvertTo<TowerDefensePacketBankData>(in args[0])));
			return true;
		}
		if (method == MethodName.CreatePanelStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreatePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.EmptyToPlaceholder && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyToPlaceholder(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasCompletePacketBankVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompletePacketBankVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.IsPlantPacketCategory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(IsPlantPacketCategory(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.ConfigurePacketSelectorPacketShow && args.Count == 2)
		{
			ConfigurePacketSelectorPacketShow(VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in args[0]), VariantUtils.ConvertTo<TowerDefensePacketConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadPacketKeyFromResourceFile && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadPacketKeyFromResourceFile(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.CleanResourceTextValue && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(CleanResourceTextValue(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.RegisterPacketSelectionPath && args.Count == 1)
		{
			RegisterPacketSelectionPath(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCurrentProjectPathForPacketBankEditor && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurrentProjectPathForPacketBankEditor());
			return true;
		}
		if (method == MethodName.ToProjectRelativeDisplayPath && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ToProjectRelativeDisplayPath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.GetCategoryArray && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<Godot.Collections.Array>(GetCategoryArray(VariantUtils.ConvertTo<Dictionary>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadPacketKey && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(ReadPacketKey(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSummary(VariantUtils.ConvertTo<TowerDefensePacketBankData>(in args[0])));
			return true;
		}
		if (method == MethodName.CountPackets && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountPackets(VariantUtils.ConvertTo<TowerDefensePacketBankData>(in args[0])));
			return true;
		}
		if (method == MethodName.CreatePanelStyle && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<StyleBoxFlat>(CreatePanelStyle(VariantUtils.ConvertTo<Color>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<int>(in args[2])));
			return true;
		}
		if (method == MethodName.AddItemIfMissing && args.Count == 2)
		{
			AddItemIfMissing(VariantUtils.ConvertTo<ItemList>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.FormatVariant && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVariant(VariantUtils.ConvertTo<Variant>(in args[0])));
			return true;
		}
		if (method == MethodName.EmptyToPlaceholder && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(EmptyToPlaceholder(VariantUtils.ConvertTo<string>(in args[0])));
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
		if (method == MethodName.HasCompletePacketBankVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.DisposePacketBankPropertyBinding)
		{
			return true;
		}
		if (method == MethodName.RenderPacketBankEditor)
		{
			return true;
		}
		if (method == MethodName.BindPacketBankLayout)
		{
			return true;
		}
		if (method == MethodName.BindDirectEditControls)
		{
			return true;
		}
		if (method == MethodName.OnPacketBankPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshResourceFieldsPreview)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketBankFromHistory)
		{
			return true;
		}
		if (method == MethodName.DrawRandomPacketFromPool)
		{
			return true;
		}
		if (method == MethodName.FindCategoryIndex)
		{
			return true;
		}
		if (method == MethodName.IsPlantPacketCategory)
		{
			return true;
		}
		if (method == MethodName.AddCategoryEntry)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedCategoryEntry)
		{
			return true;
		}
		if (method == MethodName.AddPacketToSelectedCategory)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedPacketEntry)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedPacketEntry)
		{
			return true;
		}
		if (method == MethodName.SetCategoryDictionary)
		{
			return true;
		}
		if (method == MethodName.SelectCategory)
		{
			return true;
		}
		if (method == MethodName.SelectPacket)
		{
			return true;
		}
		if (method == MethodName.OnPacketResourcePicked)
		{
			return true;
		}
		if (method == MethodName.ShowPacketSelector)
		{
			return true;
		}
		if (method == MethodName.EnsurePacketSelectorWindow)
		{
			return true;
		}
		if (method == MethodName.FilterPacketSelector)
		{
			return true;
		}
		if (method == MethodName.ConfirmPacketSelectorSelection)
		{
			return true;
		}
		if (method == MethodName.ConfigurePacketSelectorPacketShow)
		{
			return true;
		}
		if (method == MethodName.ReadPacketKeyFromResourceFile)
		{
			return true;
		}
		if (method == MethodName.CleanResourceTextValue)
		{
			return true;
		}
		if (method == MethodName.RegisterPacketSelectionPath)
		{
			return true;
		}
		if (method == MethodName.GetCurrentProjectPathForPacketBankEditor)
		{
			return true;
		}
		if (method == MethodName.ToProjectRelativeDisplayPath)
		{
			return true;
		}
		if (method == MethodName.RefreshAll)
		{
			return true;
		}
		if (method == MethodName.RefreshCategories)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketList)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketGrid)
		{
			return true;
		}
		if (method == MethodName.CreatePacketCard)
		{
			return true;
		}
		if (method == MethodName.DuplicateCategoryDictionary)
		{
			return true;
		}
		if (method == MethodName.GetSelectedPacketArray)
		{
			return true;
		}
		if (method == MethodName.GetCategoryArray)
		{
			return true;
		}
		if (method == MethodName.EnsureCategorySelected)
		{
			return true;
		}
		if (method == MethodName.GetCategoryAt)
		{
			return true;
		}
		if (method == MethodName.ReadPacketKey)
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
		if (method == MethodName.CountPackets)
		{
			return true;
		}
		if (method == MethodName.CreatePanelStyle)
		{
			return true;
		}
		if (method == MethodName.AddItemIfMissing)
		{
			return true;
		}
		if (method == MethodName.FormatVariant)
		{
			return true;
		}
		if (method == MethodName.EmptyToPlaceholder)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editingPacketBank)
		{
			_editingPacketBank = VariantUtils.ConvertTo<TowerDefensePacketBankData>(in value);
			return true;
		}
		if (name == PropertyName._runtimePacketBankPreview)
		{
			_runtimePacketBankPreview = VariantUtils.ConvertTo<LevelEditorPacketBank>(in value);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			_summaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._categorySummaryLabel)
		{
			_categorySummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._packetSummaryLabel)
		{
			_packetSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._poolPreviewOption)
		{
			_poolPreviewOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._poolPreviewResultLabel)
		{
			_poolPreviewResultLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._categoryList)
		{
			_categoryList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._packetList)
		{
			_packetList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._packetGrid)
		{
			_packetGrid = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName._categoryNameEdit)
		{
			_categoryNameEdit = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._packetKeyEdit)
		{
			_packetKeyEdit = VariantUtils.ConvertTo<LineEdit>(in value);
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
		if (name == PropertyName._packetPicker)
		{
			_packetPicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._selectedCategory)
		{
			_selectedCategory = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._selectedCategoryIndex)
		{
			_selectedCategoryIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._selectedPacketIndex)
		{
			_selectedPacketIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._packetSelectorWindow)
		{
			_packetSelectorWindow = VariantUtils.ConvertTo<Window>(in value);
			return true;
		}
		if (name == PropertyName._packetSelectorFilter)
		{
			_packetSelectorFilter = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._packetSelectorGrid)
		{
			_packetSelectorGrid = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName._packetSelectorDescription)
		{
			_packetSelectorDescription = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editingPacketBank)
		{
			value = VariantUtils.CreateFrom(in _editingPacketBank);
			return true;
		}
		if (name == PropertyName._runtimePacketBankPreview)
		{
			value = VariantUtils.CreateFrom(in _runtimePacketBankPreview);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			value = VariantUtils.CreateFrom(in _summaryLabel);
			return true;
		}
		if (name == PropertyName._categorySummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _categorySummaryLabel);
			return true;
		}
		if (name == PropertyName._packetSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _packetSummaryLabel);
			return true;
		}
		if (name == PropertyName._poolPreviewOption)
		{
			value = VariantUtils.CreateFrom(in _poolPreviewOption);
			return true;
		}
		if (name == PropertyName._poolPreviewResultLabel)
		{
			value = VariantUtils.CreateFrom(in _poolPreviewResultLabel);
			return true;
		}
		if (name == PropertyName._categoryList)
		{
			value = VariantUtils.CreateFrom(in _categoryList);
			return true;
		}
		if (name == PropertyName._packetList)
		{
			value = VariantUtils.CreateFrom(in _packetList);
			return true;
		}
		if (name == PropertyName._packetGrid)
		{
			value = VariantUtils.CreateFrom(in _packetGrid);
			return true;
		}
		if (name == PropertyName._categoryNameEdit)
		{
			value = VariantUtils.CreateFrom(in _categoryNameEdit);
			return true;
		}
		if (name == PropertyName._packetKeyEdit)
		{
			value = VariantUtils.CreateFrom(in _packetKeyEdit);
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
		if (name == PropertyName._packetPicker)
		{
			value = VariantUtils.CreateFrom(in _packetPicker);
			return true;
		}
		if (name == PropertyName._selectedCategory)
		{
			value = VariantUtils.CreateFrom(in _selectedCategory);
			return true;
		}
		if (name == PropertyName._selectedCategoryIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedCategoryIndex);
			return true;
		}
		if (name == PropertyName._selectedPacketIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedPacketIndex);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._packetSelectorWindow)
		{
			value = VariantUtils.CreateFrom(in _packetSelectorWindow);
			return true;
		}
		if (name == PropertyName._packetSelectorFilter)
		{
			value = VariantUtils.CreateFrom(in _packetSelectorFilter);
			return true;
		}
		if (name == PropertyName._packetSelectorGrid)
		{
			value = VariantUtils.CreateFrom(in _packetSelectorGrid);
			return true;
		}
		if (name == PropertyName._packetSelectorDescription)
		{
			value = VariantUtils.CreateFrom(in _packetSelectorDescription);
			return true;
		}
		if (name == PropertyName._poolPreviewRandom)
		{
			value = VariantUtils.CreateFrom(in _poolPreviewRandom);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingPacketBank, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._runtimePacketBankPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._summaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._categorySummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._poolPreviewOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._poolPreviewResultLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._categoryList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._categoryNameEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetKeyEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToScene, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._selectedCategory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedCategoryIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedPacketIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetSelectorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetSelectorFilter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetSelectorGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetSelectorDescription, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._poolPreviewRandom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingPacketBank, Variant.From(in _editingPacketBank));
		info.AddProperty(PropertyName._runtimePacketBankPreview, Variant.From(in _runtimePacketBankPreview));
		info.AddProperty(PropertyName._summaryLabel, Variant.From(in _summaryLabel));
		info.AddProperty(PropertyName._categorySummaryLabel, Variant.From(in _categorySummaryLabel));
		info.AddProperty(PropertyName._packetSummaryLabel, Variant.From(in _packetSummaryLabel));
		info.AddProperty(PropertyName._poolPreviewOption, Variant.From(in _poolPreviewOption));
		info.AddProperty(PropertyName._poolPreviewResultLabel, Variant.From(in _poolPreviewResultLabel));
		info.AddProperty(PropertyName._categoryList, Variant.From(in _categoryList));
		info.AddProperty(PropertyName._packetList, Variant.From(in _packetList));
		info.AddProperty(PropertyName._packetGrid, Variant.From(in _packetGrid));
		info.AddProperty(PropertyName._categoryNameEdit, Variant.From(in _categoryNameEdit));
		info.AddProperty(PropertyName._packetKeyEdit, Variant.From(in _packetKeyEdit));
		info.AddProperty(PropertyName._resourceName, Variant.From(in _resourceName));
		info.AddProperty(PropertyName._localToScene, Variant.From(in _localToScene));
		info.AddProperty(PropertyName._packetPicker, Variant.From(in _packetPicker));
		info.AddProperty(PropertyName._selectedCategory, Variant.From(in _selectedCategory));
		info.AddProperty(PropertyName._selectedCategoryIndex, Variant.From(in _selectedCategoryIndex));
		info.AddProperty(PropertyName._selectedPacketIndex, Variant.From(in _selectedPacketIndex));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._packetSelectorWindow, Variant.From(in _packetSelectorWindow));
		info.AddProperty(PropertyName._packetSelectorFilter, Variant.From(in _packetSelectorFilter));
		info.AddProperty(PropertyName._packetSelectorGrid, Variant.From(in _packetSelectorGrid));
		info.AddProperty(PropertyName._packetSelectorDescription, Variant.From(in _packetSelectorDescription));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingPacketBank, out var value))
		{
			_editingPacketBank = value.As<TowerDefensePacketBankData>();
		}
		if (info.TryGetProperty(PropertyName._runtimePacketBankPreview, out var value2))
		{
			_runtimePacketBankPreview = value2.As<LevelEditorPacketBank>();
		}
		if (info.TryGetProperty(PropertyName._summaryLabel, out var value3))
		{
			_summaryLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._categorySummaryLabel, out var value4))
		{
			_categorySummaryLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._packetSummaryLabel, out var value5))
		{
			_packetSummaryLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._poolPreviewOption, out var value6))
		{
			_poolPreviewOption = value6.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._poolPreviewResultLabel, out var value7))
		{
			_poolPreviewResultLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._categoryList, out var value8))
		{
			_categoryList = value8.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._packetList, out var value9))
		{
			_packetList = value9.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._packetGrid, out var value10))
		{
			_packetGrid = value10.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName._categoryNameEdit, out var value11))
		{
			_categoryNameEdit = value11.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._packetKeyEdit, out var value12))
		{
			_packetKeyEdit = value12.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._resourceName, out var value13))
		{
			_resourceName = value13.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToScene, out var value14))
		{
			_localToScene = value14.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._packetPicker, out var value15))
		{
			_packetPicker = value15.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._selectedCategory, out var value16))
		{
			_selectedCategory = value16.As<string>();
		}
		if (info.TryGetProperty(PropertyName._selectedCategoryIndex, out var value17))
		{
			_selectedCategoryIndex = value17.As<int>();
		}
		if (info.TryGetProperty(PropertyName._selectedPacketIndex, out var value18))
		{
			_selectedPacketIndex = value18.As<int>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value19))
		{
			_updatingControls = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._packetSelectorWindow, out var value20))
		{
			_packetSelectorWindow = value20.As<Window>();
		}
		if (info.TryGetProperty(PropertyName._packetSelectorFilter, out var value21))
		{
			_packetSelectorFilter = value21.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._packetSelectorGrid, out var value22))
		{
			_packetSelectorGrid = value22.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName._packetSelectorDescription, out var value23))
		{
			_packetSelectorDescription = value23.As<Label>();
		}
	}
}
