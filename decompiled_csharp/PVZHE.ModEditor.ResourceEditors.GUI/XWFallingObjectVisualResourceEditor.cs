using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.FileSystem;
using PVZHE.ModEditor.ModSystem;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWFallingObjectVisualResourceEditor.cs")]
public class XWFallingObjectVisualResourceEditor : XWGenericVisualResourceEditor
{
	private sealed class FallingObjectObjectCategory
	{
		public string Name { get; }

		public string Description { get; }

		public HashSet<ObjectManagerConfig.OBJECT> Objects { get; }

		public FallingObjectObjectCategory(string name, string description, params ObjectManagerConfig.OBJECT[] objects)
		{
			Name = name;
			Description = description;
			Objects = new HashSet<ObjectManagerConfig.OBJECT>(objects);
		}

		public bool Contains(ObjectManagerConfig.OBJECT value)
		{
			if (!(Name == "全部"))
			{
				return Objects.Contains(value);
			}
			return true;
		}
	}

	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public static readonly StringName HasCompleteFallingObjectVisualCoverage = "HasCompleteFallingObjectVisualCoverage";

		public static readonly StringName DisposeVisualPropertyBindings = "DisposeVisualPropertyBindings";

		public static readonly StringName RenderWeightItemPreview = "RenderWeightItemPreview";

		public static readonly StringName BindWeightItemDirectFields = "BindWeightItemDirectFields";

		public static readonly StringName MountWeightItemObjectLibrary = "MountWeightItemObjectLibrary";

		public static readonly StringName UpdateWeightItemPreview = "UpdateWeightItemPreview";

		public static readonly StringName RenderFallingObjectEditor = "RenderFallingObjectEditor";

		public static readonly StringName BindFallingObjectLayout = "BindFallingObjectLayout";

		public static readonly StringName MountFallingObjectLibrary = "MountFallingObjectLibrary";

		public static readonly StringName CreateFallingObjectPreviewPanel = "CreateFallingObjectPreviewPanel";

		public static readonly StringName AddWeightItem = "AddWeightItem";

		public static readonly StringName RemoveSelectedWeightItem = "RemoveSelectedWeightItem";

		public static readonly StringName MoveSelectedWeightItem = "MoveSelectedWeightItem";

		public static readonly StringName DuplicateWeightItems = "DuplicateWeightItems";

		public static readonly StringName SetFallingObjectWeightItems = "SetFallingObjectWeightItems";

		public static readonly StringName SetWeightItemProperty = "SetWeightItemProperty";

		public static readonly StringName BindSelectedWeightItemFields = "BindSelectedWeightItemFields";

		public static readonly StringName OnFallingObjectVisualPropertyEdited = "OnFallingObjectVisualPropertyEdited";

		public static readonly StringName OnFallingObjectChanged = "OnFallingObjectChanged";

		public static readonly StringName OnWeightItemVisualPropertyEdited = "OnWeightItemVisualPropertyEdited";

		public static readonly StringName ScheduleFallingObjectSave = "ScheduleFallingObjectSave";

		public static readonly StringName RefreshFallingObjectVisualsAfterWeightItemEdit = "RefreshFallingObjectVisualsAfterWeightItemEdit";

		public static readonly StringName RefreshFallingObjectEditorFromHistory = "RefreshFallingObjectEditorFromHistory";

		public static readonly StringName RefreshWeightItemEditorFromHistory = "RefreshWeightItemEditorFromHistory";

		public static readonly StringName RebuildCurrentFallingObjectEditor = "RebuildCurrentFallingObjectEditor";

		public static readonly StringName SavePendingFallingObjectResource = "SavePendingFallingObjectResource";

		public static readonly StringName SaveFallingObjectResource = "SaveFallingObjectResource";

		public static readonly StringName RunSinglePickPreview = "RunSinglePickPreview";

		public static readonly StringName RunDropSequencePreview = "RunDropSequencePreview";

		public static readonly StringName SpawnFallingObjectPreview = "SpawnFallingObjectPreview";

		public static readonly StringName ClearDropPreview = "ClearDropPreview";

		public static readonly StringName RunBatchPickPreview = "RunBatchPickPreview";

		public static readonly StringName PickWeightedIndex = "PickWeightedIndex";

		public static readonly StringName RecordSimulationHit = "RecordSimulationHit";

		public static readonly StringName ResetSimulationResults = "ResetSimulationResults";

		public static readonly StringName RefreshSimulationGrid = "RefreshSimulationGrid";

		public static readonly StringName CreateSimulationResultCard = "CreateSimulationResultCard";

		public static readonly StringName SelectWeightItem = "SelectWeightItem";

		public static readonly StringName RefreshAll = "RefreshAll";

		public static readonly StringName RefreshWeightList = "RefreshWeightList";

		public static readonly StringName RefreshSelectedWeightControls = "RefreshSelectedWeightControls";

		public static readonly StringName RefreshWeightGrid = "RefreshWeightGrid";

		public static readonly StringName CreateWeightCard = "CreateWeightCard";

		public static readonly StringName SetFallingObjectCategory = "SetFallingObjectCategory";

		public static readonly StringName RefreshObjectLibraryForCategory = "RefreshObjectLibraryForCategory";

		public static readonly StringName RefreshCategoryGrid = "RefreshCategoryGrid";

		public static readonly StringName GetObjectCategory = "GetObjectCategory";

		public static readonly StringName GetObjectCategoryIconPath = "GetObjectCategoryIconPath";

		public static readonly StringName BuildCategorySummary = "BuildCategorySummary";

		public static readonly StringName ReadSelectedObject = "ReadSelectedObject";

		public static readonly StringName GetSelectedWeightItem = "GetSelectedWeightItem";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName BuildSummary = "BuildSummary";

		public static readonly StringName GetTotalWeight = "GetTotalWeight";

		public static readonly StringName CountEmpty = "CountEmpty";

		public static readonly StringName FormatWeightItem = "FormatWeightItem";

		public static readonly StringName CreatePanelStyle = "CreatePanelStyle";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName FallingCategoryVisualCardCount = "FallingCategoryVisualCardCount";

		public static readonly StringName HasFallingObjectLibrarySearchAndPaging = "HasFallingObjectLibrarySearchAndPaging";

		public static readonly StringName HasWeightItemLibrarySearchAndPaging = "HasWeightItemLibrarySearchAndPaging";

		public static readonly StringName PreviewSequenceGeneration = "PreviewSequenceGeneration";

		public static readonly StringName IsFallingPreviewRendering = "IsFallingPreviewRendering";

		public static readonly StringName _editingFallingObject = "_editingFallingObject";

		public static readonly StringName _summaryLabel = "_summaryLabel";

		public static readonly StringName _pickPreviewLabel = "_pickPreviewLabel";

		public static readonly StringName _lastPickLabel = "_lastPickLabel";

		public static readonly StringName _categorySummaryLabel = "_categorySummaryLabel";

		public static readonly StringName _weightList = "_weightList";

		public static readonly StringName _weightGrid = "_weightGrid";

		public static readonly StringName _simulationGrid = "_simulationGrid";

		public static readonly StringName _dropPreviewRoot = "_dropPreviewRoot";

		public static readonly StringName _categoryGrid = "_categoryGrid";

		public static readonly StringName _objectPicker = "_objectPicker";

		public static readonly StringName _weightSpin = "_weightSpin";

		public static readonly StringName _emptyCheck = "_emptyCheck";

		public static readonly StringName _selectedCategory = "_selectedCategory";

		public static readonly StringName _selectedWeightIndex = "_selectedWeightIndex";

		public static readonly StringName _simulationRuns = "_simulationRuns";

		public static readonly StringName _previewRandom = "_previewRandom";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _fallingObjectSaveTimer = "_fallingObjectSaveTimer";

		public static readonly StringName _editingWeightItem = "_editingWeightItem";

		public static readonly StringName _weightItemComparisonSpin = "_weightItemComparisonSpin";

		public static readonly StringName _weightItemProbability = "_weightItemProbability";

		public static readonly StringName _weightItemProbabilityLabel = "_weightItemProbabilityLabel";

		public static readonly StringName _weightItemTitleLabel = "_weightItemTitleLabel";

		public static readonly StringName _weightItemStatusLabel = "_weightItemStatusLabel";

		public static readonly StringName _weightItemDropRoot = "_weightItemDropRoot";

		public static readonly StringName _weightItemObjectPicker = "_weightItemObjectPicker";

		public static readonly StringName _weightItemWeightSpin = "_weightItemWeightSpin";

		public static readonly StringName _weightItemEmptyCheck = "_weightItemEmptyCheck";

		public static readonly StringName _simulationViewport = "_simulationViewport";

		public static readonly StringName _weightItemPreviewViewport = "_weightItemPreviewViewport";

		public static readonly StringName _previewSequenceGeneration = "_previewSequenceGeneration";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string EditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWFallingObjectVisualEditorLayout.tscn";

	private const string SimulationPreviewScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWFallingObjectSimulationPreview.tscn";

	private const string WeightItemPreviewScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWFallingObjectWeightItemPreview.tscn";

	private const string VisualChoiceCardScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWGameVisualChoiceCard.tscn";

	private const string ObjectPoolVisualPickerScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWObjectPoolVisualPicker.tscn";

	private static PackedScene _editorLayoutScene;

	private static PackedScene _simulationPreviewScene;

	private static PackedScene _weightItemPreviewScene;

	private static PackedScene _visualChoiceCardScene;

	private static PackedScene _objectPoolVisualPickerScene;

	private static readonly FallingObjectObjectCategory[] FallingObjectCategories = new FallingObjectObjectCategory[7]
	{
		new FallingObjectObjectCategory("全部", "显示所有对象池条目。"),
		new FallingObjectObjectCategory("空项", "权重命中后不生成任何掉落物。", default(ObjectManagerConfig.OBJECT)),
		new FallingObjectObjectCategory("阳光", "阳光、脑子阳光和特殊阳光。", ObjectManagerConfig.OBJECT.GAMECOLLECT, ObjectManagerConfig.OBJECT.SUN, ObjectManagerConfig.OBJECT.SUN_BRAIN, ObjectManagerConfig.OBJECT.SUN_JALAPENO),
		new FallingObjectObjectCategory("货币", "银币、金币、钻石和其它货币掉落。", ObjectManagerConfig.OBJECT.COIN, ObjectManagerConfig.OBJECT.COIN_SILVER, ObjectManagerConfig.OBJECT.COIN_GOLD, ObjectManagerConfig.OBJECT.COIN_DIAMOND, ObjectManagerConfig.OBJECT.COIN_LUCKY_BAG, ObjectManagerConfig.OBJECT.COIN_TQ, ObjectManagerConfig.OBJECT.COIN_YB1, ObjectManagerConfig.OBJECT.COIN_YB2, ObjectManagerConfig.OBJECT.COIN_GOLD_SHARD),
		new FallingObjectObjectCategory("战斗", "子弹、伤害部件等战斗对象。", ObjectManagerConfig.OBJECT.PROJECTILE, ObjectManagerConfig.OBJECT.damagePart),
		new FallingObjectObjectCategory("特效", "一次性粒子和地面特效。", ObjectManagerConfig.OBJECT.PARTICLES_SPLASH, ObjectManagerConfig.OBJECT.PARTICLES_RISE_DIRT, ObjectManagerConfig.OBJECT.PARTICLES_ICE_TRAP),
		new FallingObjectObjectCategory("其它", "未被上面分类收纳的对象池条目。")
	};

