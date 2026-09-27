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

namespace PVZHE.ModEditor.ResourceEditors.GUI;

[ScriptPath("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCollectableVisualResourceEditor.cs")]
public class XWCollectableVisualResourceEditor : XWGenericVisualResourceEditor
{
	private enum CollectableKind
	{
		Shovel,
		AwardNote,
		Sun,
		Coin,
		Custom
	}

	public new class MethodName : XWGenericVisualResourceEditor.MethodName
	{
		public new static readonly StringName _Ready = "_Ready";

		public new static readonly StringName _ExitTree = "_ExitTree";

		public new static readonly StringName OnVisualEditorVisibilityChanged = "OnVisualEditorVisibilityChanged";

		public static readonly StringName DisposeCollectableBindings = "DisposeCollectableBindings";

		public static readonly StringName HasCompleteCollectableVisualCoverage = "HasCompleteCollectableVisualCoverage";

		public static readonly StringName RenderAwardSettlementPreview = "RenderAwardSettlementPreview";

		public static readonly StringName OnAwardSettlementPropertyEdited = "OnAwardSettlementPropertyEdited";

		public static readonly StringName SetAwardSettlementTexture = "SetAwardSettlementTexture";

		public static readonly StringName RefreshAwardSettlementPreviewFromHistory = "RefreshAwardSettlementPreviewFromHistory";

		public static readonly StringName RenderCollectableEditor = "RenderCollectableEditor";

		public static readonly StringName BindCollectableTopLevelFields = "BindCollectableTopLevelFields";

		public static readonly StringName MountCollectableConfigPicker = "MountCollectableConfigPicker";

		public static readonly StringName BindCollectableCommonConfigFields = "BindCollectableCommonConfigFields";

		public static readonly StringName OpenCollectableConfigResource = "OpenCollectableConfigResource";

		public static readonly StringName OnCollectableTopLevelPropertyEdited = "OnCollectableTopLevelPropertyEdited";

		public static readonly StringName SetCollectableConfig = "SetCollectableConfig";

		public static readonly StringName RefreshCollectableEditorFromHistory = "RefreshCollectableEditorFromHistory";

		public static readonly StringName BindCollectablePreview = "BindCollectablePreview";

		public static readonly StringName BindCollectableRuntimePreview = "BindCollectableRuntimePreview";

		public static readonly StringName RebuildCollectableRuntimePreview = "RebuildCollectableRuntimePreview";

		public static readonly StringName AddCollectableTextureLayer = "AddCollectableTextureLayer";

		public static readonly StringName SimulateCollectablePickup = "SimulateCollectablePickup";

		public static readonly StringName ResolveCollectablePoolScene = "ResolveCollectablePoolScene";

		public static readonly StringName SetCollectableRuntimeRunning = "SetCollectableRuntimeRunning";

		public static readonly StringName UpdateCollectableRuntimeStatus = "UpdateCollectableRuntimeStatus";

		public static readonly StringName BindCollectableKindPanel = "BindCollectableKindPanel";

		public static readonly StringName BuildCollectableKindCards = "BuildCollectableKindCards";

		public static readonly StringName SetCollectableKind = "SetCollectableKind";

		public static readonly StringName EnsureCollectableConfigForKind = "EnsureCollectableConfigForKind";

		public static readonly StringName CreateAwardSettlementConfig = "CreateAwardSettlementConfig";

		public static readonly StringName CreateShovelCollectableConfig = "CreateShovelCollectableConfig";

		public static readonly StringName RebuildCollectableEditor = "RebuildCollectableEditor";

		public static readonly StringName CreateCollectableConfigPanel = "CreateCollectableConfigPanel";

		public static readonly StringName CreateAwardPreviewPanel = "CreateAwardPreviewPanel";

		public static readonly StringName CreateShovelPreviewPanel = "CreateShovelPreviewPanel";

		public static readonly StringName CreateShovelListPanel = "CreateShovelListPanel";

		public static readonly StringName CreateShovelableNamePanel = "CreateShovelableNamePanel";

		public static readonly StringName SetConfigProperty = "SetConfigProperty";

		public static readonly StringName AddShovelableName = "AddShovelableName";

		public static readonly StringName RemoveSelectedShovelableName = "RemoveSelectedShovelableName";

		public static readonly StringName SaveCollectableResource = "SaveCollectableResource";

		public static readonly StringName ScheduleCollectableSave = "ScheduleCollectableSave";

		public static readonly StringName SavePendingCollectable = "SavePendingCollectable";

		public static readonly StringName UpdateCollectablePreview = "UpdateCollectablePreview";

		public static readonly StringName RefreshKindPanel = "RefreshKindPanel";

		public static readonly StringName RefreshConfigPanel = "RefreshConfigPanel";

		public static readonly StringName RefreshShovelListPanel = "RefreshShovelListPanel";

		public static readonly StringName AddSummaryRows = "AddSummaryRows";

		public static readonly StringName CreateSectionTitle = "CreateSectionTitle";

		public static readonly StringName WrapField = "WrapField";

		public static readonly StringName CreateArraySummaryList = "CreateArraySummaryList";

		public static readonly StringName WrapList = "WrapList";

		public static readonly StringName SelectCollectableKindCard = "SelectCollectableKindCard";

		public static readonly StringName TryReadTexture = "TryReadTexture";

		public static readonly StringName ResolveCollectableKind = "ResolveCollectableKind";

		public static readonly StringName GetCollectableKindLabel = "GetCollectableKindLabel";

		public static readonly StringName BuildCollectableKindSummary = "BuildCollectableKindSummary";

		public static readonly StringName BuildDefaultConfigName = "BuildDefaultConfigName";

		public static readonly StringName ReadResource = "ReadResource";

		public static readonly StringName ReadKnown = "ReadKnown";

		public static readonly StringName ReadString = "ReadString";

		public static readonly StringName ReadDouble = "ReadDouble";

		public static readonly StringName ReadVariant = "ReadVariant";

		public static readonly StringName HasResourceProperty = "HasResourceProperty";

		public static readonly StringName BuildValueText = "BuildValueText";

		public static readonly StringName AddItemIfMissing = "AddItemIfMissing";

		public static readonly StringName FormatVariant = "FormatVariant";

		public static readonly StringName FormatResource = "FormatResource";

		public static readonly StringName EmptyToPlaceholder = "EmptyToPlaceholder";
	}

	public new class PropertyName : XWGenericVisualResourceEditor.PropertyName
	{
		public static readonly StringName CollectableKindVisualCardCount = "CollectableKindVisualCardCount";

		public static readonly StringName IsCollectablePreviewRendering = "IsCollectablePreviewRendering";

		public static readonly StringName _editingCollectable = "_editingCollectable";

		public static readonly StringName _editingAwardSettlement = "_editingAwardSettlement";

		public static readonly StringName _awardBackgroundPreview = "_awardBackgroundPreview";

		public static readonly StringName _awardImagePreview = "_awardImagePreview";

		public static readonly StringName _awardTexturePreview = "_awardTexturePreview";

		public static readonly StringName _awardEmptyHint = "_awardEmptyHint";

		public static readonly StringName _awardStatusLabel = "_awardStatusLabel";

		public static readonly StringName _awardTitleLabel = "_awardTitleLabel";

		public static readonly StringName _previewTitleLabel = "_previewTitleLabel";

		public static readonly StringName _previewConfigLabel = "_previewConfigLabel";

		public static readonly StringName _previewValueLabel = "_previewValueLabel";

		public static readonly StringName _previewAudioLabel = "_previewAudioLabel";

		public static readonly StringName _previewSceneLabel = "_previewSceneLabel";

		public static readonly StringName _kindSummaryLabel = "_kindSummaryLabel";

		public static readonly StringName _texturePreview = "_texturePreview";

		public static readonly StringName _collectablePreviewViewport = "_collectablePreviewViewport";

		public static readonly StringName _collectablePreviewRoot = "_collectablePreviewRoot";

		public static readonly StringName _collectableRuntimeStatusLabel = "_collectableRuntimeStatusLabel";

		public static readonly StringName _collectableRuntimeRunButton = "_collectableRuntimeRunButton";

		public static readonly StringName _collectableRuntimeRunning = "_collectableRuntimeRunning";

		public static readonly StringName _collectableKindCards = "_collectableKindCards";

		public static readonly StringName _configPropertyList = "_configPropertyList";

		public static readonly StringName _unlockConditionList = "_unlockConditionList";

		public static readonly StringName _eventList = "_eventList";

		public static readonly StringName _shovelableNameList = "_shovelableNameList";

		public static readonly StringName _updatingControls = "_updatingControls";

		public static readonly StringName _collectableSaveTimer = "_collectableSaveTimer";

		public static readonly StringName _pendingCollectableSaveMessage = "_pendingCollectableSaveMessage";
	}

	public new class SignalName : XWGenericVisualResourceEditor.SignalName
	{
	}

	private const string AwardSettlementPreviewScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAwardSettlementVisualPreview.tscn";

	private const string VisualEditorLayoutScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCollectableVisualEditorLayout.tscn";

	private const string VisualChoiceCardScenePath = "res://addons/ModEditor/ResourceEditors/GUI/Panels/XWGameVisualChoiceCard.tscn";

	private static PackedScene _awardSettlementPreviewScene;

	private static PackedScene _visualEditorLayoutScene;

	private static PackedScene _visualChoiceCardScene;

	private CollectableConfig _editingCollectable;

	private AwardSettlementConfig _editingAwardSettlement;

	private XWVisualPropertyBinding _collectablePropertyBinding;

	private XWVisualPropertyBinding _awardSettlementPropertyBinding;

	private TextureRect _awardBackgroundPreview;

	private TextureRect _awardImagePreview;

	private TextureRect _awardTexturePreview;

	private Label _awardEmptyHint;

	private Label _awardStatusLabel;

	private Label _awardTitleLabel;

	private Label _previewTitleLabel;

	private Label _previewConfigLabel;

	private Label _previewValueLabel;

	private Label _previewAudioLabel;

	private Label _previewSceneLabel;

	private Label _kindSummaryLabel;

	private TextureRect _texturePreview;

	private SubViewport _collectablePreviewViewport;

	private Node2D _collectablePreviewRoot;

	private Label _collectableRuntimeStatusLabel;

	private Button _collectableRuntimeRunButton;

	private bool _collectableRuntimeRunning;

	private HFlowContainer _collectableKindCards;

	private ItemList _configPropertyList;

	private ItemList _unlockConditionList;

	private ItemList _eventList;

	private ItemList _shovelableNameList;

	private bool _updatingControls;

	private Timer _collectableSaveTimer;

	private string _pendingCollectableSaveMessage;

	public int CollectableKindVisualCardCount
	{
		get
		{
			if (!GodotObject.IsInstanceValid(_collectableKindCards))
			{
				return 0;
			}
			return _collectableKindCards.GetChildCount();
		}
	}

	public bool IsCollectablePreviewRendering
	{
		get
		{
			if (GodotObject.IsInstanceValid(_collectablePreviewViewport))
			{
				return _collectablePreviewViewport.RenderTargetUpdateMode != SubViewport.UpdateMode.Disabled;
			}
			return false;
		}
	}

	public override void _Ready()
	{
		base._Ready();
		_collectableSaveTimer = new Timer
		{
			Name = "CollectableSaveDebounce",
			OneShot = true,
			WaitTime = 0.18
		};
		_collectableSaveTimer.Timeout += SavePendingCollectable;
		AddChild(_collectableSaveTimer, forceReadableName: false, InternalMode.Disabled);
	}

	public override void _ExitTree()
	{
		SavePendingCollectable();
		DisposeCollectableBindings();
		if (GodotObject.IsInstanceValid(_collectableSaveTimer))
		{
			_collectableSaveTimer.QueueFree();
		}
		base._ExitTree();
	}

