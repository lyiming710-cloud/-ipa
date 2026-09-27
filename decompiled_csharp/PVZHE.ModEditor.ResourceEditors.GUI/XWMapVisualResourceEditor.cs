using System;
using System.Collections.Generic;
using System.ComponentModel;
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

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWMapVisualResourceEditor.cs")]
public class XWMapVisualResourceEditor : XWGenericVisualResourceEditor
{
	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName HasCompleteMapVisualCoverage = "HasCompleteMapVisualCoverage";

		public new static readonly StringName OnEmbeddedInspectorPropertyChanged = "OnEmbeddedInspectorPropertyChanged";

		public static readonly StringName RenderMapRuleRuntimePreview = "RenderMapRuleRuntimePreview";

		public static readonly StringName UpdateMapRuleRuntimePreview = "UpdateMapRuleRuntimePreview";

		public static readonly StringName RebuildMapRuleGamePreview = "RebuildMapRuleGamePreview";

		public static readonly StringName InstantiateMapRuleCharacter = "InstantiateMapRuleCharacter";

		public static readonly StringName RenderMapEditor = "RenderMapEditor";

		public static readonly StringName BindMapLayout = "BindMapLayout";

		public static readonly StringName OnBrushSettingsChanged = "OnBrushSettingsChanged";

		public static readonly StringName BindMapFields = "BindMapFields";

		public static readonly StringName OpenMapRulePacketTypeCatalog = "OpenMapRulePacketTypeCatalog";

		public static readonly StringName MountMapSegmentedOption = "MountMapSegmentedOption";

		public static readonly StringName EnsureMapVisualPicker = "EnsureMapVisualPicker";

		public static readonly StringName DisposeMapVisualChoices = "DisposeMapVisualChoices";

		public static readonly StringName DisposeMapPropertyBinding = "DisposeMapPropertyBinding";

		public static readonly StringName OnMapVisualPropertyEdited = "OnMapVisualPropertyEdited";

		public static readonly StringName RefreshMapField = "RefreshMapField";

		public static readonly StringName SetMapResourcePath = "SetMapResourcePath";

		public static readonly StringName RebuildLineUseControls = "RebuildLineUseControls";

		public static readonly StringName SetLineEnabled = "SetLineEnabled";

		public static readonly StringName RebuildSpecialRuleControls = "RebuildSpecialRuleControls";

		public static readonly StringName AddSpecialRule = "AddSpecialRule";

		public static readonly StringName ReplaceSpecialRule = "ReplaceSpecialRule";

		public static readonly StringName RemoveSpecialRule = "RemoveSpecialRule";

		public static readonly StringName MoveSpecialRule = "MoveSpecialRule";

		public static readonly StringName SetSpecialRules = "SetSpecialRules";

		public static readonly StringName OpenSpecialRule = "OpenSpecialRule";

		public static readonly StringName RefreshMapEditorFromHistory = "RefreshMapEditorFromHistory";

		public static readonly StringName ClearDynamicChildren = "ClearDynamicChildren";

		public static readonly StringName RebuildMapScenePreview = "RebuildMapScenePreview";

		public static readonly StringName ResetMapPreviewView = "ResetMapPreviewView";

		public static readonly StringName SetMapCanvasZoom = "SetMapCanvasZoom";

		public static readonly StringName UpdateMapCanvasTransform = "UpdateMapCanvasTransform";

		public static readonly StringName MapWorldToView = "MapWorldToView";

		public static readonly StringName MapViewToWorld = "MapViewToWorld";

		public static readonly StringName UpdateMapSceneViewportSize = "UpdateMapSceneViewportSize";

		public static readonly StringName GetMapPreviewViewSize = "GetMapPreviewViewSize";

		public static readonly StringName DrawMapGridOverlay = "DrawMapGridOverlay";

		public static readonly StringName OnMapGridOverlayGuiInput = "OnMapGridOverlayGuiInput";

		public static readonly StringName PanMapCanvasTo = "PanMapCanvasTo";

		public static readonly StringName StartGridBoxSelection = "StartGridBoxSelection";

		public static readonly StringName UpdateGridBoxSelection = "UpdateGridBoxSelection";

		public static readonly StringName FinishGridBoxSelection = "FinishGridBoxSelection";

		public static readonly StringName ApplyBrushToSelection = "ApplyBrushToSelection";

		public static readonly StringName PaintBrushAtCell = "PaintBrushAtCell";

		public static readonly StringName SetCellConfigBrushData = "SetCellConfigBrushData";

		public static readonly StringName BuildBrushGridTypeArray = "BuildBrushGridTypeArray";

		public static readonly StringName ApplySelectedCellConfig = "ApplySelectedCellConfig";

		public static readonly StringName EnsureCellConfigForSelection = "EnsureCellConfigForSelection";

		public static readonly StringName CloneCellConfigs = "CloneCellConfigs";

		public static readonly StringName SetCellConfigs = "SetCellConfigs";

		public static readonly StringName GetCellAtPosition = "GetCellAtPosition";

		public static readonly StringName GetCellRect = "GetCellRect";

		public static readonly StringName NormalizeCellSelection = "NormalizeCellSelection";

		public static readonly StringName ToCellConfigPos = "ToCellConfigPos";

		public static readonly StringName DrawConfiguredCellRects = "DrawConfiguredCellRects";

		public static readonly StringName DrawCellSelection = "DrawCellSelection";

		public static readonly StringName DrawBrushPreviewCell = "DrawBrushPreviewCell";

		public static readonly StringName GetCellConfigColor = "GetCellConfigColor";

		public static readonly StringName RectFromCellConfigPos = "RectFromCellConfigPos";

		public static readonly StringName UpdateCellSelectionLabel = "UpdateCellSelectionLabel";

		public static readonly StringName ApplyBrushGridPreset = "ApplyBrushGridPreset";

		public static readonly StringName BuildCellSelectionText = "BuildCellSelectionText";

		public static readonly StringName BuildBrushSummary = "BuildBrushSummary";

		public static readonly StringName SavePendingMapResource = "SavePendingMapResource";

		public static readonly StringName SaveMapResource = "SaveMapResource";

		public static readonly StringName UpdateMapSummary = "UpdateMapSummary";

		public static readonly StringName QueueGridOverlayRedraw = "QueueGridOverlayRedraw";

		public static readonly StringName AddMapSummaryRows = "AddMapSummaryRows";

		public static readonly StringName ReadVector4 = "ReadVector4";

		public static readonly StringName GetViewportSize = "GetViewportSize";

		public static readonly StringName ResolveMapPreviewScene = "ResolveMapPreviewScene";

		public static readonly StringName LoadDefaultFrontlawnScene = "LoadDefaultFrontlawnScene";

		public static readonly StringName BuildMapSceneLabel = "BuildMapSceneLabel";

		public static readonly StringName FormatResource = "FormatResource";

		public static readonly StringName FormatResourcePath = "FormatResourcePath";

		public static readonly StringName FormatVector = "FormatVector";

		public static readonly StringName FormatBool = "FormatBool";

		public static readonly StringName BuildSpecialRuleSummary = "BuildSpecialRuleSummary";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName UseCellBrushTool = "UseCellBrushTool";

		public static readonly StringName _editingMap = "_editingMap";

		public static readonly StringName _lineUseHost = "_lineUseHost";

		public static readonly StringName _specialRulesHost = "_specialRulesHost";

		public static readonly StringName _specialRulesSummary = "_specialRulesSummary";

		public static readonly StringName _mapSceneViewport = "_mapSceneViewport";

		public static readonly StringName _mapScenePreviewRoot = "_mapScenePreviewRoot";

		public static readonly StringName _mapPreviewStack = "_mapPreviewStack";

		public static readonly StringName _mapGridOverlay = "_mapGridOverlay";

		public static readonly StringName _scenePathLabel = "_scenePathLabel";

		public static readonly StringName _gridSummaryLabel = "_gridSummaryLabel";

		public static readonly StringName _edgeSummaryLabel = "_edgeSummaryLabel";

		public static readonly StringName _flagsSummaryLabel = "_flagsSummaryLabel";

		public static readonly StringName _cellSelectionLabel = "_cellSelectionLabel";

		public static readonly StringName _useCellBrushTool = "_useCellBrushTool";

		public static readonly StringName _brushGridTypeOption = "_brushGridTypeOption";

		public static readonly StringName _brushGroundCheckBox = "_brushGroundCheckBox";

		public static readonly StringName _brushAirCheckBox = "_brushAirCheckBox";

		public static readonly StringName _brushWaterCheckBox = "_brushWaterCheckBox";

		public static readonly StringName _brushElementFlagsSpinBox = "_brushElementFlagsSpinBox";

		public static readonly StringName _isGridBoxSelecting = "_isGridBoxSelecting";

		public static readonly StringName MapCanvasPan = "MapCanvasPan";

		public static readonly StringName MapCanvasLastPointer = "MapCanvasLastPointer";

		public static readonly StringName MapCanvasZoom = "MapCanvasZoom";

		public static readonly StringName MapCanvasOffset = "MapCanvasOffset";

		public static readonly StringName _gridSelectionStart = "_gridSelectionStart";

		public static readonly StringName _gridSelectionEnd = "_gridSelectionEnd";

		public static readonly StringName MapGridSelectionRect = "MapGridSelectionRect";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _mapSaveTimer = "_mapSaveTimer";

		public static readonly StringName _editingMapRuleResource = "_editingMapRuleResource";

		public static readonly StringName _mapRuleBaselineRoot = "_mapRuleBaselineRoot";

		public static readonly StringName _mapRuleResultRoot = "_mapRuleResultRoot";

		public static readonly StringName _mapRulePreviewStatus = "_mapRulePreviewStatus";

		public static readonly StringName _mapRulePreviewResult = "_mapRulePreviewResult";

		public static readonly StringName _mapRulePreviewValidation = "_mapRulePreviewValidation";

		public static readonly StringName _mapRulePacketType = "_mapRulePacketType";

		public static readonly StringName _mapRulePacketTypeCatalogButton = "_mapRulePacketTypeCatalogButton";

		public static readonly StringName _mapVisualPicker = "_mapVisualPicker";

		public static readonly StringName _mapRuleCharacterName = "_mapRuleCharacterName";

		public static readonly StringName _mapRuleBaseCooldown = "_mapRuleBaseCooldown";

		public static readonly StringName _mapRuleBaseSpeed = "_mapRuleBaseSpeed";

		public static readonly StringName _mapRuleBaseHitpoints = "_mapRuleBaseHitpoints";

		public static readonly StringName _mapRuleBaseScale = "_mapRuleBaseScale";

		public static readonly StringName _mapRuleBaseDps = "_mapRuleBaseDps";

		public static readonly StringName _mapRuleResultCooldown = "_mapRuleResultCooldown";

		public static readonly StringName _mapRuleResultSpeed = "_mapRuleResultSpeed";

		public static readonly StringName _mapRuleResultHitpoints = "_mapRuleResultHitpoints";

		public static readonly StringName _mapRuleResultScale = "_mapRuleResultScale";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string EditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWMapVisualEditorLayout.tscn";

	private const string MapRuleRuntimePreviewScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWMapRuleRuntimePreview.tscn";

	private static PackedScene _editorLayoutScene;

	private static PackedScene _mapRuleRuntimePreviewScene;

	private const string DefaultFrontlawnScenePath = "res://Asset/Config/Map/Frontlawn/Scene/Day/TowerDefenseMapFrontlawn.tscn";

	private static PackedScene _defaultFrontlawnScene;

	private TowerDefenseMapConfig _editingMap;

	private XWVisualPropertyBinding _mapPropertyBinding;

	private FlowContainer _lineUseHost;

	private VBoxContainer _specialRulesHost;

	private Label _specialRulesSummary;

	private SubViewport _mapSceneViewport;

	private Node2D _mapScenePreviewRoot;

	private Control _mapPreviewStack;

	private Control _mapGridOverlay;

	private Label _scenePathLabel;

	private Label _gridSummaryLabel;

	private Label _edgeSummaryLabel;

	private Label _flagsSummaryLabel;

	private Label _cellSelectionLabel;

	private CheckBox _useCellBrushTool;

	private OptionButton _brushGridTypeOption;

	private CheckBox _brushGroundCheckBox;

	private CheckBox _brushAirCheckBox;

	private CheckBox _brushWaterCheckBox;

	private SpinBox _brushElementFlagsSpinBox;

	private bool _isGridBoxSelecting;

	private bool MapCanvasPan;

	private Vector2 MapCanvasLastPointer = Vector2.Zero;

	private float MapCanvasZoom = 1f;

	private Vector2 MapCanvasOffset = Vector2.Zero;

	private Vector2I _gridSelectionStart = new Vector2I(-1, -1);

	private Vector2I _gridSelectionEnd = new Vector2I(-1, -1);

	private Rect2I MapGridSelectionRect = new Rect2I(0, 0, 0, 0);

	private bool _updatingControls;

	private Timer _mapSaveTimer;

	private Resource _editingMapRuleResource;

	private Node2D _mapRuleBaselineRoot;

	private Node2D _mapRuleResultRoot;

	private Label _mapRulePreviewStatus;

	private Label _mapRulePreviewResult;

	private Label _mapRulePreviewValidation;

	private OptionButton _mapRulePacketType;

	private Button _mapRulePacketTypeCatalogButton;

	private XWGameplayResourcePickerWindow _mapVisualPicker;

	private readonly List<XWVisualSegmentedOption> _mapSegmentedOptions = new List<XWVisualSegmentedOption>();

	private LineEdit _mapRuleCharacterName;

	private SpinBox _mapRuleBaseCooldown;

	private SpinBox _mapRuleBaseSpeed;

	private SpinBox _mapRuleBaseHitpoints;

	private SpinBox _mapRuleBaseScale;

	private SpinBox _mapRuleBaseDps;

	private double _mapRuleResultCooldown;

	private double _mapRuleResultSpeed;

	private double _mapRuleResultHitpoints;

	private double _mapRuleResultScale;

	private const float MapCanvasMinZoom = 0.05f;

	private const float MapCanvasMaxZoom = 12f;

