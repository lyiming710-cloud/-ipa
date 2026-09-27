using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;
using PVZHE.ModEditor.Inspector;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWMapCellVisualResourceEditor.cs")]
public class XWMapCellVisualResourceEditor : XWGenericVisualResourceEditor
{
	private readonly record struct GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE Type, string Label, Color Color);

	private enum CurvePreset
	{
		Flat,
		Rise,
		Fall,
		Ridge,
		Valley
	}

	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public static readonly StringName BindWorkbench = "BindWorkbench";

		public static readonly StringName DisposeCellBinding = "DisposeCellBinding";

		public static readonly StringName BindResourceHeaderControls = "BindResourceHeaderControls";

		public static readonly StringName ConnectResponsiveSignals = "ConnectResponsiveSignals";

		public static readonly StringName DisconnectResponsiveSignals = "DisconnectResponsiveSignals";

		public static readonly StringName OnMapCellPageChanged = "OnMapCellPageChanged";

		public static readonly StringName UpdateResponsiveSurfaceProcessing = "UpdateResponsiveSurfaceProcessing";

		public static readonly StringName SetResponsiveSurfaceProcessing = "SetResponsiveSurfaceProcessing";

		public static readonly StringName SetMapCellPage = "SetMapCellPage";

		public static readonly StringName BindPositionSpinner = "BindPositionSpinner";

		public static readonly StringName ReadNormalizedPosition = "ReadNormalizedPosition";

		public static readonly StringName BuildBoardButtons = "BuildBoardButtons";

		public static readonly StringName OnBoardCellPressed = "OnBoardCellPressed";

		public static readonly StringName BuildGridTypeButtons = "BuildGridTypeButtons";

		public static readonly StringName OnGridTypeToggled = "OnGridTypeToggled";

		public static readonly StringName BuildElementButtons = "BuildElementButtons";

		public static readonly StringName LoadElementIcon = "LoadElementIcon";

		public static readonly StringName OnElementToggled = "OnElementToggled";

		public static readonly StringName ApplyCurvePreset = "ApplyCurvePreset";

		public static readonly StringName ClearCurve = "ClearCurve";

		public static readonly StringName OnCurveResourceChanged = "OnCurveResourceChanged";

		public static readonly StringName OnCurveResourceSelected = "OnCurveResourceSelected";

		public static readonly StringName PopulateControls = "PopulateControls";

		public static readonly StringName PopulatePositionControls = "PopulatePositionControls";

		public static readonly StringName PopulateGridTypeButtons = "PopulateGridTypeButtons";

		public static readonly StringName PopulateElementButtons = "PopulateElementButtons";

		public static readonly StringName PopulateCurveControls = "PopulateCurveControls";

		public static readonly StringName RefreshVisualState = "RefreshVisualState";

		public static readonly StringName UpdateBoardPreview = "UpdateBoardPreview";

		public static readonly StringName DrawCurveGraph = "DrawCurveGraph";

		public static readonly StringName OnCellPropertyEdited = "OnCellPropertyEdited";

		public static readonly StringName RefreshMapCellEditorFromHistory = "RefreshMapCellEditorFromHistory";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName BuildGridTypeSummary = "BuildGridTypeSummary";

		public static readonly StringName NormalizePosition = "NormalizePosition";

		public static readonly StringName GetCellColor = "GetCellColor";

		public static readonly StringName GetCurvePresetName = "GetCurvePresetName";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName MapCellPages = "MapCellPages";

		public static readonly StringName IsBoardSurfaceActive = "IsBoardSurfaceActive";

		public static readonly StringName IsCurveSurfaceActive = "IsCurveSurfaceActive";

		public static readonly StringName GridVisualCardCount = "GridVisualCardCount";

		public static readonly StringName ElementVisualCardCount = "ElementVisualCardCount";

		public static readonly StringName _editingCell = "_editingCell";

		public static readonly StringName _layoutRoot = "_layoutRoot";

		public static readonly StringName _pages = "_pages";

		public static readonly StringName _resourceNameEdit = "_resourceNameEdit";

		public static readonly StringName _localToSceneCheck = "_localToSceneCheck";

		public static readonly StringName _startX = "_startX";

		public static readonly StringName _startY = "_startY";

		public static readonly StringName _endX = "_endX";

		public static readonly StringName _endY = "_endY";

		public static readonly StringName _boardGrid = "_boardGrid";

		public static readonly StringName _boardHint = "_boardHint";

		public static readonly StringName _selectionSummary = "_selectionSummary";

		public static readonly StringName _coverageBadge = "_coverageBadge";

		public static readonly StringName _curveStatus = "_curveStatus";

		public static readonly StringName _curveGraph = "_curveGraph";

		public static readonly StringName _curvePicker = "_curvePicker";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _responsiveSignalsConnected = "_responsiveSignalsConnected";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string LayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWMapCellVisualEditorLayout.tscn";

	private const string VisualChoiceCardScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWGameVisualChoiceCard.tscn";

	private const int PreviewColumns = 9;

	private const int PreviewRows = 6;

	private static PackedScene _layoutScene;

	private static PackedScene _visualChoiceCardScene;

	private readonly System.Collections.Generic.Dictionary<TowerDefenseEnum.PLANTGRIDTYPE, Button> _gridTypeButtons = new System.Collections.Generic.Dictionary<TowerDefenseEnum.PLANTGRIDTYPE, Button>();

	private readonly System.Collections.Generic.Dictionary<int, Button> _elementButtons = new System.Collections.Generic.Dictionary<int, Button>();

	private readonly List<Button> _boardButtons = new List<Button>();

	private TowerDefenseCellConfig _editingCell;

	private XWVisualPropertyBinding _binding;

	private XWVisualPropertyBinding _metadataBinding;

	private VBoxContainer _layoutRoot;

	private TabContainer _pages;

	private LineEdit _resourceNameEdit;

	private CheckButton _localToSceneCheck;

	private SpinBox _startX;

	private SpinBox _startY;

	private SpinBox _endX;

	private SpinBox _endY;

	private GridContainer _boardGrid;

	private Label _boardHint;

	private Label _selectionSummary;

	private Label _coverageBadge;

	private Label _curveStatus;

	private Control _curveGraph;

	private XWResourcePicker _curvePicker;

	private Vector2I? _pendingGridAnchor;

	private bool _updatingControls;

	private bool _responsiveSignalsConnected;

	private static readonly GridTypeVisual[] GridTypeVisuals = new GridTypeVisual[14]
	{
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.NOONE, "⛔ 禁止", new Color(0.52f, 0.26f, 0.22f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.ALL, "✦ 全部", new Color(0.78f, 0.63f, 0.24f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.GROUND, "▰ 陆地", new Color(0.31f, 0.62f, 0.22f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.WATER, "≈ 水面", new Color(0.18f, 0.48f, 0.83f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.AIR, "△ 空中", new Color(0.55f, 0.37f, 0.8f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.SOIL, "▣ 泥土", new Color(0.45f, 0.3f, 0.14f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.LILYPAD, "◉ 睡莲", new Color(0.18f, 0.58f, 0.47f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.POT, "∪ 花盆", new Color(0.61f, 0.36f, 0.16f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.SURROUND, "◎ 环绕", new Color(0.62f, 0.55f, 0.23f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.GRAVESTONE, "▥ 墓碑", new Color(0.36f, 0.39f, 0.43f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.CRATER, "● 弹坑", new Color(0.32f, 0.2f, 0.12f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.BRICK, "▦ 砖块", new Color(0.64f, 0.3f, 0.22f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.ICECAP, "◇ 冰面", new Color(0.42f, 0.72f, 0.85f)),
		new GridTypeVisual(TowerDefenseEnum.PLANTGRIDTYPE.PLANT, "♣ 植物", new Color(0.26f, 0.67f, 0.3f))
	};

	private static readonly (int Flag, string Label, Color Color)[] ElementVisuals = new (int, string, Color)[4]
	{
		(1, "❄ 冰", new Color(0.37f, 0.72f, 0.96f)),
		(2, "♨ 火", new Color(0.94f, 0.38f, 0.18f)),
		(4, "☀ 日", new Color(0.94f, 0.72f, 0.2f)),
		(8, "☾ 夜", new Color(0.48f, 0.42f, 0.82f))
	};

	public TabContainer MapCellPages => _pages;

	public bool IsBoardSurfaceActive { get; private set; }

	public bool IsCurveSurfaceActive { get; private set; }

	public int GridVisualCardCount => _gridTypeButtons.Count;

	public int ElementVisualCardCount => _elementButtons.Count;

	public override void _ExitTree()
	{
		DisposeCellBinding();
		base._ExitTree();
	}

	protected override void OnVisualEditorVisibilityChanged(bool visible)
	{
		base.OnVisualEditorVisibilityChanged(visible);
		UpdateResponsiveSurfaceProcessing();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeCellBinding();
		if (CurrentResource is TowerDefenseCellConfig editingCell && CanvasGrid != null)
		{
			_editingCell = editingCell;
			CanvasGrid.Columns = 1;
			if (_layoutScene == null)
			{
				_layoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWMapCellVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _layoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				_layoutRoot = vBoxContainer;
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindWorkbench(vBoxContainer);
				PopulateControls();
				AddSummaryRows();
			}
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!(resource is TowerDefenseCellConfig))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private void BindWorkbench(VBoxContainer root)
	{
		_pages = root.GetNode<TabContainer>("%MapCellPages");
		_resourceNameEdit = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToSceneCheck = root.GetNode<CheckButton>("%LocalToSceneCheck");
		_startX = root.GetNode<SpinBox>("%StartX");
		_startY = root.GetNode<SpinBox>("%StartY");
		_endX = root.GetNode<SpinBox>("%EndX");
		_endY = root.GetNode<SpinBox>("%EndY");
		_boardGrid = root.GetNode<GridContainer>("%BoardGrid");
		_boardHint = root.GetNode<Label>("%BoardHint");
		_selectionSummary = root.GetNode<Label>("%SelectionSummary");
		_coverageBadge = root.GetNode<Label>("%CoverageBadge");
		_curveStatus = root.GetNode<Label>("%CurveStatus");
		_curveGraph = root.GetNode<Control>("%CurveGraph");
		_curvePicker = root.GetNode<XWResourcePicker>("%GroundCurvePicker");
		_binding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnCellPropertyEdited);
		BindResourceHeaderControls();
		BindPositionSpinner(_startX);
		BindPositionSpinner(_startY);
		BindPositionSpinner(_endX);
		BindPositionSpinner(_endY);
		BuildBoardButtons();
		BuildGridTypeButtons(root.GetNode<HFlowContainer>("%GridTypeButtons"));
		BuildElementButtons(root.GetNode<HFlowContainer>("%ElementButtons"));
		_curveGraph.Draw += DrawCurveGraph;
		_curvePicker.Setup("CurveTexture");
		_curvePicker.ResourceChanged += OnCurveResourceChanged;
		_curvePicker.ResourceSelected += OnCurveResourceSelected;
		root.GetNode<Button>("%FlatCurve").Pressed += () =>
		{
			ApplyCurvePreset(CurvePreset.Flat);
		};
		root.GetNode<Button>("%RiseCurve").Pressed += () =>
		{
			ApplyCurvePreset(CurvePreset.Rise);
		};
		root.GetNode<Button>("%FallCurve").Pressed += () =>
		{
			ApplyCurvePreset(CurvePreset.Fall);
		};
		root.GetNode<Button>("%RidgeCurve").Pressed += () =>
		{
			ApplyCurvePreset(CurvePreset.Ridge);
		};
		root.GetNode<Button>("%ValleyCurve").Pressed += () =>
		{
			ApplyCurvePreset(CurvePreset.Valley);
		};
		root.GetNode<Button>("%ClearCurve").Pressed += ClearCurve;
		root.GetNode<Button>("%SaveButton").Pressed += SaveCurrentResource;
		ConnectResponsiveSignals();
	}

	private void DisposeCellBinding()
	{
		DisconnectResponsiveSignals();
		_binding?.Dispose();
		_binding = null;
		_metadataBinding?.Dispose();
		_metadataBinding = null;
		_editingCell = null;
		_gridTypeButtons.Clear();
		_elementButtons.Clear();
		_boardButtons.Clear();
		_pendingGridAnchor = null;
		_layoutRoot = null;
		_pages = null;
		_resourceNameEdit = null;
		_localToSceneCheck = null;
		_boardGrid = null;
		_curveGraph = null;
	}

	private void BindResourceHeaderControls()
	{
		_metadataBinding?.Dispose();
		_metadataBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnCellPropertyEdited);
		_metadataBinding.BindText(_resourceNameEdit, _editingCell, "resource_name", RefreshVisualState, this, "RefreshMapCellEditorFromHistory");
		_metadataBinding.BindToggle(_localToSceneCheck, _editingCell, "resource_local_to_scene", RefreshVisualState, this, "RefreshMapCellEditorFromHistory");
	}

	private void ConnectResponsiveSignals()
	{
		if (!_responsiveSignalsConnected && GodotObject.IsInstanceValid(_pages))
		{
			_pages.TabChanged += OnMapCellPageChanged;
			_responsiveSignalsConnected = true;
			UpdateResponsiveSurfaceProcessing();
		}
	}

	private void DisconnectResponsiveSignals()
	{
		if (_responsiveSignalsConnected)
		{
			if (GodotObject.IsInstanceValid(_pages))
			{
				_pages.TabChanged -= OnMapCellPageChanged;
			}
			_responsiveSignalsConnected = false;
			SetResponsiveSurfaceProcessing(boardActive: false, curveActive: false);
		}
	}

	private void OnMapCellPageChanged(long _)
	{
		UpdateResponsiveSurfaceProcessing();
	}

	private void UpdateResponsiveSurfaceProcessing()
	{
		bool flag = IsVisibleInTree() && GodotObject.IsInstanceValid(_layoutRoot) && _layoutRoot.IsVisibleInTree() && GodotObject.IsInstanceValid(_pages);
		SetResponsiveSurfaceProcessing(flag && _pages.CurrentTab == 0, flag && _pages.CurrentTab == 2);
	}

	private void SetResponsiveSurfaceProcessing(bool boardActive, bool curveActive)
	{
		IsBoardSurfaceActive = boardActive;
		IsCurveSurfaceActive = curveActive;
		if (GodotObject.IsInstanceValid(_boardGrid))
		{
			_boardGrid.ProcessMode = (ProcessModeEnum)(boardActive ? 0 : 4);
		}
		if (GodotObject.IsInstanceValid(_curveGraph))
		{
			_curveGraph.ProcessMode = (ProcessModeEnum)(curveActive ? 0 : 4);
		}
	}

	public void SetMapCellPage(int page)
	{
		if (GodotObject.IsInstanceValid(_pages) && page >= 0 && page < _pages.GetTabCount())
		{
			_pages.CurrentTab = page;
			UpdateResponsiveSurfaceProcessing();
		}
	}

	private void BindPositionSpinner(SpinBox spinner)
	{
		spinner.FocusEntered += () =>
		{
			_binding?.BeginEdit(_editingCell, "pos");
		};
		spinner.ValueChanged += (double _) =>
		{
			if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_editingCell))
			{
				_binding.PreviewValue(_editingCell, "pos", Variant.From<Vector4I>(ReadNormalizedPosition()));
				RefreshVisualState();
			}
		};
		spinner.FocusExited += () =>
		{
			if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_editingCell))
			{
				_binding.CommitEdit(_editingCell, "pos", Variant.From<Vector4I>(ReadNormalizedPosition()), "调整地图格子区域", this, "RefreshMapCellEditorFromHistory");
				PopulatePositionControls();
			}
		};
	}

	private Vector4I ReadNormalizedPosition()
	{
		int val = Mathf.Max(1, Mathf.RoundToInt(_startX.Value));
		int val2 = Mathf.Max(1, Mathf.RoundToInt(_startY.Value));
		int val3 = Mathf.Max(1, Mathf.RoundToInt(_endX.Value));
		int val4 = Mathf.Max(1, Mathf.RoundToInt(_endY.Value));
		return new Vector4I(Math.Min(val, val3), Math.Min(val2, val4), Math.Max(val, val3), Math.Max(val2, val4));
	}

	private void BuildBoardButtons()
	{
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 9; j++)
			{
				Vector2I cell = new Vector2I(j + 1, i + 1);
				Button button = new Button
				{
					Name = $"MapCellGridButton_{j}_{i}",
					Text = $"{j + 1}\n{i + 1}",
					CustomMinimumSize = new Vector2(48f, 48f),
					SizeFlagsHorizontal = SizeFlags.ExpandFill,
					SizeFlagsVertical = SizeFlags.ExpandFill,
					TooltipText = $"地图格子 ({j + 1}, {i + 1})"
				};
				button.Pressed += () =>
				{
					OnBoardCellPressed(cell);
				};
				_boardGrid.AddChild(button, forceReadableName: false, InternalMode.Disabled);
				_boardButtons.Add(button);
			}
		}
	}

	private void OnBoardCellPressed(Vector2I cell)
	{
		if (_binding != null && GodotObject.IsInstanceValid(_editingCell))
		{
			if (!_pendingGridAnchor.HasValue)
			{
				_pendingGridAnchor = cell;
				_boardHint.Text = $"起点 ({cell.X}, {cell.Y}) 已锁定；再点一个格子完成区域";
				UpdateBoardPreview();
			}
			else
			{
				Vector2I value = _pendingGridAnchor.Value;
				_pendingGridAnchor = null;
				Vector4I from = new Vector4I(Math.Min(value.X, cell.X), Math.Min(value.Y, cell.Y), Math.Max(value.X, cell.X), Math.Max(value.Y, cell.Y));
				_binding.SetValue(_editingCell, "pos", Variant.From(in from), "框选地图格子区域", this, "RefreshMapCellEditorFromHistory");
				_boardHint.Text = "区域已应用；继续连点两个格子可重新框选";
				PopulatePositionControls();
				RefreshVisualState();
			}
		}
	}

	private void BuildGridTypeButtons(HFlowContainer container)
	{
		if (_visualChoiceCardScene == null)
		{
			_visualChoiceCardScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWGameVisualChoiceCard.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		Texture2D icon = ResourceLoader.Load<Texture2D>("res://addons/ModEditor/Icons/ResourceMap.svg", null, ResourceLoader.CacheMode.Reuse);
		GridTypeVisual[] gridTypeVisuals = GridTypeVisuals;
		for (int i = 0; i < gridTypeVisuals.Length; i++)
		{
			GridTypeVisual visual = gridTypeVisuals[i];
			XWGameVisualChoiceCard xWGameVisualChoiceCard = _visualChoiceCardScene?.Instantiate<XWGameVisualChoiceCard>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
			{
				xWGameVisualChoiceCard.Name = $"GridType_{visual.Type}";
				xWGameVisualChoiceCard.CustomMinimumSize = new Vector2(150f, 68f);
				xWGameVisualChoiceCard.Configure(visual.Type.ToString(), visual.Label, "放置层图块", icon, selected: false);
				xWGameVisualChoiceCard.TooltipText = $"切换 {visual.Type} 放置层";
				xWGameVisualChoiceCard.AddThemeColorOverride("font_pressed_color", Colors.White);
				xWGameVisualChoiceCard.Modulate = visual.Color.Lightened(0.18f);
				xWGameVisualChoiceCard.Toggled += (bool pressed) =>
				{
					OnGridTypeToggled(visual.Type, pressed);
				};
				container.AddChild(xWGameVisualChoiceCard, forceReadableName: false, InternalMode.Disabled);
				_gridTypeButtons[visual.Type] = xWGameVisualChoiceCard;
			}
		}
	}

	private void OnGridTypeToggled(TowerDefenseEnum.PLANTGRIDTYPE type, bool pressed)
	{
		if (_updatingControls || _binding == null || !GodotObject.IsInstanceValid(_editingCell))
		{
			return;
		}
		Array<TowerDefenseEnum.PLANTGRIDTYPE> from = ((_editingCell.gridType == null) ? new Array<TowerDefenseEnum.PLANTGRIDTYPE>() : new Array<TowerDefenseEnum.PLANTGRIDTYPE>(_editingCell.gridType));
		if ((uint)(type - -1) <= 1u)
		{
			from.Clear();
			if (pressed)
			{
				from.Add(type);
			}
		}
		else
		{
			from.Remove(TowerDefenseEnum.PLANTGRIDTYPE.ALL);
			from.Remove(TowerDefenseEnum.PLANTGRIDTYPE.NOONE);
			if (pressed && !from.Contains(type))
			{
				from.Add(type);
			}
			else if (!pressed)
			{
				from.Remove(type);
			}
		}
		if (from.Count == 0)
		{
			from.Add(TowerDefenseEnum.PLANTGRIDTYPE.NOONE);
		}
		_binding.SetValue(_editingCell, "gridType", Variant.From(in from), "切换地图格子放置层", this, "RefreshMapCellEditorFromHistory");
		PopulateGridTypeButtons();
		RefreshVisualState();
	}

	private void BuildElementButtons(HFlowContainer container)
	{
		if (_visualChoiceCardScene == null)
		{
			_visualChoiceCardScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWGameVisualChoiceCard.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		(int, string, Color)[] elementVisuals = ElementVisuals;
		for (int i = 0; i < elementVisuals.Length; i++)
		{
			(int Flag, string Label, Color Color) visual = elementVisuals[i];
			XWGameVisualChoiceCard xWGameVisualChoiceCard = _visualChoiceCardScene?.Instantiate<XWGameVisualChoiceCard>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
			{
				xWGameVisualChoiceCard.Name = $"ElementFlag_{visual.Flag}";
				xWGameVisualChoiceCard.CustomMinimumSize = new Vector2(150f, 68f);
				xWGameVisualChoiceCard.Configure(visual.Flag.ToString(), visual.Label, "环境元素", LoadElementIcon(visual.Flag), selected: false);
				xWGameVisualChoiceCard.TooltipText = "该格子携带的游戏元素环境标记";
				xWGameVisualChoiceCard.Modulate = visual.Color.Lightened(0.15f);
				xWGameVisualChoiceCard.Toggled += (bool pressed) =>
				{
					OnElementToggled(visual.Flag, pressed);
				};
				container.AddChild(xWGameVisualChoiceCard, forceReadableName: false, InternalMode.Disabled);
				_elementButtons[visual.Flag] = xWGameVisualChoiceCard;
			}
		}
	}

	private static Texture2D LoadElementIcon(int flag)
	{
		string path = flag switch
		{
			1 => "res://addons/ModEditor/Icons/ClassIcon/ColorPick.svg", 
			2 => "res://addons/ModEditor/Icons/StatusError.svg", 
			4 => "res://addons/ModEditor/Icons/ClassIcon/Environment.svg", 
			8 => "res://addons/ModEditor/Icons/ClassIcon/Environment.svg", 
			_ => "res://addons/ModEditor/Icons/Signals.svg", 
		};
		if (!ResourceLoader.Exists(path))
		{
			return null;
		}
		return ResourceLoader.Load<Texture2D>(path, null, ResourceLoader.CacheMode.Reuse);
	}

	private void OnElementToggled(int flag, bool pressed)
	{
		if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_editingCell))
		{
			int num = (pressed ? (_editingCell.ElementFlags | flag) : (_editingCell.ElementFlags & ~flag));
			_binding.SetValue(_editingCell, "ElementFlags", num, "切换地图格子元素标记", this, "RefreshMapCellEditorFromHistory");
			PopulateElementButtons();
		}
	}

	private void ApplyCurvePreset(CurvePreset preset)
	{
		if (_binding != null && GodotObject.IsInstanceValid(_editingCell))
		{
			Curve curve = new Curve
			{
				MinValue = 0f,
				MaxValue = 1f
			};
			switch (preset)
			{
			case CurvePreset.Flat:
				curve.AddPoint(new Vector2(0f, 0f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				curve.AddPoint(new Vector2(1f, 0f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				break;
			case CurvePreset.Rise:
				curve.AddPoint(new Vector2(0f, 0f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				curve.AddPoint(new Vector2(1f, 1f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				break;
			case CurvePreset.Fall:
				curve.AddPoint(new Vector2(0f, 1f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				curve.AddPoint(new Vector2(1f, 0f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				break;
			case CurvePreset.Ridge:
				curve.AddPoint(new Vector2(0f, 0f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				curve.AddPoint(new Vector2(0.5f, 1f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				curve.AddPoint(new Vector2(1f, 0f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				break;
			case CurvePreset.Valley:
				curve.AddPoint(new Vector2(0f, 1f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				curve.AddPoint(new Vector2(0.5f, 0f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				curve.AddPoint(new Vector2(1f, 1f), 0f, 0f, Curve.TangentMode.Free, Curve.TangentMode.Free);
				break;
			}
			CurveTexture from = new CurveTexture
			{
				Curve = curve,
				ResourceLocalToScene = true
			};
			_binding.SetValue(_editingCell, "groundHeightCurve", Variant.From(in from), "应用" + GetCurvePresetName(preset) + "高度曲线", this, "RefreshMapCellEditorFromHistory");
			PopulateCurveControls();
		}
	}

	private void ClearCurve()
	{
		_binding?.SetValue(_editingCell, "groundHeightCurve", default, "清除地图格子高度曲线", this, "RefreshMapCellEditorFromHistory");
		PopulateCurveControls();
	}

	private void OnCurveResourceChanged(Resource resource)
	{
		if (!_updatingControls && _binding != null && GodotObject.IsInstanceValid(_editingCell) && (!GodotObject.IsInstanceValid(resource) || resource is CurveTexture))
		{
			_binding.SetValue(_editingCell, "groundHeightCurve", Variant.From(in resource), "选择地图格子高度曲线", this, "RefreshMapCellEditorFromHistory");
			PopulateCurveControls();
		}
	}

	private static void OnCurveResourceSelected(Resource resource)
	{
		if (GodotObject.IsInstanceValid(resource))
		{
			XWEditorInterface.Instance?.EditResource(resource);
		}
	}

	private void PopulateControls()
	{
		_updatingControls = true;
		try
		{
			PopulatePositionControls();
			PopulateGridTypeButtons();
			PopulateElementButtons();
			PopulateCurveControls();
		}
		finally
		{
			_updatingControls = false;
		}
		RefreshVisualState();
	}

	private void PopulatePositionControls()
	{
		if (GodotObject.IsInstanceValid(_editingCell) && GodotObject.IsInstanceValid(_startX))
		{
			bool updatingControls = _updatingControls;
			_updatingControls = true;
			Vector4I vector4I = NormalizePosition(_editingCell.pos);
			_startX.SetValueNoSignal(vector4I.X);
			_startY.SetValueNoSignal(vector4I.Y);
			_endX.SetValueNoSignal(vector4I.Z);
			_endY.SetValueNoSignal(vector4I.W);
			_updatingControls = updatingControls;
		}
	}

	private void PopulateGridTypeButtons()
	{
		bool updatingControls = _updatingControls;
		_updatingControls = true;
		foreach (KeyValuePair<TowerDefenseEnum.PLANTGRIDTYPE, Button> gridTypeButton in _gridTypeButtons)
		{
			Button value = gridTypeButton.Value;
			TowerDefenseCellConfig editingCell = _editingCell;
			value.SetPressedNoSignal(editingCell != null && editingCell.gridType?.Contains(gridTypeButton.Key) == true);
		}
		_updatingControls = updatingControls;
	}

	private void PopulateElementButtons()
	{
		bool updatingControls = _updatingControls;
		_updatingControls = true;
		foreach (KeyValuePair<int, Button> elementButton in _elementButtons)
		{
			elementButton.Value.SetPressedNoSignal((_editingCell.ElementFlags & elementButton.Key) != 0);
		}
		_updatingControls = updatingControls;
	}

	private void PopulateCurveControls()
	{
		bool updatingControls = _updatingControls;
		_updatingControls = true;
		_curvePicker?.SetEditedResource(_editingCell?.groundHeightCurve);
		if (GodotObject.IsInstanceValid(_curveStatus))
		{
			if (GodotObject.IsInstanceValid(_editingCell?.groundHeightCurve?.Curve))
			{
				Curve curve = _editingCell.groundHeightCurve.Curve;
				_curveStatus.Text = $"曲线已绑定 · {curve.PointCount} 个控制点 · 高度 {curve.MinValue:0.##}～{curve.MaxValue:0.##}";
			}
			else
			{
				_curveStatus.Text = "未配置高度曲线：整块地面高度为 0";
			}
		}
		_curveGraph?.QueueRedraw();
		_updatingControls = updatingControls;
	}

	private void RefreshVisualState()
	{
		if (GodotObject.IsInstanceValid(_editingCell))
		{
			UpdateBoardPreview();
			Vector4I vector4I = NormalizePosition(_editingCell.pos);
			int num = vector4I.Z - vector4I.X + 1;
			int num2 = vector4I.W - vector4I.Y + 1;
			int value = num * num2;
			if (GodotObject.IsInstanceValid(_selectionSummary))
			{
				_selectionSummary.Text = $"生效区域：({vector4I.X}, {vector4I.Y}) → ({vector4I.Z}, {vector4I.W})，共 {value} 格";
			}
			if (GodotObject.IsInstanceValid(_coverageBadge))
			{
				_coverageBadge.Text = $"区域 {num} × {num2}";
			}
			_curveGraph?.QueueRedraw();
		}
	}

	private void UpdateBoardPreview()
	{
		if (GodotObject.IsInstanceValid(_editingCell))
		{
			Vector4I vector4I = NormalizePosition(_editingCell.pos);
			Color cellColor = GetCellColor(_editingCell);
			for (int i = 0; i < _boardButtons.Count; i++)
			{
				int num = i % 9 + 1;
				int num2 = i / 9 + 1;
				bool flag = num >= vector4I.X && num <= vector4I.Z && num2 >= vector4I.Y && num2 <= vector4I.W;
				bool flag2 = _pendingGridAnchor == new Vector2I(num, num2);
				Button button = _boardButtons[i];
				Button button2 = button;
				Color modulate;
				if (flag2)
				{
					modulate = new Color(1f, 0.74f, 0.24f);
				}
				else
				{
					modulate = (flag ? cellColor : new Color(0.36f, 0.32f, 0.2f, 0.72f));
				}
				button2.Modulate = modulate;
				button.AddThemeColorOverride("font_color", (flag | flag2) ? Colors.White : new Color(0.72f, 0.69f, 0.56f));
			}
		}
	}

	private void DrawCurveGraph()
	{
		if (GodotObject.IsInstanceValid(_curveGraph))
		{
			Vector2 size = _curveGraph.Size;
			Rect2 rect = new Rect2(new Vector2(14f, 12f), new Vector2(Math.Max(10f, size.X - 28f), Math.Max(10f, size.Y - 24f)));
			for (int i = 0; i <= 4; i++)
			{
				float num = (float)i / 4f;
				_curveGraph.DrawLine(new Vector2(rect.Position.X, rect.Position.Y + rect.Size.Y * num), new Vector2(rect.End.X, rect.Position.Y + rect.Size.Y * num), new Color(0.24f, 0.3f, 0.2f, 0.65f), 1f);
			}
			Curve curve = _editingCell?.groundHeightCurve?.Curve;
			Vector2[] array = new Vector2[65];
			float num2 = (GodotObject.IsInstanceValid(curve) ? curve.MinValue : 0f);
			float num3 = (GodotObject.IsInstanceValid(curve) ? curve.MaxValue : 1f);
			float num4 = Math.Max(0.0001f, num3 - num2);
			for (int j = 0; j < array.Length; j++)
			{
				float num5 = (float)j / (float)(array.Length - 1);
				float num6 = Mathf.Clamp(((GodotObject.IsInstanceValid(curve) ? curve.Sample(num5) : 0f) - num2) / num4, 0f, 1f);
				array[j] = new Vector2(rect.Position.X + rect.Size.X * num5, rect.End.Y - rect.Size.Y * num6);
			}
			_curveGraph.DrawPolyline(array, new Color(0.54f, 0.9f, 0.3f), 3f, antialiased: true);
		}
	}

	private void OnCellPropertyEdited(bool committed)
	{
		if (GodotObject.IsInstanceValid(_editingCell))
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
			}
			else
			{
				MarkCurrentResourceDirty();
				_editingCell.EmitChanged();
			}
			RefreshVisualState();
		}
	}

	public void RefreshMapCellEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()))
		{
			_pendingGridAnchor = null;
			BindResourceHeaderControls();
			PopulateControls();
		}
	}

	private void AddSummaryRows()
	{
		Vector4I vector4I = NormalizePosition(_editingCell.pos);
		PreviewList?.AddItem($"地图格子区域 · ({vector4I.X}, {vector4I.Y}) → ({vector4I.Z}, {vector4I.W})");
		PreviewList?.AddItem("放置层 · " + BuildGridTypeSummary());
		GraphList?.AddItem("格子画笔 → 地图区域 → 放置层 / 元素环境 / 地面高度");
		string text;
		if (GodotObject.IsInstanceValid(_editingCell.groundHeightCurve))
		{
			text = (string.IsNullOrWhiteSpace(_editingCell.groundHeightCurve.ResourcePath) ? "内置 CurveTexture" : _editingCell.groundHeightCurve.ResourcePath);
		}
		else
		{
			text = "未配置";
		}
		ReferenceList?.AddItem("高度曲线 → " + text);
	}

	private string BuildGridTypeSummary()
	{
		if (_editingCell.gridType == null || _editingCell.gridType.Count == 0)
		{
			return "未配置";
		}
		List<string> list = new List<string>();
		foreach (TowerDefenseEnum.PLANTGRIDTYPE item in _editingCell.gridType)
		{
			list.Add(item.ToString());
		}
		return string.Join(" + ", list);
	}

	private static Vector4I NormalizePosition(Vector4I pos)
	{
		int num = Math.Max(1, Math.Min(pos.X, pos.Z));
		int num2 = Math.Max(1, Math.Min(pos.Y, pos.W));
		int z = Math.Max(num, Math.Max(pos.X, pos.Z));
		int w = Math.Max(num2, Math.Max(pos.Y, pos.W));
		return new Vector4I(num, num2, z, w);
	}

	private static Color GetCellColor(TowerDefenseCellConfig cell)
	{
		if (cell != null && cell.gridType?.Contains(TowerDefenseEnum.PLANTGRIDTYPE.WATER) == true)
		{
			return new Color(0.18f, 0.53f, 0.92f);
		}
		if (cell != null && cell.gridType?.Contains(TowerDefenseEnum.PLANTGRIDTYPE.GROUND) == true && cell.gridType.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR))
		{
			return new Color(0.26f, 0.69f, 0.34f);
		}
		if (cell != null && cell.gridType?.Contains(TowerDefenseEnum.PLANTGRIDTYPE.GROUND) == true)
		{
			return new Color(0.36f, 0.72f, 0.25f);
		}
		if (cell != null && cell.gridType?.Contains(TowerDefenseEnum.PLANTGRIDTYPE.AIR) == true)
		{
			return new Color(0.61f, 0.44f, 0.9f);
		}
		return new Color(0.65f, 0.55f, 0.24f);
	}

	private static string GetCurvePresetName(CurvePreset preset)
	{
		return preset switch
		{
			CurvePreset.Flat => "平地", 
			CurvePreset.Rise => "上坡", 
			CurvePreset.Fall => "下坡", 
			CurvePreset.Ridge => "屋脊", 
			CurvePreset.Valley => "凹地", 
			_ => "地面", 
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(39)
		{
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeCellBinding, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindResourceHeaderControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConnectResponsiveSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisconnectResponsiveSignals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnMapCellPageChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "_", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateResponsiveSurfaceProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetResponsiveSurfaceProcessing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "boardActive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "curveActive", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetMapCellPage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "page", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindPositionSpinner, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "spinner", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("SpinBox"), exported: false)
			}, null),
			new MethodInfo(MethodName.ReadNormalizedPosition, new PropertyInfo(Variant.Type.Vector4I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildBoardButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnBoardCellPressed, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector2I, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildGridTypeButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnGridTypeToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "type", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildElementButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "container", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HFlowContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.LoadElementIcon, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "flag", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.OnElementToggled, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "flag", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "pressed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyCurvePreset, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "preset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ClearCurve, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCurveResourceChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnCurveResourceSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.PopulateControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulatePositionControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateGridTypeButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateElementButtons, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.PopulateCurveControls, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshVisualState, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateBoardPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DrawCurveGraph, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnCellPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshMapCellEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BuildGridTypeSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.NormalizePosition, new PropertyInfo(Variant.Type.Vector4I, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Vector4I, "pos", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.GetCellColor, new PropertyInfo(Variant.Type.Color, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "cell", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCurvePresetName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "preset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
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
		if (method == MethodName.OnVisualEditorVisibilityChanged && args.Count == 1)
		{
			OnVisualEditorVisibilityChanged(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindWorkbench && args.Count == 1)
		{
			BindWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeCellBinding && args.Count == 0)
		{
			DisposeCellBinding();
			ret = default;
			return true;
		}
		if (method == MethodName.BindResourceHeaderControls && args.Count == 0)
		{
			BindResourceHeaderControls();
			ret = default;
			return true;
		}
		if (method == MethodName.ConnectResponsiveSignals && args.Count == 0)
		{
			ConnectResponsiveSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.DisconnectResponsiveSignals && args.Count == 0)
		{
			DisconnectResponsiveSignals();
			ret = default;
			return true;
		}
		if (method == MethodName.OnMapCellPageChanged && args.Count == 1)
		{
			OnMapCellPageChanged(VariantUtils.ConvertTo<long>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateResponsiveSurfaceProcessing && args.Count == 0)
		{
			UpdateResponsiveSurfaceProcessing();
			ret = default;
			return true;
		}
		if (method == MethodName.SetResponsiveSurfaceProcessing && args.Count == 2)
		{
			SetResponsiveSurfaceProcessing(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetMapCellPage && args.Count == 1)
		{
			SetMapCellPage(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindPositionSpinner && args.Count == 1)
		{
			BindPositionSpinner(VariantUtils.ConvertTo<SpinBox>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReadNormalizedPosition && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<Vector4I>(ReadNormalizedPosition());
			return true;
		}
		if (method == MethodName.BuildBoardButtons && args.Count == 0)
		{
			BuildBoardButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.OnBoardCellPressed && args.Count == 1)
		{
			OnBoardCellPressed(VariantUtils.ConvertTo<Vector2I>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildGridTypeButtons && args.Count == 1)
		{
			BuildGridTypeButtons(VariantUtils.ConvertTo<HFlowContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnGridTypeToggled && args.Count == 2)
		{
			OnGridTypeToggled(VariantUtils.ConvertTo<TowerDefenseEnum.PLANTGRIDTYPE>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildElementButtons && args.Count == 1)
		{
			BuildElementButtons(VariantUtils.ConvertTo<HFlowContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.LoadElementIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadElementIcon(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.OnElementToggled && args.Count == 2)
		{
			OnElementToggled(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyCurvePreset && args.Count == 1)
		{
			ApplyCurvePreset(VariantUtils.ConvertTo<CurvePreset>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ClearCurve && args.Count == 0)
		{
			ClearCurve();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCurveResourceChanged && args.Count == 1)
		{
			OnCurveResourceChanged(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCurveResourceSelected && args.Count == 1)
		{
			OnCurveResourceSelected(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateControls && args.Count == 0)
		{
			PopulateControls();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulatePositionControls && args.Count == 0)
		{
			PopulatePositionControls();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateGridTypeButtons && args.Count == 0)
		{
			PopulateGridTypeButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateElementButtons && args.Count == 0)
		{
			PopulateElementButtons();
			ret = default;
			return true;
		}
		if (method == MethodName.PopulateCurveControls && args.Count == 0)
		{
			PopulateCurveControls();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshVisualState && args.Count == 0)
		{
			RefreshVisualState();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateBoardPreview && args.Count == 0)
		{
			UpdateBoardPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.DrawCurveGraph && args.Count == 0)
		{
			DrawCurveGraph();
			ret = default;
			return true;
		}
		if (method == MethodName.OnCellPropertyEdited && args.Count == 1)
		{
			OnCellPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshMapCellEditorFromHistory && args.Count == 0)
		{
			RefreshMapCellEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 0)
		{
			AddSummaryRows();
			ret = default;
			return true;
		}
		if (method == MethodName.BuildGridTypeSummary && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildGridTypeSummary());
			return true;
		}
		if (method == MethodName.NormalizePosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector4I>(NormalizePosition(VariantUtils.ConvertTo<Vector4I>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCellColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetCellColor(VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurvePresetName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurvePresetName(VariantUtils.ConvertTo<CurvePreset>(in args[0])));
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.LoadElementIcon && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(LoadElementIcon(VariantUtils.ConvertTo<int>(in args[0])));
			return true;
		}
		if (method == MethodName.OnCurveResourceSelected && args.Count == 1)
		{
			OnCurveResourceSelected(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.NormalizePosition && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Vector4I>(NormalizePosition(VariantUtils.ConvertTo<Vector4I>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCellColor && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Color>(GetCellColor(VariantUtils.ConvertTo<TowerDefenseCellConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCurvePresetName && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCurvePresetName(VariantUtils.ConvertTo<CurvePreset>(in args[0])));
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
		if (method == MethodName.OnVisualEditorVisibilityChanged)
		{
			return true;
		}
		if (method == MethodName.BindWorkbench)
		{
			return true;
		}
		if (method == MethodName.DisposeCellBinding)
		{
			return true;
		}
		if (method == MethodName.BindResourceHeaderControls)
		{
			return true;
		}
		if (method == MethodName.ConnectResponsiveSignals)
		{
			return true;
		}
		if (method == MethodName.DisconnectResponsiveSignals)
		{
			return true;
		}
		if (method == MethodName.OnMapCellPageChanged)
		{
			return true;
		}
		if (method == MethodName.UpdateResponsiveSurfaceProcessing)
		{
			return true;
		}
		if (method == MethodName.SetResponsiveSurfaceProcessing)
		{
			return true;
		}
		if (method == MethodName.SetMapCellPage)
		{
			return true;
		}
		if (method == MethodName.BindPositionSpinner)
		{
			return true;
		}
		if (method == MethodName.ReadNormalizedPosition)
		{
			return true;
		}
		if (method == MethodName.BuildBoardButtons)
		{
			return true;
		}
		if (method == MethodName.OnBoardCellPressed)
		{
			return true;
		}
		if (method == MethodName.BuildGridTypeButtons)
		{
			return true;
		}
		if (method == MethodName.OnGridTypeToggled)
		{
			return true;
		}
		if (method == MethodName.BuildElementButtons)
		{
			return true;
		}
		if (method == MethodName.LoadElementIcon)
		{
			return true;
		}
		if (method == MethodName.OnElementToggled)
		{
			return true;
		}
		if (method == MethodName.ApplyCurvePreset)
		{
			return true;
		}
		if (method == MethodName.ClearCurve)
		{
			return true;
		}
		if (method == MethodName.OnCurveResourceChanged)
		{
			return true;
		}
		if (method == MethodName.OnCurveResourceSelected)
		{
			return true;
		}
		if (method == MethodName.PopulateControls)
		{
			return true;
		}
		if (method == MethodName.PopulatePositionControls)
		{
			return true;
		}
		if (method == MethodName.PopulateGridTypeButtons)
		{
			return true;
		}
		if (method == MethodName.PopulateElementButtons)
		{
			return true;
		}
		if (method == MethodName.PopulateCurveControls)
		{
			return true;
		}
		if (method == MethodName.RefreshVisualState)
		{
			return true;
		}
		if (method == MethodName.UpdateBoardPreview)
		{
			return true;
		}
		if (method == MethodName.DrawCurveGraph)
		{
			return true;
		}
		if (method == MethodName.OnCellPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.RefreshMapCellEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
		{
			return true;
		}
		if (method == MethodName.BuildGridTypeSummary)
		{
			return true;
		}
		if (method == MethodName.NormalizePosition)
		{
			return true;
		}
		if (method == MethodName.GetCellColor)
		{
			return true;
		}
		if (method == MethodName.GetCurvePresetName)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName.IsBoardSurfaceActive)
		{
			IsBoardSurfaceActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName.IsCurveSurfaceActive)
		{
			IsCurveSurfaceActive = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._editingCell)
		{
			_editingCell = VariantUtils.ConvertTo<TowerDefenseCellConfig>(in value);
			return true;
		}
		if (name == PropertyName._layoutRoot)
		{
			_layoutRoot = VariantUtils.ConvertTo<VBoxContainer>(in value);
			return true;
		}
		if (name == PropertyName._pages)
		{
			_pages = VariantUtils.ConvertTo<TabContainer>(in value);
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
		if (name == PropertyName._startX)
		{
			_startX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._startY)
		{
			_startY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._endX)
		{
			_endX = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._endY)
		{
			_endY = VariantUtils.ConvertTo<SpinBox>(in value);
			return true;
		}
		if (name == PropertyName._boardGrid)
		{
			_boardGrid = VariantUtils.ConvertTo<GridContainer>(in value);
			return true;
		}
		if (name == PropertyName._boardHint)
		{
			_boardHint = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._selectionSummary)
		{
			_selectionSummary = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._coverageBadge)
		{
			_coverageBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._curveStatus)
		{
			_curveStatus = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._curveGraph)
		{
			_curveGraph = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._curvePicker)
		{
			_curvePicker = VariantUtils.ConvertTo<XWResourcePicker>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._responsiveSignalsConnected)
		{
			_responsiveSignalsConnected = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.MapCellPages)
		{
			value = VariantUtils.CreateFrom<TabContainer>(MapCellPages);
			return true;
		}
		bool from;
		if (name == PropertyName.IsBoardSurfaceActive)
		{
			from = IsBoardSurfaceActive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		if (name == PropertyName.IsCurveSurfaceActive)
		{
			from = IsCurveSurfaceActive;
			value = VariantUtils.CreateFrom(in from);
			return true;
		}
		int from2;
		if (name == PropertyName.GridVisualCardCount)
		{
			from2 = GridVisualCardCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName.ElementVisualCardCount)
		{
			from2 = ElementVisualCardCount;
			value = VariantUtils.CreateFrom(in from2);
			return true;
		}
		if (name == PropertyName._editingCell)
		{
			value = VariantUtils.CreateFrom(in _editingCell);
			return true;
		}
		if (name == PropertyName._layoutRoot)
		{
			value = VariantUtils.CreateFrom(in _layoutRoot);
			return true;
		}
		if (name == PropertyName._pages)
		{
			value = VariantUtils.CreateFrom(in _pages);
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
		if (name == PropertyName._startX)
		{
			value = VariantUtils.CreateFrom(in _startX);
			return true;
		}
		if (name == PropertyName._startY)
		{
			value = VariantUtils.CreateFrom(in _startY);
			return true;
		}
		if (name == PropertyName._endX)
		{
			value = VariantUtils.CreateFrom(in _endX);
			return true;
		}
		if (name == PropertyName._endY)
		{
			value = VariantUtils.CreateFrom(in _endY);
			return true;
		}
		if (name == PropertyName._boardGrid)
		{
			value = VariantUtils.CreateFrom(in _boardGrid);
			return true;
		}
		if (name == PropertyName._boardHint)
		{
			value = VariantUtils.CreateFrom(in _boardHint);
			return true;
		}
		if (name == PropertyName._selectionSummary)
		{
			value = VariantUtils.CreateFrom(in _selectionSummary);
			return true;
		}
		if (name == PropertyName._coverageBadge)
		{
			value = VariantUtils.CreateFrom(in _coverageBadge);
			return true;
		}
		if (name == PropertyName._curveStatus)
		{
			value = VariantUtils.CreateFrom(in _curveStatus);
			return true;
		}
		if (name == PropertyName._curveGraph)
		{
			value = VariantUtils.CreateFrom(in _curveGraph);
			return true;
		}
		if (name == PropertyName._curvePicker)
		{
			value = VariantUtils.CreateFrom(in _curvePicker);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._responsiveSignalsConnected)
		{
			value = VariantUtils.CreateFrom(in _responsiveSignalsConnected);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingCell, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._layoutRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._pages, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceNameEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToSceneCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._startX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._startY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._endX, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._endY, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._boardGrid, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._boardHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectionSummary, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._coverageBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._curveStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._curveGraph, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._curvePicker, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._responsiveSignalsConnected, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName.MapCellPages, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsBoardSurfaceActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsCurveSurfaceActive, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.GridVisualCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.ElementVisualCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName.IsBoardSurfaceActive, Variant.From<bool>(IsBoardSurfaceActive));
		info.AddProperty(PropertyName.IsCurveSurfaceActive, Variant.From<bool>(IsCurveSurfaceActive));
		info.AddProperty(PropertyName._editingCell, Variant.From(in _editingCell));
		info.AddProperty(PropertyName._layoutRoot, Variant.From(in _layoutRoot));
		info.AddProperty(PropertyName._pages, Variant.From(in _pages));
		info.AddProperty(PropertyName._resourceNameEdit, Variant.From(in _resourceNameEdit));
		info.AddProperty(PropertyName._localToSceneCheck, Variant.From(in _localToSceneCheck));
		info.AddProperty(PropertyName._startX, Variant.From(in _startX));
		info.AddProperty(PropertyName._startY, Variant.From(in _startY));
		info.AddProperty(PropertyName._endX, Variant.From(in _endX));
		info.AddProperty(PropertyName._endY, Variant.From(in _endY));
		info.AddProperty(PropertyName._boardGrid, Variant.From(in _boardGrid));
		info.AddProperty(PropertyName._boardHint, Variant.From(in _boardHint));
		info.AddProperty(PropertyName._selectionSummary, Variant.From(in _selectionSummary));
		info.AddProperty(PropertyName._coverageBadge, Variant.From(in _coverageBadge));
		info.AddProperty(PropertyName._curveStatus, Variant.From(in _curveStatus));
		info.AddProperty(PropertyName._curveGraph, Variant.From(in _curveGraph));
		info.AddProperty(PropertyName._curvePicker, Variant.From(in _curvePicker));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._responsiveSignalsConnected, Variant.From(in _responsiveSignalsConnected));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName.IsBoardSurfaceActive, out var value))
		{
			IsBoardSurfaceActive = value.As<bool>();
		}
		if (info.TryGetProperty(PropertyName.IsCurveSurfaceActive, out var value2))
		{
			IsCurveSurfaceActive = value2.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._editingCell, out var value3))
		{
			_editingCell = value3.As<TowerDefenseCellConfig>();
		}
		if (info.TryGetProperty(PropertyName._layoutRoot, out var value4))
		{
			_layoutRoot = value4.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._pages, out var value5))
		{
			_pages = value5.As<TabContainer>();
		}
		if (info.TryGetProperty(PropertyName._resourceNameEdit, out var value6))
		{
			_resourceNameEdit = value6.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToSceneCheck, out var value7))
		{
			_localToSceneCheck = value7.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._startX, out var value8))
		{
			_startX = value8.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._startY, out var value9))
		{
			_startY = value9.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._endX, out var value10))
		{
			_endX = value10.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._endY, out var value11))
		{
			_endY = value11.As<SpinBox>();
		}
		if (info.TryGetProperty(PropertyName._boardGrid, out var value12))
		{
			_boardGrid = value12.As<GridContainer>();
		}
		if (info.TryGetProperty(PropertyName._boardHint, out var value13))
		{
			_boardHint = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._selectionSummary, out var value14))
		{
			_selectionSummary = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._coverageBadge, out var value15))
		{
			_coverageBadge = value15.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._curveStatus, out var value16))
		{
			_curveStatus = value16.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._curveGraph, out var value17))
		{
			_curveGraph = value17.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._curvePicker, out var value18))
		{
			_curvePicker = value18.As<XWResourcePicker>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value19))
		{
			_updatingControls = value19.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._responsiveSignalsConnected, out var value20))
		{
			_responsiveSignalsConnected = value20.As<bool>();
		}
	}
}