	protected override void OnVisualEditorVisibilityChanged(bool visible)
	{
		base.OnVisualEditorVisibilityChanged(visible);
		if (GodotObject.IsInstanceValid(_collectablePreviewViewport))
		{
			_collectablePreviewViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode)(visible ? 4 : 0);
		}
	}

	protected override void RenderCustomVisualPreset(XWVisualEditorPreset preset)
	{
		DisposeCollectableBindings();
		Resource currentResource = CurrentResource;
		if (!(currentResource is CollectableConfig collectableConfig))
		{
			if (currentResource is AwardSettlementConfig awardSettlementConfig)
			{
				_editingAwardSettlement = awardSettlementConfig;
				RenderAwardSettlementPreview(awardSettlementConfig);
			}
		}
		else
		{
			_editingCollectable = collectableConfig;
			RenderCollectableEditor(collectableConfig);
		}
	}

	private void DisposeCollectableBindings()
	{
		_collectablePropertyBinding?.Dispose();
		_collectablePropertyBinding = null;
		_awardSettlementPropertyBinding?.Dispose();
		_awardSettlementPropertyBinding = null;
		_editingCollectable = null;
		_editingAwardSettlement = null;
		_collectableRuntimeRunning = false;
	}

	protected override bool ShouldUpdateEmbeddedInspector(Resource resource, string path, XWVisualEditorDescriptor descriptor)
	{
		if (!HasCompleteCollectableVisualCoverage(resource))
		{
			return base.ShouldUpdateEmbeddedInspector(resource, path, descriptor);
		}
		return false;
	}

	private static bool HasCompleteCollectableVisualCoverage(Resource resource)
	{
		Type type = resource?.GetType();
		if (!(type == typeof(CollectableConfig)))
		{
			return type == typeof(AwardSettlementConfig);
		}
		return true;
	}

	private void RenderAwardSettlementPreview(AwardSettlementConfig award)
	{
		if (CanvasGrid == null || !GodotObject.IsInstanceValid(award))
		{
			return;
		}
		if (_awardSettlementPreviewScene == null)
		{
			_awardSettlementPreviewScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWAwardSettlementVisualPreview.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		Control control = _awardSettlementPreviewScene?.Instantiate<Control>(PackedScene.GenEditState.Disabled);
		if (GodotObject.IsInstanceValid(control))
		{
			CanvasGrid.Columns = 1;
			CanvasGrid.AddChild(control, forceReadableName: false, InternalMode.Disabled);
			_awardBackgroundPreview = control.GetNode<TextureRect>("Layout/Preview/Background");
			_awardImagePreview = control.GetNode<TextureRect>("Layout/Preview/Image");
			_awardTexturePreview = control.GetNode<TextureRect>("Layout/Preview/PickupIconFrame/PickupIcon");
			_awardEmptyHint = control.GetNode<Label>("Layout/Preview/EmptyHint");
			_awardStatusLabel = control.GetNode<Label>("Layout/Header/Status");
			_awardTitleLabel = control.GetNode<Label>("Layout/Header/Title");
			_awardSettlementPropertyBinding?.Dispose();
			_awardSettlementPropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnAwardSettlementPropertyEdited);
			_awardSettlementPropertyBinding.BindText(control.GetNode<LineEdit>("%ResourceNameEdit"), award, "resource_name", RefreshAwardSettlementPreviewFromHistory);
			_awardSettlementPropertyBinding.BindToggle(control.GetNode<CheckButton>("%LocalToSceneCheck"), award, "resource_local_to_scene", RefreshAwardSettlementPreviewFromHistory);
			MountAwardSettlementPicker(control.GetNode<HBoxContainer>("%BackgroundPickerHost"), "BackgroundPicker", award.background, (Resource resource) =>
			{
				SetAwardSettlementTexture("background", resource, "更换奖励结算背景");
			});
			MountAwardSettlementPicker(control.GetNode<HBoxContainer>("%ImagePickerHost"), "ImagePicker", award.image, (Resource resource) =>
			{
				SetAwardSettlementTexture("image", resource, "更换奖励展示图");
			});
			MountAwardSettlementPicker(control.GetNode<HBoxContainer>("%TexturePickerHost"), "TexturePicker", award.texture, (Resource resource) =>
			{
				SetAwardSettlementTexture("texture", resource, "更换奖励拾取图标");
			});
			RefreshAwardSettlementPreviewFromHistory();
			PreviewList?.AddItem("直接在结算画面下方更换背景、展示图和拾取图标；修改会立即显示并支持撤销。");
		}
	}

	private void OnAwardSettlementPropertyEdited(bool committed)
	{
		if (CurrentResource is AwardSettlementConfig awardSettlementConfig && awardSettlementConfig == _editingAwardSettlement)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
				return;
			}
			MarkCurrentResourceDirty();
			awardSettlementConfig.EmitChanged();
		}
	}

	private void MountAwardSettlementPicker(HBoxContainer host, string name, Texture2D value, Action<Resource> changed)
	{
		if (GodotObject.IsInstanceValid(host))
		{
			XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
			xWResourcePicker.Name = name;
			xWResourcePicker.Setup("Texture2D");
			xWResourcePicker.SetEditedResource(value);
			xWResourcePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			xWResourcePicker.ResourceChanged += (Resource resource) =>
			{
				changed?.Invoke(resource);
			};
			host.AddChild(xWResourcePicker, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void SetAwardSettlementTexture(StringName property, Resource resource, string actionName)
	{
		if (_awardSettlementPropertyBinding != null && CurrentResource is AwardSettlementConfig awardSettlementConfig && awardSettlementConfig == _editingAwardSettlement)
		{
			_awardSettlementPropertyBinding.SetValue(awardSettlementConfig, property, resource as Texture2D, actionName, this, "RefreshAwardSettlementPreviewFromHistory");
			RefreshAwardSettlementPreviewFromHistory();
		}
	}

	public void RefreshAwardSettlementPreviewFromHistory()
	{
		if (CurrentResource is AwardSettlementConfig awardSettlementConfig && awardSettlementConfig == _editingAwardSettlement)
		{
			if (GodotObject.IsInstanceValid(_awardBackgroundPreview))
			{
				_awardBackgroundPreview.Texture = awardSettlementConfig.background;
			}
			if (GodotObject.IsInstanceValid(_awardImagePreview))
			{
				_awardImagePreview.Texture = awardSettlementConfig.image;
			}
			if (GodotObject.IsInstanceValid(_awardTexturePreview))
			{
				_awardTexturePreview.Texture = awardSettlementConfig.texture;
			}
			bool visible = !GodotObject.IsInstanceValid(awardSettlementConfig.background) && !GodotObject.IsInstanceValid(awardSettlementConfig.image) && !GodotObject.IsInstanceValid(awardSettlementConfig.texture);
			if (GodotObject.IsInstanceValid(_awardEmptyHint))
			{
				_awardEmptyHint.Visible = visible;
			}
			if (GodotObject.IsInstanceValid(_awardStatusLabel))
			{
				_awardStatusLabel.Text = $"背景 {(GodotObject.IsInstanceValid(awardSettlementConfig.background) ? "✓" : "—")} · 展示图 {(GodotObject.IsInstanceValid(awardSettlementConfig.image) ? "✓" : "—")} · 图标 {(GodotObject.IsInstanceValid(awardSettlementConfig.texture) ? "✓" : "—")}";
			}
			if (GodotObject.IsInstanceValid(_awardTitleLabel))
			{
				_awardTitleLabel.Text = (string.IsNullOrWhiteSpace(awardSettlementConfig.ResourceName) ? "奖励结算窗口预览" : (awardSettlementConfig.ResourceName + " · 结算窗口"));
			}
		}
	}

	private void RenderCollectableEditor(CollectableConfig collectable)
	{
		if (CanvasGrid != null)
		{
			CanvasGrid.Columns = 1;
			_collectablePropertyBinding?.Dispose();
			if (_visualEditorLayoutScene == null)
			{
				_visualEditorLayoutScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWCollectableVisualEditorLayout.tscn", null, ResourceLoader.CacheMode.Reuse);
			}
			VBoxContainer vBoxContainer = _visualEditorLayoutScene?.Instantiate<VBoxContainer>(PackedScene.GenEditState.Disabled);
			if (GodotObject.IsInstanceValid(vBoxContainer))
			{
				CanvasGrid.AddChild(vBoxContainer, forceReadableName: false, InternalMode.Disabled);
				BindCollectablePreview(vBoxContainer);
				BindCollectableKindPanel(vBoxContainer);
				BindCollectableTopLevelFields(vBoxContainer, collectable);
				vBoxContainer.GetNode<VBoxContainer>("%ConfigHost").AddChild(CreateCollectableConfigPanel(collectable), forceReadableName: false, InternalMode.Disabled);
				AddSummaryRows(collectable);
			}
		}
	}

	private void BindCollectableTopLevelFields(VBoxContainer root, CollectableConfig collectable)
	{
		_collectablePropertyBinding = new XWVisualPropertyBinding(XWEditorInterface.Instance?.GetUndoRedoManager(), OnCollectableTopLevelPropertyEdited);
		_collectablePropertyBinding.BindText(root.GetNode<LineEdit>("%ResourceNameEdit"), collectable, "resource_name", UpdateCollectablePreview, this, "RefreshCollectableEditorFromHistory");
		_collectablePropertyBinding.BindText(root.GetNode<LineEdit>("%SaveKeyLineEdit"), collectable, "saveKey", UpdateCollectablePreview, this, "RefreshCollectableEditorFromHistory");
		_collectablePropertyBinding.BindToggle(root.GetNode<CheckButton>("%LocalToSceneCheck"), collectable, "resource_local_to_scene", UpdateCollectablePreview, this, "RefreshCollectableEditorFromHistory");
		MountCollectableConfigPicker(root.GetNode<HBoxContainer>("%ConfigPickerHost"), collectable);
		BindCollectableCommonConfigFields(root.GetNode<VBoxContainer>("%CommonConfigHost"), collectable);
	}

	private void MountCollectableConfigPicker(HBoxContainer host, CollectableConfig collectable)
	{
		if (GodotObject.IsInstanceValid(host))
		{
			XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
			xWResourcePicker.Name = "ConfigPicker";
			xWResourcePicker.Setup("Resource");
			xWResourcePicker.SetEditedResource(collectable.config);
			xWResourcePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
			xWResourcePicker.ResourceChanged += (Resource resource) =>
			{
				SetCollectableConfig(resource, "更换收集物子配置");
			};
			xWResourcePicker.ResourceSelected += OpenCollectableConfigResource;
			host.AddChild(xWResourcePicker, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void BindCollectableCommonConfigFields(VBoxContainer host, CollectableConfig collectable)
	{
		if (GodotObject.IsInstanceValid(host))
		{
			host.AddChild(AddResourcePicker("ScenePicker", "游戏对象场景", "PackedScene", ReadResource(collectable.config, "scene"), (Resource resource) =>
			{
				SetConfigProperty("scene", Variant.From(in resource), refreshPreview: true);
			}), forceReadableName: false, InternalMode.Disabled);
			host.AddChild(AddResourcePicker("TexturePicker", "画面图标", "Texture2D", ReadResource(collectable.config, "texture", "icon", "previewTexture"), (Resource resource) =>
			{
				SetConfigProperty("texture", Variant.From(in resource), refreshPreview: true);
			}), forceReadableName: false, InternalMode.Disabled);
			host.AddChild(AddLineEdit("AudioLineEdit", "拾取音效", ReadString(collectable.config, "audio"), (string text) =>
			{
				SetConfigProperty("audio", Variant.From(in text), refreshPreview: false);
			}), forceReadableName: false, InternalMode.Disabled);
			host.AddChild(AddSpinBox("ValueSpinBox", "数值", ReadDouble(collectable.config, "value", 0.0), -999999.0, 999999.0, 1.0, (double value) =>
			{
				SetConfigProperty("value", Variant.From(in value), refreshPreview: false);
			}), forceReadableName: false, InternalMode.Disabled);
			host.AddChild(AddSpinBox("AmountSpinBox", "数量", ReadDouble(collectable.config, "amount", 0.0), -999999.0, 999999.0, 1.0, (double value) =>
			{
				SetConfigProperty("amount", Variant.From(in value), refreshPreview: false);
			}), forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void OpenCollectableConfigResource(Resource resource)
	{
		if (GodotObject.IsInstanceValid(resource) && _editingCollectable != null)
		{
			XWResourceEditContext context = XWResourceEditContext.ForProperty(resource, _editingCollectable, resource.ResourcePath, CurrentResourcePath, "config", -1, "collectable_editor", CurrentEditContext?.IsBuiltInSource ?? false);
			XWEditorInterface.Instance?.EditResource(resource, context);
		}
	}

	private void OnCollectableTopLevelPropertyEdited(bool committed)
	{
		if (CurrentResource is CollectableConfig collectableConfig && collectableConfig == _editingCollectable)
		{
			if (committed)
			{
				NotifyCurrentResourceEdited();
				ScheduleCollectableSave(collectableConfig);
			}
			else
			{
				MarkCurrentResourceDirty();
				collectableConfig.EmitChanged();
			}
		}
	}

	private void SetCollectableConfig(Resource config, string actionName)
	{
		if (_collectablePropertyBinding != null && CurrentResource is CollectableConfig collectableConfig && collectableConfig == _editingCollectable)
		{
			_collectablePropertyBinding.SetValue(collectableConfig, "config", config, actionName, this, "RefreshCollectableEditorFromHistory");
			RebuildCollectableEditor();
		}
	}

	public void RefreshCollectableEditorFromHistory()
	{
		XWUndoRedoManager xWUndoRedoManager = XWEditorInterface.Instance?.GetUndoRedoManager();
		if (xWUndoRedoManager != null && (xWUndoRedoManager.IsUndoing() || xWUndoRedoManager.IsRedoing()) && CurrentResource is CollectableConfig editingCollectable)
		{
			_editingCollectable = editingCollectable;
			RebuildCollectableEditor();
		}
	}

	private void BindCollectablePreview(VBoxContainer root)
	{
		_texturePreview = root.GetNode<TextureRect>("%TexturePreview");
		_previewTitleLabel = root.GetNode<Label>("%PreviewTitleLabel");
		_previewConfigLabel = root.GetNode<Label>("%PreviewConfigLabel");
		_previewValueLabel = root.GetNode<Label>("%PreviewValueLabel");
		_previewAudioLabel = root.GetNode<Label>("%PreviewAudioLabel");
		_previewSceneLabel = root.GetNode<Label>("%PreviewSceneLabel");
		BindCollectableRuntimePreview(root.GetNode<Control>("%RuntimePreview"));
		UpdateCollectablePreview();
	}

	private void BindCollectableRuntimePreview(Control frame)
	{
		_collectableRuntimeStatusLabel = frame.GetNode<Label>("Layout/Toolbar/Status");
		_collectableRuntimeRunButton = frame.GetNode<Button>("Layout/Toolbar/RunButton");
		_collectablePreviewViewport = frame.GetNode<SubViewport>("Layout/ViewportContainer/Viewport");
		_collectablePreviewViewport.RenderTargetUpdateMode = (SubViewport.UpdateMode)(IsVisibleInTree() ? 4 : 0);
		_collectablePreviewRoot = frame.GetNode<Node2D>("Layout/ViewportContainer/Viewport/PreviewRoot");
		frame.GetNode<Button>("Layout/Toolbar/PickupButton").Pressed += SimulateCollectablePickup;
		frame.GetNode<Button>("Layout/Toolbar/ReloadButton").Pressed += RebuildCollectableRuntimePreview;
		_collectableRuntimeRunButton.Pressed += () =>
		{
			SetCollectableRuntimeRunning(!_collectableRuntimeRunning);
		};
		RebuildCollectableRuntimePreview();
	}

	private void RebuildCollectableRuntimePreview()
	{
		if (!GodotObject.IsInstanceValid(_collectablePreviewRoot) || _editingCollectable == null)
		{
			return;
		}
		foreach (Node child in _collectablePreviewRoot.GetChildren())
		{
			child.QueueFree();
		}
		Resource config = _editingCollectable.config;
		PackedScene packedScene = ReadResource(config, "scene", "collectableScene", "packedScene") as PackedScene;
		if (packedScene == null)
		{
			packedScene = ResolveCollectablePoolScene(config);
		}
		Node node = null;
		if (GodotObject.IsInstanceValid(packedScene))
		{
			try
			{
				node = packedScene.Instantiate(PackedScene.GenEditState.Disabled);
				node.ProcessMode = (ProcessModeEnum)(_collectableRuntimeRunning ? 0 : 4);
			}
			catch (Exception ex)
			{
				GD.PushWarning("Collectable editor scene preview failed: " + ex.Message);
			}
		}
		if (node != null)
		{
			node.Name = "ConfiguredCollectableScenePreview";
			_collectablePreviewRoot.AddChild(node, forceReadableName: false, InternalMode.Disabled);
		}
		else if (config is AwardSettlementConfig awardSettlementConfig)
		{
			AddCollectableTextureLayer(awardSettlementConfig.background, "AwardBackground", 330f, 160f, Vector2.Zero);
			AddCollectableTextureLayer(awardSettlementConfig.image, "AwardImage", 190f, 125f, Vector2.Zero);
			AddCollectableTextureLayer(awardSettlementConfig.texture, "AwardPickupIcon", 58f, 58f, new Vector2(-128f, 52f));
		}
		else
		{
			Texture2D texture = TryReadTexture(config, "texture", "icon", "previewTexture", "image");
			AddCollectableTextureLayer(texture, "CollectableTextureFallback", 150f, 140f, Vector2.Zero);
		}
		SetCollectableRuntimeRunning(_collectableRuntimeRunning);
		UpdateCollectableRuntimeStatus();
	}

	private void AddCollectableTextureLayer(Texture2D texture, string nodeName, float maxWidth, float maxHeight, Vector2 offset)
	{
		if (GodotObject.IsInstanceValid(texture) && GodotObject.IsInstanceValid(_collectablePreviewRoot))
		{
			Vector2 size = texture.GetSize();
			float a = ((size.X > 0f && size.Y > 0f) ? Mathf.Min(maxWidth / size.X, maxHeight / size.Y) : 1f);
			_collectablePreviewRoot.AddChild(new Sprite2D
			{
				Name = nodeName,
				Texture = texture,
				Position = offset,
				Scale = Vector2.One * Mathf.Min(a, 1f)
			}, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void SimulateCollectablePickup()
	{
		if (!GodotObject.IsInstanceValid(_collectablePreviewRoot))
		{
			return;
		}
		UpdateCollectableRuntimeStatus("拾取中");
		Tween tween = _collectablePreviewRoot.CreateTween();
		tween.SetParallel();
		tween.SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.In);
		tween.TweenProperty(_collectablePreviewRoot, "position", new Vector2(54f, 34f), 0.48);
		tween.TweenProperty(_collectablePreviewRoot, "scale", Vector2.One * 0.18f, 0.48);
		tween.SetParallel(parallel: false);
		tween.TweenCallback(Callable.From(() =>
		{
			if (GodotObject.IsInstanceValid(_collectablePreviewRoot))
			{
				_collectablePreviewRoot.Position = new Vector2(270f, 205f);
				_collectablePreviewRoot.Scale = Vector2.One;
				RebuildCollectableRuntimePreview();
				UpdateCollectableRuntimeStatus("已拾取并重生");
			}
		}));
	}

	private PackedScene ResolveCollectablePoolScene(Resource config)
	{
		return ResolveCollectableKind(config) switch
		{
			CollectableKind.Sun => ObjectManager.GetPoolScene(ObjectManagerConfig.OBJECT.SUN), 
			CollectableKind.Coin => ObjectManager.GetPoolScene(ObjectManagerConfig.OBJECT.COIN_GOLD), 
			_ => null, 
		};
	}

	private void SetCollectableRuntimeRunning(bool running)
	{
		_collectableRuntimeRunning = running;
		if (GodotObject.IsInstanceValid(_collectablePreviewRoot))
		{
			foreach (Node child in _collectablePreviewRoot.GetChildren())
			{
				child.ProcessMode = (ProcessModeEnum)(running ? 0 : 4);
			}
		}
		if (GodotObject.IsInstanceValid(_collectableRuntimeRunButton))
		{
			_collectableRuntimeRunButton.Text = (running ? "暂停场景" : "运行场景");
		}
	}

	private void UpdateCollectableRuntimeStatus(string prefix = "预览")
	{
		if (GodotObject.IsInstanceValid(_collectableRuntimeStatusLabel) && _editingCollectable != null)
		{
			Resource config = _editingCollectable.config;
			string collectableKindLabel = GetCollectableKindLabel(ResolveCollectableKind(config));
			string value = BuildValueText(config);
			string value2 = EmptyToPlaceholder(ReadString(config, "audio"));
			_collectableRuntimeStatusLabel.Text = $"{prefix}: {collectableKindLabel} · 数值 {value} · 音效 {value2}";
		}
	}

	private void BindCollectableKindPanel(VBoxContainer root)
	{
		_collectableKindCards = root.GetNode<HFlowContainer>("%KindCards");
		BuildCollectableKindCards(ResolveCollectableKind(_editingCollectable?.config));
		_kindSummaryLabel = root.GetNode<Label>("%KindSummaryLabel");
		_kindSummaryLabel.Text = BuildCollectableKindSummary(_editingCollectable?.config);
	}

	private void BuildCollectableKindCards(CollectableKind selectedKind)
	{
		if (!GodotObject.IsInstanceValid(_collectableKindCards))
		{
			return;
		}
		foreach (Node child in _collectableKindCards.GetChildren())
		{
			_collectableKindCards.RemoveChild(child);
			child.QueueFree();
		}
		(string, string, string)[] array = new (string, string, string)[5]
		{
			("铲子奖励", "关卡工具", "res://addons/ModEditor/Icons/ResourceShovel.svg"),
			("图鉴纸条", "解锁图鉴", "res://addons/ModEditor/Icons/FileThumbnail.svg"),
			("阳光奖励", "战斗资源", "res://addons/ModEditor/Icons/ResourceCollectable.svg"),
			("金币奖励", "账户货币", "res://addons/ModEditor/Icons/Favorites.svg"),
			("自定义资源", "保留 Mod 配置", "res://addons/ModEditor/Icons/AssetLib.svg")
		};
		if (_visualChoiceCardScene == null)
		{
			_visualChoiceCardScene = ResourceLoader.Load<PackedScene>("res://addons/ModEditor/ResourceEditors/GUI/Panels/XWGameVisualChoiceCard.tscn", null, ResourceLoader.CacheMode.Reuse);
		}
		for (int i = 0; i < array.Length; i++)
		{
			CollectableKind kind = (CollectableKind)i;
			XWGameVisualChoiceCard xWGameVisualChoiceCard = _visualChoiceCardScene?.Instantiate<XWGameVisualChoiceCard>(PackedScene.GenEditState.Disabled);
			if (!GodotObject.IsInstanceValid(xWGameVisualChoiceCard))
			{
				continue;
			}
			xWGameVisualChoiceCard.CustomMinimumSize = new Vector2(150f, 64f);
			xWGameVisualChoiceCard.Configure(i.ToString(), array[i].Item1, array[i].Item2, ResourceLoader.Load<Texture2D>(array[i].Item3, null, ResourceLoader.CacheMode.Reuse), kind == selectedKind);
			xWGameVisualChoiceCard.Pressed += () =>
			{
				if (!_updatingControls)
				{
					SetCollectableKind(kind);
				}
			};
			_collectableKindCards.AddChild(xWGameVisualChoiceCard, forceReadableName: false, InternalMode.Disabled);
		}
	}

	private void SetCollectableKind(CollectableKind kind)
	{
		if (_editingCollectable != null)
		{
			Resource resource = EnsureCollectableConfigForKind(kind);
			if (GodotObject.IsInstanceValid(resource))
			{
				resource.SetMeta("mod_collectable_kind", kind.ToString());
				SetCollectableConfig(resource, "更换收集物类型");
			}
		}
	}

	private Resource EnsureCollectableConfigForKind(CollectableKind kind)
	{
		Resource resource = _editingCollectable?.config;
		bool flag;
		switch (kind)
		{
		case CollectableKind.Shovel:
			if (resource is ShovelConfig)
			{
				return resource;
			}
			return CreateShovelCollectableConfig();
		case CollectableKind.AwardNote:
		case CollectableKind.Sun:
		case CollectableKind.Coin:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			if (resource is AwardSettlementConfig)
			{
				return resource;
			}
			return CreateAwardSettlementConfig(kind);
		}
		if (!GodotObject.IsInstanceValid(resource))
		{
			return new Resource
			{
				ResourceName = BuildDefaultConfigName()
			};
		}
		return resource;
	}

	private AwardSettlementConfig CreateAwardSettlementConfig(CollectableKind kind)
	{
		AwardSettlementConfig awardSettlementConfig = new AwardSettlementConfig();
		awardSettlementConfig.ResourceName = BuildDefaultConfigName();
		awardSettlementConfig.SetMeta("mod_collectable_kind", kind.ToString());
		return awardSettlementConfig;
	}

	private ShovelConfig CreateShovelCollectableConfig()
	{
		string text = EmptyToPlaceholder(_editingCollectable?.saveKey).Replace(" ", "_");
		return new ShovelConfig
		{
			ResourceName = BuildDefaultConfigName(),
			saveKey = text,
			name = text + "_NAME",
			describe = text + "_DESC",
			handbookDescribe = text + "_HANDBOOK_DESC",
			handbookStory = text + "_HANDBOOK_STORY"
		};
	}

	private void RebuildCollectableEditor()
	{
		if (CanvasGrid == null || _editingCollectable == null)
		{
			return;
		}
		foreach (Node child in CanvasGrid.GetChildren())
		{
			CanvasGrid.RemoveChild(child);
			child.QueueFree();
		}
		RenderCollectableEditor(_editingCollectable);
	}

	private Control CreateCollectableConfigPanel(CollectableConfig collectable)
	{
		VBoxContainer vBoxContainer = new VBoxContainer
		{
			Name = "CollectableConfigPanel",
			CustomMinimumSize = new Vector2(360f, 0f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		vBoxContainer.AddThemeConstantOverride("separation", 8);
		vBoxContainer.AddChild(CreateSectionTitle("配置摘要"), forceReadableName: false, InternalMode.Disabled);
		CollectableKind kind = ResolveCollectableKind(collectable?.config);
		if (collectable?.config is ShovelConfig config)
		{
			vBoxContainer.AddChild(CreateShovelPreviewPanel(config), forceReadableName: false, InternalMode.Disabled);
		}
		else if (collectable?.config is AwardSettlementConfig config2)
		{
			vBoxContainer.AddChild(CreateAwardPreviewPanel(config2, kind), forceReadableName: false, InternalMode.Disabled);
		}
		else
		{
			vBoxContainer.AddChild(new Label
			{
				Text = "选择收集物类型后会在这里显示对应的可视化配置。",
				AutowrapMode = TextServer.AutowrapMode.WordSmart,
				SizeFlagsHorizontal = SizeFlags.ExpandFill
			}, forceReadableName: false, InternalMode.Disabled);
		}
		_configPropertyList = new ItemList
		{
			Name = "CollectableConfigPropertyList",
			CustomMinimumSize = new Vector2(320f, 260f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
		vBoxContainer.AddChild(_configPropertyList, forceReadableName: false, InternalMode.Disabled);
		RefreshConfigPanel(collectable);
		return vBoxContainer;
	}

	private Control CreateAwardPreviewPanel(AwardSettlementConfig config, CollectableKind kind)
	{
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.Name = "AwardPreviewPanel";
		vBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		vBoxContainer.AddThemeConstantOverride("separation", 8);
		vBoxContainer.AddChild(CreateSectionTitle(GetCollectableKindLabel(kind) + "配置"), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(AddResourcePicker("TexturePicker", "拾取图标", "Texture2D", config.texture, (Resource resource) =>
		{
			SetConfigProperty("texture", Variant.From(in resource), refreshPreview: true);
		}), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(AddResourcePicker("BackgroundPicker", "结算背景", "Texture2D", config.background, (Resource resource) =>
		{
			SetConfigProperty("background", Variant.From(in resource), refreshPreview: true);
		}), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(AddResourcePicker("ImagePicker", "展示图片", "Texture2D", config.image, (Resource resource) =>
		{
			SetConfigProperty("image", Variant.From(in resource), refreshPreview: true);
		}), forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private Control CreateShovelPreviewPanel(ShovelConfig config)
	{
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.Name = "ShovelPreviewPanel";
		vBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		vBoxContainer.AddThemeConstantOverride("separation", 8);
		vBoxContainer.AddChild(CreateSectionTitle("铲子奖励配置"), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(AddResourcePicker("TexturePicker", "图标", "Texture2D", config.texture, (Resource resource) =>
		{
			SetConfigProperty("texture", Variant.From(in resource), refreshPreview: true);
		}), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(AddLineEdit("NameLineEdit", "名称文本 Key", config.name, (string text) =>
		{
			SetConfigProperty("name", Variant.From(in text), refreshPreview: true);
		}), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(AddTextEdit("DescribeTextEdit", "描述文本 Key", config.describe, (string text) =>
		{
			SetConfigProperty("describe", Variant.From(in text), refreshPreview: false);
		}), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(AddTextEdit("HandbookDescribeTextEdit", "图鉴描述 Key", config.handbookDescribe, (string text) =>
		{
			SetConfigProperty("handbookDescribe", Variant.From(in text), refreshPreview: false);
		}), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(AddTextEdit("HandbookStoryTextEdit", "图鉴故事 Key", config.handbookStory, (string text) =>
		{
			SetConfigProperty("handbookStory", Variant.From(in text), refreshPreview: false);
		}), forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(CreateShovelListPanel(config), forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private Control CreateShovelListPanel(ShovelConfig config)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.Name = "ShovelListPanel";
		hBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddThemeConstantOverride("separation", 8);
		_unlockConditionList = CreateArraySummaryList("UnlockConditionList", "解锁条件");
		_eventList = CreateArraySummaryList("EventList", "事件");
		_shovelableNameList = CreateArraySummaryList("ShovelableNameList", "可铲对象");
		hBoxContainer.AddChild(WrapList("解锁条件", _unlockConditionList), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(WrapList("事件", _eventList), forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(CreateShovelableNamePanel(config), forceReadableName: false, InternalMode.Disabled);
		RefreshShovelListPanel(config);
		return hBoxContainer;
	}

	private Control CreateShovelableNamePanel(ShovelConfig config)
	{
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.Name = "ShovelableNamePanel";
		vBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		vBoxContainer.AddChild(new Label
		{
			Text = "可铲对象",
			ClipText = true
		}, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(_shovelableNameList, forceReadableName: false, InternalMode.Disabled);
		HBoxContainer hBoxContainer = new HBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hBoxContainer.AddThemeConstantOverride("separation", 4);
		LineEdit edit = new LineEdit
		{
			Name = "ShovelableNameLineEdit",
			PlaceholderText = "角色 key",
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		hBoxContainer.AddChild(edit, forceReadableName: false, InternalMode.Disabled);
		Button button = new Button
		{
			Text = "添加"
		};
		button.Pressed += () =>
		{
			AddShovelableName(edit.Text);
		};
		hBoxContainer.AddChild(button, forceReadableName: false, InternalMode.Disabled);
		Button button2 = new Button
		{
			Text = "删除"
		};
		button2.Pressed += RemoveSelectedShovelableName;
		hBoxContainer.AddChild(button2, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(hBoxContainer, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private void SetConfigProperty(string propertyName, Variant value, bool refreshPreview)
	{
		if (!_updatingControls && _editingCollectable?.config != null && !string.IsNullOrWhiteSpace(propertyName))
		{
			if (HasResourceProperty(_editingCollectable.config, propertyName))
			{
				_editingCollectable.config.Set(propertyName, value);
			}
			else
			{
				_editingCollectable.config.SetMeta("mod_" + propertyName, value);
			}
			_editingCollectable.config.EmitChanged();
			ScheduleCollectableSave(_editingCollectable);
			if (refreshPreview)
			{
				UpdateCollectablePreview();
				RebuildCollectableRuntimePreview();
			}
			else
			{
				UpdateCollectableRuntimeStatus();
			}
			RefreshKindPanel();
			RefreshConfigPanel();
		}
	}

	private void AddShovelableName(string name)
	{
		if (_editingCollectable?.config is ShovelConfig shovelConfig)
		{
			name = (name ?? "").Trim();
			if (!string.IsNullOrWhiteSpace(name) && !shovelConfig.shovelableNames.Contains(name))
			{
				shovelConfig.shovelableNames.Add(name);
				shovelConfig.EmitChanged();
				SaveCollectableResource(_editingCollectable);
				RefreshShovelListPanel(shovelConfig);
				RefreshConfigPanel();
			}
		}
	}

	private void RemoveSelectedShovelableName()
	{
		if (_editingCollectable?.config is ShovelConfig shovelConfig && GodotObject.IsInstanceValid(_shovelableNameList))
		{
			int num = ((_shovelableNameList.GetSelectedItems().Length != 0) ? _shovelableNameList.GetSelectedItems()[0] : (-1));
			if (num >= 0 && num < shovelConfig.shovelableNames.Count)
			{
				shovelConfig.shovelableNames.RemoveAt(num);
				shovelConfig.EmitChanged();
				SaveCollectableResource(_editingCollectable);
				RefreshShovelListPanel(shovelConfig);
				RefreshConfigPanel();
			}
		}
	}

	private void SaveCollectableResource(CollectableConfig collectable)
	{
		string currentResourcePath = CurrentResourcePath;
		if (string.IsNullOrWhiteSpace(currentResourcePath) || !GodotObject.IsInstanceValid(collectable))
		{
			return;
		}
		Error error = ResourceSaver.Save(collectable, currentResourcePath, ResourceSaver.SaverFlags.None);
		if (error != Error.Ok)
		{
			GD.PushWarning($"Collectable editor save failed: {error} {currentResourcePath}");
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

	private void ScheduleCollectableSave(CollectableConfig collectable)
	{
		if (GodotObject.IsInstanceValid(collectable))
		{
			_pendingCollectableSaveMessage = CurrentResourcePath;
			if (GodotObject.IsInstanceValid(_collectableSaveTimer))
			{
				_collectableSaveTimer.Start();
			}
			else
			{
				SavePendingCollectable();
			}
		}
	}

	private void SavePendingCollectable()
	{
		if (!string.IsNullOrWhiteSpace(_pendingCollectableSaveMessage))
		{
			_pendingCollectableSaveMessage = null;
			if (_editingCollectable != null)
			{
				SaveCollectableResource(_editingCollectable);
			}
		}
	}

	private void UpdateCollectablePreview()
	{
		if (_editingCollectable == null)
		{
			return;
		}
		_updatingControls = true;
		try
		{
			if (GodotObject.IsInstanceValid(_previewTitleLabel))
			{
				_previewTitleLabel.Text = "收集物: " + EmptyToPlaceholder(_editingCollectable.saveKey);
			}
			if (GodotObject.IsInstanceValid(_previewConfigLabel))
			{
				_previewConfigLabel.Text = "配置: " + FormatResource(_editingCollectable.config);
			}
			if (GodotObject.IsInstanceValid(_previewValueLabel))
			{
				_previewValueLabel.Text = "value / amount: " + BuildValueText(_editingCollectable.config);
			}
			if (GodotObject.IsInstanceValid(_previewAudioLabel))
			{
				_previewAudioLabel.Text = "audio: " + EmptyToPlaceholder(ReadString(_editingCollectable.config, "audio"));
			}
			if (GodotObject.IsInstanceValid(_previewSceneLabel))
			{
				_previewSceneLabel.Text = "scene: " + ReadKnown(_editingCollectable.config, "scene", "collectableScene", "packedScene");
			}
			if (GodotObject.IsInstanceValid(_texturePreview))
			{
				_texturePreview.Texture = TryReadTexture(_editingCollectable.config, "texture", "icon", "previewTexture", "image");
			}
			UpdateCollectableRuntimeStatus();
			RefreshKindPanel();
		}
		finally
		{
			_updatingControls = false;
		}
	}

	private void RefreshKindPanel()
	{
		CollectableKind kind = ResolveCollectableKind(_editingCollectable?.config);
		SelectCollectableKindCard(kind);
		if (GodotObject.IsInstanceValid(_kindSummaryLabel))
		{
			_kindSummaryLabel.Text = BuildCollectableKindSummary(_editingCollectable?.config);
		}
	}

	private void RefreshConfigPanel()
	{
		if (_editingCollectable != null)
		{
			RefreshConfigPanel(_editingCollectable);
		}
	}

	private void RefreshConfigPanel(CollectableConfig collectable)
	{
		if (!GodotObject.IsInstanceValid(_configPropertyList))
		{
			return;
		}
		_configPropertyList.Clear();
		Resource resource = collectable?.config;
		if (!GodotObject.IsInstanceValid(resource))
		{
			_configPropertyList.AddItem("未配置 config");
			return;
		}
		foreach (Dictionary property in resource.GetPropertyList())
		{
			if (!property.ContainsKey("name"))
			{
				continue;
			}
			string text = property["name"].AsString();
			if (!text.StartsWith("Resource/") && !text.StartsWith("resource_") && !(text == "script"))
			{
				_configPropertyList.AddItem(text + ": " + FormatVariant(ReadVariant(resource, text)));
				if (_configPropertyList.ItemCount >= 40)
				{
					break;
				}
			}
		}
	}

	private void RefreshShovelListPanel(ShovelConfig config)
	{
		if (!GodotObject.IsInstanceValid(config))
		{
			return;
		}
		PopulateArraySummary(_unlockConditionList, config.unlockCheckList);
		PopulateArraySummary(_eventList, config.eventList);
		if (!GodotObject.IsInstanceValid(_shovelableNameList))
		{
			return;
		}
		_shovelableNameList.Clear();
		if (config.shovelableNames == null || config.shovelableNames.Count == 0)
		{
			_shovelableNameList.AddItem("未配置");
			return;
		}
		for (int i = 0; i < config.shovelableNames.Count; i++)
		{
			_shovelableNameList.AddItem(config.shovelableNames[i]);
		}
	}

	private void AddSummaryRows(CollectableConfig collectable)
	{
		AddItemIfMissing(PreviewList, "收集物 saveKey -> " + EmptyToPlaceholder(collectable.saveKey));
		AddItemIfMissing(PreviewList, "config -> " + FormatResource(collectable.config));
		AddItemIfMissing(PreviewList, "value / amount -> " + BuildValueText(collectable.config));
		AddItemIfMissing(TimelineList, "收集物资源 -> " + CurrentResourcePath);
		AddItemIfMissing(GraphList, "收集物 -> config -> scene / texture / icon / audio / value / amount");
		AddItemIfMissing(ReferenceList, "config -> " + FormatResource(collectable.config));
		AddItemIfMissing(ReferenceList, "scene -> " + ReadKnown(collectable.config, "scene", "collectableScene", "packedScene"));
		AddItemIfMissing(ReferenceList, "texture -> " + FormatResource(TryReadTexture(collectable.config, "texture", "icon", "previewTexture", "image")));
		AddItemIfMissing(ReferenceList, "audio -> " + ReadString(collectable.config, "audio"));
	}

	private Control AddLineEdit(string nodeName, string label, string value, Action<string> changed)
	{
		LineEdit lineEdit = new LineEdit
		{
			Name = nodeName,
			Text = (value ?? ""),
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
		lineEdit.TextChanged += (string text) =>
		{
			if (!_updatingControls)
			{
				changed?.Invoke(text);
			}
		};
		return WrapField(label, lineEdit);
	}

	private Control AddTextEdit(string nodeName, string label, string value, Action<string> changed)
	{
		TextEdit edit = new TextEdit
		{
			Name = nodeName,
			Text = (value ?? ""),
			CustomMinimumSize = new Vector2(0f, 62f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			WrapMode = TextEdit.LineWrappingMode.Boundary
		};
		edit.TextChanged += () =>
		{
			if (!_updatingControls)
			{
				changed?.Invoke(edit.Text);
			}
		};
		return WrapField(label, edit);
	}

	private Control AddSpinBox(string nodeName, string label, double value, double min, double max, double step, Action<double> changed)
	{
		SpinBox spinBox = new SpinBox
		{
			Name = nodeName,
			MinValue = min,
			MaxValue = max,
			Step = step,
			Value = value,
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			CustomMinimumSize = new Vector2(96f, 0f)
		};
		spinBox.ValueChanged += (double newValue) =>
		{
			if (!_updatingControls)
			{
				changed?.Invoke(newValue);
			}
		};
		return WrapField(label, spinBox);
	}

	private Control AddResourcePicker(string nodeName, string label, string baseType, Resource value, Action<Resource> changed)
	{
		XWResourcePicker xWResourcePicker = XWResourcePicker.Create();
		xWResourcePicker.Name = nodeName;
		xWResourcePicker.Setup(baseType);
		xWResourcePicker.SetEditedResource(value);
		xWResourcePicker.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		xWResourcePicker.ResourceChanged += (Resource resource) =>
		{
			if (!_updatingControls)
			{
				changed?.Invoke(resource);
			}
		};
		return WrapField(label, xWResourcePicker);
	}

	private static Label CreateSectionTitle(string text)
	{
		return new Label
		{
			Text = text,
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};
	}

	private static Control WrapField(string label, Control editor)
	{
		HBoxContainer hBoxContainer = new HBoxContainer();
		hBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		hBoxContainer.AddThemeConstantOverride("separation", 8);
		hBoxContainer.AddChild(new Label
		{
			Text = label,
			CustomMinimumSize = new Vector2(112f, 0f),
			VerticalAlignment = VerticalAlignment.Center,
			ClipText = true
		}, forceReadableName: false, InternalMode.Disabled);
		hBoxContainer.AddChild(editor, forceReadableName: false, InternalMode.Disabled);
		return hBoxContainer;
	}

	private static ItemList CreateArraySummaryList(string nodeName, string tooltip)
	{
		return new ItemList
		{
			Name = nodeName,
			TooltipText = tooltip,
			CustomMinimumSize = new Vector2(160f, 120f),
			SizeFlagsHorizontal = SizeFlags.ExpandFill,
			SizeFlagsVertical = SizeFlags.ExpandFill
		};
	}

	private static Control WrapList(string title, ItemList list)
	{
		VBoxContainer vBoxContainer = new VBoxContainer();
		vBoxContainer.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		vBoxContainer.SizeFlagsVertical = SizeFlags.ExpandFill;
		vBoxContainer.AddChild(new Label
		{
			Text = title,
			ClipText = true
		}, forceReadableName: false, InternalMode.Disabled);
		vBoxContainer.AddChild(list, forceReadableName: false, InternalMode.Disabled);
		return vBoxContainer;
	}

	private static void PopulateArraySummary<[MustBeVariant] T>(ItemList list, Array<T> array)
	{
		if (!GodotObject.IsInstanceValid(list))
		{
			return;
		}
		list.Clear();
		if (array == null || array.Count == 0)
		{
			list.AddItem("未配置");
			return;
		}
		for (int i = 0; i < array.Count; i++)
		{
			list.AddItem($"{i}: {FormatObject(array[i])}");
		}
	}

	private void SelectCollectableKindCard(CollectableKind kind)
	{
		if (!GodotObject.IsInstanceValid(_collectableKindCards))
		{
			return;
		}
		foreach (Node child in _collectableKindCards.GetChildren())
		{
			if (child is XWGameVisualChoiceCard xWGameVisualChoiceCard)
			{
				string choiceKey = xWGameVisualChoiceCard.ChoiceKey;
				int num = (int)kind;
				xWGameVisualChoiceCard.SetSelected(choiceKey == num.ToString());
			}
		}
	}

	private static Texture2D TryReadTexture(Resource resource, params string[] propertyNames)
	{
		foreach (string text in propertyNames)
		{
			if (ReadResource(resource, text) is Texture2D result)
			{
				return result;
			}
		}
		return null;
	}

	private static CollectableKind ResolveCollectableKind(Resource config)
	{
		if (config is ShovelConfig)
		{
			return CollectableKind.Shovel;
		}
		if (config is AwardSettlementConfig)
		{
			bool flag = Enum.TryParse<CollectableKind>(config.HasMeta("mod_collectable_kind") ? config.GetMeta("mod_collectable_kind").AsString() : "", out var result);
			if (flag)
			{
				bool flag2 = (uint)(result - 2) <= 1u;
				flag = flag2;
			}
			if (flag)
			{
				return result;
			}
			return CollectableKind.AwardNote;
		}
		if (!GodotObject.IsInstanceValid(config))
		{
			return CollectableKind.AwardNote;
		}
		return CollectableKind.Custom;
	}

	private static string GetCollectableKindLabel(CollectableKind kind)
	{
		return kind switch
		{
			CollectableKind.Shovel => "铲子奖励", 
			CollectableKind.AwardNote => "图鉴纸条", 
			CollectableKind.Sun => "阳光奖励", 
			CollectableKind.Coin => "金币奖励", 
			_ => "自定义资源", 
		};
	}

	private static string BuildCollectableKindSummary(Resource config)
	{
		CollectableKind kind = ResolveCollectableKind(config);
		return string.Concat(str2: FormatResource(config), str0: GetCollectableKindLabel(kind), str1: " / ");
	}

	private string BuildDefaultConfigName()
	{
		string text = _editingCollectable?.saveKey;
		if (string.IsNullOrWhiteSpace(text))
		{
			text = _editingCollectable?.ResourceName;
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			text = "NewCollectable";
		}
		return text + "Config";
	}

	private static Resource ReadResource(Resource resource, params string[] propertyNames)
	{
		foreach (string propertyName in propertyNames)
		{
			Variant variant = ReadVariant(resource, propertyName);
			if (variant.VariantType == Variant.Type.Object && variant.AsGodotObject() is Resource result)
			{
				return result;
			}
		}
		return null;
	}

	private static string ReadKnown(Resource resource, params string[] propertyNames)
	{
		foreach (string propertyName in propertyNames)
		{
			Variant value = ReadVariant(resource, propertyName);
			if (value.VariantType != Variant.Type.Nil)
			{
				return FormatVariant(value);
			}
		}
		return "未配置";
	}

	private static string ReadString(Resource resource, string propertyName)
	{
		Variant variant = ReadVariant(resource, propertyName);
		if (variant.VariantType != Variant.Type.Nil)
		{
			return variant.AsString();
		}
		return "";
	}

	private static double ReadDouble(Resource resource, string propertyName, double fallback)
	{
		Variant variant = ReadVariant(resource, propertyName);
		Variant.Type variantType = variant.VariantType;
		if ((ulong)(variantType - 2) > 1uL || 1 == 0)
		{
			return fallback;
		}
		return variant.AsDouble();
	}

	private static Variant ReadVariant(Resource resource, string propertyName)
	{
		if (!GodotObject.IsInstanceValid(resource) || string.IsNullOrWhiteSpace(propertyName))
		{
			return default;
		}
		try
		{
			if (HasResourceProperty(resource, propertyName))
			{
				return resource.Get(propertyName);
			}
			string text = "mod_" + propertyName;
			if (resource.HasMeta(text))
			{
				return resource.GetMeta(text);
			}
		}
		catch
		{
		}
		return default;
	}

	private static bool HasResourceProperty(Resource resource, string propertyName)
	{
		if (!GodotObject.IsInstanceValid(resource))
		{
			return false;
		}
		foreach (Dictionary property in resource.GetPropertyList())
		{
			if (property.ContainsKey("name") && property["name"].AsString() == propertyName)
			{
				return true;
			}
		}
		return false;
	}

	private static string BuildValueText(Resource config)
	{
		string text = ReadKnown(config, "value");
		string text2 = ReadKnown(config, "amount");
		return text + " / " + text2;
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
		Variant.Type variantType = value.VariantType;
		if ((ulong)variantType <= 4uL)
		{
			switch ((int)variantType)
			{
			case 0:
				return "空";
			case 1:
				return value.AsBool() ? "true" : "false";
			case 2:
				return value.AsInt64().ToString();
			case 3:
				return value.AsDouble().ToString("0.###");
			case 4:
				goto IL_00bb;
			}
		}
		Variant.Type num = variantType - 21;
		if ((ulong)num > 7uL)
		{
			goto IL_0161;
		}
		switch ((int)num)
		{
		case 0:
		case 1:
			break;
		case 3:
			return FormatResource(value.AsGodotObject() as Resource);
		case 7:
			return $"Array({value.AsGodotArray().Count})";
		case 6:
			return $"Dictionary({value.AsGodotDictionary().Count})";
		default:
			goto IL_0161;
		}
		goto IL_00bb;
		IL_0161:
		return value.ToString();
		IL_00bb:
		return EmptyToPlaceholder(value.AsString());
	}

	private static string FormatObject(object value)
	{
		if (value is Resource resource)
		{
			return FormatResource(resource);
		}
		if (!(value is GodotObject godotObject))
		{
			return value?.ToString() ?? "空";
		}
		return godotObject.GetType().Name;
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
		return new List<MethodInfo>(70)
		{
			new MethodInfo(MethodName._Ready, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName._ExitTree, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.OnVisualEditorVisibilityChanged, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "visible", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.DisposeCollectableBindings, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.HasCompleteCollectableVisualCoverage, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RenderAwardSettlementPreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "award", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnAwardSettlementPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetAwardSettlementTexture, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.StringName, "property", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshAwardSettlementPreviewFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RenderCollectableEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "collectable", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindCollectableTopLevelFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "collectable", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.MountCollectableConfigPicker, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("HBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "collectable", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindCollectableCommonConfigFields, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "host", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false),
				new PropertyInfo(Variant.Type.Object, "collectable", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OpenCollectableConfigResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.OnCollectableTopLevelPropertyEdited, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "committed", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCollectableConfig, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "actionName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshCollectableEditorFromHistory, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.BindCollectablePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BindCollectableRuntimePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "frame", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.RebuildCollectableRuntimePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.AddCollectableTextureLayer, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "texture", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false),
				new PropertyInfo(Variant.Type.String, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maxWidth", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "maxHeight", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Vector2, "offset", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SimulateCollectablePickup, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ResolveCollectablePoolScene, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("PackedScene"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetCollectableRuntimeRunning, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Bool, "running", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.UpdateCollectableRuntimeStatus, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "prefix", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BindCollectableKindPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "root", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("VBoxContainer"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCollectableKindCards, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "selectedKind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.SetCollectableKind, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.EnsureCollectableConfigForKind, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateAwardSettlementConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateShovelCollectableConfig, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RebuildCollectableEditor, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.CreateCollectableConfigPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "collectable", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateAwardPreviewPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.CreateShovelPreviewPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateShovelListPanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateShovelableNamePanel, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SetConfigProperty, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Nil, "value", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false),
				new PropertyInfo(Variant.Type.Bool, "refreshPreview", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.AddShovelableName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "name", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.RemoveSelectedShovelableName, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.SaveCollectableResource, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "collectable", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.ScheduleCollectableSave, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "collectable", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.SavePendingCollectable, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.UpdateCollectablePreview, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshKindPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshConfigPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.RefreshConfigPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "collectable", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.RefreshShovelListPanel, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.AddSummaryRows, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "collectable", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateSectionTitle, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Label"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "text", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WrapField, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "label", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "editor", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false)
			}, null),
			new MethodInfo(MethodName.CreateArraySummaryList, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "nodeName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.String, "tooltip", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.WrapList, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Control"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.String, "title", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Object, "list", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("ItemList"), exported: false)
			}, null),
			new MethodInfo(MethodName.SelectCollectableKindCard, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.TryReadTexture, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Texture2D"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "propertyNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ResolveCollectableKind, new PropertyInfo(Variant.Type.Int, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.GetCollectableKindLabel, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Int, "kind", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildCollectableKindSummary, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
			}, null),
			new MethodInfo(MethodName.BuildDefaultConfigName, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal, null, null),
			new MethodInfo(MethodName.ReadResource, new PropertyInfo(Variant.Type.Object, "", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "propertyNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadKnown, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.PackedStringArray, "propertyNames", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadString, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadDouble, new PropertyInfo(Variant.Type.Float, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false),
				new PropertyInfo(Variant.Type.Float, "fallback", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.ReadVariant, new PropertyInfo(Variant.Type.Nil, "", PropertyHint.None, "", PropertyUsageFlags.Default | PropertyUsageFlags.NilIsVariant, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.HasResourceProperty, new PropertyInfo(Variant.Type.Bool, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false),
				new PropertyInfo(Variant.Type.String, "propertyName", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false)
			}, null),
			new MethodInfo(MethodName.BuildValueText, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "config", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
			new MethodInfo(MethodName.FormatResource, new PropertyInfo(Variant.Type.String, "", PropertyHint.None, "", PropertyUsageFlags.Default, exported: false), MethodFlags.Normal | MethodFlags.Static, new List<PropertyInfo>
			{
				new PropertyInfo(Variant.Type.Object, "resource", PropertyHint.None, "", PropertyUsageFlags.Default, new StringName("Resource"), exported: false)
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
		if (method == MethodName.DisposeCollectableBindings && args.Count == 0)
		{
			DisposeCollectableBindings();
			ret = default;
			return true;
		}
		if (method == MethodName.HasCompleteCollectableVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteCollectableVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.RenderAwardSettlementPreview && args.Count == 1)
		{
			RenderAwardSettlementPreview(VariantUtils.ConvertTo<AwardSettlementConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnAwardSettlementPropertyEdited && args.Count == 1)
		{
			OnAwardSettlementPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetAwardSettlementTexture && args.Count == 3)
		{
			SetAwardSettlementTexture(VariantUtils.ConvertTo<StringName>(in args[0]), VariantUtils.ConvertTo<Resource>(in args[1]), VariantUtils.ConvertTo<string>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshAwardSettlementPreviewFromHistory && args.Count == 0)
		{
			RefreshAwardSettlementPreviewFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.RenderCollectableEditor && args.Count == 1)
		{
			RenderCollectableEditor(VariantUtils.ConvertTo<CollectableConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindCollectableTopLevelFields && args.Count == 2)
		{
			BindCollectableTopLevelFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<CollectableConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.MountCollectableConfigPicker && args.Count == 2)
		{
			MountCollectableConfigPicker(VariantUtils.ConvertTo<HBoxContainer>(in args[0]), VariantUtils.ConvertTo<CollectableConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindCollectableCommonConfigFields && args.Count == 2)
		{
			BindCollectableCommonConfigFields(VariantUtils.ConvertTo<VBoxContainer>(in args[0]), VariantUtils.ConvertTo<CollectableConfig>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.OpenCollectableConfigResource && args.Count == 1)
		{
			OpenCollectableConfigResource(VariantUtils.ConvertTo<Resource>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.OnCollectableTopLevelPropertyEdited && args.Count == 1)
		{
			OnCollectableTopLevelPropertyEdited(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCollectableConfig && args.Count == 2)
		{
			SetCollectableConfig(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshCollectableEditorFromHistory && args.Count == 0)
		{
			RefreshCollectableEditorFromHistory();
			ret = default;
			return true;
		}
		if (method == MethodName.BindCollectablePreview && args.Count == 1)
		{
			BindCollectablePreview(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindCollectableRuntimePreview && args.Count == 1)
		{
			BindCollectableRuntimePreview(VariantUtils.ConvertTo<Control>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RebuildCollectableRuntimePreview && args.Count == 0)
		{
			RebuildCollectableRuntimePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.AddCollectableTextureLayer && args.Count == 5)
		{
			AddCollectableTextureLayer(VariantUtils.ConvertTo<Texture2D>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<float>(in args[2]), VariantUtils.ConvertTo<float>(in args[3]), VariantUtils.ConvertTo<Vector2>(in args[4]));
			ret = default;
			return true;
		}
		if (method == MethodName.SimulateCollectablePickup && args.Count == 0)
		{
			SimulateCollectablePickup();
			ret = default;
			return true;
		}
		if (method == MethodName.ResolveCollectablePoolScene && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<PackedScene>(ResolveCollectablePoolScene(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.SetCollectableRuntimeRunning && args.Count == 1)
		{
			SetCollectableRuntimeRunning(VariantUtils.ConvertTo<bool>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCollectableRuntimeStatus && args.Count == 1)
		{
			UpdateCollectableRuntimeStatus(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BindCollectableKindPanel && args.Count == 1)
		{
			BindCollectableKindPanel(VariantUtils.ConvertTo<VBoxContainer>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.BuildCollectableKindCards && args.Count == 1)
		{
			BuildCollectableKindCards(VariantUtils.ConvertTo<CollectableKind>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SetCollectableKind && args.Count == 1)
		{
			SetCollectableKind(VariantUtils.ConvertTo<CollectableKind>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.EnsureCollectableConfigForKind && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Resource>(EnsureCollectableConfigForKind(VariantUtils.ConvertTo<CollectableKind>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateAwardSettlementConfig && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<AwardSettlementConfig>(CreateAwardSettlementConfig(VariantUtils.ConvertTo<CollectableKind>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateShovelCollectableConfig && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<ShovelConfig>(CreateShovelCollectableConfig());
			return true;
		}
		if (method == MethodName.RebuildCollectableEditor && args.Count == 0)
		{
			RebuildCollectableEditor();
			ret = default;
			return true;
		}
		if (method == MethodName.CreateCollectableConfigPanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateCollectableConfigPanel(VariantUtils.ConvertTo<CollectableConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateAwardPreviewPanel && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateAwardPreviewPanel(VariantUtils.ConvertTo<AwardSettlementConfig>(in args[0]), VariantUtils.ConvertTo<CollectableKind>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateShovelPreviewPanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateShovelPreviewPanel(VariantUtils.ConvertTo<ShovelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateShovelListPanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateShovelListPanel(VariantUtils.ConvertTo<ShovelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateShovelableNamePanel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Control>(CreateShovelableNamePanel(VariantUtils.ConvertTo<ShovelConfig>(in args[0])));
			return true;
		}
		if (method == MethodName.SetConfigProperty && args.Count == 3)
		{
			SetConfigProperty(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Variant>(in args[1]), VariantUtils.ConvertTo<bool>(in args[2]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddShovelableName && args.Count == 1)
		{
			AddShovelableName(VariantUtils.ConvertTo<string>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RemoveSelectedShovelableName && args.Count == 0)
		{
			RemoveSelectedShovelableName();
			ret = default;
			return true;
		}
		if (method == MethodName.SaveCollectableResource && args.Count == 1)
		{
			SaveCollectableResource(VariantUtils.ConvertTo<CollectableConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.ScheduleCollectableSave && args.Count == 1)
		{
			ScheduleCollectableSave(VariantUtils.ConvertTo<CollectableConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.SavePendingCollectable && args.Count == 0)
		{
			SavePendingCollectable();
			ret = default;
			return true;
		}
		if (method == MethodName.UpdateCollectablePreview && args.Count == 0)
		{
			UpdateCollectablePreview();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshKindPanel && args.Count == 0)
		{
			RefreshKindPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshConfigPanel && args.Count == 0)
		{
			RefreshConfigPanel();
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshConfigPanel && args.Count == 1)
		{
			RefreshConfigPanel(VariantUtils.ConvertTo<CollectableConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.RefreshShovelListPanel && args.Count == 1)
		{
			RefreshShovelListPanel(VariantUtils.ConvertTo<ShovelConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.AddSummaryRows && args.Count == 1)
		{
			AddSummaryRows(VariantUtils.ConvertTo<CollectableConfig>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.CreateSectionTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateSectionTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.WrapField && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(WrapField(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateArraySummaryList && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<ItemList>(CreateArraySummaryList(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.WrapList && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(WrapList(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<ItemList>(in args[1])));
			return true;
		}
		if (method == MethodName.SelectCollectableKindCard && args.Count == 1)
		{
			SelectCollectableKindCard(VariantUtils.ConvertTo<CollectableKind>(in args[0]));
			ret = default;
			return true;
		}
		if (method == MethodName.TryReadTexture && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(TryReadTexture(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolveCollectableKind && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CollectableKind>(ResolveCollectableKind(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCollectableKindLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCollectableKindLabel(VariantUtils.ConvertTo<CollectableKind>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCollectableKindSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCollectableKindSummary(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildDefaultConfigName && args.Count == 0)
		{
			ret = VariantUtils.CreateFrom<string>(BuildDefaultConfigName());
			return true;
		}
		if (method == MethodName.ReadResource && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Resource>(ReadResource(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadKnown && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadKnown(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadString && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadString(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadDouble && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadDouble(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadVariant && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadVariant(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.HasResourceProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasResourceProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildValueText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildValueText(VariantUtils.ConvertTo<Resource>(in args[0])));
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
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
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
		if (method == MethodName.HasCompleteCollectableVisualCoverage && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<bool>(HasCompleteCollectableVisualCoverage(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.CreateSectionTitle && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<Label>(CreateSectionTitle(VariantUtils.ConvertTo<string>(in args[0])));
			return true;
		}
		if (method == MethodName.WrapField && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(WrapField(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<Control>(in args[1])));
			return true;
		}
		if (method == MethodName.CreateArraySummaryList && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<ItemList>(CreateArraySummaryList(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.WrapList && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Control>(WrapList(VariantUtils.ConvertTo<string>(in args[0]), VariantUtils.ConvertTo<ItemList>(in args[1])));
			return true;
		}
		if (method == MethodName.TryReadTexture && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Texture2D>(TryReadTexture(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ResolveCollectableKind && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<CollectableKind>(ResolveCollectableKind(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.GetCollectableKindLabel && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(GetCollectableKindLabel(VariantUtils.ConvertTo<CollectableKind>(in args[0])));
			return true;
		}
		if (method == MethodName.BuildCollectableKindSummary && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildCollectableKindSummary(VariantUtils.ConvertTo<Resource>(in args[0])));
			return true;
		}
		if (method == MethodName.ReadResource && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Resource>(ReadResource(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadKnown && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadKnown(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string[]>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadString && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<string>(ReadString(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.ReadDouble && args.Count == 3)
		{
			ret = VariantUtils.CreateFrom<double>(ReadDouble(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1]), VariantUtils.ConvertTo<double>(in args[2])));
			return true;
		}
		if (method == MethodName.ReadVariant && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<Variant>(ReadVariant(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.HasResourceProperty && args.Count == 2)
		{
			ret = VariantUtils.CreateFrom<bool>(HasResourceProperty(VariantUtils.ConvertTo<Resource>(in args[0]), VariantUtils.ConvertTo<string>(in args[1])));
			return true;
		}
		if (method == MethodName.BuildValueText && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(BuildValueText(VariantUtils.ConvertTo<Resource>(in args[0])));
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
		if (method == MethodName.FormatResource && args.Count == 1)
		{
			ret = VariantUtils.CreateFrom<string>(FormatResource(VariantUtils.ConvertTo<Resource>(in args[0])));
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
		if (method == MethodName.DisposeCollectableBindings)
		{
			return true;
		}
		if (method == MethodName.HasCompleteCollectableVisualCoverage)
		{
			return true;
		}
		if (method == MethodName.RenderAwardSettlementPreview)
		{
			return true;
		}
		if (method == MethodName.OnAwardSettlementPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.SetAwardSettlementTexture)
		{
			return true;
		}
		if (method == MethodName.RefreshAwardSettlementPreviewFromHistory)
		{
			return true;
		}
		if (method == MethodName.RenderCollectableEditor)
		{
			return true;
		}
		if (method == MethodName.BindCollectableTopLevelFields)
		{
			return true;
		}
		if (method == MethodName.MountCollectableConfigPicker)
		{
			return true;
		}
		if (method == MethodName.BindCollectableCommonConfigFields)
		{
			return true;
		}
		if (method == MethodName.OpenCollectableConfigResource)
		{
			return true;
		}
		if (method == MethodName.OnCollectableTopLevelPropertyEdited)
		{
			return true;
		}
		if (method == MethodName.SetCollectableConfig)
		{
			return true;
		}
		if (method == MethodName.RefreshCollectableEditorFromHistory)
		{
			return true;
		}
		if (method == MethodName.BindCollectablePreview)
		{
			return true;
		}
		if (method == MethodName.BindCollectableRuntimePreview)
		{
			return true;
		}
		if (method == MethodName.RebuildCollectableRuntimePreview)
		{
			return true;
		}
		if (method == MethodName.AddCollectableTextureLayer)
		{
			return true;
		}
		if (method == MethodName.SimulateCollectablePickup)
		{
			return true;
		}
		if (method == MethodName.ResolveCollectablePoolScene)
		{
			return true;
		}
		if (method == MethodName.SetCollectableRuntimeRunning)
		{
			return true;
		}
		if (method == MethodName.UpdateCollectableRuntimeStatus)
		{
			return true;
		}
		if (method == MethodName.BindCollectableKindPanel)
		{
			return true;
		}
		if (method == MethodName.BuildCollectableKindCards)
		{
			return true;
		}
		if (method == MethodName.SetCollectableKind)
		{
			return true;
		}
		if (method == MethodName.EnsureCollectableConfigForKind)
		{
			return true;
		}
		if (method == MethodName.CreateAwardSettlementConfig)
		{
			return true;
		}
		if (method == MethodName.CreateShovelCollectableConfig)
		{
			return true;
		}
		if (method == MethodName.RebuildCollectableEditor)
		{
			return true;
		}
		if (method == MethodName.CreateCollectableConfigPanel)
		{
			return true;
		}
		if (method == MethodName.CreateAwardPreviewPanel)
		{
			return true;
		}
		if (method == MethodName.CreateShovelPreviewPanel)
		{
			return true;
		}
		if (method == MethodName.CreateShovelListPanel)
		{
			return true;
		}
		if (method == MethodName.CreateShovelableNamePanel)
		{
			return true;
		}
		if (method == MethodName.SetConfigProperty)
		{
			return true;
		}
		if (method == MethodName.AddShovelableName)
		{
			return true;
		}
		if (method == MethodName.RemoveSelectedShovelableName)
		{
			return true;
		}
		if (method == MethodName.SaveCollectableResource)
		{
			return true;
		}
		if (method == MethodName.ScheduleCollectableSave)
		{
			return true;
		}
		if (method == MethodName.SavePendingCollectable)
		{
			return true;
		}
		if (method == MethodName.UpdateCollectablePreview)
		{
			return true;
		}
		if (method == MethodName.RefreshKindPanel)
		{
			return true;
		}
		if (method == MethodName.RefreshConfigPanel)
		{
			return true;
		}
		if (method == MethodName.RefreshShovelListPanel)
		{
			return true;
		}
		if (method == MethodName.AddSummaryRows)
		{
			return true;
		}
		if (method == MethodName.CreateSectionTitle)
		{
			return true;
		}
		if (method == MethodName.WrapField)
		{
			return true;
		}
		if (method == MethodName.CreateArraySummaryList)
		{
			return true;
		}
		if (method == MethodName.WrapList)
		{
			return true;
		}
		if (method == MethodName.SelectCollectableKindCard)
		{
			return true;
		}
		if (method == MethodName.TryReadTexture)
		{
			return true;
		}
		if (method == MethodName.ResolveCollectableKind)
		{
			return true;
		}
		if (method == MethodName.GetCollectableKindLabel)
		{
			return true;
		}
		if (method == MethodName.BuildCollectableKindSummary)
		{
			return true;
		}
		if (method == MethodName.BuildDefaultConfigName)
		{
			return true;
		}
		if (method == MethodName.ReadResource)
		{
			return true;
		}
		if (method == MethodName.ReadKnown)
		{
			return true;
		}
		if (method == MethodName.ReadString)
		{
			return true;
		}
		if (method == MethodName.ReadDouble)
		{
			return true;
		}
		if (method == MethodName.ReadVariant)
		{
			return true;
		}
		if (method == MethodName.HasResourceProperty)
		{
			return true;
		}
		if (method == MethodName.BuildValueText)
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
		if (method == MethodName.FormatResource)
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
		if (name == PropertyName._editingCollectable)
		{
			_editingCollectable = VariantUtils.ConvertTo<CollectableConfig>(in value);
			return true;
		}
		if (name == PropertyName._editingAwardSettlement)
		{
			_editingAwardSettlement = VariantUtils.ConvertTo<AwardSettlementConfig>(in value);
			return true;
		}
		if (name == PropertyName._awardBackgroundPreview)
		{
			_awardBackgroundPreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._awardImagePreview)
		{
			_awardImagePreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._awardTexturePreview)
		{
			_awardTexturePreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._awardEmptyHint)
		{
			_awardEmptyHint = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._awardStatusLabel)
		{
			_awardStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._awardTitleLabel)
		{
			_awardTitleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewTitleLabel)
		{
			_previewTitleLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewConfigLabel)
		{
			_previewConfigLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewValueLabel)
		{
			_previewValueLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewAudioLabel)
		{
			_previewAudioLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._previewSceneLabel)
		{
			_previewSceneLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._kindSummaryLabel)
		{
			_kindSummaryLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._texturePreview)
		{
			_texturePreview = VariantUtils.ConvertTo<TextureRect>(in value);
			return true;
		}
		if (name == PropertyName._collectablePreviewViewport)
		{
			_collectablePreviewViewport = VariantUtils.ConvertTo<SubViewport>(in value);
			return true;
		}
		if (name == PropertyName._collectablePreviewRoot)
		{
			_collectablePreviewRoot = VariantUtils.ConvertTo<Node2D>(in value);
			return true;
		}
		if (name == PropertyName._collectableRuntimeStatusLabel)
		{
			_collectableRuntimeStatusLabel = VariantUtils.ConvertTo<Label>(in value);
			return true;
		}
		if (name == PropertyName._collectableRuntimeRunButton)
		{
			_collectableRuntimeRunButton = VariantUtils.ConvertTo<Button>(in value);
			return true;
		}
		if (name == PropertyName._collectableRuntimeRunning)
		{
			_collectableRuntimeRunning = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._collectableKindCards)
		{
			_collectableKindCards = VariantUtils.ConvertTo<HFlowContainer>(in value);
			return true;
		}
		if (name == PropertyName._configPropertyList)
		{
			_configPropertyList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._unlockConditionList)
		{
			_unlockConditionList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._eventList)
		{
			_eventList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._shovelableNameList)
		{
			_shovelableNameList = VariantUtils.ConvertTo<ItemList>(in value);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			_updatingControls = VariantUtils.ConvertTo<bool>(in value);
			return true;
		}
		if (name == PropertyName._collectableSaveTimer)
		{
			_collectableSaveTimer = VariantUtils.ConvertTo<Timer>(in value);
			return true;
		}
		if (name == PropertyName._pendingCollectableSaveMessage)
		{
			_pendingCollectableSaveMessage = VariantUtils.ConvertTo<string>(in value);
			return true;
		}
		return base.SetGodotClassPropertyValue(in name, in value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override bool GetGodotClassPropertyValue(in godot_string_name name, out godot_variant value)
	{
		if (name == PropertyName.CollectableKindVisualCardCount)
		{
			value = VariantUtils.CreateFrom<int>(CollectableKindVisualCardCount);
			return true;
		}
		if (name == PropertyName.IsCollectablePreviewRendering)
		{
			value = VariantUtils.CreateFrom<bool>(IsCollectablePreviewRendering);
			return true;
		}
		if (name == PropertyName._editingCollectable)
		{
			value = VariantUtils.CreateFrom(in _editingCollectable);
			return true;
		}
		if (name == PropertyName._editingAwardSettlement)
		{
			value = VariantUtils.CreateFrom(in _editingAwardSettlement);
			return true;
		}
		if (name == PropertyName._awardBackgroundPreview)
		{
			value = VariantUtils.CreateFrom(in _awardBackgroundPreview);
			return true;
		}
		if (name == PropertyName._awardImagePreview)
		{
			value = VariantUtils.CreateFrom(in _awardImagePreview);
			return true;
		}
		if (name == PropertyName._awardTexturePreview)
		{
			value = VariantUtils.CreateFrom(in _awardTexturePreview);
			return true;
		}
		if (name == PropertyName._awardEmptyHint)
		{
			value = VariantUtils.CreateFrom(in _awardEmptyHint);
			return true;
		}
		if (name == PropertyName._awardStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _awardStatusLabel);
			return true;
		}
		if (name == PropertyName._awardTitleLabel)
		{
			value = VariantUtils.CreateFrom(in _awardTitleLabel);
			return true;
		}
		if (name == PropertyName._previewTitleLabel)
		{
			value = VariantUtils.CreateFrom(in _previewTitleLabel);
			return true;
		}
		if (name == PropertyName._previewConfigLabel)
		{
			value = VariantUtils.CreateFrom(in _previewConfigLabel);
			return true;
		}
		if (name == PropertyName._previewValueLabel)
		{
			value = VariantUtils.CreateFrom(in _previewValueLabel);
			return true;
		}
		if (name == PropertyName._previewAudioLabel)
		{
			value = VariantUtils.CreateFrom(in _previewAudioLabel);
			return true;
		}
		if (name == PropertyName._previewSceneLabel)
		{
			value = VariantUtils.CreateFrom(in _previewSceneLabel);
			return true;
		}
		if (name == PropertyName._kindSummaryLabel)
		{
			value = VariantUtils.CreateFrom(in _kindSummaryLabel);
			return true;
		}
		if (name == PropertyName._texturePreview)
		{
			value = VariantUtils.CreateFrom(in _texturePreview);
			return true;
		}
		if (name == PropertyName._collectablePreviewViewport)
		{
			value = VariantUtils.CreateFrom(in _collectablePreviewViewport);
			return true;
		}
		if (name == PropertyName._collectablePreviewRoot)
		{
			value = VariantUtils.CreateFrom(in _collectablePreviewRoot);
			return true;
		}
		if (name == PropertyName._collectableRuntimeStatusLabel)
		{
			value = VariantUtils.CreateFrom(in _collectableRuntimeStatusLabel);
			return true;
		}
		if (name == PropertyName._collectableRuntimeRunButton)
		{
			value = VariantUtils.CreateFrom(in _collectableRuntimeRunButton);
			return true;
		}
		if (name == PropertyName._collectableRuntimeRunning)
		{
			value = VariantUtils.CreateFrom(in _collectableRuntimeRunning);
			return true;
		}
		if (name == PropertyName._collectableKindCards)
		{
			value = VariantUtils.CreateFrom(in _collectableKindCards);
			return true;
		}
		if (name == PropertyName._configPropertyList)
		{
			value = VariantUtils.CreateFrom(in _configPropertyList);
			return true;
		}
		if (name == PropertyName._unlockConditionList)
		{
			value = VariantUtils.CreateFrom(in _unlockConditionList);
			return true;
		}
		if (name == PropertyName._eventList)
		{
			value = VariantUtils.CreateFrom(in _eventList);
			return true;
		}
		if (name == PropertyName._shovelableNameList)
		{
			value = VariantUtils.CreateFrom(in _shovelableNameList);
			return true;
		}
		if (name == PropertyName._updatingControls)
		{
			value = VariantUtils.CreateFrom(in _updatingControls);
			return true;
		}
		if (name == PropertyName._collectableSaveTimer)
		{
			value = VariantUtils.CreateFrom(in _collectableSaveTimer);
			return true;
		}
		if (name == PropertyName._pendingCollectableSaveMessage)
		{
			value = VariantUtils.CreateFrom(in _pendingCollectableSaveMessage);
			return true;
		}
		return base.GetGodotClassPropertyValue(in name, out value);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal new static List<PropertyInfo> GetGodotPropertyList()
	{
		return new List<PropertyInfo>
		{
			new PropertyInfo(Variant.Type.Object, PropertyName._editingCollectable, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._editingAwardSettlement, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._awardBackgroundPreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._awardImagePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._awardTexturePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._awardEmptyHint, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._awardStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._awardTitleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewTitleLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewConfigLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewValueLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewAudioLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._previewSceneLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._kindSummaryLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._texturePreview, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._collectablePreviewViewport, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._collectablePreviewRoot, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._collectableRuntimeStatusLabel, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._collectableRuntimeRunButton, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._collectableRuntimeRunning, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._collectableKindCards, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._configPropertyList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._unlockConditionList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._eventList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._shovelableNameList, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName._updatingControls, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Object, PropertyName._collectableSaveTimer, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.String, PropertyName._pendingCollectableSaveMessage, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Int, PropertyName.CollectableKindVisualCardCount, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false),
			new PropertyInfo(Variant.Type.Bool, PropertyName.IsCollectablePreviewRendering, PropertyHint.None, "", PropertyUsageFlags.ScriptVariable, exported: false)
		};
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void SaveGodotObjectData(GodotSerializationInfo info)
	{
		base.SaveGodotObjectData(info);
		info.AddProperty(PropertyName._editingCollectable, Variant.From(in _editingCollectable));
		info.AddProperty(PropertyName._editingAwardSettlement, Variant.From(in _editingAwardSettlement));
		info.AddProperty(PropertyName._awardBackgroundPreview, Variant.From(in _awardBackgroundPreview));
		info.AddProperty(PropertyName._awardImagePreview, Variant.From(in _awardImagePreview));
		info.AddProperty(PropertyName._awardTexturePreview, Variant.From(in _awardTexturePreview));
		info.AddProperty(PropertyName._awardEmptyHint, Variant.From(in _awardEmptyHint));
		info.AddProperty(PropertyName._awardStatusLabel, Variant.From(in _awardStatusLabel));
		info.AddProperty(PropertyName._awardTitleLabel, Variant.From(in _awardTitleLabel));
		info.AddProperty(PropertyName._previewTitleLabel, Variant.From(in _previewTitleLabel));
		info.AddProperty(PropertyName._previewConfigLabel, Variant.From(in _previewConfigLabel));
		info.AddProperty(PropertyName._previewValueLabel, Variant.From(in _previewValueLabel));
		info.AddProperty(PropertyName._previewAudioLabel, Variant.From(in _previewAudioLabel));
		info.AddProperty(PropertyName._previewSceneLabel, Variant.From(in _previewSceneLabel));
		info.AddProperty(PropertyName._kindSummaryLabel, Variant.From(in _kindSummaryLabel));
		info.AddProperty(PropertyName._texturePreview, Variant.From(in _texturePreview));
		info.AddProperty(PropertyName._collectablePreviewViewport, Variant.From(in _collectablePreviewViewport));
		info.AddProperty(PropertyName._collectablePreviewRoot, Variant.From(in _collectablePreviewRoot));
		info.AddProperty(PropertyName._collectableRuntimeStatusLabel, Variant.From(in _collectableRuntimeStatusLabel));
		info.AddProperty(PropertyName._collectableRuntimeRunButton, Variant.From(in _collectableRuntimeRunButton));
		info.AddProperty(PropertyName._collectableRuntimeRunning, Variant.From(in _collectableRuntimeRunning));
		info.AddProperty(PropertyName._collectableKindCards, Variant.From(in _collectableKindCards));
		info.AddProperty(PropertyName._configPropertyList, Variant.From(in _configPropertyList));
		info.AddProperty(PropertyName._unlockConditionList, Variant.From(in _unlockConditionList));
		info.AddProperty(PropertyName._eventList, Variant.From(in _eventList));
		info.AddProperty(PropertyName._shovelableNameList, Variant.From(in _shovelableNameList));
		info.AddProperty(PropertyName._updatingControls, Variant.From(in _updatingControls));
		info.AddProperty(PropertyName._collectableSaveTimer, Variant.From(in _collectableSaveTimer));
		info.AddProperty(PropertyName._pendingCollectableSaveMessage, Variant.From(in _pendingCollectableSaveMessage));
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void RestoreGodotObjectData(GodotSerializationInfo info)
	{
		base.RestoreGodotObjectData(info);
		if (info.TryGetProperty(PropertyName._editingCollectable, out var value))
		{
			_editingCollectable = value.As<CollectableConfig>();
		}
		if (info.TryGetProperty(PropertyName._editingAwardSettlement, out var value2))
		{
			_editingAwardSettlement = value2.As<AwardSettlementConfig>();
		}
		if (info.TryGetProperty(PropertyName._awardBackgroundPreview, out var value3))
		{
			_awardBackgroundPreview = value3.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._awardImagePreview, out var value4))
		{
			_awardImagePreview = value4.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._awardTexturePreview, out var value5))
		{
			_awardTexturePreview = value5.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._awardEmptyHint, out var value6))
		{
			_awardEmptyHint = value6.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._awardStatusLabel, out var value7))
		{
			_awardStatusLabel = value7.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._awardTitleLabel, out var value8))
		{
			_awardTitleLabel = value8.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewTitleLabel, out var value9))
		{
			_previewTitleLabel = value9.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewConfigLabel, out var value10))
		{
			_previewConfigLabel = value10.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewValueLabel, out var value11))
		{
			_previewValueLabel = value11.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewAudioLabel, out var value12))
		{
			_previewAudioLabel = value12.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._previewSceneLabel, out var value13))
		{
			_previewSceneLabel = value13.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._kindSummaryLabel, out var value14))
		{
			_kindSummaryLabel = value14.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._texturePreview, out var value15))
		{
			_texturePreview = value15.As<TextureRect>();
		}
		if (info.TryGetProperty(PropertyName._collectablePreviewViewport, out var value16))
		{
			_collectablePreviewViewport = value16.As<SubViewport>();
		}
		if (info.TryGetProperty(PropertyName._collectablePreviewRoot, out var value17))
		{
			_collectablePreviewRoot = value17.As<Node2D>();
		}
		if (info.TryGetProperty(PropertyName._collectableRuntimeStatusLabel, out var value18))
		{
			_collectableRuntimeStatusLabel = value18.As<Label>();
		}
		if (info.TryGetProperty(PropertyName._collectableRuntimeRunButton, out var value19))
		{
			_collectableRuntimeRunButton = value19.As<Button>();
		}
		if (info.TryGetProperty(PropertyName._collectableRuntimeRunning, out var value20))
		{
			_collectableRuntimeRunning = value20.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._collectableKindCards, out var value21))
		{
			_collectableKindCards = value21.As<HFlowContainer>();
		}
		if (info.TryGetProperty(PropertyName._configPropertyList, out var value22))
		{
			_configPropertyList = value22.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._unlockConditionList, out var value23))
		{
			_unlockConditionList = value23.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._eventList, out var value24))
		{
			_eventList = value24.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._shovelableNameList, out var value25))
		{
			_shovelableNameList = value25.As<ItemList>();
		}
		if (info.TryGetProperty(PropertyName._updatingControls, out var value26))
		{
			_updatingControls = value26.As<bool>();
		}
		if (info.TryGetProperty(PropertyName._collectableSaveTimer, out var value27))
		{
			_collectableSaveTimer = value27.As<Timer>();
		}
		if (info.TryGetProperty(PropertyName._pendingCollectableSaveMessage, out var value28))
		{
			_pendingCollectableSaveMessage = value28.As<string>();
		}
	}
}
