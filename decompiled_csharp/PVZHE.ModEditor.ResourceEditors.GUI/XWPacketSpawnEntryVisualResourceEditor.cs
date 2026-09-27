using System;
using System.Collections.Generic;
using System.ComponentModel;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using PVZHE.ModEditor.Core;

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketSpawnEntryVisualResourceEditor.cs")]
public class XWPacketSpawnEntryVisualResourceEditor : XWGenericVisualResourceEditor
{
	private enum PacketEntryMode
	{
		LevelSeedBank,
		Conveyor,
		Rain
	}

	private readonly struct PacketEntryView
	{
		public Resource Resource { get; }

		public PacketEntryMode Mode { get; }

		public StringName PacketKeyProperty { get; }

		public string PacketKey { get; }

		public TowerDefensePacketOverride Override { get; }

		public bool HasSpawnTuning { get; }

		public PacketEntryView(Resource resource, PacketEntryMode mode, StringName packetKeyProperty, string packetKey, TowerDefensePacketOverride packetOverride, bool hasSpawnTuning)
		{
			Resource = resource;
			Mode = mode;
			PacketKeyProperty = packetKeyProperty;
			PacketKey = packetKey ?? "";
			Override = packetOverride;
			HasSpawnTuning = hasSpawnTuning;
		}
	}

	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public static readonly StringName HasCompletePacketSpawnEntryVisualCoverage = "HasCompletePacketSpawnEntryVisualCoverage";

		public static readonly StringName BindWorkbench = "BindWorkbench";

		public static readonly StringName BindVisualEditing = "BindVisualEditing";

		public static readonly StringName RefreshResourceMetadata = "RefreshResourceMetadata";

		public static readonly StringName ApplyModeStage = "ApplyModeStage";

		public static readonly StringName StartModeStageTween = "StartModeStageTween";

		public static readonly StringName UpdatePreviewVisibility = "UpdatePreviewVisibility";

		public static readonly StringName SetPreviewActive = "SetPreviewActive";

		public static readonly StringName RefreshSpawnTuningVisuals = "RefreshSpawnTuningVisuals";

		public static readonly StringName RefreshWeightVisuals = "RefreshWeightVisuals";

		public static readonly StringName BeginCountRangeEdit = "BeginCountRangeEdit";

		public static readonly StringName PreviewCountRange = "PreviewCountRange";

		public static readonly StringName CommitCountRange = "CommitCountRange";

		public static readonly StringName BeginMagnificationRangeEdit = "BeginMagnificationRangeEdit";

		public static readonly StringName PreviewMagnificationRange = "PreviewMagnificationRange";

		public static readonly StringName CommitMagnificationRange = "CommitMagnificationRange";

		public static readonly StringName ShowPacketSelector = "ShowPacketSelector";

		public static readonly StringName EnsurePacketSelectorWindow = "EnsurePacketSelectorWindow";

		public static readonly StringName OnPacketSelected = "OnPacketSelected";

		public static readonly StringName CreateOverride = "CreateOverride";

		public static readonly StringName OpenOverride = "OpenOverride";

		public static readonly StringName ConfirmClearOverride = "ConfirmClearOverride";

		public static readonly StringName ClearOverride = "ClearOverride";

		public static readonly StringName RefreshPacketSpawnEntryFromHistory = "RefreshPacketSpawnEntryFromHistory";

		public static readonly StringName RefreshEntryAfterEdit = "RefreshEntryAfterEdit";

		public static readonly StringName RefreshPacketPreview = "RefreshPacketPreview";

		public static readonly StringName HydratePacketPreview = "HydratePacketPreview";

		public static readonly StringName SchedulePacketPreviewRetry = "SchedulePacketPreviewRetry";

		public static readonly StringName RetryPacketPreview = "RetryPacketPreview";

		public static readonly StringName ApplyOverrideBadge = "ApplyOverrideBadge";

		public static readonly StringName SetPacketPreviewMissing = "SetPacketPreviewMissing";

		public static readonly StringName ReleasePacketPreview = "ReleasePacketPreview";

		public static readonly StringName DisposeWorkbench = "DisposeWorkbench";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName _editingResource = "_editingResource";

		public static readonly StringName _workbenchRoot = "_workbenchRoot";

		public static readonly StringName _resourceNameEdit = "_resourceNameEdit";

		public static readonly StringName _localToSceneCheck = "_localToSceneCheck";

		public static readonly StringName _resourceMetaStatus = "_resourceMetaStatus";

		public static readonly StringName _modeBadge = "_modeBadge";

		public static readonly StringName _resourceStatus = "_resourceStatus";

		public static readonly StringName _seedBankStage = "_seedBankStage";

		public static readonly StringName _conveyorStage = "_conveyorStage";

		public static readonly StringName _rainStage = "_rainStage";

		public static readonly StringName _rainLandingRing = "_rainLandingRing";

		public static readonly StringName _packetSlot = "_packetSlot";

		public static readonly StringName _packetPreviewHost = "_packetPreviewHost";

		public static readonly StringName _missingPacketBadge = "_missingPacketBadge";

		public static readonly StringName _overrideBadge = "_overrideBadge";

		public static readonly StringName _stageHint = "_stageHint";

		public static readonly StringName _spawnTuningPanel = "_spawnTuningPanel";

		public static readonly StringName _weightMeter = "_weightMeter";

		public static readonly StringName _weightSlider = "_weightSlider";

		public static readonly StringName _weightLabel = "_weightLabel";

		public static readonly StringName _countRangeControl = "_countRangeControl";

		public static readonly StringName _magnificationRangeControl = "_magnificationRangeControl";

		public static readonly StringName _retryPreviewButton = "_retryPreviewButton";

		public static readonly StringName _choosePacketButton = "_choosePacketButton";

		public static readonly StringName _createOverrideButton = "_createOverrideButton";

		public static readonly StringName _openOverrideButton = "_openOverrideButton";

		public static readonly StringName _clearOverrideButton = "_clearOverrideButton";

		public static readonly StringName _clearOverrideConfirmation = "_clearOverrideConfirmation";

		public static readonly StringName _selectorWindow = "_selectorWindow";

		public static readonly StringName _countEditingMinimum = "_countEditingMinimum";

		public static readonly StringName _magnificationEditingMinimum = "_magnificationEditingMinimum";

		public static readonly StringName _previewRequestVersion = "_previewRequestVersion";

		public static readonly StringName _previewRetryCount = "_previewRetryCount";

		public static readonly StringName _stageTween = "_stageTween";

		public static readonly StringName _packetShow = "_packetShow";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string WorkbenchScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketSpawnEntryWorkbench.tscn";

	private const string PacketSelectorScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketSpawnEntryPacketSelectorWindow.tscn";

	private static PackedScene _workbenchScene;

	private static PackedScene _packetSelectorScene;

	private PacketEntryView _entry;

	private Resource _editingResource;

	private VBoxContainer _workbenchRoot;

	private LineEdit _resourceNameEdit;

	private CheckButton _localToSceneCheck;

	private Label _resourceMetaStatus;

	private Label _modeBadge;

	private Label _resourceStatus;

	private TextureRect _seedBankStage;

	private TextureRect _conveyorStage;

	private Control _rainStage;

	private Control _rainLandingRing;

	private Control _packetSlot;

	private Control _packetPreviewHost;

	private Button _missingPacketBadge;

	private Button _overrideBadge;

	private Label _stageHint;

	private PanelContainer _spawnTuningPanel;

	private TextureProgressBar _weightMeter;

	private HSlider _weightSlider;

	private Label _weightLabel;

	private XWPacketSpawnEntryRangeControl _countRangeControl;

	private XWPacketSpawnEntryRangeControl _magnificationRangeControl;

	private Button _retryPreviewButton;

	private Button _choosePacketButton;

	private Button _createOverrideButton;

	private Button _openOverrideButton;

	private Button _clearOverrideButton;

	private ConfirmationDialog _clearOverrideConfirmation;

	private XWPacketSpawnEntryPacketSelectorWindow _selectorWindow;