	private FallingObjectConfig _editingFallingObject;

	private Label _summaryLabel;

	private Label _pickPreviewLabel;

	private Label _lastPickLabel;

	private Label _categorySummaryLabel;

	private ItemList _weightList;

	private GridContainer _weightGrid;

	private GridContainer _simulationGrid;

	private Node2D _dropPreviewRoot;

	private HFlowContainer _categoryGrid;

	private XWObjectPoolVisualPicker _objectPicker;

	private SpinBox _weightSpin;

	private CheckBox _emptyCheck;

	private string _selectedCategory = "全部";

	private int _selectedWeightIndex = -1;

	private int _simulationRuns;

	private readonly System.Collections.Generic.Dictionary<int, int> _simulationHits = new System.Collections.Generic.Dictionary<int, int>();

	private readonly RandomNumberGenerator _previewRandom = new RandomNumberGenerator();

	private bool _updatingControls;

	private Timer _fallingObjectSaveTimer;

	private XWVisualPropertyBinding _fallingObjectPropertyBinding;

	private XWVisualPropertyBinding _weightItemPropertyBinding;

	private FallingObjectWeightItemConfig _editingWeightItem;

	private SpinBox _weightItemComparisonSpin;

	private ProgressBar _weightItemProbability;

	private Label _weightItemProbabilityLabel;

	private Label _weightItemTitleLabel;

	private Label _weightItemStatusLabel;

	private Node2D _weightItemDropRoot;

	private XWObjectPoolVisualPicker _weightItemObjectPicker;

	private SpinBox _weightItemWeightSpin;

	private CheckBox _weightItemEmptyCheck;

	private SubViewport _simulationViewport;

	private SubViewport _weightItemPreviewViewport;

	private int _previewSequenceGeneration;

	public int FallingCategoryVisualCardCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_categoryGrid))
			{
				return 0;
			}
			return _categoryGrid.GetChildCount();
		}
	}

	public bool HasFallingObjectLibrarySearchAndPaging
	{
		get
		{
			if (GodotObject.IsInstanceValid(_objectPicker))
			{
				return _objectPicker.HasSearchAndPaging;
			}
			return false;
		}
	}

	public bool HasWeightItemLibrarySearchAndPaging
	{
		get
		{
			if (GodotObject.IsInstanceValid(_weightItemObjectPicker))
			{
				return _weightItemObjectPicker.HasSearchAndPaging;
			}
			return false;
		}
	}

	public int PreviewSequenceGeneration => _previewSequenceGeneration;

	public bool IsFallingPreviewRendering
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_simulationViewport) || _simulationViewport.RenderTargetUpdateMode == SubViewport.UpdateMode.Disabled)
			{
				if (GodotObject.IsInstanceValid(_weightItemPreviewViewport))
				{
					return _weightItemPreviewViewport.RenderTargetUpdateMode != SubViewport.UpdateMode.Disabled;
				}
				return false;
			}
			return true;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		_fallingObjectSaveTimer = new Timer
		{
			WaitTime = 0.35,
			OneShot = true
		};
		_fallingObjectSaveTimer.Timeout += SavePendingFallingObjectResource;
		AddChild(_fallingObjectSaveTimer, forceReadableName: false, InternalMode.Disabled);
	}

	public override void _ExitTree()
	{
		DisposeVisualPropertyBindings();
		if (GodotObject.IsInstanceValid(_fallingObjectSaveTimer))
		{
			_fallingObjectSaveTimer.Stop();
		}
		base._ExitTree();
	}

	protected override void OnVisualEditorVisibilityChanged(bool visible)
	{
		base.OnVisualEditorVisibilityChanged(visible);
		SubViewport.UpdateMode renderTargetUpdateMode = (SubViewport.UpdateMode)(visible ? 4 : 0);
		if (GodotObject.IsInstanceValid(_simulationViewport))
		{
			_simulationViewport.RenderTargetUpdateMode = renderTargetUpdateMode;
		}
		if (GodotObject.IsInstanceValid(_weightItemPreviewViewport))
		{
			_weightItemPreviewViewport.RenderTargetUpdateMode = renderTargetUpdateMode;
		}
		if (!visible)
		{
			_previewSequenceGeneration++;
		}
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeVisualPropertyBindings();
		Resource currentResource = CurrentResource;
		if (!(currentResource is FallingObjectConfig fallingObjectConfig))
		{
			if (currentResource is FallingObjectWeightItemConfig fallingObjectWeightItemConfig)
			{
				_editingFallingObject = null;
				_editingWeightItem = fallingObjectWeightItemConfig;
				RenderWeightItemPreview(fallingObjectWeightItemConfig);
			}
		}
		else
		{
			_editingFallingObject = fallingObjectConfig;
			_editingWeightItem = null;
			RenderFallingObjectEditor(fallingObjectConfig);
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!HasCompleteFallingObjectVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool HasCompleteFallingObjectVisualCoverage(Resource resource)
	{
		Type type = resource?.GetType();
		if (!(type == typeof(FallingObjectConfig)))
		{
			return type == typeof(FallingObjectWeightItemConfig);
		}
		return true;
	}

	private void DisposeVisualPropertyBindings()
	{
		_fallingObjectPropertyBinding?.Dispose();
		_fallingObjectPropertyBinding = null;
		_weightItemPropertyBinding?.Dispose();
		_weightItemPropertyBinding = null;
		_editingFallingObject = null;
		_editingWeightItem = null;
	}

	private void RenderWeightItemPreview(FallingObjectWeightItemConfig weightItem)
	{
		if (CanvasGrid == null || !GodotObject.IsInstanceValid(weightItem))
		{
			return;
		}
		if (_weightItemPreviewScene == null)
		{
			_weightItemPreviewScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWFallingObjectWeightItemPreview.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		Control control = _weightItemPreviewScene?.Instantiate<Control>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(control))
		{
			CanvasGrid.Columns = 1;
			_weightItemComparisonSpin = control.GetNode<SpinBox>("Layout/ComparisonRow/ComparisonWeight");
			_weightItemProbability = control.GetNode<ProgressBar>("Layout/ComparisonRow/Probability");
			_weightItemProbabilityLabel = control.GetNode<Label>("Layout/ComparisonRow/ProbabilityLabel");
			_weightItemTitleLabel = control.GetNode<Label>("Layout/Header/Title");
			_weightItemStatusLabel = control.GetNode<Label>("Layout/Header/Status");
			_weightItemDropRoot = control.GetNode<Node2D>("%DropRoot");
			_weightItemPreviewViewport = control.GetNode<SubViewport>("Layout/GameStage/ViewportContainer/Viewport");
			_weightItemPreviewViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode)(IsVisibleInTree() ? 4 : 0);
			_weightItemWeightSpin = control.GetNode<SpinBox>("%WeightSpinBox");
			_weightItemEmptyCheck = control.GetNode<CheckBox>("%EmptyCheckBox");
			MountWeightItemObjectLibrary(control.GetNode<VBoxContainer>("%ObjectLibraryHost"), weightItem.item);
			BindWeightItemDirectFields(control, weightItem);
			_weightItemComparisonSpin.ValueChanged += (double _) =>
			{
				UpdateWeightItemPreview();
			};
			control.GetNode<Button>("Layout/ComparisonRow/DropButton").Pressed += () =>
			{
				SpawnFallingObjectPreview(_editingWeightItem, _weightItemDropRoot);
			};
			CanvasGrid.AddChild(control, forceReadableName: false, InternalMode.Disabled);
			PreviewList?.AddItem("对照权重只用于计算单项相对概率，不会修改资源。");
			UpdateWeightItemPreview();
		}
	}

	private void BindWeightItemDirectFields(Control preview, FallingObjectWeightItemConfig weightItem)
	{
		_weightItemPropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnWeightItemVisualPropertyEdited);
		_weightItemPropertyBinding.BindText(preview.GetNode<LineEdit>("%ResourceNameEdit"), weightItem, "resource_name", UpdateWeightItemPreview, this, "RefreshWeightItemEditorFromHistory");
		_weightItemPropertyBinding.BindToggle(preview.GetNode<CheckButton>("%LocalToSceneCheck"), weightItem, "resource_local_to_scene", UpdateWeightItemPreview, this, "RefreshWeightItemEditorFromHistory");
		_weightItemPropertyBinding.BindNumber(_weightItemWeightSpin, weightItem, "weight", UpdateWeightItemPreview, this, "RefreshWeightItemEditorFromHistory");
		_weightItemPropertyBinding.BindToggle(_weightItemEmptyCheck, weightItem, "empty", UpdateWeightItemPreview, this, "RefreshWeightItemEditorFromHistory");
	}

	private void MountWeightItemObjectLibrary(VBoxContainer host, ObjectManagerConfig.OBJECT selected)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return;
		}
		if (_objectPoolVisualPickerScene == null)
		{
			_objectPoolVisualPickerScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWObjectPoolVisualPicker.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		_weightItemObjectPicker = _objectPoolVisualPickerScene?.Instantiate<XWObjectPoolVisualPicker>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(_weightItemObjectPicker))
		{
			return;
		}
		_weightItemObjectPicker.Name = "WeightItemObjectLibrary";
		host.AddChild(_weightItemObjectPicker, forceReadableName: false, InternalMode.Disabled);
		_weightItemObjectPicker.Configure(BuildObjectVisualChoices(GetObjectsForCategory("全部")), Convert.ToInt32(selected));
		_weightItemObjectPicker.ChoiceSelected += (int id) =>
		{
			if (!_updatingControls && _weightItemPropertyBinding != null && GodotObject.IsInstanceValid(_editingWeightItem))
			{
				_weightItemPropertyBinding.SetValue(_editingWeightItem, "item", id, "修改掉落对象", this, "RefreshWeightItemEditorFromHistory");
				UpdateWeightItemPreview();
			}
		};
	}

	private void UpdateWeightItemPreview()
	{
		if (GodotObject.IsInstanceValid(_editingWeightItem))
		{
			double num = (GodotObject.IsInstanceValid(_weightItemComparisonSpin) ? _weightItemComparisonSpin.Value : 100.0);
			double num2 = Math.Max(0, _editingWeightItem.weight);
			double num3 = ((num2 + num <= 0.0) ? 0.0 : (num2 / (num2 + num)));
			if (GodotObject.IsInstanceValid(_weightItemProbability))
			{
				_weightItemProbability.Value = num3 * 100.0;
			}
			if (GodotObject.IsInstanceValid(_weightItemProbabilityLabel))
			{
				_weightItemProbabilityLabel.Text = $"概率 {num3:P1}";
			}
			if (GodotObject.IsInstanceValid(_weightItemTitleLabel))
			{
				_weightItemTitleLabel.Text = (string.IsNullOrWhiteSpace(_editingWeightItem.ResourceName) ? "单项掉落权重预览" : (_editingWeightItem.ResourceName + " · 掉落权重"));
			}
			if (GodotObject.IsInstanceValid(_weightItemStatusLabel))
			{
				_weightItemStatusLabel.Text = (_editingWeightItem.empty ? $"空掉落 · 权重 {_editingWeightItem.weight}" : $"{_editingWeightItem.item} · 权重 {_editingWeightItem.weight}");
			}
		}
	}

	private void RenderFallingObjectEditor(FallingObjectConfig fallingObject)
	{
		if (CanvasGrid != null)
		{
			CanvasGrid.Columns = 1;
			if (_editorLayoutScene == null)
			{
				_editorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWFallingObjectVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _editorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindFallingObjectLayout(vBoxContainer, fallingObject);
				RefreshAll();
				BindSelectedWeightItemFields();
				AddSummaryRows(fallingObject);
			}
		}
	}

	private void BindFallingObjectLayout(VBoxContainer root, FallingObjectConfig fallingObject)
	{
		_fallingObjectPropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnFallingObjectVisualPropertyEdited);
		_fallingObjectPropertyBinding.BindText(root.GetNode<LineEdit>("%ResourceNameEdit"), fallingObject, "resource_name", null, this, "RefreshFallingObjectEditorFromHistory");
		_fallingObjectPropertyBinding.BindToggle(root.GetNode<CheckButton>("%LocalToSceneCheck"), fallingObject, "resource_local_to_scene", null, this, "RefreshFallingObjectEditorFromHistory");
		root.GetNode<VBoxContainer>("%SimulationHost").AddChild(CreateFallingObjectPreviewPanel(fallingObject), forceReadableName: false, InternalMode.Disabled);
		_categorySummaryLabel = root.GetNode<Label>("%CategorySummaryLabel");
		_categoryGrid = root.GetNode<HFlowContainer>("%CategoryGrid");
		MountFallingObjectLibrary(root.GetNode<VBoxContainer>("%ObjectLibraryHost"));
		_weightSpin = root.GetNode<SpinBox>("%WeightSpinBox");
		_emptyCheck = root.GetNode<CheckBox>("%EmptyCheckBox");
		root.GetNode<Button>("%AddWeightButton").Pressed += AddWeightItem;
		root.GetNode<Button>("%RemoveWeightButton").Pressed += RemoveSelectedWeightItem;
		root.GetNode<Button>("%WeightUpButton").Pressed += () =>
		{
			MoveSelectedWeightItem(-1);
		};
		root.GetNode<Button>("%WeightDownButton").Pressed += () =>
		{
			MoveSelectedWeightItem(1);
		};
		_weightList = root.GetNode<ItemList>("%WeightList");
		_weightList.ItemSelected += (long index) =>
		{
			SelectWeightItem((int)index);
		};
		_weightGrid = root.GetNode<GridContainer>("%WeightGrid");
	}

	private void MountFallingObjectLibrary(VBoxContainer host)
	{
		if (!GodotObject.IsInstanceValid(host))
		{
			return;
		}
		if (_objectPoolVisualPickerScene == null)
		{
			_objectPoolVisualPickerScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWObjectPoolVisualPicker.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		_objectPicker = _objectPoolVisualPickerScene?.Instantiate<XWObjectPoolVisualPicker>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(_objectPicker))
		{
			return;
		}
		_objectPicker.Name = "FallingObjectLibrary";
		host.AddChild(_objectPicker, forceReadableName: false, InternalMode.Disabled);
		RefreshObjectLibraryForCategory();
		_objectPicker.ChoiceSelected += (int id) =>
		{
			if (!_updatingControls)
			{
				SetWeightItemProperty("item", Variant.From(in id));
			}
		};
	}

	private Control CreateFallingObjectPreviewPanel(FallingObjectConfig fallingObject)
	{
		if (_simulationPreviewScene == null)
		{
			_simulationPreviewScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWFallingObjectSimulationPreview.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		if (!GodotObject.IsInstanceValid(_simulationPreviewScene))
		{
			return new Label
			{
				Text = "掉落模拟场景加载失败"
			};
		}
		Control control = _simulationPreviewScene.Instantiate<Control>(PackedScene.GenEditState.Disabled);
		_summaryLabel = control.GetNode<Label>("Layout/Summary");
		_pickPreviewLabel = control.GetNode<Label>("Layout/PickSummary");
		_lastPickLabel = control.GetNode<Label>("Layout/Toolbar/LastPick");
		_simulationGrid = control.GetNode<GridContainer>("Layout/SimulationGrid");
		_dropPreviewRoot = control.GetNode<Node2D>("%DropRoot");
		_simulationViewport = control.GetNode<SubViewport>("Layout/GameStage/ViewportContainer/Viewport");
		_simulationViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode)(IsVisibleInTree() ? 4 : 0);
		control.GetNode<SpinBox>("Layout/Toolbar/SimulationCount");
		control.GetNode<Button>("Layout/Toolbar/PickOnceButton").Pressed += RunSinglePickPreview;
		control.GetNode<Button>("Layout/Toolbar/SimulateButton").Pressed += RunDropSequencePreview;
		control.GetNode<Button>("Layout/Toolbar/ClearButton").Pressed += ClearDropPreview;
		_summaryLabel.Text = BuildSummary(fallingObject);
		_previewRandom.Randomize();
		return control;
	}

	private void AddWeightItem()
	{
		if (_editingFallingObject != null && _fallingObjectPropertyBinding != null)
		{
			Array<FallingObjectWeightItemConfig> array = DuplicateWeightItems(_editingFallingObject.weightItem);
			bool flag = _selectedCategory == "空项";
			FallingObjectWeightItemConfig item = new FallingObjectWeightItemConfig
			{
				item = ((!flag) ? ReadSelectedObject() : ObjectManagerConfig.OBJECT.NOONE),
				weight = Mathf.Max(0, Mathf.RoundToInt((float)(_weightSpin?.Value ?? 100.0))),
				empty = (flag || (_emptyCheck?.ButtonPressed ?? false))
			};
			array.Add(item);
			_selectedWeightIndex = array.Count - 1;
			SetFallingObjectWeightItems(array, "添加掉落权重项");
		}
	}

	private void RemoveSelectedWeightItem()
	{
		if (_editingFallingObject?.weightItem != null && _selectedWeightIndex >= 0 && _selectedWeightIndex < _editingFallingObject.weightItem.Count)
		{
			Array<FallingObjectWeightItemConfig> array = DuplicateWeightItems(_editingFallingObject.weightItem);
			array.RemoveAt(_selectedWeightIndex);
			_selectedWeightIndex = Math.Min(_selectedWeightIndex, array.Count - 1);
			SetFallingObjectWeightItems(array, "删除掉落权重项");
		}
	}

	private void MoveSelectedWeightItem(int direction)
	{
		if (_editingFallingObject?.weightItem != null && _selectedWeightIndex >= 0 && direction != 0)
		{
			int num = Math.Clamp(_selectedWeightIndex + direction, 0, _editingFallingObject.weightItem.Count - 1);
			if (num != _selectedWeightIndex)
			{
				Array<FallingObjectWeightItemConfig> array = DuplicateWeightItems(_editingFallingObject.weightItem);
				FallingObjectWeightItemConfig item = array[_selectedWeightIndex];
				array.RemoveAt(_selectedWeightIndex);
				array.Insert(num, item);
				_selectedWeightIndex = num;
				SetFallingObjectWeightItems(array, (direction < 0) ? "上移掉落权重项" : "下移掉落权重项");
			}
		}
	}

	private static Array<FallingObjectWeightItemConfig> DuplicateWeightItems(Array<FallingObjectWeightItemConfig> source)
	{
		if (source != null)
		{
			return source.Duplicate();
		}
		return new Array<FallingObjectWeightItemConfig>();
	}

	private void SetFallingObjectWeightItems(Array<FallingObjectWeightItemConfig> nextItems, string actionName)
	{
		if (_fallingObjectPropertyBinding != null && GodotObject.IsInstanceValid(_editingFallingObject))
		{
			_weightItemPropertyBinding?.Dispose();
			_weightItemPropertyBinding = null;
			_editingWeightItem = null;
			_fallingObjectPropertyBinding.SetValue(_editingFallingObject, "weightItem", nextItems, actionName, this, "RefreshFallingObjectEditorFromHistory");
			BindSelectedWeightItemFields();
		}
	}

	private void SetWeightItemProperty(string propertyName, Variant value)
	{
		if (!_updatingControls && _editingFallingObject?.weightItem != null && _selectedWeightIndex >= 0 && _selectedWeightIndex < _editingFallingObject.weightItem.Count)
		{
			FallingObjectWeightItemConfig fallingObjectWeightItemConfig = _editingFallingObject.weightItem[_selectedWeightIndex];
			if (fallingObjectWeightItemConfig != null && _weightItemPropertyBinding != null && propertyName == "item")
			{
				_weightItemPropertyBinding.SetValue(fallingObjectWeightItemConfig, "item", value, "修改掉落对象", this, "RefreshFallingObjectEditorFromHistory");
			}
		}
	}

	private void BindSelectedWeightItemFields()
	{
		_weightItemPropertyBinding?.Dispose();
		_weightItemPropertyBinding = null;
		_editingWeightItem = GetSelectedWeightItem();
		if (GodotObject.IsInstanceValid(_editingWeightItem))
		{
			_weightItemPropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnWeightItemVisualPropertyEdited);
			_weightItemPropertyBinding.BindNumber(_weightSpin, _editingWeightItem, "weight", null, this, "RefreshFallingObjectEditorFromHistory");
			_weightItemPropertyBinding.BindToggle(_emptyCheck, _editingWeightItem, "empty", null, this, "RefreshFallingObjectEditorFromHistory");
		}
	}

	private void OnFallingObjectVisualPropertyEdited(bool committed)
	{
		OnFallingObjectChanged();
	}

	private void OnFallingObjectChanged()
	{
		if (GodotObject.IsInstanceValid(_editingFallingObject) && CurrentResource is FallingObjectConfig fallingObjectConfig && fallingObjectConfig == _editingFallingObject)
		{
			MarkCurrentResourceDirty();
			_editingFallingObject.EmitChanged();
			_editingFallingObject.NotifyPropertyListChanged();
			ScheduleFallingObjectSave();
			ResetSimulationResults();
			RefreshAll();
		}
	}

	private void OnWeightItemVisualPropertyEdited(bool committed)
	{
		if (GodotObject.IsInstanceValid(_editingWeightItem))
		{
			if (CurrentResource is FallingObjectWeightItemConfig fallingObjectWeightItemConfig && fallingObjectWeightItemConfig == _editingWeightItem)
			{
				MarkCurrentResourceDirty();
				fallingObjectWeightItemConfig.EmitChanged();
				ScheduleFallingObjectSave();
				UpdateWeightItemPreview();
			}
			else if (CurrentResource is FallingObjectConfig fallingObjectConfig && fallingObjectConfig == _editingFallingObject)
			{
				MarkCurrentResourceDirty();
				fallingObjectConfig.EmitChanged();
				fallingObjectConfig.NotifyPropertyListChanged();
				ScheduleFallingObjectSave();
				RefreshFallingObjectVisualsAfterWeightItemEdit();
			}
		}
	}

	private void ScheduleFallingObjectSave()
	{
		if (GodotObject.IsInstanceValid(_fallingObjectSaveTimer))
		{
			_fallingObjectSaveTimer.Start();
		}
	}

	private void RefreshFallingObjectVisualsAfterWeightItemEdit()
	{
		ResetSimulationResults();
		if (GodotObject.IsInstanceValid(_summaryLabel))
		{
			_summaryLabel.Text = BuildSummary(_editingFallingObject);
		}
		if (GodotObject.IsInstanceValid(_pickPreviewLabel))
		{
			_pickPreviewLabel.Text = $"Pick 预览: 总权重 {GetTotalWeight(_editingFallingObject)}, 空项 {CountEmpty(_editingFallingObject)}";
		}
		if (GodotObject.IsInstanceValid(_categorySummaryLabel))
		{
			_categorySummaryLabel.Text = BuildCategorySummary(_editingFallingObject);
		}
		RefreshCategoryGrid();
		RefreshWeightList();
		RefreshWeightGrid();
	}

	public void RefreshFallingObjectEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()))
		{
			MarkCurrentResourceDirty();
			CurrentResource?.EmitChanged();
			ScheduleFallingObjectSave();
			RebuildCurrentFallingObjectEditor();
		}
	}

	public void RefreshWeightItemEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()))
		{
			MarkCurrentResourceDirty();
			CurrentResource?.EmitChanged();
			ScheduleFallingObjectSave();
			RebuildCurrentFallingObjectEditor();
		}
	}

	private void RebuildCurrentFallingObjectEditor()
	{
		if (CanvasGrid == null)
		{
			return;
		}
		Resource currentResource = CurrentResource;
		if (currentResource == null)
		{
			return;
		}
		DisposeVisualPropertyBindings();
		foreach (Node child in CanvasGrid.GetChildren())
		{
			CanvasGrid.RemoveChild(child);
			child.QueueFree();
		}
		if (!(currentResource is FallingObjectConfig fallingObjectConfig))
		{
			if (currentResource is FallingObjectWeightItemConfig fallingObjectWeightItemConfig)
			{
				_editingFallingObject = null;
				_editingWeightItem = fallingObjectWeightItemConfig;
				RenderWeightItemPreview(fallingObjectWeightItemConfig);
			}
		}
		else
		{
			_editingFallingObject = fallingObjectConfig;
			_editingWeightItem = null;
			_selectedWeightIndex = Math.Min(_selectedWeightIndex, (fallingObjectConfig.weightItem?.Count ?? 0) - 1);
			RenderFallingObjectEditor(fallingObjectConfig);
		}
	}

	private void SavePendingFallingObjectResource()
	{
		Resource currentResource = CurrentResource;
		if (!(currentResource is FallingObjectConfig resource))
		{
			if (currentResource is FallingObjectWeightItemConfig resource2)
			{
				SaveFallingObjectResource(resource2);
			}
		}
		else
		{
			SaveFallingObjectResource(resource);
		}
	}

	private void SaveFallingObjectResource(Resource resource)
	{
		string currentResourcePath = CurrentResourcePath;
		if (string.IsNullOrWhiteSpace(currentResourcePath) || !GodotObject.IsInstanceValid(resource))
		{
			return;
		}
		Error error = ResourceSaver.Save(resource, currentResourcePath, ResourceSaver.SaverFlags.None);
		if (error != Error.Ok)
		{
			GD.PushWarning($"FallingObject editor save failed: {error} {currentResourcePath}");
		}
		else
		{
			string text = XWFileSystem.GetSingleton()?.ProjectFolderPath ?? "";
			if (!string.IsNullOrWhiteSpace(text))
			{
				XWModManifestSyncService.RegisterPath(text, currentResourcePath);
			}
		}
	}

	private void RunSinglePickPreview()
	{
		int num = PickWeightedIndex();
		if (num < 0)
		{
			if (GodotObject.IsInstanceValid(_lastPickLabel))
			{
				_lastPickLabel.Text = "无法抽取：总权重为 0";
			}
			return;
		}
		RecordSimulationHit(num);
		FallingObjectWeightItemConfig fallingObjectWeightItemConfig = _editingFallingObject.weightItem[num];
		if (GodotObject.IsInstanceValid(_lastPickLabel))
		{
			_lastPickLabel.Text = ((fallingObjectWeightItemConfig != null && fallingObjectWeightItemConfig.empty) ? "本次结果：空掉落" : $"本次结果：{fallingObjectWeightItemConfig?.item}");
		}
		_selectedWeightIndex = num;
		RefreshWeightList();
		RefreshSelectedWeightControls();
		RefreshWeightGrid();
		RefreshSimulationGrid();
		BindSelectedWeightItemFields();
		SpawnFallingObjectPreview(fallingObjectWeightItemConfig, _dropPreviewRoot);
	}

	private async void RunDropSequencePreview()
	{
		int generation = ++_previewSequenceGeneration;
		for (int i = 0; i < 8; i++)
		{
			if (generation != _previewSequenceGeneration)
			{
				break;
			}
			if (!IsVisibleInTree())
			{
				break;
			}
			RunSinglePickPreview();
			if (!IsInsideTree())
			{
				break;
			}
			await ToSignal(GetTree().CreateTimer(0.18, processAlways: false), SceneTreeTimer.SignalName.Timeout);
		}
	}

	private void SpawnFallingObjectPreview(FallingObjectWeightItemConfig item, Node2D previewRoot)
	{
		if (!GodotObject.IsInstanceValid(previewRoot) || !GodotObject.IsInstanceValid(item) || item.empty || item.item == ObjectManagerConfig.OBJECT.NOONE)
		{
			return;
		}
		PackedScene poolScene = ObjectManager.GetPoolScene(item.item);
		if (!GodotObject.IsInstanceValid(poolScene))
		{
			if (GodotObject.IsInstanceValid(_lastPickLabel))
			{
				_lastPickLabel.Text = $"{item.item} 的对象池场景尚未加载";
			}
			return;
		}
		try
		{
			Node node = poolScene.Instantiate(PackedScene.GenEditState.Disabled);
			Node2D node2D = new Node2D
			{
				Name = $"FallingObject_{item.item}",
				Position = new Vector2(_previewRandom.RandfRange(90f, 610f), -55f)
			};
			previewRoot.AddChild(node2D, forceReadableName: false, InternalMode.Disabled);
			node2D.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			if (node is CanvasItem canvasItem)
			{
				canvasItem.Show();
			}
			Tween tween = node2D.CreateTween();
			tween.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
			tween.TweenProperty(node2D, "position:y", _previewRandom.RandfRange(205f, 255f), 0.72);
			tween.SetTrans(Tween.TransitionType.Bounce).SetEase(Tween.EaseType.Out);
			tween.TweenProperty(node2D, "scale", new Vector2(1.06f, 0.94f), 0.12);
			tween.TweenProperty(node2D, "scale", Vector2.One, 0.18);
		}
		catch (Exception ex)
		{
			GD.PushWarning($"Falling object game preview failed: {item.item} {ex.Message}");
		}
	}

	private void ClearDropPreview()
	{
		_previewSequenceGeneration++;
		if (GodotObject.IsInstanceValid(_dropPreviewRoot))
		{
			foreach (Node child in _dropPreviewRoot.GetChildren())
			{
				child.QueueFree();
			}
		}
		ResetSimulationResults();
		if (GodotObject.IsInstanceValid(_lastPickLabel))
		{
			_lastPickLabel.Text = "草坪已清空";
		}
	}

	private void RunBatchPickPreview(int sampleCount)
	{
		ResetSimulationResults();
		int num = Math.Clamp(sampleCount, 1, 100000);
		for (int i = 0; i < num; i++)
		{
			int num2 = PickWeightedIndex();
			if (num2 < 0)
			{
				break;
			}
			RecordSimulationHit(num2);
		}
		if (GodotObject.IsInstanceValid(_lastPickLabel))
		{
			_lastPickLabel.Text = ((_simulationRuns > 0) ? $"已模拟 {_simulationRuns:N0} 次" : "无法模拟：总权重为 0");
		}
		RefreshSimulationGrid();
	}

	private int PickWeightedIndex()
	{
		Array<FallingObjectWeightItemConfig> array = _editingFallingObject?.weightItem;
		int totalWeight = GetTotalWeight(_editingFallingObject);
		if (array == null || array.Count == 0 || totalWeight <= 0)
		{
			return -1;
		}
		int num = _previewRandom.RandiRange(0, totalWeight - 1);
		for (int i = 0; i < array.Count; i++)
		{
			int num2 = Math.Max(0, array[i]?.weight ?? 0);
			if (num < num2)
			{
				return i;
			}
			num -= num2;
		}
		return -1;
	}

	private void RecordSimulationHit(int index)
	{
		_simulationRuns++;
		_simulationHits[index] = _simulationHits.GetValueOrDefault(index) + 1;
	}

	private void ResetSimulationResults()
	{
		_simulationRuns = 0;
		_simulationHits.Clear();
		if (GodotObject.IsInstanceValid(_lastPickLabel))
		{
			_lastPickLabel.Text = "等待模拟";
		}
		RefreshSimulationGrid();
	}

	private void RefreshSimulationGrid()
	{
		if (!GodotObject.IsInstanceValid(_simulationGrid))
		{
			return;
		}
		foreach (Node child in _simulationGrid.GetChildren())
		{
			_simulationGrid.RemoveChild(child);
			child.QueueFree();
		}
		Array<FallingObjectWeightItemConfig> array = _editingFallingObject?.weightItem;
		if (array == null || array.Count == 0)
		{
			_simulationGrid.AddChild(new Label
			{
				Text = "添加权重项后可运行掉落模拟。"
			}, forceReadableName: false, InternalMode.Disabled);
			return;
		}
		int num = Math.Max(1, GetTotalWeight(_editingFallingObject));
		for (int i = 0; i < array.Count; i++)
		{
			FallingObjectWeightItemConfig fallingObjectWeightItemConfig = array[i];
			float expected = (float)Math.Max(0, fallingObjectWeightItemConfig?.weight ?? 0) / (float)num;
			int valueOrDefault = _simulationHits.GetValueOrDefault(i);
			float observed = ((_simulationRuns > 0) ? ((float)valueOrDefault / (float)_simulationRuns) : 0f);
			_simulationGrid.AddChild(CreateSimulationResultCard(fallingObjectWeightItemConfig, expected, observed, valueOrDefault), forceReadableName: false, InternalMode.Disabled);
		}
	}

	private Control CreateSimulationResultCard(FallingObjectWeightItemConfig item, float expected, float observed, int hits)
	{
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.Name = "FallingObjectSimulationResultCard";
		panelContainer.CustomMinimumSize = new Vector2(165f, 58f);
		panelContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		panelContainer.AddThemeStyleboxOverride("panel", CreatePanelStyle((item != null && item.empty) ? new Color(0.24f, 0.17f, 0.14f) : new Color(0.12f, 0.18f, 0.23f), 1, 3));
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 2);
		panelContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(new Label
		{
			Text = FormatWeightItem(item),
			ClipText = true,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(new Label
		{
			Text = ((_simulationRuns > 0) ? $"期望 {expected:P1} · 实测 {observed:P1} ({hits})" : $"期望 {expected:P1} · 尚未模拟"),
			ClipText = true,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		ColorRect node = new ColorRect
		{
			Color = new Color(0.35f, 0.68f, 0.91f),
			CustomMinimumSize = new Vector2(Mathf.Clamp((_simulationRuns > 0) ? observed : expected, 0f, 1f) * 145f, 5f)
		};
		vBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		return panelContainer;
	}

	private void SelectWeightItem(int index)
	{
		_weightItemPropertyBinding?.Dispose();
		_weightItemPropertyBinding = null;
		_selectedWeightIndex = index;
		RefreshSelectedWeightControls();
		RefreshWeightGrid();
		BindSelectedWeightItemFields();
	}

	private void RefreshAll()
	{
		if (_editingFallingObject == null)
		{
			return;
		}
		_updatingControls = true;
		try
		{
			if (GodotObject.IsInstanceValid(_summaryLabel))
			{
				_summaryLabel.Text = BuildSummary(_editingFallingObject);
			}
			if (GodotObject.IsInstanceValid(_pickPreviewLabel))
			{
				_pickPreviewLabel.Text = $"Pick 预览: 总权重 {GetTotalWeight(_editingFallingObject)}, 空项 {CountEmpty(_editingFallingObject)}";
			}
			if (GodotObject.IsInstanceValid(_categorySummaryLabel))
			{
				_categorySummaryLabel.Text = BuildCategorySummary(_editingFallingObject);
			}
			RefreshObjectLibraryForCategory();
			RefreshCategoryGrid();
			RefreshWeightList();
			RefreshSelectedWeightControls();
			RefreshWeightGrid();
			RefreshSimulationGrid();
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void RefreshWeightList()
	{
		if (!GodotObject.IsInstanceValid(_weightList))
		{
			return;
		}
		_weightList.Clear();
		Array<FallingObjectWeightItemConfig> array = _editingFallingObject?.weightItem;
		if (array == null || array.Count == 0)
		{
			_weightList.AddItem("未配置掉落项");
			_selectedWeightIndex = -1;
			return;
		}
		for (int i = 0; i < array.Count; i++)
		{
			FallingObjectWeightItemConfig item = array[i];
			_weightList.AddItem($"{i + 1}. {FormatWeightItem(item)}");
		}
		if (_selectedWeightIndex >= 0 && _selectedWeightIndex < array.Count)
		{
			_weightList.Select(_selectedWeightIndex);
		}
		else
		{
			_selectedWeightIndex = 0;
		}
	}

	private void RefreshSelectedWeightControls()
	{
		FallingObjectWeightItemConfig selectedWeightItem = GetSelectedWeightItem();
		if (selectedWeightItem != null)
		{
			_objectPicker?.SetSelectedId(Convert.ToInt32(selectedWeightItem.item));
			if (GodotObject.IsInstanceValid(_weightSpin))
			{
				_weightSpin.Value = selectedWeightItem.weight;
			}
			if (GodotObject.IsInstanceValid(_emptyCheck))
			{
				_emptyCheck.ButtonPressed = selectedWeightItem.empty;
			}
		}
	}

	private void RefreshWeightGrid()
	{
		if (!GodotObject.IsInstanceValid(_weightGrid))
		{
			return;
		}
		foreach (Node child in _weightGrid.GetChildren())
		{
			_weightGrid.RemoveChild(child);
			child.QueueFree();
		}
		Array<FallingObjectWeightItemConfig> array = _editingFallingObject?.weightItem;
		if (array == null || array.Count == 0)
		{
			_weightGrid.AddChild(CreateWeightCard("未配置", "添加权重项后可预览 Pick 分布", 0f, selected: false), forceReadableName: false, InternalMode.Disabled);
			return;
		}
		int num = Math.Max(1, GetTotalWeight(_editingFallingObject));
		for (int i = 0; i < array.Count; i++)
		{
			FallingObjectWeightItemConfig fallingObjectWeightItemConfig = array[i];
			float num2 = ((fallingObjectWeightItemConfig == null) ? 0f : ((float)fallingObjectWeightItemConfig.weight / (float)num));
			_weightGrid.AddChild(CreateWeightCard(FormatWeightItem(fallingObjectWeightItemConfig), $"{num2:P1}", num2, i == _selectedWeightIndex), forceReadableName: false, InternalMode.Disabled);
		}
	}

	private Control CreateWeightCard(string title, string subtitle, float ratio, bool selected)
	{
		PanelContainer panelContainer = new PanelContainer();
		panelContainer.Name = "FallingObjectWeightCard";
		panelContainer.CustomMinimumSize = new Vector2(210f, 82f);
		panelContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		panelContainer.AddThemeStyleboxOverride("panel", CreatePanelStyle(selected ? new Color(0.18f, 0.3f, 0.46f) : new Color(0.13f, 0.135f, 0.14f), (!selected) ? 1 : 2, 4));
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 4);
		panelContainer.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(new Label
		{
			Text = title,
			ClipText = true,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(new Label
		{
			Text = subtitle,
			ClipText = true,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		}, forceReadableName: false, InternalMode.Disabled);
		ColorRect node = new ColorRect
		{
			Name = "WeightRatioBar",
			Color = new Color(0.42f, 0.7f, 0.35f, 0.95f),
			CustomMinimumSize = new Vector2(Mathf.Clamp(ratio, 0f, 1f) * 180f, 6f)
		};
		vBoxContainer.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		return panelContainer;
	}

	private void SetFallingObjectCategory(string categoryName)
	{
		FallingObjectObjectCategory categoryByName = GetCategoryByName(categoryName);
		_selectedCategory = categoryByName.Name;
		if (GodotObject.IsInstanceValid(_emptyCheck) && _selectedCategory == "空项")
		{
			_emptyCheck.ButtonPressed = true;
		}
		RefreshObjectLibraryForCategory();
		if (GodotObject.IsInstanceValid(_categorySummaryLabel))
		{
			_categorySummaryLabel.Text = BuildCategorySummary(_editingFallingObject);
		}
		RefreshCategoryGrid();
	}

	private void RefreshObjectLibraryForCategory()
	{
		if (GodotObject.IsInstanceValid(_objectPicker))
		{
			ObjectManagerConfig.OBJECT oBJECT = GetSelectedWeightItem()?.item ?? ReadSelectedObject();
			List<XWObjectPoolVisualChoice> list = BuildObjectVisualChoices(GetObjectsForCategory(_selectedCategory));
			int selectedId = Convert.ToInt32(oBJECT);
			if (list.Count > 0 && list.Find((XWObjectPoolVisualChoice choice) => choice.Id == selectedId) == null)
			{
				selectedId = list[0].Id;
			}
			_objectPicker.Configure(list, selectedId);
		}
	}

	private static List<XWObjectPoolVisualChoice> BuildObjectVisualChoices(IEnumerable<ObjectManagerConfig.OBJECT> values)
	{
		List<XWObjectPoolVisualChoice> list = new List<XWObjectPoolVisualChoice>();
		foreach (ObjectManagerConfig.OBJECT value in values)
		{
			string objectCategory = GetObjectCategory(value);
			list.Add(new XWObjectPoolVisualChoice(Convert.ToInt32(value), value.ToString(), objectCategory, ResourceLoader.Load<Texture2D>(GetObjectCategoryIconPath(objectCategory), null, ResourceLoader.CacheMode.Reuse)));
		}
		return list;
	}

	private void RefreshCategoryGrid()
	{
		if (!GodotObject.IsInstanceValid(_categoryGrid))
		{
			return;
		}
		foreach (Node child in _categoryGrid.GetChildren())
		{
			_categoryGrid.RemoveChild(child);
			child.QueueFree();
		}
		FallingObjectObjectCategory[] fallingObjectCategories = FallingObjectCategories;
		foreach (FallingObjectObjectCategory category in fallingObjectCategories)
		{
			_categoryGrid.AddChild(CreateCategoryCard(category), forceReadableName: false, InternalMode.Disabled);
		}
	}

	private IEnumerable<ObjectManagerConfig.OBJECT> GetObjectsForCategory(string categoryName)
	{
		ObjectManagerConfig.OBJECT[] values = Enum.GetValues<ObjectManagerConfig.OBJECT>();
		foreach (ObjectManagerConfig.OBJECT oBJECT in values)
		{
			if (oBJECT != ObjectManagerConfig.OBJECT.MAX && (categoryName == "全部" || GetObjectCategory(oBJECT) == categoryName))
			{
				yield return oBJECT;
			}
		}
	}

	private static string GetObjectCategory(ObjectManagerConfig.OBJECT value)
	{
		FallingObjectObjectCategory[] fallingObjectCategories = FallingObjectCategories;
		foreach (FallingObjectObjectCategory fallingObjectObjectCategory in fallingObjectCategories)
		{
			string name = fallingObjectObjectCategory.Name;
			bool flag = ((name == "全部" || name == "其它") ? true : false);
			if (!flag && fallingObjectObjectCategory.Contains(value))
			{
				return fallingObjectObjectCategory.Name;
			}
		}
		return "其它";
	}

	private static string GetObjectCategoryIconPath(string categoryName)
	{
		return categoryName switch
		{
			"全部" => "res://addons/ModEditor/Icons/AssetLib.svg", 
			"空项" => "res://addons/ModEditor/Icons/Clear.svg", 
			"阳光" => "res://addons/ModEditor/Icons/ResourceCollectable.svg", 
			"货币" => "res://addons/ModEditor/Icons/Favorites.svg", 
			"战斗" => "res://addons/ModEditor/Icons/ResourceProjectile.svg", 
			"特效" => "res://addons/ModEditor/Icons/ResourceAnimation.svg", 
			_ => "res://addons/ModEditor/Icons/ResourceFallingObject.svg", 
		};
	}

	private string BuildCategorySummary(FallingObjectConfig fallingObject)
	{
		FallingObjectObjectCategory categoryByName = GetCategoryByName(_selectedCategory);
		int num = 0;
		foreach (ObjectManagerConfig.OBJECT item in GetObjectsForCategory(_selectedCategory))
		{
			_ = item;
			num++;
		}
		int num2 = 0;
		int num3 = 0;
		if (fallingObject?.weightItem != null)
		{
			foreach (FallingObjectWeightItemConfig item2 in fallingObject.weightItem)
			{
				if (item2 != null)
				{
					string text = (item2.empty ? "空项" : GetObjectCategory(item2.item));
					if (!(_selectedCategory != "全部") || !(text != _selectedCategory))
					{
						num2++;
						num3 += Math.Max(0, item2.weight);
					}
				}
			}
		}
		return $"当前分类: {_selectedCategory}  可选 {num}  已用 {num2}  权重 {num3}  {categoryByName?.Description ?? ""}";
	}

	private Control CreateCategoryCard(FallingObjectObjectCategory category)
	{
		string categoryName = category?.Name ?? "其它";
		int num = 0;
		int num2 = 0;
		if (_editingFallingObject?.weightItem != null)
		{
			foreach (FallingObjectWeightItemConfig item in _editingFallingObject.weightItem)
			{
				if (item != null)
				{
					string text = (item.empty ? "空项" : GetObjectCategory(item.item));
					if (!(categoryName != "全部") || !(text != categoryName))
					{
						num++;
						num2 += Math.Max(0, item.weight);
					}
				}
			}
		}
		if (_visualChoiceCardScene == null)
		{
			_visualChoiceCardScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWGameVisualChoiceCard.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		XWGameVisualChoiceCard xWGameVisualChoiceCard = _visualChoiceCardScene?.Instantiate<XWGameVisualChoiceCard>(PackedScene.GenEditState.Disabled);
		if (!GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
		{
			return new Label
			{
				Text = categoryName
			};
		}
		xWGameVisualChoiceCard.Name = "FallingObjectCategoryCard";
		xWGameVisualChoiceCard.CustomMinimumSize = new Vector2(142f, 64f);
		xWGameVisualChoiceCard.Configure(categoryName, categoryName, $"{num} 项 / 权重 {num2}", ResourceLoader.Load<Texture2D>(GetObjectCategoryIconPath(categoryName), null, ResourceLoader.CacheMode.Reuse), categoryName == _selectedCategory);
		xWGameVisualChoiceCard.TooltipText = category?.Description ?? string.Empty;
		xWGameVisualChoiceCard.Pressed += () =>
		{
			SetFallingObjectCategory(categoryName);
		};
		return xWGameVisualChoiceCard;
	}

	private static FallingObjectObjectCategory GetCategoryByName(string categoryName)
	{
		FallingObjectObjectCategory[] fallingObjectCategories = FallingObjectCategories;
		foreach (FallingObjectObjectCategory fallingObjectObjectCategory in fallingObjectCategories)
		{
			if (fallingObjectObjectCategory.Name == categoryName)
			{
				return fallingObjectObjectCategory;
			}
		}
		return FallingObjectCategories[0];
	}

	private ObjectManagerConfig.OBJECT ReadSelectedObject()
	{
		if (!GodotObject.IsInstanceValid(_objectPicker))
		{
			return ObjectManagerConfig.OBJECT.NOONE;
		}
		return (ObjectManagerConfig.OBJECT)_objectPicker.SelectedChoiceId;
	}

	private FallingObjectWeightItemConfig GetSelectedWeightItem()
	{
		Array<FallingObjectWeightItemConfig> array = _editingFallingObject?.weightItem;
		if (array == null || _selectedWeightIndex < 0 || _selectedWeightIndex >= array.Count)
		{
			return null;
		}
		return array[_selectedWeightIndex];
	}

	private void AddSummaryRows(FallingObjectConfig fallingObject)
	{
		AddItemIfMissing(PreviewList, BuildSummary(fallingObject));
		AddItemIfMissing(TimelineList, $"weightItem -> {fallingObject.weightItem?.Count ?? 0}");
		AddItemIfMissing(TimelineList, $"Pick -> totalWeight={GetTotalWeight(fallingObject)}");
		AddItemIfMissing(GraphList, "掉落物 -> weightItem -> item / weight / empty -> Pick");
		AddItemIfMissing(ReferenceList, "掉落物资源 -> " + CurrentResourcePath);
	}

	private static string BuildSummary(FallingObjectConfig fallingObject)
	{
		string value = (string.IsNullOrWhiteSpace(fallingObject?.ResourceName) ? "未命名掉落表" : fallingObject.ResourceName);
		return $"{value}: 条目 {(fallingObject?.weightItem?.Count).GetValueOrDefault()}, 总权重 {GetTotalWeight(fallingObject)}";
	}

	private static int GetTotalWeight(FallingObjectConfig fallingObject)
	{
		if (fallingObject?.weightItem == null)
		{
			return 0;
		}
		int num = 0;
		foreach (FallingObjectWeightItemConfig item in fallingObject.weightItem)
		{
			num += Math.Max(0, item?.weight ?? 0);
		}
		return num;
	}

	private static int CountEmpty(FallingObjectConfig fallingObject)
	{
		if (fallingObject?.weightItem == null)
		{
			return 0;
		}
		int num = 0;
		foreach (FallingObjectWeightItemConfig item in fallingObject.weightItem)
		{
			if (item != null && item.empty)
			{
				num++;
			}
		}
		return num;
	}

	private static string FormatWeightItem(FallingObjectWeightItemConfig item)
	{
		if (item == null)
		{
			return "空";
		}
		if (item.empty)
		{
			return $"[空项] empty weight={item.weight}";
		}
		return $"[{GetObjectCategory(item.item)}] {item.item} weight={item.weight}";
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(61)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasCompleteFallingObjectVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeVisualPropertyBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderWeightItemPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "weightItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindWeightItemDirectFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "preview", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Object, "weightItem", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.MountWeightItemObjectLibrary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Int, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateWeightItemPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderFallingObjectEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fallingObject", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindFallingObjectLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "fallingObject", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.MountFallingObjectLibrary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateFallingObjectPreviewPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fallingObject", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddWeightItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RemoveSelectedWeightItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MoveSelectedWeightItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DuplicateWeightItems, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFallingObjectWeightItems, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "nextItems", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetWeightItemProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.BindSelectedWeightItemFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnFallingObjectVisualPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnFallingObjectChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnWeightItemVisualPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ScheduleFallingObjectSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshFallingObjectVisualsAfterWeightItemEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshFallingObjectEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshWeightItemEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildCurrentFallingObjectEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SavePendingFallingObjectResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveFallingObjectResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RunSinglePickPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunDropSequencePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SpawnFallingObjectPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Object, "previewRoot", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false)
			}, null),
			new MethodInfo(MethodName.ClearDropPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RunBatchPickPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "sampleCount", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PickWeightedIndex, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RecordSimulationHit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResetSimulationResults, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshSimulationGrid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateSimulationResultCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Float, "expected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "observed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "hits", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SelectWeightItem, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAll, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshWeightList, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshSelectedWeightControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshWeightGrid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateWeightCard, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "subtitle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "ratio", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "selected", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetFallingObjectCategory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "categoryName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshObjectLibraryForCategory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshCategoryGrid, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetObjectCategory, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetObjectCategoryIconPath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "categoryName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCategorySummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fallingObject", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadSelectedObject, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetSelectedWeightItem, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fallingObject", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fallingObject", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetTotalWeight, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fallingObject", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CountEmpty, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "fallingObject", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatWeightItem, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "item", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.HasCompleteFallingObjectVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteFallingObjectVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.DisposeVisualPropertyBindings && args.Count == 0)
		{
			DisposeVisualPropertyBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.RenderWeightItemPreview && args.Count == 1)
		{
			RenderWeightItemPreview(VariantUtils.ConvertTo<FallingObjectWeightItemConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindWeightItemDirectFields && args.Count == 2)
		{
			BindWeightItemDirectFields(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<FallingObjectWeightItemConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MountWeightItemObjectLibrary && args.Count == 2)
		{
			MountWeightItemObjectLibrary(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateWeightItemPreview && args.Count == 0)
		{
			UpdateWeightItemPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RenderFallingObjectEditor && args.Count == 1)
		{
			RenderFallingObjectEditor(VariantUtils.ConvertTo<FallingObjectConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindFallingObjectLayout && args.Count == 2)
		{
			BindFallingObjectLayout(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<FallingObjectConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MountFallingObjectLibrary && args.Count == 1)
		{
			MountFallingObjectLibrary(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateFallingObjectPreviewPanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateFallingObjectPreviewPanel(VariantUtils.ConvertTo<FallingObjectConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.AddWeightItem && args.Count == 0)
		{
			AddWeightItem();
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedWeightItem && args.Count == 0)
		{
			RemoveSelectedWeightItem();
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSelectedWeightItem && args.Count == 1)
		{
			MoveSelectedWeightItem(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DuplicateWeightItems && args.Count == 1)
		{
			Array<FallingObjectWeightItemConfig> array = DuplicateWeightItems(VariantUtils.ConvertToArray<FallingObjectWeightItemConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.SetFallingObjectWeightItems && args.Count == 2)
		{
			SetFallingObjectWeightItems(VariantUtils.ConvertToArray<FallingObjectWeightItemConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetWeightItemProperty && args.Count == 2)
		{
			SetWeightItemProperty(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindSelectedWeightItemFields && args.Count == 0)
		{
			BindSelectedWeightItemFields();
			ret = default;
			return true;
		}
		if (method == MethodName.OnFallingObjectVisualPropertyEdited && args.Count == 1)
		{
			OnFallingObjectVisualPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnFallingObjectChanged && args.Count == 0)
		{
			OnFallingObjectChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.OnWeightItemVisualPropertyEdited && args.Count == 1)
		{
			OnWeightItemVisualPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleFallingObjectSave && args.Count == 0)
		{
			ScheduleFallingObjectSave();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshFallingObjectVisualsAfterWeightItemEdit && args.Count == 0)
		{
			RefreshFallingObjectVisualsAfterWeightItemEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshFallingObjectEditorFromHistory && args.Count == 0)
		{
			RefreshFallingObjectEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshWeightItemEditorFromHistory && args.Count == 0)
		{
			RefreshWeightItemEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildCurrentFallingObjectEditor && args.Count == 0)
		{
			RebuildCurrentFallingObjectEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.SavePendingFallingObjectResource && args.Count == 0)
		{
			SavePendingFallingObjectResource();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveFallingObjectResource && args.Count == 1)
		{
			SaveFallingObjectResource(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RunSinglePickPreview && args.Count == 0)
		{
			RunSinglePickPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RunDropSequencePreview && args.Count == 0)
		{
			RunDropSequencePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.SpawnFallingObjectPreview && args.Count == 2)
		{
			SpawnFallingObjectPreview(VariantUtils.ConvertTo<FallingObjectWeightItemConfig>(in args[0]), VariantUtils.ConvertTo<Node2D>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearDropPreview && args.Count == 0)
		{
			ClearDropPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RunBatchPickPreview && args.Count == 1)
		{
			RunBatchPickPreview(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PickWeightedIndex && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<int>(PickWeightedIndex());
			return true;
		}
		if (method == MethodName.RecordSimulationHit && args.Count == 1)
		{
			RecordSimulationHit(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ResetSimulationResults && args.Count == 0)
		{
			ResetSimulationResults();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSimulationGrid && args.Count == 0)
		{
			RefreshSimulationGrid();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSimulationResultCard && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateSimulationResultCard(VariantUtils.ConvertTo<FallingObjectWeightItemConfig>(in args[0]), VariantUtils.ConvertTo<float>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<int>(in args[3])));
			return true;
		}
		if (method == MethodName.SelectWeightItem && args.Count == 1)
		{
			SelectWeightItem(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAll && args.Count == 0)
		{
			RefreshAll();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshWeightList && args.Count == 0)
		{
			RefreshWeightList();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSelectedWeightControls && args.Count == 0)
		{
			RefreshSelectedWeightControls();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshWeightGrid && args.Count == 0)
		{
			RefreshWeightGrid();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateWeightCard && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateWeightCard(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3])));
			return true;
		}
		if (method == MethodName.SetFallingObjectCategory && args.Count == 1)
		{
			SetFallingObjectCategory(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshObjectLibraryForCategory && args.Count == 0)
		{
			RefreshObjectLibraryForCategory();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCategoryGrid && args.Count == 0)
		{
			RefreshCategoryGrid();
			ret = default;
			return true;
		}
		if (method == MethodName.GetObjectCategory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetObjectCategory(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.GetObjectCategoryIconPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetObjectCategoryIconPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCategorySummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCategorySummary(VariantUtils.ConvertTo<FallingObjectConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadSelectedObject && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ObjectManagerConfig.OBJECT>(ReadSelectedObject());
			return true;
		}
		if (method == MethodName.GetSelectedWeightItem && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<FallingObjectWeightItemConfig>(GetSelectedWeightItem());
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 1)
		{
			AddSummaryRows(VariantUtils.ConvertTo<FallingObjectConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSummary(VariantUtils.ConvertTo<FallingObjectConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTotalWeight && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetTotalWeight(VariantUtils.ConvertTo<FallingObjectConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.CountEmpty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountEmpty(VariantUtils.ConvertTo<FallingObjectConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatWeightItem && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatWeightItem(VariantUtils.ConvertTo<FallingObjectWeightItemConfig>(in args[0])));
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
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasCompleteFallingObjectVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteFallingObjectVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.DuplicateWeightItems && args.Count == 1)
		{
			Array<FallingObjectWeightItemConfig> array = DuplicateWeightItems(VariantUtils.ConvertToArray<FallingObjectWeightItemConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.GetObjectCategory && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetObjectCategory(VariantUtils.ConvertTo<ObjectManagerConfig.OBJECT>(in args[0])));
			return true;
		}
		if (method == MethodName.GetObjectCategoryIconPath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetObjectCategoryIconPath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSummary(VariantUtils.ConvertTo<FallingObjectConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetTotalWeight && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(GetTotalWeight(VariantUtils.ConvertTo<FallingObjectConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.CountEmpty && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<int>(CountEmpty(VariantUtils.ConvertTo<FallingObjectConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatWeightItem && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatWeightItem(VariantUtils.ConvertTo<FallingObjectWeightItemConfig>(in args[0])));
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
		if (method == MethodName.HasCompleteFallingObjectVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.DisposeVisualPropertyBindings)
		{
			return true;
		}
		if (method == MethodName.RenderWeightItemPreview)
		{
			return true;
		}
		if (method == MethodName.BindWeightItemDirectFields)
		{
			return true;
		}
		if (method == MethodName.MountWeightItemObjectLibrary)
		{
			return true;
		}
		if (method == MethodName.UpdateWeightItemPreview)
		{
			return true;
		}
		if (method == MethodName.RenderFallingObjectEditor)
		{
			return true;
		}
		if (method == MethodName.BindFallingObjectLayout)
		{
			return true;
		}
		if (method == MethodName.MountFallingObjectLibrary)
		{
			return true;
		}
		if (method == MethodName.CreateFallingObjectPreviewPanel)
		{
			return true;
		}
		if (method == MethodName.AddWeightItem)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedWeightItem)
		{
			return true;
		}
		if (method == MethodName.MoveSelectedWeightItem)
		{
			return true;
		}
		if (method == MethodName.DuplicateWeightItems)
		{
			return true;
		}
		if (method == MethodName.SetFallingObjectWeightItems)
		{
			return true;
		}
		if (method == MethodName.SetWeightItemProperty)
		{
			return true;
		}
		if (method == MethodName.BindSelectedWeightItemFields)
		{
			return true;
		}
		if (method == MethodName.OnFallingObjectVisualPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.OnFallingObjectChanged)
		{
			return true;
		}
		if (method == MethodName.OnWeightItemVisualPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.ScheduleFallingObjectSave)
		{
			return true;
		}
		if (method == MethodName.RefreshFallingObjectVisualsAfterWeightItemEdit)
		{
			return true;
		}
		if (method == MethodName.RefreshFallingObjectEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.RefreshWeightItemEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.RebuildCurrentFallingObjectEditor)
		{
			return true;
		}
		if (method == MethodName.SavePendingFallingObjectResource)
		{
			return true;
		}
		if (method == MethodName.SaveFallingObjectResource)
		{
			return true;
		}
		if (method == MethodName.RunSinglePickPreview)
		{
			return true;
		}
		if (method == MethodName.RunDropSequencePreview)
		{
			return true;
		}
		if (method == MethodName.SpawnFallingObjectPreview)
		{
			return true;
		}
		if (method == MethodName.ClearDropPreview)
		{
			return true;
		}
		if (method == MethodName.RunBatchPickPreview)
		{
			return true;
		}
		if (method == MethodName.PickWeightedIndex)
		{
			return true;
		}
		if (method == MethodName.RecordSimulationHit)
		{
			return true;
		}
		if (method == MethodName.ResetSimulationResults)
		{
			return true;
		}
		if (method == MethodName.RefreshSimulationGrid)
		{
			return true;
		}
		if (method == MethodName.CreateSimulationResultCard)
		{
			return true;
		}
		if (method == MethodName.SelectWeightItem)
		{
			return true;
		}
		if (method == MethodName.RefreshAll)
		{
			return true;
		}
		if (method == MethodName.RefreshWeightList)
		{
			return true;
		}
		if (method == MethodName.RefreshSelectedWeightControls)
		{
			return true;
		}
		if (method == MethodName.RefreshWeightGrid)
		{
			return true;
		}
		if (method == MethodName.CreateWeightCard)
		{
			return true;
		}
		if (method == MethodName.SetFallingObjectCategory)
		{
			return true;
		}
		if (method == MethodName.RefreshObjectLibraryForCategory)
		{
			return true;
		}
		if (method == MethodName.RefreshCategoryGrid)
		{
			return true;
		}
		if (method == MethodName.GetObjectCategory)
		{
			return true;
		}
		if (method == MethodName.GetObjectCategoryIconPath)
		{
			return true;
		}
		if (method == MethodName.BuildCategorySummary)
		{
			return true;
		}
		if (method == MethodName.ReadSelectedObject)
		{
			return true;
		}
		if (method == MethodName.GetSelectedWeightItem)
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
		if (method == MethodName.GetTotalWeight)
		{
			return true;
		}
		if (method == MethodName.CountEmpty)
		{
			return true;
		}
		if (method == MethodName.FormatWeightItem)
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
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editingFallingObject)
		{
			_editingFallingObject = VariantUtils.ConvertTo<FallingObjectConfig>(in value);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			_summaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._pickPreviewLabel)
		{
			_pickPreviewLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._lastPickLabel)
		{
			_lastPickLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._categorySummaryLabel)
		{
			_categorySummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._weightList)
		{
			_weightList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._weightGrid)
		{
			_weightGrid = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName._simulationGrid)
		{
			_simulationGrid = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName._dropPreviewRoot)
		{
			_dropPreviewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._categoryGrid)
		{
			_categoryGrid = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._objectPicker)
		{
			_objectPicker = VariantUtils.ConvertTo<XWObjectPoolVisualPicker>(in value);
			return true;
		}
		if (name == PropertyName._weightSpin)
		{
			_weightSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._emptyCheck)
		{
			_emptyCheck = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._selectedCategory)
		{
			_selectedCategory = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		if (name == PropertyName._selectedWeightIndex)
		{
			_selectedWeightIndex = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._simulationRuns)
		{
			_simulationRuns = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._fallingObjectSaveTimer)
		{
			_fallingObjectSaveTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._editingWeightItem)
		{
			_editingWeightItem = VariantUtils.ConvertTo<FallingObjectWeightItemConfig>(in value);
			return true;
		}
		if (name == PropertyName._weightItemComparisonSpin)
		{
			_weightItemComparisonSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._weightItemProbability)
		{
			_weightItemProbability = VariantUtils.ConvertTo<ProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._weightItemProbabilityLabel)
		{
			_weightItemProbabilityLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._weightItemTitleLabel)
		{
			_weightItemTitleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._weightItemStatusLabel)
		{
			_weightItemStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._weightItemDropRoot)
		{
			_weightItemDropRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._weightItemObjectPicker)
		{
			_weightItemObjectPicker = VariantUtils.ConvertTo<XWObjectPoolVisualPicker>(in value);
			return true;
		}
		if (name == PropertyName._weightItemWeightSpin)
		{
			_weightItemWeightSpin = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._weightItemEmptyCheck)
		{
			_weightItemEmptyCheck = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._simulationViewport)
		{
			_simulationViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._weightItemPreviewViewport)
		{
			_weightItemPreviewViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._previewSequenceGeneration)
		{
			_previewSequenceGeneration = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		int from;
		if (name == PropertyName.FallingCategoryVisualCardCount)
		{
			from = FallingCategoryVisualCardCount;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		bool from2;
		if (name == PropertyName.HasFallingObjectLibrarySearchAndPaging)
		{
			from2 = HasFallingObjectLibrarySearchAndPaging;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.HasWeightItemLibrarySearchAndPaging)
		{
			from2 = HasWeightItemLibrarySearchAndPaging;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.PreviewSequenceGeneration)
		{
			from = PreviewSequenceGeneration;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsFallingPreviewRendering)
		{
			from2 = IsFallingPreviewRendering;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._editingFallingObject)
		{
			value = VariantUtils.CreateFrom(in _editingFallingObject);
			return true;
		}
		if (name == PropertyName._summaryLabel)
		{
			value = VariantUtils.CreateFrom(in _summaryLabel);
			return true;
		}
		if (name == PropertyName._pickPreviewLabel)
		{
			value = VariantUtils.CreateFrom(in _pickPreviewLabel);
			return true;
		}
		if (name == PropertyName._lastPickLabel)
		{
			value = VariantUtils.CreateFrom(in _lastPickLabel);
			return true;
		}
		if (name == PropertyName._categorySummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _categorySummaryLabel);
			return true;
		}
		if (name == PropertyName._weightList)
		{
			value = VariantUtils.CreateFrom(in _weightList);
			return true;
		}
		if (name == PropertyName._weightGrid)
		{
			value = VariantUtils.CreateFrom(in _weightGrid);
			return true;
		}
		if (name == PropertyName._simulationGrid)
		{
			value = VariantUtils.CreateFrom(in _simulationGrid);
			return true;
		}
		if (name == PropertyName._dropPreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _dropPreviewRoot);
			return true;
		}
		if (name == PropertyName._categoryGrid)
		{
			value = VariantUtils.CreateFrom(in _categoryGrid);
			return true;
		}
		if (name == PropertyName._objectPicker)
		{
			value = VariantUtils.CreateFrom(in _objectPicker);
			return true;
		}
		if (name == PropertyName._weightSpin)
		{
			value = VariantUtils.CreateFrom(in _weightSpin);
			return true;
		}
		if (name == PropertyName._emptyCheck)
		{
			value = VariantUtils.CreateFrom(in _emptyCheck);
			return true;
		}
		if (name == PropertyName._selectedCategory)
		{
			value = VariantUtils.CreateFrom(in _selectedCategory);
			return true;
		}
		if (name == PropertyName._selectedWeightIndex)
		{
			value = VariantUtils.CreateFrom(in _selectedWeightIndex);
			return true;
		}
		if (name == PropertyName._simulationRuns)
		{
			value = VariantUtils.CreateFrom(in _simulationRuns);
			return true;
		}
		if (name == PropertyName._previewRandom)
		{
			value = VariantUtils.CreateFrom(in _previewRandom);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._fallingObjectSaveTimer)
		{
			value = VariantUtils.CreateFrom(in _fallingObjectSaveTimer);
			return true;
		}
		if (name == PropertyName._editingWeightItem)
		{
			value = VariantUtils.CreateFrom(in _editingWeightItem);
			return true;
		}
		if (name == PropertyName._weightItemComparisonSpin)
		{
			value = VariantUtils.CreateFrom(in _weightItemComparisonSpin);
			return true;
		}
		if (name == PropertyName._weightItemProbability)
		{
			value = VariantUtils.CreateFrom(in _weightItemProbability);
			return true;
		}
		if (name == PropertyName._weightItemProbabilityLabel)
		{
			value = VariantUtils.CreateFrom(in _weightItemProbabilityLabel);
			return true;
		}
		if (name == PropertyName._weightItemTitleLabel)
		{
			value = VariantUtils.CreateFrom(in _weightItemTitleLabel);
			return true;
		}
		if (name == PropertyName._weightItemStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _weightItemStatusLabel);
			return true;
		}
		if (name == PropertyName._weightItemDropRoot)
		{
			value = VariantUtils.CreateFrom(in _weightItemDropRoot);
			return true;
		}
		if (name == PropertyName._weightItemObjectPicker)
		{
			value = VariantUtils.CreateFrom(in _weightItemObjectPicker);
			return true;
		}
		if (name == PropertyName._weightItemWeightSpin)
		{
			value = VariantUtils.CreateFrom(in _weightItemWeightSpin);
			return true;
		}
		if (name == PropertyName._weightItemEmptyCheck)
		{
			value = VariantUtils.CreateFrom(in _weightItemEmptyCheck);
			return true;
		}
		if (name == PropertyName._simulationViewport)
		{
			value = VariantUtils.CreateFrom(in _simulationViewport);
			return true;
		}
		if (name == PropertyName._weightItemPreviewViewport)
		{
			value = VariantUtils.CreateFrom(in _weightItemPreviewViewport);
			return true;
		}
		if (name == PropertyName._previewSequenceGeneration)
		{
			value = VariantUtils.CreateFrom(in _previewSequenceGeneration);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingFallingObject, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._summaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pickPreviewLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lastPickLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._categorySummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulationGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._dropPreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._categoryGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._objectPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._emptyCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._selectedCategory, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._selectedWeightIndex, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._simulationRuns, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewRandom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._fallingObjectSaveTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingWeightItem, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightItemComparisonSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightItemProbability, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightItemProbabilityLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightItemTitleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightItemStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightItemDropRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightItemObjectPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightItemWeightSpin, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightItemEmptyCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._simulationViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightItemPreviewViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._previewSequenceGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.FallingCategoryVisualCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasFallingObjectLibrarySearchAndPaging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.HasWeightItemLibrarySearchAndPaging, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.PreviewSequenceGeneration, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsFallingPreviewRendering, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingFallingObject, Variant.From(in _editingFallingObject));
		info.AddProperty(PropertyName._summaryLabel, Variant.From(in _summaryLabel));
		info.AddProperty(PropertyName._pickPreviewLabel, Variant.From(in _pickPreviewLabel));
		info.AddProperty(PropertyName._lastPickLabel, Variant.From(in _lastPickLabel));
		info.AddProperty(PropertyName._categorySummaryLabel, Variant.From(in _categorySummaryLabel));
		info.AddProperty(PropertyName._weightList, Variant.From(in _weightList));
		info.AddProperty(PropertyName._weightGrid, Variant.From(in _weightGrid));
		info.AddProperty(PropertyName._simulationGrid, Variant.From(in _simulationGrid));
		info.AddProperty(PropertyName._dropPreviewRoot, Variant.From(in _dropPreviewRoot));
		info.AddProperty(PropertyName._categoryGrid, Variant.From(in _categoryGrid));
		info.AddProperty(PropertyName._objectPicker, Variant.From(in _objectPicker));
		info.AddProperty(PropertyName._weightSpin, Variant.From(in _weightSpin));
		info.AddProperty(PropertyName._emptyCheck, Variant.From(in _emptyCheck));
		info.AddProperty(PropertyName._selectedCategory, Variant.From(in _selectedCategory));
		info.AddProperty(PropertyName._selectedWeightIndex, Variant.From(in _selectedWeightIndex));
		info.AddProperty(PropertyName._simulationRuns, Variant.From(in _simulationRuns));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._fallingObjectSaveTimer, Variant.From(in _fallingObjectSaveTimer));
		info.AddProperty(PropertyName._editingWeightItem, Variant.From(in _editingWeightItem));
		info.AddProperty(PropertyName._weightItemComparisonSpin, Variant.From(in _weightItemComparisonSpin));
		info.AddProperty(PropertyName._weightItemProbability, Variant.From(in _weightItemProbability));
		info.AddProperty(PropertyName._weightItemProbabilityLabel, Variant.From(in _weightItemProbabilityLabel));
		info.AddProperty(PropertyName._weightItemTitleLabel, Variant.From(in _weightItemTitleLabel));
		info.AddProperty(PropertyName._weightItemStatusLabel, Variant.From(in _weightItemStatusLabel));
		info.AddProperty(PropertyName._weightItemDropRoot, Variant.From(in _weightItemDropRoot));
		info.AddProperty(PropertyName._weightItemObjectPicker, Variant.From(in _weightItemObjectPicker));
		info.AddProperty(PropertyName._weightItemWeightSpin, Variant.From(in _weightItemWeightSpin));
		info.AddProperty(PropertyName._weightItemEmptyCheck, Variant.From(in _weightItemEmptyCheck));
		info.AddProperty(PropertyName._simulationViewport, Variant.From(in _simulationViewport));
		info.AddProperty(PropertyName._weightItemPreviewViewport, Variant.From(in _weightItemPreviewViewport));
		info.AddProperty(PropertyName._previewSequenceGeneration, Variant.From(in _previewSequenceGeneration));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingFallingObject, out var value))
		{
			_editingFallingObject = value.As<FallingObjectConfig>();
		}
		if (info.TryGetProperty(PropertyName._summaryLabel, out var value2))
		{
			_summaryLabel = value2.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._pickPreviewLabel, out var value3))
		{
			_pickPreviewLabel = value3.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._lastPickLabel, out var value4))
		{
			_lastPickLabel = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._categorySummaryLabel, out var value5))
		{
			_categorySummaryLabel = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._weightList, out var value6))
		{
			_weightList = value6.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._weightGrid, out var value7))
		{
			_weightGrid = value7.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName._simulationGrid, out var value8))
		{
			_simulationGrid = value8.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName._dropPreviewRoot, out var value9))
		{
			_dropPreviewRoot = value9.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._categoryGrid, out var value10))
		{
			_categoryGrid = value10.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._objectPicker, out var value11))
		{
			_objectPicker = value11.As<XWObjectPoolVisualPicker>();
		}
		if (info.TryGetProperty(PropertyName._weightSpin, out var value12))
		{
			_weightSpin = value12.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._emptyCheck, out var value13))
		{
			_emptyCheck = value13.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._selectedCategory, out var value14))
		{
			_selectedCategory = value14.As<string>();
		}
		if (info.TryGetProperty(PropertyName._selectedWeightIndex, out var value15))
		{
			_selectedWeightIndex = value15.As<int>();
		}
		if (info.TryGetProperty(PropertyName._simulationRuns, out var value16))
		{
			_simulationRuns = value16.As<int>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value17))
		{
			_updatingControls = value17.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._fallingObjectSaveTimer, out var value18))
		{
			_fallingObjectSaveTimer = value18.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._editingWeightItem, out var value19))
		{
			_editingWeightItem = value19.As<FallingObjectWeightItemConfig>();
		}
		if (info.TryGetProperty(PropertyName._weightItemComparisonSpin, out var value20))
		{
			_weightItemComparisonSpin = value20.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._weightItemProbability, out var value21))
		{
			_weightItemProbability = value21.As<ProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._weightItemProbabilityLabel, out var value22))
		{
			_weightItemProbabilityLabel = value22.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._weightItemTitleLabel, out var value23))
		{
			_weightItemTitleLabel = value23.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._weightItemStatusLabel, out var value24))
		{
			_weightItemStatusLabel = value24.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._weightItemDropRoot, out var value25))
		{
			_weightItemDropRoot = value25.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._weightItemObjectPicker, out var value26))
		{
			_weightItemObjectPicker = value26.As<XWObjectPoolVisualPicker>();
		}
		if (info.TryGetProperty(PropertyName._weightItemWeightSpin, out var value27))
		{
			_weightItemWeightSpin = value27.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._weightItemEmptyCheck, out var value28))
		{
			_weightItemEmptyCheck = value28.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._simulationViewport, out var value29))
		{
			_simulationViewport = value29.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._weightItemPreviewViewport, out var value30))
		{
			_weightItemPreviewViewport = value30.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._previewSequenceGeneration, out var value31))
		{
			_previewSequenceGeneration = value31.As<int>();
		}
	}
}