	private bool UseCellBrushTool
	{
		get
		{
			if (GodotObject.IsInstanceValid(_useCellBrushTool))
			{
				return _useCellBrushTool.ButtonPressed;
			}
			return true;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		_mapSaveTimer = new Timer
		{
			WaitTime = 0.35,
			OneShot = true
		};
		_mapSaveTimer.Timeout += SavePendingMapResource;
		AddChild(_mapSaveTimer, forceReadableName: false, InternalMode.Disabled);
	}

	public override void _ExitTree()
	{
		DisposeMapVisualChoices();
		DisposeMapPropertyBinding();
		_editingMap = null;
		_editingMapRuleResource = null;
		if (GodotObject.IsInstanceValid(_mapSaveTimer))
		{
			_mapSaveTimer.Stop();
		}
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeMapVisualChoices();
		DisposeMapPropertyBinding();
		_editingMap = null;
		_editingMapRuleResource = null;
		Resource currentResource = CurrentResource;
		if (!(currentResource is TowerDefenseMapConfig towerDefenseMapConfig))
		{
			if (currentResource is TowerDefenseMapRuleConfig || currentResource is TowerDefenseMapPacketRuleConfig || currentResource is TowerDefenseMapCharacterRuleConfig)
			{
				_editingMap = null;
				_editingMapRuleResource = CurrentResource;
				RenderMapRuleRuntimePreview(CurrentResource);
			}
		}
		else
		{
			_editingMap = towerDefenseMapConfig;
			_editingMapRuleResource = null;
			RenderMapEditor(towerDefenseMapConfig);
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!HasCompleteMapVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool HasCompleteMapVisualCoverage(Resource resource)
	{
		return resource?.GetType() == typeof(TowerDefenseMapConfig);
	}

	protected override void OnEmbeddedInspectorPropertyChanged(GodotObject obj, StringName property, StringName field, Variant value)
	{
		Resource resource = obj as Resource;
		bool flag = resource != null;
		if (flag)
		{
			bool flag2 = ((resource is TowerDefenseMapRuleConfig || resource is TowerDefenseMapPacketRuleConfig || resource is TowerDefenseMapCharacterRuleConfig) ? true : false);
			flag = flag2;
		}
		if (flag)
		{
			_editingMapRuleResource = resource;
			UpdateMapRuleRuntimePreview();
		}
	}

	private void RenderMapRuleRuntimePreview(Resource rule)
	{
		if (CanvasGrid == null || !GodotObject.IsInstanceValid(rule))
		{
			return;
		}
		if (_mapRuleRuntimePreviewScene == null)
		{
			_mapRuleRuntimePreviewScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWMapRuleRuntimePreview.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		Control control = _mapRuleRuntimePreviewScene?.Instantiate<Control>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(control))
		{
			CanvasGrid.Columns = 1;
			_mapRuleBaselineRoot = control.GetNode<Node2D>("%BaselineRoot");
			_mapRuleResultRoot = control.GetNode<Node2D>("%ResultRoot");
			_mapRulePreviewStatus = control.GetNode<Label>("Layout/Header/Status");
			_mapRulePreviewResult = control.GetNode<Label>("Layout/Split/Inputs/Result");
			_mapRulePreviewValidation = control.GetNode<Label>("Layout/Split/Inputs/Validation");
			_mapRulePacketType = control.GetNode<OptionButton>("Layout/Split/Inputs/PacketRow/PacketType");
			_mapRulePacketTypeCatalogButton = control.GetNode<Button>("%PacketTypeCatalogButton");
			_mapRuleCharacterName = control.GetNode<LineEdit>("Layout/Split/Inputs/CharacterRow/CharacterName");
			_mapRuleBaseCooldown = control.GetNode<SpinBox>("Layout/Split/Inputs/CooldownRow/Value");
			_mapRuleBaseSpeed = control.GetNode<SpinBox>("Layout/Split/Inputs/SpeedRow/Value");
			_mapRuleBaseHitpoints = control.GetNode<SpinBox>("Layout/Split/Inputs/HitpointsRow/Value");
			_mapRuleBaseScale = control.GetNode<SpinBox>("Layout/Split/Inputs/ScaleRow/Value");
			_mapRuleBaseDps = control.GetNode<SpinBox>("Layout/Split/Inputs/DpsRow/Value");
			TowerDefenseEnum.PACKET_TYPE[] values = Enum.GetValues<TowerDefenseEnum.PACKET_TYPE>();
			for (int i = 0; i < values.Length; i++)
			{
				TowerDefenseEnum.PACKET_TYPE id = values[i];
				_mapRulePacketType.AddItem(id.ToString(), (int)id);
			}
			_mapRulePacketTypeCatalogButton.Text = $"卡牌类型图鉴 · {(TowerDefenseEnum.PACKET_TYPE)_mapRulePacketType.GetSelectedId()}";
			_mapRulePacketTypeCatalogButton.Pressed += OpenMapRulePacketTypeCatalog;
			if (rule is TowerDefenseMapCharacterRuleConfig towerDefenseMapCharacterRuleConfig)
			{
				_mapRuleCharacterName.Text = towerDefenseMapCharacterRuleConfig.characterName;
			}
			_mapRulePacketType.ItemSelected += (long _) =>
			{
				UpdateMapRuleRuntimePreview();
			};
			_mapRuleCharacterName.TextChanged += (string _) =>
			{
				UpdateMapRuleRuntimePreview();
			};
			_mapRuleBaseCooldown.ValueChanged += (double _) =>
			{
				UpdateMapRuleRuntimePreview();
			};
			_mapRuleBaseSpeed.ValueChanged += (double _) =>
			{
				UpdateMapRuleRuntimePreview();
			};
			_mapRuleBaseHitpoints.ValueChanged += (double _) =>
			{
				UpdateMapRuleRuntimePreview();
			};
			_mapRuleBaseScale.ValueChanged += (double _) =>
			{
				UpdateMapRuleRuntimePreview();
			};
			_mapRuleBaseDps.ValueChanged += (double _) =>
			{
				UpdateMapRuleRuntimePreview();
			};
			control.GetNode<Button>("Layout/Split/Inputs/RunButton").Pressed += UpdateMapRuleRuntimePreview;
			CanvasGrid.AddChild(control, forceReadableName: false, InternalMode.Disabled);
			PreviewList?.AddItem("输入测试卡牌、角色和基础数值，预览地图规则的实际乘区结果。");
			UpdateMapRuleRuntimePreview();
		}
	}

	private void UpdateMapRuleRuntimePreview()
	{
		if (!GodotObject.IsInstanceValid(_editingMapRuleResource))
		{
			return;
		}
		TowerDefenseEnum.PACKET_TYPE candidateType = (TowerDefenseEnum.PACKET_TYPE)(_mapRulePacketType?.GetSelectedId() ?? 0);
		string text = _mapRuleCharacterName?.Text ?? "";
		double num = _mapRuleBaseCooldown?.Value ?? 10.0;
		double num2 = _mapRuleBaseSpeed?.Value ?? 1.0;
		double num3 = _mapRuleBaseHitpoints?.Value ?? 100.0;
		double num4 = _mapRuleBaseScale?.Value ?? 1.0;
		double num5 = _mapRuleBaseDps?.Value ?? 20.0;
		bool flag = false;
		double value = 0.0;
		int num6 = 0;
		int num7 = 0;
		Resource editingMapRuleResource = _editingMapRuleResource;
		bool flag2;
		string reason;
		if (!(editingMapRuleResource is TowerDefenseMapRuleConfig towerDefenseMapRuleConfig))
		{
			if (!(editingMapRuleResource is TowerDefenseMapPacketRuleConfig towerDefenseMapPacketRuleConfig))
			{
				if (editingMapRuleResource is TowerDefenseMapCharacterRuleConfig towerDefenseMapCharacterRuleConfig)
				{
					flag2 = towerDefenseMapCharacterRuleConfig.TryValidateRuntime(out reason);
					if (string.Equals(towerDefenseMapCharacterRuleConfig.characterName, text, StringComparison.Ordinal))
					{
						num7 = 1;
						num2 *= towerDefenseMapCharacterRuleConfig.timeScaleMultiplier;
						num3 *= towerDefenseMapCharacterRuleConfig.hitpointScaleMultiplier;
						num4 *= towerDefenseMapCharacterRuleConfig.visualScaleMultiplier;
					}
					_mapRulePreviewStatus.Text = ((num7 > 0) ? "角色规则已命中" : "角色规则未命中");
				}
				else
				{
					flag2 = false;
					reason = "不支持的地图规则类型";
				}
			}
			else
			{
				flag2 = towerDefenseMapPacketRuleConfig.TryValidateRuntime(out reason);
				if (towerDefenseMapPacketRuleConfig.Matches(candidateType))
				{
					num6 = 1;
					num *= towerDefenseMapPacketRuleConfig.cooldownMultiplier;
					flag = towerDefenseMapPacketRuleConfig.ignoreDynamicCostGrowth;
				}
				_mapRulePreviewStatus.Text = ((num6 > 0) ? "卡牌规则已命中" : "卡牌规则未命中");
			}
		}
		else
		{
			flag2 = towerDefenseMapRuleConfig.TryValidateRuntime(out reason);
			value = num5 * towerDefenseMapRuleConfig.attackDpsLifestealRatio;
			if (towerDefenseMapRuleConfig.packetRules != null)
			{
				foreach (TowerDefenseMapPacketRuleConfig packetRule in towerDefenseMapRuleConfig.packetRules)
				{
					if (GodotObject.IsInstanceValid(packetRule) && packetRule.Matches(candidateType))
					{
						num6++;
						num *= packetRule.cooldownMultiplier;
						flag |= packetRule.ignoreDynamicCostGrowth;
					}
				}
			}
			if (towerDefenseMapRuleConfig.characterRules != null)
			{
				foreach (TowerDefenseMapCharacterRuleConfig characterRule in towerDefenseMapRuleConfig.characterRules)
				{
					if (GodotObject.IsInstanceValid(characterRule) && string.Equals(characterRule.characterName, text, StringComparison.Ordinal))
					{
						num7++;
						num2 *= characterRule.timeScaleMultiplier;
						num3 *= characterRule.hitpointScaleMultiplier;
						num4 *= characterRule.visualScaleMultiplier;
					}
				}
			}
			_mapRulePreviewStatus.Text = $"规则 {towerDefenseMapRuleConfig.id} · 卡牌 {num6} · 角色 {num7}";
		}
		_mapRuleResultCooldown = num;
		_mapRuleResultSpeed = num2;
		_mapRuleResultHitpoints = num3;
		_mapRuleResultScale = num4;
		_mapRulePreviewResult.Text = $"冷却: {num:0.###}\n速度: {num2:0.###}\n生命: {num3:0.###}\n视觉缩放: {num4:0.###}\nDPS 吸血: {value:0.###}\n忽略动态费用增长: {(flag ? "是" : "否")}";
		_mapRulePreviewValidation.Text = (flag2 ? "✓ 运行校验通过" : ("⚠ 运行校验失败: " + reason));
		_mapRulePreviewValidation.Modulate = (flag2 ? new Color(0.55f, 0.92f, 0.65f) : new Color(1f, 0.58f, 0.45f));
		RebuildMapRuleGamePreview(text);
	}

	private void RebuildMapRuleGamePreview(string characterName)
	{
		if (!GodotObject.IsInstanceValid(_mapRuleBaselineRoot) || !GodotObject.IsInstanceValid(_mapRuleResultRoot))
		{
			return;
		}
		foreach (Node child in _mapRuleBaselineRoot.GetChildren())
		{
			child.QueueFree();
		}
		foreach (Node child2 in _mapRuleResultRoot.GetChildren())
		{
			child2.QueueFree();
		}
		PackedScene packedScene = null;
		if (!string.IsNullOrWhiteSpace(characterName) && ResourceManager.Instance != null)
		{
			packedScene = ResourceManager.Instance.GetCharacterScene(characterName);
		}
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			packedScene = ResourceLoader.Load<PackedScene>(characterName.Contains("Zombie", StringComparison.OrdinalIgnoreCase) ? "res://Asset/Anime/Character/Zombie/Puzzle/Target/Scene/TowerDefenseZombieTarget.tscn" : "res://Asset/Anime/Character/Plant/Chapter0/Blover/Scene/TowerDefensePlantBlover.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			return;
		}
		TowerDefenseCharacter towerDefenseCharacter = InstantiateMapRuleCharacter(packedScene, _mapRuleBaselineRoot);
		TowerDefenseCharacter towerDefenseCharacter2 = InstantiateMapRuleCharacter(packedScene, _mapRuleResultRoot);
		if (GodotObject.IsInstanceValid(towerDefenseCharacter))
		{
			towerDefenseCharacter.timeScale = _mapRuleBaseSpeed?.Value ?? 1.0;
			towerDefenseCharacter.Scale = Vector2.One * (float)(_mapRuleBaseScale?.Value ?? 1.0);
		}
		if (GodotObject.IsInstanceValid(towerDefenseCharacter2))
		{
			towerDefenseCharacter2.timeScale = _mapRuleResultSpeed;
			towerDefenseCharacter2.Scale = Vector2.One * (float)_mapRuleResultScale;
			if (GodotObject.IsInstanceValid(towerDefenseCharacter2.instance))
			{
				towerDefenseCharacter2.instance.hitpoints = _mapRuleResultHitpoints;
			}
			double num = Math.Max(1.0, _mapRuleBaseHitpoints?.Value ?? 100.0);
			towerDefenseCharacter2.previewDamagePointPersontage = Mathf.Clamp(_mapRuleResultHitpoints / num, 0.0, 1.0);
		}
	}

	private static TowerDefenseCharacter InstantiateMapRuleCharacter(PackedScene scene, Node parent)
	{
		try
		{
			Node node = scene.Instantiate(PackedScene.GenEditState.Disabled);
			if (!(node is TowerDefenseCharacter towerDefenseCharacter))
			{
				node.Free();
				return null;
			}
			towerDefenseCharacter.inGame = false;
			towerDefenseCharacter.editorPreviewMode = true;
			parent.AddChild(towerDefenseCharacter, forceReadableName: false, InternalMode.Disabled);
			return towerDefenseCharacter;
		}
		catch (Exception ex)
		{
			GD.PushWarning("Map rule game preview failed: " + ex.Message);
			return null;
		}
	}

	private void RenderMapEditor(TowerDefenseMapConfig map)
	{
		if (CanvasGrid != null)
		{
			CanvasGrid.Columns = 1;
			if (_editorLayoutScene == null)
			{
				_editorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWMapVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _editorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindMapLayout(vBoxContainer, map);
				AddMapSummaryRows(map);
			}
		}
	}

	private void BindMapLayout(VBoxContainer root, TowerDefenseMapConfig map)
	{
		_scenePathLabel = root.GetNode<Label>("%ScenePathLabel");
		_scenePathLabel.Text = BuildMapSceneLabel(map);
		root.GetNode<Button>("%ResetViewButton").Pressed += ResetMapPreviewView;
		_mapPreviewStack = root.GetNode<Control>("%PreviewStack");
		_mapPreviewStack.Resized += () =>
		{
			UpdateMapSceneViewportSize();
			UpdateMapCanvasTransform();
			QueueGridOverlayRedraw();
		};
		_mapSceneViewport = root.GetNode<SubViewport>("%MapSceneViewport");
		_mapScenePreviewRoot = root.GetNode<Node2D>("%MapScenePreviewRoot");
		_mapGridOverlay = root.GetNode<Control>("%MapGridOverlay");
		_mapGridOverlay.Draw += () =>
		{
			DrawMapGridOverlay(_mapGridOverlay);
		};
		_mapGridOverlay.GuiInput += OnMapGridOverlayGuiInput;
		_useCellBrushTool = root.GetNode<CheckBox>("%UseCellBrushTool");
		_useCellBrushTool.Toggled += (bool _) =>
		{
			OnBrushSettingsChanged();
		};
		_brushGridTypeOption = root.GetNode<OptionButton>("%BrushGridTypeOption");
		_brushGridTypeOption.AddItem("地面 + 空中");
		_brushGridTypeOption.AddItem("仅地面");
		_brushGridTypeOption.AddItem("仅空中");
		_brushGridTypeOption.AddItem("水面");
		_brushGridTypeOption.ItemSelected += (long index) =>
		{
			ApplyBrushGridPreset((int)index);
		};
		MountMapSegmentedOption(_brushGridTypeOption, root.GetNode<HFlowContainer>("%BrushGridTypeSegments"));
		_brushGroundCheckBox = root.GetNode<CheckBox>("%BrushGroundCheckBox");
		_brushAirCheckBox = root.GetNode<CheckBox>("%BrushAirCheckBox");
		_brushWaterCheckBox = root.GetNode<CheckBox>("%BrushWaterCheckBox");
		_brushGroundCheckBox.Toggled += (bool _) =>
		{
			OnBrushSettingsChanged();
		};
		_brushAirCheckBox.Toggled += (bool _) =>
		{
			OnBrushSettingsChanged();
		};
		_brushWaterCheckBox.Toggled += (bool _) =>
		{
			OnBrushSettingsChanged();
		};
		_brushElementFlagsSpinBox = root.GetNode<SpinBox>("%BrushElementFlagsSpinBox");
		_brushElementFlagsSpinBox.ValueChanged += (double _) =>
		{
			UpdateCellSelectionLabel();
		};
		_cellSelectionLabel = root.GetNode<Label>("%CellSelectionLabel");
		_cellSelectionLabel.Text = BuildCellSelectionText();
		BindMapFields(root, map);
		RebuildMapScenePreview();
		ResetMapPreviewView();
	}

	private void OnBrushSettingsChanged()
	{
		UpdateCellSelectionLabel();
		QueueGridOverlayRedraw();
	}

	private void BindMapFields(VBoxContainer root, TowerDefenseMapConfig map)
	{
		_mapPropertyBinding?.Dispose();
		_mapPropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnMapVisualPropertyEdited);
		_mapPropertyBinding.BindText(root.GetNode<LineEdit>("%ResourceNameEdit"), map, "resource_name", () =>
		{
			RefreshMapField("resource_name", refreshOverlay: false);
		}, this, "RefreshMapEditorFromHistory");
		_mapPropertyBinding.BindToggle(root.GetNode<CheckButton>("%LocalToSceneCheck"), map, "resource_local_to_scene", () =>
		{
			RefreshMapField("resource_local_to_scene", refreshOverlay: false);
		}, this, "RefreshMapEditorFromHistory");
		_mapPropertyBinding.BindText(root.GetNode<LineEdit>("%TranslateLineEdit"), map, "translate", () =>
		{
			RefreshMapField("translate", refreshOverlay: false);
		}, this, "RefreshMapEditorFromHistory");
		_mapPropertyBinding.BindText(root.GetNode<LineEdit>("%DayNightSwitchingLineEdit"), map, "dayNightSwitching", () =>
		{
			RefreshMapField("dayNightSwitching", refreshOverlay: false);
		}, this, "RefreshMapEditorFromHistory");
		MountMapResourcePicker(root.GetNode<HBoxContainer>("%MapScenePickerHost"), "MapScenePicker", "PackedScene", map.mapScene, (Resource resource) =>
		{
			SetMapResourcePath("mapScenePath", resource, "更换地图场景", rebuildPreview: true);
		});
		MountMapResourcePicker(root.GetNode<HBoxContainer>("%MapTexturePickerHost"), "MapTexturePicker", "Texture2D", map.mapTexture, (Resource resource) =>
		{
			SetMapResourcePath("mapTexturePath", resource, "更换地图背景", rebuildPreview: true);
		});
		_mapPropertyBinding.BindToggle(root.GetNode<CheckBox>("%IsNightCheck"), map, "isNight", () =>
		{
			RefreshMapField("isNight");
		}, this, "RefreshMapEditorFromHistory");
		_mapPropertyBinding.BindToggle(root.GetNode<CheckBox>("%UseSunFallCheck"), map, "useSunFall", () =>
		{
			RefreshMapField("useSunFall");
		}, this, "RefreshMapEditorFromHistory");
		_flagsSummaryLabel = root.GetNode<Label>("%FlagsSummaryLabel");
		BindVector2Field(root, "MapSize", map, "mapSize", () =>
		{
			RefreshMapField("mapSize", refreshOverlay: true, rebuildPreview: true);
		});
		BindVector2Field(root, "MapOffset", map, "mapOffset", () =>
		{
			RefreshMapField("mapOffset");
		});
		_mapPropertyBinding.BindNumber(root.GetNode<SpinBox>("%PlantOffsetSpinBox"), map, "plantOffset", () =>
		{
			RefreshMapField("plantOffset");
		}, this, "RefreshMapEditorFromHistory");
		BindVector2IField(root, "GridNum", map, "gridNum", () =>
		{
			RefreshMapField("gridNum");
		});
		BindVector2Field(root, "GridBegin", map, "gridBeginPos", () =>
		{
			RefreshMapField("gridBeginPos");
		});
		BindVector2Field(root, "GridSize", map, "gridSize", () =>
		{
			RefreshMapField("gridSize");
		});
		BindVector4Field(root, map, "edge", () =>
		{
			RefreshMapField("edge");
		});
		_gridSummaryLabel = root.GetNode<Label>("%GridSummaryLabel");
		_edgeSummaryLabel = root.GetNode<Label>("%EdgeSummaryLabel");
		_lineUseHost = root.GetNode<FlowContainer>("%LineUseHost");
		_specialRulesHost = root.GetNode<VBoxContainer>("%SpecialRulesHost");
		_specialRulesSummary = root.GetNode<Label>("%SpecialRulesSummary");
		root.GetNode<Button>("%AddRuleButton").Pressed += AddSpecialRule;
		RebuildLineUseControls();
		RebuildSpecialRuleControls();
		UpdateMapSummary();
	}

	private XWResourcePicker MountMapResourcePicker(Control host, string name, string baseType, Resource value, Action<Resource> changed)
	{
		XWResourcePicker picker = XWResourcePicker.Create();
		picker.Name = name;
		picker.Setup(baseType);
		picker.SetEditedResource(value);
		picker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		picker.ResourceChanged += (Resource resource) =>
		{
			changed?.Invoke(resource);
		};
		host.AddChild(picker, forceReadableName: false, InternalMode.Disabled);
		Button button = new Button
		{
			Name = name + "CatalogButton",
			Text = "图鉴",
			TooltipText = "搜索游戏与当前 Mod 资源",
			CustomMinimumSize = new Vector2(72f, 34f)
		};
		button.Pressed += () =>
		{
			EnsureMapVisualPicker();
			_mapVisualPicker?.OpenResourceLibrary(baseType, (baseType == "Texture2D") ? "地图背景" : "地图场景", value?.ResourcePath ?? string.Empty, XWFileSystem.GetSingleton()?.ProjectFolderPath ?? string.Empty, new string[1] { baseType }, new string[3] { "Resources", "Scenes", "Asset" }, (baseType == "Texture2D") ? "res://addons/ModEditor/Icons/ClassIcon/Image.svg" : "res://addons/ModEditor/Icons/PackedScene.svg", (XWGameplayResourceChoice choice) =>
			{
				Resource resource = (value = ResourceLoader.Load<Resource>(choice?.ResourcePath ?? string.Empty, null, ResourceLoader.CacheMode.Reuse));
				picker.SetEditedResource(resource);
				changed?.Invoke(resource);
			});
		};
		host.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		return picker;
	}

	private void OpenMapRulePacketTypeCatalog()
	{
		if (!GodotObject.IsInstanceValid(_mapRulePacketType))
		{
			return;
		}
		EnsureMapVisualPicker();
		List<XWGameplayResourceChoice> list = new List<XWGameplayResourceChoice>();
		TowerDefenseEnum.PACKET_TYPE[] values = Enum.GetValues<TowerDefenseEnum.PACKET_TYPE>();
		for (int i = 0; i < values.Length; i++)
		{
			TowerDefenseEnum.PACKET_TYPE pACKET_TYPE = values[i];
			int num = (int)pACKET_TYPE;
			list.Add(new XWGameplayResourceChoice(num.ToString(), pACKET_TYPE.ToString(), $"PACKET_TYPE/{pACKET_TYPE}", ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ResourceCard.svg", null, ResourceLoader.CacheMode.Reuse), XWGameplayResourceKind.Resource, IsModResource: false));
		}
		_mapVisualPicker?.OpenChoices("卡牌类型图鉴", "搜索运行时规则要测试的卡牌类型", _mapRulePacketType.GetSelectedId().ToString(), list, (XWGameplayResourceChoice choice) =>
		{
			if (int.TryParse(choice?.Key, out var result))
			{
				int itemIndex = _mapRulePacketType.GetItemIndex(result);
				if (itemIndex >= 0)
				{
					_mapRulePacketType.Select(itemIndex);
					_mapRulePacketTypeCatalogButton.Text = $"卡牌类型图鉴 · {(TowerDefenseEnum.PACKET_TYPE)result}";
					_mapRulePacketType.EmitSignal(OptionButton.SignalName.ItemSelected, itemIndex);
				}
			}
		});
	}

	private void MountMapSegmentedOption(OptionButton option, HFlowContainer host)
	{
		if (!GodotObject.IsInstanceValid(option) || !GodotObject.IsInstanceValid(host) || option.ItemCount < 2 || option.ItemCount > 8)
		{
			if (GodotObject.IsInstanceValid(option))
			{
				option.Visible = true;
			}
		}
		else
		{
			XWVisualSegmentedOption xWVisualSegmentedOption = new XWVisualSegmentedOption(option, host);
			xWVisualSegmentedOption.Rebuild();
			_mapSegmentedOptions.Add(xWVisualSegmentedOption);
		}
	}

	private void EnsureMapVisualPicker()
	{
		if (!GodotObject.IsInstanceValid(_mapVisualPicker))
		{
			_mapVisualPicker = XWGameplayResourcePickerWindow.Create();
			if (GodotObject.IsInstanceValid(_mapVisualPicker))
			{
				AddChild(_mapVisualPicker, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void DisposeMapVisualChoices()
	{
		foreach (XWVisualSegmentedOption mapSegmentedOption in _mapSegmentedOptions)
		{
			mapSegmentedOption?.Dispose();
		}
		_mapSegmentedOptions.Clear();
	}

	private void BindVector2Field(VBoxContainer root, string prefix, Resource resource, StringName property, Action refresh)
	{
		SpinBox x = root.GetNode<SpinBox>("%" + prefix + "X");
		SpinBox y = root.GetNode<SpinBox>("%" + prefix + "Y");
		Vector2 vector = resource.Get(property).AsVector2();
		x.Value = vector.X;
		y.Value = vector.Y;
		BindCompositeField(resource, property, new Godot.Range[2] { x, y }, () => Variant.From<Vector2>(new Vector2((float)x.Value, (float)y.Value)), refresh);
	}

	private void BindVector2IField(VBoxContainer root, string prefix, Resource resource, StringName property, Action refresh)
	{
		SpinBox x = root.GetNode<SpinBox>("%" + prefix + "X");
		SpinBox y = root.GetNode<SpinBox>("%" + prefix + "Y");
		x.MinValue = 1.0;
		y.MinValue = 1.0;
		x.MaxValue = 50.0;
		y.MaxValue = 50.0;
		Vector2I vector2I = resource.Get(property).AsVector2I();
		x.Value = vector2I.X;
		y.Value = vector2I.Y;
		BindCompositeField(resource, property, new Godot.Range[2] { x, y }, () => Variant.From<Vector2I>(new Vector2I(Mathf.RoundToInt((float)x.Value), Mathf.RoundToInt((float)y.Value))), refresh);
	}

	private void BindVector4Field(VBoxContainer root, Resource resource, StringName property, Action refresh)
	{
		SpinBox left = root.GetNode<SpinBox>("%EdgeLeft");
		SpinBox top = root.GetNode<SpinBox>("%EdgeTop");
		SpinBox right = root.GetNode<SpinBox>("%EdgeRight");
		SpinBox bottom = root.GetNode<SpinBox>("%EdgeBottom");
		Vector4 vector = resource.Get(property).AsVector4();
		left.Value = vector.X;
		top.Value = vector.Y;
		right.Value = vector.Z;
		bottom.Value = vector.W;
		BindCompositeField(resource, property, new Godot.Range[4] { left, top, right, bottom }, () => Variant.From<Vector4>(ReadVector4(left, top, right, bottom)), refresh);
	}

	private void BindCompositeField(Resource resource, StringName property, Godot.Range[] controls, Func<Variant> readValue, Action refresh)
	{
		foreach (Godot.Range obj in controls)
		{
			obj.FocusEntered += () =>
			{
				_mapPropertyBinding?.BeginEdit(resource, property);
			};
			obj.ValueChanged += (double _) =>
			{
				if (!_updatingControls && _mapPropertyBinding != null)
				{
					_mapPropertyBinding.PreviewValue(resource, property, readValue());
					refresh?.Invoke();
				}
			};
			obj.FocusExited += () =>
			{
				if (!_updatingControls && _mapPropertyBinding != null)
				{
					_mapPropertyBinding.CommitEdit(resource, property, readValue(), $"修改 {property}", this, "RefreshMapEditorFromHistory");
					refresh?.Invoke();
				}
			};
		}
	}

	private void DisposeMapPropertyBinding()
	{
		_mapPropertyBinding?.Dispose();
		_mapPropertyBinding = null;
		_lineUseHost = null;
		_specialRulesHost = null;
		_specialRulesSummary = null;
	}

	private void OnMapVisualPropertyEdited(bool committed)
	{
		if (CurrentResource is TowerDefenseMapConfig towerDefenseMapConfig && towerDefenseMapConfig == _editingMap)
		{
			MarkCurrentResourceDirty();
			towerDefenseMapConfig.EmitChanged();
			if (committed)
			{
				towerDefenseMapConfig.NotifyPropertyListChanged();
			}
			if (GodotObject.IsInstanceValid(_mapSaveTimer))
			{
				_mapSaveTimer.Start();
			}
			UpdateMapSummary();
		}
	}

	private void RefreshMapField(string propertyName, bool refreshOverlay = true, bool rebuildPreview = false)
	{
		if (!_updatingControls && GodotObject.IsInstanceValid(_editingMap))
		{
			if (propertyName == "gridNum")
			{
				RebuildLineUseControls();
			}
			UpdateMapSummary();
			if (rebuildPreview)
			{
				RebuildMapScenePreview();
			}
			if (propertyName == "mapSize")
			{
				ResetMapPreviewView();
			}
			else if (propertyName == "mapOffset")
			{
				UpdateMapCanvasTransform();
			}
			if (refreshOverlay)
			{
				QueueGridOverlayRedraw();
			}
		}
	}

	private void SetMapResourcePath(string propertyName, Resource resource, string actionName, bool rebuildPreview)
	{
		if (_mapPropertyBinding != null && GodotObject.IsInstanceValid(_editingMap))
		{
			string text = (GodotObject.IsInstanceValid(resource) ? (resource.ResourcePath ?? "") : "");
			_mapPropertyBinding.SetValue(_editingMap, propertyName, text, actionName, this, "RefreshMapEditorFromHistory");
			_editingMap.ClearLoadedMapResources();
			RefreshMapField(propertyName, refreshOverlay: true, rebuildPreview);
		}
	}

	private void RebuildLineUseControls()
	{
		if (!GodotObject.IsInstanceValid(_lineUseHost) || !GodotObject.IsInstanceValid(_editingMap))
		{
			return;
		}
		ClearDynamicChildren(_lineUseHost);
		int num = Math.Clamp(_editingMap.gridNum.Y, 1, 50);
		for (int i = 1; i <= num; i++)
		{
			int capturedLine = i;
			CheckButton checkButton = new CheckButton
			{
				Text = $"第 {i} 行",
				ButtonPressed = (_editingMap.lineUse?.Contains(i) ?? false),
				TooltipText = $"控制游戏地图第 {i} 行能否出怪和放置角色"
			};
			checkButton.Toggled += (bool enabled) =>
			{
				SetLineEnabled(capturedLine, enabled);
			};
			_lineUseHost.AddChild(checkButton, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void SetLineEnabled(int line, bool enabled)
	{
		if (_updatingControls || _mapPropertyBinding == null || !GodotObject.IsInstanceValid(_editingMap))
		{
			return;
		}
		Array<int> array = ((_editingMap.lineUse == null) ? new Array<int>() : new Array<int>(_editingMap.lineUse));
		bool flag = array.Contains(line);
		if (enabled != flag)
		{
			if (enabled)
			{
				array.Add(line);
			}
			else
			{
				array.Remove(line);
			}
			_mapPropertyBinding.SetValue(_editingMap, "lineUse", array, enabled ? $"启用地图第 {line} 行" : $"停用地图第 {line} 行", this, "RefreshMapEditorFromHistory");
			RebuildLineUseControls();
			RefreshMapField("lineUse");
		}
	}

	private void RebuildSpecialRuleControls()
	{
		if (!GodotObject.IsInstanceValid(_specialRulesHost) || !GodotObject.IsInstanceValid(_editingMap))
		{
			return;
		}
		ClearDynamicChildren(_specialRulesHost);
		int num = _editingMap.specialRules?.Count ?? 0;
		if (GodotObject.IsInstanceValid(_specialRulesSummary))
		{
			_specialRulesSummary.Text = ((num == 0) ? "未配置特殊规则；地图只使用基础运行逻辑。" : $"已配置 {num} 条规则：{BuildSpecialRuleSummary(_editingMap)}");
		}
		for (int i = 0; i < num; i++)
		{
			int capturedIndex = i;
			TowerDefenseMapRuleConfig rule = _editingMap.specialRules[i];
			HBoxContainer hBoxContainer = new HBoxContainer
			{
				Name = $"SpecialRule_{i}",
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			};
			hBoxContainer.AddChild(new Label
			{
				Text = $"{i + 1}.",
				CustomMinimumSize = new Vector2(28f, 0f),
				VerticalAlignment = VerticalAlignment.Center
			}, forceReadableName: false, InternalMode.Disabled);
			XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
			xWResourcePicker.Name = $"SpecialRulePicker_{i}";
			xWResourcePicker.Setup("TowerDefenseMapRuleConfig");
			xWResourcePicker.SetEditedResource(rule);
			xWResourcePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			xWResourcePicker.ResourceChanged += (Resource resource) =>
			{
				ReplaceSpecialRule(capturedIndex, resource as TowerDefenseMapRuleConfig);
			};
			xWResourcePicker.ResourceSelected += (Resource resource) =>
			{
				OpenSpecialRule(capturedIndex, resource as TowerDefenseMapRuleConfig);
			};
			hBoxContainer.AddChild(xWResourcePicker, forceReadableName: false, InternalMode.Disabled);
			hBoxContainer.AddChild(CreateCompactRuleButton("↑", "规则上移", () =>
			{
				MoveSpecialRule(capturedIndex, -1);
			}, i == 0), forceReadableName: false, InternalMode.Disabled);
			hBoxContainer.AddChild(CreateCompactRuleButton("↓", "规则下移", () =>
			{
				MoveSpecialRule(capturedIndex, 1);
			}, i == num - 1), forceReadableName: false, InternalMode.Disabled);
			hBoxContainer.AddChild(CreateCompactRuleButton("编辑", "在规则运行画面中编辑", () =>
			{
				OpenSpecialRule(capturedIndex, rule);
			}), forceReadableName: false, InternalMode.Disabled);
			hBoxContainer.AddChild(CreateCompactRuleButton("×", "移除规则", () =>
			{
				RemoveSpecialRule(capturedIndex);
			}), forceReadableName: false, InternalMode.Disabled);
			_specialRulesHost.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private static Button CreateCompactRuleButton(string text, string tooltip, Action pressed, bool disabled = false)
	{
		Button button = new Button();
		button.Text = text;
		button.TooltipText = tooltip;
		button.FocusMode = FocusModeEnum.None;
		button.Disabled = disabled;
		button.CustomMinimumSize = new Vector2((text == "编辑") ? 52 : 32, 0f);
		button.Pressed += () =>
		{
			pressed?.Invoke();
		};
		return button;
	}

	private void AddSpecialRule()
	{
		if (GodotObject.IsInstanceValid(_editingMap))
		{
			int num = _editingMap.specialRules?.Count ?? 0;
			TowerDefenseMapRuleConfig item = new TowerDefenseMapRuleConfig
			{
				ResourceName = $"MapRule_{num + 1}",
				id = new StringName($"map_rule_{num + 1}")
			};
			Array<TowerDefenseMapRuleConfig> array = ((_editingMap.specialRules == null) ? new Array<TowerDefenseMapRuleConfig>() : new Array<TowerDefenseMapRuleConfig>(_editingMap.specialRules));
			array.Add(item);
			SetSpecialRules(array, "添加地图特殊规则");
		}
	}

	private void ReplaceSpecialRule(int index, TowerDefenseMapRuleConfig rule)
	{
		if (GodotObject.IsInstanceValid(_editingMap) && _editingMap.specialRules != null && index >= 0 && index < _editingMap.specialRules.Count)
		{
			Array<TowerDefenseMapRuleConfig> array = new Array<TowerDefenseMapRuleConfig>(_editingMap.specialRules);
			array[index] = rule;
			SetSpecialRules(array, "替换地图特殊规则");
		}
	}

	private void RemoveSpecialRule(int index)
	{
		if (GodotObject.IsInstanceValid(_editingMap) && _editingMap.specialRules != null && index >= 0 && index < _editingMap.specialRules.Count)
		{
			Array<TowerDefenseMapRuleConfig> array = new Array<TowerDefenseMapRuleConfig>(_editingMap.specialRules);
			array.RemoveAt(index);
			SetSpecialRules(array, "移除地图特殊规则");
		}
	}

	private void MoveSpecialRule(int index, int direction)
	{
		if (GodotObject.IsInstanceValid(_editingMap) && _editingMap.specialRules != null)
		{
			int num = index + direction;
			if (index >= 0 && index < _editingMap.specialRules.Count && num >= 0 && num < _editingMap.specialRules.Count)
			{
				Array<TowerDefenseMapRuleConfig> array = new Array<TowerDefenseMapRuleConfig>(_editingMap.specialRules);
				TowerDefenseMapRuleConfig item = array[index];
				array.RemoveAt(index);
				array.Insert(num, item);
				SetSpecialRules(array, "调整地图特殊规则顺序");
			}
		}
	}

	private void SetSpecialRules(Array<TowerDefenseMapRuleConfig> next, string actionName)
	{
		if (_mapPropertyBinding != null && GodotObject.IsInstanceValid(_editingMap))
		{
			_mapPropertyBinding.SetValue(_editingMap, "specialRules", next, actionName, this, "RefreshMapEditorFromHistory");
			RebuildSpecialRuleControls();
			RefreshMapField("specialRules", refreshOverlay: false);
		}
	}

	private void OpenSpecialRule(int index, TowerDefenseMapRuleConfig rule)
	{
		if (GodotObject.IsInstanceValid(rule) && GodotObject.IsInstanceValid(_editingMap))
		{
			XWResourceEditContext context = XWResourceEditContext.ForProperty(rule, _editingMap, rule.ResourcePath, CurrentResourcePath, "specialRules", index, "map_editor", CurrentEditContext?.IsBuiltInSource ?? false, $"map_rule_{index}");
			XWEditorInterface.Instance?.EditResource(rule, context);
		}
	}

	public void RefreshMapEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager == null || (!xWUndoRedoManager.IsUndoing() && !xWUndoRedoManager.IsRedoing()) || !(CurrentResource is TowerDefenseMapConfig towerDefenseMapConfig) || towerDefenseMapConfig != _editingMap)
		{
			return;
		}
		MarkCurrentResourceDirty();
		towerDefenseMapConfig.EmitChanged();
		towerDefenseMapConfig.ClearLoadedMapResources();
		if (GodotObject.IsInstanceValid(_mapSaveTimer))
		{
			_mapSaveTimer.Start();
		}
		DisposeMapPropertyBinding();
		foreach (Node child in CanvasGrid.GetChildren())
		{
			CanvasGrid.RemoveChild(child);
			child.QueueFree();
		}
		RenderMapEditor(towerDefenseMapConfig);
	}

	private static void ClearDynamicChildren(Node host)
	{
		foreach (Node child in host.GetChildren())
		{
			child.QueueFree();
		}
	}

	private void RebuildMapScenePreview()
	{
		if (!GodotObject.IsInstanceValid(_mapScenePreviewRoot) || _editingMap == null)
		{
			return;
		}
		foreach (Node child in _mapScenePreviewRoot.GetChildren())
		{
			child.QueueFree();
		}
		UpdateMapSceneViewportSize();
		UpdateMapCanvasTransform();
		Node node = null;
		PackedScene packedScene = ResolveMapPreviewScene(_editingMap);
		if (GodotObject.IsInstanceValid(packedScene))
		{
			try
			{
				node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
			}
			catch (Exception ex)
			{
				GD.PushWarning("Map editor scene preview failed: " + ex.Message);
			}
		}
		if (node != null)
		{
			node.Name = ((!string.IsNullOrWhiteSpace(_editingMap.mapScenePath)) ? "ConfiguredMapScenePreview" : "DefaultFrontlawnScenePreview");
			_mapScenePreviewRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
			return;
		}
		Texture2D mapTexture = _editingMap.GetMapTexture(cache: false);
		if (GodotObject.IsInstanceValid(mapTexture))
		{
			_mapScenePreviewRoot.AddChild(new Sprite2D
			{
				Name = "MapTextureFallbackPreview",
				Texture = mapTexture,
				Centered = false
			}, forceReadableName: false, InternalMode.Disabled);
		}
		else
		{
			Label node2 = new Label
			{
				Name = "MapEmptyPreviewLabel",
				Text = "未配置 mapScene 或 mapTexture"
			};
			_mapScenePreviewRoot.AddChild(node2, forceReadableName: false, InternalMode.Disabled);
		}
		UpdateMapCanvasTransform();
		QueueGridOverlayRedraw();
	}

	private void ResetMapPreviewView()
	{
		if (_editingMap != null)
		{
			Vector2 mapPreviewViewSize = GetMapPreviewViewSize();
			Vector2 vector = new Vector2(Mathf.Max(1f, _editingMap.mapSize.X), Mathf.Max(1f, _editingMap.mapSize.Y));
			float value = Mathf.Min(mapPreviewViewSize.X / vector.X, mapPreviewViewSize.Y / vector.Y) * 0.96f;
			MapCanvasZoom = Mathf.Clamp(value, 0.05f, 12f);
			MapCanvasOffset = (mapPreviewViewSize - vector * MapCanvasZoom) * 0.5f;
			UpdateMapCanvasTransform();
			QueueGridOverlayRedraw();
		}
	}

	private void SetMapCanvasZoom(float zoom, Vector2 pivot)
	{
		float num = Mathf.Clamp(zoom, 0.05f, 12f);
		if (!Mathf.IsEqualApprox(num, MapCanvasZoom))
		{
			Vector2 vector = MapViewToWorld(pivot);
			MapCanvasZoom = num;
			MapCanvasOffset = pivot - vector * MapCanvasZoom;
			UpdateMapCanvasTransform();
			QueueGridOverlayRedraw();
		}
	}

	private void UpdateMapCanvasTransform()
	{
		UpdateMapSceneViewportSize();
		if (_editingMap != null && GodotObject.IsInstanceValid(_mapScenePreviewRoot))
		{
			_mapScenePreviewRoot.Position = MapWorldToView(_editingMap.mapOffset);
			_mapScenePreviewRoot.Scale = Vector2.One * MapCanvasZoom;
		}
	}

	private Vector2 MapWorldToView(Vector2 world)
	{
		return world * MapCanvasZoom + MapCanvasOffset;
	}

	private Vector2 MapViewToWorld(Vector2 view)
	{
		float num = Mathf.Max(MapCanvasZoom, 0.001f);
		return (view - MapCanvasOffset) / num;
	}

	private void UpdateMapSceneViewportSize()
	{
		if (GodotObject.IsInstanceValid(_mapSceneViewport))
		{
			Vector2 mapPreviewViewSize = GetMapPreviewViewSize();
			_mapSceneViewport.Size = new Vector2I(Mathf.Max(1, Mathf.RoundToInt(mapPreviewViewSize.X)), Mathf.Max(1, Mathf.RoundToInt(mapPreviewViewSize.Y)));
		}
	}

	private Vector2 GetMapPreviewViewSize()
	{
		if (GodotObject.IsInstanceValid(_mapGridOverlay) && _mapGridOverlay.Size.X > 1f && _mapGridOverlay.Size.Y > 1f)
		{
			return _mapGridOverlay.Size;
		}
		if (GodotObject.IsInstanceValid(_mapPreviewStack) && _mapPreviewStack.Size.X > 1f && _mapPreviewStack.Size.Y > 1f)
		{
			return _mapPreviewStack.Size;
		}
		return new Vector2(700f, 330f);
	}

	private void DrawMapGridOverlay(Control overlay)
	{
		if (!GodotObject.IsInstanceValid(overlay) || _editingMap == null)
		{
			return;
		}
		Vector2 begin = MapWorldToView(_editingMap.mapOffset + _editingMap.gridBeginPos);
		Vector2 cellSize = _editingMap.gridSize * MapCanvasZoom;
		int num = Math.Clamp(_editingMap.gridNum.X, 1, 50);
		int num2 = Math.Clamp(_editingMap.gridNum.Y, 1, 50);
		Color color = new Color(0.12f, 0.5f, 0.22f, 0.16f);
		Color color2 = new Color(0.5f, 0.95f, 0.58f, 0.72f);
		Color color3 = new Color(0.35f, 0.72f, 1f, 0.25f);
		Color color4 = new Color(1f, 0.72f, 0.18f, 0.88f);
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				Rect2 cellRect = GetCellRect(j, i, begin, cellSize);
				overlay.DrawRect(cellRect, color);
				overlay.DrawRect(cellRect, color2, filled: false, 1f);
			}
		}
		DrawConfiguredCellRects(overlay, begin, cellSize, color3);
		Rect2 rect = new Rect2(MapWorldToView(_editingMap.mapOffset + new Vector2(_editingMap.edge.X, _editingMap.edge.Y)), new Vector2(Math.Max(0f, _editingMap.edge.Z - _editingMap.edge.X), Math.Max(0f, _editingMap.edge.W - _editingMap.edge.Y)) * MapCanvasZoom);
		overlay.DrawRect(rect, color4, filled: false, 2f);
		float y = MapWorldToView(_editingMap.mapOffset + new Vector2(0f, (float)((double)_editingMap.gridBeginPos.Y + _editingMap.plantOffset))).Y;
		overlay.DrawLine(new Vector2(0f, y), new Vector2(overlay.Size.X, y), new Color(0.35f, 0.72f, 1f, 0.9f), 1.5f);
		DrawCellSelection(overlay, begin, cellSize);
	}

	private void OnMapGridOverlayGuiInput(InputEvent inputEvent)
	{
		if (_editingMap == null || !GodotObject.IsInstanceValid(_mapGridOverlay))
		{
			return;
		}
		if (inputEvent is InputEventMouseButton inputEventMouseButton)
		{
			if (inputEventMouseButton.ButtonIndex == MouseButton.WheelUp && inputEventMouseButton.Pressed)
			{
				SetMapCanvasZoom(MapCanvasZoom * 1.1f, inputEventMouseButton.Position);
				_mapGridOverlay.AcceptEvent();
				return;
			}
			if (inputEventMouseButton.ButtonIndex == MouseButton.WheelDown && inputEventMouseButton.Pressed)
			{
				SetMapCanvasZoom(MapCanvasZoom / 1.1f, inputEventMouseButton.Position);
				_mapGridOverlay.AcceptEvent();
				return;
			}
			MouseButton buttonIndex = inputEventMouseButton.ButtonIndex;
			if (((ulong)(buttonIndex - 2) <= 1uL) ? true : false)
			{
				MapCanvasPan = inputEventMouseButton.Pressed;
				MapCanvasLastPointer = inputEventMouseButton.Position;
				_mapGridOverlay.AcceptEvent();
			}
			else
			{
				if (inputEventMouseButton.ButtonIndex != MouseButton.Left)
				{
					return;
				}
				Vector2I cellAtPosition = GetCellAtPosition(inputEventMouseButton.Position);
				if (inputEventMouseButton.Pressed)
				{
					if (cellAtPosition.X >= 0)
					{
						StartGridBoxSelection(cellAtPosition);
						_mapGridOverlay.AcceptEvent();
					}
				}
				else if (_isGridBoxSelecting)
				{
					if (cellAtPosition.X >= 0)
					{
						UpdateGridBoxSelection(cellAtPosition);
					}
					FinishGridBoxSelection();
					_mapGridOverlay.AcceptEvent();
				}
			}
		}
		else
		{
			if (!(inputEvent is InputEventMouseMotion inputEventMouseMotion))
			{
				return;
			}
			if (MapCanvasPan)
			{
				PanMapCanvasTo(inputEventMouseMotion.Position);
			}
			else if (_isGridBoxSelecting)
			{
				Vector2I cellAtPosition2 = GetCellAtPosition(inputEventMouseMotion.Position);
				if (cellAtPosition2.X >= 0)
				{
					UpdateGridBoxSelection(cellAtPosition2);
					_mapGridOverlay.AcceptEvent();
				}
			}
		}
	}

	private void PanMapCanvasTo(Vector2 position)
	{
		MapCanvasOffset += position - MapCanvasLastPointer;
		MapCanvasLastPointer = position;
		UpdateMapCanvasTransform();
		QueueGridOverlayRedraw();
		_mapGridOverlay.AcceptEvent();
	}

	private void StartGridBoxSelection(Vector2I cell)
	{
		_isGridBoxSelecting = true;
		_gridSelectionStart = cell;
		_gridSelectionEnd = cell;
		MapGridSelectionRect = NormalizeCellSelection(_gridSelectionStart, _gridSelectionEnd);
		UpdateCellSelectionLabel();
		QueueGridOverlayRedraw();
	}

	private void UpdateGridBoxSelection(Vector2I cell)
	{
		_gridSelectionEnd = cell;
		MapGridSelectionRect = NormalizeCellSelection(_gridSelectionStart, _gridSelectionEnd);
		UpdateCellSelectionLabel();
		QueueGridOverlayRedraw();
	}

	private void FinishGridBoxSelection()
	{
		_isGridBoxSelecting = false;
		if (MapGridSelectionRect.Size.X > 0 && MapGridSelectionRect.Size.Y > 0)
		{
			if (UseCellBrushTool)
			{
				ApplyBrushToSelection();
			}
			else
			{
				ApplySelectedCellConfig();
			}
		}
		UpdateCellSelectionLabel();
		QueueGridOverlayRedraw();
	}

	private void ApplyBrushToSelection()
	{
		if (_editingMap == null || _mapPropertyBinding == null || MapGridSelectionRect.Size.X <= 0 || MapGridSelectionRect.Size.Y <= 0)
		{
			return;
		}
		Array<TowerDefenseCellConfig> array = CloneCellConfigs(_editingMap.cellConfig);
		for (int i = MapGridSelectionRect.Position.Y; i < MapGridSelectionRect.Position.Y + MapGridSelectionRect.Size.Y; i++)
		{
			for (int j = MapGridSelectionRect.Position.X; j < MapGridSelectionRect.Position.X + MapGridSelectionRect.Size.X; j++)
			{
				PaintBrushAtCell(array, new Vector2I(j, i));
			}
		}
		SetCellConfigs(array, "绘制地图格子");
	}

	private void PaintBrushAtCell(Array<TowerDefenseCellConfig> cellConfigs, Vector2I cell)
	{
		if (cellConfigs == null || cell.X < 0 || cell.Y < 0)
		{
			return;
		}
		Vector4I vector4I = new Vector4I(cell.X + 1, cell.Y + 1, cell.X + 1, cell.Y + 1);
		TowerDefenseCellConfig towerDefenseCellConfig = null;
		foreach (TowerDefenseCellConfig cellConfig in cellConfigs)
		{
			if (GodotObject.IsInstanceValid(cellConfig) && cellConfig.pos == vector4I)
			{
				towerDefenseCellConfig = cellConfig;
				break;
			}
		}
		if (!GodotObject.IsInstanceValid(towerDefenseCellConfig))
		{
			towerDefenseCellConfig = new TowerDefenseCellConfig
			{
				ResourceName = $"Cell_{vector4I.X}_{vector4I.Y}",
				pos = vector4I
			};
			cellConfigs.Add(towerDefenseCellConfig);
		}
		SetCellConfigBrushData(towerDefenseCellConfig);
	}

	private void SetCellConfigBrushData(TowerDefenseCellConfig config)
	{
		if (GodotObject.IsInstanceValid(config))
		{
			config.gridType = BuildBrushGridTypeArray();
			config.ElementFlags = (GodotObject.IsInstanceValid(_brushElementFlagsSpinBox) ? Mathf.RoundToInt((float)_brushElementFlagsSpinBox.Value) : 0);
		}
	}

	private Array<TowerDefenseEnum.PLANTGRIDTYPE> BuildBrushGridTypeArray()
	{
		Array<TowerDefenseEnum.PLANTGRIDTYPE> array = new Array<TowerDefenseEnum.PLANTGRIDTYPE>();
		if (GodotObject.IsInstanceValid(_brushGroundCheckBox) && _brushGroundCheckBox.ButtonPressed)
		{
			array.Add(TowerDefenseEnum.PLANTGRIDTYPE.GROUND);
		}
		if (GodotObject.IsInstanceValid(_brushAirCheckBox) && _brushAirCheckBox.ButtonPressed)
		{
			array.Add(TowerDefenseEnum.PLANTGRIDTYPE.AIR);
		}
		if (GodotObject.IsInstanceValid(_brushWaterCheckBox) && _brushWaterCheckBox.ButtonPressed)
		{
			array.Add(TowerDefenseEnum.PLANTGRIDTYPE.WATER);
		}
		if (array.Count == 0)
		{
			array.Add(TowerDefenseEnum.PLANTGRIDTYPE.NOONE);
		}
		return array;
	}

	private void ApplySelectedCellConfig()
	{
		if (_editingMap != null && _mapPropertyBinding != null && MapGridSelectionRect.Size.X > 0 && MapGridSelectionRect.Size.Y > 0)
		{
			Array<TowerDefenseCellConfig> array = CloneCellConfigs(_editingMap.cellConfig);
			if (EnsureCellConfigForSelection(array, MapGridSelectionRect))
			{
				SetCellConfigs(array, "添加地图格子区域");
			}
		}
	}

	private static bool EnsureCellConfigForSelection(Array<TowerDefenseCellConfig> cellConfigs, Rect2I selection)
	{
		Vector4I vector4I = ToCellConfigPos(selection);
		foreach (TowerDefenseCellConfig cellConfig in cellConfigs)
		{
			if (GodotObject.IsInstanceValid(cellConfig) && cellConfig.pos == vector4I)
			{
				return false;
			}
		}
		cellConfigs.Add(new TowerDefenseCellConfig
		{
			ResourceName = $"Cell_{vector4I.X}_{vector4I.Y}_{vector4I.Z}_{vector4I.W}",
			pos = vector4I
		});
		return true;
	}

	private static Array<TowerDefenseCellConfig> CloneCellConfigs(Array<TowerDefenseCellConfig> source)
	{
		Array<TowerDefenseCellConfig> array = new Array<TowerDefenseCellConfig>();
		if (source == null)
		{
			return array;
		}
		foreach (TowerDefenseCellConfig item in source)
		{
			if (!GodotObject.IsInstanceValid(item))
			{
				array.Add(item);
			}
			else
			{
				array.Add((item.Duplicate(deep: true) as TowerDefenseCellConfig) ?? item);
			}
		}
		return array;
	}

	private void SetCellConfigs(Array<TowerDefenseCellConfig> next, string actionName)
	{
		if (_mapPropertyBinding != null && GodotObject.IsInstanceValid(_editingMap))
		{
			_mapPropertyBinding.SetValue(_editingMap, "cellConfig", next, actionName, this, "RefreshMapEditorFromHistory");
			RefreshMapField("cellConfig");
		}
	}

	private Vector2I GetCellAtPosition(Vector2 position)
	{
		if (_editingMap == null || !GodotObject.IsInstanceValid(_mapGridOverlay))
		{
			return new Vector2I(-1, -1);
		}
		if (Mathf.IsZeroApprox(MapCanvasZoom) || Mathf.IsZeroApprox(_editingMap.gridSize.X) || Mathf.IsZeroApprox(_editingMap.gridSize.Y))
		{
			return new Vector2I(-1, -1);
		}
		Vector2 vector = MapViewToWorld(position) - _editingMap.mapOffset - _editingMap.gridBeginPos;
		int num = Mathf.FloorToInt(vector.X / _editingMap.gridSize.X);
		int num2 = Mathf.FloorToInt(vector.Y / _editingMap.gridSize.Y);
		int num3 = Math.Clamp(_editingMap.gridNum.X, 1, 50);
		int num4 = Math.Clamp(_editingMap.gridNum.Y, 1, 50);
		if (num < 0 || num2 < 0 || num >= num3 || num2 >= num4)
		{
			return new Vector2I(-1, -1);
		}
		return new Vector2I(num, num2);
	}

	private static Rect2 GetCellRect(int x, int y, Vector2 begin, Vector2 cellSize)
	{
		return new Rect2(begin + new Vector2((float)x * cellSize.X, (float)y * cellSize.Y), cellSize);
	}

	private static Rect2I NormalizeCellSelection(Vector2I a, Vector2I b)
	{
		int num = Math.Min(a.X, b.X);
		int num2 = Math.Min(a.Y, b.Y);
		int num3 = Math.Max(a.X, b.X);
		int num4 = Math.Max(a.Y, b.Y);
		return new Rect2I(num, num2, num3 - num + 1, num4 - num2 + 1);
	}

	private static Vector4I ToCellConfigPos(Rect2I selection)
	{
		return new Vector4I(selection.Position.X + 1, selection.Position.Y + 1, selection.Position.X + selection.Size.X, selection.Position.Y + selection.Size.Y);
	}

	private void DrawConfiguredCellRects(Control overlay, Vector2 begin, Vector2 cellSize, Color color)
	{
		if (_editingMap?.cellConfig == null)
		{
			return;
		}
		foreach (TowerDefenseCellConfig item in _editingMap.cellConfig)
		{
			if (!GodotObject.IsInstanceValid(item))
			{
				continue;
			}
			Rect2I rect2I = RectFromCellConfigPos(item.pos);
			if (rect2I.Size.X <= 0 || rect2I.Size.Y <= 0)
			{
				continue;
			}
			Color cellConfigColor = GetCellConfigColor(item);
			for (int i = rect2I.Position.Y; i < rect2I.Position.Y + rect2I.Size.Y; i++)
			{
				for (int j = rect2I.Position.X; j < rect2I.Position.X + rect2I.Size.X; j++)
				{
					overlay.DrawRect(GetCellRect(j, i, begin, cellSize), cellConfigColor);
				}
			}
		}
	}

	private void DrawCellSelection(Control overlay, Vector2 begin, Vector2 cellSize)
	{
		if (MapGridSelectionRect.Size.X > 0 && MapGridSelectionRect.Size.Y > 0)
		{
			Rect2 rect = new Rect2(begin + new Vector2((float)MapGridSelectionRect.Position.X * cellSize.X, (float)MapGridSelectionRect.Position.Y * cellSize.Y), new Vector2((float)MapGridSelectionRect.Size.X * cellSize.X, (float)MapGridSelectionRect.Size.Y * cellSize.Y));
			DrawBrushPreviewCell(overlay, rect);
			overlay.DrawRect(rect, new Color(0.38f, 0.78f, 1f, 0.95f), filled: false, 2f);
		}
	}

	private void DrawBrushPreviewCell(Control overlay, Rect2 rect)
	{
		Color color = new Color(0.18f, 0.54f, 1f, 0.24f);
		Array<TowerDefenseEnum.PLANTGRIDTYPE> array = BuildBrushGridTypeArray();
		if (array.Contains(TowerDefenseEnum.PLANTGRIDTYPE.WATER))
		{
			color = new Color(0.12f, 0.46f, 0.95f, 0.26f);
		}
		else if (array.Contains(TowerDefenseEnum.PLANTGRIDTYPE.GROUND) && array.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR))
		{
			color = new Color(0.18f, 0.68f, 0.32f, 0.24f);
		}
		else if (array.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR))
		{
			color = new Color(0.68f, 0.46f, 1f, 0.24f);
		}
		overlay.DrawRect(rect, color);
	}

	private static Color GetCellConfigColor(TowerDefenseCellConfig config)
	{
		if (!GodotObject.IsInstanceValid(config))
		{
			return new Color(0.35f, 0.72f, 1f, 0.25f);
		}
		if (config.gridType != null && config.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.WATER))
		{
			return new Color(0.08f, 0.42f, 0.95f, 0.3f);
		}
		if (config.gridType != null && config.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.GROUND) && config.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR))
		{
			return new Color(0.2f, 0.72f, 0.34f, 0.28f);
		}
		if (config.gridType != null && config.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.GROUND))
		{
			return new Color(0.38f, 0.76f, 0.28f, 0.3f);
		}
		if (config.gridType != null && config.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR))
		{
			return new Color(0.66f, 0.45f, 1f, 0.28f);
		}
		return new Color(0.35f, 0.72f, 1f, 0.25f);
	}

	private static Rect2I RectFromCellConfigPos(Vector4I pos)
	{
		int num = Math.Max(0, pos.X - 1);
		int num2 = Math.Max(0, pos.Y - 1);
		int num3 = Math.Max(num, pos.Z - 1);
		int num4 = Math.Max(num2, pos.W - 1);
		return new Rect2I(num, num2, num3 - num + 1, num4 - num2 + 1);
	}

	private void UpdateCellSelectionLabel()
	{
		if (GodotObject.IsInstanceValid(_cellSelectionLabel))
		{
			if (MapGridSelectionRect.Size.X <= 0 || MapGridSelectionRect.Size.Y <= 0)
			{
				_cellSelectionLabel.Text = BuildCellSelectionText();
				return;
			}
			Vector4I vector4I = ToCellConfigPos(MapGridSelectionRect);
			_cellSelectionLabel.Text = $"已选格子: ({vector4I.X},{vector4I.Y}) - ({vector4I.Z},{vector4I.W})，共 {MapGridSelectionRect.Size.X * MapGridSelectionRect.Size.Y} 格；{BuildBrushSummary()}";
		}
	}

	private void ApplyBrushGridPreset(int preset)
	{
		if (GodotObject.IsInstanceValid(_brushGroundCheckBox) && GodotObject.IsInstanceValid(_brushAirCheckBox) && GodotObject.IsInstanceValid(_brushWaterCheckBox))
		{
			CheckBox brushGroundCheckBox = _brushGroundCheckBox;
			bool buttonPressed = (uint)preset <= 1u;
			brushGroundCheckBox.ButtonPressed = buttonPressed;
			brushGroundCheckBox = _brushAirCheckBox;
			buttonPressed = ((preset == 0 || preset == 2) ? true : false);
			brushGroundCheckBox.ButtonPressed = buttonPressed;
			_brushWaterCheckBox.ButtonPressed = preset == 3;
			UpdateCellSelectionLabel();
			QueueGridOverlayRedraw();
		}
	}

	private string BuildCellSelectionText()
	{
		return "在预览中设置好格子位置后拖拽刷格子；" + BuildBrushSummary();
	}

	private string BuildBrushSummary()
	{
		Array<TowerDefenseEnum.PLANTGRIDTYPE> array = BuildBrushGridTypeArray();
		List<string> list = new List<string>();
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in array)
		{
			list.Add(item.ToString());
		}
		int value = (GodotObject.IsInstanceValid(_brushElementFlagsSpinBox) ? Mathf.RoundToInt((float)_brushElementFlagsSpinBox.Value) : 0);
		return $"刷子: {string.Join("+", list)}, ElementFlags={value}";
	}

	private void SavePendingMapResource()
	{
		if (GodotObject.IsInstanceValid(_editingMap))
		{
			SaveMapResource(_editingMap);
		}
	}

	private void SaveMapResource(TowerDefenseMapConfig map)
	{
		string currentResourcePath = CurrentResourcePath;
		if (string.IsNullOrWhiteSpace(currentResourcePath) || !GodotObject.IsInstanceValid(map))
		{
			return;
		}
		Error error = ResourceSaver.Save(map, currentResourcePath, ResourceSaver.SaverFlags.None);
		if (error != Error.Ok)
		{
			GD.PushWarning($"Map editor save failed: {error} {currentResourcePath}");
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

	private void UpdateMapSummary()
	{
		if (_editingMap == null)
		{
			return;
		}
		_updatingControls = true;
		try
		{
			if (GodotObject.IsInstanceValid(_scenePathLabel))
			{
				_scenePathLabel.Text = BuildMapSceneLabel(_editingMap);
			}
			if (GodotObject.IsInstanceValid(_gridSummaryLabel))
			{
				_gridSummaryLabel.Text = $"格子: {_editingMap.gridNum.X} x {_editingMap.gridNum.Y}, 起点 {FormatVector(_editingMap.gridBeginPos)}, 大小 {FormatVector(_editingMap.gridSize)}";
			}
			if (GodotObject.IsInstanceValid(_gridSummaryLabel) && !_editingMap.TryValidateRuntime(out var reason))
			{
				Label gridSummaryLabel = _gridSummaryLabel;
				gridSummaryLabel.Text = gridSummaryLabel.Text + "\nRuntime validation: " + reason;
			}
			if (GodotObject.IsInstanceValid(_edgeSummaryLabel))
			{
				_edgeSummaryLabel.Text = $"边界: left={_editingMap.edge.X:0.##}, top={_editingMap.edge.Y:0.##}, right={_editingMap.edge.Z:0.##}, bottom={_editingMap.edge.W:0.##}";
			}
			if (GodotObject.IsInstanceValid(_flagsSummaryLabel))
			{
				_flagsSummaryLabel.Text = $"标记: 夜晚={FormatBool(_editingMap.isNight)}, 掉阳光={FormatBool(_editingMap.useSunFall)}, 特殊规则={BuildSpecialRuleSummary(_editingMap)}";
			}
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void QueueGridOverlayRedraw()
	{
		if (GodotObject.IsInstanceValid(_mapGridOverlay))
		{
			_mapGridOverlay.QueueRedraw();
		}
	}

	private void AddMapSummaryRows(TowerDefenseMapConfig map)
	{
		AddItemIfMissing(PreviewList, "地图场景 mapScene -> " + FormatResourcePath(map.mapScenePath));
		AddItemIfMissing(PreviewList, "背景贴图 mapTexture -> " + FormatResourcePath(map.mapTexturePath));
		AddItemIfMissing(PreviewList, "地图尺寸 mapSize -> " + FormatVector(map.mapSize));
		AddItemIfMissing(TimelineList, $"格子: {map.gridNum.X} x {map.gridNum.Y}, begin={FormatVector(map.gridBeginPos)}, size={FormatVector(map.gridSize)}");
		AddItemIfMissing(GraphList, "地图 -> mapScene -> " + FormatResourcePath(map.mapScenePath));
		AddItemIfMissing(GraphList, "地图 -> mapTexture -> " + FormatResourcePath(map.mapTexturePath));
		AddItemIfMissing(ReferenceList, "配置场景 mapScene -> " + FormatResourcePath(map.mapScenePath));
		AddItemIfMissing(ReferenceList, "背景贴图 mapTexture -> " + FormatResourcePath(map.mapTexturePath));
	}

	private static Vector4 ReadVector4(SpinBox left, SpinBox top, SpinBox right, SpinBox bottom)
	{
		return new Vector4((float)left.Value, (float)top.Value, (float)right.Value, (float)bottom.Value);
	}

	private static Vector2I GetViewportSize(TowerDefenseMapConfig map)
	{
		if (map == null)
		{
			return new Vector2I(1400, 600);
		}
		return new Vector2I(Mathf.Max(1, Mathf.RoundToInt(map.mapSize.X)), Mathf.Max(1, Mathf.RoundToInt(map.mapSize.Y)));
	}

	private static PackedScene ResolveMapPreviewScene(TowerDefenseMapConfig map)
	{
		if (map != null)
		{
			PackedScene mapScene = map.GetMapScene(cache: false);
			if (GodotObject.IsInstanceValid(mapScene))
			{
				return mapScene;
			}
		}
		return LoadDefaultFrontlawnScene();
	}

	private static PackedScene LoadDefaultFrontlawnScene()
	{
		if (GodotObject.IsInstanceValid(_defaultFrontlawnScene))
		{
			return _defaultFrontlawnScene;
		}
		if (!ResourceLoader.Exists("res://Asset/Config/Map/Frontlawn/Scene/Day/TowerDefenseMapFrontlawn.tscn"))
		{
			return null;
		}
		_defaultFrontlawnScene = ResourceLoader.Load<PackedScene>("res://Asset/Config/Map/Frontlawn/Scene/Day/TowerDefenseMapFrontlawn.tscn", null, ResourceLoader.CacheMode.Reuse);
		return _defaultFrontlawnScene;
	}

	private static string BuildMapSceneLabel(TowerDefenseMapConfig map)
	{
		if (map != null && !string.IsNullOrWhiteSpace(map.mapScenePath))
		{
			return "mapScene: " + FormatResourcePath(map.mapScenePath);
		}
		PackedScene packedScene = ResolveMapPreviewScene(map);
		if (!GodotObject.IsInstanceValid(packedScene))
		{
			return "mapScene: 未配置";
		}
		return "mapScene: 默认 Frontlawn (" + FormatResource(packedScene) + ")";
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
		if (!string.IsNullOrWhiteSpace(resource.ResourceName))
		{
			return resource.ResourceName;
		}
		return resource.GetType().Name;
	}

	private static string FormatResourcePath(string path)
	{
		if (!string.IsNullOrWhiteSpace(path))
		{
			return path.GetFile();
		}
		return "未配置";
	}

	private static string FormatVector(Vector2 value)
	{
		return $"({value.X:0.##}, {value.Y:0.##})";
	}

	private static string FormatBool(bool value)
	{
		if (!value)
		{
			return "关";
		}
		return "开";
	}

	private static string BuildSpecialRuleSummary(TowerDefenseMapConfig map)
	{
		if (map?.specialRules == null || map.specialRules.Count == 0)
		{
			return "无";
		}
		List<string> list = new List<string>(map.specialRules.Count);
		foreach (TowerDefenseMapRuleConfig specialRule in map.specialRules)
		{
			if (GodotObject.IsInstanceValid(specialRule) && !specialRule.id.IsEmpty)
			{
				list.Add(specialRule.id.ToString());
			}
		}
		if (list.Count <= 0)
		{
			return "无";
		}
		return string.Join(", ", list);
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
		return new List<MethodInfo>(82)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCompleteMapVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnEmbeddedInspectorPropertyChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "obj", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Object"), exported: false),
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.StringName, "field", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false)
			}, null),
			new MethodInfo(MethodName.RenderMapRuleRuntimePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "rule", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateMapRuleRuntimePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildMapRuleGamePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "characterName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.InstantiateMapRuleCharacter, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "scene", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false),
				new PropertyInfo(Variant.Type.Object, "parent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderMapEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "map", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindMapLayout, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "map", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnBrushSettingsChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindMapFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "map", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenMapRulePacketTypeCatalog, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MountMapSegmentedOption, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "option", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("OptionButton"), exported: false),
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureMapVisualPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeMapVisualChoices, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeMapPropertyBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMapVisualPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshMapField, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "refreshOverlay", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "rebuildPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMapResourcePath, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "rebuildPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildLineUseControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetLineEnabled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "line", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "enabled", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildSpecialRuleControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddSpecialRule, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReplaceSpecialRule, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "rule", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSpecialRule, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MoveSpecialRule, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "direction", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetSpecialRules, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OpenSpecialRule, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "index", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "rule", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshMapEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearDynamicChildren, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Node"), exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildMapScenePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResetMapPreviewView, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetMapCanvasZoom, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "zoom", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "pivot", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateMapCanvasTransform, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.MapWorldToView, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "world", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.MapViewToWorld, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "view", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateMapSceneViewportSize, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.GetMapPreviewViewSize, new PropertyInfo(Variant.Type.Vector2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawMapGridOverlay, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "overlay", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnMapGridOverlayGuiInput, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "inputEvent", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("InputEvent"), exported: false)
			}, null),
			new MethodInfo(MethodName.PanMapCanvasTo, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.StartGridBoxSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateGridBoxSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FinishGridBoxSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyBrushToSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PaintBrushAtCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "cellConfigs", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCellConfigBrushData, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildBrushGridTypeArray, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplySelectedCellConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsureCellConfigForSelection, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "cellConfigs", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Rect2I, "selection", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CloneCellConfigs, new PropertyInfo(Variant.Type.Array, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "source", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCellConfigs, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Array, "next", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCellAtPosition, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "position", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCellRect, new PropertyInfo(Variant.Type.Rect2, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "x", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Int, "y", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "begin", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "cellSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.NormalizeCellSelection, new PropertyInfo(Variant.Type.Rect2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "a", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2I, "b", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ToCellConfigPos, new PropertyInfo(Variant.Type.Vector4I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Rect2I, "selection", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawConfiguredCellRects, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "overlay", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "begin", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "cellSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Color, "color", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawCellSelection, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "overlay", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Vector2, "begin", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "cellSize", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DrawBrushPreviewCell, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "overlay", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false),
				new PropertyInfo(Variant.Type.Rect2, "rect", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCellConfigColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RectFromCellConfigPos, new PropertyInfo(Variant.Type.Rect2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector4I, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCellSelectionLabel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyBrushGridPreset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "preset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCellSelectionText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildBrushSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SavePendingMapResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveMapResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "map", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateMapSummary, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.QueueGridOverlayRedraw, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddMapSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "map", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadVector4, new PropertyInfo(Variant.Type.Vector4, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "left", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Object, "top", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Object, "right", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false),
				new PropertyInfo(Variant.Type.Object, "bottom", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetViewportSize, new PropertyInfo(Variant.Type.Vector2I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "map", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveMapPreviewScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "map", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadDefaultFrontlawnScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal | MethodFlags.Static, null, null),
			new MethodInfo(MethodName.BuildMapSceneLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "map", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatResource, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.FormatResourcePath, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "path", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatVector, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.FormatBool, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "value", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildSpecialRuleSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "map", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.HasCompleteMapVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteMapVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.OnEmbeddedInspectorPropertyChanged && args.Count == 4)
		{
			OnEmbeddedInspectorPropertyChanged(VariantUtils.ConvertTo<GodotObject>(in args[0]), VariantUtils.ConvertTo<StringName>(in args[1]), VariantUtils.ConvertTo<StringName>(in args[2]), VariantUtils.ConvertTo<Variant>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RenderMapRuleRuntimePreview && args.Count == 1)
		{
			RenderMapRuleRuntimePreview(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMapRuleRuntimePreview && args.Count == 0)
		{
			UpdateMapRuleRuntimePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildMapRuleGamePreview && args.Count == 1)
		{
			RebuildMapRuleGamePreview(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.InstantiateMapRuleCharacter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(InstantiateMapRuleCharacter(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.RenderMapEditor && args.Count == 1)
		{
			RenderMapEditor(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindMapLayout && args.Count == 2)
		{
			BindMapLayout(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnBrushSettingsChanged && args.Count == 0)
		{
			OnBrushSettingsChanged();
			ret = default;
			return true;
		}
		if (method == MethodName.BindMapFields && args.Count == 2)
		{
			BindMapFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenMapRulePacketTypeCatalog && args.Count == 0)
		{
			OpenMapRulePacketTypeCatalog();
			ret = default;
			return true;
		}
		if (method == MethodName.MountMapSegmentedOption && args.Count == 2)
		{
			MountMapSegmentedOption(VariantUtils.ConvertTo<OptionButton>(in args[0]), VariantUtils.ConvertTo<HFlowContainer>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureMapVisualPicker && args.Count == 0)
		{
			EnsureMapVisualPicker();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeMapVisualChoices && args.Count == 0)
		{
			DisposeMapVisualChoices();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeMapPropertyBinding && args.Count == 0)
		{
			DisposeMapPropertyBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMapVisualPropertyEdited && args.Count == 1)
		{
			OnMapVisualPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshMapField && args.Count == 3)
		{
			RefreshMapField(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMapResourcePath && args.Count == 4)
		{
			SetMapResourcePath(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]), VariantUtils.ConvertTo<bool>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildLineUseControls && args.Count == 0)
		{
			RebuildLineUseControls();
			ret = default;
			return true;
		}
		if (method == MethodName.SetLineEnabled && args.Count == 2)
		{
			SetLineEnabled(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildSpecialRuleControls && args.Count == 0)
		{
			RebuildSpecialRuleControls();
			ret = default;
			return true;
		}
		if (method == MethodName.AddSpecialRule && args.Count == 0)
		{
			AddSpecialRule();
			ret = default;
			return true;
		}
		if (method == MethodName.ReplaceSpecialRule && args.Count == 2)
		{
			ReplaceSpecialRule(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapRuleConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSpecialRule && args.Count == 1)
		{
			RemoveSpecialRule(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.MoveSpecialRule && args.Count == 2)
		{
			MoveSpecialRule(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetSpecialRules && args.Count == 2)
		{
			SetSpecialRules(VariantUtils.ConvertToArray<TowerDefenseMapRuleConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenSpecialRule && args.Count == 2)
		{
			OpenSpecialRule(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<TowerDefenseMapRuleConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshMapEditorFromHistory && args.Count == 0)
		{
			RefreshMapEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearDynamicChildren && args.Count == 1)
		{
			ClearDynamicChildren(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildMapScenePreview && args.Count == 0)
		{
			RebuildMapScenePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.ResetMapPreviewView && args.Count == 0)
		{
			ResetMapPreviewView();
			ret = default;
			return true;
		}
		if (method == MethodName.SetMapCanvasZoom && args.Count == 2)
		{
			SetMapCanvasZoom(VariantUtils.ConvertTo<float>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMapCanvasTransform && args.Count == 0)
		{
			UpdateMapCanvasTransform();
			ret = default;
			return true;
		}
		if (method == MethodName.MapWorldToView && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(MapWorldToView(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.MapViewToWorld && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2>(MapViewToWorld(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateMapSceneViewportSize && args.Count == 0)
		{
			UpdateMapSceneViewportSize();
			ret = default;
			return true;
		}
		if (method == MethodName.GetMapPreviewViewSize && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector2>(GetMapPreviewViewSize());
			return true;
		}
		if (method == MethodName.DrawMapGridOverlay && args.Count == 1)
		{
			DrawMapGridOverlay(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnMapGridOverlayGuiInput && args.Count == 1)
		{
			OnMapGridOverlayGuiInput(VariantUtils.ConvertTo<InputEvent>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PanMapCanvasTo && args.Count == 1)
		{
			PanMapCanvasTo(VariantUtils.ConvertTo<Vector2>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.StartGridBoxSelection && args.Count == 1)
		{
			StartGridBoxSelection(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateGridBoxSelection && args.Count == 1)
		{
			UpdateGridBoxSelection(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.FinishGridBoxSelection && args.Count == 0)
		{
			FinishGridBoxSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyBrushToSelection && args.Count == 0)
		{
			ApplyBrushToSelection();
			ret = default;
			return true;
		}
		if (method == MethodName.PaintBrushAtCell && args.Count == 2)
		{
			PaintBrushAtCell(VariantUtils.ConvertToArray<TowerDefenseCellConfig>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCellConfigBrushData && args.Count == 1)
		{
			SetCellConfigBrushData(VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildBrushGridTypeArray && args.Count == 0)
		{
			Array<TowerDefenseEnum.PLANTGRIDTYPE> array = BuildBrushGridTypeArray();
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.ApplySelectedCellConfig && args.Count == 0)
		{
			ApplySelectedCellConfig();
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCellConfigForSelection && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureCellConfigForSelection(VariantUtils.ConvertToArray<TowerDefenseCellConfig>(in args[0]), VariantUtils.ConvertTo<Rect2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CloneCellConfigs && args.Count == 1)
		{
			Array<TowerDefenseCellConfig> array2 = CloneCellConfigs(VariantUtils.ConvertToArray<TowerDefenseCellConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array2);
			return true;
		}
		if (method == MethodName.SetCellConfigs && args.Count == 2)
		{
			SetCellConfigs(VariantUtils.ConvertToArray<TowerDefenseCellConfig>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCellAtPosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetCellAtPosition(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCellRect && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetCellRect(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.NormalizeCellSelection && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2I>(NormalizeCellSelection(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.ToCellConfigPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector4I>(ToCellConfigPos(VariantUtils.ConvertTo<Rect2I>(in args[0])));
			return true;
		}
		if (method == MethodName.DrawConfiguredCellRects && args.Count == 4)
		{
			DrawConfiguredCellRects(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<Color>(in args[3]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawCellSelection && args.Count == 3)
		{
			DrawCellSelection(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<Vector2>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.DrawBrushPreviewCell && args.Count == 2)
		{
			DrawBrushPreviewCell(VariantUtils.ConvertTo<Control>(in args[0]), VariantUtils.ConvertTo<Rect2>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.GetCellConfigColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetCellConfigColor(VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.RectFromCellConfigPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2I>(RectFromCellConfigPos(VariantUtils.ConvertTo<Vector4I>(in args[0])));
			return true;
		}
		if (method == MethodName.UpdateCellSelectionLabel && args.Count == 0)
		{
			UpdateCellSelectionLabel();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyBrushGridPreset && args.Count == 1)
		{
			ApplyBrushGridPreset(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildCellSelectionText && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCellSelectionText());
			return true;
		}
		if (method == MethodName.BuildBrushSummary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildBrushSummary());
			return true;
		}
		if (method == MethodName.SavePendingMapResource && args.Count == 0)
		{
			SavePendingMapResource();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveMapResource && args.Count == 1)
		{
			SaveMapResource(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateMapSummary && args.Count == 0)
		{
			UpdateMapSummary();
			ret = default;
			return true;
		}
		if (method == MethodName.QueueGridOverlayRedraw && args.Count == 0)
		{
			QueueGridOverlayRedraw();
			ret = default;
			return true;
		}
		if (method == MethodName.AddMapSummaryRows && args.Count == 1)
		{
			AddMapSummaryRows(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadVector4 && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Vector4>(ReadVector4(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2]), VariantUtils.ConvertTo<SpinBox>(in args[3])));
			return true;
		}
		if (method == MethodName.GetViewportSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetViewportSize(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveMapPreviewScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(ResolveMapPreviewScene(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadDefaultFrontlawnScene && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(LoadDefaultFrontlawnScene());
			return true;
		}
		if (method == MethodName.BuildMapSceneLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildMapSceneLabel(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVector && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVector(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatBool && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatBool(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildSpecialRuleSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSpecialRuleSummary(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
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
		if (method == MethodName.HasCompleteMapVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteMapVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.InstantiateMapRuleCharacter && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<TowerDefenseCharacter>(InstantiateMapRuleCharacter(VariantUtils.ConvertTo<PackedScene>(in args[0]), VariantUtils.ConvertTo<Node>(in args[1])));
			return true;
		}
		if (method == MethodName.ClearDynamicChildren && args.Count == 1)
		{
			ClearDynamicChildren(VariantUtils.ConvertTo<Node>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCellConfigForSelection && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(EnsureCellConfigForSelection(VariantUtils.ConvertToArray<TowerDefenseCellConfig>(in args[0]), VariantUtils.ConvertTo<Rect2I>(in args[1])));
			return true;
		}
		if (method == MethodName.CloneCellConfigs && args.Count == 1)
		{
			Array<TowerDefenseCellConfig> array = CloneCellConfigs(VariantUtils.ConvertToArray<TowerDefenseCellConfig>(in args[0]));
			ret = VariantUtils.CreateFromArray(array);
			return true;
		}
		if (method == MethodName.GetCellRect && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Rect2>(GetCellRect(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<int>(in args[1]), VariantUtils.ConvertTo<Vector2>(in args[2]), VariantUtils.ConvertTo<Vector2>(in args[3])));
			return true;
		}
		if (method == MethodName.NormalizeCellSelection && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Rect2I>(NormalizeCellSelection(VariantUtils.ConvertTo<Vector2I>(in args[0]), VariantUtils.ConvertTo<Vector2I>(in args[1])));
			return true;
		}
		if (method == MethodName.ToCellConfigPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector4I>(ToCellConfigPos(VariantUtils.ConvertTo<Rect2I>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCellConfigColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetCellConfigColor(VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.RectFromCellConfigPos && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Rect2I>(RectFromCellConfigPos(VariantUtils.ConvertTo<Vector4I>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadVector4 && args.Count == 4)
		{
			ret = VariantUtils.CreateFrom<Vector4>(ReadVector4(VariantUtils.ConvertTo<SpinBox>(in args[0]), VariantUtils.ConvertTo<SpinBox>(in args[1]), VariantUtils.ConvertTo<SpinBox>(in args[2]), VariantUtils.ConvertTo<SpinBox>(in args[3])));
			return true;
		}
		if (method == MethodName.GetViewportSize && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector2I>(GetViewportSize(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.ResolveMapPreviewScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(ResolveMapPreviewScene(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.LoadDefaultFrontlawnScene && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(LoadDefaultFrontlawnScene());
			return true;
		}
		if (method == MethodName.BuildMapSceneLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildMapSceneLabel(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatResourcePath && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResourcePath(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatVector && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatVector(VariantUtils.ConvertTo<Vector2>(in args[0])));
			return true;
		}
		if (method == MethodName.FormatBool && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatBool(VariantUtils.ConvertTo<bool>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildSpecialRuleSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildSpecialRuleSummary(VariantUtils.ConvertTo<TowerDefenseMapConfig>(in args[0])));
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
		if (method == MethodName.HasCompleteMapVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.OnEmbeddedInspectorPropertyChanged)
		{
			return true;
		}
		if (method == MethodName.RenderMapRuleRuntimePreview)
		{
			return true;
		}
		if (method == MethodName.UpdateMapRuleRuntimePreview)
		{
			return true;
		}
		if (method == MethodName.RebuildMapRuleGamePreview)
		{
			return true;
		}
		if (method == MethodName.InstantiateMapRuleCharacter)
		{
			return true;
		}
		if (method == MethodName.RenderMapEditor)
		{
			return true;
		}
		if (method == MethodName.BindMapLayout)
		{
			return true;
		}
		if (method == MethodName.OnBrushSettingsChanged)
		{
			return true;
		}
		if (method == MethodName.BindMapFields)
		{
			return true;
		}
		if (method == MethodName.OpenMapRulePacketTypeCatalog)
		{
			return true;
		}
		if (method == MethodName.MountMapSegmentedOption)
		{
			return true;
		}
		if (method == MethodName.EnsureMapVisualPicker)
		{
			return true;
		}
		if (method == MethodName.DisposeMapVisualChoices)
		{
			return true;
		}
		if (method == MethodName.DisposeMapPropertyBinding)
		{
			return true;
		}
		if (method == MethodName.OnMapVisualPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshMapField)
		{
			return true;
		}
		if (method == MethodName.SetMapResourcePath)
		{
			return true;
		}
		if (method == MethodName.RebuildLineUseControls)
		{
			return true;
		}
		if (method == MethodName.SetLineEnabled)
		{
			return true;
		}
		if (method == MethodName.RebuildSpecialRuleControls)
		{
			return true;
		}
		if (method == MethodName.AddSpecialRule)
		{
			return true;
		}
		if (method == MethodName.ReplaceSpecialRule)
		{
			return true;
		}
		if (method == MethodName.RemoveSpecialRule)
		{
			return true;
		}
		if (method == MethodName.MoveSpecialRule)
		{
			return true;
		}
		if (method == MethodName.SetSpecialRules)
		{
			return true;
		}
		if (method == MethodName.OpenSpecialRule)
		{
			return true;
		}
		if (method == MethodName.RefreshMapEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.ClearDynamicChildren)
		{
			return true;
		}
		if (method == MethodName.RebuildMapScenePreview)
		{
			return true;
		}
		if (method == MethodName.ResetMapPreviewView)
		{
			return true;
		}
		if (method == MethodName.SetMapCanvasZoom)
		{
			return true;
		}
		if (method == MethodName.UpdateMapCanvasTransform)
		{
			return true;
		}
		if (method == MethodName.MapWorldToView)
		{
			return true;
		}
		if (method == MethodName.MapViewToWorld)
		{
			return true;
		}
		if (method == MethodName.UpdateMapSceneViewportSize)
		{
			return true;
		}
		if (method == MethodName.GetMapPreviewViewSize)
		{
			return true;
		}
		if (method == MethodName.DrawMapGridOverlay)
		{
			return true;
		}
		if (method == MethodName.OnMapGridOverlayGuiInput)
		{
			return true;
		}
		if (method == MethodName.PanMapCanvasTo)
		{
			return true;
		}
		if (method == MethodName.StartGridBoxSelection)
		{
			return true;
		}
		if (method == MethodName.UpdateGridBoxSelection)
		{
			return true;
		}
		if (method == MethodName.FinishGridBoxSelection)
		{
			return true;
		}
		if (method == MethodName.ApplyBrushToSelection)
		{
			return true;
		}
		if (method == MethodName.PaintBrushAtCell)
		{
			return true;
		}
		if (method == MethodName.SetCellConfigBrushData)
		{
			return true;
		}
		if (method == MethodName.BuildBrushGridTypeArray)
		{
			return true;
		}
		if (method == MethodName.ApplySelectedCellConfig)
		{
			return true;
		}
		if (method == MethodName.EnsureCellConfigForSelection)
		{
			return true;
		}
		if (method == MethodName.CloneCellConfigs)
		{
			return true;
		}
		if (method == MethodName.SetCellConfigs)
		{
			return true;
		}
		if (method == MethodName.GetCellAtPosition)
		{
			return true;
		}
		if (method == MethodName.GetCellRect)
		{
			return true;
		}
		if (method == MethodName.NormalizeCellSelection)
		{
			return true;
		}
		if (method == MethodName.ToCellConfigPos)
		{
			return true;
		}
		if (method == MethodName.DrawConfiguredCellRects)
		{
			return true;
		}
		if (method == MethodName.DrawCellSelection)
		{
			return true;
		}
		if (method == MethodName.DrawBrushPreviewCell)
		{
			return true;
		}
		if (method == MethodName.GetCellConfigColor)
		{
			return true;
		}
		if (method == MethodName.RectFromCellConfigPos)
		{
			return true;
		}
		if (method == MethodName.UpdateCellSelectionLabel)
		{
			return true;
		}
		if (method == MethodName.ApplyBrushGridPreset)
		{
			return true;
		}
		if (method == MethodName.BuildCellSelectionText)
		{
			return true;
		}
		if (method == MethodName.BuildBrushSummary)
		{
			return true;
		}
		if (method == MethodName.SavePendingMapResource)
		{
			return true;
		}
		if (method == MethodName.SaveMapResource)
		{
			return true;
		}
		if (method == MethodName.UpdateMapSummary)
		{
			return true;
		}
		if (method == MethodName.QueueGridOverlayRedraw)
		{
			return true;
		}
		if (method == MethodName.AddMapSummaryRows)
		{
			return true;
		}
		if (method == MethodName.ReadVector4)
		{
			return true;
		}
		if (method == MethodName.GetViewportSize)
		{
			return true;
		}
		if (method == MethodName.ResolveMapPreviewScene)
		{
			return true;
		}
		if (method == MethodName.LoadDefaultFrontlawnScene)
		{
			return true;
		}
		if (method == MethodName.BuildMapSceneLabel)
		{
			return true;
		}
		if (method == MethodName.FormatResource)
		{
			return true;
		}
		if (method == MethodName.FormatResourcePath)
		{
			return true;
		}
		if (method == MethodName.FormatVector)
		{
			return true;
		}
		if (method == MethodName.FormatBool)
		{
			return true;
		}
		if (method == MethodName.BuildSpecialRuleSummary)
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
		if (name == PropertyName._editingMap)
		{
			_editingMap = VariantUtils.ConvertTo<TowerDefenseMapConfig>(in value);
			return true;
		}
		if (name == PropertyName._lineUseHost)
		{
			_lineUseHost = VariantUtils.ConvertTo<FlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._specialRulesHost)
		{
			_specialRulesHost = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._specialRulesSummary)
		{
			_specialRulesSummary = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._mapSceneViewport)
		{
			_mapSceneViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._mapScenePreviewRoot)
		{
			_mapScenePreviewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._mapPreviewStack)
		{
			_mapPreviewStack = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._mapGridOverlay)
		{
			_mapGridOverlay = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._scenePathLabel)
		{
			_scenePathLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._gridSummaryLabel)
		{
			_gridSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._edgeSummaryLabel)
		{
			_edgeSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._flagsSummaryLabel)
		{
			_flagsSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._cellSelectionLabel)
		{
			_cellSelectionLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._useCellBrushTool)
		{
			_useCellBrushTool = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._brushGridTypeOption)
		{
			_brushGridTypeOption = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._brushGroundCheckBox)
		{
			_brushGroundCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._brushAirCheckBox)
		{
			_brushAirCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._brushWaterCheckBox)
		{
			_brushWaterCheckBox = VariantUtils.ConvertTo<CheckBox>(in value);
			return true;
		}
		if (name == PropertyName._brushElementFlagsSpinBox)
		{
			_brushElementFlagsSpinBox = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._isGridBoxSelecting)
		{
			_isGridBoxSelecting = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.MapCanvasPan)
		{
			MapCanvasPan = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.MapCanvasLastPointer)
		{
			MapCanvasLastPointer = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName.MapCanvasZoom)
		{
			MapCanvasZoom = VariantUtils.ConvertTo<float>(in value);
			return true;
		}
		if (name == PropertyName.MapCanvasOffset)
		{
			MapCanvasOffset = VariantUtils.ConvertTo<Vector2>(in value);
			return true;
		}
		if (name == PropertyName._gridSelectionStart)
		{
			_gridSelectionStart = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName._gridSelectionEnd)
		{
			_gridSelectionEnd = VariantUtils.ConvertTo<Vector2I>(in value);
			return true;
		}
		if (name == PropertyName.MapGridSelectionRect)
		{
			MapGridSelectionRect = VariantUtils.ConvertTo<Rect2I>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._mapSaveTimer)
		{
			_mapSaveTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._editingMapRuleResource)
		{
			_editingMapRuleResource = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleBaselineRoot)
		{
			_mapRuleBaselineRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleResultRoot)
		{
			_mapRuleResultRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._mapRulePreviewStatus)
		{
			_mapRulePreviewStatus = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._mapRulePreviewResult)
		{
			_mapRulePreviewResult = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._mapRulePreviewValidation)
		{
			_mapRulePreviewValidation = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._mapRulePacketType)
		{
			_mapRulePacketType = VariantUtils.ConvertTo<OptionButton>(in value);
			return true;
		}
		if (name == PropertyName._mapRulePacketTypeCatalogButton)
		{
			_mapRulePacketTypeCatalogButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._mapVisualPicker)
		{
			_mapVisualPicker = VariantUtils.ConvertTo<XWGameplayResourcePickerWindow>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleCharacterName)
		{
			_mapRuleCharacterName = VariantUtils.ConvertTo<LineEdit>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleBaseCooldown)
		{
			_mapRuleBaseCooldown = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleBaseSpeed)
		{
			_mapRuleBaseSpeed = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleBaseHitpoints)
		{
			_mapRuleBaseHitpoints = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleBaseScale)
		{
			_mapRuleBaseScale = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleBaseDps)
		{
			_mapRuleBaseDps = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleResultCooldown)
		{
			_mapRuleResultCooldown = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleResultSpeed)
		{
			_mapRuleResultSpeed = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleResultHitpoints)
		{
			_mapRuleResultHitpoints = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		if (name == PropertyName._mapRuleResultScale)
		{
			_mapRuleResultScale = VariantUtils.ConvertTo<double>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.UseCellBrushTool)
		{
			value = VariantUtils.CreateFrom<bool>(UseCellBrushTool);
			return true;
		}
		if (name == PropertyName._editingMap)
		{
			value = VariantUtils.CreateFrom(in _editingMap);
			return true;
		}
		if (name == PropertyName._lineUseHost)
		{
			value = VariantUtils.CreateFrom(in _lineUseHost);
			return true;
		}
		if (name == PropertyName._specialRulesHost)
		{
			value = VariantUtils.CreateFrom(in _specialRulesHost);
			return true;
		}
		if (name == PropertyName._specialRulesSummary)
		{
			value = VariantUtils.CreateFrom(in _specialRulesSummary);
			return true;
		}
		if (name == PropertyName._mapSceneViewport)
		{
			value = VariantUtils.CreateFrom(in _mapSceneViewport);
			return true;
		}
		if (name == PropertyName._mapScenePreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _mapScenePreviewRoot);
			return true;
		}
		if (name == PropertyName._mapPreviewStack)
		{
			value = VariantUtils.CreateFrom(in _mapPreviewStack);
			return true;
		}
		if (name == PropertyName._mapGridOverlay)
		{
			value = VariantUtils.CreateFrom(in _mapGridOverlay);
			return true;
		}
		if (name == PropertyName._scenePathLabel)
		{
			value = VariantUtils.CreateFrom(in _scenePathLabel);
			return true;
		}
		if (name == PropertyName._gridSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _gridSummaryLabel);
			return true;
		}
		if (name == PropertyName._edgeSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _edgeSummaryLabel);
			return true;
		}
		if (name == PropertyName._flagsSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _flagsSummaryLabel);
			return true;
		}
		if (name == PropertyName._cellSelectionLabel)
		{
			value = VariantUtils.CreateFrom(in _cellSelectionLabel);
			return true;
		}
		if (name == PropertyName._useCellBrushTool)
		{
			value = VariantUtils.CreateFrom(in _useCellBrushTool);
			return true;
		}
		if (name == PropertyName._brushGridTypeOption)
		{
			value = VariantUtils.CreateFrom(in _brushGridTypeOption);
			return true;
		}
		if (name == PropertyName._brushGroundCheckBox)
		{
			value = VariantUtils.CreateFrom(in _brushGroundCheckBox);
			return true;
		}
		if (name == PropertyName._brushAirCheckBox)
		{
			value = VariantUtils.CreateFrom(in _brushAirCheckBox);
			return true;
		}
		if (name == PropertyName._brushWaterCheckBox)
		{
			value = VariantUtils.CreateFrom(in _brushWaterCheckBox);
			return true;
		}
		if (name == PropertyName._brushElementFlagsSpinBox)
		{
			value = VariantUtils.CreateFrom(in _brushElementFlagsSpinBox);
			return true;
		}
		if (name == PropertyName._isGridBoxSelecting)
		{
			value = VariantUtils.CreateFrom(in _isGridBoxSelecting);
			return true;
		}
		if (name == PropertyName.MapCanvasPan)
		{
			value = VariantUtils.CreateFrom(in MapCanvasPan);
			return true;
		}
		if (name == PropertyName.MapCanvasLastPointer)
		{
			value = VariantUtils.CreateFrom(in MapCanvasLastPointer);
			return true;
		}
		if (name == PropertyName.MapCanvasZoom)
		{
			value = VariantUtils.CreateFrom(in MapCanvasZoom);
			return true;
		}
		if (name == PropertyName.MapCanvasOffset)
		{
			value = VariantUtils.CreateFrom(in MapCanvasOffset);
			return true;
		}
		if (name == PropertyName._gridSelectionStart)
		{
			value = VariantUtils.CreateFrom(in _gridSelectionStart);
			return true;
		}
		if (name == PropertyName._gridSelectionEnd)
		{
			value = VariantUtils.CreateFrom(in _gridSelectionEnd);
			return true;
		}
		if (name == PropertyName.MapGridSelectionRect)
		{
			value = VariantUtils.CreateFrom(in MapGridSelectionRect);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._mapSaveTimer)
		{
			value = VariantUtils.CreateFrom(in _mapSaveTimer);
			return true;
		}
		if (name == PropertyName._editingMapRuleResource)
		{
			value = VariantUtils.CreateFrom(in _editingMapRuleResource);
			return true;
		}
		if (name == PropertyName._mapRuleBaselineRoot)
		{
			value = VariantUtils.CreateFrom(in _mapRuleBaselineRoot);
			return true;
		}
		if (name == PropertyName._mapRuleResultRoot)
		{
			value = VariantUtils.CreateFrom(in _mapRuleResultRoot);
			return true;
		}
		if (name == PropertyName._mapRulePreviewStatus)
		{
			value = VariantUtils.CreateFrom(in _mapRulePreviewStatus);
			return true;
		}
		if (name == PropertyName._mapRulePreviewResult)
		{
			value = VariantUtils.CreateFrom(in _mapRulePreviewResult);
			return true;
		}
		if (name == PropertyName._mapRulePreviewValidation)
		{
			value = VariantUtils.CreateFrom(in _mapRulePreviewValidation);
			return true;
		}
		if (name == PropertyName._mapRulePacketType)
		{
			value = VariantUtils.CreateFrom(in _mapRulePacketType);
			return true;
		}
		if (name == PropertyName._mapRulePacketTypeCatalogButton)
		{
			value = VariantUtils.CreateFrom(in _mapRulePacketTypeCatalogButton);
			return true;
		}
		if (name == PropertyName._mapVisualPicker)
		{
			value = VariantUtils.CreateFrom(in _mapVisualPicker);
			return true;
		}
		if (name == PropertyName._mapRuleCharacterName)
		{
			value = VariantUtils.CreateFrom(in _mapRuleCharacterName);
			return true;
		}
		if (name == PropertyName._mapRuleBaseCooldown)
		{
			value = VariantUtils.CreateFrom(in _mapRuleBaseCooldown);
			return true;
		}
		if (name == PropertyName._mapRuleBaseSpeed)
		{
			value = VariantUtils.CreateFrom(in _mapRuleBaseSpeed);
			return true;
		}
		if (name == PropertyName._mapRuleBaseHitpoints)
		{
			value = VariantUtils.CreateFrom(in _mapRuleBaseHitpoints);
			return true;
		}
		if (name == PropertyName._mapRuleBaseScale)
		{
			value = VariantUtils.CreateFrom(in _mapRuleBaseScale);
			return true;
		}
		if (name == PropertyName._mapRuleBaseDps)
		{
			value = VariantUtils.CreateFrom(in _mapRuleBaseDps);
			return true;
		}
		if (name == PropertyName._mapRuleResultCooldown)
		{
			value = VariantUtils.CreateFrom(in _mapRuleResultCooldown);
			return true;
		}
		if (name == PropertyName._mapRuleResultSpeed)
		{
			value = VariantUtils.CreateFrom(in _mapRuleResultSpeed);
			return true;
		}
		if (name == PropertyName._mapRuleResultHitpoints)
		{
			value = VariantUtils.CreateFrom(in _mapRuleResultHitpoints);
			return true;
		}
		if (name == PropertyName._mapRuleResultScale)
		{
			value = VariantUtils.CreateFrom(in _mapRuleResultScale);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingMap, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._lineUseHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._specialRulesHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._specialRulesSummary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapSceneViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapScenePreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapPreviewStack, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapGridOverlay, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._scenePathLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._gridSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._edgeSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._flagsSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._cellSelectionLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._useCellBrushTool, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._brushGridTypeOption, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._brushGroundCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._brushAirCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._brushWaterCheckBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._brushElementFlagsSpinBox, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._isGridBoxSelecting, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.MapCanvasPan, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.MapCanvasLastPointer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName.MapCanvasZoom, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2, PropertyName.MapCanvasOffset, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._gridSelectionStart, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Vector2I, PropertyName._gridSelectionEnd, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Rect2I, PropertyName.MapGridSelectionRect, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapSaveTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingMapRuleResource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRuleBaselineRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRuleResultRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRulePreviewStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRulePreviewResult, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRulePreviewValidation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRulePacketType, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRulePacketTypeCatalogButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapVisualPicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRuleCharacterName, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRuleBaseCooldown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRuleBaseSpeed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRuleBaseHitpoints, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRuleBaseScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._mapRuleBaseDps, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._mapRuleResultCooldown, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._mapRuleResultSpeed, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._mapRuleResultHitpoints, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Float, PropertyName._mapRuleResultScale, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.UseCellBrushTool, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingMap, Variant.From(in _editingMap));
		info.AddProperty(PropertyName._lineUseHost, Variant.From(in _lineUseHost));
		info.AddProperty(PropertyName._specialRulesHost, Variant.From(in _specialRulesHost));
		info.AddProperty(PropertyName._specialRulesSummary, Variant.From(in _specialRulesSummary));
		info.AddProperty(PropertyName._mapSceneViewport, Variant.From(in _mapSceneViewport));
		info.AddProperty(PropertyName._mapScenePreviewRoot, Variant.From(in _mapScenePreviewRoot));
		info.AddProperty(PropertyName._mapPreviewStack, Variant.From(in _mapPreviewStack));
		info.AddProperty(PropertyName._mapGridOverlay, Variant.From(in _mapGridOverlay));
		info.AddProperty(PropertyName._scenePathLabel, Variant.From(in _scenePathLabel));
		info.AddProperty(PropertyName._gridSummaryLabel, Variant.From(in _gridSummaryLabel));
		info.AddProperty(PropertyName._edgeSummaryLabel, Variant.From(in _edgeSummaryLabel));
		info.AddProperty(PropertyName._flagsSummaryLabel, Variant.From(in _flagsSummaryLabel));
		info.AddProperty(PropertyName._cellSelectionLabel, Variant.From(in _cellSelectionLabel));
		info.AddProperty(PropertyName._useCellBrushTool, Variant.From(in _useCellBrushTool));
		info.AddProperty(PropertyName._brushGridTypeOption, Variant.From(in _brushGridTypeOption));
		info.AddProperty(PropertyName._brushGroundCheckBox, Variant.From(in _brushGroundCheckBox));
		info.AddProperty(PropertyName._brushAirCheckBox, Variant.From(in _brushAirCheckBox));
		info.AddProperty(PropertyName._brushWaterCheckBox, Variant.From(in _brushWaterCheckBox));
		info.AddProperty(PropertyName._brushElementFlagsSpinBox, Variant.From(in _brushElementFlagsSpinBox));
		info.AddProperty(PropertyName._isGridBoxSelecting, Variant.From(in _isGridBoxSelecting));
		info.AddProperty(PropertyName.MapCanvasPan, Variant.From(in MapCanvasPan));
		info.AddProperty(PropertyName.MapCanvasLastPointer, Variant.From(in MapCanvasLastPointer));
		info.AddProperty(PropertyName.MapCanvasZoom, Variant.From(in MapCanvasZoom));
		info.AddProperty(PropertyName.MapCanvasOffset, Variant.From(in MapCanvasOffset));
		info.AddProperty(PropertyName._gridSelectionStart, Variant.From(in _gridSelectionStart));
		info.AddProperty(PropertyName._gridSelectionEnd, Variant.From(in _gridSelectionEnd));
		info.AddProperty(PropertyName.MapGridSelectionRect, Variant.From(in MapGridSelectionRect));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._mapSaveTimer, Variant.From(in _mapSaveTimer));
		info.AddProperty(PropertyName._editingMapRuleResource, Variant.From(in _editingMapRuleResource));
		info.AddProperty(PropertyName._mapRuleBaselineRoot, Variant.From(in _mapRuleBaselineRoot));
		info.AddProperty(PropertyName._mapRuleResultRoot, Variant.From(in _mapRuleResultRoot));
		info.AddProperty(PropertyName._mapRulePreviewStatus, Variant.From(in _mapRulePreviewStatus));
		info.AddProperty(PropertyName._mapRulePreviewResult, Variant.From(in _mapRulePreviewResult));
		info.AddProperty(PropertyName._mapRulePreviewValidation, Variant.From(in _mapRulePreviewValidation));
		info.AddProperty(PropertyName._mapRulePacketType, Variant.From(in _mapRulePacketType));
		info.AddProperty(PropertyName._mapRulePacketTypeCatalogButton, Variant.From(in _mapRulePacketTypeCatalogButton));
		info.AddProperty(PropertyName._mapVisualPicker, Variant.From(in _mapVisualPicker));
		info.AddProperty(PropertyName._mapRuleCharacterName, Variant.From(in _mapRuleCharacterName));
		info.AddProperty(PropertyName._mapRuleBaseCooldown, Variant.From(in _mapRuleBaseCooldown));
		info.AddProperty(PropertyName._mapRuleBaseSpeed, Variant.From(in _mapRuleBaseSpeed));
		info.AddProperty(PropertyName._mapRuleBaseHitpoints, Variant.From(in _mapRuleBaseHitpoints));
		info.AddProperty(PropertyName._mapRuleBaseScale, Variant.From(in _mapRuleBaseScale));
		info.AddProperty(PropertyName._mapRuleBaseDps, Variant.From(in _mapRuleBaseDps));
		info.AddProperty(PropertyName._mapRuleResultCooldown, Variant.From(in _mapRuleResultCooldown));
		info.AddProperty(PropertyName._mapRuleResultSpeed, Variant.From(in _mapRuleResultSpeed));
		info.AddProperty(PropertyName._mapRuleResultHitpoints, Variant.From(in _mapRuleResultHitpoints));
		info.AddProperty(PropertyName._mapRuleResultScale, Variant.From(in _mapRuleResultScale));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingMap, out var value))
		{
			_editingMap = value.As<TowerDefenseMapConfig>();
		}
		if (info.TryGetProperty(PropertyName._lineUseHost, out var value2))
		{
			_lineUseHost = value2.As<FlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._specialRulesHost, out var value3))
		{
			_specialRulesHost = value3.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._specialRulesSummary, out var value4))
		{
			_specialRulesSummary = value4.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._mapSceneViewport, out var value5))
		{
			_mapSceneViewport = value5.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._mapScenePreviewRoot, out var value6))
		{
			_mapScenePreviewRoot = value6.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._mapPreviewStack, out var value7))
		{
			_mapPreviewStack = value7.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._mapGridOverlay, out var value8))
		{
			_mapGridOverlay = value8.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._scenePathLabel, out var value9))
		{
			_scenePathLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._gridSummaryLabel, out var value10))
		{
			_gridSummaryLabel = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._edgeSummaryLabel, out var value11))
		{
			_edgeSummaryLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._flagsSummaryLabel, out var value12))
		{
			_flagsSummaryLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._cellSelectionLabel, out var value13))
		{
			_cellSelectionLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._useCellBrushTool, out var value14))
		{
			_useCellBrushTool = value14.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._brushGridTypeOption, out var value15))
		{
			_brushGridTypeOption = value15.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._brushGroundCheckBox, out var value16))
		{
			_brushGroundCheckBox = value16.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._brushAirCheckBox, out var value17))
		{
			_brushAirCheckBox = value17.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._brushWaterCheckBox, out var value18))
		{
			_brushWaterCheckBox = value18.As<CheckBox>();
		}
		if (info.TryGetProperty(PropertyName._brushElementFlagsSpinBox, out var value19))
		{
			_brushElementFlagsSpinBox = value19.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._isGridBoxSelecting, out var value20))
		{
			_isGridBoxSelecting = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.MapCanvasPan, out var value21))
		{
			MapCanvasPan = value21.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.MapCanvasLastPointer, out var value22))
		{
			MapCanvasLastPointer = value22.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName.MapCanvasZoom, out var value23))
		{
			MapCanvasZoom = value23.As<float>();
		}
		if (info.TryGetProperty(PropertyName.MapCanvasOffset, out var value24))
		{
			MapCanvasOffset = value24.As<Vector2>();
		}
		if (info.TryGetProperty(PropertyName._gridSelectionStart, out var value25))
		{
			_gridSelectionStart = value25.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName._gridSelectionEnd, out var value26))
		{
			_gridSelectionEnd = value26.As<Vector2I>();
		}
		if (info.TryGetProperty(PropertyName.MapGridSelectionRect, out var value27))
		{
			MapGridSelectionRect = value27.As<Rect2I>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value28))
		{
			_updatingControls = value28.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._mapSaveTimer, out var value29))
		{
			_mapSaveTimer = value29.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._editingMapRuleResource, out var value30))
		{
			_editingMapRuleResource = value30.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleBaselineRoot, out var value31))
		{
			_mapRuleBaselineRoot = value31.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleResultRoot, out var value32))
		{
			_mapRuleResultRoot = value32.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._mapRulePreviewStatus, out var value33))
		{
			_mapRulePreviewStatus = value33.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._mapRulePreviewResult, out var value34))
		{
			_mapRulePreviewResult = value34.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._mapRulePreviewValidation, out var value35))
		{
			_mapRulePreviewValidation = value35.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._mapRulePacketType, out var value36))
		{
			_mapRulePacketType = value36.As<OptionButton>();
		}
		if (info.TryGetProperty(PropertyName._mapRulePacketTypeCatalogButton, out var value37))
		{
			_mapRulePacketTypeCatalogButton = value37.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._mapVisualPicker, out var value38))
		{
			_mapVisualPicker = value38.As<XWGameplayResourcePickerWindow>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleCharacterName, out var value39))
		{
			_mapRuleCharacterName = value39.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleBaseCooldown, out var value40))
		{
			_mapRuleBaseCooldown = value40.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleBaseSpeed, out var value41))
		{
			_mapRuleBaseSpeed = value41.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleBaseHitpoints, out var value42))
		{
			_mapRuleBaseHitpoints = value42.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleBaseScale, out var value43))
		{
			_mapRuleBaseScale = value43.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleBaseDps, out var value44))
		{
			_mapRuleBaseDps = value44.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleResultCooldown, out var value45))
		{
			_mapRuleResultCooldown = value45.As<double>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleResultSpeed, out var value46))
		{
			_mapRuleResultSpeed = value46.As<double>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleResultHitpoints, out var value47))
		{
			_mapRuleResultHitpoints = value47.As<double>();
		}
		if (info.TryGetProperty(PropertyName._mapRuleResultScale, out var value48))
		{
			_mapRuleResultScale = value48.As<double>();
		}
	}
}