	private XWVisualPropertyBinding _propertyBinding;

	private bool _countEditingMinimum;

	private bool _magnificationEditingMinimum;

	private int _previewRequestVersion;

	private int _previewRetryCount;

	private Tween _stageTween;

	private TowerDefenseInGamePacketShow _packetShow;

	public override void _Ready()
	{
		SetProcess(enable: false);
		VisibilityChanged += UpdatePreviewVisibility;
		base._Ready();
		SetPreviewActive(IsVisibleInTree());
	}

	public override void _ExitTree()
	{
		VisibilityChanged -= UpdatePreviewVisibility;
		DisposeWorkbench();
		if (GodotObject.IsInstanceValid(_selectorWindow))
		{
			_selectorWindow.PacketSelected -= OnPacketSelected;
			_selectorWindow.CancelPendingRequests();
		}
		base._ExitTree();
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeWorkbench();
		if (CanvasGrid == null)
		{
			return;
		}
		if (!TryCreateEntryView(CurrentResource, out var view))
		{
			base.RenderCustomVisualPreset(preset);
			return;
		}
		if (_workbenchScene == null)
		{
			_workbenchScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketSpawnEntryWorkbench.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		_workbenchRoot = _workbenchScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(_workbenchRoot))
		{
			_editingResource = view.Resource;
			_entry = view;
			_propertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), NotifyCurrentResourceEdited);
			CanvasGrid.Columns = 1;
			CanvasGrid.AddChild(_workbenchRoot, forceReadableName: false, InternalMode.Disabled);
			BindWorkbench(_workbenchRoot);
			ApplyModeStage();
			BindVisualEditing();
			RefreshSpawnTuningVisuals();
			RefreshPacketPreview();
		}
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!HasCompletePacketSpawnEntryVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool HasCompletePacketSpawnEntryVisualCoverage(Resource resource)
	{
		Type type = resource?.GetType();
		if (!(type == typeof(TowerDefenseLevelPacketConfig)) && !(type == typeof(TowerDefenseConveyorPacketConfig)))
		{
			return type == typeof(TowerDefenseRainModePacketConfig);
		}
		return true;
	}

	private static bool TryCreateEntryView(Resource resource, out PacketEntryView view)
	{
		if (!(resource is TowerDefenseLevelPacketConfig towerDefenseLevelPacketConfig))
		{
			if (!(resource is TowerDefenseConveyorPacketConfig towerDefenseConveyorPacketConfig))
			{
				if (resource is TowerDefenseRainModePacketConfig towerDefenseRainModePacketConfig)
				{
					view = new PacketEntryView(towerDefenseRainModePacketConfig, PacketEntryMode.Rain, "name", towerDefenseRainModePacketConfig.name, towerDefenseRainModePacketConfig.@override, hasSpawnTuning: true);
					return true;
				}
				view = default;
				return false;
			}
			view = new PacketEntryView(towerDefenseConveyorPacketConfig, PacketEntryMode.Conveyor, "name", towerDefenseConveyorPacketConfig.name, towerDefenseConveyorPacketConfig.@override, hasSpawnTuning: true);
			return true;
		}
		view = new PacketEntryView(towerDefenseLevelPacketConfig, PacketEntryMode.LevelSeedBank, "packetName", towerDefenseLevelPacketConfig.packetName, towerDefenseLevelPacketConfig.@override, hasSpawnTuning: false);
		return true;
	}

	private void BindWorkbench(VBoxContainer root)
	{
		_resourceNameEdit = root.GetNode<LineEdit>("%ResourceNameEdit");
		_localToSceneCheck = root.GetNode<CheckButton>("%LocalToSceneCheck");
		_resourceMetaStatus = root.GetNode<Label>("%ResourceMetaStatus");
		_modeBadge = root.GetNode<Label>("%ModeBadge");
		_resourceStatus = root.GetNode<Label>("%ResourceStatus");
		_seedBankStage = root.GetNode<TextureRect>("%SeedBankStage");
		_conveyorStage = root.GetNode<TextureRect>("%ConveyorStage");
		_rainStage = root.GetNode<Control>("%RainStage");
		_rainLandingRing = _rainStage.GetNode<Control>("RainLandingRing");
		_packetSlot = root.GetNode<Control>("%PacketSlot");
		_packetPreviewHost = root.GetNode<Control>("%PacketPreviewHost");
		_missingPacketBadge = root.GetNode<Button>("%MissingPacketBadge");
		_overrideBadge = root.GetNode<Button>("%OverrideBadge");
		_stageHint = root.GetNode<Label>("%StageHint");
		_spawnTuningPanel = root.GetNode<PanelContainer>("%SpawnTuningPanel");
		_weightMeter = root.GetNode<TextureProgressBar>("%WeightMeter");
		_weightSlider = root.GetNode<HSlider>("%WeightSlider");
		_weightLabel = root.GetNode<Label>("%WeightLabel");
		_countRangeControl = root.GetNode<XWPacketSpawnEntryRangeControl>("%CountRangeControl");
		_magnificationRangeControl = root.GetNode<XWPacketSpawnEntryRangeControl>("%MagnificationRangeControl");
		_retryPreviewButton = root.GetNode<Button>("%RetryPreviewButton");
		_choosePacketButton = root.GetNode<Button>("%ChoosePacketButton");
		_createOverrideButton = root.GetNode<Button>("%CreateOverrideButton");
		_openOverrideButton = root.GetNode<Button>("%OpenOverrideButton");
		_clearOverrideButton = root.GetNode<Button>("%ClearOverrideButton");
		_clearOverrideConfirmation = root.GetNode<ConfirmationDialog>("%ClearOverrideConfirmation");
		_retryPreviewButton.Pressed += RefreshPacketPreview;
		_choosePacketButton.Pressed += ShowPacketSelector;
		_missingPacketBadge.Pressed += ShowPacketSelector;
		_overrideBadge.Pressed += OpenOverride;
		_createOverrideButton.Pressed += CreateOverride;
		_openOverrideButton.Pressed += OpenOverride;
		_clearOverrideButton.Pressed += ConfirmClearOverride;
		_clearOverrideConfirmation.Confirmed += ClearOverride;
	}

	private void BindVisualEditing()
	{
		_propertyBinding.BindText(_resourceNameEdit, _entry.Resource, "resource_name", RefreshResourceMetadata, this, "RefreshPacketSpawnEntryFromHistory");
		_propertyBinding.BindToggle(_localToSceneCheck, _entry.Resource, "resource_local_to_scene", RefreshResourceMetadata, this, "RefreshPacketSpawnEntryFromHistory");
		RefreshResourceMetadata();
		if (_entry.HasSpawnTuning && _propertyBinding != null)
		{
			_weightSlider.MinValue = 0.0;
			_weightSlider.MaxValue = 999.0;
			_weightSlider.Step = 1.0;
			_propertyBinding.BindNumber(_weightSlider, _entry.Resource, "weight", RefreshWeightVisuals, this, "RefreshPacketSpawnEntryFromHistory");
			_countRangeControl.SetTitle("生成数量区间");
			_countRangeControl.Configure(-1.0, 999.0, 1.0, allowUnlimited: true);
			_countRangeControl.DragStarted += BeginCountRangeEdit;
			_countRangeControl.ValuesPreviewed += PreviewCountRange;
			_countRangeControl.ValuesCommitted += CommitCountRange;
			_magnificationRangeControl.SetTitle("低数量 / 高数量倍率");
			_magnificationRangeControl.Configure(0.0, 10.0, 0.05, allowUnlimited: false);
			_magnificationRangeControl.DragStarted += BeginMagnificationRangeEdit;
			_magnificationRangeControl.ValuesPreviewed += PreviewMagnificationRange;
			_magnificationRangeControl.ValuesCommitted += CommitMagnificationRange;
		}
	}

	private void RefreshResourceMetadata()
	{
		if (GodotObject.IsInstanceValid(_editingResource) && GodotObject.IsInstanceValid(_resourceMetaStatus))
		{
			string text = (string.IsNullOrWhiteSpace(_editingResource.ResourceName) ? _editingResource.GetType().Name : _editingResource.ResourceName);
			_resourceMetaStatus.Text = text + " · " + (_editingResource.ResourceLocalToScene ? "仅当前场景" : "可复用资源");
		}
	}

	private void ApplyModeStage()
	{
		if (GodotObject.IsInstanceValid(_modeBadge))
		{
			bool flag = _entry.Mode == PacketEntryMode.LevelSeedBank;
			bool flag2 = _entry.Mode == PacketEntryMode.Conveyor;
			bool visible = _entry.Mode == PacketEntryMode.Rain;
			_seedBankStage.Visible = flag;
			_conveyorStage.Visible = flag2;
			_rainStage.Visible = visible;
			_spawnTuningPanel.Visible = _entry.HasSpawnTuning;
			if (flag)
			{
				_modeBadge.Text = "种子栏条目";
				_stageHint.Text = "卡牌位于游戏种子栏槽位；卡面使用运行时配置副本";
				_packetSlot.Position = new Vector2(216f, 83f);
			}
			else if (flag2)
			{
				_modeBadge.Text = "传送带候选";
				_stageHint.Text = "卡牌位于传送带轨道；右侧展示实际生成权重与区间";
				_packetSlot.Position = new Vector2(216f, 145f);
			}
			else
			{
				_modeBadge.Text = "种子雨候选";
				_stageHint.Text = "卡牌位于种子雨掉落轨迹；落点环表示游戏内可拾取位置";
				_packetSlot.Position = new Vector2(216f, 76f);
			}
			_packetSlot.Size = new Vector2(160f, 220f);
			StartModeStageTween();
			SetPreviewActive(IsVisibleInTree());
		}
	}

	private void StartModeStageTween()
	{
		_stageTween?.Kill();
		_stageTween = null;
		if (!GodotObject.IsInstanceValid(_packetSlot) || _entry.Mode == PacketEntryMode.LevelSeedBank)
		{
			return;
		}
		if (_entry.Mode == PacketEntryMode.Conveyor)
		{
			Vector2 position = new Vector2(216f, 145f);
			_packetSlot.Position = position;
			_stageTween = CreateTween().SetLoops();
			_stageTween.SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
			_stageTween.TweenProperty(_packetSlot, "position:x", 338f, 1.6);
			_stageTween.TweenProperty(_packetSlot, "position:x", position.X, 1.6);
		}
		else
		{
			if (_entry.Mode != PacketEntryMode.Rain)
			{
				return;
			}
			Vector2 start = new Vector2(216f, 52f);
			_packetSlot.Position = start;
			if (GodotObject.IsInstanceValid(_rainLandingRing))
			{
				_rainLandingRing.PivotOffset = _rainLandingRing.Size * 0.5f;
				_rainLandingRing.Scale = new Vector2(0.78f, 0.78f);
				_rainLandingRing.Modulate = new Color(1f, 1f, 1f, 0.42f);
			}
			_stageTween = CreateTween().SetLoops();
			_stageTween.SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.In);
			_stageTween.TweenProperty(_packetSlot, "position:y", 222f, 1.35);
			if (GodotObject.IsInstanceValid(_rainLandingRing))
			{
				_stageTween.Parallel().TweenProperty(_rainLandingRing, "scale", new Vector2(1.12f, 1.12f), 1.35);
				_stageTween.Parallel().TweenProperty(_rainLandingRing, "modulate:a", 0.95f, 1.35);
			}
			_stageTween.TweenInterval(0.22);
			_stageTween.TweenCallback(Callable.From(() =>
			{
				if (GodotObject.IsInstanceValid(_packetSlot))
				{
					_packetSlot.Position = start;
				}
				if (GodotObject.IsInstanceValid(_rainLandingRing))
				{
					_rainLandingRing.Scale = new Vector2(0.78f, 0.78f);
					_rainLandingRing.Modulate = new Color(1f, 1f, 1f, 0.42f);
				}
			}));
		}
	}

	private void UpdatePreviewVisibility()
	{
		bool flag = IsVisibleInTree();
		if (flag && GodotObject.IsInstanceValid(_editingResource) && TryCreateEntryView(_editingResource, out var view))
		{
			_entry = view;
			RefreshSpawnTuningVisuals();
			RefreshPacketPreview();
		}
		SetPreviewActive(flag);
	}

	private void SetPreviewActive(bool active)
	{
		active = active && GodotObject.IsInstanceValid(_editingResource) && GodotObject.IsInstanceValid(_workbenchRoot);
		SetProcess(enable: false);
		if (GodotObject.IsInstanceValid(_packetShow))
		{
			_packetShow.ProcessMode = (ProcessModeEnum)(active ? 0 : 4);
		}
		if (active)
		{
			_stageTween?.Play();
			return;
		}
		_stageTween?.Pause();
		if (GodotObject.IsInstanceValid(_selectorWindow))
		{
			_selectorWindow.CancelPendingRequests();
			_selectorWindow.Hide();
		}
	}

	private void RefreshSpawnTuningVisuals()
	{
		if (!GodotObject.IsInstanceValid(_spawnTuningPanel))
		{
			return;
		}
		if (!_entry.HasSpawnTuning)
		{
			_spawnTuningPanel.Visible = false;
			return;
		}
		int weight;
		int minNum;
		int maxNum;
		double minMagnification;
		double maxMagnification;
		if (_entry.Resource is TowerDefenseConveyorPacketConfig towerDefenseConveyorPacketConfig)
		{
			weight = towerDefenseConveyorPacketConfig.weight;
			minNum = towerDefenseConveyorPacketConfig.minNum;
			maxNum = towerDefenseConveyorPacketConfig.maxNum;
			minMagnification = towerDefenseConveyorPacketConfig.minMagnification;
			maxMagnification = towerDefenseConveyorPacketConfig.maxMagnification;
		}
		else
		{
			if (!(_entry.Resource is TowerDefenseRainModePacketConfig towerDefenseRainModePacketConfig))
			{
				return;
			}
			weight = towerDefenseRainModePacketConfig.weight;
			minNum = towerDefenseRainModePacketConfig.minNum;
			maxNum = towerDefenseRainModePacketConfig.maxNum;
			minMagnification = towerDefenseRainModePacketConfig.minMagnification;
			maxMagnification = towerDefenseRainModePacketConfig.maxMagnification;
		}
		double maxValue = Math.Max(100, Math.Max(1, weight));
		_weightMeter.MaxValue = maxValue;
		_weightMeter.Value = Math.Max(0, weight);
		_weightSlider.MaxValue = 999.0;
		_weightSlider.Value = Math.Max(0, weight);
		_weightLabel.Text = $"相对权重 {weight}";
		_countRangeControl?.SetValues(minNum, maxNum);
		_magnificationRangeControl?.SetValues(minMagnification, maxMagnification);
	}

	private void RefreshWeightVisuals()
	{
		if (GodotObject.IsInstanceValid(_weightMeter) && TryReadSpawnTuning(out var weight, out var _, out var _, out var _, out var _))
		{
			_weightMeter.MaxValue = Math.Max(100, Math.Max(1, weight));
			_weightMeter.Value = Math.Max(0, weight);
			_weightLabel.Text = $"相对权重 {weight}";
		}
	}

	private bool TryReadSpawnTuning(out int weight, out int minimumCount, out int maximumCount, out double minimumMagnification, out double maximumMagnification)
	{
		if (_entry.Resource is TowerDefenseConveyorPacketConfig towerDefenseConveyorPacketConfig)
		{
			weight = towerDefenseConveyorPacketConfig.weight;
			minimumCount = towerDefenseConveyorPacketConfig.minNum;
			maximumCount = towerDefenseConveyorPacketConfig.maxNum;
			minimumMagnification = towerDefenseConveyorPacketConfig.minMagnification;
			maximumMagnification = towerDefenseConveyorPacketConfig.maxMagnification;
			return true;
		}
		if (_entry.Resource is TowerDefenseRainModePacketConfig towerDefenseRainModePacketConfig)
		{
			weight = towerDefenseRainModePacketConfig.weight;
			minimumCount = towerDefenseRainModePacketConfig.minNum;
			maximumCount = towerDefenseRainModePacketConfig.maxNum;
			minimumMagnification = towerDefenseRainModePacketConfig.minMagnification;
			maximumMagnification = towerDefenseRainModePacketConfig.maxMagnification;
			return true;
		}
		weight = 0;
		minimumCount = -1;
		maximumCount = -1;
		minimumMagnification = 0.0;
		maximumMagnification = 0.0;
		return false;
	}

	private void BeginCountRangeEdit(bool minimumHandle)
	{
		if (_propertyBinding != null && GodotObject.IsInstanceValid(_entry.Resource))
		{
			_countEditingMinimum = minimumHandle;
			_propertyBinding.BeginEdit(_entry.Resource, minimumHandle ? "minNum" : "maxNum");
		}
	}

	private void PreviewCountRange(double minimum, double maximum)
	{
		if (_propertyBinding != null && GodotObject.IsInstanceValid(_entry.Resource))
		{
			StringName property = (_countEditingMinimum ? "minNum" : "maxNum");
			int from = (int)Math.Round(_countEditingMinimum ? minimum : maximum);
			_propertyBinding.PreviewValue(_entry.Resource, property, Variant.From(in from));
		}
	}

	private void CommitCountRange(double minimum, double maximum)
	{
		if (_propertyBinding != null && GodotObject.IsInstanceValid(_entry.Resource))
		{
			StringName property = (_countEditingMinimum ? "minNum" : "maxNum");
			int from = (int)Math.Round(_countEditingMinimum ? minimum : maximum);
			_propertyBinding.CommitEdit(_entry.Resource, property, Variant.From(in from), "修改生成数量范围", this, "RefreshPacketSpawnEntryFromHistory");
		}
	}

	private void BeginMagnificationRangeEdit(bool minimumHandle)
	{
		if (_propertyBinding != null && GodotObject.IsInstanceValid(_entry.Resource))
		{
			_magnificationEditingMinimum = minimumHandle;
			_propertyBinding.BeginEdit(_entry.Resource, minimumHandle ? "minMagnification" : "maxMagnification");
		}
	}

	private void PreviewMagnificationRange(double minimum, double maximum)
	{
		if (_propertyBinding != null && GodotObject.IsInstanceValid(_entry.Resource))
		{
			StringName property = (_magnificationEditingMinimum ? "minMagnification" : "maxMagnification");
			double from = (_magnificationEditingMinimum ? minimum : maximum);
			_propertyBinding.PreviewValue(_entry.Resource, property, Variant.From(in from));
		}
	}

	private void CommitMagnificationRange(double minimum, double maximum)
	{
		if (_propertyBinding != null && GodotObject.IsInstanceValid(_entry.Resource))
		{
			StringName property = (_magnificationEditingMinimum ? "minMagnification" : "maxMagnification");
			double from = (_magnificationEditingMinimum ? minimum : maximum);
			_propertyBinding.CommitEdit(_entry.Resource, property, Variant.From(in from), "修改生成倍率范围", this, "RefreshPacketSpawnEntryFromHistory");
		}
	}

	private void ShowPacketSelector()
	{
		EnsurePacketSelectorWindow();
		_selectorWindow?.ShowFor(_entry.PacketKey);
	}

	private void EnsurePacketSelectorWindow()
	{
		if (!GodotObject.IsInstanceValid(_selectorWindow))
		{
			if (_packetSelectorScene == null)
			{
				_packetSelectorScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWPacketSpawnEntryPacketSelectorWindow.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			_selectorWindow = _packetSelectorScene?.Instantiate<XWPacketSpawnEntryPacketSelectorWindow>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(_selectorWindow))
			{
				_selectorWindow.PacketSelected += OnPacketSelected;
				AddChild(_selectorWindow, forceReadableName: false, InternalMode.Disabled);
			}
		}
	}

	private void OnPacketSelected(string packetKey)
	{
		if (_propertyBinding != null && GodotObject.IsInstanceValid(_entry.Resource) && !string.IsNullOrWhiteSpace(packetKey))
		{
			_propertyBinding.SetValue(_entry.Resource, _entry.PacketKeyProperty, Variant.From(in packetKey), "选择关卡卡牌", this, "RefreshPacketSpawnEntryFromHistory");
			RefreshEntryAfterEdit();
		}
	}

	private void CreateOverride()
	{
		if (_propertyBinding != null && GodotObject.IsInstanceValid(_entry.Resource) && !GodotObject.IsInstanceValid(_entry.Override))
		{
			_propertyBinding.SetValue(_entry.Resource, "override", Variant.From<TowerDefensePacketOverride>(new TowerDefensePacketOverride()), "创建卡牌覆盖规则", this, "RefreshPacketSpawnEntryFromHistory");
			RefreshEntryAfterEdit();
		}
	}

	private void OpenOverride()
	{
		if (GodotObject.IsInstanceValid(_entry.Override))
		{
			XWResourceEditContext context = XWResourceEditContext.ForProperty(_entry.Override, _entry.Resource, _entry.Override.ResourcePath, CurrentResourcePath, "override", -1, "packet_spawn_entry_editor", CurrentEditContext?.IsBuiltInSource ?? false);
			XWEditorInterface.Instance?.EditResource(_entry.Override, context);
		}
	}

	private void ConfirmClearOverride()
	{
		if (GodotObject.IsInstanceValid(_entry.Override))
		{
			_clearOverrideConfirmation?.PopupCentered();
		}
	}

	private void ClearOverride()
	{
		if (_propertyBinding != null && GodotObject.IsInstanceValid(_entry.Resource) && GodotObject.IsInstanceValid(_entry.Override))
		{
			_propertyBinding.SetValue(_entry.Resource, "override", Variant.From<TowerDefensePacketOverride>((TowerDefensePacketOverride)null), "清除卡牌覆盖规则", this, "RefreshPacketSpawnEntryFromHistory");
			RefreshEntryAfterEdit();
		}
	}

	public void RefreshPacketSpawnEntryFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager == null || (!xWUndoRedoManager.IsUndoing() && !xWUndoRedoManager.IsRedoing()) || CanvasGrid == null || !TryCreateEntryView(CurrentResource, out var _))
		{
			return;
		}
		DisposeWorkbench();
		foreach (Node child in CanvasGrid.GetChildren())
		{
			CanvasGrid.RemoveChild(child);
			child.QueueFree();
		}
		RenderCustomVisualPreset(null);
	}

	private void RefreshEntryAfterEdit()
	{
		if (TryCreateEntryView(_editingResource, out var view))
		{
			_entry = view;
			RefreshSpawnTuningVisuals();
			RefreshPacketPreview();
		}
	}

	private void RefreshPacketPreview()
	{
		_previewRequestVersion++;
		_previewRetryCount = 0;
		HydratePacketPreview(_previewRequestVersion);
	}

	private void HydratePacketPreview(int requestVersion)
	{
		if (requestVersion != _previewRequestVersion || _editingResource != CurrentResource)
		{
			return;
		}
		ReleasePacketPreview();
		if (!GodotObject.IsInstanceValid(_packetPreviewHost))
		{
			return;
		}
		string text = _entry.PacketKey?.Trim() ?? "";
		if (string.IsNullOrWhiteSpace(text))
		{
			SetPacketPreviewMissing("尚未选择卡牌");
			return;
		}
		TowerDefensePacketConfig packetConfigReadOnly;
		try
		{
			packetConfigReadOnly = TowerDefenseManager.GetPacketConfigReadOnly(text);
		}
		catch (Exception ex)
		{
			SchedulePacketPreviewRetry(requestVersion, "卡牌注册表未就绪：" + ex.Message);
			return;
		}
		TowerDefensePacketConfig towerDefensePacketConfig = packetConfigReadOnly?.Duplicate(deep: true) as TowerDefensePacketConfig;
		if (!GodotObject.IsInstanceValid(towerDefensePacketConfig))
		{
			SchedulePacketPreviewRetry(requestVersion, "找不到卡牌：" + text);
			return;
		}
		bool flag = GodotObject.IsInstanceValid(_entry.Override);
		bool invalid = false;
		if (flag)
		{
			towerDefensePacketConfig._override = _entry.Override.Duplicate(deep: true) as TowerDefensePacketOverride;
			invalid = !GodotObject.IsInstanceValid(towerDefensePacketConfig._override);
		}
		TowerDefenseInGamePacketShow towerDefenseInGamePacketShow = null;
		try
		{
			towerDefenseInGamePacketShow = TowerDefenseManager.CreatePacketShowWithConfig();
			if (!GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				SchedulePacketPreviewRetry(requestVersion, "运行时卡牌场景不可用");
				return;
			}
			if (requestVersion != _previewRequestVersion || _editingResource != CurrentResource)
			{
				towerDefenseInGamePacketShow.QueueFree();
				return;
			}
			towerDefenseInGamePacketShow.Name = "PacketSpawnEntryPreview";
			towerDefenseInGamePacketShow.onlyDraw = true;
			towerDefenseInGamePacketShow.setPcLayout = true;
			towerDefenseInGamePacketShow.showLove = false;
			towerDefenseInGamePacketShow.select = false;
			towerDefenseInGamePacketShow.alive = true;
			towerDefenseInGamePacketShow.@lock = false;
			towerDefenseInGamePacketShow.MouseFilter = MouseFilterEnum.Ignore;
			towerDefenseInGamePacketShow.ProcessMode = ProcessModeEnum.Inherit;
			towerDefenseInGamePacketShow.Position = new Vector2(80f, 110f);
			towerDefenseInGamePacketShow.Scale = new Vector2(2.65f, 2.65f);
			_packetPreviewHost.AddChild(towerDefenseInGamePacketShow, forceReadableName: false, InternalMode.Disabled);
			towerDefenseInGamePacketShow.Init(towerDefensePacketConfig);
			towerDefenseInGamePacketShow.onlyDraw = true;
			towerDefenseInGamePacketShow.RefreshPreview();
			_packetShow = towerDefenseInGamePacketShow;
			_previewRetryCount = 0;
		}
		catch (Exception ex2)
		{
			if (GodotObject.IsInstanceValid(towerDefenseInGamePacketShow))
			{
				towerDefenseInGamePacketShow.GetParent()?.RemoveChild(towerDefenseInGamePacketShow);
				towerDefenseInGamePacketShow.QueueFree();
			}
			SchedulePacketPreviewRetry(requestVersion, "卡面预览失败：" + ex2.Message);
			return;
		}
		_missingPacketBadge.Visible = false;
		_resourceStatus.Text = "卡牌：" + text;
		ApplyOverrideBadge(flag, invalid);
		SetPreviewActive(IsVisibleInTree());
	}

	private void SchedulePacketPreviewRetry(int requestVersion, string stableMessage)
	{
		if (requestVersion != _previewRequestVersion || _editingResource != CurrentResource)
		{
			return;
		}
		if (_previewRetryCount >= 3)
		{
			SetPacketPreviewMissing(stableMessage);
			return;
		}
		_previewRetryCount++;
		if (GodotObject.IsInstanceValid(_resourceStatus))
		{
			_resourceStatus.Text = $"正在准备卡面预览（{_previewRetryCount}/3）";
		}
		CallDeferred("RetryPacketPreview", requestVersion, stableMessage);
	}

	private void RetryPacketPreview(int requestVersion, string stableMessage)
	{
		if (requestVersion == _previewRequestVersion && _editingResource == CurrentResource)
		{
			HydratePacketPreview(requestVersion);
		}
	}

	private void ApplyOverrideBadge(bool hasOverride, bool invalid)
	{
		if (GodotObject.IsInstanceValid(_overrideBadge))
		{
			_overrideBadge.Visible = hasOverride;
			_overrideBadge.Text = (invalid ? "覆盖错误" : "已覆盖");
			_overrideBadge.Modulate = (invalid ? new Color(1f, 0.38f, 0.3f) : Colors.White);
			_createOverrideButton.Disabled = hasOverride;
			_openOverrideButton.Disabled = !hasOverride;
			_clearOverrideButton.Disabled = !hasOverride;
		}
	}

	private void SetPacketPreviewMissing(string message)
	{
		if (GodotObject.IsInstanceValid(_missingPacketBadge))
		{
			_missingPacketBadge.Visible = true;
		}
		if (GodotObject.IsInstanceValid(_overrideBadge))
		{
			_overrideBadge.Visible = GodotObject.IsInstanceValid(_entry.Override);
		}
		if (GodotObject.IsInstanceValid(_resourceStatus))
		{
			_resourceStatus.Text = message;
		}
		ApplyOverrideBadge(GodotObject.IsInstanceValid(_entry.Override), invalid: false);
	}

	private void ReleasePacketPreview()
	{
		if (GodotObject.IsInstanceValid(_packetShow))
		{
			_packetShow.ClearEventHandlers();
			_packetShow.GetParent()?.RemoveChild(_packetShow);
			_packetShow.QueueFree();
		}
		_packetShow = null;
	}

	private void DisposeWorkbench()
	{
		_previewRequestVersion++;
		_propertyBinding?.Dispose();
		_propertyBinding = null;
		_selectorWindow?.CancelPendingRequests();
		if (GodotObject.IsInstanceValid(_selectorWindow))
		{
			_selectorWindow.Hide();
		}
		_stageTween?.Kill();
		_stageTween = null;
		if (GodotObject.IsInstanceValid(_retryPreviewButton))
		{
			_retryPreviewButton.Pressed -= RefreshPacketPreview;
		}
		if (GodotObject.IsInstanceValid(_choosePacketButton))
		{
			_choosePacketButton.Pressed -= ShowPacketSelector;
		}
		if (GodotObject.IsInstanceValid(_missingPacketBadge))
		{
			_missingPacketBadge.Pressed -= ShowPacketSelector;
		}
		if (GodotObject.IsInstanceValid(_overrideBadge))
		{
			_overrideBadge.Pressed -= OpenOverride;
		}
		if (GodotObject.IsInstanceValid(_createOverrideButton))
		{
			_createOverrideButton.Pressed -= CreateOverride;
		}
		if (GodotObject.IsInstanceValid(_openOverrideButton))
		{
			_openOverrideButton.Pressed -= OpenOverride;
		}
		if (GodotObject.IsInstanceValid(_clearOverrideButton))
		{
			_clearOverrideButton.Pressed -= ConfirmClearOverride;
		}
		if (GodotObject.IsInstanceValid(_clearOverrideConfirmation))
		{
			_clearOverrideConfirmation.Confirmed -= ClearOverride;
		}
		if (GodotObject.IsInstanceValid(_countRangeControl))
		{
			_countRangeControl.DragStarted -= BeginCountRangeEdit;
			_countRangeControl.ValuesPreviewed -= PreviewCountRange;
			_countRangeControl.ValuesCommitted -= CommitCountRange;
		}
		if (GodotObject.IsInstanceValid(_magnificationRangeControl))
		{
			_magnificationRangeControl.DragStarted -= BeginMagnificationRangeEdit;
			_magnificationRangeControl.ValuesPreviewed -= PreviewMagnificationRange;
			_magnificationRangeControl.ValuesCommitted -= CommitMagnificationRange;
		}
		ReleasePacketPreview();
		_editingResource = null;
		_entry = default;
		_workbenchRoot = null;
		_resourceNameEdit = null;
		_localToSceneCheck = null;
		_resourceMetaStatus = null;
		_modeBadge = null;
		_resourceStatus = null;
		_seedBankStage = null;
		_conveyorStage = null;
		_rainStage = null;
		_rainLandingRing = null;
		_packetSlot = null;
		_packetPreviewHost = null;
		_missingPacketBadge = null;
		_overrideBadge = null;
		_stageHint = null;
		_spawnTuningPanel = null;
		_weightMeter = null;
		_weightSlider = null;
		_weightLabel = null;
		_countRangeControl = null;
		_magnificationRangeControl = null;
		_retryPreviewButton = null;
		_choosePacketButton = null;
		_createOverrideButton = null;
		_openOverrideButton = null;
		_clearOverrideButton = null;
		_clearOverrideConfirmation = null;
		SetProcess(enable: false);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<MethodInfo> GetGodotMethodList()
	{
		return new List<MethodInfo>(35)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCompletePacketSpawnEntryVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindVisualEditing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshResourceMetadata, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ApplyModeStage, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.StartModeStageTween, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdatePreviewVisibility, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SetPreviewActive, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "active", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshSpawnTuningVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshWeightVisuals, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BeginCountRangeEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "minimumHandle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PreviewCountRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitCountRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BeginMagnificationRangeEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "minimumHandle", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.PreviewMagnificationRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CommitMagnificationRange, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Float, "minimum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maximum", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ShowPacketSelector, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.EnsurePacketSelectorWindow, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnPacketSelected, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "packetKey", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateOverride, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OpenOverride, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ConfirmClearOverride, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ClearOverride, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPacketSpawnEntryFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshEntryAfterEdit, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshPacketPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HydratePacketPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "requestVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SchedulePacketPreviewRetry, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "requestVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "stableMessage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RetryPacketPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "requestVersion", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "stableMessage", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ApplyOverrideBadge, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "hasOverride", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Bool, "invalid", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetPacketPreviewMissing, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "message", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReleasePacketPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.DisposeWorkbench, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null)
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
		if (method == MethodName.HasCompletePacketSpawnEntryVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompletePacketSpawnEntryVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BindWorkbench && args.Count == 1)
		{
			BindWorkbench(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindVisualEditing && args.Count == 0)
		{
			BindVisualEditing();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshResourceMetadata && args.Count == 0)
		{
			RefreshResourceMetadata();
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyModeStage && args.Count == 0)
		{
			ApplyModeStage();
			ret = default;
			return true;
		}
		if (method == MethodName.StartModeStageTween && args.Count == 0)
		{
			StartModeStageTween();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdatePreviewVisibility && args.Count == 0)
		{
			UpdatePreviewVisibility();
			ret = default;
			return true;
		}
		if (method == MethodName.SetPreviewActive && args.Count == 1)
		{
			SetPreviewActive(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshSpawnTuningVisuals && args.Count == 0)
		{
			RefreshSpawnTuningVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshWeightVisuals && args.Count == 0)
		{
			RefreshWeightVisuals();
			ret = default;
			return true;
		}
		if (method == MethodName.BeginCountRangeEdit && args.Count == 1)
		{
			BeginCountRangeEdit(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewCountRange && args.Count == 2)
		{
			PreviewCountRange(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitCountRange && args.Count == 2)
		{
			CommitCountRange(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BeginMagnificationRangeEdit && args.Count == 1)
		{
			BeginMagnificationRangeEdit(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.PreviewMagnificationRange && args.Count == 2)
		{
			PreviewMagnificationRange(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.CommitMagnificationRange && args.Count == 2)
		{
			CommitMagnificationRange(VariantUtils.ConvertTo<double>(in args[0]), VariantUtils.ConvertTo<double>(in args[1]));
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
		if (method == MethodName.OnPacketSelected && args.Count == 1)
		{
			OnPacketSelected(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateOverride && args.Count == 0)
		{
			CreateOverride();
			ret = default;
			return true;
		}
		if (method == MethodName.OpenOverride && args.Count == 0)
		{
			OpenOverride();
			ret = default;
			return true;
		}
		if (method == MethodName.ConfirmClearOverride && args.Count == 0)
		{
			ConfirmClearOverride();
			ret = default;
			return true;
		}
		if (method == MethodName.ClearOverride && args.Count == 0)
		{
			ClearOverride();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketSpawnEntryFromHistory && args.Count == 0)
		{
			RefreshPacketSpawnEntryFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshEntryAfterEdit && args.Count == 0)
		{
			RefreshEntryAfterEdit();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshPacketPreview && args.Count == 0)
		{
			RefreshPacketPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.HydratePacketPreview && args.Count == 1)
		{
			HydratePacketPreview(VariantUtils.ConvertTo<int>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SchedulePacketPreviewRetry && args.Count == 2)
		{
			SchedulePacketPreviewRetry(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RetryPacketPreview && args.Count == 2)
		{
			RetryPacketPreview(VariantUtils.ConvertTo<int>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.ApplyOverrideBadge && args.Count == 2)
		{
			ApplyOverrideBadge(VariantUtils.ConvertTo<bool>(in args[0]), VariantUtils.ConvertTo<bool>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetPacketPreviewMissing && args.Count == 1)
		{
			SetPacketPreviewMissing(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ReleasePacketPreview && args.Count == 0)
		{
			ReleasePacketPreview();
			ret = default;
			return true;
		}
		if (method == MethodName.DisposeWorkbench && args.Count == 0)
		{
			DisposeWorkbench();
			ret = default;
			return true;
		}
		return base.InvokeGodotClassMethod(in method, args, out ret);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static bool InvokeGodotClassStaticMethod(in godot_string_name method, NativeVariantPtrArgs args, out godot_variant ret)
	{
		if (method == MethodName.HasCompletePacketSpawnEntryVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompletePacketSpawnEntryVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
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
		if (method == MethodName.HasCompletePacketSpawnEntryVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.BindWorkbench)
		{
			return true;
		}
		if (method == MethodName.BindVisualEditing)
		{
			return true;
		}
		if (method == MethodName.RefreshResourceMetadata)
		{
			return true;
		}
		if (method == MethodName.ApplyModeStage)
		{
			return true;
		}
		if (method == MethodName.StartModeStageTween)
		{
			return true;
		}
		if (method == MethodName.UpdatePreviewVisibility)
		{
			return true;
		}
		if (method == MethodName.SetPreviewActive)
		{
			return true;
		}
		if (method == MethodName.RefreshSpawnTuningVisuals)
		{
			return true;
		}
		if (method == MethodName.RefreshWeightVisuals)
		{
			return true;
		}
		if (method == MethodName.BeginCountRangeEdit)
		{
			return true;
		}
		if (method == MethodName.PreviewCountRange)
		{
			return true;
		}
		if (method == MethodName.CommitCountRange)
		{
			return true;
		}
		if (method == MethodName.BeginMagnificationRangeEdit)
		{
			return true;
		}
		if (method == MethodName.PreviewMagnificationRange)
		{
			return true;
		}
		if (method == MethodName.CommitMagnificationRange)
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
		if (method == MethodName.OnPacketSelected)
		{
			return true;
		}
		if (method == MethodName.CreateOverride)
		{
			return true;
		}
		if (method == MethodName.OpenOverride)
		{
			return true;
		}
		if (method == MethodName.ConfirmClearOverride)
		{
			return true;
		}
		if (method == MethodName.ClearOverride)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketSpawnEntryFromHistory)
		{
			return true;
		}
		if (method == MethodName.RefreshEntryAfterEdit)
		{
			return true;
		}
		if (method == MethodName.RefreshPacketPreview)
		{
			return true;
		}
		if (method == MethodName.HydratePacketPreview)
		{
			return true;
		}
		if (method == MethodName.SchedulePacketPreviewRetry)
		{
			return true;
		}
		if (method == MethodName.RetryPacketPreview)
		{
			return true;
		}
		if (method == MethodName.ApplyOverrideBadge)
		{
			return true;
		}
		if (method == MethodName.SetPacketPreviewMissing)
		{
			return true;
		}
		if (method == MethodName.ReleasePacketPreview)
		{
			return true;
		}
		if (method == MethodName.DisposeWorkbench)
		{
			return true;
		}
		return base.HasGodotClassMethod(in method);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool SetGodotClassPropertyValue(in godot_string_name name, in godot_variant value)
	{
		if (name == PropertyName._editingResource)
		{
			_editingResource = VariantUtils.ConvertTo<Resource>(in value);
			return true;
		}
		if (name == PropertyName._workbenchRoot)
		{
			_workbenchRoot = VariantUtils.ConvertTo<VBoxContainer>(in value);
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
		if (name == PropertyName._resourceMetaStatus)
		{
			_resourceMetaStatus = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._modeBadge)
		{
			_modeBadge = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._resourceStatus)
		{
			_resourceStatus = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._seedBankStage)
		{
			_seedBankStage = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._conveyorStage)
		{
			_conveyorStage = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._rainStage)
		{
			_rainStage = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._rainLandingRing)
		{
			_rainLandingRing = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._packetSlot)
		{
			_packetSlot = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._packetPreviewHost)
		{
			_packetPreviewHost = VariantUtils.ConvertTo<Control>(in value);
			return true;
		}
		if (name == PropertyName._missingPacketBadge)
		{
			_missingPacketBadge = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._overrideBadge)
		{
			_overrideBadge = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._stageHint)
		{
			_stageHint = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._spawnTuningPanel)
		{
			_spawnTuningPanel = VariantUtils.ConvertTo<PanelContainer>(in value);
			return true;
		}
		if (name == PropertyName._weightMeter)
		{
			_weightMeter = VariantUtils.ConvertTo<TextureProgressBar>(in value);
			return true;
		}
		if (name == PropertyName._weightSlider)
		{
			_weightSlider = VariantUtils.ConvertTo<HSlider>(in value);
			return true;
		}
		if (name == PropertyName._weightLabel)
		{
			_weightLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._countRangeControl)
		{
			_countRangeControl = VariantUtils.ConvertTo<XWPacketSpawnEntryRangeControl>(in value);
			return true;
		}
		if (name == PropertyName._magnificationRangeControl)
		{
			_magnificationRangeControl = VariantUtils.ConvertTo<XWPacketSpawnEntryRangeControl>(in value);
			return true;
		}
		if (name == PropertyName._retryPreviewButton)
		{
			_retryPreviewButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._choosePacketButton)
		{
			_choosePacketButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._createOverrideButton)
		{
			_createOverrideButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._openOverrideButton)
		{
			_openOverrideButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._clearOverrideButton)
		{
			_clearOverrideButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._clearOverrideConfirmation)
		{
			_clearOverrideConfirmation = VariantUtils.ConvertTo<ConfirmationDialog>(in value);
			return true;
		}
		if (name == PropertyName._selectorWindow)
		{
			_selectorWindow = VariantUtils.ConvertTo<XWPacketSpawnEntryPacketSelectorWindow>(in value);
			return true;
		}
		if (name == PropertyName._countEditingMinimum)
		{
			_countEditingMinimum = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._magnificationEditingMinimum)
		{
			_magnificationEditingMinimum = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._previewRequestVersion)
		{
			_previewRequestVersion = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._previewRetryCount)
		{
			_previewRetryCount = VariantUtils.ConvertTo<int>(in value);
			return true;
		}
		if (name == PropertyName._stageTween)
		{
			_stageTween = VariantUtils.ConvertTo<Tween>(in value);
			return true;
		}
		if (name == PropertyName._packetShow)
		{
			_packetShow = VariantUtils.ConvertTo<TowerDefenseInGamePacketShow>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName._editingResource)
		{
			value = VariantUtils.CreateFrom(in _editingResource);
			return true;
		}
		if (name == PropertyName._workbenchRoot)
		{
			value = VariantUtils.CreateFrom(in _workbenchRoot);
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
		if (name == PropertyName._resourceMetaStatus)
		{
			value = VariantUtils.CreateFrom(in _resourceMetaStatus);
			return true;
		}
		if (name == PropertyName._modeBadge)
		{
			value = VariantUtils.CreateFrom(in _modeBadge);
			return true;
		}
		if (name == PropertyName._resourceStatus)
		{
			value = VariantUtils.CreateFrom(in _resourceStatus);
			return true;
		}
		if (name == PropertyName._seedBankStage)
		{
			value = VariantUtils.CreateFrom(in _seedBankStage);
			return true;
		}
		if (name == PropertyName._conveyorStage)
		{
			value = VariantUtils.CreateFrom(in _conveyorStage);
			return true;
		}
		if (name == PropertyName._rainStage)
		{
			value = VariantUtils.CreateFrom(in _rainStage);
			return true;
		}
		if (name == PropertyName._rainLandingRing)
		{
			value = VariantUtils.CreateFrom(in _rainLandingRing);
			return true;
		}
		if (name == PropertyName._packetSlot)
		{
			value = VariantUtils.CreateFrom(in _packetSlot);
			return true;
		}
		if (name == PropertyName._packetPreviewHost)
		{
			value = VariantUtils.CreateFrom(in _packetPreviewHost);
			return true;
		}
		if (name == PropertyName._missingPacketBadge)
		{
			value = VariantUtils.CreateFrom(in _missingPacketBadge);
			return true;
		}
		if (name == PropertyName._overrideBadge)
		{
			value = VariantUtils.CreateFrom(in _overrideBadge);
			return true;
		}
		if (name == PropertyName._stageHint)
		{
			value = VariantUtils.CreateFrom(in _stageHint);
			return true;
		}
		if (name == PropertyName._spawnTuningPanel)
		{
			value = VariantUtils.CreateFrom(in _spawnTuningPanel);
			return true;
		}
		if (name == PropertyName._weightMeter)
		{
			value = VariantUtils.CreateFrom(in _weightMeter);
			return true;
		}
		if (name == PropertyName._weightSlider)
		{
			value = VariantUtils.CreateFrom(in _weightSlider);
			return true;
		}
		if (name == PropertyName._weightLabel)
		{
			value = VariantUtils.CreateFrom(in _weightLabel);
			return true;
		}
		if (name == PropertyName._countRangeControl)
		{
			value = VariantUtils.CreateFrom(in _countRangeControl);
			return true;
		}
		if (name == PropertyName._magnificationRangeControl)
		{
			value = VariantUtils.CreateFrom(in _magnificationRangeControl);
			return true;
		}
		if (name == PropertyName._retryPreviewButton)
		{
			value = VariantUtils.CreateFrom(in _retryPreviewButton);
			return true;
		}
		if (name == PropertyName._choosePacketButton)
		{
			value = VariantUtils.CreateFrom(in _choosePacketButton);
			return true;
		}
		if (name == PropertyName._createOverrideButton)
		{
			value = VariantUtils.CreateFrom(in _createOverrideButton);
			return true;
		}
		if (name == PropertyName._openOverrideButton)
		{
			value = VariantUtils.CreateFrom(in _openOverrideButton);
			return true;
		}
		if (name == PropertyName._clearOverrideButton)
		{
			value = VariantUtils.CreateFrom(in _clearOverrideButton);
			return true;
		}
		if (name == PropertyName._clearOverrideConfirmation)
		{
			value = VariantUtils.CreateFrom(in _clearOverrideConfirmation);
			return true;
		}
		if (name == PropertyName._selectorWindow)
		{
			value = VariantUtils.CreateFrom(in _selectorWindow);
			return true;
		}
		if (name == PropertyName._countEditingMinimum)
		{
			value = VariantUtils.CreateFrom(in _countEditingMinimum);
			return true;
		}
		if (name == PropertyName._magnificationEditingMinimum)
		{
			value = VariantUtils.CreateFrom(in _magnificationEditingMinimum);
			return true;
		}
		if (name == PropertyName._previewRequestVersion)
		{
			value = VariantUtils.CreateFrom(in _previewRequestVersion);
			return true;
		}
		if (name == PropertyName._previewRetryCount)
		{
			value = VariantUtils.CreateFrom(in _previewRetryCount);
			return true;
		}
		if (name == PropertyName._stageTween)
		{
			value = VariantUtils.CreateFrom(in _stageTween);
			return true;
		}
		if (name == PropertyName._packetShow)
		{
			value = VariantUtils.CreateFrom(in _packetShow);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingResource, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._workbenchRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceNameEdit, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._localToSceneCheck, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceMetaStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._modeBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._resourceStatus, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._seedBankStage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._conveyorStage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rainStage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._rainLandingRing, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetSlot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetPreviewHost, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._missingPacketBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._overrideBadge, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._spawnTuningPanel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightMeter, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightSlider, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._weightLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._countRangeControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._magnificationRangeControl, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._retryPreviewButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._choosePacketButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._createOverrideButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._openOverrideButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._clearOverrideButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._clearOverrideConfirmation, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._selectorWindow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._countEditingMinimum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._magnificationEditingMinimum, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._previewRequestVersion, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName._previewRetryCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._stageTween, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._packetShow, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingResource, Variant.From(in _editingResource));
		info.AddProperty(PropertyName._workbenchRoot, Variant.From(in _workbenchRoot));
		info.AddProperty(PropertyName._resourceNameEdit, Variant.From(in _resourceNameEdit));
		info.AddProperty(PropertyName._localToSceneCheck, Variant.From(in _localToSceneCheck));
		info.AddProperty(PropertyName._resourceMetaStatus, Variant.From(in _resourceMetaStatus));
		info.AddProperty(PropertyName._modeBadge, Variant.From(in _modeBadge));
		info.AddProperty(PropertyName._resourceStatus, Variant.From(in _resourceStatus));
		info.AddProperty(PropertyName._seedBankStage, Variant.From(in _seedBankStage));
		info.AddProperty(PropertyName._conveyorStage, Variant.From(in _conveyorStage));
		info.AddProperty(PropertyName._rainStage, Variant.From(in _rainStage));
		info.AddProperty(PropertyName._rainLandingRing, Variant.From(in _rainLandingRing));
		info.AddProperty(PropertyName._packetSlot, Variant.From(in _packetSlot));
		info.AddProperty(PropertyName._packetPreviewHost, Variant.From(in _packetPreviewHost));
		info.AddProperty(PropertyName._missingPacketBadge, Variant.From(in _missingPacketBadge));
		info.AddProperty(PropertyName._overrideBadge, Variant.From(in _overrideBadge));
		info.AddProperty(PropertyName._stageHint, Variant.From(in _stageHint));
		info.AddProperty(PropertyName._spawnTuningPanel, Variant.From(in _spawnTuningPanel));
		info.AddProperty(PropertyName._weightMeter, Variant.From(in _weightMeter));
		info.AddProperty(PropertyName._weightSlider, Variant.From(in _weightSlider));
		info.AddProperty(PropertyName._weightLabel, Variant.From(in _weightLabel));
		info.AddProperty(PropertyName._countRangeControl, Variant.From(in _countRangeControl));
		info.AddProperty(PropertyName._magnificationRangeControl, Variant.From(in _magnificationRangeControl));
		info.AddProperty(PropertyName._retryPreviewButton, Variant.From(in _retryPreviewButton));
		info.AddProperty(PropertyName._choosePacketButton, Variant.From(in _choosePacketButton));
		info.AddProperty(PropertyName._createOverrideButton, Variant.From(in _createOverrideButton));
		info.AddProperty(PropertyName._openOverrideButton, Variant.From(in _openOverrideButton));
		info.AddProperty(PropertyName._clearOverrideButton, Variant.From(in _clearOverrideButton));
		info.AddProperty(PropertyName._clearOverrideConfirmation, Variant.From(in _clearOverrideConfirmation));
		info.AddProperty(PropertyName._selectorWindow, Variant.From(in _selectorWindow));
		info.AddProperty(PropertyName._countEditingMinimum, Variant.From(in _countEditingMinimum));
		info.AddProperty(PropertyName._magnificationEditingMinimum, Variant.From(in _magnificationEditingMinimum));
		info.AddProperty(PropertyName._previewRequestVersion, Variant.From(in _previewRequestVersion));
		info.AddProperty(PropertyName._previewRetryCount, Variant.From(in _previewRetryCount));
		info.AddProperty(PropertyName._stageTween, Variant.From(in _stageTween));
		info.AddProperty(PropertyName._packetShow, Variant.From(in _packetShow));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingResource, out var value))
		{
			_editingResource = value.As<Resource>();
		}
		if (info.TryGetProperty(PropertyName._workbenchRoot, out var value2))
		{
			_workbenchRoot = value2.As<VBoxContainer>();
		}
		if (info.TryGetProperty(PropertyName._resourceNameEdit, out var value3))
		{
			_resourceNameEdit = value3.As<LineEdit>();
		}
		if (info.TryGetProperty(PropertyName._localToSceneCheck, out var value4))
		{
			_localToSceneCheck = value4.As<CheckButton>();
		}
		if (info.TryGetProperty(PropertyName._resourceMetaStatus, out var value5))
		{
			_resourceMetaStatus = value5.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._modeBadge, out var value6))
		{
			_modeBadge = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._resourceStatus, out var value7))
		{
			_resourceStatus = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._seedBankStage, out var value8))
		{
			_seedBankStage = value8.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._conveyorStage, out var value9))
		{
			_conveyorStage = value9.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._rainStage, out var value10))
		{
			_rainStage = value10.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._rainLandingRing, out var value11))
		{
			_rainLandingRing = value11.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._packetSlot, out var value12))
		{
			_packetSlot = value12.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._packetPreviewHost, out var value13))
		{
			_packetPreviewHost = value13.As<Control>();
		}
		if (info.TryGetProperty(PropertyName._missingPacketBadge, out var value14))
		{
			_missingPacketBadge = value14.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._overrideBadge, out var value15))
		{
			_overrideBadge = value15.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._stageHint, out var value16))
		{
			_stageHint = value16.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._spawnTuningPanel, out var value17))
		{
			_spawnTuningPanel = value17.As<PanelContainer>();
		}
		if (info.TryGetProperty(PropertyName._weightMeter, out var value18))
		{
			_weightMeter = value18.As<TextureProgressBar>();
		}
		if (info.TryGetProperty(PropertyName._weightSlider, out var value19))
		{
			_weightSlider = value19.As<HSlider>();
		}
		if (info.TryGetProperty(PropertyName._weightLabel, out var value20))
		{
			_weightLabel = value20.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._countRangeControl, out var value21))
		{
			_countRangeControl = value21.As<XWPacketSpawnEntryRangeControl>();
		}
		if (info.TryGetProperty(PropertyName._magnificationRangeControl, out var value22))
		{
			_magnificationRangeControl = value22.As<XWPacketSpawnEntryRangeControl>();
		}
		if (info.TryGetProperty(PropertyName._retryPreviewButton, out var value23))
		{
			_retryPreviewButton = value23.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._choosePacketButton, out var value24))
		{
			_choosePacketButton = value24.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._createOverrideButton, out var value25))
		{
			_createOverrideButton = value25.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._openOverrideButton, out var value26))
		{
			_openOverrideButton = value26.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._clearOverrideButton, out var value27))
		{
			_clearOverrideButton = value27.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._clearOverrideConfirmation, out var value28))
		{
			_clearOverrideConfirmation = value28.As<ConfirmationDialog>();
		}
		if (info.TryGetProperty(PropertyName._selectorWindow, out var value29))
		{
			_selectorWindow = value29.As<XWPacketSpawnEntryPacketSelectorWindow>();
		}
		if (info.TryGetProperty(PropertyName._countEditingMinimum, out var value30))
		{
			_countEditingMinimum = value30.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._magnificationEditingMinimum, out var value31))
		{
			_magnificationEditingMinimum = value31.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._previewRequestVersion, out var value32))
		{
			_previewRequestVersion = value32.As<int>();
		}
		if (info.TryGetProperty(PropertyName._previewRetryCount, out var value33))
		{
			_previewRetryCount = value33.As<int>();
		}
		if (info.TryGetProperty(PropertyName._stageTween, out var value34))
		{
			_stageTween = value34.As<Tween>();
		}
		if (info.TryGetProperty(PropertyName._packetShow, out var value35))
		{
			_packetShow = value35.As<TowerDefenseInGamePacketShow>();
		}
	}
}
